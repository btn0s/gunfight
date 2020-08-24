/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;

namespace AuroraFPSRuntime
{
    public class ObjectHealth : HealthComponent
    {
        [Serializable] public class OnTakeDamageEvent : UnityEvent<int> { }
        [Serializable] public class OnWakeUpTimerEvent : UnityEvent<float> { }

        // Base object health properties.
        [SerializeField] protected int health = 100;
        [SerializeField] protected int maxHealth = 100;
        [SerializeField] protected int minHealth = 0;
        [SerializeField] private float wakeUpDelay = 0.0f;
        [SerializeField] private OnTakeDamageEvent onTakeDamageEvent;
        [SerializeField] private UnityEvent onWakeUpEvent;
        [SerializeField] private UnityEvent onBeforeWakeUpEvent;
        [SerializeField] private OnWakeUpTimerEvent onWakeUpTimerEvent;
        [SerializeField] private UnityEvent onDeathEvent;

        // Stored reqiured properties.
        private CoroutineObject<float> wakeUpCallbackDelayCoroutine;
        private bool isPreviouslyDead;

        

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            wakeUpCallbackDelayCoroutine = new CoroutineObject<float>(this);

            OnTakeDamageCallback += onTakeDamageEvent.Invoke;
            OnWakeUpCallback += onWakeUpEvent.Invoke;
            OnBeforeWakeUpCallback += onBeforeWakeUpEvent.Invoke;
            OnWakeUpTimerCallback += onWakeUpTimerEvent.Invoke;
            OnDeadCallback += onDeathEvent.Invoke;
            OnDeadCallback += () => wakeUpCallbackDelayCoroutine.Stop();
        }

        private void OnDeadCallbackHandler()
        {
            if (!IsAlive() && !isPreviouslyDead)
            {
                OnDeadCallback?.Invoke();
                isPreviouslyDead = true;
            }
        }

        private void OnWakeUpCallbackHandler()
        {
            if (IsAlive() && isPreviouslyDead)
            {
                if (wakeUpDelay > 0)
                    wakeUpCallbackDelayCoroutine.Start(WakeUpDelay, wakeUpDelay, true);
                else
                    OnWakeUpCallback?.Invoke();
                isPreviouslyDead = false;
            }
        }

        private IEnumerator WakeUpDelay(float delay)
        {
            OnBeforeWakeUpCallback?.Invoke();
            do
            {
                OnWakeUpTimerCallback?.Invoke(delay);
                delay -= Time.deltaTime;
                yield return null;
            } while (delay > 0);
            OnWakeUpCallback?.Invoke();
        }

        #region [HealthSystem Implemetation]
        /// <summary>
        /// Get current health point.
        /// </summary>
        public override int GetHealth()
        {
            return health;
        }

        /// <summary>
        /// Set current health value.
        /// </summary>
        /// <param name="value">Health value.</param>
        public virtual void SetHealth(int value)
        {
            if (AMath.InRange(value, minHealth, maxHealth))
                health = value;
            else if (value > maxHealth)
                health = maxHealth;
            else if (value < minHealth)
                health = minHealth;

            OnWakeUpCallbackHandler();
            OnDeadCallbackHandler();
        }

        /// <summary>
        /// Alive state of health object.
        /// </summary>
        /// <returns>
        /// True if health > 0.
        /// Otherwise false.
        /// </returns>
        public override bool IsAlive()
        {
            return health > 0;
        }

        /// <summary>
        /// Take damage to the health.
        /// </summary>
        /// <param name="amount">Damage amount.</param>
        public override void TakeDamage(int amount)
        {
            amount = Mathf.Abs(amount);
            SetHealth(health - amount);
            OnTakeDamageCallback?.Invoke(amount);
            OnDeadCallbackHandler();
        }
        #endregion

        #region [Event Callback Functions]
        /// <summary>
        /// Called when object take damage.
        /// </summary>
        public event Action<int> OnTakeDamageCallback;

        /// <summary>
        /// Called once when controller is die.
        /// </summary>
        public event Action OnDeadCallback;

        /// <summary>
        /// Called when controller wake up after death.
        /// </summary>
        public event Action OnWakeUpCallback;

        /// <summary>
        /// Called when wake up delay timer stated.
        /// Note: callback will be ignored if wake delay is zero.
        /// </summary>
        public event Action OnBeforeWakeUpCallback;

        /// <summary>
        /// Called every time while wake up delay timer is running.
        /// Note: callback will be ignored if wake up delay is zero.
        /// </summary>
        /// <param name="float">Remaining time until wake up.</param>
        public event Action<float> OnWakeUpTimerCallback;
        #endregion

        #region [Getter / Setter]
        public int GetMaxHealth()
        {
            return maxHealth;
        }

        public void SetMaxHealth(int value)
        {
            maxHealth = value;
        }

        public int GetMinHealth()
        {
            return minHealth;
        }

        public void SetMinHealth(int value)
        {
            minHealth = value >= 0 ? value : 0;
        }

        public float GetWakeUpDelay()
        {
            return wakeUpDelay;
        }

        public void SetWakeUpDelay(float value)
        {
            wakeUpDelay = value;
        }

        protected OnTakeDamageEvent GetTakeDamageEvent()
        {
            return onTakeDamageEvent;
        }

        protected void SetTakeDamageEvent(OnTakeDamageEvent value)
        {
            onTakeDamageEvent = value;
        }

        public UnityEvent GetOnWakeUpEvent()
        {
            return onWakeUpEvent;
        }

        public void SetOnWakeUpEvent(UnityEvent value)
        {
            onWakeUpEvent = value;
        }

        public UnityEvent GetOnBeforeWakeUpEvent()
        {
            return onBeforeWakeUpEvent;
        }

        public void SetOnBeforeWakeUpEvent(UnityEvent value)
        {
            onBeforeWakeUpEvent = value;
        }

        public OnWakeUpTimerEvent GetOnWakeUpTimerEvent1()
        {
            return onWakeUpTimerEvent;
        }

        public void SetOnWakeUpTimerEvent1(OnWakeUpTimerEvent value)
        {
            onWakeUpTimerEvent = value;
        }

        public UnityEvent GetOnDeathEvent()
        {
            return onDeathEvent;
        }

        public void SetOnDeathEvent(UnityEvent value)
        {
            onDeathEvent = value;
        }
        #endregion
    }
}