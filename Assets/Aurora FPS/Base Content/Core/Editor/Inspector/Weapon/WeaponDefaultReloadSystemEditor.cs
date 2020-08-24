/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using AuroraFPSRuntime;
using UnityEditor;
using UnityEngine;

namespace AuroraFPSEditor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(WeaponDefaultReloadSystem))]
    public class WeaponDefaultReloadSystemEditor : AuroraEditor<WeaponDefaultReloadSystem>
    {
        private const string ActionButtonTimeTooltip = "Load animation time from Animator component animations.";

        internal new static class ContentProperties
        {
            public readonly static GUIContent AmmoProperties = new GUIContent("Ammo Properties");
            public readonly static GUIContent BulletCount = new GUIContent("Bullet Count", "Weapon bullet count.");
            public readonly static GUIContent ClipCount = new GUIContent("Clip Count", "Weapon clip count.");
            public readonly static GUIContent MaxBulletCount = new GUIContent("Max Bullet Count", "Max available weapon bullet count per clip.");
            public readonly static GUIContent MaxClipCount = new GUIContent("Max Clip Count", "Max available clip count for this weapon.");

            public readonly static GUIContent ReloadProperties = new GUIContent("Reload Properties");
            public readonly static GUIContent ReloadType = new GUIContent("Reload Type", "Default: Saves the number of bullets in the rechargeable clip.\nRealistic: Not saves the number of bullets in the rechargeable clip.");
            public readonly static GUIContent SmartReloading = new GUIContent("Smart Reloading", "Reloading will be available only when bullet count less then max bullet count.");
            public readonly static GUIContent AutoReloading = new GUIContent("Auto Reloading", "Reloading will start automatically when bullet count is empty.");

            public readonly static GUIContent AnimationProperties = new GUIContent("Animation Properties");
            public readonly static GUIContent BaseReload = new GUIContent("Base Reload", "Base reloading properties");
            public readonly static GUIContent FullReload = new GUIContent("Full Reload", "Full reloading properties.");
        }

        private SerializedProperty ammoCountProperty;
        private SerializedProperty clipCountProperty;

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        public override void InitializeProperties()
        {
            ammoCountProperty = serializedObject.FindProperty("ammoCount");
            clipCountProperty = serializedObject.FindProperty("clipCount");
        }

        /// <summary>
        /// Base serializedObject properties.
        /// 
        /// Implement this method to draw a custom properties.
        /// </summary>
        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.AmmoProperties);
            ammoCountProperty.intValue = EditorGUILayout.IntSlider(ContentProperties.BulletCount, ammoCountProperty.intValue, 0, instance.GetMaxAmmoCount());
            clipCountProperty.intValue = EditorGUILayout.IntSlider(ContentProperties.ClipCount, clipCountProperty.intValue, 0, instance.GetMaxClipCount());
            instance.SetMaxAmmoCount(AEditorGUILayout.DelayedFixedIntField(ContentProperties.MaxBulletCount, instance.GetMaxAmmoCount(), 0));
            instance.SetMaxClipCount(AEditorGUILayout.DelayedFixedIntField(ContentProperties.MaxClipCount, instance.GetMaxClipCount(), 0));
            EndGroup();

            BeginGroup(ContentProperties.ReloadProperties);
            instance.SetReloadType((WeaponDefaultReloadSystem.ReloadType) EditorGUILayout.EnumPopup(ContentProperties.ReloadType, instance.GetReloadType()));
            instance.SmartReloading(EditorGUILayout.Toggle(ContentProperties.SmartReloading, instance.SmartReloading()));
            instance.AutoReloading(EditorGUILayout.Toggle(ContentProperties.AutoReloading, instance.AutoReloading()));
            OnEventsGUI();
            EndGroup();

            BeginGroup(ContentProperties.AnimationProperties);
            instance.SetBaseReloadState(AEditorGUILayout.AnimatorStateField(ContentProperties.BaseReload, instance.GetBaseReloadState()));
            instance.SetFullReloadState(AEditorGUILayout.AnimatorStateField(ContentProperties.FullReload, instance.GetFullReloadState()));
            EndGroup();
        }

        /// <summary>
        /// Generate generic menu function and load clip time.
        /// </summary>
        protected virtual void LoadAnimationTime(object actionData)
        {
            string target = actionData.ToString();

            Animator animator = instance.GetComponent<Animator>();
            if (animator == null)
            {
                Debug.Log("The instance does not contain a Animator component.");
                return;
            }

            if (animator.runtimeAnimatorController == null)
            {
                Debug.Log("Animator component dose not contain AnimatorController.");
                return;
            }

            AnimationClip[] clips = EditorHelper.GetAllClips(animator);

            GenericMenu genericMenu = new GenericMenu();
            for (int i = 0, length = clips.Length; i < length; i++)
            {
                AnimationClip clip = clips[i];
                genericMenu.AddItem(new GUIContent(clip.name), false, () =>
                {
                    switch (target)
                    {
                        case "Base":
                            instance.SetBaseReloadTime(clip.length);
                            break;
                        case "Full":
                            instance.SetFullReloadTime(clip.length);
                            break;
                    }
                });
            }
            genericMenu.ShowAsContext();
        }
    }
}