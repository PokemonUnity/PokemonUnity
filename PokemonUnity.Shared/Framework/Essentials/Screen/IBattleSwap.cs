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
		/// <summary>
		/// Processes the scene
		/// </summary>
		/// <param name="canCancel"></param>
		void pbChoosePokemon(bool canCancel);

		void pbShowCommands(string[] commands);

		bool pbConfirm(string message);

		string[] pbGetCommands(IPokemon[] list, int[] choices);

		void pbUpdateChoices(int[] choices);

		void pbSwapChosen(int pkmnindex);

		void pbInitSwapScreen();

		void pbSwapCanceled();

		void pbSummary(IPokemon[] list, int index);

		/// <summary>
		/// Update the scene here, this is called once each frame
		/// </summary>
		void pbUpdate();

		/// <summary>
		/// End the scene here
		/// </summary>
		void pbEndScene();

		void pbStartSwapScene(IPokemon currentPokemon, IPokemon newPokemon);

		void pbStartRentScene(IPokemon[] rentals);
	}

	/// <summary>
	/// </summary>
	public interface IBattleSwapScreen : IScreen
	{
		IBattleSwapScreen initialize(IBattleSwapScene scene);

		IPokemon[] pbStartRent(IPokemon[] rentals);

		bool pbStartSwap(IPokemon[] currentPokemon, IPokemon[] newPokemon);
	}
}