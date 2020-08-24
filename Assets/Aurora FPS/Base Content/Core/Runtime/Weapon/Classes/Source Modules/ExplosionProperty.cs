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
    public struct ExplosionProperty : IEquatable<ExplosionProperty>
    {
        [SerializeField] private int damage;
        [SerializeField] private float amplitude;
        [SerializeField] private float impulse;
        [SerializeField] private float upwardsModifier;
        [SerializeField] private float minDistance;
        [SerializeField] private float maxDistance;
        [SerializeField] private CameraShake.Shake shake;

        public ExplosionProperty(int damage, float amplitude, float impulse, float upwardsModifier, float minDistance, float maxDistance, CameraShake.Shake shake)
        {
            this.damage = damage;
            this.amplitude = amplitude;
            this.impulse = impulse;
            this.upwardsModifier = upwardsModifier;
            this.minDistance = minDistance;
            this.maxDistance = maxDistance;
            this.shake = shake;
        }

        public int GetDamage()
        {
            return damage;
        }

        public void SetDamage(int value)
        {
            damage = value;
        }

        public float GetAmplitude()
        {
            return amplitude;
        }

        public void SetAmplitude(float value)
        {
            amplitude = value;
        }

        public float GetImpulse()
        {
            return impulse;
        }

        public void SetImpulse(float value)
        {
            impulse = value;
        }

        public float GetUpwardsModifier()
        {
            return upwardsModifier;
        }

        public void SetUpwardsModifier(float value)
        {
            upwardsModifier = value;
        }

        public float GetMinDistance()
        {
            return minDistance;
        }

        public void SetMinDistance(float value)
        {
            minDistance = value;
        }

        public float GetMaxDistance()
        {
            return maxDistance;
        }

        public void SetMaxDistance(float value)
        {
            maxDistance = value;
        }

        public CameraShake.Shake GetShake()
        {
            return shake;
        }

        public void SetShake(CameraShake.Shake value)
        {
            shake = value;
        }

        public readonly static ExplosionProperty none = new ExplosionProperty(0, 0, 0, 0, 0, 0, null);

        public static bool operator ==(ExplosionProperty left, ExplosionProperty right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ExplosionProperty left, ExplosionProperty right)
        {
            return !Equals(left, right);
        }

        public override bool Equals(object obj)
        {
            return (obj is ExplosionProperty metrics) && Equals(metrics);
        }

        public bool Equals(ExplosionProperty other)
        {
            return (minDistance, maxDistance) == (other.minDistance, other.maxDistance);
        }

        public override int GetHashCode()
        {
            return (damage, amplitude, impulse, upwardsModifier, minDistance, maxDistance).GetHashCode();
        }
    }
}