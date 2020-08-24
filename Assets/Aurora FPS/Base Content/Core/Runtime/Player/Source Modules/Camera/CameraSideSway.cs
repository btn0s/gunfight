/* ================================================================
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
    public class CameraSideSway
    {
        // Base camera side sway properties.
        [SerializeField] private float swayAmount = 2;
        [SerializeField] private float swaySpeed = 3;
        [SerializeField] private float returnSpeed = 5;

        // Stored required properties.
        private Transform pivot;

        /// <summary>
        /// Initialize camera side sway system.
        /// </summary>
        /// <param name="pivot">Camera transform pivot.</param>
        public virtual void Initialize(Transform pivot)
        {
            this.pivot = pivot;
        }

        /// <summary>
        /// Calculate camera side sway.
        /// </summary>
        public virtual void CalculateSway(float horizontalInput)
        {
            Vector3 targetRotation = pivot.eulerAngles;
            targetRotation.z = (horizontalInput != 0 ? -Mathf.Sign(horizontalInput) : 0) * swayAmount;
            pivot.rotation = Quaternion.Slerp(pivot.rotation, Quaternion.Euler(targetRotation), (horizontalInput != 0 ? swaySpeed : returnSpeed) * Time.deltaTime);
        }

        #region [Getter / Setter]
        public float GetSwayAmount()
        {
            return swayAmount;
        }

        public void SetSwayAmount(float value)
        {
            swayAmount = value;
        }

        public float GetSwaySpeed()
        {
            return swaySpeed;
        }

        public void SetSwaySpeed(float value)
        {
            swaySpeed = value;
        }

        public float GetReturnSpeed()
        {
            return returnSpeed;
        }

        public void SetReturnSpeed(float value)
        {
            returnSpeed = value;
        }
        #endregion
    }
}