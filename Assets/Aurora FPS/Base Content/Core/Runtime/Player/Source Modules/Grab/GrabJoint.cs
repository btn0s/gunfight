/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using System.Collections;
using UnityEngine;
using Action = System.Action;

namespace AuroraFPSRuntime
{
    [RequireComponent(typeof(Rigidbody))]
    public class GrabJoint : MonoBehaviour
    {
        // Grab object properties.
        [SerializeField] private Vector3 anchor = new Vector3(-0.35f, -0.5f, 2.0f);
        [SerializeField] private float smooth = 5.0f;
        [SerializeField] private float breakVelocity = 15.0f;
        [SerializeField] private float breakDistance = 2.5f;
        [SerializeField] private bool freezeRotaion = true;
        [SerializeField] private Vector3 customRotation;

        // Stored required components
        private Rigidbody objectRigidbody;

        // Stored required properties
        private CoroutineObject<Transform> grabCoroutine;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            objectRigidbody = GetComponent<Rigidbody>();
            grabCoroutine = new CoroutineObject<Transform>(this);
        }

        /// <summary>
        /// Update transform postion by anchor connetion position. 
        /// </summary>
        protected virtual IEnumerator SavingConnection(Transform body)
        {
            WaitForFixedUpdate fixedUpdate = new WaitForFixedUpdate();
            while (true)
            {
                Vector3 connectPosition = body.TransformPoint(anchor) - transform.position;
                objectRigidbody.velocity = connectPosition * smooth;
                objectRigidbody.rotation = Quaternion.Lerp(objectRigidbody.rotation, body.rotation * Quaternion.Euler(customRotation), smooth * Time.deltaTime);

                if(Vector3.Distance(transform.position, body.root.position) >= breakDistance)
                {
                    ApplyDefaultSettings();
                    OnBreakCallback?.Invoke();
                    yield break;
                }
                yield return fixedUpdate;
            }
        }

        /// <summary>
        /// OnCollisionEnter is called when this collider/rigidbody has begun
        /// touching another rigidbody/collider.
        /// </summary>
        /// <param name="other">The Collision data associated with this collision.</param>
        protected virtual void OnCollisionEnter(Collision other)
        {
            if (other.relativeVelocity.magnitude >= breakVelocity)
            {
                ApplyDefaultSettings();
                OnBreakCallback?.Invoke();
            }
        }

        /// <summary>
        /// Connect transform to body.
        /// </summary>
        public virtual void ConnectBody(Transform body)
        {
            ApplyGrabSettings();
            grabCoroutine.Start(SavingConnection, body);
        }

        /// <summary>
        /// Disconnect transform from current connection.
        /// </summary>
        public virtual void DisconnectBody()
        {
            grabCoroutine.Stop();
            ApplyDefaultSettings();
        }

        /// <summary>
        /// Connected object settings.
        /// Called once when object connect to the body.
        /// </summary>
        protected virtual void ApplyGrabSettings()
        {
            if (objectRigidbody != null)
            {
                objectRigidbody.useGravity = false;
                objectRigidbody.freezeRotation = freezeRotaion;
            }
        }

        /// <summary>
        /// Default object settings.
        /// Called once when object disconected from body.
        /// </summary>
        protected virtual void ApplyDefaultSettings()
        {
            if (objectRigidbody)
            {
                objectRigidbody.velocity = Vector3.zero;
                objectRigidbody.useGravity = true;
                objectRigidbody.freezeRotation = false;
            }
        }

        #region [Event Callback Functions]
        /// <summary>
        /// On object break callback function.
        /// OnBreakCallback called when object enter of collision with other collider >= break velocity.
        /// </summary>
        public event Action OnBreakCallback;
        #endregion

        #region [Getter / Setter]
        public Vector3 GetAnchor()
        {
            return anchor;
        }

        public void SetAnchor(Vector3 value)
        {
            anchor = value;
        }

        public float GetSmooth()
        {
            return smooth;
        }

        public void SetSmooth(float value)
        {
            smooth = value;
        }

        public float GetBreakVelocity()
        {
            return breakVelocity;
        }

        public void SetBreakVelocity(float value)
        {
            breakVelocity = value;
        }

        public float GetBreakDistance()
        {
            return breakDistance;
        }

        public void SetBreakDistance(float value)
        {
            breakDistance = value;
        }

        public bool FreezeRotaion()
        {
            return freezeRotaion;
        }

        public void FreezeRotaion(bool value)
        {
            freezeRotaion = value;
        }

        public Vector3 GetCustomRotation()
        {
            return customRotation;
        }

        public void SetCustomRotation(Vector3 value)
        {
            customRotation = value;
        }

        public Rigidbody GetObjectRigidbody()
        {
            return objectRigidbody;
        }

        protected void SetObjectRigidbody(Rigidbody value)
        {
            objectRigidbody = value;
        }
        #endregion
    }
}