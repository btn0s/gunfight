/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AuroraFPSRuntime
{
    [Serializable]
    public struct InventoryGroup : IEquatable<InventoryGroup>
    {
        [SerializeField] private string name;
        [SerializeField] private List<InventorySlot> inventorySlots;

        /// <summary>
        /// Inventory group constructor.
        /// </summary>
        /// <param name="name">Group name.</param>
        /// <param name="inventorySlots">Group slots.</param>
        public InventoryGroup(string name, List<InventorySlot> inventorySlots)
        {
            this.name = name;
            this.inventorySlots = inventorySlots;
        }

        /// <summary>
        /// Return group name.
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return name;
        }

        /// <summary>
        /// Set group name.
        /// </summary>
        /// <param name="name"></param>
        public void SetName(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Return group slots.
        /// </summary>
        /// <returns></returns>
        public List<InventorySlot> GetInventorySlots()
        {
            return inventorySlots;
        }

        /// <summary>
        /// Set range group slots.
        /// </summary>
        /// <param name="inventorySlots"></param>
        public void SetInventorySlotsRange(List<InventorySlot> inventorySlots)
        {
            this.inventorySlots = inventorySlots;
        }

        /// <summary>
        /// Return group slot.
        /// </summary>
        /// <param name="index">Group slot index.</param>
        /// <returns></returns>
        public InventorySlot GetInventorySlot(int index)
        {
            return inventorySlots[index];
        }

        /// <summary>
        /// Set group slot.
        /// </summary>
        /// <param name="index">Group slot index.</param>
        /// <param name="inventorySlot">Group slot.</param>
        public void SetInventorySlot(int index, InventorySlot inventorySlot)
        {
            inventorySlots[index] = inventorySlot;
        }

        /// <summary>
        /// Return group slots array length.
        /// </summary>
        /// <returns></returns>
        public int GetInventorySlotsLength()
        {
            return inventorySlots.Count;
        }

        /// <summary>
        /// Empty Inventory group.
        /// </summary>
        /// <returns></returns>
        public readonly static InventoryGroup Empty = new InventoryGroup("----", null);

        public static bool operator ==(InventoryGroup left, InventoryGroup right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(InventoryGroup left, InventoryGroup right)
        {
            return !Equals(left, right);
        }

        public override bool Equals(object obj)
        {
            return (obj is InventoryGroup metrics) && Equals(metrics);
        }

        public bool Equals(InventoryGroup other)
        {
            return (name) == (other.name);
        }

        public override int GetHashCode()
        {
            return (name, inventorySlots).GetHashCode();
        }
    }
}