using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;
using PokemonUnity.Combat.Data;
using PokemonUnity.Character;
using PokemonUnity.Saving;
using PokemonUnity.Saving.SerializableClasses;
using PokemonUnity.Utility;
using PokemonEssentials.Interface.Screen;

namespace PokemonEssentials.Interface.PokeBattle
{
	public interface IBattleArena : IBattle
	{
		IBattleArena initialize(IPokeBattle_Scene scene, IPokemon[] p1, IPokemon[] p2, ITrainer[] player, ITrainer[] opponent);

		new bool pbDoubleBattleAllowed();

		bool pbEnemyShouldWithdraw(int index);

		new bool pbCanSwitchLax(int idxPokemon, int pkmnidxTo, bool showMessages);
		 
		new void pbSwitch(bool favorDraws= false);
		 
		new void pbOnActiveAll();

		new bool pbOnActiveOne(IBattler pkmn, bool onlyabilities = false, bool moldbreaker = false);

		int pbMindScore(IBattleMove move);

		new void pbCommandPhase();
		 
		new void pbEndOfRoundPhase();
	}

	public interface IPokeBattleArena_Scene : IPokeBattle_Scene
	{
		void pbBattleArenaBattlers(IBattler battler1, IBattler battler2);

		void pbBattleArenaJudgment(IBattler battler1, IBattler battler2, int[] ratings1, int[] ratings2);

		void updateJudgment(IWindow window, int phase, IBattler battler1, IBattler battler2, int[] ratings1, int[] ratings2);
	}
}