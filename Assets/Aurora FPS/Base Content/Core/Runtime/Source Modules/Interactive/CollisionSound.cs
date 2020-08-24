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
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(AudioSource))]
    public class CollisionSound : MonoBehaviour
    {
        [System.Serializable]
        public struct KickSound
        {
            public AudioClip sound;
            public float velocity;
        }

        [SerializeField] private KickSound[] kickSounds;

        private AudioSource audioSource;


        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (kickSounds != null && kickSounds.Length > 0)
            {
                float relativeVelocity = collision.relativeVelocity.magnitude;
                AudioClip maxVelocitySound = null;
                for (int i = 0; i < kickSounds.Length; i++)
                {
                    KickSound kickSound = kickSounds[i];
                    if (relativeVelocity > kickSound.velocity)
                        maxVelocitySound = kickSound.sound;
                }

                if (maxVelocitySound != null)
                {
                    audioSource.PlayOneShot(maxVelocitySound);
                }
            }
        }

        #region [Getter / Setter]
        public KickSound[] GetKickSounds()
        {
            return kickSounds;
        }

        public void SetKickSounds(KickSound[] value)
        {
            kickSounds = value;
        }
        #endregion
    }
}