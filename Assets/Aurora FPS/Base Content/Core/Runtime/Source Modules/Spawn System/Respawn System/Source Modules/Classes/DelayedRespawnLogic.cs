/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using UnityEngine;
using System.Collections;

namespace AuroraFPSRuntime
{
    public abstract class DelayedRespawnLogic : RespawnLogic
    {
        // Base editable properties.
        [SerializeField] private float delayBeforeSpawn;
        [SerializeField] private float delayBeforeActivate;

        // Stored required properties.
        private CoroutineObject delayCoroutine;

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
            delayCoroutine = new CoroutineObject(respawnHandler);
        }

        /// <summary>
        /// Respawn logic.
        /// Override this method to make custom respawn logic.
        /// </summary>
        public override void Respawn()
        {
            delayCoroutine.Start(DelayCoroutine);
        }

        /// <summary>
        /// Called after spedific delay.
        /// Override this method to make custom spawn logic.
        /// </summary>
        protected virtual void OnSpawn()
        {
            ApplyPosition(spawnSettings.GetSpawnPosition());
            ApplyRotation(spawnSettings.GetSpawnRotation());
            ActivateObject();
        }

        /// <summary>
        /// Called after spedific delay.
        /// Override this method to make custon target activation.
        /// </summary>
        protected virtual void OnActivate()
        {
            ActivateObject();
        }

        /// <summary>
        /// Called actions after specific delays.
        /// </summary>
        protected IEnumerator DelayCoroutine()
        {
            float time = delayBeforeSpawn;
            OnBeforeSpawn();
            do
            {
                OnBeforeSpawnDelay(time);
                time -= Time.deltaTime;
                yield return null;
            } while (time > 0);

            OnSpawn();

            time = delayBeforeActivate;
            OnBeforeActivate();
            do
            {
                OnBeforeActivateDelay(time);
                time -= Time.deltaTime;
                yield return null;
            } while (time > 0);

            OnActivate();
        }

        /// <summary>
        /// Called when once when start spawn delay timer. 
        /// </summary>
        protected virtual void OnBeforeSpawn()
        {

        }

        /// <summary>
        /// Called every time, while running spawn delay timer.
        /// </summary>
        protected virtual void OnBeforeSpawnDelay(float remainingTime)
        {

        }

        /// <summary>
        /// Called when once when start activate delay timer. 
        /// </summary>
        protected virtual void OnBeforeActivate()
        {

        }

        /// <summary>
        /// Called every time, while running activate delay timer.
        /// </summary>
        protected virtual void OnBeforeActivateDelay(float remainingTime)
        {

        }

        #region [Getter / Setter]
        public float GetDelayBeforeSpawn()
        {
            return delayBeforeSpawn;
        }

        public void SetDelayBeforeSpawn(float value)
        {
            delayBeforeSpawn = value;
        }

        public float GetDelayBeforeActivate()
        {
            return delayBeforeActivate;
        }

        public void SetDelayBeforeActivate(float value)
        {
            delayBeforeActivate = value;
        }
        #endregion
    }
}