/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using AuroraFPSRuntime;
using AuroraFPSRuntime.UI;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace AuroraFPSEditor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Crosshair))]
    public class CrosshairEditor : AuroraEditor<Crosshair>
    {
        public const string EmptyPresetTypeName = "None";

        internal new static class ContentProperties
        {
            public readonly static GUIContent BaseProperties = new GUIContent("Base Properties");
            public readonly static GUIContent Preset = new GUIContent("Base Preset Settings");
            public readonly static GUIContent CrosshairPreset = new GUIContent("Type");
            public readonly static GUIContent States = new GUIContent("States");
            public readonly static GUIContent Spread = new GUIContent("Spread");
            public readonly static GUIContent HideValue = new GUIContent("Hide Value");



            public readonly static GUIContent Rotation = new GUIContent("Rotation");
            public readonly static GUIContent RotateSide = new GUIContent("Rotate Side");
            public readonly static GUIContent RotateAngle = new GUIContent("Angle");
            public readonly static GUIContent RotateSpeed = new GUIContent("Speed");

            public readonly static GUIContent FireSettings = new GUIContent("Fire Settings");
            public readonly static GUIContent FireSpread = new GUIContent("Value");
            public readonly static GUIContent FireSpeed = new GUIContent("Speed");

            public readonly static GUIContent HitSettings = new GUIContent("Hit Preset Settings");
            public readonly static GUIContent KillSettings = new GUIContent("Kill Preset Settings");



            public readonly static GUIContent OtherSettings = new GUIContent("Other");
            public readonly static GUIContent SpreadUpdateFunction = new GUIContent("Spread Update Function", "The method to use for updating spread value.");
            public readonly static GUIContent StaticSpread = new GUIContent("Spread", "Static spread value.");
            public readonly static GUIContent Visible = new GUIContent("Visible", "Crosshair visibility.");
        }

        private ReorderableList crosshairStateList;
        private bool presetsFoldout;
        private bool statesFoldout;
        private bool fireSettingsFoldout;
        private bool hitSettingFoldout;
        private bool hitSpreadFoldout;
        private bool killSettingFoldout;
        private bool killSpreadFoldout;
        private bool otherSettingsFoldout;


        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        public override void InitializeProperties()
        {
            crosshairStateList = CreateStateReorderableList();
        }

        /// <summary>
        /// Base serializedObject properties.
        /// 
        /// Implement this method to draw a custom properties.
        /// </summary>
        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.BaseProperties);
            PresetPropertiesGUILayout();
            StatesPropertiesGUILayout();
            FireSettingsGUILayout();
            HitSettingsGUILayout();
            KillSettingsGUILayout();
            OtherPropertiesGUILayout();
            EndGroup();
        }

        /// <summary>
        /// Draw crosshair preset GUI layout editor.
        /// </summary>
        public virtual void PresetPropertiesGUILayout()
        {
            IncreaseIndentLevel();
            BeginGroupLevel2(ref presetsFoldout, ContentProperties.Preset);
            if (presetsFoldout)
            {
                PresetsPopupGUI(instance.GetCrosshairPreset(), preset => instance.SetCrosshairPreset(preset));
                GUILayout.Space(3);
                SerializedProperty serializedBasePreset = serializedObject.FindProperty("crosshairPreset");
                DrawCrosshairPresetEditor(serializedBasePreset);
            }
            EndGroupLevel();
            DecreaseIndentLevel();
        }

        /// <summary>
        /// Preset popup button showing all available crosshair presets.
        /// </summary>
        public virtual void PresetsPopupGUI(CrosshairPreset current, Action<CrosshairPreset> callback)
        {
            Rect presetMenuPosition = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight);

            Rect lablePosition = new Rect(presetMenuPosition.x, presetMenuPosition.y, 120, presetMenuPosition.height);
            EditorGUI.LabelField(lablePosition, ContentProperties.CrosshairPreset);

            Rect popupPosition = new Rect(presetMenuPosition.x + EditorGUIUtility.labelWidth + 2, presetMenuPosition.y, presetMenuPosition.width - EditorGUIUtility.labelWidth - 2, presetMenuPosition.height);
            if (GUI.Button(popupPosition, GetPresetName(instance.GetCrosshairPreset()), EditorStyles.popup))
            {
                GenericMenu presetMenu = CreatePresetMenu(current, callback);
                presetMenu.ShowAsContext();
            }
        }

        /// <summary>
        /// Draw crosshair states GUI layout editor.
        /// </summary>
        public virtual void StatesPropertiesGUILayout()
        {
            IncreaseIndentLevel();
            BeginGroupLevel2(ref statesFoldout, ContentProperties.States);
            if (statesFoldout)
            {
                crosshairStateList.DoLayoutList();
            }
            EndGroupLevel();
            DecreaseIndentLevel();
        }

        /// <summary>
        /// Draw fire settings properties GUI layout.
        /// </summary>
        public virtual void FireSettingsGUILayout()
        {
            IncreaseIndentLevel();
            BeginGroupLevel2(ref fireSettingsFoldout, ContentProperties.FireSettings);
            if (fireSettingsFoldout)
            {
                CrosshairSpread crosshairSpread = instance.GetFireSpread();
                crosshairSpread.SetValue(AEditorGUILayout.FixedFloatField(ContentProperties.FireSpread, crosshairSpread.GetValue(), 0));
                crosshairSpread.SetSpeed(AEditorGUILayout.FixedFloatField(ContentProperties.FireSpeed, crosshairSpread.GetSpeed(), 0));
                instance.SetFireSpread(crosshairSpread);
            }
            EndGroupLevel();
            DecreaseIndentLevel();
        }

        /// <summary>
        /// Draw hit settings properties GUI layout.
        /// </summary>
        public virtual void HitSettingsGUILayout()
        {
            IncreaseIndentLevel();
            BeginGroupLevel2(ref hitSettingFoldout, ContentProperties.HitSettings);
            if (hitSettingFoldout)
            {
                PresetsPopupGUI(instance.GetHitPreset(), preset => instance.SetHitPreset(preset));
                GUILayout.Space(3);
                SerializedProperty serializedHitPreset = serializedObject.FindProperty("hitPreset");
                DrawCrosshairPresetEditor(serializedHitPreset.Copy());

                BeginGroupLevel3(ref hitSpreadFoldout, ContentProperties.Spread);
                if (hitSpreadFoldout)
                {
                    CrosshairSpread crosshairSpread = instance.GetHitSpread();
                    crosshairSpread.SetValue(AEditorGUILayout.FixedFloatField(ContentProperties.FireSpread, crosshairSpread.GetValue(), 0));
                    crosshairSpread.SetSpeed(AEditorGUILayout.FixedFloatField(ContentProperties.FireSpeed, crosshairSpread.GetSpeed(), 0));
                    instance.SetHitSpread(crosshairSpread);
                    instance.SetHitHideValue(AEditorGUILayout.FixedFloatField(ContentProperties.HideValue, instance.GetHitHideValue(), 0));
                }
                EndGroupLevel();
            }
            EndGroupLevel();
            DecreaseIndentLevel();
        }

        public virtual void KillSettingsGUILayout()
        {
            IncreaseIndentLevel();
            BeginGroupLevel2(ref killSettingFoldout, ContentProperties.KillSettings);
            if (killSettingFoldout)
            {
                PresetsPopupGUI(instance.GetKillPreset(), preset => instance.SetKillPreset(preset));
                GUILayout.Space(3);
                SerializedProperty serializedKillPreset = serializedObject.FindProperty("killPreset");
                DrawCrosshairPresetEditor(serializedKillPreset.Copy());

                BeginGroupLevel3(ref killSpreadFoldout, ContentProperties.Spread);
                if (killSpreadFoldout)
                {
                    CrosshairSpread crosshairSpread = instance.GetKillSpread();
                    crosshairSpread.SetValue(AEditorGUILayout.FixedFloatField(ContentProperties.FireSpread, crosshairSpread.GetValue(), 0));
                    crosshairSpread.SetSpeed(AEditorGUILayout.FixedFloatField(ContentProperties.FireSpeed, crosshairSpread.GetSpeed(), 0));
                    instance.SetKillSpread(crosshairSpread);
                    instance.SetKillHideValue(AEditorGUILayout.FixedFloatField(ContentProperties.HideValue, instance.GetKillHideValue(), 0));
                }
                EndGroupLevel();
            }
            EndGroupLevel();
            DecreaseIndentLevel();
        }

        /// <summary>
        /// Draw other crosshair properties GUI layout.
        /// </summary>
        public virtual void OtherPropertiesGUILayout()
        {
            IncreaseIndentLevel();
            BeginGroupLevel2(ref otherSettingsFoldout, ContentProperties.OtherSettings);
            if (otherSettingsFoldout)
            {
                instance.SetSpreadUpdateFunction(AEditorGUILayout.EnumPopup(ContentProperties.SpreadUpdateFunction, instance.GetSpreadUpdateFunction()));
                if (instance.GetSpreadUpdateFunction() == Crosshair.SpreadUpdateFunction.Static)
                {
                    instance.SetSpread(AEditorGUILayout.FixedFloatField(ContentProperties.StaticSpread, instance.GetSpread(), 0));
                }
                instance.Visible(EditorGUILayout.Toggle(ContentProperties.Visible, instance.Visible()));
            }
            EndGroupLevel();
            DecreaseIndentLevel();
        }

        /// <summary>
        /// Draw editor to edit all crosshair preset properties.
        /// </summary>
        /// <param name="crosshairPreset"></param>
        public static void DrawCrosshairPresetEditor(SerializedProperty crosshairPreset)
        {
            if (crosshairPreset.hasVisibleChildren)
            {
                bool baseFoldout = crosshairPreset.isExpanded;
                BeginGroupLevel3(ref baseFoldout, "Element Settings");
                if (baseFoldout)
                {
                    AEditorGUILayout.DrawPropertyChilds(crosshairPreset, new string[4] { "rotationSide", "angle", "rotationSpeed", "hideState" });
                    EditorGUILayout.PropertyField(crosshairPreset.FindPropertyRelative("hideState"));
                }
                EndGroupLevel();
                crosshairPreset.isExpanded = baseFoldout;

                SerializedProperty rotationSide = crosshairPreset.FindPropertyRelative("rotationSide");
                bool rotationFoldout = rotationSide.isExpanded;
                BeginGroupLevel3(ref rotationFoldout, "Rotation Settings");
                if (rotationFoldout)
                {
                    EditorGUILayout.PropertyField(rotationSide);
                    EditorGUILayout.PropertyField(crosshairPreset.FindPropertyRelative("angle"));
                    EditorGUILayout.PropertyField(crosshairPreset.FindPropertyRelative("rotationSpeed"));
                }
                EndGroupLevel();
                rotationSide.isExpanded = rotationFoldout;
            }
            else
            {
                GUILayout.Label("CrosshairPreset is null...");
            }
        }

        /// <summary>
        /// Create reorderable list for crosshair states.
        /// </summary>
        public ReorderableList CreateStateReorderableList()
        {
            SerializedProperty crosshairStates = serializedObject.FindProperty("crosshairStates");
            ReorderableList reorderableList = new ReorderableList(serializedObject, crosshairStates, true, true, true, true);

            reorderableList.drawHeaderCallback = (rect) =>
            {
                rect.y += 1.5f;
                Rect stateRect = new Rect(rect.x + 14, rect.y, 75, EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(stateRect, "State");

                Rect spreadRect = new Rect(rect.x + 93, rect.y, 75, EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(spreadRect, "Spread");

                Rect speedRect = new Rect(rect.x + 173, rect.y, rect.width - 160, EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(speedRect, "Speed");
            };

            reorderableList.drawElementCallback = (rect, index, isActive, isFocus) =>
            {
                CrosshairState state = instance.GetCrosshairState(index);
                rect.y += 1.5f;
                Rect stateRect = new Rect(rect.x, rect.y, 75, EditorGUIUtility.singleLineHeight);
                state.SetState((ControllerState)EditorGUI.EnumFlagsField(stateRect, GUIContent.none, state.GetState()));

                CrosshairSpread crosshairSpread = state.GetCrosshairSpread();
                Rect spreadRect = new Rect(rect.x + 80, rect.y, 75, EditorGUIUtility.singleLineHeight);
                crosshairSpread.SetValue(EditorGUI.FloatField(spreadRect, GUIContent.none, crosshairSpread.GetValue()));
                Rect speedRect = new Rect(spreadRect.x + 80, rect.y, rect.width - 160, EditorGUIUtility.singleLineHeight);
                crosshairSpread.SetSpeed(EditorGUI.FloatField(speedRect, GUIContent.none, crosshairSpread.GetSpeed()));

                state.SetCrosshairSpread(crosshairSpread);
                instance.SetCrosshairState(index, state);
            };
            return reorderableList;
        }

        /// <summary>
        /// Create GenericMenu with all available crosshair presets.
        /// </summary>
        /// <returns>GenericMenu with crosshair presets</returns>
        public GenericMenu CreatePresetMenu(CrosshairPreset current, Action<CrosshairPreset> callback)
        {
            IEnumerable<Type> presetTypes = AuroraExtension.FindSubclassesOf<CrosshairPreset>();
            GenericMenu presetMenu = new GenericMenu();
            foreach (Type presetType in presetTypes)
            {
                CrosshairPresetDataAttribute presetData = AuroraExtension.GetAttribute<CrosshairPresetDataAttribute>(presetType);
                if (presetData != null && !string.IsNullOrEmpty(presetData.GetName()) && !presetData.Hidden())
                {
                    CrosshairPreset preset = Activator.CreateInstance(presetType) as CrosshairPreset;
                    if (current == null || (current != null && preset.GetType() != current.GetType()))
                        presetMenu.AddItem(new GUIContent(presetData.GetName()), false, () => callback?.Invoke(preset));
                    else
                        presetMenu.AddItem(new GUIContent(string.Format("{0} (Current)", presetData.GetName())), false, () => callback?.Invoke(preset));
                }
            }
            return presetMenu;
        }

        /// <summary>
        /// Get preset name by specific preset type.
        /// </summary>
        /// <param name="preset">Type of crosshair preset</param>
        /// <returns>Name of crosshair preset type.</returns>
        public string GetPresetName(CrosshairPreset preset)
        {
            if (preset != null)
            {
                CrosshairPresetDataAttribute presetData = AuroraExtension.GetAttribute<CrosshairPresetDataAttribute>(preset.GetType());
                return presetData?.GetName() ?? EmptyPresetTypeName;
            }
            return EmptyPresetTypeName;
        }
    }
}