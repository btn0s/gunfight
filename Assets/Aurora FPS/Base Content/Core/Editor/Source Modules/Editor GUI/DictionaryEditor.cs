/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using System;
using UnityEditor;
using UnityEngine;
using AuroraFPSRuntime.Serialization.Collections;

namespace AuroraFPSEditor
{
    /// <summary>
    /// Aurora serializableDictionary drawer.
    /// Suitable for SerializableDictionary.
    /// </summary>
    public sealed class DictionaryEditor<TKey, TValue>
    {
        // Base AuroraDictionary properties
        private SerializableDictionary<TKey, TValue> serializableDictionary;
        private SerializedProperty serializedDictionary;
        private bool isModified;
        private AddKeyWindow addKeyWindow;

        // Available AuroraDictionary properties.
        public string headerLabel;
        public string removeButtonLabel;
        public string addButtonLabel;
        public string clearButtonLabel;
        public bool drawAddButton;
        public bool drawClearButton;
        public float addWindowWidth;
        public float addWindowHeight;
        public bool showWindow;

        // Available delegates.
        public OnElementGUICallback onElementGUICallback;
        public ElementExpanded elementExpanded;
        public OnAddWindowShowCallback onAddWindowShowCallback;
        public OnAddWindowGUICallback onAddWindowGUICallback;
        public OnRemoveElementCallback onRemoveElementCallback;
        public OnAddElementCallback onAddElementCallback;
        public OnClearElementsCallback onClearElementsCallback;
        public PropertyLabelCallback propertyLabelCallback;
        public ApplyButtonIsActive applyButtonIsActive;
        public DrawRemoveButtonCallback drawRemoveButtonCallback;

        /// <summary>
        /// AuroraDictionary constructor.
        /// </summary>
        /// <param name="serializableDictionary">Target serializableDictionary.</param>
        public DictionaryEditor(SerializableDictionary<TKey, TValue> serializableDictionary, SerializedProperty serializedDictionary)
        {
            this.serializableDictionary = serializableDictionary;
            this.serializedDictionary = serializedDictionary;
            this.headerLabel = serializedDictionary.serializedObject.targetObject.name;
            this.removeButtonLabel = "Remove";
            this.addButtonLabel = "Add Property";
            this.clearButtonLabel = "Clear Properties";
            this.drawAddButton = true;
            this.drawClearButton = true;
            this.addWindowWidth = 260.0f;
            this.addWindowHeight = 67.0f;

            onElementGUICallback = (key, value, index) =>
            {
                HelpBoxMessages.Message("Overrade OnElementGUICallback...", MessageType.Info, true);
            };

            onAddElementCallback = (key, value) =>
            {
                if (!serializableDictionary.ContainsKey(key))
                {
                    serializableDictionary.Add(key, value);
                }
            };

            onAddWindowShowCallback = () => { showWindow = true; };

            onAddWindowGUICallback = (ref TKey key, ref TValue values) =>
            {
                GUILayout.Space(7);
                GUILayout.Label("Override OnAddWindowGUICallback...", AEditorStyles.CenteredMiniGrayLabel);
            };

            onRemoveElementCallback = (key, index) =>
            {
                if (DisplayDialogs.Confirmation(string.Format("Are you really want to remove {0}", key), "Yes", "No"))
                {
                    serializableDictionary.Remove(key);
                }
            };

            drawRemoveButtonCallback = _ => true;

            onClearElementsCallback = () =>
            {
                serializableDictionary.Clear();
            };

            propertyLabelCallback = (key, value, index) =>
            {
                return new GUIContent(string.Format("Property {0}", index + 1));
            };

            applyButtonIsActive = (key, value) =>
            {
                return true;
            };
        }

        /// <summary>
        /// AuroraDictionary constructor.
        /// </summary>
        /// <param name="serializableDictionary">Target serializableDictionary.</param>
        /// <param name="headerLabel">Dictionary group header name.</param>
        public DictionaryEditor(SerializableDictionary<TKey, TValue> serializableDictionary, SerializedProperty serializedDictionary, string headerLabel)
        {
            this.serializableDictionary = serializableDictionary;
            this.serializedDictionary = serializedDictionary;
            this.headerLabel = headerLabel;
            this.removeButtonLabel = "Remove";
            this.addButtonLabel = "Add Property";
            this.clearButtonLabel = "Clear Properties";
            this.drawAddButton = true;
            this.drawClearButton = true;
            this.addWindowWidth = 260.0f;
            this.addWindowHeight = 70.0f;

            onElementGUICallback = (key, value, index) =>
            {
                HelpBoxMessages.Message("Overrade OnElementGUICallback...");
            };

            onAddElementCallback = (key, value) =>
            {
                if (!serializableDictionary.ContainsKey(key))
                {
                    serializableDictionary.Add(key, value);
                }
            };

            onAddWindowShowCallback = () => { showWindow = true; };

            onAddWindowGUICallback = (ref TKey key, ref TValue values) =>
            {
                HelpBoxMessages.Message("Overrade OnElementGUICallback...");
            };

            onRemoveElementCallback = (key, index) =>
            {
                serializableDictionary.Remove(key);
            };

            drawRemoveButtonCallback = _ => true;

            onClearElementsCallback = () =>
            {
                serializableDictionary.Clear();
            };

            propertyLabelCallback = (key, value, index) =>
            {
                return new GUIContent(string.Format("Property {0}", index + 1));
            };

            applyButtonIsActive = (key, value) =>
            {
                return true;
            };
        }

        /// <summary>
        /// AuroraDictionary constructor.
        /// </summary>
        /// <param name="serializableDictionary">Target serializableDictionary.</param>
        /// <param name="drawAddButton">Draw add new property button.</param>
        /// <param name="drawClearButton">Draw clear all properties button.</param>
        public DictionaryEditor(SerializableDictionary<TKey, TValue> serializableDictionary, SerializedProperty serializedDictionary, bool drawAddButton, bool drawClearButton)
        {
            this.serializableDictionary = serializableDictionary;
            this.serializedDictionary = serializedDictionary;
            this.headerLabel = "Dictionary";
            this.removeButtonLabel = "Remove";
            this.addButtonLabel = "Add Property";
            this.clearButtonLabel = "Clear Properties";
            this.drawAddButton = drawAddButton;
            this.drawClearButton = drawClearButton;
            this.addWindowWidth = 260.0f;
            this.addWindowHeight = 70.0f;

            onElementGUICallback = (key, value, index) =>
            {
                HelpBoxMessages.Message("Overrade OnElementGUICallback...");
            };

            onAddElementCallback = (key, value) =>
            {
                if (!serializableDictionary.ContainsKey(key))
                {
                    serializableDictionary.Add(key, value);
                }
            };

            onAddWindowShowCallback = () => { showWindow = true; };

            onAddWindowGUICallback = (ref TKey key, ref TValue values) =>
            {
                HelpBoxMessages.Message("Overrade OnElementGUICallback...");
            };

            onRemoveElementCallback = (key, index) =>
            {
                serializableDictionary.Remove(key);
            };

            drawRemoveButtonCallback = _ => true;

            onClearElementsCallback = () =>
            {
                serializableDictionary.Clear();
            };

            propertyLabelCallback = (key, value, index) =>
            {
                return new GUIContent(string.Format("Property {0}", index + 1));
            };

            applyButtonIsActive = (key, value) =>
            {
                return true;
            };
        }

        /// <summary>
        /// AuroraDictionary constructor.
        /// </summary>
        /// <param name="serializableDictionary">Target serializableDictionary.</param>
        /// <param name="headerLabel">Dictionary group header name.</param>
        /// <param name="drawAddButton">Draw add new property button.</param>
        /// <param name="drawClearButton">Draw clear all properties button.</param>
        public DictionaryEditor(SerializableDictionary<TKey, TValue> serializableDictionary, SerializedProperty serializedDictionary, string headerLabel, bool drawAddButton, bool drawClearButton)
        {
            this.serializableDictionary = serializableDictionary;
            this.serializedDictionary = serializedDictionary;
            this.headerLabel = headerLabel;
            this.removeButtonLabel = "Remove";
            this.addButtonLabel = "Add Property";
            this.clearButtonLabel = "Clear Properties";
            this.drawAddButton = drawAddButton;
            this.drawClearButton = drawClearButton;
            this.addWindowWidth = 260.0f;
            this.addWindowHeight = 70.0f;

            onElementGUICallback = (key, value, index) =>
            {
                HelpBoxMessages.Message("Overrade OnElementGUICallback...");
            };

            onAddElementCallback = (key, value) =>
            {
                if (!serializableDictionary.ContainsKey(key))
                {
                    serializableDictionary.Add(key, value);
                }
            };

            onAddWindowShowCallback = () => { showWindow = true; };

            onAddWindowGUICallback = (ref TKey key, ref TValue values) =>
            {
                HelpBoxMessages.Message("Overrade OnElementGUICallback...");
            };

            onRemoveElementCallback = (key, index) =>
            {
                serializableDictionary.Remove(key);
            };

            drawRemoveButtonCallback = _ => true;

            onClearElementsCallback = () =>
            {
                serializableDictionary.Clear();
            };

            propertyLabelCallback = (key, value, index) =>
            {
                return new GUIContent(string.Format("Property {0}", index + 1));
            };

            applyButtonIsActive = (key, value) =>
            {
                return true;
            };
        }

        /// <summary>
        /// Draw serializableDictionary in layout group.
        /// </summary>
        public void DrawLayoutDictionary()
        {
            if (serializableDictionary.Count == 0)
            {
                HelpBoxMessages.Message(string.Format("{0} is empty...\nAdd new element.", headerLabel), MessageType.Info, false);
            }

            AuroraEditor.IncreaseIndentLevel();
            int index = 0;
            foreach (var item in serializableDictionary)
            {
                SerializedProperty serializedKeys = serializedDictionary.FindPropertyRelative("keys");
                SerializedProperty serializedKey = serializedKeys.GetArrayElementAtIndex(index);

                SerializedProperty serializedValues = serializedDictionary.FindPropertyRelative("values");
                SerializedProperty serializedValue = serializedValues.GetArrayElementAtIndex(index);

                // if(key.objectReferenceValue == null)
                // {
                //     Debug.Log(string.Format("{0} key index [{1}] was deleted, because the key object are missing.", headerLabel, index));
                //     serializableDictionary.Remove(item.Key);
                //     break;
                // }

                bool isExpanded = serializedKey.isExpanded;
                AuroraEditor.BeginGroupLevel2(ref isExpanded, propertyLabelCallback(item.Key, item.Value, index));
                elementExpanded?.Invoke(item.Key, item.Value, index, isExpanded);
                if (isExpanded)
                {
                    onElementGUICallback(serializedKey.Copy(), serializedValue.Copy(), index);
                    if (isModified)
                    {
                        isModified = false;
                        break;
                    }

                    if (drawRemoveButtonCallback(item.Key))
                    {
                        GUILayout.Space(3);
                        GUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button(removeButtonLabel, GUILayout.Width(70), GUILayout.Height(16)))
                        {
                            onRemoveElementCallback(item.Key, index);
                            break;
                        }
                        GUILayout.EndHorizontal();
                    }

                }
                AuroraEditor.EndGroupLevel();
                serializedKey.isExpanded = isExpanded;
                index++;
            }
            AuroraEditor.DecreaseIndentLevel();

            if (drawAddButton || drawClearButton)
            {
                GUILayout.Space(5);

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (drawAddButton)
                {
                    Rect rect = AddKeyWindow.FixRect(addWindowWidth);
                    if (GUILayout.Button(addButtonLabel, drawClearButton ? "ButtonLeft" : "Button", AEditorGUILayout.DefaultHandleButtonOptions))
                    {
                        onAddWindowShowCallback();
                        TKey key = default(TKey);
                        TValue value = default(TValue);
                        Action onFieldGUI = () => onAddWindowGUICallback(ref key, ref value);
                        Action onApply = () => onAddElementCallback(key, value);
                        Func<bool> applyIsActive = () => applyButtonIsActive.Invoke(key, value);
                        addKeyWindow = AddKeyWindow.CreateWindow(rect, addWindowWidth, addWindowHeight, onFieldGUI, onApply, applyIsActive);
                    }
                    if (showWindow)
                    {
                        addKeyWindow.Show();
                        showWindow = false;
                    }
                }

                if (drawClearButton)
                {
                    EditorGUI.BeginDisabledGroup(serializableDictionary.Count == 0);
                    if (GUILayout.Button(clearButtonLabel, drawAddButton ? "ButtonRight" : "Button", AEditorGUILayout.DefaultHandleButtonOptions))
                    {
                        onClearElementsCallback();
                    }
                    EditorGUI.EndDisabledGroup();
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
        }

        public void IsModified()
        {
            isModified = true;
        }

        #region [Delegates]
        public delegate void OnElementGUICallback(SerializedProperty key, SerializedProperty value, int index);
        public delegate void ElementExpanded(TKey key, TValue value, int index, bool isExpanded);
        public delegate void OnAddWindowShowCallback();
        public delegate void OnAddWindowGUICallback(ref TKey key, ref TValue values);
        public delegate void OnRemoveElementCallback(TKey key, int index);
        public delegate void OnAddElementCallback(TKey key, TValue value);
        public delegate void OnClearElementsCallback();
        public delegate bool ApplyButtonIsActive(TKey key, TValue value);
        public delegate bool DrawRemoveButtonCallback(TKey key);
        public delegate GUIContent PropertyLabelCallback(TKey key, TValue value, int index);
        #endregion
    }
}