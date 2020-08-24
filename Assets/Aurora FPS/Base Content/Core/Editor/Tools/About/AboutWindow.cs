/* ==================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================== */

using System;
using UnityEditor;
using UnityEngine;

namespace AuroraFPSEditor
{
    internal sealed class AboutWindow : EditorWindow
    {
        /// <summary>
        /// Open About window.
        /// </summary>
        [MenuItem("Aurora FPS/About", false, 0)]
        public static void Open()
        {
            AboutWindow window = ScriptableObject.CreateInstance(typeof(AboutWindow)) as AboutWindow;
            window.titleContent = new GUIContent("About");
            window.maxSize = new Vector2(475, 227);
            window.minSize = new Vector2(475, 227);
            window.ShowUtility();
        }

        /// <summary>
        /// OnGUI is called for rendering and handling GUI events.
        /// </summary>
        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(4);
            GUILayout.BeginVertical();
            AuroraEditor.BeginGroup(AuroraFPSRuntime.AuroraFPSProduct.Name);
            DefaultLabelContent("Product:", AuroraFPSRuntime.AuroraFPSProduct.Name);
            DefaultLabelContent("Release:", AuroraFPSRuntime.AuroraFPSProduct.Release);
            DefaultLabelContent("Publisher:", AuroraFPSRuntime.AuroraFPSProduct.Publisher);
            DefaultLabelContent("Author:", AuroraFPSRuntime.AuroraFPSProduct.Author);
            DefaultLabelContent("Version:", AuroraFPSRuntime.AuroraFPSProduct.Version);
            GUILayout.Label(AuroraFPSRuntime.AuroraFPSProduct.Copyright, AEditorStyles.BoldLabel);

            GUILayout.Space(20);

            OpenSupportButton("For get full informations about " + AuroraFPSRuntime.AuroraFPSProduct.Name + " see - ", "Documentation", UserHelper.OpenDocumentation);
            OpenSupportButton("To keep abreast of all the new news, follow us on - ", "Twitter", UserHelper.OfficialTwitterURL);
            OpenSupportButton("If you have any questions you can ask them in the - ", "Official Thread", UserHelper.OfficialThreadURL);
            SelectableContent("Official support - ", UserHelper.OfficialEmail);
            AuroraEditor.EndGroup();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        private static void DefaultLabelContent(string title, string message = "", float space = 3.0f, float width = 65)
        {
            GUILayout.BeginHorizontal(GUILayout.Height(1));
            GUILayout.Label(title, AEditorStyles.BoldLabel, GUILayout.Width(width));
            GUILayout.Space(space);
            GUILayout.Label(message, AEditorStyles.BoldLabel);
            GUILayout.EndHorizontal();
            GUILayout.Space(2.5f);
        }

        public static void SelectableContent(string content, string selectableContent)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(content, AEditorStyles.BoldLabel, GUILayout.Height(17));
            EditorGUILayout.SelectableLabel(selectableContent, AEditorStyles.LinkLabelBold, GUILayout.Height(17));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        public static void OpenSupportButton(string content, string buttonContent, string url)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(content, AEditorStyles.BoldLabel, GUILayout.Height(17));
            if (GUILayout.Button(buttonContent, AEditorStyles.LinkLabelBold, GUILayout.Height(17)))
                Application.OpenURL(url);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        public static void OpenSupportButton(string content, string buttonContent, Action buttonAction)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(content, AEditorStyles.BoldLabel, GUILayout.Height(17));
            if (GUILayout.Button(buttonContent, AEditorStyles.LinkLabelBold, GUILayout.Height(17)))
                buttonAction.Invoke();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }
}