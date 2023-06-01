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
		void DexEntry(Pokemons species);
		void StartScreen();
	}
	public interface IPokemonPokedexScene : IScene
	{
		IPokemonPokedexScene initialize();
		void Update();
		void EndScene();
		void StartScene();
		void StartDexEntryScene(Pokemons species);
		void Pokedex();
		void DexEntry(int index);
		int DexSearch();
		void CloseSearch();
		IEnumerable<PokemonUnity.Monster.Data.PokemonData> SearchDexList(params object[] param);
		List<PokemonUnity.Monster.Data.PokemonData> GetDexList();
		void RefreshDexList(int index = 0);
		void RefreshDexSearch(params string[] param);
		bool CanAddForModeList(int mode, Pokemons nationalSpecies);
		bool CanAddForModeSearch(int mode, Pokemons nationalSpecies);
		void ChangeToDexEntry(Pokemons species);
		int DexSearchCommands(string[] commands, int selitem, string[] helptexts = null);
		int GetPokedexRegion();
		int GetSavePositionIndex();
		void MiddleDexEntryScene();
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