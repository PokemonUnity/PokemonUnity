using System;
using System.Collections;
using System.Collections.Generic;

namespace PokemonEssentials.Interface.Field
{
	/// <summary>
	/// This class keeps track of erased and moved events so their position
	/// can remain after a game is saved and loaded.  This class also includes
	/// variables that should remain valid only for the current map.
	/// </summary>
	public interface IMapMetadata
	{
		IList<int> erasedEvents	{ get; set; }
		IList<int> movedEvents	{ get; set; }
		bool strengthUsed		{ get; set; }
		bool blackFluteUsed		{ get; set; }
		bool whiteFluteUsed		{ get; set; }

		//IMapMetadata initialize();

		void clear();

		void addErasedEvent(int eventID);

		void addMovedEvent(int eventID);

		void updateMap();
	}
}