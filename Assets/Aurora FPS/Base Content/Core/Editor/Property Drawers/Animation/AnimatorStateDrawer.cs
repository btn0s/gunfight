/* ==================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================== */

using UnityEngine;
using UnityEditor;
using AuroraFPSRuntime;

namespace AuroraFPSEditor
{
    [CustomPropertyDrawer(typeof(AnimatorState), true)]
    public class AnimatorStateDrawer : PropertyDrawer
    {
        /// <summary>
        /// Override this method to make your own IMGUI based GUI for the property.
        /// </summary>
        /// <param name="position">Rectangle on the screen to use for the property GUI.</param>
        /// <param name="property">The SerializedProperty to make the custom GUI for.</param>
        /// <param name="label">The label of this property.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty name = property.FindPropertyRelative("name");
            SerializedProperty layer = property.FindPropertyRelative("layer");
            SerializedProperty fixedTime = property.FindPropertyRelative("fixedTime");
            SerializedProperty nameHash = property.FindPropertyRelative("nameHash");

            string storedName = name.stringValue;
            Rect namePosition = new Rect(position.x, position.y, position.width - 50, position.height);
            name.stringValue = EditorGUI.DelayedTextField(namePosition, label, name.stringValue);
            if (storedName != name.stringValue)
            {
                nameHash.intValue = Animator.StringToHash(name.stringValue);
            }
            
            int storedIndexLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            Rect layerPosition = new Rect(position.x + namePosition.width, position.y, 14, position.height);
            layer.intValue = EditorGUI.IntField(layerPosition, layer.intValue);

            Rect fixedTimePosition = new Rect(layerPosition.x + layerPosition.width + 1, position.y, 35, position.height);
            fixedTime.floatValue = EditorGUI.FloatField(fixedTimePosition, fixedTime.floatValue);
            EditorGUI.indentLevel = storedIndexLevel;
        }
    }
}