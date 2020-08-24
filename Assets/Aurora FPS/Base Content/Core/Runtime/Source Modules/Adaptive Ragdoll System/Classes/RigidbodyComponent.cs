/* ==================================================================
   ---------------------------------------------------
   Project   :    Beyond
   Publisher :    -
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================== */

using UnityEngine;

namespace AuroraFPSRuntime
{
    public abstract partial class CharacterRagdoll
    {
        /// <summary>
        ///  Declare a class that will hold useful information for each body physics part
        /// </summary>
        internal sealed class RigidbodyComponent
        {
            private Rigidbody rigidBody;
            private CharacterJoint joint;
            private Vector3 connectedAnchorDefault;

            public RigidbodyComponent(Rigidbody rigidBody)
            {
                this.rigidBody = rigidBody;
                joint = rigidBody.GetComponent<CharacterJoint>();
                if (joint != null)
                    connectedAnchorDefault = joint.connectedAnchor;
                else
                    connectedAnchorDefault = Vector3.zero;
            }

            public Rigidbody GetRigidBody()
            {
                return rigidBody;
            }

            public CharacterJoint GetJoint()
            {
                return joint;
            }

            public Vector3 GetConnectedAnchorDefault()
            {
                return connectedAnchorDefault;
            }
        }
    }
}