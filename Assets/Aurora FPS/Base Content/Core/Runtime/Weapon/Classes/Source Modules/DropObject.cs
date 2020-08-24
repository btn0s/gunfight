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

namespace AuroraFPSRuntime
{
    [Serializable]
    public struct DropObject : IEquatable<DropObject>
    {
        [SerializeField] private GameObject dropObject;
        [SerializeField] private float force;
        [SerializeField] private AudioClip soundEffect;
        [SerializeField] private float distance;
        [SerializeField] private Vector3 rotation;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dropObject">Instance of the object to drop.</param>
        public DropObject(GameObject dropObject)
        {
            this.dropObject = dropObject;
            this.force = 25.0f;
            this.soundEffect = null;
            this.distance = 2.0f;
            this.rotation = Vector3.zero;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dropObject">Instance of the object to drop.</param>
        /// <param name="force">Throw force.</param>
        /// <param name="soundEffect">Throw sound effect.</param>
        /// <param name="distance">Instantiate object distance.</param>
        /// <param name="rotation">Instantiate object rotation.</param>
        public DropObject(GameObject dropObject, float force, AudioClip soundEffect, float distance, Vector3 rotation) : this(dropObject)
        {
            this.force = force;
            this.soundEffect = soundEffect;
            this.distance = distance;
            this.rotation = rotation;
        }

        /// <summary>
        /// Instantiate drop object and throw it.
        /// </summary>
        public void InstantiateAndThrow(Vector3 origin, Vector3 direction)
        {
            GameObject clone = GameObject.Instantiate(dropObject, origin + (direction * distance), Quaternion.Euler(rotation));
            if (clone != null)
            {
                Rigidbody rigidbody = clone.GetComponent<Rigidbody>();
                if (rigidbody != null)
                {
                    rigidbody.AddForce(direction * force, ForceMode.Impulse);
                    rigidbody.AddTorque(direction * force, ForceMode.Impulse);
                }

                AudioSource audioSource = clone.GetComponent<AudioSource>();
                if (audioSource != null && soundEffect != null)
                {
                    audioSource.PlayOneShot(soundEffect);
                }
            }
        }

        #region [Default]
        public readonly static DropObject none = new DropObject(null);
        #endregion

        #region [IEquatable Implementation]
        public static bool operator ==(DropObject left, DropObject right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DropObject left, DropObject right)
        {
            return !Equals(left, right);
        }

        public override bool Equals(object obj)
        {
            return (obj is DropObject metrics) && Equals(metrics);
        }

        public bool Equals(DropObject other)
        {
            return (dropObject) == (other.dropObject);
        }

        public override int GetHashCode()
        {
            return (dropObject, force, soundEffect, distance, rotation).GetHashCode();
        }
        #endregion

        #region [Getter / Setter]
        public GameObject GetDropObject()
        {
            return dropObject;
        }

        public void SetDropObject(GameObject value)
        {
            dropObject = value;
        }

        public float GetForce()
        {
            return force;
        }

        public void SetForce(float value)
        {
            force = value;
        }

        public AudioClip GetSoundEffect()
        {
            return soundEffect;
        }

        public void SetSoundEffect(AudioClip value)
        {
            soundEffect = value;
        }

        public float GetDistance()
        {
            return distance;
        }

        public void SetDistance(float value)
        {
            distance = value;
        }

        public Vector3 GetRotation()
        {
            return rotation;
        }

        public void SetRotation(Vector3 value)
        {
            rotation = value;
        }
        #endregion
    }
}