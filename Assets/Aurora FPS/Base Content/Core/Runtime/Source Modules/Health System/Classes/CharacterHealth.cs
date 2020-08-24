/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using System.Collections;
using UnityEngine;

namespace AuroraFPSRuntime
{
    [RequireComponent(typeof(AudioSource))]
    public class CharacterHealth : ObjectHealth
    {
        // Base character health properties.
        [SerializeField] private VelocityDamage[] velocityDamageProperties;
        [SerializeField] private HealthSoundEffects healthSoundEffects;
        [SerializeField] private RegenerationProperties regenerationSettings;
        [SerializeField] private bool useRegeneration = true;

        // Stored required components.
        private AudioSource audioSource;

        // Stored required properties.
        private CoroutineObject<RegenerationProperties> regenerationCoroutine;
        private float storedHeartbeatRate;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            audioSource = GetComponent<AudioSource>();

            regenerationCoroutine = new CoroutineObject<RegenerationProperties>(this);

            OnTakeDamageCallback += _ => { if (IsAlive()) PlayTakeDamageSound(); };
            OnTakeDamageCallback += _ => StartRegeneration();
            OnDeadCallback += PlayDeathSound;

            InitializeHealthHitAreas();
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void Update()
        {
            HeartbeatProcessing();
        }

        /// <summary>
        /// OnCollisionEnter is called when this collider/rigidbody has begun
        /// touching another rigidbody/collider.
        /// </summary>
        /// <param name="other">The Collision data associated with this collision.</param>
        protected virtual void OnCollisionEnter(Collision other)
        {
            VelocityDamageHandler(other.relativeVelocity.magnitude);
        }

        /// <summary>
        /// Start health regenegation.
        /// </summary>
        public virtual void StartRegeneration()
        {
            if (IsAlive())
            {
                regenerationCoroutine.Start(RegenerationProcessing, regenerationSettings, true);
            }
        }

        /// <summary>
        /// Stop started regeneration process.
        /// </summary>
        public virtual void StopRegeneration()
        {
            regenerationCoroutine.Stop();
        }

        /// <summary>
        /// Health regeneration processing.
        /// </summary>
        protected virtual IEnumerator RegenerationProcessing(RegenerationProperties property)
        {
            WaitForSeconds rateDelay = new WaitForSeconds(property.GetRate());
            yield return new WaitForSeconds(property.GetDelay());
            while (true)
            {
                if (!IsAlive())
                {
                    yield break;
                }

                if ((health + property.GetValue()) < GetMaxHealth())
                {
                    health += property.GetValue();
                }
                else
                {
                    health = maxHealth;
                    yield break;
                }
                yield return rateDelay;
            }
        }

        /// <summary>
        /// Controller velocity damage handler.
        /// </summary>
        protected virtual void VelocityDamageHandler(float velocity)
        {
            if (enabled && velocity > 0)
            {
                for (int i = 0; i < velocityDamageProperties.Length; i++)
                {
                    VelocityDamage property = velocityDamageProperties[i];
                    if (AMath.InRange(velocity, property.GetMinVelocity(), property.GetMaxVelocity()))
                    {
                        TakeDamage(property.GetDamage());
                        PlayVelocityDamageSound();
                    }
                }
            }
        }

        /// <summary>
        /// Initialize all health hit areas, which found in child objects.
        /// </summary>
        protected virtual void InitializeHealthHitAreas()
        {
            HealthHitArea[] healthHitAreas = GetComponentsInChildren<HealthHitArea>();
            for (int i = 0; i < healthHitAreas.Length; i++)
            {
                healthHitAreas[i].Initialize(this);
            }
        }

        public virtual void HeartbeatProcessing()
        {
            if (IsAlive() && healthSoundEffects.GetHeartbeatSound() != null && health <= healthSoundEffects.GetHeartbeatStartFrom() && Time.time - storedHeartbeatRate >= healthSoundEffects.GetHeartbeatRate())
            {
                audioSource.PlayOneShot(healthSoundEffects.GetHeartbeatSound());
                storedHeartbeatRate = Time.time;
            }
        }

        public virtual void PlayTakeDamageSound()
        {
            if (healthSoundEffects.GetTakeDamageSound() != null)
            {
                audioSource.PlayOneShot(healthSoundEffects.GetTakeDamageSound());
            }
        }

        public virtual void PlayVelocityDamageSound()
        {
            if (healthSoundEffects.GetVelocityDamageSound())
            {
                audioSource.PlayOneShot(healthSoundEffects.GetVelocityDamageSound());
            }
        }

        public virtual void PlayDeathSound()
        {
            if (healthSoundEffects.GetDeathSound() != null)
            {
                audioSource.PlayOneShot(healthSoundEffects.GetDeathSound());
            }
        }

        #region [Getter / Setter]
        public VelocityDamage[] GetVelocityDamageProperties()
        {
            return velocityDamageProperties;
        }

        public void SetVelocityDamageProperties(VelocityDamage[] value)
        {
            velocityDamageProperties = value;
        }

        public RegenerationProperties GetRegenerationProperties()
        {
            return regenerationSettings;
        }

        public void SetRegenerationProperties(RegenerationProperties value)
        {
            regenerationSettings = value;
        }

        public bool UseRegeneration()
        {
            return useRegeneration;
        }

        public void UseRegeneration(bool value)
        {
            useRegeneration = value;
        }

        public HealthSoundEffects GetHealthSoundEffects()
        {
            return healthSoundEffects;
        }

        public void SetHealthSoundEffects(HealthSoundEffects value)
        {
            healthSoundEffects = value;
        }
        #endregion
    }
}