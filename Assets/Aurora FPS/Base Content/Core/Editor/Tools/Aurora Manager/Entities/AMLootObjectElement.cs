/* ==================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================== */

using AuroraFPSRuntime;
using UnityEditor;
using UnityEngine;

namespace AuroraFPSEditor.Window.Manager
{
    [TreeElement("Loot Object", "Interactive Object", 0)]
    [TreeElementTarget(typeof(AuroraManager))]
    internal sealed class AMLootObjectElement : TreeElementEditor
    {
        public const string TemplatesPath = "Editor/Templates/GameObject/Loot Object";

        // Base loot object properties.
        private TemplateEditor templateEditor;
        private OptionalComponentsEditor optionalComponents;
        private string name;
        private LayerMask layer;
        private int count;

        /// <summary>
        /// Called once when tree element initialized.
        /// </summary>
        /// <param name="id">Unique option id.</param>
        public AMLootObjectElement(int id) : base(id)
        {
            templateEditor = new TemplateEditor();
            templateEditor.SetExcludeTemplateSubstring(" [LootObject Template]");
            templateEditor.GroupTagsInFoldout(true);
            templateEditor.InitializeTemplateEditor(TemplatesPath);
            templateEditor.OnTagReplacedCallback += ReplaceMesh;

            optionalComponents = new OptionalComponentsEditor(
                typeof(Rigidbody),
                typeof(LootObjectMotion),
                typeof(BoxCollider),
                typeof(SphereCollider),
                typeof(CapsuleCollider),
                typeof(MeshCollider));

            name = "Loot Object";
            layer = LayerMask.NameToLayer("Interactive Object");
            count = 1;
        }

        /// <summary>
        /// OnBaseGUI is called for rendering and handling GUI events.
        /// This function can be called multiple times per frame (one call per event).
        /// </summary>
        public override void OnBaseGUI()
        {
            templateEditor.DrawPopup();
            name = EditorGUILayout.TextField("Name", name);
            layer = EditorGUILayout.LayerField("Layer", layer);
            count = AEditorGUILayout.FixedIntField("Count", count, 1);
            templateEditor.DrawTags();
            optionalComponents.DrawOptions();

            GUILayout.Space(5);

            EditorGUI.BeginDisabledGroup(!templateEditor.HasTemplates());
            DrawCreateButton();
            EditorGUI.EndDisabledGroup();
        }

        private void DrawCreateButton()
        {
            if (AEditorGUILayout.ButtonRight("Create", GUILayout.Width(105.5f)))
            {
                GameObject template = templateEditor.InstantiateSelectedTemplate();
                template.name = count > 1 ? string.Format("{0} [{1}]", name, 1) : name;
                template.transform.SetLayerRecursively(layer);

                optionalComponents.ApplyOptions(template);

                if(template.GetComponent<Collider>() == null)
                {
                    template.AddComponent<BoxCollider>();
                }

                for (int i = 1; i < count; i++)
                {
                    GameObject clone = GameObject.Instantiate(template);
                    clone.transform.position += Vector3.right * i;
                    clone.name = string.Format("{0} [{1}]", name, i + 1);
                }
            }
        }

        private void ReplaceMesh(GameObject target, GameObject oldTag, GameObject newTag)
        {
            LootObject lootObject = target.GetComponent<LootObject>();
            if (lootObject != null && lootObject.GetObjectMesh().name == oldTag.name)
            {
                lootObject.SetObjectMesh(newTag);
            }
        }
    }
}