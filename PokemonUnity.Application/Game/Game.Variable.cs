using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using PokemonUnity;
using PokemonUnity.Application;
using PokemonUnity.Character;
using PokemonUnity.Monster;
using PokemonUnity.Inventory;
using PokemonUnity.Saving;
using PokemonUnity.Overworld;

namespace PokemonUnity
{
	public partial class Game : PokemonEssentials.Interface.IGame//, PokemonEssentials.Interface.IEvents
	{
		public PokemonEssentials.Interface.Field.IGlobalMetadata Global { get; private set; }
		public PokemonEssentials.Interface.Field.IMapFactory MapFactory { get; private set; }
		public PokemonEssentials.Interface.Field.IMapMetadata PokemonMap { get; private set; }
		//public PokemonEssentials.Interface.Field.IMapMetadata MapData { get; private set; }
		public PokemonEssentials.Interface.Screen.IPokemonSystemOption PokemonSystem { get; private set; }
		public PokemonEssentials.Interface.Field.ITempMetadata PokemonTemp { get; set; }
		public PokemonEssentials.Interface.Field.IEncounters MapEncounterData { get; set; }
		public PokemonEssentials.Interface.Field.IEncounters PokemonEncounters { get; private set; }
		public PokemonEssentials.Interface.Screen.IPCPokemonStorage PokemonStorage { get; private set; }
		public PokemonEssentials.Interface.Screen.IBag Bag { get; private set; }
		public PokemonEssentials.Interface.ISceneMap Scene { get; set; }
		public PokemonEssentials.Interface.IGameTemp GameTemp { get; private set; }
		public PokemonEssentials.Interface.IGamePlayer GamePlayer { get; set; }
		//public PokemonEssentials.Interface.IGamePlayer Player { get; set; }
		public PokemonEssentials.Interface.PokeBattle.ITrainer Trainer { get; set; }
		public PokemonEssentials.Interface.RPGMaker.Kernal.ISystem DataSystem { get; set; }
		public PokemonEssentials.Interface.ITileset[] DataTilesets { get; set; }
		public PokemonEssentials.Interface.IGameCommonEvent[] DataCommonEvents { get; set; }
		public PokemonEssentials.Interface.IGameSystem GameSystem { get; set; }
		public PokemonEssentials.Interface.IGameSwitches GameSwitches { get; set; }
		public PokemonEssentials.Interface.IGameSelfSwitches GameSelfSwitches { get; set; }
		public PokemonEssentials.Interface.IGameVariable GameVariables { get; set; }
		public PokemonEssentials.Interface.IGameScreen GameScreen { get; set; }
		public PokemonEssentials.Interface.IGameMap GameMap { get; set; }
		public PokemonEssentials.Interface.IGameMessage GameMessage { get; set; }
		//public PokemonEssentials.Interface.IGameAudioPlay Audio { get; set; }

		//public static PokemonUnity.Interface.IFrontEnd UI { get; private set; }
		public PokemonEssentials.Interface.IChooseNumberParams ChooseNumberParams { get; set; }

		/// <summary>
		/// Singleton Instance of Game class to store current/active play state.
		/// </summary>
		public static PokemonEssentials.Interface.IGame GameData { get; set; }
		public PokemonEssentials.Interface.IAudio Audio { get; set; }
		public PokemonEssentials.Interface.IGraphics Graphics { get; set; }
		public PokemonEssentials.Interface.IInput Input { get; set; }
		public PokemonEssentials.Interface.IInterpreter Interpreter { get; set; }
		public PokemonEssentials.Interface.Screen.IGameScenesUI Scenes { get; set; }
		public PokemonEssentials.Interface.Screen.IGameScreensUI Screens { get; set; }

		//Used by map UI
		public PokemonEssentials.Interface.Field.ILocationWindow LocationWindow { get; set; }
		public PokemonEssentials.Interface.Field.IEncounterModifier EncounterModifier { get; set; }
		public static PokemonEssentials.Interface.IFileTest FileTest { get; set; }

		#region Events used by Game Interface
		//public PokemonEssentials.Interface.IEvents Events { get; private set; }
		/// <summary>
		/// Fires whenever the player moves to a new map. Event handler receives the old
		/// map ID or 0 if none.  Also fires when the first map of the game is loaded
		/// </summary>
		public event EventHandler OnMapChange;
		/// <summary>
		/// Fires whenever the map scene is regenerated and soon after the player moves
		/// to a new map.
		/// </summary>
		//event EventHandler<IOnMapSceneChangeEventArgs> OnMapSceneChange;
		public event Action<object, PokemonEssentials.Interface.EventArg.IOnMapSceneChangeEventArgs> OnMapSceneChange;
		/// <summary>
		/// Fires each frame during a map update.
		/// </summary>
		public event EventHandler OnMapUpdate;
		/// <summary>
		/// Fires whenever one map is about to change to a different one. Event handler
		/// receives the new map ID and the Game_Map object representing the new map.
		/// When the event handler is called, Game.GameData.GameMap still refers to the old map.
		/// </summary>
		public event EventHandler OnMapChanging;
		/// <summary>
		/// Fires whenever the player or another event leaves a tile.
		/// </summary>
		//event EventHandler<IOnLeaveTileEventArgs> OnLeaveTile;
		public event Action<object, PokemonEssentials.Interface.EventArg.IOnLeaveTileEventArgs> OnLeaveTile;
		/// <summary>
		/// Fires whenever the player takes a step.
		/// </summary>
		public event EventHandler OnStepTakenEvent;
		/// <summary>
		/// Fires whenever the player takes a step. The event handler may possibly move
		/// the player elsewhere.
		/// </summary>
		//event EventHandler<IOnStepTakenTransferPossibleEventArgs> OnStepTakenTransferPossible;
		public event Action<object, PokemonEssentials.Interface.EventArg.IOnStepTakenTransferPossibleEventArgs> OnStepTakenTransferPossible;
		/// <summary>
		/// Fires whenever the player or another event enters a tile.
		/// </summary>
		//event EventHandler<IOnStepTakenFieldMovementEventArgs> OnStepTakenFieldMovement;
		public event Action<object, PokemonEssentials.Interface.EventArg.IOnStepTakenFieldMovementEventArgs> OnStepTakenFieldMovement;
		/// <summary>
		/// Triggers at the start of a wild battle.  Event handlers can provide their own
		/// wild battle routines to override the default behavior.
		/// </summary>
		//event EventHandler<IOnWildBattleOverrideEventArgs> OnWildBattleOverride;
		public event Action<object, PokemonEssentials.Interface.EventArg.IOnWildBattleOverrideEventArgs> OnWildBattleOverride;
		/// <summary>
		/// Triggers whenever a wild Pokémon battle ends
		/// </summary>
		//event EventHandler<IOnWildBattleEndEventArgs> OnWildBattleEnd;
		public event Action<object, PokemonEssentials.Interface.EventArg.IOnWildBattleEndEventArgs> OnWildBattleEnd;
		/// <summary>
		/// Triggers whenever a wild Pokémon is created
		/// </summary>
		//event EventHandler<IOnWildPokemonCreateEventArgs> OnWildPokemonCreate;
		public event Action<object, PokemonEssentials.Interface.EventArg.IOnWildPokemonCreateEventArgs> OnWildPokemonCreate;
		/// <summary>
		/// Triggers whenever an NPC trainer's Pokémon party is loaded
		/// </summary>
		//event EventHandler<IOnTrainerPartyLoadEventArgs> OnTrainerPartyLoad;
		public event Action<object, PokemonEssentials.Interface.EventArg.IOnTrainerPartyLoadEventArgs> OnTrainerPartyLoad;
		/// <summary>
		/// Fires whenever a spriteset is created.
		/// </summary>
		//event EventHandler<IOnSpritesetCreateEventArgs> OnSpritesetCreate;
		public event Action<object, PokemonEssentials.Interface.EventArg.IOnSpritesetCreateEventArgs> OnSpritesetCreateEvent;
		public event EventHandler OnStartBattle;
		//event EventHandler OnEndBattle;
		public event Action<object, PokemonEssentials.Interface.EventArg.IOnEndBattleEventArgs> OnEndBattle;
		/// <summary>
		/// Fires whenever a map is created. Event handler receives two parameters: the
		/// map (RPG.Map) and the tileset (RPG.Tileset)
		/// </summary>
		public event EventHandler OnMapCreate;
		/// <summary>
		/// Triggers when the player presses the Action button on the map.
		/// </summary>
		public event EventHandler OnAction;
		public event EventHandler<PokemonUnity.EventArg.OnLoadLevelEventArgs> OnLoadLevel;
		event Action<object, PokemonEssentials.Interface.EventArg.IOnLoadLevelEventArgs> PokemonEssentials.Interface.IGame.OnLoadLevel { add { GameData.OnLoadLevel += value;  } remove { GameData.OnLoadLevel -= value; } }
		#endregion
	}
}