/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using UnityEngine;
using UnityEngine.AI;

namespace AuroraFPSRuntime.AI
{
    [ConditionOptions(path = "NavMeshAgent/On Destination Reach", description = "Return true when target destination is reached.")]
    public class OnDestinationReachCondition : Condition
    {
        // Base on destination reach properties.
        [SerializeField] private float tolerance = 0.1f;

        // Stored required components.
        private NavMeshAgent navMeshAgent;

        /// <summary>
        /// Initializing required properties.
        /// </summary>
        /// <param name="core">Target AICore instance.</param>
        public override void Initialize(AICore core)
        {
            base.Initialize(core);
            navMeshAgent = core.GetComponent<NavMeshAgent>();
        }

        /// <summary>
        /// Condition for translate to the next AI behaviour.
        /// </summary>
        public override bool IsExecuted()
        {
            return !navMeshAgent.pathPending && AMath.Approximately(navMeshAgent.remainingDistance, navMeshAgent.stoppingDistance, tolerance) && navMeshAgent.hasPath;
        }
    }
}