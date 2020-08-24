/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AuroraFPSEditor
{
    public sealed class Handles
    {
        private static Color ActiveHandleColor = Color.white;
        private static Color ActiveWireColor = Color.white;
        private static Color ActiveHandleHighlightedColor = new Color(0.0f, 0.0f, 1.0f, 0.3f);
        private static Color ActiveWireHighlightedColor = new Color(0.0f, 0.0f, 1.0f, 1.0f);

        public static Color HandleColor = Color.white;
        public static Color WireColor = Color.white;
        public static Color HandleHighlightedColor = new Color(0.0f, 0.0f, 1.0f, 0.3f);
        public static Color WireHighlightedColor = new Color(0.0f, 0.0f, 1.0f, 1.0f);
        public static Color HandleRemoveColor = new Color(1.0f, 0.0f, 0.0f, 0.3f);
        public static Color WireRemoveColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        public static Color LineColor = Color.white;

        /// <summary>
        /// Draw cube with highlighted edges.
        /// </summary>
        /// <param name="controlID">The control ID for the handle.</param>
        /// <param name="position">The position of the handle in the space of Handles.matrix.</param>
        /// <param name="rotation">The rotation of the handle in the space of Handles.matrix.</param>
        /// <param name="size">The size of the handle in world-space units.</param>
        /// <param name="eventType">Event type for the handle to act upon. By design it handles EventType.Layout
        ///     and EventType.Repaint events.</param>
        public static void DrawWiredCube(int controlID, Vector3 position, Quaternion rotation, float size, EventType eventType)
        {
            float dist = size + 20;
            Vector3 pos = HandleUtility.WorldToGUIPoint(position);
            if (Vector2.Distance(pos, Event.current.mousePosition) < dist)
            {
                HandleUtility.AddDefaultControl(controlID);
                ActiveHandleColor = ActiveHandleHighlightedColor;
                ActiveWireColor = ActiveWireHighlightedColor;
            }
            else
            {
                ActiveHandleColor = HandleColor;
                ActiveWireColor = WireColor;
            }

            UnityEditor.Handles.color = ActiveHandleColor;
            UnityEditor.Handles.CubeHandleCap(controlID, position, Quaternion.identity, size, eventType);

            UnityEditor.Handles.color = ActiveWireColor;
            UnityEditor.Handles.DrawWireCube(position, new Vector3(size, size, size));

            UnityEditor.Handles.color = Color.white;
        }

        /// <summary>
        /// Draw sphere with highlighted edges.
        /// </summary>
        /// <param name="controlID">The control ID for the handle.</param>
        /// <param name="position">The position of the handle in the space of Handles.matrix.</param>
        /// <param name="rotation">The rotation of the handle in the space of Handles.matrix.</param>
        /// <param name="size">The size of the handle in world-space units.</param>
        /// <param name="eventType">Event type for the handle to act upon. By design it handles EventType.Layout
        ///     and EventType.Repaint events.</param>
        public static void DrawWiredSphere(int controlID, Vector3 position, Quaternion rotation, float size, EventType eventType)
        {
            float dist = size + 20;
            Vector3 pos = HandleUtility.WorldToGUIPoint(position);
            if (Vector2.Distance(pos, Event.current.mousePosition) < dist)
            {
                HandleUtility.AddDefaultControl(controlID);
                ActiveHandleColor = ActiveHandleHighlightedColor;
                ActiveWireColor = ActiveWireHighlightedColor;
            }
            else
            {
                ActiveHandleColor = HandleColor;
                ActiveWireColor = WireColor;
            }

            UnityEditor.Handles.color = ActiveHandleColor;
            UnityEditor.Handles.SphereHandleCap(controlID, position, Quaternion.identity, size, EventType.Repaint);

            float wireRadius = size / 2;
            UnityEditor.Handles.color = ActiveWireColor;
            UnityEditor.Handles.DrawWireDisc(position, Vector3.up, wireRadius);
            UnityEditor.Handles.DrawWireDisc(position, Vector3.right, wireRadius);
            UnityEditor.Handles.DrawWireDisc(position, Vector3.forward, wireRadius);

            UnityEditor.Handles.color = Color.white;
        }

        /// <summary>
        /// Draw sphere wire.
        /// </summary>
        /// <param name="controlID">The control ID for the handle.</param>
        /// <param name="position">The position of the handle in the space of Handles.matrix.</param>
        /// <param name="rotation">The rotation of the handle in the space of Handles.matrix.</param>
        /// <param name="size">The size of the handle in world-space units.</param>
        /// <param name="eventType">Event type for the handle to act upon. By design it handles EventType.Layout
        ///     and EventType.Repaint events.</param>
        public static void DrawWireSphere(int controlID, Vector3 position, Quaternion rotation, float size, EventType eventType)
        {
            float dist = size + 20;
            Vector3 pos = HandleUtility.WorldToGUIPoint(position);
            if (Vector2.Distance(pos, Event.current.mousePosition) < dist)
            {
                HandleUtility.AddDefaultControl(controlID);
                ActiveHandleColor = ActiveHandleHighlightedColor;
                ActiveWireColor = ActiveWireHighlightedColor;
            }
            else
            {
                ActiveHandleColor = HandleColor;
                ActiveWireColor = WireColor;
            }

            UnityEditor.Handles.color = ActiveHandleColor;
            UnityEditor.Handles.DrawWireDisc(position, Vector3.up, size);
            UnityEditor.Handles.DrawWireDisc(position, Vector3.right, size);
            UnityEditor.Handles.DrawWireDisc(position, Vector3.forward, size);
        }

        /// <summary>
        /// Draw half of sphere.
        /// </summary>
        /// <param name="controlID">The control ID for the handle.</param>
        /// <param name="position">The position of the handle in the space of Handles.matrix.</param>
        /// <param name="rotation">The rotation of the handle in the space of Handles.matrix.</param>
        /// <param name="size">The size of the handle in world-space units.</param>
        /// <param name="eventType">Event type for the handle to act upon. By design it handles EventType.Layout
        ///     and EventType.Repaint events.</param>
        public static void DrawSphereHalf(int controlID, Vector3 position, Quaternion rotation, float size, EventType eventType)
        {
            float dist = size + 20;
            Vector3 pos = HandleUtility.WorldToGUIPoint(position);
            if (Vector2.Distance(pos, Event.current.mousePosition) < dist)
            {
                HandleUtility.AddDefaultControl(controlID);
                ActiveHandleColor = ActiveHandleHighlightedColor;
                ActiveWireColor = ActiveWireHighlightedColor;
            }
            else
            {
                ActiveHandleColor = HandleColor;
                ActiveWireColor = WireColor;
            }

            UnityEditor.Handles.color = ActiveHandleColor;
            UnityEditor.Handles.DrawWireArc(position, Vector3.right, Vector3.back, 180, size);
            UnityEditor.Handles.DrawWireArc(position, Vector3.forward, Vector3.right, 180, size);
            UnityEditor.Handles.DrawWireDisc(position, Vector3.up, size);
        }

        /// <summary>
        /// Draw half of sphere with solid bottom.
        /// </summary>
        /// <param name="controlID">The control ID for the handle.</param>
        /// <param name="position">The position of the handle in the space of Handles.matrix.</param>
        /// <param name="rotation">The rotation of the handle in the space of Handles.matrix.</param>
        /// <param name="size">The size of the handle in world-space units.</param>
        /// <param name="eventType">Event type for the handle to act upon. By design it handles EventType.Layout
        ///     and EventType.Repaint events.</param>
        public static void DrawSphereHalfWithSolidBottom(int controlID, Vector3 position, Quaternion rotation, float size, EventType eventType)
        {
            float dist = size + 20;
            Vector3 pos = HandleUtility.WorldToGUIPoint(position);
            if (Vector2.Distance(pos, Event.current.mousePosition) < dist)
            {
                HandleUtility.AddDefaultControl(controlID);
                ActiveHandleColor = ActiveHandleHighlightedColor;
                ActiveWireColor = ActiveWireHighlightedColor;
            }
            else
            {
                ActiveHandleColor = HandleColor;
                ActiveWireColor = WireColor;
            }

            UnityEditor.Handles.color = ActiveHandleColor;
            UnityEditor.Handles.DrawSolidDisc(position, Vector3.up, size);

            UnityEditor.Handles.color = ActiveWireColor;
            UnityEditor.Handles.DrawWireArc(position, Vector3.right, Vector3.back, 180, size);
            UnityEditor.Handles.DrawWireArc(position, Vector3.forward, Vector3.right, 180, size);
            UnityEditor.Handles.DrawWireDisc(position, Vector3.up, size);

            UnityEditor.Handles.color = Color.white;
        }

        /// <summary>
        /// Draw half of sphere.
        /// </summary>
        /// <param name="controlID">The control ID for the handle.</param>
        /// <param name="position">The position of the handle in the space of Handles.matrix.</param>
        /// <param name="rotation">The rotation of the handle in the space of Handles.matrix.</param>
        /// <param name="size">The size of the handle in world-space units.</param>
        /// <param name="eventType">Event type for the handle to act upon. By design it handles EventType.Layout
        ///     and EventType.Repaint events.</param>
        public static void DrawPositionPointArrow(int controlID, Vector3 position, Quaternion rotation, float size, EventType eventType)
        {
            float dist = size + 20;
            Vector3 pos = HandleUtility.WorldToGUIPoint(position);
            if (Vector2.Distance(pos, Event.current.mousePosition) < dist)
            {
                HandleUtility.AddDefaultControl(controlID);
                ActiveHandleColor = ActiveHandleHighlightedColor;
                ActiveWireColor = ActiveWireHighlightedColor;
            }
            else
            {
                ActiveHandleColor = HandleColor;
                ActiveWireColor = WireColor;
            }

            float offset = 2.7f;

            UnityEditor.Handles.color = ActiveHandleColor;
            UnityEditor.Handles.ArrowHandleCap(controlID, position + (Vector3.up * offset), Quaternion.LookRotation(-Vector3.up), 2, eventType);
            UnityEditor.Handles.DrawWireDisc(position, Vector3.up, 0.1f);
            UnityEditor.Handles.DrawWireDisc(position + (Vector3.up * offset), Vector3.up, 0.1f);
            UnityEditor.Handles.DrawSolidDisc(position + (Vector3.up * offset), Vector3.up, 0.1f);

        }

        public static void DrawSequentalTypeLines(Vector3[] points, bool drawLabel = true)
        {
            if (points.Length > 0)
            {
                Vector3 startPosition = points[0];
                int lastIndex = points.Length - 1;
                for (int i = 0, length = points.Length; i < length; i++)
                {
                    Vector3 destination = points[i];

                    UnityEditor.Handles.color = LineColor;
                    if (i + 1 < length)
                    {
                        UnityEditor.Handles.DrawDottedLine(destination, points[i + 1], 5);
                    }
                    else if (i == lastIndex)
                    {
                        UnityEditor.Handles.DrawDottedLine(destination, startPosition, 5);
                    }
                    UnityEditor.Handles.Label(destination, (i + 1).ToString());
                }
            }
        }

        public static void DrawRandomTypeLines(Vector3[] points, bool drawLabel = true)
        {
            for (int i = 0, length = points.Length; i < length; i++)
            {
                Vector3 destination = points[i];
                UnityEditor.Handles.color = LineColor;
                for (int j = 0; j < length; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }
                    UnityEditor.Handles.DrawDottedLine(destination, points[j], 5);
                    UnityEditor.Handles.Label(destination, (i + 1).ToString());
                }
            }
        }

        public static void DrawFiniteTypeLines(Vector3[] points, bool drawLabel = true)
        {
            if (points.Length > 0)
            {
                Vector3 startPosition = points[0];
                int lastIndex = points.Length - 1;
                for (int i = 0, length = points.Length; i < length; i++)
                {
                    Vector3 destination = points[i];

                    UnityEditor.Handles.color = LineColor;
                    if (i + 1 < length)
                        UnityEditor.Handles.DrawDottedLine(destination, points[i + 1], 5);
                    else if (i == lastIndex)
                        UnityEditor.Handles.DrawDottedLine(destination, startPosition, 5);

                    if (drawLabel)
                    {
                        if (i == 0)
                        {
                            UnityEditor.Handles.Label(destination, string.Format("{0} - [START]", (i + 1)));
                        }
                        else if (i == lastIndex)
                        {
                            UnityEditor.Handles.Label(destination, string.Format("{0} - [END]", (i + 1)));
                        }
                        else
                        {
                            UnityEditor.Handles.Label(destination, (i + 1).ToString());
                        }
                    }
                }
            }
        }

        public static void DrawSequentalTypeLines(List<Vector3> points, bool drawLabel = true)
        {
            if (points.Count > 0)
            {
                Vector3 startPosition = points[0];
                int lastIndex = points.Count - 1;
                for (int i = 0, length = points.Count; i < length; i++)
                {
                    Vector3 destination = points[i];

                    UnityEditor.Handles.color = LineColor;
                    if (i + 1 < length)
                    {
                        UnityEditor.Handles.DrawDottedLine(destination, points[i + 1], 5);
                    }
                    else if (i == lastIndex)
                    {
                        UnityEditor.Handles.DrawDottedLine(destination, startPosition, 5);
                    }
                    UnityEditor.Handles.Label(destination, (i + 1).ToString());
                }
            }
        }

        public static void DrawRandomTypeLines(List<Vector3> points, bool drawLabel = true)
        {
            for (int i = 0, length = points.Count; i < length; i++)
            {
                Vector3 destination = points[i];
                UnityEditor.Handles.color = LineColor;
                for (int j = 0; j < length; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }
                    UnityEditor.Handles.DrawDottedLine(destination, points[j], 5);
                    UnityEditor.Handles.Label(destination, (i + 1).ToString());
                }
            }
        }

        public static void DrawFiniteTypeLines(List<Vector3> points, bool drawLabel = true)
        {
            if (points.Count > 0)
            {
                Vector3 startPosition = points[0];
                int lastIndex = points.Count - 1;
                for (int i = 0, length = points.Count; i < length; i++)
                {
                    Vector3 destination = points[i];

                    UnityEditor.Handles.color = LineColor;
                    if (i + 1 < length)
                        UnityEditor.Handles.DrawDottedLine(destination, points[i + 1], 5);
                    else if (i == lastIndex)
                        UnityEditor.Handles.DrawDottedLine(destination, startPosition, 5);

                    if (drawLabel)
                    {
                        if (i == 0)
                        {
                            UnityEditor.Handles.Label(destination, string.Format("{0} - [START]", (i + 1)));
                        }
                        else if (i == lastIndex)
                        {
                            UnityEditor.Handles.Label(destination, string.Format("{0} - [END]", (i + 1)));
                        }
                        else
                        {
                            UnityEditor.Handles.Label(destination, (i + 1).ToString());
                        }
                    }
                }
            }
        }

        public static void DrawSequentalTypeLines(Transform[] points, bool drawLabel = true)
        {
            if (points.Length > 0)
            {
                Transform startDestination = points[0];
                if (startDestination != null)
                {
                    int lastIndex = points.Length - 1;
                    for (int i = 0, length = points.Length; i < length; i++)
                    {
                        Transform destination = points[i];
                        if (destination != null)
                        {
                            UnityEditor.Handles.color = LineColor;
                            if (i + 1 < length)
                            {
                                Transform nextDestination = points[i + 1];
                                if (nextDestination != null)
                                {
                                    UnityEditor.Handles.DrawDottedLine(destination.position, nextDestination.position, 5);
                                }
                            }
                            else if (i == lastIndex)
                            {
                                UnityEditor.Handles.DrawDottedLine(destination.position, startDestination.position, 5);
                            }
                            UnityEditor.Handles.Label(destination.position, (i + 1).ToString());
                        }
                    }
                }
            }
        }

        public static void DrawRandomTypeLines(Transform[] points, bool drawLabel = true)
        {
            for (int i = 0, length = points.Length; i < length; i++)
            {
                Transform destination = points[i];
                if (destination != null)
                {
                    UnityEditor.Handles.color = LineColor;
                    for (int j = 0; j < length; j++)
                    {
                        if (i == j)
                        {
                            continue;
                        }
                        
                        Transform nextDestination = points[j];
                        if (nextDestination != null)
                        {
                            UnityEditor.Handles.DrawDottedLine(destination.position, nextDestination.position, 5);
                            UnityEditor.Handles.Label(destination.position, (i + 1).ToString());
                        }
                    }
                }

            }
        }

        public static void DrawFiniteTypeLines(Transform[] points, bool drawLabel = true)
        {
            if (points.Length > 0)
            {
                Transform startDestination = points[0];
                if (startDestination != null)
                {
                    int lastIndex = points.Length - 1;
                    for (int i = 0, length = points.Length; i < length; i++)
                    {
                        Transform destination = points[i];

                        if (destination != null)
                        {
                            UnityEditor.Handles.color = LineColor;
                            if (i + 1 < length)
                            {
                                Transform nextDestination = points[i + 1];
                                if (nextDestination)
                                {
                                    UnityEditor.Handles.DrawDottedLine(destination.position, nextDestination.position, 5);
                                }
                            }
                            else if (i == lastIndex)
                            {
                                UnityEditor.Handles.DrawDottedLine(destination.position, startDestination.position, 5);
                            }

                            if (drawLabel)
                            {
                                if (i == 0)
                                {
                                    UnityEditor.Handles.Label(destination.position, string.Format("{0} - [START]", (i + 1)));
                                }
                                else if (i == lastIndex)
                                {
                                    UnityEditor.Handles.Label(destination.position, string.Format("{0} - [END]", (i + 1)));
                                }
                                else
                                {
                                    UnityEditor.Handles.Label(destination.position, (i + 1).ToString());
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void DrawSequentalTypeLines(List<Transform> points, bool drawLabel = true)
        {
            if (points.Count > 0)
            {
                Transform startDestination = points[0];
                if (startDestination != null)
                {
                    int lastIndex = points.Count - 1;
                    for (int i = 0, length = points.Count; i < length; i++)
                    {
                        Transform destination = points[i];
                        if (destination != null)
                        {
                            UnityEditor.Handles.color = LineColor;
                            if (i + 1 < length)
                            {
                                Transform nextDestination = points[i + 1];
                                if (nextDestination != null)
                                {
                                    UnityEditor.Handles.DrawDottedLine(destination.position, nextDestination.position, 5);
                                }
                            }
                            else if (i == lastIndex)
                            {
                                UnityEditor.Handles.DrawDottedLine(destination.position, startDestination.position, 5);
                            }
                            UnityEditor.Handles.Label(destination.position, (i + 1).ToString());

                        }
                    }
                }
            }
        }

        public static void DrawRandomTypeLines(List<Transform> points, bool drawLabel = true)
        {
            for (int i = 0, length = points.Count; i < length; i++)
            {
                Transform destination = points[i];
                if (destination != null)
                {
                    UnityEditor.Handles.color = LineColor;
                    for (int j = 0; j < length; j++)
                    {
                        if (i == j)
                        {
                            continue;
                        }

                        Transform nextDestination = points[j];
                        if (nextDestination != null)
                        {
                            UnityEditor.Handles.DrawDottedLine(destination.position, nextDestination.position, 5);
                            UnityEditor.Handles.Label(destination.position, (i + 1).ToString());
                        }
                    }
                }
            }
        }

        public static void DrawFiniteTypeLines(List<Transform> points, bool drawLabel = true)
        {
            if (points.Count > 0)
            {
                Transform startDestination = points[0];
                if (startDestination != null)
                {
                    int lastIndex = points.Count - 1;
                    for (int i = 0, length = points.Count; i < length; i++)
                    {
                        Transform destination = points[i];
                        if (destination != null)
                        {
                            UnityEditor.Handles.color = LineColor;
                            if (i + 1 < length)
                            {
                                Transform nextDestination = points[i + 1];
                                if (nextDestination != null)
                                {
                                    UnityEditor.Handles.DrawDottedLine(destination.position, nextDestination.position, 5);
                                }
                            }
                            else if (i == lastIndex)
                            {
                                UnityEditor.Handles.DrawDottedLine(destination.position, startDestination.position, 5);
                            }

                            if (drawLabel)
                            {
                                if (i == 0)
                                {
                                    UnityEditor.Handles.Label(destination.position, string.Format("{0} - [START]", (i + 1)));
                                }
                                else if (i == lastIndex)
                                {
                                    UnityEditor.Handles.Label(destination.position, string.Format("{0} - [END]", (i + 1)));
                                }
                                else
                                {
                                    UnityEditor.Handles.Label(destination.position, (i + 1).ToString());
                                }
                            }
                        }
                    }
                }
            }
        }

        public static Vector3[] PointsFreeMoveHandle(int hashCode, Vector3[] points, float size, UnityEditor.Handles.CapFunction capFunction)
        {
            Event current = Event.current;

            Ray ray = HandleUtility.GUIPointToWorldRay(current.mousePosition);
            RaycastHit hit;

            UnityEditor.Handles.CubeHandleCap(0, current.mousePosition, Quaternion.identity, 1, EventType.Repaint);

            if (current.control && !current.shift)
            {
                if (current.type == EventType.Layout)
                {
                    HandleUtility.AddDefaultControl(GUIUtility.GetControlID(hashCode, FocusType.Passive));
                }

                if (current.button == 0 && current.type == EventType.MouseDown && Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    System.Array.Resize<Vector3>(ref points, points.Length + 1);
                    points[points.Length - 1] = hit.point;
                }
            }

            for (int i = 0, length = points.Length; i < length; i++)
            {
                Vector3 point = points[i];

                if (current.control && current.shift)
                {
                    if (current.type == EventType.Layout)
                    {
                        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(hashCode, FocusType.Passive));
                    }
                    Vector3 pos = HandleUtility.WorldToGUIPoint(point);
                    if (Vector2.Distance(pos, current.mousePosition) < 20)
                    {
                        Handles.ActiveHandleHighlightedColor = HandleRemoveColor;
                        Handles.ActiveWireHighlightedColor = WireRemoveColor;
                        if (current.button == 0 && current.type == EventType.MouseDown)
                        {
                            List<Vector3> pointsList = points.ToList();
                            pointsList.RemoveAt(i);
                            points = pointsList.ToArray();
                            break;
                        }
                    }
                }
                else
                {
                    Handles.ActiveHandleHighlightedColor = HandleHighlightedColor;
                    Handles.ActiveWireHighlightedColor = WireHighlightedColor;
                }

                point = UnityEditor.Handles.FreeMoveHandle(point, Quaternion.identity, size, Vector3.up, capFunction);
                points[i] = point;
                UnityEditor.Handles.color = Color.white;
            }
            return points;
        }

        public static Vector3[] PointsFreeMoveHandle(int hashCode, Vector3[] points)
        {
            Event current = Event.current;

            Ray ray = HandleUtility.GUIPointToWorldRay(current.mousePosition);
            RaycastHit hit;

            UnityEditor.Handles.CubeHandleCap(0, current.mousePosition, Quaternion.identity, 1, EventType.Repaint);

            if (current.control && !current.shift)
            {
                if (current.type == EventType.Layout)
                {
                    HandleUtility.AddDefaultControl(GUIUtility.GetControlID(hashCode, FocusType.Passive));
                }

                if (current.button == 0 && current.type == EventType.MouseDown && Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    System.Array.Resize<Vector3>(ref points, points.Length + 1);
                    points[points.Length - 1] = hit.point;
                }
            }

            for (int i = 0, length = points.Length; i < length; i++)
            {
                Vector3 point = points[i];

                if (current.control && current.shift)
                {
                    if (current.type == EventType.Layout)
                    {
                        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(hashCode, FocusType.Passive));
                    }
                    Vector3 pos = HandleUtility.WorldToGUIPoint(point);
                    if (Vector2.Distance(pos, current.mousePosition) < 20)
                    {
                        Handles.ActiveHandleHighlightedColor = HandleRemoveColor;
                        Handles.ActiveWireHighlightedColor = WireRemoveColor;
                        if (current.button == 0 && current.type == EventType.MouseDown)
                        {
                            List<Vector3> pointsList = points.ToList();
                            pointsList.RemoveAt(i);
                            points = pointsList.ToArray();
                            break;
                        }
                    }
                }
                else
                {
                    Handles.ActiveHandleHighlightedColor = HandleHighlightedColor;
                    Handles.ActiveWireHighlightedColor = WireHighlightedColor;
                }

                point = UnityEditor.Handles.FreeMoveHandle(point, Quaternion.identity, 1, Vector3.up, DrawSphereHalfWithSolidBottom);
                points[i] = point;
                UnityEditor.Handles.color = Color.white;
            }
            return points;
        }

        public static List<Vector3> PointsFreeMoveHandle(int hashCode, List<Vector3> points, float size, UnityEditor.Handles.CapFunction capFunction)
        {
            Event current = Event.current;

            Ray ray = HandleUtility.GUIPointToWorldRay(current.mousePosition);
            RaycastHit hit;

            UnityEditor.Handles.CubeHandleCap(0, current.mousePosition, Quaternion.identity, 1, EventType.Repaint);

            if (current.control && !current.shift)
            {
                if (current.type == EventType.Layout)
                {
                    HandleUtility.AddDefaultControl(GUIUtility.GetControlID(hashCode, FocusType.Passive));
                }

                if (current.button == 0 && current.type == EventType.MouseDown && Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    points.Add(hit.point);
                }
            }

            for (int i = 0, length = points.Count; i < length; i++)
            {
                Vector3 point = points[i];

                if (current.control && current.shift)
                {
                    if (current.type == EventType.Layout)
                    {
                        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(hashCode, FocusType.Passive));
                    }
                    Vector3 pos = HandleUtility.WorldToGUIPoint(point);
                    if (Vector2.Distance(pos, current.mousePosition) < 20)
                    {
                        Handles.ActiveHandleHighlightedColor = HandleRemoveColor;
                        Handles.ActiveWireHighlightedColor = WireRemoveColor;
                        if (current.button == 0 && current.type == EventType.MouseDown)
                        {
                            points.RemoveAt(i);
                            break;
                        }
                    }
                }
                else
                {
                    Handles.ActiveHandleHighlightedColor = HandleHighlightedColor;
                    Handles.ActiveWireHighlightedColor = WireHighlightedColor;
                }

                point = UnityEditor.Handles.FreeMoveHandle(point, Quaternion.identity, size, Vector3.up, capFunction);
                points[i] = point;
                UnityEditor.Handles.color = Color.white;
            }
            return points;
        }

        public static List<Vector3> PointsFreeMoveHandle(int hashCode, List<Vector3> points)
        {
            Event current = Event.current;

            Ray ray = HandleUtility.GUIPointToWorldRay(current.mousePosition);
            RaycastHit hit;

            UnityEditor.Handles.CubeHandleCap(0, current.mousePosition, Quaternion.identity, 1, EventType.Repaint);

            if (current.control && !current.shift)
            {
                if (current.type == EventType.Layout)
                {
                    HandleUtility.AddDefaultControl(GUIUtility.GetControlID(hashCode, FocusType.Passive));
                }

                if (current.button == 0 && current.type == EventType.MouseDown && Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    points.Add(hit.point);
                }
            }

            for (int i = 0, length = points.Count; i < length; i++)
            {
                Vector3 point = points[i];

                if (current.control && current.shift)
                {
                    if (current.type == EventType.Layout)
                    {
                        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(hashCode, FocusType.Passive));
                    }
                    Vector3 pos = HandleUtility.WorldToGUIPoint(point);
                    if (Vector2.Distance(pos, current.mousePosition) < 20)
                    {
                        Handles.ActiveHandleHighlightedColor = HandleRemoveColor;
                        Handles.ActiveWireHighlightedColor = WireRemoveColor;
                        if (current.button == 0 && current.type == EventType.MouseDown)
                        {
                            points.RemoveAt(i);
                            break;
                        }
                    }
                }
                else
                {
                    Handles.ActiveHandleHighlightedColor = HandleHighlightedColor;
                    Handles.ActiveWireHighlightedColor = WireHighlightedColor;
                }

                point = UnityEditor.Handles.FreeMoveHandle(point, Quaternion.identity, 1, Vector3.up, DrawSphereHalfWithSolidBottom);
                points[i] = point;
                UnityEditor.Handles.color = Color.white;
            }
            return points;
        }

        public static void PointsFreeMoveHandle(int hashCode, Transform[] points, float size, UnityEditor.Handles.CapFunction capFunction)
        {
            Event current = Event.current;

            Ray ray = HandleUtility.GUIPointToWorldRay(current.mousePosition);
            RaycastHit hit;

            UnityEditor.Handles.CubeHandleCap(0, current.mousePosition, Quaternion.identity, 1, EventType.Repaint);

            if (current.control && !current.shift)
            {
                if (current.type == EventType.Layout)
                {
                    HandleUtility.AddDefaultControl(GUIUtility.GetControlID(hashCode, FocusType.Passive));
                }

                if (current.button == 0 && current.type == EventType.MouseDown && Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    System.Array.Resize<Transform>(ref points, points.Length + 1);
                    points[points.Length - 1] = new GameObject().transform;
                    points[points.Length - 1].position = hit.point;
                }
            }

            for (int i = 0, length = points.Length; i < length; i++)
            {
                if (points[i] == null)
                {
                    continue;
                }

                Vector3 point = points[i].position;

                if (current.control && current.shift)
                {
                    if (current.type == EventType.Layout)
                    {
                        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(hashCode, FocusType.Passive));
                    }
                    Vector3 pos = HandleUtility.WorldToGUIPoint(point);
                    if (Vector2.Distance(pos, current.mousePosition) < 20)
                    {
                        Handles.ActiveHandleHighlightedColor = HandleRemoveColor;
                        Handles.ActiveWireHighlightedColor = WireRemoveColor;
                        if (current.button == 0 && current.type == EventType.MouseDown)
                        {
                            List<Transform> pointsList = points.ToList();
                            GameObject.DestroyImmediate(pointsList[i].gameObject);
                            pointsList.RemoveAt(i);
                            points = pointsList.ToArray();
                            break;
                        }
                    }
                }
                else
                {
                    Handles.ActiveHandleHighlightedColor = HandleHighlightedColor;
                    Handles.ActiveWireHighlightedColor = WireHighlightedColor;
                }

                point = UnityEditor.Handles.FreeMoveHandle(point, Quaternion.identity, size, Vector3.up, capFunction);
                points[i].position = point;
                UnityEditor.Handles.color = Color.white;
            }
        }

        public static void PointsFreeMoveHandle(int hashCode, Transform[] points)
        {
            Event current = Event.current;

            Ray ray = HandleUtility.GUIPointToWorldRay(current.mousePosition);
            RaycastHit hit;

            UnityEditor.Handles.CubeHandleCap(0, current.mousePosition, Quaternion.identity, 1, EventType.Repaint);

            if (current.control && !current.shift)
            {
                if (current.type == EventType.Layout)
                {
                    HandleUtility.AddDefaultControl(GUIUtility.GetControlID(hashCode, FocusType.Passive));
                }

                if (current.button == 0 && current.type == EventType.MouseDown && Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    System.Array.Resize<Transform>(ref points, points.Length + 1);
                    points[points.Length - 1] = new GameObject().transform;
                    points[points.Length - 1].position = hit.point;
                }
            }

            for (int i = 0, length = points.Length; i < length; i++)
            {
                if (points[i] == null)
                {
                    continue;
                }

                Vector3 point = points[i].position;

                if (current.control && current.shift)
                {
                    if (current.type == EventType.Layout)
                    {
                        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(hashCode, FocusType.Passive));
                    }
                    Vector3 pos = HandleUtility.WorldToGUIPoint(point);
                    if (Vector2.Distance(pos, current.mousePosition) < 20)
                    {
                        Handles.ActiveHandleHighlightedColor = HandleRemoveColor;
                        Handles.ActiveWireHighlightedColor = WireRemoveColor;
                        if (current.button == 0 && current.type == EventType.MouseDown)
                        {
                            List<Transform> pointsList = points.ToList();
                            GameObject.DestroyImmediate(pointsList[i].gameObject);
                            pointsList.RemoveAt(i);
                            points = pointsList.ToArray();
                            break;
                        }
                    }
                }
                else
                {
                    Handles.ActiveHandleHighlightedColor = HandleHighlightedColor;
                    Handles.ActiveWireHighlightedColor = WireHighlightedColor;
                }

                point = UnityEditor.Handles.FreeMoveHandle(point, Quaternion.identity, 1, Vector3.up, DrawSphereHalfWithSolidBottom);
                points[i].position = point;
                UnityEditor.Handles.color = Color.white;
            }
        }

        public static void PointsFreeMoveHandle(int hashCode, Transform[] points, float size, UnityEditor.Handles.CapFunction capFunction, Action<GameObject> onCreate, Action<GameObject> onRemove)
        {
            Event current = Event.current;

            Ray ray = HandleUtility.GUIPointToWorldRay(current.mousePosition);
            RaycastHit hit;

            UnityEditor.Handles.CubeHandleCap(0, current.mousePosition, Quaternion.identity, 1, EventType.Repaint);

            if (current.control && !current.shift)
            {
                if (current.type == EventType.Layout)
                {
                    HandleUtility.AddDefaultControl(GUIUtility.GetControlID(hashCode, FocusType.Passive));
                }

                if (current.button == 0 && current.type == EventType.MouseDown && Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    System.Array.Resize<Transform>(ref points, points.Length + 1);
                    Transform newPoint = new GameObject().transform;
                    newPoint.position = hit.point;
                    onCreate?.Invoke(newPoint.gameObject);
                    points[points.Length - 1] = newPoint;
                }
            }

            for (int i = 0, length = points.Length; i < length; i++)
            {
                if (points[i] == null)
                {
                    continue;
                }

                Vector3 point = points[i].position;

                if (current.control && current.shift)
                {
                    if (current.type == EventType.Layout)
                    {
                        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(hashCode, FocusType.Passive));
                    }
                    Vector3 pos = HandleUtility.WorldToGUIPoint(point);
                    if (Vector2.Distance(pos, current.mousePosition) < 20)
                    {
                        Handles.ActiveHandleHighlightedColor = HandleRemoveColor;
                        Handles.ActiveWireHighlightedColor = WireRemoveColor;
                        if (current.button == 0 && current.type == EventType.MouseDown)
                        {
                            List<Transform> pointsList = points.ToList();
                            Transform removePoint = pointsList[i];
                            onRemove?.Invoke(removePoint.gameObject);
                            GameObject.DestroyImmediate(removePoint.gameObject);
                            pointsList.RemoveAt(i);
                            points = pointsList.ToArray();
                            break;
                        }
                    }
                }
                else
                {
                    Handles.ActiveHandleHighlightedColor = HandleHighlightedColor;
                    Handles.ActiveWireHighlightedColor = WireHighlightedColor;
                }

                point = UnityEditor.Handles.FreeMoveHandle(point, Quaternion.identity, size, Vector3.up, capFunction);
                points[i].position = point;
                UnityEditor.Handles.color = Color.white;
            }
        }

        public static void PointsFreeMoveHandle(int hashCode, Transform[] points, Action<GameObject> onCreate, Action<GameObject> onRemove)
        {
            Event current = Event.current;

            Ray ray = HandleUtility.GUIPointToWorldRay(current.mousePosition);
            RaycastHit hit;

            UnityEditor.Handles.CubeHandleCap(0, current.mousePosition, Quaternion.identity, 1, EventType.Repaint);

            if (current.control && !current.shift)
            {
                if (current.type == EventType.Layout)
                {
                    HandleUtility.AddDefaultControl(GUIUtility.GetControlID(hashCode, FocusType.Passive));
                }

                if (current.button == 0 && current.type == EventType.MouseDown && Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    System.Array.Resize<Transform>(ref points, points.Length + 1);
                    Transform newPoint = new GameObject().transform;
                    newPoint.position = hit.point;
                    onCreate?.Invoke(newPoint.gameObject);
                    points[points.Length - 1] = newPoint;
                }
            }

            for (int i = 0, length = points.Length; i < length; i++)
            {
                if (points[i] == null)
                {
                    continue;
                }

                Vector3 point = points[i].position;

                if (current.control && current.shift)
                {
                    if (current.type == EventType.Layout)
                    {
                        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(hashCode, FocusType.Passive));
                    }
                    Vector3 pos = HandleUtility.WorldToGUIPoint(point);
                    if (Vector2.Distance(pos, current.mousePosition) < 20)
                    {
                        Handles.ActiveHandleHighlightedColor = HandleRemoveColor;
                        Handles.ActiveWireHighlightedColor = WireRemoveColor;
                        if (current.button == 0 && current.type == EventType.MouseDown)
                        {
                            List<Transform> pointsList = points.ToList();
                            Transform removePoint = pointsList[i];
                            onRemove?.Invoke(removePoint.gameObject);
                            GameObject.DestroyImmediate(removePoint.gameObject);
                            pointsList.RemoveAt(i);
                            points = pointsList.ToArray();
                            break;
                        }
                    }
                }
                else
                {
                    Handles.ActiveHandleHighlightedColor = HandleHighlightedColor;
                    Handles.ActiveWireHighlightedColor = WireHighlightedColor;
                }

                point = UnityEditor.Handles.FreeMoveHandle(point, Quaternion.identity, 1, Vector3.up, DrawSphereHalfWithSolidBottom);
                points[i].position = point;
                UnityEditor.Handles.color = Color.white;
            }
        }

        public static void PointsFreeMoveHandle(int hashCode, List<Transform> points, float size, UnityEditor.Handles.CapFunction capFunction)
        {
            Event current = Event.current;

            Ray ray = HandleUtility.GUIPointToWorldRay(current.mousePosition);
            RaycastHit hit;

            UnityEditor.Handles.CubeHandleCap(0, current.mousePosition, Quaternion.identity, 1, EventType.Repaint);

            if (current.control && !current.shift)
            {
                if (current.type == EventType.Layout)
                {
                    HandleUtility.AddDefaultControl(GUIUtility.GetControlID(hashCode, FocusType.Passive));
                }

                if (current.button == 0 && current.type == EventType.MouseDown && Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    GameObject go = new GameObject();
                    go.transform.position = hit.point;
                    points.Add(go.transform);
                }
            }

            for (int i = 0, length = points.Count; i < length; i++)
            {
                if (points[i] == null)
                {
                    continue;
                }

                Vector3 point = points[i].position;

                if (current.control && current.shift)
                {
                    if (current.type == EventType.Layout)
                    {
                        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(hashCode, FocusType.Passive));
                    }
                    Vector3 pos = HandleUtility.WorldToGUIPoint(point);
                    if (Vector2.Distance(pos, current.mousePosition) < 20)
                    {
                        Handles.ActiveHandleHighlightedColor = HandleRemoveColor;
                        Handles.ActiveWireHighlightedColor = WireRemoveColor;
                        if (current.button == 0 && current.type == EventType.MouseDown)
                        {
                            GameObject.DestroyImmediate(points[i].gameObject);
                            points.RemoveAt(i);
                            break;
                        }
                    }
                }
                else
                {
                    Handles.ActiveHandleHighlightedColor = HandleHighlightedColor;
                    Handles.ActiveWireHighlightedColor = WireHighlightedColor;
                }

                point = UnityEditor.Handles.FreeMoveHandle(point, Quaternion.identity, size, Vector3.up, capFunction);
                points[i].position = point;
                UnityEditor.Handles.color = Color.white;
            }
        }

        public static void PointsFreeMoveHandle(int hashCode, List<Transform> points)
        {
            Event current = Event.current;

            Ray ray = HandleUtility.GUIPointToWorldRay(current.mousePosition);
            RaycastHit hit;

            UnityEditor.Handles.CubeHandleCap(0, current.mousePosition, Quaternion.identity, 1, EventType.Repaint);

            if (current.control && !current.shift)
            {
                if (current.type == EventType.Layout)
                {
                    HandleUtility.AddDefaultControl(GUIUtility.GetControlID(hashCode, FocusType.Passive));
                }

                if (current.button == 0 && current.type == EventType.MouseDown && Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    GameObject go = new GameObject();
                    go.transform.position = hit.point;
                    points.Add(go.transform);
                }
            }

            for (int i = 0, length = points.Count; i < length; i++)
            {
                if (points[i] == null)
                {
                    continue;
                }

                Vector3 point = points[i].position;

                if (current.control && current.shift)
                {
                    if (current.type == EventType.Layout)
                    {
                        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(hashCode, FocusType.Passive));
                    }
                    Vector3 pos = HandleUtility.WorldToGUIPoint(point);
                    if (Vector2.Distance(pos, current.mousePosition) < 20)
                    {
                        Handles.ActiveHandleHighlightedColor = HandleRemoveColor;
                        Handles.ActiveWireHighlightedColor = WireRemoveColor;
                        if (current.button == 0 && current.type == EventType.MouseDown)
                        {
                            GameObject.DestroyImmediate(points[i].gameObject);
                            points.RemoveAt(i);
                            break;
                        }
                    }
                }
                else
                {
                    Handles.ActiveHandleHighlightedColor = HandleHighlightedColor;
                    Handles.ActiveWireHighlightedColor = WireHighlightedColor;
                }

                point = UnityEditor.Handles.FreeMoveHandle(point, Quaternion.identity, 1, Vector3.up, DrawSphereHalfWithSolidBottom);
                points[i].position = point;
                UnityEditor.Handles.color = Color.white;
            }
        }

        public static void PointsFreeMoveHandle(int hashCode, List<Transform> points, float size, UnityEditor.Handles.CapFunction capFunction, Action<GameObject> onCreate, Action<GameObject> onRemove)
        {
            Event current = Event.current;

            Ray ray = HandleUtility.GUIPointToWorldRay(current.mousePosition);
            RaycastHit hit;

            UnityEditor.Handles.CubeHandleCap(0, current.mousePosition, Quaternion.identity, 1, EventType.Repaint);

            if (current.control && !current.shift)
            {
                if (current.type == EventType.Layout)
                {
                    HandleUtility.AddDefaultControl(GUIUtility.GetControlID(hashCode, FocusType.Passive));
                }

                if (current.button == 0 && current.type == EventType.MouseDown && Physics.Raycast(ray, out hit, Mathf.Infinity))
                {

                    GameObject newPoint = new GameObject();
                    newPoint.transform.position = hit.point;
                    onCreate?.Invoke(newPoint);
                    points.Add(newPoint.transform);
                }
            }

            for (int i = 0, length = points.Count; i < length; i++)
            {
                if (points[i] == null)
                {
                    continue;
                }

                Vector3 point = points[i].position;

                if (current.control && current.shift)
                {
                    if (current.type == EventType.Layout)
                    {
                        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(hashCode, FocusType.Passive));
                    }
                    Vector3 pos = HandleUtility.WorldToGUIPoint(point);
                    if (Vector2.Distance(pos, current.mousePosition) < 20)
                    {
                        Handles.ActiveHandleHighlightedColor = HandleRemoveColor;
                        Handles.ActiveWireHighlightedColor = WireRemoveColor;
                        if (current.button == 0 && current.type == EventType.MouseDown)
                        {
                            Transform removePoint = points[i];
                            onRemove?.Invoke(removePoint.gameObject);
                            GameObject.DestroyImmediate(removePoint.gameObject);
                            points.RemoveAt(i);
                            break;
                        }
                    }
                }
                else
                {
                    Handles.ActiveHandleHighlightedColor = HandleHighlightedColor;
                    Handles.ActiveWireHighlightedColor = WireHighlightedColor;
                }

                point = UnityEditor.Handles.FreeMoveHandle(point, Quaternion.identity, size, Vector3.up, capFunction);
                points[i].position = point;
                UnityEditor.Handles.color = Color.white;
            }
        }

        public static void PointsFreeMoveHandle(int hashCode, List<Transform> points, Action<GameObject> onCreate, Action<GameObject> onRemove)
        {
            Event current = Event.current;

            Ray ray = HandleUtility.GUIPointToWorldRay(current.mousePosition);
            RaycastHit hit;

            UnityEditor.Handles.CubeHandleCap(0, current.mousePosition, Quaternion.identity, 1, EventType.Repaint);

            if (current.control && !current.shift)
            {
                if (current.type == EventType.Layout)
                {
                    HandleUtility.AddDefaultControl(GUIUtility.GetControlID(hashCode, FocusType.Passive));
                }

                if (current.button == 0 && current.type == EventType.MouseDown && Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    GameObject newPoint = new GameObject();
                    newPoint.transform.position = hit.point;
                    onCreate?.Invoke(newPoint);
                    points.Add(newPoint.transform);
                }
            }

            for (int i = 0, length = points.Count; i < length; i++)
            {
                if (points[i] == null)
                {
                    continue;
                }

                Vector3 point = points[i].position;

                if (current.control && current.shift)
                {
                    if (current.type == EventType.Layout)
                    {
                        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(hashCode, FocusType.Passive));
                    }
                    Vector3 pos = HandleUtility.WorldToGUIPoint(point);
                    if (Vector2.Distance(pos, current.mousePosition) < 20)
                    {
                        Handles.ActiveHandleHighlightedColor = HandleRemoveColor;
                        Handles.ActiveWireHighlightedColor = WireRemoveColor;
                        if (current.button == 0 && current.type == EventType.MouseDown)
                        {
                            Transform removePoint = points[i];
                            onRemove?.Invoke(removePoint.gameObject);
                            GameObject.DestroyImmediate(removePoint.gameObject);
                            points.RemoveAt(i);
                            break;
                        }
                    }
                }
                else
                {
                    Handles.ActiveHandleHighlightedColor = HandleHighlightedColor;
                    Handles.ActiveWireHighlightedColor = WireHighlightedColor;
                }

                point = UnityEditor.Handles.FreeMoveHandle(point, Quaternion.identity, 1, Vector3.up, DrawSphereHalfWithSolidBottom);
                points[i].position = point;
                UnityEditor.Handles.color = Color.white;
            }
        }

    }
}