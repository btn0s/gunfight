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
    public interface ITreeElementEditorCallbacks
    {
        /// <summary>
        /// Called once when Manager is open.
        /// </summary>
        void OnManagerOpen();

        /// <summary>
        /// Called when the manager becomes enabled and active.
        /// </summary>
        void OnManagerEnable();

        /// <summary>
        /// Called when the Manager window gets keyboard or mouse focus.
        /// </summary>
        void OnManagerFocus();

        /// <summary>
        /// Called when this element is selected.
        /// </summary>
        void OnSelect();

        /// <summary>
        /// OnMainGUI is called for rendering and handling GUI events.
        /// This function can be called multiple times per frame (one call per event).
        /// </summary>
        void OnBaseGUI();

        /// <summary>
        /// Called when the current item is changed to another.
        /// </summary>
        void OnDisable();

        /// <summary>
        /// Called once when Manager window is closed.
        /// </summary>
        void OnManagerClosed();
    }
}