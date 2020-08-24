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
    [CustomPropertyDrawer(typeof(AnimatorValue), true)]
    public class AnimatorValueDrawer : PropertyDrawer
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
            SerializedProperty nameHash = property.FindPropertyRelative("nameHash");

            string storedName = name.stringValue;
            name.stringValue = EditorGUI.DelayedTextField(position, label, name.stringValue);
            if (storedName != name.stringValue)
            {
                nameHash.intValue = Animator.StringToHash(name.stringValue);
            }
        }
    }
}