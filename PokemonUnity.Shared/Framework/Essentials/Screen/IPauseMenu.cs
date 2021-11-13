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
	public interface IPokemonMenu_Scene : IScene
	{
		void pbEndScene();
		void pbHideMenu();
		//void pbRefresh();
		void pbShowCommands(string[] commands);
		void pbShowHelp(string text);
		void pbShowInfo(string text);
		void pbShowMenu();
		void pbStartScene();
	}

	public interface IPokemonMenu : IScreen
	{
		void initialize(IPokemonMenu_Scene scene);
		void pbShowMenu();
		void pbStartPokemonMenu();
	}
}