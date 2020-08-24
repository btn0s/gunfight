/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using System;
using System.Collections;
using UnityEngine;

namespace AuroraFPSRuntime
{
    [Serializable]
    public class CameraLandingEffect
    {
        // Base camera landing effect properties.
        [SerializeField] private RangedFloat amountLimit = new RangedFloat(0.1f, 0.4f, 0.0f, 3.0f);
        [SerializeField] private float amountPersent = 0.5f;
        [SerializeField] private float duration = 0.35f;
        [SerializeField] private AnimationCurve curve = new AnimationCurve(new Keyframe(0.0f, 0.0f), new Keyframe(0.2f, -1.0f), new Keyframe(1.0f, 0.0f));

        // Stored required components.
        private Transform camera;


        /// <summary>
        /// Initialize camera landing effect.
        /// </summary>
        /// <param name="camera">Camera transform reference.</param>
        public virtual void Initialize(Transform camera)
        {
            this.camera = camera;
        }

        /// <summary>
        /// Play camera landing effect.
        /// </summary>
        public virtual IEnumerator PlayEffect(float inAirTime)
        {
            float time = 0f;
            float speed = 1f / duration;

            Vector3 storedLocalPosition = camera.localPosition;
            float storedCameraHeight = storedLocalPosition.y;
            float landAmount = inAirTime * amountPersent;
            landAmount = AMath.Clamp(landAmount, amountLimit);
            while (time < 1f)
            {
                time += Time.deltaTime * speed;
                float offset = curve.Evaluate(time) * landAmount;

                storedLocalPosition.y = storedCameraHeight + offset;
                camera.localPosition = storedLocalPosition;

                yield return null;
            }
        }

        #region [Getter / Setter]
        public RangedFloat GetAmountLimit()
        {
            return amountLimit;
        }

        public void SetAmountLimit(RangedFloat value)
        {
            amountLimit = value;
        }

        public float GetAmountPercent()
        {
            return amountPersent;
        }

        public void SetAmountPercent(float value)
        {
            amountPersent = value;
        }

        public float GetDuration()
        {
            return duration;
        }

        public void SetDuration(float value)
        {
            duration = value;
        }

        public AnimationCurve GetCurve()
        {
            return curve;
        }

        public void SetCurve(AnimationCurve value)
        {
            curve = value;
        }

        public Transform GetCamera()
        {
            return camera;
        }

        public void SetCamera(Transform value)
        {
            camera = value;
        }
        #endregion
    }
}