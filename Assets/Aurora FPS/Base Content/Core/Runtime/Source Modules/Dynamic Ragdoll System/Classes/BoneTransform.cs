/* ==================================================================
   ---------------------------------------------------
   Project   :    Beyond
   Publisher :    -
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================== */

using UnityEngine;

namespace AuroraFPSRuntime
{
    public partial class DynamicRagdoll
    {
        /// <summary>
        /// Declare a class that will hold useful information for each body part
        /// </summary>
        public struct BoneTransform
        {
            private Transform transform;
            private Vector3 position;
            private Quaternion rotation;
            private Vector3 storedPosition;
            private Quaternion storedRotation;

            public BoneTransform(Transform transform)
            {
                this.transform = transform;
                this.position = Vector3.zero;
                this.rotation = Quaternion.identity;
                this.storedPosition = Vector3.zero;
                this.storedRotation = Quaternion.identity;
            }

            public Transform GetTransform()
            {
                return transform;
            }

            public Vector3 GetPosition()
            {
                return position;
            }

            public void SetPosition(Vector3 value)
            {
                position = value;
            }

            public Quaternion GetRotation()
            {
                return rotation;
            }

            public void SetRotation(Quaternion value)
            {
                rotation = value;
            }

            public Vector3 GetStoredPosition()
            {
                return storedPosition;
            }

            public void SetStoredPosition(Vector3 value)
            {
                storedPosition = value;
            }

            public Quaternion GetStoredRotation()
            {
                return storedRotation;
            }

            public void SetStoredRotation(Quaternion value)
            {
                storedRotation = value;
            }
        }
    }
}