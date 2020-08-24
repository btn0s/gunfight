/* ==================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================== */

using UnityEngine;

namespace AuroraFPSEditor.Window.Manager
{
    [TreeElement("Interactive Object", "Interactive Object", 301)]
    [TreeElementTarget(typeof(AuroraManager))]
    public class AMInteractiveObjectElement : TreeElementEditor
    {
        /// <summary>
        /// Called once when tree element initialized.
        /// </summary>
        /// <param name="id">Unique option id.</param>
        public AMInteractiveObjectElement(int id) : base(id)
        {

        }

        /// <summary>
        /// OnBaseGUI is called for rendering and handling GUI events.
        /// This function can be called multiple times per frame (one call per event).
        /// </summary>
        public override void OnBaseGUI()
        {
            GUILayout.Label("This section shows all the interactive objects that can be created in Aurora FPS.");
        }
    }
}

