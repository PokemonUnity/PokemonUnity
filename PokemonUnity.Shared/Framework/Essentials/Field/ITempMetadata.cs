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
		int menuLastChoice			{ get; }
		int keyItemCalling			{ get; }
		int hiddenMoveEventCalling	{ get; }
		int begunNewGame			{ get; }
		int miniupdate				{ get; }
		int waitingTrainer			{ get; }
		int darknessSprite			{ get; }
		int pokemonDexData			{ get; }
		int pokemonMetadata			{ get; }
		int pokemonPhoneData		{ get; }
		int lastbattle				{ get; }
		int flydata					{ get; }

		//ITempMetadata initialize();
	}
}