using System;
using System.Linq;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Combat.Data;
using PokemonUnity.Character;
using PokemonUnity.Utility;

namespace PokemonUnity.Combat
{
// AI skill levels:
//           0:     Wild Pokémon
//           1-31:  Basic trainer (young/inexperienced)
//           32-47: Some skill
//           48-99: High skill
//           100+:  Gym Leaders, E4, Champion, highest level
public static class PBTrainerAI {
  // Minimum skill level to be in each AI category
  public const int minimumSkill  = 1;
  public const int mediumSkill   = 32;
  public const int highSkill     = 48;
  public const int bestSkill     = 100;   // Gym Leaders, E4, Champion
}

public partial class Battle{
/// <summary>
/// Get a score for each move being considered (trainer-owned Pokémon only).
/// Moves with higher scores are more likely to be chosen.
/// </summary>
/// <param name="move"></param>
/// <param name="attacker"></param>
/// <param name="opponent"></param>
/// <param name="skill"></param>
/// <returns></returns>
  public int pbGetMoveScore(Attack.Move move,Pokemon attacker,Pokemon opponent,int skill=100) {
    if (skill<PBTrainerAI.minimumSkill) skill=PBTrainerAI.minimumSkill;
    float score=100; if (move.Type == Types.SHADOW) score += 20; // Shadow moves are more preferable
    if (!opponent.IsNotNullOrNone()) opponent=attacker.pbOppositeOpposing;
    if (opponent.IsNotNullOrNone() && opponent.isFainted()) opponent=opponent.Partner;
    #region switch variables
    bool hasspecialattack = false;
    bool hasphysicalattack = false;
    bool hasdamagingattack = false;
    bool hasDamagingMove = false;
    bool foundmove = false;
    bool canattract = false;
    bool? agender, ogender;
    int avg = 0; int count = 0;
    int spatk = 0; int attack = 0;
    int aspeed = 0; int ospeed = 0;
    int aatk = 0; int oatk = 0;
    int aspa = 0; int ospa = 0;
    int aspatk = 0; int ospatk = 0;
    int adef = 0; int odef = 0;
    int aspdef = 0; int ospdef = 0;
    int aspd = 0; int ospd = 0;
    int astages = 0; int ostages = 0;
    Attack.Data.MoveData moveData;
    Attack.Data.Effects[] blacklist=new Attack.Data.Effects[0];
    #endregion
#region Alter score depending on the move's function code #######################
    switch (move.Effect) {
    case 0x00: // No extra effect
      break;
    case Attack.Data.Effects.x056:
      score-=95;
      if (skill>=PBTrainerAI.highSkill) {
        score=0;
      }
      break;
    case Attack.Data.Effects.x0FF: // Struggle
      break;
    case Attack.Data.Effects.x002:
      if (opponent.pbCanSleep(attacker,false)) {
        score+=30;
        if (skill>=PBTrainerAI.mediumSkill) {
          if (opponent.effects.Yawn>0) score-=30;
        }
        if (skill>=PBTrainerAI.highSkill) {
          if (opponent.hasWorkingAbility(Abilities.MARVEL_SCALE)) score-=30;
        }
        if (skill>=PBTrainerAI.bestSkill) {
          foreach (var i in opponent.moves) {
            Attack.Data.MoveData movedata=Game.MoveData[i.MoveId];
            if (movedata.Effect==Attack.Data.Effects.x062 ||		// Sleep Talk
               movedata.Effect==Attack.Data.Effects.x05D) {    // Snore
              score-=50;
              break;
            }
          }
        }
      }
      else {
        if (skill>=PBTrainerAI.mediumSkill) {
          if (move.Power==0) score-=90;
        }
      }
      break;
    case Attack.Data.Effects.x0BC:
      if (opponent.effects.Yawn>0 || !opponent.pbCanSleep(attacker,false)) {
        if (skill>=PBTrainerAI.mediumSkill) {
          score-=90;
        }
      }
      else {
        score+=30;
        if (skill>=PBTrainerAI.highSkill) {
          if (opponent.hasWorkingAbility(Abilities.MARVEL_SCALE)) score-=30;
        }
        if (skill>=PBTrainerAI.bestSkill) {
          foreach (var i in opponent.moves) {
            Attack.Data.MoveData movedata=Game.MoveData[i.MoveId];
            if (movedata.Effect==Attack.Data.Effects.x062 ||		// Sleep Talk
               movedata.Effect==Attack.Data.Effects.x05D) {    // Snore
              score-=50;
              break;
            }
          }
        }
      }
      break;
    case Attack.Data.Effects.x003: case Attack.Data.Effects.x022: case Attack.Data.Effects.x04E:
      if (opponent.pbCanPoison(attacker,false)) {
        score+=30;
        if (skill>=PBTrainerAI.mediumSkill) {
          if (opponent.HP<=opponent.TotalHP/4) score+=30;
          if (opponent.HP<=opponent.TotalHP/8) score+=50;
          if (opponent.effects.Yawn>0) score-=40;
        }
        if (skill>=PBTrainerAI.highSkill) {
          if (pbRoughStat(opponent,Stats.DEFENSE,skill)>100) score+=10;
          if (pbRoughStat(opponent,Stats.SPDEF,skill)>100) score+=10;
          if (opponent.hasWorkingAbility(Abilities.GUTS)) score-=40;
          if (opponent.hasWorkingAbility(Abilities.MARVEL_SCALE)) score-=40;
          if (opponent.hasWorkingAbility(Abilities.TOXIC_BOOST)) score-=40;
        }
      }
      else {
        if (skill>=PBTrainerAI.mediumSkill) {
          if (move.Power==0) score-=90;
        }
      }
      break;
    case Attack.Data.Effects.x007: case Attack.Data.Effects.x099: case Attack.Data.Effects.x114: case Attack.Data.Effects.x14C:
      if (opponent.pbCanParalyze(attacker,false) &&
         !(skill>=PBTrainerAI.mediumSkill &&
         move.MoveId == Moves.THUNDER_WAVE &&
         pbTypeModifier(move.Type,attacker,opponent)==0)) {
        score+=30;
        if (skill>=PBTrainerAI.mediumSkill) {
           aspeed=pbRoughStat(attacker,Stats.SPEED,skill);
           ospeed=pbRoughStat(opponent,Stats.SPEED,skill);
          if (aspeed<ospeed) {
            score+=30;
          } else if (aspeed>ospeed) {
            score-=40;
          }
        }
        if (skill>=PBTrainerAI.highSkill) {
          if (opponent.hasWorkingAbility(Abilities.GUTS)) score-=40;
          if (opponent.hasWorkingAbility(Abilities.MARVEL_SCALE)) score-=40;
          if (opponent.hasWorkingAbility(Abilities.QUICK_FEET)) score-=40;
        }
      }
      else {
        if (skill>=PBTrainerAI.mediumSkill) {
          if (move.Power==0) score-=90;
        }
      }
      break;
    case Attack.Data.Effects.x005: case Attack.Data.Effects.x112: case Attack.Data.Effects.x14D:
      if (opponent.pbCanBurn(attacker,false)) {
        score+=30;
        if (skill>=PBTrainerAI.highSkill) {
          if (opponent.hasWorkingAbility(Abilities.GUTS)) score-=40;
          if (opponent.hasWorkingAbility(Abilities.MARVEL_SCALE)) score-=40;
          if (opponent.hasWorkingAbility(Abilities.QUICK_FEET)) score-=40;
          if (opponent.hasWorkingAbility(Abilities.FLARE_BOOST)) score-=40;
        }
      }
      else {
        if (skill>=PBTrainerAI.mediumSkill) {
          if (move.Power==0) score-=90;
        }
      }
      break;
    case Attack.Data.Effects.x006: case Attack.Data.Effects.x105: case Attack.Data.Effects.x113:
      if (opponent.pbCanFreeze(attacker,false)) {
        score+=30;
        if (skill>=PBTrainerAI.highSkill) {
          if (opponent.hasWorkingAbility(Abilities.MARVEL_SCALE)) score-=20;
        }
      }
      else {
        if (skill>=PBTrainerAI.mediumSkill) {
          if (move.Power==0) score-=90;
        }
      }
      break;
    case Attack.Data.Effects.x020:
      score+=30;
      if (skill>=PBTrainerAI.highSkill) {
        if (!opponent.hasWorkingAbility(Abilities.INNER_FOCUS) &&
                     opponent.effects.Substitute==0) score+=30;
      }
      break;
    case Attack.Data.Effects.x097:
      if (skill>=PBTrainerAI.highSkill) {
        if (!opponent.hasWorkingAbility(Abilities.INNER_FOCUS) &&
                     opponent.effects.Substitute==0) score+=30;
      }
      if (opponent.effects.Minimize) score+=30;
      break;
    case Attack.Data.Effects.x05D:
      if (attacker.Status==Status.SLEEP) {
        score+=100; // Because it can be used while asleep
        if (skill>=PBTrainerAI.highSkill) {
          if (!opponent.hasWorkingAbility(Abilities.INNER_FOCUS) &&
                       opponent.effects.Substitute==0) score+=30;
        }
      }
      else {
        score-=90; // Because it will fail here
        if (skill>=PBTrainerAI.bestSkill) {
          score=0;
        }
      }
      break;
    case Attack.Data.Effects.x09F:
      if (attacker.turncount==0) {
        if (skill>=PBTrainerAI.highSkill) {
          if (!opponent.hasWorkingAbility(Abilities.INNER_FOCUS) &&
                       opponent.effects.Substitute==0) score+=30;
        }
      }
      else {
        score-=90; // Because it will fail here
        if (skill>=PBTrainerAI.bestSkill) {
          score=0;
        }
      }
      break;
    case Attack.Data.Effects.x04D: case Attack.Data.Effects.x10C: case Attack.Data.Effects.x14E:
      if (opponent.pbCanConfuse(attacker,false)) {
        score+=30;
      }
      else {
        if (skill>=PBTrainerAI.mediumSkill) {
          if (move.Power==0) score-=90;
        }
      }
      break;
    case Attack.Data.Effects.x079:
      canattract=true;
      agender=attacker.Gender;
      ogender=opponent.Gender;
      if (agender==null || ogender==null || agender==ogender) {
        score-=90; canattract=false;
      } else if (opponent.effects.Attract>=0) {
        score-=80; canattract=false;
      } else if (skill>=PBTrainerAI.bestSkill &&
         opponent.hasWorkingAbility(Abilities.OBLIVIOUS)) {
        score-=80; canattract=false;
      }
      if (skill>=PBTrainerAI.highSkill) {
        if (canattract && opponent.hasWorkingItem(Items.DESTINY_KNOT) &&
           attacker.pbCanAttract(opponent,false)) {
          score-=30;
        }
      }
      break;
    case Attack.Data.Effects.x025:
      if (opponent.Status==0) score+=30;
      break;
    case Attack.Data.Effects.x0C2:
      if (attacker.Status==Status.BURN) {
        score+=40;
      } else if (attacker.Status==Status.POISON) {
        score+=40;
        if (skill>=PBTrainerAI.mediumSkill) {
          if (attacker.HP<attacker.TotalHP/8) {
            score+=60;
          } else if (skill>=PBTrainerAI.highSkill &&
             attacker.HP<(attacker.effects.Toxic+1)*attacker.TotalHP/16) {
            score+=60;
          }
        }
      } else if (attacker.Status==Status.PARALYSIS) {
        score+=40;
      }
      else {
        score-=90;
      }
      break;
    case Attack.Data.Effects.x067:
      Monster.Pokemon[] party=pbParty(attacker.Index);
      int statuses=0;
      for (int i = 0; i < party.Length; i++) {
        if (party[i].IsNotNullOrNone() && party[i].Status!=0) statuses+=1;
      }
      if (statuses==0) {
        score-=80;
      }
      else {
        score+=20*statuses;
      }
      break;
    case Attack.Data.Effects.x07D:
      if (attacker.OwnSide.Safeguard>0) {
        score-=80 ;
      } else if (attacker.Status!=0) {
        score-=40;
      }
      else {
        score+=30;
      }
      break;
    case Attack.Data.Effects.x0EB:
      if (attacker.Status==0) {
        score-=90;
      }
      else {
        score+=40;
      }
      break;
    case Attack.Data.Effects.x00B:
      if (move.Power==0) {
        if (attacker.pbTooHigh(Stats.ATTACK)) {
          score-=90;
        }
        else {
          score-=attacker.stages[(int)Stats.ATTACK]*20;
          if (skill>=PBTrainerAI.mediumSkill) {
            hasphysicalattack=false;
            foreach (var thismove in attacker.moves) {
              if (thismove.MoveId!=0 && thismove.Power>0 &&
                 thismove.pbIsPhysical(thismove.Type)) {
                hasphysicalattack=true;
              }
            }
            if (hasphysicalattack) {
              score+=20;
            } else if (skill>=PBTrainerAI.highSkill) {
              score-=90;
            }
          }
        }
      }
      else {
        if (attacker.stages[(int)Stats.ATTACK]<0) score+=20;
        if (skill>=PBTrainerAI.mediumSkill) {
          hasphysicalattack=false;
          foreach (var thismove in attacker.moves) {
            if (thismove.MoveId!=0 && thismove.Power>0 &&
               thismove.pbIsPhysical(thismove.Type)) {
              hasphysicalattack=true;
            }
          }
          if (hasphysicalattack) {
            score+=20;
          }
        }
      }
      break;
    case Attack.Data.Effects.x00C: case Attack.Data.Effects.x09D: case Attack.Data.Effects.x092:
      if (move.Power==0) {
        if (attacker.pbTooHigh(Stats.DEFENSE)) {
          score-=90;
        }
        else {
          score-=attacker.stages[(int)Stats.DEFENSE]*20;
        }
      }
      else {
        if (attacker.stages[(int)Stats.DEFENSE]<0) score+=20;
      }
      break;
    case Attack.Data.Effects.x128:
      if (move.Power==0) {
        if (attacker.pbTooHigh(Stats.SPEED)) {
          score-=90;
        }
        else {
          score-=attacker.stages[(int)Stats.SPEED]*10;
          if (skill>=PBTrainerAI.highSkill) {
            aspeed=pbRoughStat(attacker,Stats.SPEED,skill);
            ospeed=pbRoughStat(opponent,Stats.SPEED,skill);
            if (aspeed<ospeed && aspeed*2>ospeed) {
              score+=30;
            }
          }
        }
      }
      else {
        if (attacker.stages[(int)Stats.SPEED]<0) score+=20;
      }
      break;
    case Attack.Data.Effects.x115:
      if (move.Power==0) {
        if (attacker.pbTooHigh(Stats.SPATK)) {
          score-=90;
        }
        else {
          score-=attacker.stages[(int)Stats.SPATK]*20;
          if (skill>=PBTrainerAI.mediumSkill) {
            hasspecialattack=false;
            foreach (var thismove in attacker.moves) {
              if (thismove.MoveId!=0 && thismove.Power>0 &&
                 thismove.pbIsSpecial(thismove.Type)) {
                hasspecialattack=true;
              }
            }
            if (hasspecialattack) {
              score+=20;
            } else if (skill>=PBTrainerAI.highSkill) {
              score-=90;
            }
          }
        }
      }
      else {
        if (attacker.stages[(int)Stats.SPATK]<0) score+=20;
        if (skill>=PBTrainerAI.mediumSkill) {
          hasspecialattack=false;
          foreach (var thismove in attacker.moves) {
            if (thismove.MoveId!=0 && thismove.Power>0 &&
               thismove.pbIsSpecial(thismove.Type)) {
              hasspecialattack=true;
            }
          }
          if (hasspecialattack) {
            score+=20;
          }
        }
      }
      break;
    case Attack.Data.Effects.x0AF:
      foundmove=false;
      for (int i = 0; i < 4; i++) {
        if (attacker.moves[i].Type == Types.ELECTRIC &&
           attacker.moves[i].Power>0) {
          foundmove=true;
          break;
        }
      }
      if (move.Power==0) {
        if (attacker.pbTooHigh(Stats.SPDEF)) {
          score-=90;
        }
        else {
          score-=attacker.stages[(int)Stats.SPDEF]*20;
        }
        if (foundmove) score+=20;
      }
      else {
        if (attacker.stages[(int)Stats.SPDEF]<0) score+=20;
        if (foundmove) score+=20;
      }
      break;
    case Attack.Data.Effects.x011:
      if (move.Power==0) {
        if (attacker.pbTooHigh(Stats.EVASION)) {
          score-=90;
        }
        else {
          score-=attacker.stages[(int)Stats.EVASION]*10;
        }
      }
      else {
        if (attacker.stages[(int)Stats.EVASION]<0) score+=20;
      }
      break;
    case Attack.Data.Effects.x030:
      if (move.Power==0) {
        if (attacker.effects.FocusEnergy>=2) {
          score-=80;
        }
        else {
          score+=30;
        }
      }
      else {
        if (attacker.effects.FocusEnergy<2) score+=30;
      }
      break;
    case Attack.Data.Effects.x0D1:
      if (attacker.pbTooHigh(Stats.ATTACK) &&
         attacker.pbTooHigh(Stats.DEFENSE)) {
        score-=90;
      }
      else {
        score-=attacker.stages[(int)Stats.ATTACK]*10;
        score-=attacker.stages[(int)Stats.DEFENSE]*10;
        if (skill>=PBTrainerAI.mediumSkill) {
          hasphysicalattack=false;
          foreach (var thismove in attacker.moves) {
            if (thismove.MoveId!=0 && thismove.Power>0 &&
               thismove.pbIsPhysical(thismove.Type)) {
              hasphysicalattack=true;
            }
          }
          if (hasphysicalattack) {
            score+=20;
          } else if (skill>=PBTrainerAI.highSkill) {
            score-=90;
          }
        }
      }
      break;
    case Attack.Data.Effects.x143:
      if (attacker.pbTooHigh(Stats.ATTACK) &&
         attacker.pbTooHigh(Stats.DEFENSE) &&
         attacker.pbTooHigh(Stats.ACCURACY)) {
        score-=90;
      }
      else {
        score-=attacker.stages[(int)Stats.ATTACK]*10;
        score-=attacker.stages[(int)Stats.DEFENSE]*10;
        score-=attacker.stages[(int)Stats.ACCURACY]*10;
        if (skill>=PBTrainerAI.mediumSkill) {
          hasphysicalattack=false;
          foreach (var thismove in attacker.moves) {
            if (thismove.MoveId!=0 && thismove.Power>0 &&
               thismove.pbIsPhysical(thismove.Type)) {
              hasphysicalattack=true;
            }
          }
          if (hasphysicalattack) {
            score+=20;
          } else if (skill>=PBTrainerAI.highSkill) {
            score-=90;
          }
        }
      }
      break;
    case Attack.Data.Effects.x0D5:
      if (attacker.turncount==0) score+=40;	// Dragon Dance tends to be popular
      if (attacker.pbTooHigh(Stats.ATTACK) &&
         attacker.pbTooHigh(Stats.SPEED)) {
        score-=90;
      }
      else {
        score-=attacker.stages[(int)Stats.ATTACK]*10;
        score-=attacker.stages[(int)Stats.SPEED]*10;
        if (skill>=PBTrainerAI.mediumSkill) {
          hasphysicalattack=false;
          foreach (var thismove in attacker.moves) {
            if (thismove.MoveId!=0 && thismove.Power>0 &&
               thismove.pbIsPhysical(thismove.Type)) {
              hasphysicalattack=true;
            }
          }
          if (hasphysicalattack) {
            score+=20;
          } else if (skill>=PBTrainerAI.highSkill) {
            score-=90;
          }
        }
        if (skill>=PBTrainerAI.highSkill) {
          aspeed=pbRoughStat(attacker,Stats.SPEED,skill);
          ospeed=pbRoughStat(opponent,Stats.SPEED,skill);
          if (aspeed<ospeed && aspeed*2>ospeed) {
            score+=20;
          }
        }
      }
      break;
    case Attack.Data.Effects.x148: case Attack.Data.Effects.x13D:
      if (attacker.pbTooHigh(Stats.ATTACK) &&
         attacker.pbTooHigh(Stats.SPATK)) {
        score-=90;
      }
      else {
        score-=attacker.stages[(int)Stats.ATTACK]*10;
        score-=attacker.stages[(int)Stats.SPATK]*10;
        if (skill>=PBTrainerAI.mediumSkill) {
          hasdamagingattack=false;
          foreach (var thismove in attacker.moves) {
            if (thismove.MoveId!=0 && thismove.Power>0) {
              hasdamagingattack=true; break;
            }
          }
          if (hasdamagingattack) {
            score+=20;
          } else if (skill>=PBTrainerAI.highSkill) {
            score-=90;
          }
        }
        if (move.Effect==Attack.Data.Effects.x13D) {		// Growth
          if (pbWeather()==Weather.SUNNYDAY) score+=20;
        }
      }
      break;
    case Attack.Data.Effects.x116:
      if (attacker.pbTooHigh(Stats.ATTACK) &&
         attacker.pbTooHigh(Stats.ACCURACY)) {
        score-=90;
      }
      else {
        score-=attacker.stages[(int)Stats.ATTACK]*10;
        score-=attacker.stages[(int)Stats.ACCURACY]*10;
        if (skill>=PBTrainerAI.mediumSkill) {
          hasphysicalattack=false;
          foreach (var thismove in attacker.moves) {
            if (thismove.MoveId!=0 && thismove.Power>0 &&
               thismove.pbIsPhysical(thismove.Type)) {
              hasphysicalattack=true;
            }
          }
          if (hasphysicalattack) {
            score+=20;
          } else if (skill>=PBTrainerAI.highSkill) {
            score-=90;
          }
        }
      }
      break;
    case Attack.Data.Effects.x0CF:
      if (attacker.pbTooHigh(Stats.DEFENSE) &&
         attacker.pbTooHigh(Stats.SPDEF)) {
        score-=90;
      }
      else {
        score-=attacker.stages[(int)Stats.DEFENSE]*10;
        score-=attacker.stages[(int)Stats.SPDEF]*10;
      }
      break;
    case Attack.Data.Effects.x123:
      if (attacker.pbTooHigh(Stats.SPEED) &&
         attacker.pbTooHigh(Stats.SPATK) &&
         attacker.pbTooHigh(Stats.SPDEF)) {
        score-=90;
      }
      else {
        score-=attacker.stages[(int)Stats.SPATK]*10;
        score-=attacker.stages[(int)Stats.SPDEF]*10;
        score-=attacker.stages[(int)Stats.SPEED]*10;
        if (skill>=PBTrainerAI.mediumSkill) {
          hasspecialattack=false;
          foreach (var thismove in attacker.moves) {
            if (thismove.MoveId!=0 && thismove.Power>0 &&
               thismove.pbIsSpecial(thismove.Type)) {
              hasspecialattack=true;
            }
          }
          if (hasspecialattack) {
            score+=20;
          } else if (skill>=PBTrainerAI.highSkill) {
            score-=90;
          }
        }
        if (skill>=PBTrainerAI.highSkill) {
          aspeed=pbRoughStat(attacker,Stats.SPEED,skill);
          ospeed=pbRoughStat(opponent,Stats.SPEED,skill);
          if (aspeed<ospeed && aspeed*2>ospeed) {
            score+=20;
          }
        }
      }
      break;
    case Attack.Data.Effects.x0D4:
      if (attacker.pbTooHigh(Stats.SPATK) &&
         attacker.pbTooHigh(Stats.SPDEF)) {
        score-=90;
      }
      else {
        if (attacker.turncount==0) score+=40;	// Calm Mind tends to be popular
        score-=attacker.stages[(int)Stats.SPATK]*10;
        score-=attacker.stages[(int)Stats.SPDEF]*10;
        if (skill>=PBTrainerAI.mediumSkill) {
          hasspecialattack=false;
          foreach (var thismove in attacker.moves) {
            if (thismove.MoveId!=0 && thismove.Power>0 &&
               thismove.pbIsSpecial(thismove.Type)) {
              hasspecialattack=true;
            }
          }
          if (hasspecialattack) {
            score+=20;
          } else if (skill>=PBTrainerAI.highSkill) {
            score-=90;
          }
        }
      }
      break;
    case Attack.Data.Effects.x08D:
      if (attacker.stages[(int)Stats.ATTACK]<0) score+=10;
      if (attacker.stages[(int)Stats.DEFENSE]<0) score+=10;
      if (attacker.stages[(int)Stats.SPEED]<0) score+=10;
      if (attacker.stages[(int)Stats.SPATK]<0) score+=10;
      if (attacker.stages[(int)Stats.SPDEF]<0) score+=10;
      if (skill>=PBTrainerAI.mediumSkill) {
        hasdamagingattack=false;
        foreach (var thismove in attacker.moves) {
          if (thismove.MoveId!=0 && thismove.Power>0) {
            hasdamagingattack=true;
          }
        }
        if (hasdamagingattack) {
          score+=20;
        }
      }
      break;
    case Attack.Data.Effects.x033:
      if (move.Power==0) {
        if (attacker.pbTooHigh(Stats.ATTACK)) {
          score-=90;
        }
        else {
          if (attacker.turncount==0) score+=40;
          score-=attacker.stages[(int)Stats.ATTACK]*20;
          if (skill>=PBTrainerAI.mediumSkill) {
            hasphysicalattack=false;
            foreach (var thismove in attacker.moves) {
              if (thismove.MoveId!=0 && thismove.Power>0 &&
                 thismove.pbIsPhysical(thismove.Type)) {
                hasphysicalattack=true;
              }
            }
            if (hasphysicalattack) {
              score+=20;
            } else if (skill>=PBTrainerAI.highSkill) {
              score-=90;
            }
          }
        }
      }
      else {
        if (attacker.turncount==0) score+=10;
        if (attacker.stages[(int)Stats.ATTACK]<0) score+=20;
        if (skill>=PBTrainerAI.mediumSkill) {
          hasphysicalattack=false;
          foreach (var thismove in attacker.moves) {
            if (thismove.MoveId!=0 && thismove.Power>0 &&
               thismove.pbIsPhysical(thismove.Type)) {
              hasphysicalattack=true;
            }
          }
          if (hasphysicalattack) {
            score+=20;
          }
        }
      }
      break;
    case Attack.Data.Effects.x034:
      if (move.Power==0) {
        if (attacker.pbTooHigh(Stats.DEFENSE)) {
          score-=90;
        }
        else {
          if (attacker.turncount==0) score+=40;
          score-=attacker.stages[(int)Stats.DEFENSE]*20;
        }
      }
      else {
        if (attacker.turncount==0) score+=10;
        if (attacker.stages[(int)Stats.DEFENSE]<0) score+=20;
      }
      break;
    case Attack.Data.Effects.x035: case Attack.Data.Effects.x11D:
      if (move.Power==0) {
        if (attacker.pbTooHigh(Stats.SPEED)) {
          score-=90;
        }
        else {
          if (attacker.turncount==0) score+=20;
          score-=attacker.stages[(int)Stats.SPEED]*10;
          if (skill>=PBTrainerAI.highSkill) {
            aspeed=pbRoughStat(attacker,Stats.SPEED,skill);
            ospeed=pbRoughStat(opponent,Stats.SPEED,skill);
            if (aspeed<ospeed && aspeed*2>ospeed) {
              score+=30;
            }
          }
        }
      }
      else {
        if (attacker.turncount==0) score+=10;
        if (attacker.stages[(int)Stats.SPEED]<0) score+=20;
      }
      break;
    case Attack.Data.Effects.x036:
      if (move.Power==0) {
        if (attacker.pbTooHigh(Stats.SPATK)) {
          score-=90;
        }
        else {
          if (attacker.turncount==0) score+=40;
          score-=attacker.stages[(int)Stats.SPATK]*20;
          if (skill>=PBTrainerAI.mediumSkill) {
            hasspecialattack=false;
            foreach (var thismove in attacker.moves) {
              if (thismove.MoveId!=0 && thismove.Power>0 &&
                 thismove.pbIsSpecial(thismove.Type)) {
                hasspecialattack=true;
              }
            }
            if (hasspecialattack) {
              score+=20;
            } else if (skill>=PBTrainerAI.highSkill) {
              score-=90;
            }
          }
        }
      }
      else {
        if (attacker.turncount==0) score+=10;
        if (attacker.stages[(int)Stats.SPATK]<0) score+=20;
        if (skill>=PBTrainerAI.mediumSkill) {
          hasspecialattack=false;
          foreach (var thismove in attacker.moves) {
            if (thismove.MoveId!=0 && thismove.Power>0 &&
               thismove.pbIsSpecial(thismove.Type)) {
              hasspecialattack=true;
            }
          }
          if (hasspecialattack) {
            score+=20;
          }
        }
      }
      break;
    case Attack.Data.Effects.x037:
      if (move.Power==0) {
        if (attacker.pbTooHigh(Stats.SPDEF)) {
          score-=90;
        }
        else {
          if (attacker.turncount==0) score+=40;
          score-=attacker.stages[(int)Stats.SPDEF]*20;
        }
      }
      else {
        if (attacker.turncount==0) score+=10;
        if (attacker.stages[(int)Stats.SPDEF]<0) score+=20;
      }
      break;
    case Attack.Data.Effects.x06D:
      if (move.Power==0) {
        if (attacker.pbTooHigh(Stats.EVASION)) {
          score-=90;
        }
        else {
          if (attacker.turncount==0) score+=40;
          score-=attacker.stages[(int)Stats.EVASION]*10;
        }
      }
      else {
        if (attacker.turncount==0) score+=10;
        if (attacker.stages[(int)Stats.EVASION]<0) score+=20;
      }
      break;
    case Attack.Data.Effects.x135:
      score-=attacker.stages[(int)Stats.ATTACK]*20;
      score-=attacker.stages[(int)Stats.SPEED]*20;
      score-=attacker.stages[(int)Stats.SPATK]*20;
      score+=attacker.stages[(int)Stats.DEFENSE]*10;
      score+=attacker.stages[(int)Stats.SPDEF]*10;
      if (skill>=PBTrainerAI.mediumSkill) {
        hasdamagingattack=false;
        foreach (var thismove in attacker.moves) {
          if (thismove.MoveId!=0 && thismove.Power>0) {
            hasdamagingattack=true;
          }
        }
        if (hasdamagingattack) {
          score+=20;
        }
      }
      break;
    case Attack.Data.Effects.x139:
      if (attacker.pbTooHigh(Stats.ATTACK) &&
         attacker.pbTooHigh(Stats.SPEED)) {
        score-=90;
      }
      else {
        score-=attacker.stages[(int)Stats.ATTACK]*10;
        score-=attacker.stages[(int)Stats.SPEED]*10;
        if (skill>=PBTrainerAI.mediumSkill) {
          hasphysicalattack=false;
          foreach (var thismove in attacker.moves) {
            if (thismove.MoveId!=0 && thismove.Power>0 &&
               thismove.pbIsPhysical(thismove.Type)) {
              hasphysicalattack=true;
            }
          }
          if (hasphysicalattack) {
            score+=20;
          } else if (skill>=PBTrainerAI.highSkill) {
            score-=90;
          }
        }
        if (skill>=PBTrainerAI.highSkill) {
          aspeed=pbRoughStat(attacker,Stats.SPEED,skill);
          ospeed=pbRoughStat(opponent,Stats.SPEED,skill);
          if (aspeed<ospeed && aspeed*2>ospeed) {
            score+=30;
          }
        }
      }
      break;
    case Attack.Data.Effects.x0E3:
      if (opponent.pbTooHigh(Stats.ATTACK) &&
         opponent.pbTooHigh(Stats.DEFENSE) &&
         opponent.pbTooHigh(Stats.SPEED) &&
         opponent.pbTooHigh(Stats.SPATK) &&
         opponent.pbTooHigh(Stats.SPDEF) &&
         opponent.pbTooHigh(Stats.ACCURACY) &&
         opponent.pbTooHigh(Stats.EVASION)) {
        score-=90;
      }
      else {
        int avstat=0;
        avstat-=opponent.stages[(int)Stats.ATTACK];
        avstat-=opponent.stages[(int)Stats.DEFENSE];
        avstat-=opponent.stages[(int)Stats.SPEED];
        avstat-=opponent.stages[(int)Stats.SPATK];
        avstat-=opponent.stages[(int)Stats.SPDEF];
        avstat-=opponent.stages[(int)Stats.ACCURACY];
        avstat-=opponent.stages[(int)Stats.EVASION];
        if (avstat<0) avstat=(int)Math.Floor(avstat/2f);	// More chance of getting even better
        score+=avstat*10;
      }
      break;
    case Attack.Data.Effects.x149:
      if (move.Power==0) {
        if (attacker.pbTooHigh(Stats.DEFENSE)) {
          score-=90;
        }
        else {
          if (attacker.turncount==0) score+=40;
          score-=attacker.stages[(int)Stats.DEFENSE]*30;
        }
      }
      else {
        if (attacker.turncount==0) score+=10;
        if (attacker.stages[(int)Stats.DEFENSE]<0) score+=30;
      }
      break;
    case Attack.Data.Effects.x142:
      if (move.Power==0) {
        if (attacker.pbTooHigh(Stats.SPATK)) {
          score-=90;
        }
        else {
          if (attacker.turncount==0) score+=40;
          score-=attacker.stages[(int)Stats.SPATK]*30;
          if (skill>=PBTrainerAI.mediumSkill) {
            hasspecialattack=false;
            foreach (var thismove in attacker.moves) {
              if (thismove.MoveId!=0 && thismove.Power>0 &&
                 thismove.pbIsSpecial(thismove.Type)) {
                hasspecialattack=true;
              }
            }
            if (hasspecialattack) {
              score+=20;
            } else if (skill>=PBTrainerAI.highSkill) {
              score-=90;
            }
          }
        }
      }
      else {
        if (attacker.turncount==0) score+=10;
        if (attacker.stages[(int)Stats.SPATK]<0) score+=30;
        if (skill>=PBTrainerAI.mediumSkill) {
          hasspecialattack=false;
          foreach (var thismove in attacker.moves) {
            if (thismove.MoveId!=0 && thismove.Power>0 &&
               thismove.pbIsSpecial(thismove.Type)) {
              hasspecialattack=true;
            }
          }
          if (hasspecialattack) {
            score+=30;
          }
        }
      }
      break;
    case Attack.Data.Effects.x08F:
      if (attacker.pbTooHigh(Stats.ATTACK) ||
         attacker.HP<=attacker.TotalHP/2) {
        score-=100;
      }
      else {
        score+=(6-attacker.stages[(int)Stats.ATTACK])*10;
        if (skill>=PBTrainerAI.mediumSkill) {
          hasphysicalattack=false;
          foreach (var thismove in attacker.moves) {
            if (thismove.MoveId!=0 && thismove.Power>0 &&
               thismove.pbIsPhysical(thismove.Type)) {
              hasphysicalattack=true;
            }
          }
          if (hasphysicalattack) {
            score+=40;
          } else if (skill>=PBTrainerAI.highSkill) {
            score-=90;
          }
        }
      }
      break;
    case Attack.Data.Effects.x0B7:
      avg=attacker.stages[(int)Stats.ATTACK]*10;
      avg+=attacker.stages[(int)Stats.DEFENSE]*10;
      score+=avg/2;
      break;
    case Attack.Data.Effects.x0E6:
      avg=attacker.stages[(int)Stats.DEFENSE]*10;
      avg+=attacker.stages[(int)Stats.SPDEF]*10;
      score+=avg/2;
      break;
    case Attack.Data.Effects.x14F:
      avg=attacker.stages[(int)Stats.DEFENSE]*10;
      avg+=attacker.stages[(int)Stats.SPEED]*10;
      avg+=attacker.stages[(int)Stats.SPDEF]*10;
      score+=(int)Math.Floor(avg/3f);
      break;
    case Attack.Data.Effects.x0DB:
      score+=attacker.stages[(int)Stats.SPEED]*10;
      break;
    case Attack.Data.Effects.x0CD:
      score+=attacker.stages[(int)Stats.SPATK]*10;
      break;
    case Attack.Data.Effects.x0A7:
      if (!opponent.pbCanConfuse(attacker,false)) {
        score-=90;
      }
      else {
        if (opponent.stages[(int)Stats.SPATK]<0) score+=30;
      }
      break;
    case Attack.Data.Effects.x077:
      if (!opponent.pbCanConfuse(attacker,false)) {
        score-=90;
      }
      else {
        if (opponent.stages[(int)Stats.ATTACK]<0) score+=30;
      }
      break;
    case Attack.Data.Effects.x16D:
      if (move.Power==0) {
        if (!opponent.pbCanReduceStatStage(Stats.ATTACK,attacker)) {
          score-=90;
        }
        else {
          score+=opponent.stages[(int)Stats.ATTACK]*20;
          if (skill>=PBTrainerAI.mediumSkill) {
            hasphysicalattack=false;
            foreach (var thismove in opponent.moves) {
              if (thismove.MoveId!=0 && thismove.Power>0 &&
                 thismove.pbIsPhysical(thismove.Type)) {
                hasphysicalattack=true;
              }
            }
            if (hasphysicalattack) {
              score+=20;
            } else if (skill>=PBTrainerAI.highSkill) {
              score-=90;
            }
          }
        }
      }
      else {
        if (opponent.stages[(int)Stats.ATTACK]>0) score+=20;
        if (skill>=PBTrainerAI.mediumSkill) {
          hasphysicalattack=false;
          foreach (var thismove in opponent.moves) {
            if (thismove.MoveId!=0 && thismove.Power>0 &&
               thismove.pbIsPhysical(thismove.Type)) {
              hasphysicalattack=true;
            }
          }
          if (hasphysicalattack) {
            score+=20;
          }
        }
      }
      break;
    case Attack.Data.Effects.x014:
      if (move.Power==0) {
        if (!opponent.pbCanReduceStatStage(Stats.DEFENSE,attacker)) {
          score-=90;
        }
        else {
          score+=opponent.stages[(int)Stats.DEFENSE]*20;
        }
      }
      else {
        if (opponent.stages[(int)Stats.DEFENSE]>0) score+=20;
      }
      break;
    case Attack.Data.Effects.x015:
      if (move.Power==0) {
        if (!opponent.pbCanReduceStatStage(Stats.SPEED,attacker)) {
          score-=90;
        }
        else {
          score+=opponent.stages[(int)Stats.SPEED]*10;
          if (skill>=PBTrainerAI.highSkill) {
            aspeed=pbRoughStat(attacker,Stats.SPEED,skill);
            ospeed=pbRoughStat(opponent,Stats.SPEED,skill);
            if (aspeed<ospeed && aspeed*2>ospeed) {
              score+=30;
            }
          }
        }
      }
      else {
        if (attacker.stages[(int)Stats.SPEED]>0) score+=20;
      }
      break;
    case Attack.Data.Effects.x048:
      if (move.Power==0) {
        if (!opponent.pbCanReduceStatStage(Stats.SPATK,attacker)) {
          score-=90;
        }
        else {
          score+=attacker.stages[(int)Stats.SPATK]*20;
          if (skill>=PBTrainerAI.mediumSkill) {
            hasspecialattack=false;
            foreach (var thismove in opponent.moves) {
              if (thismove.MoveId!=0 && thismove.Power>0 &&
                 thismove.pbIsSpecial(thismove.Type)) {
                hasspecialattack=true;
              }
            }
            if (hasspecialattack) {
              score+=20;
            } else if (skill>=PBTrainerAI.highSkill) {
              score-=90;
            }
          }
        }
      }
      else {
        if (attacker.stages[(int)Stats.SPATK]>0) score+=20;
        if (skill>=PBTrainerAI.mediumSkill) {
          hasspecialattack=false;
          foreach (var thismove in opponent.moves) {
            if (thismove.MoveId!=0 && thismove.Power>0 &&
               thismove.pbIsSpecial(thismove.Type)) {
              hasspecialattack=true;
            }
          }
          if (hasspecialattack) {
            score+=20;
          }
        }
      }
      break;
    case Attack.Data.Effects.x049:
      if (move.Power==0) {
        if (!opponent.pbCanReduceStatStage(Stats.SPDEF,attacker)) {
          score-=90;
        }
        else {
          score+=opponent.stages[(int)Stats.SPDEF]*20;
        }
      }
      else {
        if (opponent.stages[(int)Stats.SPDEF]>0) score+=20;
      }
      break;
    case Attack.Data.Effects.x018:
      if (move.Power==0) {
        if (!opponent.pbCanReduceStatStage(Stats.ACCURACY,attacker)) {
          score-=90;
        }
        else {
          score+=opponent.stages[(int)Stats.ACCURACY]*10;
        }
      }
      else {
        if (opponent.stages[(int)Stats.ACCURACY]>0) score+=20;
      }
      break;
    case Attack.Data.Effects.x019:
      if (move.Power==0) {
        if (!opponent.pbCanReduceStatStage(Stats.EVASION,attacker)) {
          score-=90;
        }
        else {
          score+=opponent.stages[(int)Stats.EVASION]*10;
        }
      }
      else {
        if (opponent.stages[(int)Stats.EVASION]>0) score+=20;
      }
      break;
    case Attack.Data.Effects.x103:
      if (move.Power==0) {
        if (!opponent.pbCanReduceStatStage(Stats.EVASION,attacker)) {
          score-=90;
        }
        else {
          score+=opponent.stages[(int)Stats.EVASION]*10;
        }
      }
      else {
        if (opponent.stages[(int)Stats.EVASION]>0) score+=20;
      }
      if (opponent.OwnSide.Reflect>0 ||
                   opponent.OwnSide.LightScreen>0 ||
                   opponent.OwnSide.Mist>0 ||
                   opponent.OwnSide.Safeguard>0) score+=30;
      if (opponent.OwnSide.Spikes>0 ||
                   opponent.OwnSide.ToxicSpikes>0 ||
                   opponent.OwnSide.StealthRock) score-=30;
      break;
    case Attack.Data.Effects.x0CE:
      avg=opponent.stages[(int)Stats.ATTACK]*10;
      avg+=opponent.stages[(int)Stats.DEFENSE]*10;
      score+=avg/2;
      break;
    case Attack.Data.Effects.x03B:
      if (move.Power==0) {
        if (!opponent.pbCanReduceStatStage(Stats.ATTACK,attacker)) {
          score-=90;
        }
        else {
          if (attacker.turncount==0) score+=40;
          score+=opponent.stages[(int)Stats.ATTACK]*20;
          if (skill>=PBTrainerAI.mediumSkill) {
            hasphysicalattack=false;
            foreach (var thismove in opponent.moves) {
              if (thismove.MoveId!=0 && thismove.Power>0 &&
                 thismove.pbIsPhysical(thismove.Type)) {
                hasphysicalattack=true;
              }
            }
            if (hasphysicalattack) {
              score+=20;
            } else if (skill>=PBTrainerAI.highSkill) {
              score-=90;
            }
          }
        }
      }
      else {
        if (attacker.turncount==0) score+=10;
        if (opponent.stages[(int)Stats.ATTACK]>0) score+=20;
        if (skill>=PBTrainerAI.mediumSkill) {
          hasphysicalattack=false;
          foreach (var thismove in opponent.moves) {
            if (thismove.MoveId!=0 && thismove.Power>0 &&
               thismove.pbIsPhysical(thismove.Type)) {
              hasphysicalattack=true;
            }
          }
          if (hasphysicalattack) {
            score+=20;
          }
        }
      }
      break;
    case Attack.Data.Effects.x03C:
      if (move.Power==0) {
        if (!opponent.pbCanReduceStatStage(Stats.DEFENSE,attacker)) {
          score-=90;
        }
        else {
          if (attacker.turncount==0) score+=40;
          score+=opponent.stages[(int)Stats.DEFENSE]*20;
        }
      }
      else {
        if (attacker.turncount==0) score+=10;
        if (opponent.stages[(int)Stats.DEFENSE]>0) score+=20;
      }
      break;
    case Attack.Data.Effects.x03D:
      if (move.Power==0) {
        if (!opponent.pbCanReduceStatStage(Stats.SPEED,attacker)) {
          score-=90;
        }
        else {
          if (attacker.turncount==0) score+=20;
          score+=opponent.stages[(int)Stats.SPEED]*20;
          if (skill>=PBTrainerAI.highSkill) {
            aspeed=pbRoughStat(attacker,Stats.SPEED,skill);
            ospeed=pbRoughStat(opponent,Stats.SPEED,skill);
            if (aspeed<ospeed && aspeed*2>ospeed) {
              score+=30;
            }
          }
        }
      }
      else {
        if (attacker.turncount==0) score+=10;
        if (opponent.stages[(int)Stats.SPEED]>0) score+=30;
      }
      break;
    case Attack.Data.Effects.x10A:
      if (attacker.Gender==null || opponent.Gender==null ||
         attacker.Gender==opponent.Gender ||
         opponent.hasWorkingAbility(Abilities.OBLIVIOUS)) {
        score-=90;
      } else if (move.Power==0) {
        if (!opponent.pbCanReduceStatStage(Stats.SPATK,attacker)) {
          score-=90;
        }
        else {
          if (attacker.turncount==0) score+=40;
          score+=opponent.stages[(int)Stats.SPATK]*20;
          if (skill>=PBTrainerAI.mediumSkill) {
            hasspecialattack=false;
            foreach (var thismove in opponent.moves) {
              if (thismove.MoveId!=0 && thismove.Power>0 &&
                 thismove.pbIsSpecial(thismove.Type)) {
                hasspecialattack=true;
              }
            }
            if (hasspecialattack) {
              score+=20;
            } else if (skill>=PBTrainerAI.highSkill) {
              score-=90;
            }
          }
        }
      }
      else {
        if (attacker.turncount==0) score+=10;
        if (opponent.stages[(int)Stats.SPATK]>0) score+=20;
        if (skill>=PBTrainerAI.mediumSkill) {
          hasspecialattack=false;
          foreach (var thismove in opponent.moves) {
            if (thismove.MoveId!=0 && thismove.Power>0 &&
               thismove.pbIsSpecial(thismove.Type)) {
              hasspecialattack=true;
            }
          }
          if (hasspecialattack) {
            score+=30;
          }
        }
      }
      break;
    case Attack.Data.Effects.x03F:
      if (move.Power==0) {
        if (!opponent.pbCanReduceStatStage(Stats.SPDEF,attacker)) {
          score-=90;
        }
        else {
          if (attacker.turncount==0) score+=40;
          score+=opponent.stages[(int)Stats.SPDEF]*20;
        }
      }
      else {
        if (attacker.turncount==0) score+=10;
        if (opponent.stages[(int)Stats.SPDEF]>0) score+=20;
      }
      break;
    case Attack.Data.Effects.x131:
      if (opponent.effects.Substitute>0) {
        score-=90;
      }
      else {
        bool anychange=false;
        if (avg!=0) avg=opponent.stages[(int)Stats.ATTACK]; anychange=true;
        if (avg!=0) avg+=opponent.stages[(int)Stats.DEFENSE]; anychange=true;
        if (avg!=0) avg+=opponent.stages[(int)Stats.SPEED]; anychange=true;
        if (avg!=0) avg+=opponent.stages[(int)Stats.SPATK]; anychange=true;
        if (avg!=0) avg+=opponent.stages[(int)Stats.SPDEF]; anychange=true;
        if (avg!=0) avg+=opponent.stages[(int)Stats.ACCURACY]; anychange=true;
        if (avg!=0) avg+=opponent.stages[(int)Stats.EVASION]; anychange=true;
        if (anychange) {
          score+=avg*10;
        }
        else {
          score-=90;
        }
      }
      break;
    case Attack.Data.Effects.x01A:
      if (skill>=PBTrainerAI.mediumSkill) {
        int stages=0;
        for (int i = 0; i < 4; i++) {
          Combat.Pokemon battler=@battlers[i];
          if (attacker.IsOpposing(i)) {
            stages+=battler.stages[(int)Stats.ATTACK];
            stages+=battler.stages[(int)Stats.DEFENSE];
            stages+=battler.stages[(int)Stats.SPEED];
            stages+=battler.stages[(int)Stats.SPATK];
            stages+=battler.stages[(int)Stats.SPDEF];
            stages+=battler.stages[(int)Stats.EVASION];
            stages+=battler.stages[(int)Stats.ACCURACY];
          }
          else {
            stages-=battler.stages[(int)Stats.ATTACK];
            stages-=battler.stages[(int)Stats.DEFENSE];
            stages-=battler.stages[(int)Stats.SPEED];
            stages-=battler.stages[(int)Stats.SPATK];
            stages-=battler.stages[(int)Stats.SPDEF];
            stages-=battler.stages[(int)Stats.EVASION];
            stages-=battler.stages[(int)Stats.ACCURACY];
          }
        }
        score+=stages*10;
      }
      break;
    case Attack.Data.Effects.x0F4:
      if (skill>=PBTrainerAI.mediumSkill) {
        aatk=attacker.stages[(int)Stats.ATTACK];
        aspa=attacker.stages[(int)Stats.SPATK];
        oatk=opponent.stages[(int)Stats.ATTACK];
        ospa=opponent.stages[(int)Stats.SPATK];
        if (aatk>=oatk && aspa>=ospa) {
          score-=80;
        }
        else {
          score+=(oatk-aatk)*10;
          score+=(ospa-aspa)*10;
        }
      }
      else {
        score-=50;
      }
      break;
    case Attack.Data.Effects.x0F5:
      if (skill>=PBTrainerAI.mediumSkill) {
        adef=attacker.stages[(int)Stats.DEFENSE];
        aspd=attacker.stages[(int)Stats.SPDEF];
        odef=opponent.stages[(int)Stats.DEFENSE];
        ospd=opponent.stages[(int)Stats.SPDEF];
        if (adef>=odef && aspd>=ospd) {
          score-=80;
        }
        else {
          score+=(odef-adef)*10;
          score+=(ospd-aspd)*10;
        }
      }
      else {
        score-=50;
      }
      break;
    case Attack.Data.Effects.x0FB:
      if (skill>=PBTrainerAI.mediumSkill) {
        astages=attacker.stages[(int)Stats.ATTACK];
        astages+=attacker.stages[(int)Stats.DEFENSE];
        astages+=attacker.stages[(int)Stats.SPEED];
        astages+=attacker.stages[(int)Stats.SPATK];
        astages+=attacker.stages[(int)Stats.SPDEF];
        astages+=attacker.stages[(int)Stats.EVASION];
        astages+=attacker.stages[(int)Stats.ACCURACY];
        ostages=opponent.stages[(int)Stats.ATTACK];
        ostages+=opponent.stages[(int)Stats.DEFENSE];
        ostages+=opponent.stages[(int)Stats.SPEED];
        ostages+=opponent.stages[(int)Stats.SPATK];
        ostages+=opponent.stages[(int)Stats.SPDEF];
        ostages+=opponent.stages[(int)Stats.EVASION];
        ostages+=opponent.stages[(int)Stats.ACCURACY];
        score+=(ostages-astages)*10;
      }
      else {
        score-=50;
      }
      break;
    case Attack.Data.Effects.x090:
      if (skill>=PBTrainerAI.mediumSkill) {
        bool equal=true;
        foreach (var i in new Stats[] { Stats.ATTACK,Stats.DEFENSE,Stats.SPEED,
                 Stats.SPATK,Stats.SPDEF,Stats.ACCURACY,Stats.EVASION }) {
          int stagediff=opponent.stages[(int)i]-attacker.stages[(int)i];
          score+=stagediff*10;
          if (stagediff!=0) equal=false;
        }
        if (equal) score-=80;
      }
      else {
        score-=50;
      }
      break;
    case Attack.Data.Effects.x02F:
      if (attacker.OwnSide.Mist>0) score-=80;
      break;
    case Attack.Data.Effects.x0EF:
      if (skill>=PBTrainerAI.mediumSkill) {
        aatk=pbRoughStat(attacker,Stats.ATTACK,skill);
        adef=pbRoughStat(attacker,Stats.DEFENSE,skill);
        if (aatk==adef ||
           attacker.effects.PowerTrick) { // No flip-flopping
          score-=90;
        } else if (adef>aatk) {		// Prefer a higher Attack
          score+=30;
        }
        else {
          score-=30;
        }
      }
      else {
        score-=30;
      }
      break;
    case Attack.Data.Effects.x119:
      if (skill>=PBTrainerAI.mediumSkill) {
        aatk=pbRoughStat(attacker,Stats.ATTACK,skill);
        aspatk=pbRoughStat(attacker,Stats.SPATK,skill);
        oatk=pbRoughStat(opponent,Stats.ATTACK,skill);
        ospatk=pbRoughStat(opponent,Stats.SPATK,skill);
        if (aatk<oatk && aspatk<ospatk) {
          score+=50;
        } else if ((aatk+aspatk)<(oatk+ospatk)) {
          score+=30;
        }
        else {
          score-=50;
        }
      }
      else {
        score-=30;
      }
      break;
    case Attack.Data.Effects.x118:
      if (skill>=PBTrainerAI.mediumSkill) {
        adef=pbRoughStat(attacker,Stats.DEFENSE,skill);
        aspdef=pbRoughStat(attacker,Stats.SPDEF,skill);
        odef=pbRoughStat(opponent,Stats.DEFENSE,skill);
        ospdef=pbRoughStat(opponent,Stats.SPDEF,skill);
        if (adef<odef && aspdef<ospdef) {
          score+=50;
        } else if ((adef+aspdef)<(odef+ospdef)) {
          score+=30;
        }
        else {
          score-=50;
        }
      }
      else {
        score-=30;
      }
      break;
    case Attack.Data.Effects.x05C:
      if (opponent.effects.Substitute>0) {
        score-=90;
      } else if (attacker.HP>=(attacker.HP+opponent.HP)/2) {
        score-=90;
      }
      else {
        score+=40;
      }
      break;
    case Attack.Data.Effects.x0E2:
      if (attacker.OwnSide.Tailwind>0) {
        score-=90;
      }
      break;
    case Attack.Data.Effects.x053:
      blacklist=new Attack.Data.Effects[] {
         Attack.Data.Effects.x0FF,   // Struggle
         Attack.Data.Effects.x10C,   // Chatter
         Attack.Data.Effects.x053,   // Mimic
         Attack.Data.Effects.x060,   // Sketch
         Attack.Data.Effects.x054    // Metronome
      };
      if (attacker.effects.Transform ||
         opponent.lastMoveUsed<=0 ||
         Game.MoveData[opponent.lastMoveUsed].Type == Types.SHADOW ||
         blacklist.Contains(Game.MoveData[opponent.lastMoveUsed].Effect)) {
        score-=90;
      }
      foreach (var i in attacker.moves) {
        if (i.MoveId==opponent.lastMoveUsed) {
          score-=90; break;
        }
      }
      break;
    case Attack.Data.Effects.x060:
      blacklist=new Attack.Data.Effects[] {
         Attack.Data.Effects.x0FF,   // Struggle
         Attack.Data.Effects.x10C,   // Chatter
         Attack.Data.Effects.x060    // Sketch
      };
      if (attacker.effects.Transform ||
         opponent.lastMoveUsedSketch<=0 ||
         Game.MoveData[opponent.lastMoveUsedSketch].Type == Types.SHADOW ||
         blacklist.Contains(Game.MoveData[opponent.lastMoveUsedSketch].Effect)) {
        score-=90;
      }
      foreach (var i in attacker.moves) {
        if (i.MoveId==opponent.lastMoveUsedSketch) {
          score-=90; break;
        }
      }
      break;
    case Attack.Data.Effects.x01F:
      if (attacker.Ability == Abilities.MULTITYPE) {
        score-=90;
      }
      else {
        List<Types> types=new List<Types>();
        foreach (var i in attacker.moves) {
          //if (i.MoveId==@id) continue;
          //if (Types.isPseudoType(i.Type)) continue;
          if (attacker.hasType(i.Type)) continue;
          bool found=false;
          if (!types.Contains(i.Type)) types.Add(i.Type);
        }
        if (types.Count==0) {
          score-=90;
        }
      }
      break;
    case Attack.Data.Effects.x05E:
      if (attacker.Ability == Abilities.MULTITYPE) {
        score-=90;
      } else if (opponent.lastMoveUsed<=0
         ) { //|| PBTypes.isPseudoType(Game.MoveData[opponent.lastMoveUsed].Type)
        score-=90;
      }
      else {
        Types atype=Types.UNKNOWN;
        foreach (var i in opponent.moves) {
          if (i.MoveId==opponent.lastMoveUsed) {
            //atype=i.pbType(move.Type,attacker,opponent); break;
          }
        }
        if (atype<0) {
          score-=90;
        }
        else {
        List<Types> types=new List<Types>();
          //for (int i = 0; i < Game.TypeData.Keys.Count; i++) {
          foreach (Types i in Game.TypeData.Keys) {
            if (attacker.hasType(i)) continue;
            if (atype.GetEffectiveness(i)<2 ) types.Add(i);
          }
          if (types.Count==0) {
            score-=90;
          }
        }
      }
      break;
    case Attack.Data.Effects.x0D6:
      if (attacker.Ability == Abilities.MULTITYPE) {
        score-=90;
      } else if (skill>=PBTrainerAI.mediumSkill) {
        Types[] envtypes=new Types[] {
           Types.NORMAL, // None
           Types.GRASS,  // Grass
           Types.GRASS,  // Tall grass
           Types.WATER,  // Moving water
           Types.WATER,  // Still water
           Types.WATER,  // Underwater
           Types.ROCK,   // Rock
           Types.ROCK,   // Cave
           Types.GROUND  // Sand
        };
        Types type=envtypes[(int)@environment];
        if (attacker.hasType(type)) score-=90;
      }
      break;
    case Attack.Data.Effects.x127:
      if (opponent.effects.Substitute>0 ||
         opponent.Ability == Abilities.MULTITYPE) {
        score-=90;
      } else if (opponent.hasType(Types.WATER)) {
        score-=90;
      }
      break;
    case Attack.Data.Effects.x13F:
      if (attacker.Ability == Abilities.MULTITYPE) {
        score-=90;
      } else if (attacker.hasType(opponent.Type1) &&
         attacker.hasType(opponent.Type2) &&
         opponent.hasType(attacker.Type1) &&
         opponent.hasType(attacker.Type2)) {
        score-=90;
      }
      break;
    case Attack.Data.Effects.x12B:
      if (opponent.effects.Substitute>0) {
        score-=90;
      } else if (skill>=PBTrainerAI.mediumSkill) {
        if (opponent.Ability == Abilities.MULTITYPE ||
           opponent.Ability == Abilities.SIMPLE ||
           opponent.Ability == Abilities.TRUANT) {
          score-=90;
        }
      }
      break;
    case Attack.Data.Effects.x0F8:
      if (opponent.effects.Substitute>0) {
        score-=90;
      } else if (skill>=PBTrainerAI.mediumSkill) {
        if (opponent.Ability == Abilities.MULTITYPE ||
           opponent.Ability == Abilities.INSOMNIA ||
           opponent.Ability == Abilities.TRUANT) {
          score-=90;
        }
      }
      break;
    case Attack.Data.Effects.x0B3:
      score-=40; // don't prefer this move
      if (skill>=PBTrainerAI.mediumSkill) {
        if (opponent.Ability==0 ||
           attacker.Ability==opponent.Ability ||
           attacker.Ability == Abilities.MULTITYPE ||
           opponent.Ability == Abilities.FLOWER_GIFT ||
           opponent.Ability == Abilities.FORECAST ||
           opponent.Ability == Abilities.ILLUSION ||
           opponent.Ability == Abilities.IMPOSTER ||
           opponent.Ability == Abilities.MULTITYPE ||
           opponent.Ability == Abilities.TRACE ||
           opponent.Ability == Abilities.WONDER_GUARD ||
           opponent.Ability == Abilities.ZEN_MODE) {
          score-=90;
        }
      }
      if (skill>=PBTrainerAI.highSkill) {
        if (opponent.Ability == Abilities.TRUANT && 
           attacker.IsOpposing(opponent.Index)) {
          score-=90;
        } else if (opponent.Ability == Abilities.SLOW_START &&
           attacker.IsOpposing(opponent.Index)) {
          score-=90;
        }
      }
      break;
    case Attack.Data.Effects.x12C:
      score-=40; // don't prefer this move
      if (opponent.effects.Substitute>0) {
        score-=90;
      } else if (skill>=PBTrainerAI.mediumSkill) {
        if (attacker.Ability==0 ||
           attacker.Ability==opponent.Ability ||
           opponent.Ability == Abilities.MULTITYPE ||
           opponent.Ability == Abilities.TRUANT ||
           attacker.Ability == Abilities.FLOWER_GIFT ||
           attacker.Ability == Abilities.FORECAST ||
           attacker.Ability == Abilities.ILLUSION ||
           attacker.Ability == Abilities.IMPOSTER ||
           attacker.Ability == Abilities.MULTITYPE ||
           attacker.Ability == Abilities.TRACE ||
           attacker.Ability == Abilities.ZEN_MODE) {
          score-=90;
        }
        if (skill>=PBTrainerAI.highSkill) {
          if (attacker.Ability == Abilities.TRUANT && 
             attacker.IsOpposing(opponent.Index)) {
            score+=90;
          } else if (attacker.Ability == Abilities.SLOW_START &&
             attacker.IsOpposing(opponent.Index)) {
            score+=90;
          }
        }
      }
      break;
    case Attack.Data.Effects.x0C0:
      score-=40; // don't prefer this move
      if (skill>=PBTrainerAI.mediumSkill) {
        if ((attacker.Ability==0 && opponent.Ability==0) ||
           attacker.Ability==opponent.Ability ||
           attacker.Ability == Abilities.ILLUSION ||
           opponent.Ability == Abilities.ILLUSION ||
           attacker.Ability == Abilities.MULTITYPE ||
           opponent.Ability == Abilities.MULTITYPE ||
           attacker.Ability == Abilities.WONDER_GUARD ||
           opponent.Ability == Abilities.WONDER_GUARD) {
          score-=90;
        }
      }
      if (skill>=PBTrainerAI.highSkill) {
        if (opponent.Ability == Abilities.TRUANT && 
           attacker.IsOpposing(opponent.Index)) {
          score-=90;
        } else if (opponent.Ability == Abilities.SLOW_START &&
          attacker.IsOpposing(opponent.Index)) {
          score-=90;
        }
      }
      break;
    case Attack.Data.Effects.x0F0:
      if (opponent.effects.Substitute>0 ||
         opponent.effects.GastroAcid) {
        score-=90;
      } else if (skill>=PBTrainerAI.highSkill) {
        if (opponent.Ability == Abilities.MULTITYPE) score-=90;
        if (opponent.Ability == Abilities.SLOW_START) score-=90;
        if (opponent.Ability == Abilities.TRUANT) score-=90;
      }
      break;
    case Attack.Data.Effects.x03A:
      score-=70;
      break;
    case Attack.Data.Effects.x083:
      if (opponent.HP<=20) {
        score+=80;
      } else if (opponent.Level>=25) {
        score-=80; // Not useful against high-level Pokemon
      }
      break;
    case Attack.Data.Effects.x02A:
      if (opponent.HP<=40) score+=80;
      break;
    case Attack.Data.Effects.x029:
      score-=50;
      score+=(int)Math.Floor(opponent.HP*100f/opponent.TotalHP);
      break;
    case Attack.Data.Effects.x058:
      if (opponent.HP<=attacker.Level) score+=80;
      break;
    case Attack.Data.Effects.x0BE:
      if (attacker.HP>=opponent.HP) {
        score-=90;
      } else if (attacker.HP*2<opponent.HP) {
        score+=50;
      }
      break;
    case Attack.Data.Effects.x059:
      if (opponent.HP<=attacker.Level) score+=30;
      break;
    case Attack.Data.Effects.x027:
      if (opponent.hasWorkingAbility(Abilities.STURDY)) score-=90;
      if (opponent.Level>attacker.Level) score-=90;
      break;
    case Attack.Data.Effects.x05A:
      if (opponent.effects.HyperBeam>0) {
        score-=90;
      }
      else {
        attack=pbRoughStat(attacker,Stats.ATTACK,skill);
        spatk=pbRoughStat(attacker,Stats.SPATK,skill);
        if (attack*1.5<spatk) {
          score-=60;
        } else if (skill>=PBTrainerAI.mediumSkill &&
           opponent.lastMoveUsed>0) {
          moveData=Game.MoveData[opponent.lastMoveUsed];
          if (moveData.Power>0 &&
             (Core.USEMOVECATEGORY && moveData.Category==Attack.Category.STATUS) ||
             (!Core.USEMOVECATEGORY && Game.TypeData[moveData.Type].Category == Attack.Category.SPECIAL)) {
            score-=60;
          }
        }
      }
      break;
    case Attack.Data.Effects.x091:
      if (opponent.effects.HyperBeam>0) {
        score-=90;
      }
      else {
        attack=pbRoughStat(attacker,Stats.ATTACK,skill);
        spatk=pbRoughStat(attacker,Stats.SPATK,skill);
        if (attack>spatk*1.5) {
          score-=60;
        } else if (skill>=PBTrainerAI.mediumSkill && opponent.lastMoveUsed>0) {
          moveData=Game.MoveData[opponent.lastMoveUsed];
          if (moveData.Power>0 &&
             (Core.USEMOVECATEGORY && moveData.Category==Attack.Category.SPECIAL) ||
             (!Core.USEMOVECATEGORY && Game.TypeData[moveData.Type].Category != Attack.Category.SPECIAL)) {
            score-=60;
          }
        }
      }
      break;
    case Attack.Data.Effects.x0E4:
      if (opponent.effects.HyperBeam>0) score-=90;
      break;
    case Attack.Data.Effects.x122:
      if (!opponent.Partner.isFainted()) score+=10;
      break;
    case Attack.Data.Effects.x102:
      break;
    case Attack.Data.Effects.x094:
      break;
    case Attack.Data.Effects.x096:
      break;
    case Attack.Data.Effects.x093:
      if (skill>=PBTrainerAI.highSkill) {
        if (!opponent.hasWorkingAbility(Abilities.INNER_FOCUS) &&
                     opponent.effects.Substitute==0) score+=30;
      }
      break;
    case Attack.Data.Effects.x151:
      break;
    case Attack.Data.Effects.x150:
      break;
    case Attack.Data.Effects.x11C:
      break;
    case Attack.Data.Effects.x0AC:
      if (opponent.Status==Status.PARALYSIS) score-=20;	// Will cure status
      break;
    case Attack.Data.Effects.x0DA:
      if (opponent.Status==Status.SLEEP &&	// Will cure status
                   opponent.StatusCount>1) score-=20;
      break;
    case Attack.Data.Effects.x0AA:
      break;
    case Attack.Data.Effects.x137:
      break;
    case Attack.Data.Effects.x0DE:
      break;
    case Attack.Data.Effects.x0BA:
      int attspeed=pbRoughStat(attacker,Stats.SPEED,skill);
      int oppspeed=pbRoughStat(opponent,Stats.SPEED,skill);
      if (oppspeed>attspeed) score+=30;
      break;
    case Attack.Data.Effects.x0E8:
      if (@doublebattle) score+=20;
      break;
    case Attack.Data.Effects.x12E:
      if (skill>=PBTrainerAI.mediumSkill) {
        if (@doublebattle && !attacker.Partner.isFainted() &&
                     //attacker.Partner.pbHasMove(move.MoveId)) score+=20;
                     attacker.Partner.moves.Any(n => n.MoveId == move.MoveId)) score+=20;
      }
      break;
    case Attack.Data.Effects.x0E7:
      attspeed=pbRoughStat(attacker,Stats.SPEED,skill);
      oppspeed=pbRoughStat(opponent,Stats.SPEED,skill);
      if (oppspeed>attspeed) score+=30;
      break;
    case Attack.Data.Effects.x140:
    case Attack.Data.Effects.x13E:
    case Attack.Data.Effects.x0CC:
    case Attack.Data.Effects.x081:
    case Attack.Data.Effects.x07A:
    case Attack.Data.Effects.x07C:
    case Attack.Data.Effects.x0BF:
    case Attack.Data.Effects.x0EE:
    case Attack.Data.Effects.x0DC:
    case Attack.Data.Effects.x132:
    case Attack.Data.Effects.x0F6:
    case Attack.Data.Effects.x088:
    case Attack.Data.Effects.x078:
    case Attack.Data.Effects.x12F:
      break;
    case Attack.Data.Effects.x052:
      if (attacker.effects.Rage) score+=25;
      break;
    case Attack.Data.Effects.x07B:
      break;
    case Attack.Data.Effects.x07F:
      break;
    case Attack.Data.Effects.x0DF:
      break;
    case Attack.Data.Effects.x0EC:
      break;
    case Attack.Data.Effects.x064:
      break;
    case Attack.Data.Effects.x126:
      break;
    case Attack.Data.Effects.x0C5:
      break;
    case Attack.Data.Effects.x124:
      break;
    case Attack.Data.Effects.x0B1:
      if (attacker.Partner.isFainted()) score-=90;
      break;
    case Attack.Data.Effects.x0CA:
      if (attacker.effects.MudSport) score-=90;
      break;
    case Attack.Data.Effects.x0D3:
      if (attacker.effects.WaterSport) score-=90;
      break;
    case Attack.Data.Effects.x10D:
      break;
    case Attack.Data.Effects.x121:
      break;
    case Attack.Data.Effects.x0F1:
      if (attacker.OwnSide.LuckyChant>0) score-=90;
      break;
    case Attack.Data.Effects.x042:
      if (attacker.OwnSide.Reflect>0) score-=90;
      break;
    case Attack.Data.Effects.x024:
      if (attacker.OwnSide.LightScreen>0) score-=90;
      break;
    case Attack.Data.Effects.x0C6:
      break;
    case Attack.Data.Effects.x17D:
      break;
    case Attack.Data.Effects.x05F:
      if (opponent.effects.Substitute>0) score-=90;
      if (opponent.effects.LockOn>0) score-=90;
      break;
    case Attack.Data.Effects.x072:
      if (opponent.effects.Foresight) {
        score-=90;
      } else if (opponent.hasType(Types.GHOST)) {
        score+=70;
      } else if (opponent.stages[(int)Stats.EVASION]<=0) {
        score-=60;
      }
      break;
    case Attack.Data.Effects.x0D9:
      if (opponent.effects.MiracleEye) {
        score-=90;
      } else if (opponent.hasType(Types.DARK)) {
        score+=70;
      } else if (opponent.stages[(int)Stats.EVASION]<=0) {
        score-=60;
      }
      break;
    case Attack.Data.Effects.x130:
      break;
    case Attack.Data.Effects.x070:
      if (attacker.effects.ProtectRate>1 ||
         opponent.effects.HyperBeam>0) {
        score-=90;
      }
      else {
        if (skill>=PBTrainerAI.mediumSkill) {
          score-=(attacker.effects.ProtectRate*40);
        }
        if (attacker.turncount==0) score+=50;
        if (opponent.effects.TwoTurnAttack!=0) score+=30;
      }
      break;
    case Attack.Data.Effects.x133:
      break;
    case Attack.Data.Effects.x117:
      break;
    case Attack.Data.Effects.x0E0:
      break;
    case Attack.Data.Effects.x00A:
      score-=40;
      if (skill>=PBTrainerAI.highSkill) {
        if (opponent.lastMoveUsed<=0 ||
                     //(Game.MoveData[opponent.lastMoveUsed].flags&0x010)==0) score-=100; // flag e: Copyable by Mirror Move
                     Game.MoveData[opponent.lastMoveUsed].Flags.Mirror) score-=100; 
      }
      break;
    case Attack.Data.Effects.x0F3:
      break;
    case Attack.Data.Effects.x0F2:
      break;
    case Attack.Data.Effects.x0B8:
      break;
    case Attack.Data.Effects.x0C4:
      break;
    case Attack.Data.Effects.x0AE:
      break;
    case Attack.Data.Effects.x062:
      if (attacker.Status==Status.SLEEP) {
        score+=200; // Because it can be used while asleep
      }
      else {
        score-=80;
      }
      break;
    case Attack.Data.Effects.x0B5:
      break;
    case Attack.Data.Effects.x054:
      break;
    case Attack.Data.Effects.x0A6:
      if (opponent.effects.Torment) score-=90;
      break;
    case Attack.Data.Effects.x0C1:
      if (attacker.effects.Imprison) score-=90;
      break;
    case Attack.Data.Effects.x057:
      if (opponent.effects.Disable>0 ) score-=90;
      break;
    case Attack.Data.Effects.x0B0:
      if (opponent.effects.Taunt>0) score-=90;
      break;
    case Attack.Data.Effects.x0ED:
      if (opponent.effects.HealBlock>0) score-=90;
      break;
    case Attack.Data.Effects.x05B:
      aspeed=pbRoughStat(attacker,Stats.SPEED,skill);
      ospeed=pbRoughStat(opponent,Stats.SPEED,skill);
      if (opponent.effects.Encore>0) {
        score-=90;
      } else if (aspeed>ospeed) {
        if (opponent.lastMoveUsed<=0) {
          score-=90;
        }
        else {
          moveData=Game.MoveData[opponent.lastMoveUsed];
          if (moveData.Power==0 && (moveData.Target==(Attack.Data.Targets)0x10 || moveData.Target==(Attack.Data.Targets)0x20)) {
            score+=60;
          } else if (moveData.Power!=0 && moveData.Target==0x00 &&
             pbTypeModifier(moveData.Type,opponent,attacker)==0) {
            score+=60;
          }
        }
      }
      break;
    case Attack.Data.Effects.x02D:
      break;
    case Attack.Data.Effects.x069:
      break;
    case Attack.Data.Effects.x169:
      break;
    case Attack.Data.Effects.x09B:
      break;
    case Attack.Data.Effects.x051:
      break;
    case Attack.Data.Effects.x028:
      break;
    case Attack.Data.Effects.x098:
      break;
    case Attack.Data.Effects.x04C:
      if (attacker.effects.FocusEnergy>0) score+=20;
      if (skill>=PBTrainerAI.highSkill) {
        if (!opponent.hasWorkingAbility(Abilities.INNER_FOCUS) &&
                     opponent.effects.Substitute==0) score+=20;
      }
      break;
    case Attack.Data.Effects.x09C:
      break;
    case Attack.Data.Effects.x101:
      break;
    case Attack.Data.Effects.x100:
      break;
    case Attack.Data.Effects.x108:
      break;
    case Attack.Data.Effects.x111:
      break;
    case Attack.Data.Effects.x138:
      break;
    case Attack.Data.Effects.x02B:
      if (opponent.effects.MultiTurn==0) score+=40;
      break;
    case Attack.Data.Effects.x106:
      if (opponent.effects.MultiTurn==0) score+=40;
      break;
    case Attack.Data.Effects.x0A0:
      break;
    case Attack.Data.Effects.x01C:
      break;
    case Attack.Data.Effects.x076:
      break;
    case Attack.Data.Effects.x01B:
      if (attacker.HP<=attacker.TotalHP/4) {
        score-=90 ;
      } else if (attacker.HP<=attacker.TotalHP/2) {
        score-=50 ;
      }
      break;
    case Attack.Data.Effects.x021: case Attack.Data.Effects.x0D7:
      if (attacker.HP==attacker.TotalHP) {
        score-=90;
      }
      else {
        score+=50;
        score-=(attacker.HP*100/attacker.TotalHP);
      }
      break;
    case Attack.Data.Effects.x0B4:
      if (attacker.effects.Wish>0) score-=90;
      break;
    case Attack.Data.Effects.x085:
      if (attacker.HP==attacker.TotalHP) {
        score-=90;
      }
      else {
        switch (pbWeather()) {
        case Weather.SUNNYDAY:
          score+=30;
          break;
        case Weather.RAINDANCE: case Weather.SANDSTORM: case Weather.HAIL:
          score-=30;
          break;
        }
        score+=50;
        score-=(attacker.HP*100/attacker.TotalHP);
      }
      break;
    case Attack.Data.Effects.x026:
      if (attacker.HP==attacker.TotalHP || !attacker.pbCanSleep(attacker,false,null,true)) {
        score-=90;
      }
      else {
        score+=70;
        score-=(attacker.HP*140/attacker.TotalHP);
        if (attacker.Status!=0) score+=30;
      }
      break;
    case Attack.Data.Effects.x0FC:
      if (attacker.effects.AquaRing) score-=90;
      break;
    case Attack.Data.Effects.x0B6:
      if (attacker.effects.Ingrain) score-=90;
      break;
    case Attack.Data.Effects.x055:
      if (opponent.effects.LeechSeed>=0) {
        score-=90;
      } else if (skill>=PBTrainerAI.mediumSkill && opponent.hasType(Types.GRASS)) {
        score-=90;
      }
      else {
        if (attacker.turncount==0) score+=60;
      }
      break;
    case Attack.Data.Effects.x15A:
      if (skill>=PBTrainerAI.highSkill && opponent.hasWorkingAbility(Abilities.LIQUID_OOZE)) {
        score-=70;
      }
      else {
        if (attacker.HP<=(attacker.TotalHP/2)) score+=20;
      }
      break;
    case Attack.Data.Effects.x009:
      if (opponent.Status!=Status.SLEEP) {
        score-=100;
      } else if (skill>=PBTrainerAI.highSkill && opponent.hasWorkingAbility(Abilities.LIQUID_OOZE)) {
        score-=70;
      }
      else {
        if (attacker.HP<=(attacker.TotalHP/2)) score+=20;
      }
      break;
    case Attack.Data.Effects.x136:
      if (attacker.IsOpposing(opponent.Index)) {
        score-=100;
      }
      else {
        if (opponent.HP<(opponent.TotalHP/2) &&
                     opponent.effects.Substitute==0) score+=20;
      }
      break;
    case Attack.Data.Effects.x008:
      int reserves=attacker.NonActivePokemonCount;
      int foes=attacker.pbOppositeOpposing.NonActivePokemonCount;
      if (pbCheckGlobalAbility(Abilities.DAMP).IsNotNullOrNone()) {
        score-=100;
      } else if (skill>=PBTrainerAI.mediumSkill && reserves==0 && foes>0) {
        score-=100; // don't want to lose
      } else if (skill>=PBTrainerAI.highSkill && reserves==0 && foes==0) {
        score-=100; // don't want to draw
      }
      else {
        score-=(attacker.HP*100/attacker.TotalHP);
      }
      break;
    case Attack.Data.Effects.x141:
      break;
    case Attack.Data.Effects.x0A9:
      if (!opponent.pbCanReduceStatStage(Stats.ATTACK,attacker) &&
         !opponent.pbCanReduceStatStage(Stats.SPATK,attacker)) {
        score-=100;
      } else if (attacker.NonActivePokemonCount==0) {
        score-=100 ;
      }
      else {
        score+=(opponent.stages[(int)Stats.ATTACK]*10);
        score+=(opponent.stages[(int)Stats.SPATK]*10);
        score-=(attacker.HP*100/attacker.TotalHP);
      }
      break;
    case Attack.Data.Effects.x0DD: case Attack.Data.Effects.x10F:
      score-=70;
      break;
    case Attack.Data.Effects.x073:
      if (attacker.NonActivePokemonCount==0) {
        score-=90;
      }
      else {
        if (opponent.effects.PerishSong>0) score-=90;
      }
      break;
    case Attack.Data.Effects.x0C3:
      score+=50;
      score-=(attacker.HP*100/attacker.TotalHP);
      if (attacker.HP<=(attacker.TotalHP/10)) score+=30;
      break;
    case Attack.Data.Effects.x063:
      score+=50;
      score-=(attacker.HP*100/attacker.TotalHP);
      if (attacker.HP<=(attacker.TotalHP/10)) score+=30;
      break;
    case Attack.Data.Effects.x075:
      if (attacker.HP>(attacker.TotalHP/2)) score-=25;
      if (skill>=PBTrainerAI.mediumSkill) {
        if (attacker.effects.ProtectRate>1) score-=90;
        if (opponent.effects.HyperBeam>0) score-=90;
      }
      else {
        score-=(attacker.effects.ProtectRate*40);
      }
      break;
    case Attack.Data.Effects.x066:
      if (opponent.HP==1) {
        score-=90;
      } else if (opponent.HP<=(opponent.TotalHP/8)) {
        score-=60;
      } else if (opponent.HP<=(opponent.TotalHP/4)) {
        score-=30;
      }
      break;
    case Attack.Data.Effects.x09A:
      if (@opponent.IsNotNullOrNone()) score-=100;
      break;
    case Attack.Data.Effects.x01D:
      if (opponent.effects.Ingrain ||
         (skill>=PBTrainerAI.highSkill && opponent.hasWorkingAbility(Abilities.SUCTION_CUPS))) {
        score-=90 ;
      }
      else {
        party=pbParty(opponent.Index);
        int ch=0;
        for (int i = 0; i < party.Length; i++) {
          if (pbCanSwitchLax(opponent.Index,i,false)) ch+=1;
        }
        if (ch==0) score-=90;
      }
      if (score>20) {
        if (opponent.OwnSide.Spikes>0) score+=50;
        if (opponent.OwnSide.ToxicSpikes>0) score+=50;
        if (opponent.OwnSide.StealthRock) score+=50;
      }
      break;
    case Attack.Data.Effects.x13A:
      if (!opponent.effects.Ingrain &&
         !(skill>=PBTrainerAI.highSkill && opponent.hasWorkingAbility(Abilities.SUCTION_CUPS))) {
        if (opponent.OwnSide.Spikes>0) score+=40;
        if (opponent.OwnSide.ToxicSpikes>0) score+=40;
        if (opponent.OwnSide.StealthRock) score+=40;
      }
      break;
    case Attack.Data.Effects.x080:
      if (!pbCanChooseNonActive(attacker.Index)) {
        score-=80;
      }
      else {
        if (attacker.effects.Confusion>0) score-=40;
        int total=0;
        total+=(attacker.stages[(int)Stats.ATTACK]*10);
        total+=(attacker.stages[(int)Stats.DEFENSE]*10);
        total+=(attacker.stages[(int)Stats.SPEED]*10);
        total+=(attacker.stages[(int)Stats.SPATK]*10);
        total+=(attacker.stages[(int)Stats.SPDEF]*10);
        total+=(attacker.stages[(int)Stats.EVASION]*10);
        total+=(attacker.stages[(int)Stats.ACCURACY]*10);
        if (total<=0 || attacker.turncount==0) {
          score-=60;
        }
        else {
          score+=total;
          // special case: attacker has no damaging moves
          hasDamagingMove=false;
          foreach (var m in attacker.moves) {
            if (move.MoveId!=0 && move.Power>0) {
              hasDamagingMove=true;
            }
          }
          if (!hasDamagingMove) {
            score+=75;
          }
        }
      }
      break;
    case Attack.Data.Effects.x0E5:
      break;
    case Attack.Data.Effects.x176:
      if (opponent.effects.MeanLook>=0) score-=90;
      break;
    case Attack.Data.Effects.x0BD:
      if (skill>=PBTrainerAI.highSkill) {
        if (opponent.Item!=0) score+=20;
      }
      break;
    case Attack.Data.Effects.x06A:
      if (skill>=PBTrainerAI.highSkill) {
        if (attacker.Item==0 && opponent.Item!=0) {
          score+=40;
        }
        else {
          score-=90;
        }
      }
      else {
        score-=80;
      }
      break;
    case Attack.Data.Effects.x0B2:
      if (attacker.Item==0 && opponent.Item==0) {
        score-=90;
      } else if (skill>=PBTrainerAI.highSkill && opponent.hasWorkingAbility(Abilities.STICKY_HOLD)) {
        score-=90;
      } else if (attacker.hasWorkingItem(Items.FLAME_ORB) ||
            attacker.hasWorkingItem(Items.TOXIC_ORB) ||
            attacker.hasWorkingItem(Items.STICKY_BARB) ||
            attacker.hasWorkingItem(Items.IRON_BALL) ||
            attacker.hasWorkingItem(Items.CHOICE_BAND) ||
            attacker.hasWorkingItem(Items.CHOICE_SCARF) ||
            attacker.hasWorkingItem(Items.CHOICE_SPECS)) {
        score+=50;
      } else if (attacker.Item==0 && opponent.Item!=0) {
        if (Game.MoveData[attacker.lastMoveUsed].Effect==Attack.Data.Effects.x0B2) score-=30;	// Trick/Switcheroo
      }
      break;
    case Attack.Data.Effects.x144:
      if (attacker.Item==0 || opponent.Item!=0) {
        score-=90;
      }
      else {
        if (attacker.hasWorkingItem(Items.FLAME_ORB) ||
           attacker.hasWorkingItem(Items.TOXIC_ORB) ||
           attacker.hasWorkingItem(Items.STICKY_BARB) ||
           attacker.hasWorkingItem(Items.IRON_BALL) ||
           attacker.hasWorkingItem(Items.CHOICE_BAND) ||
           attacker.hasWorkingItem(Items.CHOICE_SCARF) ||
           attacker.hasWorkingItem(Items.CHOICE_SPECS)) {
          score+=50;
        }
        else {
          score-=80;
        }
      }
      break;
    case Attack.Data.Effects.x0E1: case Attack.Data.Effects.x13B:
      if (opponent.effects.Substitute==0) {
        if (skill>=PBTrainerAI.highSkill && ItemData.pbIsBerry(opponent.Item)) { //PokemonUnity.Inventory.Item.pbIsBerry(opponent.Item)
          score+=30;
        }
      }
      break;
    case Attack.Data.Effects.x0B9:
      if (attacker.pokemon.itemRecycle==0 || attacker.Item!=0) {
        score-=80;
      } else if (attacker.pokemon.itemRecycle!=0) {
        score+=30;
      }
      break;
    case Attack.Data.Effects.x0EA:
      if (attacker.Item==0 ||
         pbIsUnlosableItem(attacker,attacker.Item) ||
         //Inventory.Item.pbIsPokeBall(attacker.Item) ||
         ItemData.pbIsPokeBall(attacker.Item) ||
         attacker.hasWorkingAbility(Abilities.KLUTZ) ||
         attacker.effects.Embargo>0) {
        score-=90;
      }
      break;
    case Attack.Data.Effects.x0E9:
      if (opponent.effects.Embargo>0) score-=90;
      break;
    case Attack.Data.Effects.x11F:
      if (@field.MagicRoom>0) {
        score-=90;
      }
      else {
        if (attacker.Item==0 && opponent.Item!=0) score+=30;
      }
      break;
    case Attack.Data.Effects.x031:
      score-=25;
      break;
    case Attack.Data.Effects.x0C7:
      score-=30;
      break;
    case Attack.Data.Effects.x10E:
      score-=40;
      break;
    case Attack.Data.Effects.x107:
      score-=30;
      if (opponent.pbCanParalyze(attacker,false)) {
        score+=30;
        if (skill>=PBTrainerAI.mediumSkill) {
           aspeed=pbRoughStat(attacker,Stats.SPEED,skill);
           ospeed=pbRoughStat(opponent,Stats.SPEED,skill);
          if (aspeed<ospeed) {
            score+=30;
          } else if (aspeed>ospeed) {
            score-=40;
          }
        }
        if (skill>=PBTrainerAI.highSkill) {
          if (opponent.hasWorkingAbility(Abilities.GUTS)) score-=40;
          if (opponent.hasWorkingAbility(Abilities.MARVEL_SCALE)) score-=40;
          if (opponent.hasWorkingAbility(Abilities.QUICK_FEET)) score-=40;
        }
      }
      break;
    case Attack.Data.Effects.x0FE:
      score-=30;
      if (opponent.pbCanBurn(attacker,false)) {
        score+=30;
        if (skill>=PBTrainerAI.highSkill) {
          if (opponent.hasWorkingAbility(Abilities.GUTS)) score-=40;
          if (opponent.hasWorkingAbility(Abilities.MARVEL_SCALE)) score-=40;
          if (opponent.hasWorkingAbility(Abilities.QUICK_FEET)) score-=40;
          if (opponent.hasWorkingAbility(Abilities.FLARE_BOOST)) score-=40;
        }
      }
      break;
    case Attack.Data.Effects.x08A:
      if (pbCheckGlobalAbility(Abilities.AIR_LOCK).IsNotNullOrNone() ||
         pbCheckGlobalAbility(Abilities.CLOUD_NINE).IsNotNullOrNone()) {
        score-=90;
      } else if (pbWeather()==Weather.SUNNYDAY) {
        score-=90;
      }
      else {
        foreach (Attack.Move m in attacker.moves) {
          if (m.MoveId!=0 && m.Power>0 &&
             m.Type == Types.FIRE) {
            score+=20;
          }
        }
      }
      break;
    case Attack.Data.Effects.x089:
      if (pbCheckGlobalAbility(Abilities.AIR_LOCK).IsNotNullOrNone() ||
         pbCheckGlobalAbility(Abilities.CLOUD_NINE).IsNotNullOrNone()) {
        score-=90;
      } else if (pbWeather()==Weather.RAINDANCE) {
        score-=90;
      }
      else {
        foreach (Attack.Move m in attacker.moves) {
          if (m.MoveId!=0 && m.Power>0 &&
             m.Type == Types.WATER) {
            score+=20;
          }
        }
      }
      break;
    case Attack.Data.Effects.x074:
      if (pbCheckGlobalAbility(Abilities.AIR_LOCK).IsNotNullOrNone() ||
         pbCheckGlobalAbility(Abilities.CLOUD_NINE).IsNotNullOrNone()) {
        score-=90;
      } else if (pbWeather()==Weather.SANDSTORM) {
        score-=90;
      }
      break;
    case Attack.Data.Effects.x0A5:
      if (pbCheckGlobalAbility(Abilities.AIR_LOCK).IsNotNullOrNone() ||
         pbCheckGlobalAbility(Abilities.CLOUD_NINE).IsNotNullOrNone()) {
        score-=90;
      } else if (pbWeather()==Weather.HAIL) {
        score-=90;
      }
      break;
    case Attack.Data.Effects.x071:
      if (attacker.OpposingSide.Spikes>=3) {
        score-=90;
      } else if (!pbCanChooseNonActive(attacker.pbOpposing1.Index) &&
            !pbCanChooseNonActive(attacker.pbOpposing2.Index)) {
        // Opponent can't switch in any Pokemon
        score-=90;
      }
      else {
        score+=5*attacker.pbOppositeOpposing.NonActivePokemonCount;
        score+=new int[] { 40, 26, 13 }[attacker.OpposingSide.Spikes];
      }
      break;
    case Attack.Data.Effects.x0FA:
      if (attacker.OpposingSide.ToxicSpikes>=2) {
        score-=90;
      } else if (!pbCanChooseNonActive(attacker.pbOpposing1.Index) &&
            !pbCanChooseNonActive(attacker.pbOpposing2.Index)) {
        // Opponent can't switch in any Pokemon
        score-=90;
      }
      else {
        score+=4*attacker.pbOppositeOpposing.NonActivePokemonCount;
        score+=new int[] { 26, 13 }[attacker.OpposingSide.ToxicSpikes];
      }
      break;
    case Attack.Data.Effects.x10B:
      if (attacker.OpposingSide.StealthRock) {
        score-=90;
      } else if (!pbCanChooseNonActive(attacker.pbOpposing1.Index) &&
            !pbCanChooseNonActive(attacker.pbOpposing2.Index)) {
        // Opponent can't switch in any Pokemon
        score-=90;
      }
      else {
        score+=5*attacker.pbOppositeOpposing.NonActivePokemonCount;
      }
      break;
    case Attack.Data.Effects.x147:
      break;
    case Attack.Data.Effects.x146:
      break;
    case Attack.Data.Effects.x145:
      break;
    case Attack.Data.Effects.x023:
      break;
    case Attack.Data.Effects.x0BB:
      if (attacker.OpposingSide.Reflect>0) score+=20;
      if (attacker.OpposingSide.LightScreen>0) score+=20;
      break;
    case Attack.Data.Effects.x02E:
      score+=10*(attacker.stages[(int)Stats.ACCURACY]-opponent.stages[(int)Stats.EVASION]);
      break;
    case Attack.Data.Effects.x050:
      if (attacker.effects.Substitute>0) {
        score-=90;
      } else if (attacker.HP<=(attacker.TotalHP/4)) {
        score-=90;
      }
      break;
    case Attack.Data.Effects.x06E:
      if (attacker.hasType(Types.GHOST)) {
        if (opponent.effects.Curse) {
          score-=90;
        } else if (attacker.HP<=(attacker.TotalHP/2)) {
          if (attacker.NonActivePokemonCount==0) {
            score-=90;
          }
          else {
            score-=50;
            if (@shiftStyle) score-=30;
          }
        }
      }
      else {
        avg=(attacker.stages[(int)Stats.SPEED]*10);
        avg-=(attacker.stages[(int)Stats.ATTACK]*10);
        avg-=(attacker.stages[(int)Stats.DEFENSE]*10);
        score+=avg/3;
      }
      break;
    case Attack.Data.Effects.x065:
      score-=40;
      break;
    case Attack.Data.Effects.x06C:
      if (opponent.effects.Nightmare ||
         opponent.effects.Substitute>0) {
        score-=90;
      } else if (opponent.Status!=Status.SLEEP) {
        score-=90;
      }
      else {
        if (opponent.StatusCount<=1) score-=90;
        if (opponent.StatusCount>3) score+=50;
      }
      break;
    case Attack.Data.Effects.x082:
      if (attacker.effects.MultiTurn>0) score+=30;
      if (attacker.effects.LeechSeed>=0) score+=30;
      if (attacker.NonActivePokemonCount>0) {
        if (attacker.OwnSide.Spikes>0) score+=80;
        if (attacker.OwnSide.ToxicSpikes>0) score+=80;
        if (attacker.OwnSide.StealthRock) score+=80;
      }
      break;
    case Attack.Data.Effects.x095:
      if (opponent.effects.FutureSight>0) {
        score-=100;
      } else if (attacker.NonActivePokemonCount==0) {
        // Future Sight tends to be wasteful if down to last Pokemon
        score-=70;
      }
      break;
    case Attack.Data.Effects.x0A1:
      avg=0;
      avg-=(attacker.stages[(int)Stats.DEFENSE]*10);
      avg-=(attacker.stages[(int)Stats.SPDEF]*10);
      score+=avg/2;
      if (attacker.effects.Stockpile>=3) {
        score-=80;
      }
      else {
        // More preferable if user also has Spit Up/Swallow
        foreach (Attack.Move m in attacker.moves) {
          if (m.Effect==Attack.Data.Effects.x0A2 || m.Effect==Attack.Data.Effects.x0A3) {		// Spit Up, Swallow
            score+=20; break;
          }
        }
      }
      break;
    case Attack.Data.Effects.x0A2:
      if (attacker.effects.Stockpile==0) score-=100;
      break;
    case Attack.Data.Effects.x0A3:
      if (attacker.effects.Stockpile==0) {
        score-=90;
      } else if (attacker.HP==attacker.TotalHP) {
        score-=90;
      }
      else {
        int mult=new int[] {0,25,50,100}[attacker.effects.Stockpile];
        score+=mult;
        score-=(attacker.HP*mult*2/attacker.TotalHP);
      }
      break;
    case Attack.Data.Effects.x0AB:
      if (opponent.effects.HyperBeam>0) score+=50;
      if (opponent.HP<=(opponent.TotalHP/2)) score-=35;	// If opponent is weak, no
      if (opponent.HP<=(opponent.TotalHP/4)) score-=70;	// need to risk this move
      break;
    case Attack.Data.Effects.x0F9:
      break;
    case Attack.Data.Effects.x0AD:
      if (!@doublebattle) {
        score-=100;
      } else if (attacker.Partner.isFainted()) {
        score-=90;
      }
      break;
    case Attack.Data.Effects.x0D8:
      if (@field.Gravity>0) {
        score-=90;
      } else if (skill>=PBTrainerAI.mediumSkill) {
        score-=30;
        if (attacker.effects.SkyDrop) score-=20;
        if (attacker.effects.MagnetRise>0) score-=20;
        if (attacker.effects.Telekinesis>0) score-=20;
        if (attacker.hasType(Types.FLYING)) score-=20;
        if (attacker.hasWorkingAbility(Abilities.LEVITATE)) score-=20;
        if (attacker.hasWorkingItem(Items.AIR_BALLOON)) score-=20;
        if (opponent.effects.SkyDrop) score+=20;
        if (opponent.effects.MagnetRise>0) score+=20;
        if (opponent.effects.Telekinesis>0) score+=20;
        if (Game.MoveData[opponent.effects.TwoTurnAttack].Effect==Attack.Data.Effects.x09C ||	// Fly
                     Game.MoveData[opponent.effects.TwoTurnAttack].Effect==Attack.Data.Effects.x108 || // Bounce
                     Game.MoveData[opponent.effects.TwoTurnAttack].Effect==Attack.Data.Effects.x138) score+=20;    // Sky Drop
        if (opponent.hasType(Types.FLYING)) score+=20;
        if (opponent.hasWorkingAbility(Abilities.LEVITATE)) score+=20;
        if (opponent.hasWorkingItem(Items.AIR_BALLOON)) score+=20;
      }
      break;
    case Attack.Data.Effects.x0FD:
      if (attacker.effects.MagnetRise>0 ||
         attacker.effects.Ingrain ||
         attacker.effects.SmackDown) {
        score-=90;
      }
      break;
    case Attack.Data.Effects.x11E:
      if (opponent.effects.Telekinesis>0 ||
         opponent.effects.Ingrain ||
         opponent.effects.SmackDown) {
        score-=90;
      }
      break;
    case Attack.Data.Effects.x0D0:
      break;
    case Attack.Data.Effects.x120:
      if (skill>=PBTrainerAI.mediumSkill) {
        if (opponent.effects.MagnetRise>0) score+=20;
        if (opponent.effects.Telekinesis>0) score+=20;
        if (Game.MoveData[opponent.effects.TwoTurnAttack].Effect==Attack.Data.Effects.x09C ||	// Fly
                     Game.MoveData[opponent.effects.TwoTurnAttack].Effect==Attack.Data.Effects.x108) score+=20;    // Bounce
        if (opponent.hasType(Types.FLYING)) score+=20;
        if (opponent.hasWorkingAbility(Abilities.LEVITATE)) score+=20;
        if (opponent.hasWorkingItem(Items.AIR_BALLOON)) score+=20;
      }
      break;
    case Attack.Data.Effects.x12D:
      break;
    case Attack.Data.Effects.x13C:
      break;
    case Attack.Data.Effects.x104:
      break;
    case Attack.Data.Effects.x134:
      break;
    case Attack.Data.Effects.x12A:
      break;
    case Attack.Data.Effects.x11B:
      break;
    case Attack.Data.Effects.x125:
      if (!opponent.hasType(attacker.Type1) &&
         !opponent.hasType(attacker.Type2)) {
        score-=90;
      }
      break;
    case Attack.Data.Effects.x11A:
      break;
    case Attack.Data.Effects.x0F7:
      break;
    //case Attack.Data.Effects.x711:
    //  score+=20; // Shadow moves are more preferable
    //  break;
    //case 0x127:
    //  score+=20; // Shadow moves are more preferable
    //  if (opponent.pbCanParalyze(attacker,false)) {
    //    score+=30;
    //    if (skill>=PBTrainerAI.mediumSkill) {
    //       int aspeed=pbRoughStat(attacker,Stats.SPEED,skill);
    //       int ospeed=pbRoughStat(opponent,Stats.SPEED,skill);
    //      if (aspeed<ospeed) {
    //        score+=30;
    //      } else if (aspeed>ospeed) {
    //        score-=40;
    //      }
    //    }
    //    if (skill>=PBTrainerAI.highSkill) {
    //      if (opponent.hasWorkingAbility(Abilities.GUTS)) score-=40;
    //      if (opponent.hasWorkingAbility(Abilities.MARVEL_SCALE)) score-=40;
    //      if (opponent.hasWorkingAbility(Abilities.QUICK_FEET)) score-=40;
    //    }
    //  }
    //  break;
    //case 0x128:
    //  score+=20; // Shadow moves are more preferable
    //  if (opponent.pbCanBurn(attacker,false)) {
    //    score+=30;
    //    if (skill>=PBTrainerAI.highSkill) {
    //      if (opponent.hasWorkingAbility(Abilities.GUTS)) score-=40;
    //      if (opponent.hasWorkingAbility(Abilities.MARVEL_SCALE)) score-=40;
    //      if (opponent.hasWorkingAbility(Abilities.QUICK_FEET)) score-=40;
    //      if (opponent.hasWorkingAbility(Abilities.FLARE_BOOST)) score-=40;
    //    }
    //  }
    //  break;
    //case 0x129:
    //  score+=20; // Shadow moves are more preferable
    //  if (opponent.pbCanFreeze(attacker,false)) {
    //    score+=30;
    //    if (skill>=PBTrainerAI.highSkill) {
    //      if (opponent.hasWorkingAbility(Abilities.MARVEL_SCALE)) score-=20;
    //    }
    //  }
    //  break;
    //case Attack.Data.Effects.x032:
    //  score+=20; // Shadow moves are more preferable
    //  if (opponent.pbCanConfuse(attacker,false)) {
    //    score+=30;
    //  }
    //  else {
    //    if (skill>=PBTrainerAI.mediumSkill) {
    //      score-=90;
    //    }
    //  }
    //  break;
    //case 0x12B:
    //  score+=20; // Shadow moves are more preferable
    //  if (!opponent.pbCanReduceStatStage(Stats.DEFENSE,attacker)) {
    //    score-=90;
    //  }
    //  else {
    //    if (attacker.turncount==0) score+=40;
    //    score+=opponent.stages[(int)Stats.DEFENSE]*20;
    //  }
    //  break;
    //case Attack.Data.Effects.x714:
    //  score+=20; // Shadow moves are more preferable
    //  if (!opponent.pbCanReduceStatStage(Stats.EVASION,attacker)) {
    //    score-=90;
    //  }
    //  else {
    //    score+=opponent.stages[(int)Stats.EVASION]*15;
    //  }
    //  break;
    //case 0x12D:
    //  score+=20; // Shadow moves are more preferable
    //  break;
    //case Attack.Data.Effects.x713:
    //  score+=20; // Shadow moves are more preferable
    //  if (opponent.HP>=(opponent.TotalHP/2)) score+=20;
    //  if (attacker.HP<(attacker.HP/2)) score-=20;
    //  break;
    //case 0x12F:
    //  score+=20; // Shadow moves are more preferable
    //  if (opponent.effects.MeanLook>=0) score-=110;
    //  break;
    //case Attack.Data.Effects.x712:
    //  score+=20; // Shadow moves are more preferable
    //  score-=40;
    //  break;
    //case Attack.Data.Effects.x716:
    //  score+=20; // Shadow moves are more preferable
    //  if (pbCheckGlobalAbility(Abilities.AIR_LOCK).IsNotNullOrNone() ||
    //     pbCheckGlobalAbility(Abilities.CLOUD_NINE).IsNotNullOrNone()) {
    //    score-=90;
    //  } else if (pbWeather()==Weather.SHADOWSKY) {
    //    score-=90;
    //  }
    //  break;
    //case Attack.Data.Effects.x715:
    //  score+=20; // Shadow moves are more preferable
    //  if (opponent.OwnSide.Reflect>0 ||
    //     opponent.OwnSide.LightScreen>0 ||
    //     opponent.OwnSide.Safeguard>0) {
    //    score+=30;
    //    if (attacker.OwnSide.Reflect>0 ||
    //                 attacker.OwnSide.LightScreen>0 ||
    //                 attacker.OwnSide.Safeguard>0) score-=90;
    //  }
    //  else {
    //    score-=110;
    //  }
    //  break;
    //case 0x133: 
    case Attack.Data.Effects.x172:
      score-=95;
      if (skill>=PBTrainerAI.highSkill) {
        score=0;
      }
      break;
    case Attack.Data.Effects.x17C:
      if (opponent.pbCanFreeze(attacker,false)) {
        score+=30;
        if (skill>=PBTrainerAI.highSkill) {
          if (opponent.hasWorkingAbility(Abilities.MARVEL_SCALE)) score-=20;
        }
      }
      break;
    case Attack.Data.Effects.x167:
      if (attacker.stages[(int)Stats.DEFENSE]<0) score+=20;
      break;
    case Attack.Data.Effects.x16F:
      if (attacker.pbTooHigh(Stats.DEFENSE) &&
         attacker.pbTooHigh(Stats.SPDEF) &&
         !attacker.Partner.isFainted() &&
         attacker.Partner.pbTooHigh(Stats.DEFENSE) &&
         attacker.Partner.pbTooHigh(Stats.SPDEF)) {
        score-=90;
      }
      else {
        score-=attacker.stages[(int)Stats.DEFENSE]*10;
        score-=attacker.stages[(int)Stats.SPDEF]*10;
        if (!attacker.Partner.isFainted()) {
          score-=attacker.Partner.stages[(int)Stats.DEFENSE]*10;
          score-=attacker.Partner.stages[(int)Stats.SPDEF]*10;
        }
      }
      break;
    case Attack.Data.Effects.x16B:
      if (!@doublebattle) {
        score-=100;
      } else if (attacker.Partner.isFainted()) {
        score-=90;
      }
      else {
        score-=attacker.Partner.stages[(int)Stats.SPDEF]*10;
      }
      break;
    case Attack.Data.Effects.x165:
      if (!opponent.pbCanReduceStatStage(Stats.ATTACK,attacker)) {
        score-=90;
      }
      else {
        score+=opponent.stages[(int)Stats.ATTACK]*20;
        if (skill>=PBTrainerAI.mediumSkill) {
          hasphysicalattack=false;
          foreach (var thismove in opponent.moves) {
            if (thismove.MoveId!=0 && thismove.Power>0 &&
               thismove.pbIsPhysical(thismove.Type)) {
              hasphysicalattack=true;
            }
          }
          if (hasphysicalattack) {
            score+=20;
          } else if (skill>=PBTrainerAI.highSkill) {
            score-=90;
          }
        }
      }
      break;
    case Attack.Data.Effects.x158:
      avg=opponent.stages[(int)Stats.ATTACK]*10;
      avg+=opponent.stages[(int)Stats.SPATK]*10;
      score+=avg/2;
      break;
    //case 0x13B:
    //  if (attacker.Species != Pokemons.HOOPA || attacker.form!=1) {
    //    score-=100;
    //  }
    //  else {
    //    if (opponent.stages[(int)Stats.DEFENSE]>0) score+=20;
    //  }
    //  break;
    case Attack.Data.Effects.x166:
      if (opponent.stages[(int)Stats.SPATK]>0) score+=20;
      break;
    case Attack.Data.Effects.x03E:
      if (!opponent.pbCanReduceStatStage(Stats.SPATK,attacker)) {
        score-=90;
      }
      else {
        if (attacker.turncount==0) score+=40;
        score+=opponent.stages[(int)Stats.SPATK]*20;
      }
      break;
    case Attack.Data.Effects.x154:
      count=0;
      for (int i = 0; i < 4; i++) {
        Pokemon battler=@battlers[i];
        if (battler.hasType(Types.GRASS) && !battler.isAirborne() &&
           (!battler.pbTooHigh(Stats.ATTACK) || !battler.pbTooHigh(Stats.SPATK))) {
          count+=1;
          if (attacker.IsOpposing(battler.Index)) {
            score-=20;
          }
          else {
            score-=attacker.stages[(int)Stats.ATTACK]*10;
            score-=attacker.stages[(int)Stats.SPATK]*10;
          }
        }
      }
      if (count==0) score-=95;
      break;
    case Attack.Data.Effects.x15F:
      count=0;
      for (int i = 0; i < 4; i++) {
        Pokemon battler=@battlers[i];
        if (battler.hasType(Types.GRASS) && !battler.pbTooHigh(Stats.DEFENSE)) {
          count+=1;
          if (attacker.IsOpposing(battler.Index)) {
            score-=20;
          }
          else {
            score-=attacker.stages[(int)Stats.DEFENSE]*10;
          }
        }
      }
      if (count==0) score-=95;
      break;
    case Attack.Data.Effects.x16C:
      count=0;
      for (int i = 0; i < 4; i++) {
        Pokemon battler=@battlers[i];
        if (battler.Status==Status.POISON &&
           (!battler.pbTooLow(Stats.ATTACK) ||
           !battler.pbTooLow(Stats.SPATK) ||
           !battler.pbTooLow(Stats.SPEED))) {
          count+=1;
          if (attacker.IsOpposing(battler.Index)) {
            score+=attacker.stages[(int)Stats.ATTACK]*10;
            score+=attacker.stages[(int)Stats.SPATK]*10;
            score+=attacker.stages[(int)Stats.SPEED]*10;
          }
          else {
            score-=20;
          }
        }
      }
      if (count==0) score-=95;
      break;
    case Attack.Data.Effects.x15C:
      if (opponent.effects.Substitute>0) {
        score-=90;
      }
      else {
        int numpos=0; int numneg=0;
        foreach (var i in new Stats[] { Stats.ATTACK,Stats.DEFENSE,Stats.SPEED,
                  Stats.SPATK,Stats.SPDEF,Stats.ACCURACY,Stats.EVASION }) {
          int stat=opponent.stages[(int)i];
          if (stat>0) numpos+=stat; else numneg+=stat;
        }
        if (numpos!=0 || numneg!=0) {
          score+=(numpos-numneg)*10;
        }
        else {
          score-=95;
        }
      }
      break;
    case Attack.Data.Effects.x157:
      if (opponent.hasType(Types.GHOST)) {
        score-=90;
      }
      break;
    case Attack.Data.Effects.x178:
      if (opponent.hasType(Types.GRASS)) {
        score-=90;
      }
      break;
    case Attack.Data.Effects.x152:
      break;
    case Attack.Data.Effects.x162:
      aspeed=pbRoughStat(attacker,Stats.SPEED,skill);
      ospeed=pbRoughStat(opponent,Stats.SPEED,skill);
      if (aspeed>ospeed) {
        score-=90;
      }
      break;
    case Attack.Data.Effects.x159:
      break;
    //case 0x147:
    //  break;
    case Attack.Data.Effects.x17A:
      aspeed=pbRoughStat(attacker,Stats.SPEED,skill);
      ospeed=pbRoughStat(opponent,Stats.SPEED,skill);
      if (aspeed>ospeed) {
        score-=90;
      }
      else {
        if (opponent.HasMoveType(Types.FIRE)) score+=30;
      }
      break;
    case Attack.Data.Effects.x179:
      if (attacker.turncount==0) {
        score+=30;
      }
      else {
        score-=90; // Because it will fail here
        if (skill>=PBTrainerAI.bestSkill) score=0;
      }
      break;
    case Attack.Data.Effects.x15E:
      break;
    case Attack.Data.Effects.x164: case Attack.Data.Effects.x16A:
      if (attacker.effects.ProtectRate>1 ||
         opponent.effects.HyperBeam>0) {
        score-=90;
      }
      else {
        if (skill>=PBTrainerAI.mediumSkill) {
          score-=(attacker.effects.ProtectRate*40);
        }
        if (attacker.turncount==0) score+=50;
        if (opponent.effects.TwoTurnAttack!=0) score+=30;
      }
      break;
    //case 0x14D:
    //  break;
    case Attack.Data.Effects.x16E:
      if (attacker.pbTooHigh(Stats.SPATK) &&
         attacker.pbTooHigh(Stats.SPDEF) &&
         attacker.pbTooHigh(Stats.SPEED)) {
        score-=90;
      }
      else {
        score-=attacker.stages[(int)Stats.SPATK]*10; // Only *10 isntead of *20
        score-=attacker.stages[(int)Stats.SPDEF]*10; // because two-turn attack
        score-=attacker.stages[(int)Stats.SPEED]*10;
        if (skill>=PBTrainerAI.mediumSkill) {
          hasspecialattack=false;
          foreach (var thismove in attacker.moves) {
            if (thismove.MoveId!=0 && thismove.Power>0 &&
               thismove.pbIsSpecial(thismove.Type)) {
              hasspecialattack=true;
            }
          }
          if (hasspecialattack) {
            score+=20;
          } else if (skill>=PBTrainerAI.highSkill) {
            score-=90;
          }
        }
        if (skill>=PBTrainerAI.highSkill) {
          aspeed=pbRoughStat(attacker,Stats.SPEED,skill);
          ospeed=pbRoughStat(opponent,Stats.SPEED,skill);
          if (aspeed<ospeed && aspeed*2>ospeed) {
            score+=30;
          }
        }
      }
      break;
    case Attack.Data.Effects.x15D:
      if (skill>=PBTrainerAI.highSkill && opponent.hasWorkingAbility(Abilities.LIQUID_OOZE)) {
        score-=80;
      }
      else {
        if (attacker.HP<=(attacker.TotalHP/2)) score+=40;
      }
      break;
    case Attack.Data.Effects.x156:
      if (!attacker.pbTooHigh(Stats.ATTACK) && opponent.HP<=(opponent.TotalHP/4)) score+=20;
      break;
    case Attack.Data.Effects.x15B:
      avg=opponent.stages[(int)Stats.ATTACK]*10;
      avg+=opponent.stages[(int)Stats.SPATK]*10;
      score+=avg/2;
      break;
    case Attack.Data.Effects.x163:
      break;
    case Attack.Data.Effects.x155:
      if (opponent.OwnSide.StickyWeb) score-=95;
      break;
    case Attack.Data.Effects.x171:
      break;
    case Attack.Data.Effects.x160:
      break;
    case Attack.Data.Effects.x161:
      break;
    case Attack.Data.Effects.x170:
      score-=90;
      break;
    case Attack.Data.Effects.x153:
      if (!attacker.pokemon.IsNotNullOrNone() || !attacker.pokemon.belch) score-=90;
      break;
    }
    // A score of 0 here means it should absolutely not be used
    if (score<=0) return (int)score;
#endregion
#region Other score modifications ###############################################
    // Prefer damaging moves if AI has no more Pokémon
    if (attacker.NonActivePokemonCount==0) {
      if (skill>=PBTrainerAI.mediumSkill &&
         !(skill>=PBTrainerAI.highSkill && opponent.NonActivePokemonCount>0)) {
        if (move.Power==0) {
          score/=1.5f;
        } else if (opponent.HP<=opponent.TotalHP/2) {
          score*=1.5f;
        }
      }
    }
    // Don't prefer attacking the opponent if they'd be semi-invulnerable
    if (opponent.effects.TwoTurnAttack>0 &&
       skill>=PBTrainerAI.highSkill) {
      Attack.Data.Effects invulmove=Game.MoveData[opponent.effects.TwoTurnAttack].Effect;
      if (move.Accuracy>0 &&  		// Checks accuracy, i.e. targets opponent
         (new Attack.Data.Effects[] { Attack.Data.Effects.x09C,Attack.Data.Effects.x101,Attack.Data.Effects.x100,Attack.Data.Effects.x108,Attack.Data.Effects.x111,Attack.Data.Effects.x138 }.Contains(invulmove) ||
         opponent.effects.SkyDrop) &&
         attacker.SPE>opponent.SPE) {
        if (skill>=PBTrainerAI.bestSkill) {		// Can get past semi-invulnerability
          bool miss=false;
          switch (invulmove) {
          case Attack.Data.Effects.x09C: case Attack.Data.Effects.x108: // Fly: Bounce
            if (move.Effect!=Attack.Data.Effects.x099 || 	// Thunder
                             move.Effect!=Attack.Data.Effects.x14E ||  // Hurricane
                             move.Effect!=Attack.Data.Effects.x096 ||  // Gust
                             move.Effect!=Attack.Data.Effects.x093 ||  // Twister
                             move.Effect!=Attack.Data.Effects.x0D0 || // Sky Uppercut
                             move.Effect!=Attack.Data.Effects.x120 || // Smack Down
                             move.MoveId!=Moves.WHIRLWIND) miss=true;
            break;
          case Attack.Data.Effects.x101: // Dig
            if (move.Effect!=Attack.Data.Effects.x094 ||	// Earthquake
                             move.Effect!=Attack.Data.Effects.x07F) miss=true;    // Magnitude
            break;
          case Attack.Data.Effects.x100: // Dive
            if (move.Effect!=Attack.Data.Effects.x102 ||	// Surf
                             move.Effect!=Attack.Data.Effects.x106) miss=true;    // Whirlpool
            break;
          case Attack.Data.Effects.x111: // Shadow Force
            miss=true;
            break;
          case Attack.Data.Effects.x138: // Sky Drop
            if (move.Effect!=Attack.Data.Effects.x099 || 	// Thunder
                             move.Effect!=Attack.Data.Effects.x14E ||  // Hurricane
                             move.Effect!=Attack.Data.Effects.x096 ||  // Gust
                             move.Effect!=Attack.Data.Effects.x093 ||  // Twister
                             move.Effect!=Attack.Data.Effects.x0D0 || // Sky Uppercut
                             move.Effect!=Attack.Data.Effects.x120) miss=true;    // Smack Down
            break;
          //case 0x14D: // Phantom Force
          //  miss=true;
          //  break;
          }
          if (opponent.effects.SkyDrop) {
            if (move.Effect!=Attack.Data.Effects.x099 || 	// Thunder
                             move.Effect!=Attack.Data.Effects.x14E ||  // Hurricane
                             move.Effect!=Attack.Data.Effects.x096 ||  // Gust
                             move.Effect!=Attack.Data.Effects.x093 ||  // Twister
                             move.Effect!=Attack.Data.Effects.x0D0 || // Sky Uppercut
                             move.Effect!=Attack.Data.Effects.x120) miss=true;    // Smack Down
          }
          if (miss) score-=80;
        }
        else {
          score-=80;
        }
      }
    }
    // Pick a good move for the Choice items
    if (attacker.hasWorkingItem(Items.CHOICE_BAND) ||
       attacker.hasWorkingItem(Items.CHOICE_SPECS) ||
       attacker.hasWorkingItem(Items.CHOICE_SCARF)) {
      if (skill>=PBTrainerAI.mediumSkill) {
        if (move.Power>=60) {
          score+=60;
        } else if (move.Power>0) {
          score+=30;
        } else if (move.Effect==Attack.Data.Effects.x0B2) {		// Trick
          score+=70;
        }
        else {
          score-=60;
        }
      }
    }
    // If user has King's Rock, prefer moves that may cause flinching with it // TODO
    // If user is asleep, prefer moves that are usable while asleep
    if (attacker.Status==Status.SLEEP) {
      if (skill>=PBTrainerAI.mediumSkill) {
        if (move.Effect!=Attack.Data.Effects.x05D && move.Effect!=Attack.Data.Effects.x062) {		// Snore, Sleep Talk
          bool hasSleepMove=false;
          foreach (var m in attacker.moves) {
            if (m.Effect==Attack.Data.Effects.x05D || m.Effect==Attack.Data.Effects.x062) {		// Snore, Sleep Talk
              hasSleepMove=true; break;
            }
          }
          if (hasSleepMove) score-=60;
        }
      }
    }
    // If user is frozen, prefer a move that can thaw the user
    if (attacker.Status==Status.FROZEN) {
      if (skill>=PBTrainerAI.mediumSkill) {
        if (move.Flag.Defrost) { //.canThawUser()
          score+=40;
        }
        else {
          bool hasFreezeMove=false;
          foreach (var m in attacker.moves) {
            if (m.Flag.Defrost) { //.canThawUser
              hasFreezeMove=true; break;
            }
          }
          if (hasFreezeMove) score-=60;
        }
      }
    }
    // If target is frozen, don't prefer moves that could thaw them // TODO
#endregion
    // Adjust score based on how much damage it can deal
    if (move.Power>0) {
      float typemod=pbTypeModifier(move.Type,attacker,opponent);
      if (typemod==0 || score<=0) {
        score=0;
      } else if (skill>=PBTrainerAI.mediumSkill && typemod<=8 &&
            opponent.hasWorkingAbility(Abilities.WONDER_GUARD)) {
        score=0;
      } else if (skill>=PBTrainerAI.mediumSkill && move.Type == Types.GROUND &&
            (opponent.hasWorkingAbility(Abilities.LEVITATE) ||
            opponent.effects.MagnetRise>0)) {
        score=0;
      } else if (skill>=PBTrainerAI.mediumSkill && move.Type == Types.FIRE &&
            opponent.hasWorkingAbility(Abilities.FLASH_FIRE)) {
        score=0;
      } else if (skill>=PBTrainerAI.mediumSkill && move.Type == Types.WATER &&
            (opponent.hasWorkingAbility(Abilities.WATER_ABSORB) ||
            opponent.hasWorkingAbility(Abilities.STORM_DRAIN) ||
            opponent.hasWorkingAbility(Abilities.DRY_SKIN))) {
        score=0;
      } else if (skill>=PBTrainerAI.mediumSkill && move.Type == Types.GRASS &&
            opponent.hasWorkingAbility(Abilities.SAP_SIPPER)) {
        score=0;
      } else if (skill>=PBTrainerAI.mediumSkill && move.Type == Types.ELECTRIC &&
            (opponent.hasWorkingAbility(Abilities.VOLT_ABSORB) ||
            opponent.hasWorkingAbility(Abilities.LIGHTNING_ROD) ||
            opponent.hasWorkingAbility(Abilities.MOTOR_DRIVE))) {
        score=0;
      }
      else {
        // Calculate how much damage the move will do (roughly)
        int realDamage=move.Power??0;
        if (move.Power==1) realDamage=60;
        if (skill>=PBTrainerAI.mediumSkill) {
          realDamage=pbBetterBaseDamage(move,attacker,opponent,skill,realDamage);
        }
        realDamage=pbRoughDamage(move,attacker,opponent,skill,realDamage);
        // Account for accuracy of move
        int accuracy=pbRoughAccuracy(move,attacker,opponent,skill);
        float basedamage=realDamage*accuracy/100.0f;
        // Two-turn attacks waste 2 turns to deal one lot of damage
        //if (move.pbTwoTurnAttack(attacker) || move.Effect==Attack.Data.Effects.x051) {		// Hyper Beam
        if (Game.MoveMetaData[move.MoveId].MaxTurns > 1 || move.Effect==Attack.Data.Effects.x051) {		// Hyper Beam
          basedamage*=2/3;   // Not halved because semi-invulnerable during use or hits first turn
        }
        // Prefer flinching effects
        if (!opponent.hasWorkingAbility(Abilities.INNER_FOCUS) &&
           opponent.effects.Substitute==0) {
          if ((attacker.hasWorkingItem(Items.KINGS_ROCK) || attacker.hasWorkingItem(Items.RAZOR_FANG))
          ) { //&& move.canKingsRock //ToDo: Check if can flinch
            basedamage*=1.05f;
          } else if (attacker.hasWorkingAbility(Abilities.STENCH) &&
                move.Effect!=Attack.Data.Effects.x114 && // Thunder Fang
                move.Effect!=Attack.Data.Effects.x112 && // Fire Fang
                move.Effect!=Attack.Data.Effects.x113 && // Ice Fang
                move.Effect!=Attack.Data.Effects.x020 && // flinch-inducing moves
                move.Effect!=Attack.Data.Effects.x097 && // Stomp
                move.Effect!=Attack.Data.Effects.x05D && // Snore
                move.Effect!=Attack.Data.Effects.x09F && // Fake Out
                move.Effect!=Attack.Data.Effects.x093 && // Twister
                move.Effect!=Attack.Data.Effects.x04C) {    // Sky Attack
            basedamage*=1.05f;
          }
        }
        // Convert damage to proportion of opponent's remaining HP
        basedamage=(basedamage*100.0f/opponent.HP);
        // Don't prefer weak attacks
//      if (basedamage<40) basedamage/=2; 
        // Prefer damaging attack if level difference is significantly high
        if (attacker.Level-10>opponent.Level) basedamage*=1.2f;
        // Adjust score
        basedamage=(int)Math.Round(basedamage);
        if (basedamage>120) basedamage=120;	// Treat all OHKO moves the same
        if (basedamage>100) basedamage+=40;	// Prefer moves likely to OHKO
        score=(int)Math.Round(score);
        float oldscore=score;
        score+=basedamage;
        GameDebug.Log($"[AI] #{move.MoveId.ToString(TextScripts.Name)} damage calculated (#{realDamage}=>#{basedamage}% of target's #{opponent.HP} HP), score change #{oldscore}=>#{score}");
      }
    }
    else {
      // Don't prefer attacks which don't deal damage
      score-=10;
      // Account for accuracy of move
      int accuracy=pbRoughAccuracy(move,attacker,opponent,skill);
      score*=accuracy/100.0f;
      if (score<=10 && skill>=PBTrainerAI.highSkill) score=0;
    }
    //score=score.ToInteger();
    if (score<0) score=0;
    return (int)score.ToInteger();
  }

#region Get type effectiveness and approximate stats.
  public float pbTypeModifier(Types type,Pokemon attacker,Pokemon opponent) {
    if (type<0) return 8;
    if (type == Types.GROUND && opponent.hasType(Types.FLYING) &&
                opponent.hasWorkingItem(Items.IRON_BALL) && !Core.USENEWBATTLEMECHANICS) return 8;
    Types atype=type;
    Types otype1=opponent.Type1;
    Types otype2=opponent.Type2;
    Types otype3=opponent.effects.Type3; //|| -1
    // Roost
    if (otype1 == Types.FLYING && opponent.effects.Roost) {
      if (otype2 == Types.FLYING && otype3 == Types.FLYING) {
        otype1=Types.NORMAL; //|| 0
      }
      else {
        otype1=otype2;
      }
    }
    if (otype2 == Types.FLYING && opponent.effects.Roost) {
      otype2=otype1;
    }
    // Get effectivenesses
    float mod1=atype.GetEffectiveness(otype1);
    float mod2=(otype1==otype2) ? 2 : atype.GetEffectiveness(otype2);
    float mod3=(otype3<0 || otype1==otype3 || otype2==otype3) ? 2 : atype.GetEffectiveness(otype3);
    if (opponent.hasWorkingItem(Items.RING_TARGET)) {
      if (mod1==0) mod1=2;
      if (mod2==0) mod2=2;
      if (mod3==0) mod3=2;
    }
    // Foresight
    //if ((attacker.hasWorkingAbility(Abilities.SCRAPPY)) || opponent.effects.Foresight) { //rescue false
    //  if (otype1 == Types.GHOST && atype.isIneffective(otype1)) mod1=2;
    //  if (otype2 == Types.GHOST && atype.isIneffective(otype2)) mod2=2;
    //  if (otype3 == Types.GHOST && atype.isIneffective(otype3)) mod3=2;
    //}
    //// Miracle Eye
    //if (opponent.effects.MiracleEye) {
    //  if (otype1 == Types.DARK && atype.isIneffective(otype1)) mod1=2;
    //  if (otype2 == Types.DARK && atype.isIneffective(otype2)) mod2=2;
    //  if (otype3 == Types.DARK && atype.isIneffective(otype3)) mod3=2;
    //}
    //// Delta Stream's weather
    //if (pbWeather()==Weather.STRONGWINDS) {
    //  if (otype1 == Types.FLYING && atype.isSuperEffective(otype1)) mod1=2;
    //  if (otype2 == Types.FLYING && atype.isSuperEffective(otype2)) mod2=2;
    //  if (otype3 == Types.FLYING && atype.isSuperEffective(otype3)) mod3=2;
    //}
    // Smack Down makes Ground moves work against fliers
    if (!opponent.isAirborne(attacker.hasMoldBreaker()) && atype == Types.GROUND) { //rescue false
      if (otype1 == Types.FLYING) mod1=2;
      if (otype2 == Types.FLYING) mod2=2;
      if (otype3 == Types.FLYING) mod3=2;
    }
    return mod1*mod2*mod3;
  }

  public float pbTypeModifier2(Pokemon battlerThis,Pokemon battlerOther) {
    // battlerThis isn't a Battler object, it's a Pokémon - it has no third type
    if (battlerThis.Type1==battlerThis.Type2) {
      return 4*pbTypeModifier(battlerThis.Type1,battlerThis,battlerOther);
    }
    float ret=pbTypeModifier(battlerThis.Type1,battlerThis,battlerOther);
    ret*=pbTypeModifier(battlerThis.Type2,battlerThis,battlerOther);
    return ret*2; // 0,1,2,4,_8_,16,32,64
  }

  public int pbRoughStat(Pokemon battler,Stats stat,int skill) {
    if (skill>=PBTrainerAI.highSkill && stat==Stats.SPEED) return battler.SPE;//pbSpeed;
    int[] stagemul=new int[] {2,2,2,2,2,2,2,3,4,5,6,7,8};
    int[] stagediv=new int[] {8,7,6,5,4,3,2,2,2,2,2,2,2};
    int stage=battler.stages[(int)stat]+6;
    int value=0;
    switch (stat) {
    case Stats.ATTACK: value=battler.pokemon.ATK;
      break;
    case Stats.DEFENSE: value=battler.pokemon.DEF;
      break;
    case Stats.SPEED: value=battler.pokemon.SPE;
      break;
    case Stats.SPATK: value=battler.pokemon.SPA;
      break;
    case Stats.SPDEF: value=battler.pokemon.SPD;
      break;
    }
    return (int)Math.Floor(value*1.0f*stagemul[stage]/stagediv[stage]);
  }

  public int pbBetterBaseDamage(Attack.Move move,Pokemon attacker,Pokemon opponent,int skill,int basedamage) { 
    int mult, n = 0; float weight = 0;
    // Covers all function codes which have their own def pbBaseDamage
    switch (move.Effect) {
    case Attack.Data.Effects.x083: // SonicBoom
      basedamage=20;
      break;
    case Attack.Data.Effects.x02A: // Dragon Rage
      basedamage=40;
      break;
    case Attack.Data.Effects.x029: // Super Fang
      basedamage=(int)Math.Floor(opponent.HP/2f);
      break;
    case Attack.Data.Effects.x058: // Night Shade
      basedamage=attacker.Level;
      break;
    case Attack.Data.Effects.x0BE: // Endeavor
      basedamage=opponent.HP-attacker.HP;
      break;
    case Attack.Data.Effects.x059: // Psywave
      basedamage=attacker.Level;
      break;
    case Attack.Data.Effects.x027: // OHKO
      basedamage=opponent.TotalHP;
      break;
    case Attack.Data.Effects.x05A: // Counter
      basedamage=60;
      break;
    case Attack.Data.Effects.x091: // Mirror Coat
      basedamage=60;
      break;
    case Attack.Data.Effects.x0E4: // Metal Burst
      basedamage=60;
      break;
    case Attack.Data.Effects.x102: ///case 0x12D: // case Surf: Shadow Storm
      if (Game.MoveData[opponent.effects.TwoTurnAttack].Effect==Attack.Data.Effects.x100) basedamage*=2;	// Dive
      break;
    case Attack.Data.Effects.x094: // Earthquake
      if (Game.MoveData[opponent.effects.TwoTurnAttack].Effect==Attack.Data.Effects.x101) basedamage*=2;	// Dig
      break;
    case Attack.Data.Effects.x096: case Attack.Data.Effects.x093: // case Gust: Twister
      if (Game.MoveData[opponent.effects.TwoTurnAttack].Effect==Attack.Data.Effects.x09C ||	// Fly
                       Game.MoveData[opponent.effects.TwoTurnAttack].Effect==Attack.Data.Effects.x108 || // Bounce
                       Game.MoveData[opponent.effects.TwoTurnAttack].Effect==Attack.Data.Effects.x138) basedamage*=2;    // Sky Drop
      break;
    case Attack.Data.Effects.x11C: // Venoshock
      if (opponent.Status==Status.POISON) basedamage*=2;
      break;
    case Attack.Data.Effects.x0AC: // SmellingSalt
      if (opponent.Status==Status.PARALYSIS) basedamage*=2;
      break;
    case Attack.Data.Effects.x0DA: // Wake-Up Slap
      if (opponent.Status==Status.SLEEP) basedamage*=2;
      break;
    case Attack.Data.Effects.x0AA: // Facade
      if (attacker.Status==Status.POISON ||
                       attacker.Status==Status.BURN ||
                       attacker.Status==Status.PARALYSIS) basedamage*=2;
      break;
    case Attack.Data.Effects.x137: // Hex
      if (opponent.Status!=0) basedamage*=2;
      break;
    case Attack.Data.Effects.x0DE: // Brine
      if (opponent.HP<=(int)Math.Floor(opponent.TotalHP/2f)) basedamage*=2;
      break;
    case Attack.Data.Effects.x140: // Retaliate
      //TODO
      break;
    case Attack.Data.Effects.x13E: // Acrobatics
      if (attacker.Item==0 || attacker.hasWorkingItem(Items.FLYING_GEM)) basedamage*=2;
      break;
    case Attack.Data.Effects.x0CC: // Weather Ball
      if (pbWeather()!=0) basedamage*=2;
      break;
    case Attack.Data.Effects.x07A: // Return
      basedamage=(int)Math.Max((int)Math.Floor(attacker.happiness*2/5f),1);
      break;
    case Attack.Data.Effects.x07C: // Frustration
      basedamage=(int)Math.Max((int)Math.Floor((255-attacker.happiness)*2/5f),1);
      break;
    case Attack.Data.Effects.x0BF: // Eruption
      basedamage=(int)Math.Max((int)Math.Floor(150f*attacker.HP/attacker.TotalHP),1);
      break;
    case Attack.Data.Effects.x0EE: // Crush Grip
      basedamage=(int)Math.Max((int)Math.Floor(120f*opponent.HP/opponent.TotalHP),1);
      break;
    case Attack.Data.Effects.x0DC: // Gyro Ball
      int ospeed=pbRoughStat(opponent,Stats.SPEED,skill);
      int aspeed=pbRoughStat(attacker,Stats.SPEED,skill);
      basedamage=(int)Math.Max((int)Math.Min((int)Math.Floor(25f*ospeed/aspeed),150),1);
      break;
    case Attack.Data.Effects.x132: // Stored Power
      mult=0;
      foreach (Stats i in new Stats[] { Stats.ATTACK,Stats.DEFENSE,Stats.SPEED,
                Stats.SPATK,Stats.SPDEF,Stats.ACCURACY,Stats.EVASION }) {
        if (attacker.stages[(int)i]>0) mult+=attacker.stages[(int)i];
      }
      basedamage=20*(int)(mult+1);
      break;
    case Attack.Data.Effects.x0F6: // Punishment
      mult=0;
      foreach (Stats i in new Stats[] { Stats.ATTACK,Stats.DEFENSE,Stats.SPEED,
                Stats.SPATK,Stats.SPDEF,Stats.ACCURACY,Stats.EVASION }) {
        if (opponent.stages[(int)i]>0) mult+=opponent.stages[(int)i];
      }
      basedamage=(int)Math.Min(20*(mult+3),200);
      break;
    case Attack.Data.Effects.x088: // Hidden Power
      ///int[] hp=pbHiddenPower(attacker.IV);
      int[] hp=new int[] { 0 }; /// ToDo: Redo
      basedamage=hp[1];
      break;
    case Attack.Data.Effects.x078: // Fury Cutter
      basedamage=basedamage<<(attacker.effects.FuryCutter-1);
      break;
    case Attack.Data.Effects.x12F: // Echoed Voice
      basedamage*=attacker.OwnSide.EchoedVoiceCounter;
      break;
    case Attack.Data.Effects.x07B: // Present
      basedamage=50;
      break;
    case Attack.Data.Effects.x07F: // Magnitude
      basedamage=71;
      if (Game.MoveData[opponent.effects.TwoTurnAttack].Effect==Attack.Data.Effects.x101) basedamage*=2;	// Dig
      break;
    case Attack.Data.Effects.x0DF: // Natural Gift
                    KeyValuePair<Items, int>[] damagearray = new KeyValuePair<Items, int>[]{
         //60 => [
                new KeyValuePair<Items, int> (Items.CHERI_BERRY, 60),new KeyValuePair<Items, int> (Items.CHESTO_BERRY, 60),new KeyValuePair<Items, int> (Items.PECHA_BERRY, 60),new KeyValuePair<Items, int> (Items.RAWST_BERRY, 60),new KeyValuePair<Items, int> (Items.ASPEAR_BERRY, 60),
                new KeyValuePair<Items, int> (Items.LEPPA_BERRY, 60),new KeyValuePair<Items, int> (Items.ORAN_BERRY, 60),new KeyValuePair<Items, int> (Items.PERSIM_BERRY, 60),new KeyValuePair<Items, int> (Items.LUM_BERRY, 60),new KeyValuePair<Items, int> (Items.SITRUS_BERRY, 60),
                new KeyValuePair<Items, int> (Items.FIGY_BERRY, 60),new KeyValuePair<Items, int> (Items.WIKI_BERRY, 60),new KeyValuePair<Items, int> (Items.MAGO_BERRY, 60),new KeyValuePair<Items, int> (Items.AGUAV_BERRY, 60),new KeyValuePair<Items, int> (Items.IAPAPA_BERRY, 60),
                new KeyValuePair<Items, int> (Items.RAZZ_BERRY, 60),new KeyValuePair<Items, int> (Items.OCCA_BERRY, 60),new KeyValuePair<Items, int> (Items.PASSHO_BERRY, 60),new KeyValuePair<Items, int> (Items.WACAN_BERRY, 60),new KeyValuePair<Items, int> (Items.RINDO_BERRY, 60),
                new KeyValuePair<Items, int> (Items.YACHE_BERRY, 60),new KeyValuePair<Items, int> (Items.CHOPLE_BERRY, 60),new KeyValuePair<Items, int> (Items.KEBIA_BERRY, 60),new KeyValuePair<Items, int> (Items.SHUCA_BERRY, 60),new KeyValuePair<Items, int> (Items.COBA_BERRY, 60),
                new KeyValuePair<Items, int> (Items.PAYAPA_BERRY, 60),new KeyValuePair<Items, int> (Items.TANGA_BERRY, 60),new KeyValuePair<Items, int> (Items.CHARTI_BERRY, 60),new KeyValuePair<Items, int> (Items.KASIB_BERRY, 60),new KeyValuePair<Items, int> (Items.HABAN_BERRY, 60),
                new KeyValuePair<Items, int> (Items.COLBUR_BERRY, 60),new KeyValuePair<Items, int> (Items.BABIRI_BERRY, 60),new KeyValuePair<Items, int> (Items.CHILAN_BERRY, 60),//],
         //70 => [
                new KeyValuePair<Items, int> (Items.BLUK_BERRY, 70),new KeyValuePair<Items, int> (Items.NANAB_BERRY, 70),new KeyValuePair<Items, int> (Items.WEPEAR_BERRY, 70),new KeyValuePair<Items, int> (Items.PINAP_BERRY, 70),new KeyValuePair<Items, int> (Items.POMEG_BERRY, 70),
                new KeyValuePair<Items, int> (Items.KELPSY_BERRY, 70),new KeyValuePair<Items, int> (Items.QUALOT_BERRY, 70),new KeyValuePair<Items, int> (Items.HONDEW_BERRY, 70),new KeyValuePair<Items, int> (Items.GREPA_BERRY, 70),new KeyValuePair<Items, int> (Items.TAMATO_BERRY, 70),
                new KeyValuePair<Items, int> (Items.CORNN_BERRY, 70),new KeyValuePair<Items, int> (Items.MAGOST_BERRY, 70),new KeyValuePair<Items, int> (Items.RABUTA_BERRY, 70),new KeyValuePair<Items, int> (Items.NOMEL_BERRY, 70),new KeyValuePair<Items, int> (Items.SPELON_BERRY, 70),
                new KeyValuePair<Items, int> (Items.PAMTRE_BERRY, 70),//],
         //80 => [
                new KeyValuePair<Items, int> (Items.WATMEL_BERRY, 80),new KeyValuePair<Items, int> (Items.DURIN_BERRY, 80),new KeyValuePair<Items, int> (Items.BELUE_BERRY, 80),new KeyValuePair<Items, int> (Items.LIECHI_BERRY, 80),new KeyValuePair<Items, int> (Items.GANLON_BERRY, 80),
                new KeyValuePair<Items, int> (Items.SALAC_BERRY, 80),new KeyValuePair<Items, int> (Items.PETAYA_BERRY, 80),new KeyValuePair<Items, int> (Items.APICOT_BERRY, 80),new KeyValuePair<Items, int> (Items.LANSAT_BERRY, 80),new KeyValuePair<Items, int> (Items.STARF_BERRY, 80),
                new KeyValuePair<Items, int> (Items.ENIGMA_BERRY, 80),new KeyValuePair<Items, int> (Items.MICLE_BERRY, 80),new KeyValuePair<Items, int> (Items.CUSTAP_BERRY, 80),new KeyValuePair<Items, int> (Items.JABOCA_BERRY, 80),new KeyValuePair<Items, int> (Items.ROWAP_BERRY, 80)//]
      };
      bool haveanswer=false;
      foreach (var i in damagearray) { //.Keys
        //data=damagearray[i];
        //if (data) {
        //  foreach (var j in data) {
            if (attacker.Item == i.Key) { //j
              basedamage=i.Value; haveanswer=true; break;
            }
        //  }
        //}
        if (haveanswer) break;
      }
      break;
    case Attack.Data.Effects.x0EC: // Trump Card
      int[] dmgs=new int[] { 200, 80, 60, 50, 40 };
      int ppleft=(int)Math.Min(move.PP-1,4);   // PP is reduced before the move is used
      basedamage=dmgs[ppleft];
      break;
    case Attack.Data.Effects.x064: // Flail
      n=(int)Math.Floor(48f*attacker.HP/attacker.TotalHP);
      basedamage=20;
      if (n<33) basedamage=40;
      if (n<17) basedamage=80;
      if (n<10) basedamage=100;
      if (n<5) basedamage=150;
      if (n<2) basedamage=200;
      break;
    case Attack.Data.Effects.x126: // Electro Ball
      n=(int)Math.Floor((float)attacker.SPE/opponent.SPE);
      basedamage=40;
      if (n>=1) basedamage=60;
      if (n>=2) basedamage=80;
      if (n>=3) basedamage=120;
      if (n>=4) basedamage=150;
      break;
    case Attack.Data.Effects.x0C5: // Low Kick
      weight=opponent.Weight();
      basedamage=20;
      if (weight>100) basedamage=40;
      if (weight>250) basedamage=60;
      if (weight>500) basedamage=80;
      if (weight>1000) basedamage=100;
      if (weight>2000) basedamage=120;
      break;
    case Attack.Data.Effects.x124: // Heavy Slam
      n=(int)Math.Floor(attacker.Weight()/opponent.Weight());
      basedamage=40;
      if (n>=2) basedamage=60;
      if (n>=3) basedamage=80;
      if (n>=4) basedamage=100;
      if (n>=5) basedamage=120;
      break;
    case Attack.Data.Effects.x121: // Frost Breath
      basedamage*=2;
      break;
    case Attack.Data.Effects.x02D: case Attack.Data.Effects.x04E: // Double case Kick: Twineedle
      basedamage*=2;
      break;
    case Attack.Data.Effects.x069: // Triple Kick
      basedamage*=6;
      break;
    case Attack.Data.Effects.x169: // Fury Attack
      if (attacker.hasWorkingAbility(Abilities.SKILL_LINK)) {
        basedamage*=5;
      }
      else {
        basedamage=(int)Math.Floor(basedamage*19/6f);
      }
      break;
    case Attack.Data.Effects.x09B: // Beat Up
      Monster.Pokemon[] party=pbParty(attacker.Index);
      mult=0;
      for (int i = 0; i < party.Length; i++) {
        if (party[i].IsNotNullOrNone() && !party[i].isEgg &&
                   party[i].HP>0 && party[i].Status==0) mult+=1;
      }
      basedamage*=mult;
      break;
    case Attack.Data.Effects.x098: // SolarBeam
      if (pbWeather()!=0 && pbWeather()!=Weather.SUNNYDAY) {
        basedamage=(int)Math.Floor(basedamage*0.5f);
      }
      break;
    case Attack.Data.Effects.x106: // Whirlpool
      if (skill>=PBTrainerAI.mediumSkill) {
        if (Game.MoveData[opponent.effects.TwoTurnAttack].Effect==Attack.Data.Effects.x100) basedamage*=2;	// Dive
      }
      break;
    case Attack.Data.Effects.x076: // Rollout
      if (skill>=PBTrainerAI.mediumSkill) {
        if (attacker.effects.DefenseCurl) basedamage*=2;
      }
      break;
    case Attack.Data.Effects.x141: // Final Gambit
      basedamage=attacker.HP;
      break;
    case Attack.Data.Effects.x0EA: // Fling
      //TODO
      break;
    case Attack.Data.Effects.x0A2: // Spit Up
      basedamage*=attacker.effects.Stockpile;
      break;
    case Attack.Data.Effects.x152:
      Types type=Types.FLYING; //|| -1;
      if (type>=0) {
        //ToDo: Get math for this...
        mult=1; //type.GetCombinedEffectiveness(
        //   opponent.Type1,opponent.Type2,opponent.effects.Type3);
        basedamage=(int)Math.Round((basedamage*mult)/8f);
      }
      break;
    }
    return basedamage;
  }

  public int pbRoughDamage(Attack.Move move, Pokemon attacker, Pokemon opponent, int skill, double basedamage) {
    // Fixed damage moves
    if (move.Effect==Attack.Data.Effects.x083 ||  	// SonicBoom
                         move.Effect==Attack.Data.Effects.x02A ||   // Dragon Rage
                         move.Effect==Attack.Data.Effects.x029 ||   // Super Fang
                         move.Effect==Attack.Data.Effects.x058 ||   // Night Shade
                         move.Effect==Attack.Data.Effects.x0BE ||   // Endeavor
                         move.Effect==Attack.Data.Effects.x059 ||   // Psywave
                         move.Effect==Attack.Data.Effects.x027 ||   // OHKO
                         move.Effect==Attack.Data.Effects.x05A ||   // Counter
                         move.Effect==Attack.Data.Effects.x091 ||   // Mirror Coat
                         move.Effect==Attack.Data.Effects.x0E4 ||   // Metal Burst
                         move.Effect==Attack.Data.Effects.x141) return (int)basedamage;      // Final Gambit
    Types type=move.Type;
    // More accurate move type (includes Normalize, most type-changing moves, etc.)
    //if (skill>=PBTrainerAI.highSkill) {
    //  type=move.pbType(type,attacker,opponent);
    //}
    // Technician
    if (skill>=PBTrainerAI.highSkill) {
      if (attacker.hasWorkingAbility(Abilities.TECHNICIAN) && basedamage<=60) {
        basedamage=(int)Math.Round(basedamage*1.5);
      }
    }
    // Iron Fist
    if (skill>=PBTrainerAI.mediumSkill) {
      if (attacker.hasWorkingAbility(Abilities.IRON_FIST) && move.Flag.Punching) {
        basedamage=(int)Math.Round(basedamage*1.2);
      }
    }
    // Reckless
    if (skill>=PBTrainerAI.mediumSkill) {
      if (attacker.hasWorkingAbility(Abilities.RECKLESS)) {
        if(move.Effect==Attack.Data.Effects.x031 || 		// Take Down, etc.
           move.Effect==Attack.Data.Effects.x0C7 ||  // Double-Edge, etc.
           move.Effect==Attack.Data.Effects.x10E ||  // Head Smash
           move.Effect==Attack.Data.Effects.x107 ||  // Volt Tackle
           move.Effect==Attack.Data.Effects.x0FE ||  // Flare Blitz
           move.Effect==Attack.Data.Effects.x02E || // Jump Kick, Hi Jump Kick
           move.Effect==Attack.Data.Effects.x712) {    // Shadow End
          basedamage=(int)Math.Round(basedamage*1.2);
        }
      }
    }
    // Flare Boost
    if (skill>=PBTrainerAI.highSkill) {
      if (attacker.hasWorkingAbility(Abilities.FLARE_BOOST) &&
         attacker.Status==Status.BURN && move.pbIsSpecial(type)) {
        basedamage=(int)Math.Round(basedamage*1.5);
      }
    }
    // Toxic Boost
    if (skill>=PBTrainerAI.highSkill) {
      if (attacker.hasWorkingAbility(Abilities.TOXIC_BOOST) &&
         attacker.Status==Status.POISON && move.pbIsPhysical(type)) {
        basedamage=(int)Math.Round(basedamage*1.5);
      }
    }
    // Analytic
    // Rivalry
    if (skill>=PBTrainerAI.mediumSkill) {
      if (attacker.hasWorkingAbility(Abilities.RIVALRY) &&
         attacker.Gender!=null && opponent.Gender!=null) {
        if (attacker.Gender==opponent.Gender) {
          basedamage=(int)Math.Round(basedamage*1.25);
        }
        else {
          basedamage=(int)Math.Round(basedamage*0.75);
        }
      }
    }
    // Sand Force
    if (skill>=PBTrainerAI.mediumSkill) {
      if (attacker.hasWorkingAbility(Abilities.SAND_FORCE) &&
         pbWeather()==Weather.SANDSTORM &&
         (type == Types.ROCK ||
         type == Types.GROUND ||
         type == Types.STEEL)) {
        basedamage=(int)Math.Round(basedamage*1.3);
      }
    }
    // Heatproof
    if (skill>=PBTrainerAI.bestSkill) {
      if (opponent.hasWorkingAbility(Abilities.HEATPROOF) &&
         type == Types.FIRE) {
        basedamage=(int)Math.Round(basedamage*0.5);
      }
    }
    // Dry Skin
    if (skill>=PBTrainerAI.bestSkill) {
      if (opponent.hasWorkingAbility(Abilities.DRY_SKIN) &&
         type == Types.FIRE) {
        basedamage=(int)Math.Round(basedamage*1.25);
      }
    }
    // Sheer Force
    if (skill>=PBTrainerAI.highSkill) {
      if (attacker.hasWorkingAbility(Abilities.SHEER_FORCE)) { //&& move.AddlEffect>0 //ToDo: Refector this?
        basedamage=(int)Math.Round(basedamage*1.3);
      }
    }
    // Type-boosting items
    if ((attacker.hasWorkingItem(Items.SILK_SCARF) && type == Types.NORMAL) ||
       (attacker.hasWorkingItem(Items.BLACK_BELT) && type == Types.FIGHTING) ||
       (attacker.hasWorkingItem(Items.SHARP_BEAK) && type == Types.FLYING) ||
       (attacker.hasWorkingItem(Items.POISON_BARB) && type == Types.POISON) ||
       (attacker.hasWorkingItem(Items.SOFT_SAND) && type == Types.GROUND) ||
       (attacker.hasWorkingItem(Items.HARD_STONE) && type == Types.ROCK) ||
       (attacker.hasWorkingItem(Items.SILVER_POWDER) && type == Types.BUG) ||
       (attacker.hasWorkingItem(Items.SPELL_TAG) && type == Types.GHOST) ||
       (attacker.hasWorkingItem(Items.METAL_COAT) && type == Types.STEEL) ||
       (attacker.hasWorkingItem(Items.CHARCOAL) && type == Types.FIRE) ||
       (attacker.hasWorkingItem(Items.MYSTIC_WATER) && type == Types.WATER) ||
       (attacker.hasWorkingItem(Items.MIRACLE_SEED) && type == Types.GRASS) ||
       (attacker.hasWorkingItem(Items.MAGNET) && type == Types.ELECTRIC) ||
       (attacker.hasWorkingItem(Items.TWISTED_SPOON) && type == Types.PSYCHIC) ||
       (attacker.hasWorkingItem(Items.NEVER_MELT_ICE) && type == Types.ICE) ||
       (attacker.hasWorkingItem(Items.DRAGON_FANG) && type == Types.DRAGON) ||
       (attacker.hasWorkingItem(Items.BLACK_GLASSES) && type == Types.DARK)) {
      basedamage=(int)Math.Round(basedamage*1.2);
    }
    if ((attacker.hasWorkingItem(Items.FIST_PLATE) && type == Types.FIGHTING) ||
       (attacker.hasWorkingItem(Items.SKY_PLATE) && type == Types.FLYING) ||
       (attacker.hasWorkingItem(Items.TOXIC_PLATE) && type == Types.POISON) ||
       (attacker.hasWorkingItem(Items.EARTH_PLATE) && type == Types.GROUND) ||
       (attacker.hasWorkingItem(Items.STONE_PLATE) && type == Types.ROCK) ||
       (attacker.hasWorkingItem(Items.INSECT_PLATE) && type == Types.BUG) ||
       (attacker.hasWorkingItem(Items.SPOOKY_PLATE) && type == Types.GHOST) ||
       (attacker.hasWorkingItem(Items.IRON_PLATE) && type == Types.STEEL) ||
       (attacker.hasWorkingItem(Items.FLAME_PLATE) && type == Types.FIRE) ||
       (attacker.hasWorkingItem(Items.SPLASH_PLATE) && type == Types.WATER) ||
       (attacker.hasWorkingItem(Items.MEADOW_PLATE) && type == Types.GRASS) ||
       (attacker.hasWorkingItem(Items.ZAP_PLATE) && type == Types.ELECTRIC) ||
       (attacker.hasWorkingItem(Items.MIND_PLATE) && type == Types.PSYCHIC) ||
       (attacker.hasWorkingItem(Items.ICICLE_PLATE) && type == Types.ICE) ||
       (attacker.hasWorkingItem(Items.DRACO_PLATE) && type == Types.DRAGON) ||
       (attacker.hasWorkingItem(Items.DREAD_PLATE) && type == Types.DARK)) {
      basedamage=(int)Math.Round(basedamage*1.2);
    }
    if ((attacker.hasWorkingItem(Items.NORMAL_GEM) && type == Types.NORMAL) ||
       (attacker.hasWorkingItem(Items.FIGHTING_GEM) && type == Types.FIGHTING) ||
       (attacker.hasWorkingItem(Items.FLYING_GEM) && type == Types.FLYING) ||
       (attacker.hasWorkingItem(Items.POISON_GEM) && type == Types.POISON) ||
       (attacker.hasWorkingItem(Items.GROUND_GEM) && type == Types.GROUND) ||
       (attacker.hasWorkingItem(Items.ROCK_GEM) && type == Types.ROCK) ||
       (attacker.hasWorkingItem(Items.BUG_GEM) && type == Types.BUG) ||
       (attacker.hasWorkingItem(Items.GHOST_GEM) && type == Types.GHOST) ||
       (attacker.hasWorkingItem(Items.STEEL_GEM) && type == Types.STEEL) ||
       (attacker.hasWorkingItem(Items.FIRE_GEM) && type == Types.FIRE) ||
       (attacker.hasWorkingItem(Items.WATER_GEM) && type == Types.WATER) ||
       (attacker.hasWorkingItem(Items.GRASS_GEM) && type == Types.GRASS) ||
       (attacker.hasWorkingItem(Items.ELECTRIC_GEM) && type == Types.ELECTRIC) ||
       (attacker.hasWorkingItem(Items.PSYCHIC_GEM) && type == Types.PSYCHIC) ||
       (attacker.hasWorkingItem(Items.ICE_GEM) && type == Types.ICE) ||
       (attacker.hasWorkingItem(Items.DRAGON_GEM) && type == Types.DRAGON) ||
       (attacker.hasWorkingItem(Items.DARK_GEM) && type == Types.DARK)) {
      basedamage=(int)Math.Round(basedamage*1.5);
    }
    if (attacker.hasWorkingItem(Items.ROCK_INCENSE) && type == Types.ROCK) {
      basedamage=(int)Math.Round(basedamage*1.2);
    }
    if (attacker.hasWorkingItem(Items.ROSE_INCENSE) && type == Types.GRASS) {
      basedamage=(int)Math.Round(basedamage*1.2);
    }
    if (attacker.hasWorkingItem(Items.SEA_INCENSE) && type == Types.WATER) {
      basedamage=(int)Math.Round(basedamage*1.2);
    }
    if (attacker.hasWorkingItem(Items.WAVE_INCENSE) && type == Types.WATER) {
      basedamage=(int)Math.Round(basedamage*1.2);
    }
    if (attacker.hasWorkingItem(Items.ODD_INCENSE) && type == Types.PSYCHIC) {
      basedamage=(int)Math.Round(basedamage*1.2);
    }
    // Muscle Band
    if (attacker.hasWorkingItem(Items.MUSCLE_BAND) && move.pbIsPhysical(type)) {
      basedamage=(int)Math.Round(basedamage*1.1);
    }
    // Wise Glasses
    if (attacker.hasWorkingItem(Items.WISE_GLASSES) && move.pbIsSpecial(type)) {
      basedamage=(int)Math.Round(basedamage*1.1);
    }
    // Legendary Orbs
    if (attacker.Species == Pokemons.PALKIA &&
       attacker.hasWorkingItem(Items.LUSTROUS_ORB) &&
       (type == Types.DRAGON || type == Types.WATER)) {
      basedamage=(int)Math.Round(basedamage*1.2);
    }
    if (attacker.Species == Pokemons.DIALGA &&
       attacker.hasWorkingItem(Items.ADAMANT_ORB) &&
       (type == Types.DRAGON || type == Types.STEEL)) {
      basedamage=(int)Math.Round(basedamage*1.2);
    }
    if (attacker.Species == Pokemons.GIRATINA &&
       attacker.hasWorkingItem(Items.GRISEOUS_ORB) &&
       (type == Types.DRAGON || type == Types.GHOST)) {
      basedamage=(int)Math.Round(basedamage*1.2);
    }
    // pbBaseDamageMultiplier - TODO
    // Me First
    // Charge
    if (attacker.effects.Charge>0 && type == Types.ELECTRIC) {
      basedamage=(int)Math.Round(basedamage*2.0);
    }
    // Helping Hand - n/a
    // Water Sport
    if (skill>=PBTrainerAI.mediumSkill) {
      if (type == Types.FIRE) {
        for (int i = 0; i < 4; i++) {
          if (@battlers[i].effects.WaterSport && !@battlers[i].isFainted()) {
            basedamage=(int)Math.Round(basedamage*0.33);
            break;
          }
        }
      }
    }
    // Mud Sport
    if (skill>=PBTrainerAI.mediumSkill) {
      if (type == Types.ELECTRIC) {
        for (int i = 0; i < 4; i++) {
          if (@battlers[i].effects.MudSport && !@battlers[i].isFainted()) {
            basedamage=(int)Math.Round(basedamage*0.33);
            break;
          }
        }
      }
    }
    // Get base attack stat
    int atk=pbRoughStat(attacker,Stats.ATTACK,skill);
    if (move.Effect==Attack.Data.Effects.x12A) {		// Foul Play
      atk=pbRoughStat(opponent,Stats.ATTACK,skill);
    }
    if (type>=0 && move.pbIsSpecial(type)) {
      atk=pbRoughStat(attacker,Stats.SPATK,skill);
      if (move.Effect==Attack.Data.Effects.x12A) {		// Foul Play
        atk=pbRoughStat(opponent,Stats.SPATK,skill);
      }
    }
    // Hustle
    if (skill>=PBTrainerAI.highSkill) {
      if (attacker.hasWorkingAbility(Abilities.HUSTLE) && move.pbIsPhysical(type)) {
        atk=(int)Math.Round(atk*1.5);
      }
    }
    // Thick Fat
    if (skill>=PBTrainerAI.bestSkill) {
      if (opponent.hasWorkingAbility(Abilities.THICK_FAT) &&
         (type == Types.ICE || type == Types.FIRE)) {
        atk=(int)Math.Round(atk*0.5);
      }
    }
    // Pinch abilities
    if (skill>=PBTrainerAI.mediumSkill) {
      if (attacker.HP<=(int)Math.Floor(attacker.TotalHP/3f)) {
        if ((attacker.hasWorkingAbility(Abilities.OVERGROW) && type == Types.GRASS) ||
           (attacker.hasWorkingAbility(Abilities.BLAZE) && type == Types.FIRE) ||
           (attacker.hasWorkingAbility(Abilities.TORRENT) && type == Types.WATER) ||
           (attacker.hasWorkingAbility(Abilities.SWARM) && type == Types.BUG)) {
          atk=(int)Math.Round(atk*1.5);
        }
      }
    }
    // Guts
    if (skill>=PBTrainerAI.highSkill) {
      if (attacker.hasWorkingAbility(Abilities.GUTS) &&
         attacker.Status!=0 && move.pbIsPhysical(type)) {
        atk=(int)Math.Round(atk*1.5);
      }
    }
    // Plus, Minus
    if (skill>=PBTrainerAI.mediumSkill) {
      if ((attacker.hasWorkingAbility(Abilities.PLUS) ||
         attacker.hasWorkingAbility(Abilities.MINUS)) && move.pbIsSpecial(type)) {
        Pokemon partner=attacker.Partner;
        if (partner.hasWorkingAbility(Abilities.PLUS) || partner.hasWorkingAbility(Abilities.MINUS)) {
          atk=(int)Math.Round(atk*1.5);
        }
      }
    }
    // Defeatist
    if (skill>=PBTrainerAI.mediumSkill) {
      if (attacker.hasWorkingAbility(Abilities.DEFEATIST) &&
         attacker.HP<=(int)Math.Floor(attacker.TotalHP/2f)) {
        atk=(int)Math.Round(atk*0.5);
      }
    }
    // Pure Power, Huge Power
    if (skill>=PBTrainerAI.mediumSkill) {          
      if (attacker.hasWorkingAbility(Abilities.PURE_POWER) ||
         attacker.hasWorkingAbility(Abilities.HUGE_POWER)) {
        atk=(int)Math.Round(atk*2.0);
      }
    }
    // Solar Power
    if (skill>=PBTrainerAI.highSkill) {
      if (attacker.hasWorkingAbility(Abilities.SOLAR_POWER) &&
         pbWeather()==Weather.SUNNYDAY && move.pbIsSpecial(type)) {
        atk=(int)Math.Round(atk*1.5);
      }
    }
    // Flash Fire
    if (skill>=PBTrainerAI.highSkill) {
      if (attacker.hasWorkingAbility(Abilities.FLASH_FIRE) &&
         attacker.effects.FlashFire && type == Types.FIRE) {
        atk=(int)Math.Round(atk*1.5);
      }
    }
    // Slow Start
    if (skill>=PBTrainerAI.mediumSkill) {
      if (attacker.hasWorkingAbility(Abilities.SLOW_START) &&
         attacker.turncount<5 && move.pbIsPhysical(type)) {
        atk=(int)Math.Round(atk*0.5);
      }
    }
    // Flower Gift
    if (skill>=PBTrainerAI.highSkill) {
      if (pbWeather()==Weather.SUNNYDAY && move.pbIsPhysical(type)) {
        if (attacker.hasWorkingAbility(Abilities.FLOWER_GIFT) &&
           attacker.Species == Pokemons.CHERRIM) {
          atk=(int)Math.Round(atk*1.5);
        }
        if (attacker.Partner.hasWorkingAbility(Abilities.FLOWER_GIFT) &&
           attacker.Partner.Species == Pokemons.CHERRIM) {
          atk=(int)Math.Round(atk*1.5);
        }
      }
    }
    // Attack-boosting items
    if (attacker.hasWorkingItem(Items.THICK_CLUB) &&
       (attacker.Species == Pokemons.CUBONE ||
       attacker.Species == Pokemons.MAROWAK) && move.pbIsPhysical(type)) {
      atk=(int)Math.Round(atk*2.0);
    }
    if (attacker.hasWorkingItem(Items.DEEP_SEA_TOOTH) &&
       attacker.Species == Pokemons.CLAMPERL && move.pbIsSpecial(type)) {
      atk=(int)Math.Round(atk*2.0);
    }
    if (attacker.hasWorkingItem(Items.LIGHT_BALL) &&
       attacker.Species == Pokemons.PIKACHU) {
      atk=(int)Math.Round(atk*2.0);
    }
    if (attacker.hasWorkingItem(Items.SOUL_DEW) &&
       (attacker.Species == Pokemons.LATIAS ||
       attacker.Species == Pokemons.LATIOS) && move.pbIsSpecial(type)) {
      atk=(int)Math.Round(atk*1.5);
    }
    if (attacker.hasWorkingItem(Items.CHOICE_BAND) && move.pbIsPhysical(type)) {
      atk=(int)Math.Round(atk*1.5);
    }
    if (attacker.hasWorkingItem(Items.CHOICE_SPECS) && move.pbIsSpecial(type)) {
      atk=(int)Math.Round(atk*1.5);
    }
    // Get base defense stat
    int defense=pbRoughStat(opponent,Stats.DEFENSE,skill);
    bool applysandstorm=false;
    if (type>=0 && move.pbIsSpecial(type)) {
      if (move.Effect!=Attack.Data.Effects.x11B) {		// Psyshock
        defense=pbRoughStat(opponent,Stats.SPDEF,skill);
        applysandstorm=true;
      }
    }
    // Sandstorm weather
    if (skill>=PBTrainerAI.highSkill) {
      if (pbWeather()==Weather.SANDSTORM &&
         opponent.hasType(Types.ROCK) && applysandstorm) {
        defense=(int)Math.Round(defense*1.5);
      }
    }
    // Marvel Scale
    if (skill>=PBTrainerAI.bestSkill) {
      if (opponent.hasWorkingAbility(Abilities.MARVEL_SCALE) &&
         opponent.Status>0 && move.pbIsPhysical(type)) {
        defense=(int)Math.Round(defense*1.5);
      }
    }
    // Flower Gift
    if (skill>=PBTrainerAI.bestSkill) {
      if (pbWeather()==Weather.SUNNYDAY && move.pbIsSpecial(type)) {
        if (opponent.hasWorkingAbility(Abilities.FLOWER_GIFT) &&
           opponent.Species == Pokemons.CHERRIM) {
          defense=(int)Math.Round(defense*1.5);
        }
        if (opponent.Partner.hasWorkingAbility(Abilities.FLOWER_GIFT) &&
           opponent.Partner.Species == Pokemons.CHERRIM) {
          defense=(int)Math.Round(defense*1.5);
        }
      }
    }
    // Defense-boosting items
    if (skill>=PBTrainerAI.highSkill) {
      if (opponent.hasWorkingItem(Items.EVIOLITE)) {
        //Pokemon[] evos=pbGetEvolvedFormData(opponent.Species);
        int evos=Game.PokemonEvolutionsData[opponent.Species].Length;
        //if (evos && evos.Length>0) {
        if (evos>0) {
          defense=(int)Math.Round(defense*1.5);
        }
      }
      if (opponent.hasWorkingItem(Items.DEEP_SEA_SCALE) &&
         opponent.Species == Pokemons.CLAMPERL && move.pbIsSpecial(type)) {
        defense=(int)Math.Round(defense*2.0);
      }
      if (opponent.hasWorkingItem(Items.METAL_POWDER) &&
         opponent.Species == Pokemons.DITTO &&
         !opponent.effects.Transform && move.pbIsPhysical(type)) {
        defense=(int)Math.Round(defense*2.0);
      }
      if (opponent.hasWorkingItem(Items.SOUL_DEW) &&
         (opponent.Species == Pokemons.LATIAS ||
         opponent.Species == Pokemons.LATIOS) && move.pbIsSpecial(type)) {
        defense=(int)Math.Round(defense*1.5);
      }
    }
    // Main damage calculation
    double damage=Math.Floor(Math.Floor(Math.Floor(2.0f*attacker.Level/5f+2f)*basedamage*atk/defense)/50f)+2;
    // Multi-targeting attacks
    if (skill>=PBTrainerAI.highSkill) {
      //if (move.pbTargetsMultiple(attacker)) {
      if (move.hasMultipleTargets()) {
        damage=(int)Math.Round(damage*0.75);
      }
    }
    // Weather
    if (skill>=PBTrainerAI.mediumSkill) {
      switch (pbWeather()) {
      case Weather.SUNNYDAY:
        if (type == Types.FIRE) {
          damage=(int)Math.Round(damage*1.5);
        } else if (type == Types.WATER) {
          damage=(int)Math.Round(damage*0.5);
        }
        break;
      case Weather.RAINDANCE:
        if (type == Types.FIRE) {
          damage=(int)Math.Round(damage*0.5);
        } else if (type == Types.WATER) {
          damage=(int)Math.Round(damage*1.5);
        }
        break;
      }
    }
    // Critical hits - n/a
    // Random variance - n/a
    // STAB
    if (skill>=PBTrainerAI.mediumSkill) {
      if (attacker.hasType(type)) {
        if (attacker.hasWorkingAbility(Abilities.ADAPTABILITY) &&
           skill>=PBTrainerAI.highSkill) {
          damage=(int)Math.Round(damage*2f);
        }
        else {
          damage=(int)Math.Round(damage*1.5);
        }
      }
    }
    // Type effectiveness
    float typemod=pbTypeModifier(type,attacker,opponent);
    if (skill>=PBTrainerAI.highSkill) {
      damage=(int)Math.Round(damage*typemod*1.0/8);
    }
    // Burn
    if (skill>=PBTrainerAI.mediumSkill) {
      if (attacker.Status==Status.BURN && move.pbIsPhysical(type) &&
         !attacker.hasWorkingAbility(Abilities.GUTS)) {
        damage=(int)Math.Round(damage*0.5);
      }
    }
    // Make sure damage is at least 1
    if (damage<1) damage=1;
    // Reflect
    if (skill>=PBTrainerAI.highSkill) {
      if (opponent.OwnSide.Reflect>0 && move.pbIsPhysical(type)) {
        if (!opponent.Partner.isFainted()) {
          damage=(int)Math.Round(damage*0.66);
        }
        else {
          damage=(int)Math.Round(damage*0.5);
        }
      }
    }
    // Light Screen
    if (skill>=PBTrainerAI.highSkill) {
      if (opponent.OwnSide.LightScreen>0 && move.pbIsSpecial(type)) {
        if (!opponent.Partner.isFainted()) {
          damage=(int)Math.Round(damage*0.66);
        }
        else {
          damage=(int)Math.Round(damage*0.5);
        }
      }
    }
    // Multiscale
    if (skill>=PBTrainerAI.bestSkill) {
      if (opponent.hasWorkingAbility(Abilities.MULTISCALE) &&
         opponent.HP==opponent.TotalHP) {
        damage=(int)Math.Round(damage*0.5);
      }
    }
    // Tinted Lens
    if (skill>=PBTrainerAI.bestSkill) {
      if (attacker.hasWorkingAbility(Abilities.TINTED_LENS) && typemod<8) {
        damage=(int)Math.Round(damage*2.0);
      }
    }
    // Friend Guard
    if (skill>=PBTrainerAI.bestSkill) {
      if (opponent.Partner.hasWorkingAbility(Abilities.FRIEND_GUARD)) {
        damage=(int)Math.Round(damage*0.75);
      }
    }
    // Sniper - n/a
    // Solid Rock, Filter
    if (skill>=PBTrainerAI.bestSkill) {
      if ((opponent.hasWorkingAbility(Abilities.SOLID_ROCK) || opponent.hasWorkingAbility(Abilities.FILTER)) &&
         typemod>8) {
        damage=(int)Math.Round(damage*0.75);
      }
    }
    // Final damage-altering items
    if (attacker.hasWorkingItem(Items.METRONOME)) {
      if (attacker.effects.Metronome>4) {
        damage=(int)Math.Round(damage*2.0);
      }
      else {
        float met=1.0f+attacker.effects.Metronome*0.2f;
        damage=(int)Math.Round(damage*met);
      }
    }
    if (attacker.hasWorkingItem(Items.EXPERT_BELT) && typemod>8) {
      damage=(int)Math.Round(damage*1.2);
    }
    if (attacker.hasWorkingItem(Items.LIFE_ORB)) {
      damage=(int)Math.Round(damage*1.3);
    }
    if (typemod>8 && skill>=PBTrainerAI.highSkill) {
      if ((opponent.hasWorkingItem(Items.CHOPLE_BERRY) && type == Types.FIGHTING) ||
         (opponent.hasWorkingItem(Items.COBA_BERRY) && type == Types.FLYING) ||
         (opponent.hasWorkingItem(Items.KEBIA_BERRY) && type == Types.POISON) ||
         (opponent.hasWorkingItem(Items.SHUCA_BERRY) && type == Types.GROUND) ||
         (opponent.hasWorkingItem(Items.CHARTI_BERRY) && type == Types.ROCK) ||
         (opponent.hasWorkingItem(Items.TANGA_BERRY) && type == Types.BUG) ||
         (opponent.hasWorkingItem(Items.KASIB_BERRY) && type == Types.GHOST) ||
         (opponent.hasWorkingItem(Items.BABIRI_BERRY) && type == Types.STEEL) ||
         (opponent.hasWorkingItem(Items.OCCA_BERRY) && type == Types.FIRE) ||
         (opponent.hasWorkingItem(Items.PASSHO_BERRY) && type == Types.WATER) ||
         (opponent.hasWorkingItem(Items.RINDO_BERRY) && type == Types.GRASS) ||
         (opponent.hasWorkingItem(Items.WACAN_BERRY) && type == Types.ELECTRIC) ||
         (opponent.hasWorkingItem(Items.PAYAPA_BERRY) && type == Types.PSYCHIC) ||
         (opponent.hasWorkingItem(Items.YACHE_BERRY) && type == Types.ICE) ||
         (opponent.hasWorkingItem(Items.HABAN_BERRY) && type == Types.DRAGON) ||
         (opponent.hasWorkingItem(Items.COLBUR_BERRY) && type == Types.DARK)) {
        damage=(int)Math.Round(damage*0.5);
      }
    }
    if (skill>=PBTrainerAI.highSkill) {
      if (opponent.hasWorkingItem(Items.CHILAN_BERRY) && type == Types.NORMAL) {
        damage=(int)Math.Round(damage*0.5);
      }
    }
    // pbModifyDamage - TODO
    // "AI-specific calculations below"
    // Increased critical hit rates
    if (skill>=PBTrainerAI.mediumSkill) {
      int c=0;
      c+=attacker.effects.FocusEnergy;
      //if (move.hasHighCriticalRate) c+=1;
      if (Game.MoveMetaData[move.MoveId].CritRate > 0) c+=1;
      if ((attacker.inHyperMode()) && move.Type == Types.SHADOW) c+=1; //rescue false
      if (attacker.Species == Pokemons.CHANSEY && 
              attacker.hasWorkingItem(Items.LUCKY_PUNCH)) c+=2;
      if (attacker.Species == Pokemons.FARFETCHD && 
              attacker.hasWorkingItem(Items.STICK)) c+=2;
      if (attacker.hasWorkingAbility(Abilities.SUPER_LUCK)) c+=1;
      if (attacker.hasWorkingItem(Items.SCOPE_LENS)) c+=1;
      if (attacker.hasWorkingItem(Items.RAZOR_CLAW)) c+=1;
      if (c>4) c=4;
      basedamage+=(basedamage*0.1f*c);
    }
    return (int)damage;
    //return 0;
  }

  public int pbRoughAccuracy(Attack.Move move, Pokemon attacker, Pokemon opponent, int skill) {
    float accuracy=0;
    // Get base accuracy
    int baseaccuracy=move.Accuracy??0;
    if (skill>=PBTrainerAI.mediumSkill) {
      if (pbWeather()==Weather.SUNNYDAY &&
         (move.Effect==Attack.Data.Effects.x099 || move.Effect==Attack.Data.Effects.x14E)) { // Thunder, Hurricane
        accuracy=50;
      }
    }
    // Accuracy stages
    int accstage=attacker.stages[(int)Stats.ACCURACY];
    if (opponent.hasWorkingAbility(Abilities.UNAWARE)) accstage=0;
    accuracy=(accstage>=0) ? (accstage+3)*100.0f/3f : 300.0f/(3-accstage);
    int evastage=opponent.stages[(int)Stats.EVASION];
    if (@field.Gravity>0) evastage-=2;
    if (evastage<-6) evastage=-6;
    if (opponent.effects.Foresight ||
                  opponent.effects.MiracleEye ||
                  move.Effect==Attack.Data.Effects.x130 || // Chip Away
                  attacker.hasWorkingAbility(Abilities.UNAWARE)) evastage=0;
    float evasion=(evastage>=0) ? (evastage+3)*100.0f/3f : 300.0f/(3f-evastage);
    accuracy*=baseaccuracy/evasion;
    // Accuracy modifiers
    if (skill>=PBTrainerAI.mediumSkill) {
      if (attacker.hasWorkingAbility(Abilities.COMPOUND_EYES)) accuracy*=1.3f;
      if (attacker.hasWorkingAbility(Abilities.VICTORY_STAR)) accuracy*=1.1f;
      if (skill>=PBTrainerAI.highSkill) {
        Pokemon partner=attacker.Partner;
        if (partner.IsNotNullOrNone() && partner.hasWorkingAbility(Abilities.VICTORY_STAR)) accuracy*=1.1f;
      }
      if (attacker.effects.MicleBerry) accuracy*=1.2f;
      if (attacker.hasWorkingItem(Items.WIDE_LENS)) accuracy*=1.1f;
      if (skill>=PBTrainerAI.highSkill) {
        if (attacker.hasWorkingAbility(Abilities.HUSTLE) &&
                         move.Power>0 //&& //ToDo: here
                         //move.pbIsPhysical(move.pbType(move.Type,attacker,opponent))) accuracy*=0.8f;
                         ) accuracy*=0.8f;
      }
      if (skill>=PBTrainerAI.bestSkill) {
        if (opponent.hasWorkingAbility(Abilities.WONDER_SKIN) &&
                       move.Power==0 &&
                       attacker.IsOpposing(opponent.Index)) accuracy/=2;
        if (opponent.hasWorkingAbility(Abilities.TANGLED_FEET) &&
                         opponent.effects.Confusion>0) accuracy/=1.2f;
        if (pbWeather()==Weather.SANDSTORM &&
                         opponent.hasWorkingAbility(Abilities.SAND_VEIL)) accuracy/=1.2f;
        if (pbWeather()==Weather.HAIL &&
                         opponent.hasWorkingAbility(Abilities.SNOW_CLOAK)) accuracy/=1.2f;
      }
      if (skill>=PBTrainerAI.highSkill) {
        if (opponent.hasWorkingItem(Items.BRIGHT_POWDER)) accuracy/=1.1f;
        if (opponent.hasWorkingItem(Items.LAX_INCENSE)) accuracy/=1.1f;
      }
    }
    if (accuracy>100) accuracy=100;
    // Override accuracy
    if (move.Accuracy==0  ) accuracy=125;	// Doesn't do accuracy check (always hits)
    if (move.Effect==Attack.Data.Effects.x17D) accuracy=125;	// Swift
    if (skill>=PBTrainerAI.mediumSkill) {
      if (opponent.effects.LockOn>0 &&
                      opponent.effects.LockOnPos==attacker.Index) accuracy=125;
      if (skill>=PBTrainerAI.highSkill) {
        if (attacker.hasWorkingAbility(Abilities.NO_GUARD) ||
                        opponent.hasWorkingAbility(Abilities.NO_GUARD)) accuracy=125;
      }
      if (opponent.effects.Telekinesis>0) accuracy=125;
      switch (pbWeather()) {
      case Weather.HAIL:
        if (move.Effect==Attack.Data.Effects.x105) accuracy=125;	// Blizzard
        break;
      case Weather.RAINDANCE:
        if (move.Effect==Attack.Data.Effects.x099 || move.Effect==Attack.Data.Effects.x14E) accuracy=125;	// Thunder, Hurricane
        break;
      }
      if (move.Effect==Attack.Data.Effects.x027) {		// OHKO moves
        accuracy=(move.Accuracy??0)+attacker.Level-opponent.Level;
        if (opponent.hasWorkingAbility(Abilities.STURDY)) accuracy=0;
        if (opponent.Level>attacker.Level) accuracy=0;
      }
    }
    return (int)accuracy;
  }
#endregion 

/// <summary>
/// Choose a move to use.
/// </summary>
/// <param name="index"></param>
  public void pbChooseMoves(int index) {
    Pokemon attacker=@battlers[index];
    int[] scores=new int[] { 0, 0, 0, 0 };
    int[] targets=null;
    List<int> myChoices=new List<int>();
    int totalscore=0;
    int target=-1;
    int skill=0;
    bool wildbattle=@opponent==null && isOpposing(index);
    if (wildbattle) {		// If wild battle
      for (int i = 0; i < 4; i++) {
        if (CanChooseMove(index,i,false)) {
          scores[i]=100;
          myChoices.Add(i);
          totalscore+=100;
        }
      }
    }
    else {
      skill=Game.TrainerMetaData[pbGetOwner(attacker.Index).trainertype].SkillLevel; //|| 0;
      Pokemon opponent=attacker.pbOppositeOpposing;
      if (@doublebattle && !opponent.isFainted() && !opponent.Partner.isFainted()) {
        // Choose a target and move.  Also care about partner.
        Pokemon otheropp=opponent.Partner;
        List<int[]> scoresAndTargets=new List<int[]>();
        targets=new int[] { -1, -1, -1, -1 };
        for (int i = 0; i < 4; i++) {
          if (CanChooseMove(index,i,false)) {
            int score1=pbGetMoveScore(attacker.moves[i],attacker,opponent,skill);
            int score2=pbGetMoveScore(attacker.moves[i],attacker,otheropp,skill);
            //if ((attacker.moves[i].Targets&(Attack.Data.Targets)0x20)!=0) {		// Target's user's side
            if (attacker.moves[i].Targets == Attack.Data.Targets.OPPONENTS_FIELD) {		// Target's user's side
              if (attacker.Partner.isFainted()) {		// No partner
                score1*=5/3;
                score2*=5/3;
              }
              else {
                // If this move can also target the partner, get the partner's
                // score too
                int s=pbGetMoveScore(attacker.moves[i],attacker,attacker.Partner,skill);
                if (s>=140) {		// Highly effective
                  score1*=1/3;
                  score2*=1/3;
                } else if (s>=100) {		// Very effective
                  score1*=2/3;
                  score2*=2/3;
                } else if (s>=40) {		// Less effective
                  score1*=4/3;
                  score2*=4/3;
                } else { // Hardly effective
                  score1*=5/3;
                  score2*=5/3;
                }
              }
            }
            myChoices.Add(i);
            scoresAndTargets.Add(new int[] { i * 2, i, score1, opponent.Index });
            scoresAndTargets.Add(new int[] { i * 2 + 1, i, score2, otheropp.Index });
          }
        }
        scoresAndTargets.OrderBy(a => a[2]).ThenBy(b => b[0]); //.Sort(a,b);
        //   if (a[2]==b[2]) 		// if scores are equal
        //     a[0]!=b[0]; // sort by index (for stable comparison)
        //   } else {
        //     b[2]!=a[2];
        //   }
        //);
        for (int i = 0; i < scoresAndTargets.Count; i++) {
          int idx=scoresAndTargets[i][1];
          int thisScore=scoresAndTargets[i][2];
          if (thisScore>0) {
            if (scores[idx]==0 || ((scores[idx]==thisScore && Core.Rand.Next(10)<5) ||
               (scores[idx]!=thisScore && Core.Rand.Next(10)<3))) {
              scores[idx]=thisScore;
              targets[idx]=scoresAndTargets[i][3];
            }
          }
        }
        for (int i = 0; i < 4; i++) {
          if (scores[i]<0) scores[i]=0;
          totalscore+=scores[i];
        }
      }
      else {
        // Choose a move. There is only 1 opposing Pokémon.
        if (@doublebattle && opponent.isFainted()) {
          opponent=opponent.Partner;
        }
        for (int i = 0; i < 4; i++) {
          if (CanChooseMove(index,i,false)) {
            scores[i]=pbGetMoveScore(attacker.moves[i],attacker,opponent,skill);
            myChoices.Add(i);
          }
          if (scores[i]<0) scores[i]=0;
          totalscore+=scores[i];
        }
      }
    }
    int maxscore=0;
    for (int i = 0; i < 4; i++) {
      if (scores[i]>maxscore) maxscore=scores[i]; //&& scores[i]
    }
    // Minmax choices depending on AI
    if (!wildbattle && skill>=PBTrainerAI.mediumSkill) {
      float threshold=(skill>=PBTrainerAI.bestSkill) ? 1.5f : (skill>=PBTrainerAI.highSkill) ? 2 : 3;
      int newscore=(skill>=PBTrainerAI.bestSkill) ? 5 : (skill>=PBTrainerAI.highSkill) ? 10 : 15;
      for (int i = 0; i < scores.Length; i++) {
        if (scores[i]>newscore && scores[i]*threshold<maxscore) {
          totalscore-=(scores[i]-newscore);
          scores[i]=newscore;
        }
      }
      maxscore=0;
      for (int i = 0; i < 4; i++) {
        if (scores[i] != null && scores[i]>maxscore) maxscore=scores[i];
      }
    }
    if (Core.INTERNAL) {
      string x=$"[AI] #{attacker.ToString()}'s moves: ";
      int j=0;
      for (int i = 0; i < 4; i++) {
        if (attacker.moves[i].MoveId!=0) {
          if (j>0) x+=", ";
          x+=attacker.moves[i].MoveId.ToString(TextScripts.Name)+"="+scores[i].ToString();
          j+=1;
        }
      }
      GameDebug.Log(x);
    }
    if (!wildbattle && maxscore>100) {
      int stdev=pbStdDev(scores);
      if (stdev>=40 && Core.Rand.Next(10)!=0) {
        // If standard deviation is 40 or more,
        // there is a highly preferred move. Choose it.
        List<int> preferredMoves=new List<int>();
        for (int i = 0; i < 4; i++) {
          if (attacker.moves[i].MoveId!=0 && (scores[i]>=maxscore*0.8 || scores[i]>=200)) {
            preferredMoves.Add(i);
            if (scores[i]==maxscore) preferredMoves.Add(i);	// Doubly prefer the best move
          }
        }
        if (preferredMoves.Count>0) {
          int i=preferredMoves[Core.Rand.Next(preferredMoves.Count)];
          GameDebug.Log($"[AI] Prefer #{attacker.moves[i].MoveId.ToString(TextScripts.Name)}");
          pbRegisterMove(index,i,false);
          if (targets != null) target=targets[i];
          if (@doublebattle && target>=0) {
            pbRegisterTarget(index,target);
          }
          return;
        }
      }
    }
    if (!wildbattle && attacker.turncount>=0) {
      bool badmoves=false;
      if (((maxscore<=20 && attacker.turncount>2) ||
         (maxscore<=30 && attacker.turncount>5)) && Core.Rand.Next(10)<8) {
        badmoves=true;
      }
      if (totalscore<100 && attacker.turncount>1) {
        badmoves=true;
        int movecount=0;
        for (int i = 0; i < 4; i++) {
          if (attacker.moves[i].MoveId!=0) {
            if (scores[i]>0 && attacker.moves[i].Power>0) {
              badmoves=false;
            }
            movecount+=1;
          }
        }
        badmoves=badmoves && Core.Rand.Next(10)!=0;
      }
      if (badmoves) {
        // Attacker has terrible moves, try switching instead
        if (pbEnemyShouldWithdrawEx(index,true)) {
          if (Core.INTERNAL) {
            GameDebug.Log($"[AI] Switching due to terrible moves");
            //GameDebug.Log($@"{index},{@choices[index][0]},{@choices[index][1]},
            GameDebug.Log($@"{index},{@choices[index].Action},{@choices[index].Index},
               {pbCanChooseNonActive(index)},
               {@battlers[index].NonActivePokemonCount}");
          }
          return;
        }
      }
    }
    if (maxscore<=0) {
      // If all scores are 0 or less, choose a move at random
      if (myChoices.Count>0) {
        pbRegisterMove(index,myChoices[Core.Rand.Next(myChoices.Count)],false);
      }
      else {
        pbAutoChooseMove(index);
      }
    }
    else {
      int randnum=Core.Rand.Next(totalscore);
      int cumtotal=0;
      for (int i = 0; i < 4; i++) {
        if (scores[i]>0) {
          cumtotal+=scores[i];
          if (randnum<cumtotal) {
            pbRegisterMove(index,i,false);
            if (targets!=null) target=targets[i];
            break;
          }
        }
      }
    }
    //if (@choices[index][2]) GameDebug.Log($"[AI] Will use #{@choices[index][2].Name}");
    if (@choices[index].Move.IsNotNullOrNone()) GameDebug.Log($"[AI] Will use #{@choices[index].Move.Name}");
    if (@doublebattle && target>=0) {
      pbRegisterTarget(index,target);
    }
  }

/// <summary>
/// Decide whether the opponent should Mega Evolve their Pokémon.
/// </summary>
/// <param name="ndex"></param>
/// <returns></returns>
  public bool pbEnemyShouldMegaEvolve (int index) {
    // Simple "always should if possible"
    return pbCanMegaEvolve(index);
  }

#region Decide whether the opponent should use an item on the Pokémon.
  public bool pbEnemyShouldUseItem (int index) {
    Items item=pbEnemyItemToUse(index);
    if (item>0) {
      pbRegisterItem(index,item,null);
      return true;
    }
    return false;
  }

  public bool pbEnemyItemAlreadyUsed (int index,Items item,Items[] items) {
    //if (@choices[1][0]==3 && @choices[1][1]==item) {
    if (@choices[1].Action==(ChoiceAction)3 && @choices[1].Index==(int)item) {
      int qty=0;
      foreach (var i in items) {
        if (i==item) qty+=1;
      }
      if (qty<=1) return true;
    }
    return false;
  }

  public Items pbEnemyItemToUse(int index) {
    if (!@internalbattle) return 0;
    Items[] items=pbGetOwnerItems(index);
    if (items == null) return 0;
    Pokemon battler=@battlers[index];
    if (battler.isFainted() ||
                battler.effects.Embargo>0) return 0;
    bool hashpitem=false;
    foreach (var i in items) {
      if (pbEnemyItemAlreadyUsed(index,i,items)) continue;
      if (i == Items.POTION ||
         i == Items.SUPER_POTION || 
         i == Items.HYPER_POTION || 
         i == Items.MAX_POTION ||
         i == Items.FULL_RESTORE ) {
        hashpitem=true;
      }
    }
    foreach (var i in items) {
      if (pbEnemyItemAlreadyUsed(index,i,items)) continue;
      if (i == Items.FULL_RESTORE) {
        if (battler.HP<=battler.TotalHP/4) return i;
        if (battler.HP<=battler.TotalHP/2 && Core.Rand.Next(10)<3) return i;
        if (battler.HP<=battler.TotalHP*2/3 &&
                    (battler.Status>0 || battler.effects.Confusion>0) &&
                    Core.Rand.Next(10)<3) return i;
      } else if (i == Items.POTION || 
         i == Items.SUPER_POTION || 
         i == Items.HYPER_POTION || 
         i == Items.MAX_POTION) {
        if (battler.HP<=battler.TotalHP/4) return i;
        if (battler.HP<=battler.TotalHP/2 && Core.Rand.Next(10)<3) return i;
      } else if (i == Items.FULL_HEAL) {
        if (!hashpitem &&
                    (battler.Status>0 || battler.effects.Confusion>0)) return i;
      } else if (i == Items.X_ATTACK ||
            i == Items.X_DEFENSE ||
            i == Items.X_SPEED ||
            i == Items.X_SP_ATK ||
            i == Items.X_SP_DEF ||
            i == Items.X_ACCURACY) {
        Stats? stat=null;//0;
        if (i == Items.X_ATTACK) stat=Stats.ATTACK;
        if (i == Items.X_DEFENSE) stat=Stats.DEFENSE;
        if (i == Items.X_SPEED) stat=Stats.SPEED;
        if (i == Items.X_SP_ATK) stat=Stats.SPATK;
        if (i == Items.X_SP_DEF) stat=Stats.SPDEF;
        if (i == Items.X_ACCURACY) stat=Stats.ACCURACY;
        if (stat>0 && !battler.pbTooHigh(stat.Value)) {
          if (Core.Rand.Next(10)<3-battler.stages[(int)stat]) return i;
        }
      }
    }
    return 0;
  }
#endregion

#region Decide whether the opponent should switch Pokémon.
  public virtual bool pbEnemyShouldWithdraw (int index) {
//    if (Core.INTERNAL && !pbIsOpposing(index)) {
//      return pbEnemyShouldWithdrawOld(index);
//    }
    return pbEnemyShouldWithdrawEx(index,false);
  }

  public bool pbEnemyShouldWithdrawEx (int index,bool alwaysSwitch) {
    if (@opponent == null) return false;
    bool shouldswitch=alwaysSwitch;
    bool typecheck=false;
    int batonpass=-1;
    Types movetype=Types.NONE;
    int skill=Game.TrainerMetaData[pbGetOwner(index).trainertype].SkillLevel;// || 0;
    if (@opponent!= null && !shouldswitch && @battlers[index].turncount>0) {
      if (skill>=PBTrainerAI.highSkill) {
        Pokemon opponent=@battlers[index].pbOppositeOpposing;
        if (opponent.isFainted()) opponent=opponent.Partner;
        if (!opponent.isFainted() && opponent.lastMoveUsed>0 && 
           Math.Abs(opponent.Level-@battlers[index].Level)<=6) {
          Attack.Data.MoveData move=Game.MoveData[opponent.lastMoveUsed];
          float typemod=pbTypeModifier(move.Type,@battlers[index],@battlers[index]);
          movetype=move.Type;
          if (move.Power>70 && typemod>8) {
            shouldswitch=(Core.Rand.Next(100)<30);
          } else if (move.Power>50 && typemod>8) {
            shouldswitch=(Core.Rand.Next(100)<20);
          }
        }
      }
    }
    if (!CanChooseMove(index,0,false) &&
       !CanChooseMove(index,1,false) &&
       !CanChooseMove(index,2,false) &&
       !CanChooseMove(index,3,false) &&
       //@battlers[index].turncount &&
       @battlers[index].turncount>5) {
      shouldswitch=true;
    }
    if (skill>=PBTrainerAI.highSkill && @battlers[index].effects.PerishSong!=1) {
      for (int i = 0; i < 4; i++) {
        Attack.Move move=@battlers[index].moves[i];
        if (move.MoveId!=0 && CanChooseMove(index,i,false) &&
          move.Effect==Attack.Data.Effects.x080) { // Baton Pass
          batonpass=i;
          break;
        }
      }
    }
    if (skill>=PBTrainerAI.highSkill) {
      if (@battlers[index].Status==Status.POISON &&
         @battlers[index].StatusCount>0) {
        float toxicHP=(@battlers[index].TotalHP/16);
        float nextToxicHP=toxicHP*(@battlers[index].effects.Toxic+1);
        if (nextToxicHP>=@battlers[index].HP &&
           toxicHP<@battlers[index].HP && Core.Rand.Next(100)<80) {
          shouldswitch=true;
        }
      }
    }
    if (skill>=PBTrainerAI.mediumSkill) {
      if (@battlers[index].effects.Encore>0) {
        int scoreSum=0;
        int scoreCount=0;
        Pokemon attacker=@battlers[index];
        int encoreIndex=@battlers[index].effects.EncoreIndex;
        if (!attacker.pbOpposing1.isFainted()) {
          scoreSum+=pbGetMoveScore(attacker.moves[encoreIndex],
             attacker,attacker.pbOpposing1,skill);
          scoreCount+=1;
        }
        if (!attacker.pbOpposing2.isFainted()) {
          scoreSum+=pbGetMoveScore(attacker.moves[encoreIndex],
             attacker,attacker.pbOpposing2,skill);
          scoreCount+=1;
        }
        if (scoreCount>0 && scoreSum/scoreCount<=20 && Core.Rand.Next(10)<8) {
          shouldswitch=true;
        }
      }
    }
    if (skill>=PBTrainerAI.highSkill) {
      if (!@doublebattle && !@battlers[index].pbOppositeOpposing.isFainted() ) {
        Pokemon opp=@battlers[index].pbOppositeOpposing;
        if ((opp.effects.HyperBeam>0 ||
           (opp.hasWorkingAbility(Abilities.TRUANT) &&
           opp.effects.Truant)) && Core.Rand.Next(100)<80) {
          shouldswitch=false;
        }
      }
    }
    if (@rules.Contains("suddendeath")) {
      if (@battlers[index].HP<=(@battlers[index].TotalHP/4) && Core.Rand.Next(10)<3 && 
         @battlers[index].turncount>0) {
        shouldswitch=true;
      } else if (@battlers[index].HP<=(@battlers[index].TotalHP/2) && Core.Rand.Next(10)<8 && 
         @battlers[index].turncount>0) {
        shouldswitch=true;
      }
    }
    if (@battlers[index].effects.PerishSong==1) {
      shouldswitch=true;
    }
    if (shouldswitch) {
      List<int> list=new List<int>();
      //Combat.Pokemon[] party=pbParty(index);
      Combat.Pokemon[] party=battlers.Where(b => (b.Index % 2) == (index % 2)).ToArray();
      for (int i = 0; i < party.Length; i++) {
        if (pbCanSwitch(index,i,false)) {
          // If perish count is 1, it may be worth it to switch
          // even with Spikes, since Perish Song's effect will end
          if (@battlers[index].effects.PerishSong!=1) {
            // Will contain effects that recommend against switching
            int spikes=@battlers[index].OwnSide.Spikes;
            if ((spikes==1 && party[i].HP<=(party[i].TotalHP/8)) ||
               (spikes==2 && party[i].HP<=(party[i].TotalHP/6)) ||
               (spikes==3 && party[i].HP<=(party[i].TotalHP/4))) {
              if (!party[i].hasType(Types.FLYING) &&
                 !party[i].hasWorkingAbility(Abilities.LEVITATE)) {
                // Don't switch to this if too little HP
                continue;
              }
            }
          }
          if (movetype>=0 && pbTypeModifier(movetype,@battlers[index],@battlers[index])==0) {
            int weight=65;
            if (pbTypeModifier2(party[i],@battlers[index].pbOppositeOpposing)>8) {
              // Greater weight if new Pokemon's type is effective against opponent
              weight=85;
            }
            if (Core.Rand.Next(100)<weight) {
              //list.unshift(i); // put this Pokemon first
              list = list.Where(x => x == i).Concat(list.Where(x => x != i)).ToList(); // put this Pokemon first
            }
          } else if (movetype>=0 && pbTypeModifier(movetype,@battlers[index],@battlers[index])<8) {
            int weight=40;
            if (pbTypeModifier2(party[i],@battlers[index].pbOppositeOpposing)>8) {
              // Greater weight if new Pokemon's type is effective against opponent
              weight=60;
            }
            if (Core.Rand.Next(100)<weight) {
              //list.unshift(i); // put this Pokemon first
              list = list.Where(x => x == i).Concat(list.Where(x => x != i)).ToList(); // put this Pokemon first
            }
          }
          else {
            list.Add(i); // put this Pokemon last
          }
        }
      }
      if (list.Count>0) {
        if (batonpass!=-1) {
          if (!pbRegisterMove(index,batonpass,false)) {
            return pbRegisterSwitch(index,list[0]);
          }
          return true;
        }
        else {
          return pbRegisterSwitch(index,list[0]);
        }
      }
    }
    return false;
  }

  public int pbDefaultChooseNewEnemy(int index,Pokemon[] party) {
    List<int> enemies=new List<int>();
    for (int i = 0; i < party.Length-1; i++) {
      if (pbCanSwitchLax(index,i,false)) {
        enemies.Add(i);
      }
    }
    if (enemies.Count>0) {
      return pbChooseBestNewEnemy(index,party,enemies.ToArray());
    }
    return -1;
  }

  public int pbChooseBestNewEnemy(int index,Pokemon[] party,int[] enemies) {
    if (enemies == null || enemies.Length==0) return -1;
    //if (Game.GameData.PokemonTemp == null) Game.GameData.PokemonTemp=new PokemonTemp();
    Pokemon o1=@battlers[index].pbOpposing1;
    Pokemon o2=@battlers[index].pbOpposing2;
    if (o1.IsNotNullOrNone() && o1.isFainted()) o1=null;
    if (o2.IsNotNullOrNone() && o2.isFainted()) o2=null;
    int best=-1;
    int bestSum=0;
    foreach (int e in enemies) {
      Pokemon pkmn=party[e];
      int sum=0;
      foreach (var move in pkmn.moves) {
        if (move.MoveId==0) continue;
        Attack.Data.MoveData md=Game.MoveData[move.MoveId];
        if (md.Power==0) continue;
        if (o1.IsNotNullOrNone()) {
          //ToDo: uncomment below
          //sum+=md.Type.GetCombinedEffectiveness(o1.Type1,o1.Type2,o1.effects.Type3);
        }
        if (o2.IsNotNullOrNone()) {
          //sum+=md.Type.GetCombinedEffectiveness(o2.Type1,o2.Type2,o2.effects.Type3);
        }
      }
      if (best==-1 || sum>bestSum) {
        best=e;
        bestSum=sum;
      }
    }
    return best;
  }
#endregion

/// <summary>
/// Choose an action.
/// </summary>
/// <param name="index"></param>
  public void pbDefaultChooseEnemyCommand(int index) {
    if (!CanShowFightMenu(index)) {
      if (pbEnemyShouldUseItem(index)) return;
      if (pbEnemyShouldWithdraw(index)) return;
      pbAutoChooseMove(index);
      return;
    }
    else {
      if (pbEnemyShouldUseItem(index)) return;
      if (pbEnemyShouldWithdraw(index)) return;
      if (pbAutoFightMenu(index)) return;
      if (pbEnemyShouldMegaEvolve(index)) pbRegisterMegaEvolution(index);
      pbChooseMoves(index);
    }
  }

#region Other functions.
  public bool pbDbgPlayerOnly (int idx) {
    //ToDo:Uncomment below
    //if (!Core.INTERNAL) return true;
    //if (idx.respond_to("index"))
    //  return pbOwnedByPlayer(idx.Index);
    return pbOwnedByPlayer(idx);
  }

  public int pbStdDev(int[] scores) {
    int n=0;
    int sum=0;
    //scores.ForEach{ s => sum+=s; n+=1 );
    foreach(int s in scores) { sum += s; n += 1; }
    if (n==0) return 0;
    float mean=(float)sum/(float)n;
    float varianceTimesN=0;
    for (int i = 0; i < scores.Length; i++) {
      if (scores[i]>0) {
        float deviation=(float)scores[i]-mean;
        varianceTimesN+=deviation*deviation;
      }
    }
    // Using population standard deviation 
    // [(n-1) makes it a sample std dev, would be 0 with only 1 sample]
    return (int)Math.Sqrt((double)varianceTimesN/(double)n);
  }
#endregion
}
}