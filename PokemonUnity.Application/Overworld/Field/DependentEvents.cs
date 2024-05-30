using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Inventory;
using PokemonUnity.Overworld;
using PokemonUnity.Utility;
using PokemonUnity.Interface;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.RPGMaker;
using PokemonEssentials.Interface.RPGMaker.Kernal;

namespace PokemonUnity
{
	public partial class PokemonTemp : ITempMetadataDependantEvents {
		public IDependentEvents dependentEvents				{ get; protected set; }

		//public DependentEvents dependentEvents { get {
		//  if (@dependentEvents == null) @dependentEvents=new DependentEvents();
		//  return @dependentEvents;
		//} }
	}

	public partial class GameEvent : IGameEventDependantEvents {
		protected bool starting;

		public void set_starting() {
			@starting=true;
		}
	}

	public partial class GlobalMetadata : IGlobalMetadataDependantEvents {
		//public IList<IDependentEvents> dependentEvents				{ get; protected set; }
		public IList<IGameEvent> dependentEvents				{ get; protected set; }

		//public IDependentEvents dependentEvents { get {
		//  if (@dependentEvents == null) @dependentEvents=new List<DependentEvents>();
		//  return @dependentEvents;
		//} }
	}

	public partial class Game : IGameDependantEvents {
		event Action<object, PokemonEssentials.Interface.EventArg.IOnSpritesetCreateEventArgs> IGameDependantEvents.OnSpritesetCreate { add { OnSpritesetCreateEvent += value; } remove { OnSpritesetCreateEvent -= value; } }

		public void RemoveDependencies() {
			if (Game.GameData.PokemonTemp is ITempMetadataDependantEvents tmd) tmd.dependentEvents.removeAllEvents();
			DeregisterPartner(); //rescue null
		}

		public void AddDependency(IGameCharacter @event) {
			if (Game.GameData.PokemonTemp is ITempMetadataDependantEvents tmd) tmd.dependentEvents.addEvent(@event);
		}

		public void RemoveDependency(IGameCharacter @event) {
			if (Game.GameData.PokemonTemp is ITempMetadataDependantEvents tmd) tmd.dependentEvents.removeEvent(@event);
		}

		public void AddDependency2(int eventID,string eventName, object commonEvent) {
			if (Game.GameData.PokemonTemp is ITempMetadataDependantEvents tmd) tmd.dependentEvents.addEvent(Game.GameData.GameMap.events[eventID],eventName,commonEvent);
		}

		/// <summary>
		/// Gets the <see cref="IGameCharacter"/> object associated with a dependent event.
		/// </summary>
		/// <param name="eventName"></param>
		public IGameCharacter GetDependency(string eventName) {
			if (Game.GameData.PokemonTemp is ITempMetadataDependantEvents tmd) return tmd.dependentEvents.getEventByName(eventName);
			return null;
		}

		public void RemoveDependency2(string eventName) {
			if (Game.GameData.PokemonTemp is ITempMetadataDependantEvents tmd) tmd.dependentEvents.removeEventByName(eventName);
		}



		public bool TestPass(IGameCharacter follower,float x,float y,int? direction=null) {
			return Game.GameData.MapFactory.isPassableStrict(follower.map.map_id,x,y,follower);
		}

		// Same map only
		public void moveThrough(IGameCharacter follower,int direction) {
			bool oldThrough=follower.through;
			follower.through=true;
			switch (direction) {
				case 2: // down
					follower.move_down();
					break;
				case 4: // left
					follower.move_left();
					break;
				case 6: // right
					follower.move_right();
					break;
				case 8: // up
					follower.move_up();
				break;
			}
			follower.through=oldThrough;
		}

		// Same map only
		public void moveFancy(IGameCharacter follower,int direction) {
			int deltaX=(direction == 6 ? 1 : (direction == 4 ? -1 : 0));
			int deltaY=(direction == 2 ? 1 : (direction == 8 ? -1 : 0));
			float newX = follower.x + deltaX;
			float newY = follower.y + deltaY;
			//  Move if new position is the player's, or the new position is passable,
			//  or the current position is not passable
			if ((Game.GameData.GamePlayer.x==newX && Game.GameData.GamePlayer.y==newY) ||
				TestPass(follower,newX,newY,0) ||
				!TestPass(follower,follower.x,follower.y,0)) {
				bool oldThrough=follower.through;
				follower.through=true;
				switch (direction) {
					case 2: // down
						follower.move_down();
						break;
					case 4: // left
						follower.move_left();
						break;
					case 6: // right
						follower.move_right();
						break;
					case 8: // up
						follower.move_up();
						break;
				}
				follower.through=oldThrough;
			}
		}

		// Same map only
		public void jumpFancy(IGameCharacter follower,int direction) {
			int deltaX=(direction == 6 ? 2 : (direction == 4 ? -2 : 0));
			int deltaY=(direction == 2 ? 2 : (direction == 8 ? -2 : 0));
			int halfDeltaX=(direction == 6 ? 1 : (direction == 4 ? -1 : 0));
			int halfDeltaY=(direction == 2 ? 1 : (direction == 8 ? -1 : 0));
			bool middle=TestPass(follower,follower.x+halfDeltaX,follower.y+halfDeltaY,0);
			bool ending=TestPass(follower,follower.x+deltaX,    follower.y+deltaY,    0);
			if (middle) {
				moveFancy(follower,direction);
				moveFancy(follower,direction);
			} else if (ending) {
				if (TestPass(follower,follower.x,follower.y,0)) {
					follower.jump(deltaX,deltaY);
				} else {
					moveThrough(follower,direction);
					moveThrough(follower,direction);
				}
			}
		}

		public void FancyMoveTo(IGameCharacter follower,float newX,float newY) {
			if (follower.x-newX==-1 && follower.y==newY) {
					moveFancy(follower,6);
			} else if (follower.x-newX==1 && follower.y==newY) {
				moveFancy(follower,4);
			} else if (follower.y-newY==-1 && follower.x==newX) {
				moveFancy(follower,2);
			} else if (follower.y-newY==1 && follower.x==newX) {
				moveFancy(follower,8);
			} else if (follower.x-newX==-2 && follower.y==newY) {
				jumpFancy(follower,6);
			} else if (follower.x-newX==2 && follower.y==newY) {
				jumpFancy(follower,4);
			} else if (follower.y-newY==-2 && follower.x==newX) {
				jumpFancy(follower,2);
			} else if (follower.y-newY==2 && follower.x==newX) {
				jumpFancy(follower,8);
			} else if (follower.x!=newX || follower.y!=newY) {
				follower.moveto(newX,newY);
			}
		}
	}

	/*public class DependentEvents : IDependentEvents {
		public int lastUpdate				{ get; protected set; }
		protected IList<IGameEvent> @realEvents;
		protected IGameEvent @event;

		public DependentEvents() {
			initialize();
		}

		public IDependentEvents initialize() {
			//  Original map, Event ID, Current map, X, Y, Direction
			IList<IGameEvent> events=Game.GameData.Global.dependentEvents;
			@realEvents=new List<IGameEvent>();
			@lastUpdate=-1;
			foreach (IGameEvent @event in events) {
				@realEvents.Add(createEvent(@event));
			}
			return this;
		}

		public IGameEvent createEvent(IGameEvent eventData) {
			PokemonEssentials.Interface.RPGMaker.Kernal.IEvent rpgEvent=new RPG.Event(eventData[3],eventData[4]);
			rpgEvent.id=eventData[1];
			if (eventData != null) { //common events|eventData[9]
				//  Must setup common event list here and now
				IGameCommonEvent commonEvent=new Game_CommonEvent(eventData[9]);
				rpgEvent.pages[0].list=commonEvent.list;
			}
			IGameEvent newEvent=new Game_Event(eventData.map_id,rpgEvent, //eventData[0]
				Game.GameData.MapFactory.getMap(eventData.map_id)); //eventData[2]
			newEvent.character_name=eventData.character_name;	//eventData[6];
			newEvent.character_hue=eventData.character_hue;		//eventData[7];
			switch (eventData.direction) { // direction|eventData[5]
				case 2: // down
					newEvent.turn_down();
					break;
				case 4: // left
					newEvent.turn_left();
					break;
				case 6: // right
					newEvent.turn_right();
					break;
				case 8: // up
					newEvent.turn_up();
					break;
			}
			return newEvent;
		}

		public int EnsureEvent(IGameEvent @event,int newMapID) {
			IList<IGameEvent> events=Game.GameData.Global.dependentEvents;
			//int found=-1;
			for (int i = 0; i < events.Count; i++) {
				//  Check original map ID and original event ID
				if (events[i].map_id==@event.map_id && events[i].id==@event.id) { //[0] && [1]
					//  Change current map ID
					events[i].oldMap=newMapID; //[2]
					IGameEvent newEvent=createEvent(events[i]);
					//  Replace event
					@realEvents[i]=newEvent;
					@lastUpdate+=1;
					return i;
				}
			}
			return -1;
		}

		public void FollowEventAcrossMaps(IGameCharacter leader,IGameCharacter follower,bool instant=false,bool leaderIsTrueLeader=true) {
			int d=leader.direction;
			bool areConnected=Game.GameData.MapFactory.areConnected(leader.map.map_id,follower.map.map_id);
			//  Get the rear facing tile of leader
			int facingDirection=new int[] { 0, 0, 8, 0, 6, 0, 4, 0, 2 }[d];
			if (!leaderIsTrueLeader && areConnected) {
				IPoint relativePos=Game.GameData.MapFactory.getThisAndOtherEventRelativePos(leader,follower);
				if (relativePos.y==0 && relativePos.x==2) {		// 2 spaces to the right of leader
					facingDirection=6;
				} else if (relativePos.y==0 && relativePos.x==-2) {		// 2 spaces to the left of leader
					facingDirection=4;
				} else if (relativePos.y==-2 && relativePos.x==0) {		// 2 spaces above leader
					facingDirection=8;
				} else if (relativePos.y==2 && relativePos.x==0) {		// 2 spaces below leader
					facingDirection=2;
				}
			}
			IList<int> facings=new List<int>() { facingDirection }; // Get facing from behind
			facings.Add(new int[] { 0, 0, 4, 0, 8, 0, 2, 0, 6 }[d]); // Get right facing
			facings.Add(new int[] { 0, 0, 6, 0, 2, 0, 8, 0, 4 }[d]); // Get left facing
			if (!leaderIsTrueLeader) {
				facings.Add(new int[] { 0, 0, 2, 0, 4, 0, 6, 0, 8 }[d]); // Get forward facing
			}
			ITilePosition mapTile=null;
			if (areConnected) {
				double bestRelativePos=-1;
				bool oldthrough=follower.through;
				follower.through=false;
				for (int i = 0; i < facings.Count; i++) {
					int facing=facings[i];
					ITilePosition tile=Game.GameData.MapFactory.getFacingTile(facing,leader);
					bool passable=tile != null && Game.GameData.MapFactory.isPassableStrict(tile.MapId,tile.X,tile.Y,follower);
					if (i==0 && !passable && tile != null &&
						Terrain.isLedge(Game.GameData.MapFactory.getTerrainTag(tile.MapId, tile.X, tile.Y))) {
						//  If the tile isn't passable and the tile is a ledge,
						//  get tile from further behind
						tile=Game.GameData.MapFactory.getFacingTileFromPos(tile.MapId, tile.X, tile.Y, facing);
						passable=tile != null && Game.GameData.MapFactory.isPassableStrict(tile.MapId, tile.X, tile.Y, follower);
					}
					if (passable) {
						IPoint relativePos=Game.GameData.MapFactory.getThisAndOtherPosRelativePos(
							follower,tile.MapId,tile.X,tile.Y);
						double distance=Math.Sqrt(relativePos.x*relativePos.x+relativePos.y*relativePos.y);
						if (bestRelativePos==-1 || bestRelativePos>distance) {
							bestRelativePos=distance;
							mapTile=tile;
						}
						if (i==0 && distance<=1) {		// Prefer behind if tile can move up to 1 space
							break;
						}
					}
				}
				follower.through=oldthrough;
			} else {
				ITilePosition tile=Game.GameData.MapFactory.getFacingTile(facings[0],leader);
				bool passable=tile != null && Game.GameData.MapFactory.isPassableStrict(
					tile.MapId, tile.X, tile.Y, (IGameCharacter)follower);
				mapTile=passable ? mapTile : null;
			}
			if (mapTile != null && follower.map.map_id==mapTile.MapId) {
				//  Follower is on same map
				float newX=mapTile.X;
				float newY=mapTile.Y;
				int deltaX=(d == 6 ? -1 : d == 4 ? 1 : 0);
				int deltaY=(d == 2 ? -1 : d == 8 ? 1 : 0);
				float posX = newX + deltaX;
				float posY = newY + deltaY;
				follower.move_speed=leader.move_speed; // sync movespeed
				if ((follower.x-newX==-1 && follower.y==newY) ||
					(follower.x-newX==1 && follower.y==newY) ||
					(follower.y-newY==-1 && follower.x==newX) ||
					(follower.y-newY==1 && follower.x==newX)) {
					if (instant) {
						follower.moveto(newX,newY);
					} else {
						if (Game.GameData is IGameDependantEvents igde) igde.FancyMoveTo(follower,newX,newY);
					}
				} else if ((follower.x-newX==-2 && follower.y==newY) ||
					(follower.x-newX==2 && follower.y==newY) ||
					(follower.y-newY==-2 && follower.x==newX) ||
					(follower.y-newY==2 && follower.x==newX)) {
					if (instant) {
						follower.moveto(newX,newY);
					} else {
						if (Game.GameData is IGameDependantEvents igde) igde.FancyMoveTo(follower,newX,newY);
					}
				} else if (follower.x!=posX || follower.y!=posY) {
					if (instant) {
						follower.moveto(newX,newY);
					} else {
						if (Game.GameData is IGameDependantEvents igde) {
							igde.FancyMoveTo(follower, posX, posY);
							igde.FancyMoveTo(follower, newX, newY);
						}
					}
				}
				if (Game.GameData is IGameField igf) igf.TurnTowardEvent(follower,leader);
			} else {
				if (mapTile == null) {
					//  Make current position into leader's position
					mapTile=new TilePosition(leader.map.map_id, leader.x, leader.y);
				}
				if (follower.map.map_id==mapTile.MapId) {
					//  Follower is on same map as leader
					follower.moveto(leader.x,leader.y);
					if (Game.GameData is IGameField igf) igf.TurnTowardEvent(follower,leader);
				} else {
					//  Follower will move to different map
					IList<IGameEvent> events=Game.GameData.Global.dependentEvents;
					int eventIndex=EnsureEvent((IGameEvent)follower,mapTile.MapId);
					if (eventIndex>=0) {
						IGameEvent newFollower=@realEvents[eventIndex];
						IGameEvent newEventData=events[eventIndex];
						newFollower.moveto(mapTile.X,mapTile.Y);
						newEventData.x=(int)mapTile.X; //[3]
						newEventData.y=(int)mapTile.Y; //[4]
						if (mapTile.MapId==leader.map.map_id && Game.GameData is IGameField igf0) {
							igf0.TurnTowardEvent(follower,leader);
						}
					}
				}
			}
		}

		public void debugEcho() {
			//this.eachEvent {|e,d|
			//   echoln d;
			//   echoln [e.map_id,e.map.map_id,e.id];
			//}
		}

		public void MapChangeMoveDependentEvents() {
			IList<IGameEvent> events=Game.GameData.Global.dependentEvents;
			updateDependentEvents();
			IGamePlayer leader=Game.GameData.GamePlayer;
			for (int i = 0; i < events.Count; i++) {
				@event=@realEvents[i];
				FollowEventAcrossMaps(leader,@event,true,i==0);
				IGameEvent entity = events[i]; //(IGameCharacter)
				//  Update X and Y for this event
				//events[i][3]=@event.x;
				//events[i][4]=@event.y;
				//events[i][5]=@event.direction;
				entity.x=@event.x;
				entity.y=@event.y;
				entity.direction=@event.direction;
				events[i]=entity;
				//  Set leader to this event
				leader=@event;
			}
		}

		public void MoveDependentEvents() {
			IList<IGameEvent> events=Game.GameData.Global.dependentEvents;
			updateDependentEvents();
			IGamePlayer leader=Game.GameData.GamePlayer;
			for (int i = 0; i < events.Count; i++) {
				@event=@realEvents[i];
				FollowEventAcrossMaps(leader,@event,false,i==0);
				IGameCharacter entity = (IGameCharacter)events[i];
				//  Update X and Y for this event
				//events[i][3]=@event.x;
				entity.x=@event.x;
				//events[i][4]=@event.y;
				entity.y=@event.y;
				//events[i][5].direction=@event.direction;
				entity.direction=@event.direction;
				events[i]=entity;
				//  Set leader to this event
				leader=@event;
			}
		}

		public void TurnDependentEvents() {
			IList<IGameEvent> events=Game.GameData.Global.dependentEvents;
			updateDependentEvents();
			IGamePlayer leader=Game.GameData.GamePlayer;
			for (int i = 0; i < events.Count; i++) {
				@event=@realEvents[i];
				if (Game.GameData is IGameField igf) igf.TurnTowardEvent(@event,leader);
				IGameEvent entity = events[i]; //(IGameCharacter)
				//  Update direction for this event
				//events[i][5]=@event.direction;
				entity.direction=@event.direction;
				events[i]=entity;
				//  Set leader to this event
				leader=(IGameEvent)@event;
			}
		}

		public IEnumerable<KeyValuePair<IGameEvent,IGameEvent>> eachEvent() {
			IList<IGameEvent> events=Game.GameData.Global.dependentEvents;
			for (int i = 0; i < events.Count; i++) {
				yield return new KeyValuePair<IGameEvent,IGameEvent>(@realEvents[i],events[i]);
			}
		}

		public void updateDependentEvents() {
			IList<IGameEvent> events=Game.GameData.Global.dependentEvents;
			if (events.Count==0) return;
			for (int i = 0; i < events.Count; i++) {
				@event=@realEvents[i];
				if (@realEvents[i] == null) continue;
				@event.transparent=Game.GameData.GamePlayer.transparent;
				if ((@event.jumping || @event.moving) || !(Game.GameData.GamePlayer.jumping || Game.GameData.GamePlayer.moving)) {
					@event.update();
				} else if (!@event.starting) {
					if (@event is IGameEventDependantEvents ed) ed.set_starting();
					@event.update();
					@event.clear_starting();
				}
				//events[i][3]=@event.x;
				//events[i][4]=@event.y;
				//events[i][5]=@event.direction;
				IGameEvent entity = events[i]; //(IGameCharacter)
				entity.x=@event.x;
				entity.y=@event.y;
				entity.direction=@event.direction;
				events[i]=entity;
			}
			//  Check event triggers
			if (Input.trigger(Input.C) && (Game.GameData is IGameMessage gm) && !gm.MapInterpreterRunning()) {
				//  Get position of tile facing the player
				ITilePosition facingTile=Game.GameData.MapFactory.getFacingTile();
				//this.eachEvent {|e,d|
				foreach (KeyValuePair<IGameEvent,IGameEvent> e in this.eachEvent()) { //|e,d|
					if (!e.Value[9] != null) continue; //if (!d[9]) continue;
					if (e.Key.x==Game.GameData.GamePlayer.x && e.Key.y==Game.GameData.GamePlayer.y) {
						//  On same position
						if (!e.Key.jumping && e.Key.over_trigger) { //(!e.respond_to?("over_trigger") ||
							if (e.Key.list.Count>1) { //.size>1
								//  Start event
								if (Game.GameData.GameMap.need_refresh) Game.GameData.GameMap.refresh();
								e.Key._lock();
								//MapInterpreter.setup(e.Key.list,e.Key.id,e.Key.map.map_id);
								if (Game.GameData is IGameMessage gm0 && gm0.MapInterpreter() is IInterpreterFieldMixin ifm) ifm.setup(e.Key.list,e.Key.id,e.Key.map.map_id);
							}
						}
					} else if (facingTile!=null && e.Key.map.map_id==facingTile.MapId &&
						e.Key.x==facingTile.X && e.Key.y==facingTile.Y) {
						//  On facing tile
						if (!e.Key.jumping && !e.Key.over_trigger) { //(!e.respond_to?("over_trigger") ||
							if (e.Key.list.Count>1) { //.size>1
								//  Start event
								if (Game.GameData.GameMap.need_refresh) Game.GameData.GameMap.refresh();
								e.Key._lock();
								//MapInterpreter.setup(e.Key.list,e.Key.id,e.Key.map.map_id);
								if (Game.GameData is IGameMessage gm1 && gm1.MapInterpreter() is IInterpreterFieldMixin ifm) ifm.setup(e.Key.list,e.Key.id,e.Key.map.map_id);
							}
						}
					}
				}
			}
		}

		public void removeEvent(IGameEvent @event) {
			IList<IGameEvent> events=Game.GameData.Global.dependentEvents;
			int mapid=Game.GameData.GameMap.map_id;
			for (int i = 0; i < events.Count; i++) {
				if (events[i].map_id==mapid &&		//[2] Refer to current map
					events[i].map_id==@event.map_id && //[0] Event's map ID is original ID
					events[i].id==@event.id) {
					//events[i]=null;
					events.RemoveAt(i);
					//@realEvents[i]=null;
					@realEvents.RemoveAt(i);
					@lastUpdate+=1;
				}
				//events.compact()!;
				//@realEvents.compact()!;
			}
		}

		public IGameEvent getEventByName(string name) {
			IList<IGameEvent> events=Game.GameData.Global.dependentEvents;
			for (int i = 0; i < events.Count; i++) {
				if (events[i] != null && events[i].name==name) {		//[8] Arbitrary name given to dependent event
					return @realEvents[i];
				}
			}
			return null;
		}

		public void removeAllEvents() {
			IList<IGameEvent> events=Game.GameData.Global.dependentEvents;
			events.Clear();
			@realEvents.Clear();
			@lastUpdate+=1;
		}

		public void removeEventByName(string name) {
			IList<IGameEvent> events=Game.GameData.Global.dependentEvents;
			for (int i = 0; i < events.Count; i++) {
				if (events[i] != null && events[i].name==name) {		//[8] Arbitrary name given to dependent event
					//events[i]=null;
					events.RemoveAt(i);
					//@realEvents[i]=null;
					@realEvents.RemoveAt(i);
					@lastUpdate+=1;
				}
				//events.compact()!;
				//@realEvents.compact()!;
			}
		}

		public void addEvent(IGameEvent @event,string eventName=null,object commonEvent=null) {
			if (@event == null) return;
			IList<IGameEvent> events=Game.GameData.Global.dependentEvents;
			for (int i = 0; i < events.Count; i++) {
				if (events[i] != null && events[i].map_id==Game.GameData.GameMap.map_id && events[i].id==@event.id) { //[0] && [1]
					//  Already exists
					return;
				}
			}
			//  Original map ID, original event ID, current map ID,
			//  event X, event Y, event direction,
			//  event's filename,
			//  event's hue, event's name, common event ID
			IGameCharacter eventData=new GameCharacter (//[
				Game.GameData.GameMap.map_id,@event.id,Game.GameData.GameMap.map_id,
				@event.x,@event.y,@event.direction,
				@event.character_name,//.clone
				@event.character_hue,eventName,commonEvent
			);//];
			IGameEvent newEvent=createEvent((IGameEvent)eventData);
			events.Add((IGameEvent)eventData);
			@realEvents.Add(newEvent);
			@lastUpdate+=1;
			@event.erase();
		}
	}*/

	//public class DependentEventSprites {
	//  public void refresh() {
	//    foreach (var sprite in @sprites) {
	//      sprite.dispose();
	//    }
	//    @sprites.clear();
	//    //Game.GameData.PokemonTemp.dependentEvents.eachEvent {|@event,data|
	//    //   if (data[0]==@map.map_id) {		// Check original map
	//    //     @map.events[data[1]].erase;
	//    //   }
	//    //   if (data[2]==@map.map_id) {		// Check current map
	//    //     @sprites.Add(new Sprite_Character(@viewport,@event));
	//    //   }
	//    //}
	//  }
	//
	//  public DependentEventSprites initialize(ICanvas viewport,Game_Map map) {
	//    @disposed=false;
	//    @sprites=[];
	//    @map=map;
	//    @viewport=viewport;
	//    refresh();
	//    @lastUpdate=null;
	//    return this;
	//  }
	//
	//  public void update() {
	//    if (Game.GameData.PokemonTemp.dependentEvents.lastUpdate!=@lastUpdate) {
	//      refresh();
	//      @lastUpdate=Game.GameData.PokemonTemp.dependentEvents.lastUpdate;
	//    }
	//    foreach (var sprite in @sprites) {
	//      sprite.update();
	//    }
	//  }
	//
	//  public void dispose() {
	//    if (@disposed) return;
	//    foreach (var sprite in @sprites) {
	//      sprite.dispose();
	//    }
	//    @sprites.clear();
	//    @disposed=true;
	//  }
	//
	//  public bool disposed { get {
	//    return @disposed;
	//  } }
	//}
}

//Events.onSpritesetCreate+=proc{|sender,e|
//   spriteset=e[0]; // Spriteset being created
//   viewport=e[1]; // Viewport used for tilemap and characters
//   map=spriteset.map; // Map associated with the spriteset (not necessarily the current map).
//   spriteset.addUserSprite(new DependentEventSprites(viewport,map));
//}
//
//Events.onMapSceneChange+=proc{|sender,e|
//   scene=e[0];
//   mapChanged=e[1];
//   if (mapChanged) {
//     Game.GameData.PokemonTemp.dependentEvents.MapChangeMoveDependentEvents();
//   }}
//}