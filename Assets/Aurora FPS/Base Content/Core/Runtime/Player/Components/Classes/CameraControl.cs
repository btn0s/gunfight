/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AuroraFPSRuntime
{
    [System.Serializable]
    public class CameraControl
    {
        // Default first person camera control properties.
        [Header("Base Settings")]
        [SerializeField] private Transform instanceTransform;
        [SerializeField] private Transform pivotTransform;
        [SerializeField] private Vector2 sensitivity = new Vector2(125, 125);
        [SerializeField] private Vector2 rotationSmooth = new Vector2(20, 20);
        [SerializeField] private float positionSmooth = 12.5f;
        [SerializeField] private RangedFloat verticalRotationLimits = new RangedFloat(-90, 90, -180, 180);
        [SerializeField] private bool clampVerticalRotation = true;

        // Zoom properties.
        [Header("Zoom Settings")]
        [SerializeField] private InputHandleType zoomHandleType = InputHandleType.Hold;
        [SerializeField] private FOVAnimationSettings zoomSettings = new FOVAnimationSettings(50.0f, 0.25f, AnimationCurve.Linear(0, 0, 1, 1));

        //Swaying properties.
        [Header("Swaying Settings")]
        [SerializeField] private CameraSideSway sideSway;
        [SerializeField] private bool useSideSway = true;

        [Header("Other Settings")]
        [SerializeField] private FOVAnimationSettings defaultFOVSettings = new FOVAnimationSettings(75.0f, 0.25f, AnimationCurve.Linear(0, 0, 1, 1));

        // Stored required components.
        private FPController controller;
        private Camera cameraComponent;

        // Stored required properties.
        private Vector2 input;
        private Vector2 desiredVector;
        private Vector3 originalPosition;
        private Vector3 desiredPosition;
        private Vector3 smoothPosition;
        private Quaternion xDesiredRotation;
        private Quaternion yDesiredRotation;
        private Quaternion xSmoothRotation;
        private Quaternion ySmoothRotation;

        // Stored recoil properties.
        private RecoilPattern storedRecoilPattern;
        private Vector2 recoilAmount;
        private Vector2 recoilReturnAmount;
        private Vector2 smoothRecoilAmount;
        private Vector2 recoilElapsedAmount;
        private bool returnRecoil;

        //Stored zoom properties.
        private bool isZooming;
        private CoroutineObject<FOVAnimationSettings> changeFOVCoroutine;

        /// <summary>
        /// Initialize CameraControl instance.
        /// </summary>
        public virtual void Initialize(FPController controller)
        {
            this.controller = controller;
            cameraComponent = instanceTransform.GetComponent<Camera>();
            changeFOVCoroutine = new CoroutineObject<FOVAnimationSettings>(controller);

            originalPosition = instanceTransform.localPosition;

            xDesiredRotation = controller.transform.rotation;
            yDesiredRotation = instanceTransform.localRotation;

            desiredVector = new Vector2(xDesiredRotation.eulerAngles.y, yDesiredRotation.eulerAngles.x);

            xSmoothRotation = xDesiredRotation;
            ySmoothRotation = yDesiredRotation;

            sideSway.Initialize(pivotTransform);
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        public virtual void Update()
        {
            ReadInput();
            CalculatePosition();
            CalculateRotation();
            CalculateRecoil();
            ClampRotation();
            CalculateQuaternion();
            SmoothPosition();
            SmoothingRotation();
            ApplyPosition();
            ApplyRotation();

            ZoomHandler();
            SwayingHandler();
        }

        /// <summary>
        /// Read required input values before calculation rotation.
        /// </summary>
        protected virtual void ReadInput()
        {
            input.x = AInput.GetAxis(INC.CamHorizontal);
            input.y = AInput.GetAxis(INC.CamVertical);
        }

        /// <summary>
        /// Calculate camera rotation.
        /// </summary>
        protected virtual void CalculateRotation()
        {
            desiredVector.x += input.x * sensitivity.x * Time.deltaTime;
            desiredVector.y += input.y * sensitivity.y * Time.deltaTime;
        }

        /// <summary>
        /// Calculate camera position.
        /// </summary>
        protected virtual void CalculatePosition()
        {
            desiredPosition = originalPosition;
        }

        /// <summary>
        /// Calculate recoil amount values and apply to the camera rotation.
        /// </summary>
        protected virtual void CalculateRecoil()
        {
            if (recoilAmount.y != 0)
            {
                if (input.y < 0 && recoilAmount.y > 0)
                {
                    input.y *= storedRecoilPattern.GetImpulseInputCompensate().y;
                    recoilAmount.y += input.y;
                }
                recoilAmount.y = AMath.Clamp(recoilAmount.y, storedRecoilPattern.GetClampVerticalRecoil());

                if (recoilElapsedAmount.y > recoilAmount.y)
                {
                    recoilAmount.y = recoilElapsedAmount.y;
                }

                if (recoilElapsedAmount.y < recoilAmount.y)
                {
                    smoothRecoilAmount.y = Mathf.Lerp(smoothRecoilAmount.y, recoilAmount.y, storedRecoilPattern.GetApplyImpulseSpeed().y * Time.deltaTime);
                    recoilElapsedAmount.y += smoothRecoilAmount.y;
                    desiredVector.y += smoothRecoilAmount.y;
                    if (recoilElapsedAmount.y > recoilAmount.y)
                    {
                        recoilElapsedAmount.y = recoilAmount.y;
                        smoothRecoilAmount.y = 0;
                    }

                    if (returnRecoil)
                    {
                        recoilAmount.y = recoilElapsedAmount.y;
                        smoothRecoilAmount.y = 0;
                    }
                }
                else if (returnRecoil && storedRecoilPattern.AutoReturn() && recoilElapsedAmount.y == recoilAmount.y && recoilReturnAmount.y < recoilElapsedAmount.y)
                {
                    smoothRecoilAmount.y = Mathf.Lerp(smoothRecoilAmount.y, recoilElapsedAmount.y, storedRecoilPattern.GetReturnImpulseSpeed().y * Time.deltaTime);
                    desiredVector.y -= smoothRecoilAmount.y;
                    recoilReturnAmount.y += smoothRecoilAmount.y;
                    if (recoilReturnAmount.y > recoilElapsedAmount.y)
                    {
                        recoilAmount.y = 0;
                        smoothRecoilAmount.y = 0;
                        recoilElapsedAmount.y = 0;
                        recoilReturnAmount.y = 0;
                        returnRecoil = false;
                    }
                }
                else if (returnRecoil && !storedRecoilPattern.AutoReturn() && recoilElapsedAmount.y == recoilAmount.y && recoilReturnAmount.y < recoilElapsedAmount.y)
                {
                    recoilAmount.y = 0;
                    smoothRecoilAmount.y = 0;
                    recoilElapsedAmount.y = 0;
                    recoilReturnAmount.y = 0;
                    returnRecoil = false;
                }
            }

            if (recoilAmount.x != 0)
            {
                if (input.x < 0 && recoilAmount.x > 0)
                {
                    input.x *= storedRecoilPattern.GetImpulseInputCompensate().x;
                    recoilAmount.x -= input.x;
                }
                recoilAmount.x = AMath.Clamp(recoilAmount.x, storedRecoilPattern.GetClampHorizontalRecoil());

                if (recoilElapsedAmount.x > recoilAmount.x)
                {
                    recoilAmount.x = recoilElapsedAmount.x;
                }

                if (recoilElapsedAmount.x < recoilAmount.x)
                {
                    smoothRecoilAmount.x = Mathf.Lerp(smoothRecoilAmount.x, recoilAmount.x, storedRecoilPattern.GetApplyImpulseSpeed().x * Time.deltaTime);
                    recoilElapsedAmount.x += smoothRecoilAmount.x;
                    desiredVector.x += smoothRecoilAmount.x;
                    if (recoilElapsedAmount.x > recoilAmount.x)
                    {
                        recoilElapsedAmount.x = recoilAmount.x;
                        smoothRecoilAmount.x = 0;
                    }
                }
                else if (storedRecoilPattern.AutoReturn() && recoilElapsedAmount.x == recoilAmount.x && recoilReturnAmount.x < recoilElapsedAmount.x)
                {
                    smoothRecoilAmount.x = Mathf.Lerp(smoothRecoilAmount.x, recoilElapsedAmount.x, storedRecoilPattern.GetReturnImpulseSpeed().x * Time.deltaTime);
                    desiredVector.x -= smoothRecoilAmount.x;
                    recoilReturnAmount.x += smoothRecoilAmount.x;
                    if (recoilReturnAmount.x > recoilElapsedAmount.x)
                    {
                        recoilAmount.x = 0;
                        smoothRecoilAmount.x = 0;
                        recoilElapsedAmount.x = 0;
                        recoilReturnAmount.x = 0;
                    }
                }
                else if (returnRecoil && !storedRecoilPattern.AutoReturn() && recoilElapsedAmount.x == recoilAmount.x && recoilReturnAmount.x < recoilElapsedAmount.x)
                {
                    recoilAmount.x = 0;
                    smoothRecoilAmount.x = 0;
                    recoilElapsedAmount.x = 0;
                    recoilReturnAmount.x = 0;
                    returnRecoil = false;
                }
            }
        }

        /// <summary>
        /// Clamp vertical rotation by specific limits.
        /// </summary>
        protected virtual void ClampRotation()
        {
            if (clampVerticalRotation)
                desiredVector.y = AMath.Clamp(desiredVector.y, verticalRotationLimits);
        }

        /// <summary>
        /// Calculate quaternion rotation by calculated vector rotation.
        /// </summary>
        protected virtual void CalculateQuaternion()
        {
            xDesiredRotation = Quaternion.AngleAxis(desiredVector.x, Vector3.up);
            yDesiredRotation = Quaternion.AngleAxis(desiredVector.y, -Vector3.right);
        }

        /// <summary>
        /// Smoothing calculated rotation.
        /// </summary>
        protected virtual void SmoothingRotation()
        {
            xSmoothRotation = Quaternion.Slerp(xSmoothRotation, xDesiredRotation, rotationSmooth.x * Time.deltaTime);
            ySmoothRotation = Quaternion.Slerp(ySmoothRotation, yDesiredRotation, rotationSmooth.y * Time.deltaTime);
        }

        /// <summary>
        /// Smoothing calculated position.
        /// </summary>
        protected virtual void SmoothPosition()
        {
            smoothPosition = Vector3.Lerp(smoothPosition, desiredPosition, positionSmooth * Time.deltaTime);
        }

        /// <summary>
        /// Applying calculated and smoothed rotation to the camera transform.
        /// </summary>
        protected virtual void ApplyRotation()
        {
            controller.transform.rotation = xSmoothRotation;
            instanceTransform.localRotation = ySmoothRotation;
        }

        /// <summary>
        /// Applying calculated and smoothed rotation to the camera transform.
        /// </summary>
        protected virtual void ApplyPosition()
        {
            instanceTransform.localPosition = smoothPosition;
        }

        /// <summary>
        /// Clamp horizontal rotation in specific range.
        /// </summary>
        /// <param name="min">Min angle.</param>
        /// <param name="max">Max angle.</param>
        public virtual void ClampHorizontalRotation(float min, float max)
        {
            desiredVector.x = Mathf.Clamp(desiredVector.x, min, max);
        }

        /// <summary>
        /// Clamp horizontal rotation in specific range.
        /// </summary>
        /// <param name="range">Angle range value.</param>
        public virtual void ClampHorizontalRotation(RangedFloat range)
        {
            desiredVector.x = AMath.Clamp(desiredVector.x, range);
        }

        /// <summary>
        /// Handling camera zoom state.
        /// </summary>
        protected virtual void ZoomHandler()
        {
            if (!isZooming && AInput.GetButtonDown(INC.Zoom))
            {
                ZoomIn();
            }
            else if (isZooming &&
                (AInput.GetButtonDown(INC.Zoom) && zoomHandleType == InputHandleType.Trigger) ||
                (AInput.GetButtonUp(INC.Zoom) && zoomHandleType == InputHandleType.Hold))
            {
                ZoomOut();
            }
        }

        /// <summary>
        /// Apply zoom field of view settings to the camera compoment.
        /// </summary>
        public void ZoomIn()
        {
            if (!isZooming)
            {
                OnZoomCallback?.Invoke(true, zoomSettings.GetFieldOfView());
            }
            isZooming = true;
            ChangeFieldOfView(zoomSettings, true);
        }

        /// <summary>
        /// Apply default field of view settings to the camera component.
        /// </summary>
        public void ZoomOut()
        {
            if (isZooming)
            {
                OnZoomCallback?.Invoke(false, defaultFOVSettings.GetFieldOfView());
            }
            isZooming = false;
            ChangeFieldOfView(defaultFOVSettings, true);
        }

        /// <summary>
        /// Add recoil to the camera.
        /// </summary>
        /// <param name="recoilPattern"></param>
        public virtual void AddRecoil(RecoilPattern recoilPattern)
        {
            storedRecoilPattern = recoilPattern;

            RangedFloat impulse = recoilPattern.GetVerticalRecoilImpulse();
            recoilAmount.y += recoilPattern.RandomVerticalCameraRecoil() ? Random.Range(impulse.GetMin(), impulse.GetMax()) : impulse.GetMax();

            impulse = recoilPattern.GetHorizontalRecoilImpulse();
            recoilAmount.x += recoilPattern.RandomHorizontalCameraRecoil() ? Random.Range(impulse.GetMin(), impulse.GetMax()) : impulse.GetMax();
        }

        /// <summary>
        /// Returning camera to default rotation. 
        /// Must be called after the end of shooting.
        /// </summary>
        public virtual void ReturnRecoil()
        {
            returnRecoil = true;
        }

        /// <summary>
        /// Processing camera side swaying.
        /// </summary>
        public virtual void SwayingHandler()
        {
            if (useSideSway)
                sideSway.CalculateSway(controller.GetHorizontalInput());
        }

        /// <summary>
        /// Change camera field of view value.
        /// </summary>
        /// <param name="settings">Settings to change current field of view.</param>
        /// <param name="force">
        /// True: If at the moment change field of view processed, terminate it, and apply new field of view settings.
        /// False: Terminate applying new field of view settings, if the other field of view settings are currently being applied.
        /// </param>
        public void ChangeFieldOfView(FOVAnimationSettings settings, bool force = false)
        {
            changeFOVCoroutine.Start(ChangeFOV, settings, force);
        }

        /// <summary>
        /// Increase camera field of view to target value.
        /// </summary>
        protected IEnumerator ChangeFOV(FOVAnimationSettings settings)
        {
            float time = 0.0f;
            float speed = 1.0f / settings.GetDuration();

            float currentFOV = cameraComponent.fieldOfView;
            float targetFOV = settings.GetFieldOfView();

            while (time < 1.0f)
            {
                time += Time.deltaTime * speed;
                float smoothTime = settings.EvaluateCurve(time);
                cameraComponent.fieldOfView = Mathf.Lerp(currentFOV, targetFOV, smoothTime);
                yield return null;
            }
        }

        #region [Event Callback Function]
        public event Action<bool, float> OnZoomCallback;
        #endregion

        #region [Getter / Setter]
        public Transform GetInstanceTransform()
        {
            return instanceTransform;
        }

        public void SetInstanceTransform(Transform value)
        {
            instanceTransform = value;
        }

        public Transform GetPivotTransform()
        {
            return pivotTransform;
        }

        public void SetPivotTransform(Transform value)
        {
            pivotTransform = value;
        }

        public Vector2 GetSensitivity()
        {
            return sensitivity;
        }

        public void SetSensitivity(Vector2 value)
        {
            sensitivity = value;
        }

        public Vector2 GetRotationSmooth()
        {
            return rotationSmooth;
        }

        public void SetRotationSmooth(Vector2 value)
        {
            rotationSmooth = value;
        }

        public RangedFloat GetVerticalRotationLimits()
        {
            return verticalRotationLimits;
        }

        public void SetVerticalRotationLimits(RangedFloat value)
        {
            verticalRotationLimits = value;
        }

        public bool ClampVerticalRotation()
        {
            return clampVerticalRotation;
        }

        public void ClampVerticalRotation(bool value)
        {
            clampVerticalRotation = value;
        }

        public InputHandleType GetZoomHandleType()
        {
            return zoomHandleType;
        }

        public void SetZoomHandleType(InputHandleType value)
        {
            zoomHandleType = value;
        }

        public FOVAnimationSettings GetZoomSettings()
        {
            return zoomSettings;
        }

        public void SetZoomSettings(FOVAnimationSettings value)
        {
            zoomSettings = value;
        }

        public CameraSideSway GetSideSway()
        {
            return sideSway;
        }

        public void SetSideSway(CameraSideSway value)
        {
            sideSway = value;
        }

        public bool UseSideSway()
        {
            return useSideSway;
        }

        public void UseSideSway(bool value)
        {
            useSideSway = value;
        }

        public FOVAnimationSettings GetDefaultFOVSettings()
        {
            return defaultFOVSettings;
        }

        public void SetDefaultFOVSettings(FOVAnimationSettings value)
        {
            defaultFOVSettings = value;
        }

        public FPController GetController()
        {
            return controller;
        }

        protected void SetController(FPController value)
        {
            controller = value;
        }

        public Camera GetCameraComponent()
        {
            return cameraComponent;
        }

        protected void SetCameraComponent(Camera value)
        {
            cameraComponent = value;
        }

        public Vector2 GetInput()
        {
            return input;
        }

        protected void SetInput(Vector2 value)
        {
            input = value;
        }

        public bool IsZooming()
        {
            return isZooming;
        }

        protected void IsZooming(bool value)
        {
            isZooming = value;
        }
        #endregion
    }
}