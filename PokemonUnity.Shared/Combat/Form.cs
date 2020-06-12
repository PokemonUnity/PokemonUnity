using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Attack;
using PokemonUnity.Inventory;
using PokemonUnity.Monster;
using PokemonUnity.Character;
using PokemonUnity.Monster.Data;

namespace PokemonUnity.Combat
{
public partial class Pokemon{ //PokeBattle_Pokemon
    // Time when Furfrou's/Hoopa's form was set
  public DateTime? formTime { get; set; }
  public int? @forcedform { get; set; }
  private int formId { get; set; }
  public int form { get {
    if (@forcedform!=null) return @forcedform.Value;
    //int v=MultipleForms.call("getForm",self);
    int v=MultipleForms.getForm(this);
    //if (v!=null) {
      //if form is not null, then value has been set already?...
      if (v!=@formId) this.formId=v; //|| !@form
      return v;
    //}
    return @formId; //|| 0
  }

  set {
    @formId=value;
    //MultipleForms.call("onSetForm",this,value);
    MultipleForms.onSetForm(this,value);
    //this.calcStats;
    //pbSeenForm(this);
    if(Game.GameData.Player.Pokedex[(int)pokemon.Species, 2] < 0)
      Game.GameData.Player.Pokedex[(int)pokemon.Species,2] = (byte)value; //(byte)pokemon.form;
  } }

  public int formNoCall { set {
    @form=value;
    //this.calcStats;
  } }

    // Used by the Pokédex only
  public void forceForm(int value) {
    @forcedform=value;
  }

  /*alias __mf_baseStats baseStats;
  alias __mf_ability ability;
  alias __mf_getAbilityList getAbilityList;
  alias __mf_type1 type1;
  alias __mf_type2 type2;
  alias __mf_height height;
  alias __mf_weight weight;
  alias __mf_getMoveList getMoveList;
  alias __mf_isCompatibleWithMove? isCompatibleWithMove?;
  alias __mf_wildHoldItems wildHoldItems;
  alias __mf_baseExp baseExp;
  alias __mf_evYield evYield;
  alias __mf_kind kind;
  alias __mf_dexEntry dexEntry;
  alias __mf_initialize initialize;
  public void baseStats() {
    v=MultipleForms.call("getBaseStats",self);
    if (v!=null) return v;
    return this.__mf_baseStats;
  }

    // DEPRECATED - do not use
  public void ability() {   
    v=MultipleForms.call("ability",self);
    if (v!=null) return v;
    return this.__mf_ability;
  }

  public void getAbilityList() {
    v=MultipleForms.call("getAbilityList",self);
    if (v!=null && v.Length>0) return v;
    return this.__mf_getAbilityList;
  }

  public void type1() {
    v=MultipleForms.call("type1",self);
    if (v!=null) return v;
    return this.__mf_type1;
  }

  public void type2() {
    v=MultipleForms.call("type2",self);
    if (v!=null) return v;
    return this.__mf_type2;
  }

  public void height() {
    v=MultipleForms.call("height",self);
    if (v!=null) return v;
    return this.__mf_height;
  }

  public void weight() {
    v=MultipleForms.call("weight",self);
    if (v!=null) return v;
    return this.__mf_weight;
  }

  public void getMoveList() {
    v=MultipleForms.call("getMoveList",self);
    if (v!=null) return v;
    return this.__mf_getMoveList;
  }

  public bool isCompatibleWithMove (move) {
    v=MultipleForms.call("getMoveCompatibility",self);
    if (v!=null) {
      return v.any? {|j| j==move };
    }
    return this.__mf_isCompatibleWithMove(move);
  }

  public void wildHoldItems() {
    v=MultipleForms.call("wildHoldItems",self);
    if (v!=null) return v;
    return this.__mf_wildHoldItems;
  }

  public void baseExp() {
    v=MultipleForms.call("baseExp",self);
    if (v!=null) return v;
    return this.__mf_baseExp;
  }

  public void evYield() {
    v=MultipleForms.call("evYield",self);
    if (v!=null) return v;
    return this.__mf_evYield;
  }

  public void kind() {
    v=MultipleForms.call("kind",self);
    if (v!=null) return v;
    return this.__mf_kind;
  }

  public void dexEntry() {
    v=MultipleForms.call("dexEntry",self);
    if (v!=null) return v;
    return this.__mf_dexEntry;
  }

  public void initialize(*args) {
    __mf_initialize(*args);
    f=MultipleForms.call("getFormOnCreation",self);
    if (f) {
      this.form=f;
      this.resetMoves;
    }
  }*/
}

/*public class PokeBattle_RealBattlePeer{
  public void pbOnEnteringBattle(Combat.Battle battle,Pokemon pokemon) {
    //f=MultipleForms.call("getFormOnEnteringBattle",pokemon);
    int f=MultipleForms.getFormOnEnteringBattle(pokemon);
    //if (f) {
    //  pokemon.form=f;
    //}
    pokemon.SetForm(f);
  }
}*/

/*public static class MultipleForms{
  @@formSpecies=new HandlerHash(:PBSpecies);
  public void copy(sym,*syms) {
    @@formSpecies.copy(sym,*syms);
  }

  public void register(sym,hash) {
    @@formSpecies.add(sym,hash);
  }

  public void registerIf(cond,hash) {
    @@formSpecies.addIf(cond,hash);
  }

  public void hasFunction(pokemon,func) {
    spec=(pokemon.is_a(Numeric)) ? pokemon : pokemon.Species;
    sp=@@formSpecies[spec];
    return sp && sp[func];
  }

  public void getFunction(pokemon,func) {
    spec=(pokemon.is_a(Numeric)) ? pokemon : pokemon.Species;
    sp=@@formSpecies[spec];
    return (sp && sp[func]) ? sp[func] : null;
  }

  public void call(func,pokemon,*args) {
    sp=@@formSpecies[pokemon.Species];
    if (!sp || !sp[func]) return null;
    return sp[func].call(pokemon,*args);
  }
}*/

/*public void drawSpot(bitmap,spotpattern,x,y,red,green,blue) {
  height=spotpattern.Length;
  width=spotpattern[0].Length;
  for (int yy = 0; yy < height; yy++) {
    spot=spotpattern[yy];
    for (int xx = 0; xx < width; xx++) {
      if (spot[xx]==1) {
        xOrg=(x+xx)<<1;
        yOrg=(y+yy)<<1;
        color=bitmap.get_pixel(xOrg,yOrg);
        r=color.red+red;
        g=color.green+green;
        b=color.blue+blue;
        color.red=(int)Math.Min((int)Math.Max(r,0),255);
        color.green=(int)Math.Min((int)Math.Max(g,0),255);
        color.blue=(int)Math.Min((int)Math.Max(b,0),255);
        bitmap.set_pixel(xOrg,yOrg,color);
        bitmap.set_pixel(xOrg+1,yOrg,color);
        bitmap.set_pixel(xOrg,yOrg+1,color);
        bitmap.set_pixel(xOrg+1,yOrg+1,color);
      }   
    }
  }
}

public void pbSpindaSpots(pokemon,bitmap) {
  spot1=[
     [0,0,1,1,1,1,0,0],
     [0,1,1,1,1,1,1,0],
     [1,1,1,1,1,1,1,1],
     [1,1,1,1,1,1,1,1],
     [1,1,1,1,1,1,1,1],
     [1,1,1,1,1,1,1,1],
     [1,1,1,1,1,1,1,1],
     [0,1,1,1,1,1,1,0],
     [0,0,1,1,1,1,0,0]
  ];
  spot2=[
     [0,0,1,1,1,0,0],
     [0,1,1,1,1,1,0],
     [1,1,1,1,1,1,1],
     [1,1,1,1,1,1,1],
     [1,1,1,1,1,1,1],
     [1,1,1,1,1,1,1],
     [1,1,1,1,1,1,1],
     [0,1,1,1,1,1,0],
     [0,0,1,1,1,0,0]
  ];
  spot3=[
     [0,0,0,0,0,1,1,1,1,0,0,0,0],
     [0,0,0,1,1,1,1,1,1,1,0,0,0],
     [0,0,1,1,1,1,1,1,1,1,1,0,0],
     [0,1,1,1,1,1,1,1,1,1,1,1,0],
     [0,1,1,1,1,1,1,1,1,1,1,1,0],
     [1,1,1,1,1,1,1,1,1,1,1,1,1],
     [1,1,1,1,1,1,1,1,1,1,1,1,1],
     [1,1,1,1,1,1,1,1,1,1,1,1,1],
     [0,1,1,1,1,1,1,1,1,1,1,1,0],
     [0,1,1,1,1,1,1,1,1,1,1,1,0],
     [0,0,1,1,1,1,1,1,1,1,1,0,0],
     [0,0,0,1,1,1,1,1,1,1,0,0,0],
     [0,0,0,0,0,1,1,1,0,0,0,0,0]
  ];
  spot4=[
     [0,0,0,0,1,1,1,0,0,0,0,0],
     [0,0,1,1,1,1,1,1,1,0,0,0],
     [0,1,1,1,1,1,1,1,1,1,0,0],
     [0,1,1,1,1,1,1,1,1,1,1,0],
     [1,1,1,1,1,1,1,1,1,1,1,0],
     [1,1,1,1,1,1,1,1,1,1,1,1],
     [1,1,1,1,1,1,1,1,1,1,1,1],
     [1,1,1,1,1,1,1,1,1,1,1,1],
     [1,1,1,1,1,1,1,1,1,1,1,0],
     [0,1,1,1,1,1,1,1,1,1,1,0],
     [0,0,1,1,1,1,1,1,1,1,0,0],
     [0,0,0,0,1,1,1,1,1,0,0,0]
  ];
  id=pokemon.personalID;
  h=(id>>28)&15;
  g=(id>>24)&15;
  f=(id>>20)&15;
  e=(id>>16)&15;
  d=(id>>12)&15;
  c=(id>>8)&15;
  b=(id>>4)&15;
  a=(id)&15;
  if (pokemon.isShiny?) {
    drawSpot(bitmap,spot1,b+33,a+25,-75,-10,-150);
    drawSpot(bitmap,spot2,d+21,c+24,-75,-10,-150);
    drawSpot(bitmap,spot3,f+39,e+7,-75,-10,-150);
    drawSpot(bitmap,spot4,h+15,g+6,-75,-10,-150);
  }
  else {
    drawSpot(bitmap,spot1,b+33,a+25,0,-115,-75);
    drawSpot(bitmap,spot2,d+21,c+24,0,-115,-75);
    drawSpot(bitmap,spot3,f+39,e+7,0,-115,-75);
    drawSpot(bitmap,spot4,h+15,g+6,0,-115,-75);
  }
}*/

// ###############################################################################
public static class MultipleForms{
public static int getForm(Pokemon pokemon)
{
    if (pokemon.Species == Pokemons.GIRATINA)
    {
   int[] maps=new int[] { 49, 50, 51, 72, 73 };   // Map IDs for Origin Forme
   if (pokemon.Item == Items.GRISEOUS_ORB ||
      //($game_map && maps.Contains($game_map.map_id))) {
      Game.GameData != null && maps.Contains(Game.GameData.Player.Area)) {
     return 1;
   }
   return 0;
    }
    else if (pokemon.Species == Pokemons.SHAYMIN)
    {
   if (pokemon.HP<=0 || pokemon.Status==Status.FROZEN || Game.GetTime == Overworld.DayTime.Night)
             //PBDayNight.isNight) return 0;
   return 0; //null;
    }
    else if (pokemon.Species == Pokemons.ARCEUS)
    {
   if (pokemon.Item == Items.FIST_PLATE) return 1 ;
   if (pokemon.Item == Items.SKY_PLATE) return 2 ;
   if (pokemon.Item == Items.TOXIC_PLATE) return 3 ;
   if (pokemon.Item == Items.EARTH_PLATE) return 4 ;
   if (pokemon.Item == Items.STONE_PLATE) return 5 ;
   if (pokemon.Item == Items.INSECT_PLATE) return 6 ;
   if (pokemon.Item == Items.SPOOKY_PLATE) return 7 ;
   if (pokemon.Item == Items.IRON_PLATE) return 8 ;
   if (pokemon.Item == Items.FLAME_PLATE) return 10;
   if (pokemon.Item == Items.SPLASH_PLATE) return 11;
   if (pokemon.Item == Items.MEADOW_PLATE) return 12;
   if (pokemon.Item == Items.ZAP_PLATE) return 13;
   if (pokemon.Item == Items.MIND_PLATE) return 14;
   if (pokemon.Item == Items.ICICLE_PLATE) return 15;
   if (pokemon.Item == Items.DRACO_PLATE) return 16;
   if (pokemon.Item == Items.DREAD_PLATE) return 17;
   if (pokemon.Item == Items.PIXIE_PLATE) return 18;
   return 0;
    }
    else if (pokemon.Species == Pokemons.DEERLING || pokemon.Species == Pokemons.SAWSBUCK)
    {
   return (int)Game.Season;//pbGetSeason();
    }
    else if (pokemon.Species == Pokemons.KELDEO)
    {
   if (pokemon.pokemon.hasMove(Moves.SECRET_SWORD)) return 1;	// Resolute Form
   return 0;                                   // Ordinary Form
    }
    else if (pokemon.Species == Pokemons.GENESECT)
    {
   if (pokemon.Item == Items.SHOCK_DRIVE) return 1;
   if (pokemon.Item == Items.BURN_DRIVE) return 2;
   if (pokemon.Item == Items.CHILL_DRIVE) return 3;
   if (pokemon.Item == Items.DOUSE_DRIVE) return 4;
   return 0;
    }
    else if (pokemon.Species == Pokemons.FURFROU)
    {
   if (!pokemon.formTime.HasValue || Game.GetTimeNow>pokemon.formTime.Value.AddDays(5)) {		// 5 days => +60*60*24*5
     return 0;
   }
   //continue;
    }
    else if (pokemon.Species == Pokemons.HOOPA)
    {
   if (!pokemon.formTime.HasValue || Game.GetTimeNow>pokemon.formTime.Value.AddDays(3)) {		// 3 days => +60*60*24*3
     return 0;
   }
   //continue;
    }
    //else 
      return pokemon.pokemon.FormId; //0;
}
public static int getFormOnEnteringBattle(Pokemon pokemon)
{
    if (pokemon.Species == Pokemons.BURMY)
    {
   Environment env=Environment.None; //pbGetEnvironment();
   //if (!pbGetMetadata($game_map.map_id,MetadataOutdoor)) {
   /*if (Game.TileData[Game.GameData.Player.Area] == Indoor) {
     return 2; // Trash Cloak
   } else*/ if (env==Environment.Sand ||
         env==Environment.Rock ||
         env==Environment.Cave) {
     return 1; // Sandy Cloak
   }
   else {
     return 0; // Plant Cloak
   }
    }
    else if (pokemon.Species == Pokemons.XERNEAS)
    {
   return 1;
    }
    else return pokemon.pokemon.FormId;
}
public static void onSetForm(Pokemon pokemon,int form)
{
    if (pokemon.Species == Pokemons.ROTOM)
    {
   Moves[] moves=new Moves[] {
      Moves.OVERHEAT,  // Heat, Microwave
      Moves.HYDRO_PUMP, // Wash, Washing Machine
      Moves.BLIZZARD,  // Frost, Refrigerator
      Moves.AIR_SLASH,  // Fan
      Moves.LEAF_STORM  // Mow, Lawnmower
   };
   int hasoldmove=-1;
   for (int i = 0; i < 4; i++) {
     for (int j = 0; j < moves.Length; j++) {
       if (pokemon.moves[i].MoveId == moves[j]) {
         hasoldmove=i; break;
       }
     }
     if (hasoldmove>=0) break;
   }
   if (form>0) {
     Moves newmove=moves[form-1];
     if (newmove!=Moves.NONE) { //&& hasConst(PBMoves,newmove)
       if (hasoldmove>=0) {
         // Automatically replace the old form's special move with the new one's
         string oldmovename=pokemon.moves[hasoldmove].MoveId.ToString(TextScripts.Name);
         string newmovename=newmove.ToString(TextScripts.Name);
         pokemon.moves[hasoldmove]=new Attack.Move(newmove);
         Game.pbMessage(_INTL("\\se[]1,\\wt[4] 2,\\wt[4] and...\\wt[8] ...\\wt[8] ...\\wt[8] Poof!\\se[balldrop]\\1"));
         Game.pbMessage(_INTL("{1} forgot how to\r\nuse {2}.\\1",pokemon.Name,oldmovename));
         Game.pbMessage(_INTL("And...\\1"));
         Game.pbMessage(_INTL("\\se[]{1} learned {2}!\\se[MoveLearnt]",pokemon.Name,newmovename));
       }
       else {
         // Try to learn the new form's special move
         //pbLearnMove(pokemon,newmove,true);
         pokemon.pokemon.LearnMove(newmove, out bool s);
       }
     }
   }
   else {
     if (hasoldmove>=0) {
       // Forget the old form's special move
       string oldmovename=pokemon.moves[hasoldmove].MoveId.ToString(TextScripts.Name);
       pokemon.pokemon.DeleteMoveAtIndex(hasoldmove);
       Game.pbMessage(_INTL("{1} forgot {2}...",pokemon.Name,oldmovename));
       if (pokemon.moves.Count(i => i.MoveId!=0)==0) {
         //pbLearnMove(pokemon,Moves.THUNDER_SHOCK);
         pokemon.pokemon.LearnMove(Moves.THUNDER_SHOCK, out bool s);
       }
     }
   }
    }
    else if (pokemon.Species == Pokemons.FURFROU || pokemon.Species == Pokemons.HOOPA)
    {
   pokemon.formTime=(form>0) ? Game.GetTimeNow : (DateTime?)null;
    }
}
public static int getFormOnCreation(Pokemon pokemon)
{
    if (pokemon.Species == Pokemons.UNOWN)
    {
   return Core.Rand.Next(28);
    }
    else if (pokemon.Species == Pokemons.BURMY)
    {
   Environment env=Environment.None; //pbGetEnvironment();
   //if (!pbGetMetadata($game_map.map_id,MetadataOutdoor)) {
   /*if (Game.TileData[Game.GameData.Player.Area] == Indoor) {
     return 2; // Trash Cloak
   } else*/ if (env==Environment.Sand ||
         env==Environment.Rock ||
         env==Environment.Cave) {
     return 1; // Sandy Cloak
   }
   else {
     return 0; // Plant Cloak
   }
    }
    else if (pokemon.Species == Pokemons.WORMADAM)
    {
   Environment env=Environment.None; //pbGetEnvironment();
   //if (!pbGetMetadata($game_map.map_id,MetadataOutdoor)) {
   /*if (Game.TileData[Game.GameData.Player.Area] == Indoor) {
     return 2; // Trash Cloak
   } else*/ if (env==Environment.Sand || env==Environment.Rock ||
      env==Environment.Cave) {
     return 1; // Sandy Cloak
   }
   else {
     return 0; // Plant Cloak
   }
    }
    else if (pokemon.Species == Pokemons.SHELLOS || pokemon.Species == Pokemons.GASTRODON)
    {
   int[] maps=new int[] { 2, 5, 39, 41, 44, 69 };   // Map IDs for second form
   //if ($game_map && maps.Contains($game_map.map_id)) {
   if (Game.GameData != null && maps.Contains(Game.GameData.Player.Area)) {
     return 1;
   }
   else {
     return 0;
   }
    }
    else if (pokemon.Species == Pokemons.BASCULIN)
    {
   return Core.Rand.Next(2);
    }
    else if (pokemon.Species == Pokemons.SCATTERBUG || pokemon.Species == Pokemons.SPEWPA || pokemon.Species == Pokemons.VIVILLON)
    {
   return Game.GameData.Player.Trainer.SecretID%18;
    }
    else if (pokemon.Species == Pokemons.FLABEBE || pokemon.Species == Pokemons.FLOETTE || pokemon.Species == Pokemons.FLORGES)
    {
   return Core.Rand.Next(5);
    }
    else if (pokemon.Species == Pokemons.PUMPKABOO)
    {
   return (int)Math.Min(Core.Rand.Next(4),Core.Rand.Next(4));
    }
    else if (pokemon.Species == Pokemons.GOURGEIST)
    {
   return (int)Math.Min(Core.Rand.Next(4),Core.Rand.Next(4));
    }
    else return pokemon.pokemon.FormId; //0;
}
public static string _INTL(string msg, params object[] obj) { return msg; }
}
}