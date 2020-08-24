/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using System.Collections;
using UnityEngine;


namespace AuroraFPSRuntime
{
    public class FPCameraZoom : MonoBehaviour
    {
        [SerializeField] private Camera _FPCamera;
        [SerializeField] private Camera _FPWeaponLayer;
        [SerializeField] private float zoomFOVValue = 50.0f;
        [SerializeField] private float defaultFOVValue = 60.0f;
        [SerializeField] private float speed = 10.0f;

        private IEnumerator zoomProcessingCoroutine;

        /// <summary>
        /// LateUpdate is called after all Update functions have been called.
        /// </summary>
        protected virtual void LateUpdate()
        {
            if (AInput.GetButtonDown(INC.Zoom))
                StartZoom();

            if (AInput.GetButtonUp(INC.Zoom))
                StopZoom();
        }

        /// <summary>
        /// Zoom increase.
        /// </summary>
        public virtual void StartZoom()
        {
            if (zoomProcessingCoroutine != null)
                StopCoroutine(zoomProcessingCoroutine);

            zoomProcessingCoroutine = ZoomProcessing(zoomFOVValue);
            StartCoroutine(zoomProcessingCoroutine);
        }

        /// <summary>
        /// Zoom decrease.
        /// </summary>
        public virtual void StopZoom()
        {
            if (zoomProcessingCoroutine != null)
                StopCoroutine(zoomProcessingCoroutine);

            zoomProcessingCoroutine = ZoomProcessing(defaultFOVValue);
            StartCoroutine(zoomProcessingCoroutine);
        }

        /// <summary>
        /// Zoom processing.
        /// </summary>
        protected virtual IEnumerator ZoomProcessing(float targetFOV)
        {
            while (true)
            {
                _FPCamera.fieldOfView = Mathf.SmoothStep(_FPCamera.fieldOfView, targetFOV, speed * Time.deltaTime);
                _FPWeaponLayer.fieldOfView = Mathf.SmoothStep(_FPWeaponLayer.fieldOfView, targetFOV, speed * Time.deltaTime);

                if (AMath.Approximately(_FPCamera.fieldOfView, targetFOV))
                {
                    zoomProcessingCoroutine = null;
                    yield break;
                }
                yield return null;
            }
        }

        public Camera GetFPCamera()
        {
            return _FPCamera;
        }

        public void SetFPCamera(Camera value)
        {
            _FPCamera = value;
        }

        public Camera GetFPWeaponLayer()
        {
            return _FPWeaponLayer;
        }

        public void SetFPWeaponLayer(Camera value)
        {
            _FPWeaponLayer = value;
        }

        public float GetZoomFOVValue()
        {
            return zoomFOVValue;
        }

        public void SetZoomFOVValue(float value)
        {
            zoomFOVValue = value;
        }

        public float GetDefaultFOVValue()
        {
            return defaultFOVValue;
        }

        public void SetDefaultFOVValue(float value)
        {
            defaultFOVValue = value;
        }

        public float GetZoomTime()
        {
            return speed;
        }

        public void SetZoomTime(float value)
        {
            speed = value;
        }
    }
}