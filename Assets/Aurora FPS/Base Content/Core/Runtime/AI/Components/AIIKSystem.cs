/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using UnityEngine;

namespace AuroraFPSRuntime.AI
{
    [RequireComponent(typeof(AICore))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class AIIKSystem : CharacterIKSystem
    {
        private AICore controller;
        private Animator animator;
        private CapsuleCollider capsuleCollider;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            controller = GetComponent<AICore>();
            animator = GetComponent<Animator>();
            capsuleCollider = GetComponent<CapsuleCollider>();

            controller.OnEnabledCallback += enabled => { this.enabled = enabled; };
        }

        public override Vector3 GetVelocity()
        {
            return controller.GetVelocity();
        }

        public override Animator GetAnimator()
        {
            return animator;
        }

        public override CapsuleCollider GetCapsuleCollider()
        {
            return capsuleCollider;
        }

        #region [Getter / Setter]
        public AICore GetAIController()
        {
            return controller;
        }

        protected void SetAIController(AICore value)
        {
            controller = value;
        }

        protected void SetAnimator(Animator value)
        {
            animator = value;
        }

        protected void SetCapsuleCollider(CapsuleCollider value)
        {
            capsuleCollider = value;
        }
        #endregion
    }
}