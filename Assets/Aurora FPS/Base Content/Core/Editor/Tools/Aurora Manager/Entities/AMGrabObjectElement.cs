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

namespace AuroraFPSEditor.Window.Manager
{
    [TreeElement("Grab Object", "Interactive Object", 1)]
    [TreeElementTarget(typeof(AuroraManager))]
    internal sealed class AMGrabObjectElement : TreeElementEditor
    {
        public const string TemplatesPath = "Editor/Templates/GameObject/Grab Object";

        [SerializeField] TemplateEditor templates;
        [SerializeField] private string name;
        [SerializeField] private LayerMask layer;
        [SerializeField] private int count;

        /// <summary>
        /// Called once when tree element initialized.
        /// </summary>
        /// <param name="id">Unique option id.</param>
        public AMGrabObjectElement(int id) : base(id)
        {
            templates = new TemplateEditor();
            templates.SetExcludeTemplateSubstring(" [GrabObject Template]");
            templates.GroupTagsInFoldout(true);
            templates.InitializeTemplateEditor(TemplatesPath);

            name = "Grab Object";
            layer = LayerMask.NameToLayer("Interactive Object");
            count = 1;
        }

        /// <summary>
        /// OnBaseGUI is called for rendering and handling GUI events.
        /// This function can be called multiple times per frame (one call per event).
        /// </summary>
        public override void OnBaseGUI()
        {
            templates.DrawPopup();
            name = EditorGUILayout.TextField("Name", name);
            layer = EditorGUILayout.LayerField("Layer", layer);
            count = AEditorGUILayout.FixedIntField("Count", count, 1);
            templates.DrawTags();
            DrawCreateButton();
        }

        public void DrawCreateButton()
        {
            if (AEditorGUILayout.ButtonRight("Create", GUILayout.Width(105.5f)))
            {
                GameObject grabObject = templates.InstantiateSelectedTemplate();
                grabObject.name = count > 1 ? string.Format("{0} [{1}]", name, 1) : name;
                grabObject.transform.SetLayerRecursively(layer);

                for (int i = 1; i < count; i++)
                {
                    GameObject clone = GameObject.Instantiate(grabObject);
                    clone.transform.position += Vector3.right * i;
                    clone.name = string.Format("{0} [{1}]", name, i + 1);
                }
            }
        }
    }
}