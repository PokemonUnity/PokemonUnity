using System;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Overworld;
using PokemonUnity.UX;
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

    public interface IEncounter 
    {
        Pokemons Pokemon    { get; }
        int Level           { get; }
    }

    public interface IEncounters //Rename WildEncounters?
    {
        int[][] EnctypeChances    { get; }
        int[] EnctypeDensities    { get; }
        int[] EnctypeCompileDens  { get; }

        //IEncounters initialize();

        int stepcount { get; }

        void clearStepCount();

        bool hasEncounter(EncounterTypes enc);

        bool isCave();

        bool isGrass();

        bool isRegularGrass();

        bool isWater();

        EncounterTypes? pbEncounterType();

        bool isEncounterPossibleHere();

        void setup(int mapID);

        bool pbMapHasEncounter(int mapID, EncounterTypes enctype);

        IEncounter pbMapEncounter(int mapID, EncounterTypes enctype);
        //IPokemon pbMapEncounter(int mapID, EncounterTypes enctype);

        IEncounter pbEncounteredPokemon(EncounterTypes enctype, int tries = 1);
        //IPokemon pbEncounteredPokemon(EncounterTypes enctype, int tries = 1);

        bool pbCanEncounter(IPokemon encounter);

        IEncounter pbGenerateEncounter(EncounterTypes enctype);
        //IPokemon pbGenerateEncounter(EncounterTypes enctype);

        #region Encounter Modifiers
        //###############################################################################
        // This section was created solely for you to put various bits of code that
        // modify various wild Pokémon and trainers immediately prior to battling them.
        // Be sure that any code you use here ONLY applies to the Pokémon/trainers you
        // want it to apply to!
        //###############################################################################

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
        //     int newlevel=(int)Math.Round(Game.pbBalancedLevel(Game.GameData.Player.Party) - 4 + Core.Rand.Next(5));   // For variety
        //     if (newlevel < 1) newlevel=1;
        //     if (newlevel > Core.MAXIMUMLEVEL) newlevel = Core.MAXIMUMLEVEL;
        //     pokemon.Level=newlevel;
        //     //pokemon.calcStats();
        //     pokemon.resetMoves();
        //   }
        //}

        // This is the basis of a trainer modifier.  It works both for trainers loaded
        // when you battle them, and for partner trainers when they are registered.
        // Note that you can only modify a partner trainer's Pokémon, and not the trainer
        // themselves nor their items this way, as those are generated from scratch
        // before each battle.
        //Events.onTrainerPartyLoad+=proc {|sender,e|
        //private void onWildPokemonCreate(object sender, EventArgs e) {
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