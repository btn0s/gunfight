/* ==================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================== */

namespace AuroraFPSEditor.Window.Manager
{
    public abstract class TreeElementEditor : ITreeElementEditorCallbacks
    {
        // Base ManagerOption properties.
        private int id;

        /// <summary>
        /// Called once when tree element initialized.
        /// </summary>
        /// <param name="id">Unique option id.</param>
        public TreeElementEditor(int id)
        {
            this.id = id;
        }

        #region [IManagerOptionCallbacks Implementation]
        /// <summary>
        /// Called once when Manager is open.
        /// </summary>
        public virtual void OnManagerOpen()
        {

        }

        /// <summary>
        /// Called when the manager becomes enabled and active.
        /// Initializing base ManagerItem properties.
        /// </summary>
        public virtual void OnManagerEnable()
        {

        }

        /// <summary>
        /// Called when the Manager window gets keyboard or mouse focus.
        /// </summary>
        public virtual void OnManagerFocus()
        {

        }

        /// <summary>
        /// Called when this element is selected.
        /// </summary>
        public virtual void OnSelect()
        {

        }

        /// <summary>
        /// OnBaseGUI is called for rendering and handling GUI events.
        /// This function can be called multiple times per frame (one call per event).
        /// </summary>
        public virtual void OnBaseGUI()
        {

        }

        /// <summary>
        /// Called when the current item is changed to another.
        /// </summary>
        public virtual void OnDisable()
        {

        }

        /// <summary>
        /// Called once when Manager window is closed.
        /// </summary>
        public virtual void OnManagerClosed()
        {

        }
        #endregion

        #region [Getter / Setter]
        /// <summary>
        /// Unique option id.
        /// </summary>
        public int GetID()
        {
            return id;
        }
        #endregion
    }
}