/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using AuroraFPSRuntime;

namespace AuroraFPSEditor
{
    [CustomEditor(typeof(WeaponShootingSystem), true)]
    public class WeaponShootingSystemEditor : AuroraEditor<WeaponShootingSystem>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent BaseProperties = new GUIContent("Base Properties");
            public readonly static GUIContent FireMode = new GUIContent("Fire Mode", "Weapon fire mode.");
            public readonly static GUIContent SingleFireRate = new GUIContent("Fire Rate", "Weapon single fire mode rate (sec).");
            public readonly static GUIContent FreeFireRate = new GUIContent("Fire Rate", "Weapon free fire mode rate (sec).");
            public readonly static GUIContent QueueFireRate = new GUIContent("Fire Rate", "Weapon queue fire mode rate (sec).");
            public readonly static GUIContent QueueCount = new GUIContent("Queue Count", "The number of bullets fired per shot.");

            public readonly static GUIContent FirePoint = new GUIContent("Fire Point");

            public readonly static GUIContent RecoilSystem = new GUIContent("Recoil System");
            public readonly static GUIContent RecoilMapping = new GUIContent("Recoil Mapping", "Recoil mapping asset.");

            public readonly static GUIContent AnimationStates = new GUIContent("Animation States");
            public readonly static GUIContent Normal = new GUIContent("Normal");
            public readonly static GUIContent FireState = new GUIContent("Fire", "Normal fire state name from animator controller.");
            public readonly static GUIContent DryFireState = new GUIContent("Dry Fire", "Normal dry fire state name from animator controller.");
            public readonly static GUIContent Zoom = new GUIContent("Zoom");
            public readonly static GUIContent ZoomFireState = new GUIContent("Zoom Fire", "Zoom fire state name from animator controller.");
            public readonly static GUIContent ZoomDryFireState = new GUIContent("Zoom Dry Fire", "Zoom dry fire state name from animator controller.");

            public readonly static GUIContent FireSounds = new GUIContent("Fire Sounds");
            public readonly static GUIContent FireEffects = new GUIContent("Fire Effects");
        }

        // Base weapon shooting system.
        private SerializedProperty serializedFireMode;
        private ReorderableList normalFireSoundsList;
        private ReorderableList dryFireSoundsList;
        private ReorderableList fireEffectList;

        // Stored foldout properties.
        private bool recoilFoldout;
        private bool animationStatesFoldout;
        private bool soundsFoldout;
        private bool effectsFoldout;


        public override void InitializeProperties()
        {
            serializedFireMode = serializedObject.FindProperty("fireMode");

            SerializedProperty normalSounds = serializedObject.FindProperty("fireSounds.normalFireSounds");
            normalFireSoundsList = CreateSoundsReorderableList(normalSounds, new GUIContent("Normal Sounds"));

            SerializedProperty drySounds = serializedObject.FindProperty("fireSounds.dryFireSounds");
            dryFireSoundsList = CreateSoundsReorderableList(drySounds, new GUIContent("Dry Sounds"));

            SerializedProperty fireEffects = serializedObject.FindProperty("fireEffects");
            fireEffectList = CreateEffectReorderableList(fireEffects, new GUIContent("Effects"));
        }

        public override void DuringSceneGUI(SceneView sceneView)
        {
            Transform firePoint = instance.GetFirePoint();
            if (firePoint != null)
            {
                UnityEditor.Handles.color = Color.red;
                firePoint.position = UnityEditor.Handles.PositionHandle(firePoint.position, firePoint.rotation);
                UnityEditor.Handles.SphereHandleCap(0, firePoint.position, firePoint.rotation, 0.025f, EventType.Repaint);
                UnityEditor.Handles.ArrowHandleCap(0, firePoint.position, firePoint.rotation, 0.5f, EventType.Repaint);
            }
        }

        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.BaseProperties);
            OnBasePropertiesGUI();
            OnRecoilSystemGUI();
            OnAnimationStatesGUI();
            OnFireSoundsGUI();
            OnFireEffects();
            OnEventsGUI();
            EndGroup();
        }

        protected override void AddExcludingProperties(ref List<string> excludingProperties)
        {
            base.AddExcludingProperties(ref excludingProperties);
            excludingProperties.Add("fireMode");
            excludingProperties.Add("singleFireRate");
            excludingProperties.Add("freeFireRate");
            excludingProperties.Add("queueFireRate");
            excludingProperties.Add("queueCount");
            excludingProperties.Add("recoilMapping");
            excludingProperties.Add("controller");
            excludingProperties.Add("fireSounds");
            excludingProperties.Add("fireEffects");
            excludingProperties.Add("fireState");
            excludingProperties.Add("dryFireState");
            excludingProperties.Add("zoomFireState");
            excludingProperties.Add("zoomDryFireState");
        }

        public virtual void OnBasePropertiesGUI()
        {
            OnFireModePropertiesGUI();
            DrawPropertiesExcluding(serializedObject, GetExcludingProperties());
        }

        public virtual void OnFireModePropertiesGUI()
        {
            EditorGUILayout.PropertyField(serializedFireMode, ContentProperties.FireMode);
            switch (instance.GetCurrentFireMode())
            {
                case WeaponShootingSystem.FireMode.Mute:
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.FloatField(new GUIContent("Fire Rate", "Choose some fire mode to edit fire rate value"), 0);
                    EditorGUI.EndDisabledGroup();
                    break;
                case WeaponShootingSystem.FireMode.Single:
                    instance.SetSingleFireRate(AEditorGUILayout.FixedFloatField(ContentProperties.SingleFireRate, instance.GetSingleFireRate(), 0));
                    break;
                case WeaponShootingSystem.FireMode.Queue:
                    instance.SetQueueFireRate(AEditorGUILayout.FixedFloatField(ContentProperties.QueueFireRate, instance.GetQueueFireRate(), 0));
                    instance.SetQueueCount(AEditorGUILayout.FixedIntField(ContentProperties.QueueCount, instance.GetQueueCount(), 2));
                    break;
                case WeaponShootingSystem.FireMode.Free:
                    instance.SetFreeFireRate(AEditorGUILayout.FixedFloatField(ContentProperties.FreeFireRate, instance.GetFreeFireRate(), 0));
                    break;
            }
        }

        public virtual void OnRecoilSystemGUI()
        {
            IncreaseIndentLevel();
            BeginGroupLevel2(ref recoilFoldout, ContentProperties.RecoilSystem);
            if (recoilFoldout)
            {
                instance.SetRecoilMapping(AEditorGUILayout.ObjectField(ContentProperties.RecoilMapping, instance.GetRecoilMapping(), true));
            }
            EndGroupLevel();
            DecreaseIndentLevel();
        }

        public virtual void OnAnimationStatesGUI()
        {
            IncreaseIndentLevel();
            BeginGroupLevel2(ref animationStatesFoldout, ContentProperties.AnimationStates);
            if (animationStatesFoldout)
            {
                instance.SetFireState(AEditorGUILayout.AnimatorStateField(ContentProperties.FireState, instance.GetFireState()));
                instance.SetDryFireState(AEditorGUILayout.AnimatorStateField(ContentProperties.DryFireState, instance.GetDryFireState()));
                instance.SetZoomFireState(AEditorGUILayout.AnimatorStateField(ContentProperties.ZoomFireState, instance.GetZoomFireState()));
                instance.SetZoomDryFireState(AEditorGUILayout.AnimatorStateField(ContentProperties.ZoomDryFireState, instance.GetZoomDryFireState()));
            }
            EndGroupLevel();
            DecreaseIndentLevel();
        }

        public virtual void OnFireSoundsGUI()
        {
            IncreaseIndentLevel();
            BeginGroupLevel2(ref soundsFoldout, ContentProperties.FireSounds);
            if (soundsFoldout)
            {
                normalFireSoundsList.DoLayoutList();
                dryFireSoundsList.DoLayoutList();
            }
            EndGroupLevel();
            DecreaseIndentLevel();
        }

        public virtual void OnFireEffects()
        {
            IncreaseIndentLevel();
            BeginGroupLevel2(ref effectsFoldout, ContentProperties.FireEffects);
            if (effectsFoldout)
            {
                fireEffectList.DoLayoutList();
            }
            EndGroupLevel();
            DecreaseIndentLevel();
        }

        protected virtual ReorderableList CreateSoundsReorderableList(SerializedProperty serializedSounds, GUIContent content)
        {
            ReorderableList soundsList = new ReorderableList(serializedObject, serializedSounds, true, true, true, true);
            soundsList.drawHeaderCallback = (rect) =>
            {
                EditorGUI.LabelField(rect, content);
            };

            soundsList.drawElementCallback = (rect, index, isActive, isFocus) =>
            {
                rect.y += 1.5f;
                Rect objectRect = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(objectRect, serializedSounds.GetArrayElementAtIndex(index), GUIContent.none);
            };
            return soundsList;
        }

        protected virtual ReorderableList CreateEffectReorderableList(SerializedProperty serializedFireEffects, GUIContent content)
        {
            ReorderableList fireEffectList = new ReorderableList(serializedObject, serializedFireEffects, true, true, true, true);
            fireEffectList.drawHeaderCallback = (rect) =>
            {
                EditorGUI.LabelField(rect, content);
            };

            fireEffectList.drawElementCallback = (rect, index, isActive, isFocus) =>
            {
                rect.y += 1.5f;
                Rect objectRect = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(objectRect, serializedFireEffects.GetArrayElementAtIndex(index), GUIContent.none);
            };
            return fireEffectList;
        }

        
    }
}
