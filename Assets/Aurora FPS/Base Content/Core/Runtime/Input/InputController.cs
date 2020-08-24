/* ==================================================================
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
	/// Input Controller interface, base interface for all controllers used in AuroraFPSRuntime.
	/// Contains main functions for implementation.
	/// </summary>
	public abstract class InputController : Singleton<InputController>
	{
		/// <summary>
		/// Returns the value of the virtual axis identified by axisName.
		/// The value will be in the range -1...1 for keyboard and joystick input.
		/// </summary>
		public abstract float GetAxis(string axisName);

		/// <summary>
		/// Returns the value of the virtual axis identified by axisName with no smoothing filtering applied.
		/// The value will be in the range -1...1 for keyboard and joystick input. 
		/// Since input is not smoothed, keyboard input will always be either -1, 0 or 1.
		/// </summary>
		public abstract float GetAxisRaw(string axisName);

		/// <summary>
		/// Returns true during the frame the user pressed down the virtual button identified by buttonName.
		/// </summary>
		public abstract bool GetButtonDown(string butoonName);

		/// <summary>
		/// /// Returns true while the virtual button identified by buttonName is held down.
		/// </summary>
		public abstract bool GetButton(string buttonName);

		/// <summary>
		/// Returns true the first frame the user releases the virtual button identified by buttonName.
		/// </summary>
		public abstract bool GetButtonUp(string buttonName);
	}
}