/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using System;
using System.Collections;
using UnityEngine;

namespace AuroraFPSRuntime
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(InputController))]
    public class FPCController : FPController
    {
        public enum LocomotionType
        {
            Movement,
            Custom
        }

        public enum MovementType
        {
            Walk,
            Run
        }

        [Header("Locomotion Settings")]
        [SerializeField] private MovementType movementType = MovementType.Run;
        [SerializeField] private float walkSpeed = 5.5f;
        [SerializeField] private float runSpeed = 7.5f;
        [SerializeField] private float sprintSpeed = 8.5f;
        [SerializeField] private float crouchSpeed = 3.25f;
        [SerializeField] private float zoomSpeed = 4.0f;
        [SerializeField] private float crouchZoomSpeed = 2.5f;
        [SerializeField] private float backwardsSpeedPercent = 0.5f;
        [SerializeField] private float sideSpeedPercent = 0.75f;

        [Header("Jump Settings")]
        [SerializeField] private float jumpForce = 7.5f;
        [SerializeField] private int jumpCount = 1;
        [SerializeField] private bool jumpInAir = false;

        [Header("Air Settings")]
        [SerializeField] private bool airControl = false;
        [Range(0.01f, 100.0f)] [SerializeField] private float airControlPersent = 1.0f;

        [Header("Acceleration Settings")]
        [SerializeField] private AccelerationDirection accelerationDirection = AccelerationDirection.Forward;
        [SerializeField] private float runVelocityThreshold = 0.0f;
        [SerializeField] private float sprintVelocityThreshold = 0.0f;
        [SerializeField] private AnimationCurve accelerationCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);

        [Header("Crouch Settings")]
        [SerializeField] private InputHandleType crouchActionType = InputHandleType.Hold;
        [SerializeField] private float crouchHeightPercent = 0.6f;
        [SerializeField] private float crouchDuration = 0.5f;
        [SerializeField] private AnimationCurve crouchCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);

        [Header("Sprint Effect")]
        [SerializeField] private FOVAnimationSettings sprintFOVAnimation;
        [SerializeField] private bool useSprintEffect;

        [Header("Headbob Effect")]
        [SerializeField] private CameraHeadBobEffect cameraHeadBobEffect;
        [SerializeField] private bool useHeadBobEffect = true;

        [Header("Landing Effect")]
        [SerializeField] private CameraLandingEffect cameraLandingEffect;
        [SerializeField] private bool useLandingEffect = true;

        [Header("Gravity Settings")]
        [SerializeField] private float gravityMultiplier = 2.5f;
        [SerializeField] private float stickToGroundForce = 5.0f;

        [Header("Grounded Settings")]
        [SerializeField] private LayerMask groundCullingLayer = ~0;
        [Range(0f, 1f)] [SerializeField] private float groundCheckRange = 0.1f;
        [Range(0.01f, 1f)] [SerializeField] private float groundCheckRadius = 0.2f;

        [Header("Roof Settings")]
        [SerializeField] private LayerMask roofCullingLayer = ~0;
        [SerializeField] private float roofCheckRadius = 0.1f;

        [Header("Check Wall Settings")]
        [SerializeField] private LayerMask wallCullingLayer = ~0;
        [SerializeField] private float wallCheckRange = 0.1f;
        [SerializeField] private float wallCheckRadius = 0.1f;

        [Header("Smooth Settings")]
        [Range(1f, 100.0f)] [SerializeField] private float smoothInputSpeed = 10.0f;
        [Range(1f, 100.0f)] [SerializeField] private float smoothVelocitySpeed = 10.0f;
        [Range(1f, 100.0f)] [SerializeField] private float smoothFinalDirectionSpeed = 20.0f;
        [Range(1f, 100.0f)] [SerializeField] private float smoothInputMagnitudeSpeed = 5.0f;

        // Controller action state.
        [SerializeField] private ControllerState controllerState = ControllerState.Idle;
        private CollisionFlags collisionFlags = CollisionFlags.None;

        // Controller locomotion properties.
        private LocomotionType locomotionType = LocomotionType.Movement;
        private bool cameraControlEnabled = true;
        private CustomLocomotion customLocomotion = null;

        // Stored required components.
        private CharacterController characterController;

        // Stored required properties.
        private RaycastHit groundHitInfo;
        private Vector2 inputVector;
        private Vector2 smoothInputVector;
        private Vector3 finalMoveDirection;
        private Vector3 smoothFinalMoveDirection;
        private Vector3 moveVector;
        private float currentSpeed;
        private float smoothCurrentSpeed;
        private float finalSmoothCurrentSpeed;
        private float defaultControllerHeight;
        private float crouchControllerHeight;
        private Vector3 defaultControllerCenter;
        private Vector3 crouchControllerCenter;
        private float defaultCameraHeight;
        private float crouchCameraHeight;
        private float crouchStandHeightDifference;
        private float inAirTime;
        private float inputVectorMagnitude;
        private float smoothInputVectorMagnitude;
        private float finalGroundedCheckRange;
        private bool isGrounded;
        private bool isJumped;
        private bool isCrouched;
        private bool previouslyGrounded;
        private CoroutineObject<bool> crouchCoroutineObject;
        private CoroutineObject<float> cameraLandingEffectCoroutineObject;
        private int storedJumpCount;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            characterController = GetComponent<CharacterController>();
            characterController.center = new Vector3(0.0f, characterController.height / 2f + characterController.skinWidth, 0.0f);

            defaultControllerCenter = characterController.center;
            defaultControllerHeight = characterController.height;

            crouchControllerHeight = defaultControllerHeight * crouchHeightPercent;
            crouchControllerCenter = (crouchControllerHeight / 2f + characterController.skinWidth) * Vector3.up;

            crouchStandHeightDifference = defaultControllerHeight - crouchControllerHeight;

            defaultCameraHeight = base.cameraControl.GetPivotTransform().localPosition.y;
            crouchCameraHeight = defaultCameraHeight - crouchStandHeightDifference;

            crouchCoroutineObject = new CoroutineObject<bool>(this);

            finalGroundedCheckRange = groundCheckRange + characterController.center.y;

            isCrouched = false;
            isGrounded = true;
            previouslyGrounded = true;

            InitializeHeadBobEffect();
            InitializeCameraLandingEffect();

            ExcludeCullingLayers(LNC.Player);
        }

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        protected virtual void OnEnable()
        {
            OnEnableCallback?.Invoke();
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void Update()
        {
            if (cameraControlEnabled)
            {
                CameraControlHandler();
            }

            ReadInput();
            ResetActions();

            CheckGrounded();

            SmoothingInputProcessing();
            SmoothingSpeedProcessing();
            SmoothingDirectionProcessing();
            SmoothingInputMagnitudeProcessing();

            CalculateDirection();
            CalculateSpeed();
            CalculateMoveVector();
            CalculateControllerState();

            ApplyGravity();
            ApplyLocomotion();

            SprintEffectHandler();
            GroundedCallbackHandler();
            JumpCounterHandler();
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        protected virtual void OnDisable()
        {
            OnDisableCallback?.Invoke();
        }

        /// <summary>
        /// Read input and save in Vector2D representation.
        /// </summary>
        protected virtual void ReadInput()
        {
            inputVector = new Vector2(AInput.GetAxis(INC.CharHorizontal), AInput.GetAxis(INC.CharVertical));
        }

        /// <summary>
        /// Calculate speed of the controller.
        /// </summary>
        protected virtual void CalculateSpeed()
        {
            if (inputVector == Vector2.zero)
            {
                currentSpeed = 0.0f;
                return;
            }

            switch (locomotionType)
            {
                case LocomotionType.Movement:
                    CalculateMovementSpeed();
                    break;
                case LocomotionType.Custom:
                    currentSpeed = customLocomotion?.CalculateSpeed() ?? 0;
                    break;
            }
        }

        /// <summary>
        /// Calculate speed of the controller, while active movement locomotion type.
        /// </summary>
        protected virtual void CalculateMovementSpeed()
        {
            switch (movementType)
            {
                case MovementType.Run:
                    currentSpeed = AInput.GetButton(INC.LightWalk) ? walkSpeed : runSpeed;
                    currentSpeed = AInput.GetButton(INC.Sprint) && CanAccelerate() ? sprintSpeed : currentSpeed;
                    break;
                case MovementType.Walk:
                    currentSpeed = AInput.GetButton(INC.Sprint) && CanAccelerate() ? runSpeed : walkSpeed;
                    break;
            }
            currentSpeed = isCrouched ? crouchSpeed : currentSpeed;
            currentSpeed = cameraControl.IsZooming() ? zoomSpeed : currentSpeed;
            currentSpeed = cameraControl.IsZooming() && isCrouched ? crouchZoomSpeed : currentSpeed;
            currentSpeed = inputVector.y < 0 ? currentSpeed * backwardsSpeedPercent : currentSpeed;
            currentSpeed = inputVector.x != 0 && inputVector.y == 0 ? currentSpeed * sideSpeedPercent : currentSpeed;
        }

        /// <summary>
        /// Calculate controller direction.
        /// </summary>
        protected virtual void CalculateDirection()
        {
            switch (locomotionType)
            {
                case LocomotionType.Movement:
                    finalMoveDirection = CalculateMovementDirection(smoothInputVector);
                    break;
                case LocomotionType.Custom:
                    finalMoveDirection = customLocomotion?.CalculateDirection(smoothInputVector) ?? Vector3.zero;
                    break;
            }

        }

        /// <summary>
        /// Calculate controller movement direction, while active movement locomotion type.
        /// </summary>
        /// <param name="smoothInputVector">Calculated and smoothed input data Vector2 representation.</param>
        /// <returns>Calculated controller move direction.</returns>
        protected virtual Vector3 CalculateMovementDirection(Vector2 smoothInputVector)
        {
            Vector3 verticalDirection = transform.forward * smoothInputVector.y;
            Vector3 horizontalDirection = transform.right * smoothInputVector.x;

            Vector3 desiredDirection = verticalDirection + horizontalDirection;
            return FlattenVectorOnSlopes(desiredDirection);
        }

        /// <summary>
        /// Calculate final character controller move vector.
        /// </summary>
        protected virtual void CalculateMoveVector()
        {
            Vector3 finalVector = smoothFinalMoveDirection * finalSmoothCurrentSpeed * smoothInputVectorMagnitude;

            switch (locomotionType)
            {
                case LocomotionType.Movement:
                    moveVector = CalculateMovementVector(moveVector, finalVector);
                    break;
                case LocomotionType.Custom:
                    moveVector = customLocomotion?.CalculateMoveVector(moveVector, finalVector) ?? Vector3.zero;
                    break;
            }
        }

        /// <summary>
        /// Calculate final character controller move vector, while active movement locomotion type.
        /// </summary>
        /// <param name="currentMoveVector">Current character controller move vertor.</param>
        /// <param name="nextMoveVector">Calculated and smoothed next character controller move vector.</param>
        /// <returns>Final character controller move vector.</returns>
        protected virtual Vector3 CalculateMovementVector(Vector3 currentMoveVector, Vector3 nextMoveVector)
        {
            if (characterController.isGrounded)
            {
                currentMoveVector.x = nextMoveVector.x;
                currentMoveVector.z = nextMoveVector.z;
                currentMoveVector.y += nextMoveVector.y;
            }
            else if (airControl)
            {
                float y = currentMoveVector.y;
                float smooth = inputVector != Vector2.zero ? airControlPersent : 0.01f;
                currentMoveVector = Vector3.Lerp(currentMoveVector, nextMoveVector, smooth * Time.deltaTime);
                currentMoveVector.y = y;
            }
            return currentMoveVector;
            
        }

        /// <summary>
        /// Calculate actual controller state.
        /// </summary>
        protected virtual void CalculateControllerState()
        {
            if(currentSpeed != 0 && isGrounded)
            {
                controllerState &= ~ControllerState.Idle;
                controllerState &= ~ControllerState.InAir;

                if (currentSpeed == walkSpeed ||
                    currentSpeed == walkSpeed * backwardsSpeedPercent ||
                    currentSpeed == walkSpeed * sideSpeedPercent)
                    controllerState |= ControllerState.Walking;
                else
                    controllerState &= ~ControllerState.Walking;

                if (currentSpeed == runSpeed ||
                    currentSpeed == runSpeed * backwardsSpeedPercent ||
                    currentSpeed == runSpeed * sideSpeedPercent)
                    controllerState |= ControllerState.Running;
                else
                    controllerState &= ~ControllerState.Running;

                if (CanAccelerate() && currentSpeed == sprintSpeed ||
                    currentSpeed == sprintSpeed * backwardsSpeedPercent ||
                    currentSpeed == sprintSpeed * sideSpeedPercent)
                    controllerState |= ControllerState.Sprinting;
                else
                    controllerState &= ~ControllerState.Sprinting;
            }
            else if(isGrounded)
            {
                controllerState = ControllerState.Idle;
            }
            else
            {
                controllerState = ControllerState.InAir;
            }

            if (isCrouched)
                controllerState |= ControllerState.Crouched;
            else
                controllerState &= ~ControllerState.Crouched;

            if (isJumped)
                controllerState |= ControllerState.Jumped;
            else
                controllerState &= ~ControllerState.Jumped;

            if (cameraControl.IsZooming())
                controllerState |= ControllerState.Zooming;
            else
                controllerState &= ~ControllerState.Zooming;
        }

        /// <summary>
        /// Smoothing input vector.
        /// </summary>
        protected virtual void SmoothingInputProcessing()
        {
            inputVector = inputVector.normalized;
            smoothInputVector = Vector2.Lerp(smoothInputVector, inputVector, Time.deltaTime * smoothInputSpeed);
        }

        /// <summary>
        /// Smoothing controller speed value.
        /// </summary>
        protected virtual void SmoothingSpeedProcessing()
        {
            smoothCurrentSpeed = Mathf.Lerp(smoothCurrentSpeed, currentSpeed, Time.deltaTime * smoothVelocitySpeed);
            if (AInput.GetButton(INC.Sprint) && CanAccelerate())
            {
                float accelerateSpeed = movementType == MovementType.Run ? sprintSpeed : runSpeed;
                float acceleratePersent = Mathf.InverseLerp(currentSpeed, accelerateSpeed, smoothCurrentSpeed);
                finalSmoothCurrentSpeed = accelerationCurve.Evaluate(acceleratePersent) * (accelerateSpeed - currentSpeed) + currentSpeed;
            }
            else
            {
                finalSmoothCurrentSpeed = smoothCurrentSpeed;
            }
        }

        /// <summary>
        /// Smoothing controller direction vector.
        /// </summary>
        protected virtual void SmoothingDirectionProcessing()
        {
            smoothFinalMoveDirection = Vector3.Lerp(smoothFinalMoveDirection, finalMoveDirection, Time.deltaTime * smoothFinalDirectionSpeed);
        }

        /// <summary>
        /// Smoothing input magnitude value. 
        /// </summary>
        protected virtual void SmoothingInputMagnitudeProcessing()
        {
            inputVectorMagnitude = inputVector.magnitude;
            smoothInputVectorMagnitude = Mathf.Lerp(smoothInputVectorMagnitude, inputVectorMagnitude, Time.deltaTime * smoothInputMagnitudeSpeed);
        }

        /// <summary>
        /// OnControllerColliderHit is called when the controller hits a collider while performing a Move.
        /// </summary>
        /// <param name="hit">Detailed information about the collision and how to deal with it.</param>
        protected virtual void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;

            if (collisionFlags == CollisionFlags.Below || body == null || body.isKinematic)
                return;

            body.AddForceAtPosition(characterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
        }

        /// <summary>
        /// Checks whether there is an obstacle in the form of a wall in front of the controller.
        /// </summary>
        public virtual bool HasWallObstacle()
        {
            Vector3 origin = transform.position + characterController.center;
            return Physics.Raycast(origin, finalMoveDirection, wallCheckRange + characterController.radius, wallCullingLayer, QueryTriggerInteraction.Ignore);
        }

        /// <summary>
        /// Checks whether there is an obstacle in the form of a ceiling above the controller.
        /// </summary>
        /// <returns></returns>
        public virtual bool HasRoofObstacle()
        {
            Vector3 origin = transform.position + (Vector3.up * defaultControllerHeight);
            return Physics.CheckSphere(origin, roofCheckRadius, roofCullingLayer, QueryTriggerInteraction.Ignore);
        }

        /// <summary>
        /// Checks whether the controller can accelerate at the current moment.
        /// </summary>
        /// <returns></returns>
        public virtual bool CanAccelerate()
        {
            float threshold = movementType == MovementType.Run ? sprintVelocityThreshold : runVelocityThreshold;
            return threshold <= GetVelocity().sqrMagnitude && !AInput.GetButton(INC.Crouch) && !cameraControl.IsZooming() && CheckAccelerationDirection();
        }

        /// <summary>
        /// Check that acceleration direction is correct.
        /// </summary>
        /// <returns>True, if the current direction satisfies the direction in which controller can accelerate. 
        /// Otherwise false.</returns>
        public bool CheckAccelerationDirection()
        {
            if (accelerationDirection == AccelerationDirection.All)
                return true;
            else if (accelerationDirection == AccelerationDirection.Forward && (inputVector.y > 0 && inputVector.x == 0))
                return true;
            else if (accelerationDirection == AccelerationDirection.Backword && (inputVector.y < 0 && inputVector.x == 0))
                return true;
            else if (accelerationDirection == AccelerationDirection.Side && (inputVector.x != 0 && inputVector.y == 0))
                return true;
            else if ((accelerationDirection == (AccelerationDirection.Forward | AccelerationDirection.Backword)) && (inputVector.y != 0 && inputVector.x == 0))
                return true;
            else if ((accelerationDirection == (AccelerationDirection.Forward | AccelerationDirection.Side)) && inputVector.y > 0)
                return true;
            else if ((accelerationDirection == (AccelerationDirection.Backword | AccelerationDirection.Side)) && inputVector.y < 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Flatten direction vector on slopes.
        /// </summary>
        /// <param name="vectorToFlatten">Desired direction vector.</param>
        /// <returns>Flatten vector.</returns>
        public Vector3 FlattenVectorOnSlopes(Vector3 vectorToFlatten)
        {
            if (isGrounded)
            {
                vectorToFlatten = Vector3.ProjectOnPlane(vectorToFlatten, groundHitInfo.normal);
            }
            else if (locomotionType == LocomotionType.Custom && (!customLocomotion?.CheckGround() ?? true))
            {
                Vector3 origin = transform.position + characterController.center;
                if (Physics.SphereCast(origin, groundCheckRadius, Vector3.down, out groundHitInfo, finalGroundedCheckRange, groundCullingLayer))
                {
                    vectorToFlatten = Vector3.ProjectOnPlane(vectorToFlatten, groundHitInfo.normal);
                }
            }

            return vectorToFlatten;
        }

        /// <summary>
        /// Handle controller sprint.
        /// </summary>
        protected virtual void HandleSprint()
        {
            if (AInput.GetButtonDown(INC.Sprint))
            {
                Crouch(false);
            }
        }

        /// <summary>
        /// Handle controller jumping.
        /// </summary>
        protected virtual void HandleJump()
        {
            if ((jumpInAir || (!jumpInAir && isGrounded)) &&
                storedJumpCount < jumpCount &&
                AInput.GetButtonDown(INC.Jump) &&
                !isCrouched)
            {
                moveVector.y = jumpForce;
                isJumped = true;
                storedJumpCount++;
                OnJumpCallback?.Invoke();
            }
        }

        /// <summary>
        /// Handle controller crouching.
        /// </summary>
        protected virtual void HandleCrouch()
        {
            if (!isCrouched && AInput.GetButtonDown(INC.Crouch) && !AInput.GetButton(INC.Sprint))
            {
                Crouch(true);
            }
            else if (isCrouched &&
                ((crouchActionType == InputHandleType.Trigger && AInput.GetButtonDown(INC.Crouch)) ||
                (crouchActionType == InputHandleType.Hold && AInput.GetButtonUp(INC.Crouch))) &&
                !HasRoofObstacle())
            {
                Crouch(false);
            }
        }

        /// <summary>
        /// Switch controller crouching state.
        /// </summary>
        /// <param name="crouch">New crouching state for controller</param>
        public void Crouch(bool crouch)
        {
            crouchCoroutineObject.Start(CrouchProcessing, crouch, true);
        }

        /// <summary>
        /// Applying physics gravity to controller.
        /// </summary>
        protected virtual void ApplyGravity()
        {
            if (locomotionType == LocomotionType.Movement ||
                (locomotionType == LocomotionType.Custom && (customLocomotion?.UseGravity() ?? false)))
            {
                if (characterController.isGrounded)
                {
                    moveVector.y = -stickToGroundForce;

                    HandleSprint();
                    HandleCrouch();
                }
                else if (HasRoofObstacle())
                {
                    moveVector.y -= stickToGroundForce;
                }
                else
                {
                    moveVector += Physics.gravity * gravityMultiplier * Time.deltaTime;
                }
                HandleJump();
            }
        }

        /// <summary>
        /// Applying movement vector to controller.
        /// </summary>
        protected virtual void ApplyLocomotion()
        {
            collisionFlags = characterController.Move(moveVector * Time.deltaTime);
            OnMoveCallback?.Invoke(characterController.velocity);
        }

        /// <summary>
        /// Manually check of controller grounded.
        /// </summary>
        protected virtual void CheckGrounded()
        {
            if (locomotionType == LocomotionType.Movement ||
                (locomotionType == LocomotionType.Custom && (customLocomotion?.CheckGround() ?? false)))
            {
                Vector3 origin = transform.position + characterController.center;
                isGrounded = Physics.SphereCast(origin, groundCheckRadius, Vector3.down, out groundHitInfo, finalGroundedCheckRange, groundCullingLayer);
            }
        }

        /// <summary>
        /// Reset specific variables on default value after use.
        /// </summary>
        protected virtual void ResetActions()
        {
            isJumped = false;
        }

        /// <summary>
        /// Exclude specific layers from all culling layers, which used in controller.
        /// </summary>
        /// <param name="excludeLayers">Layers to exclude.</param>
        protected virtual void ExcludeCullingLayers(params string[] excludeLayers)
        {
            if (excludeLayers != null)
            {
                for (int i = 0; i < excludeLayers.Length; i++)
                {
                    int layer = LayerMask.NameToLayer(excludeLayers[i]);
                    groundCullingLayer = groundCullingLayer.ExcludeLayer(layer);
                    roofCullingLayer = roofCullingLayer.ExcludeLayer(layer);
                    wallCullingLayer = wallCullingLayer.ExcludeLayer(layer);
                }
            }
        }

        /// <summary>
        /// Processing camera rotation.
        /// </summary>
        protected virtual void CameraControlHandler()
        {
            cameraControl.Update();

            if (useHeadBobEffect)
                HeadBobHandler();
        }

        /// <summary>
        /// Crouch coroutine processing.
        /// </summary>
        protected virtual IEnumerator CrouchProcessing(bool crouch)
        {
            isCrouched = crouch;

            float time = 0.0f;
            float speed = 1.0f / (crouchDuration > 0 ? crouchDuration : 0.01f);

            float desiredHeight = isCrouched ? crouchControllerHeight : defaultControllerHeight;
            Vector3 desiredCenter = isCrouched ? crouchControllerCenter : defaultControllerCenter;

            Vector3 cameraPosition = base.cameraControl.GetPivotTransform().localPosition;
            float cameraDesiredHeight = isCrouched ? crouchCameraHeight : defaultCameraHeight;

            while (time < 1.0f)
            {
                time += Time.deltaTime * speed;
                float smoothTime = crouchCurve.Evaluate(time);

                characterController.height = Mathf.Lerp(characterController.height, desiredHeight, smoothTime);
                characterController.center = Vector3.Lerp(characterController.center, desiredCenter, smoothTime);

                cameraPosition.y = Mathf.Lerp(cameraPosition.y, cameraDesiredHeight, smoothTime);
                base.cameraControl.GetPivotTransform().localPosition = cameraPosition;
                yield return null;
            }
        }

        /// <summary>
        /// Playing sprint effect, when controller start sprinting.
        /// </summary>
        public virtual void SprintEffectHandler()
        {
            if(cameraControlEnabled && useSprintEffect)
            {
                if (AInput.GetButtonDown(INC.Sprint) && currentSpeed == sprintSpeed)
                {
                    cameraControl.ChangeFieldOfView(sprintFOVAnimation, true);
                }
                else if (AInput.GetButtonUp(INC.Sprint))
                {
                    cameraControl.ChangeFieldOfView(cameraControl.GetDefaultFOVSettings(), true);
                }
            }
        }

        /// <summary>
        /// Initialize camera landing effect.
        /// </summary>
        protected void InitializeCameraLandingEffect()
        {
            cameraLandingEffect.Initialize(base.cameraControl.GetPivotTransform());
            cameraLandingEffectCoroutineObject = new CoroutineObject<float>(this);

            OnJumpCallback += () => inAirTime = Time.time;
            OnGroundedCallback += () => { if (useLandingEffect) PlayLandingEffect(Time.time - inAirTime); };
        }

        /// <summary>
        /// Play camera landing effect.
        /// </summary>
        /// <param name="amount">Landing effect amount.</param>
        public void PlayLandingEffect(float amount)
        {
            cameraLandingEffectCoroutineObject.Start(cameraLandingEffect.PlayEffect, amount);
        }

        /// <summary>
        /// Initialize camera head bob effect
        /// </summary>
        protected void InitializeHeadBobEffect()
        {
            cameraHeadBobEffect.Initialize(base.cameraControl.GetPivotTransform(), backwardsSpeedPercent, sideSpeedPercent);
        }

        /// <summary>
        /// Camera head bob effect handler.
        /// </summary>
        protected void HeadBobHandler()
        {
            if (isGrounded && !HasWallObstacle() && !cameraLandingEffectCoroutineObject.IsProcessing())
            {
                cameraHeadBobEffect.SetCameraHeight(isCrouched ? crouchCameraHeight : defaultCameraHeight);
                cameraHeadBobEffect.ScrollHeadBob(inputVector, controllerState);
            }
        }

        /// <summary>
        /// OnGroundedCallback handler.
        /// </summary>
        protected void GroundedCallbackHandler()
        {
            if (!previouslyGrounded && isGrounded)
            {
                OnGroundedCallback?.Invoke();
            }
            previouslyGrounded = isGrounded;
        }

        /// <summary>
        /// Handle stored jump counter.
        /// </summary>
        protected void JumpCounterHandler()
        {
            if (isGrounded && storedJumpCount > 0)
                storedJumpCount = 0;
        }

        #region [IControllerBase Implementation]
        public override Vector3 GetVelocity()
        {
            return characterController.velocity;
        }
        #endregion

        #region [IcontrollerSpeed Implementation]
        public override float GetSpeed()
        {
            return currentSpeed;
        }

        public override float GetWalkSpeed()
        {
            return walkSpeed;
        }

        public override float GetRunSpeed()
        {
            return runSpeed;
        }

        public override float GetSprintSpeed()
        {
            return sprintSpeed;
        }

        public override float GetBackwardSpeedPersent()
        {
            return backwardsSpeedPercent;
        }

        public override float GetSideSpeedPersent()
        {
            return sideSpeedPercent;
        }
        #endregion

        #region [IControllerJump Implementation]
        public override bool IsJumped()
        {
            return isJumped;
        }
        #endregion

        #region [ControllerState Implementation]
        public override ControllerState GetState()
        {
            return controllerState;
        }

        public override bool CompareState(ControllerState value)
        {
            return controllerState == value;
        }

        public override bool HasState(ControllerState value)
        {
            return (controllerState & value) == value;
        }
        #endregion

        #region [IControllerInput Implementation]
        public override Vector2 GetMovementInput()
        {
            return inputVector;
        }

        public override float GetHorizontalInput()
        {
            return AInput.GetAxis(INC.CharHorizontal);
        }

        public override float GetVerticalInput()
        {
            return AInput.GetAxis(INC.CharVertical);
        }
        #endregion

        #region [IControllerEnabled Implementation]
        public override bool IsEnabled()
        {
            return enabled;
        }

        public override void SetEnabled(bool enabled)
        {
            this.enabled = enabled;
            characterController.enabled = enabled;
        }
        #endregion

        #region [IControllerGrounded Implementation]
        public override bool IsGrounded()
        {
            return isGrounded;
        }
        #endregion

        #region [IControllerCrouched Implementation]
        public override bool IsCrouched()
        {
            return isCrouched;
        }
        #endregion

        #region [IControllerCallbacks Implementation]
        /// <summary>
        /// Called when controller moved.
        /// <param name="Vector3">Velocity of the controller.</param>
        /// </summary>
        public override event Action<Vector3> OnMoveCallback;

        /// <summary>
        /// Called when controller being grounded.
        /// </summary>
        public override event Action OnGroundedCallback;

        /// <summary>
        /// Called when controller jumped.
        /// </summary>
        public override event Action OnJumpCallback;

        /// <summary>
        /// Called when controller being enabled.
        /// </summary>
        public override event Action OnEnableCallback;

        /// <summary>
        /// Called when controller being disabled.
        /// </summary>
        public override event Action OnDisableCallback;
        #endregion

        #region [Getter / Setter]
        public MovementType GetMovementType()
        {
            return movementType;
        }

        public void SetMovementType(MovementType value)
        {
            movementType = value;
        }

        public void SetWalkSpeed(float value)
        {
            walkSpeed = value;
        }

        public void SetRunSpeed(float value)
        {
            runSpeed = value;
        }

        public void SetSprintSpeed(float value)
        {
            sprintSpeed = value;
        }

        public float GetCrouchSpeed()
        {
            return crouchSpeed;
        }

        public void SetCrouchSpeed(float value)
        {
            crouchSpeed = value;
        }

        public float GetZoomSpeed()
        {
            return zoomSpeed;
        }

        public void SetZoomSpeed(float value)
        {
            zoomSpeed = value;
        }

        public float GetCrouchZoomSpeed()
        {
            return crouchZoomSpeed;
        }

        public void SetCrouchZoomSpeed(float value)
        {
            crouchZoomSpeed = value;
        }

        public float GetBackwardsSpeedPercent()
        {
            return backwardsSpeedPercent;
        }

        public void SetBackwardsSpeedPercent(float value)
        {
            backwardsSpeedPercent = value;
        }

        public float GetSideSpeedPercent()
        {
            return sideSpeedPercent;
        }

        public void SetSideSpeedPercent(float value)
        {
            sideSpeedPercent = value;
        }

        public float GetJumpForce()
        {
            return jumpForce;
        }

        public void SetJumpForce(float value)
        {
            jumpForce = value;
        }

        public int GetJumpCount()
        {
            return jumpCount;
        }

        public void SetJumpCount(int value)
        {
            jumpCount = value;
        }

        public bool JumpInAir()
        {
            return jumpInAir;
        }

        public void JumpInAir(bool value)
        {
            jumpInAir = value;
        }

        public bool InAirControl()
        {
            return airControl;
        }

        public void InAirControl(bool value)
        {
            airControl = value;
        }

        public float GetInAirControlPersent()
        {
            return airControlPersent;
        }

        public void SetInAirControlPersent(float value)
        {
            airControlPersent = value;
        }

        public AccelerationDirection GetAccelerationDirection()
        {
            return accelerationDirection;
        }

        public void SetAccelerationDirection(AccelerationDirection value)
        {
            accelerationDirection = value;
        }

        public float GetRunVelocityThreshold()
        {
            return runVelocityThreshold;
        }

        public void SetRunVelocityThreshold(float value)
        {
            runVelocityThreshold = value;
        }

        public float GetSprintVelocityThreshold()
        {
            return sprintVelocityThreshold;
        }

        public void SetSprintVelocityThreshold(float value)
        {
            sprintVelocityThreshold = value;
        }

        public AnimationCurve GetAccelerationCurve()
        {
            return accelerationCurve;
        }

        public void SetAccelerationCurve(AnimationCurve value)
        {
            accelerationCurve = value;
        }

        public InputHandleType GetCrouchHandleType()
        {
            return crouchActionType;
        }

        public void SetCrouchHandleType(InputHandleType value)
        {
            crouchActionType = value;
        }

        public float GetCrouchHeightPercent()
        {
            return crouchHeightPercent;
        }

        public void SetCrouchHeightPercent(float value)
        {
            crouchHeightPercent = value;
        }

        public float GetCrouchDuration()
        {
            return crouchDuration;
        }

        public void SetCrouchDuration(float value)
        {
            crouchDuration = value;
        }

        public AnimationCurve GetCrouchCurve()
        {
            return crouchCurve;
        }

        public void SetCrouchCurve(AnimationCurve value)
        {
            crouchCurve = value;
        }

        public FOVAnimationSettings GetSprintFOVAnimation()
        {
            return sprintFOVAnimation;
        }

        public void SetSprintFOVAnimation(FOVAnimationSettings value)
        {
            sprintFOVAnimation = value;
        }

        public bool UseSprintEffect()
        {
            return useSprintEffect;
        }

        public void UseSprintEffect(bool value)
        {
            useSprintEffect = value;
        }

        public CameraHeadBobEffect GetCameraHeadBobEffect()
        {
            return cameraHeadBobEffect;
        }

        public void SetCameraHeadBobEffect(CameraHeadBobEffect value)
        {
            cameraHeadBobEffect = value;
        }

        public bool UseHeadBobEffect()
        {
            return useHeadBobEffect;
        }

        public void UseHeadBobEffect(bool value)
        {
            useHeadBobEffect = value;
        }

        public CameraLandingEffect GetCameraLandingEffect()
        {
            return cameraLandingEffect;
        }

        public void SetCameraLandingEffect(CameraLandingEffect value)
        {
            cameraLandingEffect = value;
        }

        public bool UseLandingEffect()
        {
            return useLandingEffect;
        }

        public void UseLandingEffect(bool value)
        {
            useLandingEffect = value;
        }

        public float GetGravityMultiplier()
        {
            return gravityMultiplier;
        }

        public void SetGravityMultiplier(float value)
        {
            gravityMultiplier = value;
        }

        public float GetStickToGroundForce()
        {
            return stickToGroundForce;
        }

        public void SetStickToGroundForce(float value)
        {
            stickToGroundForce = value;
        }

        public LayerMask GetGroundCullingLayer()
        {
            return groundCullingLayer;
        }

        public void SetGroundCullingLayer(LayerMask value)
        {
            groundCullingLayer = value;
        }

        public float GetGroundCheckRange()
        {
            return groundCheckRange;
        }

        public void SetGroundCheckRange(float value)
        {
            groundCheckRange = value;
        }

        public float GetGroundCheckRadius()
        {
            return groundCheckRadius;
        }

        public void SetGroundCheckRadius(float value)
        {
            groundCheckRadius = value;
        }

        public LayerMask GetRoofCullingLayer()
        {
            return roofCullingLayer;
        }

        public void SetRoofCullingLayer(LayerMask value)
        {
            roofCullingLayer = value;
        }

        public float GetRoofCheckRadius()
        {
            return roofCheckRadius;
        }

        public void SetRoofCheckRadius(float value)
        {
            roofCheckRadius = value;
        }

        public LayerMask GetWallCullingLayer()
        {
            return wallCullingLayer;
        }

        public void SetWallCullingLayer(LayerMask value)
        {
            wallCullingLayer = value;
        }

        public float GetWallCheckRange()
        {
            return wallCheckRange;
        }

        public void SetWallCheckRange(float value)
        {
            wallCheckRange = value;
        }

        public float GetWallCheckRadius()
        {
            return wallCheckRadius;
        }

        public void SetWallCheckRadius(float value)
        {
            wallCheckRadius = value;
        }

        public float GetSmoothInputSpeed()
        {
            return smoothInputSpeed;
        }

        public void SetSmoothInputSpeed(float value)
        {
            smoothInputSpeed = value;
        }

        public float GetSmoothVelocitySpeed()
        {
            return smoothVelocitySpeed;
        }

        public void SetSmoothVelocitySpeed(float value)
        {
            smoothVelocitySpeed = value;
        }

        public float GetSmoothFinalDirectionSpeed()
        {
            return smoothFinalDirectionSpeed;
        }

        public void SetSmoothFinalDirectionSpeed(float value)
        {
            smoothFinalDirectionSpeed = value;
        }

        public float GetSmoothInputMagnitudeSpeed()
        {
            return smoothInputMagnitudeSpeed;
        }

        public void SetSmoothInputMagnitudeSpeed(float value)
        {
            smoothInputMagnitudeSpeed = value;
        }

        public ControllerState GetControllerState()
        {
            return controllerState;
        }

        protected void SetControllerState(ControllerState value)
        {
            controllerState = value;
        }

        public CollisionFlags GetCollisionFlags()
        {
            return collisionFlags;
        }

        public LocomotionType GetLocomotionType()
        {
            return locomotionType;
        }

        public void SetLocomotionType(LocomotionType value)
        {
            locomotionType = value;
        }

        public bool CameraControlEnabled()
        {
            return cameraControlEnabled;
        }

        public void CameraControlEnabled(bool value)
        {
            cameraControlEnabled = value;
        }

        public CustomLocomotion GetCustomLocomotion()
        {
            return customLocomotion;
        }

        public void SetCustomLocomotion(CustomLocomotion value)
        {
            customLocomotion = value;
        }

        public CharacterController GetCharacterController()
        {
            return characterController;
        }

        protected void SetCharacterController(CharacterController value)
        {
            characterController = value;
        }
        #endregion
    }
}
