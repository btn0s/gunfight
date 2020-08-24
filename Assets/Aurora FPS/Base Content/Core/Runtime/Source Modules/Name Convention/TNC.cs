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
    /// <summary>
    /// Tag name conventions of the all Aurora FPS tags.
    /// </summary>
    public static class TNC
    {
        public const string Player = "Player";
        public const string Weapon = "Weapon";
        public const string Camera = "FPCamera";
        public const string CameraLayer = "FPWeaponLayer";
        public const string AI = "AI";
        public const string Terrain = "Terrain";

        public readonly static string[] AllTags = new string[] { Player, Weapon, Camera, CameraLayer, AI, Terrain };
    }
}