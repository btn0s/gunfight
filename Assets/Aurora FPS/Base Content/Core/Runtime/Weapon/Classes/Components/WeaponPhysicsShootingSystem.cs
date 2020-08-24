/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using System;
using UnityEngine;
using AuroraFPSRuntime.UI;

namespace AuroraFPSRuntime
{
    [RequireComponent(typeof(WeaponAnimationSystem))]
    [RequireComponent(typeof(AudioSource))]
    public class WeaponPhysicsShootingSystem : WeaponShootingSystem
    {
        // Weapon physics shooting system properties.
        [SerializeField] private PhysicsBullet bullet;
        [SerializeField] private float impulseAmplifier = 0.5f;

        // Stored required components.
        private PoolManager poolManager;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            WeaponAnimationSystem weaponAnimationSystem = GetComponent<WeaponAnimationSystem>();

            Crosshair crosshair = GetComponent<Crosshair>();
            if (crosshair != null)
            {
                OnFireBulletCallback += bullet =>
                {
                    //bullet.OnHealthHitCallback += _ => crosshair.DisplayHitEffect();
                };
            }
            poolManager = PoolManager.Instance;
        }

        protected override void FireProcessing()
        {
            if (bullet != null && bullet.GetShellItem() != null)
            {
                for (int i = 0; i < bullet.GetShellItem().GetBallsNumber(); i++)
                {
                    GetFirePoint().localRotation = Quaternion.Euler(bullet.GetShellItem().GetRandomVarianceDirection(GetFirePoint().forward));
                    GameObject bulletInstance = poolManager.CreateOrPop(bullet, GetFirePoint().position, Quaternion.LookRotation(GetFirePoint().forward));
                    Rigidbody bulletRigidbody = bulletInstance.GetComponent<Rigidbody>();
                    bulletRigidbody.AddForce(GetFirePoint().forward * (bullet.GetShellItem().GetImpulse() + impulseAmplifier), ForceMode.Impulse);
                    PhysicsBullet physicsBullet = bulletInstance.GetComponent<PhysicsBullet>();
                    OnFireBulletCallback?.Invoke(physicsBullet);
                }
            }
#if UNITY_EDITOR
            else
            {
                Debug.LogError(string.Format("Physics Bullet or Bullet Item is null! Check gameobject [{0}]", name));
            }
#endif
        }

        #region [Event Callback Functions]
        /// <summary>
        /// On fire bullet callback event function.
        /// OnFireBulletCallback called when weapon fired and instantiating new physics bullet.
        /// </summary>
        /// <param name="PhysicsBullet">Fired bullet instance.</param>
        public event Action<PhysicsBullet> OnFireBulletCallback;
        #endregion

        #region [Getter / Setter]
        public PhysicsBullet GetBullet()
        {
            return bullet;
        }

        public void SetBullet(PhysicsBullet value)
        {
            bullet = value;
        }

        public float GetImpulseAmplifier()
        {
            return impulseAmplifier;
        }

        public void SetImpulseAmplifier(float value)
        {
            impulseAmplifier = value;
        }

        public PoolManager GetPoolManager()
        {
            return poolManager;
        }

        protected void SetPoolManager(PoolManager value)
        {
            poolManager = value;
        }
        #endregion
    }
}