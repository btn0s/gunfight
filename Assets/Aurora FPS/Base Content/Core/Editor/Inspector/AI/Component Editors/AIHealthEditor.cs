/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using AuroraFPSRuntime.AI;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AuroraFPSEditor
{
    [CustomEditor(typeof(AIHealth))]
    public class AIHealthEditor : CharacterHealthEditor
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent AnimationProperties = new GUIContent("Animation Properties");
            public readonly static GUIContent HitReactionState = new GUIContent("Hit Reaction State");

        }

        protected AIHealth aiHealthInstance;
        private SerializedProperty hitReactionState;
        private bool animationPropertiesFoldout;

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        public override void InitializeProperties()
        {
            base.InitializeProperties();

            aiHealthInstance = instance as AIHealth;

            hitReactionState = serializedObject.FindProperty("hitReactionState");
        }

        /// <summary>
        /// Base serializedObject properties.
        /// 
        /// Implement this method to draw a custom properties.
        /// </summary>
        public override void OnBasePropertiesGUI()
        {
            base.OnBasePropertiesGUI();
            IncreaseIndentLevel();
            BeginGroupLevel2(ref animationPropertiesFoldout, ContentProperties.AnimationProperties);
            if (animationPropertiesFoldout)
            {
                EditorGUILayout.PropertyField(hitReactionState, ContentProperties.HitReactionState);
            }
            EndGroupLevel();

            DecreaseIndentLevel();
        }


        protected override void AddExcludingProperties(ref List<string> excludingProperties)
        {
            base.AddExcludingProperties(ref excludingProperties);
            excludingProperties.Add("hitReactionState");
        }
    }
}