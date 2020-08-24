/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using System.IO;
using UnityEditor;
using UnityEngine;

namespace AuroraFPSEditor
{
    internal static class ProjectWindowMenuItems
    {
        [MenuItem("Assets/Create/Aurora FPS Editor/C# Aurora Editor By Class", false, 81)]
        private static void CreateEditorByClass()
        {
            Object selectedObject = Selection.activeObject;

            if (selectedObject is MonoScript)
            {
                MonoScript script = (MonoScript) Selection.activeObject as MonoScript;
                TextAsset template = Resources.Load<TextAsset>("Editor/Templates/Class/AuroraEditorTemplate[C#]");
                if (template != null)
                {
                    string className = script.GetClass().ToString();
                    string path = string.Format("{0}/{1}Editor.cs", "Assets/Aurora FPS/Base Content/Core/Editor/Inspector", className);
                    string templateData = template.text;

                    templateData = template.text.Replace("#CLASS_NAME#", className);
                    templateData = templateData.Replace("#HEADER_NAME#", AuroraEditor.GenerateHeaderName(className));

                    File.WriteAllText(path, templateData);
                    AssetDatabase.Refresh();

                    Object editorScript = AssetDatabase.LoadAssetAtPath(path, typeof(MonoScript));
                    if (editorScript != null)
                    {
                        EditorGUIUtility.PingObject(editorScript);
                    }
                }
            }
            else
            {
                Debug.Log(string.Format("Required file with (.cs) extension! Selected asset is [Name: {0}, Type: {1}]", selectedObject.name, selectedObject.GetType().Name));
            }
        }
    }
}