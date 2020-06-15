using PokemonUnity.Inventory;
using PokemonUnity.Combat.Data;
using PokemonUnity.Character;
using System;
using System.Collections.Generic;
using System.Linq;

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
// ###############################################################################
// Get a score for each move being considered (trainer-owned Pokémon only).
// Moves with higher scores are more likely to be chosen.
// ###############################################################################
  public int pbGetMoveScore(Move move,Pokemon attacker,Pokemon opponent,int skill=100) {
    if (skill<PBTrainerAI.minimumSkill) skill=PBTrainerAI.minimumSkill;
    float score=100;
    if (!opponent.IsNotNullOrNone()) opponent=attacker.pbOppositeOpposing;
    if (opponent.IsNotNullOrNone() && opponent.isFainted()) opponent=opponent.Partner;
    #region switch variables
    bool hasspecialattack = false;
    bool hasphysicalattack = false;
    bool hasdamagingattack = false;
    bool hasDamagingMove = false;
    bool foundmove = false;
    bool canattract = false;
    int avg = 0; int count = 0;
    int aspeed = 0; int ospeed = 0;
    #endregion
// #### Alter score depending on the move's function code #######################
    /*switch (move.Effect) {
    case 0x00: // No extra effect
      break;
    case 0x01:
      score-=95;
      if (skill>=PBTrainerAI.highSkill) {
        score=0;
      }
      break;
    case 0x02: // Struggle
      break;
    case 0x03:
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
            if (movedata.Effect==0xB4 ||		// Sleep Talk
               movedata.Effect==0x11) {    // Snore
              score-=50;
              break;
            }
          }
        }
      }
      else {
        if (skill>=PBTrainerAI.mediumSkill) {
          if (move.BaseDamage==0) score-=90;
        }
      }
      break;
    case 0x04:
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
            if (movedata.Effect==0xB4 ||		// Sleep Talk
               movedata.Effect==0x11) {    // Snore
              score-=50;
              break;
            }
          }
        }
      }
      break;
    case 0x05: case 0x06: case 0xBE:
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
          if (move.BaseDamage==0) score-=90;
        }
      }
      break;
    case 0x07: case 0x08: case 0x09: case 0xC5:
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
          if (move.BaseDamage==0) score-=90;
        }
      }
      break;
    case 0x0A: case 0x0B: case 0xC6:
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
          if (move.BaseDamage==0) score-=90;
        }
      }
      break;
    case 0x0C: case 0x0D: case 0x0E:
      if (opponent.pbCanFreeze(attacker,false)) {
        score+=30;
        if (skill>=PBTrainerAI.highSkill) {
          if (opponent.hasWorkingAbility(Abilities.MARVEL_SCALE)) score-=20;
        }
      }
      else {
        if (skill>=PBTrainerAI.mediumSkill) {
          if (move.BaseDamage==0) score-=90;
        }
      }
      break;
    case 0x0F:
      score+=30;
      if (skill>=PBTrainerAI.highSkill) {
        if (!opponent.hasWorkingAbility(Abilities.INNER_FOCUS) &&
                     opponent.effects.Substitute==0) score+=30;
      }
      break;
    case 0x10:
      if (skill>=PBTrainerAI.highSkill) {
        if (!opponent.hasWorkingAbility(Abilities.INNER_FOCUS) &&
                     opponent.effects.Substitute==0) score+=30;
      }
      if (opponent.effects.Minimize) score+=30;
      break;
    case 0x11:
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
    case 0x12:
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
    case 0x13: case 0x14: case 0x15:
      if (opponent.pbCanConfuse(attacker,false)) {
        score+=30;
      }
      else {
        if (skill>=PBTrainerAI.mediumSkill) {
          if (move.BaseDamage==0) score-=90;
        }
      }
      break;
    case 0x16:
      canattract=true;
      agender=attacker.gender;
      ogender=opponent.gender;
      if (agender==2 || ogender==2 || agender==ogender) {
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
    case 0x17:
      if (opponent.Status==0) score+=30;
      break;
    case 0x18:
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
    case 0x19:
      Pokemon[] party=pbParty(attacker.Index);
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
    case 0x1A:
      if (attacker.OwnSide.Safeguard>0) {
        score-=80 ;
      } else if (attacker.Status!=0) {
        score-=40;
      }
      else {
        score+=30;
      }
      break;
    case 0x1B:
      if (attacker.Status==0) {
        score-=90;
      }
      else {
        score+=40;
      }
      break;
    case 0x1C:
      if (move.BaseDamage==0) {
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
    case 0x1D: case 0x1E: case 0xC8:
      if (move.BaseDamage==0) {
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
    case 0x1F:
      if (move.BaseDamage==0) {
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
    case 0x20:
      if (move.BaseDamage==0) {
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
    case 0x21:
      foundmove=false;
      for (int i = 0; i < 4; i++) {
        if (attacker.moves[i].Type == Types.ELECTRIC &&
           attacker.moves[i].Power>0) {
          foundmove=true;
          break;
        }
      }
      if (move.BaseDamage==0) {
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
    case 0x22:
      if (move.BaseDamage==0) {
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
    case 0x23:
      if (move.BaseDamage==0) {
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
    case 0x24:
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
    case 0x25:
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
    case 0x26:
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
    case 0x27: case 0x28:
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
        if (move.Effect==0x28) {		// Growth
          if (pbWeather()==Weather.SUNNYDAY) score+=20;
        }
      }
      break;
    case 0x29:
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
    case 0x2A:
      if (attacker.pbTooHigh(Stats.DEFENSE) &&
         attacker.pbTooHigh(Stats.SPDEF)) {
        score-=90;
      }
      else {
        score-=attacker.stages[(int)Stats.DEFENSE]*10;
        score-=attacker.stages[(int)Stats.SPDEF]*10;
      }
      break;
    case 0x2B:
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
    case 0x2C:
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
    case 0x2D:
      if (attacker.stages[(int)Stats.ATTACK]<0) score+=10;
      if (attacker.stages[(int)Stats.DEFENSE]<0) score+=10;
      if (attacker.stages[(int)Stats.SPEED]<0) score+=10;
      if (attacker.stages[(int)Stats.SPATK]<0) score+=10;
      if (attacker.stages[(int)Stats.SPDEF]<0 ) score+=10;
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
    case 0x2E:
      if (move.BaseDamage==0) {
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
    case 0x2F:
      if (move.BaseDamage==0) {
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
    case 0x30: case 0x31:
      if (move.BaseDamage==0) {
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
    case 0x32:
      if (move.BaseDamage==0) {
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
    case 0x33:
      if (move.BaseDamage==0) {
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
    case 0x34:
      if (move.BaseDamage==0) {
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
    case 0x35:
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
    case 0x36:
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
    case 0x37:
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
        avstat=0;
        avstat-=opponent.stages[(int)Stats.ATTACK];
        avstat-=opponent.stages[(int)Stats.DEFENSE];
        avstat-=opponent.stages[(int)Stats.SPEED];
        avstat-=opponent.stages[(int)Stats.SPATK];
        avstat-=opponent.stages[(int)Stats.SPDEF];
        avstat-=opponent.stages[(int)Stats.ACCURACY];
        avstat-=opponent.stages[(int)Stats.EVASION];
        if (avstat<0) avstat=(int)Math.Floor(avstat/2);	// More chance of getting even better
        score+=avstat*10;
      }
      break;
    case 0x38:
      if (move.BaseDamage==0) {
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
    case 0x39:
      if (move.BaseDamage==0) {
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
    case 0x3A:
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
    case 0x3B:
      avg=attacker.stages[(int)Stats.ATTACK]*10;
      avg+=attacker.stages[(int)Stats.DEFENSE]*10;
      score+=avg/2;
      break;
    case 0x3C:
      avg=attacker.stages[(int)Stats.DEFENSE]*10;
      avg+=attacker.stages[(int)Stats.SPDEF]*10;
      score+=avg/2;
      break;
    case 0x3D:
      avg=attacker.stages[(int)Stats.DEFENSE]*10;
      avg+=attacker.stages[(int)Stats.SPEED]*10;
      avg+=attacker.stages[(int)Stats.SPDEF]*10;
      score+=(int)Math.Floor(avg/3);
      break;
    case 0x3E:
      score+=attacker.stages[(int)Stats.SPEED]*10;
      break;
    case 0x3F:
      score+=attacker.stages[(int)Stats.SPATK]*10;
      break;
    case 0x40:
      if (!opponent.pbCanConfuse(attacker,false)) {
        score-=90;
      }
      else {
        if (opponent.stages[(int)Stats.SPATK]<0) score+=30;
      }
      break;
    case 0x41:
      if (!opponent.pbCanConfuse(attacker,false)) {
        score-=90;
      }
      else {
        if (opponent.stages[(int)Stats.ATTACK]<0) score+=30;
      }
      break;
    case 0x42:
      if (move.BaseDamage==0) {
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
    case 0x43:
      if (move.BaseDamage==0) {
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
    case 0x44:
      if (move.BaseDamage==0) {
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
    case 0x45:
      if (move.BaseDamage==0) {
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
    case 0x46:
      if (move.BaseDamage==0) {
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
    case 0x47:
      if (move.BaseDamage==0) {
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
    case 0x48:
      if (move.BaseDamage==0) {
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
    case 0x49:
      if (move.BaseDamage==0) {
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
    case 0x4A:
      avg=opponent.stages[(int)Stats.ATTACK]*10;
      avg+=opponent.stages[(int)Stats.DEFENSE]*10;
      score+=avg/2;
      break;
    case 0x4B:
      if (move.BaseDamage==0) {
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
    case 0x4C:
      if (move.BaseDamage==0) {
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
    case 0x4D:
      if (move.BaseDamage==0) {
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
    case 0x4E:
      if (attacker.gender==2 || opponent.gender==2 ||
         attacker.gender==opponent.gender ||
         opponent.hasWorkingAbility(Abilities.OBLIVIOUS)) {
        score-=90;
      } else if (move.BaseDamage==0) {
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
    case 0x4F:
      if (move.BaseDamage==0) {
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
    case 0x50:
      if (opponent.effects.Substitute>0) {
        score-=90;
      }
      else {
        anychange=false;
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
    case 0x51:
      if (skill>=PBTrainerAI.mediumSkill) {
        stages=0;
        for (int i = 0; i < 4; i++) {
          battler=@battlers[i];
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
    case 0x52:
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
    case 0x53:
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
    case 0x54:
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
    case 0x55:
      if (skill>=PBTrainerAI.mediumSkill) {
        equal=true;
        foreach (var i in new Stats[] { Stats.ATTACK,Stats.DEFENSE,Stats.SPEED,
                 Stats.SPATK,Stats.SPDEF,Stats.ACCURACY,Stats.EVASION }) {
          stagediff=opponent.stages[(int)i]-attacker.stages[(int)i];
          score+=stagediff*10;
          if (stagediff!=0) equal=false;
        }
        if (equal) score-=80;
      }
      else {
        score-=50;
      }
      break;
    case 0x56:
      if (attacker.OwnSide.Mist>0) score-=80;
      break;
    case 0x57:
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
    case 0x58:
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
    case 0x59:
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
    case 0x5A:
      if (opponent.effects.Substitute>0) {
        score-=90;
      } else if (attacker.HP>=(attacker.HP+opponent.HP)/2) {
        score-=90;
      }
      else {
        score+=40;
      }
      break;
    case 0x5B:
      if (attacker.OwnSide.Tailwind>0) {
        score-=90;
      }
      break;
    case 0x5C:
      Attack.Data.Effects[] blacklist=new Attack.Data.Effects[] {
         0x02,   // Struggle
         0x14,   // Chatter
         0x5C,   // Mimic
         0x5D,   // Sketch
         0xB6    // Metronome
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
    case 0x5D:
      Attack.Data.Effects[] blacklist=new Attack.Data.Effects[] {
         0x02,   // Struggle
         0x14,   // Chatter
         0x5D    // Sketch
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
    case 0x5E:
      if (attacker.Ability == Abilities.MULTITYPE) {
        score-=90;
      }
      else {
        types=[];
        foreach (var i in attacker.moves) {
          if (i.MoveId==@id) continue;
          //if (Types.isPseudoType(i.Type)) continue;
          if (attacker.pbHasType(i.Type)) continue;
          found=false;
          if (!types.Contains(i.Type)) types.Add(i.Type);
        }
        if (types.Length==0) {
          score-=90;
        }
      }
      break;
    case 0x5F:
      if (attacker.Ability == Abilities.MULTITYPE) {
        score-=90;
      } else if (opponent.lastMoveUsed<=0 ||
         PBTypes.isPseudoType(Game.MoveData[opponent.lastMoveUsed].Type)) {
        score-=90;
      }
      else {
        atype=-1;
        foreach (var i in opponent.moves) {
          if (i.MoveId==opponent.lastMoveUsed) {
            atype=i.pbType(move.Type,attacker,opponent); break;
          }
        }
        if (atype<0) {
          score-=90;
        }
        else {
          types=[];
          for (int i = 0; i < PBTypes.maxValue; i++) {
            if (attacker.pbHasType(i)) continue;
            if (PBTypes.getEffectiveness(atype,i)<2 ) types.Add(i);
          }
          if (types.Length==0) {
            score-=90;
          }
        }
      }
      break;
    case 0x60:
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
        type=envtypes[@environment];
        if (attacker.pbHasType(type)) score-=90;
      }
      break;
    case 0x61:
      if (opponent.effects.Substitute>0 ||
         opponent.Ability == Abilities.MULTITYPE) {
        score-=90;
      } else if (opponent.hasType(Types.WATER)) {
        score-=90;
      }
      break;
    case 0x62:
      if (attacker.Ability == Abilities.MULTITYPE) {
        score-=90;
      } else if (attacker.pbHasType(opponent.Type1) &&
         attacker.pbHasType(opponent.Type2) &&
         opponent.pbHasType(attacker.Type1) &&
         opponent.pbHasType(attacker.Type2)) {
        score-=90;
      }
      break;
    case 0x63:
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
    case 0x64:
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
    case 0x65:
      score-=40; // don't prefer this move
      if (skill>=PBTrainerAI.mediumSkill) {
        if (opponent.Ability==0 ||
           attacker.Ability==opponent.Ability ||
           attacker.Ability == Abilities.MULTITYPE ||
           opponent.Ability == Abilities.FLOWERGIFT ||
           opponent.Ability == Abilities.FORECAST ||
           opponent.Ability == Abilities.ILLUSION ||
           opponent.Ability == Abilities.IMPOSTER ||
           opponent.Ability == Abilities.MULTITYPE ||
           opponent.Ability == Abilities.TRACE ||
           opponent.Ability == Abilities.WONDERGUARD ||
           opponent.Ability == Abilities.ZENMODE) {
          score-=90;
        }
      }
      if (skill>=PBTrainerAI.highSkill) {
        if (opponent.Ability == Abilities.TRUANT && 
           attacker.IsOpposing(opponent.Index)) {
          score-=90;
        } else if (opponent.Ability == Abilities.SLOWSTART &&
           attacker.IsOpposing(opponent.Index)) {
          score-=90;
        }
      }
      break;
    case 0x66:
      score-=40; // don't prefer this move
      if (opponent.effects.Substitute>0) {
        score-=90;
      } else if (skill>=PBTrainerAI.mediumSkill) {
        if (attacker.Ability==0 ||
           attacker.Ability==opponent.Ability ||
           opponent.Ability == Abilities.MULTITYPE ||
           opponent.Ability == Abilities.TRUANT ||
           attacker.Ability == Abilities.FLOWERGIFT ||
           attacker.Ability == Abilities.FORECAST ||
           attacker.Ability == Abilities.ILLUSION ||
           attacker.Ability == Abilities.IMPOSTER ||
           attacker.Ability == Abilities.MULTITYPE ||
           attacker.Ability == Abilities.TRACE ||
           attacker.Ability == Abilities.ZENMODE) {
          score-=90;
        }
        if (skill>=PBTrainerAI.highSkill) {
          if (attacker.Ability == Abilities.TRUANT && 
             attacker.IsOpposing(opponent.Index)) {
            score+=90;
          } else if (attacker.Ability == Abilities.SLOWSTART &&
             attacker.IsOpposing(opponent.Index)) {
            score+=90;
          }
        }
      }
      break;
    case 0x67:
      score-=40; // don't prefer this move
      if (skill>=PBTrainerAI.mediumSkill) {
        if ((attacker.Ability==0 && opponent.Ability==0) ||
           attacker.Ability==opponent.Ability ||
           attacker.Ability == Abilities.ILLUSION ||
           opponent.Ability == Abilities.ILLUSION ||
           attacker.Ability == Abilities.MULTITYPE ||
           opponent.Ability == Abilities.MULTITYPE ||
           attacker.Ability == Abilities.WONDERGUARD ||
           opponent.Ability == Abilities.WONDERGUARD) {
          score-=90;
        }
      }
      if (skill>=PBTrainerAI.highSkill) {
        if (opponent.Ability == Abilities.TRUANT && 
           attacker.IsOpposing(opponent.Index)) {
          score-=90;
        } else if (opponent.Ability == Abilities.SLOWSTART &&
          attacker.IsOpposing(opponent.Index)) {
          score-=90;
        }
      }
      break;
    case 0x68:
      if (opponent.effects.Substitute>0 ||
         opponent.effects.GastroAcid) {
        score-=90;
      } else if (skill>=PBTrainerAI.highSkill) {
        if (opponent.Ability == Abilities.MULTITYPE) score-=90;
        if (opponent.Ability == Abilities.SLOWSTART) score-=90;
        if (opponent.Ability == Abilities.TRUANT) score-=90;
      }
      break;
    case 0x69:
      score-=70;
      break;
    case 0x6A:
      if (opponent.HP<=20) {
        score+=80;
      } else if (opponent.Level>=25) {
        score-=80; // Not useful against high-level Pokemon
      }
      break;
    case 0x6B:
      if (opponent.HP<=40) score+=80;
      break;
    case 0x6C:
      score-=50;
      score+=(int)Math.Floor(opponent.HP*100f/opponent.TotalHP);
      break;
    case 0x6D:
      if (opponent.HP<=attacker.Level) score+=80;
      break;
    case 0x6E:
      if (attacker.HP>=opponent.HP) {
        score-=90;
      } else if (attacker.HP*2<opponent.HP) {
        score+=50;
      }
      break;
    case 0x6F:
      if (opponent.HP<=attacker.Level) score+=30;
      break;
    case 0x70:
      if (opponent.hasWorkingAbility(Abilities.STURDY)) score-=90;
      if (opponent.Level>attacker.Level) score-=90;
      break;
    case 0x71:
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
          if (moveData.BaseDamage>0 &&
             (USEMOVECATEGORY && moveData.category==2) ||
             (!USEMOVECATEGORY && PBTypes.isSpecialType(moveData.Type))) {
            score-=60;
          }
        }
      }
      break;
    case 0x72:
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
          if (moveData.BaseDamage>0 &&
             (Core.USEMOVECATEGORY && moveData.category==1) ||
             (!Core.USEMOVECATEGORY && !PBTypes.isSpecialType(moveData.Type))) {
            score-=60;
          }
        }
      }
      break;
    case 0x73:
      if (opponent.effects.HyperBeam>0) score-=90;
      break;
    case 0x74:
      if (!opponent.Partner.isFainted()) score+=10;
      break;
    case 0x75:
      break;
    case 0x76:
      break;
    case 0x77:
      break;
    case 0x78:
      if (skill>=PBTrainerAI.highSkill) {
        if (!opponent.hasWorkingAbility(Abilities.INNER_FOCUS) &&
                     opponent.effects.Substitute==0) score+=30;
      }
      break;
    case 0x79:
      break;
    case 0x7A:
      break;
    case 0x7B:
      break;
    case 0x7C:
      if (opponent.Status==Status.PARALYSIS) score-=20;	// Will cure status
      break;
    case 0x7D:
      if (opponent.Status==Status.SLEEP &&	// Will cure status
                   opponent.StatusCount>1) score-=20;
      break;
    case 0x7E:
      break;
    case 0x7F:
      break;
    case 0x80:
      break;
    case 0x81:
      int attspeed=pbRoughStat(attacker,Stats.SPEED,skill);
      int oppspeed=pbRoughStat(opponent,Stats.SPEED,skill);
      if (oppspeed>attspeed) score+=30;
      break;
    case 0x82:
      if (@doublebattle) score+=20;
      break;
    case 0x83:
      if (skill>=PBTrainerAI.mediumSkill) {
        if (@doublebattle && !attacker.Partner.isFainted() &&
                     attacker.Partner.pbHasMove(move.MoveId)) score+=20;
      }
      break;
    case 0x84:
      int attspeed=pbRoughStat(attacker,Stats.SPEED,skill);
      int oppspeed=pbRoughStat(opponent,Stats.SPEED,skill);
      if (oppspeed>attspeed) score+=30;
      break;
    case 0x85:
    case 0x86:
    case 0x87:
    case 0x88:
    case 0x89:
    case 0x8A:
    case 0x8B:
    case 0x8C:
    case 0x8D:
    case 0x8E:
    case 0x8F:
    case 0x90:
    case 0x91:
    case 0x92:
      break;
    case 0x93:
      if (attacker.effects.Rage) score+=25;
      break;
    case 0x94:
      break;
    case 0x95:
      break;
    case 0x96:
      break;
    case 0x97:
      break;
    case 0x98:
      break;
    case 0x99:
      break;
    case 0x9A:
      break;
    case 0x9B:
      break;
    case 0x9C:
      if (attacker.Partner.isFainted()) score-=90;
      break;
    case 0x9D:
      if (attacker.effects.MudSport) score-=90;
      break;
    case 0x9E:
      if (attacker.effects.WaterSport) score-=90;
      break;
    case 0x9F:
      break;
    case 0xA0:
      break;
    case 0xA1:
      if (attacker.OwnSide.LuckyChant>0) score-=90;
      break;
    case 0xA2:
      if (attacker.OwnSide.Reflect>0) score-=90;
      break;
    case 0xA3:
      if (attacker.OwnSide.LightScreen>0) score-=90;
      break;
    case 0xA4:
      break;
    case 0xA5:
      break;
    case 0xA6:
      if (opponent.effects.Substitute>0) score-=90;
      if (opponent.effects.LockOn>0) score-=90;
      break;
    case 0xA7:
      if (opponent.effects.Foresight) {
        score-=90;
      } else if (opponent.hasType(Types.GHOST)) {
        score+=70;
      } else if (opponent.stages[(int)Stats.EVASION]<=0) {
        score-=60;
      }
      break;
    case 0xA8:
      if (opponent.effects.MiracleEye) {
        score-=90;
      } else if (opponent.hasType(Types.DARK)) {
        score+=70;
      } else if (opponent.stages[(int)Stats.EVASION]<=0) {
        score-=60;
      }
      break;
    case 0xA9:
      break;
    case 0xAA:
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
    case 0xAB:
      break;
    case 0xAC:
      break;
    case 0xAD:
      break;
    case 0xAE:
      score-=40;
      if (skill>=PBTrainerAI.highSkill) {
        if (opponent.lastMoveUsed<=0 ||
                     (Game.MoveData[opponent.lastMoveUsed].flags&0x10)==0) score-=100; // flag e: Copyable by Mirror Move
      }
      break;
    case 0xAF:
      break;
    case 0xB0:
      break;
    case 0xB1:
      break;
    case 0xB2:
      break;
    case 0xB3:
      break;
    case 0xB4:
      if (attacker.Status==Status.SLEEP) {
        score+=200; // Because it can be used while asleep
      }
      else {
        score-=80;
      }
      break;
    case 0xB5:
      break;
    case 0xB6:
      break;
    case 0xB7:
      if (opponent.effects.Torment) score-=90;
      break;
    case 0xB8:
      if (attacker.effects.Imprison) score-=90;
      break;
    case 0xB9:
      if (opponent.effects.Disable>0 ) score-=90;
      break;
    case 0xBA:
      if (opponent.effects.Taunt>0) score-=90;
      break;
    case 0xBB:
      if (opponent.effects.HealBlock>0) score-=90;
      break;
    case 0xBC:
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
          if (moveData.BaseDamage==0 && (moveData.target==0x10 || moveData.target==0x20)) {
            score+=60;
          } else if (moveData.BaseDamage!=0 && moveData.target==0x00 &&
             pbTypeModifier(moveData.Type,opponent,attacker)==0) {
            score+=60;
          }
        }
      }
      break;
    case 0xBD:
      break;
    case 0xBF:
      break;
    case 0xC0:
      break;
    case 0xC1:
      break;
    case 0xC2:
      break;
    case 0xC3:
      break;
    case 0xC4:
      break;
    case 0xC7:
      if (attacker.effects.FocusEnergy>0) score+=20;
      if (skill>=PBTrainerAI.highSkill) {
        if (!opponent.hasWorkingAbility(Abilities.INNER_FOCUS) &&
                     opponent.effects.Substitute==0) score+=20;
      }
      break;
    case 0xC9:
      break;
    case 0xCA:
      break;
    case 0xCB:
      break;
    case 0xCC:
      break;
    case 0xCD:
      break;
    case 0xCE:
      break;
    case 0xCF:
      if (opponent.effects.MultiTurn==0) score+=40;
      break;
    case 0xD0:
      if (opponent.effects.MultiTurn==0) score+=40;
      break;
    case 0xD1:
      break;
    case 0xD2:
      break;
    case 0xD3:
      break;
    case 0xD4:
      if (attacker.HP<=attacker.TotalHP/4) {
        score-=90 ;
      } else if (attacker.HP<=attacker.TotalHP/2) {
        score-=50 ;
      }
      break;
    case 0xD5: case 0xD6:
      if (attacker.HP==attacker.TotalHP) {
        score-=90;
      }
      else {
        score+=50;
        score-=(attacker.HP*100/attacker.TotalHP);
      }
      break;
    case 0xD7:
      if (attacker.effects.Wish>0) score-=90;
      break;
    case 0xD8:
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
    case 0xD9:
      if (attacker.HP==attacker.TotalHP || !attacker.pbCanSleep(attacker,false,null,true)) {
        score-=90;
      }
      else {
        score+=70;
        score-=(attacker.HP*140/attacker.TotalHP);
        if (attacker.Status!=0) score+=30;
      }
      break;
    case 0xDA:
      if (attacker.effects.AquaRing) score-=90;
      break;
    case 0xDB:
      if (attacker.effects.Ingrain) score-=90;
      break;
    case 0xDC:
      if (opponent.effects.LeechSeed>=0) {
        score-=90;
      } else if (skill>=PBTrainerAI.mediumSkill && opponent.hasType(Types.GRASS)) {
        score-=90;
      }
      else {
        if (attacker.turncount==0) score+=60;
      }
      break;
    case 0xDD:
      if (skill>=PBTrainerAI.highSkill && opponent.hasWorkingAbility(Abilities.LIQUID_OOZE)) {
        score-=70;
      }
      else {
        if (attacker.HP<=(attacker.TotalHP/2)) score+=20;
      }
      break;
    case 0xDE:
      if (opponent.Status!=Status.SLEEP) {
        score-=100;
      } else if (skill>=PBTrainerAI.highSkill && opponent.hasWorkingAbility(Abilities.LIQUID_OOZE)) {
        score-=70;
      }
      else {
        if (attacker.HP<=(attacker.TotalHP/2)) score+=20;
      }
      break;
    case 0xDF:
      if (attacker.IsOpposing(opponent.Index)) {
        score-=100;
      }
      else {
        if (opponent.HP<(opponent.TotalHP/2) &&
                     opponent.effects.Substitute==0) score+=20;
      }
      break;
    case 0xE0:
      int reserves=attacker.pbNonActivePokemonCount;
      int foes=attacker.pbOppositeOpposing.pbNonActivePokemonCount;
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
    case 0xE1:
      break;
    case 0xE2:
      if (!opponent.pbCanReduceStatStage(Stats.ATTACK,attacker) &&
         !opponent.pbCanReduceStatStage(Stats.SPATK,attacker)) {
        score-=100;
      } else if (attacker.pbNonActivePokemonCount()==0) {
        score-=100 ;
      }
      else {
        score+=(opponent.stages[(int)Stats.ATTACK]*10);
        score+=(opponent.stages[(int)Stats.SPATK]*10);
        score-=(attacker.HP*100/attacker.TotalHP);
      }
      break;
    case 0xE3: case 0xE4:
      score-=70;
      break;
    case 0xE5:
      if (attacker.pbNonActivePokemonCount()==0) {
        score-=90;
      }
      else {
        if (opponent.effects.PerishSong>0) score-=90;
      }
      break;
    case 0xE6:
      score+=50;
      score-=(attacker.HP*100/attacker.TotalHP);
      if (attacker.HP<=(attacker.TotalHP/10)) score+=30;
      break;
    case 0xE7:
      score+=50;
      score-=(attacker.HP*100/attacker.TotalHP);
      if (attacker.HP<=(attacker.TotalHP/10)) score+=30;
      break;
    case 0xE8:
      if (attacker.HP>(attacker.TotalHP/2)) score-=25;
      if (skill>=PBTrainerAI.mediumSkill) {
        if (attacker.effects.ProtectRate>1) score-=90;
        if (opponent.effects.HyperBeam>0) score-=90;
      }
      else {
        score-=(attacker.effects.ProtectRate*40);
      }
      break;
    case 0xE9:
      if (opponent.HP==1) {
        score-=90;
      } else if (opponent.HP<=(opponent.TotalHP/8)) {
        score-=60;
      } else if (opponent.HP<=(opponent.TotalHP/4)) {
        score-=30;
      }
      break;
    case 0xEA:
      if (@opponent) score-=100;
      break;
    case 0xEB:
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
    case 0xEC:
      if (!opponent.effects.Ingrain &&
         !(skill>=PBTrainerAI.highSkill && opponent.hasWorkingAbility(Abilities.SUCTION_CUPS))) {
        if (opponent.OwnSide.Spikes>0) score+=40;
        if (opponent.OwnSide.ToxicSpikes>0) score+=40;
        if (opponent.OwnSide.StealthRock) score+=40;
      }
      break;
    case 0xED:
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
            if (move.MoveId!=0 && move.BaseDamage>0) {
              hasDamagingMove=true;
            }
          }
          if (!hasDamagingMove) {
            score+=75;
          }
        }
      }
      break;
    case 0xEE:
      break;
    case 0xEF:
      if (opponent.effects.MeanLook>=0) score-=90;
      break;
    case 0xF0:
      if (skill>=PBTrainerAI.highSkill) {
        if (opponent.Item!=0) score+=20;
      }
      break;
    case 0xF1:
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
    case 0xF2:
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
        if (Game.MoveData[attacker.lastMoveUsed].Effect==0xF2) score-=30;	// Trick/Switcheroo
      }
      break;
    case 0xF3:
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
    case 0xF4: case 0xF5:
      if (opponent.effects.Substitute==0) {
        if (skill>=PBTrainerAI.highSkill && pbIsBerry(opponent.Item)) {
          score+=30;
        }
      }
      break;
    case 0xF6:
      if (attacker.pokemon.itemRecycle==0 || attacker.Item!=0) {
        score-=80;
      } else if (attacker.pokemon.itemRecycle!=0) {
        score+=30;
      }
      break;
    case 0xF7:
      if (attacker.Item==0 ||
         pbIsUnlosableItem(attacker,attacker.Item) ||
         pbIsPokeBall(attacker.Item) ||
         attacker.hasWorkingAbility(Abilities.KLUTZ) ||
         attacker.effects.Embargo>0) {
        score-=90;
      }
      break;
    case 0xF8:
      if (opponent.effects.Embargo>0) score-=90;
      break;
    case 0xF9:
      if (@field.MagicRoom>0) {
        score-=90;
      }
      else {
        if (attacker.Item==0 && opponent.Item!=0) score+=30;
      }
      break;
    case 0xFA:
      score-=25;
      break;
    case 0xFB:
      score-=30;
      break;
    case 0xFC:
      score-=40;
      break;
    case 0xFD:
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
    case 0xFE:
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
    case 0xFF:
      if (pbCheckGlobalAbility(Abilities.AIR_LOCK).IsNotNullOrNone() ||
         pbCheckGlobalAbility(Abilities.CLOUD_NINE).IsNotNullOrNone()) {
        score-=90;
      } else if (pbWeather()==Weather.SUNNYDAY) {
        score-=90;
      }
      else {
        foreach (var move in attacker.moves) {
          if (move.MoveId!=0 && move.BaseDamage>0 &&
             move.Type == Types.FIRE) {
            score+=20;
          }
        }
      }
      break;
    case 0x100:
      if (pbCheckGlobalAbility(Abilities.AIR_LOCK).IsNotNullOrNone() ||
         pbCheckGlobalAbility(Abilities.CLOUD_NINE).IsNotNullOrNone()) {
        score-=90;
      } else if (pbWeather()==Weather.RAINDANCE) {
        score-=90;
      }
      else {
        foreach (var move in attacker.moves) {
          if (move.MoveId!=0 && move.BaseDamage>0 &&
             move.Type == Types.WATER) {
            score+=20;
          }
        }
      }
      break;
    case 0x101:
      if (pbCheckGlobalAbility(Abilities.AIR_LOCK).IsNotNullOrNone() ||
         pbCheckGlobalAbility(Abilities.CLOUD_NINE).IsNotNullOrNone()) {
        score-=90;
      } else if (pbWeather()==Weather.SANDSTORM) {
        score-=90;
      }
      break;
    case 0x102:
      if (pbCheckGlobalAbility(Abilities.AIR_LOCK).IsNotNullOrNone() ||
         pbCheckGlobalAbility(Abilities.CLOUD_NINE).IsNotNullOrNone()) {
        score-=90;
      } else if (pbWeather()==Weather.HAIL) {
        score-=90;
      }
      break;
    case 0x103:
      if (attacker.OpposingSide.Spikes>=3) {
        score-=90;
      } else if (!pbCanChooseNonActive(attacker.pbOpposing1.Index) &&
            !pbCanChooseNonActive(attacker.pbOpposing2.Index)) {
        // Opponent can't switch in any Pokemon
        score-=90;
      }
      else {
        score+=5*attacker.pbOppositeOpposing.pbNonActivePokemonCount();
        score+=[40,26,13][attacker.OpposingSide.Spikes];
      }
      break;
    case 0x104:
      if (attacker.OpposingSide.ToxicSpikes>=2) {
        score-=90;
      } else if (!pbCanChooseNonActive(attacker.pbOpposing1.Index) &&
            !pbCanChooseNonActive(attacker.pbOpposing2.Index)) {
        // Opponent can't switch in any Pokemon
        score-=90;
      }
      else {
        score+=4*attacker.pbOppositeOpposing.pbNonActivePokemonCount();
        score+=[26,13][attacker.OpposingSide.ToxicSpikes];
      }
      break;
    case 0x105:
      if (attacker.OpposingSide.StealthRock) {
        score-=90;
      } else if (!pbCanChooseNonActive(attacker.pbOpposing1.Index) &&
            !pbCanChooseNonActive(attacker.pbOpposing2.Index)) {
        // Opponent can't switch in any Pokemon
        score-=90;
      }
      else {
        score+=5*attacker.pbOppositeOpposing.pbNonActivePokemonCount();
      }
      break;
    case 0x106:
      break;
    case 0x107:
      break;
    case 0x108:
      break;
    case 0x109:
      break;
    case 0x10A:
      if (attacker.OpposingSide.Reflect>0) score+=20;
      if (attacker.OpposingSide.LightScreen>0) score+=20;
      break;
    case 0x10B:
      score+=10*(attacker.stages[(int)Stats.ACCURACY]-opponent.stages[(int)Stats.EVASION]);
      break;
    case 0x10C:
      if (attacker.effects.Substitute>0) {
        score-=90;
      } else if (attacker.HP<=(attacker.TotalHP/4)) {
        score-=90;
      }
      break;
    case 0x10D:
      if (attacker.hasType(Types.GHOST)) {
        if (opponent.effects.Curse) {
          score-=90;
        } else if (attacker.HP<=(attacker.TotalHP/2)) {
          if (attacker.pbNonActivePokemonCount()==0) {
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
    case 0x10E:
      score-=40;
      break;
    case 0x10F:
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
    case 0x110:
      if (attacker.effects.MultiTurn>0) score+=30;
      if (attacker.effects.LeechSeed>=0) score+=30;
      if (attacker.pbNonActivePokemonCount()>0) {
        if (attacker.OwnSide.Spikes>0) score+=80;
        if (attacker.OwnSide.ToxicSpikes>0) score+=80;
        if (attacker.OwnSide.StealthRock) score+=80;
      }
      break;
    case 0x111:
      if (opponent.effects.FutureSight>0) {
        score-=100;
      } else if (attacker.pbNonActivePokemonCount()==0) {
        // Future Sight tends to be wasteful if down to last Pokemon
        score-=70;
      }
      break;
    case 0x112:
      avg=0;
      avg-=(attacker.stages[(int)Stats.DEFENSE]*10);
      avg-=(attacker.stages[(int)Stats.SPDEF]*10);
      score+=avg/2;
      if (attacker.effects.Stockpile>=3) {
        score-=80;
      }
      else {
        // More preferable if user also has Spit Up/Swallow
        foreach (var move in attacker.moves) {
          if (move.Effect==0x113 || move.Effect==0x114) {		// Spit Up, Swallow
            score+=20; break;
          }
        }
      }
      break;
    case 0x113:
      if (attacker.effects.Stockpile==0) score-=100;
      break;
    case 0x114:
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
    case 0x115:
      if (opponent.effects.HyperBeam>0) score+=50;
      if (opponent.HP<=(opponent.TotalHP/2)) score-=35;	// If opponent is weak, no
      if (opponent.HP<=(opponent.TotalHP/4)) score-=70;	// need to risk this move
      break;
    case 0x116:
      break;
    case 0x117:
      if (!@doublebattle) {
        score-=100;
      } else if (attacker.Partner.isFainted()) {
        score-=90;
      }
      break;
    case 0x118:
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
        if (Game.MoveData[opponent.effects.TwoTurnAttack].Effect==0xC9 ||	// Fly
                     Game.MoveData[opponent.effects.TwoTurnAttack].Effect==0xCC || // Bounce
                     Game.MoveData[opponent.effects.TwoTurnAttack].Effect==0xCE) score+=20;    // Sky Drop
        if (opponent.hasType(Types.FLYING)) score+=20;
        if (opponent.hasWorkingAbility(Abilities.LEVITATE)) score+=20;
        if (opponent.hasWorkingItem(Items.AIR_BALLOON)) score+=20;
      }
      break;
    case 0x119:
      if (attacker.effects.MagnetRise>0 ||
         attacker.effects.Ingrain ||
         attacker.effects.SmackDown) {
        score-=90;
      }
      break;
    case 0x11A:
      if (opponent.effects.Telekinesis>0 ||
         opponent.effects.Ingrain ||
         opponent.effects.SmackDown) {
        score-=90;
      }
      break;
    case 0x11B:
      break;
    case 0x11C:
      if (skill>=PBTrainerAI.mediumSkill) {
        if (opponent.effects.MagnetRise>0) score+=20;
        if (opponent.effects.Telekinesis>0) score+=20;
        if (Game.MoveData[opponent.effects.TwoTurnAttack].Effect==0xC9 ||	// Fly
                     Game.MoveData[opponent.effects.TwoTurnAttack].Effect==0xCC) score+=20;    // Bounce
        if (opponent.hasType(Types.FLYING)) score+=20;
        if (opponent.hasWorkingAbility(Abilities.LEVITATE)) score+=20;
        if (opponent.hasWorkingItem(Items.AIR_BALLOON)) score+=20;
      }
      break;
    case 0x11D:
      break;
    case 0x11E:
      break;
    case 0x11F:
      break;
    case 0x120:
      break;
    case 0x121:
      break;
    case 0x122:
      break;
    case 0x123:
      if (!opponent.pbHasType(attacker.Type1) &&
         !opponent.pbHasType(attacker.Type2)) {
        score-=90;
      }
      break;
    case 0x124:
      break;
    case 0x125:
      break;
    case 0x126:
      score+=20; // Shadow moves are more preferable
      break;
    case 0x127:
      score+=20; // Shadow moves are more preferable
      if (opponent.pbCanParalyze(attacker,false)) {
        score+=30;
        if (skill>=PBTrainerAI.mediumSkill) {
           int aspeed=pbRoughStat(attacker,Stats.SPEED,skill);
           int ospeed=pbRoughStat(opponent,Stats.SPEED,skill);
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
    case 0x128:
      score+=20; // Shadow moves are more preferable
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
    case 0x129:
      score+=20; // Shadow moves are more preferable
      if (opponent.pbCanFreeze(attacker,false)) {
        score+=30;
        if (skill>=PBTrainerAI.highSkill) {
          if (opponent.hasWorkingAbility(Abilities.MARVEL_SCALE)) score-=20;
        }
      }
      break;
    case 0x12A:
      score+=20; // Shadow moves are more preferable
      if (opponent.pbCanConfuse(attacker,false)) {
        score+=30;
      }
      else {
        if (skill>=PBTrainerAI.mediumSkill) {
          score-=90;
        }
      }
      break;
    case 0x12B:
      score+=20; // Shadow moves are more preferable
      if (!opponent.pbCanReduceStatStage(Stats.DEFENSE,attacker)) {
        score-=90;
      }
      else {
        if (attacker.turncount==0) score+=40;
        score+=opponent.stages[(int)Stats.DEFENSE]*20;
      }
      break;
    case 0x12C:
      score+=20; // Shadow moves are more preferable
      if (!opponent.pbCanReduceStatStage(Stats.EVASION,attacker)) {
        score-=90;
      }
      else {
        score+=opponent.stages[(int)Stats.EVASION]*15;
      }
      break;
    case 0x12D:
      score+=20; // Shadow moves are more preferable
      break;
    case 0x12E:
      score+=20; // Shadow moves are more preferable
      if (opponent.HP>=(opponent.TotalHP/2)) score+=20;
      if (attacker.HP<(attacker.HP/2)) score-=20;
      break;
    case 0x12F:
      score+=20; // Shadow moves are more preferable
      if (opponent.effects.MeanLook>=0) score-=110;
      break;
    case 0x130:
      score+=20; // Shadow moves are more preferable
      score-=40;
      break;
    case 0x131:
      score+=20; // Shadow moves are more preferable
      if (pbCheckGlobalAbility(Abilities.AIR_LOCK) ||
         pbCheckGlobalAbility(Abilities.CLOUD_NINE)) {
        score-=90;
      } else if (pbWeather()==Weather.SHADOWSKY) {
        score-=90;
      }
      break;
    case 0x132:
      score+=20; // Shadow moves are more preferable
      if (opponent.OwnSide.Reflect>0 ||
         opponent.OwnSide.LightScreen>0 ||
         opponent.OwnSide.Safeguard>0) {
        score+=30;
        if (attacker.OwnSide.Reflect>0 ||
                     attacker.OwnSide.LightScreen>0 ||
                     attacker.OwnSide.Safeguard>0) score-=90;
      }
      else {
        score-=110;
      }
      break;
    case 0x133: case 0x134:
      score-=95;
      if (skill>=PBTrainerAI.highSkill) {
        score=0;
      }
      break;
    case 0x135:
      if (opponent.pbCanFreeze(attacker,false)) {
        score+=30;
        if (skill>=PBTrainerAI.highSkill) {
          if (opponent.hasWorkingAbility(Abilities.MARVEL_SCALE)) score-=20;
        }
      }
      break;
    case 0x136:
      if (attacker.stages[(int)Stats.DEFENSE]<0) score+=20;
      break;
    case 0x137:
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
    case 0x138:
      if (!@doublebattle) {
        score-=100;
      } else if (attacker.Partner.isFainted()) {
        score-=90;
      }
      else {
        score-=attacker.Partner.stages[(int)Stats.SPDEF]*10;
      }
      break;
    case 0x139:
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
    case 0x13A:
      avg=opponent.stages[(int)Stats.ATTACK]*10;
      avg+=opponent.stages[(int)Stats.SPATK]*10;
      score+=avg/2;
      break;
    case 0x13B:
      if (!attacker.Species == Pokemons.HOOPA || attacker.form!=1) {
        score-=100;
      }
      else {
        if (opponent.stages[(int)Stats.DEFENSE]>0) score+=20;
      }
      break;
    case 0x13C:
      if (opponent.stages[(int)Stats.SPATK]>0) score+=20;
      break;
    case 0x13D:
      if (!opponent.pbCanReduceStatStage(Stats.SPATK,attacker)) {
        score-=90;
      }
      else {
        if (attacker.turncount==0) score+=40;
        score+=opponent.stages[(int)Stats.SPATK]*20;
      }
      break;
    case 0x13E:
      count=0;
      for (int i = 0; i < 4; i++) {
        Pokemon battler=@battlers[i];
        if (battler.hasType(Types.GRASS) && !battler.isAirborne() &&
           (!battler.pbTooHigh(Stats.ATTACK) || !battler.pbTooHigh(Stats.SPATK))) {
          count+=1;
          if (attacker.IsOpposing(battler)) {
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
    case 0x13F:
      count=0;
      for (int i = 0; i < 4; i++) {
        Pokemon battler=@battlers[i];
        if (battler.hasType(Types.GRASS) && !battler.pbTooHigh(Stats.DEFENSE)) {
          count+=1;
          if (attacker.IsOpposing(battler)) {
            score-=20;
          }
          else {
            score-=attacker.stages[(int)Stats.DEFENSE]*10;
          }
        }
      }
      if (count==0) score-=95;
      break;
    case 0x140:
      count=0;
      for (int i = 0; i < 4; i++) {
        Pokemon battler=@battlers[i];
        if (battler.Status==Status.POISON &&
           (!battler.pbTooLow(Stats.ATTACK) ||
           !battler.pbTooLow(Stats.SPATK) ||
           !battler.pbTooLow(Stats.SPEED))) {
          count+=1;
          if (attacker.IsOpposing(battler)) {
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
    case 0x141:
      if (opponent.effects.Substitute>0) {
        score-=90;
      }
      else {
        int numpos=0; int numneg=0;
        foreach (var i in new Stats[] { Stats.ATTACK,Stats.DEFENSE,Stats.SPEED,
                  Stats.SPATK,Stats.SPDEF,Stats.ACCURACY,Stats.EVASION }) {
          int stat=opponent.stages[(int)i];
          (stat>0) ? numpos+=stat : numneg+=stat;
        }
        if (numpos!=0 || numneg!=0) {
          score+=(numpos-numneg)*10;
        }
        else {
          score-=95;
        }
      }
      break;
    case 0x142:
      if (opponent.hasType(Types.GHOST)) {
        score-=90;
      }
      break;
    case 0x143:
      if (opponent.hasType(Types.GRASS)) {
        score-=90;
      }
      break;
    case 0x144:
      break;
    case 0x145:
      aspeed=pbRoughStat(attacker,Stats.SPEED,skill);
      ospeed=pbRoughStat(opponent,Stats.SPEED,skill);
      if (aspeed>ospeed) {
        score-=90;
      }
      break;
    case 0x146:
      break;
    case 0x147:
      break;
    case 0x148:
      aspeed=pbRoughStat(attacker,Stats.SPEED,skill);
      ospeed=pbRoughStat(opponent,Stats.SPEED,skill);
      if (aspeed>ospeed) {
        score-=90;
      }
      else {
        if (opponent.pbHasMoveType(Types.FIRE)) score+=30;
      }
      break;
    case 0x149:
      if (attacker.turncount==0) {
        score+=30;
      }
      else {
        score-=90; // Because it will fail here
        if (skill>=PBTrainerAI.bestSkill) score=0;
      }
      break;
    case 0x14A:
      break;
    case 0x14B: case 0x14C:
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
    case 0x14D:
      break;
    case 0x14E:
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
    case 0x14F:
      if (skill>=PBTrainerAI.highSkill && opponent.hasWorkingAbility(Abilities.LIQUIDOOZE)) {
        score-=80;
      }
      else {
        if (attacker.HP<=(attacker.TotalHP/2)) score+=40;
      }
      break;
    case 0x150:
      if (!attacker.pbTooHigh(:ATTACK) && opponent.HP<=(opponent.TotalHP/4)) score+=20;
      break;
    case 0x151:
      avg=opponent.stages[(int)Stats.ATTACK]*10;
      avg+=opponent.stages[(int)Stats.SPATK]*10;
      score+=avg/2;
      break;
    case 0x152:
      break;
    case 0x153:
      if (opponent.OwnSide.StickyWeb) score-=95;
      break;
    case 0x154:
      break;
    case 0x155:
      break;
    case 0x156:
      break;
    case 0x157:
      score-=90;
      break;
    case 0x158:
      if (!attacker.pokemon || !attacker.pokemon.belch) score-=90;
      break;
    }
    // A score of 0 here means it should absolutely not be used
    if (score<=0) return (int)score;
// #### Other score modifications ###############################################
    // Prefer damaging moves if AI has no more Pokémon
    if (attacker.pbNonActivePokemonCount()==0) {
      if (skill>=PBTrainerAI.mediumSkill &&
         !(skill>=PBTrainerAI.highSkill && opponent.pbNonActivePokemonCount>0)) {
        if (move.BaseDamage==0) {
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
         (new Attack.Data.Effects[] { 0xC9,0xCA,0xCB,0xCC,0xCD,0xCE }.Contains(invulmove) ||
         opponent.effects.SkyDrop) &&
         attacker.pbSpeed>opponent.pbSpeed) {
        if (skill>=PBTrainerAI.bestSkill) {		// Can get past semi-invulnerability
          bool miss=false;
          switch (invulmove) {
          case 0xC9: case 0xCC: // Fly: Bounce
            unless (move.Effect==0x08 || 	// Thunder
                             move.Effect==0x15 ||  // Hurricane
                             move.Effect==0x77 ||  // Gust
                             move.Effect==0x78 ||  // Twister
                             move.Effect==0x11B || // Sky Uppercut
                             move.Effect==0x11C || // Smack Down
                             move.MoveId == Moves.WHIRLWIND) miss=true;
            break;
          case 0xCA: // Dig
            unless (move.Effect==0x76 ||	// Earthquake
                             move.Effect==0x95) miss=true;    // Magnitude
            break;
          case 0xCB: // Dive
            unless (move.Effect==0x75 ||	// Surf
                             move.Effect==0xD0) miss=true;    // Whirlpool
            break;
          case 0xCD: // Shadow Force
            miss=true;
            break;
          case 0xCE: // Sky Drop
            unless (move.Effect==0x08 || 	// Thunder
                             move.Effect==0x15 ||  // Hurricane
                             move.Effect==0x77 ||  // Gust
                             move.Effect==0x78 ||  // Twister
                             move.Effect==0x11B || // Sky Uppercut
                             move.Effect==0x11C) miss=true;    // Smack Down
            break;
          case 0x14D: // Phantom Force
            miss=true;
            break;
          }
          if (opponent.effects.SkyDrop) {
            unless (move.Effect==0x08 || 	// Thunder
                             move.Effect==0x15 ||  // Hurricane
                             move.Effect==0x77 ||  // Gust
                             move.Effect==0x78 ||  // Twister
                             move.Effect==0x11B || // Sky Uppercut
                             move.Effect==0x11C) miss=true;    // Smack Down
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
        if (move.BaseDamage>=60) {
          score+=60;
        } else if (move.BaseDamage>0) {
          score+=30;
        } else if (move.Effect==0xF2) {		// Trick
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
        if (move.Effect!=0x11 && move.Effect!=0xB4) {		// Snore, Sleep Talk
          bool hasSleepMove=false;
          foreach (var m in attacker.moves) {
            if (m.Effect==0x11 || m.Effect==0xB4) {		// Snore, Sleep Talk
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
        if (move.canThawUser()) {
          score+=40;
        }
        else {
          bool hasFreezeMove=false;
          foreach (var m in attacker.moves) {
            if (m.canThawUser?) {
              hasFreezeMove=true; break;
            }
          }
          if (hasFreezeMove) score-=60;
        }
      }
    }
    // If target is frozen, don't prefer moves that could thaw them // TODO
    // Adjust score based on how much damage it can deal
    if (move.BaseDamage>0) {
      typemod=pbTypeModifier(move.Type,attacker,opponent);
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
        int realDamage=move.BaseDamage;
        if (move.BaseDamage==1) realDamage=60;
        if (skill>=PBTrainerAI.mediumSkill) {
          realDamage=pbBetterBaseDamage(move,attacker,opponent,skill,realDamage);
        }
        realDamage=pbRoughDamage(move,attacker,opponent,skill,realDamage);
        // Account for accuracy of move
        int accuracy=pbRoughAccuracy(move,attacker,opponent,skill);
        float basedamage=realDamage*accuracy/100.0f;
        // Two-turn attacks waste 2 turns to deal one lot of damage
        if (move.pbTwoTurnAttack(attacker) || move.Effect==0xC2) {		// Hyper Beam
          basedamage*=2/3;   // Not halved because semi-invulnerable during use or hits first turn
        }
        // Prefer flinching effects
        if (!opponent.hasWorkingAbility(Abilities.INNER_FOCUS) &&
           opponent.effects.Substitute==0) {
          if ((attacker.hasWorkingItem(Items.KINGS_ROCK) || attacker.hasWorkingItem(Items.RAZOR_FANG)) &&
             move.canKingsRock?) {
            basedamage*=1.05f;
          } else if (attacker.hasWorkingAbility(Abilities.STENCH) &&
                move.Effect!=0x09 && // Thunder Fang
                move.Effect!=0x0B && // Fire Fang
                move.Effect!=0x0E && // Ice Fang
                move.Effect!=0x0F && // flinch-inducing moves
                move.Effect!=0x10 && // Stomp
                move.Effect!=0x11 && // Snore
                move.Effect!=0x12 && // Fake Out
                move.Effect!=0x78 && // Twister
                move.Effect!=0xC7) {    // Sky Attack
            basedamage*=1.05f;
          }
        }
        // Convert damage to proportion of opponent's remaining HP
        basedamage=(basedamage*100.0f/opponent.HP);
        // Don't prefer weak attacks
//        basedamage/=2 if basedamage<40
        // Prefer damaging attack if level difference is significantly high
        if (attacker.Level-10>opponent.Level) basedamage*=1.2f;
        // Adjust score
        basedamage=(int)Math.Round(basedamage);
        if (basedamage>120  ) basedamage=120;	// Treat all OHKO moves the same
        if (basedamage>100  ) basedamage+=40;	// Prefer moves likely to OHKO
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
    }*/
    //score=score.ToInteger();
    if (score<0) score=0;
    return (int)score.ToInteger();
  }

// ###############################################################################
// Get type effectiveness and approximate stats.
// ###############################################################################
  public int pbTypeModifier(Types type,Pokemon attacker,Pokemon opponent) {
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
    int mod1=0;//PBTypes.getEffectiveness(atype,otype1);
    int mod2=0;//(otype1==otype2) ? 2 : PBTypes.getEffectiveness(atype,otype2);
    int mod3=0;//(otype3<0 || otype1==otype3 || otype2==otype3) ? 2 : PBTypes.getEffectiveness(atype,otype3);
    /*if (opponent.hasWorkingItem(Items.RING_TARGET)) {
      if (mod1==0) mod1=2;
      if (mod2==0) mod2=2;
      if (mod3==0) mod3=2;
    }
    // Foresight
    if ((attacker.hasWorkingAbility(Abilities.SCRAPPY)) || opponent.effects.Foresight) { //rescue false
      if (otype1 == Types.GHOST && PBTypes.isIneffective(atype,otype1)) mod1=2;
      if (otype2 == Types.GHOST && PBTypes.isIneffective(atype,otype2)) mod2=2;
      if (otype3 == Types.GHOST && PBTypes.isIneffective(atype,otype3)) mod3=2;
    }
    // Miracle Eye
    if (opponent.effects.MiracleEye) {
      if (otype1 == Types.DARK && PBTypes.isIneffective(atype,otype1)) mod1=2;
      if (otype2 == Types.DARK && PBTypes.isIneffective(atype,otype2)) mod2=2;
      if (otype3 == Types.DARK && PBTypes.isIneffective(atype,otype3)) mod3=2;
    }
    // Delta Stream's weather
    if (pbWeather()==Weather.STRONGWINDS) {
      if (otype1 == Types.FLYING && PBTypes.isSuperEffective(atype,otype1)) mod1=2;
      if (otype2 == Types.FLYING && PBTypes.isSuperEffective(atype,otype2)) mod2=2;
      if (otype3 == Types.FLYING && PBTypes.isSuperEffective(atype,otype3)) mod3=2;
    }
    // Smack Down makes Ground moves work against fliers
    if (!opponent.isAirborne(attacker.hasMoldBreaker()) && atype == Types.GROUND) { //rescue false
      if (otype1 == Types.FLYING) mod1=2;
      if (otype2 == Types.FLYING) mod2=2;
      if (otype3 == Types.FLYING) mod3=2;
    }*/
    return mod1*mod2*mod3;
  }

  public int pbTypeModifier2(Pokemon battlerThis,Pokemon battlerOther) {
    // battlerThis isn't a Battler object, it's a Pokémon - it has no third type
    if (battlerThis.Type1==battlerThis.Type2) {
      return 4*pbTypeModifier(battlerThis.Type1,battlerThis,battlerOther);
    }
    int ret=pbTypeModifier(battlerThis.Type1,battlerThis,battlerOther);
    ret*=pbTypeModifier(battlerThis.Type2,battlerThis,battlerOther);
    return ret*2; // 0,1,2,4,_8_,16,32,64
  }

  public int pbRoughStat(Pokemon battler,Stats stat,int skill) {
    if (skill>=PBTrainerAI.highSkill && stat==Stats.SPEED) return battler.speed;//pbSpeed;
    int[] stagemul=new int[] {2,2,2,2,2,2,2,3,4,5,6,7,8};
    int[] stagediv=new int[] {8,7,6,5,4,3,2,2,2,2,2,2,2};
    int stage=battler.stages[(int)stat]+6;
    int value=0;
    switch (stat) {
    case Stats.ATTACK: value=battler.attack;
      break;
    case Stats.DEFENSE: value=battler.defense;
      break;
    case Stats.SPEED: value=battler.speed;
      break;
    case Stats.SPATK: value=battler.spatk;
      break;
    case Stats.SPDEF: value=battler.spdef;
      break;
    }
    return (int)Math.Floor(value*1.0f*stagemul[stage]/stagediv[stage]);
  }

  public int pbBetterBaseDamage(Move move,Pokemon attacker,Pokemon opponent,int skill,int basedamage) {
    // Covers all function codes which have their own def pbBaseDamage
    /*switch (move.Effect) {
    case 0x6A: // SonicBoom
      basedamage=20;
      break;
    case 0x6B: // Dragon Rage
      basedamage=40;
      break;
    case 0x6C: // Super Fang
      basedamage=(int)Math.Floor(opponent.HP/2);
      break;
    case 0x6D: // Night Shade
      basedamage=attacker.Level;
      break;
    case 0x6E: // Endeavor
      basedamage=opponent.HP-attacker.HP;
      break;
    case 0x6F: // Psywave
      basedamage=attacker.Level;
      break;
    case 0x70: // OHKO
      basedamage=opponent.TotalHP;
      break;
    case 0x71: // Counter
      basedamage=60;
      break;
    case 0x72: // Mirror Coat
      basedamage=60;
      break;
    case 0x73: // Metal Burst
      basedamage=60;
      break;
    case 0x75: case 0x12D: // case Surf: Shadow Storm
      if (Game.MoveData[opponent.effects.TwoTurnAttack].Effect==0xCB) basedamage*=2;	// Dive
      break;
    case 0x76: // Earthquake
      if (Game.MoveData[opponent.effects.TwoTurnAttack].Effect==0xCA) basedamage*=2;	// Dig
      break;
    case 0x77: case 0x78: // case Gust: Twister
      if (Game.MoveData[opponent.effects.TwoTurnAttack].Effect==0xC9 ||	// Fly
                       Game.MoveData[opponent.effects.TwoTurnAttack].Effect==0xCC || // Bounce
                       Game.MoveData[opponent.effects.TwoTurnAttack].Effect==0xCE) basedamage*=2;    // Sky Drop
      break;
    case 0x7B: // Venoshock
      if (opponent.Status==Status.POISON) basedamage*=2;
      break;
    case 0x7C: // SmellingSalt
      if (opponent.Status==Status.PARALYSIS) basedamage*=2;
      break;
    case 0x7D: // Wake-Up Slap
      if (opponent.Status==Status.SLEEP) basedamage*=2;
      break;
    case 0x7E: // Facade
      if (attacker.Status==Status.POISON ||
                       attacker.Status==Status.BURN ||
                       attacker.Status==Status.PARALYSIS) basedamage*=2;
      break;
    case 0x7F: // Hex
      if (opponent.Status!=0) basedamage*=2;
      break;
    case 0x80: // Brine
      if (opponent.HP<=(int)Math.Floor(opponent.TotalHP/2f)) basedamage*=2;
      break;
    case 0x85: // Retaliate
      //TODO
      break;
    case 0x86: // Acrobatics
      if (attacker.Item==0 || attacker.hasWorkingItem(Items.FLYING_GEM)) basedamage*=2;
      break;
    case 0x87: // Weather Ball
      if (pbWeather!=0) basedamage*=2;
      break;
    case 0x89: // Return
      basedamage=(int)Math.Max((int)Math.Floor(attacker.happiness*2/5f),1);
      break;
    case 0x8A: // Frustration
      basedamage=(int)Math.Max((int)Math.Floor((255-attacker.happiness)*2/5f),1);
      break;
    case 0x8B: // Eruption
      basedamage=(int)Math.Max((int)Math.Floor(150f*attacker.HP/attacker.TotalHP),1);
      break;
    case 0x8C: // Crush Grip
      basedamage=(int)Math.Max((int)Math.Floor(120f*opponent.HP/opponent.TotalHP),1);
      break;
    case 0x8D: // Gyro Ball
      ospeed=pbRoughStat(opponent,Stats.SPEED,skill);
      aspeed=pbRoughStat(attacker,Stats.SPEED,skill);
      basedamage=(int)Math.Max((int)Math.Min((int)Math.Floor(25*ospeed/aspeed),150),1);
      break;
    case 0x8E: // Stored Power
      mult=0;
      foreach (Stats i in new Stats[] { Stats.ATTACK,Stats.DEFENSE,Stats.SPEED,
                Stats.SPATK,Stats.SPDEF,Stats.ACCURACY,Stats.EVASION }) {
        if (attacker.stages[i]>0) mult+=attacker.stages[i];
      }
      basedamage=20*(mult+1);
      break;
    case 0x8F: // Punishment
      mult=0;
      foreach (Stats i in new Stats[] { Stats.ATTACK,Stats.DEFENSE,Stats.SPEED,
                Stats.SPATK,Stats.SPDEF,Stats.ACCURACY,Stats.EVASION }) {
        if (opponent.stages[i]>0) mult+=opponent.stages[i];
      }
      basedamage=(int)Math.Min(20*(mult+3),200);
      break;
    case 0x90: // Hidden Power
      hp=pbHiddenPower(attacker.iv);
      basedamage=hp[1];
      break;
    case 0x91: // Fury Cutter
      basedamage=basedamage<<(attacker.effects.FuryCutter-1);
      break;
    case 0x92: // Echoed Voice
      basedamage*=attacker.OwnSide.EchoedVoiceCounter;
      break;
    case 0x94: // Present
      basedamage=50;
      break;
    case 0x95: // Magnitude
      basedamage=71;
      if (Game.MoveData[opponent.effects.TwoTurnAttack].Effect==0xCA) basedamage*=2;	// Dig
      break;
    case 0x96: // Natural Gift
      KeyValuePair<Items, int>[] damagearray= new KeyValuePair<Items, int>[]{
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
      }
      haveanswer=false;
      foreach (var i in damagearray.keys) {
        data=damagearray[i];
        if (data) {
          foreach (var j in data) {
            if (attacker.Item == j) {
              basedamage=i; haveanswer=true; break;
            }
          }
        }
        if (haveanswer) break;
      }
      break;
    case 0x97: // Trump Card
      dmgs=[200,80,60,50,40];
      ppleft=(int)Math.Min(move.pp-1,4);   // PP is reduced before the move is used
      basedamage=dmgs[ppleft];
      break;
    case 0x98: // Flail
      n=(int)Math.Floor(48*attacker.HP/attacker.TotalHP);
      basedamage=20;
      if (n<33) basedamage=40;
      if (n<17) basedamage=80;
      if (n<10) basedamage=100;
      if (n<5) basedamage=150;
      if (n<2) basedamage=200;
      break;
    case 0x99: // Electro Ball
      n=(int)Math.Floor(attacker.pbSpeed/opponent.pbSpeed);
      basedamage=40;
      if (n>=1) basedamage=60;
      if (n>=2) basedamage=80;
      if (n>=3) basedamage=120;
      if (n>=4) basedamage=150;
      break;
    case 0x9A: // Low Kick
      weight=opponent.weight;
      basedamage=20;
      if (weight>100) basedamage=40;
      if (weight>250) basedamage=60;
      if (weight>500) basedamage=80;
      if (weight>1000) basedamage=100;
      if (weight>2000) basedamage=120;
      break;
    case 0x9B: // Heavy Slam
      n=(int)Math.Floor(attacker.weight/opponent.weight);
      basedamage=40;
      if (n>=2) basedamage=60;
      if (n>=3) basedamage=80;
      if (n>=4) basedamage=100;
      if (n>=5) basedamage=120;
      break;
    case 0xA0: // Frost Breath
      basedamage*=2;
      break;
    case 0xBD: case 0xBE: // Double case Kick: Twineedle
      basedamage*=2;
      break;
    case 0xBF: // Triple Kick
      basedamage*=6;
      break;
    case 0xC0: // Fury Attack
      if (attacker.hasWorkingAbility(Abilities.SKILLLINK)) {
        basedamage*=5;
      }
      else {
        basedamage=(int)Math.Floor(basedamage*19/6);
      }
      break;
    case 0xC1: // Beat Up
      party=pbParty(attacker.Index);
      mult=0;
      for (int i = 0; i < party.Length; i++) {
        if (party[i] && !party[i].isEgg? &&
                   party[i].HP>0 && party[i].Status==0) mult+=1;
      }
      basedamage*=mult;
      break;
    case 0xC4: // SolarBeam
      if (pbWeather!=0 && pbWeather!=Weather.SUNNYDAY) {
        basedamage=(int)Math.Floor(basedamage*0.5);
      }
      break;
    case 0xD0: // Whirlpool
      if (skill>=PBTrainerAI.mediumSkill) {
        if (Game.MoveData[opponent.effects.TwoTurnAttack].Effect==0xCB) basedamage*=2;	// Dive
      }
      break;
    case 0xD3: // Rollout
      if (skill>=PBTrainerAI.mediumSkill) {
        if (attacker.effects.DefenseCurl) basedamage*=2;
      }
      break;
    case 0xE1: // Final Gambit
      basedamage=attacker.HP;
      break;
    case 0xF7: // Fling
      //TODO
      break;
    case 0x113: // Spit Up
      basedamage*=attacker.effects.Stockpile;
      break;
    case 0x144:
      type=Types.FLYING || -1;
      if (type>=0) {
        mult=PBTypes.getCombinedEffectiveness(type,
           opponent.Type1,opponent.Type2,opponent.effects.Type3);
        basedamage= (int)Math.Round((basedamage*mult)/8);
      }
    }*/
    return basedamage;
  }

  public int pbRoughDamage(Move move, Pokemon attacker, Pokemon opponent, int skill, int basedamage) {
    // Fixed damage moves
    /*if (move.Effect==0x6A ||  	// SonicBoom
                         move.Effect==0x6B ||   // Dragon Rage
                         move.Effect==0x6C ||   // Super Fang
                         move.Effect==0x6D ||   // Night Shade
                         move.Effect==0x6E ||   // Endeavor
                         move.Effect==0x6F ||   // Psywave
                         move.Effect==0x70 ||   // OHKO
                         move.Effect==0x71 ||   // Counter
                         move.Effect==0x72 ||   // Mirror Coat
                         move.Effect==0x73 ||   // Metal Burst
                         move.Effect==0xE1) return basedamage;      // Final Gambit
    Types type=move.Type;
    // More accurate move type (includes Normalize, most type-changing moves, etc.)
    if (skill>=PBTrainerAI.highSkill) {
      type=move.pbType(type,attacker,opponent);
    }
    // Technician
    if (skill>=PBTrainerAI.highSkill) {
      if (attacker.hasWorkingAbility(Abilities.TECHNICIAN) && basedamage<=60) {
        basedamage=(int)Math.Round(basedamage*1.5);
      }
    }
    // Iron Fist
    if (skill>=PBTrainerAI.mediumSkill) {
      if (attacker.hasWorkingAbility(Abilities.IRON_FIST) && move.Flags.Punching) {
        basedamage=(int)Math.Round(basedamage*1.2);
      }
    }
    // Reckless
    if (skill>=PBTrainerAI.mediumSkill) {
      if (attacker.hasWorkingAbility(Abilities.RECKLESS)) {
        if(move.Effect==0xFA || 		// Take Down, etc.
           move.Effect==0xFB ||  // Double-Edge, etc.
           move.Effect==0xFC ||  // Head Smash
           move.Effect==0xFD ||  // Volt Tackle
           move.Effect==0xFE ||  // Flare Blitz
           move.Effect==0x10B || // Jump Kick, Hi Jump Kick
           move.Effect==0x130) {    // Shadow End
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
         attacker.gender!=2 && opponent.gender!=2) {
        if (attacker.gender==opponent.gender) {
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
      if (attacker.hasWorkingAbility(Abilities.SHEER_FORCE) && move.AddlEffect>0) {
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
    if (move.Effect==0x121) {		// Foul Play
      atk=pbRoughStat(opponent,Stats.ATTACK,skill);
    }
    if (type>=0 && move.pbIsSpecial(type)) {
      atk=pbRoughStat(attacker,Stats.SPATK,skill);
      if (move.Effect==0x121) {		// Foul Play
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
      if (move.Effect!=0x122) {		// Psyshock
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
    int damage=(int)Math.Floor(Math.Floor(Math.Floor(2.0f*attacker.Level/5f+2f)*basedamage*atk/defense)/50f)+2;
    // Multi-targeting attacks
    if (skill>=PBTrainerAI.highSkill) {
      if (move.pbTargetsMultiple(attacker)) {
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
      if (attacker.pbHasType(type)) {
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
    int typemod=pbTypeModifier(type,attacker,opponent);
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
      if (move.hasHighCriticalRate) c+=1;
      if ((attacker.inHyperMode()) && this.Type == Types.SHADOW) c+=1; //rescue false
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
    return damage;*/
    return 0;
  }

  public int pbRoughAccuracy(Move move, Pokemon attacker, Pokemon opponent, int skill) {
    float accuracy=0;
    // Get base accuracy
    /*int baseaccuracy=move.Accuracy;
    if (skill>=PBTrainerAI.mediumSkill) {
      if (pbWeather()==Weather.SUNNYDAY &&
         (move.Effect==0x08 || move.Effect==0x15)) { // Thunder, Hurricane
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
                  move.Effect==0xA9 || // Chip Away
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
                         move.BaseDamage>0 &&
                         move.pbIsPhysical(move.pbType(move.Type,attacker,opponent))) accuracy*=0.8f;
      }
      if (skill>=PBTrainerAI.bestSkill) {
        if (opponent.hasWorkingAbility(Abilities.WONDER_SKIN) &&
                       move.BaseDamage==0 &&
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
    if (move.Effect==0xA5) accuracy=125;	// Swift
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
        if (move.Effect==0x0D) accuracy=125;	// Blizzard
        break;
      case Weather.RAINDANCE:
        if (move.Effect==0x08 || move.Effect==0x15) accuracy=125;	// Thunder, Hurricane
        break;
      }
      if (move.Effect==0x70) {		// OHKO moves
        accuracy=move.Accuracy+attacker.Level-opponent.Level;
        if (opponent.hasWorkingAbility(Abilities.STURDY)) accuracy=0;
        if (opponent.Level>attacker.Level) accuracy=0;
      }
    }*/
    return (int)accuracy;
  }

// ###############################################################################
// Choose a move to use.
// ###############################################################################
  public void pbChooseMoves(int index) {
    /*Pokemon attacker=@battlers[index];
    int[] scores=new int[] { 0, 0, 0, 0 };
    int[] targets=null;
    List<int> myChoices=new List<int>();
    int totalscore=0;
    int target=-1;
    int skill=0;
    bool wildbattle=@opponent==null && pbIsOpposing(index);
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
      skill=pbGetOwner(attacker.Index).Skill || 0;
      Pokemon opponent=attacker.pbOppositeOpposing;
      if (@doublebattle && !opponent.isFainted() && !opponent.Partner.isFainted()) {
        // Choose a target and move.  Also care about partner.
        Pokemon otheropp=opponent.Partner;
        List<> scoresAndTargets=new List<>();
        targets=[-1,-1,-1,-1];
        for (int i = 0; i < 4; i++) {
          if (pbCanChooseMove(index,i,false)) {
            int score1=pbGetMoveScore(attacker.moves[i],attacker,opponent,skill);
            int score2=pbGetMoveScore(attacker.moves[i],attacker,otheropp,skill);
            if ((attacker.moves[i].target&0x20)!=0) {		// Target's user's side
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
                else // Hardly effective
                  score1*=5/3;
                  score2*=5/3;
                }
              }
            }
            myChoices.Add(i);
            //ToDo:: Uncomment below
            //scoresAndTargets.Add([i*2,i,score1,opponent.Index]);
            //scoresAndTargets.Add([i*2+1,i,score2,otheropp.Index]);
          }
        }
        scoresAndTargets.Sort(a,b =>
           if (a[2]==b[2]) {		// if scores are equal
             a[0]<=>b[0]; // sort by index (for stable comparison)
           }
           else {
             b[2]<=>a[2];
           }
        );
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
               {@battlers[index].pbNonActivePokemonCount()}");
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
    }*/
  }

// ###############################################################################
// Decide whether the opponent should Mega Evolve their Pokémon.
// ###############################################################################
  public bool pbEnemyShouldMegaEvolve (int ndex) {
    // Simple "always should if possible"
    //return pbCanMegaEvolve(index);
    return false;
  }

// ###############################################################################
// Decide whether the opponent should use an item on the Pokémon.
// ###############################################################################
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

// ###############################################################################
// Decide whether the opponent should switch Pokémon.
// ###############################################################################
  public virtual bool pbEnemyShouldWithdraw (int index) {
//    if (Core.INTERNAL && !pbIsOpposing(index)) {
//      return pbEnemyShouldWithdrawOld(index);
//    }
    return pbEnemyShouldWithdrawEx(index,false);
  }

  public bool pbEnemyShouldWithdrawEx (int index,bool alwaysSwitch) {
    /*if (!@opponent) return false;
    bool shouldswitch=alwaysSwitch;
    bool typecheck=false;
    int batonpass=-1;
    Types movetype=Types.NONE;
    int skill=pbGetOwner(index).Skill;// || 0;
    if (@opponent && !shouldswitch && @battlers[index].turncount>0) {
      if (skill>=PBTrainerAI.highSkill) {
        Pokemon opponent=@battlers[index].pbOppositeOpposing;
        if (opponent.isFainted()) opponent=opponent.Partner;
        if (!opponent.isFainted() && opponent.lastMoveUsed>0 && 
           Math.Abs(opponent.Level-@battlers[index].Level)<=6) {
          Attack.Data.MoveData move=Game.MoveData[opponent.lastMoveUsed];
          typemod=pbTypeModifier(move.Type,@battlers[index],@battlers[index]);
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
        Move move=@battlers[index].moves[i].IsNotNullOrNone();
        if (move.MoveId!=0 && pbCanChooseMove(index,i,false) &&
          move.Effect==0xED) { // Baton Pass
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
    if (@rules["suddendeath"]) {
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
      Pokemon[] party=pbParty(index);
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
              list.unshift(i); // put this Pokemon first
            }
          } else if (movetype>=0 && pbTypeModifier(movetype,@battlers[index],@battlers[index])<8) {
            int weight=40;
            if (pbTypeModifier2(party[i],@battlers[index].pbOppositeOpposing)>8) {
              // Greater weight if new Pokemon's type is effective against opponent
              weight=60;
            }
            if (Core.Rand.Next(100)<weight) {
              list.unshift(i); // put this Pokemon first
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
    }*/
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
    //if (!$PokemonTemp) $PokemonTemp=new PokemonTemp;
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

// ###############################################################################
// Choose an action.
// ###############################################################################
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
      //ToDo: Uncomment
      //if (pbEnemyShouldMegaEvolve(index)) pbRegisterMegaEvolution(index);
      pbChooseMoves(index);
    }
  }

// ###############################################################################
// Other functions.
// ###############################################################################
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
}
}