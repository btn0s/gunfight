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
    public class LootAmmo : LootObject
    {
        // Base loot ammo properties.
        [SerializeField] private WeaponItem weaponItem;
        [SerializeField] private int ammoCount;

        // Stored required properties.
        private WeaponAmmoSystem weaponAmmoSystem;

        #region [LootObject Implementation]
        /// <summary>
        /// Called once when target trying to loot this object.
        /// </summary>
        /// <param name="target">Transform instance that want to loot this object.</param>
        protected override void OnLoot(Transform target)
        {
            weaponAmmoSystem?.AddAmmo(ammoCount);
        }
        #endregion

        #region [ILootObjectCallbacks Implementation]
        /// <summary>
        /// Called once when player become visible loot object.
        /// </summary>
        /// <param name="target">Transform instance that want to loot this object.</param>
        public override void OnBecomeVisible(Transform target)
        {
            if (weaponItem != null)
            {
                FPInventory inventory = target.GetComponent<FPInventory>();
                if (inventory != null)
                {
                    Transform weapon = inventory.GetWeaponTransform(weaponItem);
                    if (weapon != null)
                    {
                        weaponAmmoSystem = weapon.GetComponent<WeaponAmmoSystem>();
                    }
                }
            }
        }

        /// <summary>
        /// Called once when player become invisible loot object.
        /// </summary>
        public override void OnBecomeInVisible()
        {
            weaponAmmoSystem = null;
        }

        /// <summary>
        /// Called once when player looted object.
        /// </summary>
        public override void AfterLoot()
        {
            weaponAmmoSystem = null;
        }
        #endregion

        #region [ILootObjectAvailable Implementation]
        /// <summary>
        /// Object is available to loot.
        /// </summary>
        public override bool AvailableToLoot()
        {
            return !weaponAmmoSystem.IsFull();
        }
        #endregion

        #region [Getter / Setter]
        public WeaponItem GetWeaponItem()
        {
            return weaponItem;
        }

        public void SetWeaponItem(WeaponItem value)
        {
            weaponItem = value;
        }

        public int GetAmmoCount()
        {
            return ammoCount;
        }

        public void SetAmmoCount(int value)
        {
            ammoCount = value;
        }
        #endregion
    }
}