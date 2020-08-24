/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using UnityEditor;
using AuroraFPSRuntime;
using UnityEngine;

namespace AuroraFPSEditor
{
    [CustomPropertyDrawer(typeof(RangedFloat), true)]
    public class RangedFloatDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect controlRect = EditorGUI.PrefixLabel(position, label);
            Rect[] splittedRect = AEditorGUI.SplitRect(controlRect, 3);

            SerializedProperty min = property.FindPropertyRelative("min");
            SerializedProperty max = property.FindPropertyRelative("max");
            SerializedProperty minLimit = property.FindPropertyRelative("minLimit");
            SerializedProperty maxLimit = property.FindPropertyRelative("maxLimit");

            float minValue = min.floatValue;
            float maxValue = max.floatValue;

            minValue = EditorGUI.FloatField(splittedRect[0], AMath.AllocatePart(minValue));
            maxValue = EditorGUI.FloatField(splittedRect[2], AMath.AllocatePart(maxValue));

            EditorGUI.MinMaxSlider(splittedRect[1], ref minValue, ref maxValue, minLimit.floatValue, maxLimit.floatValue);

            if (minValue < minLimit.floatValue)
                minValue = minLimit.floatValue;
            else if (minValue > maxValue)
                minValue = maxValue;

            if (maxValue > maxLimit.floatValue)
                maxValue = maxLimit.floatValue;
            else if (maxValue < minValue)
                maxValue = minValue;

            min.floatValue = minValue;
            max.floatValue = maxValue;
        }
    }
}