/* ==================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================== */

using System;
using UnityEditor;
using UnityEngine;

namespace AuroraFPSEditor
{
    public static class ScriptableObjectHelper
    {
        public static T CreateAsset<T>() where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();
            string path = "Assets/";
            string name = typeof(T).Name;
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + name + ".asset");
            AssetDatabase.CreateAsset(asset, assetPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
            return asset;
        }

        public static T CreateAsset<T>(string path) where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();
            if (path == "")
                path = "Assets/";
            string name = typeof(T).Name;
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + name + ".asset");
            AssetDatabase.CreateAsset(asset, assetPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
            return asset;
        }

        public static T CreateAsset<T>(string path, string name) where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();
            if (string.IsNullOrEmpty(path))
                path = "Assets/";
            if (string.IsNullOrEmpty(name))
                name = typeof(T).Name;
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + name + ".asset");
            AssetDatabase.CreateAsset(asset, assetPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
            return asset;
        }

        ///////////////////////////////////////

        public static ScriptableObject CreateAsset(Type type)
        {
            ScriptableObject asset = ScriptableObject.CreateInstance(type);
            string path = "Assets/";
            string name = type.Name;
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + name + ".asset");
            AssetDatabase.CreateAsset(asset, assetPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
            return asset;
        }

        public static ScriptableObject CreateAsset(Type type, string path)
        {
            ScriptableObject asset = ScriptableObject.CreateInstance(type);
            if (path == "")
                path = "Assets/";
            string name = type.Name;
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + name + ".asset");
            AssetDatabase.CreateAsset(asset, assetPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
            return asset;
        }

        public static ScriptableObject CreateAsset(Type type, string path, string name)
        {
            ScriptableObject asset = ScriptableObject.CreateInstance(type);
            if (string.IsNullOrEmpty(path))
                path = "Assets/";
            if (string.IsNullOrEmpty(name))
                name = type.Name;
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + name + ".asset");
            AssetDatabase.CreateAsset(asset, assetPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
            return asset;
        }
        
        /////////////////////////////////////////////

        public static ScriptableObject CreateAsset(ScriptableObject _object)
        {
            if(_object == null)
            {
                return null;
            }

            string path = "Assets/";
            string name = _object.name;
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + name + ".asset");
            AssetDatabase.CreateAsset(_object, assetPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = _object;
            return _object;
        }

        public static ScriptableObject CreateAsset(ScriptableObject _object, string path)
        {
            if(_object == null)
            {
                return null;
            }

            if (path == "")
                path = "Assets/";
            string name = _object.name;
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + name + ".asset");
            AssetDatabase.CreateAsset(_object, assetPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = _object;
            return _object;
        }

        public static ScriptableObject CreateAsset(ScriptableObject _object, string path, string name)
        {
            if(_object == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(path))
                path = "Assets/";
            if (string.IsNullOrEmpty(name))
                name = _object.name;
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + name + ".asset");
            AssetDatabase.CreateAsset(_object, assetPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = _object;
            return _object;
        }
    }
}