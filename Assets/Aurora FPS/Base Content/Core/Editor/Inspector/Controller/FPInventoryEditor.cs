/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using AuroraFPSRuntime;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace AuroraFPSEditor
{
    [CustomEditor(typeof(FPInventory))]
    public class FPInventoryEditor : AuroraEditor<FPInventory>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent BaseProperties = new GUIContent("Base Properties");
            public readonly static GUIContent WeaponsContainer = new GUIContent("Container", "Transform instance with all FPS weapons.");
            public readonly static GUIContent AllowSameWeapon = new GUIContent("Allow Same Weapon", "If true: Allow to save two or more identical weapons in the inventory.");
            public readonly static GUIContent SwitchMode = new GUIContent("Switch Mode", "Weapon switch mode.");
            public readonly static GUIContent MouseWheelSensitivity = new GUIContent("Switch Sensitivity", "Switch mouse wheel sensitivity.\nThe higher the value, the more sensitive the switching function will be to change the position of the mouse wheel.\nFor basic mouses suitable range [0.75-1].\nFor sensor based mouses (ex: Magic Mouse by Apple) suitable range [0.35-0.55].");
            public readonly static GUIContent StartWeaponKey = new GUIContent("Start Weapon Key", "Key of weapon (which available in group) which automatically enabled on game start.");
            public readonly static GUIContent StartWeaponPoint = new GUIContent("", "Mark this weapon, for automatically select it on game start.");
            public readonly static GUIContent Groups = new GUIContent("Groups", "Groups of weapons");
        }

        private bool groupFoldout;
        private ReorderableList[] reorderableGroups;
        private bool editGroupName;

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        public override void InitializeProperties()
        {
            SetHeaderName("First Person Inventory");
            InitializeGroupsList();
        }

        /// <summary>
        /// Base serializedObject properties.
        /// 
        /// Implement this method to draw a custom properties.
        /// </summary>
        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.BaseProperties);
            instance.SetWeaponsContainer(AEditorGUILayout.RequiredObjectField(ContentProperties.WeaponsContainer, instance.GetWeaponsContainer(), true));
            instance.AllowIdenticalWeapons(EditorGUILayout.Toggle(ContentProperties.AllowSameWeapon, instance.AllowIdenticalWeapons()));
            instance.SetSwitchMode((SwitchWeaponMode) EditorGUILayout.EnumFlagsField(ContentProperties.SwitchMode, instance.GetSwitchMode()));
            instance.SetMouseWheelSensitivity(EditorGUILayout.Slider(ContentProperties.MouseWheelSensitivity, instance.GetMouseWheelSensitivity(), 0.0f, 1.0f));
            IncreaseIndentLevel();
            BeginGroupLevel2(ref groupFoldout, ContentProperties.Groups);
            if (groupFoldout)
            {
                DecreaseIndentLevel();
                for (int i = 0, length = reorderableGroups.Length; i < length; i++)
                {
                    GUILayout.Space(3);
                    reorderableGroups[i].DoLayoutList();
                    GUILayout.Space(3);
                }
                IncreaseIndentLevel();
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                AddGroupButton();
                EditGroupButton();
                GUILayout.EndHorizontal();
            }
            EndGroupLevel();
            DecreaseIndentLevel();
            OnEventsGUI();
            EndGroup();
        }

        protected virtual void InitializeGroupsList()
        {
            List<InventoryGroup> groups = instance.GetGroups();
            reorderableGroups = new ReorderableList[groups.Count];
            for (int i = 0; i < groups.Count; i++)
            {
                InventoryGroup group = groups[i];
                List<InventorySlot> inventorySlots = group.GetInventorySlots();
                ReorderableList reorderableList = new ReorderableList(inventorySlots, typeof(InventorySlot), true, true, true, true);

                reorderableList.drawHeaderCallback += (rect) =>
                {
                    float coordinateY = rect.y + 1.5f;

                    if (!editGroupName)
                    {
                        Rect headerRect = new Rect(rect.x, coordinateY, rect.width, EditorGUIUtility.singleLineHeight);
                        EditorGUI.LabelField(headerRect, group.GetName());
                    }
                    else
                    {

                        group.SetName(EditorGUI.TextField(new Rect(rect.x, coordinateY, 100, EditorGUIUtility.singleLineHeight - 2), GUIContent.none, group.GetName()));
                        if (GUI.changed)
                        {
                            groups[i - 1] = group;
                            instance.SetGroups(groups);
                            InitializeProperties();
                        }

                        Rect buttonRect = new Rect(rect.width - 17.0f, coordinateY + 0.5f, 57.0f, 14);
                        if (GUI.Button(buttonRect, "Remove") && DisplayDialogs.Confirmation(string.Format("Are you really want to delete [{0}] group?", group.GetName()), "Yes", "Of Course No!"))
                        {
                            groups.Remove(group);
                            instance.SetGroups(groups);
                            InitializeProperties();
                        }
                    }
                };

                reorderableList.drawElementCallback += (rect, index, isActive, isFocused) =>
                {
                    InventorySlot slot = inventorySlots[index];

                    rect.y += 1.5f;

                    GUIStyle radioButton = new GUIStyle(EditorStyles.radioButton);
                    GUIStyle radioButtonActive = new GUIStyle(radioButton);
                    radioButtonActive.normal = radioButtonActive.onNormal;

                    bool isStartKey = slot.GetKey() != KeyCode.None && instance.GetStartWeaponKey() == slot.GetKey();

                    Rect startKeyButton = new Rect(rect.x, rect.y, 20.0f, EditorGUIUtility.singleLineHeight);
#if UNITY_2019_3_OR_NEWER
                    startKeyButton.y += 1.0f;
#endif
                    if (GUI.Button(startKeyButton, ContentProperties.StartWeaponPoint, isStartKey ? radioButtonActive : radioButton))
                    {
                        instance.SetStartWeaponKey(!isStartKey ? slot.GetKey() : KeyCode.None);
                    }

                    GUI.color = slot.GetKey() != KeyCode.None ? Color.white : Color.red;
                    Rect keyRect = new Rect(rect.x + 19, rect.y, 70.0f, EditorGUIUtility.singleLineHeight);
                    slot.SetKey((KeyCode) EditorGUI.EnumPopup(keyRect, slot.GetKey()));
                    GUI.color = Color.white;

                    Rect weaponIDRect = new Rect(rect.x + keyRect.width + 25.0f, rect.y, rect.width - 97.0f, EditorGUIUtility.singleLineHeight);
                    slot.SetWeaponItem(AEditorGUI.ObjectField(weaponIDRect, slot.GetWeaponItem(), true));

                    inventorySlots[index] = slot;
                };

                reorderableGroups[i] = reorderableList;
            }
        }

        /// <summary>
        /// GUI button for add new group in inventory.
        /// </summary>
        protected virtual void AddGroupButton()
        {
            if (GUILayout.Button("Add Group", "ButtonLeft", GUILayout.Width(80), GUILayout.Height(EditorGUIUtility.singleLineHeight + 1.5f)))
            {
                List<InventoryGroup> groups = instance.GetGroups();
                string name = string.Format("New Group {0}", groups.Count + 1);
                groups.Add(new InventoryGroup(name, new List<InventorySlot>()));
                instance.SetGroups(groups);
                InitializeProperties();
            }
        }

        /// <summary>
        /// GUI buttom for edit inventory groups.
        /// </summary>
        protected virtual void EditGroupButton()
        {
            if (GUILayout.Button("Edit Group", "ButtonRight", GUILayout.Width(80), GUILayout.Height(EditorGUIUtility.singleLineHeight + 1.5f)))
            {
                editGroupName = !editGroupName;
            }
        }
    }
}