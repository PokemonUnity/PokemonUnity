using System;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.UX;
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonEssentials.Interface.Field
{
	/// <summary>
	/// This class keeps track of erased and moved events so their position
	/// can remain after a game is saved and loaded.  This class also includes
	/// variables that should remain valid only for the current map.
	/// </summary>
	public interface IMapMetadata
	{
		int erasedEvents	{ get; set; }
		int movedEvents		{ get; set; }
		int strengthUsed	{ get; set; }
		int blackFluteUsed	{ get; set; }
		int whiteFluteUsed	{ get; set; }

		//IMapMetadata initialize();

		void clear();

		void addErasedEvent(int eventID);

		void addMovedEvent(int eventID);

		void updateMap();
	}
}