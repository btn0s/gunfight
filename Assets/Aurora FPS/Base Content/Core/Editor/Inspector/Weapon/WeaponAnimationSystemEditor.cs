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
    //[CustomEditor(typeof(WeaponAnimationSystem))]
    public class WeaponAnimationSystemEditor : AuroraEditor<WeaponAnimationSystem>
    {
        private const string ActionButtomTimeTooltip = "Load time from Animator component animations.";
        private const string ActionButtonNameTooltip = "Load parameter name from Animator component.";

        internal new static class ContentProperties
        {
            public readonly static GUIContent TimeProperties = new GUIContent("Time Properties");
            public readonly static GUIContent TakeInTime = new GUIContent("Take In", "Take in time weapon from inventory.");
            public readonly static GUIContent TakeOutTime = new GUIContent("Take Out", "Take out time weapon in inventory.");

            public readonly static GUIContent AnimationProperites = new GUIContent("Animation Properties");
            public readonly static GUIContent Parameters = new GUIContent("Parameters");
            public readonly static GUIContent SpeedParameter = new GUIContent("Speed", "Speed parameter name in animator controller.");
            public readonly static GUIContent ZoomParameter = new GUIContent("Zoom", "Zoom parameter name in animator controller.");
            public readonly static GUIContent IsGroundedParameter = new GUIContent("IsGrounded", "IsGrounded parameter name in animator controller.");
            public readonly static GUIContent IsCrouchingParameter = new GUIContent("IsCrouching", "IsCrouching parameter name in animator controller.");
            public readonly static GUIContent TakeOutParameter = new GUIContent("TakeOut", "TakeOut parameter name in animator controller.");
            public readonly static GUIContent Other = new GUIContent("Other");
            public readonly static GUIContent SpeedBlendTime = new GUIContent("Speed Blend Time", "Speed value blend time.");

            public readonly static GUIContent SwayProperties = new GUIContent("Sway Properties");
            public readonly static GUIContent SwaySystem = new GUIContent("Sway System");
            public readonly static GUIContent Position = new GUIContent("Position");
            public readonly static GUIContent PositionSensitivity = new GUIContent("Sensitivity", "Position sway sensitivity.");
            public readonly static GUIContent PositionLimit = new GUIContent("Limit", "Position sway min/max limit.");
            public readonly static GUIContent PositionSmooth = new GUIContent("Smooth", "Position sway smooth.");

            public readonly static GUIContent Rotation = new GUIContent("Rotation");
            public readonly static GUIContent RotationSensitivity = new GUIContent("Sensitivity", "Rotation sway sensitivity.");
            public readonly static GUIContent RotationLimit = new GUIContent("Limit", "Rotation sway min/max limit.");
            public readonly static GUIContent RotationSmooth = new GUIContent("Smooth", "Rotation sway smooth.");

            public readonly static GUIContent Vertical = new GUIContent("Vertical");
            public readonly static GUIContent VerticalVelocitySensitivity = new GUIContent("Sensitivity", "Vertical velocity sway sensitivity.");
            public readonly static GUIContent InvertVerticalSway = new GUIContent("Invert", "Invert vertical sway.");

            public readonly static GUIContent UseSway = new GUIContent("Use Sway System");
        }

        private bool swaySystemFoldout;

        /// <summary>
        /// Base serializedObject properties.
        /// 
        /// Implement this method to draw a custom properties.
        /// </summary>
        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.TimeProperties);
            instance.SetTakeInTime(AEditorGUILayout.ActionFloatField(ContentProperties.TakeInTime, instance.GetTakeInTime(), LoadAnimationTime, "TakeIn", ActionButtomTimeTooltip));
            instance.SetTakeOutTime(AEditorGUILayout.ActionFloatField(ContentProperties.TakeOutTime, instance.GetTakeOutTime(), LoadAnimationTime, "TakeOut", ActionButtomTimeTooltip));
            EndGroup();

            BeginGroup(ContentProperties.AnimationProperites);
            InsideGroupHeader(ContentProperties.Parameters);

            AnimatorValue parameterValue = AnimatorValue.none;

            parameterValue = instance.GetSpeedParameter();
            parameterValue.SetName(AEditorGUILayout.ActionTextField(ContentProperties.SpeedParameter, parameterValue.GetName(), LoadStateName, "Speed", ActionButtonNameTooltip));
            instance.SetSpeedParameter(parameterValue);

            parameterValue = instance.GetIsGroundedParameter();
            parameterValue.SetName(AEditorGUILayout.ActionTextField(ContentProperties.IsGroundedParameter, parameterValue.GetName(), LoadStateName, "IsGrounded", ActionButtonNameTooltip));
            instance.SetIsGroundedParameter(parameterValue);

            parameterValue = instance.GetIsCrouchingParameter();
            parameterValue.SetName(AEditorGUILayout.ActionTextField(ContentProperties.IsCrouchingParameter, parameterValue.GetName(), LoadStateName, "IsCrouched", ActionButtonNameTooltip));
            instance.SetIsCrouchingParameter(parameterValue);

            parameterValue = instance.GetTakeOutParameter();
            parameterValue.SetName(AEditorGUILayout.ActionTextField(ContentProperties.TakeOutParameter, parameterValue.GetName(), LoadStateName, "TakeOut", ActionButtonNameTooltip));
            instance.SetTakeOutParameter(parameterValue);

            GUILayout.Space(5);

            InsideGroupHeader(ContentProperties.Other);
            instance.SetSpeedBlendTime(EditorGUILayout.FloatField(ContentProperties.SpeedBlendTime, instance.GetSpeedBlendTime()));
            EndGroup();

            BeginGroup(ContentProperties.SwayProperties);
            IncreaseIndentLevel();
            BeginGroupLevel2(ref swaySystemFoldout, ContentProperties.SwaySystem);
            if (swaySystemFoldout)
            {
                WeaponSwaySystem weaponSwaySystem = instance.GetWeaponSwaySystem();
                weaponSwaySystem.SetAmount(AEditorGUILayout.FixedFloatField("Amount", weaponSwaySystem.GetAmount(), 0));
                weaponSwaySystem.SetMaxAmount(AEditorGUILayout.FixedFloatField("Max Amount", weaponSwaySystem.GetMaxAmount(), 0));
                weaponSwaySystem.SetSpeed(AEditorGUILayout.FixedFloatField("Speed", weaponSwaySystem.GetSpeed(), 0));
                instance.SetWeaponSwaySystem(weaponSwaySystem);
                instance.UseWeaponSwaySystem(EditorGUILayout.Toggle(instance.UseWeaponSwaySystem() ? "Enabled" : "Disabled", instance.UseWeaponSwaySystem()));
            }
            EndGroupLevel();
            DecreaseIndentLevel();
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
                        case "TakeIn":
                            instance.SetTakeInTime(clip.length);
                            break;
                        case "TakeOut":
                            instance.SetTakeOutTime(clip.length);
                            break;
                    }
                });
            }
            genericMenu.ShowAsContext();
        }

        /// <summary>
        /// Generate generic menu function and load state name.
        /// </summary>
        protected virtual void LoadStateName(object actionData)
        {
            string target = actionData.ToString();
            bool autoDisable = false;

            if (!instance.gameObject.activeSelf)
            {
                string message = string.Format("Unable to process an Animator component on a disabled object.\nEnable the \"{0}\" object and continue?", instance.name);
                switch (DisplayDialogs.MessageComplex(GetHeaderName(), message, "Yes", "No", "Yes, disable after done"))
                {
                    case 0:
                        instance.gameObject.SetActive(true);
                        break;
                    case 1:
                        return;
                    case 2:
                        instance.gameObject.SetActive(true);
                        autoDisable = true;
                        break;
                }

            }

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

            string[] parameterNames = EditorHelper.GetAnimatorParameterNames(animator);

            GenericMenu genericMenu = new GenericMenu();
            for (int i = 0, length = parameterNames.Length; i < length; i++)
            {
                string name = parameterNames[i];
                genericMenu.AddItem(new GUIContent(name), false, () =>
                {
                    AnimatorValue parameterValue = new AnimatorValue(name);
                    switch (target)
                    {
                        case "Speed":
                            instance.SetSpeedParameter(parameterValue);
                            break;
                        case "IsGrounded":
                            instance.SetIsGroundedParameter(parameterValue);
                            break;
                        case "IsCrouched":
                            instance.SetIsCrouchingParameter(parameterValue);
                            break;
                        case "TakeOut":
                            instance.SetTakeOutParameter(parameterValue);
                            break;
                    }
                    if (autoDisable)
                    {
                        instance.gameObject.SetActive(false);
                    }
                });
            }
            genericMenu.ShowAsContext();
        }
    }
}