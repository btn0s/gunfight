/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using UnityEngine;

namespace AuroraFPSRuntime.UI
{
    [System.Serializable]
    [CrosshairPresetData("Shotgun", false)]
    public class ShotgunCrosshairPreset : CrosshairPreset
    {
        [SerializeField] private CrosshairElement element = new CrosshairElement(null, 0, 0);

        protected override void OnElementsGUI(float spread)
        {
            DrawElement(element, new Rect((Screen.width - (element.GetWidth() + spread)) / 2, (Screen.height - (element.GetHeight() + spread)) / 2, element.GetWidth() + spread, element.GetHeight() + spread));
        }
    }
}