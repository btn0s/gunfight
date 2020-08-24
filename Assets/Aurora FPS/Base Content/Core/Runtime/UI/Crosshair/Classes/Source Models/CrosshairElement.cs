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
    public struct CrosshairElement : IEquatable<CrosshairElement>
    {
        [SerializeField] private Texture2D texture;
        [SerializeField] private float width;
        [SerializeField] private float height;
        [SerializeField] private float xPositionOffset;
        [SerializeField] private float yPositionOffset;
        [SerializeField] private Color color;
        [SerializeField] private float outlineAmount;
        [SerializeField] private Color outlineColor;
        [SerializeField] private bool visible;

        /// <summary>
        /// Crosshair element constructor.
        /// </summary>
        /// <param name="texture">Crosshair element texture.</param>
        /// <param name="width">Crosshair element width.</param>
        /// <param name="height">Crosshair element height.</param>
        /// <param name="color">Crosshair element color.</param>
        /// <param name="outlineAmount">Crosshair element ouline amount value.</param>
        /// <param name="outlineColor">Crosshair element outline color.</param>
        /// <param name="visible">Crosshair element visibility.</param>
        public CrosshairElement(Texture2D texture, float width, float height, float xPositionOffset, float yPositionOffset, Color color, float outlineAmount, Color outlineColor, bool visible)
        {
            this.texture = texture;
            this.width = width;
            this.height = height;
            this.xPositionOffset = xPositionOffset;
            this.yPositionOffset = yPositionOffset;
            this.color = color;
            this.outlineAmount = outlineAmount;
            this.outlineColor = outlineColor;
            this.visible = visible;
        }

        /// <summary>
        /// Crosshair element constructor.
        /// </summary>
        /// <param name="texture">Crosshair element texture.</param>
        /// <param name="width">Crosshair element width.</param>
        /// <param name="height">Crosshair element height.</param>
        /// <param name="visible">Crosshair element visibility.</param>
        public CrosshairElement(Texture2D texture, float width, float height, bool visible)
        {
            this.texture = texture;
            this.width = width;
            this.height = height;
            this.xPositionOffset = 0.0f;
            this.yPositionOffset = 0.0f;
            this.color = Color.white;
            this.outlineAmount = 1.0f;
            this.outlineColor = Color.black;
            this.visible = visible;
        }

        /// <summary>
        /// Crosshair element constructor.
        /// </summary>
        /// <param name="texture">Crosshair element texture.</param>
        /// <param name="width">Crosshair element width.</param>
        /// <param name="height">Crosshair element height.</param>
        public CrosshairElement(Texture2D texture, float width, float height)
        {
            this.texture = texture;
            this.width = width;
            this.height = height;
            this.xPositionOffset = 0.0f;
            this.yPositionOffset = 0.0f;
            this.color = Color.white;
            this.outlineAmount = 1.0f;
            this.outlineColor = Color.black;
            this.visible = true;
        }

        #region [Operator override]
        public static implicit operator CrosshairElement(Texture2D texture)
        {
            return new CrosshairElement(texture, 3, 15, 0.0f, 0.0f, Color.white, 1.0f, Color.black, true);
        }

        public static implicit operator Texture2D(CrosshairElement crosshairElement)
        {
            return crosshairElement.GetTexture();
        }
        #endregion

        #region [IEquatable Implementation]
        public static bool operator ==(CrosshairElement left, CrosshairElement right)
        {
            return left.texture == right.texture &&
                left.width == right.width &&
                left.height == right.height &&
                left.xPositionOffset == right.xPositionOffset &&
                left.yPositionOffset == right.yPositionOffset &&
                left.outlineAmount == right.outlineAmount &&
                left.outlineColor == right.outlineColor &&
                left.color == right.color &&
                left.visible == right.visible;
        }

        public static bool operator !=(CrosshairElement left, CrosshairElement right)
        {
            return left.texture != right.texture ||
                left.width != right.width ||
                left.height != right.height ||
                left.xPositionOffset != right.xPositionOffset ||
                left.yPositionOffset != right.yPositionOffset ||
                left.outlineAmount != right.outlineAmount ||
                left.outlineColor != right.outlineColor ||
                left.color != right.color ||
                left.visible != right.visible;
        }

        public override bool Equals(object obj)
        {
            return (obj is CrosshairElement metrics) && Equals(metrics);
        }

        public bool Equals(CrosshairElement other)
        {
            return (texture, width, height, xPositionOffset, yPositionOffset,  color, outlineAmount, outlineColor, visible) ==
                (other.texture, other.width, other.height, other.xPositionOffset, other.yPositionOffset, other.color, other.outlineAmount, other.outlineColor, visible);
        }

        public override int GetHashCode()
        {
            return (texture, width, height, xPositionOffset, yPositionOffset, color, outlineAmount, outlineColor, visible).GetHashCode();
        }
        #endregion

        #region [Default Values]
        public readonly static CrosshairElement none = new CrosshairElement(null, 0, 0);
        #endregion

        #region [Getter / Setter]
        public Texture2D GetTexture()
        {
            return texture;
        }

        public void SetTexture(Texture2D value)
        {
            texture = value;
        }

        public float GetWidth()
        {
            return width;
        }

        public void SetWidth(float value)
        {
            width = value;
        }

        public float GetHeight()
        {
            return height;
        }

        public void SetHeight(float value)
        {
            height = value;
        }

        public float GetXPositionOffset()
        {
            return xPositionOffset;
        }

        public void SetXPositionOffset(float value)
        {
            xPositionOffset = value;
        }

        public float GetYPositionOffset()
        {
            return yPositionOffset;
        }

        public void SetYPositionOffset(float value)
        {
            yPositionOffset = value;
        }

        public Color GetColor()
        {
            return color;
        }

        public void SetColor(Color value)
        {
            color = value;
        }

        public float GetOutlineAmount()
        {
            return outlineAmount;
        }

        public void SetOutlineAmount(float value)
        {
            outlineAmount = value;
        }

        public Color GetOutlineColor()
        {
            return outlineColor;
        }

        public void SetOutlineColor(Color value)
        {
            outlineColor = value;
        }

        public bool Visible()
        {
            return visible;
        }

        public void Visible(bool value)
        {
            visible = value;
        }
        #endregion
    }
}