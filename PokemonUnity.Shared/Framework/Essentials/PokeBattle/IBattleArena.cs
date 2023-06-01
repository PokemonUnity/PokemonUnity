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
		//new IBattleArena initialize(IScene scene, IPokemon[] p1, IPokemon[] p2, ITrainer[] player, ITrainer[] opponent);
		IBattleArena initialize(IPokeBattle_Scene scene, IPokemon[] p1, IPokemon[] p2, ITrainer[] player, ITrainer[] opponent);

		new bool DoubleBattleAllowed();

		bool EnemyShouldWithdraw(int index);

		new bool CanSwitchLax(int idxPokemon, int pkmnidxTo, bool showMessages);
		 
		new void Switch(bool favorDraws= false);
		 
		new void OnActiveAll();

		new bool OnActiveOne(IBattler pkmn, bool onlyabilities = false, bool moldbreaker = false);

		int MindScore(IBattleMove move);

		new void CommandPhase();
		 
		new void EndOfRoundPhase();
	}
}