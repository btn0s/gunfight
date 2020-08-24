/* ==================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================== */
   
using UnityEditor;
using UnityEngine;

public sealed class GUISplitView
{
    public enum Direction
    {
        Horizontal,
        Vertical
    }

    // Base AuroraGUISplitView properties.
    private Direction splitDirection;
    private float minPosition;
    private float maxPosition;

    // Stored required properties.
    private bool resize;
    private float splitNormalizedPosition;
    private Vector2 scrollPosition;
    private Rect currentRect;

    /// <summary>
    /// AuroraGUISplitView constructor.
    /// </summary>
    /// <param name="splitDirection">Split view direction.</param>
    public GUISplitView(Direction splitDirection)
    {
        this.splitDirection = splitDirection;
        this.splitNormalizedPosition = 0.25f;
        this.minPosition = 0.125f;
        this.maxPosition = 0.5f;
    }

    /// <summary>
    /// AuroraGUISplitView constructor.
    /// </summary>
    /// <param name="splitDirection">Split view direction.</param>
    /// <param name="minPosition">Min possible split view position, ranged: [0...1].</param>
    /// <param name="maxPosition">Max possible split view position, ranged: [0...1].</param>
    public GUISplitView(Direction splitDirection, float minPosition, float maxPosition)
    {
        this.splitDirection = splitDirection;
        this.splitNormalizedPosition = minPosition;
        this.minPosition = minPosition;
        this.maxPosition = maxPosition;
    }

    public void BeginSplitView()
    {
        Rect beginSplitViewPosition = Rect.zero;

        if (splitDirection == Direction.Horizontal)
        {
            beginSplitViewPosition = EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
        }
        else
        {
            beginSplitViewPosition = EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(true));
        }

        if (beginSplitViewPosition.width > 0.0f)
        {
            currentRect = beginSplitViewPosition;
        }

        if (splitDirection == Direction.Horizontal)
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(currentRect.width * splitNormalizedPosition));
        }
        else
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(currentRect.height * splitNormalizedPosition));
        }
    }

    public void Split()
    {
        GUILayout.EndScrollView();
        ResizeSplitFirstView();
    }

    public void EndSplitView()
    {
        if (splitDirection == Direction.Horizontal)
        {
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            EditorGUILayout.EndVertical();
        }
    }

    private void ResizeSplitFirstView()
    {
        Rect resizeHandleRect = Rect.zero;

        if (splitDirection == Direction.Horizontal)
        {
            resizeHandleRect = new Rect(currentRect.width * splitNormalizedPosition, currentRect.y - 0.5f, 1f, currentRect.height + 0.5f);
        }
        else
        {
            resizeHandleRect = new Rect(currentRect.x, currentRect.height * splitNormalizedPosition, currentRect.width, 1f);
        }

        EditorGUI.DrawRect(resizeHandleRect, Color.gray);

        resizeHandleRect.x -= 5;
        resizeHandleRect.width = 10;
        if (splitDirection == Direction.Horizontal)
        {
            EditorGUIUtility.AddCursorRect(resizeHandleRect, MouseCursor.ResizeHorizontal);
        }
        else
        {
            EditorGUIUtility.AddCursorRect(resizeHandleRect, MouseCursor.ResizeVertical);
        }

        if (Event.current.type == EventType.MouseDown && resizeHandleRect.Contains(Event.current.mousePosition))
        {
            resize = true;
        }

        if (resize)
        {
            if (splitDirection == Direction.Horizontal)
            {
                splitNormalizedPosition = Event.current.mousePosition.x / currentRect.width;
            }
            else
            {
                splitNormalizedPosition = Event.current.mousePosition.y / currentRect.height;
            }

            splitNormalizedPosition = Mathf.Clamp(splitNormalizedPosition, minPosition, maxPosition);
        }

        if (Event.current.type == EventType.MouseUp)
        {
            resize = false;
        }
    }
}