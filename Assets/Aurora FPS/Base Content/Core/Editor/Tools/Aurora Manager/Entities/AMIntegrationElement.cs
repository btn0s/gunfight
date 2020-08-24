/* ==================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================== */

using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace AuroraFPSEditor.Window.Manager
{
    [TreeElement("Integrations", "Integration", 901)]
    [TreeElementTarget(typeof(AuroraManager))]
    public class AMIntegrationElement : TreeElementEditor
    {
        private const string IntegrationPath = "Assets/Aurora FPS/Base Content/Resources/Editor/Integrations";
        private const string IntegrationExtension = ".unitypackage";

        private ReorderableList integrationList;

        /// <summary>
        /// Called once when tree element initialized.
        /// </summary>
        /// <param name="id">Unique option id.</param>
        public AMIntegrationElement(int id) : base(id)
        {
            integrationList = CreateIntegrationList(FindIntegrations());
        }

        /// <summary>
        /// OnBaseGUI is called for rendering and handling GUI events.
        /// This function can be called multiple times per frame (one call per event).
        /// </summary>
        public override void OnBaseGUI()
        {
            integrationList.DoLayoutList();
        }

        public ReorderableList CreateIntegrationList(FileInfo[] integrations)
        {
            ReorderableList list = new ReorderableList(integrations, typeof(FileInfo), false, false, false, false);
            list.headerHeight = 3;

            list.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                FileInfo integration = integrations[index];

                const float buttonWidth = 50;

                Rect position = new Rect(rect.x, rect.y + 1.0f, rect.width, EditorGUIUtility.singleLineHeight);

                Rect labelPosition = new Rect(position.x, position.y, position.width - buttonWidth, EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(labelPosition, Path.GetFileNameWithoutExtension(integration.Name));

                Rect buttonPosition = new Rect(labelPosition.x + labelPosition.width, position.y, buttonWidth, EditorGUIUtility.singleLineHeight);
                if (GUI.Button(buttonPosition, "Install"))
                    AssetDatabase.ImportPackage(integration.FullName, true);
            };
            return list;
        }

        public FileInfo[] FindIntegrations()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(IntegrationPath);
            return directoryInfo.GetFiles().Where(t => t.Extension == IntegrationExtension).ToArray();
        }
    }
}