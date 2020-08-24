/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using AuroraFPSRuntime.UI;

namespace AuroraFPSRuntime
{
    [RequireComponent(typeof(WeaponReloadSystem))]
    [RequireComponent(typeof(WeaponAnimationSystem))]
    [RequireComponent(typeof(AudioSource))]
    public class WeaponRayShootingSystem : WeaponShootingSystem
    {
        #region [Serializable Events Implementation]
        [Serializable] public class OnFireHitEvent : UnityEvent<RaycastHit> { }
        [Serializable] public class OnHealthHitEvent : UnityEvent<Transform> { }
        [Serializable] public class OnHealthKillEvent : UnityEvent<Transform> { }
        #endregion

        // Weapon ray shooting system properties.
        [SerializeField] private BulletItem bulletItem;
        [SerializeField] private float fireRange = 500.0f;
        [SerializeField] private float impulseAmplifier = 0.5f;
        [SerializeField] private LayerMask cullingLayer = Physics.AllLayers;

        [SerializeField] private OnFireHitEvent onFireHitEvent;
        [SerializeField] private OnHealthHitEvent onHealthHitEvent;
        [SerializeField] private OnHealthKillEvent onHealthKillEvent;

        // Stored required properties.
        private List<int> markedHealthObjects;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            WeaponAnimationSystem weaponAnimationSystem = GetComponent<WeaponAnimationSystem>();

            // If Crosshair component contained in the weapon, then register the necessary callback.
            Crosshair crosshair = GetComponent<Crosshair>();
            if (crosshair != null)
            {
                OnFireCallback += crosshair.ApplyFireSpread;
                OnHealthHitCallback += _ => crosshair.ShowHitEffect();
                OnHealthKillCallback += _ => crosshair.ShowKillEffect();
            }

            OnFireHitCallback += onFireHitEvent.Invoke;
            OnHealthHitCallback += onHealthHitEvent.Invoke;
            OnHealthKillCallback += onHealthKillEvent.Invoke;

            markedHealthObjects = new List<int>();
        }

        /// <summary>
        /// Shooting type processing.
        /// </summary>
        protected override void FireProcessing()
        {
            for (int i = 0; i < bulletItem.GetBallsNumber(); i++)
            {
                GetFirePoint().localRotation = Quaternion.Euler(bulletItem.GetRandomVarianceDirection(GetFirePoint().localEulerAngles));
                if (Physics.Raycast(GetFirePoint().position, GetFirePoint().forward, out RaycastHit hitInfo, fireRange, cullingLayer, QueryTriggerInteraction.Ignore))
                {
                    Transform hitTransform = hitInfo.transform;
                    Decal.Spawn(bulletItem.GetDecalMapping(), hitInfo);
                    SendDamage(hitTransform);
                    AddImpulseForce(hitTransform);
                    OnFireHitCallback?.Invoke(hitInfo);
                }
                ResetFirePointToOriginalAngle();
            }
        }

        /// <summary>
        /// Send damage to transform containing IHealth component.
        /// </summary>
        private void SendDamage(Transform other)
        {
            if (other == null)
            {
                return;
            }

            IHealth health = other.GetComponent<IHealth>();
            if (health != null)
            {
                health.TakeDamage(bulletItem.GetDamage());
                if (health.IsAlive())
                {
                    TryRemoveMarkedHealth(other);
                    OnHealthHitCallback?.Invoke(other);
                }
                else if (!markedHealthObjects.Contains(other.root.GetInstanceID()))
                {
                    OnHealthKillCallback?.Invoke(other);
                    markedHealthObjects.Add(other.root.GetInstanceID());
                }
            }
        }

        /// <summary>
        /// Add impulse force to rigidbody transform.
        /// </summary>
        private void AddImpulseForce(Transform other)
        {
            if (other == null)
            {
                return;
            }

            Rigidbody otherRigidbody = other.GetComponent<Rigidbody>();
            if (otherRigidbody != null)
            {
                otherRigidbody.AddForce(GetFirePoint().forward * (bulletItem.GetImpulse() + impulseAmplifier), ForceMode.Impulse);
            }
        }

        /// <summary>
        /// Try to find and remove already marked specific health system.
        /// </summary>
        /// <param name="health">Target health transform to remove.</param>
        public void TryRemoveMarkedHealth(Transform health)
        {
            markedHealthObjects.Remove(health.root.GetInstanceID());
        }

        #region [Event Callback Functions]
        /// <summary>
        /// On fire hit callback function.
        /// OnFireHitCallback called when ray fire hitted on any collider.
        /// </summary>
        /// <param name="RaycastHit">Fire raycast hit info.</param>
        public event Action<RaycastHit> OnFireHitCallback;

        /// <summary>
        /// On health hit callback function.
        /// OnHealthHitCallback called when raycast hitted on any alive object with component implemented from HealthComponent abstract class.
        /// </summary>
        /// <param name="Transform">The Transform data associated with health.</param>
        public event Action<Transform> OnHealthHitCallback;

        /// <summary>
        /// On health kill callback function.
        /// OnHealthKillCallback called every time when raycast hitted on any object with component implemented from HealthComponent abstract class and kills it.
        /// </summary>
        /// <param name="Transform">The Transform data associated with health.</param>
        public event Action<Transform> OnHealthKillCallback;
        #endregion

        #region [Getter / Setter]

        public BulletItem GetBulletItem()
        {
            return bulletItem;
        }

        public void SetBulletItem(BulletItem value)
        {
            bulletItem = value;
        }

        public float GetFireRange()
        {
            return fireRange;
        }

        public void SetFireRange(float value)
        {
            fireRange = value;
        }

        public float GetImpulseAmplifier()
        {
            return impulseAmplifier;
        }

        public void SetImpulseAmplifier(float value)
        {
            impulseAmplifier = value;
        }

        public LayerMask GetCullingLayer()
        {
            return cullingLayer;
        }

        public void SetCullingLayer(LayerMask value)
        {
            cullingLayer = value;
        }

        protected OnFireHitEvent GetOnFireHitEvent()
        {
            return onFireHitEvent;
        }

        protected void SetOnFireHitEvent(OnFireHitEvent value)
        {
            onFireHitEvent = value;
        }

        protected OnHealthHitEvent GetOnHealthHitEvent()
        {
            return onHealthHitEvent;
        }

        protected void SetOnHealthHitEvent(OnHealthHitEvent value)
        {
            onHealthHitEvent = value;
        }

        protected OnHealthKillEvent GetOnHealthKillEvent()
        {
            return onHealthKillEvent;
        }

        protected void SetOnHealthKillEvent(OnHealthKillEvent value)
        {
            onHealthKillEvent = value;
        }
        #endregion
    }
}