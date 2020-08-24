/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using AuroraFPSRuntime.Serialization.Collections;
using UnityEngine;

namespace AuroraFPSRuntime
{
    [CreateAssetMenu(fileName = "Recoil Mapping", menuName = AuroraFPSProduct.Name + "/Mapping/Weapon/Recoil", order = 125)]
    public class RecoilMapping : ScriptableMappingDictionary<DictionaryControllerStateToRecolPattern, ControllerState, RecoilPattern> { }
}