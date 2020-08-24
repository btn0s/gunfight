/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using System;
using UnityEngine;
using UnityEngine.Events;

namespace AuroraFPSRuntime
{
    [RequireComponent(typeof(AudioSource))]
    public class GrabbingSystem : MonoBehaviour
    {
        // Grabbing properties.
        [Header("Base Properties")]
        [SerializeField] private Transform playerCamera;
        [SerializeField] private Transform attachBody;

        [Header("Grab Properties")]
        [SerializeField] private float grabRange = 1.5f;
        [SerializeField] private float throwForce = 20.0f;
        [SerializeField] private AudioClip throwSound;
        [SerializeField] private LayerMask grabLayer = Physics.AllLayers;

        [SerializeField] private UnityEvent onGrabEvent;
        [SerializeField] private UnityEvent onDropEvent;
        [SerializeField] private UnityEvent onThrowEvent;

        // Stored required components.
        private AudioSource audioSource;
        private GrabJoint storedGrabJoint;

        // Stored required properties.
        private bool isGrabbing;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();

            // If CharacterHealth contained in the controller, then register the necessary callback.
            // If controller is die, automatically drop grabbed object.
            CharacterHealth health = GetComponent<CharacterHealth>();
            if (health != null)
            {
                health.OnDeadCallback += Drop;
            }

            // If AdaptiveFPRagdoll contained in the controller, then register the necessary callback.
            // If controller is ragdolled, automatically drop grabbed object.
            FPAdaptiveRagdoll adaptiveRagdoll = GetComponent<FPAdaptiveRagdoll>();
            if (adaptiveRagdoll != null)
            {
                adaptiveRagdoll.OnRagdollCallback += Drop;
            }

            // If AdaptiveFPRagdoll contained in the controller, then register the necessary callback.
            // If controller is ragdolled, automatically drop grabbed object.
            DynamicRagdoll dynamicRagdoll = GetComponent<DynamicRagdoll>();
            if (dynamicRagdoll != null)
            {
                dynamicRagdoll.OnStartRagdollCallback += Drop;
            }

            FPInventory inventory = GetComponent<FPInventory>();
            if (inventory != null)
            {
                OnGrabCallback += _ => inventory.HideWeapon();
                OnThrowCallback += () => inventory.ActivateHiddenWeapon();
                inventory.OnSwitchCallback += _ => Drop();
            }

            OnGrabCallback += _ => onGrabEvent?.Invoke();
            OnDropCallback += () => onDropEvent?.Invoke();
            OnThrowCallback += () => onThrowEvent?.Invoke();
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void Update()
        {
            if (!isGrabbing && AInput.GetButtonDown(INC.Grab))
            {
                if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hitInfo, grabRange, grabLayer, QueryTriggerInteraction.Ignore))
                {
                    GrabJoint grabJoint = hitInfo.transform.GetComponent<GrabJoint>();
                    if (grabJoint != null)
                    {
                        Grab(grabJoint);
                    }
                }
            }
            else if (isGrabbing && storedGrabJoint != null && (AInput.GetButtonDown(INC.Grab)))
            {
                Drop();
            }
            else if (isGrabbing && storedGrabJoint != null && AInput.GetButtonDown(INC.Attack))
            {
                Throw(throwForce);
            }
        }

        /// <summary>
        /// Start grabbing object.
        /// </summary>
        public virtual void Grab(GrabJoint grabJoint)
        {   
            if(grabJoint == null)
            {
                return;
            }

            storedGrabJoint = grabJoint;
            storedGrabJoint.ConnectBody(attachBody);
            storedGrabJoint.OnBreakCallback += Drop;
            OnGrabCallback?.Invoke(storedGrabJoint.gameObject);
            isGrabbing = true;
        }

        /// <summary>
        /// Drop current grabbed object.
        /// </summary>
        public virtual void Drop()
        {
            if(storedGrabJoint == null)
            {
                return;
            }

            storedGrabJoint.DisconnectBody();
            storedGrabJoint.OnBreakCallback -= Drop;
            storedGrabJoint = null;
            OnDropCallback?.Invoke();
            isGrabbing = false;
        }

        /// <summary>
        /// Throw current grabbed object and stop grabbing.
        /// </summary>
        public virtual void Throw(float force)
        {
             if(storedGrabJoint == null)
            {
                return;
            }

            storedGrabJoint.OnBreakCallback -= Drop;
            storedGrabJoint.DisconnectBody();
            Rigidbody rigidbody = storedGrabJoint.GetObjectRigidbody();
            rigidbody.AddForce(playerCamera.forward * force, ForceMode.Impulse);
            PlayThrowSound();
            OnThrowCallback?.Invoke();
            storedGrabJoint = null;
            isGrabbing = false;
        }

        /// <summary>
        /// Play throw sound
        /// </summary>
        protected void PlayThrowSound()
        {
            if (throwSound != null)
            {
                audioSource.PlayOneShot(throwSound);
            }
        }

        #region [Event Callback Functions]
        /// <summary>
        /// OnGrabCallback called once when controller grabbed object.
        /// </summary>
        /// <param name="GameObject">Grabbed gameobject.</param>
        public event Action<GameObject> OnGrabCallback;

        /// <summary>
        /// OnDropCallback called when grabbed object is dropped. 
        /// </summary>
        private event Action OnDropCallback;

        /// <summary>
        /// OnThrowCallback called when object throwed.
        /// </summary>
        private event Action OnThrowCallback;
        #endregion
       
        #region [Getter / Setter]
        public Transform GetPlayerCamera()
        {
            return playerCamera;
        }
        public void SetPlayerCamera(Transform value)
        {
            playerCamera = value;
        }

        public Transform GetAttachBody()
        {
            return attachBody;
        }

        public void SetAttachBody(Transform value)
        {
            attachBody = value;
        }

        public float GetGrabRange()
        {
            return grabRange;
        }

        public void SetGrabRange(float value)
        {
            grabRange = value;
        }

        public float GetThrowForce()
        {
            return throwForce;
        }

        public void SetThrowForce(float value)
        {
            throwForce = value;
        }

        public AudioClip GetThrowSound()
        {
            return throwSound;
        }

        public void SetThrowSound(AudioClip value)
        {
            throwSound = value;
        }

        public LayerMask GetGrabLayer()
        {
            return grabLayer;
        }

        public void SetGrabLayer(LayerMask value)
        {
            grabLayer = value;
        }

        protected UnityEvent OnGrabEvent()
        {
            return onGrabEvent;
        }

        protected void OnGrabEvent(UnityEvent value)
        {
            onGrabEvent = value;
        }

        protected UnityEvent OnDropEvent()
        {
            return onDropEvent;
        }

        protected void OnDropEvent(UnityEvent value)
        {
            onDropEvent = value;
        }

        protected UnityEvent OnThrowEvent()
        {
            return onThrowEvent;
        }

        protected void OnThrowEvent(UnityEvent value)
        {
            onThrowEvent = value;
        }

        public bool IsGrabbing()
        {
            return isGrabbing;
        }

        protected void IsGrabbing(bool value)
        {
            isGrabbing = value;
        }

        public AudioSource GetAudioSource()
        {
            return audioSource;
        }

        public void SetAudioSource(AudioSource value)
        {
            audioSource = value;
        }
        #endregion
    }
}