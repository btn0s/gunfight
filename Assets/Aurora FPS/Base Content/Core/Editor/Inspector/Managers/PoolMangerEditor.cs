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
using UnityEngine;


namespace AuroraFPSEditor
{
    [CustomEditor(typeof(PoolManager))]
    [CanEditMultipleObjects]
    public class PoolMangerEditor : AuroraEditor<PoolManager>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent Settings = new GUIContent("Settings");
            public readonly static GUIContent Containers = new GUIContent("Containers");
            public readonly static GUIContent Original = new GUIContent("Original", "Original pool container gameobject.");
            public readonly static GUIContent Allocator = new GUIContent("Allocator", "Pool container allocator type");
            public readonly static GUIContent ActualCapacity = new GUIContent("Actual Capacity", "Actual pool contaner capacity.");
            public readonly static GUIContent ReservedCapacity = new GUIContent("Reserved Capacity", "Reserved pool container capacity.");

            public readonly static GUIContent CreateReallocateButton = new GUIContent("Create / Reallocate");
            public readonly static GUIContent CreateButton = new GUIContent("Create");
            public readonly static GUIContent ReallocateButton = new GUIContent("Reallocate");
        }

        // Settings properties.
        private PoolObject original;
        private PoolContainerBase.Allocator allocator;
        private int capacity;
        private GenericMenu genericMenu;

        // Stored required properties.
        private List<bool> foldouts;

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// 
        /// Implement this method to initialize properties.
        /// </summary>
        public override void InitializeProperties()
        {
            genericMenu = new GenericMenu();

            PoolObject[] poolObjects = Resources.FindObjectsOfTypeAll<PoolObject>();
            for (int i = 0; i < poolObjects.Length; i++)
            {
                PoolObject poolObject = poolObjects[i];
                if (PrefabUtility.GetPrefabAssetType(poolObject) == PrefabAssetType.Regular)
                {
                    genericMenu.AddItem(new GUIContent(poolObject.GetPoolObjectID()), false, () =>
                    {
                        original = poolObject;
                    });
                }
            }

            foldouts = new List<bool>(instance.GetPool().Count);
            for (int i = 0; i < instance.GetPool().Count; i++)
            {
                foldouts.Add(false);
            }
        }

        /// <summary>
        /// Base serializedObject properties.
        /// 
        /// Implement this method to draw a custom properties.
        /// </summary>
        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.Settings);
            DrawSettings();
            EndGroup();

            BeginGroup(ContentProperties.Containers);
            DrawAllContainers();
            EndGroup();
        }

        /// <summary>
        /// Draw pool manager settings.
        /// </summary>
        private void DrawSettings()
        {
            original = AEditorGUILayout.ActionObjectField(ContentProperties.Original, original, false, () => genericMenu.ShowAsContext());
            allocator = AEditorGUILayout.EnumPopup(ContentProperties.Allocator, allocator);
            capacity = AEditorGUILayout.FixedIntField(ContentProperties.ReservedCapacity, capacity, 1);
            CreateReallocateButton();
        }

        /// <summary>
        /// Draw all containers in pool manager.
        /// </summary>
        private void DrawAllContainers()
        {
            if (instance.ContainerCount() > 0)
            {
                IncreaseIndentLevel();
                int index = 0;
                foreach (var container in instance.GetPool())
                {
                    if (foldouts.Count <= index)
                    {
                        foldouts.Add(true);
                    }
                    bool foldout = foldouts[index];
                    BeginGroupLevel2(ref foldout, container.Key);
                    if (foldout)
                    {
                        EditorGUI.BeginDisabledGroup(true);
                        AEditorGUILayout.ObjectField(ContentProperties.Original, container.Value.GetOriginal(), false);
                        AEditorGUILayout.EnumPopup(ContentProperties.Allocator, container.Value.GetAllocator());
                        AEditorGUILayout.FixedIntField(ContentProperties.ReservedCapacity, container.Value.GetSize(), 0);
                        AEditorGUILayout.FixedIntField(ContentProperties.ActualCapacity, container.Value.GetLength(), 0);
                        EditorGUI.EndDisabledGroup();
                        if(AEditorGUILayout.ButtonRight("Remove"))
                        {
                            instance.RemoveContainer(container.Value.GetOriginal());
                            break;
                        }
                    }
                    EndGroupLevel();
                    foldouts[index] = foldout;
                    index++;
                }
                DecreaseIndentLevel();
            }
            else
            {
                HelpBoxMessages.Message("Pool Manager is empty.");
            }
        }

        /// <summary>
        /// GUI layout button for create or reallocate container.
        /// </summary>
        private void CreateReallocateButton()
        {
            GUIContent content = ContentProperties.CreateReallocateButton;
            bool isContains = false;
            string id = "None";

            if (original != null)
            {
                id = original.GetPoolObjectID();
                isContains = instance.ContainsContainer(id);
                content = isContains ? ContentProperties.ReallocateButton : ContentProperties.CreateButton;
            }

            EditorGUI.BeginDisabledGroup(original == null);
            if (AEditorGUILayout.ButtonRight(content, GUILayout.Width(120)))
            {
                if (isContains)
                {
                    instance.ReallocateContainer(id, allocator, capacity);
                    instance.GetPoolContainer(id).GetObjectsStack();
                }
                else
                {
                    instance.InstantiateContainer(original, allocator, capacity);
                    instance.GetPoolContainer(id).GetObjectsStack();
                }
            }
            EditorGUI.EndDisabledGroup();
        }
    }
}