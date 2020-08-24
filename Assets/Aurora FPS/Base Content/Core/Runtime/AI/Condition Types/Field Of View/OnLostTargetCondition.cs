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
    [ConditionOptions(path = "Field Of View/On Lost Target", description = "Called when the AI loses all targets from view.")]
    public class OnLostTargetCondition : Condition
    {
        private bool isExecuted;
        private AIFieldOfView fieldOfView;

        /// <summary>
        /// Initializing required properties.
        /// </summary>
        /// <param name="core">Target AICore instance.</param>
        public override void Initialize(AICore core)
        {
            base.Initialize(core);
            fieldOfView = core.GetComponent<AIFieldOfView>();

            fieldOfView.OnLostTargetsCallback += () => isExecuted = true;

            fieldOfView.OnFindTargetsCallback += () => isExecuted = false;
        }

        /// <summary>
        /// Condition for translate to the next AI behaviour.
        /// </summary>
        public override bool IsExecuted()
        {
            return isExecuted;
        }
    }
}