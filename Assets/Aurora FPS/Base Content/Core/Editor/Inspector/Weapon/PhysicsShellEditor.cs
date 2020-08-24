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
    [CustomEditor(typeof(PhysicsShell), true)]
    public class PhysicsShellEditor : PoolObjectEditor
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent BaseProperties = new GUIContent("Base Properties");
            public readonly static GUIContent BulletItem = new GUIContent("Bullet Item", "Target bullet item.");
        }

        protected PhysicsShell physicsShellInstance;

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// 
        /// Implement this method to initialize properties.
        /// </summary>
        public override void InitializeProperties()
        {
            base.InitializeProperties();
            physicsShellInstance = instance as PhysicsShell;
        }

        /// <summary>
        /// On implementation properties GUI.
        /// 
        /// Implement this method to make a custom draw of implementation properties.
        /// </summary>
        public override void OnImplementationPropertiesGUI()
        {
            physicsShellInstance.SetShellItem(AEditorGUILayout.RequiredObjectField(ContentProperties.BulletItem, physicsShellInstance.GetShellItem(), true));
            base.OnImplementationPropertiesGUI();
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
            excludingProperties.Add("shellItem");
        }
    }
}