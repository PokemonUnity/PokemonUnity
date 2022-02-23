using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.EventArg;

namespace PokemonEssentials.Interface
{
	/*// <summary>
	/// Defines an event that procedures can subscribe to.
	/// </summary>
	public interface IEvent {
		/// <summary>
		/// Add/Removes an event handler procedure from the event.
		/// </summary>
		event EventHandler EventName; //{ add; remove; }

		IEvent initialize();

		/// <summary>
		/// Sets an event handler for this event and removes all other event handlers.
		/// </summary>
		/// <param name=""></param>
		void set(Action method);

		// Removes an event handler procedure from the event.
		//void -(method) {
		//  for (int i = 0; i < @callbacks.length; i++) {
		//    if (@callbacks[i]==method) {
		//      @callbacks.delete_at(i);
		//      break;
		//    }
		//  }
		//  return self;
		//}
		//
		//// Adds an event handler procedure from the event.
		//void +(method) {
		//  for (int i = 0; i < @callbacks.length; i++) {
		//    if (@callbacks[i]==method) {
		//      return self;
		//    }
		//  }
		//  @callbacks.Add(method);
		//  return self;
		//}

		/// <summary>
		/// Clears the event of event handlers.
		/// </summary>
		void clear();

		/// <summary>
		/// Triggers the event and calls all its event handlers. Normally called only
		/// by the code where the event occurred.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// The first argument is the sender of the event, the second argument contains
		/// the event's parameters. If three or more arguments are given, this method
		/// supports the following callbacks:
		/// proc {|sender,params| } where params is an array of the other parameters, and
		/// proc {|sender,arg0,arg1,...| }
		//void trigger(params int[] arg);
		void trigger(object sender, EventArgs e);

		/// <summary>
		/// Triggers the event and calls all its event handlers. Normally called only
		/// by the code where the event occurred. The first argument is the sender of the
		/// event, the other arguments are the event's parameters.
		/// </summary>
		/// <param name=""></param>
		//void trigger2(*arg);
		void trigger2(object sender, EventArgs e);
	}*/
	/// <summary>
	/// This module stores events that can happen during the game. A procedure can
	/// subscribe to an event by adding itself to the event. It will then be called
	/// whenever the event occurs.
	/// </summary>
	/// https://stackoverflow.com/a/47323956/3681384
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
		//event EventHandler<IOnMapSceneChangeEventArgs> OnMapSceneChange;
		event Action<object, IOnMapSceneChangeEventArgs> OnMapSceneChange;
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
		//event EventHandler<IOnLeaveTileEventArgs> OnLeaveTile;
		event Action<object, IOnLeaveTileEventArgs> OnLeaveTile;
		/// <summary>
		/// Fires whenever the player takes a step.
		/// </summary>
		event EventHandler OnStepTaken;
		/// <summary>
		/// Fires whenever the player takes a step. The event handler may possibly move
		/// the player elsewhere.
		/// </summary>
		//event EventHandler<IOnStepTakenTransferPossibleEventArgs> OnStepTakenTransferPossible;
		event Action<object, IOnStepTakenTransferPossibleEventArgs> OnStepTakenTransferPossible;
		/// <summary>
		/// Fires whenever the player or another event enters a tile.
		/// </summary>
		//event EventHandler<IOnStepTakenFieldMovementEventArgs> OnStepTakenFieldMovement;
		event Action<object, IOnStepTakenFieldMovementEventArgs> OnStepTakenFieldMovement;
		/// <summary>
		/// Triggers at the start of a wild battle.  Event handlers can provide their own
		/// wild battle routines to override the default behavior.
		/// </summary>
		//event EventHandler<IOnWildBattleOverrideEventArgs> OnWildBattleOverride;
		event Action<object, IOnWildBattleOverrideEventArgs> OnWildBattleOverride;
		/// <summary>
		/// Triggers whenever a wild Pokémon battle ends
		/// </summary>
		//event EventHandler<IOnWildBattleEndEventArgs> OnWildBattleEnd;
		event Action<object, IOnWildBattleEndEventArgs> OnWildBattleEnd;
		/// <summary>
		/// Triggers whenever a wild Pokémon is created
		/// </summary>
		//event EventHandler<IOnWildPokemonCreateEventArgs> OnWildPokemonCreate;
		event Action<object, IOnWildPokemonCreateEventArgs> OnWildPokemonCreate;
		/// <summary>
		/// Triggers whenever an NPC trainer's Pokémon party is loaded
		/// </summary>
		//event EventHandler<IOnTrainerPartyLoadEventArgs> OnTrainerPartyLoad;
		event Action<object, IOnTrainerPartyLoadEventArgs> OnTrainerPartyLoad;
		/// <summary>
		/// Fires whenever a spriteset is created.
		/// </summary>
		//event EventHandler<IOnSpritesetCreateEventArgs> OnSpritesetCreate;
		event Action<object, IOnSpritesetCreateEventArgs> OnSpritesetCreate;
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
		/// <summary>
		/// Parameters: 
		/// e[0] - Pokémon species
		/// e[1] - Pokémon level
		/// e[2] - Battle result (1-win, 2-loss, 3-escaped, 4-caught, 5-draw)
		/// </summary>
		//void OnWildBattleOverrideTrigger(Pokemons species,int level,BattleResults handled); //object @event,
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
		#region Global Overworld EventArgs
		/// <summary>
		/// https://stackoverflow.com/a/47323956/3681384
		/// </summary>
		public interface IEventArgs
		{
			int Id { get; }
		}
		public interface IOnMapCreateEventArgs : IEventArgs
		{
			//readonly int EventId = typeof(OnMapCreateEventArgs).GetHashCode();

			//int Id { get { return EventId; } }
			//int Id { get { return Pokemon.GetHashCode(); } } //EventId;
			int Map { get; set; }
			int Tileset { get; set; }
		}
		public interface IOnMapChangeEventArgs : IEventArgs
		{
			//readonly int EventId = typeof(OnMapChangeEventArgs).GetHashCode();

			//int Id { get { return EventId; } }
			//int Id { get { return MapId.GetHashCode(); } } //EventId;
			int MapId { get; set; }
		}
		public interface IOnMapChangingEventArgs : IEventArgs
		{
			//readonly int EventId = typeof(OnMapChangingEventArgs).GetHashCode();

			//int Id { get { return EventId; } }
			//int Id { get { return MapId.GetHashCode(); } } //EventId;
			int MapId { get; set; }
			IGameMap GameMap { get; set; }
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
		public interface IOnLeaveTileEventArgs : IEventArgs
		{
			//readonly int EventId = typeof(OnLeaveTileEventArgs).GetHashCode();

			//int Id { get { return EventId; } }
			/// <summary>
			/// Event that just left the tile.
			/// </summary>
			IGameEvent Event { get; set; }
			/// <summary>
			/// Map ID where the tile is located (not necessarily
			///  the current map). Use "Game.GameData.MapFactory.getMap(e[1])" to
			///  get the Game_Map object corresponding to that map.
			/// </summary>
			int MapId { get; set; }
			/// <summary>
			/// X-coordinate of the tile
			/// </summary>
			float X { get; set; }
			/// <summary>
			/// Y-coordinate of the tile
			/// </summary>
			float Y { get; set; }
		}
		/// <summary>
		/// Parameters:
		/// e[0] - Event that just entered a tile.
		/// </summary>
		public interface IOnStepTakenFieldMovementEventArgs : IEventArgs
		{
			//readonly int EventId = typeof(OnStepTakenFieldMovementEventArgs).GetHashCode();

			//int Id { get { return EventId; } }
			/// <summary>
			/// Event that just entered a tile.
			/// </summary>
			int Index { get; set; }
		}
		/// <summary>
		/// Parameters:
		/// e[0] = Array that contains a single boolean value.
		/// If an event handler moves the player to a new map, it should set this value
		/// to true. Other event handlers should check this parameter's value.
		/// </summary>
		public interface IOnStepTakenTransferPossibleEventArgs : IEventArgs
		{
			//readonly int EventId = typeof(OnStepTakenTransferPossibleEventArgs).GetHashCode();

			//int Id { get { return EventId; } }
			/// <summary>
			/// Array that contains a single boolean value.
			/// </summary>
			bool Index { get; set; }
		}
		/// <summary>
		/// Parameters: 
		/// e[0] - Pokémon species
		/// e[1] - Pokémon level
		/// e[2] - Battle result (1-win, 2-loss, 3-escaped, 4-caught, 5-draw)
		/// </summary>
		public interface IOnWildBattleOverrideEventArgs : IEventArgs
		{
			//static readonly int EventId = typeof(OnWildBattleOverrideEventArgs).GetHashCode();

			//int Id { get { return EventId; } }
			Pokemons Species { get; set; }
			int Level { get; set; }
			/// <summary>
			/// Battle result (1-win, 2-loss, 3-escaped, 4-caught, 5-draw)
			/// </summary>
			BattleResults Result { get; set; }
		}
		/// <summary>
		/// Parameters: 
		/// e[0] - Pokémon species
		/// e[1] - Pokémon level
		/// e[2] - Battle result (1-win, 2-loss, 3-escaped, 4-caught, 5-draw)
		/// </summary>
		public interface IOnWildBattleEndEventArgs : IEventArgs
		{
			//readonly int EventId = typeof(OnWildBattleEndEventArgs).GetHashCode();

			//int Id { get { return EventId; } }
			Pokemons Species { get; set; }
			int Level { get; set; }
			/// <summary>
			/// Battle result (1-win, 2-loss, 3-escaped, 4-caught, 5-draw)
			/// </summary>
			BattleResults Result { get; set; }
		}
		/// <summary>
		/// Parameters: 
		/// e[0] - Pokémon being created
		/// </summary>
		public interface IOnWildPokemonCreateEventArgs : IEventArgs
		{
			//readonly int EventId = typeof(OnWildPokemonCreateEventArgs).GetHashCode();

			//int Id { get; }
			/// <summary>
			/// Pokémon being created
			/// </summary>
			IPokemon Pokemon { get; set; }
		}
		/// <summary>
		/// Parameters: 
		/// e[0] - Trainer
		/// e[1] - Items possessed by the trainer
		/// e[2] - Party
		/// </summary>
		public interface IOnTrainerPartyLoadEventArgs : IEventArgs
		{
			//readonly int EventId = typeof(OnTrainerPartyLoadEventArgs).GetHashCode();

			//int Id { get; }
			ITrainer Trainer { get; set; }
			/// <summary>
			/// Items possessed by the trainer
			/// </summary>
			Items[] Items { get; set; }
			IPokemon[] Party { get; set; }
		}
		/// <summary>
		/// Parameters:
		/// e[0] = Scene_Map object.
		/// e[1] = Whether the player just moved to a new map (either true or false). If
		///   false, some other code had called <see cref="Game.GameData.Scene.createSpritesets"/>
		///   to regenerate the map scene without transferring the player elsewhere
		/// </summary>
		public interface IOnMapSceneChangeEventArgs : IEventArgs
		{
			//readonly int EventId = typeof(OnMapSceneChangeEventArgs).GetHashCode();

			//int Id { get; }
			/// <summary>
			/// Scene_Map object.
			/// </summary>
			ISceneMap Object { get; set; }
			/// <summary>
			/// Whether the player just moved to a new map (either true or false).
			/// </summary>
			bool NewMap { get; set; }
		}
		/// <summary>
		/// Parameters:
		/// e[0] = Spriteset being created
		/// e[1] = Viewport used for tilemap and characters
		/// e[0].map = Map associated with the spriteset (not necessarily the current map).
		/// </summary>
		public interface IOnSpritesetCreateEventArgs : IEventArgs
		{
			//readonly int EventId = typeof(OnSpritesetCreateEventArgs).GetHashCode();

			//int Id { get; }
			/// <summary>
			/// Spriteset being created
			/// </summary>
			int SpritesetId { get; set; }
			/// <summary>
			/// Viewport used for tilemap and characters
			/// </summary>
			IViewport Viewport { get; set; }
			/// <summary>
			/// Map associated with the spriteset (not necessarily the current map).
			/// </summary>
			int MapId { get; set; }
			//ISpritesetMap Map { get; set; }
		}
		#endregion
	}
}