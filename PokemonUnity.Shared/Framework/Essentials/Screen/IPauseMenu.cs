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
	public interface IPokemonMenuScene : IScene
	{
		void EndScene();
		void HideMenu();
		//void Refresh();
		void ShowCommands(string[] commands);
		void ShowHelp(string text);
		void ShowInfo(string text);
		void ShowMenu();
		void StartScene();
	}

	public interface IPokemonMenuScreen : IScreen
	{
		void initialize(IPokemonMenuScene scene);
		void ShowMenu();
		void StartPokemonMenu();
	}
}