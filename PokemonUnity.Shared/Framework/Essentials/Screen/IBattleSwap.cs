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
	/// </summary>
	public interface IBattleSwapScene : IScene
	{
		/// <summary>
		/// Processes the scene
		/// </summary>
		/// <param name="canCancel"></param>
		void ChoosePokemon(bool canCancel);

		void ShowCommands(string[] commands);

		bool Confirm(string message);

		string[] GetCommands(IPokemon[] list, int[] choices);

		void UpdateChoices(int[] choices);

		void SwapChosen(int pkmnindex);

		void InitSwapScreen();

		void SwapCanceled();

		void Summary(IPokemon[] list, int index);

		/// <summary>
		/// Update the scene here, this is called once each frame
		/// </summary>
		void Update();

		/// <summary>
		/// End the scene here
		/// </summary>
		void EndScene();

		void StartSwapScene(IPokemon currentPokemon, IPokemon newPokemon);

		void StartRentScene(IPokemon[] rentals);
	}

	/// <summary>
	/// </summary>
	public interface IBattleSwapScreen : IScreen
	{
		IBattleSwapScreen initialize(IBattleSwapScene scene);

		IPokemon[] StartRent(IPokemon[] rentals);

		bool StartSwap(IPokemon[] currentPokemon, IPokemon[] newPokemon);
	}
}