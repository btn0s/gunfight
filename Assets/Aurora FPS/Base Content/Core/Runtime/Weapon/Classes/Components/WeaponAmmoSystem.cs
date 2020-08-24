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
    public class WeaponAmmoSystem : MonoBehaviour, IWeaponAmmoSystem
    {
        [SerializeField] private int ammoCount;
        [SerializeField] private int maxAmmoCount;
        
        /// <summary>
        /// Add ammo count.
        /// </summary>
        /// <param name="value">Ammo count to add in the system.</param>
        public virtual void AddAmmo(int value)
        {
            int sum = ammoCount + value;
            ammoCount = sum < maxAmmoCount ? sum : maxAmmoCount;
        }

        public virtual void AmmoSubtraction()
        {
            ammoCount--;
        }

        public virtual bool HasAmmo()
        {
            return ammoCount > 0;
        }

        public virtual bool IsFull()
        {
            return ammoCount == maxAmmoCount;
        }

        #region [Getter / Setter]
        public int GetAmmoCount()
        {
            return ammoCount;
        }

        protected virtual void SetAmmoCount(int value)
        {
            ammoCount = value;
        }

        public int GetMaxAmmoCount()
        {
            return maxAmmoCount;
        }

        public void SetMaxAmmoCount(int value)
        {
            maxAmmoCount = value;
        }
        #endregion
    }
}