using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.UX;
using PokemonUnity.Combat;
using PokemonUnity.Character;
using PokemonUnity.Inventory;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
//using PokemonEssentials.Interface.PokeBattle.Rules;
using PokemonEssentials.Interface.EventArg;
using PokemonEssentials.Interface.RPGMaker;
using PokemonEssentials.Interface.RPGMaker.Kernal;

namespace PokemonEssentials.Interface.Screen
{
	public interface IPokemonPokedexScreen : IScreen
	{
		IPokemonPokedexScreen initialize(IPokemonPokedexScene scene);
		void pbDexEntry(Pokemons species);
		void pbStartScreen();
	}
	public interface IPokemonPokedexScene : IScene
	{
		IPokemonPokedexScene initialize();
		void pbUpdate();
		void pbEndScene();
		void pbStartScene();
		void pbStartDexEntryScene(Pokemons species);
		void pbPokedex();
		void pbDexEntry(int index);
		int pbDexSearch();
		void pbCloseSearch();
		IEnumerable<PokemonUnity.Monster.Data.PokemonData> pbSearchDexList(params object[] param);
		List<PokemonUnity.Monster.Data.PokemonData> pbGetDexList();
		void pbRefreshDexList(int index = 0);
		void pbRefreshDexSearch(params string[] param);
		bool pbCanAddForModeList(int mode, Pokemons nationalSpecies);
		bool pbCanAddForModeSearch(int mode, Pokemons nationalSpecies);
		void pbChangeToDexEntry(Pokemons species);
		int pbDexSearchCommands(string[] commands, int selitem, string[] helptexts = null);
		int pbGetPokedexRegion();
		int pbGetSavePositionIndex();
		void pbMiddleDexEntryScene();
		void setIconBitmap(Pokemons species);
	}

	// ===============================================================================
	// Pokédex menu screen
	// * For choosing which region list to view.  Only appears when there is more
	//   than one viable region list to choose from, and if DEXDEPENDSONLOCATION is
	//   false.
	// * Adapted from the Pokégear menu script by Maruno.
	// ===============================================================================
	public interface IWindow_DexesList : IWindow_CommandPokemon
	{
		IWindow_DexesList initialize(string[] commands, float width, int seen, int owned);

		//void drawItem(int index, int count, IRect rect);
	}

	public interface IScene_PokedexMenu 
	{
		IScene_PokedexMenu initialize(int menu_index = 0);

		void main();

		void update();

		void update_command();
	}
	
	// ===============================================================================
	// Pokédex main screen
	// ===============================================================================
	public interface IWindow_CommandPokemonWhiteArrow : IWindow_CommandPokemon 
	{
		//void drawCursor(int index, IRect rect);
	}

	public interface IWindow_Pokedex : IWindow_DrawableCommand, IDisposable 
	{
		string[] commands { set; }

		Pokemons species { get; }

		//int itemCount { get; }

		new IWindow_Pokedex initialize(float x, float y, float width, float height);

		//void dispose();
		//
		//void drawCursor(int index, IRect rect);
		//
		//void drawItem(int index, int count, IRect rect);
	}

	public interface IWindow_ComplexCommandPokemon : IWindow_DrawableCommand 
	{
		string[] commands				{ get; set; }
		
		//float width { set; }
		//
		//float height { set; }

		IWindow_ComplexCommandPokemon initialize(string[] commands, float? width = null);

		IRect Empty(float x, float y, float width, float height, IViewport viewport = null);

		//int index { set; }

		//[i1, i2]
		//int[] indexToCommand(int index);
		KeyValuePair<int,int> indexToCommand(int index);

		string getText(string[] array, int index);

		void resizeToFit(string[] commands);

		//int itemCount();

		//void drawItem(int index, int count, IRect rect);
	}
}