using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;
using PokemonEssentials.Interface.Screen;

namespace PokemonEssentials.Interface.PokeBattle
{
	public interface IFakeBattler
	{
		IFakeBattler initialize(IPokemon pokemon, int index);

		int index { get; }
		IPokemon pokemon { get; }
		Pokemons species { get; }
		bool? gender { get; }
		int status { get; }
		int hp { get; }
		int level { get; }
		string name { get; }
		int totalhp { get; }
		bool owned { get; }
		bool isFainted { get; }
		bool isShiny { get; }
		bool isShadow { get; }
		bool hasMega { get; }
		bool isMega { get; }
		bool hasPrimal { get; }
		bool isPrimal { get; }
		/// <summary>
		/// Returns the gender of the pokemon for UI
		/// </summary>
		bool? displayGender { get; }
		bool captured { get; set; }

		//string pbThis(bool lowercase= false);
		string ToString(bool lowercase = false);
	}
}