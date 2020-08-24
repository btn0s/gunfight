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
    [CustomEditor(typeof(CharacterRagdoll), true)]
    public class CharacterRagdollEditor : AuroraEditor<CharacterRagdoll>
    {
        internal new static class ContentProperties
        {
            // Animation properties contents
            public readonly static GUIContent BaseProperties = new GUIContent("Base Properties");
            public readonly static GUIContent BellyStateName = new GUIContent("Belly State", "Animator state name to get up from belly.");
            public readonly static GUIContent BackStateName = new GUIContent("Back State", "Animator state name to get up from back.");
        }

        /// <summary>
        /// Base serializedObject properties.
        /// 
        /// Implement this method to draw a custom properties.
        /// </summary>
        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.BaseProperties);
            OnBasePropertiesGUI();
            OnEventsGUI();
            EndGroup();
        }

        public virtual void OnBasePropertiesGUI()
        {
            int storedIndentLevel = EditorGUI.indentLevel;
            Rect bellyPosition = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight);
            Rect bellyStatePosition = new Rect(bellyPosition.x, bellyPosition.y, bellyPosition.width - 30, bellyPosition.height);
            Rect bellyTimePosition = new Rect(bellyPosition.x + bellyStatePosition.width, bellyPosition.y, 30, bellyPosition.height);
            instance.GetGetUpFromBellyState().SetName(EditorGUI.TextField(bellyStatePosition, ContentProperties.BellyStateName, instance.GetGetUpFromBellyState().GetName()));
            EditorGUI.indentLevel = 0;
            instance.SetBellyStandTime(AEditorGUI.FixedFloatField(bellyTimePosition, instance.GetBellyStandTime(), 0));
            EditorGUI.indentLevel = storedIndentLevel;

            GUILayout.Space(2);
            Rect backPosition = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight);
            Rect backStatePosition = new Rect(backPosition.x, backPosition.y, backPosition.width - 30, backPosition.height);
            Rect backTimePosition = new Rect(backPosition.x + backStatePosition.width, backPosition.y, 30, backPosition.height);
            instance.GetGetUpFromBackState().SetName(EditorGUI.TextField(backStatePosition, ContentProperties.BackStateName, instance.GetGetUpFromBackState().GetName()));
            EditorGUI.indentLevel = 0;
            instance.SetBackStandTime(AEditorGUI.FixedFloatField(backTimePosition, instance.GetBackStandTime(), 0));
            EditorGUI.indentLevel = storedIndentLevel;
        }
    }
}