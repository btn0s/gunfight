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
    public interface IControllerInput
    {
        Vector2 GetMovementInput();
        
        float GetHorizontalInput();

        float GetVerticalInput();
    }
}