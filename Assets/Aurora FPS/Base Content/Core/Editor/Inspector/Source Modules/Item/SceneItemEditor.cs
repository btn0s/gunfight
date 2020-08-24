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
    [CustomEditor(typeof(SceneItem), true)]
    public class SceneItemEditor : BaseItemEditor
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent BaseProperties = new GUIContent("Base Properties");
            public readonly static GUIContent DisplayName = new GUIContent("Name", "Weapon display name.");
            public readonly static GUIContent Description = new GUIContent("Description", "Weapon description (optional).");
            public readonly static GUIContent Image = new GUIContent("Image", "Weapon image in HUD.");
            public readonly static GUIContent DropProperties = new GUIContent("Drop Properties");
            public readonly static GUIContent DropObject = new GUIContent("Object", "Drop weapon object prefab.");
            public readonly static GUIContent Force = new GUIContent("Force", "Drop weapon force.");
            public readonly static GUIContent SoundEffect = new GUIContent("Sound Effect", "Drop weapon sound effect.");
            public readonly static GUIContent Distance = new GUIContent("Distance", "Drop weapon instantiate distance relative player.");
            public readonly static GUIContent Rotation = new GUIContent("Rotation", "Drop weapon instantiate rotation.");
        }

        protected SceneItem sceneItemInstance;
        private bool dropPropertiesFoldout;

        public override void InitializeProperties()
        {
            base.InitializeProperties();
            sceneItemInstance = instance as SceneItem;
        }

        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.BaseProperties);
            OnBasePropertiesGUI();
            DrawPropertiesExcluding(serializedObject, GetExcludingProperties());
            DrawDropObject();
            EndGroup();
        }

        public override void OnBasePropertiesGUI()
        {
            base.OnBasePropertiesGUI();
            sceneItemInstance.SetDisplayName(EditorGUILayout.TextField(ContentProperties.DisplayName, sceneItemInstance.GetDisplayName()));
            sceneItemInstance.SetDescription(AEditorGUILayout.TextArea(ContentProperties.Description, sceneItemInstance.GetDescription()));
            sceneItemInstance.SetSprite(AEditorGUILayout.ObjectField(ContentProperties.Image, sceneItemInstance.GetSprite(), false, GUILayout.Height(EditorGUIUtility.singleLineHeight)));
        }

        protected override void AddExcludingProperties(ref System.Collections.Generic.List<string> excludingProperties)
        {
            base.AddExcludingProperties(ref excludingProperties);
            excludingProperties.Add("displayName");
            excludingProperties.Add("description");
            excludingProperties.Add("sprite");
            excludingProperties.Add("dropObject");
        }

        public void DrawDropObject()
        {
            IncreaseIndentLevel();
            BeginGroupLevel2(ref dropPropertiesFoldout, ContentProperties.DropProperties);
            if (dropPropertiesFoldout)
            {
                DropObject properties = sceneItemInstance.GetDropObject();
                properties.SetDropObject(AEditorGUILayout.ObjectField(ContentProperties.DropObject, properties.GetDropObject(), false));
                properties.SetForce(AEditorGUILayout.FixedFloatField(ContentProperties.Force, properties.GetForce(), 0));
                properties.SetSoundEffect(AEditorGUILayout.ObjectField(ContentProperties.SoundEffect, properties.GetSoundEffect(), false));
                properties.SetDistance(AEditorGUILayout.FixedFloatField(ContentProperties.Distance, properties.GetDistance(), 0));
                properties.SetRotation(EditorGUILayout.Vector3Field(ContentProperties.Rotation, properties.GetRotation()));
                sceneItemInstance.SetDropObject(properties);
            }
            EndGroupLevel();
            DecreaseIndentLevel();
        }
    }
}