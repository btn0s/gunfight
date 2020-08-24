/* ==================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================== */

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AuroraFPSEditor
{
    public sealed class TemplateEditor
    {
        public struct TemplateObject
        {
            private string name;
            private GameObject reference;
            private TagObject[] tagObjects;

            public TemplateObject(GameObject reference, TagObject[] tagObjects)
            {
                this.name = reference.name;
                this.reference = reference;
                this.tagObjects = tagObjects;
            }

            public TemplateObject(string name, GameObject reference, TagObject[] tagObjects)
            {
                this.name = name;
                this.reference = reference;
                this.tagObjects = tagObjects;
            }

            #region [Getter / Setter]
            public string GetName()
            {
                return name;
            }

            public void SetName(string value)
            {
                name = value;
            }

            public GameObject GetReference()
            {
                return reference;
            }

            public void SetReference(GameObject value)
            {
                reference = value;
            }

            public int GetTagObjectsLenght()
            {
                return tagObjects.Length;
            }

            public TagObject GetTagObject(int index)
            {
                return tagObjects[index];
            }

            public TagObject[] GetTagObjects()
            {
                return tagObjects;
            }

            public void SetTagObject(int index, TagObject value)
            {
                tagObjects[index] = value;
            }

            public void SetTagObjects(TagObject[] value)
            {
                tagObjects = value;
            }
            #endregion
        }

        public struct TagObject
        {
            private string defaultName;
            private string labelName;
            private GameObject reference;
            private bool isRequired;

            public TagObject(string defaultName, string labelName, GameObject reference, bool isRequired)
            {
                this.defaultName = defaultName;
                this.labelName = labelName;
                this.reference = reference;
                this.isRequired = isRequired;
            }

            #region [Getter / Setter]
            public string GetDefaultName()
            {
                return defaultName;
            }

            public void SetDefaultName(string value)
            {
                defaultName = value;
            }

            public string GetLabelName()
            {
                return labelName;
            }

            public void SetLabelName(string value)
            {
                labelName = value;
            }

            public GameObject GetReference()
            {
                return reference;
            }

            public void SetReference(GameObject value)
            {
                reference = value;
            }

            public bool IsRequired()
            {
                return isRequired;
            }

            public void IsRequired(bool value)
            {
                isRequired = value;
            }
            #endregion
        }

        // Base TemplateEditor properties.
        private string path;
        private string excludeTemplateSubstring;
        private TemplateObject[] templateObjects;
        private TemplateObject selectedTemplateObject;

        // Template tag properties. 
        private char tagSymbol;
        private string tagRequiredPostfix;

        // Template popup properties.
        private string popupLabel;
        private string[] popupNames;
        private int selectedPopupIndex;
        private int lastSelectedPopupIndex;

        // Tags foldout properties.
        private bool groupTagsInFoldout;
        private GUIContent groupLabel;
        private bool tagsIsExpanded;

        /// <summary>
        /// TemplateEditor constructor.
        /// </summary>
        /// <param name="path">Relative path to templates.</param>
        public TemplateEditor()
        {
            this.path = "";
            this.popupLabel = "Template";
            this.excludeTemplateSubstring = " [Template]";
            this.tagSymbol = '#';
            this.tagRequiredPostfix = "_REQUIRED";
            this.groupTagsInFoldout = false;
            this.groupLabel = new GUIContent("Override");
            this.tagsIsExpanded = true;
        }

        /// <summary>
        /// Initialize template editor and create template objects, names and tags.
        /// </summary>
        /// <param name="path">Relative path to templates.</param>
        public void InitializeTemplateEditor(string path)
        {
            GameObject[] gameObjects = Resources.LoadAll<GameObject>(path);
            if (gameObjects != null && gameObjects.Length > 0)
            {
                popupNames = CreatePopupNames(gameObjects);
                templateObjects = CreateTemplateObjects(gameObjects);

                lastSelectedPopupIndex = -1;
                OnTemplateChangedCallback += (index) => selectedTemplateObject = templateObjects[index];
            }
            else
            {
                popupNames = new string[1] { "None" };
            }
        }

        /// <summary>
        /// Draw template name generic popup selection field.
        /// </summary>
        public void DrawPopup()
        {
            selectedPopupIndex = EditorGUILayout.Popup(popupLabel, selectedPopupIndex, popupNames);
            OnTemplateChangedCallbackHandler();
        }

        /// <summary>
        /// Draw gameobject of tags to edit.
        /// </summary>
        public void DrawTags()
        {
            if (selectedTemplateObject.GetTagObjects() != null && selectedTemplateObject.GetTagObjectsLenght() > 0)
            {
                if (groupTagsInFoldout)
                {
                    AuroraEditor.BeginGroupLevel2(ref tagsIsExpanded, groupLabel);
                    AuroraEditor.IncreaseIndentLevel();
                }

                if (tagsIsExpanded)
                {
                    for (int i = 0; i < selectedTemplateObject.GetTagObjectsLenght(); i++)
                    {
                        TagObject tagObject = selectedTemplateObject.GetTagObject(i);
                        if (tagObject.IsRequired())
                            tagObject.SetReference(AEditorGUILayout.RequiredObjectField(tagObject.GetLabelName(), tagObject.GetReference(), true));
                        else
                            tagObject.SetReference(AEditorGUILayout.ObjectField(tagObject.GetLabelName(), tagObject.GetReference(), true));
                        selectedTemplateObject.SetTagObject(i, tagObject);
                    }
                }

                if (groupTagsInFoldout)
                {
                    AuroraEditor.DecreaseIndentLevel();
                    AuroraEditor.EndGroupLevel();
                }
            }
        }

        public bool HasTemplates()
        {
            return templateObjects != null && templateObjects.Length > 0;
        }

        /// <summary>
        /// Instantiate gameobject at the scene by selected template object.
        /// </summary>
        /// <returns>Instance of instantiated gameobject</returns>
        public GameObject InstantiateSelectedTemplate()
        {
            GameObject templateGameObject = GameObject.Instantiate(selectedTemplateObject.GetReference());

            if (templateGameObject != null)
            {
                List<Transform> temprorayTags = new List<Transform>(selectedTemplateObject.GetTagObjectsLenght());
                Transform[] allChildren = templateGameObject.GetComponentsInChildren<Transform>();
                for (int i = 0; i < allChildren.Length; i++)
                {
                    Transform child = allChildren[i];
                    if (child.name[0] == tagSymbol && child.name[child.name.Length - 1] == tagSymbol)
                    {
                        temprorayTags.Add(child);
                    }
                }
                allChildren = null;

                for (int i = 0; i < selectedTemplateObject.GetTagObjectsLenght(); i++)
                {
                    TagObject tagObject = selectedTemplateObject.GetTagObject(i);
                    if (tagObject.GetReference() != null)
                    {
                        GameObject tagGameObject = GameObject.Instantiate(tagObject.GetReference());
                        for (int j = 0; j < temprorayTags.Count; j++)
                        {
                            Transform temproryTag = temprorayTags[j];
                            if (temproryTag.name == tagObject.GetDefaultName())
                            {
                                tagGameObject.transform.SetParent(temproryTag.parent);
                                tagGameObject.transform.localPosition = temproryTag.localPosition;
                                tagGameObject.transform.localRotation = temproryTag.localRotation;

                                GameObject oldTag = temproryTag.gameObject;
                                temprorayTags.RemoveAt(j);
                                OnTagReplacedCallback?.Invoke(templateGameObject, oldTag, tagGameObject);
                                Object.DestroyImmediate(oldTag);
                            }
                        }
                    }
                }
            }

            return templateGameObject;
        }

        /// <summary>
        /// OnTemplateChangedCallback handler. 
        /// Called every time when template object being changed.
        /// </summary>
        private void OnTemplateChangedCallbackHandler()
        {
            if (selectedPopupIndex != lastSelectedPopupIndex)
            {
                lastSelectedPopupIndex = selectedPopupIndex;
                OnTemplateChangedCallback?.Invoke(selectedPopupIndex);
            }
        }

        /// <summary>
        /// Create template objects by gameobjects array. 
        /// </summary>
        /// <param name="gameObjects">Array of templates.</param>
        /// <returns>Array of TemplateObject type.</returns>
        private TemplateObject[] CreateTemplateObjects(GameObject[] gameObjects)
        {
            if (gameObjects != null && gameObjects.Length > 0)
            {
                TemplateObject[] templateObjects = new TemplateObject[gameObjects.Length];
                for (int i = 0; i < templateObjects.Length; i++)
                {
                    GameObject gameObject = gameObjects[i];
                    string name = gameObject.name.Contains(excludeTemplateSubstring) ? gameObject.name : gameObject.name.Replace(excludeTemplateSubstring, "");
                    templateObjects[i] = new TemplateObject(name, gameObject, CreateTagObjects(gameObject));
                }
                return templateObjects;
            }
            return null;
        }

        /// <summary>
        /// Trying to find and create tags objects by gameobject refernce. 
        /// </summary>
        /// <param name="gameObject">Reference of gameobject</param>
        /// <returns>Array of TagObject type.</returns>
        public TagObject[] CreateTagObjects(GameObject gameObject)
        {
            if (gameObject != null)
            {
                Transform[] transforms = gameObject.GetComponentsInChildren<Transform>();
                List<TagObject> findedTags = new List<TagObject>();
                for (int i = 0; i < transforms.Length; i++)
                {
                    Transform transform = transforms[i];
                    string labelName = transform.name;
                    if (labelName[0] == tagSymbol && labelName[labelName.Length - 1] == tagSymbol)
                    {
                        bool isRequired = labelName.Contains(tagRequiredPostfix);
                        if (isRequired) labelName = labelName.Replace(tagRequiredPostfix, "");
                        labelName = labelName.Substring(1, labelName.Length - 2);
                        labelName = labelName.Substring(0, 1) + labelName.Substring(1).ToLower();
                        findedTags.Add(new TagObject(transform.name, labelName, null, isRequired));
                    }
                }
                return findedTags.ToArray();
            }
            return null;
        }

        /// <summary>
        /// Create templates name excluding specific template substring.
        /// </summary>
        /// <param name="gameObjects">Array of templates.</param>
        /// <returns>Array of template popup names.</returns>
        private string[] CreatePopupNames(GameObject[] gameObjects)
        {
            if (gameObjects != null && gameObjects.Length > 0)
            {
                string[] names = new string[gameObjects.Length];
                for (int i = 0; i < names.Length; i++)
                {
                    string name = gameObjects[i].name;
                    if (name.Contains(excludeTemplateSubstring))
                    {
                        name = name.Replace(excludeTemplateSubstring, "");
                    }
                    names[i] = name;
                }
                return names;
            }
            return null;
        }

        public string GetSelectedTemplateName()
        {
            return popupNames[selectedPopupIndex];
        }

        #region [Event Callback Function]
        /// <summary>
        /// Called every time when template object being changed.
        /// </summary>
        public event System.Action<int> OnTemplateChangedCallback;

        public event System.Action<GameObject, GameObject, GameObject> OnTagReplacedCallback;
        #endregion

        #region [Getter / Setter]
        public string GetPath()
        {
            return path;
        }

        public void SetPath(string value)
        {
            path = value;
        }

        public string GetExcludeTemplateSubstring()
        {
            return excludeTemplateSubstring;
        }

        public void SetExcludeTemplateSubstring(string value)
        {
            excludeTemplateSubstring = value;
        }

        public TemplateObject[] GetTemplateObjects()
        {
            return templateObjects;
        }

        public void SetTemplateObjects(TemplateObject[] value)
        {
            templateObjects = value;
        }

        public TemplateObject GetSelectedTemplateObject()
        {
            return selectedTemplateObject;
        }

        public void SetSelectedTemplateObject(TemplateObject value)
        {
            selectedTemplateObject = value;
        }

        public char GetTagSymbol()
        {
            return tagSymbol;
        }

        public void SetTagSymbol(char value)
        {
            tagSymbol = value;
        }

        public string GetTagRequiredPostfix()
        {
            return tagRequiredPostfix;
        }

        public void SetTagRequiredPostfix(string value)
        {
            tagRequiredPostfix = value;
        }

        public string GetPopupLabel()
        {
            return popupLabel;
        }

        public void SetPopupLabel(string value)
        {
            popupLabel = value;
        }

        public bool GroupTagsInFoldout()
        {
            return groupTagsInFoldout;
        }

        public void GroupTagsInFoldout(bool value)
        {
            groupTagsInFoldout = value;
            tagsIsExpanded = !value;
        }

        public GUIContent GetGroupLabel()
        {
            return groupLabel;
        }

        public void SetGroupLabel(GUIContent value)
        {
            groupLabel = value;
        }
        #endregion
    }
}