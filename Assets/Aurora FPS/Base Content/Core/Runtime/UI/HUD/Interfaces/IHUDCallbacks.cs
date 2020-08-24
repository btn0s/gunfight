/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using UnityEngine;
using UnityEngine.UI;

namespace AuroraFPSRuntime.UI
{
    /// <summary>
    /// IHUDCallback interface contains required functions for the HUDManager.  
    /// </summary>
    public interface IHUDCallbacks
    {
        /// <summary>
        /// Processing health point on the HUD components.
        /// </summary>
        /// <param name="health">Player health point</param>
        void UpdateHealthProperties(int health, int maxHealth);

        /// <summary>
        /// Processing weapon information on the HUD components.
        /// </summary>
        /// <param name="name">Weapon name.</param>
        /// <param name="weaponSprite">Weapon sprite.</param>
        void UpdateWeaponProperties(string name, Sprite weaponSprite = null);

        /// <summary>
        /// Processing ammo information on the HUD components.
        /// </summary>
        /// <param name="bulletCount">Weapon bullet count.</param>
        /// <param name="clipCount">Weapon clip count.</param>
        void UpdateAmmoProperties(int bulletCount, int clipCount);
    }
}