/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using AuroraFPSRuntime.AI;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace AuroraFPSEditor
{
    [AIBehaviourEditorTarget(typeof(AIWalkingBehaviour))]
    public class AIWalkingBehaviourEditor : AIBehaviourEditor
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent WalkingType = new GUIContent("Walking Type");
            public readonly static GUIContent DestinationMap = new GUIContent("Destination Map", "Walking destination map.");
            public readonly static GUIContent DestinationEvents = new GUIContent("Destination Events");
            public readonly static GUIContent DestinationEventIndex = new GUIContent("Index", "Destination index from destination map.");
            public readonly static GUIContent DestinationEventDelay = new GUIContent("Delay", "Delay on this destination.");
            public readonly static GUIContent DestinationEventInvoke = new GUIContent("Invoke", "Invoke type for this distination.");
            public readonly static GUIContent DestinationEventEvents = new GUIContent("Events", "Events for this destination.");
            public readonly static GUIContent OnStartEvent = new GUIContent("Start Events", "Start event functions called AIWalkingBehaviour become active.");
        }

        public readonly static Color HandleColor = new Color(0.5f, 0.5f, 0.5f, 0.3f);
        public readonly static Color WireColor = new Color(0.75f, 0.75f, 0.75f, 1.0f);

        private SerializedProperty serializedWalkingType;
        private SerializedProperty serializedDestinationMap;
        private SerializedProperty serializedDestinationEvents;
        private ReorderableList destinationEventsList;
        private string walkingTypeValue;
        private int lastEnumIndex;

        public AIWalkingBehaviourEditor(SerializedProperty serializedInstance, AICore targetAICore) : base(serializedInstance, targetAICore)
        {
            InitializeDestinationList();
        }

        /// <summary>
        /// Initialize destination list serialized property array 
        /// and creating reorderable list.
        /// </summary>
        public virtual void InitializeDestinationList()
        {
            serializedWalkingType = serializedInstance.FindPropertyRelative("walkingType");
            walkingTypeValue = serializedWalkingType.enumNames[serializedWalkingType.enumValueIndex];
            lastEnumIndex = serializedWalkingType.enumValueIndex;

            serializedDestinationMap = serializedInstance.FindPropertyRelative("destinationMap");

            serializedDestinationEvents = serializedInstance.FindPropertyRelative("destinationEvents");
            destinationEventsList = new ReorderableList(serializedObject, serializedDestinationEvents, true, true, true, true);

            destinationEventsList.drawHeaderCallback = (rect) =>
            {
                Rect position = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);

                Rect labelPosition = new Rect(position.x, position.y, 200, position.height);
                EditorGUI.LabelField(labelPosition, "Overrided Desitinations");

                EditorGUI.BeginDisabledGroup(serializedDestinationEvents.arraySize == 0);
                Rect buttonRect = new Rect(position.width - 20, position.y, 60, position.height);
                if (GUI.Button(buttonRect, "Clear") && DisplayDialogs.Confirmation("Are you really want to remove all destinations?", "Yes", "No"))
                {
                    serializedDestinationEvents.ClearArray();
                }
                EditorGUI.EndDisabledGroup();
            };

            destinationEventsList.drawElementCallback = (rect, index, isActive, isFocuse) =>
            {
                SerializedProperty property = serializedDestinationEvents.GetArrayElementAtIndex(index);

                float height = EditorGUIUtility.singleLineHeight + 1.5f;
                rect.y += 1.5f;

                Rect fieldRect = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);
                SerializedProperty destinationIndex = property.FindPropertyRelative("index");
                DestinationMap map = serializedDestinationMap.objectReferenceValue as DestinationMap;
                if (map != null)
                {
                    string[] indexes = new string[map.GetCount()];
                    for (int i = 0, length = map.GetCount(); i < length; i++)
                    {
                        indexes[i] = string.Format("Destination {0}", i + 1);
                    }
                    destinationIndex.intValue = EditorGUI.Popup(fieldRect, "Index", destinationIndex.intValue, indexes);
                }
                else
                {
                    EditorGUI.PropertyField(fieldRect, destinationIndex, ContentProperties.DestinationEventIndex);
                }

                fieldRect.y += height;
                SerializedProperty destinationDelay = property.FindPropertyRelative("delay");
                destinationDelay.floatValue = AEditorGUI.FixedFloatField(fieldRect, ContentProperties.DestinationEventDelay, destinationDelay.floatValue, 0);

                fieldRect.y += height;
                EditorGUI.PropertyField(fieldRect, property.FindPropertyRelative("invokeTime"), ContentProperties.DestinationEventInvoke);

                fieldRect.y += height;
                SerializedProperty destinationEvent = property.FindPropertyRelative("eventCallback");
                destinationEvent.isExpanded = EditorGUI.Foldout(fieldRect, destinationEvent.isExpanded, "Events", true);
                if (destinationEvent.isExpanded)
                {
                    fieldRect.y += height;
                    fieldRect.width = rect.width;
                    fieldRect.height = rect.height;
                    EditorGUI.PropertyField(fieldRect, destinationEvent, ContentProperties.DestinationEventEvents);
                }
            };

            destinationEventsList.elementHeightCallback = (index) =>
            {
                SerializedProperty property = serializedDestinationEvents.GetArrayElementAtIndex(index);
                SerializedProperty eventCallback = property.FindPropertyRelative("eventCallback");
                float singleLineHeight = EditorGUIUtility.singleLineHeight;
                float destinationEventHeight = eventCallback.isExpanded ? EditorGUI.GetPropertyHeight(eventCallback) + 20 : singleLineHeight;
                return (singleLineHeight * 3) + destinationEventHeight + 10;
            };

            destinationEventsList.elementHeight = 82;
        }

        /// <summary>
        /// Enables the Editor to handle an event in the Scene view.
        /// 
        /// Use this event to implement custom handles and user interface.
        /// </summary>
        /// <param name="sceneView">Scene view settings.</param>
        public override void DuringSceneGUI(SceneView sceneView)
        {
            if (serializedDestinationMap.objectReferenceValue != null)
            {
                Handles.HandleColor = HandleColor;
                Handles.WireColor = WireColor;

                DestinationMap map = serializedDestinationMap.objectReferenceValue as DestinationMap;
                Handles.PointsFreeMoveHandle(GetHashCode(), map.GetDestinations());

                if (map.GetDestinations() != null)
                {
                    if (lastEnumIndex != serializedWalkingType.enumValueIndex)
                    {
                        walkingTypeValue = serializedWalkingType.enumNames[serializedWalkingType.enumValueIndex];
                        lastEnumIndex = serializedWalkingType.enumValueIndex;
                    }

                    switch (walkingTypeValue)
                    {
                        case "Random":
                            Handles.DrawRandomTypeLines(map.GetDestinations());
                            break;
                        case "Sequential":
                            Handles.DrawSequentalTypeLines(map.GetDestinations());
                            break;
                        case "Finite":
                            Handles.DrawFiniteTypeLines(map.GetDestinations());
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Draw base serialized object properties.
        /// </summary>
        public override void OnPropertiesGUI()
        {
            base.OnPropertiesGUI();
            bool isExpanded = serializedDestinationEvents.isExpanded;
            AuroraEditor.BeginGroupLevel3(ref isExpanded, ContentProperties.DestinationEvents);
            if (isExpanded)
            {
                AuroraEditor.DecreaseIndentLevel();
                destinationEventsList.DoLayoutList();
                AuroraEditor.IncreaseIndentLevel();
            }
            AuroraEditor.EndGroupLevel();
            serializedDestinationEvents.isExpanded = isExpanded;
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
            excludingProperties.Add("destinationEvents");
        }
    }
}