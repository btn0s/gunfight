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

namespace AuroraFPSRuntime.AI
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class AIAutoCroucher : MonoBehaviour
    {
        // Auto crouch properties.
        [SerializeField] private AnimatorValue crouchParameter = "IsCrouching";
        [SerializeField] private LayerMask crouchObstacleLayer = Physics.AllLayers;

        // Stored required components.
        private Animator animator;
        private CapsuleCollider capsuleCollider;

        // Stored required properties.
        private bool isCrouching;
        private bool isCrouched = true;
        private float storedColliderHeight;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            capsuleCollider = GetComponent<CapsuleCollider>();

            OnCrouchCallback += (value) => animator.SetBool(crouchParameter.GetNameHash(), value);
        }

        /// <summary>
        /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void FixedUpdate()
        {
            CrouchHandler();
        }

        /// <summary>
        /// Automactically detect crouch obstacles.
        /// </summary>
        protected virtual void CrouchHandler()
        {
            RaycastHit hitInfo;
            float range = (capsuleCollider.height / 2.0f) + 0.1f;
            if (!isCrouching && Physics.SphereCast(GetCenter(), capsuleCollider.radius, Vector3.up, out hitInfo, range, crouchObstacleLayer, QueryTriggerInteraction.Ignore) && isCrouched)
            {
                isCrouching = true;
                isCrouched = !isCrouching;
                OnCrouchCallback?.Invoke(true);
            }
            else if (isCrouching && !Physics.SphereCast(GetCenter(), capsuleCollider.radius, Vector3.up, out hitInfo, range, crouchObstacleLayer, QueryTriggerInteraction.Ignore) && !isCrouched)
            {
                isCrouching = false;
                isCrouched = !isCrouching;
                OnCrouchCallback?.Invoke(false);
            }
        }

        /// <summary>
        /// Calculate and save AI collider center vector.
        /// </summary>
        private Vector3 GetCenter()
        {
            return transform.TransformPoint(capsuleCollider.center);
        }

        #region [Event Callback Functions]
        /// <summary>
        /// On crouch callback function.
        /// OnCrouchCallback called when AI being automatycally crouch.
        /// </summary>
        /// <param name="bool">Crouch state.</param>
        public event Action<bool> OnCrouchCallback;
        #endregion
    }
}