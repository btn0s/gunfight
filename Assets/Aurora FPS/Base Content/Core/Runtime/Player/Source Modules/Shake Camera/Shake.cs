/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AuroraFPSRuntime
{
    public partial class CameraShake
    {
        [Serializable]
        public sealed class Shake
        {
            [Flags]
            public enum Target
            {
                None = 0,
                Position = 1 << 0,
                Rotation = 1 << 1,
                Both = ~0
            }

            [SerializeField] private Target target;
            [SerializeField] private ShakeProperty positionSettings;
            [SerializeField] private ShakeProperty rotationSettings;

            private Vector3 positionNoise;
            private Vector3 rotationNoise;
            private Vector3 positionNoiseOffset;
            private Vector3 rotationNoiseOffset;
            private float positionTimeRemaining;
            private float rotationTimeRemaining;


            public Shake(Target target, ShakeProperty positionSetting, ShakeProperty rotationSettings)
            {
                this.target = target;
                this.positionSettings = positionSetting;
                this.rotationSettings = rotationSettings;
            }

            public void Initialize()
            {
                positionTimeRemaining = positionSettings.GetDuration();
                rotationTimeRemaining = rotationSettings.GetDuration();

                const float rand = 32.0f;

                positionNoiseOffset.x = Random.Range(0.0f, rand);
                positionNoiseOffset.y = Random.Range(0.0f, rand);
                positionNoiseOffset.z = Random.Range(0.0f, rand);

                rotationNoiseOffset.x = Random.Range(0.0f, rand);
                rotationNoiseOffset.y = Random.Range(0.0f, rand);
                rotationNoiseOffset.z = Random.Range(0.0f, rand);
            }

            public Vector3 GetPositionNoise()
            {
                positionTimeRemaining -= Time.deltaTime;

                float noiseOffsetDelta = Time.deltaTime * positionSettings.GetFrequency();

                positionNoiseOffset.x += noiseOffsetDelta;
                positionNoiseOffset.y += noiseOffsetDelta;
                positionNoiseOffset.z += noiseOffsetDelta;

                positionNoise.x = Mathf.PerlinNoise(positionNoiseOffset.x, 0.0f);
                positionNoise.y = Mathf.PerlinNoise(positionNoiseOffset.y, 1.0f);
                positionNoise.z = Mathf.PerlinNoise(positionNoiseOffset.z, 2.0f);

                positionNoise -= Vector3.one * 0.5f;

                positionNoise *= positionSettings.GetAmplitude();

                float time = 1.0f - (positionTimeRemaining / positionSettings.GetDuration());
                positionNoise *= positionSettings.GetBlendOverLifetime().Evaluate(time);

                return positionNoise;
            }

            public Vector3 GetRotationNoise()
            {
                rotationTimeRemaining -= Time.deltaTime;

                float noiseOffsetDelta = Time.deltaTime * rotationSettings.GetFrequency();

                rotationNoiseOffset.x += noiseOffsetDelta;
                rotationNoiseOffset.y += noiseOffsetDelta;
                rotationNoiseOffset.z += noiseOffsetDelta;

                rotationNoise.x = Mathf.PerlinNoise(rotationNoiseOffset.x, 0.0f);
                rotationNoise.y = Mathf.PerlinNoise(rotationNoiseOffset.y, 1.0f);
                rotationNoise.z = Mathf.PerlinNoise(rotationNoiseOffset.z, 2.0f);

                rotationNoise -= Vector3.one * 0.5f;

                rotationNoise *= rotationSettings.GetAmplitude();

                float time = 1.0f - (rotationTimeRemaining / rotationSettings.GetDuration());
                rotationNoise *= rotationSettings.GetBlendOverLifetime().Evaluate(time);

                return rotationNoise;
            }

            public bool IsAlive()
            {
                switch (target)
                {
                    case Target.Both:
                        return positionTimeRemaining > 0.0f && rotationTimeRemaining > 0.0f;
                    case Target.Position:
                        return positionTimeRemaining > 0.0f;
                    case Target.Rotation:
                        return rotationTimeRemaining > 0.0f;
                    case Target.None:
                        return true;
                    default:
                        return true;
                }
            }

            public Target GetTarget()
            {
                return target;
            }

            public void SetTarget(Target value)
            {
                target = value;
            }

            public ShakeProperty GetPositionSettings()
            {
                return positionSettings;
            }

            public void SetPositionSettings(ShakeProperty value)
            {
                positionSettings = value;
            }

            public ShakeProperty GetRotationSettings()
            {
                return rotationSettings;
            }

            public void SetRotationSettings(ShakeProperty value)
            {
                rotationSettings = value;
            }
        }
    }
}