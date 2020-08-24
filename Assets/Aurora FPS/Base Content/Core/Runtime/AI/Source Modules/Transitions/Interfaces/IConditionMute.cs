/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

namespace AuroraFPSRuntime.AI
{
    public interface IConditionMute
    {
        /// <summary>
        /// Transition mute state value.
        /// </summary>
        /// <returns>
        /// True if transition is muted and not checked.
        /// Otherwise false.
        /// </returns>
        bool IsMuted();
    }
}