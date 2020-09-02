using System;
using System.Collections.Generic;
using PokemonUnity.Attack;
using PokemonUnity.Character;
using PokemonUnity.Inventory;
using PokemonUnity;
using PokemonUnity.Combat.Data;

namespace PokemonUnity.Combat
{
	public interface IShadowPokemon //Rename ShadowBattler?
	{
		void InitPokemon(Monster.Pokemon pkmn, sbyte pkmnIndex);
		void pbEndTurn(Choice choice);
		bool isShadow();
		bool inHyperMode();
		void pbHyperMode();
		bool pbHyperModeObedience(Move move);
	}
}