/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using UnityEngine;

namespace AuroraFPSRuntime
{
    /// <summary>
    /// Represents base class for all scriptable mapping.
    /// </summary>
    public abstract class ScriptableMapping : ScriptableObject, IScriptableMapping
    {
        /// <summary>
        /// Get mapping length.
        /// </summary>
        public abstract int GetMappingLength();
    }
}