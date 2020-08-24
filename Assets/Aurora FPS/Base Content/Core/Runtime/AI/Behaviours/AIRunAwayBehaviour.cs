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
    [AIBehaviourOptions(path = "Movement/Run Away", priority = 3)]
    public class AIRunAwayBehaviour : AIBehaviour
    {
        // Base AIRunAwayBehaviour properties.
        [SerializeField] private Transform target;
        [SerializeField] private float minDistance = 5.0f;

        // Stored required components.
        private Transform transform;


        // <summary>
        /// Initiailze is called when the script instance is being loaded.
        /// 
        /// Implement this method to initialize properties.
        /// </summary>
        public override void Initialize(AICore core)
        {
            base.Initialize(core);
            transform = core.transform;
        }

        /// <summary>
        /// Update is called every frame, if the AIBehaviour is enabled.
        /// </summary>
        public override void Update()
        {
            base.Update();
            float distance = Vector3.Distance(transform.position, target.position);
            if(distance < minDistance)
            {
                Vector3 direction = transform.position - target.position;
                navMeshAgent.SetDestination(transform.position + direction);
            }
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

        public float GetMinDistance()
        {
            return minDistance;
        }

        public void SetMinDistance(float value)
        {
            minDistance = value;
        }
        #endregion
    }
}