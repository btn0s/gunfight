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
    [CustomEditor(typeof(RemoteBody))]
    public class RemoteBodyEditor : AuroraEditor<RemoteBody>
    {
        private const string ActionButtonTooltip = "Load parameter name from Animator component.";

        internal new static class ContentProperties
        {
            public readonly static GUIContent BodyProperties = new GUIContent("Body Properties");
            public readonly static GUIContent Controller = new GUIContent("Controller", "First person controller instance.");

            public readonly static GUIContent AnimationProperties = new GUIContent("Animation Properties");
            public readonly static GUIContent SpeedParameter = new GUIContent("Speed", "Speed parameter from animator controller");
            public readonly static GUIContent DirectionParameter = new GUIContent("Direction", "Direction parameter from animator controller.");
            public readonly static GUIContent RotateAxisParameter = new GUIContent("Rotate Axis", "Rotate axis parameter from animator controller.");
            public readonly static GUIContent IsGroundedParameter = new GUIContent("IsGrounded", "IsGrounded parameter from animator controller.");
            public readonly static GUIContent IsCrouchingParameter = new GUIContent("IsCrouching", "IsCrouching parameter from animator controller.");
            public readonly static GUIContent FireState = new GUIContent("Fire", "Fire state from animator controller.");
            public readonly static GUIContent ReloadState = new GUIContent("Reload", "Reload state from animator controller.");
            public readonly static GUIContent VelocitySmooth = new GUIContent("Velocity Smooth", "Smooth of blending velocity to speed and direction values.");
        }

        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.AnimationProperties);
            instance.SetSpeedParameter(AEditorGUILayout.ActionAnimatorValueField(ContentProperties.SpeedParameter, instance.GetSpeedParameter(), LoadParameterName, "Speed", ActionButtonTooltip));
            instance.SetDirectionParameter(AEditorGUILayout.ActionAnimatorValueField(ContentProperties.DirectionParameter, instance.GetDirectionParameter(), LoadParameterName, "Direction", ActionButtonTooltip));
            instance.SetIsGroundedParameter(AEditorGUILayout.ActionAnimatorValueField(ContentProperties.IsGroundedParameter, instance.GetIsGroundedParameter(), LoadParameterName, "IsGrounded", ActionButtonTooltip));
            instance.SetIsCrouchingParameter(AEditorGUILayout.ActionAnimatorValueField(ContentProperties.IsCrouchingParameter, instance.GetIsCrouchingParameter(), LoadParameterName, "IsCrouched", ActionButtonTooltip));
            instance.SetVelocitySmooth(AEditorGUILayout.FixedFloatField(ContentProperties.VelocitySmooth, instance.GetVelocitySmooth(), 0));
            EndGroup();
        }

        /// <summary>
        /// Generate generic menu function and load parameter name.
        /// </summary>
        protected virtual void LoadParameterName(object actionData)
        {
            string target = actionData.ToString();
            bool autoDisable = false;

            if (!instance.gameObject.activeSelf)
            {
                string message = string.Format("Unable to process an Animator component on a disabled object.\nEnable \"{0}\" object and continue?.", instance.name);
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
                        case "Direction":
                            instance.SetDirectionParameter(parameterValue);
                            break;
                        case "IsGrounded":
                            instance.SetIsGroundedParameter(parameterValue);
                            break;
                        case "IsCrouched":
                            instance.SetIsCrouchingParameter(parameterValue);
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