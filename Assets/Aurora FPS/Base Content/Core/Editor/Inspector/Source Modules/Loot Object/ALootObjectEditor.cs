/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using AuroraFPSRuntime;
using UnityEditor;
using UnityEngine;

namespace AuroraFPSEditor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(LootObject), true)]
    public class ALootObjectEditor : LootObjectBaseEditor
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent BaseGroupLabel = new GUIContent("Base Properties");
            public readonly static GUIContent ObjectMesh = new GUIContent("Mesh", "Root gameobject of loot object meshes.");

            public readonly static GUIContent MultipleLootGroupLabel = new GUIContent("Loot Properties");
            public readonly static GUIContent LootMode = new GUIContent("Loot Mode");
            public readonly static GUIContent LootKey = new GUIContent("Button Name");
            public readonly static GUIContent AllowMultipleLoot = new GUIContent("Allow Multiple", "Allow mutiple loot.");
            public readonly static GUIContent MultipleLootCount = new GUIContent("Multiple Count", "Max multiple loot count.");
            public readonly static GUIContent ReActivateDelay = new GUIContent("Delay", "Re activate delay, after loot.");
            public readonly static GUIContent DefaultLootMessage = new GUIContent("Default Message", "Displayed then loot object available to loot.");
            public readonly static GUIContent AlternativeLootMessage = new GUIContent("Alternative Message", "Displayed then loot object is not available to loot.");

            public readonly static GUIContent LootSound = new GUIContent("Loot Sound", "Sound that will be played when object being looted.");
        }

        protected LootObject lootObjectInstance;

        // Stored required properties.
        private string childPropertiesGroupName;

        // Foldout properties.
        private bool childPropertiesFoldout;
        private bool lootPropertiesFoldout;

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// 
        /// Implement this method to initialize properties.
        /// </summary>
        public override void InitializeProperties()
        {
            base.InitializeProperties();
            lootObjectInstance = instance as LootObject;
            childPropertiesGroupName = CreateChildCPropertiesGroupName();
        }

        /// <summary>
        /// Base serializedObject properties.
        /// 
        /// Implement this method to draw a custom properties.
        /// </summary>
        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.BaseGroupLabel);
            lootObjectInstance.SetObjectMesh(AEditorGUILayout.RequiredObjectField(ContentProperties.ObjectMesh, lootObjectInstance.GetObjectMesh(), true));

            IncreaseIndentLevel();

            BeginGroupLevel2(ref childPropertiesFoldout, childPropertiesGroupName);
            if (childPropertiesFoldout)
            {
                DrawPropertiesExcluding(serializedObject, GetExcludingProperties());
            }
            EndGroupLevel();

            BeginGroupLevel2(ref lootPropertiesFoldout, ContentProperties.MultipleLootGroupLabel);
            if (lootPropertiesFoldout)
            {
                lootObjectInstance.SetLootMode(AEditorGUILayout.EnumPopup(ContentProperties.LootMode, lootObjectInstance.GetLootMode()));
                if (lootObjectInstance.GetLootMode() == LootObject.LootMode.Button)
                {
                    lootObjectInstance.SetLootButton(EditorGUILayout.TextField(ContentProperties.LootKey, lootObjectInstance.GetLootButton()));
                }

                lootObjectInstance.SetAllowMultipleLoot(EditorGUILayout.Toggle(ContentProperties.AllowMultipleLoot, lootObjectInstance.GetAllowMultipleLoot()));
                if (lootObjectInstance.GetAllowMultipleLoot())
                {
                    lootObjectInstance.SetMultipleLootCount(AEditorGUILayout.FixedIntField(ContentProperties.MultipleLootCount, lootObjectInstance.GetMultipleLootCount(), 1));
                    lootObjectInstance.SetReActivateDelay(AEditorGUILayout.FixedFloatField(ContentProperties.ReActivateDelay, lootObjectInstance.GetReActivateDelay(), 0));
                }

                lootObjectInstance.SetDefaultLootMessage(EditorGUILayout.DelayedTextField(ContentProperties.DefaultLootMessage, lootObjectInstance.GetDefaultLootMessage()));
                lootObjectInstance.SetAlternativeLootMessage(EditorGUILayout.DelayedTextField(ContentProperties.AlternativeLootMessage, lootObjectInstance.GetAlternativeLootMessage()));
                lootObjectInstance.SetLootSound(AEditorGUILayout.ObjectField(ContentProperties.LootSound, lootObjectInstance.GetLootSound(), true));
            }
            EndGroupLevel();
            DecreaseIndentLevel();
            OnEventsGUI();
            EndGroup();
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
            excludingProperties.Add("objectMesh");
            excludingProperties.Add("lootMode");
            excludingProperties.Add("lootButton");
            excludingProperties.Add("allowMultipleLoot");
            excludingProperties.Add("multipleLootCount");
            excludingProperties.Add("reActivateDelay");
            excludingProperties.Add("defaultLootMessage");
            excludingProperties.Add("alternativeLootMessage");
            excludingProperties.Add("lootSound");
        }

        public virtual string CreateChildCPropertiesGroupName()
        {
            string name = GetHeaderName();
            if (GetHeaderName().Contains("Loot "))
            {
                name = GetHeaderName().Replace("Loot ", "");
            }
            return string.Format("{0} Properties", name);
        }
    }
}