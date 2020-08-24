/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using System;
using System.Linq;
using AuroraFPSRuntime;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace AuroraFPSEditor
{
    [CustomEditor(typeof(SpawnManager))]
    public class SpawnManagerEditor : AuroraEditor<SpawnManager>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent BaseProperties = new GUIContent("Items");
            public readonly static GUIContent ID = new GUIContent("ID", "Spawn item id.");
            public readonly static GUIContent Target = new GUIContent("Target", "Target to spawn.");
            public readonly static GUIContent Placement = new GUIContent("Placement", "Spawn placement type.");
            public readonly static GUIContent Radius = new GUIContent("Radius", "Sphere radius.");
            public readonly static GUIContent Point = new GUIContent("Point");
            public readonly static GUIContent InstanceCount = new GUIContent("Instance Count", "");
            public readonly static GUIContent ExecuteDelay = new GUIContent("Execute Delay", "");
            public readonly static GUIContent InstantiateDelay = new GUIContent("Instantiate Delay", "");
            public readonly static GUIContent RepeatDelay = new GUIContent("Repeat Delay", "");
            public readonly static GUIContent IsPoolObject = new GUIContent("Is PoolObject", "Mark item as pool object.");

            public readonly static GUIContent RespawnSystem = new GUIContent("Respawn System");
            public readonly static GUIContent RespawnLogic = new GUIContent("Logic");

            public readonly static GUIContent ExecuteOnStart = new GUIContent("Execute On Start", "Execute spawn on game start.");
            public readonly static GUIContent SceneObject = new GUIContent("Scene Object", "Execute spawn on game start.");
            public readonly static GUIContent OnSpawnUnityEventCallback = new GUIContent("On Spawn Event Callback", "On spawn event callback called when item spawned on scene.");
        }

        private SerializedProperty spawnItemsSerialized;
        private ReorderableList[] reorderableLists;
        private bool[] foldouts;
        private int executeIndex;
        private int actualIndex;

        // Respawn logic properties.
        private Type[] respawnlogicTypes;

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// 
        /// Implement this method to initialize properties.
        /// </summary>
        public override void InitializeProperties()
        {
            spawnItemsSerialized = serializedObject.FindProperty("spawnItems");

            int length = spawnItemsSerialized.arraySize;

            if (length == 0)
            {
                return;
            }

            reorderableLists = new ReorderableList[length];
            for (int i = 0; i < length; i++)
            {
                SerializedProperty pointsSerialized = spawnItemsSerialized.GetArrayElementAtIndex(i).FindPropertyRelative("points");
                reorderableLists[i] = new ReorderableList(serializedObject, pointsSerialized, true, true, true, true);

                reorderableLists[i].drawHeaderCallback = (rect) =>
                {
                    EditorGUI.LabelField(rect, "Points");
                };

                reorderableLists[i].drawElementCallback = (rect, index, isActive, isFocus) =>
                {
                    SerializedProperty point = pointsSerialized.GetArrayElementAtIndex(index);

                    rect.y += 1.5f;
                    rect.height = EditorGUIUtility.singleLineHeight;

                    EditorGUI.PropertyField(rect, point, GUIContent.none);
                };
            }

            foldouts = new bool[length * 4];

            actualIndex = -1;
            executeIndex = -1;

            respawnlogicTypes = AuroraExtension.FindSubclassesOf<RespawnLogicBase>().ToArray();
        }

        /// <summary>
        /// Enables the Editor to handle an event in the Scene view.
        /// 
        /// Use this event to implement custom handles and user interface.
        /// </summary>
        /// <param name="sceneView">Scene view settings.</param>
        public override void DuringSceneGUI(SceneView sceneView)
        {
            if (instance.GetSpawnItems() != null && instance.GetSpawnItemsLenght() > 0 && actualIndex != -1)
            {
                SpawnSettings spawnItem = instance.GetSpawnItem(actualIndex);
                Transform[] points = spawnItem.GetPoints();

                Handles.HandleColor = new Color32(220, 220, 220, 255);
                Handles.HandleHighlightedColor = new Color32(75, 145, 255, 255);
                Handles.HandleRemoveColor = new Color32(255, 70, 30, 255);
                Handles.WireColor = Color.white;
                Handles.WireHighlightedColor = Color.blue;
                Handles.WireRemoveColor = Color.red;

                switch (spawnItem.GetPlacement())
                {
                    case SpawnSettings.Placement.MultiplePoints:
                        Handles.PointsFreeMoveHandle(base.GetHashCode(), points, 2, Handles.DrawPositionPointArrow, (go) =>
                        {
                            go.name = string.Format("Spawn Point [{0}]", spawnItem.GetID());
                        }, null);
                        break;

                    case SpawnSettings.Placement.Point:
                        Handles.PointsFreeMoveHandle(base.GetHashCode(), points, 2, Handles.DrawPositionPointArrow, (go) =>
                        {
                            go.name = string.Format("Spawn Point [{0}]", spawnItem.GetID());
                        }, null);
                        break;

                    case SpawnSettings.Placement.Sphere:
                        Handles.HandleColor = new Color32(220, 220, 220, 100);
                        Handles.HandleHighlightedColor = new Color32(75, 145, 255, 150);
                        Handles.HandleRemoveColor = new Color32(255, 70, 30, 150);
                        Handles.PointsFreeMoveHandle(base.GetHashCode(), points, spawnItem.GetRadius(), Handles.DrawSphereHalfWithSolidBottom, (go) =>
                        {
                            go.name = string.Format("Spawn Point [{0}]", spawnItem.GetID());
                        }, null);
                        break;
                }

                spawnItem.SetPoints(points);
                instance.SetSpawnItem(actualIndex, spawnItem);
            }
        }

        /// <summary>
        /// Base serializedObject properties.
        /// 
        /// Implement this method to draw a custom properties.
        /// </summary>
        public override void OnBaseGUI()
        {
            bool isAnyFoldoutIsExpanded = false;
            BeginGroup(ContentProperties.BaseProperties);
            if (instance.GetSpawnItemsLenght() > 0)
            {
                IncreaseIndentLevel();
                for (int i = 0, f = 0; i < instance.GetSpawnItemsLenght(); i++, f += 4)
                {
                    SpawnSettings spawnItem = instance.GetSpawnItem(i);
                    bool foldout = foldouts[f];
                    BeginGroupLevel2(ref foldout, spawnItem.GetID());
                    if (foldout)
                    {
                        spawnItem.SetID(AEditorGUILayout.RequiredTextField(ContentProperties.ID, spawnItem.GetID()));
                        spawnItem.SetTarget(AEditorGUILayout.RequiredObjectField(ContentProperties.Target, spawnItem.GetTarget(), true));
                        spawnItem.SetPlacement(AEditorGUILayout.EnumPopup(ContentProperties.Placement, spawnItem.GetPlacement()));
                        spawnItem.SetInstanceCount(AEditorGUILayout.FixedIntField(ContentProperties.InstanceCount, spawnItem.GetInstanceCount(), 1));
                        if (!spawnItem.SceneObject())
                        {
                            spawnItem.ExecuteOnStart(EditorGUILayout.Toggle(ContentProperties.ExecuteOnStart, spawnItem.ExecuteOnStart()));
                        }
                        else
                        {
                            EditorGUI.BeginDisabledGroup(spawnItem.SceneObject());
                            spawnItem.ExecuteOnStart(EditorGUILayout.Toggle(ContentProperties.ExecuteOnStart, true));
                            EditorGUI.EndDisabledGroup();
                        }
                        spawnItem.SceneObject(EditorGUILayout.Toggle(ContentProperties.SceneObject, spawnItem.SceneObject()));
                        spawnItem.IsPoolObject(EditorGUILayout.Toggle(ContentProperties.IsPoolObject, spawnItem.IsPoolObject()));

                        GUILayout.Space(3);

                        if (spawnItem.GetPlacement() == SpawnSettings.Placement.Point || spawnItem.GetPlacement() == SpawnSettings.Placement.Sphere)
                        {
                            spawnItem.SetPoints(ClearPoints(spawnItem.GetPoints()));
                        }

                        bool pointsFoldout = foldouts[f + 1];
                        BeginGroupLevel3(ref pointsFoldout, "Points");
                        if (pointsFoldout)
                        {
                            switch (spawnItem.GetPlacement())
                            {
                                case SpawnSettings.Placement.MultiplePoints:
                                    {
                                        DecreaseIndentLevel();
                                        reorderableLists[i].DoLayoutList();
                                        IncreaseIndentLevel();
                                    }
                                    break;

                                case SpawnSettings.Placement.Point:
                                    {
                                        Transform point = spawnItem.GetPoint(0);
                                        point = AEditorGUILayout.ObjectField(ContentProperties.Point, point, true);
                                        spawnItem.SetPoint(0, point);
                                    }
                                    break;

                                case SpawnSettings.Placement.Sphere:
                                    {
                                        Transform point = spawnItem.GetPoint(0);
                                        point = AEditorGUILayout.ObjectField(ContentProperties.Point, point, true);
                                        spawnItem.SetPoint(0, point);
                                        spawnItem.SetRadius(AEditorGUILayout.FixedFloatField(ContentProperties.Radius, spawnItem.GetRadius(), 0));
                                    }
                                    break;
                            }
                        }

                        EndGroupLevel();
                        foldouts[f + 1] = pointsFoldout;

                        bool delayFoldout = foldouts[f + 2];
                        BeginGroupLevel3(ref delayFoldout, "Delay");
                        if (delayFoldout)
                        {
                            spawnItem.SetExecuteDelay(AEditorGUILayout.FixedFloatField(ContentProperties.ExecuteDelay, spawnItem.GeExecuteDelay(), 0));
                            spawnItem.SetInstantiateDelay(AEditorGUILayout.FixedFloatField(ContentProperties.InstantiateDelay, spawnItem.GetInstantiateDelay(), 0));
                            spawnItem.SetRepeatDelay(AEditorGUILayout.FixedFloatField(ContentProperties.RepeatDelay, spawnItem.GetRepeatDelay(), 0));
                        }
                        EndGroupLevel();
                        foldouts[f + 2] = delayFoldout;

                        bool respawnSystemFoldout = foldouts[f + 3];
                        BeginGroupLevel3(ref respawnSystemFoldout, ContentProperties.RespawnSystem);
                        if (respawnSystemFoldout)
                        {
                            Rect respawnGroupPosition = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight + 1.5f);
                            respawnGroupPosition.height -= 1.5f;

                            Rect labelPosition = new Rect(respawnGroupPosition.x + 14, respawnGroupPosition.y, 35, respawnGroupPosition.height);
                            GUI.Label(labelPosition, ContentProperties.RespawnLogic);

                            Rect buttonPosition = new Rect(labelPosition.x + EditorGUIUtility.labelWidth - 12, respawnGroupPosition.y, respawnGroupPosition.width - (EditorGUIUtility.labelWidth + 3), respawnGroupPosition.height);
                            if (GUI.Button(buttonPosition, spawnItem.GetRespawnLogic()?.GetType().Name ?? "None", EditorStyles.popup))
                            {
                                GenericMenu genericMenu = new GenericMenu();
                                for (int l = 0; l < respawnlogicTypes.Length; l++)
                                {
                                    System.Type respawnLogicType = respawnlogicTypes[l];
                                    RespawnLogicMenuAttribute attribute = AuroraExtension.GetAttribute<RespawnLogicMenuAttribute>(respawnLogicType);
                                    if (attribute != null)
                                    {
                                        genericMenu.AddItem(new GUIContent(attribute.GetPath()), false, () =>
                                        {
                                            RespawnLogicBase respawnLogic = Activator.CreateInstance(respawnLogicType) as RespawnLogicBase;
                                            spawnItem.SetRespawnLogic(respawnLogic);
                                            Debug.Log(spawnItem.GetRespawnLogic()?.ToString());
                                        });
                                    }
                                }
                                genericMenu.ShowAsContext();
                            }

                            if (spawnItem.GetRespawnLogic() != null)
                            {
                                SerializedProperty serializedRespawnLogic = spawnItemsSerialized.GetArrayElementAtIndex(i).FindPropertyRelative("respawnLogic");
                                if (serializedRespawnLogic != null)
                                    AEditorGUILayout.DrawPropertyChilds(serializedRespawnLogic);
                            }
                        }
                        EndGroupLevel();
                        foldouts[f + 3] = respawnSystemFoldout;

                        SerializedProperty onSpawnUnityEventCallback = spawnItemsSerialized.GetArrayElementAtIndex(i).FindPropertyRelative("onSpawnUnityEvent");
                        bool eventFoldout = onSpawnUnityEventCallback.isExpanded;
                        BeginGroupLevel3(ref eventFoldout, "Event");
                        if (eventFoldout)
                        {
                            EditorGUILayout.PropertyField(onSpawnUnityEventCallback, ContentProperties.OnSpawnUnityEventCallback, true);
                        }
                        EndGroupLevel();
                        onSpawnUnityEventCallback.isExpanded = eventFoldout;

                        GUILayout.Space(5);
                        ExpandFoldouts(ref foldouts, f, i);

                        if (AEditorGUILayout.ButtonRight("Remove", AEditorGUILayout.RemoveElementButtonOptions))
                        {
                            spawnItemsSerialized.DeleteArrayElementAtIndex(i);
                            InitializeProperties();
                            break;
                        }

                        GUILayout.Space(5);

                        isAnyFoldoutIsExpanded = true;
                    }
                    EndGroupLevel();
                    foldouts[f] = foldout;
                    instance.SetSpawnItem(i, spawnItem);
                }
                DecreaseIndentLevel();
            }
            else
            {
                HelpBoxMessages.Message("Spawn items is empty...");
            }

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Add Item", "ButtonLeft", AEditorGUILayout.DefaultHandleButtonOptions))
            {
                spawnItemsSerialized.arraySize++;
                spawnItemsSerialized.GetArrayElementAtIndex(spawnItemsSerialized.arraySize - 1).FindPropertyRelative("id").stringValue = string.Format("New Spawn Item [{0}]", spawnItemsSerialized.arraySize);
                InitializeProperties();
            }
            if (GUILayout.Button("Remove All Items", "ButtonRight", AEditorGUILayout.DefaultHandleButtonOptions))
            {
                spawnItemsSerialized.ClearArray();
                InitializeProperties();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            EndGroup();

            if (!isAnyFoldoutIsExpanded)
            {
                actualIndex = -1;
            }
        }

        private Transform[] ClearPoints(Transform[] points)
        {
            if (points == null || points.Length == 0)
            {
                points = new Transform[1] { null };
            }
            else if (points.Length > 1)
            {
                Transform lastPoint = points[points.Length - 1];
                for (int j = 0; j < points.Length - 1; j++)
                {
                    Transform go = points[j];
                    if (go != null)
                    {
                        GameObject.DestroyImmediate(go.gameObject);
                    }
                }
                points = new Transform[1] { lastPoint };
            }
            return points;
        }

        private void ExpandFoldouts(ref bool[] values, int executeIndex, int actualIndex)
        {
            if (executeIndex >= values.Length || this.executeIndex == executeIndex)
            {
                return;
            }

            for (int i = 0; i < values.Length; i++)
            {
                if (i == executeIndex)
                {
                    continue;
                }

                values[i] = false;
            }

            this.executeIndex = executeIndex;
            this.actualIndex = actualIndex;
        }
    }
}