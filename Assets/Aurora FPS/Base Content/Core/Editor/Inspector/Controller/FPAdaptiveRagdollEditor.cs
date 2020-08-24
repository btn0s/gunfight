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
using UnityEditorInternal;
using UnityEngine;

namespace AuroraFPSEditor
{
    [CustomEditor(typeof(FPAdaptiveRagdoll))]
    public class FPAdaptiveRagdollEditor : CharacterRagdollEditor
    {
        internal new static class ContentProperties
        {
            // Ragdoll properties contents
            public readonly static GUIContent RagdollProperties = new GUIContent("Ragdoll Properties");
            public readonly static GUIContent RelativeVelocity = new GUIContent("Relative Velocity Limit", "Limits of the relative linear velocity of the two colliding objects for the character to start ragdoll system.");
            public readonly static GUIContent StandDelay = new GUIContent("Stand Delay", "Delay before character stand. after ragdoll is played and wait in stable position.");

            //Player properties contents
            public readonly static GUIContent PlayerProperties = new GUIContent("Player Properties");
            public readonly static GUIContent FullBody = new GUIContent("Full Body");
            public readonly static GUIContent Meshes = new GUIContent("Meshes");
            public readonly static GUIContent FirstPersonMeshes = new GUIContent("First Person Meshes");
            public readonly static GUIContent FullBodyMeshes = new GUIContent("Full Body Meshes");

            public readonly static GUIContent FreeCamera = new GUIContent("Free Camera");
            public readonly static GUIContent CameraInstance = new GUIContent("Camera", "Camera instance.");
            public readonly static GUIContent Target = new GUIContent("Target", "Camera look target.");
            public readonly static GUIContent SmoothCameraRotation = new GUIContent("Rotation Smooth", "Camera rotation smooth value.");
            public readonly static GUIContent CullingLayer = new GUIContent("Culling Layer", "Camera culling layer for detect obstacle.");
            public readonly static GUIContent CullingSmooth = new GUIContent("Culling Smooth", "Culling handle smooth.");
            public readonly static GUIContent RightOffset = new GUIContent("Right Offset", "Camera right offset.");
            public readonly static GUIContent DefaultDistance = new GUIContent("Default Distance", "Default camera distance.");
            public readonly static GUIContent DistanceLimit = new GUIContent("Distance Limit", "Limits camera distance.");
            public readonly static GUIContent MinMaxDistance = new GUIContent("Distance");
            public readonly static GUIContent Height = new GUIContent("Height", "Camera height (Y axis).");
            public readonly static GUIContent FollowSpeed = new GUIContent("Follow Speed", "The speed of the camera following the controller.");
            public readonly static GUIContent SensitivityByX = new GUIContent("Sensitivity X", "Sensitivity by X axis.");
            public readonly static GUIContent SensitivityByY = new GUIContent("Sensitivity Y", "Sensitivity by Y axis.");
            public readonly static GUIContent LimitByY = new GUIContent("Limit Y", "Limits by Y axis.");
            public readonly static GUIContent ScreenFadeProperties = new GUIContent("Screen Fade Properties");
        }

        protected FPAdaptiveRagdoll fpAdaptiveRadgollInstance;

        private ReorderableList rl_FirstPersonMehesh;
        private ReorderableList rl_FullBodyMeshes;
        private bool meshesFoldout;
        private bool freeCameraFoldout;
        private bool screenFadeFoldout;

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        public override void InitializeProperties()
        {
            base.InitializeProperties();

            SetHeaderName("First Person Adaptive Ragdoll");

            fpAdaptiveRadgollInstance = instance as FPAdaptiveRagdoll;

            SerializedProperty sp_FirstPersonMeshes = serializedObject.FindProperty("firstPersonMeshes");
            rl_FirstPersonMehesh = new ReorderableList(serializedObject, sp_FirstPersonMeshes, true, true, true, true)
            {
                drawHeaderCallback = (rect) =>
                    {
                        EditorGUI.LabelField(rect, ContentProperties.FirstPersonMeshes);
                    },
                drawElementCallback = (rect, index, isFocused, isActive) =>
                {
                    SerializedProperty sp_Mesh = sp_FirstPersonMeshes.GetArrayElementAtIndex(index);
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y + 1.5f, rect.width, EditorGUIUtility.singleLineHeight), sp_Mesh, GUIContent.none);
                }
            };

            SerializedProperty sp_FullBodyMeshes = serializedObject.FindProperty("fullBodyMeshes");
            rl_FullBodyMeshes = new ReorderableList(serializedObject, sp_FullBodyMeshes, true, true, true, true)
            {
                drawHeaderCallback = (rect) =>
                    {
                        EditorGUI.LabelField(rect, ContentProperties.FullBodyMeshes);
                    },
                drawElementCallback = (rect, index, isFocused, isActive) =>
                {
                    SerializedProperty sp_Mesh = sp_FullBodyMeshes.GetArrayElementAtIndex(index);
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y + 1.5f, rect.width, EditorGUIUtility.singleLineHeight), sp_Mesh, GUIContent.none);
                }
            };
        }

        /// <summary>
        /// Base serializedObject properties.
        /// 
        /// Implement this method to draw a custom properties.
        /// </summary>
        public override void OnBaseGUI()
        {
            base.OnBaseGUI();
            BeginGroup(ContentProperties.PlayerProperties);
            fpAdaptiveRadgollInstance.SetFullBody(AEditorGUILayout.RequiredObjectField(ContentProperties.FullBody, fpAdaptiveRadgollInstance.GetFullBody(), true));
            IncreaseIndentLevel();

            BeginGroupLevel2(ref freeCameraFoldout, ContentProperties.FreeCamera);
            if (freeCameraFoldout)
            {
                FreeCamera freeCamera = fpAdaptiveRadgollInstance.GetFreeCamera();
                freeCamera.SetCameraTransform(AEditorGUILayout.ObjectField(ContentProperties.CameraInstance, freeCamera.GetCameraTransform(), true));
                freeCamera.SetTarget(AEditorGUILayout.ObjectField(ContentProperties.Target, freeCamera.GetTarget(), true));
                freeCamera.SetSmooth(EditorGUILayout.FloatField(ContentProperties.SmoothCameraRotation, freeCamera.GetSmooth()));
                LayerMask cullingLayer = EditorGUILayout.MaskField(ContentProperties.CullingLayer, InternalEditorUtility.LayerMaskToConcatenatedLayersMask(freeCamera.GetCullingLayer()), InternalEditorUtility.layers);
                freeCamera.SetCullingLayer(InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(cullingLayer));
                freeCamera.SetCullingSmooth(EditorGUILayout.FloatField(ContentProperties.CullingSmooth, freeCamera.GetCullingSmooth()));
                freeCamera.SetRightOffset(EditorGUILayout.FloatField(ContentProperties.RightOffset, freeCamera.GetRightOffset()));
                freeCamera.SetDefaultDistance(EditorGUILayout.FloatField(ContentProperties.DefaultDistance, freeCamera.GetDefaultDistance()));
                float minDistance = freeCamera.GetMinDistance();
                float maxDistance = freeCamera.GetMaxDistance();
                AEditorGUILayout.MinMaxSlider(ContentProperties.MinMaxDistance, ref minDistance, ref maxDistance, 1, 10);
                freeCamera.SetMinDistance(minDistance);
                freeCamera.SetMaxDistance(maxDistance);
                freeCamera.SetHeight(EditorGUILayout.FloatField(ContentProperties.Height, freeCamera.GetHeight()));
                freeCamera.SetFollowSpeed(EditorGUILayout.FloatField(ContentProperties.FollowSpeed, freeCamera.GetFollowSpeed()));
                freeCamera.SetXMouseSensitivity(EditorGUILayout.FloatField(ContentProperties.SensitivityByX, freeCamera.GetXMouseSensitivity()));
                freeCamera.SetYMouseSensitivity(EditorGUILayout.FloatField(ContentProperties.SensitivityByY, freeCamera.GetYMouseSensitivity()));
                float minLimit = freeCamera.GetYMinLimit();
                float maxLimit = freeCamera.GetYMaxLimit();
                AEditorGUILayout.MinMaxSlider(ContentProperties.LimitByY, ref minLimit, ref maxLimit, -360, 360);
                freeCamera.SetYMinLimit(minLimit);
                freeCamera.SetYMaxLimit(maxLimit);
                fpAdaptiveRadgollInstance.SetFreeCamera(freeCamera);
            }
            EndGroupLevel();

            BeginGroupLevel2(ref screenFadeFoldout, ContentProperties.ScreenFadeProperties);
            if (screenFadeFoldout)
            {
                fpAdaptiveRadgollInstance.SetScreenFadeProperties(AEditorGUILayout.CameraFadeSettingsField(fpAdaptiveRadgollInstance.GetScreenFadeProperties()));
            }
            EndGroupLevel();

            BeginGroupLevel2(ref meshesFoldout, ContentProperties.Meshes);
            if (meshesFoldout)
            {
                DecreaseIndentLevel();
                rl_FirstPersonMehesh.DoLayoutList();

                GUILayout.Space(5);

                rl_FullBodyMeshes.DoLayoutList();
                IncreaseIndentLevel();
            }
            EndGroupLevel();
            DecreaseIndentLevel();
            EndGroup();
        }

        public override void OnBasePropertiesGUI()
        {
            fpAdaptiveRadgollInstance.SetRelativeVelocityLimit(EditorGUILayout.FloatField(ContentProperties.RelativeVelocity, fpAdaptiveRadgollInstance.GetRelativeVelocityLimit()));
            fpAdaptiveRadgollInstance.SetStandDelay(EditorGUILayout.FloatField(ContentProperties.StandDelay, fpAdaptiveRadgollInstance.GetStandDelay()));
            base.OnBasePropertiesGUI();
        }
    }
}