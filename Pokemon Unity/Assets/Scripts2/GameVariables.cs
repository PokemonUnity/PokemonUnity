using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
//using Localization;
//using Localization.Dictionaries;
//using Localization.Dictionaries.Xml;
//using System.IO;
using System.Xml;
using System.Collections;
//using System.Collections.Immutable;
using System.Globalization;

/// <summary>
/// Variables that are stored when game is saved, and other temp values used for gameplay
/// </summary>
public class GameVariables : UnityEngine.MonoBehaviour//, UnityEngine.EventSystems.
{
    #region Variables
    //Game UI
    //public UnityEngine.Texture2D DialogWindowSkin;
    //private UnityEngine.UI.Image DialogWindowSkin;
    /// <summary>
    /// Frame Style
    /// </summary>
    public static UnityEngine.Sprite WindowSkin { get; private set; }
    public static UnityEngine.Sprite DialogSkin { get; private set; }
    /// <summary>
    /// Music Volume
    /// </summary>
    public static float mVol = (7f / 20f) * (7f / 20f);
    /// <summary>
    /// SFX (Sound Effects) Volume 
    /// </summary>
    public static float sVol = (14f / 20f) * (14f / 20f);
    public static bool battleScene = true;
    public static bool fullscreen;
    public static byte textSpeed = 2;

    //public static Translator.Languages UserLanguage = Translator.Languages.English;
    public static Settings.Languages UserLanguage = Settings.Languages.English;

    private byte slotIndex;
    //private int buildID;
    public static bool SaveFileFound;
    public System.DateTimeOffset fileCreationDate;
    public System.DateTimeOffset? lastSave;
    public System.DateTimeOffset startTime = new System.DateTimeOffset();
    //private double buildNum = 0.17;
    //var t = new System.Resources.ResourceManager().
    #if DEBUG
    private static string FILE_NAME = @"..\..\..\\Pokemon Unity\Assets\Scripts2\Translations\"; //TestProject\bin\Debug
    //string file = System.Environment.CurrentDirectory + @"\Resources\Database\Pokemon\Pokemon_" + fileLanguage + ".xml"; //TestProject\bin\Debug
    //string file =  @"$(SolutionDir)\Assets\Resources\Database\Pokemon\Pokemon_" + fileLanguage + ".xml"; //Doesnt work
#else
    private string FILE_NAME = UnityEngine.Application.persistentDataPath + "/Test.data";
    //string filepath = UnityEngine.Application.dataPath + "/Scripts2/Translations/";//Resources/Database/Pokemon/Pokemon_" + fileLanguage + ".xml"; //Use for production
#endif
    #endregion

    /// <summary>
    /// Loads saved game data from memory slot
    /// </summary>
    /// <param name="i">Array int from binary stream</param>
    public static void Load(byte i)
    {
        GameVariables.SaveLoad.Load();
    }
    public static void Save()
    {
        //using (System.IO.BinaryWriter writer = new System.IO.BinaryWriter(System.IO.File.Open(FILE_NAME,)))
        //GameVariables.SaveLoad.Save();
    }
    #region Save/Load Data
    private class SaveLoad {
        #region Variables
        //int DatabaseEntryStringWidth = 100;
        System.IO.FileStream fs;
        //BinaryWriter w; //= new BinaryWriter(fs);
        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        //GameVariables data = new GameVariables();
        GameVariables[] gamesaves = new GameVariables[3];
        Translator.Languages userpreflanguage = Translator.Languages.English;
        #endregion

        public static void Save(System.IO.BinaryWriter writer) { }
        public static void Load() { }
        
        /// <summary>
        /// When initially boots up, this will be all the application data stored
        /// on user's PersonalComputer (PC). If first time running game, naturally
        /// stored data would not exist, and the game will produce one by default.
        /// </summary>
        void OldMe()
        {
            //UnityEngine.Debug.Log("Checking to see if BinaryText exist...");
            if (System.IO.File.Exists(FILE_NAME))
            {
                //UnityEngine.Debug.Log(FILE_NAME + " already exists!");

                //UnityEngine.Debug.Log("Loading Old Info from BinaryText...");
                // Create the reader for data.
                //fs = new FileStream(FILE_NAME, FileMode.Open, FileAccess.Read);
                fs = System.IO.File.Open(FILE_NAME, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                //UnityEngine.Debug.Log("Information Loaded.");
                //UnityEngine.Debug.Log("Deserializing Information...");
                //data = (GameVariables)bf.Deserialize(fs);
                SaveLoad d = (SaveLoad)bf.Deserialize(fs);
                //UnityEngine.Debug.Log("Rewriting TextField/Variables...");

                //username = data.username;
                //rememberme = data.rememberme;
                Settings.UserLanguage = d.userpreflanguage;

                //An Array[3] of GameVariables representing GameSaves
                gamesaves = d.gamesaves;//(GameVariables[])bf.Deserialize(fs);
                SaveFileFound = true;

                //UnityEngine.Debug.Log("Information Loaded and Updated.");
                //UnityEngine.Debug.Log("Closing BinaryText...");
                //UnityEngine.Debug.Log("Closing FileStream...");
                fs.Close();
                //UnityEngine.Debug.Log("BinaryText Closed.");
                return;
            }
            else
            {
                SaveFileFound = false;
                System.IO.File.Open(FILE_NAME, System.IO.FileMode.Create).Close();
            }
        }
    }
    #endregion
}

/// <summary>
/// <see cref="GlobalVariables"/> is a <see cref="UnityEngine.GameObject"/>
/// that persist throughout the game and inbetween scene levels.
/// <see cref="Settings"/> is not an <see cref="object"/>
/// but a series of const variables that will be used as rules or 
/// placeholders for the game mechanics.
/// </summary>
public class Settings //: Settings<Translations.Languages>
{
    #region Constant Values and Game Rules
    public static Translator.Languages UserLanguage = Translator.Languages.English;
    public static bool TextLTR { get; private set; }
    //public XmlFileLocalizationDictionaryProvider TranslationText;// = new XmlFileLocalizationDictionaryProvider(Server.MapPath("~/App_Data/"));
    //information.Initialize("PokemonUnity");

    /// <summary>
    /// 
    /// </summary>
    public const float framesPerSecond = 30f;
    #endregion

    #region Variables
    #endregion

    #region Properties
    #endregion

    #region Resources
    public UnityEngine.Sprite[] LoadAllWindowSkinSprites()
    {
        return UnityEngine.Resources.LoadAll<UnityEngine.Sprite>(@"\Sprites\GUI\Frame\WindowSkin");
    }

    public UnityEngine.Sprite[] LoadAllDialogSkinSprites()
    {
        return UnityEngine.Resources.LoadAll<UnityEngine.Sprite>(@"\Sprites\GUI\Frame\DialogSkin");
    }
    #endregion

    #region Custom Game Mode
    //Nuzlocke Challenge
    #endregion

    /* Tried to attempt a PokemonRNG for converting ints into bytes of data
     * but since the engine will handle all the heavy lifting, i'll just 
     * continue to build the logic out as normal and not worry or bother 
     * with binary values
    public int seed;
    void newSeed()
    {
        //Random t = new Random(seed);
        if(seed == 0) seed = (UInt16)new Random().Next(0, UInt16.MaxValue);
        if (!useFixedSeed) { 
            if(true) seed = (UInt16)(seed * 0x41C64E6D + 0x6073);
            else {  
            seed = (UInt16)new Random().Next(0, UInt16.MaxValue);
            seed ^= (UInt16)System.DateTime.Now.Ticks;
            seed &= UInt16.MaxValue;}
        }
    }*/     
    private static System.UInt16 seed = 0x0000; //{ get; set; }
    public static UInt16 Seed   { get {
            if(seed == 0x0000) seed = (UInt16)new Random().Next(0, UInt16.MaxValue);
            if (!useFixedSeed) { 
                if(true) seed = (UInt16)(seed * 0x41C64E6D + 0x6073);
                /*else {  
                seed = (UInt16)new Random().Next(0, UInt16.MaxValue);
                seed ^= (UInt16)System.DateTime.Now.Ticks;
                seed &= UInt16.MaxValue;}*/
            }
            return seed;
        }
    } //readonly
    public static bool useFixedSeed;
    #region Enumerators
    /// <summary>
    /// Still need to sort out Language Enums
    /// </summary>
    public enum Languages //: Languages<Translations.Languages>//Translator.Languages//
    {
        /// <summary>
        /// US English
        /// </summary>
        English = 9
    }
    /*public class Languagess : ILanguage<Translations.Languages>
    {
        public enum Translations.Languages { get; set; } 
    }*/
    #endregion

	#region Screen Options
	/// <summary>
	/// The default screen width (at a zoom of 1.0; size is half this at zoom 0.5).
	/// </summary>
	public const int DEFAULTSCREENWIDTH = 512;
	/// <summary>
	/// The default screen height (at a zoom of 1.0).
	/// </summary>
	public const int DEFAULTSCREENHEIGHT = 384;
	/// <summary>
	/// The default screen zoom. (1.0 means each tile is 32x32 pixels, 0.5 means
	/// each tile is 16x16 pixels, 2.0 means each tile is 64x64 pixels.)
	/// </summary>
	public const float DEFAULTSCREENZOOM = 1.0f;
	/// <summary>
	/// Whether full-screen display lets the border graphic go outside the edges of
	/// the screen (true), or forces the border graphic to always be fully shown
	/// </summary>
	public const bool FULLSCREENBORDERCROP = false;
	/// <summary>
	/// The width of each of the left and right sides of the screen border. This is
	/// added on to the screen width above, only if the border is turned on.
	/// </summary>
	public const int BORDERWIDTH = 80;
	/// <summary>
	/// The height of each of the top and bottom sides of the screen border. This is
	/// added on to the screen height above, only if the border is turned on.
	/// </summary>
	public const int BORDERHEIGHT = 80;
	/// <summary>
	/// Map view mode (0=original, 1=custom, 2=perspective).
	/// </summary>
	public const int MAPVIEWMODE = 1;
	#endregion

	#region Pokemon Settings
	/// <summary>
	/// The maximum level Pokémon can reach.
	/// </summary>
	public const int MAXIMUMLEVEL = 100;
	/// <summary>
	/// The level of newly hatched Pokémon.
	/// </summary>
	public const int EGGINITIALLEVEL = 1;
	/// <summary>
	/// The odds of a newly generated Pokémon being shiny (out of 65536).
	/// </summary>
	public const int SHINYPOKEMONCHANCE = 8;
	/// <summary>
	/// The odds of a wild Pokémon/bred egg having Pokérus (out of 65536).
	/// </summary>
	public const int POKERUSCHANCE = 3;
	#endregion

	#region
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

	#region
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
	public static readonly eItems.Item[] MEGARINGS = new eItems.Item[] { eItems.Item.MEGA_RING/*, eItems.Item.MEGA_BRACELET, eItems.Item.MEGA_CUFF, eItems.Item.MEGA_CHARM*/ };
	#endregion

	#region
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
	/// ToDo: Consider a mechanic that allows each region to have their own TM/HM #s Region[Generation,Badges] <see cref="eItems.Item"/>
	/// <summary>
	/// </summary>
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
		
	#region
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
		
	#region
	/// <summary>
	/// Whether the Pokédex list shown is the one for the player's current region
	///    (true), or whether a menu pops up for the player to manually choose which
	///    Dex list to view when appropriate (false).
	/// </summary>
	public const bool DEXDEPENDSONLOCATION = false;
	/// <summary>
	/// The names of each Dex list in the game, in order and with National Dex at
	///    the end. This is also the order that PokemonGlobal.pokedexUnlocked is
	///    in, which records which Dexes have been unlocked (first is unlocked by
	///    default).<para></para>
	///    You can define which region a particular Dex list is linked to. This
	///    means the area map shown while viewing that Dex list will ALWAYS be that
	///    of the defined region, rather than whichever region the player is
	///    currently in. To define this, put the Dex name and the region number in
	///    an array, like the Kanto and Johto Dexes are. The National Dex isn't in
	///    an array with a region number, therefore its area map is whichever region
	///    the player is currently in.
	/// </summary>
	public class Pokedex {
		//[_INTL("Kanto Pokédex"),0],
		//[_INTL("Johto Pokédex"),1],
		//_INTL("National Pokédex")

		/// <summary>
		/// Translated from DB
		/// </summary>
		string Name;
		/// <summary>
		/// Generation
		/// </summary>
		/// Equal to or greater than 0 assigns to region id
		int Region = -1;
		int PokedexId;
		/// <summary>
		/// An array of numbers, where each number is that of a Dex list (National Dex
		///    is -1). All Dex lists included here have the species numbers in them
		///    reduced by 1, thus making the first listed species have a species number
		///    of 0 (e.g. Victini in Unova's Dex).
		/// </summary>
		/// ToDo: Pokedex should be assigned pokemonId start and end numbers
		/// Example: Johto is pokemons 1-151, while national is all... 
		int DEXINDEXOFFSETS;//    = []
	}
	/// <summary>
	/// Whether all forms of a given species will be immediately available to view
	///    in the Pokédex so long as that species has been seen at all (true), or
	///    whether each form needs to be seen specifically before that form appears
	///    in the Pokédex (false).
	/// </summary>
	public const bool ALWAYSSHOWALLFORMS = false;
	#endregion

	#region
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
		
	#region
	/*// <summary>
	/// A list of maps used by roaming Pokémon. Each map has an array of other maps
	///    it can lead to.
	/// </summary>
	RoamingAreas = {
	5  => [21,28,31,39,41,44,47,66,69],
	21 => [5,28,31,39,41,44,47,66,69],
	28 => [5,21,31,39,41,44,47,66,69],
	31 => [5,21,28,39,41,44,47,66,69],
	39 => [5,21,28,31,41,44,47,66,69],
	41 => [5,21,28,31,39,44,47,66,69],
	44 => [5,21,28,31,39,41,47,66,69],
	47 => [5,21,28,31,39,41,44,66,69],
	66 => [5,21,28,31,39,41,44,47,69],
	69 => [5,21,28,31,39,41,44,47,66]
	}
	/// <summary>
	/// A set of arrays each containing the details of a roaming Pokémon. The
	///    information within is as follows:<para></para>
	///    - Species.<para></para>
	///    - Level.<para></para>
	///    - Global Switch; the Pokémon roams while this is ON.<para></para>
	///    - Encounter type (0=any, 1=grass/walking in cave, 2=surfing, 3=fishing,<para></para>
	///         4=surfing/fishing). See bottom of PField_RoamingPokemon for lists.<para></para>
	///    - Name of BGM to play for that encounter (optional).<para></para>
	///    - Roaming areas specifically for this Pokémon (optional).
	/// </summary>
	RoamingSpecies = [
	[:LATIAS, 30, 53, 0, "Battle roaming"],
	[:LATIOS, 30, 53, 0, "Battle roaming"],
	[:KYOGRE, 40, 54, 2, nil, {
		2  => [21,31],
		21 => [2,31,69],
		31 => [2,21,69],
		69 => [21,31]
		}],
	[:ENTEI, 40, 55, 1, nil]
	]*/
	#endregion

	#region
	/// <summary>
	/// A set of arrays each containing details of a wild encounter that can only
	///    occur via using the Poké Radar. The information within is as follows:<para></para>
	///    - Map ID on which this encounter can occur.<para></para>
	///    - Probability that this encounter will occur (as a percentage).<para></para>
	///    - Species.<para></para>
	///    - Minimum possible level.<para></para>
	///    - Maximum possible level (optional).
	/// </summary>
	public class PokeRadars
	{
		int MapId;
		int EncounterChance;
		Pokemon.PokemonData.Pokemons Species;
		/// <summary>
		/// LevelMin is 0 in Array.
		/// LevelMax is 1 in Array.
		/// LevelMax is Optional.
		/// </summary>
		int[] LevelMinMax;
		/*POKERADAREXCLUSIVES=[
		[5,  20, :STARLY,     12, 15],
		[21, 10, :STANTLER,   14],
		[28, 20, :BUTTERFREE, 15, 18],
		[28, 20, :BEEDRILL,   15, 18]
		]*/
	}
	#endregion

	#region
	/// <summary>
	/// The number of steps allowed before a Safari Zone game is over (0=infinite).
	/// </summary>
	public const int SAFARISTEPS = 600;
	/// <summary>
	/// The number of seconds a Bug Catching Contest lasts for (0=infinite).
	/// </summary>
	public const int BUGCONTESTTIME = 1200;
	#endregion

	#region
	/// <summary>
	/// The Global Switch that is set to ON when the player whites out.
	/// </summary>
	public const bool STARTING_OVER_SWITCH = true;
	/// <summary>
	/// The Global Switch that is set to ON when the player has seen Pokérus in the
	///    Poké Center, and doesn't need to be told about it again.
	/// </summary>
	public const bool SEEN_POKERUS_SWITCH = false;
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
}

public static class TransformExtension
{
    public static UnityEngine.Transform ClearChildren (this UnityEngine.Transform transform)
    {
        foreach (UnityEngine.Transform child in transform)
        {
            UnityEngine.GameObject.Destroy(child.gameObject);
        }
        return transform;
    }
}