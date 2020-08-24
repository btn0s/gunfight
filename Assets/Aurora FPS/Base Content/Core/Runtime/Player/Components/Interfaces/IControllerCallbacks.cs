﻿/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using System;
using UnityEngine;

namespace AuroraFPSRuntime
{
    public interface IControllerCallbacks
    {
        /// <summary>
        /// Called when controller moved.
        /// <param name="Vector3">Velocity of the controller.</param>
        /// </summary>
        event Action<Vector3> OnMoveCallback;

        /// <summary>
        /// Called when controller being grounded.
        /// </summary>
        event Action OnGroundedCallback;

        /// <summary>
        /// Called when controller jumped.
        /// </summary>
        event Action OnJumpCallback;

        /// <summary>
        /// Called when controller being enabled.
        /// </summary>
        event Action OnEnableCallback;
        
        /// <summary>
        /// Called when controller being disabled.
        /// </summary>
        event Action OnDisableCallback;
    }
}