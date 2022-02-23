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
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
using PokemonEssentials.Interface.EventArg;

namespace PokemonEssentials.Interface.Screen
{
	public interface ITrainerCardScene : IScene {
		void update();

		void pbStartScene();

		void pbDrawTrainerCardFront();

		IEnumerator pbTrainerCard();

		void pbEndScene();
	}

	public interface ITrainerCardScreen : IScreen {
		ITrainerCardScreen initialize(ITrainerCardScene scene);

		void pbStartScreen();
	}
}