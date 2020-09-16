using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Monster;
using PokemonUnity.Character;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PokemonUnity.Avatar { 
public class Trainer{
  public string name { get; private set; }
  public int id { get; private set; }
  public int? metaID { get; private set; }
  public TrainerTypes trainertype { get; private set; }
  public int? outfit { get; private set; }
  public bool[] badges { get; private set; }
  public int money { get; private set; }
  public bool[] seen { get; private set; }
  public bool[] owned { get; private set; }
  public int?[][] formseen { get; private set; }
  public int[][] formlastseen { get; private set; }
  public bool[] shadowcaught { get; private set; }
  public Pokemon[] party { get; private set; }
    // Whether the Pokédex was obtained
  public bool pokedex { get; private set; }   
    // Whether the Pokégear was obtained
  public bool pokegear { get; private set; }  
  public Languages? language { get; private set; }
    // Name of this trainer type (localized)
  public string trainerTypeName { get { 
    return @trainertype.ToString() ?? "PkMn Trainer"; }
  }

  public string fullname() {
    return string.Format("{0} {1}",this.trainerTypeName,@name);
  }

    // Portion of the ID which is visible on the Trainer Card
  public int publicID(int? id=null) {   
    return id.HasValue ? (int)id.Value&0xFFFF : this.id&0xFFFF;
  }

    // Other portion of the ID
  public int secretID(int? id=null) {   
    return id.HasValue ? (int)id.Value>>16 : this.id>>16;
  }

    // Random ID other than this Trainer's ID
  public int getForeignID() {   
    int fid=0;
    do { //loop;
      fid=Core.Rand.Next(256);
      fid|=Core.Rand.Next(256)<<8;
      fid|=Core.Rand.Next(256)<<16;
      fid|=Core.Rand.Next(256)<<24;
      if (fid!=@id) break;
    } while (true);
    return fid;
  }

  public void setForeignID(Trainer other) {
    @id=other.getForeignID();
  }

  public int MetaID() {
    if (!@metaID.HasValue && Game.GameData != null) @metaID=Game.GameData.Player.GetHashCode();//ID;
    if (!@metaID.HasValue) @metaID=0;
    return @metaID.Value;
  }

  public int Outfit() {
    if (!@outfit.HasValue) @outfit=0;
    return @outfit.Value;
  }

  public Languages Language() {
    if (!@language.HasValue) @language=Game.pbGetLanguage();
    return @language.Value;
  }

  public int Money{ get { return money; } private set {
    @money=(int)Math.Max((int)Math.Min(value,Core.MAXMONEY),0);
  } }

    // Money won when trainer is defeated
  public int moneyEarned() {   
    int ret=0;
    //pbRgssOpen("Data/trainertypes.dat","rb"){|f|
    //   trainertypes=Marshal.load(f);
    //   if (!Game.TrainerData[@trainertype]) return 30;
       ret=Game.TrainerMetaData[@trainertype].BaseMoney;
    //}
    return ret;
  }

    // Skill level (for AI)
  public int skill () {
    int ret=0;
    //pbRgssOpen("Data/trainertypes.dat","rb"){|f|
    //   trainertypes=Marshal.load(f);
    //   if (!trainertypes[@trainertype]) return 30;
       ret= Game.TrainerMetaData[@trainertype].SkillLevel;
    //}
    return ret;
  }

  public string skillCode() {
    string ret="";
    //pbRgssOpen("Data/trainertypes.dat","rb"){|f|
    //   trainertypes=Marshal.load(f);
    //   if (!trainertypes[@trainertype]) return "";
       ret= Game.TrainerMetaData[@trainertype].SkillCodes.Value.ToString();
    //}
    return ret;
  }

  public bool hasSkillCode(string code) {
    string c=skillCode();
    if (c!=null && c!="" && c.Contains(code)) return true;
    return false;
  }

    // Number of badges
  public int numbadges() {   
    int ret=0;
    for (int i = 0; i < @badges.Length; i++) {
      if (@badges[i]) ret+=1;
    }
    return ret;
  }

  public bool? gender { get { 
    bool? ret=null;   // 2 = gender unknown
    //pbRgssOpen("Data/trainertypes.dat","rb"){|f|
    //   trainertypes=Marshal.load(f);
       //if (!trainertypes || !trainertypes[trainertype]) {
       //  ret=null;
       //}
       //else {
         ret= Game.TrainerMetaData[@trainertype].Gender;
         //if (!ret.HasValue) ret=null;
       //}
    //}
    return ret;
  } }

  public bool isMale() { 
    return this.gender==true; }
  public bool isFemale() { 
    return this.gender==false; }

  public IEnumerable<Pokemon> pokemonParty() {
    return @party.Where(item => item.IsNotNullOrNone() && !item.isEgg );
  }

  public IEnumerable<Pokemon> ablePokemonParty() {
    return @party.Where(item => item.IsNotNullOrNone() && !item.isEgg && item.HP>0 );
  }

  public int partyCount() {
    return @party.Length;
  }

  public int pokemonCount() {
    int ret=0;
    for (int i = 0; i < @party.Length; i++) {
      if (@party[i].IsNotNullOrNone() && !@party[i].isEgg) ret+=1;
    }
    return ret;
  }

  public int ablePokemonCount() {
    int ret=0;
    for (int i = 0; i < @party.Length; i++) {
      if (@party[i].IsNotNullOrNone() && !@party[i].isEgg && @party[i].HP>0) ret+=1;
    }
    return ret;
  }

  public Pokemon firstParty() {
    if (@party.Length==0) return new Pokemon();
    return @party[0];
  }

  public Pokemon firstPokemon() {
    Pokemon[] p=this.pokemonParty().ToArray();
    if (p.Length==0) return new Pokemon();
    return p[0];
  }

  public Pokemon firstAblePokemon() {
    Pokemon[] p=this.ablePokemonParty().ToArray();
    if (p.Length==0) return new Pokemon();
    return p[0];
  }

  public Pokemon lastParty() {
    if (@party.Length==0) return new Pokemon();
    return @party[@party.Length-1];
  }

  public Pokemon lastPokemon() {
    Pokemon[] p=this.pokemonParty().ToArray();
    if (p.Length==0) return new Pokemon();
    return p[p.Length-1];
  }

  public Pokemon lastAblePokemon() {
    Pokemon[] p=this.ablePokemonParty().ToArray();
    if (p.Length==0) return new Pokemon();
    return p[p.Length-1];
  }

    // Number of Pokémon seen
  public int pokedexSeen(Regions? region=null) {   
    int ret=0;
    if (region==null) {
      for (int i = 0; i < Game.PokemonData.Count; i++) {
        if (@seen[i]) ret+=1;
      }
    }
    else {
      //int[] regionlist=Game.pbAllRegionalSpecies(region);
      int[] regionlist=new int[0];
      foreach (var i in regionlist) {
        if (@seen[i]) ret+=1;
      }
    }
    return ret;
  }

    // Number of Pokémon owned
  public int pokedexOwned(Regions? region=null) {   
    int ret=0;
    if (region==null) {
      for (int i = 0; i < Game.PokemonData.Count; i++) {
        if (@owned[i]) ret+=1;
      }
    }
    else {
      //int[] regionlist=Game.pbAllRegionalSpecies(region);
      int[] regionlist=new int[0];
      foreach (var i in regionlist) {
        if (@owned[i]) ret+=1;
      }
    }
    return ret;
  }

  public int numFormsSeen(Pokemons species) {
    int ret=0;
    int?[][] array=new int?[0][];//@formseen[species];
    //Game.GameData.Player.Pokedex[(int)species,2];
    for (int i = 0; i < (int)Math.Max(array[0].Length,array[1].Length); i++) {
      if (array[0][i].HasValue || array[1][i].HasValue) ret+=1;
    }
    return ret;
  }

  public bool hasSeen (Pokemons species) {
    //if (Pokemons.is_a(String) || Pokemons.is_a(Symbol)) {
    //  species=getID(PBSpecies,species);
    //}
    return species>0 ? @seen[(int)species] : false;
  }

  public bool hasOwned (Pokemons species) {
    //if (species.is_a(String) || species.is_a(Symbol)) {
    //  species=getID(PBSpecies,species);
    //}
    return species>0 ? @owned[(int)species] : false;
  }

  public void setSeen(Pokemons species) {
    //if (species.is_a(String) || species.is_a(Symbol)) {
    //  species=getID(PBSpecies,species);
    //}
    if (species>0) @seen[(int)species]=true;
  }

  public void setOwned(Pokemons species) {
    //if (species.is_a(String) || species.is_a(Symbol)) {
    //  species=getID(PBSpecies,species);
    //}
    if (species>0) @owned[(int)species]=true;
  }

  public void clearPokedex() {
    @seen=new bool[0];
    @owned=new bool[0];
    @formseen=new int?[0][];
    @formlastseen=new int[0][];
    //for (int i = 1; i < Game.PokemonData.Count; i++) {
    //  @seen[i]=false;
    //  @owned[i]=false;
    //  @formlastseen[i]=[];
    //  @formseen[i]=new int?[]{null};
    //}
  }

  Trainer (string name,TrainerTypes trainertype) {
    this.name=name;
    @language=Game.pbGetLanguage();
    this.trainertype=trainertype;
    @id=Core.Rand.Next(256);
    @id|=Core.Rand.Next(256)<<8;
    @id|=Core.Rand.Next(256)<<16;
    @id|=Core.Rand.Next(256)<<24;
    @metaID=0;
    @outfit=0;
    @pokegear=false;
    @pokedex=false;
    clearPokedex();
    @shadowcaught=new bool[0];
    for (int i = 1; i < Game.PokemonData.Count; i++) {
      @shadowcaught[i]=false;
    }
    @badges=new bool[0];
    for (int i = 0; i < 8; i++) {
      @badges[i]=false;
    }
    @money=Core.INITIALMONEY;
    @party=new Pokemon[Core.MAXPARTYSIZE];
  }
}
}