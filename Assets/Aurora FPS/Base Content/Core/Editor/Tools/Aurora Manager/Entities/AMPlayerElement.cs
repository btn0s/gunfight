/* ==================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================== */

using System.Collections.Generic;
using AuroraFPSRuntime;
using UnityEditor;
using UnityEngine;

namespace AuroraFPSEditor.Window.Manager
{
    [TreeElement("Player", "General", 0)]
    [TreeElementTarget(typeof(AuroraManager))]
    internal sealed class AMPlayerElement : TreeElementEditor
    {
        public const string TemplatesPath = "Editor/Templates/GameObject/Player";

        public static class ContentProperties
        {
            public readonly static GUIContent Template = new GUIContent("Template", "Select player template for create it.");
            public readonly static GUIContent DefaultInputMapping = new GUIContent("Input Mapping", "Input mapping asset.");
            public readonly static GUIContent FirstPersonBody = new GUIContent("First Person Body");
            public readonly static GUIContent RemoteBody = new GUIContent("Remote Body");
            public readonly static GUIContent CustomBody = new GUIContent("Body");
        }

        // Player editable settings.
        private DefaultInputMapping inputMapping;

        // Stored required properties.
        private TemplateEditor templateEditor;
        private OptionalComponentsEditor optionalComponents;
        private AuroraManager auroraManagerWindow;

        /// <summary>
        /// Called once when tree element initialized.
        /// </summary>
        /// <param name="id">Unique option id.</param>
        public AMPlayerElement(int id) : base(id)
        {
            templateEditor = new TemplateEditor();
            templateEditor.GroupTagsInFoldout(true);
            templateEditor.InitializeTemplateEditor(TemplatesPath);

            optionalComponents = new OptionalComponentsEditor(
                typeof(FPHealth),
                typeof(FPInventory),
                typeof(FPFootstepSoundSystem),
                typeof(GrabbingSystem)
            );
        }

        /// <summary>
        /// Called when the manager becomes enabled and active.
        /// Initializing base ManagerItem properties.
        /// </summary>
        public override void OnManagerEnable()
        {
            auroraManagerWindow = EditorWindow.GetWindow<AuroraManager>();
        }

        /// <summary>
        /// OnBaseGUI is called for rendering and handling GUI events.
        /// This function can be called multiple times per frame (one call per event).
        /// </summary>
        public override void OnBaseGUI()
        {
            bool hasErrors = false;

            templateEditor.DrawPopup();

            inputMapping = AEditorGUILayout.ObjectField(ContentProperties.DefaultInputMapping, inputMapping, true);
            if (inputMapping == null)
            {
                hasErrors = true;
            }

            templateEditor.DrawTags();
            optionalComponents.DrawOptions();

            GUILayout.Space(5);

            EditorGUI.BeginDisabledGroup(hasErrors || !templateEditor.HasTemplates());
            DrawCreateButton();
            EditorGUI.EndDisabledGroup();
        }

        /// <summary>
        /// Create player button GUI layout.
        /// </summary>
        /// <param name="enabled">Button is enabled state.</param>
        private void DrawCreateButton()
        {
            if (AEditorGUILayout.ButtonRight("Create", GUILayout.Width(105.5f)))
            {
                GameObject player = templateEditor.InstantiateSelectedTemplate();
                player.GetComponent<DefaultInputController>().SetDefaultInputMapping(inputMapping);
                optionalComponents.ApplyOptions(player);
            }
        }
    }
}