/* ==================================================================
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
    [CustomEditor(typeof(HeadBobSettings))]
    public class HeadBobSettingsEditor : AuroraEditor<HeadBobSettings>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent BaseProperties = new GUIContent("Base Properties");
            public readonly static GUIContent AmplitudeX = new GUIContent("Amplitude X", "Amplitude by axis X.");
            public readonly static GUIContent AmplitudeY = new GUIContent("Amplitude Y", "Amplitude by axis Y.");
            public readonly static GUIContent FrequencyX = new GUIContent("Frequency X", "Frequency by axis X.");
            public readonly static GUIContent FrequencyY = new GUIContent("Frequency Y", "Frequency by axis Y.");
            public readonly static GUIContent CurveX = new GUIContent("Curve X", "Curve evaluate by axis X.");
            public readonly static GUIContent CurveY = new GUIContent("Curve Y", "Curve evaluate by axis Y.");
            public readonly static GUIContent WalkMultiplier = new GUIContent("Walk Multiplier");
            public readonly static GUIContent RunMultiplier = new GUIContent("Run Multiplier");
            public readonly static GUIContent SprintMultiplier = new GUIContent("Sprint Multiplier");
            public readonly static GUIContent CrouchMultiplier = new GUIContent("Crouch Multiplier");
            public readonly static GUIContent ZoomMultiplier = new GUIContent("Zoom Multiplier");
            public readonly static GUIContent AmplitudeMultiplier = new GUIContent("Amplitude", "Amplitude multiplier while active this state.");
            public readonly static GUIContent FrequencyMultiplier = new GUIContent("Frequency", "Frequency multiplier while active this state.");
        }

        private bool walkMultiplierIsExpanded;
        private bool runMultiplierIsExpanded;
        private bool sprintkMultiplierIsExpanded;
        private bool crouchMultiplierIsExpanded;
        private bool zoomMultiplierIsExpanded;

        /// <summary>
        /// Base serializedObject properties.
        /// 
        /// Implement this method to draw a custom properties.
        /// </summary>
        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.BaseProperties);
            instance.SetAmplitudeX(AEditorGUILayout.FixedFloatField(ContentProperties.AmplitudeX, instance.GetAmplitudeX(), 0));
            instance.SetAmplitudeY(AEditorGUILayout.FixedFloatField(ContentProperties.AmplitudeY, instance.GetAmplitudeY(), 0));
            instance.SetFrequencyX(AEditorGUILayout.FixedFloatField(ContentProperties.FrequencyX, instance.GetFrequencyX(), 0));
            instance.SetFrequencyY(AEditorGUILayout.FixedFloatField(ContentProperties.FrequencyY, instance.GetFrequencyY(), 0));
            instance.SetCurveX(EditorGUILayout.CurveField(ContentProperties.CurveX, instance.GetCurveX()));
            instance.SetCurveY(EditorGUILayout.CurveField(ContentProperties.CurveY, instance.GetCurveY()));

            IncreaseIndentLevel();
            BeginGroupLevel2(ref walkMultiplierIsExpanded, ContentProperties.WalkMultiplier);
            if (walkMultiplierIsExpanded)
            {
                HeadBobSettings.Multiplier walkMultiplier = instance.GetWalkMultiplier();
                walkMultiplier.SetAmplitude(AEditorGUILayout.FixedFloatField(ContentProperties.AmplitudeMultiplier, walkMultiplier.GetAmplitude(), 0));
                walkMultiplier.SetFrequency(AEditorGUILayout.FixedFloatField(ContentProperties.FrequencyMultiplier, walkMultiplier.GetFrequency(), 0));
                instance.SetWalkMultiplier(walkMultiplier);
            }
            EndGroupLevel();

            BeginGroupLevel2(ref runMultiplierIsExpanded, ContentProperties.RunMultiplier);
            if (runMultiplierIsExpanded)
            {
                HeadBobSettings.Multiplier runMultiplier = instance.GetRunMultiplier();
                runMultiplier.SetAmplitude(AEditorGUILayout.FixedFloatField(ContentProperties.AmplitudeMultiplier, runMultiplier.GetAmplitude(), 0));
                runMultiplier.SetFrequency(AEditorGUILayout.FixedFloatField(ContentProperties.FrequencyMultiplier, runMultiplier.GetFrequency(), 0));
                instance.SetRunMultiplier(runMultiplier);
            }
            EndGroupLevel();

            BeginGroupLevel2(ref sprintkMultiplierIsExpanded, ContentProperties.SprintMultiplier);
            if (sprintkMultiplierIsExpanded)
            {
                HeadBobSettings.Multiplier sprintMultiplier = instance.GetSprintMultiplier();
                sprintMultiplier.SetAmplitude(AEditorGUILayout.FixedFloatField(ContentProperties.AmplitudeMultiplier, sprintMultiplier.GetAmplitude(), 0));
                sprintMultiplier.SetFrequency(AEditorGUILayout.FixedFloatField(ContentProperties.FrequencyMultiplier, sprintMultiplier.GetFrequency(), 0));
                instance.SetSprintMultiplier(sprintMultiplier);
            }
            EndGroupLevel();

            BeginGroupLevel2(ref crouchMultiplierIsExpanded, ContentProperties.CrouchMultiplier);
            if (crouchMultiplierIsExpanded)
            {
                HeadBobSettings.Multiplier crouchMultiplier = instance.GetCrouchMultiplier();
                crouchMultiplier.SetAmplitude(AEditorGUILayout.FixedFloatField(ContentProperties.AmplitudeMultiplier, crouchMultiplier.GetAmplitude(), 0));
                crouchMultiplier.SetFrequency(AEditorGUILayout.FixedFloatField(ContentProperties.FrequencyMultiplier, crouchMultiplier.GetFrequency(), 0));
                instance.SetCrouchMultiplier(crouchMultiplier);
            }
            EndGroupLevel();

            BeginGroupLevel2(ref zoomMultiplierIsExpanded, ContentProperties.ZoomMultiplier);
            if (zoomMultiplierIsExpanded)
            {
                HeadBobSettings.Multiplier zoomMultiplier = instance.GetZoomMultiplier();
                zoomMultiplier.SetAmplitude(AEditorGUILayout.FixedFloatField(ContentProperties.AmplitudeMultiplier, zoomMultiplier.GetAmplitude(), 0));
                zoomMultiplier.SetFrequency(AEditorGUILayout.FixedFloatField(ContentProperties.FrequencyMultiplier, zoomMultiplier.GetFrequency(), 0));
                instance.SetZoomMultiplier(zoomMultiplier);
            }
            EndGroupLevel();
            DecreaseIndentLevel();
            EndGroup();
        }
    }
}