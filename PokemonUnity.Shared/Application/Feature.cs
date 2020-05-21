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
		/// Cannot use PC fs of any sort (cant access stored pokemons or items)
		/// </summary>
		public bool NoPC { get; private set; }
		/// <summary>
		/// If entire party is defeated, player gets a white screen effect, and the game ends
		/// </summary>
		public bool GameOverOnWhiteOut { get; private set; }
		public bool RandomizePartyAfterBattle { get; private set; }
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
			return this == obj;
		}
		public override bool Equals(object obj)
		{
			if (typeof(Feature) == obj.GetType())
				return Equals((Feature)obj);
			return base.Equals(obj);
		}
		bool IEquatable<Feature>.Equals(Feature other)
		{
			return Equals(obj: (object)other);
		}
		public override int GetHashCode()
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
				).GetHashCode();
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
	}
	#endregion
}