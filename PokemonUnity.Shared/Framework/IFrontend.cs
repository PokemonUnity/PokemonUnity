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
	public interface IGameScreensUI
	{
		PokemonEssentials.Interface.Screen.IPokemonEntryScreen TextEntryScreen { get; }
		PokemonEssentials.Interface.Screen.IIntroEventScreen IntroScreen { get; }
		PokemonEssentials.Interface.Screen.ILoadScreen Load { get; }
		PokemonEssentials.Interface.Screen.ISaveScreen Save { get; }
		PokemonEssentials.Interface.Screen.IOptionScreen OptionScreen { get; }
		PokemonEssentials.Interface.Screen.IPokemonMenuScreen PauseMenuScreen { get; }
		PokemonEssentials.Interface.Screen.IPartyDisplayScreen Party { get; }
		PokemonEssentials.Interface.Screen.IPokemonSummaryScreen Summary { get; }
		PokemonEssentials.Interface.Screen.IBagScreen Bag { get; }
		//PokemonEssentials.Interface.Screen. RegionMap { get; }
		//PokemonEssentials.Interface.Screen. Phone { get; }
		PokemonEssentials.Interface.Screen.ITrainerCardScreen TrainerCard { get; }
		PokemonEssentials.Interface.Screen.IPokemonStorageScreen PokemonStorageScreen { get; }
		PokemonEssentials.Interface.Screen.IPokemonPokedexScreen PokedexScreen { get; }
		PokemonEssentials.Interface.Screen.IPokemonNestMapScreen PokedexNestScreen { get; }
		PokemonEssentials.Interface.Screen.IPokemonFormScreen PokedexFormScreen { get; }
		//PokemonEssentials.Interface.Screen. EggHatching { get; }
		//PokemonEssentials.Interface.Screen. Trading { get; }
		//PokemonEssentials.Interface.Screen. MoveRelearner { get; }
		PokemonEssentials.Interface.Screen.IMartScreen Mart { get; }
		//PokemonEssentials.Interface.Screen. Debug { get; }
		PokemonEssentials.Interface.Screen.IRelicStoneScreen RelicStone { get; } //Shadow Pokemon
		//PokemonEssentials.Interface.Screen. PurityChamber { get; } //Shadow Pokemon
		PokemonEssentials.Interface.Screen.IBattleSwapScreen BattleSwapScreen { get; }

		IGameScreensUI initialize(params PokemonEssentials.Interface.Screen.IScreen[] screens);
	}

	public interface IGameScenesUI
	{
		PokemonEssentials.Interface.Screen.IPokemonEntryScene TextEntryScene { get; }
		PokemonEssentials.Interface.Screen.IIntroEventScene IntroScene { get; }
		PokemonEssentials.Interface.Screen.ILoadScene Load { get; }
		PokemonEssentials.Interface.Screen.ISaveScene Save { get; }
		PokemonEssentials.Interface.Screen.IOptionScene OptionScene { get; }
		PokemonEssentials.Interface.Screen.IPokemonMenuScene PauseMenuScene { get; }
		PokemonEssentials.Interface.Screen.IPartyDisplayScene Party { get; }
		PokemonEssentials.Interface.Screen.IPokemonSummaryScene Summary { get; }
		PokemonEssentials.Interface.Screen.IBagScene Bag { get; }
		PokemonEssentials.Interface.Screen.IItemStorageScene Bag_ItemStore { get; }
		PokemonEssentials.Interface.Screen.IWithdrawItemScene Bag_ItemWithdraw { get; }
		PokemonEssentials.Interface.Screen.ITossItemScene Bag_ItemToss { get; }
		//PokemonEssentials.Interface.Screen. RegionMap { get; }
		PokemonEssentials.Interface.Screen.IPokemonPhoneScene Phone { get; }
		PokemonEssentials.Interface.Screen.IPokegearScene PokeGear { get; }
		PokemonEssentials.Interface.Screen.ITrainerCardScene TrainerCard { get; }
		PokemonEssentials.Interface.Screen.IPokemonStorageScene PokemonStorageScene { get; }
		PokemonEssentials.Interface.Screen.IPokemonPokedexScene PokedexScene { get; }
		PokemonEssentials.Interface.Screen.IPokemonNestMapScene PokedexNestScene { get; }
		PokemonEssentials.Interface.Screen.IPokemonFormScene PokedexFormScene { get; }
		PokemonEssentials.Interface.Screen.IPokemonEvolutionScene EvolvingScene { get; }
		//PokemonEssentials.Interface.Screen. EggHatching { get; }
		//PokemonEssentials.Interface.Screen. Trading { get; }
		//PokemonEssentials.Interface.Screen. MoveRelearner { get; }
		PokemonEssentials.Interface.Screen.IMartScene Mart { get; }
		//PokemonEssentials.Interface.Screen. Debug { get; }
		PokemonEssentials.Interface.Screen.IRelicStoneScene RelicStone { get; } //Shadow Pokemon
		PokemonEssentials.Interface.Screen.IPurifyChamberScene PurityChamber { get; } //Shadow Pokemon


		PokemonEssentials.Interface.Screen.IPokeBattle_Scene BattleScene { get; }
		PokemonEssentials.Interface.Screen.IPokeBattleArena_Scene BattleArenaScene { get; }
		PokemonEssentials.Interface.Screen.IBattleSwapScene BattleSwapScene { get; }
		//PokemonEssentials.Interface.Screen.IBattlePalace_Scene BattlePalaceScene { get; }
		PokemonEssentials.Interface.Screen.ISafariZone_Scene BattleSafari { get; }
		PokemonEssentials.Interface.Screen.IPokeBattle_DebugScene BattleSceneDebug { get; }
		PokemonEssentials.Interface.Screen.IPokeBattle_SceneNonInteractive BattleSceneDebugNoUI { get; }
		PokemonEssentials.Interface.Screen.IPokeBattle_DebugSceneNoLogging BattleSceneDebugWithoutLog { get; }
		PokemonEssentials.Interface.Screen.IPokeBattle_DebugSceneNoGraphics BattleSceneDebugWithoutGfx { get; }

		//IGameScenesUI initialize(params PokemonEssentials.Interface.Screen.IScene[] scenes);
	}
}