/* ==================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

namespace AuroraFPSRuntime
{
    public interface IFreeCamera
    {
        /// <summary>
        /// Start processing camera.
        /// </summary>
        void StartProcessing();

        /// <summary>
        /// Stop processing camera.
        /// </summary>
        void StopProcessing();
    }
}