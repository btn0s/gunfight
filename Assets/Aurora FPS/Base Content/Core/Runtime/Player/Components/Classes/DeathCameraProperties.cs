/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using UnityEngine;

namespace AuroraFPSRuntime
{
    [System.Serializable]
    public struct DeathCameraProperties
    {
        [SerializeField] private Vector3 position;
        [SerializeField] private Vector3 rotation;
        [SerializeField] private float positionSpeed;
        [SerializeField] private float rotationSpeed;

        public DeathCameraProperties(Vector3 position, Vector3 rotation, float positionSpeed, float rotationSpeed)
        {
            this.position = position;
            this.rotation = rotation;
            this.positionSpeed = positionSpeed;
            this.rotationSpeed = rotationSpeed;
        }

        public readonly static DeathCameraProperties Default = new DeathCameraProperties(new Vector3(0.0f, -0.5f, 0.0f), Vector3.zero, 5.0f, 5.0f);

        #region [Getter / Setter]
        public Vector3 GetPosition()
        {
            return position;
        }

        public void SetPosition(Vector3 value)
        {
            position = value;
        }

        public Vector3 GetRotation()
        {
            return rotation;
        }

        public void SetRotation(Vector3 value)
        {
            rotation = value;
        }

        public float GetPositionSpeed()
        {
            return positionSpeed;
        }

        public void SetPositionSpeed(float value)
        {
            positionSpeed = value;
        }

        public float GetRotationSpeed()
        {
            return rotationSpeed;
        }

        public void SetRotationSpeed(float value)
        {
            rotationSpeed = value;
        }
        #endregion
    }
}