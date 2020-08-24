/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using UnityEngine;

namespace AuroraFPSRuntime.UI
{
    [System.Serializable]
    public struct CrosshairSpread
    {
        [SerializeField] private float value;
        [SerializeField] private float speed;

        /// <summary>
        /// CrosshairSpread constructor.
        /// </summary>
        /// <param name="value">Spread value.</param>
        /// <param name="speed">Speed of step to calculate spread value.</param>
        public CrosshairSpread(float value, float speed)
        {
            this.value = value;
            this.speed = speed;
        }

        #region [Default Values]
        public readonly static CrosshairSpread none = new CrosshairSpread(0.0f, 0.0f);
        #endregion

        #region [Getter / Setter]
        public float GetValue()
        {
            return value;
        }

        public void SetValue(float value)
        {
            this.value = value;
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