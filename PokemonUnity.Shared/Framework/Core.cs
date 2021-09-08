using System;
using PokemonUnity.Inventory;

namespace PokemonUnity
{
	/// <summary>
	/// Static and unchanging rules for game to function off of.<para></para>
	/// <see cref="Game"/> is a <see cref="UnityEngine.GameObject"/>
	/// that persist throughout the game and inbetween scene levels.
	/// <see cref="Core"/> is not an <see cref="object"/>
	/// but a series of const variables that will be used as rules 
	/// for the game mechanics or structure.
	/// </summary>
	public static class Core
	{
		#region Constant Values and Game Rules
		public static Translator.Languages UserLanguage { get; set; } //= Translator.Languages.English;
		public static bool TextLTR { get; private set; }
		//Ping server for latest hash value?...
		//public const string PKU_Server_Address = "";
#if DEBUG
		public const bool DEBUG = true;
		/// <summary>
		/// If pokemon battles are being done in an console for ai training, or visually for player experience
		/// </summary>
		public const bool INTERNAL = true;
		/// <summary>
		/// Location of text file for game translation file import
		/// </summary>
		public const string FILEPATH = @"..\..\..\\Pokemon Unity\Assets\Scripts2\XML\";//\Pokemon\Pokemon_" + fileLanguage + ".xml"; //TestProject\bin\Debug
		//string filepath = @"..\..\..\\Pokemon Unity\Assets\Scripts2\Translations\";//\Pokemon\Pokemon_" + fileLanguage + ".xml"; //TestProject\bin\Debug
		//string file = System.Environment.CurrentDirectory + @"\Resources\Database\Pokemon\Pokemon_" + fileLanguage + ".xml"; //TestProject\bin\Debug
		//string file =  @"$(SolutionDir)\Assets\Resources\Database\Pokemon\Pokemon_" + fileLanguage + ".xml"; //Doesnt work
#else
		public const bool DEBUG = false;
		public const string FILEPATH = UnityEngine.Application.dataPath + "/Scripts2/XML/";//Resources/Database/Pokemon/Pokemon_" + fileLanguage + ".xml"; //Use for production
		//string filepath = UnityEngine.Application.dataPath + "/Scripts2/Translations/";//Resources/Database/Pokemon/Pokemon_" + fileLanguage + ".xml"; //Use for production
#endif
		//public XmlFileLocalizationDictionaryProvider TranslationText;// = new XmlFileLocalizationDictionaryProvider(Server.MapPath("~/App_Data/"));
		public const string FILENAME_POKEMON_DATABASE = "";

		/// <summary>
		/// </summary>
		public const float framesPerSecond = 30f;

		public const sbyte pokemonGeneration = (sbyte)Generation.All;

#pragma warning disable 0162 //Warning CS0162  Unreachable code detected 
		public static int pokemonGenerationCount { 
			get { 
				int MaxPoke = 0;
				int Gen1 = 151;
				int Gen2 = 251;
				int Gen3 = 386;
				int Gen4 = 493;
				int Gen5 = 649;
				int Gen6 = 721;
				int Gen7 = 807;

				#region Generation
				switch (pokemonGeneration)
				{
					case (1):
						MaxPoke = Gen1;
						break;
					case (2):
						MaxPoke = Gen2;
						break;
					case (3):
						MaxPoke = Gen3;
						break;
					case (4):
						MaxPoke = Gen4;
						break;
					case (5):
						MaxPoke = Gen5;
						break;
					case (6):
						MaxPoke = Gen6;
						break;
					case (-1):
					case (0):
					case (7):
					default:
						MaxPoke = Gen7;
						break;
				}
				#endregion
				return MaxPoke; }
		}
#pragma warning restore 0162 //Warning CS0162  Unreachable code detected 
		#endregion

		#region Variables
		/// <summary>
		/// Constantly revolving random, that won't repeat the same seed number twice, 
		/// until it cycles thru all possible seed values
		/// </summary>
		public static Random Rand { get { return new Random(Seed()); } }
		/// <summary>
		/// Constantly revolving random, that uses the same seed number that was previously used
		/// </summary>
		public static Random RandWithSetSeed { get { return new Random(Seed(true)); } }
		public static System.UInt16? seed { get; private set; }// = 0x0000;
		public static UInt16 Seed(bool useFixedSeed = false)
		{
			//lock (Rand)
			//{
				if (!seed.HasValue) {
					//seed = (UInt16)new Random().Next(0, UInt16.MaxValue);
					seed = (UInt16)new Random(DateTime.Now.Millisecond).Next(0, UInt16.MaxValue); 
					seed ^= (UInt16)System.DateTime.Now.Ticks;
					seed &= UInt16.MaxValue;
				}
				if (!useFixedSeed) { 
					seed = (UInt16)(seed * 0x41C64E6D + 0x6073);
				} 
				return seed.Value;
			//}
		}

		/*/// <summary>
		/// Only an example. Do not use, will  not work.
		/// <para>Could be combined with database values 
		/// and used with ints instead of strings</para>
		/// <para>Convert the pokemon type into a color 
		/// that can be used with Unity's color lighting</para>
		/// </summary>
		/// <param name="color"></param>
		/// <returns></returns>
		public UnityEngine.Color ColorToUnity(Color color) {
			switch (color)
			{
				//case 1:
				//	return StringToColorDic["text"];
				default:
					return StringToColor(color.ToString());
					break;
			}
				
			//{"Black",Color.black },//dark
			//{"", new Color() },//dark blue -> dark, 
			{ "Blue",Color.blue },//water
			{ "Clear",Color.clear },
			{ "Cyan",Color.cyan },
			{ "Gray",Color.gray },//grAy-American
			//{"Grey",Color.grey },//grEy-European
			//{"Grey",Color.grey },//dark grey -> rock,
			{ "Green",Color.green },//grass
			//{"", new Color() },//dark green -> bug,
			{ "Magenta",Color.magenta },//magenta, purple -> poison
			{ "Red",Color.red },//orange, dark red -> fire
			{ "White",Color.white },//normals
			{ "Yellow",Color.yellow },//electric
			{ "Purple", new Color() },//ghost
			{ "Brown", new Color() },//fighting
			{ "Pink", new Color() }//,//fairy
			//{"", new Color() },//pink, lavender -> psychic, 
			//{"", new Color() },//ocre, brown -> ground
			//{"", new Color() },
			//{"", new Color() },
			//{"", new Color() }//fly, drag, steel, psychic, ice, shadow, unknown, bug, ground, poison?
		
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
		/// <summary>
		/// The default text scroll speed (from 1-6, default being 4)
		/// </summary>
		public const int SCROLL_SPEED_DEFAULT = 4;
		#endregion

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

		#region Move Settings
		/// <summary>
		/// If moves deal type damage (or if damage is affected by move type)
		/// </summary>
		public const byte NOTYPE					= 0x01;
		/// <summary>
		/// If pokemon type change damage (or if move damage is affected by pokemon type)
		/// </summary>
		/// Double negatives (ignore), so in code the bool is reversed
		public const byte IGNOREPKMNTYPES			= 0x02;
		/// <summary
		/// If RNG affects damage
		/// </summary>
		public const byte NOWEIGHTING				= 0x04;
		/// <summary>
		/// If Moves can do Crit (extra) Damage
		/// </summary>
		public const byte NOCRITICAL				= 0x08;
		/// <summary>
		/// If Reflect-like Moves ignore Damage Modifiers
		/// </summary>
		public const byte NOREFLECT					= 0x10;
		/// <summary>
		/// I actually dont know what this is about...
		/// </summary>
		/// Use Ctrl+F to locate, i commented this out in code
		public const byte SELFCONFUSE				= 0x20;
		#endregion
	}
}