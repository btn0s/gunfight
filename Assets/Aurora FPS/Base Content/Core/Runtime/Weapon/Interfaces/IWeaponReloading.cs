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
    public interface IWeaponReloadSystem
    {
        bool IsReloading();

        void BulletSubtraction();

        bool AddBullets(int value);

        bool AddClips(int value);
        
        int GetBulletCount();

        int GetClipCount();

        void SetMaxBulletCount(int value);

        int GetMaxBulletCount();

        void SetMaxClipCount(int value);

        int GetMaxClipCount();

        bool ClipsIsEmpty();

        bool BulletsIsEmpty();
    }
}