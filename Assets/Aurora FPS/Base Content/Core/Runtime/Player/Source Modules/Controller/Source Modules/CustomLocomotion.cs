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
    public abstract class CustomLocomotion
    {
        protected FPCController controller;

        /// <summary>
        /// Intialize custom locomotion system.
        /// Called once when the controller instance is being loaded.
        /// </summary>
        /// <param name="controller">First person controller reference.</param>
        public virtual void Initialize(FPCController controller)
        {
            this.controller = controller;
        }

        /// <summary>
        /// Calculate custom locomotion speed.
        /// </summary>
        /// <returns>Calculated controller move speed.</returns>
        public abstract float CalculateSpeed();

        /// <summary>
        /// Calculate custom locomotion direction.
        /// </summary>
        /// <param name="smoothInputVector">Calculated and smoothed input data Vector2 representation.</param>
        /// <returns>Calculated controller move direction.</returns>
        public abstract Vector3 CalculateDirection(Vector2 smoothInputVector);

        /// <summary>
        /// Calculate final character controller move vector.
        /// </summary>
        /// <param name="currentMoveVector">Current character controller move vertor.</param>
        /// <param name="nextMoveVector">Calculated and smoothed next character controller move vector.</param>
        /// <returns>Final character controller move vector.</returns>
        public virtual Vector3 CalculateMoveVector(Vector3 currentMoveVector, Vector3 nextMoveVector)
        {
            return nextMoveVector;
        }

        /// <summary>
        /// Use a physics gravity effect on the controller.
        /// </summary>
        public virtual bool UseGravity()
        {
            return false;
        }

        /// <summary>
        /// Check controller grounded.
        /// </summary>
        public virtual bool CheckGround()
        {
            return false;
        }
    }
}
