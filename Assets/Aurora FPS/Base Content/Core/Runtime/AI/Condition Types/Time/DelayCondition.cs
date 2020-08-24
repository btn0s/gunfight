/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using UnityEngine;

namespace AuroraFPSRuntime.AI
{
    [ConditionOptions(path = "Time/Delay", description = "Return true after specific delay time.")]
    public class DelayCondition : Condition
    {
        // Base dealy condition properties.
        [SerializeField] private float delay;

        // Stored required properties.
        private float startTime;

        /// <summary>
        /// Called when the behaviour in which this condition is located, becomes enabled.
        /// </summary>
        public override void OnEnable()
        {
            startTime = Time.time;
        }

        /// <summary>
        /// Condition for translate to the next AI behaviour.
        /// </summary>
        public override bool IsExecuted()
        {
            return Time.time - startTime >= delay;
        }

        /// <summary>
        /// Called when the behaviour in which this condition is located, switch to another.
        /// </summary>
        public override void OnDisable()
        {
            startTime = 0;
        }

        #region [Getter / Setter]
        public float GetDealy()
        {
            return delay;
        }

        public void SetDelay(float value)
        {
            delay = value;
        }
        #endregion
    }
}