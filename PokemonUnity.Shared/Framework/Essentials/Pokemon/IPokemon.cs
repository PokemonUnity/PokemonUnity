using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Combat.Data;
using PokemonUnity.Character;
using PokemonUnity.Saving;
using PokemonUnity.Saving.SerializableClasses;
using PokemonUnity.Utility;
using PokemonUnity.Monster;
using PokemonEssentials.Interface.Item;

namespace PokemonEssentials.Interface.PokeBattle
{
	public interface IPokemon
	{
		/// <summary>
		/// Current Total HP
		/// </summary>
		int TotalHP { get; }
		/// <summary>
		/// Current Attack stat
		/// </summary>
		int attack { get; set; }
		/// <summary>
		/// Current Defense stat
		/// </summary>
		int defense { get; set; }
		/// <summary>
		/// Current Speed stat
		/// </summary>
		int speed { get; set; }
		/// <summary>
		/// Current Special Attack stat
		/// </summary>
		int spatk { get; set; }
		/// <summary>
		/// Current Special Defense stat
		/// </summary>
		int spdef { get; set; }
		/// <summary>
		/// Array of 6 Individual Values for HP, Atk, Def,
		/// Speed, Sp Atk, and Sp Def
		/// </summary>
		int[] IV { get; }
		/// <summary>
		/// Effort Values
		/// </summary>  
		byte[] EV { get; }
		/// <summary>
		/// Species (National Pokedex number)
		/// </summary>
		Pokemons Species { get; set; }
		/// <summary>
		/// Personal ID
		/// </summary>
		int personalID { get; set; }
		/// <summary>
		/// 32-bit Trainer ID (the secret ID is in the upper 16 bits)
		/// </summary>
		int trainerID { get; set; }
		/// <summary>
		/// Current HP
		/// </summary>
		int HP { get; set; }
		/// <summary>
		/// Pokérus strain and infection time
		/// </summary>
		int pokerus { get; set; }
		/// <summary>
		/// Held item
		/// </summary>
		Items Item { get; }
		/// <summary>
		/// Consumed held item (used in battle only)
		/// </summary>
		Items itemRecycle { get; set; }
		/// <summary>
		/// Resulting held item (used in battle only)
		/// </summary>
		Items itemInitial { get; set; }
		/// <summary>
		/// Whether Pokémon can use Belch (used in battle only)
		/// </summary>
		bool belch { get; set; }
		/// <summary>
		/// Mail
		/// </summary>
		IMail mail { get; set; }
		/// <summary>
		/// The Pokémon fused into this one
		/// </summary>
		IPokemon[] fused { get; set; }
		/// <summary>
		/// Nickname
		/// </summary>
		string Name { get; set; }
		/// <summary>
		/// Current experience points
		/// </summary>
		int exp { get; set; }
		/// <summary>
		/// Current happiness
		/// </summary>
		int Happiness { get; }
		/// <summary>
		/// Status problem (PBStatuses) 
		/// </summary>
		Status Status { get; set; }
		/// <summary>
		/// Sleep count/Toxic flag
		/// </summary>
		int StatusCount { get; set; }
		/// <summary>
		/// Steps to hatch egg, 0 if Pokémon is not an egg
		/// </summary>
		int eggsteps { get; set; }
		/// <summary>
		/// Moves (PBMove)
		/// </summary>
		IMove[] moves { get; }
		/// <summary>
		/// The moves known when this Pokémon was obtained
		/// </summary>
		IList<Moves> firstmoves { get; set; }
		/// <summary>
		/// Ball used
		/// </summary>
		Items ballUsed { get; set; }
		/// <summary>
		/// Markings
		/// </summary>
		//bool[] markings { get; }
		/// <summary>
		/// Manner obtained:
		/// 0 - met, 1 - as egg, 2 - traded,
		/// 4 - fateful encounter
		/// </summary>
		int obtainMode { get; set; }
		/// <summary>
		/// Map where obtained
		/// </summary>
		int obtainMap { get; set; }
		/// <summary>
		/// Replaces the obtain map's name if not null
		/// </summary>
		string obtainText { get; set; }
		/// <summary>
		/// Level obtained
		/// </summary>
		int obtainLevel { get; set; }
		/// <summary>
		/// Map where an egg was hatched
		/// </summary>
		int hatchedMap { get; set; }
		/// <summary>
		/// Language
		/// </summary>
		//int language { get; set; }
		/// <summary>
		/// Original Trainer's name 
		/// </summary>
		string ot { get; set; }
		/// <summary>
		/// Original Trainer's gender:
		/// 0 - male, 1 - female, 2 - mixed, 3 - unknown
		/// </summary>
		/// <remarks>
		/// For information only, not used to verify
		/// ownership of the Pokemon
		/// </remarks>
		int otgender { get; set; }
		/// <summary>
		/// Forces the first/second/hidden (0/1/2) ability
		/// </summary>
		int abilityflag { get; set; }
		/// <summary>
		/// Forces male (0) or female (1)
		/// </summary>
		bool genderflag { get; set; }
		/// <summary>
		/// Forces a particular nature
		/// </summary>
		int natureflag { get; set; }
		/// <summary>
		/// Forces the shininess (true/false)
		/// </summary>
		bool shinyflag { get; set; }
		/// <summary>
		/// Array of ribbons
		/// </summary>
		IList<Ribbons> ribbons { get; set; }
		/// <summary>
		/// Contest stats
		/// </summary>
		int cool { get; set; }
		/// <summary>
		/// Contest stats
		/// </summary>
		int beauty { get; set; }
		/// <summary>
		/// Contest stats
		/// </summary>
		int cute { get; set; }
		/// <summary>
		/// Contest stats
		/// </summary>
		int smart { get; set; }
		/// <summary>
		/// Contest stats
		/// </summary>
		int tough { get; set; }
		/// <summary>
		/// Contest stats
		/// </summary>
		int sheen { get; set; }

		// ###############################################################################
		// Ownership, obtained information
		// ###############################################################################
		// Returns the gender of this Pokémon's original trainer (2=unknown).
		//int otgender { get; }

		/// <summary>
		/// Returns whether the specified Trainer is NOT this Pokemon's original trainer.
		/// </summary>
		/// <param name="trainer"></param>
		/// <returns></returns>
		bool isForeign(ITrainer trainer);

		/// <summary>
		/// Returns the portion of the original trainer's ID.
		/// </summary>
		int publicID { get; }

		/// <summary>
		/// Returns this Pokémon's level when this Pokémon was obtained.
		/// </summary>
		//int obtainLevel { get; }

		/// <summary>
		/// Returns the time when this Pokémon was obtained.
		/// </summary>
		DateTime? timeReceived { get; set; }

		/// <summary>
		/// Returns the time when this Pokémon hatched.
		/// </summary>
		DateTime? timeEggHatched { get; set; }

		// ###############################################################################
		// Level
		// ###############################################################################
		/// <summary>
		/// Returns this Pokemon's level.
		/// </summary>
		int Level { get; }

		/// <summary>
		/// Returns whether this Pokemon is an egg.
		/// </summary>
		bool isEgg { get; }

		bool egg { get; }

		/// <summary>
		/// Returns this Pokemon's growth rate.
		/// </summary>
		LevelingRate GrowthRate { get; }

		/// <summary>
		/// Returns this Pokemon's base Experience value.
		/// </summary>
		int baseExp { get; }

		// ###############################################################################
		// Gender
		// ###############################################################################
		/// <summary>
		/// Returns this Pokemon's gender. 0=male, 1=female, 2=genderless
		/// </summary>
		int Gender { get; }

		/// <summary>
		/// Helper function that determines whether the input values would make a female.
		/// </summary>
		/// <param name="b"></param>
		/// <param name="genderRate"></param>
		/// <returns></returns>
		bool isFemale(int b, int genderRate);

		/// <summary>
		/// Returns whether this Pokémon species is restricted to only ever being one
		/// gender (or genderless).
		/// </summary>
		bool isSingleGendered { get; }

		/// <summary>
		/// Returns whether this Pokémon is male.
		/// </summary>
		bool IsMale { get; }

		/// <summary>
		/// Returns whether this Pokémon is female.
		/// </summary>
		bool IsFemale { get; }

		/// <summary>
		/// Returns whether this Pokémon is genderless.
		/// </summary>
		bool IsGenderless { get; }

		/// <summary>
		/// Sets this Pokémon's gender to a particular gender (if possible).
		/// </summary>
		/// <param name="value"></param>
		void setGender(int value);

		void makeMale();
		void makeFemale();

		// ###############################################################################
		// Ability
		// ###############################################################################
		/// <summary>
		/// Returns the index of this Pokémon's ability.
		/// </summary>
		int abilityIndex { get; }

		/// <summary>
		/// Returns the ID of this Pokemon's ability.
		/// </summary>
		Abilities Ability { get; }

		/// <summary>
		/// Returns whether this Pokémon has a particular ability.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		bool hasAbility(Abilities value = 0);

		/// <summary>
		/// Sets this Pokémon's ability to a particular ability (if possible).
		/// </summary>
		/// <param name="value"></param>
		void setAbility(Abilities value);

		bool hasHiddenAbility();

		/// <summary>
		/// Returns the list of abilities this Pokémon can have.
		/// </summary>
		/// <returns></returns>
		Abilities[] getAbilityList();

		// ###############################################################################
		// Nature
		// ###############################################################################
		/// <summary>
		/// Returns the ID of this Pokémon's nature.
		/// </summary>
		Natures Nature { get; }

		/// <summary>
		/// Returns whether this Pokémon has a particular nature.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		bool hasNature(Natures? value = null); //-1

		/// <summary>
		/// Sets this Pokémon's nature to a particular nature.
		/// </summary>
		/// <param name="value"></param>
		void setNature(Natures value);

		// ###############################################################################
		// Shininess
		// ###############################################################################
		/// <summary>
		/// Returns whether this Pokemon is shiny (differently colored).
		/// </summary>
		bool isShiny { get; }

		/// <summary>
		/// Makes this Pokemon shiny.
		/// </summary>
		void makeShiny();

		/// <summary>
		/// Makes this Pokemon not shiny.
		/// </summary>
		void makeNotShiny();

		// ###############################################################################
		// Pokérus
		// ###############################################################################
		/// <summary>
		/// Gives this Pokemon Pokérus (either the specified strain or a random one).
		/// </summary>
		/// <param name="strain"></param>
		void givePokerus(int strain = 0);

		/// <summary>
		/// Resets the infection time for this Pokemon's Pokérus (even if cured).
		/// </summary>
		void resetPokerusTime();

		/// <summary>
		/// Reduces the time remaining for this Pokemon's Pokérus (if infected).
		/// </summary>
		void lowerPokerusCount();

		/// <summary>
		/// Returns the Pokérus infection stage for this Pokemon.
		/// </summary>
		int pokerusStage { get; }

		// ###############################################################################
		// Types
		// ###############################################################################
		/// <summary>
		/// Returns whether this Pokémon has the specified type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		bool hasType(Types type);

		/// <summary>
		/// Returns this Pokémon's first type.
		/// </summary>
		Types Type1 { get; }

		/// <summary>
		/// Returns this Pokémon's second type.
		/// </summary>
		Types Type2 { get; }

		// ###############################################################################
		// Moves
		// ###############################################################################
		/// <summary>
		/// Returns the number of moves known by the Pokémon.
		/// </summary>
		int numMoves { get; }

		/// <summary>
		/// Returns true if the Pokémon knows the given move.
		/// </summary>
		/// <param name="move"></param>
		/// <returns></returns>
		bool hasMove(Moves move);

		bool knowsMove(Moves move);

		/// <summary>
		/// Returns the list of moves this Pokémon can learn by levelling up.
		/// </summary>
		/// <returns></returns>
		//KeyValuePair<Moves, int>[] getMoveList();
		Moves[] getMoveList(LearnMethod? method = null);

		/// <summary>
		/// Sets this Pokémon's movelist to the default movelist it originally had.
		/// </summary>
		void resetMoves();

		/// <summary>
		/// Silently learns the given move. Will erase the first known move if it has to.
		/// </summary>
		/// <param name="move"></param>
		void pbLearnMove(Moves move);

		/// <summary>
		/// Deletes the given move from the Pokémon.
		/// </summary>
		/// <param name="move"></param>
		void pbDeleteMove(Moves move);

		/// <summary>
		/// Deletes the move at the given index from the Pokémon.
		/// </summary>
		/// <param name="index"></param>
		void pbDeleteMoveAtIndex(int index);

		/// <summary>
		/// Deletes all moves from the Pokémon.
		/// </summary>
		void pbDeleteAllMoves();

		/// <summary>
		/// Copies currently known moves into a separate array, for Move Relearner.
		/// </summary>
		void pbRecordFirstMoves();

		bool isCompatibleWithMove(Moves move);

		// ###############################################################################
		// Contest attributes, ribbons
		// ###############################################################################
		//int cool   { get; }
		//int beauty { get; }
		//int cute   { get; }
		//int smart  { get; }
		//int tough  { get; }
		//int sheen  { get; }

		/// <summary>
		/// Returns the number of ribbons this Pokemon has.
		/// </summary>
		/// <returns></returns>
		int ribbonCount();

		/// <summary>
		/// Returns whether this Pokémon has the specified ribbon.
		/// </summary>
		/// <param name="ribbon"></param>
		/// <returns></returns>
		bool hasRibbon(Ribbons ribbon);

		/// <summary>
		/// Gives this Pokémon the specified ribbon.
		/// </summary>
		/// <param name="ribbon"></param>
		void giveRibbon(Ribbons ribbon);

		/// <summary>
		/// Replaces one ribbon with the next one along, if possible.
		/// </summary>
		/// <param name="arg"></param>
		/// <returns></returns>
		int upgradeRibbon(params Ribbons[] arg);

		/// <summary>
		/// Removes the specified ribbon from this Pokémon.
		/// </summary>
		void takeRibbon(Ribbons ribbon);

		/// <summary>
		/// Removes all ribbons from this Pokémon.
		/// </summary>
		void clearAllRibbons();

		// ###############################################################################
		// Other
		// ###############################################################################
		/// <summary>
		/// Returns whether this Pokémon has a hold item.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		bool hasItem(Items value = Items.NONE);

		/// <summary>
		/// Sets this Pokémon's item. Accepts symbols.
		/// </summary>
		/// <param name="value"></param>
		void setItem(Items value);

		/// <summary>
		/// Returns the items this species can be found holding in the wild.
		/// </summary>
		/// <returns>[itemcommon,itemuncommon,itemrare]</returns>
		Items[] wildHoldItems();

		/// <summary>
		/// Returns this Pokémon's mail.
		/// </summary>
		//IMail mail { get; }

		/// <summary>
		/// Returns this Pokémon's language.
		/// </summary>
		int language { get; }

		/// <summary>
		/// Returns the markings this Pokémon has.
		/// </summary>
		bool[] markings { get; }

		/// <summary>
		/// Returns a string stating the Unown form of this Pokémon.
		/// </summary>
		char unownShape { get; }

		/// <summary>
		/// Returns the height of this Pokémon.
		/// </summary>
		float height { get; }

		/// <summary>
		/// Returns the weight of this Pokémon.
		/// </summary>
		float weight { get; }

		/// <summary>
		/// Returns the EV yield of this Pokémon.
		/// </summary>
		int[] evYield { get; }

		string kind { get; }

		string dexEntry { get; }

		/// <summary>
		/// Sets this Pokémon's HP.
		/// </summary>
		//int hp { set; }

		/// <summary>
		/// Heals all HP of this Pokémon.
		/// </summary>
		void healHP();

		/// <summary>
		/// Heals the status problem of this Pokémon.
		/// </summary>
		void healStatus();

		/// <summary>
		/// Heals all PP of this Pokémon.
		/// </summary>
		/// <param name="index"></param>
		void healPP(int index = -1);

		/// <summary>
		/// Heals all HP, PP, and status problems of this Pokémon.
		/// </summary>
		void heal();

		// Changes the happiness of this Pokémon depending on what happened to change it.
		void changeHappiness(HappinessMethods method);

		// ###############################################################################
		// Stat calculations, Pokémon creation
		// ###############################################################################
		/// <summary>
		/// Returns this Pokémon's base stats.  An array of six values.
		/// </summary>
		/// <returns></returns>
		int[] baseStats { get; }

		/// <summary>
		/// Returns the maximum HP of this Pokémon.
		/// </summary>
		/// <param name="_base"></param>
		/// <param name="level"></param>
		/// <param name="iv"></param>
		/// <param name="ev"></param>
		/// <returns></returns>
		int calcHP(int _base, int level, int iv, int ev);

		/// <summary>
		/// Returns the specified stat of this Pokémon (not used for total HP).
		/// </summary>
		/// <param name="_base"></param>
		/// <param name="level"></param>
		/// <param name="iv"></param>
		/// <param name="ev"></param>
		/// <param name="pv"></param>
		void calcStat(int _base, int level, int iv, int ev, int pv);

		/// <summary>
		/// Recalculates this Pokémon's stats.
		/// </summary>
		void calcStats();

		/// <summary>
		/// Creates a new Pokémon object.
		/// </summary>
		/// <param name="species">Pokémon species.</param>
		/// <param name="level">Pokémon level.</param>
		/// <param name="player">Trainer object for the original trainer.</param>
		/// <param name="withMoves">If false, this Pokémon has no moves.</param>
		/// <returns></returns>
		IPokemon initialize(Pokemons species, int level, ITrainer player= null, bool withMoves = true);
	}
}