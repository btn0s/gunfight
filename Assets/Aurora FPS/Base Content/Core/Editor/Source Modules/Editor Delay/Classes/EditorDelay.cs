/* ==================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================== */

using UnityEngine;
using Action = System.Action;

namespace AuroraFPSEditor
{
    public sealed class EditorDelay : IEditorDelay
    {
        private float delay;
        private float savedTime;
        private bool start;

        /// <summary>
        /// EditorDelay constructor.
        /// </summary>
        public EditorDelay()
        {
            this.delay = 1.0f;
            savedTime = -1.0f;
        }

        /// <summary>
        /// EditorDelay constructor.
        /// </summary>
        public EditorDelay(float delay)
        {
            this.delay = delay;
            savedTime = -1.0f;
        }

        /// <summary>
        /// Start processing delay.
        /// </summary>
        public void Start()
        {
            start = true;
        }

        /// <summary>
        /// Check that delay is done.
        /// </summary>
        public bool WaitForSeconds()
        {
            if (start)
            {
                if (savedTime == -1.0f)
                {
                    savedTime = Time.realtimeSinceStartup;
                }
                else if (Time.realtimeSinceStartup - savedTime >= delay)
                {
                    savedTime = -1.0f;
                    start = false;
                    DelayDoneCallback?.Invoke();
                    return true;
                }
            }
            return false;
        }

        #region [Event Callback Functions]
        /// <summary>
        /// Delay is done callback event function.
        /// DelayDoneCallback called once when delay time out.
        /// </summary>
        public event Action DelayDoneCallback;
        #endregion

        #region [Getter / Setter]
        /// <summary>
        /// Get current delay time.
        /// </summary>
        public float GetDelay()
        {
            return delay;
        }

        /// <summary>
        /// Set new delay time.
        /// </summary>
        public void SetDelay(float delay)
        {
            this.delay = delay;
        }

        /// <summary>
        /// Check that delay processing is already started.
        /// </summary>
        public bool IsStart()
        {
            return start;
        }
        #endregion
    }
}