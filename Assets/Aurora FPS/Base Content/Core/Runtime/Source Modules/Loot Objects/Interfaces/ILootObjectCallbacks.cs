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
    public interface ILootObjectCallbacks
    {
        /// <summary>
        /// Called once when some target become visible loot object.
        /// </summary>
        /// <param name="target">Transform that become visible loot object.</param>
        void OnBecomeVisible(Transform target);

        /// <summary>
        /// Called once when player become invisible loot object.
        /// </summary>
        void OnBecomeInVisible();

        /// <summary>
        /// Called once when player looted object.
        /// </summary>
        void AfterLoot();
    }
}