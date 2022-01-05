using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Globalization;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Inventory;
using PokemonUnity.Saving;
using System.IO;
using PokemonUnity.Overworld;

namespace PokemonUnity.Application
{
	public struct Feature : IEquatable<Feature>, IEqualityComparer<Feature>
	{
		/// <summary>
		/// Shift: after you kill the enemy's pokemon, 
		/// the game will tell you what pokemon will be switched in and then allow you to freely switch your pokemon with another. <para></para>
		/// Set: you can't do the above (link battles can only be set btw)
		/// </summary>
		public bool BattleShiftStyle { get; private set; }
		public bool ForceBattleStyle { get; private set; }
		#region Core Game Design and Settings
		//ToDo: Remove Const/Static and Store data in Constructor, Explicit Operator, GenerateHashId
		#region Pokemon Settings
		/// <summary>
		/// The maximum level Pokémon can reach.
		/// </summary>
		public const byte MAXIMUMLEVEL = 100;
		/// <summary>
		/// The level of newly hatched Pokémon.
		/// </summary>
		public const byte EGGINITIALLEVEL = 1;
		/// <summary>
		/// The odds of a newly generated Pokémon being shiny (out of 65536).
		/// </summary>
		public const int SHINYPOKEMONCHANCE = 8;
		/// <summary>
		/// The odds of a wild Pokémon/bred egg having Pokérus (out of 65536).
		/// </summary>
		public const int POKERUSCHANCE = 3;
		#endregion

		#region OverWorld Rules
		/// <summary>
		/// Whether poisoned Pokémon will lose HP while walking around in the field.
		/// </summary>
		public const bool POISONINFIELD = true;
		/// <summary>
		/// Whether poisoned Pokémon will faint while walking around in the field
		///    (true), or survive the poisoning with 1HP (false).
		/// </summary>
		public const bool POISONFAINTINFIELD = false;
		/// <summary>
		/// Whether fishing automatically hooks the Pokémon (if false, there is a
		/// reaction test first).
		/// </summary>
		public const bool FISHINGAUTOHOOK = false;
		/// <summary>
		/// Whether the player can surface from anywhere while diving (true), or only in
		/// spots where they could dive down from above (false).
		/// </summary>
		public const bool DIVINGSURFACEANYWHERE = false;
		/// <summary>
		/// Whether planted berries grow according to Gen 4 mechanics (true) or Gen 3
		/// mechanics (false).
		/// </summary>
		public const bool NEWBERRYPLANTS = true;
		/// <summary>
		/// Whether TMs can be used infinitely as in Gen 5 (true), or are one-use-only
		/// as in older Gens (false).
		/// </summary>
		public const bool INFINITETMS = true;
		#endregion

		#region Battle Settings
		/// <summary>
		/// Whether a move's physical/special category depends on the move itself as in
		///    newer Gens (true), or on its type as in older Gens (false).
		/// </summary>
		public const bool USEMOVECATEGORY = true;
		/// <summary>
		/// Whether the battle mechanics mimic Gen 6 (true) or Gen 5 (false).
		/// </summary>
		public const bool USENEWBATTLEMECHANICS = false;
		/// <summary>
		/// Whether the Exp gained from beating a Pokémon should be scaled depending on
		///    the gainer's level as in Gen 5 (true), or not as in other Gens (false).
		/// </summary>
		public const bool USESCALEDEXPFORMULA = true;
		/// <summary>
		/// Whether the Exp gained from beating a Pokémon should be divided equally
		///    between each participant (false), or whether each participant should gain
		///    that much Exp. This also applies to Exp gained via the Exp Share (held
		///    item version) being distributed to all Exp Share holders. This is true in
		///    Gen 6 and false otherwise.
		/// </summary>
		public const bool NOSPLITEXP = false;
		/// <summary>
		/// Whether the critical capture mechanic applies (true) or not (false). Note
		///    that it is based on a total of 600+ species (i.e. that many species need
		///    to be caught to provide the greatest critical capture chance of 2.5x),
		///    and there may be fewer species in your game.
		/// </summary>
		public const bool USECRITICALCAPTURE = false;
		/// <summary>
		/// Whether Pokémon gain Exp for capturing a Pokémon (true) or not (false).
		/// </summary>
		public const bool GAINEXPFORCAPTURE = false;
		/// <summary>
		/// An array of items which act as Mega Rings for the player (NPCs don't need a
		///    Mega Ring item, just a Mega Stone).
		/// </summary>
		public static readonly Items[] MEGARINGS = new Items[] { Items.MEGA_RING, Items.MEGA_BRACELET, Items.MEGA_CUFF, Items.MEGA_CHARM };
		#endregion

		#region Badges
		/// <summary>
		/// The minimum number of badges required to boost each stat of a player's
		/// Pokémon by 1.1x, while using moves in battle only.
		/// </summary>
		public const int BADGESBOOSTATTACK = 1;
		/// <summary>
		/// The minimum number of badges required to boost each stat of a player's
		/// Pokémon by 1.1x, while using moves in battle only.
		/// </summary>
		public const int BADGESBOOSTDEFENSE = 5;
		/// <summary>
		/// The minimum number of badges required to boost each stat of a player's
		/// Pokémon by 1.1x, while using moves in battle only.
		/// </summary>
		public const int BADGESBOOSTSPEED = 3;
		/// <summary>
		/// The minimum number of badges required to boost each stat of a player's
		/// Pokémon by 1.1x, while using moves in battle only.
		/// </summary>
		public const int BADGESBOOSTSPATK = 7;
		/// <summary>
		/// The minimum number of badges required to boost each stat of a player's
		/// Pokémon by 1.1x, while using moves in battle only.
		/// </summary>
		public const int BADGESBOOSTSPDEF = 7;
		/// <summary>
		/// Whether the badge restriction on using certain hidden moves is either owning
		/// at least a certain number of badges (true), or owning a particular badge (false).
		/// </summary>
		public const bool HIDDENMOVESCOUNTBADGES = true;
		/// <summary>
		/// Depending on HIDDENMOVESCOUNTBADGES, either the number of badges required to
		/// use each hidden move, or the specific badge number required to use each
		/// move. Remember that badge 0 is the first badge, badge 1 is the second
		/// badge, etc.<para></para>
		/// e.g. To require the second badge, put false and 1.
		/// To require at least 2 badges, put true and 2.
		/// </summary>
		//ToDo: Consider a mechanic that allows each region to have their own TM/HM #s Region[Generation,Badges] <see cref="PokemonUnity.Item.Items"/>
		public const int BADGEFORCUT = 1;
		/// <summary>
		/// </summary>
		public const int BADGEFORFLASH = 2;
		/// <summary>
		/// </summary>
		public const int BADGEFORROCKSMASH = 3;
		/// <summary>
		/// </summary>
		public const int BADGEFORSURF = 4;
		/// <summary>
		/// </summary>
		public const int BADGEFORFLY = 5;
		/// <summary>
		/// </summary>
		public const int BADGEFORSTRENGTH = 6;
		/// <summary>
		/// </summary>
		public const int BADGEFORDIVE = 7;
		/// <summary>
		/// </summary>
		public const int BADGEFORWATERFALL = 8;
		#endregion

		#region Player Storage
		/// <summary>
		/// The maximum number of slots per pocket (-1 means infinite number). Ignore
		///    the first number (0).
		/// </summary>
		/// The names of each pocket of the Bag. Leave the first entry blank.
		/// "Items"
		/// "Medicine"
		/// "Poké Balls"
		/// "TMs & HMs"
		/// "Berries"
		/// "Mail"
		/// "Battle Items"
		/// "Key Items"
		public static readonly int[] MAXPOCKETSIZE = new int[] { 0, -1, -1, -1, -1, -1, -1, -1, -1 };
		/// <summary>
		/// The maximum number of items each slot in the Bag can hold.
		/// </summary>
		public const int BAGMAXPERSLOT = 999;
		/// <summary>
		/// Whether each pocket in turn auto-sorts itself by item ID number. Ignore the
		///    first entry (the 0).
		/// </summary>
		public static readonly bool[] POCKETAUTOSORT = new bool[] { false/*null*/, false, false, false, true, true, false, false, false };
		/// <summary>
		/// The number of boxes in Pokémon storage.
		/// </summary>
		public const int STORAGEBOXES = 24;
		#endregion

		#region Pokedex
		/// <summary>
		/// Whether the Pokédex list shown is the one for the player's current region
		///    (true), or whether a menu pops up for the player to manually choose which
		///    Dex list to view when appropriate (false).
		/// </summary>
		public const bool DEXDEPENDSONLOCATION = false;
		/// <summary>
		/// Whether all forms of a given species will be immediately available to view
		///    in the Pokédex so long as that species has been seen at all (true), or
		///    whether each form needs to be seen specifically before that form appears
		///    in the Pokédex (false).
		/// </summary>
		public const bool ALWAYSSHOWALLFORMS = false;
		#endregion

		#region Currency Limit
		/// <summary>
		/// The amount of money the player starts the game with.
		/// </summary>
		public const int INITIALMONEY = 3000;
		/// <summary>
		/// The maximum amount of money the player can have.
		/// </summary>
		/// Do you really NEED 6 digits for money?
		public const int MAXMONEY = 999999;
		/// <summary>
		/// The maximum number of Game Corner coins the player can have.
		/// </summary>
		public const int MAXCOINS = 99999;
		/// <summary>
		/// The maximum length, in characters, that the player's name can be.
		/// </summary>
		public const int PLAYERNAMELIMIT = 10;
		#endregion

		#region Safari
		/// <summary>
		/// The number of steps allowed before a Safari Zone game is over (0=infinite).
		/// </summary>
		public const int SAFARISTEPS = 600;
		/// <summary>
		/// The number of seconds a Bug Catching Contest lasts for (0=infinite).
		/// </summary>
		public const int BUGCONTESTTIME = 1200;
		#endregion

		#region Battling and Encounter
		public const int MAXPARTYSIZE = 6;
		/// <summary>
		/// The Global Switch that is set to ON when the player whites out.
		/// </summary>
		// ToDo: Move to Game class, as this is not a core const...
		public static bool STARTING_OVER_SWITCH = true;
		/// <summary>
		/// The Global Switch that is set to ON when the player has seen Pokérus in the
		///    Poké Center, and doesn't need to be told about it again.
		/// </summary>
		// ToDo: Move to Game class, as this is not a core const...
		public static bool SEEN_POKERUS_SWITCH = false;
		/// <summary>
		/// The Global Switch which, while ON, makes all wild Pokémon created be
		///    shiny.
		/// </summary>
		public const bool SHINY_WILD_POKEMON_SWITCH = false;
		/// <summary>
		/// The Global Switch which, while ON, makes all Pokémon created considered to
		///    be met via a fateful encounter.
		/// </summary>
		public const bool FATEFUL_ENCOUNTER_SWITCH = false;
		/// <summary>
		/// The Global Switch which determines whether the player will lose money if
		///    they lose a battle (they can still gain money from trainers for winning).
		/// </summary>
		public const bool NO_MONEY_LOSS = true;
		/// <summary>
		/// The Global Switch which, while ON, prevents all Pokémon in battle from Mega
		///    Evolving even if they otherwise could.
		/// </summary>
		public const bool NO_MEGA_EVOLUTION = true;
		#endregion
		#endregion
		#region Custom Game Features
		public bool SandBoxMode { get; private set; }
		//Nuzlocke Challenge => Pokemon Centers cost money, every pokemon must be named, when defeated/fainted pokemon is gone, only allowed to capture first pokemon encountered when entering new map
		/// <summary>
		/// Basically, you use the Dexnav to find pokemon in the area, they appear as shadows in the grass, and you need to sneak up on them
		/// these pokemon can have egg moves, or even their HiddenAbility
		/// </summary>
		/// Apparently you can use the Sneaking feature to helps with this. 
		/// ToDo: OnlyAllowEggMovesWhenUsingDexNav or DexNavAllowsEggMoves
		public bool CatchPokemonsWithEggMoves { get; private set; }
		public bool ForcePokemonNaming { get; private set; }
		public bool PokemonCentersCostMoney { get; private set; }
		public bool PokemonDeathOnFaint { get; private set; }
		/// <summary>
		/// Else it's data is released from code memory
		/// </summary>
		/// <remarks>
		/// Use with <see cref="PokemonDeathOnFaint"/>
		/// </remarks>
		public bool PokemonSentToPcOnDeath { get; private set; }
		/// <summary>
		/// If active PC box is full, it will automatically store pokemon in next available spot
		/// </summary>
		public bool OverflowPokemonsIntoNextBox { get; private set; }
		/// <summary>
		/// Only allowed to capture the first encounter of any pokemon per map id/zone/route 
		/// (whether it's successful or not)
		/// </summary>
		public bool CanOnlyCaptureFirstWildEncounter { get; private set; }
		public bool CanOnlySaveAtPokemonCenter { get; private set; }
		/// <summary>
		/// Dark Souls-esque; when you heal at a center, 
		/// overworld NPCs you've already defeated will want to battle,
		/// if you encounter them (they'll stop you to battle if you cross line of sights)
		/// </summary>
		public bool PokemonCenterHealsResetTrainers { get; private set; }
		public bool RandomStarters { get; private set; }
		public bool RandomWildEncounters { get; private set; }
		/// <summary>
		/// like <see cref="RandomWildEncounters"/>, 
		/// except normal map encounter sprinkled in with high difficulty spawns
		/// </summary>
		/// <remarks>
		/// Maybe higher IV count, increase rarity or spawn level, 
		/// or even S.O.S. battles and swarms
		/// </remarks>
		public bool RandomBossEncounters { get; private set; } 
		public bool NoCapturesAllowed { get; private set; }
		public bool NuzlockeSkipDuplicates { get; private set; }
		public bool ReleasedPokemonsRoamNearbyAreas { get; private set; }
		public bool ReleasedPokemonsReturnToEncounterArea { get; private set; }
		public bool NoPokemonCenterHeals { get; private set; }
		/// <summary>
		/// </summary>
		/// (from 6 to 3?...)
		public byte LimitPokemonPartySize { get; private set; } 
		public bool NoStoreBoughtMedicine { get; private set; }
		public bool PcDoesNotHealPokemons { get; private set; }
		public bool NoHealingNPCs { get; private set; }
		public bool NoHealingItemsOutsideBattle { get; private set; }
		public bool NoHealingItemsInsideBattle { get; private set; }
		public bool LimitOneCapturePerEncounterArea { get; private set; }
		public bool OnlinePlayEnabled { get; private set; }
		/// <summary>
		/// load and save profile over internet?... 
		/// or maybe sql vs xml?
		/// </summary>
		public bool EnableCloudStorage { get; private set; }
		/// <summary>
		/// If playing a challenge where it's possible to get a game over, 
		/// then your save profile is deleted and there is no do-over.
		/// </summary>
		public bool FailChallengeWipesData { get; private set; }
		/// <summary>
		/// Immediately after reaching win goal for challenge criteria,
		/// disables all features related to challenge 
		/// (returns game to "normmal"/classic at the end of challenge)
		/// </summary>
		//ToDo: Left Open-ended for different win-criterias
		public bool ChallengeEndsAfter { get; private set; }
		/// <summary>
		/// Cannot use or access PC, of any sort (cant access stored pokemons or items)
		/// </summary>
		public bool NoPC { get; private set; }
		/// <summary>
		/// If entire party is defeated, player gets a white screen effect, and the game ends
		/// </summary>
		public bool GameOverOnWhiteOut { get; private set; }
		public bool RandomizePartyAfterBattle { get; private set; }
		public bool CanUseItemsDuringBattle { get; private set; }
		/// <summary>
		/// Gets average level of gym leader party and caps your party
		/// max level at that number temporarily (will return back after battle)
		/// </summary>
		/// Your stats scale with your level automatically
		public bool CanOverLevelGymLeader { get; private set; }
		/// <summary>
		/// When you heal your pokemons at a pokemon center, or access pc,
		/// all trainers that challenge you to trainer battle are reset.
		/// </summary>
		public bool PokemonCenterAndPcResetNpcTrainers { get; private set; }
		/// <summary>
		/// Repeat battles with the same trainer, will increase their party level
		/// </summary>
		public bool NpcTrainersGetStrongerEachBattle { get; private set; }
		/// <summary>
		/// Repeat battles with the same trainer, will increase their AI level
		/// </summary>
		public bool NpcTrainersGetSmarterEachBattle { get; private set; }
		public bool LoseAllMoneyOnLoss { get; private set; }
		/// <summary>
		/// Need to defeat last trainer you lost to, to reclaim money.
		/// Only recover money from trainers, not wilds.
		/// (Works best with LoseAllMoneyOnLoss)
		/// </summary>
		public bool LosingTwiceInRowResetsMoney { get; private set; }
		/// <summary>
		/// Will TMs stay in bag inventory after consumed by pokmeon
		/// </summary>
		public bool MachineIsInfiniteUse { get; private set; }
		#endregion
		#region Game Event Switches
		/// <summary>
		/// Used to track whether or not player has whited out,
		/// and the amount of times it has occurred.
		/// </summary>
		/// Can use this to check if player had a perfect clear.
		public int PlayerHasStartedOverCounter { get; set; }
		/// <summary>
		/// The Global Switch that is set to ON when the player whites out.
		/// </summary>
		public bool StartingOverSwitch { get; set; }
		/// <summary>
		/// The Global Switch that is set to ON when the player has seen Pokérus in the
		/// Poké Center, and doesn't need to be told about it again.
		/// </summary>
		public bool SeenPokerusSwitch { get; private set; }
		/// <summary>
		/// While this Switch is ON, all wild Pokémon encountered will be shiny.
		/// </summary>
		/// Doesnt include hatching from egg?...
		public bool ShinyWildPokémon { get; private set; }
		/// <summary>
		/// While this Switch is ON, all Pokémon created will be fateful encounters.
		/// </summary>
		public bool FatefulEncounters { get; private set; }
		/// <summary>
		/// While this Switch is ON, the player will not lose any money if they lose a wild battle or a trainer battle. 
		/// They can still gain money from battles, though.
		/// </summary>
		public bool NoMoneyLostInBattle { get; private set; }
		/// <summary>
		/// While this Switch is ON, no Pokémon can Mega Evolve in battle, not even if they normally could.
		/// </summary>
		public bool NoMegaEvolution { get; private set; }
		#endregion

		#region Explicit Operators
		public static bool operator ==(Feature x, Feature y)
		{
			return
				x.BattleShiftStyle == y.BattleShiftStyle &&
				x.CanOnlyCaptureFirstWildEncounter == y.CanOnlyCaptureFirstWildEncounter &&
				x.CanOnlySaveAtPokemonCenter == y.CanOnlySaveAtPokemonCenter &&
				x.CatchPokemonsWithEggMoves == y.CatchPokemonsWithEggMoves &&
				x.ChallengeEndsAfter == y.ChallengeEndsAfter &&
				x.EnableCloudStorage == y.EnableCloudStorage &&
				x.FailChallengeWipesData == y.FailChallengeWipesData &&
				x.ForcePokemonNaming == y.ForcePokemonNaming &&
				x.GameOverOnWhiteOut == y.GameOverOnWhiteOut &&
				x.LimitPokemonPartySize == y.LimitPokemonPartySize &&
				x.NoCapturesAllowed == y.NoCapturesAllowed &&
				x.NoHealingItemsInsideBattle == y.NoHealingItemsInsideBattle &&
				x.NoHealingItemsOutsideBattle == y.NoHealingItemsOutsideBattle &&
				x.NoHealingNPCs == y.NoHealingNPCs &&
				x.NoPC == y.NoPokemonCenterHeals &&
				x.NoPokemonCenterHeals == y.NoPokemonCenterHeals &&
				x.NoStoreBoughtMedicine == y.NoStoreBoughtMedicine &&
				x.NuzlockeSkipDuplicates == y.NuzlockeSkipDuplicates &&
				x.OnlinePlayEnabled == y.OnlinePlayEnabled &&
				x.OverflowPokemonsIntoNextBox == y.OverflowPokemonsIntoNextBox &&
				x.PcDoesNotHealPokemons == y.PcDoesNotHealPokemons &&
				x.PokemonCenterHealsResetTrainers == y.PokemonCenterHealsResetTrainers &&
				x.PokemonCentersCostMoney == y.PokemonCentersCostMoney &&
				x.PokemonDeathOnFaint == y.PokemonDeathOnFaint &&
				x.PokemonSentToPcOnDeath == y.PokemonSentToPcOnDeath &&
				x.RandomBossEncounters == y.RandomBossEncounters &&
				x.RandomizePartyAfterBattle == y.RandomizePartyAfterBattle &&
				x.RandomStarters == y.RandomStarters &&
				x.RandomWildEncounters == y.RandomWildEncounters &&
				x.ReleasedPokemonsReturnToEncounterArea == y.ReleasedPokemonsReturnToEncounterArea &&
				x.ReleasedPokemonsRoamNearbyAreas == y.ReleasedPokemonsRoamNearbyAreas &&
				x.SandBoxMode == y.SandBoxMode;
		}
		public static bool operator !=(Feature x, Feature y)
		{
			return
				x.BattleShiftStyle != y.BattleShiftStyle ||
				x.CanOnlyCaptureFirstWildEncounter != y.CanOnlyCaptureFirstWildEncounter ||
				x.CanOnlySaveAtPokemonCenter != y.CanOnlySaveAtPokemonCenter ||
				x.CatchPokemonsWithEggMoves != y.CatchPokemonsWithEggMoves ||
				x.ChallengeEndsAfter != y.ChallengeEndsAfter ||
				x.EnableCloudStorage != y.EnableCloudStorage ||
				x.FailChallengeWipesData != y.FailChallengeWipesData ||
				x.ForcePokemonNaming != y.ForcePokemonNaming ||
				x.GameOverOnWhiteOut != y.GameOverOnWhiteOut ||
				x.LimitPokemonPartySize != y.LimitPokemonPartySize ||
				x.NoCapturesAllowed != y.NoCapturesAllowed ||
				x.NoHealingItemsInsideBattle != y.NoHealingItemsInsideBattle ||
				x.NoHealingItemsOutsideBattle != y.NoHealingItemsOutsideBattle ||
				x.NoHealingNPCs != y.NoHealingNPCs ||
				x.NoPC != y.NoPokemonCenterHeals ||
				x.NoPokemonCenterHeals != y.NoPokemonCenterHeals ||
				x.NoStoreBoughtMedicine != y.NoStoreBoughtMedicine ||
				x.NuzlockeSkipDuplicates != y.NuzlockeSkipDuplicates ||
				x.OnlinePlayEnabled != y.OnlinePlayEnabled ||
				x.OverflowPokemonsIntoNextBox != y.OverflowPokemonsIntoNextBox ||
				x.PcDoesNotHealPokemons != y.PcDoesNotHealPokemons ||
				x.PokemonCenterHealsResetTrainers != y.PokemonCenterHealsResetTrainers ||
				x.PokemonCentersCostMoney != y.PokemonCentersCostMoney ||
				x.PokemonDeathOnFaint != y.PokemonDeathOnFaint ||
				x.PokemonSentToPcOnDeath != y.PokemonSentToPcOnDeath ||
				x.RandomBossEncounters != y.RandomBossEncounters ||
				x.RandomizePartyAfterBattle != y.RandomizePartyAfterBattle ||
				x.RandomStarters != y.RandomStarters ||
				x.RandomWildEncounters != y.RandomWildEncounters ||
				x.ReleasedPokemonsReturnToEncounterArea != y.ReleasedPokemonsReturnToEncounterArea ||
				x.ReleasedPokemonsRoamNearbyAreas != y.ReleasedPokemonsRoamNearbyAreas ||
				x.SandBoxMode != y.SandBoxMode;
		}
		public bool Equals(Feature obj)
		{
			if (obj == null) return false;
			return this == obj;
		}
		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (typeof(Feature) == obj.GetType())
				return Equals((Feature)obj);
			return base.Equals(obj);
		}
		bool IEquatable<Feature>.Equals(Feature other)
		{
			if (other == null) return false;
			return Equals(obj: (object)other);
		}
		public override int GetHashCode()
		{
			return GetGuid().GetHashCode();
		}

		bool IEqualityComparer<Feature>.Equals(Feature x, Feature y)
		{
			return x == y;
		}

		int IEqualityComparer<Feature>.GetHashCode(Feature obj)
		{
			return obj.GetHashCode();
		}
		#endregion


		#region Methods
		public string GetGuid()
		{
			return string.Format(""
				+ (BattleShiftStyle ? "1" : "0"							)
				+ (CanOnlyCaptureFirstWildEncounter ? "1" : "0"			)
				+ (CanOnlySaveAtPokemonCenter ? "1" : "0"				)
				+ (CatchPokemonsWithEggMoves ? "1" : "0"				)
				+ (ChallengeEndsAfter ? "1" : "0"						)
				+ (EnableCloudStorage ? "1" : "0"						)
				+ (FailChallengeWipesData ? "1" : "0"					)
				+ (ForcePokemonNaming ? "1" : "0"						)
				+ (GameOverOnWhiteOut ? "1" : "0"						)
				+ ("|"+((int)LimitPokemonPartySize).ToString()+"|"		)
				+ (NoCapturesAllowed ? "1" : "0"						)
				+ (NoHealingItemsInsideBattle ? "1" : "0"				)
				+ (NoHealingItemsOutsideBattle ? "1" : "0"				)
				+ (NoHealingNPCs ? "1" : "0"							)
				+ (NoPC ? "1" : "0"										)
				+ (NoPokemonCenterHeals ? "1" : "0"						)
				+ (NoStoreBoughtMedicine ? "1" : "0"					)
				+ (NuzlockeSkipDuplicates ? "1" : "0"					)
				+ (OnlinePlayEnabled ? "1" : "0"						)
				+ (OverflowPokemonsIntoNextBox ? "1" : "0"				)
				+ (PcDoesNotHealPokemons ? "1" : "0"					)
				+ (PokemonCenterHealsResetTrainers ? "1" : "0"			)
				+ (PokemonCentersCostMoney ? "1" : "0"					)
				+ (PokemonDeathOnFaint ? "1" : "0"						)
				+ (PokemonSentToPcOnDeath ? "1" : "0"					)
				+ (RandomBossEncounters ? "1" : "0"						)
				+ (RandomizePartyAfterBattle ? "1" : "0"				)
				+ (RandomStarters ? "1" : "0"							)
				+ (RandomWildEncounters ? "1" : "0"						)
				+ (ReleasedPokemonsReturnToEncounterArea ? "1" : "0"	)
				+ (ReleasedPokemonsRoamNearbyAreas ? "1" : "0"			)
				+ (SandBoxMode ? "1" : "0"								)
			);
		}

		public void LoadGuid(string guid)
		{
			string[] text = guid.Split('|');
			if (text.Length < 2) return;
			BattleShiftStyle						 = text[0][0] == '1';
			CanOnlyCaptureFirstWildEncounter		 = text[0][1] == '1';
			CanOnlySaveAtPokemonCenter				 = text[0][2] == '1';
			CatchPokemonsWithEggMoves				 = text[0][3] == '1';
			ChallengeEndsAfter						 = text[0][4] == '1';
			EnableCloudStorage						 = text[0][5] == '1';
			FailChallengeWipesData					 = text[0][6] == '1';
			ForcePokemonNaming						 = text[0][7] == '1';
			GameOverOnWhiteOut						 = text[0][8] == '1';
			LimitPokemonPartySize					 = (byte)int.Parse(text[1]);
			NoCapturesAllowed						 = text[2][0] == '1';
			NoHealingItemsInsideBattle				 = text[2][1] == '1';
			NoHealingItemsOutsideBattle				 = text[2][2] == '1';
			NoHealingNPCs							 = text[2][3] == '1';
			NoPC									 = text[2][4] == '1';
			NoPokemonCenterHeals					 = text[2][5] == '1';
			NoStoreBoughtMedicine					 = text[2][6] == '1';
			NuzlockeSkipDuplicates					 = text[2][7] == '1';
			OnlinePlayEnabled						 = text[2][8] == '1';
			OverflowPokemonsIntoNextBox				 = text[2][9] == '1';
			PcDoesNotHealPokemons					 = text[2][10] == '1';
			PokemonCenterHealsResetTrainers			 = text[2][11] == '1';
			PokemonCentersCostMoney					 = text[2][12] == '1';
			PokemonDeathOnFaint						 = text[2][13] == '1';
			PokemonSentToPcOnDeath					 = text[2][14] == '1';
			RandomBossEncounters					 = text[2][15] == '1';
			RandomizePartyAfterBattle				 = text[2][16] == '1';
			RandomStarters							 = text[2][17] == '1';
			RandomWildEncounters					 = text[2][18] == '1';
			ReleasedPokemonsReturnToEncounterArea	 = text[2][19] == '1';
			ReleasedPokemonsRoamNearbyAreas			 = text[2][20] == '1';
			SandBoxMode                              = text[2][21] == '1';
		}

		public void StartingOver()
		{
			StartingOverSwitch = true;
			PlayerHasStartedOverCounter += 1;
		}
		#endregion
	}

	#region Custom Game Mode
	// Challenge is different from Game Mode because...
	// Challenge affects your experience playing thru the open-world adventure
	// While Survival, Battle-Mode, Statium, Elite Four, Battle Frontier, [Misc] Contents...
	// can all be labeled as a Mode of gamplay, and be a stand-alone expierence without `World`-element
	public enum Challenges
	{
		/// <summary>
		/// </summary>
		Classic
		/// <summary>
		/// Nuzlocke Challenge => Pokemon Centers cost money, every pokemon must be named, when defeated/fainted pokemon is gone, only allowed to capture first pokemon encountered when entering new map
		/// </summary>
		,Nuzlocke
		/// <summary>
		/// Clone Challenge => RNG only pokemon you're allowed to use [and capture (maybe catch but not have in party)]
		/// </summary>
		,Clone
		/// <summary>
		/// DarkSouls Challenge => PokemonCenters Resets NPCs
		/// </summary>
		,DarkSouls
		/// <summary>
		/// Unevolved Challenge => Can only battle with pokemons in their first/base form
		/// </summary>
		,Unevolved
		/// <summary>
		/// Roleplay Challenge => RNG Random Character from pokemon Universe, and play with their pokemon Party
		/// </summary>
		,RolePlay
		/// <summary>
		/// Montoype Challenge => Like Gym Leader, theme entire collection of pokemon around a single type (or concept)
		/// </summary>
		,Monotype
		/// <summary>
		/// Catchless Challenge => No captures allowed, though npc gifts/trades are welcomed
		/// </summary>
		,Catchless
		/// <summary>
		/// Single Challenge => RNG or Starter pokemon only...
		/// </summary>
		,Single
		/// <summary>
		/// Shiny Challenge => Capture only allowed on shinies...
		/// </summary>
		,Shiny
		/// <summary>
		/// Alphabet Challenge => 26 Pokemons, only allowed to 1 of each letter of alphabet (in first letter of name); cant evolve unless you release existing letter
		/// </summary>
		,Alphabet
		/// <summary>
		/// No-Hit Challenge => Any damage taken even self inflicted suicide (not including status) causes challenge to be failed 
		/// </summary>
		,NoHit
		,ItemOnly
		/// <summary>
		/// Nuzlocke Challenge with Extra Spicy Sauce
		///	[1]
		///	You must nickname ALL pokemon you catch.
		///	[2]
		///	No healing items in or out of battle.
		///	REVIVEs and MAX REVIVES can be used only in the ways described below(not directly on pokemon).
		///	OPTIONAL: No healing outside the pokecenter at all(no healing NPCs)
		///	[3]
		///	If all your pokemon faint, the first one who fainted is dead.You can choose to either cremate it(release) or bury it(PC box)
		///	If a pokemon is fainted by a critical hit it is dead.
		///	Any time you have only one pokemon you can discard four REVIVEs or one MAX REVIVE to revive one buried pokemon.
		///	[4]
		///	Once you catch a pokemon in an area you can't catch another in that area.
		///	You must catch the first pokemon you encounter in the route.
		///	If you don't like the first pokemon or if it faints, you can try to catch the next instead by sacrificing one REVIVE for every pokemon you own(including PC and buried pokemon). For each retry you have to toss another set of REVIVEs but you can still only ever catch one pokemon per route.
		///	[The idea here is that you can cremate your dead pokemon so it's easier to get new pokemon you want or you can bury them so you can bring them back later but this makes it more expensive to retry catching a pokemon]
		///	[5]
		///	Once you enter a GYM you cannot leave until either all your pokemon faint, or you win.
		///	If you beat a GYM on the first try you may revive one buried pokemon(see cost above).
		/// </summary>
		,NuzlockeExtra
		,HardcoreNuzlocke
		,DarkSoulsNuzlocke
	}
	#endregion
}