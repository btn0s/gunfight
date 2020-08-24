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
    public sealed class WeaponSwaySystem
    {
        [SerializeField] private float amount;
        [SerializeField] private float maxAmount;
        [SerializeField] private float speed;

        public Vector3 GetSwayPosition(Vector3 currentPosition, Vector3 originalPosition)
        {
            float vertical = AInput.GetAxis(INC.CamVertical) * amount;
            float horizontal = AInput.GetAxis(INC.CamHorizontal) * amount;

            vertical = Mathf.Clamp(vertical, -maxAmount, maxAmount);
            horizontal = Mathf.Clamp(horizontal, -maxAmount, maxAmount);

            Vector3 desiredPosition = new Vector3(originalPosition.x - horizontal, originalPosition.y - vertical, originalPosition.z);
            return Vector3.Lerp(currentPosition, desiredPosition, speed * Time.deltaTime);
        }

        #region [Getter / Setter]
        public float GetAmount()
        {
            return amount;
        }

        public void SetAmount(float value)
        {
            amount = value;
        }

        public float GetMaxAmount()
        {
            return maxAmount;
        }

        public void SetMaxAmount(float value)
        {
            maxAmount = value;
        }

        public float GetSpeed()
        {
            return speed;
        }

        public void SetSpeed(float value)
        {
            speed = value;
        }
        #endregion
    }
}