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
    [Serializable]
    public struct HealthSoundEffects : IEquatable<HealthSoundEffects>
    {
        [SerializeField] private AudioClip takeDamageSound;
        [SerializeField] private AudioClip velocityDamageSound;
        [SerializeField] private AudioClip heartbeatSound;
        [SerializeField] private float heartbeatRate;
        [SerializeField] private int heartbeatStartFrom;
        [SerializeField] private AudioClip deathSound;

        public HealthSoundEffects(AudioClip takeDamageSound, AudioClip velocityDamageSound, AudioClip heartbeatSound, float heartbeatRate, int heartbeatStartFrom, AudioClip deathSound)
        {
            this.takeDamageSound = takeDamageSound;
            this.velocityDamageSound = velocityDamageSound;
            this.heartbeatSound = heartbeatSound;
            this.heartbeatRate = heartbeatRate;
            this.heartbeatStartFrom = heartbeatStartFrom;
            this.deathSound = deathSound;
        }

        public AudioClip GetTakeDamageSound()
        {
            return takeDamageSound;
        }

        public void SetTakeDamageSound(AudioClip value)
        {
            takeDamageSound = value;
        }

        public AudioClip GetVelocityDamageSound()
        {
            return velocityDamageSound;
        }

        public void SetVelocityDamageSound(AudioClip value)
        {
            velocityDamageSound = value;
        }

        public AudioClip GetHeartbeatSound()
        {
            return heartbeatSound;
        }

        public void SetHeartbeatSound(AudioClip value)
        {
            heartbeatSound = value;
        }

        public float GetHeartbeatRate()
        {
            return heartbeatRate;
        }

        public void SetHeartbeatRate(float value)
        {
            heartbeatRate = value;
        }

        public int GetHeartbeatStartFrom()
        {
            return heartbeatStartFrom;
        }

        public void SetHeartbeatStartFrom(int value)
        {
            heartbeatStartFrom = value;
        }

        public AudioClip GetDeathSound()
        {
            return deathSound;
        }

        public void SetDeathSound(AudioClip value)
        {
            deathSound = value;
        }

        /// <summary>
        /// Empty Regeniration properties.
        /// </summary>
        /// <returns></returns>
        public readonly static HealthSoundEffects Empty = new HealthSoundEffects(null, null, null, 1.0f, 50, null);

        public static bool operator ==(HealthSoundEffects left, HealthSoundEffects right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(HealthSoundEffects left, HealthSoundEffects right)
        {
            return !Equals(left, right);
        }

        public override bool Equals(object obj)
        {
            return (obj is HealthSoundEffects metrics) && Equals(metrics);
        }

        public bool Equals(HealthSoundEffects other)
        {
            return (takeDamageSound, velocityDamageSound, heartbeatSound, heartbeatRate, heartbeatStartFrom, deathSound) == (other.takeDamageSound, other.velocityDamageSound, other.heartbeatSound, other.heartbeatRate, other.heartbeatStartFrom, other.deathSound);
        }

        public override int GetHashCode()
        {
            return (takeDamageSound, velocityDamageSound, heartbeatSound, heartbeatRate, heartbeatStartFrom, deathSound).GetHashCode();
        }
    }
}