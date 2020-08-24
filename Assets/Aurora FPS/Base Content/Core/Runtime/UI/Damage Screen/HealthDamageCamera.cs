/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AuroraFPSRuntime.UI
{
    public class HealthDamageCamera : MonoBehaviour
    {
        [SerializeReference] private HealthComponent healthSystem;
        [SerializeField] private Image damageImage;
        [SerializeField] private int startHealthPoint;
        [SerializeField] private HealthDamageCameraValue[] healthRanges;
        [SerializeField] private UnityEvent onStartProcessingEvent;
        [SerializeField] private UnityEvent onEndProcessingEvent;

        private bool startProcessingCallbackInvoked;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        protected virtual void Start()
        {
            DisableImage();

            OnStartProcessingCallback += () => damageImage.gameObject.SetActive(true);
            OnStartProcessingCallback += () => onStartProcessingEvent?.Invoke();

            OnEndProcessingCallback += () => damageImage.gameObject.SetActive(false);
            OnEndProcessingCallback += () => onEndProcessingEvent?.Invoke();
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void Update()
        {
            if (healthSystem.GetHealth() <= startHealthPoint)
            {
                if (!startProcessingCallbackInvoked)
                {
                    OnStartProcessingCallback?.Invoke();
                    startProcessingCallbackInvoked = true;
                }
                for (int i = 0; i < healthRanges.Length; i++)
                {
                    HealthDamageCameraValue healthRange = healthRanges[i];
                    if (AMath.InRange(healthSystem.GetHealth(), healthRange.GetMinHealth(), healthRange.GetMaxHealth()))
                    {
                        Color color = damageImage.color;
                        color.a = Mathf.Lerp(color.a, healthRange.GetAlpha(), Time.deltaTime * healthRange.GetLerpSpeed());
                        damageImage.color = color;
                    }
                }
            }
            else if (startProcessingCallbackInvoked)
            {
                OnEndProcessingCallback();
                startProcessingCallbackInvoked = false;
            }
        }

        /// <summary>
        /// Disabling damage image and making transparent.
        /// </summary>
        private void DisableImage()
        {
            Color colorTransparent = damageImage.color;
            colorTransparent.a = 0;
            damageImage.color = colorTransparent;
            damageImage.gameObject.SetActive(false);
        }

        #region [Event Callback Functions]
        /// <summary>
        /// Called once when damage screen image start processsing.
        /// </summary>
        public event System.Action OnStartProcessingCallback;

        /// <summary>
        /// Called once when damage screen image end processsing.
        /// </summary>
        public event System.Action OnEndProcessingCallback;

        #endregion

        #region [Getter / Setter]
        public HealthComponent GetHealthSystem()
        {
            return healthSystem;
        }

        public void SetHealthSystem(HealthComponent value)
        {
            healthSystem = value;
        }

        public Image GetDamageImage()
        {
            return damageImage;
        }

        public void SetDamageImage(Image value)
        {
            damageImage = value;
        }

        public int GetStartHealthPoint()
        {
            return startHealthPoint;
        }

        public void SetStartHealthPoint(int value)
        {
            startHealthPoint = value;
        }

        public HealthDamageCameraValue[] GetHealthRanges()
        {
            return healthRanges;
        }

        public void SetHealthRanges(HealthDamageCameraValue[] value)
        {
            healthRanges = value;
        }

        public UnityEvent GetOnStartProcessingEvent()
        {
            return onStartProcessingEvent;
        }

        public void SetOnStartProcessingEvent(UnityEvent value)
        {
            onStartProcessingEvent = value;
        }

        public UnityEvent GetOnEndProcessingEvent()
        {
            return onEndProcessingEvent;
        }

        public void SetOnEndProcessingEvent(UnityEvent value)
        {
            onEndProcessingEvent = value;
        }
        #endregion
    }
}