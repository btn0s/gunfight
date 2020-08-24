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
using UnityEngine;

namespace AuroraFPSEditor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ObjectHealth), true)]
    public class ObjectHealthEditor : AuroraEditor<ObjectHealth>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent HealthProperties = new GUIContent("Base Properties");
            public readonly static GUIContent Health = new GUIContent("Health", "Health point value.");
            public readonly static GUIContent MaxHealth = new GUIContent("Max Health", "Max health point value.");
            public readonly static GUIContent MinHealth = new GUIContent("Min Health", "Min health point value.");
        }

        /// <summary>
        /// Base serializedObject properties.
        /// 
        /// Implement this method to draw a custom properties.
        /// </summary>
        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.HealthProperties);
            OnBasePropertiesGUI();
            OnEventsGUI();
            EndGroup();
        }

        public virtual void OnBasePropertiesGUI()
        {
            instance.SetHealth(EditorGUILayout.IntSlider(ContentProperties.Health, instance.GetHealth(), 0, instance.GetMaxHealth()));
            instance.SetMinHealth(EditorGUILayout.IntField(ContentProperties.MinHealth, instance.GetMinHealth()));
            instance.SetMaxHealth(EditorGUILayout.IntField(ContentProperties.MaxHealth, instance.GetMaxHealth()));
            DrawPropertiesExcluding(serializedObject, GetExcludingProperties());
        }

        protected override void AddExcludingProperties(ref List<string> excludingProperties)
        {
            base.AddExcludingProperties(ref excludingProperties);
            excludingProperties.Add("health");
            excludingProperties.Add("minHealth");
            excludingProperties.Add("maxHealth");
        }
    }
}