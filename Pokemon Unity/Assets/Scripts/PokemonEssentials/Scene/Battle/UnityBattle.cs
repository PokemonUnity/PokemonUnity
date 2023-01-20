using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
using PokemonEssentials.Interface.Screen;
using PokemonUnity.Attack;
using PokemonUnity.Attack.Data;
using PokemonUnity.Combat.Data;
using PokemonUnity.Inventory;
using PokemonUnity.Monster;
using PokemonUnity.Monster.Data;
using PokemonUnity.Overworld;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

namespace PokemonUnity.Combat
{
  [Serializable]
  public class UnityBattle : 
    IBattleAI,
    IBattle,
    IBattleCommon,
    IHasDisplayMessage,
    ISerializable,
    IBattleShadowPokemon,
    IBattleClause
  {
    private int pickupUse;

    public int pbGetMoveScore(IBattleMove move, IBattler attacker, IBattler opponent, int skill = 100)
    {
      if (skill < 1)
        skill = 1;
      float a1 = 100f;
      if (move.Type == Types.SHADOW)
        a1 += 20f;
      if (!opponent.IsNotNullOrNone())
        opponent = attacker.pbOppositeOpposing;
      if (opponent.IsNotNullOrNone() && opponent.isFainted())
        opponent = opponent.pbPartner;
      int num1 = 0;
      IBattlerEffect battlerEffect1 = opponent is IBattlerEffect ? opponent as IBattlerEffect : (IBattlerEffect) null;
      IBattlerEffect battlerEffect2 = attacker is IBattlerEffect ? attacker as IBattlerEffect : (IBattlerEffect) null;
      PokemonUnity.Attack.Data.Effects[] effectsArray = new PokemonUnity.Attack.Data.Effects[0];
      int? nullable1;
      switch (move.Effect)
      {
        case PokemonUnity.Attack.Data.Effects.x002:
          if (battlerEffect1 != null && battlerEffect1.pbCanSleep(attacker, false))
          {
            a1 += 30f;
            if (skill >= 32 && opponent.effects.Yawn > 0)
              a1 -= 30f;
            if (skill >= 48 && opponent.hasWorkingAbility(Abilities.MARVEL_SCALE))
              a1 -= 30f;
            if (skill >= 100)
            {
              foreach (IBattleMove move1 in opponent.moves)
              {
                MoveData moveData = Kernal.MoveData[move1.id];
                if (moveData.Effect == PokemonUnity.Attack.Data.Effects.x062 || moveData.Effect == PokemonUnity.Attack.Data.Effects.x05D)
                {
                  a1 -= 50f;
                  break;
                }
              }
              break;
            }
            break;
          }
          if (skill >= 32 && move.basedamage == 0)
            a1 -= 90f;
          break;
        case PokemonUnity.Attack.Data.Effects.x003:
        case PokemonUnity.Attack.Data.Effects.x022:
        case PokemonUnity.Attack.Data.Effects.x04E:
          if (battlerEffect1 != null && battlerEffect1.pbCanPoison(attacker, false))
          {
            a1 += 30f;
            if (skill >= 32)
            {
              if (opponent.HP <= opponent.TotalHP / 4)
                a1 += 30f;
              if (opponent.HP <= opponent.TotalHP / 8)
                a1 += 50f;
              if (opponent.effects.Yawn > 0)
                a1 -= 40f;
            }
            if (skill >= 48)
            {
              if (this.pbRoughStat(opponent, Stats.DEFENSE, skill) > 100)
                a1 += 10f;
              if (this.pbRoughStat(opponent, Stats.SPDEF, skill) > 100)
                a1 += 10f;
              if (opponent.hasWorkingAbility(Abilities.GUTS))
                a1 -= 40f;
              if (opponent.hasWorkingAbility(Abilities.MARVEL_SCALE))
                a1 -= 40f;
              if (opponent.hasWorkingAbility(Abilities.TOXIC_BOOST))
                a1 -= 40f;
              break;
            }
            break;
          }
          if (skill >= 32 && move.basedamage == 0)
            a1 -= 90f;
          break;
        case PokemonUnity.Attack.Data.Effects.x005:
        case PokemonUnity.Attack.Data.Effects.x112:
        case PokemonUnity.Attack.Data.Effects.x14D:
          if (battlerEffect1 != null && battlerEffect1.pbCanBurn(attacker, false))
          {
            a1 += 30f;
            if (skill >= 48)
            {
              if (opponent.hasWorkingAbility(Abilities.GUTS))
                a1 -= 40f;
              if (opponent.hasWorkingAbility(Abilities.MARVEL_SCALE))
                a1 -= 40f;
              if (opponent.hasWorkingAbility(Abilities.QUICK_FEET))
                a1 -= 40f;
              if (opponent.hasWorkingAbility(Abilities.FLARE_BOOST))
                a1 -= 40f;
              break;
            }
            break;
          }
          if (skill >= 32 && move.basedamage == 0)
            a1 -= 90f;
          break;
        case PokemonUnity.Attack.Data.Effects.x006:
        case PokemonUnity.Attack.Data.Effects.x105:
        case PokemonUnity.Attack.Data.Effects.x113:
          if (battlerEffect1 != null && battlerEffect1.pbCanFreeze(attacker, false))
          {
            a1 += 30f;
            if (skill >= 48 && opponent.hasWorkingAbility(Abilities.MARVEL_SCALE))
            {
              a1 -= 20f;
              break;
            }
            break;
          }
          if (skill >= 32 && move.basedamage == 0)
            a1 -= 90f;
          break;
        case PokemonUnity.Attack.Data.Effects.x007:
        case PokemonUnity.Attack.Data.Effects.x099:
        case PokemonUnity.Attack.Data.Effects.x114:
        case PokemonUnity.Attack.Data.Effects.x14C:
          if (battlerEffect1 != null && battlerEffect1.pbCanParalyze(attacker, false))
          {
            a1 += 30f;
            if (skill >= 32)
            {
              int num2 = this.pbRoughStat(attacker, Stats.SPEED, skill);
              int num3 = this.pbRoughStat(opponent, Stats.SPEED, skill);
              if (num2 < num3)
                a1 += 30f;
              else if (num2 > num3)
                a1 -= 40f;
            }
            if (skill >= 48)
            {
              if (opponent.hasWorkingAbility(Abilities.GUTS))
                a1 -= 40f;
              if (opponent.hasWorkingAbility(Abilities.MARVEL_SCALE))
                a1 -= 40f;
              if (opponent.hasWorkingAbility(Abilities.QUICK_FEET))
                a1 -= 40f;
              break;
            }
            break;
          }
          if (skill >= 32 && move.basedamage == 0)
            a1 -= 90f;
          break;
        case PokemonUnity.Attack.Data.Effects.x008:
          int activePokemonCount1 = attacker.pbNonActivePokemonCount;
          int activePokemonCount2 = attacker.pbOppositeOpposing.pbNonActivePokemonCount;
          if (this.pbCheckGlobalAbility(Abilities.DAMP).IsNotNullOrNone())
          {
            a1 -= 100f;
            break;
          }
          if (skill >= 32 && activePokemonCount1 == 0 && activePokemonCount2 > 0)
          {
            a1 -= 100f;
            break;
          }
          if (skill >= 48 && activePokemonCount1 == 0 && activePokemonCount2 == 0)
          {
            a1 -= 100f;
            break;
          }
          a1 -= (float) (attacker.HP * 100 / attacker.TotalHP);
          break;
        case PokemonUnity.Attack.Data.Effects.x009:
          if (opponent.Status != Status.SLEEP)
          {
            a1 -= 100f;
            break;
          }
          if (skill >= 48 && opponent.hasWorkingAbility(Abilities.LIQUID_OOZE))
          {
            a1 -= 70f;
            break;
          }
          if (attacker.HP <= attacker.TotalHP / 2)
            a1 += 20f;
          break;
        case PokemonUnity.Attack.Data.Effects.x00A:
          a1 -= 40f;
          if (skill >= 48 && (opponent.lastMoveUsed <= Moves.NONE || Kernal.MoveData[opponent.lastMoveUsed].Flags.Mirror))
          {
            a1 -= 100f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x00B:
          if (move.basedamage == 0)
          {
            if (battlerEffect2 != null && battlerEffect2.pbTooHigh(Stats.ATTACK))
            {
              a1 -= 90f;
              break;
            }
            a1 -= (float) (attacker.stages[1] * 20);
            if (skill >= 32)
            {
              bool flag = false;
              foreach (IBattleMove move2 in attacker.moves)
              {
                if (move2.id != Moves.NONE && move2.basedamage > 0 && move2.pbIsPhysical(move2.Type))
                  flag = true;
              }
              if (flag)
                a1 += 20f;
              else if (skill >= 48)
                a1 -= 90f;
            }
            break;
          }
          if (attacker.stages[1] < 0)
            a1 += 20f;
          if (skill >= 32)
          {
            bool flag = false;
            foreach (IBattleMove move3 in attacker.moves)
            {
              if (move3.id != Moves.NONE && move3.basedamage > 0 && move3.pbIsPhysical(move3.Type))
                flag = true;
            }
            if (flag)
              a1 += 20f;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x00C:
        case PokemonUnity.Attack.Data.Effects.x092:
        case PokemonUnity.Attack.Data.Effects.x09D:
          if (move.basedamage == 0)
          {
            if (battlerEffect2 != null && battlerEffect2.pbTooHigh(Stats.DEFENSE))
            {
              a1 -= 90f;
              break;
            }
            a1 -= (float) (attacker.stages[2] * 20);
            break;
          }
          if (attacker.stages[2] < 0)
            a1 += 20f;
          break;
        case PokemonUnity.Attack.Data.Effects.x011:
          if (move.basedamage == 0)
          {
            if (battlerEffect2 != null && battlerEffect2.pbTooHigh(Stats.EVASION))
            {
              a1 -= 90f;
              break;
            }
            a1 -= (float) (attacker.stages[7] * 10);
            break;
          }
          if (attacker.stages[7] < 0)
            a1 += 20f;
          break;
        case PokemonUnity.Attack.Data.Effects.x014:
          if (move.basedamage == 0)
          {
            if (battlerEffect1 != null && !battlerEffect1.pbCanReduceStatStage(Stats.DEFENSE, attacker))
            {
              a1 -= 90f;
              break;
            }
            a1 += (float) (opponent.stages[2] * 20);
            break;
          }
          if (opponent.stages[2] > 0)
            a1 += 20f;
          break;
        case PokemonUnity.Attack.Data.Effects.x015:
          if (move.basedamage == 0)
          {
            if (battlerEffect1 != null && !battlerEffect1.pbCanReduceStatStage(Stats.SPEED, attacker))
            {
              a1 -= 90f;
              break;
            }
            a1 += (float) (opponent.stages[3] * 10);
            if (skill >= 48)
            {
              int num4 = this.pbRoughStat(attacker, Stats.SPEED, skill);
              int num5 = this.pbRoughStat(opponent, Stats.SPEED, skill);
              if (num4 < num5 && num4 * 2 > num5)
                a1 += 30f;
            }
            break;
          }
          if (attacker.stages[3] > 0)
            a1 += 20f;
          break;
        case PokemonUnity.Attack.Data.Effects.x018:
          if (move.basedamage == 0)
          {
            if (battlerEffect1 != null && !battlerEffect1.pbCanReduceStatStage(Stats.ACCURACY, attacker))
            {
              a1 -= 90f;
              break;
            }
            a1 += (float) (opponent.stages[6] * 10);
            break;
          }
          if (opponent.stages[6] > 0)
            a1 += 20f;
          break;
        case PokemonUnity.Attack.Data.Effects.x019:
          if (move.basedamage == 0)
          {
            if (battlerEffect1 != null && !battlerEffect1.pbCanReduceStatStage(Stats.EVASION, attacker))
            {
              a1 -= 90f;
              break;
            }
            a1 += (float) (opponent.stages[7] * 10);
            break;
          }
          if (opponent.stages[7] > 0)
            a1 += 20f;
          break;
        case PokemonUnity.Attack.Data.Effects.x01A:
          if (skill >= 32)
          {
            int num6 = 0;
            for (int i = 0; i < 4; ++i)
            {
              IBattler battler = this.battlers[i];
              num6 = !attacker.pbIsOpposing(i) ? num6 - battler.stages[1] - battler.stages[2] - battler.stages[3] - battler.stages[4] - battler.stages[5] - battler.stages[7] - battler.stages[6] : num6 + battler.stages[1] + battler.stages[2] + battler.stages[3] + battler.stages[4] + battler.stages[5] + battler.stages[7] + battler.stages[6];
            }
            a1 += (float) (num6 * 10);
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x01B:
          if (attacker.HP <= attacker.TotalHP / 4)
          {
            a1 -= 90f;
            break;
          }
          if (attacker.HP <= attacker.TotalHP / 2)
          {
            a1 -= 50f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x01D:
          if (opponent.effects.Ingrain || skill >= 48 && opponent.hasWorkingAbility(Abilities.SUCTION_CUPS))
          {
            a1 -= 90f;
          }
          else
          {
            IPokemon[] pokemonArray = this.pbParty(opponent.Index);
            int num7 = 0;
            for (int pkmnidxTo = 0; pkmnidxTo < pokemonArray.Length; ++pkmnidxTo)
            {
              if (this.pbCanSwitchLax(opponent.Index, pkmnidxTo, false))
                ++num7;
            }
            if (num7 == 0)
              a1 -= 90f;
          }
          if ((double) a1 > 20.0)
          {
            if (opponent.pbOwnSide.Spikes > (byte) 0)
              a1 += 50f;
            if (opponent.pbOwnSide.ToxicSpikes > (byte) 0)
              a1 += 50f;
            if (opponent.pbOwnSide.StealthRock)
              a1 += 50f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x01F:
          if (attacker.Ability == Abilities.MULTITYPE)
          {
            a1 -= 90f;
            break;
          }
          List<Types> typesList1 = new List<Types>();
          foreach (IBattleMove move4 in attacker.moves)
          {
            if (move4.id != move.id && !attacker.pbHasType(move4.Type))
            {
              if (!typesList1.Contains(move4.Type))
                typesList1.Add(move4.Type);
            }
          }
          if (typesList1.Count == 0)
            a1 -= 90f;
          break;
        case PokemonUnity.Attack.Data.Effects.x020:
          a1 += 30f;
          if (skill >= 48 && !opponent.hasWorkingAbility(Abilities.INNER_FOCUS) && opponent.effects.Substitute == 0)
          {
            a1 += 30f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x021:
        case PokemonUnity.Attack.Data.Effects.x0D7:
          if (attacker.HP == attacker.TotalHP)
          {
            a1 -= 90f;
            break;
          }
          a1 = a1 + 50f - (float) (attacker.HP * 100 / attacker.TotalHP);
          break;
        case PokemonUnity.Attack.Data.Effects.x024:
          if (attacker.pbOwnSide.LightScreen > (byte) 0)
          {
            a1 -= 90f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x025:
          if (opponent.Status == Status.NONE)
          {
            a1 += 30f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x026:
          if (attacker.HP == attacker.TotalHP || (battlerEffect2 != null ? (battlerEffect2.pbCanSleep(attacker, false, ignorestatus: true) ? 1 : 0) : 0) == 0)
          {
            a1 -= 90f;
            break;
          }
          a1 = a1 + 70f - (float) (attacker.HP * 140 / attacker.TotalHP);
          if ((uint) attacker.Status > 0U)
            a1 += 30f;
          break;
        case PokemonUnity.Attack.Data.Effects.x027:
          if (opponent.hasWorkingAbility(Abilities.STURDY))
            a1 -= 90f;
          if (opponent.Level > attacker.Level)
          {
            a1 -= 90f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x029:
          a1 = a1 - 50f + (float) (int) Math.Floor((double) opponent.HP * 100.0 / (double) opponent.TotalHP);
          break;
        case PokemonUnity.Attack.Data.Effects.x02A:
          if (opponent.HP <= 40)
          {
            a1 += 80f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x02B:
          if (opponent.effects.MultiTurn == 0)
          {
            a1 += 40f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x02E:
          a1 += (float) (10 * (attacker.stages[6] - opponent.stages[7]));
          break;
        case PokemonUnity.Attack.Data.Effects.x02F:
          if (attacker.pbOwnSide.Mist > (byte) 0)
          {
            a1 -= 80f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x030:
          if (move.basedamage == 0)
          {
            if (attacker.effects.FocusEnergy >= 2)
            {
              a1 -= 80f;
              break;
            }
            a1 += 30f;
            break;
          }
          if (attacker.effects.FocusEnergy < 2)
            a1 += 30f;
          break;
        case PokemonUnity.Attack.Data.Effects.x031:
          a1 -= 25f;
          break;
        case PokemonUnity.Attack.Data.Effects.x033:
          if (move.basedamage == 0)
          {
            if (battlerEffect2 != null && battlerEffect2.pbTooHigh(Stats.ATTACK))
            {
              a1 -= 90f;
              break;
            }
            if (attacker.turncount == 0)
              a1 += 40f;
            a1 -= (float) (attacker.stages[1] * 20);
            if (skill >= 32)
            {
              bool flag = false;
              foreach (IBattleMove move5 in attacker.moves)
              {
                if (move5.id != Moves.NONE && move5.basedamage > 0 && move5.pbIsPhysical(move5.Type))
                  flag = true;
              }
              if (flag)
                a1 += 20f;
              else if (skill >= 48)
                a1 -= 90f;
            }
            break;
          }
          if (attacker.turncount == 0)
            a1 += 10f;
          if (attacker.stages[1] < 0)
            a1 += 20f;
          if (skill >= 32)
          {
            bool flag = false;
            foreach (IBattleMove move6 in attacker.moves)
            {
              if (move6.id != Moves.NONE && move6.basedamage > 0 && move6.pbIsPhysical(move6.Type))
                flag = true;
            }
            if (flag)
              a1 += 20f;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x034:
          if (move.basedamage == 0)
          {
            if (battlerEffect2 != null && battlerEffect2.pbTooHigh(Stats.DEFENSE))
            {
              a1 -= 90f;
              break;
            }
            if (attacker.turncount == 0)
              a1 += 40f;
            a1 -= (float) (attacker.stages[2] * 20);
            break;
          }
          if (attacker.turncount == 0)
            a1 += 10f;
          if (attacker.stages[2] < 0)
            a1 += 20f;
          break;
        case PokemonUnity.Attack.Data.Effects.x035:
        case PokemonUnity.Attack.Data.Effects.x11D:
          if (move.basedamage == 0)
          {
            if (battlerEffect2 != null && battlerEffect2.pbTooHigh(Stats.SPEED))
            {
              a1 -= 90f;
              break;
            }
            if (attacker.turncount == 0)
              a1 += 20f;
            a1 -= (float) (attacker.stages[3] * 10);
            if (skill >= 48)
            {
              int num8 = this.pbRoughStat(attacker, Stats.SPEED, skill);
              int num9 = this.pbRoughStat(opponent, Stats.SPEED, skill);
              if (num8 < num9 && num8 * 2 > num9)
                a1 += 30f;
            }
            break;
          }
          if (attacker.turncount == 0)
            a1 += 10f;
          if (attacker.stages[3] < 0)
            a1 += 20f;
          break;
        case PokemonUnity.Attack.Data.Effects.x036:
          if (move.basedamage == 0)
          {
            if (battlerEffect2 != null && battlerEffect2.pbTooHigh(Stats.SPATK))
            {
              a1 -= 90f;
              break;
            }
            if (attacker.turncount == 0)
              a1 += 40f;
            a1 -= (float) (attacker.stages[4] * 20);
            if (skill >= 32)
            {
              bool flag = false;
              foreach (IBattleMove move7 in attacker.moves)
              {
                if (move7.id != Moves.NONE && move7.basedamage > 0 && move7.pbIsSpecial(move7.Type))
                  flag = true;
              }
              if (flag)
                a1 += 20f;
              else if (skill >= 48)
                a1 -= 90f;
            }
            break;
          }
          if (attacker.turncount == 0)
            a1 += 10f;
          if (attacker.stages[4] < 0)
            a1 += 20f;
          if (skill >= 32)
          {
            bool flag = false;
            foreach (IBattleMove move8 in attacker.moves)
            {
              if (move8.id != Moves.NONE && move8.basedamage > 0 && move8.pbIsSpecial(move8.Type))
                flag = true;
            }
            if (flag)
              a1 += 20f;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x037:
          if (move.basedamage == 0)
          {
            if (battlerEffect2 != null && battlerEffect2.pbTooHigh(Stats.SPDEF))
            {
              a1 -= 90f;
              break;
            }
            if (attacker.turncount == 0)
              a1 += 40f;
            a1 -= (float) (attacker.stages[5] * 20);
            break;
          }
          if (attacker.turncount == 0)
            a1 += 10f;
          if (attacker.stages[5] < 0)
            a1 += 20f;
          break;
        case PokemonUnity.Attack.Data.Effects.x03A:
          a1 -= 70f;
          break;
        case PokemonUnity.Attack.Data.Effects.x03B:
          if (move.basedamage == 0)
          {
            if (battlerEffect1 != null && !battlerEffect1.pbCanReduceStatStage(Stats.ATTACK, attacker))
            {
              a1 -= 90f;
              break;
            }
            if (attacker.turncount == 0)
              a1 += 40f;
            a1 += (float) (opponent.stages[1] * 20);
            if (skill >= 32)
            {
              bool flag = false;
              foreach (IBattleMove move9 in opponent.moves)
              {
                if (move9.id != Moves.NONE && move9.basedamage > 0 && move9.pbIsPhysical(move9.Type))
                  flag = true;
              }
              if (flag)
                a1 += 20f;
              else if (skill >= 48)
                a1 -= 90f;
            }
            break;
          }
          if (attacker.turncount == 0)
            a1 += 10f;
          if (opponent.stages[1] > 0)
            a1 += 20f;
          if (skill >= 32)
          {
            bool flag = false;
            foreach (IBattleMove move10 in opponent.moves)
            {
              if (move10.id != Moves.NONE && move10.basedamage > 0 && move10.pbIsPhysical(move10.Type))
                flag = true;
            }
            if (flag)
              a1 += 20f;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x03C:
          if (move.basedamage == 0)
          {
            if (battlerEffect1 != null && !battlerEffect1.pbCanReduceStatStage(Stats.DEFENSE, attacker))
            {
              a1 -= 90f;
              break;
            }
            if (attacker.turncount == 0)
              a1 += 40f;
            a1 += (float) (opponent.stages[2] * 20);
            break;
          }
          if (attacker.turncount == 0)
            a1 += 10f;
          if (opponent.stages[2] > 0)
            a1 += 20f;
          break;
        case PokemonUnity.Attack.Data.Effects.x03D:
          if (move.basedamage == 0)
          {
            if (battlerEffect1 != null && !battlerEffect1.pbCanReduceStatStage(Stats.SPEED, attacker))
            {
              a1 -= 90f;
              break;
            }
            if (attacker.turncount == 0)
              a1 += 20f;
            a1 += (float) (opponent.stages[3] * 20);
            if (skill >= 48)
            {
              int num10 = this.pbRoughStat(attacker, Stats.SPEED, skill);
              int num11 = this.pbRoughStat(opponent, Stats.SPEED, skill);
              if (num10 < num11 && num10 * 2 > num11)
                a1 += 30f;
            }
            break;
          }
          if (attacker.turncount == 0)
            a1 += 10f;
          if (opponent.stages[3] > 0)
            a1 += 30f;
          break;
        case PokemonUnity.Attack.Data.Effects.x03E:
          if ((battlerEffect1 != null ? (battlerEffect1.pbCanReduceStatStage(Stats.SPATK, attacker) ? 1 : 0) : 0) == 0)
          {
            a1 -= 90f;
            break;
          }
          if (attacker.turncount == 0)
            a1 += 40f;
          a1 += (float) (opponent.stages[4] * 20);
          break;
        case PokemonUnity.Attack.Data.Effects.x03F:
          if (move.basedamage == 0)
          {
            if (battlerEffect1 != null && !battlerEffect1.pbCanReduceStatStage(Stats.SPDEF, attacker))
            {
              a1 -= 90f;
              break;
            }
            if (attacker.turncount == 0)
              a1 += 40f;
            a1 += (float) (opponent.stages[5] * 20);
            break;
          }
          if (attacker.turncount == 0)
            a1 += 10f;
          if (opponent.stages[5] > 0)
            a1 += 20f;
          break;
        case PokemonUnity.Attack.Data.Effects.x042:
          if (attacker.pbOwnSide.Reflect > (byte) 0)
          {
            a1 -= 90f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x048:
          if (move.basedamage == 0)
          {
            if (battlerEffect1 != null && !battlerEffect1.pbCanReduceStatStage(Stats.SPATK, attacker))
            {
              a1 -= 90f;
              break;
            }
            a1 += (float) (attacker.stages[4] * 20);
            if (skill >= 32)
            {
              bool flag = false;
              foreach (IBattleMove move11 in opponent.moves)
              {
                if (move11.id != Moves.NONE && move11.basedamage > 0 && move11.pbIsSpecial(move11.Type))
                  flag = true;
              }
              if (flag)
                a1 += 20f;
              else if (skill >= 48)
                a1 -= 90f;
            }
            break;
          }
          if (attacker.stages[4] > 0)
            a1 += 20f;
          if (skill >= 32)
          {
            bool flag = false;
            foreach (IBattleMove move12 in opponent.moves)
            {
              if (move12.id != Moves.NONE && move12.basedamage > 0 && move12.pbIsSpecial(move12.Type))
                flag = true;
            }
            if (flag)
              a1 += 20f;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x049:
          if (move.basedamage == 0)
          {
            if (battlerEffect1 != null && !battlerEffect1.pbCanReduceStatStage(Stats.SPDEF, attacker))
            {
              a1 -= 90f;
              break;
            }
            a1 += (float) (opponent.stages[5] * 20);
            break;
          }
          if (opponent.stages[5] > 0)
            a1 += 20f;
          break;
        case PokemonUnity.Attack.Data.Effects.x04C:
          if (attacker.effects.FocusEnergy > 0)
            a1 += 20f;
          if (skill >= 48 && !opponent.hasWorkingAbility(Abilities.INNER_FOCUS) && opponent.effects.Substitute == 0)
          {
            a1 += 20f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x04D:
        case PokemonUnity.Attack.Data.Effects.x10C:
        case PokemonUnity.Attack.Data.Effects.x14E:
          if (battlerEffect1 != null && battlerEffect1.pbCanConfuse(attacker, false))
          {
            a1 += 30f;
            break;
          }
          if (skill >= 32 && move.basedamage == 0)
            a1 -= 90f;
          break;
        case PokemonUnity.Attack.Data.Effects.x050:
          if (attacker.effects.Substitute > 0)
          {
            a1 -= 90f;
            break;
          }
          if (attacker.HP <= attacker.TotalHP / 4)
          {
            a1 -= 90f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x052:
          if (attacker.effects.Rage)
          {
            a1 += 25f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x053:
          PokemonUnity.Attack.Data.Effects[] source1 = new PokemonUnity.Attack.Data.Effects[5]
          {
            PokemonUnity.Attack.Data.Effects.x0FF,
            PokemonUnity.Attack.Data.Effects.x10C,
            PokemonUnity.Attack.Data.Effects.x053,
            PokemonUnity.Attack.Data.Effects.x060,
            PokemonUnity.Attack.Data.Effects.x054
          };
          if (attacker.effects.Transform || opponent.lastMoveUsed <= Moves.NONE || Kernal.MoveData[opponent.lastMoveUsed].Type == Types.SHADOW || ((IEnumerable<PokemonUnity.Attack.Data.Effects>) source1).Contains<PokemonUnity.Attack.Data.Effects>(Kernal.MoveData[opponent.lastMoveUsed].Effect))
            a1 -= 90f;
          foreach (IBattleMove move13 in attacker.moves)
          {
            if (move13.id == opponent.lastMoveUsed)
            {
              a1 -= 90f;
              break;
            }
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x055:
          if (opponent.effects.LeechSeed >= 0)
          {
            a1 -= 90f;
            break;
          }
          if (skill >= 32 && opponent.pbHasType(Types.GRASS))
          {
            a1 -= 90f;
            break;
          }
          if (attacker.turncount == 0)
            a1 += 60f;
          break;
        case PokemonUnity.Attack.Data.Effects.x056:
          a1 -= 95f;
          if (skill >= 48)
          {
            a1 = 0.0f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x057:
          if (opponent.effects.Disable > 0)
          {
            a1 -= 90f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x058:
          if (opponent.HP <= attacker.Level)
          {
            a1 += 80f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x059:
          if (opponent.HP <= attacker.Level)
          {
            a1 += 30f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x05A:
          if (opponent.effects.HyperBeam > 0)
          {
            a1 -= 90f;
            break;
          }
          if ((double) this.pbRoughStat(attacker, Stats.ATTACK, skill) * 1.5 < (double) this.pbRoughStat(attacker, Stats.SPATK, skill))
            a1 -= 60f;
          else if (skill >= 32 && opponent.lastMoveUsed > Moves.NONE)
          {
            MoveData moveData = Kernal.MoveData[opponent.lastMoveUsed];
            nullable1 = moveData.Power;
            int num12 = 0;
            if (nullable1.GetValueOrDefault() > num12 & nullable1.HasValue && moveData.Category == Category.STATUS)
              a1 -= 60f;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x05B:
          int num13 = this.pbRoughStat(attacker, Stats.SPEED, skill);
          int num14 = this.pbRoughStat(opponent, Stats.SPEED, skill);
          if (opponent.effects.Encore > 0)
          {
            a1 -= 90f;
            break;
          }
          if (num13 > num14)
          {
            if (opponent.lastMoveUsed <= Moves.NONE)
            {
              a1 -= 90f;
            }
            else
            {
              MoveData moveData = Kernal.MoveData[opponent.lastMoveUsed];
              nullable1 = moveData.Power;
              int num15 = 0;
              if (nullable1.GetValueOrDefault() == num15 & nullable1.HasValue && (moveData.Target == (Targets) 16 || moveData.Target == (Targets) 32))
              {
                a1 += 60f;
              }
              else
              {
                nullable1 = moveData.Power;
                int num16 = 0;
                if (!(nullable1.GetValueOrDefault() == num16 & nullable1.HasValue) && moveData.Target == (Targets) 0 && (double) this.pbTypeModifier(moveData.Type, opponent, attacker) == 0.0)
                  a1 += 60f;
              }
            }
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x05C:
          if (opponent.effects.Substitute > 0)
          {
            a1 -= 90f;
            break;
          }
          if (attacker.HP >= (attacker.HP + opponent.HP) / 2)
          {
            a1 -= 90f;
            break;
          }
          a1 += 40f;
          break;
        case PokemonUnity.Attack.Data.Effects.x05D:
          if (attacker.Status == Status.SLEEP)
          {
            a1 += 100f;
            if (skill >= 48 && !opponent.hasWorkingAbility(Abilities.INNER_FOCUS) && opponent.effects.Substitute == 0)
            {
              a1 += 30f;
              break;
            }
            break;
          }
          a1 -= 90f;
          if (skill >= 100)
            a1 = 0.0f;
          break;
        case PokemonUnity.Attack.Data.Effects.x05E:
          if (attacker.Ability == Abilities.MULTITYPE)
          {
            a1 -= 90f;
            break;
          }
          if (opponent.lastMoveUsed <= Moves.NONE)
          {
            a1 -= 90f;
            break;
          }
          Types atk = Types.UNKNOWN;
          foreach (IBattleMove move14 in opponent.moves)
          {
            if (move14.id == opponent.lastMoveUsed)
            {
              atk = move14.pbType(move.Type, attacker, opponent);
              break;
            }
          }
          if (atk < Types.NONE)
          {
            a1 -= 90f;
          }
          else
          {
            List<Types> typesList2 = new List<Types>();
            foreach (Types key in (IEnumerable<Types>) Kernal.TypeData.Keys)
            {
              if (!attacker.pbHasType(key) && (double) atk.GetEffectiveness(key) < 2.0)
                typesList2.Add(key);
            }
            if (typesList2.Count == 0)
              a1 -= 90f;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x05F:
          if (opponent.effects.Substitute > 0)
            a1 -= 90f;
          if (opponent.effects.LockOn > 0)
          {
            a1 -= 90f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x060:
          PokemonUnity.Attack.Data.Effects[] source2 = new PokemonUnity.Attack.Data.Effects[3]
          {
            PokemonUnity.Attack.Data.Effects.x0FF,
            PokemonUnity.Attack.Data.Effects.x10C,
            PokemonUnity.Attack.Data.Effects.x060
          };
          if (attacker.effects.Transform || opponent.lastMoveUsedSketch <= Moves.NONE || Kernal.MoveData[opponent.lastMoveUsedSketch].Type == Types.SHADOW || ((IEnumerable<PokemonUnity.Attack.Data.Effects>) source2).Contains<PokemonUnity.Attack.Data.Effects>(Kernal.MoveData[opponent.lastMoveUsedSketch].Effect))
            a1 -= 90f;
          foreach (IBattleMove move15 in attacker.moves)
          {
            if (move15.id == opponent.lastMoveUsedSketch)
            {
              a1 -= 90f;
              break;
            }
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x062:
          if (attacker.Status == Status.SLEEP)
          {
            a1 += 200f;
            break;
          }
          a1 -= 80f;
          break;
        case PokemonUnity.Attack.Data.Effects.x063:
          a1 = a1 + 50f - (float) (attacker.HP * 100 / attacker.TotalHP);
          if (attacker.HP <= attacker.TotalHP / 10)
          {
            a1 += 30f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x065:
          a1 -= 40f;
          break;
        case PokemonUnity.Attack.Data.Effects.x066:
          if (opponent.HP == 1)
          {
            a1 -= 90f;
            break;
          }
          if (opponent.HP <= opponent.TotalHP / 8)
          {
            a1 -= 60f;
            break;
          }
          if (opponent.HP <= opponent.TotalHP / 4)
          {
            a1 -= 30f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x067:
          IPokemon[] pokemonArray1 = this.pbParty(attacker.Index);
          int num17 = 0;
          for (int index = 0; index < pokemonArray1.Length; ++index)
          {
            if (pokemonArray1[index].IsNotNullOrNone() && (uint) pokemonArray1[index].Status > 0U)
              ++num17;
          }
          if (num17 == 0)
          {
            a1 -= 80f;
            break;
          }
          a1 += (float) (20 * num17);
          break;
        case PokemonUnity.Attack.Data.Effects.x06A:
          if (skill >= 48)
          {
            if (attacker.Item == Items.NONE && (uint) opponent.Item > 0U)
            {
              a1 += 40f;
              break;
            }
            a1 -= 90f;
            break;
          }
          a1 -= 80f;
          break;
        case PokemonUnity.Attack.Data.Effects.x06C:
          if (opponent.effects.Nightmare || opponent.effects.Substitute > 0)
          {
            a1 -= 90f;
            break;
          }
          if (opponent.Status != Status.SLEEP)
          {
            a1 -= 90f;
            break;
          }
          if (opponent.StatusCount <= 1)
            a1 -= 90f;
          if (opponent.StatusCount > 3)
            a1 += 50f;
          break;
        case PokemonUnity.Attack.Data.Effects.x06D:
          if (move.basedamage == 0)
          {
            if (battlerEffect2 != null && battlerEffect2.pbTooHigh(Stats.EVASION))
            {
              a1 -= 90f;
              break;
            }
            if (attacker.turncount == 0)
              a1 += 40f;
            a1 -= (float) (attacker.stages[7] * 10);
            break;
          }
          if (attacker.turncount == 0)
            a1 += 10f;
          if (attacker.stages[7] < 0)
            a1 += 20f;
          break;
        case PokemonUnity.Attack.Data.Effects.x06E:
          if (attacker.pbHasType(Types.GHOST))
          {
            if (opponent.effects.Curse)
            {
              a1 -= 90f;
              break;
            }
            if (attacker.HP <= attacker.TotalHP / 2)
            {
              if (attacker.pbNonActivePokemonCount == 0)
              {
                a1 -= 90f;
              }
              else
              {
                a1 -= 50f;
                if (this.shiftStyle)
                  a1 -= 30f;
              }
              break;
            }
            break;
          }
          int num18 = attacker.stages[3] * 10 - attacker.stages[1] * 10 - attacker.stages[2] * 10;
          a1 += (float) (num18 / 3);
          break;
        case PokemonUnity.Attack.Data.Effects.x070:
          if (attacker.effects.ProtectRate > (short) 1 || opponent.effects.HyperBeam > 0)
          {
            a1 -= 90f;
            break;
          }
          if (skill >= 32)
            a1 -= (float) ((int) attacker.effects.ProtectRate * 40);
          if (attacker.turncount == 0)
            a1 += 50f;
          if ((uint) opponent.effects.TwoTurnAttack > 0U)
            a1 += 30f;
          break;
        case PokemonUnity.Attack.Data.Effects.x071:
          if (attacker.pbOpposingSide.Spikes >= (byte) 3)
          {
            a1 -= 90f;
            break;
          }
          if (!this.pbCanChooseNonActive(attacker.pbOpposing1.Index) && !this.pbCanChooseNonActive(attacker.pbOpposing2.Index))
          {
            a1 -= 90f;
            break;
          }
          a1 = (float) ((double) (a1 + (float) (5 * attacker.pbOppositeOpposing.pbNonActivePokemonCount)) + (double) new int[3]
          {
            40,
            26,
            13
          }[(int) attacker.pbOpposingSide.Spikes]);
          break;
        case PokemonUnity.Attack.Data.Effects.x072:
          if (opponent.effects.Foresight)
          {
            a1 -= 90f;
            break;
          }
          if (opponent.pbHasType(Types.GHOST))
          {
            a1 += 70f;
            break;
          }
          if (opponent.stages[7] <= 0)
          {
            a1 -= 60f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x073:
          if (attacker.pbNonActivePokemonCount == 0)
          {
            a1 -= 90f;
            break;
          }
          if (opponent.effects.PerishSong > 0)
            a1 -= 90f;
          break;
        case PokemonUnity.Attack.Data.Effects.x074:
          if (this.pbCheckGlobalAbility(Abilities.AIR_LOCK).IsNotNullOrNone() || this.pbCheckGlobalAbility(Abilities.CLOUD_NINE).IsNotNullOrNone())
          {
            a1 -= 90f;
            break;
          }
          if (this.pbWeather == Weather.SANDSTORM)
          {
            a1 -= 90f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x075:
          if (attacker.HP > attacker.TotalHP / 2)
            a1 -= 25f;
          if (skill >= 32)
          {
            if (attacker.effects.ProtectRate > (short) 1)
              a1 -= 90f;
            if (opponent.effects.HyperBeam > 0)
            {
              a1 -= 90f;
              break;
            }
            break;
          }
          a1 -= (float) ((int) attacker.effects.ProtectRate * 40);
          break;
        case PokemonUnity.Attack.Data.Effects.x077:
          if (battlerEffect1 != null && !battlerEffect1.pbCanConfuse(attacker, false))
          {
            a1 -= 90f;
            break;
          }
          if (opponent.stages[1] < 0)
            a1 += 30f;
          break;
        case PokemonUnity.Attack.Data.Effects.x079:
          bool flag1 = true;
          bool? gender1 = attacker.Gender;
          bool? gender2 = opponent.Gender;
          int num19;
          if (gender1.HasValue && gender2.HasValue)
          {
            bool? nullable2 = gender1;
            bool? nullable3 = gender2;
            num19 = nullable2.GetValueOrDefault() == nullable3.GetValueOrDefault() & nullable2.HasValue == nullable3.HasValue ? 1 : 0;
          }
          else
            num19 = 1;
          if (num19 != 0)
          {
            a1 -= 90f;
            flag1 = false;
          }
          else if (opponent.effects.Attract >= 0)
          {
            a1 -= 80f;
            flag1 = false;
          }
          else if (skill >= 100 && opponent.hasWorkingAbility(Abilities.OBLIVIOUS))
          {
            a1 -= 80f;
            flag1 = false;
          }
          if (skill >= 48 && flag1 && opponent.hasWorkingItem(Items.DESTINY_KNOT) && battlerEffect2 != null && battlerEffect2.pbCanAttract(opponent, false))
          {
            a1 -= 30f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x07D:
          if (attacker.pbOwnSide.Safeguard > (byte) 0)
          {
            a1 -= 80f;
            break;
          }
          if ((uint) attacker.Status > 0U)
          {
            a1 -= 40f;
            break;
          }
          a1 += 30f;
          break;
        case PokemonUnity.Attack.Data.Effects.x080:
          if (!this.pbCanChooseNonActive(attacker.Index))
          {
            a1 -= 80f;
            break;
          }
          if (attacker.effects.Confusion > 0)
            a1 -= 40f;
          int num20 = 0 + attacker.stages[1] * 10 + attacker.stages[2] * 10 + attacker.stages[3] * 10 + attacker.stages[4] * 10 + attacker.stages[5] * 10 + attacker.stages[7] * 10 + attacker.stages[6] * 10;
          if (num20 <= 0 || attacker.turncount == 0)
          {
            a1 -= 60f;
          }
          else
          {
            a1 += (float) num20;
            bool flag2 = false;
            foreach (IBattleMove move16 in attacker.moves)
            {
              if (move.id != Moves.NONE && move.basedamage > 0)
                flag2 = true;
            }
            if (!flag2)
              a1 += 75f;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x082:
          if (attacker.effects.MultiTurn > 0)
            a1 += 30f;
          if (attacker.effects.LeechSeed >= 0)
            a1 += 30f;
          if (attacker.pbNonActivePokemonCount > 0)
          {
            if (attacker.pbOwnSide.Spikes > (byte) 0)
              a1 += 80f;
            if (attacker.pbOwnSide.ToxicSpikes > (byte) 0)
              a1 += 80f;
            if (attacker.pbOwnSide.StealthRock)
              a1 += 80f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x083:
          if (opponent.HP <= 20)
          {
            a1 += 80f;
            break;
          }
          if (opponent.Level >= 25)
          {
            a1 -= 80f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x085:
          if (attacker.HP == attacker.TotalHP)
          {
            a1 -= 90f;
            break;
          }
          switch (this.pbWeather)
          {
            case Weather.RAINDANCE:
            case Weather.SANDSTORM:
            case Weather.HAIL:
              a1 -= 30f;
              break;
            case Weather.SUNNYDAY:
              a1 += 30f;
              break;
          }
          a1 = a1 + 50f - (float) (attacker.HP * 100 / attacker.TotalHP);
          break;
        case PokemonUnity.Attack.Data.Effects.x089:
          if (this.pbCheckGlobalAbility(Abilities.AIR_LOCK).IsNotNullOrNone() || this.pbCheckGlobalAbility(Abilities.CLOUD_NINE).IsNotNullOrNone())
          {
            a1 -= 90f;
            break;
          }
          if (this.pbWeather == Weather.RAINDANCE)
          {
            a1 -= 90f;
            break;
          }
          foreach (IBattleMove move17 in attacker.moves)
          {
            if (move17.id != Moves.NONE && move17.basedamage > 0 && move17.Type == Types.WATER)
              a1 += 20f;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x08A:
          if (this.pbCheckGlobalAbility(Abilities.AIR_LOCK).IsNotNullOrNone() || this.pbCheckGlobalAbility(Abilities.CLOUD_NINE).IsNotNullOrNone())
          {
            a1 -= 90f;
            break;
          }
          if (this.pbWeather == Weather.SUNNYDAY)
          {
            a1 -= 90f;
            break;
          }
          foreach (IBattleMove move18 in attacker.moves)
          {
            if (move18.id != Moves.NONE && move18.basedamage > 0 && move18.Type == Types.FIRE)
              a1 += 20f;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x08D:
          if (attacker.stages[1] < 0)
            a1 += 10f;
          if (attacker.stages[2] < 0)
            a1 += 10f;
          if (attacker.stages[3] < 0)
            a1 += 10f;
          if (attacker.stages[4] < 0)
            a1 += 10f;
          if (attacker.stages[5] < 0)
            a1 += 10f;
          if (skill >= 32)
          {
            bool flag3 = false;
            foreach (IBattleMove move19 in attacker.moves)
            {
              if (move19.id != Moves.NONE && move19.basedamage > 0)
                flag3 = true;
            }
            if (flag3)
              a1 += 20f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x08F:
          if (battlerEffect2 != null && battlerEffect2.pbTooHigh(Stats.ATTACK) || attacker.HP <= attacker.TotalHP / 2)
          {
            a1 -= 100f;
            break;
          }
          a1 += (float) ((6 - attacker.stages[1]) * 10);
          if (skill >= 32)
          {
            bool flag4 = false;
            foreach (IBattleMove move20 in attacker.moves)
            {
              if (move20.id != Moves.NONE && move20.basedamage > 0 && move20.pbIsPhysical(move20.Type))
                flag4 = true;
            }
            if (flag4)
              a1 += 40f;
            else if (skill >= 48)
              a1 -= 90f;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x090:
          if (skill >= 32)
          {
            bool flag5 = true;
            Stats[] statsArray = new Stats[7]
            {
              Stats.ATTACK,
              Stats.DEFENSE,
              Stats.SPEED,
              Stats.SPATK,
              Stats.SPDEF,
              Stats.ACCURACY,
              Stats.EVASION
            };
            foreach (Stats index in statsArray)
            {
              int num21 = opponent.stages[(int) index] - attacker.stages[(int) index];
              a1 += (float) (num21 * 10);
              if ((uint) num21 > 0U)
                flag5 = false;
            }
            if (flag5)
            {
              a1 -= 80f;
              break;
            }
            break;
          }
          a1 -= 50f;
          break;
        case PokemonUnity.Attack.Data.Effects.x091:
          if (opponent.effects.HyperBeam > 0)
          {
            a1 -= 90f;
            break;
          }
          if ((double) this.pbRoughStat(attacker, Stats.ATTACK, skill) > (double) this.pbRoughStat(attacker, Stats.SPATK, skill) * 1.5)
            a1 -= 60f;
          else if (skill >= 32 && opponent.lastMoveUsed > Moves.NONE)
          {
            MoveData moveData = Kernal.MoveData[opponent.lastMoveUsed];
            nullable1 = moveData.Power;
            int num22 = 0;
            if (nullable1.GetValueOrDefault() > num22 & nullable1.HasValue && moveData.Category == Category.SPECIAL)
              a1 -= 60f;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x093:
          if (skill >= 48 && !opponent.hasWorkingAbility(Abilities.INNER_FOCUS) && opponent.effects.Substitute == 0)
          {
            a1 += 30f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x095:
          if (opponent.effects.FutureSight > 0)
          {
            a1 -= 100f;
            break;
          }
          if (attacker.pbNonActivePokemonCount == 0)
          {
            a1 -= 70f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x097:
          if (skill >= 48 && !opponent.hasWorkingAbility(Abilities.INNER_FOCUS) && opponent.effects.Substitute == 0)
            a1 += 30f;
          if (opponent.effects.Minimize)
          {
            a1 += 30f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x09A:
          if (opponent.IsNotNullOrNone())
          {
            a1 -= 100f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x09F:
          if (attacker.turncount == 0)
          {
            if (skill >= 48 && !opponent.hasWorkingAbility(Abilities.INNER_FOCUS) && opponent.effects.Substitute == 0)
            {
              a1 += 30f;
              break;
            }
            break;
          }
          a1 -= 90f;
          if (skill >= 100)
            a1 = 0.0f;
          break;
        case PokemonUnity.Attack.Data.Effects.x0A1:
          int num23 = -(attacker.stages[2] * 10) - attacker.stages[5] * 10;
          a1 += (float) (num23 / 2);
          if (attacker.effects.Stockpile >= 3)
          {
            a1 -= 80f;
            break;
          }
          foreach (IBattleMove move21 in attacker.moves)
          {
            if (move21.Effect == PokemonUnity.Attack.Data.Effects.x0A2 || move21.Effect == PokemonUnity.Attack.Data.Effects.x0A3)
            {
              a1 += 20f;
              break;
            }
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0A2:
          if (attacker.effects.Stockpile == 0)
          {
            a1 -= 100f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0A3:
          if (attacker.effects.Stockpile == 0)
          {
            a1 -= 90f;
            break;
          }
          if (attacker.HP == attacker.TotalHP)
          {
            a1 -= 90f;
            break;
          }
          int num24 = new int[4]{ 0, 25, 50, 100 }[attacker.effects.Stockpile];
          a1 = a1 + (float) num24 - (float) (attacker.HP * num24 * 2 / attacker.TotalHP);
          break;
        case PokemonUnity.Attack.Data.Effects.x0A5:
          if (this.pbCheckGlobalAbility(Abilities.AIR_LOCK).IsNotNullOrNone() || this.pbCheckGlobalAbility(Abilities.CLOUD_NINE).IsNotNullOrNone())
          {
            a1 -= 90f;
            break;
          }
          if (this.pbWeather == Weather.HAIL)
          {
            a1 -= 90f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0A6:
          if (opponent.effects.Torment)
          {
            a1 -= 90f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0A7:
          if (battlerEffect1 != null && !battlerEffect1.pbCanConfuse(attacker, false))
          {
            a1 -= 90f;
            break;
          }
          if (opponent.stages[4] < 0)
            a1 += 30f;
          break;
        case PokemonUnity.Attack.Data.Effects.x0A9:
          if ((battlerEffect1 == null || !battlerEffect1.pbCanReduceStatStage(Stats.ATTACK, attacker)) && (battlerEffect1 != null ? (battlerEffect1.pbCanReduceStatStage(Stats.SPATK, attacker) ? 1 : 0) : 0) == 0)
          {
            a1 -= 100f;
            break;
          }
          if (attacker.pbNonActivePokemonCount == 0)
          {
            a1 -= 100f;
            break;
          }
          a1 = a1 + (float) (opponent.stages[1] * 10) + (float) (opponent.stages[4] * 10) - (float) (attacker.HP * 100 / attacker.TotalHP);
          break;
        case PokemonUnity.Attack.Data.Effects.x0AB:
          if (opponent.effects.HyperBeam > 0)
            a1 += 50f;
          if (opponent.HP <= opponent.TotalHP / 2)
            a1 -= 35f;
          if (opponent.HP <= opponent.TotalHP / 4)
          {
            a1 -= 70f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0AC:
          if (opponent.Status == Status.PARALYSIS)
          {
            a1 -= 20f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0AD:
          if (!this.doublebattle)
          {
            a1 -= 100f;
            break;
          }
          if (attacker.pbPartner.isFainted())
          {
            a1 -= 90f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0AF:
          bool flag6 = false;
          for (int index = 0; index < 4; ++index)
          {
            if (attacker.moves[index].Type == Types.ELECTRIC && attacker.moves[index].basedamage > 0)
            {
              flag6 = true;
              break;
            }
          }
          if (move.basedamage == 0)
          {
            if (battlerEffect2 != null && battlerEffect2.pbTooHigh(Stats.SPDEF))
              a1 -= 90f;
            else
              a1 -= (float) (attacker.stages[5] * 20);
            if (flag6)
            {
              a1 += 20f;
              break;
            }
            break;
          }
          if (attacker.stages[5] < 0)
            a1 += 20f;
          if (flag6)
            a1 += 20f;
          break;
        case PokemonUnity.Attack.Data.Effects.x0B0:
          if (opponent.effects.Taunt > 0)
          {
            a1 -= 90f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0B1:
          if (attacker.pbPartner.isFainted())
          {
            a1 -= 90f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0B2:
          if (attacker.Item == Items.NONE && opponent.Item == Items.NONE)
          {
            a1 -= 90f;
            break;
          }
          if (skill >= 48 && opponent.hasWorkingAbility(Abilities.STICKY_HOLD))
          {
            a1 -= 90f;
            break;
          }
          if (attacker.hasWorkingItem(Items.FLAME_ORB) || attacker.hasWorkingItem(Items.TOXIC_ORB) || attacker.hasWorkingItem(Items.STICKY_BARB) || attacker.hasWorkingItem(Items.IRON_BALL) || attacker.hasWorkingItem(Items.CHOICE_BAND) || attacker.hasWorkingItem(Items.CHOICE_SCARF) || attacker.hasWorkingItem(Items.CHOICE_SPECS))
          {
            a1 += 50f;
            break;
          }
          if (attacker.Item == Items.NONE && (uint) opponent.Item > 0U && Kernal.MoveData[attacker.lastMoveUsed].Effect == PokemonUnity.Attack.Data.Effects.x0B2)
          {
            a1 -= 30f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0B3:
          a1 -= 40f;
          if (skill >= 32 && (opponent.Ability == Abilities.NONE || attacker.Ability == opponent.Ability || attacker.Ability == Abilities.MULTITYPE || opponent.Ability == Abilities.FLOWER_GIFT || opponent.Ability == Abilities.FORECAST || opponent.Ability == Abilities.ILLUSION || opponent.Ability == Abilities.IMPOSTER || opponent.Ability == Abilities.MULTITYPE || opponent.Ability == Abilities.TRACE || opponent.Ability == Abilities.WONDER_GUARD || opponent.Ability == Abilities.ZEN_MODE))
            a1 -= 90f;
          if (skill >= 48)
          {
            if (opponent.Ability == Abilities.TRUANT && attacker.pbIsOpposing(opponent.Index))
              a1 -= 90f;
            else if (opponent.Ability == Abilities.SLOW_START && attacker.pbIsOpposing(opponent.Index))
              a1 -= 90f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0B4:
          if (attacker.effects.Wish > 0)
          {
            a1 -= 90f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0B6:
          if (attacker.effects.Ingrain)
          {
            a1 -= 90f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0B7:
          int num25 = attacker.stages[1] * 10 + attacker.stages[2] * 10;
          a1 += (float) (num25 / 2);
          break;
        case PokemonUnity.Attack.Data.Effects.x0B9:
          if (attacker.pokemon.itemRecycle == Items.NONE || (uint) attacker.Item > 0U)
          {
            a1 -= 80f;
            break;
          }
          if ((uint) attacker.pokemon.itemRecycle > 0U)
          {
            a1 += 30f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0BA:
          int num26 = this.pbRoughStat(attacker, Stats.SPEED, skill);
          if (this.pbRoughStat(opponent, Stats.SPEED, skill) > num26)
          {
            a1 += 30f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0BB:
          if (attacker.pbOpposingSide.Reflect > (byte) 0)
            a1 += 20f;
          if (attacker.pbOpposingSide.LightScreen > (byte) 0)
          {
            a1 += 20f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0BC:
          if (opponent.effects.Yawn > 0 || (battlerEffect1 != null ? (battlerEffect1.pbCanSleep(attacker, false) ? 1 : 0) : 0) == 0)
          {
            if (skill >= 32)
            {
              a1 -= 90f;
              break;
            }
            break;
          }
          a1 += 30f;
          if (skill >= 48 && opponent.hasWorkingAbility(Abilities.MARVEL_SCALE))
            a1 -= 30f;
          if (skill >= 100)
          {
            foreach (IBattleMove move22 in opponent.moves)
            {
              MoveData moveData = Kernal.MoveData[move22.id];
              if (moveData.Effect == PokemonUnity.Attack.Data.Effects.x062 || moveData.Effect == PokemonUnity.Attack.Data.Effects.x05D)
              {
                a1 -= 50f;
                break;
              }
            }
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0BD:
          if (skill >= 48 && (uint) opponent.Item > 0U)
          {
            a1 += 20f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0BE:
          if (attacker.HP >= opponent.HP)
          {
            a1 -= 90f;
            break;
          }
          if (attacker.HP * 2 < opponent.HP)
          {
            a1 += 50f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0C0:
          a1 -= 40f;
          if (skill >= 32 && (attacker.Ability == Abilities.NONE && opponent.Ability == Abilities.NONE || attacker.Ability == opponent.Ability || attacker.Ability == Abilities.ILLUSION || opponent.Ability == Abilities.ILLUSION || attacker.Ability == Abilities.MULTITYPE || opponent.Ability == Abilities.MULTITYPE || attacker.Ability == Abilities.WONDER_GUARD || opponent.Ability == Abilities.WONDER_GUARD))
            a1 -= 90f;
          if (skill >= 48)
          {
            if (opponent.Ability == Abilities.TRUANT && attacker.pbIsOpposing(opponent.Index))
              a1 -= 90f;
            else if (opponent.Ability == Abilities.SLOW_START && attacker.pbIsOpposing(opponent.Index))
              a1 -= 90f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0C1:
          if (attacker.effects.Imprison)
          {
            a1 -= 90f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0C2:
          if (attacker.Status == Status.BURN)
          {
            a1 += 40f;
            break;
          }
          if (attacker.Status == Status.POISON)
          {
            a1 += 40f;
            if (skill >= 32)
            {
              if (attacker.HP < attacker.TotalHP / 8)
                a1 += 60f;
              else if (skill >= 48 && attacker.HP < (attacker.effects.Toxic + 1) * attacker.TotalHP / 16)
                a1 += 60f;
              break;
            }
            break;
          }
          if (attacker.Status == Status.PARALYSIS)
          {
            a1 += 40f;
            break;
          }
          a1 -= 90f;
          break;
        case PokemonUnity.Attack.Data.Effects.x0C3:
          a1 = a1 + 50f - (float) (attacker.HP * 100 / attacker.TotalHP);
          if (attacker.HP <= attacker.TotalHP / 10)
          {
            a1 += 30f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0C7:
          a1 -= 30f;
          break;
        case PokemonUnity.Attack.Data.Effects.x0CA:
          if (attacker.effects.MudSport)
          {
            a1 -= 90f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0CD:
          a1 += (float) (attacker.stages[4] * 10);
          break;
        case PokemonUnity.Attack.Data.Effects.x0CE:
          int num27 = opponent.stages[1] * 10 + opponent.stages[2] * 10;
          a1 += (float) (num27 / 2);
          break;
        case PokemonUnity.Attack.Data.Effects.x0CF:
          if (battlerEffect2 != null && battlerEffect2.pbTooHigh(Stats.DEFENSE) && battlerEffect2 != null && battlerEffect2.pbTooHigh(Stats.SPDEF))
          {
            a1 -= 90f;
            break;
          }
          a1 = a1 - (float) (attacker.stages[2] * 10) - (float) (attacker.stages[5] * 10);
          break;
        case PokemonUnity.Attack.Data.Effects.x0D1:
          if (battlerEffect2 != null && battlerEffect2.pbTooHigh(Stats.ATTACK) && battlerEffect2 != null && battlerEffect2.pbTooHigh(Stats.DEFENSE))
          {
            a1 -= 90f;
            break;
          }
          a1 = a1 - (float) (attacker.stages[1] * 10) - (float) (attacker.stages[2] * 10);
          if (skill >= 32)
          {
            bool flag7 = false;
            foreach (IBattleMove move23 in attacker.moves)
            {
              if (move23.id != Moves.NONE && move23.basedamage > 0 && move23.pbIsPhysical(move23.Type))
                flag7 = true;
            }
            if (flag7)
              a1 += 20f;
            else if (skill >= 48)
              a1 -= 90f;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0D3:
          if (attacker.effects.WaterSport)
          {
            a1 -= 90f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0D4:
          if (battlerEffect2 != null && battlerEffect2.pbTooHigh(Stats.SPATK) && battlerEffect2 != null && battlerEffect2.pbTooHigh(Stats.SPDEF))
          {
            a1 -= 90f;
            break;
          }
          if (attacker.turncount == 0)
            a1 += 40f;
          a1 = a1 - (float) (attacker.stages[4] * 10) - (float) (attacker.stages[5] * 10);
          if (skill >= 32)
          {
            bool flag8 = false;
            foreach (IBattleMove move24 in attacker.moves)
            {
              if (move24.id != Moves.NONE && move24.basedamage > 0 && move24.pbIsSpecial(move24.Type))
                flag8 = true;
            }
            if (flag8)
              a1 += 20f;
            else if (skill >= 48)
              a1 -= 90f;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0D5:
          if (attacker.turncount == 0)
            a1 += 40f;
          if (battlerEffect2 != null && battlerEffect2.pbTooHigh(Stats.ATTACK) && battlerEffect2 != null && battlerEffect2.pbTooHigh(Stats.SPEED))
          {
            a1 -= 90f;
            break;
          }
          a1 = a1 - (float) (attacker.stages[1] * 10) - (float) (attacker.stages[3] * 10);
          if (skill >= 32)
          {
            bool flag9 = false;
            foreach (IBattleMove move25 in attacker.moves)
            {
              if (move25.id != Moves.NONE && move25.basedamage > 0 && move25.pbIsPhysical(move25.Type))
                flag9 = true;
            }
            if (flag9)
              a1 += 20f;
            else if (skill >= 48)
              a1 -= 90f;
          }
          if (skill >= 48)
          {
            int num28 = this.pbRoughStat(attacker, Stats.SPEED, skill);
            int num29 = this.pbRoughStat(opponent, Stats.SPEED, skill);
            if (num28 < num29 && num28 * 2 > num29)
              a1 += 20f;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0D6:
          if (attacker.Ability == Abilities.MULTITYPE)
          {
            a1 -= 90f;
            break;
          }
          if (skill >= 32)
          {
            Types type = new Types[9]
            {
              Types.NORMAL,
              Types.GRASS,
              Types.GRASS,
              Types.WATER,
              Types.WATER,
              Types.WATER,
              Types.ROCK,
              Types.ROCK,
              Types.GROUND
            }[(int) this.environment];
            if (attacker.pbHasType(type))
              a1 -= 90f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0D8:
          if (this.field.Gravity > (byte) 0)
          {
            a1 -= 90f;
            break;
          }
          if (skill >= 32)
          {
            a1 -= 30f;
            if (attacker.effects.SkyDrop)
              a1 -= 20f;
            if (attacker.effects.MagnetRise > 0)
              a1 -= 20f;
            if (attacker.effects.Telekinesis > 0)
              a1 -= 20f;
            if (attacker.pbHasType(Types.FLYING))
              a1 -= 20f;
            if (attacker.hasWorkingAbility(Abilities.LEVITATE))
              a1 -= 20f;
            if (attacker.hasWorkingItem(Items.AIR_BALLOON))
              a1 -= 20f;
            if (opponent.effects.SkyDrop)
              a1 += 20f;
            if (opponent.effects.MagnetRise > 0)
              a1 += 20f;
            if (opponent.effects.Telekinesis > 0)
              a1 += 20f;
            if (Kernal.MoveData[opponent.effects.TwoTurnAttack].Effect == PokemonUnity.Attack.Data.Effects.x09C || Kernal.MoveData[opponent.effects.TwoTurnAttack].Effect == PokemonUnity.Attack.Data.Effects.x108 || Kernal.MoveData[opponent.effects.TwoTurnAttack].Effect == PokemonUnity.Attack.Data.Effects.x138)
              a1 += 20f;
            if (opponent.pbHasType(Types.FLYING))
              a1 += 20f;
            if (opponent.hasWorkingAbility(Abilities.LEVITATE))
              a1 += 20f;
            if (opponent.hasWorkingItem(Items.AIR_BALLOON))
              a1 += 20f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0D9:
          if (opponent.effects.MiracleEye)
          {
            a1 -= 90f;
            break;
          }
          if (opponent.pbHasType(Types.DARK))
          {
            a1 += 70f;
            break;
          }
          if (opponent.stages[7] <= 0)
          {
            a1 -= 60f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0DA:
          if (opponent.Status == Status.SLEEP && opponent.StatusCount > 1)
          {
            a1 -= 20f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0DB:
          a1 += (float) (attacker.stages[3] * 10);
          break;
        case PokemonUnity.Attack.Data.Effects.x0DD:
        case PokemonUnity.Attack.Data.Effects.x10F:
          a1 -= 70f;
          break;
        case PokemonUnity.Attack.Data.Effects.x0E1:
        case PokemonUnity.Attack.Data.Effects.x13B:
          if (opponent.effects.Substitute == 0 && skill >= 48 && Game.GameData is IItemCheck gameData1 && gameData1.pbIsBerry(opponent.Item))
          {
            a1 += 30f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0E2:
          if (attacker.pbOwnSide.Tailwind > (byte) 0)
          {
            a1 -= 90f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0E3:
          if (battlerEffect1 != null && battlerEffect1.pbTooHigh(Stats.ATTACK) && battlerEffect1 != null && battlerEffect1.pbTooHigh(Stats.DEFENSE) && battlerEffect1 != null && battlerEffect1.pbTooHigh(Stats.SPEED) && battlerEffect1 != null && battlerEffect1.pbTooHigh(Stats.SPATK) && battlerEffect1 != null && battlerEffect1.pbTooHigh(Stats.SPDEF) && battlerEffect1 != null && battlerEffect1.pbTooHigh(Stats.ACCURACY) && battlerEffect1 != null && battlerEffect1.pbTooHigh(Stats.EVASION))
          {
            a1 -= 90f;
            break;
          }
          int num30 = -opponent.stages[1] - opponent.stages[2] - opponent.stages[3] - opponent.stages[4] - opponent.stages[5] - opponent.stages[6] - opponent.stages[7];
          if (num30 < 0)
            num30 = (int) Math.Floor((double) num30 / 2.0);
          a1 += (float) (num30 * 10);
          break;
        case PokemonUnity.Attack.Data.Effects.x0E4:
          if (opponent.effects.HyperBeam > 0)
          {
            a1 -= 90f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0E6:
          int num31 = attacker.stages[2] * 10 + attacker.stages[5] * 10;
          a1 += (float) (num31 / 2);
          break;
        case PokemonUnity.Attack.Data.Effects.x0E7:
          int num32 = this.pbRoughStat(attacker, Stats.SPEED, skill);
          if (this.pbRoughStat(opponent, Stats.SPEED, skill) > num32)
          {
            a1 += 30f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0E8:
          if (this.doublebattle)
          {
            a1 += 20f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0E9:
          if (opponent.effects.Embargo > 0)
          {
            a1 -= 90f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0EA:
          if (attacker.Item == Items.NONE || this.pbIsUnlosableItem(attacker, attacker.Item) || Game.GameData is IItemCheck gameData2 && gameData2.pbIsPokeBall(attacker.Item) || attacker.hasWorkingAbility(Abilities.KLUTZ) || attacker.effects.Embargo > 0)
          {
            a1 -= 90f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0EB:
          if (attacker.Status == Status.NONE)
          {
            a1 -= 90f;
            break;
          }
          a1 += 40f;
          break;
        case PokemonUnity.Attack.Data.Effects.x0ED:
          if (opponent.effects.HealBlock > 0)
          {
            a1 -= 90f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0EF:
          if (skill >= 32)
          {
            int num33 = this.pbRoughStat(attacker, Stats.ATTACK, skill);
            int num34 = this.pbRoughStat(attacker, Stats.DEFENSE, skill);
            if (num33 == num34 || attacker.effects.PowerTrick)
            {
              a1 -= 90f;
              break;
            }
            if (num34 > num33)
            {
              a1 += 30f;
              break;
            }
            a1 -= 30f;
            break;
          }
          a1 -= 30f;
          break;
        case PokemonUnity.Attack.Data.Effects.x0F0:
          if (opponent.effects.Substitute > 0 || opponent.effects.GastroAcid)
          {
            a1 -= 90f;
            break;
          }
          if (skill >= 48)
          {
            if (opponent.Ability == Abilities.MULTITYPE)
              a1 -= 90f;
            if (opponent.Ability == Abilities.SLOW_START)
              a1 -= 90f;
            if (opponent.Ability == Abilities.TRUANT)
              a1 -= 90f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0F1:
          if (attacker.pbOwnSide.LuckyChant > (byte) 0)
          {
            a1 -= 90f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0F4:
          if (skill >= 32)
          {
            int stage1 = attacker.stages[1];
            int stage2 = attacker.stages[4];
            int stage3 = opponent.stages[1];
            int stage4 = opponent.stages[4];
            if (stage1 >= stage3 && stage2 >= stage4)
            {
              a1 -= 80f;
              break;
            }
            a1 = a1 + (float) ((stage3 - stage1) * 10) + (float) ((stage4 - stage2) * 10);
            break;
          }
          a1 -= 50f;
          break;
        case PokemonUnity.Attack.Data.Effects.x0F5:
          if (skill >= 32)
          {
            int stage5 = attacker.stages[2];
            int stage6 = attacker.stages[5];
            int stage7 = opponent.stages[2];
            int stage8 = opponent.stages[5];
            if (stage5 >= stage7 && stage6 >= stage8)
            {
              a1 -= 80f;
              break;
            }
            a1 = a1 + (float) ((stage7 - stage5) * 10) + (float) ((stage8 - stage6) * 10);
            break;
          }
          a1 -= 50f;
          break;
        case PokemonUnity.Attack.Data.Effects.x0F8:
          if (opponent.effects.Substitute > 0)
          {
            a1 -= 90f;
            break;
          }
          if (skill >= 32 && (opponent.Ability == Abilities.MULTITYPE || opponent.Ability == Abilities.INSOMNIA || opponent.Ability == Abilities.TRUANT))
          {
            a1 -= 90f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0FA:
          if (attacker.pbOpposingSide.ToxicSpikes >= (byte) 2)
          {
            a1 -= 90f;
            break;
          }
          if (!this.pbCanChooseNonActive(attacker.pbOpposing1.Index) && !this.pbCanChooseNonActive(attacker.pbOpposing2.Index))
          {
            a1 -= 90f;
            break;
          }
          a1 = (float) ((double) (a1 + (float) (4 * attacker.pbOppositeOpposing.pbNonActivePokemonCount)) + (double) new int[2]
          {
            26,
            13
          }[(int) attacker.pbOpposingSide.ToxicSpikes]);
          break;
        case PokemonUnity.Attack.Data.Effects.x0FB:
          if (skill >= 32)
          {
            int num35 = attacker.stages[1] + attacker.stages[2] + attacker.stages[3] + attacker.stages[4] + attacker.stages[5] + attacker.stages[7] + attacker.stages[6];
            int num36 = opponent.stages[1] + opponent.stages[2] + opponent.stages[3] + opponent.stages[4] + opponent.stages[5] + opponent.stages[7] + opponent.stages[6];
            a1 += (float) ((num36 - num35) * 10);
            break;
          }
          a1 -= 50f;
          break;
        case PokemonUnity.Attack.Data.Effects.x0FC:
          if (attacker.effects.AquaRing)
          {
            a1 -= 90f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0FD:
          if (attacker.effects.MagnetRise > 0 || attacker.effects.Ingrain || attacker.effects.SmackDown)
          {
            a1 -= 90f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0FE:
          a1 -= 30f;
          if (battlerEffect1 != null && battlerEffect1.pbCanBurn(attacker, false))
          {
            a1 += 30f;
            if (skill >= 48)
            {
              if (opponent.hasWorkingAbility(Abilities.GUTS))
                a1 -= 40f;
              if (opponent.hasWorkingAbility(Abilities.MARVEL_SCALE))
                a1 -= 40f;
              if (opponent.hasWorkingAbility(Abilities.QUICK_FEET))
                a1 -= 40f;
              if (opponent.hasWorkingAbility(Abilities.FLARE_BOOST))
                a1 -= 40f;
            }
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x103:
          if (move.basedamage == 0)
          {
            if (battlerEffect1 != null && !battlerEffect1.pbCanReduceStatStage(Stats.EVASION, attacker))
              a1 -= 90f;
            else
              a1 += (float) (opponent.stages[7] * 10);
          }
          else if (opponent.stages[7] > 0)
            a1 += 20f;
          if (opponent.pbOwnSide.Reflect > (byte) 0 || opponent.pbOwnSide.LightScreen > (byte) 0 || opponent.pbOwnSide.Mist > (byte) 0 || opponent.pbOwnSide.Safeguard > (byte) 0)
            a1 += 30f;
          if (opponent.pbOwnSide.Spikes > (byte) 0 || opponent.pbOwnSide.ToxicSpikes > (byte) 0 || opponent.pbOwnSide.StealthRock)
          {
            a1 -= 30f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x106:
          if (opponent.effects.MultiTurn == 0)
          {
            a1 += 40f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x107:
          a1 -= 30f;
          if (battlerEffect1 != null && battlerEffect1.pbCanParalyze(attacker, false))
          {
            a1 += 30f;
            if (skill >= 32)
            {
              int num37 = this.pbRoughStat(attacker, Stats.SPEED, skill);
              int num38 = this.pbRoughStat(opponent, Stats.SPEED, skill);
              if (num37 < num38)
                a1 += 30f;
              else if (num37 > num38)
                a1 -= 40f;
            }
            if (skill >= 48)
            {
              if (opponent.hasWorkingAbility(Abilities.GUTS))
                a1 -= 40f;
              if (opponent.hasWorkingAbility(Abilities.MARVEL_SCALE))
                a1 -= 40f;
              if (opponent.hasWorkingAbility(Abilities.QUICK_FEET))
                a1 -= 40f;
            }
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x10A:
          int num39;
          if (attacker.Gender.HasValue && opponent.Gender.HasValue)
          {
            bool? gender3 = attacker.Gender;
            bool? gender4 = opponent.Gender;
            if (!(gender3.GetValueOrDefault() == gender4.GetValueOrDefault() & gender3.HasValue == gender4.HasValue))
            {
              num39 = opponent.hasWorkingAbility(Abilities.OBLIVIOUS) ? 1 : 0;
              goto label_780;
            }
          }
          num39 = 1;
label_780:
          if (num39 != 0)
          {
            a1 -= 90f;
            break;
          }
          if (move.basedamage == 0)
          {
            if (battlerEffect1 != null && !battlerEffect1.pbCanReduceStatStage(Stats.SPATK, attacker))
            {
              a1 -= 90f;
              break;
            }
            if (attacker.turncount == 0)
              a1 += 40f;
            a1 += (float) (opponent.stages[4] * 20);
            if (skill >= 32)
            {
              bool flag10 = false;
              foreach (IBattleMove move26 in opponent.moves)
              {
                if (move26.id != Moves.NONE && move26.basedamage > 0 && move26.pbIsSpecial(move26.Type))
                  flag10 = true;
              }
              if (flag10)
                a1 += 20f;
              else if (skill >= 48)
                a1 -= 90f;
            }
            break;
          }
          if (attacker.turncount == 0)
            a1 += 10f;
          if (opponent.stages[4] > 0)
            a1 += 20f;
          if (skill >= 32)
          {
            bool flag11 = false;
            foreach (IBattleMove move27 in opponent.moves)
            {
              if (move27.id != Moves.NONE && move27.basedamage > 0 && move27.pbIsSpecial(move27.Type))
                flag11 = true;
            }
            if (flag11)
              a1 += 30f;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x10B:
          if (attacker.pbOpposingSide.StealthRock)
          {
            a1 -= 90f;
            break;
          }
          if (!this.pbCanChooseNonActive(attacker.pbOpposing1.Index) && !this.pbCanChooseNonActive(attacker.pbOpposing2.Index))
          {
            a1 -= 90f;
            break;
          }
          a1 += (float) (5 * attacker.pbOppositeOpposing.pbNonActivePokemonCount);
          break;
        case PokemonUnity.Attack.Data.Effects.x10E:
          a1 -= 40f;
          break;
        case PokemonUnity.Attack.Data.Effects.x115:
          if (move.basedamage == 0)
          {
            if (battlerEffect2 != null && battlerEffect2.pbTooHigh(Stats.SPATK))
            {
              a1 -= 90f;
              break;
            }
            a1 -= (float) (attacker.stages[4] * 20);
            if (skill >= 32)
            {
              bool flag12 = false;
              foreach (IBattleMove move28 in attacker.moves)
              {
                if (move28.id != Moves.NONE && move28.basedamage > 0 && move28.pbIsSpecial(move28.Type))
                  flag12 = true;
              }
              if (flag12)
                a1 += 20f;
              else if (skill >= 48)
                a1 -= 90f;
            }
            break;
          }
          if (attacker.stages[4] < 0)
            a1 += 20f;
          if (skill >= 32)
          {
            bool flag13 = false;
            foreach (IBattleMove move29 in attacker.moves)
            {
              if (move29.id != Moves.NONE && move29.basedamage > 0 && move29.pbIsSpecial(move29.Type))
                flag13 = true;
            }
            if (flag13)
              a1 += 20f;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x116:
          if (battlerEffect2 != null && battlerEffect2.pbTooHigh(Stats.ATTACK) && battlerEffect2 != null && battlerEffect2.pbTooHigh(Stats.ACCURACY))
          {
            a1 -= 90f;
            break;
          }
          a1 = a1 - (float) (attacker.stages[1] * 10) - (float) (attacker.stages[6] * 10);
          if (skill >= 32)
          {
            bool flag14 = false;
            foreach (IBattleMove move30 in attacker.moves)
            {
              if (move30.id != Moves.NONE && move30.basedamage > 0 && move30.pbIsPhysical(move30.Type))
                flag14 = true;
            }
            if (flag14)
              a1 += 20f;
            else if (skill >= 48)
              a1 -= 90f;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x118:
          if (skill >= 32)
          {
            int num40 = this.pbRoughStat(attacker, Stats.DEFENSE, skill);
            int num41 = this.pbRoughStat(attacker, Stats.SPDEF, skill);
            int num42 = this.pbRoughStat(opponent, Stats.DEFENSE, skill);
            int num43 = this.pbRoughStat(opponent, Stats.SPDEF, skill);
            if (num40 < num42 && num41 < num43)
            {
              a1 += 50f;
              break;
            }
            if (num40 + num41 < num42 + num43)
            {
              a1 += 30f;
              break;
            }
            a1 -= 50f;
            break;
          }
          a1 -= 30f;
          break;
        case PokemonUnity.Attack.Data.Effects.x119:
          if (skill >= 32)
          {
            int num44 = this.pbRoughStat(attacker, Stats.ATTACK, skill);
            int num45 = this.pbRoughStat(attacker, Stats.SPATK, skill);
            int num46 = this.pbRoughStat(opponent, Stats.ATTACK, skill);
            int num47 = this.pbRoughStat(opponent, Stats.SPATK, skill);
            if (num44 < num46 && num45 < num47)
            {
              a1 += 50f;
              break;
            }
            if (num44 + num45 < num46 + num47)
            {
              a1 += 30f;
              break;
            }
            a1 -= 50f;
            break;
          }
          a1 -= 30f;
          break;
        case PokemonUnity.Attack.Data.Effects.x11E:
          if (opponent.effects.Telekinesis > 0 || opponent.effects.Ingrain || opponent.effects.SmackDown)
          {
            a1 -= 90f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x11F:
          if (this.field.MagicRoom > (byte) 0)
          {
            a1 -= 90f;
            break;
          }
          if (attacker.Item == Items.NONE && (uint) opponent.Item > 0U)
            a1 += 30f;
          break;
        case PokemonUnity.Attack.Data.Effects.x120:
          if (skill >= 32)
          {
            if (opponent.effects.MagnetRise > 0)
              a1 += 20f;
            if (opponent.effects.Telekinesis > 0)
              a1 += 20f;
            if (Kernal.MoveData[opponent.effects.TwoTurnAttack].Effect == PokemonUnity.Attack.Data.Effects.x09C || Kernal.MoveData[opponent.effects.TwoTurnAttack].Effect == PokemonUnity.Attack.Data.Effects.x108)
              a1 += 20f;
            if (opponent.pbHasType(Types.FLYING))
              a1 += 20f;
            if (opponent.hasWorkingAbility(Abilities.LEVITATE))
              a1 += 20f;
            if (opponent.hasWorkingItem(Items.AIR_BALLOON))
              a1 += 20f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x122:
          if (!opponent.pbPartner.isFainted())
          {
            a1 += 10f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x123:
          if (battlerEffect2 != null && battlerEffect2.pbTooHigh(Stats.SPEED) && battlerEffect2 != null && battlerEffect2.pbTooHigh(Stats.SPATK) && battlerEffect2 != null && battlerEffect2.pbTooHigh(Stats.SPDEF))
          {
            a1 -= 90f;
            break;
          }
          a1 = a1 - (float) (attacker.stages[4] * 10) - (float) (attacker.stages[5] * 10) - (float) (attacker.stages[3] * 10);
          if (skill >= 32)
          {
            bool flag15 = false;
            foreach (IBattleMove move31 in attacker.moves)
            {
              if (move31.id != Moves.NONE && move31.basedamage > 0 && move31.pbIsSpecial(move31.Type))
                flag15 = true;
            }
            if (flag15)
              a1 += 20f;
            else if (skill >= 48)
              a1 -= 90f;
          }
          if (skill >= 48)
          {
            int num48 = this.pbRoughStat(attacker, Stats.SPEED, skill);
            int num49 = this.pbRoughStat(opponent, Stats.SPEED, skill);
            if (num48 < num49 && num48 * 2 > num49)
              a1 += 20f;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x125:
          if (!opponent.pbHasType(attacker.Type1) && !opponent.pbHasType(attacker.Type2))
          {
            a1 -= 90f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x127:
          if (opponent.effects.Substitute > 0 || opponent.Ability == Abilities.MULTITYPE)
          {
            a1 -= 90f;
            break;
          }
          if (opponent.pbHasType(Types.WATER))
          {
            a1 -= 90f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x128:
          if (move.basedamage == 0)
          {
            if (battlerEffect2 != null && battlerEffect2.pbTooHigh(Stats.SPEED))
            {
              a1 -= 90f;
              break;
            }
            a1 -= (float) (attacker.stages[3] * 10);
            if (skill >= 48)
            {
              int num50 = this.pbRoughStat(attacker, Stats.SPEED, skill);
              int num51 = this.pbRoughStat(opponent, Stats.SPEED, skill);
              if (num50 < num51 && num50 * 2 > num51)
                a1 += 30f;
            }
            break;
          }
          if (attacker.stages[3] < 0)
            a1 += 20f;
          break;
        case PokemonUnity.Attack.Data.Effects.x12B:
          if (opponent.effects.Substitute > 0)
          {
            a1 -= 90f;
            break;
          }
          if (skill >= 32 && (opponent.Ability == Abilities.MULTITYPE || opponent.Ability == Abilities.SIMPLE || opponent.Ability == Abilities.TRUANT))
          {
            a1 -= 90f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x12C:
          a1 -= 40f;
          if (opponent.effects.Substitute > 0)
          {
            a1 -= 90f;
            break;
          }
          if (skill >= 32)
          {
            if (attacker.Ability == Abilities.NONE || attacker.Ability == opponent.Ability || opponent.Ability == Abilities.MULTITYPE || opponent.Ability == Abilities.TRUANT || attacker.Ability == Abilities.FLOWER_GIFT || attacker.Ability == Abilities.FORECAST || attacker.Ability == Abilities.ILLUSION || attacker.Ability == Abilities.IMPOSTER || attacker.Ability == Abilities.MULTITYPE || attacker.Ability == Abilities.TRACE || attacker.Ability == Abilities.ZEN_MODE)
              a1 -= 90f;
            if (skill >= 48)
            {
              if (attacker.Ability == Abilities.TRUANT && attacker.pbIsOpposing(opponent.Index))
                a1 += 90f;
              else if (attacker.Ability == Abilities.SLOW_START && attacker.pbIsOpposing(opponent.Index))
                a1 += 90f;
            }
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x12E:
          if (skill >= 32 && this.doublebattle && !attacker.pbPartner.isFainted() && ((IEnumerable<IBattleMove>) attacker.pbPartner.moves).Any<IBattleMove>((Func<IBattleMove, bool>) (n => n.id == move.id)))
          {
            a1 += 20f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x131:
          if (opponent.effects.Substitute > 0)
          {
            a1 -= 90f;
            break;
          }
          bool flag16 = false;
          if ((uint) num1 > 0U)
            num1 = opponent.stages[1];
          flag16 = true;
          if ((uint) num1 > 0U)
            num1 += opponent.stages[2];
          flag16 = true;
          if ((uint) num1 > 0U)
            num1 += opponent.stages[3];
          flag16 = true;
          if ((uint) num1 > 0U)
            num1 += opponent.stages[4];
          flag16 = true;
          if ((uint) num1 > 0U)
            num1 += opponent.stages[5];
          flag16 = true;
          if ((uint) num1 > 0U)
            num1 += opponent.stages[6];
          flag16 = true;
          if ((uint) num1 > 0U)
            num1 += opponent.stages[7];
          if (true)
            a1 += (float) (num1 * 10);
          else
            a1 -= 90f;
          break;
        case PokemonUnity.Attack.Data.Effects.x135:
          a1 = a1 - (float) (attacker.stages[1] * 20) - (float) (attacker.stages[3] * 20) - (float) (attacker.stages[4] * 20) + (float) (attacker.stages[2] * 10) + (float) (attacker.stages[5] * 10);
          if (skill >= 32)
          {
            bool flag17 = false;
            foreach (IBattleMove move32 in attacker.moves)
            {
              if (move32.id != Moves.NONE && move32.basedamage > 0)
                flag17 = true;
            }
            if (flag17)
              a1 += 20f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x136:
          if (attacker.pbIsOpposing(opponent.Index))
          {
            a1 -= 100f;
            break;
          }
          if (opponent.HP < opponent.TotalHP / 2 && opponent.effects.Substitute == 0)
            a1 += 20f;
          break;
        case PokemonUnity.Attack.Data.Effects.x139:
          if (battlerEffect2 != null && battlerEffect2.pbTooHigh(Stats.ATTACK) && battlerEffect2 != null && battlerEffect2.pbTooHigh(Stats.SPEED))
          {
            a1 -= 90f;
            break;
          }
          a1 = a1 - (float) (attacker.stages[1] * 10) - (float) (attacker.stages[3] * 10);
          if (skill >= 32)
          {
            bool flag18 = false;
            foreach (IBattleMove move33 in attacker.moves)
            {
              if (move33.id != Moves.NONE && move33.basedamage > 0 && move33.pbIsPhysical(move33.Type))
                flag18 = true;
            }
            if (flag18)
              a1 += 20f;
            else if (skill >= 48)
              a1 -= 90f;
          }
          if (skill >= 48)
          {
            int num52 = this.pbRoughStat(attacker, Stats.SPEED, skill);
            int num53 = this.pbRoughStat(opponent, Stats.SPEED, skill);
            if (num52 < num53 && num52 * 2 > num53)
              a1 += 30f;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x13A:
          if (!opponent.effects.Ingrain && (skill < 48 || !opponent.hasWorkingAbility(Abilities.SUCTION_CUPS)))
          {
            if (opponent.pbOwnSide.Spikes > (byte) 0)
              a1 += 40f;
            if (opponent.pbOwnSide.ToxicSpikes > (byte) 0)
              a1 += 40f;
            if (opponent.pbOwnSide.StealthRock)
              a1 += 40f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x13D:
        case PokemonUnity.Attack.Data.Effects.x148:
          if (battlerEffect2 != null && battlerEffect2.pbTooHigh(Stats.ATTACK) && battlerEffect2 != null && battlerEffect2.pbTooHigh(Stats.SPATK))
          {
            a1 -= 90f;
            break;
          }
          a1 = a1 - (float) (attacker.stages[1] * 10) - (float) (attacker.stages[4] * 10);
          if (skill >= 32)
          {
            bool flag19 = false;
            foreach (IBattleMove move34 in attacker.moves)
            {
              if (move34.id != Moves.NONE && move34.basedamage > 0)
              {
                flag19 = true;
                break;
              }
            }
            if (flag19)
              a1 += 20f;
            else if (skill >= 48)
              a1 -= 90f;
          }
          if (move.Effect == PokemonUnity.Attack.Data.Effects.x13D && this.pbWeather == Weather.SUNNYDAY)
            a1 += 20f;
          break;
        case PokemonUnity.Attack.Data.Effects.x13F:
          if (attacker.Ability == Abilities.MULTITYPE)
          {
            a1 -= 90f;
            break;
          }
          if (attacker.pbHasType(opponent.Type1) && attacker.pbHasType(opponent.Type2) && opponent.pbHasType(attacker.Type1) && opponent.pbHasType(attacker.Type2))
          {
            a1 -= 90f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x142:
          if (move.basedamage == 0)
          {
            if (battlerEffect2 != null && battlerEffect2.pbTooHigh(Stats.SPATK))
            {
              a1 -= 90f;
              break;
            }
            if (attacker.turncount == 0)
              a1 += 40f;
            a1 -= (float) (attacker.stages[4] * 30);
            if (skill >= 32)
            {
              bool flag20 = false;
              foreach (IBattleMove move35 in attacker.moves)
              {
                if (move35.id != Moves.NONE && move35.basedamage > 0 && move35.pbIsSpecial(move35.Type))
                  flag20 = true;
              }
              if (flag20)
                a1 += 20f;
              else if (skill >= 48)
                a1 -= 90f;
            }
            break;
          }
          if (attacker.turncount == 0)
            a1 += 10f;
          if (attacker.stages[4] < 0)
            a1 += 30f;
          if (skill >= 32)
          {
            bool flag21 = false;
            foreach (IBattleMove move36 in attacker.moves)
            {
              if (move36.id != Moves.NONE && move36.basedamage > 0 && move36.pbIsSpecial(move36.Type))
                flag21 = true;
            }
            if (flag21)
              a1 += 30f;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x143:
          if (battlerEffect2 != null && battlerEffect2.pbTooHigh(Stats.ATTACK) && battlerEffect2 != null && battlerEffect2.pbTooHigh(Stats.DEFENSE) && battlerEffect2 != null && battlerEffect2.pbTooHigh(Stats.ACCURACY))
          {
            a1 -= 90f;
            break;
          }
          a1 = a1 - (float) (attacker.stages[1] * 10) - (float) (attacker.stages[2] * 10) - (float) (attacker.stages[6] * 10);
          if (skill >= 32)
          {
            bool flag22 = false;
            foreach (IBattleMove move37 in attacker.moves)
            {
              if (move37.id != Moves.NONE && move37.basedamage > 0 && move37.pbIsPhysical(move37.Type))
                flag22 = true;
            }
            if (flag22)
              a1 += 20f;
            else if (skill >= 48)
              a1 -= 90f;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x144:
          if (attacker.Item == Items.NONE || (uint) opponent.Item > 0U)
          {
            a1 -= 90f;
            break;
          }
          if (attacker.hasWorkingItem(Items.FLAME_ORB) || attacker.hasWorkingItem(Items.TOXIC_ORB) || attacker.hasWorkingItem(Items.STICKY_BARB) || attacker.hasWorkingItem(Items.IRON_BALL) || attacker.hasWorkingItem(Items.CHOICE_BAND) || attacker.hasWorkingItem(Items.CHOICE_SCARF) || attacker.hasWorkingItem(Items.CHOICE_SPECS))
            a1 += 50f;
          else
            a1 -= 80f;
          break;
        case PokemonUnity.Attack.Data.Effects.x149:
          if (move.basedamage == 0)
          {
            if (battlerEffect2 != null && battlerEffect2.pbTooHigh(Stats.DEFENSE))
            {
              a1 -= 90f;
              break;
            }
            if (attacker.turncount == 0)
              a1 += 40f;
            a1 -= (float) (attacker.stages[2] * 30);
            break;
          }
          if (attacker.turncount == 0)
            a1 += 10f;
          if (attacker.stages[2] < 0)
            a1 += 30f;
          break;
        case PokemonUnity.Attack.Data.Effects.x14F:
          int num54 = attacker.stages[2] * 10 + attacker.stages[3] * 10 + attacker.stages[5] * 10;
          a1 += (float) (int) Math.Floor((double) num54 / 3.0);
          break;
        case PokemonUnity.Attack.Data.Effects.x153:
          if (!attacker.pokemon.IsNotNullOrNone() || !attacker.pokemon.belch)
          {
            a1 -= 90f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x154:
          int num55 = 0;
          for (int index = 0; index < 4; ++index)
          {
            IBattler battler = this.battlers[index];
            if (battler.pbHasType(Types.GRASS) && !battler.isAirborne() && battler is IBattlerEffect battlerEffect3 && (!battlerEffect3.pbTooHigh(Stats.ATTACK) || !battlerEffect3.pbTooHigh(Stats.SPATK)))
            {
              ++num55;
              if (attacker.pbIsOpposing(battler.Index))
                a1 -= 20f;
              else
                a1 = a1 - (float) (attacker.stages[1] * 10) - (float) (attacker.stages[4] * 10);
            }
          }
          if (num55 == 0)
          {
            a1 -= 95f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x155:
          if (opponent.pbOwnSide.StickyWeb)
          {
            a1 -= 95f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x156:
          if ((battlerEffect2 == null || !battlerEffect2.pbTooHigh(Stats.ATTACK)) && opponent.HP <= opponent.TotalHP / 4)
          {
            a1 += 20f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x157:
          if (opponent.pbHasType(Types.GHOST))
          {
            a1 -= 90f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x158:
          int num56 = opponent.stages[1] * 10 + opponent.stages[4] * 10;
          a1 += (float) (num56 / 2);
          break;
        case PokemonUnity.Attack.Data.Effects.x15A:
          if (skill >= 48 && opponent.hasWorkingAbility(Abilities.LIQUID_OOZE))
          {
            a1 -= 70f;
            break;
          }
          if (attacker.HP <= attacker.TotalHP / 2)
            a1 += 20f;
          break;
        case PokemonUnity.Attack.Data.Effects.x15B:
          int num57 = opponent.stages[1] * 10 + opponent.stages[4] * 10;
          a1 += (float) (num57 / 2);
          break;
        case PokemonUnity.Attack.Data.Effects.x15C:
          if (opponent.effects.Substitute > 0)
          {
            a1 -= 90f;
            break;
          }
          int num58 = 0;
          int num59 = 0;
          Stats[] statsArray1 = new Stats[7]
          {
            Stats.ATTACK,
            Stats.DEFENSE,
            Stats.SPEED,
            Stats.SPATK,
            Stats.SPDEF,
            Stats.ACCURACY,
            Stats.EVASION
          };
          foreach (Stats index in statsArray1)
          {
            int stage = opponent.stages[(int) index];
            if (stage > 0)
              num58 += stage;
            else
              num59 += stage;
          }
          if (num58 != 0 || (uint) num59 > 0U)
            a1 += (float) ((num58 - num59) * 10);
          else
            a1 -= 95f;
          break;
        case PokemonUnity.Attack.Data.Effects.x15D:
          if (skill >= 48 && opponent.hasWorkingAbility(Abilities.LIQUID_OOZE))
          {
            a1 -= 80f;
            break;
          }
          if (attacker.HP <= attacker.TotalHP / 2)
            a1 += 40f;
          break;
        case PokemonUnity.Attack.Data.Effects.x15F:
          int num60 = 0;
          for (int index = 0; index < 4; ++index)
          {
            IBattler battler = this.battlers[index];
            if (battler.pbHasType(Types.GRASS) && battler is IBattlerEffect battlerEffect4 && !battlerEffect4.pbTooHigh(Stats.DEFENSE))
            {
              ++num60;
              if (attacker.pbIsOpposing(battler.Index))
                a1 -= 20f;
              else
                a1 -= (float) (attacker.stages[2] * 10);
            }
          }
          if (num60 == 0)
          {
            a1 -= 95f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x162:
          if (this.pbRoughStat(attacker, Stats.SPEED, skill) > this.pbRoughStat(opponent, Stats.SPEED, skill))
          {
            a1 -= 90f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x164:
        case PokemonUnity.Attack.Data.Effects.x16A:
          if (attacker.effects.ProtectRate > (short) 1 || opponent.effects.HyperBeam > 0)
          {
            a1 -= 90f;
            break;
          }
          if (skill >= 32)
            a1 -= (float) ((int) attacker.effects.ProtectRate * 40);
          if (attacker.turncount == 0)
            a1 += 50f;
          if ((uint) opponent.effects.TwoTurnAttack > 0U)
            a1 += 30f;
          break;
        case PokemonUnity.Attack.Data.Effects.x165:
          if ((battlerEffect1 != null ? (battlerEffect1.pbCanReduceStatStage(Stats.ATTACK, attacker) ? 1 : 0) : 0) == 0)
          {
            a1 -= 90f;
            break;
          }
          a1 += (float) (opponent.stages[1] * 20);
          if (skill >= 32)
          {
            bool flag23 = false;
            foreach (IBattleMove move38 in opponent.moves)
            {
              if (move38.id != Moves.NONE && move38.basedamage > 0 && move38.pbIsPhysical(move38.Type))
                flag23 = true;
            }
            if (flag23)
              a1 += 20f;
            else if (skill >= 48)
              a1 -= 90f;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x166:
          if (opponent.stages[4] > 0)
          {
            a1 += 20f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x167:
          if (attacker.stages[2] < 0)
          {
            a1 += 20f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x16B:
          if (!this.doublebattle)
          {
            a1 -= 100f;
            break;
          }
          if (attacker.pbPartner.isFainted())
          {
            a1 -= 90f;
            break;
          }
          a1 -= (float) (attacker.pbPartner.stages[5] * 10);
          break;
        case PokemonUnity.Attack.Data.Effects.x16C:
          int num61 = 0;
          for (int index = 0; index < 4; ++index)
          {
            IBattler battler = this.battlers[index];
            if (battler.Status == Status.POISON && battler is IBattlerEffect battlerEffect5 && (!battlerEffect5.pbTooLow(Stats.ATTACK) || !battlerEffect5.pbTooLow(Stats.SPATK) || !battlerEffect5.pbTooLow(Stats.SPEED)))
            {
              ++num61;
              if (attacker.pbIsOpposing(battler.Index))
                a1 = a1 + (float) (attacker.stages[1] * 10) + (float) (attacker.stages[4] * 10) + (float) (attacker.stages[3] * 10);
              else
                a1 -= 20f;
            }
          }
          if (num61 == 0)
          {
            a1 -= 95f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x16D:
          if (move.basedamage == 0)
          {
            if (battlerEffect1 != null && !battlerEffect1.pbCanReduceStatStage(Stats.ATTACK, attacker))
            {
              a1 -= 90f;
              break;
            }
            a1 += (float) (opponent.stages[1] * 20);
            if (skill >= 32)
            {
              bool flag24 = false;
              foreach (IBattleMove move39 in opponent.moves)
              {
                if (move39.id != Moves.NONE && move39.basedamage > 0 && move39.pbIsPhysical(move39.Type))
                  flag24 = true;
              }
              if (flag24)
                a1 += 20f;
              else if (skill >= 48)
                a1 -= 90f;
            }
            break;
          }
          if (opponent.stages[1] > 0)
            a1 += 20f;
          if (skill >= 32)
          {
            bool flag25 = false;
            foreach (IBattleMove move40 in opponent.moves)
            {
              if (move40.id != Moves.NONE && move40.basedamage > 0 && move40.pbIsPhysical(move40.Type))
                flag25 = true;
            }
            if (flag25)
              a1 += 20f;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x16E:
          if (battlerEffect2 != null && battlerEffect2.pbTooHigh(Stats.SPATK) && battlerEffect2 != null && battlerEffect2.pbTooHigh(Stats.SPDEF) && battlerEffect2 != null && battlerEffect2.pbTooHigh(Stats.SPEED))
          {
            a1 -= 90f;
            break;
          }
          a1 = a1 - (float) (attacker.stages[4] * 10) - (float) (attacker.stages[5] * 10) - (float) (attacker.stages[3] * 10);
          if (skill >= 32)
          {
            bool flag26 = false;
            foreach (IBattleMove move41 in attacker.moves)
            {
              if (move41.id != Moves.NONE && move41.basedamage > 0 && move41.pbIsSpecial(move41.Type))
                flag26 = true;
            }
            if (flag26)
              a1 += 20f;
            else if (skill >= 48)
              a1 -= 90f;
          }
          if (skill >= 48)
          {
            int num62 = this.pbRoughStat(attacker, Stats.SPEED, skill);
            int num63 = this.pbRoughStat(opponent, Stats.SPEED, skill);
            if (num62 < num63 && num62 * 2 > num63)
              a1 += 30f;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x16F:
          if (battlerEffect2 != null && battlerEffect2.pbTooHigh(Stats.DEFENSE) && battlerEffect2 != null && battlerEffect2.pbTooHigh(Stats.SPDEF) && !attacker.pbPartner.isFainted() && attacker.pbPartner is IBattlerEffect pbPartner && pbPartner.pbTooHigh(Stats.DEFENSE) && pbPartner.pbTooHigh(Stats.SPDEF))
          {
            a1 -= 90f;
            break;
          }
          a1 = a1 - (float) (attacker.stages[2] * 10) - (float) (attacker.stages[5] * 10);
          if (!attacker.pbPartner.isFainted())
            a1 = a1 - (float) (attacker.pbPartner.stages[2] * 10) - (float) (attacker.pbPartner.stages[5] * 10);
          break;
        case PokemonUnity.Attack.Data.Effects.x170:
          a1 -= 90f;
          break;
        case PokemonUnity.Attack.Data.Effects.x172:
          a1 -= 95f;
          if (skill >= 48)
          {
            a1 = 0.0f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x176:
          if (opponent.effects.MeanLook >= 0)
          {
            a1 -= 90f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x178:
          if (opponent.pbHasType(Types.GRASS))
          {
            a1 -= 90f;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x179:
          if (attacker.turncount == 0)
          {
            a1 += 30f;
            break;
          }
          a1 -= 90f;
          if (skill >= 100)
            a1 = 0.0f;
          break;
        case PokemonUnity.Attack.Data.Effects.x17A:
          if (this.pbRoughStat(attacker, Stats.SPEED, skill) > this.pbRoughStat(opponent, Stats.SPEED, skill))
          {
            a1 -= 90f;
            break;
          }
          if (opponent.pbHasMoveType(Types.FIRE))
            a1 += 30f;
          break;
        case PokemonUnity.Attack.Data.Effects.x17C:
          if (battlerEffect1 != null && battlerEffect1.pbCanFreeze(attacker, false))
          {
            a1 += 30f;
            if (skill >= 48 && opponent.hasWorkingAbility(Abilities.MARVEL_SCALE))
              a1 -= 20f;
            break;
          }
          break;
      }
      int moveScore;
      if ((double) a1 <= 0.0)
      {
        moveScore = (int) a1;
      }
      else
      {
        if (attacker.pbNonActivePokemonCount == 0 && skill >= 32 && (skill < 48 || opponent.pbNonActivePokemonCount <= 0))
        {
          if (move.basedamage == 0)
            a1 /= 1.5f;
          else if (opponent.HP <= opponent.TotalHP / 2)
            a1 *= 1.5f;
        }
        if (opponent.effects.TwoTurnAttack > Moves.NONE && skill >= 48)
        {
          PokemonUnity.Attack.Data.Effects effect = Kernal.MoveData[opponent.effects.TwoTurnAttack].Effect;
          int num64;
          if (move.Accuracy > 0)
          {
            if (((IEnumerable<PokemonUnity.Attack.Data.Effects>) new PokemonUnity.Attack.Data.Effects[6]
            {
              PokemonUnity.Attack.Data.Effects.x09C,
              PokemonUnity.Attack.Data.Effects.x101,
              PokemonUnity.Attack.Data.Effects.x100,
              PokemonUnity.Attack.Data.Effects.x108,
              PokemonUnity.Attack.Data.Effects.x111,
              PokemonUnity.Attack.Data.Effects.x138
            }).Contains<PokemonUnity.Attack.Data.Effects>(effect) || opponent.effects.SkyDrop)
            {
              num64 = attacker.SPE > opponent.SPE ? 1 : 0;
              goto label_1648;
            }
          }
          num64 = 0;
label_1648:
          if (num64 != 0)
          {
            if (skill >= 100)
            {
              bool flag27 = false;
              switch (effect)
              {
                case PokemonUnity.Attack.Data.Effects.x09C:
                case PokemonUnity.Attack.Data.Effects.x108:
                  if (move.Effect != PokemonUnity.Attack.Data.Effects.x099 || move.Effect != PokemonUnity.Attack.Data.Effects.x14E || move.Effect != PokemonUnity.Attack.Data.Effects.x096 || move.Effect != PokemonUnity.Attack.Data.Effects.x093 || move.Effect != PokemonUnity.Attack.Data.Effects.x0D0 || move.Effect != PokemonUnity.Attack.Data.Effects.x120 || move.id != Moves.WHIRLWIND)
                  {
                    flag27 = true;
                    break;
                  }
                  break;
                case PokemonUnity.Attack.Data.Effects.x100:
                  if (move.Effect != PokemonUnity.Attack.Data.Effects.x102 || move.Effect != PokemonUnity.Attack.Data.Effects.x106)
                  {
                    flag27 = true;
                    break;
                  }
                  break;
                case PokemonUnity.Attack.Data.Effects.x101:
                  if (move.Effect != PokemonUnity.Attack.Data.Effects.x094 || move.Effect != PokemonUnity.Attack.Data.Effects.x07F)
                  {
                    flag27 = true;
                    break;
                  }
                  break;
                case PokemonUnity.Attack.Data.Effects.x111:
                  flag27 = true;
                  break;
                case PokemonUnity.Attack.Data.Effects.x138:
                  if (move.Effect != PokemonUnity.Attack.Data.Effects.x099 || move.Effect != PokemonUnity.Attack.Data.Effects.x14E || move.Effect != PokemonUnity.Attack.Data.Effects.x096 || move.Effect != PokemonUnity.Attack.Data.Effects.x093 || move.Effect != PokemonUnity.Attack.Data.Effects.x0D0 || move.Effect != PokemonUnity.Attack.Data.Effects.x120)
                  {
                    flag27 = true;
                    break;
                  }
                  break;
              }
              if (opponent.effects.SkyDrop && (move.Effect != PokemonUnity.Attack.Data.Effects.x099 || move.Effect != PokemonUnity.Attack.Data.Effects.x14E || move.Effect != PokemonUnity.Attack.Data.Effects.x096 || move.Effect != PokemonUnity.Attack.Data.Effects.x093 || move.Effect != PokemonUnity.Attack.Data.Effects.x0D0 || move.Effect != PokemonUnity.Attack.Data.Effects.x120))
                flag27 = true;
              if (flag27)
                a1 -= 80f;
            }
            else
              a1 -= 80f;
          }
        }
        if ((attacker.hasWorkingItem(Items.CHOICE_BAND) || attacker.hasWorkingItem(Items.CHOICE_SPECS) || attacker.hasWorkingItem(Items.CHOICE_SCARF)) && skill >= 32)
        {
          if (move.basedamage >= 60)
            a1 += 60f;
          else if (move.basedamage > 0)
            a1 += 30f;
          else if (move.Effect == PokemonUnity.Attack.Data.Effects.x0B2)
            a1 += 70f;
          else
            a1 -= 60f;
        }
        if (attacker.Status == Status.SLEEP && skill >= 32 && move.Effect != PokemonUnity.Attack.Data.Effects.x05D && move.Effect != PokemonUnity.Attack.Data.Effects.x062)
        {
          bool flag28 = false;
          foreach (IBattleMove move42 in attacker.moves)
          {
            if (move42.Effect == PokemonUnity.Attack.Data.Effects.x05D || move42.Effect == PokemonUnity.Attack.Data.Effects.x062)
            {
              flag28 = true;
              break;
            }
          }
          if (flag28)
            a1 -= 60f;
        }
        if (attacker.Status == Status.FROZEN && skill >= 32)
        {
          if (move.Flags.Defrost)
          {
            a1 += 40f;
          }
          else
          {
            bool flag29 = false;
            foreach (IBattleMove move43 in attacker.moves)
            {
              if (move43.Flags.Defrost)
              {
                flag29 = true;
                break;
              }
            }
            if (flag29)
              a1 -= 60f;
          }
        }
        float num65;
        if (move.basedamage > 0)
        {
          float num66 = this.pbTypeModifier(move.Type, attacker, opponent);
          if ((double) num66 == 0.0 || (double) a1 <= 0.0)
            num65 = 0.0f;
          else if (skill >= 32 && (double) num66 <= 8.0 && opponent.hasWorkingAbility(Abilities.WONDER_GUARD))
            num65 = 0.0f;
          else if (skill >= 32 && move.Type == Types.GROUND && (opponent.hasWorkingAbility(Abilities.LEVITATE) || opponent.effects.MagnetRise > 0))
            num65 = 0.0f;
          else if (skill >= 32 && move.Type == Types.FIRE && opponent.hasWorkingAbility(Abilities.FLASH_FIRE))
            num65 = 0.0f;
          else if (skill >= 32 && move.Type == Types.WATER && (opponent.hasWorkingAbility(Abilities.WATER_ABSORB) || opponent.hasWorkingAbility(Abilities.STORM_DRAIN) || opponent.hasWorkingAbility(Abilities.DRY_SKIN)))
            num65 = 0.0f;
          else if (skill >= 32 && move.Type == Types.GRASS && opponent.hasWorkingAbility(Abilities.SAP_SIPPER))
            num65 = 0.0f;
          else if (skill >= 32 && move.Type == Types.ELECTRIC && (opponent.hasWorkingAbility(Abilities.VOLT_ABSORB) || opponent.hasWorkingAbility(Abilities.LIGHTNING_ROD) || opponent.hasWorkingAbility(Abilities.MOTOR_DRIVE)))
          {
            num65 = 0.0f;
          }
          else
          {
            int basedamage = move.basedamage;
            if (move.basedamage == 1)
              basedamage = 60;
            if (skill >= 32)
              basedamage = this.pbBetterBaseDamage(move, attacker, opponent, skill, basedamage);
            int num67 = this.pbRoughDamage(move, attacker, opponent, skill, (double) basedamage);
            int num68 = this.pbRoughAccuracy(move, attacker, opponent, skill);
            float num69 = (float) (num67 * num68) / 100f;
            nullable1 = Kernal.MoveMetaData[move.id].MaxTurns;
            int num70 = 1;
            if (nullable1.GetValueOrDefault() > num70 & nullable1.HasValue || move.Effect == PokemonUnity.Attack.Data.Effects.x051)
              num69 *= 0.0f;
            if (!opponent.hasWorkingAbility(Abilities.INNER_FOCUS) && opponent.effects.Substitute == 0)
            {
              if (attacker.hasWorkingItem(Items.KINGS_ROCK) || attacker.hasWorkingItem(Items.RAZOR_FANG))
                num69 *= 1.05f;
              else if (attacker.hasWorkingAbility(Abilities.STENCH) && move.Effect != PokemonUnity.Attack.Data.Effects.x114 && move.Effect != PokemonUnity.Attack.Data.Effects.x112 && move.Effect != PokemonUnity.Attack.Data.Effects.x113 && move.Effect != PokemonUnity.Attack.Data.Effects.x020 && move.Effect != PokemonUnity.Attack.Data.Effects.x097 && move.Effect != PokemonUnity.Attack.Data.Effects.x05D && move.Effect != PokemonUnity.Attack.Data.Effects.x09F && move.Effect != PokemonUnity.Attack.Data.Effects.x093 && move.Effect != PokemonUnity.Attack.Data.Effects.x04C)
                num69 *= 1.05f;
            }
            float a2 = num69 * 100f / (float) opponent.HP;
            if ((double) a2 < 40.0)
              a2 /= 2f;
            if (attacker.Level - 10 > opponent.Level)
              a2 *= 1.2f;
            float num71 = (float) (int) Math.Round((double) a2);
            if ((double) num71 > 120.0)
              num71 = 120f;
            if ((double) num71 > 100.0)
              num71 += 40f;
            float num72 = (float) (int) Math.Round((double) a1);
            float num73 = num72;
            num65 = num72 + num71;
            GameDebug.Log(string.Format("[AI] #{0} damage calculated (#{1}=>#{2}% of target's #{3} HP), score change #{4}=>#{5}", (object) Game._INTL(move.id.ToString(TextScripts.Name)), (object) num67, (object) num71, (object) opponent.HP, (object) num73, (object) num65));
          }
        }
        else
        {
          num65 = (a1 - 10f) * ((float) this.pbRoughAccuracy(move, attacker, opponent, skill) / 100f);
          if ((double) num65 <= 10.0 && skill >= 48)
            num65 = 0.0f;
        }
        if ((double) num65 < 0.0)
          num65 = 0.0f;
        moveScore = (int) num65;
      }
      return moveScore;
    }

    public float pbTypeModifier(Types type, IBattler attacker, IBattler opponent)
    {
      if (type < Types.NONE || type == Types.GROUND && opponent.pbHasType(Types.FLYING) && opponent.hasWorkingItem(Items.IRON_BALL))
        return 8f;
      Types atk = type;
      Types target1 = opponent.Type1;
      Types target2 = opponent.Type2;
      Types type3 = opponent.effects.Type3;
      if (target1 == Types.FLYING && opponent.effects.Roost)
        target1 = target2 != Types.FLYING || type3 != Types.FLYING ? target2 : Types.NORMAL;
      if (target2 == Types.FLYING && opponent.effects.Roost)
        target2 = target1;
      float num1 = atk.GetEffectiveness(target1);
      float num2 = target1 == target2 ? 2f : atk.GetEffectiveness(target2);
      float num3 = type3 < Types.NONE || target1 == type3 || target2 == type3 ? 2f : atk.GetEffectiveness(type3);
      if (opponent.hasWorkingItem(Items.RING_TARGET))
      {
        if ((double) num1 == 0.0)
          num1 = 2f;
        if ((double) num2 == 0.0)
          num2 = 2f;
        if ((double) num3 == 0.0)
          num3 = 2f;
      }
      if (!opponent.isAirborne(attacker.hasMoldBreaker()) && atk == Types.GROUND)
      {
        if (target1 == Types.FLYING)
          num1 = 2f;
        if (target2 == Types.FLYING)
          num2 = 2f;
        if (type3 == Types.FLYING)
          num3 = 2f;
      }
      return num1 * num2 * num3;
    }

    public float pbTypeModifier2(IBattler battlerThis, IBattler battlerOther) => battlerThis.Type1 == battlerThis.Type2 ? 4f * this.pbTypeModifier(battlerThis.Type1, battlerThis, battlerOther) : this.pbTypeModifier(battlerThis.Type1, battlerThis, battlerOther) * this.pbTypeModifier(battlerThis.Type2, battlerThis, battlerOther) * 2f;

    public int pbRoughStat(IBattler battler, Stats stat, int skill)
    {
      if (skill >= 48 && stat == Stats.SPEED)
        return battler.SPE;
      int[] numArray1 = new int[13]
      {
        2,
        2,
        2,
        2,
        2,
        2,
        2,
        3,
        4,
        5,
        6,
        7,
        8
      };
      int[] numArray2 = new int[13]
      {
        8,
        7,
        6,
        5,
        4,
        3,
        2,
        2,
        2,
        2,
        2,
        2,
        2
      };
      int index = battler.stages[(int) stat] + 6;
      int num = 0;
      switch (stat)
      {
        case Stats.ATTACK:
          num = battler.pokemon.ATK;
          break;
        case Stats.DEFENSE:
          num = battler.pokemon.DEF;
          break;
        case Stats.SPEED:
          num = battler.pokemon.SPE;
          break;
        case Stats.SPATK:
          num = battler.pokemon.SPA;
          break;
        case Stats.SPDEF:
          num = battler.pokemon.SPD;
          break;
      }
      return (int) Math.Floor((double) num * 1.0 * (double) numArray1[index] / (double) numArray2[index]);
    }

    public int pbBetterBaseDamage(
      IBattleMove move,
      IBattler attacker,
      IBattler opponent,
      int skill,
      int basedamage)
    {
      switch (move.Effect)
      {
        case PokemonUnity.Attack.Data.Effects.x027:
          basedamage = opponent.TotalHP;
          break;
        case PokemonUnity.Attack.Data.Effects.x029:
          basedamage = (int) Math.Floor((double) opponent.HP / 2.0);
          break;
        case PokemonUnity.Attack.Data.Effects.x02A:
          basedamage = 40;
          break;
        case PokemonUnity.Attack.Data.Effects.x02D:
        case PokemonUnity.Attack.Data.Effects.x04E:
          basedamage *= 2;
          break;
        case PokemonUnity.Attack.Data.Effects.x058:
          basedamage = attacker.Level;
          break;
        case PokemonUnity.Attack.Data.Effects.x059:
          basedamage = attacker.Level;
          break;
        case PokemonUnity.Attack.Data.Effects.x05A:
          basedamage = 60;
          break;
        case PokemonUnity.Attack.Data.Effects.x064:
          int num1 = (int) Math.Floor(48.0 * (double) attacker.HP / (double) attacker.TotalHP);
          basedamage = 20;
          if (num1 < 33)
            basedamage = 40;
          if (num1 < 17)
            basedamage = 80;
          if (num1 < 10)
            basedamage = 100;
          if (num1 < 5)
            basedamage = 150;
          if (num1 < 2)
          {
            basedamage = 200;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x069:
          basedamage *= 6;
          break;
        case PokemonUnity.Attack.Data.Effects.x076:
          if (skill >= 32 && attacker.effects.DefenseCurl)
          {
            basedamage *= 2;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x078:
          basedamage <<= attacker.effects.FuryCutter - 1;
          break;
        case PokemonUnity.Attack.Data.Effects.x07A:
          basedamage = Math.Max((int) Math.Floor((double) (attacker.Happiness * 2) / 5.0), 1);
          break;
        case PokemonUnity.Attack.Data.Effects.x07B:
          basedamage = 50;
          break;
        case PokemonUnity.Attack.Data.Effects.x07C:
          basedamage = Math.Max((int) Math.Floor((double) (((int) byte.MaxValue - attacker.Happiness) * 2) / 5.0), 1);
          break;
        case PokemonUnity.Attack.Data.Effects.x07F:
          basedamage = 71;
          if (Kernal.MoveData[opponent.effects.TwoTurnAttack].Effect == PokemonUnity.Attack.Data.Effects.x101)
          {
            basedamage *= 2;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x083:
          basedamage = 20;
          break;
        case PokemonUnity.Attack.Data.Effects.x088:
          basedamage = PokeBattle_Move_090.pbHiddenPower(attacker.IV).Value;
          break;
        case PokemonUnity.Attack.Data.Effects.x091:
          basedamage = 60;
          break;
        case PokemonUnity.Attack.Data.Effects.x093:
        case PokemonUnity.Attack.Data.Effects.x096:
          if (Kernal.MoveData[opponent.effects.TwoTurnAttack].Effect == PokemonUnity.Attack.Data.Effects.x09C || Kernal.MoveData[opponent.effects.TwoTurnAttack].Effect == PokemonUnity.Attack.Data.Effects.x108 || Kernal.MoveData[opponent.effects.TwoTurnAttack].Effect == PokemonUnity.Attack.Data.Effects.x138)
          {
            basedamage *= 2;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x094:
          if (Kernal.MoveData[opponent.effects.TwoTurnAttack].Effect == PokemonUnity.Attack.Data.Effects.x101)
          {
            basedamage *= 2;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x098:
          if (this.pbWeather != Weather.NONE && this.pbWeather != Weather.SUNNYDAY)
          {
            basedamage = (int) Math.Floor((double) basedamage * 0.5);
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x09B:
          IPokemon[] pokemonArray = this.pbParty(attacker.Index);
          int num2 = 0;
          for (int index = 0; index < pokemonArray.Length; ++index)
          {
            if (pokemonArray[index].IsNotNullOrNone() && !pokemonArray[index].isEgg && pokemonArray[index].HP > 0 && pokemonArray[index].Status == Status.NONE)
              ++num2;
          }
          basedamage *= num2;
          break;
        case PokemonUnity.Attack.Data.Effects.x0A2:
          basedamage *= attacker.effects.Stockpile;
          break;
        case PokemonUnity.Attack.Data.Effects.x0AA:
          if (attacker.Status == Status.POISON || attacker.Status == Status.BURN || attacker.Status == Status.PARALYSIS)
          {
            basedamage *= 2;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0AC:
          if (opponent.Status == Status.PARALYSIS)
          {
            basedamage *= 2;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0BE:
          basedamage = opponent.HP - attacker.HP;
          break;
        case PokemonUnity.Attack.Data.Effects.x0BF:
          basedamage = Math.Max((int) Math.Floor(150.0 * (double) attacker.HP / (double) attacker.TotalHP), 1);
          break;
        case PokemonUnity.Attack.Data.Effects.x0C5:
          float num3 = opponent.Weight();
          basedamage = 20;
          if ((double) num3 > 100.0)
            basedamage = 40;
          if ((double) num3 > 250.0)
            basedamage = 60;
          if ((double) num3 > 500.0)
            basedamage = 80;
          if ((double) num3 > 1000.0)
            basedamage = 100;
          if ((double) num3 > 2000.0)
          {
            basedamage = 120;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0CC:
          if ((uint) this.pbWeather > 0U)
          {
            basedamage *= 2;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0DA:
          if (opponent.Status == Status.SLEEP)
          {
            basedamage *= 2;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0DC:
          basedamage = Math.Max(Math.Min((int) Math.Floor(25.0 * (double) this.pbRoughStat(opponent, Stats.SPEED, skill) / (double) this.pbRoughStat(attacker, Stats.SPEED, skill)), 150), 1);
          break;
        case PokemonUnity.Attack.Data.Effects.x0DE:
          if (opponent.HP <= (int) Math.Floor((double) opponent.TotalHP / 2.0))
          {
            basedamage *= 2;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0DF:
          KeyValuePair<Items, int>[] keyValuePairArray = new KeyValuePair<Items, int>[64]
          {
            new KeyValuePair<Items, int>(Items.CHERI_BERRY, 60),
            new KeyValuePair<Items, int>(Items.CHESTO_BERRY, 60),
            new KeyValuePair<Items, int>(Items.PECHA_BERRY, 60),
            new KeyValuePair<Items, int>(Items.RAWST_BERRY, 60),
            new KeyValuePair<Items, int>(Items.ASPEAR_BERRY, 60),
            new KeyValuePair<Items, int>(Items.LEPPA_BERRY, 60),
            new KeyValuePair<Items, int>(Items.ORAN_BERRY, 60),
            new KeyValuePair<Items, int>(Items.PERSIM_BERRY, 60),
            new KeyValuePair<Items, int>(Items.LUM_BERRY, 60),
            new KeyValuePair<Items, int>(Items.SITRUS_BERRY, 60),
            new KeyValuePair<Items, int>(Items.FIGY_BERRY, 60),
            new KeyValuePair<Items, int>(Items.WIKI_BERRY, 60),
            new KeyValuePair<Items, int>(Items.MAGO_BERRY, 60),
            new KeyValuePair<Items, int>(Items.AGUAV_BERRY, 60),
            new KeyValuePair<Items, int>(Items.IAPAPA_BERRY, 60),
            new KeyValuePair<Items, int>(Items.RAZZ_BERRY, 60),
            new KeyValuePair<Items, int>(Items.OCCA_BERRY, 60),
            new KeyValuePair<Items, int>(Items.PASSHO_BERRY, 60),
            new KeyValuePair<Items, int>(Items.WACAN_BERRY, 60),
            new KeyValuePair<Items, int>(Items.RINDO_BERRY, 60),
            new KeyValuePair<Items, int>(Items.YACHE_BERRY, 60),
            new KeyValuePair<Items, int>(Items.CHOPLE_BERRY, 60),
            new KeyValuePair<Items, int>(Items.KEBIA_BERRY, 60),
            new KeyValuePair<Items, int>(Items.SHUCA_BERRY, 60),
            new KeyValuePair<Items, int>(Items.COBA_BERRY, 60),
            new KeyValuePair<Items, int>(Items.PAYAPA_BERRY, 60),
            new KeyValuePair<Items, int>(Items.TANGA_BERRY, 60),
            new KeyValuePair<Items, int>(Items.CHARTI_BERRY, 60),
            new KeyValuePair<Items, int>(Items.KASIB_BERRY, 60),
            new KeyValuePair<Items, int>(Items.HABAN_BERRY, 60),
            new KeyValuePair<Items, int>(Items.COLBUR_BERRY, 60),
            new KeyValuePair<Items, int>(Items.BABIRI_BERRY, 60),
            new KeyValuePair<Items, int>(Items.CHILAN_BERRY, 60),
            new KeyValuePair<Items, int>(Items.BLUK_BERRY, 70),
            new KeyValuePair<Items, int>(Items.NANAB_BERRY, 70),
            new KeyValuePair<Items, int>(Items.WEPEAR_BERRY, 70),
            new KeyValuePair<Items, int>(Items.PINAP_BERRY, 70),
            new KeyValuePair<Items, int>(Items.POMEG_BERRY, 70),
            new KeyValuePair<Items, int>(Items.KELPSY_BERRY, 70),
            new KeyValuePair<Items, int>(Items.QUALOT_BERRY, 70),
            new KeyValuePair<Items, int>(Items.HONDEW_BERRY, 70),
            new KeyValuePair<Items, int>(Items.GREPA_BERRY, 70),
            new KeyValuePair<Items, int>(Items.TAMATO_BERRY, 70),
            new KeyValuePair<Items, int>(Items.CORNN_BERRY, 70),
            new KeyValuePair<Items, int>(Items.MAGOST_BERRY, 70),
            new KeyValuePair<Items, int>(Items.RABUTA_BERRY, 70),
            new KeyValuePair<Items, int>(Items.NOMEL_BERRY, 70),
            new KeyValuePair<Items, int>(Items.SPELON_BERRY, 70),
            new KeyValuePair<Items, int>(Items.PAMTRE_BERRY, 70),
            new KeyValuePair<Items, int>(Items.WATMEL_BERRY, 80),
            new KeyValuePair<Items, int>(Items.DURIN_BERRY, 80),
            new KeyValuePair<Items, int>(Items.BELUE_BERRY, 80),
            new KeyValuePair<Items, int>(Items.LIECHI_BERRY, 80),
            new KeyValuePair<Items, int>(Items.GANLON_BERRY, 80),
            new KeyValuePair<Items, int>(Items.SALAC_BERRY, 80),
            new KeyValuePair<Items, int>(Items.PETAYA_BERRY, 80),
            new KeyValuePair<Items, int>(Items.APICOT_BERRY, 80),
            new KeyValuePair<Items, int>(Items.LANSAT_BERRY, 80),
            new KeyValuePair<Items, int>(Items.STARF_BERRY, 80),
            new KeyValuePair<Items, int>(Items.ENIGMA_BERRY, 80),
            new KeyValuePair<Items, int>(Items.MICLE_BERRY, 80),
            new KeyValuePair<Items, int>(Items.CUSTAP_BERRY, 80),
            new KeyValuePair<Items, int>(Items.JABOCA_BERRY, 80),
            new KeyValuePair<Items, int>(Items.ROWAP_BERRY, 80)
          };
          bool flag = false;
          foreach (KeyValuePair<Items, int> keyValuePair in keyValuePairArray)
          {
            if (attacker.Item == keyValuePair.Key)
            {
              basedamage = keyValuePair.Value;
              break;
            }
            if (flag)
              break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x0E4:
          basedamage = 60;
          break;
        case PokemonUnity.Attack.Data.Effects.x0EC:
          basedamage = new int[5]{ 200, 80, 60, 50, 40 }[Math.Min(move.PP - 1, 4)];
          break;
        case PokemonUnity.Attack.Data.Effects.x0EE:
          basedamage = Math.Max((int) Math.Floor(120.0 * (double) opponent.HP / (double) opponent.TotalHP), 1);
          break;
        case PokemonUnity.Attack.Data.Effects.x0F6:
          int num4 = 0;
          Stats[] statsArray1 = new Stats[7]
          {
            Stats.ATTACK,
            Stats.DEFENSE,
            Stats.SPEED,
            Stats.SPATK,
            Stats.SPDEF,
            Stats.ACCURACY,
            Stats.EVASION
          };
          foreach (Stats index in statsArray1)
          {
            if (opponent.stages[(int) index] > 0)
              num4 += opponent.stages[(int) index];
          }
          basedamage = Math.Min(20 * (num4 + 3), 200);
          break;
        case PokemonUnity.Attack.Data.Effects.x102:
          if (Kernal.MoveData[opponent.effects.TwoTurnAttack].Effect == PokemonUnity.Attack.Data.Effects.x100)
          {
            basedamage *= 2;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x106:
          if (skill >= 32 && Kernal.MoveData[opponent.effects.TwoTurnAttack].Effect == PokemonUnity.Attack.Data.Effects.x100)
          {
            basedamage *= 2;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x11C:
          if (opponent.Status == Status.POISON)
          {
            basedamage *= 2;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x121:
          basedamage *= 2;
          break;
        case PokemonUnity.Attack.Data.Effects.x124:
          int num5 = (int) Math.Floor((double) attacker.Weight() / (double) opponent.Weight());
          basedamage = 40;
          if (num5 >= 2)
            basedamage = 60;
          if (num5 >= 3)
            basedamage = 80;
          if (num5 >= 4)
            basedamage = 100;
          if (num5 >= 5)
          {
            basedamage = 120;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x126:
          int num6 = (int) Math.Floor((double) attacker.SPE / (double) opponent.SPE);
          basedamage = 40;
          if (num6 >= 1)
            basedamage = 60;
          if (num6 >= 2)
            basedamage = 80;
          if (num6 >= 3)
            basedamage = 120;
          if (num6 >= 4)
          {
            basedamage = 150;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x12F:
          basedamage *= (int) attacker.pbOwnSide.EchoedVoiceCounter;
          break;
        case PokemonUnity.Attack.Data.Effects.x132:
          int num7 = 0;
          Stats[] statsArray2 = new Stats[7]
          {
            Stats.ATTACK,
            Stats.DEFENSE,
            Stats.SPEED,
            Stats.SPATK,
            Stats.SPDEF,
            Stats.ACCURACY,
            Stats.EVASION
          };
          foreach (Stats index in statsArray2)
          {
            if (attacker.stages[(int) index] > 0)
              num7 += attacker.stages[(int) index];
          }
          basedamage = 20 * (num7 + 1);
          break;
        case PokemonUnity.Attack.Data.Effects.x137:
          if ((uint) opponent.Status > 0U)
          {
            basedamage *= 2;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x13E:
          if (attacker.Item == Items.NONE || attacker.hasWorkingItem(Items.FLYING_GEM))
          {
            basedamage *= 2;
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x141:
          basedamage = attacker.HP;
          break;
        case PokemonUnity.Attack.Data.Effects.x152:
          if (3 >= 0)
          {
            int num8 = 1;
            basedamage = (int) Math.Round((double) (basedamage * num8) / 8.0);
            break;
          }
          break;
        case PokemonUnity.Attack.Data.Effects.x169:
          if (attacker.hasWorkingAbility(Abilities.SKILL_LINK))
          {
            basedamage *= 5;
            break;
          }
          basedamage = (int) Math.Floor((double) (basedamage * 19) / 6.0);
          break;
      }
      return basedamage;
    }

    public int pbRoughDamage(
      IBattleMove move,
      IBattler attacker,
      IBattler opponent,
      int skill,
      double basedamage)
    {
      if (move.Effect == PokemonUnity.Attack.Data.Effects.x083 || move.Effect == PokemonUnity.Attack.Data.Effects.x02A || move.Effect == PokemonUnity.Attack.Data.Effects.x029 || move.Effect == PokemonUnity.Attack.Data.Effects.x058 || move.Effect == PokemonUnity.Attack.Data.Effects.x0BE || move.Effect == PokemonUnity.Attack.Data.Effects.x059 || move.Effect == PokemonUnity.Attack.Data.Effects.x027 || move.Effect == PokemonUnity.Attack.Data.Effects.x05A || move.Effect == PokemonUnity.Attack.Data.Effects.x091 || move.Effect == PokemonUnity.Attack.Data.Effects.x0E4 || move.Effect == PokemonUnity.Attack.Data.Effects.x141)
        return (int) basedamage;
      Types type = move.Type;
      if (skill >= 48)
        type = move.pbType(type, attacker, opponent);
      if (skill >= 48 && attacker.hasWorkingAbility(Abilities.TECHNICIAN) && basedamage <= 60.0)
        basedamage = (double) (int) Math.Round(basedamage * 1.5);
      if (skill >= 32 && attacker.hasWorkingAbility(Abilities.IRON_FIST) && move.Flags.Punching)
        basedamage = (double) (int) Math.Round(basedamage * 1.2);
      if (skill >= 32 && attacker.hasWorkingAbility(Abilities.RECKLESS) && (move.Effect == PokemonUnity.Attack.Data.Effects.x031 || move.Effect == PokemonUnity.Attack.Data.Effects.x0C7 || move.Effect == PokemonUnity.Attack.Data.Effects.x10E || move.Effect == PokemonUnity.Attack.Data.Effects.x107 || move.Effect == PokemonUnity.Attack.Data.Effects.x0FE || move.Effect == PokemonUnity.Attack.Data.Effects.x02E || move.Effect == PokemonUnity.Attack.Data.Effects.x712))
        basedamage = (double) (int) Math.Round(basedamage * 1.2);
      if (skill >= 48 && attacker.hasWorkingAbility(Abilities.FLARE_BOOST) && attacker.Status == Status.BURN && move.pbIsSpecial(type))
        basedamage = (double) (int) Math.Round(basedamage * 1.5);
      if (skill >= 48 && attacker.hasWorkingAbility(Abilities.TOXIC_BOOST) && attacker.Status == Status.POISON && move.pbIsPhysical(type))
        basedamage = (double) (int) Math.Round(basedamage * 1.5);
      if (skill >= 32 && attacker.hasWorkingAbility(Abilities.RIVALRY) && attacker.Gender.HasValue && opponent.Gender.HasValue)
      {
        bool? gender1 = attacker.Gender;
        bool? gender2 = opponent.Gender;
        basedamage = !(gender1.GetValueOrDefault() == gender2.GetValueOrDefault() & gender1.HasValue == gender2.HasValue) ? (double) (int) Math.Round(basedamage * 0.75) : (double) (int) Math.Round(basedamage * 1.25);
      }
      if (skill >= 32 && attacker.hasWorkingAbility(Abilities.SAND_FORCE) && this.pbWeather == Weather.SANDSTORM && (type == Types.ROCK || type == Types.GROUND || type == Types.STEEL))
        basedamage = (double) (int) Math.Round(basedamage * 1.3);
      if (skill >= 100 && opponent.hasWorkingAbility(Abilities.HEATPROOF) && type == Types.FIRE)
        basedamage = (double) (int) Math.Round(basedamage * 0.5);
      if (skill >= 100 && opponent.hasWorkingAbility(Abilities.DRY_SKIN) && type == Types.FIRE)
        basedamage = (double) (int) Math.Round(basedamage * 1.25);
      if (skill >= 48 && attacker.hasWorkingAbility(Abilities.SHEER_FORCE) && move.AddlEffect > 0)
        basedamage = (double) (int) Math.Round(basedamage * 1.3);
      if (attacker.hasWorkingItem(Items.SILK_SCARF) && type == Types.NORMAL || attacker.hasWorkingItem(Items.BLACK_BELT) && type == Types.FIGHTING || attacker.hasWorkingItem(Items.SHARP_BEAK) && type == Types.FLYING || attacker.hasWorkingItem(Items.POISON_BARB) && type == Types.POISON || attacker.hasWorkingItem(Items.SOFT_SAND) && type == Types.GROUND || attacker.hasWorkingItem(Items.HARD_STONE) && type == Types.ROCK || attacker.hasWorkingItem(Items.SILVER_POWDER) && type == Types.BUG || attacker.hasWorkingItem(Items.SPELL_TAG) && type == Types.GHOST || attacker.hasWorkingItem(Items.METAL_COAT) && type == Types.STEEL || attacker.hasWorkingItem(Items.CHARCOAL) && type == Types.FIRE || attacker.hasWorkingItem(Items.MYSTIC_WATER) && type == Types.WATER || attacker.hasWorkingItem(Items.MIRACLE_SEED) && type == Types.GRASS || attacker.hasWorkingItem(Items.MAGNET) && type == Types.ELECTRIC || attacker.hasWorkingItem(Items.TWISTED_SPOON) && type == Types.PSYCHIC || attacker.hasWorkingItem(Items.NEVER_MELT_ICE) && type == Types.ICE || attacker.hasWorkingItem(Items.DRAGON_FANG) && type == Types.DRAGON || attacker.hasWorkingItem(Items.BLACK_GLASSES) && type == Types.DARK)
        basedamage = (double) (int) Math.Round(basedamage * 1.2);
      if (attacker.hasWorkingItem(Items.FIST_PLATE) && type == Types.FIGHTING || attacker.hasWorkingItem(Items.SKY_PLATE) && type == Types.FLYING || attacker.hasWorkingItem(Items.TOXIC_PLATE) && type == Types.POISON || attacker.hasWorkingItem(Items.EARTH_PLATE) && type == Types.GROUND || attacker.hasWorkingItem(Items.STONE_PLATE) && type == Types.ROCK || attacker.hasWorkingItem(Items.INSECT_PLATE) && type == Types.BUG || attacker.hasWorkingItem(Items.SPOOKY_PLATE) && type == Types.GHOST || attacker.hasWorkingItem(Items.IRON_PLATE) && type == Types.STEEL || attacker.hasWorkingItem(Items.FLAME_PLATE) && type == Types.FIRE || attacker.hasWorkingItem(Items.SPLASH_PLATE) && type == Types.WATER || attacker.hasWorkingItem(Items.MEADOW_PLATE) && type == Types.GRASS || attacker.hasWorkingItem(Items.ZAP_PLATE) && type == Types.ELECTRIC || attacker.hasWorkingItem(Items.MIND_PLATE) && type == Types.PSYCHIC || attacker.hasWorkingItem(Items.ICICLE_PLATE) && type == Types.ICE || attacker.hasWorkingItem(Items.DRACO_PLATE) && type == Types.DRAGON || attacker.hasWorkingItem(Items.DREAD_PLATE) && type == Types.DARK)
        basedamage = (double) (int) Math.Round(basedamage * 1.2);
      if (attacker.hasWorkingItem(Items.NORMAL_GEM) && type == Types.NORMAL || attacker.hasWorkingItem(Items.FIGHTING_GEM) && type == Types.FIGHTING || attacker.hasWorkingItem(Items.FLYING_GEM) && type == Types.FLYING || attacker.hasWorkingItem(Items.POISON_GEM) && type == Types.POISON || attacker.hasWorkingItem(Items.GROUND_GEM) && type == Types.GROUND || attacker.hasWorkingItem(Items.ROCK_GEM) && type == Types.ROCK || attacker.hasWorkingItem(Items.BUG_GEM) && type == Types.BUG || attacker.hasWorkingItem(Items.GHOST_GEM) && type == Types.GHOST || attacker.hasWorkingItem(Items.STEEL_GEM) && type == Types.STEEL || attacker.hasWorkingItem(Items.FIRE_GEM) && type == Types.FIRE || attacker.hasWorkingItem(Items.WATER_GEM) && type == Types.WATER || attacker.hasWorkingItem(Items.GRASS_GEM) && type == Types.GRASS || attacker.hasWorkingItem(Items.ELECTRIC_GEM) && type == Types.ELECTRIC || attacker.hasWorkingItem(Items.PSYCHIC_GEM) && type == Types.PSYCHIC || attacker.hasWorkingItem(Items.ICE_GEM) && type == Types.ICE || attacker.hasWorkingItem(Items.DRAGON_GEM) && type == Types.DRAGON || attacker.hasWorkingItem(Items.DARK_GEM) && type == Types.DARK)
        basedamage = (double) (int) Math.Round(basedamage * 1.5);
      if (attacker.hasWorkingItem(Items.ROCK_INCENSE) && type == Types.ROCK)
        basedamage = (double) (int) Math.Round(basedamage * 1.2);
      if (attacker.hasWorkingItem(Items.ROSE_INCENSE) && type == Types.GRASS)
        basedamage = (double) (int) Math.Round(basedamage * 1.2);
      if (attacker.hasWorkingItem(Items.SEA_INCENSE) && type == Types.WATER)
        basedamage = (double) (int) Math.Round(basedamage * 1.2);
      if (attacker.hasWorkingItem(Items.WAVE_INCENSE) && type == Types.WATER)
        basedamage = (double) (int) Math.Round(basedamage * 1.2);
      if (attacker.hasWorkingItem(Items.ODD_INCENSE) && type == Types.PSYCHIC)
        basedamage = (double) (int) Math.Round(basedamage * 1.2);
      if (attacker.hasWorkingItem(Items.MUSCLE_BAND) && move.pbIsPhysical(type))
        basedamage = (double) (int) Math.Round(basedamage * 1.1);
      if (attacker.hasWorkingItem(Items.WISE_GLASSES) && move.pbIsSpecial(type))
        basedamage = (double) (int) Math.Round(basedamage * 1.1);
      if (attacker.Species == Pokemons.PALKIA && attacker.hasWorkingItem(Items.LUSTROUS_ORB) && (type == Types.DRAGON || type == Types.WATER))
        basedamage = (double) (int) Math.Round(basedamage * 1.2);
      if (attacker.Species == Pokemons.DIALGA && attacker.hasWorkingItem(Items.ADAMANT_ORB) && (type == Types.DRAGON || type == Types.STEEL))
        basedamage = (double) (int) Math.Round(basedamage * 1.2);
      if (attacker.Species == Pokemons.GIRATINA && attacker.hasWorkingItem(Items.GRISEOUS_ORB) && (type == Types.DRAGON || type == Types.GHOST))
        basedamage = (double) (int) Math.Round(basedamage * 1.2);
      if (attacker.effects.Charge > 0 && type == Types.ELECTRIC)
        basedamage = (double) (int) Math.Round(basedamage * 2.0);
      if (skill >= 32 && type == Types.FIRE)
      {
        for (int index = 0; index < 4; ++index)
        {
          if (this.battlers[index].effects.WaterSport && !this.battlers[index].isFainted())
          {
            basedamage = (double) (int) Math.Round(basedamage * 0.33);
            break;
          }
        }
      }
      if (skill >= 32 && type == Types.ELECTRIC)
      {
        for (int index = 0; index < 4; ++index)
        {
          if (this.battlers[index].effects.MudSport && !this.battlers[index].isFainted())
          {
            basedamage = (double) (int) Math.Round(basedamage * 0.33);
            break;
          }
        }
      }
      int num1 = this.pbRoughStat(attacker, Stats.ATTACK, skill);
      if (move.Effect == PokemonUnity.Attack.Data.Effects.x12A)
        num1 = this.pbRoughStat(opponent, Stats.ATTACK, skill);
      if (type >= Types.NONE && move.pbIsSpecial(type))
      {
        num1 = this.pbRoughStat(attacker, Stats.SPATK, skill);
        if (move.Effect == PokemonUnity.Attack.Data.Effects.x12A)
          num1 = this.pbRoughStat(opponent, Stats.SPATK, skill);
      }
      if (skill >= 48 && attacker.hasWorkingAbility(Abilities.HUSTLE) && move.pbIsPhysical(type))
        num1 = (int) Math.Round((double) num1 * 1.5);
      if (skill >= 100 && opponent.hasWorkingAbility(Abilities.THICK_FAT) && (type == Types.ICE || type == Types.FIRE))
        num1 = (int) Math.Round((double) num1 * 0.5);
      if (skill >= 32 && attacker.HP <= (int) Math.Floor((double) attacker.TotalHP / 3.0) && (attacker.hasWorkingAbility(Abilities.OVERGROW) && type == Types.GRASS || attacker.hasWorkingAbility(Abilities.BLAZE) && type == Types.FIRE || attacker.hasWorkingAbility(Abilities.TORRENT) && type == Types.WATER || attacker.hasWorkingAbility(Abilities.SWARM) && type == Types.BUG))
        num1 = (int) Math.Round((double) num1 * 1.5);
      if (skill >= 48 && attacker.hasWorkingAbility(Abilities.GUTS) && attacker.Status != Status.NONE && move.pbIsPhysical(type))
        num1 = (int) Math.Round((double) num1 * 1.5);
      if (skill >= 32 && (attacker.hasWorkingAbility(Abilities.PLUS) || attacker.hasWorkingAbility(Abilities.MINUS)) && move.pbIsSpecial(type))
      {
        IBattler pbPartner = attacker.pbPartner;
        if (pbPartner.hasWorkingAbility(Abilities.PLUS) || pbPartner.hasWorkingAbility(Abilities.MINUS))
          num1 = (int) Math.Round((double) num1 * 1.5);
      }
      if (skill >= 32 && attacker.hasWorkingAbility(Abilities.DEFEATIST) && attacker.HP <= (int) Math.Floor((double) attacker.TotalHP / 2.0))
        num1 = (int) Math.Round((double) num1 * 0.5);
      if (skill >= 32 && (attacker.hasWorkingAbility(Abilities.PURE_POWER) || attacker.hasWorkingAbility(Abilities.HUGE_POWER)))
        num1 = (int) Math.Round((double) num1 * 2.0);
      if (skill >= 48 && attacker.hasWorkingAbility(Abilities.SOLAR_POWER) && this.pbWeather == Weather.SUNNYDAY && move.pbIsSpecial(type))
        num1 = (int) Math.Round((double) num1 * 1.5);
      if (skill >= 48 && attacker.hasWorkingAbility(Abilities.FLASH_FIRE) && attacker.effects.FlashFire && type == Types.FIRE)
        num1 = (int) Math.Round((double) num1 * 1.5);
      if (skill >= 32 && attacker.hasWorkingAbility(Abilities.SLOW_START) && attacker.turncount < 5 && move.pbIsPhysical(type))
        num1 = (int) Math.Round((double) num1 * 0.5);
      if (skill >= 48 && this.pbWeather == Weather.SUNNYDAY && move.pbIsPhysical(type))
      {
        if (attacker.hasWorkingAbility(Abilities.FLOWER_GIFT) && attacker.Species == Pokemons.CHERRIM)
          num1 = (int) Math.Round((double) num1 * 1.5);
        if (this.doublebattle && attacker.pbPartner.hasWorkingAbility(Abilities.FLOWER_GIFT) && attacker.pbPartner.Species == Pokemons.CHERRIM)
          num1 = (int) Math.Round((double) num1 * 1.5);
      }
      if (attacker.hasWorkingItem(Items.THICK_CLUB) && (attacker.Species == Pokemons.CUBONE || attacker.Species == Pokemons.MAROWAK) && move.pbIsPhysical(type))
        num1 = (int) Math.Round((double) num1 * 2.0);
      if (attacker.hasWorkingItem(Items.DEEP_SEA_TOOTH) && attacker.Species == Pokemons.CLAMPERL && move.pbIsSpecial(type))
        num1 = (int) Math.Round((double) num1 * 2.0);
      if (attacker.hasWorkingItem(Items.LIGHT_BALL) && attacker.Species == Pokemons.PIKACHU)
        num1 = (int) Math.Round((double) num1 * 2.0);
      if (attacker.hasWorkingItem(Items.SOUL_DEW) && (attacker.Species == Pokemons.LATIAS || attacker.Species == Pokemons.LATIOS) && move.pbIsSpecial(type))
        num1 = (int) Math.Round((double) num1 * 1.5);
      if (attacker.hasWorkingItem(Items.CHOICE_BAND) && move.pbIsPhysical(type))
        num1 = (int) Math.Round((double) num1 * 1.5);
      if (attacker.hasWorkingItem(Items.CHOICE_SPECS) && move.pbIsSpecial(type))
        num1 = (int) Math.Round((double) num1 * 1.5);
      int num2 = this.pbRoughStat(opponent, Stats.DEFENSE, skill);
      bool flag = false;
      if (type >= Types.NONE && move.pbIsSpecial(type) && move.Effect != PokemonUnity.Attack.Data.Effects.x11B)
      {
        num2 = this.pbRoughStat(opponent, Stats.SPDEF, skill);
        flag = true;
      }
      if (skill >= 48 && ((this.pbWeather != Weather.SANDSTORM ? 0 : (opponent.pbHasType(Types.ROCK) ? 1 : 0)) & (flag ? 1 : 0)) != 0)
        num2 = (int) Math.Round((double) num2 * 1.5);
      if (skill >= 100 && opponent.hasWorkingAbility(Abilities.MARVEL_SCALE) && opponent.Status > Status.NONE && move.pbIsPhysical(type))
        num2 = (int) Math.Round((double) num2 * 1.5);
      if (skill >= 100 && this.pbWeather == Weather.SUNNYDAY && move.pbIsSpecial(type))
      {
        if (opponent.hasWorkingAbility(Abilities.FLOWER_GIFT) && opponent.Species == Pokemons.CHERRIM)
          num2 = (int) Math.Round((double) num2 * 1.5);
        if (opponent.pbPartner.hasWorkingAbility(Abilities.FLOWER_GIFT) && opponent.pbPartner.Species == Pokemons.CHERRIM)
          num2 = (int) Math.Round((double) num2 * 1.5);
      }
      if (skill >= 48)
      {
        if (opponent.hasWorkingItem(Items.EVIOLITE) && Kernal.PokemonEvolutionsData[opponent.Species].Length > 0)
          num2 = (int) Math.Round((double) num2 * 1.5);
        if (opponent.hasWorkingItem(Items.DEEP_SEA_SCALE) && opponent.Species == Pokemons.CLAMPERL && move.pbIsSpecial(type))
          num2 = (int) Math.Round((double) num2 * 2.0);
        if (opponent.hasWorkingItem(Items.METAL_POWDER) && opponent.Species == Pokemons.DITTO && !opponent.effects.Transform && move.pbIsPhysical(type))
          num2 = (int) Math.Round((double) num2 * 2.0);
        if (opponent.hasWorkingItem(Items.SOUL_DEW) && (opponent.Species == Pokemons.LATIAS || opponent.Species == Pokemons.LATIOS) && move.pbIsSpecial(type))
          num2 = (int) Math.Round((double) num2 * 1.5);
      }
      double num3 = Math.Floor(Math.Floor(Math.Floor(2.0 * (double) attacker.Level / 5.0 + 2.0) * basedamage * (double) num1 / (double) num2) / 50.0) + 2.0;
      if (skill >= 48 && move.pbTargetsMultiple(attacker))
        num3 = (double) (int) Math.Round(num3 * 0.75);
      if (skill >= 32)
      {
        switch (this.pbWeather)
        {
          case Weather.RAINDANCE:
            if (type == Types.FIRE)
            {
              num3 = (double) (int) Math.Round(num3 * 0.5);
              break;
            }
            if (type == Types.WATER)
            {
              num3 = (double) (int) Math.Round(num3 * 1.5);
              break;
            }
            break;
          case Weather.SUNNYDAY:
            if (type == Types.FIRE)
            {
              num3 = (double) (int) Math.Round(num3 * 1.5);
              break;
            }
            if (type == Types.WATER)
            {
              num3 = (double) (int) Math.Round(num3 * 0.5);
              break;
            }
            break;
        }
      }
      if (skill >= 32 && attacker.pbHasType(type))
        num3 = !attacker.hasWorkingAbility(Abilities.ADAPTABILITY) || skill < 48 ? (double) (int) Math.Round(num3 * 1.5) : (double) (int) Math.Round(num3 * 2.0);
      float num4 = this.pbTypeModifier(type, attacker, opponent);
      if (skill >= 48)
        num3 = (double) (int) Math.Round(num3 * (double) num4 * 1.0 / 8.0);
      if (skill >= 32 && attacker.Status == Status.BURN && move.pbIsPhysical(type) && !attacker.hasWorkingAbility(Abilities.GUTS))
        num3 = (double) (int) Math.Round(num3 * 0.5);
      if (num3 < 1.0)
        num3 = 1.0;
      if (skill >= 48 && opponent.pbOwnSide.Reflect > (byte) 0 && move.pbIsPhysical(type))
        num3 = opponent.pbPartner.isFainted() ? (double) (int) Math.Round(num3 * 0.5) : (double) (int) Math.Round(num3 * 0.66);
      if (skill >= 48 && opponent.pbOwnSide.LightScreen > (byte) 0 && move.pbIsSpecial(type))
        num3 = opponent.pbPartner.isFainted() ? (double) (int) Math.Round(num3 * 0.5) : (double) (int) Math.Round(num3 * 0.66);
      if (skill >= 100 && opponent.hasWorkingAbility(Abilities.MULTISCALE) && opponent.HP == opponent.TotalHP)
        num3 = (double) (int) Math.Round(num3 * 0.5);
      if (skill >= 100 && attacker.hasWorkingAbility(Abilities.TINTED_LENS) && (double) num4 < 8.0)
        num3 = (double) (int) Math.Round(num3 * 2.0);
      if (skill >= 100 && opponent.pbPartner.hasWorkingAbility(Abilities.FRIEND_GUARD))
        num3 = (double) (int) Math.Round(num3 * 0.75);
      if (skill >= 100 && (opponent.hasWorkingAbility(Abilities.SOLID_ROCK) || opponent.hasWorkingAbility(Abilities.FILTER)) && (double) num4 > 8.0)
        num3 = (double) (int) Math.Round(num3 * 0.75);
      if (attacker.hasWorkingItem(Items.METRONOME))
      {
        if (attacker.effects.Metronome > 4)
        {
          num3 = (double) (int) Math.Round(num3 * 2.0);
        }
        else
        {
          float num5 = (float) (1.0 + (double) attacker.effects.Metronome * 0.200000002980232);
          num3 = (double) (int) Math.Round(num3 * (double) num5);
        }
      }
      if (attacker.hasWorkingItem(Items.EXPERT_BELT) && (double) num4 > 8.0)
        num3 = (double) (int) Math.Round(num3 * 1.2);
      if (attacker.hasWorkingItem(Items.LIFE_ORB))
        num3 = (double) (int) Math.Round(num3 * 1.3);
      if ((double) num4 > 8.0 && skill >= 48 && (opponent.hasWorkingItem(Items.CHOPLE_BERRY) && type == Types.FIGHTING || opponent.hasWorkingItem(Items.COBA_BERRY) && type == Types.FLYING || opponent.hasWorkingItem(Items.KEBIA_BERRY) && type == Types.POISON || opponent.hasWorkingItem(Items.SHUCA_BERRY) && type == Types.GROUND || opponent.hasWorkingItem(Items.CHARTI_BERRY) && type == Types.ROCK || opponent.hasWorkingItem(Items.TANGA_BERRY) && type == Types.BUG || opponent.hasWorkingItem(Items.KASIB_BERRY) && type == Types.GHOST || opponent.hasWorkingItem(Items.BABIRI_BERRY) && type == Types.STEEL || opponent.hasWorkingItem(Items.OCCA_BERRY) && type == Types.FIRE || opponent.hasWorkingItem(Items.PASSHO_BERRY) && type == Types.WATER || opponent.hasWorkingItem(Items.RINDO_BERRY) && type == Types.GRASS || opponent.hasWorkingItem(Items.WACAN_BERRY) && type == Types.ELECTRIC || opponent.hasWorkingItem(Items.PAYAPA_BERRY) && type == Types.PSYCHIC || opponent.hasWorkingItem(Items.YACHE_BERRY) && type == Types.ICE || opponent.hasWorkingItem(Items.HABAN_BERRY) && type == Types.DRAGON || opponent.hasWorkingItem(Items.COLBUR_BERRY) && type == Types.DARK))
        num3 = (double) (int) Math.Round(num3 * 0.5);
      if (skill >= 48 && opponent.hasWorkingItem(Items.CHILAN_BERRY) && type == Types.NORMAL)
        num3 = (double) (int) Math.Round(num3 * 0.5);
      if (skill >= 32)
      {
        int num6 = 0 + attacker.effects.FocusEnergy;
        if (Kernal.MoveMetaData[move.id].CritRate > 0)
          ++num6;
        if (attacker is IBattlerShadowPokemon battlerShadowPokemon && battlerShadowPokemon.inHyperMode() && move.Type == Types.SHADOW)
          ++num6;
        if (attacker.Species == Pokemons.CHANSEY && attacker.hasWorkingItem(Items.LUCKY_PUNCH))
          num6 += 2;
        if (attacker.Species == Pokemons.FARFETCHD && attacker.hasWorkingItem(Items.STICK))
          num6 += 2;
        if (attacker.hasWorkingAbility(Abilities.SUPER_LUCK))
          ++num6;
        if (attacker.hasWorkingItem(Items.SCOPE_LENS))
          ++num6;
        if (attacker.hasWorkingItem(Items.RAZOR_CLAW))
          ++num6;
        if (num6 > 4)
          num6 = 4;
        basedamage += basedamage * 0.100000001490116 * (double) num6;
      }
      return (int) num3;
    }

    public int pbRoughAccuracy(IBattleMove move, IBattler attacker, IBattler opponent, int skill)
    {
      float num1 = 0.0f;
      int accuracy = move.Accuracy;
      if (skill >= 32 && this.pbWeather == Weather.SUNNYDAY && (move.Effect == PokemonUnity.Attack.Data.Effects.x099 || move.Effect == PokemonUnity.Attack.Data.Effects.x14E))
        num1 = 50f;
      int num2 = attacker.stages[6];
      if (opponent.hasWorkingAbility(Abilities.UNAWARE))
        num2 = 0;
      float num3 = num2 >= 0 ? (float) ((double) (num2 + 3) * 100.0 / 3.0) : 300f / (float) (3 - num2);
      int num4 = opponent.stages[6];
      if (this.field.Gravity > (byte) 0)
        num4 -= 2;
      if (num4 < -6)
        num4 = -6;
      if (opponent.effects.Foresight || opponent.effects.MiracleEye || move.Effect == PokemonUnity.Attack.Data.Effects.x130 || attacker.hasWorkingAbility(Abilities.UNAWARE))
        num4 = 0;
      float num5 = num4 >= 0 ? (float) ((double) (num4 + 3) * 100.0 / 3.0) : (float) (300.0 / (3.0 - (double) num4));
      float num6 = num3 * ((float) accuracy / num5);
      if (skill >= 32)
      {
        if (attacker.hasWorkingAbility(Abilities.COMPOUND_EYES))
          num6 *= 1.3f;
        if (attacker.hasWorkingAbility(Abilities.VICTORY_STAR))
          num6 *= 1.1f;
        if (skill >= 48)
        {
          IBattler pokemon = !this.doublebattle ? (IBattler) null : attacker.pbPartner;
          if (pokemon.IsNotNullOrNone() && pokemon.hasWorkingAbility(Abilities.VICTORY_STAR))
            num6 *= 1.1f;
        }
        if (attacker.effects.MicleBerry)
          num6 *= 1.2f;
        if (attacker.hasWorkingItem(Items.WIDE_LENS))
          num6 *= 1.1f;
        if (skill >= 48 && attacker.hasWorkingAbility(Abilities.HUSTLE) && move.basedamage > 0)
          num6 *= 0.8f;
        if (skill >= 100)
        {
          if (opponent.hasWorkingAbility(Abilities.WONDER_SKIN) && move.basedamage == 0 && attacker.pbIsOpposing(opponent.Index))
            num6 /= 2f;
          if (opponent.hasWorkingAbility(Abilities.TANGLED_FEET) && opponent.effects.Confusion > 0)
            num6 /= 1.2f;
          if (this.pbWeather == Weather.SANDSTORM && opponent.hasWorkingAbility(Abilities.SAND_VEIL))
            num6 /= 1.2f;
          if (this.pbWeather == Weather.HAIL && opponent.hasWorkingAbility(Abilities.SNOW_CLOAK))
            num6 /= 1.2f;
        }
        if (skill >= 48)
        {
          if (opponent.hasWorkingItem(Items.BRIGHT_POWDER))
            num6 /= 1.1f;
          if (opponent.hasWorkingItem(Items.LAX_INCENSE))
            num6 /= 1.1f;
        }
      }
      if ((double) num6 > 100.0)
        num6 = 100f;
      if (move.Accuracy == 0)
        num6 = 125f;
      if (move.Effect == PokemonUnity.Attack.Data.Effects.x17D)
        num6 = 125f;
      if (skill >= 32)
      {
        if (opponent.effects.LockOn > 0 && opponent.effects.LockOnPos == attacker.Index)
          num6 = 125f;
        if (skill >= 48 && (attacker.hasWorkingAbility(Abilities.NO_GUARD) || opponent.hasWorkingAbility(Abilities.NO_GUARD)))
          num6 = 125f;
        if (opponent.effects.Telekinesis > 0)
          num6 = 125f;
        switch (this.pbWeather)
        {
          case Weather.RAINDANCE:
            if (move.Effect == PokemonUnity.Attack.Data.Effects.x099 || move.Effect == PokemonUnity.Attack.Data.Effects.x14E)
            {
              num6 = 125f;
              break;
            }
            break;
          case Weather.HAIL:
            if (move.Effect == PokemonUnity.Attack.Data.Effects.x105)
            {
              num6 = 125f;
              break;
            }
            break;
        }
        if (move.Effect == PokemonUnity.Attack.Data.Effects.x027)
        {
          num6 = (float) (move.Accuracy + attacker.Level - opponent.Level);
          if (opponent.hasWorkingAbility(Abilities.STURDY))
            num6 = 0.0f;
          if (opponent.Level > attacker.Level)
            num6 = 0.0f;
        }
      }
      return (int) num6;
    }

    public void pbChooseMoves(int index)
    {
      IBattler battler = this.battlers[index];
      int[] scores = new int[4];
      int[] numArray = (int[]) null;
      List<int> intList1 = new List<int>();
      int maxValue = 0;
      int idxTarget = -1;
      int skill = 0;
      bool flag1 = (this.opponent == null || this.opponent.Length == 0) && this.pbIsOpposing(index);
      if (flag1)
      {
        for (int idxMove = 0; idxMove < 4; ++idxMove)
        {
          if (this.CanChooseMove(index, idxMove, false))
          {
            scores[idxMove] = 100;
            intList1.Add(idxMove);
            maxValue += 100;
          }
        }
      }
      else
      {
        skill = (int) Kernal.TrainerMetaData[this.pbGetOwner(battler.Index).trainertype].SkillLevel;
        IBattler opponent = battler.pbOppositeOpposing;
        if (this.doublebattle && !opponent.isFainted() && !opponent.pbPartner.isFainted())
        {
          IBattler pbPartner = opponent.pbPartner;
          List<int[]> source = new List<int[]>();
          numArray = new int[4]{ -1, -1, -1, -1 };
          for (int idxMove = 0; idxMove < 4; ++idxMove)
          {
            if (this.CanChooseMove(index, idxMove, false))
            {
              int num1 = this.pbGetMoveScore(battler.moves[idxMove], battler, opponent, skill);
              int num2 = this.pbGetMoveScore(battler.moves[idxMove], battler, pbPartner, skill);
              if (battler.moves[idxMove].Target == Targets.ALL_POKEMON || battler.moves[idxMove].Target == Targets.ALL_OTHER_POKEMON || battler.moves[idxMove].Target == Targets.ENTIRE_FIELD || battler.moves[idxMove].Target == Targets.USERS_FIELD)
              {
                if (battler.pbPartner.isFainted())
                {
                  num1 = num1;
                  num2 = num2;
                }
                else
                {
                  int moveScore = this.pbGetMoveScore(battler.moves[idxMove], battler, battler.pbPartner, skill);
                  if (moveScore >= 140)
                  {
                    num1 = 0;
                    num2 = 0;
                  }
                  else if (moveScore >= 100)
                  {
                    num1 = 0;
                    num2 = 0;
                  }
                  else if (moveScore >= 40)
                  {
                    num1 = num1;
                    num2 = num2;
                  }
                  else
                  {
                    num1 = num1;
                    num2 = num2;
                  }
                }
              }
              intList1.Add(idxMove);
              source.Add(new int[4]
              {
                idxMove * 2,
                idxMove,
                num1,
                opponent.Index
              });
              source.Add(new int[4]
              {
                idxMove * 2 + 1,
                idxMove,
                num2,
                pbPartner.Index
              });
            }
          }
          source.OrderBy<int[], int>((Func<int[], int>) (a => a[2])).ThenBy<int[], int>((Func<int[], int>) (b => b[0]));
          for (int index1 = 0; index1 < source.Count; ++index1)
          {
            int index2 = source[index1][1];
            int num = source[index1][2];
            if (num > 0 && (scores[index2] == 0 || scores[index2] == num && Core.Rand.Next(10) < 5 || scores[index2] != num && Core.Rand.Next(10) < 3))
            {
              scores[index2] = num;
              numArray[index2] = source[index1][3];
            }
          }
          for (int index3 = 0; index3 < 4; ++index3)
          {
            if (scores[index3] < 0)
              scores[index3] = 0;
            maxValue += scores[index3];
          }
        }
        else
        {
          if (this.doublebattle && opponent.isFainted())
            opponent = opponent.pbPartner;
          for (int idxMove = 0; idxMove < 4; ++idxMove)
          {
            if (this.CanChooseMove(index, idxMove, false))
            {
              scores[idxMove] = this.pbGetMoveScore(battler.moves[idxMove], battler, opponent, skill);
              intList1.Add(idxMove);
            }
            if (scores[idxMove] < 0)
              scores[idxMove] = 0;
            maxValue += scores[idxMove];
          }
        }
      }
      int num3 = 0;
      for (int index4 = 0; index4 < 4; ++index4)
      {
        if (scores[index4] > num3)
          num3 = scores[index4];
      }
      if (!flag1 && skill >= 32)
      {
        float num4 = skill >= 100 ? 1.5f : (skill >= 48 ? 2f : 3f);
        int num5 = skill >= 100 ? 5 : (skill >= 48 ? 10 : 15);
        for (int index5 = 0; index5 < scores.Length; ++index5)
        {
          if (scores[index5] > num5 && (double) scores[index5] * (double) num4 < (double) num3)
          {
            maxValue -= scores[index5] - num5;
            scores[index5] = num5;
          }
        }
        num3 = 0;
        for (int index6 = 0; index6 < 4; ++index6)
        {
          int num6 = scores[index6];
          if (scores[index6] > num3)
            num3 = scores[index6];
        }
      }
      string message = "[AI] #" + battler.ToString(false) + "'s moves: ";
      int num7 = 0;
      for (int index7 = 0; index7 < 4; ++index7)
      {
        if ((uint) battler.moves[index7].id > 0U)
        {
          if (num7 > 0)
            message += ", ";
          message = message + Game._INTL(battler.moves[index7].id.ToString(TextScripts.Name)) + "=" + scores[index7].ToString();
          ++num7;
        }
      }
      GameDebug.Log(message);
      if (!flag1 && num3 > 100 && this.pbStdDev(scores) >= 40 && (uint) Core.Rand.Next(10) > 0U)
      {
        List<int> intList2 = new List<int>();
        for (int index8 = 0; index8 < 4; ++index8)
        {
          if (battler.moves[index8].id != Moves.NONE && ((double) scores[index8] >= (double) num3 * 0.8 || scores[index8] >= 200))
          {
            intList2.Add(index8);
            if (scores[index8] == num3)
              intList2.Add(index8);
          }
        }
        if (intList2.Count > 0)
        {
          int idxMove = intList2[Core.Rand.Next(intList2.Count)];
          GameDebug.Log("[AI] Prefer #" + Game._INTL(battler.moves[idxMove].id.ToString(TextScripts.Name)));
          this.pbRegisterMove(index, idxMove, false);
          if (numArray != null)
            idxTarget = numArray[idxMove];
          if (!this.doublebattle || idxTarget < 0)
            return;
          this.pbRegisterTarget(index, idxTarget);
          return;
        }
      }
      if (!flag1 && battler.turncount >= 0)
      {
        bool flag2 = false;
        if ((num3 <= 20 && battler.turncount > 2 || num3 <= 30 && battler.turncount > 5) && Core.Rand.Next(10) < 8)
          flag2 = true;
        if (maxValue < 100 && battler.turncount > 1)
        {
          bool flag3 = true;
          int num8 = 0;
          for (int index9 = 0; index9 < 4; ++index9)
          {
            if ((uint) battler.moves[index9].id > 0U)
            {
              if (scores[index9] > 0 && battler.moves[index9].basedamage > 0)
                flag3 = false;
              ++num8;
            }
          }
          flag2 = flag3 && (uint) Core.Rand.Next(10) > 0U;
        }
        if (flag2 && this.pbEnemyShouldWithdrawEx(index, true))
        {
          GameDebug.Log("[AI] Switching due to terrible moves");
          GameDebug.Log(string.Format("{0},{1},{2},\r\n\t\t\t\t\t\t\t{3},\r\n\t\t\t\t\t\t\t{4}", (object) index, (object) this.choices[index].Action, (object) this.choices[index].Index, (object) this.pbCanChooseNonActive(index), (object) this.battlers[index].pbNonActivePokemonCount));
          return;
        }
      }
      if (num3 <= 0)
      {
        if (intList1.Count > 0)
          this.pbRegisterMove(index, intList1[Core.Rand.Next(intList1.Count)], false);
        else
          this.pbAutoChooseMove(index, true);
      }
      else
      {
        int num9 = Core.Rand.Next(maxValue);
        int num10 = 0;
        for (int idxMove = 0; idxMove < 4; ++idxMove)
        {
          if (scores[idxMove] > 0)
          {
            num10 += scores[idxMove];
            if (num9 < num10)
            {
              this.pbRegisterMove(index, idxMove, false);
              if (numArray != null)
              {
                idxTarget = numArray[idxMove];
                break;
              }
              break;
            }
          }
        }
      }
      if (this.choices[index].Move.IsNotNullOrNone())
        GameDebug.Log("[AI] Will use #" + this.choices[index].Move.Name);
      if (!this.doublebattle || idxTarget < 0)
        return;
      this.pbRegisterTarget(index, idxTarget);
    }

    public bool pbEnemyShouldMegaEvolve(int index) => this.pbCanMegaEvolve(index);

    public bool pbEnemyShouldUseItem(int index)
    {
      Items use = this.pbEnemyItemToUse(index);
      if (use <= Items.NONE)
        return false;
      this.pbRegisterItem(index, use, new int?());
      return true;
    }

    public bool pbEnemyItemAlreadyUsed(int index, Items item, Items[] items)
    {
      if (this.choices[1].Action == ChoiceAction.UseItem && (Items) this.choices[1].Index == item)
      {
        int num = 0;
        foreach (Items items1 in items)
        {
          if (items1 == item)
            ++num;
        }
        if (num <= 1)
          return true;
      }
      return false;
    }

    public Items pbEnemyItemToUse(int index)
    {
      if (!this.internalbattle)
        return Items.NONE;
      Items[] ownerItems = this.pbGetOwnerItems(index);
      if (ownerItems == null)
        return Items.NONE;
      IBattler battler = this.battlers[index];
      if (battler.isFainted() || battler.effects.Embargo > 0)
        return Items.NONE;
      bool flag = false;
      foreach (Items items in ownerItems)
      {
        if (!this.pbEnemyItemAlreadyUsed(index, items, ownerItems) && (items == Items.POTION || items == Items.SUPER_POTION || items == Items.HYPER_POTION || items == Items.MAX_POTION || items == Items.FULL_RESTORE))
          flag = true;
      }
      foreach (Items use in ownerItems)
      {
        if (!this.pbEnemyItemAlreadyUsed(index, use, ownerItems))
        {
          int num1;
          switch (use)
          {
            case Items.POTION:
            case Items.HYPER_POTION:
            case Items.SUPER_POTION:
              num1 = 1;
              break;
            case Items.FULL_RESTORE:
              if (battler.HP <= battler.TotalHP / 4 || battler.HP <= battler.TotalHP / 2 && Core.Rand.Next(10) < 3 || battler.HP <= battler.TotalHP * 2 / 3 && (battler.Status > Status.NONE || battler.effects.Confusion > 0) && Core.Rand.Next(10) < 3)
                return use;
              goto label_41;
            default:
              num1 = use == Items.MAX_POTION ? 1 : 0;
              break;
          }
          if (num1 != 0)
          {
            if (battler.HP <= battler.TotalHP / 4 || battler.HP <= battler.TotalHP / 2 && Core.Rand.Next(10) < 3)
              return use;
          }
          else
          {
            int num2;
            switch (use)
            {
              case Items.FULL_HEAL:
                if (!flag && (battler.Status > Status.NONE || battler.effects.Confusion > 0))
                  return use;
                goto label_41;
              case Items.X_ATTACK:
              case Items.X_DEFENSE:
              case Items.X_SPEED:
              case Items.X_SP_ATK:
              case Items.X_SP_DEF:
                num2 = 1;
                break;
              default:
                num2 = use == Items.X_ACCURACY ? 1 : 0;
                break;
            }
            if (num2 != 0)
            {
              Stats? nullable1 = new Stats?();
              if (use == Items.X_ATTACK)
                nullable1 = new Stats?(Stats.ATTACK);
              if (use == Items.X_DEFENSE)
                nullable1 = new Stats?(Stats.DEFENSE);
              if (use == Items.X_SPEED)
                nullable1 = new Stats?(Stats.SPEED);
              if (use == Items.X_SP_ATK)
                nullable1 = new Stats?(Stats.SPATK);
              if (use == Items.X_SP_DEF)
                nullable1 = new Stats?(Stats.SPDEF);
              if (use == Items.X_ACCURACY)
                nullable1 = new Stats?(Stats.ACCURACY);
              Stats? nullable2 = nullable1;
              Stats stats = Stats.HP;
              if (nullable2.GetValueOrDefault() > stats & nullable2.HasValue && battler is IBattlerEffect battlerEffect && !battlerEffect.pbTooHigh(nullable1.Value) && Core.Rand.Next(10) < 3 - battler.stages[(int) nullable1.Value])
                return use;
            }
          }
label_41:;
        }
      }
      return Items.NONE;
    }

    public virtual bool pbEnemyShouldWithdraw(int index) => this.pbEnemyShouldWithdrawEx(index, false);

    public bool pbEnemyShouldWithdrawEx(int index, bool alwaysSwitch)
    {
      if (this.opponent == null)
        return false;
      bool flag = alwaysSwitch;
      int idxMove1 = -1;
      Types type = Types.NONE;
      int skillLevel = (int) Kernal.TrainerMetaData[this.pbGetOwner(index).trainertype].SkillLevel;
      if (this.opponent != null && !flag && this.battlers[index].turncount > 0 && skillLevel >= 48)
      {
        IBattler battler = this.battlers[index].pbOppositeOpposing;
        if (battler.isFainted())
          battler = battler.pbPartner;
        if (!battler.isFainted() && battler.lastMoveUsed > Moves.NONE && Math.Abs(battler.Level - this.battlers[index].Level) <= 6)
        {
          MoveData moveData = Kernal.MoveData[battler.lastMoveUsed];
          float num1 = this.pbTypeModifier(moveData.Type, this.battlers[index], this.battlers[index]);
          type = moveData.Type;
          int? power1 = moveData.Power;
          int num2 = 70;
          if (power1.GetValueOrDefault() > num2 & power1.HasValue && (double) num1 > 8.0)
          {
            flag = Core.Rand.Next(100) < 30;
          }
          else
          {
            int? power2 = moveData.Power;
            int num3 = 50;
            if (power2.GetValueOrDefault() > num3 & power2.HasValue && (double) num1 > 8.0)
              flag = Core.Rand.Next(100) < 20;
          }
        }
      }
      if (!this.CanChooseMove(index, 0, false) && !this.CanChooseMove(index, 1, false) && !this.CanChooseMove(index, 2, false) && !this.CanChooseMove(index, 3, false) && this.battlers[index].turncount > 5)
        flag = true;
      if (skillLevel >= 48 && this.battlers[index].effects.PerishSong != 1)
      {
        for (int idxMove2 = 0; idxMove2 < 4; ++idxMove2)
        {
          IBattleMove move = this.battlers[index].moves[idxMove2];
          if (move.id != Moves.NONE && this.CanChooseMove(index, idxMove2, false) && move.Effect == PokemonUnity.Attack.Data.Effects.x080)
          {
            idxMove1 = idxMove2;
            break;
          }
        }
      }
      if (skillLevel >= 48 && this.battlers[index].Status == Status.POISON && this.battlers[index].StatusCount > 0)
      {
        float num = (float) (this.battlers[index].TotalHP / 16);
        if ((double) (num * (float) (this.battlers[index].effects.Toxic + 1)) >= (double) this.battlers[index].HP && (double) num < (double) this.battlers[index].HP && Core.Rand.Next(100) < 80)
          flag = true;
      }
      if (skillLevel >= 32 && this.battlers[index].effects.Encore > 0)
      {
        int num4 = 0;
        int num5 = 0;
        IBattler battler = this.battlers[index];
        int encoreIndex = this.battlers[index].effects.EncoreIndex;
        if (!battler.pbOpposing1.isFainted())
        {
          num4 += this.pbGetMoveScore(battler.moves[encoreIndex], battler, battler.pbOpposing1, skillLevel);
          ++num5;
        }
        if (!battler.pbOpposing2.isFainted())
        {
          num4 += this.pbGetMoveScore(battler.moves[encoreIndex], battler, battler.pbOpposing2, skillLevel);
          ++num5;
        }
        if (num5 > 0 && num4 / num5 <= 20 && Core.Rand.Next(10) < 8)
          flag = true;
      }
      if (skillLevel >= 48 && !this.doublebattle && !this.battlers[index].pbOppositeOpposing.isFainted())
      {
        IBattler oppositeOpposing = this.battlers[index].pbOppositeOpposing;
        if ((oppositeOpposing.effects.HyperBeam > 0 || oppositeOpposing.hasWorkingAbility(Abilities.TRUANT) && oppositeOpposing.effects.Truant) && Core.Rand.Next(100) < 80)
          flag = false;
      }
      if (this.rules["suddendeath"])
      {
        if (this.battlers[index].HP <= this.battlers[index].TotalHP / 4 && Core.Rand.Next(10) < 3 && this.battlers[index].turncount > 0)
          flag = true;
        else if (this.battlers[index].HP <= this.battlers[index].TotalHP / 2 && Core.Rand.Next(10) < 8 && this.battlers[index].turncount > 0)
          flag = true;
      }
      if (this.battlers[index].effects.PerishSong == 1)
        flag = true;
      if (flag)
      {
        List<int> source = new List<int>();
        IBattler[] array = ((IEnumerable<IBattler>) this.battlers).Where<IBattler>((Func<IBattler, bool>) (b => b.Index % 2 == index % 2)).ToArray<IBattler>();
        for (int i = 0; i < array.Length; i++)
        {
          if (this.pbCanSwitch(index, i, false, false))
          {
            if (this.battlers[index].effects.PerishSong != 1)
            {
              int spikes = (int) this.battlers[index].pbOwnSide.Spikes;
              if ((spikes == 1 && array[i].HP <= array[i].TotalHP / 8 || spikes == 2 && array[i].HP <= array[i].TotalHP / 6 || spikes == 3 && array[i].HP <= array[i].TotalHP / 4) && !array[i].pbHasType(Types.FLYING) && !array[i].hasWorkingAbility(Abilities.LEVITATE))
                continue;
            }
            if (type >= Types.NONE && (double) this.pbTypeModifier(type, this.battlers[index], this.battlers[index]) == 0.0)
            {
              int num = 65;
              if ((double) this.pbTypeModifier2(array[i], this.battlers[index].pbOppositeOpposing) > 8.0)
                num = 85;
              if (Core.Rand.Next(100) < num)
                source = source.Where<int>((Func<int, bool>) (x => x == i)).Concat<int>(source.Where<int>((Func<int, bool>) (x => x != i))).ToList<int>();
            }
            else if (type >= Types.NONE && (double) this.pbTypeModifier(type, this.battlers[index], this.battlers[index]) < 8.0)
            {
              int num = 40;
              if ((double) this.pbTypeModifier2(array[i], this.battlers[index].pbOppositeOpposing) > 8.0)
                num = 60;
              if (Core.Rand.Next(100) < num)
                source = source.Where<int>((Func<int, bool>) (x => x == i)).Concat<int>(source.Where<int>((Func<int, bool>) (x => x != i))).ToList<int>();
            }
            else
              source.Add(i);
          }
        }
        if (source.Count > 0)
          return idxMove1 != -1 && this.pbRegisterMove(index, idxMove1, false) || this.pbRegisterSwitch(index, source[0]);
      }
      return false;
    }

    public int pbDefaultChooseNewEnemy(int index, IPokemon[] party)
    {
      List<int> intList = new List<int>();
      for (int pkmnidxTo = 0; pkmnidxTo < party.Length - 1; ++pkmnidxTo)
      {
        if (this.pbCanSwitchLax(index, pkmnidxTo, false))
          intList.Add(pkmnidxTo);
      }
      return intList.Count > 0 ? this.pbChooseBestNewEnemy(index, party, intList.ToArray()) : -1;
    }

    public int pbChooseBestNewEnemy(int index, IPokemon[] party, int[] enemies)
    {
      if (enemies == null || enemies.Length == 0)
        return -1;
      if (Game.GameData.PokemonTemp == null)
        Game.GameData.PokemonTemp = new PokemonTemp().initialize();
      IBattler pokemon1 = this.battlers[index].pbOpposing1;
      IBattler pokemon2 = this.battlers[index].pbOpposing2;
      if (pokemon1.IsNotNullOrNone() && pokemon1.isFainted())
        pokemon1 = (IBattler) null;
      if (pokemon2.IsNotNullOrNone() && pokemon2.isFainted())
        pokemon2 = (IBattler) null;
      int num1 = -1;
      int num2 = 0;
      foreach (int enemy in enemies)
      {
        IPokemon pokemon3 = party[enemy];
        int num3 = 0;
        foreach (IMove move in pokemon3.moves)
        {
          if (move.id != Moves.NONE)
          {
            int? power = Kernal.MoveData[move.id].Power;
            int num4 = 0;
            if (!(power.GetValueOrDefault() == num4 & power.HasValue))
            {
              if (!pokemon1.IsNotNullOrNone())
                ;
              if (!pokemon2.IsNotNullOrNone())
                ;
            }
          }
        }
        if (num1 == -1 || num3 > num2)
        {
          num1 = enemy;
          num2 = num3;
        }
      }
      return num1;
    }

    public void pbDefaultChooseEnemyCommand(int index)
    {
      if (!this.CanShowFightMenu(index))
      {
        if (this.pbEnemyShouldUseItem(index) || this.pbEnemyShouldWithdraw(index))
          return;
        this.pbAutoChooseMove(index, true);
      }
      else
      {
        if (this.pbEnemyShouldUseItem(index) || this.pbEnemyShouldWithdraw(index) || this.pbAutoFightMenu(index))
          return;
        if (this.pbEnemyShouldMegaEvolve(index))
          this.pbRegisterMegaEvolution(index);
        this.pbChooseMoves(index);
      }
    }

    public bool pbDbgPlayerOnly(int idx) => this.pbOwnedByPlayer(idx);

    public int pbStdDev(int[] scores)
    {
      int num1 = 0;
      int num2 = 0;
      foreach (int score in scores)
      {
        num2 += score;
        ++num1;
      }
      if (num1 == 0)
        return 0;
      float num3 = (float) num2 / (float) num1;
      float num4 = 0.0f;
      for (int index = 0; index < scores.Length; ++index)
      {
        if (scores[index] > 0)
        {
          float num5 = (float) scores[index] - num3;
          num4 += num5 * num5;
        }
      }
      return (int) Math.Sqrt((double) num4 / (double) num1);
    }

    public IScene scene { get; protected set; }

    public BattleResults decision { get; set; }

    public bool internalbattle { get; set; }

    public bool doublebattle { get; set; }

    public bool cantescape { get; set; }

    public bool canLose { get; private set; }

    public bool shiftStyle { get; set; }

    public bool battlescene { get; set; }

    public bool debug { get; protected set; }

    public int debugupdate { get; protected set; }

    public ITrainer[] player { get; protected set; }

    public ITrainer[] opponent { get; protected set; }

    public IPokemon[] party1 { get; protected set; }

    public IPokemon[] party2 { get; protected set; }

    public IList<int> party1order { get; protected set; }

    public IList<int> party2order { get; protected set; }

    public bool fullparty1 { get; set; }

    public bool fullparty2 { get; set; }

    public IBattler[] battlers { get; protected set; }

    public Items[][] items { get; set; }

    public IEffectsSide[] sides { get; private set; }

    public IEffectsField field { get; private set; }

    public Environments environment { get; set; }

    public Weather weather { get; set; }

    public int weatherduration { get; set; }

    public bool switching { get; private set; }

    public bool futuresight { get; private set; }

    public IBattleMove struggle { get; private set; }

    public IBattleChoice[] choices { get; protected set; }

    public ISuccessState[] successStates { get; private set; }

    public Moves lastMoveUsed { get; set; }

    public int lastMoveUser { get; set; }

    public int[][] megaEvolution { get; private set; }

    public bool amuletcoin { get; protected set; }

    public int extramoney { get; set; }

    public bool doublemoney { get; set; }

    public string endspeech { get; set; }

    public string endspeech2 { get; set; }

    public string endspeechwin { get; set; }

    public string endspeechwin2 { get; set; }

    public IDictionary<string, bool> rules { get; private set; }

    public int turncount { get; set; }

    public IBattler[] priority { get; protected set; }

    public List<int> snaggedpokemon { get; private set; }

    public int runCommand { get; private set; }

    public int nextPickupUse
    {
      get
      {
        ++this.pickupUse;
        return this.pickupUse;
      }
    }

    public bool controlPlayer { get; set; }

    public bool usepriority { get; set; }

    public IBattlePeer peer { get; set; }

    public UnityBattle(IScene scene, IPokemon[] p1, IPokemon[] p2, ITrainer player, ITrainer opponent) => ((IBattle) this).initialize(scene, p1, p2, player, opponent);

    public UnityBattle(
      IScene scene,
      IPokemon[] p1,
      IPokemon[] p2,
      ITrainer[] player,
      ITrainer[] opponent)
    {
      ((IBattle) this).initialize(scene, p1, p2, player, opponent);
    }

    protected UnityBattle(SerializationInfo info, StreamingContext context)
    {
      this.scene = (IScene) info.GetValue(nameof (scene), typeof (IScene));
      this.decision = (BattleResults) info.GetValue(nameof (decision), typeof (BattleResults));
      this.internalbattle = (bool) info.GetValue(nameof (internalbattle), typeof (bool));
      this.doublebattle = (bool) info.GetValue(nameof (doublebattle), typeof (bool));
      this.cantescape = (bool) info.GetValue(nameof (cantescape), typeof (bool));
      this.canLose = (bool) info.GetValue(nameof (canLose), typeof (bool));
      this.shiftStyle = (bool) info.GetValue(nameof (shiftStyle), typeof (bool));
      this.battlescene = (bool) info.GetValue(nameof (battlescene), typeof (bool));
      this.debug = (bool) info.GetValue(nameof (debug), typeof (bool));
      this.debugupdate = (int) info.GetValue(nameof (debugupdate), typeof (int));
      this.player = (ITrainer[]) info.GetValue(nameof (player), typeof (ITrainer[]));
      this.opponent = (ITrainer[]) info.GetValue(nameof (opponent), typeof (ITrainer[]));
      this.party1 = (IPokemon[]) info.GetValue(nameof (party1), typeof (IPokemon[]));
      this.party2 = (IPokemon[]) info.GetValue(nameof (party2), typeof (IPokemon[]));
      this.party1order = (IList<int>) info.GetValue(nameof (party1order), typeof (IList<int>));
      this.party2order = (IList<int>) info.GetValue(nameof (party2order), typeof (IList<int>));
      this.fullparty1 = (bool) info.GetValue(nameof (fullparty1), typeof (bool));
      this.fullparty2 = (bool) info.GetValue(nameof (fullparty2), typeof (bool));
      this.battlers = (IBattler[]) info.GetValue(nameof (battlers), typeof (IBattler[]));
      this.items = (Items[][]) info.GetValue(nameof (items), typeof (Items[][]));
      this.sides = (IEffectsSide[]) info.GetValue(nameof (sides), typeof (IEffectsSide[]));
      this.field = (IEffectsField) info.GetValue(nameof (field), typeof (IEffectsField));
      this.environment = (Environments) info.GetValue(nameof (environment), typeof (Environments));
      this.weather = (Weather) info.GetValue(nameof (weather), typeof (Weather));
      this.weatherduration = (int) info.GetValue(nameof (weatherduration), typeof (int));
      this.switching = (bool) info.GetValue(nameof (switching), typeof (bool));
      this.futuresight = (bool) info.GetValue(nameof (futuresight), typeof (bool));
      this.struggle = (IBattleMove) info.GetValue(nameof (struggle), typeof (IBattleMove));
      this.choices = (IBattleChoice[]) info.GetValue(nameof (choices), typeof (IBattleChoice[]));
      this.successStates = (ISuccessState[]) info.GetValue(nameof (successStates), typeof (ISuccessState[]));
      this.lastMoveUsed = (Moves) info.GetValue(nameof (lastMoveUsed), typeof (Moves));
      this.lastMoveUser = (int) info.GetValue(nameof (lastMoveUser), typeof (int));
      this.megaEvolution = (int[][]) info.GetValue(nameof (megaEvolution), typeof (int[][]));
      this.amuletcoin = (bool) info.GetValue(nameof (amuletcoin), typeof (bool));
      this.extramoney = (int) info.GetValue(nameof (extramoney), typeof (int));
      this.doublemoney = (bool) info.GetValue(nameof (doublemoney), typeof (bool));
      this.endspeech = (string) info.GetValue(nameof (endspeech), typeof (string));
      this.endspeech2 = (string) info.GetValue(nameof (endspeech2), typeof (string));
      this.endspeechwin = (string) info.GetValue(nameof (endspeechwin), typeof (string));
      this.endspeechwin2 = (string) info.GetValue(nameof (endspeechwin2), typeof (string));
      this.rules = (IDictionary<string, bool>) info.GetValue(nameof (rules), typeof (IDictionary<string, bool>));
      this.turncount = (int) info.GetValue(nameof (turncount), typeof (int));
      this.priority = (IBattler[]) info.GetValue(nameof (priority), typeof (IBattler[]));
      this.snaggedpokemon = (List<int>) info.GetValue(nameof (snaggedpokemon), typeof (List<int>));
      this.runCommand = (int) info.GetValue(nameof (runCommand), typeof (int));
      this.pickupUse = (int) info.GetValue(nameof (pickupUse), typeof (int));
      this.controlPlayer = (bool) info.GetValue(nameof (controlPlayer), typeof (bool));
      this.usepriority = (bool) info.GetValue(nameof (usepriority), typeof (bool));
      this.peer = (IBattlePeer) info.GetValue(nameof (peer), typeof (IBattlePeer));
    }

    IBattle IBattle.initialize(
      IScene scene,
      IPokemon[] p1,
      IPokemon[] p2,
      ITrainer player,
      ITrainer opponent)
    {
      IScene scene1 = scene;
      IPokemon[] p1_1 = p1;
      IPokemon[] p2_1 = p2;
      ITrainer[] player1;
      if (player != null)
        player1 = new ITrainer[1]{ player };
      else
        player1 = (ITrainer[]) null;
      ITrainer[] opponent1;
      if (opponent != null)
        opponent1 = new ITrainer[1]{ opponent };
      else
        opponent1 = (ITrainer[]) null;
      return this.initialize(scene1, p1_1, p2_1, player1, opponent1, 4);
    }

    IBattle IBattle.initialize(
      IScene scene,
      IPokemon[] p1,
      IPokemon[] p2,
      ITrainer[] player,
      ITrainer[] opponent)
    {
      return this.initialize(scene, p1, p2, player, opponent, 4);
    }

    public IBattle initialize(
      IScene scene,
      IPokemon[] p1,
      IPokemon[] p2,
      ITrainer[] player,
      ITrainer[] opponent,
      int maxBattlers = 4)
    {
      this.player = player ?? new ITrainer[0];
      this.opponent = opponent ?? new ITrainer[0];
      if (p1.Length == 0)
      {
        GameDebug.LogError("Party 1 has no Pokémon.");
        return (IBattle) this;
      }
      if (p2.Length == 0)
      {
        GameDebug.LogError("Party 2 has no Pokémon.");
        return (IBattle) this;
      }
      if (p2.Length > 2 && this.opponent.Length == 0)
      {
        GameDebug.LogError("Wild battles with more than two Pokémon are not allowed.");
        return (IBattle) this;
      }
      this.scene = scene;
      this.decision = BattleResults.ABORTED;
      this.internalbattle = true;
      this.doublebattle = false;
      this.cantescape = false;
      this.shiftStyle = true;
      this.battlescene = true;
      this.debug = true;
      this.debugupdate = 0;
      this.party1 = p1;
      this.party2 = p2;
      this.party1order = (IList<int>) new List<int>();
      for (int index = 0; index < 12; ++index)
        this.party1order.Add(index);
      this.party2order = (IList<int>) new List<int>();
      for (int index = 0; index < 12; ++index)
        this.party2order.Add(index);
      this.fullparty1 = false;
      this.fullparty2 = false;
      this.battlers = (IBattler[]) new Pokemon[maxBattlers];
      this.items = new Items[this.opponent.Length][];
      for (int index = 0; index < this.opponent.Length; ++index)
        this.items[index] = new Items[0];
      this.sides = (IEffectsSide[]) new PokemonUnity.Combat.Effects.Side[2]
      {
        new PokemonUnity.Combat.Effects.Side(),
        new PokemonUnity.Combat.Effects.Side()
      };
      this.field = (IEffectsField) new PokemonUnity.Combat.Effects.Field();
      this.environment = Environments.None;
      this.weather = Weather.NONE;
      this.weatherduration = 0;
      this.switching = false;
      this.futuresight = false;
      this.choices = new IBattleChoice[4];
      this.successStates = (ISuccessState[]) new SuccessState[this.battlers.Length];
      for (int index = 0; index < this.battlers.Length; ++index)
        this.successStates[index] = (ISuccessState) new SuccessState();
      this.lastMoveUsed = Moves.NONE;
      this.lastMoveUser = -1;
      this.pickupUse = 0;
      this.megaEvolution = new int[2][]
      {
        new int[this.party1.Length],
        new int[this.party2.Length]
      };
      for (int index1 = 0; index1 < this.sides.Length; ++index1)
      {
        for (int index2 = 0; index2 < this.megaEvolution[index1].Length; ++index2)
          this.megaEvolution[index1][index2] = -1;
      }
      this.amuletcoin = false;
      this.extramoney = 0;
      this.doublemoney = false;
      this.endspeech = "";
      this.endspeech2 = "";
      this.endspeechwin = "";
      this.endspeechwin2 = "";
      this.rules = (IDictionary<string, bool>) new Dictionary<string, bool>();
      this.turncount = 0;
      this.peer = PokeBattle_BattlePeer.create();
      this.priority = (IBattler[]) new Pokemon[this.battlers.Length];
      this.snaggedpokemon = new List<int>();
      this.runCommand = 0;
      this.struggle = !Kernal.MoveData.Keys.Contains(Moves.STRUGGLE) ? new PokeBattle_Struggle().Initialize((IBattle) this, (IMove) new PokemonUnity.Attack.Move(Moves.STRUGGLE)) : Move.pbFromPBMove((IBattle) this, (IMove) new PokemonUnity.Attack.Move(Moves.STRUGGLE));
      for (int idx = 0; idx < this.battlers.Length; ++idx)
      {
        this.battlers[idx] = (IBattler) new Pokemon((IBattle) this, (int) (sbyte) idx);
        Debug.Log("Battler Initialization : #" + idx + " " + this.battlers[idx].Name);
      }
        
      foreach (IPokemon pokemon in this.party1)
      {
        if (pokemon.Species != Pokemons.NONE)
        {
          pokemon.itemRecycle = Items.NONE;
          pokemon.itemInitial = pokemon.Item;
          pokemon.belch = false;
        }
      }
      foreach (IPokemon pokemon in this.party2)
      {
        if (pokemon.Species != Pokemons.NONE)
        {
          pokemon.itemRecycle = Items.NONE;
          pokemon.itemInitial = pokemon.Item;
          pokemon.belch = false;
        }
      }
      return (IBattle) this;
    }

    public virtual int pbRandom(int index) => Core.Rand.Next(index);

    public virtual void pbAbort() => throw new BattleAbortedException("Battle aborted");

    public virtual void pbStorePokemon(IPokemon pokemon)
    {
      if (pokemon is IPokemonShadowPokemon pokemonShadowPokemon && !pokemonShadowPokemon.isShadow)
      {
        if (this.pbDisplayConfirm(Game._INTL("Would you like to give a nickname to {1}?", (object) Game._INTL(pokemon.Species.ToString(TextScripts.Name)))))
        {
          string nick = string.Empty;
          if (this.scene is IPokeBattle_Scene scene)
          {
            string helptext = Game._INTL("{1}'s nickname?", (object) Game._INTL(pokemon.Species.ToString(TextScripts.Name)));
            IPokemon pokemon1 = pokemon;
            nick = scene.pbNameEntry(helptext, pokemon1);
          }
          if (!string.IsNullOrEmpty(nick))
            (pokemon as PokemonUnity.Monster.Pokemon).SetNickname(nick);
        }
      }
      int box1 = this.peer.pbCurrentBox();
      int box2 = this.peer.pbStorePokemon(this.pbPlayer(), pokemon);
      if (box2 < 0)
        return;
      string storageCreator = this.peer.pbGetStorageCreator();
      string str1 = this.peer.pbBoxName(box1);
      string str2 = this.peer.pbBoxName(box2);
      if (box2 != box1)
      {
        if (storageCreator != null)
          this.pbDisplayPaused(Game._INTL("Box \"{1}\" on {2}'s PC was full.", (object) str1, (object) storageCreator));
        else
          this.pbDisplayPaused(Game._INTL("Box \"{1}\" on someone's PC was full.", (object) str1));
        this.pbDisplayPaused(Game._INTL("{1} was transferred to box \"{2}\".", (object) pokemon.Name, (object) str2));
      }
      else
      {
        if (storageCreator != null)
          this.pbDisplayPaused(Game._INTL("{1} was transferred to {2}'s PC.", (object) pokemon.Name, (object) storageCreator));
        else
          this.pbDisplayPaused(Game._INTL("{1} was transferred to someone's PC.", (object) pokemon.Name));
        this.pbDisplayPaused(Game._INTL("It was stored in box \"{1}\".", (object) str2));
      }
    }

    public virtual void pbThrowPokeball(
      int idxPokemon,
      Items ball,
      int? rareness = null,
      bool showplayer = false)
    {
      string str = Game._INTL(ball.ToString(TextScripts.Name));
      IBattler battler = !this.pbIsOpposing(idxPokemon) ? this.battlers[idxPokemon].pbOppositeOpposing : this.battlers[idxPokemon];
      if (battler.isFainted())
        battler = battler.pbPartner;
      this.pbDisplayBrief(Game._INTL("{1} threw one {2}!", (object) Game.GameData.Trainer.name, (object) str));
      if (battler.isFainted())
      {
        this.pbDisplay(Game._INTL("But there was no target..."));
      }
      else
      {
        int num1 = 0;
        bool flag = false;
        if (this.opponent.Length != 0 && (Game.GameData is IItemCheck gameData && !gameData.pbIsSnagBall(ball) || battler is IBattlerShadowPokemon battlerShadowPokemon && !battlerShadowPokemon.isShadow()))
        {
          if (this.scene is IPokeBattle_Scene scene)
            scene.pbThrowAndDeflect(ball, 1);
          this.pbDisplay(Game._INTL("The Trainer blocked the Ball!\nDon't be a thief!"));
        }
        else
        {
          IPokemon pokemon = battler.pokemon;
          Pokemons species = pokemon.Species;
          if (!rareness.HasValue)
            rareness = new int?((int) Kernal.PokemonData[battler.Species].Rarity);
          int totalHp = battler.TotalHP;
          int hp = battler.HP;
          rareness = new int?(BallHandlers.ModifyCatchRate(ball, rareness.Value, (IBattle) this, battler));
          int num2 = (int) Math.Floor((double) ((totalHp * 3 - hp * 2) * rareness.Value) / ((double) totalHp * 3.0));
          if (battler.Status == Status.SLEEP || battler.Status == Status.FROZEN)
            num2 = (int) Math.Floor((double) num2 * 2.5);
          else if ((uint) battler.Status > 0U)
            num2 = (int) Math.Floor((double) num2 * 1.5);
          int num3 = 0;
          if (Game.GameData.Trainer != null)
          {
            if (Game.GameData.Trainer.pokedexOwned() > 600)
              num3 = (int) Math.Floor((double) num2 * 2.5 / 6.0);
            else if (Game.GameData.Trainer.pokedexOwned() > 450)
              num3 = (int) Math.Floor((double) num2 * 2.0 / 6.0);
            else if (Game.GameData.Trainer.pokedexOwned() > 300)
              num3 = (int) Math.Floor((double) num2 * 1.5 / 6.0);
            else if (Game.GameData.Trainer.pokedexOwned() > 150)
              num3 = (int) Math.Floor((double) num2 * 1.0 / 6.0);
            else if (Game.GameData.Trainer.pokedexOwned() > 30)
              num3 = (int) Math.Floor((double) num2 * 0.5 / 6.0);
          }
          if (num2 > (int) byte.MaxValue || BallHandlers.IsUnconditional(ball, (IBattle) this, battler))
          {
            num1 = 4;
          }
          else
          {
            if (num2 < 1)
              num2 = 1;
            int num4 = (int) Math.Floor(65536.0 / Math.Pow((double) byte.MaxValue / (double) num2, 3.0 / 16.0));
            if (this.pbRandom(65536) < num4)
              ++num1;
            if (this.pbRandom(65536) < num4 && num1 == 1)
              ++num1;
            if (this.pbRandom(65536) < num4 && num1 == 2)
              ++num1;
            if (this.pbRandom(65536) < num4 && num1 == 3)
              ++num1;
          }
          GameDebug.Log(string.Format("[Threw Poké Ball] #{0}, #{1} shakes (4=capture)", (object) str, (object) num1));
          if (this.scene is IPokeBattle_Scene scene1)
          {
            int ball1 = (int) ball;
            int shakes = flag ? 1 : num1;
            int num5 = flag ? 1 : 0;
            int index = battler.Index;
            int num6 = showplayer ? 1 : 0;
            scene1.pbThrow((Items) ball1, shakes, num5 != 0, index, num6 != 0);
          }
          switch (num1)
          {
            case 0:
              this.pbDisplay(Game._INTL("Oh no! The Pokémon broke free!"));
              BallHandlers.OnFailCatch(ball, (IBattle) this, battler);
              break;
            case 1:
              this.pbDisplay(Game._INTL("Aww... It appeared to be caught!"));
              BallHandlers.OnFailCatch(ball, (IBattle) this, battler);
              break;
            case 2:
              this.pbDisplay(Game._INTL("Aargh! Almost had it!"));
              BallHandlers.OnFailCatch(ball, (IBattle) this, battler);
              break;
            case 3:
              this.pbDisplay(Game._INTL("Gah! It was so close, too!"));
              BallHandlers.OnFailCatch(ball, (IBattle) this, battler);
              break;
            case 4:
              this.pbDisplayBrief(Game._INTL("Gotcha! {1} was caught!", (object) pokemon.Name));
              if (this.scene is IPokeBattle_Scene scene2)
                scene2.pbThrowSuccess();
              if (Game.GameData is IItemCheck gameData1 && gameData1.pbIsSnagBall(ball) && (uint) this.opponent.Length > 0U)
              {
                this.pbRemoveFromParty(battler.Index, battler.pokemonIndex);
                battler.pbReset();
                battler.participants = (IList<int>) new List<int>();
              }
              else
                this.decision = BattleResults.CAPTURED;
              if (Game.GameData is IItemCheck gameData2 && gameData2.pbIsSnagBall(ball))
              {
                pokemon.ot = this.player[0].name;
                pokemon.trainerID = this.player[0].id;
              }
              BallHandlers.OnCatch(ball, (IBattle) this, pokemon);
              pokemon.ballUsed = ball;
              if (pokemon is IPokemonMegaEvolution pokemonMegaEvolution1)
                pokemonMegaEvolution1.makeUnmega();
              if (pokemon is IPokemonMegaEvolution pokemonMegaEvolution2)
                pokemonMegaEvolution2.makeUnprimal();
              pokemon.pbRecordFirstMoves();
              BallHandlers.OnCatch(ball, (IBattle) this, pokemon);
              if (Game.GameData.Trainer.pokedex && !this.player[0].hasOwned(species))
              {
                this.player[0].setOwned(species);
                this.pbDisplayPaused(Game._INTL("{1}'s data was added to the Pokédex.", (object) pokemon.Name));
                if (this.scene is IPokeBattle_Scene scene3)
                  scene3.pbShowPokedex(pokemon.Species);
              }
              if (this.scene is IPokeBattle_Scene scene4)
                scene4.pbHideCaptureBall();
              if (Game.GameData is IItemCheck gameData3 && gameData3.pbIsSnagBall(ball) && (uint) this.opponent.Length > 0U)
              {
                if (pokemon is IPokemonShadowPokemon pokemonShadowPokemon)
                  pokemonShadowPokemon.pbUpdateShadowMoves();
                this.snaggedpokemon.Add((int) (byte) battler.Index);
                break;
              }
              this.pbStorePokemon(pokemon);
              break;
          }
        }
      }
    }

    public virtual bool pbDoubleBattleAllowed()
    {
      if (!this.fullparty1 && this.party1.Length > 6 || !this.fullparty2 && this.party2.Length > 6)
        return false;
      ITrainer[] opponent = this.opponent;
      ITrainer[] player = this.player;
      if (opponent != null && opponent.Length == 0)
        return this.party2.Length != 1 && this.party2.Length == 2;
      if (opponent != null && (uint) opponent.Length > 0U && opponent.Length != 1 && opponent.Length != 2)
        return false;
      if ((uint) player.Length > 0U && player.Length != 1 && player.Length != 2)
        return false;
      if ((uint) opponent.Length > 0U)
      {
        int nextUnfainted1 = this.pbFindNextUnfainted(this.party2, 0, this.pbSecondPartyBegin(1));
        int nextUnfainted2 = this.pbFindNextUnfainted(this.party2, this.pbSecondPartyBegin(1), -1);
        if (nextUnfainted1 < 0 || nextUnfainted2 < 0)
          return false;
      }
      else
      {
        int nextUnfainted3 = this.pbFindNextUnfainted(this.party2, 0, -1);
        int nextUnfainted4 = this.pbFindNextUnfainted(this.party2, nextUnfainted3 + 1, -1);
        if (nextUnfainted3 < 0 || nextUnfainted4 < 0)
          return false;
      }
      if ((uint) player.Length > 0U)
      {
        int nextUnfainted5 = this.pbFindNextUnfainted(this.party1, 0, this.pbSecondPartyBegin(0));
        int nextUnfainted6 = this.pbFindNextUnfainted(this.party1, this.pbSecondPartyBegin(0), -1);
        if (nextUnfainted5 < 0 || nextUnfainted6 < 0)
          return false;
      }
      else
      {
        int nextUnfainted7 = this.pbFindNextUnfainted(this.party1, 0, -1);
        int nextUnfainted8 = this.pbFindNextUnfainted(this.party1, nextUnfainted7 + 1, -1);
        if (nextUnfainted7 < 0 || nextUnfainted8 < 0)
          return false;
      }
      return true;
    }

    public Weather pbWeather
    {
      get
      {
        for (int index = 0; index < this.battlers.Length; ++index)
        {
          if (this.battlers[index].hasWorkingAbility(Abilities.CLOUD_NINE) || this.battlers[index].hasWorkingAbility(Abilities.AIR_LOCK))
            return Weather.NONE;
        }
        return this.weather;
      }
    }

    public bool pbIsOpposing(int index) => index % 2 == 1;

    public bool pbOwnedByPlayer(int index) => !this.pbIsOpposing(index) && (this.player.Length == 0 || index != 2);

    public bool pbIsDoubleBattler(int index) => index >= 2;

    public string ToString(int battlerindex, int pokemonindex)
    {
      IPokemon[] pokemonArray = this.pbParty(battlerindex);
      return this.pbIsOpposing(battlerindex) ? (this.opponent != null ? Game._INTL("The foe {1}", (object) pokemonArray[pokemonindex].Name) : Game._INTL("The wild {1}", (object) pokemonArray[pokemonindex].Name)) : Game._INTL("{1}", (object) pokemonArray[pokemonindex].Name);
    }

    public bool pbIsUnlosableItem(IBattler pkmn, Items item)
    {
      if (Kernal.ItemData[item].IsLetter)
        return true;
      if (pkmn.effects.Transform)
        return false;
      if (pkmn.Ability == Abilities.MULTITYPE)
      {
        Items[] itemsArray = new Items[17]
        {
          Items.FIST_PLATE,
          Items.SKY_PLATE,
          Items.TOXIC_PLATE,
          Items.EARTH_PLATE,
          Items.STONE_PLATE,
          Items.INSECT_PLATE,
          Items.SPOOKY_PLATE,
          Items.IRON_PLATE,
          Items.FLAME_PLATE,
          Items.SPLASH_PLATE,
          Items.MEADOW_PLATE,
          Items.ZAP_PLATE,
          Items.MIND_PLATE,
          Items.ICICLE_PLATE,
          Items.DRACO_PLATE,
          Items.DREAD_PLATE,
          Items.PIXIE_PLATE
        };
        foreach (Items items in itemsArray)
        {
          if (item == items)
            return true;
        }
      }
      KeyValuePair<Pokemons, Items>[] keyValuePairArray = new KeyValuePair<Pokemons, Items>[54]
      {
        new KeyValuePair<Pokemons, Items>(Pokemons.GIRATINA, Items.GRISEOUS_ORB),
        new KeyValuePair<Pokemons, Items>(Pokemons.GENESECT, Items.BURN_DRIVE),
        new KeyValuePair<Pokemons, Items>(Pokemons.GENESECT, Items.CHILL_DRIVE),
        new KeyValuePair<Pokemons, Items>(Pokemons.GENESECT, Items.DOUSE_DRIVE),
        new KeyValuePair<Pokemons, Items>(Pokemons.GENESECT, Items.SHOCK_DRIVE),
        new KeyValuePair<Pokemons, Items>(Pokemons.ABOMASNOW, Items.ABOMASITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.ABSOL, Items.ABSOLITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.AERODACTYL, Items.AERODACTYLITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.AGGRON, Items.AGGRONITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.ALAKAZAM, Items.ALAKAZITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.ALTARIA, Items.ALTARIANITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.AMPHAROS, Items.AMPHAROSITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.AUDINO, Items.AUDINITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.BANETTE, Items.BANETTITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.BEEDRILL, Items.BEEDRILLITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.BLASTOISE, Items.BLASTOISINITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.BLAZIKEN, Items.BLAZIKENITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.CAMERUPT, Items.CAMERUPTITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.CHARIZARD, Items.CHARIZARDITE_X),
        new KeyValuePair<Pokemons, Items>(Pokemons.CHARIZARD, Items.CHARIZARDITE_Y),
        new KeyValuePair<Pokemons, Items>(Pokemons.DIANCIE, Items.DIANCITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.GALLADE, Items.GALLADITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.GARCHOMP, Items.GARCHOMPITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.GARDEVOIR, Items.GARDEVOIRITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.GENGAR, Items.GENGARITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.GLALIE, Items.GLALITITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.GYARADOS, Items.GYARADOSITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.HERACROSS, Items.HERACRONITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.HOUNDOOM, Items.HOUNDOOMINITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.KANGASKHAN, Items.KANGASKHANITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.LATIAS, Items.LATIASITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.LATIOS, Items.LATIOSITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.LOPUNNY, Items.LOPUNNITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.LUCARIO, Items.LUCARIONITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.MANECTRIC, Items.MANECTITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.MAWILE, Items.MAWILITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.MEDICHAM, Items.MEDICHAMITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.METAGROSS, Items.METAGROSSITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.MEWTWO, Items.MEWTWONITE_X),
        new KeyValuePair<Pokemons, Items>(Pokemons.MEWTWO, Items.MEWTWONITE_Y),
        new KeyValuePair<Pokemons, Items>(Pokemons.PIDGEOT, Items.PIDGEOTITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.PINSIR, Items.PINSIRITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.SABLEYE, Items.SABLENITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.SALAMENCE, Items.SALAMENCITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.SCEPTILE, Items.SCEPTILITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.SCIZOR, Items.SCIZORITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.SHARPEDO, Items.SHARPEDONITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.SLOWBRO, Items.SLOWBRONITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.STEELIX, Items.STEELIXITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.SWAMPERT, Items.SWAMPERTITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.TYRANITAR, Items.TYRANITARITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.VENUSAUR, Items.VENUSAURITE),
        new KeyValuePair<Pokemons, Items>(Pokemons.KYOGRE, Items.BLUE_ORB),
        new KeyValuePair<Pokemons, Items>(Pokemons.GROUDON, Items.RED_ORB)
      };
      foreach (KeyValuePair<Pokemons, Items> keyValuePair in keyValuePairArray)
      {
        if (pkmn.Species == keyValuePair.Key && item == keyValuePair.Value)
          return true;
      }
      return false;
    }

    public IBattler pbCheckGlobalAbility(Abilities a)
    {
      for (int index = 0; index < this.battlers.Length; ++index)
      {
        if (this.battlers[index].hasWorkingAbility(a))
          return this.battlers[index];
      }
      return (IBattler) null;
    }

    public ITrainer pbPlayer()
    {
      ITrainer[] player = this.player;
      return player != null && (uint) player.Length > 0U ? this.player[0] : (ITrainer) new Trainer(this.battlers[0].Name, TrainerTypes.WildPokemon);
    }

    public Items[] pbGetOwnerItems(int battlerIndex)
    {
      if (this.items == null)
        return new Items[0];
      if (!this.pbIsOpposing(battlerIndex))
        return new Items[0];
      return (uint) this.opponent.Length > 0U ? (battlerIndex == 1 ? this.items[0] : this.items[1]) : new Items[0];
    }

    public void pbSetSeen(IPokemon pokemon)
    {
      if (!Game.GameData.Trainer.pokedex || !pokemon.IsNotNullOrNone() || !this.internalbattle)
        return;
      this.pbPlayer().seen[pokemon.Species] = true;
      if (Game.GameData is IGameUtility gameData)
        gameData.pbSeenForm(pokemon);
    }

    public string pbGetMegaRingName(int battlerIndex)
    {
      if (this.pbBelongsToPlayer(battlerIndex))
      {
        foreach (Items items in Core.MEGARINGS)
        {
          if (Game.GameData.Bag.pbQuantity(items) > 0)
            return Game._INTL(items.ToString(TextScripts.Name));
        }
      }
      return this.pbGetOwner(battlerIndex).trainertype == TrainerTypes.BUGCATCHER ? Game._INTL("Mega Net") : Game._INTL("Mega Ring");
    }

    public bool pbHasMegaRing(int battlerIndex)
    {
      if (!this.pbBelongsToPlayer(battlerIndex))
        return true;
      foreach (Items items in Core.MEGARINGS)
      {
        if (Game.GameData.Bag.pbQuantity(items) > 0)
          return true;
      }
      return false;
    }

    public int PokemonCount(IPokemon[] party)
    {
      int num = 0;
      for (int index = 0; index < party.Length; ++index)
      {
        if (party[index].Species != Pokemons.NONE && party[index].HP > 0 && !party[index].isEgg)
          ++num;
      }
      return num;
    }

    public bool AllFainted(IPokemon[] party) => this.PokemonCount(party) == 0;

    public int MaxLevel(IPokemon[] party)
    {
      int num = 0;
      for (int index = 0; index < party.Length; ++index)
      {
        if (party[index].Species != Pokemons.NONE && num < party[index].Level)
          num = party[index].Level;
      }
      return num;
    }

    public int pbPokemonCount(IPokemon[] party)
    {
      int num = 0;
      foreach (IPokemon pokemon in party)
      {
        if (pokemon.IsNotNullOrNone() && pokemon.HP > 0 && !pokemon.isEgg)
          ++num;
      }
      return num;
    }

    public bool pbAllFainted(IPokemon[] party) => this.pbPokemonCount(party) == 0;

    public int pbMaxLevel(IPokemon[] party)
    {
      int num = 0;
      foreach (IPokemon pokemon in party)
      {
        if (pokemon.IsNotNullOrNone() && num < pokemon.Level)
          num = pokemon.Level;
      }
      return num;
    }

    public int pbMaxLevelFromIndex(int index)
    {
      IPokemon[] pokemonArray = this.pbParty(index);
      ITrainer[] trainerArray = this.pbIsOpposing(index) ? this.opponent : this.player;
      int num1 = 0;
      if ((uint) trainerArray.Length > 0U)
      {
        int num2 = 0;
        int num3 = this.pbSecondPartyBegin(index);
        if (this.pbIsDoubleBattler(index))
          num2 = num3;
        for (int index1 = num2; index1 < num2 + num3; ++index1)
        {
          if (pokemonArray[index1].IsNotNullOrNone() && num1 < pokemonArray[index1].Level)
            num1 = pokemonArray[index1].Level;
        }
      }
      else
      {
        foreach (IPokemon pokemon in pokemonArray)
        {
          if (pokemon.IsNotNullOrNone() && num1 < pokemon.Level)
            num1 = pokemon.Level;
        }
      }
      return num1;
    }

    public IPokemon[] pbParty(int index) => this.pbIsOpposing(index) ? this.party2 : this.party1;

    public IPokemon[] pbOpposingParty(int index) => this.pbIsOpposing(index) ? this.party1 : this.party2;

    public int pbSecondPartyBegin(int battlerIndex) => this.pbIsOpposing(battlerIndex) ? (this.fullparty2 ? (int) (Game.GameData as Game).Features.LimitPokemonPartySize : 3) : (this.fullparty1 ? (int) (Game.GameData as Game).Features.LimitPokemonPartySize : 3);

    public int pbPartyLength(int battlerIndex) => this.pbIsOpposing(battlerIndex) ? (this.opponent.Length != 0 ? this.pbSecondPartyBegin(battlerIndex) : (int) (Game.GameData as Game).Features.LimitPokemonPartySize) : (this.player.Length != 0 ? this.pbSecondPartyBegin(battlerIndex) : (int) (Game.GameData as Game).Features.LimitPokemonPartySize);

    public int pbFindNextUnfainted(IPokemon[] party, int start, int finish = -1)
    {
      if (finish < 0)
        finish = party.Length;
      for (int nextUnfainted = start; nextUnfainted < finish; ++nextUnfainted)
      {
        if (party[nextUnfainted].IsNotNullOrNone() && party[nextUnfainted].HP > 0 && !party[nextUnfainted].isEgg)
          return nextUnfainted;
      }
      return -1;
    }

    public int pbGetLastPokeInTeam(int index)
    {
      IPokemon[] pokemonArray = this.pbParty(index);
      int[] array = (!this.pbIsOpposing(index) ? (IEnumerable<int>) this.party1order : (IEnumerable<int>) this.party2order).ToArray<int>();
      int num1 = this.pbPartyLength(index);
      int num2 = this.pbGetOwnerIndex(index) * num1;
      int lastPokeInTeam = -1;
      for (int index1 = num2; index1 < num2 + num1 - 1; ++index1)
      {
        IPokemon pokemon = pokemonArray[array[index1]];
        if (pokemon.IsNotNullOrNone() && !pokemon.isEgg && pokemon.HP > 0)
          lastPokeInTeam = array[index1];
      }
      return lastPokeInTeam;
    }

    public IBattler pbFindPlayerBattler(int pkmnIndex)
    {
      IBattler playerBattler = (IBattler) null;
      for (int index = 0; index < this.battlers.Length; ++index)
      {
        if (!this.pbIsOpposing(index) && this.battlers[index].pokemonIndex == pkmnIndex)
        {
          playerBattler = this.battlers[index];
          break;
        }
      }
      return playerBattler;
    }

    public bool pbIsOwner(int battlerIndex, int partyIndex)
    {
      int num = this.pbSecondPartyBegin(battlerIndex);
      return !this.pbIsOpposing(battlerIndex) ? this.player == null || this.player.Length == 0 || (battlerIndex == 0 ? partyIndex < num : partyIndex >= num) : this.opponent == null || this.opponent.Length == 0 || (battlerIndex == 1 ? partyIndex < num : partyIndex >= num);
    }

    public ITrainer pbGetOwner(int battlerIndex) => this.pbIsOpposing(battlerIndex) ? ((uint) this.opponent.Length > 0U ? (battlerIndex == 1 ? this.opponent[0] : this.opponent[1]) : (ITrainer) new Trainer((string) null, TrainerTypes.WildPokemon)) : ((uint) this.player.Length > 0U ? (battlerIndex == 0 ? this.player[0] : this.player[1]) : (ITrainer) new Trainer((string) null, TrainerTypes.WildPokemon));

    public ITrainer pbGetOwnerPartner(int battlerIndex) => this.pbIsOpposing(battlerIndex) ? ((uint) this.opponent.Length > 0U ? (battlerIndex == 1 ? this.opponent[1] : this.opponent[0]) : (ITrainer) new Trainer((string) null, TrainerTypes.WildPokemon)) : ((uint) this.player.Length > 0U ? (battlerIndex == 0 ? this.player[1] : this.player[0]) : (ITrainer) new Trainer((string) null, TrainerTypes.WildPokemon));

    public int pbGetOwnerIndex(int battlerIndex) => this.pbIsOpposing(battlerIndex) ? (this.opponent.Length != 0 ? (battlerIndex == 1 ? 0 : 1) : 0) : (this.player.Length != 0 ? (battlerIndex == 0 ? 0 : 1) : 0);

    public bool pbBelongsToPlayer(int battlerIndex) => this.player.Length != 0 && this.player.Length > 1 ? battlerIndex == 0 : battlerIndex % 2 == 0;

    public ITrainer pbPartyGetOwner(int battlerIndex, int partyIndex)
    {
      int num = this.pbSecondPartyBegin(battlerIndex);
      return !this.pbIsOpposing(battlerIndex) ? (this.player == null || this.player.Length == 0 ? (ITrainer) new Trainer((string) null, TrainerTypes.WildPokemon) : (partyIndex < num ? this.player[0] : this.player[1])) : (this.opponent == null || this.opponent.Length == 0 ? (ITrainer) new Trainer((string) null, TrainerTypes.WildPokemon) : (partyIndex < num ? this.opponent[0] : this.opponent[1]));
    }

    public void pbAddToPlayerParty(IPokemon pokemon)
    {
      IPokemon[] pokemonArray = this.pbParty(0);
      for (int partyIndex = 0; partyIndex < pokemonArray.Length; ++partyIndex)
      {
        if (this.pbIsOwner(0, partyIndex) && !pokemonArray[partyIndex].IsNotNullOrNone())
          pokemonArray[partyIndex] = pokemon;
      }
    }

    public void pbRemoveFromParty(int battlerIndex, int partyIndex)
    {
      IPokemon[] Party = this.pbParty(battlerIndex);
      ITrainer[] trainerArray = this.pbIsOpposing(battlerIndex) ? this.opponent : this.player;
      int[] array = (this.pbIsOpposing(battlerIndex) ? (IEnumerable<int>) this.party2order : (IEnumerable<int>) this.party1order).ToArray<int>();
      int num = this.pbSecondPartyBegin(battlerIndex);
      Party[partyIndex] = (IPokemon) null;
      if (trainerArray == null || trainerArray.Length == 1)
      {
        Party.PackParty();
        for (int index = partyIndex; index < Party.Length + 1; ++index)
        {
          for (int battlerIndex1 = 0; battlerIndex1 < this.battlers.Length; ++battlerIndex1)
          {
            if (this.battlers[battlerIndex1].IsNotNullOrNone() && this.pbGetOwner(battlerIndex1) == trainerArray[0] && this.battlers[battlerIndex1].pokemonIndex == index)
            {
              --this.battlers[battlerIndex1].pokemonIndex;
              break;
            }
          }
        }
        for (int index = 0; index < array.Length; ++index)
          array[index] = index == partyIndex ? array.Length - 1 : array[index] - 1;
      }
      else if (partyIndex < num - 1)
      {
        for (int index = partyIndex; index < num; ++index)
          Party[index] = index < num - 1 ? Party[index + 1] : (IPokemon) null;
        for (int index = 0; index < array.Length; ++index)
        {
          if (array[index] < num)
            array[index] = index == partyIndex ? num - 1 : array[index] - 1;
        }
      }
      else
      {
        for (int index = partyIndex; index < num + this.pbPartyLength(battlerIndex); ++index)
          Party[index] = index < Party.Length - 1 ? Party[index + 1] : (IPokemon) null;
        for (int index = 0; index < array.Length; ++index)
        {
          if (array[index] >= num)
            array[index] = index == partyIndex ? num + this.pbPartyLength(battlerIndex) - 1 : array[index] - 1;
        }
      }
    }

    public bool CanShowCommands(int idxPokemon)
    {
      IBattler battler = this.battlers[idxPokemon];
      return !battler.isFainted() && battler.effects.TwoTurnAttack <= Moves.NONE && battler.effects.HyperBeam <= 0 && battler.effects.Rollout <= (byte) 0 && battler.effects.Outrage <= 0 && battler.effects.Uproar <= 0 && battler.effects.Bide <= 0;
    }

    public bool CanShowFightMenu(int idxPokemon)
    {
      IBattler battler = this.battlers[idxPokemon];
      return this.CanShowCommands(idxPokemon) && (this.CanChooseMove(idxPokemon, 0, false) || this.CanChooseMove(idxPokemon, 1, false) || this.CanChooseMove(idxPokemon, 2, false) || this.CanChooseMove(idxPokemon, 3, false)) && battler.effects.Encore <= 0;
    }

    public bool CanChooseMove(int idxPokemon, int idxMove, bool showMessages, bool sleeptalk = false)
    {
      IBattler battler = this.battlers[idxPokemon];
      IBattleMove move = battler.moves[idxMove];
      IBattler pbOpposing1 = battler.pbOpposing1;
      IBattler pokemon = (IBattler) null;
      if (move == null || move.id == Moves.NONE)
        return false;
      if (move.PP <= 0 && move.TotalPP > 0 && !sleeptalk)
      {
        if (showMessages)
          this.pbDisplayPaused(Game._INTL("There's no PP left for this move!"));
        return false;
      }
      if (battler.hasWorkingItem(Items.ASSAULT_VEST))
      {
        if (showMessages)
          this.pbDisplayPaused(Game._INTL("The effects of the {1} prevent status moves from being used!", (object) Game._INTL(battler.Item.ToString(TextScripts.Name))));
        return false;
      }
      if (battler.effects.ChoiceBand >= Moves.NONE && (battler.hasWorkingItem(Items.CHOICE_BAND) || battler.hasWorkingItem(Items.CHOICE_SPECS) || battler.hasWorkingItem(Items.CHOICE_SCARF)))
      {
        bool flag = false;
        for (int index = 0; index < battler.moves.Length; ++index)
        {
          if (battler.moves[index].id == battler.effects.ChoiceBand)
          {
            flag = true;
            break;
          }
        }
        if (flag && move.id != battler.effects.ChoiceBand)
        {
          if (showMessages)
            this.pbDisplayPaused(Game._INTL("{1} allows the use of only {2}!", (object) Game._INTL(battler.Item.ToString(TextScripts.Name)), (object) Game._INTL(battler.effects.ChoiceBand.ToString(TextScripts.Name))));
          return false;
        }
      }
      if (pbOpposing1.IsNotNullOrNone() && pbOpposing1.effects.Imprison && (move.id == pbOpposing1.moves[0].id || move.id == pbOpposing1.moves[1].id || move.id == pbOpposing1.moves[2].id || move.id == pbOpposing1.moves[3].id))
      {
        if (showMessages)
          this.pbDisplayPaused(Game._INTL("{1} can't use the sealed {2}!", (object) battler.ToString(false), (object) Game._INTL(move.id.ToString(TextScripts.Name))));
        GameDebug.Log("[CanChoose][#" + pbOpposing1.ToString(false) + " has: #" + Game._INTL(pbOpposing1.moves[0].id.ToString(TextScripts.Name)) + ", #" + Game._INTL(pbOpposing1.moves[1].id.ToString(TextScripts.Name)) + ", #" + Game._INTL(pbOpposing1.moves[2].id.ToString(TextScripts.Name)) + ", #" + Game._INTL(pbOpposing1.moves[3].id.ToString(TextScripts.Name)) + "]");
        return false;
      }
      if (pokemon.IsNotNullOrNone() && pokemon.effects.Imprison && (move.id == pokemon.moves[0].id || move.id == pokemon.moves[1].id || move.id == pokemon.moves[2].id || move.id == pokemon.moves[3].id))
      {
        if (showMessages)
          this.pbDisplayPaused(Game._INTL("{1} can't use the sealed {2}!", (object) battler.ToString(false), (object) Game._INTL(move.id.ToString(TextScripts.Name))));
        GameDebug.Log("[CanChoose][#" + pokemon.ToString(false) + " has: #" + Game._INTL(pokemon.moves[0].id.ToString(TextScripts.Name)) + ", #" + Game._INTL(pokemon.moves[1].id.ToString(TextScripts.Name)) + ", #" + Game._INTL(pokemon.moves[2].id.ToString(TextScripts.Name)) + ", #" + Game._INTL(pokemon.moves[3].id.ToString(TextScripts.Name)) + "]");
        return false;
      }
      if (battler.effects.Taunt > 0 && move.basedamage == 0)
      {
        if (showMessages)
          this.pbDisplayPaused(Game._INTL("{1} can't use {2} after the taunt!", (object) battler.ToString(false), (object) Game._INTL(move.id.ToString(TextScripts.Name))));
        return false;
      }
      if (battler.effects.Torment && move.id == battler.lastMoveUsed)
      {
        if (showMessages)
          this.pbDisplayPaused(Game._INTL("{1} can't use the same move twice in a row due to the torment!", (object) battler.ToString(false)));
        return false;
      }
      if (move.id == battler.effects.DisableMove && !sleeptalk)
      {
        if (showMessages)
          this.pbDisplayPaused(Game._INTL("{1}'s {2} is disabled!", (object) battler.ToString(false), (object) Game._INTL(move.id.ToString(TextScripts.Name))));
        return false;
      }
      if (move.Effect == PokemonUnity.Attack.Data.Effects.x153 && (battler.Species != Pokemons.NONE || !battler.pokemon.belch))
      {
        if (showMessages)
          this.pbDisplayPaused(Game._INTL("{1} hasn't eaten any held berry, so it can't possibly belch!", (object) battler.ToString(false)));
        return false;
      }
      return battler.effects.Encore <= 0 || idxMove == battler.effects.EncoreIndex;
    }

    public virtual void pbAutoChooseMove(int idxPokemon, bool showMessages = true)
    {
      IBattler battler = this.battlers[idxPokemon];
      if (battler.isFainted())
        this.choices[idxPokemon] = (IBattleChoice) new Choice();
      else if (battler.effects.Encore > 0 && this.CanChooseMove(idxPokemon, battler.effects.EncoreIndex, false))
      {
        GameDebug.Log("[Auto choosing Encore move] #" + Game._INTL(battler.moves[battler.effects.EncoreIndex].id.ToString(TextScripts.Name)));
        this.choices[idxPokemon] = (IBattleChoice) new Choice(ChoiceAction.UseMove, battler.effects.EncoreIndex, battler.moves[battler.effects.EncoreIndex]);
        if (!this.doublebattle)
          return;
        IBattleMove move = battler.moves[battler.effects.EncoreIndex];
        Targets targettype = battler.pbTarget(move);
        if (targettype == Targets.SELECTED_POKEMON || targettype == Targets.SELECTED_POKEMON_ME_FIRST)
        {
          int idxTarget = (this.scene as IPokeBattle_SceneNonInteractive).pbChooseTarget(idxPokemon, targettype);
          if (idxTarget >= 0)
            this.pbRegisterTarget(idxPokemon, idxTarget);
        }
        else if (targettype == Targets.USER_OR_ALLY)
        {
          int idxTarget = (this.scene as IPokeBattle_SceneNonInteractive).pbChooseTarget(idxPokemon, targettype);
          if (idxTarget >= 0 && (idxTarget & 1) == (idxPokemon & 1))
            this.pbRegisterTarget(idxPokemon, idxTarget);
        }
      }
      else
      {
        if (!this.pbIsOpposing(idxPokemon) && showMessages)
          this.pbDisplayPaused(Game._INTL("{1} has no moves left!", (object) battler.Name));
        this.choices[idxPokemon] = (IBattleChoice) new Choice(ChoiceAction.UseMove, -1, this.struggle);
      }
    }

    public virtual bool pbRegisterMove(int idxPokemon, int idxMove, bool showMessages = true)
    {
      IBattleMove move = this.battlers[idxPokemon].moves[idxMove];
      if (!this.CanChooseMove(idxPokemon, idxMove, showMessages))
        return false;
      this.choices[idxPokemon] = (IBattleChoice) new Choice(ChoiceAction.UseMove, idxMove, move);
      return true;
    }

    public bool pbChoseMove(int i, Moves move)
    {
      if (this.battlers[i].isFainted() || this.choices[i].Action != ChoiceAction.UseMove || this.choices[i].Index < 0)
        return false;
      int index = this.choices[i].Index;
      return this.battlers[i].moves[index].id == move;
    }

    public bool pbChoseMoveFunctionCode(int i, PokemonUnity.Attack.Data.Effects code)
    {
      if (this.battlers[i].isFainted() || this.choices[i].Action != ChoiceAction.UseMove || this.choices[i].Index < 0)
        return false;
      int index = this.choices[i].Index;
      return this.battlers[i].moves[index].Effect == code;
    }

    public virtual bool pbRegisterTarget(int idxPokemon, int idxTarget)
    {
      this.choices[idxPokemon] = (IBattleChoice) new Choice(ChoiceAction.UseMove, this.choices[idxPokemon].Index, this.choices[idxPokemon].Move, idxTarget);
      return true;
    }

    public IBattler[] pbPriority(bool ignorequickclaw = false, bool log = false)
    {
      if (this.usepriority)
        return this.priority;
      this.priority = (IBattler[]) new Pokemon[this.battlers.Length];
      int[] numArray1 = new int[this.battlers.Length];
      int[] numArray2 = new int[this.battlers.Length];
      bool[] flagArray1 = new bool[this.battlers.Length];
      bool[] flagArray2 = new bool[this.battlers.Length];
      int num1 = 0;
      int num2 = 0;
      List<int> intList = new List<int>();
      for (int index = 0; index < this.battlers.Length; ++index)
      {
        numArray1[index] = this.battlers[index].SPE;
        flagArray1[index] = false;
        flagArray2[index] = false;
        int num3;
        if (!ignorequickclaw)
        {
          IBattleChoice choice = this.choices[index];
          num3 = choice != null ? (choice.Action == ChoiceAction.UseMove ? 1 : 0) : 0;
        }
        else
          num3 = 0;
        if (num3 != 0)
        {
          if (!flagArray1[index] && this.battlers[index].hasWorkingItem(Items.CUSTAP_BERRY) && !this.battlers[index].pbOpposing1.hasWorkingAbility(Abilities.UNNERVE) && !this.battlers[index].pbOpposing2.hasWorkingAbility(Abilities.UNNERVE) && (this.battlers[index].hasWorkingAbility(Abilities.GLUTTONY) && this.battlers[index].HP <= (int) Math.Floor((double) this.battlers[index].TotalHP * 0.5) || this.battlers[index].HP <= (int) Math.Floor((double) this.battlers[index].TotalHP * 0.25)))
          {
            this.pbCommonAnimation("UseItem", this.battlers[index], (IBattler) null, 0);
            flagArray1[index] = true;
            this.pbDisplayBrief(Game._INTL("{1}'s {2} let it move first!", (object) this.battlers[index].ToString(false), (object) Game._INTL(this.battlers[index].Item.ToString(TextScripts.Name))));
            this.battlers[index].pbConsumeItem();
          }
          if (!flagArray1[index] && this.battlers[index].hasWorkingItem(Items.QUICK_CLAW) && this.pbRandom(10) < 2)
          {
            this.pbCommonAnimation("UseItem", this.battlers[index], (IBattler) null, 0);
            flagArray1[index] = true;
            this.pbDisplayBrief(Game._INTL("{1}'s {2} let it move first!", (object) this.battlers[index].ToString(false), (object) Game._INTL(this.battlers[index].Item.ToString(TextScripts.Name))));
          }
          if (!flagArray1[index] && (this.battlers[index].hasWorkingAbility(Abilities.STALL) || this.battlers[index].hasWorkingItem(Items.LAGGING_TAIL) || this.battlers[index].hasWorkingItem(Items.FULL_INCENSE)))
            flagArray2[index] = true;
        }
      }
      for (int index = 0; index < this.battlers.Length; ++index)
      {
        int num4 = 0;
        IBattleChoice choice = this.choices[index];
        if (choice != null && choice.Action == ChoiceAction.UseMove)
        {
          num4 = this.choices[index].Move.Priority;
          if (this.battlers[index].hasWorkingAbility(Abilities.PRANKSTER) && this.choices[index].Move.pbIsStatus)
            ++num4;
          if (this.battlers[index].hasWorkingAbility(Abilities.GALE_WINGS) && this.choices[index].Move.Type == Types.FLYING)
            ++num4;
        }
        numArray2[index] = num4;
        if (index == 0)
        {
          num1 = num4;
          num2 = num4;
        }
        else
        {
          if (num1 > num4)
            num1 = num4;
          if (num2 < num4)
            num2 = num4;
        }
      }
      int num5 = num2;
      while (true)
      {
        intList.Clear();
        for (int index = 0; index < this.battlers.Length; ++index)
        {
          if (numArray2[index] == num5)
            intList.Add(index);
        }
        if (intList.Count == 1)
          this.priority[this.priority.Length] = this.battlers[intList[0]];
        else if (intList.Count > 1)
        {
          int num6 = intList.Count - 1;
          for (int index1 = 0; index1 < intList.Count - 1; ++index1)
          {
            for (int index2 = 1; index2 < intList.Count; ++index2)
            {
              int num7 = 0;
              if (flagArray1[intList[index2]])
              {
                num7 = -1;
                if (flagArray1[intList[index2 - 1]])
                  num7 = numArray1[intList[index2]] != numArray1[intList[index2 - 1]] ? (numArray1[intList[index2]] > numArray1[intList[index2 - 1]] ? -1 : 1) : 0;
              }
              else if (flagArray1[intList[index2 - 1]])
                num7 = 1;
              else if (flagArray2[intList[index2]])
              {
                num7 = 1;
                if (flagArray2[intList[index2 - 1]])
                  num7 = numArray1[intList[index2]] != numArray1[intList[index2 - 1]] ? (numArray1[intList[index2]] > numArray1[intList[index2 - 1]] ? 1 : -1) : 0;
              }
              else if (flagArray2[intList[index2 - 1]])
                num7 = -1;
              else if (numArray1[intList[index2]] != numArray1[intList[index2 - 1]])
                num7 = this.field.TrickRoom <= (byte) 0 ? (numArray1[intList[index2]] > numArray1[intList[index2 - 1]] ? -1 : 1) : (numArray1[intList[index2]] > numArray1[intList[index2 - 1]] ? 1 : -1);
              if (num7 < 0 || num7 == 0 && this.pbRandom(2) == 0)
              {
                int num8 = intList[index2];
                intList[index2] = intList[index2 - 1];
                intList[index2 - 1] = num8;
              }
            }
          }
          int index3 = 0;
          foreach (int index4 in intList)
          {
            this.priority[index3] = this.battlers[index4];
            ++index3;
          }
        }
        --num5;
        if (num5 < num1)
          break;
      }
      if (log)
      {
        string message = "[Priority] ";
        bool lowercase = false;
        for (int index = 0; index < this.battlers.Length; ++index)
        {
          if (this.priority[index].IsNotNullOrNone() && !this.priority[index].isFainted())
          {
            if (lowercase)
              message += ", ";
            message += string.Format("#{0} (#{1})", (object) this.priority[index].ToString(lowercase), (object) this.priority[index].Index);
            lowercase = true;
          }
        }
        GameDebug.Log(message);
      }
      this.usepriority = true;
      return this.priority;
    }

    public virtual bool pbCanSwitchLax(int idxPokemon, int pkmnidxTo, bool showMessages)
    {
      if (pkmnidxTo >= 0)
      {
        IPokemon[] pokemonArray = this.pbParty(idxPokemon);
        if (pkmnidxTo >= pokemonArray.Length || !pokemonArray[pkmnidxTo].IsNotNullOrNone())
          return false;
        if (pokemonArray[pkmnidxTo].isEgg)
        {
          if (showMessages)
            this.pbDisplayPaused(Game._INTL("An Egg can't battle!"));
          return false;
        }
        if (!this.pbIsOwner(idxPokemon, pkmnidxTo))
        {
          ITrainer owner = this.pbPartyGetOwner(idxPokemon, pkmnidxTo);
          if (showMessages)
            this.pbDisplayPaused(Game._INTL("You can't switch {1}'s Pokémon with one of yours!", (object) owner.name));
          return false;
        }
        if (pokemonArray[pkmnidxTo].HP <= 0)
        {
          if (showMessages)
            this.pbDisplayPaused(Game._INTL("{1} has no energy left to battle!", (object) pokemonArray[pkmnidxTo].Name));
          return false;
        }
        if (this.battlers[idxPokemon].pokemonIndex == pkmnidxTo || this.battlers[idxPokemon].pbPartner.pokemonIndex == pkmnidxTo)
        {
          if (showMessages)
            this.pbDisplayPaused(Game._INTL("{1} is already in battle!", (object) pokemonArray[pkmnidxTo].Name));
          return false;
        }
      }
      return true;
    }

    public bool pbCanSwitch(int idxPokemon, int pkmnidxTo, bool showMessages, bool ignoremeanlook = false)
    {
      IBattler battler = this.battlers[idxPokemon];
      if (!this.pbCanSwitchLax(idxPokemon, pkmnidxTo, showMessages))
        return false;
      bool flag = this.pbIsOpposing(idxPokemon);
      IPokemon[] pokemonArray = this.pbParty(idxPokemon);
      for (int index = 0; index < this.battlers.Length; ++index)
      {
        if (flag == this.pbIsOpposing(index) && this.choices[index].Action == ChoiceAction.SwitchPokemon && this.choices[index].Index == pkmnidxTo)
        {
          if (showMessages)
            this.pbDisplayPaused(Game._INTL("{1} has already been selected.", (object) pokemonArray[pkmnidxTo].Name));
          return false;
        }
      }
      if (battler.hasWorkingItem(Items.SHED_SHELL))
        return true;
      if (battler.effects.MultiTurn > 0 || !ignoremeanlook && battler.effects.MeanLook >= 0)
      {
        if (showMessages)
          this.pbDisplayPaused(Game._INTL("{1} can't be switched out!", (object) battler.ToString(false)));
        return false;
      }
      if (this.field.FairyLock > (byte) 0)
      {
        if (showMessages)
          this.pbDisplayPaused(Game._INTL("{1} can't be switched out!", (object) battler.ToString(false)));
        return false;
      }
      if (battler.effects.Ingrain)
      {
        if (showMessages)
          this.pbDisplayPaused(Game._INTL("{1} can't be switched out!", (object) battler.ToString(false)));
        return false;
      }
      IBattler pbOpposing1 = battler.pbOpposing1;
      IBattler pbOpposing2 = battler.pbOpposing2;
      IBattler pokemon = (IBattler) null;
      if (battler.pbHasType(Types.STEEL))
      {
        if (pbOpposing1.hasWorkingAbility(Abilities.MAGNET_PULL))
          pokemon = pbOpposing1;
        if (pbOpposing2.hasWorkingAbility(Abilities.MAGNET_PULL))
          pokemon = pbOpposing2;
      }
      if (!battler.isAirborne())
      {
        if (pbOpposing1.hasWorkingAbility(Abilities.ARENA_TRAP))
          pokemon = pbOpposing1;
        if (pbOpposing2.hasWorkingAbility(Abilities.ARENA_TRAP))
          pokemon = pbOpposing2;
      }
      if (!battler.hasWorkingAbility(Abilities.SHADOW_TAG))
      {
        if (pbOpposing1.hasWorkingAbility(Abilities.SHADOW_TAG))
          pokemon = pbOpposing1;
        if (pbOpposing2.hasWorkingAbility(Abilities.SHADOW_TAG))
          pokemon = pbOpposing2;
      }
      if (!pokemon.IsNotNullOrNone())
        return true;
      string str = Game._INTL(pokemon.Ability.ToString(TextScripts.Name));
      if (showMessages)
        this.pbDisplayPaused(Game._INTL("{1}'s {2} prevents switching!", (object) pokemon.ToString(false), (object) str));
      return false;
    }

    public virtual bool pbRegisterSwitch(int idxPokemon, int idxOther)
    {
      if (!this.pbCanSwitch(idxPokemon, idxOther, false, false))
        return false;
      this.choices[idxPokemon] = (IBattleChoice) new Choice(ChoiceAction.SwitchPokemon, idxOther);
      int index = this.pbIsOpposing(idxPokemon) ? 1 : 0;
      int ownerIndex = this.pbGetOwnerIndex(idxPokemon);
      if (this.megaEvolution[index][ownerIndex] == idxPokemon)
        this.megaEvolution[index][ownerIndex] = -1;
      return true;
    }

    public bool pbCanChooseNonActive(int index)
    {
      IPokemon[] pokemonArray = this.pbParty(index);
      for (int pkmnidxTo = 0; pkmnidxTo < pokemonArray.Length; ++pkmnidxTo)
      {
        if (this.pbCanSwitchLax(index, pkmnidxTo, false))
          return true;
      }
      return false;
    }

    public virtual void pbSwitch(bool favorDraws = false)
    {
      if (!favorDraws)
      {
        if (this.decision > BattleResults.ABORTED)
          return;
      }
      else if (this.decision == BattleResults.DRAW)
        return;
      this.pbJudge();
      if (this.decision > BattleResults.ABORTED)
        return;
      int hp = this.battlers[0].HP;
      List<int> intList = new List<int>();
      for (int index = 0; index < this.battlers.Length; ++index)
      {
        if ((this.doublebattle || !this.pbIsDoubleBattler(index)) && (!this.battlers[index].IsNotNullOrNone() || this.battlers[index].isFainted()) && this.pbCanChooseNonActive(index))
        {
          if (!this.pbOwnedByPlayer(index))
          {
            if (!this.pbIsOpposing(index) || this.opponent.Length != 0 && this.pbIsOpposing(index))
            {
              int newpoke1 = this.pbSwitchInBetween(index, false, false);
              int newpokename1 = newpoke1;
              if (newpoke1 >= 0 && this.pbParty(index)[newpoke1].Ability == Abilities.ILLUSION)
                newpokename1 = this.pbGetLastPokeInTeam(index);
              ITrainer owner = this.pbGetOwner(index);
              if (!this.doublebattle && hp > 0 && this.shiftStyle && this.opponent.Length != 0 && this.internalbattle && this.pbCanChooseNonActive(0) && this.pbIsOpposing(index) && this.battlers[0].effects.Outrage == 0)
              {
                this.pbDisplayPaused(Game._INTL("{1} is about to send in {2}.", (object) owner.name, (object) this.pbParty(index)[newpokename1].Name));
                if (this.pbDisplayConfirm(Game._INTL("Will {1} change Pokémon?", (object) this.pbPlayer().name)))
                {
                  int newpoke2 = this.pbSwitchPlayer(0, true, true);
                  if (newpoke2 >= 0)
                  {
                    int newpokename2 = newpoke2;
                    if (this.party1[newpoke2].Ability == Abilities.ILLUSION)
                      newpokename2 = this.pbGetLastPokeInTeam(0);
                    this.pbDisplayBrief(Game._INTL("{1}, that's enough! Come back!", (object) this.battlers[0].Name));
                    this.pbRecallAndReplace(0, newpoke2, newpokename2, false, false);
                    intList.Add(0);
                  }
                }
              }
              this.pbRecallAndReplace(index, newpoke1, newpokename1, false, false);
              intList.Add(index);
            }
          }
          else if ((uint) this.opponent.Length > 0U)
          {
            int newpoke = this.pbSwitchInBetween(index, true, false);
            int newpokename = newpoke;
            if (this.party1[newpoke].Ability == Abilities.ILLUSION)
              newpokename = this.pbGetLastPokeInTeam(index);
            this.pbRecallAndReplace(index, newpoke, newpokename, false, false);
            intList.Add(index);
          }
          else if (this.pbDisplayConfirm(Game._INTL("Use next Pokémon?")) || this.pbRun(index, true) <= 0)
          {
            int newpoke = this.pbSwitchInBetween(index, true, false);
            int newpokename = newpoke;
            if (this.party1[newpoke].Ability == Abilities.ILLUSION)
              newpokename = this.pbGetLastPokeInTeam(index);
            this.pbRecallAndReplace(index, newpoke, newpokename, false, false);
            intList.Add(index);
          }
        }
      }
      if (intList.Count <= 0)
        return;
      this.priority = this.pbPriority(false, false);
      foreach (IBattler battler in this.priority)
      {
        if (intList.Contains(battler.Index))
          battler.pbAbilitiesOnSwitchIn(true);
      }
    }

    public void pbSendOut(int index, IPokemon pokemon)
    {
      this.pbSetSeen(pokemon);
      if (this.peer is IBattlePeerMultipleForms peer)
        peer.pbOnEnteringBattle((IBattle) this, pokemon);
      if (this.pbIsOpposing(index))
      {
        if (this.scene is IPokeBattle_Scene scene1)
          scene1.pbTrainerSendOut(index, pokemon);
      }
      else if (this.scene is IPokeBattle_Scene scene2)
        scene2.pbSendOut(index, pokemon);
      if (!(this.scene is IPokeBattle_Scene scene3))
        return;
      scene3.pbResetMoveIndex(index);
    }

    public void pbReplace(int index, int newpoke, bool batonpass = false)
    {
      IPokemon[] pokemonArray = this.pbParty(index);
      int pokemonIndex = this.battlers[index].pokemonIndex;
      this.battlers[index].pbInitialize(pokemonArray[newpoke], (int) (sbyte) newpoke, batonpass);
      int[] array = (!this.pbIsOpposing(index) ? (IEnumerable<int>) this.party1order : (IEnumerable<int>) this.party2order).ToArray<int>();
      int index1 = -1;
      int index2 = -1;
      for (int index3 = 0; index3 < array.Length; ++index3)
      {
        if (array[index3] == pokemonIndex)
          index1 = index3;
        if (array[index3] == newpoke)
          index2 = index3;
      }
      int num = array[index1];
      array[index1] = array[index2];
      array[index2] = num;
      this.pbSendOut(index, pokemonArray[newpoke]);
      this.pbSetSeen(pokemonArray[newpoke]);
    }

    public bool pbRecallAndReplace(
      int index,
      int newpoke,
      int newpokename = -1,
      bool batonpass = false,
      bool moldbreaker = false)
    {
      this.battlers[index].pbResetForm();
      if (!this.battlers[index].isFainted())
        (this.scene as IPokeBattle_DebugSceneNoGraphics).pbRecall(index);
      this.pbMessagesOnReplace(index, newpoke, newpokename);
      this.pbReplace(index, newpoke, batonpass);
      return this.pbOnActiveOne(this.battlers[index], false, moldbreaker);
    }

    public void pbMessagesOnReplace(int index, int newpoke, int newpokename = -1)
    {
      if (newpokename < 0)
        newpokename = newpoke;
      IPokemon[] pokemonArray = this.pbParty(index);
      if (this.pbOwnedByPlayer(index))
      {
        if (!pokemonArray[newpoke].IsNotNullOrNone())
        {
          GameDebug.Log(string.Format("[{0},{1},{2},pbMOR]", (object) index, (object) newpoke, (object) pokemonArray[newpoke]));
          for (int index1 = 0; index1 < pokemonArray.Length; ++index1)
            GameDebug.Log(string.Format("[{0},{1}]", (object) index1, (object) pokemonArray[index1].HP));
          GameDebug.LogError("BattleAbortedException");
          this.pbAbort();
        }
        IBattler oppositeOpposing = this.battlers[index].pbOppositeOpposing;
        if (oppositeOpposing.isFainted() || oppositeOpposing.HP == oppositeOpposing.TotalHP)
          this.pbDisplayBrief(Game._INTL("Go! {1}!", (object) pokemonArray[newpokename].Name));
        else if (oppositeOpposing.HP >= oppositeOpposing.TotalHP / 2)
          this.pbDisplayBrief(Game._INTL("Do it! {1}!", (object) pokemonArray[newpokename].Name));
        else if (oppositeOpposing.HP >= oppositeOpposing.TotalHP / 4)
          this.pbDisplayBrief(Game._INTL("Go for it, {1}!", (object) pokemonArray[newpokename].Name));
        else
          this.pbDisplayBrief(Game._INTL("Your opponent's weak!\nGet 'em, {1}!", (object) pokemonArray[newpokename].Name));
        GameDebug.Log(string.Format("[Send out Pokémon] Player sent out #{0} in position #{1}", (object) pokemonArray[newpokename].Name, (object) index));
      }
      else
      {
        if (!pokemonArray[newpoke].IsNotNullOrNone())
        {
          GameDebug.Log(string.Format("[{0},{1},{2},pbMOR]", (object) index, (object) newpoke, (object) pokemonArray[newpoke]));
          for (int index2 = 0; index2 < pokemonArray.Length; ++index2)
            GameDebug.Log(string.Format("[{0},{1}]", (object) index2, (object) pokemonArray[index2].HP));
          GameDebug.LogError("BattleAbortedException");
          this.pbAbort();
        }
        this.pbDisplayBrief(Game._INTL("{1} sent\r\nout {2}!", (object) this.pbGetOwner(index).name, (object) pokemonArray[newpokename].Name));
        GameDebug.Log(string.Format("[Send out Pokémon] Opponent sent out #{0} in position #{1}", (object) pokemonArray[newpokename].Name, (object) index));
      }
    }

    public virtual int pbSwitchInBetween(int index, bool lax, bool cancancel) => !this.pbOwnedByPlayer(index) ? (this.scene as IPokeBattle_SceneNonInteractive).pbChooseNewEnemy(index, this.pbParty(index)) : this.pbSwitchPlayer(index, lax, cancancel);

    public int pbSwitchPlayer(int index, bool lax, bool cancancel) => (this.scene as IPokeBattle_SceneNonInteractive).pbSwitch(index, lax, cancancel);

    bool IBattle.pbUseItemOnPokemon(
      Items item,
      int pkmnIndex,
      IBattler userPkmn,
      IHasDisplayMessage scene)
    {
      IPokemon pokemon = this.party1[pkmnIndex];
      IBattler battler = (IBattler) null;
      string name = this.pbGetOwner(userPkmn.Index).name;
      if (this.pbBelongsToPlayer(userPkmn.Index))
        name = this.pbGetOwner(userPkmn.Index).name;
      this.pbDisplayBrief(Game._INTL("{1} used the\r\n{2}.", (object) name, (object) Game._INTL(item.ToString(TextScripts.Name))));
      GameDebug.Log("[Use item] Player used #" + Game._INTL(item.ToString(TextScripts.Name)) + " on #" + pokemon.Name);
      bool flag = false;
      if (pokemon.isEgg)
      {
        this.pbDisplay(Game._INTL("But it had no effect!"));
      }
      else
      {
        for (int index = 0; index < this.battlers.Length; ++index)
        {
          if (!this.pbIsOpposing(index) && this.battlers[index].pokemonIndex == pkmnIndex)
            battler = this.battlers[index];
        }
        flag = ItemHandlers.triggerBattleUseOnPokemon(item, pokemon, battler, scene);
      }
      if (!flag && this.pbBelongsToPlayer(userPkmn.Index))
      {
        if (Game.GameData.Bag.pbCanStore(item))
          Game.GameData.Bag.pbStoreItem(item);
        else
          GameDebug.LogError(Game._INTL("Couldn't return unused item to Bag somehow."));
      }
      return flag;
    }

    public bool pbUseItemOnBattler(
      Items item,
      int index,
      IBattler userPkmn,
      IHasDisplayMessage scene)
    {
      GameDebug.Log("[Use item] Player used #" + Game._INTL(item.ToString(TextScripts.Name)) + " on #" + this.battlers[index].ToString(true));
      bool flag = ItemHandlers.triggerBattleUseOnBattler(item, this.battlers[index], scene);
      if (!flag && this.pbBelongsToPlayer(userPkmn.Index))
      {
        if (Game.GameData.Bag.pbCanStore(item))
          Game.GameData.Bag.pbStoreItem(item);
        else
          GameDebug.LogError(Game._INTL("Couldn't return unused item to Bag somehow."));
      }
      return flag;
    }

    public bool pbRegisterItem(int idxPokemon, Items idxItem, int? idxTarget = null)
    {
      if (idxTarget.HasValue && idxTarget.Value >= 0)
      {
        for (int index = 0; index < this.battlers.Length; ++index)
        {
          if (!this.battlers[index].pbIsOpposing(idxPokemon) && this.battlers[index].pokemonIndex == idxTarget.Value && this.battlers[index].effects.Embargo > 0)
          {
            this.pbDisplay(Game._INTL("Embargo's effect prevents the item's use on {1}!", (object) this.battlers[index].ToString(true)));
            if (this.pbBelongsToPlayer(this.battlers[index].Index))
            {
              if (Game.GameData.Bag.pbCanStore(idxItem))
                Game.GameData.Bag.pbStoreItem(idxItem);
              else
                GameDebug.LogError(Game._INTL("Couldn't return unused item to Bag somehow."));
            }
            return false;
          }
        }
      }
      if (ItemHandlers.hasUseInBattle(idxItem))
      {
        if (idxPokemon == 0)
        {
          if (ItemHandlers.triggerBattleUseOnBattler(idxItem, this.battlers[idxPokemon], (IHasDisplayMessage) this))
          {
            ItemHandlers.triggerUseInBattle(idxItem, this.battlers[idxPokemon], this);
            if (this.doublebattle)
              this.battlers[idxPokemon + 2].effects.SkipTurn = true;
          }
          else
          {
            if (Game.GameData.Bag.pbCanStore(idxItem))
              Game.GameData.Bag.pbStoreItem(idxItem);
            else
              GameDebug.LogError(Game._INTL("Couldn't return unusable item to Bag somehow."));
            return false;
          }
        }
        else
        {
          if (ItemHandlers.triggerBattleUseOnBattler(idxItem, this.battlers[idxPokemon], (IHasDisplayMessage) this))
            this.pbDisplay(Game._INTL("It's impossible to aim without being focused!"));
          return false;
        }
      }
      this.choices[idxPokemon] = (IBattleChoice) new Choice(ChoiceAction.UseItem, idxItem, idxTarget.Value);
      int index1 = this.pbIsOpposing(idxPokemon) ? 1 : 0;
      int ownerIndex = this.pbGetOwnerIndex(idxPokemon);
      if (this.megaEvolution[index1][ownerIndex] == idxPokemon)
        this.megaEvolution[index1][ownerIndex] = -1;
      return true;
    }

    public void pbEnemyUseItem(Items item, IBattler battler)
    {
      if (!this.internalbattle)
        return;
      Items[] ownerItems = this.pbGetOwnerItems(battler.Index);
      if (ownerItems == null)
        return;
      ITrainer owner = this.pbGetOwner(battler.Index);
      for (int index = 0; index < ownerItems.Length; ++index)
      {
        if (ownerItems[index] == item)
        {
          ownerItems[index] = Items.NONE;
          break;
        }
      }
      string str = Game._INTL(item.ToString(TextScripts.Name));
      this.pbDisplayBrief(Game._INTL("{1} used the\r\n{2}!", (object) owner.name, (object) str));
      GameDebug.Log("[Use item] Opponent used #" + str + " on #" + battler.ToString(true));
      switch (item)
      {
        case Items.POTION:
          battler.pbRecoverHP(20, true);
          this.pbDisplay(Game._INTL("{1}'s HP was restored.", (object) battler.ToString(false)));
          break;
        case Items.FULL_RESTORE:
          bool flag = battler.HP == battler.TotalHP;
          battler.pbRecoverHP(battler.TotalHP - battler.HP, true);
          battler.Status = Status.NONE;
          battler.StatusCount = 0;
          battler.effects.Confusion = 0;
          if (flag)
          {
            this.pbDisplay(Game._INTL("{1} became healthy!", (object) battler.ToString(false)));
            break;
          }
          this.pbDisplay(Game._INTL("{1}'s HP was restored.", (object) battler.ToString(false)));
          break;
        case Items.MAX_POTION:
          battler.pbRecoverHP(battler.TotalHP - battler.HP, true);
          this.pbDisplay(Game._INTL("{1}'s HP was restored.", (object) battler.ToString(false)));
          break;
        case Items.HYPER_POTION:
          battler.pbRecoverHP(200, true);
          this.pbDisplay(Game._INTL("{1}'s HP was restored.", (object) battler.ToString(false)));
          break;
        case Items.SUPER_POTION:
          battler.pbRecoverHP(50, true);
          this.pbDisplay(Game._INTL("{1}'s HP was restored.", (object) battler.ToString(false)));
          break;
        case Items.FULL_HEAL:
          battler.Status = Status.NONE;
          battler.StatusCount = 0;
          battler.effects.Confusion = 0;
          this.pbDisplay(Game._INTL("{1} became healthy!", (object) battler.ToString(false)));
          break;
        case Items.X_ATTACK:
          if (!(battler is IBattlerEffect battlerEffect1) || !battlerEffect1.pbCanIncreaseStatStage(Stats.ATTACK, battler))
            break;
          battlerEffect1.pbIncreaseStat(Stats.ATTACK, 1, battler, true);
          break;
        case Items.X_DEFENSE:
          if (!(battler is IBattlerEffect battlerEffect2) || !battlerEffect2.pbCanIncreaseStatStage(Stats.DEFENSE, battler))
            break;
          battlerEffect2.pbIncreaseStat(Stats.DEFENSE, 1, battler, true);
          break;
        case Items.X_SPEED:
          if (!(battler is IBattlerEffect battlerEffect3) || !battlerEffect3.pbCanIncreaseStatStage(Stats.SPEED, battler))
            break;
          battlerEffect3.pbIncreaseStat(Stats.SPEED, 1, battler, true);
          break;
        case Items.X_ACCURACY:
          if (battler is IBattlerEffect battlerEffect4 && battlerEffect4.pbCanIncreaseStatStage(Stats.ACCURACY, battler))
            battlerEffect4.pbIncreaseStat(Stats.ACCURACY, 1, battler, true);
          break;
        case Items.X_SP_ATK:
          if (!(battler is IBattlerEffect battlerEffect5) || !battlerEffect5.pbCanIncreaseStatStage(Stats.SPATK, battler))
            break;
          battlerEffect5.pbIncreaseStat(Stats.SPATK, 1, battler, true);
          break;
        case Items.X_SP_DEF:
          if (!(battler is IBattlerEffect battlerEffect6) || !battlerEffect6.pbCanIncreaseStatStage(Stats.SPDEF, battler))
            break;
          battlerEffect6.pbIncreaseStat(Stats.SPDEF, 1, battler, true);
          break;
      }
    }

    public bool pbCanRun(int idxPokemon)
    {
      if ((uint) this.opponent.Length > 0U || this.cantescape && !this.pbIsOpposing(idxPokemon))
        return false;
      IBattler battler = this.battlers[idxPokemon];
      return !battler.pbHasType(Types.GHOST) && false || battler.hasWorkingItem(Items.SMOKE_BALL) || battler.hasWorkingAbility(Abilities.RUN_AWAY) || this.pbCanSwitch(idxPokemon, -1, false, false);
    }

    public virtual int pbRun(int idxPokemon, bool duringBattle = false)
    {
      IBattler battler1 = this.battlers[idxPokemon];
      if (this.pbIsOpposing(idxPokemon))
      {
        if ((uint) this.opponent.Length > 0U)
          return 0;
        this.choices[idxPokemon] = (IBattleChoice) new Choice(ChoiceAction.Run);
        return -1;
      }
      if ((uint) this.opponent.Length > 0U)
      {
        if (this.debug && Input.press(0))
        {
          if (this.pbDisplayConfirm(Game._INTL("Treat this battle as a win?")))
          {
            this.decision = BattleResults.WON;
            return 1;
          }
          if (this.pbDisplayConfirm(Game._INTL("Treat this battle as a loss?")))
          {
            this.decision = BattleResults.LOST;
            return 1;
          }
        }
        else if (this.internalbattle)
          this.pbDisplayPaused(Game._INTL("No! There's no running from a Trainer battle!"));
        else if (this.pbDisplayConfirm(Game._INTL("Would you like to forfeit the match and quit now?")))
        {
          this.pbDisplay(Game._INTL("{1} forfeited the match!", (object) this.pbPlayer().name));
          this.decision = BattleResults.FORFEIT;
          return 1;
        }
        return 0;
      }
      if (this.debug && Input.press(0))
      {
        this.pbDisplayPaused(Game._INTL("Got away safely!"));
        this.decision = BattleResults.FORFEIT;
        return 1;
      }
      if (this.cantescape)
      {
        this.pbDisplayPaused(Game._INTL("Can't escape!"));
        return 0;
      }
      if (!battler1.pbHasType(Types.GHOST) && false)
      {
        this.pbDisplayPaused(Game._INTL("Got away safely!"));
        this.decision = BattleResults.FORFEIT;
        return 1;
      }
      if (battler1.hasWorkingAbility(Abilities.RUN_AWAY))
      {
        if (duringBattle)
          this.pbDisplayPaused(Game._INTL("Got away safely!"));
        else
          this.pbDisplayPaused(Game._INTL("{1} escaped using Run Away!", (object) battler1.ToString(false)));
        this.decision = BattleResults.FORFEIT;
        return 1;
      }
      if (battler1.hasWorkingItem(Items.SMOKE_BALL))
      {
        if (duringBattle)
          this.pbDisplayPaused(Game._INTL("Got away safely!"));
        else
          this.pbDisplayPaused(Game._INTL("{1} escaped using its {2}!", (object) battler1.ToString(false), (object) Game._INTL(battler1.Item.ToString(TextScripts.Name))));
        this.decision = BattleResults.FORFEIT;
        return 1;
      }
      if (!duringBattle && !this.pbCanSwitch(idxPokemon, -1, false, false))
      {
        this.pbDisplayPaused(Game._INTL("Can't escape!"));
        return 0;
      }
      int spe = this.battlers[idxPokemon].pokemon.SPE;
      IBattler battler2 = this.battlers[idxPokemon].pbOppositeOpposing;
      if (battler2.isFainted())
        battler2 = battler2.pbPartner;
      int num1;
      if (!battler2.isFainted())
      {
        int num2 = battler2.pokemon.SPE;
        if (spe > num2)
        {
          num1 = 256;
        }
        else
        {
          if (num2 <= 0)
            num2 = 1;
          num1 = spe * 128 / num2 + this.runCommand * 30 & (int) byte.MaxValue;
        }
      }
      else
        num1 = 256;
      int num3 = 1;
      if (this.pbRandom(256) < num1)
      {
        this.pbDisplayPaused(Game._INTL("Got away safely!"));
        this.decision = BattleResults.FORFEIT;
      }
      else
      {
        this.pbDisplayPaused(Game._INTL("Can't escape!"));
        num3 = -1;
      }
      if (!duringBattle)
        ++this.runCommand;
      return num3;
    }
    
    public virtual IEnumerator pbRunIE(int idxPokemon, bool duringBattle, System.Action<int> result)
    {
      IBattler battler1 = this.battlers[idxPokemon];
      if (this.pbIsOpposing(idxPokemon))
      {
        if ((uint) this.opponent.Length > 0U)
        {
          result(0);
          goto label_run;
        }
        this.choices[idxPokemon] = (IBattleChoice) new Choice(ChoiceAction.Run);
        result(-1);
        goto label_run;
      }
      if ((uint) this.opponent.Length > 0U)
      {
        if (this.debug && Input.press(0))
        {
          if (this.pbDisplayConfirm(Game._INTL("Treat this battle as a win?")))
          {
            this.decision = BattleResults.WON;
            result(1);
            goto label_run;
          }
          if (this.pbDisplayConfirm(Game._INTL("Treat this battle as a loss?")))
          {
            this.decision = BattleResults.LOST;
            result(1);
            goto label_run;
          }
        }
        else if (this.internalbattle)
          this.pbDisplayPaused(Game._INTL("No! There's no running from a Trainer battle!"));
        else if (this.pbDisplayConfirm(Game._INTL("Would you like to forfeit the match and quit now?")))
        {
          this.pbDisplay(Game._INTL("{1} forfeited the match!", (object) this.pbPlayer().name));
          this.decision = BattleResults.FORFEIT;
          result(1);
          goto label_run;
        }
        result(0);
        goto label_run;
      }
      if (this.debug && Input.press(0))
      {
        this.pbDisplayPaused(Game._INTL("Got away safely!"));
        this.decision = BattleResults.FORFEIT;
        result(1);
        goto label_run;
      }
      if (this.cantescape)
      {
        this.pbDisplayPaused(Game._INTL("Can't escape!"));
        result(0);
        goto label_run;
      }
      if (!battler1.pbHasType(Types.GHOST) && false)
      {
        this.pbDisplayPaused(Game._INTL("Got away safely!"));
        this.decision = BattleResults.FORFEIT;
        result(1);
        goto label_run;
      }
      if (battler1.hasWorkingAbility(Abilities.RUN_AWAY))
      {
        if (duringBattle)
          this.pbDisplayPaused(Game._INTL("Got away safely!"));
        else
          this.pbDisplayPaused(Game._INTL("{1} escaped using Run Away!", (object) battler1.ToString(false)));
        this.decision = BattleResults.FORFEIT;
        result(1);
        goto label_run;
      }
      if (battler1.hasWorkingItem(Items.SMOKE_BALL))
      {
        if (duringBattle)
          this.pbDisplayPaused(Game._INTL("Got away safely!"));
        else
          this.pbDisplayPaused(Game._INTL("{1} escaped using its {2}!", (object) battler1.ToString(false), (object) Game._INTL(battler1.Item.ToString(TextScripts.Name))));
        this.decision = BattleResults.FORFEIT;
        result(1);
        goto label_run;
      }
      if (!duringBattle && !this.pbCanSwitch(idxPokemon, -1, false, false))
      {
        this.pbDisplayPaused(Game._INTL("Can't escape!"));
        result(0);
        goto label_run;
      }
      int spe = this.battlers[idxPokemon].pokemon.SPE;
      IBattler battler2 = this.battlers[idxPokemon].pbOppositeOpposing;
      if (battler2.isFainted())
        battler2 = battler2.pbPartner;
      int num1;
      if (!battler2.isFainted())
      {
        int num2 = battler2.pokemon.SPE;
        if (spe > num2)
        {
          num1 = 256;
        }
        else
        {
          if (num2 <= 0)
            num2 = 1;
          num1 = spe * 128 / num2 + this.runCommand * 30 & (int) byte.MaxValue;
        }
      }
      else
        num1 = 256;
      int num3 = 1;
      if (this.pbRandom(256) < num1)
      {
        this.pbDisplayPaused(Game._INTL("Got away safely!"));
        this.decision = BattleResults.FORFEIT;
      }
      else
      {
        this.pbDisplayPaused(Game._INTL("Can't escape!"));
        num3 = -1;
      }
      if (!duringBattle)
        ++this.runCommand;
      result(num3); 
  label_run:
      yield return null;
    }

    public bool pbCanMegaEvolve(int index) => false;

    public void pbRegisterMegaEvolution(int index) => this.megaEvolution[this.pbIsOpposing(index) ? 1 : 0][this.pbGetOwnerIndex(index)] = (int) (sbyte) index;

    public void pbMegaEvolve(int index)
    {
      if (!this.battlers[index].IsNotNullOrNone() || !this.battlers[index].pokemon.IsNotNullOrNone() || !this.battlers[index].hasMega || this.battlers[index].isMega)
        return;
      string name = this.pbGetOwner(index).name;
      if (this.pbBelongsToPlayer(index))
        name = this.pbGetOwner(index).name;
      if (this.battlers[index].pokemon is IPokemonMegaEvolution pokemon1 && pokemon1.megaMessage() == 1)
        this.pbDisplay(Game._INTL("{1}'s fervent wish has reached {2}!", (object) name, (object) this.battlers[index].ToString(false)));
      else
        this.pbDisplay(Game._INTL("{1}'s {2} is reacting to {3}'s {4}!", (object) this.battlers[index].ToString(false), (object) Game._INTL(this.battlers[index].Item.ToString(TextScripts.Name)), (object) name, (object) this.pbGetMegaRingName(index)));
      this.pbCommonAnimation("MegaEvolution", this.battlers[index], (IBattler) null, 0);
      if (this.battlers[index].pokemon is IPokemonMegaEvolution pokemon2)
        pokemon2.makeMega();
      this.battlers[index].form = this.battlers[index].pokemon is IPokemonMultipleForms pokemon3 ? pokemon3.form : 0;
      this.battlers[index].pbUpdate(true);
      if (this.scene is IPokeBattle_Scene scene)
      {
        IBattler battler = this.battlers[index];
        int id = (int) (this.battlers[index] as Pokemon).Form.Id;
        scene.pbChangePokemon(battler, (Forms) id);
      }
      this.pbCommonAnimation("MegaEvolution2", this.battlers[index], (IBattler) null, 0);
      string str = this.battlers[index].pokemon.Name;
      if (string.IsNullOrEmpty(str))
        str = Game._INTL("Mega {1}", (object) Game._INTL(this.battlers[index].pokemon.Species.ToString(TextScripts.Name)));
      this.pbDisplay(Game._INTL("{1} has Mega Evolved into {2}!", (object) this.battlers[index].ToString(false), (object) str));
      GameDebug.Log("[Mega Evolution] #" + this.battlers[index].ToString(false) + " Mega Evolved");
      this.megaEvolution[this.pbIsOpposing(index) ? 1 : 0][this.pbGetOwnerIndex(index)] = -2;
    }

    public void pbPrimalReversion(int index)
    {
      if (!this.battlers[index].IsNotNullOrNone() || !this.battlers[index].pokemon.IsNotNullOrNone() || !this.battlers[index].hasPrimal || this.battlers[index].pokemon.Species != Pokemons.KYOGRE || this.battlers[index].pokemon.Species != Pokemons.GROUDON || this.battlers[index].isPrimal)
        return;
      if (this.battlers[index].pokemon.Species == Pokemons.KYOGRE)
        this.pbCommonAnimation("PrimalKyogre", this.battlers[index], (IBattler) null, 0);
      else if (this.battlers[index].pokemon.Species == Pokemons.GROUDON)
        this.pbCommonAnimation("PrimalGroudon", this.battlers[index], (IBattler) null, 0);
      if (this.battlers[index].pokemon is IPokemonMegaEvolution pokemon1)
        pokemon1.makePrimal();
      this.battlers[index].form = this.battlers[index].pokemon is IPokemonMultipleForms pokemon2 ? pokemon2.form : 0;
      this.battlers[index].pbUpdate(true);
      if (this.scene is IPokeBattle_Scene scene)
      {
        IBattler battler = this.battlers[index];
        int id = (int) (this.battlers[index] as Pokemon).Form.Id;
        scene.pbChangePokemon(battler, (Forms) id);
      }
      if (this.battlers[index].pokemon.Species == Pokemons.KYOGRE)
        this.pbCommonAnimation("PrimalKyogre2", this.battlers[index], (IBattler) null, 0);
      else if (this.battlers[index].pokemon.Species == Pokemons.GROUDON)
        this.pbCommonAnimation("PrimalGroudon2", this.battlers[index], (IBattler) null, 0);
      this.pbDisplay(Game._INTL("{1}'s Primal Reversion!\nIt reverted to its primal form!", (object) this.battlers[index].ToString(false)));
      GameDebug.Log("[Primal Reversion] #" + this.battlers[index].ToString(false) + " Primal Reverted");
    }

    public void pbCall(int index)
    {
      ITrainer owner = this.pbGetOwner(index);
      this.pbDisplay(Game._INTL("{1} called {2}!", (object) owner.name, (object) this.battlers[index].Name));
      this.pbDisplay(Game._INTL("{1}!", (object) this.battlers[index].Name));
      GameDebug.Log("[Call to Pokémon] #" + owner.name + " called to #" + this.battlers[index].ToString(true));
      if (this.battlers[index] is IBattlerShadowPokemon battler2 && battler2.isShadow())
      {
        IPokemonShadowPokemon pokemon = null;
        int num;
        if (battler2.inHyperMode())
        {
          pokemon = this.battlers[index].pokemon as IPokemonShadowPokemon;
          num = pokemon != null ? 1 : 0;
        }
        else
          num = 0;
        if (num != 0)
        {
          if (pokemon != null)
          {
            pokemon.hypermode = false;
            pokemon.adjustHeart(-300);
          }

          this.pbDisplay(Game._INTL("{1} came to its senses from the Trainer's call!", (object) this.battlers[index].ToString(false)));
        }
        else
          this.pbDisplay(Game._INTL("But nothing happened!"));
      }
      else if (this.battlers[index].Status != Status.SLEEP && this.battlers[index] is IBattlerEffect battler1 && battler1.pbCanIncreaseStatStage(Stats.ACCURACY, this.battlers[index]))
        battler1.pbIncreaseStat(Stats.ACCURACY, 1, this.battlers[index], true);
      else
        this.pbDisplay(Game._INTL("But nothing happened!"));
    }

    public virtual void pbGainEXP()
    {
      if (!this.internalbattle)
        return;
      bool flag1 = true;
      for (int index1 = 0; index1 < this.battlers.Length; ++index1)
      {
        if (!this.doublebattle && this.pbIsDoubleBattler(index1))
          this.battlers[index1].participants.Clear();
        else if (this.pbIsOpposing(index1) && this.battlers[index1].participants.Count > 0 && (this.battlers[index1].isFainted() || this.battlers[index1].captured))
        {
          bool haveexpall = Game.GameData.Bag.pbQuantity(Items.EXP_ALL) > 0;
          int partic = 0;
          int expshare = 0;
          foreach (int participant in (IEnumerable<int>) this.battlers[index1].participants)
          {
            if (this.party1[participant].IsNotNullOrNone() && this.pbIsOwner(0, participant) && this.party1[participant].HP > 0 && !this.party1[participant].isEgg)
              ++partic;
          }
          if (!haveexpall)
          {
            for (int partyIndex = 0; partyIndex < this.party1.Length; ++partyIndex)
            {
              if (this.party1[partyIndex].IsNotNullOrNone() && this.pbIsOwner(0, partyIndex) && this.party1[partyIndex].HP > 0 && !this.party1[partyIndex].isEgg && (this.party1[partyIndex].Item == Items.EXP_SHARE || this.party1[partyIndex].itemInitial == Items.EXP_SHARE))
                ++expshare;
            }
          }
          if (((partic > 0 ? 1 : (expshare > 0 ? 1 : 0)) | (haveexpall ? 1 : 0)) != 0)
          {
            if (this.opponent == null & flag1 && this.pbAllFainted(this.party2))
            {
              (this.scene as IPokeBattle_DebugSceneNoGraphics).pbWildBattleSuccess();
              flag1 = false;
            }
            for (int index2 = 0; index2 < this.party1.Length; ++index2)
            {
              if (this.party1[index2].IsNotNullOrNone() && this.pbIsOwner(0, index2) && this.party1[index2].HP > 0 && !this.party1[index2].isEgg && (this.party1[index2].Item == Items.EXP_SHARE || this.party1[index2].itemInitial == Items.EXP_SHARE || this.battlers[index1].participants.Contains((int) (byte) index2)))
                this.pbGainExpOne(index2, this.battlers[index1], partic, expshare, haveexpall, true);
            }
            if (haveexpall)
            {
              bool flag2 = true;
              for (int index3 = 0; index3 < this.party1.Length; ++index3)
              {
                if (this.party1[index3].IsNotNullOrNone() && this.pbIsOwner(0, index3) && this.party1[index3].HP > 0 && !this.party1[index3].isEgg && this.party1[index3].Item != Items.EXP_SHARE && this.party1[index3].itemInitial != Items.EXP_SHARE && !this.battlers[index1].participants.Contains((int) (byte) index3))
                {
                  if (flag2)
                    this.pbDisplayPaused(Game._INTL("The rest of your team gained Exp. Points thanks to the {1}!", (object) Game._INTL(Items.EXP_ALL.ToString(TextScripts.Name))));
                  flag2 = false;
                  this.pbGainExpOne(index3, this.battlers[index1], partic, expshare, haveexpall, false);
                }
              }
            }
          }
          this.battlers[index1].participants.Clear();
        }
      }
    }

    public void pbGainExpOne(
      int index,
      IBattler defeated,
      int partic,
      int expshare,
      bool haveexpall,
      bool showmessages = true)
    {
      IPokemon thispoke = this.party1[index];
      int level1 = defeated.Level;
      float baseExpYield = (float) Kernal.PokemonData[defeated.Species].BaseExpYield;
      int[] evYield = Kernal.PokemonData[defeated.Species].EVYield;
      int num1 = 0;
      for (int index1 = 0; index1 < 6; ++index1)
        num1 += (int) thispoke.EV[index1];
      for (int index2 = 0; index2 < 6; ++index2)
      {
        int num2 = evYield[index2];
        if (thispoke.Item == Items.MACHO_BRACE || thispoke.itemInitial == Items.MACHO_BRACE)
          num2 *= 2;
        switch (index2)
        {
          case 0:
            if (thispoke.Item == Items.POWER_WEIGHT || thispoke.itemInitial == Items.POWER_WEIGHT)
            {
              num2 += 4;
              break;
            }
            break;
          case 1:
            if (thispoke.Item == Items.POWER_BRACER || thispoke.itemInitial == Items.POWER_BRACER)
            {
              num2 += 4;
              break;
            }
            break;
          case 2:
            if (thispoke.Item == Items.POWER_BELT || thispoke.itemInitial == Items.POWER_BELT)
            {
              num2 += 4;
              break;
            }
            break;
          case 3:
            if (thispoke.Item == Items.POWER_ANKLET || thispoke.itemInitial == Items.POWER_ANKLET)
            {
              num2 += 4;
              break;
            }
            break;
          case 4:
            if (thispoke.Item == Items.POWER_LENS || thispoke.itemInitial == Items.POWER_LENS)
            {
              num2 += 4;
              break;
            }
            break;
          case 5:
            if (thispoke.Item == Items.POWER_BAND || thispoke.itemInitial == Items.POWER_BAND)
            {
              num2 += 4;
              break;
            }
            break;
        }
        bool? pokerusStage = thispoke.PokerusStage;
        bool flag = true;
        if (pokerusStage.GetValueOrDefault() == flag & pokerusStage.HasValue)
          num2 *= 2;
        if (num2 > 0)
        {
          if (num1 + num2 > 510)
            num2 -= num1 + num2 - 510;
          if ((int) thispoke.EV[index2] + num2 > 252)
            num2 -= (int) thispoke.EV[index2] + num2 - 252;
          thispoke.EV[index2] = (byte) ((uint) thispoke.EV[index2] + (uint) num2);
          if (thispoke.EV[index2] > (byte) 252)
          {
            GameDebug.LogWarning(string.Format("Single-stat EV limit #{0} exceeded.\r\nStat: #{1}  EV gain: #{2}  EVs: #{3}", (object) 252, (object) index2, (object) num2, (object) thispoke.EV.ToString()));
            thispoke.EV[index2] = (byte) 252;
          }
          num1 += num2;
          if (num1 > 510)
            GameDebug.LogWarning(string.Format("EV limit #{0} exceeded.\r\nTotal EVs: #{1} EV gain: #{2}  EVs: #{3}", (object) 510, (object) num1, (object) num2, (object) thispoke.EV.ToString()));
        }
      }
      (thispoke as PokemonUnity.Monster.Pokemon).GainEffort(defeated.Species);
      bool flag1 = defeated.participants.Contains((int) (byte) index);
      bool flag2 = thispoke.Item == Items.EXP_SHARE || thispoke.itemInitial == Items.EXP_SHARE;
      int num3 = 0;
      if (expshare > 0)
      {
        if (partic == 0)
        {
          int num4 = (int) Math.Floor((double) level1 * (double) baseExpYield);
          num3 = flag2 ? (int) Math.Floor((double) num4 / (double) expshare) : 0;
        }
        else
        {
          int num5 = (int) Math.Floor((double) level1 * (double) baseExpYield * 0.5);
          num3 = flag2 ? (int) Math.Floor((double) num5 / (double) partic) * (flag1 ? 1 : 0) + (int) Math.Floor((double) num5 / (double) expshare) : 0;
        }
      }
      else if (flag1)
        num3 = (int) Math.Floor((double) level1 * (double) baseExpYield / (double) partic);
      else if (haveexpall)
        num3 = (int) Math.Floor((double) level1 * (double) baseExpYield / 2.0);
      if (num3 <= 0)
        return;
      if ((uint) this.opponent.Length > 0U)
        num3 = (int) Math.Floor((double) (num3 * 3) * 0.5);
      int experienceGain = (int) Math.Floor((double) (int) Math.Floor((double) num3 / 5.0) * Math.Sqrt(Math.Pow(((double) (2 * level1) + 10.0) / ((double) (level1 + thispoke.Level) + 10.0), 5.0)));
      if (flag1 | flag2)
        ++experienceGain;
      bool flag3 = thispoke.isForeign(this.pbPlayer());
      if (flag3)
        experienceGain = (int) Math.Floor((double) (experienceGain * 3) / 2.0);
      if (thispoke.Item == Items.LUCKY_EGG || thispoke.itemInitial == Items.LUCKY_EGG)
        experienceGain = (int) Math.Floor((double) (experienceGain * 3) / 2.0);
      LevelingRate growthRate = thispoke.GrowthRate;
      Experience experience = new Experience(growthRate, thispoke.Exp);
      experience.AddExperience(experienceGain);
      int total = experience.Total;
      int num6 = total - thispoke.Exp;
      if (num6 <= 0)
        return;
      if (showmessages)
      {
        if (flag3)
          this.pbDisplayPaused(Game._INTL("{1} gained a boosted {2} Exp. Points!", (object) thispoke.Name, (object) num6.ToString()));
        else
          this.pbDisplayPaused(Game._INTL("{1} gained {2} Exp. Points!", (object) thispoke.Name, (object) num6.ToString()));
      }
      int levelFromExperience = (int) Experience.GetLevelFromExperience(growthRate, total);
      int level2 = thispoke.Level;
      if (levelFromExperience < level2)
      {
        string str = string.Format("#{0}: #{1}/#{2} | #{3}/#{4} | gain: #{5}", (object) thispoke.Name, (object) thispoke.Level, (object) levelFromExperience, (object) thispoke.Exp, (object) total, (object) num6);
        GameDebug.LogError(Game._INTL("The new level {1) is less than the Pokémon's\r\ncurrent level (2), which shouldn't happen.\r\n[Debug: {3}]", (object) levelFromExperience.ToString(), (object) level2.ToString(), (object) str));
      }
      else if (thispoke is IPokemonShadowPokemon pokemonShadowPokemon && pokemonShadowPokemon.isShadow)
      {
        pokemonShadowPokemon.savedexp += num6;
      }
      else
      {
        int tempexp1 = thispoke.Exp;
        IBattler playerBattler = this.pbFindPlayerBattler(index);
        while (true)
        {
          int startExperience1 = Experience.GetStartExperience(growthRate, level2);
          int startExperience2 = Experience.GetStartExperience(growthRate, level2 + 1);
          int tempexp2 = startExperience2 < total ? startExperience2 : total;
          thispoke.Exp = tempexp2;
          (this.scene as IPokeBattle_DebugSceneNoGraphics).pbEXPBar(playerBattler, thispoke, startExperience1, startExperience2, tempexp1, tempexp2);
          tempexp1 = tempexp2;
          ++level2;
          if (level2 <= levelFromExperience)
          {
            int totalHp = thispoke.TotalHP;
            int atk = thispoke.ATK;
            int def = thispoke.DEF;
            int spe = thispoke.SPE;
            int spa = thispoke.SPA;
            int spd = thispoke.SPD;
            if (playerBattler.IsNotNullOrNone() && this.internalbattle)
              (playerBattler.pokemon as PokemonUnity.Monster.Pokemon).ChangeHappiness(HappinessMethods.LEVELUP);
            thispoke.calcStats();
            if (playerBattler.IsNotNullOrNone())
              playerBattler.pbUpdate();
            this.scene.pbRefresh();
            this.pbDisplayPaused(Game._INTL("{1} grew to Level {2}!", (object) thispoke.Name, (object) level2.ToString()));
            (this.scene as IPokeBattle_DebugSceneNoGraphics).pbLevelUp(playerBattler, thispoke, totalHp, atk, def, spe, spa, spd);
            foreach (Moves move in thispoke.getMoveList(new LearnMethod?(LearnMethod.levelup)))
              this.pbLearnMove(index, move);
          }
          else
            break;
        }
        thispoke.calcStats();
        if (playerBattler.IsNotNullOrNone())
          playerBattler.pbUpdate();
        this.scene.pbRefresh();
      }
    }

    public void pbLearnMove(int pkmnIndex, Moves move)
    {
      IPokemon pokemon = this.party1[pkmnIndex];
      if (!pokemon.IsNotNullOrNone())
        return;
      string name = pokemon.Name;
      IBattler playerBattler = this.pbFindPlayerBattler(pkmnIndex);
      string str1 = Game._INTL(move.ToString(TextScripts.Name));
      for (int index = 0; index < pokemon.moves.Length; ++index)
      {
        if (pokemon.moves[index].id == move)
          return;
        if (pokemon.moves[index].id == Moves.NONE)
        {
          pokemon.moves[index] = (IMove) new PokemonUnity.Attack.Move(move);
          if (playerBattler.IsNotNullOrNone())
            playerBattler.moves[index] = Move.pbFromPBMove((IBattle) this, pokemon.moves[index]);
          this.pbDisplayPaused(Game._INTL("{1} learned {2}!", (object) name, (object) str1));
          GameDebug.Log("[Learn move] #" + name + " learned #" + str1);
          return;
        }
      }
      int index1;
      while (true)
      {
        this.pbDisplayPaused(Game._INTL("{1} is trying to learn {2}.", (object) name, (object) str1));
        this.pbDisplayPaused(Game._INTL("But {1} can't learn more than four moves.", (object) name));
        if (this.pbDisplayConfirm(Game._INTL("Delete a move to make room for {1}?", (object) str1)))
        {
          this.pbDisplayPaused(Game._INTL("Which move should be forgotten?"));
          index1 = (this.scene as IPokeBattle_DebugSceneNoGraphics).pbForgetMove(pokemon, move);
          if (index1 < 0)
          {
            if (this.pbDisplayConfirm(Game._INTL("Should {1} stop learning {2}?", (object) name, (object) str1)))
              goto label_15;
          }
          else
            break;
        }
        else if (this.pbDisplayConfirm(Game._INTL("Should {1} stop learning {2}?", (object) name, (object) str1)))
          goto label_17;
      }
      string str2 = Game._INTL(pokemon.moves[index1].id.ToString(TextScripts.Name));
      pokemon.moves[index1] = (IMove) new PokemonUnity.Attack.Move(move);
      if (playerBattler.IsNotNullOrNone())
        playerBattler.moves[index1] = Move.pbFromPBMove((IBattle) this, pokemon.moves[index1]);
      this.pbDisplayPaused(Game._INTL("1,  2, and... ... ..."));
      this.pbDisplayPaused(Game._INTL("Poof!"));
      this.pbDisplayPaused(Game._INTL("{1} forgot {2}.", (object) name, (object) str2));
      this.pbDisplayPaused(Game._INTL("And..."));
      this.pbDisplayPaused(Game._INTL("{1} learned {2}!", (object) name, (object) str1));
      GameDebug.Log("[Learn move] #" + name + " forgot #" + str2 + " and learned #" + str1);
      return;
label_15:
      this.pbDisplayPaused(Game._INTL("{1} did not learn {2}.", (object) name, (object) str1));
      return;
label_17:
      this.pbDisplayPaused(Game._INTL("{1} did not learn {2}.", (object) name, (object) str1));
    }

    public virtual void pbOnActiveAll()
    {
      for (int index = 0; index < this.battlers.Length; ++index)
      {
        if (this.pbIsOpposing(index))
          this.battlers[index].pbUpdateParticipants();
        if (!this.pbIsOpposing(index) && (this.battlers[index].Item == Items.AMULET_COIN || this.battlers[index].Item == Items.LUCK_INCENSE))
          this.amuletcoin = true;
      }
      for (int index = 0; index < this.battlers.Length; ++index)
      {
        if (!this.battlers[index].isFainted() && this.battlers[index] is IBattlerShadowPokemon battler && battler.isShadow() && this.pbIsOpposing(index))
        {
          this.pbCommonAnimation("Shadow", this.battlers[index], (IBattler) null, 0);
          this.pbDisplay(Game._INTL("Oh!\nA Shadow Pokémon!"));
        }
      }
      this.usepriority = false;
      foreach (IBattler battler in this.pbPriority(false, false))
        battler?.pbAbilitiesOnSwitchIn(true);
      for (int index = 0; index < this.battlers.Length; ++index)
      {
        if (!this.battlers[index].isFainted())
          this.battlers[index].pbCheckForm();
      }
    }

    public virtual bool pbOnActiveOne(IBattler pkmn, bool onlyabilities = false, bool moldbreaker = false)
    {
      if (pkmn.isFainted())
        return false;
      if (!onlyabilities)
      {
        for (int index = 0; index < this.battlers.Length; ++index)
        {
          if (this.pbIsOpposing(index))
            this.battlers[index].pbUpdateParticipants();
          if (!this.pbIsOpposing(index) && (this.battlers[index].Item == Items.AMULET_COIN || this.battlers[index].Item == Items.LUCK_INCENSE))
            this.amuletcoin = true;
        }
        if (pkmn is IPokemonShadowPokemon pokemonShadowPokemon && pokemonShadowPokemon.isShadow && this.pbIsOpposing(pkmn.Index))
        {
          this.pbCommonAnimation("Shadow", pkmn, (IBattler) null, 0);
          this.pbDisplay(Game._INTL("Oh!\nA Shadow Pokémon!"));
        }
        if (pkmn.effects.HealingWish)
        {
          GameDebug.Log("[Lingering effect triggered] #" + pkmn.ToString(false) + "'s Healing Wish");
          this.pbCommonAnimation("HealingWish", pkmn, (IBattler) null, 0);
          this.pbDisplayPaused(Game._INTL("The healing wish came true for {1}!", (object) pkmn.ToString(true)));
          pkmn.pbRecoverHP(pkmn.TotalHP, true);
          if (pkmn is IBattlerEffect battlerEffect)
            battlerEffect.pbCureStatus(false);
          pkmn.effects.HealingWish = false;
        }
        if (pkmn.effects.LunarDance)
        {
          GameDebug.Log("[Lingering effect triggered] #" + pkmn.ToString(false) + "'s Lunar Dance");
          this.pbCommonAnimation("LunarDance", pkmn, (IBattler) null, 0);
          this.pbDisplayPaused(Game._INTL("{1} became cloaked in mystical moonlight!", (object) pkmn.ToString(false)));
          pkmn.pbRecoverHP(pkmn.TotalHP, true);
          if (pkmn is IBattlerEffect battlerEffect)
            battlerEffect.pbCureStatus(false);
          for (int index = 0; index < pkmn.moves.Length; ++index)
            pkmn.moves[index].PP = (int) (byte) pkmn.moves[index].TotalPP;
          pkmn.effects.LunarDance = false;
        }
        if (pkmn.pbOwnSide.Spikes > (byte) 0 && !pkmn.isAirborne(moldbreaker) && !pkmn.hasWorkingAbility(Abilities.MAGIC_GUARD))
        {
          GameDebug.Log("[Entry hazard] #" + pkmn.ToString(false) + " triggered Spikes");
          float num = (float) new int[3]{ 8, 6, 4 }[(int) pkmn.pbOwnSide.Spikes - 1];
          (this.scene as IPokeBattle_DebugSceneNoGraphics).pbDamageAnimation(pkmn, TypeEffective.Ineffective);
          pkmn.pbReduceHP((int) Math.Floor((double) pkmn.TotalHP / (double) num));
          this.pbDisplayPaused(Game._INTL("{1} is hurt by the spikes!", (object) pkmn.ToString(false)));
        }
        if (pkmn.isFainted())
          pkmn.pbFaint();
        if (!pkmn.pbOwnSide.StealthRock || pkmn.isFainted() || pkmn.hasWorkingAbility(Abilities.MAGIC_GUARD))
          ;
        if (pkmn.isFainted())
          pkmn.pbFaint();
        if (pkmn.pbOwnSide.ToxicSpikes > (byte) 0 && !pkmn.isFainted() && !pkmn.isAirborne(moldbreaker))
        {
          if (pkmn.pbHasType(Types.POISON))
          {
            GameDebug.Log("[Entry hazard] #" + pkmn.ToString(false) + " absorbed Toxic Spikes");
            pkmn.pbOwnSide.ToxicSpikes = (byte) 0;
            this.pbDisplayPaused(Game._INTL("{1} absorbed the poison spikes!", (object) pkmn.ToString(false)));
          }
          else if (pkmn is IBattlerEffect battlerEffect1 && battlerEffect1.pbCanPoisonSpikes(moldbreaker))
          {
            GameDebug.Log("[Entry hazard] #" + pkmn.ToString(false) + " triggered Toxic Spikes");
            if (pkmn.pbOwnSide.ToxicSpikes == (byte) 2)
              battlerEffect1.pbPoison((IBattler) null, Game._INTL("{1} was badly poisoned by the poison spikes!", (object) pkmn.ToString(false)), true);
            else
              battlerEffect1.pbPoison((IBattler) null, Game._INTL("{1} was poisoned by the poison spikes!", (object) pkmn.ToString(false)));
          }
        }
        if (pkmn.pbOwnSide.StickyWeb && !pkmn.isFainted() && !pkmn.isAirborne(moldbreaker) && pkmn is IBattlerEffect battlerEffect2 && battlerEffect2.pbCanReduceStatStage(Stats.SPEED, moldbreaker: moldbreaker))
        {
          GameDebug.Log("[Entry hazard] #" + pkmn.ToString(false) + " triggered Sticky Web");
          this.pbDisplayPaused(Game._INTL("{1} was caught in a sticky web!", (object) pkmn.ToString(false)));
          battlerEffect2.pbReduceStat(Stats.SPEED, 1, (IBattler) null, false, moldbreaker: moldbreaker);
        }
      }
      pkmn.pbAbilityCureCheck();
      if (pkmn.isFainted())
      {
        this.pbGainEXP();
        this.pbJudge();
        return false;
      }
      pkmn.pbAbilitiesOnSwitchIn(true);
      if (!onlyabilities)
      {
        pkmn.pbCheckForm();
        pkmn.pbBerryCureCheck();
      }
      return true;
    }

    public void pbPrimordialWeather()
    {
      bool flag1 = false;
      bool flag2;
      switch (this.weather)
      {
        case Weather.HEAVYRAIN:
          for (int index = 0; index < this.battlers.Length; ++index)
          {
            if (this.battlers[index].Ability == Abilities.PRIMORDIAL_SEA && !this.battlers[index].isFainted())
            {
              flag2 = true;
              break;
            }
            if (!flag1)
            {
              this.weather = Weather.NONE;
              this.pbDisplayBrief("The heavy rain has lifted!");
            }
          }
          break;
        case Weather.HARSHSUN:
          for (int index = 0; index < this.battlers.Length; ++index)
          {
            if (this.battlers[index].Ability == Abilities.DESOLATE_LAND && !this.battlers[index].isFainted())
            {
              flag2 = true;
              break;
            }
            if (!flag1)
            {
              this.weather = Weather.NONE;
              this.pbDisplayBrief("The harsh sunlight faded!");
            }
          }
          break;
        case Weather.STRONGWINDS:
          for (int index = 0; index < this.battlers.Length; ++index)
          {
            if (this.battlers[index].Ability == Abilities.DELTA_STREAM && !this.battlers[index].isFainted())
            {
              flag2 = true;
              break;
            }
            if (!flag1)
            {
              this.weather = Weather.NONE;
              this.pbDisplayBrief("The mysterious air current has dissipated!");
            }
          }
          break;
      }
    }

    private void _pbJudgeCheckpoint(IBattler attacker, IBattleMove move = null)
    {
    }

    public BattleResults pbDecisionOnTime()
    {
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      int num4 = 0;
      foreach (IPokemon pokemon in this.party1)
      {
        if (pokemon.IsNotNullOrNone() && pokemon.HP > 0 && !pokemon.isEgg)
        {
          ++num1;
          num3 += pokemon.HP;
        }
      }
      foreach (IPokemon pokemon in this.party2)
      {
        if (pokemon.IsNotNullOrNone() && pokemon.HP > 0 && !pokemon.isEgg)
        {
          ++num2;
          num4 += pokemon.HP;
        }
      }
      if (num1 > num2)
        return BattleResults.WON;
      if (num1 < num2)
        return BattleResults.LOST;
      if (num3 > num4)
        return BattleResults.WON;
      return num3 < num4 ? BattleResults.LOST : BattleResults.DRAW;
    }

    public BattleResults pbDecisionOnTime2()
    {
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      int num4 = 0;
      foreach (IPokemon pokemon in this.party1)
      {
        if (pokemon.IsNotNullOrNone() && pokemon.HP > 0 && !pokemon.isEgg)
        {
          ++num1;
          num3 += pokemon.HP * 100 / pokemon.TotalHP;
        }
      }
      if (num1 > 0)
        num3 /= num1;
      foreach (IPokemon pokemon in this.party2)
      {
        if (pokemon.IsNotNullOrNone() && pokemon.HP > 0 && !pokemon.isEgg)
        {
          ++num2;
          num4 += pokemon.HP * 100 / pokemon.TotalHP;
        }
      }
      if (num2 > 0)
        num4 /= num2;
      if (num1 > num2)
        return BattleResults.WON;
      if (num1 < num2)
        return BattleResults.LOST;
      if (num3 > num4)
        return BattleResults.WON;
      return num3 < num4 ? BattleResults.LOST : BattleResults.DRAW;
    }

    BattleResults IBattle.pbDecisionOnDraw() => BattleResults.DRAW;

    public void pbJudge()
    {
      GameDebug.Log(string.Format("[Counts: #{0}/#{1}]", (object) this.pbPokemonCount(this.party1), (object) this.pbPokemonCount(this.party2)));
      if (this.pbAllFainted(this.party1) && this.pbAllFainted(this.party2))
        this.decision = this.pbDecisionOnDraw();
      else if (this.pbAllFainted(this.party1))
      {
        this.decision = BattleResults.LOST;
      }
      else
      {
        if (!this.pbAllFainted(this.party2))
          return;
        this.decision = BattleResults.WON;
      }
    }

    public virtual void pbDisplay(string msg) => (this.scene as IPokeBattle_DebugSceneNoGraphics).pbDisplayMessage(msg);

    public virtual IEnumerator pbDisplayIE(string msg)
    {
      yield return null;
    }

    public virtual void pbDisplayPaused(string msg) => (this.scene as IPokeBattle_DebugSceneNoGraphics).pbDisplayPausedMessage(msg);

    public virtual IEnumerator pbDisplayPausedIE(string msg)
    {
      yield return null;
    }

    public virtual void pbDisplayBrief(string msg) => (this.scene as IPokeBattle_DebugSceneNoGraphics).pbDisplayMessage(msg, true);

    public virtual IEnumerator pbDisplayBriefIE(string msg)
    {
      yield return null;
    }

    public virtual bool pbDisplayConfirm(string msg) => (this.scene as IPokeBattle_DebugSceneNoGraphics).pbDisplayConfirmMessage(msg);

    public virtual IEnumerator pbDisplayConfirmIE(string msg)
    {
      yield return null;
    }

    public void pbShowCommands(string msg, string[] commands, bool cancancel = true) => (this.scene as IPokeBattle_DebugSceneNoGraphics).pbShowCommands(msg, commands, cancancel);

    public void pbShowCommands(string msg, string[] commands, int cancancel) => (this.scene as IPokeBattle_DebugSceneNoGraphics).pbShowCommands(msg, commands, cancancel);

    public void pbAnimation(Moves move, IBattler attacker, IBattler opponent, int hitnum = 0)
    {
      if (!this.battlescene)
        return;
      (this.scene as IPokeBattle_DebugSceneNoGraphics).pbAnimation(move, attacker, opponent, hitnum);
    }

    public void pbCommonAnimation(string name, IBattler attacker, IBattler opponent, int hitnum = 0)
    {
      if (!this.battlescene || !(this.scene is IPokeBattle_Scene scene))
        return;
      scene.pbCommonAnimation(name, attacker, opponent, hitnum);
    }
    
    public virtual BattleResults pbStartBattle(bool canlose = false)
    {
      GameDebug.LogError("Use pbStartBattleIE instead");
      return this.decision;
    }

    public virtual IEnumerator pbStartBattleIE(MonoBehaviour owner, bool canlose = false)
    {
      GameDebug.Log("");
      GameDebug.Log("******************************************");
      this.decision = BattleResults.InProgress;
      try
      {
        owner.StartCoroutine(pbStartBattleCoreIE(owner, canlose));
      }
      catch (BattleAbortedException ex)
      {
        GameDebug.LogError(ex.Message);
        GameDebug.LogError(ex.StackTrace);
        this.decision = BattleResults.ABORTED;
        (this.scene as IPokeBattle_DebugSceneNoGraphics).pbEndBattle(this.decision);
      }

      yield return this.decision;
    }

    public void pbStartBattleCore(bool canlose)
    {
      GameDebug.LogError("Use pbStartBattleCoreIE instead");
    }

    public IEnumerator pbStartBattleCoreIE(MonoBehaviour owner, bool canlose)
    {
      if (!this.fullparty1 && this.party1.Length > 6)
      {
        GameDebug.LogError(Game._INTL("Party 1 has more than {1} Pokémon.", (object) 6));
        this.party1 = new IPokemon[6];
        for (int index = 0; index < 6; ++index)
          this.party1[index] = this.party1[index];
      }
      if (!this.fullparty2 && this.party2.Length > 6)
      {
        GameDebug.LogError(Game._INTL("Party 2 has more than {1} Pokémon.", (object) 6));
        this.party2 = new IPokemon[6];
        for (int index = 0; index < 6; ++index)
          this.party2[index] = this.party2[index];
      }
      if (this.opponent == null || this.opponent.Length == 0)
      {
        while (this.party2.Length != 1)
        {
          if (this.party2.Length > 1)
          {
            if (!this.doublebattle)
            {
              GameDebug.LogError(Game._INTL("Only one wild Pokémon is allowed in single battles"));
              if (this.party1.GetBattleCount() > 1)
              {
                this.doublebattle = true;
                GameDebug.LogWarning("Changed battle to double.");
              }
              else
              {
                this.party2 = new IPokemon[1]
                {
                  ((IEnumerable<IPokemon>) this.party2).First<IPokemon>((Func<IPokemon, bool>) (x => x.IsNotNullOrNone()))
                };
                GameDebug.LogWarning("Removed additional wild pokemon from opposing side.");
                continue;
              }
            }
            this.battlers[1].pbInitialize(this.party2[0], 0, false);
            this.battlers[3].pbInitialize(this.party2[1], 0, false);
            if (this.peer is IBattlePeerMultipleForms peer1)
              peer1.pbOnEnteringBattle((IBattle) this, this.party2[0]);
            if (this.peer is IBattlePeerMultipleForms peer2)
              peer2.pbOnEnteringBattle((IBattle) this, this.party2[1]);
            this.pbSetSeen(this.party2[0]);
            this.pbSetSeen(this.party2[1]);
            (this.scene as IPokeBattle_DebugSceneNoGraphics).pbStartBattle((IBattle) this);
            this.pbDisplayPaused(Game._INTL("Wild {1} and\r\n{2} appeared!", (object) Game._INTL(this.party2[0].Species.ToString(TextScripts.Name)), (object) Game._INTL(this.party2[1].Species.ToString(TextScripts.Name))));
            goto label_51;
          }
          else
          {
            GameDebug.LogError(Game._INTL("Only one or two wild Pokémon are allowed"));
            goto label_51;
          }
        }
        if (this.doublebattle)
        {
          GameDebug.LogError(Game._INTL("Only two wild Pokémon are allowed in double battles"));
          this.doublebattle = false;
          GameDebug.LogWarning("Changed battle to single.");
        }
        IPokemon pokemon = this.party2[0];
        this.battlers[1].pbInitialize(pokemon, 0, false);
        if (this.peer is IBattlePeerMultipleForms peer)
          peer.pbOnEnteringBattle((IBattle) this, pokemon);
        this.pbSetSeen(pokemon);
        (this.scene as IPokeBattle_DebugSceneNoGraphics).pbStartBattle((IBattle) this);
        this.pbDisplayPaused(Game._INTL("Wild {1} appeared!", (object) Game._INTL(pokemon.Species.ToString(TextScripts.Name))));
      }
      else if (this.doublebattle)
      {
        if ((uint) this.opponent.Length > 0U && this.opponent.Length != 1 && this.opponent.Length != 2)
        {
          GameDebug.LogError(Game._INTL("Opponents with zero or more than two people are not allowed"));
          this.opponent = new ITrainer[2]
          {
            this.opponent[0],
            this.opponent[1]
          };
        }
        if ((uint) this.player.Length > 0U && this.player.Length != 1 && this.player.Length != 2)
        {
          GameDebug.LogError(Game._INTL("Player trainers with zero or more than two people are not allowed"));
          this.player = new ITrainer[2]
          {
            this.player[0],
            this.player[1]
          };
        }
        (this.scene as IPokeBattle_DebugSceneNoGraphics).pbStartBattle((IBattle) this);
        if ((uint) this.opponent.Length > 0U)
        {
          this.pbDisplayPaused(Game._INTL("{1} and {2} want to battle!", (object) this.opponent[0].name, (object) this.opponent[1].name));
          int nextUnfainted1 = this.pbFindNextUnfainted(this.party2, 0, this.pbSecondPartyBegin(1));
          if (nextUnfainted1 < 0)
            GameDebug.LogError(Game._INTL("Opponent 1 has no unfainted Pokémon"));
          int nextUnfainted2 = this.pbFindNextUnfainted(this.party2, this.pbSecondPartyBegin(1), -1);
          if (nextUnfainted2 < 0)
            GameDebug.LogError(Game._INTL("Opponent 2 has no unfainted Pokémon"));
          this.battlers[1].pbInitialize(this.party2[nextUnfainted1], (int) (sbyte) nextUnfainted1, false);
          this.pbDisplayBrief(Game._INTL("{1} sent\r\nout {2}!", (object) this.opponent[0].name, (object) this.battlers[1].Name));
          this.pbSendOut(1, this.party2[nextUnfainted1]);
          this.battlers[3].pbInitialize(this.party2[nextUnfainted2], (int) (sbyte) nextUnfainted2, false);
          this.pbDisplayBrief(Game._INTL("{1} sent\r\nout {2}!", (object) this.opponent[1].name, (object) this.battlers[3].Name));
          this.pbSendOut(3, this.party2[nextUnfainted2]);
        }
        else
        {
          this.pbDisplayPaused(Game._INTL("{1}\r\nwould like to battle!", (object) this.opponent[0].name));
          int nextUnfainted3 = this.pbFindNextUnfainted(this.party2, 0, -1);
          int nextUnfainted4 = this.pbFindNextUnfainted(this.party2, nextUnfainted3 + 1, -1);
          if (nextUnfainted3 < 0 || nextUnfainted4 < 0)
            GameDebug.LogError(Game._INTL("Opponent doesn't have two unfainted Pokémon"));
          this.battlers[1].pbInitialize(this.party2[nextUnfainted3], (int) (sbyte) nextUnfainted3, false);
          this.battlers[3].pbInitialize(this.party2[nextUnfainted4], (int) (sbyte) nextUnfainted4, false);
          this.pbDisplayBrief(Game._INTL("{1} sent\r\nout {2} and {3}!", (object) this.opponent[0].name, (object) this.battlers[1].Name, (object) this.battlers[3].Name));
          this.pbSendOut(1, this.party2[nextUnfainted3]);
          this.pbSendOut(3, this.party2[nextUnfainted4]);
        }
      }
      else
      {
        int nextUnfainted = this.pbFindNextUnfainted(this.party2, 0, -1);
        if (nextUnfainted < 0)
          GameDebug.LogError(Game._INTL("Trainer has no unfainted Pokémon"));
        if ((uint) this.opponent.Length > 0U)
        {
          if (this.opponent.Length != 1)
            GameDebug.LogError(Game._INTL("Opponent trainer must be only one person in single battles"));
          this.opponent = new ITrainer[1]
          {
            this.opponent[0]
          };
        }
        if ((uint) this.player.Length > 0U)
        {
          if (this.player.Length != 1)
            GameDebug.LogError(Game._INTL("Player trainer must be only one person in single battles"));
          this.player = new ITrainer[1]{ this.player[0] };
        }
        IPokemon pokemon = this.party2[nextUnfainted];
        (this.scene as IPokeBattle_DebugSceneNoGraphics).pbStartBattle((IBattle) this);
        this.pbDisplayPaused(Game._INTL("{1}\r\nwould like to battle!", (object) this.opponent[0].name));
        this.battlers[1].pbInitialize(pokemon, (int) (sbyte) nextUnfainted, false);
        this.pbDisplayBrief(Game._INTL("{1} sent\r\nout {2}!", (object) this.opponent[0].name, (object) this.battlers[1].Name));
        this.pbSendOut(1, pokemon);
      }
label_51:
      if (this.doublebattle)
      {
        int nextUnfainted5;
        int nextUnfainted6;
        if ((uint) this.player.Length > 0U)
        {
          nextUnfainted5 = this.pbFindNextUnfainted(this.party1, 0, this.pbSecondPartyBegin(0));
          if (nextUnfainted5 < 0)
            GameDebug.LogError(Game._INTL("Player 1 has no unfainted Pokémon"));
          nextUnfainted6 = this.pbFindNextUnfainted(this.party1, this.pbSecondPartyBegin(0), -1);
          if (nextUnfainted6 < 0)
            GameDebug.LogError(Game._INTL("Player 2 has no unfainted Pokémon"));
          this.battlers[0].pbInitialize(this.party1[nextUnfainted5], (int) (sbyte) nextUnfainted5, false);
          this.battlers[2].pbInitialize(this.party1[nextUnfainted6], (int) (sbyte) nextUnfainted6, false);
          this.pbDisplayBrief(Game._INTL("{1} sent\r\nout {2}! Go! {3}!", (object) this.player[1].name, (object) this.battlers[2].Name, (object) this.battlers[0].Name));
          this.pbSetSeen(this.party1[nextUnfainted5]);
          this.pbSetSeen(this.party1[nextUnfainted6]);
        }
        else
        {
          nextUnfainted5 = this.pbFindNextUnfainted(this.party1, 0, -1);
          nextUnfainted6 = this.pbFindNextUnfainted(this.party1, nextUnfainted5 + 1, -1);
          if (nextUnfainted5 < 0 || nextUnfainted6 < 0)
            GameDebug.LogError(Game._INTL("Player doesn't have two unfainted Pokémon"));
          this.battlers[0].pbInitialize(this.party1[nextUnfainted5], (int) (sbyte) nextUnfainted5, false);
          this.battlers[2].pbInitialize(this.party1[nextUnfainted6], (int) (sbyte) nextUnfainted6, false);
          this.pbDisplayBrief(Game._INTL("Go! {1} and {2}!", (object) this.battlers[0].Name, (object) this.battlers[2].Name));
        }
        this.pbSendOut(0, this.party1[nextUnfainted5]);
        this.pbSendOut(2, this.party1[nextUnfainted6]);
      }
      else
      {
        int nextUnfainted = this.pbFindNextUnfainted(this.party1, 0, -1);
        if (nextUnfainted < 0)
          GameDebug.LogError(Game._INTL("Player has no unfainted Pokémon"));
        this.battlers[0].pbInitialize(this.party1[nextUnfainted], (int) (sbyte) nextUnfainted, false);
        this.pbDisplayBrief(Game._INTL("Go! {1}!", (object) this.battlers[0].Name));
        this.pbSendOut(0, this.party1[nextUnfainted]);
      }
      if (this.weather == Weather.SUNNYDAY)
      {
        this.pbCommonAnimation("Sunny", (IBattler) null, (IBattler) null, 0);
        this.pbDisplay(Game._INTL("The sunlight is strong."));
      }
      else if (this.weather == Weather.RAINDANCE)
      {
        this.pbCommonAnimation("Rain", (IBattler) null, (IBattler) null, 0);
        this.pbDisplay(Game._INTL("It is raining."));
      }
      else if (this.weather == Weather.SANDSTORM)
      {
        this.pbCommonAnimation("Sandstorm", (IBattler) null, (IBattler) null, 0);
        this.pbDisplay(Game._INTL("A sandstorm is raging."));
      }
      else if (this.weather == Weather.HAIL)
      {
        this.pbCommonAnimation("Hail", (IBattler) null, (IBattler) null, 0);
        this.pbDisplay(Game._INTL("Hail is falling."));
      }
      else if (this.weather == Weather.HEAVYRAIN)
      {
        this.pbCommonAnimation("HeavyRain", (IBattler) null, (IBattler) null, 0);
        this.pbDisplay(Game._INTL("It is raining heavily."));
      }
      else if (this.weather == Weather.HARSHSUN)
      {
        this.pbCommonAnimation("HarshSun", (IBattler) null, (IBattler) null, 0);
        this.pbDisplay(Game._INTL("The sunlight is extremely harsh."));
      }
      else if (this.weather == Weather.STRONGWINDS)
      {
        this.pbCommonAnimation("StrongWinds", (IBattler) null, (IBattler) null, 0);
        this.pbDisplay(Game._INTL("The wind is strong."));
      }
      this.pbOnActiveAll();
      this.turncount = 0;
      do
      {
        GameDebug.Log("");
        GameDebug.Log(string.Format("***Round #{0}***", (object) (this.turncount + 1)));
        if (this.debug && this.turncount >= 100)
        {
          this.decision = this.pbDecisionOnTime();
          GameDebug.Log("");
          GameDebug.Log("***Undecided after 100 rounds, aborting***");
          this.pbAbort();
          break;
        }
        yield return owner.StartCoroutine(pbCommandPhaseIE(owner));
        if (this.decision <= BattleResults.ABORTED)
        {
          this.pbAttackPhase();
          if (this.decision <= BattleResults.ABORTED)
          {
            this.pbEndOfRoundPhase();
            if (this.decision <= BattleResults.ABORTED)
              ++this.turncount;
            else
              break;
          }
          else
            break;
        }
        else
          break;
      }
      while (this.decision == BattleResults.InProgress);
      int num = (int) this.pbEndOfBattle(canlose);

      yield return null;
    }
    public virtual MenuCommands pbCommandMenu(int i) => (MenuCommands) (this.scene as IPokeBattle_SceneNonInteractive).pbCommandMenu(i);

    public virtual KeyValuePair<Items, int?> pbItemMenu(int i) => new KeyValuePair<Items, int?>((this.scene as IPokeBattle_SceneNonInteractive).pbItemMenu(i), new int?());

    public virtual bool pbAutoFightMenu(int i) => false;

    public virtual void pbCommandPhase()
    {
      
    }

    public virtual IEnumerator pbCommandPhaseIE(MonoBehaviour owner)
    {
      if (this.scene is PokemonUnity.UX.IPokeBattle_SceneIE scene1)
        scene1.pbBeginCommandPhase();
      if (this.scene is PokemonUnity.UX.IPokeBattle_SceneIE scene2)
        scene2.pbResetCommandIndices();
      for (int index = 0; index < this.battlers.Length; ++index)
      {
        this.battlers[index].effects.SkipTurn = false;
        if (this.CanShowCommands(index) || this.battlers[index].isFainted())
          this.choices[index] = (IBattleChoice) new Choice();
        else if (this.doublebattle && !this.pbIsDoubleBattler(index))
          GameDebug.Log("[Reusing commands] #" + this.battlers[index].ToString(true));
      }
      for (int index1 = 0; index1 < this.sides.Length; ++index1)
      {
        for (int index2 = 0; index2 < this.megaEvolution[index1].Length; ++index2)
        {
          if (this.megaEvolution[index1][index2] >= 0)
            this.megaEvolution[index1][index2] = -1;
        }
      }
      for (int index3 = 0; index3 < this.battlers.Length && this.decision != BattleResults.ABORTED; ++index3)
      {
        if ((uint) this.choices[index3].Action <= 0U)
        {
          if (!this.pbOwnedByPlayer(index3) || this.controlPlayer)
          {
            if (!this.battlers[index3].isFainted() && this.CanShowCommands(index3))
              (this.scene as PokemonUnity.UX.IPokeBattle_SceneIE).pbChooseEnemyCommand(index3);
          }
          else
          {
            bool flag = false;
            if (this.CanShowCommands(index3))
            {
              while (true)
              {
                // Command Menu Selection
                MenuCommands menuCommands = MenuCommands.POKEMON;
                //yield return owner.StartCoroutine((this.scene as BattleScene).pbCommandMenuIE(index3, new string[] {
                //    Game._INTL("What will\n{1} do?", this.battlers[index3].Name),
                //    Game._INTL("Battle"),
                //    Game._INTL("Bag"),
                //    Game._INTL("Pokémon"),
                //    Game._INTL("Run") //shadowTrainer ? Game._INTL("Call") :
                //}, 0, value => menuCommands = (MenuCommands) value));
                //menuCommands = (MenuCommands)(this.scene as BattleScene).pbCommandMenu(index3);
                yield return (this.scene as PokemonUnity.UX.IPokeBattle_SceneIE).pbCommandMenu(index3, value => menuCommands = (MenuCommands)value);
                
                Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name + " - menuCommands = " + menuCommands);

                // Command Menu Result Check
                if (menuCommands == MenuCommands.FIGHT)
                {
                  if (this.CanShowFightMenu(index3))
                  {
                    if (this.pbAutoFightMenu(index3))
                      flag = true;
                    do
                    {
                      // Fight Menu Selection
                      int idxMove = 0;
                      //yield return owner.StartCoroutine((this.scene as BattleScene).pbFightMenuIE(index3, value => idxMove = value));
                      //idxMove = (this.scene as BattleScene).pbFightMenu(index3);
                      yield return (this.scene as PokemonUnity.UX.IPokeBattle_SceneIE).pbFightMenu(index3, value => idxMove = value);
                      
                      // Fight Menu Result Check
                      if (idxMove < 0)
                      {
                        int index4 = this.pbIsOpposing(index3) ? 1 : 0;
                        int ownerIndex = this.pbGetOwnerIndex(index3);
                        if (this.megaEvolution[index4][ownerIndex] == index3)
                        {
                          this.megaEvolution[index4][ownerIndex] = -1;
                          break;
                        }
                        break;
                      }
                      if (this.pbRegisterMove(index3, idxMove, true))
                      {
                        // Selects a target in Double Battle
                        if (this.doublebattle)
                        {
                          IBattleMove move = this.battlers[index3].moves[idxMove];
                          Targets targettype = this.battlers[index3].pbTarget(move);
                          if (targettype == Targets.SELECTED_POKEMON || targettype == Targets.SELECTED_POKEMON_ME_FIRST)
                          {
                            // Target Selection
                            int idxTarget = -1;
                            //owner.StartCoroutine((this.scene as BattleScene).pbChooseTargetIE(index3, targettype, value => idxTarget = value));
                            //idxTarget = (this.scene as BattleScene).pbChooseTarget(index3, targettype);
                            yield return (this.scene as PokemonUnity.UX.IPokeBattle_SceneIE).pbChooseTarget(index3, targettype, value => idxTarget = value);
                            
                            // Target Selection Result Check
                            if (idxTarget >= 0)
                              this.pbRegisterTarget(index3, idxTarget);
                            else
                              goto label_43;
                          }
                          else if (targettype == Targets.USER_OR_ALLY)
                          {
                            // Target Selection
                            int idxTarget = -1;
                            //owner.StartCoroutine((this.scene as BattleScene).pbChooseTargetIE(index3, targettype, value => idxTarget = value));
                            //idxTarget = (this.scene as BattleScene).pbChooseTarget(index3, targettype);
                            yield return (this.scene as PokemonUnity.UX.IPokeBattle_SceneIE).pbChooseTarget(index3, targettype, value => idxTarget = value);
                            
                            // Target Selection Result Check
                            if (idxTarget >= 0 && idxTarget % 2 != 1)
                              this.pbRegisterTarget(index3, idxTarget);
                            else
                              goto label_43;
                          }
                        }
                        flag = true;
                      }
label_43:;
                    }
                    while (!flag);
                  }
                  else
                  {
                    this.pbAutoChooseMove(index3, true);
                    flag = true;
                  }
                }
                else if (menuCommands != MenuCommands.FIGHT && this.battlers[index3].effects.SkyDrop)
                {
                  this.pbDisplay(Game._INTL("Sky Drop won't let {1} go!", (object) this.battlers[index3].ToString(true)));
                }
                else
                {
                  switch (menuCommands)
                  {
                    case MenuCommands.CANCEL:
                      goto label_66;
                    case MenuCommands.BAG:
                      if (!this.internalbattle)
                      {
                        if (this.pbOwnedByPlayer(index3))
                        {
                          this.pbDisplay(Game._INTL("Items can't be used here."));
                          break;
                        }
                        break;
                      }
                      
                      // Item Menu Selection
                      KeyValuePair<Items, int?> keyValuePair = new KeyValuePair<Items, int?>(Items.NONE, new int?());
                      //yield return owner.StartCoroutine((scene as BattleScene).pbItemMenuIE(index3, 
                      //  value => keyValuePair = new KeyValuePair<Items, int?>((Items)value, new int?())));
                      keyValuePair = new KeyValuePair<Items, int?>((scene as BattleScene).pbItemMenu(index3), new int?());
                      
                      // Item Menu Result Check
                      if (keyValuePair.Key > Items.NONE && this.pbRegisterItem(index3, keyValuePair.Key, keyValuePair.Value))
                        flag = true;
                      break;
                    case MenuCommands.POKEMON:
                      // Pokemon Switch Selection
                      int idxOther = -1;
                      //yield return owner.StartCoroutine((this.scene as BattleScene).pbSwitchIE(index3, false, true, value => idxOther = value));
                      idxOther = (this.scene as BattleScene).pbSwitch(index3, false, true);
                      
                      // Pokemon Switch Result Check
                      if (idxOther >= 0 && this.pbRegisterSwitch(index3, idxOther))
                      {
                        flag = true;
                        break;
                      }
                      break;
                    case MenuCommands.RUN:
                      // Run Check
                      int num = -1;
                      yield return owner.StartCoroutine(this.pbRunIE(index3, false, value => num = value));
                      
                      // Run Value Set
                      if (num <= 0)
                      {
                        if (num < 0)
                        {
                          flag = true;
                          int index5 = this.pbIsOpposing(index3) ? 1 : 0;
                          int ownerIndex = this.pbGetOwnerIndex(index3);
                          if (this.megaEvolution[index5][ownerIndex] == index3)
                            this.megaEvolution[index5][ownerIndex] = -1;
                          break;
                        }
                        break;
                      }
                      goto label_58;
                    case MenuCommands.CALL:
                      IBattler battler = this.battlers[index3];
                      this.choices[index3] = (IBattleChoice) new Choice(ChoiceAction.CallPokemon);
                      int index6 = this.pbIsOpposing(index3) ? 1 : 0;
                      int ownerIndex1 = this.pbGetOwnerIndex(index3);
                      if (this.megaEvolution[index6][ownerIndex1] == index3)
                        this.megaEvolution[index6][ownerIndex1] = -1;
                      flag = true;
                      break;
                  }
                }
                if (flag)
                  goto label_75;
              }
label_58:
              break;
label_66:
              if (this.megaEvolution[0][0] >= 0)
                this.megaEvolution[0][0] = -1;
              if (this.megaEvolution[1][0] >= 0)
                this.megaEvolution[1][0] = -1;
              if (this.choices[0].Action == ChoiceAction.UseItem && Game.GameData.Bag != null && Game.GameData.Bag.pbCanStore((Items) this.choices[0].Index))
                Game.GameData.Bag.pbStoreItem((Items) this.choices[0].Index);
              owner.StartCoroutine(pbCommandPhaseIE(owner));
              break;
            }
label_75:;
          }
        }
      }
    }

    public void pbAttackPhase()
    {
      if (this.scene is IPokeBattle_DebugSceneNoGraphics scene)
        scene.pbBeginAttackPhase();
      for (int i = 0; i < this.battlers.Length; ++i)
      {
        this.successStates[i].Clear();
        if (this.choices[i].Action != ChoiceAction.UseMove && this.choices[i].Action != ChoiceAction.SwitchPokemon)
        {
          this.battlers[i].effects.DestinyBond = false;
          this.battlers[i].effects.Grudge = false;
        }
        if (!this.battlers[i].isFainted())
          ++this.battlers[i].turncount;
        if (!this.pbChoseMove(i, Moves.RAGE))
          this.battlers[i].effects.Rage = false;
      }
      this.usepriority = false;
      this.priority = this.pbPriority(false, true);
      List<int> intList1 = new List<int>();
      foreach (IBattler battler in this.priority)
      {
        if (this.choices[battler.Index].Action == ChoiceAction.UseMove && !battler.effects.SkipTurn && this.megaEvolution[this.pbIsOpposing(battler.Index) ? 1 : 0][this.pbGetOwnerIndex(battler.Index)] == battler.Index)
          intList1.Add(battler.Index);
      }
      if (intList1.Count > 0)
      {
        foreach (IBattler battler in this.priority)
        {
          if (intList1.Contains(battler.Index))
            battler.pbAbilitiesOnSwitchIn(true);
        }
      }
      foreach (IBattler battler in this.priority)
      {
        if (this.choices[battler.Index].Action == ChoiceAction.CallPokemon && !battler.effects.SkipTurn)
          this.pbCall(battler.Index);
      }
      this.switching = true;
      List<int> intList2 = new List<int>();
      foreach (IBattler battler1 in this.priority)
      {
        if (this.choices[battler1.Index].Action == ChoiceAction.SwitchPokemon && !battler1.effects.SkipTurn)
        {
          int index = this.choices[battler1.Index].Index;
          int newpokename = index;
          if (this.pbParty(battler1.Index)[index].Ability == Abilities.ILLUSION)
            newpokename = this.pbGetLastPokeInTeam(battler1.Index);
          this.lastMoveUser = battler1.Index;
          if (!this.pbOwnedByPlayer(battler1.Index))
          {
            this.pbDisplayBrief(Game._INTL("{1} withdrew {2}!", (object) this.pbGetOwner(battler1.Index).name, (object) battler1.Name));
            GameDebug.Log("[Withdrew Pokémon] Opponent withdrew #" + battler1.ToString(true));
          }
          else
          {
            this.pbDisplayBrief(Game._INTL("{1}, that's enough!\r\nCome back!", (object) battler1.Name));
            GameDebug.Log("[Withdrew Pokémon] Player withdrew #" + battler1.ToString(true));
          }
          foreach (IBattler battler2 in this.priority)
          {
            if (battler1.pbIsOpposing(battler2.Index))
            {
              if (this.pbChoseMoveFunctionCode(battler2.Index, PokemonUnity.Attack.Data.Effects.x081) && !battler2.hasMovedThisRound() && battler2.Status != Status.SLEEP && battler2.Status != Status.FROZEN && !battler2.effects.SkyDrop && (!battler2.hasWorkingAbility(Abilities.TRUANT) || !battler2.effects.Truant))
              {
                this.choices[battler2.Index] = (IBattleChoice) new Choice(this.choices[battler2.Index].Action, this.choices[battler2.Index].Index, this.choices[battler2.Index].Move, battler1.Index);
                battler2.pbUseMove(this.choices[battler2.Index]);
                battler2.effects.Pursuit = true;
                this.switching = false;
                if (this.decision > BattleResults.ABORTED)
                  return;
              }
              if (battler1.isFainted())
                break;
            }
          }
          if (!this.pbRecallAndReplace(battler1.Index, index, newpokename, false, false))
          {
            if (!this.doublebattle)
            {
              this.switching = false;
              return;
            }
          }
          else
            intList2.Add(battler1.Index);
        }
      }
      if (intList2.Count > 0)
      {
        foreach (IBattler battler in this.priority)
        {
          if (intList2.Contains(battler.Index))
            battler.pbAbilitiesOnSwitchIn(true);
        }
      }
      this.switching = false;
      foreach (IBattler battler in this.priority)
      {
        if (this.choices[battler.Index].Action == ChoiceAction.UseItem && !battler.effects.SkipTurn)
        {
          if (this.pbIsOpposing(battler.Index))
          {
            this.pbEnemyUseItem((Items) this.choices[battler.Index].Index, battler);
          }
          else
          {
            Items index = (Items) this.choices[battler.Index].Index;
            if (index > Items.NONE)
            {
              int num = 0;
              if (num == 1 || num == 3)
              {
                if (this.choices[battler.Index].Target >= 0)
                  this.pbUseItemOnPokemon(index, this.choices[battler.Index].Target, battler, (IHasDisplayMessage) this.scene);
              }
              else if ((num == 2 || num == 4) && !ItemHandlers.hasUseInBattle(index))
                this.pbUseItemOnBattler(index, this.choices[battler.Index].Target, battler, (IHasDisplayMessage) this.scene);
            }
          }
        }
      }
      foreach (IBattler attacker in this.priority)
      {
        if (!attacker.effects.SkipTurn && this.pbChoseMoveFunctionCode(attacker.Index, PokemonUnity.Attack.Data.Effects.x0AB))
        {
          this.pbCommonAnimation("FocusPunch", attacker, (IBattler) null, 0);
          this.pbDisplay(Game._INTL("{1} is tightening its focus!", (object) attacker.ToString(false)));
        }
      }
      int num1 = 0;
      do
      {
        bool flag = false;
        foreach (IBattler battler in this.priority)
        {
          if (battler.effects.MoveNext && !battler.hasMovedThisRound() && !battler.effects.SkipTurn)
          {
            flag = battler.pbProcessTurn(this.choices[battler.Index]);
            if (flag)
              break;
          }
        }
        if (this.decision <= BattleResults.ABORTED)
        {
          if (!flag)
          {
            foreach (IBattler battler in this.priority)
            {
              if (!battler.effects.Quash && !battler.hasMovedThisRound() && !battler.effects.SkipTurn)
              {
                flag = battler.pbProcessTurn(this.choices[battler.Index]);
                if (flag)
                  break;
              }
            }
            if (this.decision <= BattleResults.ABORTED)
            {
              if (!flag)
              {
                foreach (IBattler battler in this.priority)
                {
                  if (battler.effects.Quash && !battler.hasMovedThisRound() && !battler.effects.SkipTurn)
                  {
                    flag = battler.pbProcessTurn(this.choices[battler.Index]);
                    if (flag)
                      break;
                  }
                }
                if (this.decision <= BattleResults.ABORTED)
                {
                  if (!flag)
                  {
                    foreach (IBattler battler in this.priority)
                    {
                      string battlerName = battler.Name;
                    
                      ChoiceAction action = this.choices[battler.Index].Action;
                      
                      bool hasValue = battler.lastRoundMoved.HasValue;
                      int Value = battler.lastRoundMoved.Value;

                      bool hasMovedThisRound = (hasValue && Value == this.turncount);
                      bool skipTurn = battler.effects.SkipTurn;
                      
                      if (action == ChoiceAction.UseMove && !hasMovedThisRound && !skipTurn)
                        flag = true;
                      if (flag)
                      {
                        break;
                      }
                    }
                    
                    // Test break here
                    break;
                    
                    if (!flag)
                      ++num1;
                  }
                }
                else
                  goto label_91;
              }
            }
            else
              goto label_92;
          }
        }
        else
          goto label_94;
      }
      while (num1 < 10);
      goto label_110;
label_94:
      return;
label_92:
      return;
label_91:
      return;
label_110:
      if (!(Game.GameData is IGameField gameData))
        return;
      gameData.pbWait(20);
    }

    void IBattle.pbEndOfRoundPhase()
    {
      GameDebug.Log("[End of round]");
      for (int index = 0; index < this.battlers.Length; ++index)
      {
        this.battlers[index].effects.Electrify = false;
        this.battlers[index].effects.Endure = false;
        this.battlers[index].effects.FirstPledge = PokemonUnity.Attack.Data.Effects.NONE;
        if (this.battlers[index].effects.HyperBeam > 0)
          --this.battlers[index].effects.HyperBeam;
        this.battlers[index].effects.KingsShield = false;
        this.battlers[index].effects.LifeOrb = false;
        this.battlers[index].effects.MoveNext = false;
        this.battlers[index].effects.Powder = false;
        this.battlers[index].effects.Protect = false;
        this.battlers[index].effects.ProtectNegation = false;
        this.battlers[index].effects.Quash = false;
        this.battlers[index].effects.Roost = false;
        this.battlers[index].effects.SpikyShield = false;
      }
      this.usepriority = false;
      this.priority = this.pbPriority(true, false);
      switch (this.weather)
      {
        case Weather.RAINDANCE:
          if (this.weatherduration > 0)
            --this.weatherduration;
          if (this.weatherduration == 0)
          {
            this.pbDisplay(Game._INTL("The rain stopped."));
            this.weather = Weather.NONE;
            GameDebug.Log("[End of effect] Rain weather ended");
            break;
          }
          this.pbCommonAnimation("Rain", (IBattler) null, (IBattler) null, 0);
          this.pbDisplay(Game._INTL("Rain continues to fall."));
          break;
        case Weather.HEAVYRAIN:
          bool flag1 = false;
          for (int index = 0; index < this.battlers.Length; ++index)
          {
            if (this.battlers[index].Ability == Abilities.PRIMORDIAL_SEA && !this.battlers[index].isFainted())
            {
              flag1 = true;
              break;
            }
          }
          if (!flag1)
            this.weatherduration = 0;
          if (this.weatherduration == 0)
          {
            this.pbDisplay(Game._INTL("The heavy rain stopped."));
            this.weather = Weather.NONE;
            GameDebug.Log("[End of effect] Primordial Sea's rain weather ended");
            break;
          }
          this.pbCommonAnimation("HeavyRain", (IBattler) null, (IBattler) null, 0);
          this.pbDisplay(Game._INTL("It is raining heavily."));
          break;
        case Weather.SUNNYDAY:
          if (this.weatherduration > 0)
            --this.weatherduration;
          if (this.weatherduration == 0)
          {
            this.pbDisplay(Game._INTL("The sunlight faded."));
            this.weather = Weather.NONE;
            GameDebug.Log("[End of effect] Sunlight weather ended");
            break;
          }
          this.pbCommonAnimation("Sunny", (IBattler) null, (IBattler) null, 0);
          this.pbDisplay(Game._INTL("The sunlight is strong."));
          if (this.pbWeather == Weather.SUNNYDAY)
          {
            foreach (IBattler pkmn in this.priority)
            {
              if (pkmn.hasWorkingAbility(Abilities.SOLAR_POWER))
              {
                GameDebug.Log("[Ability triggered] #" + pkmn.ToString(false) + "'s Solar Power");
                (this.scene as IPokeBattle_DebugSceneNoGraphics).pbDamageAnimation(pkmn, TypeEffective.Ineffective);
                pkmn.pbReduceHP((int) Math.Floor((double) pkmn.TotalHP / 8.0));
                this.pbDisplay(Game._INTL("{1} was hurt by the sunlight!", (object) pkmn.ToString(false)));
                if (pkmn.isFainted() && !pkmn.pbFaint())
                  return;
              }
            }
          }
          break;
        case Weather.HARSHSUN:
          bool flag2 = false;
          for (int index = 0; index < this.battlers.Length; ++index)
          {
            if (this.battlers[index].Ability == Abilities.DESOLATE_LAND && !this.battlers[index].isFainted())
            {
              flag2 = true;
              break;
            }
          }
          if (!flag2)
            this.weatherduration = 0;
          if (this.weatherduration == 0)
          {
            this.pbDisplay(Game._INTL("The harsh sunlight faded."));
            this.weather = Weather.NONE;
            GameDebug.Log("[End of effect] Desolate Land's sunlight weather ended");
            break;
          }
          this.pbCommonAnimation("HarshSun", (IBattler) null, (IBattler) null, 0);
          this.pbDisplay(Game._INTL("The sunlight is extremely harsh."));
          if (this.pbWeather == Weather.HARSHSUN)
          {
            foreach (IBattler pkmn in this.priority)
            {
              if (pkmn.hasWorkingAbility(Abilities.SOLAR_POWER))
              {
                GameDebug.Log("[Ability triggered] #" + pkmn.ToString(false) + "'s Solar Power");
                (this.scene as IPokeBattle_DebugSceneNoGraphics).pbDamageAnimation(pkmn, TypeEffective.Ineffective);
                pkmn.pbReduceHP((int) Math.Floor((double) pkmn.TotalHP / 8.0));
                this.pbDisplay(Game._INTL("{1} was hurt by the sunlight!", (object) pkmn.ToString(false)));
                if (pkmn.isFainted() && !pkmn.pbFaint())
                  return;
              }
            }
          }
          break;
        case Weather.SANDSTORM:
          if (this.weatherduration > 0)
            --this.weatherduration;
          if (this.weatherduration == 0)
          {
            this.pbDisplay(Game._INTL("The sandstorm subsided."));
            this.weather = Weather.NONE;
            GameDebug.Log("[End of effect] Sandstorm weather ended");
            break;
          }
          this.pbCommonAnimation("Sandstorm", (IBattler) null, (IBattler) null, 0);
          this.pbDisplay(Game._INTL("The sandstorm rages."));
          if (this.pbWeather == Weather.SANDSTORM)
          {
            GameDebug.Log("[Lingering effect triggered] Sandstorm weather damage");
            foreach (IBattler pkmn in this.priority)
            {
              if (!pkmn.isFainted())
              {
                int num;
                if (!pkmn.pbHasType(Types.GROUND) && !pkmn.pbHasType(Types.ROCK) && !pkmn.pbHasType(Types.STEEL) && !pkmn.hasWorkingAbility(Abilities.SAND_VEIL) && !pkmn.hasWorkingAbility(Abilities.SAND_RUSH) && !pkmn.hasWorkingAbility(Abilities.SAND_FORCE) && !pkmn.hasWorkingAbility(Abilities.MAGIC_GUARD) && !pkmn.hasWorkingAbility(Abilities.OVERCOAT) && !pkmn.hasWorkingItem(Items.SAFETY_GOGGLES))
                  num = !((IEnumerable<PokemonUnity.Attack.Data.Effects>) new PokemonUnity.Attack.Data.Effects[2]
                  {
                    PokemonUnity.Attack.Data.Effects.x101,
                    PokemonUnity.Attack.Data.Effects.x100
                  }).Contains<PokemonUnity.Attack.Data.Effects>(Kernal.MoveData[pkmn.effects.TwoTurnAttack].Effect) ? 1 : 0;
                else
                  num = 0;
                if (num != 0)
                {
                  (this.scene as IPokeBattle_DebugSceneNoGraphics).pbDamageAnimation(pkmn, TypeEffective.Ineffective);
                  pkmn.pbReduceHP((int) Math.Floor((double) pkmn.TotalHP / 16.0));
                  this.pbDisplay(Game._INTL("{1} is buffeted by the sandstorm!", (object) pkmn.ToString(false)));
                  if (pkmn.isFainted() && !pkmn.pbFaint())
                    return;
                }
              }
            }
          }
          break;
        case Weather.STRONGWINDS:
          bool flag3 = false;
          for (int index = 0; index < this.battlers.Length; ++index)
          {
            if (this.battlers[index].Ability == Abilities.DELTA_STREAM && !this.battlers[index].isFainted())
            {
              flag3 = true;
              break;
            }
          }
          if (!flag3)
            this.weatherduration = 0;
          if (this.weatherduration == 0)
          {
            this.pbDisplay(Game._INTL("The air current subsided."));
            this.weather = Weather.NONE;
            GameDebug.Log("[End of effect] Delta Stream's wind weather ended");
            break;
          }
          this.pbCommonAnimation("StrongWinds", (IBattler) null, (IBattler) null, 0);
          this.pbDisplay(Game._INTL("The wind is strong."));
          break;
        case Weather.HAIL:
          if (this.weatherduration > 0)
            --this.weatherduration;
          if (this.weatherduration == 0)
          {
            this.pbDisplay(Game._INTL("The hail stopped."));
            this.weather = Weather.NONE;
            GameDebug.Log("[End of effect] Hail weather ended");
            break;
          }
          this.pbCommonAnimation("Hail", (IBattler) null, (IBattler) null, 0);
          this.pbDisplay(Game._INTL("Hail continues to fall."));
          if (this.pbWeather == Weather.HAIL)
          {
            GameDebug.Log("[Lingering effect triggered] Hail weather damage");
            foreach (IBattler pkmn in this.priority)
            {
              if (!pkmn.isFainted())
              {
                int num;
                if (!pkmn.pbHasType(Types.ICE) && !pkmn.hasWorkingAbility(Abilities.ICE_BODY) && !pkmn.hasWorkingAbility(Abilities.SNOW_CLOAK) && !pkmn.hasWorkingAbility(Abilities.MAGIC_GUARD) && !pkmn.hasWorkingAbility(Abilities.OVERCOAT) && !pkmn.hasWorkingItem(Items.SAFETY_GOGGLES))
                  num = !((IEnumerable<int>) new int[2]
                  {
                    202,
                    203
                  }).Contains<int>((int) Kernal.MoveData[pkmn.effects.TwoTurnAttack].Effect) ? 1 : 0;
                else
                  num = 0;
                if (num != 0)
                {
                  (this.scene as IPokeBattle_DebugSceneNoGraphics).pbDamageAnimation(pkmn, TypeEffective.Ineffective);
                  pkmn.pbReduceHP((int) Math.Floor((double) pkmn.TotalHP / 16.0));
                  this.pbDisplay(Game._INTL("{1} is buffeted by the hail!", (object) pkmn.ToString(false)));
                  if (pkmn.isFainted() && !pkmn.pbFaint())
                    return;
                }
              }
            }
          }
          break;
      }
      if (this.weather == Weather.SHADOWSKY)
      {
        if (this.weatherduration > 0)
          --this.weatherduration;
        if (this.weatherduration == 0)
        {
          this.pbDisplay(Game._INTL("The shadow sky faded."));
          this.weather = Weather.NONE;
          GameDebug.Log("[End of effect] Shadow Sky weather ended");
        }
        else
        {
          this.pbCommonAnimation("ShadowSky", (IBattler) null, (IBattler) null, 0);
          this.pbDisplay(Game._INTL("The shadow sky continues."));
          if (this.pbWeather == Weather.SHADOWSKY)
          {
            GameDebug.Log("[Lingering effect triggered] Shadow Sky weather damage");
            foreach (IBattler pkmn in this.priority)
            {
              if (!pkmn.isFainted() && pkmn is IBattlerShadowPokemon battlerShadowPokemon && !battlerShadowPokemon.isShadow())
              {
                (this.scene as IPokeBattle_DebugSceneNoGraphics).pbDamageAnimation(pkmn, TypeEffective.Ineffective);
                pkmn.pbReduceHP((int) Math.Floor((double) pkmn.TotalHP / 16.0));
                this.pbDisplay(Game._INTL("{1} was hurt by the shadow sky!", (object) pkmn.ToString(false)));
                if (pkmn.isFainted() && !pkmn.pbFaint())
                  return;
              }
            }
          }
        }
      }
      foreach (IBattler battler1 in this.battlers)
      {
        if (!battler1.isFainted() && battler1.effects.FutureSight > 0)
        {
          --battler1.effects.FutureSight;
          if (battler1.effects.FutureSight == 0)
          {
            Moves futureSightMove = battler1.effects.FutureSightMove;
            GameDebug.Log("[Lingering effect triggered] #" + Game._INTL(futureSightMove.ToString(TextScripts.Name)) + " struck #" + battler1.ToString(true));
            this.pbDisplay(Game._INTL("{1} took the {2} attack!", (object) battler1.ToString(false), (object) Game._INTL(futureSightMove.ToString(TextScripts.Name))));
            IBattler pokemon = (IBattler) null;
            foreach (IBattler battler2 in this.battlers)
            {
              if (!battler2.pbIsOpposing(battler1.effects.FutureSightUserPos) && battler2.pokemonIndex == battler1.effects.FutureSightUser && !battler2.isFainted())
              {
                pokemon = battler2;
                break;
              }
            }
            if (!pokemon.IsNotNullOrNone())
            {
              IPokemon[] pokemonArray = this.pbParty(battler1.effects.FutureSightUserPos);
              if (pokemonArray[battler1.effects.FutureSightUser].HP > 0)
              {
                pokemon = (IBattler) new Pokemon((IBattle) this, (int) (sbyte) battler1.effects.FutureSightUserPos);
                pokemon.pbInitPokemon(pokemonArray[battler1.effects.FutureSightUser], (int) (sbyte) battler1.effects.FutureSightUser);
              }
            }
            if (!pokemon.IsNotNullOrNone())
            {
              this.pbDisplay(Game._INTL("But it failed!"));
            }
            else
            {
              this.futuresight = true;
              pokemon.pbUseMoveSimple(futureSightMove, target: battler1.Index);
              this.futuresight = false;
            }
            battler1.effects.FutureSight = 0;
            battler1.effects.FutureSightMove = Moves.NONE;
            battler1.effects.FutureSightUser = -1;
            battler1.effects.FutureSightUserPos = -1;
            if (battler1.isFainted() && !battler1.pbFaint())
              return;
          }
        }
      }
      foreach (IBattler pkmn in this.priority)
      {
        if (!pkmn.isFainted())
        {
          if (pkmn.hasWorkingAbility(Abilities.RAIN_DISH) && (this.pbWeather == Weather.RAINDANCE || this.pbWeather == Weather.HEAVYRAIN))
          {
            GameDebug.Log("[Ability triggered] #" + pkmn.ToString(false) + "'s Rain Dish");
            if (pkmn.pbRecoverHP((int) Math.Floor((double) pkmn.TotalHP / 16.0), true) > 0)
              this.pbDisplay(Game._INTL("{1}'s {2} restored its HP a little!", (object) pkmn.ToString(false), (object) Game._INTL(pkmn.Ability.ToString(TextScripts.Name))));
          }
          if (pkmn.hasWorkingAbility(Abilities.DRY_SKIN))
          {
            if (this.pbWeather == Weather.RAINDANCE || this.pbWeather == Weather.HEAVYRAIN)
            {
              GameDebug.Log("[Ability triggered] #" + pkmn.ToString(false) + "'s Dry Skin (in rain)");
              if (pkmn.pbRecoverHP((int) Math.Floor((double) pkmn.TotalHP / 8.0), true) > 0)
                this.pbDisplay(Game._INTL("{1}'s {2} was healed by the rain!", (object) pkmn.ToString(false), (object) Game._INTL(pkmn.Ability.ToString(TextScripts.Name))));
            }
            else if (this.pbWeather == Weather.SUNNYDAY || this.pbWeather == Weather.HARSHSUN)
            {
              GameDebug.Log("[Ability triggered] #" + pkmn.ToString(false) + "'s Dry Skin (in sun)");
              (this.scene as IPokeBattle_DebugSceneNoGraphics).pbDamageAnimation(pkmn, TypeEffective.Ineffective);
              if (pkmn.pbReduceHP((int) Math.Floor((double) pkmn.TotalHP / 8.0)) > 0)
                this.pbDisplay(Game._INTL("{1}'s {2} was hurt by the sunlight!", (object) pkmn.ToString(false), (object) Game._INTL(pkmn.Ability.ToString(TextScripts.Name))));
            }
          }
          if (pkmn.hasWorkingAbility(Abilities.ICE_BODY) && this.pbWeather == Weather.HAIL)
          {
            GameDebug.Log("[Ability triggered] #" + pkmn.ToString(false) + "'s Ice Body");
            if (pkmn.pbRecoverHP((int) Math.Floor((double) pkmn.TotalHP / 16.0), true) > 0)
              this.pbDisplay(Game._INTL("{1}'s {2} restored its HP a little!", (object) pkmn.ToString(false), (object) Game._INTL(pkmn.Ability.ToString(TextScripts.Name))));
          }
          if (pkmn.isFainted() && !pkmn.pbFaint())
            return;
        }
      }
      foreach (IBattler battler in this.priority)
      {
        if (!battler.isFainted() && battler.effects.Wish > 0)
        {
          --battler.effects.Wish;
          if (battler.effects.Wish == 0)
          {
            GameDebug.Log("[Lingering effect triggered] #" + battler.ToString(false) + "'s Wish");
            if (battler.pbRecoverHP(battler.effects.WishAmount, true) > 0)
              this.pbDisplay(Game._INTL("{1}'s wish came true!", (object) this.ToString(battler.Index, battler.effects.WishMaker)));
          }
        }
      }
      for (int index = 0; index < this.sides.Length; ++index)
      {
        if (this.sides[index].SeaOfFire > (byte) 0 && this.pbWeather != Weather.RAINDANCE && this.pbWeather != Weather.HEAVYRAIN)
        {
          if (index == 0)
            this.pbCommonAnimation("SeaOfFire", (IBattler) null, (IBattler) null, 0);
          if (index == 1)
            this.pbCommonAnimation("SeaOfFireOpp", (IBattler) null, (IBattler) null, 0);
          foreach (IBattler pkmn in this.priority)
          {
            if ((pkmn.Index & 1) == index && !pkmn.pbHasType(Types.FIRE) && !pkmn.hasWorkingAbility(Abilities.MAGIC_GUARD))
            {
              (this.scene as IPokeBattle_DebugSceneNoGraphics).pbDamageAnimation(pkmn, TypeEffective.Ineffective);
              if (pkmn.pbReduceHP((int) Math.Floor((double) pkmn.TotalHP / 8.0)) > 0)
                this.pbDisplay(Game._INTL("{1} is hurt by the sea of fire!", (object) pkmn.ToString(false)));
              if (pkmn.isFainted() && !pkmn.pbFaint())
                return;
            }
          }
        }
      }
      foreach (IBattler battler in this.priority)
      {
        if (!battler.isFainted())
        {
          if ((battler.hasWorkingAbility(Abilities.SHED_SKIN) && this.pbRandom(10) < 3 || battler.hasWorkingAbility(Abilities.HYDRATION) && (this.pbWeather == Weather.RAINDANCE || this.pbWeather == Weather.HEAVYRAIN)) && battler.Status > Status.NONE)
          {
            GameDebug.Log("[Ability triggered] #" + battler.ToString(false) + "'s #" + Game._INTL(battler.Ability.ToString(TextScripts.Name)));
            Status status = battler.Status;
            if (battler is IBattlerEffect battlerEffect)
              battlerEffect.pbCureStatus(false);
            switch (status)
            {
              case Status.SLEEP:
                this.pbDisplay(Game._INTL("{1}'s {2} cured its sleep problem!", (object) battler.ToString(false), (object) Game._INTL(battler.Ability.ToString(TextScripts.Name))));
                break;
              case Status.POISON:
                this.pbDisplay(Game._INTL("{1}'s {2} cured its poison problem!", (object) battler.ToString(false), (object) Game._INTL(battler.Ability.ToString(TextScripts.Name))));
                break;
              case Status.PARALYSIS:
                this.pbDisplay(Game._INTL("{1}'s {2} cured its paralysis!", (object) battler.ToString(false), (object) Game._INTL(battler.Ability.ToString(TextScripts.Name))));
                break;
              case Status.BURN:
                this.pbDisplay(Game._INTL("{1}'s {2} healed its burn!", (object) battler.ToString(false), (object) Game._INTL(battler.Ability.ToString(TextScripts.Name))));
                break;
              case Status.FROZEN:
                this.pbDisplay(Game._INTL("{1}'s {2} thawed it out!", (object) battler.ToString(false), (object) Game._INTL(battler.Ability.ToString(TextScripts.Name))));
                break;
            }
          }
          if (battler.hasWorkingAbility(Abilities.HEALER) && this.pbRandom(10) < 3)
          {
            IBattler pbPartner = battler.pbPartner;
            if (pbPartner.IsNotNullOrNone() && pbPartner.Status > Status.NONE)
            {
              GameDebug.Log("[Ability triggered] #" + battler.ToString(false) + "'s #" + Game._INTL(battler.Ability.ToString(TextScripts.Name)));
              Status status = pbPartner.Status;
              if (pbPartner is IBattlerEffect battlerEffect)
                battlerEffect.pbCureStatus(false);
              switch (status)
              {
                case Status.SLEEP:
                  this.pbDisplay(Game._INTL("{1}'s {2} cured its partner's sleep problem!", (object) battler.ToString(false), (object) Game._INTL(battler.Ability.ToString(TextScripts.Name))));
                  break;
                case Status.POISON:
                  this.pbDisplay(Game._INTL("{1}'s {2} cured its partner's poison problem!", (object) battler.ToString(false), (object) Game._INTL(battler.Ability.ToString(TextScripts.Name))));
                  break;
                case Status.PARALYSIS:
                  this.pbDisplay(Game._INTL("{1}'s {2} cured its partner's paralysis!", (object) battler.ToString(false), (object) Game._INTL(battler.Ability.ToString(TextScripts.Name))));
                  break;
                case Status.BURN:
                  this.pbDisplay(Game._INTL("{1}'s {2} healed its partner's burn!", (object) battler.ToString(false), (object) Game._INTL(battler.Ability.ToString(TextScripts.Name))));
                  break;
                case Status.FROZEN:
                  this.pbDisplay(Game._INTL("{1}'s {2} thawed its partner out!", (object) battler.ToString(false), (object) Game._INTL(battler.Ability.ToString(TextScripts.Name))));
                  break;
              }
            }
          }
        }
      }
      foreach (IBattler battler in this.priority)
      {
        if (!battler.isFainted())
        {
          if (this.field.GrassyTerrain > (byte) 0 && !battler.isAirborne() && battler.pbRecoverHP((int) Math.Floor((double) battler.TotalHP / 16.0), true) > 0)
            this.pbDisplay(Game._INTL("{1}'s HP was restored.", (object) battler.ToString(false)));
          battler.pbBerryCureCheck(true);
          if (battler.isFainted() && !battler.pbFaint())
            return;
        }
      }
      foreach (IBattler battler in this.priority)
      {
        if (!battler.isFainted() && battler.effects.AquaRing)
        {
          GameDebug.Log("[Lingering effect triggered] #" + battler.ToString(false) + "'s Aqua Ring");
          int amt = (int) Math.Floor((double) battler.TotalHP / 16.0);
          if (battler.hasWorkingItem(Items.BIG_ROOT))
            amt = (int) Math.Floor((double) amt * 1.3);
          if (battler.pbRecoverHP(amt, true) > 0)
            this.pbDisplay(Game._INTL("Aqua Ring restored {1}'s HP!", (object) battler.ToString(false)));
        }
      }
      foreach (IBattler battler in this.priority)
      {
        if (!battler.isFainted() && battler.effects.Ingrain)
        {
          GameDebug.Log("[Lingering effect triggered] #" + battler.ToString(false) + "'s Ingrain");
          int amt = (int) Math.Floor((double) battler.TotalHP / 16.0);
          if (battler.hasWorkingItem(Items.BIG_ROOT))
            amt = (int) Math.Floor((double) amt * 1.3);
          if (battler.pbRecoverHP(amt, true) > 0)
            this.pbDisplay(Game._INTL("{1} absorbed nutrients with its roots!", (object) battler.ToString(false)));
        }
      }
      foreach (IBattler opponent in this.priority)
      {
        if (!opponent.isFainted() && opponent.effects.LeechSeed >= 0 && !opponent.hasWorkingAbility(Abilities.MAGIC_GUARD))
        {
          IBattler battler = this.battlers[opponent.effects.LeechSeed];
          if (battler.IsNotNullOrNone() && !battler.isFainted())
          {
            GameDebug.Log("[Lingering effect triggered] #" + opponent.ToString(false) + "'s Leech Seed");
            this.pbCommonAnimation("LeechSeed", battler, opponent, 0);
            int amt = opponent.pbReduceHP((int) Math.Floor((double) opponent.TotalHP / 8.0), true);
            if (opponent.hasWorkingAbility(Abilities.LIQUID_OOZE))
            {
              battler.pbReduceHP(amt, true);
              this.pbDisplay(Game._INTL("{1} sucked up the liquid ooze!", (object) battler.ToString(false)));
            }
            else
            {
              if (battler.effects.HealBlock == 0)
              {
                if (battler.hasWorkingItem(Items.BIG_ROOT))
                  amt = (int) Math.Floor((double) amt * 1.3);
                battler.pbRecoverHP(amt, true);
              }
              this.pbDisplay(Game._INTL("{1}'s health was sapped by Leech Seed!", (object) opponent.ToString(false)));
            }
            if (opponent.isFainted() && !opponent.pbFaint() || battler.isFainted() && !battler.pbFaint())
              return;
          }
        }
      }
      foreach (IBattler attacker in this.priority)
      {
        if (!attacker.isFainted())
        {
          if (attacker.Status == Status.POISON)
          {
            if (attacker.StatusCount > 0)
            {
              ++attacker.effects.Toxic;
              attacker.effects.Toxic = Math.Min(15, attacker.effects.Toxic);
            }
            if (attacker.hasWorkingAbility(Abilities.POISON_HEAL))
            {
              this.pbCommonAnimation("Poison", attacker, (IBattler) null, 0);
              if (attacker.effects.HealBlock == 0 && attacker.HP < attacker.TotalHP)
              {
                GameDebug.Log("[Ability triggered] #" + attacker.ToString(false) + "'s Poison Heal");
                attacker.pbRecoverHP((int) Math.Floor((double) attacker.TotalHP / 8.0), true);
                this.pbDisplay(Game._INTL("{1} is healed by poison!", (object) attacker.ToString(false)));
              }
            }
            else if (!attacker.hasWorkingAbility(Abilities.MAGIC_GUARD))
            {
              GameDebug.Log("[Status damage] #" + attacker.ToString(false) + " took damage from poison/toxic");
              if (attacker.StatusCount == 0)
                attacker.pbReduceHP((int) Math.Floor((double) attacker.TotalHP / 8.0));
              else
                attacker.pbReduceHP((int) Math.Floor((double) (attacker.TotalHP * attacker.effects.Toxic) / 16.0));
              if (attacker is IBattlerEffect battlerEffect)
                battlerEffect.pbContinueStatus();
            }
          }
          if (attacker.Status == Status.BURN)
          {
            if (!attacker.hasWorkingAbility(Abilities.MAGIC_GUARD))
            {
              GameDebug.Log("[Status damage] #" + attacker.ToString(false) + " took damage from burn");
              if (attacker.hasWorkingAbility(Abilities.HEATPROOF))
              {
                GameDebug.Log("[Ability triggered] #" + attacker.ToString(false) + "'s Heatproof");
                attacker.pbReduceHP((int) Math.Floor((double) attacker.TotalHP / 16.0));
              }
              else
                attacker.pbReduceHP((int) Math.Floor((double) attacker.TotalHP / 8.0));
            }
            if (attacker is IBattlerEffect battlerEffect)
              battlerEffect.pbContinueStatus();
          }
          if (attacker.effects.Nightmare)
          {
            if (attacker.Status == Status.SLEEP)
            {
              if (!attacker.hasWorkingAbility(Abilities.MAGIC_GUARD))
              {
                GameDebug.Log("[Lingering effect triggered] #" + attacker.ToString(false) + "'s nightmare");
                attacker.pbReduceHP((int) Math.Floor((double) attacker.TotalHP / 4.0), true);
                this.pbDisplay(Game._INTL("{1} is locked in a nightmare!", (object) attacker.ToString(false)));
              }
            }
            else
              attacker.effects.Nightmare = false;
          }
          if (attacker.isFainted() && !attacker.pbFaint())
            return;
        }
      }
      foreach (IBattler battler in this.priority)
      {
        if (!battler.isFainted())
        {
          if (battler.effects.Curse && !battler.hasWorkingAbility(Abilities.MAGIC_GUARD))
          {
            GameDebug.Log("[Lingering effect triggered] #" + battler.ToString(false) + "'s curse");
            battler.pbReduceHP((int) Math.Floor((double) battler.TotalHP / 4.0), true);
            this.pbDisplay(Game._INTL("{1} is afflicted by the curse!", (object) battler.ToString(false)));
          }
          if (battler.isFainted() && !battler.pbFaint())
            return;
        }
      }
      foreach (IBattler battler in this.priority)
      {
        if (!battler.isFainted())
        {
          if (battler.effects.MultiTurn > 0)
          {
            --battler.effects.MultiTurn;
            string str = Game._INTL(battler.effects.MultiTurnAttack.ToString(TextScripts.Name));
            if (battler.effects.MultiTurn == 0)
            {
              GameDebug.Log("[End of effect] Trapping move #" + str + " affecting #" + battler.ToString(false) + " ended");
              this.pbDisplay(Game._INTL("{1} was freed from {2}!", (object) battler.ToString(false), (object) str));
            }
            else
            {
              if (battler.effects.MultiTurnAttack == Moves.BIND)
                this.pbCommonAnimation("Bind", battler, (IBattler) null, 0);
              else if (battler.effects.MultiTurnAttack == Moves.CLAMP)
                this.pbCommonAnimation("Clamp", battler, (IBattler) null, 0);
              else if (battler.effects.MultiTurnAttack == Moves.FIRE_SPIN)
                this.pbCommonAnimation("FireSpin", battler, (IBattler) null, 0);
              else if (battler.effects.MultiTurnAttack == Moves.MAGMA_STORM)
                this.pbCommonAnimation("MagmaStorm", battler, (IBattler) null, 0);
              else if (battler.effects.MultiTurnAttack == Moves.SAND_TOMB)
                this.pbCommonAnimation("SandTomb", battler, (IBattler) null, 0);
              else if (battler.effects.MultiTurnAttack == Moves.WRAP)
                this.pbCommonAnimation("Wrap", battler, (IBattler) null, 0);
              else if (battler.effects.MultiTurnAttack == Moves.INFESTATION)
                this.pbCommonAnimation("Infestation", battler, (IBattler) null, 0);
              else
                this.pbCommonAnimation("Wrap", battler, (IBattler) null, 0);
              if (!battler.hasWorkingAbility(Abilities.MAGIC_GUARD))
              {
                GameDebug.Log("[Lingering effect triggered] #" + battler.ToString(false) + " took damage from trapping move #" + str);
                (this.scene as IPokeBattle_DebugSceneNoGraphics).pbDamageAnimation(battler, TypeEffective.Ineffective);
                int amt = (int) Math.Floor((double) battler.TotalHP / 16.0);
                if (this.battlers[battler.effects.MultiTurnUser].hasWorkingItem(Items.BINDING_BAND))
                  amt = (int) Math.Floor((double) battler.TotalHP / 8.0);
                battler.pbReduceHP(amt);
                this.pbDisplay(Game._INTL("{1} is hurt by {2}!", (object) battler.ToString(false), (object) str));
              }
            }
          }
          if (battler.isFainted() && !battler.pbFaint())
            return;
        }
      }
      foreach (IBattler battler in this.priority)
      {
        if (!battler.isFainted() && battler.effects.Taunt > 0)
        {
          --battler.effects.Taunt;
          if (battler.effects.Taunt == 0)
          {
            this.pbDisplay(Game._INTL("{1}'s taunt wore off!", (object) battler.ToString(false)));
            GameDebug.Log("[End of effect] #" + battler.ToString(false) + " is no longer taunted");
          }
        }
      }
      foreach (IBattler battler in this.priority)
      {
        if (!battler.isFainted() && battler.effects.Encore > 0)
        {
          if (battler.moves[battler.effects.EncoreIndex].id != battler.effects.EncoreMove)
          {
            battler.effects.Encore = 0;
            battler.effects.EncoreIndex = 0;
            battler.effects.EncoreMove = Moves.NONE;
            GameDebug.Log("[End of effect] #" + battler.ToString(false) + " is no longer encored (encored move was lost)");
          }
          else
          {
            --battler.effects.Encore;
            if (battler.effects.Encore == 0 || battler.moves[battler.effects.EncoreIndex].PP == 0)
            {
              battler.effects.Encore = 0;
              this.pbDisplay(Game._INTL("{1}'s encore ended!", (object) battler.ToString(false)));
              GameDebug.Log("[End of effect] #" + battler.ToString(false) + " is no longer encored");
            }
          }
        }
      }
      foreach (IBattler battler in this.priority)
      {
        if (!battler.isFainted() && battler.effects.Disable > 0)
        {
          --battler.effects.Disable;
          if (battler.effects.Disable == 0)
          {
            battler.effects.DisableMove = Moves.NONE;
            this.pbDisplay(Game._INTL("{1} is no longer disabled!", (object) battler.ToString(false)));
            GameDebug.Log("[End of effect] #" + battler.ToString(false) + " is no longer disabled");
          }
        }
      }
      foreach (IBattler battler in this.priority)
      {
        if (!battler.isFainted() && battler.effects.MagnetRise > 0)
        {
          --battler.effects.MagnetRise;
          if (battler.effects.MagnetRise == 0)
          {
            this.pbDisplay(Game._INTL("{1} stopped levitating.", (object) battler.ToString(false)));
            GameDebug.Log("[End of effect] #" + battler.ToString(false) + " is no longer levitating by Magnet Rise");
          }
        }
      }
      foreach (IBattler battler in this.priority)
      {
        if (!battler.isFainted() && battler.effects.Telekinesis > 0)
        {
          --battler.effects.Telekinesis;
          if (battler.effects.Telekinesis == 0)
          {
            this.pbDisplay(Game._INTL("{1} stopped levitating.", (object) battler.ToString(false)));
            GameDebug.Log("[End of effect] #" + battler.ToString(false) + " is no longer levitating by Telekinesis");
          }
        }
      }
      foreach (IBattler battler in this.priority)
      {
        if (!battler.isFainted() && battler.effects.HealBlock > 0)
        {
          --battler.effects.HealBlock;
          if (battler.effects.HealBlock == 0)
          {
            this.pbDisplay(Game._INTL("{1}'s Heal Block wore off!", (object) battler.ToString(false)));
            GameDebug.Log("[End of effect] #" + battler.ToString(false) + " is no longer Heal Blocked");
          }
        }
      }
      foreach (IBattler battler in this.priority)
      {
        if (!battler.isFainted() && battler.effects.Embargo > 0)
        {
          --battler.effects.Embargo;
          if (battler.effects.Embargo == 0)
          {
            this.pbDisplay(Game._INTL("{1} can use items again!", (object) battler.ToString(true)));
            GameDebug.Log("[End of effect] #" + battler.ToString(false) + " is no longer affected by an embargo");
          }
        }
      }
      foreach (IBattler battler in this.priority)
      {
        if (!battler.isFainted() && battler.effects.Yawn > 0)
        {
          --battler.effects.Yawn;
          if (battler.effects.Yawn == 0 && battler is IBattlerClause battlerClause && battlerClause.pbCanSleepYawn())
          {
            GameDebug.Log("[Lingering effect triggered] #" + battler.ToString(false) + "'s Yawn");
            if (battler is IBattlerEffect battlerEffect)
              battlerEffect.pbSleep();
          }
        }
      }
      List<int> source = new List<int>();
      foreach (IBattler battler in this.priority)
      {
        if (!battler.isFainted())
        {
          if (battler.effects.PerishSong > 0)
          {
            --battler.effects.PerishSong;
            this.pbDisplay(Game._INTL("{1}'s perish count fell to {2}!", (object) battler.ToString(false), (object) battler.effects.PerishSong.ToString()));
            GameDebug.Log(string.Format("[Lingering effect triggered] #{0}'s Perish Song count dropped to #{1}", (object) battler.ToString(false), (object) battler.effects.PerishSong));
            if (battler.effects.PerishSong == 0)
            {
              source.Add(battler.effects.PerishSongUser);
              battler.pbReduceHP(battler.HP, true);
            }
          }
          if (battler.isFainted() && !battler.pbFaint())
            return;
        }
      }
      if (source.Count > 0 && (source.Count<int>((Func<int, bool>) (item => this.pbIsOpposing(item))) == source.Count || source.Count<int>((Func<int, bool>) (item => !this.pbIsOpposing(item))) == source.Count))
        this.pbJudgeCheckpoint(this.battlers[source[0]], (IBattleMove) null);
      if (this.decision > BattleResults.ABORTED)
      {
        this.pbGainEXP();
      }
      else
      {
        for (int index = 0; index < this.sides.Length; ++index)
        {
          if (this.sides[index].Reflect > (byte) 0)
          {
            --this.sides[index].Reflect;
            if (this.sides[index].Reflect == (byte) 0)
            {
              if (index == 0)
                this.pbDisplay(Game._INTL("Your team's Reflect faded!"));
              if (index == 1)
                this.pbDisplay(Game._INTL("The opposing team's Reflect faded!"));
              if (index == 0)
                GameDebug.Log("[End of effect] Reflect ended on the player's side");
              if (index == 1)
                GameDebug.Log("[End of effect] Reflect ended on the opponent's side");
            }
          }
        }
        for (int index = 0; index < this.sides.Length; ++index)
        {
          if (this.sides[index].LightScreen > (byte) 0)
          {
            --this.sides[index].LightScreen;
            if (this.sides[index].LightScreen == (byte) 0)
            {
              if (index == 0)
                this.pbDisplay(Game._INTL("Your team's Light Screen faded!"));
              if (index == 1)
                this.pbDisplay(Game._INTL("The opposing team's Light Screen faded!"));
              if (index == 0)
                GameDebug.Log("[End of effect] Light Screen ended on the player's side");
              if (index == 1)
                GameDebug.Log("[End of effect] Light Screen ended on the opponent's side");
            }
          }
        }
        for (int index = 0; index < this.sides.Length; ++index)
        {
          if (this.sides[index].Safeguard > (byte) 0)
          {
            --this.sides[index].Safeguard;
            if (this.sides[index].Safeguard == (byte) 0)
            {
              if (index == 0)
                this.pbDisplay(Game._INTL("Your team is no longer protected by Safeguard!"));
              if (index == 1)
                this.pbDisplay(Game._INTL("The opposing team is no longer protected by Safeguard!"));
              if (index == 0)
                GameDebug.Log("[End of effect] Safeguard ended on the player's side");
              if (index == 1)
                GameDebug.Log("[End of effect] Safeguard ended on the opponent's side");
            }
          }
        }
        for (int index = 0; index < this.sides.Length; ++index)
        {
          if (this.sides[index].Mist > (byte) 0)
          {
            --this.sides[index].Mist;
            if (this.sides[index].Mist == (byte) 0)
            {
              if (index == 0)
                this.pbDisplay(Game._INTL("Your team's Mist faded!"));
              if (index == 1)
                this.pbDisplay(Game._INTL("The opposing team's Mist faded!"));
              if (index == 0)
                GameDebug.Log("[End of effect] Mist ended on the player's side");
              if (index == 1)
                GameDebug.Log("[End of effect] Mist ended on the opponent's side");
            }
          }
        }
        for (int index = 0; index < this.sides.Length; ++index)
        {
          if (this.sides[index].Tailwind > (byte) 0)
          {
            --this.sides[index].Tailwind;
            if (this.sides[index].Tailwind == (byte) 0)
            {
              if (index == 0)
                this.pbDisplay(Game._INTL("Your team's Tailwind petered out!"));
              if (index == 1)
                this.pbDisplay(Game._INTL("The opposing team's Tailwind petered out!"));
              if (index == 0)
                GameDebug.Log("[End of effect] Tailwind ended on the player's side");
              if (index == 1)
                GameDebug.Log("[End of effect] Tailwind ended on the opponent's side");
            }
          }
        }
        for (int index = 0; index < this.sides.Length; ++index)
        {
          if (this.sides[index].LuckyChant > (byte) 0)
          {
            --this.sides[index].LuckyChant;
            if (this.sides[index].LuckyChant == (byte) 0)
            {
              if (index == 0)
                this.pbDisplay(Game._INTL("Your team's Lucky Chant faded!"));
              if (index == 1)
                this.pbDisplay(Game._INTL("The opposing team's Lucky Chant faded!"));
              if (index == 0)
                GameDebug.Log("[End of effect] Lucky Chant ended on the player's side");
              if (index == 1)
                GameDebug.Log("[End of effect] Lucky Chant ended on the opponent's side");
            }
          }
        }
        for (int index = 0; index < this.sides.Length; ++index)
        {
          if (this.sides[index].Swamp > (byte) 0)
          {
            --this.sides[index].Swamp;
            if (this.sides[index].Swamp == (byte) 0)
            {
              if (index == 0)
                this.pbDisplay(Game._INTL("The swamp around your team disappeared!"));
              if (index == 1)
                this.pbDisplay(Game._INTL("The swamp around the opposing team disappeared!"));
              if (index == 0)
                GameDebug.Log("[End of effect] Grass Pledge's swamp ended on the player's side");
              if (index == 1)
                GameDebug.Log("[End of effect] Grass Pledge's swamp ended on the opponent's side");
            }
          }
          if (this.sides[index].SeaOfFire > (byte) 0)
          {
            --this.sides[index].SeaOfFire;
            if (this.sides[index].SeaOfFire == (byte) 0)
            {
              if (index == 0)
                this.pbDisplay(Game._INTL("The sea of fire around your team disappeared!"));
              if (index == 1)
                this.pbDisplay(Game._INTL("The sea of fire around the opposing team disappeared!"));
              if (index == 0)
                GameDebug.Log("[End of effect] Fire Pledge's sea of fire ended on the player's side");
              if (index == 1)
                GameDebug.Log("[End of effect] Fire Pledge's sea of fire ended on the opponent's side");
            }
          }
          if (this.sides[index].Rainbow > (byte) 0)
          {
            --this.sides[index].Rainbow;
            if (this.sides[index].Rainbow == (byte) 0)
            {
              if (index == 0)
                this.pbDisplay(Game._INTL("The rainbow around your team disappeared!"));
              if (index == 1)
                this.pbDisplay(Game._INTL("The rainbow around the opposing team disappeared!"));
              if (index == 0)
                GameDebug.Log("[End of effect] Water Pledge's rainbow ended on the player's side");
              if (index == 1)
                GameDebug.Log("[End of effect] Water Pledge's rainbow ended on the opponent's side");
            }
          }
        }
        if (this.field.Gravity > (byte) 0)
        {
          --this.field.Gravity;
          if (this.field.Gravity == (byte) 0)
          {
            this.pbDisplay(Game._INTL("Gravity returned to normal."));
            GameDebug.Log("[End of effect] Strong gravity ended");
          }
        }
        if (this.field.TrickRoom > (byte) 0)
        {
          --this.field.TrickRoom;
          if (this.field.TrickRoom == (byte) 0)
          {
            this.pbDisplay(Game._INTL("The twisted dimensions returned to normal."));
            GameDebug.Log("[End of effect] Trick Room ended");
          }
        }
        if (this.field.WonderRoom > (byte) 0)
        {
          --this.field.WonderRoom;
          if (this.field.WonderRoom == (byte) 0)
          {
            this.pbDisplay(Game._INTL("Wonder Room wore off, and the Defense and Sp. public void stats returned to normal!"));
            GameDebug.Log("[End of effect] Wonder Room ended");
          }
        }
        if (this.field.MagicRoom > (byte) 0)
        {
          --this.field.MagicRoom;
          if (this.field.MagicRoom == (byte) 0)
          {
            this.pbDisplay(Game._INTL("The area returned to normal."));
            GameDebug.Log("[End of effect] Magic Room ended");
          }
        }
        if (this.field.MudSportField > (byte) 0)
        {
          --this.field.MudSportField;
          if (this.field.MudSportField == (byte) 0)
          {
            this.pbDisplay(Game._INTL("The effects of Mud Sport have faded."));
            GameDebug.Log("[End of effect] Mud Sport ended");
          }
        }
        if (this.field.WaterSportField > (byte) 0)
        {
          --this.field.WaterSportField;
          if (this.field.WaterSportField == (byte) 0)
          {
            this.pbDisplay(Game._INTL("The effects of Water Sport have faded."));
            GameDebug.Log("[End of effect] Water Sport ended");
          }
        }
        if (this.field.ElectricTerrain > (byte) 0)
        {
          --this.field.ElectricTerrain;
          if (this.field.ElectricTerrain == (byte) 0)
          {
            this.pbDisplay(Game._INTL("The electric current disappeared from the battlefield."));
            GameDebug.Log("[End of effect] Electric Terrain ended");
          }
        }
        if (this.field.GrassyTerrain > (byte) 0)
        {
          --this.field.GrassyTerrain;
          if (this.field.GrassyTerrain == (byte) 0)
          {
            this.pbDisplay(Game._INTL("The grass disappeared from the battlefield."));
            GameDebug.Log("[End of effect] Grassy Terrain ended");
          }
        }
        if (this.field.MistyTerrain > (byte) 0)
        {
          --this.field.MistyTerrain;
          if (this.field.MistyTerrain == (byte) 0)
          {
            this.pbDisplay(Game._INTL("The mist disappeared from the battlefield."));
            GameDebug.Log("[End of effect] Misty Terrain ended");
          }
        }
        foreach (IBattler battler3 in this.priority)
        {
          if (!battler3.isFainted() && battler3.effects.Uproar > 0)
          {
            foreach (IBattler battler4 in this.priority)
            {
              if (!battler4.isFainted() && battler4.Status == Status.SLEEP && !battler4.hasWorkingAbility(Abilities.SOUNDPROOF))
              {
                GameDebug.Log("[Lingering effect triggered] Uproar woke up #" + battler4.ToString(true));
                if (battler4 is IBattlerEffect battlerEffect)
                  battlerEffect.pbCureStatus(false);
                this.pbDisplay(Game._INTL("{1} woke up in the uproar!", (object) battler4.ToString(false)));
              }
            }
            --battler3.effects.Uproar;
            if (battler3.effects.Uproar == 0)
            {
              this.pbDisplay(Game._INTL("{1} calmed down.", (object) battler3.ToString(false)));
              GameDebug.Log("[End of effect] #" + battler3.ToString(false) + " is no longer uproaring");
            }
            else
              this.pbDisplay(Game._INTL("{1} is making an uproar!", (object) battler3.ToString(false)));
          }
        }
        foreach (IBattler attacker in this.priority)
        {
          if (!attacker.isFainted())
          {
            if (attacker.turncount > 0 && attacker.hasWorkingAbility(Abilities.SPEED_BOOST) && attacker is IBattlerEffect battlerEffect1 && battlerEffect1.pbIncreaseStatWithCause(Stats.SPEED, 1, attacker, Game._INTL(attacker.Ability.ToString(TextScripts.Name))))
              GameDebug.Log("[Ability triggered] #" + attacker.ToString(false) + "'s #" + Game._INTL(attacker.Ability.ToString(TextScripts.Name)));
            if (attacker.Status == Status.SLEEP && !attacker.hasWorkingAbility(Abilities.MAGIC_GUARD) && (attacker.pbOpposing1.hasWorkingAbility(Abilities.BAD_DREAMS) || attacker.pbOpposing2.hasWorkingAbility(Abilities.BAD_DREAMS)))
            {
              GameDebug.Log("[Ability triggered] #" + attacker.ToString(false) + "'s opponent's Bad Dreams");
              if (attacker.pbReduceHP((int) Math.Floor((double) attacker.TotalHP / 8.0), true) > 0)
                this.pbDisplay(Game._INTL("{1} is having a bad dream!", (object) attacker.ToString(false)));
            }
            if (attacker.isFainted())
            {
              if (!attacker.pbFaint())
                return;
            }
            else
            {
              if (attacker.hasWorkingAbility(Abilities.PICKUP) && attacker.Item <= Items.NONE)
              {
                Items items = Items.NONE;
                int index1 = -1;
                int num = 0;
                for (int index2 = 0; index2 < this.battlers.Length; ++index2)
                {
                  if (index2 != attacker.Index && this.battlers[index2].effects.PickupUse > num)
                  {
                    items = this.battlers[index2].effects.PickupItem;
                    index1 = index2;
                    num = this.battlers[index2].effects.PickupUse;
                  }
                }
                if (items > Items.NONE)
                {
                  attacker.Item = items;
                  this.battlers[index1].effects.PickupItem = Items.NONE;
                  this.battlers[index1].effects.PickupUse = 0;
                  if (this.battlers[index1].pokemon.itemRecycle == items)
                    this.battlers[index1].pokemon.itemRecycle = Items.NONE;
                  if (this.opponent.Length == 0 && attacker.pokemon.itemInitial == Items.NONE && this.battlers[index1].pokemon.itemInitial == items)
                  {
                    attacker.pokemon.itemInitial = items;
                    this.battlers[index1].pokemon.itemInitial = Items.NONE;
                  }
                  this.pbDisplay(Game._INTL("{1} found one {2}!", (object) attacker.ToString(false), (object) Game._INTL(items.ToString(TextScripts.Name))));
                  attacker.pbBerryCureCheck(true);
                }
              }
              if (attacker.hasWorkingAbility(Abilities.HARVEST) && attacker.Item <= Items.NONE && attacker.pokemon.itemRecycle > Items.NONE && Kernal.ItemData[attacker.pokemon.itemRecycle].IsBerry && (this.pbWeather == Weather.SUNNYDAY || this.pbWeather == Weather.HARSHSUN || this.pbRandom(10) < 5))
              {
                attacker.Item = attacker.pokemon.itemRecycle;
                attacker.pokemon.itemRecycle = Items.NONE;
                if (attacker.pokemon.itemInitial == Items.NONE)
                  attacker.pokemon.itemInitial = attacker.Item;
                this.pbDisplay(Game._INTL("{1} harvested one {2}!", (object) attacker.ToString(false), (object) Game._INTL(attacker.Item.ToString(TextScripts.Name))));
                attacker.pbBerryCureCheck(true);
              }
              if (attacker.hasWorkingAbility(Abilities.MOODY))
              {
                List<Stats> statsList1 = new List<Stats>();
                List<Stats> statsList2 = new List<Stats>();
                Stats[] statsArray = new Stats[7]
                {
                  Stats.ATTACK,
                  Stats.DEFENSE,
                  Stats.SPEED,
                  Stats.SPATK,
                  Stats.SPDEF,
                  Stats.ACCURACY,
                  Stats.EVASION
                };
                foreach (Stats stat in statsArray)
                {
                  if (attacker is IBattlerEffect battlerEffect2 && battlerEffect2.pbCanIncreaseStatStage(stat, attacker))
                    statsList1.Add(stat);
                  if (attacker is IBattlerEffect battlerEffect3 && battlerEffect3.pbCanReduceStatStage(stat, attacker))
                    statsList2.Add(stat);
                }
                if (statsList1.Count > 0)
                {
                  GameDebug.Log("[Ability triggered] #" + attacker.ToString(false) + "'s Moody (raise stat)");
                  int index3 = this.pbRandom(statsList1.Count);
                  if (attacker is IBattlerEffect battlerEffect4)
                    battlerEffect4.pbIncreaseStatWithCause(statsList1[index3], 2, attacker, Game._INTL(attacker.Ability.ToString(TextScripts.Name)));
                  for (int index4 = 0; index4 < statsList2.Count; ++index4)
                  {
                    if (statsList2[index4] == statsList1[index3])
                    {
                      statsList2.RemoveAt(index4);
                      break;
                    }
                  }
                }
                if (statsList2.Count > 0)
                {
                  GameDebug.Log("[Ability triggered] #" + attacker.ToString(false) + "'s Moody (lower stat)");
                  int index = this.pbRandom(statsList2.Count);
                  if (attacker is IBattlerEffect battlerEffect5)
                    battlerEffect5.pbReduceStatWithCause(statsList2[index], 1, attacker, Game._INTL(attacker.Ability.ToString(TextScripts.Name)));
                }
              }
            }
          }
        }
        foreach (IBattler pkmn in this.priority)
        {
          if (!pkmn.isFainted())
          {
            if (pkmn.hasWorkingItem(Items.TOXIC_ORB) && pkmn.Status == Status.NONE && pkmn is IBattlerEffect battlerEffect6 && battlerEffect6.pbCanPoison((IBattler) null, false))
            {
              GameDebug.Log("[Item triggered] #" + pkmn.ToString(false) + "'s Toxic Orb");
              battlerEffect6.pbPoison((IBattler) null, Game._INTL("{1} was badly poisoned by its {2}!", (object) pkmn.ToString(false), (object) Game._INTL(pkmn.Item.ToString(TextScripts.Name))), true);
            }
            if (pkmn.hasWorkingItem(Items.FLAME_ORB) && pkmn.Status == Status.NONE && pkmn is IBattlerEffect battlerEffect7 && battlerEffect7.pbCanBurn((IBattler) null, false))
            {
              GameDebug.Log("[Item triggered] #" + pkmn.ToString(false) + "'s Flame Orb");
              battlerEffect7.pbBurn((IBattler) null, Game._INTL("{1} was burned by its {2}!", (object) pkmn.ToString(false), (object) Game._INTL(pkmn.Item.ToString(TextScripts.Name))));
            }
            if (pkmn.hasWorkingItem(Items.STICKY_BARB) && !pkmn.hasWorkingAbility(Abilities.MAGIC_GUARD))
            {
              GameDebug.Log("[Item triggered] #" + pkmn.ToString(false) + "'s Sticky Barb");
              (this.scene as IPokeBattle_DebugSceneNoGraphics).pbDamageAnimation(pkmn, TypeEffective.Ineffective);
              pkmn.pbReduceHP((int) Math.Floor((double) pkmn.TotalHP / 8.0));
              this.pbDisplay(Game._INTL("{1} is hurt by its {2}!", (object) pkmn.ToString(false), (object) Game._INTL(pkmn.Item.ToString(TextScripts.Name))));
            }
            if (pkmn.isFainted() && !pkmn.pbFaint())
              return;
          }
        }
        for (int index = 0; index < this.battlers.Length; ++index)
        {
          if (!this.battlers[index].isFainted())
            this.battlers[index].pbCheckForm();
        }
        this.pbGainEXP();
        this.pbSwitch(false);
        if (this.decision > BattleResults.ABORTED)
          return;
        foreach (IBattler battler in this.priority)
        {
          if (!battler.isFainted())
            battler.pbAbilitiesOnSwitchIn(false);
        }
        for (int index = 0; index < this.battlers.Length; ++index)
        {
          if (this.battlers[index].turncount > 0 && this.battlers[index].hasWorkingAbility(Abilities.TRUANT))
            this.battlers[index].effects.Truant = !this.battlers[index].effects.Truant;
          if (this.battlers[index].effects.LockOn > 0)
          {
            --this.battlers[index].effects.LockOn;
            if (this.battlers[index].effects.LockOn == 0)
              this.battlers[index].effects.LockOnPos = -1;
          }
          this.battlers[index].effects.Flinch = false;
          this.battlers[index].effects.FollowMe = 0;
          this.battlers[index].effects.HelpingHand = false;
          this.battlers[index].effects.MagicCoat = false;
          this.battlers[index].effects.Snatch = false;
          if (this.battlers[index].effects.Charge > 0)
            --this.battlers[index].effects.Charge;
          this.battlers[index].lastHPLost = 0;
          this.battlers[index].tookDamage = false;
          this.battlers[index].lastAttacker.Clear();
          this.battlers[index].effects.Counter = -1;
          this.battlers[index].effects.CounterTarget = -1;
          this.battlers[index].effects.MirrorCoat = -1;
          this.battlers[index].effects.MirrorCoatTarget = -1;
        }
        for (int index = 0; index < this.sides.Length; ++index)
        {
          if (!this.sides[index].EchoedVoiceUsed)
            this.sides[index].EchoedVoiceCounter = (byte) 0;
          this.sides[index].EchoedVoiceUsed = false;
          this.sides[index].QuickGuard = false;
          this.sides[index].WideGuard = false;
          this.sides[index].CraftyShield = false;
          this.sides[index].Round = (byte) 0;
        }
        this.field.FusionBolt = false;
        this.field.FusionFlare = false;
        this.field.IonDeluge = false;
        if (this.field.FairyLock > (byte) 0)
          --this.field.FairyLock;
        this.usepriority = false;
      }
    }

    public BattleResults pbEndOfBattle(bool canlose = false)
    {
      if (this.decision == BattleResults.WON)
      {
        GameDebug.Log("");
        GameDebug.Log("***Player won***");
        if ((uint) this.opponent.Length > 0U)
        {
          (this.scene as IPokeBattle_DebugSceneNoGraphics).pbTrainerBattleSuccess();
          if ((uint) this.opponent.Length > 0U)
            this.pbDisplayPaused(Game._INTL("{1} defeated {2} and {3}!", (object) this.pbPlayer().name, (object) this.opponent[0].name, (object) this.opponent[1].name));
          else
            this.pbDisplayPaused(Game._INTL("{1} defeated\r\n{2}!", (object) this.pbPlayer().name, (object) this.opponent[0].name));
          (this.scene as IPokeBattle_DebugSceneNoGraphics).pbShowOpponent(0);
          this.pbDisplayPaused(this.endspeech.Replace("/\\[Pp][Nn]/", this.pbPlayer().name));
          if ((uint) this.opponent.Length > 0U)
          {
            (this.scene as IPokeBattle_DebugSceneNoGraphics).pbHideOpponent();
            (this.scene as IPokeBattle_DebugSceneNoGraphics).pbShowOpponent(1);
            this.pbDisplayPaused(this.endspeech2.Replace("/\\[Pp][Nn]/", this.pbPlayer().name));
          }
          if (this.internalbattle)
          {
            int num1 = 0;
            int num2;
            if (this.opponent.Length > 1)
            {
              int num3 = 0;
              int num4 = 0;
              int num5 = this.pbSecondPartyBegin(1);
              for (int index = 0; index < num5; ++index)
              {
                if (this.party2[index].IsNotNullOrNone() && num3 < this.party2[index].Level)
                  num3 = this.party2[index].Level;
                if (this.party2[index + num5].IsNotNullOrNone() && num3 < this.party2[index + num5].Level)
                  num4 = this.party2[index + num5].Level;
              }
              num2 = num1 + num3 * this.opponent[0].Money + num4 * this.opponent[1].Money;
            }
            else
            {
              int num6 = 0;
              foreach (IPokemon pokemon in this.party2)
              {
                if (pokemon.IsNotNullOrNone() && num6 < pokemon.Level)
                  num6 = pokemon.Level;
              }
              num2 = num1 + num6 * this.opponent[0].Money;
            }
            if (this.amuletcoin)
              num2 *= 2;
            if (this.doublemoney)
              num2 *= 2;
            int money = this.pbPlayer().Money;
            this.pbPlayer().Money += num2;
            if (this.pbPlayer().Money - money > 0)
              this.pbDisplayPaused(Game._INTL("{1} got ${2}\r\nfor winning!", (object) this.pbPlayer().name, (object) num2.ToString()));
          }
        }
        if (this.internalbattle && this.extramoney > 0)
        {
          if (this.amuletcoin)
            this.extramoney *= 2;
          if (this.doublemoney)
            this.extramoney *= 2;
          int money = this.pbPlayer().Money;
          this.pbPlayer().Money += this.extramoney;
          if (this.pbPlayer().Money - money > 0)
            this.pbDisplayPaused(Game._INTL("{1} picked up ${2}!", (object) this.pbPlayer().name, (object) this.extramoney.ToString()));
        }
        foreach (int index in this.snaggedpokemon)
        {
          IPokemon pokemon = this.party2[index];
          this.pbStorePokemon(pokemon);
          if (this.pbPlayer().shadowcaught == null)
            this.pbPlayer().shadowcaught = (IList<Pokemons>) new List<Pokemons>();
          if (!this.pbPlayer().shadowcaught.Contains(pokemon.Species))
            this.pbPlayer().shadowcaught.Add(pokemon.Species);
        }
        this.snaggedpokemon.Clear();
      }
      else if (this.decision == BattleResults.LOST || this.decision == BattleResults.DRAW)
      {
        GameDebug.Log("");
        if (this.decision == BattleResults.LOST)
          GameDebug.Log("***Player lost***");
        if (this.decision == BattleResults.DRAW)
          GameDebug.Log("***Player drew with opponent***");
        if (this.internalbattle)
        {
          this.pbDisplayPaused(Game._INTL("{1} is out of usable Pokémon!", (object) this.pbPlayer().name));
          int num7 = this.pbMaxLevelFromIndex(0);
          int[] numArray = new int[9]
          {
            8,
            16,
            24,
            36,
            48,
            60,
            80,
            100,
            120
          };
          if (num7 * numArray[Math.Min(numArray.Length - 1, this.pbPlayer().badges.Length)] > this.pbPlayer().Money)
          {
            int money1 = this.pbPlayer().Money;
          }
          int num8 = 0;
          int money2 = this.pbPlayer().Money;
          this.pbPlayer().Money -= num8;
          int num9 = money2 - this.pbPlayer().Money;
          if ((uint) this.opponent.Length > 0U)
          {
            if ((uint) this.opponent.Length > 0U)
              this.pbDisplayPaused(Game._INTL("{1} lost against {2} and {3}!", (object) this.pbPlayer().name, (object) this.opponent[0].name, (object) this.opponent[1].name));
            else
              this.pbDisplayPaused(Game._INTL("{1} lost against\r\n{2}!", (object) this.pbPlayer().name, (object) this.opponent[0].name));
            if (num8 > 0)
            {
              this.pbDisplayPaused(Game._INTL("{1} paid ${2}\r\nas the prize money...", (object) this.pbPlayer().name, (object) num9.ToString()));
              if (!canlose)
                this.pbDisplayPaused(Game._INTL("..."));
            }
          }
          else if (num8 > 0)
          {
            this.pbDisplayPaused(Game._INTL("{1} panicked and lost\r\n${2}...", (object) this.pbPlayer().name, (object) num9.ToString()));
            if (!canlose)
              this.pbDisplayPaused(Game._INTL("..."));
          }
          if (!canlose)
            this.pbDisplayPaused(Game._INTL("{1} blacked out!", (object) this.pbPlayer().name));
        }
        else if (this.decision == BattleResults.LOST)
        {
          (this.scene as IPokeBattle_DebugSceneNoGraphics).pbShowOpponent(0);
          this.pbDisplayPaused(this.endspeechwin.Replace("/\\[Pp][Nn]/", this.pbPlayer().name));
          if ((uint) this.opponent.Length > 0U)
          {
            (this.scene as IPokeBattle_DebugSceneNoGraphics).pbHideOpponent();
            (this.scene as IPokeBattle_DebugSceneNoGraphics).pbShowOpponent(1);
            this.pbDisplayPaused(this.endspeechwin2.Replace("/\\[Pp][Nn]/", this.pbPlayer().name));
          }
        }
      }
      List<int> intList = new List<int>();
      bool? pokerusStage;
      for (int index = 0; index < Game.GameData.Trainer.party.Length; ++index)
      {
        pokerusStage = Game.GameData.Trainer.party[index].PokerusStage;
        int num;
        if (pokerusStage.HasValue)
        {
          pokerusStage = Game.GameData.Trainer.party[index].PokerusStage;
          num = pokerusStage.Value ? 1 : 0;
        }
        else
          num = 0;
        if (num != 0)
          intList.Add(index);
      }
      if (intList.Count >= 1)
      {
        foreach (int index in intList)
        {
          int pokerusStrain = (Game.GameData.Trainer.party[index] as PokemonUnity.Monster.Pokemon).PokerusStrain;
          int num10;
          if (index > 0)
          {
            pokerusStage = Game.GameData.Trainer.party[index - 1].PokerusStage;
            num10 = !pokerusStage.HasValue ? 1 : 0;
          }
          else
            num10 = 0;
          if (num10 != 0 && this.pbRandom(3) == 0)
            Game.GameData.Trainer.party[index - 1].GivePokerus(pokerusStrain);
          int num11;
          if (index < Game.GameData.Trainer.party.Length - 1)
          {
            pokerusStage = Game.GameData.Trainer.party[index + 1].PokerusStage;
            num11 = !pokerusStage.HasValue ? 1 : 0;
          }
          else
            num11 = 0;
          if (num11 != 0 && this.pbRandom(3) == 0)
            Game.GameData.Trainer.party[index + 1].GivePokerus(pokerusStrain);
        }
      }
      (this.scene as IPokeBattle_DebugSceneNoGraphics).pbEndBattle(this.decision);
      foreach (IBattler battler in this.battlers)
      {
        battler.pbResetForm();
        if (battler.hasWorkingAbility(Abilities.NATURAL_CURE))
          battler.Status = Status.NONE;
      }
      foreach (IPokemon pokemon in Game.GameData.Trainer.party)
      {
        pokemon.setItem(pokemon.itemInitial);
        pokemon.itemInitial = pokemon.itemRecycle = Items.NONE;
        pokemon.belch = false;
      }
      return this.decision;
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      info.AddValue("scene", (object) this.scene, typeof (IScene));
      info.AddValue("decision", (object) this.decision, typeof (BattleResults));
      info.AddValue("internalbattle", (object) this.internalbattle, typeof (bool));
      info.AddValue("doublebattle", (object) this.doublebattle, typeof (bool));
      info.AddValue("cantescape", (object) this.cantescape, typeof (bool));
      info.AddValue("canLose", (object) this.canLose, typeof (bool));
      info.AddValue("shiftStyle", (object) this.shiftStyle, typeof (bool));
      info.AddValue("battlescene", (object) this.battlescene, typeof (bool));
      info.AddValue("debug", (object) this.debug, typeof (bool));
      info.AddValue("debugupdate", (object) this.debugupdate, typeof (int));
      info.AddValue("player", (object) this.player, typeof (ITrainer[]));
      info.AddValue("opponent", (object) this.opponent, typeof (ITrainer[]));
      info.AddValue("party1", (object) this.party1, typeof (IPokemon[]));
      info.AddValue("party2", (object) this.party2, typeof (IPokemon[]));
      info.AddValue("party1order", (object) this.party1order, typeof (IList<int>));
      info.AddValue("party2order", (object) this.party2order, typeof (IList<int>));
      info.AddValue("fullparty1", (object) this.fullparty1, typeof (bool));
      info.AddValue("fullparty2", (object) this.fullparty2, typeof (bool));
      info.AddValue("battlers", (object) this.battlers, typeof (IBattler[]));
      info.AddValue("items", (object) this.items, typeof (Items[][]));
      info.AddValue("sides", (object) this.sides, typeof (IEffectsSide[]));
      info.AddValue("field", (object) this.field, typeof (IEffectsField));
      info.AddValue("environment", (object) this.environment, typeof (Environments));
      info.AddValue("weather", (object) this.weather, typeof (Weather));
      info.AddValue("weatherduration", (object) this.weatherduration, typeof (int));
      info.AddValue("switching", (object) this.switching, typeof (bool));
      info.AddValue("futuresight", (object) this.futuresight, typeof (bool));
      info.AddValue("struggle", (object) this.struggle, typeof (IBattleMove));
      info.AddValue("choices", (object) this.choices, typeof (IBattleChoice[]));
      info.AddValue("successStates", (object) this.successStates, typeof (ISuccessState[]));
      info.AddValue("lastMoveUsed", (object) this.lastMoveUsed, typeof (Moves));
      info.AddValue("lastMoveUser", (object) this.lastMoveUser, typeof (int));
      info.AddValue("megaEvolution", (object) this.megaEvolution, typeof (int[][]));
      info.AddValue("amuletcoin", (object) this.amuletcoin, typeof (bool));
      info.AddValue("extramoney", (object) this.extramoney, typeof (int));
      info.AddValue("doublemoney", (object) this.doublemoney, typeof (bool));
      info.AddValue("endspeech", (object) this.endspeech, typeof (string));
      info.AddValue("endspeech2", (object) this.endspeech2, typeof (string));
      info.AddValue("endspeechwin", (object) this.endspeechwin, typeof (string));
      info.AddValue("endspeechwin2", (object) this.endspeechwin2, typeof (string));
      info.AddValue("rules", (object) this.rules, typeof (IDictionary<string, bool>));
      info.AddValue("turncount", (object) this.turncount, typeof (int));
      info.AddValue("priority", (object) this.priority, typeof (IBattler[]));
      info.AddValue("snaggedpokemon", (object) this.snaggedpokemon, typeof (List<int>));
      info.AddValue("runCommand", (object) this.runCommand, typeof (int));
      info.AddValue("pickupUse", (object) this.pickupUse, typeof (int));
      info.AddValue("controlPlayer", (object) this.controlPlayer, typeof (bool));
      info.AddValue("usepriority", (object) this.usepriority, typeof (bool));
      info.AddValue("peer", (object) this.peer, typeof (IBattlePeer));
    }

    IEnumerator IBattle.pbDebugUpdate() => throw new NotImplementedException();

    int IBattle.pbAIRandom(int x) => throw new NotImplementedException();

    //void IBattle.pbAddToPlayerParty(IBattler pokemon) => throw new NotImplementedException();

    bool IBattle.pbCanShowCommands(int idxPokemon) => throw new NotImplementedException();

    bool IBattle.pbCanShowFightMenu(int idxPokemon) => throw new NotImplementedException();

    bool IBattle.pbCanChooseMove(
      int idxPokemon,
      int idxMove,
      bool showMessages,
      bool sleeptalk)
    {
      throw new NotImplementedException();
    }
    bool IBattle.pbOnActiveOne(IBattler pkmn, bool onlyabilities, bool moldbreaker) => throw new NotImplementedException();


    //void IBattle.pbSendOut(int index, IBattler pokemon) => throw new NotImplementedException();
    //
    //void IBattleCommon.pbThrowPokeBall(
    //  int idxPokemon,
    //  Items ball,
    //  int? rareness,
    //  bool showplayer)
    //{
    //  throw new NotImplementedException();
    //}

    public bool pbUseItemOnPokemon(
      Items item,
      int pkmnIndex,
      IBattler userPkmn,
      IHasDisplayMessage scene)
    {
      return ((IBattleShadowPokemon) this).pbUseItemOnPokemon(item, pkmnIndex, userPkmn, scene);
    }

    bool IBattleShadowPokemon.pbUseItemOnPokemon(
      Items item,
      int pkmnIndex,
      IBattler userPkmn,
      IHasDisplayMessage scene)
    {
      if (!(this.party1[pkmnIndex] is IPokemonShadowPokemon pokemonShadowPokemon) || !pokemonShadowPokemon.hypermode)
        return ((IBattle) this).pbUseItemOnPokemon(item, pkmnIndex, userPkmn, scene);
      scene.pbDisplay(Game._INTL("This item can't be used on that Pokemon."));
      return false;
    }

    public virtual BattleResults pbDecisionOnDraw() => ((IBattleClause) this).pbDecisionOnDraw();

    BattleResults IBattleClause.pbDecisionOnDraw()
    {
      if (!this.rules["selfkoclause"])
        return ((IBattle) this).pbDecisionOnDraw();
      if (this.lastMoveUser < 0)
        return BattleResults.DRAW;
      return this.pbIsOpposing(this.lastMoveUser) ? BattleResults.LOST : BattleResults.WON;
    }

    public virtual void pbJudgeCheckpoint(IBattler attacker, IBattleMove move = null)
    {
      if (this.rules["drawclause"])
      {
        if (move.IsNotNullOrNone() && move.Effect == PokemonUnity.Attack.Data.Effects.x15A || !this.pbAllFainted(this.party1) || !this.pbAllFainted(this.party2))
          return;
        this.decision = this.pbIsOpposing(attacker.Index) ? BattleResults.WON : BattleResults.LOST;
      }
      else
      {
        if (!this.rules["modifiedselfdestructclause"] || !move.IsNotNullOrNone() || move.Effect != PokemonUnity.Attack.Data.Effects.x008 || !this.pbAllFainted(this.party1) || !this.pbAllFainted(this.party2))
          return;
        this.decision = this.pbIsOpposing(attacker.Index) ? BattleResults.WON : BattleResults.LOST;
      }
    }

    public virtual void pbEndOfRoundPhase() => ((IBattleClause) this).pbEndOfRoundPhase();

    void IBattleClause.pbEndOfRoundPhase()
    {
      ((IBattle) this).pbEndOfRoundPhase();
      if (!this.rules["suddendeath"] || this.decision != BattleResults.ABORTED)
        return;
      if (this.pbPokemonCount(this.party1) > this.pbPokemonCount(this.party2))
        this.decision = BattleResults.LOST;
      else if (this.pbPokemonCount(this.party1) < this.pbPokemonCount(this.party2))
        this.decision = BattleResults.WON;
    }
  }
}
