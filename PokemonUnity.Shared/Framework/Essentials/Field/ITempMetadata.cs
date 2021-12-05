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
		int menuLastChoice									{ get; }
		int keyItemCalling									{ get; }
		int hiddenMoveEventCalling							{ get; }
		bool begunNewGame									{ get; }
		int miniupdate										{ get; }
		int waitingTrainer									{ get; }
		int darknessSprite									{ get; }
		IList<string> pokemonDexData						{ get; }
		IDictionary<int, IPokemonMetadata> pokemonMetadata	{ get; }
		IList<int> pokemonPhoneData							{ get; }
		int lastbattle										{ get; }
		int flydata											{ get; }

		//ITempMetadata initialize();
	}
}