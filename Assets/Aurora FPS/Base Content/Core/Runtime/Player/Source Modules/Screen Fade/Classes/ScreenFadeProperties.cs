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
    public struct ScreenFadeProperties : IEquatable<ScreenFadeProperties>
    {
        [SerializeField] private Color inFadeColor;
        [SerializeField] private float inFadeSpeed;
        [SerializeField] private float outFadeSpeed;

        public ScreenFadeProperties(Color inFadeColor, float inFadeSpeed, float outFadeSpeed)
        {
            this.inFadeColor = inFadeColor;
            this.inFadeSpeed = inFadeSpeed;
            this.outFadeSpeed = outFadeSpeed;
        }

        public Color GetInFadeColor()
        {
            return inFadeColor;
        }

        public void SetInFadeColor(Color value)
        {
            inFadeColor = value;
        }

        public float GetInFadeSpeed()
        {
            return inFadeSpeed;
        }

        public void SetInFadeSpeed(float value)
        {
            inFadeSpeed = value;
        }

        public float GetOutFadeSpeed()
        {
            return outFadeSpeed;
        }

        public void SetOutFadeSpeed(float value)
        {
            outFadeSpeed = value;
        }

        public readonly static ScreenFadeProperties DefaultProperties = new ScreenFadeProperties(Color.black, 3.0f, 3.0f);

        public static bool operator ==(ScreenFadeProperties left, ScreenFadeProperties right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ScreenFadeProperties left, ScreenFadeProperties right)
        {
            return !Equals(left, right);
        }

        public override bool Equals(object obj)
        {
            return (obj is ScreenFadeProperties metrics) && Equals(metrics);
        }

        public bool Equals(ScreenFadeProperties other)
        {
            return (inFadeColor, inFadeSpeed, outFadeSpeed) == (other.inFadeColor, other.inFadeSpeed, other.outFadeSpeed);
        }

        public override int GetHashCode()
        {
            return (inFadeColor, inFadeSpeed, outFadeSpeed).GetHashCode();
        }
    }
}
