/* ==================================================================
   ---------------------------------------------------
   Project   :    Beyond
   Publisher :    -
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================== */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AuroraFPSRuntime
{
    public abstract partial class CharacterRagdoll : MonoBehaviour, ICharacterRagdoll
    {
        /// <summary>
        /// Ragdoll to mecanim blend time.
        /// </summary>
        public const float BLEND_TIME = 0.5f;

        /// <summary>
        /// Amplifier to extra move in air.
        /// </summary>
        public const float AIR_SPEED = 5.0f;

        // Base ragdoll properties.
        [Header("Animation Properties")]
        [SerializeField] private AnimatorValue getUpFromBellyState = "FromBelly";
        [SerializeField] private AnimatorValue getUpFromBackState = "FromBack";
        [SerializeField] private float bellyStandTime = 1.0f;
        [SerializeField] private float backStandTime = 1.0f;

        [SerializeField] private UnityEvent onRagdollEvent;
        [SerializeField] private UnityEvent onGetUpEvent;
        [SerializeField] private UnityEvent onReadyToGoEvent;

        // Stored required properties.
        private float ragdollingEndTime;
        private RagdollState ragdollState = RagdollState.Animated;
        private Transform hipsTransform;
        private Rigidbody hipsRigidbody;
        private Vector3 storedHipsPosition;
        private Vector3 storedHipsPositionPrivAnim;
        private Vector3 storedHipsPositionPrivBlend;
        private List<RigidbodyComponent> rigidbodyComponents;
        private List<TransformComponent> transformComponents;
        private CoroutineObject<float> getUpTimeCompleteCoroutine;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            OnRagdollCallback += () => onRagdollEvent?.Invoke();
            OnGetUpCallback += () => onGetUpEvent?.Invoke();
            OnReadyToGoCallback += () => onReadyToGoEvent?.Invoke();
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        protected virtual void Start()
        {
            hipsTransform = GetBodyAnimator().GetBoneTransform(HumanBodyBones.Hips);

            hipsRigidbody = hipsTransform.GetComponent<Rigidbody>();

            getUpTimeCompleteCoroutine = new CoroutineObject<float>(this);

            rigidbodyComponents = new List<RigidbodyComponent>();
            transformComponents = new List<TransformComponent>();

            Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>(true);
            for (int i = 0, length = rigidbodies.Length; i < length; i++)
            {
                Rigidbody rigid = rigidbodies[i];
                if (rigid.transform == transform)
                {
                    continue;
                }

                RigidbodyComponent rigidCompontnt = new RigidbodyComponent(rigid);
                rigidbodyComponents.Add(rigidCompontnt);
            }

            Transform[] array = GetComponentsInChildren<Transform>();
            for (int i = 0, length = array.Length; i < length; i++)
            {
                Transform t = array[i];
                TransformComponent trComp = new TransformComponent(t);
                transformComponents.Add(trComp);
            }

            ActivateRagdollParts(false);
        }

        /// <summary>
        /// LateUpdate is called every frame, if the Behaviour is enabled.
        /// It is called after all Update functions have been called.
        /// </summary>
        protected virtual void LateUpdate()
        {
            if (ragdollState == RagdollState.BlendToAnim)
            {
                float ragdollBlendAmount = 1f - Mathf.InverseLerp(ragdollingEndTime, ragdollingEndTime + BLEND_TIME, Time.time);

                if (storedHipsPositionPrivBlend != hipsTransform.position)
                {
                    storedHipsPositionPrivAnim = hipsTransform.position;
                }
                storedHipsPositionPrivBlend = Vector3.Lerp(storedHipsPositionPrivAnim, storedHipsPosition, ragdollBlendAmount);
                hipsTransform.position = storedHipsPositionPrivBlend;

                for (int i = 0; i < transformComponents.Count; i++)
                {
                    TransformComponent tc = transformComponents[i];
                    if (tc.GetRotation() != tc.GetTransform().localRotation)
                    {
                        tc.SetRotation(Quaternion.Slerp(tc.GetTransform().localRotation, tc.GetStoredRotation(), ragdollBlendAmount));
                        tc.GetTransform().localRotation = tc.GetRotation();
                    }

                    if (tc.GetPosition() != tc.GetTransform().localPosition)
                    {
                        tc.SetPosition(Vector3.Slerp(tc.GetTransform().localPosition, tc.GetStoredPosition(), ragdollBlendAmount));
                        tc.GetTransform().localPosition = tc.GetPosition();
                    }
                }

                if (Mathf.Abs(ragdollBlendAmount) < Mathf.Epsilon)
                {
                    ragdollState = RagdollState.Animated;
                }
            }
        }

        /// <summary>
        /// Controller velocity.
        /// </summary>
        public abstract Vector3 GetControllerVelocity();

        /// <summary>
        /// Ragdoll body animator component.
        /// </summary>
        public abstract Animator GetBodyAnimator();

        /// <summary>
        /// Ragdoll body transform component.
        /// </summary>
        public abstract Transform GetBodyTransform();

        /// <summary>
        /// Activate character ragdoll.
        /// </summary>
        public virtual void RagdollIn()
        {
            BeforeRagdollCallback?.Invoke();
            Vector3 characterVelocity = GetControllerVelocity();
            SwitchComponentsEnabled(false);
            ActivateRagdollParts(true);
            GetBodyAnimator().enabled = false;
            ragdollState = RagdollState.Ragdolled;
            ApplyVelocity(characterVelocity);
            getUpTimeCompleteCoroutine.Stop();
            OnRagdollCallback?.Invoke();
        }

        /// <summary>
        /// Deactivate character ragdoll and play stand up animation.
        /// </summary>
        public virtual void GetUp()
        {
            ragdollingEndTime = Time.time;
            GetBodyAnimator().enabled = true;
            ragdollState = RagdollState.BlendToAnim;
            storedHipsPositionPrivAnim = Vector3.zero;
            storedHipsPositionPrivBlend = Vector3.zero;

            storedHipsPosition = hipsTransform.position;

            Vector3 shiftPos = hipsTransform.position - GetBodyTransform().position;
            shiftPos.y = GetDistanceToFloor(shiftPos.y);

            MoveNodeWithoutChildren(shiftPos);

            for (int i = 0; i < transformComponents.Count; i++)
            {
                TransformComponent tc = transformComponents[i];
                tc.SetStoredRotation(tc.GetTransform().localRotation);
                tc.SetRotation(tc.GetTransform().localRotation);

                tc.SetStoredPosition(tc.GetTransform().localPosition);
                tc.SetPosition(tc.GetTransform().localPosition);
            }

            // ApplyVelocity(Vector3.zero);

            float timeToGetUp = 0;
            if (CheckIfLieOnBack())
            {
                GetBodyAnimator().CrossFadeInFixedTime(getUpFromBackState.GetNameHash(), 0, 0);
                timeToGetUp = backStandTime;
            }
            else
            {
                GetBodyAnimator().CrossFadeInFixedTime(getUpFromBellyState.GetNameHash(), 0);
                timeToGetUp = bellyStandTime;
            }
            getUpTimeCompleteCoroutine.Start(GetUpTimeComplete, timeToGetUp);
            ActivateRagdollParts(false);
            OnGetUpCallback?.Invoke();
        }

        /// <summary>
        /// Return true ragdolled controller is in a stable state.
        /// </summary>
        public bool IsStablePosition()
        {
            return RagdollStateIs(RagdollState.Ragdolled) && hipsRigidbody.velocity.magnitude < 0.1f;
        }

        /// <summary>
        /// Calculate distance between character and floor.
        /// </summary>
        protected float GetDistanceToFloor(float currentY)
        {
            RaycastHit[] hits = Physics.RaycastAll(new Ray(hipsTransform.position, Vector3.down));
            float distFromFloor = float.MinValue;

            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];
                if (!hit.transform.IsChildOf(transform))
                {
                    distFromFloor = Mathf.Max(distFromFloor, hit.point.y);
                }
            }

            if (Mathf.Abs(distFromFloor - float.MinValue) > Mathf.Epsilon)
                currentY = distFromFloor - GetBodyTransform().position.y;

            return currentY;
        }

        /// <summary>
        /// Move node without children.
        /// </summary>
        protected void MoveNodeWithoutChildren(Vector3 shiftPos)
        {
            Vector3 ragdollDirection = GetRagdollDirection();

            hipsTransform.position -= shiftPos;
            GetBodyTransform().position += shiftPos;

            Vector3 forward = GetBodyTransform().forward;
            GetBodyTransform().rotation = Quaternion.FromToRotation(forward, ragdollDirection) * GetBodyTransform().rotation;
            hipsTransform.rotation = Quaternion.FromToRotation(ragdollDirection, forward) * hipsTransform.rotation;
        }

        /// <summary>
        /// Return true character lie on back and false if on front.
        /// </summary>
        protected bool CheckIfLieOnBack()
        {
            Vector3 left = GetBodyAnimator().GetBoneTransform(HumanBodyBones.LeftUpperLeg).position;
            Vector3 right = GetBodyAnimator().GetBoneTransform(HumanBodyBones.RightUpperLeg).position;
            Vector3 hipsPos = hipsTransform.position;

            left -= hipsPos;
            left.y = 0f;
            right -= hipsPos;
            right.y = 0f;

            Quaternion q = Quaternion.FromToRotation(left, Vector3.right);
            Vector3 t = q * right;

            return t.z < 0f;
        }

        /// <summary>
        /// Character ragdoll direction.
        /// </summary>
        protected Vector3 GetRagdollDirection()
        {
            Vector3 ragdolledFeetPosition = (
                GetBodyAnimator().GetBoneTransform(HumanBodyBones.Hips).position);
            Vector3 ragdolledHeadPosition = GetBodyAnimator().GetBoneTransform(HumanBodyBones.Head).position;
            Vector3 ragdollDirection = ragdolledFeetPosition - ragdolledHeadPosition;
            ragdollDirection.y = 0;
            ragdollDirection = ragdollDirection.normalized;

            if (CheckIfLieOnBack())
                return ragdollDirection;
            else
                return -ragdollDirection;
        }

        /// <summary>
        /// Apply velocity 'predieVelocity' to to each rigid of character
        /// </summary>
        protected void ApplyVelocity(Vector3 predieVelocity)
        {
            for (int i = 0, length = rigidbodyComponents.Count; i < length; i++)
            {
                rigidbodyComponents[i].GetRigidBody().velocity = predieVelocity;
            }
        }

        /// <summary>
        /// Activate character ragdoll required parts.
        /// </summary>
        protected void ActivateRagdollParts(bool activate)
        {
            for (int i = 0, length = rigidbodyComponents.Count; i < length; i++)
            {
                RigidbodyComponent rigidbody = rigidbodyComponents[i];
                Collider partColider = rigidbody.GetRigidBody().GetComponent<Collider>();

                if (partColider == null)
                {
                    string childName = rigidbody.GetRigidBody().name + "_ColliderRotator";
                    Transform transform = rigidbody.GetRigidBody().transform.Find(childName);
                    partColider = GetBodyTransform().GetComponent<Collider>();
                }

                partColider.isTrigger = !activate;

                if (activate)
                {
                    rigidbody.GetRigidBody().isKinematic = false;
                    StartCoroutine(FixTransformAndEnableJoint(rigidbody));
                }
                else
                {
                    rigidbody.GetRigidBody().isKinematic = true;
                }
            }
        }

        /// <summary>
        /// Switch components enabled state.
        /// </summary>
        protected virtual void SwitchComponentsEnabled(bool enabled) { }

        /// <summary>
        /// Character is ragdolled
        /// </summary>
        public bool IsRagdolled()
        {
            return ragdollState == RagdollState.Ragdolled;
        }

        /// <summary>
        /// Add extra move direction for all character rigidbody parts.
        /// </summary>
        public void AddExtraMove(Vector3 move)
        {
            if (IsRagdolled())
            {
                Vector3 airMove = new Vector3(move.x * AIR_SPEED, 0f, move.z * AIR_SPEED);
                for (int i = 0, length = rigidbodyComponents.Count; i < length; i++)
                {
                    RigidbodyComponent rigidbodyComponent = rigidbodyComponents[i];
                    rigidbodyComponent.GetRigidBody().AddForce(airMove / 100f, ForceMode.VelocityChange);
                }
            }
        }

        /// <summary>
        /// Compare current ragdoll state.
        /// </summary>
        public bool RagdollStateIs(RagdollState value)
        {
            return ragdollState == value;
        }

        /// <summary>
        /// Prevents jittering (as a result of applying joint limits) of bone and smoothly translate rigid from animated mode to ragdoll
        /// </summary>
        private IEnumerator FixTransformAndEnableJoint(RigidbodyComponent rigidbodyComponent)
        {
            CharacterJoint joint = rigidbodyComponent.GetJoint();

            if (joint == null || !joint.autoConfigureConnectedAnchor)
            {
                yield break;
            }

            SoftJointLimit highTwistLimit = new SoftJointLimit();
            SoftJointLimit lowTwistLimit = new SoftJointLimit();
            SoftJointLimit swing1Limit = new SoftJointLimit();
            SoftJointLimit swing2Limit = new SoftJointLimit();

            SoftJointLimit curHighTwistLimit = highTwistLimit = joint.highTwistLimit;
            SoftJointLimit curLowTwistLimit = lowTwistLimit = joint.lowTwistLimit;
            SoftJointLimit curSwing1Limit = swing1Limit = joint.swing1Limit;
            SoftJointLimit curSwing2Limit = swing2Limit = joint.swing2Limit;

            float aTime = 0.3f;
            Vector3 startConPosition = joint.connectedBody.transform.InverseTransformVector(joint.transform.position - joint.connectedBody.transform.position);

            joint.autoConfigureConnectedAnchor = false;
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
            {
                Vector3 newConPosition = Vector3.Lerp(startConPosition, rigidbodyComponent.GetConnectedAnchorDefault(), t);
                joint.connectedAnchor = newConPosition;

                curHighTwistLimit.limit = Mathf.Lerp(177, highTwistLimit.limit, t);
                curLowTwistLimit.limit = Mathf.Lerp(-177, lowTwistLimit.limit, t);
                curSwing1Limit.limit = Mathf.Lerp(177, swing1Limit.limit, t);
                curSwing2Limit.limit = Mathf.Lerp(177, swing2Limit.limit, t);

                joint.highTwistLimit = curHighTwistLimit;
                joint.lowTwistLimit = curLowTwistLimit;
                joint.swing1Limit = curSwing1Limit;
                joint.swing2Limit = curSwing2Limit;

                yield return null;
            }
            joint.connectedAnchor = rigidbodyComponent.GetConnectedAnchorDefault();
            yield return new WaitForFixedUpdate();
            joint.autoConfigureConnectedAnchor = true;

            joint.highTwistLimit = highTwistLimit;
            joint.lowTwistLimit = lowTwistLimit;
            joint.swing1Limit = swing1Limit;
            joint.swing2Limit = swing2Limit;
        }

        /// <summary>
        /// Called when the get up animation is fully completed.
        /// </summary>
        private IEnumerator GetUpTimeComplete(float time)
        {
            yield return new WaitForSeconds(time);
            SwitchComponentsEnabled(true);
            OnReadyToGoCallback?.Invoke();
            yield break;
        }

        #region [Event Callback Functions]
        public event Action BeforeRagdollCallback;

        /// <summary>
        /// Сalled when controller enter in ragdoll state.
        /// </summary>
        public event Action OnRagdollCallback;

        /// <summary>
        /// Сalled when controller still in stable position and start get up.
        /// </summary>
        public event Action OnGetUpCallback;

        /// <summary>
        /// Сalled when controller fully get up and ready to go.
        /// </summary>
        public event Action OnReadyToGoCallback;

        #endregion

        #region [Getter / Setter]
        public AnimatorValue GetGetUpFromBellyState()
        {
            return getUpFromBellyState;
        }

        public void SetGetUpFromBellyState(AnimatorValue value)
        {
            getUpFromBellyState = value;
        }

        public AnimatorValue GetGetUpFromBackState()
        {
            return getUpFromBackState;
        }

        public void SetGetUpFromBackState(AnimatorValue value)
        {
            getUpFromBackState = value;
        }

        public Transform GetHipsTransform()
        {
            return hipsTransform;
        }

        public void SetHipsTransform(Transform value)
        {
            hipsTransform = value;
        }

        public Rigidbody GetHipsRigidbody()
        {
            return hipsRigidbody;
        }

        public void SetHipsRigidbody(Rigidbody value)
        {
            hipsRigidbody = value;
        }

        public float GetBellyStandTime()
        {
            return bellyStandTime;
        }

        public void SetBellyStandTime(float value)
        {
            bellyStandTime = value;
        }

        public float GetBackStandTime()
        {
            return backStandTime;
        }

        public void SetBackStandTime(float value)
        {
            backStandTime = value;
        }

        public UnityEvent OnRagdollEvent()
        {
            return onRagdollEvent;
        }

        protected void OnRagdollEvent(UnityEvent value)
        {
            onRagdollEvent = value;
        }

        public UnityEvent OnGetUpEvent()
        {
            return onGetUpEvent;
        }

        protected void OnGetUpEvent(UnityEvent value)
        {
            onGetUpEvent = value;
        }

        public UnityEvent OnReadyToGoEvent()
        {
            return onReadyToGoEvent;
        }

        protected void OnReadyToGoEvent(UnityEvent value)
        {
            onReadyToGoEvent = value;
        }

        public float GetRagdollingEndTime()
        {
            return ragdollingEndTime;
        }

        public Vector3 GetStoredHipsPosition()
        {
            return storedHipsPosition;
        }

        public void SetStoredHipsPosition(Vector3 value)
        {
            storedHipsPosition = value;
        }

        public Vector3 GetStoredHipsPositionPrivAnim()
        {
            return storedHipsPositionPrivAnim;
        }

        public void SetStoredHipsPositionPrivAnim(Vector3 value)
        {
            storedHipsPositionPrivAnim = value;
        }

        public Vector3 GetStoredHipsPositionPrivBlend()
        {
            return storedHipsPositionPrivBlend;
        }

        public void SetStoredHipsPositionPrivBlend(Vector3 value)
        {
            storedHipsPositionPrivBlend = value;
        }
        #endregion
    }
}