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
    [CustomEditor(typeof(WeaponSequentialReloadSystem))]
    public class WeaponSequentialReloadSystemEditor : AuroraEditor<WeaponSequentialReloadSystem>
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
            public readonly static GUIContent SmartReloading = new GUIContent("Smart Reloading", "Reloading will be available only when bullet count is empty.");
            public readonly static GUIContent AutoReloading = new GUIContent("Auto Reloading", "Reloading will start automatically when bullet count is empty.");

            public readonly static GUIContent AnimationProperties = new GUIContent("Animation Properties");
            public readonly static GUIContent Start = new GUIContent("Strat", "Start reloading properties");
            public readonly static GUIContent StartReloadTime = new GUIContent("Time", "Start reload time.");
            public readonly static GUIContent StartReloadState = new GUIContent("Start State", "Start reload state.");
            public readonly static GUIContent Iteration = new GUIContent("Iteration", "Iteration reloading properties.");
            public readonly static GUIContent IterationReloadTime = new GUIContent("Time", "Iteration reload time.");
            public readonly static GUIContent IterationReloadState = new GUIContent("Iteration State", "Iteration reload state.");
            public readonly static GUIContent End = new GUIContent("End", "End reloading properties.");
            public readonly static GUIContent EndReloadTime = new GUIContent("Time", "End reload time.");
            public readonly static GUIContent EndReloadState = new GUIContent("End State", "End reload state.");
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
            instance.SmartReloading(EditorGUILayout.Toggle(ContentProperties.SmartReloading, instance.SmartReloading()));
            instance.AutoReloading(EditorGUILayout.Toggle(ContentProperties.AutoReloading, instance.AutoReloading()));
            OnEventsGUI();
            EndGroup();

            BeginGroup(ContentProperties.AnimationProperties);
            instance.SetStartState(AEditorGUILayout.AnimatorStateField(ContentProperties.StartReloadState, instance.GetStartState()));
            instance.SetIterationState(AEditorGUILayout.AnimatorStateField(ContentProperties.IterationReloadState, instance.GetIterationState()));
            instance.SetEndState(AEditorGUILayout.AnimatorStateField(ContentProperties.EndReloadState, instance.GetEndState()));
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
                        case "Start":
                            instance.SetStartTime(clip.length);
                            break;
                        case "Iteration":
                            instance.SetIterationTime(clip.length);
                            break;
                        case "End":
                            instance.SetEndTime(clip.length);
                            break;
                    }
                });
            }
            genericMenu.ShowAsContext();
        }
    }
}