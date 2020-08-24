/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using System.Collections;
using UnityEngine;


namespace AuroraFPSRuntime
{
    [System.Serializable]
    public class ScreenFade : Singleton<ScreenFade>, IScreenFade
    {
        // Base screen fade properties.
        [SerializeField] private ScreenFadeProperties properties = ScreenFadeProperties.DefaultProperties;

        // Stored required properties.
        private Rect textureRect;
        private Texture2D texture;
        private Color targetColor;
        private Color currentColor;
        private float currentSpeed;
        private IEnumerator pingPongCoroutine;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            textureRect = new Rect(0, 0, Screen.width, Screen.height);
            texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, Color.black);
            texture.Apply();
        }

        /// <summary>
        /// OnGUI is called for rendering and handling GUI events.
        /// This function can be called multiple times per frame (one call per event).
        /// </summary>
        protected virtual void OnGUI()
        {
            currentColor = Color.Lerp(currentColor, targetColor, currentSpeed * Time.deltaTime);

            GUI.depth = -2;
            GUI.color = currentColor;
            GUI.DrawTexture(textureRect, texture, ScaleMode.StretchToFill, true);
        }

        public virtual void PingPongFade()
        {
            if (pingPongCoroutine != null)
            {
                return;
            }
            pingPongCoroutine = PingPongFadeCoroutine(properties);
            StartCoroutine(pingPongCoroutine);
        }

        public virtual void PingPongFade(ScreenFadeProperties properties)
        {
            if (pingPongCoroutine != null)
            {
                return;
            }

            pingPongCoroutine = PingPongFadeCoroutine(properties);
            StartCoroutine(pingPongCoroutine);
        }

        public virtual void InFade()
        {
            if (pingPongCoroutine != null)
            {
                StopPingPong();
            }

            ApplyTarget(properties.GetInFadeColor(), properties.GetInFadeSpeed());
        }

        public virtual void InFade(Color color)
        {
            if (pingPongCoroutine != null)
            {
                StopPingPong();
            }

            ApplyTarget(color, properties.GetInFadeSpeed());
        }

        public virtual void InFade(Color color, float speed)
        {
            if (pingPongCoroutine != null)
            {
                StopPingPong();
            }

            ApplyTarget(color, speed);
        }

        public virtual void InFade(ScreenFadeProperties properties)
        {
            if (pingPongCoroutine != null)
            {
                StopPingPong();
            }

            ApplyTarget(properties.GetInFadeColor(), properties.GetInFadeSpeed());
        }

        public virtual void OutFade()
        {
            if (pingPongCoroutine != null)
            {
                StopPingPong();
            }

            targetColor = ClearColor(targetColor);
            currentSpeed = properties.GetOutFadeSpeed();
        }

        public virtual void OutFade(float speed)
        {
            if (pingPongCoroutine != null)
            {
                StopPingPong();
            }

            targetColor = ClearColor(targetColor);
            currentSpeed = properties.GetOutFadeSpeed();
        }

        public virtual void OutFade(ScreenFadeProperties properties)
        {
            if (pingPongCoroutine != null)
            {
                StopPingPong();
            }

            targetColor = ClearColor(targetColor);
            currentSpeed = properties.GetOutFadeSpeed();
        }

        protected virtual IEnumerator PingPongFadeCoroutine(ScreenFadeProperties properties)
        {
            ApplyTarget(properties.GetInFadeColor(), properties.GetInFadeSpeed());
            bool isOuting = false;
            while (true)
            {
                if (AMath.Approximately(currentColor.a, targetColor.a))
                {
                    currentSpeed = properties.GetOutFadeSpeed();
                    targetColor = ClearColor(targetColor);
                    isOuting = true;
                }
                if (isOuting && AMath.Approximately(currentColor.a, targetColor.a))
                {
                    pingPongCoroutine = null;
                    yield break;
                }
                yield return null;
            }
        }

        private void StopPingPong()
        {
            StopCoroutine(pingPongCoroutine);
            pingPongCoroutine = null;
        }

        private void ApplyTarget(Color color, float speed)
        {
            targetColor = color;
            currentSpeed = speed;
            texture.SetPixel(0, 0, targetColor);
            texture.Apply();
        }

        private Color ClearColor(Color color)
        {
            color.a = 0;
            return color;
        }

        public ScreenFadeProperties GetProperties()
        {
            return properties;
        }

        public void SetProperties(ScreenFadeProperties value)
        {
            properties = value;
        }
    }
}
