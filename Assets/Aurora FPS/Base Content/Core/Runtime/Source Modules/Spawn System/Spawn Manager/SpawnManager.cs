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
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField] private SpawnSettings[] spawnItems;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        protected virtual void Start()
        {
            for (int i = 0; i < spawnItems.Length; i++)
            {
                SpawnSettings spawnItem = spawnItems[i];
                if(spawnItem.ExecuteOnStart())
                {
                    StartCoroutine(spawnItem.ExecuteSpawn(this));
                }
            }
        }

        /// <summary>
        /// Execute SpawnItem by id.
        /// </summary>
        /// <param name="id">SpawnItem specific id.</param>
        public void ExecuteSpawnItem(string id)
        {
            for (int i = 0; i < spawnItems.Length; i++)
            {
                SpawnSettingsBase spawnItem = spawnItems[i];
                if(spawnItem.GetID() == id)
                {
                    StartCoroutine(spawnItem.ExecuteSpawn(this));
                    return;
                }
            }
        }

        #region [Getter / Setter]
        

        public SpawnSettings[] GetSpawnItems()
        {
            return spawnItems;
        }

        public void SetSpawnItems(SpawnSettings[] value)
        {
            spawnItems = value;
        }

        public SpawnSettings GetSpawnItem(string id)
        {
            for (int i = 0; i < spawnItems.Length; i++)
            {
                SpawnSettings spawnItem = spawnItems[i];
                if(spawnItem.GetID() == id)
                {
                    return spawnItem;
                }
            }
            return null;
        }

        public SpawnSettings GetSpawnItem(int index)
        {
            return spawnItems[index];
        }

        public void SetSpawnItem(int index, SpawnSettings value)
        {
            spawnItems[index] = value;
        }

        public void SetSpawnItem(string id, SpawnSettings value)
        {
            for (int i = 0; i < spawnItems.Length; i++)
            {
                if(spawnItems[i].GetID() == id)
                {
                    spawnItems[i] = value;
                }
            }
        }

        public int GetSpawnItemsLenght()
        {
            return spawnItems?.Length ?? 0;
        }
        #endregion
    }
}