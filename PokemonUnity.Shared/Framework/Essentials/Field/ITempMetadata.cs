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
	/// Temporary data which is not saved and which is erased when a game restarts.
	/// </summary>
	public interface ITempMetadata
	{
		int menuLastChoice									{ get; set; }
		int keyItemCalling									{ get; set; }
		int hiddenMoveEventCalling							{ get; set; }
		bool begunNewGame									{ get; set; }
		int miniupdate										{ get; set; }
		int waitingTrainer									{ get; set; }
		int darknessSprite									{ get; set; }
		IList<string> pokemonDexData						{ get; }
		IDictionary<int, IPokemonMetadata> pokemonMetadata	{ get; set; }
		IList<int> pokemonPhoneData							{ get; }
		int lastbattle										{ get; set; }
		int flydata											{ get; set; }

		//ITempMetadata initialize();
	}
}