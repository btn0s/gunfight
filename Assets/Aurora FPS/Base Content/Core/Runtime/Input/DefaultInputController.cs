/* ==================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using UnityEngine;

namespace AuroraFPSRuntime
{
	/// <summary>
	/// Base StandaloneController Controller in Aurora FPS, which used default Input manager.
	/// </summary>
	public class DefaultInputController : InputController
	{
		[SerializeField] private DefaultInputMapping inputMapping;
		[SerializeField] private bool lockCursorOnStart;

		/// <summary>
		/// Lock the and hide the cursor at the start.
		/// </summary>
		protected virtual void Start()
		{

#if UNITY_EDITOR
			if(inputMapping == null)
            {
				UnityEditor.EditorApplication.isPaused = true;
				string message = string.Format("Input Mapping is empty, add Input Mapping in {0} component, which added on {1} gameobject!", GetType().Name, name);
				Debug.LogError(message);
            }
#endif

            if (lockCursorOnStart)
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
		}

		/// <summary>
		/// Returns the value of the virtual axis identified by key.
		/// The value will be in the range -1...1 for keyboard and joystick input.
		/// </summary>
		public override float GetAxis(string key)
		{
            if (inputMapping != null && inputMapping.TryGetAxis(key, out string axis))
            {
                return Input.GetAxis(axis);
            }
            return 0;
		}

		/// <summary>
		/// Returns the value of the virtual axis identified by key with no smoothing filtering applied.
		/// The value will be in the range -1...1 for keyboard and joystick input. 
		/// Since input is not smoothed, keyboard input will always be either -1, 0 or 1.
		/// </summary>
		public override float GetAxisRaw(string key)
		{
            if (inputMapping != null && inputMapping.TryGetAxis(key, out string axis))
            {
                return Input.GetAxisRaw(axis);
            }
            return 0;
		}

		/// <summary>
		/// Returns true while the virtual button identified by key is held down.
		/// </summary>
		public override bool GetButtonDown(string key)
		{
            if (inputMapping != null && inputMapping.TryGetButton(key, out KeyCode keyCode))
            {
                return Input.GetKeyDown(keyCode);
            }
            return false;
		}

		/// <summary>
		/// Returns true during the frame the user pressed down the virtual button identified by key.
		/// </summary>
		public override bool GetButton(string key)
		{
            if (inputMapping != null && inputMapping.TryGetButton(key, out KeyCode keyCode))
            {
                return Input.GetKey(keyCode);
            }
            return false;
		}

		/// <summary>
		/// Returns true the first frame the user releases the virtual button identified by key.
		/// </summary>
		public override bool GetButtonUp(string key)
		{
            if (inputMapping != null && inputMapping.TryGetButton(key, out KeyCode keyCode))
            {
                return Input.GetKeyUp(keyCode);
            }
            return false;
		}

		#region [Getter / Setter]
		public DefaultInputMapping GetDefaultInputMapping()
		{
			return inputMapping;
		}

		public void SetDefaultInputMapping(DefaultInputMapping value)
		{
			inputMapping = value;
		}

		public bool LockCursorOnStart()
		{
			return lockCursorOnStart;
		}

		public void LockCursorOnStart(bool value)
		{
			lockCursorOnStart = value;
		}
		#endregion
	}
}