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
    public interface IBullet
    {
        string GetModelName();

        int GetDamage();

        int GetBallsNumber();

        float GetBallsVariance();

        DecalMapping GetDecalMapping();
    }
}