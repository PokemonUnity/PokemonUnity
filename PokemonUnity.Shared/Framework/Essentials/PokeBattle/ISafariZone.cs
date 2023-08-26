using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;
using PokemonEssentials.Interface.Screen;

namespace PokemonEssentials.Interface.PokeBattle
{
	public interface IFakeBattler
	{
		IFakeBattler initialize(IPokemon pokemon, int index);

		int index { get; }
		IPokemon pokemon { get; }
		Pokemons species { get; }
		bool? gender { get; }
		int status { get; }
		int hp { get; }
		int level { get; }
		string name { get; }
		int totalhp { get; }
		bool owned { get; }
		bool isFainted { get; }
		bool isShiny { get; }
		bool isShadow { get; }
		bool hasMega { get; }
		bool isMega { get; }
		bool hasPrimal { get; }
		bool isPrimal { get; }
		/// <summary>
		/// Returns the gender of the pokemon for UI
		/// </summary>
		bool? displayGender { get; }
		bool captured { get; set; }

		//string pbThis(bool lowercase= false);
		string ToString(bool lowercase = false);
	}

	public interface ISafariZone : IBattleCommon
	{
        PokemonUnity.Overworld.Environments environment { get; set; }
		IPokemon[] party1 { get; }
		IPokemon[] party2 { get; }
		ITrainer[] player { get; }
		bool battlescene { get; set; }

		ISafariZone initialize(IPokeBattle_Scene scene, ITrainer player, IPokemon[] party2);

		bool pbIsOpposing(int index);

		bool pbIsDoubleBattler(int index);

		IBattler[] battlers { get; }
		ITrainer[] opponent { get; }
		bool doublebattle { get; }

		int ballCount { get; set; }

		ITrainer pbPlayer();

		void pbAbort();

		int pbEscapeRate(int rareness);

		BattleResults pbStartBattle();

		// ############
		void pbDebugUpdate();

		void pbDisplayPaused(string msg);

		void pbDisplay(string msg);

		void pbDisplayBrief(string msg);

		bool pbDisplayConfirm(string msg);

		//int pbAIRandom(int x);

		int pbRandom(int x);

		void pbGainEXP();
	}
}