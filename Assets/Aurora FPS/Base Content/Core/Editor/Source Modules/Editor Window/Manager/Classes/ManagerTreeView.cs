/* ==================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================== */

using AuroraFPSRuntime;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;

namespace AuroraFPSEditor.Window.Manager
{
    public class ManagerTreeView : TreeView
    {
        internal struct ManagerTreeElement
        {
            public int id;
            public string name;
            public string section;
            public int priority;
            public int detpth;

            public ManagerTreeElement(int id, string name, string section, int priority, int detpth)
            {
                this.id = id;
                this.name = name;
                this.section = section;
                this.priority = priority;
                this.detpth = detpth;
            }
        }

        // Base ManagerTreeView properties.
        private TreeViewItem treeViewItemRoot;
        private List<TreeViewItem> treeViewItems;
        private List<ManagerTreeElement> managerTreeElements;

        public ManagerTreeView(TreeViewState state, TreeElementEditor[] treeElementEditors) : base(state)
        {
            useScrollView = true;
            InitializeTreeViewItems(treeElementEditors);
            SetupParentsAndChildrenFromDepths(treeViewItemRoot, treeViewItems);
            Reload();

        }

        protected override TreeViewItem BuildRoot()
        {
            return treeViewItemRoot ?? new TreeViewItem { id = 0, depth = -1, displayName = "Root" };;
        }

        protected virtual void InitializeTreeViewItems(TreeElementEditor[] treeElementEditors)
        {
            treeViewItemRoot = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
            this.treeViewItems = new List<TreeViewItem>();

            if (treeElementEditors != null)
            {
                // Create and fill manager option values array.
                managerTreeElements = new List<ManagerTreeElement>();
                for (int i = 0; i < treeElementEditors.Length; i++)
                {
                    TreeElementEditor treeElementEditor = treeElementEditors[i];
                    TreeElementAttribute attribute = AuroraExtension.GetAttribute<TreeElementAttribute>(treeElementEditor.GetType());
                    if (attribute != null)
                    {
                        managerTreeElements.Add(new ManagerTreeElement(treeElementEditor.GetID(), attribute.GetName(), attribute.GetSection(), attribute.GetPriority(), 0));
                    }
                }

                // Sort all manager option values by priority.
                managerTreeElements = managerTreeElements.OrderBy(t => t.priority).ToList();

                for (int i = 0; i < managerTreeElements.Count; i++)
                {
                    ManagerTreeElement managerTreeElement = managerTreeElements[i];
                    treeViewItems.Add(new TreeViewItem(managerTreeElement.id, managerTreeElement.detpth, managerTreeElement.name));
                }

                for (int i = 0; i < treeViewItems.Count; i++)
                {
                    TreeViewItem reference = treeViewItems[i];
                    for (int j = 0; j < treeViewItems.Count; j++)
                    {
                        TreeViewItem item = treeViewItems[j];
                        if (!TryMakeChildren(reference, item, ref i, ref j))
                        {
                            CheckChilds(reference, item, ref i, ref j);
                        }
                    }
                }

                for (int i = 0; i < treeViewItems.Count; i++)
                {
                    FixDepth(treeViewItems[i]);
                }
            }
        }

        private void CheckChilds(TreeViewItem reference, TreeViewItem item, ref int i, ref int j)
        {
            if (item.children != null)
            {
                for (int k = 0; k < item.children.Count; k++)
                {
                    TreeViewItem child = item.children[k];
                    if (!TryMakeChildren(child, reference, ref i, ref j))
                    {
                        CheckChilds(reference, child, ref i, ref j);
                    }
                }
            }
        }

        private bool TryMakeChildren(TreeViewItem reference, TreeViewItem item, ref int i, ref int j)
        {
            if (reference.displayName != item.displayName)
            {
                ManagerTreeElement parentData = managerTreeElements.Find(t => t.name == reference.displayName);
                ManagerTreeElement childData = managerTreeElements.Find(t => t.name == item.displayName);
                if (parentData.name == childData.section)
                {
                    reference.AddChild(item);
                    treeViewItems.Remove(item);
                    i = 0;
                    j = 0;
                    return true;
                }
            }
            return false;
        }

        private void FixDepth(TreeViewItem treeItem)
        {
            if (treeItem.children != null)
            {
                for (int i = 0; i < treeItem.children.Count; i++)
                {
                    TreeViewItem cItem = treeItem.children[i];
                    cItem.depth = treeItem.depth + 1;
                    FixDepth(cItem);
                }
            }
        }

        public TreeViewItem FindTreeViewItem(int id)
        {
            return FindItem(id, treeViewItemRoot);
        }

        #region [Getter / Setter]
        public TreeViewItem GetTreeViewItemRoot()
        {
            return treeViewItemRoot;
        }

        protected void SetTreeViewItemRoot(TreeViewItem value)
        {
            treeViewItemRoot = value;
        }

        public List<TreeViewItem> GetTreeViewItems()
        {
            return treeViewItems;
        }

        protected void SetTreeViewItems(List<TreeViewItem> value)
        {
            treeViewItems = value;
        }
        #endregion
    }
}