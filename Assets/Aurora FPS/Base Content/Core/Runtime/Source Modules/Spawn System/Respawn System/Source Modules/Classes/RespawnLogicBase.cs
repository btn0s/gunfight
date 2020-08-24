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
    public abstract class RespawnLogicBase : IRespawnLogic
    {
        // Stored required components.
        protected Transform target;
        protected RespawnHandler respawnHandler;
        protected SpawnSettings spawnSettings;
        protected PoolObject poolObject;

        /// <summary>
        /// Initialize respawn system.
        /// Override this method to initialize properties.
        /// </summary>
        /// <param name="target">Respawn system target transform.</param>
        /// <param name="spawnSettings">Target spawn settings.</param>
        /// <param name="respawnHandler">Target respawn handler instance.</param>
        public virtual void Initialize(Transform target, SpawnSettings spawnSettings, RespawnHandler respawnHandler)
        {
            this.target = target;
            this.respawnHandler = respawnHandler;
            this.spawnSettings = spawnSettings;
            this.poolObject = target.GetComponent<PoolObject>();
        }

        /// <summary>
        /// Respawn logic.
        /// Override this method to make custom respawn logic.
        /// </summary>
        public abstract void Respawn();

        /// <summary>
        /// Respawn logic execute condition.
        /// </summary>
        public abstract bool Execute();
    }
}