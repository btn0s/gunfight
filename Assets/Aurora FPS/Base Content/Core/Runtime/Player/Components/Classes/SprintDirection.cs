/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using System;

namespace AuroraFPSRuntime
{
    /// <summary>
    /// First person controller sprint directions.
    /// </summary>
    [Flags]
    public enum SprintDirection
    {
        /// <summary>
        /// Sprint movement disabled.
        /// </summary>
        Disabled = 0,

        /// <summary>
        ///  Sprint movement available only on forward direction.
        /// </summary>
        Forward = 1 << 0,

        /// <summary>
        /// Sprint movement available only on side directions.
        /// </summary>
        Side = 1 << 1,

        /// <summary>
        /// Sprint movement available only on backword direction.
        /// </summary>
        Backword = 1 << 2,

        /// <summary>
        /// Sprint movement available on any directions.
        /// </summary>
        Free = ~0
    }
}