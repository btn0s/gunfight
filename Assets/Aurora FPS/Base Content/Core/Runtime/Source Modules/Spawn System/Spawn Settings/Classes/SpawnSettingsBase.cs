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
    public abstract class SpawnSettingsBase : ISpawnSettings
    {
        [SerializeField] private string id;

        /// <summary>
        /// Execute item spawning.
        /// </summary>
        public abstract IEnumerator ExecuteSpawn(SpawnManager spawnManager);

        #region [Getter / Setter]
        public string GetID()
        {
            return id;
        }

        public void SetID(string value)
        {
            id = value;
        }
        #endregion
    }
}