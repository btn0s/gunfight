/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using UnityEngine;
using System.Collections;

namespace AuroraFPSRuntime
{
    [CameraSwitcherSettings("Standard")]
    public class StandardCameraSwitcher : CameraSwitcher
    {
        // Base editable properties.
        [SerializeField] private Transform enableCamera;
        [SerializeField] private Transform disableCamera;

        /// <summary>
        /// Called when character die.
        /// </summary>
        public override void OnSwitch()
        {
            disableCamera.gameObject.SetActive(false);
            enableCamera.gameObject.SetActive(true);
        }

        #region [Getter / Setter]
        public Transform GetEnableCamera()
        {
            return enableCamera;
        }

        public void SetEnableCamera(Transform value)
        {
            enableCamera = value;
        }

        public Transform GetDisableCamera()
        {
            return disableCamera;
        }

        public void SetDisableCamera(Transform value)
        {
            disableCamera = value;
        }
        #endregion
    }
}

