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
    [CustomEditor(typeof(QuickGrenadeThrower))]
    public class QuickGrenadeThrowerEditor : AuroraEditor<QuickGrenadeThrower>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent BaseProperties = new GUIContent("Base Properties");
            public readonly static GUIContent WeaponGrenadeSystem = new GUIContent("Weapon Grenade System", "Weapon grenade system instance.");
            public readonly static GUIContent TimeToThrow = new GUIContent("Time", "Throw time from animation moment.");
            public readonly static GUIContent ThrowKey = new GUIContent("Key", "Input key for throw grenade.");
            public readonly static GUIContent State = new GUIContent("State", "Throw animation state from animator controller.");
        }

        /// <summary>
        /// Base serializedObject properties.
        /// 
        /// Implement this method to draw a custom properties.
        /// </summary>
        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.BaseProperties);
            instance.SetGrenadeThrowSystem(AEditorGUILayout.RequiredObjectField(ContentProperties.WeaponGrenadeSystem, instance.GetGrenadeThrowSystem(), true));
            instance.SetTimeToThrow(AEditorGUILayout.FixedFloatField(ContentProperties.TimeToThrow, instance.GetTimeToThrow(), 0));
            instance.SetThrowKey((KeyCode) EditorGUILayout.EnumPopup(ContentProperties.ThrowKey, instance.GetThrowKey()));
            instance.SetThrowState(AEditorGUILayout.AnimatorStateField(ContentProperties.State, instance.GetThrowState()));
            EndGroup();
        }
    }
}