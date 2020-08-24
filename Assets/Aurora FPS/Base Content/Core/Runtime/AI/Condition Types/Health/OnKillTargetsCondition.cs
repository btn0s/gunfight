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
    [RequireComponent(typeof(AIFieldOfView))]
    [ConditionOptions(path = "Health/OnKillTargets")]
    public class OnKillTargetsCondition : Condition
    {
        // Stored required components.
        private AIFieldOfView fieldOfView;

        /// <summary>
        /// Initializing required properties.
        /// </summary>
        /// <param name="core">Target AICore instance.</param>
        public override void Initialize(AICore core)
        {
            base.Initialize(core);
            fieldOfView = core.GetComponent<AIFieldOfView>();
        }

        /// <summary>
        /// Condition for translate to the next AI behaviour.
        /// </summary>
        public override bool IsExecuted()
        {
            for (int i = 0; i < fieldOfView.GetVisibleTargetCount(); i++)
            {
                IHealth health = fieldOfView.GetVisibleTarget(i).GetComponent<IHealth>();
                if(health != null && health.IsAlive())
                {
                    return false;
                }
            }
            return true;
        }
    }
}