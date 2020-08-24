/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using UnityEngine;
using UnityEngine.UI;


namespace AuroraFPSRuntime
{
    [RequireComponent(typeof(CharacterHealth))]
    [RequireComponent(typeof(AudioSource))]
    public class ControllerRespawner : MonoBehaviour, IControllerRespawner
    {
        // Base controller respawner properties.
        [SerializeField] private Transform respawnPoint;
        [SerializeField] private float delay = 5.0f;
        [SerializeField] private int respawnHealth;
        [SerializeField] private bool drawOnGUI;
        [SerializeField] private string textFieldPrefix = "Respawn after: {0}";
        [SerializeField] private string delayFormat = "0.00";
        [SerializeField] private RectTransform uiRoot;
        [SerializeField] private Text textField;

        // Stored required components 
        private CharacterHealth health;
        private AudioSource audioSource;

        // Stored required properties.
        private float storedDelay;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            health = GetComponent<CharacterHealth>();
            audioSource = GetComponent<AudioSource>();
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        protected virtual void Start()
        {
            health.OnDeadCallback += Respawn;
        }

        /// <summary>
        /// LateUpdate is called every frame, if the Behaviour is enabled.
        /// It is called after all Update functions have been called.
        /// </summary>
        protected virtual void LateUpdate()
        {
            if (storedDelay > 0)
            {
                float elapsedTime = Time.time - storedDelay;

                if (drawOnGUI && uiRoot != null && textField != null)
                {
                    if (!uiRoot.gameObject.activeSelf)
                    {
                        uiRoot.gameObject.SetActive(true);
                    }
                    textField.text = string.Format(textFieldPrefix, elapsedTime.ToString(delayFormat));
                }

                if (elapsedTime >= delay)
                {
                    health.SetHealth(respawnHealth);
                    storedDelay = 0;
                }
            }
            
        }

        /// <summary>
        /// Respawn controller on the respawn point after specific delay.
        /// </summary>
        public virtual void Respawn()
        {
            if(storedDelay == 0)
            {
                storedDelay = Time.time;
                health.SetHealth(0);
            }
        }

        #region [Getter / Setter]
        public Transform GetRespawnPoint()
        {
            return respawnPoint;
        }

        public void SetRespawnPoint(Transform value)
        {
            respawnPoint = value;
        }

        public float GetDelay()
        {
            return delay;
        }

        public void SetDelay(float value)
        {
            delay = value;
        }

        public int GetRespawnHealth()
        {
            return respawnHealth;
        }

        public void SetRespawnHealth(int value)
        {
            respawnHealth = value;
        }

        public bool GetDrawOnGUI()
        {
            return drawOnGUI;
        }

        public void SetDrawOnGUI(bool value)
        {
            drawOnGUI = value;
        }

        public string GetTextFieldPrefix()
        {
            return textFieldPrefix;
        }

        public void SetTextFieldPrefix(string value)
        {
            textFieldPrefix = value;
        }

        public string GetDelayFormat()
        {
            return delayFormat;
        }

        public void SetDelayFormat(string value)
        {
            delayFormat = value;
        }

        public RectTransform GetUIRoot()
        {
            return uiRoot;
        }

        public void SetUIRoot(RectTransform value)
        {
            uiRoot = value;
        }

        public Text GetTextField()
        {
            return textField;
        }

        public void SetTextField(Text value)
        {
            textField = value;
        }

        public CharacterHealth GetHealth()
        {
            return health;
        }

        public void SetHealth(CharacterHealth value)
        {
            health = value;
        }

        public AudioSource GetAudioSource()
        {
            return audioSource;
        }

        public void SetAudioSource(AudioSource value)
        {
            audioSource = value;
        }
        #endregion
    }
}