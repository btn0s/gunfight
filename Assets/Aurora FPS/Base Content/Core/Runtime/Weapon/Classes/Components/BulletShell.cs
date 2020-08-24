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
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(AudioSource))]
    public class BulletShell : PoolObject
    {
        [SerializeField] private AudioClip sound;
        [SerializeField] private float relativeVelocity = 2.0f;

        private AudioSource audioSource;
        private new Rigidbody rigidbody;


        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            Collider shellCollider = GetComponent<Collider>();

            GameObject playerController = GameObject.FindGameObjectWithTag(TNC.Player);
            Collider playerCollider = playerController.GetComponent<Collider>();

            Physics.IgnoreCollision(shellCollider, playerCollider, true);
            audioSource = GetComponent<AudioSource>();
            rigidbody = GetComponent<Rigidbody>();
        }

        /// <summary>
        /// Throw this bullet shell.
        /// </summary>
        /// <param name="direction">Throw direction.</param>
        /// <param name="force">Throw force.</param>
        public virtual void Throw(Vector3 direction, float force)
        {
            rigidbody.AddForce(direction * force, ForceMode.Impulse);
            rigidbody.AddTorque(direction * force, ForceMode.Impulse);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.relativeVelocity.sqrMagnitude >= relativeVelocity)
            {
                audioSource.PlayOneShot(sound);
            }
        }

        #region [Getter / Setter]
        public AudioClip GetSound()
        {
            return sound;
        }

        public void SetSound(AudioClip value)
        {
            sound = value;
        }

        public float GetRelativeVelocity()
        {
            return relativeVelocity;
        }

        public void SetRelativeVelocity(float value)
        {
            relativeVelocity = value;
        }
        #endregion
    }
}