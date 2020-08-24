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
    public interface IBaseItem
    {
        /// <summary>
        /// Unique ID of the item.
        /// </summary>
        /// <returns>Item ID.</returns>
        string GetID();
    }
}