/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using System.Collections;
using UnityEngine;
using AuroraFPSRuntime.Serialization.Collections;
using System.Collections.Generic;

namespace AuroraFPSRuntime
{
    [System.Serializable]
    public class PoolContainer : PoolContainerBase, IEnumerable, IEnumerable<GameObject>
    {
        // Base PoolContainer properties.
        [SerializeField] protected StackGameObject objectsStack;
        [SerializeField] protected PoolObject original;
        [SerializeField] protected Allocator allocator;
        [SerializeField] protected int capacity;

        /// <summary>
        /// Initializes a new instance of the PoolContainer class.
        /// </summary>
        /// <param name="original">Object type of (PoolObject) that will contains in this container.</param>
        public PoolContainer(PoolObject original)
        {
            objectsStack = new StackGameObject();
            this.original = original;
            this.allocator = Allocator.Dynamic;
            this.capacity = 1;
        }

        /// <summary>
        /// Initializes a new instance of the PoolContainer class.
        /// </summary>
        /// <param name="original">Object type of (PoolObject) that will contains in this container.</param>
        /// <param name="allocator">Container objects allocator type.</param>
        /// <param name="capacity">Reserved container capacity for objects.</param>
        public PoolContainer(PoolObject original, Allocator allocator, int capacity)
        {
            objectsStack = new StackGameObject();
            this.original = original;
            this.allocator = allocator;
            this.capacity = capacity;
        }

        /// <summary>
        /// Push pool value in pool container.
        /// </summary>
        /// <param name="value">Value type of UnityEngine.Object.</param>
        public virtual void Push(GameObject value)
        {
            if (value != null)
            {
                if (objectsStack.Count < capacity)
                {
                    value.SetActive(false);
                    objectsStack.Push(value);
                }
                else
                {
                    GameObject.Destroy(value);
                }
            }
        }

        /// <summary>
        /// Get available value form pool container.
        /// </summary>
        /// <returns>
        /// Available pool value type of UnityEngine.Object.
        /// If pool container is empty, return the value depending on the type of pool container allocator.
        /// </returns>
        public virtual GameObject Pop()
        {
            if (objectsStack.Count > 0)
            {
                GameObject value = objectsStack.Pop();
                if (value == null)
                {
                    value = GameObject.Instantiate(original.gameObject);
                }
                value.SetActive(true);
                return value;
            }
            switch (allocator)
            {
                case Allocator.Free:
                    {
                        GameObject value = GameObject.Instantiate(original.gameObject);
                        capacity++;
                        return value;
                    }

                case Allocator.Dynamic:
                    {
                        GameObject value = GameObject.Instantiate(original.gameObject);
                        return value;
                    }
            }
            return null;
        }

        /// <summary>
        /// Try to peek value from container without remove it or creating new if is container is empty.
        /// 
        /// Note: value won't be activated after peek.
        /// </summary>
        /// <param name="value">
        /// First available pool value type of UnityEngine.Object.
        /// If the pool container is empty, the output value will be null.
        /// </param>
        /// <returns>True if container contains available object for peek, else false.</returns>
        public virtual bool TryPeek(out GameObject value)
        {
            if (objectsStack.Count > 0)
            {
                value = objectsStack.Peek();
                return true;
            }
            value = null;
            return false;
        }

        /// <summary>
        /// Reallocate pool container type.
        /// </summary>
        /// <param name="allocator">Container allocator type.</param>
        /// <param name="size">Container size.</param>
        public virtual void Reallocate(Allocator allocator, int size)
        {
            this.allocator = allocator;
            this.capacity = size;
            if (objectsStack.Count > size)
            {
                int count = objectsStack.Count - size;
                for (int i = 0; i < count; i++)
                {
                    GameObject value = objectsStack.Pop();
                    if (value != null)
                    {
#if UNITY_EDITOR
                        GameObject.DestroyImmediate(value);
#else
                        GameObject.Destroy(value);
#endif
                    }
                }
            }
            else if (objectsStack.Count < size)
            {
                int count = size - objectsStack.Count;
                for (int i = 0; i < count; i++)
                {
                    GameObject value = GameObject.Instantiate(original.gameObject);
                    if (value != null)
                    {
                        Push(value);
                    }
                }
            }

        }

        /// <summary>
        /// Length of available object in pool.
        /// </summary>
        public sealed override int GetLength()
        {
            return objectsStack.Count;
        }

        #region [IEnumerable / IEnumerable<T> Implementation]
        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An IEnumerator that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)objectsStack).GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An IEnumerator(GameObject) that can be used to iterate through the collection.</returns>
        public IEnumerator<GameObject> GetEnumerator()
        {
            return objectsStack.GetEnumerator();
        }
        #endregion

        #region [Getter / Setter]
        public StackGameObject GetObjectsStack()
        {
            return objectsStack;
        }

        public PoolObject GetOriginal()
        {
            return original;
        }

        public Allocator GetAllocator()
        {
            return allocator;
        }

        public int GetSize()
        {
            return capacity;
        }
        #endregion
    }
}