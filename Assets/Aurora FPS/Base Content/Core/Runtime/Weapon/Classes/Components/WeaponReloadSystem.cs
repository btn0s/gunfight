/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using System;
using System.Collections;
using UnityEngine;

namespace AuroraFPSRuntime
{
    [RequireComponent(typeof(WeaponAnimationSystem))]
    [RequireComponent(typeof(AudioSource))]
    public abstract class WeaponReloadSystem : WeaponAmmoSystem
    {
        // Base weapon reloading properties.
        [Header("Ammo Properties")]
        [SerializeField] private int clipCount = 256;
        [SerializeField] private int maxClipCount = 512;

        [Header("Reload Properties")]
        [SerializeField] private bool smartReloading = true;
        [SerializeField] private bool autoReloading = false;

        // Stored required properties.
        protected bool isReloading;
        private CoroutineObject reloadCoroutine;

        // Stored required components.
        private CameraControl cameraLook;


        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            reloadCoroutine = new CoroutineObject(this);

            cameraLook = transform.root.GetComponent<FPController>().GetCameraControl();

            OnStartReloadCallback += () => isReloading = true;
            OnStartReloadCallback += () => { if (cameraLook.IsZooming()) { cameraLook.ZoomOut(); } };

            OnEndReloadCallback += () => isReloading = false;
        }

        
        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void Update()
        {
            if (!ClipsIsEmpty() && CheckSmartReloading() && (CheckAutoReloading() || AInput.GetButtonDown(INC.Reload)))
            {
                reloadCoroutine.Start(ReloadProcessing);
            }
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        protected virtual void OnDisable()
        {
            if(isReloading)
            {
                reloadCoroutine.Stop();
                isReloading = false;
            }
        }

        /// <summary>
        /// Weapon reload processing. 
        /// </summary>
        protected abstract IEnumerator ReloadProcessing();

        /// <summary>
        /// Default calculate weapon bullet and clip.
        /// Saves the number of bullets in the rechargeable clip.
        /// </summary>
        protected void DefaultCalculate()
        {
            bool conditions = clipCount >= GetMaxAmmoCount();
            int previousAmmoCount = GetAmmoCount();
            SetAmmoCount(conditions ? GetMaxAmmoCount() : clipCount + GetAmmoCount());
            clipCount = conditions ? clipCount - (GetMaxAmmoCount() - previousAmmoCount) : 0;
        }

        /// <summary>
        /// Realistic calculate weapon bullet and clip.
        /// Not saves the number of bullets in the rechargeable clip.
        /// </summary>
        protected void RealisticCalculate()
        {
            bool conditions = clipCount >= GetMaxAmmoCount();
            SetAmmoCount( conditions ? GetMaxAmmoCount() : clipCount + GetAmmoCount());
            clipCount = conditions ? clipCount - GetMaxAmmoCount() : 0;
        }

        public bool CheckSmartReloading()
        {
            if(smartReloading)
            {
                return GetAmmoCount() < GetMaxAmmoCount();
            }
            return true;
        }

        public bool CheckAutoReloading()
        {
            if(autoReloading)
            {
                return AmmoIsEmpty();
            }
            return false;
        }

        /// <summary>
        /// Add bullets to weapon.
        /// </summary>
        public override void AddAmmo(int value)
        {
            if (clipCount + value <= maxClipCount)
                clipCount += value;
            else
                clipCount = maxClipCount;
        }

        public override bool IsFull()
        {
            return clipCount == maxClipCount;
        }


        #region [Event Callback Functions]
        /// <summary>
        /// On start reload callback function.
        /// OnStartReload called every time when weapon start reloading.
        /// </summary>
        /// <param name="float">Time to reload.</param>
        public abstract event Action OnStartReloadCallback;

        /// <summary>
        /// On end reload callback function.
        /// OnEndReloadCallback called every time when weapon end of reloading.
        /// </summary>
        public abstract event Action OnEndReloadCallback;

        /// <summary>
        /// On reload callback function.
        /// OnBaseReloadCallback called when weapon start base reloading (means that rechargeable clip isn't empty).
        /// </summary>
        public abstract event Action OnBaseReloadCallback;

        /// <summary>
        /// On full reload callback function.
        /// OnFullReloadCallback called when weapon start full reloading (means that rechargeable clip is empty).
        /// </summary>
        public abstract event Action OnFullReloadCallback;
        #endregion

        #region [Getter / Setter]
        public bool IsReloading()
        {
            return isReloading;
        }

        protected void IsReloading(bool value)
        {
            isReloading = value;
        }

        public int GetClipCount()
        {
            return clipCount;
        }

        protected void SetClipCount(int value)
        {
            if (value <= maxClipCount)
                clipCount = value;
            else
                clipCount = maxClipCount;
        }

        public int GetMaxClipCount()
        {
            return maxClipCount;
        }

        public void SetMaxClipCount(int value)
        {
            maxClipCount = value;
        }

        public bool ClipsIsEmpty()
        {
            return clipCount <= 0;
        }

        public bool AmmoIsEmpty()
        {
            return GetAmmoCount() <= 0;
        }

        public bool SmartReloading()
        {
            return smartReloading;
        }

        public void SmartReloading(bool value)
        {
            smartReloading = value;
        }

        public bool AutoReloading()
        {
            return autoReloading;
        }

        public void AutoReloading(bool value)
        {
            autoReloading = value;
        }
        #endregion
    }
}