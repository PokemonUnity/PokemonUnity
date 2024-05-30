using System;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Overworld;
using PokemonUnity.Interface;
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonEssentials.Interface.Field
{
	//public interface IEncounterTypes {
	//    int Land                  { get; }
	//    int Cave                  { get; }
	//    int Water                 { get; }
	//    int RockSmash             { get; }
	//    int OldRod                { get; }
	//    int GoodRod               { get; }
	//    int SuperRod              { get; }
	//    int HeadbuttLow           { get; }
	//    int HeadbuttHigh          { get; }
	//    int LandMorning           { get; }
	//    int LandDay               { get; }
	//    int LandNight             { get; }
	//    int BugContest            { get; }
	//    string[] Names            { get; }
	//    int[][] EnctypeChances    { get; }
	//    int[] EnctypeDensities    { get; }
	//    int[] EnctypeCompileDens  { get; }
	//}

	public interface IEncounterPokemon
	{
		Pokemons Pokemon    { get; }
		int MinLevel		{ get; }
		int MaxLevel		{ get; }
	}

	public interface IEncounters //: PokemonEssentials.Interface.Field.IEncounterModifier //ToDo: Rename to WildEncounters or EncounterData?
	{
		/// <summary>
		/// Chances of pokemon encounter equal up to 100%
		/// </summary>
		/// Technically all land encounters share the same odds
		//int[][] EnctypeChances    { get; }
		IDictionary<EncounterOptions, int[]> EnctypeChances    { get; }
		/// <summary>
		/// Density is rate encounter;
		/// How often player is to encounter a pokemon
		/// </summary>
		//int[] EnctypeDensities    { get; }
		IDictionary<EncounterOptions, int> EnctypeDensities    { get; }
		//IDictionary<EncounterOptions, int> EnctypeDensities    { get; }
		//int[] EnctypeCompileDens  { get; } //Not sure what this is for, but may not be needed with enough data from database
		IDictionary<EncounterOptions, IList<IEncounterPokemon>> EnctypeEncounters { get; }

		//IEncounters initialize();

		int stepcount { get; }

		void clearStepCount();

		bool hasEncounter(EncounterOptions enc);
		//bool hasEncounter(Method enc);

		bool IsCave { get; }

		bool IsGrass { get; }

		bool IsRegularGrass { get; }

		bool IsWater { get; }

		EncounterOptions? EncounterType();
		//Method? EncounterType();

		bool IsEncounterPossibleHere { get; }

		void setup(int mapID);

		bool MapHasEncounter(int mapID, EncounterOptions enctype);
		//bool MapHasEncounter(int mapID, Method enctype);

		IPokemon MapEncounter(int mapID, EncounterOptions enctype);
		//IEncounterPokemon MapEncounter(int mapID, Method enctype);

		IPokemon EncounteredPokemon(EncounterOptions enctype, int tries = 1);
		//IEncounterPokemon EncounteredPokemon(Method enctype, int tries = 1);

		bool CanEncounter(IPokemon encounter);

		IPokemon GenerateEncounter(EncounterOptions enctype);
		//IEncounterPokemon GenerateEncounter(Method enctype);

		#region Encounter Modifiers
		//###############################################################################
		// This section was created solely for you to put various bits of code that
		// modify various wild Pokémon and trainers immediately prior to battling them.
		// Be sure that any code you use here ONLY applies to the Pokémon/trainers you
		// want it to apply to!
		//###############################################################################

		/// <summary>
		/// Triggers whenever a wild Pokémon is created
		/// </summary>
		//event EventHandler<IOnWildPokemonCreateEventArgs> OnWildPokemonCreate;
		event Action<object, EventArg.IOnWildPokemonCreateEventArgs> OnWildPokemonCreate;
		//Events.onWildPokemonCreate+=proc {|sender,e|
		//private void onWildPokemonCreate(object sender, EventArg.OnWildPokemonCreateEventArgs e) {
		//   // Make all wild Pokémon shiny while a certain Switch is ON (see Settings).
		//   //Monster.Pokemon pokemon = e[0];
		//   Monster.Pokemon pokemon = e.Pokemon;
		//   if (Core.SHINY_WILD_POKEMON_SWITCH) {
		//     pokemon.makeShiny();
		//   }
		//}

		// Used in the random dungeon map.  Makes the levels of all wild Pokémon in that
		// map depend on the levels of Pokémon in the player's party.
		// This is a simple method, and can/should be modified to account for evolutions
		// and other such details.  Of course, you don't HAVE to use this code.
		//Events.onWildPokemonCreate+=proc {|sender,e|
		//private void onWildPokemonCreate(object sender, EventArg.OnWildPokemonCreateEventArgs e) {
		//   Monster.Pokemon pokemon = e[0];
		//   if (Game.GameData.GameMap.map_id==51) {
		//     int newlevel=(int)Math.Round(Game.BalancedLevel(Game.GameData.Player.Party) - 4 + Core.Rand.Next(5));   // For variety
		//     if (newlevel < 1) newlevel=1;
		//     if (newlevel > Core.MAXIMUMLEVEL) newlevel = Core.MAXIMUMLEVEL;
		//     pokemon.Level=newlevel;
		//     //pokemon.calcStats();
		//     pokemon.resetMoves();
		//   }
		//}

		/// <summary>
		/// Triggers whenever an NPC trainer's Pokémon party is loaded.
		/// It works both for trainers loaded
		/// when you battle them, and for partner trainers when they are registered.
		/// Note that you can only modify a partner trainer's Pokémon, and not the trainer
		/// themselves nor their items this way, as those are generated from scratch
		/// before each battle.
		/// </summary>
		/// This is the basis of a trainer modifier.
		//event EventHandler<IOnTrainerPartyLoadEventArgs> OnTrainerPartyLoad;
		event Action<object, EventArg.IOnTrainerPartyLoadEventArgs> OnTrainerPartyLoad;
		//Events.onTrainerPartyLoad+=proc {|sender,e|
		//private void onTrainerPartyLoad(object sender, EventArgs e) {
		//   if (e[0]) { // Game.GameData.Trainer data should exist to be loaded, but may not exist somehow
		//     Game.GameData.Trainer trainer=e[0][0]; // A PokeBattle_Trainer object of the loaded trainer
		//     Items[] items=e[0][1];   // An array of the trainer's items they can use
		//     Monster.Pokemon[] party=e[0][2];   // An array of the trainer's Pokémon
		//     //YOUR CODE HERE
		//   }
		//}
		#endregion
	}
}