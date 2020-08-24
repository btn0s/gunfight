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
    [CustomEditor(typeof(GrabbingSystem))]
    public class GrabbingSystemEditor : AuroraEditor<GrabbingSystem>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent BaseProperties = new GUIContent("Base Properties");
            public readonly static GUIContent PlayerCamera = new GUIContent("Camera", "First person camera transfrom.");
            public readonly static GUIContent AttachBody = new GUIContent("Attach Body", "Grab object attach body transform.");
            public readonly static GUIContent GrabRange = new GUIContent("Grab Range", "Max grab range distance.");
            public readonly static GUIContent ThrowForce = new GUIContent("Throw Force", "Grab object throw force.");
            public readonly static GUIContent ThrowSound = new GUIContent("Throw Sound", "Grab object throw sound clip.");
            public readonly static GUIContent GrabLayer = new GUIContent("Grab Layer", "Grab object layer.");
        }

        /// <summary>
        /// Base serializedObject properties.
        /// 
        /// Implement this method to draw a custom properties.
        /// </summary>
        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.BaseProperties);
            instance.SetPlayerCamera(AEditorGUILayout.RequiredObjectField(ContentProperties.PlayerCamera, instance.GetPlayerCamera(), true));
            instance.SetAttachBody(AEditorGUILayout.RequiredObjectField(ContentProperties.AttachBody, instance.GetAttachBody(), true));
            instance.SetGrabRange(EditorGUILayout.FloatField(ContentProperties.GrabRange, instance.GetGrabRange()));
            instance.SetThrowForce(EditorGUILayout.FloatField(ContentProperties.ThrowForce, instance.GetThrowForce()));
            instance.SetThrowSound(AEditorGUILayout.ObjectField(ContentProperties.ThrowSound, instance.GetThrowSound(), true));
            instance.SetGrabLayer(AEditorGUILayout.LayerMaskField(ContentProperties.GrabRange, instance.GetGrabLayer()));
            OnEventsGUI();
            EndGroup();
        }
    }
}