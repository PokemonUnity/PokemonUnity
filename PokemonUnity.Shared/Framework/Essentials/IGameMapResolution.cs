using PokemonUnity.UX;
using System.Collections;
using System.Collections.Generic;

namespace PokemonEssentials.Interface
{
	public interface IGameMapResolution
	{
		//int TILEWIDTH { get; set; }
		//int TILEHEIGHT { get; set; }
		//int XSUBPIXEL { get; set; }
		//int YSUBPIXEL { get; set; }
		int realResX { get; set; }
		int realResY { get; set; }
		/// <summary>
		/// display x-coordinate * 128
		/// </summary>
		int display_x { get; set; }
		/// <summary>
		/// display y-coordinate * 128
		/// </summary>
		int display_y { get; set; }
		void scroll_down(int distance);
		void scroll_left(int distance);
		void scroll_right(int distance);
		void scroll_up(int distance);
	}
}