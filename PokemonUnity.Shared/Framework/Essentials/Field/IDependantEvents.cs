using System;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Overworld;
using PokemonUnity.Interface;
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonEssentials.Interface.Field
{
	/// <summary>
	/// Extension of <see cref="ITempMetadata"/>
	/// </summary>
	public interface ITempMetadataDependantEvents : ITempMetadata {
		IDependentEvents dependentEvents				{ get; }

		//IDependentEvents dependentEvents();
	}

	public interface IGameDependantEvents : IGame
	{
		void RemoveDependencies();

		void AddDependency(IGameCharacter @event);

		void RemoveDependency(IGameCharacter @event);

		void AddDependency2(int eventID, string eventName, object commonEvent);

		/// <summary>
		/// Gets the <see cref="IGameCharacter"/> object associated with a dependent event.
		/// </summary>
		/// <param name="eventName"></param>
		IGameCharacter GetDependency(string eventName);
		//  return Game.GameData.PokemonTemp.dependentEvents.getEventByName(eventName);
		//}

		void RemoveDependency2(string eventName);



		bool TestPass(IGameCharacter follower, float x, float y, int? direction = null);

		/// <summary>
		/// Same map only
		/// </summary>
		/// <param name="follower"></param>
		/// <param name="direction"></param>
		void moveThrough(IGameCharacter follower, int direction);

		/// <summary>
		/// Same map only
		/// </summary>
		/// <param name="follower"></param>
		/// <param name="direction"></param>
		void moveFancy(IGameCharacter follower, int direction);

		/// <summary>
		/// Same map only
		/// </summary>
		/// <param name="follower"></param>
		/// <param name="direction"></param>
		void jumpFancy(IGameCharacter follower, int direction);

		void FancyMoveTo(IGameCharacter follower, float newX, float newY);

		/// <summary>
		/// Fires whenever a spriteset is created.
		/// </summary>
		//event EventHandler<IOnSpritesetCreateEventArgs> OnSpritesetCreate;
		event Action<object, EventArg.IOnSpritesetCreateEventArgs> OnSpritesetCreate;
		//Events.onSpritesetCreate+=delegate(object sender, EventArgs e) {
		//   spriteset=e[0]; // Spriteset being created
		//   viewport=e[1]; // Viewport used for tilemap and characters
		//   map=spriteset.map; // Map associated with the spriteset (not necessarily the current map).
		//   spriteset.addUserSprite(new DependentEventSprites(viewport,map));
		//}

		/// <summary>
		/// Fires whenever the map scene is regenerated and soon after the player moves
		/// to a new map.
		/// </summary>
		//event EventHandler<IOnMapSceneChangeEventArgs> OnMapSceneChange;
		event Action<object, EventArg.IOnMapSceneChangeEventArgs> OnMapSceneChange;
		//Events.onMapSceneChange+=delegate(object sender, EventArgs e) {
		//   scene=e[0];
		//   mapChanged=e[1];
		//   if (mapChanged) {
		//	 Game.GameData.PokemonTemp.dependentEvents.MapChangeMoveDependentEvents;
		//   }}
	}

	public interface IGlobalMetadataDependantEvents : Field.IGlobalMetadata {
		//IList<IDependentEvents> dependentEvents				{ get; }
		//IList<IGameEvent> dependentEvents				{ get; }
		IList<IGameCharacter> dependentEvents				{ get; }

		//public void dependentEvents() {
		//	if (!@dependentEvents) @dependentEvents=[];
		//	return @dependentEvents;
		//}
	}

	public interface IGameEventDependantEvents //: IGameEvent
	{
		void set_starting();
	}

	//ToDo: replace IGameEvents to IGameCharacters?
	public interface IDependentEvents {
		/// <summary>
		/// </summary>
		/// <param name="eventData">
		/// eventData input parameter:
		///  [Original map ID, original event ID, current map ID,
		///  event X, event Y, event direction,
		///  event's filename,
		///  event's hue, event's name, common event ID]
		/// </param>
		/// <returns></returns>
		IGameEvent createEvent(IGameCharacter eventData);

		int lastUpdate				{ get; }

		IDependentEvents initialize();

		int EnsureEvent(IGameEvent @event, int newMapID);

		void FollowEventAcrossMaps(IGameCharacter leader, IGameCharacter follower, bool instant = false, bool leaderIsTrueLeader = true);

		void debugEcho();

		void MapChangeMoveDependentEvents();

		void MoveDependentEvents();

		void TurnDependentEvents();

		//ToDo: Review Return Datatypes
		IEnumerable<KeyValuePair<IGameCharacter, IGameEvent>> eachEvent();

		void updateDependentEvents();

		//void removeEvent(IGameEvent @event);
		void removeEvent(IGameCharacter @event);

		IGameEvent getEventByName(string name);

		void removeAllEvents();

		void removeEventByName(string name);

		void addEvent(IGameCharacter @event, string eventName = null, object commonEvent= null);
	}



	public interface IDependentEventSprites : IDisposable {
		void refresh();

		IDependentEventSprites initialize(IViewport viewport, IGameMap map);

		void update();

		//void dispose();

		bool disposed { get; }
	}
}