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
	/// <summary>
	/// </summary>
	public interface IBattleSwapScene : IScene 
	{
		//  Processes the scene
		void pbChoosePokemon(bool canCancel);

		void pbShowCommands(string[] commands);

		bool pbConfirm(string message);

		string[] pbGetCommands(IPokemon[] list, int[] choices);

		void pbUpdateChoices(int[] choices);

		void pbSwapChosen(int pkmnindex);

		void pbInitSwapScreen();

		void pbSwapCanceled();

		void pbSummary(IPokemon[] list, int index);

		//  Update the scene here, this is called once each frame
		void pbUpdate();

		//  End the scene here
		void pbEndScene();

		void pbStartSwapScene(IPokemon currentPokemon, IPokemon newPokemon);

		void pbStartRentScene(IPokemon[] rentals);
	}

	/// <summary>
	/// </summary>
	public interface IBattleSwapScreen : IScreen 
	{
		IBattleSwapScreen initialize(IBattleSwapScene scene);

		void pbStartRent(IPokemon[] rentals);

		bool pbStartSwap(IPokemon[] currentPokemon, IPokemon[] newPokemon);
	}
}