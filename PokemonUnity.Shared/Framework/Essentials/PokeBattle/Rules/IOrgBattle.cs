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
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonEssentials.Interface.PokeBattle.Rules
{
// ===============================================================================
// Pokémon Organized Battle
// ===============================================================================
	public interface IGameOrgBattle
	{
        bool pbHasEligible(params object[] args);

        ITrainer[] pbGetBTTrainers(int challengeID);
        IPokemon pbGetBTPokemon(int challengeID);


        void pbRecordLastBattle();

        void pbPlayBattle(IBattleRecordData battledata);

        void pbDebugPlayBattle();

        void pbPlayLastBattle();

        void pbPlayBattleFromFile(string filename);



        IBattleChallenge pbBattleChallenge();

        int pbBattleChallengeTrainer(int numwins, ITrainer[] bttrainers);

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

public interface IGameMap : PokemonEssentials.Interface.IGameMap
{
    int map_id { get; set; }
}

//public interface IGamePlayer
//{
//    int direction { get; set; }
//
//    void moveto2(float x, float y);
//}

public interface IBattleChallengeType
{
    int currentWins { get; set; }
    int previousWins { get; set; }
    int maxWins { get; set; }
    int currentSwaps { get; set; }
    int previousSwaps { get; set; }
    int maxSwaps { get; set; }
    int doublebattle { get; set; }
    int numPokemon { get; set; }
    int battletype { get; set; }
    int mode { get; set; }

    IBattleChallengeType initialize();

    void saveWins(IBattleChallenge challenge);
}

public interface IBattleChallengeData
{
    int resting { get; set; }
    int wins { get; set; }
    int swaps { get; set; }
    int inProgress { get; set; }
    int battleNumber { get; set; }
    int numRounds { get; set; }
    int decision { get; set; }
    IPokemon[] party { get; set; }
    IBattleFactoryData extraData { get; set; }

    IBattleChallengeData initialize();

    void setExtraData(IBattleFactoryData value);

    void pbAddWin();

    void pbAddSwap();

    bool pbMatchOver();

    ITrainer nextTrainer();

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
    int BattleTower   { get; } //= 0;
    int BattlePalace  { get; } //= 1;
    int BattleArena   { get; } //= 2;
    int BattleFactory { get; } //= 3;

    IBattleChallenge initialize();

    IPokemonChallengeRules rules { get; }

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
    ITrainer nextTrainer    { get; }
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