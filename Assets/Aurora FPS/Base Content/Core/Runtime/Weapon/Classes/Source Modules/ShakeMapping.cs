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
    [CreateAssetMenu(fileName = "Camera Shake Mapping", menuName = AuroraFPSProduct.Name + "/Mapping/Camera/Shake", order = 125)]
    public class ShakeMapping : ScriptableMappingDictionary<DictionaryStringToShake, string, CameraShake.Shake> { }
}