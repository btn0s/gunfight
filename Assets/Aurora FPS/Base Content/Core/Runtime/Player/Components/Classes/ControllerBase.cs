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
using Mirror;

namespace AuroraFPSRuntime
{
    public abstract class ControllerBase : NetworkBehaviour, IController, IControllerSpeed, IControllerJump, IControllerState, IControllerInput, IControllerEnabled, IControllerGrounded, IControllerCrouched, IControllerCallbacks
    {
        #region [IControllerBase Implementation]
        public abstract Vector3 GetVelocity();
        #endregion

        #region [IcontrollerSpeed Implementation]
        public abstract float GetSpeed();
        public abstract float GetWalkSpeed();
        public abstract float GetRunSpeed();
        public abstract float GetSprintSpeed();
        public abstract float GetBackwardSpeedPersent();
        public abstract float GetSideSpeedPersent();
        #endregion

        #region [IControllerJump Implementation]
        public abstract bool IsJumped();
        #endregion

        #region [ControllerState Implementation]
        public abstract ControllerState GetState();
        public abstract bool CompareState(ControllerState value);
        public abstract bool HasState(ControllerState value);
        #endregion

        #region [IControllerInput Implementation]
        public abstract Vector2 GetMovementInput();
        public abstract float GetHorizontalInput();
        public abstract float GetVerticalInput();
        #endregion

        #region [IControllerEnabled Implementation]
        public abstract bool IsEnabled();
        public abstract void SetEnabled(bool enabled);
        #endregion

        #region [IControllerGrounded Implementation]
        public abstract bool IsGrounded();
        #endregion

        #region [IControllerCrouched Implementation]
        public abstract bool IsCrouched();
        #endregion

        #region [IControllerCallbacks Implementation]
        public abstract event Action<Vector3> OnMoveCallback;
        public abstract event Action OnGroundedCallback;
        public abstract event Action OnJumpCallback;
        public abstract event Action OnEnableCallback;
        public abstract event Action OnDisableCallback;
        #endregion
    }
}