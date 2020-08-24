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
    /// <summary>
    /// Base represents of loot object.
    /// </summary>
    public abstract class LootObjectBase : MonoBehaviour, ILootObject, ILootObjectCallbacks, ILootObjectAvailable, ILootObjectEnabled, ILootObjectMessage
    {
        #region [ILootObject Implementation]
        /// <summary>
        /// Loot this object to target transfrom.
        /// </summary>
        /// <param name="target">Transform instance that want to loot this object.</param>
        public abstract void Loot(Transform target);
        #endregion

        #region [ILootObjectCallbacks Implementation]
        /// <summary>
        /// Called once when player become visible loot object.
        /// </summary>
        /// <param name="target">Transform instance that want to loot this object.</param>
        public abstract void OnBecomeVisible(Transform target);

        /// <summary>
        /// Called once when player become invisible loot object.
        /// </summary>
        public abstract void OnBecomeInVisible();

        /// <summary>
        /// Called once when player looted object.
        /// </summary>
        public abstract void AfterLoot();
        #endregion

        #region [ILootObjectEnabled Implementation]
        /// <summary>
        /// Enabled state of loot object.
        /// </summary>
        /// <returns></returns>
        public abstract bool IsEnabled();
        #endregion

        #region [ILootObjectAvailable Implementation]
        /// <summary>
        /// Object is available to loot.
        /// </summary>
        public abstract bool AvailableToLoot();
        #endregion

        #region [ILootObjectMessage Implementation]
        /// <summary>
        /// Loot message that displayed on player HUD.
        /// </summary>
        public abstract string GetLootMessage();
        #endregion
    }
}