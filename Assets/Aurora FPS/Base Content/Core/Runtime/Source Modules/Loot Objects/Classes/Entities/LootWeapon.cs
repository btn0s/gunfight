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
    public class LootWeapon : LootObject
    {
        [SerializeField] private WeaponItem weaponItem;
        [SerializeField] private bool autoActivate = true;

        private FPInventory inventory;

        #region [LootObject Implementation]
        /// <summary>
        /// Called once when target trying to loot this object.
        /// </summary>
        /// <param name="target">Transform instance that want to loot this object.</param>
        protected override void OnLoot(Transform target)
        {
            if (inventory != null)
            {
                if (inventory.Add(weaponItem) && autoActivate)
                {
                    inventory.ActivateWeapon(weaponItem);
                }
            }
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
                inventory = target.GetComponent<FPInventory>();
            }
        }

        /// <summary>
        /// Called once when player become invisible loot object.
        /// </summary>
        public override void OnBecomeInVisible()
        {
            inventory = null;
        }

        /// <summary>
        /// Called once when player looted object.
        /// </summary>
        public override void AfterLoot()
        {
            inventory = null;
        }
        #endregion

        #region [ILootObjectAvailable Implementation]
        /// <summary>
        /// Object is available to loot.
        /// </summary>
        public override bool AvailableToLoot()
        {
            if (!inventory.AllowIdenticalWeapons())
            {
                return !inventory.ContainsWeapon(weaponItem);
            }
            return true;
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

        public bool GetAutoActivate()
        {
            return autoActivate;
        }

        public void SetAutoActivate(bool value)
        {
            autoActivate = value;
        }
        #endregion
    }
}