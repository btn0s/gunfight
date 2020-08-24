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
    public struct FootstepInterval
    {
        [SerializeField] private float rate;
        [SerializeField] private float minVelocity;
        [SerializeField] private float maxVelocity;

        public FootstepInterval(float rate, float minVelocity, float maxVelocity)
        {
            this.minVelocity = minVelocity;
            this.maxVelocity = maxVelocity;
            this.rate = rate;
        }

        public readonly static FootstepInterval[] Default = new FootstepInterval[4] { new FootstepInterval(1.5f, 1.5f, 3.5f), new FootstepInterval(2.5f, 1.5f, 3.0f), new FootstepInterval(6.5f, 1.5f, 2.0f), new FootstepInterval(7.5f, 1.5f, 1.5f) };

        #region [Getter / Setter]
        public float GetRate()
        {
            return rate;
        }

        public void SetRate(float value)
        {
            rate = value;
        }
        public float GetMinVelocity()
        {
            return minVelocity;
        }

        public void SetMinVelocity(float value)
        {
            minVelocity = value;
        }

        public float GetMaxVelocity()
        {
            return maxVelocity;
        }

        public void SetMaxVelocity(float value)
        {
            maxVelocity = value;
        }

        #endregion
    }

}