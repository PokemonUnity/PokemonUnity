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

		bool IsOpposing(int index);

		bool IsDoubleBattler(int index);

		IBattler[] battlers { get; }
		ITrainer[] opponent { get; }
		bool doublebattle { get; }

		int ballCount { get; set; }

		ITrainer Player();

		void Abort();

		int EscapeRate(int rareness);

		BattleResults StartBattle();

		// ############
		void DebugUpdate();

		void DisplayPaused(string msg);

		//void Display(string msg);

		void DisplayBrief(string msg);

		//bool DisplayConfirm(string msg);

		//int AIRandom(int x);

		int Random(int x);

		void GainEXP();
	}
}