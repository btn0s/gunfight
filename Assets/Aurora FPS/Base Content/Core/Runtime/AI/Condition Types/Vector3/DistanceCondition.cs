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
    [ConditionOptions(path = "Vector3/Distance", description = "Compare distance betweem target and AI.")]
    public class DistanceCondition : Condition
    {
        public enum Comparison
        {
            Equal,
            NotEqual,
            Greater,
            Less,
        }

        [SerializeField] private Transform target;
        [SerializeField] private Comparison comparison;
        [SerializeField] private float distance;

        private Transform transform;


        public override void Initialize(AICore core)
        {
            base.Initialize(core);
            transform = core.transform;
        }

        /// <summary>
        /// Condition for translate to the next AI behaviour.
        /// </summary>
        public override bool IsExecuted()
        {
            if(target == null)
            {
                return false;
            }

            switch (comparison)
            {
                case Comparison.Equal:
                    return Vector3.Distance(transform.position, target.position) == distance;
                case Comparison.NotEqual:
                    return Vector3.Distance(transform.position, target.position) != distance;
                case Comparison.Greater:
                    return Vector3.Distance(transform.position, target.position) > distance;
                case Comparison.Less:
                    return Vector3.Distance(transform.position, target.position) < distance;
                default:
                    return false;
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

        public Comparison GetComparison()
        {
            return comparison;
        }

        public void SetComparison(Comparison value)
        {
            comparison = value;
        }

        public float GetDistance()
        {
            return distance;
        }

        public void SetDistance(float value)
        {
            distance = value;
        }

        public Transform GetTransform()
        {
            return transform;
        }
        #endregion
    }
}
