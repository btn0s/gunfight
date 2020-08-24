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
using System.Collections.Generic;
using UnityEngine;

namespace AuroraFPSRuntime.AI
{
    public class AIFieldOfView : MonoBehaviour
    {
        /// <summary>
        /// Visible targets array of AI field of view.
        /// </summary>
        protected readonly List<Transform> VisibleTargets = new List<Transform>();

        // Base field of view properties.
        [Header("Mask's")]
        [SerializeField] private LayerMask targetMask;
        [SerializeField] private LayerMask obstacleMask;

        [Header("View Properties")]
        [SerializeField] private float viewRadius = 10.0f;
        [SerializeField] private float viewAngle = 120.0f;
        [SerializeField] private float viewOffset = 1.0f;

        [Header("Other")]
        [SerializeField] private bool onlyAlive = true;
        [SerializeField] private float updateRate = 0.2f;

        // Stored required properties.
        private CoroutineObject<float> searchTargetsCoroutine;
        private bool isFindedTargets = false;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            searchTargetsCoroutine = new CoroutineObject<float>(this);

            // Switch AIFieldOfView enabled state when AI core enabled state will changing.
            AICore core = GetComponent<AICore>();
            if (core != null)
            {
                core.OnEnabledCallback += enabled => { this.enabled = enabled; };
            }
        }

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        protected virtual void OnEnable()
        {
            searchTargetsCoroutine.Start(SearchTargets, updateRate);
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        protected virtual void OnDisable()
        {
            searchTargetsCoroutine.Stop();
        }

        /// <summary>
        /// Processing of the searching visible targets.
        /// </summary>
        protected virtual void SearchTargetProcessing()
        {
            VisibleTargets.Clear();
            Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask, QueryTriggerInteraction.Ignore);

            for (int i = 0, length = targetsInViewRadius.Length; i < length; i++)
            {
                Transform target = targetsInViewRadius[i].transform;

                // If only "alive condition", check that the target has a health component and it is alive
                // else skip this one and check next target.
                if (onlyAlive)
                {
                    IHealth health = target.GetComponent<IHealth>();
                    if (health == null || !health.IsAlive())
                    {
                        continue;
                    }
                }

                Vector3 direction = (target.position - transform.position).normalized;
                if (Vector3.Angle(transform.forward, direction) < (viewAngle / 2))
                {
                    Vector3 offset = Vector3.up * viewOffset;
                    Vector3 origin = transform.position + offset;
                    direction += offset;
                    float distance = Vector3.Distance(origin, direction);
                    if (!Physics.Raycast(origin, direction, distance, obstacleMask))
                    {
                        VisibleTargets.Add(target);
                        if (!isFindedTargets)
                        {
                            OnFindTargetsCallback?.Invoke();
                            isFindedTargets = true;
                        }
                    }
                }
            }

            EventCallbackHandler();
        }

        /// <summary>
        /// Search targets coroutine.
        /// </summary>
        private IEnumerator SearchTargets(float updateRate)
        {
            WaitForSeconds update = new WaitForSeconds(updateRate);
            while (true)
            {
                yield return update;
                SearchTargetProcessing();
            }
        }

        private void EventCallbackHandler()
        {
            if (VisibleTargets.Count == 0 && isFindedTargets)
            {
                OnLostTargetsCallback?.Invoke();
                isFindedTargets = false;
            }
        }

        #region [Event Callback Functions]
        /// <summary>
        /// On find targets callback function.
        /// Called once when field of view find targets.
        /// </summary>
        public event Action OnFindTargetsCallback;

        /// <summary>
        /// On lost targets callback function.
        /// Called once when field of view lost targets.
        /// </summary>
        public event Action OnLostTargetsCallback;
        #endregion

        #region [Getter / Setter]
        public LayerMask GetTargetMask()
        {
            return targetMask;
        }

        public void SetTargetMask(LayerMask value)
        {
            targetMask = value;
        }

        public LayerMask GetObstacleMask()
        {
            return obstacleMask;
        }

        public void SetObstacleMask(LayerMask value)
        {
            obstacleMask = value;
        }

        public float GetViewRadius()
        {
            return viewRadius;
        }

        public void SetViewRadius(float value)
        {
            viewRadius = value;
        }

        public float GetViewAngle()
        {
            return viewAngle;
        }

        public void SetViewAngle(float value)
        {
            viewAngle = value;
        }

        public float GetViewOffset()
        {
            return viewOffset;
        }

        public void SetViewOffset(float value)
        {
            viewOffset = value;
        }

        public bool OnlyAlive()
        {
            return onlyAlive;
        }

        public void OnlyAlive(bool value)
        {
            onlyAlive = value;
        }

        public float GetUpdateRate()
        {
            return updateRate;
        }

        public void SetUpdateRate(float value)
        {
            updateRate = value;
        }

        public List<Transform> GetVisibleTargets()
        {
            return VisibleTargets;
        }

        public Transform GetVisibleTarget(int index)
        {
            return VisibleTargets[index];
        }

        public int GetVisibleTargetCount()
        {
            return VisibleTargets.Count;
        }

        public bool HasVisibleTargets()
        {
            return VisibleTargets.Count > 0;
        }
        #endregion
    }
}