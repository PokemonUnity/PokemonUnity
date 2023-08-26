using System;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.UX;

namespace PokemonEssentials.Interface
{
	/// <summary>
	/// <seealso cref="IInterpreter"/>
	/// </summary>
	public interface IInterpreterAutoscroll
	{
		/// <summary>
		/// Map Autoscroll to Coordinates
		/// </summary>
		/// <param name="x">x coordinate to scroll to and center on</param>
		/// <param name="y"> y coordinate to scroll to and center on</param>
		/// <param name="speed">(optional) scroll speed (from 1-6, default being 4)</param>
		/// <returns></returns>
		bool autoscroll(float x, float y, int speed = Core.SCROLL_SPEED_DEFAULT);
		/// <summary>
		/// Map Autoscroll (to Player)
		/// </summary>
		/// <param name="speed">(optional) scroll speed (from 1-6, default being 4)</param>
		void autoscroll_player(int speed = Core.SCROLL_SPEED_DEFAULT);
	}
	public interface IGameMapAutoscroll
	{
		void scroll_downright(int distance);
		void scroll_downleft(int distance);
		void scroll_upright(int distance);
		void scroll_upleft(int distance);
		void update_scrolling();
	}
}