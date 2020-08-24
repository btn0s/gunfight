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
using UnityEditor;
using UnityEngine;

namespace AuroraFPSEditor
{
    [CustomEditor(typeof(LootObjectSystem))]
    public class LootObjectSystemEditor : AuroraEditor<LootObjectSystem>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent BaseProperties = new GUIContent("Base Properties");
            public readonly static GUIContent SearchPoint = new GUIContent("Search Point", "Point from casting ray to search loot object.");
            public readonly static GUIContent SearchDistance = new GUIContent("Search Distance", "Casting max distance to search loot object.");
            public readonly static GUIContent HUDManager = new GUIContent("HUDManager", "Player HUD manager.");
            public readonly static GUIContent CullingLayer = new GUIContent("Culling Layer", "Casting loot objects culling layer.");
            public readonly static GUIContent AdvancedSettings = new GUIContent("Advanced Settings");
            public readonly static GUIContent SearchRate = new GUIContent("Search Rate", "How many times per frame to search for an object.\nThe higher the value, the less often the check.");
        }

        private bool advancedSettingsFoldout;

        public override void InitializeProperties()
        {
            if(instance.GetHUDManager() == null)
            {
                HUDManager hudManager = instance.GetComponentInChildren<HUDManager>();
                if(hudManager != null)
                {
                    instance.SetHUDManager(hudManager);
                }
            }
        }

        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.BaseProperties);
            instance.SetSearchPoint(AEditorGUILayout.RequiredObjectField(ContentProperties.SearchPoint, instance.GetSearchPoint(), true));
            instance.SetSearchDistance(AEditorGUILayout.FixedFloatField(ContentProperties.SearchDistance, instance.GetSearchDistance(), 0.01f));
            instance.SetHUDManager(AEditorGUILayout.RequiredObjectField(ContentProperties.HUDManager, instance.GetHUDManager(), true));
            instance.SetCullingLayer(AEditorGUILayout.LayerMaskField(ContentProperties.CullingLayer, instance.GetCullingLayer()));

            IncreaseIndentLevel();
            BeginGroupLevel2(ref advancedSettingsFoldout, ContentProperties.AdvancedSettings);
            if(advancedSettingsFoldout)
            {
                instance.SetSearchRate(AEditorGUILayout.FixedFloatField(ContentProperties.SearchRate, instance.GetSearchRate(), 0.01f));
                if (instance.GetSearchRate() < 0.1f)
                {
                    HelpBoxMessages.Message("Searching too often can slow performance on weak devices.", MessageType.Warning, true);
                }
            }
            EndGroupLevel();
            DecreaseIndentLevel();
            OnEventsGUI();
            EndGroup();
        }
    }
}