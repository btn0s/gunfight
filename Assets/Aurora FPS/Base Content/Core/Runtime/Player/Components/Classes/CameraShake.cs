/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using System.Collections.Generic;
using UnityEngine;

namespace AuroraFPSRuntime
{
    public partial class CameraShake : Singleton<CameraShake>
    {
        [SerializeField] private List<Shake> shakes = new List<Shake>();

        /// <summary>
        /// LateUpdate is called every frame, if the Behaviour is enabled.
        /// It is called after all Update functions have been called.
        /// </summary>
        protected virtual void LateUpdate()
        {
            if (shakes.Count > 0)
            {
                for (int i = shakes.Count - 1; i != -1; i--)
                {
                    Shake shake = shakes[i];
                    PlayShake(shake);
                    if (!shake.IsAlive())
                    {
                        shakes.RemoveAt(i);
                        continue;
                    }
                }
            }
        }

        protected virtual void PlayShake(Shake shake)
        {
            Vector3 positionOffset = transform.localPosition;
            Vector3 rotationOffset = transform.localRotation.eulerAngles;
            switch (shake.GetTarget())
            {
                case Shake.Target.Position:
                    positionOffset += shake.GetPositionNoise();
                    break;
                case Shake.Target.Rotation:
                    rotationOffset += shake.GetRotationNoise();
                    break;
                case Shake.Target.Both:
                    positionOffset += shake.GetPositionNoise();
                    rotationOffset += shake.GetRotationNoise();
                    break;
            }
            transform.localPosition = positionOffset;
            transform.localRotation = Quaternion.Euler(rotationOffset);
        }

        public void AddShake(Shake shake)
        {
            shake.Initialize();
            shakes.Add(shake);
        }
    }
}