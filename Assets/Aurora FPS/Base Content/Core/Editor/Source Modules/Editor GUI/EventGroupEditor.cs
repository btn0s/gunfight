/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using UnityEditor;
using UnityEngine;

namespace AuroraFPSEditor
{
    public sealed class EventGroupEditor
    {
        // Base aurora event group editor properties.
        private SerializedProperty[] events;
        private GUIContent content;

        // Stored required properties.
        private bool[] addedEventIndexes;
        private bool foldout;

        public EventGroupEditor(params SerializedProperty[] events)
        {
            this.events = events;
            this.content = new GUIContent("Events");
            this.foldout = false;
            this.addedEventIndexes = GetAddedEventIndexes(events);
        }

        public EventGroupEditor(string content, params SerializedProperty[] events)
        {
            this.events = events;
            this.content = new GUIContent(content);
            this.foldout = false;
            this.addedEventIndexes = GetAddedEventIndexes(events);
        }

        public EventGroupEditor(GUIContent content, params SerializedProperty[] events)
        {
            this.events = events;
            this.content = content;
            this.foldout = false;
            this.addedEventIndexes = GetAddedEventIndexes(events);
        }

        public void DrawLayoutGroup()
        {
            AuroraEditor.IncreaseIndentLevel();
            AuroraEditor.BeginGroupLevel2(ref foldout, content);
            if (foldout)
            {
                for (int i = 0; i < events.Length; i++)
                {
                    SerializedProperty eventProperty = events[i];
                    if (addedEventIndexes[i])
                    {
                        EditorGUILayout.PropertyField(eventProperty);

                        Rect removeButtonPosition = GUILayoutUtility.GetRect(0, 0);
                        removeButtonPosition.x += EditorGUIUtility.currentViewWidth - 140;
                        removeButtonPosition.y -= EditorGUI.GetPropertyHeight(eventProperty);
                        removeButtonPosition.height = 16;
                        removeButtonPosition.width = 65;
                        if (GUI.Button(removeButtonPosition, "Remove") && DisplayDialogs.Confirmation("Are you really want to remove event?\nThis action remove all listeners in this event."))
                        {
                            addedEventIndexes[i] = false;
                            eventProperty.FindPropertyRelative("m_PersistentCalls.m_Calls").ClearArray();
                            GUIUtility.ExitGUI();
                        }
                    }
                }


                Rect addButtonPosition = Rect.zero;
                if (AnyEventAdded())
                {
                    addButtonPosition = GUILayoutUtility.GetRect(0, 0);
                    addButtonPosition.y -= 20;
                    addButtonPosition.height = 16;
                    addButtonPosition.width = EditorGUIUtility.currentViewWidth - 142;
                }
                else 
                {
                    addButtonPosition = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight);
                    addButtonPosition.width = 150;
                    addButtonPosition.x = (EditorGUIUtility.currentViewWidth / 2) - (addButtonPosition.width / 2);
                }

                EditorGUI.BeginDisabledGroup(!HasEventToAdd());
                if (GUI.Button(addButtonPosition, "Add New Event"))
                {
                    GetEventMenu().ShowAsContext();
                }
                EditorGUI.EndDisabledGroup();
            }
            AuroraEditor.EndGroupLevel();
            AuroraEditor.DecreaseIndentLevel();
        }

        public GenericMenu GetEventMenu()
        {
            GenericMenu eventMenu = new GenericMenu();
            for (int i = 0; i < events.Length; i++)
            {
                SerializedProperty eventProperty = events[i];
                if (!addedEventIndexes[i])
                {
                    int indexCopy = i;
                    eventMenu.AddItem(new GUIContent(AuroraEditor.GenerateHeaderName(eventProperty.name)), false, () => { addedEventIndexes[indexCopy] = true; });
                }
            }
            return eventMenu;
        }

        public bool HasEventToAdd()
        {
            for (int i = 0; i < addedEventIndexes.Length; i++)
            {
                if (!addedEventIndexes[i])
                {
                    return true;
                }
            }
            return false;
        }

        public bool AnyEventAdded()
        {
            for (int i = 0; i < addedEventIndexes.Length; i++)
            {
                if (addedEventIndexes[i])
                {
                    return true;
                }
            }
            return false;
        }

        public bool[] GetAddedEventIndexes(SerializedProperty[] events)
        {
            bool[] availableEvents = new bool[events.Length];
            for (int i = 0; i < events.Length; i++)
            {
                SerializedProperty eventProperty = events[i];
                int listenerCount = eventProperty.FindPropertyRelative("m_PersistentCalls.m_Calls").arraySize;
                if (listenerCount > 0)
                {
                    availableEvents[i] = true;
                }
            }
            return availableEvents;
        }
    }
}
