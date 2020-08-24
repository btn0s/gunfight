/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using UnityEngine;

namespace AuroraFPSRuntime.AI
{
    [System.Serializable]
    public abstract class Condition : ICondition, IConditionMute
    {
        [SerializeField] private bool mute;

        // Stored required components.
        protected AICore core;

        /// <summary>
        /// Initializing required properties.
        /// </summary>
        /// <param name="core">Target AICore instance.</param>
        public virtual void Initialize(AICore core)
        {
            this.core = core;
        }

        /// <summary>
        /// Called when the behaviour in which this condition is located, becomes enabled.
        /// </summary>
        public virtual void OnEnable()
        {
            
        }
        
        /// <summary>
        /// Condition for translate to the next AI behaviour.
        /// </summary>
        public abstract bool IsExecuted();
        
        /// <summary>
        /// Called when the behaviour in which this condition is located, switch to another.
        /// </summary>
        public virtual void OnDisable()
        {

        }

        /// <summary>
        /// Transition mute state value.
        /// </summary>
        /// <returns>
        /// True if transition is muted and not checked.
        /// Otherwise false.
        /// </returns>
        public bool IsMuted()
        {
            return mute;
        }
        
        /// <summary>
        /// Set transition mute state.
        /// </summary>
        /// <param name="value">New transition mute state</param>
        public void SetMute(bool value)
        {
            mute = value;
        }
    }
}
