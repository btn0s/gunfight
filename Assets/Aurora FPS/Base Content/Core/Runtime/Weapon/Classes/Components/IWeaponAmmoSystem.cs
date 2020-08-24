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
    public interface IWeaponAmmoSystem
    {
        void AddAmmo(int value);

        void AmmoSubtraction();

        bool HasAmmo();

        int GetAmmoCount();

        int GetMaxAmmoCount();
    }
}