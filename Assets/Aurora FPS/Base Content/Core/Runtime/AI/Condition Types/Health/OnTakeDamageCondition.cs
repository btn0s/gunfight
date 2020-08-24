/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using UnityEngine;
using System.Collections;

namespace AuroraFPSRuntime.AI
{
    [RequireComponent(typeof(AIHealth))]
    [ConditionOptions(path = "Health/On Take Damage", description = "Called every time when AI take damage.")]
    public class OnTakeDamageCondition : Condition
    {
        protected bool isExecuted;
        protected CharacterHealth health;
        protected WaitForEndOfFrame waitForEndOfFrame;

        /// <summary>
        /// Initializing required properties.
        /// </summary>
        /// <param name="core">Target AICore instance.</param>
        public override void Initialize(AICore core)
        {
            base.Initialize(core);
            health = core.GetComponent<CharacterHealth>();
            waitForEndOfFrame = new WaitForEndOfFrame();
            RegisterCallbackFunction();
        }

        /// <summary>
        /// Condition for translate to the next AI behaviour.
        /// </summary>
        public override bool IsExecuted()
        {
            return isExecuted;
        }

        /// <summary>
        /// Register health take damage callback function.
        /// </summary>
        protected virtual void RegisterCallbackFunction()
        {
            health.OnTakeDamageCallback += _ =>
            {
                InvokeDefaultCallback();
            };
        }
        
        /// <summary>
        /// Represents default actions to invoke on callback function.
        /// </summary>
        protected virtual void InvokeDefaultCallback()
        {
            isExecuted = true;
            core.StartCoroutine(ResetExecutedValue());
        }

        /// <summary>
        /// Called every time when AI take damage.
        /// Reset executed value on false after end of frame.
        /// </summary>
        protected virtual IEnumerator ResetExecutedValue()
        {
            yield return waitForEndOfFrame;
            isExecuted = false;
        }

        #region [Getter / Setter]
        public CharacterHealth GetHealth()
        {
            return health;
        }

        protected void SetHealth(CharacterHealth value)
        {
            health = value;
        }

        public WaitForEndOfFrame GetWaitForEndOfFrame()
        {
            return waitForEndOfFrame;
        }

        protected void SetWaitForEndOfFrame(WaitForEndOfFrame value)
        {
            waitForEndOfFrame = value;
        }
        #endregion
    }
}