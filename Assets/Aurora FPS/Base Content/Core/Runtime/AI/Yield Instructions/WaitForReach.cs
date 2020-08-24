/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using UnityEngine.AI;

namespace AuroraFPSRuntime.AI
{
    /// <summary>
    /// Custom IEnumerator yield instruction for AI NavMeshAgent.
    /// Suspends the coroutine execution until the AI reaches it's destination.
    /// </summary>
    public sealed class WaitForReach : AuroraYieldInstruction
    {
        // Base instruction properties.
        private NavMeshAgent navMeshAgent;

        /// <summary>
        /// WaitForReach constructor.
        /// </summary>
        public WaitForReach(NavMeshAgent navMeshAgent)
        {
            this.navMeshAgent = navMeshAgent;
        }

        protected override bool Update()
        {
            return !(!navMeshAgent.pathPending && (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) && !navMeshAgent.hasPath);
        }
    }
}