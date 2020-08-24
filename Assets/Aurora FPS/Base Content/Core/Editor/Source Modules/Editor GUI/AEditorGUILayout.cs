/* ==================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================== */

using System;
using AuroraFPSRuntime;
using AuroraFPSRuntime.UI;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;
using Shake = AuroraFPSRuntime.CameraShake.Shake;

namespace AuroraFPSEditor
{
    public sealed class AEditorGUILayout
    {
        internal static class ContentProperties
        {
            public readonly static GUIContent ReadButton = new GUIContent(" ←", "Set instance transform vector in this field.");
            public readonly static GUIContent WriteButton = new GUIContent("→", "Set this vector field to instance transform.");
            public readonly static GUIContent ActionButton = new GUIContent("•", "Invoke field action.");

            // Shake properties value field contents.
            public readonly static GUIContent Target = new GUIContent("Target", "Shake vector target.");
            public readonly static GUIContent Amplitude = new GUIContent("Amplitude", "Shake amplitude.");
            public readonly static GUIContent Frequency = new GUIContent("Frequency", "Shake frequency.");
            public readonly static GUIContent Duration = new GUIContent("Duration", "Shake duration (sec).");
            public readonly static GUIContent BlendOverLifetime = new GUIContent("Life Time", "Blend over life time.");

            // Screen fade value field contents.
            public readonly static GUIContent FadeColor = new GUIContent("Color", "Screen fade color.");
            public readonly static GUIContent FadeInSpeed = new GUIContent("In Speed", "Fade in speed.");
            public readonly static GUIContent FadeOutSpeed = new GUIContent("Out Speed", "Fade out speed.");

            // Death camera properties field contents.
            public readonly static GUIContent Position = new GUIContent("Position", "Camera position (relative controller)..");
            public readonly static GUIContent Rotation = new GUIContent("Rotation", "Camera rotation (relative controller).");
            public readonly static GUIContent PositionSpeed = new GUIContent("Speed", "The speed of position displacement of the camera.");
            public readonly static GUIContent RotationSpeed = new GUIContent("Speed", "The speed of rotation displacement of the camera.");

        }

        public enum VectorType
        {
            Position,
            Rotation,
            Scale
        }

        public struct ArraysToolbarEditorProperty
        {
            public GUIContent toolbarName;
            public GUIContent elementsContent;
            public SerializedProperty serializedArray;

            public ArraysToolbarEditorProperty(GUIContent toolbarName, GUIContent elementsContent, SerializedProperty serializedArray)
            {
                this.toolbarName = toolbarName;
                this.elementsContent = elementsContent;
                this.serializedArray = serializedArray;
            }
        }

        /// <summary>
        /// Draw serialized property childs without foldout.
        /// </summary>
        /// <param name="property">Target serialized property reference.</param>
        public static void DrawPropertyChilds(SerializedProperty property)
        {
            foreach (SerializedProperty child in EditorHelper.GetChildren(property))
            {
                EditorGUILayout.PropertyField(child, true);
            }
        }

        /// <summary>
        /// Draw serialized property childs without foldout.
        /// </summary>
        /// <param name="property">Target serialized property reference.</param>
        /// <param name="excludingProperties">Names of properties to exclude from display.</param>
        public static void DrawPropertyChilds(SerializedProperty property, params string[] excludingProperties)
        {
            foreach (SerializedProperty child in EditorHelper.GetChildren(property))
            {
                bool excludingProperty = false;
                for (int i = 0; i < excludingProperties.Length; i++)
                {
                    if (child.name == excludingProperties[i])
                    {
                        excludingProperty = true;
                        break;
                    }
                }

                if (!excludingProperty)
                {
                    EditorGUILayout.PropertyField(child, true);
                }
            }
        }

        /// <summary>
        /// Draw serialized property childs without foldout.
        /// </summary>
        /// <param name="property">Target serialized property reference.</param>
        /// <param name="bottomProperties">The names of the properties that need to be on the bottom.</param>
        public static void DrawPropertyChilds(SerializedProperty property, string[] upProperties, string[] bottomProperties)
        {
            if (!property.hasVisibleChildren)
            {
                return;
            }

            if (upProperties != null && upProperties.Length > 0)
            {
                for (int i = 0; i < upProperties.Length; i++)
                {
                    EditorGUILayout.PropertyField(property.FindPropertyRelative(upProperties[i]), true);
                }
            }

            foreach (SerializedProperty child in EditorHelper.GetChildren(property))
            {
                bool excludingProperty = false;

                if (upProperties != null && upProperties.Length > 0)
                {
                    for (int i = 0; i < upProperties.Length; i++)
                    {
                        if (child.name == upProperties[i])
                        {
                            excludingProperty = true;
                            break;
                        }
                    }
                }

                if (!excludingProperty && bottomProperties != null && bottomProperties.Length > 0)
                {
                    for (int i = 0; i < bottomProperties.Length; i++)
                    {
                        if (child.name == bottomProperties[i])
                        {
                            excludingProperty = true;
                            break;
                        }
                    }
                }

                if (!excludingProperty)
                {
                    EditorGUILayout.PropertyField(child, true);
                }
            }

            if (bottomProperties != null && bottomProperties.Length > 0)
            {
                for (int i = 0; i < bottomProperties.Length; i++)
                {

                    EditorGUILayout.PropertyField(property.FindPropertyRelative(bottomProperties[i]), true);
                }
            }
        }

        /// <summary>
        /// Make a field to receive any object type.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The object the field shows.</param>
        /// <param name="allowSceneObjects">Allow assigning Scene objects. See Description for more info.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <typeparam name="TObject">The type of the objects that can be assigned.</typeparam>
        /// <returns>The object that has been set by the user.</returns>
        public static TObject ObjectField<TObject>(GUIContent content, TObject value, bool allowSceneObjects, params GUILayoutOption[] options) where TObject : Object
        {
            return (TObject)EditorGUILayout.ObjectField(content, value, typeof(TObject), allowSceneObjects, options);
        }

        /// <summary>
        /// Make a field to receive any object type.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The object the field shows.</param>
        /// <param name="allowSceneObjects">Allow assigning Scene objects. See Description for more info.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <typeparam name="TObject">The type of the objects that can be assigned.</typeparam>
        /// <returns>The object that has been set by the user.</returns>
        public static TObject ObjectField<TObject>(string content, TObject value, bool allowSceneObjects, params GUILayoutOption[] options) where TObject : Object
        {
            return (TObject)EditorGUILayout.ObjectField(content, value, typeof(TObject), allowSceneObjects, options);
        }

        /// <summary>
        /// Make a field to receive any object type.
        /// </summary>
        /// <param name="value">The object the field shows.</param>
        /// <param name="allowSceneObjects">Allow assigning Scene objects. See Description for more info.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <typeparam name="TObject">The type of the objects that can be assigned.</typeparam>
        /// <returns>The object that has been set by the user.</returns>
        public static TObject ObjectField<TObject>(TObject value, bool allowSceneObjects, params GUILayoutOption[] options) where TObject : Object
        {
            return (TObject)EditorGUILayout.ObjectField(GUIContent.none, value, typeof(TObject), allowSceneObjects, options);
        }

        /// <summary>
        /// Make a field for layer masks.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="layerMask">The current mask to display.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The layer mask modified by the user.</returns>
        public static LayerMask LayerMaskField(GUIContent content, LayerMask layerMask, params GUILayoutOption[] options)
        {
            LayerMask mask = EditorGUILayout.MaskField(content, InternalEditorUtility.LayerMaskToConcatenatedLayersMask(layerMask), InternalEditorUtility.layers, options);
            return InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(mask);
        }

        /// <summary>
        /// Make a field for layer masks.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="layerMask">The current mask to display.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The layer mask modified by the user.</returns>
        public static LayerMask LayerMaskField(string content, LayerMask layerMask, params GUILayoutOption[] options)
        {
            LayerMask mask = EditorGUILayout.MaskField(content, InternalEditorUtility.LayerMaskToConcatenatedLayersMask(layerMask), InternalEditorUtility.layers, options);
            return InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(mask);
        }

        /// <summary>
        /// Make a field for layer masks.
        /// </summary>
        /// <param name="layerMask">The current mask to display.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The layer mask modified by the user.</returns>
        public static LayerMask LayerMaskField(LayerMask layerMask, params GUILayoutOption[] options)
        {
            LayerMask mask = EditorGUILayout.MaskField(InternalEditorUtility.LayerMaskToConcatenatedLayersMask(layerMask), InternalEditorUtility.layers, options);
            return InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(mask);
        }

        /// <summary>
        /// Make an enum popup selection field.
        /// </summary>
        /// <param name="selected">The enum option the field shows.</param>
        /// <param name="options">An optional list of layout options that specify extra layout properties. Any
        //     values passed in here will override settings defined by the style.<br> See Also:
        //     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        //     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <typeparam name="TEnum">Type of target enum.</typeparam>
        /// <returns>The enum option that has been selected by the user.</returns>
        public static TEnum EnumPopup<TEnum>(TEnum selected, params GUILayoutOption[] options) where TEnum : Enum
        {
            selected = (TEnum)EditorGUILayout.EnumPopup(selected, options);
            return selected;
        }

        /// <summary>
        /// Make an enum popup selection field.
        /// </summary>
        /// <param name="selected">The enum option the field shows.</param>
        /// <param name="style">Optional GUIStyle.</param>
        /// <param name="options">An optional list of layout options that specify extra layout properties. Any
        //     values passed in here will override settings defined by the style.<br> See Also:
        //     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        //     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <typeparam name="TEnum">Type of target enum.</typeparam>
        /// <returns>The enum option that has been selected by the user.</returns>
        public static TEnum EnumPopup<TEnum>(TEnum selected, GUIStyle style, params GUILayoutOption[] options) where TEnum : Enum
        {
            selected = (TEnum)EditorGUILayout.EnumPopup(selected, style, options);
            return selected;
        }

        /// <summary>
        /// Make an enum popup selection field.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="selected">The enum option the field shows.</param>
        /// <param name="options">An optional list of layout options that specify extra layout properties. Any
        //     values passed in here will override settings defined by the style.<br> See Also:
        //     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        //     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <typeparam name="TEnum">Type of target enum.</typeparam>
        /// <returns>The enum option that has been selected by the user.</returns>
        public static TEnum EnumPopup<TEnum>(string content, TEnum selected, params GUILayoutOption[] options) where TEnum : Enum
        {
            selected = (TEnum)EditorGUILayout.EnumPopup(content, selected, options);
            return selected;
        }

        /// <summary>
        /// Make an enum popup selection field.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="selected">The enum option the field shows.</param>
        /// <param name="style">Optional GUIStyle.</param>
        /// <param name="options">An optional list of layout options that specify extra layout properties. Any
        //     values passed in here will override settings defined by the style.<br> See Also:
        //     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        //     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <typeparam name="TEnum">Type of target enum.</typeparam>
        /// <returns>The enum option that has been selected by the user.</returns>
        public static TEnum EnumPopup<TEnum>(string content, TEnum selected, GUIStyle style, params GUILayoutOption[] options) where TEnum : Enum
        {
            selected = (TEnum)EditorGUILayout.EnumPopup(content, selected, style, options);
            return selected;
        }

        /// <summary>
        /// Make an enum popup selection field.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="selected">The enum option the field shows.</param>
        /// <param name="options">An optional list of layout options that specify extra layout properties. Any
        //     values passed in here will override settings defined by the style.<br> See Also:
        //     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        //     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <typeparam name="TEnum">Type of target enum.</typeparam>
        /// <returns>The enum option that has been selected by the user.</returns>
        public static TEnum EnumPopup<TEnum>(GUIContent content, TEnum selected, params GUILayoutOption[] options) where TEnum : Enum
        {
            selected = (TEnum)EditorGUILayout.EnumPopup(content, selected, options);
            return selected;
        }

        /// <summary>
        /// Make an enum popup selection field.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="selected">The enum option the field shows.</param>
        /// <param name="style">Optional GUIStyle.</param>
        /// <param name="options">An optional list of layout options that specify extra layout properties. Any
        //     values passed in here will override settings defined by the style.<br> See Also:
        //     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        //     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <typeparam name="TEnum">Type of target enum.</typeparam>
        /// <returns>The enum option that has been selected by the user.</returns>
        public static TEnum EnumPopup<TEnum>(GUIContent content, TEnum selected, GUIStyle style, params GUILayoutOption[] options) where TEnum : Enum
        {
            selected = (TEnum)EditorGUILayout.EnumPopup(content, selected, style, options);
            return selected;
        }

        /// <summary>
        /// Make an enum popup selection field.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="selected">The enum option the field shows.</param>
        /// <param name="checkEnabled">Set to true to include Enum values with ObsoleteAttribute. Set to false to exclude
        //     Enum values with ObsoleteAttribute.
        /// </param>
        /// <param name="includeObsolete">Method called for each Enum value displayed. The specified method should return
        //     true if the option can be selected, false otherwise.
        /// </param>
        /// <param name="options">An optional list of layout options that specify extra layout properties. Any
        //     values passed in here will override settings defined by the style.<br> See Also:
        //     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        //     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <typeparam name="TEnum">Type of target enum.</typeparam>
        /// <returns>The enum option that has been selected by the user.</returns>
        public static TEnum EnumPopup<TEnum>(GUIContent content, TEnum selected, Func<Enum, bool> checkEnabled, bool includeObsolete, params GUILayoutOption[] options) where TEnum : Enum
        {
            selected = (TEnum)EditorGUILayout.EnumPopup(content, selected, checkEnabled, includeObsolete, options);
            return selected;
        }

        /// <summary>
        /// Make an enum popup selection field.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="selected">The enum option the field shows.</param>
        /// <param name="checkEnabled">Set to true to include Enum values with ObsoleteAttribute. Set to false to exclude
        //     Enum values with ObsoleteAttribute.
        /// </param>
        /// <param name="includeObsolete">Method called for each Enum value displayed. The specified method should return
        //     true if the option can be selected, false otherwise.
        /// </param>
        /// <param name="style">Optional GUIStyle.</param>
        /// <param name="options">An optional list of layout options that specify extra layout properties. Any
        //     values passed in here will override settings defined by the style.<br> See Also:
        //     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        //     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <typeparam name="TEnum">Type of target enum.</typeparam>
        /// <returns>The enum option that has been selected by the user.</returns>
        public static TEnum EnumPopup<TEnum>(GUIContent content, TEnum selected, Func<Enum, bool> checkEnabled, bool includeObsolete, GUIStyle style, params GUILayoutOption[] options) where TEnum : Enum
        {
            selected = (TEnum)EditorGUILayout.EnumPopup(content, selected, checkEnabled, includeObsolete, style, options);
            return selected;
        }

        /// <summary>
        /// Make a required id text field with button to generate id.
        /// Displaying error icon when text is null or empty.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="text">The text to edit.</param>
        /// <param name="onGenerateID">Called when user generate id.</param>
        /// <returns>The text entered by the user.</returns>
        public static string IDTextField(GUIContent content, string text, Action<string> onGenerateID)
        {
            Rect idFieldPosition = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight);
            idFieldPosition.width -= 19;
            text = AEditorGUI.RequiredTextField(idFieldPosition, content, text);

            Rect generateButtonPosition = new Rect(idFieldPosition.x + idFieldPosition.width, idFieldPosition.y, 20, idFieldPosition.height);
            if (AEditorGUI.OptionsButton(generateButtonPosition))
            {
                ShowGenerateIDMenu(text, onGenerateID);
            }
            return text;
        }

        /// <summary>
        /// Make a required id text field with button to generate id.
        /// Displaying error icon when text is null or empty.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="text">The text to edit.</param>
        /// <param name="onGenerateID">Called when user generate id.</param>
        /// <returns>The text entered by the user.</returns>
        public static string IDTextField(string content, string text, Action<string> onGenerateID)
        {
            Rect idFieldPosition = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight);
            idFieldPosition.width -= 19;
            text = AEditorGUI.RequiredTextField(idFieldPosition, content, text);

            Rect generateButtonPosition = new Rect(idFieldPosition.x + idFieldPosition.width, idFieldPosition.y, 20, idFieldPosition.height);
            if (AEditorGUI.OptionsButton(generateButtonPosition))
            {
                ShowGenerateIDMenu(text, onGenerateID);
            }
            return text;
        }

        /// <summary>
        /// Show generic menu to generate id.
        /// </summary>
        /// <param name="text">Current id text value.</param>
        /// <param name="onGenerateID">Called when user generate id.</param>
        public static void ShowGenerateIDMenu(string text, Action<string> onGenerateID)
        {
            const string FocusTextInControl = "Instance ID Control";

            GenericMenu menu = new GenericMenu();
            GUIContent generateIDContent = new GUIContent();
            generateIDContent.text = text == "" ? "Generate ID" : "Regenerate ID";
            menu.AddItem(generateIDContent, false, () =>
            {
                text = AuroraExtension.GenerateID(32);
                onGenerateID?.Invoke(text);
                EditorGUI.FocusTextInControl(FocusTextInControl);
            });

            if (text != "" && text.Contains("-"))
            {
                menu.AddItem(new GUIContent("Remove dashes"), false, () =>
                {
                    text = text.Replace("-", "");
                    onGenerateID?.Invoke(text);
                    EditorGUI.FocusTextInControl(FocusTextInControl);
                });
            }
            else
            {
                menu.AddDisabledItem(new GUIContent("Remove dashes"));
            }

            if (text != "" && text.Length > 8)
            {
                menu.AddItem(new GUIContent("Trim to 8 letters"), false, () =>
                {
                    text = text.Substring(0, 8);
                    onGenerateID?.Invoke(text);
                    EditorGUI.FocusTextInControl(FocusTextInControl);
                });
            }
            else
            {
                menu.AddDisabledItem(new GUIContent("Trim to 8 letters"));
            }

            if (text != "" && text.Length > 16)
            {
                menu.AddItem(new GUIContent("Trim to 16 letters"), false, () =>
                {
                    text = text.Substring(0, 16);
                    onGenerateID?.Invoke(text);
                    EditorGUI.FocusTextInControl(FocusTextInControl);
                });
            }
            else
            {
                menu.AddDisabledItem(new GUIContent("Trim to 16 letters"));
            }

            if (text != "" && text.Length > 24)
            {
                menu.AddItem(new GUIContent("Trim to 24 letters"), false, () =>
                {
                    text = text.Substring(0, 24);
                    onGenerateID?.Invoke(text);
                    EditorGUI.FocusTextInControl(FocusTextInControl);
                });
            }
            else
            {
                menu.AddDisabledItem(new GUIContent("Trim to 24 letters"));
            }

            if (text != "")
            {
                menu.AddItem(new GUIContent("To uppercase"), false, () =>
                {
                    text = text.ToUpper();
                    onGenerateID?.Invoke(text);
                    EditorGUI.FocusTextInControl(FocusTextInControl);
                });
            }
            else
            {
                menu.AddDisabledItem(new GUIContent("To uppercase"));
            }

            if (text != "")
            {
                menu.AddItem(new GUIContent("To lowercase"), false, () =>
                {
                    text = text.ToLower();
                    onGenerateID?.Invoke(text);
                    EditorGUI.FocusTextInControl(FocusTextInControl);
                });
            }
            else
            {
                menu.AddDisabledItem(new GUIContent("To lowercase"));
            }

            if (text != "")
            {
                menu.AddItem(new GUIContent("Clear"), false, () =>
                {
                    text = "";
                    onGenerateID?.Invoke(text);
                    EditorGUI.FocusTextInControl(FocusTextInControl);
                });
            }
            else
            {
                menu.AddDisabledItem(new GUIContent("Clear"));
            }

            menu.ShowAsContext();
        }

        /// <summary>
        /// Make a text field for entering float value with disable option.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="switchValue">The value at which the field turns off.</param>
        /// <param name="switchText">The text that display on the off field.</param>
        /// <param name="enableClickCount">The required number of clicks in the disabled field to enable it again.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static float SwitchableFloatField(GUIContent content, float value, float switchValue, string switchText, int enableClickCount, params GUILayoutOption[] options)
        {
            if (value > switchValue)
            {
                value = FixedFloatField(content, value, switchValue, options);
            }
            else
            {
                Rect infiniteLabelPosition = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight);
                infiniteLabelPosition.y += 1.5f;

                EditorGUI.BeginDisabledGroup(true);
                EditorGUI.TextField(infiniteLabelPosition, content, switchText);
                EditorGUI.EndDisabledGroup();

                Event current = Event.current;
                if (current.clickCount == enableClickCount && infiniteLabelPosition.Contains(current.mousePosition))
                {
                    value = 0;
                }
            }
            return value;
        }

        /// <summary>
        /// Make a text field for entering float value with disable option.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="switchValue">The value at which the field turns off.</param>
        /// <param name="switchText">The text that display on the off field.</param>
        /// <param name="enableClickCount">The required number of clicks in the disabled field to enable it again.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static float SwitchableFloatField(string content, float value, float switchValue, string switchText, int enableClickCount, params GUILayoutOption[] options)
        {
            if (value > switchValue)
            {
                value = FixedFloatField(content, value, switchValue, options);
            }
            else
            {
                Rect infiniteLabelPosition = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight);
                infiniteLabelPosition.y += 1.5f;

                EditorGUI.BeginDisabledGroup(true);
                EditorGUI.TextField(infiniteLabelPosition, content, switchText);
                EditorGUI.EndDisabledGroup();

                Event current = Event.current;
                if (current.clickCount == enableClickCount && infiniteLabelPosition.Contains(current.mousePosition))
                {
                    value = 0;
                }
            }
            return value;
        }

        /// <summary>
        /// Make a text field for entering float value with disable option.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="switchValue">The value at which the field turns off.</param>
        /// <param name="switchText">The text that display on the off field.</param>
        /// <param name="enableClickCount">The required number of clicks in the disabled field to enable it again.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static float SwitchableFloatField(float value, float switchValue, string switchText, int enableClickCount, params GUILayoutOption[] options)
        {
            if (value > switchValue)
            {
                value = FixedFloatField(value, switchValue, options);
            }
            else
            {
                Rect infiniteLabelPosition = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight);
                infiniteLabelPosition.y += 1.5f;

                EditorGUI.BeginDisabledGroup(true);
                EditorGUI.TextField(infiniteLabelPosition, switchText);
                EditorGUI.EndDisabledGroup();

                Event current = Event.current;
                if (current.clickCount == enableClickCount && infiniteLabelPosition.Contains(current.mousePosition))
                {
                    value = 0;
                }
            }
            return value;
        }


        /// <summary>
        /// Make a text field for entering int value with disable option.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="switchValue">The value at which the field turns off.</param>
        /// <param name="switchText">The text that display on the off field.</param>
        /// <param name="enableClickCount">The required number of clicks in the disabled field to enable it again.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static int SwitchableFloatField(GUIContent content, int value, int switchValue, string switchText, int enableClickCount, params GUILayoutOption[] options)
        {
            if (value > switchValue)
            {
                value = FixedIntField(content, value, switchValue, options);
            }
            else
            {
                Rect infiniteLabelPosition = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight);
                infiniteLabelPosition.y += 1.5f;

                EditorGUI.BeginDisabledGroup(true);
                EditorGUI.TextField(infiniteLabelPosition, content, switchText);
                EditorGUI.EndDisabledGroup();

                Event current = Event.current;
                if (current.clickCount == enableClickCount && infiniteLabelPosition.Contains(current.mousePosition))
                {
                    value = 0;
                }
            }
            return value;
        }

        /// <summary>
        /// Make a text field for entering int value with disable option.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="switchValue">The value at which the field turns off.</param>
        /// <param name="switchText">The text that display on the off field.</param>
        /// <param name="enableClickCount">The required number of clicks in the disabled field to enable it again.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static int SwitchableFloatField(string content, int value, int switchValue, string switchText, int enableClickCount, params GUILayoutOption[] options)
        {
            if (value > switchValue)
            {
                value = FixedIntField(content, value, switchValue, options);
            }
            else
            {
                Rect infiniteLabelPosition = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight);
                infiniteLabelPosition.y += 1.5f;

                EditorGUI.BeginDisabledGroup(true);
                EditorGUI.TextField(infiniteLabelPosition, content, switchText);
                EditorGUI.EndDisabledGroup();

                Event current = Event.current;
                if (current.clickCount == enableClickCount && infiniteLabelPosition.Contains(current.mousePosition))
                {
                    value = 0;
                }
            }
            return value;
        }

        /// <summary>
        /// Make a text field for entering int value with disable option.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="switchValue">The value at which the field turns off.</param>
        /// <param name="switchText">The text that display on the off field.</param>
        /// <param name="enableClickCount">The required number of clicks in the disabled field to enable it again.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static int SwitchableFloatField(int value, int switchValue, string switchText, int enableClickCount, params GUILayoutOption[] options)
        {
            if (value > switchValue)
            {
                value = FixedIntField(value, switchValue, options);
            }
            else
            {
                Rect infiniteLabelPosition = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight);
                infiniteLabelPosition.y += 1.5f;

                EditorGUI.BeginDisabledGroup(true);
                EditorGUI.TextField(infiniteLabelPosition, switchText);
                EditorGUI.EndDisabledGroup();

                Event current = Event.current;
                if (current.clickCount == enableClickCount && infiniteLabelPosition.Contains(current.mousePosition))
                {
                    value = 0;
                }
            }
            return value;
        }

        /// <summary>
        /// Make a validate text field.
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
        public static string ValidateTextField(string text, bool error, bool warning, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            text = EditorGUILayout.TextField(text, options);
            if (error || warning)
            {
                GUILayout.Space(18);
                GUIContent iconContent = GUIContent.none;

                if (error)
                    iconContent = EditorGUIUtility.IconContent("CollabError");
                else if (warning)
                    iconContent = EditorGUIUtility.IconContent("console.warnicon.sml");

                Rect iconRect = GUILayoutUtility.GetLastRect();
                iconRect.width = 18;
                iconRect.height = 18;
                iconRect.x += 2.25f;
                iconRect.y += 1.5f;
                GUI.Label(iconRect, iconContent);
            }
            GUILayout.EndHorizontal();

            return text;
        }

        /// <summary>
        /// Make a required text field.
        /// Displaying error icon when text is null or empty.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="text">The text to edit.</param>
        /// <returns>The text entered by the user.</returns>
        public static string RequiredTextField(GUIContent content, string text)
        {
            Rect fieldPosition = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight);
            fieldPosition.height -= 1.5f;

            bool isValid = !string.IsNullOrEmpty(text);
            Rect textFieldPosition = new Rect(fieldPosition.x, fieldPosition.y, fieldPosition.width - (!isValid ? 19 : 0), fieldPosition.height);
            text = EditorGUI.TextField(textFieldPosition, content, text);

            if (!isValid)
            {
                GUIContent iconContent = EditorGUIUtility.IconContent("CollabError", "|Text is required!");
                Rect iconPosition = new Rect(textFieldPosition.x + textFieldPosition.width + 2, textFieldPosition.y, 30, textFieldPosition.height);
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
        /// <returns>The text entered by the user.</returns>
        public static string RequiredTextField(string content, string text)
        {
            Rect fieldPosition = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight);
            //fieldPosition.height -= 1.5f;

            bool isValid = !string.IsNullOrEmpty(text);
            Rect textFieldPosition = new Rect(fieldPosition.x + 2, fieldPosition.y, fieldPosition.width - (!isValid ? 19 : 0), fieldPosition.height);
            text = EditorGUI.TextField(textFieldPosition, content, text);

            if (!isValid)
            {
                GUIContent iconContent = EditorGUIUtility.IconContent("CollabError", "|Text is required!");
                Rect iconPosition = new Rect(textFieldPosition.x + textFieldPosition.width + 2, textFieldPosition.y, 30, textFieldPosition.height);
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
        public static string RequiredTextField(string text, params GUILayoutOption[] options)
        {
            Rect fieldPosition = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight);

            bool isValid = !string.IsNullOrEmpty(text);
            Rect textFieldPosition = new Rect(fieldPosition.x, fieldPosition.y, fieldPosition.width - (!isValid ? 19 : 0), fieldPosition.height);
            text = EditorGUI.TextField(textFieldPosition, text);

            if (!isValid)
            {
                GUIContent iconContent = EditorGUIUtility.IconContent("CollabError", "|Text is required!");
                Rect iconPosition = new Rect(textFieldPosition.x + textFieldPosition.width + 2, textFieldPosition.y, 30, textFieldPosition.height);
                GUI.Label(iconPosition, iconContent);
            }
            return text;
        }

        /// <summary>
        /// Make a required field to receive any object type.
        /// Displaying error icon when text is null or empty.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The object the field shows.</param>
        /// <param name="allowSceneObjects">Allow assigning Scene objects. See Description for more info.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <typeparam name="TObject">The type of the objects that can be assigned.</typeparam>
        /// <returns>The object that has been set by the user.</returns>
        public static TObject RequiredObjectField<TObject>(GUIContent content, TObject value, bool allowSceneObjects) where TObject : Object
        {
            Rect fieldPosition = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight + 3f);
            fieldPosition.height -= 3f;

            bool isValid = value != null;
            Rect objectFieldPosition = new Rect(fieldPosition.x, fieldPosition.y, fieldPosition.width - (!isValid ? 19 : 0), fieldPosition.height);
            value = AEditorGUI.ObjectField(objectFieldPosition, content, value, allowSceneObjects);

            if (!isValid)
            {
                GUIContent iconContent = EditorGUIUtility.IconContent("CollabError", "|Object is required!");
                Rect iconPosition = new Rect(objectFieldPosition.x + objectFieldPosition.width + 2, objectFieldPosition.y, 30, objectFieldPosition.height);
                GUI.Label(iconPosition, iconContent);
            }
            return value;
        }

        /// Make a required field to receive any object type.
        /// Displaying error icon when text is null or empty.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The object the field shows.</param>
        /// <param name="allowSceneObjects">Allow assigning Scene objects. See Description for more info.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <typeparam name="TObject">The type of the objects that can be assigned.</typeparam>
        /// <returns>The object that has been set by the user.</returns>
        public static TObject RequiredObjectField<TObject>(string content, TObject value, bool allowSceneObjects) where TObject : Object
        {
            Rect fieldPosition = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight);

            bool isValid = value != null;
            Rect objectFieldPosition = new Rect(fieldPosition.x, fieldPosition.y, fieldPosition.width - (!isValid ? 19 : 0), fieldPosition.height);
            value = AEditorGUI.ObjectField(objectFieldPosition, content, value, allowSceneObjects);

            if (!isValid)
            {
                GUIContent iconContent = EditorGUIUtility.IconContent("CollabError", "|Object is required!");
                Rect iconPosition = new Rect(objectFieldPosition.x + objectFieldPosition.width + 2, objectFieldPosition.y, 30, objectFieldPosition.height);
                GUI.Label(iconPosition, iconContent);
            }
            return value;
        }

        /// <summary>
        /// Make a required field to receive any object type.
        /// Displaying error icon when text is null or empty.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The object the field shows.</param>
        /// <param name="allowSceneObjects">Allow assigning Scene objects. See Description for more info.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <typeparam name="TObject">The type of the objects that can be assigned.</typeparam>
        /// <returns>The object that has been set by the user.</returns>
        public static TObject RequiredObjectField<TObject>(TObject value, bool allowSceneObjects, params GUILayoutOption[] options) where TObject : Object
        {
            Rect fieldPosition = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight);

            bool isValid = value != null;
            Rect objectFieldPosition = new Rect(fieldPosition.x, fieldPosition.y, fieldPosition.width - (!isValid ? 19 : 0), fieldPosition.height);
            value = AEditorGUI.ObjectField(objectFieldPosition, value, allowSceneObjects);

            if (!isValid)
            {
                GUIContent iconContent = EditorGUIUtility.IconContent("CollabError", "|Object is required!");
                Rect iconPosition = new Rect(objectFieldPosition.x + objectFieldPosition.width + 2, objectFieldPosition.y, 30, objectFieldPosition.height);
                GUI.Label(iconPosition, iconContent);
            }
            return value;
        }

        /// <summary>
        /// Make a text area.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="text">The text to edit.</param>
        /// <param name="options">An optional list of layout options that specify extra layout properties. Any
        //     values passed in here will override settings defined by the style.<br> See Also:
        //     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        //     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The text entered by the user.</returns>
        public static string TextArea(string content, string text, params GUILayoutOption[] options)
        {
            GUIContent guiContent = new GUIContent(content);
            GUILayout.BeginHorizontal();
            float width = GUI.skin.label.CalcSize(guiContent).x + (16 * EditorGUI.indentLevel);
            EditorGUILayout.LabelField(guiContent, GUILayout.MaxWidth(width));
#if UNITY_2019_3_OR_NEWER
            GUILayout.Space(EditorGUIUtility.labelWidth - width - 1 - (EditorGUI.indentLevel * 15));
#else
            GUILayout.Space(EditorGUIUtility.labelWidth - width - 4 - (EditorGUI.indentLevel * 15));
#endif
            text = EditorGUILayout.TextArea(text, options);
            GUILayout.EndHorizontal();
            return text;
        }

        /// <summary>
        /// Make a text area.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="text">The text to edit.</param>
        /// <param name="options">An optional list of layout options that specify extra layout properties. Any
        //     values passed in here will override settings defined by the style.<br> See Also:
        //     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        //     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The text entered by the user.</returns>
        public static string TextArea(GUIContent content, string text, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            float width = GUI.skin.label.CalcSize(content).x + (16 * EditorGUI.indentLevel);
            EditorGUILayout.LabelField(content, GUILayout.MaxWidth(width));
#if UNITY_2019_3_OR_NEWER
            GUILayout.Space(EditorGUIUtility.labelWidth - width - 1 - (EditorGUI.indentLevel * 15));
#else
            GUILayout.Space(EditorGUIUtility.labelWidth - width - 4 - (EditorGUI.indentLevel * 15));
#endif
            text = EditorGUILayout.TextArea(text, options);
            GUILayout.EndHorizontal();
            return text;
        }

        /// <summary>
        /// Make a text area.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="text">The text to edit.</param>
        /// <param name="style"> Optional GUIStyle.</param>
        /// <param name="options">An optional list of layout options that specify extra layout properties. Any
        //     values passed in here will override settings defined by the style.<br> See Also:
        //     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        //     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The text entered by the user.</returns>
        public static string TextArea(string content, string text, GUIStyle style, params GUILayoutOption[] options)
        {
            GUIContent guiContent = new GUIContent(content);
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(guiContent, GUILayout.MaxWidth(GUI.skin.label.CalcSize(guiContent).x + (16 * EditorGUI.indentLevel)));
            GUILayout.Space(49 - (31 * EditorGUI.indentLevel));
            text = EditorGUILayout.TextArea(text, style, options);
            GUILayout.EndHorizontal();
            return text;
        }

        /// <summary>
        /// Make a text area.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="text">The text to edit.</param>
        /// <param name="style"> Optional GUIStyle.</param>
        /// <param name="options">An optional list of layout options that specify extra layout properties. Any
        //     values passed in here will override settings defined by the style.<br> See Also:
        //     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        //     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The text entered by the user.</returns>
        public static string TextArea(GUIContent content, string text, GUIStyle style, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(content, GUILayout.MaxWidth(GUI.skin.label.CalcSize(content).x + (16 * EditorGUI.indentLevel)));
            GUILayout.Space(49 - (31 * EditorGUI.indentLevel));
            text = EditorGUILayout.TextArea(text, style, options);
            GUILayout.EndHorizontal();
            return text;
        }

        /// <summary>
        /// Make a int field for entering fixed integers with the lowest possible value.
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
        public static int FixedIntField(GUIContent content, int value, int minValue, params GUILayoutOption[] options)
        {
            value = EditorGUILayout.IntField(content, value, options);
            if (value < minValue) value = minValue;
            return value;
        }

        /// <summary>
        /// Make a int field for entering fixed integers with the lowest possible value.
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
        public static int FixedIntField(string content, int value, int minValue, params GUILayoutOption[] options)
        {
            value = EditorGUILayout.IntField(content, value, options);
            if (value < minValue) value = minValue;
            return value;
        }

        /// <summary>
        /// Make a int field for entering fixed integers with the lowest possible value.
        /// </summary>
        /// <param name="value">The value to edit.</param>
        /// <param name="minValue">The lowest possible value.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static int FixedIntField(int value, int minValue, params GUILayoutOption[] options)
        {
            value = EditorGUILayout.IntField(value, options);
            if (value < minValue) value = minValue;
            return value;
        }

        /// <summary>
        /// Make a delayed int field for entering fixed integers with the lowest possible value.
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
        public static int DelayedFixedIntField(GUIContent content, int value, int minValue, params GUILayoutOption[] options)
        {
            value = EditorGUILayout.DelayedIntField(content, value, options);
            if (value < minValue) value = minValue;
            return value;
        }

        /// <summary>
        /// Make a delayed int field for entering fixed integers with the lowest possible value.
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
        public static int DelayedFixedIntField(string content, int value, int minValue, params GUILayoutOption[] options)
        {
            value = EditorGUILayout.DelayedIntField(content, value, options);
            if (value < minValue) value = minValue;
            return value;
        }

        /// <summary>
        /// Make a delayed int field for entering fixed integers with the lowest possible value.
        /// </summary>
        /// <param name="value">The value to edit.</param>
        /// <param name="minValue">The lowest possible value.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static int DelayedFixedIntField(int value, int minValue, params GUILayoutOption[] options)
        {
            value = EditorGUILayout.DelayedIntField(value, options);
            if (value < minValue) value = minValue;
            return value;
        }

        /// <summary>
        /// Make a delayed float field for entering fixed integers with the lowest possible value.
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
        public static float FixedFloatField(GUIContent content, float value, float minValue, params GUILayoutOption[] options)
        {
            value = EditorGUILayout.FloatField(content, value, options);
            if (value < minValue) value = minValue;
            return value;
        }

        /// <summary>
        /// Make a delayed float field for entering fixed integers with the lowest possible value.
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
        public static float FixedFloatField(string content, float value, float minValue, params GUILayoutOption[] options)
        {
            value = EditorGUILayout.FloatField(content, value, options);
            if (value < minValue) value = minValue;
            return value;
        }

        /// <summary>
        /// Make a delayed float field for entering fixed integers with the lowest possible value.
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
        public static float FixedFloatField(float value, float minValue, params GUILayoutOption[] options)
        {
            value = EditorGUILayout.FloatField(value, options);
            if (value < minValue) value = minValue;
            return value;
        }

        /// <summary>
        /// Make a delayed float field for entering fixed integers with the lowest possible value.
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
        public static float DelayedFixedFloatField(GUIContent content, float value, float minValue, params GUILayoutOption[] options)
        {
            value = EditorGUILayout.DelayedFloatField(content, value, options);
            if (value < minValue) value = minValue;
            return value;
        }

        /// <summary>
        /// Make a delayed float field for entering fixed integers with the lowest possible value.
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
        public static float DelayedFixedFloatField(string content, float value, float minValue, params GUILayoutOption[] options)
        {
            value = EditorGUILayout.DelayedFloatField(content, value, options);
            if (value < minValue) value = minValue;
            return value;
        }

        /// <summary>
        /// Make a delayed float field for entering fixed integers with the lowest possible value.
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
        public static float DelayedFixedFloatField(float value, float minValue, params GUILayoutOption[] options)
        {
            value = EditorGUILayout.DelayedFloatField(value, options);
            if (value < minValue) value = minValue;
            return value;
        }

        /// <summary>
        /// Make a slider with visual clamped value from 0 to 1.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="minValue">The lowest possible value.</param>
        /// <param name="maxValue">The maximum possible value.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static float VisualClamp(GUIContent content, float value, float minValue, float maxValue, params GUILayoutOption[] options)
        {
            return Mathf.Lerp(minValue, maxValue, EditorGUILayout.Slider(content, Mathf.InverseLerp(minValue, maxValue, value), 0, 1, options));
        }

        /// <summary>
        /// Make a slider with visual clamped value from 0 to 1.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="minValue">The lowest possible value.</param>
        /// <param name="maxValue">The maximum possible value.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static float VisualClamp(string content, float value, float minValue, float maxValue, params GUILayoutOption[] options)
        {
            return Mathf.Lerp(minValue, maxValue, EditorGUILayout.Slider(content, Mathf.InverseLerp(minValue, maxValue, value), 0, 1, options));
        }

        /// <summary>
        /// Make a slider with visual clamped value from 0 to 1.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="minValue">The lowest possible value.</param>
        /// <param name="maxValue">The maximum possible value.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static float VisualClamp(float value, float minValue, float maxValue, params GUILayoutOption[] options)
        {
            return Mathf.Lerp(minValue, maxValue, EditorGUILayout.Slider(Mathf.InverseLerp(minValue, maxValue, value), 0, 1, options));
        }

        /// <summary>
        /// Make an X, Y & Z field for entering a Vector3 with read/write button to read value from instance or set value to instance.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="instance">Transform instance to read/write.</param>
        /// <param name="vectorType">Type of vector to read/write.
        ///  Position, Rotation, Scale.
        /// </param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static Vector3 ReadWriteVector3Field(GUIContent content, Vector3 value, Transform instance, VectorType vectorType, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.Vector3Field(content, value, options);
            if (GUILayout.Button(ContentProperties.ReadButton, "ButtonLeft", ReadWriteButtonOptions))
            {
                switch (vectorType)
                {
                    case VectorType.Position:
                        value = instance.localPosition;
                        break;
                    case VectorType.Rotation:
                        value = instance.localEulerAngles;
                        break;
                    case VectorType.Scale:
                        value = instance.localScale;
                        break;
                }
            }
            if (GUILayout.Button(ContentProperties.WriteButton, "ButtonRight", ReadWriteButtonOptions))
            {
                switch (vectorType)
                {
                    case VectorType.Position:
                        instance.localPosition = value;
                        break;
                    case VectorType.Rotation:
                        instance.localEulerAngles = value;
                        break;
                    case VectorType.Scale:
                        instance.localScale = value;
                        break;
                }
            }
            GUILayout.EndHorizontal();
            return value;
        }

        /// <summary>
        /// Make an X, Y & Z field for entering a Vector3 with read/write button to read value from instance or set value to instance.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="instance">Transform instance to read/write.</param>
        /// <param name="vectorType">Type of vector to read/write.
        ///  Position, Rotation, Scale.
        /// </param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static Vector3 ReadWriteVector3Field(string content, Vector3 value, Transform instance, VectorType vectorType, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.Vector3Field(content, value, options);
            if (GUILayout.Button(ContentProperties.ReadButton, "ButtonLeft", ReadWriteButtonOptions))
            {
                switch (vectorType)
                {
                    case VectorType.Position:
                        value = instance.localPosition;
                        break;
                    case VectorType.Rotation:
                        value = instance.localEulerAngles;
                        break;
                    case VectorType.Scale:
                        value = instance.localScale;
                        break;
                }
            }
            if (GUILayout.Button(ContentProperties.WriteButton, "ButtonRight", ReadWriteButtonOptions))
            {
                switch (vectorType)
                {
                    case VectorType.Position:
                        instance.localPosition = value;
                        break;
                    case VectorType.Rotation:
                        instance.localEulerAngles = value;
                        break;
                    case VectorType.Scale:
                        instance.localScale = value;
                        break;
                }
            }
            GUILayout.EndHorizontal();
            return value;
        }

        /// <summary>
        /// Make an X, Y & Z field for entering a Vector3 with read/write button to read value from instance or set value to instance.
        /// </summary>
        /// <param name="value">The value to edit.</param>
        /// <param name="instance">Transform instance to read/write.</param>
        /// <param name="vectorType">Type of vector to read/write.
        ///  Position, Rotation, Scale.
        /// </param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static Vector3 ReadWriteVector3Field(Vector3 value, Transform instance, VectorType vectorType, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.Vector3Field(GUIContent.none, value, options);
            if (GUILayout.Button(ContentProperties.ReadButton, "ButtonLeft", ReadWriteButtonOptions))
            {
                switch (vectorType)
                {
                    case VectorType.Position:
                        value = instance.localPosition;
                        break;
                    case VectorType.Rotation:
                        value = instance.localEulerAngles;
                        break;
                    case VectorType.Scale:
                        value = instance.localScale;
                        break;
                }
            }
            if (GUILayout.Button(ContentProperties.WriteButton, "ButtonRight", ReadWriteButtonOptions))
            {
                switch (vectorType)
                {
                    case VectorType.Position:
                        instance.localPosition = value;
                        break;
                    case VectorType.Rotation:
                        instance.localEulerAngles = value;
                        break;
                    case VectorType.Scale:
                        instance.localScale = value;
                        break;
                }
            }
            GUILayout.EndHorizontal();
            return value;
        }

        /// <summary>
        /// Make a text field for entering float values with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static float ActionFloatField(GUIContent content, float value, Action action, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.FloatField(content, value);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field for entering float values with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static float ActionFloatField(string content, float value, Action action, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.FloatField(content, value);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field for entering float values with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static float ActionFloatField(GUIContent content, float value, Action<object> action, object actionData, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.FloatField(content, value);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field for entering float values with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static float ActionFloatField(string content, float value, Action<object> action, object actionData, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.FloatField(content, value);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field for entering float values with right action button.
        /// </summary>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static float ActionFloatField(float value, Action action, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.FloatField(value);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field for entering float values with right action button.
        /// </summary>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static float ActionFloatField(float value, Action<object> action, object actionData, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.FloatField(value);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static string ActionTextField(GUIContent content, string value, Action action, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.TextField(content, value, options);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static string ActionTextField(string content, string value, Action action, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.TextField(content, value, options);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static string ActionTextField(GUIContent content, string value, Action<object> action, object actionData, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.TextField(content, value, options);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static string ActionTextField(string content, string value, Action<object> action, object actionData, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.TextField(content, value, options);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static string ActionTextField(string value, Action action, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.TextField(value, options);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static string ActionTextField(string value, Action<object> action, object actionData, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.TextField(value, options);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field for entering integer values with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static int ActionIntField(GUIContent content, int value, Action action, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.IntField(content, value, options);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field for entering integer values with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static int ActionIntField(string content, int value, Action action, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.IntField(content, value, options);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field for entering integer values with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static int ActionIntField(GUIContent content, int value, Action<object> action, object actionData, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.IntField(content, value, options);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field for entering integer values with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static int ActionIntField(string content, int value, Action<object> action, object actionData, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.IntField(content, value, options);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field for entering integer values with right action button.
        /// </summary>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static int ActionIntField(int value, Action action, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.IntField(value, options);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field for entering integer values with right action button.
        /// </summary>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static int ActionIntField(int value, Action<object> action, object actionData, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.IntField(value, options);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field for entering double values with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static double ActionDoubleField(GUIContent content, double value, Action action, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.DoubleField(content, value, options);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field for entering double values with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static double ActionDoubleField(string content, double value, Action action, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.DoubleField(content, value, options);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field for entering double values with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static double ActionDoubleField(GUIContent content, double value, Action<object> action, object actionData, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.DoubleField(content, value, options);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field for entering double values with right action button.
        /// </summary>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static double ActionDoubleField(double value, Action action, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.DoubleField(value, options);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field for entering double values with right action button.
        /// </summary>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static double ActionDoubleField(double value, Action<object> action, object actionData, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.DoubleField(value, options);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field for entering double values with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static double ActionDoubleField(string content, double value, Action<object> action, object actionData, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.DoubleField(content, value, options);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a field to receive any object type with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static TObject ActionObjectField<TObject>(GUIContent content, TObject value, bool allowSceneObjects, Action action, params GUILayoutOption[] options) where TObject : Object
        {
            GUILayout.BeginHorizontal();
            value = ObjectField(content, value, allowSceneObjects, options);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a field to receive any object type with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static TObject ActionObjectField<TObject>(string content, TObject value, bool allowSceneObjects, Action action, params GUILayoutOption[] options) where TObject : Object
        {
            GUILayout.BeginHorizontal();
            value = ObjectField(content, value, allowSceneObjects, options);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a field to receive any object type with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static TObject ActionObjectField<TObject>(GUIContent content, TObject value, bool allowSceneObjects, Action<object> action, object actionData, params GUILayoutOption[] options) where TObject : Object
        {
            GUILayout.BeginHorizontal();
            value = ObjectField(content, value, allowSceneObjects, options);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a field to receive any object type with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static TObject ActionObjectField<TObject>(string content, TObject value, bool allowSceneObjects, Action<object> action, object actionData, params GUILayoutOption[] options) where TObject : Object
        {
            GUILayout.BeginHorizontal();
            value = ObjectField(content, value, allowSceneObjects, options);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a field to receive any object type with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static TObject ActionObjectField<TObject>(TObject value, bool allowSceneObjects, Action action, params GUILayoutOption[] options) where TObject : Object
        {
            GUILayout.BeginHorizontal();
            value = ObjectField(value, allowSceneObjects, options);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a field to receive any object type with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static TObject ActionObjectField<TObject>(TObject value, bool allowSceneObjects, Action<object> action, object actionData, params GUILayoutOption[] options) where TObject : Object
        {
            GUILayout.BeginHorizontal();
            value = ObjectField(value, allowSceneObjects, options);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a field for selecting a Color with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static Color ActionColorField(GUIContent content, Color value, Action action, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.ColorField(content, value, options);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a field for selecting a Color with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static Color ActionColorField(string content, Color value, Action action, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.ColorField(content, value, options);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a field for selecting a Color with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static Color ActionColorField(GUIContent content, Color value, Action<object> action, object actionData, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.ColorField(content, value, options);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a field for selecting a Color with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static Color ActionColorField(string content, Color value, Action<object> action, object actionData, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.ColorField(content, value, options);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a field for selecting a Color with right action button.
        /// </summary>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static Color ActionColorField(Color value, Action action, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.ColorField(value, options);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a field for selecting a Color with right action button.
        /// </summary>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static Color ActionColorField(Color value, Action<object> action, object actionData, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.ColorField(value, options);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a field for editing an AnimationCurve with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static AnimationCurve ActionCurveField(GUIContent content, AnimationCurve value, Action action, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.CurveField(content, value, options);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a field for editing an AnimationCurve with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static AnimationCurve ActionCurveField(string content, AnimationCurve value, Action action, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.CurveField(content, value, options);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a field for editing an AnimationCurve with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static AnimationCurve ActionCurveField(GUIContent content, AnimationCurve value, Action<object> action, object actionData, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.CurveField(content, value, options);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a field for editing an AnimationCurve with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static AnimationCurve ActionCurveField(string content, AnimationCurve value, Action<object> action, object actionData, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.CurveField(content, value, options);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a field for editing an AnimationCurve with right action button.
        /// </summary>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static AnimationCurve ActionCurveField(AnimationCurve value, Action action, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.CurveField(value, options);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a field for editing an AnimationCurve with right action button.
        /// </summary>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static AnimationCurve ActionCurveField(AnimationCurve value, Action<object> action, object actionData, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.CurveField(value, options);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field for entering float values with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static float ActionFloatField(GUIContent content, float value, Action action, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.FloatField(content, value);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field for entering float values with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static float ActionFloatField(string content, float value, Action action, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.FloatField(content, value);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field for entering float values with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static float ActionFloatField(GUIContent content, float value, Action<object> action, object actionData, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.FloatField(content, value);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field for entering float values with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static float ActionFloatField(string content, float value, Action<object> action, object actionData, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.FloatField(content, value);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field for entering float values with right action button.
        /// </summary>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static float ActionFloatField(float value, Action action, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.FloatField(value);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field for entering float values with right action button.
        /// </summary>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static float ActionFloatField(float value, Action<object> action, object actionData, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.FloatField(value);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static string ActionTextField(GUIContent content, string value, Action action, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.TextField(content, value, options);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static string ActionTextField(string content, string value, Action action, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.TextField(content, value, options);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static string ActionTextField(GUIContent content, string value, Action<object> action, object actionData, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.TextField(content, value, options);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static string ActionTextField(string content, string value, Action<object> action, object actionData, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.TextField(content, value, options);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static string ActionTextField(string value, Action action, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.TextField(value, options);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static string ActionTextField(string value, Action<object> action, object actionData, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.TextField(value, options);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field for entering integer values with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static int ActionIntField(GUIContent content, int value, Action action, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.IntField(content, value, options);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field for entering integer values with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static int ActionIntField(string content, int value, Action action, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.IntField(content, value, options);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field for entering integer values with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static int ActionIntField(GUIContent content, int value, Action<object> action, object actionData, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.IntField(content, value, options);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field for entering integer values with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static int ActionIntField(string content, int value, Action<object> action, object actionData, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.IntField(content, value, options);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field for entering integer values with right action button.
        /// </summary>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static int ActionIntField(int value, Action action, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.IntField(value, options);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field for entering integer values with right action button.
        /// </summary>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static int ActionIntField(int value, Action<object> action, object actionData, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.IntField(value, options);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field for entering double values with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static double ActionDoubleField(GUIContent content, double value, Action action, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.DoubleField(content, value, options);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field for entering double values with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static double ActionDoubleField(string content, double value, Action action, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.DoubleField(content, value, options);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field for entering double values with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static double ActionDoubleField(GUIContent content, double value, Action<object> action, object actionData, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.DoubleField(content, value, options);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field for entering double values with right action button.
        /// </summary>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static double ActionDoubleField(double value, Action action, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.DoubleField(value, options);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field for entering double values with right action button.
        /// </summary>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static double ActionDoubleField(double value, Action<object> action, object actionData, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.DoubleField(value, options);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a text field for entering double values with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static double ActionDoubleField(string content, double value, Action<object> action, object actionData, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.DoubleField(content, value, options);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a field to receive any object type with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static TObject ActionObjectField<TObject>(GUIContent content, TObject value, bool allowSceneObjects, Action action, string actionButtonTooltip = "", params GUILayoutOption[] options) where TObject : Object
        {
            GUILayout.BeginHorizontal();
            value = ObjectField(content, value, allowSceneObjects, options);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a field to receive any object type with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static TObject ActionObjectField<TObject>(string content, TObject value, bool allowSceneObjects, Action action, string actionButtonTooltip = "", params GUILayoutOption[] options) where TObject : Object
        {
            GUILayout.BeginHorizontal();
            value = ObjectField(content, value, allowSceneObjects, options);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a field to receive any object type with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static TObject ActionObjectField<TObject>(GUIContent content, TObject value, bool allowSceneObjects, Action<object> action, object actionData, string actionButtonTooltip = "", params GUILayoutOption[] options) where TObject : Object
        {
            GUILayout.BeginHorizontal();
            value = ObjectField(content, value, allowSceneObjects, options);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a field to receive any object type with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static TObject ActionObjectField<TObject>(string content, TObject value, bool allowSceneObjects, Action<object> action, object actionData, string actionButtonTooltip = "", params GUILayoutOption[] options) where TObject : Object
        {
            GUILayout.BeginHorizontal();
            value = ObjectField(content, value, allowSceneObjects, options);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a field to receive any object type with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static TObject ActionObjectField<TObject>(TObject value, bool allowSceneObjects, Action action, string actionButtonTooltip = "", params GUILayoutOption[] options) where TObject : Object
        {
            GUILayout.BeginHorizontal();
            value = ObjectField(value, allowSceneObjects, options);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a field to receive any object type with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static TObject ActionObjectField<TObject>(TObject value, bool allowSceneObjects, Action<object> action, object actionData, string actionButtonTooltip = "", params GUILayoutOption[] options) where TObject : Object
        {
            GUILayout.BeginHorizontal();
            value = ObjectField(value, allowSceneObjects, options);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a field for selecting a Color with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static Color ActionColorField(GUIContent content, Color value, Action action, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.ColorField(content, value, options);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a field for selecting a Color with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static Color ActionColorField(string content, Color value, Action action, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.ColorField(content, value, options);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a field for selecting a Color with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static Color ActionColorField(GUIContent content, Color value, Action<object> action, object actionData, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.ColorField(content, value, options);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a field for selecting a Color with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static Color ActionColorField(string content, Color value, Action<object> action, object actionData, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.ColorField(content, value, options);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a field for selecting a Color with right action button.
        /// </summary>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static Color ActionColorField(Color value, Action action, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.ColorField(value, options);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a field for selecting a Color with right action button.
        /// </summary>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static Color ActionColorField(Color value, Action<object> action, object actionData, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.ColorField(value, options);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a field for editing an AnimationCurve with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static AnimationCurve ActionCurveField(GUIContent content, AnimationCurve value, Action action, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.CurveField(content, value, options);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a field for editing an AnimationCurve with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static AnimationCurve ActionCurveField(string content, AnimationCurve value, Action action, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.CurveField(content, value, options);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a field for editing an AnimationCurve with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static AnimationCurve ActionCurveField(GUIContent content, AnimationCurve value, Action<object> action, object actionData, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.CurveField(content, value, options);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a field for editing an AnimationCurve with right action button.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static AnimationCurve ActionCurveField(string content, AnimationCurve value, Action<object> action, object actionData, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.CurveField(content, value, options);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a field for editing an AnimationCurve with right action button.
        /// </summary>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static AnimationCurve ActionCurveField(AnimationCurve value, Action action, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.CurveField(value, options);

            if (OptionsButton())
            {
                action?.Invoke();
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a field for editing an AnimationCurve with right action button.
        /// </summary>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>The value entered by the user.</returns>
        public static AnimationCurve ActionCurveField(AnimationCurve value, Action<object> action, object actionData, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.CurveField(value, options);

            if (OptionsButton())
            {
                action?.Invoke(actionData);
            }
            GUILayout.EndHorizontal();

            return value;
        }

        /// <summary>
        /// Make a min/max slider field.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="min">References of min value.</param>
        /// <param name="max">References of max value.</param>
        /// <param name="minLimit">Lowest possible value for min references value.</param>
        /// <param name="maxLimit">Max possible value for max references value.</param>
        /// <param name="spaceBeforeField">Space between left side and field.</param>
        /// <param name="spaceBetweenField">Space between label and field.</param>
        public static void MinMaxSlider(GUIContent content, ref float min, ref float max, float minLimit, float maxLimit)
        {
            Rect position = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight);

            Rect controlRect = EditorGUI.PrefixLabel(position, content);
            Rect[] splittedRect = AEditorGUI.SplitRect(controlRect, 3);

            min = EditorGUI.DelayedFloatField(splittedRect[0], AMath.AllocatePart(min));
            max = EditorGUI.DelayedFloatField(splittedRect[2], AMath.AllocatePart(max));

            EditorGUI.MinMaxSlider(splittedRect[1], ref min, ref max, minLimit, maxLimit);

            if (min < minLimit)
                min = minLimit;
            else if (min > max)
                min = max;

            if (max > maxLimit)
                max = maxLimit;
            else if (max < min)
                max = min;
        }

        /// <summary>
        /// Make a min/max slider field.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="min">References of min value.</param>
        /// <param name="max">References of max value.</param>
        /// <param name="minLimit">Lowest possible value for min references value.</param>
        /// <param name="maxLimit">Max possible value for max references value.</param>
        /// <param name="spaceBeforeField">Space between left side and field.</param>
        /// <param name="spaceBetweenField">Space between label and field.</param>
        public static void MinMaxSlider(string content, ref float min, ref float max, float minLimit, float maxLimit)
        {
            Rect position = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight);

            GUIContent guiContent = new GUIContent(content);
            Rect controlRect = EditorGUI.PrefixLabel(position, guiContent);
            Rect[] splittedRect = AEditorGUI.SplitRect(controlRect, 3);

            min = EditorGUI.DelayedFloatField(splittedRect[0], AMath.AllocatePart(min));
            max = EditorGUI.DelayedFloatField(splittedRect[2], AMath.AllocatePart(max));

            EditorGUI.MinMaxSlider(splittedRect[1], ref min, ref max, minLimit, maxLimit);

            if (min < minLimit)
                min = minLimit;
            else if (min > max)
                min = max;

            if (max > maxLimit)
                max = maxLimit;
            else if (max < min)
                max = min;
        }

        /// <summary>
        /// Make a min/max slider field.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="min">References of min value.</param>
        /// <param name="max">References of max value.</param>
        /// <param name="minLimit">Lowest possible value for min references value.</param>
        /// <param name="maxLimit">Max possible value for max references value.</param>
        /// <param name="spaceBeforeField">Space between left side and field.</param>
        /// <param name="spaceBetweenField">Space between label and field.</param>
        public static RangedFloat MinMaxSlider(GUIContent content, RangedFloat value)
        {
            Rect position = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight);

            Rect controlRect = EditorGUI.PrefixLabel(position, content);
            Rect[] splittedRect = AEditorGUI.SplitRect(controlRect, 3);

            float min = value.GetMin();
            float max = value.GetMax();

            EditorGUI.MinMaxSlider(splittedRect[1], ref min, ref max, value.GetMinLimit(), value.GetMaxLimit());


            if (min < value.GetMinLimit())
                value.SetMin(value.GetMinLimit());
            else if (min > max)
                value.SetMin(max);

            if (max > value.GetMaxLimit())
                value.SetMax(value.GetMaxLimit());
            else if (max < min)
                value.SetMax(min);

            value.SetMin(EditorGUI.FloatField(splittedRect[0], AMath.AllocatePart(min)));
            value.SetMax(EditorGUI.FloatField(splittedRect[2], AMath.AllocatePart(max)));
            
            return value;
        }

        /// <summary>
        /// Make a min/max slider field.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="min">References of min value.</param>
        /// <param name="max">References of max value.</param>
        /// <param name="minLimit">Lowest possible value for min references value.</param>
        /// <param name="maxLimit">Max possible value for max references value.</param>
        /// <param name="spaceBeforeField">Space between left side and field.</param>
        /// <param name="spaceBetweenField">Space between label and field.</param>
        public static RangedFloat MinMaxSlider(string content, RangedFloat value)
        {
            Rect position = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight + 1);
            position.height -= 1;

            GUIContent guiContent = new GUIContent(content);
            Rect controlRect = EditorGUI.PrefixLabel(position, guiContent);
            Rect[] splittedRect = AEditorGUI.SplitRect(controlRect, 3);

            float min = value.GetMin();
            float max = value.GetMax();

            EditorGUI.MinMaxSlider(splittedRect[1], ref min, ref max, value.GetMinLimit(), value.GetMaxLimit());

            if (min < value.GetMinLimit())
                value.SetMin(value.GetMinLimit());
            else if (min > max)
                value.SetMin(max);

            if (max > value.GetMaxLimit())
                value.SetMax(value.GetMaxLimit());
            else if (max < min)
                value.SetMax(min);

            value.SetMin(EditorGUI.FloatField(splittedRect[0], AMath.AllocatePart(min)));
            value.SetMax(EditorGUI.FloatField(splittedRect[2], AMath.AllocatePart(max)));

            return value;
        }

        /// <summary>
        /// Group two transform references value.
        /// </summary>
        /// <param name="leftValue">First/Left transform reference.</param>
        /// <param name="rightValue">Second/Right transform reference.</param>
        /// <param name="leftValueContent">Label to display for first/left transform value.</param>
        /// <param name="rightValueContent">Label to display for second/right transform value.</param>
        public static void TransformGroupField(ref Transform leftValue, ref Transform rightValue, GUIContent leftValueContent, GUIContent rightValueContent)
        {
            GUILayout.Space(3);
            Rect leftToggleHeaderGroupRect = GUILayoutUtility.GetRect(0, 20);
            leftToggleHeaderGroupRect.height = 20;
            leftToggleHeaderGroupRect.width = leftToggleHeaderGroupRect.width / 2;
            GUI.Label(leftToggleHeaderGroupRect, GUIContent.none, GUI.skin.GetStyle("Tooltip"));
            GUI.Label(leftToggleHeaderGroupRect, leftValueContent, AEditorStyles.CenteredLabel);

            Rect rightToggleHeaderGroupRect = leftToggleHeaderGroupRect;
            rightToggleHeaderGroupRect.x = rightToggleHeaderGroupRect.width + 28;
            GUI.Label(rightToggleHeaderGroupRect, GUIContent.none, GUI.skin.GetStyle("Tooltip"));
            GUI.Label(rightToggleHeaderGroupRect, rightValueContent, AEditorStyles.CenteredLabel);

            Rect leftToggleValueGroupRect = GUILayoutUtility.GetRect(0, 25);
            leftToggleValueGroupRect.y -= 1;
            leftToggleValueGroupRect.height = 25;
            leftToggleValueGroupRect.width = leftToggleValueGroupRect.width / 2;
            GUI.Label(leftToggleValueGroupRect, GUIContent.none, GUI.skin.GetStyle("Tooltip"));

            Rect leftToggleValueRect = leftToggleValueGroupRect;
            leftToggleValueRect.width -= 12;
            leftToggleValueRect.height = 15;
            leftToggleValueRect.x += 7;
            leftToggleValueRect.y += 4;
            leftValue = AEditorGUI.ObjectField<Transform>(leftToggleValueRect, GUIContent.none, leftValue, true);

            Rect rightToggleValueGroupRect = leftToggleValueGroupRect;
            rightToggleValueGroupRect.x = rightToggleValueGroupRect.width + 28;
            GUI.Label(rightToggleValueGroupRect, GUIContent.none, GUI.skin.GetStyle("Tooltip"));

            Rect rightToggleValueRect = rightToggleValueGroupRect;
            rightToggleValueRect.width -= 12;
            rightToggleValueRect.height = 15;
            rightToggleValueRect.x += 7;
            rightToggleValueRect.y += 4;
            rightValue = AEditorGUI.ObjectField<Transform>(rightToggleValueRect, GUIContent.none, rightValue, true);
        }

        /// <summary>
        /// Group two transform references value.
        /// </summary>
        /// <param name="leftValue">First/Left transform reference.</param>
        /// <param name="rightValue">Second/Right transform reference.</param>
        /// <param name="leftValueContent">Label to display for first/left transform value.</param>
        /// <param name="rightValueContent">Label to display for second/right transform value.</param>
        public static void TransformGroupField(ref Transform leftValue, ref Transform rightValue, string leftValueContent, string rightValueContent)
        {
            GUILayout.Space(3);
            Rect leftToggleHeaderGroupRect = GUILayoutUtility.GetRect(0, 20);
            leftToggleHeaderGroupRect.height = 20;
            leftToggleHeaderGroupRect.width = leftToggleHeaderGroupRect.width / 2;
            GUI.Label(leftToggleHeaderGroupRect, GUIContent.none, GUI.skin.GetStyle("Tooltip"));
            GUI.Label(leftToggleHeaderGroupRect, leftValueContent, AEditorStyles.CenteredLabel);

            Rect rightToggleHeaderGroupRect = leftToggleHeaderGroupRect;
            rightToggleHeaderGroupRect.x = rightToggleHeaderGroupRect.width + 28;
            GUI.Label(rightToggleHeaderGroupRect, GUIContent.none, GUI.skin.GetStyle("Tooltip"));
            GUI.Label(rightToggleHeaderGroupRect, rightValueContent, AEditorStyles.CenteredLabel);

            Rect leftToggleValueGroupRect = GUILayoutUtility.GetRect(0, 25);
            leftToggleValueGroupRect.y -= 1;
            leftToggleValueGroupRect.height = 25;
            leftToggleValueGroupRect.width = leftToggleValueGroupRect.width / 2;
            GUI.Label(leftToggleValueGroupRect, GUIContent.none, GUI.skin.GetStyle("Tooltip"));

            Rect leftToggleValueRect = leftToggleValueGroupRect;
            leftToggleValueRect.width -= 12;
            leftToggleValueRect.height = 15;
            leftToggleValueRect.x += 7;
            leftToggleValueRect.y += 4;
            leftValue = AEditorGUI.ObjectField<Transform>(leftToggleValueRect, GUIContent.none, leftValue, true);

            Rect rightToggleValueGroupRect = leftToggleValueGroupRect;
            rightToggleValueGroupRect.x = rightToggleValueGroupRect.width + 28;
            GUI.Label(rightToggleValueGroupRect, GUIContent.none, GUI.skin.GetStyle("Tooltip"));

            Rect rightToggleValueRect = rightToggleValueGroupRect;
            rightToggleValueRect.width -= 12;
            rightToggleValueRect.height = 15;
            rightToggleValueRect.x += 7;
            rightToggleValueRect.y += 4;
            rightValue = AEditorGUI.ObjectField<Transform>(rightToggleValueRect, GUIContent.none, rightValue, true);
        }

        /// <summary>
        /// Group two bool references value.
        /// </summary>
        /// <param name="leftValue">First/Left bool reference.</param>
        /// <param name="rightValue">Second/Right bool reference.</param>
        /// <param name="leftValueContent">Label to display for first/left bool value.</param>
        /// <param name="rightValueContent">Label to display for second/right bool value.</param>
        public static void BoolGroupField(ref bool leftValue, ref bool rightValue, GUIContent leftValueContent, GUIContent rightValueContent)
        {
            GUILayout.Space(3);
            Rect leftToggleHeaderGroupRect = GUILayoutUtility.GetRect(0, 20);
            leftToggleHeaderGroupRect.height = 20;
            leftToggleHeaderGroupRect.width = leftToggleHeaderGroupRect.width / 2;
            GUI.Label(leftToggleHeaderGroupRect, GUIContent.none, GUI.skin.GetStyle("Tooltip"));
            GUI.Label(leftToggleHeaderGroupRect, leftValueContent, AEditorStyles.CenteredLabel);

            Rect rightToggleHeaderGroupRect = leftToggleHeaderGroupRect;
            rightToggleHeaderGroupRect.x = rightToggleHeaderGroupRect.width + 28;
            GUI.Label(rightToggleHeaderGroupRect, GUIContent.none, GUI.skin.GetStyle("Tooltip"));
            GUI.Label(rightToggleHeaderGroupRect, rightValueContent, AEditorStyles.CenteredLabel);

            Rect leftToggleValueGroupRect = GUILayoutUtility.GetRect(0, 25);
            leftToggleValueGroupRect.y -= 1;
            leftToggleValueGroupRect.height = 25;
            leftToggleValueGroupRect.width = leftToggleValueGroupRect.width / 2;
            GUI.Label(leftToggleValueGroupRect, GUIContent.none, GUI.skin.GetStyle("Tooltip"));

            Rect leftToggleValueRect = leftToggleValueGroupRect;
            leftToggleValueRect.width = 13;
            leftToggleValueRect.height = 13;
            leftToggleValueRect.x = leftToggleValueGroupRect.x + leftToggleValueGroupRect.width / 2 - leftToggleValueRect.width;
            leftToggleValueRect.y += 4;
            leftValue = EditorGUI.Toggle(leftToggleValueRect, leftValue);

            Rect rightToggleValueGroupRect = leftToggleValueGroupRect;
            rightToggleValueGroupRect.x = rightToggleValueGroupRect.width + 28;
            GUI.Label(rightToggleValueGroupRect, GUIContent.none, GUI.skin.GetStyle("Tooltip"));

            Rect rightToggleValueRect = rightToggleValueGroupRect;
            rightToggleValueRect.width = 13;
            rightToggleValueRect.height = 13;
            rightToggleValueRect.x = rightToggleValueGroupRect.x + rightToggleValueGroupRect.width / 2 - rightToggleValueRect.width;
            rightToggleValueRect.y += 4;
            rightValue = EditorGUI.Toggle(rightToggleValueRect, rightValue);
        }

        /// <summary>
        /// Group two bool references value.
        /// </summary>
        /// <param name="leftValue">First/Left bool reference.</param>
        /// <param name="rightValue">Second/Right bool reference.</param>
        /// <param name="leftValueContent">Label to display for first/left bool value.</param>
        /// <param name="rightValueContent">Label to display for second/right bool value.</param>
        public static void BoolGroupField(ref bool leftValue, ref bool rightValue, string leftValueContent, string rightValueContent)
        {
            GUILayout.Space(3);
            Rect leftToggleHeaderGroupRect = GUILayoutUtility.GetRect(0, 20);
            leftToggleHeaderGroupRect.height = 20;
            leftToggleHeaderGroupRect.width = leftToggleHeaderGroupRect.width / 2;
            GUI.Label(leftToggleHeaderGroupRect, GUIContent.none, GUI.skin.GetStyle("Tooltip"));
            GUI.Label(leftToggleHeaderGroupRect, leftValueContent, AEditorStyles.CenteredLabel);

            Rect rightToggleHeaderGroupRect = leftToggleHeaderGroupRect;
            rightToggleHeaderGroupRect.x = rightToggleHeaderGroupRect.width + 28;
            GUI.Label(rightToggleHeaderGroupRect, GUIContent.none, GUI.skin.GetStyle("Tooltip"));
            GUI.Label(rightToggleHeaderGroupRect, rightValueContent, AEditorStyles.CenteredLabel);

            Rect leftToggleValueGroupRect = GUILayoutUtility.GetRect(0, 25);
            leftToggleValueGroupRect.y -= 1;
            leftToggleValueGroupRect.height = 25;
            leftToggleValueGroupRect.width = leftToggleValueGroupRect.width / 2;
            GUI.Label(leftToggleValueGroupRect, GUIContent.none, GUI.skin.GetStyle("Tooltip"));

            Rect leftToggleValueRect = leftToggleValueGroupRect;
            leftToggleValueRect.width = 13;
            leftToggleValueRect.height = 13;
            leftToggleValueRect.x = leftToggleValueGroupRect.x + leftToggleValueGroupRect.width / 2 - leftToggleValueRect.width;
            leftToggleValueRect.y += 4;
            leftValue = EditorGUI.Toggle(leftToggleValueRect, leftValue);

            Rect rightToggleValueGroupRect = leftToggleValueGroupRect;
            rightToggleValueGroupRect.x = rightToggleValueGroupRect.width + 28;
            GUI.Label(rightToggleValueGroupRect, GUIContent.none, GUI.skin.GetStyle("Tooltip"));

            Rect rightToggleValueRect = rightToggleValueGroupRect;
            rightToggleValueRect.width = 13;
            rightToggleValueRect.height = 13;
            rightToggleValueRect.x = rightToggleValueGroupRect.x + rightToggleValueGroupRect.width / 2 - rightToggleValueRect.width;
            rightToggleValueRect.y += 4;
            rightValue = EditorGUI.Toggle(rightToggleValueRect, rightValue);
        }

        /// <summary>
        /// Make a text field for entering float values.
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        /// <param name="content">Label to display in front of the float field.</param>
        /// <param name="toggleContent">Label to display in front of the bool field.</param>
        /// <param name="value">Float value to edit.</param>
        /// <param name="toggle">Bool value to edit.</param>
        public static void HiddenFloatField(GUIContent content, GUIContent toggleContent, ref float value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.FloatField(content, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Make a text field for entering float values.
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        /// <param name="content">Label to display in front of the float field.</param>
        /// <param name="toggleContent">Label to display in front of the bool field.</param>
        /// <param name="value">Float value to edit.</param>
        /// <param name="toggle">Bool value to edit.</param>
        public static void HiddenFloatField(string content, string toggleContent, ref float value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.FloatField(content, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Make a delayed text field for entering float values.
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        /// <param name="content">Label to display in front of the float field.</param>
        /// <param name="toggleContent">Label to display in front of the bool field.</param>
        /// <param name="value">Float value to edit.</param>
        /// <param name="toggle">Bool value to edit.</param>
        public static void HiddenDelayedFloatField(GUIContent content, GUIContent toggleContent, ref float value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.DelayedFloatField(content, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Make a delayed text field for entering float values.
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        /// <param name="content">Label to display in front of the float field.</param>
        /// <param name="toggleContent">Label to display in front of the bool field.</param>
        /// <param name="value">Float value to edit.</param>
        /// <param name="toggle">Bool value to edit.</param>
        public static void HiddenDelayedFloatField(string content, string toggleContent, ref float value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.DelayedFloatField(content, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Make a text field for entering float values with the lowest possible value.
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        /// <param name="content">Label to display in front of the float field.</param>
        /// <param name="toggleContent">Label to display in front of the bool field.</param>
        /// <param name="value">Float value to edit.</param>
        /// <param name="toggle">Bool value to edit.</param>
        /// <param name="minValue">The lowest possible value.</param>
        public static void HiddenFixedFloatField(GUIContent content, GUIContent toggleContent, ref float value, ref bool toggle, float minValue)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = FixedFloatField(content, value, minValue);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Make a text field for entering float values with the lowest possible value.
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        /// <param name="content">Label to display in front of the float field.</param>
        /// <param name="toggleContent">Label to display in front of the bool field.</param>
        /// <param name="value">Float value to edit.</param>
        /// <param name="toggle">Bool value to edit.</param>
        /// <param name="minValue">The lowest possible value.</param>
        public static void HiddenFixedFloatField(string content, string toggleContent, ref float value, ref bool toggle, float minValue)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = FixedFloatField(content, value, minValue);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Make a text field for entering double values.
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        /// <param name="content">Label to display in front of the double field.</param>
        /// <param name="toggleContent">Label to display in front of the bool field.</param>
        /// <param name="value">Double value to edit.</param>
        /// <param name="toggle">Bool value to edit.</param>
        public static void HiddenDoubleField(GUIContent content, GUIContent toggleContent, ref double value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.DoubleField(content, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Make a text field for entering double values.
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        /// <param name="content">Label to display in front of the double field.</param>
        /// <param name="toggleContent">Label to display in front of the bool field.</param>
        /// <param name="value">Double value to edit.</param>
        /// <param name="toggle">Bool value to edit.</param>
        public static void HiddenDoubleField(string content, string toggleContent, ref double value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.DoubleField(content, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Make a delayed text field for entering double values.
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        /// <param name="content">Label to display in front of the double field.</param>
        /// <param name="toggleContent">Label to display in front of the bool field.</param>
        /// <param name="value">Double value to edit.</param>
        /// <param name="toggle">Bool value to edit.</param>
        public static void HiddenDelayedDoubleField(GUIContent content, GUIContent toggleContent, ref double value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.DelayedDoubleField(content, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Make a delayed text field for entering double values.
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        /// <param name="content">Label to display in front of the double field.</param>
        /// <param name="toggleContent">Label to display in front of the bool field.</param>
        /// <param name="value">Double value to edit.</param>
        /// <param name="toggle">Bool value to edit.</param>
        public static void HiddenDelayedDoubleField(string content, string toggleContent, ref double value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.DoubleField(content, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Make a text field for entering int values.
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        /// <param name="content">Label to display in front of the int field.</param>
        /// <param name="toggleContent">Label to display in front of the bool field.</param>
        /// <param name="value">Int value to edit.</param>
        /// <param name="toggle">Bool value to edit.</param>
        public static void HiddenIntField(GUIContent content, GUIContent toggleContent, ref int value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.IntField(content, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Make a text field for entering int values.
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        /// <param name="content">Label to display in front of the int field.</param>
        /// <param name="toggleContent">Label to display in front of the bool field.</param>
        /// <param name="value">Int value to edit.</param>
        /// <param name="toggle">Bool value to edit.</param>
        public static void HiddenIntField(string content, string toggleContent, ref int value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.IntField(content, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Make a delayed text field for entering int values.
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        /// <param name="content">Label to display in front of the int field.</param>
        /// <param name="toggleContent">Label to display in front of the bool field.</param>
        /// <param name="value">Int value to edit.</param>
        /// <param name="toggle">Bool value to edit.</param>
        public static void HiddenDelayedIntField(GUIContent content, GUIContent toggleContent, ref int value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.IntField(content, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Make a delayed text field for entering int values.
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        /// <param name="content">Label to display in front of the int field.</param>
        /// <param name="toggleContent">Label to display in front of the bool field.</param>
        /// <param name="value">Int value to edit.</param>
        /// <param name="toggle">Bool value to edit.</param>
        public static void HiddenDelayedIntField(string content, string toggleContent, ref int value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.IntField(content, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Make a text field.
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        /// <param name="content">Label to display in front of the text field.</param>
        /// <param name="toggleContent">Label to display in front of the bool field.</param>
        /// <param name="value">String value to edit.</param>
        /// <param name="toggle">Bool value to edit.</param>
        public static void HiddenTextField(GUIContent content, GUIContent toggleContent, ref string value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.TextField(content, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Make a text field.
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        /// <param name="content">Label to display in front of the text field.</param>
        /// <param name="toggleContent">Label to display in front of the bool field.</param>
        /// <param name="value">String value to edit.</param>
        /// <param name="toggle">Bool value to edit.</param>
        public static void HiddenTextField(string content, string toggleContent, ref string value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.TextField(content, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Make a delayed text field.
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        /// <param name="content">Label to display in front of the text field.</param>
        /// <param name="toggleContent">Label to display in front of the bool field.</param>
        /// <param name="value">String value to edit.</param>
        /// <param name="toggle">Bool value to edit.</param>
        public static void HiddenDelayedTextField(GUIContent content, GUIContent toggleContent, ref string value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.DelayedTextField(content, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Make a delayed text field.
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        /// <param name="content">Label to display in front of the text field.</param>
        /// <param name="toggleContent">Label to display in front of the bool field.</param>
        /// <param name="value">String value to edit.</param>
        /// <param name="toggle">Bool value to edit.</param>
        public static void HiddenDelayedTextField(string content, string toggleContent, ref string value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.DelayedTextField(content, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Make a field for selecting a Color.
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        /// <param name="content">Label to display in front of the color field.</param>
        /// <param name="toggleContent">Label to display in front of the bool field.</param>
        /// <param name="value">Color value to edit.</param>
        /// <param name="toggle">Bool value to edit.</param>
        public static void HiddenColorField(GUIContent content, GUIContent toggleContent, ref Color value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.ColorField(content, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Make a field for selecting a Color.
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        /// <param name="content">Label to display in front of the color field.</param>
        /// <param name="toggleContent">Label to display in front of the bool field.</param>
        /// <param name="value">Color value to edit.</param>
        /// <param name="toggle">Bool value to edit.</param>
        public static void HiddenColorField(string content, string toggleContent, ref Color value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.ColorField(content, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Make an X & Y field for entering a Vector2.
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        /// <param name="content">Label to display in front of the vector2 field.</param>
        /// <param name="toggleContent">Label to display in front of the bool field.</param>
        /// <param name="value">Vector2 value to edit.</param>
        /// <param name="toggle">Bool value to edit.</param>
        public static void HiddenVector2Field(GUIContent content, GUIContent toggleContent, ref Vector2 value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.Vector2Field(content, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Make an X & Y field for entering a Vector2.
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        /// <param name="content">Label to display in front of the vector2 field.</param>
        /// <param name="toggleContent">Label to display in front of the bool field.</param>
        /// <param name="value">Vector2 value to edit.</param>
        /// <param name="toggle">Bool value to edit.</param>
        public static void HiddenVector2Field(string content, string toggleContent, ref Vector2 value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.Vector2Field(content, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Make an X, Y & Z field for entering a Vector3.
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        /// <param name="content">Label to display in front of the vector3 field.</param>
        /// <param name="toggleContent">Label to display in front of the bool field.</param>
        /// <param name="value">Vector3 value to edit.</param>
        /// <param name="toggle">Bool value to edit.</param>
        public static void HiddenVector3Field(GUIContent content, GUIContent toggleContent, ref Vector3 value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.Vector3Field(content, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Make an X, Y & Z field for entering a Vector3.
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        /// <param name="content">Label to display in front of the vector3 field.</param>
        /// <param name="toggleContent">Label to display in front of the bool field.</param>
        /// <param name="value">Vector3 value to edit.</param>
        /// <param name="toggle">Bool value to edit.</param>
        public static void HiddenVector3Field(string content, string toggleContent, ref Vector3 value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.Vector3Field(content, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Make an X, Y, Z & W field for entering a Vector4.
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        /// <param name="content">Label to display in front of the vector4 field.</param>
        /// <param name="toggleContent">Label to display in front of the bool field.</param>
        /// <param name="value">Vector4 value to edit.</param>
        /// <param name="toggle">Bool value to edit.</param>
        public static void HiddenVector4Field(GUIContent content, GUIContent toggleContent, ref Vector4 value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.Vector4Field(content, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Make an X, Y, Z & W field for entering a Vector4.
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        /// <param name="content">Label to display in front of the vector4 field.</param>
        /// <param name="toggleContent">Label to display in front of the bool field.</param>
        /// <param name="value">Vector4 value to edit.</param>
        /// <param name="toggle">Bool value to edit.</param>
        public static void HiddenVector4Field(string content, string toggleContent, ref Vector4 value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.Vector4Field(content, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Make a field for editing an AnimationCurve.
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        /// <param name="content">Label to display in front of the animationCurve field.</param>
        /// <param name="toggleContent">Label to display in front of the bool field.</param>
        /// <param name="value">AnimationCurve value to edit.</param>
        /// <param name="toggle">Bool value to edit.</param>
        public static void HiddenCurveField(GUIContent content, GUIContent toggleContent, ref AnimationCurve value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.CurveField(content, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Make a field for editing an AnimationCurve.
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        /// <param name="content">Label to display in front of the animationCurve field.</param>
        /// <param name="toggleContent">Label to display in front of the bool field.</param>
        /// <param name="value">AnimationCurve value to edit.</param>
        /// <param name="toggle">Bool value to edit.</param>
        public static void HiddenCurveField(string content, string toggleContent, ref AnimationCurve value, ref bool toggle)
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                value = EditorGUILayout.CurveField(content, value);
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Make a field to receive any object type.
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        /// <param name="content">Label to display in front of the TObject field.</param>
        /// <param name="toggleContent">Label to display in front of the bool field.</param>
        /// <param name="value">TObject value to edit.</param>
        /// <param name="toggle">Bool value to edit.</param>
        /// <typeparam name="TObject">where TObject is Object.</typeparam>
        public static void HiddenObjectField<TObject>(string content, string toggleContent, ref TObject value, ref bool toggle) where TObject : Object
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(!toggle);
                value = (TObject)EditorGUILayout.ObjectField(content, value, typeof(TObject), true);
                EditorGUI.EndDisabledGroup();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Make a field to receive any object type.
        /// Field hidden behind the toggle, the box will be displayed if the toggle is true.
        /// </summary>
        /// <param name="content">Label to display in front of the TObject field.</param>
        /// <param name="toggleContent">Label to display in front of the bool field.</param>
        /// <param name="value">TObject value to edit.</param>
        /// <param name="toggle">Bool value to edit.</param>
        public static void HiddenObjectField<TObject>(GUIContent content, GUIContent toggleContent, ref TObject value, ref bool toggle) where TObject : Object
        {
            if (toggle)
            {
                GUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(!toggle);
                value = (TObject)EditorGUILayout.ObjectField(content, value, typeof(TObject), true);
                EditorGUI.EndDisabledGroup();
                float width = EditorGUI.indentLevel > 0 ? 30.0f : 14.0f;
                toggle = EditorGUILayout.Toggle(toggle, GUILayout.Width(width));
                GUILayout.EndHorizontal();
            }
            else
            {
                toggle = EditorGUILayout.Toggle(toggleContent, toggle);
            }
        }

        /// <summary>
        /// Make a editor to edit all properties of ShakeProperties value.
        /// </summary>
        /// <param name="value">ShakeProperties value to edit.</param>
        /// <returns>ShakeProperties value edited by user.</returns>
        public static Shake ShakeField(Shake value)
        {
            value.SetTarget((Shake.Target)EditorGUILayout.EnumFlagsField(ContentProperties.Target, value.GetTarget()));

            CameraShake.ShakeProperty positionShake = value.GetPositionSettings();

            Rect position = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight);

            Rect controlRect = EditorGUI.PrefixLabel(position, new GUIContent("Amplitude"));
            Rect[] splittedRect = AEditorGUI.SplitRect(controlRect, 2);

            positionShake.SetAmplitude(AEditorGUI.FixedFloatField(splittedRect[0], positionShake.GetAmplitude(), 0));
            positionShake.SetAmplitude(AEditorGUI.FixedFloatField(splittedRect[1], positionShake.GetAmplitude(), 0));
            //value.SetFrequency(EditorGUILayout.FloatField(ContentProperties.Frequency, value.GetFrequency()));
            //value.SetDuration(EditorGUILayout.FloatField(ContentProperties.Duration, value.GetDuration()));
            //value.SetBlendOverLifetime(EditorGUILayout.CurveField(ContentProperties.BlendOverLifetime, value.GetBlendOverLifetime()));
            return value;
        }

        /// <summary>
        /// Make a editor to edit all properties of ScreenFadeProperties value.
        /// </summary>
        /// <param name="value">ScreenFadeProperties value to edit.</param>
        /// <returns>ScreenFadeProperties value edited by user.</returns>
        public static ScreenFadeProperties CameraFadeSettingsField(ScreenFadeProperties value)
        {
            value.SetInFadeColor(EditorGUILayout.ColorField(ContentProperties.FadeColor, value.GetInFadeColor()));
            value.SetInFadeSpeed(FixedFloatField(ContentProperties.FadeInSpeed, value.GetInFadeSpeed(), 0));
            value.SetOutFadeSpeed(FixedFloatField(ContentProperties.FadeOutSpeed, value.GetOutFadeSpeed(), 0));
            return value;
        }

        /// <summary>
        /// Make a text field for edit AnimatorValue.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>AnimatorValue value edited by user.</returns>
        public static AnimatorValue AnimatorValueField(GUIContent content, AnimatorValue value, params GUILayoutOption[] options)
        {
            value.SetName(EditorGUILayout.TextField(content, value.GetName(), options));
            return value;
        }

        /// <summary>
        /// Make a text field for edit AnimatorValue.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>AnimatorValue value edited by user.</returns>
        public static AnimatorValue AnimatorValueField(string content, AnimatorValue value, params GUILayoutOption[] options)
        {
            value.SetName(EditorGUILayout.TextField(content, value.GetName(), options));
            return value;
        }

        /// <summary>
        /// Make a text field for edit AnimatorValue.
        /// </summary>
        /// <param name="value">The value to edit.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>AnimatorValue value edited by user.</returns>
        public static AnimatorValue AnimatorValueField(AnimatorValue value, params GUILayoutOption[] options)
        {
            value.SetName(EditorGUILayout.TextField(value.GetName(), options));
            return value;
        }

        /// <summary>
        /// Make a text field for edit AnimatorState.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <returns>AnimatorState value edited by user.</returns>
        public static AnimatorState AnimatorStateField(GUIContent content, AnimatorState value)
        {
            Rect position = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight);

            string storedName = value.GetName();
            Rect namePosition = new Rect(position.x, position.y, position.width - 50, position.height);
            value.SetName(EditorGUI.DelayedTextField(namePosition, content, value.GetName()));
            if (storedName != value.GetName())
            {
                value = new AnimatorState(value.GetName(), value.GetLayer(), value.GetFixedTime());
            }

            int storedIndexLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            Rect layerPosition = new Rect(position.x + namePosition.width, position.y, 15, position.height);
            value.SetLayer(EditorGUI.IntField(layerPosition, value.GetLayer()));

            Rect fixedTimePosition = new Rect(layerPosition.x + layerPosition.width, position.y, 35, position.height);
            value.SetFixedTime(EditorGUI.FloatField(fixedTimePosition, value.GetFixedTime()));
            EditorGUI.indentLevel = storedIndexLevel;

            return value;
        }

        /// <summary>
        /// Make a text field for edit AnimatorState.
        /// </summary>
        /// <param name="content">Optional label in front of the field.</param>
        /// <param name="value">The value to edit.</param>
        /// <returns>AnimatorState value edited by user.</returns>
        public static AnimatorState AnimatorStateField(string content, AnimatorState value)
        {
            Rect position = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight);

            string storedName = value.GetName();
            Rect namePosition = new Rect(position.x, position.y, position.width - 50, position.height);
            value.SetName(EditorGUI.DelayedTextField(namePosition, content, value.GetName()));
            if (storedName != value.GetName())
            {
                value = new AnimatorState(value.GetName(), value.GetLayer(), value.GetFixedTime());
            }

            int storedIndexLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            Rect layerPosition = new Rect(position.x + namePosition.width, position.y, 15, position.height);
            value.SetLayer(EditorGUI.IntField(layerPosition, value.GetLayer()));

            Rect fixedTimePosition = new Rect(layerPosition.x + layerPosition.width, position.y, 35, position.height);
            value.SetFixedTime(EditorGUI.FloatField(fixedTimePosition, value.GetFixedTime()));
            EditorGUI.indentLevel = storedIndexLevel;

            return value;
        }

        /// <summary>
        /// Make a text field for edit AnimatorValue with right action button.
        /// </summary>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>AnimatorValue value edited by user.</returns>
        public static AnimatorValue ActionAnimatorValueField(GUIContent content, AnimatorValue value, Action action, params GUILayoutOption[] options)
        {
            value.SetName(AEditorGUILayout.ActionTextField(content, value.GetName(), action, options));
            return value;
        }

        /// <summary>
        /// Make a text field for edit AnimatorValue with right action button.
        /// </summary>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>AnimatorValue value edited by user.</returns>
        public static AnimatorValue ActionAnimatorValueField(string content, AnimatorValue value, Action action, params GUILayoutOption[] options)
        {
            value.SetName(AEditorGUILayout.ActionTextField(content, value.GetName(), action, options));
            return value;
        }

        /// <summary>
        /// Make a text field for edit AnimatorValue with right action button.
        /// </summary>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>AnimatorValue value edited by user.</returns>
        public static AnimatorValue ActionAnimatorValueField(AnimatorValue value, Action action, params GUILayoutOption[] options)
        {
            value.SetName(AEditorGUILayout.ActionTextField(value.GetName(), action, options));
            return value;
        }

        /// <summary>
        /// Make a text field for edit AnimatorValue with right action button.
        /// </summary>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>AnimatorValue value edited by user.</returns>
        public static AnimatorValue ActionAnimatorValueField(GUIContent content, AnimatorValue value, Action action, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            value.SetName(AEditorGUILayout.ActionTextField(content, value.GetName(), action, actionButtonTooltip, options));
            return value;
        }

        /// <summary>
        /// Make a text field for edit AnimatorValue with right action button.
        /// </summary>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>AnimatorValue value edited by user.</returns>
        public static AnimatorValue ActionAnimatorValueField(string content, AnimatorValue value, Action action, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            value.SetName(AEditorGUILayout.ActionTextField(content, value.GetName(), action, actionButtonTooltip, options));
            return value;
        }

        /// <summary>
        /// Make a text field for edit AnimatorValue with right action button.
        /// </summary>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function will called after button pressed.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>AnimatorValue value edited by user.</returns>
        public static AnimatorValue ActionAnimatorValueField(AnimatorValue value, Action action, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            value.SetName(AEditorGUILayout.ActionTextField(value.GetName(), action, actionButtonTooltip, options));
            return value;
        }

        /// <summary>
        /// Make a text field for edit AnimatorValue with right action button.
        /// </summary>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>AnimatorValue value edited by user.</returns>
        public static AnimatorValue ActionAnimatorValueField(GUIContent content, AnimatorValue value, Action<object> action, object actionData, params GUILayoutOption[] options)
        {
            value.SetName(AEditorGUILayout.ActionTextField(content, value.GetName(), action, actionData, options));
            return value;
        }

        /// <summary>
        /// Make a text field for edit AnimatorValue with right action button.
        /// </summary>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>AnimatorValue value edited by user.</returns>
        public static AnimatorValue ActionAnimatorValueField(string content, AnimatorValue value, Action<object> action, object actionData, params GUILayoutOption[] options)
        {
            value.SetName(AEditorGUILayout.ActionTextField(content, value.GetName(), action, actionData, options));
            return value;
        }

        /// <summary>
        /// Make a text field for edit AnimatorValue with right action button.
        /// </summary>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>AnimatorValue value edited by user.</returns>
        public static AnimatorValue ActionAnimatorValueField(AnimatorValue value, Action<object> action, object actionData, params GUILayoutOption[] options)
        {
            value.SetName(AEditorGUILayout.ActionTextField(value.GetName(), action, actionData, options));
            return value;
        }

        /// <summary>
        /// Make a text field for edit AnimatorValue with right action button.
        /// </summary>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>AnimatorValue value edited by user.</returns>
        public static AnimatorValue ActionAnimatorValueField(GUIContent content, AnimatorValue value, Action<object> action, object actionData, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            value.SetName(AEditorGUILayout.ActionTextField(content, value.GetName(), action, actionData, actionButtonTooltip, options));
            return value;
        }

        /// <summary>
        /// Make a text field for edit AnimatorValue with right action button.
        /// </summary>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>AnimatorValue value edited by user.</returns>
        public static AnimatorValue ActionAnimatorValueField(string content, AnimatorValue value, Action<object> action, object actionData, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            value.SetName(AEditorGUILayout.ActionTextField(content, value.GetName(), action, actionData, actionButtonTooltip, options));
            return value;
        }

        /// <summary>
        /// Make a text field for edit AnimatorValue with right action button.
        /// </summary>
        /// <param name="value">The value to edit.</param>
        /// <param name="action">Callback function with user data will called after button pressed.</param>
        /// <param name="actionData">Callback function user data.</param>
        /// <param name="actionButtonTooltip">Tooltip for action button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>AnimatorValue value edited by user.</returns>
        public static AnimatorValue ActionAnimatorValueField(AnimatorValue value, Action<object> action, object actionData, string actionButtonTooltip = "", params GUILayoutOption[] options)
        {
            value.SetName(AEditorGUILayout.ActionTextField(value.GetName(), action, actionData, actionButtonTooltip, options));
            return value;
        }

        /// <summary>
        /// Arrays editor grouped in toolbar.
        /// </summary>
        /// <param name="selected">Reference of int value.</param>
        /// <param name="properties">Arrays toolbar editor properties.</param>
        public static void ArraysToolbarEditor(ref int selected, ArraysToolbarEditorProperty[] properties)
        {
            GUILayout.BeginVertical();
            GUIContent[] barNames = new GUIContent[properties.Length];
            for (int i = 0; i < properties.Length; i++)
            {
                barNames[i] = properties[i].toolbarName;
            }

            GUIStyle toolbarButtonStyle = new GUIStyle("ToolbarButton");
            GUIStyle borderStyle = new GUIStyle("grey_border");
            float singleLineHeight = EditorGUIUtility.singleLineHeight;
            float fieldsHeight = 0;

            Rect layoutRect = GUILayoutUtility.GetRect(1, 1);
            float topCoordinate = layoutRect.y;
#if UNITY_2019_3_OR_NEWER
            layoutRect.x += 2;
#endif

            selected = GUI.Toolbar(layoutRect, selected, barNames, toolbarButtonStyle);

#if UNITY_2019_3_OR_NEWER
            layoutRect.x -= 2;
#endif

            Rect groupRect = new Rect(layoutRect.x + 0.5f, layoutRect.y + 19, layoutRect.width + 0.5f, singleLineHeight);
            for (int i = 0; i < properties.Length; i++)
            {
                if (selected == i)
                {
                    ArraysToolbarEditorProperty property = properties[i];
                    if (property.serializedArray != null)
                    {
                        if (property.serializedArray.arraySize == 0)
                        {
                            Rect labelRect = new Rect(groupRect.x, groupRect.y - 2, groupRect.width - 40, singleLineHeight + 5);
#if UNITY_2019_3_OR_NEWER
                            labelRect.y += 2;
#endif
                            GUI.Label(labelRect, GUIContent.none, borderStyle);
                            GUI.Label(labelRect, string.Format("Add new {0}", property.elementsContent.text), AEditorStyles.CenteredMiniGrayLabel);
                        }

                        for (int j = 0; j < property.serializedArray.arraySize; j++)
                        {
                            SerializedProperty element = property.serializedArray.GetArrayElementAtIndex(j);

                            Rect fieldRect = new Rect(groupRect.x - 3.5f, groupRect.y + 1.5f, groupRect.width, singleLineHeight);

#if UNITY_2019_3_OR_NEWER
                            groupRect.x += 1;
#endif
                            GUI.Label(groupRect, GUIContent.none, toolbarButtonStyle);

#if UNITY_2019_3_OR_NEWER
                            groupRect.x -= 1;
                            groupRect.height += 4;
                            groupRect.width += 1;
#endif
                            GUI.Label(groupRect, GUIContent.none, borderStyle);
#if UNITY_2019_3_OR_NEWER
                            groupRect.x += 1;
                            groupRect.height -= 4;
                            groupRect.width -= 1;
#endif

                            EditorGUI.PropertyField(fieldRect, element, property.elementsContent);

#if UNITY_2019_3_OR_NEWER
                            groupRect.x -= 1;
#endif
                            groupRect.y += singleLineHeight + 3f;
                            fieldsHeight += singleLineHeight + 3f;
                        }
                    }

                    Rect plusButtonRect = new Rect(groupRect.width - 7.5f, groupRect.y, 20, singleLineHeight);

#if UNITY_2019_3_OR_NEWER
                    plusButtonRect.x += 3f;
                    plusButtonRect.y += 1.5f;
#endif

                    if (GUI.Button(plusButtonRect, " +", toolbarButtonStyle))
                    {
                        property.serializedArray.arraySize++;
                    }

                    Rect minusButtonRect = new Rect(groupRect.width + 13, groupRect.y, 20, singleLineHeight);

#if UNITY_2019_3_OR_NEWER
                    minusButtonRect.x += 3f;
                    minusButtonRect.y += 1.5f;
                    minusButtonRect.width += 1;
#endif

                    EditorGUI.BeginDisabledGroup(property.serializedArray.arraySize <= 0);
                    if (GUI.Button(minusButtonRect, " -", toolbarButtonStyle))
                    {
                        property.serializedArray.arraySize--;
                    }
                    EditorGUI.EndDisabledGroup();
                    GUILayout.Space(plusButtonRect.y + 20);

                    Rect buttonBorderRect = new Rect(groupRect.width - 7.5f, groupRect.y - 1f, 41.5f, 20);

#if UNITY_2019_3_OR_NEWER
                    buttonBorderRect.x += 2f;
                    buttonBorderRect.y += 1.5f;
                    buttonBorderRect.width += 1;
                    buttonBorderRect.height += 2;
#endif

                    GUI.Label(buttonBorderRect, GUIContent.none, borderStyle);
                }
            }

            Rect groupBorderRect = new Rect(groupRect.x, topCoordinate - 1, groupRect.width + 1, groupRect.height + fieldsHeight + 3);
            GUI.Label(groupBorderRect, GUIContent.none, borderStyle);
            GUILayout.EndVertical();
        }

        /// <summary>
        /// Make a single press button placed on right side.
        /// </summary>
        /// <param name="content">Text to display on the button.</param>
        /// <param name="style">The style to use. If left out, the button style from the current GUISkin is used.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>True when the users clicks the button.</returns>
        public static bool ButtonRight(string content, GUIStyle style, params GUILayoutOption[] options)
        {
            bool value = false;
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(content, style, options))
            {
                value = true;
            }
            GUILayout.EndHorizontal();
            return value;
        }

        /// <summary>
        /// Make a single press button placed on right side.
        /// </summary>
        /// <param name="content">Text to display on the button.</param>
        /// <param name="style">The style to use. If left out, the button style from the current GUISkin is used.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>True when the users clicks the button.</returns>
        public static bool ButtonRight(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
        {
            bool value = false;
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(content, style, options))
            {
                value = true;
            }
            GUILayout.EndHorizontal();
            return value;
        }

        /// <summary>
        /// Make a single press button placed on right side.
        /// </summary>
        /// <param name="content">Text to display on the button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>True when the users clicks the button.</returns>
        public static bool ButtonRight(string content, params GUILayoutOption[] options)
        {
            bool value = false;
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(content, options))
            {
                value = true;
            }
            GUILayout.EndHorizontal();
            return value;
        }

        /// <summary>
        /// Make a single press button placed on right side.
        /// </summary>
        /// <param name="content">Text to display on the button.</param>
        /// <param name="options"> An optional list of layout options that specify extra layout properties. Any
        ///     values passed in here will override settings defined by the style. See Also:
        ///     GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        ///     GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.
        /// </param>
        /// <returns>True when the users clicks the button.</returns>
        public static bool ButtonRight(GUIContent content, params GUILayoutOption[] options)
        {
            bool value = false;
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(content, options))
            {
                value = true;
            }
            GUILayout.EndHorizontal();
            return value;
        }

        public static bool OptionsButton()
        {
            bool value = false;
            GUILayout.Space(20);
            Rect buttonRect = GUILayoutUtility.GetLastRect();
            buttonRect.width = 20;
            buttonRect.height = 20;
            buttonRect.x += 1.5f;
            buttonRect.y += 1;
            GUIContent iconContent = EditorGUIUtility.IconContent("_Popup", "|Option field button.");
            if (GUI.Button(buttonRect, iconContent, GUI.skin.label))
            {
                value = true;
            }
            return value;
        }

        /// <summary>
        /// Draw dividing line.
        /// </summary>
        /// <param name="separate">Add separate before line.</param>
        public static void HorizontalLine(float separate = 1.0f)
        {
            Rect rect = GUILayoutUtility.GetRect(0, separate);
            rect.x -= 3;
            rect.width += 7;
            GUI.Label(rect, GUIContent.none, GUI.skin.GetStyle("IN Title"));
        }

        /// <summary>
        /// Draw dividing line.
        /// </summary>
        /// <param name="height">Line height.</param>
        /// <param name="width">Line width.</param>
        public static void VerticalLine(float height, float width = 1.0f)
        {
            Rect rect = GUILayoutUtility.GetRect(0, 0);
            rect.height = height;
            rect.width = width;
            EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
        }

        /// <summary>
        /// Draw dividing line.
        /// </summary>
        /// <param name="height">Line height.</param>
        /// <param name="color">Color of the line.</param>
        /// <param name="width">Line width.</param>
        public static void VerticalLine(float height, Color color, float width = 1.0f)
        {
            Rect rect = GUILayoutUtility.GetRect(0, 0);
            rect.height = height;
            rect.width = width;
            EditorGUI.DrawRect(rect, color);
        }

        /// <summary>
        /// Default button GUILayoutOption for option button.
        /// </summary>
        /// <returns>Default button GUILayoutOption array.</returns>
        public readonly static GUILayoutOption[] DefaultHandleButtonOptions = new GUILayoutOption[]
        {
#if UNITY_2019_3_OR_NEWER
            GUILayout.Width(130),
            GUILayout.MinWidth(30),
            GUILayout.MaxWidth(130)
#else
            GUILayout.Width(120),
            GUILayout.MinWidth(30),
            GUILayout.MaxWidth(120)
#endif
        };

        /// <summary>
        /// Remove element button GUILayoutOptions.
        /// </summary>
        /// <returns>Remove element button GUILayoutOptions array.</returns>
        public readonly static GUILayoutOption[] RemoveElementButtonOptions = new GUILayoutOption[]
        {
            GUILayout.Width(70),
            GUILayout.Height(15)
        };

        /// <summary>
        /// Default button GUILayoutOption for read/write button.
        /// </summary>
        /// <returns>Read/write button GUILayoutOption array.</returns>
        public readonly static GUILayoutOption[] ReadWriteButtonOptions = new GUILayoutOption[]
        {
            GUILayout.Height(15),
            GUILayout.Width(20)
        };

        /// <summary>
        /// Default button GUILayoutOption for action button.
        /// </summary>
        /// <returns>Action button GUILayoutOption array.</returns>
        public readonly static GUILayoutOption[] ActionButtonOptions = new GUILayoutOption[]
        {
            GUILayout.Height(15),
            GUILayout.Width(16f)
        };
    }
}