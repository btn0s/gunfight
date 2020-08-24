/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using System;
using System.Collections;
using UnityEngine;

namespace AuroraFPSRuntime
{
    [RequireComponent(typeof(Animator))]
    public partial class WeaponAnimationSystem : MonoBehaviour, IWeaponAnimationSystem
    {
        // Base weapon animation properties.
        [Header("Take In/Out Time's")]
        [SerializeField] private float takeInTime = 1.0f;
        [SerializeField] private float takeOutTime = 1.0f;

        [Header("Animation Properties")]
        [SerializeField] private AnimatorValue movementParameter = "Movement";
        [SerializeField] private AnimatorValue isGroundedParameter = "IsGrounded";
        [SerializeField] private AnimatorValue isCrouchingParameter = "IsCrouching";
        [SerializeField] private AnimatorValue isSprintingParameter = "IsSprinting";
        [SerializeField] private AnimatorValue takeOutParameter = "TakeOut";
        [SerializeField] private AnimatorState zoomInState = "Zoom In";
        [SerializeField] private AnimatorState zoomOutState = "Zoom Out";
        [SerializeField] private float speedBlendTime = 9.0f;

        // Weapon sway properties.
        [Header("Sway Properties")]
        [SerializeField] WeaponSwaySystem swaySystem;
        [SerializeField] private bool swayEnabled = true;

        // Stored required components.
        private Animator animator;
        private FPController controller;
        private CameraControl cameraControl;

        // Stored transform properties.
        private Vector3 originalPosition;
        private Quaternion originalRotation;
        private Vector3 defaultPosition;
        private Quaternion defaultRotation;

        // Stored kickback properties.
        private KickbackProperty storedKickbackProperty;
        private Vector3 kickbackPosition;
        private Quaternion kickbackRotation;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            controller = transform.root.GetComponent<FPController>();
            cameraControl = controller.GetCameraControl();

            originalPosition = transform.localPosition;
            originalRotation = transform.localRotation;

            defaultPosition = originalPosition;
            defaultRotation = originalRotation;
            InitializeZoomAnimationCallback();
        }

        private void InitializeZoomAnimationCallback()
        {
            cameraControl.OnZoomCallback += (isZooming, fov) =>
            {
                if (isZooming)
                    animator.CrossFadeInFixedTime(zoomInState);
                else
                    animator.CrossFadeInFixedTime(zoomOutState);
            };
        }

        protected virtual void OnEnable()
        {
            if (cameraControl.IsZooming())
            {
                animator.CrossFadeInFixedTime(zoomInState);
            }
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void Update()
        {
            UpdateAnimatorParameters();

            KickbackHandler();
            SwaySystemHandler();

            ApplyPosition();
            ApplyRotation();
        }

        /// <summary>
        /// Apply transform local position to default weapon position.
        /// </summary>
        protected virtual void ApplyPosition()
        {
            transform.localPosition = defaultPosition;
        }

        /// <summary>
        /// Apply transform local rotation to default weapon position.
        /// </summary>
        protected virtual void ApplyRotation()
        {
            transform.localRotation = defaultRotation;
        }

        /// <summary>
        /// Update animator controller parameters.
        /// </summary>
        protected virtual void UpdateAnimatorParameters()
        {
            animator.SetFloat(movementParameter.GetNameHash(), controller.GetSpeed());
            animator.SetBool(isSprintingParameter.GetNameHash(), controller.GetSpeed() == controller.GetSprintSpeed());
            animator.SetBool(isGroundedParameter.GetNameHash(), controller.IsGrounded());
            animator.SetBool(isCrouchingParameter.GetNameHash(), controller.IsCrouched());
        }

        /// <summary>
        /// Add kickback effect.
        /// </summary>
        /// <param name="kickbackProperty"></param>
        public virtual void AddKickback(KickbackProperty kickbackProperty)
        {
            storedKickbackProperty = kickbackProperty;
            kickbackPosition = defaultPosition + kickbackProperty.GetKickbackPosition();
            kickbackRotation = defaultRotation * Quaternion.Euler(kickbackProperty.GetKickbackRotation());
        }

        /// <summary>
        /// Calculating added kickback property and apply to transform.
        /// </summary>
        protected virtual void KickbackHandler()
        {
            if (storedKickbackProperty != null)
            {
                float deltaTime = Time.deltaTime;

                kickbackPosition = Vector3.Lerp(kickbackPosition, originalPosition, storedKickbackProperty.GetPositionSpeedOut() * deltaTime);
                kickbackRotation = Quaternion.Slerp(kickbackRotation, originalRotation, storedKickbackProperty.GetRotationSpeedOut() * deltaTime);

                defaultPosition = Vector3.Lerp(defaultPosition, kickbackPosition, storedKickbackProperty.GetPositionSpeedUp() * deltaTime);
                defaultRotation = Quaternion.Slerp(defaultRotation, kickbackRotation, storedKickbackProperty.GetRotationSpeedUp() * deltaTime);
            }
        }

        /// <summary>
        /// Processing weapon sway system.
        /// </summary>
        protected virtual void SwaySystemHandler()
        {
            if (swayEnabled)
            {
                defaultPosition = swaySystem.GetSwayPosition(defaultPosition, originalPosition);
            }
        }

        /// <summary>
        /// Play take out animation.
        /// </summary>
        public void PlayTakeOutAnimation()
        {
            animator.SetTrigger(takeOutParameter.GetNameHash());
        }

        #region [Getter / Setter]
        public Vector3 GetOriginalPosition()
        {
            return originalPosition;
        }

        public void SetOriginalPosition(Vector3 value)
        {
            originalPosition = value;
        }

        public Quaternion GetOriginalRotation()
        {
            return originalRotation;
        }

        public void SetOriginalRotation(Quaternion value)
        {
            originalRotation = value;
        }

        public float GetTakeInTime()
        {
            return takeInTime;
        }

        public void SetTakeInTime(float value)
        {
            takeInTime = value;
        }

        public float GetTakeOutTime()
        {
            return takeOutTime;
        }

        public void SetTakeOutTime(float value)
        {
            takeOutTime = value;
        }

        public AnimatorValue GetSpeedParameter()
        {
            return movementParameter;
        }

        public void SetSpeedParameter(AnimatorValue value)
        {
            movementParameter = value;
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

        public AnimatorValue GetTakeOutParameter()
        {
            return takeOutParameter;
        }

        public void SetTakeOutParameter(AnimatorValue value)
        {
            takeOutParameter = value;
        }

        public float GetSpeedBlendTime()
        {
            return speedBlendTime;
        }

        public void SetSpeedBlendTime(float value)
        {
            speedBlendTime = value;
        }

        public WeaponSwaySystem GetWeaponSwaySystem()
        {
            return swaySystem;
        }

        public void SetWeaponSwaySystem(WeaponSwaySystem value)
        {
            swaySystem = value;
        }

        public bool UseWeaponSwaySystem()
        {
            return swayEnabled;
        }

        public void UseWeaponSwaySystem(bool value)
        {
            swayEnabled = value;
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

        protected void SetController(FPController value)
        {
            controller = value;
        }

        public CameraControl GetCameraControl()
        {
            return cameraControl;
        }

        protected void SetCameraControl(CameraControl value)
        {
            cameraControl = value;
        }
        #endregion
    }
}