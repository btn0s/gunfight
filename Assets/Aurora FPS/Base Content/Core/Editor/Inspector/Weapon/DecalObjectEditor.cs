/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using AuroraFPSRuntime;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace AuroraFPSEditor
{
    [CustomEditor(typeof(DecalObject), true)]
    [CanEditMultipleObjects]
    public class DecalObjectEditor : PoolObjectEditor
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent OriginalTransform = new GUIContent("Original Transform");
            public readonly static GUIContent OriginalScale = new GUIContent("Original Scale");
            public readonly static GUIContent OriginalRotation = new GUIContent("Original Rotation");

            public readonly static GUIContent RandomizeTransform = new GUIContent("Randomize Transform");
            public readonly static GUIContent RotationAxis = new GUIContent("Rotation Axis");
            public readonly static GUIContent RandomRotationRange = new GUIContent("Random Rotation Range");
            public readonly static GUIContent RandomizeRotation = new GUIContent("Randomize Rotation");
            public readonly static GUIContent RandomScaleRange = new GUIContent("Random ScaleRange");
            public readonly static GUIContent RandomizeScale = new GUIContent("Randomize Scale");

            public readonly static GUIContent Sounds = new GUIContent("Sounds");

        }

        private DecalObject decalObjectInstance;
        private ReorderableList soundsList;
        private bool originalValuesFoldout;
        private bool randomizeTransformValues;
        private bool soundsFoldout;

        public override void InitializeProperties()
        {
            base.InitializeProperties();
            decalObjectInstance = instance as DecalObject;
            soundsList = new ReorderableList(serializedObject, serializedObject.FindProperty("sounds"));
            soundsList.headerHeight = 2.0f;
            soundsList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                rect.y += 1.5f;
                rect.height = EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(rect, serializedObject.FindProperty("sounds").GetArrayElementAtIndex(index), GUIContent.none);
            };
        }

        /// <summary>
        /// Drawing all childs implementation properties.
        /// </summary>
        public override void OnImplementationPropertiesGUI()
        {
            OnDecalObjectPropertiesGUI();
            base.OnImplementationPropertiesGUI();
        }

        /// <summary>
        /// On decal object properties GUI.
        /// 
        /// Implement this method to make a custom draw of decal object properties.
        /// </summary>
        public virtual void OnDecalObjectPropertiesGUI()
        {
            IncreaseIndentLevel();
            BeginGroupLevel2(ref originalValuesFoldout, ContentProperties.OriginalTransform);
            if (originalValuesFoldout)
            {
                decalObjectInstance.SetOriginalRotation(EditorGUILayout.Vector3Field(ContentProperties.OriginalRotation, decalObjectInstance.GetOriginalRotation()));
                decalObjectInstance.SetOriginalScale(EditorGUILayout.Vector3Field(ContentProperties.OriginalScale, decalObjectInstance.GetOriginalScale()));
            }
            EndGroupLevel();

            BeginGroupLevel2(ref randomizeTransformValues, ContentProperties.RandomizeTransform);
            if (randomizeTransformValues)
            {
                EditorGUI.BeginDisabledGroup(!decalObjectInstance.RandomizeRotation());
                decalObjectInstance.SetRotationAxis(AEditorGUILayout.EnumPopup(ContentProperties.RotationAxis, decalObjectInstance.GetRotationAxis()));
                float minRandomRotation = decalObjectInstance.GetMinRandomRotation();
                float maxRandomRotation = decalObjectInstance.GetMaxRandomRotation();
                AEditorGUILayout.MinMaxSlider(ContentProperties.RandomRotationRange, ref minRandomRotation, ref maxRandomRotation, -180, 180);
                decalObjectInstance.SetMinRandomRotation(minRandomRotation);
                decalObjectInstance.SetMaxRandomRotation(maxRandomRotation);
                EditorGUI.EndDisabledGroup();
                decalObjectInstance.RandomizeRotation(EditorGUILayout.Toggle(ContentProperties.RandomizeRotation, decalObjectInstance.RandomizeRotation()));

                EditorGUILayout.Space();

                EditorGUI.BeginDisabledGroup(!decalObjectInstance.RandomizeScale());
                float minRandomScale = decalObjectInstance.GetMinRandomScale();
                float maxRandomScale = decalObjectInstance.GetMaxRandomScale();
                AEditorGUILayout.MinMaxSlider(ContentProperties.RandomScaleRange, ref minRandomScale, ref maxRandomScale, 0, 10);
                decalObjectInstance.SetMinRandomScale(minRandomScale);
                decalObjectInstance.SetMaxRandomScale(maxRandomScale);
                EditorGUI.EndDisabledGroup();
                decalObjectInstance.RandomizeScale(EditorGUILayout.Toggle(ContentProperties.RandomizeScale, decalObjectInstance.RandomizeScale()));
            }
            EndGroupLevel();

            BeginGroupLevel2(ref soundsFoldout, ContentProperties.Sounds);
            if (soundsFoldout)
            {
                DecreaseIndentLevel();
                soundsList.DoLayoutList();
                IncreaseIndentLevel();
            }
            EndGroupLevel();
            DecreaseIndentLevel();
        }

        /// <summary>
        /// Add new custom exclusive properties.
        /// 
        /// Implement this method to add new custom exclusive properties that will be hidden in the inspector GUI.
        /// </summary>
        /// <param name="excludingProperties">Properties that will be hidden in the inspector GUI</param>
        protected override void AddExcludingProperties(ref System.Collections.Generic.List<string> excludingProperties)
        {
            base.AddExcludingProperties(ref excludingProperties);
            excludingProperties.Add("originalScale");
            excludingProperties.Add("originalRotation");
            excludingProperties.Add("minRandomScale");
            excludingProperties.Add("maxRandomScale");
            excludingProperties.Add("randomizeScale");
            excludingProperties.Add("rotationAxis");
            excludingProperties.Add("minRandomRotation");
            excludingProperties.Add("maxRandomRotation");
            excludingProperties.Add("randomizeRotation");
            excludingProperties.Add("sounds");
        }
    }
}