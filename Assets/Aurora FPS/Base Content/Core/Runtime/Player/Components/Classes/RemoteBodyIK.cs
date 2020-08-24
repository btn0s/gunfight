/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using UnityEngine;

namespace AuroraFPSRuntime
{
    [RequireComponent(typeof(Animator))]
    public class RemoteBodyIK : CharacterIKSystem
    {
        // Base remote body IK properties.
        [Header("Remote Body Properties")]
        [SerializeReference] private FPController controller;

        // Stored required properties.
        private Animator animator;
    
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            controller = transform.root.GetComponent<FPController>();
            animator = GetComponent<Animator>();
        }

        #region [CharacterIKSystem Implementation]
        public override Animator GetAnimator()
        {
            return animator;
        }

        public override CapsuleCollider GetCapsuleCollider()
        {
            return null;
        }

        public override Vector3 GetVelocity()
        {
            return controller.GetVelocity();
        }

        #endregion

        #region [Getter / Setter]


        protected void SetAnimator(Animator value)
        {
            animator = value;
        }


        public FPController GetController()
        {
            return controller;
        }

        protected void SetController(FPController value)
        {
            controller = value;
        }
        #endregion
    }
}