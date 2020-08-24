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
    public interface ILootObjectAvailable
    {
        /// <summary>
        /// This object are available for loot
        /// </summary>
        bool AvailableToLoot();
    }
}