using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Combat;
using PokemonUnity.Monster;
using PokemonUnity.Utility;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;

namespace PokemonEssentials.Interface
{
	//ToDo: Too many variables are named with `pokemon` and have nothing to do with an individual pokemon record, and only pertains to pokemon in general sense of the application's game logic...
	public interface IGame //: IGameBerryPlants, IGameDungeon, IGameFactory, IGameField, IGameHiddenMoves, IGameItem, IGameItemEffect, IGameOrgBattle, IGamePokeball, IGameResizer, IGameSafari, IGameTime, IGameMessage, IGameAudioPlay, IGameMetadataMisc, IGameUtility
	{
		PokemonEssentials.Interface.Field.IGlobalMetadata Global			{ get; }
		PokemonEssentials.Interface.Field.IMapFactory MapFactory			{ get; }
		PokemonEssentials.Interface.Field.IMapMetadata PokemonMap			{ get; }			//ToDo: Rename to `MapData`
		//PokemonEssentials.Interface.Field.IMapMetadata MapData				{ get; }		//this is Data about the Current/Active Map the player is on
		PokemonEssentials.Interface.Screen.IPokemonSystemOption PokemonSystem { get; }
		PokemonEssentials.Interface.Field.ITempMetadata PokemonTemp			{ get; set; }		//ToDo: Rename to `TempData`
		//PokemonEssentials.Interface.Field.IEncounters MapEncounterData		{ get; set; }
		PokemonEssentials.Interface.Field.IEncounters PokemonEncounters		{ get; }			//ToDo: Rename to `MapEncounterData`
		PokemonEssentials.Interface.Screen.IPCPokemonStorage PokemonStorage	{ get; }
		PokemonEssentials.Interface.Screen.IBag Bag							{ get; }
		PokemonEssentials.Interface.ISceneMap Scene							{ get; set; }
		PokemonEssentials.Interface.IGameTemp GameTemp						{ get; }
		PokemonEssentials.Interface.IGamePlayer GamePlayer					{ get; set; }		//ToDo: Rename to `Player` -> `Avatar`
		//PokemonEssentials.Interface.IGamePlayer Player						{ get; set; }		//ToDo: Rename to `Avatar`?
		PokemonEssentials.Interface.PokeBattle.ITrainer Trainer				{ get; set; }
		PokemonEssentials.Interface.RPGMaker.Kernal.ISystem DataSystem		{ get; set; }
		PokemonEssentials.Interface.ITileset[] DataTilesets					{ get; set; }
		PokemonEssentials.Interface.IGameCommonEvent[] DataCommonEvents		{ get; set; }
		PokemonEssentials.Interface.IGameSystem GameSystem					{ get; set; }
		PokemonEssentials.Interface.IGameSwitches GameSwitches				{ get; set; }
		PokemonEssentials.Interface.IGameSelfSwitches GameSelfSwitches		{ get; set; }
		PokemonEssentials.Interface.IGameVariable GameVariables				{ get; set; }
		PokemonEssentials.Interface.IGameMap GameMap						{ get; set; }

		#region UI Components
		PokemonEssentials.Interface.IGameScreen GameScreen					{ get; set; }		//ToDo: Rename to `Screen`, maybe include `Component` or `Element` in name?
		PokemonEssentials.Interface.IGameMessage GameMessage				{ get; set; }
		PokemonEssentials.Interface.IChooseNumberParams ChooseNumberParams	{ get; set; }
		//PokemonEssentials.Interface.IGameAudioPlay Audio					{ get; set; }
		PokemonEssentials.Interface.IAudio Audio							{ get; set; }
		PokemonEssentials.Interface.IGraphics Graphics						{ get; set; }
		PokemonEssentials.Interface.IInput Input							{ get; set; }
		PokemonEssentials.Interface.IInterpreter Interpreter				{ get; set; }
		PokemonEssentials.Interface.Screen.IGameScenesUI Scenes				{ get; set; }
		PokemonEssentials.Interface.Screen.IGameScreensUI Screens			{ get; set; }
		/// <summary>
		/// UI component for sign-post, when entering new map zone
		/// </summary>
		PokemonEssentials.Interface.Field.ILocationWindow LocationWindow	{ get; set; }
		PokemonEssentials.Interface.Field.IEncounterModifier EncounterModifier	{ get; set; }
		//PokemonEssentials.Interface.IFileTest FileTest						{ get; set; }
		#endregion
		event Action<object, PokemonEssentials.Interface.EventArg.IOnLoadLevelEventArgs> OnLoadLevel;
	}
	//public interface IGlobalMetadata : Field.IGlobalMetadata, IGlobalMetadataDependantEvents, IGlobalMetadataPokeRadar, IGlobalMetadataRoaming { }
	//public interface ITempMetadata : Field.ITempMetadata, ITempMetadataBerryPlants, ITempMetadataDependantEvents, ITempMetadataField, ITempMetadataPokeRadar, ITempMetadataRoaming, ITempMetadataPokemonShadow { }
}