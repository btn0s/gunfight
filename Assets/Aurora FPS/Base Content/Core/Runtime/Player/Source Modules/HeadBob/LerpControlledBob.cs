/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using System;
using System.Collections;
using UnityEngine;

namespace AuroraFPSRuntime
{
    [Serializable]
    public class LerpControlledBob
    {
        [SerializeField] private float bobDuration = 0.1f;
        [SerializeField] private float bobAmount = 0.05f;

        private float offset;

        /// <summary>
        /// Bob cycle processing.
        /// </summary>
        public virtual IEnumerator PlayBobCycle()
        {
            float t = 0f;
            while (t < bobDuration)
            {
                offset = Mathf.Lerp(0f, bobAmount, t / bobDuration);
                t += Time.deltaTime;
                yield return null;
            }

            t = 0f;
            while (t < bobDuration)
            {
                offset = Mathf.Lerp(bobAmount, 0f, t / bobDuration);
                t += Time.deltaTime;
                yield return null;
            }
            offset = 0f;
            yield break;
        }

        #region [Getter / Setter]
        public float GetBobDuration()
        {
            return bobDuration;
        }

        public void SetBobDuration(float value)
        {
            bobDuration = value;
        }

        public float GetBobAmount()
        {
            return bobAmount;
        }

        public void SetBobAmount(float value)
        {
            bobAmount = value;
        }

        public float GetOffset()
        {
            return offset;
        }
        #endregion
    }
}