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
    [CustomEditor(typeof(WeaponAnimationEventHandler))]
    public class WeaponAnimationEventHandlerEditor : AuroraEditor<WeaponAnimationEventHandler>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent BaseProperties = new GUIContent("Base Properties");
            public readonly static GUIContent ShakeEvents = new GUIContent("Shake Properties", "Shake properties mapping.");
        }

        /// <summary>
        /// Base serializedObject properties.
        /// 
        /// Implement this method to draw a custom properties.
        /// </summary>
        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.BaseProperties);
            instance.SetShakePropertiesMapping(AEditorGUILayout.ObjectField(ContentProperties.ShakeEvents, instance.GetShakePropertiesMapping(), true));
            EndGroup();
        }
    }
}