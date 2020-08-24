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

namespace AuroraFPSEditor
{
    [CustomEditor(typeof(ScriptableMappingArray<>),true)]
    public class ScriptableMappingArrayEditor : AuroraEditor
    {
        private ArrayEditor mapping;

        public override void InitializeProperties()
        {
            SerializedProperty mappingValues = serializedObject.FindProperty("mappingValues");
            mapping = new ArrayEditor(mappingValues);
        }

        public override void OnBaseGUI()
        {
            mapping.DrawLayoutGroup();
        }
    }
}