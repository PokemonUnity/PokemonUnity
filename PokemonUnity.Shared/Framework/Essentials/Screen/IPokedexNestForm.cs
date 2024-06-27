using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Interface;
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
	/// <summary>
	/// Extension of <seealso cref="IGame"/>
	/// </summary>
	public interface IGamePokedexNest : IGame
	{
		bool FindEncounter(IEncounterPokemon encounter, Pokemons species);
	}

	/// <summary>
	/// Shows the "Nest" page of the Pokédex entry screen.
	/// </summary>
	public interface IPokemonNestMapScene : IScene
	{
		void Update();
		void EndScene();
		void StartScene(Pokemons species,int regionmap= -1);
		/// <summary>
		/// </summary>
		/// <param name="listlimits">an enum that represents end of list</param>
		/// <returns></returns>
		int MapScene(int listlimits);
	}
	public interface IPokemonNestMapScreen : IScreen
	{
		IPokemonNestMapScreen initialize(IPokemonNestMapScene scene);
		void StartScreen(Pokemons species,int region,int listlimits);
	}

	/// <summary>
	/// Shows the "Form" page of the Pokédex entry screen.
	/// </summary>
	public interface IPokemonFormScene : IScene
	{
		void Update();
		//void Refresh();
		List<PokemonUnity.Monster.Forms> GetAvailable(); //returns [Name, Gender, Form]
		List<string> GetCommands();
		void ChooseForm();
		void EndScene();
		void StartScene(Pokemons species);
		int Controls(int listlimits);
	}
	public interface IPokemonFormScreen : IScreen
	{
		IPokemonFormScreen initialize(IPokemonFormScene scene);
		void StartScreen(Pokemons species,int listlimits);
	}
}