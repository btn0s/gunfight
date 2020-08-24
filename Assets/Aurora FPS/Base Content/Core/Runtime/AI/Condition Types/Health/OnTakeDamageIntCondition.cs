/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using UnityEngine;

namespace AuroraFPSRuntime.AI
{
    [RequireComponent(typeof(AIHealth))]
    [ConditionOptions(path = "Health/On Take Damage(int)", description = "Called every time when AI take specific(value) damage.")]
    public class OnTakeDamageIntCondition : OnTakeDamageCondition
    {
        [SerializeField] private int value;

        /// <summary>
        /// Register health take damage callback function.
        /// </summary>
        protected override void RegisterCallbackFunction()
        {
            health.OnTakeDamageCallback += value =>
            {
                InvokeDefaultCallback();
            };
        }
    }
}