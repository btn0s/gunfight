/* ================================================================
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
    public interface IInventory
    {
        /// <summary>
        /// Add new weapon in available slot in inventory.
        /// </summary>
        bool Add(WeaponItem weapon);

        /// <summary>
        /// Remove weapon from inventory.
        /// </summary>
        bool Remove(WeaponItem weapon);

        /// <summary>
        /// Replace current weapon on new.
        /// </summary>
        bool Replace(WeaponItem weapon);

        /// <summary>
        /// Replace inventory weapon on some new weapon.
        /// </summary>
        bool Replace(WeaponItem inventoryWeapon, WeaponItem someWeapon);

        /// <summary>
        /// Add new group in inventory.
        /// </summary>
        bool AddGroup(string groupName, params InventorySlot[] slots);

        /// <summary>
        /// Remove group from inventory including weapons contained in this group.
        /// </summary>
        bool RemoveGroup(string groupName);

        /// <summary>
        /// Add new slot in group.
        /// </summary>
        bool AddSlot(string groupName, InventorySlot slot);

        /// <summary>
        /// Remove slot from group including weapon contained in this slot.
        /// </summary>
        bool RemoveSlotFromGroup(string groupName, KeyCode key);

        /// <summary>
        /// Activate weapon by key.
        /// </summary>
        bool ActivateWeapon(KeyCode key);

        /// <summary>
        /// Activate weapon by WeaponItem.
        /// </summary>
        bool ActivateWeapon(WeaponItem weapon);

        /// <summary>
        /// Get weapon transform by WeaponItem.
        /// </summary>
        Transform GetWeaponTransform(WeaponItem weapon);

        /// <summary>
        /// Hide active weapon.
        /// </summary>
        void HideWeapon();

        /// <summary>
        /// Drop active weapon.
        /// </summary>
        void Drop();

        /// <summary>
        /// Get active weapon transform.
        /// </summary>
        Transform GetActiveWeaponTransform();

        /// <summary>
        /// Get active WeaponItem.
        /// </summary>
        /// <returns></returns>
        WeaponItem GetActiveWeaponItem();

        /// <summary>
        /// Weapon count.
        /// </summary>
        /// <returns></returns>
        int WeaponCount();

        /// <summary>
        /// Slot count, the maximum possible wearable number of weapons.
        /// </summary>
        int SlotCount(string groupName);

        /// <summary>
        /// Group count.
        /// </summary>
        int GroupCount(string groupName);

        /// <summary>
        /// Check what specific group is full.
        /// </summary>
        bool IsFull(string group);

        /// <summary>
        /// Has any current active weapon.
        /// </summary>
        bool HasActiveWeapon();
    }
}