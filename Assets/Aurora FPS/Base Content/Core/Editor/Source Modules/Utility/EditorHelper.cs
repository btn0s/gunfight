/* ==================================================================
   ---------------------------------------------------
   Project   :    #0001
   Publisher :    #0002
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================== */

using AuroraFPSRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AuroraFPSEditor
{
    public static class EditorHelper
    {

        public static Transform FindFPControllerTransform()
        {
            GameObject player = GameObject.FindGameObjectWithTag(TNC.Player);
            if (player != null)
                return player.transform;
            return null;
        }

        public static T FindComponent<T>(Transform transform)where T : Component
        {
            if (transform == null)
                return null;

            T t = transform.GetComponentInChildren<T>();
            if (t != null)
                return t;
            return null;
        }

        public static T AddComponent<T>(GameObject gameObject)where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (component == null)
                component = gameObject.AddComponent<T>();
            return component;
        }

        public static Component AddComponent(System.Type type, GameObject gameObject)
        {
            Component component = gameObject.GetComponent(type);
            if (component == null)
                component = gameObject.AddComponent(type);
            return component;
        }

        /// <summary>
        /// Move component with type T to top in inspector.
        /// </summary>
        /// <param name="target"></param>
        /// <typeparam name="T"></typeparam>
        public static void MoveComponentTop<T>(Transform target)where T : Component
        {
            T targetComponent = target.GetComponent<T>();
            Component[] components = target.GetComponents<Component>();
            int length = components.Length;
            int index = 0;
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] == targetComponent)
                {
                    index = i;
                }
            }
            int some = length - index;
            for (int i = 0; i < some; i++)
            {
                UnityEditorInternal.ComponentUtility.MoveComponentUp(targetComponent);
            }
        }

        /// <summary>
        /// Move component with type T to bottom in inspector.
        /// </summary>
        public static void MoveComponentBottom<T>(Transform target)where T : Component
        {
            T targetComponent = target.GetComponent<T>();
            Component[] components = target.GetComponents<Component>();
            int length = components.Length;
            int index = 0;
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] == targetComponent)
                {
                    index = i;
                }
            }
            int some = length - index;
            for (int i = 0; i < some; i++)
            {
                UnityEditorInternal.ComponentUtility.MoveComponentDown(targetComponent);
            }
        }

        /// <summary>
        /// Move component with type T to bottom in inspector.
        /// </summary>
        public static void MoveComponentBottom(System.Type type, Transform target)
        {
            Component[] components = target.GetComponents<Component>();

            int length = components.Length;
            int index = 0;
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i].GetType() == type)
                {
                    index = i;
                }
            }
            int some = length - index;
            for (int i = 0; i < some; i++)
            {
                UnityEditorInternal.ComponentUtility.MoveComponentDown(components[index]);
            }
        }

        public static List<T> FindAssetsByType<T>()where T : Object
        {
            List<T> assets = new List<T>();
            string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null)
                {
                    assets.Add(asset);
                }
            }
            return assets;
        }

        public static List<Object> FindAssetsByType(System.Type type)
        {
            List<Object> assets = new List<Object>();
            string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", type));
            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                Object asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
                if (asset != null)
                {
                    assets.Add(asset);
                }
            }
            return assets;
        }

        /// <summary>
        /// Return all animation clips from transform animator.
        /// If animator not found return null.
        /// </summary>
        public static AnimationClip[] GetAllClips(Animator animator)
        {
            if (animator == null || animator.runtimeAnimatorController == null)
                return null;

            return animator.runtimeAnimatorController.animationClips;
        }

        /// <summary>
        /// Return all animator parameter names.
        /// If animator not found return null.
        /// </summary>
        public static string[] GetAnimatorParameterNames(Animator animator)
        {
            if (animator == null || animator.runtimeAnimatorController == null)
                return null;

            return animator.parameters.Select(n => n.name).ToArray();
        }

        /// <summary>
        /// Return all animator parameters.
        /// If animator not found return null.
        /// </summary>
        public static UnityEngine.AnimatorControllerParameter[] GetAnimatorParameters(Animator animator)
        {
            if (animator == null || animator.runtimeAnimatorController == null)
                return null;

            return animator.parameters;
        }

        /// <summary>
        /// Iterate through all fields in SerializedObject.
        /// </summary>
        public static IEnumerable<SerializedProperty> GetChildren(SerializedObject serializedObject)
        {
            SerializedProperty property = serializedObject.GetIterator();
            if (property.NextVisible(true))
            {
                do
                {
                    yield return property;
                }
                while (property.NextVisible(false));
            }
        }

        /// <summary>
        /// Iterate through all SerializedProperty childrens.
        /// </summary>
        public static IEnumerable<SerializedProperty> GetChildren(SerializedProperty property)
        {
            property = property.Copy();
            var nextElement = property.Copy();
            bool hasNextElement = nextElement.NextVisible(false);
            if (!hasNextElement)
            {
                nextElement = null;
            }

            property.NextVisible(true);
            while (true)
            {
                if ((SerializedProperty.EqualContents(property, nextElement)))
                {
                    yield break;
                }

                yield return property;

                bool hasNext = property.NextVisible(false);
                if (!hasNext)
                {
                    break;
                }
            }
        }

        public static IEnumerable<Type> GetTypesWithHelpAttribute(Type attribute)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetCustomAttributes(attribute, true).Length > 0)
                {
                    yield return type;
                }
            }
        }

        public static IEnumerable<Type> GetTypesWithHelpAttribute(Assembly assembly, Type attribute)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetCustomAttributes(attribute, true).Length > 0)
                {
                    yield return type;
                }
            }
        }

        /// <summary>
        /// Gets the object the property represents.
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static object GetTargetObjectOfProperty(SerializedProperty prop)
        {
            if (prop == null)return null;

            var path = prop.propertyPath.Replace(".Array.data[", "[");
            object obj = prop.serializedObject.targetObject;
            var elements = path.Split('.');
            foreach (var element in elements)
            {
                if (element.Contains("["))
                {
                    var elementName = element.Substring(0, element.IndexOf("["));
                    var index = System.Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                    obj = GetValue_Imp(obj, elementName, index);
                }
                else
                {
                    obj = GetValue_Imp(obj, element);
                }
            }
            return obj;
        }

        public static void SetValueDirect(this SerializedProperty property, object value)
        {
            if (property == null)
                throw new System.NullReferenceException("SerializedProperty is null");

            object obj = property.serializedObject.targetObject;
            string propertyPath = property.propertyPath;
            var flag = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            var paths = propertyPath.Split('.');
            FieldInfo field = null;

            for (int i = 0; i < paths.Length; i++)
            {
                var path = paths[i];
                if (obj == null)
                    throw new System.NullReferenceException("Can't set a value on a null instance");

                var type = obj.GetType();
                if (path == "Array")
                {
                    path = paths[++i];
                    var iter = (obj as System.Collections.IEnumerable);
                    if (iter == null)
                        //Property path thinks this property was an enumerable, but isn't. property path can't be parsed
                        throw new System.ArgumentException("SerializedProperty.PropertyPath [" + propertyPath + "] thinks that [" + paths[i - 2] + "] is Enumerable.");

                    var sind = path.Split('[', ']');
                    int index = -1;

                    if (sind == null || sind.Length < 2)
                        // the array string index is malformed. the property path can't be parsed
                        throw new System.FormatException("PropertyPath [" + propertyPath + "] is malformed");

                    if (!Int32.TryParse(sind[1], out index))
                        //the array string index in the property path couldn't be parsed,
                        throw new System.FormatException("PropertyPath [" + propertyPath + "] is malformed");

                    obj = iter.ElementAtOrDefault(index);
                    continue;
                }

                field = type.GetField(path, flag);
                if (field == null)
                    //field wasn't found
                    throw new System.MissingFieldException("The field [" + path + "] in [" + propertyPath + "] could not be found");

                if (i < paths.Length - 1)
                    obj = field.GetValue(obj);

            }

            var valueType = value.GetType();
            if (!valueType.Is(field.FieldType))
                // can't set value into field, types are incompatible
                throw new System.InvalidCastException("Cannot cast [" + valueType + "] into Field type [" + field.FieldType + "]");

            field.SetValue(obj, value);
        }

        public static System.Object ElementAtOrDefault(this System.Collections.IEnumerable collection, int index)
        {
            var enumerator = collection.GetEnumerator();
            int j = 0;
            for (; enumerator.MoveNext(); j++)
            {
                if (j == index)break;
            }

            System.Object element = (j == index) ?
                enumerator.Current :
                default(System.Object);

            var disposable = enumerator as System.IDisposable;
            if (disposable != null)disposable.Dispose();

            return element;
        }

        public static bool Is(this Type type, Type baseType)
        {
            if (type == null)return false;
            if (baseType == null)return false;

            return baseType.IsAssignableFrom(type);
        }

        public static bool Is<T>(this Type type)
        {
            if (type == null)return false;
            Type baseType = typeof(T);

            return baseType.IsAssignableFrom(type);
        }

        private static object GetValue_Imp(object source, string name)
        {
            if (source == null)
                return null;
            var type = source.GetType();

            while (type != null)
            {
                var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (f != null)
                    return f.GetValue(source);

                var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (p != null)
                    return p.GetValue(source, null);

                type = type.BaseType;
            }
            return null;
        }

        private static object GetValue_Imp(object source, string name, int index)
        {
            var enumerable = GetValue_Imp(source, name)as System.Collections.IEnumerable;
            if (enumerable == null)return null;
            var enm = enumerable.GetEnumerator();
            //while (index-- >= 0)
            //    enm.MoveNext();
            //return enm.Current;

            for (int i = 0; i <= index; i++)
            {
                if (!enm.MoveNext())return null;
            }
            return enm.Current;
        }

        /// <summary>
        /// Get all inputs that contains in project
        /// </summary>
        public static string[] GetAllInputs()
        {
            Object inputManager = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];
            SerializedObject obj = new SerializedObject(inputManager);
            SerializedProperty axesArray = obj.FindProperty("m_Axes");
            List<string> inputs = new List<string>();
            for (int i = 0; i < axesArray.arraySize; ++i)
            {
                SerializedProperty axis = axesArray.GetArrayElementAtIndex(i);
                string name = axis.FindPropertyRelative("m_Name").stringValue;
                inputs.Add(name);
            }
            return inputs.ToArray();
        }

        /// <summary>
        /// Verify project inputs and return missing require Aurora FPS inputs. 
        /// </summary>
        /// <param name="type">[Axes], [Buttons], [All]</param>
        /// <returns>Axes / Buttons / All</returns>
        public static string[] GetMissingInput(string type)
        {
            List<string> missingAxes = new List<string>();
            List<string> incAxes = new List<string>();
            switch (type)
            {
                case "Axes":
                    incAxes.AddRange(INC.Axes);
                    break;
                case "Buttons":
                    incAxes.AddRange(INC.Buttons);
                    break;
                case "All":
                    incAxes.AddRange(INC.Axes);
                    incAxes.AddRange(INC.Buttons);
                    break;
                default:
                    return null;
            }

            Object inputManager = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];
            SerializedObject obj = new SerializedObject(inputManager);
            SerializedProperty axesArray = obj.FindProperty("m_Axes");
            List<string> axesArrayList = new List<string>();
            for (int i = 0; i < axesArray.arraySize; ++i)
            {
                SerializedProperty axis = axesArray.GetArrayElementAtIndex(i);
                string name = axis.FindPropertyRelative("m_Name").stringValue;
                axesArrayList.Add(name);
            }
            for (int i = 0; i < incAxes.Count; i++)
            {
                bool contains = axesArrayList.Any(t => t == incAxes[i]);
                if (contains)
                    continue;
                else
                    missingAxes.Add(incAxes[i]);
            }
            return missingAxes.ToArray();
        }

        /// <summary>
        /// Verify project tags and return missing require Aurora FPS tags. 
        /// </summary>
        /// <returns></returns>
        public static string[] GetMissingTags()
        {
            string[] editorTags = InternalEditorUtility.tags;
            string[] uTags = TNC.AllTags;
            List<string> missingTags = new List<string>();
            for (int i = 0; i < uTags.Length; i++)
            {
                bool contain = false;
                for (int j = 0; j < editorTags.Length; j++)
                {
                    if (uTags[i] == editorTags[j])
                    {
                        contain = true;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                if (!contain)
                {
                    missingTags.Add(uTags[i]);
                }
            }
            return missingTags.ToArray();
        }

        /// <summary>
        /// Verify project layers and return missing require Aurora FPS layers. 
        /// </summary>
        /// <returns></returns>
        public static string[] GetMissingLayers()
        {
            string[] editorLayers = InternalEditorUtility.layers;
            string[] requireLayers = LNC.AllLayers;
            List<string> missingLayers = new List<string>();
            for (int i = 0; i < requireLayers.Length; i++)
            {
                bool contain = editorLayers.Any(t => t == requireLayers[i]);
                if (contain)
                    continue;
                else
                    missingLayers.Add(requireLayers[i]);
            }
            return missingLayers.ToArray();
        }

        /// <summary>
        /// Auto add all require Aurora FPS missing tags in project settings.
        /// </summary>
        public static void AddMissingTags()
        {
            string[] missingTags = GetMissingTags();
            if (missingTags != null && missingTags.Length == 0)
            {
                DisplayDialogs.Message("Tags", "Your project has all the necessary tags!");
                return;
            }
            else if (missingTags == null)
            {
                return;
            }

            for (int i = 0; i < missingTags.Length; i++)
            {
                InternalEditorUtility.AddTag(missingTags[i]);
            }
        }

        public static IEnumerable<string> GetNamespacesInAssembly(Assembly assembly)
        {
            System.Type[] types = assembly.GetTypes();

            return types.Select(t => t.Namespace)
                .Distinct()
                .Where(n => n != null);
        }

        public static bool ContatinsNamespace(string name)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string[] namespaces = GetNamespacesInAssembly(assembly).ToArray();
            for (int i = 0, length = namespaces.Length; i < length; i++)
            {
                if (namespaces[i] == name)
                {
                    return true;
                }
            }
            return false;
        }

        public static string ResizeString(string label, int maxWordCount)
        {
            string[] stateArray = label.Split(' ');
            if (stateArray.Length > maxWordCount)
            {
                System.Array.Resize<string>(ref stateArray, maxWordCount);
                label = string.Join(" ", stateArray);
                label = label.Remove(label.Length - 1, 1) + "...";
            }
            return label;
        }
    }
}