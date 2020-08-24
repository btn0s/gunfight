/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace AuroraFPSRuntime
{
    /// <summary>
    /// Represents of loot object.
    /// Implementation of LootObjectBase.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public abstract class LootObject : LootObjectBase
    {
        public enum LootMode
        {
            Auto,
            Button,
        }

        // Base loot object properties.
        [SerializeField] private GameObject objectMesh;
        [SerializeField] private LootMode lootMode = LootMode.Auto;
        [SerializeField] private string lootButton = "Loot Weapon";

        // Multiple loot properties.
        [SerializeField] private bool allowMultipleLoot = false;
        [SerializeField] private int multipleLootCount = 2;
        [SerializeField] private float reActivateDelay = 3.0f;

        // Loot messages properties.
        [SerializeField] private string defaultLootMessage = "HOLD [F] TO LOOT";
        [SerializeField] private string alternativeLootMessage = "FULL";

        // Loot sound properties.
        [SerializeField] private AudioClip lootSound;

        // Stored required components.
        private AudioSource audioSource;

        // Stored required properties.
        private CoroutineObject<Transform> lootProcessing;
        private bool isBecomeVisible;


        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();

            lootProcessing = new CoroutineObject<Transform>(this);
        }

        /// <summary>
        /// Start up base loot logic in dependence loot mode.
        /// </summary>
        /// <param name="target">Transform instance that want to loot this object.</param>
        private IEnumerator LootProcessing(Transform target)
        {
            if (lootMode == LootMode.Button)
            {
                yield return new WaitForAInput(lootButton, InputEvent.Pressed);
            }

            OnLoot(target);
            AfterLoot();
            objectMesh.SetActive(false);
            PlayLootSound();

            yield return lootSound != null ? new WaitForSeconds(lootSound.length) : null;

            if (!allowMultipleLoot)
            {
                Destroy(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
                yield return new WaitForSeconds(reActivateDelay);
                gameObject.SetActive(true);
                objectMesh.SetActive(true);
            }
        }

        /// <summary>
        /// Play loot sound.
        /// </summary>
        public void PlayLootSound()
        {
            if (lootSound != null)
            {
                audioSource.PlayOneShot(lootSound);
            }
        }

        #region [LootObject Abstract Methdos]
        /// <summary>
        /// Called once when target trying to loot this object.
        /// </summary>
        /// <param name="target">Transform instance that want to loot this object.</param>
        protected abstract void OnLoot(Transform target);
        #endregion

        #region [ILootObject Implementation]
        /// <summary>
        /// Loot this object to target transfrom.
        /// </summary>
        /// <param name="target">Transform instance that want to loot this object.</param>
        public sealed override void Loot(Transform target)
        {
            if (target != null)
            {
                if (!isBecomeVisible)
                {
                    OnBecomeVisible(target);
                    isBecomeVisible = true;
                }
                if (AvailableToLoot())
                {
                    lootProcessing.Start(LootProcessing, target);
                }
            }
            else
            {
                lootProcessing.Stop();
                if (isBecomeVisible)
                {
                    OnBecomeInVisible();
                    isBecomeVisible = false;
                }
            }
        }
        #endregion

        #region [ILootObjectEnabled Implementation]
        public override bool IsEnabled()
        {
            return objectMesh.activeSelf;
        }
        #endregion

        #region [ILootObjectMessage Implementation]
        /// <summary>
        /// Message to display in player HUD, when player watch on this loot object.
        /// </summary>
        /// <returns>Message of loot object.</returns>
        public override string GetLootMessage()
        {
            return AvailableToLoot() ? defaultLootMessage : alternativeLootMessage;
        }
        #endregion

        #region [Getter / Setter]
        public GameObject GetObjectMesh()
        {
            return objectMesh;
        }

        public void SetObjectMesh(GameObject value)
        {
            objectMesh = value;
        }

        public LootMode GetLootMode()
        {
            return lootMode;
        }

        public void SetLootMode(LootMode value)
        {
            lootMode = value;
        }

        public string GetLootButton()
        {
            return lootButton;
        }

        public void SetLootButton(string value)
        {
            lootButton = value;
        }

        public bool GetAllowMultipleLoot()
        {
            return allowMultipleLoot;
        }

        public void SetAllowMultipleLoot(bool value)
        {
            allowMultipleLoot = value;
        }

        public int GetMultipleLootCount()
        {
            return multipleLootCount;
        }

        public void SetMultipleLootCount(int value)
        {
            multipleLootCount = value;
        }

        public float GetReActivateDelay()
        {
            return reActivateDelay;
        }

        public void SetReActivateDelay(float value)
        {
            reActivateDelay = value;
        }

        public string GetDefaultLootMessage()
        {
            return defaultLootMessage;
        }

        public void SetDefaultLootMessage(string value)
        {
            defaultLootMessage = value;
        }

        public string GetAlternativeLootMessage()
        {
            return alternativeLootMessage;
        }

        public void SetAlternativeLootMessage(string value)
        {
            alternativeLootMessage = value;
        }

        public AudioClip GetLootSound()
        {
            return lootSound;
        }

        public void SetLootSound(AudioClip value)
        {
            lootSound = value;
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