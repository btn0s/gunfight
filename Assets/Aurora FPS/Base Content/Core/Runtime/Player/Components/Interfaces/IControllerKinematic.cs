/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

namespace AuroraFPSRuntime
{
    public interface IControllerKinematic
    {
        /// <summary>
        /// Set controller kinematic state.
        /// </summary>
        void SetKinematic(bool isKinematic);
    }
}