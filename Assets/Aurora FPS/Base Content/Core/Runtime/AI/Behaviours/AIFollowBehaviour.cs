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
    [AIBehaviourOptions(path = "Movement/Follow", priority = 2)]
    public class AIFollowBehaviour : AIBehaviour
    {
        [SerializeField] private Transform target;

        /// <summary>
        /// Update is called every frame, if the AIBehaviour is enabled.
        /// </summary>
        public override void Update()
        {
            base.Update();
            navMeshAgent.SetDestination(target.position);
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