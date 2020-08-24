/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using System;
using UnityEngine;
using UnityEngine.AI;

namespace AuroraFPSRuntime.AI
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AICore))]
    [RequireComponent(typeof(Collider))]
    public class AIAdaptiveRagdoll : CharacterRagdoll
    {
        // Base adaptive ragdoll system properties.
        [Header("AI Ragdoll Properties")]
        [SerializeField] private float relativeVelocityLimit = 10.0f;
        [SerializeField] private float standDelay = 1.5f;

        // Stored required components.
        private Animator animator;
        private AICore controller;
        private Collider controllerCollider;
        private NavMeshAgent navMeshAgent;
        private CharacterHealth health;

        // Stored required properties.
        private float storedDelayTime;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            animator = GetComponent<Animator>();
            controller = GetComponent<AICore>();
            controllerCollider = GetComponent<Collider>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<CharacterHealth>();
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        protected override void Start()
        {
            base.Start();

            BeforeRagdollCallback += () => controllerCollider.enabled = false;
            OnGetUpCallback += () => controllerCollider.enabled = true;
            OnRagdollCallback += () => storedDelayTime = Time.time;
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void Update()
        {
            if (RagdollStateIs(RagdollState.Animated) && Mathf.Abs(controller.GetVelocity().y) >= relativeVelocityLimit)
            {
                RagdollIn();
            }
            else if (IsStablePosition() && DelayElapsed() && HealthCondition())
            {
                GetUp();
            }
        }

        /// <summary>
        /// OnCollisionEnter is called when this collider/rigidbody has begun
        /// touching another rigidbody/collider.
        /// </summary>
        /// <param name="other">The Collision data associated with this collision.</param>
        protected virtual void OnCollisionEnter(Collision other)
        {
            if (enabled && RagdollStateIs(RagdollState.Animated) && other.relativeVelocity.sqrMagnitude >= (relativeVelocityLimit * relativeVelocityLimit))
            {
                RagdollIn();
                OnCollisionVelocityLimitCallback?.Invoke(other);
            }
        }

        /// <summary>
        /// Switch components enabled state.
        /// </summary>
        protected override void SwitchComponentsEnabled(bool enabled)
        {
            base.SwitchComponentsEnabled(enabled);
            controller.Enabled(enabled);
        }

        /// <summary>
        /// Controller velocity.
        /// </summary>
        public override Vector3 GetControllerVelocity()
        {
            return controller.GetVelocity();
        }

        /// <summary>
        /// Character animator component.
        /// </summary>
        public override Animator GetBodyAnimator()
        {
            return animator;
        }

        public override Transform GetBodyTransform()
        {
            return transform;
        }

        /// <summary>
        /// Return true if the delay time has completely elapsed.
        /// </summary>
        public bool DelayElapsed()
        {
            return storedDelayTime > 0 && Time.time - storedDelayTime >= standDelay;
        }

        public bool HealthCondition()
        {
            return health != null ? health.IsAlive() : true;
        }

        #region [Event Callback Function]
        /// <summary>
        /// On collision hit callback function.
        /// OnCollisionHit is called when this collider/rigidbody has begun touching another rigidbody/collider 
        /// with relative velocity more/equal then relative velocity limit.
        /// </summary>
        /// <param name="Collision">The Collision data associated with this collision.</param>
        public event Action<Collision> OnCollisionVelocityLimitCallback;
        #endregion

        #region [Getter / Setter]
        public float GetRelativeVelocityLimit()
        {
            return relativeVelocityLimit;
        }

        public void SetRelativeVelocityLimit(float value)
        {
            relativeVelocityLimit = value;
        }

        public float GetStandDelay()
        {
            return standDelay;
        }

        public void SetStandDelay(float value)
        {
            standDelay = value;
        }
        #endregion
    }
}