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
//using System.Security.Cryptography.MD5;

namespace PokemonUnity
{
	public partial class Game
	{
		#region Custom Game Features
		//Nuzlocke Challenge => Pokemon Centers cost money, every pokemon must be named, when defeated/fainted pokemon is gone, only allowed to capture first pokemon encountered when entering new map
		/// <summary>
		/// Basically, you use the Dexnav to find pokemon in the area, they appear as shadows in the grass, and you need to sneak up on them
		/// these pokemon can have egg moves, or even their HiddenAbility
		/// </summary>
		/// Apparently you can use the Sneaking feature to helps with this. 
		/// ToDo: OnlyAllowEggMovesWhenUsingDexNav or DexNavAllowsEggMoves
		public static bool CatchPokemonsWithEggMoves { get; private set; }
		//ForcePokemonNaming
		//PokemonCentersCostMoney
		//PokemonDeathOnFaint
		//CanOnlyCaptureFirstWildEncounter
		//CanOnlySaveAtPokemonCenter
		//PokemonCenterHealsResetTrainers
		//RandomStarters
		//RandomWildEncounters
		//RandomBossEncounters (like randomwild, except normal map encounter sprinkled in with high difficulty spawns)
		//NoCapturesAllowed
		//NuzlockeSkipDuplicates
		//ReleasedPokemonsRoamNearbyAreas
		//NoPokemonCenterHeals
		//LimitPokemonPartySize (from 6 to 3?...)
		//NoStoreBoughtMedicine
		//PcDoesNotHealPokemons
		#endregion

		#region Custom Game Mode
		//Nuzlocke Challenge => Pokemon Centers cost money, every pokemon must be named, when defeated/fainted pokemon is gone, only allowed to capture first pokemon encountered when entering new map
		//Clone Challenge => RNG only pokemon you're allowed to use [and capture (maybe catch but not have in party)]
		//DarkSouls Challenge => PokemonCenters Resets NPCs
		//Unevolved Challenge => Can only battle with pokemons in their first/base form
		//Roleplay Challenge => RNG Random Character from pokemon Universe, and play with their pokemon Party
		//Montoype Challenge => Like Gym Leader, theme entire collection of pokemon around a single type (or concept)
		//Catchless Challenge => No captures allowed, though npc gifts/trades are welcomed
		//Single Challenge => RNG or Starter pokemon only...
		//Shiny Challenge => Capture only allowed on shinies...
		//Alphabet Challenge => 26 Pokemons, only allowed to 1 of each letter of alphabet (in first letter of name); cant evolve unless you release existing letter
		//Nuzlocke Challenge with Extra Spicy Sauce
		//[1]
		//You must nickname ALL pokemon you catch.
		//[2]
		//No healing items in or out of battle.
		//REVIVEs and MAX REVIVES can be used only in the ways described below(not directly on pokemon).
		//OPTIONAL: No healing outside the pokecenter at all(no healing NPCs)
		//[3]
		//If all your pokemon faint, the first one who fainted is dead.You can choose to either cremate it(release) or bury it(PC box)
		//If a pokemon is fainted by a critical hit it is dead.
		//Any time you have only one pokemon you can discard four REVIVEs or one MAX REVIVE to revive one buried pokemon.
		//[4]
		//Once you catch a pokemon in an area you can't catch another in that area.
		//You must catch the first pokemon you encounter in the route.
		//If you don't like the first pokemon or if it faints, you can try to catch the next instead by sacrificing one REVIVE for every pokemon you own(including PC and buried pokemon). For each retry you have to toss another set of REVIVEs but you can still only ever catch one pokemon per route.
		//[The idea here is that you can cremate your dead pokemon so it's easier to get new pokemon you want or you can bury them so you can bring them back later but this makes it more expensive to retry catching a pokemon]
		//[5]
		//Once you enter a GYM you cannot leave until either all your pokemon faint, or you win.
		//If you beat a GYM on the first try you may revive one buried pokemon(see cost above).
		#endregion
	}
}