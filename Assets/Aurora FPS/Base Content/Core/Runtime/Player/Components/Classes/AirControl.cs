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
        /// First person controller air control types.
        /// </summary>
        [Flags]
        public enum AirControl
        {
            /// <summary>
            /// Controller air control is disabled.
            /// </summary>
            Disabled = 0,

            /// <summary>
            /// Controller air control handled only by input.
            /// </summary>
            Input = 1 << 0,

            /// <summary>
            /// Controller air control handled only by camera direction.
            /// </summary>
            Camera = 1 << 1,

            /// <summary>
            /// Controller air control handled by input and camera direction.
            /// </summary>
            Both = ~0
        }
}