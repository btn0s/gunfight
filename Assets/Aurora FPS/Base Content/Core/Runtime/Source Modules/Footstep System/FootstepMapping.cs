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
using Object = UnityEngine.Object;

namespace AuroraFPSRuntime
{
    [CreateAssetMenu(fileName = "Footstep Mapping", menuName = AuroraFPSProduct.Name + "/Mapping/Player/Footstep", order = 121)]
    public class FootstepMapping : ScriptableMappingDictionary<DictionaryObjectToFootstepProperty, Object, FootstepSounds> { }
}