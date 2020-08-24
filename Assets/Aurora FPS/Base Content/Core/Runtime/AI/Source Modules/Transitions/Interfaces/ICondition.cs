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
    public interface ICondition
    {
        /// <summary>
        /// Condition for translate to the next AI behaviour.
        /// </summary>
        bool IsExecuted();
    }
}