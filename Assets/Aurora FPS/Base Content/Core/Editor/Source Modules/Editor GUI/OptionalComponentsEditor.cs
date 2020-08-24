/* ==================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================== */

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace AuroraFPSEditor
{
    public sealed class OptionalComponentsEditor
    {
        // Base OptionalComponents properties.
        private string headerLabel;

        // List properties.
        private List<Type> allComponents;
        private List<Type> addedComponents;
        private ReorderableList reorderableList;

        public OptionalComponentsEditor(params Type[] components)
        {
            this.headerLabel = "Add Optional Components";
            this.allComponents = components.ToList();
            this.addedComponents = new List<Type>();

            if (components != null)
                InitializeReorderableList(headerLabel, this.addedComponents);

            OverrideOptionNameCallback = (name) => { return name; };
        }

        public OptionalComponentsEditor(string headerLabel, params Type[] components)
        {
            this.headerLabel = headerLabel;
            this.allComponents = components.ToList();
            this.addedComponents = new List<Type>();

            if (components != null)
                InitializeReorderableList(headerLabel, this.addedComponents);

            OverrideOptionNameCallback = (name) => { return name; };
        }

        private void InitializeReorderableList(string headerLabel, List<Type> components)
        {
            components.Clear();
            reorderableList = new ReorderableList(components, typeof(Type), true, true, true, true);

            reorderableList.drawHeaderCallback = (position) =>
            {
                EditorGUI.LabelField(position, headerLabel);
            };

            reorderableList.drawElementCallback = (position, index, isActive, isFocused) =>
            {
                Rect labelPosition = new Rect(position.x, position.y, position.width - 50, EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(labelPosition, OverrideOptionNameCallback.Invoke(components[index].Name));
            };

            reorderableList.onAddDropdownCallback = (position, list) =>
            {
                GenericMenu genericMenu = CreateGenericMenu();
                genericMenu.ShowAsContext();
            };
        }

        private GenericMenu CreateGenericMenu()
        {
            GenericMenu genericMenu = new GenericMenu();
            for (int i = 0; i < allComponents.Count; i++)
            {
                Type component = allComponents[i];
                string text = AuroraEditor.GenerateHeaderName(component.Name);
                if (!addedComponents.Contains(component))
                {
                    genericMenu.AddItem(new GUIContent(text), false, () => { addedComponents.Add(component); });
                }
                else
                {
                    genericMenu.AddDisabledItem(new GUIContent(text));
                }
            }
            return genericMenu;
        }

        /// <summary>
        /// Draw options layout list.
        /// </summary>
        public void DrawOptions()
        {
            reorderableList?.DoLayoutList();
        }

        /// <summary>
        /// Apply options to target gameobject.
        /// </summary>
        public void ApplyOptions(GameObject target)
        {
            if (target != null && addedComponents != null)
            {
                for (int i = 0; i < addedComponents.Count; i++)
                {
                    Type componentType = addedComponents[i];
                    Component component = target.GetComponent(componentType);
                    if (component == null)
                    {
                        target.AddComponent(componentType);
                    }
                }
            }

        }

        #region [Event Callback Functions]
        public Func<string, string> OverrideOptionNameCallback;
        #endregion
    }
}