using System;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.UX;

namespace PokemonEssentials.Interface
{
	public interface IGameCharacter : IEntity
	{
		int id { get; set; }
		int x { get; set; }
		int y { get; set; }
		int real_x { get; set; }
		int real_y { get; set; }
		int tile_id { get; set; }
		string character_name { get; set; }
		int character_hue { get; set; }
		int opacity { get; set; }
		int blend_type { get; set; }
		int direction { get; set; }
		int pattern { get; set; }
		bool move_route_forcing { get; set; }
		bool through { get; set; }
		int animation_id { get; set; }
		bool transparent { get; set; }
		IGameMap map { get; set; }
		int move_speed { get; set; }
		bool walk_anime { get; set; }

		bool step_anime { get; set; }
		float anime_count { get; set; }
		int jump_count { get; set; }
		int stop_count { get; set; }
		float jump_peak { get; set; }
		int wait_count { get; set; }
		int move_route_index { get; set; }
		bool direction_fix { get; set; }
		bool locked { get; set; }
		bool always_on_top { get; set; }
		float? oldX { get; set; }
		float? oldY { get; set; }
		int? oldMap { get; set; }
		//IMoveRoute move_route { get; set; }
		int? original_move_route { get; set; }
		int? original_move_route_index { get; set; }
		int prelock_direction { get; set; }

		//IGameCharacter(Game_Map map = null);
		IGameCharacter initialize(IGameMap map = null);

		bool moving { get; }

		bool jumping
		{
			get;
		}

		//IBitmap pattern { set; }

		void straighten();

		void force_move_route(int move_route);

		bool passableEx(float x, float y, int d, bool strict = false);

		/// <summary>
		/// Passable Determinants
		/// </summary>
		/// <param name="x">x-coordinate</param>
		/// <param name="y">y-coordinate</param>
		/// <param name="d">direction (0,2,4,6,8)
		///		* 0 = Determines if all directions are impassable (for jumping)</param>
		bool passable(float x, float y, int d);

		bool passableStrict(float x, float y, int d);

		void _lock();

		bool islock
		{
			get;
		}

		void unlock();

		void triggerLeaveTile();

		/// <summary>
		/// Move to Designated Position
		/// </summary>
		/// <param name="x">x-coordinate</param>
		/// <param name="y">y-coordinate</param>
		void moveto(int x, int y);

		int screen_x
		{
			get;
		}

		int screen_y
		{
			get;
		}

		int screen_y_ground();

		int screen_z(int height = 0);

		int bush_depth
		{
			get;
		}

		int terrain_tag
		{
			get;
		}

		#region Updating stuff
		IEnumerator update();

		void update_jump();

		void update_move();

		void update_stop();

		void move_type_random();

		void move_type_toward_player();

		void move_type_custom();

		void increase_steps();
		#endregion

		#region Movement stuff
		void move_down(bool turn_enabled = true);

		void move_left(bool turn_enabled = true);

		void move_right(bool turn_enabled = true);

		void move_up(bool turn_enabled = true);

		void move_lower_left();

		void move_lower_right();

		void move_upper_left();

		void move_upper_right();

		void moveLeft90();

		void moveRight90();

		void move_random();

		void move_toward_player();

		void move_away_from_player();

		void move_forward();

		void move_backward();

		void jump(int x_plus, int y_plus);

		void jumpForward();

		void jumpBackward();

		void turnGeneric(int dir);

		void turn_down();

		void turn_left();

		void turn_right();

		void turn_up();

		void turn_right_90();

		void turn_left_90();

		void turn_180();

		void turn_right_or_left_90();

		void turn_random();

		void turn_toward_player();

		void turn_away_from_player();

		void minilock();
		#endregion
	}
}