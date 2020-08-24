/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using UnityEngine;

namespace AuroraFPSRuntime
{
    [RequireComponent(typeof(Camera))]
    public class CameraDuplicator : MonoBehaviour
    {
        // Base camera duplicator properties.
        [SerializeField] private Camera targetCamera;
        
        // Stored required properties.
        private Camera cameraComponent;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            cameraComponent = GetComponent<Camera>();
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void Update()
        {
            cameraComponent.fieldOfView = targetCamera.fieldOfView;
        }

        #region [Getter / Setter]
        public Camera GetTargetCamera()
        {
            return targetCamera;
        }

        public void SetTargetCamera(Camera value)
        {
            targetCamera = value;
        }
        #endregion
    }
}