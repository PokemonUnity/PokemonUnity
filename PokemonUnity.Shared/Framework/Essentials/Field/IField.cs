using System;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Overworld;
using PokemonUnity.UX;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.Field.EventArg;

namespace PokemonEssentials.Interface.Field
{
	/// <summary>
	/// This module stores encounter-modifying events that can happen during the game.
	/// A procedure can subscribe to an event by adding itself to the event.  It will
	/// then be called whenever the event occurs.
	/// </summary>
	public interface IEncounterModifier
	{
		//List<> procs { get; }
		//List<> procsEnd { get; }

		//void register(Action<Encounter> p);

		//void registerEncounterEnd(Action<Encounter> p);

		//Encounter trigger(encounter);

		void triggerEncounterEnd();
	}

	/// <summary>
	/// This module stores events that can happen during the game.  A procedure can
	/// subscribe to an event by adding itself to the event.  It will then be called
	/// whenever the event occurs.
	/// </summary>
	public interface IEvents
	{
		#region EventHandlers
		/// <summary>
		/// Fires whenever the player moves to a new map. Event handler receives the old
		/// map ID or 0 if none.  Also fires when the first map of the game is loaded
		/// </summary>
		event EventHandler OnMapChange;
		/// <summary>
		/// Fires whenever the map scene is regenerated and soon after the player moves
		/// to a new map.
		/// </summary>
		event EventHandler<OnMapSceneChangeEventArgs> OnMapSceneChange;
		/// <summary>
		/// Fires each frame during a map update.
		/// </summary>
		event EventHandler OnMapUpdate;
		/// <summary>
		/// Fires whenever one map is about to change to a different one. Event handler
		/// receives the new map ID and the Game_Map object representing the new map.
		/// When the event handler is called, Game.GameData.GameMap still refers to the old map.
		/// </summary>
		event EventHandler OnMapChanging;
		/// <summary>
		/// Fires whenever the player or another event leaves a tile.
		/// </summary>
		event EventHandler<OnLeaveTileEventArgs> OnLeaveTile;
		/// <summary>
		/// Fires whenever the player takes a step.
		/// </summary>
		event EventHandler OnStepTaken;
		/// <summary>
		/// Fires whenever the player takes a step. The event handler may possibly move
		/// the player elsewhere.
		/// </summary>
		event EventHandler<OnStepTakenTransferPossibleEventArgs> OnStepTakenTransferPossible;
		/// <summary>
		/// Fires whenever the player or another event enters a tile.
		/// </summary>
		event EventHandler<OnStepTakenFieldMovementEventArgs> OnStepTakenFieldMovement;
		/// <summary>
		/// Triggers at the start of a wild battle.  Event handlers can provide their own
		/// wild battle routines to override the default behavior.
		/// </summary>
		event EventHandler OnWildBattleOverride;
		/// <summary>
		/// Triggers whenever a wild Pokémon battle ends
		/// </summary>
		event EventHandler<OnWildBattleEndEventArgs> OnWildBattleEnd;
		/// <summary>
		/// Triggers whenever a wild Pokémon is created
		/// </summary>
		event EventHandler<OnWildPokemonCreateEventArgs> OnWildPokemonCreate;
		/// <summary>
		/// Triggers whenever an NPC trainer's Pokémon party is loaded
		/// </summary>
		event EventHandler<OnTrainerPartyLoadEventArgs> OnTrainerPartyLoad;
		/// <summary>
		/// Fires whenever a spriteset is created.
		/// </summary>
		event EventHandler<OnSpritesetCreateEventArgs> OnSpritesetCreate;
		event EventHandler OnStartBattle;
		event EventHandler OnEndBattle;
		/// <summary>
		/// Fires whenever a map is created. Event handler receives two parameters: the
		/// map (RPG.Map) and the tileset (RPG.Tileset)
		/// </summary>
		event EventHandler OnMapCreate;
		/// <summary>
		/// Triggers when the player presses the Action button on the map.
		/// </summary>
		event EventHandler OnAction;
		#endregion

		#region Event Sender / Raise Events
		void OnMapCreateTrigger();
		void OnMapChangeTrigger();
		void OnMapChangingTrigger();
		/// <summary>
		/// Parameters:
		/// e[0] - Event that just left the tile.
		/// e[1] - Map ID where the tile is located (not necessarily
		///  the current map). Use "Game.GameData.MapFactory.getMap(e[1])" to
		///  get the Game_Map object corresponding to that map.
		/// e[2] - X-coordinate of the tile
		/// e[3] - Y-coordinate of the tile
		/// </summary>
		//void OnLeaveTileTrigger(object @event, int mapId, IVector tile);
		void OnLeaveTileTrigger(object @event, int mapId, float x, float y, float z);
		/// <summary>
		/// Parameters:
		/// e[0] - Event that just entered a tile.
		/// </summary>
		void OnStepTakenFieldMovementTrigger();
		/// <summary>
		/// Parameters:
		/// e[0] = Array that contains a single boolean value.
		/// If an event handler moves the player to a new map, it should set this value
		/// to true. Other event handlers should check this parameter's value.
		/// </summary>
		void OnStepTakenTransferPossibleTrigger();
		//void OnWildBattleOverrideTrigger(null,Pokemons species,int level,bool handled);
		/// <summary>
		/// Parameters: 
		/// e[0] - Pokémon species
		/// e[1] - Pokémon level
		/// e[2] - Battle result (1-win, 2-loss, 3-escaped, 4-caught, 5-draw)
		/// </summary>
		void OnWildBattleEndTrigger();
		/// <summary>
		/// Parameters: 
		/// e[0] - Pokémon being created
		/// </summary>
		void OnWildPokemonCreateTrigger();
		/// <summary>
		/// Parameters: 
		/// e[0] - Trainer
		/// e[1] - Items possessed by the trainer
		/// e[2] - Party
		/// </summary>
		void OnTrainerPartyLoadTrigger();
		/// <summary>
		/// Parameters:
		/// e[0] = Scene_Map object.
		/// e[1] = Whether the player just moved to a new map (either true or false). If
		///   false, some other code had called <see cref="Game.GameData.Scene.createSpritesets"/>
		///   to regenerate the map scene without transferring the player elsewhere
		/// </summary>
		void OnMapSceneChangeTrigger();
		/// <summary>
		/// Parameters:
		/// e[0] = Spriteset being created
		/// e[1] = Viewport used for tilemap and characters
		/// e[0].map = Map associated with the spriteset (not necessarily the current map).
		/// </summary>
		void OnSpritesetCreateTrigger();
		#endregion
	}

	namespace EventArg
	{
		#region EventArgs
		public interface IEventArgs
		{
			int Id { get; }
		}
		public class OnMapCreateEventArgs : EventArgs, IEventArgs
		{
			public readonly int EventId = typeof(OnMapCreateEventArgs).GetHashCode();

			public int Id { get { return EventId; } }
			//public int Id { get { return Pokemon.GetHashCode(); } } //EventId;
			public int Map { get; set; }
			public int Tileset { get; set; }
		}
		public class OnMapChangeEventArgs : EventArgs
		{
			public readonly int EventId = typeof(OnMapChangeEventArgs).GetHashCode();

			//public int Id { get { return EventId; } }
			public int Id { get { return MapId.GetHashCode(); } } //EventId;
			public int MapId { get; set; }
		}
		public class OnMapChangingEventArgs : EventArgs
		{
			public readonly int EventId = typeof(OnMapChangingEventArgs).GetHashCode();

			//public int Id { get { return EventId; } }
			public int Id { get { return MapId.GetHashCode(); } } //EventId;
			public int MapId { get; set; }
			//public Game_Map GameMap { get; set; }
		}
		/// <summary>
		/// Parameters:
		/// e[0] - Event that just left the tile.
		/// e[1] - Map ID where the tile is located (not necessarily
		///  the current map). Use "Game.GameData.MapFactory.getMap(e[1])" to
		///  get the Game_Map object corresponding to that map.
		/// e[2] - X-coordinate of the tile
		/// e[3] - Y-coordinate of the tile
		/// </summary>
		public class OnLeaveTileEventArgs : EventArgs
		{
			public readonly int EventId = typeof(OnLeaveTileEventArgs).GetHashCode();

			public int Id { get { return EventId; } }
			/// <summary>
			/// Event that just left the tile.
			/// </summary>
			//public Avatar.GameEvent Event { get; set; }
			/// <summary>
			/// Map ID where the tile is located (not necessarily
			///  the current map). Use "Game.GameData.MapFactory.getMap(e[1])" to
			///  get the Game_Map object corresponding to that map.
			/// </summary>
			public int MapId { get; set; }
			/// <summary>
			/// X-coordinate of the tile
			/// </summary>
			public float X { get; set; }
			/// <summary>
			/// Y-coordinate of the tile
			/// </summary>
			public float Y { get; set; }
		}
		/// <summary>
		/// Parameters:
		/// e[0] - Event that just entered a tile.
		/// </summary>
		public class OnStepTakenFieldMovementEventArgs : EventArgs
		{
			public readonly int EventId = typeof(OnStepTakenFieldMovementEventArgs).GetHashCode();

			public int Id { get { return EventId; } }
			/// <summary>
			/// Event that just entered a tile.
			/// </summary>
			public int Index { get; set; }
		}
		/// <summary>
		/// Parameters:
		/// e[0] = Array that contains a single boolean value.
		/// If an event handler moves the player to a new map, it should set this value
		/// to true. Other event handlers should check this parameter's value.
		/// </summary>
		public class OnStepTakenTransferPossibleEventArgs : EventArgs
		{
			public readonly int EventId = typeof(OnStepTakenTransferPossibleEventArgs).GetHashCode();

			public int Id { get { return EventId; } }
			/// <summary>
			/// Array that contains a single boolean value.
			/// </summary>
			public bool Index { get; set; }
		}
		/// <summary>
		/// Parameters: 
		/// e[0] - Pokémon species
		/// e[1] - Pokémon level
		/// e[2] - Battle result (1-win, 2-loss, 3-escaped, 4-caught, 5-draw)
		/// </summary>
		public class OnWildBattleEndEventArgs : EventArgs
		{
			public readonly int EventId = typeof(OnWildBattleEndEventArgs).GetHashCode();

			public int Id { get { return EventId; } }
			public Pokemons Species { get; set; }
			public int Level { get; set; }
			/// <summary>
			/// Battle result (1-win, 2-loss, 3-escaped, 4-caught, 5-draw)
			/// </summary>
			public int Result { get; set; }
		}
		/// <summary>
		/// Parameters: 
		/// e[0] - Pokémon being created
		/// </summary>
		public class OnWildPokemonCreateEventArgs : EventArgs
		{
			public readonly int EventId = typeof(OnWildPokemonCreateEventArgs).GetHashCode();

			public int Id { get; }
			/// <summary>
			/// Pokémon being created
			/// </summary>
			public IPokemon Pokemon { get; set; }
		}
		/// <summary>
		/// Parameters: 
		/// e[0] - Trainer
		/// e[1] - Items possessed by the trainer
		/// e[2] - Party
		/// </summary>
		public class OnTrainerPartyLoadEventArgs : EventArgs
		{
			public readonly int EventId = typeof(OnTrainerPartyLoadEventArgs).GetHashCode();

			public int Id { get; }
			public int Trainer { get; set; }
			/// <summary>
			/// Items possessed by the trainer
			/// </summary>
			public Items[] Items { get; set; }
			public IPokemon[] Party { get; set; }
		}
		/// <summary>
		/// Parameters:
		/// e[0] = Scene_Map object.
		/// e[1] = Whether the player just moved to a new map (either true or false). If
		///   false, some other code had called <see cref="Game.GameData.Scene.createSpritesets"/>
		///   to regenerate the map scene without transferring the player elsewhere
		/// </summary>
		public class OnMapSceneChangeEventArgs : EventArgs
		{
			public readonly int EventId = typeof(OnMapSceneChangeEventArgs).GetHashCode();

			public int Id { get; }
			/// <summary>
			/// Scene_Map object.
			/// </summary>
			public int Object { get; set; }
			/// <summary>
			/// Whether the player just moved to a new map (either true or false).
			/// </summary>
			public bool NewMap { get; set; }
		}
		/// <summary>
		/// Parameters:
		/// e[0] = Spriteset being created
		/// e[1] = Viewport used for tilemap and characters
		/// e[0].map = Map associated with the spriteset (not necessarily the current map).
		/// </summary>
		public class OnSpritesetCreateEventArgs : EventArgs
		{
			public readonly int EventId = typeof(OnSpritesetCreateEventArgs).GetHashCode();

			public int Id { get; }
			/// <summary>
			/// Spriteset being created
			/// </summary>
			public int SpritesetId { get; set; }
			/// <summary>
			/// Viewport used for tilemap and characters
			/// </summary>
			public int Viewport { get; set; }
			/// <summary>
			/// Map associated with the spriteset (not necessarily the current map).
			/// </summary>
			public int MapId { get; set; }
		}
		#endregion
	}

	public interface IGameField
	{
		IPokeBattle_Scene pbNewBattleScene();

		IEnumerator pbSceneStandby();

		IEnumerator pbBattleAnimation(IAudioBGM bgm = null, int trainerid = -1, string trainername = "");

		// Alias and use this method if you want to add a custom battle intro animation
		// e.g. variants of the Vs. animation.
		// Note that Game.GameData.GameTemp.background_bitmap contains an image of the current game
		// screen.
		// When the custom animation has finished, the screen should have faded to black
		// somehow.
		bool pbBattleAnimationOverride(IViewport viewport, int trainerid = -1, string trainername = "");

		void pbPrepareBattle(IBattle battle);

		Environments pbGetEnvironment();

		IPokemon pbGenerateWildPokemon(Pokemons species, int level, bool isroamer = false);

		PokemonUnity.Combat.BattleResults pbWildBattle(Pokemons species, int level, int? variable = null, bool canescape = true, bool canlose = false);

		PokemonUnity.Combat.BattleResults pbDoubleWildBattle(Pokemons species1, int level1, Pokemons species2, int level2, int? variable = null, bool canescape = true, bool canlose = false);

		void pbCheckAllFainted();

		void pbEvolutionCheck(int currentlevels);

		Items[] pbDynamicItemList(params Items[] args);

		// Runs the Pickup event after a battle if a Pokemon has the ability Pickup.
		void pbPickup(IPokemon pokemon);

		bool pbEncounter(EncounterTypes enctype);

		//void pbOnSpritesetCreate(spriteset, IViewport viewport);

		#region Field movement
		bool pbLedge(float xOffset, float yOffset);

		void pbSlideOnIce(IEntity _event = null);

		void pbBattleOnStepTaken();

		void pbOnStepTaken(bool eventTriggered);

		// This method causes a lot of lag when the game is encrypted
		//ICharset pbGetPlayerCharset(meta, charset, trainer= null);

		void pbUpdateVehicle();

		/// <summary>
		/// </summary>
		/// <param name="destination">Map location id</param>
		void pbCancelVehicles(int? destination = null);

		bool pbCanUseBike(int mapid);

		void pbMountBike();

		void pbDismountBike();

		void pbSetPokemonCenter();
		#endregion

		#region Fishing
		void pbFishingBegin();

		void pbFishingEnd();

		bool pbFishing(bool hasencounter, int rodtype = 1);

		bool pbWaitForInput(IWindow msgwindow, string message, int frames);

		bool pbWaitMessage(IWindow msgwindow, int time);
		#endregion

		#region Moving between maps
		void pbStartOver(bool gameover = false);

		void pbCaveEntranceEx(bool exiting);

		void pbCaveEntrance();

		void pbCaveExit();

		void pbSetEscapePoint();

		void pbEraseEscapePoint();
		#endregion

		#region Partner trainer
		void pbRegisterPartner(TrainerTypes trainerid, string trainername, int partyid = 0);

		void pbDeregisterPartner();
		#endregion

		#region Constant checks
		/// <summary>
		/// Returns whether the Poké Center should explain Pokérus to the player, if a
		/// healed Pokémon has it.
		/// </summary>
		/// <returns></returns>
		bool pbPokerus();
		#endregion

		bool pbBatteryLow();

		#region Audio playing
		void pbCueBGM(IAudioBGM bgm, int seconds, int? volume = null, int? pitch = null);

		void pbAutoplayOnTransition();

		void pbAutoplayOnSave();
		#endregion

		#region Voice recorder
		/// <summary>
		/// </summary>
		/// BGM audio file?
		IAudioObject pbRecord(string text, float maxtime = 30.0f);

		bool pbRxdataExists(string file);
		#endregion

		#region Gaining items
		bool pbItemBall(Items item, int quantity = 1);

		bool pbReceiveItem(Items item, int quantity = 1);

		void pbUseKeyItem();
		#endregion

		#region Bridges
		void pbBridgeOn(int height = 2);

		void pbBridgeOff();
		#endregion

		#region Event locations, terrain tags
		bool pbEventFacesPlayer(IEntity _event, IGamePlayer player, int distance);

		bool pbEventCanReachPlayer(IEntity _event, IGamePlayer player, int distance);

		ITilePosition pbFacingTileRegular(int? direction = null, IEntity _event = null);

		ITilePosition pbFacingTile(int? direction = null, IEntity _event = null);

		bool pbFacingEachOther(IEntity event1, IEntity event2);

		Terrains pbGetTerrainTag(IEntity _event = null, bool countBridge = false);

		Terrains pbFacingTerrainTag(IEntity _event = null, int? dir = null);
		#endregion

		#region Event movement
		IEnumerator pbTurnTowardEvent(IEntity _event, IEntity otherEvent);

		IEnumerator pbMoveTowardPlayer(IEntity _event);

		bool pbJumpToward(int dist = 1, bool playSound = false, bool cancelSurf = false);

		void pbWait(int numframes);
		#endregion

		//MoveRoute pbMoveRoute(IEntity _event, string[] commands, bool waitComplete= false);

		#region Screen effects
		void pbToneChangeAll(ITone tone, int duration);

		void pbShake(int power, int speed, int frames);

		void pbFlash(IColor color, int frames);

		void pbScrollMap(int direction, int distance, int speed);
		#endregion
	}

	public interface IPokemonTempField
	{
		#region 
		//EncounterMethods encounterType { get; set; }
		int evolutionLevels { get; set; }
		#endregion

		#region 
		bool batterywarning { get; set; }
		IAudioBGM cueBGM { get; set; }
		int cueFrames { get; set; }
		#endregion
	}

	// ===============================================================================
	// Scene_Map and Spriteset_Map
	// ===============================================================================
	//public interface Scene_Map
	//{
	//	void createSingleSpriteset(map);
	//}

	//public interface Spriteset_Map
	//{
	//	public void getAnimations();
	//
	//	public void restoreAnimations(anims);
	//}

	/*public static partial class PBMoveRoute
	{
		Down               = 1;
	  Left               = 2;
	  Right              = 3;
	  Up                 = 4;
	  LowerLeft          = 5;
	  LowerRight         = 6;
	  UpperLeft          = 7;
	  UpperRight         = 8;
	  Random             = 9;
	  TowardPlayer       = 10;
	  AwayFromPlayer     = 11;
	  Forward            = 12
	  Backward           = 13;
	  Jump               = 14; // xoffset, yoffset
	  Wait               = 15; // frames
	  TurnDown           = 16;
	  TurnLeft           = 17;
	  TurnRight          = 18;
	  TurnUp             = 19;
	  TurnRight90        = 20;
	  TurnLeft90         = 21;
	  Turn180            = 22;
	  TurnRightOrLeft90  = 23;
	  TurnRandom         = 24;
	  TurnTowardPlayer   = 25;
	  TurnAwayFromPlayer = 26;
	  SwitchOn           = 27; // 1 param
	  SwitchOff          = 28; // 1 param
	  ChangeSpeed        = 29; // 1 param
	  ChangeFreq         = 30; // 1 param
	  WalkAnimeOn        = 31;
	  WalkAnimeOff       = 32;
	  StepAnimeOn        = 33;
	  StepAnimeOff       = 34;
	  DirectionFixOn     = 35;
	  DirectionFixOff    = 36;
	  ThroughOn          = 37;
	  ThroughOff         = 38;
	  AlwaysOnTopOn      = 39;
	  AlwaysOnTopOff     = 40;
	  Graphic            = 41; // Name, hue, direction, pattern
	  Opacity            = 42; // 1 param
	  Blending           = 43; // 1 param
	  PlaySE             = 44; // 1 param
	  Script             = 45; // 1 param
	  ScriptAsync        = 101; // 1 param
	}*/

	// ===============================================================================
	// Events
	// ===============================================================================
	public interface IGameEventField
	{
		bool cooledDown(int seconds);

		bool cooledDownDays(int days);
	}

	public interface IInterpreterFieldMixinField
	{
		//  Used in boulder events. Allows an event to be pushed. To be used in
		//  a script event command.
		void pbPushThisEvent();

		void pbPushThisBoulder();

		void pbHeadbutt();

		void pbTrainerIntro(TrainerTypes symbol);

		void pbTrainerEnd();

		object pbParams { get; }

		void pbGetPokemon(Pokemons id);

		void pbSetEventTime(params int[] arg);

		object getVariable(params int[] arg);

		void setVariable(params int[] arg);

		bool tsOff(string c);

		bool tsOn(string c);

		//alias isTempSwitchOn? tsOn?;
		//alias isTempSwitchOff? tsOff?;

		void setTempSwitchOn(string c);

		void setTempSwitchOff(string c);

		// Must use this approach to share the methods because the methods already
		// defined in a class override those defined in an included module
		/*CustomEventCommands =<< _END_;

		bool command_352();

		bool command_125();

		bool command_132();

		bool command_133();

		bool command_353();

		bool command_314();

		_END_;*/
	}

	public interface ITilePosition
	{
		int MapId { get; }
		float x { get; }
		float y { get; }
		float z { get; }
		//IVector Vector { get; }
	}
}