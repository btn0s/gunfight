/* ==================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================== */

using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace AuroraFPSEditor.Window.Manager
{
    public abstract class ManagerWindow : EditorWindow
    {
        // Manager tree elements properties.
        protected TreeElementEditor[] treeElementEdtiors;
        protected TreeElementEditor selectedTreeElementEditor;

        // Tree view properties.
        protected TreeViewState treeViewState;
        protected ManagerTreeView managerTree;
        protected SearchField treeSearchField;

        // Stored required properties.
        private Vector2 scrollViewPosition = Vector2.zero;
        private GUISplitView splitView = new GUISplitView(GUISplitView.Direction.Horizontal);
        private int lastSelectedID = -1;
        private bool managerIsOpen;

        /// <summary>
        /// Create new editor manager window instance.
        /// </summary>
        /// <param name="title">Window title text.</param>
        public static void CreateManagerWindow<T>(string title) where T : ManagerWindow
        {
            T window = GetWindow<T>();
            window.titleContent = new GUIContent(title);
            window.minSize = new Vector2(440, 300);
            window.maxSize = new Vector2(800, 600);
            window.Show();
        }

        /// <summary>
        /// Create new editor manager window instance.
        /// </summary>
        /// <param name="title">Window title text.</param>
        /// <param name="image">Window title texture image.</param>
        public static void CreateManagerWindow<T>(string title, Texture image) where T : ManagerWindow
        {
            T window = GetWindow<T>();
            window.titleContent = new GUIContent(title, image);
            window.minSize = new Vector2(440, 300);
            window.maxSize = new Vector2(800, 600);
            window.Show();
        }

        /// <summary>
        /// Create new editor manager window instance.
        /// </summary>
        /// <param name="title">Window title text.</param>
        /// <param name="minSize">Window min possible size.</param>
        /// <param name="maxSize">Window max possible size.</param>
        public static void CreateManagerWindow<T>(string title, Vector2 minSize, Vector2 maxSize) where T : ManagerWindow
        {
            T window = GetWindow<T>();
            window.titleContent = new GUIContent(title);
            window.minSize = minSize;
            window.maxSize = maxSize;
            window.Show();
        }

        /// <summary>
        /// Create new editor manager window instance.
        /// </summary>
        /// <param name="title">Window title text.</param>
        /// <param name="image">Window title texture image.</param>
        /// <param name="minSize">Window min possible size.</param>
        /// <param name="maxSize">Window max possible size.</param>
        public static void CreateManagerWindow<T>(string title, Texture image, Vector2 minSize, Vector2 maxSize) where T : ManagerWindow
        {
            T window = GetWindow<T>();
            window.titleContent = new GUIContent(title, image);
            window.minSize = new Vector2(440, 300);
            window.maxSize = new Vector2(800, 600);
            window.Show();
        }

        /// <summary>
        /// This function is called when the window becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            // Find all target tree elements for this manager.
            System.Type[] treeElementTypes = ManagerUtility.FindTreeElememts<ManagerWindow>(this);

            // Create tree element editors.
            treeElementEdtiors = ManagerUtility.CreateTreeElementEditors(treeElementTypes);

            // Create new tree view state.
            treeViewState = new TreeViewState();

            // Create manager tree and expand it.
            managerTree = new ManagerTreeView(treeViewState, treeElementEdtiors);
            managerTree.ExpandAll();

            // Create tree search field.
            treeSearchField = new SearchField();
            treeSearchField.downOrUpArrowKeyPressed += managerTree.SetFocusAndEnsureSelectedItem;

            // Register callback event functions.
            OnSelectedTreeElementCallback += SwitchTreeElement;

            IntitalizeProperties();

            if (!managerIsOpen)
            {
                OnManagerOpenCallback();
                managerIsOpen = true;
            }

            OnManagerEnableCallback();
        }

        /// <summary>
        /// Moves keyboard focus to another EditorWindow.
        /// </summary>
        protected virtual void OnFocus()
        {
            OnManagerFocusCallback();
        }

        /// <summary>
        /// This function is called when the window becomes enabled and active.
        /// 
        /// Implement this method to initialize required properties.
        /// </summary>
        public virtual void IntitalizeProperties()
        {

        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void Update()
        {
            Repaint();
        }

        /// <summary>
        /// OnGUI is called for rendering and handling GUI events.
        /// This function can be called multiple times per frame (one call per event).
        /// </summary>
        public void OnGUI()
        {
            ManagerWindowGUI();
            OnSelectedTreeElementCallbackHandler();
        }

        /// <summary>
        /// Base manager window GUI.
        /// 
        /// This function can be called multiple times per frame (one call per event).
        /// Implement this method to make a manager window GUI.
        /// </summary>
        public virtual void ManagerWindowGUI()
        {
            DrawTreeSearch();
            splitView.BeginSplitView();
            DrawTree();
            splitView.Split();
            GUILayout.BeginVertical();
            OnTreeElementGUI();
            GUILayout.EndVertical();
            splitView.EndSplitView();
        }

        /// <summary>
        /// Called for rendering and handling tree element GUI.
        /// This function can be called multiple times per frame (one call per event).
        /// </summary>
        public virtual void OnTreeElementGUI()
        {
            selectedTreeElementEditor?.OnBaseGUI();
        }

        /// <summary>
        /// This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        protected virtual void OnDestroy()
        {
            OnManagerClosedCallback();
        }

        /// <summary>
        /// Draw tree view elements.
        /// </summary>
        public virtual void DrawTree()
        {
            Rect treePosition = GUILayoutUtility.GetRect(0, position.height - 35);
            managerTree.OnGUI(treePosition);
        }

        /// <summary>
        /// Draw tree search field.
        /// </summary>
        public virtual void DrawTreeSearch()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.FlexibleSpace();
            managerTree.searchString = treeSearchField.OnToolbarGUI(managerTree.searchString);
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Switch tree element by id and call new tree element GUI.
        /// </summary>
        /// <param name="id">Id of the element that you want to switch to.</param>
        public void SwitchTreeElement(int id)
        {
            if (treeElementEdtiors != null)
            {
                for (int i = 0; i < treeElementEdtiors.Length; i++)
                {
                    TreeElementEditor treeElementEditor = treeElementEdtiors[i];
                    if (treeElementEditor.GetID() == id)
                    {
                        selectedTreeElementEditor?.OnDisable();
                        selectedTreeElementEditor = treeElementEditor;
                        selectedTreeElementEditor?.OnSelect();
                    }
                }
            }

        }

        /// <summary>
        /// OnSelectedTreeElementCallback listener that checks whether a new tree element has been selected.
        /// If a new tree element was selected, it calling a OnSelectedTreeElementCallback event.
        /// 
        /// This function can be called multiple times per frame (one call per event).
        /// </summary>
        protected void OnSelectedTreeElementCallbackHandler()
        {
            if (lastSelectedID != treeViewState.lastClickedID)
            {
                OnSelectedTreeElementCallback?.Invoke(treeViewState.lastClickedID);
                lastSelectedID = treeViewState.lastClickedID;
            }
        }

        #region [Tree Element Editor Callbacks Implementation]
        private void OnManagerOpenCallback()
        {
            if (treeElementEdtiors != null)
            {
                for (int i = 0; i < treeElementEdtiors.Length; i++)
                {
                    TreeElementEditor treeElementEditor = treeElementEdtiors[i];
                    treeElementEditor?.OnManagerOpen();
                }
            }
        }

        private void OnManagerEnableCallback()
        {
            if (treeElementEdtiors != null)
            {
                for (int i = 0; i < treeElementEdtiors.Length; i++)
                {
                    TreeElementEditor treeElementEditor = treeElementEdtiors[i];
                    treeElementEditor?.OnManagerEnable();
                }
            }
        }

        private void OnManagerFocusCallback()
        {
            if (treeElementEdtiors != null)
            {
                for (int i = 0; i < treeElementEdtiors.Length; i++)
                {
                    TreeElementEditor treeElementEditor = treeElementEdtiors[i];
                    treeElementEditor?.OnManagerFocus();
                }
            }
        }

        private void OnManagerClosedCallback()
        {
            if (treeElementEdtiors != null)
            {
                for (int i = 0; i < treeElementEdtiors.Length; i++)
                {
                    TreeElementEditor treeElementEditor = treeElementEdtiors[i];
                    treeElementEditor?.OnManagerClosed();
                }
            }
        }
        #endregion

        #region [Event Callback Functions]
        /// <summary>
        /// Called every time when selected new tree element.
        /// </summary>
        public event System.Action<int> OnSelectedTreeElementCallback;
        #endregion

        #region [Getter / Setter]
        public TreeElementEditor[] GetTreeElementEdtiors()
        {
            return treeElementEdtiors;
        }

        public void SetTreeElementEdtiors(TreeElementEditor[] value)
        {
            treeElementEdtiors = value;
        }

        public TreeElementEditor GetSelectedTreeElementEditor()
        {
            return selectedTreeElementEditor;
        }

        protected void SetSelectedTreeElementEditor(TreeElementEditor value)
        {
            selectedTreeElementEditor = value;
        }

        public TreeViewState GetTreeViewState()
        {
            return treeViewState;
        }

        public void SetTreeViewState(TreeViewState value)
        {
            treeViewState = value;
        }

        public ManagerTreeView GetManagerTree()
        {
            return managerTree;
        }

        public void SetManagerTree(ManagerTreeView value)
        {
            managerTree = value;
        }

        public SearchField GetTreeSearchField()
        {
            return treeSearchField;
        }

        public void SetTreeSearchField(SearchField value)
        {
            treeSearchField = value;
        }

        public Vector2 GetScrollViewPosition()
        {
            return scrollViewPosition;
        }

        public void SetScrollViewPosition(Vector2 value)
        {
            scrollViewPosition = value;
        }

        public GUISplitView GetSplitView()
        {
            return splitView;
        }

        public void SetSplitView(GUISplitView value)
        {
            splitView = value;
        }

        public int GetLastSelectedID()
        {
            return lastSelectedID;
        }

        protected void SetLastSelectedID(int value)
        {
            lastSelectedID = value;
        }
        #endregion
    }
}