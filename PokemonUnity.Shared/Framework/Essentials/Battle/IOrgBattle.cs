using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;
using PokemonUnity.Combat.Data;
using PokemonUnity.Character;
using PokemonUnity.Saving;
using PokemonUnity.Saving.SerializableClasses;
using PokemonUnity.Utility;
using PokemonEssentials;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonEssentials.Interface.Battle
{
	// ===============================================================================
	// Pokémon Organized Battle
	// ===============================================================================
	/// <summary>
	/// Extension of <seealso cref="IGame"/>
	/// </summary>
	public interface IGameOrgBattle
	{
		bool pbHasEligible(params object[] args);

		PokemonUnity.Character.TrainerMetaData[] pbGetBTTrainers(int challengeID);
		IPokemon pbGetBTPokemon(int challengeID);


		void pbRecordLastBattle();

		void pbPlayBattle(IBattleRecordData battledata);

		void pbDebugPlayBattle();

		void pbPlayLastBattle();

		void pbPlayBattleFromFile(string filename);



		IBattleChallenge pbBattleChallenge { get; }

		//ITrainer pbBattleChallengeTrainer(int numwins, ITrainer[] bttrainers);
		int pbBattleChallengeTrainer(int numwins, PokemonUnity.Character.TrainerMetaData[] bttrainers);

		void pbBattleChallengeGraphic(IEntity @event);

		string pbBattleChallengeBeginSpeech();

		bool pbEntryScreen(params object[] arg);

		IBattle pbBattleChallengeBattle();





		IPokemon[] pbBattleFactoryPokemon(IPokemonChallengeRules rule, int numwins, int numswaps, IPokemon[] rentals);

		ITrainer pbGenerateBattleTrainer(int trainerid, IPokemonChallengeRules rule);

		//ToDo: return bool?
		BattleResults pbOrganizedBattleEx(ITrainer opponent, IPokemonChallengeRules challengedata, string endspeech, string endspeechwin);

		bool pbIsBanned(IPokemon pokemon);
	}

	public interface IPokemonSerialized
	{
		Pokemons species { get; set; }
		Items item { get; set; }
		Natures nature { get; set; }
		Moves move1 { get; set; }
		Moves move2 { get; set; }
		Moves move3 { get; set; }
		Moves move4 { get; set; }
		int[] ev { get; set; }

		//IPokemonSerialized(species, item, nature, move1, move2, move3, move4, ev);

		/*=begin;
		  void _dump(depth) {
			return [@species,@item,@nature,@move1,@move2,
			   @move3,@move4,@ev].pack("vvCvvvvC");
		  }

		  void _load(str) {
			data=str.unpack("vvCvvvvC");
			return new this(
			   data[0],data[1],data[2],data[3],
			   data[4],data[5],data[6],data[7];
			);
		  }
		=end;*/

		IPokemonSerialized fromInspected(string str);

		IPokemonSerialized fromPokemon(IPokemon pokemon);

		string inspect();

		string tocompact();

		int constFromStr(int[] mod, string str);

		IPokemonSerialized fromString(string str);

		IPokemonSerialized fromstring(string str);

		Moves convertMove(Moves move);

		IPokemon createPokemon(int level, int[] iv, ITrainer trainer);
	}

	public interface IGameMapOrgBattle //: PokemonEssentials.Interface.IGameMap
	{
		int map_id { get; set; }
	}

	public interface IGamePlayerOrgBattle //: IGamePlayer
	{
	    int direction { get; }

	    void moveto2(float x, float y);
	}

	public interface IBattleChallengeType : ICloneable
	{
		int currentWins		{ get; }
		int previousWins	{ get; }
		int maxWins			{ get; }
		int currentSwaps	{ get; }
		int previousSwaps	{ get; }
		int maxSwaps		{ get; }
		bool doublebattle	{ get; }
		int numPokemon		{ get; }
		int battletype		{ get; }
		int mode			{ get; }
		int numRounds			{ get; set; }

		IBattleChallengeType initialize();

		void saveWins(IBattleChallengeData challenge);
	}

	public interface IBattleChallengeData
	{
		bool resting					{ get; }
		int wins						{ get; }
		int swaps						{ get; }
		bool inProgress					{ get; }
		int battleNumber				{ get; }
		int numRounds					{ get; }
		BattleResults decision			{ get; set; }
		IPokemon[] party				{ get; }
		IBattleFactoryData extraData	{ get; }

		IBattleChallengeData initialize();

		void setExtraData(IBattleFactoryData value);

		void pbAddWin();

		void pbAddSwap();

		bool pbMatchOver();

		//ITrainer nextTrainer();
		int nextTrainer { get; }

		void pbGoToStart();

		void setParty(IPokemon[] value);

		void pbStart(IBattleChallengeType t, int numRounds);

		void pbCancel();

		void pbEnd();

		void pbGoOn();

		void pbRest();
	}

	public interface IBattleChallenge
	{
		int currentChallenge { get; }
		//int BattleTower   { get; } //= 0;
		//int BattlePalace  { get; } //= 1;
		//int BattleArena   { get; } //= 2;
		//int BattleFactory { get; } //= 3;

		IBattleChallenge initialize();

		IPokemonChallengeRules rules { get; }

		/// <summary>
		/// </summary>
		/// <param name="doublebattle"></param>
		/// <param name="numPokemon"></param>
		/// <param name="battletype">0=>BattleTower; 1=>BattlePalace; 2=>BattleArena; 3=>BattleFactory</param>
		/// <param name="mode">1=>Open Level; 2=>Battle Tent</param>
		/// <returns></returns>
		IPokemonChallengeRules modeToRules(bool doublebattle, int numPokemon, int battletype, int mode);

		void set(int id, int numrounds, IPokemonChallengeRules rules);

		void start(params object[] args);

		void register(int id, bool doublebattle, int numrounds, int numPokemon, int battletype, int mode= 1);

		bool pbInChallenge { get; }

		//IBattleChallengeData data { get; }
		IBattleChallengeType data { get; }

		int getCurrentWins(int challenge);

		int getPreviousWins(int challenge);

		int getMaxWins(int challenge);

		int getCurrentSwaps(int challenge);

		int getPreviousSwaps(int challenge);

		int getMaxSwaps(int challenge);

		void pbStart(int challenge);

		void pbEnd();

		//ToDo: returns pbOrganizedBattleEx
		BattleResults pbBattle();

		bool pbInProgress { get; }

		bool pbResting();

		void setDecision(BattleResults value);

		void setParty(IPokemon[] value);

		IBattleFactoryData extra { get; }
		BattleResults decision  { get; }
		int wins                { get; }
		int swaps               { get; }
		int battleNumber        { get; }
		int nextTrainer    { get; }
		void pbGoOn         ();
		void pbAddWin       ();
		void pbCancel       ();
		void pbRest         ();
		bool pbMatchOver    ();
		void pbGoToStart    ();
	}

	/// <summary>
	/// <seealso cref="IGameEvent"/>
	/// </summary>
	public interface IGameEventOrgBattle
	{
		bool pbInChallenge { get; }
	}

	public interface IBattleFactoryData
	{
		IBattleFactoryData initialize(IBattleChallengeData bcdata);

		void pbPrepareRentals();

		void pbChooseRentals();

		//ToDo: returns pbOrganizedBattleEx
		BattleResults pbBattle(IBattleChallenge challenge);

		void pbPrepareSwaps();

		bool pbChooseSwaps();
	}
}