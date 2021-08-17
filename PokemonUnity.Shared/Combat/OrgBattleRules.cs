using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Inventory;

namespace PokemonUnity
{
public partial class Game { 
public static int pbBaseStatTotal(Pokemons species) {
  //dexdata=pbOpenDexData();
  //pbDexDataOffset(dexdata,species,10);
  //int bst=dexdata.fgetb;
  int bst=PokemonData[species].BaseStatsHP; //dexdata.fgetb;
  bst+=PokemonData[species].BaseStatsATK; //dexdata.fgetb;
  bst+=PokemonData[species].BaseStatsDEF; //dexdata.fgetb;
  bst+=PokemonData[species].BaseStatsSPE; //dexdata.fgetb;
  bst+=PokemonData[species].BaseStatsSPA; //dexdata.fgetb;
  bst+=PokemonData[species].BaseStatsSPD; //dexdata.fgetb;
  //dexdata.close();
  return bst;
}

public static int pbBalancedLevelFromBST(Pokemons species) {
  return (int)Math.Round(113-(pbBaseStatTotal(species)*0.072));
}

public static bool pbTooTall (Monster.Pokemon pokemon,double maxHeightInMeters) {
  //dexdata=pbOpenDexData();
  //pbDexDataOffset(dexdata,pokemon is Numeric ? pokemon : pokemon.Species,33);
  float height=PokemonData[pokemon.Species].Height; //dexdata.fgetw;
  float weight=PokemonData[pokemon.Species].Weight; //dexdata.fgetw;
  maxHeightInMeters=Math.Round(maxHeightInMeters*10);
  //dexdata.close();
  return height>maxHeightInMeters;
}

public static bool pbTooHeavy (Monster.Pokemon pokemon,double maxWeightInKg) {
  //dexdata=pbOpenDexData();
  //pbDexDataOffset(dexdata,pokemon is Numeric ? pokemon : pokemon.Species,33);
  float height=PokemonData[pokemon.Species].Height; //dexdata.fgetw;
  float weight=PokemonData[pokemon.Species].Weight; //dexdata.fgetw;
  maxWeightInKg=Math.Round(maxWeightInKg*10f);
  //dexdata.close();
  return weight>maxWeightInKg;
}



public partial class LevelAdjustment {
  public const int BothTeams          = 0;
  public const int EnemyTeam          = 1;
  public const int MyTeam             = 2;
  public const int BothTeamsDifferent = 3;
            private int adjustment;

  public virtual int type { get {
    return @adjustment;
  } }

  public LevelAdjustment(int adjustment) {
    this.adjustment=adjustment;
  }

  public static int[] getNullAdjustment(Monster.Pokemon[] thisTeam,Monster.Pokemon[] otherTeam) {
    int[] ret=new int[6];
    for (int i = 0; i < thisTeam.Length; i++) {
      ret[i]=thisTeam[i].Level;
    }
    return ret;
  }

  public virtual int[] getAdjustment(Monster.Pokemon[] thisTeam,Monster.Pokemon[] otherTeam) {
    return LevelAdjustment.getNullAdjustment(thisTeam,otherTeam);
  }

  public int?[] getOldExp(Monster.Pokemon[] team1,Monster.Pokemon[] team2) {
    //List<int> ret=new List<int>();
    int?[] ret=new int?[6];
    for (int i = 0; i < team1.Length; i++) {
      //ret.Add(team1[i].exp);
      //ret.Add(team1[i].Experience.Total);
      if(team1[i].IsNotNullOrNone()) ret[i] = team1[i].Experience.Total;
    }
    //return ret.ToArray();
    return ret;
  }

  public virtual void unadjustLevels(Monster.Pokemon[] team1,Monster.Pokemon[] team2,int?[][] adjustments) {
    for (int i = 0; i < team1.Length; i++) {
      int? exp=adjustments[0][i];
      if (exp != null && team1[i].Experience.Total!=exp) {
        team1[i].Exp=exp.Value;
        team1[i].calcStats();
      }
    }
    for (int i = 0; i < team2.Length; i++) {
      int? exp=adjustments[1][i];
      if (exp != null && team2[i].Experience.Total!=exp) {
        team2[i].Exp=exp.Value;
        team2[i].calcStats();
      }
    }
  }

  public virtual int?[][] adjustLevels(Monster.Pokemon[] team1,Monster.Pokemon[] team2) {
    int[] adj1=null;
    int[] adj2=null;
    int?[][] ret= new int?[][]{ getOldExp(team1,team2),getOldExp(team2,team1) };
    if (@adjustment==BothTeams || @adjustment==MyTeam) {
      adj1=getAdjustment(team1,team2);
    } else if (@adjustment==BothTeamsDifferent) {
      adj1=getMyAdjustment(team1,team2);
    }
    if (@adjustment==BothTeams || @adjustment==EnemyTeam) {
      adj2=getAdjustment(team2,team1);
    } else if (@adjustment==BothTeamsDifferent) {
      adj2=getTheirAdjustment(team2,team1);
    }
    if (adj1 != null) {
      for (int i = 0; i < team1.Length; i++) {
        if (team1[i].Level!=adj1[i]) {
          //team1[i].Level=adj1[i];
          team1[i].SetLevel((byte)adj1[i]);
          team1[i].calcStats();
        }
      }
    }
    if (adj2 != null) {
      for (int i = 0; i < team2.Length; i++) {
        if (team2[i].Level!=adj2[i]) {
          //team2[i].Level=adj2[i];
          team2[i].SetLevel((byte)adj2[i]);
          team2[i].calcStats();
        }
      }
    }
    return ret;
  }
  public virtual int[] getMyAdjustment(Monster.Pokemon[] myTeam,Monster.Pokemon[] theirTeam) {
    return getNullAdjustment(myTeam,theirTeam);
  }
  public virtual int[] getTheirAdjustment(Monster.Pokemon[] theirTeam,Monster.Pokemon[] myTeam) {
    return getNullAdjustment(theirTeam,myTeam);
  }
}



public partial class LevelBalanceAdjustment : LevelAdjustment {
            private int minLevel;
  public LevelBalanceAdjustment(int minLevel) : base(LevelAdjustment.BothTeams) {
    //base.initialize(LevelAdjustment.BothTeams);
    this.minLevel=minLevel;
  }

  public override int[] getAdjustment(Monster.Pokemon[] thisTeam,Monster.Pokemon[] otherTeam) {
    int[] ret=new int[6];
    for (int i = 0; i < thisTeam.Length; i++) {
      ret[i]=Game.pbBalancedLevelFromBST(thisTeam[i].Species);
    }
    return ret;
  }
}



public partial class EnemyLevelAdjustment : LevelAdjustment {
            private int level;
  public EnemyLevelAdjustment(int level) : base(LevelAdjustment.EnemyTeam) {
    //base.initialize(LevelAdjustment.EnemyTeam);
    this.level=Math.Min(Math.Max(1,level),Core.MAXIMUMLEVEL);
  }

  public override int[] getAdjustment(Monster.Pokemon[] thisTeam,Monster.Pokemon[] otherTeam) {
    int[] ret=new int[6];
    for (int i = 0; i < thisTeam.Length; i++) {
      ret[i]=@level;
    }
    return ret;
  }
}



public partial class CombinedLevelAdjustment : LevelAdjustment {
            private LevelAdjustment my;
            private LevelAdjustment their;
  public CombinedLevelAdjustment(LevelAdjustment my,LevelAdjustment their) : base(LevelAdjustment.BothTeamsDifferent) {
    //base.initialize(LevelAdjustment.BothTeamsDifferent);
    this.my=my;
    this.their=their;
  }
  
  public override int[] getMyAdjustment(Monster.Pokemon[] myTeam,Monster.Pokemon[] theirTeam) {
    return @my != null ? @my.getAdjustment(myTeam,theirTeam) : 
       LevelAdjustment.getNullAdjustment(myTeam,theirTeam);
  }

  public override int[] getTheirAdjustment(Monster.Pokemon[] theirTeam,Monster.Pokemon[] myTeam) {
    return @their != null ? @their.getAdjustment(theirTeam,myTeam) : 
       LevelAdjustment.getNullAdjustment(theirTeam,myTeam);
  }
}



public partial class SinglePlayerCappedLevelAdjustment : CombinedLevelAdjustment {
  public SinglePlayerCappedLevelAdjustment(int level) : base(new CappedLevelAdjustment(level),new FixedLevelAdjustment(level)) {
    //base.initialize(new CappedLevelAdjustment(level),new FixedLevelAdjustment(level));
  }
}



public partial class CappedLevelAdjustment : LevelAdjustment {
            private int level;
  public CappedLevelAdjustment(int level) : base(LevelAdjustment.BothTeams) {
    //base.initialize(LevelAdjustment.BothTeams);
    this.level=Math.Min(Math.Max(1,level),Core.MAXIMUMLEVEL);
  }

  public override int[] getAdjustment(Monster.Pokemon[] thisTeam,Monster.Pokemon[] otherTeam) {
    int[] ret=new int[6];
    for (int i = 0; i < thisTeam.Length; i++) {
      ret[i]=Math.Min(thisTeam[i].Level,@level);
    }
    return ret;
  }
}



public partial class FixedLevelAdjustment : LevelAdjustment {
            private int level;
  public FixedLevelAdjustment(int level) : base(LevelAdjustment.BothTeams) {
    //base.initialize(LevelAdjustment.BothTeams);
    this.level=Math.Min(Math.Max(1,level),Core.MAXIMUMLEVEL);
  }

  public override int[] getAdjustment(Monster.Pokemon[] thisTeam,Monster.Pokemon[] otherTeam) {
    int[] ret=new int[6];
    for (int i = 0; i < thisTeam.Length; i++) {
      ret[i]=@level;
    }
    return ret;
  }
}



public partial class TotalLevelAdjustment : LevelAdjustment {
            private int minLevel;
            private int maxLevel;
            private int totalLevel;
  public TotalLevelAdjustment(int minLevel,int maxLevel,int totalLevel) : base(LevelAdjustment.EnemyTeam) {
    //base.initialize(LevelAdjustment.EnemyTeam);
    this.minLevel=Math.Min(Math.Max(1,minLevel),Core.MAXIMUMLEVEL);
    this.maxLevel=Math.Min(Math.Max(1,maxLevel),Core.MAXIMUMLEVEL);
    this.totalLevel=totalLevel;
  }

  public override int[] getAdjustment(Monster.Pokemon[] thisTeam,Monster.Pokemon[] otherTeam) {
    int[] ret=new int[6];
    int total=0;
    for (int i = 0; i < thisTeam.Length; i++) {
      ret[i]=@minLevel;
      total+=@minLevel;
    }
    do { //;loop
      bool work=false;
      for (int i = 0; i < thisTeam.Length; i++) {
        if (ret[i]>=@maxLevel || total>=@totalLevel) {
          continue;
        }
        ret[i]+=1;
        total+=1;
        work=true;
      }
      if (!work) break;
    } while (true);
    return ret;
  }
}



public partial class OpenLevelAdjustment : LevelAdjustment {
            private int minLevel;
  public OpenLevelAdjustment(int minLevel=1) : base(LevelAdjustment.EnemyTeam) {
    //base.initialize(LevelAdjustment.EnemyTeam);
    this.minLevel=minLevel;
  }

  public override int[] getAdjustment(Monster.Pokemon[] thisTeam,Monster.Pokemon[] otherTeam) {
    int maxLevel=1;
    for (int i = 0; i < otherTeam.Length; i++) {
      int level=otherTeam[i].Level;
      if (maxLevel<level) maxLevel=level;
    }
    if (maxLevel<@minLevel) maxLevel=@minLevel;
    int[] ret=new int[6];
    for (int i = 0; i < thisTeam.Length; i++) {
      ret[i]=maxLevel;
    }
    return ret;
  }
}



public partial class NonEggRestriction : IBattleRestriction {
  public bool isValid (Monster.Pokemon pokemon) {
    return pokemon != null && !pokemon.isEgg;
  }
}



public partial class AblePokemonRestriction : IBattleRestriction {
  public bool isValid (Monster.Pokemon pokemon) {
    return pokemon != null && !pokemon.isEgg && pokemon.HP>0;
  }
}



public partial class SpeciesRestriction : IBattleRestriction {
            private Pokemons[] specieslist;
  public SpeciesRestriction(params Pokemons[] specieslist) {
    this.specieslist=specieslist;
  }

  public bool isSpecies (Pokemons species,Pokemons[] specieslist) {
    foreach (var s in specieslist) {
      if (species == s) return true;
    }
    return false;
  }

  public bool isValid (Monster.Pokemon pokemon) {
    int count=0;
    if (isSpecies(pokemon.Species,@specieslist)) {
      count+=1;
    }
    return count!=0;
  }
}



public partial class BannedSpeciesRestriction : IBattleRestriction {
            private Pokemons[] specieslist;
  public BannedSpeciesRestriction(params Pokemons[] specieslist) {
    this.specieslist=specieslist;
  }

  public bool isSpecies (Pokemons species,Pokemons[] specieslist) {
    foreach (var s in specieslist) {
      if (species == s) return true;
    }
    return false;
  }

  public bool isValid (Monster.Pokemon pokemon) {
    int count=0;
    if (isSpecies(pokemon.Species,@specieslist)) {
      count+=1;
    }
    return count==0;
  }
}



public partial class BannedItemRestriction : IBattleRestriction {
            private Items[] specieslist;
  public BannedItemRestriction(params Items[] specieslist) {
    this.specieslist=specieslist;
  }

  //public bool isSpecies (Pokemons species,Pokemons[] specieslist) {
  public bool isSpecies (Items species,Items[] specieslist) {
    foreach (var s in specieslist) {
      if (species == s) return true;
    }
    return false;
  }

  public bool isValid (Monster.Pokemon pokemon) {
    int count=0;
    if (pokemon.Item!=0 && isSpecies(pokemon.Item,@specieslist)) {
      count+=1;
    }
    return count==0;
  }
}



public partial class RestrictedSpeciesRestriction : IBattleTeamRestriction {
			string IBattleTeamRestriction.errorMessage { get; }
            private int maxValue;
            private Pokemons[] specieslist;
  public RestrictedSpeciesRestriction(int maxValue,params Pokemons[] specieslist) {
    this.specieslist=specieslist;
    this.maxValue=maxValue;
  }

  public bool isSpecies (Pokemons species,Pokemons[] specieslist) {
    foreach (var s in specieslist) {
      if (species == s) return true;
    }
    return false;
  }

  public bool isValid (Monster.Pokemon[] team) {
    int count=0;
    for (int i = 0; i < team.Length; i++) {
      if (isSpecies(team[i].Species,@specieslist)) {
        count+=1;
      }
    }
    return count<=@maxValue;
  }
}



public partial class RestrictedSpeciesTeamRestriction : RestrictedSpeciesRestriction {
  public RestrictedSpeciesTeamRestriction(params Pokemons[] specieslist) : base(4,specieslist) {
    //base.initialize(4,*specieslist);
  }
}



public partial class RestrictedSpeciesSubsetRestriction : RestrictedSpeciesRestriction {
  public RestrictedSpeciesSubsetRestriction(params Pokemons[] specieslist) : base(2,specieslist) {
    //base.initialize(2,*specieslist);
  }
}



public partial class StandardRestriction : IBattleRestriction {
  public bool isValid (Monster.Pokemon pokemon) {
    if (pokemon == null || pokemon.isEgg) return false;
    //dexdata=pbOpenDexData();
    //pbDexDataOffset(dexdata,pokemon.Species,10);
    int basestatsum=PokemonData[pokemon.Species].BaseStatsHP; //dexdata.fgetb;
    basestatsum+=PokemonData[pokemon.Species].BaseStatsATK; //dexdata.fgetb;
    basestatsum+=PokemonData[pokemon.Species].BaseStatsDEF; //dexdata.fgetb;
    basestatsum+=PokemonData[pokemon.Species].BaseStatsSPE; //dexdata.fgetb;
    basestatsum+=PokemonData[pokemon.Species].BaseStatsSPD; //dexdata.fgetb;
    basestatsum+=PokemonData[pokemon.Species].BaseStatsSPA; //dexdata.fgetb;
    //pbDexDataOffset(dexdata,pokemon.Species,2);
    Abilities ability1=PokemonData[pokemon.Species].Ability[0]; //dexdata.fgetw;
    Abilities ability2=PokemonData[pokemon.Species].Ability[1]; //dexdata.fgetw;
    //dexdata.close();
    //  Species with disadvantageous abilities are not banned
    if (ability1 == Abilities.TRUANT ||
       ability2 == Abilities.SLOW_START) {
      return true;
    }
    //  Certain named species are banned
    Pokemons[] blacklist= new Pokemons[]{ Pokemons.WYNAUT,Pokemons.WOBBUFFET };
    foreach (var i in blacklist) {
      if (pokemon.Species == i) return false;
    }
    //  Certain named species are not banned
    Pokemons[] whitelist= new Pokemons[]{ Pokemons.DRAGONITE,Pokemons.SALAMENCE,Pokemons.TYRANITAR };
    foreach (Pokemons i in whitelist) {
      if (pokemon.Species == i) return true;
    }
    //  Species with total base stat 600 or more are banned
    if (basestatsum>=600) {
      return false;
    }
    return true;
  }
}



public static partial class LevelRestriction { }

  
  
public partial class MinimumLevelRestriction : IBattleRestriction {
  public int level				{ get; protected set; }

  public MinimumLevelRestriction(int minLevel) {
    @level=minLevel;
  }

  public bool isValid (Monster.Pokemon pokemon) {
    return pokemon.Level>=@level;
  }
}



public partial class MaximumLevelRestriction : IBattleRestriction {
  public int level				{ get; protected set; }

  public MaximumLevelRestriction(int maxLevel) {
    @level=maxLevel;
  }

  public bool isValid (Monster.Pokemon pokemon) {
    return pokemon.Level<=@level;
  }
}



public partial class HeightRestriction : IBattleRestriction {
            private int level;
  public HeightRestriction(int maxHeightInMeters) {
    @level=maxHeightInMeters;
  }

  public bool isValid (Monster.Pokemon pokemon) {
    return !Game.pbTooTall(pokemon,@level);
  }
}



public partial class WeightRestriction : IBattleRestriction {
            private int level;
  public WeightRestriction(int maxWeightInKg) {
    @level=maxWeightInKg;
  }

  public bool isValid (Monster.Pokemon pokemon) {
    return !Game.pbTooHeavy(pokemon,@level);
  }
}



public partial class SoulDewClause : IBattleRestriction {
  public bool isValid (Monster.Pokemon pokemon) {
    return pokemon.Item != Items.SOUL_DEW;
  }
}



public partial class ItemsDisallowedClause : IBattleRestriction {
  public bool isValid (Monster.Pokemon pokemon) {
    return !pokemon.hasItem();
  }
}



public partial class NegativeExtendedGameClause : IBattleRestriction {
  public bool isValid (Monster.Pokemon pokemon) {
    if (pokemon.Species == Pokemons.ARCEUS) return false;
    if (pokemon.Item == Items.MICLE_BERRY) return false;
    if (pokemon.Item == Items.CUSTAP_BERRY) return false;
    if (pokemon.Item == Items.JABOCA_BERRY) return false;
    if (pokemon.Item == Items.ROWAP_BERRY) return false;
    return true;
  }
}



public partial class TotalLevelRestriction : IBattleTeamRestriction {
  public int level				{ get; protected set; }

  public TotalLevelRestriction(int level) {
    this.level=level;
  }

  public bool isValid (Monster.Pokemon[] team) {
    int totalLevel=0;
    for (int i = 0; i < team.Length-1; i++) {
      if (team[i].Species==0) continue;
      totalLevel+=team[i].Level;
    }
    return (totalLevel<=@level);
  }

  public string errorMessage { get {
    return string.Format("The combined levels exceed {0}.",@level);
  } }
}



public partial class SameSpeciesClause : IBattleTeamRestriction {
  public bool isValid (Monster.Pokemon[] team) {
    Pokemons species=0;
    for (int i = 0; i < team.Length-1; i++) {
      if (team[i].Species==0) continue;
      if (species==0) {
        species=team[i].Species;
      } else {
        if (team[i].Species!=species) return false;
      }
    }
    return true;
  }

  public string errorMessage { get {
    return Game._INTL("Pokémon can't be the same.");
  } }
}



public partial class SpeciesClause : IBattleTeamRestriction {
  public bool isValid (Monster.Pokemon[] team) {
    for (int i = 0; i < team.Length-1; i++) {
      if (team[i].Species==0) continue;
      for (int j = i+1; j < team.Length; j++) {
        if (team[i].Species==team[j].Species) return false;
      }
    }
    return true;
  }

  public string errorMessage { get {
    return Game._INTL("Pokémon can't be the same.");
  } }
}



//public static Pokemons[] babySpeciesData = {}
//public static Pokemons[] canEvolve       = {}



public partial class BabyRestriction : IBattleRestriction {
  public bool isValid (Monster.Pokemon pokemon) {
    //baby=babySpeciesData[pokemon.Species] != null ? babySpeciesData[pokemon.Species] :
    //   (babySpeciesData[pokemon.Species]=pbGetBabySpecies(pokemon.Species));
    //return baby==pokemon.Species;
    return Game.PokemonData[pokemon.Species].IsBaby;
  }
}



public partial class UnevolvedFormRestriction : IBattleRestriction {
  public bool isValid (Monster.Pokemon pokemon) {
    //baby=$babySpeciesData[pokemon.Species] ? $babySpeciesData[pokemon.Species] :
    //   ($babySpeciesData[pokemon.Species]=pbGetBabySpecies(pokemon.Species))
    //if (baby!=pokemon.Species) return false;
    if (!Game.PokemonData[pokemon.Species].IsBaby) return false;
    //bool canEvolve=(canEvolve[pokemon.Species]!=null) ? canEvolve[pokemon.Species] :
    //   (canEvolve[pokemon.Species]=(pbGetEvolvedFormData(pokemon.Species).Length!=0));
    bool canEvolve=Game.PokemonEvolutionsData[pokemon.Species].Length!=0;
    if (!canEvolve) return false;
    return true;
  }
}



public partial class LittleCupRestriction : IBattleRestriction {
  public bool isValid (Monster.Pokemon pokemon) {
    if (pokemon.Item == Items.BERRY_JUICE) return false;
    if (pokemon.Item == Items.DEEP_SEA_TOOTH) return false;
    if (pokemon.hasMove(Moves.SONIC_BOOM)) return false;
    if (pokemon.hasMove(Moves.DRAGON_RAGE)) return false;
    if (pokemon.Species == Pokemons.SCYTHER) return false;
    if (pokemon.Species == Pokemons.SNEASEL) return false;
    if (pokemon.Species == Pokemons.MEDITITE) return false;
    if (pokemon.Species == Pokemons.YANMA) return false;
    if (pokemon.Species == Pokemons.TANGELA) return false;
    if (pokemon.Species == Pokemons.MURKROW) return false;
    return true;
  }
}



public partial class ItemClause : IBattleTeamRestriction {
  public bool isValid (Monster.Pokemon[] team) {
    for (int i = 0; i < team.Length-1; i++) {
      if (!team[i].hasItem()) continue;
      for (int j = i+1; j < team.Length; j++) {
        if (team[i].Item==team[j].Item) return false;
      }
    }
    return true;
  }

  public string errorMessage { get {
    return Game._INTL("No identical hold items.");
  } }
}



public partial class NicknameChecker {
  public static Dictionary<Pokemons,string> names=new Dictionary<Pokemons, string>();
  public static int namesMaxValue=0;

  public static string getName(Pokemons species) {
    string n=names[species];
    if (n != null) return n;
    n=species.ToString(TextScripts.Name);
    names[species]=n.ToUpper(); //.upcase;
    return n;
  }

  public static bool check(string name,Pokemons species) {
    name=name.ToUpper(); //.upcase;
    if (name==getName(species)) return true;
    if (@names.Values.Contains(name)) {
      return false;
    }
    //foreach (var i in @@namesMaxValue..PBSpecies.maxValue) {
    foreach (Pokemons i in PokemonData.Keys) {
      if (i!=species) {
        string n=getName(i);
        if (n==name) return false;
      }
    }
    return true;
  }
}



/// <summary>
/// No two Pokemon can have the same nickname.
/// No nickname can be the same as the (real) name of another Pokemon character.
/// </summary>
public partial class NicknameClause : IBattleTeamRestriction {
  public bool isValid (Monster.Pokemon[] team) {
    for (int i = 0; i < team.Length-1; i++) {
      for (int j = i+1; j < team.Length; j++) {
        if (team[i].Name==team[j].Name) return false;
        if (!NicknameChecker.check(team[i].Name,team[i].Species)) return false;
      }
    }
    return true;
  }

  public string errorMessage { get {
    return Game._INTL("No identical nicknames.");
  } }
}



public partial class PokemonRuleSet {
  public int minTeamLength { get {
    return Math.Min(1,this.minLength);
  } }

  public int maxTeamLength { get {
    return Math.Max(6,this.maxLength);
  } }

  public int minLength { get {
    return _minLength.HasValue ? _minLength.Value : this.maxLength;
  } }

  public int maxLength { get {
    return @number<0 ? 6 : @number;
  } }

  public int number { get {
    return _maxLength;
  } }
            private int _maxLength;
            private int? _minLength;
            private List<IBattleRestriction> pokemonRules;
            private List<IBattleTeamRestriction> teamRules;
            private List<IBattleTeamRestriction> subsetRules;

  public PokemonRuleSet(int number=0) {
    @pokemonRules=new List<IBattleRestriction>();
    @teamRules=new List<IBattleTeamRestriction>();
    @subsetRules=new List<IBattleTeamRestriction>();
    _minLength=1;
    _maxLength=number <= 0 ? GameData.Features.LimitPokemonPartySize : number;
  }

  public PokemonRuleSet copy() {
    PokemonRuleSet ret=new PokemonRuleSet(@number);
    foreach (var rule in @pokemonRules) {
      ret.addPokemonRule(rule);
    }
    foreach (var rule in @teamRules) {
      ret.addTeamRule(rule);
    }
    foreach (var rule in @subsetRules) {
      ret.addSubsetRule(rule);
    }
    return ret;
  }

/// <summary>
/// Returns the length of a valid subset of a Pokemon team.
/// </summary>
  public int suggestedNumber { get {
    return this.maxLength;
  } }

/// <summary>
/// Returns a valid level to assign to each member of a valid Pokemon team.
/// </summary>
/// <returns></returns>
  public int suggestedLevel() {
    int minLevel=1;
    int maxLevel=Core.MAXIMUMLEVEL;
    int num=this.suggestedNumber;
    foreach (var rule in @pokemonRules) {
      if (rule is MinimumLevelRestriction r1) {
        minLevel=r1.level; //rule.level;
      } else if (rule is MaximumLevelRestriction r2) {
        maxLevel=r2.level; //rule.level;
      }
    }
    int totalLevel=maxLevel*num;
    foreach (var rule in @subsetRules) {
      if (rule is TotalLevelRestriction r) {
        totalLevel=r.level; //rule.level;
      }
    }
    if (totalLevel>=maxLevel*num) {
      return Math.Max(maxLevel,minLevel);
    } else {
      return Math.Max((totalLevel/this.suggestedNumber),minLevel);
    }
  }

  public PokemonRuleSet setNumberRange(int minValue,int maxValue) {
    _minLength= Math.Max(1,minValue);
    _maxLength= Math.Min(maxValue,6);
    return this;
  }

  public PokemonRuleSet setNumber(int value) {
    return setNumberRange(value,value);
  }

  public PokemonRuleSet addPokemonRule(IBattleRestriction rule) {
    @pokemonRules.Add(rule);
    return this;
  }

//  This rule checks
//  - the entire team to determine whether a subset of the team meets the rule, or 
//  - a list of Pokemon whose length is equal to the suggested number. For an
//    entire team, the condition must hold for at least one possible subset of
//    the team, but not necessarily for the entire team.
//  A subset rule is "number-dependent", that is, whether the condition is likely
//  to hold depends on the number of Pokemon in the subset.
//  Example of a subset rule:
//  - The combined level of X Pokemon can't exceed Y.
  public PokemonRuleSet addSubsetRule(IBattleTeamRestriction rule) {
    @teamRules.Add(rule);
    return this;
  }

/// <summary>
///  This rule checks either<para>
///  - the entire team to determine whether a subset of the team meets the rule, or </para>
///  - whether the entire team meets the rule. If the condition holds for the
///    entire team, the condition must also hold for any possible subset of the
///    team with the suggested number.
/// </summary>
/// <example>
///  Examples of team rules:
///  - No two Pokemon can be the same species.
///  - No two Pokemon can hold the same items.
/// </example>
/// <param name="rule"></param>
/// <returns></returns>
  public PokemonRuleSet addTeamRule(IBattleTeamRestriction rule) {
    @teamRules.Add(rule);
    return this;
  }

  public PokemonRuleSet clearPokemonRules() {
    @pokemonRules.Clear();
    return this;
  }

  public PokemonRuleSet clearTeamRules() {
    @teamRules.Clear();
    return this;
  }

  public PokemonRuleSet clearSubsetRules() {
    @subsetRules.Clear();
    return this;
  }

  public bool isPokemonValid (Monster.Pokemon pokemon) {
    if (!pokemon.IsNotNullOrNone()) return false;
    foreach (var rule in @pokemonRules) {
      if (!rule.isValid(pokemon)) {
        return false;
      }
    }
    return true;
  }

  public bool hasRegistrableTeam (Monster.Pokemon[] list) {
    if (list == null || list.Length<this.minTeamLength) return false;
    //pbEachCombination(list,this.maxTeamLength){//Monster.Pokemon[] |comb|
    //   if (canRegisterTeam(comb)) return true;
    //}
    return false;
  }

/// <summary>
///  Returns true if the team's length is greater or equal to the suggested number
///  and is 6 or less, the team as a whole meets the requirements of any team
///  rules, and at least one subset of the team meets the requirements of any
///  subset rules. Each Pokemon in the team must be valid.
/// </summary>
/// <param name="team"></param>
/// <returns></returns>
  public bool canRegisterTeam (Monster.Pokemon[] team) {
    if (team == null || team.Length<this.minTeamLength) {
      return false;
    }
    if (team.Length>this.maxTeamLength) {
      return false;
    }
    int teamNumber=Math.Min(this.maxLength,team.Length);
    foreach (var pokemon in team) {
      if (!isPokemonValid(pokemon)) {
        return false;
      }
    }
    foreach (var rule in @teamRules) {
      if (!rule.isValid(team)) {
        return false;
      }
    }
    if (@subsetRules.Count>0) {
      //pbEachCombination(team,teamNumber){|comb|
      //   bool isValid=true;
      //   foreach (var rule in @subsetRules) {
      //     if (!rule.isValid(comb)) {
      //       isValid=false;
      //       break;
      //     }
      //   }
      //   if (isValid) return true;
      //}
      return false;
    }
    return true;
  }

/// <summary>
///  Returns true if the team's length is greater or equal to the suggested number
///  and at least one subset of the team meets the requirements of any team rules
///  and subset rules. Not all Pokemon in the team have to be valid.
/// </summary>
/// <param name="team"></param>
/// <returns></returns>
  public bool hasValidTeam (Monster.Pokemon[] team) {
    if (team == null || team.Length<this.minTeamLength) {
      return false;
    }
    int teamNumber=Math.Min(this.maxLength,team.Length);
    List<Monster.Pokemon> validPokemon=new List<Monster.Pokemon>();
    foreach (var pokemon in team) {
      if (isPokemonValid(pokemon)) {
        validPokemon.Add(pokemon);
      }
    }
    if (validPokemon.Count<teamNumber) {
      return false;
    }
    if (@teamRules.Count>0) {
      //pbEachCombination(team,teamNumber){|comb|
      //   if (isValid(comb)) {
      //     return true;
      //   }
      //}
      return false;
    }
    return true;
  }

/// <summary>
///  Returns true if the team's length meets the subset length range requirements
///  and the team meets the requirements of any team rules and subset rules. Each
///  Pokemon in the team must be valid.
/// </summary>
/// <param name="team"></param>
/// <param name="error"></param>
/// <returns></returns>
  public bool isValid (Monster.Pokemon[] team, List<string> error=null) {
    if (team.Length<this.minLength) {
      if (error != null && this.minLength==1) error.Add(Game._INTL("Choose a Pokémon."));
      if (error != null && this.minLength>1) error.Add(Game._INTL("{1} Pokémon are needed.",this.minLength));
      return false;
    } else if (team.Length>this.maxLength) {
      if (error != null) error.Add(Game._INTL("No more than {1} Pokémon may enter.",this.maxLength));
      return false;
    }
    foreach (var pokemon in team) {
      if (!isPokemonValid(pokemon)) {
        if (pokemon.IsNotNullOrNone()) {
          if (error != null) error.Add(Game._INTL("This team is not allowed.", pokemon.Name));
        } else {
          if (error != null) error.Add(Game._INTL("{1} is not allowed.", pokemon.Name));
        }
        return false;
      }
    }
    foreach (var rule in @teamRules) {
      if (!rule.isValid(team)) {
        if (error != null) error.Add(rule.errorMessage);
        return false;
      }
    }
    foreach (var rule in @subsetRules) {
      if (!rule.isValid(team)) {
        if (error != null) error.Add(rule.errorMessage);
        return false;
      }
    }
    return true;
  }
}



public partial class BattleType : IBattleType {
  public virtual Combat.Battle pbCreateBattle(IPokeBattle_Scene scene,Combat.Trainer[] trainer1,Combat.Trainer[] trainer2) {
    return new Combat.Battle(scene,
       trainer1[0].party,trainer2[0].party,trainer1,trainer2);
  }
}



public partial class BattleTower : BattleType {
  public override Combat.Battle pbCreateBattle(IPokeBattle_Scene scene,Combat.Trainer[] trainer1,Combat.Trainer[] trainer2) {
    return new Combat.PokeBattle_RecordedBattle(scene,
       trainer1[0].party,trainer2[0].party,trainer1,trainer2);
  }
}



public partial class BattlePalace : BattleType {
  public override Combat.Battle pbCreateBattle(IPokeBattle_Scene scene,Combat.Trainer[] trainer1,Combat.Trainer[] trainer2) {
    return new Combat.PokeBattle_RecordedBattlePalace(scene,
       trainer1[0].party,trainer2[0].party,trainer1,trainer2);
  }
}



public partial class BattleArena : BattleType {
  public override Combat.Battle pbCreateBattle(IPokeBattle_Scene scene,Combat.Trainer[] trainer1,Combat.Trainer[] trainer2) {
    return new Combat.PokeBattle_RecordedBattleArena(scene,
       trainer1[0].party,trainer2[0].party,trainer1,trainer2);
  }
}



public abstract partial class BattleRule : IBattleRule {
            public const string SOULDEWCLAUSE       = "souldewclause";
            public const string SLEEPCLAUSE         = "sleepclause";
            public const string FREEZECLAUSE        = "freezeclause";
            public const string EVASIONCLAUSE       = "evasionclause";
            public const string OHKOCLAUSE          = "ohkoclause";
            public const string PERISHSONG          = "perishsong";
            public const string SELFKOCLAUSE        = "selfkoclause";
            public const string SELFDESTRUCTCLAUSE  = "selfdestructclause";
            public const string SONICBOOMCLAUSE     = "sonicboomclause";
            public const string MODIFIEDSLEEPCLAUSE = "modifiedsleepclause";
            public const string SKILLSWAPCLAUSE     = "skillswapclause";
  public virtual void setRule(Combat.Battle battle) { }
}



public partial class DoubleBattle : BattleRule {
  public override void setRule(Combat.Battle battle) {
    battle.doublebattle=battle.pbDoubleBattleAllowed();
  }
}



public partial class SingleBattle : BattleRule {
  public override void setRule(Combat.Battle battle) {
    battle.doublebattle=false;
  }
}



public partial class SoulDewBattleClause : BattleRule {
  public override void setRule(Combat.Battle battle) { battle.rules["souldewclause"]=true; }
}
  
  
  
public partial class SleepClause : BattleRule {
  public override void setRule(Combat.Battle battle) { battle.rules["sleepclause"]=true; }
}
  
  
  
public partial class FreezeClause : BattleRule {
  public override void setRule(Combat.Battle battle) { battle.rules["freezeclause"]=true; }
}
  
  
  
public partial class EvasionClause : BattleRule {
  public override void setRule(Combat.Battle battle) { battle.rules["evasionclause"]=true; }
}
  
  
  
public partial class OHKOClause : BattleRule {
  public override void setRule(Combat.Battle battle) { battle.rules["ohkoclause"]=true; }
}
  
  
  
public partial class PerishSongClause : BattleRule {
  public override void setRule(Combat.Battle battle) { battle.rules["perishsong"]=true; }
}



public partial class SelfKOClause : BattleRule {
  public override void setRule(Combat.Battle battle) { battle.rules["selfkoclause"]=true; }
}



public partial class SelfdestructClause : BattleRule {
  public override void setRule(Combat.Battle battle) { battle.rules["selfdestructclause"]=true; }
}



public partial class SonicBoomClause : BattleRule {
  public override void setRule(Combat.Battle battle) { battle.rules["sonicboomclause"]=true; }
}



public partial class ModifiedSleepClause : BattleRule {
  public override void setRule(Combat.Battle battle) { battle.rules["modifiedsleepclause"]=true; }
}



public partial class SkillSwapClause : BattleRule {
  public override void setRule(Combat.Battle battle) { battle.rules["skillswapclause"]=true; }
}



public partial class PokemonChallengeRules {
  public PokemonRuleSet ruleset				{ get; protected set; }
  public IBattleType battletype				{ get; protected set; }
  public LevelAdjustment levelAdjustment	{ get; protected set; }
            private List<IBattleRule> battlerules;

  public PokemonChallengeRules(PokemonRuleSet ruleset=null) {
    @ruleset=ruleset != null ? ruleset : new PokemonRuleSet();
    @battletype=new BattleTower();
    @levelAdjustment=null;
    @battlerules=new List<IBattleRule>();
  }

  public PokemonChallengeRules copy() {
    PokemonChallengeRules ret=new PokemonChallengeRules(@ruleset.copy());
    ret.setBattleType(@battletype);
    ret.setLevelAdjustment(@levelAdjustment);
    foreach (var rule in @battlerules) {
      ret.addBattleRule(rule);
    }
    return ret;
  }

  public int number { get {
    return this.ruleset.number;
  } }

  public PokemonChallengeRules setNumber(int number) {
    this.ruleset.setNumber(number);
    return this;
  }

  public PokemonChallengeRules setDoubleBattle(bool value) {
    if (value) {
      this.ruleset.setNumber(4);
      this.addBattleRule(new DoubleBattle());
    } else {
      this.ruleset.setNumber(3);
      this.addBattleRule(new SingleBattle());
    }
    return this;
  }

  public int?[][] adjustLevelsBilateral(Monster.Pokemon[] party1, Monster.Pokemon[] party2) {
    if (@levelAdjustment != null && @levelAdjustment.type==LevelAdjustment.BothTeams) {
      return @levelAdjustment.adjustLevels(party1,party2);
    } else {
      return null;
    }
  }

  public void unadjustLevelsBilateral(Monster.Pokemon[] party1, Monster.Pokemon[] party2, int?[][] adjusts) {
    if (@levelAdjustment != null && adjusts != null && @levelAdjustment.type==LevelAdjustment.BothTeams) {
      @levelAdjustment.unadjustLevels(party1,party2,adjusts);
    }
  }

  public int?[][] adjustLevels(Monster.Pokemon[] party1, Monster.Pokemon[] party2) {
    if (@levelAdjustment != null) {
      return @levelAdjustment.adjustLevels(party1,party2);
    } else {
      return null;
    }
  }

  public void unadjustLevels(Monster.Pokemon[] party1, Monster.Pokemon[] party2, int?[][] adjusts) {
    if (@levelAdjustment != null && adjusts != null) {
      @levelAdjustment.unadjustLevels(party1,party2,adjusts);
    }
  }

  public PokemonChallengeRules addPokemonRule(IBattleRestriction rule) {
    this.ruleset.addPokemonRule(rule);
    return this;
  }

  public PokemonChallengeRules addLevelRule(int minLevel,int maxLevel,int totalLevel) {
    this.addPokemonRule(new MinimumLevelRestriction(minLevel));
    this.addPokemonRule(new MaximumLevelRestriction(maxLevel));
    this.addSubsetRule(new TotalLevelRestriction(totalLevel));
    this.setLevelAdjustment(new TotalLevelAdjustment(minLevel,maxLevel,totalLevel));
    return this;
  }

  public PokemonChallengeRules addSubsetRule(IBattleTeamRestriction rule) {
    this.ruleset.addSubsetRule(rule);
    return this;
  }

  public PokemonChallengeRules addTeamRule(IBattleTeamRestriction rule) {
    this.ruleset.addTeamRule(rule);
    return this;
  }

  public PokemonChallengeRules addBattleRule(IBattleRule rule) {
    @battlerules.Add(rule);
    return this;
  }

  public Combat.Battle createBattle(IPokeBattle_Scene scene,Combat.Trainer[] trainer1,Combat.Trainer[] trainer2) {
    Combat.Battle battle=@battletype.pbCreateBattle(scene,trainer1,trainer2);
    foreach (var p in @battlerules) {
      p.setRule(battle);
    }
    return battle;
  }

  public PokemonChallengeRules setRuleset(PokemonRuleSet rule) {
    @ruleset=rule;
    return this;
  }

  public PokemonChallengeRules setBattleType(IBattleType rule) {
    @battletype=rule;
    return this;
  }

  public PokemonChallengeRules setLevelAdjustment(LevelAdjustment rule) {
    @levelAdjustment=rule;
    return this;
  }
}



#region Generation IV Cups
// ##########################################
//  Generation IV Cups
// ##########################################
public partial class StandardRules : PokemonRuleSet {
  //public int number				{ get; protected set; }

  public StandardRules(int number,int? level=null) : base(number) {
    //base.initialize(number);
    addPokemonRule(new StandardRestriction());
    addTeamRule(new SpeciesClause()); //addPokemonRule(new SpeciesClause());
    addTeamRule(new ItemClause()); //addPokemonRule(new ItemClause());
    if (level != null) {
      addPokemonRule(new MaximumLevelRestriction(level.Value));
    }
  }
}



public partial class StandardCup : StandardRules {
  public StandardCup() : base(3,50) {
    //base.initialize(3,50);
  }

  public string name { get {
    return Game._INTL("STANDARD Cup");
  } }
}



public partial class DoubleCup : StandardRules {
  public DoubleCup() : base(4,50) {
    //base.initialize(4,50);
  }

  public string name { get {
    return Game._INTL("DOUBLE Cup");
  } }
}



public partial class FancyCup : PokemonRuleSet {
  public FancyCup() : base(3) {
    //base.initialize(3);
    addPokemonRule(new StandardRestriction());
    addPokemonRule(new MaximumLevelRestriction(30));
    addSubsetRule(new TotalLevelRestriction(80));
    addPokemonRule(new HeightRestriction(2));
    addPokemonRule(new WeightRestriction(20));
    addPokemonRule(new BabyRestriction());
    addTeamRule(new SpeciesClause()); //addPokemonRule(new SpeciesClause());
    addTeamRule(new ItemClause()); //addPokemonRule(new ItemClause());
  }

  public string name { get {
    return Game._INTL("FANCY Cup");
  } }
}



public partial class LittleCup : PokemonRuleSet {
  public LittleCup() : base(3) {
    //base.initialize(3);
    addPokemonRule(new StandardRestriction());
    addPokemonRule(new MaximumLevelRestriction(5));
    addPokemonRule(new BabyRestriction());
    addTeamRule(new SpeciesClause()); //addPokemonRule(new SpeciesClause());
    addTeamRule(new ItemClause()); //addPokemonRule(new ItemClause());
  }

  public string name { get {
    return Game._INTL("LITTLE Cup");
  } }
}



public partial class LightCup : PokemonRuleSet {
  public LightCup() : base(3) {
    //base.initialize(3);
    addPokemonRule(new StandardRestriction());
    addPokemonRule(new MaximumLevelRestriction(50));
    addPokemonRule(new WeightRestriction(99));
    addPokemonRule(new BabyRestriction());
    addTeamRule(new SpeciesClause()); //addPokemonRule(new SpeciesClause());
    addTeamRule(new ItemClause()); //addPokemonRule(new ItemClause());
  }
  public string name { get {
    return Game._INTL("LIGHT Cup");
  } }
}
#endregion


        
#region Stadium Cups
// ##########################################
//  Stadium Cups
// ##########################################
public PokemonChallengeRules pbPikaCupRules(bool @double) {
  PokemonChallengeRules ret=new PokemonChallengeRules();
  ret.addPokemonRule(new StandardRestriction());
  ret.addLevelRule(15,20,50);
  ret.addTeamRule(new SpeciesClause());
  ret.addTeamRule(new ItemClause());
  ret.addBattleRule(new SleepClause());
  ret.addBattleRule(new FreezeClause());
  ret.addBattleRule(new SelfKOClause());
  ret.setDoubleBattle(@double).setNumber(3);
  return ret;
}

public PokemonChallengeRules pbPokeCupRules(bool @double) {
  PokemonChallengeRules ret=new PokemonChallengeRules();
  ret.addPokemonRule(new StandardRestriction());
  ret.addLevelRule(50,55,155);
  ret.addTeamRule(new SpeciesClause());
  ret.addTeamRule(new ItemClause());
  ret.addBattleRule(new SleepClause());
  ret.addBattleRule(new FreezeClause());
  ret.addBattleRule(new SelfdestructClause());
  ret.setDoubleBattle(@double).setNumber(3);
  return ret;
}

public PokemonChallengeRules pbPrimeCupRules(bool @double) {
  PokemonChallengeRules ret=new PokemonChallengeRules();
  ret.setLevelAdjustment(new OpenLevelAdjustment(Core.MAXIMUMLEVEL));
  ret.addTeamRule(new SpeciesClause());
  ret.addTeamRule(new ItemClause());
  ret.addBattleRule(new SleepClause());
  ret.addBattleRule(new FreezeClause());
  ret.addBattleRule(new SelfdestructClause());
  ret.setDoubleBattle(@double);
  return ret;
}

public PokemonChallengeRules pbFancyCupRules(bool @double) {
  PokemonChallengeRules ret=new PokemonChallengeRules();
  ret.addPokemonRule(new StandardRestriction());
  ret.addLevelRule(25,30,80);
  ret.addPokemonRule(new HeightRestriction(2));
  ret.addPokemonRule(new WeightRestriction(20));
  ret.addPokemonRule(new BabyRestriction());
  ret.addTeamRule(new SpeciesClause());
  ret.addTeamRule(new ItemClause());
  ret.addBattleRule(new SleepClause());
  ret.addBattleRule(new FreezeClause());
  ret.addBattleRule(new PerishSongClause());
  ret.addBattleRule(new SelfdestructClause());
  ret.setDoubleBattle(@double).setNumber(3);
  return ret;
}

public PokemonChallengeRules pbLittleCupRules(bool @double) {
  PokemonChallengeRules ret=new PokemonChallengeRules();
  ret.addPokemonRule(new StandardRestriction());
  ret.addPokemonRule(new UnevolvedFormRestriction());
  ret.setLevelAdjustment(new EnemyLevelAdjustment(5));
  ret.addPokemonRule(new MaximumLevelRestriction(5));
  ret.addTeamRule(new SpeciesClause());
  ret.addTeamRule(new ItemClause());
  ret.addBattleRule(new SleepClause());
  ret.addBattleRule(new FreezeClause());
  ret.addBattleRule(new SelfdestructClause());
  ret.addBattleRule(new PerishSongClause());
  ret.addBattleRule(new SonicBoomClause());
  ret.setDoubleBattle(@double);
  return ret;
}

public PokemonChallengeRules pbStrictLittleCupRules(bool @double) {
  PokemonChallengeRules ret=new PokemonChallengeRules();
  ret.addPokemonRule(new StandardRestriction());
  ret.addPokemonRule(new UnevolvedFormRestriction());
  ret.setLevelAdjustment(new EnemyLevelAdjustment(5));
  ret.addPokemonRule(new MaximumLevelRestriction(5));
  ret.addPokemonRule(new LittleCupRestriction());
  ret.addTeamRule(new SpeciesClause());
  ret.addBattleRule(new SleepClause());
  ret.addBattleRule(new EvasionClause());
  ret.addBattleRule(new OHKOClause());
  ret.addBattleRule(new SelfKOClause());
  ret.setDoubleBattle(@double).setNumber(3);
  return ret;
}
#endregion


        
#region Battle Frontier Rules
// ##########################################
//  Battle Frontier Rules
// ##########################################
public PokemonChallengeRules pbBattleTowerRules(bool @double,bool openlevel) {
  PokemonChallengeRules ret=new PokemonChallengeRules();
  if (openlevel) {
    ret.setLevelAdjustment(new OpenLevelAdjustment(60));
  } else {
    ret.setLevelAdjustment(new CappedLevelAdjustment(50));
  }
  ret.addPokemonRule(new StandardRestriction());
  ret.addTeamRule(new SpeciesClause());
  ret.addTeamRule(new ItemClause());
  ret.addBattleRule(new SoulDewBattleClause());
  ret.setDoubleBattle(@double);
  return ret;
}

public PokemonChallengeRules pbBattlePalaceRules(bool @double,bool openlevel) {
  return pbBattleTowerRules(@double,openlevel).setBattleType(new BattlePalace());
}

public PokemonChallengeRules pbBattleArenaRules(bool openlevel) {
  return pbBattleTowerRules(false,openlevel).setBattleType(new BattleArena());
}

public PokemonChallengeRules pbBattleFactoryRules(bool @double,bool openlevel) {
  PokemonChallengeRules ret=new PokemonChallengeRules();
  if (openlevel) {
    ret.setLevelAdjustment(new FixedLevelAdjustment(100));
    ret.addPokemonRule(new MaximumLevelRestriction(100));
  } else {
    ret.setLevelAdjustment(new FixedLevelAdjustment(50));
    ret.addPokemonRule(new MaximumLevelRestriction(50));
  }
  ret.addTeamRule(new SpeciesClause());
  ret.addPokemonRule(new BannedSpeciesRestriction(Pokemons.UNOWN));
  ret.addTeamRule(new ItemClause());
  ret.addBattleRule(new SoulDewBattleClause());
  ret.setDoubleBattle(@double).setNumber(0);
  return ret;
}
#endregion



#region Other Interesting Rulesets
/*/ ##########################################
// Other Interesting Rulesets
// ##########################################

// Official Species Restriction
new PokemonChallengeRules()
.addPokemonRule(new BannedSpeciesRestriction(
   Pokemons.MEWTWO,Pokemons.MEW,
   Pokemons.LUGIA,Pokemons.HOOH,Pokemons.CELEBI,
   Pokemons.KYOGRE,Pokemons.GROUDON,Pokemons.RAYQUAZA,Pokemons.JIRACHI,Pokemons.DEOXYS,
   Pokemons.DIALGA,Pokemons.PALKIA,Pokemons.GIRATINA,Pokemons.MANAPHY,Pokemons.PHIONE,
   Pokemons.DARKRAI,Pokemons.SHAYMIN,Pokemons.ARCEUS))
.addBattleRule(new SoulDewBattleClause());



// New Official Species Restriction
new PokemonChallengeRules()
.addPokemonRule(new BannedSpeciesRestriction(
   Pokemons.MEW,
   Pokemons.CELEBI,
   Pokemons.JIRACHI,Pokemons.DEOXYS,
   Pokemons.MANAPHY,Pokemons.PHIONE,Pokemons.DARKRAI,Pokemons.SHAYMIN,Pokemons.ARCEUS))
.addBattleRule(new SoulDewBattleClause());



// Pocket Monsters Stadium
new PokemonChallengeRules()
.addPokemonRule(new SpeciesRestriction(
   Pokemons.VENUSAUR,Pokemons.CHARIZARD,Pokemons.BLASTOISE,Pokemons.BEEDRILL,Pokemons.FEAROW,
   Pokemons.PIKACHU,Pokemons.NIDOQUEEN,Pokemons.NIDOKING,Pokemons.DUGTRIO,Pokemons.PRIMEAPE,
   Pokemons.ARCANINE,Pokemons.ALAKAZAM,Pokemons.MACHAMP,Pokemons.GOLEM,Pokemons.MAGNETON,
   Pokemons.CLOYSTER,Pokemons.GENGAR,Pokemons.ONIX,Pokemons.HYPNO,Pokemons.ELECTRODE,
   Pokemons.EXEGGUTOR,Pokemons.CHANSEY,Pokemons.KANGASKHAN,Pokemons.STARMIE,Pokemons.SCYTHER,
   Pokemons.JYNX,Pokemons.PINSIR,Pokemons.TAUROS,Pokemons.GYARADOS,Pokemons.LAPRAS,
   Pokemons.DITTO,Pokemons.VAPOREON,Pokemons.JOLTEON,Pokemons.FLAREON,Pokemons.AERODACTYL,
   Pokemons.SNORLAX,Pokemons.ARTICUNO,Pokemons.ZAPDOS,Pokemons.MOLTRES,Pokemons.DRAGONITE
));



// 1999 Tournament Rules
new PokemonChallengeRules()
.addTeamRule(new SpeciesClause())
.addPokemonRule(new ItemsDisallowedClause())
.addBattleRule(new SleepClause())
.addBattleRule(new FreezeClause())
.addBattleRule(new SelfdestructClause())
.setDoubleBattle(false)
.setLevelRule(1,50,150)
.addPokemonRule(new BannedSpeciesRestriction(
   Pokemons.VENUSAUR,Pokemons.DUGTRIO,Pokemons.ALAKAZAM,Pokemons.GOLEM,Pokemons.MAGNETON,
   Pokemons.GENGAR,Pokemons.HYPNO,Pokemons.ELECTRODE,Pokemons.EXEGGUTOR,Pokemons.CHANSEY,
   Pokemons.KANGASKHAN,Pokemons.STARMIE,Pokemons.JYNX,Pokemons.TAUROS,Pokemons.GYARADOS,
   Pokemons.LAPRAS,Pokemons.DITTO,Pokemons.VAPOREON,Pokemons.JOLTEON,Pokemons.SNORLAX,
   Pokemons.ARTICUNO,Pokemons.ZAPDOS,Pokemons.DRAGONITE,Pokemons.MEWTWO,Pokemons.MEW));

   
   
// 2005 Tournament Rules
new PokemonChallengeRules()
.addPokemonRule(new BannedSpeciesRestriction(
   Pokemons.DRAGONITE,Pokemons.MEW,Pokemons.MEWTWO,
   Pokemons.TYRANITAR,Pokemons.LUGIA,Pokemons.CELEBI,Pokemons.HOOH,Pokemons.GROUDON,Pokemons.KYOGRE,Pokemons.RAYQUAZA,
   Pokemons.JIRACHI,Pokemons.DEOXYS))
.setDoubleBattle(true)
.addLevelRule(1,50,200)
.addTeamRule(new ItemClause())
.addPokemonRule(new BannedItemRestriction(Items.SOULDEW,Items.ENIGMA_BERRY))
.addBattleRule(new SleepClause())
.addBattleRule(new FreezeClause())
.addBattleRule(new SelfdestructClause())
.addBattleRule(new PerishSongClause());



// 2008 Tournament Rules
new PokemonChallengeRules()
.addPokemonRule(new BannedSpeciesRestriction(
   Pokemons.MEWTWO,Pokemons.MEW,Pokemons.TYRANITAR,Pokemons.LUGIA,Pokemons.HOOH,Pokemons.CELEBI,
   Pokemons.GROUDON,Pokemons.KYOGRE,Pokemons.RAYQUAZA,Pokemons.JIRACHI,Pokemons.DEOXYS,
   Pokemons.PALKIA,Pokemons.DIALGA,Pokemons.PHIONE,Pokemons.MANAPHY,Pokemons.ROTOM,Pokemons.SHAYMIN,Pokemons.DARKRAI
.setDoubleBattle(true)
.addLevelRule(1,50,200)
.addTeamRule(new NicknameClause())
.addTeamRule(new ItemClause())
.addBattleRule(new SoulDewBattleClause());



// 2010 Tournament Rules
new PokemonChallengeRules()
.addPokemonRule(new BannedSpeciesRestriction(
   Pokemons.MEW,Pokemons.CELEBI,Pokemons.JIRACHI,Pokemons.DEOXYS,
   Pokemons.PHIONE,Pokemons.MANAPHY,Pokemons.SHAYMIN,Pokemons.DARKRAI,Pokemons.ARCEUS));
.addSubsetRule(new RestrictedSpeciesSubsetRestriction(
   Pokemons.MEWTWO,Pokemons.LUGIA,Pokemons.HOOH,
   Pokemons.GROUDON,Pokemons.KYOGRE,Pokemons.RAYQUAZA,
   Pokemons.PALKIA,Pokemons.DIALGA,Pokemons.GIRATINA))
.setDoubleBattle(true)
.addLevelRule(1,100,600)
.setLevelAdjustment(new CappedLevelAdjustment(50))
.addTeamRule(new NicknameClause())
.addTeamRule(new ItemClause())
.addPokemonRule(new SoulDewClause());



// Pokemon Colosseum -- Anything Goes
new PokemonChallengeRules()
.addLevelRule(1,100,600)
.addBattleRule(new SleepClause())
.addBattleRule(new FreezeClause())
.addBattleRule(new SelfdestructClause())
.addBattleRule(new PerishSongClause());



// Pokemon Colosseum -- Max Lv. 50
new PokemonChallengeRules()
.addLevelRule(1,50,300)
.addTeamRule(new SpeciesClause())
.addTeamRule(new ItemClause())
.addBattleRule(new SleepClause())
.addBattleRule(new FreezeClause())
.addBattleRule(new SelfdestructClause())
.addBattleRule(new PerishSongClause());



// Pokemon Colosseum -- Max Lv. 100
new PokemonChallengeRules()
.addLevelRule(1,100,600)
.addTeamRule(new SpeciesClause())
.addTeamRule(new ItemClause())
.addBattleRule(new SleepClause())
.addBattleRule(new FreezeClause())
.addBattleRule(new SelfdestructClause())
.addBattleRule(new PerishSongClause());



// Battle Time (includes animations)
If the time runs out, the team with the most Pokemon left wins. If both teams have
the same number of Pokémon left, total HP remaining breaks the tie. If both HP
totals are identical, the battle is a draw.

// Command Time
If the player is in the process of switching Pokémon when the time runs out, the
one that can still battle that's closest to the top of the roster is chosen.
Otherwise, the attack on top of the list is chosen.*/
#endregion

	public interface IBattleType
	{
		Combat.Battle pbCreateBattle(IPokeBattle_Scene scene, Combat.Trainer[] trainer1, Combat.Trainer[] trainer2);

    }
	public interface IBattleRule
	{
		void setRule(Combat.Battle battle);
	}
	public interface IBattleRestriction
	{
        //string errorMessage { get; }
        bool isValid(Monster.Pokemon pokemon);
	}
	public interface IBattleTeamRestriction
	{
        string errorMessage { get; }
        bool isValid(Monster.Pokemon[] team);
	}
}
}