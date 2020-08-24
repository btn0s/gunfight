/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AuroraFPSRuntime;
using AuroraFPSRuntime.AI;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace AuroraFPSEditor
{
    public class AIBehaviourEditorSerializedProperties
    {
        internal static class ContentProperties
        {
            public readonly static GUIContent Name = new GUIContent("Name", "Behaviour name.");
            public readonly static GUIContent TransitionControlHeader = new GUIContent("Transition to", "Transition to [Behaviour name].");
            public readonly static GUIContent TransitionControlRemoveButton = new GUIContent("Remove", "Remove this transition.");
        }

        // Base AIBehaviourEditor properties.
        protected AIBehaviour instance;
        protected SerializedProperty serializedInstance;
        protected SerializedObject serializedObject;
        protected AICore targetAICore;

        // Base AIBehaviour properties.
        private SerializedProperty behaviourName;
        private SerializedProperty transitions;
        private SerializedProperty startEvent;

        // Transition / Conditions list properties.
        private ReorderableList[] conditionsList;
        private string[] behaviourNames;
        private int nextBehaviourPopupIndex;
        private int selectedIndex;
        private bool conditionsIsCollapsed;
        private List<int> removeIndexList;

        // Stored required properties.
        private bool changed;

        /// <summary>
        /// AIBehaviourEditor constructor.
        /// </summary>
        /// <param name="serializedInstance">Serialized AIBehaviour instance.</param>
        public AIBehaviourEditorSerializedProperties(AIBehaviour instance, SerializedProperty serializedInstance, AICore targetAICore)
        {
            this.instance = instance;
            this.serializedInstance = serializedInstance;
            this.serializedObject = serializedInstance.serializedObject;
            this.targetAICore = targetAICore;

            InitializeDefaultProperties();
        }

        protected virtual void InitializeDefaultProperties()
        {
            behaviourName = serializedInstance.FindPropertyRelative("name");
            transitions = serializedInstance.FindPropertyRelative("transitions");
            startEvent = serializedInstance.FindPropertyRelative("startEvent");

            conditionsList = CreateConditionsList();
            removeIndexList = new List<int>();
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
        /// Main AI behaviour GUI.
        /// </summary>
        public void OnMainGUI()
        {
            serializedObject.Update();
            OnPropertiesGUI();
            RemoveTransitionHandler();
            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Base serializedObject properties.
        /// 
        /// Implement this method to draw a custom properties.
        /// </summary>
        public virtual void OnPropertiesGUI()
        {
            DrawNameField();
            DrawDefaultProperties();
            DrawTransition();
            DrawStartEvent();
        }

        public virtual void DrawNameField()
        {
            string name = behaviourName.stringValue;
            name = EditorGUILayout.DelayedTextField(ContentProperties.Name, name);
            if (name != "" && name != behaviourName.stringValue)
            {
                if (!targetAICore.ContainsBehaviour(name))
                {
                    behaviourName.stringValue = name;
                    changed = true;
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
        /// Drawing TransitionCondition reorderable lists.
        /// </summary>
        public virtual void DrawTransition()
        {
            bool isExpanded = transitions.isExpanded;
            AuroraEditor.BeginGroupLevel3(ref isExpanded, "Transitions");
            if (isExpanded)
            {
                for (int i = 0; i < conditionsList.Length; i++)
                {
                    AuroraEditor.DecreaseIndentLevel();
                    conditionsList[i].DoLayoutList();
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
                    instance.AddTransition(new AuroraFPSRuntime.AI.Transition("", new List<Condition>()));
                    conditionsList = CreateConditionsList();
                }
                EditorGUI.EndDisabledGroup();

                EditorGUI.BeginDisabledGroup(instance.GetTransitionCount() == 0);
                if (GUILayout.Button("Remove Transitions", "ButtonRight"))
                {
                    List<string> transitionsToText = new List<string>(transitions.arraySize);
                    for (int i = 0; i < transitions.arraySize; i++)
                    {
                        string nextBehaviour = transitions.GetArrayElementAtIndex(i).FindPropertyRelative("nextBehaviour").stringValue;
                        transitionsToText.Add(string.Format("{0} -> {1}", instance.GetName(), nextBehaviour));
                    }

                    transitionsToText.Sort();

                    for (int i = 0; i < transitionsToText.Count; i++)
                    {
                        string transition = transitionsToText[i];
                        transitionsToText[i] = string.Format("{0}: {1}", (i + 1), transition);
                    }

                    if (DisplayDialogs.Confirmation(string.Format("Are you really want to delete all of the following transitions?\n-\n{0}\n-", string.Join("\n", transitionsToText)), "Yes", "No"))
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
            transitions.isExpanded = isExpanded;
        }

        /// <summary>
        /// Drawing start event serialized property
        /// </summary>
        public virtual void DrawStartEvent()
        {
            bool isExpanded = startEvent.isExpanded;
            AuroraEditor.BeginGroupLevel3(ref isExpanded, "Events");
            if (isExpanded)
            {
                EditorGUILayout.PropertyField(startEvent);
            }
            AuroraEditor.EndGroupLevel();
            startEvent.isExpanded = isExpanded;
        }

        public void DrawDefaultProperties()
        {
            foreach (SerializedProperty property in EditorHelper.GetChildren(serializedInstance))
            {
                if (property.name == "name" || property.name == "transitions" || property.name == "startEvent")
                {
                    continue;
                }
                EditorGUILayout.PropertyField(property, true);
            }
        }

        public static void DefaultConditionGUI(Rect position, SerializedProperty condition, GUIContent content)
        {
            SerializedProperty mute = condition.FindPropertyRelative("mute");
            if (mute.boolValue)
            {
                content.text = string.Format("{0} (Mute)", content.text);
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

        public ReorderableList[] CreateConditionsList()
        {
            Type[] conditionsTypes = AuroraExtension.FindSubclassesOf<Condition>().ToArray();
            conditionsList = new ReorderableList[instance.GetTransitionCount()];

            UpdateModifiedProperties();

            for (int i = 0; i < instance.GetTransitionCount(); i++)
            {
                int indexCopy = i;

                AuroraFPSRuntime.AI.Transition transition = instance.GetTransition(i);

                ReorderableList conditionList = new ReorderableList(transition.GetConditions(), typeof(Condition), true, true, true, true);

                conditionList.drawHeaderCallback = (rect) =>
                {
                    Rect position = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);

                    Rect labelPosition = new Rect(position.x, position.y, 75, position.height);
                    EditorGUI.LabelField(labelPosition, ContentProperties.TransitionControlHeader);

                    Rect nextBehaviourPosition = new Rect(position.x + 75, position.y, position.width - 140, position.height);
                    if (behaviourNames != null && behaviourNames.Length > 0)
                    {
                        string nextBehaviour = instance.GetTransition(indexCopy).GetNextBehaviour();
                        for (int j = 0; j < behaviourNames.Length; j++)
                        {
                            if (behaviourNames[j] == nextBehaviour)
                            {
                                nextBehaviourPopupIndex = j;
                            }
                        }
                        nextBehaviourPopupIndex = EditorGUI.Popup(nextBehaviourPosition, nextBehaviourPopupIndex, behaviourNames);
                        nextBehaviour = behaviourNames[nextBehaviourPopupIndex];

                        nextBehaviourPosition.y -= 15;
                        instance.GetTransition(indexCopy).SetNextBehaviour(nextBehaviour);
                    }
                    else
                    {
                        EditorGUI.BeginDisabledGroup(true);
                        EditorGUI.LabelField(nextBehaviourPosition, "Need more than one behaviour!", EditorStyles.popup);
                        EditorGUI.EndDisabledGroup();

                    }

                    Rect removeButtonPosition = new Rect(position.width - 20, position.y, 60, position.height);
                    if (GUI.Button(removeButtonPosition, ContentProperties.TransitionControlRemoveButton))
                    {
                        RemoveTransition(indexCopy);
                    }
                };

                conditionList.drawElementCallback = (rect, index, isActive, isFocused) =>
                {
                    Condition condition = transition.GetCondition(index);
                    SerializedProperty transitionSerialized = serializedInstance.FindPropertyRelative("transitions").GetArrayElementAtIndex(indexCopy);
                    SerializedProperty conditionSerialized = transitionSerialized.FindPropertyRelative("conditions").GetArrayElementAtIndex(index);

                    Rect position = new Rect(rect.x + 10, rect.y, rect.width - 10, EditorGUIUtility.singleLineHeight);
                    GUIContent content = new GUIContent("Undefinded condition");
                    ConditionOptionsAttribute conditionOptions = AuroraExtension.GetAttribute<ConditionOptionsAttribute>(condition.GetType());
                    if (conditionOptions != null)
                    {
                        content.text = Path.GetFileName(conditionOptions.path);
                        content.tooltip = conditionOptions.description;
                    }

                    DefaultConditionGUI(position, conditionSerialized, content);

                    if (conditionSerialized.isExpanded)
                    {
                        conditionsIsCollapsed = false;
                    }

                    if (isActive)
                    {
                        selectedIndex = index;
                    }

                };

                conditionList.elementHeightCallback = (index) =>
                {
                    float height = 22;

                    SerializedProperty transitionSerialized = serializedInstance.FindPropertyRelative("transitions").GetArrayElementAtIndex(indexCopy);
                    SerializedProperty conditionSerialized = transitionSerialized.FindPropertyRelative("conditions").GetArrayElementAtIndex(index);

                    if (conditionSerialized.isExpanded)
                    {
                        foreach (SerializedProperty property in EditorHelper.GetChildren(conditionSerialized))
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
                                transition.AddCondition(condition);
                            });
                        }
                    }
                    genericMenu.ShowAsContext();
                };

                conditionList.onRemoveCallback = (list) =>
                {
                    instance.GetTransition(indexCopy).RemoveCondition(selectedIndex);
                    serializedObject.ApplyModifiedProperties();
                };

                conditionList.onMouseDragCallback = (list) =>
                {
                    if (!conditionsIsCollapsed)
                    {
                        SerializedProperty transitionSerialized = serializedInstance.FindPropertyRelative("transitions").GetArrayElementAtIndex(indexCopy);
                        SerializedProperty conditionSerialized = transitionSerialized.FindPropertyRelative("conditions");
                        CollapseConditions(conditionSerialized);
                    }
                };

                conditionsList[i] = conditionList;
            }
            return conditionsList;
        }

        public void UpdateModifiedProperties()
        {
            int behaviourCount = targetAICore.GetBehaviourCount();

            List<string> names = new List<string>();
            for (int i = 0; i < behaviourCount; i++)
            {
                names.Add(targetAICore.GetBehaviour(i).GetName());
            }
            names.Remove(instance.GetName());
            behaviourNames = names.ToArray();
            changed = true;
        }

        public void RemoveTransition(int index)
        {
            removeIndexList.Add(index);
        }

        public void RemoveTransition(params int[] index)
        {
            removeIndexList.AddRange(index);
        }

        public void RemoveAllTransitions()
        {
            instance.RemoveAllTransitions();
            removeIndexList.Clear();
            conditionsList = CreateConditionsList();
        }

        private void RemoveTransitionHandler()
        {
            if (removeIndexList.Count > 0)
            {
                for (int i = 0; i < removeIndexList.Count; i++)
                {
                    instance.RemoveTransition(i);
                }
                removeIndexList.Clear();
                conditionsList = CreateConditionsList();
            }
        }

        private void CollapseConditions(SerializedProperty conditions)
        {
            for (int i = 0; i < conditions.arraySize; i++)
            {
                conditions.GetArrayElementAtIndex(i).isExpanded = false;
            }
            conditionsIsCollapsed = true;
        }

        public bool Changed()
        {
            return changed;
        }
    }
}