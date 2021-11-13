using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Overworld;

namespace PokemonUnity
{
    public partial class Game { 
/// <summary>
/// This class handles the map. It includes scrolling and passable determining
/// functions. Refer to <see cref="GameData.GameMap"/> for the instance of this class.
/// </summary>
public partial class Game_Map {
        #region Variables
  public MapData map				            { get; set; }
  public int map_id				                { get; set; }
  public string tileset_name			        { get; set; }     // tileset file name
  public string autotile_names			        { get; set; }     // autotile file name
  public string panorama_name			        { get; set; }     // panorama file name
  public int panorama_hue				        { get; set; }     // panorama hue
  public string fog_name				        { get; set; }     // fog file name
  public int fog_hue				            { get; set; }     // fog hue
  public float fog_opacity				        { get; set; }     // fog opacity level
  public int fog_blend_type				        { get; set; }     // fog blending method
  public float fog_zoom				            { get; set; }     // fog zoom rate
  public float fog_sx				            { get; set; }     // fog sx
  public float fog_sy				            { get; set; }     // fog sy
  public string battleback_name			        { get; set; }     // battleback file name
  public float display_x				        { get; set; }     // display x-coordinate * 128
  public float display_y				        { get; set; }     // display y-coordinate * 128
  public bool need_refresh				        { get; set; }     // refresh request flag
  public int[] passages				            { get; set; }     // passage table
  public int[] priorities				        { get; set; }     // prioroty table
  public Terrains[] terrain_tags				{ get; set; }     // terrain tag table
  //public Dictionary<int,Avatar.GameEvent> events{ get; set; }     // events
  public float fog_ox				            { get; set; }     // fog x-coordinate starting point
  public float fog_oy				            { get; set; }     // fog y-coordinate starting point
  public int fog_tone				            { get; set; }     // fog color tone
  public int mapsInRange				        { get; set; }
        private int fog_tone_duration;
        private int fog_opacity_duration;
        private float fog_opacity_target;
        private int scroll_direction;
        private int scroll_rest;
        private float scroll_speed;
        private Dictionary<int, object> common_events;
        #endregion

        #region Constructor
        public Game_Map() {
    @map_id = 0;
    @display_x = 0;
    @display_y = 0;
  }
        #endregion
        
        #region Methods
  public void setup(int map_id) {
    this.map_id = map_id;
    @map=new MapData();//load_data(string.Format("Data/Map%03d.%s", map_id,Core.RPGVX != null ? "rvdata" : "rxdata"));
    //tileset = GameData.DataTilesets[@map.tileset_id];
    //@tileset_name = tileset.tileset_name;
    //@autotile_names = tileset.autotile_names;
    //@panorama_name = tileset.panorama_name;
    //@panorama_hue = tileset.panorama_hue;
    //@fog_name = tileset.fog_name;
    //@fog_hue = tileset.fog_hue;
    //@fog_opacity = tileset.fog_opacity;
    //@fog_blend_type = tileset.fog_blend_type;
    //@fog_zoom = tileset.fog_zoom;
    //@fog_sx = tileset.fog_sx;
    //@fog_sy = tileset.fog_sy;
    //@battleback_name = tileset.battleback_name;
    //@passages = tileset.passages;
    //@priorities = tileset.priorities;
    //@terrain_tags = tileset.terrain_tags;
    this.display_x = 0;
    this.display_y = 0;
    @need_refresh = false;
    //Events.onMapCreate.trigger(this,map_id, @map, tileset);
    //Events.OnMapCreate.Invoke(this,map_id, @map, tileset);
    //@events = new Dictionary<int, Avatar.GameEvent>();
    //foreach (int i in @map.events.Keys) {
    //  @events[i] = new Game_Event(@map_id, @map.events[i],this);
    //}
    //@common_events = new Dictionary<int,object>();
    //for (int i = 1; i < DataCommonEvents.size; i++) {
    //  @common_events[i] = new Game_CommonEvent(i);
    //}
    //@fog_tone = new Tone(0, 0, 0, 0);
    //@fog_tone_target = new Tone(0, 0, 0, 0);
    @fog_ox = 0;
    @fog_oy = 0;
    @fog_tone_duration = 0;
    @fog_opacity_duration = 0;
    @fog_opacity_target = 0;
    @scroll_direction = 2;
    @scroll_rest = 0;
    @scroll_speed = 4;
  }

  //public int map_id { get {
  //  return @map_id;
  //} }

  public int width { get {
    return @map.width;
  } }

  public int height { get {
    return @map.height;
  } }

  public List<Pokemons> encounter_list { get {
    return @map.encounter_list;
  } }

  public int encounter_step { get {
    return @map.encounter_step;
  } }

  public int?[,,] data { get { //Vector that returns tile terrain?
    return @map.data;
  } }
  /// <summary>
  /// Autoplays background music
  /// Plays music called "[normal BGM]n" if it's night time and it exists
  /// </summary>
  public void autoplayAsCue() {
    if (@map.autoplay_bgm) {
      if (IsNight) { //&& FileTest.audio_exist("Audio/BGM/"+ @map.bgm.name+ "n")
        UI.pbCueBGM(@map.bgm.name+"n",1.0f,@map.bgm.volume,@map.bgm.pitch);
      } else {
        UI.pbCueBGM(@map.bgm,1.0f);
      }
    }
    if (@map.autoplay_bgs) {
      UI.pbBGSPlay(@map.bgs);
    }
  }
  /// <summary>
  /// Plays background music
  /// Plays music called "[normal BGM]n" if it's night time and it exists
  /// </summary>
  public void autoplay() {
    if (@map.autoplay_bgm) {
      if (IsNight) { //&& FileTest.audio_exist("Audio/BGM/"+ @map.bgm.name+ "n")
        UI.pbBGMPlay(@map.bgm.name+"n",@map.bgm.volume,@map.bgm.pitch);
      } else {
        UI.pbBGMPlay(@map.bgm);
      }
    }
    if (@map.autoplay_bgs) {
      UI.pbBGSPlay(@map.bgs);
    }
  }

  public void refresh() {
    //if (@map_id > 0) {
    //  foreach (var @event in this.events.Values) {
    //    @event.refresh();
    //  }
    //  foreach (var common_event in @common_events.Values) {
    //    common_event.refresh();
    //  }
    //}
    @need_refresh = false;
  }

  public void scroll_down(float distance) {
    @display_y = Math.Min(@display_y + distance, (this.height - 15) * 128);
  }

  public void scroll_left(float distance) {
    @display_x = Math.Max(@display_x - distance, 0);
  }

  public void scroll_right(float distance) {
    @display_x = Math.Min(@display_x + distance, (this.width - 20) * 128);
  }

  public void scroll_up(float distance) {
    @display_y = Math.Max(@display_y - distance, 0);
  }

  public bool valid (float x,float y) {
     return (x >= 0 && x < width && y >= 0 && y < height);
  }

  public bool validLax (float x,float y) {
    return (x >=-10 && x <= width+10 && y >=-10 && y <= height+10);
  }

  /*public bool passable (float x,float y,int d,Avatar.Character self_event = null) {
    if (!valid(x, y)) return false;
    int bit = (1 << (d / 2 - 1)) & 0x0f;
    foreach (var @event in events.Values) {
      if (@event.tile_id >= 0 && @event != self_event &&
         @event.x == x && @event.y == y && !@event.through) {
//        if @terrain_tags[@event.tile_id]!=Terrains.Neutral
          if (@passages[@event.tile_id] & bit != 0) return false;
          if (@passages[@event.tile_id] & 0x0f == 0x0f) return false;
          if (@priorities[@event.tile_id] == 0) return true;
//        }
      }
    }
    if (self_event==GameData.GamePlayer) {
      return playerPassable(x, y, d, self_event);
    } else {
      //  All other events
      float newx=x;float newy=y;
      switch (d) {
      case 1:
        newx-=1; newy+=1;
        break;
      case 2:
        newy+=1;
        break;
      case 3:
        newx+=1; newy+=1;
        break;
      case 4:
        newx-=1;
        break;
      case 6:
        newx+=1;
        break;
      case 7:
        newx-=1; newy-=1;
        break;
      case 8:
        newy-=1;
        break;
      case 9:
        newx+=1; newy-=1;
        break;
      }
      if (!valid(newx, newy)) return false;
      foreach (int i in new int[] { 2, 1, 0 }) { //foreach z axis in map
        int? tile_id = data[(int)System.Math.Floor(x), (int)System.Math.Floor(y), i];
        if (tile_id == null) {
          return false;
        //  If already on water, only allow movement to another water tile
        } else if (self_event!=null &&
           Terrain.isJustWater(@terrain_tags[tile_id.Value])) {
          foreach (var j in new int[] { 2, 1, 0 }) { //foreach z axis in map
            int? facing_tile_id=data[(int)System.Math.Floor(newx), (int)System.Math.Floor(newy), j];
            if (facing_tile_id==null) return false;
            if (@terrain_tags[facing_tile_id.Value]!=0 &&
               @terrain_tags[facing_tile_id.Value]!=Terrains.Neutral) {
              return Terrain.isJustWater(@terrain_tags[facing_tile_id.Value]);
            }
          }
          return false;
        //  Can't walk onto ice
        } else if (Terrain.isIce(@terrain_tags[tile_id.Value])) {
          return false;
        } else if (self_event!=null && self_event.x==x && self_event.y==y) {
          //  Can't walk onto ledges
          foreach (int j in new int[] { 2, 1, 0 }) { //foreach z axis in map
            int? facing_tile_id=data[(int)System.Math.Floor(newx), (int)System.Math.Floor(newy), j];
            if (facing_tile_id==null) return false;
            if (@terrain_tags[facing_tile_id.Value]!=0 &&
               @terrain_tags[facing_tile_id.Value]!=Terrains.Neutral) {
              if (Terrain.isLedge(@terrain_tags[facing_tile_id.Value])) return false;
              break;
            }
          }
//        Regular passability checks
//        if (@terrain_tags[tile_id]!=Terrains.Neutral)
            if ((@passages[tile_id.Value] & bit) != 0 ||
               (@passages[tile_id.Value] & 0x0f) == 0x0f) {
              return false;
            } else if (@priorities[tile_id.Value] == 0) {
              return true;
            }
//        }
//      Regular passability checks
        } else { //if (@terrain_tags[tile_id]!=Terrains.Neutral)
          if ((@passages[tile_id.Value] & bit) != 0 ||
             (@passages[tile_id.Value] & 0x0f) == 0x0f) {
            return false;
          } else if (@priorities[tile_id.Value] == 0) {
            return true;
          }
        }
      }
      return true;
    }
  }

  public bool playerPassable (float x,float y,int d,Avatar.Character self_event = null) {
    int bit = (1 << (d / 2 - 1)) & 0x0f;
    foreach (var i in new int[] { 2, 1, 0 }) { //foreach z axis in map
      int? tile_id = data[(int)System.Math.Floor(x), (int)System.Math.Floor(y), i];
      //  Ignore bridge tiles if not on a bridge
      if (GameData.Global != null && GameData.Global.bridge==0 &&
         tile_id != null && Terrain.isBridge(@terrain_tags[tile_id.Value])) continue;
      if (tile_id == null) {
        return false;
        //  Make water tiles passable if player is surfing
      } else if (GameData.Global.surfing &&
         Terrain.isPassableWater(@terrain_tags[tile_id.Value])) {
        return true;
        //  Prevent cycling in really tall grass/on ice
      } else if (GameData.Global.bicycle &&
         Terrain.onlyWalk(@terrain_tags[tile_id.Value])) {
        return false;
        //  Depend on passability of bridge tile if on bridge
      } else if (GameData.Global != null && GameData.Global.bridge>0 &&
         Terrain.isBridge(@terrain_tags[tile_id.Value])) {
        if ((@passages[tile_id.Value] & bit) != 0 ||
           (@passages[tile_id.Value] & 0x0f) == 0x0f) {
          return false;
        } else {
          return true;
        }
        //  Regular passability checks
      } else { //if (@terrain_tags[tile_id]!=Terrains.Neutral)
        if ((@passages[tile_id.Value] & bit) != 0 ||
           (@passages[tile_id.Value] & 0x0f) == 0x0f) {
          return false;
        } else if (@priorities[tile_id.Value] == 0) {
          return true;
        }
      }
    }
    return true;
  }

  public bool passableStrict (float x,float y,int d,Avatar.Character self_event = null) {
    if (!valid(x, y)) return false;
    foreach (Avatar.GameEvent @event in events.Values) {
      if (@event.tile_id >= 0 && @event != self_event &&
         @event.x == x && @event.y == y && !@event.through) {
//        if @terrain_tags[@event.tile_id]!=Terrains.Neutral
          if ((@passages[@event.tile_id] & 0x0f) != 0) return false;
          if (@priorities[@event.tile_id] == 0) return true;
//        }
      }
    }
    foreach (int i in new int[] { 2, 1, 0 }) { //foreach z axis in map
      int? tile_id = data[(int)System.Math.Floor(x), (int)System.Math.Floor(y), i];
      if (tile_id == null) return false;
//      if (@terrain_tags[tile_id]!=Terrains.Neutral)
        if ((@passages[tile_id.Value] & 0x0f) != 0) return false;
        if (@priorities[tile_id.Value] == 0) return true;
//      }
    }
    return true;
  }*/

  public bool deepBush (float x,float y) {
    if (@map_id != 0) {
      foreach (var i in new int[] { 2, 1, 0 }) { //foreach z axis in map
        int? tile_id = data[(int)System.Math.Floor(x), (int)System.Math.Floor(y), i];
        if (tile_id == null) {
          return false;
        } else if (Terrain.isBridge(@terrain_tags[tile_id.Value]) && GameData.Global != null &&
              GameData.Global.bridge>0) {
          return false;
        } else if ((@passages[tile_id.Value] & 0x40) == 0x40 &&
           @terrain_tags[tile_id.Value]==Terrains.TallGrass) {
          return true;
        }
      }
    }
    return false;
  }

  public bool bush (float x,float y) {
    if (@map_id != 0) {
      foreach (var i in new int[] { 2, 1, 0 }) { //foreach z axis in map
        int? tile_id = data[(int)System.Math.Floor(x), (int)System.Math.Floor(y), i];
        if (tile_id == null) {
          return false;
        } else if (Terrain.isBridge(@terrain_tags[tile_id.Value]) && GameData.Global != null &&
              GameData.Global.bridge>0) {
          return false;
        } else if ((@passages[tile_id.Value] & 0x40) == 0x40) {
          return true;
        }
      }
    }
    return false;
  }

  public bool counter (float x,float y) {
    if (@map_id != 0) {
      foreach (int i in new int[] { 2, 1, 0 }) { //foreach z axis in map
        int? tile_id = data[(int)System.Math.Floor(x), (int)System.Math.Floor(y), i];
        if (tile_id == null) {
          return false;
        } else if (//@passages[tile_id.Value] != null && 
            (@passages[tile_id.Value] & 0x80) == 0x80) {
          return true;
        }
      }
    }
    return false;
  }

  public Terrains terrain_tag(float x,float y,bool countBridge=false) {
    if (@map_id != 0) {
      foreach (int i in new int[] { 2, 1, 0 }) { //foreach z axis in map
        int? tile_id = data[(int)System.Math.Floor(x), (int)System.Math.Floor(y), i];
        if (tile_id != null && Terrain.isBridge(@terrain_tags[tile_id.Value]) &&
                GameData.Global != null && GameData.Global.bridge==0 && !countBridge) continue;
        if (tile_id == null) {
          return 0;
        } else if (//@terrain_tags[tile_id.Value] != null && 
            @terrain_tags[tile_id.Value] > 0 &&
          @terrain_tags[tile_id.Value]!=Terrains.Neutral) {
          return @terrain_tags[tile_id.Value];
        }
      }
    }
    return 0;
  }

  public int? check_event(float x,float y) {
    //foreach (var @event in this.events.Values) {
    //  if (@event.x == x && @event.y == y) return @event.id;
    //}
    return null;
  }

  public void start_scroll(int direction,int distance,float speed) {
    @scroll_direction = direction;
    @scroll_rest = distance * 128;
    @scroll_speed = speed;
  }

  public bool scrolling { get {
    return @scroll_rest > 0;
  } }

  //public void start_fog_tone_change(Tone tone,int duration) {
  //  @fog_tone_target = tone.clone();
  //  @fog_tone_duration = duration;
  //  if (@fog_tone_duration == 0) {
  //    @fog_tone = @fog_tone_target.clone();
  //  }
  //}

  public void start_fog_opacity_change(float opacity,int duration) {
    @fog_opacity_target = opacity * 1f;
    @fog_opacity_duration = duration;
    if (@fog_opacity_duration == 0) {
      @fog_opacity = @fog_opacity_target;
    }
  }

  public bool in_range (object @object) {
    //if (GameData.PokemonSystem.tilemap==2) return true;
    //float screne_x = display_x - 4*32*4;
    //float screne_y = display_y - 4*32*4;
    //int screne_width = display_x + Graphics.width*4 + 4*32*4;
    //int screne_height = display_y + Graphics.height*4 + 4*32*4;
    //if (@object.real_x <= screne_x) return false;
    //if (@object.real_x >= screne_width) return false;
    //if (@object.real_y <= screne_y) return false;
    //if (@object.real_y >= screne_height) return false;
    return true;
  }

  public void update() {
    //if (GameData.MapFactory != null) {
    //  foreach (var i in GameData.MapFactory.maps) {
    //    if (i.need_refresh) i.refresh();
    //  }
    //  GameData.MapFactory.setCurrentMap();
    //}
    //if (@scroll_rest > 0) {
    //  float distance = (float)Math.Pow(2, (double)@scroll_speed);
    //  switch (@scroll_direction) {
    //  case 2:
    //    scroll_down(distance);
    //    break;
    //  case 4: 
    //    scroll_left(distance);
    //    break;
    //  case 6: 
    //    scroll_right(distance);
    //    break;
    //  case 8:
    //    scroll_up(distance);
    //    break;
    //  }
    //  @scroll_rest -= distance;
    //}
    //foreach (var @event in @events.Values) {
    //  if (in_range(@event) || @event.trigger == 3 || @event.trigger == 4) {
    //    @event.update();
    //  }
    //}
    //foreach (var common_event in @common_events.Values) {
    //  common_event.update();
    //}
    //@fog_ox -= @fog_sx / 8.0f;
    //@fog_oy -= @fog_sy / 8.0f;
    //if (@fog_tone_duration >= 1) {
    //  int d = @fog_tone_duration;
    //  target = @fog_tone_target;
    //  @fog_tone.red = (@fog_tone.red * (d - 1) + target.red) / d;
    //  @fog_tone.green = (@fog_tone.green * (d - 1) + target.green) / d;
    //  @fog_tone.blue = (@fog_tone.blue * (d - 1) + target.blue) / d;
    //  @fog_tone.gray = (@fog_tone.gray * (d - 1) + target.gray) / d;
    //  @fog_tone_duration -= 1;
    //}
    //if (@fog_opacity_duration >= 1) {
    //  int d = @fog_opacity_duration;
    //  @fog_opacity = (@fog_opacity * (d - 1) + @fog_opacity_target) / d;
    //  @fog_opacity_duration -= 1;
    //}
  }
        #endregion
}

public partial class Game_Map {
  public string name { get {
    string ret=""; //Game.pbGetMessage(MessageTypes.MapNames,this.map_id); //Dictionary of Static Strings
    if (GameData.Trainer != null) {
      // Replace "\PN" with the Trainer.Name
      //ret.gsub!(/\\PN/,GameData.Trainer.name);
    }
    return ret;
  } }
}

public partial class Game_Map {
  public const int TILEWIDTH = 32;
  public const int TILEHEIGHT = 32;
  public static int XSUBPIXEL = 4; //Core.RPGVX != null ? 8 :
  public static int YSUBPIXEL = 4; //Core.RPGVX != null ? 8 :

  public static int realResX { get {
    return XSUBPIXEL * TILEWIDTH;
  } }

  public static int realResY { get {
    return YSUBPIXEL * TILEHEIGHT;
  } }

  //public int display_x { set {
  //  @display_x=value;
  //  if (pbGetMetadata(this.map_id,MetadataSnapEdges)) {
  //    max_x = (this.width - Graphics.width*1.0f/Game_Map.TILEWIDTH) * Game_Map.realResX;
  //    @display_x = Math.Max(0, Math.Min(@display_x, max_x));
  //  }
  //  if (GameData.MapFactory != null) GameData.MapFactory.setMapsInRange;
  //} }
  //
  //public int display_y { set {
  //  @display_y=value;
  //  if (pbGetMetadata(this.map_id,MetadataSnapEdges)) {
  //    int max_y = (this.height - Graphics.height*1.0f/Game_Map.TILEHEIGHT) * Game_Map.realResY;
  //    @display_y = Math.Max(0, Math.Min(@display_y, max_y));
  //  }
  //  if (GameData.MapFactory != null) GameData.MapFactory.setMapsInRange;
  //} }
  //
  //public void start_scroll(int direction,int distance,int speed) {
  //  @scroll_direction = direction;
  //  if (direction==2 || direction==8) {
  //     @scroll_rest = distance * Game_Map.realResY;
  //  } else {
  //     @scroll_rest = distance * Game_Map.realResX;
  //  }
  //  @scroll_speed = speed;
  //}
  //
  //public void scroll_down(int distance) {
  //  this.display_y+=distance;
  //}
  //
  //public void scroll_left(int distance) {
  // this.display_x-=distance;
  //}
  //
  //public void scroll_right(int distance) {
  //  this.display_x+=distance;
  //}
  //
  //public void scroll_up(int distance) {
  // this.display_y-=distance;
  //}
}
}

	public struct MapData
    {
        public int tileset_id                   { get; private set; }
        public int width                        { get; private set; }
        public int height                       { get; private set; }
        public bool autoplay_bgm                { get; private set; }
        public IAudioObject bgm                 { get; private set; }
        public bool autoplay_bgs                { get; private set; }
        public IAudioObject bgs                 { get; private set; }
        public List<Pokemons> encounter_list         { get; private set; }
        public int encounter_step               { get; private set; }
        public int?[,,] data                    { get; private set; }
        public Dictionary<int,int> events       { get; private set; }

        public MapData(int width, int height)
        {
            @tileset_id = 1;
            this.width = width;
            this.height = height;
            @autoplay_bgm = false;
            @bgm = null; //new RPG.AudioFile();
            @autoplay_bgs = false;
            @bgs = null; //new RPG.AudioFile("", 80);
            @encounter_list = new List<Pokemons>();
            @encounter_step = 30;
            @data = new int?[width, height, 3];
            @events = new Dictionary<int, int>();
        }
    }
}