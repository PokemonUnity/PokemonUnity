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
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.RPGMaker.Kernal;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Battle;

namespace PokemonUnity//.Character
{
	public partial class GameCharacter : IGameCharacter //IEntity
	{
		public int id { get; set; }
		public float x { get; set; }
		public float y { get; set; }
		public float real_x { get; set; }
		public float real_y { get; set; }
		public int tile_id { get; set; }
		public virtual string character_name { get; set; }
		public int character_hue { get; set; }
		public int opacity { get; set; }
		public int blend_type { get; set; }
		public int direction { get; set; }
		public int pattern { get; set; }
		public bool move_route_forcing { get; set; }
		public bool through { get; set; }
		public int animation_id { get; set; }
		public bool transparent { get; set; }
		//public virtual IGameMap map { get; set; }
		public float move_speed { get; set; }
		public bool walk_anime { get; set; }

		public bool step_anime { get; set; }
		public float anime_count { get; set; }
		public int jump_count { get; set; }
		public int stop_count { get; set; }
		public float jump_peak { get; set; }
		public int wait_count { get; set; }
		public int move_route_index { get; set; }
		public bool direction_fix { get; set; }
		public bool locked { get; set; }
		public bool always_on_top { get; set; }
		public float? oldX { get; set; }
		public float? oldY { get; set; }
		public int? oldMap { get; set; }
		public IMoveRoute move_route { get; set; }
		public IMoveRoute original_move_route { get; set; }
		public int? original_move_route_index { get; set; }
		public int prelock_direction { get; set; }
		public bool starting { get; set; }
		//protected bool starting;
		protected int? original_direction;
		protected int? original_pattern;
		protected int? move_type;
		protected int? move_frequency;
		protected IGameMap _map;

		public virtual IGameMap map { get {
			//Player temporary location without affecting actual player location
			return _map != null ? @map : Game.GameData.GameMap;
		} }

		public GameCharacter(IGameMap map = null) { initialize(map); }

		public IGameCharacter initialize(IGameMap map = null)
		{
			_map = map;
			@id = 0;
			@x = 0;
			@y = 0;
			@real_x = 0;
			@real_y = 0;
			@tile_id = 0;
			@character_name = "";
			@character_hue = 0;
			@opacity = 255;
			@blend_type = 0;
			@direction = 2;
			@pattern = 0;
			@move_route_forcing = false;
			@through = false;
			@animation_id = 0;
			@transparent = false;
			@original_direction = 2;
			@original_pattern = 0;
			@move_type = 0;
			@move_speed = 4;
			@move_frequency = 6;
			@move_route = null;
			@move_route_index = 0;
			@original_move_route = null;
			@original_move_route_index = 0;
			@walk_anime = true;
			@step_anime = false;
			@direction_fix = false;
			@always_on_top = false;
			@anime_count = 0;
			@stop_count = 0;
			@jump_count = 0;
			@jump_peak = 0;
			@wait_count = 0;
			@locked = false;
			@prelock_direction = 0;
			return this;
		}

		public bool moving
		{
			get
			{
				return (@real_x != @x * 4 * Game_Map.TILEWIDTH || @real_y != @y * 4 * Game_Map.TILEHEIGHT);
			}
		}

		public bool jumping
		{
			get
			{
				return @jump_count > 0;
			}
		}

		//public void pattern { set {
		//  @pattern=value;
		//} }

		public void straighten()
		{
			if (@walk_anime || @step_anime)
			{
				@pattern = 0;
			}
			@anime_count = 0;
			@prelock_direction = 0;
		}

		public void force_move_route(IMoveRoute move_route)
		{
			if (@original_move_route == null)
			{
				@original_move_route = @move_route;
				@original_move_route_index = @move_route_index;
			}
			this.move_route = move_route;
			@move_route_index = 0;
			@move_route_forcing = true;
			@prelock_direction = 0;
			@wait_count = 0;
			move_type_custom();
		}

		public bool passableEx(float x, float y, int d, bool strict = false)
		{
			float new_x = x + (d == 6 ? 1 : d == 4 ? -1 : 0);
			float new_y = y + (d == 2 ? 1 : d == 8 ? -1 : 0);
			if (!this.map.valid(new_x, new_y)) return false;
			if (@through) return true;
			if (strict)
			{
				if (!this.map.passableStrict(x, y, d, this)) return false;
				if (!this.map.passableStrict(new_x, new_y, 10 - d, this)) return false;
			}
			else
			{
				if (!this.map.passable(x, y, d, this)) return false;
				if (!this.map.passable(new_x, new_y, 10 - d, this)) return false;
			}
			foreach (IGameEvent @event in this.map.events.Values)
			{
				if (@event.x == new_x && @event.y == new_y)
				{
					if (!@event.through)
					{
						if (this != Game.GameData.GamePlayer || @event.character_name != "") return false;
					}
				}
			}
			if (Game.GameData.GamePlayer.x == new_x && Game.GameData.GamePlayer.y == new_y)
			{
				if (!Game.GameData.GamePlayer.through)
				{
					if (@character_name != "") return false;
				}
			}
			return true;
		}

		public virtual bool passable(float x, float y, int d)
		{
			return passableEx(x, y, d, false);
		}

		public bool passableStrict(float x, float y, int d)
		{
			return passableEx(x, y, d, true);
		}

		public void _lock()
		{
			if (@locked) return;
			@prelock_direction = @direction;
			turn_toward_player();
			@locked = true;
		}

		public bool islock
		{
			get
			{
				return @locked;
			}
		}

		public void unlock()
		{
			if (!@locked) return;
			@locked = false;
			if (!@direction_fix)
			{
				if (@prelock_direction != 0)
				{
					@direction = @prelock_direction;
				}
			}
		}

		public void triggerLeaveTile()
		{
			if (@oldX != null && @oldY != null && @oldMap != null &&
			   (@oldX != this.x || @oldY != this.y || (this.map is IGameMapOrgBattle gmo && @oldMap != gmo.map_id)))
			{
				//Events.onLeaveTile.trigger(this,this,@oldMap,@oldX,@oldY);
			}
			@oldX = this.x;
			@oldY = this.y;
			if (this.map is IGameMapOrgBattle gmob)
				@oldMap = gmob.map_id;
		}

		public virtual void moveto(float x, float y)
		{
			this.x = x % this.map.width;
			this.y = y % this.map.height;
			@real_x = @x * Game_Map.realResX;
			@real_y = @y * Game_Map.realResY;
			@prelock_direction = 0;
			triggerLeaveTile();
		}

		public int screen_x
		{
			get
			{
				return (int)(@real_x - this.map.display_x + 3) / 4 + (Game_Map.TILEWIDTH / 2);
			}
		}

		public int screen_y
		{
			get
			{
				float y = (@real_y - this.map.display_y + 3) / 4 + (Game_Map.TILEHEIGHT);
				if (jumping)
				{
					int n = (int)Math.Abs(@jump_count - @jump_peak);
					return (int)(y - (@jump_peak * @jump_peak - n * n) / 2);
				}
				return (int)y;
			}
		}

		public int screen_y_ground()
		{
			return (int)(@real_y - this.map.display_y + 3) / 4 + (Game_Map.TILEHEIGHT);
		}

		public int screen_z(int height = 0)
		{
			if (@always_on_top)
			{
				return 999;
			}
			float z = (@real_y - this.map.display_y + 3) / 4 + 32;
			if (@tile_id > 0)
			{
				return (int)z + this.map.priorities[@tile_id] * 32;
			}
			//  Add z if height exceeds 32
			return (int)z + ((height > 32) ? 31 : 0);
		}

		public virtual int bush_depth
		{
			get
			{
				if (@tile_id > 0 || @always_on_top)
				{
					return 0;
				}
				if (@jump_count <= 0)
				{
					float xbehind = (@direction == 4) ? @x + 1 : (@direction == 6) ? @x - 1 : @x;
					float ybehind = (@direction == 8) ? @y + 1 : (@direction == 2) ? @y - 1 : @y;
					if (this.map.deepBush(@x, @y) && this.map.deepBush(xbehind, ybehind))
					{
						return 32;
					}
					if (this.map.bush(@x, @y) && !moving)
					{
						return 12;
					}
				}
				return 0;
			}
		}

		public Overworld.Terrains terrain_tag
		{
			get
			{
				return this.map.terrain_tag(@x, @y);
			}
		}

		// Updating stuff ###############################################################
		public virtual IEnumerator update()
		{
			if (Game.GameData.GameTemp.in_menu) yield break;
			if (jumping)
			{
				update_jump();
			}
			else if (moving)
			{
				update_move();
			}
			else
			{
				update_stop();
			}
			if (@wait_count > 0)
			{
				@wait_count -= 1;
			}
			else if (@move_route_forcing)
			{
				move_type_custom();
			}
			else if (!@starting && !islock)
			{
				if (@stop_count > (40 - @move_frequency * 2) * (6 - @move_frequency))
				{
					switch (@move_type)
					{
						case 1:
							move_type_random();
							break;
						case 2:
							move_type_toward_player();
							break;
						case 3:
							move_type_custom();
							break;
					}
				}
			}
			if (!@step_anime && @stop_count > 0) @anime_count = 20;
			if (@anime_count > 18 - @move_speed * 3)
			{
				if (!@step_anime && @stop_count > 0)
				{
					@pattern = @original_pattern.Value;
				}
				else
				{
					@pattern = (@pattern + 1) % 4;
				}
				@anime_count = 0;
			}
		}

		public void update_jump()
		{
			@jump_count -= 1;
			@real_x = (@real_x * @jump_count + @x * Game_Map.realResX) / (@jump_count + 1);
			@real_y = (@real_y * @jump_count + @y * Game_Map.realResY) / (@jump_count + 1);
			if (!jumping && !moving)
			{
				//Events.onStepTakenFieldMovement.trigger(this, this);
			}
		}

		public void update_move()
		{
			int distance = (int)Math.Pow(2, @move_speed);
			int realResX = Game_Map.realResX;
			int realResY = Game_Map.realResY;
			if (@x * realResX < @real_x)
			{
				@real_x = Math.Max(@real_x - distance, @x * realResX);
			}
			if (@x * realResX > @real_x)
			{
				@real_x = Math.Min(@real_x + distance, @x * realResX);
			}
			if (@y * realResY > @real_y)
			{
				@real_y = Math.Min(@real_y + distance, @y * realResY);
			}
			if (@y * realResY < @real_y)
			{
				@real_y = Math.Max(@real_y - distance, @y * realResY);
			}
			if (!jumping && !moving)
			{
				//Events.onStepTakenFieldMovement.trigger(this,this);
			}
			if (@walk_anime)
			{
				@anime_count += 1.5f;
			}
			else if (@step_anime)
			{
				@anime_count += 1;
			}
		}

		public void update_stop()
		{
			if (@step_anime)
			{
				@anime_count += 1;
			}
			if (!@starting || !islock)
			{
				@stop_count += 1;
			}
		}

		public void move_type_random()
		{
			switch (Core.Rand.Next(6))
			{
				case 0:
				case 1:
				case 2:
				case 3:
					move_random();
					break;
				case 4:
					move_forward();
					break;
				case 5:
					@stop_count = 0;
					break;
			}
		}

		public void move_type_toward_player()
		{
			float sx = @x - Game.GameData.GamePlayer.x;
			float sy = @y - Game.GameData.GamePlayer.y;
			if (Math.Abs(sx) + Math.Abs(sy) >= 20)
			{
				move_random();
				return;
			}
			switch (Core.Rand.Next(6))
			{
				case 0:
				case 1:
				case 2:
				case 3:
					move_toward_player();
					break;
				case 4:
					move_random();
					break;
				case 5:
					move_forward();
					break;
			}
		}

		public void move_type_custom()
		{
			if (jumping || moving)
			{
				return;
			}
			while (@move_route_index < @move_route.list.Count)
			{
				IMoveCommand command = @move_route.list[@move_route_index];
				if (command.code == 0)
				{
					if (@move_route.repeat)
					{
						@move_route_index = 0;
					}
					if (!@move_route.repeat)
					{
						if (@move_route_forcing && !@move_route.repeat)
						{
							@move_route_forcing = false;
							@move_route = @original_move_route;
							@move_route_index = @original_move_route_index.Value;
							@original_move_route = null;
						}
						@stop_count = 0;
					}
					return;
				}
				if (command.code <= 14)
				{
					switch (command.code)
					{
						case 1:
							move_down();
							break;
						case 2:
							move_left();
							break;
						case 3:
							move_right();
							break;
						case 4:
							move_up();
							break;
						case 5:
							move_lower_left();
							break;
						case 6:
							move_lower_right();
							break;
						case 7:
							move_upper_left();
							break;
						case 8:
							move_upper_right();
							break;
						case 9:
							move_random();
							break;
						case 10:
							move_toward_player();
							break;
						case 11:
							move_away_from_player();
							break;
						case 12:
							move_forward();
							break;
						case 13:
							move_backward();
							break;
						case 14:
							jump((int)command.parameters[0], (int)command.parameters[1]);
							break;
					}
					if (@move_route.skippable || moving || jumping)
					{
						@move_route_index += 1;
					}
					return;
				}
				if (command.code == 15)
				{
					@wait_count = (int)command.parameters[0] * 2 - 1;
					@move_route_index += 1;
					return;
				}
				if (command.code >= 16 && command.code <= 26)
				{
					switch (command.code)
					{
						case 16:
							turn_down();
							break;
						case 17:
							turn_left();
							break;
						case 18:
							turn_right();
							break;
						case 19:
							turn_up();
							break;
						case 20:
							turn_right_90();
							break;
						case 21:
							turn_left_90();
							break;
						case 22:
							turn_180();
							break;
						case 23:
							turn_right_or_left_90();
							break;
						case 24:
							turn_random();
							break;
						case 25:
							turn_toward_player();
							break;
						case 26:
							turn_away_from_player();
							break;
					}
					@move_route_index += 1;
					return;
				}
				if (command.code >= 27)
				{
					switch (command.code)
					{
						case 27:
							Game.GameData.GameSwitches[(int)command.parameters[0]] = true;
							this.map.need_refresh = true;
							break;
						case 28:
							Game.GameData.GameSwitches[(int)command.parameters[0]] = false;
							this.map.need_refresh = true;
							break;
						case 29:
							@move_speed = (int)command.parameters[0];
							break;
						case 30:
							@move_frequency = (int)command.parameters[0];
							break;
						case 31:
							@walk_anime = true;
							break;
						case 32:
							@walk_anime = false;
							break;
						case 33:
							@step_anime = true;
							break;
						case 34:
							@step_anime = false;
							break;
						case 35:
							@direction_fix = true;
							break;
						case 36:
							@direction_fix = false;
							break;
						case 37:
							@through = true;
							break;
						case 38:
							@through = false;
							break;
						case 39:
							@always_on_top = true;
							break;
						case 40:
							@always_on_top = false;
							break;
						case 41:
							@tile_id = 0;
							@character_name = (string)command.parameters[0];
							@character_hue = (int)command.parameters[1];
							if (@original_direction != (int)command.parameters[2])
							{
								@direction = (int)command.parameters[2];
								@original_direction = @direction;
								@prelock_direction = 0;
							}
							if (@original_pattern != (int)command.parameters[3])
							{
								@pattern = (int)command.parameters[3];
								@original_pattern = @pattern;
							}
							break;
						case 42:
							@opacity = (int)command.parameters[0];
							break;
						case 43:
							@blend_type = (int)command.parameters[0];
							break;
						case 44:
							if (Game.GameData is IGameAudioPlay gap)
								gap.SEPlay((string)command.parameters[0]);
							break;
						case 45:
							//result = eval(command.parameters[0]);
							break;
					}
					@move_route_index += 1;
				}
			}
		}

		public void increase_steps()
		{
			@stop_count = 0;
			triggerLeaveTile();
		}

		// Movement stuff ###############################################################
		public virtual void move_down(bool turn_enabled = true)
		{
			if (turn_enabled)
			{
				turn_down();
			}
			if (passable(@x, @y, 2))
			{
				turn_down();
				@y += 1;
				increase_steps();
			}
			else
			{
				if (this is IGameEvent ge) ge.check_event_trigger_touch(@x, @y + 1);
			}
		}

		public virtual void move_left(bool turn_enabled = true)
		{
			if (turn_enabled)
			{
				turn_left();
			}
			if (passable(@x, @y, 4))
			{
				turn_left();
				@x -= 1;
				increase_steps();
			}
			else
			{
				if (this is IGameEvent ge) ge.check_event_trigger_touch(@x - 1, @y);
			}
		}

		public virtual void move_right(bool turn_enabled = true)
		{
			if (turn_enabled)
			{
				turn_right();
			}
			if (passable(@x, @y, 6))
			{
				turn_right();
				@x += 1;
				increase_steps();
			}
			else
			{
				if (this is IGameEvent ge) ge.check_event_trigger_touch(@x + 1, @y);
			}
		}

		public virtual void move_up(bool turn_enabled = true)
		{
			if (turn_enabled)
			{
				turn_up();
			}
			if (passable(@x, @y, 8))
			{
				turn_up();
				@y -= 1;
				increase_steps();
			}
			else
			{
				if (this is IGameEvent ge) ge.check_event_trigger_touch(@x, @y - 1);
			}
		}

		public virtual void move_lower_left()
		{
			if (!@direction_fix)
			{
				@direction = (@direction == 6 ? 4 : @direction == 8 ? 2 : @direction);
			}
			if ((passable(@x, @y, 2) && passable(@x, @y + 1, 4)) ||
			   (passable(@x, @y, 4) && passable(@x - 1, @y, 2)))
			{
				@x -= 1;
				@y += 1;
				increase_steps();
			}
		}

		public virtual void move_lower_right()
		{
			if (!@direction_fix)
			{
				@direction = (@direction == 4 ? 6 : @direction == 8 ? 2 : @direction);
			}
			if ((passable(@x, @y, 2) && passable(@x, @y + 1, 6)) ||
			   (passable(@x, @y, 6) && passable(@x + 1, @y, 2)))
			{
				@x += 1;
				@y += 1;
				increase_steps();
			}
		}

		public virtual void move_upper_left()
		{
			if (!@direction_fix)
			{
				@direction = (@direction == 6 ? 4 : @direction == 2 ? 8 : @direction);
			}
			if ((passable(@x, @y, 8) && passable(@x, @y - 1, 4)) ||
			   (passable(@x, @y, 4) && passable(@x - 1, @y, 8)))
			{
				@x -= 1;
				@y -= 1;
				increase_steps();
			}
		}

		public virtual void move_upper_right()
		{
			if (!@direction_fix)
			{
				@direction = (@direction == 4 ? 6 : @direction == 2 ? 8 : @direction);
			}
			if ((passable(@x, @y, 8) && passable(@x, @y - 1, 6)) ||
			   (passable(@x, @y, 6) && passable(@x + 1, @y, 8)))
			{
				@x += 1;
				@y -= 1;
				increase_steps();
			}
		}

		public virtual void moveLeft90()
		{
			switch (this.direction)
			{
				case 2: // down
					move_right();
					break;
				case 4: // left
					move_down();
					break;
				case 6: // right
					move_up();
					break;
				case 8: // up
					move_left();
					break;
			}
		}

		public virtual void moveRight90()
		{
			switch (this.direction)
			{
				case 2: // down
					move_left();
					break;
				case 4: // left
					move_up();
					break;
				case 6: // right
					move_down();
					break;
				case 8: // up
					move_right();
					break;
			}
		}

		public virtual void move_random()
		{
			switch (Core.Rand.Next(4))
			{
				case 0:
					move_down(false);
					break;
				case 1:
					move_left(false);
					break;
				case 2:
					move_right(false);
					break;
				case 3:
					move_up(false);
					break;
			}
		}

		public virtual void move_toward_player()
		{
			float sx = @x - Game.GameData.GamePlayer.x;
			float sy = @y - Game.GameData.GamePlayer.y;
			if (sx == 0 && sy == 0)
			{
				return;
			}
			float abs_sx = Math.Abs(sx);
			float abs_sy = Math.Abs(sy);
			if (abs_sx == abs_sy)
			{
				if (Core.Rand.Next(2) == 0) abs_sx += 1; else abs_sy += 1;
			}
			if (abs_sx > abs_sy)
			{
				if (sx > 0) move_left(); else move_right();
				if (!moving && sy != 0)
				{
					if (sy > 0) move_up(); else move_down();
				}
			}
			else
			{
				if (sy > 0) move_up(); else move_down();
				if (!moving && sx != 0)
				{
					if (sx > 0) move_left(); else move_right();
				}
			}
		}

		public virtual void move_away_from_player()
		{
			float sx = @x - Game.GameData.GamePlayer.x;
			float sy = @y - Game.GameData.GamePlayer.y;
			if (sx == 0 && sy == 0)
			{
				return;
			}
			float abs_sx = Math.Abs(sx);
			float abs_sy = Math.Abs(sy);
			if (abs_sx == abs_sy)
			{
				if (Core.Rand.Next(2) == 0) abs_sx += 1; else abs_sy += 1;
			}
			if (abs_sx > abs_sy)
			{
				if (sx > 0) move_right(); else move_left();
				if (!moving && sy != 0)
				{
					if (sy > 0) move_down(); else move_up();
				}
			}
			else
			{
				if (sy > 0) move_down(); else move_up();
				if (!moving && sx != 0)
				{
					if (sx > 0) move_right(); else move_left();
				}
			}
		}

		public virtual void move_forward()
		{
			switch (@direction)
			{
				case 2:
					move_down(false);
					break;
				case 4:
					move_left(false);
					break;
				case 6:
					move_right(false);
					break;
				case 8:
					move_up(false);
					break;
			}
		}

		public virtual void move_backward()
		{
			bool last_direction_fix = @direction_fix;
			@direction_fix = true;
			switch (@direction)
			{
				case 2:
					move_up(false);
					break;
				case 4:
					move_right(false);
					break;
				case 6:
					move_left(false);
					break;
				case 8:
					move_down(false);
					break;
			}
			@direction_fix = last_direction_fix;
		}

		public void jump(int x_plus, int y_plus)
		{
			if (x_plus != 0 || y_plus != 0)
			{
				if (Math.Abs(x_plus) > Math.Abs(y_plus))
				{
					if (x_plus < 0) turn_left(); else turn_right();
				}
				else
				{
					if (y_plus < 0) turn_up(); else turn_down();
				}
			}
			int new_x = (int)(@x + x_plus);
			int new_y = (int)(@y + y_plus);
			float oldX = @x;
			float oldY = @y;
			if ((x_plus == 0 && y_plus == 0) || passable(new_x, new_y, 0))
			{
				straighten();
				@x = new_x;
				@y = new_y;
				int distance = (int)Math.Max(4, x_plus * x_plus + y_plus * y_plus);
				@jump_peak = 6 + distance - @move_speed;
				@jump_peak = (int)Math.Floor(@jump_peak);
				@jump_count = (int)(@jump_peak * 2);
				@stop_count = 0;
				if (this is IGamePlayer && Game.GameData.PokemonTemp is ITempMetadataDependantEvents tmd)
				{
					tmd.dependentEvents.MoveDependentEvents();
				}
				triggerLeaveTile();
			}
		}

		public virtual void jumpForward()
		{
			switch (this.direction)
			{
				case 2: // down
					jump(0, 1);
					break;
				case 4: // left
					jump(-1, 0);
					break;
				case 6: // right
					jump(1, 0);
					break;
				case 8: // up
					jump(0, -1);
					break;
			}
		}

		public virtual void jumpBackward()
		{
			switch (this.direction)
			{
				case 2: // down
					jump(0, -1);
					break;
				case 4: // left
					jump(1, 0);
					break;
				case 6: // right
					jump(-1, 0);
					break;
				case 8: // up
					jump(0, 1);
					break;
			}
		}

		public virtual void turnGeneric(int dir)
		{
			if (!@direction_fix)
			{
				int oldDirection = @direction;
				@direction = dir;
				@stop_count = 0;
				if (dir != oldDirection)
				{
					if (this is IGameEvent ge) ge.CheckEventTriggerAfterTurning();
				}
			}
		}

		public virtual void turn_down() { turnGeneric(2); }

		public virtual void turn_left() { turnGeneric(4); }

		public virtual void turn_right() { turnGeneric(6); }

		public virtual void turn_up() { turnGeneric(8); }

		public virtual void turn_right_90()
		{
			switch (@direction)
			{
				case 2:
					turn_left();
					break;
				case 4:
					turn_up();
					break;
				case 6:
					turn_down();
					break;
				case 8:
					turn_right();
					break;
			}
		}

		public virtual void turn_left_90()
		{
			switch (@direction)
			{
				case 2:
					turn_right();
					break;
				case 4:
					turn_down();
					break;
				case 6:
					turn_up();
					break;
				case 8:
					turn_left();
					break;
			}
		}

		public virtual void turn_180()
		{
			switch (@direction)
			{
				case 2:
					turn_up();
					break;
				case 4:
					turn_right();
					break;
				case 6:
					turn_left();
					break;
				case 8:
					turn_down();
					break;
			}
		}

		public virtual void turn_right_or_left_90()
		{
			if (Core.Rand.Next(2) == 0)
			{
				turn_right_90();
			}
			else
			{
				turn_left_90();
			}
		}

		public virtual void turn_random()
		{
			switch (Core.Rand.Next(4))
			{
				case 0:
					turn_up();
					break;
				case 1:
					turn_right();
					break;
				case 2:
					turn_left();
					break;
				case 3:
					turn_down();
					break;
			}
		}

		public virtual void turn_toward_player()
		{
			float sx = @x - Game.GameData.GamePlayer.x;
			float sy = @y - Game.GameData.GamePlayer.y;
			if (sx == 0 && sy == 0)
			{
				return;
			}
			if (Math.Abs(sx) > Math.Abs(sy))
			{
				if (sx > 0) turn_left(); else turn_right();
			}
			else
			{
				if (sy > 0) turn_up(); else turn_down();
			}
		}

		public virtual void turn_away_from_player()
		{
			float sx = @x - Game.GameData.GamePlayer.x;
			float sy = @y - Game.GameData.GamePlayer.y;
			if (sx == 0 && sy == 0)
			{
				return;
			}
			if (Math.Abs(sx) > Math.Abs(sy))
			{
				if (sx > 0) turn_right(); else turn_left();
			}
			else
			{
				if (sy > 0) turn_down(); else turn_up();
			}
		}

		public virtual void minilock()
		{
			@prelock_direction = @direction;
			@locked = true;
		}
	}
}