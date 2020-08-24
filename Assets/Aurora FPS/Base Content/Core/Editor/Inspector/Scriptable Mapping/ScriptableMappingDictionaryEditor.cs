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
    [CustomEditor(typeof(ScriptableMappingDictionary<, ,>), true)]
    public class ScriptableMappingDictionaryEditor : AuroraEditor
    {
        private SerializedProperty mapping;

        public override void InitializeProperties()
        {
            mapping = serializedObject.FindProperty("mapping");
        }

        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.BaseProperties);
            IncreaseIndentLevel();
            bool isExpanded = mapping.isExpanded;
            BeginGroupLevel2(ref isExpanded, "Mapping");
            if (isExpanded)
            {
                EditorGUILayout.PropertyField(mapping, new GUIContent("List"));
            }
            EndGroupLevel();
            mapping.isExpanded = isExpanded;
            DecreaseIndentLevel();
            EndGroup();
        }
    }
}