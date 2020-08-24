/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using UnityEngine;

namespace AuroraFPSRuntime
{
    [RequireComponent(typeof(FPController))]
    public partial class FPHealth : CharacterHealth
    {
        // Base first person health properties.
        [SerializeReference] private CameraSwitcher onDeadCameraSwitcher = new StandardCameraSwitcher();
        [SerializeReference] private CameraSwitcher onWakeUpCameraSwitcher = new StandardCameraSwitcher();

        // Stored required properties.
        private FPController controller;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            controller = GetComponent<FPCController>();
            OnDeadCallback += () => controller.SetEnabled(false);
            OnWakeUpCallback += () => controller.SetEnabled(true);

            onDeadCameraSwitcher.Initialize(this);
            onWakeUpCameraSwitcher.Initialize(this);
            OnDeadCallback += onDeadCameraSwitcher.OnSwitch;
            OnWakeUpCallback += onWakeUpCameraSwitcher.OnSwitch;

            controller.OnGroundedCallback += ControllerVelocityDamage;
        }

        protected virtual void ControllerVelocityDamage()
        {
            Vector3 velocity = Vector3.zero;
            velocity.y = controller.GetVelocity().y;
            VelocityDamageHandler(velocity.magnitude);
        }
    }
}