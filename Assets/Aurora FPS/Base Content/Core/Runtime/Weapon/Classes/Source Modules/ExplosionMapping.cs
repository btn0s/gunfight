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
    [CreateAssetMenu(fileName = "Explosion Mapping", menuName = AuroraFPSProduct.Name + "/Mapping/Weapon/Explosion", order = 126)]
    public class ExplosionMapping : ScriptableMappingArray<ExplosionProperty> { }
}