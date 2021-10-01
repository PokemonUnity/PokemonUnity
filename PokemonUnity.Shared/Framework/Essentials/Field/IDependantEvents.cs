using System;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Overworld;
using PokemonUnity.UX;
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonEssentials.Interface.Field
{
	/// <summary>
	/// Extension of <see cref="ITempMetadata"/>
	/// </summary>
	public interface ITempMetadataDependantEvents {
		IDependentEvents dependentEvents				{ get; }

		//IDependentEvents dependentEvents();
	}

	public interface IGameDependantEvents
	{
		void pbRemoveDependencies();

		void pbAddDependency(IGameEvent @event);

		void pbRemoveDependency(IGameEvent @event);

		void pbAddDependency2(int eventID, string eventName, object commonEvent);

		/// <summary>
		/// Gets the Game_Character object associated with a dependent event.
		/// </summary>
		/// <param name="eventName"></param>
		//void pbGetDependency(string eventName) {
		//  return Game.GameData.PokemonTemp.dependentEvents.getEventByName(eventName);
		//}

		void pbRemoveDependency2(string eventName);



		bool pbTestPass(IGameCharacter follower, float x, float y, int? direction = null);

		// Same map only
		void moveThrough(IGameCharacter follower, int direction);

		// Same map only
		void moveFancy(IGameCharacter follower, int direction);

		// Same map only
		void jumpFancy(IGameCharacter follower, int direction);

		void pbFancyMoveTo(IGameCharacter follower, float newX, float newY);

		//Events.onSpritesetCreate+=delegate(object sender, EventArgs e) {
		//   spriteset=e[0]; // Spriteset being created
		//   viewport=e[1]; // Viewport used for tilemap and characters
		//   map=spriteset.map; // Map associated with the spriteset (not necessarily the current map).
		//   spriteset.addUserSprite(new DependentEventSprites(viewport,map));
		//}

		//Events.onMapSceneChange+=delegate(object sender, EventArgs e) {
		//   scene=e[0];
		//   mapChanged=e[1];
		//   if (mapChanged) {
		//	 Game.GameData.PokemonTemp.dependentEvents.pbMapChangeMoveDependentEvents;
		//   }}
	}

	public interface IGlobalMetadataDependantEvents {
		IDependentEvents dependentEvents				{ get; }

		//public void dependentEvents() {
		//	if (!@dependentEvents) @dependentEvents=[];
		//	return @dependentEvents;
		//}
	}

	public interface IGameEventDependantEvents {
		void set_starting();
	}

	public interface IDependentEvents {
		// eventData input parameter:
		//  [Original map ID, original event ID, current map ID,
		//  event X, event Y, event direction,
		//  event's filename,
		//  event's hue, event's name, common event ID]
		IGameEvent createEvent(IGameEvent eventData);

		int lastUpdate				{ get; }

		IDependentEvents initialize();

		int pbEnsureEvent(IGameEvent @event, int newMapID);

		void pbFollowEventAcrossMaps(IGameCharacter leader, IGameCharacter follower, bool instant = false, bool leaderIsTrueLeader = true);

		void debugEcho();

		void pbMapChangeMoveDependentEvents();

		void pbMoveDependentEvents();

		void pbTurnDependentEvents();

		IEnumerator<KeyValuePair<IGameEvent, IGameEvent>> eachEvent();

		void updateDependentEvents();

		void removeEvent(IGameEvent @event);

		IGameEvent getEventByName(string name);

		void removeAllEvents();

		void removeEventByName(string name);

		void addEvent(IGameEvent @event, string eventName = null, object commonEvent= null);
	}



	public interface IDependentEventSprites : IDisposable {
		void refresh();

		IDependentEventSprites initialize(IViewport viewport, IGameMap map);

		void update();

		void dispose();

		bool disposed();
	}
}