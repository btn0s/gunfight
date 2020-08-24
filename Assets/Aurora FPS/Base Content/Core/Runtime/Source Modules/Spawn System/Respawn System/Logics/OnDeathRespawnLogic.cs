/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

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
    [System.Serializable]
    [RespawnLogicMenu("Character/On Death")]
    public class OnDeathRespawnLogic : DelayedRespawnLogic
    {
        // Base character respawn logic properties.
        [SerializeField] private int respawnHealth = 100;

        // Stored required components.
        private CharacterHealth characterHealth;

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
            characterHealth = target.GetComponent<CharacterHealth>();
        }

        /// <summary>
        /// Activate target object.
        /// </summary>
        protected override void ActivateObject()
        {
            base.ActivateObject();
            characterHealth.SetHealth(respawnHealth);
        }

        /// <summary>
        /// Respawn logic execute condition.
        /// </summary>
        public override bool Execute()
        {
            return !characterHealth.IsAlive();
        }
    }
}