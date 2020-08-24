/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using UnityEngine;
using AuroraFPSRuntime.Serialization.Collections;
using Allocator = AuroraFPSRuntime.PoolContainerBase.Allocator;

namespace AuroraFPSRuntime
{
    public class PoolManager : Singleton<PoolManager>
    {
        [SerializeField] private DictionaryStringToPoolContainer pool = new DictionaryStringToPoolContainer();

        /// <summary>
        /// Instantiate new pool container with reserved pool objects.
        /// </summary>
        /// <param name="original">PoolObject instance.</param>
        /// <param name="allocator">Container allocator type.</param>
        /// <param name="capacity">Reserved container capacity.</param>
        public void InstantiateContainer(PoolObject original, Allocator allocator, int capacity)
        {
            string id = original.GetPoolObjectID();

            GameObject parent = new GameObject();
            parent.name = string.Format("{0} [Container]", id);
            parent.transform.SetParent(transform);
            parent.transform.localPosition = Vector3.zero;
            parent.transform.localRotation = Quaternion.identity;

            PoolContainer container = new PoolContainer(original, allocator, capacity);
            for (int i = 0; i < capacity; i++)
            {
                GameObject poolObject = Instantiate(original.gameObject);
                poolObject.transform.SetParent(parent.transform);
                poolObject.transform.localPosition = Vector3.zero;
                poolObject.transform.localRotation = Quaternion.identity;
                poolObject.SetActive(false);
                container.Push(poolObject);
            }
            pool.Add(id, container);
        }

        /// <summary>
        /// Create or pop gameobject.
        /// 
        /// If original gameobjects contains in pool return free gameobject depending on the type of pool container allocator.
        /// 
        /// Else create new pool container by original gameobject and return it.
        /// New pool container settings: [Allocator - Free], [Default size - 1]
        /// </summary>
        /// <param name="original">Original GameObject instance.</param>
        /// <param name="position">Position for the new object.</param>
        /// <param name="rotation">Orientation of the new object.</param>
        /// <param name="parent">Parent that will be assigned to the new object.</param>
        /// <returns>The instantiated or poped gameobject.</returns>
        public GameObject CreateOrPop(PoolObject original, Vector3 position, Quaternion rotation, Transform parent)
        {
            GameObject gameObject = CreateOrPop(original, position, rotation);
            if (gameObject != null)
            {
                gameObject.transform.SetParent(parent);
            }
            return gameObject;
        }

        /// <summary>
        /// Create or pop gameobject.
        /// 
        /// If original gameobjects contains in pool return free gameobject depending on the type of pool container allocator.
        /// 
        /// Else create new pool container by original gameobject and return it.
        /// New pool container settings: [Allocator - Free], [Default size - 1]
        /// </summary>
        /// <param name="original">Original GameObject instance.</param>
        /// <param name="position">Position for the new object.</param>
        /// <param name="rotation">Orientation of the new object.</param>
        /// <param name="parent">Parent that will be assigned to the new object.</param>
        /// <returns>The instantiated or poped gameobject.</returns>
        public GameObject CreateOrPop(PoolObject original, Vector3 position, Quaternion rotation)
        {
            GameObject gameObject = CreateOrPop(original);
            if (gameObject != null)
            {
                gameObject.transform.position = position;
                gameObject.transform.rotation = rotation;
            }
            return gameObject;
        }

        /// <summary>
        /// Create or pop gameobject.
        /// 
        /// If original gameobjects contains in pool manager return free gameobject depending on the type of pool container allocator.
        /// 
        /// Else create new pool container by original gameobject and return it.
        /// New pool container settings: [Allocator - Free], [Default size - 1]
        /// </summary>
        /// <param name="original">Original GameObject instance.</param>
        /// <param name="position">Position for the new object.</param>
        /// <param name="rotation">Orientation of the new object.</param>
        /// <param name="parent">Parent that will be assigned to the new object.</param>
        /// <returns>The instantiated or poped gameobject.</returns>
        public GameObject CreateOrPop(PoolObject original)
        {
            if (original != null)
            {
                string id = original.GetPoolObjectID();
                PoolContainer container = null;

                if (!pool.TryGetValue(id, out container))
                {
                    container = new PoolContainer(original, Allocator.Free, 1);
                    pool.Add(id, container);
                }

                GameObject value = container.Pop();
                return value;
            }
            return null;
        }

        /// <summary>
        /// Pop gameobject form pool container by pool id.
        /// </summary>
        /// <param name="id">Pool container ID (By default used name of original gameobject).</param>
        /// <returns>
        /// If ID contains in pool manager: Poped gameobject.
        /// Else: null.
        /// </returns>
        public GameObject Pop(string id)
        {
            PoolContainer container = null;
            if (pool.TryGetValue(id, out container))
            {
                GameObject value = container.Pop();
                return value;
            }

            return null;
        }

        /// <summary>
        /// Pop gameobject form pool container by pool id.
        /// </summary>
        /// <param name="original">Original gameobject.</param>
        /// <returns>
        /// If ID contains in pool manager: Poped gameobject.
        /// Else: null.
        /// </returns>
        public GameObject Pop(PoolObject original)
        {
            if (original != null)
            {
                PoolContainer container = null;
                if (pool.TryGetValue(original.GetPoolObjectID(), out container))
                {
                    GameObject value = container.Pop();
                    return value;
                }
            }

            return null;
        }

        /// <summary>
        /// Push gameobject to pool container.
        /// </summary>
        /// <param name="value">Gameobject to push.</param>
        public void Push(PoolObject value)
        {
            if (value != null)
            {
                PoolContainer container = null;
                if (!pool.TryGetValue(value.GetPoolObjectID(), out container))
                {
                    container = new PoolContainer(value, Allocator.Free, 1);
                    pool.Add(value.GetPoolObjectID(), container);
                }
                container.Push(value.gameObject);
                value.transform.SetParent(transform);
            }
        }

        /// <summary>
        /// Try to peek value from pool container by id without remove it or creating new if is container is empty.
        /// 
        /// Note: value won't be activated after peek.
        /// </summary>
        /// <param name="value">
        /// First available pool value.
        /// If the pool container is empty, the output value will be null.
        /// </param>
        /// <returns>True if pool manager contains container with this id and container contains available gameobject for peek, else false.</returns>
        public bool TryPeek(string id, out GameObject value)
        {
            PoolContainer container = null;
            if (pool.TryGetValue(id, out container))
            {
                return container.TryPeek(out value);
            }
            value = null;
            return false;
        }

        /// <summary>
        /// Try to peek value from pool container by id without remove it or creating new if is container is empty.
        /// 
        /// Note: value won't be activated after peek.
        /// </summary>
        /// <param name="value">
        /// First available pool value.
        /// If the pool container is empty, the output value will be null.
        /// </param>
        /// <returns>True if pool manager contains container with this id and container contains available gameobject for peek, else false.</returns>
        public bool TryPeek(PoolObject original, out GameObject value)
        {
            PoolContainer container = null;
            if (original != null && pool.TryGetValue(original.GetPoolObjectID(), out container))
            {
                return container.TryPeek(out value);
            }
            value = null;
            return false;
        }

        /// <summary>
        /// Reallocate container by id.
        /// </summary>
        /// <param name="id">Pool container id.</param>
        /// <param name="allocator">New container allocator type.</param>
        /// <param name="capacity">New container capacity.</param>
        public void ReallocateContainer(string id, Allocator allocator, int capacity)
        {
            PoolContainer container = null;
            if (pool.TryGetValue(id, out container))
            {
                container.Reallocate(allocator, capacity);
            }
        }

        /// <summary>
        /// Reallocate container by original PoolObject id.
        /// </summary>
        /// <param name="original">PoolObject original instance.</param>
        /// <param name="allocator">New container allocator type.</param>
        /// <param name="capacity">New container capacity.</param>
        public void ReallocateContainer(PoolObject original, Allocator allocator, int capacity)
        {
            if (original != null)
            {
                string id = original.GetPoolObjectID();
                PoolContainer container = null;
                if (pool.TryGetValue(id, out container))
                {
                    container.Reallocate(allocator, capacity);
                }
            }
        }

        /// <summary>
        /// Trying to remove a pool container with all the objects it contains from pool.
        /// </summary>
        /// <param name="id">Pool container id.</param>
        /// <returns>
        /// True if container contains in pool.
        /// Else false.
        /// </returns>
        public bool RemoveContainer(string id)
        {
            PoolContainer container = null;
            if (pool.TryGetValue(id, out container))
            {
                foreach (GameObject poolObject in container.GetObjectsStack())
                {
#if UNITY_EDITOR
                    DestroyImmediate(poolObject);
#else
                    Destroy(poolObject);
#endif
                }
                container = null;
                pool.Remove(id);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Trying to remove a pool container with all the objects it contains from pool.
        /// </summary>
        /// <param name="original">Original PoolObject instance.</param>
        /// <returns>
        /// True if original gameobject not null & container contains in pool.
        /// Else false.
        /// </returns>
        public bool RemoveContainer(PoolObject original)
        {
            if (original != null)
            {
                string id = original.GetPoolObjectID();
                PoolContainer container = null;
                if (pool.TryGetValue(id, out container))
                {
                    foreach (GameObject poolObject in container.GetObjectsStack())
                    {
#if UNITY_EDITOR
                        DestroyImmediate(poolObject);
#else
                    Destroy(poolObject);
#endif
                    }
                    container = null;
                    pool.Remove(id);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checking that the pool container with the specified id is contained in the pool.
        /// </summary>
        /// <param name="id">Pool container id.</param>
        /// <returns>
        /// True if container contains in pool.
        /// Else false.
        /// </returns>
        public bool ContainsContainer(string id)
        {
            return pool.ContainsKey(id);
        }

        /// <summary>
        /// Checking that the pool container with the original PoolObject is contained in the pool.
        /// </summary>
        /// <param name="original">Original PoolObject instance.</param>
        /// <returns>
        /// True if original gameobject not null & container contains in pool.
        /// Else false.
        /// </returns>
        public bool ContainsContainer(PoolObject original)
        {
            if (original != null)
            {
                return pool.ContainsKey(original.GetPoolObjectID());
            }
            return false;
        }

        /// <summary>
        /// Get pool container from pool manager.
        /// </summary>
        /// <param name="id">Pool container id.</param>
        /// <returns>
        /// If pool container contains in pool manager return container.
        /// Else return null.
        /// </returns>
        public PoolContainer GetPoolContainer(string id)
        {
            PoolContainer container = null;
            pool.TryGetValue(id, out container);
            return container;
        }

        /// <summary>
        /// Get pool container from pool manager.
        /// </summary>
        /// <param name="original">Original pool object of container.</param>
        /// <returns>
        /// If pool container contains in pool manager return container.
        /// Else return null.
        /// </returns>
        public PoolContainer GetPoolContainer(PoolObject original)
        {
            if (original != null)
            {
                PoolContainer container = null;
                pool.TryGetValue(original.GetPoolObjectID(), out container);
                return container;
            }
            return null;
        }

        #region [Getter / Setter]
        public DictionaryStringToPoolContainer GetPool()
        {
            return pool;
        }

        protected void SetPool(DictionaryStringToPoolContainer value)
        {
            pool = value;
        }

        public int ContainerCount()
        {
            return pool.Count;
        }
        #endregion
    }
}