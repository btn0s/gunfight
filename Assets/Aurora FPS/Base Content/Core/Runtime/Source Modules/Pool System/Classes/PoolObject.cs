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
using UnityEngine.Events;

namespace AuroraFPSRuntime
{
    public class PoolObject : MonoBehaviour, IPoolObject
    {
        // Base pool object properties.
        [SerializeField] private string poolObjectID = AuroraExtension.GenerateID(7);
        [SerializeField] private float delayedTime = 7.0f;

        [SerializeField] private UnityEvent onBeforePushEvent;
        [SerializeField] private UnityEvent onAfterPushEvent;

        // Stored required components.
        private PoolManager poolManager;

        // Stored required properties.
        private CoroutineObject<float> delayedPushCoroutine;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            poolManager = PoolManager.Instance;

            delayedPushCoroutine = new CoroutineObject<float>(this);

            OnBeforePushCallback += () => onBeforePushEvent?.Invoke();
            OnAfterPushCallback += () => onAfterPushEvent?.Invoke();
        }

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        protected virtual void OnEnable()
        {
            if (delayedTime >= 0)
                delayedPushCoroutine.Start(DelayedPush, delayedTime);
        }
        
        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        protected virtual void OnDisable()
        {
            if (delayedTime >= 0)
                delayedPushCoroutine.Stop();
        }

        /// <summary>
        /// Delayed pool object push coroutine.
        /// Started when pool object being active and disable object after delayed time.
        /// The coroutine can be stopped if the object being disabled before the delayed time expires.
        /// 
        /// Implement this method to make custom delayed push coroutine.
        /// </summary>
        /// <param name="time">Delayed time to push pool object in pool.</param>
        protected virtual IEnumerator DelayedPush(float time)
        {
            yield return new WaitForSeconds(time);
            OnBeforePushCallback?.Invoke();
            Push();
            OnAfterPushCallback?.Invoke();
        }

        /// <summary>
        /// Push this object to pool.
        /// </summary>
        public void Push()
        {
            poolManager.Push(this);
        }

        #region [Event Callback Functions]
        /// <summary>
        /// Called in DelayedPush coroutine after delayed time, before pushing in pool.
        /// </summary>
        public event System.Action OnBeforePushCallback;

        /// <summary>
        /// Called in DelayedPush coroutine after delayed time, after pushing in pool.
        /// </summary>
        public event System.Action OnAfterPushCallback;
        #endregion

        #region [Getter / Setter]
        public string GetPoolObjectID()
        {
            return poolObjectID;
        }

        public void SetPoolObjectID(string value)
        {
            poolObjectID = value;
        }

        public float GetDelayedTime()
        {
            return delayedTime;
        }

        public void SetDelayedTime(float value)
        {
            delayedTime = value;
        }

        protected UnityEvent GetOnBeforePushEvent()
        {
            return onBeforePushEvent;
        }

        protected void SetOnBeforePushEvent(UnityEvent value)
        {
            onBeforePushEvent = value;
        }

        protected UnityEvent GetOnAfterPushEvent()
        {
            return onAfterPushEvent;
        }

        protected void SetOnAfterPushEvent(UnityEvent value)
        {
            onAfterPushEvent = value;
        }
        #endregion
    }
}