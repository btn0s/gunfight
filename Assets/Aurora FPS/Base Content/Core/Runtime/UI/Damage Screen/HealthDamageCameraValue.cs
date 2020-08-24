/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using System;
using UnityEngine;

namespace AuroraFPSRuntime.UI
{
    [Serializable]
    public struct HealthDamageCameraValue : IEquatable<HealthDamageCameraValue>
    {
        [SerializeField] private float alpha;
        [SerializeField] private int minHealth;
        [SerializeField] private int maxHealth;
        [SerializeField] private float lerpSpeed;


        public HealthDamageCameraValue(int alpha, int minHealth, int maxHealth, float lerpSpeed)
        {
            this.alpha = alpha;
            this.minHealth = minHealth;
            this.maxHealth = maxHealth;
            this.lerpSpeed = lerpSpeed;
        }

        #region [IEquatable Implementation]
        public override bool Equals(object obj)
        {
            return (obj is HealthDamageCameraValue metrics) && Equals(metrics);
        }

        public bool Equals(HealthDamageCameraValue other)
        {
            return (alpha, minHealth, maxHealth, lerpSpeed) == (other.alpha, other.minHealth, other.maxHealth, other.lerpSpeed);
        }

        public override int GetHashCode()
        {
            return (alpha, minHealth, maxHealth, lerpSpeed).GetHashCode();
        }
        #endregion

        #region [Operator Overloading]
        public static bool operator ==(HealthDamageCameraValue left, HealthDamageCameraValue right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(HealthDamageCameraValue left, HealthDamageCameraValue right)
        {
            return !Equals(left, right);
        }
        #endregion

        #region [Getter / Setter]
        public float GetAlpha()
        {
            return alpha;
        }

        public void SetAlpha(float value)
        {
            alpha = value;
        }

        public int GetMinHealth()
        {
            return minHealth;
        }

        public void SetMinHealth(int value)
        {
            minHealth = value;
        }

        public int GetMaxHealth()
        {
            return maxHealth;
        }

        public void SetMaxHealth(int value)
        {
            maxHealth = value;
        }

        public float GetLerpSpeed()
        {
            return lerpSpeed;
        }

        public void SetLerpSpeed(float value)
        {
            lerpSpeed = value;
        }
        #endregion
    }
}