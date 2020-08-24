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
    public partial class CameraShake
    {
        [System.Serializable]
        public struct ShakeProperty : IEquatable<ShakeProperty>
        {
            [SerializeField] private float amplitude;
            [SerializeField] private float frequency;
            [SerializeField] private float duration;
            [SerializeField] private AnimationCurve blendOverLifetime;

            public ShakeProperty(float amplitude, float frequency, float duration, AnimationCurve blendOverLifetime)
            {
                this.amplitude = amplitude;
                this.frequency = frequency;
                this.duration = duration;
                this.blendOverLifetime = blendOverLifetime;
            }

            #region [IEquatable Implementation]
            public static bool operator ==(ShakeProperty left, ShakeProperty right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(ShakeProperty left, ShakeProperty right)
            {
                return !Equals(left, right);
            }

            public override bool Equals(object obj)
            {
                return (obj is ShakeProperty metrics) && Equals(metrics);
            }

            public bool Equals(ShakeProperty other)
            {
                return (amplitude, blendOverLifetime, duration, frequency) == (other.amplitude, other.blendOverLifetime, other.duration, other.frequency);
            }

            public override int GetHashCode()
            {
                return (amplitude, blendOverLifetime, duration, frequency).GetHashCode();
            }
            #endregion

            #region [Getter / Setter]
            public float GetAmplitude()
            {
                return amplitude;
            }

            public void SetAmplitude(float value)
            {
                amplitude = value;
            }

            public float GetFrequency()
            {
                return frequency;
            }

            public void SetFrequency(float value)
            {
                frequency = value;
            }

            public float GetDuration()
            {
                return duration;
            }

            public void SetDuration(float value)
            {
                duration = value;
            }

            public AnimationCurve GetBlendOverLifetime()
            {
                return blendOverLifetime;
            }

            public void SetBlendOverLifetime(AnimationCurve value)
            {
                blendOverLifetime = value;
            }
            #endregion
        }
    }
}