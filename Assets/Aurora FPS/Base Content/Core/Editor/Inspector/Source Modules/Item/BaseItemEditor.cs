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
    [CustomEditor(typeof(BaseItem), true)]
    public class BaseItemEditor : AuroraEditor<BaseItem>
    {
        private const string FocusTextInControl = "Instance ID Controll";

        internal new static class ContentProperties
        {
            public readonly static GUIContent BaseProperties = new GUIContent("Base Properties");
            public readonly static GUIContent ID = new GUIContent("ID", "Unique weapon identifier.");
        }

        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.BaseProperties);
            OnBasePropertiesGUI();
            DrawPropertiesExcluding(serializedObject, GetExcludingProperties());
            EndGroup();
        }

        protected override void AddExcludingProperties(ref System.Collections.Generic.List<string> excludingProperties)
        {
            base.AddExcludingProperties(ref excludingProperties);
            excludingProperties.Add("id");
        }

        public virtual void OnBasePropertiesGUI()
        {
            instance.SetID(AEditorGUILayout.IDTextField(ContentProperties.ID, instance.GetID(), (value) => instance.SetID(value)));
        }
    }
}