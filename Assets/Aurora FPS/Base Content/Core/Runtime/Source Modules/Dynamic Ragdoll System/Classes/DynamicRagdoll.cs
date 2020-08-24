/* ==================================================================
   ---------------------------------------------------
   Project   :    Beyond
   Publisher :    -
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================== */

using System;
using UnityEngine;
using System.Collections;

namespace AuroraFPSRuntime
{
    public partial class DynamicRagdoll : MonoBehaviour, IDynamicRagdoll
    {
        public enum LieSide
        {
            Front,
            Back
        }

        public enum State
        {
            Animated,
            Radolled,
            BlendingToAnimation,
            BlendingToRagdoll
        }

        // Blending properties.
        [SerializeField] private float animToRagdollBlendTime = 1.25f;
        [SerializeField] private float ragdollToAnimBlendTime = 0.25f;
        [SerializeField] private AnimatorState getUpFromBelly = "FromBelly";
        [SerializeField] private AnimatorState getUpFromBack = "FromBack";
        [SerializeField] private AnimatorState blendAnimation = AnimatorState.none;

        // Stand settings
        [SerializeField] private bool autoGetUp = true;
        [SerializeField] private float autoGetUpDelay = 1.0f;

        // Stored required components.
        private Transform ragdollTransfrom;
        private Animator animator;
        private new Collider collider;
        private Rigidbody[] rigidbodies;
        private Transform hipsTransform;
        private Rigidbody hipsRigidbody;

        // Stored required properties.
        private State state;
        private BoneTransform[] bonesTransform;
        private Vector3 storedHipsPosition;
        private Vector3 storedHipsPositionPrivAnim;
        private Vector3 storedHipsPositionPrivBlend;
        private float ragdollTime;
        private CoroutineObject blendingToRagdollCoroutineObject;
        private CoroutineObject<float> autoGetUpDelayCoroutineObject;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            CopyAnimator(out animator);
            CopyRagdollTransform(out ragdollTransfrom);
            CopyRootCollider(out collider);
            CopyBonesTransform(out bonesTransform);
            CopyRigidbodies(out rigidbodies);
            hipsTransform = animator.GetBoneTransform(HumanBodyBones.Hips);
            hipsRigidbody = hipsTransform.GetComponent<Rigidbody>();
            blendingToRagdollCoroutineObject = new CoroutineObject(this);
            autoGetUpDelayCoroutineObject = new CoroutineObject<float>(this);
            BonesKinematic(true);
            CanAutoGetUp += () => autoGetUp;
            InitializeHealthExtension(GetComponent<ObjectHealth>());
        }

        /// <summary>
        /// LateUpdate is called every frame, after all Update functions have been called, 
        /// if the Behaviour is enabled.
        /// </summary>
        protected virtual void LateUpdate()
        {
            if (StateIs(State.BlendingToAnimation))
            {
                BlendingToAnimation();
            }
            else if (CanAutoGetUp() && StateIs(State.Radolled) && RagdollStabilized())
            {
                autoGetUpDelayCoroutineObject.Start(AutoGetUpDelay, autoGetUpDelay);
            }
        }

        /// <summary>
        /// Start processing ragdoll of the character.
        /// </summary>
        public void StartRagdoll()
        {
            if (StateIs(State.Animated) && !blendingToRagdollCoroutineObject.IsProcessing())
            {
                blendingToRagdollCoroutineObject.Start(BlendingToRagdoll);
            }
        }

        /// <summary>
        /// Start processing animator of the character.
        /// </summary>
        public void StartAnimator()
        {
            ragdollTime = Time.time;
            OnStartAnimatorCallback?.Invoke();
            DependentComponents(true);
            state = State.BlendingToAnimation;

            storedHipsPositionPrivAnim = Vector3.zero;
            storedHipsPositionPrivBlend = Vector3.zero;

            storedHipsPosition = hipsTransform.position;

            Vector3 shiftPos = hipsTransform.position - ragdollTransfrom.position;
            shiftPos.y = GetDistanceToFloor(shiftPos.y);

            MoveNodeWithoutChildren(shiftPos);

            for (int i = 0; i < bonesTransform.Length; i++)
            {
                BoneTransform tramsformComponent = bonesTransform[i];
                tramsformComponent.SetStoredRotation(tramsformComponent.GetTransform().localRotation);
                tramsformComponent.SetRotation(tramsformComponent.GetTransform().localRotation);

                tramsformComponent.SetStoredPosition(tramsformComponent.GetTransform().localPosition);
                tramsformComponent.SetPosition(tramsformComponent.GetTransform().localPosition);
                bonesTransform[i] = tramsformComponent;
            }

            switch (GetCharacterLieSide())
            {
                case LieSide.Front:
                    animator.CrossFadeInFixedTime(getUpFromBelly);
                    break;
                case LieSide.Back:
                    animator.CrossFadeInFixedTime(getUpFromBack);
                    break;
            }

            BonesKinematic(true);
        }

        /// <summary>
        /// Start processing animator of the character skipping blending from ragdoll.
        /// </summary>
        public void StartAnimatorForce()
        {
            StartAnimator();
            ragdollTime = 0;
            animator.PlayInFixedTime(string.Empty, 0);
        }

        /// <summary>
        /// Calculate body direction, when character get up.
        /// </summary>
        /// <returns>Body direction when character get up.</returns>
        public virtual Vector3 CalculateBodyDirection()
        {
            Vector3 ragdolledFeetPosition = (animator.GetBoneTransform(HumanBodyBones.Hips).position);
            Vector3 ragdolledHeadPosition = animator.GetBoneTransform(HumanBodyBones.Head).position;
            Vector3 ragdollDirection = ragdolledFeetPosition - ragdolledHeadPosition;
            ragdollDirection.y = 0;
            ragdollDirection = ragdollDirection.normalized;

            if (GetCharacterLieSide() == LieSide.Front)
                return -ragdollDirection;

            return ragdollDirection;
        }

        /// <summary>
        /// Calculate distance between character and floor.
        /// </summary>
        public float GetDistanceToFloor(float currentY)
        {
            RaycastHit[] hits = Physics.RaycastAll(new Ray(hipsTransform.position, Vector3.down));
            float distFromFloor = float.MinValue;

            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];
                if (!hit.transform.IsChildOf(ragdollTransfrom))
                {
                    distFromFloor = Mathf.Max(distFromFloor, hit.point.y);
                }
            }

            if (Mathf.Abs(distFromFloor - float.MinValue) > Mathf.Epsilon)
                currentY = distFromFloor - ragdollTransfrom.position.y;

            return currentY;
        }

        /// <summary>
        /// The side on which the character lies (while ragdolled).
        /// </summary>
        public LieSide GetCharacterLieSide()
        {
            return Vector3.Dot(hipsTransform.up, Vector3.up) < 0 ? LieSide.Front : LieSide.Back;
        }

        /// <summary>
        /// Compare current state.
        /// </summary>
        public bool StateIs(State state)
        {
            return this.state == state;
        }

        /// <summary>
        /// Return true when ragdoll bones stabilized (not moving).
        /// </summary>
        /// <returns></returns>
        public bool RagdollStabilized()
        {
            return hipsRigidbody.velocity.magnitude <= 0.1f;
        }

        /// <summary>
        /// Override this method to return animator component of the ragdoll character.
        /// Use GetComponent<Animator>() method.
        /// </summary>
        /// <param name="animator">Animator component of the ragdoll character.</param>
        public virtual void CopyAnimator(out Animator animator)
        {
            animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Override this method to initialize transform component of the ragdoll character.
        /// </summary>
        /// <param name="transform">Transform component of the ragdoll character.</param>
        public virtual void CopyRagdollTransform(out Transform ragdollTransform)
        {
            ragdollTransform = transform;
        }

        /// <summary>
        /// Override this method to initialize root collider component of the ragdoll character.
        /// </summary>
        /// <param name="collider">Collider component of the ragdoll character.</param>
        public virtual void CopyRootCollider(out Collider collider)
        {
            collider = GetComponent<Collider>();
        }

        /// <summary>
        /// Copy all rigidbodies of character.
        /// </summary>
        /// <param name="rigidbodies">Rigidbody of the character, in array representation.</param>
        public void CopyRigidbodies(out Rigidbody[] rigidbodies)
        {
            rigidbodies = ragdollTransfrom.GetComponentsInChildren<Rigidbody>();
        }

        /// <summary>
        /// Get BoneTranforms of character.
        /// </summary>
        /// <param name="bonesTransform">Bones transform of the character, in array representation.</param>
        public void CopyBonesTransform(out BoneTransform[] bonesTransform)
        {
            Transform[] transforms = ragdollTransfrom.GetComponentsInChildren<Transform>();
            bonesTransform = new BoneTransform[transforms.Length];
            for (int i = 0; i < transforms.Length; i++)
            {
                bonesTransform[i] = new BoneTransform(transforms[i]);
            }
        }

        /// <summary>
        /// Blending processing from animation to ragdoll.
        /// </summary>
        private IEnumerator BlendingToRagdoll()
        {
            if (!string.IsNullOrEmpty(blendAnimation))
            {
                state = State.BlendingToRagdoll;
                animator.CrossFadeInFixedTime(blendAnimation);
                yield return new WaitForSeconds(animToRagdollBlendTime);
            }
            OnStartRagdollCallback?.Invoke();
            DependentComponents(false);
            Vector3 storedVelocity = animator.velocity;
            BonesKinematic(false);
            AddVelocity(storedVelocity);
            state = State.Radolled;
            OnBlendCompleteCallback?.Invoke();
        }

        /// <summary>
        /// Get up the character after specific delay time.
        /// </summary>
        /// <param name="delay">Time to delay.</param>
        protected IEnumerator AutoGetUpDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (!CanAutoGetUp())
                yield break;
            StartAnimator();
        }

        /// <summary>
        /// Blending processing from ragdoll to animation.
        /// </summary>
        private void BlendingToAnimation()
        {
            float ragdollBlendAmount = 1f - Mathf.InverseLerp(ragdollTime, ragdollTime + ragdollToAnimBlendTime, Time.time);

            if (storedHipsPositionPrivBlend != hipsTransform.position)
            {
                storedHipsPositionPrivAnim = hipsTransform.position;
            }
            storedHipsPositionPrivBlend = Vector3.Lerp(storedHipsPositionPrivAnim, storedHipsPosition, ragdollBlendAmount);
            hipsTransform.position = storedHipsPositionPrivBlend;

            for (int i = 0; i < bonesTransform.Length; i++)
            {
                BoneTransform transformComponent = bonesTransform[i];
                if (transformComponent.GetPosition() != transformComponent.GetTransform().localPosition)
                {
                    transformComponent.SetPosition(Vector3.Lerp(transformComponent.GetTransform().localPosition, transformComponent.GetStoredPosition(), ragdollBlendAmount));
                    transformComponent.GetTransform().localPosition = transformComponent.GetPosition();
                }

                if (transformComponent.GetRotation() != transformComponent.GetTransform().localRotation)
                {
                    transformComponent.SetRotation(Quaternion.Slerp(transformComponent.GetTransform().localRotation, transformComponent.GetStoredRotation(), ragdollBlendAmount));
                    transformComponent.GetTransform().localRotation = transformComponent.GetRotation();
                }
            }

            if (Mathf.Abs(ragdollBlendAmount) < Mathf.Epsilon)
            {
                state = State.Animated;
                OnBlendCompleteCallback?.Invoke();
            }
        }

        /// <summary>
        /// Switch enabled state of dependent components.
        /// </summary>
        /// <param name="enabled">Enabled state of components.</param>
        protected virtual void DependentComponents(bool enabled)
        {
            animator.enabled = enabled;
            if (collider != null)
                collider.enabled = enabled;
        }

        /// <summary>
        /// Add velocty to each rigidbody component of the character.
        /// </summary>
        protected void AddVelocity(Vector3 velocity)
        {
            for (int i = 0; i < rigidbodies.Length; i++)
            {
                rigidbodies[i].velocity = velocity;
            }
        }

        /// <summary>
        /// Switch kinematic enabled state of the each rigidbody component of the character.
        /// </summary>
        /// <param name="enabled">Enabled state of parts.</param>
        protected virtual void BonesKinematic(bool enabled)
        {
            Rigidbody[] rigidbodies = ragdollTransfrom.GetComponentsInChildren<Rigidbody>();
            int i = 0;
            for (i = 0;  i < rigidbodies.Length; i++)
            {
                rigidbodies[i].isKinematic = enabled;
            }
        }

        /// <summary>
        /// Move node without children.
        /// </summary>
        protected void MoveNodeWithoutChildren(Vector3 shiftPos)
        {
            Vector3 ragdollDirection = CalculateBodyDirection();

            hipsTransform.position -= shiftPos;
            ragdollTransfrom.position += shiftPos;

            Vector3 forward = ragdollTransfrom.forward;
            ragdollTransfrom.rotation = Quaternion.FromToRotation(forward, ragdollDirection) * ragdollTransfrom.rotation;
            hipsTransform.rotation = Quaternion.FromToRotation(ragdollDirection, forward) * hipsTransform.rotation;
        }

        /// <summary>
        /// Register health required callbacks.
        /// </summary>
        protected virtual void InitializeHealthExtension(ObjectHealth objectHealth)
        {
            if (objectHealth != null)
            {
                objectHealth.OnWakeUpCallback += StartAnimatorForce;
                objectHealth.OnDeadCallback += StartRagdoll;
                CanAutoGetUp += objectHealth.IsAlive;
            }
        }

        #region [Event Callback Function]
        /// <summary>
        /// Called when starting ragdoll processing.
        /// </summary>
        public event Action OnStartRagdollCallback;

        /// <summary>
        /// Called when stating animation processing.
        /// </summary>
        public event Action OnStartAnimatorCallback;

        /// <summary>
        /// Called when complete blending.
        /// </summary>
        public event Action OnBlendCompleteCallback;

        /// <summary>
        /// Called when character trying get up.
        /// </summary>
        public event Func<bool> CanAutoGetUp;
        #endregion

        #region [Getter / Setter]
        public float GetAnimToRagdollBlendTime()
        {
            return animToRagdollBlendTime;
        }

        public void SetAnimToRagdollBlendTime(float value)
        {
            animToRagdollBlendTime = value;
        }

        public float GetRagdollToAnimBlendTime()
        {
            return ragdollToAnimBlendTime;
        }

        public void SetRagdollToAnimBlendTime(float value)
        {
            ragdollToAnimBlendTime = value;
        }

        public AnimatorState GetGetUpFromBelly()
        {
            return getUpFromBelly;
        }

        public void SetGetUpFromBelly(AnimatorState value)
        {
            getUpFromBelly = value;
        }

        public AnimatorState GetGetUpFromBack()
        {
            return getUpFromBack;
        }

        public void SetGetUpFromBack(AnimatorState value)
        {
            getUpFromBack = value;
        }

        public AnimatorState GetBlendAnimation()
        {
            return blendAnimation;
        }

        public void SetBlendAnimation(AnimatorState value)
        {
            blendAnimation = value;
        }

        public bool AutoGetUp()
        {
            return autoGetUp;
        }

        public void AutoGetUp(bool value)
        {
            autoGetUp = value;
        }

        public float GetAutoGetUpDelay()
        {
            return autoGetUpDelay;
        }

        public void SetAutoGetUpDelay(float value)
        {
            autoGetUpDelay = value;
        }

        public State GetState()
        {
            return state;
        }

        protected void SetState(State value)
        {
            state = value;
        }

        public Animator GetAnimator()
        {
            return animator;
        }

        public void SetAnimator(Animator value)
        {
            animator = value;
        }
        #endregion
    }
}