/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using System.Collections.Generic;
using System.Linq;
using AuroraFPSRuntime;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace AuroraFPSEditor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(FootstepSoundSystem), true)]
    public class FootstepSoundSystemEditor : AuroraEditor<FootstepSoundSystem>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent BaseProperties = new GUIContent("Base Properties");
            public readonly static GUIContent PlaySoundType = new GUIContent("Play Sound");
            public readonly static GUIContent FootstepMapping = new GUIContent("Mapping");
            public readonly static GUIContent FootstepIntervals = new GUIContent("Footstep Properties");
            public readonly static GUIContent GroundLayer = new GUIContent("Ground Layer", "Footstep culling ground layer.");
        }

        private SerializedProperty footstepIntervals;
        private ReorderableList footstepIntervalsList;
        private float maxSpeed = 20.0f;
        private bool editMaxSpeed = false;
        private bool debug;

        public override void InitializeProperties()
        {
            footstepIntervals = serializedObject.FindProperty("footStepIntervals");
            InitializeVelocityList(ref footstepIntervalsList);
        }

        /// <summary>
        /// Base inspector GUI.
        /// 
        /// Implement this method to draw a custom properties.
        /// </summary>
        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.BaseProperties);
            instance.SetFootstepMapping(AEditorGUILayout.ObjectField(ContentProperties.FootstepMapping, instance.GetFootstepMapping(), true));
            instance.SetPlaySoundType((PlaySoundType)EditorGUILayout.EnumPopup(ContentProperties.PlaySoundType, instance.GetPlaySoundType()));
            instance.SetGroundLayer(AEditorGUILayout.LayerMaskField(ContentProperties.GroundLayer, instance.GetGroundLayer()));
            DrawPropertiesExcluding(serializedObject, GetExcludingProperties());
            IncreaseIndentLevel();
            footstepIntervals.isExpanded = BeginGroupLevel2(footstepIntervals.isExpanded, ContentProperties.FootstepIntervals);
            if (footstepIntervals.isExpanded)
            {
                DecreaseIndentLevel();
                footstepIntervalsList.DoLayoutList();
                IncreaseIndentLevel();
            }
            EndGroupLevel();

            BeginGroupLevel2(ref debug, "Debug");
            if (debug)
            {
                EditorGUI.BeginDisabledGroup(true);
                if (EditorApplication.isPlaying)
                {
                    EditorGUILayout.FloatField("Current Velocity", instance.GetVelocity().sqrMagnitude);
                }
                else
                {
                    EditorGUILayout.TextField("Current Velocity", "Not available in edit mode...");
                }
                EditorGUI.EndDisabledGroup();
            }
            EndGroupLevel();

            DecreaseIndentLevel();
            EndGroup();
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
            excludingProperties.Add("playSoundType");
            excludingProperties.Add("footstepMapping");
            excludingProperties.Add("footStepIntervals");
            excludingProperties.Add("groundLayer");
        }

        /// <summary>
        /// Initialize velocity intervals reorderable list.
        /// </summary>
        protected virtual void InitializeVelocityList(ref ReorderableList footstepIntervalsList)
        {
            maxSpeed = FindMaxVelocity();
            footstepIntervalsList = new ReorderableList(serializedObject, footstepIntervals, true, true, true, true)
            {
                drawHeaderCallback = (rect) =>
                    {
                        EditorGUI.LabelField(new Rect(rect.x + 25, rect.y, 65, EditorGUIUtility.singleLineHeight), "Rate");
                        EditorGUI.LabelField(new Rect(rect.x + 77.5f, rect.y, 77, EditorGUIUtility.singleLineHeight), "Min Velocity");
                        float x = editMaxSpeed ? 90 : 35;
                        if (GUI.Button(new Rect(rect.width - x, rect.y, 75, EditorGUIUtility.singleLineHeight), "Max Velocity", EditorStyles.label))
                            editMaxSpeed = !editMaxSpeed;
                        if (editMaxSpeed)
                            maxSpeed = EditorGUI.FloatField(new Rect(rect.width - 9, rect.y, 35, EditorGUIUtility.singleLineHeight), GUIContent.none, maxSpeed);
                    },

                    drawElementCallback = (rect, index, isActive, isFocused) =>
                    {
                        SerializedProperty property = footstepIntervals.GetArrayElementAtIndex(index);
                        SerializedProperty rate = property.FindPropertyRelative("rate");
                        SerializedProperty minVelocity = property.FindPropertyRelative("minVelocity");
                        SerializedProperty maxVelocity = property.FindPropertyRelative("maxVelocity");

                        rate.floatValue = AEditorGUI.FixedFloatField(new Rect(rect.x + 7.5f, rect.y + 1, 35, EditorGUIUtility.singleLineHeight), rate.floatValue, 0.1f);
                        EditorGUI.LabelField(new Rect(rect.x + 55, rect.y + 1, 50, EditorGUIUtility.singleLineHeight), "->");
                        EditorGUI.PropertyField(new Rect(rect.x + 77.5f, rect.y + 1, 35, EditorGUIUtility.singleLineHeight), minVelocity, GUIContent.none);
                        float min = minVelocity.floatValue;
                        float max = maxVelocity.floatValue;
                        EditorGUI.MinMaxSlider(new Rect(rect.x + 120, rect.y + 1, rect.width - 175, EditorGUIUtility.singleLineHeight), ref min, ref max, 0, maxSpeed);
                        minVelocity.floatValue = AMath.AllocatePart(min);
                        maxVelocity.floatValue = AMath.AllocatePart(max);
                        EditorGUI.PropertyField(new Rect(rect.width + 5, rect.y + 1, 35, EditorGUIUtility.singleLineHeight), maxVelocity, GUIContent.none);
                    }
            };
        }

        /// <summary>
        /// Find max velocity value.
        /// </summary>
        /// <returns></returns>
        private float FindMaxVelocity()
        {
            if (instance.GetStepInterval() == null || instance.GetStepInterval().Length == 0)
                return 50;
            float velocity = instance.GetStepInterval().Max(h => h.GetMaxVelocity());
            return Mathf.Ceil(velocity);
        }
    }
}