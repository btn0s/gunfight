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
    public class FPDynamicRagdoll : DynamicRagdoll
    {
        // Base first person dynamic ragdoll properties.
        [SerializeField] private Transform character;


        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            OnBlendCompleteCallback += FixCharacterPosition;
        }

        /// <summary>
        /// Override this method to initialize animator component of the ragdoll character.
        /// Use GetComponent<Animator>() method.
        /// </summary>
        /// <param name="animator">Animator component of the ragdoll character.</param>
        public override void CopyAnimator(out Animator animator)
        {
            animator = character.GetComponent<Animator>();
        }

        /// <summary>
        /// Override this method to initialize transform component of the ragdoll character.
        /// </summary>
        /// <param name="transform">Transform component of the ragdoll character.</param>
        public override void CopyRagdollTransform(out Transform transform)
        {
            transform = character.transform;
        }

        /// <summary>
        /// Switch kinematic enabled state of the each rigidbody component of the character.
        /// </summary>
        /// <param name="enabled">Enabled state of parts.</param>
        protected override void BonesKinematic(bool enabled)
        {
            base.BonesKinematic(enabled);
            Collider[] colliders = character.GetComponentsInChildren<Collider>();
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = !enabled;
            }
        }

        /// <summary>
        /// Calculate body direction, when character get up.
        /// </summary>
        /// <returns>Body direction when character get up.</returns>
        public override Vector3 CalculateBodyDirection()
        {
            return character.transform.forward;
        }

        /// <summary>
        /// Fix character position when character blended to animator.
        /// </summary>
        public virtual void FixCharacterPosition()
        {
            if (GetState() == State.Animated)
                character.position = transform.position;
        }

        #region [Getter / Setter]
        public Transform GetCharacter()
        {
            return character;
        }

        public void SetCharacter(Transform value)
        {
            character = value;
        }
        #endregion
    }
}