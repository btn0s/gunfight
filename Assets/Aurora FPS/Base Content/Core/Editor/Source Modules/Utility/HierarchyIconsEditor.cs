/* ==================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================== */

using System;
using UnityEditor;
using UnityEngine;

namespace AuroraFPSEditor
{
    [InitializeOnLoad]
    static class HierarchyIconsEditor
    {
        private const string HeirarchyWindowID = " (UnityEditor.SceneHierarchyWindow)";

        internal struct HierarchyIconsProperties
        {
            private System.Type component;
            private Texture2D icon;

            public HierarchyIconsProperties(Type component, Texture2D icon)
            {
                this.component = component;
                this.icon = icon;
            }

            public HierarchyIconsProperties(Type component, string name)
            {
                this.component = component;
                this.icon = Resources.Load<Texture2D>(EditorResourcesHelper.PropertiesIconsPath + name);
            }

            public Type GetComponent()
            {
                return component;
            }

            public void SetComponent(Type value)
            {
                component = value;
            }

            public Texture2D GetIcon()
            {
                return icon;
            }

            public void SetIcon(Texture2D value)
            {
                icon = value;
            }
        }

        private static Rect sceneHierarchyWindowRect = Rect.zero;
        private static HierarchyIconsProperties[] hierarchyIconsProperties;

        static HierarchyIconsEditor()
        {
            IntializeHierarchyIcons(ref hierarchyIconsProperties);
            if (hierarchyIconsProperties != null)
            {
                // EditorApplication.hierarchyWindowItemOnGUI += DrawAuroraIcons;
            }
        }

        private static void IntializeHierarchyIcons(ref HierarchyIconsProperties[] properties)
        {
            properties = new HierarchyIconsProperties[]
            {
                new HierarchyIconsProperties(typeof(AuroraFPSRuntime.AI.AICore), "AI"),
                new HierarchyIconsProperties(typeof(AuroraFPSRuntime.ControllerBase), "Controller")
            };
        }

        private static void DrawAuroraIcons(int instanceID, Rect selectionRect)
        {
            Rect r = new Rect(selectionRect);
            r.x = r.width + 40;

            if (EditorWindow.focusedWindow?.ToString() == HeirarchyWindowID)
            {
                sceneHierarchyWindowRect = EditorWindow.focusedWindow.position;
            }

            GameObject hierarchyItem = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (hierarchyItem != null)
            {
                // Debug.Log(hierarchyItem.transform.root == hierarchyItem.transform);
                for (int i = 0, length = hierarchyIconsProperties.Length; i < length; i++)
                {
                    HierarchyIconsProperties property = hierarchyIconsProperties[i];
                    if (hierarchyItem.GetComponent(property.GetComponent()))
                    {
                        GUI.Label(r, property.GetIcon());
                        break;
                    }
                }
            }
        }
    }
}