/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace AuroraFPSRuntime
{
    [Serializable]
    public class SpawnSettings : SpawnSettingsBase
    {
        public enum Placement
        {
            Point,
            MultiplePoints,
            Sphere
        }

        // Base spawn settings.
        [SerializeField] private GameObject target;
        [SerializeField] private Placement placement = Placement.Point;
        [SerializeField] private Transform[] points;
        [SerializeField] private Vector3 rotation = Vector3.zero;
        [SerializeField] private float radius;
        [SerializeField] private int instanceCount = 1;
        [SerializeField] private int repeatCount = 1;
        [SerializeField] private float executeDelay = 0;
        [SerializeField] private float instantiateDelay = 0;
        [SerializeField] private float repeatDelay = 0;
        [SerializeField] private bool sceneObject = false;
        [SerializeField] private bool executeOnStart = true;
        [SerializeField] private bool isPoolObject = false;
        [SerializeReference] private RespawnLogicBase respawnLogic;
        [SerializeField] private UnityEvent onSpawnUnityEvent;

        // Stored required properties.
        private int currentSpawnPoint;

        /// <summary>
        /// Execute spawn settings.
        /// </summary>
        public override IEnumerator ExecuteSpawn(SpawnManager spawnManager)
        {
            if (!sceneObject)
            {
                Transform point = placement == Placement.Point || placement == Placement.Sphere ? points[0] : null;
                PoolObject poolObject = null;
                int currectRepeatCount = 0;

                WaitForSeconds instantiateDelayYield = instantiateDelay == 0 ? null : new WaitForSeconds(instantiateDelay);
                WaitForSeconds repeatDelayYield = repeatDelay == 0 ? null : new WaitForSeconds(repeatDelay);

                OnSpawnCallback -= _ => onSpawnUnityEvent?.Invoke();
                OnSpawnCallback += _ => onSpawnUnityEvent?.Invoke();

                if (isPoolObject)
                {
                    poolObject = target.GetComponent<PoolObject>();
                    if (poolObject == null)
                    {
                        poolObject = target.AddComponent<PoolObject>();
                    }
                }

                yield return new WaitForSeconds(executeDelay);
                do
                {
                    for (int i = 0; i < instanceCount; i++)
                    {
                        GameObject instance = null;

                        if (isPoolObject)
                            instance = PoolManager.Instance.CreateOrPop(poolObject);
                        else
                            instance = GameObject.Instantiate(target);

                        instance.transform.position = GetSpawnPosition();
                        instance.transform.rotation = GetSpawnRotation();
                        RespawnHandler respawnHandler = instance.AddComponent(typeof(RespawnHandler))as RespawnHandler;
                        respawnHandler.Initialize(respawnLogic, spawnManager, GetID());

                        OnSpawnCallback?.Invoke(instance);
                        yield return instantiateDelayYield;
                    }
                    currectRepeatCount++;
                    yield return repeatDelayYield;
                }
                while (currectRepeatCount < repeatCount);
            }
            else
            {
                RespawnHandler respawnHandler = target.AddComponent(typeof(RespawnHandler))as RespawnHandler;
                respawnHandler.Initialize(respawnLogic, spawnManager, GetID());
                OnSpawnCallback?.Invoke(target);
            }
        }

        public Vector3 GetSpawnPosition()
        {
            switch (placement)
            {
                case Placement.Point:
                    return points[0].position;
                case Placement.Sphere:
                    return AMath.RandomPositionInCircle(points[0].position, radius);
                case Placement.MultiplePoints:
                    Vector3 position = points[currentSpawnPoint].position;
                    currentSpawnPoint = currentSpawnPoint < points.Length - 1 ? currentSpawnPoint++ : 0;
                    return position;
            }
            return Vector3.zero;
        }

        public Quaternion GetSpawnRotation()
        {
            return Quaternion.Euler(rotation);
        }

        #region [Event Function Callback]
        /// <summary>
        /// On spawn event callback function.
        /// 
        /// OnSpawnCallback called when item spawned on scene.
        /// </summary>
        public event Action<GameObject> OnSpawnCallback;
        #endregion

        #region [Getter / Setter]
        public GameObject GetTarget()
        {
            return target;
        }

        public void SetTarget(GameObject value)
        {
            target = value;
        }

        public Placement GetPlacement()
        {
            return placement;
        }

        public void SetPlacement(Placement value)
        {
            placement = value;
        }

        public float GetRadius()
        {
            return radius;
        }

        public void SetRadius(float value)
        {
            radius = value;
        }

        public Transform[] GetPoints()
        {
            return points;
        }

        public void SetPoints(Transform[] value)
        {
            points = value;
        }

        public Transform GetPoint(int index)
        {
            return points[index];
        }

        public void SetPoint(int index, Transform value)
        {
            points[index] = value;
        }

        public Vector3 GetRotation()
        {
            return rotation;
        }

        public void SetRotation(Vector3 value)
        {
            rotation = value;
        }

        public int GetInstanceCount()
        {
            return instanceCount;
        }

        public void SetInstanceCount(int value)
        {
            instanceCount = value;
        }

        public int GetRepeatCount()
        {
            return repeatCount;
        }

        public void SetRepeatCount(int value)
        {
            repeatCount = value;
        }

        public float GeExecuteDelay()
        {
            return executeDelay;
        }

        public void SetExecuteDelay(float value)
        {
            executeDelay = value;
        }

        public float GetInstantiateDelay()
        {
            return instantiateDelay;
        }

        public void SetInstantiateDelay(float value)
        {
            instantiateDelay = value;
        }

        public float GetRepeatDelay()
        {
            return repeatDelay;
        }

        public void SetRepeatDelay(float value)
        {
            repeatDelay = value;
        }

        public bool ExecuteOnStart()
        {
            return executeOnStart;
        }

        public void ExecuteOnStart(bool value)
        {
            executeOnStart = value;
        }

        public bool SceneObject()
        {
            return sceneObject;
        }

        public void SceneObject(bool value)
        {
            sceneObject = value;
        }

        public bool IsPoolObject()
        {
            return isPoolObject;
        }

        public void IsPoolObject(bool value)
        {
            isPoolObject = value;
        }

        public RespawnLogicBase GetRespawnLogic()
        {
            return respawnLogic;
        }

        public void SetRespawnLogic(RespawnLogicBase value)
        {
            respawnLogic = value;
        }

        protected UnityEvent OnSpawnUnityEvent()
        {
            return onSpawnUnityEvent;
        }

        protected void OnSpawnUnityEvent(UnityEvent value)
        {
            onSpawnUnityEvent = value;
        }
        #endregion
    }
}