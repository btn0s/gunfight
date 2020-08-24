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
    public struct VelocityDamage : IEquatable<VelocityDamage>
    {
        [SerializeField] private int damage;
        [SerializeField] private float minVelocity;
        [SerializeField] private float maxVelocity;

        public VelocityDamage(int damage, float minVelocity, float maxVelocity)
        {
            this.damage = damage;
            this.minVelocity = minVelocity;
            this.maxVelocity = maxVelocity;
        }

        #region [IEquatable Implementation]
        public override bool Equals(object obj)
        {
            return (obj is VelocityDamage metrics) && Equals(metrics);
        }

        public bool Equals(VelocityDamage other)
        {
            return (damage, minVelocity, maxVelocity) == (other.damage, other.minVelocity, other.maxVelocity);
        }

        public override int GetHashCode()
        {
            return (damage, minVelocity, maxVelocity).GetHashCode();
        }
        #endregion

        #region [Operator Overloading]
        public static bool operator ==(VelocityDamage left, VelocityDamage right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(VelocityDamage left, VelocityDamage right)
        {
            return !Equals(left, right);
        }
        #endregion

        #region [Getter / Setter]
        public int GetDamage()
        {
            return damage;
        }

        public void SetDamage(int value)
        {
            damage = value;
        }

        public float GetMinVelocity()
        {
            return minVelocity;
        }

        public void SetMinVelocity(float value)
        {
            minVelocity = value;
        }

        public float GetMaxVelocity()
        {
            return maxVelocity;
        }

        public void SetMaxVelocity(float value)
        {
            maxVelocity = value;
        }
        #endregion
    }
}