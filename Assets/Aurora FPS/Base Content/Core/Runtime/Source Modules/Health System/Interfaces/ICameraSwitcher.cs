/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

namespace AuroraFPSRuntime
{
    public interface ICameraSwitcher
    {
        /// <summary>
        /// Called when camera switching.
        /// </summary>
        void OnSwitch();
    }
}