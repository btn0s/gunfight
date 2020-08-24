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
    [CustomEditor(typeof(RecoilMapping))]
    public class RecoilMappingEditor : AuroraEditor<RecoilMapping>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent Patterns = new GUIContent("Patterns");
            public readonly static GUIContent ControllerState = new GUIContent("Controller State", "Controller state when this recoil pattern will be active.");
            public readonly static GUIContent VerticalSpread = new GUIContent("Vertical Spread", "Accuracy by Y axis.");
            public readonly static GUIContent HorizontalSpread = new GUIContent("Horizontal Spread", "Accuracy by X axis.");
            public readonly static GUIContent VerticalRecoil = new GUIContent("Vertical Impulse", "Vertical camera recoil.");
            public readonly static GUIContent HorizontalRecoil = new GUIContent("Horizontal Impulse", "Horizontal camera recoil.");
            public readonly static GUIContent RandomizeVerticalRecoil = new GUIContent("Randomize Vertical", "Clamp vertical camera recoil.");
            public readonly static GUIContent RandomizeHorizontalRecoil = new GUIContent("Randomize Horizontal", "Clamp horizontal camera recoil.");
            public readonly static GUIContent ClampVerticalRecoil = new GUIContent("Clamp Vertical", "Clamp vertical camera recoil.");
            public readonly static GUIContent ClampHorizontalRecoil = new GUIContent("Clamp Horizontal", "Clamp horizontal camera recoil.");
            public readonly static GUIContent ImpulseInputCompensate = new GUIContent("Input Compensate", "Persent of compensate input recoil impulse.");
            public readonly static GUIContent SpeedApplyImpulse = new GUIContent("Speed Apply Impulse", "Automatically return camera after recoil.");
            public readonly static GUIContent SpeedReturnImpulse = new GUIContent("Speed Return Impulse", "Automatically return camera after recoil.");
            public readonly static GUIContent SmoothCameraRecoilOutVertical = new GUIContent("Vertical Recoil Out Time", "Automatically return camera after recoil.");
            public readonly static GUIContent SmoothCameraRecoilOutHorizontal = new GUIContent("Horizontal Recoil Out Time", "Automatically return camera after recoil.");
            public readonly static GUIContent AutoReturn = new GUIContent("Auto Return", "Automatically return camera after recoil.");


            public readonly static GUIContent KickbackProperty = new GUIContent("Kickback Property");
            public readonly static GUIContent KickbackPosition = new GUIContent("Position", "Weapon kickback position.");
            public readonly static GUIContent PositionDampTime = new GUIContent("Speed Up", "Weapon kickback position speed up.");
            public readonly static GUIContent PositionSmooth = new GUIContent("Speed Out", "Weapon reset speed to default position from kickback position.");
            public readonly static GUIContent KickbackRotation = new GUIContent("Rotation", "Weapon kickback rotation");
            public readonly static GUIContent RotationDampTime = new GUIContent("Speed Up", "Weapon kickback rotation speed up.");
            public readonly static GUIContent RotationSmooth = new GUIContent("Speed Out", "Weapon reset speed to default rotation from kickback rotation.");

            public readonly static GUIContent ShakeTarget = new GUIContent("Target");
            public readonly static GUIContent PositionShake = new GUIContent("Position Shake");
            public readonly static GUIContent RotationShake = new GUIContent("Rotation Shake");
        }

        private SerializedProperty serializedMapping;
        private DictionaryEditor<ControllerState, RecoilPattern> serializedMappingDictionary;

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        public override void InitializeProperties()
        {
            serializedMapping = serializedObject.FindProperty("mapping");
            InitializeRecoilMappingDictionary();
        }

        private void InitializeRecoilMappingDictionary()
        {
            serializedMappingDictionary = new DictionaryEditor<ControllerState, RecoilPattern>(instance.GetMapping(), serializedMapping);

            serializedMappingDictionary.headerLabel = "Patterns";
            serializedMappingDictionary.addButtonLabel = "Add Pattern";
            serializedMappingDictionary.clearButtonLabel = "Clear Patterns";

            serializedMappingDictionary.onAddWindowGUICallback = (ref ControllerState state, ref RecoilPattern pattern) =>
            {
                state = (ControllerState)EditorGUILayout.EnumFlagsField(state);
            };

            serializedMappingDictionary.onElementGUICallback = (state, pattern, index) =>
            {
                SerializedProperty serializedVerticalBulletSpread = pattern.FindPropertyRelative("verticalBulletSpread");
                SerializedProperty serializedHorizontalBulletSpread = pattern.FindPropertyRelative("horizontalBulletSpread");

                bool verticalBulletAccuracyIsExpanded = serializedVerticalBulletSpread.isExpanded;
                BeginGroupLevel3(ref verticalBulletAccuracyIsExpanded, "Bullet Accuracy");
                if (verticalBulletAccuracyIsExpanded)
                {
                    EditorGUILayout.PropertyField(serializedVerticalBulletSpread, ContentProperties.VerticalSpread);
                    EditorGUILayout.PropertyField(serializedHorizontalBulletSpread, ContentProperties.HorizontalSpread);
                }
                EndGroupLevel();
                serializedVerticalBulletSpread.isExpanded = verticalBulletAccuracyIsExpanded;

                SerializedProperty serializedVerticalRecoilImpulse = pattern.FindPropertyRelative("verticalRecoilImpulse");
                SerializedProperty serializedHorizontalRecoilImpulse = pattern.FindPropertyRelative("horizontalRecoilImpulse");
                SerializedProperty serializedRandomVerticalCameraRecoil = pattern.FindPropertyRelative("randomVerticalCameraRecoil");
                SerializedProperty serializedRandomHorizontalCameraRecoil = pattern.FindPropertyRelative("randomHorizontalCameraRecoil");
                bool verticalCameraRecoilIsExpanded = serializedVerticalRecoilImpulse.isExpanded;
                BeginGroupLevel3(ref verticalCameraRecoilIsExpanded, "Camera Recoil");
                if (verticalCameraRecoilIsExpanded)
                {
                    if (serializedRandomVerticalCameraRecoil.boolValue)
                        EditorGUILayout.PropertyField(serializedVerticalRecoilImpulse, ContentProperties.VerticalRecoil);
                    else
                        serializedVerticalRecoilImpulse.FindPropertyRelative("max").floatValue = EditorGUILayout.FloatField(ContentProperties.VerticalRecoil, serializedVerticalRecoilImpulse.FindPropertyRelative("max").floatValue);
                    EditorGUILayout.PropertyField(serializedRandomVerticalCameraRecoil, ContentProperties.RandomizeVerticalRecoil);
                    if (serializedRandomHorizontalCameraRecoil.boolValue)
                        EditorGUILayout.PropertyField(serializedHorizontalRecoilImpulse, ContentProperties.HorizontalRecoil);
                    else
                        serializedHorizontalRecoilImpulse.FindPropertyRelative("max").floatValue = EditorGUILayout.FloatField(ContentProperties.HorizontalRecoil, serializedHorizontalRecoilImpulse.FindPropertyRelative("max").floatValue);
                    EditorGUILayout.PropertyField(serializedRandomHorizontalCameraRecoil, ContentProperties.RandomizeHorizontalRecoil);

                    SerializedProperty serializedClampVerticalRecoil = pattern.FindPropertyRelative("clampVerticalRecoil");
                    SerializedProperty serializedClampHorizontalRecoil = pattern.FindPropertyRelative("clampHorizontalRecoil");
                    EditorGUILayout.PropertyField(serializedClampVerticalRecoil, ContentProperties.ClampVerticalRecoil);
                    EditorGUILayout.PropertyField(serializedClampHorizontalRecoil, ContentProperties.ClampHorizontalRecoil);

                    SerializedProperty serializedImpulseInputCompensate = pattern.FindPropertyRelative("impulseInputCompensate");
                    SerializedProperty serializedSpeedApplyImpulse = pattern.FindPropertyRelative("speedApplyImpulse");
                    SerializedProperty serializedspeedReturnImpulse = pattern.FindPropertyRelative("speedReturnImpulse");
                    SerializedProperty serializedAutoReturn = pattern.FindPropertyRelative("autoReturn");
                    EditorGUILayout.PropertyField(serializedImpulseInputCompensate, ContentProperties.ImpulseInputCompensate);
                    EditorGUILayout.PropertyField(serializedSpeedApplyImpulse, ContentProperties.SpeedApplyImpulse);
                    EditorGUILayout.PropertyField(serializedspeedReturnImpulse, ContentProperties.SpeedReturnImpulse);
                    EditorGUILayout.PropertyField(serializedAutoReturn, ContentProperties.AutoReturn);
                }
                EndGroupLevel();
                serializedVerticalRecoilImpulse.isExpanded = verticalCameraRecoilIsExpanded;

                SerializedProperty serializedWeaponKickbackProperty = pattern.FindPropertyRelative("weaponKickbackProperty");
                bool weaponKickbackPropertyIsExpanded = serializedWeaponKickbackProperty.isExpanded;
                BeginGroupLevel3(ref weaponKickbackPropertyIsExpanded, "Weapon Kickback");
                if (weaponKickbackPropertyIsExpanded)
                {
                    serializedWeaponKickbackProperty.FindPropertyRelative("kickbackPosition").vector3Value = EditorGUILayout.Vector3Field(ContentProperties.KickbackPosition, serializedWeaponKickbackProperty.FindPropertyRelative("kickbackPosition").vector3Value);
                    serializedWeaponKickbackProperty.FindPropertyRelative("positionSpeedUp").floatValue = EditorGUILayout.FloatField(ContentProperties.PositionDampTime, serializedWeaponKickbackProperty.FindPropertyRelative("positionSpeedUp").floatValue);
                    serializedWeaponKickbackProperty.FindPropertyRelative("positionSpeedOut").floatValue = EditorGUILayout.FloatField(ContentProperties.PositionSmooth, serializedWeaponKickbackProperty.FindPropertyRelative("positionSpeedOut").floatValue);
                    serializedWeaponKickbackProperty.FindPropertyRelative("randomizePosition").boolValue = EditorGUILayout.Toggle("Randomize", serializedWeaponKickbackProperty.FindPropertyRelative("randomizePosition").boolValue);
                    GUILayout.Space(3);
                    serializedWeaponKickbackProperty.FindPropertyRelative("kickbackRotation").vector3Value = EditorGUILayout.Vector3Field(ContentProperties.KickbackRotation, serializedWeaponKickbackProperty.FindPropertyRelative("kickbackRotation").vector3Value);
                    serializedWeaponKickbackProperty.FindPropertyRelative("rotationSpeedUp").floatValue = EditorGUILayout.FloatField(ContentProperties.RotationDampTime, serializedWeaponKickbackProperty.FindPropertyRelative("rotationSpeedUp").floatValue);
                    serializedWeaponKickbackProperty.FindPropertyRelative("rotationSpeedOut").floatValue = EditorGUILayout.FloatField(ContentProperties.RotationSmooth, serializedWeaponKickbackProperty.FindPropertyRelative("rotationSpeedOut").floatValue);
                    serializedWeaponKickbackProperty.FindPropertyRelative("randomizeRotation").boolValue = EditorGUILayout.Toggle("Randomize", serializedWeaponKickbackProperty.FindPropertyRelative("randomizeRotation").boolValue);

                }
                EndGroupLevel();
                serializedWeaponKickbackProperty.isExpanded = weaponKickbackPropertyIsExpanded;


                SerializedProperty serializedCameraShakeProperties = pattern.FindPropertyRelative("shake");
                bool cameraShakePropertiesIsExpanded = serializedCameraShakeProperties.isExpanded;
                BeginGroupLevel3(ref cameraShakePropertiesIsExpanded, "Camera Shake");
                if (cameraShakePropertiesIsExpanded)
                {
                    EditorGUILayout.PropertyField(serializedCameraShakeProperties.FindPropertyRelative("target"), ContentProperties.ShakeTarget);
                    EditorGUILayout.PropertyField(serializedCameraShakeProperties.FindPropertyRelative("positionSettings"), ContentProperties.PositionShake);
                    EditorGUILayout.PropertyField(serializedCameraShakeProperties.FindPropertyRelative("rotationSettings"), ContentProperties.RotationShake);
                }
                EndGroupLevel();
                serializedCameraShakeProperties.isExpanded = cameraShakePropertiesIsExpanded;
            };

            serializedMappingDictionary.propertyLabelCallback = (state, pattern, index) =>
            {
                return new GUIContent(string.Format("Pattern: [{0}]", state.ToString()));
            };
        }

        /// <summary>
        /// Base serializedObject properties.
        /// 
        /// Implement this method to draw a custom properties.
        /// </summary>
        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.Patterns);
            serializedMappingDictionary.DrawLayoutDictionary();
            EndGroup();
        }
    }
}