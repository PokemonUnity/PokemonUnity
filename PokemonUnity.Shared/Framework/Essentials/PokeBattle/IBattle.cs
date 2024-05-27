using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle.Effects;

namespace PokemonEssentials.Interface.PokeBattle
{
	public interface IBattleCommon
	{
		void StorePokemon(IPokemon pokemon);

		void ThrowPokeball(int idxPokemon, Items ball, int? rareness = null, bool showplayer = false);
	}

	/// <summary>
	/// Main battle class.
	/// </summary>
	public interface IBattle : IBattleCommon
	{
		#region Variable
		/// <summary>
		/// Scene object for this battle
		/// </summary>
		IScene scene { get; }
		/// <summary>
		/// Decision: 0=undecided; 1=win; 2=loss; 3=escaped; 4=caught
		/// </summary>
		BattleResults decision { get; set; }
		/// <summary>
		/// Internal battle flag
		/// </summary>
		bool internalbattle { get; set; }
		/// <summary>
		/// Double battle flag
		/// </summary>
		bool doublebattle { get; set; }
		/// <summary>
		/// True if player can't escape
		/// </summary>
		bool cantescape { get; set; }
		/// <summary>
		/// Shift/Set "battle style" option
		/// </summary>
		bool shiftStyle { get; set; }
		/// <summary>
		/// "Battle scene" option
		/// </summary>
		bool battlescene { get; set; }
		/// <summary>
		/// Debug flag
		/// </summary>
		bool debug { get; }
		/// <summary>
		/// Player trainer
		/// </summary>
		ITrainer[] player { get; }
		/// <summary>
		/// Opponent trainer
		/// </summary>
		ITrainer[] opponent { get; }
		/// <summary>
		/// Player's Pokémon party
		/// </summary>
		IPokemon[] party1 { get; }
		/// <summary>
		/// Foe's Pokémon party
		/// </summary>
		IPokemon[] party2 { get; }
		/// <summary>
		/// Order of Pokémon in the player's party
		/// </summary>
		IList<int> party1order { get; }
		/// <summary>
		/// Order of Pokémon in the opponent's party
		/// </summary>
		IList<int> party2order { get; }
		/// <summary>
		/// True if player's party's max size is 6 instead of 3
		/// </summary>
		bool fullparty1 { get; set; }
		/// <summary>
		/// True if opponent's party's max size is 6 instead of 3
		/// </summary>
		bool fullparty2 { get; }
		/// <summary>
		/// Currently active Pokémon
		/// </summary>
		IBattler[] battlers { get; }
		/// <summary>
		/// Items held by opponents
		/// </summary>
		Items[][] items { get; set; }
		/// <summary>
		/// Effects common to each side of a battle
		/// </summary>
		IEffectsSide[] sides { get; }
		/// <summary>
		/// Effects common to the whole of a battle
		/// </summary>
		IEffectsField field { get; }
		/// <summary>
		/// Battle surroundings
		/// </summary>
		PokemonUnity.Overworld.Environments environment { get; set; }
		/// <summary>
		/// Current weather, custom methods should use <see cref="Weather"/> instead
		/// </summary>
		Weather weather { get; set; }
		/// <summary>
		/// Duration of current weather, or -1 if indefinite
		/// </summary>
		int weatherduration { get; set; }
		/// <summary>
		/// True if during the switching phase of the round
		/// </summary>
		bool switching { get; }
		/// <summary>
		/// True if Future Sight is hitting
		/// </summary>
		bool futuresight { get; }
		/// <summary>
		/// The Struggle move
		/// </summary>
		IBattleMove struggle { get; }
		/// <summary>
		/// Choices made by each Pokémon this round
		/// </summary>
		IBattleChoice[] choices { get; }
		/// <summary>
		/// Success states
		/// </summary>
		ISuccessState[] successStates { get; }
		/// <summary>
		/// Last move used
		/// </summary>
		Moves lastMoveUsed { get; set; }
		/// <summary>
		/// Last move user
		/// </summary>
		int lastMoveUser { get; set; }
		/// <summary>
		/// Battle index of each trainer's Pokémon to Mega Evolve
		/// </summary>
		int[][] megaEvolution { get; }
		/// <summary>
		/// Whether Amulet Coin's effect applies
		/// </summary>
		bool amuletcoin { get; }
		/// <summary>
		/// Money gained in battle by using Pay Day
		/// </summary>
		int extramoney { get; set; }
		/// <summary>
		/// Whether Happy Hour's effect applies
		/// </summary>
		bool doublemoney { get; set; }
		/// <summary>
		/// Speech by opponent when player wins
		/// </summary>
		string endspeech { get; set; }
		/// <summary>
		/// Speech by opponent when player wins
		/// </summary>
		string endspeech2 { get; set; }
		/// <summary>
		/// Speech by opponent when opponent wins
		/// </summary>
		string endspeechwin { get; set; }
		/// <summary>
		/// Speech by opponent when opponent wins
		/// </summary>
		string endspeechwin2 { get; set; }
		IDictionary<string, bool> rules { get; }
		int turncount { get; set; }
		//IBattler[] Priority { get; }
		//bool controlPlayer { get; set; }
		#endregion

		void Abort();

		IEnumerator DebugUpdate();

		int Random(int x);

		int AIRandom(int x);

		#region Initialize battle class.
		IBattle initialize(IScene scene, IPokemon[] p1, IPokemon[] p2, ITrainer player, ITrainer opponent);
		IBattle initialize(IScene scene, IPokemon[] p1, IPokemon[] p2, ITrainer[] player, ITrainer[] opponent);
		//IBattle initialize(IScene scene, IList<IPokemon> p1, IList<IPokemon> p2, IList<ITrainer> player, IList<ITrainer> opponent);
		#endregion

		#region Info about battle.
		bool DoubleBattleAllowed();

		Weather Weather { get; }
		#endregion

		#region Get battler info.
		bool IsOpposing(int index);

		bool OwnedByPlayer(int index);

		bool IsDoubleBattler(int index);

		/// <summary>
		/// Only used for Wish
		/// </summary>
		/// <param name="battlerindex"></param>
		/// <param name="pokemonindex"></param>
		/// <returns></returns>
		//string ThisEx(battlerindex, pokemonindex);
		string ToString(int battlerindex, int pokemonindex);

		/// <summary>
		/// Checks whether an item can be removed from a Pokémon.
		/// </summary>
		/// <param name="pkmn"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		bool IsUnlosableItem(IBattler pkmn, Items item);

		IBattler CheckGlobalAbility(Abilities a);

		int nextPickupUse { get; }
		#endregion

		#region Player-related info.
		ITrainer Player();

		Items[] GetOwnerItems(int battlerIndex);

		void SetSeen(IPokemon pokemon);

		string GetMegaRingName(int battlerIndex);

		bool HasMegaRing(int battlerIndex);
		#endregion

		#region Get party info, manipulate parties.
		int PokemonCount(IPokemon[] party);

		bool AllFainted(IPokemon[] party);

		int MaxLevel(IPokemon[] party);

		int MaxLevelFromIndex(int index);

		/// <summary>
		/// Gets player party of selected battler
		/// </summary>
		/// <param name="index"></param>
		/// <returns>Returns the trainer party of pokemon at this index?</returns>
		IPokemon[] Party(int index);

		IPokemon[] OpposingParty(int index);

		int SecondPartyBegin(int battlerIndex);

		int PartyLength(int battlerIndex);

		int FindNextUnfainted(IPokemon[] party, int start, int finish = -1);

		int GetLastPokeInTeam(int index);

		IBattler FindPlayerBattler(int pkmnIndex);

		bool IsOwner(int battlerIndex, int partyIndex);

		ITrainer GetOwner(int battlerIndex);

		ITrainer GetOwnerPartner(int battlerIndex);

		int GetOwnerIndex(int battlerIndex);

		bool BelongsToPlayer(int battlerIndex);

		ITrainer PartyGetOwner(int battlerIndex, int partyIndex);

		void AddToPlayerParty(IPokemon pokemon);

		void RemoveFromParty(int battlerIndex, int partyIndex);
		#endregion

		#region Check whether actions can be taken.
		/// <summary>
		/// Check whether actions can be taken.
		/// </summary>
		/// <param name="idxPokemon"></param>
		/// <returns></returns>
		bool CanShowCommands(int idxPokemon);
		#endregion

		#region Attacking.
		bool CanShowFightMenu(int idxPokemon);

		bool CanChooseMove(int idxPokemon, int idxMove, bool showMessages, bool sleeptalk = false);

		void AutoChooseMove(int idxPokemon, bool showMessages = true);

		bool RegisterMove(int idxPokemon, int idxMove, bool showMessages = true);

		bool ChoseMove(int i, Moves move);

		bool ChoseMoveFunctionCode(int i, PokemonUnity.Attack.Effects code);

		bool RegisterTarget(int idxPokemon, int idxTarget);

		IBattler[] Priority(bool ignorequickclaw = false, bool log = false);
		#endregion

		#region Switching Pokémon.
		bool CanSwitchLax(int idxPokemon, int pkmnidxTo, bool showMessages);

		bool CanSwitch(int idxPokemon, int pkmnidxTo, bool showMessages, bool ignoremeanlook = false);

		bool RegisterSwitch(int idxPokemon, int idxOther);

		bool CanChooseNonActive(int index);

		void Switch(bool favorDraws= false);

		void SendOut(int index, IPokemon pokemon);

		void Replace(int index, int newpoke, bool batonpass = false);

		bool RecallAndReplace(int index, int newpoke, int newpokename = -1, bool batonpass = false, bool moldbreaker = false);

		void MessagesOnReplace(int index, int newpoke, int newpokename= -1);

		int SwitchInBetween(int index, bool lax, bool cancancel);

		int SwitchPlayer(int index, bool lax, bool cancancel);
		#endregion

		#region Using an item.
		/// <summary>
		/// Uses an item on a Pokémon in the player's party.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="pkmnIndex"></param>
		/// <param name="userPkmn"></param>
		/// <param name="scene"></param>
		/// <returns></returns>
		bool UseItemOnPokemon(Items item, int pkmnIndex, IBattler userPkmn, IHasDisplayMessage scene);

		/// <summary>
		/// Uses an item on an active Pokémon.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="index"></param>
		/// <param name="userPkmn"></param>
		/// <param name="scene"></param>
		/// <returns></returns>
		bool UseItemOnBattler(Items item, int index, IBattler userPkmn, IHasDisplayMessage scene);

		bool RegisterItem(int idxPokemon, Items idxItem, int? idxTarget = null);

		void EnemyUseItem(Items item, IBattler battler);
		#endregion

		#region Fleeing from battle.
		bool CanRun(int idxPokemon);

		/// <summary>
		/// </summary>
		/// <param name="idxPokemon"></param>
		/// <param name="duringBattle"></param>
		/// <returns>1 = success; -1 = failed</returns>
		int Run(int idxPokemon, bool duringBattle= false);
		#endregion

		#region Mega Evolve battler.
		bool CanMegaEvolve(int index);

		void RegisterMegaEvolution(int index);

		void MegaEvolve(int index);
		#endregion

		#region Primal Revert battler.
		void PrimalReversion(int index);
		#endregion

		#region Call battler.
		void Call(int index);
		#endregion

		#region Gaining Experience.
		void GainEXP();

		void GainExpOne(int index, IBattler defeated, int partic, int expshare, bool haveexpall, bool showmessages = true);
		#endregion

		#region Learning a move.
		void LearnMove(int pkmnIndex, Moves move);
		#endregion

		#region Abilities.
		void OnActiveAll();

		bool OnActiveOne(IBattler pkmn, bool onlyabilities = false, bool moldbreaker = false);

		void PrimordialWeather();
		#endregion

		#region Judging.
		void JudgeCheckpoint(IBattler attacker, IBattleMove move = null);

		BattleResults DecisionOnTime();

		BattleResults DecisionOnTime2();

		BattleResults DecisionOnDraw();

		void Judge();
		#endregion

		#region Messages and animations.
		void Display(string msg);

		void DisplayPaused(string msg);

		void DisplayBrief(string msg);

		bool DisplayConfirm(string msg);

		void ShowCommands(string msg, string[] commands, bool cancancel = true);

		void Animation(Moves move, IBattler attacker, IBattler opponent, int hitnum = 0);

		void CommonAnimation(string name, IBattler attacker, IBattler opponent, int hitnum = 0);
		#endregion

		#region Battle core.
		BattleResults StartBattle(bool canlose= false);

		void StartBattleCore(bool canlose);
		#endregion

		#region Command phase.
		MenuCommands CommandMenu(int i);

		KeyValuePair<Items, int?> ItemMenu(int i);

		bool AutoFightMenu(int i);

		void CommandPhase();
		#endregion

		#region Attack phase.
		void AttackPhase();
		#endregion

		#region End of round.
		void EndOfRoundPhase();
		#endregion

		#region End of battle.
		BattleResults EndOfBattle(bool canlose = false);
		#endregion
	}
}