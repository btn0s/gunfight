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
    [CustomEditor(typeof(GrabJoint), true)]
    public class GrabJointEditor : AuroraEditor<GrabJoint>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent BaseProperties = new GUIContent("Base Properties");
            public readonly static GUIContent Anchor = new GUIContent("Anchor", "The position of the axis around which the body swings. The position is defined in local space.");
            public readonly static GUIContent Smooth = new GUIContent("Smooth", "Object transform update smooth.");
            public readonly static GUIContent BreakVelocity = new GUIContent("Break Velocity", "The velocity at which an object must touch another object to break the joint.");
            public readonly static GUIContent BreakDistance = new GUIContent("Break Distance");
            public readonly static GUIContent FreezeRotation = new GUIContent("Freeze Rotation", "Controls whether physics will change the rotation of the object.");
            public readonly static GUIContent CustomRotation = new GUIContent("Custom Rotation");

        }

        /// <summary>
        /// Enables the Editor to handle an event in the Scene view.
        /// 
        /// Use this event to implement custom handles and user interface.
        /// </summary>
        /// <param name="sceneView">Scene view settings.</param>
        public override void DuringSceneGUI(SceneView sceneView)
        {
            if (!Application.isPlaying)
            {
                Transform transform = instance.transform;

                Vector3 forward = transform.position + Vector3.forward * instance.GetAnchor().z;
                Vector3 anchor = transform.position + instance.GetAnchor();

                UnityEditor.Handles.color = Color.blue;
                UnityEditor.Handles.DrawAAPolyLine(5, transform.position, forward);
                UnityEditor.Handles.Label((transform.position + forward) / 2, new GUIContent("Forward"));

                UnityEditor.Handles.color = new Color32(255, 100, 0, 255);
                UnityEditor.Handles.DrawAAPolyLine(5, transform.position, anchor);
                UnityEditor.Handles.Label((transform.position + anchor) / 2, new GUIContent("Anchor"));

                UnityEditor.Handles.color = Color.red;
                UnityEditor.Handles.DrawAAPolyLine(5, forward, anchor);
                UnityEditor.Handles.Label((forward + anchor) / 2, new GUIContent("Difference"));
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
            instance.SetAnchor(EditorGUILayout.Vector3Field(ContentProperties.Anchor, instance.GetAnchor()));
            instance.SetSmooth(AEditorGUILayout.VisualClamp(ContentProperties.Smooth, instance.GetSmooth(), 20, 0.01f));
            instance.SetBreakVelocity(AEditorGUILayout.FixedFloatField(ContentProperties.BreakVelocity, instance.GetBreakVelocity(), 0));
            instance.SetBreakDistance(AEditorGUILayout.FixedFloatField(ContentProperties.BreakDistance, instance.GetBreakDistance(), 0.1f));

            Vector3 customRotation = instance.GetCustomRotation();
            bool freezeRotation = instance.FreezeRotaion();
            AEditorGUILayout.HiddenVector3Field(ContentProperties.CustomRotation, ContentProperties.FreezeRotation, ref customRotation, ref freezeRotation);
            instance.FreezeRotaion(freezeRotation);
            instance.SetCustomRotation(customRotation);
            EndGroup();
        }
    }
}
