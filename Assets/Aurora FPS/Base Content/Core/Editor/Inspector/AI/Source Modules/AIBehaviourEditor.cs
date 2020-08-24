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
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace AuroraFPSEditor
{
    public class AIBehaviourEditor
    {
        internal static class ContentProperties
        {
            public readonly static GUIContent Name = new GUIContent("Name", "Behaviour name.");
            public readonly static GUIContent TransitionControlHeader = new GUIContent("Transition to", "Transition to [Behaviour name].");
            public readonly static GUIContent TransitionControlRemoveButton = new GUIContent("Remove", "Remove this transition.");
        }

        // Base AIBehaviourEditor properties.
        protected SerializedProperty serializedInstance;
        protected SerializedObject serializedObject;
        protected AICore targetAICore;

        // Base AIBehaviour properties.
        private SerializedProperty serializedBehaviourName;
        private SerializedProperty serializedTransitions;
        private SerializedProperty serializedStartEvent;
        private SerializedProperty serializedStartEventCalls;

        // Transition / Conditions list properties.
        private ReorderableList[] conditionsList;
        private string[] behaviourNames;
        private int nextBehaviourPopupIndex;
        private int selectedIndex;
        private bool conditionsIsCollapsed;
        private List<int> removeIndexList;

        // Stored required properties.
        private string[] excludingProperties;
        private int arraySize;
        private int lastArraySize;
        private bool isExpanded;

        /// <summary>
        /// AIBehaviourEditor constructor.
        /// </summary>
        /// <param name="serializedInstance">Serialized AIBehaviour instance.</param>
        public AIBehaviourEditor(SerializedProperty serializedInstance, AICore targetAICore)
        {
            this.serializedInstance = serializedInstance;
            this.serializedObject = serializedInstance.serializedObject;
            this.targetAICore = targetAICore;

            serializedBehaviourName = serializedInstance.FindPropertyRelative("name");
            serializedTransitions = serializedInstance.FindPropertyRelative("transitions");
            serializedStartEvent = serializedInstance.FindPropertyRelative("startEvent");
            serializedStartEventCalls = serializedStartEvent.FindPropertyRelative("m_PersistentCalls").FindPropertyRelative("m_Calls");
            arraySize = serializedStartEventCalls.arraySize;

            conditionsList = CreateConditionsList();
            removeIndexList = new List<int>();

            AddStartEventCallback += SetInstanceAsTarget;

            InitializeExcludingProperties();
        }

        /// <summary>
        /// Initialize excluding properties added by AddExcludingProperties method.
        /// </summary>
        private void InitializeExcludingProperties()
        {
            List<string> excludingPropertiesList = new List<string>();
            AddExcludingProperties(ref excludingPropertiesList);
            excludingProperties = excludingPropertiesList.ToArray();
        }

        /// <summary>
        /// The event issued when the OnGUI method is called.
        /// 
        /// Use this callback to queue meshes for rendering in the Scene view. To implement handles, tools, and UI, use duringSceneGui.
        /// </summary>
        /// <param name="sceneView">Scene view settings.</param>
        public virtual void BeforeSceneGUI(SceneView sceneView)
        {

        }

        /// <summary>
        /// Enables the Editor to handle an event in the Scene view.
        /// 
        /// Use this event to implement custom handles and user interface.
        /// </summary>
        /// <param name="sceneView">Scene view settings.</param>
        public virtual void DuringSceneGUI(SceneView sceneView)
        {

        }

        /// <summary>
        /// Base AI behaviour GUI.
        /// 
        /// Implement this method to make a custom behaviour GUI.
        /// </summary>
        public void OnBaseGUI()
        {
            serializedObject.Update();
            DrawNameField();
            OnPropertiesGUI();
            DrawTransitions();
            DrawStartEvent();
            RemoveTransitionHandler();
            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Behaviour properties GUI.
        /// 
        /// Implement this method to draw a custom properties.
        /// </summary>
        public virtual void OnPropertiesGUI()
        {
            DrawDefaultProperties(GetExcludingProperties());
        }

        /// <summary>
        /// Called when the AICore object with this AIBehaviour becomes disabled or inactive.
        /// </summary>
        public virtual void OnDisable()
        {

        }

        /// <summary>
        /// Called when the AIBehaviour will be removed.
        /// </summary>
        public virtual void OnRemove()
        {

        }

        /// <summary>
        /// Draw a delayed text field to edit behaviour name.
        /// </summary>
        public virtual void DrawNameField()
        {
            string name = serializedBehaviourName.stringValue;
            name = EditorGUILayout.DelayedTextField(ContentProperties.Name, name);
            if (name != "" && name != serializedBehaviourName.stringValue)
            {
                if (!targetAICore.ContainsBehaviour(name))
                {
                    serializedBehaviourName.stringValue = name;
                }
                else
                {
                    Debug.Log("Aurora FPS: Behaviour with this name already exists, come up with a unique name.");
                }
            }
            else if (name == "")
            {
                Debug.Log("Aurora FPS: Behaviour name cannot be empty!");
            }
        }

        /// <summary>
        /// Draw reorderable lists to edit behaviour transition and condition.
        /// </summary>
        public virtual void DrawTransitions()
        {
            bool isExpanded = serializedTransitions.isExpanded;
            AuroraEditor.BeginGroupLevel3(ref isExpanded, "Transitions");
            if (isExpanded)
            {
                for (int i = 0; i < conditionsList.Length; i++)
                {
                    AuroraEditor.DecreaseIndentLevel();
                    SerializedProperty serializedMuteProperty = serializedTransitions.GetArrayElementAtIndex(i).FindPropertyRelative("mute");
                    EditorGUI.BeginDisabledGroup(serializedMuteProperty.boolValue);
                    conditionsList[i].DoLayoutList();
                    EditorGUI.EndDisabledGroup();
                    AuroraEditor.IncreaseIndentLevel();
                }

                if (targetAICore.GetBehaviourCount() == 1)
                {
                    HelpBoxMessages.Message("Need more than one behaviour to create transition.", MessageType.Info, true);
                }
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                EditorGUI.BeginDisabledGroup(targetAICore.GetBehaviourCount() == 1);
                if (GUILayout.Button("Add Transition", "ButtonLeft"))
                {
                    serializedTransitions.arraySize++;
                    conditionsList = CreateConditionsList();
                }
                EditorGUI.EndDisabledGroup();

                EditorGUI.BeginDisabledGroup(serializedTransitions.arraySize == 0);
                if (GUILayout.Button("Remove Transitions", "ButtonRight"))
                {
                    List<string> transitionsToText = new List<string>(serializedTransitions.arraySize);
                    for (int i = 0; i < serializedTransitions.arraySize; i++)
                    {
                        string nextBehaviour = serializedTransitions.GetArrayElementAtIndex(i).FindPropertyRelative("nextBehaviour").stringValue;
                        transitionsToText.Add(string.Format("{0} -> {1}", serializedBehaviourName.stringValue, nextBehaviour));
                    }

                    transitionsToText.Sort();

                    for (int i = 0; i < transitionsToText.Count; i++)
                    {
                        string transition = transitionsToText[i];
                        transitionsToText[i] = string.Format("{0}: {1}", (i + 1), transition);
                    }

                    if (DisplayDialogs.Confirmation(string.Format("Are you really want to delete all of the following transitions?\n{0}", string.Join("\n", transitionsToText)), "Yes", "No"))
                    {
                        RemoveAllTransitions();
                        conditionsList = CreateConditionsList();
                    }

                }
                EditorGUI.EndDisabledGroup();
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

            }
            AuroraEditor.EndGroupLevel();
            serializedTransitions.isExpanded = isExpanded;
        }

        /// <summary>
        /// Draw serialized start event property.
        /// </summary>
        public virtual void DrawStartEvent()
        {
            bool isExpanded = serializedStartEvent.isExpanded;
            AuroraEditor.BeginGroupLevel3(ref isExpanded, "Events");
            if (isExpanded)
            {
                EditorGUILayout.PropertyField(serializedStartEvent);
                AddStartEventCallbackHandler();
            }
            AuroraEditor.EndGroupLevel();
            serializedStartEvent.isExpanded = isExpanded;
        }

        /// <summary>
        /// Draw default instance properties.
        /// </summary>
        public void DrawDefaultProperties()
        {
            foreach (SerializedProperty property in EditorHelper.GetChildren(serializedInstance))
            {
                EditorGUILayout.PropertyField(property, true);
            }
        }

        /// <summary>
        /// Draw default instance properties.
        /// </summary>
        /// <param name="propertiesToExclude">Properties to exclude from draw.</param>
        public void DrawDefaultProperties(params string[] propertiesToExclude)
        {
            foreach (SerializedProperty property in EditorHelper.GetChildren(serializedInstance))
            {
                if (propertiesToExclude != null && propertiesToExclude.Length > 0)
                {
                    bool exclude = false;
                    for (int i = 0; i < propertiesToExclude.Length; i++)
                    {
                        if (propertiesToExclude[i] == property.name)
                        {
                            exclude = true;
                            break;
                        }
                    }
                    if (exclude)
                    {
                        continue;
                    }
                }
                EditorGUILayout.PropertyField(property, true);
            }
        }

        /// <summary>
        /// Array of exclusive properties that will be hidden in the inspector GUI.
        /// 
        /// Implement this method to return custom excluding properties default OnBaseGUI.
        /// </summary>
        public string[] GetExcludingProperties()
        {
            return excludingProperties;
        }

        /// <summary>
        /// Add new custom exclusive properties.
        /// 
        /// Implement this method to add new custom exclusive properties that will be hidden in the inspector GUI.
        /// </summary>
        /// <param name="excludingProperties">Properties that will be hidden in the inspector GUI</param>
        protected virtual void AddExcludingProperties(ref List<string> excludingProperties)
        {
            excludingProperties.Add("name");
            excludingProperties.Add("transitions");
            excludingProperties.Add("startEvent");
        }

        /// <summary>
        /// Default representation of condition GUI.
        /// </summary>
        /// <param name="position">Rectangle on the screen to use for the condition GUI.</param>
        /// <param name="condition">The SerializedProperty of condition instance.</param>
        /// <param name="content">The label of this condition.</param>
        public static void OnConditionGUI(Rect position, SerializedProperty condition, GUIContent content)
        {
            SerializedProperty mute = condition.FindPropertyRelative("mute");
            if (mute.boolValue)
            {
                content.text = string.Format("{0} (Muted)", content.text);
            }

            condition.isExpanded = EditorGUI.Foldout(position, condition.isExpanded, content, true);
            if (condition.isExpanded)
            {
                position.y += 20;

                EditorGUI.BeginDisabledGroup(mute.boolValue);
                foreach (SerializedProperty property in EditorHelper.GetChildren(condition))
                {
                    if (property.name == "mute")
                        continue;

                    EditorGUI.PropertyField(position, property, true);
                    position.y += 20;
                }
                EditorGUI.EndDisabledGroup();

                EditorGUI.PropertyField(position, mute);
            }
        }

        /// <summary>
        /// Add all required components to target AICore.
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
                    if (requireComponent.m_Type0 != null && targetAICore.GetComponent(requireComponent.m_Type0) == null)
                        targetAICore.gameObject.AddComponent(requireComponent.m_Type0);
                    if (requireComponent.m_Type1 != null && targetAICore.GetComponent(requireComponent.m_Type1) == null)
                        targetAICore.gameObject.AddComponent(requireComponent.m_Type1);
                    if (requireComponent.m_Type2 != null && targetAICore.GetComponent(requireComponent.m_Type1) == null)
                        targetAICore.gameObject.AddComponent(requireComponent.m_Type2);
                }
            }
        }

        /// <summary>
        /// Request to update behaviour name.
        /// </summary>
        public void UpdateBehaviourName()
        {
            int behaviourCount = targetAICore.GetBehaviourCount();

            List<string> names = new List<string>();
            for (int i = 0; i < behaviourCount; i++)
            {
                names.Add(targetAICore.GetBehaviour(i).GetName());
            }
            names.Remove(serializedBehaviourName.stringValue);
            behaviourNames = names.ToArray();
        }

        /// <summary>
        /// Remove behaviour transition by index.
        /// </summary>
        public void RemoveTransition(int index)
        {
            removeIndexList.Add(index);
        }

        /// <summary>
        /// Remove multiple behaviour transition by indexes.
        /// </summary>
        public void RemoveTransition(params int[] index)
        {
            removeIndexList.AddRange(index);
        }

        /// <summary>
        /// Remove all transition that contatins in behaviour.
        /// </summary>
        public void RemoveAllTransitions()
        {
            serializedTransitions.ClearArray();
            removeIndexList.Clear();
            conditionsList = CreateConditionsList();
        }

        /// <summary>
        /// Handler of RemoveTransition method.
        /// </summary>
        private void RemoveTransitionHandler()
        {
            if (removeIndexList.Count > 0)
            {
                for (int i = 0; i < removeIndexList.Count; i++)
                {
                    serializedTransitions.DeleteArrayElementAtIndex(i);
                }
                removeIndexList.Clear();
                conditionsList = CreateConditionsList();
            }
        }

        /// <summary>
        /// Collapse all conditions in reorderable list.
        /// 
        /// Automatically called when moving conditions in reorderable list.
        /// </summary>
        public void CollapseConditions(SerializedProperty conditions)
        {
            for (int i = 0; i < conditions.arraySize; i++)
            {
                conditions.GetArrayElementAtIndex(i).isExpanded = false;
            }
            conditionsIsCollapsed = true;
        }

        /// <summary>
        /// Handler of start event property, call AddStartEventCallback when added new event callback.
        /// </summary>
        protected void AddStartEventCallbackHandler()
        {
            if (arraySize != serializedStartEventCalls.arraySize)
            {
                int newArraySize = serializedStartEventCalls.arraySize;
                if (newArraySize > arraySize)
                {
                    SerializedProperty property = serializedStartEventCalls.GetArrayElementAtIndex(newArraySize - 1);
                    AddStartEventCallback?.Invoke(property);
                }
                arraySize = newArraySize;
            }
        }

        /// <summary>
        /// Automatically set instance to new event call.
        /// </summary>
        /// <param name="property">Start event property element.</param>
        private void SetInstanceAsTarget(SerializedProperty property)
        {
            if (property != null)
            {
                property.FindPropertyRelative("m_Target").objectReferenceValue = targetAICore.gameObject;
            }
        }

        /// <summary>
        /// Create reorderable condition list for each transition.
        /// </summary>
        /// <returns>Array of reorderable condition list.</returns>
        public ReorderableList[] CreateConditionsList()
        {
            Type[] conditionsTypes = AuroraExtension.FindSubclassesOf<Condition>().ToArray();

            conditionsList = new ReorderableList[serializedTransitions.arraySize];

            UpdateBehaviourName();

            for (int i = 0; i < serializedTransitions.arraySize; i++)
            {
                int indexCopy = i;

                SerializedProperty serializedTransition = serializedTransitions.GetArrayElementAtIndex(i);
                SerializedProperty serializedConditions = serializedTransition.FindPropertyRelative("conditions");

                ReorderableList conditionList = new ReorderableList(serializedObject, serializedConditions, true, true, true, true);

                conditionList.drawHeaderCallback = (rect) =>
                {
                    Rect position = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);

                    Rect labelPosition = new Rect(position.x, position.y, 75, position.height);
                    EditorGUI.LabelField(labelPosition, ContentProperties.TransitionControlHeader);

                    Rect nextBehaviourPosition = new Rect(position.x + 75, position.y, position.width - 192, position.height);
                    SerializedProperty serializedNextBehaviour = serializedTransition?.FindPropertyRelative("nextBehaviour");
                    if (behaviourNames != null && behaviourNames.Length > 0)
                    {
                        if (serializedNextBehaviour != null)
                        {
                            for (int j = 0; j < behaviourNames.Length; j++)
                            {
                                if (behaviourNames[j] == serializedNextBehaviour.stringValue)
                                {
                                    nextBehaviourPopupIndex = j;
                                }
                            }
                            nextBehaviourPopupIndex = EditorGUI.Popup(nextBehaviourPosition, nextBehaviourPopupIndex, behaviourNames);
                            if (serializedNextBehaviour.stringValue != behaviourNames[nextBehaviourPopupIndex])
                            {
                                serializedNextBehaviour.stringValue = behaviourNames[nextBehaviourPopupIndex];
                            }
                        }
                        else
                        {
                            EditorGUI.BeginDisabledGroup(true);
                            EditorGUI.LabelField(nextBehaviourPosition, "Serialized property not found.", EditorStyles.popup);
                            EditorGUI.EndDisabledGroup();
                        }

                    }
                    else
                    {
                        EditorGUI.BeginDisabledGroup(true);
                        EditorGUI.LabelField(nextBehaviourPosition, "Need more than one behaviour!", EditorStyles.popup);
                        EditorGUI.EndDisabledGroup();
                    }

                    GUI.enabled = true;
                    SerializedProperty serializedMuteProperty = serializedTransition.FindPropertyRelative("mute");
                    Rect muteTogglePosition = new Rect(position.width - 70, position.y, 60, position.height);
                    if (GUI.Button(muteTogglePosition, serializedMuteProperty.boolValue ? "Muted" : "Mute"))
                    {
                        serializedMuteProperty.boolValue = !serializedMuteProperty.boolValue;
                    }
                    GUI.enabled = !serializedMuteProperty.boolValue;

                    Rect removeButtonPosition = new Rect(position.width - 10, position.y, 60, position.height);
                    if (GUI.Button(removeButtonPosition, ContentProperties.TransitionControlRemoveButton) && DisplayDialogs.Confirmation(string.Format("Are you really want to remove [{0}->{1}] transition?", serializedBehaviourName.stringValue, serializedNextBehaviour.stringValue)))
                    {
                        RemoveTransition(indexCopy);
                    }
                };

                conditionList.drawElementCallback = (rect, index, isActive, isFocused) =>
                {
                    Rect position = new Rect(rect.x + 10, rect.y, rect.width - 10, EditorGUIUtility.singleLineHeight);
                    SerializedProperty serializedCondition = serializedConditions?.GetArrayElementAtIndex(index);
                    if (serializedCondition != null)
                    {
                        GUIContent content = new GUIContent("Undefinded Behaviour");

                        Type type = EditorHelper.GetTargetObjectOfProperty(serializedCondition).GetType();
                        ConditionOptionsAttribute conditionOptions = AuroraExtension.GetAttribute<ConditionOptionsAttribute>(type);
                        if (conditionOptions != null)
                        {
                            content.text = Path.GetFileName(conditionOptions.path);
                            content.tooltip = conditionOptions.description;
                        }

                        OnConditionGUI(position, serializedCondition, content);

                        if (serializedCondition.isExpanded)
                        {
                            conditionsIsCollapsed = false;
                        }

                        if (isActive)
                        {
                            selectedIndex = index;
                        }
                    }
                    else
                    {
                        EditorGUI.LabelField(rect, "Serialized property not found.");
                    }

                };

                conditionList.elementHeightCallback = (index) =>
                {
                    float height = 22;

                    SerializedProperty serializedCondition = serializedConditions?.GetArrayElementAtIndex(index);

                    if (serializedCondition != null && serializedCondition.isExpanded)
                    {
                        foreach (SerializedProperty property in EditorHelper.GetChildren(serializedCondition))
                        {
                            height += 22;
                        }
                    }

                    return height;
                };

                conditionList.onAddDropdownCallback = (rect, list) =>
                {
                    GenericMenu genericMenu = new GenericMenu();
                    for (int j = 0; j < conditionsTypes.Length; j++)
                    {
                        Type type = conditionsTypes[j];
                        ConditionOptionsAttribute conditionOptions = AuroraExtension.GetAttribute<ConditionOptionsAttribute>(conditionsTypes[j]);
                        if (conditionOptions != null)
                        {
                            Condition condition = Activator.CreateInstance(type)as Condition;
                            genericMenu.AddItem(new GUIContent(conditionOptions.path), false, () =>
                            {
                                int index = serializedConditions.arraySize;
                                serializedConditions.arraySize++;
                                serializedConditions.GetArrayElementAtIndex(index).managedReferenceValue = condition;
                                serializedObject.ApplyModifiedProperties();
                                AddRequiredComponents(type);
                            });
                        }
                    }
                    genericMenu.ShowAsContext();
                };

                conditionList.onMouseDragCallback = (list) =>
                {
                    if (!conditionsIsCollapsed)
                    {
                        if (serializedConditions != null)
                        {
                            CollapseConditions(serializedConditions);
                        }
                    }
                };

                conditionList.drawNoneElementCallback = (rect) =>
                {
                    EditorGUI.LabelField(rect, "Add new condition...");
                };

                conditionsList[i] = conditionList;
            }
            return conditionsList;
        }

        public string GetBehaviourName()
        {
            return serializedBehaviourName.stringValue;
        }

        public bool IsExpanded()
        {
            return isExpanded;
        }

        public void IsExpanded(bool value)
        {
            isExpanded = value;
        }

        #region [Event Callback Function]
        /// <summary>
        /// Called when added new callback event to start event property.
        /// </summary>
        public event Action<SerializedProperty> AddStartEventCallback;
        #endregion
    }
}