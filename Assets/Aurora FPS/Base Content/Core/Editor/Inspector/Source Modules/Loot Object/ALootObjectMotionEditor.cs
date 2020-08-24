/* ==================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================== */

using AuroraFPSRuntime;
using UnityEditor;
using UnityEngine;

namespace AuroraFPSEditor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(LootObjectMotion))]
    public class ALootObjectMotionEditor : AuroraEditor<LootObjectMotion>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent VerticalMovement = new GUIContent("Vertical Movement");
            public readonly static GUIContent Range = new GUIContent("Range");
            public readonly static GUIContent VerticalSpeed = new GUIContent("Speed", "Vertical speed.");
            public readonly static GUIContent Offset = new GUIContent("Offset", "Vertical offset.");
            public readonly static GUIContent UseMovement = new GUIContent("Use Movement");
            public readonly static GUIContent HorizontalRotation = new GUIContent("HorizontalRotation");
            public readonly static GUIContent HorizontalSpeed = new GUIContent("Speed", "Horizontal speed.");
            public readonly static GUIContent Direction = new GUIContent("Direction", "Horizontal direction rotation.");
            public readonly static GUIContent UseRotation = new GUIContent("Use Rotation");
        }

        public enum Direction
        {
            Up,
            Down,
            Left,
            Right,
            LeftSide,
            RightSide,
            Custom
        }

        private Direction direction;

        public override void InitializeProperties()
        {
            direction = ParseVectorToDirection(instance.GetDirection());
        }

        /// <summary>
        /// Base serializedObject properties.
        /// 
        /// Implement this method to draw a custom properties.
        /// </summary>
        public override void OnBaseGUI()
        {
            BeginGroup(ContentProperties.VerticalMovement);
            instance.SetRange(AEditorGUILayout.FixedFloatField(ContentProperties.Range, instance.GetRange(), 0));
            instance.SetVerticalSpeed(AEditorGUILayout.FixedFloatField(ContentProperties.VerticalSpeed, instance.GetVerticalSpeed(), 0));
            instance.SetOffset(AEditorGUILayout.FixedFloatField(ContentProperties.Offset, instance.GetOffset(), 0));
            instance.UseMovement(EditorGUILayout.Toggle(ContentProperties.UseMovement, instance.UseMovement()));
            EndGroup();

            BeginGroup(ContentProperties.HorizontalRotation);
            instance.SetHorizontalSpeed(AEditorGUILayout.FixedFloatField(ContentProperties.HorizontalSpeed, instance.GetHorizontalSpeed(), 0));
            direction = AEditorGUILayout.EnumPopup(ContentProperties.Direction, direction);
            if (direction == Direction.Custom)
                instance.SetDirection(EditorGUILayout.Vector3Field(ContentProperties.Direction, instance.GetDirection()));
            else
                instance.SetDirection(ParseDirectionToVector(direction));
            instance.UseRotation(EditorGUILayout.Toggle(ContentProperties.UseRotation, instance.UseRotation()));
            EndGroup();
        }

        public Direction ParseVectorToDirection(Vector3 vector)
        {
            if (vector == Vector3.left)
                return Direction.Up;
            else if (vector == Vector3.right)
                return Direction.Down;
            else if (vector == Vector3.down)
                return Direction.Left;
            else if (vector == Vector3.up)
                return Direction.Right;
            else if (vector == Vector3.forward)
                return Direction.LeftSide;
            else if (vector == Vector3.back)
                return Direction.RightSide;
            else
                return Direction.Custom;
        }

        public Vector3 ParseDirectionToVector(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return Vector3.left;
                case Direction.Down:
                    return Vector3.right;
                case Direction.Left:
                    return Vector3.down;
                case Direction.Right:
                    return Vector3.up;
                case Direction.LeftSide:
                    return Vector3.forward;
                case Direction.RightSide:
                    return Vector3.back;
            }
            return Vector3.zero;
        }
    }
}