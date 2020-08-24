/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using AuroraFPSRuntime.AI;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace AuroraFPSEditor
{
    [CustomEditor(typeof(AIFieldOfView))]
    public class AIFieldOfViewEditor : AuroraEditor<AIFieldOfView>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent BaseProperties = new GUIContent("Base Properties");

            public readonly static GUIContent TargetMask = new GUIContent("Target Mask", "Object with this mask will handle like a main target objects.");
            public readonly static GUIContent ObstacleMask = new GUIContent("Obstacle Mask", "Objects with this masks will handle like obstacle through AI can't see.");
            public readonly static GUIContent ViewRadius = new GUIContent("View Radius", "AI field ov view radius.");
            public readonly static GUIContent ViewAngle = new GUIContent("View Angle", "AI field of view angle.");
            public readonly static GUIContent ViewOffset = new GUIContent("View Offset", "AI field of view height offset.");

            public readonly static GUIContent AdvancedSettings = new GUIContent("Advanced Settings");
            public readonly static GUIContent OnlyAlive = new GUIContent("Only Alive", "Process only those objects that have a health component\n(Any components that implemented by IHealth interface).");
            public readonly static GUIContent UpdateRate = new GUIContent("Update Rate", "How often in seconds to do a search targets.");

        }

        ArcHandle m_ArcHandle = new ArcHandle();
        private bool otherPropertiesFoldout;

        public override void InitializeProperties()
        {
            m_ArcHandle.SetColorWithRadiusHandle(Color.white, 0.1f);
        }

        /// <summary>
        /// Enables the Editor to handle an event in the Scene view.
        /// </summary>
        public override void DuringSceneGUI(SceneView sceneView)
        {
            UnityEditor.Handles.color = Color.white;
            UnityEditor.Handles.DrawWireArc(instance.transform.position, Vector3.up, Vector3.forward, 360, instance.GetViewRadius());

            UnityEditor.Handles.color = new Color32(100, 100, 100, 100);
            UnityEditor.Handles.DrawSolidDisc(instance.transform.position, Vector3.up, instance.GetViewRadius());

            UnityEditor.Handles.color = Color.yellow;
            // Angle handlers
            Vector3 viewAngleA = DirFromAngle(-instance.GetViewAngle() / 2, false);
            Vector3 viewAngleB = DirFromAngle(instance.GetViewAngle() / 2, false);

            Vector3 viewAngleARelative = instance.transform.position + viewAngleA * instance.GetViewRadius();
            Vector3 viewAngleBRelative = instance.transform.position + viewAngleB * instance.GetViewRadius();

            m_ArcHandle.angle = instance.GetViewAngle();
            m_ArcHandle.radius = instance.GetViewRadius();

            Matrix4x4 handleMatrix = Matrix4x4.TRS(
                instance.transform.position,
                instance.transform.rotation * Quaternion.Euler(0, -instance.GetViewAngle() / 2, 0),
                Vector3.one);

            using(new UnityEditor.Handles.DrawingScope(handleMatrix))
            {
                EditorGUI.BeginChangeCheck();
                m_ArcHandle.DrawHandle();
                if (EditorGUI.EndChangeCheck())
                {
                    instance.SetViewAngle(m_ArcHandle.angle);
                    instance.SetViewRadius(m_ArcHandle.radius);
                }
            }

            UnityEditor.Handles.color = Color.red;
            for (int i = 0, length = instance.GetVisibleTargetCount(); i < length; i++)
            {
                Transform visibleTarget = instance.GetVisibleTarget(i);
                Vector3 offset = Vector3.up * instance.GetViewOffset();
                Vector3 origin = instance.transform.position + offset;
                Vector3 direction = visibleTarget.position + offset;
                UnityEditor.Handles.DrawLine(origin, direction);
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
            instance.SetTargetMask(AEditorGUILayout.LayerMaskField(ContentProperties.TargetMask, instance.GetTargetMask()));
            instance.SetObstacleMask(AEditorGUILayout.LayerMaskField(ContentProperties.ObstacleMask, instance.GetObstacleMask()));
            instance.SetViewRadius(AEditorGUILayout.FixedFloatField(ContentProperties.ViewRadius, instance.GetViewRadius(), 0));
            instance.SetViewAngle(EditorGUILayout.Slider(ContentProperties.ViewAngle, instance.GetViewAngle(), 0.0f, 360.0f));
            instance.SetViewOffset(EditorGUILayout.FloatField(ContentProperties.ViewOffset, instance.GetViewOffset()));

            IncreaseIndentLevel();
            BeginGroupLevel2(ref otherPropertiesFoldout, ContentProperties.AdvancedSettings);
            if (otherPropertiesFoldout)
            {
                instance.OnlyAlive(EditorGUILayout.Toggle(ContentProperties.OnlyAlive, instance.OnlyAlive()));
                instance.SetUpdateRate(AEditorGUILayout.FixedFloatField(ContentProperties.UpdateRate, instance.GetUpdateRate(), 0.001f));
                if (instance.GetUpdateRate() < 0.01f)
                {
                    HelpBoxMessages.Message("Calling too often for a lot of AI can be bad for performance.", MessageType.Warning, true);
                }
            }
            EndGroupLevel();
            DecreaseIndentLevel();
            EndGroup();
        }

        public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal)
            {
                angleInDegrees += instance.transform.eulerAngles.y;
            }
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
    }
}