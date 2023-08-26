using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Inventory;
using PokemonUnity.Saving;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.EventArg;
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonEssentials.Interface
{
	/// <summary>
	/// </summary>
	public interface IGameUtility
	{
		//static string _INTL(string message, params object[] param);

		#region General purpose utilities
		bool _pbNextComb(int[] comb, int length);

		/// <summary>
		/// Iterates through the array and yields each combination of <paramref name="num"/> 
		/// elements in the array.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="num"></param>
		/// <returns></returns>
		//IEnumerator<T[]> pbEachCombination<T>(T[] array, int num);
		IEnumerable<T[]> pbEachCombination<T>(T[] array, int num);

		/// <summary>
		/// Gets the path of the user's "My Documents" folder.
		/// </summary>
		/// <returns></returns>
		string pbGetMyDocumentsFolder();

		/// <summary>
		/// Returns a country ID
		/// </summary>
		/// http://msdn.microsoft.com/en-us/library/dd374073%28VS.85%29.aspx?
		int pbGetCountry();

		/// <summary>
		/// Returns a language ID
		/// </summary>
		int pbGetLanguage();

		/// <summary>
		/// Converts a Celsius temperature to Fahrenheit.
		/// </summary>
		/// <param name="celsius"></param>
		/// <returns></returns>
		double toFahrenheit(float celsius);

		/// <summary>
		/// Converts a Fahrenheit temperature to Celsius.
		/// </summary>
		/// <param name="fahrenheit"></param>
		/// <returns></returns>
		double toCelsius(float fahrenheit);
		#endregion

		#region Player-related utilities, random name generator
		//bool pbChangePlayer(int id);
		void pbChangePlayer(int id);

		string pbGetPlayerGraphic();

		TrainerTypes pbGetPlayerTrainerType();

		int pbGetTrainerTypeGender(TrainerTypes trainertype);
		//bool? pbGetTrainerTypeGender(TrainerTypes trainertype);

		void pbTrainerName(string name = null, int outfit = 0);

		string pbSuggestTrainerName(int gender);

		string pbGetUserName();

		string getRandomNameEx(int type, int? variable, int? upper, int maxLength = 100);

		string getRandomName(int maxLength = 100);
		#endregion

		#region Event timing utilities
		void pbTimeEvent(int? variableNumber, int secs = 86400);

		void pbTimeEventDays(int? variableNumber, int days = 0);

		bool pbTimeEventValid(int? variableNumber);
		#endregion

		#region General-purpose utilities with dependencies
		/// <summary>
		/// Similar to pbFadeOutIn, but pauses the music as it fades out.
		/// Requires scripts "Audio" (for bgm_pause) and "SpriteWindow" (for pbFadeOutIn).
		/// </summary>
		/// <param name="zViewport"></param>
		void pbFadeOutInWithMusic(int zViewport, Action block = null);

		/// <summary>
		/// Gets the wave data from a file and displays an message if an error occurs.
		/// Can optionally delete the wave file (this is useful if the file was a
		/// temporary file created by a recording).
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="deleteFile"></param>
		/// <returns></returns>
		/// Requires the script AudioUtilities
		/// Requires the script "PokemonMessages"
		IWaveData getWaveDataUI(string filename, bool deleteFile = false);

		/// <summary>
		/// Starts recording, and displays a message if the recording failed to start.
		/// </summary>
		/// <returns>Returns true if successful, false otherwise</returns>
		/// Requires the script AudioUtilities
		/// Requires the script "PokemonMessages"
		bool beginRecordUI();

		IList<object> pbHideVisibleObjects();

		void pbShowObjects(IList<object> visibleObjects);

		void pbLoadRpgxpScene(ISceneMap scene);

		/// <summary>
		/// Gets the value of a variable.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		object pbGet(int? id);

		/// <summary>
		/// Sets the value of a variable.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="value"></param>
		void pbSet(int? id, object value);

		/// <summary>
		/// Runs a common event and waits until the common event is finished.
		/// </summary>
		/// Requires the script "PokemonMessages"
		/// <param name="id"></param>
		/// <returns></returns>
		bool pbCommonEvent(int id);

		void pbExclaim(IGameCharacter[] @event, int id = Core.EXCLAMATION_ANIMATION_ID, bool tinting = false);

		void pbNoticePlayer(IGameCharacter @event);
		#endregion

		#region Loads Pokémon/item/trainer graphics
		[System.Obsolete("Unused")] string pbPokemonBitmapFile(Pokemons species, bool shiny, bool back = false);

		//IAnimatedBitmap pbLoadPokemonBitmap(IPokemon pokemon, bool back = false);

		// Note: Returns an AnimatedBitmap, not a Bitmap
		//IAnimatedBitmap pbLoadPokemonBitmapSpecies(IPokemon pokemon, Pokemons species, bool back= false);

		// Note: Returns an AnimatedBitmap, not a Bitmap
		//IAnimatedBitmap pbLoadSpeciesBitmap(Pokemons species, bool female = false, int form = 0, bool shiny = false, bool shadow = false, bool back = false, bool egg = false);

		//IBitmap pbCheckPokemonBitmapFiles(Pokemons species, bool female = false, int form = 0, bool shiny = false, bool shadow = false, bool back = false, bool egg = false);
		string pbCheckPokemonBitmapFiles(params object[] arg);

		//IAnimatedBitmap pbLoadPokemonIcon(IPokemon pokemon);

		string pbPokemonIconFile(IPokemon pokemon);

		string pbCheckPokemonIconFiles(object[] param, bool egg = false);

		// Used by the Pokédex
		string pbPokemonFootprintFile(Pokemons pokemon);
		string pbPokemonFootprintFile(IPokemon pokemon);

		string pbItemIconFile(Items item);

		string pbMailBackFile(Items item);

		string pbTrainerCharFile(TrainerTypes type);

		string pbTrainerCharNameFile(TrainerTypes type);

		string pbTrainerHeadFile(TrainerTypes type);

		string pbPlayerHeadFile(TrainerTypes type);

		string pbTrainerSpriteFile(TrainerTypes type);

		string pbTrainerSpriteBackFile(TrainerTypes type);

		string pbPlayerSpriteFile(TrainerTypes type);

		string pbPlayerSpriteBackFile(TrainerTypes type);
		#endregion

		#region Loads music and sound effects
		string pbResolveAudioSE(string file);

		int pbCryFrameLength(IPokemon pokemon, float? pitch = null);

		void pbPlayCry(Pokemons pokemon, int volume = 90, float? pitch = null);

		void pbPlayCry(IPokemon pokemon, int volume = 90, float? pitch = null);

		string pbCryFile(Pokemons pokemon);

		string pbCryFile(IPokemon pokemon);

		IAudioBGM pbGetWildBattleBGM(Pokemons species);

		IAudioME pbGetWildVictoryME();

		void pbPlayTrainerIntroME(TrainerTypes trainertype);

		// can be a PokeBattle_Trainer or an array of PokeBattle_Trainer
		IAudioBGM pbGetTrainerBattleBGM(params ITrainer[] trainer);

		IAudioBGM pbGetTrainerBattleBGMFromType(TrainerTypes trainertype);

		// can be a PokeBattle_Trainer or an array of PokeBattle_Trainer
		IAudioME pbGetTrainerVictoryME(params ITrainer[] trainer);
		#endregion

		#region Creating and storing Pokémon
		bool pbBoxesFull();

		void pbNickname(IPokemon pokemon);

		void pbStorePokemon(IPokemon pokemon);

		void pbNicknameAndStore(IPokemon pokemon);

		bool pbAddPokemon(Pokemons pokemon, int? level = null, bool seeform = true);
		bool pbAddPokemon(IPokemon pokemon, int? level = null, bool seeform = true);

		bool pbAddPokemonSilent(Pokemons pokemon, int? level = null, bool seeform = true);
		bool pbAddPokemonSilent(IPokemon pokemon, int? level = null, bool seeform = true);

		bool pbAddToParty(Pokemons pokemon, int? level = null, bool seeform = true);
		bool pbAddToParty(IPokemon pokemon, int? level = null, bool seeform = true);

		bool pbAddToPartySilent(Pokemons pokemon, int? level = null, bool seeform = true);
		bool pbAddToPartySilent(IPokemon pokemon, int? level = null, bool seeform = true);

		bool pbAddForeignPokemon(Pokemons pokemon,int? level=null,string ownerName=null,string nickname=null,int ownerGender=0,bool seeform=true);
		bool pbAddForeignPokemon(IPokemon pokemon,int? level=null,string ownerName=null,string nickname=null,int ownerGender=0,bool seeform=true);

		bool pbGenerateEgg(Pokemons pokemon, string text = "");
		bool pbGenerateEgg(IPokemon pokemon, string text = "");

		bool pbRemovePokemonAt(int index);

		void pbSeenForm(Pokemons poke, int gender = 0, int form = 0);
		void pbSeenForm(IPokemon poke);//, int gender = 0, int form = 0
		#endregion

		#region Analysing Pokémon
		/// <summary>
		/// Heals all Pokémon in the party.
		/// </summary>
		void pbHealAll();

		/// <summary>
		/// Returns the first unfainted, non-egg Pokémon in the player's party.
		/// </summary>
		/// <param name="variableNumber"></param>
		/// <returns></returns>
		IPokemon pbFirstAblePokemon(int variableNumber);

		/// <summary>
		/// Checks whether the player would still have an unfainted Pokémon if the
		/// Pokémon given by <paramref name="pokemonIndex"/> were removed from the party.
		/// </summary>
		/// <param name="pokemonIndex"></param>
		/// <returns></returns>
		bool pbCheckAble(int pokemonIndex);

		/// <summary>
		/// Returns true if there are no usable Pokémon in the player's party.
		/// </summary>
		/// <returns></returns>
		bool pbAllFainted();

		double pbBalancedLevel(IPokemon[] party);

		/// <summary>
		/// Returns the Pokémon's size in millimeters.
		/// </summary>
		/// <param name="pokemon"></param>
		/// <returns></returns>
		int pbSize(IPokemon pokemon);

		/// <summary>
		/// Returns true if the given species can be legitimately obtained as an egg.
		/// </summary>
		/// <param name="species"></param>
		/// <returns></returns>
		bool pbHasEgg(Pokemons species);
		#endregion

		#region Look through Pokémon in storage, choose a Pokémon in the party
		/// <summary>
		/// Yields every Pokémon/egg in storage in turn.
		/// </summary>
		/// <returns></returns>
		IEnumerable<KeyValuePair<IPokemon, int>> pbEachPokemon();

		/// <summary>
		/// Yields every Pokémon in storage in turn.
		/// </summary>
		/// <returns></returns>
		IEnumerable<KeyValuePair<IPokemon, int>> pbEachNonEggPokemon();

		/// <summary>
		/// Choose a Pokémon/egg from the party.
		/// Stores result in variable <paramref name="variableNumber"/> and the chosen Pokémon's name in
		/// variable <paramref name="nameVarNumber"/>; result is -1 if no Pokémon was chosen
		/// </summary>
		/// <param name="variableNumber">Stores result in variable</param>
		/// <param name="nameVarNumber">the chosen Pokémon's name in variable</param>
		/// <param name="ableProc">an array of which pokemons are affected</param>
		/// <param name="allowIneligible"></param>
		/// Supposed to return a value of pokemon chosen by player... as an int...
		/// ToDo: Instead of assigning value to variable, change void to return int
		void pbChoosePokemon(int variableNumber, int nameVarNumber, Predicate<IPokemon> ableProc = null, bool allowIneligible = false);

		void pbChooseNonEggPokemon(int variableNumber, int nameVarNumber);

		void pbChooseAblePokemon(int variableNumber, int nameVarNumber);

		void pbChoosePokemonForTrade(int variableNumber, int nameVarNumber, Pokemons wanted);
		#endregion

		#region Checks through the party for something
		bool pbHasSpecies(Pokemons species);

		bool pbHasFatefulSpecies(Pokemons species);

		bool pbHasType(Types type);

		/// <summary>
		/// Checks whether any Pokémon in the party knows the given move, and returns
		/// the index of that Pokémon, or null if no Pokémon has that move.
		/// </summary>
		/// <param name="move"></param>
		/// <returns></returns>
		IPokemon pbCheckMove(Moves move);
		#endregion

		#region Regional and National Pokédexes
		/// <summary>
		/// Gets the Regional Pokédex number of the national species for the specified
		/// Regional Dex. The parameter "region" is zero-based. For example, if two
		/// regions are defined, they would each be specified as 0 and 1.
		/// </summary>
		/// <param name="region"></param>
		/// <param name="nationalSpecies"></param>
		/// <returns></returns>
		int pbGetRegionalNumber(int region, Pokemons nationalSpecies);

		/// <summary>
		/// Gets the National Pokédex number of the specified species and region. The
		/// parameter "region" is zero-based. For example, if two regions are defined,
		/// they would each be specified as 0 and 1.
		/// </summary>
		/// <param name="region"></param>
		/// <param name="regionalSpecies"></param>
		/// <returns></returns>
		int pbGetNationalNumber(int region, Pokemons regionalSpecies);

		/// <summary>
		/// Gets an array of all national species within the given Regional Dex, sorted by
		/// Regional Dex number. The number of items in the array should be the
		/// number of species in the Regional Dex plus 1, since index 0 is considered
		/// to be empty. The parameter "region" is zero-based. For example, if two
		/// regions are defined, they would each be specified as 0 and 1.
		/// </summary>
		/// <param name="region"></param>
		/// <returns></returns>
		Pokemons[] pbAllRegionalSpecies(int region);

		/// <summary>
		/// Gets the ID number for the current region based on the player's current
		/// position. 
		/// </summary>
		/// <param name="defaultRegion">
		/// Returns the value of "defaultRegion" (optional, default is -1) if
		/// no region was defined in the game's metadata.
		/// </param>
		/// <returns>
		/// The ID numbers returned by
		/// this function depend on the current map's position metadata.
		/// </returns>
		int pbGetCurrentRegion(int defaultRegion = -1);

		/// <summary>
		/// Decides which Dex lists are able to be viewed (i.e. they are unlocked and have
		/// at least 1 seen species in them), and saves all viable dex region numbers
		/// (National Dex comes after regional dexes).<para>
		/// If the Dex list shown depends on the player's location, this just decides if
		/// a species in the current region has been seen - doesn't look at other regions.</para>
		/// Here, just used to decide whether to show the Pokédex in the Pause menu.
		/// </summary>
		void pbSetViableDexes();

		/// <summary>
		/// Unlocks a Dex list. The National Dex is -1 here (or null argument).
		/// </summary>
		/// <param name="dex"></param>
		void pbUnlockDex(int dex = -1);

		/// <summary>
		/// Locks a Dex list. The National Dex is -1 here (or null argument).
		/// </summary>
		/// <param name="dex"></param>
		void pbLockDex(int dex = -1);
		#endregion

		#region Other utilities
		void pbTextEntry(string helptext, int minlength, int maxlength, int variableNumber);

		string[] pbMoveTutorAnnotations(Moves move, Pokemons[] movelist = null);

		bool pbMoveTutorChoose(Moves move, Pokemons[] movelist = null, bool bymachine = false);

		void pbChooseMove(IPokemon pokemon, int variableNumber, int nameVarNumber);
		#endregion
	}
}