/* ==================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using UnityEngine;

namespace AuroraFPSRuntime
{
    [System.Serializable]
    public class FreeCamera
    {
        // Camera properties.
        [SerializeField] private Transform cameraInstance;
        [SerializeField] private Transform target;
        [SerializeField] private float rotationSmooth = 7.0f;
        [SerializeField] private LayerMask cullingLayer = 1 << 0;
        [SerializeField] private float cullingSmooth = 10.0f;
        [SerializeField] private float rightOffset = 0.0f;
        [SerializeField] private float defaultDistance = 2.5f;
        [SerializeField] private float minDistance = 0.7f;
        [SerializeField] private float maxDistance = 3.5f;
        [SerializeField] private float height = 1.4f;
        [SerializeField] private float followSpeed = 20.0f;
        [SerializeField] private float xMouseSensitivity = 3.0f;
        [SerializeField] private float yMouseSensitivity = 3.0f;
        [SerializeField] private float yMinLimit = -40.0f;
        [SerializeField] private float yMaxLimit = 80.0f;

        // Stored required components.
        private MonoBehaviour monoBehaviour;
        private Camera cameraComponent;

        // Stored required properties.
        private Transform targetLookAt;
        private Vector2 movementSpeed;
        private Vector3 currentTargetPos;
        private Vector3 currentPos;
        private Vector3 desiredPos;
        private float distance;
        private float mouseY = 0f;
        private float mouseX = 0f;
        private float currentHeight;
        private float cullingDistance;
        private float checkHeightRadius = 0.4f;
        private float clipPlaneMargin = 0f;
        private float forward = -1f;
        private float xMinLimit = -360f;
        private float xMaxLimit = 360f;
        private float cullingHeight = 0.2f;
        private float cullingMinDist = 0.1f;

        /// <summary>
        /// Initialize DeathCamera instance.
        /// </summary>
        public void Initialize()
        {
            cameraComponent = cameraInstance.GetComponent<Camera>();

            currentTargetPos = new Vector3(target.position.x, target.position.y, target.position.z);

            targetLookAt = new GameObject("TargetLookAt").transform;
            targetLookAt.position = target.position;
            targetLookAt.hideFlags = HideFlags.HideInHierarchy;
            targetLookAt.rotation = target.rotation;

            mouseY = target.eulerAngles.x;
            mouseX = target.eulerAngles.y;

            distance = defaultDistance;
            currentHeight = height;
        }

        /// <summary>
        /// Camera look processing.
        /// </summary>
        public virtual void CameraLook()
        {
            if (target == null || targetLookAt == null)
            {
                return;
            }

            CameraMovement();
            CameraHandle();
        }

        /// <summary>
        /// Read input and rotate camera by input values.
        /// </summary>
        protected virtual void CameraHandle()
        {
            float vertical = AInput.GetAxis(INC.CamVertical);
            float horizontal = AInput.GetAxis(INC.CamHorizontal);

            RotateCamera(horizontal, vertical);
        }

        /// <summary>
        /// Set the target for the camera.
        /// </summary>
        public void UpdateTarget(Transform newTarget)
        {
            target = newTarget ?? target;
        }

        /// <summary>
        /// Set the target for the camera and reset rotation and position by new target.
        /// </summary>
        public void CompleteUpdateTarget(Transform newTarget)
        {
            target = newTarget;
            mouseY = target.rotation.eulerAngles.x;
            mouseX = target.rotation.eulerAngles.y;
            Initialize();
        }

        /// <summary>    
        /// Convert a point in the screen in a Ray for the world.
        /// </summary>
        public Ray ScreenPointToRay(Vector3 Point)
        {
            return cameraComponent.ScreenPointToRay(Point);
        }

        /// <summary>
        /// Camera Rotation processing.
        /// </summary>
        public void RotateCamera(float x, float y)
        {
            // Free rotation.
            mouseX += x * xMouseSensitivity;
            mouseY -= y * yMouseSensitivity;

            movementSpeed.x = x;
            movementSpeed.y = -y;
            mouseY = AMath.ClampAngle(mouseY, yMinLimit, yMaxLimit);
            mouseX = AMath.ClampAngle(mouseX, xMinLimit, xMaxLimit);
        }

        /// <summary>
        /// Camera movement processing.
        /// </summary>    
        protected virtual void CameraMovement()
        {
            if (target == null)
            {
                return;
            }

            distance = Mathf.Lerp(distance, defaultDistance, cullingSmooth * Time.deltaTime);
            cullingDistance = Mathf.Lerp(cullingDistance, distance, Time.deltaTime);
            Vector3 camDir = (forward * targetLookAt.forward) + (rightOffset * targetLookAt.right);

            camDir = camDir.normalized;

            Vector3 targetPos = new Vector3(target.position.x, target.position.y, target.position.z);
            currentTargetPos = Vector3.Lerp(currentTargetPos, targetPos, Time.deltaTime * followSpeed);
            desiredPos = targetPos + new Vector3(0, height, 0);
            currentPos = currentTargetPos + new Vector3(0, currentHeight, 0);
            RaycastHit hitInfo;

            ClipPlanePoints planePoints = cameraComponent.NearClipPlanePoints(currentPos + (camDir * (distance)), clipPlaneMargin);
            ClipPlanePoints oldPoints = cameraComponent.NearClipPlanePoints(desiredPos + (camDir * distance), clipPlaneMargin);

            //Check if Height is not blocked.
            if (Physics.SphereCast(targetPos, checkHeightRadius, Vector3.up, out hitInfo, cullingHeight + 0.2f, cullingLayer))
            {
                float t = hitInfo.distance - 0.2f;
                t -= height;
                t /= (cullingHeight - height);
                cullingHeight = Mathf.Lerp(height, cullingHeight, Mathf.Clamp(t, 0.0f, 1.0f));
            }

            //Check if desired target position is not blocked.
            if (AMath.CullingRayCast(desiredPos, oldPoints, out hitInfo, distance + 0.2f, cullingLayer, Color.blue))
            {
                cullingDistance = hitInfo.distance;
                distance = hitInfo.distance - 0.2f;
                if (distance < defaultDistance)
                {
                    float t = hitInfo.distance;
                    t -= cullingMinDist;
                    t /= cullingMinDist;
                    currentHeight = Mathf.Lerp(cullingHeight, height, Mathf.Clamp(t, 0.0f, 1.0f));
                    currentPos = currentTargetPos + new Vector3(0, currentHeight, 0);
                }
            }
            else
            {
                currentHeight = height;
            }
            //Check if target position with culling height applied is not blocked.
            if (AMath.CullingRayCast(currentPos, planePoints, out hitInfo, distance, cullingLayer, Color.cyan))
            {
                cullingDistance = hitInfo.distance;
                distance = Mathf.Clamp(cullingDistance, 0.0f, defaultDistance);
            }
            Vector3 lookPoint = currentPos + targetLookAt.forward * 2f;
            lookPoint += (targetLookAt.right * Vector3.Dot(camDir * (distance), targetLookAt.right));
            targetLookAt.position = currentPos;

            Quaternion newRot = Quaternion.Euler(mouseY, mouseX, 0);
            targetLookAt.rotation = newRot;

            Vector3 tCameraPosition = currentPos + (camDir * (distance));
            cameraInstance.position = Vector3.Lerp(cameraInstance.position, tCameraPosition, rotationSmooth * Time.deltaTime);

            Quaternion tCameraRotation = Quaternion.LookRotation((lookPoint) - cameraInstance.position);
            cameraInstance.rotation = Quaternion.Slerp(cameraInstance.rotation, tCameraRotation, rotationSmooth * Time.deltaTime);

            movementSpeed = Vector2.zero;
        }

        #region [Getter / Setter]
        public Transform GetTarget()
        {
            return target;
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        public float GetSmooth()
        {
            return rotationSmooth;
        }

        public void SetSmooth(float value)
        {
            rotationSmooth = value;
        }

        public LayerMask GetCullingLayer()
        {
            return cullingLayer;
        }

        public void SetCullingLayer(LayerMask value)
        {
            cullingLayer = value;
        }

        public float GetCullingSmooth()
        {
            return cullingSmooth;
        }

        public void SetCullingSmooth(float value)
        {
            cullingSmooth = value;
        }

        public float GetRightOffset()
        {
            return rightOffset;
        }

        public void SetRightOffset(float value)
        {
            rightOffset = value;
        }

        public float GetDefaultDistance()
        {
            return defaultDistance;
        }

        public void SetDefaultDistance(float value)
        {
            defaultDistance = value;
        }

        public float GetMinDistance()
        {
            return minDistance;
        }

        public void SetMinDistance(float value)
        {
            minDistance = value;
        }

        public float GetMaxDistance()
        {
            return maxDistance;
        }

        public void SetMaxDistance(float value)
        {
            maxDistance = value;
        }

        public float GetHeight()
        {
            return height;
        }

        public void SetHeight(float value)
        {
            height = value;
        }

        public float GetFollowSpeed()
        {
            return followSpeed;
        }

        public void SetFollowSpeed(float value)
        {
            followSpeed = value;
        }

        public float GetXMouseSensitivity()
        {
            return xMouseSensitivity;
        }

        public void SetXMouseSensitivity(float value)
        {
            xMouseSensitivity = value;
        }

        public float GetYMouseSensitivity()
        {
            return yMouseSensitivity;
        }

        public void SetYMouseSensitivity(float value)
        {
            yMouseSensitivity = value;
        }

        public float GetYMinLimit()
        {
            return yMinLimit;
        }

        public void SetYMinLimit(float value)
        {
            yMinLimit = value;
        }

        public float GetYMaxLimit()
        {
            return yMaxLimit;
        }

        public void SetYMaxLimit(float value)
        {
            yMaxLimit = value;
        }

        public MonoBehaviour GetHealth()
        {
            return monoBehaviour;
        }

        protected void SetHealth(MonoBehaviour value)
        {
            monoBehaviour = value;
        }

        public Transform GetCameraTransform()
        {
            return cameraInstance;
        }

        public void SetCameraTransform(Transform value)
        {
            cameraInstance = value;
        }

        public Camera GetCameraComponent()
        {
            return cameraComponent;
        }

        protected void SetCameraComponent(Camera value)
        {
            cameraComponent = value;
        }
        #endregion
    }
}