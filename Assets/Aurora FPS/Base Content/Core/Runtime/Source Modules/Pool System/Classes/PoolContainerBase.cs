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
    [System.Serializable]
    public abstract class PoolContainerBase : IPoolContainer
    {
        public enum Allocator
        {
            Free,
            Fixed,
            Dynamic
        }

        /// <summary>
        /// Length of available object in pool.
        /// </summary>
        public abstract int GetLength();
    }
}