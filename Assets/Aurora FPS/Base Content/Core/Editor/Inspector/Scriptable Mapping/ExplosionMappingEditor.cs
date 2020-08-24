/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using System.Collections.Generic;
using AuroraFPSRuntime;
using UnityEditor;
using UnityEngine;

namespace AuroraFPSEditor
{
    [CustomEditor(typeof(ExplosionMapping))]
    public class ExplosionMappingEditor : AuroraEditor<ExplosionMapping>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent Properties = new GUIContent("Properties");
            public readonly static GUIContent Damage = new GUIContent("Damage", "Explosion damage.");
            public readonly static GUIContent Amplitude = new GUIContent("Amplitude", "Explosion amplitude.");
            public readonly static GUIContent Impulse = new GUIContent("Impulse", "Exploxion physics impulse.");
            public readonly static GUIContent UpwardsModifier = new GUIContent("Upwards Modifier", "Exploxion physics upwards modifier.");
            public readonly static GUIContent Distance = new GUIContent("Distance", "Exploxion distance.");
        }

        private SerializedProperty mappingValues;
        private List<bool> foldouts;
        private bool showPropertyInfo;

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        public override void InitializeProperties()
        {
            mappingValues = serializedObject.FindProperty("mappingValues");
            CreateFoldoutsList(ref foldouts);
        }

        /// <summary>
        /// Base serializedObject properties.
        /// 
        /// Implement this method to draw a custom properties.
        /// </summary>
        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.Properties);

            if (instance.GetMappingLength() == 0)
            {
                MessageHelper();
            }

            DrawExplosionProperties();
            GUILayout.Space(5);
            DrawOptionButtons();

            EndGroup();
        }

        /// <summary>
        /// Draw exaplosion mapping properties.
        /// </summary>
        protected virtual void DrawExplosionProperties()
        {
            IncreaseIndentLevel();
            for (int i = 0, length = instance.GetMappingLength(); i < length; i++)
            {
                ExplosionProperty property = instance.GetMappingValue(i);
                bool foldout = foldouts[i];
                string name = string.Format("Property {0}", i + 1);
                if (showPropertyInfo)
                {
                    name = string.Format("{0} [Damage: {1}, Amplitude: {2}, Distance: ({3}-{4})]",
                        name,
                        property.GetDamage(),
                        property.GetAmplitude().ToString("0.00"),
                        property.GetMinDistance().ToString("0.00"),
                        property.GetMaxDistance().ToString("0.00"));
                }
                BeginGroupLevel2(ref foldout, name);

                if (foldout)
                {
                    property.SetDamage(EditorGUILayout.IntField(ContentProperties.Damage, property.GetDamage()));
                    property.SetAmplitude(EditorGUILayout.FloatField(ContentProperties.Amplitude, property.GetAmplitude()));
                    property.SetImpulse(EditorGUILayout.FloatField(ContentProperties.Impulse, property.GetImpulse()));
                    property.SetUpwardsModifier(EditorGUILayout.FloatField(ContentProperties.UpwardsModifier, property.GetUpwardsModifier()));

                    float min = property.GetMinDistance();
                    float max = property.GetMaxDistance();
                    AEditorGUILayout.MinMaxSlider(ContentProperties.Distance, ref min, ref max, -100, 100);
                    property.SetMinDistance(min);
                    property.SetMaxDistance(max);

                    GUILayout.Space(5);
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Remove", AEditorGUILayout.RemoveElementButtonOptions))
                    {
                        mappingValues.DeleteArrayElementAtIndex(i);
                        foldouts.RemoveAt(i);
                        break;
                    }
                    GUILayout.EndHorizontal();
                    instance.SetMappingValue(i, property);
                }

                EndGroupLevel();
                foldouts[i] = foldout;
            }
            DecreaseIndentLevel();
        }

        /// <summary>
        /// Draw explosion mapping option buttons.
        /// </summary>
        protected virtual void DrawOptionButtons()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Add Property", "ButtonLeft", AEditorGUILayout.DefaultHandleButtonOptions))
            {
                mappingValues.arraySize++;
                foldouts.Add(false);
            }

            if (GUILayout.Button("Clear Properties", "ButtonMid", AEditorGUILayout.DefaultHandleButtonOptions) && DisplayDialogs.Confirmation("Are you really want to remove all explosion proeprties?"))
            {
                mappingValues.ClearArray();
                foldouts.Clear();
            }

            GUIStyle buttonStyle = new GUIStyle("ButtonRight");
            GUIStyle pressedButtonStyle = new GUIStyle(buttonStyle);
            pressedButtonStyle.normal = pressedButtonStyle.onActive;
            if (GUILayout.Button(new GUIContent("Show Info", "Show properties info on property label"), showPropertyInfo ? pressedButtonStyle : buttonStyle, AEditorGUILayout.DefaultHandleButtonOptions))
            {
                showPropertyInfo = !showPropertyInfo;
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Message helper displayed when explosion mapping is empty.
        /// </summary>
        protected virtual void MessageHelper()
        {
            HelpBoxMessages.Message("Explosion Mapping is empty!", MessageType.Warning);
        }

        /// <summary>
        /// Create editor GUI foldouts bool values.
        /// </summary>
        private void CreateFoldoutsList(ref List<bool> foldouts)
        {
            foldouts = new List<bool>();

            for (int i = 0, length = instance.GetMappingLength(); i < length; i++)
            {
                foldouts.Add(false);
            }
        }
    }
}