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
    [System.Flags]
    public enum SwitchWeaponMode
    {
        Disabled = 0,
        Keyboard = 1 << 0,
        MouseWheel = 1 << 1,
        Both = ~0
    }
}