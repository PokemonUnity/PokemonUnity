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
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonEssentials.Interface
{
	//	public static partial class PBEvolution {
	//  Unknown           = 0; // Do not use
	//  Happiness         = 1;
	//  HappinessDay      = 2;
	//  HappinessNight    = 3;
	//  Level             = 4;
	//  Trade             = 5;
	//  TradeItem         = 6;
	//  Item              = 7;
	//  AttackGreater     = 8;
	//  AtkDefEqual       = 9;
	//  DefenseGreater    = 10;
	//  Silcoon           = 11;
	//  Cascoon           = 12;
	//  Ninjask           = 13;
	//  Shedinja          = 14;
	//  Beauty            = 15;
	//  ItemMale          = 16;
	//  ItemFemale        = 17;
	//  DayHoldItem       = 18;
	//  NightHoldItem     = 19;
	//  HasMove           = 20;
	//  HasInParty        = 21;
	//  LevelMale         = 22;
	//  LevelFemale       = 23;
	//  Location          = 24;
	//  TradeSpecies      = 25;
	//  LevelDay          = 26;
	//  LevelNight        = 27;
	//  LevelDarkInParty  = 28;
	//  LevelRain         = 29;
	//  HappinessMoveType = 30;
	//  Custom1           = 31;
	//  Custom2           = 32;
	//  Custom3           = 33;
	//  Custom4           = 34;
	//  Custom5           = 35;

	//  EVONAMES=["Unknown",
	//	 "Happiness","HappinessDay","HappinessNight","Level","Trade",
	//	 "TradeItem","Item","AttackGreater","AtkDefEqual","DefenseGreater",
	//	 "Silcoon","Cascoon","Ninjask","Shedinja","Beauty",
	//	 "ItemMale","ItemFemale","DayHoldItem","NightHoldItem","HasMove",
	//	 "HasInParty","LevelMale","LevelFemale","Location","TradeSpecies",
	//	 "LevelDay","LevelNight","LevelDarkInParty","LevelRain","HappinessMoveType",
	//	 "Custom1","Custom2","Custom3","Custom4","Custom5";
	//  ];

	////  0 = no parameter
	////  1 = Positive integer
	////  2 = Item internal name
	////  3 = Move internal name
	////  4 = Species internal name
	////  5 = Type internal name
	//  EVOPARAM=[0,    // Unknown (do not use)
	//	 0,0,0,1,0,   // Happiness, HappinessDay, HappinessNight, Level, Trade
	//	 2,2,1,1,1,   // TradeItem, Item, AttackGreater, AtkDefEqual, DefenseGreater
	//	 1,1,1,1,1,   // Silcoon, Cascoon, Ninjask, Shedinja, Beauty
	//	 2,2,2,2,3,   // ItemMale, ItemFemale, DayHoldItem, NightHoldItem, HasMove
	//	 4,1,1,1,4,   // HasInParty, LevelMale, LevelFemale, Location, TradeSpecies
	//	 1,1,1,1,5,   // LevelDay, LevelNight, LevelDarkInParty, LevelRain, HappinessMoveType
	//	 1,1,1,1,1;    // Custom 1-5
	//  ];
	//}

	/// <summary>
	/// Extensions of <seealso cref="IPokemon"/>
	/// </summary>
	public interface IGamePokemonEvolution
	{
		// ===============================================================================
		// Evolution helper functions
		// ===============================================================================
		PokemonUnity.Monster.Data.PokemonEvolution[] pbGetEvolvedFormData(Pokemons species);

		//Loops through each pokemon in db with evolution, 
		//every 5 pokemons, log in debug output pokemon evolution
		void pbEvoDebug();

		Pokemons pbGetPreviousForm(Pokemons species);

		int pbGetMinimumLevel(Pokemons species);

		Pokemons pbGetBabySpecies(Pokemons species, Items item1 = Items.NONE, Items item2 = Items.NONE);

		// ===============================================================================
		// Evolution methods
		// ===============================================================================
		Pokemons pbMiniCheckEvolution(IPokemon pokemon, EvolutionMethod evonib, int level, Pokemons poke);

		Pokemons pbMiniCheckEvolutionItem(IPokemon pokemon, EvolutionMethod evonib, Items level, Pokemons poke, Items item);

		/// <summary>
		/// Checks whether a Pokemon can evolve now. If a block is given, calls the block
		/// with the following parameters:<para></para>
		/// Pokemon to check; evolution type; level or other parameter; ID of the new Pokemon species
		/// </summary>
		/// <param name="pokemon"></param>
		Pokemons pbCheckEvolutionEx(IPokemon pokemon);

		/// <summary>
		/// Checks whether a Pokemon can evolve now. If an item is used on the Pokémon,
		/// checks whether the Pokemon can evolve with the given item.
		/// </summary>
		/// <param name="pokemon"></param>
		/// <param name="item"></param>
		Pokemons[] pbCheckEvolution(IPokemon pokemon, Items item = 0);

		// ===============================================================================
		// Evolution animation
		// ===============================================================================
		/*public void pbSaveSpriteState(ISprite sprite) {
		  state=[];
		  if (!sprite || sprite.disposed?) return state;
		  state[SpriteMetafile.BITMAP]     = sprite.x;
		  state[SpriteMetafile.X]          = sprite.x;
		  state[SpriteMetafile.Y]          = sprite.y;
		  state[SpriteMetafile.SRC_RECT]   = sprite.src_rect.clone();
		  state[SpriteMetafile.VISIBLE]    = sprite.visible;
		  state[SpriteMetafile.Z]          = sprite.z;
		  state[SpriteMetafile.OX]         = sprite.ox;
		  state[SpriteMetafile.OY]         = sprite.oy;
		  state[SpriteMetafile.ZOOM_X]     = sprite.zoom_x;
		  state[SpriteMetafile.ZOOM_Y]     = sprite.zoom_y;
		  state[SpriteMetafile.ANGLE]      = sprite.angle;
		  state[SpriteMetafile.MIRROR]     = sprite.mirror;
		  state[SpriteMetafile.BUSH_DEPTH] = sprite.bush_depth;
		  state[SpriteMetafile.OPACITY]    = sprite.opacity;
		  state[SpriteMetafile.BLEND_TYPE] = sprite.blend_type;
		  state[SpriteMetafile.COLOR]      = sprite.color.clone();
		  state[SpriteMetafile.CONE]       = sprite.tone.clone();
		  return state;
		}

		public void pbRestoreSpriteState(ISprite sprite,state) {
		  if (!state || !sprite || sprite.disposed?) return;
		  sprite.x          = state[SpriteMetafile.X];
		  sprite.y          = state[SpriteMetafile.Y];
		  sprite.src_rect   = state[SpriteMetafile.SRC_RECT];
		  sprite.visible    = state[SpriteMetafile.VISIBLE];
		  sprite.z          = state[SpriteMetafile.Z];
		  sprite.ox         = state[SpriteMetafile.OX];
		  sprite.oy         = state[SpriteMetafile.OY];
		  sprite.zoom_x     = state[SpriteMetafile.ZOOM_X];
		  sprite.zoom_y     = state[SpriteMetafile.ZOOM_Y];
		  sprite.angle      = state[SpriteMetafile.ANGLE];
		  sprite.mirror     = state[SpriteMetafile.MIRROR];
		  sprite.bush_depth = state[SpriteMetafile.BUSH_DEPTH];
		  sprite.opacity    = state[SpriteMetafile.OPACITY];
		  sprite.blend_type = state[SpriteMetafile.BLEND_TYPE];
		  sprite.color      = state[SpriteMetafile.COLOR];
		  sprite.tone       = state[SpriteMetafile.TONE];
		}

		public void pbSaveSpriteStateAndBitmap(sprite) {
		  if (!sprite || sprite.disposed?) return [];
		  state=pbSaveSpriteState(sprite);
		  state[SpriteMetafile.eITMAP]=sprite.bitmap;
		  return state;
		}

		public void pbRestoreSpriteStateAndBitmap(sprite,state) {
		  if (!state || !sprite || sprite.disposed?) return;
		  sprite.bitmap=state[SpriteMetafile.eITMAP];
		  pbRestoreSpriteState(sprite,state);
		  return state;
		}*/
	}

	/*public partial class SpriteMetafile {
		//VIEWPORT      = 0;
		//TONE          = 1;
		//SRC_RECT      = 2;
		//VISIBLE       = 3;
		//X             = 4;
		//Y             = 5;
		//Z             = 6;
		//OX            = 7;
		//OY            = 8;
		//ZOOM_X        = 9;
		//ZOOM_Y        = 10;
		//ANGLE         = 11;
		//MIRROR        = 12;
		//BUSH_DEPTH    = 13;
		//OPACITY       = 14;
		//BLEND_TYPE    = 15;
		//COLOR         = 16;
		//FLASHCOLOR    = 17;
		//FLASHDURATION = 18;
		//BITMAP        = 19;

		public void length { get; }

		//void this[int i] { get; }

		void initialize(IViewport viewport = null);

		bool disposed();

		void dispose();

		void flash(color, duration);

		int x { get; set; }

		int y { get; set; }
		void bitmap { get; set; }

		IRect src_rect { get; set; }

		void visible { get; set; }

		int z { get; set; }

		void ox { get; set; }

		void oy { get; set; }

		void zoom_x { get; set; }

		void zoom_y { get; set; }

		void zoom { set; }

		void angle { get; set; }

		void mirror { get; set; }

		void bush_depth { get; set; }

		void opacity { get; set; }

		void blend_type { get; set; }

		void color { get; set; }

		void tone { get; set; }

		void update();
	}

	public partial class SpriteMetafilePlayer {
		public void initialize(metafile, sprite= null);

		public void add(sprite);

		public bool playing();

		public void play();

		public void update();
	}*/
}