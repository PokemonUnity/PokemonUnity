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
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.RPGMaker.Kernal;

namespace PokemonUnity.Avatar
{
	/// <summary>
	/// </summary>
	public partial class GameEvent : Character, IGameEvent
	{
		#region Variables
		public int? trigger { get; protected set; }
		public IList<IEventCommand> list { get; protected set; }
		//public bool starting { get; protected set; }
		/// <summary>
		/// Temporary self-switches
		/// </summary>
		//public bool?[] tempSwitches { get; protected set; }
		public IDictionary<string,bool?> tempSwitches { get; protected set; }
		public bool need_refresh { get; set; }
		private IInterpreter interpreter;
		private IGameEvent @event;
		private IEventPage page;
		private bool route_erased;
		private bool erased;
		#endregion

		#region Constructor
		public GameEvent(int map_id, IGameEvent @event, IGameMap map = null) : base(map) {
			initialize(map_id, @event, map);
		}
		public IGameEvent initialize(int map_id, IGameEvent @event, IGameMap map = null) {
			base.initialize(map); //super(map);
			this.map_id = map_id;
			this.@event = @event;
			@id = @event.id;
			@erased = false;
			@starting = false;
			@need_refresh=false;
			@route_erased=false;
			@through = true;
			@tempSwitches=new Dictionary<string, bool?>();
			if (map != null) moveto(@event.x, @event.y);
			refresh();
			return this;
		}
		#endregion

		#region Methods
		public int map_id { get; set; }// {
		//  return @map_id;
		//} }

		public void clear_starting() {
			@starting = false;
		}

		public bool over_trigger { get {
			if (@character_name != "" && !@through) {
				return false;
			}
			if (@event.name=="HiddenItem") {
				return false;
			}
			if (!this.map.passable(@x, @y, 0)) {
				return false;
			}
			return true;
		} }

		public void start() {
			if (@list.size > 1) {
				@starting = true;
			}
		}

		public void erase() {
			@erased = true;
			refresh();
		}

		public void erase_route() {
			@route_erased=true;
			refresh();
		}

		public string name { get {
			return @event.name;
		} }

		public override int id { get {
			return @event.id;
		} }

		public void pbCheckEventTriggerAfterTurning() {
			if (Game.GameData.GameSystem.map_interpreter.running || @starting) {
				return;
			}
			if (@event.name != null) { //[/^Trainer\((\d+)\)$/]
				int distance=(int)int.Parse("first match"); //Regex name to int
				if (@trigger == 2 && Game.GameData is IGameField gf && gf.pbEventCanReachPlayer(this,Game.GameData.GamePlayer,distance)) {
					if (!jumping && !over_trigger) {
						start();
					}
				}
			}
		}

		public bool tsOn (string c) {
			return @tempSwitches != null && @tempSwitches[c]==true;
		}

		public bool tsOff (string c) {
			return @tempSwitches == null || @tempSwitches[c] == null;
		}

		public void setTempSwitchOn(string c) {
			@tempSwitches[c]=true;
			refresh();
		}

		public void setTempSwitchOff(string c) {
			@tempSwitches[c]=false;
			refresh();
		}

		public object variable { get {
			if (Game.GameData.Global.eventvars == null) return null;
			return Game.GameData.Global.eventvars[new KeyValuePair<int, int>(@map_id, @event.id)];
		} }

		public void setVariable(int variable) {
			Game.GameData.Global.eventvars[new KeyValuePair<int, int>(@map_id, @event.id)]=variable;
		}

		public int varAsInt() {
			if (Game.GameData.Global.eventvars == null) return 0;
			return (int)Game.GameData.Global.eventvars[new KeyValuePair<int, int>(@map_id, @event.id)];
		}

		public bool expired (int secs=86400) {
			int? ontime=(int)this.variable;
			DateTime time=Game.pbGetTimeNow();
			return ontime != null && (time.Ticks>ontime+secs);
		}

		public bool expiredDays (int days=1) {
			int? ontime=(int)this.variable;
			if (ontime == null) return false;
			DateTime now=Game.pbGetTimeNow();
			double elapsed=(TimeSpan.FromTicks(now.Ticks).TotalSeconds-ontime.Value)/86400;
			if ((TimeSpan.FromTicks(now.Ticks).TotalSeconds-ontime.Value)%86400>(now.Hour*3600+now.Minute*60+now.Second)) elapsed+=1;
			return elapsed>=days;
		}

		public bool onEvent() {
		return @map_id==Game.GameData.GameMap.map_id &&
			Game.GameData.GamePlayer.x==this.x && Game.GameData.GamePlayer.y==this.y;
		}
	
		public bool isOff (string c) {
			return !Game.GameData.GameSelfSwitches[new SelfSwitchVariable(@map_id, @event.id, c)];
		}

		public bool switchIsOn (int id) {
			string switchname=Game.GameData.DataSystem.switches[id];
			if (switchname == null) return false;
			//if (switchname[/^s\:/]) {
			//	return eval($~.post_match);
			//} else {
				return Game.GameData.GameSwitches[id];
			//}
		}

		public void refresh() {
			PokemonEssentials.Interface.RPGMaker.Kernal.IEventPage new_page = null;
			if (@erased) {
				foreach (IEventPage page in @event.pages.reverse) {
					IEventPageCondition c = page.condition;
					if (c.switch1_valid) {
						if (!switchIsOn(c.switch1_id)) {
							continue;
						}
					}
					if (c.switch2_valid) {
						if (!switchIsOn(c.switch2_id)) {
							continue;
						}
					}
					if (c.variable_valid) {
						if ((int)Game.GameData.GameVariables[c.variable_id] < c.variable_value) {
							continue;
						}
					}
					if (c.self_switch_valid) {
						ISelfSwitchVariable key = new SelfSwitchVariable(@map_id, @event.id, c.self_switch_ch.ToString());
						if (Game.GameData.GameSelfSwitches[key] != true) {
							continue;
						}
					}
					new_page = page;
					break;
				}
			}
			if (new_page == @page) {
				return;
			}
			@page = new_page;
			clear_starting();
			if (@page == null) {
				@tile_id = 0;
				@character_name = "";
				@character_hue = 0;
				@move_type = 0;
				@through = true;
				@trigger = null;
				@list = null;
				@interpreter = null;
				return;
			}
			@tile_id = @page.graphic.tile_id;
			@character_name = @page.graphic.character_name;
			@character_hue = @page.graphic.character_hue;
			if (@original_direction != @page.graphic.direction) {
				@direction = @page.graphic.direction;
				@original_direction = @direction;
				@prelock_direction = 0;
			}
			if (@original_pattern != @page.graphic.pattern) {
				@pattern = @page.graphic.pattern;
				@original_pattern = @pattern;
			}
			@opacity = @page.graphic.opacity;
			@blend_type = @page.graphic.blend_type;
			@move_type = @page.move_type;
			@move_speed = @page.move_speed * 1.25f;
			@move_frequency = @page.move_frequency;
			@move_route = @route_erased ? new RPG.MoveRoute() : @page.move_route;
			@move_route_index = 0;
			@move_route_forcing = false;
			@walk_anime = @page.walk_anime;
			@step_anime = @page.step_anime;
			@direction_fix = @page.direction_fix;
			@through = @page.through;
			@always_on_top = @page.always_on_top;
			@trigger = @page.trigger;
			@list = @page.list;
			@interpreter = null;
			if (@trigger == 4) {
				@interpreter = new Interpreter();
			}
			check_event_trigger_auto();
		}

		public void check_event_trigger_touch(float x,float y) {
			if (Game.GameData.GameSystem.map_interpreter.running) {
				return;
			}
			if (@trigger!=2) return;
			if (x != Game.GameData.GamePlayer.x || y != Game.GameData.GamePlayer.y) return;
			if (!jumping && !over_trigger) {
				start();
			}
		}

		public void check_event_trigger_auto() {
			if (@trigger == 2 && @x == Game.GameData.GamePlayer.x && @y == Game.GameData.GamePlayer.y) {
				if (!jumping && over_trigger) {
				  start();
				}
			}
			if (@trigger == 3) {
				start();
			}
		}

		public override void update() {
			bool last_moving=moving;
			base.update();
			if (!moving && last_moving) {
				Game.GameData.GamePlayer.pbCheckEventTriggerFromDistance(new int[] { 2 });
			}
			if (@need_refresh) {
				@need_refresh=false;
				refresh();
			}
			check_event_trigger_auto();
			if (@interpreter != null) {
				if (!@interpreter.running) {
					@interpreter.setup(@list, @event.id, @map_id);
				}
				@interpreter.update();
			}
		}
		#endregion
	}
}