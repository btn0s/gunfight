/* ==================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================== */

using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AuroraFPSEditor
{
    public sealed class AEditorGUI
    {
        /// <summary>
        /// Make a field to receive any object type.
        /// </summary>
        /// <param name="rect">Rectangle on the screen to use for the field.</param>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The object the field shows.</param>
        /// <param name="allowSceneObjects">Allow assigning Scene objects. See Description for more info.</param>
        /// <typeparam name="TObject">The type of the objects that can be assigned.</typeparam>
        /// <returns>The object that has been set by the user.</returns>
        public static TObject ObjectField<TObject>(Rect rect, GUIContent content, TObject value, bool allowSceneObjects) where TObject : Object
        {
            return (TObject) EditorGUI.ObjectField(rect, content, value, typeof(TObject), allowSceneObjects);
        }

        /// <summary>
        /// Make a field to receive any object type.
        /// </summary>
        /// <param name="rect">Rectangle on the screen to use for the field.</param>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The object the field shows.</param>
        /// <param name="allowSceneObjects">Allow assigning Scene objects. See Description for more info.</param>
        /// <typeparam name="TObject">The type of the objects that can be assigned.</typeparam>
        /// <returns>The object that has been set by the user.</returns>
        public static TObject ObjectField<TObject>(Rect rect, string content, TObject value, bool allowSceneObjects) where TObject : Object
        {
            return (TObject) EditorGUI.ObjectField(rect, content, value, typeof(TObject), allowSceneObjects);
        }

        /// <summary>
        /// Make a field to receive any object type.
        /// </summary>
        /// <param name="rect">Rectangle on the screen to use for the field.</param>
        /// <param name="value">The object the field shows.</param>
        /// <param name="allowSceneObjects">Allow assigning Scene objects. See Description for more info.</param>
        /// <typeparam name="TObject">The type of the objects that can be assigned.</typeparam>
        /// <returns>The object that has been set by the user.</returns>
        public static TObject ObjectField<TObject>(Rect rect, TObject value, bool allowSceneObjects) where TObject : Object
        {
            return (TObject) EditorGUI.ObjectField(rect, GUIContent.none, value, typeof(TObject), allowSceneObjects);
        }

        /// <summary>
        /// Make a field for layer masks.
        /// </summary>
        /// <param name="rect">Rectangle on the screen to use for the field.</param>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="layerMask">The current mask to display.</param>
        /// <returns>The layer mask modified by the user.</returns>
        public static LayerMask LayerMaskField(Rect rect, GUIContent content, LayerMask layerMask)
        {
            LayerMask mask = EditorGUI.MaskField(rect, content, InternalEditorUtility.LayerMaskToConcatenatedLayersMask(layerMask), InternalEditorUtility.layers);
            return InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(mask);
        }

        /// <summary>
        /// Make a field for layer masks.
        /// </summary>
        /// <param name="rect">Rectangle on the screen to use for the field.</param>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="layerMask">The current mask to display.</param>
        /// <returns>The layer mask modified by the user.</returns>
        public static LayerMask LayerMaskField(Rect rect, string content, LayerMask layerMask)
        {
            LayerMask mask = EditorGUI.MaskField(rect, content, InternalEditorUtility.LayerMaskToConcatenatedLayersMask(layerMask), InternalEditorUtility.layers);
            return InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(mask);
        }

        /// <summary>
        /// Make a field for layer masks.
        /// </summary>
        /// <param name="rect">Rectangle on the screen to use for the field.</param>
        /// <param name="layerMask">The current mask to display.</param>
        /// <returns>The layer mask modified by the user.</returns>
        public static LayerMask LayerMaskField(Rect rect, LayerMask layerMask)
        {
            LayerMask mask = EditorGUI.MaskField(rect, InternalEditorUtility.LayerMaskToConcatenatedLayersMask(layerMask), InternalEditorUtility.layers);
            return InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(mask);
        }

        /// <summary>
        /// Make a text field for entering fixed integers with the lowest possible value.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="minValue">The lowest possible value.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static float FixedFloatField(Rect position, GUIContent content, float value, float minValue)
        {
            value = EditorGUI.FloatField(position, content, value);
            if (value < minValue) value = minValue;
            return value;
        }

        /// <summary>
        /// Make a text field for entering fixed integers with the lowest possible value.
        /// </summary>
        /// <param name="rect">Rectangle on the screen to use for the field.</param>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="minValue">The lowest possible value.</param>
        /// <returns>The value entered by the user.</returns>
        public static float FixedFloatField(Rect rect, float value, float minValue)
        {
            value = EditorGUI.FloatField(rect, value);
            if (value < minValue) value = minValue;
            return value;
        }

        /// <summary>
        /// Make a required text field.
        /// Displaying error icon when text is null or empty.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="text">The text to edit.</param>
        /// <param name="options">An optional list of layout options that specify extra layout properties. Any
        //     values passed in here will override settings defined by the style.<br> See Also:
        //     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        //     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The text entered by the user.</returns>
        public static string RequiredTextField(Rect rect, GUIContent content, string text)
        {
            bool isValid = !string.IsNullOrEmpty(text);
            Rect fieldPosition = new Rect(rect.x, rect.y, rect.width - (!isValid ? 19 : 0), rect.height);
            text = EditorGUI.TextField(fieldPosition, content, text);

            if (!isValid)
            {
                GUIContent iconContent = EditorGUIUtility.IconContent("CollabError", "|Text is required!");
                Rect iconPosition = new Rect(fieldPosition.x + fieldPosition.width + 1.5f, fieldPosition.y, 30, fieldPosition.height);
                GUI.Label(iconPosition, iconContent);
            }
            return text;
        }

        /// <summary>
        /// Make a required text field.
        /// Displaying error icon when text is null or empty.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="text">The text to edit.</param>
        /// <param name="options">An optional list of layout options that specify extra layout properties. Any
        //     values passed in here will override settings defined by the style.<br> See Also:
        //     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        //     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The text entered by the user.</returns>
        public static string RequiredTextField(Rect rect, string content, string text)
        {
            bool isValid = !string.IsNullOrEmpty(text);
            Rect fieldPosition = new Rect(rect.x, rect.y, rect.width - (!isValid ? 19 : 0), rect.height);
            text = EditorGUI.TextField(fieldPosition, content, text);

            if (!isValid)
            {
                GUIContent iconContent = EditorGUIUtility.IconContent("CollabError", "|Text is required!");
                Rect iconPosition = new Rect(fieldPosition.x + fieldPosition.width + 1.5f, fieldPosition.y, 30, fieldPosition.height);
                GUI.Label(iconPosition, iconContent);
            }
            return text;
        }

        /// <summary>
        /// Make a required text field.
        /// Displaying error icon when text is null or empty.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="text">The text to edit.</param>
        /// <param name="options">An optional list of layout options that specify extra layout properties. Any
        //     values passed in here will override settings defined by the style.<br> See Also:
        //     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        //     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The text entered by the user.</returns>
        public static string RequiredTextField(Rect rect, string text)
        {
            bool isValid = !string.IsNullOrEmpty(text);
            Rect fieldPosition = new Rect(rect.x, rect.y, rect.width - (!isValid ? 19 : 0), rect.height);
            text = EditorGUI.TextField(fieldPosition, text);

            if (!isValid)
            {
                GUIContent iconContent = EditorGUIUtility.IconContent("CollabError", "|Text is required!");
                Rect iconPosition = new Rect(fieldPosition.x + fieldPosition.width + 1.5f, fieldPosition.y, 30, fieldPosition.height);
                GUI.Label(iconPosition, iconContent);
            }
            return text;
        }

        public static bool OptionsButton(Rect rect)
        {
            bool value = false;
            GUIContent iconContent = EditorGUIUtility.IconContent("_Popup", "|Option field button.");
            if (GUI.Button(rect, iconContent, GUI.skin.label))
            {
                value = true;
            }
            return value;
        }

        public static bool ResetButton(Rect rect)
        {
            bool value = false;
#if UNITY_2019_3_OR_NEWER
            GUIContent iconContent = EditorGUIUtility.IconContent("ClothInspector.PaintValue", "|Option field button.");
#else
            GUIContent iconContent = EditorGUIUtility.IconContent("LookDevResetEnv", "|Option field button.");
#endif
            if (GUI.Button(rect, iconContent, GUI.skin.label))
            {
                value = true;
            }
            return value;
        }

        public static Rect[] SplitRect(Rect rectToSplit, int n)
        {
            Rect[] rects = new Rect[n];

            for (int i = 0; i < n; i++)
                rects[i] = new Rect(rectToSplit.position.x + (i * rectToSplit.width / n) - (EditorGUI.indentLevel * 15), rectToSplit.position.y, (rectToSplit.width / n) + (EditorGUI.indentLevel * 15), rectToSplit.height);

            int padding = (int)rects[0].width - 41 - (EditorGUI.indentLevel * 17);
            int space = 3;

            rects[0].width -= padding + space;
            rects[2].width -= padding + space;
            rects[1].x -= padding;
            rects[1].width += padding * 2;
            rects[2].x += padding + space - 1;

            return rects;
        }
    }
}