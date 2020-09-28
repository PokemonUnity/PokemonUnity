using PokemonUnity.Inventory;
using PokemonUnity.Combat.Data;
using PokemonUnity.Character;
using PokemonUnity;
using System;
using System.Collections.Generic;
using System.Linq;
using PokemonUnity.Monster;

namespace PokemonUnity.Combat
{
public class PokeBattle_BattlePalace : Battle {
  public bool[] justswitched { get; private set; }
  public static readonly int[] BattlePalaceUsualTable=new int[] {
    61, 7,32,
    20,25,55,
    70,15,15,
    38,31,31,
    20,70,10,
    30,20,50,
    56,22,22,
    25,15,60,
    69, 6,25,
    35,10,55,
    62,10,28,
    58,37, 5,
    34,11,55,
    35, 5,60,
    56,22,22,
    35,45,20,
    44,50, 6,
    56,22,22,
    30,58,12,
    30,13,57,
    40,50,10,
    18,70,12,
    88, 6, 6,
    42,50, 8,
    56,22,22
  };
  public static readonly int[] BattlePalacePinchTable=new int[] {
    61, 7,32,
    84, 8, 8,
    32,60, 8,
    70,15,15,
    70,22, 8,
    32,58,10,
    56,22,22,
    75,15,10,
    28,55,17,
    29, 6,65,
    30,20,50,
    88, 6, 6,
    29,11,60,
    35,60, 5,
    56,22,22,
    34,60, 6,
    34, 6,60,
    56,22,22,
    30,58,12,
    27, 6,67,
    25,62,13,
    90, 5, 5,
    22,20,58,
    42, 5,53,
    56,22,22
  };
  public PokeBattle_BattlePalace(IPokeBattle_Scene scene, Monster.Pokemon[] p1, Monster.Pokemon[] p2, Combat.Trainer[] player, Combat.Trainer[] opponent) : base (scene, p1, p2, player, opponent) { 
  //public void initialize() {
    //base.this();
    @justswitched=new bool[] { false, false, false, false };
  }

  public int pbMoveCategory(Attack.Move move) {
    /*if (Game.MoveData[move.MoveId].Target==0x10 || move.Effect==0xD4) {		// Bide
      return 1;
    } else if (move.Power==0 || move.Effect==0x71 ||		// Counter
       move.Effect==0x72) { // Mirror Coat
      return 2;
    }
    else {
      return 0;
    }*/
    return 0;
  }

/// <summary>
/// Different implementation of pbCanChooseMove, ignores Imprison/Torment/Taunt/Disable/Encore
/// </summary>
/// <param name="idxPokemon"></param>
/// <param name="idxMove"></param>
/// <returns></returns>
  public bool pbCanChooseMovePartial (int idxPokemon,int idxMove) {
    Pokemon thispkmn=@battlers[idxPokemon];
    Attack.Move thismove=thispkmn.moves[idxMove];
    if (!thismove.IsNotNullOrNone()||thismove.MoveId==0) {
      return false;
    }
    if (thismove.PP<=0) {
      return false;
    }
    if (thispkmn.effects.ChoiceBand>=0 && 
       thismove.MoveId!=thispkmn.effects.ChoiceBand &&
       thispkmn.hasWorkingItem(Items.CHOICE_BAND)) {
      return false;
    }
    // though incorrect, just for convenience (actually checks Torment later)
    if (thispkmn.effects.Torment) {
      if (thismove.MoveId==thispkmn.lastMoveUsed) {
        return false;
      }
    }
    return true;
  }

  public void pbPinchChange(int idxPokemon) {
    Pokemon thispkmn=@battlers[idxPokemon];
    if (!thispkmn.effects.Pinch && thispkmn.Status!=Status.SLEEP && 
       thispkmn.HP<=(int)Math.Floor(thispkmn.TotalHP/2f)) {
      Natures nature=thispkmn.pokemon.Nature;
      thispkmn.effects.Pinch=true;
      if (nature==Natures.QUIET|| 
         nature==Natures.BASHFUL||
         nature==Natures.NAIVE||
         nature==Natures.QUIRKY||
         nature==Natures.HARDY||
         nature==Natures.DOCILE||
         nature==Natures.SERIOUS) {
        pbDisplay(Game._INTL("{1} is eager for more!",thispkmn.ToString()));
      }
      if (nature==Natures.CAREFUL||
         nature==Natures.RASH||
         nature==Natures.LAX||
         nature==Natures.SASSY||
         nature==Natures.MILD||
         nature==Natures.TIMID) {
        pbDisplay(Game._INTL("{1} began growling deeply!",thispkmn.ToString()));
      }
      if (nature==Natures.GENTLE||
         nature==Natures.ADAMANT||
         nature==Natures.HASTY||
         nature==Natures.LONELY||
         nature==Natures.RELAXED||
         nature==Natures.NAUGHTY) {
        pbDisplay(Game._INTL("A glint appears in {1}'s eyes!",thispkmn.ToString(true)));
      }
      if (nature==Natures.JOLLY||
         nature==Natures.BOLD||
         nature==Natures.BRAVE||
         nature==Natures.CALM||
         nature==Natures.IMPISH||
         nature==Natures.MODEST) {
        pbDisplay(Game._INTL("{1} is getting into position!",thispkmn.ToString()));
      }
    }
  }

  public override bool pbEnemyShouldWithdraw (int index) {
    bool shouldswitch=false;
    if (@battlers[index].effects.PerishSong==1) {
      shouldswitch=true;
    } else if (!CanChooseMove(index,0,false) &&
          !CanChooseMove(index,1,false) &&
          !CanChooseMove(index,2,false) &&
          !CanChooseMove(index,3,false) &&
          @battlers[index].turncount>=0 &&
          @battlers[index].turncount>5) {
      shouldswitch=true;
    }
    else {
      int hppercent=@battlers[index].HP*100/@battlers[index].TotalHP;
      int[] percents=new int[Core.MAXPARTYSIZE];
      int maxindex=-1;
      int maxpercent=0;
      int factor=0;
      Pokemon[] party=pbParty(index);
      for (int i = 0; i < party.Length; i++) {
        if (pbCanSwitch(index,i,false)) {
          percents[i]=party[i].HP*100/party[i].TotalHP;
          if (percents[i]>maxpercent) {
            maxindex=i;
            maxpercent=percents[i];
          }
        }
        else {
          percents[i]=0;
        }
      }
      if (hppercent<50) {
        factor=(maxpercent<hppercent) ? 20 : 40;
      }
      if (hppercent<25) {
        factor=(maxpercent<hppercent) ? 30 : 50;
      }
      if (@battlers[index].Status==Status.BURN ||
         @battlers[index].Status==Status.POISON) {
        factor+=10;
      }
      if (@battlers[index].Status==Status.PARALYSIS) {
        factor+=15;
      }
      if (@battlers[index].Status==Status.FROZEN ||
         @battlers[index].Status==Status.SLEEP) {
        factor+=20;
      }
      if (@justswitched[index]) {
        factor-=60;
        if (factor<0) factor=0;
      }
      shouldswitch=(Core.Rand.Next(100)<factor);
      if (shouldswitch && maxindex>=0) {
        pbRegisterSwitch(index,maxindex);
        return true;
      }
    }
    @justswitched[index]=shouldswitch;
    if (shouldswitch) {
      Pokemon[] party=pbParty(index);
      for (int i = 0; i < party.Length; i++) {
        if (pbCanSwitch(index,i,false)) {
          pbRegisterSwitch(index,i);
          return true;
        }
      }
    }
    return false;
  }

  public override bool pbRegisterMove(int idxPokemon,int idxMove,bool showMessages=true) {
    Pokemon thispkmn=@battlers[idxPokemon];
    if (idxMove==-2) {
      //@choices[idxPokemon][0]=1; // Move
      //@choices[idxPokemon][1]=-2; // "Incapable of using its power..."
      //@choices[idxPokemon][2]=@struggle;
      //@choices[idxPokemon][3]=-1;
      @choices[idxPokemon]=new Choice(ChoiceAction.UseMove,-2,@struggle,-1);
    }
    else {
      //@choices[idxPokemon][0]=1; // Move
      //@choices[idxPokemon][1]=idxMove; // Index of move
      //@choices[idxPokemon][2]=thispkmn.moves[idxMove]; // Move object
      //@choices[idxPokemon][3]=-1; // No target chosen
      @choices[idxPokemon]=new Choice(ChoiceAction.UseMove,idxMove,thispkmn.moves[idxMove],-1);
    }
    return true;
  }

  public override bool pbAutoFightMenu(int idxPokemon) {
    Pokemon thispkmn=@battlers[idxPokemon];
    Natures nature=thispkmn.pokemon.Nature;
    int randnum=Core.Rand.Next(100);
    int category=0;
    int atkpercent=0;
    int defpercent=0;
    if (!thispkmn.effects.Pinch) {
      atkpercent=BattlePalaceUsualTable[(int)nature*3];
      defpercent=atkpercent+BattlePalaceUsualTable[(int)nature*3+1];
    }
    else {
      atkpercent=BattlePalacePinchTable[(int)nature*3];
      defpercent=atkpercent+BattlePalacePinchTable[(int)nature*3+1];
    }
    if (randnum<atkpercent) {
      category=0;
    } else if (randnum<defpercent) {
      category=1;
    }
    else {
      category=2;
    }
    int[] moves=new int[4];
    for (int i = 0; i < thispkmn.moves.Length; i++) {
      if (!pbCanChooseMovePartial(idxPokemon,i)) continue;
      if (pbMoveCategory(thispkmn.moves[i])==category) {
        moves[moves.Length]=i;
      }
    }
    if (moves.Length==0) {
      // No moves of selected category
      pbRegisterMove(idxPokemon,-2);
    }
    else {
      int chosenmove=moves[Core.Rand.Next(moves.Length)];
      pbRegisterMove(idxPokemon,chosenmove);
    }
    return true;
  }

  public override void pbEndOfRoundPhase() {
    base.pbEndOfRoundPhase();
    if (@decision!=0) return;
    for (int i = 0; i < 4; i++) {
      if (!@battlers[i].isFainted()) {
        pbPinchChange(i);
      }
    }
  }
}
}