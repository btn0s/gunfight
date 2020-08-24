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
    [CreateAssetMenu(fileName = "Weapon Item", menuName = AuroraFPSProduct.Name + "/Items/Weapon", order = 123)]
    public class WeaponItem : SceneItem
    {
        [SerializeField] private string group;

        #region [Getter / Setter]
        public string GetGroup()
        {
            return group;
        }

        public void SetGroup(string value)
        {
            group = value;
        }
        #endregion
    }
}