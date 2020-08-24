/* ==================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================== */

using System;
using System.Collections.Generic;
using System.Linq;
using AuroraFPSRuntime;

namespace AuroraFPSEditor.Window.Manager
{
    public static class ManagerUtility
    {
        /// <summary>
        /// Find all target tree elements for specific manager.
        /// </summary>
        /// <returns>Array of tree element types.</returns>
        public static Type[] FindTreeElememts<T>(T managerType) where T : ManagerWindow
        {
            Type[] treeElements = AuroraExtension.FindSubclassesOf<TreeElementEditor>().ToArray();
            if (treeElements != null && treeElements.Length > 0)
            {
                List<Type> targetTreeElements = new List<Type>();
                for (int i = 0; i < treeElements.Length; i++)
                {
                    Type treeElement = treeElements[i];
                    TreeElementTargetAttribute attribute = AuroraExtension.GetAttribute<TreeElementTargetAttribute>(treeElement);
                    if (attribute != null && attribute.GetTarget() == managerType.GetType())
                    {
                        targetTreeElements.Add(treeElement);
                    }
                }
                return targetTreeElements.ToArray();
            }
            return null;
        }

        /// <summary>
        /// Create TreeElementEditor types. 
        /// </summary>
        /// <param name="treeElements">Array of tree element types.</param>
        /// <returns>Created TreeElementEditor instances.</returns>
        public static TreeElementEditor[] CreateTreeElementEditors(Type[] treeElements)
        {
            if (treeElements != null && treeElements.Length > 0)
            {
                TreeElementEditor[] createdTreeElements = new TreeElementEditor[treeElements.Length];
                for (int i = 0; i < treeElements.Length; i++)
                {
                    createdTreeElements[i] = Activator.CreateInstance(treeElements[i], i + 1) as TreeElementEditor;
                }
                return createdTreeElements;
            }
            return null;
        }
    }
}