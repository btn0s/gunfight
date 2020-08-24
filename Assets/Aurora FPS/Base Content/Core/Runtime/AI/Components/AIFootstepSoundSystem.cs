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
    public class AIFootstepSoundSystem : FootstepSoundSystem
    {
        // Stored require components.
        private AICore core;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            core = GetComponent<AICore>();

            core.OnEnabledCallback += enabled => { this.enabled = enabled; };
        }

        public override Vector3 GetVelocity()
        {
            return core.GetVelocity();
        }

        public override bool IsGrounded()
        {
            return true;
        }

        public override bool IsJumped()
        {
            return false;
        }
    }
}