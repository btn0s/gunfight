/* ==================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================== */

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace AuroraFPSEditor
{
    /// <summary>
    /// Derive from this base class to create a custom inspector editor.
    /// </summary>
    public class AuroraEditor<TObject> : AuroraEditor where TObject : Object
    {
        protected TObject instance;

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        protected override void OnEnable()
        {
            instance = (TObject)target as TObject;
            base.OnEnable();
        }

        /// <summary>
        /// Default inspector GUI.
        /// 
        /// Implement this function to make a custom inspector.
        /// </summary>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            BeginBackground();
            OnHeaderGUI(GetHeaderName());
            BeginBody();
            if (instance != null)
            {
                EditorGUI.BeginChangeCheck();
                OnBaseGUI();
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(instance, string.Format("Applying changes to ({0}), Time: [{1}]", instance.name, System.DateTime.Now.ToString("HH:mm:ss")));
                }
            }
            EndBody();
            EndBackground();
            serializedObject.ApplyModifiedProperties();

            if (instance != null && GUI.changed && !EditorApplication.isPlaying)
            {
                MarkDirty();
            }
        }

        /// <summary>
        /// Mark this object/scene as dirty and required to save.
        /// </summary>
        public void MarkDirty()
        {
            EditorUtility.SetDirty(instance);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}