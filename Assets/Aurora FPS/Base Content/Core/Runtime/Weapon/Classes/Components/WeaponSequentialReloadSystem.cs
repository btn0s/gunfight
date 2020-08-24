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
    [RequireComponent(typeof(Animator))]
    public class WeaponSequentialReloadSystem : WeaponReloadSystem
    {
        // Sequential reload properties.
        [Header("Animation Properties")]
        [SerializeField] private float startTime = 1.0f;
        [SerializeField] private float iterationTime = 1.25f;
        [SerializeField] private float endTime = 1.0f;
        [SerializeField] private AnimatorState startState = "Start Reload";
        [SerializeField] private AnimatorState iterationState = "Iteration Reload";
        [SerializeField] private AnimatorState endState = "End Reload";

        // Stored required components.
        private Animator animator;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            animator = GetComponent<Animator>();
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

            animator.CrossFadeInFixedTime(startState);
            yield return new WaitForSeconds(startTime);

            WaitForSeconds waitForIterationTime = new WaitForSeconds(iterationTime);
            int requiredBulletCount = GetMaxAmmoCount() - GetAmmoCount();
            while (requiredBulletCount > 0 && !ClipsIsEmpty())
            {
                animator.CrossFadeInFixedTime(iterationState);
                yield return waitForIterationTime;
                SetClipCount(GetClipCount() - 1);
                requiredBulletCount--;
                SetAmmoCount(GetAmmoCount() + 1);
                OnIterationReloadCallback?.Invoke();
            }

            animator.CrossFadeInFixedTime(endState);
            yield return new WaitForSeconds(endTime);

            OnEndReloadCallback?.Invoke();
        }

        #region [Event Callback Functions]
        /// <summary>
        /// On start reload callback function.
        /// OnStartReload called every time when weapon start reloading.
        /// </summary>
        public override event Action OnStartReloadCallback;

        /// <summary>
        /// On iteration reload callback.
        /// OnIterationReloadCallback called every iteration time.
        /// </summary>
        public event Action OnIterationReloadCallback;

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
        public float GetStartTime()
        {
            return startTime;
        }

        public void SetStartTime(float value)
        {
            startTime = value;
        }

        public float GetIterationTime()
        {
            return iterationTime;
        }

        public void SetIterationTime(float value)
        {
            iterationTime = value;
        }

        public float GetEndTime()
        {
            return endTime;
        }

        public void SetEndTime(float value)
        {
            endTime = value;
        }

        public AnimatorState GetStartState()
        {
            return startState;
        }

        public void SetStartState(AnimatorState value)
        {
            startState = value;
        }

        public AnimatorState GetIterationState()
        {
            return iterationState;
        }

        public void SetIterationState(AnimatorState value)
        {
            iterationState = value;
        }

        public AnimatorState GetEndState()
        {
            return endState;
        }

        public void SetEndState(AnimatorState value)
        {
            endState = value;
        }
        #endregion
    }
}