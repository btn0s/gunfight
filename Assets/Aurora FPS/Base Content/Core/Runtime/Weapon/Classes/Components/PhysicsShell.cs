/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using System;
using UnityEngine;
using UnityEngine.Events;

namespace AuroraFPSRuntime
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(AudioSource))]
    public abstract class PhysicsShell : PoolObject
    {
        // Base physics shell properties.
        [SerializeField] private BulletItem shellItem;

        // Physics shell events.
        [SerializeField] private UnityEvent onAnyHitEvent;
        [SerializeField] private UnityEvent onHealthHitEvent;

        // Stored required components.
        private Rigidbody shellRigidbody;
        private Collider shellCollider;
        private AudioSource audioSource;


        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            shellRigidbody = GetComponent<Rigidbody>();
            shellCollider = GetComponent<Collider>();
            audioSource = GetComponent<AudioSource>();

            OnBeforePushCallback += () => shellRigidbody.velocity = Vector3.zero;

            OnAnyHitCallback += _ => onAnyHitEvent?.Invoke();
            OnHealthHitCallback += _ => onHealthHitEvent?.Invoke();
        }

        /// <summary>
        /// Trying to send damage to transform.
        /// </summary>
        protected virtual void SendDamage(Transform other, int damage)
        {
            HealthComponent health = other?.GetComponent<HealthComponent>();
            if (health != null)
            {
                health.TakeDamage(damage);
                OnHealthHitCallback?.Invoke(other);
            }
        }

        /// <summary>
        /// Trying to send physics impulse force to transform.
        /// </summary>
        protected virtual void SendImpulse(Transform other, float impulse)
        {
            Rigidbody otherRigidbody = other?.GetComponent<Rigidbody>();
            if (otherRigidbody != null)
            {
                otherRigidbody.AddForce(transform.forward * impulse, ForceMode.Impulse);
            }
        }


        #region [Event Callback Functions]
        /// <summary>
        /// On any hit callback function.
        /// OnBulletHitCallback is called when bullet has begun touching any rigidbody/collider.
        /// </summary>
        /// <param name="Transform">The Transform data associated with this collision.</param>
        public abstract event Action<Transform> OnAnyHitCallback;

        /// <summary>
        /// On health hit callback function.
        /// OnHealthHitCallback is called when bullet has begun touching any component implemented IHealth interface.
        /// </summary>
        /// <param name="Transform">The Transform data associated with health.</param>
        public event Action<Transform> OnHealthHitCallback;
        #endregion
        
        #region [Getter / Setter]
        public BulletItem GetShellItem()
        {
            return shellItem;
        }

        public void SetShellItem(BulletItem value)
        {
            shellItem = value;
        }

        public Rigidbody GetShellRigidbody()
        {
            return shellRigidbody;
        }

        public void SetShellRigidbody(Rigidbody value)
        {
            shellRigidbody = value;
        }

        public Collider GetShellCollider()
        {
            return shellCollider;
        }

        public void SetShellCollider(Collider value)
        {
            shellCollider = value;
        }

        public AudioSource GetAudioSource()
        {
            return audioSource;
        }

        public void SetAudioSource(AudioSource value)
        {
            audioSource = value;
        }

        protected UnityEvent OnAnyHitEvent()
        {
            return onAnyHitEvent;
        }

        protected void OnAnyHitEvent(UnityEvent value)
        {
            onAnyHitEvent = value;
        }

        protected UnityEvent OnHealthHitEvent()
        {
            return onHealthHitEvent;
        }

        protected void OnHealthHitEvent(UnityEvent value)
        {
            onHealthHitEvent = value;
        }
        #endregion
    }
}