using System;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.UX;

namespace PokemonEssentials.Interface
{
	/// <summary>
	/// This class handles common events. It includes execution of parallel process
	/// event. This class is used within the Game_Map class (Game.GameData.GameMap).
	/// </summary>
	public interface IGameCommonEvent
	{
		/// <summary>
		/// Object Initialization
		/// </summary>
		/// <param name="common_event_id">common event ID</param>
		//IGameCommonEvent(int common_event_id);
		IGameCommonEvent initialize(int common_event_id);
		/// <summary>
		/// Get Name
		/// </summary>
		string name { get; }
		/// <summary>
		/// Get Trigger
		/// </summary>
		int trigger { get; }
		/// <summary>
		/// Get Condition Switch ID
		/// </summary>
		int switch_id { get; }
		/// <summary>
		/// Get List of Event Commands
		/// </summary>
		IList<PokemonEssentials.Interface.RPGMaker.Kernal.IEventCommand> list { get; }
		/// <summary>
		/// Checks if switch is on
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		bool switchIsOn(int id);
		/// <summary>
		/// Refresh
		/// </summary>
		void refresh();
		/// <summary>
		/// Frame Update
		/// </summary>
		IEnumerator update();
	}
}