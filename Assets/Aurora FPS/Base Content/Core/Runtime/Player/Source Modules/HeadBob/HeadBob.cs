/* ==================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using System;
using UnityEngine;

namespace AuroraFPSRuntime
{
    [Serializable]
    public class HeadBob
    {
        // Base HeadBob properties.
        [SerializeField] private HeadBobSettings settings;
        
        // Stored required properties.
        private float moveBackwardsMultiplier;
        private float moveSideMultiplier;
        private float xScroll;
        private float yScroll;
        private bool isResetted;
        private Vector3 finalOffset;
        private Vector3 defaultPosition;
        private float currentStateHeight;



        /// <summary>
        /// HeadBob constructor.
        /// </summary>
        public void Initialize(Vector3 position, float moveBackwardsMultiplier, float moveSideMultiplier)
        {
            this.defaultPosition = position;
            this.finalOffset = position;
            this.moveBackwardsMultiplier = moveBackwardsMultiplier;
            this.moveSideMultiplier = moveSideMultiplier;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ScrollHeadBob(Vector2 movementInput, ControllerState controllerState)
        {
            float amplitudeMultiplier = 1.0f;
            float frequencyMultiplier = 1.0f;
            float additionalMultiplier = 1.0f;

            if (controllerState == ControllerState.Walking)
            {
                amplitudeMultiplier = settings.GetWalkMultiplier().GetAmplitude();
                frequencyMultiplier = settings.GetWalkMultiplier().GetFrequency();
            }
            else if (controllerState == ControllerState.Running)
            {
                amplitudeMultiplier = settings.GetRunMultiplier().GetAmplitude();
                frequencyMultiplier = settings.GetRunMultiplier().GetFrequency();
            }
            else if (controllerState == ControllerState.Sprinting)
            {
                amplitudeMultiplier = settings.GetSprintMultiplier().GetAmplitude();
                frequencyMultiplier = settings.GetSprintMultiplier().GetFrequency();
            }
            else if ((controllerState & ControllerState.Crouched) == ControllerState.Crouched)
            {
                amplitudeMultiplier = settings.GetCrouchMultiplier().GetAmplitude();
                frequencyMultiplier = settings.GetCrouchMultiplier().GetFrequency();
            }
            else if ((controllerState & ControllerState.Zooming) == ControllerState.Zooming)
            {
                amplitudeMultiplier = settings.GetZoomMultiplier().GetAmplitude();
                frequencyMultiplier = settings.GetZoomMultiplier().GetFrequency();
            }

            additionalMultiplier = movementInput.y == -1 ? moveBackwardsMultiplier : 1f;
            additionalMultiplier = movementInput.x != 0 & movementInput.y == 0 ? moveSideMultiplier : additionalMultiplier;

            xScroll += Time.deltaTime * settings.GetFrequencyX() * frequencyMultiplier; // you can also multiply this by _additionalMultiplier but it looks unnatural a bit;
            yScroll += Time.deltaTime * settings.GetFrequencyY() * frequencyMultiplier;

            float _xValue;
            float _yValue;

            _xValue = settings.GetCurveX().Evaluate(xScroll);
            _yValue = settings.GetCurveY().Evaluate(yScroll);

            finalOffset.x = _xValue * settings.GetAmplitudeX() * amplitudeMultiplier * additionalMultiplier;
            finalOffset.y = _yValue * settings.GetAmplitudeY() * amplitudeMultiplier * additionalMultiplier;

            finalOffset += (Vector3.up * defaultPosition.y);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResetHeadBob()
        {
            xScroll = 0f;
            yScroll = 0f;

            finalOffset = defaultPosition;
        }

        #region [Getter / Setter]
        public HeadBobSettings GetSettings()
        {
            return settings;
        }

        public void SetSettings(HeadBobSettings value)
        {
            settings = value;
        }

        public Vector3 GetFinalOffset()
        {
            return finalOffset;
        }

        protected void SetFinalOffset(Vector3 value)
        {
            finalOffset = value;
        }

        public float GetMoveBackwardsMultiplier()
        {
            return moveBackwardsMultiplier;
        }

        public void SetMoveBackwardsMultiplier(float value)
        {
            moveBackwardsMultiplier = value;
        }

        public float GetMoveSideMultiplier()
        {
            return moveSideMultiplier;
        }

        public void SetMoveSideMultiplier(float value)
        {
            moveSideMultiplier = value;
        }
        #endregion
    }
}