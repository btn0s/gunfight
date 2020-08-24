﻿/* ================================================================
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
    public class DecalObject : PoolObject
    {
        /// <summary>
        /// Axes of transform rotation.
        /// </summary>
        public enum RotationAxis { X, Y, Z }

        // Base decal object properties.
        [SerializeField] private Vector3 originalScale = Vector3.one;
        [SerializeField] private Vector3 originalRotation = Vector3.zero;

        // Randomize scale properties.
        [SerializeField] private float minRandomScale = 0.5f;
        [SerializeField] private float maxRandomScale = 1.0f;
        [SerializeField] private bool randomizeScale = true;

        // Randomize rotation properties.
        [SerializeField] RotationAxis rotationAxis;
        [SerializeField] private float minRandomRotation = -180;
        [SerializeField] private float maxRandomRotation = 180;
        [SerializeField] private bool randomizeRotation = true;

        // Sound properties.
        [SerializeField] private AudioClip[] sounds;

        // Stored required components.
        private AudioSource audioSource;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            audioSource = GetComponent<AudioSource>();
            OnAfterPushCallback += ResetTransform;
        }

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();

            if (randomizeRotation)
                ApplyRandomRotation();

            if (randomizeScale)
                ApplyRandomScale();

            PlayRandomSound();
        }

        /// <summary>
        /// Apply random scale for this decal object.
        /// Will be reseted on original scale after pushing in Pool Manager.
        /// </summary>
        protected virtual void ApplyRandomScale()
        {
            transform.localScale = GetRandomScale();
        }

        /// <summary>
        /// Apply random rotation by specific axis for this decal object.
        /// Will be reseted on original rotation after pushing in Pool Manager.
        /// </summary>
        protected virtual void ApplyRandomRotation()
        {
            transform.eulerAngles = GetRandomRotation();
        }

        /// <summary>
        /// Reset rotation and scale values to original.
        /// </summary>
        public void ResetTransform()
        {
            transform.eulerAngles = originalRotation;
            transform.localScale = originalScale;
        }

        /// <summary>
        /// Get random scale relative original decal object scale.
        /// Every call return random scale relative original scale vector.
        /// </summary>
        /// <returns>Random scale of decal object.</returns>
        public Vector3 GetRandomScale()
        {
            Vector3 randomScale = originalScale;
            float randomScaleValue = Random.Range(minRandomScale, maxRandomScale);
            randomScale.x += randomScaleValue;
            randomScale.y += randomScaleValue;
            randomScale.z += randomScaleValue;
            return randomScale;
        }

        /// <summary>
        /// Get random rotation by specific axis relative original decal object rotation.
        /// Every call return original rotation with specific random axis value.
        /// </summary>
        /// <returns>Random scale of decal object.</returns>
        public Vector3 GetRandomRotation()
        {
            Vector3 randomRotation = originalRotation;
            float randomAxisValue = Random.Range(minRandomRotation, maxRandomRotation);
            switch (rotationAxis)
            {
                case RotationAxis.X:
                    randomRotation.x = randomAxisValue;
                    break;
                case RotationAxis.Y:
                    randomRotation.y = randomAxisValue;
                    break;
                case RotationAxis.Z:
                    randomRotation.z = randomAxisValue;
                    break;
            }
            return randomRotation;
        }

        public void PlayRandomSound()
        {
            if (sounds != null && sounds.Length > 0)
            {
                audioSource.PlayOneShot(sounds[Random.Range(0, sounds.Length)]);
            }
        }

        #region [Getter / Setter]
        public Vector3 GetOriginalScale()
        {
            return originalScale;
        }

        public void SetOriginalScale(Vector3 value)
        {
            originalScale = value;
        }

        public Vector3 GetOriginalRotation()
        {
            return originalRotation;
        }

        public void SetOriginalRotation(Vector3 value)
        {
            originalRotation = value;
        }

        public RotationAxis GetRotationAxis()
        {
            return rotationAxis;
        }

        public void SetRotationAxis(RotationAxis value)
        {
            rotationAxis = value;
        }

        public float GetMinRandomRotation()
        {
            return minRandomRotation;
        }

        public void SetMinRandomRotation(float value)
        {
            minRandomRotation = value;
        }

        public float GetMaxRandomRotation()
        {
            return maxRandomRotation;
        }

        public void SetMaxRandomRotation(float value)
        {
            maxRandomRotation = value;
        }

        public bool RandomizeRotation()
        {
            return randomizeRotation;
        }

        public void RandomizeRotation(bool value)
        {
            randomizeRotation = value;
        }

        public float GetMinRandomScale()
        {
            return minRandomScale;
        }

        public void SetMinRandomScale(float value)
        {
            minRandomScale = value;
        }

        public float GetMaxRandomScale()
        {
            return maxRandomScale;
        }

        public void SetMaxRandomScale(float value)
        {
            maxRandomScale = value;
        }

        public bool RandomizeScale()
        {
            return randomizeScale;
        }

        public void RandomizeScale(bool value)
        {
            randomizeScale = value;
        }

        public AudioClip[] GetSounds()
        {
            return sounds;
        }

        public void SetSounds(AudioClip[] value)
        {
            sounds = value;
        }
        #endregion
    }
}