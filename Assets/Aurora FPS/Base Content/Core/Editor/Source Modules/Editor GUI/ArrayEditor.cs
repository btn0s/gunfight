/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using UnityEditor;
using UnityEngine;

namespace AuroraFPSEditor
{
    /// <summary>
    /// Aurora array drawer.
    /// Suitable for delault C# Array and Generic List<T>.
    /// </summary>
    public sealed class ArrayEditor
    {
        // Base AuroraArray properties.
        private SerializedProperty arrayProperty;

        // Available AuroraArray properties.
        public string headerLabel;
        public string removeButtonLabel;
        public string addButtonLabel;
        public string clearButtonLabel;
        public bool drawAddButton;
        public bool drawClearButton;

        // Available delegates.
        public OnElementGUI onElementGUICallback;
        public RemoveElementButtonGUI removeElementButtonGUICallback;
        public AddElementButtonGUI addElementButtonGUICallback;
        public ClearElementsButtonGUI clearElementsButtonGUICallback;
        public PropertyLabel propertyLabelCallback;

        /// <summary>
        /// AuroraArray constructor.
        /// </summary>
        /// <param name="arrayProperty">Target property array.</param>
        public ArrayEditor(SerializedProperty arrayProperty)
        {
            this.arrayProperty = arrayProperty;
            this.headerLabel = AuroraEditor.GenerateHeaderName(arrayProperty.name);
            this.removeButtonLabel = "Remove";
            this.addButtonLabel = "Add Property";
            this.clearButtonLabel = "Clear Property";
            this.drawAddButton = true;
            this.drawClearButton = true;

            onElementGUICallback = (property, index) =>
            {
                EditorGUILayout.PropertyField(property, true);
            };

            removeElementButtonGUICallback = (array, index) =>
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(removeButtonLabel, GUILayout.Width(70), GUILayout.Height(16)))
                {
                    array.DeleteArrayElementAtIndex(index);
                    return true;
                }
                GUILayout.EndHorizontal();
                return false;
            };

            addElementButtonGUICallback = (array) =>
            {
                if (GUILayout.Button(addButtonLabel, drawClearButton ? "ButtonLeft" : "Button", AEditorGUILayout.DefaultHandleButtonOptions))
                {
                    arrayProperty.arraySize++;
                }
            };

            clearElementsButtonGUICallback = (array) =>
            {
                if (GUILayout.Button(clearButtonLabel, drawAddButton ? "ButtonRight" : "Button", AEditorGUILayout.DefaultHandleButtonOptions))
                {
                    arrayProperty.ClearArray();
                }
            };

            propertyLabelCallback = (property, index) =>
            {
                return string.Format("Property {0}", index + 1);
            };
        }

        /// <summary>
        /// AuroraArray constructor.
        /// </summary>
        /// <param name="arrayProperty">Target property array.</param>
        /// <param name="headerLabel">Array header label.</param>
        public ArrayEditor(SerializedProperty arrayProperty, string headerLabel)
        {
            this.arrayProperty = arrayProperty;
            this.headerLabel = headerLabel;
            this.removeButtonLabel = "Remove";

            this.addButtonLabel = "Add Property";
            this.clearButtonLabel = "Clear Property";
            this.drawAddButton = true;
            this.drawClearButton = true;

            onElementGUICallback = (property, index) =>
            {
                if (property.hasChildren)
                {
                    foreach (var item in EditorHelper.GetChildren(property))
                    {
                        EditorGUILayout.PropertyField(item);
                    }
                }
                else
                {
                    EditorGUILayout.PropertyField(property);
                }
            };

            removeElementButtonGUICallback = (array, index) =>
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(removeButtonLabel, GUILayout.Width(70), GUILayout.Height(16)))
                {
                    array.DeleteArrayElementAtIndex(index);
                    return true;
                }
                GUILayout.EndHorizontal();
                return false;
            };

            addElementButtonGUICallback = (array) =>
            {
                if (GUILayout.Button(addButtonLabel, "ButtonLeft", AEditorGUILayout.DefaultHandleButtonOptions))
                {
                    arrayProperty.arraySize++;
                }
            };

            clearElementsButtonGUICallback = (array) =>
            {
                if (GUILayout.Button(clearButtonLabel, "ButtonRight", AEditorGUILayout.DefaultHandleButtonOptions))
                {
                    arrayProperty.ClearArray();
                }
            };

            propertyLabelCallback = (property, index) =>
            {
                return string.Format("Property {0}", index + 1);
            };
        }

        /// <summary>
        /// AuroraArray constructor.
        /// </summary>
        /// <param name="arrayProperty">Target property array.</param>
        /// <param name="drawAddButton">Draw add new property button.</param>
        /// <param name="drawClearButton">Draw clear all properties button</param>
        public ArrayEditor(SerializedProperty arrayProperty, bool drawAddButton, bool drawClearButton)
        {
            this.arrayProperty = arrayProperty;
            this.headerLabel = AuroraEditor.GenerateHeaderName(arrayProperty.name);
            this.removeButtonLabel = "Remove";
            this.addButtonLabel = "Add Property";
            this.clearButtonLabel = "Clear Property";
            this.drawAddButton = drawAddButton;
            this.drawClearButton = drawClearButton;

            onElementGUICallback = (property, index) =>
            {
                if (property.hasChildren)
                {
                    foreach (var item in EditorHelper.GetChildren(property))
                    {
                        EditorGUILayout.PropertyField(item);
                    }
                }
                else
                {
                    EditorGUILayout.PropertyField(property);
                }
            };

            removeElementButtonGUICallback = (array, index) =>
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(removeButtonLabel, GUILayout.Width(70), GUILayout.Height(16)))
                {
                    array.DeleteArrayElementAtIndex(index);
                    return true;
                }
                GUILayout.EndHorizontal();
                return false;
            };

            addElementButtonGUICallback = (array) =>
            {
                if (GUILayout.Button(addButtonLabel, "ButtonLeft", AEditorGUILayout.DefaultHandleButtonOptions))
                {
                    arrayProperty.arraySize++;
                }
            };

            clearElementsButtonGUICallback = (array) =>
            {
                if (GUILayout.Button(clearButtonLabel, "ButtonRight", AEditorGUILayout.DefaultHandleButtonOptions))
                {
                    arrayProperty.ClearArray();
                }
            };

            propertyLabelCallback = (property, index) =>
            {
                return string.Format("Property {0}", index + 1);
            };
        }

        /// <summary>
        /// AuroraArray constructor.
        /// </summary>
        /// <param name="arrayProperty">Target property array.</param>
        /// <param name="headerLabel">Array group header name.</param>
        /// <param name="drawAddButton">Draw add new property button.</param>
        /// <param name="drawClearButton">Draw clear all properties button.</param>
        public ArrayEditor(SerializedProperty arrayProperty, string headerLabel, bool drawAddButton, bool drawClearButton)
        {
            this.arrayProperty = arrayProperty;
            this.headerLabel = headerLabel;
            this.removeButtonLabel = "Remove";
            this.addButtonLabel = "Add Property";
            this.clearButtonLabel = "Clear Property";
            this.drawAddButton = drawAddButton;
            this.drawClearButton = drawClearButton;

            onElementGUICallback = (property, index) =>
            {
                if (property.hasChildren)
                {
                    foreach (var item in EditorHelper.GetChildren(property))
                    {
                        EditorGUILayout.PropertyField(item);
                    }
                }
                else
                {
                    EditorGUILayout.PropertyField(property);
                }
            };

            removeElementButtonGUICallback = (array, index) =>
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(removeButtonLabel, GUILayout.Width(70), GUILayout.Height(16)))
                {
                    array.DeleteArrayElementAtIndex(index);
                    return true;
                }
                GUILayout.EndHorizontal();
                return false;
            };

            addElementButtonGUICallback = (array) =>
            {
                if (GUILayout.Button(addButtonLabel, "ButtonLeft", AEditorGUILayout.DefaultHandleButtonOptions))
                {
                    arrayProperty.arraySize++;
                }
            };

            clearElementsButtonGUICallback = (array) =>
            {
                if (GUILayout.Button(clearButtonLabel, "ButtonRight", AEditorGUILayout.DefaultHandleButtonOptions))
                {
                    arrayProperty.ClearArray();
                }
            };

            propertyLabelCallback = (property, index) =>
            {
                return string.Format("Property {0}", index + 1);
            };
        }

        /// <summary>
        /// Draw serialized array in layout group.
        /// </summary>
        public void DrawLayoutGroup()
        {
            AuroraEditor.BeginGroup(headerLabel);
            if (arrayProperty.arraySize == 0)
            {
                HelpBoxMessages.Message(string.Format("{0} is empty...\nAdd new property.", headerLabel), MessageType.Info, false);
            }

            AuroraEditor.IncreaseIndentLevel();
            for (int i = 0, length = arrayProperty.arraySize; i < length; i++)
            {
                SerializedProperty property = arrayProperty.GetArrayElementAtIndex(i);
                bool isExpanded = property.isExpanded;
                AuroraEditor.BeginGroupLevel2(ref isExpanded, propertyLabelCallback(property, i));
                if (isExpanded)
                {
                    onElementGUICallback(property, i);
                    GUILayout.Space(3);
                    if (removeElementButtonGUICallback(arrayProperty, i))
                    {
                        break;
                    }

                }
                AuroraEditor.EndGroupLevel();
                property.isExpanded = isExpanded;
            }
            AuroraEditor.DecreaseIndentLevel();

            if (drawAddButton || drawClearButton)
            {
                GUILayout.Space(5);

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (drawAddButton)
                {
                    addElementButtonGUICallback(arrayProperty);
                }

                if (drawClearButton)
                {
                    EditorGUI.BeginDisabledGroup(arrayProperty.arraySize == 0);
                    clearElementsButtonGUICallback(arrayProperty);
                    EditorGUI.EndDisabledGroup();
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

            AuroraEditor.EndGroup();
        }

        #region [Delegates]
        public delegate void OnElementGUI(SerializedProperty array, int index);
        public delegate bool RemoveElementButtonGUI(SerializedProperty property, int index);
        public delegate void AddElementButtonGUI(SerializedProperty array);
        public delegate void ClearElementsButtonGUI(SerializedProperty array);
        public delegate string PropertyLabel(SerializedProperty property, int index);
        #endregion
    }
}