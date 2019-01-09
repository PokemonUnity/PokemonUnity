using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.ComponentModel.DataAnnotations;
using PokemonUnity;
using PokemonUnity.Pokemon;
using PokemonUnity.Item;
using PokemonUnity.Move;

/// <summary>
/// During battle, the moves used are modified by these classes before calculations are applied
/// </summary>
public class Function
{
	
	/// <summary>
	/// Superclass that handles moves using a non-existent function code.
	/// Damaging moves just do damage with no additional effect.
	/// Non-damaging moves always fail.
	/// <summary>
	public class PokeBattle_UnimplementedMove : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, byte? alltargets= null, bool showanimation= true)
		{
			if (IsDamaging())
				return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			else
			{
				//battle.pbDisplay("But it failed!");
				return -1;
			}
		}
	}
	
	/// <summary>
	/// Superclass for a failed move. Always fails.
	/// This class is unused.
	/// <summary>
	public class PokeBattle_FailedMove : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, byte? alltargets= null, bool showanimation= true)
		{
			//battle.pbDisplay("But it failed!");
			return -1;
		}
	}
	
	/// <summary>
	/// Pseudomove for confusion damage.
	/// <summary>
	public class PokeBattle_Confusion : PokeBattle_Move
	{
		//public object initialize(battle, move)
		//{
		//
		//	battle     = battle;
		//	basedamage = 40;
		//
		//	type       = -1;
		//	accuracy   = 100;
		//	pp         = -1;
		//	addlEffect = 0;
		//	target     = 0;
		//	priority   = 0;
		//	flags      = 0;
		//	thismove   = move;
		//	name = "";
		//
		//	id         = 0;
		//}

		public bool IsPhysical(){ return true; }
		public bool IsSpecial(){ return false; }

		public override object pbCalcDamage(Battle.Battler attacker, Battle.Battler opponent){
    		return base.pbCalcDamage(attacker, opponent,
	   			thismove.NOCRITICAL|thismove.SELFCONFUSE|thismove.NOTYPE|thismove.NOWEIGHTING);
  		}

 		public object pbEffectMessages(Battle.Battler attacker, Battle.Battler opponent, bool ignoretype= false){
    		return base.pbEffectMessages(attacker, opponent,true);
  		}
	}

	/// <summary>
	/// Implements the move Struggle.
	/// For cases where the real move named Struggle is not defined.
	/// <summary>
	public class PokeBattle_Struggle : PokeBattle_Move
	{
		//public object initialize(battle, move)
		//{
		//	id         = -1;    // doesn't work if 0
		//	battle     = battle;
		//	name = _INTL("Struggle")
		//
		//	basedamage = 50;
		//	type       = -1;
		//	accuracy   = 0;
		//	addlEffect = 0;
		//	target     = 0;
		//	priority   = 0;
		//	flags      = 0;
		//	thismove   = null;   // not associated with a move
		//	pp		   = -1;
		//
		//	totalpp    = 0;
		//	if (move)
		//	{
		//		id = move.id;
		//
		//		name = PBMoves.getName(id);
		//	}
		//}

		public bool IsPhysical(){ return true; }
		public bool IsSpecial(){ return false; }

		public object pbEffectAfterHit(Battle.Battler attacker, Battle.Battler opponent, Effects.Move turneffects)
		{
			if (!attacker.isFainted() && turneffects.TotalDamage>0)
			{
				attacker.pbReduceHP((int)Math.Round(attacker.TotalHP / 4.0f));
				//battle.pbDisplay(_INTL("{1} is damaged by recoil!", attacker.pbThis));
			}
		}

		public object pbCalcDamage(Battle.Battler attacker, Battle.Battler opponent)
		{
			return base.pbCalcDamage(attacker, opponent, thismove.IGNOREPKMNTYPES);
		}
	}



	/// <summary>
	/// No additional effect.
	/// <summary>
	public class PokeBattle_Move_000 : PokeBattle_Move
	{
	}



	/// <summary>
	/// Does absolutely nothing. (Splash)
	/// <summary>
	public class PokeBattle_Move_001 : PokeBattle_Move
	{ 
		public bool unusableInGravity(){
			return true;
		}

		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		{
			pbShowAnimation(id, attacker, null, hitnum, alltargets, showanimation);
			//battle.pbDisplay(_INTL("But nothing happened!"));
			return 0;
		}
	}



	/// <summary>
	/// Struggle. Overrides the default Struggle effect above.
	/// <summary>
	public class PokeBattle_Move_002 : PokeBattle_Struggle
	{
	}


	/// <summary>
	/// Puts the target to sleep.
	/// <summary>
	public class PokeBattle_Move_003 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, byte? alltargets= null, bool showanimation= true) { 
		if (pbIsDamaging()) return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
		if (pbTypeImmunityByAbility(pbType(this.type, attacker, opponent), attacker, opponent)) return -1;
		if (opponent.pbCanSleep? (attacker,true,self)){
		  pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		  opponent.pbSleep
		  return 0
		}
		return -1
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		if (opponent.pbCanSleep? (attacker,false,self)){
		  opponent.pbSleep
		}
	  }

	  public object pbEffectAfterHit(Battle.Battler attacker, Battle.Battler opponent, turneffects){
		if (id == Moves.RELIC_SONG){
		  if (attacker.Species == Pokemons.MELOETTA &&
			 !attacker.effects.Transform &&
			 !(attacker.hasWorkingAbility(Abilities.SHEER_FORCE) && self.addlEffect>0) &&
			 !attacker.isFainted()){
			attacker.form=(attacker.form+1)%2
			attacker.pbUpdate(true)
			this.battle.scene.pbChangePokemon(attacker, attacker.pokemon)
			//battle.pbDisplay(_INTL("{1} transformed!", attacker.pbThis))
			//PBDebug.log("[Form changed] #{attacker.pbThis} changed to form #{attacker.form}")
		  }
		}
	  }
	}



	/// <summary>
	/// Makes the target drowsy; it will fall asleep at the end of the next turn. (Yawn)
	/// <summary>
	public class PokeBattle_Move_004 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (!opponent.pbCanSleep? (attacker,true,self)) return -1;
		if (opponent.effects.Yawn>0){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.effects.Yawn=2
		//battle.pbDisplay(_INTL("{1} made {2} drowsy!",attacker.pbThis,opponent.pbThis(true)))
		return 0
	  }
	}



	/// <summary>
	/// Poisons the target.
	/// <summary>
	public class PokeBattle_Move_005 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true) { 
		if (pbIsDamaging()) return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
		if (!opponent.pbCanPoison? (attacker,true,self)) return -1;
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.pbPoison(attacker)
		return 0
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		if (opponent.pbCanPoison? (attacker,false,self)){
		  opponent.pbPoison(attacker)
		}
	  }
	}



	/// <summary>
	/// Badly poisons the target. (Poison Fang, Toxic)
	/// (Handled in Battler's pbSuccessCheck): Hits semi-invulnerable targets if user
	/// is Poison-type and move is status move.
	/// <summary>
	public class PokeBattle_Move_006 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (pbIsDamaging()) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!opponent.pbCanPoison? (attacker,true,self)) return -1;
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.pbPoison(attacker, null,true)
		return 0
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		if (opponent.pbCanPoison? (attacker,false,self)){
		  opponent.pbPoison(attacker, null,true)
		}
	  }
	}



	/// <summary>
	/// Paralyzes the target.
	/// Thunder Wave: Doesn't affect target if move's type has no effect on it.
	/// Bolt Strike: Powers up the next Fusion Flare used this round.
	/// <summary>
	public class PokeBattle_Move_007 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (pbIsDamaging?){
		  ret = super(attacker, opponent, hitnum, alltargets, showanimation)
		  if (opponent.damagestate.CalcDamage>0 && id == Moves.BOLT_STRIKE){
			this.battle.field.FusionFlare=true
		  }
		  return ret
		else
		  if (Id == Moves.THUNDER_WAVE){
			if (pbTypeModifier(type, attacker, opponent)==0){
			  //battle.pbDisplay(_INTL("It doesn't affect {1}...",opponent.pbThis(true)))
			  return -1
			}
		  }
		  if (pbTypeImmunityByAbility(pbType(this.type, attacker, opponent), attacker, opponent)) return -1;
		  if (!opponent.pbCanParalyze? (attacker,true,self)) return -1;
		  pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		  opponent.pbParalyze(attacker)
		  return 0
		}
		return -1
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		if (opponent.pbCanParalyze? (attacker,false,self)){
		  opponent.pbParalyze(attacker)
		}
	  }
	}



	/// <summary>
	/// Paralyzes the target. Accuracy perfect in rain, 50% in sunshine. (Thunder)
	/// (Handled in Battler's pbSuccessCheck): Hits some semi-invulnerable targets.
	/// <summary>
	public class PokeBattle_Move_008 : PokeBattle_Move
	{
		public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		if (opponent.pbCanParalyze? (attacker,false,self)){
		  opponent.pbParalyze(attacker)
		}
	  }

	  public object pbModifyBaseAccuracy(baseaccuracy, Battle.Battler attacker, Battle.Battler opponent){
		case this.battle.pbWeather
		when PBWeather::RAINDANCE, PBWeather::HEAVYRAIN
		  return 0
		when PBWeather::SUNNYDAY, PBWeather::HARSHSUN
		  return 50

		}
		return baseaccuracy
	  }
	}



	/// <summary>
	/// Paralyzes the target. May cause the target to flinch. (Thunder Fang)
	/// <summary>
	public class PokeBattle_Move_009 : PokeBattle_Move
	{
		public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		if (this.battle.pbRandom(10)==0){
		  if (opponent.pbCanParalyze? (attacker,false,self)){
			opponent.pbParalyze(attacker)
		  }

		}
		if (this.battle.pbRandom(10)==0){
		  opponent.pbFlinch(attacker)
		}
	  }
	}



	/// <summary>
	/// Burns the target.
	/// Blue Flare: Powers up the next Fusion Bolt used this round.
	/// <summary>
	public class PokeBattle_Move_00A : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (pbIsDamaging?){
		  ret = super(attacker, opponent, hitnum, alltargets, showanimation)
		  if (opponent.damagestate.CalcDamage>0 && id == Moves.BLUE_FLARE){
			this.battle.field.FusionBolt=true
		  }
		  return ret
		else
		  if (pbTypeImmunityByAbility(pbType(this.type, attacker, opponent), attacker, opponent)) return -1;
		  if (!opponent.pbCanBurn? (attacker,true,self)) return -1;
		  pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		  opponent.pbBurn(attacker)
		  return 0
		}
		return -1
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		if (opponent.pbCanBurn? (attacker,false,self)){
		  opponent.pbBurn(attacker)
		}
	  }
	}



	/// <summary>
	/// Burns the target. May cause the target to flinch. (Fire Fang)
	/// <summary>
	public class PokeBattle_Move_00B : PokeBattle_Move
	{
		public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		if (this.battle.pbRandom(10)==0){
		  if (opponent.pbCanBurn? (attacker,false,self)){
			opponent.pbBurn(attacker)
		  }

		}
		if (this.battle.pbRandom(10)==0){
		  opponent.pbFlinch(attacker)
		}
	  }
	}



	/// <summary>
	/// Freezes the target.
	/// <summary>
	public class PokeBattle_Move_00C : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (pbIsDamaging()) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!opponent.pbCanFreeze? (attacker,true,self)) return -1;
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.pbFreeze
		return 0
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		if (opponent.pbCanFreeze? (attacker,false,self)){
		  opponent.pbFreeze
		}
	  }
	}



	/// <summary>
	/// Freezes the target. Accuracy perfect in hail. (Blizzard)
	/// <summary>
	public class PokeBattle_Move_00D : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (pbIsDamaging()) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!opponent.pbCanFreeze? (attacker,true,self)) return -1;
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.pbFreeze
		return 0
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		if (opponent.pbCanFreeze? (attacker,false,self)){
		  opponent.pbFreeze
		}
	  }

	  public object pbModifyBaseAccuracy(baseaccuracy, Battle.Battler attacker, Battle.Battler opponent){
		if (this.battle.pbWeather==PBWeather::HAIL){
		  return 0
		}
		return baseaccuracy
	  }
	}



	/// <summary>
	/// Freezes the target. May cause the target to flinch. (Ice Fang)
	/// <summary>
	public class PokeBattle_Move_00E : PokeBattle_Move
	{
		public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		if (this.battle.pbRandom(10)==0){
		  if (opponent.pbCanFreeze? (attacker,false,self)){
			opponent.pbFreeze
		  }

		}
		if (this.battle.pbRandom(10)==0){
		  opponent.pbFlinch(attacker)
		}
	  }
	}



	/// <summary>
	/// Causes the target to flinch.
	/// <summary>
	public class PokeBattle_Move_00F : PokeBattle_Move
	{
		public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		opponent.pbFlinch(attacker)
	  }
	}



	/// <summary>
	/// Causes the target to flinch. Does double damage and has perfect accuracy if
	/// the target is Minimized.
	/// <summary>
	public class PokeBattle_Move_010 : PokeBattle_Move
	{
		public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		opponent.pbFlinch(attacker);
	  }

	  public object tramplesMinimize (byte param=1){
		if (id == Moves.DRAGON_RUSH && !Settings.USENEWBATTLEMECHANICS) return false;
		if (param==1 && Settings.USENEWBATTLEMECHANICS) return true; // Perfect accuracy
		if (param==2) return true; // Double damage
		return false;
	  }
	}



	/// <summary>
	/// Causes the target to flinch. Fails if the user is not asleep. (Snore)
	/// <summary>
	public class PokeBattle_Move_011 : PokeBattle_Move
	{
		public object pbCanUseWhileAsleep() { 
		return true
	  }

	  public object pbMoveFailed(Battle.Battler attacker, Battle.Battler opponent){
		return (attacker.status!=PBStatuses::SLEEP)
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		opponent.pbFlinch(attacker)
	  }
	}



	/// <summary>
	/// Causes the target to flinch. Fails if this isn't the user's first turn. (Fake Out)
	/// <summary>
	public class PokeBattle_Move_012 : PokeBattle_Move
	{
		public object pbMoveFailed(Battle.Battler attacker, Battle.Battler opponent){
		return (attacker.turncount>1)
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		opponent.pbFlinch(attacker)
	  }
	}



	/// <summary>
	/// Confuses the target.
	/// <summary>
	public class PokeBattle_Move_013 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (pbIsDamaging()) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (opponent.pbCanConfuse? (attacker,true,self)){
		  pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		  opponent.pbConfuse
		  //battle.pbDisplay(_INTL("{1} became confused!", opponent.pbThis))
		  return 0
		}
		return -1
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		if (opponent.pbCanConfuse? (attacker,false,self)){
		  opponent.pbConfuse
		  //battle.pbDisplay(_INTL("{1} became confused!", opponent.pbThis))
		}
	  }
	}



	/// <summary>
	/// Confuses the target. Chance of causing confusion depends on the cry's volume.
	/// Confusion chance is 0% if user doesn't have a recorded cry. (Chatter)
	/// TODO: Play the actual chatter cry as part of the move animation
	///       this.battle.scene.pbChatter(attacker,opponent) // Just plays cry
	/// <summary>
	public class PokeBattle_Move_014 : PokeBattle_Move
	{
		public object addlEffect
		if (USENEWBATTLEMECHANICS) return 100;
		if (attacker.pokemon && attacker.pokemon.chatter){
		  return attacker.pokemon.chatter.intensity*10/127
		}
		return 0
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		if (opponent.pbCanConfuse? (attacker,false,self)){
		  opponent.pbConfuse
		  //battle.pbDisplay(_INTL("{1} became confused!", opponent.pbThis))
		}
	  }
	}



	/// <summary>
	/// Confuses the target. Accuracy perfect in rain, 50% in sunshine. (Hurricane)
	/// (Handled in Battler's pbSuccessCheck): Hits some semi-invulnerable targets.
	/// <summary>
	public class PokeBattle_Move_015 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (pbIsDamaging()) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (opponent.pbCanConfuse? (attacker,true,self)){
		  pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		  opponent.pbConfuse
		  //battle.pbDisplay(_INTL("{1} became confused!", opponent.pbThis))
		  return 0
		}
		return -1
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		if (opponent.pbCanConfuse? (attacker,false,self)){
		  opponent.pbConfuse
		  //battle.pbDisplay(_INTL("{1} became confused!", opponent.pbThis))
		}
	  }

	  public object pbModifyBaseAccuracy(baseaccuracy, Battle.Battler attacker, Battle.Battler opponent){
		case this.battle.pbWeather
		when PBWeather::RAINDANCE, PBWeather::HEAVYRAIN
		  return 0
		when PBWeather::SUNNYDAY, PBWeather::HARSHSUN
		  return 50

		}
		return baseaccuracy
	  }
	}



	/// <summary>
	/// Attracts the target. (Attract)
	/// <summary>
	public class PokeBattle_Move_016 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (!opponent.pbCanAttract? (attacker)){
		  return -1
		}
		if (!attacker.hasMoldBreaker){
		  if (opponent.hasWorkingAbility(Abilities.AROMAVEIL)){
			//battle.pbDisplay(_INTL("But it failed because of {1}'s {2}!",
			   opponent.pbThis,PBAbilities.getName(opponent.ability)))
			return -1
		  else if opponent.pbPartner.hasWorkingAbility(Abilities.AROMAVEIL)

			//battle.pbDisplay(_INTL("But it failed because of {1}'s {2}!",
			   opponent.pbPartner.pbThis,PBAbilities.getName(opponent.pbPartner.ability)))
			return -1
		  }
		}

		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.pbAttract(attacker)
		return 0
	  }
	}



	/// <summary>
	/// Burns, freezes or paralyzes the target. (Tri Attack)
	/// <summary>
	public class PokeBattle_Move_017 : PokeBattle_Move
	{
		public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		case this.battle.pbRandom(3)
		when 0
		  if (opponent.pbCanBurn? (attacker,false,self)){
			opponent.pbBurn(attacker)
		  }

		when 1
		  if (opponent.pbCanFreeze? (attacker,false,self)){
			opponent.pbFreeze
		  }

		when 2
		  if (opponent.pbCanParalyze? (attacker,false,self)){
			opponent.pbParalyze(attacker)
		  }

		}
	  }
	}



	/// <summary>
	/// Cures user of burn, poison and paralysis. (Refresh)
	/// <summary>
	public class PokeBattle_Move_018 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (attacker.status!=PBStatuses::BURN &&){
		   attacker.status!=PBStatuses::POISON &&
		   attacker.status!=PBStatuses::PARALYSIS
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		else
		  t=attacker.status
		  attacker.pbCureStatus(false)

		  pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)
		  if (t==PBStatuses::BURN){
			//battle.pbDisplay(_INTL("{1} healed its burn!", attacker.pbThis))  
		  else if t==PBStatuses::POISON
			//battle.pbDisplay(_INTL("{1} cured its poisoning!", attacker.pbThis))  
		  else if t==PBStatuses::PARALYSIS
			//battle.pbDisplay(_INTL("{1} cured its paralysis!", attacker.pbThis))  
		  }
		  return 0
		}
	  }
	}



	/// <summary>
	/// Cures all party Pokémon of permanent status problems. (Aromatherapy, Heal Bell)
	/// <summary>
	public class PokeBattle_Move_019 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){

		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)
		if (id == Moves.AROMATHERAPY) {
			//battle.pbDisplay(_INTL("A soothing aroma wafted through the area!"))
		}
		else { 
		  //battle.pbDisplay(_INTL("A bell chimed!"))
		}
		activepkmn =[]
		for i in this.battle.battlers
		  if (attacker.pbIsOpposing? (i.index) || i.isFainted()) continue; //next
		   activepkmn.push(i.pokemonIndex)

		  if (USENEWBATTLEMECHANICS && i.index!=attacker.index && ) continue; //next
			 pbTypeImmunityByAbility(pbType(this.type, attacker, i), attacker, i)
		  case i.status
		  when PBStatuses::PARALYSIS
			//battle.pbDisplay(_INTL("{1} was cured of paralysis.", i.pbThis))
		  when PBStatuses::SLEEP

			//battle.pbDisplay(_INTL("{1}'s sleep was woken.", i.pbThis))
		  when PBStatuses::POISON

			//battle.pbDisplay(_INTL("{1} was cured of its poisoning.", i.pbThis))
		  when PBStatuses::BURN

			//battle.pbDisplay(_INTL("{1}'s burn was healed.", i.pbThis))
		  when PBStatuses::FROZEN

			//battle.pbDisplay(_INTL("{1} was thawed out.", i.pbThis))
		  }
		  i.pbCureStatus(false)

		}
		party = this.battle.pbParty(attacker.index) // NOTE: Considers both parties in multi battles
		for (int i = 0; i < party.length; i++){ 
		  if (activepkmn.include? (i)) continue; //next
		   if (!party[i] || party[i].egg? || party[i].hp<=0) continue; //next
		  case party[i].status
		  when PBStatuses::PARALYSIS
			//battle.pbDisplay(_INTL("{1} was cured of paralysis.", party[i].name))
		  when PBStatuses::SLEEP

			//battle.pbDisplay(_INTL("{1} was woken from its sleep.", party[i].name))
		  when PBStatuses::POISON

			//battle.pbDisplay(_INTL("{1} was cured of its poisoning.", party[i].name))
		  when PBStatuses::BURN

			//battle.pbDisplay(_INTL("{1}'s burn was healed.", party[i].name))
		  when PBStatuses::FROZEN

			//battle.pbDisplay(_INTL("{1} was thawed out.", party[i].name))
		  }
		  party[i].status=0
		  party[i].statusCount=0
		}
		return 0
	  }
	}



	/// <summary>
	/// Safeguards the user's side from being inflicted with status problems. (Safeguard)
	/// <summary>
	public class PokeBattle_Move_01A : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (attacker.OwnSide.Safeguard>0){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		attacker.OwnSide.Safeguard= 5

		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)
		if (!this.battle.pbIsOpposing?(attacker.index)){
		  //battle.pbDisplay(_INTL("Your team became cloaked in a mystical veil!"))
		else
		  //battle.pbDisplay(_INTL("The opposing team became cloaked in a mystical veil!"))
		}
		return 0
	  }
	}



	/// <summary>
	/// User passes its status problem to the target. (Psycho Shift)
	/// <summary>
	public class PokeBattle_Move_01B : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (attacker.status==0 ||){
		  (attacker.status==PBStatuses::PARALYSIS && !opponent.pbCanParalyze? (attacker,false,self)) ||
		  (attacker.status==PBStatuses::SLEEP && !opponent.pbCanSleep? (attacker,false,self)) ||
		  (attacker.status==PBStatuses::POISON && !opponent.pbCanPoison? (attacker,false,self)) ||
		  (attacker.status==PBStatuses::BURN && !opponent.pbCanBurn? (attacker,false,self)) ||
		  (attacker.status==PBStatuses::FROZEN && !opponent.pbCanFreeze? (attacker,false,self))
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)
		case attacker.status
		when PBStatuses::PARALYSIS
		  opponent.pbParalyze(attacker)

		  opponent.pbAbilityCureCheck
		  attacker.pbCureStatus(false)

		  //battle.pbDisplay(_INTL("{1} was cured of paralysis.",attacker.pbThis))
		when PBStatuses::SLEEP

		  opponent.pbSleep
		  opponent.pbAbilityCureCheck
		  attacker.pbCureStatus(false)

		  //battle.pbDisplay(_INTL("{1} woke up.",attacker.pbThis))
		when PBStatuses::POISON

		  opponent.pbPoison(attacker, null, attacker.statusCount!=0)

		  opponent.pbAbilityCureCheck
		  attacker.pbCureStatus(false)

		  //battle.pbDisplay(_INTL("{1} was cured of its poisoning.",attacker.pbThis))
		when PBStatuses::BURN

		  opponent.pbBurn(attacker)
		  opponent.pbAbilityCureCheck
		  attacker.pbCureStatus(false)

		  //battle.pbDisplay(_INTL("{1}'s burn was healed.",attacker.pbThis))
		when PBStatuses::FROZEN

		  opponent.pbFreeze
		  opponent.pbAbilityCureCheck
		  attacker.pbCureStatus(false)

		  //battle.pbDisplay(_INTL("{1} was thawed out.",attacker.pbThis))
		}
		return 0
	  }
	}



	/// <summary>
	/// Increases the user's Attack by 1 stage.
	/// <summary>
	public class PokeBattle_Move_01C : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (pbIsDamaging()) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,true,self)) return -1;
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=attacker.pbIncreaseStat(PBStats::ATTACK,1,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::ATTACK,1,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Increases the user's Defense by 1 stage.
	/// <summary>
	public class PokeBattle_Move_01D : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (pbIsDamaging()) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!attacker.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,true,self)) return -1;
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=attacker.pbIncreaseStat(PBStats::DEFENSE,1,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (attacker.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::DEFENSE,1,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Increases the user's Defense by 1 stage. User curls up. (Defense Curl)
	/// <summary>
	public class PokeBattle_Move_01E : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){

		attacker.effects.DefenseCurl=true
		if (!attacker.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,true,self)) return -1;
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=attacker.pbIncreaseStat(PBStats::DEFENSE,1,attacker,false,self)
		return ret? 0 : -1
	  }
	}



	/// <summary>
	/// Increases the user's Speed by 1 stage.
	/// <summary>
	public class PokeBattle_Move_01F : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (pbIsDamaging()) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!attacker.pbCanIncreaseStatStage? (PBStats::SPEED, attacker,true,self)) return -1;
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=attacker.pbIncreaseStat(PBStats::SPEED,1,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (attacker.pbCanIncreaseStatStage? (PBStats::SPEED, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::SPEED,1,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Increases the user's Special Attack by 1 stage.
	/// <summary>
	public class PokeBattle_Move_020 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (pbIsDamaging()) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!attacker.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,true,self)) return -1;
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=attacker.pbIncreaseStat(PBStats::SPATK,1,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (attacker.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::SPATK,1,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Increases the user's Special Defense by 1 stage.
	/// Charges up user's next attack if it is Electric-type. (Charge)
	/// <summary>
	public class PokeBattle_Move_021 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){

		attacker.effects.Charge=2
		//battle.pbDisplay(_INTL("{1} began charging power!",attacker.pbThis))
		if (attacker.pbCanIncreaseStatStage? (PBStats::SPDEF, attacker,true,self)){
		  pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		  attacker.pbIncreaseStat(PBStats::SPDEF,1,attacker,false,self)
		}
		return 0
	  }
	}



	/// <summary>
	/// Increases the user's evasion by 1 stage.
	/// <summary>
	public class PokeBattle_Move_022 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (pbIsDamaging()) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!attacker.pbCanIncreaseStatStage? (PBStats::EVASION, attacker,true,self)) return -1;
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=attacker.pbIncreaseStat(PBStats::EVASION,1,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (attacker.pbCanIncreaseStatStage? (PBStats::EVASION, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::EVASION,1,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Increases the user's critical hit rate. (Focus Energy)
	/// <summary>
	public class PokeBattle_Move_023 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (pbIsDamaging()) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (attacker.effects.FocusEnergy>=2){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		attacker.effects.FocusEnergy=2
		//battle.pbDisplay(_INTL("{1} is getting pumped!",attacker.pbThis))
		return 0
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (attacker.effects.FocusEnergy<2){
		  attacker.effects.FocusEnergy=2
		  //battle.pbDisplay(_INTL("{1} is getting pumped!",attacker.pbThis))
		}
	  }
	}



	/// <summary>
	/// Increases the user's Attack and Defense by 1 stage each. (Bulk Up)
	/// <summary>
	public class PokeBattle_Move_024 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (!attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self) &&){
		   !attacker.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,false,self)
		  //battle.pbDisplay(_INTL("{1}'s stats won't go any higher!",attacker.pbThis))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		showanim=true
		if (attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::ATTACK,1,attacker,false,self,showanim)
		  showanim=false
		}
		if (attacker.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::DEFENSE,1,attacker,false,self,showanim)
		  showanim=false
		}
		return 0
	  }
	}



	/// <summary>
	/// Increases the user's Attack, Defense and accuracy by 1 stage each. (Coil)
	/// <summary>
	public class PokeBattle_Move_025 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (!attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self) &&){
		   !attacker.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,false,self) &&
		   !attacker.pbCanIncreaseStatStage? (PBStats::ACCURACY, attacker,false,self)
		  //battle.pbDisplay(_INTL("{1}'s stats won't go any higher!",attacker.pbThis))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		showanim=true
		if (attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::ATTACK,1,attacker,false,self,showanim)
		  showanim=false
		}
		if (attacker.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::DEFENSE,1,attacker,false,self,showanim)
		  showanim=false
		}
		if (attacker.pbCanIncreaseStatStage? (PBStats::ACCURACY, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::ACCURACY,1,attacker,false,self,showanim)
		  showanim=false
		}
		return 0
	  }
	}



	/// <summary>
	/// Increases the user's Attack and Speed by 1 stage each. (Dragon Dance)
	/// <summary>
	public class PokeBattle_Move_026 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (!attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self) &&){
		   !attacker.pbCanIncreaseStatStage? (PBStats::SPEED, attacker,false,self)
		  //battle.pbDisplay(_INTL("{1}'s stats won't go any higher!",attacker.pbThis))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		showanim=true
		if (attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::ATTACK,1,attacker,false,self,showanim)
		  showanim=false
		}
		if (attacker.pbCanIncreaseStatStage? (PBStats::SPEED, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::SPEED,1,attacker,false,self,showanim)
		  showanim=false
		}
		return 0
	  }
	}



	/// <summary>
	/// Increases the user's Attack and Special Attack by 1 stage each. (Work Up)
	/// <summary>
	public class PokeBattle_Move_027 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (!attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self) &&){
		   !attacker.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,false,self)
		  //battle.pbDisplay(_INTL("{1}'s stats won't go any higher!",attacker.pbThis))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		showanim=true
		if (attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::ATTACK,1,attacker,false,self,showanim)
		  showanim=false
		}
		if (attacker.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::SPATK,1,attacker,false,self,showanim)
		  showanim=false
		}
		return 0
	  }
	}



	/// <summary>
	/// Increases the user's Attack and Sp. Attack by 1 stage each.
	/// In sunny weather, increase is 2 stages each instead. (Growth)
	/// <summary>
	public class PokeBattle_Move_028 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (!attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self) &&){
		   !attacker.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,false,self)
		  //battle.pbDisplay(_INTL("{1}'s stats won't go any higher!",attacker.pbThis))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		showanim=true
		increment=1
		if (this.battle.pbWeather==PBWeather::SUNNYDAY ||){
		   this.battle.pbWeather==PBWeather::HARSHSUN
		  increment = 2

		}
		if (attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::ATTACK, increment, attacker,false,self,showanim)
		  showanim=false
		}
		if (attacker.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::SPATK, increment, attacker,false,self,showanim)
		  showanim=false
		}
		return 0
	  }
	}



	/// <summary>
	/// Increases the user's Attack and accuracy by 1 stage each. (Hone Claws)
	/// <summary>
	public class PokeBattle_Move_029 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (!attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self) &&){
		   !attacker.pbCanIncreaseStatStage? (PBStats::ACCURACY, attacker,false,self)
		  //battle.pbDisplay(_INTL("{1}'s stats won't go any higher!",attacker.pbThis))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		showanim=true
		if (attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::ATTACK,1,attacker,false,self,showanim)
		  showanim=false
		}
		if (attacker.pbCanIncreaseStatStage? (PBStats::ACCURACY, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::ACCURACY,1,attacker,false,self,showanim)
		  showanim=false
		}
		return 0
	  }
	}



	/// <summary>
	/// Increases the user's Defense and Special Defense by 1 stage each. (Cosmic Power)
	/// <summary>
	public class PokeBattle_Move_02A : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (!attacker.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,false,self) &&){
		   !attacker.pbCanIncreaseStatStage? (PBStats::SPDEF, attacker,false,self)
		  //battle.pbDisplay(_INTL("{1}'s stats won't go any higher!",attacker.pbThis))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		showanim=true
		if (attacker.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::DEFENSE,1,attacker,false,self,showanim)
		  showanim=false
		}
		if (attacker.pbCanIncreaseStatStage? (PBStats::SPDEF, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::SPDEF,1,attacker,false,self,showanim)
		  showanim=false
		}
		return 0
	  }
	}



	/// <summary>
	/// Increases the user's Sp. Attack, Sp. Defense and Speed by 1 stage each. (Quiver Dance)
	/// <summary>
	public class PokeBattle_Move_02B : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (!attacker.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,false,self) &&){
		   !attacker.pbCanIncreaseStatStage? (PBStats::SPDEF, attacker,false,self) &&
		   !attacker.pbCanIncreaseStatStage? (PBStats::SPEED, attacker,false,self)
		  //battle.pbDisplay(_INTL("{1}'s stats won't go any higher!",attacker.pbThis))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		showanim=true
		if (attacker.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::SPATK,1,attacker,false,self,showanim)
		  showanim=false
		}
		if (attacker.pbCanIncreaseStatStage? (PBStats::SPDEF, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::SPDEF,1,attacker,false,self,showanim)
		  showanim=false
		}
		if (attacker.pbCanIncreaseStatStage? (PBStats::SPEED, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::SPEED,1,attacker,false,self,showanim)
		  showanim=false
		}
		return 0
	  }
	}



	/// <summary>
	/// Increases the user's Sp. Attack and Sp. Defense by 1 stage each. (Calm Mind)
	/// <summary>
	public class PokeBattle_Move_02C : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (!attacker.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,false,self) &&){
		   !attacker.pbCanIncreaseStatStage? (PBStats::SPDEF, attacker,false,self)
		  //battle.pbDisplay(_INTL("{1}'s stats won't go any higher!",attacker.pbThis))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		showanim=true
		if (attacker.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::SPATK,1,attacker,false,self,showanim)
		  showanim=false
		}
		if (attacker.pbCanIncreaseStatStage? (PBStats::SPDEF, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::SPDEF,1,attacker,false,self,showanim)
		  showanim=false
		}
		return 0
	  }
	}



	/// <summary>
	/// Increases the user's Attack, Defense, Speed, Special Attack and Special Defense
	/// by 1 stage each. (AncientPower, Ominous Wind, Silver Wind)
	/// <summary>
	public class PokeBattle_Move_02D : PokeBattle_Move
	{
		public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){

		showanim=true
		if (attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::ATTACK,1,attacker,false,self,showanim)
		  showanim=false
		}
		if (attacker.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::DEFENSE,1,attacker,false,self,showanim)
		  showanim=false
		}
		if (attacker.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::SPATK,1,attacker,false,self,showanim)
		  showanim=false
		}
		if (attacker.pbCanIncreaseStatStage? (PBStats::SPDEF, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::SPDEF,1,attacker,false,self,showanim)
		  showanim=false
		}
		if (attacker.pbCanIncreaseStatStage? (PBStats::SPEED, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::SPEED,1,attacker,false,self,showanim)
		  showanim=false
		}
	  }
	}



	/// <summary>
	/// Increases the user's Attack by 2 stages.
	/// <summary>
	public class PokeBattle_Move_02E : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (pbIsDamaging()) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,true,self)) return -1;
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=attacker.pbIncreaseStat(PBStats::ATTACK,2,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::ATTACK,2,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Increases the user's Defense by 2 stages.
	/// <summary>
	public class PokeBattle_Move_02F : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (pbIsDamaging()) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!attacker.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,true,self)) return -1;
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=attacker.pbIncreaseStat(PBStats::DEFENSE,2,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (attacker.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::DEFENSE,2,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Increases the user's Speed by 2 stages.
	/// <summary>
	public class PokeBattle_Move_030 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (pbIsDamaging()) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!attacker.pbCanIncreaseStatStage? (PBStats::SPEED, attacker,true,self)) return -1;
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=attacker.pbIncreaseStat(PBStats::SPEED,2,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (attacker.pbCanIncreaseStatStage? (PBStats::SPEED, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::SPEED,2,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Increases the user's Speed by 2 stages. Lowers user's weight by 100kg. (Autotomize)
	/// <summary>
	public class PokeBattle_Move_031 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (!attacker.pbCanIncreaseStatStage? (PBStats::SPEED, attacker,true,self)) return -1;
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=attacker.pbIncreaseStat(PBStats::SPEED,2,attacker,false,self)
		if (ret){
		  attacker.effects.WeightChange-=1000

		  //battle.pbDisplay(_INTL("{1} became nimble!", attacker.pbThis))
		}
		return ret? 0 : -1
	  }
	}



	/// <summary>
	/// Increases the user's Special Attack by 2 stages.
	/// <summary>
	public class PokeBattle_Move_032 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (pbIsDamaging()) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!attacker.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,true,self)) return -1;
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=attacker.pbIncreaseStat(PBStats::SPATK,2,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (attacker.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::SPATK,2,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Increases the user's Special Defense by 2 stages.
	/// <summary>
	public class PokeBattle_Move_033 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (pbIsDamaging()) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!attacker.pbCanIncreaseStatStage? (PBStats::SPDEF, attacker,true,self)) return -1;
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=attacker.pbIncreaseStat(PBStats::SPDEF,2,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (attacker.pbCanIncreaseStatStage? (PBStats::SPDEF, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::SPDEF,2,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Increases the user's evasion by 2 stages. Minimizes the user. (Minimize)
	/// <summary>
	public class PokeBattle_Move_034 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (pbIsDamaging()) return super(attacker, opponent, hitnum, alltargets, showanimation);
			attacker.effects.Minimize= true
		if (!attacker.pbCanIncreaseStatStage?(PBStats::EVASION, attacker,true, self)) return -1;

		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=attacker.pbIncreaseStat(PBStats::EVASION,2,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){

		attacker.effects.Minimize=true
		if (attacker.pbCanIncreaseStatStage? (PBStats::EVASION, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::EVASION,2,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Decreases the user's Defense and Special Defense by 1 stage each. (Shell Smash)
	/// Increases the user's Attack, Speed and Special Attack by 2 stages each.
	/// <summary>
	public class PokeBattle_Move_035 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (!attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self) &&){
		   !attacker.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,false,self) &&
		   !attacker.pbCanIncreaseStatStage? (PBStats::SPEED, attacker,false,self)
		  //battle.pbDisplay(_INTL("{1}'s stats won't go any higher!",attacker.pbThis))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		showanim=true
		if (attacker.pbCanReduceStatStage? (PBStats::DEFENSE, attacker,false,self)){
		  attacker.pbReduceStat(PBStats::DEFENSE,1,attacker,false,self,showanim)
		  showanim=false
		}
		if (attacker.pbCanReduceStatStage? (PBStats::SPDEF, attacker,false,self)){
		  attacker.pbReduceStat(PBStats::SPDEF,1,attacker,false,self,showanim)
		  showanim=false
		}
		showanim = true
		if (attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::ATTACK,2,attacker,false,self,showanim)
		  showanim=false
		}
		if (attacker.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::SPATK,2,attacker,false,self,showanim)
		  showanim=false
		}
		if (attacker.pbCanIncreaseStatStage? (PBStats::SPEED, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::SPEED,2,attacker,false,self,showanim)
		  showanim=false
		}
		return 0
	  }
	}



	/// <summary>
	/// Increases the user's Speed by 2 stages, and its Attack by 1 stage. (Shift Gear)
	/// <summary>
	public class PokeBattle_Move_036 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (!attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self) &&){
		   !attacker.pbCanIncreaseStatStage? (PBStats::SPEED, attacker,false,self)
		  //battle.pbDisplay(_INTL("{1}'s stats won't go any higher!",attacker.pbThis))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		showanim=true
		if (attacker.pbCanIncreaseStatStage? (PBStats::SPEED, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::SPEED,2,attacker,false,self,showanim)
		  showanim=false
		}
		if (attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::ATTACK,1,attacker,false,self,showanim)
		  showanim=false
		}
		return 0
	  }
	}



	/// <summary>
	/// Increases one random stat of the user by 2 stages (except HP). (Acupressure)
	/// <summary>
	public class PokeBattle_Move_037 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (attacker.index!=opponent.index ){
		  if (opponent.effects.Substitute>0 && !ignoresSubstitute? (attacker)) ||
			 opponent.OwnSide.CraftyShield
			//battle.pbDisplay(_INTL("But it failed!"))
			return -1
		  }
		}

		array=[]
		foreach (var i in [PBStats::ATTACK, PBStats::DEFENSE, PBStats::SPEED,
				  PBStats::SPATK, PBStats::SPDEF, PBStats::ACCURACY, PBStats::EVASION]){ 
		array.push(i) if opponent.pbCanIncreaseStatStage? (i, attacker,false,self)
		}
		if (array.length==0){
		  //battle.pbDisplay(_INTL("{1}'s stats won't go any higher!",opponent.pbThis))
		  return -1
		}
		stat = array[this.battle.pbRandom(array.length)]

		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=opponent.pbIncreaseStat(stat,2,attacker,false,self)
		return 0
	  }
	}



	/// <summary>
	/// Increases the user's Defense by 3 stages.
	/// <summary>
	public class PokeBattle_Move_038 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (pbIsDamaging()) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!attacker.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,true,self)) return -1;
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=attacker.pbIncreaseStat(PBStats::DEFENSE,3,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (attacker.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::DEFENSE,3,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Increases the user's Special Attack by 3 stages.
	/// <summary>
	public class PokeBattle_Move_039 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (pbIsDamaging()) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!attacker.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,true,self)) return -1;
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=attacker.pbIncreaseStat(PBStats::SPATK,3,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (attacker.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::SPATK,3,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Reduces the user's HP by half of max, and sets its Attack to maximum. (Belly Drum)
	/// <summary>
	public class PokeBattle_Move_03A : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (attacker.hp<=(attacker.TotalHP/2).floor ||){
		   !attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self)
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		attacker.pbReduceHP((attacker.TotalHP/2).floor)
		if (attacker.hasWorkingAbility(Abilities.CONTRARY)){
		  attacker.stages[PBStats::ATTACK]=-6
		  this.battle.pbCommonAnimation("StatDown",attacker,null)
		  //battle.pbDisplay(_INTL("{1} cut its own HP and minimized its Attack!",attacker.pbThis))
		else
		  attacker.stages[PBStats::ATTACK]=6
		  this.battle.pbCommonAnimation("StatUp",attacker,null)
		  //battle.pbDisplay(_INTL("{1} cut its own HP and maximized its Attack!",attacker.pbThis))
		}
		return 0
	  }
	}



	/// <summary>
	/// Decreases the user's Attack and Defense by 1 stage each. (Superpower)
	/// <summary>
	public class PokeBattle_Move_03B : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){

		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if (opponent.damagestate.CalcDamage>0){
		  showanim=true
		  if (attacker.pbCanReduceStatStage? (PBStats::ATTACK, attacker,false,self)){
			attacker.pbReduceStat(PBStats::ATTACK,1,attacker,false,self,showanim)
			showanim=false
		  }
		  if (attacker.pbCanReduceStatStage? (PBStats::DEFENSE, attacker,false,self)){
			attacker.pbReduceStat(PBStats::DEFENSE,1,attacker,false,self,showanim)
			showanim=false
		  }
		}
		return ret
	  }
	}



	/// <summary>
	/// Decreases the user's Defense and Special Defense by 1 stage each. (Close Combat)
	/// <summary>
	public class PokeBattle_Move_03C : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){

		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if (opponent.damagestate.CalcDamage>0){
		  showanim=true
		  if (attacker.pbCanReduceStatStage? (PBStats::DEFENSE, attacker,false,self)){
			attacker.pbReduceStat(PBStats::DEFENSE,1,attacker,false,self,showanim)
			showanim=false
		  }
		  if (attacker.pbCanReduceStatStage? (PBStats::SPDEF, attacker,false,self)){
			attacker.pbReduceStat(PBStats::SPDEF,1,attacker,false,self,showanim)
			showanim=false
		  }
		}
		return ret
	  }
	}



	/// <summary>
	/// Decreases the user's Defense, Special Defense and Speed by 1 stage each.
	/// User's ally loses 1/16 of its total HP. (V-create)
	/// <summary>
	public class PokeBattle_Move_03D : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){

		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if (opponent.damagestate.CalcDamage>0){
		  if (attacker.pbPartner && !attacker.pbPartner.isFainted()){
			attacker.pbPartner.pbReduceHP((attacker.pbPartner.TotalHP/16).floor,true)
		  }
		  showanim = true
		  if (attacker.pbCanReduceStatStage? (PBStats::SPEED, attacker,false,self)){
			attacker.pbReduceStat(PBStats::SPEED,1,attacker,false,self,showanim)
			showanim=false
		  }
		  if (attacker.pbCanReduceStatStage? (PBStats::DEFENSE, attacker,false,self)){
			attacker.pbReduceStat(PBStats::DEFENSE,1,attacker,false,self,showanim)
			showanim=false
		  }
		  if (attacker.pbCanReduceStatStage? (PBStats::SPDEF, attacker,false,self)){
			attacker.pbReduceStat(PBStats::SPDEF,1,attacker,false,self,showanim)
			showanim=false
		  }
		}
		return ret
	  }
	}



	/// <summary>
	/// Decreases the user's Speed by 1 stage.
	/// <summary>
	public class PokeBattle_Move_03E : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){

		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if (opponent.damagestate.CalcDamage>0){
		  if (attacker.pbCanReduceStatStage? (PBStats::SPEED, attacker,false,self)){
			attacker.pbReduceStat(PBStats::SPEED,1,attacker,false,self)
		  }
		}
		return ret
	  }
	}



	/// <summary>
	/// Decreases the user's Special Attack by 2 stages.
	/// <summary>
	public class PokeBattle_Move_03F : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){

		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if (opponent.damagestate.CalcDamage>0){
		  if (attacker.pbCanReduceStatStage? (PBStats::SPATK, attacker,false,self)){
			attacker.pbReduceStat(PBStats::SPATK,2,attacker,false,self)
		  }
		}
		return ret
	  }
	}



	/// <summary>
	/// Increases the target's Special Attack by 1 stage. Confuses the target. (Flatter)
	/// <summary>
	public class PokeBattle_Move_040 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (opponent.effects.Substitute>0 && !ignoresSubstitute? (attacker)){
		   //battle.pbDisplay(_INTL("{1}'s attack missed!", attacker.pbThis))
		  return -1
		}
		ret = -1

		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)
		if (opponent.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,false,self)){
		  opponent.pbIncreaseStat(PBStats::SPATK,1,attacker,false,self)
		  ret=0
		}
		if (opponent.pbCanConfuse? (attacker,true,self)){
		  opponent.pbConfuse
		  //battle.pbDisplay(_INTL("{1} became confused!", opponent.pbThis))
		  ret=0
		}
		return ret
	  }
	}



	/// <summary>
	/// Increases the target's Attack by 2 stages. Confuses the target. (Swagger)
	/// <summary>
	public class PokeBattle_Move_041 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (opponent.effects.Substitute>0 && !ignoresSubstitute? (attacker)){
		   //battle.pbDisplay(_INTL("{1}'s attack missed!", attacker.pbThis))
		  return -1
		}
		ret = -1

		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)
		if (opponent.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self)){
		  opponent.pbIncreaseStat(PBStats::ATTACK,2,attacker,false,self)
		  ret=0
		}
		if (opponent.pbCanConfuse? (attacker,true,self)){
		  opponent.pbConfuse
		  //battle.pbDisplay(_INTL("{1} became confused!", opponent.pbThis))
		  ret=0
		}
		return ret
	  }
	}



	/// <summary>
	/// Decreases the target's Attack by 1 stage.
	/// <summary>
	public class PokeBattle_Move_042 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (pbIsDamaging()) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!opponent.pbCanReduceStatStage? (PBStats::ATTACK, attacker,true,self)) return -1;
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=opponent.pbReduceStat(PBStats::ATTACK,1,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		if (opponent.pbCanReduceStatStage? (PBStats::ATTACK, attacker,false,self)){
		  opponent.pbReduceStat(PBStats::ATTACK,1,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Decreases the target's Defense by 1 stage.
	/// <summary>
	public class PokeBattle_Move_043 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (pbIsDamaging()) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!opponent.pbCanReduceStatStage? (PBStats::DEFENSE, attacker,true,self)) return -1;
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=opponent.pbReduceStat(PBStats::DEFENSE,1,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		if (opponent.pbCanReduceStatStage? (PBStats::DEFENSE, attacker,false,self)){
		  opponent.pbReduceStat(PBStats::DEFENSE,1,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Decreases the target's Speed by 1 stage.
	/// <summary>
	public class PokeBattle_Move_044 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (pbIsDamaging()) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!opponent.pbCanReduceStatStage? (PBStats::SPEED, attacker,true,self)) return -1;
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=opponent.pbReduceStat(PBStats::SPEED,1,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		if (opponent.pbCanReduceStatStage? (PBStats::SPEED, attacker,false,self)){
		  opponent.pbReduceStat(PBStats::SPEED,1,attacker,false,self)
		}
	  }

	  public object pbModifyDamage(damagemult, Battle.Battler attacker, Battle.Battler opponent){
		if (id == Moves.BULLDOZE &&){
		   this.battle.field.GrassyTerrain>0
		  return (int)Math.Round(damagemult/2.0f)
		}
		return damagemult
	  }
	}



	/// <summary>
	/// Decreases the target's Special Attack by 1 stage.
	/// <summary>
	public class PokeBattle_Move_045 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (pbIsDamaging()) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!opponent.pbCanReduceStatStage? (PBStats::SPATK, attacker,true,self)) return -1;
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=opponent.pbReduceStat(PBStats::SPATK,1,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		if (opponent.pbCanReduceStatStage? (PBStats::SPATK, attacker,false,self)){
		  opponent.pbReduceStat(PBStats::SPATK,1,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Decreases the target's Special Defense by 1 stage.
	/// <summary>
	public class PokeBattle_Move_046 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (pbIsDamaging()) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!opponent.pbCanReduceStatStage? (PBStats::SPDEF, attacker,true,self)) return -1;
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=opponent.pbReduceStat(PBStats::SPDEF,1,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		if (opponent.pbCanReduceStatStage? (PBStats::SPDEF, attacker,false,self)){
		  opponent.pbReduceStat(PBStats::SPDEF,1,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Decreases the target's accuracy by 1 stage.
	/// <summary>
	public class PokeBattle_Move_047 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (pbIsDamaging()) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!opponent.pbCanReduceStatStage? (PBStats::ACCURACY, attacker,true,self)) return -1;
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=opponent.pbReduceStat(PBStats::ACCURACY,1,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		if (opponent.pbCanReduceStatStage? (PBStats::ACCURACY, attacker,false,self)){
		  opponent.pbReduceStat(PBStats::ACCURACY,1,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Decreases the target's evasion by 1 stage OR 2 stages. (Sweet Scent)
	/// <summary>
	public class PokeBattle_Move_048 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (pbIsDamaging()) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!opponent.pbCanReduceStatStage? (PBStats::EVASION, attacker,true,self)) return -1;
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		increment=(USENEWBATTLEMECHANICS)? 2 : 1
		ret=opponent.pbReduceStat(PBStats::EVASION, increment, attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		if (opponent.pbCanReduceStatStage? (PBStats::EVASION, attacker,false,self)){
		  increment=(USENEWBATTLEMECHANICS)? 2 : 1
		  opponent.pbReduceStat(PBStats::EVASION, increment, attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Decreases the target's evasion by 1 stage. Ends all barriers and entry
	/// hazards for the target's side OR on both sides. (Defog)
	/// <summary>
	public class PokeBattle_Move_049 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (pbIsDamaging()) return super(attacker, opponent, hitnum, alltargets, showanimation);
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.pbReduceStat(PBStats::EVASION,1,attacker,false,self)
		opponent.OwnSide.Reflect     = 0
		opponent.OwnSide.LightScreen = 0
		opponent.OwnSide.Mist        = 0
		opponent.OwnSide.Safeguard   = 0
		opponent.OwnSide.Spikes      = 0
		opponent.OwnSide.StealthRock = false
		opponent.OwnSide.StickyWeb   = false
		opponent.OwnSide.ToxicSpikes = 0
		if (USENEWBATTLEMECHANICS){
		  opponent.pbOpposingSide.effects.Reflect     = 0

		  opponent.pbOpposingSide.effects.LightScreen = 0

		  opponent.pbOpposingSide.effects.Mist        = 0

		  opponent.pbOpposingSide.effects.Safeguard   = 0

		  opponent.pbOpposingSide.effects.Spikes      = 0

		  opponent.pbOpposingSide.effects.StealthRock = false

		  opponent.pbOpposingSide.effects.StickyWeb   = false

		  opponent.pbOpposingSide.effects.ToxicSpikes = 0

		}
		return 0
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (!opponent.damagestate.Substitute){
		  if (opponent.pbCanReduceStatStage?(PBStats::EVASION, attacker,false, self)){

			opponent.pbReduceStat(PBStats::EVASION,1,attacker,false,self)
		  }
		}

		opponent.OwnSide.Reflect     = 0
		opponent.OwnSide.LightScreen = 0
		opponent.OwnSide.Mist        = 0
		opponent.OwnSide.Safeguard   = 0
		opponent.OwnSide.Spikes      = 0
		opponent.OwnSide.StealthRock = false
		opponent.OwnSide.StickyWeb   = false
		opponent.OwnSide.ToxicSpikes = 0
		if (USENEWBATTLEMECHANICS){
		  opponent.pbOpposingSide.effects.Reflect     = 0

		  opponent.pbOpposingSide.effects.LightScreen = 0

		  opponent.pbOpposingSide.effects.Mist        = 0

		  opponent.pbOpposingSide.effects.Safeguard   = 0

		  opponent.pbOpposingSide.effects.Spikes      = 0

		  opponent.pbOpposingSide.effects.StealthRock = false

		  opponent.pbOpposingSide.effects.StickyWeb   = false

		  opponent.pbOpposingSide.effects.ToxicSpikes = 0

		}
	  }
	}



	/// <summary>
	/// Decreases the target's Attack and Defense by 1 stage each. (Tickle)
	/// <summary>
	public class PokeBattle_Move_04A : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		// Replicates public object pbCanReduceStatStage? so that certain messages aren't shown
		// multiple times
		if (opponent.effects.Substitute>0 && !ignoresSubstitute? (attacker)){
		   //battle.pbDisplay(_INTL("{1}'s attack missed!", attacker.pbThis))
		  return -1
		}
		if (opponent.pbTooLow? (PBStats::ATTACK) &&){
		   opponent.pbTooLow? (PBStats::DEFENSE)
		   //battle.pbDisplay(_INTL("{1}'s stats won't go any lower!", opponent.pbThis))
		  return -1
		}
		if (opponent.OwnSide.Mist>0){
		  //battle.pbDisplay(_INTL("{1} is protected by Mist!",opponent.pbThis))
		  return -1
		}
		if (!attacker.hasMoldBreaker){
		  if (opponent.hasWorkingAbility(Abilities.CLEARBODY) ||){
			 opponent.hasWorkingAbility(Abilities.WHITESMOKE)
			//battle.pbDisplay(_INTL("{1}'s {2} prevents stat loss!",opponent.pbThis,
			   PBAbilities.getName(opponent.ability)))
			return -1
		  }
		}

		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=-1; showanim=true
		if (!attacker.hasMoldBreaker && opponent.hasWorkingAbility(Abilities.HYPERCUTTER) &&){
		   !opponent.pbTooLow? (PBStats::ATTACK)
		   abilityname = PBAbilities.getName(opponent.ability)

		  //battle.pbDisplay(_INTL("{1}'s {2} prevents Attack loss!",opponent.pbThis,abilityname))
		else if opponent.pbReduceStat(PBStats::ATTACK,1, attacker,false, self, showanim)

		  ret=0; showanim=false
		}
		if (!attacker.hasMoldBreaker && opponent.hasWorkingAbility(Abilities.BIGPECKS) &&){
		   !opponent.pbTooLow? (PBStats::DEFENSE)
		   abilityname = PBAbilities.getName(opponent.ability)

		  //battle.pbDisplay(_INTL("{1}'s {2} prevents Defense loss!",opponent.pbThis,abilityname))
		else if opponent.pbReduceStat(PBStats::DEFENSE,1, attacker,false, self, showanim)

		  ret=0; showanim=false
		}
		return ret
	  }
	}



	/// <summary>
	/// Decreases the target's Attack by 2 stages.
	/// <summary>
	public class PokeBattle_Move_04B : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (pbIsDamaging()) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!opponent.pbCanReduceStatStage? (PBStats::ATTACK, attacker,true,self)) return -1;
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=opponent.pbReduceStat(PBStats::ATTACK,2,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		if (opponent.pbCanReduceStatStage? (PBStats::ATTACK, attacker,false,self)){
		  opponent.pbReduceStat(PBStats::ATTACK,2,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Decreases the target's Defense by 2 stages. (Screech)
	/// <summary>
	public class PokeBattle_Move_04C : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (pbIsDamaging()) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!opponent.pbCanReduceStatStage? (PBStats::DEFENSE, attacker,true,self)) return -1;
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=opponent.pbReduceStat(PBStats::DEFENSE,2,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		if (opponent.pbCanReduceStatStage? (PBStats::DEFENSE, attacker,false,self)){
		  opponent.pbReduceStat(PBStats::DEFENSE,2,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Decreases the target's Speed by 2 stages. (Cotton Spore, Scary Face, String Shot)
	/// <summary>
	public class PokeBattle_Move_04D : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (pbIsDamaging()) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (pbTypeImmunityByAbility(pbType(this.type, attacker, opponent), attacker, opponent)) return -1;
		if (!opponent.pbCanReduceStatStage? (PBStats::SPEED, attacker,true,self)) return -1;
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		increment=(id == Moves.STRING_SHOT && !Settings.USENEWBATTLEMECHANICS) ? 1 : 2
		ret=opponent.pbReduceStat(PBStats::SPEED, increment, attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		if (opponent.pbCanReduceStatStage? (PBStats::SPEED, attacker,false,self)){
		  increment=(id == Moves.STRING_SHOT && !Settings.USENEWBATTLEMECHANICS) ? 1 : 2
		  opponent.pbReduceStat(PBStats::SPEED, increment, attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Decreases the target's Special Attack by 2 stages. Only works on the opposite
	/// gender. (Captivate)
	/// <summary>
	public class PokeBattle_Move_04E : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (pbIsDamaging()) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!opponent.pbCanReduceStatStage? (PBStats::SPATK, attacker,true,self)) return -1;
		if (attacker.gender==2 || opponent.gender==2 || attacker.gender==opponent.gender){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if (!attacker.hasMoldBreaker && opponent.hasWorkingAbility(Abilities.OBLIVIOUS)){
		  //battle.pbDisplay(_INTL("{1}'s {2} prevents romance!",opponent.pbThis,
			 PBAbilities.getName(opponent.ability)))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=opponent.pbReduceStat(PBStats::SPATK,2,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		if (attacker.gender!=2 && opponent.gender!=2 && attacker.gender!=opponent.gender){
		  if (attacker.hasMoldBreaker || !opponent.hasWorkingAbility(Abilities.OBLIVIOUS)){
			if (opponent.pbCanReduceStatStage? (PBStats::SPATK, attacker,false,self)){
			  opponent.pbReduceStat(PBStats::SPATK,2,attacker,false,self)
			}
		  }

		}
	  }
	}



	/// <summary>
	/// Decreases the target's Special Defense by 2 stages.
	/// <summary>
	public class PokeBattle_Move_04F : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (pbIsDamaging()) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!opponent.pbCanReduceStatStage? (PBStats::SPDEF, attacker,true,self)) return -1;
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=opponent.pbReduceStat(PBStats::SPDEF,2,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		if (opponent.pbCanReduceStatStage? (PBStats::SPDEF, attacker,false,self)){
		  opponent.pbReduceStat(PBStats::SPDEF,2,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Resets all target's stat stages to 0. (Clear Smog)
	/// <summary>
	public class PokeBattle_Move_050 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){

		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if (opponent.damagestate.CalcDamage>0 && !opponent.damagestate.Substitute){
		  opponent.stages[PBStats::ATTACK]   = 0

		  opponent.stages[PBStats::DEFENSE]  = 0

		  opponent.stages[PBStats::SPEED]    = 0

		  opponent.stages[PBStats::SPATK]    = 0

		  opponent.stages[PBStats::SPDEF]    = 0

		  opponent.stages[PBStats::ACCURACY] = 0

		  opponent.stages[PBStats::EVASION]  = 0

		  //battle.pbDisplay(_INTL("{1}'s stat changes were removed!", opponent.pbThis))
		}
		return ret
	  }
	}



	/// <summary>
	/// Resets all stat stages for all battlers to 0. (Haze)
	/// <summary>
	public class PokeBattle_Move_051 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		for (int i = 0; i < 4; i++){ 
		  this.battle.battlers[i].stages[PBStats::ATTACK]   = 0
		  this.battle.battlers[i].stages[PBStats::DEFENSE]  = 0
		  this.battle.battlers[i].stages[PBStats::SPEED]    = 0
		  this.battle.battlers[i].stages[PBStats::SPATK]    = 0
		  this.battle.battlers[i].stages[PBStats::SPDEF]    = 0
		  this.battle.battlers[i].stages[PBStats::ACCURACY] = 0
		  this.battle.battlers[i].stages[PBStats::EVASION]  = 0
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		//battle.pbDisplay(_INTL("All stat changes were eliminated!"))
		return 0
	  }
	}



	/// <summary>
	/// User and target swap their Attack and Special Attack stat stages. (Power Swap)
	/// <summary>
	public class PokeBattle_Move_052 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){

		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		astage=attacker.stages
		ostage = opponent.stages

		astage[PBStats::ATTACK],ostage[PBStats::ATTACK]=ostage[PBStats::ATTACK],astage[PBStats::ATTACK]
		astage[PBStats::SPATK], ostage[PBStats::SPATK] = ostage[PBStats::SPATK], astage[PBStats::SPATK]

		//battle.pbDisplay(_INTL("{1} switched all changes to its Attack and Sp. Atk with the target!",attacker.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// User and target swap their Defense and Special Defense stat stages. (Guard Swap)
	/// <summary>
	public class PokeBattle_Move_053 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){

		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		astage=attacker.stages
		ostage = opponent.stages

		astage[PBStats::DEFENSE],ostage[PBStats::DEFENSE]=ostage[PBStats::DEFENSE],astage[PBStats::DEFENSE]
		astage[PBStats::SPDEF], ostage[PBStats::SPDEF] = ostage[PBStats::SPDEF], astage[PBStats::SPDEF]

		//battle.pbDisplay(_INTL("{1} switched all changes to its Defense and Sp. Def with the target!",attacker.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// User and target swap all their stat stages. (Heart Swap)
	/// <summary>
	public class PokeBattle_Move_054 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){

		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)
		foreach (var i in [PBStats::ATTACK, PBStats::DEFENSE, PBStats::SPEED,
				  PBStats::SPATK, PBStats::SPDEF, PBStats::ACCURACY, PBStats::EVASION]){ 
		attacker.stages[i],opponent.stages[i]=opponent.stages[i],attacker.stages[i]
	  }

		//battle.pbDisplay(_INTL("{1} switched stat changes with the target!",attacker.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// User copies the target's stat stages. (Psych Up)
	/// <summary>
	public class PokeBattle_Move_055 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (opponent.OwnSide.CraftyShield){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)
		foreach (var i in [PBStats::ATTACK, PBStats::DEFENSE, PBStats::SPEED,
				  PBStats::SPATK, PBStats::SPDEF, PBStats::ACCURACY, PBStats::EVASION]){ 
		attacker.stages[i]=opponent.stages[i]
	  }

		//battle.pbDisplay(_INTL("{1} copied {2}'s stat changes!",attacker.pbThis,opponent.pbThis(true)))
		return 0
	  }
	}



	/// <summary>
	/// For 5 rounds, user's and ally's stat stages cannot be lowered by foes. (Mist)
	/// <summary>
	public class PokeBattle_Move_056 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (attacker.OwnSide.Mist>0){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		attacker.OwnSide.Mist=5
		if (!this.battle.pbIsOpposing? (attacker.index)){
		   //battle.pbDisplay(_INTL("Your team became shrouded in mist!"))
		else
		  //battle.pbDisplay(_INTL("The opposing team became shrouded in mist!"))
		}
		return 0
	  }
	}



	/// <summary>
	/// Swaps the user's Attack and Defense stats. (Power Trick)
	/// <summary>
	public class PokeBattle_Move_057 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){

		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		attacker.attack,attacker.defense=attacker.defense,attacker.attack
		attacker.effects.PowerTrick= !attacker.effects.PowerTrick

		//battle.pbDisplay(_INTL("{1} switched its Attack and Defense!", attacker.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// Averages the user's and target's Attack.
	/// Averages the user's and target's Special Attack. (Power Split)
	/// <summary>
	public class PokeBattle_Move_058 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (opponent.effects.Substitute>0 && !ignoresSubstitute? (attacker)){
		   //battle.pbDisplay(_INTL("But it failed!"))  
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		avatk=((attacker.attack+opponent.attack)/2).floor
		avspatk = ((attacker.spatk + opponent.spatk) / 2).floor

		attacker.attack=opponent.attack=avatk
		attacker.spatk=opponent.spatk= avspatk

		//battle.pbDisplay(_INTL("{1} shared its power with the target!", attacker.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// Averages the user's and target's Defense.
	/// Averages the user's and target's Special Defense. (Guard Split)
	/// <summary>
	public class PokeBattle_Move_059 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (opponent.effects.Substitute>0 && !ignoresSubstitute? (attacker)){
		   //battle.pbDisplay(_INTL("But it failed!"))  
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		avdef=((attacker.defense+opponent.defense)/2).floor
		avspdef = ((attacker.spdef + opponent.spdef) / 2).floor

		attacker.defense=opponent.defense=avdef
		attacker.spdef=opponent.spdef= avspdef

		//battle.pbDisplay(_INTL("{1} shared its guard with the target!", attacker.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// Averages the user's and target's current HP. (Pain Split)
	/// <summary>
	public class PokeBattle_Move_05A : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (opponent.effects.Substitute>0 && !ignoresSubstitute? (attacker)){
		   //battle.pbDisplay(_INTL("But it failed!"))  
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		olda=attacker.hp
		oldo = opponent.hp

		avhp=((attacker.hp+opponent.hp)/2).floor
		attacker.hp=[avhp, attacker.TotalHP].min
		opponent.hp=[avhp, opponent.TotalHP].min

		this.battle.scene.pbHPChanged(attacker, olda)
		this.battle.scene.pbHPChanged(opponent, oldo)
		//battle.pbDisplay(_INTL("The battlers shared their pain!"))
		return 0
	  }
	}



	/// <summary>
	/// For 4 rounds, doubles the Speed of all battlers on the user's side. (Tailwind)
	/// <summary>
	public class PokeBattle_Move_05B : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (attacker.OwnSide.Tailwind>0){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		attacker.OwnSide.Tailwind=4
		if (!this.battle.pbIsOpposing? (attacker.index)){
		   //battle.pbDisplay(_INTL("The tailwind blew from behind your team!"))
		else
		  //battle.pbDisplay(_INTL("The tailwind blew from behind the opposing team!"))
		}
		return 0
	  }
	}



	/// <summary>
	/// This move turns into the last move used by the target, until user switches
	/// out. (Mimic)
	/// <summary>
	public class PokeBattle_Move_05C : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){

		blacklist=[
		   0x02,   // Struggle
		   0x14,   // Chatter
		   0x5C,   // Mimic
		   0x5D,   // Sketch
		   0xB6    // Metronome
		]
		if (attacker.effects.Transform ||){
		   opponent.lastMoveUsed<=0 ||
		   isConst? (PBMoveData.new(opponent.lastMoveUsed).type,PBTypes,:SHADOW) ||
		   blacklist.include? (PBMoveData.new(opponent.lastMoveUsed).function)
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		foreach (var i in attacker.moves){ 
		  if (i.id==opponent.lastMoveUsed){
			//battle.pbDisplay(_INTL("But it failed!"))
			return -1 
		  }
		}

		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)
		for (int i = 0; i < attacker.moves.length; i++){ 
		  if (attacker.moves[i].id==this.id){
			newmove = PBMove.new(opponent.lastMoveUsed)
			  attacker.moves[i]= PokeBattle_Move.pbFromPBMove(this.battle, newmove)
  
			  movename= PBMoves.getName(opponent.lastMoveUsed)
  
			  //battle.pbDisplay(_INTL("{1} learned {2}!", attacker.pbThis, movename))
			return 0
		  }
		}

		//battle.pbDisplay(_INTL("But it failed!"))
		return -1
	  }
	}



	/// <summary>
	/// This move permanently turns into the last move used by the target. (Sketch)
	/// <summary>
	public class PokeBattle_Move_05D : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){

		blacklist=[
		   0x02,   // Struggle
		   0x14,   // Chatter
		   0x5D    // Sketch
		]
		if (attacker.effects.Transform ||){
		   opponent.lastMoveUsedSketch<=0 ||
		   isConst? (PBMoveData.new(opponent.lastMoveUsedSketch).type,PBTypes,:SHADOW) ||
		   blacklist.include? (PBMoveData.new(opponent.lastMoveUsedSketch).function)
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		foreach (var i in attacker.moves){ 
		  if (i.id==opponent.lastMoveUsedSketch){
			//battle.pbDisplay(_INTL("But it failed!"))
			return -1 
		  }
		}
		if (opponent.OwnSide.CraftyShield){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)
		for (int i = 0; i < attacker.moves.length; i++){ 
		  if (attacker.moves[i].id==this.id){
			newmove = PBMove.new(opponent.lastMoveUsedSketch)
			  attacker.moves[i]= PokeBattle_Move.pbFromPBMove(this.battle, newmove)
  
			  party= this.battle.pbParty(attacker.index)
  
			  party[attacker.pokemonIndex].moves[i]= newmove
  
			  movename= PBMoves.getName(opponent.lastMoveUsedSketch)
  
			  //battle.pbDisplay(_INTL("{1} learned {2}!", attacker.pbThis, movename))
			return 0
		  }
		}

		//battle.pbDisplay(_INTL("But it failed!"))
		return -1
	  }
	}



	/// <summary>
	/// Changes user's type to that of a random user's move, except this one, OR the
	/// user's first move's type. (Conversion)
	/// <summary>
	public class PokeBattle_Move_05E : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (isConst? (attacker.ability, PBAbilities,:MULTITYPE)){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		types =[]
		foreach (var i in attacker.moves){ 
		  if (i.id==this.id) continue; //next
		  if (PBTypes.isPseudoType? (i.type)) continue; //next
		   if (attacker.pbHasType? (i.type)) continue; //next
		  if (!types.include? (i.type)){
			 types.push(i.type)
			break if USENEWBATTLEMECHANICS
		  }

		}
		if (types.length==0){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		newtype=types[this.battle.pbRandom(types.length)]
		attacker.type1=newtype
		attacker.type2= newtype

		attacker.effects.Type3= -1

		typename= PBTypes.getName(newtype)

		//battle.pbDisplay(_INTL("{1} transformed into the {2} type!", attacker.pbThis, typename))
	  }
	}



	/// <summary>
	/// Changes user's type to a random one that resists/is immune to the last move
	/// used by the target. (Conversion 2)
	/// <summary>
	public class PokeBattle_Move_05F : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (isConst? (attacker.ability, PBAbilities,:MULTITYPE)){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if (opponent.lastMoveUsed<=0 ||){
		   PBTypes.isPseudoType? (PBMoveData.new(opponent.lastMoveUsed).type)
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if (opponent.OwnSide.CraftyShield){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		types =[]

		atype=opponent.lastMoveUsedType
		if (atype<0){

		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		for (int i = 0; i < BTypes.maxValue; i++){ 
		  if (PBTypes.isPseudoType? (i)) continue; //next
		   if (attacker.pbHasType? (i)) continue; //next
			types.push(i) if PBTypes.getEffectiveness(atype, i)<2 
		}
		if (types.length==0){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		newtype=types[this.battle.pbRandom(types.length)]
		attacker.type1=newtype
		attacker.type2= newtype

		attacker.effects.Type3= -1

		typename= PBTypes.getName(newtype)

		//battle.pbDisplay(_INTL("{1} transformed into the {2} type!", attacker.pbThis, typename))
		return 0
	  }
	}



	/// <summary>
	/// Changes user's type depending on the environment. (Camouflage)
	/// <summary>
	public class PokeBattle_Move_060 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (isConst? (attacker.ability, PBAbilities,:MULTITYPE)){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		type = Types.NORMAL
		case this.battle.environment
		when PBEnvironment::None;        type=Types.NORMAL
		when PBEnvironment::Grass; type=Types.GRASS
		when PBEnvironment::TallGrass; type=Types.GRASS
		when PBEnvironment::MovingWater; type=Types.WATER
		when PBEnvironment::StillWater; type=Types.WATER
		when PBEnvironment::Underwater; type=Types.WATER
		when PBEnvironment::Cave; type=Types.ROCK
		when PBEnvironment::Rock; type=Types.GROUND
		when PBEnvironment::Sand; type=Types.GROUND
		when PBEnvironment::Forest; type=Types.BUG
		when PBEnvironment::Snow; type=Types.ICE
		when PBEnvironment::Volcano; type=Types.FIRE
		when PBEnvironment::Graveyard; type=Types.GHOST
		when PBEnvironment::Sky; type=Types.FLYING
		when PBEnvironment::Space; type=Types.DRAGON
		}
		if (this.battle.field.ElectricTerrain>0){
		  type=Types.ELECTRIC if hasConst? (PBTypes,:ELECTRIC)
		else if this.battle.field.GrassyTerrain>0

		  type= Types.GRASS if hasConst?(PBTypes,:GRASS)

		else if this.battle.field.MistyTerrain>0

		  type= Types.FAIRY if hasConst?(PBTypes,:FAIRY)

		}
		if (attacker.pbHasType? (type)){
		   //battle.pbDisplay(_INTL("But it failed!"))
		  return -1  
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		attacker.type1=type
		attacker.type2=type
		attacker.effects.Type3= -1

		typename= PBTypes.getName(type)

		//battle.pbDisplay(_INTL("{1} transformed into the {2} type!", attacker.pbThis, typename))  
		return 0
	  }
	}



	/// <summary>
	/// Target becomes Water type. (Soak)
	/// <summary>
	public class PokeBattle_Move_061 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (opponent.effects.Substitute>0 && !ignoresSubstitute? (attacker)){
		   //battle.pbDisplay(_INTL("But it failed!"))  
		  return -1
		}
		if (pbTypeImmunityByAbility(pbType(this.type, attacker, opponent), attacker, opponent)) return -1;
		if (isConst? (opponent.ability, PBAbilities,:MULTITYPE)){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)
		if (opponent.type1==Types.WATER &&){
		   opponent.type2==Types.WATER &&
		   (opponent.effects.Type3<0 ||
		   opponent.effects.Type3==Types.WATER)
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		opponent.type1=Types.WATER

		opponent.type2=Types.WATER

		opponent.effects.Type3=-1
		typename=PBTypes.getName(Types.WATER)
		//battle.pbDisplay(_INTL("{1} transformed into the {2} type!",opponent.pbThis,typename))
		return 0
	  }
	}



	/// <summary>
	/// User copes target's types. (Reflect Type)
	/// <summary>
	public class PokeBattle_Move_062 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (isConst? (attacker.ability, PBAbilities,:MULTITYPE)){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if (attacker.pbHasType? (opponent.type1) &&){
		   attacker.pbHasType? (opponent.type2) &&
		   attacker.pbHasType? (opponent.effects.Type3) &&
		   opponent.pbHasType? (attacker.type1) &&
		   opponent.pbHasType? (attacker.type2) &&
		   opponent.pbHasType? (attacker.effects.Type3)
		   //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		attacker.type1=opponent.type1
		attacker.type2=opponent.type2
		attacker.effects.Type3= -1

		//battle.pbDisplay(_INTL("{1}'s type changed to match {2}'s!", attacker.pbThis, opponent.pbThis(true)))
		return 0
	  }
	}



	/// <summary>
	/// Target's ability becomes Simple. (Simple Beam)
	/// <summary>
	public class PokeBattle_Move_063 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (opponent.effects.Substitute>0 && !ignoresSubstitute? (attacker)){
		   //battle.pbDisplay(_INTL("But it failed!"))  
		  return -1
		}
		if (isConst? (opponent.ability, PBAbilities,:MULTITYPE) ||){
		   isConst? (opponent.ability, PBAbilities,:SIMPLE) ||
		   isConst? (opponent.ability, PBAbilities,:STANCECHANGE) ||
		   isConst? (opponent.ability, PBAbilities,:TRUANT)
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		oldabil=opponent.ability
		opponent.ability=Abilities.SIMPLE
		abilityname=PBAbilities.getName(Abilities.SIMPLE)
		//battle.pbDisplay(_INTL("{1} acquired {2}!",opponent.pbThis,abilityname))
		if (opponent.effects.Illusion && isConst? (oldabil, PBAbilities,:ILLUSION)){
		  //PBDebug.log("[Ability triggered] #{opponent.pbThis}'s Illusion ended")    
		  opponent.effects.Illusion=null
		  this.battle.scene.pbChangePokemon(opponent, opponent.pokemon)

		  //battle.pbDisplay(_INTL("{1}'s {2} wore off!",opponent.pbThis,PBAbilities.getName(oldabil)))
		}
		return 0
	  }
	}



	/// <summary>
	/// Target's ability becomes Insomnia. (Worry Seed)
	/// <summary>
	public class PokeBattle_Move_064 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (opponent.effects.Substitute>0 && !ignoresSubstitute? (attacker)){
		   //battle.pbDisplay(_INTL("But it failed!"))  
		  return -1
		}
		if (pbTypeImmunityByAbility(pbType(this.type, attacker, opponent), attacker, opponent)) return -1;
		if (isConst? (opponent.ability, PBAbilities,:MULTITYPE) ||){
		   isConst? (opponent.ability, PBAbilities,:INSOMNIA) ||
		   isConst? (opponent.ability, PBAbilities,:STANCECHANGE) ||
		   isConst? (opponent.ability, PBAbilities,:TRUANT)
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		oldabil=opponent.ability
		opponent.ability=Abilities.INSOMNIA
		abilityname=PBAbilities.getName(Abilities.INSOMNIA)
		//battle.pbDisplay(_INTL("{1} acquired {2}!",opponent.pbThis,abilityname))
		if (opponent.effects.Illusion && isConst? (oldabil, PBAbilities,:ILLUSION)){
		  //PBDebug.log("[Ability triggered] #{opponent.pbThis}'s Illusion ended")    
		  opponent.effects.Illusion=null
		  this.battle.scene.pbChangePokemon(opponent, opponent.pokemon)

		  //battle.pbDisplay(_INTL("{1}'s {2} wore off!",opponent.pbThis,PBAbilities.getName(oldabil)))
		}
		return 0
	  }
	}



	/// <summary>
	/// User copies target's ability. (Role Play)
	/// <summary>
	public class PokeBattle_Move_065 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (opponent.OwnSide.CraftyShield){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if (opponent.ability==0 ||){
		   attacker.ability==opponent.ability ||
		   isConst? (attacker.ability, PBAbilities,:MULTITYPE) ||
		   isConst? (attacker.ability, PBAbilities,:STANCECHANGE) ||
		   isConst? (opponent.ability, PBAbilities,:FLOWERGIFT) ||
		   isConst? (opponent.ability, PBAbilities,:FORECAST) ||
		   isConst? (opponent.ability, PBAbilities,:ILLUSION) ||
		   isConst? (opponent.ability, PBAbilities,:IMPOSTER) ||
		   isConst? (opponent.ability, PBAbilities,:MULTITYPE) ||
		   isConst? (opponent.ability, PBAbilities,:STANCECHANGE) ||
		   isConst? (opponent.ability, PBAbilities,:TRACE) ||
		   isConst? (opponent.ability, PBAbilities,:WONDERGUARD) ||
		   isConst? (opponent.ability, PBAbilities,:ZENMODE)
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		oldabil=attacker.ability
		attacker.ability=opponent.ability
		abilityname = PBAbilities.getName(opponent.ability)

		//battle.pbDisplay(_INTL("{1} copied {2}'s {3}!", attacker.pbThis, opponent.pbThis(true),abilityname))
		if (attacker.effects.Illusion && isConst? (oldabil, PBAbilities,:ILLUSION)){
		  //PBDebug.log("[Ability triggered] #{attacker.pbThis}'s Illusion ended")    
		  attacker.effects.Illusion=null
		  this.battle.scene.pbChangePokemon(attacker, attacker.pokemon)

		  //battle.pbDisplay(_INTL("{1}'s {2} wore off!",attacker.pbThis,PBAbilities.getName(oldabil)))
		}
		return 0
	  }
	}



	/// <summary>
	/// Target copies user's ability. (Entrainment)
	/// <summary>
	public class PokeBattle_Move_066 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (opponent.effects.Substitute>0 && !ignoresSubstitute? (attacker)){
		   //battle.pbDisplay(_INTL("But it failed!"))  
		  return -1
		}
		if (opponent.OwnSide.CraftyShield){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if (attacker.ability==0 ||){
		   attacker.ability==opponent.ability ||
		   isConst? (opponent.ability, PBAbilities,:FLOWERGIFT) ||
		   isConst? (opponent.ability, PBAbilities,:IMPOSTER) ||
		   isConst? (opponent.ability, PBAbilities,:MULTITYPE) ||
		   isConst? (opponent.ability, PBAbilities,:STANCECHANGE) ||
		   isConst? (opponent.ability, PBAbilities,:TRACE) ||
		   isConst? (opponent.ability, PBAbilities,:TRUANT) ||
		   isConst? (opponent.ability, PBAbilities,:ZENMODE) ||
		   isConst? (attacker.ability, PBAbilities,:FLOWERGIFT) ||
		   isConst? (attacker.ability, PBAbilities,:FORECAST) ||
		   isConst? (attacker.ability, PBAbilities,:ILLUSION) ||
		   isConst? (attacker.ability, PBAbilities,:IMPOSTER) ||
		   isConst? (attacker.ability, PBAbilities,:MULTITYPE) ||
		   isConst? (attacker.ability, PBAbilities,:STANCECHANGE) ||
		   isConst? (attacker.ability, PBAbilities,:TRACE) ||
		   isConst? (attacker.ability, PBAbilities,:ZENMODE)
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		oldabil=opponent.ability
		opponent.ability=attacker.ability
		abilityname = PBAbilities.getName(attacker.ability)

		//battle.pbDisplay(_INTL("{1} acquired {2}!", opponent.pbThis, abilityname))
		if (opponent.effects.Illusion && isConst? (oldabil, PBAbilities,:ILLUSION)){
		  //PBDebug.log("[Ability triggered] #{opponent.pbThis}'s Illusion ended")    
		  opponent.effects.Illusion=null
		  this.battle.scene.pbChangePokemon(opponent, opponent.pokemon)

		  //battle.pbDisplay(_INTL("{1}'s {2} wore off!",opponent.pbThis,PBAbilities.getName(oldabil)))
		}
		return 0
	  }
	}



	/// <summary>
	/// User and target swap abilities. (Skill Swap)
	/// <summary>
	public class PokeBattle_Move_067 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (attacker.ability==0 && opponent.ability==0) ||
		   (attacker.ability==opponent.ability && !USENEWBATTLEMECHANICS) ||
		   isConst? (attacker.ability, PBAbilities,:ILLUSION) ||
		   isConst? (opponent.ability, PBAbilities,:ILLUSION) ||
		   isConst? (attacker.ability, PBAbilities,:MULTITYPE) ||
		   isConst? (opponent.ability, PBAbilities,:MULTITYPE) ||
		   isConst? (attacker.ability, PBAbilities,:STANCECHANGE) ||
		   isConst? (opponent.ability, PBAbilities,:STANCECHANGE) ||
		   isConst? (attacker.ability, PBAbilities,:WONDERGUARD) ||
		   isConst? (opponent.ability, PBAbilities,:WONDERGUARD)
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		tmp=attacker.ability
		attacker.ability=opponent.ability
		opponent.ability= tmp

		//battle.pbDisplay(_INTL("{1} swapped its {2} Ability with its target's {3} Ability!",
		   attacker.pbThis, PBAbilities.getName(opponent.ability),
		   PBAbilities.getName(attacker.ability)))
		attacker.pbAbilitiesOnSwitchIn(true)
		opponent.pbAbilitiesOnSwitchIn(true)
		return 0
	  }
	}



	/// <summary>
	/// Target's ability is negated. (Gastro Acid)
	/// <summary>
	public class PokeBattle_Move_068 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (opponent.effects.Substitute>0 && !ignoresSubstitute? (attacker)){
		   //battle.pbDisplay(_INTL("But it failed!"))  
		  return -1
		}
		if (isConst? (opponent.ability, PBAbilities,:MULTITYPE) ||){
		   isConst? (opponent.ability, PBAbilities,:STANCECHANGE)
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		oldabil=opponent.ability
		opponent.effects.GastroAcid= true

		opponent.effects.Truant= false

		//battle.pbDisplay(_INTL("{1}'s Ability was suppressed!", opponent.pbThis)) 
		if (opponent.effects.Illusion && isConst? (oldabil, PBAbilities,:ILLUSION)){
		  //PBDebug.log("[Ability triggered] #{opponent.pbThis}'s Illusion ended")    
		  opponent.effects.Illusion=null
		  this.battle.scene.pbChangePokemon(opponent, opponent.pokemon)

		  //battle.pbDisplay(_INTL("{1}'s {2} wore off!",opponent.pbThis,PBAbilities.getName(oldabil)))
		}
		return 0
	  }
	}



	/// <summary>
	/// User transforms into the target. (Transform)
	/// <summary>
	public class PokeBattle_Move_069 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){

		blacklist=[
		   0xC9,   // Fly
		   0xCA,   // Dig
		   0xCB,   // Dive
		   0xCC,   // Bounce
		   0xCD,   // Shadow Force
		   0xCE,   // Sky Drop
		   0x14D   // Phantom Force
		]
		if (attacker.effects.Transform ||){
		   opponent.effects.Transform ||
		   opponent.effects.Illusion ||
		   (opponent.effects.Substitute>0 && !ignoresSubstitute? (attacker)) ||
		   opponent.effects.SkyDrop ||
		   blacklist.include? (PBMoveData.new(opponent.effects.TwoTurnAttack).function)
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if (opponent.OwnSide.CraftyShield){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		attacker.effects.Transform=true
		attacker.type1=opponent.type1
		attacker.type2=opponent.type2
		attacker.effects.Type3= -1

		attacker.ability= opponent.ability

		attacker.attack= opponent.attack

		attacker.defense= opponent.defense

		attacker.speed= opponent.speed

		attacker.spatk= opponent.spatk

		attacker.spdef= opponent.spdef
		foreach (var i in [PBStats::ATTACK, PBStats::DEFENSE, PBStats::SPEED,
				  PBStats::SPATK, PBStats::SPDEF, PBStats::ACCURACY, PBStats::EVASION]){ 
		  attacker.stages[i]= opponent.stages[i]

		}
		for (int i = 0; i < 4; i++){ 

		  attacker.moves[i]= PokeBattle_Move.pbFromPBMove(
			 this.battle, PBMove.new (opponent.moves[i].id))

		  attacker.moves[i].pp=5
		  attacker.moves[i].totalpp=5
		}
		attacker.effects.Disable= 0

		attacker.effects.DisableMove= 0

		//battle.pbDisplay(_INTL("{1} transformed into {2}!", attacker.pbThis, opponent.pbThis(true)))
		return 0
	  }
	}



	/// <summary>
	/// Inflicts a fixed 20HP damage. (SonicBoom)
	/// <summary>
	public class PokeBattle_Move_06A : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		return pbEffectFixedDamage(20, attacker, opponent, hitnum, alltargets, showanimation)
	  }
	}



	/// <summary>
	/// Inflicts a fixed 40HP damage. (Dragon Rage)
	/// <summary>
	public class PokeBattle_Move_06B : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		return pbEffectFixedDamage(40, attacker, opponent, hitnum, alltargets, showanimation)
	  }
	}



	/// <summary>
	/// Halves the target's current HP. (Super Fang)
	/// <summary>
	public class PokeBattle_Move_06C : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		return pbEffectFixedDamage([(opponent.hp / 2).floor,1].max, attacker, opponent, hitnum, alltargets, showanimation)
	  }
	}



	/// <summary>
	/// Inflicts damage equal to the user's level. (Night Shade, Seismic Toss)
	/// <summary>
	public class PokeBattle_Move_06D : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		return pbEffectFixedDamage(attacker.level, attacker, opponent, hitnum, alltargets, showanimation)
	  }
	}



	/// <summary>
	/// Inflicts damage to bring the target's HP down to equal the user's HP. (Endeavor)
	/// <summary>
	public class PokeBattle_Move_06E : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (attacker.hp>=opponent.hp){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		return pbEffectFixedDamage(opponent.hp-attacker.hp, attacker, opponent, hitnum, alltargets, showanimation)
	  }
	}



	/// <summary>
	/// Inflicts damage between 0.5 and 1.5 times the user's level. (Psywave)
	/// <summary>
	public class PokeBattle_Move_06F : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){

		dmg=[(attacker.level * (this.battle.pbRandom(101) + 50) / 100).floor,1].max
		return pbEffectFixedDamage(dmg, attacker, opponent, hitnum, alltargets, showanimation)
	  }
	}



	/// <summary>
	/// OHKO. Accuracy increases by difference between levels of user and target.
	/// <summary>
	public class PokeBattle_Move_070 : PokeBattle_Move
	{
		public object pbAccuracyCheck(Battle.Battler attacker, Battle.Battler opponent){
		if (!attacker.hasMoldBreaker && opponent.hasWorkingAbility(Abilities.STURDY)){
		  //battle.pbDisplay(_INTL("{1} was protected by {2}!",opponent.pbThis,PBAbilities.getName(opponent.ability)))  
		  return false
		}
		if (opponent.level>attacker.level){
		  //battle.pbDisplay(_INTL("{1} is unaffected!", opponent.pbThis))
		  return false
		}
		acc = this.accuracy + attacker.level - opponent.level
		return this.battle.pbRandom(100)<acc
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){

		damage=pbEffectFixedDamage(opponent.TotalHP, attacker, opponent, hitnum, alltargets, showanimation)
		if (opponent.isFainted()){
		  //battle.pbDisplay(_INTL("It's a one-hit KO!"))
		}
		return damage
	  }
	}


	/// <summary>
	/// Counters a physical move used against the user this round, with 2x the power. (Counter)
	/// <summary>
	public class PokeBattle_Move_071 : PokeBattle_Move
	{
		public object pbAddTarget(targets, Battle.Battler attacker){
		if (attacker.effects.CounterTarget>=0 &&){
		   attacker.pbIsOpposing? (attacker.effects.CounterTarget)
		  if (!attacker.pbAddTarget(targets, this.battle.battlers[attacker.effects.CounterTarget])){
			attacker.pbRandomTarget(targets)

		  }
		}
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (attacker.effects.Counter<0 || !opponent){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		ret = pbEffectFixedDamage([attacker.effects.Counter * 2, 1].max, attacker, opponent, hitnum, alltargets, showanimation)
		return ret
	  }
	}



	/// <summary>
	/// Counters a specical move used against the user this round, with 2x the power. (Mirror Coat)
	/// <summary>
	public class PokeBattle_Move_072 : PokeBattle_Move
	{
		public object pbAddTarget(targets, Battle.Battler attacker){
		if (attacker.effects.MirrorCoatTarget>=0 && ){
		   attacker.pbIsOpposing? (attacker.effects.MirrorCoatTarget)
		  if (!attacker.pbAddTarget(targets, this.battle.battlers[attacker.effects.MirrorCoatTarget])){
			attacker.pbRandomTarget(targets)

		  }
		}
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (attacker.effects.MirrorCoat<0 || !opponent){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		ret = pbEffectFixedDamage([attacker.effects.MirrorCoat * 2, 1].max, attacker, opponent, hitnum, alltargets, showanimation)
		return ret
	  }
	}



	/// <summary>
	/// Counters the last damaging move used against the user this round, with 1.5x
	/// the power. (Metal Burst)
	/// <summary>
	public class PokeBattle_Move_073 : PokeBattle_Move
	{
		public object pbAddTarget(targets, Battle.Battler attacker){
		if (attacker.lastAttacker.length>0){
		  lastattacker=attacker.lastAttacker[attacker.lastAttacker.length - 1]
		  if (lastattacker>=0 && attacker.pbIsOpposing? (lastattacker)){
			if (!attacker.pbAddTarget(targets, this.battle.battlers[lastattacker])){
			  attacker.pbRandomTarget(targets)

			}
		  }

		}
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (attacker.lastHPLost==0 || !opponent){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		ret = pbEffectFixedDamage([(attacker.lastHPLost * 1.5f).floor, 1].max, attacker, opponent, hitnum, alltargets, showanimation)
		return ret
	  }
	}



	/// <summary>
	/// The target's ally loses 1/16 of its max HP. (Flame Burst)
	/// <summary>
	public class PokeBattle_Move_074 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){

		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if (opponent.damagestate.CalcDamage>0){
		  if (opponent.pbPartner && !opponent.pbPartner.isFainted() &&){
			 !opponent.pbPartner.hasWorkingAbility(Abilities.MAGICGUARD)
			opponent.pbPartner.pbReduceHP((opponent.pbPartner.TotalHP/16).floor)
			//battle.pbDisplay(_INTL("The bursting flame hit {1}!",opponent.pbPartner.pbThis(true)))
		  }
		}
		return ret
	  }
	}



	/// <summary>
	/// Power is doubled if the target is using Dive. (Surf)
	/// (Handled in Battler's pbSuccessCheck): Hits some semi-invulnerable targets.
	/// <summary>
	public class PokeBattle_Move_075 : PokeBattle_Move
	{
		public object pbModifyDamage(damagemult, Battle.Battler attacker, Battle.Battler opponent){
		if (PBMoveData.new(opponent.effects.TwoTurnAttack).function==0xCB // Dive){
		  return (int)Math.Round(damagemult*2.0f)
		}
		return damagemult
	  }
	}



	/// <summary>
	/// Power is doubled if the target is using Dig. Power is halved if Grassy Terrain
	/// is in effect. (Earthquake)
	/// (Handled in Battler's pbSuccessCheck): Hits some semi-invulnerable targets.
	/// <summary>
	public class PokeBattle_Move_076 : PokeBattle_Move
	{
		public object pbModifyDamage(damagemult, Battle.Battler attacker, Battle.Battler opponent){

		ret=damagemult
		if (PBMoveData.new(opponent.effects.TwoTurnAttack).function==0xCA // Dig){
		  ret= (int)Math.Round(damagemult*2.0f)
		}
		if (this.battle.field.GrassyTerrain>0){
		  ret=(int)Math.Round(damagemult/2.0f)
		}
		return ret
	  }
	}



	/// <summary>
	/// Power is doubled if the target is using Bounce, Fly or Sky Drop. (Gust)
	/// (Handled in Battler's pbSuccessCheck): Hits some semi-invulnerable targets.
	/// <summary>
	public class PokeBattle_Move_077 : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){
		if (PBMoveData.new(opponent.effects.TwoTurnAttack).function==0xC9 || // Fly){
		   PBMoveData.new(opponent.effects.TwoTurnAttack).function==0xCC || // Bounce
		   PBMoveData.new(opponent.effects.TwoTurnAttack).function==0xCE || // Sky Drop
		   opponent.effects.SkyDrop
		  return basedmg*2
		}
		return basedmg
	  }
	}



	/// <summary>
	/// Power is doubled if the target is using Bounce, Fly or Sky Drop. (Twister)
	/// May make the target flinch.
	/// (Handled in Battler's pbSuccessCheck): Hits some semi-invulnerable targets.
	/// <summary>
	public class PokeBattle_Move_078 : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){
		if (PBMoveData.new(opponent.effects.TwoTurnAttack).function==0xC9 || // Fly){
		   PBMoveData.new(opponent.effects.TwoTurnAttack).function==0xCC || // Bounce
		   PBMoveData.new(opponent.effects.TwoTurnAttack).function==0xCE || // Sky Drop
		   opponent.effects.SkyDrop
		  return basedmg*2
		}
		return basedmg
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		opponent.pbFlinch(attacker)
	  }
	}



	/// <summary>
	/// Power is doubled if Fusion Flare has already been used this round. (Fusion Bolt)
	/// <summary>
	public class PokeBattle_Move_079 : PokeBattle_Move
	{
		public object pbBaseDamageMultiplier(damagemult, Battle.Battler attacker, Battle.Battler opponent){
		if (this.battle.field.FusionBolt){
		  this.battle.field.FusionBolt= false

		  this.doubled= true
		  return (int)Math.Round(damagemult*2.0f)
		}
		return damagemult
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)

		this.doubled=false
		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if (opponent.damagestate.CalcDamage>0){
		  this.battle.field.FusionFlare=true
		}
		return ret
	  }

	  public object pbShowAnimation(id, Battle.Battler attacker, Battle.Battler opponent, byte hitnum= 0, byte? alltargets= null, bool showanimation= true){
		if (opponent.damagestate.Critical || this.doubled){
		  return super(id, attacker, opponent,1, alltargets, showanimation) // Charged anim
		}
		return super(id, attacker, opponent, hitnum, alltargets, showanimation)
	  }
	}



	/// <summary>
	/// Power is doubled if Fusion Bolt has already been used this round. (Fusion Flare)
	/// <summary>
	public class PokeBattle_Move_07A : PokeBattle_Move
	{
		public object pbBaseDamageMultiplier(damagemult, Battle.Battler attacker, Battle.Battler opponent){
		if (battle.field.FusionFlare){
		  this.battle.field.FusionFlare= false
		  return (int)Math.Round(damagemult*2.0f)
		}
		return damagemult
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){

		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if (opponent.damagestate.CalcDamage>0){
		  this.battle.field.FusionBolt=true
		}
		return ret
	  }

	  public object pbShowAnimation(id, Battle.Battler attacker, Battle.Battler opponent, byte hitnum= 0, byte? alltargets= null, bool showanimation= true){
		if (opponent.damagestate.Critical || this.doubled){
		  return super(id, attacker, opponent,1, alltargets, showanimation) // Charged anim
		}
		return super(id, attacker, opponent, hitnum, alltargets, showanimation)
	  }
	}



	/// <summary>
	/// Power is doubled if the target is poisoned. (Venoshock)
	/// <summary>
	public class PokeBattle_Move_07B : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.status==PBStatuses::POISON &&){
		   (opponent.effects.Substitute==0 || ignoresSubstitute? (attacker))
		  return basedmg*2
		}
		return basedmg
	  }
	}



	/// <summary>
	/// Power is doubled if the target is paralyzed. Cures the target of paralysis.
	/// (SmellingSalt)
	/// <summary>
	public class PokeBattle_Move_07C : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.status==PBStatuses::PARALYSIS &&){
		   (opponent.effects.Substitute==0 || ignoresSubstitute? (attacker))
		  return basedmg*2
		}
		return basedmg
	  }

	  public object pbEffectAfterHit(Battle.Battler attacker, Battle.Battler opponent, turneffects){
		if (!opponent.isFainted() && opponent.damagestate.CalcDamage>0 &&){
		   !opponent.damagestate.Substitute && opponent.status==PBStatuses::PARALYSIS
		  opponent.pbCureStatus

		}
	  }
	}



	/// <summary>
	/// Power is doubled if the target is asleep. Wakes the target up. (Wake-Up Slap)
	/// <summary>
	public class PokeBattle_Move_07D : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.status==PBStatuses::SLEEP &&){
		   (opponent.effects.Substitute==0 || ignoresSubstitute? (attacker))
		  return basedmg*2
		}
		return basedmg
	  }

	  public object pbEffectAfterHit(Battle.Battler attacker, Battle.Battler opponent, turneffects){
		if (!opponent.isFainted() && opponent.damagestate.CalcDamage>0 &&){
		   !opponent.damagestate.Substitute && opponent.status==PBStatuses::SLEEP
		  opponent.pbCureStatus

		}
	  }
	}



	/// <summary>
	/// Power is doubled if the user is burned, poisoned or paralyzed. (Facade)
	/// <summary>
	public class PokeBattle_Move_07E : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){
		if (attacker.status==PBStatuses::POISON ||){
		   attacker.status==PBStatuses::BURN ||
		   attacker.status==PBStatuses::PARALYSIS
		  return basedmg*2
		}
		return basedmg
	  }
	}



	/// <summary>
	/// Power is doubled if the target has a status problem. (Hex)
	/// <summary>
	public class PokeBattle_Move_07F : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.status>0 &&){
		   (opponent.effects.Substitute==0 || ignoresSubstitute? (attacker))
		  return basedmg*2
		}
		return basedmg
	  }
	}



	/// <summary>
	/// Power is doubled if the target's HP is down to 1/2 or less. (Brine)
	/// <summary>
	public class PokeBattle_Move_080 : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.hp<=opponent.TotalHP/2){
		  return basedmg*2
		}
		return basedmg
	  }
	}



	/// <summary>
	/// Power is doubled if the user has lost HP due to the target's move this round.
	/// (Revenge, Avalanche)
	/// <summary>
	public class PokeBattle_Move_081 : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){
		if (attacker.lastHPLost>0 && attacker.lastAttacker.include? (opponent.index)){
		  return basedmg*2
		}
		return basedmg
	  }
	}



	/// <summary>
	/// Power is doubled if the target has already lost HP this round. (Assurance)
	/// <summary>
	public class PokeBattle_Move_082 : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.tookDamage){
		  return basedmg*2
		}
		return basedmg
	  }
	}



	/// <summary>
	/// Power is doubled if a user's ally has already used this move this round. (Round)
	/// If an ally is about to use the same move, make it go next, ignoring priority.
	/// <summary>
	public class PokeBattle_Move_083 : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){

		ret=basedmg
		attacker.OwnSide.Round.times do

		  ret*=2

		}
		return ret
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){

		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if (opponent.damagestate.CalcDamage>0){
		  attacker.OwnSide.Round+=1
		  if (attacker.pbPartner && !attacker.pbPartner.hasMovedThisRound?){
			if (this.battle.choices[attacker.pbPartner.index][0]==1 // Will use a move){
			  partnermove=this.battle.choices[attacker.pbPartner.index][2]
			  if (partnermove.function==this.function){
				attacker.pbPartner.effects.MoveNext= true

				attacker.pbPartner.effects.Quash= false

			  }
			}

		  }
		}
		return ret
	  }
	}



	/// <summary>
	/// Power is doubled if the target has already moved this round. (Payback)
	/// <summary>
	public class PokeBattle_Move_084 : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){
		if (this.battle.choices[opponent.index][0]!=1 || // Didn){'t choose a move
		   opponent.hasMovedThisRound? // Used a move already
		  return basedmg*2
		}
		return basedmg
	  }
	}



	/// <summary>
	/// Power is doubled if a user's teammate fainted last round. (Retaliate)
	/// <summary>
	public class PokeBattle_Move_085 : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){
		if (attacker.OwnSide.LastRoundFainted>=0 &&
		   attacker.OwnSide.LastRoundFainted==this.battle.turncount-1){
		  return basedmg*2
		}
		return basedmg
	  }
	}



	/// <summary>
	/// Power is doubled if the user has no held item. (Acrobatics)
	/// <summary>
	public class PokeBattle_Move_086 : PokeBattle_Move
	{
		public object pbBaseDamageMultiplier(damagemult, Battle.Battler attacker, Battle.Battler opponent){
		if (attacker.item==0){
		  return (int)Math.Round(damagemult*2.0f)
		}
		return damagemult
	  }
	}



	/// <summary>
	/// Power is doubled in weather. Type changes depending on the weather. (Weather Ball)
	/// <summary>
	public class PokeBattle_Move_087 : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){
		if (this.battle.pbWeather!=0){
			return basedmg * 2;
		}
		return basedmg;
	  }

	  public object pbModifyType(Types type, Battle.Battler attacker, Battle.Battler opponent){

		type = Types.NORMAL;
		case this.battle.pbWeather
		when PBWeather::SUNNYDAY, PBWeather::HARSHSUN
		  type = (Types.FIRE)

		when PBWeather::RAINDANCE, PBWeather::HEAVYRAIN
		  type = (Types.WATER)

		when PBWeather::SANDSTORM
		  type = (Types.ROCK)

		when PBWeather::HAIL
		  type = (Types.ICE)

		}
		return type
	  }
	}



	/// <summary>
	/// Power is doubled if a foe tries to switch out or use U-turn/Volt Switch/
	/// Parting Shot. (Pursuit)
	/// (Handled in Battle's pbAttackPhase): Makes this attack happen before switching.
	/// <summary>
	public class PokeBattle_Move_088 : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){
		if (this.battle.switching){
		  return basedmg*2
		}
		return basedmg
	  }

	  public object pbAccuracyCheck(Battle.Battler attacker, Battle.Battler opponent){
		if (this.battle.switching) return true;
		return super(attacker, opponent)
	  }
	}



	/// <summary>
	/// Power increases with the user's happiness. (Return)
	/// <summary>
	public class PokeBattle_Move_089 : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){
			return [(attacker.happiness * 2 / 5).floor,1].max
		}
	}



	/// <summary>
	/// Power decreases with the user's happiness. (Frustration)
	/// <summary>
	public class PokeBattle_Move_08A : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){
			return [((255 - attacker.happiness) * 2 / 5).floor,1].max
		}
	}



	/// <summary>
	/// Power increases with the user's HP. (Eruption, Water Spout)
	/// <summary>
	public class PokeBattle_Move_08B : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){
			return [(150 * attacker.hp / attacker.TotalHP).floor,1].max
		}
	}



	/// <summary>
	/// Power increases with the target's HP. (Crush Grip, Wring Out)
	/// <summary>
	public class PokeBattle_Move_08C : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){
			return [(120 * opponent.hp / opponent.TotalHP).floor,1].max
		}
	}



	/// <summary>
	/// Power increases the quicker the target is than the user. (Gyro Ball)
	/// <summary>
	public class PokeBattle_Move_08D : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){
			return [[(25 * opponent.pbSpeed / attacker.pbSpeed).floor,150].min,1].max
		}
	}



	/// <summary>
	/// Power increases with the user's positive stat changes (ignores negative ones).
	/// (Stored Power)
	/// <summary>
	public class PokeBattle_Move_08E : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){

		mult=1
		foreach (var i in [PBStats::ATTACK, PBStats::DEFENSE, PBStats::SPEED,
				  PBStats::SPATK, PBStats::SPDEF, PBStats::ACCURACY, PBStats::EVASION]){ 
		mult+=attacker.stages[i] if attacker.stages[i]>0
		}
		return 20*mult
	  }
	}



	/// <summary>
	/// Power increases with the target's positive stat changes (ignores negative ones).
	/// (Punishment)
	/// <summary>
	public class PokeBattle_Move_08F : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){
			mult=3
			foreach (var i in [PBStats::ATTACK, PBStats::DEFENSE, PBStats::SPEED,
					  PBStats::SPATK, PBStats::SPDEF, PBStats::ACCURACY, PBStats::EVASION]){ 
				mult+=opponent.stages[i] if opponent.stages[i]>0
			}
			return [20*mult,200].min
		}
	}



	/// <summary>
	/// Power and type depends on the user's IVs. (Hidden Power)
	/// <summary>
	public class PokeBattle_Move_090 : PokeBattle_Move
	{
		public object pbModifyType(type, Battle.Battler attacker, Battle.Battler opponent){
			hp=pbHiddenPower(attacker.iv)

			type=hp[0]
			return type
		  }

	  public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){
			if (USENEWBATTLEMECHANICS) return 60;
			hp = pbHiddenPower(attacker.iv)
			return hp[1]
		  }
		}

	public object pbHiddenPower(iv){
	  powermin=30
	  powermax=70
	  type=0; base=0
	  types=[]
	  for (int i = 0; i < BTypes.maxValue; i++){ 
		types.push(i) if !PBTypes.isPseudoType? (i) &&
						 !isConst? (i, PBTypes,:NORMAL) && !isConst? (i, PBTypes,:SHADOW)
	  }
	  type|=(iv[PBStats::HP]&1)
	  type|=(iv[PBStats::ATTACK]&1)<<1
	  type|=(iv[PBStats::DEFENSE]&1)<<2
	  type|=(iv[PBStats::SPEED]&1)<<3
	  type|=(iv[PBStats::SPATK]&1)<<4
	  type|=(iv[PBStats::SPDEF]&1)<<5
	  type=(type*(types.length-1)/63).floor
	  hptype = types[type]
	  base|=(iv[PBStats::HP]&2)>>1
	  base|=(iv[PBStats::ATTACK]&2)
	  base|=(iv[PBStats::DEFENSE]&2)<<1
	  base|=(iv[PBStats::SPEED]&2)<<2
	  base|=(iv[PBStats::SPATK]&2)<<3
	  base|=(iv[PBStats::SPDEF]&2)<<4
	  base=(base*(powermax-powermin)/63).floor+powermin
	  return [hptype,base]
		}

	/// <summary>
	/// Power doubles for each consecutive use. (Fury Cutter)
	/// <summary>
	public class PokeBattle_Move_091 : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){

		basedmg=basedmg<<(attacker.effects.FuryCutter-1) // can be 1 to 4
		return basedmg
	  }
	}



	/// <summary>
	/// Power is multiplied by the number of consecutive rounds in which this move was
	/// used by any Pokémon on the user's side. (Echoed Voice)
	/// <summary>
	public class PokeBattle_Move_092 : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){

		basedmg*=attacker.OwnSide.EchoedVoiceCounter // can be 1 to 5
		return basedmg
	  }
	}



	/// <summary>
	/// User rages until the start of a round in which they don't use this move. (Rage)
	/// (Handled in Battler's pbProcessMoveAgainstTarget): Ups rager's Attack by 1
	/// stage each time it loses HP due to a move.
	/// <summary>
	public class PokeBattle_Move_093 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){

		ret=super(attacker, opponent, hitnum, alltargets, showanimation)

		attacker.effects.Rage=true if ret>0
		return ret
	  }
	}



	/// <summary>
	/// Randomly damages or heals the target. (Present)
	/// <summary>
	public class PokeBattle_Move_094 : PokeBattle_Move
	{
		public object pbOnStartUse(Battle.Battler attacker) { 
		// Just to ensure that Parental Bond's second hit damages if the first hit does
		this.forcedamage=false
		return true
	  }

	  public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent)
	  {
		this.forcedamage=true
		return this.calcbasedmg
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
	  {
		this.calcbasedmg = 1;
		r = this.battle.pbRandom((this.forcedamage) ? 8 : 10);
		if (r < 4)
			this.calcbasedmg = 40;
		else if (r < 7)
			this.calcbasedmg = 80;
		else if (r < 8)
			this.calcbasedmg = 120;
		else { 
		  if (pbTypeModifier(pbType(this.type, attacker, opponent), attacker, opponent) == 0) {
				//battle.pbDisplay(_INTL("It doesn't affect {1}...",opponent.pbThis(true)))
				return -1;
		  }
		  if (opponent.hp==opponent.TotalHP){
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
		  }
		  damage = pbCalcDamage(attacker, opponent) // Consumes Gems even if it will heal
		  pbShowAnimation(this.id, attacker, opponent,1, alltargets, showanimation) // Healing animation
		  opponent.pbRecoverHP((opponent.TotalHP/4).floor,true)
		  //battle.pbDisplay(_INTL("{1} had its HP restored.",opponent.pbThis))   
			return 0;
		}
		return super(attacker, opponent, hitnum, alltargets, showanimation)
	  }
	}



	/// <summary>
	/// Power is chosen at random. Power is doubled if the target is using Dig. (Magnitude)
	/// (Handled in Battler's pbSuccessCheck): Hits some semi-invulnerable targets.
	/// <summary>
	public class PokeBattle_Move_095 : PokeBattle_Move
	{
		public object pbOnStartUse(Battle.Battler attacker){

		basedmg=[10,30,50,70,90,110,150]
		magnitudes=[
		   4,
		   5,5,
		   6,6,6,6,
		   7,7,7,7,7,7,
		   8,8,8,8,
		   9,9,
		   10
		]
		magni=magnitudes[this.battle.pbRandom(magnitudes.length)]
		this.calcbasedmg = basedmg[magni - 4]

		//battle.pbDisplay(_INTL("Magnitude {1}!",magni))
		return true
	  }

	  public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){

		ret=this.calcbasedmg
		if (PBMoveData.new(opponent.effects.TwoTurnAttack).function==0xCA // Dig){
		  ret*=2
		}
		if (this.battle.field.GrassyTerrain>0){
		  ret=(int)Math.Round(ret/2.0f)
		}
		return ret
	  }
	}



	/// <summary>
	/// Power and type depend on the user's held berry. Destroys the berry. (Natural Gift)
	/// <summary>
	public class PokeBattle_Move_096 : PokeBattle_Move
	{
		public object pbOnStartUse(Battle.Battler attacker){
		if (!pbIsBerry? (attacker.item) ||){
		   attacker.effects.Embargo>0 ||
		   this.battle.field.MagicRoom>0 ||
		   attacker.hasWorkingAbility(Abilities.KLUTZ) ||
		   attacker.pbOpposing1.hasWorkingAbility(Abilities.UNNERVE) ||
		   attacker.pbOpposing2.hasWorkingAbility(Abilities.UNNERVE)
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return false
		}
		this.berry = attacker.item
		return true
	  }

	  public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){

		damagearray={
		   60 => [:CHERIBERRY,:CHESTOBERRY,:PECHABERRY,:RAWSTBERRY,:ASPEARBERRY,
				  :LEPPABERRY,:ORANBERRY,:PERSIMBERRY,:LUMBERRY,:SITRUSBERRY,
				  :FIGYBERRY,:WIKIBERRY,:MAGOBERRY,:AGUAVBERRY,:IAPAPABERRY,
				  :RAZZBERRY,:OCCABERRY,:PASSHOBERRY,:WACANBERRY,:RINDOBERRY,
				  :YACHEBERRY,:CHOPLEBERRY,:KEBIABERRY,:SHUCABERRY,:COBABERRY,
				  :PAYAPABERRY,:TANGABERRY,:CHARTIBERRY,:KASIBBERRY,:HABANBERRY,
				  :COLBURBERRY,:BABIRIBERRY,:CHILANBERRY,:ROSELIBERRY],
		   70 => [:BLUKBERRY,:NANABBERRY,:WEPEARBERRY,:PINAPBERRY,:POMEGBERRY,
				  :KELPSYBERRY,:QUALOTBERRY,:HONDEWBERRY,:GREPABERRY,:TAMATOBERRY,
				  :CORNNBERRY,:MAGOSTBERRY,:RABUTABERRY,:NOMELBERRY,:SPELONBERRY,
				  :PAMTREBERRY],
		   80 => [:WATMELBERRY,:DURINBERRY,:BELUEBERRY,:LIECHIBERRY,:GANLONBERRY,
				  :SALACBERRY,:PETAYABERRY,:APICOTBERRY,:LANSATBERRY,:STARFBERRY,
				  :ENIGMABERRY,:MICLEBERRY,:CUSTAPBERRY,:JABOCABERRY,:ROWAPBERRY,
				  :KEEBERRY,:MARANGABERRY]
		}
		foreach (var i in damagearray.keys){ 
		  data = damagearray[i]
		  if (data){
			foreach (var i in data){ 
			  if (isConst? (this.berry, PBItems, j)){
				 ret = i

				ret+=20 if USENEWBATTLEMECHANICS
				return ret
			  }

			}
		  }

		}
		return 1
	  }

	  public object pbModifyType(type, Battle.Battler attacker, Battle.Battler opponent){

		type=Types.NORMAL
		typearray={
		   :NORMAL   => [:CHILANBERRY],
		   :FIRE     => [:CHERIBERRY,:BLUKBERRY,:WATMELBERRY,:OCCABERRY],
		   :WATER    => [:CHESTOBERRY,:NANABBERRY,:DURINBERRY,:PASSHOBERRY],
		   :ELECTRIC => [:PECHABERRY,:WEPEARBERRY,:BELUEBERRY,:WACANBERRY],
		   :GRASS    => [:RAWSTBERRY,:PINAPBERRY,:RINDOBERRY,:LIECHIBERRY],
		   :ICE      => [:ASPEARBERRY,:POMEGBERRY,:YACHEBERRY,:GANLONBERRY],
		   :FIGHTING => [:LEPPABERRY,:KELPSYBERRY,:CHOPLEBERRY,:SALACBERRY],
		   :POISON   => [:ORANBERRY,:QUALOTBERRY,:KEBIABERRY,:PETAYABERRY],
		   :GROUND   => [:PERSIMBERRY,:HONDEWBERRY,:SHUCABERRY,:APICOTBERRY],
		   :FLYING   => [:LUMBERRY,:GREPABERRY,:COBABERRY,:LANSATBERRY],
		   :PSYCHIC  => [:SITRUSBERRY,:TAMATOBERRY,:PAYAPABERRY,:STARFBERRY],
		   :BUG      => [:FIGYBERRY,:CORNNBERRY,:TANGABERRY,:ENIGMABERRY],
		   :ROCK     => [:WIKIBERRY,:MAGOSTBERRY,:CHARTIBERRY,:MICLEBERRY],
		   :GHOST    => [:MAGOBERRY,:RABUTABERRY,:KASIBBERRY,:CUSTAPBERRY],
		   :DRAGON   => [:AGUAVBERRY,:NOMELBERRY,:HABANBERRY,:JABOCABERRY],
		   :DARK     => [:IAPAPABERRY,:SPELONBERRY,:COLBURBERRY,:ROWAPBERRY,:MARANGABERRY],
		   :STEEL    => [:RAZZBERRY,:PAMTREBERRY,:BABIRIBERRY],
		   :FAIRY    => [:ROSELIBERRY,:KEEBERRY]
		}
		foreach (var i in typearray.keys){ 
		  data = typearray[i]
		  if (data){
			foreach (var i in data){ 
			  if (isConst? (this.berry, PBItems, j)){
				 type = getConst(PBTypes, i) || type

			  }
			}

		  }
		}
		return type
	  }

	  public object pbEffectAfterHit(Battle.Battler attacker, Battle.Battler opponent, turneffects){
		if (turneffects.TotalDamage>0){
		  attacker.pbConsumeItem
		}
	  }
	}



	/// <summary>
	/// Power increases the less PP this move has. (Trump Card)
	/// <summary>
	public class PokeBattle_Move_097 : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){
		dmgs =[200, 80, 60, 50, 40]

		ppleft=[this.pp,4].min   // PP is reduced before the move is used
		basedmg = dmgs[ppleft]
		return basedmg
	  }
	}



	/// <summary>
	/// Power increases the less HP the user has. (Flail, Reversal)
	/// <summary>
	public class PokeBattle_Move_098 : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){
		n = (48 * attacker.hp / attacker.TotalHP).floor

		ret=20
		ret=40 if n<33

		ret=80 if n<17

		ret=100 if n<10

		ret=150 if n<5

		ret=200 if n<2
		return ret
	  }
	}



	/// <summary>
	/// Power increases the quicker the user is than the target. (Electro Ball)
	/// <summary>
	public class PokeBattle_Move_099 : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){
		n = ([attacker.pbSpeed, 1].max /[opponent.pbSpeed, 1].max).floor

		ret=60
		ret=80 if n>=2
		ret=120 if n>=3
		ret=150 if n>=4
		return ret
	  }
	}



	/// <summary>
	/// Power increases the heavier the target is. (Grass Knot, Low Kick)
	/// <summary>
	public class PokeBattle_Move_09A : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){
		weight = opponent.weight(attacker)

		ret=20
		ret=40 if weight>=100
		ret=60 if weight>=250
		ret=80 if weight>=500
		ret=100 if weight>=1000
		ret=120 if weight>=2000
		return ret
	  }
	}



	/// <summary>
	/// Power increases the heavier the user is than the target. (Heat Crash, Heavy Slam)
	/// <summary>
	public class PokeBattle_Move_09B : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){
		n = (attacker.weight / opponent.weight(attacker)).floor

		ret=40
		ret=60 if n>=2
		ret=80 if n>=3
		ret=100 if n>=4
		ret=120 if n>=5
		return ret
	  }
	}



	/// <summary>
	/// Powers up the ally's attack this round by 1.5. (Helping Hand)
	/// <summary>
	public class PokeBattle_Move_09C : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (!this.battle.doublebattle || opponent.isFainted() ||){
		   this.battle.choices[opponent.index][0]!=1 || // Didn't choose a move
		   opponent.hasMovedThisRound? ||
		   opponent.effects.HelpingHand
		  //battle.pbDisplay(_INTL("But it failed!"))  
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.effects.HelpingHand=true
		//battle.pbDisplay(_INTL("{1} is ready to help {2}!",attacker.pbThis,opponent.pbThis(true)))
		return 0
	  }
	}



	/// <summary>
	/// Weakens Electric attacks. (Mud Sport)
	/// <summary>
	public class PokeBattle_Move_09D : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (USENEWBATTLEMECHANICS){
		  if (this.battle.field.MudSportField>0){
			//battle.pbDisplay(_INTL("But it failed!"))
			return -1
		  }
		  pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		  this.battle.field.MudSportField=5
		  //battle.pbDisplay(_INTL("Electricity's power was weakened!"))
		  return 0
		else
		  for (int i = 0; i < 4; i++){ 
			if (attacker.battle.battlers[i].effects.MudSport){
			  //battle.pbDisplay(_INTL("But it failed!"))
			  return -1
			}
		  }

		  pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		  attacker.effects.MudSport=true
		  //battle.pbDisplay(_INTL("Electricity's power was weakened!"))
		  return 0
		}
		return -1
	  }
	}



	/// <summary>
	/// Weakens Fire attacks. (Water Sport)
	/// <summary>
	public class PokeBattle_Move_09E : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (USENEWBATTLEMECHANICS){
		  if (this.battle.field.WaterSportField>0){
			//battle.pbDisplay(_INTL("But it failed!"))
			return -1
		  }
		  pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		  this.battle.field.WaterSportField=5
		  //battle.pbDisplay(_INTL("Fire's power was weakened!"))
		  return 0
		else
		  for (int i = 0; i < 4; i++){ 
			if (attacker.battle.battlers[i].effects.WaterSport){
			  //battle.pbDisplay(_INTL("But it failed!"))
			  return -1
			}
		  }

		  pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		  attacker.effects.WaterSport=true
		  //battle.pbDisplay(_INTL("Fire's power was weakened!"))
		  return 0
		}
	  }
	}



	/// <summary>
	/// Type depends on the user's held item. (Judgment, Techno Blast)
	/// <summary>
	public class PokeBattle_Move_09F : PokeBattle_Move
	{
		public object pbModifyType(type, Battle.Battler attacker, Battle.Battler opponent){
		if (id == Moves.JUDGMENT){
		  if (isConst? (attacker.item, PBItems,:FISTPLATE)) return (Types.FIGHTING);
		  if (isConst? (attacker.item, PBItems,:SKYPLATE)) return (Types.FLYING)  ;
		  if (isConst? (attacker.item, PBItems,:TOXICPLATE)) return (Types.POISON)  ;
		  if (isConst? (attacker.item, PBItems,:EARTHPLATE)) return (Types.GROUND)  ;
		  if (isConst? (attacker.item, PBItems,:STONEPLATE)) return (Types.ROCK)    ;
		  if (isConst? (attacker.item, PBItems,:INSECTPLATE)) return (Types.BUG)     ;
		  if (isConst? (attacker.item, PBItems,:SPOOKYPLATE)) return (Types.GHOST)   ;
		  if (isConst? (attacker.item, PBItems,:IRONPLATE)) return (Types.STEEL)   ;
		  if (isConst? (attacker.item, PBItems,:FLAMEPLATE)) return (Types.FIRE)    ;
		  if (isConst? (attacker.item, PBItems,:SPLASHPLATE)) return (Types.WATER)   ;
		  if (isConst? (attacker.item, PBItems,:MEADOWPLATE)) return (Types.GRASS)   ;
		  if (isConst? (attacker.item, PBItems,:ZAPPLATE)) return (Types.ELECTRIC);
		  if (isConst? (attacker.item, PBItems,:MINDPLATE)) return (Types.PSYCHIC) ;
		  if (isConst? (attacker.item, PBItems,:ICICLEPLATE)) return (Types.ICE)     ;
		  if (isConst? (attacker.item, PBItems,:DRACOPLATE)) return (Types.DRAGON)  ;
		  if (isConst? (attacker.item, PBItems,:DREADPLATE)) return (Types.DARK)    ;
		  if (isConst? (attacker.item, PBItems,:PIXIEPLATE)) return (Types.FAIRY)   ;
		else if id == Moves.TECHNOBLAST
		  if (isConst? (attacker.item, PBItems,:SHOCKDRIVE)) return Types.ELECTRIC;
		  if (isConst? (attacker.item, PBItems,:BURNDRIVE)) return Types.FIRE    ;
		  if (isConst? (attacker.item, PBItems,:CHILLDRIVE)) return Types.ICE     ;
		  if (isConst? (attacker.item, PBItems,:DOUSEDRIVE)) return Types.WATER   ;
		}
		return (Types.NORMAL)
	  }

	  public object pbShowAnimation(id, Battle.Battler attacker, Battle.Battler opponent, byte hitnum= 0, byte? alltargets= null, bool showanimation= true){
		if (id == Moves.TECHNOBLAST){
		  anim = 0
		  anim = 1 if isConst? (pbType(this.type, attacker, opponent),PBTypes,:ELECTRIC)
		  anim = 2 if isConst? (pbType(this.type, attacker, opponent),PBTypes,:FIRE)
		  anim = 3 if isConst? (pbType(this.type, attacker, opponent),PBTypes,:ICE)
		  anim = 4 if isConst? (pbType(this.type, attacker, opponent),PBTypes,:WATER)
		  return super(id, attacker, opponent, anim, alltargets, showanimation) // Type-specific anim
		}
		return super(id, attacker, opponent, hitnum, alltargets, showanimation)
	  }
	}



	/// <summary>
	/// This attack is always a critical hit. (Frost Breath, Storm Throw)
	/// <summary>
	public class PokeBattle_Move_0A0 : PokeBattle_Move
	{
		public object pbCritialOverride(Battle.Battler attacker, Battle.Battler opponent){
		return true
	  }
	}



	/// <summary>
	/// For 5 rounds, foes' attacks cannot become critical hits. (Lucky Chant)
	/// <summary>
	public class PokeBattle_Move_0A1 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (attacker.OwnSide.LuckyChant>0){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		attacker.OwnSide.LuckyChant=5
		if (!this.battle.pbIsOpposing? (attacker.index)){
		   //battle.pbDisplay(_INTL("The Lucky Chant shielded your team from critical hits!"))
		else
		  //battle.pbDisplay(_INTL("The Lucky Chant shielded the opposing team from critical hits!"))
		}  
		return 0
	  }
	}



	/// <summary>
	/// For 5 rounds, lowers power of physical attacks against the user's side. (Reflect)
	/// <summary>
	public class PokeBattle_Move_0A2 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (attacker.OwnSide.Reflect>0){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		attacker.OwnSide.Reflect=5
		attacker.OwnSide.Reflect=8 if attacker.hasWorkingItem(Items.LIGHTCLAY)
		if (!this.battle.pbIsOpposing? (attacker.index)){
		   //battle.pbDisplay(_INTL("Reflect raised your team's Defense!"))
		else
		  //battle.pbDisplay(_INTL("Reflect raised the opposing team's Defense!"))
		}  
		return 0
	  }
	}



	/// <summary>
	/// For 5 rounds, lowers power of special attacks against the user's side. (Light Screen)
	/// <summary>
	public class PokeBattle_Move_0A3 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (attacker.OwnSide.LightScreen>0){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		attacker.OwnSide.LightScreen=5
		attacker.OwnSide.LightScreen=8 if attacker.hasWorkingItem(Items.LIGHTCLAY)
		if (!this.battle.pbIsOpposing? (attacker.index)){
		   //battle.pbDisplay(_INTL("Light Screen raised your team's Special Defense!"))
		else
		  //battle.pbDisplay(_INTL("Light Screen raised the opposing team's Special Defense!"))
		}
		return 0
	  }
	}



	/// <summary>
	/// Effect depends on the environment. (Secret Power)
	/// <summary>
	public class PokeBattle_Move_0A4 : PokeBattle_Move
	{
		public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		if (this.battle.field.ElectricTerrain>0){
		  if (opponent.pbCanParalyze? (attacker,false,self)){
			opponent.pbParalyze(attacker)
		  }
		  return
		else if this.battle.field.GrassyTerrain>0
		  if (opponent.pbCanSleep?(attacker,false, self)){

			opponent.pbSleep
		  }
		  return
		else if this.battle.field.MistyTerrain>0
		  if (opponent.pbCanReduceStatStage?(PBStats::SPATK, attacker,false, self)){

			opponent.pbReduceStat(PBStats::SPATK,1,attacker,false,self)
		  }
		  return
		}
		case this.battle.environment
		when PBEnvironment::Grass, PBEnvironment::TallGrass, PBEnvironment::Forest
		  if (opponent.pbCanSleep? (attacker,false,self)){
			opponent.pbSleep
		  }

		when PBEnvironment::MovingWater, PBEnvironment::Underwater
		  if (opponent.pbCanReduceStatStage?(PBStats::ATTACK, attacker,false, self)){

			opponent.pbReduceStat(PBStats::ATTACK,1,attacker,false,self)
		  }
		when PBEnvironment::StillWater, PBEnvironment::Sky
		  if (opponent.pbCanReduceStatStage? (PBStats::SPEED, attacker,false,self)){
			opponent.pbReduceStat(PBStats::SPEED,1,attacker,false,self)
		  }
		when PBEnvironment::Sand
		  if (opponent.pbCanReduceStatStage? (PBStats::ACCURACY, attacker,false,self)){
			opponent.pbReduceStat(PBStats::ACCURACY,1,attacker,false,self)
		  }
		when PBEnvironment::Rock
		  if (USENEWBATTLEMECHANICS){
			if (opponent.pbCanReduceStatStage? (PBStats::ACCURACY, attacker,false,self)){
			  opponent.pbReduceStat(PBStats::ACCURACY,1,attacker,false,self)
			}
		  else
			if (opponent.effects.Substitute==0 || ignoresSubstitute? (attacker)){
			   opponent.pbFlinch(attacker)

			}
		  }

		when PBEnvironment::Cave, PBEnvironment::Graveyard, PBEnvironment::Space
		  if (opponent.effects.Substitute==0 || ignoresSubstitute?(attacker)){
			opponent.pbFlinch(attacker)
		  }

		when PBEnvironment::Snow
		  if (opponent.pbCanFreeze?(attacker,false, self)){

			opponent.pbFreeze
		  }

		when PBEnvironment::Volcano
		  if (opponent.pbCanBurn?(attacker,false, self)){

			opponent.pbBurn(attacker)
		  }
		else
		  if (opponent.pbCanParalyze? (attacker,false,self)){
			opponent.pbParalyze(attacker)
		  }

		}
	  }

	  public object pbShowAnimation(id, Battle.Battler attacker, Battle.Battler opponent, byte hitnum= 0, byte? alltargets= null, bool showanimation= true){

		id=Moves.BODYSLAM
		if (this.battle.field.ElectricTerrain>0){
		  id=Moves.THUNDERSHOCK
		else if this.battle.field.GrassyTerrain>0
		  id=Moves.VINEWHIP
		else if this.battle.field.MistyTerrain>0
		  id=Moves.FAIRYWIND
		else
		  case this.battle.environment
		  when PBEnvironment::Grass, PBEnvironment::TallGrass
			id = ((USENEWBATTLEMECHANICS) ? Moves.VINEWHIP : Moves.NEEDLEARM) || id

		  when PBEnvironment::MovingWater; id=Moves.WATERPULSE
		  when PBEnvironment::StillWater;  id=Moves.MUDSHOT
		  when PBEnvironment::Underwater;  id=Moves.WATERPULSE
		  when PBEnvironment::Cave;        id=Moves.ROCKTHROW
		  when PBEnvironment::Rock;        id=Moves.MUDSLAP
		  when PBEnvironment::Sand;        id=Moves.MUDSLAP
		  when PBEnvironment::Forest;      id=Moves.RAZORLEAF
		// Ice tiles in Gen 6 should be Ice Shard
		  when PBEnvironment::Snow;        id=Moves.AVALANCHE
		  when PBEnvironment::Volcano;     id=Moves.INCINERATE
		  when PBEnvironment::Graveyard;   id=Moves.SHADOWSNEAK
		  when PBEnvironment::Sky;         id=Moves.GUST
		  when PBEnvironment::Space;       id=Moves.SWIFT
		  }

		}
		return super(id, attacker, opponent, hitnum, alltargets, showanimation) // Environment-specific anim
	  }
	}



	/// <summary>
	/// Always hits.
	/// <summary>
	public class PokeBattle_Move_0A5 : PokeBattle_Move
	{
		public object pbAccuracyCheck(Battle.Battler attacker, Battle.Battler opponent){
		return true
	  }
	}



	/// <summary>
	/// User's attack next round against the target will definitely hit. (Lock-On, Mind Reader)
	/// <summary>
	public class PokeBattle_Move_0A6 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (opponent.effects.Substitute>0 && !ignoresSubstitute? (attacker)){
		   //battle.pbDisplay(_INTL("But it failed!"))  
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.effects.LockOn=2
		opponent.effects.LockOnPos=attacker.index
		//battle.pbDisplay(_INTL("{1} took aim at {2}!", attacker.pbThis, opponent.pbThis(true)))
		return 0
	  }
	}



	/// <summary>
	/// Target's evasion stat changes are ignored from now on. (Foresight, Odor Sleuth)
	/// Normal and Fighting moves have normal effectiveness against the Ghost-type target.
	/// <summary>
	public class PokeBattle_Move_0A7 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (opponent.OwnSide.CraftyShield){
		  //battle.pbDisplay(_INTL("But it failed!"))  
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.effects.Foresight=true
		//battle.pbDisplay(_INTL("{1} was identified!",opponent.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// Target's evasion stat changes are ignored from now on. (Miracle Eye)
	/// Psychic moves have normal effectiveness against the Dark-type target.
	/// <summary>
	public class PokeBattle_Move_0A8 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (opponent.OwnSide.CraftyShield){
		  //battle.pbDisplay(_INTL("But it failed!"))  
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.effects.MiracleEye=true
		//battle.pbDisplay(_INTL("{1} was identified!",opponent.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// This move ignores target's Defense, Special Defense and evasion stat changes.
	/// (Chip Away, Sacred Sword)
	/// <summary>
	public class PokeBattle_Move_0A9 : PokeBattle_Move
	{
		// Handled in superclass public object pbAccuracyCheck and public object pbCalcDamage, do not edit!
	}



	/// <summary>
	/// User is protected against moves with the "B" flag this round. (Detect, Protect)
	/// <summary>
	public class PokeBattle_Move_0AA : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		ratesharers=[
		   0xAA,   // Detect, Protect
		   0xAB,   // Quick Guard
		   0xAC,   // Wide Guard
		   0xE8,   // Endure
		   0x14B,  // King's Shield
		   0x14C   // Spiky Shield
		]
		if (!ratesharers.include? (PBMoveData.new(attacker.lastMoveUsed).function)){
		  attacker.effects.ProtectRate=1
		}
		unmoved = false
		for poke in this.battle.battlers
		  if (poke.index==attacker.index) continue; //next
		  if (this.battle.choices[poke.index][0]==1 && // Chose a move){
			 !poke.hasMovedThisRound?
			unmoved = true; break
		  }
		}
		if (!unmoved ||){
		   this.battle.pbRandom(65536)>=(65536/attacker.effects.ProtectRate).floor
		  attacker.effects.ProtectRate= 1

		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		attacker.effects.Protect=true
		attacker.effects.ProtectRate*=2
		//battle.pbDisplay(_INTL("{1} protected itself!",attacker.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// User's side is protected against moves with priority greater than 0 this round.
	/// (Quick Guard)
	/// <summary>
	public class PokeBattle_Move_0AB : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (attacker.OwnSide.QuickGuard){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		ratesharers =[
		   0xAA,   // Detect, Protect
		   0xAB,   // Quick Guard
		   0xAC,   // Wide Guard
		   0xE8,   // Endure
		   0x14B,  // King's Shield
		   0x14C   // Spiky Shield
		]
		if (!ratesharers.include? (PBMoveData.new(attacker.lastMoveUsed).function)){
		  attacker.effects.ProtectRate=1
		}
		unmoved = false
		for poke in this.battle.battlers
		  if (poke.index==attacker.index) continue; //next
		  if (this.battle.choices[poke.index][0]==1 && // Chose a move){
			 !poke.hasMovedThisRound?
			unmoved = true; break
		  }
		}
		if (!unmoved ||){
		   (!USENEWBATTLEMECHANICS &&
		   this.battle.pbRandom(65536)>=(65536/attacker.effects.ProtectRate).floor)
		  attacker.effects.ProtectRate=1
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		attacker.OwnSide.QuickGuard=true
		attacker.effects.ProtectRate*=2
		if (!this.battle.pbIsOpposing? (attacker.index)){
		   //battle.pbDisplay(_INTL("Quick Guard protected your team!"))
		else
		  //battle.pbDisplay(_INTL("Quick Guard protected the opposing team!"))
		}
		return 0
	  }
	}



	/// <summary>
	/// User's side is protected against moves that target multiple battlers this round.
	/// (Wide Guard)
	/// <summary>
	public class PokeBattle_Move_0AC : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (attacker.OwnSide.WideGuard){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		ratesharers =[
		   0xAA,   // Detect, Protect
		   0xAB,   // Quick Guard
		   0xAC,   // Wide Guard
		   0xE8,   // Endure
		   0x14B,  // King's Shield
		   0x14C   // Spiky Shield
		]
		if (!ratesharers.include? (PBMoveData.new(attacker.lastMoveUsed).function)){
		  attacker.effects.ProtectRate=1
		}
		unmoved = false
		for poke in this.battle.battlers
		  if (poke.index==attacker.index) continue; //next
		  if (this.battle.choices[poke.index][0]==1 && // Chose a move){
			 !poke.hasMovedThisRound?
			unmoved = true; break
		  }
		}
		if (!unmoved ||){
		   (!USENEWBATTLEMECHANICS &&
		   this.battle.pbRandom(65536)>=(65536/attacker.effects.ProtectRate).floor)
		  attacker.effects.ProtectRate=1
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		attacker.OwnSide.WideGuard=true
		attacker.effects.ProtectRate*=2
		if (!this.battle.pbIsOpposing? (attacker.index)){
		   //battle.pbDisplay(_INTL("Wide Guard protected your team!"))
		else
		  //battle.pbDisplay(_INTL("Wide Guard protected the opposing team!"))
		}
		return 0
	  }
	}



	/// <summary>
	/// Ignores target's protections. If successful, all other moves this round
	/// ignore them too. (Feint)
	/// <summary>
	public class PokeBattle_Move_0AD : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if (ret>0){
		  opponent.effects.ProtectNegation=true
		  opponent.OwnSide.CraftyShield=false
		}
		return ret
	  }
	}



	/// <summary>
	/// Uses the last move that the target used. (Mirror Move)
	/// <summary>
	public class PokeBattle_Move_0AE : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (opponent.lastMoveUsed<=0 ||){
		   (PBMoveData.new(opponent.lastMoveUsed).flags&0x10)==0 // flag e: Copyable by Mirror Move
		  //battle.pbDisplay(_INTL("The mirror move failed!"))
		  return -1
		}
		attacker.pbUseMoveSimple(opponent.lastMoveUsed,-1, opponent.index)
		return 0
	  }
	}



	/// <summary>
	/// Uses the last move that was used. (Copycat)
	/// <summary>
	public class PokeBattle_Move_0AF : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		blacklist=[
		   0x02,    // Struggle
		   0x69,    // Transform
		   0x71,    // Counter
		   0x72,    // Mirror Coat
		   0x73,    // Metal Burst
		   0x9C,    // Helping Hand
		   0xAA,    // Detect, Protect
		   0xAD,    // Feint
		   0xAE,    // Mirror Move
		   0xAF,    // Copycat
		   0xB2,    // Snatch
		   0xE7,    // Destiny Bond
		   0xE8,    // Endure
		   0xEC,    // Circle Throw, Dragon Tail
		   0xF1,    // Covet, Thief
		   0xF2,    // Switcheroo, Trick
		   0xF3,    // Bestow
		   0x115,   // Focus Punch
		   0x117,   // Follow Me, Rage Powder
		   0x158    // Belch
		]
		if (USENEWBATTLEMECHANICS){
		  blacklist+=[
			 0xEB,    // Roar, Whirlwind
			 // Two-turn attacks
			 0xC3,    // Razor Wind
			 0xC4,    // SolarBeam
			 0xC5,    // Freeze Shock
			 0xC6,    // Ice Burn
			 0xC7,    // Sky Attack
			 0xC8,    // Skull Bash
			 0xC9,    // Fly
			 0xCA,    // Dig
			 0xCB,    // Dive
			 0xCC,    // Bounce
			 0xCD,    // Shadow Force
			 0xCE,    // Sky Drop
			 0x14D,   // Phantom Force
			 0x14E    // Geomancy
		  ]
	}
		if (this.battle.lastMoveUsed<=0 ||){
		   blacklist.include? (PBMoveData.new(this.battle.lastMoveUsed).function)
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		attacker.pbUseMoveSimple(this.battle.lastMoveUsed)
		return 0
	  }
	}



	/// <summary>
	/// Uses the move the target was about to use this round, with 1.5x power. (Me First)
	/// <summary>
	public class PokeBattle_Move_0B0 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		blacklist=[
		   0x02,    // Struggle
		   0x14,    // Chatter
		   0x71,    // Counter
		   0x72,    // Mirror Coat
		   0x73,    // Metal Burst
		   0xB0,    // Me First
		   0xF1,    // Covet, Thief
		   0x115,   // Focus Punch
		   0x158    // Belch
		]
	oppmove=this.battle.choices[opponent.index][2]
		if (this.battle.choices[opponent.index][0]!=1 || // Didn){'t choose a move
		   opponent.hasMovedThisRound? ||
		   !oppmove || oppmove.id<=0 ||
		   oppmove.pbIsStatus? ||
		   blacklist.include? (oppmove.function)
		   //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		attacker.effects.MeFirst= true

		attacker.pbUseMoveSimple(oppmove.id)
		attacker.effects.MeFirst= false
		return 0
	  }
	}



	/// <summary>
	/// This round, reflects all moves with the "C" flag targeting the user back at
	/// their origin. (Magic Coat)
	/// <summary>
	public class PokeBattle_Move_0B1 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		attacker.effects.MagicCoat=true
		//battle.pbDisplay(_INTL("{1} shrouded itself with Magic Coat!",attacker.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// This round, snatches all used moves with the "D" flag. (Snatch)
	/// <summary>
	public class PokeBattle_Move_0B2 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		attacker.effects.Snatch=true
		//battle.pbDisplay(_INTL("{1} waits for a target to make a move!",attacker.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// Uses a different move depending on the environment. (Nature Power)
	/// <summary>
	public class PokeBattle_Move_0B3 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		move=Moves.TRIATTACK
		case this.battle.environment
		when PBEnvironment::Grass, PBEnvironment::TallGrass, PBEnvironment::Forest
		  move = ((USENEWBATTLEMECHANICS) ? Moves.ENERGYBALL : Moves.SEEDBOMB) || move

		when PBEnvironment::MovingWater; move=Moves.HYDROPUMP
		when PBEnvironment::StillWater;  move=Moves.MUDBOMB
		when PBEnvironment::Underwater;  move=Moves.HYDROPUMP
		when PBEnvironment::Cave
		  move = ((USENEWBATTLEMECHANICS) ? Moves.POWERGEM : Moves.ROCKSLIDE) || move

		when PBEnvironment::Rock

		  move= ((USENEWBATTLEMECHANICS) ? Moves.EARTHPOWER : Moves.ROCKSLIDE) || move

		when PBEnvironment::Sand
		  move = ((USENEWBATTLEMECHANICS) ? Moves.EARTHPOWER : Moves.EARTHQUAKE) || move
		// Ice tiles in Gen 6 should be Ice Beam
		when PBEnvironment::Snow
		  move = ((USENEWBATTLEMECHANICS) ? Moves.FROSTBREATH : Moves.ICEBEAM) || move

		when PBEnvironment::Volcano; move=Moves.LAVAPLUME
	when PBEnvironment::Graveyard;   move=Moves.SHADOWBALL
	when PBEnvironment::Sky;         move=Moves.AIRSLASH
	when PBEnvironment::Space;       move=Moves.DRACOMETEOR
	}
		if (this.battle.field.ElectricTerrain>0){
		  move=Moves.THUNDERBOLT
		else if this.battle.field.GrassyTerrain>0
		  move=Moves.ENERGYBALL
		else if this.battle.field.MistyTerrain>0
		  move=Moves.MOONBLAST
		}
		if (move==0){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		thismovename = PBMoves.getName(this.id)

		movename=PBMoves.getName(move)
		//battle.pbDisplay(_INTL("{1} turned into {2}!", thismovename, movename))
		target=(USENEWBATTLEMECHANICS && opponent) ? opponent.index : -1
		attacker.pbUseMoveSimple(move,-1, target)
		return 0
	  }
	}



	/// <summary>
	/// Uses a random move the user knows. Fails if user is not asleep. (Sleep Talk)
	/// <summary>
	public class PokeBattle_Move_0B4 : PokeBattle_Move
	{
		public object pbCanUseWhileAsleep?
		return true
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (attacker.status!=PBStatuses::SLEEP){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		blacklist =[
		   0x02,    // Struggle
		   0x14,    // Chatter
		   0x5C,    // Mimic
		   0x5D,    // Sketch
		   0xAE,    // Mirror Move
		   0xAF,    // Copycat
		   0xB0,    // Me First
		   0xB3,    // Nature Power
		   0xB4,    // Sleep Talk
		   0xB5,    // Assist
		   0xB6,    // Metronome
		   0xD1,    // Uproar
		   0xD4,    // Bide
		   0x115,   // Focus Punch
	/// Two-turn attacks
		   0xC3,    // Razor Wind
		   0xC4,    // SolarBeam
		   0xC5,    // Freeze Shock
		   0xC6,    // Ice Burn
		   0xC7,    // Sky Attack
		   0xC8,    // Skull Bash
		   0xC9,    // Fly
		   0xCA,    // Dig
		   0xCB,    // Dive
		   0xCC,    // Bounce
		   0xCD,    // Shadow Force
		   0xCE,    // Sky Drop
		   0x14D,   // Phantom Force
		   0x14E,   // Geomancy
		]

		choices=[]
		for (int i = 0; i < 4; i++){ 
		  found=false
		  if (attacker.moves[i].id==0) continue; //next
		  found=true if blacklist.include? (attacker.moves[i].function)
		  if (found) continue; //next
		  choices.push(i) if this.battle.pbCanChooseMove? (attacker.index, i,false,true)
		}
		if (choices.length==0){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		choice=choices[this.battle.pbRandom(choices.length)]
		attacker.pbUseMoveSimple(attacker.moves[choice].id,-1, attacker.pbOppositeOpposing.index)
		return 0
	  }
	}



	/// <summary>
	/// Uses a random move known by any non-user Pokémon in the user's party. (Assist)
	/// <summary>
	public class PokeBattle_Move_0B5 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		blacklist=[
		   0x02,    // Struggle
		   0x14,    // Chatter
		   0x5C,    // Mimic
		   0x5D,    // Sketch
		   0x69,    // Transform
		   0x71,    // Counter
		   0x72,    // Mirror Coat
		   0x73,    // Metal Burst
		   0x9C,    // Helping Hand
		   0xAA,    // Detect, Protect
		   0xAD,    // Feint
		   0xAE,    // Mirror Move
		   0xAF,    // Copycat
		   0xB0,    // Me First
		   0xB2,    // Snatch
		   0xB3,    // Nature Power
		   0xB4,    // Sleep Talk
		   0xB5,    // Assist
		   0xB6,    // Metronome
		   0xCD,    // Shadow Force
		   0xE7,    // Destiny Bond
		   0xE8,    // Endure
		   0xEB,    // Roar, Whirlwind
		   0xEC,    // Circle Throw, Dragon Tail
		   0xF1,    // Covet, Thief
		   0xF2,    // Switcheroo, Trick
		   0xF3,    // Bestow
		   0x115,   // Focus Punch
		   0x117,   // Follow Me, Rage Powder
		   0x149,   // Mat Block
		   0x14B,   // King's Shield
		   0x14C,   // Spiky Shield
		   0x14D,   // Phantom Force
		   0x158    // Belch
		]
		if (USENEWBATTLEMECHANICS){
		  blacklist+=[
			 // Two-turn attacks
			 0xC3,    // Razor Wind
			 0xC4,    // SolarBeam
			 0xC5,    // Freeze Shock
			 0xC6,    // Ice Burn
			 0xC7,    // Sky Attack
			 0xC8,    // Skull Bash
			 0xC9,    // Fly
			 0xCA,    // Dig
			 0xCB,    // Dive
			 0xCC,    // Bounce
			 0xCD,    // Shadow Force
			 0xCE,    // Sky Drop
			 0x14D,   // Phantom Force
			 0x14E    // Geomancy
		  ]
	}
		moves =[]

		party=this.battle.pbParty(attacker.index) // NOTE: pbParty is common to both allies in multi battles
		for (int i = 0; i < party.length; i++){ 
		  if (i!=attacker.pokemonIndex && party[i] && !(USENEWBATTLEMECHANICS && party[i].egg?)){
			foreach (var i in party[i].moves){ 
			  if (isConst? (j.type, PBTypes,:SHADOW)) continue; //next
			  if (j.id==0) continue; //next
			  found=false
			  moves.push(j.id) if !blacklist.include? (PBMoveData.new(j.id).function)
			}
		  }

		}
		if (moves.length==0){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		move=moves[this.battle.pbRandom(moves.length)]
		attacker.pbUseMoveSimple(move)
		return 0
	  }
	}



	/// <summary>
	/// Uses a random move that exists. (Metronome)
	/// <summary>
	public class PokeBattle_Move_0B6 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		blacklist=[
		   0x02,    // Struggle
		   0x11,    // Snore
		   0x14,    // Chatter
		   0x5C,    // Mimic
		   0x5D,    // Sketch
		   0x69,    // Transform
		   0x71,    // Counter
		   0x72,    // Mirror Coat
		   0x73,    // Metal Burst
		   0x9C,    // Helping Hand
		   0xAA,    // Detect, Protect
		   0xAB,    // Quick Guard
		   0xAC,    // Wide Guard
		   0xAD,    // Feint
		   0xAE,    // Mirror Move
		   0xAF,    // Copycat
		   0xB0,    // Me First
		   0xB2,    // Snatch
		   0xB3,    // Nature Power
		   0xB4,    // Sleep Talk
		   0xB5,    // Assist
		   0xB6,    // Metronome
		   0xE7,    // Destiny Bond
		   0xE8,    // Endure
		   0xF1,    // Covet, Thief
		   0xF2,    // Switcheroo, Trick
		   0xF3,    // Bestow
		   0x115,   // Focus Punch
		   0x117,   // Follow Me, Rage Powder
		   0x11D,   // After You
		   0x11E    // Quash
		]
	blacklistmoves=[
		   :FREEZESHOCK,
		   :ICEBURN,
		   :RELICSONG,
		   :SECRETSWORD,
		   :SNARL,
		   :TECHNOBLAST,
		   :VCREATE,
		   :GEOMANCY
	]
	i=0; loop do break unless i<1000
		  move=this.battle.pbRandom(PBMoves.maxValue)+1
		  if (isConst? (PBMoveData.new(move).type,PBTypes,:SHADOW)) continue; //next
		  found=false
		  if (blacklist.include? (PBMoveData.new(move).function)){
			found=true
		  else
			foreach (var i in blacklistmoves){ 
			  if (isConst? (move, PBMoves, j)){
				 found = true
				break
			  }
			}

		  }
		  if (!found){
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

			attacker.pbUseMoveSimple(move)
			return 0
		  }
		  i+=1
		}
		//battle.pbDisplay(_INTL("But it failed!"))
		return -1
	  }
	}



	/// <summary>
	/// The target can no longer use the same move twice in a row. (Torment)
	/// <summary>
	public class PokeBattle_Move_0B7 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (opponent.effects.Torment){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if (!attacker.hasMoldBreaker){
		  if (opponent.hasWorkingAbility(Abilities.AROMAVEIL)){
			//battle.pbDisplay(_INTL("But it failed because of {1}'s {2}!",
			   opponent.pbThis,PBAbilities.getName(opponent.ability)))
			return -1
		  else if opponent.pbPartner.hasWorkingAbility(Abilities.AROMAVEIL)

			//battle.pbDisplay(_INTL("But it failed because of {1}'s {2}!",
			   opponent.pbPartner.pbThis,PBAbilities.getName(opponent.pbPartner.ability)))
			return -1
		  }
		}

		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.effects.Torment=true
		//battle.pbDisplay(_INTL("{1} was subjected to torment!",opponent.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// Disables all target's moves that the user also knows. (Imprison)
	/// <summary>
	public class PokeBattle_Move_0B8 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (attacker.effects.Imprison){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1  
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		attacker.effects.Imprison=true
		//battle.pbDisplay(_INTL("{1} sealed the opponent's move(s)!",attacker.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// For 5 rounds, disables the last move the target used. (Disable)
	/// <summary>
	public class PokeBattle_Move_0B9 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (opponent.effects.Disable>0){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if (!attacker.hasMoldBreaker){
		  if (opponent.hasWorkingAbility(Abilities.AROMAVEIL)){
			//battle.pbDisplay(_INTL("But it failed because of {1}'s {2}!",
			   opponent.pbThis,PBAbilities.getName(opponent.ability)))
			return -1
		  else if opponent.pbPartner.hasWorkingAbility(Abilities.AROMAVEIL)

			//battle.pbDisplay(_INTL("But it failed because of {1}'s {2}!",
			   opponent.pbPartner.pbThis,PBAbilities.getName(opponent.pbPartner.ability)))
			return -1
		  }
		}
		foreach (var i in opponent.moves){ 
		  if (i.id>0 && i.id==opponent.lastMoveUsed && (i.pp>0 || i.totalpp==0)){
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

			opponent.effects.Disable=5
			opponent.effects.DisableMove=opponent.lastMoveUsed
			//battle.pbDisplay(_INTL("{1}'s {2} was disabled!", opponent.pbThis, i.name))
			return 0
		  }
		}

		//battle.pbDisplay(_INTL("But it failed!"))
		return -1
	  }
	}



	/// <summary>
	/// For 4 rounds, disables the target's non-damaging moves. (Taunt)
	/// <summary>
	public class PokeBattle_Move_0BA : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (opponent.effects.Taunt>0 ||){
		   (USENEWBATTLEMECHANICS &&
		   !attacker.hasMoldBreaker && opponent.hasWorkingAbility(Abilities.OBLIVIOUS))
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if (!attacker.hasMoldBreaker){
		  if (opponent.hasWorkingAbility(Abilities.AROMAVEIL)){
			//battle.pbDisplay(_INTL("But it failed because of {1}'s {2}!",
			   opponent.pbThis,PBAbilities.getName(opponent.ability)))
			return -1
		  else if opponent.pbPartner.hasWorkingAbility(Abilities.AROMAVEIL)

			//battle.pbDisplay(_INTL("But it failed because of {1}'s {2}!",
			   opponent.pbPartner.pbThis,PBAbilities.getName(opponent.pbPartner.ability)))
			return -1
		  }
		}

		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.effects.Taunt=4
		//battle.pbDisplay(_INTL("{1} fell for the taunt!",opponent.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// For 5 rounds, disables the target's healing moves. (Heal Block)
	/// <summary>
	public class PokeBattle_Move_0BB : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (opponent.effects.HealBlock>0){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if (!attacker.hasMoldBreaker){
		  if (opponent.hasWorkingAbility(Abilities.AROMAVEIL)){
			//battle.pbDisplay(_INTL("But it failed because of {1}'s {2}!",
			   opponent.pbThis,PBAbilities.getName(opponent.ability)))
			return -1
		  else if opponent.pbPartner.hasWorkingAbility(Abilities.AROMAVEIL)

			//battle.pbDisplay(_INTL("But it failed because of {1}'s {2}!",
			   opponent.pbPartner.pbThis,PBAbilities.getName(opponent.pbPartner.ability)))
			return -1
		  }
		}

		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.effects.HealBlock=5
		//battle.pbDisplay(_INTL("{1} was prevented from healing!",opponent.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// For 4 rounds, the target must use the same move each round. (Encore)
	/// <summary>
	public class PokeBattle_Move_0BC : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		blacklist=[
		   0x02,    // Struggle
		   0x5C,    // Mimic
		   0x5D,    // Sketch
		   0x69,    // Transform
		   0xAE,    // Mirror Move
		   0xBC     // Encore
		]
		if (opponent.effects.Encore>0){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if (opponent.lastMoveUsed<=0 ||){
		   blacklist.include? (PBMoveData.new(opponent.lastMoveUsed).function)
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if (!attacker.hasMoldBreaker){
		  if (opponent.hasWorkingAbility(Abilities.AROMAVEIL)){
			//battle.pbDisplay(_INTL("But it failed because of {1}'s {2}!",
			   opponent.pbThis,PBAbilities.getName(opponent.ability)))
			return -1
		  else if opponent.pbPartner.hasWorkingAbility(Abilities.AROMAVEIL)

			//battle.pbDisplay(_INTL("But it failed because of {1}'s {2}!",
			   opponent.pbPartner.pbThis,PBAbilities.getName(opponent.pbPartner.ability)))
			return -1
		  }
		}
		for (int i = 0; i < 4; i++){ 
		  if (opponent.lastMoveUsed==opponent.moves[i].id &&){
			 (opponent.moves[i].pp>0 || opponent.moves[i].totalpp==0)
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

			opponent.effects.Encore=4
			opponent.effects.EncoreIndex=i
			opponent.effects.EncoreMove= opponent.moves[i].id

			//battle.pbDisplay(_INTL("{1} received an encore!", opponent.pbThis))
			return 0
		  }
		}

		//battle.pbDisplay(_INTL("But it failed!"))
		return -1
	  }
	}



	/// <summary>
	/// Hits twice.
	/// <summary>
	public class PokeBattle_Move_0BD : PokeBattle_Move
	{
		public object pbIsMultiHit
		return true
	  }

	  public object pbNumHits(Battle.Battler attacker){
		return 2
	  }
	}



	/// <summary>
	/// Hits twice. May poison the target on each hit. (Twineedle)
	/// <summary>
	public class PokeBattle_Move_0BE : PokeBattle_Move
	{
		public object pbIsMultiHit
		return true
	  }

	  public object pbNumHits(Battle.Battler attacker){
		return 2
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		if (opponent.pbCanPoison? (attacker,false,self)){
		  opponent.pbPoison(attacker)
		}
	  }
	}



	/// <summary>
	/// Hits 3 times. Power is multiplied by the hit number. (Triple Kick)
	/// An accuracy check is performed for each hit.
	/// <summary>
	public class PokeBattle_Move_0BF : PokeBattle_Move
	{
		public object pbIsMultiHit
		return true
	  }

	  public object pbNumHits(Battle.Battler attacker){
		return 3
	  }

	  public object successCheckPerHit?
		return this.checks
	  }

	  public object pbOnStartUse(Battle.Battler attacker)

		this.calcbasedmg=this.basedamage
		this.checks = !attacker.hasWorkingAbility(Abilities.SKILLLINK)
		return true
	  }

	  public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){

		ret=this.calcbasedmg
		this.calcbasedmg+=basedmg
		return ret
	  }
	}



	/// <summary>
	/// Hits 2-5 times.
	/// <summary>
	public class PokeBattle_Move_0C0 : PokeBattle_Move
	{
		public object pbIsMultiHit
		return true
	  }

	  public object pbNumHits(Battle.Battler attacker){
		hitchances =[2, 2, 3, 3, 4, 5]

		ret=hitchances[this.battle.pbRandom(hitchances.length)]
		ret = 5 if attacker.hasWorkingAbility(Abilities.SKILLLINK)
		return ret
	  }
	}



	/// <summary>
	/// Hits X times, where X is 1 (the user) plus the number of non-user unfainted
	/// status-free Pokémon in the user's party (the participants). Fails if X is 0.
	/// Base power of each hit depends on the base Attack stat for the species of that
	/// hit's participant. (Beat Up)
	/// <summary>
	public class PokeBattle_Move_0C1 : PokeBattle_Move
	{
		public object pbIsMultiHit
		return true
	  }

	  public object pbNumHits(Battle.Battler attacker){
		return this.participants.length
	  }

	  public object pbOnStartUse(Battle.Battler attacker){

		party=this.battle.pbParty(attacker.index)
		this.participants =[]
		for (int i = 0; i < party.length; i++){ 
		  if (attacker.pokemonIndex==i){
			this.participants.push(i)

		  else if party[i] && !party[i].egg? && party[i].hp>0 && party[i].status==0
			this.participants.push(i)
		  }

		}
		if (this.participants.length==0){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return false
		}
		return true
	  }

	  public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){

		party=this.battle.pbParty(attacker.index)
		atk = party[this.participants[0]].baseStats[1]

		this.participants[0]=null; this.participants.compact!
		return 5+(atk/10)
	  }
	}



	/// <summary>
	/// Two turn attack. Attacks first turn, skips second turn (if successful).
	/// <summary>
	public class PokeBattle_Move_0C2 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if (opponent.damagestate.CalcDamage>0){
		  attacker.effects.HyperBeam=2
		  attacker.currentMove=this.id
		}
		return ret
	  }
	}



	/// <summary>
	/// Two turn attack. Skips first turn, attacks second turn. (Razor Wind)
	/// <summary>
	public class PokeBattle_Move_0C3 : PokeBattle_Move
	{
		public object pbTwoTurnAttack(Battle.Battler attacker)
		this.immediate = false
		if (!this.immediate && attacker.hasWorkingItem(Items.POWERHERB)){
		  this.immediate=true
		}
		if (this.immediate) return false;
		return attacker.effects.TwoTurnAttack==0
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (this.immediate || attacker.effects.TwoTurnAttack>0){
		  pbShowAnimation(this.id, attacker, opponent,1, alltargets, showanimation) // Charging anim
		  //battle.pbDisplay(_INTL("{1} whipped up a whirlwind!",attacker.pbThis))
		}
		if (this.immediate){
		  this.battle.pbCommonAnimation("UseItem", attacker, null)

		  //battle.pbDisplay(_INTL("{1} became fully charged due to its Power Herb!",attacker.pbThis))
		  attacker.pbConsumeItem
		}
		if (attacker.effects.TwoTurnAttack>0) return 0;
		return super(attacker, opponent, hitnum, alltargets, showanimation)
	  }
	}



	/// <summary>
	/// Two turn attack. Skips first turn, attacks second turn. (SolarBeam)
	/// Power halved in all weather except sunshine. In sunshine, takes 1 turn instead.
	/// <summary>
	public class PokeBattle_Move_0C4 : PokeBattle_Move
	{
		public object pbTwoTurnAttack(Battle.Battler attacker)
		this.immediate = false; this.sunny=false
		if (attacker.effects.TwoTurnAttack==0){
		  if (this.battle.pbWeather==PBWeather::SUNNYDAY ||){
			 this.battle.pbWeather==PBWeather::HARSHSUN
			this.immediate = true; this.sunny=true
		  }
		}
		if (!this.immediate && attacker.hasWorkingItem(Items.POWERHERB)){
		  this.immediate=true
		}
		if (this.immediate) return false;
		return attacker.effects.TwoTurnAttack==0
	  }

	  public object pbBaseDamageMultiplier(damagemult, Battle.Battler attacker, Battle.Battler opponent){
		if (this.battle.pbWeather!=0 &&){
		   this.battle.pbWeather!=PBWeather::SUNNYDAY &&
		   this.battle.pbWeather!=PBWeather::HARSHSUN
		  return (int)Math.Round(damagemult*0.5f)
		}
		return damagemult
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (this.immediate || attacker.effects.TwoTurnAttack>0){
		  pbShowAnimation(this.id, attacker, opponent,1, alltargets, showanimation) // Charging anim
		  //battle.pbDisplay(_INTL("{1} took in sunlight!",attacker.pbThis))
		}
		if (this.immediate && !this.sunny){
		  this.battle.pbCommonAnimation("UseItem", attacker, null)

		  //battle.pbDisplay(_INTL("{1} became fully charged due to its Power Herb!",attacker.pbThis))
		  attacker.pbConsumeItem
		}
		if (attacker.effects.TwoTurnAttack>0) return 0;
		return super(attacker, opponent, hitnum, alltargets, showanimation)
	  }
	}




	/// <summary>
	/// Two turn attack. Skips first turn, attacks second turn. (Freeze Shock)
	/// May paralyze the target.
	/// <summary>
	public class PokeBattle_Move_0C5 : PokeBattle_Move
	{
		public object pbTwoTurnAttack(Battle.Battler attacker)
		this.immediate = false
		if (!this.immediate && attacker.hasWorkingItem(Items.POWERHERB)){
		  this.immediate=true
		}
		if (this.immediate) return false;
		return attacker.effects.TwoTurnAttack==0
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (this.immediate || attacker.effects.TwoTurnAttack>0){
		  pbShowAnimation(this.id, attacker, opponent,1, alltargets, showanimation) // Charging anim
		  //battle.pbDisplay(_INTL("{1} became cloaked in a freezing light!",attacker.pbThis))
		}
		if (this.immediate){
		  this.battle.pbCommonAnimation("UseItem", attacker, null)

		  //battle.pbDisplay(_INTL("{1} became fully charged due to its Power Herb!",attacker.pbThis))
		  attacker.pbConsumeItem
		}
		if (attacker.effects.TwoTurnAttack>0) return 0;
		return super(attacker, opponent, hitnum, alltargets, showanimation)
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		if (opponent.pbCanParalyze? (attacker,false,self)){
		  opponent.pbParalyze(attacker)
		}
	  }
	}



	/// <summary>
	/// Two turn attack. Skips first turn, attacks second turn. (Ice Burn)
	/// May burn the target.
	/// <summary>
	public class PokeBattle_Move_0C6 : PokeBattle_Move
	{
		public object pbTwoTurnAttack(Battle.Battler attacker)
		this.immediate = false
		if (!this.immediate && attacker.hasWorkingItem(Items.POWERHERB)){
		  this.immediate=true
		}
		if (this.immediate) return false;
		return attacker.effects.TwoTurnAttack==0
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (this.immediate || attacker.effects.TwoTurnAttack>0){
		  pbShowAnimation(this.id, attacker, opponent,1, alltargets, showanimation) // Charging anim
		  //battle.pbDisplay(_INTL("{1} became cloaked in freezing air!",attacker.pbThis))
		}
		if (this.immediate){
		  this.battle.pbCommonAnimation("UseItem", attacker, null)

		  //battle.pbDisplay(_INTL("{1} became fully charged due to its Power Herb!",attacker.pbThis))
		  attacker.pbConsumeItem
		}
		if (attacker.effects.TwoTurnAttack>0) return 0;
		return super(attacker, opponent, hitnum, alltargets, showanimation)
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		if (opponent.pbCanBurn? (attacker,false,self)){
		  opponent.pbBurn(attacker)
		}
	  }
	}



	/// <summary>
	/// Two turn attack. Skips first turn, attacks second turn. (Sky Attack)
	/// May make the target flinch.
	/// <summary>
	public class PokeBattle_Move_0C7 : PokeBattle_Move
	{
		public object pbTwoTurnAttack(Battle.Battler attacker)
		this.immediate = false
		if (!this.immediate && attacker.hasWorkingItem(Items.POWERHERB)){
		  this.immediate=true
		}
		if (this.immediate) return false;
		return attacker.effects.TwoTurnAttack==0
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (this.immediate || attacker.effects.TwoTurnAttack>0){
		  pbShowAnimation(this.id, attacker, opponent,1, alltargets, showanimation) // Charging anim
		  //battle.pbDisplay(_INTL("{1} became cloaked in a harsh light!",attacker.pbThis))
		}
		if (this.immediate){
		  this.battle.pbCommonAnimation("UseItem", attacker, null)

		  //battle.pbDisplay(_INTL("{1} became fully charged due to its Power Herb!",attacker.pbThis))
		  attacker.pbConsumeItem
		}
		if (attacker.effects.TwoTurnAttack>0) return 0;
		return super(attacker, opponent, hitnum, alltargets, showanimation)
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		opponent.pbFlinch(attacker)
	  }
	}



	/// <summary>
	/// Two turn attack. Ups user's Defense by 1 stage first turn, attacks second turn.
	/// (Skull Bash)
	/// <summary>
	public class PokeBattle_Move_0C8 : PokeBattle_Move
	{
		public object pbTwoTurnAttack(Battle.Battler attacker)
		this.immediate = false
		if (!this.immediate && attacker.hasWorkingItem(Items.POWERHERB)){
		  this.immediate=true
		}
		if (this.immediate) return false;
		return attacker.effects.TwoTurnAttack==0
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (this.immediate || attacker.effects.TwoTurnAttack>0){
		  pbShowAnimation(this.id, attacker, opponent,1, alltargets, showanimation) // Charging anim
		  //battle.pbDisplay(_INTL("{1} tucked in its head!",attacker.pbThis))
		  if (attacker.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,false,self)){
			attacker.pbIncreaseStat(PBStats::DEFENSE,1,attacker,false,self)
		  }
		}
		if (this.immediate){
		  this.battle.pbCommonAnimation("UseItem", attacker, null)

		  //battle.pbDisplay(_INTL("{1} became fully charged due to its Power Herb!",attacker.pbThis))
		  attacker.pbConsumeItem
		}
		if (attacker.effects.TwoTurnAttack>0) return 0;
		return super(attacker, opponent, hitnum, alltargets, showanimation)
	  }
	}



	/// <summary>
	/// Two turn attack. Skips first turn, attacks second turn. (Fly)
	/// (Handled in Battler's pbSuccessCheck): Is semi-invulnerable during use.
	/// <summary>
	public class PokeBattle_Move_0C9 : PokeBattle_Move
	{
		public object unusableInGravity?
		return true
	  }

	  public object pbTwoTurnAttack(Battle.Battler attacker)
		this.immediate = false
		if (!this.immediate && attacker.hasWorkingItem(Items.POWERHERB)){
		  this.immediate=true
		}
		if (this.immediate) return false;
		return attacker.effects.TwoTurnAttack==0
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (this.immediate || attacker.effects.TwoTurnAttack>0){
		  pbShowAnimation(this.id, attacker, opponent,1, alltargets, showanimation) // Charging anim
		  //battle.pbDisplay(_INTL("{1} flew up high!",attacker.pbThis))
		}
		if (this.immediate){
		  this.battle.pbCommonAnimation("UseItem", attacker, null)

		  //battle.pbDisplay(_INTL("{1} became fully charged due to its Power Herb!",attacker.pbThis))
		  attacker.pbConsumeItem
		}
		if (attacker.effects.TwoTurnAttack>0) return 0;
		return super(attacker, opponent, hitnum, alltargets, showanimation)
	  }
	}



	/// <summary>
	/// Two turn attack. Skips first turn, attacks second turn. (Dig)
	/// (Handled in Battler's pbSuccessCheck): Is semi-invulnerable during use.
	/// <summary>
	public class PokeBattle_Move_0CA : PokeBattle_Move
	{
		public object pbTwoTurnAttack(Battle.Battler attacker)
		this.immediate = false
		if (!this.immediate && attacker.hasWorkingItem(Items.POWERHERB)){
		  this.immediate=true
		}
		if (this.immediate) return false;
		return attacker.effects.TwoTurnAttack==0
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (this.immediate || attacker.effects.TwoTurnAttack>0){
		  pbShowAnimation(this.id, attacker, opponent,1, alltargets, showanimation) // Charging anim
		  //battle.pbDisplay(_INTL("{1} burrowed its way under the ground!",attacker.pbThis))
		}
		if (this.immediate){
		  this.battle.pbCommonAnimation("UseItem", attacker, null)

		  //battle.pbDisplay(_INTL("{1} became fully charged due to its Power Herb!",attacker.pbThis))
		  attacker.pbConsumeItem
		}
		if (attacker.effects.TwoTurnAttack>0) return 0;
		return super(attacker, opponent, hitnum, alltargets, showanimation)
	  }
	}



	/// <summary>
	/// Two turn attack. Skips first turn, attacks second turn. (Dive)
	/// (Handled in Battler's pbSuccessCheck): Is semi-invulnerable during use.
	/// <summary>
	public class PokeBattle_Move_0CB : PokeBattle_Move
	{
		public object pbTwoTurnAttack(Battle.Battler attacker)
		this.immediate = false
		if (!this.immediate && attacker.hasWorkingItem(Items.POWERHERB)){
		  this.immediate=true
		}
		if (this.immediate) return false;
		return attacker.effects.TwoTurnAttack==0
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (this.immediate || attacker.effects.TwoTurnAttack>0){
		  pbShowAnimation(this.id, attacker, opponent,1, alltargets, showanimation) // Charging anim
		  //battle.pbDisplay(_INTL("{1} hid underwater!",attacker.pbThis))
		}
		if (this.immediate){
		  this.battle.pbCommonAnimation("UseItem", attacker, null)

		  //battle.pbDisplay(_INTL("{1} became fully charged due to its Power Herb!",attacker.pbThis))
		  attacker.pbConsumeItem
		}
		if (attacker.effects.TwoTurnAttack>0) return 0;
		return super(attacker, opponent, hitnum, alltargets, showanimation)
	  }
	}



	/// <summary>
	/// Two turn attack. Skips first turn, attacks second turn. (Bounce)
	/// May paralyze the target.
	/// (Handled in Battler's pbSuccessCheck): Is semi-invulnerable during use.
	/// <summary>
	public class PokeBattle_Move_0CC : PokeBattle_Move
	{
		public object unusableInGravity?
		return true
	  }

	  public object pbTwoTurnAttack(Battle.Battler attacker)
		this.immediate = false
		if (!this.immediate && attacker.hasWorkingItem(Items.POWERHERB)){
		  this.immediate=true
		}
		if (this.immediate) return false;
		return attacker.effects.TwoTurnAttack==0
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (this.immediate || attacker.effects.TwoTurnAttack>0){
		  pbShowAnimation(this.id, attacker, opponent,1, alltargets, showanimation) // Charging anim
		  //battle.pbDisplay(_INTL("{1} sprang up!",attacker.pbThis))
		}
		if (this.immediate){
		  this.battle.pbCommonAnimation("UseItem", attacker, null)

		  //battle.pbDisplay(_INTL("{1} became fully charged due to its Power Herb!",attacker.pbThis))
		  attacker.pbConsumeItem
		}
		if (attacker.effects.TwoTurnAttack>0) return 0;
		return super(attacker, opponent, hitnum, alltargets, showanimation)
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		if (opponent.pbCanParalyze? (attacker,false,self)){
		  opponent.pbParalyze(attacker)
		}
	  }
	}



	/// <summary>
	/// Two turn attack. Skips first turn, attacks second turn. (Shadow Force)
	/// Is invulnerable during use.
	/// Ignores target's Detect, King's Shield, Mat Block, Protect and Spiky Shield
	/// this round. If successful, negates them this round.
	/// <summary>
	public class PokeBattle_Move_0CD : PokeBattle_Move
	{
		public object pbTwoTurnAttack(Battle.Battler attacker)
		this.immediate = false
		if (!this.immediate && attacker.hasWorkingItem(Items.POWERHERB)){
		  this.immediate=true
		}
		if (this.immediate) return false;
		return attacker.effects.TwoTurnAttack==0
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (this.immediate || attacker.effects.TwoTurnAttack>0){
		  pbShowAnimation(this.id, attacker, opponent,1, alltargets, showanimation) // Charging anim
		  //battle.pbDisplay(_INTL("{1} vanished instantly!",attacker.pbThis))
		}
		if (this.immediate){
		  this.battle.pbCommonAnimation("UseItem", attacker, null)

		  //battle.pbDisplay(_INTL("{1} became fully charged due to its Power Herb!",attacker.pbThis))
		  attacker.pbConsumeItem
		}
		if (attacker.effects.TwoTurnAttack>0) return 0;
		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if (ret>0){
		  opponent.effects.ProtectNegation=true
		  opponent.OwnSide.CraftyShield=false
		}
		return ret
	  }
	}



	/// <summary>
	/// Two turn attack. Skips first turn, attacks second turn. (Sky Drop)
	/// (Handled in Battler's pbSuccessCheck):  Is semi-invulnerable during use.
	/// Target is also semi-invulnerable during use, and can't take any action.
	/// Doesn't damage airborne Pokémon (but still makes them unable to move during).
	/// <summary>
	public class PokeBattle_Move_0CE : PokeBattle_Move
	{
		public object unusableInGravity?
		return true
	  }

	  public object pbMoveFailed(Battle.Battler attacker, Battle.Battler opponent){
		ret = false

		ret=true if opponent.effects.Substitute>0 && !ignoresSubstitute? (attacker)
		 ret = true if opponent.effects.TwoTurnAttack>0
		ret=true if opponent.effects.SkyDrop && attacker.effects.TwoTurnAttack>0
		ret=true if !opponent.pbIsOpposing? (attacker.index)
		 ret = true if USENEWBATTLEMECHANICS && opponent.weight(attacker)>=2000
		return ret
	  }

	  public object pbTwoTurnAttack(Battle.Battler attacker){
		return attacker.effects.TwoTurnAttack==0
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (attacker.effects.TwoTurnAttack>0){
		  pbShowAnimation(this.id, attacker, opponent,1, alltargets, showanimation) // Charging anim
		  //battle.pbDisplay(_INTL("{1} took {2} into the sky!",attacker.pbThis,opponent.pbThis(true)))
		  opponent.effects.SkyDrop=true
		}
		if (attacker.effects.TwoTurnAttack>0) return 0;
		ret=super
		//battle.pbDisplay(_INTL("{1} was freed from the Sky Drop!", opponent.pbThis))
		opponent.effects.SkyDrop=false
		return ret
	  }

	  public object pbTypeModifier(type, Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.hasType(Types.FLYING)) return 0;
		if (!attacker.hasMoldBreaker &&
		   opponent.hasWorkingAbility(Abilities.LEVITATE) && 
		   !opponent.effects.SmackDown) return 0;
		return super
	  }
	}



	/// <summary>
	/// Trapping move. Traps for 5 or 6 rounds. Trapped Pokémon lose 1/16 of max HP
	/// at end of each round.
	/// <summary>
	public class PokeBattle_Move_0CF : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if (opponent.damagestate.CalcDamage>0 && !opponent.isFainted() &&){
		   !opponent.damagestate.Substitute
		  if (opponent.effects.MultiTurn==0){
			opponent.effects.MultiTurn=5+this.battle.pbRandom(2)
			if (attacker.hasWorkingItem(Items.GRIPCLAW)){
			  opponent.effects.MultiTurn=(USENEWBATTLEMECHANICS)? 8 : 6
			}
			opponent.effects.MultiTurnAttack= this.id

			opponent.effects.MultiTurnUser= attacker.index
			if (id == Moves.BIND){

			  //battle.pbDisplay(_INTL("{1} was squeezed by {2}!",opponent.pbThis,attacker.pbThis(true)))
			else if id == Moves.CLAMP
			  //battle.pbDisplay(_INTL("{1} clamped {2}!",attacker.pbThis,opponent.pbThis(true)))
			else if id == Moves.FIRESPIN
			  //battle.pbDisplay(_INTL("{1} was trapped in the fiery vortex!",opponent.pbThis))
			else if id == Moves.MAGMASTORM
			  //battle.pbDisplay(_INTL("{1} became trapped by Magma Storm!",opponent.pbThis))
			else if id == Moves.SANDTOMB
			  //battle.pbDisplay(_INTL("{1} became trapped by Sand Tomb!",opponent.pbThis))
			else if id == Moves.WRAP
			  //battle.pbDisplay(_INTL("{1} was wrapped by {2}!",opponent.pbThis,attacker.pbThis(true)))
			else if id == Moves.INFESTATION
			  //battle.pbDisplay(_INTL("{1} has been afflicted with an infestation by {2}!",opponent.pbThis,attacker.pbThis(true)))
			else
			  //battle.pbDisplay(_INTL("{1} was trapped in the vortex!",opponent.pbThis))
			}
		  }

		}
		return ret
	  }
	}



	/// <summary>
	/// Trapping move. Traps for 5 or 6 rounds. Trapped Pokémon lose 1/16 of max HP
	/// at end of each round. (Whirlpool)
	/// Power is doubled if target is using Dive.
	/// (Handled in Battler's pbSuccessCheck): Hits some semi-invulnerable targets.
	/// <summary>
	public class PokeBattle_Move_0D0 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if (opponent.damagestate.CalcDamage>0 && !opponent.isFainted() &&){
		   !opponent.damagestate.Substitute
		  if (opponent.effects.MultiTurn==0){
			opponent.effects.MultiTurn=5+this.battle.pbRandom(2)
			if (attacker.hasWorkingItem(Items.GRIPCLAW)){
			  opponent.effects.MultiTurn=(USENEWBATTLEMECHANICS)? 8 : 6
			}
			opponent.effects.MultiTurnAttack= this.id

			opponent.effects.MultiTurnUser= attacker.index

			//battle.pbDisplay(_INTL("{1} became trapped in the vortex!", opponent.pbThis))
		  }
		}
		return ret
	  }

	  public object pbModifyDamage(damagemult, Battle.Battler attacker, Battle.Battler opponent){
		if (PBMoveData.new(opponent.effects.TwoTurnAttack).function==0xCB // Dive){
		  return (int)Math.Round(damagemult*2.0f)
		}
		return damagemult
	  }
	}



	/// <summary>
	/// User must use this move for 2 more rounds. No battlers can sleep. (Uproar)
	/// <summary>
	public class PokeBattle_Move_0D1 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if (opponent.damagestate.CalcDamage>0){
		  if (attacker.effects.Uproar==0){
			attacker.effects.Uproar=3
			//battle.pbDisplay(_INTL("{1} caused an uproar!",attacker.pbThis))
			attacker.currentMove=this.id
		  }

		}
		return ret
	  }
	}



	/// <summary>
	/// User must use this move for 1 or 2 more rounds. At end, user becomes confused.
	/// (Outrage, Petal Dange, Thrash)
	/// <summary>
	public class PokeBattle_Move_0D2 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if (opponent.damagestate.CalcDamage>0 &&){
		   attacker.effects.Outrage==0 && 
		   attacker.status!=PBStatuses::SLEEP
		  attacker.effects.Outrage= 2 + this.battle.pbRandom(2)

		  attacker.currentMove= this.id

		else if pbTypeModifier(this.type, attacker, opponent)==0
		  // Cancel effect if attack is ineffective
		  attacker.effects.Outrage= 0

		}
		if (attacker.effects.Outrage>0){

		  attacker.effects.Outrage-=1
		  if (attacker.effects.Outrage==0 && attacker.pbCanConfuseSelf?(false)){

			attacker.pbConfuse
			//battle.pbDisplay(_INTL("{1} became confused due to fatigue!", attacker.pbThis))
		  }
		}
		return ret
	  }
	}



	/// <summary>
	/// User must use this move for 4 more rounds. Power doubles each round.
	/// Power is also doubled if user has curled up. (Ice Ball, Rollout)
	/// <summary>
	public class PokeBattle_Move_0D3 : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){
		shift = (4 - attacker.effects.Rollout) // from 0 through 4, 0 is most powerful
		shift+=1 if attacker.effects.DefenseCurl
		basedmg = basedmg << shift
		return basedmg
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){

		attacker.effects.Rollout=5 if attacker.effects.Rollout==0
		attacker.effects.Rollout-=1
		attacker.currentMove=thismove.id
		ret = super(attacker, opponent, hitnum, alltargets, showanimation)
		if (opponent.damagestate.CalcDamage==0 ||){
		   pbTypeModifier(this.type, attacker, opponent)==0 || 
		   attacker.status==PBStatuses::SLEEP
	/// Cancel effect if attack is ineffective
		  attacker.effects.Rollout= 0

		}
		return ret
	  }
	}



	/// <summary>
	/// User bides its time this round and next round. The round after, deals 2x the
	/// total damage it took while biding to the last battler that damaged it. (Bide)
	/// <summary>
	public class PokeBattle_Move_0D4 : PokeBattle_Move
	{
		public object pbDisplayUseMessage(Battle.Battler attacker){
		if (attacker.effects.Bide==0){
		  //battle.pbDisplayBrief(_INTL("{1} used\r\n{2}!",attacker.pbThis,name))
		  attacker.effects.Bide=2
		  attacker.effects.BideDamage=0
		  attacker.effects.BideTarget=-1
		  attacker.currentMove=this.id
		  pbShowAnimation(this.id, attacker, null)
		  return 1
		else
		  attacker.effects.Bide-=1
		  if (attacker.effects.Bide==0){
			//battle.pbDisplayBrief(_INTL("{1} unleashed energy!",attacker.pbThis))
			return 0
		  else
			//battle.pbDisplayBrief(_INTL("{1} is storing energy!",attacker.pbThis))
			return 2
		  }
		}
	  }

	  public object pbAddTarget(targets, Battle.Battler attacker){
		if (attacker.effects.BideTarget>=0){
		  if (!attacker.pbAddTarget(targets, this.battle.battlers[attacker.effects.BideTarget])){
			attacker.pbRandomTarget(targets)

		  }
		else
		  attacker.pbRandomTarget(targets)
		}
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (attacker.effects.BideDamage==0 || !opponent){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if (USENEWBATTLEMECHANICS){
		  typemod = pbTypeModifier(pbType(this.type, attacker, opponent), attacker, opponent)
		  if (typemod==0){
			//battle.pbDisplay(_INTL("It doesn't affect {1}...",opponent.pbThis(true)))
			return -1
		  }
		}

		ret=pbEffectFixedDamage(attacker.effects.BideDamage*2, attacker, opponent, hitnum, alltargets, showanimation)
		return ret
	  }
	}



	/// <summary>
	/// Heals user by 1/2 of its max HP.
	/// <summary>
	public class PokeBattle_Move_0D5 : PokeBattle_Move
	{
		public object isHealingMove?
		return true
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (attacker.hp==attacker.TotalHP){
		  //battle.pbDisplay(_INTL("{1}'s HP is full!", attacker.pbThis))
		  return -1
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		attacker.pbRecoverHP(((attacker.TotalHP+1)/2).floor,true)
		//battle.pbDisplay(_INTL("{1}'s HP was restored.",attacker.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// Heals user by 1/2 of its max HP. (Roost)
	/// User roosts, and its Flying type is ignored for attacks used against it.
	/// <summary>
	public class PokeBattle_Move_0D6 : PokeBattle_Move
	{
		public object isHealingMove?
		return true
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (attacker.hp==attacker.TotalHP){
		  //battle.pbDisplay(_INTL("{1}'s HP is full!", attacker.pbThis))
		  return -1
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		attacker.pbRecoverHP(((attacker.TotalHP+1)/2).floor,true)
		attacker.effects.Roost=true
		//battle.pbDisplay(_INTL("{1}'s HP was restored.",attacker.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// Battler in user's position is healed by 1/2 of its max HP, at the end of the
	/// next round. (Wish)
	/// <summary>
	public class PokeBattle_Move_0D7 : PokeBattle_Move
	{
		public object isHealingMove?
		return true
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (attacker.effects.Wish>0){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		attacker.effects.Wish=2
		attacker.effects.WishAmount=((attacker.TotalHP+1)/2).floor
		attacker.effects.WishMaker= attacker.pokemonIndex
		return 0
	  }
	}



	/// <summary>
	/// Heals user by an amount depending on the weather. (Moonlight, Morning Sun,
	/// Synthesis)
	/// <summary>
	public class PokeBattle_Move_0D8 : PokeBattle_Move
	{
		public object isHealingMove?
		return true
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (attacker.hp==attacker.TotalHP){
		  //battle.pbDisplay(_INTL("{1}'s HP is full!", attacker.pbThis))
		  return -1
		}
		hpgain = 0
		if (this.battle.pbWeather==PBWeather::SUNNYDAY ||){
		   this.battle.pbWeather==PBWeather::HARSHSUN
		  hpgain = (attacker.TotalHP * 2 / 3).floor

		else if this.battle.pbWeather!=0
		  hpgain= (attacker.TotalHP / 4).floor
		else

		  hpgain= (attacker.TotalHP / 2).floor

		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)
		attacker.pbRecoverHP(hpgain,true)

		//battle.pbDisplay(_INTL("{1}'s HP was restored.",attacker.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// Heals user to full HP. User falls asleep for 2 more rounds. (Rest)
	/// <summary>
	public class PokeBattle_Move_0D9 : PokeBattle_Move
	{
		public object isHealingMove?
		return true
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (!attacker.pbCanSleep? (attacker,true,self,true)){
		  return -1
		}
		if (attacker.status==PBStatuses::SLEEP){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if (attacker.hp==attacker.TotalHP){
		  //battle.pbDisplay(_INTL("{1}'s HP is full!", attacker.pbThis))
		  return -1
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		attacker.pbSleepSelf(3)
		//battle.pbDisplay(_INTL("{1} slept and became healthy!",attacker.pbThis))
		hp=attacker.pbRecoverHP(attacker.TotalHP-attacker.hp,true)
		//battle.pbDisplay(_INTL("{1}'s HP was restored.",attacker.pbThis)) if hp>0
		return 0
	  }
	}



	/// <summary>
	/// Rings the user. Ringed Pokémon gain 1/16 of max HP at the end of each round.
	/// (Aqua Ring)
	/// <summary>
	public class PokeBattle_Move_0DA : PokeBattle_Move
	{
		public object isHealingMove?
		return true
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (attacker.effects.AquaRing){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		attacker.effects.AquaRing=true
		//battle.pbDisplay(_INTL("{1} surrounded itself with a veil of water!",attacker.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// Ingrains the user. Ingrained Pokémon gain 1/16 of max HP at the end of each
	/// round, and cannot flee or switch out. (Ingrain)
	/// <summary>
	public class PokeBattle_Move_0DB : PokeBattle_Move
	{
		public object isHealingMove?
		return true
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (attacker.effects.Ingrain){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		attacker.effects.Ingrain=true
		//battle.pbDisplay(_INTL("{1} planted its roots!",attacker.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// Seeds the target. Seeded Pokémon lose 1/8 of max HP at the end of each round,
	/// and the Pokémon in the user's position gains the same amount. (Leech Seed)
	/// <summary>
	public class PokeBattle_Move_0DC : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (opponent.effects.Substitute>0 && !ignoresSubstitute? (attacker)){
		   //battle.pbDisplay(_INTL("But it failed!"))  
		  return -1
		}
		if (pbTypeImmunityByAbility(pbType(this.type, attacker, opponent), attacker, opponent)) return -1;
		if (opponent.effects.LeechSeed>=0){
		  //battle.pbDisplay(_INTL("{1} evaded the attack!",opponent.pbThis))
		  return -1
		}
		if (opponent.hasType(Types.GRASS)){
		  //battle.pbDisplay(_INTL("It doesn't affect {1}...",opponent.pbThis(true)))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.effects.LeechSeed=attacker.index
		//battle.pbDisplay(_INTL("{1} was seeded!", opponent.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// User gains half the HP it inflicts as damage.
	/// <summary>
	public class PokeBattle_Move_0DD : PokeBattle_Move
	{
		public object isHealingMove?
		return USENEWBATTLEMECHANICS
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if (opponent.damagestate.CalcDamage>0){
		  hpgain= (int)Math.Round(opponent.damagestate.HpLost/2)
		  if (opponent.hasWorkingAbility(Abilities.LIQUIDOOZE)){
			attacker.pbReduceHP(hpgain,true)
			//battle.pbDisplay(_INTL("{1} sucked up the liquid ooze!",attacker.pbThis))
		  else if attacker.effects.HealBlock==0

			hpgain= (hpgain * 1.3f).floor if attacker.hasWorkingItem(Items.BIGROOT)

			attacker.pbRecoverHP(hpgain,true)
			//battle.pbDisplay(_INTL("{1} had its energy drained!",opponent.pbThis))
		  }
		}
		return ret
	  }
	}



	/// <summary>
	/// User gains half the HP it inflicts as damage. (Dream Eater)
	/// (Handled in Battler's pbSuccessCheck): Fails if target is not asleep.
	/// <summary>
	public class PokeBattle_Move_0DE : PokeBattle_Move
	{
		public object isHealingMove?
		return USENEWBATTLEMECHANICS
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if (opponent.damagestate.CalcDamage>0){
		  hpgain= (int)Math.Round(opponent.damagestate.HpLost/2)
		  if (opponent.hasWorkingAbility(Abilities.LIQUIDOOZE)){
			attacker.pbReduceHP(hpgain,true)
			//battle.pbDisplay(_INTL("{1} sucked up the liquid ooze!",attacker.pbThis))
		  else if attacker.effects.HealBlock==0

			hpgain= (hpgain * 1.3f).floor if attacker.hasWorkingItem(Items.BIGROOT)

			attacker.pbRecoverHP(hpgain,true)
			//battle.pbDisplay(_INTL("{1} had its energy drained!",opponent.pbThis))
		  }
		}
		return ret
	  }
	}



	/// <summary>
	/// Heals target by 1/2 of its max HP. (Heal Pulse)
	/// <summary>
	public class PokeBattle_Move_0DF : PokeBattle_Move
	{
		public object isHealingMove?
		return true
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (opponent.effects.Substitute>0 && !ignoresSubstitute? (attacker)){
		   //battle.pbDisplay(_INTL("But it failed!"))  
		  return -1
		}
		if (opponent.hp==opponent.TotalHP){
		  //battle.pbDisplay(_INTL("{1}'s HP is full!", opponent.pbThis))  
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		hpgain=((opponent.TotalHP+1)/2).floor
		if (attacker.hasWorkingAbility(Abilities.MEGALAUNCHER))hpgain = (int)Math.Round(opponent.TotalHP * 3 / 4) 
		opponent.pbRecoverHP(hpgain,true)
		//battle.pbDisplay(_INTL("{1}'s HP was restored.",opponent.pbThis))  
		return 0
	  }
	}



	/// <summary>
	/// User faints. (Explosion, Selfdestruct)
	/// <summary>
	public class PokeBattle_Move_0E0 : PokeBattle_Move
	{
		public object pbOnStartUse(Battle.Battler attacker){
		if (!attacker.hasMoldBreaker){
		  bearer = this.battle.pbCheckGlobalAbility(:DAMP)
		  if (bearer!=null){
			//battle.pbDisplay(_INTL("{1}'s {2} prevents {3} from using {4}!",
			   bearer.pbThis, PBAbilities.getName(bearer.ability), attacker.pbThis(true),this.name))
			return false
		  }
		}
		return true
	  }

	  public object pbShowAnimation(id, Battle.Battler attacker, Battle.Battler opponent, byte hitnum= 0, byte? alltargets= null, bool showanimation= true){

		super(id, attacker, opponent, hitnum, alltargets, showanimation)
		if (!attacker.isFainted()){
		  attacker.pbReduceHP(attacker.hp)

		  attacker.pbFaint if attacker.isFainted()
		}
	  }
	}



	/// <summary>
	/// Inflicts fixed damage equal to user's current HP. (Final Gambit)
	/// User faints (if successful).
	/// <summary>
	public class PokeBattle_Move_0E1 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		typemod=pbTypeModifier(pbType(this.type, attacker, opponent), attacker, opponent)
		if (typemod==0){
		  //battle.pbDisplay(_INTL("It doesn't affect {1}...",opponent.pbThis(true)))
		  return -1
		}
		ret = pbEffectFixedDamage(attacker.hp, attacker, opponent, hitnum, alltargets, showanimation)
		return ret
	  }

	  public object pbShowAnimation(id, Battle.Battler attacker, Battle.Battler opponent, byte hitnum= 0, byte? alltargets= null, bool showanimation= true){

		super(id, attacker, opponent, hitnum, alltargets, showanimation)
		if (!attacker.isFainted()){
		  attacker.pbReduceHP(attacker.hp)

		  attacker.pbFaint if attacker.isFainted()
		}
	  }
	}



	/// <summary>
	/// Decreases the target's Attack and Special Attack by 2 stages each. (Memento)
	/// User faints (even if effect does nothing).
	/// <summary>
	public class PokeBattle_Move_0E2 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (opponent.effects.Substitute>0 && !ignoresSubstitute? (attacker)){
		   //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=-1; showanim=true
		if (opponent.pbReduceStat(PBStats::ATTACK,2,attacker,false,self,showanim)){
		  ret=0; showanim=false
		}
		if (opponent.pbReduceStat(PBStats::SPATK,2,attacker,false,self,showanim)){
		  ret=0; showanim=false
		}
		attacker.pbReduceHP(attacker.hp)
		return ret
	  }
	}



	/// <summary>
	/// User faints. The Pokémon that replaces the user is fully healed (HP and
	/// status). Fails if user won't be replaced. (Healing Wish)
	/// <summary>
	public class PokeBattle_Move_0E3 : PokeBattle_Move
	{
		public object isHealingMove?
		return true
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (!this.battle.pbCanChooseNonActive? (attacker.index)){
		   //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		attacker.pbReduceHP(attacker.hp)
		attacker.effects.HealingWish= true
		return 0
	  }
	}



	/// <summary>
	/// User faints. The Pokémon that replaces the user is fully healed (HP, PP and
	/// status). Fails if user won't be replaced. (Lunar Dance)
	/// <summary>
	public class PokeBattle_Move_0E4 : PokeBattle_Move
	{
		public object isHealingMove?
		return true
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (!this.battle.pbCanChooseNonActive? (attacker.index)){
		   //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		attacker.pbReduceHP(attacker.hp)
		attacker.effects.LunarDance= true
		return 0
	  }
	}



	/// <summary>
	/// All current battlers will perish after 3 more rounds. (Perish Song)
	/// <summary>
	public class PokeBattle_Move_0E5 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		failed=true
		for (int i = 0; i < 4; i++){ 
		  if (this.battle.battlers[i].effects.PerishSong==0 &&){
			 (attacker.hasMoldBreaker ||
			 !this.battle.battlers[i].hasWorkingAbility(Abilities.SOUNDPROOF))
			failed=false; break
		  }
		}
		if (failed){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		//battle.pbDisplay(_INTL("All Pokémon that hear the song will faint in three turns!"))
		for (int i = 0; i < 4; i++){ 
		  if (this.battle.battlers[i].effects.PerishSong==0){
			if (!attacker.hasMoldBreaker && this.battle.battlers[i].hasWorkingAbility(Abilities.SOUNDPROOF)){

			  //battle.pbDisplay(_INTL("{1}'s {2} blocks {3}!",this.battle.battlers[i].pbThis,
				 PBAbilities.getName(this.battle.battlers[i].ability),this.name))
			else
			  this.battle.battlers[i].effects.PerishSong=4
			  this.battle.battlers[i].effects.PerishSongUser=attacker.index
			}

		  }
		}
		return 0
	  }
	}



	/// <summary>
	/// If user is KO'd before it next moves, the attack that caused it loses all PP.
	/// (Grudge)
	/// <summary>
	public class PokeBattle_Move_0E6 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		attacker.effects.Grudge=true
		//battle.pbDisplay(_INTL("{1} wants its target to bear a grudge!",attacker.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// If user is KO'd before it next moves, the battler that caused it also faints.
	/// (Destiny Bond)
	/// <summary>
	public class PokeBattle_Move_0E7 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		attacker.effects.DestinyBond=true
		//battle.pbDisplay(_INTL("{1} is trying to take its foe down with it!",attacker.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// If user would be KO'd this round, it survives with 1HP instead. (Endure)
	/// <summary>
	public class PokeBattle_Move_0E8 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		ratesharers=[
		   0xAA,   // Detect, Protect
		   0xAB,   // Quick Guard
		   0xAC,   // Wide Guard
		   0xE8,   // Endure
		   0x14B,  // King's Shield
		   0x14C   // Spiky Shield
		]
		if (!ratesharers.include? (PBMoveData.new(attacker.lastMoveUsed).function)){
		  attacker.effects.ProtectRate=1
		}
		unmoved = false
		for poke in this.battle.battlers
		  if (poke.index==attacker.index) continue; //next
		  if (this.battle.choices[poke.index][0]==1 && // Chose a move){
			 !poke.hasMovedThisRound?
			unmoved = true; break
		  }
		}
		if (!unmoved ||){
		   this.battle.pbRandom(65536)>(65536/attacker.effects.ProtectRate).floor
		  attacker.effects.ProtectRate= 1

		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		attacker.effects.Endure=true
		attacker.effects.ProtectRate*=2
		//battle.pbDisplay(_INTL("{1} braced itself!",attacker.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// If target would be KO'd by this attack, it survives with 1HP instead. (False Swipe)
	/// <summary>
	public class PokeBattle_Move_0E9 : PokeBattle_Move
	/// Handled in superclass public object pbReduceHPDamage, do not edit!
	}



	/// <summary>
	/// User flees from battle. Fails in trainer battles. (Teleport)
	/// <summary>
	public class PokeBattle_Move_0EA : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (this.battle.opponent ||){
		   !this.battle.pbCanRun? (attacker.index)
		   //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		//battle.pbDisplay(_INTL("{1} fled from battle!",attacker.pbThis))
		this.battle.decision=3
		return 0
	  }
	}



	/// <summary>
	/// In wild battles, makes target flee. Fails if target is a higher level than the
	/// user.
	/// In trainer battles, target switches out.
	/// For status moves. (Roar, Whirlwind)
	/// <summary>
	public class PokeBattle_Move_0EB : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (!attacker.hasMoldBreaker && opponent.hasWorkingAbility(Abilities.SUCTIONCUPS)){
		  //battle.pbDisplay(_INTL("{1} anchored itself with {2}!",opponent.pbThis,PBAbilities.getName(opponent.ability)))  
		  return -1
		}
		if (opponent.effects.Ingrain){
		  //battle.pbDisplay(_INTL("{1} anchored itself with its roots!", opponent.pbThis))  
		  return -1
		}
		if (!this.battle.opponent){
		  if (opponent.level>attacker.level){
			//battle.pbDisplay(_INTL("But it failed!"))
			return -1
		  }
		  pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		  this.battle.decision=3 // Set decision to escaped
		  return 0
		else
		  choices=false
		  party=this.battle.pbParty(opponent.index)
		  for (int i = 0; i < party.length; i++){ 
			if (this.battle.pbCanSwitch? (opponent.index, i,false,true)){
			  choices=true
			  break
			}
		  }
		  if (!choices){
			//battle.pbDisplay(_INTL("But it failed!"))
			return -1
		  }
		  pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		  opponent.effects.Roar=true
		  return 0
		}
	  }
	}



	/// <summary>
	/// In wild battles, makes target flee. Fails if target is a higher level than the
	/// user.
	/// In trainer battles, target switches out.
	/// For damaging moves. (Circle Throw, Dragon Tail)
	/// <summary>
	public class PokeBattle_Move_0EC : PokeBattle_Move
	{
		public object pbEffectAfterHit(Battle.Battler attacker, Battle.Battler opponent, turneffects){
		if (!attacker.isFainted() && !opponent.isFainted() &&){
		   opponent.damagestate.CalcDamage>0 && !opponent.damagestate.Substitute && 
		   (attacker.hasMoldBreaker || !opponent.hasWorkingAbility(Abilities.SUCTIONCUPS)) &&
		   !opponent.effects.Ingrain
		  if (!this.battle.opponent){
			if (opponent.level<=attacker.level){
			  this.battle.decision=3 // Set decision to escaped
			}
		  else

			party= this.battle.pbParty(opponent.index)
			for (int i = 0; i < arty.length; i++){ -1
			  if (this.battle.pbCanSwitch?(opponent.index, i,false)){

				opponent.effects.Roar=true
				break
			  }
			}

		  }
		}
	  }
	}



	/// <summary>
	/// User switches out. Various effects affecting the user are passed to the
	/// replacement. (Baton Pass)
	/// <summary>
	public class PokeBattle_Move_0ED : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (!this.battle.pbCanChooseNonActive? (attacker.index)){
		   //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		attacker.effects.BatonPass=true
		return 0
	  }
	}



	/// <summary>
	/// After inflicting damage, user switches out. Ignores trapping moves.
	/// (U-turn, Volt Switch)
	/// TODO: Pursuit should interrupt this move.
	/// <summary>
	public class PokeBattle_Move_0EE : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if (!attacker.isFainted() && opponent.damagestate.CalcDamage>0 &&){
		   this.battle.pbCanChooseNonActive? (attacker.index) &&
		   !this.battle.pbAllFainted? (this.battle.pbParty(opponent.index))
		  attacker.effects.Uturn=true
		}
		return ret
	  }
	}



	/// <summary>
	/// Target can no longer switch out or flee, as long as the user remains active.
	/// (Block, Mean Look, Spider Web, Thousand Waves)
	/// <summary>
	public class PokeBattle_Move_0EF : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (pbIsDamaging?){
		  ret = super(attacker, opponent, hitnum, alltargets, showanimation)
		  if (opponent.damagestate.CalcDamage>0 && !opponent.damagestate.Substitute &&){
			 !opponent.isFainted()
			if (opponent.effects.MeanLook<0 &&){
			   (!USENEWBATTLEMECHANICS || !opponent.hasType(Types.GHOST))
			  opponent.effects.MeanLook=attacker.index
			  //battle.pbDisplay(_INTL("{1} can no longer escape!", opponent.pbThis))
			}
		  }
		  return ret
		}
		if (opponent.effects.MeanLook>=0 ||){
		   (opponent.effects.Substitute>0 && !ignoresSubstitute? (attacker))
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if (USENEWBATTLEMECHANICS && opponent.hasType(Types.GHOST)){
		  //battle.pbDisplay(_INTL("It doesn't affect {1}...",opponent.pbThis(true)))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.effects.MeanLook=attacker.index
		//battle.pbDisplay(_INTL("{1} can no longer escape!", opponent.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// Target drops its item. It regains the item at the end of the battle. (Knock Off)
	/// If target has a losable item, damage is multiplied by 1.5.
	/// <summary>
	public class PokeBattle_Move_0F0 : PokeBattle_Move
	{
		public object pbEffectAfterHit(Battle.Battler attacker, Battle.Battler opponent, turneffects){
		if (!attacker.isFainted() && !opponent.isFainted() && opponent.item!=0 &&){
		   opponent.damagestate.CalcDamage>0 && !opponent.damagestate.Substitute
		  if (!attacker.hasMoldBreaker && opponent.hasWorkingAbility(Abilities.STICKYHOLD)){
			abilityname=PBAbilities.getName(opponent.ability)
			//battle.pbDisplay(_INTL("{1}'s {2} made {3} ineffective!", opponent.pbThis, abilityname, this.name))
		  else if !this.battle.pbIsUnlosableItem(opponent, opponent.item)
			itemname = PBItems.getName(opponent.item)

			opponent.item=0
			opponent.effects.ChoiceBand=-1
			opponent.effects.Unburden=true
			//battle.pbDisplay(_INTL("{1} dropped its {2}!",opponent.pbThis,itemname))
		  }
		}
	  }

	  public object pbModifyDamage(damagemult, Battle.Battler attacker, Battle.Battler opponent){
		if (USENEWBATTLEMECHANICS &&){
		   !this.battle.pbIsUnlosableItem(opponent, opponent.item)
		   // Still boosts damage even if opponent has Sticky Hold
		  return (int)Math.Round(damagemult*1.5f)
		}
		return damagemult
	  }
	}



	/// <summary>
	/// User steals the target's item, if the user has none itself. (Covet, Thief)
	/// Items stolen from wild Pokémon are kept after the battle.
	/// <summary>
	public class PokeBattle_Move_0F1 : PokeBattle_Move
	{
		public object pbEffectAfterHit(Battle.Battler attacker, Battle.Battler opponent, turneffects){
		if (!attacker.isFainted() && !opponent.isFainted() && opponent.item!=0 &&){
		   opponent.damagestate.CalcDamage>0 && !opponent.damagestate.Substitute
		  if (!attacker.hasMoldBreaker && opponent.hasWorkingAbility(Abilities.STICKYHOLD)){
			abilityname=PBAbilities.getName(opponent.ability)
			//battle.pbDisplay(_INTL("{1}'s {2} made {3} ineffective!", opponent.pbThis, abilityname, this.name))
		  else if !this.battle.pbIsUnlosableItem(opponent, opponent.item) &&
				!this.battle.pbIsUnlosableItem(attacker, opponent.item) &&
				attacker.item==0 &&
				(this.battle.opponent || !this.battle.pbIsOpposing? (attacker.index))
			itemname=PBItems.getName(opponent.item)
			attacker.item=opponent.item
			opponent.item= 0

			opponent.effects.ChoiceBand= -1

			opponent.effects.Unburden= true
			if (!this.battle.opponent && // In a wild battle){
			   attacker.pokemon.itemInitial==0 &&

			   opponent.pokemon.itemInitial==attacker.item
			  attacker.pokemon.itemInitial= attacker.item

			  opponent.pokemon.itemInitial= 0

			}
			//battle.pbDisplay(_INTL("{1} stole {2}'s {3}!", attacker.pbThis, opponent.pbThis(true),itemname))
		  }
		}
	  }
	}



	/// <summary>
	/// User and target swap items. They remain swapped after wild battles.
	/// (Switcheroo, Trick)
	/// <summary>
	public class PokeBattle_Move_0F2 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (opponent.effects.Substitute>0 && !ignoresSubstitute? (attacker)) ||
		   (attacker.item==0 && opponent.item==0) ||
		   (!this.battle.opponent && this.battle.pbIsOpposing? (attacker.index))
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if (this.battle.pbIsUnlosableItem(opponent, opponent.item) ||){
		   this.battle.pbIsUnlosableItem(attacker, opponent.item) ||
		   this.battle.pbIsUnlosableItem(opponent, attacker.item) ||
		   this.battle.pbIsUnlosableItem(attacker, attacker.item)
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if (!attacker.hasMoldBreaker && opponent.hasWorkingAbility(Abilities.STICKYHOLD)){
		  abilityname=PBAbilities.getName(opponent.ability)
		  //battle.pbDisplay(_INTL("{1}'s {2} made {3} ineffective!", opponent.pbThis, abilityname, name))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		oldattitem=attacker.item
		oldoppitem = opponent.item

		oldattitemname=PBItems.getName(oldattitem)
		oldoppitemname = PBItems.getName(oldoppitem)

		tmpitem=attacker.item
		attacker.item=opponent.item
		opponent.item= tmpitem
		if (!this.battle.opponent && // In a wild battle){
		   attacker.pokemon.itemInitial==oldattitem &&

		   opponent.pokemon.itemInitial==oldoppitem
		  attacker.pokemon.itemInitial= oldoppitem

		  opponent.pokemon.itemInitial= oldattitem

		}
		//battle.pbDisplay(_INTL("{1} switched items with its opponent!", attacker.pbThis))
		if (oldoppitem>0 && oldattitem>0){
		  //battle.pbDisplayPaused(_INTL("{1} obtained {2}.",attacker.pbThis,oldoppitemname))
		  //battle.pbDisplay(_INTL("{1} obtained {2}.",opponent.pbThis,oldattitemname))
		else
		  //battle.pbDisplay(_INTL("{1} obtained {2}.",attacker.pbThis,oldoppitemname)) if oldoppitem>0
		  //battle.pbDisplay(_INTL("{1} obtained {2}.",opponent.pbThis,oldattitemname)) if oldattitem>0
		}
		attacker.effects.ChoiceBand= -1

		opponent.effects.ChoiceBand= -1
		return 0
	  }
	}



	/// <summary>
	/// User gives its item to the target. The item remains given after wild battles.
	/// (Bestow)
	/// <summary>
	public class PokeBattle_Move_0F3 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (opponent.effects.Substitute>0 && !ignoresSubstitute? (attacker)) ||
		   attacker.item==0 || opponent.item!=0
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if (this.battle.pbIsUnlosableItem(attacker, attacker.item) ||){
		   this.battle.pbIsUnlosableItem(opponent, attacker.item)
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		itemname=PBItems.getName(attacker.item)
		opponent.item=attacker.item
		attacker.item= 0

		attacker.effects.ChoiceBand= -1

		attacker.effects.Unburden= true
		if (!this.battle.opponent && // In a wild battle){
		   opponent.pokemon.itemInitial==0 &&

		   attacker.pokemon.itemInitial==opponent.item
		  opponent.pokemon.itemInitial= opponent.item

		  attacker.pokemon.itemInitial= 0

		}
		//battle.pbDisplay(_INTL("{1} received {2} from {3}!", opponent.pbThis, itemname, attacker.pbThis(true)))
		return 0
	  }
	}



	/// <summary>
	/// User consumes target's berry and gains its effect. (Bug Bite, Pluck)
	/// <summary>
	public class PokeBattle_Move_0F4 : PokeBattle_Move
	{
		public object pbEffectAfterHit(Battle.Battler attacker, Battle.Battler opponent, turneffects){
		if (!attacker.isFainted() && !opponent.isFainted() && pbIsBerry? (opponent.item) &&){
		   opponent.damagestate.CalcDamage>0 && !opponent.damagestate.Substitute
		  if (attacker.hasMoldBreaker || !opponent.hasWorkingAbility(Abilities.STICKYHOLD)){
			item=opponent.item
			itemname = PBItems.getName(item)

			opponent.pbConsumeItem(false,false)
			//battle.pbDisplay(_INTL("{1} stole and ate its target's {2}!",attacker.pbThis,itemname))
			if (!attacker.hasWorkingAbility(Abilities.KLUTZ) &&){
			   attacker.effects.Embargo==0
			  attacker.pbActivateBerryEffect(item,false)
			}
			// Symbiosis
			if (attacker.item==0 &&){
			   attacker.pbPartner && attacker.pbPartner.hasWorkingAbility(Abilities.SYMBIOSIS)
			  partner=attacker.pbPartner
			  if (partner.item>0 &&){
				 !this.battle.pbIsUnlosableItem(partner, partner.item) &&
				 !this.battle.pbIsUnlosableItem(attacker, partner.item)
				//battle.pbDisplay(_INTL("{1}'s {2} let it share its {3} with {4}!",
				   partner.pbThis, PBAbilities.getName(partner.ability),
				   PBItems.getName(partner.item), attacker.pbThis(true)))
				attacker.item=partner.item
				partner.item=0
				partner.effects.Unburden= true

				attacker.pbBerryCureCheck
			  }

			}
		  }

		}
	  }
	}



	/// <summary>
	/// Target's berry is destroyed. (Incinerate)
	/// <summary>
	public class PokeBattle_Move_0F5 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if (!attacker.isFainted() && opponent.damagestate.CalcDamage>0 &&){
		   !opponent.damagestate.Substitute &&
		   (pbIsBerry?(opponent.item) || (USENEWBATTLEMECHANICS && pbIsGem? (opponent.item)))
		  itemname=PBItems.getName(opponent.item)
		  opponent.pbConsumeItem(false,false)

		  //battle.pbDisplay(_INTL("{1}'s {2} was incinerated!",opponent.pbThis,itemname))
		}
		return ret
	  }
	}



	/// <summary>
	/// User recovers the last item it held and consumed. (Recycle)
	/// <summary>
	public class PokeBattle_Move_0F6 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (!attacker.pokemon || attacker.pokemon.itemRecycle==0){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		item=attacker.pokemon.itemRecycle
		itemname = PBItems.getName(item)

		attacker.item=item
		if (!this.battle.opponent // In a wild battle){
		  attacker.pokemon.itemInitial=item if attacker.pokemon.itemInitial==0

		}
		attacker.pokemon.itemRecycle= 0

		attacker.effects.PickupItem= 0

		attacker.effects.PickupUse= 0

		//battle.pbDisplay(_INTL("{1} found one {2}!", attacker.pbThis, itemname))
		return 0
	  }
	}



	/// <summary>
	/// User flings its item at the target. Power and effect depend on the item. (Fling)
	/// <summary>
	public class PokeBattle_Move_0F7 : PokeBattle_Move
	{
		public object flingarray
		return { //keyvaluepair<item,byte>
		   130 => [:IRONBALL],
		   100 => [:ARMORFOSSIL,:CLAWFOSSIL,:COVERFOSSIL,:DOMEFOSSIL,:HARDSTONE,
				   :HELIXFOSSIL,:JAWFOSSIL,:OLDAMBER,:PLUMEFOSSIL,:RAREBONE,
				   :ROOTFOSSIL,:SAILFOSSIL,:SKULLFOSSIL],
			90 => [:DEEPSEATOOTH,:DRACOPLATE,:DREADPLATE,:EARTHPLATE,:FISTPLATE,
				   :FLAMEPLATE,:GRIPCLAW,:ICICLEPLATE,:INSECTPLATE,:IRONPLATE,
				   :MEADOWPLATE,:MINDPLATE,:PIXIEPLATE,:SKYPLATE,:SPLASHPLATE,
				   :SPOOKYPLATE,:STONEPLATE,:THICKCLUB,:TOXICPLATE,:ZAPPLATE],
			80 => [:ASSAULTVEST,:DAWNSTONE,:DUSKSTONE,:ELECTIRIZER,:MAGMARIZER,
				   :ODDKEYSTONE,:OVALSTONE,:PROTECTOR,:QUICKCLAW,:RAZORCLAW,
				   :SAFETYGOGGLES,:SHINYSTONE,:STICKYBARB,:WEAKNESSPOLICY],
			70 => [:BURNDRIVE,:CHILLDRIVE,:DOUSEDRIVE,:DRAGONFANG,:POISONBARB,
				   :POWERANKLET,:POWERBAND,:POWERBELT,:POWERBRACER,:POWERLENS,
				   :POWERWEIGHT,:SHOCKDRIVE],
			60 => [:ADAMANTORB,:DAMPROCK,:GRISEOUSORB,:HEATROCK,:LUSTROUSORB,
				   :MACHOBRACE,:ROCKYHELMET,:STICK],
			50 => [:DUBIOUSDISC,:SHARPBEAK],
			40 => [:EVIOLITE,:ICYROCK,:LUCKYPUNCH],
			30 => [:ABILITYCAPSULE,:ABILITYURGE,:ABSORBBULB,:AMAZEMULCH,:AMULETCOIN,
				   :ANTIDOTE,:AWAKENING,:BALMMUSHROOM,:BERRYJUICE,:BIGMUSHROOM,
				   :BIGNUGGET,:BIGPEARL,:BINDINGBAND,:BLACKBELT,:BLACKFLUTE,
				   :BLACKGLASSES,:BLACKSLUDGE,:BLUEFLUTE,:BLUESHARD,:BOOSTMULCH,
				   :BURNHEAL,:CALCIUM,:CARBOS,:CASTELIACONE,:CELLBATTERY,
				   :CHARCOAL,:CLEANSETAG,:COMETSHARD,:DAMPMULCH,:DEEPSEASCALE,
				   :DIREHIT,:DIREHIT2,:DIREHIT3,:DRAGONSCALE,:EJECTBUTTON,
				   :ELIXIR,:ENERGYPOWDER,:ENERGYROOT,:ESCAPEROPE,:ETHER,
				   :EVERSTONE,:EXPSHARE,:FIRESTONE,:FLAMEORB,:FLOATSTONE,
				   :FLUFFYTAIL,:FRESHWATER,:FULLHEAL,:FULLRESTORE,:GOOEYMULCH,
				   :GREENSHARD,:GROWTHMULCH,:GUARDSPEC,:HEALPOWDER,:HEARTSCALE,
				   :HONEY,:HPUP,:HYPERPOTION,:ICEHEAL,:IRON,
				   :ITEMDROP,:ITEMURGE,:KINGSROCK,:LAVACOOKIE,:LEAFSTONE,
				   :LEMONADE,:LIFEORB,:LIGHTBALL,:LIGHTCLAY,:LUCKYEGG,
				   :LUMINOUSMOSS,:LUMIOSEGALETTE,:MAGNET,:MAXELIXIR,:MAXETHER,
				   :MAXPOTION,:MAXREPEL,:MAXREVIVE,:METALCOAT,:METRONOME,
				   :MIRACLESEED,:MOOMOOMILK,:MOONSTONE,:MYSTICWATER,:NEVERMELTICE,
				   :NUGGET,:OLDGATEAU,:PARALYZEHEAL,:PARLYZHEAL,:PASSORB,
				   :PEARL,:PEARLSTRING,:POKEDOLL,:POKETOY,:POTION,
				   :PPMAX,:PPUP,:PRISMSCALE,:PROTEIN,:RAGECANDYBAR,
				   :RARECANDY,:RAZORFANG,:REDFLUTE,:REDSHARD,:RELICBAND,
				   :RELICCOPPER,:RELICCROWN,:RELICGOLD,:RELICSILVER,:RELICSTATUE,
				   :RELICVASE,:REPEL,:RESETURGE,:REVIVALHERB,:REVIVE,
				   :RICHMULCH,:SACHET,:SACREDASH,:SCOPELENS,:SHALOURSABLE,
				   :SHELLBELL,:SHOALSALT,:SHOALSHELL,:SMOKEBALL,:SNOWBALL,
				   :SODAPOP,:SOULDEW,:SPELLTAG,:STABLEMULCH,:STARDUST,
				   :STARPIECE,:SUNSTONE,:SUPERPOTION,:SUPERREPEL,:SURPRISEMULCH,
				   :SWEETHEART,:THUNDERSTONE,:TINYMUSHROOM,:TOXICORB,:TWISTEDSPOON,
				   :UPGRADE,:WATERSTONE,:WHIPPEDDREAM,:WHITEFLUTE,:XACCURACY,
				   :XACCURACY2,:XACCURACY3,:XACCURACY6,:XATTACK,:XATTACK2,
				   :XATTACK3,:XATTACK6,:XDEFEND,:XDEFEND2,:XDEFEND3,
				   :XDEFEND6,:XDEFENSE,:XDEFENSE2,:XDEFENSE3,:XDEFENSE6,
				   :XSPDEF,:XSPDEF2,:XSPDEF3,:XSPDEF6,:XSPATK,
				   :XSPATK2,:XSPATK3,:XSPATK6,:XSPECIAL,:XSPECIAL2,
				   :XSPECIAL3,:XSPECIAL6,:XSPEED,:XSPEED2,:XSPEED3,
				   :XSPEED6,:YELLOWFLUTE,:YELLOWSHARD,:ZINC],
			20 => [:CLEVERWING,:GENIUSWING,:HEALTHWING,:MUSCLEWING,:PRETTYWING,
				   :RESISTWING,:SWIFTWING],
			10 => [:AIRBALLOON,:BIGROOT,:BLUESCARF,:BRIGHTPOWDER,:CHOICEBAND,
				   :CHOICESCARF,:CHOICESPECS,:DESTINYKNOT,:EXPERTBELT,:FOCUSBAND,
				   :FOCUSSASH,:FULLINCENSE,:GREENSCARF,:LAGGINGTAIL,:LAXINCENSE,
				   :LEFTOVERS,:LUCKINCENSE,:MENTALHERB,:METALPOWDER,:MUSCLEBAND,
				   :ODDINCENSE,:PINKSCARF,:POWERHERB,:PUREINCENSE,:QUICKPOWDER,
				   :REAPERCLOTH,:REDCARD,:REDSCARF,:RINGTARGET,:ROCKINCENSE,
				   :ROSEINCENSE,:SEAINCENSE,:SHEDSHELL,:SILKSCARF,:SILVERPOWDER,
				   :SMOOTHROCK,:SOFTSAND,:SOOTHEBELL,:WAVEINCENSE,:WHITEHERB,
				   :WIDELENS,:WISEGLASSES,:YELLOWSCARF,:ZOOMLENS]
		}
	}

	public object pbMoveFailed(Battle.Battler attacker, Battle.Battler opponent){
		if (attacker.item==0 ||
					   this.battle.pbIsUnlosableItem(attacker, attacker.item) ||
					   pbIsPokeBall? (attacker.item) ||
					   this.battle.field.MagicRoom>0 ||
					   attacker.hasWorkingAbility(Abilities.KLUTZ) ||
					   attacker.effects.Embargo>0) return true;
		foreach (var i in flingarray.keys){ 
		  if (flingarray[i]){
			foreach (var i in flingarray[i]){ 
			  if (isConst? (attacker.item, PBItems, j)) return false;
			 }

		  }
		}
		if (pbIsBerry? (attacker.item) &&
						!attacker.pbOpposing1.hasWorkingAbility(Abilities.UNNERVE) &&
						!attacker.pbOpposing2.hasWorkingAbility(Abilities.UNNERVE)) return false;
		return true
	  }

	  public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){
		if (pbIsBerry? (attacker.item)) return 10;
		if (pbIsMegaStone? (attacker.item)) return 80;
		foreach (var i in flingarray.keys){ 
		  if (flingarray[i]){
			foreach (var i in flingarray[i]){ 
			  if (isConst? (attacker.item, PBItems, j)) return i;
			 }

		  }
		}
		return 1
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (attacker.item==0){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return 0
		}
		attacker.effects.Unburden= true

		//battle.pbDisplay(_INTL("{1} flung its {2}!", attacker.pbThis, PBItems.getName(attacker.item)))
		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if (opponent.damagestate.CalcDamage>0 && !opponent.damagestate.Substitute &&){
		   (attacker.hasMoldBreaker || !opponent.hasWorkingAbility(Abilities.SHIELDDUST))
		  if (attacker.hasWorkingBerry){
			opponent.pbActivateBerryEffect(attacker.item,false)

		  else if attacker.hasWorkingItem(Items.FLAMEORB)
			if (opponent.pbCanBurn? (attacker,false,self)){
			  opponent.pbBurn(attacker)
			}

		  else if attacker.hasWorkingItem(Items.KINGSROCK) ||
				attacker.hasWorkingItem(Items.RAZORFANG)
			opponent.pbFlinch(attacker)
		  else if attacker.hasWorkingItem(Items.LIGHTBALL)
			if (opponent.pbCanParalyze? (attacker,false,self)){
			  opponent.pbParalyze(attacker)
			}

		  else if attacker.hasWorkingItem(Items.MENTALHERB)
			if (opponent.effects.Attract>=0){
			  opponent.pbCureAttract
			  //battle.pbDisplay(_INTL("{1} got over its infatuation.", opponent.pbThis))
			}
			if (opponent.effects.Taunt>0){
			  opponent.effects.Taunt=0
			  //battle.pbDisplay(_INTL("{1}'s taunt wore off!",opponent.pbThis))
			}
			if (opponent.effects.Encore>0){
			  opponent.effects.Encore=0
			  opponent.effects.EncoreMove=0
			  opponent.effects.EncoreIndex=0
			  //battle.pbDisplay(_INTL("{1}'s encore ended!",opponent.pbThis))
			}
			if (opponent.effects.Torment){
			  opponent.effects.Torment= false

			  //battle.pbDisplay(_INTL("{1}'s torment wore off!", opponent.pbThis))
			}
			if (opponent.effects.Disable>0){
			  opponent.effects.Disable=0
			  //battle.pbDisplay(_INTL("{1} is no longer disabled!",opponent.pbThis))
			}
			if (opponent.effects.HealBlock>0){
			  opponent.effects.HealBlock=0
			  //battle.pbDisplay(_INTL("{1}'s Heal Block wore off!",opponent.pbThis))
			}
		  else if attacker.hasWorkingItem(Items.POISONBARB)
			if (opponent.pbCanPoison? (attacker,false,self)){
			  opponent.pbPoison(attacker)
			}

		  else if attacker.hasWorkingItem(Items.TOXICORB)
			if (opponent.pbCanPoison? (attacker,false,self)){
			  opponent.pbPoison(attacker, null,true)
			}
		  else if attacker.hasWorkingItem(Items.WHITEHERB)
			while true
			  reducedstats=false
			  foreach (var i in [PBStats::ATTACK, PBStats::DEFENSE,
						PBStats::SPEED, PBStats::SPATK, PBStats::SPDEF,
						PBStats::EVASION, PBStats::ACCURACY]){ 
				if (opponent.stages[i]<0){
				  opponent.stages[i]=0; reducedstats=true
				}
			  }
			  break if !reducedstats
			  //battle.pbDisplay(_INTL("{1}'s status is returned to normal!",
				 opponent.pbThis(true)))
			}
		  }

		}
		attacker.pbConsumeItem
		return ret
	  }
	}



	/// <summary>
	/// For 5 rounds, the target cannnot use its held item, its held item has no
	/// effect, and no items can be used on it. (Embargo)
	/// <summary>
	public class PokeBattle_Move_0F8 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (opponent.effects.Embargo>0){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.effects.Embargo=5
		//battle.pbDisplay(_INTL("{1} can't use items anymore!",opponent.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// For 5 rounds, all held items cannot be used in any way and have no effect.
	/// Held items can still change hands, but can't be thrown. (Magic Room)
	/// <summary>
	public class PokeBattle_Move_0F9 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (this.battle.field.MagicRoom>0){
		  this.battle.field.MagicRoom=0
		  //battle.pbDisplay(_INTL("The area returned to normal!"))
		else
		  pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		  this.battle.field.MagicRoom=5
		  //battle.pbDisplay(_INTL("It created a bizarre area in which Pokémon's held items lose their effects!"))
		}
		return 0
	  }
	}



	/// <summary>
	/// User takes recoil damage equal to 1/4 of the damage this move dealt.
	/// <summary>
	public class PokeBattle_Move_0FA : PokeBattle_Move
	{
		public object isRecoilMove?
		return true
	  }

	  public object pbEffectAfterHit(Battle.Battler attacker, Battle.Battler opponent, turneffects){
		if (!attacker.isFainted() && turneffects.TotalDamage>0){
		  if (!attacker.hasWorkingAbility(Abilities.ROCKHEAD) &&){
			 !attacker.hasWorkingAbility(Abilities.MAGICGUARD)
			attacker.pbReduceHP((int)Math.Round(turneffects.TotalDamage/4.0f))
			//battle.pbDisplay(_INTL("{1} is damaged by recoil!",attacker.pbThis))
		  }
		}
	  }
	}



	/// <summary>
	/// User takes recoil damage equal to 1/3 of the damage this move dealt.
	/// <summary>
	public class PokeBattle_Move_0FB : PokeBattle_Move
	{
		public object isRecoilMove?
		return true
	  }

	  public object pbEffectAfterHit(Battle.Battler attacker, Battle.Battler opponent, turneffects){
		if (!attacker.isFainted() && turneffects.TotalDamage>0){
		  if (!attacker.hasWorkingAbility(Abilities.ROCKHEAD) &&){
			 !attacker.hasWorkingAbility(Abilities.MAGICGUARD)
			attacker.pbReduceHP((int)Math.Round(turneffects.TotalDamage/3.0f))
			//battle.pbDisplay(_INTL("{1} is damaged by recoil!",attacker.pbThis))
		  }
		}
	  }
	}



	/// <summary>
	/// User takes recoil damage equal to 1/2 of the damage this move dealt.
	/// (Head Smash)
	/// <summary>
	public class PokeBattle_Move_0FC : PokeBattle_Move
	{
		public object isRecoilMove?
		return true
	  }

	  public object pbEffectAfterHit(Battle.Battler attacker, Battle.Battler opponent, turneffects){
		if (!attacker.isFainted() && turneffects.TotalDamage>0){
		  if (!attacker.hasWorkingAbility(Abilities.ROCKHEAD) &&){
			 !attacker.hasWorkingAbility(Abilities.MAGICGUARD)
			attacker.pbReduceHP((int)Math.Round(turneffects.TotalDamage/2.0f))
			//battle.pbDisplay(_INTL("{1} is damaged by recoil!",attacker.pbThis))
		  }
		}
	  }
	}



	/// <summary>
	/// User takes recoil damage equal to 1/3 of the damage this move dealt.
	/// May paralyze the target. (Volt Tackle)
	/// <summary>
	public class PokeBattle_Move_0FD : PokeBattle_Move
	{
		public object isRecoilMove?
		return true
	  }

	  public object pbEffectAfterHit(Battle.Battler attacker, Battle.Battler opponent, turneffects){
		if (!attacker.isFainted() && turneffects.TotalDamage>0){
		  if (!attacker.hasWorkingAbility(Abilities.ROCKHEAD) &&){
			 !attacker.hasWorkingAbility(Abilities.MAGICGUARD)
			attacker.pbReduceHP((int)Math.Round(turneffects.TotalDamage/3.0f))
			//battle.pbDisplay(_INTL("{1} is damaged by recoil!",attacker.pbThis))
		  }
		}
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		if (opponent.pbCanParalyze? (attacker,false,self)){
		  opponent.pbParalyze(attacker)
		}
	  }
	}



	/// <summary>
	/// User takes recoil damage equal to 1/3 of the damage this move dealt.
	/// May burn the target. (Flare Blitz)
	/// <summary>
	public class PokeBattle_Move_0FE : PokeBattle_Move
	{
		public object isRecoilMove?
		return true
	  }

	  public object pbEffectAfterHit(Battle.Battler attacker, Battle.Battler opponent, turneffects){
		if (!attacker.isFainted() && turneffects.TotalDamage>0){
		  if (!attacker.hasWorkingAbility(Abilities.ROCKHEAD) &&){
			 !attacker.hasWorkingAbility(Abilities.MAGICGUARD)
			attacker.pbReduceHP((int)Math.Round(turneffects.TotalDamage/3.0f))
			//battle.pbDisplay(_INTL("{1} is damaged by recoil!",attacker.pbThis))
		  }
		}
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		if (opponent.pbCanBurn? (attacker,false,self)){
		  opponent.pbBurn(attacker)
		}
	  }
	}



	/// <summary>
	/// Starts sunny weather. (Sunny Day)
	/// <summary>
	public class PokeBattle_Move_0FF : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		case this.battle.weather
		when PBWeather::HEAVYRAIN
		  //battle.pbDisplay(_INTL("There is no relief from this heavy rain!"))
		  return -1
		when PBWeather::HARSHSUN

		  //battle.pbDisplay(_INTL("The extremely harsh sunlight was not lessened at all!"))
		  return -1
		when PBWeather::STRONGWINDS

		  //battle.pbDisplay(_INTL("The mysterious air current blows on regardless!"))
		  return -1
		when PBWeather::SUNNYDAY

		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		this.battle.weather=PBWeather::SUNNYDAY
		this.battle.weatherduration=5
		this.battle.weatherduration= 8 if attacker.hasWorkingItem(Items.HEATROCK)

		this.battle.pbCommonAnimation("Sunny",null,null)
		//battle.pbDisplay(_INTL("The sunlight turned harsh!"))
		return 0
	  }
	}



	/// <summary>
	/// Starts rainy weather. (Rain Dance)
	/// <summary>
	public class PokeBattle_Move_100 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		case this.battle.weather
		when PBWeather::HEAVYRAIN
		  //battle.pbDisplay(_INTL("There is no relief from this heavy rain!"))
		  return -1
		when PBWeather::HARSHSUN

		  //battle.pbDisplay(_INTL("The extremely harsh sunlight was not lessened at all!"))
		  return -1
		when PBWeather::STRONGWINDS

		  //battle.pbDisplay(_INTL("The mysterious air current blows on regardless!"))
		  return -1
		when PBWeather::RAINDANCE

		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		this.battle.weather=PBWeather::RAINDANCE
		this.battle.weatherduration=5
		this.battle.weatherduration= 8 if attacker.hasWorkingItem(Items.DAMPROCK)

		this.battle.pbCommonAnimation("Rain",null,null)
		//battle.pbDisplay(_INTL("It started to rain!"))
		return 0
	  }
	}



	/// <summary>
	/// Starts sandstorm weather. (Sandstorm)
	/// <summary>
	public class PokeBattle_Move_101 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		case this.battle.weather
		when PBWeather::HEAVYRAIN
		  //battle.pbDisplay(_INTL("There is no relief from this heavy rain!"))
		  return -1
		when PBWeather::HARSHSUN

		  //battle.pbDisplay(_INTL("The extremely harsh sunlight was not lessened at all!"))
		  return -1
		when PBWeather::STRONGWINDS

		  //battle.pbDisplay(_INTL("The mysterious air current blows on regardless!"))
		  return -1
		when PBWeather::SANDSTORM

		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		this.battle.weather=PBWeather::SANDSTORM
		this.battle.weatherduration=5
		this.battle.weatherduration= 8 if attacker.hasWorkingItem(Items.SMOOTHROCK)

		this.battle.pbCommonAnimation("Sandstorm",null,null)
		//battle.pbDisplay(_INTL("A sandstorm brewed!"))
		return 0
	  }
	}



	/// <summary>
	/// Starts hail weather. (Hail)
	/// <summary>
	public class PokeBattle_Move_102 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		case this.battle.weather
		when PBWeather::HEAVYRAIN
		  //battle.pbDisplay(_INTL("There is no relief from this heavy rain!"))
		  return -1
		when PBWeather::HARSHSUN

		  //battle.pbDisplay(_INTL("The extremely harsh sunlight was not lessened at all!"))
		  return -1
		when PBWeather::STRONGWINDS

		  //battle.pbDisplay(_INTL("The mysterious air current blows on regardless!"))
		  return -1
		when PBWeather::HAIL

		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		this.battle.weather=PBWeather::HAIL
		this.battle.weatherduration=5
		this.battle.weatherduration= 8 if attacker.hasWorkingItem(Items.ICYROCK)

		this.battle.pbCommonAnimation("Hail",null,null)
		//battle.pbDisplay(_INTL("It started to hail!"))
		return 0
	  }
	}



	/// <summary>
	/// Entry hazard. Lays spikes on the opposing side (max. 3 layers). (Spikes)
	/// <summary>
	public class PokeBattle_Move_103 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (attacker.pbOpposingSide.effects.Spikes>=3){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		attacker.pbOpposingSide.effects.Spikes+=1
		if (!this.battle.pbIsOpposing? (attacker.index)){
		   //battle.pbDisplay(_INTL("Spikes were scattered all around the opposing team's feet!"))
		else
		  //battle.pbDisplay(_INTL("Spikes were scattered all around your team's feet!"))
		}
		return 0
	  }
	}



	/// <summary>
	/// Entry hazard. Lays poison spikes on the opposing side (max. 2 layers).
	/// (Toxic Spikes)
	/// <summary>
	public class PokeBattle_Move_104 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (attacker.pbOpposingSide.effects.ToxicSpikes>=2){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		attacker.pbOpposingSide.effects.ToxicSpikes+=1
		if (!this.battle.pbIsOpposing? (attacker.index)){
		   //battle.pbDisplay(_INTL("Poison spikes were scattered all around the opposing team's feet!"))
		else
		  //battle.pbDisplay(_INTL("Poison spikes were scattered all around your team's feet!"))
		}
		return 0
	  }
	}



	/// <summary>
	/// Entry hazard. Lays stealth rocks on the opposing side. (Stealth Rock)
	/// <summary>
	public class PokeBattle_Move_105 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (attacker.pbOpposingSide.effects.StealthRock){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		attacker.pbOpposingSide.effects.StealthRock=true
		if (!this.battle.pbIsOpposing? (attacker.index)){
		   //battle.pbDisplay(_INTL("Pointed stones float in the air around the opposing team!"))
		else
		  //battle.pbDisplay(_INTL("Pointed stones float in the air around your team!"))
		}
		return 0
	  }
	}



	/// <summary>
	/// Forces ally's Pledge move to be used next, if it hasn't already. (Grass Pledge)
	/// Combo's with ally's Pledge move if it was just used. Power is doubled, and
	/// causes either a sea of fire or a swamp on the opposing side.
	/// <summary>
	public class PokeBattle_Move_106 : PokeBattle_Move
	{
		public object pbOnStartUse(Battle.Battler attacker)
		this.doubledamage = false; this.overridetype=false
		if (attacker.effects.FirstPledge==0x107 ||   // Fire Pledge){
		   attacker.effects.FirstPledge==0x108      // Water Pledge
		  //battle.pbDisplay(_INTL("The two moves have become one! It's a combined move!"))
		  this.doubledamage=true
		  if (attacker.effects.FirstPledge==0x107   // Fire Pledge){
			this.overridetype=true
		  }
		}
		return true
	  }

	  public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){
		if (this.doubledamage){
		  return basedmg*2
		}
		return basedmg
	  }

	  public object pbModifyType(type, Battle.Battler attacker, Battle.Battler opponent){
		if (this.overridetype){
		  type = Types.FIRE

		}
		return super(type, attacker, opponent)
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (!this.battle.doublebattle || !attacker.pbPartner || attacker.pbPartner.isFainted()){
		  attacker.effects.FirstPledge= 0
		  return super(attacker, opponent, hitnum, alltargets, showanimation)
		}
		// Combined move's effect
		if (attacker.effects.FirstPledge==0x107   // Fire Pledge){
		  ret= super(attacker, opponent, hitnum, alltargets, showanimation)
		  if (opponent.damagestate.CalcDamage>0){

			attacker.pbOpposingSide.effects.SeaOfFire= 4
			if (!this.battle.pbIsOpposing?(attacker.index)){
			  //battle.pbDisplay(_INTL("A sea of fire enveloped the opposing team!"))
			  this.battle.pbCommonAnimation("SeaOfFireOpp",null,null)
			else
			  //battle.pbDisplay(_INTL("A sea of fire enveloped your team!"))
			  this.battle.pbCommonAnimation("SeaOfFire",null,null)
			}
		  }

		  attacker.effects.FirstPledge=0
		  return ret
		else if attacker.effects.FirstPledge==0x108   // Water Pledge
		  ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		  if (opponent.damagestate.CalcDamage>0){
			attacker.pbOpposingSide.effects.Swamp=4
			if (!this.battle.pbIsOpposing? (attacker.index)){
			   //battle.pbDisplay(_INTL("A swamp enveloped the opposing team!"))
			  this.battle.pbCommonAnimation("SwampOpp",null,null)
			else
			  //battle.pbDisplay(_INTL("A swamp enveloped your team!"))
			  this.battle.pbCommonAnimation("Swamp",null,null)
			}
		  }

		  attacker.effects.FirstPledge=0
		  return ret
		}
		// Set up partner for a combined move
		attacker.effects.FirstPledge=0
		partnermove=-1
		if (this.battle.choices[attacker.pbPartner.index][0]==1 // Chose a move){
		  if (!attacker.pbPartner.hasMovedThisRound?){
			move = this.battle.choices[attacker.pbPartner.index][2]
			if (move && move.id>0){
			  partnermove=this.battle.choices[attacker.pbPartner.index][2].function
			}

		  }
		}
		if (partnermove==0x107 ||   // Fire Pledge){
		   partnermove==0x108      // Water Pledge
		  //battle.pbDisplay(_INTL("{1} is waiting for {2}'s move...",attacker.pbThis,attacker.pbPartner.pbThis(true)))
		  attacker.pbPartner.effects.FirstPledge==this.function
		  attacker.pbPartner.effects.MoveNext= true
		  return 0

		}
		// Use the move on its own
		return super(attacker, opponent, hitnum, alltargets, showanimation)
	  }

	  public object pbShowAnimation(id, Battle.Battler attacker, Battle.Battler opponent, byte hitnum= 0, byte? alltargets= null, bool showanimation= true){
		if (this.overridetype){
		  return super(Moves.FIREPLEDGE,attacker,opponent,hitnum,alltargets,showanimation)
		}
		return super(id, attacker, opponent, hitnum, alltargets, showanimation)
	  }
	}



	/// <summary>
	/// Forces ally's Pledge move to be used next, if it hasn't already. (Fire Pledge)
	/// Combo's with ally's Pledge move if it was just used. Power is doubled, and
	/// causes either a sea of fire on the opposing side or a rainbow on the user's side.
	/// <summary>
	public class PokeBattle_Move_107 : PokeBattle_Move
	{
		public object pbOnStartUse(Battle.Battler attacker)
		this.doubledamage = false; this.overridetype=false
		if (attacker.effects.FirstPledge==0x106 ||   // Grass Pledge){
		   attacker.effects.FirstPledge==0x108      // Water Pledge
		  //battle.pbDisplay(_INTL("The two moves have become one! It's a combined move!"))
		  this.doubledamage=true
		  if (attacker.effects.FirstPledge==0x108   // Water Pledge){
			this.overridetype=true
		  }
		}
		return true
	  }

	  public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){
		if (this.doubledamage){
		  return basedmg*2
		}
		return basedmg
	  }

	  public object pbModifyType(type, Battle.Battler attacker, Battle.Battler opponent){
		if (this.overridetype){
		  type = Types.WATER

		}
		return super(type, attacker, opponent)
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (!this.battle.doublebattle || !attacker.pbPartner || attacker.pbPartner.isFainted()){
		  attacker.effects.FirstPledge= 0
		  return super(attacker, opponent, hitnum, alltargets, showanimation)
		}
		// Combined move's effect
		if (attacker.effects.FirstPledge==0x106   // Grass Pledge){
		  ret= super(attacker, opponent, hitnum, alltargets, showanimation)
		  if (opponent.damagestate.CalcDamage>0){

			attacker.pbOpposingSide.effects.SeaOfFire= 4
			if (!this.battle.pbIsOpposing?(attacker.index)){
			  //battle.pbDisplay(_INTL("A sea of fire enveloped the opposing team!"))
			  this.battle.pbCommonAnimation("SeaOfFireOpp",null,null)
			else
			  //battle.pbDisplay(_INTL("A sea of fire enveloped your team!"))
			  this.battle.pbCommonAnimation("SeaOfFire",null,null)
			}
		  }

		  attacker.effects.FirstPledge=0
		  return ret
		else if attacker.effects.FirstPledge==0x108   // Water Pledge
		  ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		  if (opponent.damagestate.CalcDamage>0){
			attacker.OwnSide.Rainbow=4
			if (!this.battle.pbIsOpposing? (attacker.index)){
			   //battle.pbDisplay(_INTL("A rainbow appeared in the sky on your team's side!"))
			  this.battle.pbCommonAnimation("Rainbow",null,null)
			else
			  //battle.pbDisplay(_INTL("A rainbow appeared in the sky on the opposing team's side!"))
			  this.battle.pbCommonAnimation("RainbowOpp",null,null)
			}
		  }

		  attacker.effects.FirstPledge=0
		  return ret
		}
		// Set up partner for a combined move
		attacker.effects.FirstPledge=0
		partnermove=-1
		if (this.battle.choices[attacker.pbPartner.index][0]==1 // Chose a move){
		  if (!attacker.pbPartner.hasMovedThisRound?){
			move = this.battle.choices[attacker.pbPartner.index][2]
			if (move && move.id>0){
			  partnermove=this.battle.choices[attacker.pbPartner.index][2].function
			}

		  }
		}
		if (partnermove==0x106 ||   // Grass Pledge){
		   partnermove==0x108      // Water Pledge
		  //battle.pbDisplay(_INTL("{1} is waiting for {2}'s move...",attacker.pbThis,attacker.pbPartner.pbThis(true)))
		  attacker.pbPartner.effects.FirstPledge==this.function
		  attacker.pbPartner.effects.MoveNext= true
		  return 0

		}
		// Use the move on its own
		return super(attacker, opponent, hitnum, alltargets, showanimation)
	  }

	  public object pbShowAnimation(id, Battle.Battler attacker, Battle.Battler opponent, byte hitnum= 0, byte? alltargets= null, bool showanimation= true){
		if (this.overridetype){
		  return super(Moves.WATERPLEDGE,attacker,opponent,hitnum,alltargets,showanimation)
		}
		return super(id, attacker, opponent, hitnum, alltargets, showanimation)
	  }
	}



	/// <summary>
	/// Forces ally's Pledge move to be used next, if it hasn't already. (Water Pledge)
	/// Combo's with ally's Pledge move if it was just used. Power is doubled, and
	/// causes either a swamp on the opposing side or a rainbow on the user's side.
	/// <summary>
	public class PokeBattle_Move_108 : PokeBattle_Move
	{
		public object pbOnStartUse(Battle.Battler attacker)
		this.doubledamage = false; this.overridetype=false
		if (attacker.effects.FirstPledge==0x106 ||   // Grass Pledge){
		   attacker.effects.FirstPledge==0x107      // Fire Pledge
		  //battle.pbDisplay(_INTL("The two moves have become one! It's a combined move!"))
		  this.doubledamage=true
		  if (attacker.effects.FirstPledge==0x106   // Grass Pledge){
			this.overridetype=true
		  }
		}
		return true
	  }

	  public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){
		if (this.doubledamage){
		  return basedmg*2
		}
		return basedmg
	  }

	  public object pbModifyType(type, Battle.Battler attacker, Battle.Battler opponent){
		if (this.overridetype){
		  type = Types.GRASS

		}
		return super(type, attacker, opponent)
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (!this.battle.doublebattle || !attacker.pbPartner || attacker.pbPartner.isFainted()){
		  attacker.effects.FirstPledge= 0
		  return super(attacker, opponent, hitnum, alltargets, showanimation)
		}
		// Combined move's effect
		if (attacker.effects.FirstPledge==0x106   // Grass Pledge){
		  ret= super(attacker, opponent, hitnum, alltargets, showanimation)
		  if (opponent.damagestate.CalcDamage>0){

			attacker.pbOpposingSide.effects.Swamp= 4
			if (!this.battle.pbIsOpposing?(attacker.index)){
			  //battle.pbDisplay(_INTL("A swamp enveloped the opposing team!"))
			  this.battle.pbCommonAnimation("SwampOpp",null,null)
			else
			  //battle.pbDisplay(_INTL("A swamp enveloped your team!"))
			  this.battle.pbCommonAnimation("Swamp",null,null)
			}
		  }

		  attacker.effects.FirstPledge=0
		  return ret
		else if attacker.effects.FirstPledge==0x107   // Fire Pledge
		  ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		  if (opponent.damagestate.CalcDamage>0){
			attacker.OwnSide.Rainbow=4
			if (!this.battle.pbIsOpposing? (attacker.index)){
			   //battle.pbDisplay(_INTL("A rainbow appeared in the sky on your team's side!"))
			  this.battle.pbCommonAnimation("Rainbow",null,null)
			else
			  //battle.pbDisplay(_INTL("A rainbow appeared in the sky on the opposing team's side!"))
			  this.battle.pbCommonAnimation("RainbowOpp",null,null)
			}
		  }

		  attacker.effects.FirstPledge=0
		  return ret
		}
		// Set up partner for a combined move
		attacker.effects.FirstPledge=0
		partnermove=-1
		if (this.battle.choices[attacker.pbPartner.index][0]==1 // Chose a move){
		  if (!attacker.pbPartner.hasMovedThisRound?){
			move = this.battle.choices[attacker.pbPartner.index][2]
			if (move && move.id>0){
			  partnermove=this.battle.choices[attacker.pbPartner.index][2].function
			}

		  }
		}
		if (partnermove==0x106 ||   // Grass Pledge){
		   partnermove==0x107      // Fire Pledge
		  //battle.pbDisplay(_INTL("{1} is waiting for {2}'s move...",attacker.pbThis,attacker.pbPartner.pbThis(true)))
		  attacker.pbPartner.effects.FirstPledge==this.function
		  attacker.pbPartner.effects.MoveNext= true
		  return 0

		}
		// Use the move on its own
		return super(attacker, opponent, hitnum, alltargets, showanimation)
	  }

	  public object pbShowAnimation(id, Battle.Battler attacker, Battle.Battler opponent, byte hitnum= 0, byte? alltargets= null, bool showanimation= true){
		if (this.overridetype){
		  return super(Moves.GRASSPLEDGE,attacker,opponent,hitnum,alltargets,showanimation)
		}
		return super(id, attacker, opponent, hitnum, alltargets, showanimation)
	  }
	}



	/// <summary>
	/// Scatters coins that the player picks up after winning the battle. (Pay Day)
	/// <summary>
	public class PokeBattle_Move_109 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if (opponent.damagestate.CalcDamage>0){
		  if (this.battle.pbOwnedByPlayer? (attacker.index)){
			 this.battle.extramoney+=5*attacker.level
			 this.battle.extramoney= MAXMONEY if this.battle.extramoney>MAXMONEY
		   }
 
		   //battle.pbDisplay(_INTL("Coins were scattered everywhere!"))
		}
		return ret
	  }
	}



	/// <summary>
	/// Ends the opposing side's Light Screen and Reflect. (Brick Break)
	/// <summary>
	public class PokeBattle_Move_10A : PokeBattle_Move
	{
		public object pbCalcDamage(Battle.Battler attacker, Battle.Battler opponent){
		return base.pbCalcDamage(attacker, opponent, thismove.NOREFLECT);
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){

		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if (attacker.pbOpposingSide.effects.Reflect>0){
		  attacker.pbOpposingSide.effects.Reflect=0
		  if (!this.battle.pbIsOpposing? (attacker.index)){
			 //battle.pbDisplay(_INTL("The opposing team's Reflect wore off!"))
		  else
			//battle.pbDisplayPaused(_INTL("Your team's Reflect wore off!"))
		  }
		}
		if (attacker.pbOpposingSide.effects.LightScreen>0){
		  attacker.pbOpposingSide.effects.LightScreen=0
		  if (!this.battle.pbIsOpposing? (attacker.index)){
			 //battle.pbDisplay(_INTL("The opposing team's Light Screen wore off!"))
		  else
			//battle.pbDisplay(_INTL("Your team's Light Screen wore off!"))
		  }
		}
		return ret
	  }

	  public object pbShowAnimation(id, Battle.Battler attacker, Battle.Battler opponent, byte hitnum= 0, byte? alltargets= null, bool showanimation= true){
		if (attacker.pbOpposingSide.effects.Reflect>0 ||){
		   attacker.pbOpposingSide.effects.LightScreen>0
		  return super(id, attacker, opponent,1, alltargets, showanimation) // Wall-breaking anim
		}
		return super(id, attacker, opponent, hitnum, alltargets, showanimation)
	  }
	}



	/// <summary>
	/// If attack misses, user takes crash damage of 1/2 of max HP.
	/// (Hi Jump Kick, Jump Kick)
	/// <summary>
	public class PokeBattle_Move_10B : PokeBattle_Move
	{
		public object isRecoilMove?
		return true
	  }

	  public object unusableInGravity?
		return true
	  }
	}



	/// <summary>
	/// User turns 1/4 of max HP into a substitute. (Substitute)
	/// <summary>
	public class PokeBattle_Move_10C : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (attacker.effects.Substitute>0){
		  //battle.pbDisplay(_INTL("{1} already has a substitute!",attacker.pbThis))
		  return -1
		}
		sublife =[(attacker.TotalHP / 4).floor, 1].max
		if (attacker.hp<=sublife){
		  //battle.pbDisplay(_INTL("It was too weak to make a substitute!"))
		  return -1  
		}
		attacker.pbReduceHP(sublife,false,false)

		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		attacker.effects.MultiTurn=0
		attacker.effects.MultiTurnAttack=0
		attacker.effects.Substitute=sublife
		//battle.pbDisplay(_INTL("{1} put in a substitute!", attacker.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// User is not Ghost: Decreases the user's Speed, increases the user's Attack &
	/// Defense by 1 stage each.
	/// User is Ghost: User loses 1/2 of max HP, and curses the target.
	/// Cursed Pokémon lose 1/4 of their max HP at the end of each round.
	/// (Curse)
	/// <summary>
	public class PokeBattle_Move_10D : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		failed=false
		if (attacker.hasType(Types.GHOST)){
		  if (opponent.effects.Curse ||){
			 opponent.OwnSide.CraftyShield
			failed = true
		  else
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

			//battle.pbDisplay(_INTL("{1} cut its own HP and laid a curse on {2}!",attacker.pbThis,opponent.pbThis(true)))
			opponent.effects.Curse=true
			attacker.pbReduceHP((attacker.TotalHP/2).floor)
		  }
		else
		  lowerspeed=attacker.pbCanReduceStatStage? (PBStats::SPEED, attacker,false,self)
		  raiseatk=attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self)
		  raisedef=attacker.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,false,self)
		  if (!lowerspeed && !raiseatk && !raisedef){
			failed = true
		  else
			pbShowAnimation(this.id, attacker, null,1, alltargets, showanimation) // Non-Ghost move animation
			if (lowerspeed){
			  attacker.pbReduceStat(PBStats::SPEED,1, attacker,false, self)

			}
			showanim = true
			if (raiseatk){
			  attacker.pbIncreaseStat(PBStats::ATTACK,1, attacker,false, self, showanim)

			  showanim=false
			}
			if (raisedef){
			  attacker.pbIncreaseStat(PBStats::DEFENSE,1, attacker,false, self, showanim)

			  showanim=false
			}
		  }

		}
		if (failed){
		  //battle.pbDisplay(_INTL("But it failed!"))
		}
		return failed? -1 : 0
	  }
	}



	/// <summary>
	/// Target's last move used loses 4 PP. (Spite)
	/// <summary>
	public class PokeBattle_Move_10E : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		foreach (var i in opponent.moves){ 
		  if (i.id==opponent.lastMoveUsed && i.id>0 && i.pp>0){
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

			reduction=[4, i.pp].min
	opponent.pbSetPP(i, i.pp-reduction)

			//battle.pbDisplay(_INTL("It reduced the PP of {1}'s {2} by {3}!",opponent.pbThis(true),i.name,reduction))
			return 0
		  }
		}

		//battle.pbDisplay(_INTL("But it failed!"))
		return -1
	  }
	}



	/// <summary>
	/// Target will lose 1/4 of max HP at end of each round, while asleep. (Nightmare)
	/// <summary>
	public class PokeBattle_Move_10F : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (opponent.status!=PBStatuses::SLEEP || opponent.effects.Nightmare ||){
		   (opponent.effects.Substitute>0 && !ignoresSubstitute? (attacker))
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.effects.Nightmare=true
		//battle.pbDisplay(_INTL("{1} began having a nightmare!",opponent.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// Removes trapping moves, entry hazards and Leech Seed on user/user's side.
	/// (Rapid Spin)
	/// <summary>
	public class PokeBattle_Move_110 : PokeBattle_Move
	{
		public object pbEffectAfterHit(Battle.Battler attacker, Battle.Battler opponent, turneffects){
		if (!attacker.isFainted() && turneffects.TotalDamage>0){
		  if (attacker.effects.MultiTurn>0){
			mtattack=PBMoves.getName(attacker.effects.MultiTurnAttack)
			mtuser = this.battle.battlers[attacker.effects.MultiTurnUser]

			//battle.pbDisplay(_INTL("{1} got free of {2}'s {3}!",attacker.pbThis,mtuser.pbThis(true),mtattack))
			attacker.effects.MultiTurn=0
			attacker.effects.MultiTurnAttack=0
			attacker.effects.MultiTurnUser=-1
		  }
		  if (attacker.effects.LeechSeed>=0){
			attacker.effects.LeechSeed=-1
			//battle.pbDisplay(_INTL("{1} shed Leech Seed!",attacker.pbThis))   
		  }
		  if (attacker.OwnSide.StealthRock){
			attacker.OwnSide.StealthRock= false

			//battle.pbDisplay(_INTL("{1} blew away stealth rocks!", attacker.pbThis))     
		  }
		  if (attacker.OwnSide.Spikes>0){
			attacker.OwnSide.Spikes=0
			//battle.pbDisplay(_INTL("{1} blew away Spikes!",attacker.pbThis))     
		  }
		  if (attacker.OwnSide.ToxicSpikes>0){
			attacker.OwnSide.ToxicSpikes=0
			//battle.pbDisplay(_INTL("{1} blew away poison spikes!",attacker.pbThis))     
		  }
		  if (attacker.OwnSide.StickyWeb){
			attacker.OwnSide.StickyWeb= false

			//battle.pbDisplay(_INTL("{1} blew away sticky webs!", attacker.pbThis))     
		  }
		}
	  }
	}



	/// <summary>
	/// Attacks 2 rounds in the future. (Doom Desire, Future Sight)
	/// <summary>
	public class PokeBattle_Move_111 : PokeBattle_Move
	{
		public object pbDisplayUseMessage(Battle.Battler attacker){
		if (this.battle.futuresight) return 0;
		return super(attacker)
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (opponent.effects.FutureSight>0){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if (this.battle.futuresight){
		  // Attack hits
		  return super(attacker, opponent, hitnum, alltargets, showanimation)

		}
	/// Attack is launched
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		opponent.effects.FutureSight=3 
		opponent.effects.FutureSightMove=this.id
		opponent.effects.FutureSightUser= attacker.pokemonIndex

		opponent.effects.FutureSightUserPos= attacker.index
		if (id == Moves.FUTURESIGHT){

		  //battle.pbDisplay(_INTL("{1} foresaw an attack!",attacker.pbThis))
		else
		  //battle.pbDisplay(_INTL("{1} chose Doom Desire as its destiny!",attacker.pbThis))
		}
		return 0
	  }

	  public object pbShowAnimation(id, Battle.Battler attacker, Battle.Battler opponent, byte hitnum= 0, byte? alltargets= null, bool showanimation= true){
		if (this.battle.futuresight){
		  return super(id, attacker, opponent,1, alltargets, showanimation) // Hit opponent anim
		}
		return super(id, attacker, opponent, hitnum, alltargets, showanimation)
	  }
	}



	/// <summary>
	/// Increases the user's Defense and Special Defense by 1 stage each. Ups the
	/// user's stockpile by 1 (max. 3). (Stockpile)
	/// <summary>
	public class PokeBattle_Move_112 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (attacker.effects.Stockpile>=3){
		  //battle.pbDisplay(_INTL("{1} can't stockpile any more!",attacker.pbThis))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		attacker.effects.Stockpile+=1
		//battle.pbDisplay(_INTL("{1} stockpiled {2}!",attacker.pbThis,
			attacker.effects.Stockpile))
		showanim=true
		if (attacker.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::DEFENSE,1,attacker,false,self,showanim)
		  attacker.effects.StockpileDef+=1
		  showanim=false
		}
		if (attacker.pbCanIncreaseStatStage? (PBStats::SPDEF, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::SPDEF,1,attacker,false,self,showanim)
		  attacker.effects.StockpileSpDef+=1
		  showanim=false
		}
		return 0
	  }
	}



	/// <summary>
	/// Power is 100 multiplied by the user's stockpile (X). Resets the stockpile to
	/// 0. Decreases the user's Defense and Special Defense by X stages each. (Spit Up)
	/// <summary>
	public class PokeBattle_Move_113 : PokeBattle_Move
	{
		public object pbMoveFailed(Battle.Battler attacker, Battle.Battler opponent){
		return (attacker.effects.Stockpile==0)
	  }

	  public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){
		return 100* attacker.effects.Stockpile
	  }

	  public object pbEffectAfterHit(Battle.Battler attacker, Battle.Battler opponent, turneffects){
		if (!attacker.isFainted() && turneffects.TotalDamage>0){

		  showanim= true
		  if (attacker.effects.StockpileDef>0){
			if (attacker.pbCanReduceStatStage?(PBStats::DEFENSE, attacker,false, self)){

			  attacker.pbReduceStat(PBStats::DEFENSE, attacker.effects.StockpileDef,
				 attacker,false,self,showanim)
			  showanim=false
			}
		  }
		  if (attacker.effects.StockpileSpDef>0){
			if (attacker.pbCanReduceStatStage? (PBStats::SPDEF, attacker,false,self)){
			  attacker.pbReduceStat(PBStats::SPDEF, attacker.effects.StockpileSpDef,
				 attacker,false,self,showanim)
			  showanim=false
			}
		  }

		  attacker.effects.Stockpile=0
		  attacker.effects.StockpileDef=0
		  attacker.effects.StockpileSpDef=0
		  //battle.pbDisplay(_INTL("{1}'s stockpiled effect wore off!",attacker.pbThis))
		}
	  }
	}



	/// <summary>
	/// Heals user depending on the user's stockpile (X). Resets the stockpile to 0.
	/// Decreases the user's Defense and Special Defense by X stages each. (Swallow)
	/// <summary>
	public class PokeBattle_Move_114 : PokeBattle_Move
	{
		public object isHealingMove?
		return true
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		hpgain=0
		case attacker.effects.Stockpile
		when 0
		  //battle.pbDisplay(_INTL("But it failed to swallow a thing!"))
		  return -1
		when 1
		  hpgain=(attacker.TotalHP/4).floor
		when 2
		  hpgain=(attacker.TotalHP/2).floor
		when 3
		  hpgain=attacker.TotalHP
		}
		if (attacker.hp==attacker.TotalHP &&){
		   attacker.effects.StockpileDef==0 &&
		   attacker.effects.StockpileSpDef==0
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)
		if (attacker.pbRecoverHP(hpgain,true)>0){
		  //battle.pbDisplay(_INTL("{1}'s HP was restored.",attacker.pbThis))
		}
		showanim = true
		if (attacker.effects.StockpileDef>0){
		  if (attacker.pbCanReduceStatStage? (PBStats::DEFENSE, attacker,false,self)){
			attacker.pbReduceStat(PBStats::DEFENSE, attacker.effects.StockpileDef,
			   attacker,false,self,showanim)
			showanim=false
		  }
		}
		if (attacker.effects.StockpileSpDef>0){
		  if (attacker.pbCanReduceStatStage? (PBStats::SPDEF, attacker,false,self)){
			attacker.pbReduceStat(PBStats::SPDEF, attacker.effects.StockpileSpDef,
			   attacker,false,self,showanim)
			showanim=false
		  }
		}

		attacker.effects.Stockpile=0
		attacker.effects.StockpileDef=0
		attacker.effects.StockpileSpDef=0
		//battle.pbDisplay(_INTL("{1}'s stockpiled effect wore off!",attacker.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// Fails if user was hit by a damaging move this round. (Focus Punch)
	/// <summary>
	public class PokeBattle_Move_115 : PokeBattle_Move
	{
		public object pbDisplayUseMessage(Battle.Battler attacker){
		if (attacker.lastHPLost>0){
		  //battle.pbDisplayBrief(_INTL("{1} lost its focus and couldn't move!",attacker.pbThis))
		  return -1
		}
		return super(attacker)
	  }
	}



	/// <summary>
	/// Fails if the target didn't chose a damaging move to use this round, or has
	/// already moved. (Sucker Punch)
	/// <summary>
	public class PokeBattle_Move_116 : PokeBattle_Move
	{
		public object pbMoveFailed(Battle.Battler attacker, Battle.Battler opponent){
		if (this.battle.choices[opponent.index][0]!=1 // Didn't choose a move
		oppmove=this.battle.choices[opponent.index][2]) return true;
		if (!oppmove || oppmove.id<=0 || oppmove.pbIsStatus?) return true;
		if (opponent.hasMovedThisRound? && oppmove.function!=0xB0) return true; // Me First
		return false
	  }
	}



	/// <summary>
	/// This round, user becomes the target of attacks that have single targets.
	/// (Follow Me, Rage Powder)
	/// <summary>
	public class PokeBattle_Move_117 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (!this.battle.doublebattle){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		attacker.effects.FollowMe=1
		if (!attacker.pbPartner.isFainted() && attacker.pbPartner.effects.FollowMe>0){
		  attacker.effects.FollowMe=attacker.pbPartner.effects.FollowMe+1
		}
		//battle.pbDisplay(_INTL("{1} became the center of attention!", attacker.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// For 5 rounds, increases gravity on the field. Pokémon cannot become airborne.
	/// (Gravity)
	/// <summary>
	public class PokeBattle_Move_118 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (this.battle.field.Gravity>0){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		this.battle.field.Gravity=5
		for (int i = 0; i < 4; i++){ 
		  poke=this.battle.battlers[i]
		  if (!poke) continue; //next
		  if (PBMoveData.new(poke.effects.TwoTurnAttack).function==0xC9 || // Fly){
			 PBMoveData.new(poke.effects.TwoTurnAttack).function==0xCC || // Bounce
			 PBMoveData.new(poke.effects.TwoTurnAttack).function==0xCE    // Sky Drop
			poke.effects.TwoTurnAttack=0
		  }
		  if (poke.effects.SkyDrop){
			poke.effects.SkyDrop= false

		  }
		  if (poke.effects.MagnetRise>0){

			poke.effects.MagnetRise= 0

		  }
		  if (poke.effects.Telekinesis>0){

			poke.effects.Telekinesis= 0

		  }
		}

		//battle.pbDisplay(_INTL("Gravity intensified!"))
		return 0
	  }
	}



	/// <summary>
	/// For 5 rounds, user becomes airborne. (Magnet Rise)
	/// <summary>
	public class PokeBattle_Move_119 : PokeBattle_Move
	{
		public object unusableInGravity?
		return true
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (attacker.effects.Ingrain ||){
		   attacker.effects.SmackDown ||
		   attacker.effects.MagnetRise>0
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		attacker.effects.MagnetRise=5
		//battle.pbDisplay(_INTL("{1} levitated with electromagnetism!",attacker.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// For 3 rounds, target becomes airborne and can always be hit. (Telekinesis)
	/// <summary>
	public class PokeBattle_Move_11A : PokeBattle_Move
	{
		public object unusableInGravity?
		return true
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (opponent.effects.Ingrain ||){
		   opponent.effects.SmackDown ||
		   opponent.effects.Telekinesis>0
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.effects.Telekinesis=3
		//battle.pbDisplay(_INTL("{1} was hurled into the air!",opponent.pbThis))
		return 0
	  }
	}




	/// <summary>
	/// Hits airborne semi-invulnerable targets. (Sky Uppercut)
	/// <summary>
	public class PokeBattle_Move_11B : PokeBattle_Move
	/// Handled in Battler's pbSuccessCheck, do not edit!
	}



	/// <summary>
	/// Grounds the target while it remains active. (Smack Down, Thousand Arrows)
	/// (Handled in Battler's pbSuccessCheck): Hits some semi-invulnerable targets.
	/// <summary>
	public class PokeBattle_Move_11C : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, Battle.Battler attacker, Battle.Battler opponent){
		if(PBMoveData.new(opponent.effects.TwoTurnAttack).function==0xC9 || // Fly
		   PBMoveData.new(opponent.effects.TwoTurnAttack).function==0xCC || // Bounce
		   PBMoveData.new(opponent.effects.TwoTurnAttack).function==0xCE || // Sky Drop
		   opponent.effects.SkyDrop)
		  return basedmg*2
		}
		return basedmg
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){

		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if (opponent.damagestate.CalcDamage>0 && 
			!opponent.damagestate.Substitute &&
			!opponent.effects.Roost)
		  opponent.effects.SmackDown= true

		  showmsg= (opponent.pbHasType ? (:FLYING) ||
				   opponent.hasWorkingAbility(Abilities.LEVITATE))
		  if(PBMoveData.new(opponent.effects.TwoTurnAttack).function==0xC9 || // Fly
			 PBMoveData.new(opponent.effects.TwoTurnAttack).function==0xCC)    // Bounce
			opponent.effects.TwoTurnAttack=0; showmsg=true
		  }
		  if (opponent.effects.MagnetRise>0)
			opponent.effects.MagnetRise=0; showmsg=true
		  }
		  if (opponent.effects.Telekinesis>0)
			opponent.effects.Telekinesis=0; showmsg=true
		  }
		  //battle.pbDisplay(_INTL("{1} fell straight down!", opponent.pbThis)) if showmsg
		 }
		return ret
	  }
	}



	/// <summary>
	/// Target moves immediately after the user, ignoring priority/speed. (After You)
	/// <summary>
	public class PokeBattle_Move_11D : PokeBattle_Move
	{
		public object pbMoveFailed(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.effects.MoveNext) return true;
		if (this.battle.choices[opponent.index][0]!=1 // Didn't choose a move
		oppmove=this.battle.choices[opponent.index][2]) return true;
		if (!oppmove || oppmove.id<=0) return true;
		if (opponent.hasMovedThisRound?) return true;
		return false
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){

		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.effects.MoveNext=true
		opponent.effects.Quash=false
		//battle.pbDisplay(_INTL("{1} took the kind offer!",opponent.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// Target moves last this round, ignoring priority/speed. (Quash)
	/// <summary>
	public class PokeBattle_Move_11E : PokeBattle_Move
	{
		public object pbMoveFailed(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.effects.Quash) return true;
		if (this.battle.choices[opponent.index][0]!=1 // Didn't choose a move
		oppmove=this.battle.choices[opponent.index][2]) return true;
		if (!oppmove || oppmove.id<=0) return true;
		if (opponent.hasMovedThisRound?) return true;
		return false
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){

		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.effects.Quash=true
		opponent.effects.MoveNext=false
		//battle.pbDisplay(_INTL("{1}'s move was postponed!",opponent.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// For 5 rounds, for each priority bracket, slow Pokémon move before fast ones.
	/// (Trick Room)
	/// <summary>
	public class PokeBattle_Move_11F : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (this.battle.field.TrickRoom>0){
		  this.battle.field.TrickRoom=0
		  //battle.pbDisplay(_INTL("{1} reverted the dimensions!",attacker.pbThis))
		else
		  pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		  this.battle.field.TrickRoom=5
		  //battle.pbDisplay(_INTL("{1} twisted the dimensions!",attacker.pbThis))
		}
		return 0
	  }
	}



	/// <summary>
	/// User switches places with its ally. (Ally Switch)
	/// <summary>
	public class PokeBattle_Move_120 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if(!this.battle.doublebattle ||
		   !attacker.pbPartner || 
		   attacker.pbPartner.isFainted()){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		a=this.battle.battlers[attacker.index]
		b = this.battle.battlers[attacker.pbPartner.index]

		temp=a; a=b; b=temp
		// Swap effects that point at the position rather than the Pokémon
		// NOT PerishSongUser (no need to swap), Attract, MultiTurnUser
		effectstoswap =[PBEffects::BideTarget,
					   PBEffects::CounterTarget,
					   PBEffects::LeechSeed,
					   PBEffects::LockOnPos,
					   PBEffects::MeanLook,
					   PBEffects::MirrorCoatTarget]
		foreach (var i in effectstoswap){ 
		  a.effects[i], b.effects[i]= b.effects[i], a.effects[i]
		}

		attacker.pbUpdate(true)

		opponent.pbUpdate(true)
		//battle.pbDisplay(_INTL("{1} and {2} switched places!",opponent.pbThis,attacker.pbThis(true)))
	  }
	}



	/// <summary>
	/// Target's Attack is used instead of user's Attack for this move's calculations.
	/// (Foul Play)
	/// <summary>
	public class PokeBattle_Move_121 : PokeBattle_Move
	{
		// Handled in superclass public object pbCalcDamage, do not edit!
	}



	/// <summary>
	/// Target's Defense is used instead of its Special Defense for this move's
	/// calculations. (Psyshock, Psystrike, Secret Sword)
	/// <summary>
	public class PokeBattle_Move_122 : PokeBattle_Move
	{
		// Handled in superclass public object pbCalcDamage, do not edit!
	}



	/// <summary>
	/// Only damages Pokémon that share a type with the user. (Synchronoise)
	/// <summary>
	public class PokeBattle_Move_123 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (!opponent.pbHasType? (attacker.type1) &&){
		   !opponent.pbHasType? (attacker.type2) &&
		   !opponent.pbHasType? (attacker.effects.Type3)
		   //battle.pbDisplay(_INTL("{1} was unaffected!", opponent.pbThis))
		  return -1
		}
		return super(attacker, opponent, hitnum, alltargets, showanimation)
	  }
	}



	/// <summary>
	/// For 5 rounds, swaps all battlers' base Defense with base Special Defense.
	/// (Wonder Room)
	/// <summary>
	public class PokeBattle_Move_124 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (this.battle.field.WonderRoom>0){
		  this.battle.field.WonderRoom=0
		  //battle.pbDisplay(_INTL("Wonder Room wore off, and the Defense and Sp. Def stats returned to normal!"))
		else
		  pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		  this.battle.field.WonderRoom=5
		  //battle.pbDisplay(_INTL("It created a bizarre area in which the Defense and Sp. Def stats are swapped!"))
		}
		return 0
	  }
	}



	/// <summary>
	/// Fails unless user has already used all other moves it knows. (Last Resort)
	/// <summary>
	public class PokeBattle_Move_125 : PokeBattle_Move
	{
		public object pbMoveFailed(Battle.Battler attacker, Battle.Battler opponent){
		counter = 0; nummoves=0
		for move in attacker.moves
		  if (move.id<=0) continue; //next
		  counter+=1 if move.id!=this.id && !attacker.movesUsed.include? (move.id)
		   nummoves+=1
		}
		return counter!=0 || nummoves==1
	  }
	}



	#===============================================================================
	/// NOTE: Shadow moves use function codes 126-132 inclusive.
	#===============================================================================



	/// <summary>
	/// Does absolutely nothing. (Hold Hands)
	/// <summary>
	public class PokeBattle_Move_133 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (!this.battle.doublebattle ||){
		   !attacker.pbPartner || attacker.pbPartner.isFainted()
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)
		return 0
	  }
	}



	/// <summary>
	/// Does absolutely nothing. Shows a special message. (Celebrate)
	/// <summary>
	public class PokeBattle_Move_134 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		//battle.pbDisplay(_INTL("Congratulations, {1}!",this.battle.pbGetOwner(attacker.index).name))
		return 0
	  }
	}



	/// <summary>
	/// Freezes the target. (Freeze-Dry)
	/// (Superclass's pbTypeModifier): Effectiveness against Water-type is 2x.
	/// <summary>
	public class PokeBattle_Move_135 : PokeBattle_Move
	{
		public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		if (opponent.pbCanFreeze? (attacker,false,self)){
		  opponent.pbFreeze
		}
	  }
	}



	/// <summary>
	/// Increases the user's Defense by 1 stage for each target hit. (Diamond Storm)
	/// <summary>
	public class PokeBattle_Move_136 : PokeBattle_Move_01D
	{
		// No difference to function code 01D. It may need to be separate in future.
	}



	/// <summary>
	/// Increases the user's and its ally's Defense and Special Defense by 1 stage
	/// each, if they have Plus or Minus. (Magnetic Flux)
	/// <summary>
	public class PokeBattle_Move_137 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		didsomething=false
		foreach (var i in [attacker, attacker.pbPartner]){ 
		if (!i || i.isFainted()) continue; //next
		if (!i.hasWorkingAbility(Abilities.PLUS) && !i.hasWorkingAbility(Abilities.MINUS)) continue; //next 
		  if (!i.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,false,self) &&
				  !i.pbCanIncreaseStatStage? (PBStats::SPDEF, attacker,false,self)) continue; //next 
		  pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation) if !didsomething
			   didsomething = true

		  showanim=true
		  if (i.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,false,self)){
			i.pbIncreaseStat(PBStats::DEFENSE,1,attacker,false,self,showanim)
			showanim=false;
		  }
		  if (i.pbCanIncreaseStatStage? (PBStats::SPDEF, attacker,false,self)){
			i.pbIncreaseStat(PBStats::SPDEF,1,attacker,false,self,showanim)
			showanim=false;
		  }
		}
		if (!didsomething){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		return 0
	  }
	}



	/// <summary>
	/// Increases ally's Special Defense by 1 stage. (Aromatic Mist)
	/// <summary>
	public class PokeBattle_Move_138 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (!this.battle.doublebattle || !opponent ||){
		   !opponent.pbCanIncreaseStatStage? (PBStats::SPDEF, attacker,false,self)
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=attacker.pbIncreaseStat(PBStats::SPDEF,1,attacker,false,self)
		return ret? 0 : -1
	  }
	}



	/// <summary>
	/// Decreases the target's Attack by 1 stage. Always hits. (Play Nice)
	/// <summary>
	public class PokeBattle_Move_139 : PokeBattle_Move
	{
		public object pbAccuracyCheck(Battle.Battler attacker, Battle.Battler opponent){
		return true
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (!opponent.pbCanReduceStatStage? (PBStats::ATTACK, attacker,true,self)) return -1;
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=opponent.pbReduceStat(PBStats::ATTACK,1,attacker,false,self)
		return ret ? 0 : -1;
	  }
	}



	/// <summary>
	/// Decreases the target's Attack and Special Attack by 1 stage each. (Noble Roar)
	/// <summary>
	public class PokeBattle_Move_13A : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true)
		// Replicates public object pbCanReduceStatStage? so that certain messages aren't shown
		// multiple times
		if(opponent.effects.Substitute>0 && !ignoresSubstitute? (attacker)){
		   //battle.pbDisplay(_INTL("{1}'s attack missed!", attacker.pbThis))
		  return -1
		}
		if(opponent.pbTooLow? (PBStats::ATTACK) &&
		   opponent.pbTooLow? (PBStats::SPATK)){
		   //battle.pbDisplay(_INTL("{1}'s stats won't go any lower!", opponent.pbThis))
		  return -1
		}
		if(opponent.OwnSide.Mist>0){
		  //battle.pbDisplay(_INTL("{1} is protected by Mist!",opponent.pbThis))
		  return -1
		}
		if(!attacker.hasMoldBreaker){
		  if(opponent.hasWorkingAbility(Abilities.CLEARBODY) ||
			 opponent.hasWorkingAbility(Abilities.WHITESMOKE)){
			//battle.pbDisplay(_INTL("{1}'s {2} prevents stat loss!",opponent.pbThis,
			//   PBAbilities.getName(opponent.ability)))
			return -1
		  }
		}

		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=-1; showanim=true
		if (!attacker.hasMoldBreaker && opponent.hasWorkingAbility(Abilities.HYPERCUTTER)
		  abilityname=PBAbilities.getName(opponent.ability)){
		  //battle.pbDisplay(_INTL("{1}'s {2} prevents Attack loss!", opponent.pbThis, abilityname))
		}
		else if (opponent.pbReduceStat(PBStats::ATTACK,1, attacker,false, self, showanim)){
		  ret=0; showanim=false
		}
		if (opponent.pbReduceStat(PBStats::SPATK,1,attacker,false,self,showanim)){
		  ret=0; showanim=false
		}
		return ret
	  }
	}



	/// <summary>
	/// Decreases the target's Defense by 1 stage. Always hits. (Hyperspace Fury)
	/// <summary>
	public class PokeBattle_Move_13B : PokeBattle_Move
	{
		public object pbMoveFailed(Battle.Battler attacker, Battle.Battler opponent){
		if (attacker.Species == Pokemons.HOOPA) return true;
		if (attacker.form!=1) return true;
		return false
	  }

	  public object pbAccuracyCheck(Battle.Battler attacker, Battle.Battler opponent){
		return true
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		if (opponent.pbCanReduceStatStage? (PBStats::DEFENSE, attacker,false,self)){
		  opponent.pbReduceStat(PBStats::DEFENSE,1,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Decreases the target's Special Attack by 1 stage. Always hits. (Confide)
	/// <summary>
	public class PokeBattle_Move_13C : PokeBattle_Move
	{
		public object pbAccuracyCheck(Battle.Battler attacker, Battle.Battler opponent){
		return true
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (!opponent.pbCanReduceStatStage? (PBStats::SPATK, attacker,true,self)) return -1;
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=opponent.pbReduceStat(PBStats::SPATK,1,attacker,false,self)
		return ret? 0 : -1
	  }
	}



	/// <summary>
	/// Decreases the target's Special Attack by 2 stages. (Eerie Impulse)
	/// <summary>
	public class PokeBattle_Move_13D : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (pbIsDamaging()) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (pbTypeImmunityByAbility(pbType(this.type, attacker, opponent), attacker, opponent)) return -1;
		if (!opponent.pbCanReduceStatStage? (PBStats::SPATK, attacker,true,self)) return -1;
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=opponent.pbReduceStat(PBStats::SPATK,2,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent){
		if (opponent.damagestate.Substitute) return;
		if (opponent.pbCanReduceStatStage? (PBStats::SPATK, attacker,false,self)){
		  opponent.pbReduceStat(PBStats::SPATK,2,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Increases the Attack and Special Attack of all Grass-type Pokémon on the field
	/// by 1 stage each. Doesn't affect airborne Pokémon. (Rototiller)
	/// <summary>
	public class PokeBattle_Move_13E : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		didsomething=false
		foreach (var i in [attacker, attacker.pbPartner, attacker.pbOpposing1, attacker.pbOpposing2]){ 
	 if (!i || i.isFainted()) continue; //next
	 if (!i.hasType(Types.GRASS)) continue; //next
		  if (i.isAirborne? (attacker.hasMoldBreaker)) continue; //next
		    if (!i.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self) &&
				  !i.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,false,self)) continue;//next
		  pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation) if !didsomething
			   didsomething = true

		  showanim=true
		  if (i.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self)){
			i.pbIncreaseStat(PBStats::ATTACK,1,attacker,false,self,showanim)
			showanim=false
		  }
		  if (i.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,false,self)){
			i.pbIncreaseStat(PBStats::SPATK,1,attacker,false,self,showanim)
			showanim=false
		  }
		}
		if (!didsomething){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		return 0
	  }
	}



	/// <summary>
	/// Increases the Defense of all Grass-type Pokémon on the field by 1 stage each.
	/// (Flower Shield)
	/// <summary>
	public class PokeBattle_Move_13F : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		didsomething=false
		foreach (var i in [attacker, attacker.pbPartner, attacker.pbOpposing1, attacker.pbOpposing2]){ 
	if (!i || i.isFainted()) continue; //next
	if (!i.hasType(Types.GRASS)) continue; //next
		  if (!i.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self)) continue; //next
		  pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation) if !didsomething
			   didsomething = true

		  showanim=true
		  if (i.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,false,self)){
			i.pbIncreaseStat(PBStats::DEFENSE,1,attacker,false,self,showanim)
			showanim=false
		  }
		}
		if (!didsomething){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		return 0
	  }
	}



	/// <summary>
	/// Decreases the Attack, Special Attack and Speed of all poisoned opponents by 1
	/// stage each. (Venom Drench)
	/// <summary>
	public class PokeBattle_Move_140 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		didsomething=false
		foreach (var i in [attacker.pbOpposing1, attacker.pbOpposing2]){ 
	if (!i || i.isFainted()) continue; //next
	if (!i.status==PBStatuses::POISON) continue; //next
	if (!i.pbCanReduceStatStage? (PBStats::ATTACK, attacker,false,self) &&) continue; //next
				  !i.pbCanReduceStatStage? (PBStats::SPATK, attacker,false,self) &&
				  !i.pbCanReduceStatStage? (PBStats::SPEED, attacker,false,self)
		  pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation) if !didsomething
			   didsomething = true

		  showanim=true
		  if (i.pbCanReduceStatStage? (PBStats::ATTACK, attacker,false,self)){
			i.pbReduceStat(PBStats::ATTACK,1,attacker,false,self,showanim)
			showanim=false
		  }
		  if (i.pbCanReduceStatStage? (PBStats::SPATK, attacker,false,self)){
			i.pbReduceStat(PBStats::SPATK,1,attacker,false,self,showanim)
			showanim=false
		  }
		  if (i.pbCanReduceStatStage? (PBStats::SPEED, attacker,false,self)){
			i.pbReduceStat(PBStats::SPEED,1,attacker,false,self,showanim)
			showanim=false
		  }
		}
		if (!didsomething){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		return 0
	  }
	}



	/// <summary>
	/// Reverses all stat changes of the target. (Topsy-Turvy)
	/// <summary>
	public class PokeBattle_Move_141 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		nonzero=false
		foreach (var i in [PBStats::ATTACK, PBStats::DEFENSE, PBStats::SPEED, 
				  PBStats::SPATK, PBStats::SPDEF, PBStats::ACCURACY, PBStats::EVASION]){
		  if (opponent.stages[i]!=0){
			nonzero=true; break
		  }
		}
		if (!nonzero){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)
		foreach (var i in [PBStats::ATTACK, PBStats::DEFENSE, PBStats::SPEED,
				  PBStats::SPATK, PBStats::SPDEF, PBStats::ACCURACY, PBStats::EVASION]){ 
	opponent.stages[i]*=-1
		}
		//battle.pbDisplay(_INTL("{1}'s stats were reversed!", opponent.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// Gives target the Ghost type. (Trick-or-Treat)
	/// <summary>
	public class PokeBattle_Move_142 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (opponent.effects.Substitute>0 && !ignoresSubstitute? (attacker)) ||
		   !hasConst? (PBTypes,:GHOST) || opponent.hasType(Types.GHOST) ||
		   isConst? (opponent.ability, PBAbilities,:MULTITYPE)
		  //battle.pbDisplay(_INTL("But it failed!"))  
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.effects.Type3=Types.GHOST

		typename=PBTypes.getName(Types.GHOST)
		//battle.pbDisplay(_INTL("{1} transformed into the {2} type!",opponent.pbThis,typename))
		return 0
	  }
	}



	/// <summary>
	/// Gives target the Grass type. (Forest's Curse)
	/// <summary>
	public class PokeBattle_Move_143 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (opponent.effects.Substitute>0 && !ignoresSubstitute? (attacker)){
		   //battle.pbDisplay(_INTL("But it failed!"))  
		  return -1
		}
		if (pbTypeImmunityByAbility(pbType(this.type, attacker, opponent), attacker, opponent)) return -1;
		if (opponent.effects.LeechSeed>=0){
		  //battle.pbDisplay(_INTL("{1} evaded the attack!",opponent.pbThis))
		  return -1
		}
		if (!hasConst? (PBTypes,:GRASS) || opponent.hasType(Types.GRASS) ||){
		   isConst? (opponent.ability, PBAbilities,:MULTITYPE)
		  //battle.pbDisplay(_INTL("But it failed!"))  
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.effects.Type3=Types.GRASS

		typename=PBTypes.getName(Types.GRASS)
		//battle.pbDisplay(_INTL("{1} transformed into the {2} type!",opponent.pbThis,typename))
		return 0
	  }
	}



	/// <summary>
	/// Damage is multiplied by Flying's effectiveness against the target. Does double
	/// damage and has perfect accuracy if the target is Minimized. (Flying Press)
	/// <summary>
	public class PokeBattle_Move_144 : PokeBattle_Move
	{
		public object pbModifyDamage(damagemult, Battle.Battler attacker, Battle.Battler opponent){
		type = Types.FLYING || -1
		if (type>=0){
		  mult=PBTypes.getCombinedEffectiveness(type,
			 opponent.type1, opponent.type2, opponent.effects.Type3)
		  return (int)Math.Round((damagemult* mult)/8)
		 }
		return damagemult
	  }

	  public object tramplesMinimize? (param=1){
		if (param==1 && USENEWBATTLEMECHANICS) return true; // Perfect accuracy
		if (param==2) return true; // Double damage
		return false
	  }
	}



	/// <summary>
	/// Target's moves become Electric-type for the rest of the round. (Electrify)
	/// <summary>
	public class PokeBattle_Move_145 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (pbTypeImmunityByAbility(pbType(this.type, attacker, opponent), attacker, opponent)) return -1;
		if (opponent.effects.Electrify){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if(this.battle.choices[opponent.index][0]!=1 || // Didn't choose a move
		   !this.battle.choices[opponent.index][2] ||
		   this.battle.choices[opponent.index][2].id<=0 ||
		   opponent.hasMovedThisRound?){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.effects.Electrify=true
		//battle.pbDisplay(_INTL("{1} was electrified!",opponent.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// All Normal-type moves become Electric-type for the rest of the round.
	/// (Ion Deluge)
	/// <summary>
	public class PokeBattle_Move_146 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		unmoved=false
		for poke in this.battle.battlers
		  if (poke.index==attacker.index) continue; //next
		  if (this.battle.choices[poke.index][0]==1 && // Chose a move){
			 !poke.hasMovedThisRound?
			unmoved = true; break
		  }
		}
		if (!unmoved || this.battle.field.IonDeluge){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		this.battle.field.IonDeluge=true
		//battle.pbDisplay(_INTL("The Ion Deluge started!"))
		return 0
	  }
	}



	/// <summary>
	/// Always hits. (Hyperspace Hole)
	/// TODO: Hits through various shields.
	/// <summary>
	public class PokeBattle_Move_147 : PokeBattle_Move
	{
		public object pbAccuracyCheck(Battle.Battler attacker, Battle.Battler opponent){
		return true
	  }
	}


	/// <summary>
	/// Powders the foe. This round, if it uses a Fire move, it loses 1/4 of its max
	/// HP instead. (Powder)
	/// <summary>
	public class PokeBattle_Move_148 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (opponent.effects.Powder){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		opponent.effects.Powder= true

		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)
		//battle.pbDisplay(_INTL("{1} is covered in powder!", attacker.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// This round, the user's side is unaffected by damaging moves. (Mat Block)
	/// <summary>
	public class PokeBattle_Move_149 : PokeBattle_Move
	{
		public object pbMoveFailed(Battle.Battler attacker, Battle.Battler opponent){
		return (attacker.turncount>1)
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){

		attacker.OwnSide.MatBlock=true
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		//battle.pbDisplay(_INTL("{1} intends to flip up a mat and block incoming attacks!",attacker.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// User's side is protected against status moves this round. (Crafty Shield)
	/// <summary>
	public class PokeBattle_Move_14A : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (attacker.OwnSide.CraftyShield){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		unmoved = false
		for poke in this.battle.battlers
		  if (poke.index==attacker.index) continue; //next
		  if (this.battle.choices[poke.index][0]==1 && // Chose a move){
			 !poke.hasMovedThisRound?
			unmoved = true; break
		  }
		}
		if (!unmoved){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		attacker.OwnSide.CraftyShield=true
		if (!this.battle.pbIsOpposing? (attacker.index)){
		   //battle.pbDisplay(_INTL("Crafty Shield protected your team!"))
		else
		  //battle.pbDisplay(_INTL("Crafty Shield protected the opposing team!"))
		}
		return 0
	  }
	}



	/// <summary>
	/// User is protected against damaging moves this round. Decreases the Attack of
	/// the user of a stopped contact move by 2 stages. (King's Shield)
	/// <summary>
	public class PokeBattle_Move_14B : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (attacker.effects.KingsShield){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		ratesharers =[
		   0xAA,   // Detect, Protect
		   0xAB,   // Quick Guard
		   0xAC,   // Wide Guard
		   0xE8,   // Endure
		   0x14B,  // King's Shield
		   0x14C   // Spiky Shield
		]
		if (!ratesharers.include? (PBMoveData.new(attacker.lastMoveUsed).function)){
		  attacker.effects.ProtectRate=1
		}
		unmoved = false
		for poke in this.battle.battlers
		  if (poke.index==attacker.index) continue; //next
		  if (this.battle.choices[poke.index][0]==1 && // Chose a move){
			 !poke.hasMovedThisRound?
			unmoved = true; break
		  }
		}
		if (!unmoved ||){
		   (!USENEWBATTLEMECHANICS &&
		   this.battle.pbRandom(65536)>=(65536/attacker.effects.ProtectRate).floor)
		  attacker.effects.ProtectRate=1
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		attacker.effects.KingsShield=true
		attacker.effects.ProtectRate*=2
		//battle.pbDisplay(_INTL("{1} protected itself!",attacker.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// User is protected against moves that target it this round. Damages the user of
	/// a stopped contact move by 1/8 of its max HP. (Spiky Shield)
	/// <summary>
	public class PokeBattle_Move_14C : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (attacker.effects.SpikyShield){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		ratesharers =[
		   0xAA,   // Detect, Protect
		   0xAB,   // Quick Guard
		   0xAC,   // Wide Guard
		   0xE8,   // Endure
		   0x14B,  // King's Shield
		   0x14C   // Spiky Shield
		]
		if (!ratesharers.include? (PBMoveData.new(attacker.lastMoveUsed).function)){
		  attacker.effects.ProtectRate=1
		}
		unmoved = false
		for poke in this.battle.battlers
		  if (poke.index==attacker.index) continue; //next
		  if (this.battle.choices[poke.index][0]==1 && // Chose a move){
			 !poke.hasMovedThisRound?
			unmoved = true; break
		  }
		}
		if (!unmoved ||){
		   this.battle.pbRandom(65536)>=(65536/attacker.effects.ProtectRate).floor
		  attacker.effects.ProtectRate= 1

		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		attacker.effects.SpikyShield=true
		attacker.effects.ProtectRate*=2
		//battle.pbDisplay(_INTL("{1} protected itself!",attacker.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// Two turn attack. Skips first turn, attacks second turn. (Phantom Force)
	/// Is invulnerable during use.
	/// Ignores target's Detect, King's Shield, Mat Block, Protect and Spiky Shield
	/// this round. If successful, negates them this round.
	/// Does double damage and has perfect accuracy if the target is Minimized.
	/// <summary>
	public class PokeBattle_Move_14D : PokeBattle_Move
	{
		public object pbTwoTurnAttack(Battle.Battler attacker)
		this.immediate = false
		if (!this.immediate && attacker.hasWorkingItem(Items.POWERHERB)){
		  this.immediate=true
		}
		if (this.immediate) return false;
		return attacker.effects.TwoTurnAttack==0
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (this.immediate || attacker.effects.TwoTurnAttack>0){
		  pbShowAnimation(this.id, attacker, opponent,1, alltargets, showanimation) // Charging anim
		  //battle.pbDisplay(_INTL("{1} vanished instantly!",attacker.pbThis))
		}
		if (this.immediate){
		  this.battle.pbCommonAnimation("UseItem", attacker, null)

		  //battle.pbDisplay(_INTL("{1} became fully charged due to its Power Herb!",attacker.pbThis))
		  attacker.pbConsumeItem
		}
		if (attacker.effects.TwoTurnAttack>0) return 0;
		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if (ret>0){
		  opponent.effects.ProtectNegation=true
		  opponent.OwnSide.CraftyShield=false
		}
		return ret
	  }

	  public object tramplesMinimize? (param=1){
		if (param==1 && USENEWBATTLEMECHANICS) return true; // Perfect accuracy
		if (param==2) return true; // Double damage
		return false
	  }
	}



	/// <summary>
	/// Two turn attack. Skips first turn, increases the user's Special Attack,
	/// Special Defense and Speed by 2 stages each second turn. (Geomancy)
	/// <summary>
	public class PokeBattle_Move_14E : PokeBattle_Move
	{
		public object pbTwoTurnAttack(Battle.Battler attacker)
		this.immediate = false
		if (!this.immediate && attacker.hasWorkingItem(Items.POWERHERB)){
		  this.immediate=true
		}
		if (this.immediate) return false;
		return attacker.effects.TwoTurnAttack==0
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true){
		if (this.immediate || attacker.effects.TwoTurnAttack>0){
		  pbShowAnimation(this.id, attacker, opponent,1, alltargets, showanimation) // Charging anim
		  //battle.pbDisplay(_INTL("{1} is absorbing power!",attacker.pbThis))
		}
		if (this.immediate){
		  this.battle.pbCommonAnimation("UseItem", attacker, null)

		  //battle.pbDisplay(_INTL("{1} became fully charged due to its Power Herb!",attacker.pbThis))
		  attacker.pbConsumeItem
		}
		if (attacker.effects.TwoTurnAttack>0) return 0;
		if (!attacker.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,false,self) &&){
		   !attacker.pbCanIncreaseStatStage? (PBStats::SPDEF, attacker,false,self) &&
		   !attacker.pbCanIncreaseStatStage? (PBStats::SPEED, attacker,false,self)
		  //battle.pbDisplay(_INTL("{1}'s stats won't go any higher!",attacker.pbThis))
		  return -1
		}
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)

		showanim=true
		if (attacker.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::SPATK,2,attacker,false,self,showanim)
		  showanim=false
		}
		if (attacker.pbCanIncreaseStatStage? (PBStats::SPDEF, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::SPDEF,2,attacker,false,self,showanim)
		  showanim=false
		}
		if (attacker.pbCanIncreaseStatStage? (PBStats::SPEED, attacker,false,self)){
		  attacker.pbIncreaseStat(PBStats::SPEED,2,attacker,false,self,showanim)
		  showanim=false
		}
		return 0
	  }
	}



	/// <summary>
	/// User gains 3/4 the HP it inflicts as damage. (Draining Kiss, Oblivion Wing)
	/// <summary>
	public class PokeBattle_Move_14F : PokeBattle_Move
	{
		public object isHealingMove?
		return USENEWBATTLEMECHANICS
	  }

	  public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if (opponent.damagestate.CalcDamage>0){
		  hpgain= (int)Math.Round(opponent.damagestate.HpLost*3/4)
		  if (opponent.hasWorkingAbility(Abilities.LIQUIDOOZE)){
			attacker.pbReduceHP(hpgain,true)
			//battle.pbDisplay(_INTL("{1} sucked up the liquid ooze!",attacker.pbThis))
		  else if attacker.effects.HealBlock==0

			hpgain= (hpgain * 1.3f).floor if attacker.hasWorkingItem(Items.BIGROOT)

			attacker.pbRecoverHP(hpgain,true)
			//battle.pbDisplay(_INTL("{1} had its energy drained!",opponent.pbThis))
		  }
		}
		return ret
	  }
	}



	/// <summary>
	/// If this move KO's the target, increases the user's Attack by 2 stages.
	/// (Fell Stinger)
	/// <summary>
	public class PokeBattle_Move_150 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if (opponent.damagestate.CalcDamage>0 && opponent.isFainted()){
		  if (attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self)){
			attacker.pbIncreaseStat(PBStats::ATTACK,2,attacker,false,self)
		  }
		}
		return ret
	  }
	}



	/// <summary>
	/// Decreases the target's Attack and Special Attack by 1 stage each. Then, user
	/// switches out. Ignores trapping moves. (Parting Shot)
	/// TODO: Pursuit should interrupt this move.
	/// <summary>
	public class PokeBattle_Move_151 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		ret=-1
		pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation)
		if (!self.isSoundBased? ||){
		   attacker.hasMoldBreaker || !opponent.hasWorkingAbility(Abilities.SOUNDPROOF)
		  showanim=true
		  if (opponent.pbReduceStat(PBStats::ATTACK,1,attacker,false,self,showanim)){
			showanim=false; ret=0
		  }
		  if (opponent.pbReduceStat(PBStats::SPATK,1,attacker,false,self,showanim)){
			showanim=false; ret=0
		  }
		}
		if (!attacker.isFainted() &&){
		   this.battle.pbCanChooseNonActive? (attacker.index) &&
		   !this.battle.pbAllFainted? (this.battle.pbParty(opponent.index))
		  attacker.effects.Uturn=true; ret=0
		}
		return ret
	  }
	}



	/// <summary>
	/// No Pokémon can switch out or flee until the end of the next round, as long as
	/// the user remains active. (Fairy Lock)
	/// <summary>
	public class PokeBattle_Move_152 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (this.battle.field.FairyLock>0){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		this.battle.field.FairyLock=2
		//battle.pbDisplay(_INTL("No one will be able to run away during the next turn!"))
		return 0
	  }
	}



	/// <summary>
	/// Entry hazard. Lays stealth rocks on the opposing side. (Sticky Web)
	/// <summary>
	public class PokeBattle_Move_153 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (attacker.pbOpposingSide.effects.StickyWeb){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		attacker.pbOpposingSide.effects.StickyWeb=true
		if (!this.battle.pbIsOpposing? (attacker.index)){
		   //battle.pbDisplay(_INTL("A sticky web has been laid out beneath the opposing team's feet!"))
		else
		  //battle.pbDisplay(_INTL("A sticky web has been laid out beneath your team's feet!"))
		}
		return 0
	  }
	}



	/// <summary>
	/// For 5 rounds, creates an electric terrain which boosts Electric-type moves and
	/// prevents Pokémon from falling asleep. Affects non-airborne Pokémon only.
	/// (Electric Terrain)
	/// <summary>
	public class PokeBattle_Move_154 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (this.battle.field.ElectricTerrain>0){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		this.battle.field.GrassyTerrain=0
		this.battle.field.MistyTerrain=0
		this.battle.field.ElectricTerrain=5
		//battle.pbDisplay(_INTL("An electric current runs across the battlefield!"))
		return 0
	  }
	}



	/// <summary>
	/// For 5 rounds, creates a grassy terrain which boosts Grass-type moves and heals
	/// Pokémon at the end of each round. Affects non-airborne Pokémon only.
	/// (Grassy Terrain)
	/// <summary>
	public class PokeBattle_Move_155 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (this.battle.field.GrassyTerrain>0){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		this.battle.field.ElectricTerrain=0
		this.battle.field.MistyTerrain=0
		this.battle.field.GrassyTerrain=5
		//battle.pbDisplay(_INTL("Grass grew to cover the battlefield!"))
		return 0
	  }
	}



	/// <summary>
	/// For 5 rounds, creates a misty terrain which weakens Dragon-type moves and
	/// protects Pokémon from status problems. Affects non-airborne Pokémon only.
	/// (Misty Terrain)
	/// <summary>
	public class PokeBattle_Move_156 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (this.battle.field.MistyTerrain>0){
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		this.battle.field.ElectricTerrain=0
		this.battle.field.GrassyTerrain=0
		this.battle.field.MistyTerrain=5
		//battle.pbDisplay(_INTL("Mist swirled about the battlefield!"))
		return 0
	  }
	}



	/// <summary>
	/// Doubles the prize money the player gets after winning the battle. (Happy Hour)
	/// </summary>
	public class PokeBattle_Move_157 : PokeBattle_Move
	{
		public object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum=0, byte? alltargets=null, bool showanimation=true){
		if (this.battle.pbIsOpposing? (attacker.index) || this.battle.doublemoney){
		   //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation)

		this.battle.doublemoney=true
		//battle.pbDisplay(_INTL("Everyone is caught up in the happy atmosphere!"))
		return 0
	  }
	}



	/// <summary>
	/// Fails unless user has consumed a berry at some point. (Belch)
	/// </summary>
	public class PokeBattle_Move_158 : PokeBattle_Move
	{
		public object pbMoveFailed(Battle.Battler attacker, Battle.Battler opponent){
		return !attacker.pokemon || !attacker.pokemon.belch
	  }
	}



	//===============================================================================
	// NOTE: If you're inventing new move effects, use function code 159 and onwards.
	//===============================================================================
	*/

}

public abstract class PokeBattle_Move : IPokeBattle_Move
{
	public Battle battle { get; set; }
	public Battle.InBattleMove thismove { get; set; }
	public bool IsDamaging()
	{
		return true;
	}
	public bool pbIsDamaging()
	{
		return true;
	}

	public virtual object pbEffect(Battle.Battler attacker, Battle.Battler opponent, int hitnum = 0, byte? alltargets = null, bool showanimation = true)
	{
		return null;
	}
	public virtual object pbEffectMessages(Battle.Battler attacker, Battle.Battler opponent, bool ignoretype = false)
	{
		return null;
	}
	public virtual object pbCalcDamage(Battle.Battler attacker, Battle.Battler opponent)
	{
		return null;
	}
	public virtual object pbCalcDamage(Battle.Battler attacker, Battle.Battler opponent, bool somethinghere) //ToDo: Fix this
	{
		return null;
	}
	public virtual object pbShowAnimation(int id, Battle.Battler attacker, Battle.Battler opponent, byte hitnum= 0, byte? alltargets= null, bool showanimation= true)
	{
		return null;
	}
}

public interface IPokeBattle_Move
{

}
