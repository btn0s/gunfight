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
    [ConditionOptions(path = "Field Of View/On Find Target(Transform)", description = "Called when the target is in view.")]
    public class OnFindTargetTransformCondition : Condition
    {
        [SerializeField] private Transform target;

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
                if(fieldOfView.GetVisibleTarget(i) == target)
                {
                    return true;
                }
            }
            return false;
        }

        #region [Getter / Setter]
        public Transform GetTarget()
        {
            return target;
        }

        public void SetTarget(Transform value)
        {
            target = value;
        }
        #endregion
    }
}