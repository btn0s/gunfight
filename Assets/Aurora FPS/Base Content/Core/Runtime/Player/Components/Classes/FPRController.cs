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
using UnityEngine.Events;

namespace AuroraFPSRuntime
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(AudioSource))]
    public class FPRController : FPController
    {
        /// <summary>
        /// First person rigidbody controller movement mode.
        /// </summary>
        public enum MovementMode
        {
            /// <summary>
            /// Proceed controller movement by add force method.
            /// </summary>
            AddForce,

            /// <summary>
            /// Proceed controller movement by changing velocity.
            /// </summary>
            Velocity
        }

        /// <summary>
        /// First person controller climbing types.
        /// </summary>
        public enum ClimbMovement
        {
            /// <summary>
            /// Controller can climbing in vertical and horizontal directions.
            /// </summary>
            Free,

            /// <summary>
            /// Controller can climb only in vertical directions.
            /// </summary>
            Vertical
        }

        // Movement properties.
        [Header("Movement Properties")]
        [SerializeField] private MovementMode movementMode = MovementMode.Velocity;
        [SerializeField] private float runSpeed = 5.0f;
        [SerializeField] private float walkSpeed = 3.0f;
        [SerializeField] private float sprintSpeed = 7.0f;
        [SerializeField] private float backwardSpeedPersent = 0.5f;
        [SerializeField] private float sideSpeedPersent = 0.75f;
        [SerializeField] private float zoomSpeedPersent = 0.75f;
        [SerializeField] private SprintDirection sprintDirection = SprintDirection.Forward;
        [SerializeField] private bool gradualAcceleration = true;
        [SerializeField] private float maxStepOffsetHeight = 0.5f;
        [SerializeField] private float stepOffsetCheckRange = 1.5f;
        [SerializeField] private float stepOffsetSmooth = 10.0f;

        [SerializeField] private AnimationCurve slopeCurveModifier = DefaultSlopeCurveModifier();
        [SerializeField] private float groundCheckDistance = 0.25f;
        [SerializeField] private float stickToGroundHelperDistance = 0.5f;
        [SerializeField] private float shellOffset = 0.1f;

        // Jump properties.
        [Header("Jump Properties")]
        [SerializeField] private AirControl airControl = AirControl.Disabled;
        [SerializeField] private float jumpForce = 50.0f;
        [SerializeField] private float airControlSpeed = 2.0f;
        [SerializeField] private int maxJumpCount = 1;
        [SerializeField] private float multipleJumpForce = 50.0f;

        // Crouch properties.
        [Header("Crouch Properties")]
        [SerializeField] private InputHandleType crouchHandleType = InputHandleType.Hold;
        [SerializeField] private float crouchWalkSpeed = 2.0f;
        [SerializeField] private float crouchSpeed = 7.0f;
        [SerializeField] private float crouchHeight = 1.25f;

        // Climb properties.
        [Header("Climb Properties")]
        [SerializeField] private ClimbMovement climbMovement = ClimbMovement.Vertical;
        [SerializeField] private float climbSpeed = 2.0f;
        [SerializeField] private bool climbCameraLock = true;
        [SerializeField] private float downThreshold = 0.2f;
        [SerializeField] private float climbSoundRate = 0.5f;
        [SerializeField] private AudioClip climbSound;
        [SerializeField] private float climbCheckRate = 0.25f;
        [SerializeField] private LayerMask climbCullingLayer = 0;

        [SerializeField] private UnityEvent onEnableEvent;
        [SerializeField] private UnityEvent onDisableEvent;
        [SerializeField] private UnityEvent onGroundedEvent;

        // Controller action state.
        private ControllerState controllerState = ControllerState.Idle;

        // Controller actions.
        private bool isJump;
        private bool isJumping;
        private bool isCrouching;
        private bool isClimbing;
        private bool isGrounded;

        // Stored physics materials.     
        private PhysicMaterial frictionPhysics;
        private PhysicMaterial maxFrictionPhysics;
        private PhysicMaterial slippyPhysics;

        // Stored required components.
        private Rigidbody controllerRigidbody;
        private CapsuleCollider capsuleCollider;
        private AudioSource audioSource;
        private Transform cameraTransform;
        private Transform storedLadderTransform;

        // Stored required properties.
        private float speed;
        private float storedControllerHeight;
        private float storedClimbRateTime;
        private int storedJumpCount;
        private bool previouslyGrounded;
        private Vector2 movementInput;
        private Vector3 storedGroundHitNormal;
        private CoroutineObject checkLadderCoroutine;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            controllerRigidbody = GetComponent<Rigidbody>();
            capsuleCollider = GetComponent<CapsuleCollider>();
            audioSource = GetComponent<AudioSource>();
            cameraTransform = GetCameraControl().GetInstanceTransform();

            cameraControl.Initialize(this);

            CreatePhysicMaterial(ref frictionPhysics, ref maxFrictionPhysics, ref slippyPhysics);

            checkLadderCoroutine = new CoroutineObject(this);

            storedControllerHeight = capsuleCollider.height;

            OnGroundedCallback += () => checkLadderCoroutine.Start(CheckLadder);
            OnGroundedCallback += () => { if (isClimbing) StopClimb(); };

            OnGroundedCallback += () => storedJumpCount = 0;

            OnEnableCallback += () => onEnableEvent?.Invoke();
            OnDisableCallback += () => onDisableEvent?.Invoke();
            OnGroundedCallback += () => onGroundedEvent?.Invoke();
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
            ReadInput();
            ControlSpeed();
            CalculateControllerState();
            CameraProcessing();
        }

        /// <summary>
        /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void FixedUpdate()
        {
            ControlMovement();
            CrouchProcessing();
            GroundCheck();
            CheckLadder();
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        protected virtual void OnDisable()
        {
            OnDisableCallback?.Invoke();
        }

        /// <summary>
        /// Read input values and apply input vector.
        /// </summary>
        protected virtual void ReadInput()
        {
            movementInput.x = AInput.GetAxis(INC.CharHorizontal);
            movementInput.y = AInput.GetAxis(INC.CharVertical);

            if (!isJump && !isCrouching && AInput.GetButtonDown(INC.Jump))
            {
                isJump = true;
                OnJumpCallback?.Invoke();
            }

            if ((IsGrounded() && (IsCrouchHandleType(InputHandleType.Trigger) && AInput.GetButtonDown(INC.Crouch))) ||
                (IsCrouchHandleType(InputHandleType.Hold) && AInput.GetButtonDown(INC.Crouch)) ||
                (IsCrouchHandleType(InputHandleType.Hold) && AInput.GetButtonUp(INC.Crouch)) ||
                (isCrouching && IsCrouchHandleType(InputHandleType.Hold) && HasObstaclesOverhead() && !AInput.GetButton(INC.Crouch)))
            {
                SwitchCrouchState();
            }
        }

        /// <summary>
        /// Controlling different movement types.
        /// </summary>
        protected virtual void ControlMovement()
        {
            if (isClimbing)
            {
                ClimbMovementProcessing();
            }
            else
            {
                BaseMovementProcessing();
            }
        }

        protected virtual void ControlSpeed()
        {
            if (movementInput == Vector2.zero || !IsGrounded())
            {
                speed = 0;
            }
            else if (isCrouching)
            {
                speed = crouchWalkSpeed;
            }
            else if (AInput.GetButton(INC.LightWalk))
            {
                speed = walkSpeed;
            }
            else if (AInput.GetButton(INC.Sprint) && IsSprintGradualAcceleration() && IsSprintCondition(sprintDirection, movementInput.y, movementInput.x))
            {
                speed = sprintSpeed;
            }
            else
            {
                speed = runSpeed;
            }

            if (IsGrounded())
            {
                if (movementInput.y < 0)
                {
                    speed *= backwardSpeedPersent;
                }
                else if (movementInput.x != 0 && movementInput.y == 0)
                {
                    speed *= sideSpeedPersent;
                }

                if (cameraControl.IsZooming())
                {
                    speed *= zoomSpeedPersent;
                }
            }
            else if (AirControlContains(AirControl.Input))
            {
                speed = airControlSpeed;
            }
        }

        public virtual bool FindedStepOffset(out float stepHeight)
        {
            stepHeight = 0;

            Vector3 direction = Vector3.zero;
            if (movementInput.y > 0)
                direction += transform.forward;
            else if (movementInput.y < 0)
                direction += -transform.forward;
            if (movementInput.x > 0)
                direction += transform.right;
            else if (movementInput.x < 0)
                direction += -transform.right;

            direction *= capsuleCollider.radius + 0.1f;

            float controllerHeight = transform.position.y - (capsuleCollider.height / 2f);
            Vector3 origin = new Vector3(transform.position.x, controllerHeight, transform.position.z);
            origin += Vector3.up * maxStepOffsetHeight;
            origin += direction;
            if (Physics.Raycast(origin, -Vector3.up, out RaycastHit hitInfo, stepOffsetCheckRange, LNC.IgnorePlayer, QueryTriggerInteraction.Ignore))
            {
                float angle = Vector3.Angle(hitInfo.normal, Vector3.up);
                if (AMath.InRange(angle, 0, 15))
                {
                    stepHeight = hitInfo.point.y - controllerHeight;
                }
            }
            return stepHeight != 0;
        }

        protected virtual void MakeStepOffset(float stepHeight)
        {
            transform.position = Vector3.Lerp(transform.position, transform.position + (Vector3.up * stepHeight), stepOffsetSmooth * Time.deltaTime);
        }

        /// <summary>
        /// Processing camera rotation.
        /// </summary>
        protected virtual void CameraProcessing()
        {
            if (isClimbing && climbCameraLock)
            {
                // Quaternion lookAt = Quaternion.LookRotation(storedLadderTransform.forward) * Quaternion.Euler(0, -180, 0);
                // cameraTransform.localRotation = Quaternion.Slerp(cameraTransform.localRotation, lookAt, 10 * Time.deltaTime);
                // transform.rotation = Quaternion.Slerp(transform.rotation, lookAt, 10 * Time.deltaTime);
                return;
            }

            float oldYRotation = transform.eulerAngles.y;

            cameraControl.Update();

            if (isGrounded || (!isGrounded && AirControlContains(AirControl.Camera)))
            {
                Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
                controllerRigidbody.velocity = velRotation * controllerRigidbody.velocity;
            }
        }

        /// <summary>
        /// Calculate controller state.
        /// </summary>
        protected virtual void CalculateControllerState()
        {
            if (speed == 0)
            {
                controllerState = ControllerState.Idle;
                controllerState &= ~ControllerState.Walking;
                controllerState &= ~ControllerState.Running;
                controllerState &= ~ControllerState.Sprinting;
            }
            else
            {
                controllerState &= ~ControllerState.Idle;
                if (speed == walkSpeed || (cameraControl.IsZooming() && AMath.Approximately(walkSpeed / zoomSpeedPersent, speed)))
                    controllerState |= ControllerState.Walking;
                else
                    controllerState &= ~ControllerState.Walking;

                if (speed == runSpeed || (cameraControl.IsZooming() && AMath.Approximately(runSpeed / zoomSpeedPersent, speed)))
                    controllerState |= ControllerState.Running;
                else
                    controllerState &= ~ControllerState.Running;

                if (speed == sprintSpeed || (cameraControl.IsZooming() && AMath.Approximately(sprintSpeed / zoomSpeedPersent, speed)))
                    controllerState |= ControllerState.Sprinting;
                else
                    controllerState &= ~ControllerState.Sprinting;
            }

            if (!IsGrounded())
                controllerState |= ControllerState.InAir;
            else
                controllerState &= ~ControllerState.InAir;

            if (cameraControl.IsZooming())
                controllerState |= ControllerState.Zooming;
            else
                controllerState &= ~ControllerState.Zooming;

            if (isJump)
                controllerState |= ControllerState.Jumped;
            else
                controllerState &= ~ControllerState.Jumped;

            if (isCrouching)
                controllerState |= ControllerState.Crouched;
            else
                controllerState &= ~ControllerState.Crouched;
        }

        /// <summary>
        /// Processing controller surface movement.
        /// </summary>
        protected virtual void BaseMovementProcessing()
        {
            if ((Mathf.Abs(GetHorizontalInput()) > float.Epsilon || Mathf.Abs(GetVerticalInput()) > float.Epsilon) && (isGrounded || (!isGrounded && AirControlContains(AirControl.Input))))
            {
                Vector3 desiredMove = transform.forward * GetVerticalInput() + cameraTransform.right * GetHorizontalInput();
                desiredMove = Vector3.ProjectOnPlane(desiredMove, storedGroundHitNormal).normalized;
                desiredMove *= GetSpeed();

                if (GetVelocity().sqrMagnitude < (GetSpeed() * GetSpeed()))
                {
                    desiredMove *= SlopeMultiplier();

                    if (isGrounded && !isJumping && FindedStepOffset(out float stepHeight))
                    {
                        transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up * stepHeight, stepOffsetSmooth * Time.deltaTime);
                    }

                    switch (movementMode)
                    {
                        case MovementMode.AddForce:
                            controllerRigidbody.AddForce(desiredMove, ForceMode.Impulse);

                            break;
                        case MovementMode.Velocity:
                            controllerRigidbody.velocity = desiredMove;
                            break;
                    }
                    OnMoveCallback?.Invoke(controllerRigidbody.velocity);
                }
            }

            if (isGrounded)
            {
                controllerRigidbody.drag = 5f;

                if (IsJumped())
                {
                    MakeJump(jumpForce);
                }

                if (!isJumping && Mathf.Abs(GetHorizontalInput()) < float.Epsilon && Mathf.Abs(GetVerticalInput()) < float.Epsilon && controllerRigidbody.velocity.magnitude < 1f)
                {
                    controllerRigidbody.Sleep();
                }
            }
            else
            {
                if (IsJumped() && storedJumpCount < maxJumpCount)
                {
                    MakeJump(multipleJumpForce);
                }

                controllerRigidbody.drag = 0f;
                if (previouslyGrounded && !isJumping)
                {
                    StickToGroundHelper();
                }
            }
            IsJump(false);
        }

        /// <summary>
        /// Make controller jump.
        /// </summary>
        protected virtual void MakeJump(float force)
        {
            controllerRigidbody.drag = 0f;
            // controllerRigidbody.velocity = new Vector3(controllerRigidbody.velocity.x, 0f, controllerRigidbody.velocity.z);
            controllerRigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);
            storedJumpCount++;
            isJumping = true;
        }

        /// <summary>
        /// Processing controller crouch.
        /// </summary>
        protected virtual void CrouchProcessing()
        {
            Vector3 heightPosition = transform.position;
            float storedHeight = capsuleCollider.height;
            capsuleCollider.height = Mathf.Lerp(capsuleCollider.height, isCrouching ? crouchHeight : storedControllerHeight, crouchSpeed * Time.deltaTime);
            heightPosition.y += (capsuleCollider.height - storedHeight) / 2;
            transform.position = heightPosition;
        }

        /// <summary>
        /// Switch current controller crouch state.
        /// </summary>
        public virtual void SwitchCrouchState()
        {
            isCrouching = !(isCrouching && HasObstaclesOverhead());
        }

        /// <summary>
        /// Checks if there are any obstacles overhead controller. 
        /// </summary>
        public bool HasObstaclesOverhead()
        {
            float range = ((capsuleCollider.height / 2) + (storedControllerHeight - crouchHeight));
            return !Physics.Raycast(transform.position, Vector3.up, range, LNC.IgnorePlayer, QueryTriggerInteraction.Ignore);
        }

        /// <summary>
        /// Return permission for sprinting.
        /// </summary>
        public bool IsSprintCondition(SprintDirection sprintDirection, float vertical, float horizontal)
        {
            if (sprintDirection == SprintDirection.Free)
                return true;
            else if (sprintDirection == SprintDirection.Forward && (vertical > 0 && horizontal == 0))
                return true;
            else if (sprintDirection == SprintDirection.Backword && (vertical < 0 && horizontal == 0))
                return true;
            else if (sprintDirection == SprintDirection.Side && (horizontal != 0 && vertical == 0))
                return true;
            else if ((sprintDirection == (SprintDirection.Forward | SprintDirection.Backword)) && (vertical != 0 && horizontal == 0))
                return true;
            else if ((sprintDirection == (SprintDirection.Forward | SprintDirection.Side)) && vertical > 0)
                return true;
            else if ((sprintDirection == (SprintDirection.Backword | SprintDirection.Side)) && vertical < 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Gradual speed acceleration condition.
        /// </summary>
        public bool IsSprintGradualAcceleration()
        {
            float threshold = (runSpeed * runSpeed) * 0.75f;
            return gradualAcceleration ? (movementInput.x * movementInput.x) >= threshold : true;
        }

        /// <summary>
        /// Chech that controller is grounded.
        /// </summary>
        private void GroundCheck()
        {
            OnGroundedCallbackHandler();

            if (isGrounded && speed == 0)
				capsuleCollider.material = maxFrictionPhysics;
			else if (isGrounded && speed != 0)
				capsuleCollider.material = frictionPhysics;
			else
				capsuleCollider.material = slippyPhysics;


            previouslyGrounded = isGrounded;
            RaycastHit hitInfo;
            if (Physics.SphereCast(transform.position, capsuleCollider.radius * (1.0f - shellOffset), Vector3.down, out hitInfo,
                    ((capsuleCollider.height / 2f) - capsuleCollider.radius) + groundCheckDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                isGrounded = true;
                storedGroundHitNormal = hitInfo.normal;
            }
            else
            {
                isGrounded = false;
                storedGroundHitNormal = Vector3.up;
            }
            if (!previouslyGrounded && isGrounded && isJumping)
            {
                isJumping = false;
            }
        }

        /// <summary>
        /// OnGroundedCallback event function handler.
        /// </summary>
        private void OnGroundedCallbackHandler()
        {
            if (!previouslyGrounded && isGrounded)
            {
                OnGroundedCallback?.Invoke();
            }
        }

        /// <summary>
        ///  Evaluate surface angle.
        /// </summary>
        private float SlopeMultiplier()
        {
            float angle = Vector3.Angle(storedGroundHitNormal, Vector3.up);
            return slopeCurveModifier.Evaluate(angle);
        }

        /// <summary>
        /// Stops the controller.
        /// </summary>
        private void StickToGroundHelper()
        {
            RaycastHit hitInfo;
            if (Physics.SphereCast(transform.position, capsuleCollider.radius * (1.0f - shellOffset), Vector3.down, out hitInfo,
                    ((capsuleCollider.height / 2f) - capsuleCollider.radius) +
                    stickToGroundHelperDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                if (Mathf.Abs(Vector3.Angle(hitInfo.normal, Vector3.up)) < 85f)
                {
                    controllerRigidbody.velocity = Vector3.ProjectOnPlane(controllerRigidbody.velocity, hitInfo.normal);
                }
            }
        }

        /// <summary>
        /// Check ladder before controller using raycast with specific delay.
        /// </summary>
        protected virtual IEnumerator CheckLadder()
        {
            float range = capsuleCollider.radius + 0.1f;
            WaitForSeconds delay = new WaitForSeconds(climbCheckRate);
            while (true)
            {
                yield return delay;
                if (storedLadderTransform != null)
                {
                    if (Vector3.Distance(transform.position, storedLadderTransform.position) <= range)
                    {
                        continue;
                    }
                    storedLadderTransform = null;
                }

                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, range, climbCullingLayer, QueryTriggerInteraction.Ignore))
                {
                    StartClimb(hitInfo.transform);
                    yield break;
                }
            }
        }

        /// <summary>
        /// Start controller climbing on ladder.
        /// </summary>
        /// <param name="ladder"></param>
        protected virtual void StartClimb(Transform ladder)
        {
            if (ladder != null)
            {
                isClimbing = true;
                controllerRigidbody.useGravity = false;
                storedLadderTransform = ladder;
                checkLadderCoroutine.Stop();
            }
        }

        /// <summary>
        /// Stop controller climbing
        /// </summary>
        protected virtual void StopClimb()
        {
            isClimbing = false;
            controllerRigidbody.useGravity = true;
        }

        /// <summary>
        /// Processing controller climb movement.
        /// </summary>
        protected virtual void ClimbMovementProcessing()
        {
            Vector3 desiredMove = Vector3.zero;
            switch (climbMovement)
            {
                case ClimbMovement.Free:
                    {
                        if (climbCameraLock)
                        {
                            desiredMove = transform.up * GetVerticalInput() + transform.right * GetHorizontalInput();
                        }
                        else
                        {
                            desiredMove = cameraTransform.forward * Mathf.Clamp01(GetVerticalInput()) + transform.right * GetHorizontalInput();
                        }
                        break;
                    }
                case ClimbMovement.Vertical:
                    {
                        Vector3 ladderCenter = storedLadderTransform.position + (Vector3.forward * 0.5f);
                        ladderCenter.y = transform.position.y;
                        transform.position = Vector3.Lerp(transform.position, ladderCenter, 10 * Time.deltaTime);
                        if (climbCameraLock)
                        {
                            desiredMove.y = GetVerticalInput();
                        }
                        else
                        {
                            float lookDirection = (cameraTransform.forward.y > downThreshold && GetVerticalInput() > 0) ? 1 : -1;
                            desiredMove.y = Mathf.Abs(GetVerticalInput()) * lookDirection;
                        }
                    }
                    break;
            }

            controllerRigidbody.velocity = desiredMove * GetSpeed();

            float halfControllerHeight = capsuleCollider.height / 2f;
            float controllerHeight = (transform.position.y - halfControllerHeight);
            float radius = capsuleCollider.radius + 0.25f;
            Vector3 origin = new Vector3(transform.position.x, controllerHeight + capsuleCollider.radius, transform.position.z);
            if (isJump || !Physics.CheckSphere(origin, radius, climbCullingLayer, QueryTriggerInteraction.Ignore))
            {
                StopClimb();
                controllerRigidbody.velocity = Vector3.zero;
            }

            PlayClimbSound();
        }

        /// <summary>
        /// Play sound while controller climbing.
        /// </summary>
        protected virtual void PlayClimbSound()
        {
            // Player climb sound only if controller is climbing.
            if ((GetVerticalInput() != 0 || (GetHorizontalInput() != 0 && IsClimbType(ClimbMovement.Free))) &&
                Time.time - storedClimbRateTime >= climbSoundRate)
            {
                if (climbSound != null)
                {
                    audioSource.PlayOneShot(climbSound);
                }
                storedClimbRateTime = Time.time;
            }
        }

        public virtual void CreatePhysicMaterial(ref PhysicMaterial movementPhysics, ref PhysicMaterial stayPhysics, ref PhysicMaterial airPhysics)
        {
            // Slides the character through walls and edges
            movementPhysics = new PhysicMaterial();
            movementPhysics.name = "FrictionPhysics";
            movementPhysics.staticFriction = 0.25f;
            movementPhysics.dynamicFriction = 0.25f;
            movementPhysics.frictionCombine = PhysicMaterialCombine.Multiply;

            // Prevents the collider from slipping on ramps
            stayPhysics = new PhysicMaterial();
            stayPhysics.name = "MaxFrictionPhysics";
            stayPhysics.staticFriction = 1.0f;
            stayPhysics.dynamicFriction = 1.0f;
            stayPhysics.frictionCombine = PhysicMaterialCombine.Maximum;

            // Air physics 
            airPhysics = new PhysicMaterial();
            airPhysics.name = "ZeroFrictionPhysics";
            airPhysics.staticFriction = 0.0f;
            airPhysics.dynamicFriction = 0.0f;
            airPhysics.frictionCombine = PhysicMaterialCombine.Multiply;
            airPhysics.bounceCombine = PhysicMaterialCombine.Average;
        }

        #region [IControllerBase Implementation]
        public override Vector3 GetVelocity()
        {
            return controllerRigidbody.velocity;
        }
        #endregion

        #region [IControllerSpeed Implementation]
        public override float GetSpeed()
        {
            return speed;
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

        public override float GetSideSpeedPersent()
        {
            return sideSpeedPersent;
        }

        public override float GetBackwardSpeedPersent()
        {
            return backwardSpeedPersent;
        }
        #endregion

        #region [IControllerJump Implementation]
        public override bool IsJumped()
        {
            return isJump;
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
            return movementInput;
        }

        public override float GetHorizontalInput()
        {
            return movementInput.x;
        }

        public override float GetVerticalInput()
        {
            return movementInput.y;
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
            capsuleCollider.enabled = enabled;
            controllerRigidbody.detectCollisions = enabled;
            controllerRigidbody.isKinematic = !enabled;
        }
        #endregion

        #region [IControllerCrouched Implementation]
        public override bool IsCrouched()
        {
            return isCrouching;
        }
        #endregion

        #region [IControllerGrounded Implementation]
        public override bool IsGrounded()
        {
            return isGrounded;
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
        protected void IsJump(bool value)
        {
            isJump = value;
        }

        protected void IsCrouching(bool value)
        {
            isCrouching = value;
        }

        protected void SetMovementInput(Vector2 value)
        {
            movementInput = value;
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

        public SprintDirection GetSprintDirection()
        {
            return sprintDirection;
        }

        public void SetSprintDirection(SprintDirection value)
        {
            sprintDirection = value;
        }

        public void SetBackwardSpeedPersent(float value)
        {
            backwardSpeedPersent = value;
        }
        public void SetSideSpeedPersent(float value)
        {
            sideSpeedPersent = value;
        }

        public float GetZoomSpeedPersent()
        {
            return zoomSpeedPersent;
        }

        public void SetZoomSpeedPersent(float value)
        {
            zoomSpeedPersent = value;
        }

        public bool GetGradualAcceleration()
        {
            return gradualAcceleration;
        }

        public void SetGradualAcceleration(bool value)
        {
            gradualAcceleration = value;
        }

        public float GetMaxStepOffsetHeight()
        {
            return maxStepOffsetHeight;
        }

        public void SetMaxStepOffsetHeight(float value)
        {
            maxStepOffsetHeight = value;
        }

        public float GetStepOffsetCheckRange()
        {
            return stepOffsetCheckRange;
        }

        public void SetStepOffsetCheckRange(float value)
        {
            stepOffsetCheckRange = value;
        }

        public float GetStepOffsetSmooth()
        {
            return stepOffsetSmooth;
        }

        public void SetStepOffsetSmooth(float value)
        {
            stepOffsetSmooth = value;
        }

        public InputHandleType GetCrouchHandleType()
        {
            return crouchHandleType;
        }

        public void SetCrouchHandleType(InputHandleType value)
        {
            crouchHandleType = value;
        }

        public bool IsCrouchHandleType(InputHandleType value)
        {
            return crouchHandleType == value;
        }

        public float GetCrouchWalkSpeed()
        {
            return crouchWalkSpeed;
        }

        public void SetCrouchWalkSpeed(float value)
        {
            crouchWalkSpeed = value;
        }

        public float GetCrouchSpeed()
        {
            return crouchSpeed;
        }

        public void SetCrouchSpeed(float value)
        {
            crouchSpeed = value;
        }

        public float GetCrouchHeight()
        {
            return crouchHeight;
        }

        public void SetCrouchHeight(float value)
        {
            crouchHeight = value;
        }

        protected void SetControllerState(ControllerState value)
        {
            controllerState = value;
        }

        public AnimationCurve GetSlopeCurveModifier()
        {
            return slopeCurveModifier;
        }

        public void SetSlopeCurveModifier(AnimationCurve value)
        {
            slopeCurveModifier = value;
        }

        public static AnimationCurve DefaultSlopeCurveModifier()
        {
            return new AnimationCurve(new Keyframe(-90.0f, 1.0f), new Keyframe(0.0f, 1.0f), new Keyframe(90.0f, 0.0f));
        }

        public float GetGroundCheckDistance()
        {
            return groundCheckDistance;
        }

        public void SetGroundCheckDistance(float value)
        {
            groundCheckDistance = value;
        }

        public float GetStickToGroundHelperDistance()
        {
            return stickToGroundHelperDistance;
        }

        public void SetStickToGroundHelperDistance(float value)
        {
            stickToGroundHelperDistance = value;
        }

        public float GetShellOffset()
        {
            return shellOffset;
        }

        public void SetShellOffset(float value)
        {
            shellOffset = value;
        }

        public MovementMode GetMovementMode()
        {
            return movementMode;
        }

        public void SetMovementMode(MovementMode value)
        {
            movementMode = value;
        }

        public float GetJumpForce()
        {
            return jumpForce;
        }

        public void SetJumpForce(float value)
        {
            jumpForce = value;
        }

        public AirControl GetAirControl()
        {
            return airControl;
        }

        public void SetAirControl(AirControl value)
        {
            airControl = value;
        }

        public bool AirControlContains(AirControl value)
        {
            return (airControl & value) == value;
        }

        public bool AirControlIs(AirControl value)
        {
            return airControl == value;
        }

        public float GetAirControlSpeed()
        {
            return airControlSpeed;
        }

        public void SetAirControlSpeed(float value)
        {
            airControlSpeed = value;
        }

        public int GetMaxJumpCount()
        {
            return maxJumpCount;
        }

        public void SetMaxJumpCount(int value)
        {
            maxJumpCount = value;
        }

        public float GetMultipleJumpForce()
        {
            return multipleJumpForce;
        }

        public void SetMultipleJumpForce(float value)
        {
            multipleJumpForce = value;
        }

        public bool IsJumping()
        {
            return isJumping;
        }

        public ClimbMovement GetClimbMovement()
        {
            return climbMovement;
        }

        public void SetClimbMovement(ClimbMovement value)
        {
            climbMovement = value;
        }

        public bool IsClimbType(ClimbMovement value)
        {
            return climbMovement == value;
        }

        public bool ClimbCameraLock()
        {
            return climbCameraLock;
        }

        public void ClimbCameraLock(bool value)
        {
            climbCameraLock = value;
        }

        public float GetClimbSpeed()
        {
            return climbSpeed;
        }

        public void SetClimbSpeed(float value)
        {
            climbSpeed = value;
        }

        public float GetClimbDownThreshold()
        {
            return downThreshold;
        }

        public void SetClimbDownThreshold(float value)
        {
            downThreshold = value;
        }

        public float GetClimbCheckRate()
        {
            return climbCheckRate;
        }

        public void SetClimbCheckRate(float value)
        {
            climbCheckRate = value;
        }

        public float GetClimbSoundRate()
        {
            return climbSoundRate;
        }

        public void SetClimbSoundRate(float value)
        {
            climbSoundRate = value;
        }

        public AudioClip GetClimbSound()
        {
            return climbSound;
        }

        public void SetClimbSound(AudioClip value)
        {
            climbSound = value;
        }

        public LayerMask GetClimbCullingLayer()
        {
            return climbCullingLayer;
        }

        public void SetClimbCullingLayer(LayerMask value)
        {
            climbCullingLayer = value;
        }

        public bool IsClimbing()
        {
            return isClimbing;
        }

        public UnityEvent OnEnableEvent()
        {
            return onEnableEvent;
        }

        protected void OnEnableEvent(UnityEvent value)
        {
            onEnableEvent = value;
        }

        public UnityEvent OnDisableEvent()
        {
            return onDisableEvent;
        }

        protected void OnDisableEvent(UnityEvent value)
        {
            onDisableEvent = value;
        }

        public UnityEvent GetOnGroundedEvent()
        {
            return onGroundedEvent;
        }

        protected void SetOnGroundedEvent(UnityEvent value)
        {
            onGroundedEvent = value;
        }

        public Rigidbody GetControllerRigidbody()
        {
            return controllerRigidbody;
        }

        protected void SetControllerRigidbody(Rigidbody value)
        {
            controllerRigidbody = value;
        }

        public CapsuleCollider GetCapsuleCollider()
        {
            return capsuleCollider;
        }

        protected void SetCapsuleCollider(CapsuleCollider value)
        {
            capsuleCollider = value;
        }

        public AudioSource GetAudioSource()
        {
            return audioSource;
        }

        protected void SetAudioSource(AudioSource value)
        {
            audioSource = value;
        }
        #endregion
    }
}