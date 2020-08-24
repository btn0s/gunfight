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
    public abstract class FPController : ControllerBase
    {
        // First person camera look instance.
        [SerializeField] protected CameraControl cameraControl;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            cameraControl.Initialize(this);
        }

        #region [Getter / Setter]
        public CameraControl GetCameraControl()
        {
            return cameraControl;
        }

        public void SetCameraControl(CameraControl value)
        {
            cameraControl = value;
        }
        #endregion
    }
}