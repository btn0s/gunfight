/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using UnityEngine;

namespace AuroraFPSRuntime
{
    [RequireComponent(typeof(Animator))]
    public class RemoteBody : MonoBehaviour
    {
        // Remote body properties.
        [Header("Body Properties")]
        [SerializeField] private FPController controller;

        [Header("Animation Properties")]
        [SerializeField] private AnimatorValue speedParameter = "Speed";
        [SerializeField] private AnimatorValue directionParameter = "Direction";
        [SerializeField] private AnimatorValue isGroundedParameter = "IsGrounded";
        [SerializeField] private AnimatorValue isCrouchingParameter = "IsCrouching";
        [SerializeField] private float velocitySmooth = 0.9f;

        // Stored required components.
        private Animator animator;

        // Stored required properties.
        private Vector3 deltaVelocity;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// Called only once during the lifetime of the script instance and after all objects are initialized
        /// </summary>
        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void Update()
        {
            VelocityPrcessing();
            ParametersProcessing();
        }

        protected virtual void VelocityPrcessing()
        {
            if (controller.IsEnabled())
            {
                Vector3 worldDeltaPosition = controller.GetVelocity();
                float dx = Vector3.Dot(transform.right, worldDeltaPosition);
                float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
                Vector3 newDeltaVelocity = new Vector2(dx, dy);
                deltaVelocity = Vector3.Lerp(deltaVelocity, newDeltaVelocity, velocitySmooth * Time.deltaTime);
            }
            else
            {
                deltaVelocity = Vector3.zero;
            }
        }

        /// <summary>
        /// Processing remote body animator parameters.
        /// </summary>
        protected virtual void ParametersProcessing()
        {
            animator.SetFloat(speedParameter.GetNameHash(), deltaVelocity.y);
            animator.SetFloat(directionParameter.GetNameHash(), deltaVelocity.x);
            animator.SetBool(isGroundedParameter.GetNameHash(), controller.IsGrounded());
            animator.SetBool(isCrouchingParameter.GetNameHash(), controller.IsCrouched());
        }

        #region [Getter / Setter]
        public FPController GetController()
        {
            return controller;
        }

        public void SetController(FPController value)
        {
            controller = value;
        }

        public Vector3 GetDeltaVelocity()
        {
            return deltaVelocity;
        }

        public void SetDeltaVelocity(Vector3 value)
        {
            deltaVelocity = value;
        }

        public AnimatorValue GetSpeedParameter()
        {
            return speedParameter;
        }

        public void SetSpeedParameter(AnimatorValue value)
        {
            speedParameter = value;
        }

        public AnimatorValue GetDirectionParameter()
        {
            return directionParameter;
        }

        public void SetDirectionParameter(AnimatorValue value)
        {
            directionParameter = value;
        }

        public AnimatorValue GetIsGroundedParameter()
        {
            return isGroundedParameter;
        }

        public void SetIsGroundedParameter(AnimatorValue value)
        {
            isGroundedParameter = value;
        }

        public AnimatorValue GetIsCrouchingParameter()
        {
            return isCrouchingParameter;
        }

        public void SetIsCrouchingParameter(AnimatorValue value)
        {
            isCrouchingParameter = value;
        }

        public float GetVelocitySmooth()
        {
            return velocitySmooth;
        }

        public void SetVelocitySmooth(float value)
        {
            velocitySmooth = value;
        }

        public Animator GetAnimator()
        {
            return animator;
        }

        protected void SetAnimator(Animator value)
        {
            animator = value;
        }
        #endregion
    }
}