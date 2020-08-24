/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using System;
using UnityEngine;

namespace AuroraFPSRuntime
{
    [RequireComponent(typeof(IController))]
    public class FPAdaptiveRagdoll : CharacterRagdoll
    {
        // Base first person ragdoll properties.
        [Header("First Person Properties")]
        [SerializeField] private float relativeVelocityLimit = 10.0f;
        [SerializeField] private float standDelay = 1.5f;
        [SerializeField] private Transform fullBody;
        [SerializeField] private GameObject[] firstPersonMeshes;
        [SerializeField] private GameObject[] fullBodyMeshes;
        [SerializeField] private FreeCamera freeCamera;
        [SerializeField] private ScreenFadeProperties screenFadeProperties = new ScreenFadeProperties(Color.black, 10.0f, 3.0f);

        // Stored required components.
        private Animator animator;
        private IController controller;
        private IControllerEnabled controllerEnabled;
        private ScreenFade screenFade;

        // Stored optional component.
        private CharacterHealth health;

        // Stored required properties.
        private float storedDelayTime;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            animator = fullBody.GetComponent<Animator>();
            controller = GetComponent<IController>();
            controllerEnabled = GetComponent<IControllerEnabled>();
            health = GetComponent<CharacterHealth>();

            screenFade = ScreenFade.Instance;
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        protected override void Start()
        {
            base.Start();
            freeCamera.Initialize();

            OnGetUpCallback += UpdateTransform;
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
            else if (RagdollStateIs(RagdollState.Ragdolled) || RagdollStateIs(RagdollState.BlendToAnim))
            {
                freeCamera.CameraLook();
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
                OnCollisionHitCallback?.Invoke(other);
            }
        }

        /// <summary>
        /// /// Switch components enabled state.
        /// </summary>
        protected override void SwitchComponentsEnabled(bool enabled)
        {
            base.SwitchComponentsEnabled(enabled);
            LayerMask remoteLayer = LayerMask.NameToLayer(LNC.RemoteBody);
            LayerMask defaultLayer = LayerMask.NameToLayer("Default");
            AuroraExtension.SetLayer(firstPersonMeshes, enabled ? defaultLayer : remoteLayer);
            AuroraExtension.SetLayer(fullBodyMeshes, enabled ? remoteLayer : defaultLayer);
            controllerEnabled.SetEnabled(enabled);
            screenFade.PingPongFade(screenFadeProperties);
        }

        private void UpdateTransform()
        {
            transform.position = fullBody.position + (Vector3.up * 1f);
            transform.rotation = Quaternion.LookRotation(fullBody.forward);

            fullBody.position = new Vector3(transform.position.x, fullBody.position.y, transform.position.z);
        }

        /// <summary>
        /// Controller velocity.
        /// </summary>
        public override Vector3 GetControllerVelocity()
        {
            return controller.GetVelocity();
        }

        /// <summary>
        /// Ragdoll body animator component.
        /// </summary>
        public override Animator GetBodyAnimator()
        {
            return animator;
        }

        /// <summary>
        /// Ragdoll body transform component.
        /// </summary>
        public override Transform GetBodyTransform()
        {
            return fullBody.transform;
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

        #region [Delegates callback]
        /// <summary>
        /// On collision hit callback function.
        /// OnCollisionHit is called when this collider/rigidbody has begun touching another rigidbody/collider 
        /// with relative velocity more/equal then relative velocity limit.
        /// </summary>
        /// <param name="Collision">The Collision data associated with this collision.</param>
        public Action<Collision> OnCollisionHitCallback;
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

        public Transform GetFullBody()
        {
            return fullBody;
        }

        public void SetFullBody(Transform value)
        {
            fullBody = value;
        }

        public GameObject[] GetFirstPersonMeshes()
        {
            return firstPersonMeshes;
        }

        public void SetFirstPersonMeshes(GameObject[] value)
        {
            firstPersonMeshes = value;
        }

        public GameObject[] GetFullBodyMeshes()
        {
            return fullBodyMeshes;
        }

        public void SetFullBodyMeshes(GameObject[] value)
        {
            fullBodyMeshes = value;
        }

        public FreeCamera GetFreeCamera()
        {
            return freeCamera;
        }

        public void SetFreeCamera(FreeCamera value)
        {
            freeCamera = value;
        }

        public ScreenFadeProperties GetScreenFadeProperties()
        {
            return screenFadeProperties;
        }

        public void SetScreenFadeProperties(ScreenFadeProperties value)
        {
            screenFadeProperties = value;
        }
        #endregion
    }
}