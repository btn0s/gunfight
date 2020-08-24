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
    [CustomEditor(typeof(DefaultInputMapping))]
    public class DefaultInputMappingEditor : AuroraEditor<DefaultInputMapping>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent Axes = new GUIContent("Axes");
            public readonly static GUIContent Buttons = new GUIContent("Buttons");
        }

        private DictionaryEditor<string, string> serializedAxes;
        private DictionaryEditor<string, KeyCode> serializedButtons;

        public override void InitializeProperties()
        {
            SerializedProperty axesMapping = serializedObject.FindProperty("axes");
            SerializedProperty buttonsMapping = serializedObject.FindProperty("buttons");

            InitializeSerializedAxis(axesMapping);
            InitializeSerializedButtons(buttonsMapping);
        }

        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.Axes);
            serializedAxes.DrawLayoutDictionary();
            EndGroup();

            BeginGroup(ContentProperties.Buttons);
            serializedButtons.DrawLayoutDictionary();
            EndGroup();
        }

        protected virtual void InitializeSerializedAxis(SerializedProperty axes)
        {
            serializedAxes = new DictionaryEditor<string, string>(instance.GetAxes(), axes);

            serializedAxes.headerLabel = "Axes";

            serializedAxes.onAddElementCallback = (key, value) =>
            {
                if (!instance.ContainAxis(key))
                {
                    instance.AddAxis(key, "None");
                }
            };

            serializedAxes.onAddWindowGUICallback = (ref string key, ref string values) =>
            {
                key = EditorGUILayout.TextField(GUIContent.none, key);
            };

            serializedAxes.onElementGUICallback = (key, value, index) =>
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(key, new GUIContent("Key"));
                EditorGUI.EndDisabledGroup();

                EditorGUILayout.PropertyField(value, new GUIContent("Name"));
            };

            serializedAxes.drawRemoveButtonCallback = (key) =>
            {
                for (int i = 0, length = INC.Axes.Length; i < length; i++)
                {
                    if (key == INC.Axes[i])
                    {
                        return false;
                    }
                }
                return true;
            };

            serializedAxes.propertyLabelCallback = (key, value, index) =>
            {
                return new GUIContent(key);
            };

            serializedAxes.onClearElementsCallback = () =>
            {
                List<string> onRemoveKeys = new List<string>();
                foreach (var item in instance.GetAxes())
                {
                    string key = item.Key;
                    bool remove = true;
                    for (int i = 0, length = INC.Axes.Length; i < length; i++)
                    {
                        if (item.Key == INC.Axes[i])
                        {
                            remove = false;
                        }
                    }
                    if (remove)
                    {
                        onRemoveKeys.Add(key);
                    }
                }

                for (int i = 0; i < onRemoveKeys.Count; i++)
                {
                    instance.RemoveAxis(onRemoveKeys[i]);
                }
            };
        }

        protected virtual void InitializeSerializedButtons(SerializedProperty buttons)
        {
            serializedButtons = new DictionaryEditor<string, KeyCode>(instance.GetButtons(), buttons);

            serializedButtons.headerLabel = "Inputs";

            serializedButtons.onAddElementCallback = (key, value) =>
            {
                if (!instance.ContainButton(key))
                {
                    instance.AddButtons(key, KeyCode.None);
                }
            };

            serializedButtons.onAddWindowGUICallback = (ref string key, ref KeyCode value) =>
            {
                key = EditorGUILayout.TextField(GUIContent.none, key);
            };

            serializedButtons.onElementGUICallback = (key, value, index) =>
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(key, new GUIContent("Key"));
                EditorGUI.EndDisabledGroup();

                EditorGUILayout.PropertyField(value, new GUIContent("KeyCode"));
            };

            serializedButtons.drawRemoveButtonCallback = (key) =>
            {
                for (int i = 0, length = INC.Buttons.Length; i < length; i++)
                {
                    if (key == INC.Buttons[i])
                    {
                        return false;
                    }
                }
                return true;
            };

            serializedButtons.propertyLabelCallback = (key, value, index) =>
            {
                return new GUIContent(key);
            };

            serializedButtons.onClearElementsCallback = () =>
            {
                List<string> onRemoveKeys = new List<string>();
                foreach (var item in instance.GetButtons())
                {
                    string key = item.Key;
                    bool remove = true;
                    for (int i = 0, length = INC.Buttons.Length; i < length; i++)
                    {
                        if (item.Key == INC.Buttons[i])
                        {
                            remove = false;
                        }
                    }
                    if (remove)
                    {
                        onRemoveKeys.Add(key);
                    }
                }

                for (int i = 0; i < onRemoveKeys.Count; i++)
                {
                    instance.RemoveButton(onRemoveKeys[i]);
                }
            };
        }
    }
}