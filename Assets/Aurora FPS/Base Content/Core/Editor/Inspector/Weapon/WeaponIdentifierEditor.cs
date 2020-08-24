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
    [CustomEditor(typeof(WeaponIdentifier))]
    public class WeaponIdentifierEditor : AuroraEditor<WeaponIdentifier>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent Property = new GUIContent("Property");
            public readonly static GUIContent WeaponID = new GUIContent("Weapon Item");
        }

        /// <summary>
        /// Base serializedObject properties.
        /// 
        /// Implement this method to draw a custom properties.
        /// </summary>
        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.Property);
            instance.SetWeaponItem(AEditorGUILayout.RequiredObjectField(ContentProperties.WeaponID, instance.GetWeaponItem(), true));
            EndGroup();
        }
    }
}