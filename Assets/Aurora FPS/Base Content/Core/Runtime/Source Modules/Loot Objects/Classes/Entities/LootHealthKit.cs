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
    public class LootHealthKit : LootObject
    {
        // Base loot ammo properties.
        [SerializeField] private int healthPoint = 100;
        [SerializeField] private bool maximaze;

        // Stored required properties.
        private ObjectHealth objectHealth;
        
        #region [LootObject Implementation]
        /// <summary>
        /// Called once when target trying to loot this object.
        /// </summary>
        /// <param name="target">Transform instance that want to loot this object.</param>
        protected override void OnLoot(Transform target)
        {
            if (!maximaze)
                objectHealth?.SetHealth(objectHealth.GetHealth() + healthPoint);
            else
                objectHealth.SetHealth(objectHealth.GetMaxHealth());
        }
        #endregion

        #region [ILootObjectCallbacks Implementation]
        /// <summary>
        /// Called once when player become visible loot object.
        /// </summary>
        /// <param name="target">Transform instance that want to loot this object.</param>
        public override void OnBecomeVisible(Transform target)
        {
            objectHealth = target.GetComponent<ObjectHealth>();
        }

        /// <summary>
        /// Called once when player become invisible loot object.
        /// </summary>
        public override void OnBecomeInVisible()
        {
            objectHealth = null;
        }

        /// <summary>
        /// Called once when player looted object.
        /// </summary>
        public override void AfterLoot()
        {
            objectHealth = null;
        }
        #endregion

        #region [ILootObjectAvailable Implementation]
        /// <summary>
        /// Object is available to loot.
        /// </summary>
        public override bool AvailableToLoot()
        {
            return objectHealth.GetHealth() < objectHealth.GetMaxHealth();
        }
        #endregion

        #region [Getter / Setter]
        public int GetHealthPoint()
        {
            return healthPoint;
        }

        public void SetHealthPoint(int value)
        {
            healthPoint = value;
        }

        public bool GetMaximaze()
        {
            return maximaze;
        }

        public void SetMaximaze(bool value)
        {
            maximaze = value;
        }

        public ObjectHealth GetObjectHealth()
        {
            return objectHealth;
        }

        public void SetObjectHealth(ObjectHealth value)
        {
            objectHealth = value;
        }
        #endregion
    }
}