using PokemonUnity.Inventory;
using PokemonUnity.Combat.Data;
using PokemonUnity.Character;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PokemonUnity.Combat
{
public class PokeBattle_BattleArena : Battle{
  public bool battlerschanged { get; private set; }
  public int[] mind { get; private set; }
  public int[] skill { get; private set; }
  public int[] starthp { get; private set; }
  public int[] partyindexes { get; private set; }
  public int count { get; private set; }
  public PokeBattle_BattleArena(IPokeBattle_Scene scene, Monster.Pokemon[] p1, Monster.Pokemon[] p2, Trainer[] player, Trainer[] opponent) : base (scene, p1, p2, player, opponent) { 
  //public void initialize(*arg) {
    //base.this();
    @battlerschanged=true;
    @mind=new int[] { 0, 0 };
    @skill=new int[] { 0, 0 };
    @starthp=new int[] { 0, 0 };
    @count=0;
    @partyindexes=new int[] { 0, 0 };
  }

  public override bool pbDoubleBattleAllowed() {
    return false;
  }

  public override bool pbEnemyShouldWithdraw (int index) {
    return false;
  }

  public override bool pbCanSwitchLax (int idxPokemon,int pkmnidxTo,bool showMessages) {
    if (showMessages) {
      Pokemon thispkmn=@battlers[idxPokemon];
      pbDisplayPaused(_INTL("{1} can't be switched out!",thispkmn.ToString()));
    }
    return false;
  }

  public override void pbSwitch(bool favorDraws=false) {
    if (!favorDraws) {
      if (@decision>0) return;
    }
    else {
      if (@decision==BattleResults.DRAW) return;
    }
    pbJudge();
    if (@decision>0) return;
    List<int> switched=new List<int>();
    if (@battlers[0].isFainted() && @partyindexes[0]+1<this.pbParty(0).Length) {
      @partyindexes[0]+=1;
      int newpoke=@partyindexes[0];
      pbMessagesOnReplace(0,newpoke);
      pbReplace(0,newpoke);
      pbOnActiveOne(@battlers[0]);
      switched.Add(0);
    }
    if (@battlers[1].isFainted() && @partyindexes[1]+1<this.pbParty(1).Length) {
      @partyindexes[1]+=1  ;
      int newenemy=@partyindexes[1];
      pbMessagesOnReplace(1,newenemy);
      pbReplace(1,newenemy);
      pbOnActiveOne(@battlers[1]);
      switched.Add(1);
    }
    if (switched.Count>0) {
      Pokemon[] priority=pbPriority();
      foreach (var i in priority) {
        if (switched.Contains(i.Index)) i.pbAbilitiesOnSwitchIn(true);
      }
    }
  }

  public override void pbOnActiveAll() {
    @battlerschanged=true;
    @mind[0]=0;
    @mind[1]=0;
    @skill[0]=0;
    @skill[1]=0;
    @starthp[0]=battlers[0].HP;
    @starthp[1]=battlers[1].HP;
    @count=0;
    base.pbOnActiveAll(); //return
  }

  public override bool pbOnActiveOne(Pokemon pkmn, bool onlyabilities = false, bool moldbreaker = false) {
    @battlerschanged=true;
    @mind[0]=0;
    @mind[1]=0;
    @skill[0]=0;
    @skill[1]=0;
    @starthp[0]=battlers[0].HP;
    @starthp[1]=battlers[1].HP;
    @count=0;
    return base.pbOnActiveOne(pkmn, onlyabilities, moldbreaker);
  }

  public int pbMindScore(Move move) {
    if (move.Effect==0xAA ||		// Detect/Protect
       move.Effect==0xE8 || // Endure
       move.Effect==0x12) {    // Fake Out
      return -1;
    }
    if (move.Effect==0x71 ||		// Counter
       move.Effect==0x72 || // Mirror Coat
       move.Effect==0xD4) {    // Bide
      return 0;
    }
    if (move.BaseDamage==0) {
      return 0;
    }
    else {
      return 1;
    }
  }

  public override void pbCommandPhase() {
    if (@battlerschanged) {
      //@scene.pbBattleArenaBattlers(@battlers[0],@battlers[1]);
      @battlerschanged=false;
      @count=0;
    }
    base.pbCommandPhase();
    if (@decision!=0) return;
    // Update mind rating (asserting that a move was chosen)
    // TODO: Actually done at Pokémon's turn
    for (int i = 0; i < 2; i++) {
      if (@choices[i].Move.IsNotNullOrNone() && @choices[i].Action==ChoiceAction.UseMove) {//[0]==1
        @mind[i]+=pbMindScore(@choices[i].Move);
      }
    }
  }

  public override void pbEndOfRoundPhase() {
    base.pbEndOfRoundPhase();
    if (@decision!=0) return;
    @count+=1;
    // Update skill rating
    for (int i = 0; i < 2; i++) {
      @skill[i]+=this.successStates[i].Skill;
    }
    GameDebug.Log($"[Mind: #{@mind.ToString()}, Skill: #{@skill.ToString()}]");
    if (@count==3) {
      int[] points=new int[] { 0, 0 };
      @battlers[0].pbCancelMoves();
      @battlers[1].pbCancelMoves();
      int[] ratings1=new int[] {0,0,0};
      int[] ratings2=new int[] {0,0,0};
      if (@mind[0]==@mind[1]) {
        ratings1[0]=1;
        ratings2[0]=1;
      } else if (@mind[0]>@mind[1]) {
        ratings1[0]=2;
      }
      else {
        ratings2[0]=2;
      }
      if (@skill[0]==@skill[1]) {
        ratings1[1]=1;
        ratings2[1]=1;
      } else if (@skill[0]>@skill[1]) {
        ratings1[1]=2;
      }
      else {
        ratings2[1]=2;
      }
      int[] body=new int[] { 0, 0 };
      body[0]=(int)Math.Floor((@battlers[0].HP*100f)/(int)Math.Max(@starthp[0],1));
      body[1]=(int)Math.Floor((@battlers[1].HP*100f)/(int)Math.Max(@starthp[1],1));
      if (body[0]==body[1]) {
        ratings1[2]=1;
        ratings2[2]=1;
      } else if (body[0]>body[1]) {
        ratings1[2]=2;
      }
      else {
        ratings2[2]=2;
      }
      //@scene.pbBattleArenaJudgment(@battlers[0],@battlers[1],ratings1,ratings2);
      points=new int[] { 0, 0 };
      for (int i = 0; i < 3; i++) {
        points[0]+=ratings1[i];
        points[1]+=ratings2[i];
      }
      if (points[0]==points[1]) {
        pbDisplay(_INTL("{1} tied the opponent\n{2} in a referee's decision!",
           @battlers[0].Name,@battlers[1].Name)) ;
        @battlers[0].HP=0; // Note: Pokemon doesn't really lose HP, but the effect is mostly the same
        @battlers[0].pbFaint(false);
        @battlers[1].HP=0  ;
        @battlers[1].pbFaint(false);
      } else if (points[0]>points[1]) {
        pbDisplay(_INTL("{1} defeated the opponent\n{2} in a referee's decision!",
           @battlers[0].Name,@battlers[1].Name));
        @battlers[1].HP=0  ;
        @battlers[1].pbFaint(false);
      }
      else {
        pbDisplay(_INTL("{1} lost to the opponent\n{2} in a referee's decision!",
           @battlers[0].Name,@battlers[1].Name));
        @battlers[0].HP=0  ;
        @battlers[0].pbFaint(false);
      }
      pbGainEXP();
      pbSwitch();
    } 
  }
}

/*public class PokeBattle_Scene{
  public void updateJudgment(window,phase,battler1,battler2,ratings1,ratings2) {
    total1=0;
    total2=0;
    for (int i = 0; i < phase; i++) {
      total1+=ratings1[i];
      total2+=ratings2[i];
    }
    window.contents.clear;
    pbSetSystemFont(window.contents);
    textpos=[
       [battler1.Name,64,0,2,new Color(248,0,0),new Color(208,208,200)],
       [_INTL("VS"),144,0,2,new Color(72,72,72),new Color(208,208,200)],
       [battler2.Name,224,0,2,new Color(72,72,72),new Color(208,208,200)],
       [_INTL("Mind"),144,48,2,new Color(72,72,72),new Color(208,208,200)],
       [_INTL("Skill"),144,80,2,new Color(72,72,72),new Color(208,208,200)],
       [_INTL("Body"),144,112,2,new Color(72,72,72),new Color(208,208,200)],
       [string.Format("{0}",total1),64,160,2,new Color(72,72,72),new Color(208,208,200)],
       [_INTL("Judgment"),144,160,2,new Color(72,72,72),new Color(208,208,200)],
       [string.Format("{0}",total2),224,160,2,new Color(72,72,72),new Color(208,208,200)]
    ];
    pbDrawTextPositions(window.contents,textpos);
    images=[];
    for (int i = 0; i < phase; i++) {
      y=[48,80,112][i];
      x=(ratings1[i]==ratings2[i]) ? 64 : ((ratings1[i]>ratings2[i]) ? 0 : 32);
      images.Add(["Graphics/Pictures/judgment",64-16,y,x,0,32,32]);
      x=(ratings1[i]==ratings2[i]) ? 64 : ((ratings1[i]<ratings2[i]) ? 0 : 32);
      images.Add(["Graphics/Pictures/judgment",224-16,y,x,0,32,32]);
    }
    pbDrawImagePositions(window.contents,images);
    window.contents.fill_rect(16,150,256,4,new Color(80,80,80));
  }

  public void pbBattleArenaBattlers(battler1,battler2) {
    Game.pbMessage(_INTL("REFEREE: {1} VS {2}!\nCommence battling!\\wtnp[20]",
       battler1.Name,battler2.Name)) { pbUpdate }
  }

  public void pbBattleArenaJudgment(battler1,battler2,ratings1,ratings2) {
    msgwindow=null;
    dimmingvp=null;
    infowindow=null;
    try { //begin;
      msgwindow=Game.pbCreateMessageWindow;
      dimmingvp=new Viewport(0,0,Graphics.width,Graphics.height-msgwindow.height);
      Game.pbMessageDisplay(msgwindow,
         _INTL("REFEREE: That's it! We will now go to judging to determine the winner!\\wtnp[20]")) {
         pbUpdate; dimmingvp.update }
      dimmingvp.z=99999;
      infowindow=new SpriteWindow_Base(80,0,320,224);
      infowindow.contents=new Bitmap(;
         infowindow.width-infowindow.borderX,
         infowindow.height-infowindow.borderY);
      infowindow.z=99999;
      infowindow.visible=false;
      for (int i = 0; i < 10; i++) {
        pbGraphicsUpdate;
        pbInputUpdate;
        msgwindow.update;
        dimmingvp.update;
        dimmingvp.color=new Color(0,0,0,i*128/10);
      }
      updateJudgment(infowindow,0,battler1,battler2,ratings1,ratings2);
      infowindow.visible=true;
      for (int i = 0; i < 10; i++) {
        pbGraphicsUpdate;
        pbInputUpdate;
        msgwindow.update;
        dimmingvp.update;
        infowindow.update;
      }
      updateJudgment(infowindow,1,battler1,battler2,ratings1,ratings2);
      Game.pbMessageDisplay(msgwindow,
         _INTL("REFEREE: Judging category 1, Mind!\nThe Pokemon showing the most guts!\\wtnp[40]")) { 
         pbUpdate; dimmingvp.update; infowindow.update } 
      updateJudgment(infowindow,2,battler1,battler2,ratings1,ratings2);
      Game.pbMessageDisplay(msgwindow,
         _INTL("REFEREE: Judging category 2, Skill!\nThe Pokemon using moves the best!\\wtnp[40]")) { 
         pbUpdate; dimmingvp.update; infowindow.update } 
      updateJudgment(infowindow,3,battler1,battler2,ratings1,ratings2);
      Game.pbMessageDisplay(msgwindow,
         _INTL("REFEREE: Judging category 3, Body!\nThe Pokemon with the most vitality!\\wtnp[40]")) { 
         pbUpdate; dimmingvp.update; infowindow.update }
      total1=0;
      total2=0;
      for (int i = 0; i < 3; i++) {
        total1+=ratings1[i];
        total2+=ratings2[i];
      }
      if (total1==total2) {
        Game.pbMessageDisplay(msgwindow,
           _INTL("REFEREE: Judgment: {1} to {2}!\nWe have a draw!\\wtnp[40]",total1,total2)) { 
          pbUpdate; dimmingvp.update; infowindow.update }
      } else if (total1>total2) {
        Game.pbMessageDisplay(msgwindow,
           _INTL("REFEREE: Judgment: {1} to {2}!\nThe winner is {3}'s {4}!\\wtnp[40]",
           total1,total2,@battle.pbGetOwner(battler1.index).Name,battler1.Name)) { 
           pbUpdate; dimmingvp.update; infowindow.update }
      }
      else {
        Game.pbMessageDisplay(msgwindow,
           _INTL("REFEREE: Judgment: {1} to {2}!\nThe winner is {3}!\\wtnp[40]",
           total1,total2,battler2.Name)) { 
           pbUpdate; dimmingvp.update; infowindow.update }
      }
      infowindow.visible=false;
      msgwindow.visible=false;
      for (int i = 0; i < 10; i++) {
        pbGraphicsUpdate;
        pbInputUpdate;
        msgwindow.update;
        dimmingvp.update;
        dimmingvp.color=new Color(0,0,0,(10-i)*128/10);
      }
    } finally { //ensure;
      Game.pbDisposeMessageWindow(msgwindow);
      dimmingvp.dispose;
      infowindow.contents.dispose;
      infowindow.dispose;
    }
  }
}*/
}