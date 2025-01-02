using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.UX;
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
	public interface IGamePokedexNest
	{
		bool pbFindEncounter(IEncounter encounter, Pokemons species);
	}

	/// <summary>
	/// Shows the "Nest" page of the Pokédex entry screen.
	/// </summary>
	public interface IPokemonNestMapScene : IScene
	{
		void pbUpdate();
		void pbEndScene();
		void pbStartScene(Pokemons species,int regionmap= -1);
		/// <summary>
		/// </summary>
		/// <param name="listlimits">an enum that represents end of list</param>
		/// <returns></returns>
		int pbMapScene(int listlimits);
	}
	public interface IPokemonNestMapScreen : IScreen
	{
		IPokemonNestMapScreen initialize(IPokemonNestMapScene scene);
		void pbStartScreen(Pokemons species,int region,int listlimits);
	}

	/// <summary>
	/// Shows the "Form" page of the Pokédex entry screen.
	/// </summary>
	public interface IPokemonFormScene : IScene
	{
		void pbUpdate();
		//void pbRefresh();
		List<PokemonUnity.Monster.Forms> pbGetAvailable(); //returns [Name, Gender, Form] 
		List<string> pbGetCommands();
		void pbChooseForm();
		void pbEndScene();
		void pbStartScene(Pokemons species);
		int pbControls(int listlimits);
	}
	public interface IPokemonFormScreen : IScreen
	{
		IPokemonFormScreen initialize(IPokemonFormScene scene);
		void pbStartScreen(Pokemons species,int listlimits);
	}
}