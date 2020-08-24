/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using AuroraFPSRuntime.UI;
using UnityEditor;
using UnityEngine;

namespace AuroraFPSEditor
{
    [CustomEditor(typeof(HUDManager))]
    public class AHUDManagerEditor : AuroraEditor<HUDManager>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent BaseProperties = new GUIContent("Base Properties");
            public readonly static GUIContent Player = new GUIContent("Player");
            public readonly static GUIContent HUDElements = new GUIContent("HUD Elements");
            public readonly static GUIContent HealthProperties = new GUIContent("Health Properties");
            public readonly static GUIContent DisplayMode = new GUIContent("Display Mode");
            public readonly static GUIContent GrenadeProperties = new GUIContent("Grenade Properties");
            public readonly static GUIContent GroupName = new GUIContent("Group Name");
        }

        private SerializedProperty hudElements;
        private bool hudElementsFoldout;
        private bool healthPropertiesFoldout;
        private bool grenadePropertiesFoldout;

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// 
        /// Implement this method to initialize properties.
        /// </summary>
        public override void InitializeProperties()
        {
            hudElements = serializedObject.FindProperty("hudElements");

            if (instance.GetPlayer() == null)
            {
                Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
                if (player != null)
                {
                    instance.SetPlayer(player);
                }
            }
        }

        /// <summary>
        /// Base serializedObject properties.
        /// 
        /// Implement this method to draw a custom properties.
        /// </summary>
        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.BaseProperties);
            instance.SetPlayer(AEditorGUILayout.RequiredObjectField(ContentProperties.Player, instance.GetPlayer(), true));

            IncreaseIndentLevel();
            BeginGroupLevel2(ref hudElementsFoldout, ContentProperties.HUDElements);
            if (hudElementsFoldout)
            {
                foreach (SerializedProperty element in EditorHelper.GetChildren(hudElements))
                {
                    EditorGUILayout.PropertyField(element);
                }
            }
            EndGroupLevel();

            BeginGroupLevel2(ref healthPropertiesFoldout, ContentProperties.HealthProperties);
            if (healthPropertiesFoldout)
            {
                instance.SetHealthValue(AEditorGUILayout.EnumPopup(ContentProperties.DisplayMode, instance.GetHealthValue()));
            }
            EndGroupLevel();

            BeginGroupLevel2(ref grenadePropertiesFoldout, ContentProperties.GrenadeProperties);
            if (grenadePropertiesFoldout)
            {
                instance.SetGrenadeGroupName(EditorGUILayout.TextField(ContentProperties.GroupName, instance.GetGrenadeGroupName()));
            }
            EndGroupLevel();
            DecreaseIndentLevel();
            EndGroup();
        }
    }
}