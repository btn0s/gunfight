/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using AuroraFPSRuntime;
using UnityEditor;

namespace AuroraFPSEditor
{
    [CustomEditor(typeof(CameraShake))]
    public class CameraShakeEditor : AuroraEditor<CameraShake>
    {
        /// <summary>
        /// Base serializedObject properties.
        /// 
        /// Implement this method to draw a custom properties.
        /// </summary>
        public override void OnBaseGUI()
        {
            HelpBoxMessages.Message(string.Format("{0} component instance.", GetHeaderName()));
        }
    }
}