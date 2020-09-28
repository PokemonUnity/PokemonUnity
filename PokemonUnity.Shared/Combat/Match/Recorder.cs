using PokemonUnity.Inventory;
using PokemonUnity.Combat.Data;
using PokemonUnity.Character;
using PokemonUnity.Saving;
using PokemonUnity.Saving.SerializableClasses;
using PokemonUnity.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PokemonUnity.Combat
{ 
public static class Commands{
    public const int Fight   = 0;
    public const int Bag     = 1;
    public const int Pokemon = 2;
    public const int Run     = 3;
  }

public class PokeBattle_RecordedBattleModule<TBattle> : Battle 
        where TBattle : Battle {
  public List<int> randomnums { get; protected set; }
  public List<int[][]> rounds { get; protected set; }
  public List<int> switches { get; protected set; }
  public int roundindex { get; protected set; }
  public object properties { get; protected set; }
  public int battletype { get; protected set; }
  public PokeBattle_RecordedBattleModule(IPokeBattle_Scene scene,Monster.Pokemon[] p1,Monster.Pokemon[] p2,Combat.Trainer[] player,Combat.Trainer[] opponent) : base (scene, p1, p2, player, opponent)
  //public void initialize(IPokeBattle_Scene scene,Monster.Pokemon[] p1,Monster.Pokemon[] p2,Trainer[] player,Trainer[] opponent) 
  {
    //@randomnumbers=new List<int>();
    @randomnums=new List<int>();
    @rounds=new List<int[][]>();
    @switches=new List<int>();
    @roundindex=-1;
    @properties=new object();
    //base.initialize(scene, p1, p2, player, opponent);
  }

  public virtual int pbGetBattleType() {
    return 0; // Battle Tower
  }

  public Combat.Trainer[] pbGetTrainerInfo(Combat.Trainer[] trainer) {
    if (trainer == null) return null;
    if (trainer.Length > 0) {
      return trainer;
      //return new Trainer[] {
      //   //new Trainer(trainer[0].trainertype,trainer[0].Name,trainer[0].id,trainer[0].badges),
      //   new Trainer(trainer[0].ID,trainer[0].Name,trainer[0].TrainerID,trainer[0].Badges),
      //   new Trainer(trainer[1].ID,trainer[1].Name,trainer[1].TrainerID,trainer[0].Badges)
      //};
    }
    else {
      return trainer;
      //return new Trainer[] {
      //   new Trainer(trainer.trainertype,trainer.Name,trainer.id,trainer.badges)
      //};
    }
  }

  public override void pbStartBattle(bool canlose=false) {
    @properties=new object();
    /*@properties["internalbattle"]=@internalbattle;
    @properties["player"]=pbGetTrainerInfo(@player);
    @properties["opponent"]=pbGetTrainerInfo(@opponent);
    @properties["party1"]=@party1.Serialize();//Marshal.dump(@party1);
    @properties["party2"]=@party2.Serialize();//Marshal.dump(@party2);
    @properties["endspeech"]=@endspeech != null ? @endspeech : "";
    @properties["endspeech2"]=@endspeech2 != null ? @endspeech2 : "";
    @properties["endspeechwin"]=@endspeechwin != null ? @endspeechwin : "";
    @properties["endspeechwin2"]=@endspeechwin2 != null ? @endspeechwin2 : "";
    @properties["doublebattle"]=@doublebattle;
    @properties["weather"]=@weather;
    @properties["weatherduration"]=@weatherduration;
    @properties["cantescape"]=@cantescape;
    @properties["shiftStyle"]=@shiftStyle;
    @properties["battlescene"]=@battlescene;
    @properties["items"]=Marshal.dump(@items);
    @properties["environment"]=@environment;
    @properties["rules"]=Marshal.dump(@rules);*/
    base.pbStartBattle(canlose);
  }

  public string pbDumpRecord() {
    //return Marshal.dump([pbGetBattleType,@properties,@rounds,@randomnumbers,@switches]);
    return null;
  }

  public override int pbSwitchInBetween(int i1,bool i2,bool i3) {
    int ret=base.pbSwitchInBetween(i1,i2,i3);
    @switches.Add(ret);
    return ret;
  }

  public override bool pbRegisterMove(int i1,int i2, bool showMessages=true) {
    if (base.pbRegisterMove(i1,i2,showMessages)) {
      @rounds[@roundindex][i1]=new int[] { Commands.Fight, i2 };
      return true;
    }
    return false;
  }

  public override int pbRun(int i1,bool duringBattle=false) {
    int ret=base.pbRun(i1,duringBattle);
    @rounds[@roundindex][i1]=new int[] { Commands.Run, (int)@decision };
    return ret;
  }

  public override bool pbRegisterTarget(int i1,int i2) {
    bool ret=base.pbRegisterTarget(i1,i2);
    @rounds[@roundindex][i1][2]=i2;
    return ret;
  }

  public override void pbAutoChooseMove(int i1,bool showMessages=true) {
    //bool ret= //no return value...
        base.pbAutoChooseMove(i1,showMessages);
    @rounds[@roundindex][i1]=new int[] { Commands.Fight, -1 };
    return; //ret;
  }

  public override bool pbRegisterSwitch(int i1,int i2) {
    if (base.pbRegisterSwitch(i1,i2)) {
      @rounds[@roundindex][i1]=new int[] { Commands.Pokemon, i2 };
      return true;
    }
    return false;
  }

  public bool pbRegisterItem(int i1,Items i2) {
    if (base.pbRegisterItem(i1,i2)) {
      @rounds[@roundindex][i1]=new int[] { Commands.Bag, (int)i2 }; //Commands.Item == Bag
      return true;
    }
    return false;
  }

  public override void pbCommandPhase() {
    //@roundindex+=1;
    //@rounds[@roundindex]=new int[4][]; //[[],[],[],[]];
    @rounds.Add(new int[4][]); //[[],[],[],[]];
    base.pbCommandPhase();
  }

  public void pbStorePokemon(Pokemon pkmn) {
  }

  public override int pbRandom(int num) {
    int ret=base.pbRandom(num);
    //@randomnumbers.Add(ret);
    @randomnums.Add(ret);
    return ret;
  }
}

/*public static class BattlePlayerHelper{
  public static Trainer[] pbGetOpponent(Battle battle) {
    //return this.pbCreateTrainerInfo(battle[1]["opponent"]);
    return pbCreateTrainerInfo(battle.opponent);
  }

  public void pbGetBattleBGM(Battle battle) {
    return pbGetTrainerBattleBGM(this.pbGetOpponent(battle));
  }

  public void pbCreateTrainerInfo(trainer) {
    if (!trainer) return null;
    if (trainer.Length>1) {
      ret=[];
      ret[0]=new PokeBattle_Trainer(trainer[0][1],trainer[0][0]);
      ret[0].id=trainer[0][2];
      ret[0].badges=trainer[0][3];
      ret[1]=new PokeBattle_Trainer(trainer[1][1],trainer[1][0]);
      ret[1].id=trainer[1][2];
      ret[1].badges=trainer[1][3];
      return ret;
    }
    else {
      ret=new PokeBattle_Trainer(trainer[0][1],trainer[0][0]);
      ret.id=trainer[0][2];
      ret.badges=trainer[0][3];
      return ret;
    }
  }
}*/

/// <summary>
/// Playback?
/// </summary>
/// <typeparam name="TBattle"></typeparam>
public class PokeBattle_BattlePlayerModule<TBattle> : PokeBattle_RecordedBattleModule<TBattle> 
        where TBattle : PokeBattle_RecordedBattleModule<Battle> {
        //where TBattle : Battle {
  public int randomindex { get; protected set; }
  public int switchindex { get; protected set; }
  //public PokeBattle_BattlePlayerModule(IPokeBattle_Scene scene, TBattle battle) : base (scene, battle.party1, battle.party2, battle.player, battle.opponent)
  public PokeBattle_BattlePlayerModule(IPokeBattle_Scene scene, TBattle battle) : base (scene, new Monster.Pokemon[0], new Monster.Pokemon[0], battle.player, battle.opponent)
  //public void initialize(scene,Battle battle)
  {
    @battletype=battle.battletype;
    @properties=battle.properties;
    @rounds=battle.rounds;
    @randomnums=battle.randomnums;
    @switches=battle.switches;
    @roundindex=-1;
    @randomindex=0;
    @switchindex=0;
    //base.initialize(scene,
    //   Marshal.restore(new StringInput(@properties["party1"])),
    //   Marshal.restore(new StringInput(@properties["party2"])),
    //   BattlePlayerHelper.pbCreateTrainerInfo(@properties["player"]),
    //   BattlePlayerHelper.pbCreateTrainerInfo(@properties["opponent"])
    //);
  }

  public override void pbStartBattle(bool canlose=false) {
    /*@internalbattle=@properties["internalbattle"];
    @endspeech=@properties["endspeech"].ToString();
    @endspeech2=@properties["endspeech2"].ToString();
    @endspeechwin=@properties["endspeechwin"].ToString();
    @endspeechwin2=@properties["endspeechwin2"].ToString();
    @doublebattle=(bool)@properties["doublebattle"];
    @weather=@properties["weather"];
    @weatherduration=@properties["weatherduration"];
    @cantescape=(bool)@properties["cantescape"];
    @shiftStyle=(bool)@properties["shiftStyle"];
    @battlescene=@properties["battlescene"];
    @items=Marshal.restore(new StringInput(@properties["items"]));
    @rules=Marshal.restore(new StringInput(@properties["rules"]));
    @environment=@properties["environment"];*/
    base.pbStartBattle(canlose);
  }

  public int pbSwitchInBetween(int i1,int i2,bool i3) {
    int ret=@switches[@switchindex];
    @switchindex+=1;
    return ret;
  }

  public override int pbRandom(int num) {
    int ret=@randomnums[@randomindex];
    @randomindex+=1;
    return ret;
  }

  public override void pbDisplayPaused(string str) {
    pbDisplay(str);
  }

  public void pbCommandPhaseCore() {
    @roundindex+=1;
    for (int i = 0; i < 4; i++) {
      if (@rounds[@roundindex][i].Length==0) continue;
      //@choices[i][0]=0;
      //@choices[i][1]=0;
      //@choices[i][2]=null;
      //@choices[i][3]=-1;
      @choices[i]=new Choice();
      switch (@rounds[@roundindex][i][0]) {
      case Commands.Fight:
        if (@rounds[@roundindex][i][1]==-1) {
          pbAutoChooseMove(i,false);
        }
        else {
          pbRegisterMove(i,@rounds[@roundindex][i][1]);
        }
        //if (@rounds[@roundindex][i][2]!=null)
          pbRegisterTarget(i,@rounds[@roundindex][i][2]);
        break;
      case Commands.Pokemon:
        pbRegisterSwitch(i,@rounds[@roundindex][i][1]);
        break;
      case Commands.Bag:
        pbRegisterItem(i,(Items)@rounds[@roundindex][i][1]);
        break;
      case Commands.Run:
        @decision=(BattleResults)@rounds[@roundindex][i][1];
        break;
      }
    }
  }
}

public class PokeBattle_RecordedBattle : PokeBattle_RecordedBattleModule<Battle> { 
  //include PokeBattle_RecordedBattleModule;
  public PokeBattle_RecordedBattle(IPokeBattle_Scene scene, Monster.Pokemon[] p1, Monster.Pokemon[] p2, Combat.Trainer[] player, Combat.Trainer[] opponent) : base(scene, p1, p2, player, opponent)
  {
  }
  public override int pbGetBattleType() {
    return 0;
  }
}

public class PokeBattle_RecordedBattlePalace : PokeBattle_RecordedBattleModule<PokeBattle_BattlePalace>{
  //include PokeBattle_RecordedBattleModule;
  public PokeBattle_RecordedBattlePalace(IPokeBattle_Scene scene, Monster.Pokemon[] p1, Monster.Pokemon[] p2, Combat.Trainer[] player, Combat.Trainer[] opponent) : base(scene, p1, p2, player, opponent)
  {
  }
  public override int pbGetBattleType() {
    return 1;
  }
}

public class PokeBattle_RecordedBattleArena : PokeBattle_RecordedBattleModule<PokeBattle_BattleArena>{
  //include PokeBattle_RecordedBattleModule;
  public PokeBattle_RecordedBattleArena(IPokeBattle_Scene scene, Monster.Pokemon[] p1, Monster.Pokemon[] p2, Combat.Trainer[] player, Combat.Trainer[] opponent) : base(scene, p1, p2, player, opponent)
  {
  }
  public override int pbGetBattleType() {
    return 2;
  }
}

public class PokeBattle_BattlePlayer : PokeBattle_BattlePlayerModule<PokeBattle_RecordedBattleModule<Battle>>{
  //include PokeBattle_BattlePlayerModule;
  public PokeBattle_BattlePlayer(IPokeBattle_Scene scene, PokeBattle_RecordedBattleModule<Battle> battle) : base(scene, battle)
  {
  }
}

/*public class PokeBattle_BattlePalacePlayer : PokeBattle_BattlePlayerModule<PokeBattle_RecordedBattleModule<PokeBattle_BattlePalace>>{
  //include PokeBattle_BattlePlayerModule;
  public PokeBattle_BattlePalacePlayer(IPokeBattle_Scene scene, PokeBattle_RecordedBattleModule<PokeBattle_BattlePalace> battle) : base(scene, battle)
  {
  }
}

public class PokeBattle_BattleArenaPlayer : PokeBattle_BattlePlayerModule<PokeBattle_RecordedBattleModule<PokeBattle_BattleArena>>{
  //include PokeBattle_BattlePlayerModule;
  public PokeBattle_BattleArenaPlayer(IPokeBattle_Scene scene, PokeBattle_RecordedBattleModule<PokeBattle_BattleArena> battle) : base(scene, battle)
  {
  }
}*/
}