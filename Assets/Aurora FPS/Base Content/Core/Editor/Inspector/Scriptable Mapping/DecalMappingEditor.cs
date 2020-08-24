/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using AuroraFPSRuntime;
using AuroraFPSRuntime.Serialization.Collections;
using UnityEditor;
using UnityEngine;
using ArraysToolbarEditorProperty = AuroraFPSEditor.AEditorGUILayout.ArraysToolbarEditorProperty;

namespace AuroraFPSEditor
{
    [CustomEditor(typeof(DecalMapping))]
    public class DecalMappingEditor : AuroraEditor<DecalMapping>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent Mapping = new GUIContent("Mapping");
            public readonly static GUIContent DecalToolbar = new GUIContent("Decals", "Decal effects that will randomly create on this key.");
            public readonly static GUIContent SoundToolbar = new GUIContent("Sounds", "Sound effects that will randomly played on this key.");
            public readonly static GUIContent Decal = new GUIContent("Decal", "Decal effect.");
            public readonly static GUIContent Sound = new GUIContent("Sound", "Sound effect.");
        }

        private DictionaryEditor<Object, PoolObjectStorage> serializedDictionary;

        public override void InitializeProperties()
        {
            SerializedProperty mapping = serializedObject.FindProperty("mapping");

            serializedDictionary = new DictionaryEditor<Object, PoolObjectStorage>(instance.GetMapping(), mapping);

            serializedDictionary.onAddWindowShowCallback = () =>
            {
                GenericMenu genericMenu = new GenericMenu();
                genericMenu.AddItem(new GUIContent("Physics Material"), false, () =>
                {
                    serializedDictionary.onAddWindowGUICallback = (ref Object key, ref PoolObjectStorage property) =>
                    {
                        key = AEditorGUILayout.ObjectField<PhysicMaterial>(GUIContent.none, key as PhysicMaterial, true);
                    };
                    serializedDictionary.showWindow = true;
                });

                genericMenu.AddItem(new GUIContent("Texture2D"), false, () =>
                {
                    serializedDictionary.onAddWindowGUICallback = (ref Object key, ref PoolObjectStorage property) =>
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

                EditorGUILayout.PropertyField(value, new GUIContent("Decals"));
            };

            serializedDictionary.propertyLabelCallback = (key, value, index) =>
            {
                return new GUIContent(string.Format("{0} ({1})", key.name, key.GetType().Name));
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