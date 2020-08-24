/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using UnityEngine;
using System;

namespace AuroraFPSRuntime
{
    [Serializable]
    public struct AnimatorValue : IEquatable<AnimatorValue>
    {
        // Base animator value properties.
        [SerializeField] private string name;
        [SerializeField, HideInInspector] private int nameHash;

        /// <summary>
        /// Animator value constructor.
        /// </summary>
        public AnimatorValue(string value)
        {
            this.name = value;
            this.nameHash = Animator.StringToHash(name);
        }

        public readonly static AnimatorValue none = new AnimatorValue(string.Empty);

        #region [IEquatable Implementation]
        public override bool Equals(object obj)
        {
            return (obj is AnimatorValue metrics) && Equals(metrics);
        }

        public bool Equals(string other)
        {
            return nameHash == Animator.StringToHash(other);
        }

        public bool Equals(AnimatorValue other)
        {
            return nameHash == other.nameHash;
        }

        public override int GetHashCode()
        {
            return (name, nameHash).GetHashCode();
        }
        #endregion

        #region [Operator Overloading]
        public static implicit operator AnimatorValue(string name)
        {
            return new AnimatorValue(name);
        }

        public static implicit operator int(AnimatorValue animatorValue)
        {
            return animatorValue.GetNameHash();
        }

        public static bool operator ==(AnimatorValue left, AnimatorValue right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AnimatorValue left, AnimatorValue right)
        {
            return !Equals(left, right);
        }
        #endregion

        #region [Getter / Setter]
        public string GetName()
        {
            return name;
        }

        public void SetName(string value)
        {
            this.name = value;
            nameHash = Animator.StringToHash(this.name);
        }

        public int GetNameHash()
        {
            return nameHash;
        }

        private void SetNameHash(int value)
        {
            nameHash = value;
        }
        #endregion
    }
}