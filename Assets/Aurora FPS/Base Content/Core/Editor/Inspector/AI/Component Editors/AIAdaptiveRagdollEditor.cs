/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using AuroraFPSRuntime.AI;
using UnityEditor;
using UnityEngine;

namespace AuroraFPSEditor
{
    [CustomEditor(typeof(AIAdaptiveRagdoll))]
    public class AIAdaptiveRagdollEditor : CharacterRagdollEditor
    {
        internal new static class ContentProperties
        {
            // Ragdoll properties contents
            public readonly static GUIContent RagdollProperties = new GUIContent("Ragdoll Properties");
            public readonly static GUIContent RelativeVelocity = new GUIContent("Relative Velocity Limit", "Limits of the relative linear velocity of the two colliding objects for the character to start ragdoll system.");
            public readonly static GUIContent StandDelay = new GUIContent("Stand Delay", "Delay before character stand, after ragdoll is played and wait in stable position.");
        }

        protected AIAdaptiveRagdoll aiAdaptiveRagdollInstance;

        public override void InitializeProperties()
        {
            base.InitializeProperties();
            aiAdaptiveRagdollInstance = instance as AIAdaptiveRagdoll;
        }

        /// <summary>
        /// Base serializedObject properties.
        /// 
        /// Implement this method to draw a custom properties.
        /// </summary>
        public override void OnBasePropertiesGUI()
        {
            aiAdaptiveRagdollInstance.SetRelativeVelocityLimit(AEditorGUILayout.FixedFloatField(ContentProperties.RelativeVelocity, aiAdaptiveRagdollInstance.GetRelativeVelocityLimit(), 0));
            aiAdaptiveRagdollInstance.SetStandDelay(AEditorGUILayout.FixedFloatField(ContentProperties.StandDelay, aiAdaptiveRagdollInstance.GetStandDelay(), 0));
            base.OnBasePropertiesGUI();
        }
    }
}