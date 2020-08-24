/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using UnityEngine;
using UnityEditor;
using AuroraFPSRuntime;
using UnityEditorInternal;
using System.Linq;
using System.Collections.Generic;

namespace AuroraFPSEditor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(CharacterHealth), true)]
    public class CharacterHealthEditor : ObjectHealthEditor
    {
        protected const float FieldLineHeight = 20;

        internal new static class ContentProperties
        {
            public readonly static GUIContent HealthSounds = new GUIContent("Sounds Effects");
            public readonly static GUIContent TakeDamageSound = new GUIContent("Take Damage", "Sound will be player when player take daamge.");
            public readonly static GUIContent VelocityDamageSound = new GUIContent("Velocity Damage", "Sound will be played when player take damage from velocity speed.");
            public readonly static GUIContent DeathSound = new GUIContent("Death", "Death sound will player when player die.");
            public readonly static GUIContent Heartbeat = new GUIContent("Heartbeat");
            public readonly static GUIContent HeartbeatStartFrom = new GUIContent("Start", "Start play heartbeat sound if player health <= this value.");
            public readonly static GUIContent HeartbeatRate = new GUIContent("Rate", "Heartbeat sound play rate.");
            public readonly static GUIContent HeartbeatSound = new GUIContent("Sound", "Heartbeat sound will player every rate, if condition [Heartbeat Start From] is met.");

            public readonly static GUIContent VelocityDamageProperties = new GUIContent("Velocity Damage Properties");

            public readonly static GUIContent RegenerationProperties = new GUIContent("Regeneration Properties");
            public readonly static GUIContent Rate = new GUIContent("Rate", "Rate (in seconds) of adding health points.\n(V/R - Value per rate).");
            public readonly static GUIContent Value = new GUIContent("Value", "Health point value.");
            public readonly static GUIContent Delay = new GUIContent("Delay", "Delay before start adding health.");

            public readonly static GUIContent HitAreas = new GUIContent("Hit Areas");
        }

        public struct HitAreaEditor
        {
            public HealthHitArea healthHit;
            public Editor editor;
            public bool foldout;
            public Collider targetCollider;
        }

        protected CharacterHealth characterHealthInstance;
        private ReorderableList velocityDamageList;
        private ReorderableList hitAreaList;
        private List<HitAreaEditor> hitAreaEditors;
        private Collider[] childColliders;
        private int activeElement;

        private bool regenerationFoldout;
        private bool velocityDamagePropertiesFoldout;
        private bool healthSoundFoldout;
        private bool healthSoundHeartbeatFoldout;
        private bool hitAreasFoldout;
        private float maxVelocity = 20.0f;
        private bool editMaxVelocity = false;

        public override void InitializeProperties()
        {
            base.InitializeProperties();
            characterHealthInstance = instance as CharacterHealth;
            InitializeVelocityDamageList();
            InitializeHitAreaList();
        }

        protected virtual void InitializeVelocityDamageList()
        {
            SerializedProperty fallDamageProperties = serializedObject.FindProperty("velocityDamageProperties");
            maxVelocity = FindMaxVelocity();
            velocityDamageList = new ReorderableList(serializedObject, fallDamageProperties, true, true, true, true)
            {
                drawHeaderCallback = (rect) =>
                {
                    EditorGUI.LabelField(new Rect(rect.x + 15, rect.y, 65, EditorGUIUtility.singleLineHeight), "Damage");
                    EditorGUI.LabelField(new Rect(rect.x + 77.5f, rect.y, 77, EditorGUIUtility.singleLineHeight), "Min Velocity");
                    float x = editMaxVelocity ? 90 : 35;
                    if (GUI.Button(new Rect(rect.width - x, rect.y, 75, EditorGUIUtility.singleLineHeight), "Max Velocity", EditorStyles.label))
                        editMaxVelocity = !editMaxVelocity;
                    if (editMaxVelocity)
                        maxVelocity = EditorGUI.FloatField(new Rect(rect.width - 9, rect.y, 35, EditorGUIUtility.singleLineHeight), GUIContent.none, maxVelocity);
                },

                drawElementCallback = (rect, index, isActive, isFocused) =>
                {
                    SerializedProperty property = fallDamageProperties.GetArrayElementAtIndex(index);

                    EditorGUI.PropertyField(new Rect(rect.x + 7.5f, rect.y + 1.5f, 35, EditorGUIUtility.singleLineHeight), property.FindPropertyRelative("damage"), GUIContent.none);
                    EditorGUI.LabelField(new Rect(rect.x + 55, rect.y + 1, 50, EditorGUIUtility.singleLineHeight), "->");
                    EditorGUI.PropertyField(new Rect(rect.x + 77.5f, rect.y + 1.5f, 35, EditorGUIUtility.singleLineHeight), property.FindPropertyRelative("minVelocity"), GUIContent.none);
                    float min = property.FindPropertyRelative("minVelocity").floatValue;
                    float max = property.FindPropertyRelative("maxVelocity").floatValue;
                    EditorGUI.MinMaxSlider(new Rect(rect.x + 120, rect.y + 1.5f, rect.width - 175, EditorGUIUtility.singleLineHeight), ref min, ref max, 0, maxVelocity);
                    property.FindPropertyRelative("minVelocity").floatValue = AMath.AllocatePart(min);
                    property.FindPropertyRelative("maxVelocity").floatValue = AMath.AllocatePart(max);
                    EditorGUI.PropertyField(new Rect(rect.width + 5, rect.y + 1.5f, 35, EditorGUIUtility.singleLineHeight), property.FindPropertyRelative("maxVelocity"), GUIContent.none);
                }
            };
        }

        protected virtual void InitializeHitAreaList()
        {
            childColliders = instance.GetComponentsInChildren<Collider>();
            CopyHitAreaEditors(childColliders, ref hitAreaEditors);
            hitAreaList = new ReorderableList(hitAreaEditors, typeof(HitAreaEditor), true, false, true, true);

            hitAreaList.headerHeight = 2;

            hitAreaList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                if (isActive)
                {
                    activeElement = index;
                }

                HitAreaEditor hitAreaEditor = hitAreaEditors[index];

                Rect fieldPosition = new Rect(rect.x - 2.5f, rect.y + 1.5f, rect.width, EditorGUIUtility.singleLineHeight);
                hitAreaEditor.foldout = EditorGUI.Foldout(fieldPosition, hitAreaEditor.foldout, new GUIContent(hitAreaEditor.healthHit.name), false);
                if (hitAreaEditor.foldout)
                {
                    fieldPosition.y += FieldLineHeight;
                    foreach (SerializedProperty property in EditorHelper.GetChildren(hitAreaEditor.editor.serializedObject))
                    {
                        if (property.name == "m_Script")
                        {
                            EditorGUI.BeginDisabledGroup(true);
                            AEditorGUI.ObjectField(fieldPosition, "Target", hitAreaEditor.targetCollider, true);
                            EditorGUI.EndDisabledGroup();
                        }
                        else
                        {
                            EditorGUI.PropertyField(fieldPosition, property);
                        }
                        fieldPosition.y += FieldLineHeight;
                    }
                }
                hitAreaEditors[index] = hitAreaEditor;
            };

            hitAreaList.onAddDropdownCallback = (rect, list) =>
            {
                GenericMenu hitAreasMenu = GetHitAreaMenu();
                hitAreasMenu.ShowAsContext();
            };

            hitAreaList.elementHeightCallback = (index) =>
            {
                HitAreaEditor hitAreaEditor = hitAreaEditors[index];
                float height = FieldLineHeight;
                if (hitAreaEditor.foldout)
                {
                    foreach (SerializedProperty property in EditorHelper.GetChildren(hitAreaEditor.editor.serializedObject))
                    {
                        height += FieldLineHeight;
                    }
                }
                return height + 3;
            };

            hitAreaList.onRemoveCallback = (list) =>
            {
                DestroyImmediate(hitAreaEditors[activeElement].healthHit);
                hitAreaEditors.RemoveAt(activeElement);
            };
        }

        public GenericMenu GetHitAreaMenu()
        {
            GenericMenu genericMenu = new GenericMenu();
            for (int i = 0; i < childColliders.Length; i++)
            {
                Collider collider = childColliders[i];
                bool contains = false;
                for (int j = 0; j < hitAreaEditors.Count; j++)
                {
                    HitAreaEditor hitAreaEditor = hitAreaEditors[j];
                    if(hitAreaEditor.targetCollider == collider)
                    {
                        genericMenu.AddDisabledItem(new GUIContent(collider.name));
                        contains = true;
                    }
                }
                if (!contains)
                {
                    genericMenu.AddItem(new GUIContent(collider.name), false, OnHitAreaMenuCallback, collider);
                }
            }
            return genericMenu;
        }

        private void OnHitAreaMenuCallback(object data)
        {
            Collider collider = data as Collider;
            HealthHitArea healthHitArea = collider.gameObject.AddComponent<HealthHitArea>();
            hitAreaEditors.Add(new HitAreaEditor() { healthHit = healthHitArea, editor = CreateEditor(healthHitArea), targetCollider = collider });
        }

        protected override void AddExcludingProperties(ref List<string> excludingProperties)
        {
            base.AddExcludingProperties(ref excludingProperties);
            excludingProperties.Add("velocityDamageProperties");
            excludingProperties.Add("healthSoundEffects");
            excludingProperties.Add("regenerationSettings");
            excludingProperties.Add("useRegeneration");
        }

        public override void OnBasePropertiesGUI()
        {
            base.OnBasePropertiesGUI();
            IncreaseIndentLevel();

            OnHitAreaHelperGUI();

            BeginGroupLevel2(ref regenerationFoldout, ContentProperties.RegenerationProperties);
            if (regenerationFoldout)
            {
                RegenerationProperties regenerationSystem = characterHealthInstance.GetRegenerationProperties();
                regenerationSystem.SetRate(AEditorGUILayout.FixedFloatField(ContentProperties.Rate, regenerationSystem.GetRate(), 0));
                regenerationSystem.SetValue(AEditorGUILayout.FixedIntField(ContentProperties.Value, regenerationSystem.GetValue(), 0));
                regenerationSystem.SetDelay(AEditorGUILayout.FixedFloatField(ContentProperties.Delay, regenerationSystem.GetDelay(), 0));
                characterHealthInstance.SetRegenerationProperties(regenerationSystem);
            }
            EditorGUI.EndDisabledGroup();
            if (regenerationFoldout)
            {
                string rpToggleName = characterHealthInstance.UseRegeneration() ? "Regeneration Enabled" : "Regeneration Disabled";
                characterHealthInstance.UseRegeneration(EditorGUILayout.Toggle(new GUIContent(rpToggleName), characterHealthInstance.UseRegeneration()));
            }
            EndGroupLevel();

            BeginGroupLevel2(ref healthSoundFoldout, ContentProperties.HealthSounds);
            if (healthSoundFoldout)
            {
                HealthSoundEffects healthSounds = characterHealthInstance.GetHealthSoundEffects();
                healthSounds.SetTakeDamageSound(AEditorGUILayout.ObjectField<AudioClip>(ContentProperties.TakeDamageSound, healthSounds.GetTakeDamageSound(), true));
                healthSounds.SetVelocityDamageSound(AEditorGUILayout.ObjectField<AudioClip>(ContentProperties.VelocityDamageSound, healthSounds.GetVelocityDamageSound(), true));
                healthSounds.SetDeathSound(AEditorGUILayout.ObjectField<AudioClip>(ContentProperties.DeathSound, healthSounds.GetDeathSound(), true));
                BeginGroupLevel3(ref healthSoundHeartbeatFoldout, ContentProperties.Heartbeat);
                if (healthSoundHeartbeatFoldout)
                {
                    healthSounds.SetHeartbeatStartFrom(EditorGUILayout.IntSlider(ContentProperties.HeartbeatStartFrom, healthSounds.GetHeartbeatStartFrom(), 0, characterHealthInstance.GetMaxHealth()));
                    healthSounds.SetHeartbeatRate(EditorGUILayout.Slider(ContentProperties.HeartbeatRate, healthSounds.GetHeartbeatRate(), 0.0f, 10.0f));
                    healthSounds.SetHeartbeatSound(AEditorGUILayout.ObjectField<AudioClip>(ContentProperties.HeartbeatSound, healthSounds.GetHeartbeatSound(), true));
                }
                EndGroupLevel();
                characterHealthInstance.SetHealthSoundEffects(healthSounds);
            }
            EndGroupLevel();

            BeginGroupLevel2(ref velocityDamagePropertiesFoldout, ContentProperties.VelocityDamageProperties);
            if (velocityDamagePropertiesFoldout)
            {
                DecreaseIndentLevel();
                velocityDamageList.DoLayoutList();
                IncreaseIndentLevel();
            }
            EndGroupLevel();
            DecreaseIndentLevel();
        }

        public virtual void OnHitAreaHelperGUI()
        {
            BeginGroupLevel2(ref hitAreasFoldout, ContentProperties.HitAreas);
            if (hitAreasFoldout)
            {
                hitAreaList.DoLayoutList();
            }
            EndGroupLevel();
        }

        protected void CopyHitAreaEditors(Collider[] colliders, ref List<HitAreaEditor> hitAreaEditors)
        {
            hitAreaEditors = new List<HitAreaEditor>();
            for (int i = 0; i < colliders.Length; i++)
            {
                Collider collider = colliders[i];
                HealthHitArea healthHitArea = collider.GetComponent<HealthHitArea>();
                if(healthHitArea != null)
                {
                    hitAreaEditors.Add(new HitAreaEditor() { healthHit = healthHitArea, editor = CreateEditor(healthHitArea), targetCollider = collider });
                }
            }
        }

        private float FindMaxVelocity()
        {
            if (characterHealthInstance.GetVelocityDamageProperties() == null || characterHealthInstance.GetVelocityDamageProperties().Length == 0)
                return 20;
            float height = characterHealthInstance.GetVelocityDamageProperties().Max(h => h.GetMaxVelocity());
            return Mathf.Ceil(height);
        }
    }
}