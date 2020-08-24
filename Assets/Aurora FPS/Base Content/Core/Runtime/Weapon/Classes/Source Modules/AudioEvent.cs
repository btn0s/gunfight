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
	/// Audio Event component used for playing specific sound in weapon animation.
	/// </summary>
	[RequireComponent(typeof(AudioSource))]
	[System.Obsolete("AudioEvent component is obsolete.\nUse animation event properties.")]
	public class AudioEvent : MonoBehaviour
	{
		private AudioSource audioSource;

		/// <summary>
		/// Start is called on the frame when a script is enabled just before
		/// any of the Update methods is called the first time.
		/// </summary>
		protected virtual void Start()
		{
			audioSource = GetComponent<AudioSource>();
		}

		/// <summary>
		/// Play specific sound
		/// </summary>
		/// <param name="audioClip"></param>
		public virtual void PlaySound(AudioClip audioClip)
		{
			if (audioSource == null)
				return;

			audioSource.clip = audioClip;
			audioSource.Play();
		}
	}
}