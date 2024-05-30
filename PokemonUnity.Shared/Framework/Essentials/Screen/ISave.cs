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
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
using PokemonEssentials.Interface.EventArg;

namespace PokemonEssentials.Interface.Screen
{
	public interface IGameSave {

		void EmergencySave();

		bool Save(bool safesave = false);
	}

	public interface ISaveScene : IScene {
		void StartScreen();

		void EndScreen();
	}

	public interface ISaveScreen : IScreen {
		ISaveScreen initialize(ISaveScene scene);

		void Display(string text, bool brief = false);

		void DisplayPaused(string text);

		bool Confirm(string text);

		bool SaveScreen();
	}
}