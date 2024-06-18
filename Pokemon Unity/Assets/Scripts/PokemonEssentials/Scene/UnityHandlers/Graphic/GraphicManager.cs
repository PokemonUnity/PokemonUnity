using System;
using System.Collections;
using System.Collections.Generic;
using PokemonEssentials.Interface;
using PokemonUnity;
using UnityEngine;

namespace PokemonUnity.Interface.UnityEngine
{
	/// <summary>
	/// UnityEngine GraphicHandler Logic
	/// </summary>
	[RequireComponent(typeof(RectTransform),typeof(CanvasRenderer))]
	public class GraphicManager : MonoBehaviour, IGraphics
	{
		private RectTransform rect;
		private int frameCount;
		private int frameRate;
		private static float reciprocalFps; //readonly
		public event EventHandler<EventArgs> OnUpdate;
		//public static GraphicManager Instance { get; private set; }
		/// <summary>
		/// The number of times the screen is refreshed per second. The larger the value, the more CPU power is required. Normally set at 60.
		/// Changing this property is not recommended; however, it can be set anywhere from 10 to 120.
		/// </summary>
		public int frame_rate {
			get { return frame_rate; }
			set {

				if (value <= 0)
				{
					//throw new ArgumentException("Frames per second cannot be zero.");
					Core.Logger.LogWarning("Frames per second cannot be zero.");
					return;
				}
				if (value == frameRate) return;  // No change
				if (value >= 10 && value <= 120)
				{
					reciprocalFps = 1.0f / value;  // Precompute the reciprocal once
					frameRate = value;
				}
			}
		}
		/// <summary>
		/// The screen's refresh rate count. Set this property to 0 at game start and the game play time (in seconds)
		/// can be calculated by dividing this value by the frame_rate property value.
		/// </summary>
		public int frame_count { get { return frameCount; } set { frameCount = value; } }
		/// <summary>
		/// The brightness of the screen. Takes a value from 0 to 255.
		/// The fadeout, fade-in, and transition methods change this value internally, as required.
		/// </summary>
		/// ToDO: Takes a value from -126 to 126. Negative use black overlay, positive use white.
		public sbyte brightness { get; set; }
		public int width { get { return (int)rect.rect.width; } } //rect.sizeDelta.x
		public int height { get { return (int)rect.rect.height; } } //rect.sizeDelta.y
		public bool deletefailed { get; set; }

		#region Unity MonoBehavior Methods
		private void Awake()
		{
			rect = GetComponent<RectTransform>();
			//Instance = this;
			//if (Instance == null)
			//{
			//	Instance = this;
			//}
			//else if (Instance != this)
			//{
			//	Destroy(gameObject);
			//}
		}

		private void Start()
		{
			frameCount = 0;
		}

		private void Update()
		{
			frameCount++;
		}
		#endregion


		/// <summary>
		/// Performs a fade-in of the screen.
		/// </summary>
		/// <param name="duration">duration is the number of frames to spend on the fade-in.</param>
		public void fadein(float duration)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Performs a fade-in of the screen.
		/// </summary>
		/// <param name="duration">duration is the number of frames to spend on the fade-in.</param>
		public void fadein(int frames)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Performs a fade-out of the screen.
		/// </summary>
		/// <param name="duration">duration is the number of frames to spend on the fade-out.</param>
		public void fadeout(float duration)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Performs a fade-out of the screen.
		/// </summary>
		/// <param name="frames">duration is the number of frames to spend on the fade-out.</param>
		public void fadeout(int frames)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Resets the screen refresh timing. After a time-consuming process, call this method to prevent extreme frame skips.
		/// </summary>
		public void frame_reset()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Fixes the current screen in preparation for transitions.
		/// Screen rewrites are prohibited until the transition method is called.
		/// </summary>
		public void freeze()
		{
			throw new NotImplementedException();
		}

		public void resize_screen(int w, int h)
		{
			throw new NotImplementedException();
		}

		public ISprite snap_to_bitmap()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Carries out a transition from the screen fixed in Graphics.freeze to the current screen.
		/// </summary>
		/// <param name="duration">duration is the number of frames the transition will last. When omitted, this value is set to 8.</param>
		/// <param name="filename">filename specifies the transition graphic file name. When not specified, a standard fade will be used.</param>
		/// <param name="vague">vague sets the ambiguity of the borderline between the graphic's starting and ending points. The larger the value, the greater the ambiguity. When omitted, this value is set to 40.</param>
		public void transition(float duration, string filename, int vague = 40)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Refreshes the game screen and advances time by 1 frame. This method must be called at set intervals.
		/// </summary>
		/// <returns></returns>
		public IEnumerator update()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Waits for the specified number of frames.
		/// </summary>
		/// <example>
		/// do {
		///		<see cref="update"/>
		/// } while (duration.times)
		/// </example>
		/// <param name="duration"></param>
		public void wait(float duration)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Waits for the specified number of frames.
		/// </summary>
		/// <example>
		/// do {
		///		<see cref="update"/>
		/// } while (duration.times)
		/// </example>
		/// <param name="frames"></param>
		public void wait(int frames)
		{
			throw new NotImplementedException();
		}

		public float CalculateDuration(int frames)
		{
			return frames * reciprocalFps;  // Use multiplication instead of division
		}

		#region Explicit Interface
		#endregion
	}
}