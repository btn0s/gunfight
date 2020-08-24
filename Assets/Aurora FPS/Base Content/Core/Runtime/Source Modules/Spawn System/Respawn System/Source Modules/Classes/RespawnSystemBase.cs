/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using UnityEngine;

namespace AuroraFPSRuntime
{
    public abstract class RespawnSystemBase : MonoBehaviour, IRespawnSystem
    {
        #region [IRespawnSystem Implementation]
        /// <summary>
        /// Respawn target object.
        /// </summary>
        public abstract void Respawn();
        #endregion
    }
}