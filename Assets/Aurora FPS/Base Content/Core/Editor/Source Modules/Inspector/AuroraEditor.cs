/* ==================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================== */

using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace AuroraFPSEditor
{
    /// <summary>
    /// Derive from this base class to create a custom inspector editor.
    /// </summary>
    public class AuroraEditor : Editor
    {
        internal static class ContentProperties
        {
            public readonly static GUIContent BaseProperties = new GUIContent("Base Properties");
        }

        private string headerName;
        private static bool foldoutIsActive;
        private string[] excludingProperties;
        private EventGroupEditor eventGroupEditor;

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        protected virtual void OnEnable()
        {
            headerName = GenerateHeaderName(target.GetType().Name);

            SerializedProperty[] events = GetEvents();
            if (events != null && events.Length > 0)
            {
                eventGroupEditor = new EventGroupEditor(events);
            }

            InitializeProperties();

            InitializeCustomExcludingProperties();

            SceneView.beforeSceneGui += BeforeSceneGUI;
            SceneView.duringSceneGui += DuringSceneGUI;

            EditorApplication.update += Update;
            EditorApplication.delayCall += AfterInspectorsGUI;
        }

        /// <summary>
        /// Default inspector GUI.
        /// 
        /// Implement this method to make a custom inspector.
        /// </summary>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            BeginBackground();
            OnHeaderGUI(GetHeaderName());
            BeginBody();
            OnBaseGUI();
            EndBody();
            EndBackground();
            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// 
        /// Implement this method to initialize properties.
        /// </summary>
        public virtual void InitializeProperties()
        {

        }

        /// <summary>
        /// Initialize custom excluding properties added by AddExcludingProperties method.
        /// </summary>
        private void InitializeCustomExcludingProperties()
        {
            List<string> excludingPropertiesList = new List<string>();
            AddExcludingProperties(ref excludingPropertiesList);
            excludingProperties = excludingPropertiesList.ToArray();
        }

        /// <summary>
        /// Enables the Editor to handle an event in the Scene view.
        /// 
        /// Use this event to implement custom handles and user interface.
        /// </summary>
        /// <param name="sceneView">Scene view settings.</param>
        public virtual void DuringSceneGUI(SceneView sceneView)
        {

        }

        /// <summary>
        /// The event issued when the OnGUI method is called.
        /// 
        /// Use this callback to queue meshes for rendering in the Scene view. To implement handles, tools, and UI, use duringSceneGui.
        /// </summary>
        /// <param name="sceneView">Scene view settings.</param>
        public virtual void BeforeSceneGUI(SceneView sceneView)
        {

        }

        /// <summary>
        /// Called every time when editor updated.
        /// </summary>
        public virtual void Update()
        {

        }

        /// <summary>
        /// Called once after all inspectors update.
        /// 
        /// Implement this method to delay task execution until after inspectors have updated.
        /// </summary>
        public virtual void AfterInspectorsGUI()
        {

        }

        /// <summary>
        /// This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        public virtual void OnDestroy()
        {
            OnDisable();
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        public virtual void OnDisable()
        {
            SceneView.beforeSceneGui -= BeforeSceneGUI;
            SceneView.duringSceneGui -= DuringSceneGUI;

            EditorApplication.update -= Update;
            EditorApplication.delayCall -= AfterInspectorsGUI;
        }

        /// <summary>
        /// Base serializedObject properties.
        /// 
        /// Implement this method to draw a custom properties.
        /// </summary>
        public virtual void OnBaseGUI()
        {
            BeginGroup(ContentProperties.BaseProperties);
            DrawPropertiesExcluding(serializedObject, GetExcludingProperties());
            OnEventsGUI();
            EndGroup();
        }

        /// <summary>
        /// Get component header name.
        /// </summary>
        public string GetHeaderName()
        {
            return headerName;
        }

        /// <summary>
        /// Set new header name to this editor.
        /// </summary>
        public void SetHeaderName(string value)
        {
            headerName = value;
        }

        /// <summary>
        /// Header component GUI.
        /// 
        /// Implement this method to make a custom component header GUI.
        /// </summary>
        /// <param name="content">Label on component header.</param>
        public virtual void OnHeaderGUI(string content)
        {
            GUI.color = Color.white;
            GUILayout.BeginVertical(GUI.skin.button);
            GUILayout.Space(2);
            EditorGUILayout.LabelField(content, AEditorStyles.CenteredBoldLabel);
            GUILayout.Space(2);
            GUILayout.EndVertical();
        }

        /// <summary>
        /// Header component GUI.
        /// 
        /// Implement this method to make a custom component header GUI.
        /// </summary>
        /// <param name="content">Label on component header.</param>
        public virtual void OnHeaderGUI(GUIContent content)
        {
            GUI.color = Color.white;
            GUILayout.BeginVertical(GUI.skin.button);
            GUILayout.Space(2);
            EditorGUILayout.LabelField(content, AEditorStyles.CenteredBoldLabel);
            GUILayout.Space(2);
            GUILayout.EndVertical();
        }

        /// <summary>
        /// Begin vertical control GUI background.
        /// </summary>
        public static void BeginBackground()
        {
            GUILayout.Space(7);
            GUI.color = new Color32(128, 128, 128, 255);
            GUILayout.BeginVertical(GUI.skin.window, GUILayout.Height(1));
            GUI.color = Color.white;
        }

        /// <summary>
        /// End vertical control GUI background.
        /// </summary>
        public static void EndBackground()
        {
            GUILayout.EndVertical();
            GUILayout.Space(7);
        }

        /// <summary>
        /// Begin vertical control GUI body.
        /// </summary>
        public static void BeginBody()
        {
            GUI.color = Color.white;
            GUILayout.BeginVertical(GUI.skin.button);
            GUI.color = Color.white;
            GUILayout.Space(7);
        }

        /// <summary>
        /// End vertical control GUI body.
        /// </summary>
        public static void EndBody()
        {
            GUILayout.Space(7);
            GUILayout.EndVertical();
        }

        /// <summary>
        /// Begin vertical control GUI group.
        /// </summary>
        /// <param name="content">Label on group header.</param>
        public static void BeginGroup(GUIContent content)
        {
            GUI.color = new Color32(210, 210, 210, 255);
            GUILayout.BeginVertical(EditorStyles.helpBox);

            Rect headerRect = GetGroupHeaderRect();
            GUI.color = new Color32(210, 210, 210, 255);
            GUI.Label(GetGroupHeaderRect(), GUIContent.none, EditorStyles.helpBox);

            GUI.color = Color.white;
            Rect labelRect = headerRect;
            labelRect.y += 5;
            GUI.Label(labelRect, content, AEditorStyles.GroupHeaderLabel);

            GUILayout.Space(30);
        }

        /// <summary>
        /// Begin vertical control foldout GUI group.
        /// </summary>
        /// <param name="foldout">References of bool value
        /// Add properties inside bool value "if statement".
        /// </param>
        /// <param name="content">Label on group header.</param>
        public static void BeginGroupLevel2(ref bool foldout, GUIContent content)
        {
            GUI.color = new Color32(205, 205, 205, 255);

            GUILayout.BeginVertical(EditorStyles.helpBox);

            GUI.color = new Color32(205, 205, 205, 255);

            GUI.Label(GetGroupHeaderRect(), GUIContent.none, EditorStyles.helpBox);

            GUI.color = Color.white;
            foldout = EditorGUILayout.Foldout(foldout, content, true);
            foldoutIsActive = foldout;

            GUILayout.Space(foldout ? 10 : 1);
        }

        /// <summary>
        /// Begin vertical control foldout GUI group.
        /// </summary>
        /// <param name="foldout">References of bool value
        /// Add properties inside bool value "if statement".
        /// </param>
        /// <param name="content">Label on group header.</param>
        public static void BeginGroupLevel3(ref bool foldout, GUIContent content)
        {
            foldoutIsActive = foldout;

            GUI.color = new Color(0.675f, 0.675f, 0.675f, 1.0f);

            GUILayout.BeginVertical(GUI.skin.GetStyle("HelpBox"));

            Rect headerRect = GetGroupHeaderRect();

            GUI.Label(headerRect, GUIContent.none, GUI.skin.GetStyle("HelpBox"));

            foldout = EditorGUILayout.Foldout(foldout, content, true);

            GUILayout.Space(foldoutIsActive ? 10 : 1);

            GUI.color = Color.white;
        }

        /// <summary>
        /// Begin vertical control foldout GUI group.
        /// </summary>
        /// <param name="foldout">References of bool value
        /// Add properties inside bool value "if statement".
        /// </param>
        /// <param name="content">Label on group header.</param>
        public static void BeginGroupLevel2(ref bool foldout, string content)
        {

            GUI.color = new Color32(205, 205, 205, 255);

            GUILayout.BeginVertical(EditorStyles.helpBox);

            Rect headerRect = GetGroupHeaderRect();

            GUI.color = new Color32(205, 205, 205, 255);

            GUI.Label(GetGroupHeaderRect(), GUIContent.none, EditorStyles.helpBox);

            GUI.color = Color.white;
            foldout = EditorGUILayout.Foldout(foldout, content, true);
            foldoutIsActive = foldout;

            GUILayout.Space(foldout ? 10 : 1);
        }

        /// <summary>
        /// Begin vertical control foldout GUI group.
        /// </summary>
        /// <param name="foldout">References of bool value
        /// Add properties inside bool value "if statement".
        /// </param>
        /// <param name="content">Label on group header.</param>
        public static void BeginGroupLevel3(ref bool foldout, string content)
        {
            foldoutIsActive = foldout;

            GUI.color = new Color(0.675f, 0.675f, 0.675f, 1.0f);

            GUILayout.BeginVertical(GUI.skin.GetStyle("HelpBox"));

            Rect headerRect = GetGroupHeaderRect();

            GUI.Label(headerRect, GUIContent.none, GUI.skin.GetStyle("HelpBox"));

            foldout = EditorGUILayout.Foldout(foldout, content, true);

            GUILayout.Space(foldoutIsActive ? 10 : 1);

            GUI.color = Color.white;
        }

        /// <summary>
        /// Begin vertical control foldout GUI group.
        /// </summary>
        /// <param name="foldout">References of bool value
        /// Add properties inside bool value "if statement".
        /// </param>
        /// <param name="content">Label on group header.</param>
        public static bool BeginGroupLevel2(bool foldout, GUIContent content)
        {
            GUI.color = new Color32(205, 205, 205, 255);

            GUILayout.BeginVertical(EditorStyles.helpBox);

            GUI.color = new Color32(205, 205, 205, 255);

            GUI.Label(GetGroupHeaderRect(), GUIContent.none, EditorStyles.helpBox);

            GUI.color = Color.white;
            foldout = EditorGUILayout.Foldout(foldout, content, true);
            foldoutIsActive = foldout;

            GUILayout.Space(foldout ? 10 : 1);
            return foldout;
        }

        /// <summary>
        /// Begin vertical control foldout GUI group.
        /// </summary>
        /// <param name="foldout">References of bool value
        /// Add properties inside bool value "if statement".
        /// </param>
        /// <param name="content">Label on group header.</param>
        public static bool BeginGroupLevel3(bool foldout, GUIContent content)
        {
            foldoutIsActive = foldout;

            GUI.color = new Color(0.675f, 0.675f, 0.675f, 1.0f);

            GUILayout.BeginVertical(GUI.skin.GetStyle("HelpBox"));

            Rect headerRect = GetGroupHeaderRect();

            GUI.Label(headerRect, GUIContent.none, GUI.skin.GetStyle("HelpBox"));

            foldout = EditorGUILayout.Foldout(foldout, content, true);

            GUILayout.Space(foldoutIsActive ? 10 : 1);

            GUI.color = Color.white;

            return foldout;
        }

        /// <summary>
        /// Begin vertical control GUI group.
        /// </summary>
        /// <param name="content">Label on group header.</param>
        public static void BeginGroup(string content)
        {
            GUI.color = new Color32(210, 210, 210, 255);
            GUILayout.BeginVertical(EditorStyles.helpBox);

            Rect headerRect = GetGroupHeaderRect();
            GUI.color = new Color32(210, 210, 210, 255);
            GUI.Label(GetGroupHeaderRect(), GUIContent.none, EditorStyles.helpBox);

            GUI.color = Color.white;
            Rect labelRect = headerRect;
            labelRect.y += 5;
            GUI.Label(labelRect, content, AEditorStyles.GroupHeaderLabel);

            GUILayout.Space(30);
        }

        /// <summary>
        /// Begin vertical control foldout GUI group.
        /// </summary>
        /// <param name="foldout">References of bool value
        /// Add properties inside bool value "if statement".
        /// </param>
        /// <param name="content">Label on group header.</param>
        public static bool BeginGroupLevel2(bool foldout, string content)
        { 
            GUI.color = new Color32(205, 205, 205, 255);

            GUILayout.BeginVertical(EditorStyles.helpBox);

            GUI.color = new Color32(205, 205, 205, 255);

            GUI.Label(GetGroupHeaderRect(), GUIContent.none, EditorStyles.helpBox);

            GUI.color = Color.white;
            foldout = EditorGUILayout.Foldout(foldout, content, true);
            foldoutIsActive = foldout;

            GUILayout.Space(foldout ? 10 : 1);
            return foldout;
        }

        /// <summary>
        /// Begin vertical control foldout GUI group.
        /// </summary>
        /// <param name="foldout">References of bool value
        /// Add properties inside bool value "if statement".
        /// </param>
        /// <param name="content">Label on group header.</param>
        public static bool BeginGroupLevel3(bool foldout, string content)
        {
            foldoutIsActive = foldout;

            GUI.color = new Color(0.675f, 0.675f, 0.675f, 1.0f);

            GUILayout.BeginVertical(GUI.skin.GetStyle("HelpBox"));

            Rect headerRect = GetGroupHeaderRect();
            headerRect.x -= 1;
            headerRect.width += 2;
            headerRect.y -= 2;
            headerRect.height += 4;

            GUI.Label(headerRect, GUIContent.none, GUI.skin.GetStyle("HelpBox"));

            foldout = EditorGUILayout.Foldout(foldout, content, true);

            GUILayout.Space(foldoutIsActive ? 10 : 1);

            GUI.color = Color.white;

            return foldout;
        }

        /// <summary>
        /// End vertical control GUI group.
        /// </summary>
        public static void EndGroup()
        {
            GUILayout.Space(5);

            GUILayout.EndVertical();
        }

        /// <summary>
        /// End vertical control GUI foldout group.
        /// </summary>
        public static void EndGroupLevel()
        {
            GUILayout.Space(foldoutIsActive ? 5 : 0);

            GUILayout.EndVertical();

            GUILayout.Space(2);
        }

        /// <summary>
        /// Draw header inside group.
        /// </summary>
        /// <param name="content">Header label.</param>
        public static void InsideGroupHeader(GUIContent content)
        {
            EditorGUILayout.LabelField(content, AEditorStyles.CenteredMiniGrayLabel);
            GUILayout.Space(5);
        }

        /// <summary>
        /// Draw header inside group.
        /// </summary>
        /// <param name="content">Header label.</param>
        public static void InsideGroupHeader(string content)
        {
            EditorGUILayout.LabelField(content, AEditorStyles.CenteredMiniGrayLabel);
            GUILayout.Space(5);
        }

        public void OnEventsGUI()
        {
            eventGroupEditor?.DrawLayoutGroup();
        }

        /// <summary>
        /// Add new custom exclusive properties.
        /// 
        /// Implement this method to add new custom exclusive properties that will be hidden in the inspector GUI.
        /// </summary>
        /// <param name="excludingProperties">Properties that will be hidden in the inspector GUI</param>
        protected virtual void AddExcludingProperties(ref List<string> excludingProperties)
        {
            excludingProperties.Add("m_Script");

            SerializedProperty[] events = GetEvents();
            for (int i = 0; i < events.Length; i++)
            {
                excludingProperties.Add(events[i].name);
            }
        }

        /// <summary>
        /// Array of exclusive properties that will be hidden in the inspector GUI.
        /// 
        /// Implement this method to return custom excluding properties default OnBaseGUI.
        /// </summary>
        public string[] GetExcludingProperties()
        {
            return excludingProperties;
        }

        public virtual SerializedProperty[] GetEvents()
        {
            List<SerializedProperty> eventList = new List<SerializedProperty>(3);
            foreach (SerializedProperty property in EditorHelper.GetChildren(serializedObject))
            {
                if (property.FindPropertyRelative("m_PersistentCalls.m_Calls") != null)
                {
                    eventList.Add(property.Copy());
                }
            }
            return eventList.ToArray();
        }

        /// <summary>
        /// Generate header name by value. 
        /// </summary>
        /// <param name="value">Target string value.</param>
        /// <returns>Modified value.</returns>
        public static string GenerateHeaderName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }

            const string APrefix = "A";
            if (value.Contains(APrefix))
            {
                value = value.Remove(value.IndexOf(APrefix), APrefix.Length);
            }

            const string EditorPostfix = "Editor";
            if (value.Contains(EditorPostfix))
            {
                value = value.Remove(value.IndexOf(EditorPostfix), EditorPostfix.Length);
            }

            StringBuilder stBuilder = new StringBuilder(value);
            stBuilder[0] = char.ToUpper(stBuilder[0]);
            for (int i = 1, length = stBuilder.Length; i < length; i++)
            {
                if ((char.IsLower(stBuilder[i]) && ((i + 1) < length && char.IsUpper(stBuilder[i + 1]))) ||
                    char.IsUpper(stBuilder[i]) && (i + 1) < length && char.IsUpper(stBuilder[i + 1]) && (i + 2) < length && char.IsLower(stBuilder[i + 2]))
                {
                    stBuilder.Insert(i + 1, " ");
                }
            }
            return stBuilder.ToString();
        }

        /// <summary>
        /// Increase left side indent level.
        /// </summary>
        /// <param name="level">Indent level value.</param>
        public static void IncreaseIndentLevel(int level = 1)
        {
            EditorGUI.indentLevel += level;
        }

        /// <summary>
        /// Decrease left side indent level.
        /// </summary>
        /// <param name="level">Indent level value.</param>
        public static void DecreaseIndentLevel(int level = 1)
        {
            EditorGUI.indentLevel -= level;
        }

        /// <summary>
        /// Get header rectangle of current group.
        /// </summary>
        /// <returns>Rectangle of group header.</returns>
        public static Rect GetGroupHeaderRect()
        {
            Rect rect = GUILayoutUtility.GetRect(0, 0);
            rect.x -= 4;
            rect.y -= 3;
            rect.width += 8;
#if UNITY_2019_3_OR_NEWER
            rect.height += 27;
#else
            rect.height += 25;
#endif
            return rect;
        }
    }
}