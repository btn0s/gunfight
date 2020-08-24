/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using UnityEngine;

namespace AuroraFPSRuntime.Serialization.Collections
{
    /// <summary>
    /// Storage wrapper of generic type.
    /// </summary>
    /// <typeparam name="T">Type of storage.</typeparam>
    public class Storage<T> : StorageBase
    {
        // Storage data.
        [SerializeField] protected T data;

        /// <summary>
        /// Get storage data.
        /// </summary>
        /// <returns>Storage data type of (T)</returns>
        public T GetData()
        {
            return data;
        }

        /// <summary>
        /// Set storage data.
        /// </summary>
        /// <param name="value">Storage data type of (T)</param>
        public void SetData(T value)
        {
            data = value;
        }
    }
}