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
    public abstract class RespawnLogic : RespawnLogicBase
    {
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
        }

        /// <summary>
        /// Respawn logic.
        /// Override this method to make custom respawn logic.
        /// </summary>
        public override void Respawn()
        {
            ApplyPosition(spawnSettings.GetSpawnPosition());
            ApplyRotation(spawnSettings.GetSpawnRotation());
            ActivateObject();
        }

        /// <summary>
        /// Apply spawn point position to the target.
        /// </summary>
        /// <param name="position">Spawn point position.</param>
        protected virtual void ApplyPosition(Vector3 position)
        {
            target.position = position;
        }

        /// <summary>
        /// Apply spawn point rotation to the target.
        /// </summary>
        /// <param name="rotation"></param>
        protected virtual void ApplyRotation(Quaternion rotation)
        {
            target.rotation = rotation;
        }

        /// <summary>
        /// Activate target object.
        /// </summary>
        protected virtual void ActivateObject()
        {
            if (spawnSettings.IsPoolObject() && poolObject != null)
            {
                PoolManager.Instance.CreateOrPop(poolObject);
            }
            else if (!target.gameObject.activeSelf)
            {
                target.gameObject.SetActive(true);
            }
        }
    }
}