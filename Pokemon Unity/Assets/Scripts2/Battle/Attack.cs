using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.ComponentModel.DataAnnotations;
using PokemonUnity;
using PokemonUnity.Pokemon;
using PokemonUnity.Effects;
using PokemonUnity.Item;
using PokemonUnity.Move;

public class Function
{
	
	/// <summary>
	/// Superclass that handles moves using a non-existent function code.
	/// Damaging moves just do damage with no additional effect.
	/// Non-damaging moves always fail.
	/// <summary>
	public class PokeBattle_UnimplementedMove : PokeBattle_Move
	{
		public override object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
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

	/*

	/// <summary>
	/// Superclass for a failed move. Always fails.
	/// This class is unused.
	/// <summary>
	public class PokeBattle_FailedMove : PokeBattle_Move
	{
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
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
		public object initialize(battle, move)
		{

			battle     = battle;
			basedamage = 40;

			type       = -1;
			accuracy   = 100;
			pp         = -1;
			addlEffect = 0;
			target     = 0;
			priority   = 0;
			flags      = 0;
			thismove   = move;
			name = "";

			id         = 0;
		}

		public bool IsPhysical(){ return true; }
		public bool IsSpecial(){ return false; }

		public object pbCalcDamage(attacker, opponent){
    		return super(attacker, opponent,
	   			PokeBattle_Move::NOCRITICAL|PokeBattle_Move::SELFCONFUSE|PokeBattle_Move::NOTYPE|PokeBattle_Move::NOWEIGHTING);
  		}

 		public object pbEffectMessages(attacker, opponent, ignoretype= false){
    		return super(attacker, opponent,true);
  		}
	}



	/// <summary>
	/// Implements the move Struggle.
	/// For cases where the real move named Struggle is not defined.
	/// <summary>
	public class PokeBattle_Struggle : PokeBattle_Move
	{
		public object initialize(battle, move)
		{

			id         = -1;    // doesn't work if 0
			battle     = battle;
			name = _INTL("Struggle")

			basedamage = 50;
			type       = -1;
			accuracy   = 0;
			addlEffect = 0;
			target     = 0;
			priority   = 0;
			flags      = 0;
			thismove   = null;   // not associated with a move
			pp		   = -1;

			totalpp    = 0;
			if (move)
			{
				id = move.id;

				name = PBMoves.getName(id);
			}
		}

		public bool IsPhysical(){ return true; }
		public bool IsSpecial(){ return false; }

		public object pbEffectAfterHit(Pokemon attacker, Pokemon opponent, turneffects)
		{
			if (!attacker.fainted? && turneffects[PBEffects::TotalDamage]>0)
			{
				attacker.pbReduceHP((attacker.totalhp / 4.0).round);
				//battle.pbDisplay(_INTL("{1} is damaged by recoil!", attacker.pbThis));
			}
		}

		public object pbCalcDamage(Pokemon attacker, Pokemon opponent)
		{
			return super(attacker, opponent, PokeBattle_Move::IGNOREPKMNTYPES);
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

		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		{
			//pbShowAnimation(id, attacker, null, hitnum, alltargets, showanimation);
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if (pbIsDamaging?) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (pbTypeImmunityByAbility(pbType(@type, attacker, opponent), attacker, opponent)) return -1;
		if opponent.pbCanSleep? (attacker,true,self)
		  pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		  opponent.pbSleep
		  return 0
		}
		return -1
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		if opponent.pbCanSleep? (attacker,false,self)
		  opponent.pbSleep
		}
	  }

	  public object pbEffectAfterHit(attacker, opponent, turneffects){
		if isConst? (@id, PBMoves,:RELICSONG)
		  if isConst? (attacker.species, PBSpecies,:MELOETTA) &&
			 !attacker.effects[PBEffects::Transform] &&
			 !(attacker.hasWorkingAbility(:SHEERFORCE) && self.addlEffect>0) &&
			 !attacker.fainted?
			attacker.form=(attacker.form+1)%2
			attacker.pbUpdate(true)
			@battle.scene.pbChangePokemon(attacker, attacker.pokemon)
			//battle.pbDisplay(_INTL("{1} transformed!", attacker.pbThis))
			PBDebug.log("[Form changed] #{attacker.pbThis} changed to form #{attacker.form}")
		  }
		}
	  }
	}



	/// <summary>
	/// Makes the target drowsy; it will fall asleep at the } of the next turn. (Yawn)
	/// <summary>
	public class PokeBattle_Move_004 : PokeBattle_Move
	{
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if (!opponent.pbCanSleep? (attacker,true,self)) return -1;
		if opponent.effects[PBEffects::Yawn]>0
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.effects[PBEffects::Yawn]=2
		//battle.pbDisplay(_INTL("{1} made {2} drowsy!",attacker.pbThis,opponent.pbThis(true)))
		return 0
	  }
	}



	/// <summary>
	/// Poisons the target.
	/// <summary>
	public class PokeBattle_Move_005 : PokeBattle_Move
	{
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if (pbIsDamaging?) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!opponent.pbCanPoison? (attacker,true,self)) return -1;
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.pbPoison(attacker)
		return 0
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		if opponent.pbCanPoison? (attacker,false,self)
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if (pbIsDamaging?) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!opponent.pbCanPoison? (attacker,true,self)) return -1;
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.pbPoison(attacker, null,true)
		return 0
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		if opponent.pbCanPoison? (attacker,false,self)
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if pbIsDamaging?
		  ret = super(attacker, opponent, hitnum, alltargets, showanimation)
		  if opponent.damagestate.calcdamage>0 && isConst? (@id, PBMoves,:BOLTSTRIKE)
			@battle.field.effects[PBEffects::FusionFlare]=true
		  }
		  return ret
		else
		  if isConst? (@id, PBMoves,:THUNDERWAVE)
			if pbTypeModifier(type, attacker, opponent)==0
			  //battle.pbDisplay(_INTL("It doesn't affect {1}...",opponent.pbThis(true)))
			  return -1
			}
		  }
		  if (pbTypeImmunityByAbility(pbType(@type, attacker, opponent), attacker, opponent)) return -1;
		  if (!opponent.pbCanParalyze? (attacker,true,self)) return -1;
		  pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		  opponent.pbParalyze(attacker)
		  return 0
		}
		return -1
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		if opponent.pbCanParalyze? (attacker,false,self)
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
		public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		if opponent.pbCanParalyze? (attacker,false,self)
		  opponent.pbParalyze(attacker)
		}
	  }

	  public object pbModifyBaseAccuracy(baseaccuracy, attacker, opponent){
		case @battle.pbWeather
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
		public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		if @battle.pbRandom(10)==0
		  if opponent.pbCanParalyze? (attacker,false,self)
			opponent.pbParalyze(attacker)
		  }

		}
		if @battle.pbRandom(10)==0
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if pbIsDamaging?
		  ret = super(attacker, opponent, hitnum, alltargets, showanimation)
		  if opponent.damagestate.calcdamage>0 && isConst? (@id, PBMoves,:BLUEFLARE)
			@battle.field.effects[PBEffects::FusionBolt]=true
		  }
		  return ret
		else
		  if (pbTypeImmunityByAbility(pbType(@type, attacker, opponent), attacker, opponent)) return -1;
		  if (!opponent.pbCanBurn? (attacker,true,self)) return -1;
		  pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		  opponent.pbBurn(attacker)
		  return 0
		}
		return -1
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		if opponent.pbCanBurn? (attacker,false,self)
		  opponent.pbBurn(attacker)
		}
	  }
	}



	/// <summary>
	/// Burns the target. May cause the target to flinch. (Fire Fang)
	/// <summary>
	public class PokeBattle_Move_00B : PokeBattle_Move
	{
		public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		if @battle.pbRandom(10)==0
		  if opponent.pbCanBurn? (attacker,false,self)
			opponent.pbBurn(attacker)
		  }

		}
		if @battle.pbRandom(10)==0
		  opponent.pbFlinch(attacker)
		}
	  }
	}



	/// <summary>
	/// Freezes the target.
	/// <summary>
	public class PokeBattle_Move_00C : PokeBattle_Move
	{
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if (pbIsDamaging?) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!opponent.pbCanFreeze? (attacker,true,self)) return -1;
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.pbFreeze
		return 0
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		if opponent.pbCanFreeze? (attacker,false,self)
		  opponent.pbFreeze
		}
	  }
	}



	/// <summary>
	/// Freezes the target. Accuracy perfect in hail. (Blizzard)
	/// <summary>
	public class PokeBattle_Move_00D : PokeBattle_Move
	{
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if (pbIsDamaging?) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!opponent.pbCanFreeze? (attacker,true,self)) return -1;
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.pbFreeze
		return 0
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		if opponent.pbCanFreeze? (attacker,false,self)
		  opponent.pbFreeze
		}
	  }

	  public object pbModifyBaseAccuracy(baseaccuracy, attacker, opponent){
		if @battle.pbWeather==PBWeather::HAIL
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
		public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		if @battle.pbRandom(10)==0
		  if opponent.pbCanFreeze? (attacker,false,self)
			opponent.pbFreeze
		  }

		}
		if @battle.pbRandom(10)==0
		  opponent.pbFlinch(attacker)
		}
	  }
	}



	/// <summary>
	/// Causes the target to flinch.
	/// <summary>
	public class PokeBattle_Move_00F : PokeBattle_Move
	{
		public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		opponent.pbFlinch(attacker)
	  }
	}



	/// <summary>
	/// Causes the target to flinch. Does double damage and has perfect accuracy if
	/// the target is Minimized.
	/// <summary>
	public class PokeBattle_Move_010 : PokeBattle_Move
	{
		public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		opponent.pbFlinch(attacker)
	  }

	  public object tramplesMinimize? (param=1)
		if (isConst? (@id, PBMoves,:DRAGONRUSH) && !USENEWBATTLEMECHANICS) return false;
		if (param==1 && USENEWBATTLEMECHANICS // Perfect accuracy) return true;
		if (param==2 // Double damage) return true;
		return false
	  }
	}



	/// <summary>
	/// Causes the target to flinch. Fails if the user is not asleep. (Snore)
	/// <summary>
	public class PokeBattle_Move_011 : PokeBattle_Move
	{
		public object pbCanUseWhileAsleep?
		return true
	  }

	  public object pbMoveFailed(attacker, opponent){
		return (attacker.status!=PBStatuses::SLEEP)
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		opponent.pbFlinch(attacker)
	  }
	}



	/// <summary>
	/// Causes the target to flinch. Fails if this isn't the user's first turn. (Fake Out)
	/// <summary>
	public class PokeBattle_Move_012 : PokeBattle_Move
	{
		public object pbMoveFailed(attacker, opponent){
		return (attacker.turncount>1)
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		opponent.pbFlinch(attacker)
	  }
	}



	/// <summary>
	/// Confuses the target.
	/// <summary>
	public class PokeBattle_Move_013 : PokeBattle_Move
	{
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if (pbIsDamaging?) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if opponent.pbCanConfuse? (attacker,true,self)
		  pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		  opponent.pbConfuse
		  //battle.pbDisplay(_INTL("{1} became confused!", opponent.pbThis))
		  return 0
		}
		return -1
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		if opponent.pbCanConfuse? (attacker,false,self)
		  opponent.pbConfuse
		  //battle.pbDisplay(_INTL("{1} became confused!", opponent.pbThis))
		}
	  }
	}



	/// <summary>
	/// Confuses the target. Chance of causing confusion depends on the cry's volume.
	/// Confusion chance is 0% if user doesn't have a recorded cry. (Chatter)
	/// TODO: Play the actual chatter cry as part of the move animation
	///       @battle.scene.pbChatter(attacker,opponent) // Just plays cry
	/// <summary>
	public class PokeBattle_Move_014 : PokeBattle_Move
	{
		public object addlEffect
		if (USENEWBATTLEMECHANICS) return 100;
		if attacker.pokemon && attacker.pokemon.chatter
		  return attacker.pokemon.chatter.intensity*10/127
		}
		return 0
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		if opponent.pbCanConfuse? (attacker,false,self)
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if (pbIsDamaging?) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if opponent.pbCanConfuse? (attacker,true,self)
		  pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		  opponent.pbConfuse
		  //battle.pbDisplay(_INTL("{1} became confused!", opponent.pbThis))
		  return 0
		}
		return -1
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		if opponent.pbCanConfuse? (attacker,false,self)
		  opponent.pbConfuse
		  //battle.pbDisplay(_INTL("{1} became confused!", opponent.pbThis))
		}
	  }

	  public object pbModifyBaseAccuracy(baseaccuracy, attacker, opponent){
		case @battle.pbWeather
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if !opponent.pbCanAttract? (attacker)
		  return -1
		}
		if !attacker.hasMoldBreaker
		  if opponent.hasWorkingAbility(:AROMAVEIL)
			//battle.pbDisplay(_INTL("But it failed because of {1}'s {2}!",
			   opponent.pbThis,PBAbilities.getName(opponent.ability)))
			return -1
		  elsif opponent.pbPartner.hasWorkingAbility(:AROMAVEIL)

			//battle.pbDisplay(_INTL("But it failed because of {1}'s {2}!",
			   opponent.pbPartner.pbThis,PBAbilities.getName(opponent.pbPartner.ability)))
			return -1
		  }
		}

		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.pbAttract(attacker)
		return 0
	  }
	}



	/// <summary>
	/// Burns, freezes or paralyzes the target. (Tri Attack)
	/// <summary>
	public class PokeBattle_Move_017 : PokeBattle_Move
	{
		public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		case @battle.pbRandom(3)
		when 0
		  if opponent.pbCanBurn? (attacker,false,self)
			opponent.pbBurn(attacker)
		  }

		when 1
		  if opponent.pbCanFreeze? (attacker,false,self)
			opponent.pbFreeze
		  }

		when 2
		  if opponent.pbCanParalyze? (attacker,false,self)
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if attacker.status!=PBStatuses::BURN &&
		   attacker.status!=PBStatuses::POISON &&
		   attacker.status!=PBStatuses::PARALYSIS
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		else
		  t=attacker.status
		  attacker.pbCureStatus(false)

		  pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)
		  if t==PBStatuses::BURN
			//battle.pbDisplay(_INTL("{1} healed its burn!", attacker.pbThis))  
		  elsif t==PBStatuses::POISON
			//battle.pbDisplay(_INTL("{1} cured its poisoning!", attacker.pbThis))  
		  elsif t==PBStatuses::PARALYSIS
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)

		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)
		if isConst? (@id, PBMoves,:AROMATHERAPY)
		  //battle.pbDisplay(_INTL("A soothing aroma wafted through the area!"))
		else
		  //battle.pbDisplay(_INTL("A bell chimed!"))
		}
		activepkmn =[]
		for i in @battle.battlers
		  next if attacker.pbIsOpposing? (i.index) || i.fainted?
		   activepkmn.push(i.pokemonIndex)

		  next if USENEWBATTLEMECHANICS && i.index!=attacker.index && 
			 pbTypeImmunityByAbility(pbType(@type, attacker, i), attacker, i)
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
		party = @battle.pbParty(attacker.index) // NOTE: Considers both parties in multi battles
		for i in 0...party.length
		  next if activepkmn.include? (i)
		   next if !party[i] || party[i].egg? || party[i].hp<=0
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if attacker.pbOwnSide.effects[PBEffects::Safeguard]>0
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		attacker.pbOwnSide.effects[PBEffects::Safeguard]= 5

		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)
		if !@battle.pbIsOpposing?(attacker.index)
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if attacker.status==0 ||
		  (attacker.status==PBStatuses::PARALYSIS && !opponent.pbCanParalyze? (attacker,false,self)) ||
		  (attacker.status==PBStatuses::SLEEP && !opponent.pbCanSleep? (attacker,false,self)) ||
		  (attacker.status==PBStatuses::POISON && !opponent.pbCanPoison? (attacker,false,self)) ||
		  (attacker.status==PBStatuses::BURN && !opponent.pbCanBurn? (attacker,false,self)) ||
		  (attacker.status==PBStatuses::FROZEN && !opponent.pbCanFreeze? (attacker,false,self))
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if (pbIsDamaging?) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,true,self)) return -1;
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=attacker.pbIncreaseStat(PBStats::ATTACK,1,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self)
		  attacker.pbIncreaseStat(PBStats::ATTACK,1,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Increases the user's Defense by 1 stage.
	/// <summary>
	public class PokeBattle_Move_01D : PokeBattle_Move
	{
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if (pbIsDamaging?) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!attacker.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,true,self)) return -1;
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=attacker.pbIncreaseStat(PBStats::DEFENSE,1,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if attacker.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,false,self)
		  attacker.pbIncreaseStat(PBStats::DEFENSE,1,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Increases the user's Defense by 1 stage. User curls up. (Defense Curl)
	/// <summary>
	public class PokeBattle_Move_01E : PokeBattle_Move
	{
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)

		attacker.effects[PBEffects::DefenseCurl]=true
		if (!attacker.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,true,self)) return -1;
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=attacker.pbIncreaseStat(PBStats::DEFENSE,1,attacker,false,self)
		return ret? 0 : -1
	  }
	}



	/// <summary>
	/// Increases the user's Speed by 1 stage.
	/// <summary>
	public class PokeBattle_Move_01F : PokeBattle_Move
	{
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if (pbIsDamaging?) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!attacker.pbCanIncreaseStatStage? (PBStats::SPEED, attacker,true,self)) return -1;
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=attacker.pbIncreaseStat(PBStats::SPEED,1,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if attacker.pbCanIncreaseStatStage? (PBStats::SPEED, attacker,false,self)
		  attacker.pbIncreaseStat(PBStats::SPEED,1,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Increases the user's Special Attack by 1 stage.
	/// <summary>
	public class PokeBattle_Move_020 : PokeBattle_Move
	{
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if (pbIsDamaging?) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!attacker.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,true,self)) return -1;
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=attacker.pbIncreaseStat(PBStats::SPATK,1,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if attacker.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,false,self)
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)

		attacker.effects[PBEffects::Charge]=2
		//battle.pbDisplay(_INTL("{1} began charging power!",attacker.pbThis))
		if attacker.pbCanIncreaseStatStage? (PBStats::SPDEF, attacker,true,self)
		  pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if (pbIsDamaging?) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!attacker.pbCanIncreaseStatStage? (PBStats::EVASION, attacker,true,self)) return -1;
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=attacker.pbIncreaseStat(PBStats::EVASION,1,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if attacker.pbCanIncreaseStatStage? (PBStats::EVASION, attacker,false,self)
		  attacker.pbIncreaseStat(PBStats::EVASION,1,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Increases the user's critical hit rate. (Focus Energy)
	/// <summary>
	public class PokeBattle_Move_023 : PokeBattle_Move
	{
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if (pbIsDamaging?) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if attacker.effects[PBEffects::FocusEnergy]>=2
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		attacker.effects[PBEffects::FocusEnergy]=2
		//battle.pbDisplay(_INTL("{1} is getting pumped!",attacker.pbThis))
		return 0
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if attacker.effects[PBEffects::FocusEnergy]<2
		  attacker.effects[PBEffects::FocusEnergy]=2
		  //battle.pbDisplay(_INTL("{1} is getting pumped!",attacker.pbThis))
		}
	  }
	}



	/// <summary>
	/// Increases the user's Attack and Defense by 1 stage each. (Bulk Up)
	/// <summary>
	public class PokeBattle_Move_024 : PokeBattle_Move
	{
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if !attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self) &&
		   !attacker.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,false,self)
		  //battle.pbDisplay(_INTL("{1}'s stats won't go any higher!",attacker.pbThis))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		showanim=true
		if attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self)
		  attacker.pbIncreaseStat(PBStats::ATTACK,1,attacker,false,self,showanim)
		  showanim=false
		}
		if attacker.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,false,self)
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if !attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self) &&
		   !attacker.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,false,self) &&
		   !attacker.pbCanIncreaseStatStage? (PBStats::ACCURACY, attacker,false,self)
		  //battle.pbDisplay(_INTL("{1}'s stats won't go any higher!",attacker.pbThis))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		showanim=true
		if attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self)
		  attacker.pbIncreaseStat(PBStats::ATTACK,1,attacker,false,self,showanim)
		  showanim=false
		}
		if attacker.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,false,self)
		  attacker.pbIncreaseStat(PBStats::DEFENSE,1,attacker,false,self,showanim)
		  showanim=false
		}
		if attacker.pbCanIncreaseStatStage? (PBStats::ACCURACY, attacker,false,self)
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if !attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self) &&
		   !attacker.pbCanIncreaseStatStage? (PBStats::SPEED, attacker,false,self)
		  //battle.pbDisplay(_INTL("{1}'s stats won't go any higher!",attacker.pbThis))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		showanim=true
		if attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self)
		  attacker.pbIncreaseStat(PBStats::ATTACK,1,attacker,false,self,showanim)
		  showanim=false
		}
		if attacker.pbCanIncreaseStatStage? (PBStats::SPEED, attacker,false,self)
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if !attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self) &&
		   !attacker.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,false,self)
		  //battle.pbDisplay(_INTL("{1}'s stats won't go any higher!",attacker.pbThis))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		showanim=true
		if attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self)
		  attacker.pbIncreaseStat(PBStats::ATTACK,1,attacker,false,self,showanim)
		  showanim=false
		}
		if attacker.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,false,self)
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if !attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self) &&
		   !attacker.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,false,self)
		  //battle.pbDisplay(_INTL("{1}'s stats won't go any higher!",attacker.pbThis))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		showanim=true
		increment=1
		if @battle.pbWeather==PBWeather::SUNNYDAY ||
		   @battle.pbWeather==PBWeather::HARSHSUN
		  increment = 2

		}
		if attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self)
		  attacker.pbIncreaseStat(PBStats::ATTACK, increment, attacker,false,self,showanim)
		  showanim=false
		}
		if attacker.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,false,self)
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if !attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self) &&
		   !attacker.pbCanIncreaseStatStage? (PBStats::ACCURACY, attacker,false,self)
		  //battle.pbDisplay(_INTL("{1}'s stats won't go any higher!",attacker.pbThis))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		showanim=true
		if attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self)
		  attacker.pbIncreaseStat(PBStats::ATTACK,1,attacker,false,self,showanim)
		  showanim=false
		}
		if attacker.pbCanIncreaseStatStage? (PBStats::ACCURACY, attacker,false,self)
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if !attacker.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,false,self) &&
		   !attacker.pbCanIncreaseStatStage? (PBStats::SPDEF, attacker,false,self)
		  //battle.pbDisplay(_INTL("{1}'s stats won't go any higher!",attacker.pbThis))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		showanim=true
		if attacker.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,false,self)
		  attacker.pbIncreaseStat(PBStats::DEFENSE,1,attacker,false,self,showanim)
		  showanim=false
		}
		if attacker.pbCanIncreaseStatStage? (PBStats::SPDEF, attacker,false,self)
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if !attacker.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,false,self) &&
		   !attacker.pbCanIncreaseStatStage? (PBStats::SPDEF, attacker,false,self) &&
		   !attacker.pbCanIncreaseStatStage? (PBStats::SPEED, attacker,false,self)
		  //battle.pbDisplay(_INTL("{1}'s stats won't go any higher!",attacker.pbThis))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		showanim=true
		if attacker.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,false,self)
		  attacker.pbIncreaseStat(PBStats::SPATK,1,attacker,false,self,showanim)
		  showanim=false
		}
		if attacker.pbCanIncreaseStatStage? (PBStats::SPDEF, attacker,false,self)
		  attacker.pbIncreaseStat(PBStats::SPDEF,1,attacker,false,self,showanim)
		  showanim=false
		}
		if attacker.pbCanIncreaseStatStage? (PBStats::SPEED, attacker,false,self)
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if !attacker.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,false,self) &&
		   !attacker.pbCanIncreaseStatStage? (PBStats::SPDEF, attacker,false,self)
		  //battle.pbDisplay(_INTL("{1}'s stats won't go any higher!",attacker.pbThis))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		showanim=true
		if attacker.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,false,self)
		  attacker.pbIncreaseStat(PBStats::SPATK,1,attacker,false,self,showanim)
		  showanim=false
		}
		if attacker.pbCanIncreaseStatStage? (PBStats::SPDEF, attacker,false,self)
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
		public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){

		showanim=true
		if attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self)
		  attacker.pbIncreaseStat(PBStats::ATTACK,1,attacker,false,self,showanim)
		  showanim=false
		}
		if attacker.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,false,self)
		  attacker.pbIncreaseStat(PBStats::DEFENSE,1,attacker,false,self,showanim)
		  showanim=false
		}
		if attacker.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,false,self)
		  attacker.pbIncreaseStat(PBStats::SPATK,1,attacker,false,self,showanim)
		  showanim=false
		}
		if attacker.pbCanIncreaseStatStage? (PBStats::SPDEF, attacker,false,self)
		  attacker.pbIncreaseStat(PBStats::SPDEF,1,attacker,false,self,showanim)
		  showanim=false
		}
		if attacker.pbCanIncreaseStatStage? (PBStats::SPEED, attacker,false,self)
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if (pbIsDamaging?) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,true,self)) return -1;
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=attacker.pbIncreaseStat(PBStats::ATTACK,2,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self)
		  attacker.pbIncreaseStat(PBStats::ATTACK,2,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Increases the user's Defense by 2 stages.
	/// <summary>
	public class PokeBattle_Move_02F : PokeBattle_Move
	{
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if (pbIsDamaging?) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!attacker.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,true,self)) return -1;
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=attacker.pbIncreaseStat(PBStats::DEFENSE,2,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if attacker.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,false,self)
		  attacker.pbIncreaseStat(PBStats::DEFENSE,2,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Increases the user's Speed by 2 stages.
	/// <summary>
	public class PokeBattle_Move_030 : PokeBattle_Move
	{
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if (pbIsDamaging?) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!attacker.pbCanIncreaseStatStage? (PBStats::SPEED, attacker,true,self)) return -1;
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=attacker.pbIncreaseStat(PBStats::SPEED,2,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if attacker.pbCanIncreaseStatStage? (PBStats::SPEED, attacker,false,self)
		  attacker.pbIncreaseStat(PBStats::SPEED,2,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Increases the user's Speed by 2 stages. Lowers user's weight by 100kg. (Autotomize)
	/// <summary>
	public class PokeBattle_Move_031 : PokeBattle_Move
	{
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if (!attacker.pbCanIncreaseStatStage? (PBStats::SPEED, attacker,true,self)) return -1;
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=attacker.pbIncreaseStat(PBStats::SPEED,2,attacker,false,self)
		if ret
		  attacker.effects[PBEffects::WeightChange]-=1000

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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if (pbIsDamaging?) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!attacker.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,true,self)) return -1;
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=attacker.pbIncreaseStat(PBStats::SPATK,2,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if attacker.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,false,self)
		  attacker.pbIncreaseStat(PBStats::SPATK,2,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Increases the user's Special Defense by 2 stages.
	/// <summary>
	public class PokeBattle_Move_033 : PokeBattle_Move
	{
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if (pbIsDamaging?) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!attacker.pbCanIncreaseStatStage? (PBStats::SPDEF, attacker,true,self)) return -1;
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=attacker.pbIncreaseStat(PBStats::SPDEF,2,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if attacker.pbCanIncreaseStatStage? (PBStats::SPDEF, attacker,false,self)
		  attacker.pbIncreaseStat(PBStats::SPDEF,2,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Increases the user's evasion by 2 stages. Minimizes the user. (Minimize)
	/// <summary>
	public class PokeBattle_Move_034 : PokeBattle_Move
	{
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if (pbIsDamaging?) return super(attacker, opponent, hitnum, alltargets, showanimation);
			attacker.effects[PBEffects::Minimize]= true
		if (!attacker.pbCanIncreaseStatStage?(PBStats::EVASION, attacker,true, self)) return -1;

		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=attacker.pbIncreaseStat(PBStats::EVASION,2,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){

		attacker.effects[PBEffects::Minimize]=true
		if attacker.pbCanIncreaseStatStage? (PBStats::EVASION, attacker,false,self)
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if !attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self) &&
		   !attacker.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,false,self) &&
		   !attacker.pbCanIncreaseStatStage? (PBStats::SPEED, attacker,false,self)
		  //battle.pbDisplay(_INTL("{1}'s stats won't go any higher!",attacker.pbThis))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		showanim=true
		if attacker.pbCanReduceStatStage? (PBStats::DEFENSE, attacker,false,self)
		  attacker.pbReduceStat(PBStats::DEFENSE,1,attacker,false,self,showanim)
		  showanim=false
		}
		if attacker.pbCanReduceStatStage? (PBStats::SPDEF, attacker,false,self)
		  attacker.pbReduceStat(PBStats::SPDEF,1,attacker,false,self,showanim)
		  showanim=false
		}
		showanim = true
		if attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self)
		  attacker.pbIncreaseStat(PBStats::ATTACK,2,attacker,false,self,showanim)
		  showanim=false
		}
		if attacker.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,false,self)
		  attacker.pbIncreaseStat(PBStats::SPATK,2,attacker,false,self,showanim)
		  showanim=false
		}
		if attacker.pbCanIncreaseStatStage? (PBStats::SPEED, attacker,false,self)
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if !attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self) &&
		   !attacker.pbCanIncreaseStatStage? (PBStats::SPEED, attacker,false,self)
		  //battle.pbDisplay(_INTL("{1}'s stats won't go any higher!",attacker.pbThis))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		showanim=true
		if attacker.pbCanIncreaseStatStage? (PBStats::SPEED, attacker,false,self)
		  attacker.pbIncreaseStat(PBStats::SPEED,2,attacker,false,self,showanim)
		  showanim=false
		}
		if attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self)
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if attacker.index!=opponent.index 
		  if (opponent.effects[PBEffects::Substitute]>0 && !ignoresSubstitute? (attacker)) ||
			 opponent.pbOwnSide.effects[PBEffects::CraftyShield]
			//battle.pbDisplay(_INTL("But it failed!"))
			return -1
		  }
		}

		array=[]
		for i in [PBStats::ATTACK, PBStats::DEFENSE, PBStats::SPEED,
				  PBStats::SPATK, PBStats::SPDEF, PBStats::ACCURACY, PBStats::EVASION]
		array.push(i) if opponent.pbCanIncreaseStatStage? (i, attacker,false,self)
		}
		if array.length==0
		  //battle.pbDisplay(_INTL("{1}'s stats won't go any higher!",opponent.pbThis))
		  return -1
		}
		stat = array[@battle.pbRandom(array.length)]

		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=opponent.pbIncreaseStat(stat,2,attacker,false,self)
		return 0
	  }
	}



	/// <summary>
	/// Increases the user's Defense by 3 stages.
	/// <summary>
	public class PokeBattle_Move_038 : PokeBattle_Move
	{
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if (pbIsDamaging?) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!attacker.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,true,self)) return -1;
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=attacker.pbIncreaseStat(PBStats::DEFENSE,3,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if attacker.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,false,self)
		  attacker.pbIncreaseStat(PBStats::DEFENSE,3,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Increases the user's Special Attack by 3 stages.
	/// <summary>
	public class PokeBattle_Move_039 : PokeBattle_Move
	{
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if (pbIsDamaging?) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!attacker.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,true,self)) return -1;
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=attacker.pbIncreaseStat(PBStats::SPATK,3,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if attacker.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,false,self)
		  attacker.pbIncreaseStat(PBStats::SPATK,3,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Reduces the user's HP by half of max, and sets its Attack to maximum. (Belly Drum)
	/// <summary>
	public class PokeBattle_Move_03A : PokeBattle_Move
	{
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if attacker.hp<=(attacker.totalhp/2).floor ||
		   !attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self)
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		attacker.pbReduceHP((attacker.totalhp/2).floor)
		if attacker.hasWorkingAbility(:CONTRARY)
		  attacker.stages[PBStats::ATTACK]=-6
		  @battle.pbCommonAnimation("StatDown",attacker,null)
		  //battle.pbDisplay(_INTL("{1} cut its own HP and minimized its Attack!",attacker.pbThis))
		else
		  attacker.stages[PBStats::ATTACK]=6
		  @battle.pbCommonAnimation("StatUp",attacker,null)
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)

		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if opponent.damagestate.calcdamage>0
		  showanim=true
		  if attacker.pbCanReduceStatStage? (PBStats::ATTACK, attacker,false,self)
			attacker.pbReduceStat(PBStats::ATTACK,1,attacker,false,self,showanim)
			showanim=false
		  }
		  if attacker.pbCanReduceStatStage? (PBStats::DEFENSE, attacker,false,self)
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)

		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if opponent.damagestate.calcdamage>0
		  showanim=true
		  if attacker.pbCanReduceStatStage? (PBStats::DEFENSE, attacker,false,self)
			attacker.pbReduceStat(PBStats::DEFENSE,1,attacker,false,self,showanim)
			showanim=false
		  }
		  if attacker.pbCanReduceStatStage? (PBStats::SPDEF, attacker,false,self)
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)

		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if opponent.damagestate.calcdamage>0
		  if attacker.pbPartner && !attacker.pbPartner.fainted?
			attacker.pbPartner.pbReduceHP((attacker.pbPartner.totalhp/16).floor,true)
		  }
		  showanim = true
		  if attacker.pbCanReduceStatStage? (PBStats::SPEED, attacker,false,self)
			attacker.pbReduceStat(PBStats::SPEED,1,attacker,false,self,showanim)
			showanim=false
		  }
		  if attacker.pbCanReduceStatStage? (PBStats::DEFENSE, attacker,false,self)
			attacker.pbReduceStat(PBStats::DEFENSE,1,attacker,false,self,showanim)
			showanim=false
		  }
		  if attacker.pbCanReduceStatStage? (PBStats::SPDEF, attacker,false,self)
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)

		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if opponent.damagestate.calcdamage>0
		  if attacker.pbCanReduceStatStage? (PBStats::SPEED, attacker,false,self)
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)

		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if opponent.damagestate.calcdamage>0
		  if attacker.pbCanReduceStatStage? (PBStats::SPATK, attacker,false,self)
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if opponent.effects[PBEffects::Substitute]>0 && !ignoresSubstitute? (attacker)
		   //battle.pbDisplay(_INTL("{1}'s attack missed!", attacker.pbThis))
		  return -1
		}
		ret = -1

		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)
		if opponent.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,false,self)
		  opponent.pbIncreaseStat(PBStats::SPATK,1,attacker,false,self)
		  ret=0
		}
		if opponent.pbCanConfuse? (attacker,true,self)
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if opponent.effects[PBEffects::Substitute]>0 && !ignoresSubstitute? (attacker)
		   //battle.pbDisplay(_INTL("{1}'s attack missed!", attacker.pbThis))
		  return -1
		}
		ret = -1

		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)
		if opponent.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self)
		  opponent.pbIncreaseStat(PBStats::ATTACK,2,attacker,false,self)
		  ret=0
		}
		if opponent.pbCanConfuse? (attacker,true,self)
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if (pbIsDamaging?) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!opponent.pbCanReduceStatStage? (PBStats::ATTACK, attacker,true,self)) return -1;
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=opponent.pbReduceStat(PBStats::ATTACK,1,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		if opponent.pbCanReduceStatStage? (PBStats::ATTACK, attacker,false,self)
		  opponent.pbReduceStat(PBStats::ATTACK,1,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Decreases the target's Defense by 1 stage.
	/// <summary>
	public class PokeBattle_Move_043 : PokeBattle_Move
	{
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if (pbIsDamaging?) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!opponent.pbCanReduceStatStage? (PBStats::DEFENSE, attacker,true,self)) return -1;
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=opponent.pbReduceStat(PBStats::DEFENSE,1,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		if opponent.pbCanReduceStatStage? (PBStats::DEFENSE, attacker,false,self)
		  opponent.pbReduceStat(PBStats::DEFENSE,1,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Decreases the target's Speed by 1 stage.
	/// <summary>
	public class PokeBattle_Move_044 : PokeBattle_Move
	{
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if (pbIsDamaging?) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!opponent.pbCanReduceStatStage? (PBStats::SPEED, attacker,true,self)) return -1;
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=opponent.pbReduceStat(PBStats::SPEED,1,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		if opponent.pbCanReduceStatStage? (PBStats::SPEED, attacker,false,self)
		  opponent.pbReduceStat(PBStats::SPEED,1,attacker,false,self)
		}
	  }

	  public object pbModifyDamage(damagemult, attacker, opponent){
		if isConst? (@id, PBMoves,:BULLDOZE) &&
		   @battle.field.effects[PBEffects::GrassyTerrain]>0
		  return (damagemult/2.0).round
		}
		return damagemult
	  }
	}



	/// <summary>
	/// Decreases the target's Special Attack by 1 stage.
	/// <summary>
	public class PokeBattle_Move_045 : PokeBattle_Move
	{
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if (pbIsDamaging?) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!opponent.pbCanReduceStatStage? (PBStats::SPATK, attacker,true,self)) return -1;
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=opponent.pbReduceStat(PBStats::SPATK,1,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		if opponent.pbCanReduceStatStage? (PBStats::SPATK, attacker,false,self)
		  opponent.pbReduceStat(PBStats::SPATK,1,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Decreases the target's Special Defense by 1 stage.
	/// <summary>
	public class PokeBattle_Move_046 : PokeBattle_Move
	{
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if (pbIsDamaging?) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!opponent.pbCanReduceStatStage? (PBStats::SPDEF, attacker,true,self)) return -1;
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=opponent.pbReduceStat(PBStats::SPDEF,1,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		if opponent.pbCanReduceStatStage? (PBStats::SPDEF, attacker,false,self)
		  opponent.pbReduceStat(PBStats::SPDEF,1,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Decreases the target's accuracy by 1 stage.
	/// <summary>
	public class PokeBattle_Move_047 : PokeBattle_Move
	{
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if (pbIsDamaging?) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!opponent.pbCanReduceStatStage? (PBStats::ACCURACY, attacker,true,self)) return -1;
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=opponent.pbReduceStat(PBStats::ACCURACY,1,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		if opponent.pbCanReduceStatStage? (PBStats::ACCURACY, attacker,false,self)
		  opponent.pbReduceStat(PBStats::ACCURACY,1,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Decreases the target's evasion by 1 stage OR 2 stages. (Sweet Scent)
	/// <summary>
	public class PokeBattle_Move_048 : PokeBattle_Move
	{
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if (pbIsDamaging?) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!opponent.pbCanReduceStatStage? (PBStats::EVASION, attacker,true,self)) return -1;
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		increment=(USENEWBATTLEMECHANICS)? 2 : 1
		ret=opponent.pbReduceStat(PBStats::EVASION, increment, attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		if opponent.pbCanReduceStatStage? (PBStats::EVASION, attacker,false,self)
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if (pbIsDamaging?) return super(attacker, opponent, hitnum, alltargets, showanimation);
			pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.pbReduceStat(PBStats::EVASION,1,attacker,false,self)
		opponent.pbOwnSide.effects[PBEffects::Reflect]     = 0
		opponent.pbOwnSide.effects[PBEffects::LightScreen] = 0
		opponent.pbOwnSide.effects[PBEffects::Mist]        = 0
		opponent.pbOwnSide.effects[PBEffects::Safeguard]   = 0
		opponent.pbOwnSide.effects[PBEffects::Spikes]      = 0
		opponent.pbOwnSide.effects[PBEffects::StealthRock] = false
		opponent.pbOwnSide.effects[PBEffects::StickyWeb]   = false
		opponent.pbOwnSide.effects[PBEffects::ToxicSpikes] = 0
		if USENEWBATTLEMECHANICS
		  opponent.pbOpposingSide.effects[PBEffects::Reflect]     = 0

		  opponent.pbOpposingSide.effects[PBEffects::LightScreen] = 0

		  opponent.pbOpposingSide.effects[PBEffects::Mist]        = 0

		  opponent.pbOpposingSide.effects[PBEffects::Safeguard]   = 0

		  opponent.pbOpposingSide.effects[PBEffects::Spikes]      = 0

		  opponent.pbOpposingSide.effects[PBEffects::StealthRock] = false

		  opponent.pbOpposingSide.effects[PBEffects::StickyWeb]   = false

		  opponent.pbOpposingSide.effects[PBEffects::ToxicSpikes] = 0

		}
		return 0
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if !opponent.damagestate.substitute
		  if opponent.pbCanReduceStatStage?(PBStats::EVASION, attacker,false, self)

			opponent.pbReduceStat(PBStats::EVASION,1,attacker,false,self)
		  }
		}

		opponent.pbOwnSide.effects[PBEffects::Reflect]     = 0
		opponent.pbOwnSide.effects[PBEffects::LightScreen] = 0
		opponent.pbOwnSide.effects[PBEffects::Mist]        = 0
		opponent.pbOwnSide.effects[PBEffects::Safeguard]   = 0
		opponent.pbOwnSide.effects[PBEffects::Spikes]      = 0
		opponent.pbOwnSide.effects[PBEffects::StealthRock] = false
		opponent.pbOwnSide.effects[PBEffects::StickyWeb]   = false
		opponent.pbOwnSide.effects[PBEffects::ToxicSpikes] = 0
		if USENEWBATTLEMECHANICS
		  opponent.pbOpposingSide.effects[PBEffects::Reflect]     = 0

		  opponent.pbOpposingSide.effects[PBEffects::LightScreen] = 0

		  opponent.pbOpposingSide.effects[PBEffects::Mist]        = 0

		  opponent.pbOpposingSide.effects[PBEffects::Safeguard]   = 0

		  opponent.pbOpposingSide.effects[PBEffects::Spikes]      = 0

		  opponent.pbOpposingSide.effects[PBEffects::StealthRock] = false

		  opponent.pbOpposingSide.effects[PBEffects::StickyWeb]   = false

		  opponent.pbOpposingSide.effects[PBEffects::ToxicSpikes] = 0

		}
	  }
	}



	/// <summary>
	/// Decreases the target's Attack and Defense by 1 stage each. (Tickle)
	/// <summary>
	public class PokeBattle_Move_04A : PokeBattle_Move
	{
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		// Replicates public object pbCanReduceStatStage? so that certain messages aren't shown
		// multiple times
		if opponent.effects[PBEffects::Substitute]>0 && !ignoresSubstitute? (attacker)
		   //battle.pbDisplay(_INTL("{1}'s attack missed!", attacker.pbThis))
		  return -1
		}
		if opponent.pbTooLow? (PBStats::ATTACK) &&
		   opponent.pbTooLow? (PBStats::DEFENSE)
		   //battle.pbDisplay(_INTL("{1}'s stats won't go any lower!", opponent.pbThis))
		  return -1
		}
		if opponent.pbOwnSide.effects[PBEffects::Mist]>0
		  //battle.pbDisplay(_INTL("{1} is protected by Mist!",opponent.pbThis))
		  return -1
		}
		if !attacker.hasMoldBreaker
		  if opponent.hasWorkingAbility(:CLEARBODY) ||
			 opponent.hasWorkingAbility(:WHITESMOKE)
			//battle.pbDisplay(_INTL("{1}'s {2} prevents stat loss!",opponent.pbThis,
			   PBAbilities.getName(opponent.ability)))
			return -1
		  }
		}

		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=-1; showanim=true
		if !attacker.hasMoldBreaker && opponent.hasWorkingAbility(:HYPERCUTTER) &&
		   !opponent.pbTooLow? (PBStats::ATTACK)
		   abilityname = PBAbilities.getName(opponent.ability)

		  //battle.pbDisplay(_INTL("{1}'s {2} prevents Attack loss!",opponent.pbThis,abilityname))
		elsif opponent.pbReduceStat(PBStats::ATTACK,1, attacker,false, self, showanim)

		  ret=0; showanim=false
		}
		if !attacker.hasMoldBreaker && opponent.hasWorkingAbility(:BIGPECKS) &&
		   !opponent.pbTooLow? (PBStats::DEFENSE)
		   abilityname = PBAbilities.getName(opponent.ability)

		  //battle.pbDisplay(_INTL("{1}'s {2} prevents Defense loss!",opponent.pbThis,abilityname))
		elsif opponent.pbReduceStat(PBStats::DEFENSE,1, attacker,false, self, showanim)

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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if (pbIsDamaging?) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!opponent.pbCanReduceStatStage? (PBStats::ATTACK, attacker,true,self)) return -1;
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=opponent.pbReduceStat(PBStats::ATTACK,2,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		if opponent.pbCanReduceStatStage? (PBStats::ATTACK, attacker,false,self)
		  opponent.pbReduceStat(PBStats::ATTACK,2,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Decreases the target's Defense by 2 stages. (Screech)
	/// <summary>
	public class PokeBattle_Move_04C : PokeBattle_Move
	{
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if (pbIsDamaging?) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!opponent.pbCanReduceStatStage? (PBStats::DEFENSE, attacker,true,self)) return -1;
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=opponent.pbReduceStat(PBStats::DEFENSE,2,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		if opponent.pbCanReduceStatStage? (PBStats::DEFENSE, attacker,false,self)
		  opponent.pbReduceStat(PBStats::DEFENSE,2,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Decreases the target's Speed by 2 stages. (Cotton Spore, Scary Face, String Shot)
	/// <summary>
	public class PokeBattle_Move_04D : PokeBattle_Move
	{
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if (pbIsDamaging?) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (pbTypeImmunityByAbility(pbType(@type, attacker, opponent), attacker, opponent)) return -1;
		if (!opponent.pbCanReduceStatStage? (PBStats::SPEED, attacker,true,self)) return -1;
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		increment=(isConst?(@id, PBMoves,:STRINGSHOT) && !USENEWBATTLEMECHANICS) ? 1 : 2
		ret=opponent.pbReduceStat(PBStats::SPEED, increment, attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		if opponent.pbCanReduceStatStage? (PBStats::SPEED, attacker,false,self)
		  increment=(isConst?(@id, PBMoves,:STRINGSHOT) && !USENEWBATTLEMECHANICS) ? 1 : 2
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if (pbIsDamaging?) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!opponent.pbCanReduceStatStage? (PBStats::SPATK, attacker,true,self)) return -1;
		if attacker.gender==2 || opponent.gender==2 || attacker.gender==opponent.gender
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if !attacker.hasMoldBreaker && opponent.hasWorkingAbility(:OBLIVIOUS)
		  //battle.pbDisplay(_INTL("{1}'s {2} prevents romance!",opponent.pbThis,
			 PBAbilities.getName(opponent.ability)))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=opponent.pbReduceStat(PBStats::SPATK,2,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		if attacker.gender!=2 && opponent.gender!=2 && attacker.gender!=opponent.gender
		  if attacker.hasMoldBreaker || !opponent.hasWorkingAbility(:OBLIVIOUS)
			if opponent.pbCanReduceStatStage? (PBStats::SPATK, attacker,false,self)
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if (pbIsDamaging?) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (!opponent.pbCanReduceStatStage? (PBStats::SPDEF, attacker,true,self)) return -1;
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=opponent.pbReduceStat(PBStats::SPDEF,2,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		if opponent.pbCanReduceStatStage? (PBStats::SPDEF, attacker,false,self)
		  opponent.pbReduceStat(PBStats::SPDEF,2,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Resets all target's stat stages to 0. (Clear Smog)
	/// <summary>
	public class PokeBattle_Move_050 : PokeBattle_Move
	{
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)

		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if opponent.damagestate.calcdamage>0 && !opponent.damagestate.substitute
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		for i in 0...4
		  @battle.battlers[i].stages[PBStats::ATTACK]   = 0
		  @battle.battlers[i].stages[PBStats::DEFENSE]  = 0
		  @battle.battlers[i].stages[PBStats::SPEED]    = 0
		  @battle.battlers[i].stages[PBStats::SPATK]    = 0
		  @battle.battlers[i].stages[PBStats::SPDEF]    = 0
		  @battle.battlers[i].stages[PBStats::ACCURACY] = 0
		  @battle.battlers[i].stages[PBStats::EVASION]  = 0
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		//battle.pbDisplay(_INTL("All stat changes were eliminated!"))
		return 0
	  }
	}



	/// <summary>
	/// User and target swap their Attack and Special Attack stat stages. (Power Swap)
	/// <summary>
	public class PokeBattle_Move_052 : PokeBattle_Move
	{
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)

		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)

		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)

		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)
		for i in [PBStats::ATTACK, PBStats::DEFENSE, PBStats::SPEED,
				  PBStats::SPATK, PBStats::SPDEF, PBStats::ACCURACY, PBStats::EVASION]
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if opponent.pbOwnSide.effects[PBEffects::CraftyShield]
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)
		for i in [PBStats::ATTACK, PBStats::DEFENSE, PBStats::SPEED,
				  PBStats::SPATK, PBStats::SPDEF, PBStats::ACCURACY, PBStats::EVASION]
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if attacker.pbOwnSide.effects[PBEffects::Mist]>0
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		attacker.pbOwnSide.effects[PBEffects::Mist]=5
		if !@battle.pbIsOpposing? (attacker.index)
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)

		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		attacker.attack,attacker.defense=attacker.defense,attacker.attack
		attacker.effects[PBEffects::PowerTrick]= !attacker.effects[PBEffects::PowerTrick]

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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if opponent.effects[PBEffects::Substitute]>0 && !ignoresSubstitute? (attacker)
		   //battle.pbDisplay(_INTL("But it failed!"))  
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if opponent.effects[PBEffects::Substitute]>0 && !ignoresSubstitute? (attacker)
		   //battle.pbDisplay(_INTL("But it failed!"))  
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if opponent.effects[PBEffects::Substitute]>0 && !ignoresSubstitute? (attacker)
		   //battle.pbDisplay(_INTL("But it failed!"))  
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		olda=attacker.hp
		oldo = opponent.hp

		avhp=((attacker.hp+opponent.hp)/2).floor
		attacker.hp=[avhp, attacker.totalhp].min
		opponent.hp=[avhp, opponent.totalhp].min

		@battle.scene.pbHPChanged(attacker, olda)
		@battle.scene.pbHPChanged(opponent, oldo)
		//battle.pbDisplay(_INTL("The battlers shared their pain!"))
		return 0
	  }
	}



	/// <summary>
	/// For 4 rounds, doubles the Speed of all battlers on the user's side. (Tailwind)
	/// <summary>
	public class PokeBattle_Move_05B : PokeBattle_Move
	{
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if attacker.pbOwnSide.effects[PBEffects::Tailwind]>0
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		attacker.pbOwnSide.effects[PBEffects::Tailwind]=4
		if !@battle.pbIsOpposing? (attacker.index)
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)

		blacklist=[
		   0x02,   // Struggle
		   0x14,   // Chatter
		   0x5C,   // Mimic
		   0x5D,   // Sketch
		   0xB6    // Metronome
		]
		if attacker.effects[PBEffects::Transform] ||
		   opponent.lastMoveUsed<=0 ||
		   isConst? (PBMoveData.new(opponent.lastMoveUsed).type,PBTypes,:SHADOW) ||
		   blacklist.include? (PBMoveData.new(opponent.lastMoveUsed).function)
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		for i in attacker.moves
		  if i.id==opponent.lastMoveUsed
			//battle.pbDisplay(_INTL("But it failed!"))
			return -1 
		  }
		}

		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)
		for i in 0...attacker.moves.length
		  if attacker.moves[i].id==@id
			newmove = PBMove.new(opponent.lastMoveUsed)
			  attacker.moves[i]= PokeBattle_Move.pbFromPBMove(@battle, newmove)
  
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)

		blacklist=[
		   0x02,   // Struggle
		   0x14,   // Chatter
		   0x5D    // Sketch
		]
		if attacker.effects[PBEffects::Transform] ||
		   opponent.lastMoveUsedSketch<=0 ||
		   isConst? (PBMoveData.new(opponent.lastMoveUsedSketch).type,PBTypes,:SHADOW) ||
		   blacklist.include? (PBMoveData.new(opponent.lastMoveUsedSketch).function)
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		for i in attacker.moves
		  if i.id==opponent.lastMoveUsedSketch
			//battle.pbDisplay(_INTL("But it failed!"))
			return -1 
		  }
		}
		if opponent.pbOwnSide.effects[PBEffects::CraftyShield]
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)
		for i in 0...attacker.moves.length
		  if attacker.moves[i].id==@id
			newmove = PBMove.new(opponent.lastMoveUsedSketch)
			  attacker.moves[i]= PokeBattle_Move.pbFromPBMove(@battle, newmove)
  
			  party= @battle.pbParty(attacker.index)
  
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if isConst? (attacker.ability, PBAbilities,:MULTITYPE)
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		types =[]
		for i in attacker.moves
		  next if i.id==@id
		  next if PBTypes.isPseudoType? (i.type)
		   next if attacker.pbHasType? (i.type)
		  if !types.include? (i.type)
			 types.push(i.type)
			break if USENEWBATTLEMECHANICS
		  }

		}
		if types.length==0
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		newtype=types[@battle.pbRandom(types.length)]
		attacker.type1=newtype
		attacker.type2= newtype

		attacker.effects[PBEffects::Type3]= -1

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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if isConst? (attacker.ability, PBAbilities,:MULTITYPE)
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if opponent.lastMoveUsed<=0 ||
		   PBTypes.isPseudoType? (PBMoveData.new(opponent.lastMoveUsed).type)
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if opponent.pbOwnSide.effects[PBEffects::CraftyShield]
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		types =[]

		atype=opponent.lastMoveUsedType
		if atype<0

		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		for i in 0..PBTypes.maxValue
		  next if PBTypes.isPseudoType? (i)
		   next if attacker.pbHasType? (i)
			types.push(i) if PBTypes.getEffectiveness(atype, i)<2 
		}
		if types.length==0
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		newtype=types[@battle.pbRandom(types.length)]
		attacker.type1=newtype
		attacker.type2= newtype

		attacker.effects[PBEffects::Type3]= -1

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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if isConst? (attacker.ability, PBAbilities,:MULTITYPE)
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		type = getConst(PBTypes,:NORMAL) || 0
		case @battle.environment
		when PBEnvironment::None;        type=getConst(PBTypes,:NORMAL) || 0
		when PBEnvironment::Grass; type=getConst(PBTypes,:GRASS) || 0
		when PBEnvironment::TallGrass; type=getConst(PBTypes,:GRASS) || 0
		when PBEnvironment::MovingWater; type=getConst(PBTypes,:WATER) || 0
		when PBEnvironment::StillWater; type=getConst(PBTypes,:WATER) || 0
		when PBEnvironment::Underwater; type=getConst(PBTypes,:WATER) || 0
		when PBEnvironment::Cave; type=getConst(PBTypes,:ROCK) || 0
		when PBEnvironment::Rock; type=getConst(PBTypes,:GROUND) || 0
		when PBEnvironment::Sand; type=getConst(PBTypes,:GROUND) || 0
		when PBEnvironment::Forest; type=getConst(PBTypes,:BUG) || 0
		when PBEnvironment::Snow; type=getConst(PBTypes,:ICE) || 0
		when PBEnvironment::Volcano; type=getConst(PBTypes,:FIRE) || 0
		when PBEnvironment::Graveyard; type=getConst(PBTypes,:GHOST) || 0
		when PBEnvironment::Sky; type=getConst(PBTypes,:FLYING) || 0
		when PBEnvironment::Space; type=getConst(PBTypes,:DRAGON) || 0
		}
		if @battle.field.effects[PBEffects::ElectricTerrain]>0
		  type=getConst(PBTypes,:ELECTRIC) if hasConst? (PBTypes,:ELECTRIC)
		elsif @battle.field.effects[PBEffects::GrassyTerrain]>0

		  type= getConst(PBTypes,:GRASS) if hasConst?(PBTypes,:GRASS)

		elsif @battle.field.effects[PBEffects::MistyTerrain]>0

		  type= getConst(PBTypes,:FAIRY) if hasConst?(PBTypes,:FAIRY)

		}
		if attacker.pbHasType? (type)
		   //battle.pbDisplay(_INTL("But it failed!"))
		  return -1  
		}
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		attacker.type1=type
		attacker.type2=type
		attacker.effects[PBEffects::Type3]= -1

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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if opponent.effects[PBEffects::Substitute]>0 && !ignoresSubstitute? (attacker)
		   //battle.pbDisplay(_INTL("But it failed!"))  
		  return -1
		}
		if (pbTypeImmunityByAbility(pbType(@type, attacker, opponent), attacker, opponent)) return -1;
		if isConst? (opponent.ability, PBAbilities,:MULTITYPE)
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)
		if opponent.type1==getConst(PBTypes,:WATER) &&
		   opponent.type2==getConst(PBTypes,:WATER) &&
		   (opponent.effects[PBEffects::Type3]<0 ||
		   opponent.effects[PBEffects::Type3]==getConst(PBTypes,:WATER))
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		opponent.type1=getConst(PBTypes,:WATER)

		opponent.type2=getConst(PBTypes,:WATER)

		opponent.effects[PBEffects::Type3]=-1
		typename=PBTypes.getName(getConst(PBTypes,:WATER))
		//battle.pbDisplay(_INTL("{1} transformed into the {2} type!",opponent.pbThis,typename))
		return 0
	  }
	}



	/// <summary>
	/// User copes target's types. (Reflect Type)
	/// <summary>
	public class PokeBattle_Move_062 : PokeBattle_Move
	{
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if isConst? (attacker.ability, PBAbilities,:MULTITYPE)
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if attacker.pbHasType? (opponent.type1) &&
		   attacker.pbHasType? (opponent.type2) &&
		   attacker.pbHasType? (opponent.effects[PBEffects::Type3]) &&
		   opponent.pbHasType? (attacker.type1) &&
		   opponent.pbHasType? (attacker.type2) &&
		   opponent.pbHasType? (attacker.effects[PBEffects::Type3])
		   //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		attacker.type1=opponent.type1
		attacker.type2=opponent.type2
		attacker.effects[PBEffects::Type3]= -1

		//battle.pbDisplay(_INTL("{1}'s type changed to match {2}'s!", attacker.pbThis, opponent.pbThis(true)))
		return 0
	  }
	}



	/// <summary>
	/// Target's ability becomes Simple. (Simple Beam)
	/// <summary>
	public class PokeBattle_Move_063 : PokeBattle_Move
	{
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if opponent.effects[PBEffects::Substitute]>0 && !ignoresSubstitute? (attacker)
		   //battle.pbDisplay(_INTL("But it failed!"))  
		  return -1
		}
		if isConst? (opponent.ability, PBAbilities,:MULTITYPE) ||
		   isConst? (opponent.ability, PBAbilities,:SIMPLE) ||
		   isConst? (opponent.ability, PBAbilities,:STANCECHANGE) ||
		   isConst? (opponent.ability, PBAbilities,:TRUANT)
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		oldabil=opponent.ability
		opponent.ability=getConst(PBAbilities,:SIMPLE) || 0
		abilityname=PBAbilities.getName(getConst(PBAbilities,:SIMPLE))
		//battle.pbDisplay(_INTL("{1} acquired {2}!",opponent.pbThis,abilityname))
		if opponent.effects[PBEffects::Illusion] && isConst? (oldabil, PBAbilities,:ILLUSION)
		  PBDebug.log("[Ability triggered] #{opponent.pbThis}'s Illusion ended")    
		  opponent.effects[PBEffects::Illusion]=null
		  @battle.scene.pbChangePokemon(opponent, opponent.pokemon)

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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if opponent.effects[PBEffects::Substitute]>0 && !ignoresSubstitute? (attacker)
		   //battle.pbDisplay(_INTL("But it failed!"))  
		  return -1
		}
		if (pbTypeImmunityByAbility(pbType(@type, attacker, opponent), attacker, opponent)) return -1;
		if isConst? (opponent.ability, PBAbilities,:MULTITYPE) ||
		   isConst? (opponent.ability, PBAbilities,:INSOMNIA) ||
		   isConst? (opponent.ability, PBAbilities,:STANCECHANGE) ||
		   isConst? (opponent.ability, PBAbilities,:TRUANT)
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		oldabil=opponent.ability
		opponent.ability=getConst(PBAbilities,:INSOMNIA) || 0
		abilityname=PBAbilities.getName(getConst(PBAbilities,:INSOMNIA))
		//battle.pbDisplay(_INTL("{1} acquired {2}!",opponent.pbThis,abilityname))
		if opponent.effects[PBEffects::Illusion] && isConst? (oldabil, PBAbilities,:ILLUSION)
		  PBDebug.log("[Ability triggered] #{opponent.pbThis}'s Illusion ended")    
		  opponent.effects[PBEffects::Illusion]=null
		  @battle.scene.pbChangePokemon(opponent, opponent.pokemon)

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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if opponent.pbOwnSide.effects[PBEffects::CraftyShield]
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if opponent.ability==0 ||
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
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		oldabil=attacker.ability
		attacker.ability=opponent.ability
		abilityname = PBAbilities.getName(opponent.ability)

		//battle.pbDisplay(_INTL("{1} copied {2}'s {3}!", attacker.pbThis, opponent.pbThis(true),abilityname))
		if attacker.effects[PBEffects::Illusion] && isConst? (oldabil, PBAbilities,:ILLUSION)
		  PBDebug.log("[Ability triggered] #{attacker.pbThis}'s Illusion ended")    
		  attacker.effects[PBEffects::Illusion]=null
		  @battle.scene.pbChangePokemon(attacker, attacker.pokemon)

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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if opponent.effects[PBEffects::Substitute]>0 && !ignoresSubstitute? (attacker)
		   //battle.pbDisplay(_INTL("But it failed!"))  
		  return -1
		}
		if opponent.pbOwnSide.effects[PBEffects::CraftyShield]
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if attacker.ability==0 ||
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
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		oldabil=opponent.ability
		opponent.ability=attacker.ability
		abilityname = PBAbilities.getName(attacker.ability)

		//battle.pbDisplay(_INTL("{1} acquired {2}!", opponent.pbThis, abilityname))
		if opponent.effects[PBEffects::Illusion] && isConst? (oldabil, PBAbilities,:ILLUSION)
		  PBDebug.log("[Ability triggered] #{opponent.pbThis}'s Illusion ended")    
		  opponent.effects[PBEffects::Illusion]=null
		  @battle.scene.pbChangePokemon(opponent, opponent.pokemon)

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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
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
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if opponent.effects[PBEffects::Substitute]>0 && !ignoresSubstitute? (attacker)
		   //battle.pbDisplay(_INTL("But it failed!"))  
		  return -1
		}
		if isConst? (opponent.ability, PBAbilities,:MULTITYPE) ||
		   isConst? (opponent.ability, PBAbilities,:STANCECHANGE)
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		oldabil=opponent.ability
		opponent.effects[PBEffects::GastroAcid]= true

		opponent.effects[PBEffects::Truant]= false

		//battle.pbDisplay(_INTL("{1}'s Ability was suppressed!", opponent.pbThis)) 
		if opponent.effects[PBEffects::Illusion] && isConst? (oldabil, PBAbilities,:ILLUSION)
		  PBDebug.log("[Ability triggered] #{opponent.pbThis}'s Illusion ended")    
		  opponent.effects[PBEffects::Illusion]=null
		  @battle.scene.pbChangePokemon(opponent, opponent.pokemon)

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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)

		blacklist=[
		   0xC9,   // Fly
		   0xCA,   // Dig
		   0xCB,   // Dive
		   0xCC,   // Bounce
		   0xCD,   // Shadow Force
		   0xCE,   // Sky Drop
		   0x14D   // Phantom Force
		]
		if attacker.effects[PBEffects::Transform] ||
		   opponent.effects[PBEffects::Transform] ||
		   opponent.effects[PBEffects::Illusion] ||
		   (opponent.effects[PBEffects::Substitute]>0 && !ignoresSubstitute? (attacker)) ||
		   opponent.effects[PBEffects::SkyDrop] ||
		   blacklist.include? (PBMoveData.new(opponent.effects[PBEffects::TwoTurnAttack]).function)
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if opponent.pbOwnSide.effects[PBEffects::CraftyShield]
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		attacker.effects[PBEffects::Transform]=true
		attacker.type1=opponent.type1
		attacker.type2=opponent.type2
		attacker.effects[PBEffects::Type3]= -1

		attacker.ability= opponent.ability

		attacker.attack= opponent.attack

		attacker.defense= opponent.defense

		attacker.speed= opponent.speed

		attacker.spatk= opponent.spatk

		attacker.spdef= opponent.spdef
		for i in [PBStats::ATTACK, PBStats::DEFENSE, PBStats::SPEED,
				  PBStats::SPATK, PBStats::SPDEF, PBStats::ACCURACY, PBStats::EVASION]
		  attacker.stages[i]= opponent.stages[i]

		}
		for i in 0...4

		  attacker.moves[i]= PokeBattle_Move.pbFromPBMove(
			 @battle, PBMove.new (opponent.moves[i].id))

		  attacker.moves[i].pp=5
		  attacker.moves[i].totalpp=5
		}
		attacker.effects[PBEffects::Disable]= 0

		attacker.effects[PBEffects::DisableMove]= 0

		//battle.pbDisplay(_INTL("{1} transformed into {2}!", attacker.pbThis, opponent.pbThis(true)))
		return 0
	  }
	}



	/// <summary>
	/// Inflicts a fixed 20HP damage. (SonicBoom)
	/// <summary>
	public class PokeBattle_Move_06A : PokeBattle_Move
	{
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		return pbEffectFixedDamage(20, attacker, opponent, hitnum, alltargets, showanimation)
	  }
	}



	/// <summary>
	/// Inflicts a fixed 40HP damage. (Dragon Rage)
	/// <summary>
	public class PokeBattle_Move_06B : PokeBattle_Move
	{
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		return pbEffectFixedDamage(40, attacker, opponent, hitnum, alltargets, showanimation)
	  }
	}



	/// <summary>
	/// Halves the target's current HP. (Super Fang)
	/// <summary>
	public class PokeBattle_Move_06C : PokeBattle_Move
	{
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		return pbEffectFixedDamage([(opponent.hp / 2).floor,1].max, attacker, opponent, hitnum, alltargets, showanimation)
	  }
	}



	/// <summary>
	/// Inflicts damage equal to the user's level. (Night Shade, Seismic Toss)
	/// <summary>
	public class PokeBattle_Move_06D : PokeBattle_Move
	{
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		return pbEffectFixedDamage(attacker.level, attacker, opponent, hitnum, alltargets, showanimation)
	  }
	}



	/// <summary>
	/// Inflicts damage to bring the target's HP down to equal the user's HP. (Endeavor)
	/// <summary>
	public class PokeBattle_Move_06E : PokeBattle_Move
	{
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if attacker.hp>=opponent.hp
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)

		dmg=[(attacker.level * (@battle.pbRandom(101) + 50) / 100).floor,1].max
		return pbEffectFixedDamage(dmg, attacker, opponent, hitnum, alltargets, showanimation)
	  }
	}



	/// <summary>
	/// OHKO. Accuracy increases by difference between levels of user and target.
	/// <summary>
	public class PokeBattle_Move_070 : PokeBattle_Move
	{
		public object pbAccuracyCheck(attacker, opponent){
		if !attacker.hasMoldBreaker && opponent.hasWorkingAbility(:STURDY)
		  //battle.pbDisplay(_INTL("{1} was protected by {2}!",opponent.pbThis,PBAbilities.getName(opponent.ability)))  
		  return false
		}
		if opponent.level>attacker.level
		  //battle.pbDisplay(_INTL("{1} is unaffected!", opponent.pbThis))
		  return false
		}
		acc = @accuracy + attacker.level - opponent.level
		return @battle.pbRandom(100)<acc
	  }

	  public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)

		damage=pbEffectFixedDamage(opponent.totalhp, attacker, opponent, hitnum, alltargets, showanimation)
		if opponent.fainted?
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
		public object pbAddTarget(targets, attacker){
		if attacker.effects[PBEffects::CounterTarget]>=0 &&
		   attacker.pbIsOpposing? (attacker.effects[PBEffects::CounterTarget])
		  if !attacker.pbAddTarget(targets, @battle.battlers[attacker.effects[PBEffects::CounterTarget]])
			attacker.pbRandomTarget(targets)

		  }
		}
	  }

	  public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if attacker.effects[PBEffects::Counter]<0 || !opponent
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		ret = pbEffectFixedDamage([attacker.effects[PBEffects::Counter] * 2, 1].max, attacker, opponent, hitnum, alltargets, showanimation)
		return ret
	  }
	}



	/// <summary>
	/// Counters a specical move used against the user this round, with 2x the power. (Mirror Coat)
	/// <summary>
	public class PokeBattle_Move_072 : PokeBattle_Move
	{
		public object pbAddTarget(targets, attacker){
		if attacker.effects[PBEffects::MirrorCoatTarget]>=0 && 
		   attacker.pbIsOpposing? (attacker.effects[PBEffects::MirrorCoatTarget])
		  if !attacker.pbAddTarget(targets, @battle.battlers[attacker.effects[PBEffects::MirrorCoatTarget]])
			attacker.pbRandomTarget(targets)

		  }
		}
	  }

	  public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if attacker.effects[PBEffects::MirrorCoat]<0 || !opponent
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		ret = pbEffectFixedDamage([attacker.effects[PBEffects::MirrorCoat] * 2, 1].max, attacker, opponent, hitnum, alltargets, showanimation)
		return ret
	  }
	}



	/// <summary>
	/// Counters the last damaging move used against the user this round, with 1.5x
	/// the power. (Metal Burst)
	/// <summary>
	public class PokeBattle_Move_073 : PokeBattle_Move
	{
		public object pbAddTarget(targets, attacker){
		if attacker.lastAttacker.length>0
		  lastattacker=attacker.lastAttacker[attacker.lastAttacker.length - 1]
		  if lastattacker>=0 && attacker.pbIsOpposing? (lastattacker)
			if !attacker.pbAddTarget(targets, @battle.battlers[lastattacker])
			  attacker.pbRandomTarget(targets)

			}
		  }

		}
	  }

	  public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if attacker.lastHPLost==0 || !opponent
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		ret = pbEffectFixedDamage([(attacker.lastHPLost * 1.5).floor, 1].max, attacker, opponent, hitnum, alltargets, showanimation)
		return ret
	  }
	}



	/// <summary>
	/// The target's ally loses 1/16 of its max HP. (Flame Burst)
	/// <summary>
	public class PokeBattle_Move_074 : PokeBattle_Move
	{
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)

		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if opponent.damagestate.calcdamage>0
		  if opponent.pbPartner && !opponent.pbPartner.fainted? &&
			 !opponent.pbPartner.hasWorkingAbility(:MAGICGUARD)
			opponent.pbPartner.pbReduceHP((opponent.pbPartner.totalhp/16).floor)
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
		public object pbModifyDamage(damagemult, attacker, opponent){
		if PBMoveData.new(opponent.effects[PBEffects::TwoTurnAttack]).function==0xCB // Dive
		  return (damagemult*2.0).round
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
		public object pbModifyDamage(damagemult, attacker, opponent){

		ret=damagemult
		if PBMoveData.new(opponent.effects[PBEffects::TwoTurnAttack]).function==0xCA // Dig
		  ret=(damagemult*2.0).round
		}
		if @battle.field.effects[PBEffects::GrassyTerrain]>0
		  ret=(damagemult/2.0).round
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
		public object pbBaseDamage(basedmg, attacker, opponent){
		if PBMoveData.new(opponent.effects[PBEffects::TwoTurnAttack]).function==0xC9 || // Fly
		   PBMoveData.new(opponent.effects[PBEffects::TwoTurnAttack]).function==0xCC || // Bounce
		   PBMoveData.new(opponent.effects[PBEffects::TwoTurnAttack]).function==0xCE || // Sky Drop
		   opponent.effects[PBEffects::SkyDrop]
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
		public object pbBaseDamage(basedmg, attacker, opponent){
		if PBMoveData.new(opponent.effects[PBEffects::TwoTurnAttack]).function==0xC9 || // Fly
		   PBMoveData.new(opponent.effects[PBEffects::TwoTurnAttack]).function==0xCC || // Bounce
		   PBMoveData.new(opponent.effects[PBEffects::TwoTurnAttack]).function==0xCE || // Sky Drop
		   opponent.effects[PBEffects::SkyDrop]
		  return basedmg*2
		}
		return basedmg
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		opponent.pbFlinch(attacker)
	  }
	}



	/// <summary>
	/// Power is doubled if Fusion Flare has already been used this round. (Fusion Bolt)
	/// <summary>
	public class PokeBattle_Move_079 : PokeBattle_Move
	{
		public object pbBaseDamageMultiplier(damagemult, attacker, opponent){
		if @battle.field.effects[PBEffects::FusionBolt]
		  @battle.field.effects[PBEffects::FusionBolt]= false

		  @doubled= true
		  return (damagemult*2.0).round
		}
		return damagemult
	  }

	  public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)

		@doubled=false
		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if opponent.damagestate.calcdamage>0
		  @battle.field.effects[PBEffects::FusionFlare]=true
		}
		return ret
	  }

	  public object pbShowAnimation(id, attacker, opponent, hitnum= 0, alltargets= null, showanimation= true){
		if opponent.damagestate.critical || @doubled
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
		public object pbBaseDamageMultiplier(damagemult, attacker, opponent){
		if @battle.field.effects[PBEffects::FusionFlare]
		  @battle.field.effects[PBEffects::FusionFlare]= false
		  return (damagemult*2.0).round
		}
		return damagemult
	  }

	  public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)

		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if opponent.damagestate.calcdamage>0
		  @battle.field.effects[PBEffects::FusionBolt]=true
		}
		return ret
	  }

	  public object pbShowAnimation(id, attacker, opponent, hitnum= 0, alltargets= null, showanimation= true){
		if opponent.damagestate.critical || @doubled
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
		public object pbBaseDamage(basedmg, attacker, opponent){
		if opponent.status==PBStatuses::POISON &&
		   (opponent.effects[PBEffects::Substitute]==0 || ignoresSubstitute? (attacker))
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
		public object pbBaseDamage(basedmg, attacker, opponent){
		if opponent.status==PBStatuses::PARALYSIS &&
		   (opponent.effects[PBEffects::Substitute]==0 || ignoresSubstitute? (attacker))
		  return basedmg*2
		}
		return basedmg
	  }

	  public object pbEffectAfterHit(attacker, opponent, turneffects){
		if !opponent.fainted? && opponent.damagestate.calcdamage>0 &&
		   !opponent.damagestate.substitute && opponent.status==PBStatuses::PARALYSIS
		  opponent.pbCureStatus

		}
	  }
	}



	/// <summary>
	/// Power is doubled if the target is asleep. Wakes the target up. (Wake-Up Slap)
	/// <summary>
	public class PokeBattle_Move_07D : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, attacker, opponent){
		if opponent.status==PBStatuses::SLEEP &&
		   (opponent.effects[PBEffects::Substitute]==0 || ignoresSubstitute? (attacker))
		  return basedmg*2
		}
		return basedmg
	  }

	  public object pbEffectAfterHit(attacker, opponent, turneffects){
		if !opponent.fainted? && opponent.damagestate.calcdamage>0 &&
		   !opponent.damagestate.substitute && opponent.status==PBStatuses::SLEEP
		  opponent.pbCureStatus

		}
	  }
	}



	/// <summary>
	/// Power is doubled if the user is burned, poisoned or paralyzed. (Facade)
	/// <summary>
	public class PokeBattle_Move_07E : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, attacker, opponent){
		if attacker.status==PBStatuses::POISON ||
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
		public object pbBaseDamage(basedmg, attacker, opponent){
		if opponent.status>0 &&
		   (opponent.effects[PBEffects::Substitute]==0 || ignoresSubstitute? (attacker))
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
		public object pbBaseDamage(basedmg, attacker, opponent){
		if opponent.hp<=opponent.totalhp/2
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
		public object pbBaseDamage(basedmg, attacker, opponent){
		if attacker.lastHPLost>0 && attacker.lastAttacker.include? (opponent.index)
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
		public object pbBaseDamage(basedmg, attacker, opponent){
		if opponent.tookDamage
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
		public object pbBaseDamage(basedmg, attacker, opponent){

		ret=basedmg
		attacker.pbOwnSide.effects[PBEffects::Round].times do

		  ret*=2

		}
		return ret
	  }

	  public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)

		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if opponent.damagestate.calcdamage>0
		  attacker.pbOwnSide.effects[PBEffects::Round]+=1
		  if attacker.pbPartner && !attacker.pbPartner.hasMovedThisRound?
			if @battle.choices[attacker.pbPartner.index][0]==1 // Will use a move
			  partnermove=@battle.choices[attacker.pbPartner.index][2]
			  if partnermove.function==@function
				attacker.pbPartner.effects[PBEffects::MoveNext]= true

				attacker.pbPartner.effects[PBEffects::Quash]= false

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
		public object pbBaseDamage(basedmg, attacker, opponent){
		if @battle.choices[opponent.index][0]!=1 || // Didn't choose a move
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
		public object pbBaseDamage(basedmg, attacker, opponent){
		if attacker.pbOwnSide.effects[PBEffects::LastRoundFainted]>=0 &&
		   attacker.pbOwnSide.effects[PBEffects::LastRoundFainted]==@battle.turncount-1
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
		public object pbBaseDamageMultiplier(damagemult, attacker, opponent){
		if attacker.item==0
		  return (damagemult*2.0).round
		}
		return damagemult
	  }
	}



	/// <summary>
	/// Power is doubled in weather. Type changes depending on the weather. (Weather Ball)
	/// <summary>
	public class PokeBattle_Move_087 : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, attacker, opponent){
		if @battle.pbWeather!=0
		  return basedmg*2
		}
		return basedmg
	  }

	  public object pbModifyType(type, attacker, opponent){

		type=getConst(PBTypes,:NORMAL) || 0
		case @battle.pbWeather
		when PBWeather::SUNNYDAY, PBWeather::HARSHSUN
		  type = (getConst(PBTypes,:FIRE) || type)

		when PBWeather::RAINDANCE, PBWeather::HEAVYRAIN
		  type = (getConst(PBTypes,:WATER) || type)

		when PBWeather::SANDSTORM
		  type = (getConst(PBTypes,:ROCK) || type)

		when PBWeather::HAIL
		  type = (getConst(PBTypes,:ICE) || type)

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
		public object pbBaseDamage(basedmg, attacker, opponent){
		if @battle.switching
		  return basedmg*2
		}
		return basedmg
	  }

	  public object pbAccuracyCheck(attacker, opponent){
		if (@battle.switching) return true;
		return super(attacker, opponent)
	  }
	}



	/// <summary>
	/// Power increases with the user's happiness. (Return)
	/// <summary>
	public class PokeBattle_Move_089 : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, attacker, opponent){
		return [(attacker.happiness * 2 / 5).floor,1].max
	}
	}



	/// <summary>
	/// Power decreases with the user's happiness. (Frustration)
	/// <summary>
	public class PokeBattle_Move_08A : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, attacker, opponent){
		return [((255 - attacker.happiness) * 2 / 5).floor,1].max
	}
	}



	/// <summary>
	/// Power increases with the user's HP. (Eruption, Water Spout)
	/// <summary>
	public class PokeBattle_Move_08B : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, attacker, opponent){
		return [(150 * attacker.hp / attacker.totalhp).floor,1].max
	}
	}



	/// <summary>
	/// Power increases with the target's HP. (Crush Grip, Wring Out)
	/// <summary>
	public class PokeBattle_Move_08C : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, attacker, opponent){
		return [(120 * opponent.hp / opponent.totalhp).floor,1].max
	}
	}



	/// <summary>
	/// Power increases the quicker the target is than the user. (Gyro Ball)
	/// <summary>
	public class PokeBattle_Move_08D : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, attacker, opponent){
		return [[(25 * opponent.pbSpeed / attacker.pbSpeed).floor,150].min,1].max
	}
	}



	/// <summary>
	/// Power increases with the user's positive stat changes (ignores negative ones).
	/// (Stored Power)
	/// <summary>
	public class PokeBattle_Move_08E : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, attacker, opponent){

		mult=1
		for i in [PBStats::ATTACK, PBStats::DEFENSE, PBStats::SPEED,
				  PBStats::SPATK, PBStats::SPDEF, PBStats::ACCURACY, PBStats::EVASION]
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
		public object pbBaseDamage(basedmg, attacker, opponent){

		mult=3
		for i in [PBStats::ATTACK, PBStats::DEFENSE, PBStats::SPEED,
				  PBStats::SPATK, PBStats::SPDEF, PBStats::ACCURACY, PBStats::EVASION]
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
		public object pbModifyType(type, attacker, opponent){

		hp=pbHiddenPower(attacker.iv)

		type=hp[0]
		return type
	  }

	  public object pbBaseDamage(basedmg, attacker, opponent){
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
	  for i in 0..PBTypes.maxValue
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
		public object pbBaseDamage(basedmg, attacker, opponent){

		basedmg=basedmg<<(attacker.effects[PBEffects::FuryCutter]-1) // can be 1 to 4
		return basedmg
	  }
	}



	/// <summary>
	/// Power is multiplied by the number of consecutive rounds in which this move was
	/// used by any Pokémon on the user's side. (Echoed Voice)
	/// <summary>
	public class PokeBattle_Move_092 : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, attacker, opponent){

		basedmg*=attacker.pbOwnSide.effects[PBEffects::EchoedVoiceCounter] // can be 1 to 5
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
		public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)

		ret=super(attacker, opponent, hitnum, alltargets, showanimation)

		attacker.effects[PBEffects::Rage]=true if ret>0
		return ret
	  }
	}



	/// <summary>
	/// Randomly damages or heals the target. (Present)
	/// <summary>
	public class PokeBattle_Move_094 : PokeBattle_Move
	{
		public object pbOnStartUse(attacker)
		// Just to ensure that Parental Bond's second hit damages if the first hit does
		@forcedamage=false
		return true
	  }

	  public object pbBaseDamage(basedmg, attacker, opponent)

		@forcedamage=true
		return @calcbasedmg
	  }

	  public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)

		@calcbasedmg=1
		r=@battle.pbRandom((@forcedamage)? 8 : 10)
		if r<4

		  @calcbasedmg=40

		elsif r<7

		  @calcbasedmg=80

		elsif r<8

		  @calcbasedmg=120
		else
		  if pbTypeModifier(pbType(@type, attacker, opponent),attacker,opponent)==0
			//battle.pbDisplay(_INTL("It doesn't affect {1}...",opponent.pbThis(true)))
			return -1
		  }
		  if opponent.hp==opponent.totalhp
			//battle.pbDisplay(_INTL("But it failed!"))
			return -1
		  }
		  damage = pbCalcDamage(attacker, opponent) // Consumes Gems even if it will heal
		  pbShowAnimation(@id, attacker, opponent,1, alltargets, showanimation) // Healing animation
		  opponent.pbRecoverHP((opponent.totalhp/4).floor,true)
		  //battle.pbDisplay(_INTL("{1} had its HP restored.",opponent.pbThis))   
		  return 0
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
		public object pbOnStartUse(attacker){

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
		magni=magnitudes[@battle.pbRandom(magnitudes.length)]
		@calcbasedmg = basedmg[magni - 4]

		//battle.pbDisplay(_INTL("Magnitude {1}!",magni))
		return true
	  }

	  public object pbBaseDamage(basedmg, attacker, opponent){

		ret=@calcbasedmg
		if PBMoveData.new(opponent.effects[PBEffects::TwoTurnAttack]).function==0xCA // Dig
		  ret*=2
		}
		if @battle.field.effects[PBEffects::GrassyTerrain]>0
		  ret=(ret/2.0).round
		}
		return ret
	  }
	}



	/// <summary>
	/// Power and type depend on the user's held berry. Destroys the berry. (Natural Gift)
	/// <summary>
	public class PokeBattle_Move_096 : PokeBattle_Move
	{
		public object pbOnStartUse(attacker){
		if !pbIsBerry? (attacker.item) ||
		   attacker.effects[PBEffects::Embargo]>0 ||
		   @battle.field.effects[PBEffects::MagicRoom]>0 ||
		   attacker.hasWorkingAbility(:KLUTZ) ||
		   attacker.pbOpposing1.hasWorkingAbility(:UNNERVE) ||
		   attacker.pbOpposing2.hasWorkingAbility(:UNNERVE)
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return false
		}
		@berry = attacker.item
		return true
	  }

	  public object pbBaseDamage(basedmg, attacker, opponent){

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
		for i in damagearray.keys
		  data = damagearray[i]
		  if data
			for j in data
			  if isConst? (@berry, PBItems, j)
				 ret = i

				ret+=20 if USENEWBATTLEMECHANICS
				return ret
			  }

			}
		  }

		}
		return 1
	  }

	  public object pbModifyType(type, attacker, opponent){

		type=getConst(PBTypes,:NORMAL) || 0
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
		for i in typearray.keys
		  data = typearray[i]
		  if data
			for j in data
			  if isConst? (@berry, PBItems, j)
				 type = getConst(PBTypes, i) || type

			  }
			}

		  }
		}
		return type
	  }

	  public object pbEffectAfterHit(attacker, opponent, turneffects){
		if turneffects[PBEffects::TotalDamage]>0
		  attacker.pbConsumeItem
		}
	  }
	}



	/// <summary>
	/// Power increases the less PP this move has. (Trump Card)
	/// <summary>
	public class PokeBattle_Move_097 : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, attacker, opponent){
		dmgs =[200, 80, 60, 50, 40]

		ppleft=[@pp,4].min   // PP is reduced before the move is used
		basedmg = dmgs[ppleft]
		return basedmg
	  }
	}



	/// <summary>
	/// Power increases the less HP the user has. (Flail, Reversal)
	/// <summary>
	public class PokeBattle_Move_098 : PokeBattle_Move
	{
		public object pbBaseDamage(basedmg, attacker, opponent){
		n = (48 * attacker.hp / attacker.totalhp).floor

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
		public object pbBaseDamage(basedmg, attacker, opponent){
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
		public object pbBaseDamage(basedmg, attacker, opponent){
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
		public object pbBaseDamage(basedmg, attacker, opponent){
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if !@battle.doublebattle || opponent.fainted? ||
		   @battle.choices[opponent.index][0]!=1 || // Didn't choose a move
		   opponent.hasMovedThisRound? ||
		   opponent.effects[PBEffects::HelpingHand]
		  //battle.pbDisplay(_INTL("But it failed!"))  
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.effects[PBEffects::HelpingHand]=true
		//battle.pbDisplay(_INTL("{1} is ready to help {2}!",attacker.pbThis,opponent.pbThis(true)))
		return 0
	  }
	}



	/// <summary>
	/// Weakens Electric attacks. (Mud Sport)
	/// <summary>
	public class PokeBattle_Move_09D : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if USENEWBATTLEMECHANICS
		  if @battle.field.effects[PBEffects::MudSportField]>0
			//battle.pbDisplay(_INTL("But it failed!"))
			return -1
		  }
		  pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		  @battle.field.effects[PBEffects::MudSportField]=5
		  //battle.pbDisplay(_INTL("Electricity's power was weakened!"))
		  return 0
		else
		  for i in 0...4
			if attacker.battle.battlers[i].effects[PBEffects::MudSport]
			  //battle.pbDisplay(_INTL("But it failed!"))
			  return -1
			}
		  }

		  pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		  attacker.effects[PBEffects::MudSport]=true
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if USENEWBATTLEMECHANICS
		  if @battle.field.effects[PBEffects::WaterSportField]>0
			//battle.pbDisplay(_INTL("But it failed!"))
			return -1
		  }
		  pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		  @battle.field.effects[PBEffects::WaterSportField]=5
		  //battle.pbDisplay(_INTL("Fire's power was weakened!"))
		  return 0
		else
		  for i in 0...4
			if attacker.battle.battlers[i].effects[PBEffects::WaterSport]
			  //battle.pbDisplay(_INTL("But it failed!"))
			  return -1
			}
		  }

		  pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		  attacker.effects[PBEffects::WaterSport]=true
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
		public object pbModifyType(type, attacker, opponent){
		if isConst? (@id, PBMoves,:JUDGMENT)
		  if (isConst? (attacker.item, PBItems,:FISTPLATE)) return (getConst(PBTypes,:FIGHTING) || 0);
		  if (isConst? (attacker.item, PBItems,:SKYPLATE)) return (getConst(PBTypes,:FLYING) || 0)  ;
		  if (isConst? (attacker.item, PBItems,:TOXICPLATE)) return (getConst(PBTypes,:POISON) || 0)  ;
		  if (isConst? (attacker.item, PBItems,:EARTHPLATE)) return (getConst(PBTypes,:GROUND) || 0)  ;
		  if (isConst? (attacker.item, PBItems,:STONEPLATE)) return (getConst(PBTypes,:ROCK) || 0)    ;
		  if (isConst? (attacker.item, PBItems,:INSECTPLATE)) return (getConst(PBTypes,:BUG) || 0)     ;
		  if (isConst? (attacker.item, PBItems,:SPOOKYPLATE)) return (getConst(PBTypes,:GHOST) || 0)   ;
		  if (isConst? (attacker.item, PBItems,:IRONPLATE)) return (getConst(PBTypes,:STEEL) || 0)   ;
		  if (isConst? (attacker.item, PBItems,:FLAMEPLATE)) return (getConst(PBTypes,:FIRE) || 0)    ;
		  if (isConst? (attacker.item, PBItems,:SPLASHPLATE)) return (getConst(PBTypes,:WATER) || 0)   ;
		  if (isConst? (attacker.item, PBItems,:MEADOWPLATE)) return (getConst(PBTypes,:GRASS) || 0)   ;
		  if (isConst? (attacker.item, PBItems,:ZAPPLATE)) return (getConst(PBTypes,:ELECTRIC) || 0);
		  if (isConst? (attacker.item, PBItems,:MINDPLATE)) return (getConst(PBTypes,:PSYCHIC) || 0) ;
		  if (isConst? (attacker.item, PBItems,:ICICLEPLATE)) return (getConst(PBTypes,:ICE) || 0)     ;
		  if (isConst? (attacker.item, PBItems,:DRACOPLATE)) return (getConst(PBTypes,:DRAGON) || 0)  ;
		  if (isConst? (attacker.item, PBItems,:DREADPLATE)) return (getConst(PBTypes,:DARK) || 0)    ;
		  if (isConst? (attacker.item, PBItems,:PIXIEPLATE)) return (getConst(PBTypes,:FAIRY) || 0)   ;
		elsif isConst? (@id, PBMoves,:TECHNOBLAST)
		  if (isConst? (attacker.item, PBItems,:SHOCKDRIVE)) return getConst(PBTypes,:ELECTRIC);
		  if (isConst? (attacker.item, PBItems,:BURNDRIVE)) return getConst(PBTypes,:FIRE)    ;
		  if (isConst? (attacker.item, PBItems,:CHILLDRIVE)) return getConst(PBTypes,:ICE)     ;
		  if (isConst? (attacker.item, PBItems,:DOUSEDRIVE)) return getConst(PBTypes,:WATER)   ;
		}
		return (getConst(PBTypes,:NORMAL) || 0)
	  }

	  public object pbShowAnimation(id, attacker, opponent, hitnum= 0, alltargets= null, showanimation= true){
		if isConst? (@id, PBMoves,:TECHNOBLAST)
		  anim = 0
		  anim = 1 if isConst? (pbType(@type, attacker, opponent),PBTypes,:ELECTRIC)
		  anim = 2 if isConst? (pbType(@type, attacker, opponent),PBTypes,:FIRE)
		  anim = 3 if isConst? (pbType(@type, attacker, opponent),PBTypes,:ICE)
		  anim = 4 if isConst? (pbType(@type, attacker, opponent),PBTypes,:WATER)
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
		public object pbCritialOverride(attacker, opponent){
		return true
	  }
	}



	/// <summary>
	/// For 5 rounds, foes' attacks cannot become critical hits. (Lucky Chant)
	/// <summary>
	public class PokeBattle_Move_0A1 : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if attacker.pbOwnSide.effects[PBEffects::LuckyChant]>0
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		attacker.pbOwnSide.effects[PBEffects::LuckyChant]=5
		if !@battle.pbIsOpposing? (attacker.index)
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if attacker.pbOwnSide.effects[PBEffects::Reflect]>0
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		attacker.pbOwnSide.effects[PBEffects::Reflect]=5
		attacker.pbOwnSide.effects[PBEffects::Reflect]=8 if attacker.hasWorkingItem(:LIGHTCLAY)
		if !@battle.pbIsOpposing? (attacker.index)
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if attacker.pbOwnSide.effects[PBEffects::LightScreen]>0
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		attacker.pbOwnSide.effects[PBEffects::LightScreen]=5
		attacker.pbOwnSide.effects[PBEffects::LightScreen]=8 if attacker.hasWorkingItem(:LIGHTCLAY)
		if !@battle.pbIsOpposing? (attacker.index)
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
		public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		if @battle.field.effects[PBEffects::ElectricTerrain]>0
		  if opponent.pbCanParalyze? (attacker,false,self)
			opponent.pbParalyze(attacker)
		  }
		  return
		elsif @battle.field.effects[PBEffects::GrassyTerrain]>0
		  if opponent.pbCanSleep?(attacker,false, self)

			opponent.pbSleep
		  }
		  return
		elsif @battle.field.effects[PBEffects::MistyTerrain]>0
		  if opponent.pbCanReduceStatStage?(PBStats::SPATK, attacker,false, self)

			opponent.pbReduceStat(PBStats::SPATK,1,attacker,false,self)
		  }
		  return
		}
		case @battle.environment
		when PBEnvironment::Grass, PBEnvironment::TallGrass, PBEnvironment::Forest
		  if opponent.pbCanSleep? (attacker,false,self)
			opponent.pbSleep
		  }

		when PBEnvironment::MovingWater, PBEnvironment::Underwater
		  if opponent.pbCanReduceStatStage?(PBStats::ATTACK, attacker,false, self)

			opponent.pbReduceStat(PBStats::ATTACK,1,attacker,false,self)
		  }
		when PBEnvironment::StillWater, PBEnvironment::Sky
		  if opponent.pbCanReduceStatStage? (PBStats::SPEED, attacker,false,self)
			opponent.pbReduceStat(PBStats::SPEED,1,attacker,false,self)
		  }
		when PBEnvironment::Sand
		  if opponent.pbCanReduceStatStage? (PBStats::ACCURACY, attacker,false,self)
			opponent.pbReduceStat(PBStats::ACCURACY,1,attacker,false,self)
		  }
		when PBEnvironment::Rock
		  if USENEWBATTLEMECHANICS
			if opponent.pbCanReduceStatStage? (PBStats::ACCURACY, attacker,false,self)
			  opponent.pbReduceStat(PBStats::ACCURACY,1,attacker,false,self)
			}
		  else
			if opponent.effects[PBEffects::Substitute]==0 || ignoresSubstitute? (attacker)
			   opponent.pbFlinch(attacker)

			}
		  }

		when PBEnvironment::Cave, PBEnvironment::Graveyard, PBEnvironment::Space
		  if opponent.effects[PBEffects::Substitute]==0 || ignoresSubstitute?(attacker)
			opponent.pbFlinch(attacker)
		  }

		when PBEnvironment::Snow
		  if opponent.pbCanFreeze?(attacker,false, self)

			opponent.pbFreeze
		  }

		when PBEnvironment::Volcano
		  if opponent.pbCanBurn?(attacker,false, self)

			opponent.pbBurn(attacker)
		  }
		else
		  if opponent.pbCanParalyze? (attacker,false,self)
			opponent.pbParalyze(attacker)
		  }

		}
	  }

	  public object pbShowAnimation(id, attacker, opponent, hitnum= 0, alltargets= null, showanimation= true){

		id=getConst(PBMoves,:BODYSLAM)
		if @battle.field.effects[PBEffects::ElectricTerrain]>0
		  id=getConst(PBMoves,:THUNDERSHOCK) || id
		elsif @battle.field.effects[PBEffects::GrassyTerrain]>0
		  id=getConst(PBMoves,:VINEWHIP) || id
		elsif @battle.field.effects[PBEffects::MistyTerrain]>0
		  id=getConst(PBMoves,:FAIRYWIND) || id
		else
		  case @battle.environment
		  when PBEnvironment::Grass, PBEnvironment::TallGrass
			id = ((USENEWBATTLEMECHANICS) ? getConst(PBMoves,:VINEWHIP) : getConst(PBMoves,:NEEDLEARM)) || id

		  when PBEnvironment::MovingWater; id=getConst(PBMoves,:WATERPULSE) || id
		  when PBEnvironment::StillWater;  id=getConst(PBMoves,:MUDSHOT) || id
		  when PBEnvironment::Underwater;  id=getConst(PBMoves,:WATERPULSE) || id
		  when PBEnvironment::Cave;        id=getConst(PBMoves,:ROCKTHROW) || id
		  when PBEnvironment::Rock;        id=getConst(PBMoves,:MUDSLAP) || id
		  when PBEnvironment::Sand;        id=getConst(PBMoves,:MUDSLAP) || id
		  when PBEnvironment::Forest;      id=getConst(PBMoves,:RAZORLEAF) || id
	/// Ice tiles in Gen 6 should be Ice Shard
		  when PBEnvironment::Snow;        id=getConst(PBMoves,:AVALANCHE) || id
		  when PBEnvironment::Volcano;     id=getConst(PBMoves,:INCINERATE) || id
		  when PBEnvironment::Graveyard;   id=getConst(PBMoves,:SHADOWSNEAK) || id
		  when PBEnvironment::Sky;         id=getConst(PBMoves,:GUST) || id
		  when PBEnvironment::Space;       id=getConst(PBMoves,:SWIFT) || id
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
		public object pbAccuracyCheck(attacker, opponent){
		return true
	  }
	}



	/// <summary>
	/// User's attack next round against the target will definitely hit. (Lock-On, Mind Reader)
	/// <summary>
	public class PokeBattle_Move_0A6 : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if opponent.effects[PBEffects::Substitute]>0 && !ignoresSubstitute? (attacker)
		   //battle.pbDisplay(_INTL("But it failed!"))  
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.effects[PBEffects::LockOn]=2
		opponent.effects[PBEffects::LockOnPos]=attacker.index
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if opponent.pbOwnSide.effects[PBEffects::CraftyShield]
		  //battle.pbDisplay(_INTL("But it failed!"))  
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.effects[PBEffects::Foresight]=true
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if opponent.pbOwnSide.effects[PBEffects::CraftyShield]
		  //battle.pbDisplay(_INTL("But it failed!"))  
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.effects[PBEffects::MiracleEye]=true
		//battle.pbDisplay(_INTL("{1} was identified!",opponent.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// This move ignores target's Defense, Special Defense and evasion stat changes.
	/// (Chip Away, Sacred Sword)
	/// <summary>
	public class PokeBattle_Move_0A9 : PokeBattle_Move
	/// Handled in superclass public object pbAccuracyCheck and public object pbCalcDamage, do not edit!
	}



	/// <summary>
	/// User is protected against moves with the "B" flag this round. (Detect, Protect)
	/// <summary>
	public class PokeBattle_Move_0AA : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		ratesharers=[
		   0xAA,   // Detect, Protect
		   0xAB,   // Quick Guard
		   0xAC,   // Wide Guard
		   0xE8,   // Endure
		   0x14B,  // King's Shield
		   0x14C   // Spiky Shield
		]
		if !ratesharers.include? (PBMoveData.new(attacker.lastMoveUsed).function)
		  attacker.effects[PBEffects::ProtectRate]=1
		}
		unmoved = false
		for poke in @battle.battlers
		  next if poke.index==attacker.index
		  if @battle.choices[poke.index][0]==1 && // Chose a move
			 !poke.hasMovedThisRound?
			unmoved = true; break
		  }
		}
		if !unmoved ||
		   @battle.pbRandom(65536)>=(65536/attacker.effects[PBEffects::ProtectRate]).floor
		  attacker.effects[PBEffects::ProtectRate]= 1

		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		attacker.effects[PBEffects::Protect]=true
		attacker.effects[PBEffects::ProtectRate]*=2
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if attacker.pbOwnSide.effects[PBEffects::QuickGuard]
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
		if !ratesharers.include? (PBMoveData.new(attacker.lastMoveUsed).function)
		  attacker.effects[PBEffects::ProtectRate]=1
		}
		unmoved = false
		for poke in @battle.battlers
		  next if poke.index==attacker.index
		  if @battle.choices[poke.index][0]==1 && // Chose a move
			 !poke.hasMovedThisRound?
			unmoved = true; break
		  }
		}
		if !unmoved ||
		   (!USENEWBATTLEMECHANICS &&
		   @battle.pbRandom(65536)>=(65536/attacker.effects[PBEffects::ProtectRate]).floor)
		  attacker.effects[PBEffects::ProtectRate]=1
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		attacker.pbOwnSide.effects[PBEffects::QuickGuard]=true
		attacker.effects[PBEffects::ProtectRate]*=2
		if !@battle.pbIsOpposing? (attacker.index)
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if attacker.pbOwnSide.effects[PBEffects::WideGuard]
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
		if !ratesharers.include? (PBMoveData.new(attacker.lastMoveUsed).function)
		  attacker.effects[PBEffects::ProtectRate]=1
		}
		unmoved = false
		for poke in @battle.battlers
		  next if poke.index==attacker.index
		  if @battle.choices[poke.index][0]==1 && // Chose a move
			 !poke.hasMovedThisRound?
			unmoved = true; break
		  }
		}
		if !unmoved ||
		   (!USENEWBATTLEMECHANICS &&
		   @battle.pbRandom(65536)>=(65536/attacker.effects[PBEffects::ProtectRate]).floor)
		  attacker.effects[PBEffects::ProtectRate]=1
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		attacker.pbOwnSide.effects[PBEffects::WideGuard]=true
		attacker.effects[PBEffects::ProtectRate]*=2
		if !@battle.pbIsOpposing? (attacker.index)
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if ret>0
		  opponent.effects[PBEffects::ProtectNegation]=true
		  opponent.pbOwnSide.effects[PBEffects::CraftyShield]=false
		}
		return ret
	  }
	}



	/// <summary>
	/// Uses the last move that the target used. (Mirror Move)
	/// <summary>
	public class PokeBattle_Move_0AE : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if opponent.lastMoveUsed<=0 ||
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
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
		if USENEWBATTLEMECHANICS
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
		if @battle.lastMoveUsed<=0 ||
		   blacklist.include? (PBMoveData.new(@battle.lastMoveUsed).function)
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		attacker.pbUseMoveSimple(@battle.lastMoveUsed)
		return 0
	  }
	}



	/// <summary>
	/// Uses the move the target was about to use this round, with 1.5x power. (Me First)
	/// <summary>
	public class PokeBattle_Move_0B0 : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
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
	oppmove=@battle.choices[opponent.index][2]
		if @battle.choices[opponent.index][0]!=1 || // Didn't choose a move
		   opponent.hasMovedThisRound? ||
		   !oppmove || oppmove.id<=0 ||
		   oppmove.pbIsStatus? ||
		   blacklist.include? (oppmove.function)
		   //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		attacker.effects[PBEffects::MeFirst]= true

		attacker.pbUseMoveSimple(oppmove.id)
		attacker.effects[PBEffects::MeFirst]= false
		return 0
	  }
	}



	/// <summary>
	/// This round, reflects all moves with the "C" flag targeting the user back at
	/// their origin. (Magic Coat)
	/// <summary>
	public class PokeBattle_Move_0B1 : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		attacker.effects[PBEffects::MagicCoat]=true
		//battle.pbDisplay(_INTL("{1} shrouded itself with Magic Coat!",attacker.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// This round, snatches all used moves with the "D" flag. (Snatch)
	/// <summary>
	public class PokeBattle_Move_0B2 : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		attacker.effects[PBEffects::Snatch]=true
		//battle.pbDisplay(_INTL("{1} waits for a target to make a move!",attacker.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// Uses a different move depending on the environment. (Nature Power)
	/// <summary>
	public class PokeBattle_Move_0B3 : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		move=getConst(PBMoves,:TRIATTACK) || 0
		case @battle.environment
		when PBEnvironment::Grass, PBEnvironment::TallGrass, PBEnvironment::Forest
		  move = ((USENEWBATTLEMECHANICS) ? getConst(PBMoves,:ENERGYBALL) : getConst(PBMoves,:SEEDBOMB)) || move

		when PBEnvironment::MovingWater; move=getConst(PBMoves,:HYDROPUMP) || move
		when PBEnvironment::StillWater;  move=getConst(PBMoves,:MUDBOMB) || move
		when PBEnvironment::Underwater;  move=getConst(PBMoves,:HYDROPUMP) || move
		when PBEnvironment::Cave
		  move = ((USENEWBATTLEMECHANICS) ? getConst(PBMoves,:POWERGEM) : getConst(PBMoves,:ROCKSLIDE)) || move

		when PBEnvironment::Rock

		  move= ((USENEWBATTLEMECHANICS) ? getConst(PBMoves,:EARTHPOWER) : getConst(PBMoves,:ROCKSLIDE)) || move

		when PBEnvironment::Sand
		  move = ((USENEWBATTLEMECHANICS) ? getConst(PBMoves,:EARTHPOWER) : getConst(PBMoves,:EARTHQUAKE)) || move
		// Ice tiles in Gen 6 should be Ice Beam
		when PBEnvironment::Snow
		  move = ((USENEWBATTLEMECHANICS) ? getConst(PBMoves,:FROSTBREATH) : getConst(PBMoves,:ICEBEAM)) || move

		when PBEnvironment::Volcano; move=getConst(PBMoves,:LAVAPLUME) || move
	when PBEnvironment::Graveyard;   move=getConst(PBMoves,:SHADOWBALL) || move
	when PBEnvironment::Sky;         move=getConst(PBMoves,:AIRSLASH) || move
	when PBEnvironment::Space;       move=getConst(PBMoves,:DRACOMETEOR) || move
	}
		if @battle.field.effects[PBEffects::ElectricTerrain]>0
		  move=getConst(PBMoves,:THUNDERBOLT) || move
		elsif @battle.field.effects[PBEffects::GrassyTerrain]>0
		  move=getConst(PBMoves,:ENERGYBALL) || move
		elsif @battle.field.effects[PBEffects::MistyTerrain]>0
		  move=getConst(PBMoves,:MOONBLAST) || move
		}
		if move==0
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		thismovename = PBMoves.getName(@id)

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

	  public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if attacker.status!=PBStatuses::SLEEP
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
		for i in 0...4
		  found=false
		  next if attacker.moves[i].id==0
		  found=true if blacklist.include? (attacker.moves[i].function)
		  next if found
		  choices.push(i) if @battle.pbCanChooseMove? (attacker.index, i,false,true)
		}
		if choices.length==0
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		choice=choices[@battle.pbRandom(choices.length)]
		attacker.pbUseMoveSimple(attacker.moves[choice].id,-1, attacker.pbOppositeOpposing.index)
		return 0
	  }
	}



	/// <summary>
	/// Uses a random move known by any non-user Pokémon in the user's party. (Assist)
	/// <summary>
	public class PokeBattle_Move_0B5 : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
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
		if USENEWBATTLEMECHANICS
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

		party=@battle.pbParty(attacker.index) // NOTE: pbParty is common to both allies in multi battles
		for i in 0...party.length
		  if i!=attacker.pokemonIndex && party[i] && !(USENEWBATTLEMECHANICS && party[i].egg?)
			for j in party[i].moves
			  next if isConst? (j.type, PBTypes,:SHADOW)
			  next if j.id==0
			  found=false
			  moves.push(j.id) if !blacklist.include? (PBMoveData.new(j.id).function)
			}
		  }

		}
		if moves.length==0
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		move=moves[@battle.pbRandom(moves.length)]
		attacker.pbUseMoveSimple(move)
		return 0
	  }
	}



	/// <summary>
	/// Uses a random move that exists. (Metronome)
	/// <summary>
	public class PokeBattle_Move_0B6 : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
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
		  move=@battle.pbRandom(PBMoves.maxValue)+1
		  next if isConst? (PBMoveData.new(move).type,PBTypes,:SHADOW)
		  found=false
		  if blacklist.include? (PBMoveData.new(move).function)
			found=true
		  else
			for j in blacklistmoves
			  if isConst? (move, PBMoves, j)
				 found = true
				break
			  }
			}

		  }
		  if !found
			pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if opponent.effects[PBEffects::Torment]
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if !attacker.hasMoldBreaker
		  if opponent.hasWorkingAbility(:AROMAVEIL)
			//battle.pbDisplay(_INTL("But it failed because of {1}'s {2}!",
			   opponent.pbThis,PBAbilities.getName(opponent.ability)))
			return -1
		  elsif opponent.pbPartner.hasWorkingAbility(:AROMAVEIL)

			//battle.pbDisplay(_INTL("But it failed because of {1}'s {2}!",
			   opponent.pbPartner.pbThis,PBAbilities.getName(opponent.pbPartner.ability)))
			return -1
		  }
		}

		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.effects[PBEffects::Torment]=true
		//battle.pbDisplay(_INTL("{1} was subjected to torment!",opponent.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// Disables all target's moves that the user also knows. (Imprison)
	/// <summary>
	public class PokeBattle_Move_0B8 : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if attacker.effects[PBEffects::Imprison]
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1  
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		attacker.effects[PBEffects::Imprison]=true
		//battle.pbDisplay(_INTL("{1} sealed the opponent's move(s)!",attacker.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// For 5 rounds, disables the last move the target used. (Disable)
	/// <summary>
	public class PokeBattle_Move_0B9 : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if opponent.effects[PBEffects::Disable]>0
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if !attacker.hasMoldBreaker
		  if opponent.hasWorkingAbility(:AROMAVEIL)
			//battle.pbDisplay(_INTL("But it failed because of {1}'s {2}!",
			   opponent.pbThis,PBAbilities.getName(opponent.ability)))
			return -1
		  elsif opponent.pbPartner.hasWorkingAbility(:AROMAVEIL)

			//battle.pbDisplay(_INTL("But it failed because of {1}'s {2}!",
			   opponent.pbPartner.pbThis,PBAbilities.getName(opponent.pbPartner.ability)))
			return -1
		  }
		}
		for i in opponent.moves
		  if i.id>0 && i.id==opponent.lastMoveUsed && (i.pp>0 || i.totalpp==0)
			pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

			opponent.effects[PBEffects::Disable]=5
			opponent.effects[PBEffects::DisableMove]=opponent.lastMoveUsed
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if opponent.effects[PBEffects::Taunt]>0 ||
		   (USENEWBATTLEMECHANICS &&
		   !attacker.hasMoldBreaker && opponent.hasWorkingAbility(:OBLIVIOUS))
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if !attacker.hasMoldBreaker
		  if opponent.hasWorkingAbility(:AROMAVEIL)
			//battle.pbDisplay(_INTL("But it failed because of {1}'s {2}!",
			   opponent.pbThis,PBAbilities.getName(opponent.ability)))
			return -1
		  elsif opponent.pbPartner.hasWorkingAbility(:AROMAVEIL)

			//battle.pbDisplay(_INTL("But it failed because of {1}'s {2}!",
			   opponent.pbPartner.pbThis,PBAbilities.getName(opponent.pbPartner.ability)))
			return -1
		  }
		}

		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.effects[PBEffects::Taunt]=4
		//battle.pbDisplay(_INTL("{1} fell for the taunt!",opponent.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// For 5 rounds, disables the target's healing moves. (Heal Block)
	/// <summary>
	public class PokeBattle_Move_0BB : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if opponent.effects[PBEffects::HealBlock]>0
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if !attacker.hasMoldBreaker
		  if opponent.hasWorkingAbility(:AROMAVEIL)
			//battle.pbDisplay(_INTL("But it failed because of {1}'s {2}!",
			   opponent.pbThis,PBAbilities.getName(opponent.ability)))
			return -1
		  elsif opponent.pbPartner.hasWorkingAbility(:AROMAVEIL)

			//battle.pbDisplay(_INTL("But it failed because of {1}'s {2}!",
			   opponent.pbPartner.pbThis,PBAbilities.getName(opponent.pbPartner.ability)))
			return -1
		  }
		}

		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.effects[PBEffects::HealBlock]=5
		//battle.pbDisplay(_INTL("{1} was prevented from healing!",opponent.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// For 4 rounds, the target must use the same move each round. (Encore)
	/// <summary>
	public class PokeBattle_Move_0BC : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		blacklist=[
		   0x02,    // Struggle
		   0x5C,    // Mimic
		   0x5D,    // Sketch
		   0x69,    // Transform
		   0xAE,    // Mirror Move
		   0xBC     // Encore
		]
		if opponent.effects[PBEffects::Encore]>0
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if opponent.lastMoveUsed<=0 ||
		   blacklist.include? (PBMoveData.new(opponent.lastMoveUsed).function)
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if !attacker.hasMoldBreaker
		  if opponent.hasWorkingAbility(:AROMAVEIL)
			//battle.pbDisplay(_INTL("But it failed because of {1}'s {2}!",
			   opponent.pbThis,PBAbilities.getName(opponent.ability)))
			return -1
		  elsif opponent.pbPartner.hasWorkingAbility(:AROMAVEIL)

			//battle.pbDisplay(_INTL("But it failed because of {1}'s {2}!",
			   opponent.pbPartner.pbThis,PBAbilities.getName(opponent.pbPartner.ability)))
			return -1
		  }
		}
		for i in 0...4
		  if opponent.lastMoveUsed==opponent.moves[i].id &&
			 (opponent.moves[i].pp>0 || opponent.moves[i].totalpp==0)
			pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

			opponent.effects[PBEffects::Encore]=4
			opponent.effects[PBEffects::EncoreIndex]=i
			opponent.effects[PBEffects::EncoreMove]= opponent.moves[i].id

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

	  public object pbNumHits(attacker){
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

	  public object pbNumHits(attacker){
		return 2
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		if opponent.pbCanPoison? (attacker,false,self)
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

	  public object pbNumHits(attacker){
		return 3
	  }

	  public object successCheckPerHit?
		return @checks
	  }

	  public object pbOnStartUse(attacker)

		@calcbasedmg=@basedamage
		@checks = !attacker.hasWorkingAbility(:SKILLLINK)
		return true
	  }

	  public object pbBaseDamage(basedmg, attacker, opponent){

		ret=@calcbasedmg
		@calcbasedmg+=basedmg
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

	  public object pbNumHits(attacker){
		hitchances =[2, 2, 3, 3, 4, 5]

		ret=hitchances[@battle.pbRandom(hitchances.length)]
		ret = 5 if attacker.hasWorkingAbility(:SKILLLINK)
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

	  public object pbNumHits(attacker){
		return @participants.length
	  }

	  public object pbOnStartUse(attacker){

		party=@battle.pbParty(attacker.index)
		@participants =[]
		for i in 0...party.length
		  if attacker.pokemonIndex==i
			@participants.push(i)

		  elsif party[i] && !party[i].egg? && party[i].hp>0 && party[i].status==0
			@participants.push(i)
		  }

		}
		if @participants.length==0
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return false
		}
		return true
	  }

	  public object pbBaseDamage(basedmg, attacker, opponent){

		party=@battle.pbParty(attacker.index)
		atk = party[@participants[0]].baseStats[1]

		@participants[0]=null; @participants.compact!
		return 5+(atk/10)
	  }
	}



	/// <summary>
	/// Two turn attack. Attacks first turn, skips second turn (if successful).
	/// <summary>
	public class PokeBattle_Move_0C2 : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if opponent.damagestate.calcdamage>0
		  attacker.effects[PBEffects::HyperBeam]=2
		  attacker.currentMove=@id
		}
		return ret
	  }
	}



	/// <summary>
	/// Two turn attack. Skips first turn, attacks second turn. (Razor Wind)
	/// <summary>
	public class PokeBattle_Move_0C3 : PokeBattle_Move
	{
		public object pbTwoTurnAttack(attacker)
		@immediate = false
		if !@immediate && attacker.hasWorkingItem(:POWERHERB)
		  @immediate=true
		}
		if (@immediate) return false;
		return attacker.effects[PBEffects::TwoTurnAttack]==0
	  }

	  public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if @immediate || attacker.effects[PBEffects::TwoTurnAttack]>0
		  pbShowAnimation(@id, attacker, opponent,1, alltargets, showanimation) // Charging anim
		  //battle.pbDisplay(_INTL("{1} whipped up a whirlwind!",attacker.pbThis))
		}
		if @immediate
		  @battle.pbCommonAnimation("UseItem", attacker, null)

		  //battle.pbDisplay(_INTL("{1} became fully charged due to its Power Herb!",attacker.pbThis))
		  attacker.pbConsumeItem
		}
		if (attacker.effects[PBEffects::TwoTurnAttack]>0) return 0;
		return super(attacker, opponent, hitnum, alltargets, showanimation)
	  }
	}



	/// <summary>
	/// Two turn attack. Skips first turn, attacks second turn. (SolarBeam)
	/// Power halved in all weather except sunshine. In sunshine, takes 1 turn instead.
	/// <summary>
	public class PokeBattle_Move_0C4 : PokeBattle_Move
	{
		public object pbTwoTurnAttack(attacker)
		@immediate = false; @sunny=false
		if attacker.effects[PBEffects::TwoTurnAttack]==0
		  if @battle.pbWeather==PBWeather::SUNNYDAY ||
			 @battle.pbWeather==PBWeather::HARSHSUN
			@immediate = true; @sunny=true
		  }
		}
		if !@immediate && attacker.hasWorkingItem(:POWERHERB)
		  @immediate=true
		}
		if (@immediate) return false;
		return attacker.effects[PBEffects::TwoTurnAttack]==0
	  }

	  public object pbBaseDamageMultiplier(damagemult, attacker, opponent){
		if @battle.pbWeather!=0 &&
		   @battle.pbWeather!=PBWeather::SUNNYDAY &&
		   @battle.pbWeather!=PBWeather::HARSHSUN
		  return (damagemult*0.5).round
		}
		return damagemult
	  }

	  public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if @immediate || attacker.effects[PBEffects::TwoTurnAttack]>0
		  pbShowAnimation(@id, attacker, opponent,1, alltargets, showanimation) // Charging anim
		  //battle.pbDisplay(_INTL("{1} took in sunlight!",attacker.pbThis))
		}
		if @immediate && !@sunny
		  @battle.pbCommonAnimation("UseItem", attacker, null)

		  //battle.pbDisplay(_INTL("{1} became fully charged due to its Power Herb!",attacker.pbThis))
		  attacker.pbConsumeItem
		}
		if (attacker.effects[PBEffects::TwoTurnAttack]>0) return 0;
		return super(attacker, opponent, hitnum, alltargets, showanimation)
	  }
	}




	/// <summary>
	/// Two turn attack. Skips first turn, attacks second turn. (Freeze Shock)
	/// May paralyze the target.
	/// <summary>
	public class PokeBattle_Move_0C5 : PokeBattle_Move
	{
		public object pbTwoTurnAttack(attacker)
		@immediate = false
		if !@immediate && attacker.hasWorkingItem(:POWERHERB)
		  @immediate=true
		}
		if (@immediate) return false;
		return attacker.effects[PBEffects::TwoTurnAttack]==0
	  }

	  public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if @immediate || attacker.effects[PBEffects::TwoTurnAttack]>0
		  pbShowAnimation(@id, attacker, opponent,1, alltargets, showanimation) // Charging anim
		  //battle.pbDisplay(_INTL("{1} became cloaked in a freezing light!",attacker.pbThis))
		}
		if @immediate
		  @battle.pbCommonAnimation("UseItem", attacker, null)

		  //battle.pbDisplay(_INTL("{1} became fully charged due to its Power Herb!",attacker.pbThis))
		  attacker.pbConsumeItem
		}
		if (attacker.effects[PBEffects::TwoTurnAttack]>0) return 0;
		return super(attacker, opponent, hitnum, alltargets, showanimation)
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		if opponent.pbCanParalyze? (attacker,false,self)
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
		public object pbTwoTurnAttack(attacker)
		@immediate = false
		if !@immediate && attacker.hasWorkingItem(:POWERHERB)
		  @immediate=true
		}
		if (@immediate) return false;
		return attacker.effects[PBEffects::TwoTurnAttack]==0
	  }

	  public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if @immediate || attacker.effects[PBEffects::TwoTurnAttack]>0
		  pbShowAnimation(@id, attacker, opponent,1, alltargets, showanimation) // Charging anim
		  //battle.pbDisplay(_INTL("{1} became cloaked in freezing air!",attacker.pbThis))
		}
		if @immediate
		  @battle.pbCommonAnimation("UseItem", attacker, null)

		  //battle.pbDisplay(_INTL("{1} became fully charged due to its Power Herb!",attacker.pbThis))
		  attacker.pbConsumeItem
		}
		if (attacker.effects[PBEffects::TwoTurnAttack]>0) return 0;
		return super(attacker, opponent, hitnum, alltargets, showanimation)
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		if opponent.pbCanBurn? (attacker,false,self)
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
		public object pbTwoTurnAttack(attacker)
		@immediate = false
		if !@immediate && attacker.hasWorkingItem(:POWERHERB)
		  @immediate=true
		}
		if (@immediate) return false;
		return attacker.effects[PBEffects::TwoTurnAttack]==0
	  }

	  public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if @immediate || attacker.effects[PBEffects::TwoTurnAttack]>0
		  pbShowAnimation(@id, attacker, opponent,1, alltargets, showanimation) // Charging anim
		  //battle.pbDisplay(_INTL("{1} became cloaked in a harsh light!",attacker.pbThis))
		}
		if @immediate
		  @battle.pbCommonAnimation("UseItem", attacker, null)

		  //battle.pbDisplay(_INTL("{1} became fully charged due to its Power Herb!",attacker.pbThis))
		  attacker.pbConsumeItem
		}
		if (attacker.effects[PBEffects::TwoTurnAttack]>0) return 0;
		return super(attacker, opponent, hitnum, alltargets, showanimation)
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		opponent.pbFlinch(attacker)
	  }
	}



	/// <summary>
	/// Two turn attack. Ups user's Defense by 1 stage first turn, attacks second turn.
	/// (Skull Bash)
	/// <summary>
	public class PokeBattle_Move_0C8 : PokeBattle_Move
	{
		public object pbTwoTurnAttack(attacker)
		@immediate = false
		if !@immediate && attacker.hasWorkingItem(:POWERHERB)
		  @immediate=true
		}
		if (@immediate) return false;
		return attacker.effects[PBEffects::TwoTurnAttack]==0
	  }

	  public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if @immediate || attacker.effects[PBEffects::TwoTurnAttack]>0
		  pbShowAnimation(@id, attacker, opponent,1, alltargets, showanimation) // Charging anim
		  //battle.pbDisplay(_INTL("{1} tucked in its head!",attacker.pbThis))
		  if attacker.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,false,self)
			attacker.pbIncreaseStat(PBStats::DEFENSE,1,attacker,false,self)
		  }
		}
		if @immediate
		  @battle.pbCommonAnimation("UseItem", attacker, null)

		  //battle.pbDisplay(_INTL("{1} became fully charged due to its Power Herb!",attacker.pbThis))
		  attacker.pbConsumeItem
		}
		if (attacker.effects[PBEffects::TwoTurnAttack]>0) return 0;
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

	  public object pbTwoTurnAttack(attacker)
		@immediate = false
		if !@immediate && attacker.hasWorkingItem(:POWERHERB)
		  @immediate=true
		}
		if (@immediate) return false;
		return attacker.effects[PBEffects::TwoTurnAttack]==0
	  }

	  public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if @immediate || attacker.effects[PBEffects::TwoTurnAttack]>0
		  pbShowAnimation(@id, attacker, opponent,1, alltargets, showanimation) // Charging anim
		  //battle.pbDisplay(_INTL("{1} flew up high!",attacker.pbThis))
		}
		if @immediate
		  @battle.pbCommonAnimation("UseItem", attacker, null)

		  //battle.pbDisplay(_INTL("{1} became fully charged due to its Power Herb!",attacker.pbThis))
		  attacker.pbConsumeItem
		}
		if (attacker.effects[PBEffects::TwoTurnAttack]>0) return 0;
		return super(attacker, opponent, hitnum, alltargets, showanimation)
	  }
	}



	/// <summary>
	/// Two turn attack. Skips first turn, attacks second turn. (Dig)
	/// (Handled in Battler's pbSuccessCheck): Is semi-invulnerable during use.
	/// <summary>
	public class PokeBattle_Move_0CA : PokeBattle_Move
	{
		public object pbTwoTurnAttack(attacker)
		@immediate = false
		if !@immediate && attacker.hasWorkingItem(:POWERHERB)
		  @immediate=true
		}
		if (@immediate) return false;
		return attacker.effects[PBEffects::TwoTurnAttack]==0
	  }

	  public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if @immediate || attacker.effects[PBEffects::TwoTurnAttack]>0
		  pbShowAnimation(@id, attacker, opponent,1, alltargets, showanimation) // Charging anim
		  //battle.pbDisplay(_INTL("{1} burrowed its way under the ground!",attacker.pbThis))
		}
		if @immediate
		  @battle.pbCommonAnimation("UseItem", attacker, null)

		  //battle.pbDisplay(_INTL("{1} became fully charged due to its Power Herb!",attacker.pbThis))
		  attacker.pbConsumeItem
		}
		if (attacker.effects[PBEffects::TwoTurnAttack]>0) return 0;
		return super(attacker, opponent, hitnum, alltargets, showanimation)
	  }
	}



	/// <summary>
	/// Two turn attack. Skips first turn, attacks second turn. (Dive)
	/// (Handled in Battler's pbSuccessCheck): Is semi-invulnerable during use.
	/// <summary>
	public class PokeBattle_Move_0CB : PokeBattle_Move
	{
		public object pbTwoTurnAttack(attacker)
		@immediate = false
		if !@immediate && attacker.hasWorkingItem(:POWERHERB)
		  @immediate=true
		}
		if (@immediate) return false;
		return attacker.effects[PBEffects::TwoTurnAttack]==0
	  }

	  public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if @immediate || attacker.effects[PBEffects::TwoTurnAttack]>0
		  pbShowAnimation(@id, attacker, opponent,1, alltargets, showanimation) // Charging anim
		  //battle.pbDisplay(_INTL("{1} hid underwater!",attacker.pbThis))
		}
		if @immediate
		  @battle.pbCommonAnimation("UseItem", attacker, null)

		  //battle.pbDisplay(_INTL("{1} became fully charged due to its Power Herb!",attacker.pbThis))
		  attacker.pbConsumeItem
		}
		if (attacker.effects[PBEffects::TwoTurnAttack]>0) return 0;
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

	  public object pbTwoTurnAttack(attacker)
		@immediate = false
		if !@immediate && attacker.hasWorkingItem(:POWERHERB)
		  @immediate=true
		}
		if (@immediate) return false;
		return attacker.effects[PBEffects::TwoTurnAttack]==0
	  }

	  public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if @immediate || attacker.effects[PBEffects::TwoTurnAttack]>0
		  pbShowAnimation(@id, attacker, opponent,1, alltargets, showanimation) // Charging anim
		  //battle.pbDisplay(_INTL("{1} sprang up!",attacker.pbThis))
		}
		if @immediate
		  @battle.pbCommonAnimation("UseItem", attacker, null)

		  //battle.pbDisplay(_INTL("{1} became fully charged due to its Power Herb!",attacker.pbThis))
		  attacker.pbConsumeItem
		}
		if (attacker.effects[PBEffects::TwoTurnAttack]>0) return 0;
		return super(attacker, opponent, hitnum, alltargets, showanimation)
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		if opponent.pbCanParalyze? (attacker,false,self)
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
		public object pbTwoTurnAttack(attacker)
		@immediate = false
		if !@immediate && attacker.hasWorkingItem(:POWERHERB)
		  @immediate=true
		}
		if (@immediate) return false;
		return attacker.effects[PBEffects::TwoTurnAttack]==0
	  }

	  public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if @immediate || attacker.effects[PBEffects::TwoTurnAttack]>0
		  pbShowAnimation(@id, attacker, opponent,1, alltargets, showanimation) // Charging anim
		  //battle.pbDisplay(_INTL("{1} vanished instantly!",attacker.pbThis))
		}
		if @immediate
		  @battle.pbCommonAnimation("UseItem", attacker, null)

		  //battle.pbDisplay(_INTL("{1} became fully charged due to its Power Herb!",attacker.pbThis))
		  attacker.pbConsumeItem
		}
		if (attacker.effects[PBEffects::TwoTurnAttack]>0) return 0;
		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if ret>0
		  opponent.effects[PBEffects::ProtectNegation]=true
		  opponent.pbOwnSide.effects[PBEffects::CraftyShield]=false
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

	  public object pbMoveFailed(attacker, opponent){
		ret = false

		ret=true if opponent.effects[PBEffects::Substitute]>0 && !ignoresSubstitute? (attacker)
		 ret = true if opponent.effects[PBEffects::TwoTurnAttack]>0
		ret=true if opponent.effects[PBEffects::SkyDrop] && attacker.effects[PBEffects::TwoTurnAttack]>0
		ret=true if !opponent.pbIsOpposing? (attacker.index)
		 ret = true if USENEWBATTLEMECHANICS && opponent.weight(attacker)>=2000
		return ret
	  }

	  public object pbTwoTurnAttack(attacker){
		return attacker.effects[PBEffects::TwoTurnAttack]==0
	  }

	  public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if attacker.effects[PBEffects::TwoTurnAttack]>0
		  pbShowAnimation(@id, attacker, opponent,1, alltargets, showanimation) // Charging anim
		  //battle.pbDisplay(_INTL("{1} took {2} into the sky!",attacker.pbThis,opponent.pbThis(true)))
		  opponent.effects[PBEffects::SkyDrop]=true
		}
		if (attacker.effects[PBEffects::TwoTurnAttack]>0) return 0;
		ret=super
		//battle.pbDisplay(_INTL("{1} was freed from the Sky Drop!", opponent.pbThis))
		opponent.effects[PBEffects::SkyDrop]=false
		return ret
	  }

	  public object pbTypeModifier(type, attacker, opponent){
		return 0 if !attacker.hasMoldBreaker &&
		if (opponent.pbHasType? (:FLYING)) return 0;
		   opponent.hasWorkingAbility(:LEVITATE) && !opponent.effects[PBEffects::SmackDown]
		return super
	  }
	}



	/// <summary>
	/// Trapping move. Traps for 5 or 6 rounds. Trapped Pokémon lose 1/16 of max HP
	/// at } of each round.
	/// <summary>
	public class PokeBattle_Move_0CF : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if opponent.damagestate.calcdamage>0 && !opponent.fainted? &&
		   !opponent.damagestate.substitute
		  if opponent.effects[PBEffects::MultiTurn]==0
			opponent.effects[PBEffects::MultiTurn]=5+@battle.pbRandom(2)
			if attacker.hasWorkingItem(:GRIPCLAW)
			  opponent.effects[PBEffects::MultiTurn]=(USENEWBATTLEMECHANICS)? 8 : 6
			}
			opponent.effects[PBEffects::MultiTurnAttack]= @id

			opponent.effects[PBEffects::MultiTurnUser]= attacker.index
			if isConst?(@id, PBMoves,:BIND)

			  //battle.pbDisplay(_INTL("{1} was squeezed by {2}!",opponent.pbThis,attacker.pbThis(true)))
			elsif isConst? (@id, PBMoves,:CLAMP)
			  //battle.pbDisplay(_INTL("{1} clamped {2}!",attacker.pbThis,opponent.pbThis(true)))
			elsif isConst? (@id, PBMoves,:FIRESPIN)
			  //battle.pbDisplay(_INTL("{1} was trapped in the fiery vortex!",opponent.pbThis))
			elsif isConst? (@id, PBMoves,:MAGMASTORM)
			  //battle.pbDisplay(_INTL("{1} became trapped by Magma Storm!",opponent.pbThis))
			elsif isConst? (@id, PBMoves,:SANDTOMB)
			  //battle.pbDisplay(_INTL("{1} became trapped by Sand Tomb!",opponent.pbThis))
			elsif isConst? (@id, PBMoves,:WRAP)
			  //battle.pbDisplay(_INTL("{1} was wrapped by {2}!",opponent.pbThis,attacker.pbThis(true)))
			elsif isConst? (@id, PBMoves,:INFESTATION)
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
	/// at } of each round. (Whirlpool)
	/// Power is doubled if target is using Dive.
	/// (Handled in Battler's pbSuccessCheck): Hits some semi-invulnerable targets.
	/// <summary>
	public class PokeBattle_Move_0D0 : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if opponent.damagestate.calcdamage>0 && !opponent.fainted? &&
		   !opponent.damagestate.substitute
		  if opponent.effects[PBEffects::MultiTurn]==0
			opponent.effects[PBEffects::MultiTurn]=5+@battle.pbRandom(2)
			if attacker.hasWorkingItem(:GRIPCLAW)
			  opponent.effects[PBEffects::MultiTurn]=(USENEWBATTLEMECHANICS)? 8 : 6
			}
			opponent.effects[PBEffects::MultiTurnAttack]= @id

			opponent.effects[PBEffects::MultiTurnUser]= attacker.index

			//battle.pbDisplay(_INTL("{1} became trapped in the vortex!", opponent.pbThis))
		  }
		}
		return ret
	  }

	  public object pbModifyDamage(damagemult, attacker, opponent){
		if PBMoveData.new(opponent.effects[PBEffects::TwoTurnAttack]).function==0xCB // Dive
		  return (damagemult*2.0).round
		}
		return damagemult
	  }
	}



	/// <summary>
	/// User must use this move for 2 more rounds. No battlers can sleep. (Uproar)
	/// <summary>
	public class PokeBattle_Move_0D1 : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if opponent.damagestate.calcdamage>0
		  if attacker.effects[PBEffects::Uproar]==0
			attacker.effects[PBEffects::Uproar]=3
			//battle.pbDisplay(_INTL("{1} caused an uproar!",attacker.pbThis))
			attacker.currentMove=@id
		  }

		}
		return ret
	  }
	}



	/// <summary>
	/// User must use this move for 1 or 2 more rounds. At }, user becomes confused.
	/// (Outrage, Petal Dange, Thrash)
	/// <summary>
	public class PokeBattle_Move_0D2 : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if opponent.damagestate.calcdamage>0 &&
		   attacker.effects[PBEffects::Outrage]==0 && 
		   attacker.status!=PBStatuses::SLEEP
		  attacker.effects[PBEffects::Outrage]= 2 + @battle.pbRandom(2)

		  attacker.currentMove= @id

		elsif pbTypeModifier(@type, attacker, opponent)==0
		  // Cancel effect if attack is ineffective
		  attacker.effects[PBEffects::Outrage]= 0

		}
		if attacker.effects[PBEffects::Outrage]>0

		  attacker.effects[PBEffects::Outrage]-=1
		  if attacker.effects[PBEffects::Outrage]==0 && attacker.pbCanConfuseSelf?(false)

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
		public object pbBaseDamage(basedmg, attacker, opponent){
		shift = (4 - attacker.effects[PBEffects::Rollout]) // from 0 through 4, 0 is most powerful
		shift+=1 if attacker.effects[PBEffects::DefenseCurl]
		basedmg = basedmg << shift
		return basedmg
	  }

	  public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)

		attacker.effects[PBEffects::Rollout]=5 if attacker.effects[PBEffects::Rollout]==0
		attacker.effects[PBEffects::Rollout]-=1
		attacker.currentMove=thismove.id
		ret = super(attacker, opponent, hitnum, alltargets, showanimation)
		if opponent.damagestate.calcdamage==0 ||
		   pbTypeModifier(@type, attacker, opponent)==0 || 
		   attacker.status==PBStatuses::SLEEP
	/// Cancel effect if attack is ineffective
		  attacker.effects[PBEffects::Rollout]= 0

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
		public object pbDisplayUseMessage(attacker){
		if attacker.effects[PBEffects::Bide]==0
		  //battle.pbDisplayBrief(_INTL("{1} used\r\n{2}!",attacker.pbThis,name))
		  attacker.effects[PBEffects::Bide]=2
		  attacker.effects[PBEffects::BideDamage]=0
		  attacker.effects[PBEffects::BideTarget]=-1
		  attacker.currentMove=@id
		  pbShowAnimation(@id, attacker, null)
		  return 1
		else
		  attacker.effects[PBEffects::Bide]-=1
		  if attacker.effects[PBEffects::Bide]==0
			//battle.pbDisplayBrief(_INTL("{1} unleashed energy!",attacker.pbThis))
			return 0
		  else
			//battle.pbDisplayBrief(_INTL("{1} is storing energy!",attacker.pbThis))
			return 2
		  }
		}
	  }

	  public object pbAddTarget(targets, attacker){
		if attacker.effects[PBEffects::BideTarget]>=0
		  if !attacker.pbAddTarget(targets, @battle.battlers[attacker.effects[PBEffects::BideTarget]])
			attacker.pbRandomTarget(targets)

		  }
		else
		  attacker.pbRandomTarget(targets)
		}
	  }

	  public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if attacker.effects[PBEffects::BideDamage]==0 || !opponent
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if USENEWBATTLEMECHANICS
		  typemod = pbTypeModifier(pbType(@type, attacker, opponent), attacker, opponent)
		  if typemod==0
			//battle.pbDisplay(_INTL("It doesn't affect {1}...",opponent.pbThis(true)))
			return -1
		  }
		}

		ret=pbEffectFixedDamage(attacker.effects[PBEffects::BideDamage]*2, attacker, opponent, hitnum, alltargets, showanimation)
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

	  public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if attacker.hp==attacker.totalhp
		  //battle.pbDisplay(_INTL("{1}'s HP is full!", attacker.pbThis))
		  return -1
		}
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		attacker.pbRecoverHP(((attacker.totalhp+1)/2).floor,true)
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

	  public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if attacker.hp==attacker.totalhp
		  //battle.pbDisplay(_INTL("{1}'s HP is full!", attacker.pbThis))
		  return -1
		}
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		attacker.pbRecoverHP(((attacker.totalhp+1)/2).floor,true)
		attacker.effects[PBEffects::Roost]=true
		//battle.pbDisplay(_INTL("{1}'s HP was restored.",attacker.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// Battler in user's position is healed by 1/2 of its max HP, at the } of the
	/// next round. (Wish)
	/// <summary>
	public class PokeBattle_Move_0D7 : PokeBattle_Move
	{
		public object isHealingMove?
		return true
	  }

	  public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if attacker.effects[PBEffects::Wish]>0
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		attacker.effects[PBEffects::Wish]=2
		attacker.effects[PBEffects::WishAmount]=((attacker.totalhp+1)/2).floor
		attacker.effects[PBEffects::WishMaker]= attacker.pokemonIndex
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

	  public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if attacker.hp==attacker.totalhp
		  //battle.pbDisplay(_INTL("{1}'s HP is full!", attacker.pbThis))
		  return -1
		}
		hpgain = 0
		if @battle.pbWeather==PBWeather::SUNNYDAY ||
		   @battle.pbWeather==PBWeather::HARSHSUN
		  hpgain = (attacker.totalhp * 2 / 3).floor

		elsif @battle.pbWeather!=0
		  hpgain= (attacker.totalhp / 4).floor
		else

		  hpgain= (attacker.totalhp / 2).floor

		}
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)
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

	  public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if !attacker.pbCanSleep? (attacker,true,self,true)
		  return -1
		}
		if attacker.status==PBStatuses::SLEEP
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if attacker.hp==attacker.totalhp
		  //battle.pbDisplay(_INTL("{1}'s HP is full!", attacker.pbThis))
		  return -1
		}
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		attacker.pbSleepSelf(3)
		//battle.pbDisplay(_INTL("{1} slept and became healthy!",attacker.pbThis))
		hp=attacker.pbRecoverHP(attacker.totalhp-attacker.hp,true)
		//battle.pbDisplay(_INTL("{1}'s HP was restored.",attacker.pbThis)) if hp>0
		return 0
	  }
	}



	/// <summary>
	/// Rings the user. Ringed Pokémon gain 1/16 of max HP at the } of each round.
	/// (Aqua Ring)
	/// <summary>
	public class PokeBattle_Move_0DA : PokeBattle_Move
	{
		public object isHealingMove?
		return true
	  }

	  public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if attacker.effects[PBEffects::AquaRing]
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		attacker.effects[PBEffects::AquaRing]=true
		//battle.pbDisplay(_INTL("{1} surrounded itself with a veil of water!",attacker.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// Ingrains the user. Ingrained Pokémon gain 1/16 of max HP at the } of each
	/// round, and cannot flee or switch out. (Ingrain)
	/// <summary>
	public class PokeBattle_Move_0DB : PokeBattle_Move
	{
		public object isHealingMove?
		return true
	  }

	  public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if attacker.effects[PBEffects::Ingrain]
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		attacker.effects[PBEffects::Ingrain]=true
		//battle.pbDisplay(_INTL("{1} planted its roots!",attacker.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// Seeds the target. Seeded Pokémon lose 1/8 of max HP at the } of each round,
	/// and the Pokémon in the user's position gains the same amount. (Leech Seed)
	/// <summary>
	public class PokeBattle_Move_0DC : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if opponent.effects[PBEffects::Substitute]>0 && !ignoresSubstitute? (attacker)
		   //battle.pbDisplay(_INTL("But it failed!"))  
		  return -1
		}
		if (pbTypeImmunityByAbility(pbType(@type, attacker, opponent), attacker, opponent)) return -1;
		if opponent.effects[PBEffects::LeechSeed]>=0
		  //battle.pbDisplay(_INTL("{1} evaded the attack!",opponent.pbThis))
		  return -1
		}
		if opponent.pbHasType? (:GRASS)
		  //battle.pbDisplay(_INTL("It doesn't affect {1}...",opponent.pbThis(true)))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.effects[PBEffects::LeechSeed]=attacker.index
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

	  public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if opponent.damagestate.calcdamage>0
		  hpgain=(opponent.damagestate.hplost/2).round
		  if opponent.hasWorkingAbility(:LIQUIDOOZE)
			attacker.pbReduceHP(hpgain,true)
			//battle.pbDisplay(_INTL("{1} sucked up the liquid ooze!",attacker.pbThis))
		  elsif attacker.effects[PBEffects::HealBlock]==0

			hpgain= (hpgain * 1.3).floor if attacker.hasWorkingItem(:BIGROOT)

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

	  public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if opponent.damagestate.calcdamage>0
		  hpgain=(opponent.damagestate.hplost/2).round
		  if opponent.hasWorkingAbility(:LIQUIDOOZE)
			attacker.pbReduceHP(hpgain,true)
			//battle.pbDisplay(_INTL("{1} sucked up the liquid ooze!",attacker.pbThis))
		  elsif attacker.effects[PBEffects::HealBlock]==0

			hpgain= (hpgain * 1.3).floor if attacker.hasWorkingItem(:BIGROOT)

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

	  public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if opponent.effects[PBEffects::Substitute]>0 && !ignoresSubstitute? (attacker)
		   //battle.pbDisplay(_INTL("But it failed!"))  
		  return -1
		}
		if opponent.hp==opponent.totalhp
		  //battle.pbDisplay(_INTL("{1}'s HP is full!", opponent.pbThis))  
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		hpgain=((opponent.totalhp+1)/2).floor
		hpgain = (opponent.totalhp * 3 / 4).round if attacker.hasWorkingAbility(:MEGALAUNCHER)
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
		public object pbOnStartUse(attacker){
		if !attacker.hasMoldBreaker
		  bearer = @battle.pbCheckGlobalAbility(:DAMP)
		  if bearer!=null
			//battle.pbDisplay(_INTL("{1}'s {2} prevents {3} from using {4}!",
			   bearer.pbThis, PBAbilities.getName(bearer.ability), attacker.pbThis(true),@name))
			return false
		  }
		}
		return true
	  }

	  public object pbShowAnimation(id, attacker, opponent, hitnum= 0, alltargets= null, showanimation= true){

		super(id, attacker, opponent, hitnum, alltargets, showanimation)
		if !attacker.fainted?
		  attacker.pbReduceHP(attacker.hp)

		  attacker.pbFaint if attacker.fainted?
		}
	  }
	}



	/// <summary>
	/// Inflicts fixed damage equal to user's current HP. (Final Gambit)
	/// User faints (if successful).
	/// <summary>
	public class PokeBattle_Move_0E1 : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		typemod=pbTypeModifier(pbType(@type, attacker, opponent), attacker, opponent)
		if typemod==0
		  //battle.pbDisplay(_INTL("It doesn't affect {1}...",opponent.pbThis(true)))
		  return -1
		}
		ret = pbEffectFixedDamage(attacker.hp, attacker, opponent, hitnum, alltargets, showanimation)
		return ret
	  }

	  public object pbShowAnimation(id, attacker, opponent, hitnum= 0, alltargets= null, showanimation= true){

		super(id, attacker, opponent, hitnum, alltargets, showanimation)
		if !attacker.fainted?
		  attacker.pbReduceHP(attacker.hp)

		  attacker.pbFaint if attacker.fainted?
		}
	  }
	}



	/// <summary>
	/// Decreases the target's Attack and Special Attack by 2 stages each. (Memento)
	/// User faints (even if effect does nothing).
	/// <summary>
	public class PokeBattle_Move_0E2 : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if opponent.effects[PBEffects::Substitute]>0 && !ignoresSubstitute? (attacker)
		   //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=-1; showanim=true
		if opponent.pbReduceStat(PBStats::ATTACK,2,attacker,false,self,showanim)
		  ret=0; showanim=false
		}
		if opponent.pbReduceStat(PBStats::SPATK,2,attacker,false,self,showanim)
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

	  public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if !@battle.pbCanChooseNonActive? (attacker.index)
		   //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		attacker.pbReduceHP(attacker.hp)
		attacker.effects[PBEffects::HealingWish]= true
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

	  public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if !@battle.pbCanChooseNonActive? (attacker.index)
		   //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		attacker.pbReduceHP(attacker.hp)
		attacker.effects[PBEffects::LunarDance]= true
		return 0
	  }
	}



	/// <summary>
	/// All current battlers will perish after 3 more rounds. (Perish Song)
	/// <summary>
	public class PokeBattle_Move_0E5 : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		failed=true
		for i in 0...4
		  if @battle.battlers[i].effects[PBEffects::PerishSong]==0 &&
			 (attacker.hasMoldBreaker ||
			 !@battle.battlers[i].hasWorkingAbility(:SOUNDPROOF))
			failed=false; break
		  }
		}
		if failed
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		//battle.pbDisplay(_INTL("All Pokémon that hear the song will faint in three turns!"))
		for i in 0...4
		  if @battle.battlers[i].effects[PBEffects::PerishSong]==0
			if !attacker.hasMoldBreaker && @battle.battlers[i].hasWorkingAbility(:SOUNDPROOF)

			  //battle.pbDisplay(_INTL("{1}'s {2} blocks {3}!",@battle.battlers[i].pbThis,
				 PBAbilities.getName(@battle.battlers[i].ability),@name))
			else
			  @battle.battlers[i].effects[PBEffects::PerishSong]=4
			  @battle.battlers[i].effects[PBEffects::PerishSongUser]=attacker.index
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		attacker.effects[PBEffects::Grudge]=true
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		attacker.effects[PBEffects::DestinyBond]=true
		//battle.pbDisplay(_INTL("{1} is trying to take its foe down with it!",attacker.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// If user would be KO'd this round, it survives with 1HP instead. (Endure)
	/// <summary>
	public class PokeBattle_Move_0E8 : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		ratesharers=[
		   0xAA,   // Detect, Protect
		   0xAB,   // Quick Guard
		   0xAC,   // Wide Guard
		   0xE8,   // Endure
		   0x14B,  // King's Shield
		   0x14C   // Spiky Shield
		]
		if !ratesharers.include? (PBMoveData.new(attacker.lastMoveUsed).function)
		  attacker.effects[PBEffects::ProtectRate]=1
		}
		unmoved = false
		for poke in @battle.battlers
		  next if poke.index==attacker.index
		  if @battle.choices[poke.index][0]==1 && // Chose a move
			 !poke.hasMovedThisRound?
			unmoved = true; break
		  }
		}
		if !unmoved ||
		   @battle.pbRandom(65536)>(65536/attacker.effects[PBEffects::ProtectRate]).floor
		  attacker.effects[PBEffects::ProtectRate]= 1

		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		attacker.effects[PBEffects::Endure]=true
		attacker.effects[PBEffects::ProtectRate]*=2
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if @battle.opponent ||
		   !@battle.pbCanRun? (attacker.index)
		   //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		//battle.pbDisplay(_INTL("{1} fled from battle!",attacker.pbThis))
		@battle.decision=3
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if !attacker.hasMoldBreaker && opponent.hasWorkingAbility(:SUCTIONCUPS)
		  //battle.pbDisplay(_INTL("{1} anchored itself with {2}!",opponent.pbThis,PBAbilities.getName(opponent.ability)))  
		  return -1
		}
		if opponent.effects[PBEffects::Ingrain]
		  //battle.pbDisplay(_INTL("{1} anchored itself with its roots!", opponent.pbThis))  
		  return -1
		}
		if !@battle.opponent
		  if opponent.level>attacker.level
			//battle.pbDisplay(_INTL("But it failed!"))
			return -1
		  }
		  pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		  @battle.decision=3 // Set decision to escaped
		  return 0
		else
		  choices=false
		  party=@battle.pbParty(opponent.index)
		  for i in 0...party.length
			if @battle.pbCanSwitch? (opponent.index, i,false,true)
			  choices=true
			  break
			}
		  }
		  if !choices
			//battle.pbDisplay(_INTL("But it failed!"))
			return -1
		  }
		  pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		  opponent.effects[PBEffects::Roar]=true
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
		public object pbEffectAfterHit(attacker, opponent, turneffects){
		if !attacker.fainted? && !opponent.fainted? &&
		   opponent.damagestate.calcdamage>0 && !opponent.damagestate.substitute && 
		   (attacker.hasMoldBreaker || !opponent.hasWorkingAbility(:SUCTIONCUPS)) &&
		   !opponent.effects[PBEffects::Ingrain]
		  if !@battle.opponent
			if opponent.level<=attacker.level
			  @battle.decision=3 // Set decision to escaped
			}
		  else

			party= @battle.pbParty(opponent.index)
			for i in 0..party.length-1
			  if @battle.pbCanSwitch?(opponent.index, i,false)

				opponent.effects[PBEffects::Roar]=true
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if !@battle.pbCanChooseNonActive? (attacker.index)
		   //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		attacker.effects[PBEffects::BatonPass]=true
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if !attacker.fainted? && opponent.damagestate.calcdamage>0 &&
		   @battle.pbCanChooseNonActive? (attacker.index) &&
		   !@battle.pbAllFainted? (@battle.pbParty(opponent.index))
		  attacker.effects[PBEffects::Uturn]=true
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if pbIsDamaging?
		  ret = super(attacker, opponent, hitnum, alltargets, showanimation)
		  if opponent.damagestate.calcdamage>0 && !opponent.damagestate.substitute &&
			 !opponent.fainted?
			if opponent.effects[PBEffects::MeanLook]<0 &&
			   (!USENEWBATTLEMECHANICS || !opponent.pbHasType? (:GHOST))
			  opponent.effects[PBEffects::MeanLook]=attacker.index
			  //battle.pbDisplay(_INTL("{1} can no longer escape!", opponent.pbThis))
			}
		  }
		  return ret
		}
		if opponent.effects[PBEffects::MeanLook]>=0 ||
		   (opponent.effects[PBEffects::Substitute]>0 && !ignoresSubstitute? (attacker))
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if USENEWBATTLEMECHANICS && opponent.pbHasType? (:GHOST)
		  //battle.pbDisplay(_INTL("It doesn't affect {1}...",opponent.pbThis(true)))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.effects[PBEffects::MeanLook]=attacker.index
		//battle.pbDisplay(_INTL("{1} can no longer escape!", opponent.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// Target drops its item. It regains the item at the } of the battle. (Knock Off)
	/// If target has a losable item, damage is multiplied by 1.5.
	/// <summary>
	public class PokeBattle_Move_0F0 : PokeBattle_Move
	{
		public object pbEffectAfterHit(attacker, opponent, turneffects){
		if !attacker.fainted? && !opponent.fainted? && opponent.item!=0 &&
		   opponent.damagestate.calcdamage>0 && !opponent.damagestate.substitute
		  if !attacker.hasMoldBreaker && opponent.hasWorkingAbility(:STICKYHOLD)
			abilityname=PBAbilities.getName(opponent.ability)
			//battle.pbDisplay(_INTL("{1}'s {2} made {3} ineffective!", opponent.pbThis, abilityname, @name))
		  elsif !@battle.pbIsUnlosableItem(opponent, opponent.item)
			itemname = PBItems.getName(opponent.item)

			opponent.item=0
			opponent.effects[PBEffects::ChoiceBand]=-1
			opponent.effects[PBEffects::Unburden]=true
			//battle.pbDisplay(_INTL("{1} dropped its {2}!",opponent.pbThis,itemname))
		  }
		}
	  }

	  public object pbModifyDamage(damagemult, attacker, opponent){
		if USENEWBATTLEMECHANICS &&
		   !@battle.pbIsUnlosableItem(opponent, opponent.item)
		   // Still boosts damage even if opponent has Sticky Hold
		  return (damagemult*1.5).round
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
		public object pbEffectAfterHit(attacker, opponent, turneffects){
		if !attacker.fainted? && !opponent.fainted? && opponent.item!=0 &&
		   opponent.damagestate.calcdamage>0 && !opponent.damagestate.substitute
		  if !attacker.hasMoldBreaker && opponent.hasWorkingAbility(:STICKYHOLD)
			abilityname=PBAbilities.getName(opponent.ability)
			//battle.pbDisplay(_INTL("{1}'s {2} made {3} ineffective!", opponent.pbThis, abilityname, @name))
		  elsif !@battle.pbIsUnlosableItem(opponent, opponent.item) &&
				!@battle.pbIsUnlosableItem(attacker, opponent.item) &&
				attacker.item==0 &&
				(@battle.opponent || !@battle.pbIsOpposing? (attacker.index))
			itemname=PBItems.getName(opponent.item)
			attacker.item=opponent.item
			opponent.item= 0

			opponent.effects[PBEffects::ChoiceBand]= -1

			opponent.effects[PBEffects::Unburden]= true
			if !@battle.opponent && // In a wild battle
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if (opponent.effects[PBEffects::Substitute]>0 && !ignoresSubstitute? (attacker)) ||
		   (attacker.item==0 && opponent.item==0) ||
		   (!@battle.opponent && @battle.pbIsOpposing? (attacker.index))
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if @battle.pbIsUnlosableItem(opponent, opponent.item) ||
		   @battle.pbIsUnlosableItem(attacker, opponent.item) ||
		   @battle.pbIsUnlosableItem(opponent, attacker.item) ||
		   @battle.pbIsUnlosableItem(attacker, attacker.item)
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if !attacker.hasMoldBreaker && opponent.hasWorkingAbility(:STICKYHOLD)
		  abilityname=PBAbilities.getName(opponent.ability)
		  //battle.pbDisplay(_INTL("{1}'s {2} made {3} ineffective!", opponent.pbThis, abilityname, name))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		oldattitem=attacker.item
		oldoppitem = opponent.item

		oldattitemname=PBItems.getName(oldattitem)
		oldoppitemname = PBItems.getName(oldoppitem)

		tmpitem=attacker.item
		attacker.item=opponent.item
		opponent.item= tmpitem
		if !@battle.opponent && // In a wild battle
		   attacker.pokemon.itemInitial==oldattitem &&

		   opponent.pokemon.itemInitial==oldoppitem
		  attacker.pokemon.itemInitial= oldoppitem

		  opponent.pokemon.itemInitial= oldattitem

		}
		//battle.pbDisplay(_INTL("{1} switched items with its opponent!", attacker.pbThis))
		if oldoppitem>0 && oldattitem>0
		  //battle.pbDisplayPaused(_INTL("{1} obtained {2}.",attacker.pbThis,oldoppitemname))
		  //battle.pbDisplay(_INTL("{1} obtained {2}.",opponent.pbThis,oldattitemname))
		else
		  //battle.pbDisplay(_INTL("{1} obtained {2}.",attacker.pbThis,oldoppitemname)) if oldoppitem>0
		  //battle.pbDisplay(_INTL("{1} obtained {2}.",opponent.pbThis,oldattitemname)) if oldattitem>0
		}
		attacker.effects[PBEffects::ChoiceBand]= -1

		opponent.effects[PBEffects::ChoiceBand]= -1
		return 0
	  }
	}



	/// <summary>
	/// User gives its item to the target. The item remains given after wild battles.
	/// (Bestow)
	/// <summary>
	public class PokeBattle_Move_0F3 : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if (opponent.effects[PBEffects::Substitute]>0 && !ignoresSubstitute? (attacker)) ||
		   attacker.item==0 || opponent.item!=0
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if @battle.pbIsUnlosableItem(attacker, attacker.item) ||
		   @battle.pbIsUnlosableItem(opponent, attacker.item)
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		itemname=PBItems.getName(attacker.item)
		opponent.item=attacker.item
		attacker.item= 0

		attacker.effects[PBEffects::ChoiceBand]= -1

		attacker.effects[PBEffects::Unburden]= true
		if !@battle.opponent && // In a wild battle
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
		public object pbEffectAfterHit(attacker, opponent, turneffects){
		if !attacker.fainted? && !opponent.fainted? && pbIsBerry? (opponent.item) &&
		   opponent.damagestate.calcdamage>0 && !opponent.damagestate.substitute
		  if attacker.hasMoldBreaker || !opponent.hasWorkingAbility(:STICKYHOLD)
			item=opponent.item
			itemname = PBItems.getName(item)

			opponent.pbConsumeItem(false,false)
			//battle.pbDisplay(_INTL("{1} stole and ate its target's {2}!",attacker.pbThis,itemname))
			if !attacker.hasWorkingAbility(:KLUTZ) &&
			   attacker.effects[PBEffects::Embargo]==0
			  attacker.pbActivateBerryEffect(item,false)
			}
			// Symbiosis
			if attacker.item==0 &&
			   attacker.pbPartner && attacker.pbPartner.hasWorkingAbility(:SYMBIOSIS)
			  partner=attacker.pbPartner
			  if partner.item>0 &&
				 !@battle.pbIsUnlosableItem(partner, partner.item) &&
				 !@battle.pbIsUnlosableItem(attacker, partner.item)
				//battle.pbDisplay(_INTL("{1}'s {2} let it share its {3} with {4}!",
				   partner.pbThis, PBAbilities.getName(partner.ability),
				   PBItems.getName(partner.item), attacker.pbThis(true)))
				attacker.item=partner.item
				partner.item=0
				partner.effects[PBEffects::Unburden]= true

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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if !attacker.fainted? && opponent.damagestate.calcdamage>0 &&
		   !opponent.damagestate.substitute &&
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if !attacker.pokemon || attacker.pokemon.itemRecycle==0
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		item=attacker.pokemon.itemRecycle
		itemname = PBItems.getName(item)

		attacker.item=item
		if !@battle.opponent // In a wild battle
		  attacker.pokemon.itemInitial=item if attacker.pokemon.itemInitial==0

		}
		attacker.pokemon.itemRecycle= 0

		attacker.effects[PBEffects::PickupItem]= 0

		attacker.effects[PBEffects::PickupUse]= 0

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
		return {
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

	public object pbMoveFailed(attacker, opponent){
		return true if attacker.item==0 ||
					   @battle.pbIsUnlosableItem(attacker, attacker.item) ||
					   pbIsPokeBall? (attacker.item) ||
					   @battle.field.effects[PBEffects::MagicRoom]>0 ||
					   attacker.hasWorkingAbility(:KLUTZ) ||
					   attacker.effects[PBEffects::Embargo]>0
		for i in flingarray.keys
		  if flingarray[i]
			for j in flingarray[i]
			  if (isConst? (attacker.item, PBItems, j)) return false;
			 }

		  }
		}
		return false if pbIsBerry? (attacker.item) &&
						!attacker.pbOpposing1.hasWorkingAbility(:UNNERVE) &&
						!attacker.pbOpposing2.hasWorkingAbility(:UNNERVE)
		return true
	  }

	  public object pbBaseDamage(basedmg, attacker, opponent){
		if (pbIsBerry? (attacker.item)) return 10;
		if (pbIsMegaStone? (attacker.item)) return 80;
		for i in flingarray.keys
		  if flingarray[i]
			for j in flingarray[i]
			  if (isConst? (attacker.item, PBItems, j)) return i;
			 }

		  }
		}
		return 1
	  }

	  public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if attacker.item==0
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return 0
		}
		attacker.effects[PBEffects::Unburden]= true

		//battle.pbDisplay(_INTL("{1} flung its {2}!", attacker.pbThis, PBItems.getName(attacker.item)))
		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if opponent.damagestate.calcdamage>0 && !opponent.damagestate.substitute &&
		   (attacker.hasMoldBreaker || !opponent.hasWorkingAbility(:SHIELDDUST))
		  if attacker.hasWorkingBerry
			opponent.pbActivateBerryEffect(attacker.item,false)

		  elsif attacker.hasWorkingItem(:FLAMEORB)
			if opponent.pbCanBurn? (attacker,false,self)
			  opponent.pbBurn(attacker)
			}

		  elsif attacker.hasWorkingItem(:KINGSROCK) ||
				attacker.hasWorkingItem(:RAZORFANG)
			opponent.pbFlinch(attacker)
		  elsif attacker.hasWorkingItem(:LIGHTBALL)
			if opponent.pbCanParalyze? (attacker,false,self)
			  opponent.pbParalyze(attacker)
			}

		  elsif attacker.hasWorkingItem(:MENTALHERB)
			if opponent.effects[PBEffects::Attract]>=0
			  opponent.pbCureAttract
			  //battle.pbDisplay(_INTL("{1} got over its infatuation.", opponent.pbThis))
			}
			if opponent.effects[PBEffects::Taunt]>0
			  opponent.effects[PBEffects::Taunt]=0
			  //battle.pbDisplay(_INTL("{1}'s taunt wore off!",opponent.pbThis))
			}
			if opponent.effects[PBEffects::Encore]>0
			  opponent.effects[PBEffects::Encore]=0
			  opponent.effects[PBEffects::EncoreMove]=0
			  opponent.effects[PBEffects::EncoreIndex]=0
			  //battle.pbDisplay(_INTL("{1}'s encore ended!",opponent.pbThis))
			}
			if opponent.effects[PBEffects::Torment]
			  opponent.effects[PBEffects::Torment]= false

			  //battle.pbDisplay(_INTL("{1}'s torment wore off!", opponent.pbThis))
			}
			if opponent.effects[PBEffects::Disable]>0
			  opponent.effects[PBEffects::Disable]=0
			  //battle.pbDisplay(_INTL("{1} is no longer disabled!",opponent.pbThis))
			}
			if opponent.effects[PBEffects::HealBlock]>0
			  opponent.effects[PBEffects::HealBlock]=0
			  //battle.pbDisplay(_INTL("{1}'s Heal Block wore off!",opponent.pbThis))
			}
		  elsif attacker.hasWorkingItem(:POISONBARB)
			if opponent.pbCanPoison? (attacker,false,self)
			  opponent.pbPoison(attacker)
			}

		  elsif attacker.hasWorkingItem(:TOXICORB)
			if opponent.pbCanPoison? (attacker,false,self)
			  opponent.pbPoison(attacker, null,true)
			}
		  elsif attacker.hasWorkingItem(:WHITEHERB)
			while true
			  reducedstats=false
			  for i in [PBStats::ATTACK, PBStats::DEFENSE,
						PBStats::SPEED, PBStats::SPATK, PBStats::SPDEF,
						PBStats::EVASION, PBStats::ACCURACY]
				if opponent.stages[i]<0
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if opponent.effects[PBEffects::Embargo]>0
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.effects[PBEffects::Embargo]=5
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if @battle.field.effects[PBEffects::MagicRoom]>0
		  @battle.field.effects[PBEffects::MagicRoom]=0
		  //battle.pbDisplay(_INTL("The area returned to normal!"))
		else
		  pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		  @battle.field.effects[PBEffects::MagicRoom]=5
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

	  public object pbEffectAfterHit(attacker, opponent, turneffects){
		if !attacker.fainted? && turneffects[PBEffects::TotalDamage]>0
		  if !attacker.hasWorkingAbility(:ROCKHEAD) &&
			 !attacker.hasWorkingAbility(:MAGICGUARD)
			attacker.pbReduceHP((turneffects[PBEffects::TotalDamage]/4.0).round)
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

	  public object pbEffectAfterHit(attacker, opponent, turneffects){
		if !attacker.fainted? && turneffects[PBEffects::TotalDamage]>0
		  if !attacker.hasWorkingAbility(:ROCKHEAD) &&
			 !attacker.hasWorkingAbility(:MAGICGUARD)
			attacker.pbReduceHP((turneffects[PBEffects::TotalDamage]/3.0).round)
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

	  public object pbEffectAfterHit(attacker, opponent, turneffects){
		if !attacker.fainted? && turneffects[PBEffects::TotalDamage]>0
		  if !attacker.hasWorkingAbility(:ROCKHEAD) &&
			 !attacker.hasWorkingAbility(:MAGICGUARD)
			attacker.pbReduceHP((turneffects[PBEffects::TotalDamage]/2.0).round)
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

	  public object pbEffectAfterHit(attacker, opponent, turneffects){
		if !attacker.fainted? && turneffects[PBEffects::TotalDamage]>0
		  if !attacker.hasWorkingAbility(:ROCKHEAD) &&
			 !attacker.hasWorkingAbility(:MAGICGUARD)
			attacker.pbReduceHP((turneffects[PBEffects::TotalDamage]/3.0).round)
			//battle.pbDisplay(_INTL("{1} is damaged by recoil!",attacker.pbThis))
		  }
		}
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		if opponent.pbCanParalyze? (attacker,false,self)
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

	  public object pbEffectAfterHit(attacker, opponent, turneffects){
		if !attacker.fainted? && turneffects[PBEffects::TotalDamage]>0
		  if !attacker.hasWorkingAbility(:ROCKHEAD) &&
			 !attacker.hasWorkingAbility(:MAGICGUARD)
			attacker.pbReduceHP((turneffects[PBEffects::TotalDamage]/3.0).round)
			//battle.pbDisplay(_INTL("{1} is damaged by recoil!",attacker.pbThis))
		  }
		}
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		if opponent.pbCanBurn? (attacker,false,self)
		  opponent.pbBurn(attacker)
		}
	  }
	}



	/// <summary>
	/// Starts sunny weather. (Sunny Day)
	/// <summary>
	public class PokeBattle_Move_0FF : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		case @battle.weather
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
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		@battle.weather=PBWeather::SUNNYDAY
		@battle.weatherduration=5
		@battle.weatherduration= 8 if attacker.hasWorkingItem(:HEATROCK)

		@battle.pbCommonAnimation("Sunny",null,null)
		//battle.pbDisplay(_INTL("The sunlight turned harsh!"))
		return 0
	  }
	}



	/// <summary>
	/// Starts rainy weather. (Rain Dance)
	/// <summary>
	public class PokeBattle_Move_100 : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		case @battle.weather
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
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		@battle.weather=PBWeather::RAINDANCE
		@battle.weatherduration=5
		@battle.weatherduration= 8 if attacker.hasWorkingItem(:DAMPROCK)

		@battle.pbCommonAnimation("Rain",null,null)
		//battle.pbDisplay(_INTL("It started to rain!"))
		return 0
	  }
	}



	/// <summary>
	/// Starts sandstorm weather. (Sandstorm)
	/// <summary>
	public class PokeBattle_Move_101 : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		case @battle.weather
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
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		@battle.weather=PBWeather::SANDSTORM
		@battle.weatherduration=5
		@battle.weatherduration= 8 if attacker.hasWorkingItem(:SMOOTHROCK)

		@battle.pbCommonAnimation("Sandstorm",null,null)
		//battle.pbDisplay(_INTL("A sandstorm brewed!"))
		return 0
	  }
	}



	/// <summary>
	/// Starts hail weather. (Hail)
	/// <summary>
	public class PokeBattle_Move_102 : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		case @battle.weather
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
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		@battle.weather=PBWeather::HAIL
		@battle.weatherduration=5
		@battle.weatherduration= 8 if attacker.hasWorkingItem(:ICYROCK)

		@battle.pbCommonAnimation("Hail",null,null)
		//battle.pbDisplay(_INTL("It started to hail!"))
		return 0
	  }
	}



	/// <summary>
	/// Entry hazard. Lays spikes on the opposing side (max. 3 layers). (Spikes)
	/// <summary>
	public class PokeBattle_Move_103 : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if attacker.pbOpposingSide.effects[PBEffects::Spikes]>=3
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		attacker.pbOpposingSide.effects[PBEffects::Spikes]+=1
		if !@battle.pbIsOpposing? (attacker.index)
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if attacker.pbOpposingSide.effects[PBEffects::ToxicSpikes]>=2
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		attacker.pbOpposingSide.effects[PBEffects::ToxicSpikes]+=1
		if !@battle.pbIsOpposing? (attacker.index)
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if attacker.pbOpposingSide.effects[PBEffects::StealthRock]
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		attacker.pbOpposingSide.effects[PBEffects::StealthRock]=true
		if !@battle.pbIsOpposing? (attacker.index)
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
		public object pbOnStartUse(attacker)
		@doubledamage = false; @overridetype=false
		if attacker.effects[PBEffects::FirstPledge]==0x107 ||   // Fire Pledge
		   attacker.effects[PBEffects::FirstPledge]==0x108      // Water Pledge
		  //battle.pbDisplay(_INTL("The two moves have become one! It's a combined move!"))
		  @doubledamage=true
		  if attacker.effects[PBEffects::FirstPledge]==0x107   // Fire Pledge
			@overridetype=true
		  }
		}
		return true
	  }

	  public object pbBaseDamage(basedmg, attacker, opponent){
		if @doubledamage
		  return basedmg*2
		}
		return basedmg
	  }

	  public object pbModifyType(type, attacker, opponent){
		if @overridetype
		  type = getConst(PBTypes,:FIRE) || 0

		}
		return super(type, attacker, opponent)
	  }

	  public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if !@battle.doublebattle || !attacker.pbPartner || attacker.pbPartner.fainted?
		  attacker.effects[PBEffects::FirstPledge]= 0
		  return super(attacker, opponent, hitnum, alltargets, showanimation)
		}
		// Combined move's effect
		if attacker.effects[PBEffects::FirstPledge]==0x107   // Fire Pledge
		  ret= super(attacker, opponent, hitnum, alltargets, showanimation)
		  if opponent.damagestate.calcdamage>0

			attacker.pbOpposingSide.effects[PBEffects::SeaOfFire]= 4
			if !@battle.pbIsOpposing?(attacker.index)
			  //battle.pbDisplay(_INTL("A sea of fire enveloped the opposing team!"))
			  @battle.pbCommonAnimation("SeaOfFireOpp",null,null)
			else
			  //battle.pbDisplay(_INTL("A sea of fire enveloped your team!"))
			  @battle.pbCommonAnimation("SeaOfFire",null,null)
			}
		  }

		  attacker.effects[PBEffects::FirstPledge]=0
		  return ret
		elsif attacker.effects[PBEffects::FirstPledge]==0x108   // Water Pledge
		  ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		  if opponent.damagestate.calcdamage>0
			attacker.pbOpposingSide.effects[PBEffects::Swamp]=4
			if !@battle.pbIsOpposing? (attacker.index)
			   //battle.pbDisplay(_INTL("A swamp enveloped the opposing team!"))
			  @battle.pbCommonAnimation("SwampOpp",null,null)
			else
			  //battle.pbDisplay(_INTL("A swamp enveloped your team!"))
			  @battle.pbCommonAnimation("Swamp",null,null)
			}
		  }

		  attacker.effects[PBEffects::FirstPledge]=0
		  return ret
		}
		// Set up partner for a combined move
		attacker.effects[PBEffects::FirstPledge]=0
		partnermove=-1
		if @battle.choices[attacker.pbPartner.index][0]==1 // Chose a move
		  if !attacker.pbPartner.hasMovedThisRound?
			move = @battle.choices[attacker.pbPartner.index][2]
			if move && move.id>0
			  partnermove=@battle.choices[attacker.pbPartner.index][2].function
			}

		  }
		}
		if partnermove==0x107 ||   // Fire Pledge
		   partnermove==0x108      // Water Pledge
		  //battle.pbDisplay(_INTL("{1} is waiting for {2}'s move...",attacker.pbThis,attacker.pbPartner.pbThis(true)))
		  attacker.pbPartner.effects[PBEffects::FirstPledge]==@function
		  attacker.pbPartner.effects[PBEffects::MoveNext]= true
		  return 0

		}
		// Use the move on its own
		return super(attacker, opponent, hitnum, alltargets, showanimation)
	  }

	  public object pbShowAnimation(id, attacker, opponent, hitnum= 0, alltargets= null, showanimation= true){
		if @overridetype
		  return super(getConst(PBMoves,:FIREPLEDGE),attacker,opponent,hitnum,alltargets,showanimation)
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
		public object pbOnStartUse(attacker)
		@doubledamage = false; @overridetype=false
		if attacker.effects[PBEffects::FirstPledge]==0x106 ||   // Grass Pledge
		   attacker.effects[PBEffects::FirstPledge]==0x108      // Water Pledge
		  //battle.pbDisplay(_INTL("The two moves have become one! It's a combined move!"))
		  @doubledamage=true
		  if attacker.effects[PBEffects::FirstPledge]==0x108   // Water Pledge
			@overridetype=true
		  }
		}
		return true
	  }

	  public object pbBaseDamage(basedmg, attacker, opponent){
		if @doubledamage
		  return basedmg*2
		}
		return basedmg
	  }

	  public object pbModifyType(type, attacker, opponent){
		if @overridetype
		  type = getConst(PBTypes,:WATER) || 0

		}
		return super(type, attacker, opponent)
	  }

	  public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if !@battle.doublebattle || !attacker.pbPartner || attacker.pbPartner.fainted?
		  attacker.effects[PBEffects::FirstPledge]= 0
		  return super(attacker, opponent, hitnum, alltargets, showanimation)
		}
		// Combined move's effect
		if attacker.effects[PBEffects::FirstPledge]==0x106   // Grass Pledge
		  ret= super(attacker, opponent, hitnum, alltargets, showanimation)
		  if opponent.damagestate.calcdamage>0

			attacker.pbOpposingSide.effects[PBEffects::SeaOfFire]= 4
			if !@battle.pbIsOpposing?(attacker.index)
			  //battle.pbDisplay(_INTL("A sea of fire enveloped the opposing team!"))
			  @battle.pbCommonAnimation("SeaOfFireOpp",null,null)
			else
			  //battle.pbDisplay(_INTL("A sea of fire enveloped your team!"))
			  @battle.pbCommonAnimation("SeaOfFire",null,null)
			}
		  }

		  attacker.effects[PBEffects::FirstPledge]=0
		  return ret
		elsif attacker.effects[PBEffects::FirstPledge]==0x108   // Water Pledge
		  ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		  if opponent.damagestate.calcdamage>0
			attacker.pbOwnSide.effects[PBEffects::Rainbow]=4
			if !@battle.pbIsOpposing? (attacker.index)
			   //battle.pbDisplay(_INTL("A rainbow appeared in the sky on your team's side!"))
			  @battle.pbCommonAnimation("Rainbow",null,null)
			else
			  //battle.pbDisplay(_INTL("A rainbow appeared in the sky on the opposing team's side!"))
			  @battle.pbCommonAnimation("RainbowOpp",null,null)
			}
		  }

		  attacker.effects[PBEffects::FirstPledge]=0
		  return ret
		}
		// Set up partner for a combined move
		attacker.effects[PBEffects::FirstPledge]=0
		partnermove=-1
		if @battle.choices[attacker.pbPartner.index][0]==1 // Chose a move
		  if !attacker.pbPartner.hasMovedThisRound?
			move = @battle.choices[attacker.pbPartner.index][2]
			if move && move.id>0
			  partnermove=@battle.choices[attacker.pbPartner.index][2].function
			}

		  }
		}
		if partnermove==0x106 ||   // Grass Pledge
		   partnermove==0x108      // Water Pledge
		  //battle.pbDisplay(_INTL("{1} is waiting for {2}'s move...",attacker.pbThis,attacker.pbPartner.pbThis(true)))
		  attacker.pbPartner.effects[PBEffects::FirstPledge]==@function
		  attacker.pbPartner.effects[PBEffects::MoveNext]= true
		  return 0

		}
		// Use the move on its own
		return super(attacker, opponent, hitnum, alltargets, showanimation)
	  }

	  public object pbShowAnimation(id, attacker, opponent, hitnum= 0, alltargets= null, showanimation= true){
		if @overridetype
		  return super(getConst(PBMoves,:WATERPLEDGE),attacker,opponent,hitnum,alltargets,showanimation)
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
		public object pbOnStartUse(attacker)
		@doubledamage = false; @overridetype=false
		if attacker.effects[PBEffects::FirstPledge]==0x106 ||   // Grass Pledge
		   attacker.effects[PBEffects::FirstPledge]==0x107      // Fire Pledge
		  //battle.pbDisplay(_INTL("The two moves have become one! It's a combined move!"))
		  @doubledamage=true
		  if attacker.effects[PBEffects::FirstPledge]==0x106   // Grass Pledge
			@overridetype=true
		  }
		}
		return true
	  }

	  public object pbBaseDamage(basedmg, attacker, opponent){
		if @doubledamage
		  return basedmg*2
		}
		return basedmg
	  }

	  public object pbModifyType(type, attacker, opponent){
		if @overridetype
		  type = getConst(PBTypes,:GRASS) || 0

		}
		return super(type, attacker, opponent)
	  }

	  public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if !@battle.doublebattle || !attacker.pbPartner || attacker.pbPartner.fainted?
		  attacker.effects[PBEffects::FirstPledge]= 0
		  return super(attacker, opponent, hitnum, alltargets, showanimation)
		}
		// Combined move's effect
		if attacker.effects[PBEffects::FirstPledge]==0x106   // Grass Pledge
		  ret= super(attacker, opponent, hitnum, alltargets, showanimation)
		  if opponent.damagestate.calcdamage>0

			attacker.pbOpposingSide.effects[PBEffects::Swamp]= 4
			if !@battle.pbIsOpposing?(attacker.index)
			  //battle.pbDisplay(_INTL("A swamp enveloped the opposing team!"))
			  @battle.pbCommonAnimation("SwampOpp",null,null)
			else
			  //battle.pbDisplay(_INTL("A swamp enveloped your team!"))
			  @battle.pbCommonAnimation("Swamp",null,null)
			}
		  }

		  attacker.effects[PBEffects::FirstPledge]=0
		  return ret
		elsif attacker.effects[PBEffects::FirstPledge]==0x107   // Fire Pledge
		  ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		  if opponent.damagestate.calcdamage>0
			attacker.pbOwnSide.effects[PBEffects::Rainbow]=4
			if !@battle.pbIsOpposing? (attacker.index)
			   //battle.pbDisplay(_INTL("A rainbow appeared in the sky on your team's side!"))
			  @battle.pbCommonAnimation("Rainbow",null,null)
			else
			  //battle.pbDisplay(_INTL("A rainbow appeared in the sky on the opposing team's side!"))
			  @battle.pbCommonAnimation("RainbowOpp",null,null)
			}
		  }

		  attacker.effects[PBEffects::FirstPledge]=0
		  return ret
		}
		// Set up partner for a combined move
		attacker.effects[PBEffects::FirstPledge]=0
		partnermove=-1
		if @battle.choices[attacker.pbPartner.index][0]==1 // Chose a move
		  if !attacker.pbPartner.hasMovedThisRound?
			move = @battle.choices[attacker.pbPartner.index][2]
			if move && move.id>0
			  partnermove=@battle.choices[attacker.pbPartner.index][2].function
			}

		  }
		}
		if partnermove==0x106 ||   // Grass Pledge
		   partnermove==0x107      // Fire Pledge
		  //battle.pbDisplay(_INTL("{1} is waiting for {2}'s move...",attacker.pbThis,attacker.pbPartner.pbThis(true)))
		  attacker.pbPartner.effects[PBEffects::FirstPledge]==@function
		  attacker.pbPartner.effects[PBEffects::MoveNext]= true
		  return 0

		}
		// Use the move on its own
		return super(attacker, opponent, hitnum, alltargets, showanimation)
	  }

	  public object pbShowAnimation(id, attacker, opponent, hitnum= 0, alltargets= null, showanimation= true){
		if @overridetype
		  return super(getConst(PBMoves,:GRASSPLEDGE),attacker,opponent,hitnum,alltargets,showanimation)
		}
		return super(id, attacker, opponent, hitnum, alltargets, showanimation)
	  }
	}



	/// <summary>
	/// Scatters coins that the player picks up after winning the battle. (Pay Day)
	/// <summary>
	public class PokeBattle_Move_109 : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if opponent.damagestate.calcdamage>0
		  if @battle.pbOwnedByPlayer? (attacker.index)
			 @battle.extramoney+=5*attacker.level
			 @battle.extramoney= MAXMONEY if @battle.extramoney>MAXMONEY
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
		public object pbCalcDamage(attacker, opponent){
		return super(attacker, opponent, PokeBattle_Move::NOREFLECT)
	  }

	  public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)

		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if attacker.pbOpposingSide.effects[PBEffects::Reflect]>0
		  attacker.pbOpposingSide.effects[PBEffects::Reflect]=0
		  if !@battle.pbIsOpposing? (attacker.index)
			 //battle.pbDisplay(_INTL("The opposing team's Reflect wore off!"))
		  else
			//battle.pbDisplayPaused(_INTL("Your team's Reflect wore off!"))
		  }
		}
		if attacker.pbOpposingSide.effects[PBEffects::LightScreen]>0
		  attacker.pbOpposingSide.effects[PBEffects::LightScreen]=0
		  if !@battle.pbIsOpposing? (attacker.index)
			 //battle.pbDisplay(_INTL("The opposing team's Light Screen wore off!"))
		  else
			//battle.pbDisplay(_INTL("Your team's Light Screen wore off!"))
		  }
		}
		return ret
	  }

	  public object pbShowAnimation(id, attacker, opponent, hitnum= 0, alltargets= null, showanimation= true){
		if attacker.pbOpposingSide.effects[PBEffects::Reflect]>0 ||
		   attacker.pbOpposingSide.effects[PBEffects::LightScreen]>0
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if attacker.effects[PBEffects::Substitute]>0
		  //battle.pbDisplay(_INTL("{1} already has a substitute!",attacker.pbThis))
		  return -1
		}
		sublife =[(attacker.totalhp / 4).floor, 1].max
		if attacker.hp<=sublife
		  //battle.pbDisplay(_INTL("It was too weak to make a substitute!"))
		  return -1  
		}
		attacker.pbReduceHP(sublife,false,false)

		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		attacker.effects[PBEffects::MultiTurn]=0
		attacker.effects[PBEffects::MultiTurnAttack]=0
		attacker.effects[PBEffects::Substitute]=sublife
		//battle.pbDisplay(_INTL("{1} put in a substitute!", attacker.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// User is not Ghost: Decreases the user's Speed, increases the user's Attack &
	/// Defense by 1 stage each.
	/// User is Ghost: User loses 1/2 of max HP, and curses the target.
	/// Cursed Pokémon lose 1/4 of their max HP at the } of each round.
	/// (Curse)
	/// <summary>
	public class PokeBattle_Move_10D : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		failed=false
		if attacker.pbHasType? (:GHOST)
		  if opponent.effects[PBEffects::Curse] ||
			 opponent.pbOwnSide.effects[PBEffects::CraftyShield]
			failed = true
		  else
			pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

			//battle.pbDisplay(_INTL("{1} cut its own HP and laid a curse on {2}!",attacker.pbThis,opponent.pbThis(true)))
			opponent.effects[PBEffects::Curse]=true
			attacker.pbReduceHP((attacker.totalhp/2).floor)
		  }
		else
		  lowerspeed=attacker.pbCanReduceStatStage? (PBStats::SPEED, attacker,false,self)
		  raiseatk=attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self)
		  raisedef=attacker.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,false,self)
		  if !lowerspeed && !raiseatk && !raisedef
			failed = true
		  else
			pbShowAnimation(@id, attacker, null,1, alltargets, showanimation) // Non-Ghost move animation
			if lowerspeed
			  attacker.pbReduceStat(PBStats::SPEED,1, attacker,false, self)

			}
			showanim = true
			if raiseatk
			  attacker.pbIncreaseStat(PBStats::ATTACK,1, attacker,false, self, showanim)

			  showanim=false
			}
			if raisedef
			  attacker.pbIncreaseStat(PBStats::DEFENSE,1, attacker,false, self, showanim)

			  showanim=false
			}
		  }

		}
		if failed
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		for i in opponent.moves
		  if i.id==opponent.lastMoveUsed && i.id>0 && i.pp>0
			pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

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
	/// Target will lose 1/4 of max HP at } of each round, while asleep. (Nightmare)
	/// <summary>
	public class PokeBattle_Move_10F : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if opponent.status!=PBStatuses::SLEEP || opponent.effects[PBEffects::Nightmare] ||
		   (opponent.effects[PBEffects::Substitute]>0 && !ignoresSubstitute? (attacker))
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.effects[PBEffects::Nightmare]=true
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
		public object pbEffectAfterHit(attacker, opponent, turneffects){
		if !attacker.fainted? && turneffects[PBEffects::TotalDamage]>0
		  if attacker.effects[PBEffects::MultiTurn]>0
			mtattack=PBMoves.getName(attacker.effects[PBEffects::MultiTurnAttack])
			mtuser = @battle.battlers[attacker.effects[PBEffects::MultiTurnUser]]

			//battle.pbDisplay(_INTL("{1} got free of {2}'s {3}!",attacker.pbThis,mtuser.pbThis(true),mtattack))
			attacker.effects[PBEffects::MultiTurn]=0
			attacker.effects[PBEffects::MultiTurnAttack]=0
			attacker.effects[PBEffects::MultiTurnUser]=-1
		  }
		  if attacker.effects[PBEffects::LeechSeed]>=0
			attacker.effects[PBEffects::LeechSeed]=-1
			//battle.pbDisplay(_INTL("{1} shed Leech Seed!",attacker.pbThis))   
		  }
		  if attacker.pbOwnSide.effects[PBEffects::StealthRock]
			attacker.pbOwnSide.effects[PBEffects::StealthRock]= false

			//battle.pbDisplay(_INTL("{1} blew away stealth rocks!", attacker.pbThis))     
		  }
		  if attacker.pbOwnSide.effects[PBEffects::Spikes]>0
			attacker.pbOwnSide.effects[PBEffects::Spikes]=0
			//battle.pbDisplay(_INTL("{1} blew away Spikes!",attacker.pbThis))     
		  }
		  if attacker.pbOwnSide.effects[PBEffects::ToxicSpikes]>0
			attacker.pbOwnSide.effects[PBEffects::ToxicSpikes]=0
			//battle.pbDisplay(_INTL("{1} blew away poison spikes!",attacker.pbThis))     
		  }
		  if attacker.pbOwnSide.effects[PBEffects::StickyWeb]
			attacker.pbOwnSide.effects[PBEffects::StickyWeb]= false

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
		public object pbDisplayUseMessage(attacker){
		if (@battle.futuresight) return 0;
		return super(attacker)
	  }

	  public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if opponent.effects[PBEffects::FutureSight]>0
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if @battle.futuresight
		  // Attack hits
		  return super(attacker, opponent, hitnum, alltargets, showanimation)

		}
	/// Attack is launched
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		opponent.effects[PBEffects::FutureSight]=3 
		opponent.effects[PBEffects::FutureSightMove]=@id
		opponent.effects[PBEffects::FutureSightUser]= attacker.pokemonIndex

		opponent.effects[PBEffects::FutureSightUserPos]= attacker.index
		if isConst?(@id, PBMoves,:FUTURESIGHT)

		  //battle.pbDisplay(_INTL("{1} foresaw an attack!",attacker.pbThis))
		else
		  //battle.pbDisplay(_INTL("{1} chose Doom Desire as its destiny!",attacker.pbThis))
		}
		return 0
	  }

	  public object pbShowAnimation(id, attacker, opponent, hitnum= 0, alltargets= null, showanimation= true){
		if @battle.futuresight
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if attacker.effects[PBEffects::Stockpile]>=3
		  //battle.pbDisplay(_INTL("{1} can't stockpile any more!",attacker.pbThis))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		attacker.effects[PBEffects::Stockpile]+=1
		//battle.pbDisplay(_INTL("{1} stockpiled {2}!",attacker.pbThis,
			attacker.effects[PBEffects::Stockpile]))
		showanim=true
		if attacker.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,false,self)
		  attacker.pbIncreaseStat(PBStats::DEFENSE,1,attacker,false,self,showanim)
		  attacker.effects[PBEffects::StockpileDef]+=1
		  showanim=false
		}
		if attacker.pbCanIncreaseStatStage? (PBStats::SPDEF, attacker,false,self)
		  attacker.pbIncreaseStat(PBStats::SPDEF,1,attacker,false,self,showanim)
		  attacker.effects[PBEffects::StockpileSpDef]+=1
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
		public object pbMoveFailed(attacker, opponent){
		return (attacker.effects[PBEffects::Stockpile]==0)
	  }

	  public object pbBaseDamage(basedmg, attacker, opponent){
		return 100* attacker.effects[PBEffects::Stockpile]
	  }

	  public object pbEffectAfterHit(attacker, opponent, turneffects){
		if !attacker.fainted? && turneffects[PBEffects::TotalDamage]>0

		  showanim= true
		  if attacker.effects[PBEffects::StockpileDef]>0
			if attacker.pbCanReduceStatStage?(PBStats::DEFENSE, attacker,false, self)

			  attacker.pbReduceStat(PBStats::DEFENSE, attacker.effects[PBEffects::StockpileDef],
				 attacker,false,self,showanim)
			  showanim=false
			}
		  }
		  if attacker.effects[PBEffects::StockpileSpDef]>0
			if attacker.pbCanReduceStatStage? (PBStats::SPDEF, attacker,false,self)
			  attacker.pbReduceStat(PBStats::SPDEF, attacker.effects[PBEffects::StockpileSpDef],
				 attacker,false,self,showanim)
			  showanim=false
			}
		  }

		  attacker.effects[PBEffects::Stockpile]=0
		  attacker.effects[PBEffects::StockpileDef]=0
		  attacker.effects[PBEffects::StockpileSpDef]=0
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

	  public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		hpgain=0
		case attacker.effects[PBEffects::Stockpile]
		when 0
		  //battle.pbDisplay(_INTL("But it failed to swallow a thing!"))
		  return -1
		when 1
		  hpgain=(attacker.totalhp/4).floor
		when 2
		  hpgain=(attacker.totalhp/2).floor
		when 3
		  hpgain=attacker.totalhp
		}
		if attacker.hp==attacker.totalhp &&
		   attacker.effects[PBEffects::StockpileDef]==0 &&
		   attacker.effects[PBEffects::StockpileSpDef]==0
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)
		if attacker.pbRecoverHP(hpgain,true)>0
		  //battle.pbDisplay(_INTL("{1}'s HP was restored.",attacker.pbThis))
		}
		showanim = true
		if attacker.effects[PBEffects::StockpileDef]>0
		  if attacker.pbCanReduceStatStage? (PBStats::DEFENSE, attacker,false,self)
			attacker.pbReduceStat(PBStats::DEFENSE, attacker.effects[PBEffects::StockpileDef],
			   attacker,false,self,showanim)
			showanim=false
		  }
		}
		if attacker.effects[PBEffects::StockpileSpDef]>0
		  if attacker.pbCanReduceStatStage? (PBStats::SPDEF, attacker,false,self)
			attacker.pbReduceStat(PBStats::SPDEF, attacker.effects[PBEffects::StockpileSpDef],
			   attacker,false,self,showanim)
			showanim=false
		  }
		}

		attacker.effects[PBEffects::Stockpile]=0
		attacker.effects[PBEffects::StockpileDef]=0
		attacker.effects[PBEffects::StockpileSpDef]=0
		//battle.pbDisplay(_INTL("{1}'s stockpiled effect wore off!",attacker.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// Fails if user was hit by a damaging move this round. (Focus Punch)
	/// <summary>
	public class PokeBattle_Move_115 : PokeBattle_Move
	{
		public object pbDisplayUseMessage(attacker){
		if attacker.lastHPLost>0
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
		public object pbMoveFailed(attacker, opponent){
		return true if @battle.choices[opponent.index][0]!=1 // Didn't choose a move
		oppmove=@battle.choices[opponent.index][2]
		return true if opponent.hasMovedThisRound? && oppmove.function!=0xB0 // Me First
		if (!oppmove || oppmove.id<=0 || oppmove.pbIsStatus?) return true;
		return false
	  }
	}



	/// <summary>
	/// This round, user becomes the target of attacks that have single targets.
	/// (Follow Me, Rage Powder)
	/// <summary>
	public class PokeBattle_Move_117 : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if !@battle.doublebattle
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		attacker.effects[PBEffects::FollowMe]=1
		if !attacker.pbPartner.fainted? && attacker.pbPartner.effects[PBEffects::FollowMe]>0
		  attacker.effects[PBEffects::FollowMe]=attacker.pbPartner.effects[PBEffects::FollowMe]+1
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if @battle.field.effects[PBEffects::Gravity]>0
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		@battle.field.effects[PBEffects::Gravity]=5
		for i in 0...4
		  poke=@battle.battlers[i]
		  next if !poke
		  if PBMoveData.new(poke.effects[PBEffects::TwoTurnAttack]).function==0xC9 || // Fly
			 PBMoveData.new(poke.effects[PBEffects::TwoTurnAttack]).function==0xCC || // Bounce
			 PBMoveData.new(poke.effects[PBEffects::TwoTurnAttack]).function==0xCE    // Sky Drop
			poke.effects[PBEffects::TwoTurnAttack]=0
		  }
		  if poke.effects[PBEffects::SkyDrop]
			poke.effects[PBEffects::SkyDrop]= false

		  }
		  if poke.effects[PBEffects::MagnetRise]>0

			poke.effects[PBEffects::MagnetRise]= 0

		  }
		  if poke.effects[PBEffects::Telekinesis]>0

			poke.effects[PBEffects::Telekinesis]= 0

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

	  public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if attacker.effects[PBEffects::Ingrain] ||
		   attacker.effects[PBEffects::SmackDown] ||
		   attacker.effects[PBEffects::MagnetRise]>0
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		attacker.effects[PBEffects::MagnetRise]=5
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

	  public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if opponent.effects[PBEffects::Ingrain] ||
		   opponent.effects[PBEffects::SmackDown] ||
		   opponent.effects[PBEffects::Telekinesis]>0
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.effects[PBEffects::Telekinesis]=3
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
		public object pbBaseDamage(basedmg, attacker, opponent){
		if PBMoveData.new(opponent.effects[PBEffects::TwoTurnAttack]).function==0xC9 || // Fly
		   PBMoveData.new(opponent.effects[PBEffects::TwoTurnAttack]).function==0xCC || // Bounce
		   PBMoveData.new(opponent.effects[PBEffects::TwoTurnAttack]).function==0xCE || // Sky Drop
		   opponent.effects[PBEffects::SkyDrop]
		  return basedmg*2
		}
		return basedmg
	  }

	  public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)

		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if opponent.damagestate.calcdamage>0 && !opponent.damagestate.substitute &&
		   !opponent.effects[PBEffects::Roost]
		  opponent.effects[PBEffects::SmackDown]= true

		  showmsg= (opponent.pbHasType ? (: FLYING) ||
				   opponent.hasWorkingAbility(:LEVITATE))
		  if PBMoveData.new(opponent.effects[PBEffects::TwoTurnAttack]).function==0xC9 || // Fly
			 PBMoveData.new(opponent.effects[PBEffects::TwoTurnAttack]).function==0xCC    // Bounce
			opponent.effects[PBEffects::TwoTurnAttack]=0; showmsg=true
		  }
		  if opponent.effects[PBEffects::MagnetRise]>0
			opponent.effects[PBEffects::MagnetRise]=0; showmsg=true
		  }
		  if opponent.effects[PBEffects::Telekinesis]>0
			opponent.effects[PBEffects::Telekinesis]=0; showmsg=true
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
		public object pbMoveFailed(attacker, opponent){
		return true if @battle.choices[opponent.index][0]!=1 // Didn't choose a move
		if (opponent.effects[PBEffects::MoveNext]) return true;
		oppmove=@battle.choices[opponent.index][2]
		if (!oppmove || oppmove.id<=0) return true;
		if (opponent.hasMovedThisRound?) return true;
		return false
	  }

	  public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)

		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.effects[PBEffects::MoveNext]=true
		opponent.effects[PBEffects::Quash]=false
		//battle.pbDisplay(_INTL("{1} took the kind offer!",opponent.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// Target moves last this round, ignoring priority/speed. (Quash)
	/// <summary>
	public class PokeBattle_Move_11E : PokeBattle_Move
	{
		public object pbMoveFailed(attacker, opponent){
		return true if @battle.choices[opponent.index][0]!=1 // Didn't choose a move
		if (opponent.effects[PBEffects::Quash]) return true;
		oppmove=@battle.choices[opponent.index][2]
		if (!oppmove || oppmove.id<=0) return true;
		if (opponent.hasMovedThisRound?) return true;
		return false
	  }

	  public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)

		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.effects[PBEffects::Quash]=true
		opponent.effects[PBEffects::MoveNext]=false
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if @battle.field.effects[PBEffects::TrickRoom]>0
		  @battle.field.effects[PBEffects::TrickRoom]=0
		  //battle.pbDisplay(_INTL("{1} reverted the dimensions!",attacker.pbThis))
		else
		  pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		  @battle.field.effects[PBEffects::TrickRoom]=5
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if !@battle.doublebattle ||
		   !attacker.pbPartner || attacker.pbPartner.fainted?
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		a=@battle.battlers[attacker.index]
		b = @battle.battlers[attacker.pbPartner.index]

		temp=a; a=b; b=temp
	/// Swap effects that point at the position rather than the Pokémon
	/// NOT PerishSongUser (no need to swap), Attract, MultiTurnUser
		effectstoswap =[PBEffects::BideTarget,
					   PBEffects::CounterTarget,
					   PBEffects::LeechSeed,
					   PBEffects::LockOnPos,
					   PBEffects::MeanLook,
					   PBEffects::MirrorCoatTarget]
		for i in effectstoswap
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
	/// Handled in superclass public object pbCalcDamage, do not edit!
	}



	/// <summary>
	/// Target's Defense is used instead of its Special Defense for this move's
	/// calculations. (Psyshock, Psystrike, Secret Sword)
	/// <summary>
	public class PokeBattle_Move_122 : PokeBattle_Move
	/// Handled in superclass public object pbCalcDamage, do not edit!
	}



	/// <summary>
	/// Only damages Pokémon that share a type with the user. (Synchronoise)
	/// <summary>
	public class PokeBattle_Move_123 : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if !opponent.pbHasType? (attacker.type1) &&
		   !opponent.pbHasType? (attacker.type2) &&
		   !opponent.pbHasType? (attacker.effects[PBEffects::Type3])
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if @battle.field.effects[PBEffects::WonderRoom]>0
		  @battle.field.effects[PBEffects::WonderRoom]=0
		  //battle.pbDisplay(_INTL("Wonder Room wore off, and the Defense and Sp. Def stats returned to normal!"))
		else
		  pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		  @battle.field.effects[PBEffects::WonderRoom]=5
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
		public object pbMoveFailed(attacker, opponent){
		counter = 0; nummoves=0
		for move in attacker.moves
		  next if move.id<=0
		  counter+=1 if move.id!=@id && !attacker.movesUsed.include? (move.id)
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if !@battle.doublebattle ||
		   !attacker.pbPartner || attacker.pbPartner.fainted?
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)
		return 0
	  }
	}



	/// <summary>
	/// Does absolutely nothing. Shows a special message. (Celebrate)
	/// <summary>
	public class PokeBattle_Move_134 : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		//battle.pbDisplay(_INTL("Congratulations, {1}!",@battle.pbGetOwner(attacker.index).name))
		return 0
	  }
	}



	/// <summary>
	/// Freezes the target. (Freeze-Dry)
	/// (Superclass's pbTypeModifier): Effectiveness against Water-type is 2x.
	/// <summary>
	public class PokeBattle_Move_135 : PokeBattle_Move
	{
		public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		if opponent.pbCanFreeze? (attacker,false,self)
		  opponent.pbFreeze
		}
	  }
	}



	/// <summary>
	/// Increases the user's Defense by 1 stage for each target hit. (Diamond Storm)
	/// <summary>
	public class PokeBattle_Move_136 : PokeBattle_Move_01D
	/// No difference to function code 01D. It may need to be separate in future.
	}



	/// <summary>
	/// Increases the user's and its ally's Defense and Special Defense by 1 stage
	/// each, if they have Plus or Minus. (Magnetic Flux)
	/// <summary>
	public class PokeBattle_Move_137 : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		didsomething=false
		for i in [attacker, attacker.pbPartner]
	next if !i || i.fainted?
	next if !i.hasWorkingAbility(:PLUS) && !i.hasWorkingAbility(:MINUS)
		  next if !i.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,false,self) &&
				  !i.pbCanIncreaseStatStage? (PBStats::SPDEF, attacker,false,self)
		  pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation) if !didsomething
			   didsomething = true

		  showanim=true
		  if i.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,false,self)
			i.pbIncreaseStat(PBStats::DEFENSE,1,attacker,false,self,showanim)
			showanim=false
		  }
		  if i.pbCanIncreaseStatStage? (PBStats::SPDEF, attacker,false,self)
			i.pbIncreaseStat(PBStats::SPDEF,1,attacker,false,self,showanim)
			showanim=false
		  }
		}
		if !didsomething
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if !@battle.doublebattle || !opponent ||
		   !opponent.pbCanIncreaseStatStage? (PBStats::SPDEF, attacker,false,self)
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=attacker.pbIncreaseStat(PBStats::SPDEF,1,attacker,false,self)
		return ret? 0 : -1
	  }
	}



	/// <summary>
	/// Decreases the target's Attack by 1 stage. Always hits. (Play Nice)
	/// <summary>
	public class PokeBattle_Move_139 : PokeBattle_Move
	{
		public object pbAccuracyCheck(attacker, opponent){
		return true
	  }

	  public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if (!opponent.pbCanReduceStatStage? (PBStats::ATTACK, attacker,true,self)) return -1;
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=opponent.pbReduceStat(PBStats::ATTACK,1,attacker,false,self)
		return ret? 0 : -1
	  }
	}



	/// <summary>
	/// Decreases the target's Attack and Special Attack by 1 stage each. (Noble Roar)
	/// <summary>
	public class PokeBattle_Move_13A : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true)
		// Replicates public object pbCanReduceStatStage? so that certain messages aren't shown
		// multiple times
		if opponent.effects[PBEffects::Substitute]>0 && !ignoresSubstitute? (attacker)
		   //battle.pbDisplay(_INTL("{1}'s attack missed!", attacker.pbThis))
		  return -1
		}
		if opponent.pbTooLow? (PBStats::ATTACK) &&
		   opponent.pbTooLow? (PBStats::SPATK)
		   //battle.pbDisplay(_INTL("{1}'s stats won't go any lower!", opponent.pbThis))
		  return -1
		}
		if opponent.pbOwnSide.effects[PBEffects::Mist]>0
		  //battle.pbDisplay(_INTL("{1} is protected by Mist!",opponent.pbThis))
		  return -1
		}
		if !attacker.hasMoldBreaker
		  if opponent.hasWorkingAbility(:CLEARBODY) ||
			 opponent.hasWorkingAbility(:WHITESMOKE)
			//battle.pbDisplay(_INTL("{1}'s {2} prevents stat loss!",opponent.pbThis,
			   PBAbilities.getName(opponent.ability)))
			return -1
		  }
		}

		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=-1; showanim=true
		if !attacker.hasMoldBreaker && opponent.hasWorkingAbility(:HYPERCUTTER)
		  abilityname=PBAbilities.getName(opponent.ability)
		  //battle.pbDisplay(_INTL("{1}'s {2} prevents Attack loss!", opponent.pbThis, abilityname))
		elsif opponent.pbReduceStat(PBStats::ATTACK,1, attacker,false, self, showanim)

		  ret=0; showanim=false
		}
		if opponent.pbReduceStat(PBStats::SPATK,1,attacker,false,self,showanim)
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
		public object pbMoveFailed(attacker, opponent){
		if (!isConst? (attacker.species, PBSpecies,:HOOPA)) return true;
		if (attacker.form!=1) return true;
		return false
	  }

	  public object pbAccuracyCheck(attacker, opponent){
		return true
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		if opponent.pbCanReduceStatStage? (PBStats::DEFENSE, attacker,false,self)
		  opponent.pbReduceStat(PBStats::DEFENSE,1,attacker,false,self)
		}
	  }
	}



	/// <summary>
	/// Decreases the target's Special Attack by 1 stage. Always hits. (Confide)
	/// <summary>
	public class PokeBattle_Move_13C : PokeBattle_Move
	{
		public object pbAccuracyCheck(attacker, opponent){
		return true
	  }

	  public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if (!opponent.pbCanReduceStatStage? (PBStats::SPATK, attacker,true,self)) return -1;
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=opponent.pbReduceStat(PBStats::SPATK,1,attacker,false,self)
		return ret? 0 : -1
	  }
	}



	/// <summary>
	/// Decreases the target's Special Attack by 2 stages. (Eerie Impulse)
	/// <summary>
	public class PokeBattle_Move_13D : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if (pbIsDamaging?) return super(attacker, opponent, hitnum, alltargets, showanimation);
		if (pbTypeImmunityByAbility(pbType(@type, attacker, opponent), attacker, opponent)) return -1;
		if (!opponent.pbCanReduceStatStage? (PBStats::SPATK, attacker,true,self)) return -1;
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		ret=opponent.pbReduceStat(PBStats::SPATK,2,attacker,false,self)
		return ret? 0 : -1
	  }

	  public object pbAdditionalEffect(Pokemon attacker, Pokemon opponent){
		if (opponent.damagestate.substitute) return;
		if opponent.pbCanReduceStatStage? (PBStats::SPATK, attacker,false,self)
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		didsomething=false
		for i in [attacker, attacker.pbPartner, attacker.pbOpposing1, attacker.pbOpposing2]
	next if !i || i.fainted?
	next if !i.pbHasType? (:GRASS)
		  next if i.isAirborne? (attacker.hasMoldBreaker)
		   next if !i.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self) &&
				  !i.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,false,self)
		  pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation) if !didsomething
			   didsomething = true

		  showanim=true
		  if i.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self)
			i.pbIncreaseStat(PBStats::ATTACK,1,attacker,false,self,showanim)
			showanim=false
		  }
		  if i.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,false,self)
			i.pbIncreaseStat(PBStats::SPATK,1,attacker,false,self,showanim)
			showanim=false
		  }
		}
		if !didsomething
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		didsomething=false
		for i in [attacker, attacker.pbPartner, attacker.pbOpposing1, attacker.pbOpposing2]
	next if !i || i.fainted?
	next if !i.pbHasType? (:GRASS)
		  next if !i.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self)
		  pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation) if !didsomething
			   didsomething = true

		  showanim=true
		  if i.pbCanIncreaseStatStage? (PBStats::DEFENSE, attacker,false,self)
			i.pbIncreaseStat(PBStats::DEFENSE,1,attacker,false,self,showanim)
			showanim=false
		  }
		}
		if !didsomething
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		didsomething=false
		for i in [attacker.pbOpposing1, attacker.pbOpposing2]
	next if !i || i.fainted?
	next if !i.status==PBStatuses::POISON
	next if !i.pbCanReduceStatStage? (PBStats::ATTACK, attacker,false,self) &&
				  !i.pbCanReduceStatStage? (PBStats::SPATK, attacker,false,self) &&
				  !i.pbCanReduceStatStage? (PBStats::SPEED, attacker,false,self)
		  pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation) if !didsomething
			   didsomething = true

		  showanim=true
		  if i.pbCanReduceStatStage? (PBStats::ATTACK, attacker,false,self)
			i.pbReduceStat(PBStats::ATTACK,1,attacker,false,self,showanim)
			showanim=false
		  }
		  if i.pbCanReduceStatStage? (PBStats::SPATK, attacker,false,self)
			i.pbReduceStat(PBStats::SPATK,1,attacker,false,self,showanim)
			showanim=false
		  }
		  if i.pbCanReduceStatStage? (PBStats::SPEED, attacker,false,self)
			i.pbReduceStat(PBStats::SPEED,1,attacker,false,self,showanim)
			showanim=false
		  }
		}
		if !didsomething
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		nonzero=false
		for i in [PBStats::ATTACK, PBStats::DEFENSE, PBStats::SPEED,
				  PBStats::SPATK, PBStats::SPDEF, PBStats::ACCURACY, PBStats::EVASION]
		  if opponent.stages[i]!=0
			nonzero=true; break
		  }
		}
		if !nonzero
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)
		for i in [PBStats::ATTACK, PBStats::DEFENSE, PBStats::SPEED,
				  PBStats::SPATK, PBStats::SPDEF, PBStats::ACCURACY, PBStats::EVASION]
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if (opponent.effects[PBEffects::Substitute]>0 && !ignoresSubstitute? (attacker)) ||
		   !hasConst? (PBTypes,:GHOST) || opponent.pbHasType? (:GHOST) ||
		   isConst? (opponent.ability, PBAbilities,:MULTITYPE)
		  //battle.pbDisplay(_INTL("But it failed!"))  
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.effects[PBEffects::Type3]=getConst(PBTypes,:GHOST)

		typename=PBTypes.getName(getConst(PBTypes,:GHOST))
		//battle.pbDisplay(_INTL("{1} transformed into the {2} type!",opponent.pbThis,typename))
		return 0
	  }
	}



	/// <summary>
	/// Gives target the Grass type. (Forest's Curse)
	/// <summary>
	public class PokeBattle_Move_143 : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if opponent.effects[PBEffects::Substitute]>0 && !ignoresSubstitute? (attacker)
		   //battle.pbDisplay(_INTL("But it failed!"))  
		  return -1
		}
		if (pbTypeImmunityByAbility(pbType(@type, attacker, opponent), attacker, opponent)) return -1;
		if opponent.effects[PBEffects::LeechSeed]>=0
		  //battle.pbDisplay(_INTL("{1} evaded the attack!",opponent.pbThis))
		  return -1
		}
		if !hasConst? (PBTypes,:GRASS) || opponent.pbHasType? (:GRASS) ||
		   isConst? (opponent.ability, PBAbilities,:MULTITYPE)
		  //battle.pbDisplay(_INTL("But it failed!"))  
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.effects[PBEffects::Type3]=getConst(PBTypes,:GRASS)

		typename=PBTypes.getName(getConst(PBTypes,:GRASS))
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
		public object pbModifyDamage(damagemult, attacker, opponent){
		type = getConst(PBTypes,:FLYING) || -1
		if type>=0
		  mult=PBTypes.getCombinedEffectiveness(type,
			 opponent.type1, opponent.type2, opponent.effects[PBEffects::Type3])
		  return ((damagemult* mult)/8).round
		 }
		return damagemult
	  }

	  public object tramplesMinimize? (param=1)
		return true if param==1 && USENEWBATTLEMECHANICS // Perfect accuracy
		return true if param==2 // Double damage
		return false
	  }
	}



	/// <summary>
	/// Target's moves become Electric-type for the rest of the round. (Electrify)
	/// <summary>
	public class PokeBattle_Move_145 : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if (pbTypeImmunityByAbility(pbType(@type, attacker, opponent), attacker, opponent)) return -1;
		if opponent.effects[PBEffects::Electrify]
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		if @battle.choices[opponent.index][0]!=1 || // Didn't choose a move
		   !@battle.choices[opponent.index][2] ||
		   @battle.choices[opponent.index][2].id<=0 ||
		   opponent.hasMovedThisRound?
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		opponent.effects[PBEffects::Electrify]=true
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		unmoved=false
		for poke in @battle.battlers
		  next if poke.index==attacker.index
		  if @battle.choices[poke.index][0]==1 && // Chose a move
			 !poke.hasMovedThisRound?
			unmoved = true; break
		  }
		}
		if !unmoved || @battle.field.effects[PBEffects::IonDeluge]
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		@battle.field.effects[PBEffects::IonDeluge]=true
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
		public object pbAccuracyCheck(attacker, opponent){
		return true
	  }
	}


	/// <summary>
	/// Powders the foe. This round, if it uses a Fire move, it loses 1/4 of its max
	/// HP instead. (Powder)
	/// <summary>
	public class PokeBattle_Move_148 : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if opponent.effects[PBEffects::Powder]
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		opponent.effects[PBEffects::Powder]= true

		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)
		//battle.pbDisplay(_INTL("{1} is covered in powder!", attacker.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// This round, the user's side is unaffected by damaging moves. (Mat Block)
	/// <summary>
	public class PokeBattle_Move_149 : PokeBattle_Move
	{
		public object pbMoveFailed(attacker, opponent){
		return (attacker.turncount>1)
	  }

	  public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)

		attacker.pbOwnSide.effects[PBEffects::MatBlock]=true
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		//battle.pbDisplay(_INTL("{1} intends to flip up a mat and block incoming attacks!",attacker.pbThis))
		return 0
	  }
	}



	/// <summary>
	/// User's side is protected against status moves this round. (Crafty Shield)
	/// <summary>
	public class PokeBattle_Move_14A : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if attacker.pbOwnSide.effects[PBEffects::CraftyShield]
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		unmoved = false
		for poke in @battle.battlers
		  next if poke.index==attacker.index
		  if @battle.choices[poke.index][0]==1 && // Chose a move
			 !poke.hasMovedThisRound?
			unmoved = true; break
		  }
		}
		if !unmoved
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		attacker.pbOwnSide.effects[PBEffects::CraftyShield]=true
		if !@battle.pbIsOpposing? (attacker.index)
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if attacker.effects[PBEffects::KingsShield]
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
		if !ratesharers.include? (PBMoveData.new(attacker.lastMoveUsed).function)
		  attacker.effects[PBEffects::ProtectRate]=1
		}
		unmoved = false
		for poke in @battle.battlers
		  next if poke.index==attacker.index
		  if @battle.choices[poke.index][0]==1 && // Chose a move
			 !poke.hasMovedThisRound?
			unmoved = true; break
		  }
		}
		if !unmoved ||
		   (!USENEWBATTLEMECHANICS &&
		   @battle.pbRandom(65536)>=(65536/attacker.effects[PBEffects::ProtectRate]).floor)
		  attacker.effects[PBEffects::ProtectRate]=1
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		attacker.effects[PBEffects::KingsShield]=true
		attacker.effects[PBEffects::ProtectRate]*=2
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if attacker.effects[PBEffects::SpikyShield]
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
		if !ratesharers.include? (PBMoveData.new(attacker.lastMoveUsed).function)
		  attacker.effects[PBEffects::ProtectRate]=1
		}
		unmoved = false
		for poke in @battle.battlers
		  next if poke.index==attacker.index
		  if @battle.choices[poke.index][0]==1 && // Chose a move
			 !poke.hasMovedThisRound?
			unmoved = true; break
		  }
		}
		if !unmoved ||
		   @battle.pbRandom(65536)>=(65536/attacker.effects[PBEffects::ProtectRate]).floor
		  attacker.effects[PBEffects::ProtectRate]= 1

		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		attacker.effects[PBEffects::SpikyShield]=true
		attacker.effects[PBEffects::ProtectRate]*=2
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
		public object pbTwoTurnAttack(attacker)
		@immediate = false
		if !@immediate && attacker.hasWorkingItem(:POWERHERB)
		  @immediate=true
		}
		if (@immediate) return false;
		return attacker.effects[PBEffects::TwoTurnAttack]==0
	  }

	  public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if @immediate || attacker.effects[PBEffects::TwoTurnAttack]>0
		  pbShowAnimation(@id, attacker, opponent,1, alltargets, showanimation) // Charging anim
		  //battle.pbDisplay(_INTL("{1} vanished instantly!",attacker.pbThis))
		}
		if @immediate
		  @battle.pbCommonAnimation("UseItem", attacker, null)

		  //battle.pbDisplay(_INTL("{1} became fully charged due to its Power Herb!",attacker.pbThis))
		  attacker.pbConsumeItem
		}
		if (attacker.effects[PBEffects::TwoTurnAttack]>0) return 0;
		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if ret>0
		  opponent.effects[PBEffects::ProtectNegation]=true
		  opponent.pbOwnSide.effects[PBEffects::CraftyShield]=false
		}
		return ret
	  }

	  public object tramplesMinimize? (param=1)
		return true if param==1 && USENEWBATTLEMECHANICS // Perfect accuracy
		return true if param==2 // Double damage
		return false
	  }
	}



	/// <summary>
	/// Two turn attack. Skips first turn, increases the user's Special Attack,
	/// Special Defense and Speed by 2 stages each second turn. (Geomancy)
	/// <summary>
	public class PokeBattle_Move_14E : PokeBattle_Move
	{
		public object pbTwoTurnAttack(attacker)
		@immediate = false
		if !@immediate && attacker.hasWorkingItem(:POWERHERB)
		  @immediate=true
		}
		if (@immediate) return false;
		return attacker.effects[PBEffects::TwoTurnAttack]==0
	  }

	  public object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum= 0, int? alltargets= null, bool showanimation= true)
		if @immediate || attacker.effects[PBEffects::TwoTurnAttack]>0
		  pbShowAnimation(@id, attacker, opponent,1, alltargets, showanimation) // Charging anim
		  //battle.pbDisplay(_INTL("{1} is absorbing power!",attacker.pbThis))
		}
		if @immediate
		  @battle.pbCommonAnimation("UseItem", attacker, null)

		  //battle.pbDisplay(_INTL("{1} became fully charged due to its Power Herb!",attacker.pbThis))
		  attacker.pbConsumeItem
		}
		if (attacker.effects[PBEffects::TwoTurnAttack]>0) return 0;
		if !attacker.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,false,self) &&
		   !attacker.pbCanIncreaseStatStage? (PBStats::SPDEF, attacker,false,self) &&
		   !attacker.pbCanIncreaseStatStage? (PBStats::SPEED, attacker,false,self)
		  //battle.pbDisplay(_INTL("{1}'s stats won't go any higher!",attacker.pbThis))
		  return -1
		}
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)

		showanim=true
		if attacker.pbCanIncreaseStatStage? (PBStats::SPATK, attacker,false,self)
		  attacker.pbIncreaseStat(PBStats::SPATK,2,attacker,false,self,showanim)
		  showanim=false
		}
		if attacker.pbCanIncreaseStatStage? (PBStats::SPDEF, attacker,false,self)
		  attacker.pbIncreaseStat(PBStats::SPDEF,2,attacker,false,self,showanim)
		  showanim=false
		}
		if attacker.pbCanIncreaseStatStage? (PBStats::SPEED, attacker,false,self)
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

	  public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if opponent.damagestate.calcdamage>0
		  hpgain=(opponent.damagestate.hplost*3/4).round
		  if opponent.hasWorkingAbility(:LIQUIDOOZE)
			attacker.pbReduceHP(hpgain,true)
			//battle.pbDisplay(_INTL("{1} sucked up the liquid ooze!",attacker.pbThis))
		  elsif attacker.effects[PBEffects::HealBlock]==0

			hpgain= (hpgain * 1.3).floor if attacker.hasWorkingItem(:BIGROOT)

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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		ret=super(attacker, opponent, hitnum, alltargets, showanimation)
		if opponent.damagestate.calcdamage>0 && opponent.fainted?
		  if attacker.pbCanIncreaseStatStage? (PBStats::ATTACK, attacker,false,self)
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		ret=-1
		pbShowAnimation(@id, attacker, opponent, hitnum, alltargets, showanimation)
		if !self.isSoundBased? ||
		   attacker.hasMoldBreaker || !opponent.hasWorkingAbility(:SOUNDPROOF)
		  showanim=true
		  if opponent.pbReduceStat(PBStats::ATTACK,1,attacker,false,self,showanim)
			showanim=false; ret=0
		  }
		  if opponent.pbReduceStat(PBStats::SPATK,1,attacker,false,self,showanim)
			showanim=false; ret=0
		  }
		}
		if !attacker.fainted? &&
		   @battle.pbCanChooseNonActive? (attacker.index) &&
		   !@battle.pbAllFainted? (@battle.pbParty(opponent.index))
		  attacker.effects[PBEffects::Uturn]=true; ret=0
		}
		return ret
	  }
	}



	/// <summary>
	/// No Pokémon can switch out or flee until the } of the next round, as long as
	/// the user remains active. (Fairy Lock)
	/// <summary>
	public class PokeBattle_Move_152 : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if @battle.field.effects[PBEffects::FairyLock]>0
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		@battle.field.effects[PBEffects::FairyLock]=2
		//battle.pbDisplay(_INTL("No one will be able to run away during the next turn!"))
		return 0
	  }
	}



	/// <summary>
	/// Entry hazard. Lays stealth rocks on the opposing side. (Sticky Web)
	/// <summary>
	public class PokeBattle_Move_153 : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if attacker.pbOpposingSide.effects[PBEffects::StickyWeb]
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		attacker.pbOpposingSide.effects[PBEffects::StickyWeb]=true
		if !@battle.pbIsOpposing? (attacker.index)
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if @battle.field.effects[PBEffects::ElectricTerrain]>0
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		@battle.field.effects[PBEffects::GrassyTerrain]=0
		@battle.field.effects[PBEffects::MistyTerrain]=0
		@battle.field.effects[PBEffects::ElectricTerrain]=5
		//battle.pbDisplay(_INTL("An electric current runs across the battlefield!"))
		return 0
	  }
	}



	/// <summary>
	/// For 5 rounds, creates a grassy terrain which boosts Grass-type moves and heals
	/// Pokémon at the } of each round. Affects non-airborne Pokémon only.
	/// (Grassy Terrain)
	/// <summary>
	public class PokeBattle_Move_155 : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if @battle.field.effects[PBEffects::GrassyTerrain]>0
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		@battle.field.effects[PBEffects::ElectricTerrain]=0
		@battle.field.effects[PBEffects::MistyTerrain]=0
		@battle.field.effects[PBEffects::GrassyTerrain]=5
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
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if @battle.field.effects[PBEffects::MistyTerrain]>0
		  //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		@battle.field.effects[PBEffects::ElectricTerrain]=0
		@battle.field.effects[PBEffects::GrassyTerrain]=0
		@battle.field.effects[PBEffects::MistyTerrain]=5
		//battle.pbDisplay(_INTL("Mist swirled about the battlefield!"))
		return 0
	  }
	}



	/// <summary>
	/// Doubles the prize money the player gets after winning the battle. (Happy Hour)
	/// </summary>
	public class PokeBattle_Move_157 : PokeBattle_Move
	{
		public object pbEffect(attacker, opponent, hitnum=0,alltargets=null,showanimation=true){
		if @battle.pbIsOpposing? (attacker.index) || @battle.doublemoney
		   //battle.pbDisplay(_INTL("But it failed!"))
		  return -1
		}
		pbShowAnimation(@id, attacker, null, hitnum, alltargets, showanimation)

		@battle.doublemoney=true
		//battle.pbDisplay(_INTL("Everyone is caught up in the happy atmosphere!"))
		return 0
	  }
	}



	/// <summary>
	/// Fails unless user has consumed a berry at some point. (Belch)
	/// </summary>
	public class PokeBattle_Move_158 : PokeBattle_Move
	{
		public object pbMoveFailed(attacker, opponent){
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
	//public Battle battle { get; set; }
	public bool IsDamaging()
	{
		return true;
	}

	public virtual object pbEffect(Pokemon attacker, Pokemon opponent, int hitnum = 0, int? alltargets = null, bool showanimation = true)
	{

	}
}

public interface IPokeBattle_Move
{

}
