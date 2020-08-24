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
    [CreateAssetMenu(fileName = "Climbstep Mapping", menuName = AuroraFPSProduct.Name + "/Mapping/Player/Climbstep", order = 122)]
    public class ClimbstepMapping : ScriptableMappingDictionary<DictionaryObjectToArrayAudioClip, Object, AudioClipStorage> { }
}