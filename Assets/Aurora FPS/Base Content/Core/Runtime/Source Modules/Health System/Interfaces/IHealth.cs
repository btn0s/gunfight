/* ==================================================================
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
    public interface IHealth
    {
        /// <summary>
        /// Get current health value.
        /// </summary>
        int GetHealth();

        /// <summary>
        /// Take damage to the health.
        /// </summary>
        /// <param name="amount">Damage amount.</param>
        void TakeDamage(int amount);

        /// <summary>
        /// Alive state of health object.
        /// </summary>
        /// <returns>
        /// True if health > health limit value.
        /// Otherwise false.
        /// </returns>
        bool IsAlive();
    }
}