using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Attack;
using PokemonUnity.Inventory;
using PokemonUnity.Saving.SerializableClasses;
using PokemonUnity.Character;
using PokemonEssentials.Interface;
using System.Text.RegularExpressions;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Battle;

namespace PokemonUnity//.Character
{
	// ===============================================================================
	// ** Game_Player
	// -------------------------------------------------------------------------------
	// ===============================================================================
	/// <summary>
	/// This class handles the player. Its functions include event starting
	/// determinants and map scrolling.
	/// </summary>
	/// <remarks>
	/// There is only one `IGamePlayer`, everything else is an `IGameEvent` or `IGameCharacter`.
	/// Refer to <see cref="IGame.GamePlayer"/> for the one instance of this class.
	/// </remarks>
	public partial class GamePlayer : GameCharacter, IGamePlayer {
		//private IGameMap map;
		private int lastdir;
		private int lastdirframe;
		private int? bump_se;
		IAudioSE IGamePlayer.bump_se { get; set; }

		//IGameMap IGameCharacter.map { get {
		//		//@map = null;
		//		return Game.GameData.GameMap;
		//	} set { map = value; }
		//}

		public GamePlayer(IGameMap map) { initialize(map); } //: base(map)

		new public IGamePlayer initialize (IGameMap map) {
			base.initialize(map); //super(*arg);
			@lastdir = 0;
			@lastdirframe = 0;
			@bump_se = 0;
			return this;
		}

		public override int bush_depth
		{
			get
			{
				if (@tile_id > 0 || @always_on_top) return 0;
				float xbehind = (@direction == 4) ? @x + 1 : (@direction == 6) ? @x - 1 : @x;
				float ybehind = (@direction == 8) ? @y + 1 : (@direction == 2) ? @y - 1 : @y;
				if (!Game.GameData.GameMap.valid(@x, @y) || !Game.GameData.GameMap.valid(xbehind, ybehind))
				{
					if (Game.GameData.MapFactory==null) return 0;
					IMapChunkUnit newhere = Game.GameData.MapFactory.getNewMap(@x, @y);
					IMapChunkUnit newbehind = Game.GameData.MapFactory.getNewMap(xbehind, ybehind);
					IGameMap heremap = this.map; float herex = @x; float herey = @y;
					IGameMap behindmap = this.map; float behindx = @x; float behindy = @y;
					if (Game.GameData.GameMap.valid(@x, @y))
					{
						heremap = this.map; herex = @x; herey = @y;
					}
					else if (newhere != null && newhere.MapId != null) //[0]
					{
						//heremap = newhere[0]; herex = newhere[1]; herey = newhere[2];
						if (heremap is IGameMapOrgBattle gmo) gmo.map_id = newhere.MapId; herex = newhere.X; herey = newhere.Y;
					}
					else
					{
						return 0;
					}
					if (Game.GameData.GameMap.valid(xbehind, ybehind))
					{
						behindmap = this.map; behindx = xbehind; behindy = ybehind;
					}
					else if (newbehind != null && newbehind.MapId != null) //[0]
					{
						//behindmap = newbehind[0]; behindx = newbehind[1]; behindy = newbehind[2];
						if (behindmap is IGameMapOrgBattle gmo) gmo.map_id = newbehind.MapId; behindx = newbehind.X; behindy = newbehind.Y;
					}
					else
					{
						return 0;
					}
					if (@jump_count <= 0 && heremap.deepBush(herex, herey) &&
						behindmap.deepBush(behindx, behindy))
					{
						return 32;
					}
					else if (@jump_count <= 0 && heremap.bush(herex, herey) && !moving)
					{
						return 12;
					}
					else
					{
						return 0;
					}
				}
				else
				{
					return base.bush_depth;
				}
			}
		}

		public bool HasDependentEvents() {
			return Game.GameData.Global is IGlobalMetadataDependantEvents gmd ? gmd.dependentEvents.Count > 0 : false;
		}

		public override void move_down(bool turn_enabled = true) {
			if (turn_enabled) {
				turn_down();
			}
			if (passable(@x, @y, 2)) {
				if (Game.GameData is IGameField gf && gf.Ledge(0, 1)) return;
				if (Game.GameData is IGameHiddenMoves gh && gh.EndSurf(0, 1)) return;
				turn_down();
				@y += 1;
				if (Game.GameData.PokemonTemp is ITempMetadataDependantEvents tmd) tmd.dependentEvents.MoveDependentEvents();
				increase_steps();
			} else {
				if (!check_event_trigger_touch(@x, @y + 1)) {
					if ((@bump_se == null || @bump_se.Value <= 0) && Game.GameData is IGameAudioPlay gap) {
						if (Game.FileTest.ResourcesAudio.SoundEffectBump != null)
							gap.SEPlay(Game.FileTest.ResourcesAudio.SoundEffectBump);
						else gap.SEPlay("bump"); @bump_se = 10;
					}
				}
			}
		}

		public override void move_left(bool turn_enabled = true) {
			if (turn_enabled) {
				turn_left();
			}
			if (passable(@x, @y, 4)) {
				if (Game.GameData is IGameField gf && gf.Ledge(-1, 0)) return;
				if (Game.GameData is IGameHiddenMoves gh && gh.EndSurf(-1, 0)) return;
				turn_left();
				@x -= 1;
				if (Game.GameData.PokemonTemp is ITempMetadataDependantEvents tmd) tmd.dependentEvents.MoveDependentEvents();
				increase_steps();
			} else {
				if (!check_event_trigger_touch(@x - 1, @y)) {
					if ((@bump_se == null || @bump_se.Value <= 0) && Game.GameData is IGameAudioPlay gap) {
						if (Game.FileTest.ResourcesAudio.SoundEffectBump != null)
							gap.SEPlay(Game.FileTest.ResourcesAudio.SoundEffectBump);
						else gap.SEPlay("bump"); @bump_se = 10;
					}
				}
			}
		}

		public override void move_right(bool turn_enabled = true) {
			if (turn_enabled) {
				turn_right();
			}
			if (passable(@x, @y, 6)) {
				if (Game.GameData is IGameField gf && gf.Ledge(1, 0)) return;
				if (Game.GameData is IGameHiddenMoves gh && gh.EndSurf(1, 0)) return;
				turn_right();
				@x += 1;
				if (Game.GameData.PokemonTemp is ITempMetadataDependantEvents tmd) tmd.dependentEvents.MoveDependentEvents();
				increase_steps();
			} else {
				if (!check_event_trigger_touch(@x + 1, @y)) {
					if ((@bump_se == null || @bump_se.Value <= 0) && Game.GameData is IGameAudioPlay gap) {
						if (Game.FileTest.ResourcesAudio.SoundEffectBump != null)
							gap.SEPlay(Game.FileTest.ResourcesAudio.SoundEffectBump);
						else gap.SEPlay("bump"); @bump_se = 10;
					}
				}
			}
		}

		public override void move_up(bool turn_enabled = true) {
			if (turn_enabled) {
				turn_up();
			}
			if (passable(@x, @y, 8)) {
				if (Game.GameData is IGameField gf && gf.Ledge(0, -1)) return;
				if (Game.GameData is IGameHiddenMoves gh && gh.EndSurf(0, -1)) return;
				turn_up();
				@y -= 1;
				if (Game.GameData.PokemonTemp is ITempMetadataDependantEvents tmd) tmd.dependentEvents.MoveDependentEvents();
				increase_steps();
			} else {
				if (!check_event_trigger_touch(@x, @y - 1)) {
					if ((@bump_se == null || @bump_se.Value <= 0) && Game.GameData is IGameAudioPlay gap) {
						if (Game.FileTest.ResourcesAudio.SoundEffectBump != null)
							gap.SEPlay(Game.FileTest.ResourcesAudio.SoundEffectBump);
						else gap.SEPlay("bump"); @bump_se = 10;
					}
				}
			}
		}

		public IList<IGameCharacter> TriggeredTrainerEvents(IList<int> triggers, bool checkIfRunning = true) {
			IList<IGameCharacter> result = new List<IGameCharacter>();
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
				if (Game.GameData is IGameField gf && gf.EventCanReachPlayer(@event, this, distance) && triggers.Contains(@event.trigger)) {
					//  If starting determinant is front event (other than jumping)
					if (!@event.jumping && !@event.over_trigger) {
						result.Add(@event);
					}
				}
			}
			return result;
		}

		public IList<IGameCharacter> TriggeredCounterEvents(IList<int> triggers, bool checkIfRunning = true) {
			IList<IGameCharacter> result = new List<IGameCharacter>();
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
				if (Game.GameData is IGameField gf && gf.EventFacesPlayer(@event, this, distance) && triggers.Contains(@event.trigger)) {
					//  If starting determinant is front event (other than jumping)
					if (!@event.jumping && !@event.over_trigger) {
						result.Add(@event);
					}
				}
			}
			return result;
		}

		public void CheckEventTriggerAfterTurning() {
		}

		public bool CheckEventTriggerFromDistance(IList<int> triggers) {
			List<IGameCharacter> ret = (List<IGameCharacter>)TriggeredTrainerEvents(triggers);
			ret.AddRange(TriggeredCounterEvents(triggers));
			if (ret.Count == 0) return false;
			foreach (IGameEvent @event in ret) {
				@event.start();
			}
			return true;
		}

		public IGameCharacter FacingEvent() {
			if (Game.GameData.GameSystem.map_interpreter.running) {
				return null;
			}
			float new_x = @x + (@direction == 6 ? 1 : @direction == 4 ? -1 : 0);
			float new_y = @y + (@direction == 2 ? 1 : @direction == 8 ? -1 : 0);
			foreach (IGameEvent @event in Game.GameData.GameMap.events.Values) {
				if (@event.x == new_x && @event.y == new_y) {
					if (!@event.jumping && !@event.over_trigger) {
						return @event;
					}
				}
			}
			if (Game.GameData.GameMap.counter(new_x, new_y)) {
				new_x += (@direction == 6 ? 1 : @direction == 4 ? -1 : 0);
				new_y += (@direction == 2 ? 1 : @direction == 8 ? -1 : 0);
				foreach (IGameEvent @event in Game.GameData.GameMap.events.Values) {
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
		/// <param name="d">direction (0,2,4,6,8)
		///					* 0 = Determines if all directions are impassable (for jumping)
		/// </param>
		/// <returns></returns>
		public override bool passable(float x, float y, int d) {
			//  Get new coordinates
			float new_x = x + (d == 6 ? 1 : d == 4 ? -1 : 0);
			float new_y = y + (d == 2 ? 1 : d == 8 ? -1 : 0);
			//  If coordinates are outside of map
			if (!Game.GameData.GameMap.validLax(new_x, new_y)) {
				//  Impassable
				return false;
			}
			if (!Game.GameData.GameMap.valid(new_x, new_y)) {
				if (Game.GameData.MapFactory==null) return false;
				return Game.GameData.MapFactory.isPassableFromEdge(new_x, new_y);
			}
			//  If debug mode is ON and ctrl key was pressed
			if (Core.DEBUG && Input.press(Input.CTRL)) {
				//  Passable
				return true;
			}
			return base.passable(x, y, d);
		}
		// -----------------------------------------------------------------------------
		//  * Set Map Display Position to Center of Screen
		// -----------------------------------------------------------------------------
		public void center(float x, float y) {
			//  X coordinate in the center of the screen
			int center_x = (int)((Game.GameData.Graphics.width / 2 - Game_Map.TILEWIDTH / 2) * Game_Map.XSUBPIXEL);
			//  Y coordinate in the center of the screen
			int center_y = (int)((Game.GameData.Graphics.height / 2 - Game_Map.TILEHEIGHT / 2) * Game_Map.YSUBPIXEL);
			int max_x = (int)((this.map.width - Game.GameData.Graphics.width * 1.0f / Game_Map.TILEWIDTH) * Game_Map.realResX);
			int max_y = (int)((this.map.height - Game.GameData.Graphics.height * 1.0f / Game_Map.TILEHEIGHT) * Game_Map.realResY);
			int dispx = (int)(x * Game_Map.realResX - center_x);
			int dispy = (int)(y * Game_Map.realResY - center_y);
			this.map.display_x = dispx;
			this.map.display_y = dispy;
		}
		// -----------------------------------------------------------------------------
		//  * Move to Designated Position
		//      x : x-coordinate
		//      y : y-coordinate
		// -----------------------------------------------------------------------------
		public override void moveto(float x, float y) {
			base.moveto(x,y);
			//  Centering
			center(x, y);
			//  Make encounter count
			make_encounter_count();
		}
		// -----------------------------------------------------------------------------
		//  * Get Encounter Count
		// -----------------------------------------------------------------------------
		public int encounter_count { get; set; }
		//{
		//    return @encounter_count;
		//}
		// -----------------------------------------------------------------------------
		//  * Make Encounter Count
		// -----------------------------------------------------------------------------
		public void make_encounter_count() {
			//  Image of two dice rolling
			if (Game.GameData.GameMap is IGameMapOrgBattle gmo && gmo.map_id != 0) {
				int n = Game.GameData.GameMap.encounter_step;
				@encounter_count = Core.Rand.Next(n) + Core.Rand.Next(n) + 1;
			}
		}
		// -----------------------------------------------------------------------------
		//  * Refresh
		// -----------------------------------------------------------------------------
		public void refresh() {
			@opacity = 255;
			@blend_type = 0;
		}
		// -----------------------------------------------------------------------------
		//  * Same Position Starting Determinant
		// -----------------------------------------------------------------------------
		public bool check_event_trigger_here(IList<int> triggers) {
			bool result = false;
			//  If event is running
			if (Game.GameData.GameSystem.map_interpreter.running) {
				return result;
			}
			//  All event loops
			foreach (IGameEvent @event in Game.GameData.GameMap.events.Values) {
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
		// -----------------------------------------------------------------------------
		//  * Front Event Starting Determinant
		// -----------------------------------------------------------------------------
		public bool check_event_trigger_there(IList<int> triggers) {
			bool result = false;
			//  If event is running
			if (Game.GameData.GameSystem.map_interpreter.running) {
				return result;
			}
			//  Calculate front event coordinates
			float new_x = @x + (@direction == 6 ? 1 : @direction == 4 ? -1 : 0);
			float new_y = @y + (@direction == 2 ? 1 : @direction == 8 ? -1 : 0);
			//  All event loops
			foreach (IGameEvent @event in Game.GameData.GameMap.events.Values) {
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
					foreach (IGameEvent @event in Game.GameData.GameMap.events.Values) {
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
		// -----------------------------------------------------------------------------
		//  * Touch Event Starting Determinant
		// -----------------------------------------------------------------------------
		public bool check_event_trigger_touch(float x, float y) {
			bool result = false;
			//  If event is running
			if (Game.GameData.GameSystem.map_interpreter.running) {
				return result;
			}
			//  All event loops
			int distance = 0;
			foreach (IGameEvent @event in Game.GameData.GameMap.events.Values) {
				Match match = Regex.Match(@event.name, @"^Trainer\((\d+)\)$");
				if (match.Success) { //@event.name | [/^Trainer\((\d+)\)$/]
					distance = int.Parse(match.Groups[1].Value); //$~[1].to_i;
					if (Game.GameData is IGameField gf && !gf.EventCanReachPlayer(@event, this, distance)) continue;
				}
				match = Regex.Match(@event.name, @"^Trainer\((\d+)\)$");
				if (match.Success) { //@event.name | [/^Counter\((\d+)\)$/]
					distance = int.Parse(match.Groups[1].Value); //$~[1].to_i;
					if (Game.GameData is IGameField gf && !gf.EventFacesPlayer(@event, this, distance)) continue;
				}
				//  If event coordinates and triggers are consistent
				if (@event.x == x && @event.y == y && new List<int>{ 1, 2 }.Contains(@event.trigger)) {
					//  If starting determinant is front event (other than jumping)
					if (!@event.jumping && !@event.over_trigger) {
						@event.start();
						result = true;
					}
				}
			}
			return result;
		}
		// -----------------------------------------------------------------------------
		//  * Frame Update
		// -----------------------------------------------------------------------------
		private void update_old() {
			//  Remember whether or not moving in local variables
			bool last_moving = moving;
			//  If moving, event running, move route forcing, and message window
			//  display are all not occurring
			int dir = Input.dir4();
			if (!moving || !Game.GameData.GameSystem.map_interpreter.running ||
				!@move_route_forcing || !Game.GameData.GameTemp.message_window_showing ||
				Game.GameData.PokemonTemp.miniupdate==null) {
				//  Move player in the direction the directional button is being pressed
				if (dir == @lastdir && Game.GameData.Graphics.frame_count - @lastdirframe > 2) {
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
				} else if (dir != @lastdir) {
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
			if (Game.GameData.PokemonTemp is ITempMetadataDependantEvents tmd) tmd.dependentEvents.updateDependentEvents();
			if (dir != @lastdir) {
				@lastdirframe = Game.GameData.Graphics.frame_count;
			}
			@lastdir = dir;
			//  Remember coordinates in local variables
			float last_real_x = @real_x;
			float last_real_y = @real_y;
			base.update();
			float center_x = (Game.GameData.Graphics.width / 2 - Game_Map.TILEWIDTH / 2) *
				Game_Map.XSUBPIXEL;   // Center screen x-coordinate * 4
			float center_y = (Game.GameData.Graphics.height / 2 - Game_Map.TILEHEIGHT / 2) *
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
			if (@bump_se != null && @bump_se.Value > 0) @bump_se -= 1;
			//  If not moving
			if (!moving) {
				//  If player was moving last time
				if (last_moving) {
					if (Game.GameData.PokemonTemp is ITempMetadataDependantEvents tmd0) tmd0.dependentEvents.TurnDependentEvents();
					bool result = CheckEventTriggerFromDistance(new int[] { 2 });
					//  Event determinant is via touch of same position event
					result |= check_event_trigger_here(new int[] { 1, 2 });
					//  If event which started does not exist
					//Kernal.OnStepTaken(result); // *Added function call
					if (Game.GameData is IGameField gf) gf.OnStepTaken(result); // *Added function call
				}
				//  If C button was pressed
				if (Input.trigger(Input.C) && Game.GameData.PokemonTemp.miniupdate==null) {
					//  Same position and front event determinant
					check_event_trigger_here(new int[] { 0 });
					check_event_trigger_there(new int[] { 0, 2 }); // *Modified to prevent unnecessary triggers
				}
			}
		}
	}
}