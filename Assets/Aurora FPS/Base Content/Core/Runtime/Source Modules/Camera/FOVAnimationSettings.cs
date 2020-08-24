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
    [System.Serializable]
    public class FOVAnimationSettings
    {
        [SerializeField] private float fieldOfView = 60;
        [SerializeField] private float duration = 0.0f;
        [SerializeField] private AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);


        public FOVAnimationSettings(float fieldOfView, AnimationCurve curve)
        {
            this.fieldOfView = fieldOfView;
            this.duration = 0.25f;
            this.curve = curve;
        }

        public FOVAnimationSettings(float fieldOfView, float duration, AnimationCurve curve)
        {
            this.fieldOfView = fieldOfView;
            this.duration = duration > 0 ? duration : 0.01f;
            this.curve = curve;
        }

        public float EvaluateCurve(float time)
        {
            return curve.Evaluate(time);
        }

        #region [Getter / Setter]
        public float GetFieldOfView()
        {
            return fieldOfView;
        }

        public void SetFieldOfView(float value)
        {
            this.fieldOfView = value;
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
        #endregion
    }
}