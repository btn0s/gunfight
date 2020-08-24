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
    public class LootObjectSystem : MonoBehaviour
    {
        // Base loot object system properties.
        [SerializeField] private Transform searchPoint;
        [SerializeField] private HUDManager hudManager;
        [SerializeField] private float searchDistance;
        [SerializeField] private LayerMask cullingLayer;

        // Advanced loot object system properties.
        [SerializeField] private float searchRate;

        // Stored required properties.
        private CoroutineObject lootObjectSearch;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            lootObjectSearch = new CoroutineObject(this);

            OnLootObjectFindCallback += value => hudManager.GetElements().DisplayMessage(value.GetLootMessage());
            OnLootObjectLostCallback += () => hudManager.GetElements().HideMessage();
        }

        /// <summary>
        /// Searching loot objects coroutine with specific rate.
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator SearchLootObject()
        {
            WaitForSeconds waitForRate = new WaitForSeconds(searchRate);
            LootObjectBase storedLootObject = null;
            bool lootObjectIsFinded = false;
            if (searchPoint == null)
            {
                searchPoint = transform;
            }

            while (true)
            {
                if (Physics.Raycast(searchPoint.position, searchPoint.forward, out RaycastHit hitInfo, searchDistance, cullingLayer, QueryTriggerInteraction.Ignore))
                {
                    LootObjectBase lootObject = hitInfo.transform.GetComponent<LootObjectBase>();
                    if (lootObject != null && lootObject.IsEnabled())
                    {
                        if (lootObject != storedLootObject)
                        {
                            storedLootObject?.Loot(null);
                            lootObject.Loot(transform);
                            storedLootObject = lootObject;
                            if (!lootObjectIsFinded)
                            {
                                OnLootObjectFindCallback?.Invoke(lootObject);
                                lootObjectIsFinded = true;
                            }
                        }
                    }
                    else if (lootObjectIsFinded)
                    {
                        OnLootObjectLostCallback?.Invoke();
                        lootObjectIsFinded = false;
                    }
                }
                else
                {
                    if (storedLootObject != null)
                    {
                        storedLootObject.Loot(null);
                        storedLootObject = null;
                    }
                    if (lootObjectIsFinded)
                    {
                        OnLootObjectLostCallback?.Invoke();
                        lootObjectIsFinded = false;
                    }
                }
                yield return waitForRate;
            }
        }

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        protected virtual void OnEnable()
        {
            lootObjectSearch.Start(SearchLootObject);
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        protected virtual void OnDisable()
        {
            lootObjectSearch.Stop();
        }

        #region [Event Callback Functions]
        /// <summary>
        /// Called every time when any loot object being finded.
        /// </summary>
        public event System.Action<LootObjectBase> OnLootObjectFindCallback;

        /// <summary>
        /// Called every time when loot object being lost or looted.
        /// </summary>
        public event System.Action OnLootObjectLostCallback;
        #endregion

        #region [Getter / Setter]
        public Transform GetSearchPoint()
        {
            return searchPoint;
        }

        public void SetSearchPoint(Transform value)
        {
            searchPoint = value;
        }

        public HUDManager GetHUDManager()
        {
            return hudManager;
        }

        public void SetHUDManager(HUDManager value)
        {
            hudManager = value;
        }

        public float GetSearchDistance()
        {
            return searchDistance;
        }

        public void SetSearchDistance(float value)
        {
            searchDistance = value;
        }

        public LayerMask GetCullingLayer()
        {
            return cullingLayer;
        }

        public void SetCullingLayer(LayerMask value)
        {
            cullingLayer = value;
        }

        public float GetSearchRate()
        {
            return searchRate;
        }

        public void SetSearchRate(float value)
        {
            searchRate = value;
        }
        #endregion
    }
}