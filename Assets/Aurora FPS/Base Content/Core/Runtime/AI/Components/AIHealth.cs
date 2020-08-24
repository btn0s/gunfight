/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using IEnumerator = System.Collections.IEnumerator;
using UnityEngine;

namespace AuroraFPSRuntime.AI
{
    [RequireComponent(typeof(Animator))]
    public class AIHealth : CharacterHealth
    {
        // AI animation properties.
        [SerializeField] private AnimatorState hitReactionState = "Hit Reaction";

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        protected virtual void Start()
        {
            // If AdaptiveAIRagdoll contained in the controller, then register the necessary callbacks.
            Animator animator = GetComponent<Animator>();
            OnTakeDamageCallback += _ => animator.PlayInFixedTime(hitReactionState.GetNameHash(), hitReactionState.GetLayer(), hitReactionState.GetFixedTime());

            // AICore enabled necessary callbacks.
            AICore core = GetComponent<AICore>();
            OnDeadCallback += () => core.Enabled(false);
            OnWakeUpCallback += () => core.Enabled(true);
        }


        #region [Getter / Setter]
        public AnimatorState GetHitReactionState()
        {
            return hitReactionState;
        }

        public void SetHitReactionState(AnimatorState value)
        {
            hitReactionState = value;
        }
        #endregion
    }
}