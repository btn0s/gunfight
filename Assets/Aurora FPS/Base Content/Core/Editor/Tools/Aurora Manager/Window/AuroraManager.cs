/* ==================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================== */

using UnityEditor;
using UnityEngine;

namespace AuroraFPSEditor.Window.Manager
{
    public sealed class AuroraManager : ManagerWindow
    {
        // Tree element header label properties.
        private string elementHeaderLabel;

        /// <summary>
        /// Create and open AuroraManager editor window.
        /// </summary>
        [MenuItem(EditorPaths.MenuItem + "Aurora Manager", false, 91)]
        public static void Open()
        {
            CreateManagerWindow<AuroraManager>("Aurora Manager");
        }

        public override void IntitalizeProperties()
        {
            OnSelectedTreeElementCallback += UpdateTreeElementHeader;
        }

        public override void OnTreeElementGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label(elementHeaderLabel, new GUIStyle(EditorStyles.boldLabel) { fontSize = 17 });
            GUILayout.Space(7);
            base.OnTreeElementGUI();
            GUILayout.EndVertical();
            GUILayout.Space(5);
            GUILayout.EndHorizontal();
        }

        private void UpdateTreeElementHeader(int id)
        {
            string name = managerTree.FindTreeViewItem(id)?.displayName;
            if (name == null || name == "Root")
                name = string.Empty;
            elementHeaderLabel = name;
        }
    }
}