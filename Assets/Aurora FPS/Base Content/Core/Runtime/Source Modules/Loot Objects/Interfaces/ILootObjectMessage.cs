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
    public interface ILootObjectMessage
    {
        /// <summary>
        /// Message to display in player HUD, when player watch on this loot object.
        /// </summary>
        /// <returns>Message of loot object.</returns>
        string GetLootMessage();
    }
}