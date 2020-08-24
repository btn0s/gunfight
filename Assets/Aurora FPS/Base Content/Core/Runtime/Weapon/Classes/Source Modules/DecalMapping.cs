/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using UnityEngine;
using AuroraFPSRuntime.Serialization.Collections;

namespace AuroraFPSRuntime
{
    [CreateAssetMenu(fileName = "Decal Mapping", menuName = AuroraFPSProduct.Name + "/Mapping/Weapon/Decal", order = 125)]
    public class DecalMapping : ScriptableMappingDictionary<DictionaryObjectToPoolObjectStorage, Object, PoolObjectStorage> { }
}