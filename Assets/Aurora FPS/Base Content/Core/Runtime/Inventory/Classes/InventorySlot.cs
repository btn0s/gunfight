/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using System;
using UnityEngine;

namespace AuroraFPSRuntime
{
    [Serializable]
    public struct InventorySlot : IEquatable<InventorySlot>
    {
        [SerializeField] private KeyCode key;
        [SerializeField] private WeaponItem weaponItem;

        /// <summary>
        /// Inventory slot constructor.
        /// </summary>
        /// <param name="key">Slot key.</param>
        /// <param name="weaponItem">Slot weapon.</param>
        public InventorySlot(KeyCode key, WeaponItem weaponItem)
        {
            this.key = key;
            this.weaponItem = weaponItem;
        }

        /// <summary>
        /// Return slot key.
        /// </summary>
        /// <returns></returns>
        public KeyCode GetKey()
        {
            return key;
        }

        /// <summary>
        /// Set slot key.
        /// </summary>
        /// <param name="value"></param>
        public void SetKey(KeyCode value)
        {
            key = value;
        }

        /// <summary>
        /// Return slot weapon.
        /// </summary>
        /// <returns></returns>
        public WeaponItem GetWeaponItem()
        {
            return weaponItem;
        }

        /// <summary>
        /// Set slot weapon.
        /// </summary>
        /// <param name="weapon"></param>
        public void SetWeaponItem(WeaponItem weapon)
        {
            this.weaponItem = weapon;
        }

        /// <summary>
        /// Empty Inventory slot.
        /// </summary>
        /// <returns></returns>
        public readonly static InventorySlot Empty = new InventorySlot(KeyCode.None, null);

        public static bool operator ==(InventorySlot left, InventorySlot right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(InventorySlot left, InventorySlot right)
        {
            return !Equals(left, right);
        }

        public override bool Equals(object obj)
        {
            return (obj is InventorySlot metrics) && Equals(metrics);
        }

        public bool Equals(InventorySlot other)
        {
            return (key, weaponItem) == (other.key, other.weaponItem);
        }

        public override int GetHashCode()
        {
            return (key, weaponItem).GetHashCode();
        }
    }
}