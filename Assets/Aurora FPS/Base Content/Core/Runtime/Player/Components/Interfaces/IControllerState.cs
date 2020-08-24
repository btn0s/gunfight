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
    public interface IControllerState
    {
        ControllerState GetState();

        bool CompareState(ControllerState value);

        bool HasState(ControllerState value);
    }
}