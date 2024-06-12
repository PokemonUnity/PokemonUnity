using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Combat;
using PokemonUnity.Character;
using PokemonUnity.Inventory;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
//using PokemonEssentials.Interface.PokeBattle.Rules;
using PokemonEssentials.Interface.EventArg;

namespace PokemonEssentials.Interface.Screen
{
	public interface IPokemonEvolutionScene : IScene
	{
		IPokemonEvolutionScene initialize(); //Maybe not needed, since startscreen is called after this
		void EndScreen();
		void Evolution(bool cancancel = true);
		void FlashInOut(bool canceled,string oldstate,string oldstate2);
		void StartScreen(IPokemon pokemon, Pokemons newspecies);
		void Update(bool animating = false);
		void UpdateExpandScreen();
		void UpdateNarrowScreen();
	}
}