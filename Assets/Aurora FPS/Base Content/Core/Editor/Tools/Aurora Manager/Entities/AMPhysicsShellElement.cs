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
    [TreeElement("Physics Shell", "General", 2)]
    [TreeElementTarget(typeof(AuroraManager))]
    internal sealed class AMPhysicsShellElement : TreeElementEditor
    {
        public const string TemplatesPath = "Editor/Templates/GameObject/Physics Bullet";

        private BulletItem bulletItem;
        private TemplateEditor templateEditor;

        /// <summary>
        /// Called once when tree element initialized.
        /// </summary>
        /// <param name="id">Unique option id.</param>
        public AMPhysicsShellElement(int id) : base(id)
        {
            templateEditor = new TemplateEditor();
            templateEditor.SetExcludeTemplateSubstring(" [PhysicsShell Template]");
            templateEditor.GroupTagsInFoldout(true);
            templateEditor.InitializeTemplateEditor(TemplatesPath);
        }

        /// <summary>
        /// OnBaseGUI is called for rendering and handling GUI events.
        /// This function can be called multiple times per frame (one call per event).
        /// </summary>
        public override void OnBaseGUI()
        {
            templateEditor.DrawPopup();
            bulletItem = AEditorGUILayout.ObjectField("Bullet Item", bulletItem, false);
            templateEditor.DrawTags();

            EditorGUI.BeginDisabledGroup(!templateEditor.HasTemplates() || bulletItem == null);
            DrawCreateButtom();
            EditorGUI.EndDisabledGroup();
        }

        public void DrawCreateButtom()
        {
            if(AEditorGUILayout.ButtonRight("Create", GUILayout.Width(105.5f)))
            {
                GameObject bullet = templateEditor.InstantiateSelectedTemplate();
                bullet.name = string.Format("{0} [Physics {1}]", bulletItem.GetDisplayName(), templateEditor.GetSelectedTemplateName());
                PhysicsBullet physicsBulletComponent = bullet.GetComponent<PhysicsBullet>();
                physicsBulletComponent.SetShellItem(bulletItem);
            }
        }
    }
}