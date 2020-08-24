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
    [RequireComponent(typeof(AudioSource))]
    public abstract partial class FootstepSoundSystem : MonoBehaviour
    {
        // Base footstep properties.
        [SerializeField] protected PlaySoundType playSoundType = PlaySoundType.Sequential;
        [SerializeField] protected FootstepMapping footstepMapping;
        [SerializeField] protected FootstepInterval[] footStepIntervals = FootstepInterval.Default;
        [SerializeField] protected float range;
        [SerializeField] protected LayerMask groundLayer = Physics.DefaultRaycastLayers;

        // Stored require components.
        private AudioSource audioSource;
        private TerrainManager terrainManager;

        // Stored require properties.
        private float storedTime;
        private int soundIndex = -1;
        private bool previousGrounded;
        private bool ignoreNextLand;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            terrainManager = TerrainManager.Instance;
            previousGrounded = true;
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void Update()
        {
            if (footstepMapping != null)
            {
                StepSoundProcessing();
                JumpSoundProcessing();
                LandSoundProcessing();
            }
        }

        /// <summary>
        /// Step sound processing.
        /// </summary>
        protected virtual void StepSoundProcessing()
        {
            if (IsGrounded())
            {
                UpdateStoredTime();
                if (RateTimeElapsed())
                {
                    FootstepSounds property = GetFootstepProperty();
                    if (property != FootstepSounds.Empty)
                    {
                        AudioClip clip = null;
                        switch (playSoundType)
                        {
                            case PlaySoundType.Sequential:
                                soundIndex = soundIndex < property.GetStepSoundsLength() - 1 ? soundIndex + 1 : 0;
                                clip = property.GetStepSound(soundIndex);
                                break;
                            case PlaySoundType.Random:
                                clip = property.GetRandomStepSound();
                                break;
                        }
                        ResetStoredTime();
                        PlaySound(clip);
                    }
                }
            }
            else
            {
                ResetStoredTime();
            }
        }

        /// <summary>
        /// Jump sound processing.
        /// </summary>
        protected virtual void JumpSoundProcessing()
        {
            if (IsJumped())
            {
                FootstepSounds property = GetFootstepProperty();
                AudioClip clip = property.GetRandomJumpSound();
                PlaySound(clip);
            }
        }

        /// <summary>
        /// Land sound processing.
        /// </summary>
        protected virtual void LandSoundProcessing()
        {
            if (IsGrounded() && !previousGrounded)
            {
                if (!ignoreNextLand)
                {
                    FootstepSounds property = GetFootstepProperty();
                    AudioClip clip = property.GetRandomLandSound();
                    PlaySound(clip);
                }
                ignoreNextLand = false;
            }
            previousGrounded = IsGrounded();
        }

        /// <summary>
        /// Processing interval by controller velocity.
        /// </summary>
        protected virtual float GetCurrentRate()
        {
            float sqrMagnitude = GetVelocity().sqrMagnitude;
            for (int i = 0, length = footStepIntervals.Length; i < length; i++)
            {
                FootstepInterval property = footStepIntervals[i];
                if (AMath.InRange(sqrMagnitude, property.GetMinVelocity(), property.GetMaxVelocity()))
                {
                    return property.GetRate();
                }
            }
            return 0;
        }

        /// <summary>
        /// Return true if rate time is elapsed
        /// </summary>
        public bool RateTimeElapsed()
        {
            float rate = GetCurrentRate();
            return storedTime > 0 && rate > 0 && Time.time - storedTime >= rate;
        }

        /// <summary>
        /// Ignore next land sound when controller being ground.
        /// </summary>
        public void IgnoreNextLand()
        {
            ignoreNextLand = true;
        }

        /// <summary>
        /// Controller is jumped.
        /// </summary>
        public abstract bool IsJumped();

        /// <summary>
        /// Controller is grounded.
        /// </summary>
        public abstract bool IsGrounded();

        /// <summary>
        /// Controller velocity.
        /// </summary>
        public abstract Vector3 GetVelocity();

        /// <summary>
        /// Get footstep property by current position surface.
        /// </summary>
        public FootstepSounds GetFootstepProperty()
        {
            Vector3 position = transform.position;
            if (Physics.Raycast(position, -Vector3.up, out RaycastHit hitInfo, range, groundLayer, QueryTriggerInteraction.Ignore))
            {
                if (hitInfo.transform.CompareTag(TNC.Terrain) && terrainManager.GetTerrainTextureDetectors() != null)
                {
                    for (int i = 0, length = terrainManager.GetTerrainTextureDetectors().Length; i < length; i++)
                    {
                        TerrainTextureDetector terrainTextureDetector = terrainManager.GetTerrainTextureDetector(i);
                        if (terrainTextureDetector.GetTerrain().transform == hitInfo.transform)
                        {
                            Texture2D terrainTexture = terrainTextureDetector.GetActiveTexture(hitInfo.point);
                            if (terrainTexture != null && footstepMapping.ContainsKey(terrainTexture))
                            {
                                return footstepMapping.GetValue(terrainTexture);
                            }
                        }
                    }
                }
                else
                {
                    PhysicMaterial physicMaterial = hitInfo.collider.sharedMaterial;
                    if (physicMaterial != null && footstepMapping.ContainsKey(physicMaterial))
                    {
                        return footstepMapping.GetValue(physicMaterial);
                    }
                }
            }
            return FootstepSounds.Empty;
        }

        /// <summary>
        /// Play AudioClip sound.
        /// </summary>
        protected void PlaySound(AudioClip sound)
        {
            if (sound != null)
            {
                audioSource.PlayOneShot(sound);
            }
        }

        public void UpdateStoredTime()
        {
            if (storedTime == 0)
            {
                storedTime = Time.time;
            }
        }

        protected void ResetStoredTime()
        {
            if (storedTime != 0)
            {
                storedTime = 0;
            }
        }

        #region [Getter / Setter]
        public PlaySoundType GetPlaySoundType()
        {
            return playSoundType;
        }

        public void SetPlaySoundType(PlaySoundType value)
        {
            playSoundType = value;
        }

        public FootstepMapping GetFootstepMapping()
        {
            return footstepMapping;
        }

        public void SetFootstepMapping(FootstepMapping value)
        {
            footstepMapping = value;
        }

        public FootstepInterval[] GetStepInterval()
        {
            return footStepIntervals;
        }

        public void SetStepInterval(FootstepInterval[] value)
        {
            footStepIntervals = value;
        }

        public LayerMask GetGroundLayer()
        {
            return groundLayer;
        }

        public void SetGroundLayer(LayerMask value)
        {
            groundLayer = value;
        }

        public float GetCurrentInterval()
        {
            return storedTime;
        }

        protected void SetCurrentInterval(float value)
        {
            storedTime = value;
        }

        public AudioSource GetAudioSource()
        {
            return audioSource;
        }
        #endregion
    }
}