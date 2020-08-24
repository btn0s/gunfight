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
using AuroraFPSRuntime;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AuroraFPSEditor.Window.Manager
{
    [TreeElement("Mapping Assets", "General", 3)]
    [TreeElementTarget(typeof(AuroraManager))]
    internal sealed class AMMappingElement : TreeElementEditor
    {
        public struct Properties
        {
            public Type[] types;
            public string[] names;

            public Properties(List<Type> types)
            {
                types.Remove(typeof(ScriptableMappingArray<>));
                types.Remove(typeof(ScriptableMappingDictionary<, ,>));
                this.types = types.ToArray();
                if (this.types != null)
                {
                    names = new string[this.types.Length];
                    for (int i = 0, length = this.types.Length; i < length; i++)
                    {
                        names[i] = AuroraEditor.GenerateHeaderName(this.types[i].Name);
                    }
                }
                else
                {
                    names = new string[1] { "None" };
                }
            }
        }

        private Properties properties;

        // Base mapping assets properties.
        private List<Type> mappingTypes;
        private List<Object> mappingAssets;
        private ReorderableList mappingList;

        // Mapping assets popup name properties.
        private string[] popupNames;
        private int selectedMappingIndex;
        private int lastSelectedMappingIndex;

        // Stored required properties.
        private AuroraManager auroraManagerWindow;

        /// <summary>
        /// Called once when tree element initialized.
        /// </summary>
        /// <param name="id">Unique option id.</param>
        public AMMappingElement(int id) : base(id)
        {
            InitializeMappingTypes();
            OnMappingTypeSwitchedCallback += SwitchMappingAssetType;
        }

        /// <summary>
        /// Called when the manager becomes enabled and active.
        /// Initializing base ManagerItem properties.
        /// </summary>
        public override void OnManagerEnable()
        {
            auroraManagerWindow = EditorWindow.GetWindow<AuroraManager>();
        }

        /// <summary>
        /// Called when the Manager window gets keyboard or mouse focus.
        /// </summary>
        public override void OnManagerFocus()
        {
            InitializeMappingTypes();
        }

        /// <summary>
        /// OnBaseGUI is called for rendering and handling GUI events.
        /// This function can be called multiple times per frame (one call per event).
        /// </summary>
        public override void OnBaseGUI()
        {
            DrawPopup();
            GUILayout.Space(5);
            DrawSelectedMappingAssets();
        }

        /// <summary>
        /// Draw asset name generic popup selection field.
        /// </summary>
        public void DrawPopup()
        {
            selectedMappingIndex = EditorGUILayout.Popup("Mapping Type", selectedMappingIndex, popupNames);
            OnMappingTypeSwitchedHandler();
        }

        /// <summary>
        /// Draw selected mapping assets list.
        /// </summary>
        public void DrawSelectedMappingAssets()
        {
            mappingList?.DoLayoutList();
        }

        public void CreateNewAsset(string name, Type type)
        {
            string path = EditorUtility.SaveFilePanelInProject("Create new Property", name, "", "");
            if (!string.IsNullOrEmpty(path))
            {
                string fileName = System.IO.Path.GetFileName(path);
                path = path.Replace(fileName, "");
                Object asset = ScriptableObjectHelper.CreateAsset(type, path, fileName);
                AssetDatabase.Refresh();
                Selection.activeObject = asset;
            }
        }

        public void OpenAsset(Object asset)
        {
            if (Selection.activeObject != asset)
            {
                Selection.activeObject = asset;
                return;
            }
            EditorGUIUtility.PingObject(asset);
        }

        public void DeleteAsset(Object asset)
        {
            string path = AssetDatabase.GetAssetPath(asset);
            AssetDatabase.DeleteAsset(path);
            AssetDatabase.Refresh();
        }

        public void DuplicateAsset(Object asset)
        {
            string path = AssetDatabase.GetAssetPath(asset);
            string name = asset.name;
            path = path.Replace(asset.name + ".asset", "");
            if (name[name.Length - 5] == '#')
            {
                name = name.Substring(0, name.Length - 6);
            }

            name = string.Format("{0} #{1}", name, UnityEngine.Random.Range(0, 9999));
            AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(asset), string.Format("{0}{1}.asset", path, name));
            AssetDatabase.Refresh();
        }

        public void InitializeMappingTypes()
        {
            mappingTypes = AuroraExtension.FindSubclassesOf<ScriptableMapping>().ToList();
            if (mappingTypes != null)
            {
                ExecludeSunParent(ref mappingTypes);
                popupNames = GeneratePopupNames(mappingTypes);
                SwitchMappingAssetType(mappingTypes[lastSelectedMappingIndex < mappingTypes.Count ? lastSelectedMappingIndex : 0]);
            }
        }

        public void SwitchMappingAssetType(Type assetType)
        {
            mappingAssets = EditorHelper.FindAssetsByType(assetType);
            mappingList = CreateMappingAssetsList(mappingAssets);
        }

        public void ExecludeSunParent(ref List<Type> mappingAssetTypes)
        {
            mappingAssetTypes.Remove(typeof(ScriptableMappingArray<>));
            mappingAssetTypes.Remove(typeof(ScriptableMappingDictionary<, ,>));
        }

        public string[] GeneratePopupNames(List<Type> mappingAssetTypes)
        {
            if (mappingAssetTypes != null)
            {
                string[] popupNames = new string[mappingAssetTypes.Count];
                for (int i = 0; i < popupNames.Length; i++)
                {
                    popupNames[i] = AuroraEditor.GenerateHeaderName(mappingAssetTypes[i].Name);
                }
                return popupNames;
            }
            return new string[1] { "None" };
        }

        public ReorderableList CreateMappingAssetsList(List<Object> mappingAssets)
        {
            ReorderableList list = new ReorderableList(mappingAssets, typeof(Object), true, false, false, false);

            list.headerHeight = 4;

            list.drawElementCallback = (position, index, isActive, isFocus) =>
            {
                Object asset = mappingAssets[index];

                GUI.color = Color.white;
                Rect labelPosition = new Rect(position.x, position.y + 1.5f, auroraManagerWindow.position.width - 235, EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(labelPosition, mappingAssets[index].name);

                float width = 75;
                GUI.color = new Color32(70, 220, 70, 255);
                Rect openButtonPosition = new Rect(auroraManagerWindow.position.width - 235, position.y + 1.2f, width, EditorGUIUtility.singleLineHeight);
                if (GUI.Button(openButtonPosition, "Open", "ButtonLeft"))
                {
                    OpenAsset(asset);
                }

                GUI.color = new Color32(252, 160, 3, 255);
                Rect duplicateButtonPosition = new Rect(openButtonPosition.x + width, position.y + 1.2f, width, EditorGUIUtility.singleLineHeight);
                if (GUI.Button(duplicateButtonPosition, "Duplicate", "ButtonMid"))
                {
                    DuplicateAsset(asset);
                    InitializeMappingTypes();
                    return;
                }

                GUI.color = new Color32(255, 55, 55, 255);
                Rect deleteButtonPosition = new Rect(openButtonPosition.x + (width * 2), position.y + 1.2f, width, EditorGUIUtility.singleLineHeight);
                if (GUI.Button(deleteButtonPosition, "Delete", "ButtonRight"))
                {
                    DeleteAsset(asset);
                    InitializeMappingTypes();
                    return;
                }

                GUI.color = Color.white;
            };

            return list;
        }

        /// <summary>
        /// OnMappingTypeSwitchedCallback handler. 
        /// Called every time when mapping type being changed.
        /// </summary>
        private void OnMappingTypeSwitchedHandler()
        {
            if (selectedMappingIndex != lastSelectedMappingIndex)
            {
                lastSelectedMappingIndex = selectedMappingIndex;
                OnMappingTypeSwitchedCallback?.Invoke(mappingTypes[selectedMappingIndex]);
            }
        }

        #region [Event Callback Function]
        /// <summary>
        /// Called every time when mapping type being changed.
        /// </summary>
        public event Action<Type> OnMappingTypeSwitchedCallback;
        #endregion
    }
}