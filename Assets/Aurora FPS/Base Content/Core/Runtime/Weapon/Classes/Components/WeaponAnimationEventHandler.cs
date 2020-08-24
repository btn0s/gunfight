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
    [RequireComponent(typeof(AudioSource))]
    public class WeaponAnimationEventHandler : MonoBehaviour
    {
        // Base weapon animation event handler.
        [SerializeField] private ShakeMapping shakePropertiesMapping;

        // Stored required components.
        private CameraShake shakeCamera;
        private AudioSource audioSource;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            shakeCamera = CameraShake.Instance;
            audioSource = GetComponent<AudioSource>();
        }

        /// <summary>
        /// Add shake effect to player camera property by name.
        /// </summary>
        public virtual void AddShakeProperty(string name)
        {
            if (shakePropertiesMapping != null && shakePropertiesMapping.ContainsKey(name))
            {
                shakeCamera.AddShake(shakePropertiesMapping.GetValue(name));
            }
        }

        /// <summary>
        /// Play audio sound on weapon audiosource.
        /// </summary>
        public virtual void PlaySound(AudioClip sound)
        {
            audioSource.PlayOneShot(sound);
        }

        #region [Getter / Setter]
        public ShakeMapping GetShakePropertiesMapping()
        {
            return shakePropertiesMapping;
        }

        public void SetShakePropertiesMapping(ShakeMapping value)
        {
            shakePropertiesMapping = value;
        }

        public CameraShake GetShakeCameraInstance()
        {
            return shakeCamera;
        }

        public void SetShakeCameraInstance(CameraShake value)
        {
            shakeCamera = value;
        }

        public AudioSource GetAudioSource()
        {
            return audioSource;
        }

        protected void SetAudioSource(AudioSource value)
        {
            audioSource = value;
        }
        #endregion
    }
}