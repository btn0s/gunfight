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
    [CustomEditor(typeof(PoolObject), true)]
    [CanEditMultipleObjects]
    public class PoolObjectEditor : AuroraEditor<PoolObject>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent BaseProperties = new GUIContent("Base Properties");
            public readonly static GUIContent PoolObjectProperties = new GUIContent("Pool Object");
            public readonly static GUIContent ID = new GUIContent("ID", "Pool object id.");
            public readonly static GUIContent Delay = new GUIContent("Delay", "Delay before object will be added in pool.");
            public readonly static GUIContent OriginalScale = new GUIContent("Original Scale", "");
        }

        private bool foldout;
        private bool hasOtherProperties;

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// 
        /// Implement this method to initialize properties.
        /// </summary>
        public override void InitializeProperties()
        {
            int count = 0;
            foreach (var item in EditorHelper.GetChildren(serializedObject))
            {
                count++;
                if (count > 4)
                {
                    hasOtherProperties = true;
                    break;
                }
            }
        }

        /// <summary>
        /// Base serializedObject properties.
        /// 
        /// Implement this method to draw a custom properties.
        /// </summary>
        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.BaseProperties);
            OnImplementationPropertiesGUI();
            OnPoolObjectPropertiesGUI();
            OnEventsGUI();
            EndGroup();
        }

        /// <summary>
        /// Drawing all childs implementation properties.
        /// </summary>
        public virtual void OnImplementationPropertiesGUI()
        {
            DrawPropertiesExcluding(serializedObject, GetExcludingProperties());
        }

        /// <summary>
        /// On pool object properties GUI.
        /// 
        /// Implement this method to make a custom draw of pool object properties.
        /// </summary>
        public virtual void OnPoolObjectPropertiesGUI()
        {
            if (hasOtherProperties)
            {
                IncreaseIndentLevel();
                BeginGroupLevel2(ref foldout, ContentProperties.PoolObjectProperties);
                if (foldout)
                {
                    DrawPoolObjectProperties();
                }
                EndGroupLevel();
                DecreaseIndentLevel();
            }
            else
            {
                DrawPoolObjectProperties();
            }
        }

        /// <summary>
        /// Drawing default pool object properties.
        /// </summary>
        public virtual void DrawPoolObjectProperties()
        {
            instance.SetPoolObjectID(AEditorGUILayout.IDTextField(ContentProperties.ID, instance.GetPoolObjectID(), (value) => instance.SetPoolObjectID(value)));
            instance.SetDelayedTime(AEditorGUILayout.SwitchableFloatField(ContentProperties.Delay, instance.GetDelayedTime(), -0.01f, "Not use.", 2));
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
            excludingProperties.Add("poolObjectID");
            excludingProperties.Add("delayedTime");
        }
    }
}