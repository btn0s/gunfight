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
    [ConditionOptions(path = "Field Of View/On Find Target", description = "Called when any target is in view.")]
    public class OnFindTargetCondition : Condition
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

            fieldOfView.OnFindTargetsCallback += () => isExecuted = true;

            fieldOfView.OnLostTargetsCallback += () => isExecuted = false;
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