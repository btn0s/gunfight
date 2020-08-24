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
    public class AILookAt : MonoBehaviour
    {
        // Base AI look at properties.
        [SerializeField] private Transform head;
        [SerializeField] private Vector3 lookAtTargetPosition;
        [SerializeField] private float lookAtCoolTime = 0.2f;
        [SerializeField] private float lookAtHeatTime = 0.2f;
        [SerializeField] private bool looking = true;

        // Stored required components.
        private Animator animator;

        // Stored required properties.
        private Vector3 lookAtPosition;
        private float lookAtWeight = 0.0f;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            InitializeLootAtPosition(head);
        }

        /// <summary>
        /// Callback for setting up animation IK (inverse kinematics).
        /// </summary>
        /// <param name="layerIndex">Index of the layer on which the IK solver is called.</param>
        protected virtual void OnAnimatorIK(int layerIndex)
        {
            if (head != null)
            {
                lookAtTargetPosition.y = head.position.y;
                float lookAtTargetWeight = looking ? 1.0f : 0.0f;

                Vector3 curDir = lookAtPosition - head.position;
                Vector3 futDir = lookAtTargetPosition - head.position;

                curDir = Vector3.RotateTowards(curDir, futDir, 6.28f * Time.deltaTime, float.PositiveInfinity);
                lookAtPosition = head.position + curDir;

                float blendTime = lookAtTargetWeight > lookAtWeight ? lookAtHeatTime : lookAtCoolTime;
                lookAtWeight = Mathf.MoveTowards(lookAtWeight, lookAtTargetWeight, Time.deltaTime / blendTime);
                animator.SetLookAtWeight(lookAtWeight, 0.2f, 0.5f, 0.7f, 0.5f);
                animator.SetLookAtPosition(lookAtPosition);
            }

        }

        /// <summary>
        /// Initialize lookAtTargetPosition and lookAtPosition properties by head transform.
        /// </summary>
        private void InitializeLootAtPosition(Transform head)
        {
            if (head != null)
            {
                lookAtTargetPosition = head.position + transform.forward;
                lookAtPosition = lookAtTargetPosition;
            }
        }

        #region [Getter / Setter]
        public Transform GetHead()
        {
            return head;
        }

        public void SetHead(Transform value)
        {
            head = value;
            InitializeLootAtPosition(head);
        }

        public Vector3 GetLookAtTargetPosition()
        {
            return lookAtTargetPosition;
        }

        public void SetLookAtTargetPosition(Vector3 value)
        {
            lookAtTargetPosition = value;
        }

        public float GetLookAtCoolTime()
        {
            return lookAtCoolTime;
        }

        public void SetLookAtCoolTime(float value)
        {
            lookAtCoolTime = value;
        }

        public float GetLookAtHeatTime()
        {
            return lookAtHeatTime;
        }

        public void SetLookAtHeatTime(float value)
        {
            lookAtHeatTime = value;
        }

        public bool GetLooking()
        {
            return looking;
        }

        public void SetLooking(bool value)
        {
            looking = value;
        }

        public Animator GetAnimator()
        {
            return animator;
        }

        protected void SetAnimator(Animator value)
        {
            animator = value;
        }

        public Vector3 GetLookAtPosition()
        {
            return lookAtPosition;
        }

        public void SetLookAtPosition(Vector3 value)
        {
            lookAtPosition = value;
        }

        public float GetLookAtWeight()
        {
            return lookAtWeight;
        }

        public void SetLookAtWeight(float value)
        {
            lookAtWeight = value;
        }
        #endregion
    }
}