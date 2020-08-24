/* ==================================================================
   ---------------------------------------------------
   Project   :    Beyond
   Publisher :    -
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================== */

namespace AuroraFPSRuntime
{
    public abstract partial class CharacterRagdoll
    {
        /// <summary>
        /// //Possible states of the ragdoll
        /// </summary>
        public enum RagdollState
        {
            /// <summary>
            /// Mecanim is fully in control
            /// </summary>
            Animated,

            /// <summary>
            /// Mecanim turned off, physics controls the ragdoll
            /// </summary>
            Ragdolled,

            /// <summary>
            /// Mecanim in control, but LateUpdate() is used to partially blend in the last ragdolled pose
            /// </summary>
            BlendToAnim,
        }
    }
}