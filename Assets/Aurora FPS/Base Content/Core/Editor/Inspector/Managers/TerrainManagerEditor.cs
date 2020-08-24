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
using UnityEditorInternal;
using UnityEngine;

namespace AuroraFPSEditor
{
    [CustomEditor(typeof(TerrainManager))]
    public class TerrainManagerEditor : AuroraEditor<TerrainManager>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent BaseProperties = new GUIContent("Base Properties");
        }

        private ReorderableList excludingTerrainsList;
        private Terrain[] allSceneTerrains;

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// 
        /// Implement this method to initialize properties.
        /// </summary>
        public override void InitializeProperties()
        {
            SerializedProperty excludingTerrains = serializedObject.FindProperty("excludingTerrains");

            allSceneTerrains = GameObject.FindObjectsOfType<Terrain>();

            excludingTerrainsList = new ReorderableList(serializedObject, excludingTerrains, true, true, true, true);
            excludingTerrainsList.drawHeaderCallback = (rect) =>
            {
                Rect labelRect = new Rect(rect.x, rect.y + 1.5f, rect.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(labelRect, "Excluding Terrains");
            };

            excludingTerrainsList.drawElementCallback = (rect, index, isActive, isFocuse) =>
            {
                SerializedProperty excludingTerrain = excludingTerrains.GetArrayElementAtIndex(index);

                bool hasErrors = false;
                GUIContent iconContent = EditorGUIUtility.IconContent("console.warnicon.inactive.sml");

                List<string> list = new List<string>();
                for (int i = 0; i < index; i++)
                {
                    list.Add(excludingTerrains.GetArrayElementAtIndex(i).stringValue);
                }

                if (list.Contains(excludingTerrain.stringValue))
                {
                    hasErrors = true;
                    iconContent = EditorGUIUtility.IconContent("console.warnicon.sml");
                }

                if (allSceneTerrains != null)
                {
                    for (int i = 0; i < allSceneTerrains.Length; i++)
                    {
                        if (excludingTerrain.stringValue != allSceneTerrains[i].name)
                        {
                            hasErrors = true;
                            iconContent = EditorGUIUtility.IconContent("CollabError");
                        }
                    }
                }


                Rect propertyRect = new Rect(rect.x, rect.y + 1.5f, rect.width, EditorGUIUtility.singleLineHeight);

                if (hasErrors)
                {
                    propertyRect.width -= 20;
                    Rect iconRect = new Rect(propertyRect.width + 52.5f, propertyRect.y, 15, propertyRect.height);
#if UNITY_2019_3_OR_NEWER
                    iconRect.x += 6.5f;
#endif
                    EditorGUI.LabelField(iconRect, iconContent);
                }

                excludingTerrain.stringValue = EditorGUI.TextField(propertyRect, GUIContent.none, excludingTerrain.stringValue);
            };
        }

        /// <summary>
        /// Base serializedObject properties.
        /// 
        /// Implement this method to draw a custom properties.
        /// </summary>
        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.BaseProperties);
            excludingTerrainsList.DoLayoutList();
            EndGroup();
        }
    }
}