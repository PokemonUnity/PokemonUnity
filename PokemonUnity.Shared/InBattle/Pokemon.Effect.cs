using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Localization;
using PokemonUnity.Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonUnity.Combat
{
    /// <summary>
    /// </summary>
    public partial class Pokemon //: PokemonUnity.Battle.Pokemon
    {
#region Sleep
  public bool pbCanSleep(Pokemon attacker, bool showMessages, Move move=null, bool ignorestatus=false) {
    if (isFainted()) return false;
    bool selfsleep=(attacker.IsNotNullOrNone() && attacker.Index==this.Index);
    if (!ignorestatus && status==Status.SLEEP) {
      if (showMessages) @battle.pbDisplay(_INTL("{1} is already asleep!",ToString()));
      return false;
    }
    if (!selfsleep) {
      if (this.status!=0 ||
         (effects.Substitute>0 && (!move.IsNotNullOrNone() || !move.ignoresSubstitute(attacker)))) {
        if (showMessages) @battle.pbDisplay(_INTL("But it failed!"));
        return false;
      }
    }
    if (!this.isAirborne(attacker.IsNotNullOrNone() && attacker.hasMoldBreaker()))
      if (@battle.field.ElectricTerrain>0) {
        if (showMessages) @battle.pbDisplay(_INTL("The Electric Terrain prevented {1} from falling asleep!",ToString(true)));
        return false;
      }else if (@battle.field.MistyTerrain>0) {
        if (showMessages) @battle.pbDisplay(_INTL("The Misty Terrain prevented {1} from falling asleep!",ToString(true)));
        return false;
      }
    if ((attacker.IsNotNullOrNone() && attacker.hasMoldBreaker()) || !hasWorkingAbility(Abilities.SOUNDPROOF))
      for (int i = 0; i< 4; i++)
        if (@battle.battlers[i].effects.Uproar>0) {
          if (showMessages) @battle.pbDisplay(_INTL("But the uproar kept {1} awake!",ToString(true)));
          return false;
        }
    if (!attacker.IsNotNullOrNone() || attacker.Index==this.Index || !attacker.hasMoldBreaker()) {
      if (hasWorkingAbility(Abilities.VITAL_SPIRIT) ||
         hasWorkingAbility(Abilities.INSOMNIA) ||
         hasWorkingAbility(Abilities.SWEET_VEIL) ||
         (hasWorkingAbility(Abilities.FLOWER_VEIL) && hasType(Types.GRASS)) ||
         (hasWorkingAbility(Abilities.LEAF_GUARD) && (@battle.Weather==Weather.SUNNYDAY ||
                                            @battle.Weather==Weather.HARSHSUN))) {
        string abilityname=this.ability.ToString(TextScripts.Name);
        if (showMessages) @battle.pbDisplay(_INTL("{1} stayed awake using its {2}!",ToString(),abilityname));
        return false;
      }
      if (Partner.hasWorkingAbility(Abilities.SWEET_VEIL) ||
         (Partner.hasWorkingAbility(Abilities.FLOWER_VEIL) && hasType(Types.GRASS))) {
        string abilityname=Partner.ability.ToString(TextScripts.Name);
        if (showMessages) @battle.pbDisplay(_INTL("{1} stayed awake using its partner's {2}!",ToString(),abilityname));
        return false;
      }
    }
    if (!selfsleep)
      if (OwnSide.Safeguard>0 &&
         (!attacker.IsNotNullOrNone() || !attacker.hasWorkingAbility(Abilities.INFILTRATOR))) {
        if (showMessages) @battle.pbDisplay(_INTL("{1}'s team is protected by Safeguard!",ToString()));
        return false;
      }
    return true;
  }

  public bool pbCanSleepYawn() {
    if (status!=0) return false;
    if (!hasWorkingAbility(Abilities.SOUNDPROOF))
      for (int i = 0; i < 4; i++)
        if (@battle.battlers[i].effects.Uproar>0) return false;
    if (!this.isAirborne()) {
      if (@battle.field.ElectricTerrain>0) return false;
      if (@battle.field.MistyTerrain>0) return false;
    }
    if (hasWorkingAbility(Abilities.VITAL_SPIRIT) ||
       hasWorkingAbility(Abilities.INSOMNIA) ||
       hasWorkingAbility(Abilities.SWEET_VEIL) ||
       (hasWorkingAbility(Abilities.LEAF_GUARD) && (@battle.Weather==Weather.SUNNYDAY ||
                                          @battle.Weather==Weather.HARSHSUN)))
      return false;
    if (Partner.hasWorkingAbility(Abilities.SWEET_VEIL)) return false;
    return true;
  }

  public void pbSleep(string msg=null) {
    this.status=Status.SLEEP;
    this.StatusCount=2+@battle.pbRandom(3);
    if (this.hasWorkingAbility(Abilities.EARLY_BIRD)) this.StatusCount=(int)Math.Floor(this.StatusCount/2d);
    pbCancelMoves();
    @battle.pbCommonAnimation("Sleep",this,null);
    if (!string.IsNullOrEmpty(msg))
      @battle.pbDisplay(msg);
    else
      @battle.pbDisplay(_INTL("{1} fell asleep!",ToString()));
    GameDebug.Log($"[Status change] #{ToString()} fell asleep (#{this.StatusCount} turns)");
  }

  public void pbSleepSelf(int duration=-1) {
    this.status=Status.SLEEP;
    if (duration>0)
      this.StatusCount=(byte)duration;
    else
      this.StatusCount=(byte)(2+@battle.pbRandom(3));
    if (this.hasWorkingAbility(Abilities.EARLY_BIRD)) this.StatusCount=(int)Math.Floor(this.StatusCount/2d);
    pbCancelMoves();
    @battle.pbCommonAnimation("Sleep",this,null);
    GameDebug.Log($"[Status change] #{ToString()} made itself fall asleep (#{this.StatusCount} turns)");
  }
#endregion

#region Poison
  public bool pbCanPoison(Pokemon attacker,bool showMessages,Move move=null) {
    if (isFainted()) return false;
    if (status==Status.POISON) {
      if (showMessages) @battle.pbDisplay(_INTL("{1} is already poisoned.",ToString()));
      return false;
    }
    if (this.status!=0 ||
       (effects.Substitute>0 && (!move.IsNotNullOrNone() || !move.ignoresSubstitute(attacker)))) {
      if (showMessages) @battle.pbDisplay(_INTL("But it failed!"));
      return false;
    }
    if ((hasType(Types.POISON) || hasType(Types.STEEL)) && !hasWorkingItem(Items.RING_TARGET)) {
      if (showMessages) @battle.pbDisplay(_INTL("It doesn't affect {1}...",ToString(true)));
      return false;
    }
    if (@battle.field.MistyTerrain>0 &&
       !this.isAirborne(attacker.IsNotNullOrNone() && attacker.hasMoldBreaker())) {
      if (showMessages) @battle.pbDisplay(_INTL("The Misty Terrain prevented {1} from being poisoned!",ToString(true)));
      return false;
    }
    if (!attacker.IsNotNullOrNone() || !attacker.hasMoldBreaker()) {
      if (hasWorkingAbility(Abilities.IMMUNITY) ||
         (hasWorkingAbility(Abilities.FLOWER_VEIL) && hasType(Types.GRASS)) ||
         (hasWorkingAbility(Abilities.LEAF_GUARD) && (@battle.Weather==Weather.SUNNYDAY ||
                                            @battle.Weather==Weather.HARSHSUN))) {
        if (showMessages) @battle.pbDisplay(_INTL("{1}'s {2} prevents poisoning!",ToString(),this.ability.ToString(TextScripts.Name)));
        return false;
      }
      if (Partner.hasWorkingAbility(Abilities.FLOWER_VEIL) && hasType(Types.GRASS)) {
        string abilityname=Partner.ability.ToString(TextScripts.Name);
        if (showMessages) @battle.pbDisplay(_INTL("{1}'s partner's {2} prevents poisoning!",ToString(),abilityname));
        return false;
      }
    }
    if (OwnSide.Safeguard>0 &&
       (!attacker.IsNotNullOrNone() || !attacker.hasWorkingAbility(Abilities.INFILTRATOR))) {
      if (showMessages) @battle.pbDisplay(_INTL("{1}'s team is protected by Safeguard!",ToString()));
      return false;
    }
    return true;
  }

  public bool pbCanPoisonSynchronize(Pokemon opponent) {
    if (isFainted()) return false;
    if ((hasType(Types.POISON) || hasType(Types.STEEL)) && !hasWorkingItem(Items.RING_TARGET)) {
      @battle.pbDisplay(_INTL("{1}'s {2} had no effect on {3}!",
         opponent.ToString(),opponent.ability.ToString(TextScripts.Name),ToString(true)));
      return false;
    }   
    if (this.status!=0) return false;
    if (hasWorkingAbility(Abilities.IMMUNITY) ||
       (hasWorkingAbility(Abilities.FLOWER_VEIL) && hasType(Types.GRASS)) ||
       (hasWorkingAbility(Abilities.LEAF_GUARD) && (@battle.Weather==Weather.SUNNYDAY ||
                                          @battle.Weather==Weather.HARSHSUN))) {
      @battle.pbDisplay(_INTL("{1}'s {2} prevents {3}'s {4} from working!",
         ToString(),this.ability.ToString(TextScripts.Name),
         opponent.ToString(true),opponent.ability.ToString(TextScripts.Name)));
      return false;
    }
    if (Partner.hasWorkingAbility(Abilities.FLOWER_VEIL) && hasType(Types.GRASS)) {
      @battle.pbDisplay(_INTL("{1}'s {2} prevents {3}'s {4} from working!",
         Partner.ToString(),Partner.ability.ToString(TextScripts.Name),
         opponent.ToString(true),opponent.ability.ToString(TextScripts.Name)));
      return false;
    }
    return true;
  }

  public bool pbCanPoisonSpikes(bool moldbreaker=false) {
    if (isFainted()) return false;
    if (this.status!=0) return false;
    if (hasType(Types.POISON) || hasType(Types.STEEL)) return false;
    if (!moldbreaker) {
      if (hasWorkingAbility(Abilities.IMMUNITY) ||
                      (hasWorkingAbility(Abilities.FLOWER_VEIL) && hasType(Types.GRASS)) ||
                      (Partner.hasWorkingAbility(Abilities.FLOWER_VEIL) && hasType(Types.GRASS))) return false;
      if (hasWorkingAbility(Abilities.LEAF_GUARD) &&
                      (@battle.Weather==Weather.SUNNYDAY ||
                      @battle.Weather==Weather.HARSHSUN)) return false;
    }
    if (OwnSide.Safeguard>0) return false;
    return true;
  }

  public void pbPoison(Pokemon attacker,string msg=null, bool toxic=false) {
    this.status=Status.POISON;
    this.StatusCount=(toxic) ? 1 : 0;
    this.effects.Toxic=0;
    @battle.pbCommonAnimation("Poison",this,null);
    if (!string.IsNullOrEmpty(msg))
      @battle.pbDisplay(msg);
    else {
      if (toxic)
        @battle.pbDisplay(_INTL("{1} was badly poisoned!",ToString()));
      else
        @battle.pbDisplay(_INTL("{1} was poisoned!",ToString()));
    }
    if (toxic)
      GameDebug.Log($"[Status change] #{ToString()} was badly poisoned]");
    else
      GameDebug.Log($"[Status change] #{ToString()} was poisoned");
    if (attacker.IsNotNullOrNone() && this.Index!=attacker.Index &&
       this.hasWorkingAbility(Abilities.SYNCHRONIZE))
      if (attacker.pbCanPoisonSynchronize(this)) {
        GameDebug.Log($"[Ability triggered] #{this.ToString()}'s Synchronize");
        attacker.pbPoison(null,_INTL("{1}'s {2} poisoned {3}!",this.ToString(),
           this.ability.ToString(TextScripts.Name),attacker.ToString(true)),toxic);
      }
  }
#endregion

#region Burn
  public bool pbCanBurn(Pokemon attacker,bool showMessages,Move move=null) {
    if (isFainted()) return false;
    if (this.status==Status.BURN) {
      if (showMessages) @battle.pbDisplay(_INTL("{1} already has a burn.",ToString()));
      return false;
    }
    if (this.status!=0 ||
       (effects.Substitute>0 && (!move.IsNotNullOrNone() || !move.ignoresSubstitute(attacker)))) {
      if (showMessages) @battle.pbDisplay(_INTL("But it failed!"));
      return false;
    }
    if (@battle.field.MistyTerrain>0 &&
       !this.isAirborne(attacker.IsNotNullOrNone() && attacker.hasMoldBreaker())) {
      if (showMessages) @battle.pbDisplay(_INTL("The Misty Terrain prevented {1} from being burned!",ToString(true)));
      return false;
    }
    if (hasType(Types.FIRE) && !hasWorkingItem(Items.RING_TARGET)) {
      if (showMessages) @battle.pbDisplay(_INTL("It doesn't affect {1}...",ToString(true)));
      return false;
    }
    if (!attacker.IsNotNullOrNone() || !attacker.hasMoldBreaker()) {
      if (hasWorkingAbility(Abilities.WATER_VEIL) ||
         (hasWorkingAbility(Abilities.FLOWER_VEIL) && hasType(Types.GRASS)) ||
         (hasWorkingAbility(Abilities.LEAF_GUARD) && (@battle.Weather==Weather.SUNNYDAY ||
                                            @battle.Weather==Weather.HARSHSUN))) {
        if (showMessages) @battle.pbDisplay(_INTL("{1}'s {2} prevents burns!",ToString(),this.ability.ToString(TextScripts.Name)));
        return false;
      }
      if (Partner.hasWorkingAbility(Abilities.FLOWER_VEIL) && hasType(Types.GRASS)) {
        string abilityname=Partner.ability.ToString(TextScripts.Name);
        if (showMessages) @battle.pbDisplay(_INTL("{1}'s partner's {2} prevents burns!",ToString(),abilityname));
        return false;
      }
    }
    if (OwnSide.Safeguard>0 &&
       (!attacker.IsNotNullOrNone() || !attacker.hasWorkingAbility(Abilities.INFILTRATOR))) {
      if (showMessages) @battle.pbDisplay(_INTL("{1}'s team is protected by Safeguard!",ToString()));
      return false;
    }
    return true;
  }

  public bool pbCanBurnSynchronize(Pokemon opponent) {
    if (isFainted()) return false;
    if (this.status!=0) return false;
    if (hasType(Types.FIRE) && !hasWorkingItem(Items.RING_TARGET)) {
       @battle.pbDisplay(_INTL("{1}'s {2} had no effect on {3}!",
          opponent.ToString(),opponent.ability.ToString(TextScripts.Name),ToString(true)));
       return false;
    }   
    if (hasWorkingAbility(Abilities.WATER_VEIL) ||
       (hasWorkingAbility(Abilities.FLOWER_VEIL) && hasType(Types.GRASS)) ||
       (hasWorkingAbility(Abilities.LEAF_GUARD) && (@battle.Weather==Weather.SUNNYDAY ||
                                          @battle.Weather==Weather.HARSHSUN))) {
      @battle.pbDisplay(_INTL("{1}'s {2} prevents {3}'s {4} from working!",
         ToString(),this.ability.ToString(TextScripts.Name),
         opponent.ToString(true),opponent.ability.ToString(TextScripts.Name)));
      return false;
    }
    if (Partner.hasWorkingAbility(Abilities.FLOWER_VEIL) && hasType(Types.GRASS)) {
      @battle.pbDisplay(_INTL("{1}'s {2} prevents {3}'s {4} from working!",
         Partner.ToString(),Partner.ability.ToString(TextScripts.Name),
         opponent.ToString(true),opponent.ability.ToString(TextScripts.Name)));
      return false;
    }
    return true;
  }

  public void pbBurn(Pokemon attacker,string msg=null) {
    this.status=Status.BURN;
    this.StatusCount=0;
    @battle.pbCommonAnimation("Burn",this,null);
    if (!string.IsNullOrEmpty(msg))
      @battle.pbDisplay(msg);
    else
      @battle.pbDisplay(_INTL("{1} was burned!",ToString()));
    GameDebug.Log($"[Status change] #{ToString()} was burned");
    if (attacker.IsNotNullOrNone() && this.Index!=attacker.Index &&
       this.hasWorkingAbility(Abilities.SYNCHRONIZE))
      if (attacker.pbCanBurnSynchronize(this)) {
        GameDebug.Log($"[Ability triggered] #{this.ToString()}'s Synchronize");
        attacker.pbBurn(null,_INTL("{1}'s {2} burned {3}!",this.ToString(),
           this.ability.ToString(TextScripts.Name),attacker.ToString(true)));
      }
  }
#endregion

#region Paralyze
  public bool pbCanParalyze(Pokemon attacker,bool showMessages,Move move=null) {
    if (isFainted()) return false;
    if (status==Status.PARALYSIS) {
      if (showMessages) @battle.pbDisplay(_INTL("{1} is already paralyzed!",ToString()));
      return false;
    }
    if (this.status!=0 ||
       (effects.Substitute>0 && (!move.IsNotNullOrNone() || !move.ignoresSubstitute(attacker)))) {
      if (showMessages) @battle.pbDisplay(_INTL("But it failed!"));
      return false;
    }
    if (@battle.field.MistyTerrain>0 &&
       !this.isAirborne(attacker.IsNotNullOrNone() && attacker.hasMoldBreaker())) {
      if (showMessages) @battle.pbDisplay(_INTL("The Misty Terrain prevented {1} from being paralyzed!",ToString(true)));
      return false;
    }
    if (hasType(Types.ELECTRIC) && !hasWorkingItem(Items.RING_TARGET) && Core.USENEWBATTLEMECHANICS) {
      if (showMessages) @battle.pbDisplay(_INTL("It doesn't affect {1}...",ToString(true)));
      return false;
    }
    if (!attacker.IsNotNullOrNone() || !attacker.hasMoldBreaker()) {
      if (hasWorkingAbility(Abilities.LIMBER) ||
         (hasWorkingAbility(Abilities.FLOWER_VEIL) && hasType(Types.GRASS)) ||
         (hasWorkingAbility(Abilities.LEAF_GUARD) && (@battle.Weather==Weather.SUNNYDAY ||
                                            @battle.Weather==Weather.HARSHSUN))) {
        if (showMessages) @battle.pbDisplay(_INTL("{1}'s {2} prevents paralysis!",ToString(),this.ability.ToString(TextScripts.Name)));
        return false;
      }
      if (Partner.hasWorkingAbility(Abilities.FLOWER_VEIL) && hasType(Types.GRASS)) {
        string abilityname=Partner.ability.ToString(TextScripts.Name);
        if (showMessages) @battle.pbDisplay(_INTL("{1}'s partner's {2} prevents paralysis!",ToString(),abilityname));
        return false;
      }
    }
    if (OwnSide.Safeguard>0 &&
       (!attacker.IsNotNullOrNone() || !attacker.hasWorkingAbility(Abilities.INFILTRATOR))) {
      if (showMessages) @battle.pbDisplay(_INTL("{1}'s team is protected by Safeguard!",ToString()));
      return false;
    }
    return true;
  }

  public bool pbCanParalyzeSynchronize(Pokemon opponent) {
    if (this.status!=0) return false;
    if (@battle.field.MistyTerrain>0 && !this.isAirborne()) return false;
    if (hasType(Types.ELECTRIC) && !hasWorkingItem(Items.RING_TARGET) && Core.USENEWBATTLEMECHANICS) return false;
    if (hasWorkingAbility(Abilities.LIMBER) ||
       (hasWorkingAbility(Abilities.FLOWER_VEIL) && hasType(Types.GRASS)) ||
       (hasWorkingAbility(Abilities.LEAF_GUARD) && (@battle.Weather==Weather.SUNNYDAY ||
                                          @battle.Weather==Weather.HARSHSUN))) {
      @battle.pbDisplay(_INTL("{1}'s {2} prevents {3}'s {4} from working!",
         ToString(),this.ability.ToString(TextScripts.Name),
         opponent.ToString(true),opponent.ability.ToString(TextScripts.Name)));
      return false;
    }
    if (Partner.hasWorkingAbility(Abilities.FLOWER_VEIL) && hasType(Types.GRASS)) {
      @battle.pbDisplay(_INTL("{1}'s {2} prevents {3}'s {4} from working!",
         Partner.ToString(),Partner.ability.ToString(TextScripts.Name),
         opponent.ToString(true),opponent.ability.ToString(TextScripts.Name)));
      return false;
    }
    return true;
  }

  public void pbParalyze(Pokemon attacker,string msg=null) {
    this.status=Status.PARALYSIS;
    this.StatusCount=0;
    @battle.pbCommonAnimation("Paralysis",this,null);
    if (!string.IsNullOrEmpty(msg))
      @battle.pbDisplay(msg);
    else
      @battle.pbDisplay(_INTL("{1} is paralyzed! It may be unable to move!",ToString()));
    GameDebug.Log($"[Status change] #{ToString()} was paralyzed");
    if (attacker.IsNotNullOrNone() && this.Index!=attacker.Index &&
       this.hasWorkingAbility(Abilities.SYNCHRONIZE))
      if (attacker.pbCanParalyzeSynchronize(this)) {
        GameDebug.Log($"[Ability triggered] #{this.ToString()}'s Synchronize");
        attacker.pbParalyze(null,_INTL("{1}'s {2} paralyzed {3}! It may be unable to move!",
           this.ToString(),this.ability.ToString(TextScripts.Name),attacker.ToString(true)));
      }
  }
#endregion

#region Freeze
  public bool pbCanFreeze(Pokemon attacker,bool showMessages,Move move=null) {
    if (isFainted()) return false;
    if (status==Status.FROZEN) {
      if (showMessages) @battle.pbDisplay(_INTL("{1} is already frozen solid!",ToString()));
      return false;
    }
    if (this.status!=0 ||
       (effects.Substitute>0 && (!move.IsNotNullOrNone() || !move.ignoresSubstitute(attacker))) ||
       @battle.Weather==Weather.SUNNYDAY ||
       @battle.Weather==Weather.HARSHSUN) {
      if (showMessages) @battle.pbDisplay(_INTL("But it failed!"));
      return false;
    }
    if (hasType(Types.ICE) && !hasWorkingItem(Items.RING_TARGET)) {
      if (showMessages) @battle.pbDisplay(_INTL("It doesn't affect {1}...",ToString(true)));
      return false;
    }
    if (@battle.field.MistyTerrain>0 &&
       !this.isAirborne(attacker.IsNotNullOrNone() && attacker.hasMoldBreaker())) {
      if (showMessages) @battle.pbDisplay(_INTL("The Misty Terrain prevented {1} from being frozen!",ToString(true)));
      return false;
    }
    if (!attacker.IsNotNullOrNone() || !attacker.hasMoldBreaker()) {
      if (hasWorkingAbility(Abilities.MAGMA_ARMOR) ||
         (hasWorkingAbility(Abilities.FLOWER_VEIL) && hasType(Types.GRASS)) ||
         (hasWorkingAbility(Abilities.LEAF_GUARD) && (@battle.Weather==Weather.SUNNYDAY ||
                                            @battle.Weather==Weather.HARSHSUN))) {
        if (showMessages) @battle.pbDisplay(_INTL("{1}'s {2} prevents freezing!",ToString(),this.ability.ToString(TextScripts.Name)));
        return false;
      }
      if (Partner.hasWorkingAbility(Abilities.FLOWER_VEIL) && hasType(Types.GRASS)) {
        string abilityname=Partner.ability.ToString(TextScripts.Name);
        if (showMessages) @battle.pbDisplay(_INTL("{1}'s partner's {2} prevents freezing!",ToString(),abilityname));
        return false;
      }
    }
    if (OwnSide.Safeguard>0 &&
       (!attacker.IsNotNullOrNone() || !attacker.hasWorkingAbility(Abilities.INFILTRATOR))) {
      if (showMessages) @battle.pbDisplay(_INTL("{1}'s team is protected by Safeguard!",ToString()));
      return false;
    }
    return true;
  }

  public void pbFreeze(string msg=null) {
    this.status=Status.FROZEN;
    this.StatusCount=0;
    pbCancelMoves();
    @battle.pbCommonAnimation("Frozen",this,null);
    if (!string.IsNullOrEmpty(msg))
      @battle.pbDisplay(msg);
    else
      @battle.pbDisplay(_INTL("{1} was frozen solid!",ToString()));
    GameDebug.Log($"[Status change] #{ToString()} was frozen");
  }
#endregion

#region Generalised status displays
  public void pbContinueStatus(bool showAnim=true) {
    switch (this.status) {
    case Status.SLEEP:
      @battle.pbCommonAnimation("Sleep",this,null);
      @battle.pbDisplay(_INTL("{1} is fast asleep.",ToString()));
      break;
    case Status.POISON:
      @battle.pbCommonAnimation("Poison",this,null);
      @battle.pbDisplay(_INTL("{1} was hurt by poison!",ToString()));
      break;
    case Status.BURN:
      @battle.pbCommonAnimation("Burn",this,null);
      @battle.pbDisplay(_INTL("{1} was hurt by its burn!",ToString()));
      break;
    case Status.PARALYSIS:
      @battle.pbCommonAnimation("Paralysis",this,null);
      @battle.pbDisplay(_INTL("{1} is paralyzed! It can't move!",ToString())) ;
      break;
    case Status.FROZEN:
      @battle.pbCommonAnimation("Frozen",this,null);
      @battle.pbDisplay(_INTL("{1} is frozen solid!",ToString()));
      break;
    }
  }

  public void pbCureStatus(bool showMessages=true) {
    Status oldstatus=this.status;
    this.status=0;
    this.StatusCount=0;
  if (showMessages)  
      switch (oldstatus) {
      case Status.SLEEP:
        @battle.pbDisplay(_INTL("{1} woke up!",ToString()));
        break;
      case Status.POISON:
      case Status.BURN:
      case Status.PARALYSIS:
        @battle.pbDisplay(_INTL("{1} was cured of paralysis.",ToString()));
        break;
      case Status.FROZEN:
        @battle.pbDisplay(_INTL("{1} thawed out!",ToString()));
        break;
      }
    GameDebug.Log($"[Status change] #{ToString()}'s status was cured");
  }
#endregion

#region Confuse
  public bool pbCanConfuse(Pokemon attacker=null,bool showMessages=true,Move move=null) {
    if (isFainted()) return false;
    if (effects.Confusion>0) {
      if (showMessages) @battle.pbDisplay(_INTL("{1} is already confused!",ToString()));
      return false;
    }
    if (effects.Substitute>0 && (!move.IsNotNullOrNone() || !move.ignoresSubstitute(attacker))) {
      if (showMessages) @battle.pbDisplay(_INTL("But it failed!"));
      return false;
    }
    if (!attacker.IsNotNullOrNone() || !attacker.hasMoldBreaker())
      if (hasWorkingAbility(Abilities.OWN_TEMPO)) {
        if (showMessages) @battle.pbDisplay(_INTL("{1}'s {2} prevents confusion!",ToString(),this.ability.ToString(TextScripts.Name)));
        return false;
      }
    if (OwnSide.Safeguard>0 &&
       (!attacker.IsNotNullOrNone() || !attacker.hasWorkingAbility(Abilities.INFILTRATOR))) {
      if (showMessages) @battle.pbDisplay(_INTL("{1}'s team is protected by Safeguard!",ToString()));
      return false;
    }
    return true;
  }

  public bool pbCanConfuseSelf(bool showMessages) {
    if (isFainted()) return false;
    if (effects.Confusion>0) {
      if (showMessages) @battle.pbDisplay(_INTL("{1} is already confused!",ToString()));
      return false;
    }
    if (hasWorkingAbility(Abilities.OWN_TEMPO)) {
      if (showMessages) @battle.pbDisplay(_INTL("{1}'s {2} prevents confusion!",ToString(),this.ability.ToString(TextScripts.Name)));
      return false;
    }
    return true;
  }

  public void pbConfuse() {
    effects.Confusion=2+@battle.pbRandom(4);
    @battle.pbCommonAnimation("Confusion",this,null);
    GameDebug.Log($"[Lingering effect triggered] #{ToString()} became confused (#{effects.Confusion} turns)");
  }

  public void pbConfuseSelf() {
    if (pbCanConfuseSelf(false)) {
      effects.Confusion=2+@battle.pbRandom(4);
      @battle.pbCommonAnimation("Confusion",this,null);
      @battle.pbDisplay(_INTL("{1} became confused!",ToString()));
      GameDebug.Log($"[Lingering effect triggered] #{ToString()} became confused (#{effects.Confusion} turns)");
    }
  }

  public void pbContinueConfusion() {
    @battle.pbCommonAnimation("Confusion",this,null);
    @battle.pbDisplayBrief(_INTL("{1} is confused!",ToString()));
  }

  public void pbCureConfusion(bool showMessages=true) {
    effects.Confusion=0;
    if (showMessages) @battle.pbDisplay(_INTL("{1} snapped out of confusion!",ToString()));
    GameDebug.Log($"[End of effect] #{ToString()} was cured of confusion");
  }
#endregion

#region Attraction
  public bool pbCanAttract(Pokemon attacker,bool showMessages=true) {
    if (isFainted()) return false;
    if (!attacker.IsNotNullOrNone() || attacker.isFainted()) return false;
    if (effects.Attract>=0) {
      if (showMessages) @battle.pbDisplay(_INTL("But it failed!"));
      return false;
    }
    bool? agender=attacker.gender;
    bool? ogender=this.gender;
    if (!agender.HasValue || !ogender.HasValue || agender==ogender) {
      if (showMessages) @battle.pbDisplay(_INTL("But it failed!"));
      return false;
    }
    if ((!attacker.IsNotNullOrNone() || !attacker.hasMoldBreaker()) && hasWorkingAbility(Abilities.OBLIVIOUS)) {
      if (showMessages) @battle.pbDisplay(_INTL("{1}'s {2} prevents romance!",ToString(),
         this.ability.ToString(TextScripts.Name)));
      return false;
    }
    return true;
  }

  public void pbAttract(Pokemon attacker,string msg=null) {
    effects.Attract=attacker.Index;
    @battle.pbCommonAnimation("Attract",this,null);
    if (!string.IsNullOrEmpty(msg))
      @battle.pbDisplay(msg);
    else
      @battle.pbDisplay(_INTL("{1} fell in love!",ToString()));
    GameDebug.Log($"[Lingering effect triggered] #{ToString()} became infatuated (with #{attacker.ToString(true)})");
    if (this.hasWorkingItem(Items.DESTINY_KNOT) &&
       attacker.pbCanAttract(this,false)) {
      GameDebug.Log($"[Item triggered] #{ToString()}'s Destiny Knot");
      attacker.pbAttract(this,_INTL("{1}'s {2} made {3} fall in love!",ToString(),
         this.Item.ToString(TextScripts.Name),attacker.ToString(true)));
    }
  }

  public void pbAnnounceAttract(Pokemon seducer) {
    @battle.pbCommonAnimation("Attract",this,null);
    @battle.pbDisplayBrief(_INTL("{1} is in love with {2}!",
       ToString(),seducer.ToString(true)));
  }

  public void pbContinueAttract() {
    @battle.pbDisplay(_INTL("{1} is immobilized by love!",ToString()));
  }

  public void pbCureAttract() {
    effects.Attract=-1;
    GameDebug.Log($"[End of effect] #{ToString()} was cured of infatuation");
  }
#endregion

#region Flinching
  public bool pbFlinch(Pokemon attacker) {
    if ((!attacker.IsNotNullOrNone() || !attacker.hasMoldBreaker()) && hasWorkingAbility(Abilities.INNER_FOCUS)) return false;
    effects.Flinch=true;
    return true;
  }
#endregion

#region Increase stat stages
  public bool pbTooHigh(Stats stat) {
    return @stages[(int)stat]>=6;
  }

  public bool pbCanIncreaseStatStage(Stats stat,Pokemon attacker=null,bool showMessages=false,Move move=null,bool moldbreaker=false,bool ignoreContrary=false) {
    if (!moldbreaker)
      if (!attacker.IsNotNullOrNone() || attacker.Index==this.Index || !attacker.hasMoldBreaker())
        if (hasWorkingAbility(Abilities.CONTRARY) && !ignoreContrary)
          return pbCanReduceStatStage(stat,attacker,showMessages,moldbreaker:moldbreaker,ignoreContrary: true);
    if (isFainted()) return false;
    if (pbTooHigh(stat)) { 
      if (showMessages) @battle.pbDisplay(_INTL("{1}'s {2} won't go any higher!",
         ToString(),stat.ToString(TextScripts.Name)));
      return false;
    }
    return true;
  }

  public int pbIncreaseStatBasic(Stats stat,int increment,Pokemon attacker=null,bool moldbreaker=false,bool ignoreContrary=false) {
    if (!moldbreaker)
      if (!attacker.IsNotNullOrNone() || attacker.Index==this.Index || !attacker.hasMoldBreaker()) { 
        if (hasWorkingAbility(Abilities.CONTRARY) && !ignoreContrary)
          return pbReduceStatBasic(stat,increment,attacker,moldbreaker,true);
        if (hasWorkingAbility(Abilities.SIMPLE)) increment*=2;
      }
    increment=Math.Min(increment,6-@stages[(int)stat]);
    GameDebug.Log($"[Stat change] #{ToString()}'s #{stat.ToString(TextScripts.Name)} rose by #{increment} stage(s) (was #{@stages[(int)stat]}, now #{@stages[(int)stat]+increment})");
    @stages[(int)stat]+=increment;
    return increment;
  }

  public bool pbIncreaseStat(Stats stat,int increment,Pokemon attacker,bool showMessages,Move move=null,bool upanim=true,bool moldbreaker=false,bool ignoreContrary=false) {
    if (!moldbreaker)
      if (!attacker.IsNotNullOrNone() || attacker.Index==this.Index || !attacker.hasMoldBreaker())
        if (hasWorkingAbility(Abilities.CONTRARY) && !ignoreContrary)
          return pbReduceStat(stat,increment,attacker,showMessages,move,upanim,moldbreaker,true);
    if (stat!=Stats.ATTACK && stat!=Stats.DEFENSE &&
                    stat!=Stats.SPATK && stat!=Stats.SPDEF &&
                    stat!=Stats.SPEED && stat!=Stats.EVASION &&
                    stat!=Stats.ACCURACY) return false;
    if (pbCanIncreaseStatStage(stat, attacker, showMessages, move, moldbreaker, ignoreContrary)) { 
      increment=pbIncreaseStatBasic(stat,increment,attacker,moldbreaker,ignoreContrary);
      if (increment > 0) { 
        if (ignoreContrary)
          if (upanim) @battle.pbDisplay(_INTL("{1}'s {2} activated!",ToString(),this.ability.ToString(TextScripts.Name)));
        if (upanim) @battle.pbCommonAnimation("StatUp", this, null);
        string[] arrStatTexts=new string[] {_INTL("{1}'s {2} rose!",ToString(),stat.ToString(TextScripts.Name)),
           _INTL("{1}'s {2} rose sharply!",ToString(),stat.ToString(TextScripts.Name)),
           _INTL("{1}'s {2} rose drastically!",ToString(),stat.ToString(TextScripts.Name))};
        @battle.pbDisplay(arrStatTexts[Math.Min(increment-1,2)]);
        return true;
      }
    }
    return false;
  }

  public bool pbIncreaseStatWithCause(Stats stat,int increment,Pokemon attacker,string cause,bool showanim=true,bool showmessage=true,bool moldbreaker=false,bool ignoreContrary=false) {
    if (!moldbreaker)
      if (!attacker.IsNotNullOrNone() || attacker.Index==this.Index || !attacker.hasMoldBreaker())
        if (hasWorkingAbility(Abilities.CONTRARY) && !ignoreContrary)
          return pbReduceStatWithCause(stat,increment,attacker,cause,showanim,showmessage,moldbreaker,true);
    if (stat!=Stats.ATTACK && stat!=Stats.DEFENSE &&
                    stat!=Stats.SPATK && stat!=Stats.SPDEF &&
                    stat!=Stats.SPEED && stat!=Stats.EVASION &&
                    stat!=Stats.ACCURACY) return false;
    if (pbCanIncreaseStatStage(stat,attacker,false,null,moldbreaker,ignoreContrary))
      increment=pbIncreaseStatBasic(stat,increment,attacker,moldbreaker,ignoreContrary);
      if (increment > 0) { 
        //if (ignoreContrary) //ToDo: UpAnimation?
        //  if (upanim) @battle.pbDisplay(_INTL("{1}'s {2} activated!",ToString(),this.ability.ToString(TextScripts.Name)));
        if (showanim) @battle.pbCommonAnimation("StatUp", this, null); 
        string [] arrStatTexts = null;
        if (attacker.Index==this.Index)
          arrStatTexts=new string[] {_INTL("{1}'s {2} raised its {3}!",ToString(),cause,stat.ToString(TextScripts.Name)),
             _INTL("{1}'s {2} sharply raised its {3}!",ToString(),cause,stat.ToString(TextScripts.Name)),
             _INTL("{1}'s {2} went way up!",ToString(),stat.ToString(TextScripts.Name))};
        else
          arrStatTexts=new string[] {_INTL("{1}'s {2} raised {3}'s {4}!",attacker.ToString(),cause,ToString(true),stat.ToString(TextScripts.Name)),
             _INTL("{1}'s {2} sharply raised {3}'s {4}!",attacker.ToString(),cause,ToString(true),stat.ToString(TextScripts.Name)),
             _INTL("{1}'s {2} drastically raised {3}'s {4}!",attacker.ToString(),cause,ToString(true),stat.ToString(TextScripts.Name))};
        if (showmessage) @battle.pbDisplay(arrStatTexts[Math.Min(increment-1,2)]); 
        return true;
      }
    return false;
  }
#endregion

#region Decrease stat stages
  public bool pbTooLow(Stats stat) {
    return @stages[(int)stat]<=-6;
  }

  /// <summary>
  /// Tickle (04A) and Noble Roar (13A) can't use this, but replicate it instead.
  /// (Reason is they lowers more than 1 stat independently, and therefore could
  /// show certain messages twice which is undesirable.)
  /// </summary>
  /// <param name=""></param>
  /// <param name=""></param>
  /// <param name=""></param>
  /// <param name=""></param>
  /// <param name=""></param>
  /// <param name=""></param>
  /// <returns></returns>
  public bool pbCanReduceStatStage(Stats stat,Pokemon attacker=null,bool showMessages=false,Move move=null,bool moldbreaker=false,bool ignoreContrary=false) {
    if (!moldbreaker)
      if (!attacker.IsNotNullOrNone() || attacker.Index==this.Index || !attacker.hasMoldBreaker())
        if (hasWorkingAbility(Abilities.CONTRARY) && !ignoreContrary)
          return pbCanIncreaseStatStage(stat,attacker,showMessages,move,moldbreaker,true);
    if (isFainted()) return false;
    bool selfreduce=(attacker.IsNotNullOrNone() && attacker.Index==this.Index);
    if (!selfreduce) {
      if (effects.Substitute>0 && (!move.IsNotNullOrNone() || !move.ignoresSubstitute(attacker))) {
        if (showMessages) @battle.pbDisplay(_INTL("But it failed!"));
        return false;
      }
      if (OwnSide.Mist>0 &&
        (!attacker.IsNotNullOrNone() || !attacker.hasWorkingAbility(Abilities.INFILTRATOR))) {
        if (showMessages) @battle.pbDisplay(_INTL("{1} is protected by Mist!",ToString()));
        return false;
      }
      string abilityname;
      if (!moldbreaker && (!attacker.IsNotNullOrNone() || !attacker.hasMoldBreaker())) {
        if (hasWorkingAbility(Abilities.CLEAR_BODY) || hasWorkingAbility(Abilities.WHITE_SMOKE)) {
          abilityname=this.ability.ToString(TextScripts.Name);
          if (showMessages) @battle.pbDisplay(_INTL("{1}'s {2} prevents stat loss!",ToString(),abilityname));
          return false;
        }
        if (hasType(Types.GRASS))
          if (hasWorkingAbility(Abilities.FLOWER_VEIL)) {
            abilityname=this.ability.ToString(TextScripts.Name);
            if (showMessages) @battle.pbDisplay(_INTL("{1}'s {2} prevents stat loss!",ToString(),abilityname));
            return false;
          }else if (Partner.hasWorkingAbility(Abilities.FLOWER_VEIL)) {
            abilityname=Partner.ability.ToString(TextScripts.Name);
            if (showMessages) @battle.pbDisplay(_INTL("{1}'s {2} prevents {3}'s stat loss!",Partner.ToString(),abilityname,ToString(true)));
            return false;
          }
        if (stat==Stats.ATTACK && hasWorkingAbility(Abilities.HYPER_CUTTER)) {
          abilityname=this.ability.ToString(TextScripts.Name);
          if (showMessages) @battle.pbDisplay(_INTL("{1}'s {2} prevents Attack loss!",ToString(),abilityname));
          return false;
        }
        if (stat==Stats.DEFENSE && hasWorkingAbility(Abilities.BIG_PECKS)) {
          abilityname=this.ability.ToString(TextScripts.Name);
          if (showMessages) @battle.pbDisplay(_INTL("{1}'s {2} prevents Defense loss!",ToString(),abilityname));
          return false;
        }
        if (stat==Stats.ACCURACY && hasWorkingAbility(Abilities.KEEN_EYE)) {
          abilityname=this.ability.ToString(TextScripts.Name);
          if (showMessages) @battle.pbDisplay(_INTL("{1}'s {2} prevents accuracy loss!",ToString(),abilityname));
          return false;
        }
      }
    }
    if (pbTooLow(stat)) {
      if (showMessages) @battle.pbDisplay(_INTL("{1}'s {2} won't go any lower!",
         ToString(),stat.ToString(TextScripts.Name)));
      return false;
    }
    return true;
  }

  public int pbReduceStatBasic(Stats stat,int increment,Pokemon attacker=null,bool moldbreaker=false,bool ignoreContrary=false) {
    if (!moldbreaker) // moldbreaker is true only when Roar forces out a Pokémon into Sticky Web
      if (!attacker.IsNotNullOrNone() || attacker.Index==this.Index || !attacker.hasMoldBreaker()) {
        if (hasWorkingAbility(Abilities.CONTRARY) && !ignoreContrary)
          return pbIncreaseStatBasic(stat,increment,attacker,moldbreaker,true);
        if (hasWorkingAbility(Abilities.SIMPLE)) increment*=2;
      }
    increment=Math.Min(increment,6+@stages[(int)stat]);
    GameDebug.Log($"[Stat change] #{ToString()}'s #{stat.ToString(TextScripts.Name)} fell by #{increment} stage(s) (was #{@stages[(int)stat]}, now #{@stages[(int)stat]-increment})");
    @stages[(int)stat]-=increment;
    return increment;
  }

  public bool pbReduceStat(Stats stat,int increment,Pokemon attacker,bool showMessages,Move move=null,bool downanim=true,bool moldbreaker=false,bool ignoreContrary=false) {
    if (!moldbreaker)
      if (!attacker.IsNotNullOrNone() || attacker.Index==this.Index || !attacker.hasMoldBreaker())
        if (hasWorkingAbility(Abilities.CONTRARY) && !ignoreContrary)
          return pbIncreaseStat(stat,increment,attacker,showMessages,move,downanim,moldbreaker,true);
    if (stat!=Stats.ATTACK && stat!=Stats.DEFENSE &&
                    stat!=Stats.SPATK && stat!=Stats.SPDEF &&
                    stat!=Stats.SPEED && stat!=Stats.EVASION &&
                    stat!=Stats.ACCURACY) return false;
    if (pbCanReduceStatStage(stat,attacker,showMessages,move,moldbreaker,ignoreContrary))
      increment=pbReduceStatBasic(stat,increment,attacker,moldbreaker,ignoreContrary);
      if (increment > 0) {
        if (ignoreContrary)
          if (downanim) @battle.pbDisplay(_INTL("{1}'s {2} activated!",ToString(),this.ability.ToString(TextScripts.Name)));
        if (downanim) @battle.pbCommonAnimation("StatDown", this, null); 
        string[] arrStatTexts= new string[] {_INTL("{1}'s {2} fell!",ToString(),stat.ToString(TextScripts.Name)),
           _INTL("{1}'s {2} harshly fell!",ToString(),stat.ToString(TextScripts.Name)),
           _INTL("{1}'s {2} severely fell!",ToString(),stat.ToString(TextScripts.Name))};
        @battle.pbDisplay(arrStatTexts[Math.Min(increment-1,2)]);
        // Defiant
        if (hasWorkingAbility(Abilities.DEFIANT) && (!attacker.IsNotNullOrNone() || attacker.IsOpposing(this.Index)))
          pbIncreaseStatWithCause(Stats.ATTACK,2,this,this.ability.ToString(TextScripts.Name));
        // Competitive
        if (hasWorkingAbility(Abilities.COMPETITIVE) && (!attacker.IsNotNullOrNone() || attacker.IsOpposing(this.Index)))
          pbIncreaseStatWithCause(Stats.SPATK,2,this,this.ability.ToString(TextScripts.Name));
        return true;
      }
    return false;
  }

  public bool pbReduceStatWithCause(Stats stat,int increment,Pokemon attacker,string cause,bool showanim=true,bool showmessage=true,bool moldbreaker=false,bool ignoreContrary=false) {
    if (!moldbreaker)
      if (!attacker.IsNotNullOrNone() || attacker.Index==this.Index || !attacker.hasMoldBreaker())
        if (hasWorkingAbility(Abilities.CONTRARY) && !ignoreContrary)
          return pbIncreaseStatWithCause(stat,increment,attacker,cause,showanim,showmessage,moldbreaker,true);
    if (stat!=Stats.ATTACK && stat!=Stats.DEFENSE &&
                    stat!=Stats.SPATK && stat!=Stats.SPDEF &&
                    stat!=Stats.SPEED && stat!=Stats.EVASION &&
                    stat!=Stats.ACCURACY) return false;
    if (pbCanReduceStatStage(stat, attacker, false, null, moldbreaker, ignoreContrary)) {
      increment=pbReduceStatBasic(stat,increment,attacker,moldbreaker,ignoreContrary);
      if (increment > 0) {
        //if (ignoreContrary) //ToDo: DownAnimation?
        //  if (downanim) @battle.pbDisplay(_INTL("{1}'s {2} activated!",ToString(),this.ability.ToString(TextScripts.Name))); 
        if (showanim) @battle.pbCommonAnimation("StatDown",this,null);
        string[] arrStatTexts = null;
        if (attacker.Index==this.Index)
          arrStatTexts=new string[] {_INTL("{1}'s {2} lowered its {3}!",ToString(),cause,stat.ToString(TextScripts.Name)),
             _INTL("{1}'s {2} harshly lowered its {3}!",ToString(),cause,stat.ToString(TextScripts.Name)),
             _INTL("{1}'s {2} severely lowered its {3}!",ToString(),stat.ToString(TextScripts.Name))};
        else
          arrStatTexts=new string[] {_INTL("{1}'s {2} lowered {3}'s {4}!",attacker.ToString(),cause,ToString(true),stat.ToString(TextScripts.Name)),
             _INTL("{1}'s {2} harshly lowered {3}'s {4}!",attacker.ToString(),cause,ToString(true),stat.ToString(TextScripts.Name)),
             _INTL("{1}'s {2} severely lowered {3}'s {4}!",attacker.ToString(),cause,ToString(true),stat.ToString(TextScripts.Name))};
        if (showmessage) @battle.pbDisplay(arrStatTexts[Math.Min(increment-1,2)]); 
        // Defiant
        if (hasWorkingAbility(Abilities.DEFIANT) && (!attacker.IsNotNullOrNone() || attacker.IsOpposing(this.Index)))
          pbIncreaseStatWithCause(Stats.ATTACK,2,this,this.ability.ToString(TextScripts.Name));
        // Competitive
        if (hasWorkingAbility(Abilities.COMPETITIVE) && (!attacker.IsNotNullOrNone() || attacker.IsOpposing(this.Index)))
          pbIncreaseStatWithCause(Stats.SPATK,2,this,this.ability.ToString(TextScripts.Name));
        return true;
      }
    }
    return false;
  }

  public bool pbReduceAttackStatIntimidate(Pokemon opponent) {
    if (isFainted()) return false;
    if (effects.Substitute>0) {
      @battle.pbDisplay(_INTL("{1}'s substitute protected it from {2}'s {3}!",
         ToString(),opponent.ToString(true),opponent.ability.ToString(TextScripts.Name)));
      return false;
    }
    if (!opponent.hasWorkingAbility(Abilities.CONTRARY)) {
      if (OwnSide.Mist>0) {
        @battle.pbDisplay(_INTL("{1} is protected from {2}'s {3} by Mist!",
           ToString(),opponent.ToString(true),opponent.ability.ToString(TextScripts.Name)));
        return false;
      }
      if (hasWorkingAbility(Abilities.CLEAR_BODY) || hasWorkingAbility(Abilities.WHITE_SMOKE) ||
         hasWorkingAbility(Abilities.HYPER_CUTTER) ||
         (hasWorkingAbility(Abilities.FLOWER_VEIL) && hasType(Types.GRASS))) {
        string abilityname=this.ability.ToString(TextScripts.Name);
        string oppabilityname=opponent.ability.ToString(TextScripts.Name);
        @battle.pbDisplay(_INTL("{1}'s {2} prevented {3}'s {4} from working!",
           ToString(),abilityname,opponent.ToString(true),oppabilityname));
        return false;
      }
      if (Partner.hasWorkingAbility(Abilities.FLOWER_VEIL) && hasType(Types.GRASS)) {
        string abilityname=Partner.ability.ToString(TextScripts.Name);
        string oppabilityname=opponent.ability.ToString(TextScripts.Name);
        @battle.pbDisplay(_INTL("{1}'s {2} prevented {3}'s {4} from working!",
           Partner.ToString(),abilityname,opponent.ToString(true),oppabilityname));
        return false;
      }
    }
    return pbReduceStatWithCause(Stats.ATTACK,1,opponent,opponent.ability.ToString(TextScripts.Name));
  }
#endregion

        public string _INTL(string message, params string[] param)
        {
            return message;
        }
    }
}