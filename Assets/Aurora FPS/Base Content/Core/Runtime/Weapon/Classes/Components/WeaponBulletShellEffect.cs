/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using UnityEngine;

namespace AuroraFPSRuntime
{
    [RequireComponent(typeof(WeaponShootingSystem))]
    public class WeaponBulletShellEffect : MonoBehaviour
    {
        public enum CallEvent
        {
            Fire,
            Custom
        }

        [SerializeField] private CallEvent callEvent = CallEvent.Fire;
        [SerializeField] private BulletShell bulletShell;
        [SerializeField] private Transform throwPoint;
        [SerializeField] private float throwForce = 1.5f;


        protected virtual void Awake()
        {
            if (callEvent == CallEvent.Fire)
            {
                WeaponShootingSystem weaponShootingSystem = GetComponent<WeaponShootingSystem>();
                weaponShootingSystem.OnFireCallback += ThrowShell;
            }
        }

        public virtual void ThrowShell()
        {
            GameObject bulletShellReference = PoolManager.Instance.CreateOrPop(bulletShell, throwPoint.position, Quaternion.LookRotation(GetRandomizedThrowDirection()) * Quaternion.Euler(180, 90, 0));
            BulletShell bulletShellComponent = bulletShellReference.GetComponent<BulletShell>();
            bulletShellComponent.Throw(throwPoint.forward, throwForce);
        }

        public Vector3 GetRandomizedThrowDirection()
        {
            Vector3 direction = throwPoint.forward;
            direction.x += Random.Range(-0.5f, 0.5f);
            direction.y += Random.Range(-0.5f, 0.5f);
            return direction;
        }

        #region [Getter / Setter]
        public CallEvent GetCallEvent()
        {
            return callEvent;
        }

        public void SetCallEvent(CallEvent value)
        {
            callEvent = value;
        }

        public BulletShell GetBulletShell()
        {
            return bulletShell;
        }

        public void SetBulletShell(BulletShell value)
        {
            bulletShell = value;
        }

        public Transform GetThrowPoint()
        {
            return throwPoint;
        }

        public void SetThrowPoint(Transform value)
        {
            throwPoint = value;
        }

        public float GetThrowForce()
        {
            return throwForce;
        }

        public void SetThrowForce(float value)
        {
            throwForce = value;
        }
        #endregion
    }
}
