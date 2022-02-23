using PokemonUnity.UX;
using System.Collections;

namespace PokemonEssentials.Interface
{
	public interface IGamePicture 
	{
		/// <summary>
		/// picture number
		/// </summary>
		int number { get; set; }
		/// <summary>
		/// file name
		/// </summary>
		string name { get; set; }
		/// <summary>
		/// starting point
		/// </summary>
		float origin { get; set; }
		/// <summary>
		/// The picture's X-coordinate.
		/// </summary>
		float x { get; set; }
		/// <summary>
		/// The picture's Y-coordinate.
		/// </summary>
		float y { get; set; }
		/// <summary>
		/// The picture's X-axis zoom level. 1.0 denotes actual pixel size.
		/// </summary>
		float zoom_x { get; set; }
		/// <summary>
		/// The picture's Y-axis zoom level. 1.0 denotes actual pixel size.
		/// </summary>
		float zoom_y { get; set; }
		/// <summary>
		/// The sprite's opacity (0-255). Values out of range are automatically corrected.
		/// </summary>
		float opacity { get; set; }
		/// <summary>
		/// The sprite's blending mode (0: normal, 1: addition, 2: subtraction).
		/// </summary>
		int blend_type { get; set; }
		/// <summary>
		/// color tone
		/// </summary>
		ITone tone { get; set; }
		/// <summary>
		/// The sprite's angle of rotation. Specifies up to 360 degrees of counterclockwise rotation.
		/// </summary>
		float angle { get; set; }

		/// <summary>
		/// If a flash or wave is not needed, it is not necessary to call this.
		/// </summary>
		/// <returns></returns>
		IEnumerator update();
		/// <summary>
		/// Show Picture
		/// </summary>
		/// <param name="name">file name</param>
		/// <param name="origin">starting point</param>
		/// <param name="x">x-coordinate</param>
		/// <param name="y">y-coordinate</param>
		/// <param name="zoom_x">x directional zoom rate</param>
		/// <param name="zoom_y">y directional zoom rate</param>
		/// <param name="opacity">opacity level</param>
		/// <param name="blend_type">blend method</param>
		void show(string name, int origin, float x, float y, float zoom_x, float zoom_y, float opacity, int? blend_type);
		/// <summary>
		/// Move Picture
		/// </summary>
		/// <param name="duration">time</param>
		/// <param name="origin">starting point</param>
		/// <param name="x">x-coordinate</param>
		/// <param name="y">y-coordinate</param>
		/// <param name="zoom_x">x directional zoom rate</param>
		/// <param name="zoom_y">y directional zoom rate</param>
		/// <param name="opacity">opacity level</param>
		/// <param name="blend_type">blend method</param>
		void move(int duration, int origin, float x, float y, float zoom_x, float zoom_y, float opacity, int? blend_type);
		/// <summary>
		/// Change Rotation Speed
		/// </summary>
		/// <param name="speed">rotation speed</param>
		void rotate(int speed);
		/// <summary>
		/// Start Change of Color Tone
		/// </summary>
		/// <param name="tone">color tone</param>
		/// <param name="duration">time</param>
		void start_tone_change(ITone tone, float duration);
		/// <summary>
		/// Erase Picture
		/// </summary>
		void erase();
		IGamePicture initialize(int number);
	}
}