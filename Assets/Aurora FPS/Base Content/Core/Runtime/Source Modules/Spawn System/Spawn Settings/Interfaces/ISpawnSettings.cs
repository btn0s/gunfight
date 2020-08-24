/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */


using System.Collections;

namespace AuroraFPSRuntime
{
    public interface ISpawnSettings
    {
        IEnumerator ExecuteSpawn(SpawnManager spawnManager);
    }
}