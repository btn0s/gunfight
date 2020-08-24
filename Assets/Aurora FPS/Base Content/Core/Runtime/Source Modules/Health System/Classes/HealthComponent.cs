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
    public abstract class HealthComponent : MonoBehaviour, IHealth
    {
        #region [IHealth Implementation]
        /// <summary>
        /// Get current health point.
        /// </summary>
        public abstract int GetHealth();

        /// <summary>
        /// Take damage to the health.
        /// </summary>
        /// <param name="amount">Damage amount.</param>
        public abstract void TakeDamage(int amount);

        /// <summary>
        /// Alive state of health object.
        /// </summary>
        /// <returns>
        /// True if health > 0.
        /// Otherwise false.
        /// </returns>
        public abstract bool IsAlive();
        #endregion
    }
}