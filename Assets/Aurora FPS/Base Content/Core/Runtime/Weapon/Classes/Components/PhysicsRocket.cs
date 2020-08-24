/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using UnityEngine;

namespace AuroraFPSRuntime
{
    public class PhysicsRocket : PhysicsBullet
    {
        [SerializeField] private float damageRadius = 5;
        [SerializeField] private int overlapDamage = 5;
        [SerializeField] private float overlapImpulse = 5.0f;
        [SerializeField] private ParticleSystem explosionEffect;
        [SerializeField] private AudioClip explosionSound;
        [SerializeField] private LayerMask cullingLayer = Physics.AllLayers;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            OnAnyHitCallback -= Push;

            OnAnyHitCallback += _ => GetShellRigidbody().isKinematic = true;
            OnAnyHitCallback += _ => GetShellRigidbody().velocity = Vector3.zero;

            OnAnyHitCallback += _ =>
            {
                explosionEffect.gameObject.SetActive(true);
                explosionEffect.Play();
            };

            OnBeforePushCallback += () => explosionEffect.gameObject.SetActive(false);
            OnBeforePushCallback += () => GetShellRigidbody().isKinematic = false;
        }

        /// <summary>
        /// OnCollisionEnter is called when this collider/rigidbody has begun
        /// touching another rigidbody/collider.
        /// </summary>
        /// <param name="other">The Collision data associated with this collision.</param>
        protected override void OnCollisionEnter(Collision other)
        {
            GetShellRigidbody().velocity = Vector3.zero;

            Transform colliderTransform = other.transform;
            Collider[] overlapColliders = Physics.OverlapSphere(colliderTransform.position, damageRadius, cullingLayer, QueryTriggerInteraction.Ignore);
            if (overlapColliders != null)
            {
                for (int i = 0; i < overlapColliders.Length; i++)
                {
                    Transform overlapTransform = overlapColliders[i].transform;
                    if (colliderTransform != overlapTransform)
                    {
                        SendDamage(overlapTransform, overlapDamage);
                        SendImpulse(overlapTransform, overlapImpulse);
                    }
                }
            }
            base.OnCollisionEnter(other);
        }

        #region [Getter / Setter]
        public float GetDamageRadius()
        {
            return damageRadius;
        }

        public void SetDamageRadius(float value)
        {
            damageRadius = value;
        }

        public int GetOverlapDamage()
        {
            return overlapDamage;
        }

        public void SetOverlapDamage(int value)
        {
            overlapDamage = value;
        }

        public float GetOverlapImpulse()
        {
            return overlapImpulse;
        }

        public void SetOverlapImpulse(float value)
        {
            overlapImpulse = value;
        }

        public ParticleSystem GetExplosionEffect()
        {
            return explosionEffect;
        }

        public void SetExplosionEffect(ParticleSystem value)
        {
            explosionEffect = value;
        }

        public AudioClip GetExplosionSound()
        {
            return explosionSound;
        }

        public void SetExplosionSound(AudioClip value)
        {
            explosionSound = value;
        }

        public LayerMask GetCullingLayer()
        {
            return cullingLayer;
        }

        public void SetCullingLayer(LayerMask value)
        {
            cullingLayer = value;
        }
        #endregion
    }
}