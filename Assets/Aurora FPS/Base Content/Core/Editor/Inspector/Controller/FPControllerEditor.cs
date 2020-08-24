/* ==================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================== */

using UnityEngine;
using AuroraFPSRuntime;
using System.Collections.Generic;
using UnityEditor;

namespace AuroraFPSEditor
{
    [CustomEditor(typeof(FPController), true)]
    public class FPControllerEditor : AuroraEditor<FPController>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent CameraControl = new GUIContent("Camera Properties", "Camera Control properties.");

            public readonly static GUIContent CameraBaseSettings = new GUIContent("Base Settings", "Camera base settings.");
            public readonly static GUIContent CameraInstance = new GUIContent("Camera Instance", "Camera instance transform.");
            public readonly static GUIContent CameraPivot = new GUIContent("Camera Pivot", "First person camera pivot transform.");
            public readonly static GUIContent Sensitivity = new GUIContent("Sensitivity", "Mouse sensitivity by X and Y axis.");
            public readonly static GUIContent RotationSmooth = new GUIContent("Smooth", "Mouse smooth value by X and Y axis.");
            public readonly static GUIContent VerticalLimits = new GUIContent("Vertical Limits", "Limit rotation range by Y axis.");

            public readonly static GUIContent CameraZoomSettings = new GUIContent("Zoom Settings", "Camera zoom settings.");
            public readonly static GUIContent ZoomAction = new GUIContent("Action", "Zoom action type.");
            public readonly static GUIContent ZoomFOV = new GUIContent("Field Of View", "Zoom field of view settings.");
            public readonly static GUIContent ZoomDuration = new GUIContent("Duration", "Zoom duration.");
            public readonly static GUIContent ZoomCurve = new GUIContent("Curve", "Zoom duration curve.");

            public readonly static GUIContent CameraSwaySettings = new GUIContent("Sway Settings", "Camera sway settings.");
            public readonly static GUIContent SwayAmout = new GUIContent("Amount", "Camera sway settings.");
            public readonly static GUIContent SwayIncreaseSpeed = new GUIContent("Increase Speed", "Camera sway settings.");
            public readonly static GUIContent SwayDecreaseSpeed = new GUIContent("Decrease Speed", "Camera sway settings.");

            public readonly static GUIContent CameraOtherSettings = new GUIContent("Other Settings", "Camera other settings.");
            public readonly static GUIContent DefaultFOVSettings = new GUIContent("Default FOV Settings", "Camera other settings.");
            public readonly static GUIContent DefaultFOV = new GUIContent("Field Of View", "Zoom field of view settings.");
            public readonly static GUIContent DefaultDuration = new GUIContent("Duration", "Zoom duration.");
            public readonly static GUIContent DefaultCurve = new GUIContent("Curve", "Zoom duration curve.");

            public readonly static GUIContent Controller = new GUIContent("Controller Properties", "Controller properties.");
        }

        private bool[] cameraControlFoldouts;

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// 
        /// Implement this method to initialize properties.
        /// </summary>
        public override void InitializeProperties()
        {
            base.InitializeProperties();
            cameraControlFoldouts = new bool[5];
        }

        /// <summary>
        /// Base serializedObject properties.
        /// 
        /// Implement this method to draw a custom properties.
        /// </summary>
        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.CameraControl);
            OnCameraControlGUI();
            EndGroup();

            BeginGroup(ContentProperties.Controller);
            OnControllerGUI();
            EndGroup();
        }

        /// <summary>
        /// Properties of first person camera control instance.
        /// </summary>
        public virtual void OnCameraControlGUI()
        {
            IncreaseIndentLevel();
            CameraControl cameraControl = instance.GetCameraControl();
            bool isExpanded = false;
            isExpanded = cameraControlFoldouts[0];
            BeginGroupLevel2(ref isExpanded, ContentProperties.CameraBaseSettings);
            if (isExpanded)
            {
                cameraControl.SetInstanceTransform(AEditorGUILayout.ObjectField(ContentProperties.CameraInstance, cameraControl.GetInstanceTransform(), true));
                cameraControl.SetPivotTransform(AEditorGUILayout.ObjectField(ContentProperties.CameraPivot, cameraControl.GetPivotTransform(), true));
                cameraControl.SetSensitivity(EditorGUILayout.Vector2Field(ContentProperties.Sensitivity, cameraControl.GetSensitivity()));
                cameraControl.SetRotationSmooth(EditorGUILayout.Vector2Field(ContentProperties.RotationSmooth, cameraControl.GetRotationSmooth()));
                cameraControl.SetVerticalRotationLimits(AEditorGUILayout.MinMaxSlider(ContentProperties.VerticalLimits, cameraControl.GetVerticalRotationLimits()));
            }
            EndGroupLevel();
            cameraControlFoldouts[0] = isExpanded;

            isExpanded = cameraControlFoldouts[1];
            BeginGroupLevel2(ref isExpanded, ContentProperties.CameraZoomSettings);
            if (isExpanded)
            {
                cameraControl.SetZoomHandleType(AEditorGUILayout.EnumPopup(ContentProperties.ZoomAction, cameraControl.GetZoomHandleType()));

                FOVAnimationSettings zoomSettings = cameraControl.GetZoomSettings();
                zoomSettings.SetFieldOfView(AEditorGUILayout.FixedFloatField(ContentProperties.ZoomFOV, zoomSettings.GetFieldOfView(), 0));
                zoomSettings.SetDuration(AEditorGUILayout.FixedFloatField(ContentProperties.ZoomDuration, zoomSettings.GetDuration(), 0.01f));
                zoomSettings.SetCurve(EditorGUILayout.CurveField(ContentProperties.ZoomCurve, zoomSettings.GetCurve()));
                cameraControl.SetZoomSettings(zoomSettings);
            }
            EndGroupLevel();
            cameraControlFoldouts[1] = isExpanded;

            isExpanded = cameraControlFoldouts[2];
            BeginGroupLevel2(ref isExpanded, ContentProperties.CameraSwaySettings);
            if (isExpanded)
            {
                CameraSideSway sideSway = cameraControl.GetSideSway();
                sideSway.SetSwayAmount(AEditorGUILayout.FixedFloatField(ContentProperties.SwayAmout, sideSway.GetSwayAmount(), 0));
                sideSway.SetSwaySpeed(AEditorGUILayout.FixedFloatField(ContentProperties.SwayIncreaseSpeed, sideSway.GetSwaySpeed(), 0));
                sideSway.SetReturnSpeed(AEditorGUILayout.FixedFloatField(ContentProperties.SwayDecreaseSpeed, sideSway.GetReturnSpeed(), 0));
                cameraControl.SetSideSway(sideSway);
            }
            EndGroupLevel();
            cameraControlFoldouts[2] = isExpanded;

            isExpanded = cameraControlFoldouts[3];
            BeginGroupLevel2(ref isExpanded, ContentProperties.CameraOtherSettings);
            if (isExpanded)
            {
                bool isExpandedChild = cameraControlFoldouts[4];
                BeginGroupLevel3(ref isExpandedChild, ContentProperties.DefaultFOVSettings);
                if (isExpandedChild)
                {
                    FOVAnimationSettings defaultFOV = cameraControl.GetDefaultFOVSettings();
                    defaultFOV.SetFieldOfView(AEditorGUILayout.FixedFloatField(ContentProperties.DefaultFOV, defaultFOV.GetFieldOfView(), 0));
                    defaultFOV.SetDuration(AEditorGUILayout.FixedFloatField(ContentProperties.DefaultDuration, defaultFOV.GetDuration(), 0.01f));
                    defaultFOV.SetCurve(EditorGUILayout.CurveField(ContentProperties.DefaultCurve, defaultFOV.GetCurve()));
                    cameraControl.SetDefaultFOVSettings(defaultFOV);
                }
                EndGroupLevel();
                cameraControlFoldouts[4] = isExpandedChild;
            }
            EndGroupLevel();
            cameraControlFoldouts[3] = isExpanded;
            instance.SetCameraControl(cameraControl);
            DecreaseIndentLevel();
        }

        /// <summary>
        /// Properties of first person controller instance.
        /// </summary>
        public virtual void OnControllerGUI()
        {
            DrawPropertiesExcluding(serializedObject, GetExcludingProperties());
        }

        /// <summary>
        /// Add new custom exclusive properties.
        /// 
        /// Implement this method to add new custom exclusive properties that will be hidden in the inspector GUI.
        /// </summary>
        /// <param name="excludingProperties">Properties that will be hidden in the inspector GUI</param>
        protected override void AddExcludingProperties(ref List<string> excludingProperties)
        {
            base.AddExcludingProperties(ref excludingProperties);
            excludingProperties.Add("cameraControl");
        }
    }
}

