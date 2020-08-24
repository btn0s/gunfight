/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using UnityEngine;

namespace AuroraFPSRuntime
{
    [RequireComponent(typeof(Animator))]
    public class RemoteBodyDuplicator : MonoBehaviour
    {
        // Base CloneRemoteBody properties.
        [SerializeField] private RemoteBody target;
        
        // Stored required components.
        private Animator animator;
        private FPController controller;


        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        protected virtual void Start()
        {
            controller = target.GetController();
        }
        
        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void Update()
        {
            ParametersProcessing();
        }

        /// <summary>
        /// Processing remote body animator parameters.
        /// </summary>
        protected virtual void ParametersProcessing()
        {
            animator.SetFloat(target.GetSpeedParameter().GetNameHash(), target.GetDeltaVelocity().y);
            animator.SetFloat(target.GetDirectionParameter().GetNameHash(), target.GetDeltaVelocity().x);
            animator.SetBool(target.GetIsGroundedParameter().GetNameHash(), controller.IsGrounded());
            animator.SetBool(target.GetIsCrouchingParameter().GetNameHash(), controller.IsCrouched());
        }

        #region [Getter / Setter]
        public RemoteBody GetRemoteBodyTarget()
        {
            return target;
        }

        public void SetRemoteBodyTarget(RemoteBody value)
        {
            target = value;
        }

        public Animator GetAnimator()
        {
            return animator;
        }

        protected void SetAnimator(Animator value)
        {
            animator = value;
        }

        public FPController GetController()
        {
            return controller;
        }

        public void SetController(FPController value)
        {
            controller = value;
        }
        #endregion
    }
}