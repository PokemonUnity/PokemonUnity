using System;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.UX;
using PokemonEssentials.Interface.Screen;

namespace PokemonEssentials.Interface
{
	/// <summary>
	/// This class handles the player. Its functions include event starting
	/// determinants and map scrolling. 
	/// </summary>
	/// Refer to "Game.GameData.GamePlayer" for the one instance of this class.
	public interface IGamePlayer : IGameCharacter
	{
		IAudioSE bump_se { get; set; }

		//IGameMap map { get; }
		//int bush_depth { get; }

		//IGamePlayer(Game_Map map);
		//IGamePlayer initialize(IGameMap map);

		bool pbHasDependentEvents();

		//void move_down(bool turn_enabled = true);
		//void move_left(bool turn_enabled = true);
		//void move_right(bool turn_enabled = true);
		//void move_up(bool turn_enabled = true);

		List<IGameEvent> pbTriggeredTrainerEvents(int[] triggers, bool checkIfRunning = true);

		List<IGameEvent> pbTriggeredCounterEvents(int[] triggers, bool checkIfRunning = true);

		void pbCheckEventTriggerAfterTurning();

		bool pbCheckEventTriggerFromDistance(int[] triggers);

		IEntity pbFacingEvent();
		/// <summary>
		/// Passable Determinants
		/// </summary>
		/// <param name="x">x-coordinate</param>
		/// <param name="y">y-coordinate</param>
		/// <param name="d">direction (0,2,4,6,8)
		///		* 0 = Determines if all directions are impassable (for jumping)</param>
		/// <returns></returns>
		//bool passable(float x, float y, int d);
		/// <summary>
		/// Set Map Display Position to Center of Screen
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		void center(float x, float y);
		/// <summary>
		/// Move to Designated Position
		/// </summary>
		/// <param name="x">x-coordinate</param>
		/// <param name="y">y-coordinate</param>
		//void moveto(int x, int y);
		/// <summary>
		/// Get Encounter Count
		/// </summary>
		int encounter_count { get; }
		/// <summary>
		/// Make Encounter Count
		/// </summary>
		void make_encounter_count();
		/// <summary>
		/// Refresh
		/// </summary>
		void refresh();
		/// <summary>
		/// Same Position Starting Determinant
		/// </summary>
		/// <param name="triggers"></param>
		/// <returns></returns>
		bool check_event_trigger_here(int[] triggers);
		/// <summary>
		/// Front Event Starting Determinant
		/// </summary>
		/// <param name="triggers"></param>
		/// <returns></returns>
		bool check_event_trigger_there(int[] triggers);
		/// <summary>
		/// Touch Event Starting Determinant
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		bool check_event_trigger_touch(float x, float y);
	}
}