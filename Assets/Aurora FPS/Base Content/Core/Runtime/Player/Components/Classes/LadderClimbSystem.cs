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
    [DisallowMultipleComponent]
    [RequireComponent(typeof(FPCController))]
    public class LadderClimbSystem : MonoBehaviour
    {
        [Header("Locomotion System")]
        [SerializeReference] private LadderClimbLocomotion ladderClimbLocomotion = new LadderClimbLocomotion();

        [Header("Ladder Check Settings")]
        [SerializeField] private float ladderCheckRange = 0.2f;
        [SerializeField] private float ladderCheckRadius = 0.1f;
        [SerializeField] private LayerMask cullingLayer = ~0;

        [Header("Weapon Setting")]
        [SerializeField] private bool hideOnLadder;
        [SerializeField] private bool getBackOnStop;

        [Header("Camera Settings")]
        [SerializeField] private Vector2 cameraSensitivity = new Vector2(10, 10);
        [SerializeField] private RangedFloat clampCameraVerticalRotation = new RangedFloat(-10, 75, -90, 90);
        [SerializeField] private RangedFloat clampCameraHorizontalRotation = new RangedFloat(-15, 15, -90, 90);
        [SerializeField] private bool clampHorizontalRotation;

        // Stored required components.
        private FPCController controller;
        private CameraControl cameraControl;
        private FPInventory inventory;
        private FootstepSoundSystem footstepSoundSystem;

        // Stored required properties.
        private bool isClimbing;
        private bool isRecentlyClimbed;
        private bool checkGrounded;
        private Vector2 storedCameraSensitivity;
        private RangedFloat storedClampCameraVerticalRotation;
        private RangedFloat storedClampCameraHorizontalRotation;
        private Vector3 storedTargetPosition;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            controller = GetComponent<FPCController>();
            inventory = GetComponent<FPInventory>();
            footstepSoundSystem = GetComponent<FootstepSoundSystem>();
            cameraControl = controller.GetCameraControl();
            ladderClimbLocomotion.Initialize(controller);
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void Update()
        {
            if (!isClimbing)
                CheckLadder();
            else if (isClimbing && IsJumpDown())
                StopClimb();
            else if (isClimbing && clampHorizontalRotation)
                cameraControl.ClampHorizontalRotation(storedClampCameraHorizontalRotation);

            CheckGroundHandler();
        }

        /// <summary>
        /// Start climbing on specific ladder.
        /// </summary>
        /// <param name="ladderTransform">Ladder transform component reference.</param>
        /// <param name="ladderCollider">Ladder collider component reference.</param>
        protected virtual void StartClimb(InteractiveLadder interactiveLadder, Collider ladderCollider)
        {
            if (hideOnLadder && inventory != null)
                inventory.HideWeapon();

            footstepSoundSystem?.IgnoreNextLand();
            SaveCameraSettings();
            CalculateHorizontalRotation(interactiveLadder);
            ApplyLocomotion(interactiveLadder, ladderCollider);
            storedTargetPosition = ladderClimbLocomotion.GetTargetPosition();
            isClimbing = true;
            checkGrounded = false;
        }

        protected virtual void StopClimb()
        {
            if (getBackOnStop && inventory != null)
                inventory.ActivateHiddenWeapon();
            LoadCameraSettings();
            ResetLocomotion();
            isClimbing = false;
            isRecentlyClimbed = true;
        }

        protected virtual void CheckLadder()
        {
            if (!isClimbing && controller.GetVerticalInput() > 0)
            {
                Vector3 origin = transform.position + controller.GetCharacterController().center;
                if (Physics.SphereCast(origin, ladderCheckRadius, transform.forward, out RaycastHit hit, ladderCheckRange + controller.GetCharacterController().radius, cullingLayer, QueryTriggerInteraction.Ignore))
                {
                    InteractiveLadder interactiveLadder = hit.transform.GetComponent<InteractiveLadder>();
                    if (interactiveLadder != null && !isRecentlyClimbed)
                    {
                        StartClimb(interactiveLadder, hit.collider);
                    }
                }
                else
                {
                    isRecentlyClimbed = false;
                }
            }
        }

        private void ApplyLocomotion(InteractiveLadder interactiveLadder, Collider ladderCollider)
        {
            controller.SetLocomotionType(FPCController.LocomotionType.Custom);
            ladderClimbLocomotion.SetLadder(interactiveLadder, ladderCollider);
            controller.SetCustomLocomotion(ladderClimbLocomotion);
            cameraControl.SetVerticalRotationLimits(clampCameraVerticalRotation);
            cameraControl.SetSensitivity(cameraSensitivity);
        }

        private void CalculateHorizontalRotation(InteractiveLadder interactiveLadder)
        {
            Quaternion offset = Quaternion.LookRotation(-interactiveLadder.transform.forward);
            offset *= Quaternion.Euler(interactiveLadder.rotationOffset);
            storedClampCameraHorizontalRotation = new RangedFloat(offset.eulerAngles.y + clampCameraHorizontalRotation.GetMin(), offset.eulerAngles.y + clampCameraHorizontalRotation.GetMax());
        }

        private void ResetLocomotion()
        {
            controller.SetLocomotionType(FPCController.LocomotionType.Movement);
            controller.SetCustomLocomotion(null);
        }

        protected virtual bool IsJumpDown()
        {
            return !isClimbing || AInput.GetButtonDown(INC.Jump) || (checkGrounded && controller.GetCharacterController().isGrounded) || Vector3.Distance(controller.transform.position, storedTargetPosition) <= 0.5f;
        }

        protected virtual void SaveCameraSettings()
        {
            storedCameraSensitivity = cameraControl.GetSensitivity();
            storedClampCameraVerticalRotation = cameraControl.GetVerticalRotationLimits();
        }

        public virtual void LoadCameraSettings()
        {
            cameraControl.SetSensitivity(storedCameraSensitivity);
            cameraControl.SetVerticalRotationLimits(storedClampCameraVerticalRotation);
        }

        private void CheckGroundHandler()
        {
            if (isClimbing && !checkGrounded && !controller.IsGrounded())
                checkGrounded = true;
        }

        #region [Getter / Setter]
        public LadderClimbLocomotion GetLadderClimbLocomotion()
        {
            return ladderClimbLocomotion;
        }

        public void SetLadderClimbLocomotion(LadderClimbLocomotion value)
        {
            ladderClimbLocomotion = value;
        }

        public float GetLadderCheckRange()
        {
            return ladderCheckRange;
        }

        public void SetLadderCheckRange(float value)
        {
            ladderCheckRange = value;
        }

        public float GetLadderCheckRadius()
        {
            return ladderCheckRadius;
        }

        public void SetLadderCheckRadius(float value)
        {
            ladderCheckRadius = value;
        }

        public LayerMask GetCullingLayer()
        {
            return cullingLayer;
        }

        public void SetCullingLayer(LayerMask value)
        {
            cullingLayer = value;
        }

        public bool GetHideOnLadder()
        {
            return hideOnLadder;
        }

        public void SetHideOnLadder(bool value)
        {
            hideOnLadder = value;
        }

        public bool GetGetBackOnStop()
        {
            return getBackOnStop;
        }

        public void SetGetBackOnStop(bool value)
        {
            getBackOnStop = value;
        }

        public Vector2 GetCameraSensitivity()
        {
            return cameraSensitivity;
        }

        public void SetCameraSensitivity(Vector2 value)
        {
            cameraSensitivity = value;
        }

        public RangedFloat GetClampCameraVerticalRotation()
        {
            return clampCameraVerticalRotation;
        }

        public void SetClampCameraVerticalRotation(RangedFloat value)
        {
            clampCameraVerticalRotation = value;
        }

        public RangedFloat GetClampCameraHorizontalRotation()
        {
            return clampCameraHorizontalRotation;
        }

        public void SetClampCameraHorizontalRotation(RangedFloat value)
        {
            clampCameraHorizontalRotation = value;
        }

        public bool ClampHorizontalRotation()
        {
            return clampHorizontalRotation;
        }

        public void ClampHorizontalRotation(bool value)
        {
            clampHorizontalRotation = value;
        }
        #endregion
    }
}

