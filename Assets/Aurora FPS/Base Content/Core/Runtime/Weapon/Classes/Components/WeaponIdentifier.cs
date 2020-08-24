/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using UnityEngine;

namespace AuroraFPSRuntime
{
    public class WeaponIdentifier : MonoBehaviour
    {
        [SerializeField] WeaponItem item;

        public WeaponItem GetWeaponItem()
        {
            return item;
        }

        public void SetWeaponItem(WeaponItem value)
        {
            item = value;
        }
    }
}