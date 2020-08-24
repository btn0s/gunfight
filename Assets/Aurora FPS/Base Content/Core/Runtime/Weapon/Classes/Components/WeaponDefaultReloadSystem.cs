/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using System;
using System.Collections;
using UnityEngine;

namespace AuroraFPSRuntime
{
    public class WeaponDefaultReloadSystem : WeaponReloadSystem
    {
        public enum ReloadType
        {
            /// <summary>
            /// Default calculate weapon bullet and clip.
            /// Saves the number of bullets in the rechargeable clip.
            /// </summary>
            Default,

            /// <summary>
            /// Realistic calculate weapon bullet and clip.
            /// Not saves the number of bullets in the rechargeable clip.
            /// </summary>
            Realistic
        }

        // Default reload properties.
        [Header("Reload Type")]
        [SerializeField] private ReloadType reloadType = ReloadType.Default;

        [Header("Animation Properties")]
        [SerializeField] private float baseReloadTime = 3.0f;
        [SerializeField] private float fullReloadTime = 4.0f;
        [SerializeField] private AnimatorState baseReloadState = "Base Reload";
        [SerializeField] private AnimatorState fullReloadState = "Full Reload";

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            Animator animator = GetComponent<Animator>();

            // Register animation callbacks.
            OnBaseReloadCallback += () => animator.CrossFadeInFixedTime(baseReloadState);
            OnFullReloadCallback += () => animator.CrossFadeInFixedTime(fullReloadState);
        }

        /// <summary>
        /// Reload processing.
        /// </summary>
        protected override IEnumerator ReloadProcessing()
        {
            OnStartReloadCallback?.Invoke();

            if (!AmmoIsEmpty())
                OnBaseReloadCallback?.Invoke();
            else
                OnFullReloadCallback?.Invoke();

            float time = !AmmoIsEmpty() ? baseReloadTime : fullReloadTime;
            yield return new WaitForSeconds(time);

            switch (reloadType)
            {
                case ReloadType.Default:
                    DefaultCalculate();
                    break;
                case ReloadType.Realistic:
                    RealisticCalculate();
                    break;
            }

            OnEndReloadCallback?.Invoke();
        }

        #region [Event Callback Functions]
        /// <summary>
        /// On start reload callback function.
        /// OnStartReload called every time when weapon start reloading.
        /// </summary>
        public override event Action OnStartReloadCallback;

        /// <summary>
        /// On end reload callback function.
        /// OnEndReloadCallback called every time when weapon end of reloading.
        /// </summary>
        public override event Action OnEndReloadCallback;

        /// <summary>
        /// On reload callback function.
        /// OnBaseReloadCallback called when weapon start base reloading (means that rechargeable clip isn't empty).
        /// </summary>
        public override event Action OnBaseReloadCallback;

        /// <summary>
        /// On full reload callback function.
        /// OnFullReloadCallback called when weapon start full reloading (means that rechargeable clip is empty).
        /// </summary>
        public override event Action OnFullReloadCallback;
        #endregion

        #region [Getter / Setter]
        public ReloadType GetReloadType()
        {
            return reloadType;
        }

        public void SetReloadType(ReloadType value)
        {
            reloadType = value;
        }

        public float GetBaseReloadTime()
        {
            return baseReloadTime;
        }

        public void SetBaseReloadTime(float value)
        {
            baseReloadTime = value;
        }

        public float GetFullReloadTime()
        {
            return fullReloadTime;
        }

        public void SetFullReloadTime(float value)
        {
            fullReloadTime = value;
        }

        public AnimatorState GetBaseReloadState()
        {
            return baseReloadState;
        }

        public void SetBaseReloadState(AnimatorState value)
        {
            baseReloadState = value;
        }

        public AnimatorState GetFullReloadState()
        {
            return fullReloadState;
        }

        public void SetFullReloadState(AnimatorState value)
        {
            fullReloadState = value;
        }
        #endregion
    }
}