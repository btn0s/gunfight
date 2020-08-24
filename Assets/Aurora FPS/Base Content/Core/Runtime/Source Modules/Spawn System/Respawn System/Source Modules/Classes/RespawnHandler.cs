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
    public class RespawnHandler : MonoBehaviour
    {
        // Base respawn system properties.
        [SerializeReference] private RespawnLogicBase respawnLogic;
        [SerializeField] private SpawnManager spawnManager;
        [SerializeField] private string spawnSettingsID;

        // Stored required properties.
        private SpawnSettings spawnSettings;

        /// <summary>
        /// Initialize respawn system.
        /// </summary>
        /// <param name="spawnManager">Target spawn manager.</param>
        /// <param name="spawnSettingsID">Target spawn item id.</param>
        public void Initialize(RespawnLogicBase respawnLogic, SpawnManager spawnManager, string spawnSettingsID)
        {
            this.respawnLogic = respawnLogic;
            this.spawnManager = spawnManager;
            this.spawnSettingsID = spawnSettingsID;
            this.spawnSettings = spawnManager.GetSpawnItem(spawnSettingsID);
            this.respawnLogic?.Initialize(transform, spawnSettings, this);
        }

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            if (spawnManager != null && respawnLogic != null)
                spawnSettings = spawnManager.GetSpawnItem(spawnSettingsID);

            respawnLogic?.Initialize(transform, spawnSettings, this);
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void Update()
        {
            if(respawnLogic != null && respawnLogic.Execute())
            {
                respawnLogic.Respawn();
            }
        }

        #region [Getter / Setter]
        public SpawnManager GetSpawnManager()
        {
            return spawnManager;
        }

        public void SetSpawnManager(SpawnManager value)
        {
            spawnManager = value;
        }

        public string GetSpawnItemID()
        {
            return spawnSettingsID;
        }

        public void SetSpawnItemID(string value)
        {
            spawnSettingsID = value;
        }

        public SpawnSettings GetSpawnItem()
        {
            return spawnSettings;
        }

        public void SetSpawnItem(SpawnSettings value)
        {
            spawnSettings = value;
        }
        #endregion
    }
}