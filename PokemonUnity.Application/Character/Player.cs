using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Attack;
using PokemonUnity.Inventory;
using PokemonUnity.Saving.SerializableClasses;
using PokemonUnity.Character;
using PokemonUnity.Overworld;
using PokemonUnity.Utility;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.RPGMaker.Kernal;

namespace PokemonUnity.Avatar
{
	/// <summary>
	/// This class handles the player. Its functions include event starting
	/// determinants and map scrolling. Refer to "Game.GameData.GamePlayer" for the one
	/// instance of this class.
	/// </summary>
	public partial class Player : Character, IGamePlayer
	{
		#region Variables
		public int bump_se				{ get;  set; }

		public int encounter_count      { get; set; }
		protected int lastdir;
		protected int lastdirframe;
		#endregion

		#region Constructor
		public Player(IGameMap map = null) : base(map) {
			initialize(map);
		}
		new public IGamePlayer initialize(IGameMap map = null) {
			base.initialize(map); //super(*arg);
			@lastdir=0;
			@lastdirframe=0;
			@bump_se=0;
			return this;
		}
		#endregion

		#region Methods
		public override int bush_depth { get {
			if (@tile_id > 0 || @always_on_top) return 0;
			float xbehind=(@direction==4) ? @x+1 : (@direction==6) ? @x-1 : @x;
			float ybehind=(@direction==8) ? @y+1 : (@direction==2) ? @y-1 : @y;
			if (!Game.GameData.GameMap.valid(@x,@y) || !Game.GameData.GameMap.valid(xbehind,ybehind)) {
				if (Game.GameData.MapFactory == null) return 0;
				IMapChunkUnit newhere=Game.GameData.MapFactory.getNewMap(@x,@y);
				IMapChunkUnit newbehind=Game.GameData.MapFactory.getNewMap(xbehind,ybehind);
				IGameMap heremap=this.map;float herex=@x;float herey=@y;
				if (Game.GameData.GameMap.valid(@x,@y)) {
					heremap=this.map;herex=@x;herey=@y;
				} else if (newhere != null && newhere.Map != null) {
					heremap=newhere.Map; herex=newhere.X; herey=newhere.Y;
				} else {
					return 0;
				}
				IGameMap behindmap=this.map;float behindx=xbehind;float behindy=ybehind;
				if (Game.GameData.GameMap.valid(xbehind,ybehind)) {
					behindmap=this.map; behindx=xbehind; behindy=ybehind;
				} else if (newbehind && newbehind.Map != null) {
					behindmap=newbehind.Map; behindx=newbehind.X; behindy=newbehind.Y;
				} else {
					return 0;
				}
				if (@jump_count <= 0 && heremap.deepBush(herex, herey) &&
					behindmap.deepBush(behindx, behindy)) {
					return 32;
				} else if (@jump_count <= 0 && heremap.bush(herex, herey) && !moving) {
					return 12;
				} else {
					return 0;
				}
			} else {
				return base.bush_depth;
			}
		} }

		public bool pbHasDependentEvents() {
			return Game.GameData.Global.dependentEvents.Length>0;
		}

		public override void move_down(bool turn_enabled = true) {
			if (turn_enabled) {
				turn_down();
			}
			if (passable(@x, @y, 2)) {
				if (Game.GameData is IGameField gf1 && gf1.pbLedge(0,1)) return;
				if (Game.GameData is IGameField gf2 && gf2.pbEndSurf(0,1)) return;
				turn_down();
				@y += 1;
				if (Game.GameData.PokemonTemp is ITempMetadataDependantEvents td) td.dependentEvents.pbMoveDependentEvents();
				increase_steps();
			} else {
				if (!check_event_trigger_touch(@x, @y+1)) {
					if (@bump_se == null || @bump_se<=0) {
						Game.UI.pbSEPlay("bump"); @bump_se=10;
					}
				}
			}
		}

		public override void move_left(bool turn_enabled = true) {
			if (turn_enabled) {
				turn_left();
			}
			if (passable(@x, @y, 4)) {
				if (Game.GameData is IGameField gf1 && gf1.pbLedge(-1,0)) return;
				if (Game.GameData is IGameField gf2 && gf2.pbEndSurf(-1,0)) return;
				turn_left();
				@x -= 1;
				if (Game.GameData.PokemonTemp is ITempMetadataDependantEvents td) td.dependentEvents.pbMoveDependentEvents();
				increase_steps();
			} else {
				if (!check_event_trigger_touch(@x-1, @y)) {
					if (@bump_se == null || @bump_se<=0) {
						Game.UI.pbSEPlay("bump"); @bump_se=10;
					}
				}
			}
		}

		public override void move_right(bool turn_enabled = true) {
			if (turn_enabled) {
				turn_right();
			}
			if (passable(@x, @y, 6)) {
				if (Game.GameData is IGameField gf1 && gf1.pbLedge(1,0)) return;
				if (Game.GameData is IGameField gf2 && gf2.pbEndSurf(1,0)) return;
				turn_right();
				@x += 1;
				if(Game.GameData.PokemonTemp is ITempMetadataDependantEvents td) td.dependentEvents.pbMoveDependentEvents();
				increase_steps();
			} else {
				if (!check_event_trigger_touch(@x+1, @y)) {
					if (@bump_se == null || @bump_se<=0) {
						Game.UI.pbSEPlay("bump"); @bump_se=10;
					}
				}
			}
		}

		public override void move_up(bool turn_enabled = true) {
			if (turn_enabled) {
				turn_up();
			}
			if (passable(@x, @y, 8)) {
				if (Game.GameData is IGameField gf1 && gf1.pbLedge(0,-1)) return;
				if (Game.GameData is IGameField gf2 && gf2.pbEndSurf(0,-1)) return;
				turn_up();
				@y -= 1;
				if (Game.GameData.PokemonTemp is ITempMetadataDependantEvents td) td.dependentEvents.pbMoveDependentEvents();
				increase_steps();
			} else {
				if (!check_event_trigger_touch(@x, @y-1)) {
					if (@bump_se == null || @bump_se<=0) {
						Game.UI.pbSEPlay("bump"); @bump_se=10;
					}
				}
			}
		}

		public IList<IGameEvent> pbTriggeredTrainerEvents(int[] triggers,bool checkIfRunning=true) {
			IList<IGameEvent> result = new List<IGameEvent>();
			//  If event is running
			if (checkIfRunning && Game.GameData.GameSystem.map_interpreter.running) {
				return result;
			}
			//  All event loops
			foreach (IGameEvent @event in Game.GameData.GameMap.events.Values) {
				var regex = System.Text.RegularExpressions.Regex.Matches(@event.name, @"^Trainer\((\d+)\)$");
				if (regex == null) continue; //[/^Trainer\((\d+)\)$/]
				int distance = int.Parse(regex[1].Value);
				//  If event coordinates and triggers are consistent
				if (Game.GameData is IGameField gf && gf.pbEventCanReachPlayer(@event,this,distance) && triggers.Contains(@event.trigger)) {
					//  If starting determinant is front event (other than jumping)
					if (!@event.jumping && !@event.over_trigger) {
						result.Add(@event);
					}
				}
			}
			return result;
		}

		public IList<IGameEvent> pbTriggeredCounterEvents(int[] triggers,bool checkIfRunning=true) {
			IList<IGameEvent> result=new List<IGameEvent>();
			//  If event is running
			if (checkIfRunning && Game.GameData.GameSystem.map_interpreter.running) {
				return result;
			}
			//  All event loops
			foreach (IGameEvent @event in Game.GameData.GameMap.events.Values) {
				var regex = System.Text.RegularExpressions.Regex.Matches(@event.name, @"^Trainer\((\d+)\)$");
				if (regex == null) continue; //[/^Trainer\((\d+)\)$/]
				int distance = int.Parse(regex[1].Value);
				//  If event coordinates and triggers are consistent
				if (Game.GameData is IGameField gf && gf.pbEventFacesPlayer(@event,this,distance) && triggers.Contains(@event.trigger)) {
					//  If starting determinant is front event (other than jumping)
					if (!@event.jumping && !@event.over_trigger) {
						result.Add(@event);
					}
				}
			}
			return result;
		}

		public void pbCheckEventTriggerAfterTurning() {
		}

		public bool pbCheckEventTriggerFromDistance(int[] triggers) {
			List<IGameEvent> ret=(List<IGameEvent>)pbTriggeredTrainerEvents(triggers);
			ret.AddRange(pbTriggeredCounterEvents(triggers));
			if (ret.Count==0) return false;
			foreach (IGameEvent @event in ret) {
				@event.start();
			}
			return true;
		}

		public IGameEvent pbFacingEvent() {
			if (Game.GameData.GameSystem.map_interpreter.running) {
				return null;
			}
			float new_x = @x + (@direction == 6 ? 1 : @direction == 4 ? -1 : 0);
			float new_y = @y + (@direction == 2 ? 1 : @direction == 8 ? -1 : 0);
			foreach (var @event in Game.GameData.GameMap.events.Values) {
				if (@event.x == new_x && @event.y == new_y) {
					if (!@event.jumping && !@event.over_trigger) {
						return @event;
					}
				}
			}
			if (Game.GameData.GameMap.counter(new_x,new_y)) {
				new_x += (@direction == 6 ? 1 : @direction == 4 ? -1 : 0);
				new_y += (@direction == 2 ? 1 : @direction == 8 ? -1 : 0);
				foreach (var @event in Game.GameData.GameMap.events.Values) {
					if (@event.x == new_x && @event.y == new_y) {
						if (!@event.jumping && !@event.over_trigger) {
							return @event;
						}
					}
				}
			}
			return null;
		}
		/// <summary>
		/// Passable Determinants
		/// </summary>
		/// <param name="x">x-coordinate</param>
		/// <param name="y">y-coordinate</param>
		/// <param name="d">direction (0,2,4,6,8);
		/// 0 = Determines if all directions are impassable (for jumping)</param>
		/// <returns></returns>
		public override bool passable (float x,float y,int d) {
			//  Get new coordinates
			float new_x = x + (d == 6 ? 1 : d == 4 ? -1 : 0);
			float new_y = y + (d == 2 ? 1 : d == 8 ? -1 : 0);
			//  If coordinates are outside of map
			if (!Game.GameData.GameMap.validLax(new_x, new_y)) {
				//  Impassable
				return false;
			}
			if (!Game.GameData.GameMap.valid(new_x, new_y)) {
				if (Game.GameData.MapFactory == null) return false;
				return Game.GameData.MapFactory.isPassableFromEdge(new_x, new_y);
			}
			//  If debug mode is ON and ctrl key was pressed
			if (Core.DEBUG && Input.press(Input.CTRL)) {
				//  Passable
				return true;
			}
			return base.passable(x, y, d);
		}
		/// <summary>
		/// Set Map Display Position to Center of Screen
		/// </summary>
		/// <returns></returns>
		public void center(float x,float y) {
			//  X coordinate in the center of the screen
			float center_x = (Graphics.width/2 - Game_Map.TILEWIDTH/2) * Game_Map.XSUBPIXEL;
			//  Y coordinate in the center of the screen   
			float center_y = (Graphics.height/2 - Game_Map.TILEHEIGHT/2) * Game_Map.YSUBPIXEL;
			float max_x = (this.map.width - Graphics.width*1.0f/Game_Map.TILEWIDTH) * Game_Map.realResX;
			float max_y = (this.map.height - Graphics.height*1.0f/Game_Map.TILEHEIGHT) * Game_Map.realResY;
			float dispx=x * Game_Map.realResX - center_x;
			float dispy=y * Game_Map.realResY - center_y;
			this.map.display_x = dispx;
			this.map.display_y = dispy;
		}
		/// <summary>
		/// Move to Designated Position
		/// </summary>
		/// <param name="x">x-coordinate</param>
		/// <param name="y">y-coordinate</param>
		public override void moveto(float x,float y) {
			base.moveto(x,y);
			//  Centering
			center(x, y);
			//  Make encounter count
			make_encounter_count();
		}
		/// <summary>
		/// Get Encounter Count
		/// </summary>
		/// <returns></returns>
		//public int encounter_count() {
		//  return @encounter_count;
		//}
		/// <summary>
		/// Make Encounter Count
		/// </summary>
		/// <returns></returns>
		public void make_encounter_count() {
			//  Image of two dice rolling
			if (Game.GameData.GameMap.map_id != 0) {
				int n = Game.GameData.GameMap.encounter_step;
				@encounter_count = Core.Rand.Next(n) + Core.Rand.Next(n) + 1;
			}
		}
		/// <summary>
		/// Refresh
		/// </summary>
		/// <returns></returns>
		public void refresh() {
			@opacity = 255;
			@blend_type = 0;
		}
		/// <summary>
		/// Same Position Starting Determinant
		/// </summary>
		/// <returns></returns>
		public bool check_event_trigger_here(int[] triggers) {
			bool result = false;
			//  If event is running
			if (Game.GameData.GameSystem.map_interpreter.running) {
				return result;
			}
			//  All event loops
			foreach (var @event in Game.GameData.GameMap.events.Values) {
				//  If event coordinates and triggers are consistent
				if (@event.x == @x && @event.y == @y && triggers.Contains(@event.trigger)) {
					//  If starting determinant is same position event (other than jumping)
					if (!@event.jumping && @event.over_trigger) {
						@event.start();
						result = true;
					}
				}
			}
			return result;
		}
		/// <summary>
		/// Front Event Starting Determinant
		/// </summary>
		/// <returns></returns>
		public bool check_event_trigger_there(int[] triggers) {
			bool result = false;
			//  If event is running
			if (Game.GameData.GameSystem.map_interpreter.running) {
				return result;
			}
			//  Calculate front event coordinates
			float new_x = @x + (@direction == 6 ? 1 : @direction == 4 ? -1 : 0);
			float new_y = @y + (@direction == 2 ? 1 : @direction == 8 ? -1 : 0);
			//  All event loops
			foreach (var @event in Game.GameData.GameMap.events.Values) {
				//  If event coordinates and triggers are consistent
				if (@event.x == new_x && @event.y == new_y &&
					triggers.Contains(@event.trigger)) {
					//  If starting determinant is front event (other than jumping)
					if (!@event.jumping && !@event.over_trigger) {
						@event.start();
						result = true;
					}
				}
			}
			//  If fitting event is not found
			if (result == false) {
				//  If front tile is a counter
				if (Game.GameData.GameMap.counter(new_x, new_y)) {
					//  Calculate 1 tile inside coordinates
					new_x += (@direction == 6 ? 1 : @direction == 4 ? -1 : 0);
					new_y += (@direction == 2 ? 1 : @direction == 8 ? -1 : 0);
					//  All event loops
					foreach (var @event in Game.GameData.GameMap.events.Values) {
						//  If event coordinates and triggers are consistent
						if (@event.x == new_x && @event.y == new_y &&
							triggers.Contains(@event.trigger)) {
							//  If starting determinant is front event (other than jumping)
							if (!@event.jumping && !@event.over_trigger) {
								@event.start();
								result = true;
							}
						}
					}
				}
			}
			return result;
		}
		/// <summary>
		/// Touch Event Starting Determinant
		/// </summary>
		/// <returns></returns>
		public bool check_event_trigger_touch(float x,float y) {
			bool result = false;
			//  If event is running
			if (Game.GameData.GameSystem.map_interpreter.running) {
				return result;
			}
			//  All event loops
			int distance = 0;
			foreach (IGameEvent @event in Game.GameData.GameMap.events.Values) {
					if (@event.name!=null) { //[/^Trainer\((\d+)\)$/]
					distance=(int)~0;//[1].to_i;
					if (Game.GameData is IGameField gf && !gf.pbEventCanReachPlayer(@event,this,distance)) continue;
				}
				if (@event.name!=null) { //[/^Counter\((\d+)\)$/]
					distance=(int)~0;//[1].to_i;
					if (Game.GameData is IGameField gf && !gf.pbEventFacesPlayer(@event,this,distance)) continue;
				}
				//  If event coordinates and triggers are consistent
				if (@event.x == x && @event.y == y && new int[] { 1, 2 }.Contains(@event.trigger)) {
					//  If starting determinant is front event (other than jumping)
					if (!@event.jumping && !@event.over_trigger) {
						@event.start();
						result = true;
					}
				}
			}
			return result;
		}
		/// <summary>
		/// Frame Update
		/// </summary>
		public void update_old() {
			//  Remember whether or not moving in local variables
			bool last_moving = moving;
			//  If moving, event running, move route forcing, and message window
			//  display are all not occurring
			int dir=Input.dir4;
			if (!moving || !Game.GameData.GameSystem.map_interpreter.running ||
					!@move_route_forcing || !Game.GameData.GameTemp.message_window_showing ||
					!Game.GameData.PokemonTemp.miniupdate()) {
				//  Move player in the direction the directional button is being pressed
				if (dir==@lastdir && Graphics.frame_count-@lastdirframe>2) {
					switch (dir) {
						case 2:
							move_down();
							break;
						case 4:
							move_left();
							break;
						case 6:
							move_right();
							break;
						case 8:
							move_up();
							break;
					}
				} else if (dir!=@lastdir) {
					switch (dir) {
						case 2:
							turn_down();
							break;
						case 4:
							turn_left();
							break;
						case 6:
							turn_right();
							break;
						case 8:
							turn_up();
							break;
					}
				}
			}
			if (Game.GameData.PokemonTemp is ITempMetadataDependantEvents td) td.dependentEvents.updateDependentEvents();
			if (dir!=@lastdir) {
				@lastdirframe=Graphics.frame_count;
			}
			@lastdir=dir;
			//  Remember coordinates in local variables
			float last_real_x = @real_x;
			float last_real_y = @real_y;
			base.update();
			float center_x = (Graphics.width/2 - Game_Map.TILEWIDTH/2) * 
						Game_Map.XSUBPIXEL;   // Center screen x-coordinate * 4
			float center_y = (Graphics.height/2 - Game_Map.TILEHEIGHT/2) * 
						Game_Map.YSUBPIXEL;   // Center screen y-coordinate * 4
			//  If character moves down and is positioned lower than the center
			//  of the screen
			if (@real_y > last_real_y && @real_y - Game.GameData.GameMap.display_y > center_y) {
				//  Scroll map down
				Game.GameData.GameMap.scroll_down(@real_y - last_real_y);
			}
			//  If character moves left and is positioned more left on-screen than
			//  center
			if (@real_x < last_real_x && @real_x - Game.GameData.GameMap.display_x < center_x) {
				//  Scroll map left
				Game.GameData.GameMap.scroll_left(last_real_x - @real_x);
			}
			//  If character moves right and is positioned more right on-screen than
			//  center
			if (@real_x > last_real_x && @real_x - Game.GameData.GameMap.display_x > center_x) {
				//  Scroll map right
				Game.GameData.GameMap.scroll_right(@real_x - last_real_x);
			}
			//  If character moves up and is positioned higher than the center
			//  of the screen
			if (@real_y < last_real_y && @real_y - Game.GameData.GameMap.display_y < center_y) {
				//  Scroll map up
				Game.GameData.GameMap.scroll_up(last_real_y - @real_y);
			}
			//  Count down the time between allowed bump sounds
			if (@bump_se != null && @bump_se>0) @bump_se-=1;
			//  If not moving
			if (!moving) {
				//  If player was moving last time
				if (last_moving) {
					if (Game.GameData.PokemonTemp is ITempMetadataDependantEvents td1) td1.dependentEvents.pbTurnDependentEvents();
					bool result = pbCheckEventTriggerFromDistance(new int[] { 2 });
					//  Event determinant is via touch of same position event
					result |= check_event_trigger_here(new int[] { 1, 2 });
					//  If event which started does not exist
					Game.GameData.pbOnStepTaken(result); // *Added function call
				}
				//  If C button was pressed
				if (Input.trigger(Input.C) && !Game.GameData.PokemonTemp.miniupdate()) {
					//  Same position and front event determinant
					check_event_trigger_here(new int[] { 0 });
					check_event_trigger_there(new int[] { 0, 2 }); // *Modified to prevent unnecessary triggers
				}
			}
		}

		public void moveto2(float x,float y) {
			this.x = x;
			this.y = y;
			@real_x = @x * 128;
			@real_y = @y * 128;
			@prelock_direction = 0;
		}
		#endregion
	}
}