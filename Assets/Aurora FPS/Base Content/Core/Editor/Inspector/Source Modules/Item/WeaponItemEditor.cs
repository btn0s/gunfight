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
    [CustomEditor(typeof(WeaponItem), true)]
    public class WeaponItemEditor : SceneItemEditor
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent Group = new GUIContent("Group", "Weapon group in inventory.");
        }

        protected WeaponItem weaponItemInstance;

        public override void InitializeProperties()
        {
            base.InitializeProperties();
            weaponItemInstance = instance as WeaponItem;
        }

        public override void OnBasePropertiesGUI()
        {
            base.OnBasePropertiesGUI();
            DrawGroupField();
        }

        protected override void AddExcludingProperties(ref System.Collections.Generic.List<string> excludingProperties)
        {
            base.AddExcludingProperties(ref excludingProperties);
            excludingProperties.Add("group");
        }

        public virtual void DrawGroupField()
        {
            weaponItemInstance.SetGroup(AEditorGUILayout.RequiredTextField(ContentProperties.Group, weaponItemInstance.GetGroup()));
        }
    }
}