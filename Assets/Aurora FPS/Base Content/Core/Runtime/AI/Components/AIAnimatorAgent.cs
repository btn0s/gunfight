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
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AICore))]
    public class AIAnimatorAgent : MonoBehaviour
    {
        [Header("Animator Settings")]
        [SerializeField] private AnimatorValue speedParameter = "Speed";
        [SerializeField] private AnimatorValue directionParameter = "Direction";
        [SerializeField] private AnimatorValue crouchingParameter = "IsCrouching";
        [SerializeField] private float velocitySmooth = 7.0f;

        [Header("Other")]
        [SerializeField] private float defaultOffset = -0.16f;

        // Stored required component.
        private Animator animator;
        private NavMeshAgent navMeshAgent;
        private AICore core;

        // Stored required properties.
        private Vector2 smoothDeltaPosition = Vector2.zero;
        private Vector2 velocity = Vector2.zero;


        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            core = GetComponent<AICore>();

            navMeshAgent.updatePosition = false;
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void Update()
        {
            VelocityProcessing();
            ParametersProcessing();
        }

        /// <summary>
        /// Callback for processing animation movements for modifying root motion.
        /// </summary>
        protected virtual void OnAnimatorMove()
        {
            if (core.Enabled())
            {
                navMeshAgent.baseOffset = defaultOffset;
                transform.position = navMeshAgent.nextPosition;
            }
            else
            {
                navMeshAgent.nextPosition = animator.rootPosition;
                animator.ApplyBuiltinRootMotion();
            }
        }

        /// <summary>
        /// Update animator parameters.
        /// </summary>
        protected virtual void ParametersProcessing()
        {
            animator.SetFloat(speedParameter.GetNameHash(), velocity.y);
            animator.SetFloat(directionParameter.GetNameHash(), velocity.x);
        }

        /// <summary>
        /// Velocity processing.
        /// </summary>
        protected virtual void VelocityProcessing()
        {
            Vector3 worldDeltaPosition = navMeshAgent.nextPosition - transform.position;

            // Map 'worldDeltaPosition' to local space
            float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
            float dx = Vector3.Dot(transform.right, worldDeltaPosition);
            Vector2 deltaPosition = new Vector2(dx, dy);

            // Low-pass filter the deltaMove
            float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
            smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

            // Update velocity if time advances
            if (Time.deltaTime > 1e-5f)
            {
                velocity = smoothDeltaPosition / Time.deltaTime;
            }

            //float targetSpeed = navMeshAgent.velocity.magnitude > 0.2f ? navMeshAgent.speed : 0;
            //velocity.y = Mathf.SmoothStep(velocity.y, targetSpeed, 7 * Time.deltaTime);
        }

        /// <summary>
        /// Set AI crouching state.
        /// </summary>
        /// <param name="value"></param>
        public void Crouching(bool value)
        {
            animator.SetBool(crouchingParameter.GetNameHash(), value);
        }

        #region [Getter / Setter]
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

        public AnimatorValue GetCrouchingParameter()
        {
            return crouchingParameter;
        }

        public void SetCrouchingParameter(AnimatorValue value)
        {
            crouchingParameter = value;
        }

        public float GetVelocitySmooth()
        {
            return velocitySmooth;
        }

        public void SetVelocitySmooth(float value)
        {
            velocitySmooth = value;
        }

        public float GetDefaultOffset()
        {
            return defaultOffset;
        }

        public void SetDefaultOffset(float value)
        {
            defaultOffset = value;
        }

        public Animator GetAnimator()
        {
            return animator;
        }

        protected void SetAnimator(Animator value)
        {
            animator = value;
        }

        public NavMeshAgent GetNavMeshAgent()
        {
            return navMeshAgent;
        }

        protected void SetNavMeshAgent(NavMeshAgent value)
        {
            navMeshAgent = value;
        }

        public AICore GetController()
        {
            return core;
        }

        protected void SetController(AICore value)
        {
            core = value;
        }

        public Vector2 GetSmoothDeltaPosition()
        {
            return smoothDeltaPosition;
        }

        protected void SetSmoothDeltaPosition(Vector2 value)
        {
            smoothDeltaPosition = value;
        }

        public Vector2 GetVelocity()
        {
            return velocity;
        }

        protected void SetVelocity(Vector2 value)
        {
            velocity = value;
        }
        #endregion
    }
}