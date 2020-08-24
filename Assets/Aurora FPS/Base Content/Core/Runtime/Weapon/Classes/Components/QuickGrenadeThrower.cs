/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

namespace AuroraFPSRuntime
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(WeaponShootingSystem))]
    public class QuickGrenadeThrower : MonoBehaviour
    {
        // Base quick grenade thrower properties.
        [SerializeField] private WeaponGrenadeSystem weaponGrenadeSystem;
        [SerializeField] private float timeToThrow;
        [SerializeField] private KeyCode throwKey = KeyCode.G;
        [SerializeField] private AnimatorState throwState = "Grenade Throw";

        // Stored required components.
        private Animator animator;
        private WeaponShootingSystem weaponShootingSystem;

        // Stored required properties.
        private CoroutineObject quickThrowCoroutine;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            weaponShootingSystem = GetComponent<WeaponShootingSystem>();

            quickThrowCoroutine = new CoroutineObject(this);

            weaponGrenadeSystem?.gameObject.SetActive(true);
        }

        protected virtual void Start()
        {
            weaponGrenadeSystem?.gameObject.SetActive(false);
        }

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        protected virtual void OnEnable()
        {
            if(weaponGrenadeSystem != null)
            {
                quickThrowCoroutine.Start(QuickThrowProcessing);
            }
        }

        /// <summary>
        /// Quick grenade throw processing. 
        /// </summary>
        protected virtual IEnumerator QuickThrowProcessing()
        {
            WaitForSeconds waitForThrow = new WaitForSeconds(timeToThrow);
            while (true)
            {
                if (weaponGrenadeSystem.GetWeaponAmmoSystem().HasAmmo() && !weaponShootingSystem.IsShooting() && Input.GetKeyDown(throwKey))
                {
                    weaponShootingSystem.enabled = false;
                    PlayThrowAnimation();
                    yield return waitForThrow;
                    weaponGrenadeSystem.Throw();
                    weaponShootingSystem.enabled = true;
                }
                yield return null;
            }
        }

        /// <summary>
        /// Play throw animation from animator controller.
        /// </summary>
        protected virtual void PlayThrowAnimation()
        {
            animator.PlayInFixedTime(throwState.GetNameHash(), throwState.GetLayer(), throwState.GetFixedTime());
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        protected virtual void OnDisable()
        {
            quickThrowCoroutine.Stop();
        }

        #region [Getter / Setter]
        public WeaponGrenadeSystem GetGrenadeThrowSystem()
        {
            return weaponGrenadeSystem;
        }

        public void SetGrenadeThrowSystem(WeaponGrenadeSystem value)
        {
            weaponGrenadeSystem = value;
        }

        public float GetTimeToThrow()
        {
            return timeToThrow;
        }

        public void SetTimeToThrow(float value)
        {
            timeToThrow = value;
        }

        public KeyCode GetThrowKey()
        {
            return throwKey;
        }

        public void SetThrowKey(KeyCode value)
        {
            throwKey = value;
        }

        public AnimatorState GetThrowState()
        {
            return throwState;
        }

        public void SetThrowState(AnimatorState value)
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

        public WeaponShootingSystem GetWeaponShootingSystem()
        {
            return weaponShootingSystem;
        }

        protected void SetWeaponShootingSystem(WeaponShootingSystem value)
        {
            weaponShootingSystem = value;
        }
        #endregion
    }
}