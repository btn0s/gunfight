/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using UnityEngine;
using System;
using Random = UnityEngine.Random;

namespace AuroraFPSRuntime
{
    [System.Serializable]
    public struct FootstepSounds : IEquatable<FootstepSounds>
    {
        [SerializeField] private AudioClip[] stepSounds;
        [SerializeField] private AudioClip[] jumpSounds;
        [SerializeField] private AudioClip[] landSounds;

        /// <summary>
        /// FootstepProperty constructor.
        /// </summary>
        public FootstepSounds(AudioClip[] stepSounds, AudioClip[] jumpSounds, AudioClip[] landSounds)
        {
            this.stepSounds = stepSounds;
            this.jumpSounds = jumpSounds;
            this.landSounds = landSounds;
        }

        /// <summary>
        /// Return footstep sounds.
        /// </summary>
        public AudioClip[] GetStepSounds()
        {
            return stepSounds;
        }

        /// <summary>
        /// Set range footstep sounds.
        /// </summary>
        /// <param name="stepSounds"></param>
        public void SetStepSoundsRange(AudioClip[] stepSounds)
        {
            this.stepSounds = stepSounds;
        }

        /// <summary>
        /// Return footstep sound.
        /// </summary>
        /// <param name="index">Footstep sound index.</param>
        public AudioClip GetStepSound(int index)
        {
            return stepSounds[index];
        }

        /// <summary>
        /// Return random step sound form stepsSounds array.
        /// </summary>
        public AudioClip GetRandomStepSound()
        {
            if (stepSounds == null || stepSounds.Length == 0)
            {
                return null;
            }

            int index = Random.Range(0, stepSounds.Length);
            return stepSounds[index];
        }

        /// <summary>
        /// Set footstep sound.
        /// </summary>
        /// <param name="index">Footstep sound index.</param>
        /// <param name="stepSound">Footstep sound.</param>
        public void SetStepSound(int index, AudioClip stepSound)
        {
            stepSounds[index] = stepSound;
        }

        /// <summary>
        /// Return jumpstep sounds.
        /// </summary>
        public AudioClip[] GetJumpSounds()
        {
            return jumpSounds;
        }

        /// <summary>
        /// Set range jumpstep sounds.
        /// </summary>
        /// <param name="jumpSounds"></param>
        public void SetJumpSoundsRange(AudioClip[] jumpSounds)
        {
            this.jumpSounds = jumpSounds;
        }

        /// <summary>
        /// Return jumpstep sound.
        /// </summary>
        /// <param name="index">Jumpstep sound index.</param>
        public AudioClip GetJumpSound(int index)
        {
            return jumpSounds[index];
        }

        /// <summary>
        /// Return random jump sound form jumpSounds array.
        /// </summary>
        public AudioClip GetRandomJumpSound()
        {
            if (jumpSounds == null || jumpSounds.Length == 0)
            {
                return null;
            }

            int index = Random.Range(0, jumpSounds.Length);
            return jumpSounds[index];
        }

        /// <summary>
        /// Set jumpstep sound.
        /// </summary>
        /// <param name="index">Jumpstep sound index.</param>
        /// <param name="jumpSound">Jumpstep sound.</param>
        public void SetJumpSound(int index, AudioClip jumpSound)
        {
            jumpSounds[index] = jumpSound;
        }

        /// <summary>
        /// Return landstep sounds.
        /// </summary>
        public AudioClip[] GetLandSounds()
        {
            return landSounds;
        }

        /// <summary>
        /// Set range landstep sounds.
        /// </summary>
        public void SetLandSoundsRange(AudioClip[] landSounds)
        {
            this.landSounds = landSounds;
        }

        /// <summary>
        /// Return land step sound.
        /// </summary>
        /// <param name="index">Landstep sound index.</param>
        public AudioClip GetLandSound(int index)
        {
            return landSounds[index];
        }

        /// <summary>
        /// Return random land sound form landSounds array.
        /// </summary>
        public AudioClip GetRandomLandSound()
        {
            if (landSounds == null || landSounds.Length == 0)
            {
                return null;
            }

            int index = Random.Range(0, landSounds.Length);
            return landSounds[index];
        }

        /// <summary>
        /// Set landstep sound.
        /// </summary>
        /// <param name="index">Landstep sound index.</param>
        /// <param name="landSound">Landstep sound.</param>
        public void SetLandSound(int index, AudioClip landSound)
        {
            landSounds[index] = landSound;
        }

        /// <summary>
        /// Return footstep sounds array length.
        /// </summary>
        public int GetStepSoundsLength()
        {
            return stepSounds.Length;
        }

        /// <summary>
        /// Return jump step sounds array length.
        /// </summary>
        public int GetJumpSoundsLength()
        {
            return jumpSounds.Length;
        }

        /// <summary>
        /// Return land step sounds array length.
        /// </summary>
        public int GetLandSoundsLength()
        {
            return landSounds.Length;
        }

        public readonly static FootstepSounds Empty = new FootstepSounds(new AudioClip[0], new AudioClip[0], new AudioClip[0]);


        public static bool operator ==(FootstepSounds left, FootstepSounds right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(FootstepSounds left, FootstepSounds right)
        {
            return !Equals(left, right);
        }

        public override bool Equals(object obj)
        {
            return (obj is FootstepSounds metrics) && Equals(metrics);
        }

        public bool Equals(FootstepSounds other)
        {
            return (stepSounds, jumpSounds, landSounds) == (other.stepSounds, other.jumpSounds, other.landSounds);
        }

        public override int GetHashCode()
        {
            return (stepSounds, jumpSounds, landSounds).GetHashCode();
        }
    }
}