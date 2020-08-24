/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using System.Collections;
using AuroraFPSRuntime.UI;
using UnityEngine;

namespace AuroraFPSRuntime
{
    [RespawnLogicMenu("Character/On Death (HUD)")]
    public class OnDeathHUDRespawnLogic : OnDeathRespawnLogic
    {
        // Base character respawn logic properties.
        [SerializeField] private string displayFormat = "Respawn after [{0}]";
        [SerializeField] private string timeFormat = "0.00";

        // Stored required components.
        private HUDManager hudManager;

        /// <summary>
        /// Initialize respawn system.
        /// Override this method to initialize properties.
        /// </summary>
        /// <param name="target">Respawn system target transform.</param>
        /// <param name="spawnSettings">Target spawn settings.</param>
        /// <param name="respawnHandler">Target respawn handler instance.</param>
        public override void Initialize(Transform target, SpawnSettings spawnSettings, RespawnHandler respawnHandler)
        {
            base.Initialize(target, spawnSettings, respawnHandler);
            hudManager = target.GetComponentInChildren<HUDManager>();
        }

        /// <summary>
        /// Called every time, while running spawn delay timer.
        /// </summary>
        protected override void OnBeforeSpawnDelay(float remainingTime)
        {
            base.OnBeforeSpawnDelay(remainingTime);
            string message = string.Format(displayFormat, remainingTime.ToString(timeFormat));
            hudManager.GetElements().DisplayMessage(message);
        }

        /// <summary>
        /// Called when once when start activate delay timer. 
        /// </summary>
        protected override void OnBeforeActivate()
        {
            base.OnBeforeActivate();
            hudManager.GetElements().HideMessage();
        }
    }
}