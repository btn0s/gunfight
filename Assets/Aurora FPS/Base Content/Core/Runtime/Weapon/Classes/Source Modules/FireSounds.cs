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
    public abstract partial class WeaponShootingSystem
    {
        [Serializable]
        public struct FireSounds
        {
            [SerializeField] private AudioClip[] normalFireSounds;
            [SerializeField] private AudioClip[] dryFireSounds;

            public AudioClip[] GetNormalFireSounds()
            {
                return normalFireSounds;
            }

            public AudioClip GetNormalFireSound(int index)
            {
                return normalFireSounds[index];
            }

            public void SetNormalFireSounds(AudioClip[] value)
            {
                normalFireSounds = value;
            }

            public void SetNormalFireSound(int index, AudioClip value)
            {
                normalFireSounds[index] = value;
            }

            public AudioClip[] GetDryFireSounds()
            {
                return dryFireSounds;
            }

            public void SetDryFireSounds(AudioClip[] value)
            {
                dryFireSounds = value;
            }

            public AudioClip GetDryFireSound(int index)
            {
                return dryFireSounds[index];
            }

            public void SetDryFireSound(int index, AudioClip value)
            {
                dryFireSounds[index] = value;
            }

            public AudioClip GetRandomNormalFireSound()
            {
                if (normalFireSounds == null || normalFireSounds.Length == 0)
                    return null;
                return normalFireSounds[Random.Range(0, normalFireSounds.Length)];
            }

            public AudioClip GetRandomDryFireSound()
            {
                if (dryFireSounds == null || dryFireSounds.Length == 0)
                    return null;
                return dryFireSounds[Random.Range(0, dryFireSounds.Length)];
            }
        }
    }
}