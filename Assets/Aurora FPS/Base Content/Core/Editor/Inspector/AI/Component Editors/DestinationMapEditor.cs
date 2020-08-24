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
    [CustomEditor(typeof(DestinationMap))]
    public class DestinationMapEditor : AuroraEditor<DestinationMap>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent BaseProperties = new GUIContent("Base Properties");
            public readonly static GUIContent Destinations = new GUIContent("Destinations");
            public readonly static GUIContent Clear = new GUIContent("Clear");
        }

        public const float SphereSize = 1;

        public readonly static Color HandleColor = new Color(0.5f, 0.5f, 0.5f, 0.3f);
        public readonly static Color WireColor = new Color(0.75f, 0.75f, 0.75f, 1.0f);

        private ReorderableList destinationsList;

        public override void InitializeProperties()
        {
            SerializedProperty destinations = serializedObject.FindProperty("destinations");
            destinationsList = new ReorderableList(serializedObject, destinations, true, true, true, true);

            destinationsList.drawHeaderCallback = (rect) =>
            {
                Rect position = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);

                Rect labelPosition = new Rect(position.x, position.y, 50, position.height);
                EditorGUI.LabelField(rect, ContentProperties.Destinations);

                Rect clearButtonPosition = new Rect(position.width - 25, position.y, 60, position.height);
                EditorGUI.BeginDisabledGroup(destinations.arraySize <= 1);
                if (GUI.Button(clearButtonPosition, ContentProperties.Clear) && DisplayDialogs.Confirmation("Are you really want to remove all destinations?", "Yes", "No"))
                {
                    destinations.ClearArray();
                }
                EditorGUI.EndDisabledGroup();
            };

            destinationsList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                SerializedProperty destination = destinations.GetArrayElementAtIndex(index);
                Rect position = new Rect(rect.x, rect.y + 1.5f, rect.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(position, destination, GUIContent.none);
            };

            destinationsList.elementHeightCallback = (index) =>
            {
                return EditorGUIUtility.singleLineHeight + 4;
            };

        }

        public override void DuringSceneGUI(SceneView sceneView)
        {
            Handles.HandleColor = HandleColor;
            Handles.WireColor = WireColor;
            if (instance.GetDestinations() != null && instance.GetCount() > 0)
            {
                Handles.PointsFreeMoveHandle(GetHashCode(), instance.GetDestinations());
            }
        }

        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.BaseProperties);
            destinationsList.DoLayoutList();
            EndGroup();
        }
    }
}