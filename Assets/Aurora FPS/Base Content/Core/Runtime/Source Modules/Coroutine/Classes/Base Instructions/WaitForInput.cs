/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2020 All rights reserved.
   ================================================================ */

using UnityEngine;

namespace AuroraFPSRuntime
{
    /// <summary>
    /// Input event type.
    /// </summary>
    public enum InputEvent
    {
        Pressed,
        Hold,
        Released,
    }

    public sealed class WaitForButton : AuroraYieldInstruction
    {
        // Base WaitForInput properties.
        [SerializeField] private string buttonName;
        [SerializeField] private InputEvent inputEvent;

        /// <summary>
        /// WaitForInput constructor.
        /// </summary>
        /// <param name="buttonName">Button name from input manager.</param>
        /// <param name="inputEvent">Input event type.</param>
        public WaitForButton(string buttonName, InputEvent inputEvent)
        {
            this.buttonName = buttonName;
            this.inputEvent = inputEvent;
        }

        /// <summary>
        /// Yield instruction.
        /// </summary>
        protected override bool Update()
        {
            switch (inputEvent)
            {
                case InputEvent.Pressed:
                    return !Input.GetButtonDown(buttonName);
                case InputEvent.Hold:
                    return !Input.GetButton(buttonName);
                case InputEvent.Released:
                    return !Input.GetButtonUp(buttonName);
                default:
                    return true;
            }
        }
    }

    public sealed class WaitForAInput : AuroraYieldInstruction
    {
        // Base WaitForInput properties.
        [SerializeField] private string buttonName;
        [SerializeField] private InputEvent inputEvent;

        /// <summary>
        /// WaitForInput constructor.
        /// </summary>
        /// <param name="buttonName">Button name from input manager.</param>
        /// <param name="inputEvent">Input event type.</param>
        public WaitForAInput(string buttonName, InputEvent inputEvent)
        {
            this.buttonName = buttonName;
            this.inputEvent = inputEvent;
        }

        /// <summary>
        /// Yield instruction.
        /// </summary>
        protected override bool Update()
        {
            switch (inputEvent)
            {
                case InputEvent.Pressed:
                    return !AInput.GetButtonDown(buttonName);
                case InputEvent.Hold:
                    return !AInput.GetButton(buttonName);
                case InputEvent.Released:
                    return !AInput.GetButtonUp(buttonName);
                default:
                    return true;
            }
        }
    }

    public sealed class WaitForKeyCode : AuroraYieldInstruction
    {
        // Base WaitForInput properties.
        [SerializeField] private KeyCode keycode;
        [SerializeField] private InputEvent inputEvent;

        /// <summary>
        /// WaitForInput constructor.
        /// </summary>
        /// <param name="keycode">KeyCode type.</param>
        /// <param name="inputEvent">Input event type.</param>
        public WaitForKeyCode(KeyCode keycode, InputEvent inputEvent)
        {
            this.keycode = keycode;
            this.inputEvent = inputEvent;
        }

        /// <summary>
        /// Yield instruction.
        /// </summary>
        protected override bool Update()
        {
            switch (inputEvent)
            {
                case InputEvent.Pressed:
                    return !Input.GetKeyDown(keycode);
                case InputEvent.Hold:
                    return !Input.GetKey(keycode);
                case InputEvent.Released:
                    return !Input.GetKeyDown(keycode);
                default:
                    return true;
            }
        }
    }
}