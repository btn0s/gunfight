/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using System;
using System.Collections;
using UnityEngine;

namespace AuroraFPSRuntime
{
    public class PhysicsGrenade : PhysicsShell
    {
        // Base grenade properties.
        [SerializeField] private GameObject grenadeObject;
        [SerializeField] private float radius = 1.25f;
        [SerializeField] private ParticleSystem explosionEffect;
        [SerializeField] private AudioClip explosionSound;
        [SerializeField] private ExplosionMapping explosionProperties;
        [SerializeField] private LayerMask cullingLayer;

        /// <summary>
        /// Explosion is called when the delay time has elapsed.
        /// </summary>
        protected virtual void Explosion()
        {
            Vector3 position = transform.position;
            Collider[] overlapColliders = Physics.OverlapSphere(position, radius, cullingLayer, QueryTriggerInteraction.Ignore);

            for (int i = 0; i < overlapColliders.Length; i++)
            {
                Transform overlapTransform = overlapColliders[i].transform;

                float distance = Vector3.Distance(position, overlapTransform.position);
                ExplosionProperty projectileExplosion = GetProjectileExplosion(distance);
                SendDamage(overlapTransform, projectileExplosion.GetDamage());
                SendImpulse(overlapTransform, projectileExplosion.GetImpulse(), position, radius, projectileExplosion.GetUpwardsModifier());

                if (overlapTransform.CompareTag(TNC.Player) && projectileExplosion.GetShake() != null)
                {
                    CameraShake.Instance.AddShake(projectileExplosion.GetShake());
                }
                OnAnyHitCallback?.Invoke(overlapTransform);
            }
            explosionEffect.gameObject.SetActive(true);
            explosionEffect?.Play();
            GetAudioSource().PlayOneShot(explosionSound);
        }

        /// <summary>
        /// Trying to send physics explosion impulse force to transform.
        /// </summary>
        protected virtual void SendImpulse(Transform other, float impulse, Vector3 position, float radius, float upwardsModifier)
        {
            Rigidbody rigidbody = other.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.AddExplosionForce(impulse, position, radius, upwardsModifier);
            }
        }

        /// <summary>
        /// Delayed pool object push coroutine.
        /// Started when pool object being active and disable object after delayed time.
        /// The coroutine can be stopped if the object being disabled before the delayed time expires.
        /// 
        /// Implement this method to make custom delayed push coroutine.
        /// </summary>
        /// <param name="time">Delayed time to push pool object in pool.</param>
        protected override IEnumerator DelayedPush(float time)
        {
            yield return new WaitForSeconds(time);
            grenadeObject.SetActive(false);
            Explosion();
            GetShellRigidbody().velocity = Vector3.zero;
            float effectDelay = Mathf.Max(explosionEffect.main.duration, explosionSound.length, GetAudioSource().clip != null ? GetAudioSource().clip.length : 0);
            yield return new WaitForSeconds(effectDelay);
            gameObject.SetActive(false);
            grenadeObject.SetActive(true);
            Push();
        }

        /// <summary>
        /// Get ExplosionProperty struct by distance.
        /// </summary>
        protected ExplosionProperty GetProjectileExplosion(float distance)
        {
            for (int i = 0, length = explosionProperties.GetMappingLength(); i < length; i++)
            {
                ExplosionProperty projectileExplosion = explosionProperties.GetMappingValue(i);
                if (AMath.InRange(distance, projectileExplosion.GetMinDistance(), projectileExplosion.GetMaxDistance()))
                {
                    return projectileExplosion;
                }
            }
            return ExplosionProperty.none;
        }

        #region [Event Callback Functions]
        /// <summary>
        /// On any hit callback function.
        /// OnBulletHitCallback is called when bullet has begun touching any rigidbody/collider.
        /// </summary>
        /// <param name="Transform">The Transform data associated with this collision.</param>
        public override event Action<Transform> OnAnyHitCallback;

        #endregion

        #region [Getter / Setter]
        public GameObject GetGrenadeObject()
        {
            return grenadeObject;
        }

        public void SetGrenadeObject(GameObject value)
        {
            grenadeObject = value;
        }

        public float GetRadius()
        {
            return radius;
        }

        public void SetRadius(float value)
        {
            radius = value;
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

        public ExplosionMapping GetExplosionProperties()
        {
            return explosionProperties;
        }

        public void SetExplosionProperties(ExplosionMapping value)
        {
            explosionProperties = value;
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