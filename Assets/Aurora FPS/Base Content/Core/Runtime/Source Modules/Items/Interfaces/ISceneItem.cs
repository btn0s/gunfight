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
    public interface ISceneItem
    {
        /// <summary>
        /// Display name of the item.
        /// </summary>
        /// <returns>Item display name.</returns>
        string GetDisplayName();

        /// <summary>
        /// Description of the item.
        /// </summary>
        /// <returns>Item description text.</returns>
        string GetDescription();
    }
}