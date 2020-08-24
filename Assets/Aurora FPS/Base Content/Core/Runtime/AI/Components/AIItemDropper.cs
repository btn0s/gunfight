/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using UnityEngine;

namespace AuroraFPSRuntime.AI
{
    [RequireComponent(typeof(CharacterHealth))]
    public class AIItemDropper : MonoBehaviour
    {
        [System.Serializable]
        public struct Item
        {
            [SerializeField] private GameObject itemObject;
            [SerializeField] private float throwForce;
            [SerializeField] private Vector3 throwDirection;

            private Rigidbody itemRigidbody;

            public void Initialize()
            {
                itemRigidbody = itemObject.GetComponent<Rigidbody>();
                itemRigidbody.isKinematic = true;
            }

            public void Throw()
            {
                if (!itemObject.activeSelf)
                    itemObject.SetActive(true);

                itemObject.transform.parent = null;

                itemRigidbody.isKinematic = false;

                if (throwForce > 0)
                    itemRigidbody.AddForce(throwDirection * throwForce, ForceMode.Impulse);

            }

            #region [Getter / Setter]
            public GameObject GetItemObject()
            {
                return itemObject;
            }

            public void SetItemObject(GameObject value)
            {
                itemObject = value;
            }

            public float GetThrowForce()
            {
                return throwForce;
            }

            public void SetThrowForce(float value)
            {
                throwForce = value;
            }

            public Vector3 GetThrowDirection()
            {
                return throwDirection;
            }

            public void SetThrowDirection(Vector3 value)
            {
                throwDirection = value;
            }
            #endregion
        }

        // Base AI item dropper properties.
        [SerializeField] private Item[] items;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            InitializeAllItems();

            // If CharacterHealth contained in the AICore controller, then register the necessary callback.
            // If AI is die, automatically throw all items.
            CharacterHealth health = GetComponent<CharacterHealth>();
            if (health != null)
            {
                health.OnDeadCallback += ThrowAllItems;
            }
        }

        /// <summary>
        /// Initialize all items.
        /// </summary>
        protected virtual void InitializeAllItems()
        {
            for (int i = 0, length = items.Length; i < length; i++)
            {
                items[i].Initialize();
            }
        }

        /// <summary>
        /// Throw all items.
        /// </summary>
        public virtual void ThrowAllItems()
        {
            for (int i = 0, length = items.Length; i < length; i++)
            {
                items[i].Throw();
            }
            OnThrowAllItemsCallback?.Invoke();
        }

        /// <summary>
        /// Throw item by index.
        /// </summary>
        /// <param name="index"></param>
        public void ThowItem(int index)
        {
            items[index].Throw();
        }

        #region [Event Callback Functions]
        /// <summary>
        /// Called when AI death and throw all items.
        /// </summary>
        public event System.Action OnThrowAllItemsCallback;
        #endregion

        #region [Getter / Setter]
        public Item GetItem(int index)
        {
            return items[index];
        }

        public Item[] GetItems()
        {
            return items;
        }

        public void SetItem(int index, Item value)
        {
            items[index] = value;
        }

        public void SetItems(Item[] value)
        {
            items = value;
        }
        #endregion
    }
}