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
	/// Scene class for handling appearance of the screen
	/// </summary>
	public interface IRelicStoneScene : IScene
	{
		IRelicStoneScene initialize(IScene scene);
		void Update();
		//void Refresh();
		void Purify();
		//void Confirm(string msg);
		void Display(string msg, bool brief = false);
		void StartScene(IPokemonShadowPokemon pokemon);
		void EndScene();
	}

	/// <summary>
	/// Screen class for handling game logic
	/// </summary>
	public interface IRelicStoneScreen : IScreen
	{
		IRelicStoneScreen initialize(IRelicStoneScene scene);
		void Refresh();
		void Confirm(string x);
		void Display(string x);
		void StartScreen(IPokemonShadowPokemon pokemon);
	}
}