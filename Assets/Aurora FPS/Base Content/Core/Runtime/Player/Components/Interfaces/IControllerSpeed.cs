/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

namespace AuroraFPSRuntime
{
    public interface IControllerSpeed
    {
        float GetSpeed();

        float GetWalkSpeed();

        float GetRunSpeed();

        float GetSprintSpeed();

        float GetBackwardSpeedPersent();

        float GetSideSpeedPersent();
    }
}