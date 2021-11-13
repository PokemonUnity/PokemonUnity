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
	/// Scene class for handling appearance of the screen
	/// </summary>
	public interface IRelicStoneScene : IScene
	{
		IRelicStoneScene initialize(IScene scene);
		void pbUpdate();
		//void pbRefresh();
		void pbPurify();
		//void pbConfirm(string msg);
		void pbDisplay(string msg, bool brief = false);
		void pbStartScene(IPokemon pokemon);
		void pbEndScene();
	}

	/// <summary>
	/// Screen class for handling game logic
	/// </summary>
	public interface IRelicStoneScreen : IScreen
	{
		IRelicStoneScreen initialize(IRelicStoneScene scene);
		void pbRefresh();
		void pbConfirm(string x);
		void pbDisplay(string x);
		void pbStartScreen(IPokemon pokemon);
	}
}