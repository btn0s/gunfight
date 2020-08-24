/* ==================================================================
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
    [CreateAssetMenu(fileName = "Head Bob Settings", menuName = AuroraFPSProduct.Name + "/Camera/Settings/Head Bob", order = 125)]
    public class HeadBobSettings : ScriptableObject
    {
        [System.Serializable]
        public struct Multiplier
        {
            [SerializeField] private float amplitude;
            [SerializeField] private float frequency;

            public Multiplier(float amplitude, float frequency)
            {
                this.amplitude = amplitude;
                this.frequency = frequency;
            }

            #region [Getter / Setter]
            public float GetAmplitude()
            {
                return amplitude;
            }

            public void SetAmplitude(float value)
            {
                amplitude = value;
            }

            public float GetFrequency()
            {
                return frequency;
            }

            public void SetFrequency(float value)
            {
                frequency = value;
            }
            #endregion
        }

        // Base head bob settings properties.
        [SerializeField] private float xAmplitude = 0.05f;
        [SerializeField] private float yAmplitude = 0.1f;
        [SerializeField] private float xFrequency = 2.0f;
        [SerializeField] private float yFrequency = 4.0f;
        [SerializeField] private AnimationCurve xCurve = DefaultCurve;
        [SerializeField] private AnimationCurve yCurve = DefaultCurve;
        [SerializeField] private float speed = 10.0f;
        [SerializeField] private Multiplier walkMultiplier = new Multiplier(0.5f, 0.75f);
        [SerializeField] private Multiplier runMultiplier = new Multiplier(0.5f, 0.75f);
        [SerializeField] private Multiplier sprintMultiplier = new Multiplier(1.0f, 1.25f);
        [SerializeField] private Multiplier crouchMultiplier = new Multiplier(0.2f, 1.0f);
        [SerializeField] private Multiplier zoomMultiplier = new Multiplier(0.1f, 0.5f);

        #region [Getter / Setter]
        public float GetFrequencyX()
        {
            return xFrequency;
        }

        public void SetFrequencyX(float value)
        {
            xFrequency = value;
        }

        public float GetFrequencyY()
        {
            return yFrequency;
        }

        public void SetFrequencyY(float value)
        {
            yFrequency = value;
        }

        public float GetAmplitudeX()
        {
            return xAmplitude;
        }

        public void SetAmplitudeX(float value)
        {
            xAmplitude = value;
        }

        public float GetAmplitudeY()
        {
            return yAmplitude;
        }

        public void SetAmplitudeY(float value)
        {
            yAmplitude = value;
        }

        public AnimationCurve GetCurveX()
        {
            return xCurve;
        }

        public void SetCurveX(AnimationCurve value)
        {
            xCurve = value;
        }

        public AnimationCurve GetCurveY()
        {
            return yCurve;
        }

        public void SetCurveY(AnimationCurve value)
        {
            yCurve = value;
        }

        public float GetSpeed()
        {
            return speed;
        }

        public void SetSpeed(float value)
        {
            speed = value;
        }

        public Multiplier GetWalkMultiplier()
        {
            return walkMultiplier;
        }

        public void SetWalkMultiplier(Multiplier value)
        {
            walkMultiplier = value;
        }

        public Multiplier GetRunMultiplier()
        {
            return runMultiplier;
        }

        public void SetRunMultiplier(Multiplier value)
        {
            runMultiplier = value;
        }

        public Multiplier GetSprintMultiplier()
        {
            return sprintMultiplier;
        }

        public void SetSprintMultiplier(Multiplier value)
        {
            sprintMultiplier = value;
        }

        public Multiplier GetCrouchMultiplier()
        {
            return crouchMultiplier;
        }

        public void SetCrouchMultiplier(Multiplier value)
        {
            crouchMultiplier = value;
        }

        public Multiplier GetZoomMultiplier()
        {
            return zoomMultiplier;
        }

        public void SetZoomMultiplier(Multiplier value)
        {
            zoomMultiplier = value;
        }

        public readonly static AnimationCurve DefaultCurve = new AnimationCurve(new Keyframe(0.0f, 0.0f), new Keyframe(0.5f, 1.0f), new Keyframe(1.0f, 0.0f), new Keyframe(1.5f, -1.0f), new Keyframe(2.0f, 0.0f));
        #endregion
    }
}