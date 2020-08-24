/* ==================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================== */

using UnityEngine;
using UnityEditor;
using AuroraFPSRuntime;

namespace AuroraFPSEditor
{
    [CustomEditor(typeof(FPCController))]
    public class FPCControllerEditor : FPControllerEditor
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent LocomotionSettings = new GUIContent("Locomotion Settings");
            public readonly static GUIContent MovementType = new GUIContent("Movement Type");
            public readonly static GUIContent WalkSpeed = new GUIContent("Walk Speed");
            public readonly static GUIContent RunSpeed = new GUIContent("Run Speed");
            public readonly static GUIContent SprintSpeed = new GUIContent("Sprint Speed");
            public readonly static GUIContent CrouchSpeed = new GUIContent("Crouch Speed");
            public readonly static GUIContent ZoomSpeed = new GUIContent("Zoom Speed");
            public readonly static GUIContent CrouchZoomSpeed = new GUIContent("Crouch Zoom Speed");
            public readonly static GUIContent SideSpeedPercent = new GUIContent("Side Speed Percent");
            public readonly static GUIContent BackwardsSpeedPercent = new GUIContent("Backwards Speed Percent");
            public readonly static GUIContent SmoothInputSpeed = new GUIContent("Smooth Input");
            public readonly static GUIContent SmoothVelocitySpeed = new GUIContent("Smooth Velocity");
            public readonly static GUIContent SmoothFinalDirectionSpeed = new GUIContent("Smooth Move Vector");
            public readonly static GUIContent SmoothInputMagnitudeSpeed = new GUIContent("Smooth Input Magnitude");

            public readonly static GUIContent JumpSettings = new GUIContent("Jump Settings");
            public readonly static GUIContent JumpForce = new GUIContent("Force");
            public readonly static GUIContent JumpCount = new GUIContent("Count");
            public readonly static GUIContent JumpInAir = new GUIContent("Jump In Air");

            public readonly static GUIContent AirSettings = new GUIContent("Air Settings");
            public readonly static GUIContent InAirControl = new GUIContent("In Air Control");
            public readonly static GUIContent InAirControlPersent = new GUIContent("Control Persent");

            public readonly static GUIContent AccelerationSettings = new GUIContent("Acceleration Settings");
            public readonly static GUIContent AccelerationDirection = new GUIContent("Direction");
            public readonly static GUIContent RunVelocityThreshold = new GUIContent("Run Threshold");
            public readonly static GUIContent SprintVelocityThreshold = new GUIContent("Sprint Threshold");
            public readonly static GUIContent AccelerationCurve = new GUIContent("Curve");

            public readonly static GUIContent CrouchSettings = new GUIContent("Crouch Settings");
            public readonly static GUIContent CrouchActionType = new GUIContent("Action");
            public readonly static GUIContent CrouchHeightPercent = new GUIContent("Height Percent");
            public readonly static GUIContent CrouchDuration = new GUIContent("Duration");
            public readonly static GUIContent CrouchCurve = new GUIContent("Curve");

            public readonly static GUIContent SprintEffect = new GUIContent("Sprint Effect");
            public readonly static GUIContent UseSprintEffect = new GUIContent("Enabled");

            public readonly static GUIContent HeadBobEffect = new GUIContent("HeadBob Effect");
            public readonly static GUIContent HeadBobSettings = new GUIContent("HeadBob Settings");
            public readonly static GUIContent UseHeadBobEffect = new GUIContent("Enabled");

            public readonly static GUIContent LandingEffect = new GUIContent("Landing Effect");
            public readonly static GUIContent LandingAmountLimits = new GUIContent("Amount Limits");
            public readonly static GUIContent LandingTimer = new GUIContent("Amount Percent");
            public readonly static GUIContent LandingDuration = new GUIContent("Duration");
            public readonly static GUIContent LandingCurve = new GUIContent("Curve");
            public readonly static GUIContent UseLandingEffect = new GUIContent("Enabled");

            public readonly static GUIContent GravitySettings = new GUIContent("Gravity Settings");
            public readonly static GUIContent GravityMultiplier = new GUIContent("Gravity Multiplier");
            public readonly static GUIContent StickToGroundForce = new GUIContent("Stick Force");

            public readonly static GUIContent GroundedSettings = new GUIContent("Grounded Settings");
            public readonly static GUIContent GroundedCheckRange = new GUIContent("Check Range");
            public readonly static GUIContent GroundedCheckRadius = new GUIContent("Check Radius");
            public readonly static GUIContent GroundedCullingLayer = new GUIContent("Culling Layer");

            public readonly static GUIContent RoofSettings = new GUIContent("Roof Settings");
            public readonly static GUIContent RoofCheckRadius = new GUIContent("Check Radius");
            public readonly static GUIContent RoofCullingLayer = new GUIContent("Culling Layer");

            public readonly static GUIContent WallSettings = new GUIContent("Wall Settings");
            public readonly static GUIContent WallCheckRange = new GUIContent("Check Range");
            public readonly static GUIContent WallCheckRadius = new GUIContent("Check Radius");
            public readonly static GUIContent WallCullingLayer = new GUIContent("Culling Layer");

        }

        private FPCController fpcController;
        private bool[] controllerFoldouts;

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// 
        /// Implement this method to initialize properties.
        /// </summary>
        public override void InitializeProperties()
        {
            base.InitializeProperties();
            SetHeaderName("First Person Controller");
            fpcController = instance as FPCController;
            controllerFoldouts = new bool[12];
        }

        /// <summary>
        /// Properties of first person controller instance.
        /// </summary>
        public override void OnControllerGUI()
        {
            IncreaseIndentLevel();
            bool isExpanded = false;

            isExpanded = controllerFoldouts[0];
            BeginGroupLevel2(ref isExpanded, ContentProperties.LocomotionSettings);
            if (isExpanded)
            {
                fpcController.SetMovementType(AEditorGUILayout.EnumPopup(ContentProperties.MovementType, fpcController.GetMovementType()));
                fpcController.SetWalkSpeed(AEditorGUILayout.FixedFloatField(ContentProperties.WalkSpeed, fpcController.GetWalkSpeed(), 0));
                fpcController.SetRunSpeed(AEditorGUILayout.FixedFloatField(ContentProperties.RunSpeed, fpcController.GetRunSpeed(), 0));
                fpcController.SetSprintSpeed(AEditorGUILayout.FixedFloatField(ContentProperties.SprintSpeed, fpcController.GetSprintSpeed(), 0));
                fpcController.SetCrouchSpeed(AEditorGUILayout.FixedFloatField(ContentProperties.CrouchSpeed, fpcController.GetCrouchSpeed(), 0));
                fpcController.SetZoomSpeed(AEditorGUILayout.FixedFloatField(ContentProperties.ZoomSpeed, fpcController.GetZoomSpeed(), 0));
                fpcController.SetCrouchZoomSpeed(AEditorGUILayout.FixedFloatField(ContentProperties.CrouchZoomSpeed, fpcController.GetCrouchZoomSpeed(), 0));
                fpcController.SetSideSpeedPercent(AEditorGUILayout.FixedFloatField(ContentProperties.SideSpeedPercent, fpcController.GetSideSpeedPercent(), 0));
                fpcController.SetBackwardsSpeedPercent(AEditorGUILayout.FixedFloatField(ContentProperties.BackwardsSpeedPercent, fpcController.GetBackwardSpeedPersent(), 0));

                GUILayout.Space(15);

                fpcController.SetSmoothInputSpeed(AEditorGUILayout.VisualClamp(ContentProperties.SmoothInputSpeed, fpcController.GetSmoothInputSpeed(), 100, 0.01f));
                fpcController.SetSmoothVelocitySpeed(AEditorGUILayout.VisualClamp(ContentProperties.SmoothVelocitySpeed, fpcController.GetSmoothVelocitySpeed(), 100, 0.01f));
                fpcController.SetSmoothFinalDirectionSpeed(AEditorGUILayout.VisualClamp(ContentProperties.SmoothFinalDirectionSpeed, fpcController.GetSmoothFinalDirectionSpeed(), 100, 0.01f));
                fpcController.SetSmoothInputMagnitudeSpeed(AEditorGUILayout.VisualClamp(ContentProperties.SmoothInputMagnitudeSpeed, fpcController.GetSmoothInputMagnitudeSpeed(), 100, 0.01f));
            }
            EndGroupLevel();
            controllerFoldouts[0] = isExpanded;

            isExpanded = controllerFoldouts[1];
            BeginGroupLevel2(ref isExpanded, ContentProperties.AccelerationSettings);
            if (isExpanded)
            {
                fpcController.SetAccelerationDirection(AEditorGUILayout.EnumPopup(ContentProperties.AccelerationDirection, fpcController.GetAccelerationDirection()));
                fpcController.SetRunVelocityThreshold(AEditorGUILayout.FixedFloatField(ContentProperties.RunVelocityThreshold, fpcController.GetRunVelocityThreshold(), 0));
                fpcController.SetSprintVelocityThreshold(AEditorGUILayout.FixedFloatField(ContentProperties.SprintVelocityThreshold, fpcController.GetSprintVelocityThreshold(), 0));
                fpcController.SetAccelerationCurve(EditorGUILayout.CurveField(ContentProperties.AccelerationCurve, fpcController.GetAccelerationCurve()));
            }
            EndGroupLevel();
            controllerFoldouts[1] = isExpanded;

            isExpanded = controllerFoldouts[2];
            BeginGroupLevel2(ref isExpanded, ContentProperties.JumpSettings);
            if (isExpanded)
            {
                fpcController.SetJumpForce(AEditorGUILayout.FixedFloatField(ContentProperties.JumpForce, fpcController.GetJumpForce(), 0));
                fpcController.SetJumpCount(AEditorGUILayout.FixedIntField(ContentProperties.JumpCount, fpcController.GetJumpCount(), 0));
                fpcController.JumpInAir(EditorGUILayout.Toggle(ContentProperties.JumpInAir, fpcController.JumpInAir()));
            }
            EndGroupLevel();
            controllerFoldouts[2] = isExpanded;

            isExpanded = controllerFoldouts[3];
            BeginGroupLevel2(ref isExpanded, ContentProperties.AirSettings);
            if (isExpanded)
            {
                fpcController.InAirControl(EditorGUILayout.Toggle(ContentProperties.InAirControl, fpcController.InAirControl()));
                fpcController.SetInAirControlPersent(AEditorGUILayout.FixedFloatField(ContentProperties.InAirControlPersent, fpcController.GetInAirControlPersent(), 0));
            }
            EndGroupLevel();
            controllerFoldouts[3] = isExpanded;

            isExpanded = controllerFoldouts[4];
            BeginGroupLevel2(ref isExpanded, ContentProperties.CrouchSettings);
            if (isExpanded)
            {
                fpcController.SetCrouchHandleType(AEditorGUILayout.EnumPopup(ContentProperties.CrouchActionType, fpcController.GetCrouchHandleType()));
                fpcController.SetCrouchHeightPercent(AEditorGUILayout.FixedFloatField(ContentProperties.CrouchHeightPercent, fpcController.GetCrouchHeightPercent(), 0));
                fpcController.SetCrouchDuration(AEditorGUILayout.FixedFloatField(ContentProperties.CrouchDuration, fpcController.GetCrouchDuration(), 0));
                fpcController.SetCrouchCurve(EditorGUILayout.CurveField(ContentProperties.CrouchCurve, fpcController.GetCrouchCurve()));
            }
            EndGroupLevel();
            controllerFoldouts[4] = isExpanded;

            isExpanded = controllerFoldouts[5];
            BeginGroupLevel2(ref isExpanded, ContentProperties.SprintEffect);
            if (isExpanded)
            {
                FOVAnimationSettings sprintEffect = fpcController.GetSprintFOVAnimation();
                sprintEffect.SetFieldOfView(AEditorGUILayout.FixedFloatField("Field Of View", sprintEffect.GetFieldOfView(), 0));
                sprintEffect.SetDuration(AEditorGUILayout.FixedFloatField("Duration", sprintEffect.GetDuration(), 0));
                sprintEffect.SetCurve(EditorGUILayout.CurveField("Curve", sprintEffect.GetCurve()));
                fpcController.SetSprintFOVAnimation(sprintEffect);
                fpcController.UseSprintEffect(EditorGUILayout.Toggle("Enabled", fpcController.UseSprintEffect()));
            }
            EndGroupLevel();
            controllerFoldouts[5] = isExpanded;

            isExpanded = controllerFoldouts[6];
            BeginGroupLevel2(ref isExpanded, ContentProperties.HeadBobEffect);
            if (isExpanded)
            {
                CameraHeadBobEffect headBobEffect = fpcController.GetCameraHeadBobEffect();
                headBobEffect.SetSettings(AEditorGUILayout.ObjectField(ContentProperties.HeadBobSettings, headBobEffect.GetSettings(), true));
                fpcController.SetCameraHeadBobEffect(headBobEffect);
                bool previous = fpcController.UseHeadBobEffect();
                fpcController.UseHeadBobEffect(EditorGUILayout.Toggle(ContentProperties.UseHeadBobEffect, fpcController.UseHeadBobEffect()));
                if (headBobEffect.GetSettings() == null && fpcController.UseHeadBobEffect() && !previous)
                {
                    DisplayDialogs.Message("Headbob effect", "Add Headbob settings to enable headbob effect!");
                    fpcController.UseHeadBobEffect(false);
                    GUIUtility.ExitGUI();
                }
                else if (headBobEffect.GetSettings() == null && fpcController.UseHeadBobEffect())
                {
                    fpcController.UseHeadBobEffect(false);
                }
            }
            EndGroupLevel();
            controllerFoldouts[6] = isExpanded;

            isExpanded = controllerFoldouts[7];
            BeginGroupLevel2(ref isExpanded, ContentProperties.LandingEffect);
            if (isExpanded)
            {
                CameraLandingEffect landingEffect = fpcController.GetCameraLandingEffect();
                landingEffect.SetAmountLimit(AEditorGUILayout.MinMaxSlider(ContentProperties.LandingAmountLimits, landingEffect.GetAmountLimit()));
                landingEffect.SetAmountPercent(AEditorGUILayout.FixedFloatField(ContentProperties.LandingTimer, landingEffect.GetAmountPercent(), 0.01f));
                landingEffect.SetDuration(AEditorGUILayout.FixedFloatField(ContentProperties.LandingDuration, landingEffect.GetDuration(), 0.01f));
                landingEffect.SetCurve(EditorGUILayout.CurveField(ContentProperties.LandingCurve, landingEffect.GetCurve()));
                fpcController.SetCameraLandingEffect(landingEffect);
                fpcController.UseLandingEffect(EditorGUILayout.Toggle(ContentProperties.UseLandingEffect, fpcController.UseLandingEffect()));
            }
            EndGroupLevel();
            controllerFoldouts[7] = isExpanded;

            isExpanded = controllerFoldouts[8];
            BeginGroupLevel2(ref isExpanded, ContentProperties.GravitySettings);
            if (isExpanded)
            {
                fpcController.SetGravityMultiplier(AEditorGUILayout.FixedFloatField(ContentProperties.GravityMultiplier, fpcController.GetGravityMultiplier(), 0));
                fpcController.SetStickToGroundForce(AEditorGUILayout.FixedFloatField(ContentProperties.StickToGroundForce, fpcController.GetStickToGroundForce(), 0));
            }
            EndGroupLevel();
            controllerFoldouts[8] = isExpanded;

            isExpanded = controllerFoldouts[9];
            BeginGroupLevel2(ref isExpanded, ContentProperties.GroundedSettings);
            if (isExpanded)
            {
                fpcController.SetGroundCheckRange(AEditorGUILayout.FixedFloatField(ContentProperties.GroundedCheckRange, fpcController.GetGroundCheckRange(), 0));
                fpcController.SetGroundCheckRadius(AEditorGUILayout.FixedFloatField(ContentProperties.GroundedCheckRadius, fpcController.GetGroundCheckRadius(), 0));
                fpcController.SetGroundCullingLayer(AEditorGUILayout.LayerMaskField(ContentProperties.GroundedCullingLayer, fpcController.GetGroundCullingLayer()));
                if (fpcController.GetGroundCullingLayer().HasLayer(LayerMask.NameToLayer(LNC.Player)))
                {
                    HelpBoxMessages.Message(string.Format("Culling layer has a root object layer: [{0}]. Root object layer will be removed from culling layer on game start.", LNC.Player), MessageType.Warning, true);
                }
            }
            EndGroupLevel();
            controllerFoldouts[9] = isExpanded;

            isExpanded = controllerFoldouts[10];
            BeginGroupLevel2(ref isExpanded, ContentProperties.RoofSettings);
            if (isExpanded)
            {
                fpcController.SetRoofCheckRadius(AEditorGUILayout.FixedFloatField(ContentProperties.RoofCheckRadius, fpcController.GetRoofCheckRadius(), 0));
                fpcController.SetRoofCullingLayer(AEditorGUILayout.LayerMaskField(ContentProperties.GroundedCullingLayer, fpcController.GetRoofCullingLayer()));
                if (fpcController.GetRoofCullingLayer().HasLayer(LayerMask.NameToLayer(LNC.Player)))
                {
                    HelpBoxMessages.Message(string.Format("Culling layer has a root object layer: [{0}]. Root object layer will be removed from culling layer on game start.", LNC.Player), MessageType.Warning, true);
                }
            }
            EndGroupLevel();
            controllerFoldouts[10] = isExpanded;

            isExpanded = controllerFoldouts[11];
            BeginGroupLevel2(ref isExpanded, ContentProperties.WallSettings);
            if (isExpanded)
            {
                fpcController.SetWallCheckRange(AEditorGUILayout.FixedFloatField(ContentProperties.WallCheckRange, fpcController.GetWallCheckRange(), 0));
                fpcController.SetWallCheckRadius(AEditorGUILayout.FixedFloatField(ContentProperties.WallCheckRadius, fpcController.GetWallCheckRadius(), 0));
                fpcController.SetWallCullingLayer(AEditorGUILayout.LayerMaskField(ContentProperties.WallCullingLayer, fpcController.GetWallCullingLayer()));
                if (fpcController.GetWallCullingLayer().HasLayer(LayerMask.NameToLayer(LNC.Player)))
                {
                    HelpBoxMessages.Message(string.Format("Culling layer has a root object layer: [{0}]. Root object layer will be removed from culling layer on game start.", LNC.Player), MessageType.Warning, true);
                }
            }
            EndGroupLevel();
            controllerFoldouts[11] = isExpanded;


            GUILayout.Label(fpcController.GetControllerState().ToString());
            DecreaseIndentLevel();
        }
    }
}
