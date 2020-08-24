﻿/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using AuroraFPSRuntime;
using UnityEditor;
using UnityEngine;

namespace AuroraFPSEditor
{
    //[CustomEditor(typeof(FPRController))]
    public class FPRControllerEditor : AuroraEditor<FPRController>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent CameraProperties = new GUIContent("Camera Properties");

            public readonly static GUIContent BaseProperties = new GUIContent("Base Properites");
            public readonly static GUIContent CameraInstance = new GUIContent("Camera Instance", "Camera instance transform.");
            public readonly static GUIContent FPCameraPivot = new GUIContent("FPCamera", "First person camera pivot transform.");
            public readonly static GUIContent XSensitivity = new GUIContent("Sensitivity X", "Look sensitivity by X(Horizontal) axis.");
            public readonly static GUIContent YSensitivity = new GUIContent("Sensitivity Y", "Look sensitivity by Y(Vertical) axis.");
            public readonly static GUIContent MinimumY = new GUIContent("Minimum Y", "Minimum Y(Vertical) look direction.");
            public readonly static GUIContent MaximumY = new GUIContent("Maximum Y", "Maximum Y(Vertical) look direction.");
            public readonly static GUIContent PositionSmooth = new GUIContent("Position Smooth", "Position smooth value");
            public readonly static GUIContent RotationSmooth = new GUIContent("Rotation Smooth", "Rotation smooth value.");
            public readonly static GUIContent ClampVerticalRotation = new GUIContent("Clamp Vertical Rotation");

            public readonly static GUIContent BobbingProperties = new GUIContent("Bobbing Properites");
            public readonly static GUIContent HeadBob = new GUIContent("Head Bob");
            public readonly static GUIContent Bob = new GUIContent("Settings", "Head bob settings asset file.");

            public readonly static GUIContent JumpCycleBob = new GUIContent("Jump Bob");
            public readonly static GUIContent JumpBobDuration = new GUIContent("Duration", "Jump bob duration.");
            public readonly static GUIContent JumpBobAmount = new GUIContent("Amount", "Jump bob amount.");
            public readonly static GUIContent UseHeadBob = new GUIContent("Use Head Bob");
            public readonly static GUIContent UseJumpBob = new GUIContent("Use Jump Bob");

            public readonly static GUIContent FieldOfViewProperties = new GUIContent("Field Of View Kick");
            public readonly static GUIContent IncreaseFOV = new GUIContent("FOV Value", "Increase field of view value.");
            public readonly static GUIContent IncreaseSpeed = new GUIContent("Increase Speed", "Increase speed to kick field of view value.");
            public readonly static GUIContent DecreaseSpeed = new GUIContent("Decrease Speed", "Decrease speed to default field of view value.");

            public readonly static GUIContent ZoomProperties = new GUIContent("Zoom Properites");
            public readonly static GUIContent ZoomHandleType = new GUIContent("Handle Type", "Zoom input handle type.");
            public readonly static GUIContent ZoomFOV = new GUIContent("FOV Value", "Zoom field of view value.");
            public readonly static GUIContent ZoomIncreaseSpeed = new GUIContent("Increase Speed", "Speed to zoom field of view value.");
            public readonly static GUIContent ZoomDecreaseSpeed = new GUIContent("Decrease Speed", "Speed to default field of view value.");

            public readonly static GUIContent SwayingProperties = new GUIContent("Swaying Properties");
            public readonly static GUIContent SwayAmount = new GUIContent("Amount", "Camera swaying amount (Max sway camera angle).");
            public readonly static GUIContent SwaySpeed = new GUIContent("Speed", "Swaying speed.");
            public readonly static GUIContent SwayReturnSpeed = new GUIContent("Return Speed", "Return speed on default angle.");
            public readonly static GUIContent UseCameraSway = new GUIContent("Use Camera Sway");

            public readonly static GUIContent TiltsProperties = new GUIContent("Tilts Properites");
            public readonly static GUIContent TiltOutput = new GUIContent("Output", "Tilt output value.");
            public readonly static GUIContent TiltAngle = new GUIContent("Angle", "Tilt angle value.");
            public readonly static GUIContent TiltAngleSpeed = new GUIContent("Angle Speed", "Speed tilt to target angle.");
            public readonly static GUIContent TiltOutputSpeed = new GUIContent("Output Speed", "Speed tilt to output value.");

            public readonly static GUIContent ControllerProperties = new GUIContent("Controller Properties");

            public readonly static GUIContent MovementProperties = new GUIContent("Movement Properties");

            public readonly static GUIContent MovementMode = new GUIContent("Mode");

            public readonly static GUIContent Speed = new GUIContent("Speed");
            public readonly static GUIContent RunSpeed = new GUIContent("Run Speed", "Controller run speed.");
            public readonly static GUIContent WalkSpeed = new GUIContent("Walk Speed", "Controller walk speed.");
            public readonly static GUIContent SprintSpeed = new GUIContent("Sprint Speed", "Controller sprint speed.");
            public readonly static GUIContent BackwardSpeedPersent = new GUIContent("Backward Speed", "Controller speed delta while zooming.");
            public readonly static GUIContent SideSpeedPersent = new GUIContent("Side Speed", "Controller speed delta while zooming.");
            public readonly static GUIContent ZoomSpeedPersent = new GUIContent("Zoom Speed", "Controller speed delta while zooming.");
            public readonly static GUIContent SprintDirection = new GUIContent("Sprint Direction", "Available sprint directions.");
            public readonly static GUIContent GradualAcceleration = new GUIContent("Gradual Acceleration", "Use gradual speed acceleration.");

            public readonly static GUIContent StepOffset = new GUIContent("Step Offset");
            public readonly static GUIContent MaxStepOffsetHeight = new GUIContent("Max Height");
            public readonly static GUIContent StepOffsetCheckRange = new GUIContent("Check Range");
            public readonly static GUIContent StepOffsetSmooth = new GUIContent("Smooth");

            public readonly static GUIContent Ground = new GUIContent("Ground");
            public readonly static GUIContent SlopeCurveModifier = new GUIContent("Slope Modifier", "Controller slope curve modifier.");
            public readonly static GUIContent GroundCheckDistance = new GUIContent("Ground Distance", "Controller ground check distance.");
            public readonly static GUIContent StickToGroundHelperDistance = new GUIContent("Stick Distance", "Stick to ground helper distance.");
            public readonly static GUIContent ShellOffset = new GUIContent("Shell Offset");

            public readonly static GUIContent JumpProperties = new GUIContent("Jump Properties");
            public readonly static GUIContent AirControl = new GUIContent("Air Control", "Type of control the player on the air.");
            public readonly static GUIContent JumpForce = new GUIContent("Force", "Controller jump force.");
            public readonly static GUIContent AirControlSpeed = new GUIContent("Speed", "Speed while player still in air (air control speed).");
            public readonly static GUIContent MaxJumpCount = new GUIContent("Max Jump Count");
            public readonly static GUIContent MultipleJumpForce = new GUIContent("Multiple Jump Force");

            public readonly static GUIContent CrouchProperties = new GUIContent("Crouch Properties");
            public readonly static GUIContent СrouchHandleType = new GUIContent("Handle Type", "Controller crouch input handle type.");
            public readonly static GUIContent СrouchWalkSpeed = new GUIContent("Speed", "Controller speed while crouching.");
            public readonly static GUIContent СrouchSpeed = new GUIContent("Crouch Speed", "Crouch speed, controller collider height processing speed.");
            public readonly static GUIContent СrouchHeight = new GUIContent("Height", "Controller height while crouching.");

            public readonly static GUIContent ClimbProperties = new GUIContent("Climb Properties");
            public readonly static GUIContent ClimbMovement = new GUIContent("Climb Movement");
            public readonly static GUIContent ClimbCamera = new GUIContent("Climb Camera");
            public readonly static GUIContent ClimbSpeed = new GUIContent("Speed", "Controller speed while climbing.");
            public readonly static GUIContent DownThreshold = new GUIContent("Threshold", "Down threshold.");
            public readonly static GUIContent ClimbCheckRange = new GUIContent("Check Range");
            public readonly static GUIContent ClimbSoundRate = new GUIContent("Rate", "Climb sound rate.");
            public readonly static GUIContent ClimbSound = new GUIContent("Sound", "Climb sound clip which will playing while climbing.");
            public readonly static GUIContent ClimbCheckRate = new GUIContent("Check Rate");
            public readonly static GUIContent ClimbCullingLayer = new GUIContent("Culling Layer");
            public readonly static GUIContent ClimbSoundProperties = new GUIContent("Sound Properties");
            public readonly static GUIContent ClimbCameraLock = new GUIContent("Camera Lock");

            public readonly static GUIContent DebugReadOnly = new GUIContent("Debug");
            public readonly static GUIContent AdvancedProperties = new GUIContent("Advanced Properties");
        }

        private bool cameraBasePropertiesFoldout;
        private bool cameraBobbingPropertiesFoldout;
        private bool cameraFieldOfViewPropertiesFoldout;
        private bool cameraZoomPropertiesFoldout;
        private bool cameraSwayingPropertiesFoldout;
        private bool cameraTiltsPropertiesFoldout;
        private bool controllerMovementPropertiesFoldout;
        private bool controllerJumpPropertiesFoldout;
        private bool controllerCrouchPropertiesFoldout;
        private bool controllerClimbPropertiesFoldout;
        private bool controllerClimbAdvancedPropertiesFoldout;
        private bool controllerClimbSoundPropertiesFoldout;
        private bool controllerSpeedFoldout;
        private bool controllerGroundFoldout;
        private bool controllerStepOffsetFoldout;
        private bool controllerDebugFoldout;

        public override void InitializeProperties()
        {
            SetHeaderName("First Person Rigidbody Controller");
        }

        /// <summary>
        /// Base serializedObject properties.
        /// 
        /// Implement this method to draw a custom properties.
        /// </summary>
        public override void OnBaseGUI()
        {
            //#region [Camera Properties]
            //BeginGroup(ContentProperties.CameraProperties);
            //IncreaseIndentLevel();
            //// Base properties.
            //BeginGroupLevel2(ref cameraBasePropertiesFoldout, ContentProperties.BaseProperties);
            //if (cameraBasePropertiesFoldout)
            //{
            //    FPCameraLook cameraLook = instance.GetCameraLook();
            //    cameraLook.SetCameraInstanceTransform(AEditorGUILayout.RequiredObjectField(ContentProperties.CameraInstance, cameraLook.GetCameraInstanceTransform(), true));
            //    cameraLook.SetCameraPivotTransform(AEditorGUILayout.RequiredObjectField(ContentProperties.FPCameraPivot, cameraLook.GetCameraPivotTransform(), true));
            //    //cameraLook.SetSensitivityByX(EditorGUILayout.FloatField(ContentProperties.XSensitivity, cameraLook.GetSensitivityByX()));
            //    //cameraLook.SetSensitivityByY(EditorGUILayout.FloatField(ContentProperties.YSensitivity, cameraLook.GetSensitivityByY()));

            //    //float min = cameraLook.GetMinimumAngleByY();
            //    //float max = cameraLook.GetMaximumAngleByY();
            //    //AEditorGUILayout.MinMaxSlider("Limits Y", ref min, ref max, -180.0f, 180.0f);
            //    //cameraLook.SetMinimumAngleByY(min);
            //    //cameraLook.SetMaximumAngleByY(max);

            //    //cameraLook.SetPositionSmooth(AEditorGUILayout.VisualClamp(ContentProperties.PositionSmooth, cameraLook.GetPositionSmooth(), 20, 0.1f));
            //    //cameraLook.SetRotationSmooth(AEditorGUILayout.VisualClamp(ContentProperties.RotationSmooth, cameraLook.GetRotationSmooth(), 20, 0.1f));

            //    cameraLook.ClampVerticalRotation(EditorGUILayout.Toggle(ContentProperties.ClampVerticalRotation, cameraLook.ClampVerticalRotation()));

            //    instance.SetCameraLook(cameraLook);
            //}
            //EndGroupLevel();

            //// Field of view properties.
            ////BeginGroupLevel2(ref cameraFieldOfViewPropertiesFoldout, ContentProperties.FieldOfViewProperties);
            ////if (cameraFieldOfViewPropertiesFoldout)
            ////{
            ////    FOVSettings fovKickSettings = instance.GetCameraLook().GetFOVKickSettings();
            ////    fovKickSettings.SetValue(EditorGUILayout.FloatField(ContentProperties.IncreaseFOV, fovKickSettings.GetValue()));
            ////    fovKickSettings.SetIncreaseSpeed(EditorGUILayout.FloatField(ContentProperties.IncreaseSpeed, fovKickSettings.GetIncreaseSpeed()));
            ////    fovKickSettings.SetDecreaseSpeed(EditorGUILayout.FloatField(ContentProperties.DecreaseSpeed, fovKickSettings.GetDecreaseSpeed()));
            ////    instance.GetCameraLook().SetFOVKickSettings(fovKickSettings);
            ////}
            ////EndGroupLevel();

            //// Zoom properties.
            //BeginGroupLevel2(ref cameraZoomPropertiesFoldout, ContentProperties.ZoomProperties);
            //if (cameraZoomPropertiesFoldout)
            //{
            //    FOVSettings zoomFOVKickSettings = instance.GetCameraLook().GetZoomFOVKickSettings();
            //    instance.GetCameraLook().SetZoomHandleType((InputHandleType)EditorGUILayout.EnumPopup(ContentProperties.ZoomHandleType, instance.GetCameraLook().GetZoomHandleType()));
            //    zoomFOVKickSettings.SetFieldOfView(EditorGUILayout.FloatField(ContentProperties.ZoomFOV, zoomFOVKickSettings.GetFieldOfView()));
            //    zoomFOVKickSettings.SetIncreaseSpeed(EditorGUILayout.FloatField(ContentProperties.ZoomIncreaseSpeed, zoomFOVKickSettings.GetIncreaseSpeed()));
            //    zoomFOVKickSettings.SetDecreaseSpeed(EditorGUILayout.FloatField(ContentProperties.ZoomDecreaseSpeed, zoomFOVKickSettings.GetDecreaseSpeed()));
            //    instance.GetCameraLook().SetZoomFOVKickSettings(zoomFOVKickSettings);
            //}
            //EndGroupLevel();

            //BeginGroupLevel2(ref cameraSwayingPropertiesFoldout, ContentProperties.SwayingProperties);
            //if (cameraSwayingPropertiesFoldout)
            //{
            //    FPCameraLook cameraLook = instance.GetCameraLook();
            //    cameraLook.SetSwayAmount(AEditorGUILayout.FixedFloatField(ContentProperties.SwayAmount, cameraLook.GetSwayAmount(), 0));
            //    cameraLook.SetSwaySpeed(AEditorGUILayout.FixedFloatField(ContentProperties.SwaySpeed, cameraLook.GetSwaySpeed(), 0));
            //    cameraLook.SetSwayReturnSpeed(AEditorGUILayout.FixedFloatField(ContentProperties.SwayReturnSpeed, cameraLook.GetSwayReturnSpeed(), 0));
            //    cameraLook.UseCameraSway(EditorGUILayout.Toggle(ContentProperties.UseCameraSway, cameraLook.UseCameraSway()));
            //    instance.SetCameraLook(cameraLook);
            //}
            //EndGroupLevel();

            //// Tilts properties.
            //BeginGroupLevel2(ref cameraTiltsPropertiesFoldout, ContentProperties.TiltsProperties);
            //if (cameraTiltsPropertiesFoldout)
            //{
            //    FPCameraLook cameraLook = instance.GetCameraLook();
            //    cameraLook.SetTiltOutput(EditorGUILayout.FloatField(ContentProperties.TiltOutput, cameraLook.GetTiltOutput()));
            //    cameraLook.SetTiltOutputSpeed(EditorGUILayout.FloatField(ContentProperties.TiltOutputSpeed, cameraLook.GetTiltOutputSpeed()));
            //    cameraLook.SetTiltAngle(EditorGUILayout.Slider(ContentProperties.TiltAngle, cameraLook.GetTiltAngle(), 0.0f, 90.0f));
            //    cameraLook.SetTiltAngleSpeed(EditorGUILayout.FloatField(ContentProperties.TiltAngleSpeed, cameraLook.GetTiltAngleSpeed()));
            //    instance.SetCameraLook(cameraLook);
            //}
            //EndGroupLevel();
            //DecreaseIndentLevel();
            //EndGroup();
            //#endregion

            #region [Controller Properties]
            BeginGroup(ContentProperties.ControllerProperties);
            IncreaseIndentLevel();

            // Movement properties.
            BeginGroupLevel2(ref controllerMovementPropertiesFoldout, ContentProperties.MovementProperties);
            if (controllerMovementPropertiesFoldout)
            {
                instance.SetMovementMode(AEditorGUILayout.EnumPopup(ContentProperties.MovementMode, instance.GetMovementMode()));
                GUILayout.Space(3);
                BeginGroupLevel3(ref controllerSpeedFoldout, ContentProperties.Speed);
                if (controllerSpeedFoldout)
                {
                    instance.SetWalkSpeed(EditorGUILayout.FloatField(ContentProperties.WalkSpeed, instance.GetWalkSpeed()));
                    instance.SetRunSpeed(EditorGUILayout.FloatField(ContentProperties.RunSpeed, instance.GetRunSpeed()));
                    instance.SetSprintSpeed(EditorGUILayout.FloatField(ContentProperties.SprintSpeed, instance.GetSprintSpeed()));
                    instance.SetBackwardSpeedPersent(EditorGUILayout.Slider(ContentProperties.BackwardSpeedPersent, instance.GetBackwardSpeedPersent(), 0.0f, 1.0f));
                    instance.SetSideSpeedPersent(EditorGUILayout.Slider(ContentProperties.SideSpeedPersent, instance.GetSideSpeedPersent(), 0.0f, 1.0f));
                    instance.SetZoomSpeedPersent(EditorGUILayout.Slider(ContentProperties.ZoomSpeedPersent, instance.GetZoomSpeedPersent(), 0.0f, 1.0f));
                    instance.SetSprintDirection((SprintDirection)EditorGUILayout.EnumFlagsField(ContentProperties.SprintDirection, instance.GetSprintDirection()));
                    instance.SetGradualAcceleration(EditorGUILayout.Toggle(ContentProperties.GradualAcceleration, instance.GetGradualAcceleration()));
                }
                EndGroupLevel();

                BeginGroupLevel3(ref controllerStepOffsetFoldout, ContentProperties.StepOffset);
                if (controllerStepOffsetFoldout)
                {
                    instance.SetMaxStepOffsetHeight(AEditorGUILayout.FixedFloatField(ContentProperties.MaxStepOffsetHeight, instance.GetMaxStepOffsetHeight(), 0));
                    instance.SetStepOffsetCheckRange(AEditorGUILayout.FixedFloatField(ContentProperties.StepOffsetCheckRange, instance.GetStepOffsetCheckRange(), 0));
                    instance.SetStepOffsetSmooth(AEditorGUILayout.VisualClamp(ContentProperties.StepOffsetSmooth, instance.GetStepOffsetSmooth(), 20, 0.1f));
                }
                EndGroupLevel();

                BeginGroupLevel3(ref controllerGroundFoldout, ContentProperties.Ground);
                if (controllerGroundFoldout)
                {
                    instance.SetSlopeCurveModifier(EditorGUILayout.CurveField(ContentProperties.SlopeCurveModifier, instance.GetSlopeCurveModifier()));
                    instance.SetGroundCheckDistance(EditorGUILayout.FloatField(ContentProperties.GroundCheckDistance, instance.GetGroundCheckDistance()));
                    instance.SetStickToGroundHelperDistance(EditorGUILayout.FloatField(ContentProperties.StickToGroundHelperDistance, instance.GetStickToGroundHelperDistance()));
                    instance.SetShellOffset(EditorGUILayout.FloatField(ContentProperties.ShellOffset, instance.GetShellOffset()));
                }
                EndGroupLevel();

                BeginGroupLevel3(ref controllerDebugFoldout, ContentProperties.DebugReadOnly);
                if (controllerDebugFoldout)
                {
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.Vector2Field("Input", instance.GetMovementInput());
                    EditorGUILayout.FloatField("Speed", instance.GetSpeed());
                    EditorGUILayout.TextField("Controller State", instance.GetState().ToString());
                    EditorGUI.EndDisabledGroup();
                }
                EndGroupLevel();
                GUILayout.Space(5);
            }
            EndGroupLevel();

            // Jump properties.
            BeginGroupLevel2(ref controllerJumpPropertiesFoldout, ContentProperties.JumpProperties);
            if (controllerJumpPropertiesFoldout)
            {
                instance.SetAirControl((AirControl)EditorGUILayout.EnumFlagsField(ContentProperties.AirControl, instance.GetAirControl()));
                instance.SetJumpForce(EditorGUILayout.FloatField(ContentProperties.JumpForce, instance.GetJumpForce()));
                instance.SetAirControlSpeed(EditorGUILayout.FloatField(ContentProperties.AirControlSpeed, instance.GetAirControlSpeed()));
                instance.SetMaxJumpCount(AEditorGUILayout.FixedIntField(ContentProperties.MaxJumpCount, instance.GetMaxJumpCount(), 0));
                instance.SetMultipleJumpForce(AEditorGUILayout.FixedFloatField(ContentProperties.MultipleJumpForce, instance.GetMultipleJumpForce(), 0));
            }
            EndGroupLevel();

            // Crouch properties.
            BeginGroupLevel2(ref controllerCrouchPropertiesFoldout, ContentProperties.CrouchProperties);
            if (controllerCrouchPropertiesFoldout)
            {
                instance.SetCrouchHandleType((InputHandleType)EditorGUILayout.EnumPopup(ContentProperties.СrouchHandleType, instance.GetCrouchHandleType()));
                instance.SetCrouchWalkSpeed(EditorGUILayout.FloatField(ContentProperties.СrouchWalkSpeed, instance.GetCrouchWalkSpeed()));
                instance.SetCrouchSpeed(EditorGUILayout.FloatField(ContentProperties.СrouchSpeed, instance.GetCrouchSpeed()));
                instance.SetCrouchHeight(EditorGUILayout.FloatField(ContentProperties.СrouchHeight, instance.GetCrouchHeight()));
            }
            EndGroupLevel();

            // Climb properties.
            BeginGroupLevel2(ref controllerClimbPropertiesFoldout, ContentProperties.ClimbProperties);
            if (controllerClimbPropertiesFoldout)
            {
                instance.SetClimbMovement(AEditorGUILayout.EnumPopup(ContentProperties.ClimbMovement, instance.GetClimbMovement()));
                instance.SetClimbSpeed(EditorGUILayout.FloatField(ContentProperties.ClimbSpeed, instance.GetClimbSpeed()));
                instance.ClimbCameraLock(EditorGUILayout.Toggle(ContentProperties.ClimbCameraLock, instance.ClimbCameraLock()));
                if (!instance.ClimbCameraLock())
                {
                    instance.SetClimbDownThreshold(EditorGUILayout.Slider(ContentProperties.DownThreshold, instance.GetClimbDownThreshold(), -1.0f, 1.0f));
                }

                BeginGroupLevel3(ref controllerClimbSoundPropertiesFoldout, ContentProperties.ClimbSoundProperties);
                if (controllerClimbSoundPropertiesFoldout)
                {
                    instance.SetClimbSoundRate(EditorGUILayout.FloatField(ContentProperties.ClimbSoundRate, instance.GetClimbSoundRate()));
                    instance.SetClimbSound(AEditorGUILayout.ObjectField(ContentProperties.ClimbSound, instance.GetClimbSound(), true));
                }
                EndGroupLevel();

                BeginGroupLevel3(ref controllerClimbAdvancedPropertiesFoldout, ContentProperties.AdvancedProperties);
                if (controllerClimbAdvancedPropertiesFoldout)
                {
                    instance.SetClimbCheckRate(EditorGUILayout.Slider(ContentProperties.ClimbCheckRate, instance.GetClimbCheckRate(), 0.01f, 1));
                    instance.SetClimbCullingLayer(AEditorGUILayout.LayerMaskField(ContentProperties.ClimbCullingLayer, instance.GetClimbCullingLayer()));
                }
                EndGroupLevel();
            }
            EndGroupLevel();
            DecreaseIndentLevel();

            OnEventsGUI();
            EndGroup();
            #endregion
        }
    }
}