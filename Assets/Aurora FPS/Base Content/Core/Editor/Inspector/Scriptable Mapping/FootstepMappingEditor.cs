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
using UnityEngine;
using ArraysToolbarEditorProperty = AuroraFPSEditor.AEditorGUILayout.ArraysToolbarEditorProperty;


namespace AuroraFPSEditor
{
    [CustomEditor(typeof(FootstepMapping))]
    public class FootstepMappingEditor : AuroraEditor<FootstepMapping>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent Mapping = new GUIContent("Mapping");
            public readonly static GUIContent StepToolbar = new GUIContent("Step", "Sounds of steps that will be randomly played on this key.");
            public readonly static GUIContent JumpToolbar = new GUIContent("Jump", "Sounds of jump that will be randomly played on this key.");
            public readonly static GUIContent LandToolbar = new GUIContent("Land", "Sounds of land that will be randomly played on this key.");
            public readonly static GUIContent StepSound = new GUIContent("Sound", "Sounds of steps that will be randomly played on this key.");
            public readonly static GUIContent JumpSound = new GUIContent("Sound", "Sounds of jump that will be randomly played on this key.");
            public readonly static GUIContent LandSound = new GUIContent("Sound", "Sounds of land that will be randomly played on this key.");
        }

        private DictionaryEditor<Object, FootstepSounds> serializedDictionary;
        private ArraysToolbarEditorProperty[] arraysToolbarEditorProperties;
        private int selected = 0;
        private Object key;

        public override void InitializeProperties()
        {
            SerializedProperty mapping = serializedObject.FindProperty("mapping");

            serializedDictionary = new DictionaryEditor<Object, FootstepSounds>(instance.GetMapping(), mapping);

            arraysToolbarEditorProperties = new ArraysToolbarEditorProperty[3]
            {
                new ArraysToolbarEditorProperty(ContentProperties.StepToolbar, ContentProperties.StepSound, null),
                new ArraysToolbarEditorProperty(ContentProperties.JumpToolbar, ContentProperties.JumpSound, null),
                new ArraysToolbarEditorProperty(ContentProperties.LandToolbar, ContentProperties.LandSound, null)
            };

            serializedDictionary.onAddWindowShowCallback = () =>
            {
                GenericMenu genericMenu = new GenericMenu();
                genericMenu.AddItem(new GUIContent("Physics Material"), false, () =>
                {
                    serializedDictionary.onAddWindowGUICallback = (ref Object key, ref FootstepSounds property) =>
                    {
                        key = AEditorGUILayout.ObjectField<PhysicMaterial>(GUIContent.none, key as PhysicMaterial, true);
                    };
                    serializedDictionary.showWindow = true;
                });

                genericMenu.AddItem(new GUIContent("Texture2D"), false, () =>
                {
                    serializedDictionary.onAddWindowGUICallback = (ref Object key, ref FootstepSounds property) =>
                    {
                        key = AEditorGUILayout.ObjectField<Texture2D>(GUIContent.none, key as Texture2D, true, GUILayout.Height(EditorGUIUtility.singleLineHeight));
                    };
                    serializedDictionary.showWindow = true;
                });
                genericMenu.ShowAsContext();
            };

            serializedDictionary.onElementGUICallback = (key, value, index) =>
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(key, new GUIContent("Target"));
                EditorGUI.EndDisabledGroup();

                GUILayout.Space(3);

                arraysToolbarEditorProperties[0].serializedArray = value.FindPropertyRelative("stepSounds");
                arraysToolbarEditorProperties[1].serializedArray = value.FindPropertyRelative("jumpSounds");
                arraysToolbarEditorProperties[2].serializedArray = value.FindPropertyRelative("landSounds");

                AEditorGUILayout.ArraysToolbarEditor(ref selected, arraysToolbarEditorProperties);
            };

            serializedDictionary.propertyLabelCallback = (key, value, index) =>
            {
                return new GUIContent(string.Format("{0} ({1})", key?.name, key?.GetType().Name));
            };
        }

        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.Mapping);
            serializedDictionary.DrawLayoutDictionary();
            EndGroup();
        }
    }
}