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
using UnityEngine.Events;

namespace AuroraFPSRuntime.AI
{
    [Serializable]
    public class DestinationEvent
    {
        [System.Flags]
        public enum InvokeTime
        {
            /// <summary>
            /// Not called, all event will be disabled.
            /// </summary>
            Disabled = 0,

            /// <summary>
            /// Only when destination is complete.
            /// </summary>
            OnComplete = 1 << 0,

            /// <summary>
            /// Only when destination is reached.
            /// </summary>
            OnReadched = 1 << 1,

            /// <summary>
            /// Called in all states.
            /// </summary>
            Both = ~0
        }

        [SerializeField] private int index = 0;
        [SerializeField] private float delay = 0.0f;
        [SerializeField] private InvokeTime invokeTime = InvokeTime.Disabled;
        [SerializeField] private UnityEvent eventCallback;

        /// <summary>
        /// Invoke destination event by InvokeTime.
        /// </summary>
        public void InvokeEvent(InvokeTime invokeTime)
        {
            if ((this.invokeTime & invokeTime) == invokeTime)
            {
                eventCallback.Invoke();
            }
        }
        
        #region [Getter / Setter]
        public int GetIndex()
        {
            return index;
        }

        public void SetIndex(int value)
        {
            index = value;
        }

        public float GetDelay()
        {
            return delay;
        }

        public void SetDelay(float value)
        {
            delay = value;
        }

        public InvokeTime GetEventInvoke()
        {
            return invokeTime;
        }

        public void SetEventInvoke(InvokeTime value)
        {
            invokeTime = value;
        }

        public UnityEvent GetEventCallback()
        {
            return eventCallback;
        }

        public void SetEventCallback(UnityEvent value)
        {
            eventCallback = value;
        }

        #endregion
    }
}