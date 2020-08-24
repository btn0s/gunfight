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
    [RequireComponent(typeof(FPController))]
    public class FPFootstepSoundSystem : FootstepSoundSystem
    {
        // Stored require components.
        private FPController controller;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            controller = GetComponent<FPController>();
        }

        public override Vector3 GetVelocity()
        {
            return controller.IsEnabled() ? controller.GetVelocity() : Vector3.zero;
        }

        public override bool IsGrounded()
        {
            return controller.IsGrounded();
        }

        public override bool IsJumped()
        {
            return controller.IsJumped();
        }
    }
}