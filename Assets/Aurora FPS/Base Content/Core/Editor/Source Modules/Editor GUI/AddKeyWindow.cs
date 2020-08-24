/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using System;
using UnityEditor;
using UnityEngine;

namespace AuroraFPSEditor
{
    public sealed class AddKeyWindow : EditorWindow
    {
        /// <summary>
        /// AddKeyWindow required styles. 
        /// </summary>
        internal sealed class ContentStyles
        {
            public GUIStyle header = new GUIStyle("In BigTitle");
            public GUIStyle background = new GUIStyle("grey_border");
            public GUIStyle button = new GUIStyle("ToolbarButton");

            public ContentStyles()
            {
                this.header.font = EditorStyles.boldLabel.font;
            }
        }

        // Base properties.
        private event Action onFieldGUICallback;
        private event Action onApplyCallback;
        private event Func<bool> applyIsAcitve;

        // Stored required properties.
        private ContentStyles styles;
        private Vector2 scrollPosition;
        private Rect rect;
        private float width;
        private float height;

        public static AddKeyWindow CreateWindow(Rect rect, float width, float height, Action onFieldGUICallback, Action onApplyCallback)
        {
            AddKeyWindow addKeyWindow = ScriptableObject.CreateInstance<AddKeyWindow>();
            addKeyWindow.rect = rect;
            addKeyWindow.height = height;
            addKeyWindow.width = width;

#if UNITY_2019_3_OR_NEWER
            addKeyWindow.rect.y += 2;
            addKeyWindow.height += 7;
            width += 5;
#endif

            addKeyWindow.onFieldGUICallback = onFieldGUICallback;
            addKeyWindow.onApplyCallback = onApplyCallback;
            addKeyWindow.applyIsAcitve = () => { return true; };
            return addKeyWindow;
        }

        public static AddKeyWindow CreateWindow(Rect rect, float width, float height, Action onFieldGUICallback, Action onApplyCallback, Func<bool> applyIsAcitve)
        {
            AddKeyWindow addKeyWindow = ScriptableObject.CreateInstance<AddKeyWindow>();
            addKeyWindow.rect = rect;
            addKeyWindow.height = height;
            addKeyWindow.width = width;

#if UNITY_2019_3_OR_NEWER
            addKeyWindow.rect.y += 2;
            addKeyWindow.height += 7;
            width += 5;
#endif

            addKeyWindow.onFieldGUICallback = onFieldGUICallback;
            addKeyWindow.onApplyCallback = onApplyCallback;
            addKeyWindow.applyIsAcitve = applyIsAcitve;
            return addKeyWindow;
        }

        /// <summary>
        /// Show add key editor window.
        /// </summary>
        /// <param name="width">Window width.</param>
        /// <param name="height">Window height.</param>
        public new void Show()
        {
            this.Initialize(rect);
            this.Repaint();
        }

        /// <summary>
        /// Initialize and show window.
        /// </summary>
        /// <param name="rect">Window rectangle.</param>
        private void Initialize(Rect rect)
        {
            Vector2 v2 = GUIUtility.GUIToScreenPoint(new Vector2(rect.x, rect.y));
            rect.x = v2.x;
            rect.y = v2.y;
            ShowAsDropDown(rect, new Vector2(rect.width, height));
            Focus();
            styles = new ContentStyles();
            wantsMouseMove = true;
        }

        /// <summary>
        /// OnGUI is called for rendering and handling GUI events.
        /// This function can be called multiple times per frame (one call per event).
        /// </summary>
        private void OnGUI()
        {
            Rect rect = GUILayoutUtility.GetRect(10f, 25f);
            GUI.Label(rect, "Add Key", styles.header);
            GUI.Label(new Rect(0.0f, 0.0f, width, height), GUIContent.none, styles.background);
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            onFieldGUICallback?.Invoke();
            GUILayout.EndScrollView();

            Rect closeButtonRect = new Rect(0, height - 19, 50, 15);

#if UNITY_2019_3_OR_NEWER
            closeButtonRect.x = 1.55f;
            closeButtonRect.y -= 2;
#endif
            if (GUI.Button(closeButtonRect, "Cancel", styles.button))
            {
                Close();
            }

            Rect applyButtonRect = new Rect(width - 50, height - 19, 50, 15);

#if UNITY_2019_3_OR_NEWER
            applyButtonRect.x -= 1;
            applyButtonRect.y -= 2;
#endif
            EditorGUI.BeginDisabledGroup(!applyIsAcitve.Invoke());
            if (GUI.Button(applyButtonRect, "Apply", styles.button))
            {
                onApplyCallback?.Invoke();
                Close();
            }
            EditorGUI.EndDisabledGroup();
        }

        public static Rect FixRect(float width)
        {
            Rect rect = GUILayoutUtility.GetLastRect();
            rect.x += rect.width;
            rect.y += 21;
            rect.width = width;
            return rect;
        }
    }
}