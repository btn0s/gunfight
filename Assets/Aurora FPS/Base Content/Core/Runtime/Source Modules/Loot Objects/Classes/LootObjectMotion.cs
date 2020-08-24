/* ==================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================== */

using UnityEngine;

namespace AuroraFPSRuntime
{
    public class LootObjectMotion : MonoBehaviour
    {
        [SerializeField] private float range = 1.0f;
        [SerializeField] private float verticalSpeed = 1.0f;
        [SerializeField] private float offset = 1.0f;
        [SerializeField] private bool useMovement = true;
        [SerializeField] private float horizontalSpeed = 1.0f;
        [SerializeField] private Vector3 direction = Vector3.up;
        [SerializeField] private bool useRotation = true;

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void Update()
        {
            if (useMovement)
                transform.localPosition = new Vector3(transform.localPosition.x, Mathf.PingPong(Time.time * verticalSpeed, range) + offset, transform.localPosition.z);

            if (useRotation)
                transform.localRotation *= Quaternion.Euler(direction * horizontalSpeed);
        }

        #region [Getter / Setter]
        public float GetRange()
        {
            return range;
        }

        public void SetRange(float value)
        {
            range = value;
        }

        public float GetVerticalSpeed()
        {
            return verticalSpeed;
        }

        public void SetVerticalSpeed(float value)
        {
            verticalSpeed = value;
        }

        public float GetOffset()
        {
            return offset;
        }

        public void SetOffset(float value)
        {
            offset = value;
        }

        public bool UseMovement()
        {
            return useMovement;
        }

        public void UseMovement(bool value)
        {
            useMovement = value;
        }

        public float GetHorizontalSpeed()
        {
            return horizontalSpeed;
        }

        public void SetHorizontalSpeed(float value)
        {
            horizontalSpeed = value;
        }

        public Vector3 GetDirection()
        {
            return direction;
        }

        public void SetDirection(Vector3 value)
        {
            direction = value;
        }

        public bool UseRotation()
        {
            return useRotation;
        }

        public void UseRotation(bool value)
        {
            useRotation = value;
        }
        #endregion
    }
}