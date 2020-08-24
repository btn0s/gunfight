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

namespace AuroraFPSRuntime
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(AudioSource))]
    public class PhysicsBullet : PhysicsShell
    {
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            OnAnyHitCallback += Push;
        }

        /// <summary>
        /// OnCollisionEnter is called when this collider/rigidbody has begun
        /// touching another rigidbody/collider.
        /// </summary>
        /// <param name="other">The Collision data associated with this collision.</param>
        protected virtual void OnCollisionEnter(Collision other)
        {
            Transform otherTransform = other.transform;
            SendDamage(otherTransform, GetShellItem().GetDamage());
            SendImpulse(otherTransform, GetShellItem().GetImpulse());
            Decal.Spawn(GetShellItem().GetDecalMapping(), other.contacts[0]);
            OnAnyHitCallback?.Invoke(otherTransform);
        }

        /// <summary>
        /// Wrapper of the pool object Push() methord to add in OnAnyHitCallback.
        /// </summary>
        protected void Push(Transform transform)
        {
            Push();
        }

        #region [Event Callback Functions]
        /// <summary>
        /// On any hit callback function.
        /// OnBulletHitCallback is called when bullet has begun touching any rigidbody/collider.
        /// </summary>
        /// <param name="Transform">The Transform data associated with this collision.</param>
        public override event Action<Transform> OnAnyHitCallback;
        #endregion
    }
}