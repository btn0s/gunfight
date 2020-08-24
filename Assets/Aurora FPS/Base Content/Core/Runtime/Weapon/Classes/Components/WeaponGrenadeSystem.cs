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
using IEnumerator = System.Collections.IEnumerator;

namespace AuroraFPSRuntime
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(WeaponAmmoSystem))]
    public class WeaponGrenadeSystem : MonoBehaviour
    {
        // Base grenade system properties.
        [SerializeField] private PhysicsGrenade grenade;
        [SerializeField] private Transform throwPoint;
        [SerializeField] private float force = 20.0f;
        [SerializeField] private float throwRate = 2.0f;
        [SerializeField] private AnimatorValue throwState = "Throw";
        [SerializeField] private float timeToThrow = 1.0f;

        // Stored required components.
        private Animator animator;
        private WeaponAmmoSystem weaponAmmoSystem;
        private PoolManager poolManager;

        // Stored required properties.
        private CoroutineObject grenadeCoroutine;


        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            weaponAmmoSystem = GetComponent<WeaponAmmoSystem>();
            grenadeCoroutine = new CoroutineObject(this);

            poolManager = PoolManager.Instance;
        }

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        protected virtual void OnEnable()
        {
            grenadeCoroutine.Start(ThrowProcessing);
        }

        protected virtual IEnumerator ThrowProcessing()
        {
            WaitForSeconds waitTimeToThrow = new WaitForSeconds(timeToThrow);
            WaitForSeconds waitForRate = new WaitForSeconds(throwRate);
            while (true)
            {
                if (AInput.GetButtonDown(INC.Attack))
                {
                    PlayThrowAnimation();
                    yield return waitTimeToThrow;
                    Throw();
                    yield return waitForRate;
                }
                yield return null;
            }
        }

        /// <summary>
        /// Throw grenade.
        /// </summary>
        public virtual void Throw()
        {
            if (weaponAmmoSystem.HasAmmo())
            {

                if (poolManager == null)
                {
                    poolManager = PoolManager.Instance;
                }

                GameObject grenadeObject = poolManager.CreateOrPop(grenade, throwPoint.position, Quaternion.LookRotation(throwPoint.forward));
                PhysicsGrenade grenadeInstance = grenadeObject.GetComponent<PhysicsGrenade>();
                Rigidbody grenadeRigidbody = grenadeObject.GetComponent<Rigidbody>();
                if (grenadeRigidbody != null)
                {
                    grenadeRigidbody.AddForce(throwPoint.forward * force, ForceMode.Impulse);
                }
                weaponAmmoSystem.AmmoSubtraction();
                OnThrowCallback?.Invoke(grenadeInstance);
            }
        }

        /// <summary>
        /// Play throw animation from animator controller.
        /// </summary>
        private void PlayThrowAnimation()
        {
            animator.PlayInFixedTime(throwState.GetNameHash(), 0, 0.1f);
        }


        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        protected virtual void OnDisable()
        {
            grenadeCoroutine.Stop();
        }

        #region [Event Callback Functions]
        /// <summary>
        /// On throw callback event function.
        /// OnThrowCallback called when throw grenade.
        /// </summary>
        /// <param name="Grenade">Throwed grenade instance.</param>
        public event Action<PhysicsGrenade> OnThrowCallback;

        #endregion

        #region [Getter / Setter]
        public PhysicsGrenade GetGrenade()
        {
            return grenade;
        }

        public void SetGrenade(PhysicsGrenade value)
        {
            grenade = value;
        }

        public Transform GetThrowPoint()
        {
            return throwPoint;
        }

        public void SetThrowPoint(Transform value)
        {
            throwPoint = value;
        }

        public float GetForce()
        {
            return force;
        }

        public void SetForce(float value)
        {
            force = value;
        }


        public float GetThrowRate()
        {
            return throwRate;
        }

        public void SetThrowRate(float value)
        {
            throwRate = value;
        }

        public float GetTimeToThrow()
        {
            return timeToThrow;
        }

        public void SetTimeToThrow(float value)
        {
            timeToThrow = value;
        }

        public AnimatorValue GetThrowState()
        {
            return throwState;
        }

        public void SetThrowState(AnimatorValue value)
        {
            throwState = value;
        }

        public Animator GetAnimator()
        {
            return animator;
        }

        protected void SetAnimator(Animator value)
        {
            animator = value;
        }

        public WeaponAmmoSystem GetWeaponAmmoSystem()
        {
            return weaponAmmoSystem;
        }

        public void SetWeaponAmmoSystem(WeaponAmmoSystem value)
        {
            weaponAmmoSystem = value;
        }

        public PoolManager GetPoolManagerInstance()
        {
            return poolManager;
        }

        protected void SetPoolManagerInstance(PoolManager value)
        {
            poolManager = value;
        }
        #endregion
    }
}