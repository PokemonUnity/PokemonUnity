using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity.Monster;
using PokemonUnity.Utility;
using PokemonEssentials.Interface.EventArg;

namespace PokemonUnity
{
	/// <summary>
	/// This module stores events that can happen during the game.  A procedure can
	/// subscribe to an event by adding itself to the event.  It will then be called
	/// whenever the event occurs.
	/// </summary>
	public static partial class Events
	{
		#region EventHandlers
		//public event EventHandler<OnLevelUpEventArgs> OnLevelUp;
		//public event EventHandler<OnBattlePhaseEventArgs> OnBattlePhase;
		//public event EventHandler<OnHPChangedEventArgs> OnHPChanged;
		//public event EventHandler<OnFaintedEventArgs> OnFainted;
		//public event EventHandler<OnDisplayEventArgs> OnDisplay;
		/// <summary>
		/// Fires whenever the player moves to a new map. Event handler receives the old
		/// map ID or 0 if none.  Also fires when the first map of the game is loaded
		/// </summary>
		public static event EventHandler OnMapChange;
		/// <summary>
		/// Fires whenever the map scene is regenerated and soon after the player moves
		/// to a new map.
		/// </summary>
		public static event EventHandler<OnMapSceneChangeEventArgs> OnMapSceneChange;
		/// <summary>
		/// Fires each frame during a map update.
		/// </summary>
		public static event EventHandler OnMapUpdate;
		/// <summary>
		/// Fires whenever one map is about to change to a different one. Event handler
		/// receives the new map ID and the Game_Map object representing the new map.
		/// When the event handler is called, Game.GameData.GameMap still refers to the old map.
		/// </summary>
		public static event EventHandler OnMapChanging;
		/// <summary>
		/// Fires whenever the player or another event leaves a tile.
		/// </summary>
		public static event EventHandler<OnLeaveTileEventArgs> OnLeaveTile;
		/// <summary>
		/// Fires whenever the player takes a step.
		/// </summary>
		public static event EventHandler OnStepTaken;
		/// <summary>
		/// Fires whenever the player takes a step. The event handler may possibly move
		/// the player elsewhere.
		/// </summary>
		public static event EventHandler<OnStepTakenTransferPossibleEventArgs> OnStepTakenTransferPossible;
		/// <summary>
		/// Fires whenever the player or another event enters a tile.
		/// </summary>
		public static event EventHandler<OnStepTakenFieldMovementEventArgs> OnStepTakenFieldMovement;
		/// <summary>
		/// Triggers at the start of a wild battle.  Event handlers can provide their own
		/// wild battle routines to override the default behavior.
		/// </summary>
		public static event EventHandler<OnWildBattleOverrideEventArgs> OnWildBattleOverride;
		/// <summary>
		/// Triggers whenever a wild Pokémon battle ends
		/// </summary>
		public static event EventHandler<OnWildBattleEndEventArgs> OnWildBattleEnd;
		/// <summary>
		/// Triggers whenever a wild Pokémon is created
		/// </summary>
		public static event EventHandler<OnWildPokemonCreateEventArgs> OnWildPokemonCreate;
		/// <summary>
		/// Triggers whenever an NPC trainer's Pokémon party is loaded
		/// </summary>
		public static event EventHandler<OnTrainerPartyLoadEventArgs> OnTrainerPartyLoad;
		/// <summary>
		/// Fires whenever a spriteset is created.
		/// </summary>
		public static event EventHandler<OnSpritesetCreateEventArgs> OnSpritesetCreate;
		public static event EventHandler OnStartBattle;
		public static event EventHandler OnEndBattle;
		/// <summary>
		/// Fires whenever a map is created. Event handler receives two parameters: the
		/// map (RPG.Map) and the tileset (RPG.Tileset)
		/// </summary>
		public static event EventHandler OnMapCreate;
		/// <summary>
		/// Triggers when the player presses the Action button on the map.
		/// </summary>
		public static event EventHandler OnAction;
		#endregion

		#region EventArgs
		/*public class OnMapCreateEventArgs : EventArgs
		{
			public static readonly int EventId = typeof(OnMapCreateEventArgs).GetHashCode();

			public int Id { get { return EventId; } }
			//public int Id { get { return Pokemon.GetHashCode(); } } //EventId;
			public int Map { get; set; }
			public int Tileset { get; set; }
		}
		public class OnMapChangeEventArgs : EventArgs
		{
			public static readonly int EventId = typeof(OnMapChangeEventArgs).GetHashCode();

			//public int Id { get { return EventId; } }
			public int Id { get { return MapId.GetHashCode(); } } //EventId;
			public int MapId { get; set; }
		}
		public class OnMapChangingEventArgs : EventArgs
		{
			public static readonly int EventId = typeof(OnMapChangingEventArgs).GetHashCode();

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
			public static readonly int EventId = typeof(OnLeaveTileEventArgs).GetHashCode();

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
			public static readonly int EventId = typeof(OnStepTakenFieldMovementEventArgs).GetHashCode();

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
			public static readonly int EventId = typeof(OnStepTakenTransferPossibleEventArgs).GetHashCode();

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
		public class OnWildBattleOverrideEventArgs : EventArgs
		{
			public static readonly int EventId = typeof(OnWildBattleOverrideEventArgs).GetHashCode();

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
		/// e[0] - Pokémon species
		/// e[1] - Pokémon level
		/// e[2] - Battle result (1-win, 2-loss, 3-escaped, 4-caught, 5-draw)
		/// </summary>
		public class OnWildBattleEndEventArgs : EventArgs
		{
			public static readonly int EventId = typeof(OnWildBattleEndEventArgs).GetHashCode();

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
			public static readonly int EventId = typeof(OnWildPokemonCreateEventArgs).GetHashCode();

			public int Id { get; }
			/// <summary>
			/// Pokémon being created
			/// </summary>
			public Monster.Pokemon Pokemon { get; set; }
		}
		/// <summary>
		/// Parameters: 
		/// e[0] - Trainer
		/// e[1] - Items possessed by the trainer
		/// e[2] - Party
		/// </summary>
		public class OnTrainerPartyLoadEventArgs : EventArgs
		{
			public static readonly int EventId = typeof(OnTrainerPartyLoadEventArgs).GetHashCode();

			public int Id { get; }
			public int Trainer { get; set; }
			/// <summary>
			/// Items possessed by the trainer
			/// </summary>
			public Items[] Items { get; set; }
			public Monster.Pokemon[] Party { get; set; }
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
			public static readonly int EventId = typeof(OnMapSceneChangeEventArgs).GetHashCode();

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
			public static readonly int EventId = typeof(OnSpritesetCreateEventArgs).GetHashCode();

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
		}*/
		#endregion

		#region Event Sender / Raise Events
		public static void OnMapCreateTrigger()
		{
			//OnMapCreateEventArgs e = new OnMapCreateEventArgs();
			if (OnMapCreate != null) OnMapCreate.Invoke(null, new OnMapCreateEventArgs());
		}
		public static void OnMapChangeTrigger()
		{
			OnMapChangeEventArgs e = new OnMapChangeEventArgs(); 
			if(OnMapChange != null) OnMapChange.Invoke(null, e);
		}
		public static void OnMapChangingTrigger()
		{
			OnMapChangingEventArgs e = new OnMapChangingEventArgs(); 
			if(OnMapChanging != null) OnMapChanging.Invoke(null, e);
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
		//public static void OnLeaveTileTrigger(object @event, int mapId, float x, float y, float z) { OnLeaveTileTrigger(@event, mapId, new Vector(x, y, z)); }
		public static void OnLeaveTileTrigger(object @event, int mapId, IVector tile)
		{
			OnLeaveTileEventArgs e = new OnLeaveTileEventArgs();
			//EventHandler<OnLeaveTileEventArgs> handler = OnLeaveTile;
			//if (handler != null) handler.Invoke(null, e);
			if (OnLeaveTile != null) OnLeaveTile.Invoke(null, e);
		}
		/// <summary>
		/// Parameters:
		/// e[0] - Event that just entered a tile.
		/// </summary>
		public static void OnStepTakenFieldMovementTrigger()
		{
			OnStepTakenFieldMovementEventArgs e = new OnStepTakenFieldMovementEventArgs(); 
			if(OnStepTakenFieldMovement != null) OnStepTakenFieldMovement.Invoke(null, e);
		}
		/// <summary>
		/// Parameters:
		/// e[0] = Array that contains a single boolean value.
		/// If an event handler moves the player to a new map, it should set this value
		/// to true. Other event handlers should check this parameter's value.
		/// </summary>
		public static void OnStepTakenTransferPossibleTrigger()
		{
			OnStepTakenTransferPossibleEventArgs e = new OnStepTakenTransferPossibleEventArgs(); 
			if(OnStepTakenTransferPossible != null) OnStepTakenTransferPossible.Invoke(null, e);
		}
		/// <summary>
		/// Parameters: 
		/// e[0] - Pokémon species
		/// e[1] - Pokémon level
		/// e[2] - Battle result (1-win, 2-loss, 3-escaped, 4-caught, 5-draw)
		/// </summary>
		//public static void OnWildBattleOverrideTrigger(Pokemons species,int level,BattleResults handled)//object @event,
		//{
		//	OnWildBattleOverrideEventArgs e = new OnWildBattleOverrideEventArgs();
		//	if(OnWildBattleOverride != null) OnWildBattleOverride.Invoke(null, e);
		//}
		/// <summary>
		/// Parameters: 
		/// e[0] - Pokémon species
		/// e[1] - Pokémon level
		/// e[2] - Battle result (1-win, 2-loss, 3-escaped, 4-caught, 5-draw)
		/// </summary>
		public static void OnWildBattleEndTrigger(object @event, Pokemons species, int level, BattleResults decision)
		{
			OnWildBattleEndEventArgs e = new OnWildBattleEndEventArgs()
			{
				Level = level,
				Species = species,
				Result = decision
			};
			if(OnWildBattleEnd != null) OnWildBattleEnd.Invoke(@event, e);
		}
		/// <summary>
		/// Parameters: 
		/// e[0] - Pokémon being created
		/// </summary>
		public static void OnWildPokemonCreateTrigger()
		{
			OnWildPokemonCreateEventArgs e = new OnWildPokemonCreateEventArgs(); 
			if(OnWildPokemonCreate != null) OnWildPokemonCreate.Invoke(null, e);
		}
		/// <summary>
		/// Parameters: 
		/// e[0] - Trainer
		/// e[1] - Items possessed by the trainer
		/// e[2] - Party
		/// </summary>
		public static void OnTrainerPartyLoadTrigger()
		{
			OnTrainerPartyLoadEventArgs e = new OnTrainerPartyLoadEventArgs(); 
			if(OnTrainerPartyLoad != null) OnTrainerPartyLoad.Invoke(null, e);
		}
		/// <summary>
		/// Parameters:
		/// e[0] = Scene_Map object.
		/// e[1] = Whether the player just moved to a new map (either true or false). If
		///   false, some other code had called <see cref="Game.GameData.Scene.createSpritesets"/>
		///   to regenerate the map scene without transferring the player elsewhere
		/// </summary>
		public static void OnMapSceneChangeTrigger()
		{
			OnMapSceneChangeEventArgs e= new OnMapSceneChangeEventArgs(); 
			if(OnMapSceneChange != null) OnMapSceneChange.Invoke(null, e);
		}
		/// <summary>
		/// Parameters:
		/// e[0] = Spriteset being created
		/// e[1] = Viewport used for tilemap and characters
		/// e[0].map = Map associated with the spriteset (not necessarily the current map).
		/// </summary>
		public static void OnSpritesetCreateTrigger()
		{
			OnSpritesetCreateEventArgs e = new OnSpritesetCreateEventArgs(); 
			if(OnSpritesetCreate != null) OnSpritesetCreate.Invoke(null, e);
		}
		#endregion
	}
}