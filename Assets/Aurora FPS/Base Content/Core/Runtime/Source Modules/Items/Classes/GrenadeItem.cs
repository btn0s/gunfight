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
    [CreateAssetMenu(fileName = "Grenade Item", menuName = AuroraFPSProduct.Name + "/Items/Grenade", order = 124)]
    public class GrenadeItem : WeaponItem
    {
        public const string GroupName = "Grenades";

        public GrenadeItem()
        {
            SetGroup(GroupName);
        }
    }
}