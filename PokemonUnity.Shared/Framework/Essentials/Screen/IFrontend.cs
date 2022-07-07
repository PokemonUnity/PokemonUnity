using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
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

namespace PokemonEssentials.Interface.Screen
{
	#region Text Entry
	public interface IPokemonEntryScreen : IScreen
	{
		void initialize(IPokemonEntryScene scene);
		string pbStartScreen(string helptext, int minlength, int maxlength, string initialText, PokemonUnity.UX.TextEntryTypes mode = 0, IPokemon pokemon = null);
	}

	/// <summary>
	/// Text entry screen - free typing.
	/// </summary>
	public interface IPokemonEntryScene : IScene
	{
		void pbStartScene(string helptext, int minlength, int maxlength, string initialText, PokemonUnity.UX.TextEntryTypes subject = 0, IPokemon pokemon = null);
		void pbEndScene();
		string pbEntry();
		//string pbEntry1();
		//string pbEntry2();
	}

	/// <summary>
	/// Text entry screen - arrows to select letter.
	/// </summary>
	public interface IPokemonEntryScene2 : IScene, IPokemonEntryScene
	{
		//void pbStartScene(string helptext, int minlength, int maxlength, string initialText, int subject = 0, Pokemon pokemon = null);
		//void pbEndScene();
		//string pbEntry();
		void pbUpdate();
		void pbChangeTab(int newtab = 0);
		bool pbColumnEmpty(int m);
		void pbUpdateOverlay();
		void pbDoUpdateOverlay();
		void pbDoUpdateOverlay2();
		bool pbMoveCursor();
		int wrapmod(int x, int y);
	}

	public interface IGameTextEntry
	{
		string pbEnterText(string helptext, int minlength, int maxlength, string initialText = "", int mode = 0, IPokemon pokemon = null, bool nofadeout = false);

		string pbEnterPlayerName(string helptext, int minlength, int maxlength, string initialText = "", bool nofadeout = false);

		string pbEnterPokemonName(string helptext, int minlength, int maxlength, string initialText = "", IPokemon pokemon = null, bool nofadeout = false);

		string pbEnterBoxName(string helptext, int minlength, int maxlength, string initialText = "", bool nofadeout = false);

		string pbEnterNPCName(string helptext, int minlength, int maxlength, string initialText = "", int id = 0, bool nofadeout = false);
	}
	#endregion

	#region Evolution
	public interface IPokemonEvolutionScene : IScene
	{
		void pbEndScreen();
		void pbEvolution(bool cancancel = true);
		void pbFlashInOut(bool canceled,string oldstate,string oldstate2);
		void pbStartScreen(IPokemon pokemon, Pokemons newspecies);
		void pbUpdate(bool animating = false);
		void pbUpdateExpandScreen();
		void pbUpdateNarrowScreen();
	}
	#endregion
		
	public interface IGameScreensUI
	{
		PokemonEssentials.Interface.Screen.IPokemonEntryScreen TextEntryScreen { get; set; }
		PokemonEssentials.Interface.Screen.IIntroEventScreen IntroScreen { get; set; }
		PokemonEssentials.Interface.Screen.ILoadScreen Load { get; set; }
		PokemonEssentials.Interface.Screen.ISaveScreen Save { get; set; }
		PokemonEssentials.Interface.Screen.IOptionScreen OptionScreen { get; set; }
		PokemonEssentials.Interface.Screen.IPokemonMenuScreen PauseMenuScreen { get; set; }
		PokemonEssentials.Interface.Screen.IPartyDisplayScreen Party { get; set; }
		PokemonEssentials.Interface.Screen.IPokemonSummaryScreen Summary { get; set; }
		PokemonEssentials.Interface.Screen.IBagScreen Bag { get; set; }
		//PokemonEssentials.Interface.Screen. RegionMap { get; set; }
		//PokemonEssentials.Interface.Screen. Phone { get; set; }
		PokemonEssentials.Interface.Screen.ITrainerCardScreen TrainerCard { get; set; }
		PokemonEssentials.Interface.Screen.IPokemonStorageScreen PokemonStorageScreen { get; set; }
		PokemonEssentials.Interface.Screen.IPokemonPokedexScreen PokedexScreen { get; set; }
		PokemonEssentials.Interface.Screen.IPokemonNestMapScreen PokedexNestScreen { get; set; }
		PokemonEssentials.Interface.Screen.IPokemonFormScreen PokedexFormScreen { get; set; }
		//PokemonEssentials.Interface.Screen. EggHatching { get; set; }
		//PokemonEssentials.Interface.Screen. Trading { get; set; }
		//PokemonEssentials.Interface.Screen. MoveRelearner { get; set; }
		PokemonEssentials.Interface.Screen.IMartScreen Mart { get; set; }
		//PokemonEssentials.Interface.Screen. Debug { get; set; }
		PokemonEssentials.Interface.Screen.IRelicStoneScreen RelicStone { get; set; } //Shadow Pokemon
		//PokemonEssentials.Interface.Screen. PurityChamber { get; set; } //Shadow Pokemon
		PokemonEssentials.Interface.Screen.IBattleSwapScreen BattleSwapScreen { get; set; }

		IGameScreensUI initialize(params PokemonEssentials.Interface.Screen.IScreen[] screens);
	}
		
	public interface IGameScenesUI
	{
		PokemonEssentials.Interface.Screen.IPokemonEntryScene TextEntryScene { get; set; }
		PokemonEssentials.Interface.Screen.IIntroEventScene IntroScene { get; set; }
		PokemonEssentials.Interface.Screen.ILoadScene Load { get; set; }
		PokemonEssentials.Interface.Screen.ISaveScene Save { get; set; }
		PokemonEssentials.Interface.Screen.IOptionScene OptionScene { get; set; }
		PokemonEssentials.Interface.Screen.IPokemonMenuScene PauseMenuScene { get; set; }
		PokemonEssentials.Interface.Screen.IPartyDisplayScene Party { get; set; }
		PokemonEssentials.Interface.Screen.IPokemonSummaryScene Summary { get; set; }
		PokemonEssentials.Interface.Screen.IBagScene Bag { get; set; }
		PokemonEssentials.Interface.Screen.IItemStorageScene Bag_ItemStore { get; set; }
		PokemonEssentials.Interface.Screen.IWithdrawItemScene Bag_ItemWithdraw { get; set; }
		PokemonEssentials.Interface.Screen.ITossItemScene Bag_ItemToss { get; set; }
		//PokemonEssentials.Interface.Screen. RegionMap { get; set; }
		//PokemonEssentials.Interface.Screen. Phone { get; set; }
		PokemonEssentials.Interface.Screen.IPokegearScene PokeGear { get; set; }
		PokemonEssentials.Interface.Screen.ITrainerCardScene TrainerCard { get; set; }
		PokemonEssentials.Interface.Screen.IPokemonStorageScene PokemonStorageScene { get; set; }
		PokemonEssentials.Interface.Screen.IPokemonPokedexScene PokedexScene { get; set; }
		PokemonEssentials.Interface.Screen.IPokemonNestMapScene PokedexNestScene { get; set; }
		PokemonEssentials.Interface.Screen.IPokemonFormScene PokedexFormScene { get; set; }
		PokemonEssentials.Interface.Screen.IPokemonEvolutionScene EvolvingScene { get; set; }
		//PokemonEssentials.Interface.Screen. EggHatching { get; set; }
		//PokemonEssentials.Interface.Screen. Trading { get; set; }
		//PokemonEssentials.Interface.Screen. MoveRelearner { get; set; }
		PokemonEssentials.Interface.Screen.IMartScene Mart { get; set; }
		//PokemonEssentials.Interface.Screen. Debug { get; set; }
		PokemonEssentials.Interface.Screen.IRelicStoneScene RelicStone { get; set; } //Shadow Pokemon
		PokemonEssentials.Interface.Screen.IPurifyChamberScene PurityChamber { get; set; } //Shadow Pokemon


		PokemonEssentials.Interface.Screen.IPokeBattle_Scene BattleScene { get; set; }
		PokemonEssentials.Interface.Screen.IPokeBattleArena_Scene BattleArenaScene { get; set; }
		PokemonEssentials.Interface.Screen.IBattleSwapScene BattleSwapScene { get; set; }
		//PokemonEssentials.Interface.Screen. BattlePalaceScene { get; set; }
		//PokemonEssentials.Interface.Screen. BattleSafari { get; set; }
		PokemonEssentials.Interface.Screen.IPokeBattle_DebugScene BattleSceneDebug { get; set; }
		PokemonEssentials.Interface.Screen.IPokeBattle_SceneNonInteractive BattleSceneDebugNoUI { get; set; }
		PokemonEssentials.Interface.Screen.IPokeBattle_DebugSceneNoLogging BattleSceneDebugWithoutLog { get; set; }
		PokemonEssentials.Interface.Screen.IPokeBattle_DebugSceneNoGraphics BattleSceneDebugWithoutGfx { get; set; }

		IGameScenesUI initialize(params PokemonEssentials.Interface.Screen.IScene[] scenes);
	}
}