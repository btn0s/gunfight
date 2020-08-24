/* ==================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================== */

using AuroraFPSRuntime;
using AuroraFPSRuntime.UI;
using UnityEditor;
using UnityEngine;

namespace AuroraFPSEditor.Window.Manager
{
    [TreeElement("Weapon", "General", 1)]
    [TreeElementTarget(typeof(AuroraManager))]
    internal class AMWeaponElement : TreeElementEditor
    {
        public const string TemplatesPath = "Editor/Templates/GameObject/Weapon";

        public static class ContentProperties
        {
            public readonly static GUIContent WeaponItem = new GUIContent("Weapon Item", "Weapon item target.");
            public readonly static GUIContent ShootingSystem = new GUIContent("Shooting System");
        }

        public enum ShootingSystem
        {
            Raycast,
            Physics
        }

        // Weapon editable settings.
        private WeaponItem weaponItem;
        private ShootingSystem shootingSystem;
        private TemplateEditor templateEditor;

        // Stored required properties.
        private AuroraManager auroraManagerWindow;
        private OptionalComponentsEditor optionalComponents;

        /// <summary>
        /// Called once when tree element initialized.
        /// </summary>
        /// <param name="id">Unique option id.</param>
        public AMWeaponElement(int id) : base(id)
        {
            templateEditor = new TemplateEditor();
            templateEditor.GroupTagsInFoldout(true);
            templateEditor.InitializeTemplateEditor(TemplatesPath);

            optionalComponents = new OptionalComponentsEditor(
                typeof(Crosshair),
                typeof(WeaponAnimationEventHandler)
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
        /// OnPropertiesGUI is called for rendering and handling GUI events.
        /// This function can be called multiple times per frame (one call per event).
        /// </summary>
        public override void OnBaseGUI()
        {
            templateEditor.DrawPopup();
            shootingSystem = AEditorGUILayout.EnumPopup(ContentProperties.ShootingSystem, shootingSystem);
            weaponItem = AEditorGUILayout.ObjectField(ContentProperties.WeaponItem, weaponItem, true);
            templateEditor.DrawTags();
            optionalComponents.DrawOptions();

            GUILayout.Space(5);

            EditorGUI.BeginDisabledGroup(weaponItem == null || !templateEditor.HasTemplates());
            DrawCreateButton();
            EditorGUI.EndDisabledGroup();
        }

        public void DrawCreateButton()
        {
            if (AEditorGUILayout.ButtonRight("Create", GUILayout.Width(105.5f)))
            {
                GameObject weapon = templateEditor.InstantiateSelectedTemplate();
                weapon.transform.SetLayerRecursively(LayerMask.NameToLayer(LNC.Weapon));
                optionalComponents.ApplyOptions(weapon);
                WeaponIdentifier weaponIdentifier = weapon.GetComponent<WeaponIdentifier>();
                weaponIdentifier.SetWeaponItem(weaponItem);

                WeaponShootingSystem weaponShootingSystem = weapon.GetComponent<WeaponShootingSystem>();
                switch (shootingSystem)
                {
                    case ShootingSystem.Raycast:
                        if (!(weaponShootingSystem is WeaponRayShootingSystem))
                        {
                            WeaponRayShootingSystem weaponRayShooting = weapon.AddComponent<WeaponRayShootingSystem>();
                            Object.DestroyImmediate(weaponShootingSystem);
                        }
                        break;
                    case ShootingSystem.Physics:
                        if (!(weaponShootingSystem is WeaponPhysicsShootingSystem))
                        {
                            WeaponPhysicsShootingSystem weaponRayShooting = weapon.AddComponent<WeaponPhysicsShootingSystem>();
                            Object.DestroyImmediate(weaponShootingSystem);
                        }
                        break;
                }
                EditorGUIUtility.PingObject(weapon);
            }
        }
    }
}