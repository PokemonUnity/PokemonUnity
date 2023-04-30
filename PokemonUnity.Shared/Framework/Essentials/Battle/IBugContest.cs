using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonEssentials.Interface.Battle
{
	public interface IBugContestState 
	{
		int ballcount				        { get; }
		BattleResults decision				{ get; }
		IPokemon lastPokemon				{ get; }
		int timer				            { get; }
		string[] ContestantNames            { get; }
		//int TimerSeconds=BUGCONTESTTIME;

		IBugContestState initialize();

		bool pbContestHeld();

		bool expired();

		void clear();

		bool inProgress { get; }

		bool undecided { get; }

		bool decided { get; }
		void pbSetPokemon(IPokemon chosenpoke);

		/// <summary>
		/// Reception map is handled separately from contest map since the reception map
		/// can be outdoors, with its own grassy patches.
		/// </summary>
		/// <param name="arg"></param>
		void pbSetReception(params int[] arg);

		bool pbOffLimits(int map);

		void pbSetJudgingPoint(int startMap, float startX, float startY, int dir = 8);

		void pbSetContestMap(int map);

		void pbJudge();

		//[trainer type, pokemon, level?]
		void pbGetPlaceInfo(int place);

		void pbClearIfEnded();

		void pbStartJudging();

		/// <summary>
		/// Linq through a list of trainers, to confirm if id is present
		/// </summary>
		/// <param name="i">Trainer Types?</param>
		/// <returns></returns>
		bool pbIsContestant(int i);

		void pbStart(int ballcount);

		int place { get; }

		void pbEnd(bool interrupted = false);
	}

	/// <summary>
	/// Extension of <seealso cref="IGame"/>
	/// </summary>
	public interface IGameBugContest
	{
		/// <summary>
		/// Returns a score for this Pokemon in the Bug Catching Contest.
		/// Not exactly the HGSS calculation, but it should be decent enough.
		/// </summary>
		/// <param name="pokemon"></param>
		/// <returns></returns>
		int pbBugContestScore(IPokemon pokemon);

		IBugContestState pbBugContestState { get; }

		/// <summary>
		/// Returns true if the Bug Catching Contest in progress
		/// </summary>
		bool pbInBugContest { get; }

		/// <summary>
		/// Returns true if the Bug Catching Contest in progress and has not yet been judged
		/// </summary>
		bool pbBugContestUndecided { get; }

		/// <summary>
		/// Returns true if the Bug Catching Contest in progress and is being judged
		/// </summary>
		bool pbBugContestDecided { get; }


		void pbBugContestStartOver();

		BattleResults pbBugContestBattle(Pokemons species, int level);

		/// <summary>
		/// Fires whenever the player moves to a new map. Event handler receives the old
		/// map ID or 0 if none.  Also fires when the first map of the game is loaded
		/// </summary>
		event EventHandler OnMapChange;
		//Events.onMapChange+=delegate(object sender, EventArgs e) {
		//   pbBugContestState.pbClearIfEnded;
		//}

		/// <summary>
		/// Fires whenever the map scene is regenerated and soon after the player moves
		/// to a new map.
		/// </summary>
		//event EventHandler<IOnMapSceneChangeEventArgs> OnMapSceneChange;
		event Action<object, EventArg.IOnMapSceneChangeEventArgs> OnMapSceneChange;
		//Events.onMapSceneChange+=delegate(object sender, EventArgs e) {
		//   scene=e[0];
		//   mapChanged=e[1];
		//   if (pbInBugContest? && pbBugContestState.decision==0 && BugContestState.eimerSeconds>0) {
		//     scene.spriteset.addUserSprite(new TimerDisplay(
		//        pbBugContestState.timer,
		//        BugContestState.eimerSeconds*Graphics.frame_rate));
		//   }
		//}

		/// <summary>
		/// Fires each frame during a map update.
		/// </summary>
		event EventHandler OnMapUpdate;
		//Events.onMapUpdate+=delegate(object sender, EventArgs e) {
		//   if (!Game.GameData.Trainer || !Game.GameData.Global || !Game.GameData.GamePlayer || !Game.GameData.GameMap) {
		//    //  do nothing
		//   } else if (!Game.GameData.GamePlayer.move_route_forcing && !pbMapInterpreterRunning? &&
		//         !Game.GameData.GameTemp.message_window_showing) {
		//     if (pbBugContestState.expired?) {
		//       Kernel.pbMessage(_INTL("ANNOUNCER:  BEEEEEP!"));
		//       Kernel.pbMessage(_INTL("Time's up!"));
		//       pbBugContestState.pbStartJudging;
		//     }
		//   }
		//}

		/// <summary>
		/// Fires whenever one map is about to change to a different one. Event handler
		/// receives the new map ID and the Game_Map object representing the new map.
		/// When the event handler is called, Game.GameData.GameMap still refers to the old map.
		/// </summary>
		event EventHandler OnMapChanging;
		//Events.onMapChanging+=delegate(object sender, EventArgs e) {
		//   newmapID=e[0];
		//   newmap=e[1];
		//   if (pbInBugContest?) {
		//     if (pbBugContestState.pbOffLimits(newmapID)) {
		//       //  Clear bug contest if player flies/warps/teleports out of the contest
		//       pbBugContestState.pbEnd(true);
		//     }
		//   }
		//}

		/// <summary>
		/// Triggers at the start of a wild battle.  Event handlers can provide their own
		/// wild battle routines to override the default behavior.
		/// </summary>
		//event EventHandler<IOnWildBattleOverrideEventArgs> OnWildBattleOverride;
		event Action<object, EventArg.IOnWildBattleOverrideEventArgs> OnWildBattleOverride;
		//Events.onWildBattleOverride+= delegate(object sender, EventArgs e) {
		//   species=e[0];
		//   level=e[1];
		//   handled=e[2];
		//   if (handled[0]!=null) continue;
		//   if (!pbInBugContest?) continue;
		//   handled[0]=pbBugContestBattle(species,level);
		//}
	}

	public interface IBattle_BugContestBattle : IBattle 
	{
		int ballcount				{ get; }

		//IBattle_BugContestBattle initialize(IPokeBattle_Scene scene, Monster.Pokemon[] p1, Monster.Pokemon[] p2, Combat.Trainer[] player, Combat.Trainer[] opponent, int maxBattlers = 4);

		new Items pbItemMenu(int index);
		//KeyValuePair<Items, int> pbItemMenu(int index);

		new MenuCommands pbCommandMenu(int i);

		new void pbStorePokemon(IPokemon pokemon);

		new void pbEndOfRoundPhase();
	}

	/// <inheritdoc/>
	public interface ITimerDisplay : IDisposable
	{
		void initialize(int start, int maxtime);

		void dispose();

		bool disposed();

		void update();
	}
}