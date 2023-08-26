using PokemonUnity;
using PokemonUnity.Overworld;
using PokemonUnity.UX;
using System;
using System.Collections;
using System.Collections.Generic;

namespace PokemonEssentials.Interface
{
	public interface IGameScreen
	{
		/// <summary>
		/// brightness
		/// </summary>
		int brightness { get; set; }
		/// <summary>
		/// color tone
		/// </summary>
		ITone tone { get; set; }
		/// <summary>
		/// flash color
		/// </summary>
		IColor flash_color { get; set; }
		/// <summary>
		/// shake positioning
		/// </summary>
		int shake { get; set; }
		/// <summary>
		/// pictures
		/// </summary>
		List<IGamePicture> pictures { get; set; }
		/// <summary>
		/// weather type
		/// </summary>
		FieldWeathers weather_type { get; set; }
		/// <summary>
		/// max number of weather sprites
		/// </summary>
		int weather_max { get; set; }

		/// <summary>
		/// Start Changing Color Tone
		/// </summary>
		/// <param name="tone">color tone</param>
		/// <param name="duration">time</param>
		void start_tone_change(ITone tone, float duration);
		/// <summary>
		/// Start Flashing
		/// </summary>
		/// <param name="color">color</param>
		/// <param name="duration">time</param>
		void start_flash(IColor color, int duration);
		/// <summary>
		/// Start Shaking
		/// </summary>
		/// <param name="power">strength</param>
		/// <param name="speed">speed</param>
		/// <param name="duration">time</param>
		void start_shake(int power, int speed, int duration);
		/// <summary>
		/// Set Weather
		/// </summary>
		/// <param name="type">type</param>
		/// <param name="power">strength</param>
		/// <param name="duration">time</param>
		void weather(FieldWeathers type, int power, int duration);
		/// <summary>
		/// Frame Update
		/// </summary>
		/// <returns></returns>
		IEnumerator update();
	}
}