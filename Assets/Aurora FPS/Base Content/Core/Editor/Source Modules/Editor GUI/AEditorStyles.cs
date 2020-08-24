/* ==================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================== */

using UnityEngine;

namespace AuroraFPSEditor
{
    public static class AEditorStyles
    {
        public static GUIStyle LabelWrap
        {
            get
            {
                GUIStyle style = new GUIStyle();
                style.wordWrap = true;
                return style;
            }
        }

        public static GUIStyle GroupHeaderLabel
        {
            get
            {
                GUIStyle style = new GUIStyle();
                style.alignment = TextAnchor.UpperCenter;
                style.fontStyle = FontStyle.Bold;
                style.normal.textColor = new Color32(70, 70, 70, 255);
                return style;
            }
        }

        public static GUIStyle CenteredBoldLabel
        {
            get
            {
                GUIStyle style = new GUIStyle();
                style.fontStyle = FontStyle.Bold;
                style.alignment = TextAnchor.MiddleCenter;
                return style;
            }
        }

        public static GUIStyle CenteredLabel
        {
            get
            {
                GUIStyle style = new GUIStyle();
                style.fontStyle = FontStyle.Normal;
                style.alignment = TextAnchor.MiddleCenter;
                return style;
            }
        }

        public static GUIStyle CenteredBoldLabelWrap
        {
            get
            {
                GUIStyle style = new GUIStyle();
                style.fontStyle = FontStyle.Bold;
                style.wordWrap = true;
                style.alignment = TextAnchor.MiddleCenter;
                return style;
            }
        }

        public static GUIStyle CenteredLabelWrap
        {
            get
            {
                GUIStyle style = new GUIStyle();
                style.fontStyle = FontStyle.Normal;
                style.wordWrap = true;
                style.alignment = TextAnchor.MiddleCenter;
                return style;
            }
        }

        public static GUIStyle WhiteLargeBoldLabel
        {
            get
            {
                GUIStyle style = new GUIStyle();
                style.alignment = TextAnchor.MiddleCenter;
                style.fontStyle = FontStyle.Bold;
                style.fontSize = 17;
                style.normal.textColor = Color.white;
                return style;
            }
        }

        public static GUIStyle LargeBoldLabel
        {
            get
            {
                GUIStyle style = new GUIStyle();
                style.fontStyle = FontStyle.Bold;
                style.fontSize = 12;
                return style;
            }
        }

        public static GUIStyle LargeCenteredGrayLabel
        {
            get
            {
                GUIStyle style = new GUIStyle();
                style.alignment = TextAnchor.MiddleCenter;
                style.fontStyle = FontStyle.Bold;
                style.fontSize = 12;
                style.normal.textColor = new Color32(70, 70, 70, 255);
                return style;
            }
        }

        public static GUIStyle LargeCenteredBoldLabel
        {
            get
            {
                GUIStyle style = new GUIStyle();
                style.alignment = TextAnchor.MiddleCenter;
                style.fontStyle = FontStyle.Bold;
                style.fontSize = 12;
                return style;
            }
        }

        public static GUIStyle MiniGrayLabel
        {
            get
            {
                GUIStyle style = new GUIStyle();
                style.fontSize = 9;
                style.normal.textColor = new Color32(70, 70, 70, 255);
                return style;
            }
        }

        public static GUIStyle CenteredMiniGrayLabel
        {
            get
            {
                GUIStyle style = new GUIStyle();
                style.fontSize = 9;
                style.alignment = TextAnchor.MiddleCenter;
                style.normal.textColor = new Color32(70, 70, 70, 255);
                return style;
            }
        }

        public static GUIStyle CenteredGrayBoldAndItalicLabel
        {
            get
            {
                GUIStyle style = new GUIStyle();
                style.fontSize = 9;
                style.alignment = TextAnchor.MiddleCenter;
                style.fontStyle = FontStyle.BoldAndItalic;
                style.normal.textColor = new Color32(90, 90, 90, 255);
                return style;
            }
        }

        public static GUIStyle BoldLabel
        {
            get
            {
                GUIStyle style = new GUIStyle();
                style.fontStyle = FontStyle.Bold;
                return style;
            }
        }

        public static GUIStyle LinkLabel
        {
            get
            {
                GUIStyle style = new GUIStyle();
                style.normal.textColor = new Color32(0, 0, 225, 255);
                return style;
            }
        }

        public static GUIStyle LinkLabelBold
        {
            get
            {
                GUIStyle style = new GUIStyle();
                style.fontStyle = FontStyle.Bold;
                style.normal.textColor = new Color32(0, 0, 225, 255);
                return style;
            }
        }

        public static GUIStyle ToolbarButton
        {
            get
            {
                GUIStyle style = new GUIStyle("ToolbarButton");
                style.fontSize = 10;
                return style;
            }
        }

        public static GUIStyle ToolbarButtonBold
        {
            get
            {
                GUIStyle style = new GUIStyle("ToolbarButton");
                style.fontSize = 10;
                style.fontStyle = FontStyle.Bold;
                return style;
            }
        }

        public static GUIStyle TextAreaWrap
        {
            get
            {
                GUIStyle style = new GUIStyle(GUI.skin.textArea);
                style.wordWrap = true;
                return style;
            }
        }
    }
}