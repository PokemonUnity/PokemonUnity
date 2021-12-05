using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Overworld;
using PokemonEssentials.Interface.Screen;

namespace PokemonEssentials.Interface
{
	public interface IGameMap
	{
		/// <summary>
		/// tileset file name
		/// </summary>
		string tileset_name { get; set; }
		/// <summary>
		/// autotile file name
		/// </summary>
		string[] autotile_names { get; set; }
		/// <summary>
		/// panorama file name
		/// </summary>
		string panorama_name { get; set; }
		/// <summary>
		/// panorama hue
		/// </summary>
		int panorama_hue { get; set; }
		/// <summary>
		/// fog file name
		/// </summary>
		string fog_name { get; set; }
		/// <summary>
		/// fog hue
		/// </summary>
		int fog_hue { get; set; }
		/// <summary>
		/// fog opacity level
		/// </summary>
		float fog_opacity { get; set; }
		/// <summary>
		/// fog blending method
		/// </summary>
		int fog_blend_type { get; set; }
		/// <summary>
		/// fog zoom rate
		/// </summary>
		int fog_zoom { get; set; }
		/// <summary>
		/// fog sx
		/// </summary>
		float fog_sx { get; set; }
		/// <summary>
		/// fog sy
		/// </summary>
		float fog_sy { get; set; }
		/// <summary>
		/// battleback file name
		/// </summary>
		string battleback_name { get; set; }
		/// <summary>
		/// display x-coordinate * 128
		/// </summary>
		int display_x { get; set; }
		/// <summary>
		/// display y-coordinate * 128
		/// </summary>
		int display_y { get; set; }
		/// <summary>
		/// refresh request flag
		/// </summary>
		bool need_refresh { get; set; }
		/// <summary>
		/// passage table
		/// </summary>
		int[] passages { get; set; }
		/// <summary>
		/// prioroty table
		/// </summary>
		int[] priorities { get; set; }
		/// <summary>
		/// terrain tag table
		/// </summary>
		Terrains[] terrain_tags { get; set; }
		/// <summary>
		/// events
		/// </summary>
		IDictionary<int, IGameEvent> events { get; set; }
		/// <summary>
		/// fog x-coordinate starting point
		/// </summary>
		float fog_ox { get; set; }
		/// <summary>
		/// fog y-coordinate starting point
		/// </summary>
		float fog_oy { get; set; }
		/// <summary>
		/// fog color tone
		/// </summary>
		ITone fog_tone { get; set; }
		int mapsInRange { get; set; }
		int width { get; }
		int height { get; }
		IList<Pokemons> encounter_list { get; }
		int encounter_step { get; }
		int?[,,] data { get; }
		string name { get; }

		//IGameMap initialize();
		void setup(int map_id);
		/// <summary>
		/// Autoplays background music.
		/// Plays music called "[normal BGM]n" if it's night time and it exists
		/// </summary>
		void autoplayAsCue();
		/// <summary>
		/// Plays background music
		/// Plays music called "[normal BGM]n" if it's night time and it exists
		/// </summary>
		void autoplay();
		void refresh();
		void scroll_down(int distance);
		void scroll_left(int distance);
		void scroll_right(int distance);
		void scroll_up(int distance);
		bool valid(float x, float y);
		bool validLax(float x, float y);
		bool passable(float x, float y, int d, IGameCharacter self_event = null);
		bool playerPassable(float x, float y, int d, IGameCharacter self_event = null);
		bool passableStrict(float x, float y, int d, IGameCharacter self_event = null);
		bool deepBush(float x, float y);
		bool bush(float x, float y);
		bool counter(float x, float y);
		Terrains terrain_tag(float x, float y, bool countBridge = false);
		int? check_event(float x, float y);
		void start_scroll(int direction, int distance, int speed);
		bool scrolling { get; }
		void start_fog_tone_change(ITone tone, int duration);
		void start_fog_opacity_change(float opacity, int duration);
		bool in_range(object @object);
		void update();
	}
}