using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonEssentials.Interface.Screen
{
	public interface ISafariZone_Scene : IScene, IBattleCommon
	{
		PokemonUnity.Overworld.Environments environment { get; set; }
		IPokemon[] party1 { get; }
		IPokemon[] party2 { get; }
		ITrainer[] player { get; }
		bool battlescene { get; set; }

		ISafariZone_Scene initialize(IPokeBattle_Scene scene, ITrainer player, IPokemon[] party2);

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

		//void pbDisplay(string msg);

		void pbDisplayBrief(string msg);

		//bool pbDisplayConfirm(string msg);

		//int pbAIRandom(int x);

		int pbRandom(int x);

		void pbGainEXP();
	}
}