/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using AuroraFPSRuntime;
using AuroraFPSRuntime.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AuroraFPSEditor
{
    [CustomEditor(typeof(AICore), true)]
    public class AICoreEditor : AuroraEditor<AICore>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent BaseProperties = new GUIContent("Base Properties");
            public readonly static GUIContent DefaultBehaviour = new GUIContent("Default Behaviour");
        }

        private struct BehaviourMenuItem
        {
            public string path;
            public int priority;
            public GenericMenu.MenuFunction2 menuFunction;
            public Type type;

            public BehaviourMenuItem(string path, int priority, GenericMenu.MenuFunction2 menuFunction, Type type)
            {
                this.path = path;
                this.priority = priority;
                this.menuFunction = menuFunction;
                this.type = type;
            }
        }

        public const float BehaviourButtonsWidth = 150;

        // Base AICore editor properties.
        private SerializedProperty defaultBehaviour;
        private SerializedProperty behaviours;
        private AIBehaviourEditor[] behaviourEditors;

        // Default behaviour name popup properties.
        private string[] behaviourNames;
        private int selectedBehaviourNameIndex;

        // Stored required properties.
        private GenericMenu addBehaviourMenu;
        private List<int> removeIndexesCache;

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// 
        /// Implement this method to initialize properties.
        /// </summary>
        public override void InitializeProperties()
        {
            defaultBehaviour = serializedObject.FindProperty("defaultBehaviour");
            behaviours = serializedObject.FindProperty("behaviours");
            behaviourNames = GetBehaviourNames();
            addBehaviourMenu = CreateAddBehaviourGenericMenu();
            selectedBehaviourNameIndex = LoadDefaultBehaviourIndex();
            behaviourEditors = CreateBehaviourEditors();
            removeIndexesCache = new List<int>();

            CollapseBehaviours();
        }

        /// <summary>
        /// Base serializedObject properties.
        /// </summary>
        public sealed override void OnBaseGUI()
        {
            OnPropertiesGUI();
            RemoveBehavioursHandler();
            UpdateBehaviourNames();
        }

        /// <summary>
        /// Base AICore properties GUI.
        /// 
        /// Implement this method to draw a custom properties.
        /// </summary>
        public virtual void OnPropertiesGUI()
        {
            BeginGroup(ContentProperties.BaseProperties);
            DefaultBehaviourField();
            DrawBehavioursGUI();
            EndGroup();
        }

        /// <summary>
        /// Draw a popup field to edit default behaviour name.
        /// </summary>
        public virtual void DefaultBehaviourField()
        {
            if (behaviourNames != null && behaviourNames.Length > 0)
            {
                string previousBehaviourName = defaultBehaviour.stringValue;
                selectedBehaviourNameIndex = EditorGUILayout.Popup(ContentProperties.DefaultBehaviour, selectedBehaviourNameIndex, behaviourNames);
                defaultBehaviour.stringValue = behaviourNames[selectedBehaviourNameIndex];
                if (previousBehaviourName != defaultBehaviour.stringValue)
                {
                    serializedObject.ApplyModifiedProperties();
                }
            }
            else
            {
                DrawEmptyDefaultBehaviour("No available behaviours");
            }
        }

        /// <summary>
        /// Draw all AICore behaviours.
        /// </summary>
        public virtual void DrawBehavioursGUI()
        {
            if (instance.GetBehaviourCount() > 0)
            {
                if(behaviourEditors == null || behaviourEditors.Length == 0)
                {
                    behaviourEditors = CreateBehaviourEditors();
                }

                IncreaseIndentLevel();
                for (int i = 0; i < instance.GetBehaviourCount(); i++)
                {
                    AIBehaviourEditor behaviourEditor = behaviourEditors[i];
                    bool isExpanded = behaviourEditor.IsExpanded();
                    bool previousExpanded = isExpanded;
                    BeginGroupLevel2(ref isExpanded, behaviourEditor.GetBehaviourName());
                    if (isExpanded)
                    {
                        behaviourEditor.OnBaseGUI();
                        if (AEditorGUILayout.ButtonRight("Remove", GUILayout.Width(100)))
                        {
                            RemoveBehaviour(i);
                        }
                    }
                    EndGroupLevel();
                    behaviourEditor.IsExpanded(isExpanded);
                    if (previousExpanded != isExpanded)
                    {
                        if (!previousExpanded && isExpanded)
                        {
                            SceneView.beforeSceneGui += behaviourEditor.BeforeSceneGUI;
                            SceneView.duringSceneGui += behaviourEditor.DuringSceneGUI;
                        }

                        if (previousExpanded && !isExpanded)
                        {
                            SceneView.beforeSceneGui -= behaviourEditor.BeforeSceneGUI;
                            SceneView.duringSceneGui -= behaviourEditor.DuringSceneGUI;
                        }
                    }
                }
                DecreaseIndentLevel();
            }
            else
            {
                HelpBoxMessages.Message("Add new behaviours.");
            }

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Add Behaviour", "ButtonLeft", GUILayout.Width(BehaviourButtonsWidth)))
            {
                ShowAvailableBehaviours();
            }

            EditorGUI.BeginDisabledGroup(instance.GetBehaviourCount() == 0);
            if (GUILayout.Button("Remove All Behaviours", "ButtonRight", GUILayout.Width(BehaviourButtonsWidth)) && DisplayDialogs.Confirmation("Are you really want to remove all behaviours?"))
            {
                RemoveAllBehaviours();
            }
            EditorGUI.EndDisabledGroup();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        public override void OnDisable()
        {
            base.OnDisable();
            ClearSceneViewCallbacks();
        }

        /// <summary>
        /// Clear all editors scene view callbacks.
        /// </summary>
        public void ClearSceneViewCallbacks()
        {
            if (behaviourEditors != null && behaviourEditors.Length > 0)
            {
                for (int i = 0; i < behaviourEditors.Length; i++)
                {
                    AIBehaviourEditor behaviourEditor = behaviourEditors[i];
                    SceneView.beforeSceneGui -= behaviourEditor.BeforeSceneGUI;
                    SceneView.duringSceneGui -= behaviourEditor.DuringSceneGUI;
                }
            }
        }

        /// <summary>
        /// Clear editor scene view callbacks.
        /// </summary>
        public void ClearSceneViewCallback(int index)
        {
            if (behaviourEditors != null && behaviourEditors.Length > index)
            {
                AIBehaviourEditor behaviourEditor = behaviourEditors[index];
                SceneView.beforeSceneGui -= behaviourEditor.BeforeSceneGUI;
                SceneView.duringSceneGui -= behaviourEditor.DuringSceneGUI;
            }
        }

        /// <summary>
        /// Draw empty default behaviour message.
        /// </summary>
        public void DrawEmptyDefaultBehaviour(string message)
        {
            Rect position = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight);

            Rect contentPosition = new Rect(position.x, position.y, 150, position.height);
            EditorGUI.LabelField(contentPosition, ContentProperties.DefaultBehaviour);
            EditorGUI.BeginDisabledGroup(true);
            Rect textPosition = new Rect(position.x + EditorGUIUtility.labelWidth + 2, position.y, position.width - EditorGUIUtility.labelWidth - 2, position.height);
            EditorGUI.LabelField(textPosition, message, EditorStyles.popup);
            EditorGUI.EndDisabledGroup();
        }

        /// <summary>
        /// Load default behaviour popup index.
        /// </summary>
        public int LoadDefaultBehaviourIndex()
        {
            for (int i = 0; i < behaviourNames.Length; i++)
            {
                if (defaultBehaviour.stringValue == behaviourNames[i])
                {
                    return i;
                }
            }
            return 0;
        }

        /// <summary>
        /// Collapse all behaviour editor foldouts.
        /// </summary>
        public void CollapseBehaviours()
        {
            for (int i = 0; i < behaviours.arraySize; i++)
            {
                SerializedProperty behaviour = behaviours.GetArrayElementAtIndex(i);
                behaviour.isExpanded = false;
            }
        }

        /// <summary>
        /// Update behaviour names when GUI is changed.
        /// </summary>
        public void UpdateBehaviourNames()
        {
            if (GUI.changed)
            {
                behaviourNames = GetBehaviourNames();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public GenericMenu CreateAddBehaviourGenericMenu()
        {
            GenericMenu genericMenu = new GenericMenu();
            List<BehaviourMenuItem> behaviourMenuItems = new List<BehaviourMenuItem>();

            Type[] behaviourTypes = AuroraExtension.FindSubclassesOf<AIBehaviour>().ToArray();
            for (int i = 0; i < behaviourTypes.Length; i++)
            {
                Type behaviourType = behaviourTypes[i];
                AIBehaviourOptionsAttribute behaviourOptions = AuroraExtension.GetAttribute<AIBehaviourOptionsAttribute>(behaviourType);
                if (behaviourOptions != null && !behaviourOptions.hide)
                {
                    behaviourMenuItems.Add(new BehaviourMenuItem(behaviourOptions.path, behaviourOptions.priority, AddBehaviourFunction, behaviourType));
                }
            }

            behaviourMenuItems = behaviourMenuItems.OrderBy(t => t.priority).ToList();

            for (int i = 0; i < behaviourMenuItems.Count; i++)
            {
                BehaviourMenuItem behaviourMenuItem = behaviourMenuItems[i];
                genericMenu.AddItem(new GUIContent(behaviourMenuItem.path), false, behaviourMenuItem.menuFunction, behaviourMenuItem.type);
            }

            return genericMenu;
        }

        /// <summary>
        /// Show as context generic menu with available behaviours to add in AICore.
        /// </summary>
        /// <param name="behaviourTypes"></param>
        public void ShowAvailableBehaviours()
        {
            addBehaviourMenu.ShowAsContext();
        }

        /// <summary>
        /// Add new behaviour to AICore by type.
        /// </summary>
        /// <param name="data">Behaviour type.</param>
        private void AddBehaviourFunction(object data)
        {
            Type behaviourType = data as Type;
            AIBehaviour behaviour = Activator.CreateInstance(behaviourType)as AIBehaviour;
            AIBehaviourOptionsAttribute behaviourOptions = AuroraExtension.GetAttribute<AIBehaviourOptionsAttribute>(behaviourType);
            string behaviourName = behaviourOptions != null ? System.IO.Path.GetFileName(behaviourOptions.path) : "Undefined name";
            AddRequiredComponents(behaviourType);

            int count = 0;
            for (int i = 0; i < behaviours.arraySize; i++)
            {
                string name = behaviours.GetArrayElementAtIndex(i).FindPropertyRelative("name").stringValue;
                if (name == behaviourName || (name.Length >= 3 && name.Substring(0, name.Length - 2) == behaviourName))
                {
                    count++;
                }
            }

            if (count > 0)
            {
                behaviourName = string.Format("{0} {1}", behaviourName, ++count);
            }

            behaviour.SetName(behaviourName);
            int index = behaviours.arraySize;
            behaviours.arraySize++;
            behaviours.GetArrayElementAtIndex(index).managedReferenceValue = behaviour;
            serializedObject.ApplyModifiedProperties();

            behaviourNames = GetBehaviourNames();
            behaviourEditors = CreateBehaviourEditors();
        }

        /// <summary>
        /// Add all required components to instance.
        /// </summary>
        /// <param name="type">Adding behaviour type.</param>
        public void AddRequiredComponents(Type type)
        {
            RequireComponent[] requireComponents = AuroraExtension.GetAttributes<RequireComponent>(type);
            if (requireComponents != null && requireComponents.Length > 0)
            {
                for (int i = 0; i < requireComponents.Length; i++)
                {
                    RequireComponent requireComponent = requireComponents[i];
                    if (requireComponent.m_Type0 != null && instance.GetComponent(requireComponent.m_Type0) == null)
                        instance.gameObject.AddComponent(requireComponent.m_Type0);
                    if (requireComponent.m_Type1 != null && instance.GetComponent(requireComponent.m_Type1) == null)
                        instance.gameObject.AddComponent(requireComponent.m_Type1);
                    if (requireComponent.m_Type2 != null && instance.GetComponent(requireComponent.m_Type2) == null)
                        instance.gameObject.AddComponent(requireComponent.m_Type2);
                }
            }
        }

        /// <summary>
        /// Remove behaviour by index.
        /// </summary>
        public void RemoveBehaviour(int index)
        {
            removeIndexesCache.Add(index);
        }

        /// <summary>
        /// Remove multiple behaviour by indexes.
        /// </summary>
        public void RemoveBehaviour(params int[] indexes)
        {
            removeIndexesCache.AddRange(indexes);
        }

        /// <summary>
        /// Remove all behaviour that contatins in AICore.
        /// </summary>
        public void RemoveAllBehaviours()
        {
            ClearSceneViewCallbacks();
            behaviours.ClearArray();
            serializedObject.ApplyModifiedProperties();
            behaviourNames = GetBehaviourNames();
            selectedBehaviourNameIndex = LoadDefaultBehaviourIndex();
            behaviourEditors = null;
        }

        /// <summary>
        /// Handles of RemoveBehaviour method.
        /// </summary>
        private void RemoveBehavioursHandler()
        {
            if (removeIndexesCache.Count > 0)
            {
                for (int i = 0; i < removeIndexesCache.Count; i++)
                {
                    int index = removeIndexesCache[i];
                    ClearSceneViewCallback(index);
                    behaviours.DeleteArrayElementAtIndex(index);
                }
                serializedObject.ApplyModifiedProperties();
                removeIndexesCache.Clear();
                behaviourNames = GetBehaviourNames();
                selectedBehaviourNameIndex = LoadDefaultBehaviourIndex();
                behaviourEditors = CreateBehaviourEditors();
            }
        }

        /// <summary>
        /// Create behaviour editors for all AIBehaviour instances that contains in AICore.
        /// </summary>
        public AIBehaviourEditor[] CreateBehaviourEditors()
        {
            int behaviourCount = behaviours.arraySize;

            behaviourEditors = new AIBehaviourEditor[behaviourCount];

            IEnumerable<Type> types = AuroraExtension.FindSubclassesOf<AIBehaviourEditor>();
            foreach (Type type in types)
            {
                AIBehaviourEditorTargetAttribute behaviourEditorTarget = AuroraExtension.GetAttribute<AIBehaviourEditorTargetAttribute>(type);
                if (behaviourEditorTarget != null)
                {
                    for (int i = 0; i < behaviourCount; i++)
                    {
                        SerializedProperty behaviour = behaviours.GetArrayElementAtIndex(i).Copy();
                        Type behaviourType = EditorHelper.GetTargetObjectOfProperty(behaviour).GetType();
                        if (behaviourEditorTarget.GetTarget() == behaviourType)
                        {
                            behaviourEditors[i] = Activator.CreateInstance(type, behaviour, instance)as AIBehaviourEditor;
                        }
                    }
                }
            }

            for (int i = 0; i < behaviourCount; i++)
            {
                if (behaviourEditors[i] == null)
                {
                    SerializedProperty behaviour = behaviours.GetArrayElementAtIndex(i).Copy();
                    behaviourEditors[i] = new AIBehaviourEditor(behaviour, instance);
                }
            }

            return behaviourEditors;
        }

        /// <summary>
        /// Get behaviour names array of behaviour instances.
        /// </summary>
        /// <returns></returns>
        public string[] GetBehaviourNames()
        {
            string[] names = new string[behaviours.arraySize];
            if(name != null)
            {
                for (int i = 0; i < names.Length; i++)
                {
                    names[i] = behaviours.GetArrayElementAtIndex(i).FindPropertyRelative("name").stringValue;
                }
            }
            return names;
        }
    }
}