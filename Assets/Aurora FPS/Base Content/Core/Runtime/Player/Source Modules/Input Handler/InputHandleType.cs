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
    /// <summary>
    /// Input handle type.
    /// </summary>
    public enum InputHandleType
    {
        /// <summary>
        /// Once press button for update action and one more time for reset action.
        /// </summary>
        Trigger,

        /// <summary>
        /// Hold button for update action.
        /// </summary>
        Hold
    }
}