/* ==================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================== */

using AuroraFPSRuntime;
using AuroraFPSRuntime.AI;
using UnityEditor;
using UnityEngine;

namespace AuroraFPSEditor.Window.Manager
{
    [TreeElement("Artificial Intelligence", "Artificial Intelligence", 101)]
    [TreeElementTarget(typeof(AuroraManager))]
    internal sealed class AMAIElement : TreeElementEditor
    {
        public const string TemplatesPath = "Editor/Templates/GameObject/AI";

        private string name;
        private GameObject body;
        private Avatar avatar;
        private bool hasRagdoll;
        private string tag;
        private LayerMask layer;
        private int count;
        private TemplateEditor templateEditor;
        private OptionalComponentsEditor optionalComponents;

        public AMAIElement(int id) : base(id)
        {
            name = "AI Character";
            tag = "Untagged";

            templateEditor = new TemplateEditor();
            templateEditor.SetExcludeTemplateSubstring(" [AI Template]");
            templateEditor.GroupTagsInFoldout(true);
            templateEditor.InitializeTemplateEditor(TemplatesPath);

            optionalComponents = new OptionalComponentsEditor(
                typeof(AIHealth),
                typeof(AIFieldOfView),
                typeof(AIAdaptiveRagdoll),
                typeof(AIFootstepSoundSystem),
                typeof(AIItemDropper)
            );
            count = 1;
        }

        public override void OnBaseGUI()
        {
            templateEditor.DrawPopup();
            name = EditorGUILayout.TextField("Name", name);
            body = AEditorGUILayout.ObjectField("Body", body, true);
            avatar = AEditorGUILayout.ObjectField("Avatar", avatar, true);
            tag = EditorGUILayout.TagField("Tag", tag);
            layer = EditorGUILayout.LayerField("Layer", layer);
            hasRagdoll = EditorGUILayout.Toggle("Has Ragdoll", hasRagdoll);
            count = AEditorGUILayout.FixedIntField("Count", count, 1);
            templateEditor.DrawTags();
            optionalComponents.DrawOptions();
            GUILayout.Space(5);
            EditorGUI.BeginDisabledGroup(!templateEditor.HasTemplates());
            DrawCreateButton();
            EditorGUI.EndDisabledGroup();
        }

        public void DrawCreateButton()
        {
            if (AEditorGUILayout.ButtonRight("Create", GUILayout.Width(105.5f)))
            {
                GameObject ai = templateEditor.InstantiateSelectedTemplate();
                ai.name = count > 1 ? string.Format("{0} [{1}]", name, 1) : name;
                ai.tag = tag;
                ai.layer = layer.value;

                if (body != null)
                {
                    RemoveAllChilds(ai);

                    GameObject bodyInstance = GameObject.Instantiate(body);
                    ReplaceAllChilds(bodyInstance, ai, true);
                }

                if (avatar != null)
                {
                    Animator animator = ai.GetComponent<Animator>();
                    if (animator != null)
                    {
                        animator.avatar = avatar;
                    }
                }

                optionalComponents.ApplyOptions(ai);

                if (!hasRagdoll)
                {
                    AIAdaptiveRagdoll adaptiveRagdoll = ai.GetComponent<AIAdaptiveRagdoll>();
                    if (adaptiveRagdoll != null)
                    {
                        GameObject.DestroyImmediate(adaptiveRagdoll);
                    }
                }

                for (int i = 1; i < count; i++)
                {
                    GameObject clone = GameObject.Instantiate(ai);
                    clone.name = string.Format("{0} [{1}]", name, i + 1);
                }
            }
        }

        private static void ReplaceAllChilds(GameObject reference, GameObject target, bool destroyReference = false)
        {
            while (reference.transform.childCount > 0)
            {
                foreach (Transform child in reference.transform)
                {
                    child.SetParent(target.transform);
                    child.localPosition = Vector3.zero;
                    child.localRotation = Quaternion.identity;
                }
            }

            if (destroyReference)
            {
                GameObject.DestroyImmediate(reference);
            }
        }

        private static void RemoveAllChilds(GameObject target)
        {
            while (target.transform.childCount > 0)
            {
                foreach (Transform child in target.transform)
                {
                    GameObject.DestroyImmediate(child.gameObject);
                }
            }
        }
    }
}