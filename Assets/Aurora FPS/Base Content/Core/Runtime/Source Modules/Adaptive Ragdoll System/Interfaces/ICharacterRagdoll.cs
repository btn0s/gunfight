/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

namespace AuroraFPSRuntime
{
    public interface ICharacterRagdoll
    {
        /// <summary>
        /// Activate character ragdoll.
        /// </summary>
        void RagdollIn();

        /// <summary>
        /// Deactivate character ragdoll and play get up animation.
        /// </summary>
        void GetUp();
    }
}