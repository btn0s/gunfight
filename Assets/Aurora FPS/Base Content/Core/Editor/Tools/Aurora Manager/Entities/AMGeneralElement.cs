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
    [TreeElement("General", "General", 0)]
    [TreeElementTarget(typeof(AuroraManager))]
    internal sealed class AMGeneralElement : TreeElementEditor
    {
        /// <summary>
        /// Called once when tree element initialized.
        /// </summary>
        /// <param name="id">Unique option id.</param>
        public AMGeneralElement(int id) : base(id)
        {
            
        }

        /// <summary>
        /// OnBaseGUI is called for rendering and handling GUI events.
        /// This function can be called multiple times per frame (one call per event).
        /// </summary>
        public override void OnBaseGUI()
        {
            GUILayout.Label("General settings of Aurora FPS.");
        }
    }
}