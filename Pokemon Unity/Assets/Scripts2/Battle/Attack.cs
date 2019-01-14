using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Pokemon;
using PokemonUnity.Item;
using PokemonUnity.Move;
using System.Collections;

/// <summary>
/// During battle, the moves used are modified by these classes before calculations are applied
/// </summary>
public class Function
{
	#region Battle Class Functions
	/// <summary>
	/// Superclass that handles moves using a non-existent function code.
	/// Damaging moves just do damage with no additional effect.
	/// Non-damaging moves always fail.
	/// <summary>
	public class PokeBattle_UnimplementedMove : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
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
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
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

		public bool IsPhysical() { return true; }
		public bool IsSpecial() { return false; }

		public override int pbCalcDamage(Battle.Battler attacker, Battle.Battler opponent)
		{
			return base.pbCalcDamage(attacker, opponent,
	   			thismove.NOCRITICAL | thismove.SELFCONFUSE | thismove.NOTYPE | thismove.NOWEIGHTING);
		}

		public override object pbEffectMessages(Battle.Battler attacker, Battle.Battler opponent, bool ignoretype = false)
		{
			return base.pbEffectMessages(attacker, opponent, true);
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

		public bool IsPhysical() { return true; }
		public bool IsSpecial() { return false; }

		public void pbEffectAfterHit(Battle.Battler attacker, Battle.Battler opponent, Effects.Move turneffects)
		{
			if (!attacker.isFainted() && turneffects.TotalDamage > 0)
			{
				attacker.ReduceHP((int)Math.Round(attacker.TotalHP / 4.0f));
				//battle.pbDisplay(_INTL("{1} is damaged by recoil!", attacker.pbThis));
			}
		}

		public override int pbCalcDamage(Battle.Battler attacker, Battle.Battler opponent)
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
		public bool unusableInGravity()
		{
			return true;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);
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
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (pbIsDamaging()) return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (pbTypeImmunityByAbility(pbType(this.type, attacker, opponent), attacker, opponent)) return -1;
			if (opponent.pbCanSleep(attacker, true, this))
			{
				pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

				opponent.pbSleep();
				return 0;
			}
			return -1;
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			if (opponent.pbCanSleep(attacker, false, this))
			{
				opponent.pbSleep();
			}
		}

		public void pbEffectAfterHit(Battle.Battler attacker, Battle.Battler opponent, Effects.Move turneffects)
		{
			if (id == Moves.RELIC_SONG)
			{
				if (attacker.Species == Pokemons.MELOETTA &&
					!attacker.effects.Transform &&
					!(attacker.hasWorkingAbility(Abilities.SHEER_FORCE) && this.AddlEffect > 0) &&
					!attacker.isFainted())
				{
					attacker.Form = (attacker.Form + 1) % 2;
					attacker.Update(true);
					this.battle.scene.pbChangePokemon(attacker, attacker.Species); //ToDo: Feed animation thru Battleor make a Scene class
					//battle.pbDisplay(_INTL("{1} transformed!", attacker.pbThis))
					//PBDebug.log("[Form changed] #{attacker.pbThis} changed to form #{attacker.Form}")
				}
			}
		}
	}

	/// <summary>
	/// Makes the target drowsy; it will fall asleep at the end of the next turn. (Yawn)
	/// <summary>
	public class PokeBattle_Move_004 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (!opponent.pbCanSleep(attacker, true, this)) return -1;
			if (opponent.effects.Yawn > 0)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			opponent.effects.Yawn = 2;
			//battle.pbDisplay(_INTL("{1} made {2} drowsy!",attacker.pbThis,opponent.pbThis(true)))
			return 0;
		}
	}

	/// <summary>
	/// Poisons the target.
	/// <summary>
	public class PokeBattle_Move_005 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (pbIsDamaging()) return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (!opponent.pbCanPoison(attacker, true, this)) return -1;
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			opponent.pbPoison(attacker);
			return 0;
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			if (opponent.pbCanPoison(attacker, false, this))
			{
				opponent.pbPoison(attacker);
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
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (pbIsDamaging()) return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (!opponent.pbCanPoison(attacker, true, this)) return -1;
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			opponent.pbPoison(attacker, null, true);
			return 0;
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			if (opponent.pbCanPoison(attacker, false, this))
			{
				opponent.pbPoison(attacker, null, true);
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
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (pbIsDamaging())
			{
				object ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
				if (opponent.damagestate.CalcDamage > 0 && id == Moves.BOLT_STRIKE)
				{
					this.battle.field.FusionFlare = true;
				}
				return ret;
			}
			else
			{
				if (this.id == Moves.THUNDER_WAVE)
				{
					if (pbTypeModifier(type, attacker, opponent) == 0)
					{
						//battle.pbDisplay(_INTL("It doesn't affect {1}...",opponent.pbThis(true)))
						return -1;
					}
				}
				if (pbTypeImmunityByAbility(pbType(this.type, attacker, opponent), attacker, opponent)) return -1;
				if (!opponent.pbCanParalyze(attacker, true, this)) return -1;
				pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

				opponent.pbParalyze(attacker);
				return 0;
			}
			//return -1
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			if (opponent.pbCanParalyze(attacker, false, this))
			{
				opponent.pbParalyze(attacker);
			}
		}
	}

	/// <summary>
	/// Paralyzes the target. Accuracy perfect in rain, 50% in sunshine. (Thunder)
	/// (Handled in Battler's pbSuccessCheck): Hits some semi-invulnerable targets.
	/// <summary>
	public class PokeBattle_Move_008 : PokeBattle_Move
	{
		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			if (opponent.pbCanParalyze(attacker, false, this))
			{
				opponent.pbParalyze(attacker);
			}
		}

		public object pbModifyBaseAccuracy(byte baseaccuracy, Battle.Battler attacker, Battle.Battler opponent)
		{
			switch (this.battle.Weather)
			{
				case Weather.RAINDANCE:
				case Weather.HEAVYRAIN:
					return 0;
				case Weather.SUNNYDAY:
				case Weather.HARSHSUN:
					return 50;
				default:
					return baseaccuracy;
			}
		}
	}

	/// <summary>
	/// Paralyzes the target. May cause the target to flinch. (Thunder Fang)
	/// <summary>
	public class PokeBattle_Move_009 : PokeBattle_Move
	{
		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			if (this.battle.pbRandom(10) == 0)
			{
				if (opponent.pbCanParalyze(attacker, false, this))
				{
					opponent.pbParalyze(attacker);
				}
			}

			if (this.battle.pbRandom(10) == 0)
			{
				opponent.pbFlinch(attacker);
			}
		}
	}

	/// <summary>
	/// Burns the target.
	/// Blue Flare: Powers up the next Fusion Bolt used this round.
	/// <summary>
	public class PokeBattle_Move_00A : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (pbIsDamaging())
			{
				object ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
				if (opponent.damagestate.CalcDamage > 0 && id == Moves.BLUE_FLARE)
				{
					this.battle.field.FusionBolt = true;
				}
				return ret;
			}
			else
			{
				if (pbTypeImmunityByAbility(pbType(this.type, attacker, opponent), attacker, opponent)) return -1;
				if (!opponent.pbCanBurn(attacker, true, this)) return -1;
				pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

				opponent.pbBurn(attacker);
				return 0;
			}
			//return -1;
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			if (opponent.pbCanBurn(attacker, false, this))
			{
				opponent.pbBurn(attacker);
			}
		}
	}

	/// <summary>
	/// Burns the target. May cause the target to flinch. (Fire Fang)
	/// <summary>
	public class PokeBattle_Move_00B : PokeBattle_Move
	{
		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			if (this.battle.pbRandom(10) == 0)
			{
				if (opponent.pbCanBurn(attacker, false, this))
				{
					opponent.pbBurn(attacker);
				}

			}
			if (this.battle.pbRandom(10) == 0)
			{
				opponent.pbFlinch(attacker);
			}
		}
	}

	/// <summary>
	/// Freezes the target.
	/// <summary>
	public class PokeBattle_Move_00C : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (pbIsDamaging()) return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (!opponent.pbCanFreeze(attacker, true, this)) return -1;
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			opponent.pbFreeze();
			return 0;
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			if (opponent.pbCanFreeze(attacker, false, this))
			{
				opponent.pbFreeze();
			}
		}
	}

	/// <summary>
	/// Freezes the target. Accuracy perfect in hail. (Blizzard)
	/// <summary>
	public class PokeBattle_Move_00D : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (pbIsDamaging()) return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (!opponent.pbCanFreeze(attacker, true, this)) return -1;
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			opponent.pbFreeze();
			return 0;
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			if (opponent.pbCanFreeze(attacker, false, this))
			{
				opponent.pbFreeze();
			}
		}

		public object pbModifyBaseAccuracy(byte baseaccuracy, Battle.Battler attacker, Battle.Battler opponent)
		{
			if (this.battle.Weather == Weather.HAIL)
			{
				return 0;
			}
			return baseaccuracy;
		}
	}

	/// <summary>
	/// Freezes the target. May cause the target to flinch. (Ice Fang)
	/// <summary>
	public class PokeBattle_Move_00E : PokeBattle_Move
	{
		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			if (this.battle.pbRandom(10) == 0)
			{
				if (opponent.pbCanFreeze(attacker, false, this))
				{
					opponent.pbFreeze();
				}

			}
			if (this.battle.pbRandom(10) == 0)
			{
				opponent.pbFlinch(attacker);
			}
		}
	}

	/// <summary>
	/// Causes the target to flinch.
	/// <summary>
	public class PokeBattle_Move_00F : PokeBattle_Move
	{
		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			opponent.pbFlinch(attacker);
		}
	}

	/// <summary>
	/// Causes the target to flinch. Does double damage and has perfect accuracy if
	/// the target is Minimized.
	/// <summary>
	public class PokeBattle_Move_010 : PokeBattle_Move
	{
		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			opponent.pbFlinch(attacker);
		}

		public object tramplesMinimize(byte param = 1)
		{
			if (id == Moves.DRAGON_RUSH && !Settings.USENEWBATTLEMECHANICS) return false;
			if (param == 1 && Settings.USENEWBATTLEMECHANICS) return true; // Perfect accuracy
			if (param == 2) return true; // Double damage
			return false;
		}
	}

	/// <summary>
	/// Causes the target to flinch. Fails if the user is not asleep. (Snore)
	/// <summary>
	public class PokeBattle_Move_011 : PokeBattle_Move
	{
		public object pbCanUseWhileAsleep()
		{
			return true;
		}

		public object pbMoveFailed(Battle.Battler attacker, Battle.Battler opponent)
		{
			return (attacker.Status != Status.SLEEP);
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			opponent.pbFlinch(attacker);
		}
	}

	/// <summary>
	/// Causes the target to flinch. Fails if this isn't the user's first turn. (Fake Out)
	/// <summary>
	public class PokeBattle_Move_012 : PokeBattle_Move
	{
		public object pbMoveFailed(Battle.Battler attacker, Battle.Battler opponent)
		{
			return (attacker.turncount > 1);
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			opponent.pbFlinch(attacker);
		}
	}

	/// <summary>
	/// Confuses the target.
	/// <summary>
	public class PokeBattle_Move_013 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (pbIsDamaging()) return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (opponent.pbCanConfuse(attacker, true, this))
			{
				pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

				opponent.pbConfuse();
				//battle.pbDisplay(_INTL("{1} became confused!", opponent.pbThis))
				return 0;
			}
			return -1;
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			if (opponent.pbCanConfuse(attacker, false, this))
			{
				opponent.pbConfuse();
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
		public override int AddlEffect
		{
			get
			{
				if (Settings.USENEWBATTLEMECHANICS) return 100;
				//if (attacker.Species != Pokemons.NONE && attacker.chatter != null) {
				//	return attacker.pokemon.chatter.intensity * 10 / 127;
				//}
				return 0;
			}
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			if (opponent.pbCanConfuse(attacker, false, this))
			{
				opponent.pbConfuse();
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
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (pbIsDamaging()) return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (opponent.pbCanConfuse(attacker, true, this))
			{
				pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

				opponent.pbConfuse();
				//battle.pbDisplay(_INTL("{1} became confused!", opponent.pbThis))
				return 0;
			}
			return -1;
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			if (opponent.pbCanConfuse(attacker, false, this))
			{
				opponent.pbConfuse();
				//battle.pbDisplay(_INTL("{1} became confused!", opponent.pbThis))
			}
		}

		public object pbModifyBaseAccuracy(byte baseaccuracy, Battle.Battler attacker, Battle.Battler opponent)
		{
			switch (this.battle.Weather)
			{
				case Weather.RAINDANCE:
				case Weather.HEAVYRAIN:
					return 0;
				case Weather.SUNNYDAY:
				case Weather.HARSHSUN:
					return 50;
				default:
					return baseaccuracy;
			}
		}
	}

	/// <summary>
	/// Attracts the target. (Attract)
	/// <summary>
	public class PokeBattle_Move_016 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (!opponent.pbCanAttract(attacker))
			{
				return -1;
			}
			if (!attacker.hasMoldBreaker())
			{
				if (opponent.hasWorkingAbility(Abilities.AROMA_VEIL))
				{
					//battle.pbDisplay(_INTL("But it failed because of {1}'s {2}!",
					//   opponent.pbThis,PBAbilities.getName(opponent.Ability)))
					return -1;
				}
				else if (opponent.Partner.hasWorkingAbility(Abilities.AROMA_VEIL))
				{

					//battle.pbDisplay(_INTL("But it failed because of {1}'s {2}!",
					//   opponent.Partner.pbThis,PBAbilities.getName(opponent.Partner.Ability)))
					return -1;
				}
			}

			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			opponent.pbAttract(attacker);
			return 0;
		}
	}

	/// <summary>
	/// Burns, freezes or paralyzes the target. (Tri Attack)
	/// <summary>
	public class PokeBattle_Move_017 : PokeBattle_Move
	{
		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			switch (this.battle.pbRandom(3))
			{
				case 0:
					if (opponent.pbCanBurn(attacker, false, this))
					{
						opponent.pbBurn(attacker);
					}
					break;
				case 1:
					if (opponent.pbCanFreeze(attacker, false, this))
					{
						opponent.pbFreeze();
					}
					break;
				case 2:
					if (opponent.pbCanParalyze(attacker, false, this))
					{
						opponent.pbParalyze(attacker);
					}
					break;
				default:
					return;
			}
		}
	}

	/// <summary>
	/// Cures user of burn, poison and paralysis. (Refresh)
	/// <summary>
	public class PokeBattle_Move_018 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.Status != Status.BURN &&
			   attacker.Status != Status.POISON &&
			   attacker.Status != Status.PARALYSIS)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			else
			{
				Status t = attacker.Status;
				attacker.pbCureStatus(false);

				pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);
				if (t == Status.BURN)
				{
					//battle.pbDisplay(_INTL("{1} healed its burn!", attacker.pbThis))  
				}
				else if (t == Status.POISON)
				{
					//battle.pbDisplay(_INTL("{1} cured its poisoning!", attacker.pbThis))  
				}
				else if (t == Status.PARALYSIS)
				{
					//battle.pbDisplay(_INTL("{1} cured its paralysis!", attacker.pbThis))  
				}
				return 0;
			}
		}
	}

	/// <summary>
	/// Cures all party Pokémon of permanent status problems. (Aromatherapy, Heal Bell)
	/// <summary>
	public class PokeBattle_Move_019 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{

			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);
			if (id == Moves.AROMATHERAPY)
			{
				//battle.pbDisplay(_INTL("A soothing aroma wafted through the area!"))
			}
			else
			{
				//battle.pbDisplay(_INTL("A bell chimed!"))
			}
			List<byte> activepkmn = new List<byte>();
			foreach (Battle.Battler i in this.battle.battlers)
			{
				if (attacker.IsOpposing(i.Index) || i.isFainted()) continue; //next
				activepkmn.Add(i.pokemonIndex);

				if (Settings.USENEWBATTLEMECHANICS && i.Index != attacker.Index &&
				   pbTypeImmunityByAbility(pbType(this.type, attacker, i), attacker, i)) continue; //next
				switch (i.Status)
				{
					case Status.PARALYSIS:
						//battle.pbDisplay(_INTL("{1} was cured of paralysis.", i.pbThis))
						break;
					case Status.SLEEP:

						//battle.pbDisplay(_INTL("{1}'s sleep was woken.", i.pbThis))
						break;
					case Status.POISON:

						//battle.pbDisplay(_INTL("{1} was cured of its poisoning.", i.pbThis))
						break;
					case Status.BURN:

						//battle.pbDisplay(_INTL("{1}'s burn was healed.", i.pbThis))
						break;
					case Status.FROZEN:

						//battle.pbDisplay(_INTL("{1} was thawed out.", i.pbThis))
						break;
					default: break;
				}
				i.pbCureStatus(false);

			}
			Battle.Battler[] party = this.battle.pbParty(attacker.Index); // NOTE: Considers both parties in multi battles
			for (byte i = 0; i < party.Length; i++)
			{
				if (activepkmn.Contains(i)) continue; //next
				if (party[i].Species == Pokemons.NONE || party[i].isEgg || party[i].HP <= 0) continue; //next
				switch (party[i].Status)
				{
					case Status.PARALYSIS:
						//battle.pbDisplay(_INTL("{1} was cured of paralysis.", party[i].name))
						break;
					case Status.SLEEP:

						//battle.pbDisplay(_INTL("{1} was woken from its sleep.", party[i].name))
						break;
					case Status.POISON:

						//battle.pbDisplay(_INTL("{1} was cured of its poisoning.", party[i].name))
						break;
					case Status.BURN:

						//battle.pbDisplay(_INTL("{1}'s burn was healed.", party[i].name))
						break;
					case Status.FROZEN:

						//battle.pbDisplay(_INTL("{1} was thawed out.", party[i].name))
						break;
					default:
						break;
				}
				party[i].Status = 0;
				//party[i].statusCount=0; //Done automatically
			}
			return 0;
		}
	}

	/// <summary>
	/// Safeguards the user's side from being inflicted with status problems. (Safeguard)
	/// <summary>
	public class PokeBattle_Move_01A : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.OwnSide.Safeguard > 0)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			attacker.OwnSide.Safeguard = 5;

			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);
			if (!this.battle.isOpposing(attacker.Index))
			{
				//battle.pbDisplay(_INTL("Your team became cloaked in a mystical veil!"))
			}
			else
			{
				//battle.pbDisplay(_INTL("The opposing team became cloaked in a mystical veil!"))
			}
			return 0;
		}
	}

	/// <summary>
	/// User passes its status problem to the target. (Psycho Shift)
	/// <summary>
	public class PokeBattle_Move_01B : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.Status == 0 ||
			  (attacker.Status == Status.PARALYSIS && !opponent.pbCanParalyze(attacker, false, this)) ||
			  (attacker.Status == Status.SLEEP && !opponent.pbCanSleep(attacker, false, this)) ||
			  (attacker.Status == Status.POISON && !opponent.pbCanPoison(attacker, false, this)) ||
			  (attacker.Status == Status.BURN && !opponent.pbCanBurn(attacker, false, this)) ||
			  (attacker.Status == Status.FROZEN && !opponent.pbCanFreeze(attacker, false, this)))
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);
			switch (attacker.Status)
			{
				case Status.PARALYSIS:
					opponent.pbParalyze(attacker);

					opponent.pbAbilityCureCheck();
					attacker.pbCureStatus(false);

					//battle.pbDisplay(_INTL("{1} was cured of paralysis.",attacker.pbThis))
					break;
				case Status.SLEEP:

					opponent.pbSleep();
					opponent.pbAbilityCureCheck();
					attacker.pbCureStatus(false);

					//battle.pbDisplay(_INTL("{1} woke up.",attacker.pbThis))
					break;
				case Status.POISON:

					opponent.pbPoison(attacker, null, attacker.StatusCount != 0);

					opponent.pbAbilityCureCheck();
					attacker.pbCureStatus(false);

					//battle.pbDisplay(_INTL("{1} was cured of its poisoning.",attacker.pbThis))
					break;
				case Status.BURN:

					opponent.pbBurn(attacker);
					opponent.pbAbilityCureCheck();
					attacker.pbCureStatus(false);

					//battle.pbDisplay(_INTL("{1}'s burn was healed.",attacker.pbThis))
					break;
				case Status.FROZEN:

					opponent.pbFreeze();
					opponent.pbAbilityCureCheck();
					attacker.pbCureStatus(false);

					//battle.pbDisplay(_INTL("{1} was thawed out.",attacker.pbThis))
					break;
			}
			return 0;
		}
	}

	/// <summary>
	/// Increases the user's Attack by 1 stage.
	/// <summary>
	public class PokeBattle_Move_01C : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (pbIsDamaging()) return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (!attacker.pbCanIncreaseStatStage(Stats.ATTACK, attacker, true, this)) return -1;
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool ret = attacker.pbIncreaseStat(Stats.ATTACK, 1, attacker, false, this);
			return ret ? 0 : -1;
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (attacker.pbCanIncreaseStatStage(Stats.ATTACK, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.ATTACK, 1, attacker, false, this);
			}
		}
	}

	/// <summary>
	/// Increases the user's Defense by 1 stage.
	/// <summary>
	public class PokeBattle_Move_01D : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (pbIsDamaging()) return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (!attacker.pbCanIncreaseStatStage(Stats.DEFENSE, attacker, true, this)) return -1;
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool ret = attacker.pbIncreaseStat(Stats.DEFENSE, 1, attacker, false, this);
			return ret ? 0 : -1;
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (attacker.pbCanIncreaseStatStage(Stats.DEFENSE, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.DEFENSE, 1, attacker, false, this);
			}
		}
	}

	/// <summary>
	/// Increases the user's Defense by 1 stage. User curls up. (Defense Curl)
	/// <summary>
	public class PokeBattle_Move_01E : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{

			attacker.effects.DefenseCurl = true;
			if (!attacker.pbCanIncreaseStatStage(Stats.DEFENSE, attacker, true, this)) return -1;
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool ret = attacker.pbIncreaseStat(Stats.DEFENSE, 1, attacker, false, this);
			return ret ? 0 : -1;
		}
	}

	/// <summary>
	/// Increases the user's Speed by 1 stage.
	/// <summary>
	public class PokeBattle_Move_01F : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (pbIsDamaging()) return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (!attacker.pbCanIncreaseStatStage(Stats.SPEED, attacker, true, this)) return -1;
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool ret = attacker.pbIncreaseStat(Stats.SPEED, 1, attacker, false, this);
			return ret ? 0 : -1;
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (attacker.pbCanIncreaseStatStage(Stats.SPEED, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.SPEED, 1, attacker, false, this);
			}
		}
	}

	/// <summary>
	/// Increases the user's Special Attack by 1 stage.
	/// <summary>
	public class PokeBattle_Move_020 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (pbIsDamaging()) return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (!attacker.pbCanIncreaseStatStage(Stats.SPATK, attacker, true, this)) return -1;
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool ret = attacker.pbIncreaseStat(Stats.SPATK, 1, attacker, false, this);
			return ret ? 0 : -1;
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (attacker.pbCanIncreaseStatStage(Stats.SPATK, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.SPATK, 1, attacker, false, this);
			}
		}
	}

	/// <summary>
	/// Increases the user's Special Defense by 1 stage.
	/// Charges up user's next attack if it is Electric-type. (Charge)
	/// <summary>
	public class PokeBattle_Move_021 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{

			attacker.effects.Charge = 2;
			//battle.pbDisplay(_INTL("{1} began charging power!",attacker.pbThis))
			if (attacker.pbCanIncreaseStatStage(Stats.SPDEF, attacker, true, this))
			{
				pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

				attacker.pbIncreaseStat(Stats.SPDEF, 1, attacker, false, this);
			}
			return 0;
		}
	}

	/// <summary>
	/// Increases the user's evasion by 1 stage.
	/// <summary>
	public class PokeBattle_Move_022 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (pbIsDamaging()) return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (!attacker.pbCanIncreaseStatStage(Stats.EVASION, attacker, true, this)) return -1;
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool ret = attacker.pbIncreaseStat(Stats.EVASION, 1, attacker, false, this);
			return ret ? 0 : -1;
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (attacker.pbCanIncreaseStatStage(Stats.EVASION, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.EVASION, 1, attacker, false, this);
			}
		}
	}

	/// <summary>
	/// Increases the user's critical hit rate. (Focus Energy)
	/// <summary>
	public class PokeBattle_Move_023 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (pbIsDamaging()) return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (attacker.effects.FocusEnergy >= 2)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			attacker.effects.FocusEnergy = 2;
			//battle.pbDisplay(_INTL("{1} is getting pumped!",attacker.pbThis))
			return 0;
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (attacker.effects.FocusEnergy < 2)
			{
				attacker.effects.FocusEnergy = 2;
				//battle.pbDisplay(_INTL("{1} is getting pumped!",attacker.pbThis))
			}
		}
	}

	/// <summary>
	/// Increases the user's Attack and Defense by 1 stage each. (Bulk Up)
	/// <summary>
	public class PokeBattle_Move_024 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (!attacker.pbCanIncreaseStatStage(Stats.ATTACK, attacker, false, this) &&
			   !attacker.pbCanIncreaseStatStage(Stats.DEFENSE, attacker, false, this))
			{
				//battle.pbDisplay(_INTL("{1}'s stats won't go any higher!",attacker.pbThis))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool showanim = true;
			if (attacker.pbCanIncreaseStatStage(Stats.ATTACK, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.ATTACK, 1, attacker, false, this, showanim);
				showanim = false;
			}
			if (attacker.pbCanIncreaseStatStage(Stats.DEFENSE, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.DEFENSE, 1, attacker, false, this, showanim);
				showanim = false;
			}
			return 0;
		}
	}

	/// <summary>
	/// Increases the user's Attack, Defense and accuracy by 1 stage each. (Coil)
	/// <summary>
	public class PokeBattle_Move_025 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (!attacker.pbCanIncreaseStatStage(Stats.ATTACK, attacker, false, this) &&
			   !attacker.pbCanIncreaseStatStage(Stats.DEFENSE, attacker, false, this) &&
			   !attacker.pbCanIncreaseStatStage(Stats.ACCURACY, attacker, false, this))
			{
				//battle.pbDisplay(_INTL("{1}'s stats won't go any higher!",attacker.pbThis))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool showanim = true;
			if (attacker.pbCanIncreaseStatStage(Stats.ATTACK, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.ATTACK, 1, attacker, false, this, showanim);
				showanim = false;
			}
			if (attacker.pbCanIncreaseStatStage(Stats.DEFENSE, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.DEFENSE, 1, attacker, false, this, showanim);
				showanim = false;
			}
			if (attacker.pbCanIncreaseStatStage(Stats.ACCURACY, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.ACCURACY, 1, attacker, false, this, showanim);
				showanim = false;
			}
			return 0;
		}
	}

	/// <summary>
	/// Increases the user's Attack and Speed by 1 stage each. (Dragon Dance)
	/// <summary>
	public class PokeBattle_Move_026 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (!attacker.pbCanIncreaseStatStage(Stats.ATTACK, attacker, false, this) &&
			   !attacker.pbCanIncreaseStatStage(Stats.SPEED, attacker, false, this))
			{
				//battle.pbDisplay(_INTL("{1}'s stats won't go any higher!",attacker.pbThis))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool showanim = true;
			if (attacker.pbCanIncreaseStatStage(Stats.ATTACK, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.ATTACK, 1, attacker, false, this, showanim);
				showanim = false;
			}
			if (attacker.pbCanIncreaseStatStage(Stats.SPEED, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.SPEED, 1, attacker, false, this, showanim);
				showanim = false;
			}
			return 0;
		}
	}

	/// <summary>
	/// Increases the user's Attack and Special Attack by 1 stage each. (Work Up)
	/// <summary>
	public class PokeBattle_Move_027 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (!attacker.pbCanIncreaseStatStage(Stats.ATTACK, attacker, false, this) &&
			   !attacker.pbCanIncreaseStatStage(Stats.SPATK, attacker, false, this))
			{
				//battle.pbDisplay(_INTL("{1}'s stats won't go any higher!",attacker.pbThis))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool showanim = true;
			if (attacker.pbCanIncreaseStatStage(Stats.ATTACK, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.ATTACK, 1, attacker, false, this, showanim);
				showanim = false;
			}
			if (attacker.pbCanIncreaseStatStage(Stats.SPATK, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.SPATK, 1, attacker, false, this, showanim);
				showanim = false;
			}
			return 0;
		}
	}

	/// <summary>
	/// Increases the user's Attack and Sp. Attack by 1 stage each.
	/// In sunny weather, increase is 2 stages each instead. (Growth)
	/// <summary>
	public class PokeBattle_Move_028 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (!attacker.pbCanIncreaseStatStage(Stats.ATTACK, attacker, false, this) &&
			   !attacker.pbCanIncreaseStatStage(Stats.SPATK, attacker, false, this))
			{
				//battle.pbDisplay(_INTL("{1}'s stats won't go any higher!",attacker.pbThis))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool showanim = true;
			byte increment = 1;
			if (this.battle.Weather == Weather.SUNNYDAY ||
			   this.battle.Weather == Weather.HARSHSUN)
			{
				increment = 2;

			}
			if (attacker.pbCanIncreaseStatStage(Stats.ATTACK, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.ATTACK, increment, attacker, false, this, showanim);
				showanim = false;
			}
			if (attacker.pbCanIncreaseStatStage(Stats.SPATK, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.SPATK, increment, attacker, false, this, showanim);
				showanim = false;
			}
			return 0;
		}
	}

	/// <summary>
	/// Increases the user's Attack and accuracy by 1 stage each. (Hone Claws)
	/// <summary>
	public class PokeBattle_Move_029 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (!attacker.pbCanIncreaseStatStage(Stats.ATTACK, attacker, false, this) &&
			   !attacker.pbCanIncreaseStatStage(Stats.ACCURACY, attacker, false, this))
			{
				//battle.pbDisplay(_INTL("{1}'s stats won't go any higher!",attacker.pbThis))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool showanim = true;
			if (attacker.pbCanIncreaseStatStage(Stats.ATTACK, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.ATTACK, 1, attacker, false, this, showanim);
				showanim = false;
			}
			if (attacker.pbCanIncreaseStatStage(Stats.ACCURACY, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.ACCURACY, 1, attacker, false, this, showanim);
				showanim = false;
			}
			return 0;
		}
	}

	/// <summary>
	/// Increases the user's Defense and Special Defense by 1 stage each. (Cosmic Power)
	/// <summary>
	public class PokeBattle_Move_02A : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (!attacker.pbCanIncreaseStatStage(Stats.DEFENSE, attacker, false, this) &&
			   !attacker.pbCanIncreaseStatStage(Stats.SPDEF, attacker, false, this))
			{
				//battle.pbDisplay(_INTL("{1}'s stats won't go any higher!",attacker.pbThis))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool showanim = true;
			if (attacker.pbCanIncreaseStatStage(Stats.DEFENSE, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.DEFENSE, 1, attacker, false, this, showanim);
				showanim = false;
			}
			if (attacker.pbCanIncreaseStatStage(Stats.SPDEF, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.SPDEF, 1, attacker, false, this, showanim);
				showanim = false;
			}
			return 0;
		}
	}

	/// <summary>
	/// Increases the user's Sp. Attack, Sp. Defense and Speed by 1 stage each. (Quiver Dance)
	/// <summary>
	public class PokeBattle_Move_02B : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (!attacker.pbCanIncreaseStatStage(Stats.SPATK, attacker, false, this) &&
			   !attacker.pbCanIncreaseStatStage(Stats.SPDEF, attacker, false, this) &&
			   !attacker.pbCanIncreaseStatStage(Stats.SPEED, attacker, false, this))
			{
				//battle.pbDisplay(_INTL("{1}'s stats won't go any higher!",attacker.pbThis))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool showanim = true;
			if (attacker.pbCanIncreaseStatStage(Stats.SPATK, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.SPATK, 1, attacker, false, this, showanim);
				showanim = false;
			}
			if (attacker.pbCanIncreaseStatStage(Stats.SPDEF, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.SPDEF, 1, attacker, false, this, showanim);
				showanim = false;
			}
			if (attacker.pbCanIncreaseStatStage(Stats.SPEED, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.SPEED, 1, attacker, false, this, showanim);
				showanim = false;
			}
			return 0;
		}
	}

	/// <summary>
	/// Increases the user's Sp. Attack and Sp. Defense by 1 stage each. (Calm Mind)
	/// <summary>
	public class PokeBattle_Move_02C : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (!attacker.pbCanIncreaseStatStage(Stats.SPATK, attacker, false, this) &&
			   !attacker.pbCanIncreaseStatStage(Stats.SPDEF, attacker, false, this))
			{
				//battle.pbDisplay(_INTL("{1}'s stats won't go any higher!",attacker.pbThis))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool showanim = true;
			if (attacker.pbCanIncreaseStatStage(Stats.SPATK, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.SPATK, 1, attacker, false, this, showanim);
				showanim = false;
			}
			if (attacker.pbCanIncreaseStatStage(Stats.SPDEF, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.SPDEF, 1, attacker, false, this, showanim);
				showanim = false;
			}
			return 0;
		}
	}

	/// <summary>
	/// Increases the user's Attack, Defense, Speed, Special Attack and Special Defense
	/// by 1 stage each. (AncientPower, Ominous Wind, Silver Wind)
	/// <summary>
	public class PokeBattle_Move_02D : PokeBattle_Move
	{
		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{

			bool showanim = true;
			if (attacker.pbCanIncreaseStatStage(Stats.ATTACK, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.ATTACK, 1, attacker, false, this, showanim);
				showanim = false;
			}
			if (attacker.pbCanIncreaseStatStage(Stats.DEFENSE, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.DEFENSE, 1, attacker, false, this, showanim);
				showanim = false;
			}
			if (attacker.pbCanIncreaseStatStage(Stats.SPATK, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.SPATK, 1, attacker, false, this, showanim);
				showanim = false;
			}
			if (attacker.pbCanIncreaseStatStage(Stats.SPDEF, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.SPDEF, 1, attacker, false, this, showanim);
				showanim = false;
			}
			if (attacker.pbCanIncreaseStatStage(Stats.SPEED, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.SPEED, 1, attacker, false, this, showanim);
				showanim = false;
			}
		}
	}

	/// <summary>
	/// Increases the user's Attack by 2 stages.
	/// <summary>
	public class PokeBattle_Move_02E : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (pbIsDamaging()) return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (!attacker.pbCanIncreaseStatStage(Stats.ATTACK, attacker, true, this)) return -1;
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool ret = attacker.pbIncreaseStat(Stats.ATTACK, 2, attacker, false, this);
			return ret ? 0 : -1;
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (attacker.pbCanIncreaseStatStage(Stats.ATTACK, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.ATTACK, 2, attacker, false, this);
			}
		}
	}

	/// <summary>
	/// Increases the user's Defense by 2 stages.
	/// <summary>
	public class PokeBattle_Move_02F : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (pbIsDamaging()) return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (!attacker.pbCanIncreaseStatStage(Stats.DEFENSE, attacker, true, this)) return -1;
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool ret = attacker.pbIncreaseStat(Stats.DEFENSE, 2, attacker, false, this);
			return ret ? 0 : -1;
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (attacker.pbCanIncreaseStatStage(Stats.DEFENSE, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.DEFENSE, 2, attacker, false, this);
			}
		}
	}

	/// <summary>
	/// Increases the user's Speed by 2 stages.
	/// <summary>
	public class PokeBattle_Move_030 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (pbIsDamaging()) return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (!attacker.pbCanIncreaseStatStage(Stats.SPEED, attacker, true, this)) return -1;
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool ret = attacker.pbIncreaseStat(Stats.SPEED, 2, attacker, false, this);
			return ret ? 0 : -1;
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (attacker.pbCanIncreaseStatStage(Stats.SPEED, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.SPEED, 2, attacker, false, this);
			}
		}
	}

	/// <summary>
	/// Increases the user's Speed by 2 stages. Lowers user's weight by 100kg. (Autotomize)
	/// <summary>
	public class PokeBattle_Move_031 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (!attacker.pbCanIncreaseStatStage(Stats.SPEED, attacker, true, this)) return -1;
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool ret = attacker.pbIncreaseStat(Stats.SPEED, 2, attacker, false, this);
			if (ret)
			{
				attacker.effects.WeightChange -= 1000;

				//battle.pbDisplay(_INTL("{1} became nimble!", attacker.pbThis))
			}
			return ret ? 0 : -1;
		}
	}

	/// <summary>
	/// Increases the user's Special Attack by 2 stages.
	/// <summary>
	public class PokeBattle_Move_032 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (pbIsDamaging()) return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (!attacker.pbCanIncreaseStatStage(Stats.SPATK, attacker, true, this)) return -1;
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool ret = attacker.pbIncreaseStat(Stats.SPATK, 2, attacker, false, this);
			return ret ? 0 : -1;
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (attacker.pbCanIncreaseStatStage(Stats.SPATK, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.SPATK, 2, attacker, false, this);
			}
		}
	}

	/// <summary>
	/// Increases the user's Special Defense by 2 stages.
	/// <summary>
	public class PokeBattle_Move_033 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (pbIsDamaging()) return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (!attacker.pbCanIncreaseStatStage(Stats.SPDEF, attacker, true, this)) return -1;
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool ret = attacker.pbIncreaseStat(Stats.SPDEF, 2, attacker, false, this);
			return ret ? 0 : -1;
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (attacker.pbCanIncreaseStatStage(Stats.SPDEF, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.SPDEF, 2, attacker, false, this);
			}
		}
	}

	/// <summary>
	/// Increases the user's evasion by 2 stages. Minimizes the user. (Minimize)
	/// <summary>
	public class PokeBattle_Move_034 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (pbIsDamaging()) return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			attacker.effects.Minimize = true;
			if (!attacker.pbCanIncreaseStatStage(Stats.EVASION, attacker, true, this)) return -1;

			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool ret = attacker.pbIncreaseStat(Stats.EVASION, 2, attacker, false, this);
			return ret ? 0 : -1;
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{

			attacker.effects.Minimize = true;
			if (attacker.pbCanIncreaseStatStage(Stats.EVASION, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.EVASION, 2, attacker, false, this);
			}
		}
	}

	/// <summary>
	/// Decreases the user's Defense and Special Defense by 1 stage each. (Shell Smash)
	/// Increases the user's Attack, Speed and Special Attack by 2 stages each.
	/// <summary>
	public class PokeBattle_Move_035 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (!attacker.pbCanIncreaseStatStage(Stats.ATTACK, attacker, false, this) &&
			   !attacker.pbCanIncreaseStatStage(Stats.SPATK, attacker, false, this) &&
			   !attacker.pbCanIncreaseStatStage(Stats.SPEED, attacker, false, this))
			{
				//battle.pbDisplay(_INTL("{1}'s stats won't go any higher!",attacker.pbThis))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool showanim = true;
			if (attacker.pbCanReduceStatStage(Stats.DEFENSE, attacker, false, this))
			{
				attacker.pbReduceStat(Stats.DEFENSE, 1, attacker, false, this, showanim);
				showanim = false;
			}
			if (attacker.pbCanReduceStatStage(Stats.SPDEF, attacker, false, this))
			{
				attacker.pbReduceStat(Stats.SPDEF, 1, attacker, false, this, showanim);
				showanim = false;
			}
			showanim = true;
			if (attacker.pbCanIncreaseStatStage(Stats.ATTACK, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.ATTACK, 2, attacker, false, this, showanim);
				showanim = false;
			}
			if (attacker.pbCanIncreaseStatStage(Stats.SPATK, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.SPATK, 2, attacker, false, this, showanim);
				showanim = false;
			}
			if (attacker.pbCanIncreaseStatStage(Stats.SPEED, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.SPEED, 2, attacker, false, this, showanim);
				showanim = false;
			}
			return 0;
		}
	}

	/// <summary>
	/// Increases the user's Speed by 2 stages, and its Attack by 1 stage. (Shift Gear)
	/// <summary>
	public class PokeBattle_Move_036 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (!attacker.pbCanIncreaseStatStage(Stats.ATTACK, attacker, false, this) &&
			   !attacker.pbCanIncreaseStatStage(Stats.SPEED, attacker, false, this))
			{
				//battle.pbDisplay(_INTL("{1}'s stats won't go any higher!",attacker.pbThis))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool showanim = true;
			if (attacker.pbCanIncreaseStatStage(Stats.SPEED, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.SPEED, 2, attacker, false, this, showanim);
				showanim = false;
			}
			if (attacker.pbCanIncreaseStatStage(Stats.ATTACK, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.ATTACK, 1, attacker, false, this, showanim);
				showanim = false;
			}
			return 0;
		}
	}

	/// <summary>
	/// Increases one random stat of the user by 2 stages (except HP). (Acupressure)
	/// <summary>
	public class PokeBattle_Move_037 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.Index != opponent.Index)
			{
				if ((opponent.effects.Substitute > 0 && !ignoresSubstitute(attacker)) ||
				   opponent.OwnSide.CraftyShield)
				{
					//battle.pbDisplay(_INTL("But it failed!"))
					return -1;
				}
			}

			List<Stats> array = new List<Stats>();
			foreach (var i in new[] { Stats.ATTACK, Stats.DEFENSE, Stats.SPEED,
				  Stats.SPATK, Stats.SPDEF, Stats.ACCURACY, Stats.EVASION })
			{
				if (opponent.pbCanIncreaseStatStage(i, attacker, false, this)) array.Add(i);
			}
			if (array.Count == 0)
			{
				//battle.pbDisplay(_INTL("{1}'s stats won't go any higher!",opponent.pbThis))
				return -1;
			}
			Stats stat = array[this.battle.pbRandom(array.Count)];

			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			object ret = opponent.pbIncreaseStat(stat, 2, attacker, false, this);
			return 0;
		}
	}

	/// <summary>
	/// Increases the user's Defense by 3 stages.
	/// <summary>
	public class PokeBattle_Move_038 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (pbIsDamaging()) return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (!attacker.pbCanIncreaseStatStage(Stats.DEFENSE, attacker, true, this)) return -1;
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool ret = attacker.pbIncreaseStat(Stats.DEFENSE, 3, attacker, false, this);
			return ret ? 0 : -1;
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (attacker.pbCanIncreaseStatStage(Stats.DEFENSE, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.DEFENSE, 3, attacker, false, this);
			}
		}
	}

	/// <summary>
	/// Increases the user's Special Attack by 3 stages.
	/// <summary>
	public class PokeBattle_Move_039 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (pbIsDamaging()) return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (!attacker.pbCanIncreaseStatStage(Stats.SPATK, attacker, true, this)) return -1;
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool ret = attacker.pbIncreaseStat(Stats.SPATK, 3, attacker, false, this);
			return ret ? 0 : -1;
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (attacker.pbCanIncreaseStatStage(Stats.DEFENSE, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.SPATK, 3, attacker, false, this);
			}
		}
	}

	/// <summary>
	/// Reduces the user's HP by half of max, and sets its Attack to maximum. (Belly Drum)
	/// <summary>
	public class PokeBattle_Move_03A : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.HP <= Math.Floor(attacker.TotalHP / 2f) ||
			   !attacker.pbCanIncreaseStatStage(Stats.ATTACK, attacker, false, this))
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			attacker.ReduceHP((int)Math.Floor(attacker.TotalHP / 2f));
			if (attacker.hasWorkingAbility(Abilities.CONTRARY))
			{
				attacker.stages[(byte)Stats.ATTACK] = -6;
				this.battle.pbCommonAnimation("StatDown", attacker, null);
				//battle.pbDisplay(_INTL("{1} cut its own HP and minimized its Attack!",attacker.pbThis))
			}
			else
			{
				attacker.stages[(byte)Stats.ATTACK] = 6;
				this.battle.pbCommonAnimation("StatUp", attacker, null);
				//battle.pbDisplay(_INTL("{1} cut its own HP and maximized its Attack!",attacker.pbThis))
			}
			return 0;
		}
	}

	/// <summary>
	/// Decreases the user's Attack and Defense by 1 stage each. (Superpower)
	/// <summary>
	public class PokeBattle_Move_03B : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{

			object ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (opponent.damagestate.CalcDamage > 0)
			{
				bool showanim = true;
				if (attacker.pbCanReduceStatStage(Stats.ATTACK, attacker, false, this))
				{
					attacker.pbReduceStat(Stats.ATTACK, 1, attacker, false, this, showanim);
					showanim = false;
				}
				if (attacker.pbCanReduceStatStage(Stats.DEFENSE, attacker, false, this))
				{
					attacker.pbReduceStat(Stats.DEFENSE, 1, attacker, false, this, showanim);
					showanim = false;
				}
			}
			return ret;
		}
	}

	/// <summary>
	/// Decreases the user's Defense and Special Defense by 1 stage each. (Close Combat)
	/// <summary>
	public class PokeBattle_Move_03C : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{

			object ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (opponent.damagestate.CalcDamage > 0)
			{
				bool showanim = true;
				if (attacker.pbCanReduceStatStage(Stats.DEFENSE, attacker, false, this))
				{
					attacker.pbReduceStat(Stats.DEFENSE, 1, attacker, false, this, showanim);
					showanim = false;
				}
				if (attacker.pbCanReduceStatStage(Stats.SPDEF, attacker, false, this))
				{
					attacker.pbReduceStat(Stats.SPDEF, 1, attacker, false, this, showanim);
					showanim = false;
				}
			}
			return ret;
		}
	}

	/// <summary>
	/// Decreases the user's Defense, Special Defense and Speed by 1 stage each.
	/// User's ally loses 1/16 of its total HP. (V-create)
	/// <summary>
	public class PokeBattle_Move_03D : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{

			object ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (opponent.damagestate.CalcDamage > 0)
			{
				if (attacker.Partner.Species != Pokemons.NONE && !attacker.Partner.isFainted())
				{
					attacker.Partner.ReduceHP((int)Math.Floor(attacker.Partner.TotalHP / 16f), true);
				}
				bool showanim = true;
				if (attacker.pbCanReduceStatStage(Stats.SPEED, attacker, false, this))
				{
					attacker.pbReduceStat(Stats.SPEED, 1, attacker, false, this, showanim);
					showanim = false;
				}
				if (attacker.pbCanReduceStatStage(Stats.DEFENSE, attacker, false, this))
				{
					attacker.pbReduceStat(Stats.DEFENSE, 1, attacker, false, this, showanim);
					showanim = false;
				}
				if (attacker.pbCanReduceStatStage(Stats.SPDEF, attacker, false, this))
				{
					attacker.pbReduceStat(Stats.SPDEF, 1, attacker, false, this, showanim);
					showanim = false;
				}
			}
			return ret;
		}
	}

	/// <summary>
	/// Decreases the user's Speed by 1 stage.
	/// <summary>
	public class PokeBattle_Move_03E : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{

			object ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (opponent.damagestate.CalcDamage > 0)
			{
				if (attacker.pbCanReduceStatStage(Stats.SPEED, attacker, false, this))
				{
					attacker.pbReduceStat(Stats.SPEED, 1, attacker, false, this);
				}
			}
			return ret;
		}
	}

	/// <summary>
	/// Decreases the user's Special Attack by 2 stages.
	/// <summary>
	public class PokeBattle_Move_03F : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{

			object ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (opponent.damagestate.CalcDamage > 0)
			{
				if (attacker.pbCanReduceStatStage(Stats.SPATK, attacker, false, this))
				{
					attacker.pbReduceStat(Stats.SPATK, 2, attacker, false, this);
				}
			}
			return ret;
		}
	}

	/// <summary>
	/// Increases the target's Special Attack by 1 stage. Confuses the target. (Flatter)
	/// <summary>
	public class PokeBattle_Move_040 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (opponent.effects.Substitute > 0 && !ignoresSubstitute(attacker))
			{
				//battle.pbDisplay(_INTL("{1}'s attack missed!", attacker.pbThis))
				return -1;
			}
			object ret = -1;

			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);
			if (opponent.pbCanIncreaseStatStage(Stats.SPATK, attacker, false, this))
			{
				opponent.pbIncreaseStat(Stats.SPATK, 1, attacker, false, this);
				ret = 0;
			}
			if (opponent.pbCanConfuse(attacker, true, this))
			{
				opponent.pbConfuse();
				//battle.pbDisplay(_INTL("{1} became confused!", opponent.pbThis))
				ret = 0;
			}
			return ret;
		}
	}

	/// <summary>
	/// Increases the target's Attack by 2 stages. Confuses the target. (Swagger)
	/// <summary>
	public class PokeBattle_Move_041 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (opponent.effects.Substitute > 0 && !ignoresSubstitute(attacker))
			{
				//battle.pbDisplay(_INTL("{1}'s attack missed!", attacker.pbThis))
				return -1;
			}
			object ret = -1;

			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);
			if (opponent.pbCanIncreaseStatStage(Stats.ATTACK, attacker, false, this))
			{
				opponent.pbIncreaseStat(Stats.ATTACK, 2, attacker, false, this);
				ret = 0;
			}
			if (opponent.pbCanConfuse(attacker, true, this))
			{
				opponent.pbConfuse();
				//battle.pbDisplay(_INTL("{1} became confused!", opponent.pbThis))
				ret = 0;
			}
			return ret;
		}
	}

	/// <summary>
	/// Decreases the target's Attack by 1 stage.
	/// <summary>
	public class PokeBattle_Move_042 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (pbIsDamaging()) return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (!opponent.pbCanReduceStatStage(Stats.ATTACK, attacker, true, this)) return -1;
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool ret = opponent.pbReduceStat(Stats.ATTACK, 1, attacker, false, this);
			return ret ? 0 : -1;
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			if (opponent.pbCanReduceStatStage(Stats.ATTACK, attacker, false, this))
			{
				opponent.pbReduceStat(Stats.ATTACK, 1, attacker, false, this);
			}
		}
	}

	/// <summary>
	/// Decreases the target's Defense by 1 stage.
	/// <summary>
	public class PokeBattle_Move_043 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (pbIsDamaging()) return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (!opponent.pbCanReduceStatStage(Stats.DEFENSE, attacker, true, this)) return -1;
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool ret = opponent.pbReduceStat(Stats.DEFENSE, 1, attacker, false, this);
			return ret ? 0 : -1;
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			if (opponent.pbCanReduceStatStage(Stats.DEFENSE, attacker, false, this))
			{
				opponent.pbReduceStat(Stats.DEFENSE, 1, attacker, false, this);
			}
		}
	}

	/// <summary>
	/// Decreases the target's Speed by 1 stage.
	/// <summary>
	public class PokeBattle_Move_044 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (pbIsDamaging()) return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (!opponent.pbCanReduceStatStage(Stats.SPEED, attacker, true, this)) return -1;
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool ret = opponent.pbReduceStat(Stats.SPEED, 1, attacker, false, this);
			return ret ? 0 : -1;
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			if (opponent.pbCanReduceStatStage(Stats.SPEED, attacker, false, this))
			{
				opponent.pbReduceStat(Stats.SPEED, 1, attacker, false, this);
			}
		}

		public object pbModifyDamage(int damagemult, Battle.Battler attacker, Battle.Battler opponent)
		{
			if (id == Moves.BULLDOZE &&
			   this.battle.field.GrassyTerrain > 0)
			{
				return (int)Math.Round(damagemult / 2.0f);
			}
			return damagemult;
		}
	}

	/// <summary>
	/// Decreases the target's Special Attack by 1 stage.
	/// <summary>
	public class PokeBattle_Move_045 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (pbIsDamaging()) return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (!opponent.pbCanReduceStatStage(Stats.SPATK, attacker, true, this)) return -1;
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool ret = opponent.pbReduceStat(Stats.SPATK, 1, attacker, false, this);
			return ret ? 0 : -1;
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			if (opponent.pbCanReduceStatStage(Stats.SPATK, attacker, false, this))
			{
				opponent.pbReduceStat(Stats.SPATK, 1, attacker, false, this);
			}
		}
	}

	/// <summary>
	/// Decreases the target's Special Defense by 1 stage.
	/// <summary>
	public class PokeBattle_Move_046 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (pbIsDamaging()) return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (!opponent.pbCanReduceStatStage(Stats.SPDEF, attacker, true, this)) return -1;
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool ret = opponent.pbReduceStat(Stats.SPDEF, 1, attacker, false, this);
			return ret ? 0 : -1;
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			if (opponent.pbCanReduceStatStage(Stats.SPDEF, attacker, false, this))
			{
				opponent.pbReduceStat(Stats.SPDEF, 1, attacker, false, this);
			}
		}
	}

	/// <summary>
	/// Decreases the target's accuracy by 1 stage.
	/// <summary>
	public class PokeBattle_Move_047 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (pbIsDamaging()) return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (!opponent.pbCanReduceStatStage(Stats.ACCURACY, attacker, true, this)) return -1;
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool ret = opponent.pbReduceStat(Stats.ACCURACY, 1, attacker, false, this);
			return ret ? 0 : -1;
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			if (opponent.pbCanReduceStatStage(Stats.ACCURACY, attacker, false, this))
			{
				opponent.pbReduceStat(Stats.ACCURACY, 1, attacker, false, this);
			}
		}
	}

	/// <summary>
	/// Decreases the target's evasion by 1 stage OR 2 stages. (Sweet Scent)
	/// <summary>
	public class PokeBattle_Move_048 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (pbIsDamaging()) return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (!opponent.pbCanReduceStatStage(Stats.EVASION, attacker, true, this)) return -1;
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			byte increment = (Settings.USENEWBATTLEMECHANICS) ? 2 : 1;
			bool ret = opponent.pbReduceStat(Stats.EVASION, increment, attacker, false, this);
			return ret ? 0 : -1;
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			if (opponent.pbCanReduceStatStage(Stats.EVASION, attacker, false, this))
			{
				byte increment = (Settings.USENEWBATTLEMECHANICS) ? 2 : 1;
				opponent.pbReduceStat(Stats.EVASION, increment, attacker, false, this);
			}
		}
	}

	/// <summary>
	/// Decreases the target's evasion by 1 stage. Ends all barriers and entry
	/// hazards for the target's side OR on both sides. (Defog)
	/// <summary>
	public class PokeBattle_Move_049 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (pbIsDamaging()) return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			opponent.pbReduceStat(Stats.EVASION, 1, attacker, false, this);
			opponent.OwnSide.Reflect = 0;
			opponent.OwnSide.LightScreen = 0;
			opponent.OwnSide.Mist = 0;
			opponent.OwnSide.Safeguard = 0;
			opponent.OwnSide.Spikes = 0;
			opponent.OwnSide.StealthRock = false;
			opponent.OwnSide.StickyWeb = false;
			opponent.OwnSide.ToxicSpikes = 0;
			if (Settings.USENEWBATTLEMECHANICS)
			{
				opponent.OpposingSide.Reflect = 0;

				opponent.OpposingSide.LightScreen = 0;

				opponent.OpposingSide.Mist = 0;

				opponent.OpposingSide.Safeguard = 0;

				opponent.OpposingSide.Spikes = 0;

				opponent.OpposingSide.StealthRock = false;

				opponent.OpposingSide.StickyWeb = false;

				opponent.OpposingSide.ToxicSpikes = 0;

			}
			return 0;
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (!opponent.damagestate.Substitute)
			{
				if (opponent.pbCanReduceStatStage(Stats.EVASION, attacker, false, this))
				{

					opponent.pbReduceStat(Stats.EVASION, 1, attacker, false, this);
				}
			}

			opponent.OwnSide.Reflect = 0;
			opponent.OwnSide.LightScreen = 0;
			opponent.OwnSide.Mist = 0;
			opponent.OwnSide.Safeguard = 0;
			opponent.OwnSide.Spikes = 0;
			opponent.OwnSide.StealthRock = false;
			opponent.OwnSide.StickyWeb = false;
			opponent.OwnSide.ToxicSpikes = 0;
			if (Settings.USENEWBATTLEMECHANICS)
			{
				opponent.OpposingSide.Reflect = 0;

				opponent.OpposingSide.LightScreen = 0;

				opponent.OpposingSide.Mist = 0;

				opponent.OpposingSide.Safeguard = 0;

				opponent.OpposingSide.Spikes = 0;

				opponent.OpposingSide.StealthRock = false;

				opponent.OpposingSide.StickyWeb = false;

				opponent.OpposingSide.ToxicSpikes = 0;

			}
		}
	}

	/// <summary>
	/// Decreases the target's Attack and Defense by 1 stage each. (Tickle)
	/// <summary>
	public class PokeBattle_Move_04A : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			// Replicates pbCanReduceStatStage? so that certain messages aren't shown
			// multiple times
			if (opponent.effects.Substitute > 0 && !ignoresSubstitute(attacker))
			{
				//battle.pbDisplay(_INTL("{1}'s attack missed!", attacker.pbThis))
				return -1;
			}
			if (opponent.pbTooLow(Stats.ATTACK) &&
			   opponent.pbTooLow(Stats.DEFENSE))
			{
				//battle.pbDisplay(_INTL("{1}'s stats won't go any lower!", opponent.pbThis))
				return -1;
			}
			if (opponent.OwnSide.Mist > 0)
			{
				//battle.pbDisplay(_INTL("{1} is protected by Mist!",opponent.pbThis))
				return -1;
			}
			if (!attacker.hasMoldBreaker())
			{
				if (opponent.hasWorkingAbility(Abilities.CLEAR_BODY) ||
				   opponent.hasWorkingAbility(Abilities.WHITE_SMOKE))
				{
					//battle.pbDisplay(_INTL("{1}'s {2} prevents stat loss!",opponent.pbThis,
					//   PBAbilities.getName(opponent.Ability)))
					return -1;
				}
			}

			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			object ret = -1; bool showanim = true;
			if (!attacker.hasMoldBreaker() && opponent.hasWorkingAbility(Abilities.HYPER_CUTTER) &&
			   !opponent.pbTooLow(Stats.ATTACK))
			{
				string abilityname = PBAbilities.getName(opponent.Ability);

				//battle.pbDisplay(_INTL("{1}'s {2} prevents Attack loss!",opponent.pbThis,abilityname))
			}
			else if (opponent.pbReduceStat(Stats.ATTACK, 1, attacker, false, this, showanim))
			{

				ret = 0; showanim = false;
			}
			if (!attacker.hasMoldBreaker() && opponent.hasWorkingAbility(Abilities.BIG_PECKS) &&
			   !opponent.pbTooLow(Stats.DEFENSE))
			{
				string abilityname = PBAbilities.getName(opponent.Ability);

				//battle.pbDisplay(_INTL("{1}'s {2} prevents Defense loss!",opponent.pbThis,abilityname))
			}
			else if (opponent.pbReduceStat(Stats.DEFENSE, 1, attacker, false, this, showanim))
			{

				ret = 0; showanim = false;
			}
			return ret;
		}
	}

	/// <summary>
	/// Decreases the target's Attack by 2 stages.
	/// <summary>
	public class PokeBattle_Move_04B : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (pbIsDamaging()) return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (!opponent.pbCanReduceStatStage(Stats.ATTACK, attacker, true, this)) return -1;
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool ret = opponent.pbReduceStat(Stats.ATTACK, 2, attacker, false, this);
			return ret ? 0 : -1;
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			if (opponent.pbCanReduceStatStage(Stats.ATTACK, attacker, false, this))
			{
				opponent.pbReduceStat(Stats.ATTACK, 2, attacker, false, this);
			}
		}
	}

	/// <summary>
	/// Decreases the target's Defense by 2 stages. (Screech)
	/// <summary>
	public class PokeBattle_Move_04C : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (pbIsDamaging()) return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (!opponent.pbCanReduceStatStage(Stats.DEFENSE, attacker, true, this)) return -1;
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool ret = opponent.pbReduceStat(Stats.DEFENSE, 2, attacker, false, this);
			return ret ? 0 : -1;
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			if (opponent.pbCanReduceStatStage(Stats.DEFENSE, attacker, false, this))
			{
				opponent.pbReduceStat(Stats.DEFENSE, 2, attacker, false, this);
			}
		}
	}

	/// <summary>
	/// Decreases the target's Speed by 2 stages. (Cotton Spore, Scary Face, String Shot)
	/// <summary>
	public class PokeBattle_Move_04D : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (pbIsDamaging()) return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (pbTypeImmunityByAbility(pbType(this.type, attacker, opponent), attacker, opponent)) return -1;
			if (!opponent.pbCanReduceStatStage(Stats.SPEED, attacker, true, this)) return -1;
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			byte increment = (id == Moves.STRING_SHOT && !Settings.USENEWBATTLEMECHANICS) ? (byte)1 : (byte)2;
			bool ret = opponent.pbReduceStat(Stats.SPEED, increment, attacker, false, this);
			return ret ? 0 : -1;
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			if (opponent.pbCanReduceStatStage(Stats.SPEED, attacker, false, this))
			{
				byte increment = (id == Moves.STRING_SHOT && !Settings.USENEWBATTLEMECHANICS) ? (byte)1 : (byte)2;
				opponent.pbReduceStat(Stats.SPEED, increment, attacker, false, this);
			}
		}
	}

	/// <summary>
	/// Decreases the target's Special Attack by 2 stages. Only works on the opposite
	/// gender. (Captivate)
	/// <summary>
	public class PokeBattle_Move_04E : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (pbIsDamaging()) return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (!opponent.pbCanReduceStatStage(Stats.SPATK, attacker, true, this)) return -1;
			if (attacker.gender == 2 || opponent.gender == 2 || attacker.gender == opponent.gender)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			if (!attacker.hasMoldBreaker() && opponent.hasWorkingAbility(Abilities.OBLIVIOUS))
			{
				//battle.pbDisplay(_INTL("{1}'s {2} prevents romance!",opponent.pbThis,
				// PBAbilities.getName(opponent.Ability)))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool ret = opponent.pbReduceStat(Stats.SPATK, 2, attacker, false, this);
			return ret ? 0 : -1;
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			if (attacker.gender != 2 && opponent.gender != 2 && attacker.gender != opponent.gender)
			{
				if (attacker.hasMoldBreaker() || !opponent.hasWorkingAbility(Abilities.OBLIVIOUS))
				{
					if (opponent.pbCanReduceStatStage(Stats.SPATK, attacker, false, this))
					{
						opponent.pbReduceStat(Stats.SPATK, 2, attacker, false, this);
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
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (pbIsDamaging()) return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (!opponent.pbCanReduceStatStage(Stats.SPDEF, attacker, true, this)) return -1;
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool ret = opponent.pbReduceStat(Stats.SPDEF, 2, attacker, false, this);
			return ret ? 0 : -1;
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			if (opponent.pbCanReduceStatStage(Stats.SPDEF, attacker, false, this))
			{
				opponent.pbReduceStat(Stats.SPDEF, 2, attacker, false, this);
			}
		}
	}

	/// <summary>
	/// Resets all target's stat stages to 0. (Clear Smog)
	/// <summary>
	public class PokeBattle_Move_050 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{

			object ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (opponent.damagestate.CalcDamage > 0 && !opponent.damagestate.Substitute)
			{
				opponent.stages[(byte)Stats.ATTACK] = 0;
				opponent.stages[(byte)Stats.DEFENSE] = 0;
				opponent.stages[(byte)Stats.SPEED] = 0;
				opponent.stages[(byte)Stats.SPATK] = 0;
				opponent.stages[(byte)Stats.SPDEF] = 0;
				opponent.stages[(byte)Stats.ACCURACY] = 0;
				opponent.stages[(byte)Stats.EVASION] = 0;

				//battle.pbDisplay(_INTL("{1}'s stat changes were removed!", opponent.pbThis))
			}
			return ret;
		}
	}

	/// <summary>
	/// Resets all stat stages for all battlers to 0. (Haze)
	/// <summary>
	public class PokeBattle_Move_051 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			for (int i = 0; i < 4; i++)
			{
				this.battle.battlers[i].stages[(byte)Stats.ATTACK] = 0;
				this.battle.battlers[i].stages[(byte)Stats.DEFENSE] = 0;
				this.battle.battlers[i].stages[(byte)Stats.SPEED] = 0;
				this.battle.battlers[i].stages[(byte)Stats.SPATK] = 0;
				this.battle.battlers[i].stages[(byte)Stats.SPDEF] = 0;
				this.battle.battlers[i].stages[(byte)Stats.ACCURACY] = 0;
				this.battle.battlers[i].stages[(byte)Stats.EVASION] = 0;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			//battle.pbDisplay(_INTL("All stat changes were eliminated!"))
			return 0;
		}
	}

	/// <summary>
	/// User and target swap their Attack and Special Attack stat stages. (Power Swap)
	/// <summary>
	public class PokeBattle_Move_052 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{

			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			int[] astage = attacker.stages;
			int[] ostage = opponent.stages;
			//ToDo: create temp variables then override source
			//astage[(byte)Stats.ATTACK],ostage[(byte)Stats.ATTACK]=ostage[(byte)Stats.ATTACK],astage[(byte)Stats.ATTACK]
			//astage[(byte)Stats.SPATK], ostage[(byte)Stats.SPATK] = ostage[(byte)Stats.SPATK], astage[(byte)Stats.SPATK]

			//battle.pbDisplay(_INTL("{1} switched all changes to its Attack and Sp. Atk with the target!",attacker.pbThis))
			return 0;
		}
	}

	/// <summary>
	/// User and target swap their Defense and Special Defense stat stages. (Guard Swap)
	/// <summary>
	public class PokeBattle_Move_053 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{

			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			int[] astage = attacker.stages;
			int[] ostage = opponent.stages;

			//ToDo: create temp variables then override source
			//astage[(byte)Stats.DEFENSE],ostage[(byte)Stats.DEFENSE]=ostage[(byte)Stats.DEFENSE],astage[(byte)Stats.DEFENSE]
			//astage[(byte)Stats.SPDEF], ostage[(byte)Stats.SPDEF] = ostage[(byte)Stats.SPDEF], astage[(byte)Stats.SPDEF]

			//battle.pbDisplay(_INTL("{1} switched all changes to its Defense and Sp. Def with the target!",attacker.pbThis))
			return 0;
		}
	}

	/// <summary>
	/// User and target swap all their stat stages. (Heart Swap)
	/// <summary>
	public class PokeBattle_Move_054 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{

			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);
			foreach (var i in new[] { Stats.ATTACK, Stats.DEFENSE, Stats.SPEED,
				  Stats.SPATK, Stats.SPDEF, Stats.ACCURACY, Stats.EVASION })
			{
				//ToDo: create temp variables then override source
				//attacker.stages[(byte)i],opponent.stages[(byte)i] = opponent.stages[(byte)i],attacker.stages[(byte)i];
			}

			//battle.pbDisplay(_INTL("{1} switched stat changes with the target!",attacker.pbThis))
			return 0;
		}
	}

	/// <summary>
	/// User copies the target's stat stages. (Psych Up)
	/// <summary>
	public class PokeBattle_Move_055 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (opponent.OwnSide.CraftyShield)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);
			foreach (var i in new[] { Stats.ATTACK, Stats.DEFENSE, Stats.SPEED,
				  Stats.SPATK, Stats.SPDEF, Stats.ACCURACY, Stats.EVASION })
			{
				attacker.stages[(byte)i] = opponent.stages[(byte)i];
			}

			//battle.pbDisplay(_INTL("{1} copied {2}'s stat changes!",attacker.pbThis,opponent.pbThis(true)))
			return 0;
		}
	}

	/// <summary>
	/// For 5 rounds, user's and ally's stat stages cannot be lowered by foes. (Mist)
	/// <summary>
	public class PokeBattle_Move_056 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.OwnSide.Mist > 0)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			attacker.OwnSide.Mist = 5;
			if (!this.battle.isOpposing(attacker.Index))
			{
				//battle.pbDisplay(_INTL("Your team became shrouded in mist!"))
			}
			else
			{
				//battle.pbDisplay(_INTL("The opposing team became shrouded in mist!"))
			}
			return 0;
		}
	}

	/// <summary>
	/// Swaps the user's Attack and Defense stats. (Power Trick)
	/// <summary>
	public class PokeBattle_Move_057 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{

			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			//ToDo: create temp variables then override source
			//attacker.attack,attacker.defense = attacker.defense,attacker.attack;
			attacker.effects.PowerTrick = !attacker.effects.PowerTrick;

			//battle.pbDisplay(_INTL("{1} switched its Attack and Defense!", attacker.pbThis))
			return 0;
		}
	}

	/// <summary>
	/// Averages the user's and target's Attack.
	/// Averages the user's and target's Special Attack. (Power Split)
	/// <summary>
	public class PokeBattle_Move_058 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (opponent.effects.Substitute > 0 && !ignoresSubstitute(attacker))
			{
				//battle.pbDisplay(_INTL("But it failed!"))  
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			int avatk = (int)Math.Floor((attacker.attack + opponent.attack) / 2f);
			int avspatk = (int)Math.Floor((attacker.spatk + opponent.spatk) / 2f);

			attacker.attack = opponent.attack = avatk;
			attacker.spatk = opponent.spatk = avspatk;

			//battle.pbDisplay(_INTL("{1} shared its power with the target!", attacker.pbThis))
			return 0;
		}
	}

	/// <summary>
	/// Averages the user's and target's Defense.
	/// Averages the user's and target's Special Defense. (Guard Split)
	/// <summary>
	public class PokeBattle_Move_059 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (opponent.effects.Substitute > 0 && !ignoresSubstitute(attacker))
			{
				//battle.pbDisplay(_INTL("But it failed!"))  
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			int avdef = (int)Math.Floor((attacker.defense + opponent.defense) / 2f);
			int avspdef = (int)Math.Floor((attacker.spdef + opponent.spdef) / 2f);

			attacker.defense = opponent.defense = avdef;
			attacker.spdef = opponent.spdef = avspdef;

			//battle.pbDisplay(_INTL("{1} shared its guard with the target!", attacker.pbThis))
			return 0;
		}
	}

	/// <summary>
	/// Averages the user's and target's current HP. (Pain Split)
	/// <summary>
	public class PokeBattle_Move_05A : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (opponent.effects.Substitute > 0 && !ignoresSubstitute(attacker))
			{
				//battle.pbDisplay(_INTL("But it failed!"))  
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			int olda = attacker.HP;
			int oldo = opponent.HP;

			int avhp = (int)Math.Floor((attacker.HP + opponent.HP) / 2f);
			attacker.HP = Math.Min(avhp, attacker.TotalHP);
			opponent.HP = Math.Min(avhp, opponent.TotalHP);

			this.battle.scene.pbHPChanged(attacker, olda);
			this.battle.scene.pbHPChanged(opponent, oldo);
			//battle.pbDisplay(_INTL("The battlers shared their pain!"))
			return 0;
		}
	}

	/// <summary>
	/// For 4 rounds, doubles the Speed of all battlers on the user's side. (Tailwind)
	/// <summary>
	public class PokeBattle_Move_05B : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.OwnSide.Tailwind > 0)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			attacker.OwnSide.Tailwind = 4;
			if (!this.battle.isOpposing(attacker.Index))
			{
				//battle.pbDisplay(_INTL("The tailwind blew from behind your team!"))
			}
			else
			{
				//battle.pbDisplay(_INTL("The tailwind blew from behind the opposing team!"))
			}
			return 0;
		}
	}

	/// <summary>
	/// This move turns into the last move used by the target, until user switches
	/// out. (Mimic)
	/// <summary>
	public class PokeBattle_Move_05C : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{

			List<Move.Effect> blacklist = new List<Move.Effect> {
		   (Move.Effect)0x02,   // Struggle
		   (Move.Effect)0x14,   // Chatter
		   (Move.Effect)0x5C,   // Mimic
		   (Move.Effect)0x5D,   // Sketch
		   (Move.Effect)0xB6    // Metronome
		};
			if (attacker.effects.Transform ||
			   opponent.lastMoveUsed <= 0 ||
			   new Move((Moves)opponent.lastMoveUsed).Type == Types.SHADOW ||
			   blacklist.Contains(new Move((Moves)opponent.lastMoveUsed).Function))
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			foreach (var i in attacker.moves)
			{
				if (i.MoveId == opponent.lastMoveUsed)
				{
					//battle.pbDisplay(_INTL("But it failed!"))
					return -1;
				}
			}

			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);
			for (int i = 0; i < attacker.moves.Length; i++)
			{
				if (attacker.moves[i].MoveId == this.id)
				{
					Move newmove = new Move(opponent.lastMoveUsed);
					attacker.moves[i] = new Battle.InBattleMove(//this.battle, 
						newmove);

					string movename = PBMoves.getName(opponent.lastMoveUsed);

					//battle.pbDisplay(_INTL("{1} learned {2}!", attacker.pbThis, movename))
					return 0;
				}
			}

			//battle.pbDisplay(_INTL("But it failed!"))
			return -1;
		}
	}

	/// <summary>
	/// This move permanently turns into the last move used by the target. (Sketch)
	/// <summary>
	public class PokeBattle_Move_05D : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{

			List<Move.Effect> blacklist = new List<Move.Effect> {
		   (Move.Effect)0x02,   // Struggle
		   (Move.Effect)0x14,   // Chatter
		   (Move.Effect)0x5D    // Sketch
		};
			if (attacker.effects.Transform ||
			   opponent.lastMoveUsedSketch <= 0 ||
			   new Move((Moves)opponent.lastMoveUsedSketch).Type == Types.SHADOW ||
			   blacklist.Contains(new Move((Moves)opponent.lastMoveUsedSketch).Function))
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			foreach (var i in attacker.moves)
			{
				if (i.MoveId == opponent.lastMoveUsedSketch)
				{
					//battle.pbDisplay(_INTL("But it failed!"))
					return -1;
				}
			}
			if (opponent.OwnSide.CraftyShield)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);
			for (int i = 0; i < attacker.moves.Length; i++)
			{
				if (attacker.moves[i].MoveId == this.id)
				{
					Moves newmove = opponent.lastMoveUsedSketch;
					attacker.moves[i] = new Battle.InBattleMove(//this.battle, 
						newmove);

					Battle.Battler[] party = this.battle.pbParty(attacker.Index);


					party[attacker.pokemonIndex].moves[i] = new Battle.InBattleMove(newmove);


					string movename = PBMoves.getName(opponent.lastMoveUsedSketch);

					//battle.pbDisplay(_INTL("{1} learned {2}!", attacker.pbThis, movename))
					return 0;
				}
			}

			//battle.pbDisplay(_INTL("But it failed!"))
			return -1;
		}
	}

	/// <summary>
	/// Changes user's type to that of a random user's move, except this one, OR the
	/// user's first move's type. (Conversion)
	/// <summary>
	public class PokeBattle_Move_05E : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.Ability == Abilities.MULTITYPE)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			List<Types> types = new List<Types>(); //[]
			foreach (var i in attacker.moves)
			{
				if (i.MoveId == this.id) continue; //next
				//if (PBTypes.isPseudoType(i.Type)) continue; //next
				if (attacker.pbHasType(i.Type)) continue; //next
				if (!types.Contains(i.Type))
				{
					types.Add(i.Type);
					if (Settings.USENEWBATTLEMECHANICS) break;
				}

			}
			if (types.Count == 0)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			Types newtype = types[this.battle.pbRandom(types.Count)];
			attacker.Type1 = newtype;
			attacker.Type2 = newtype;

			attacker.effects.Type3 = Types.NONE; //-1;

			string typename = PBTypes.getName(newtype);

			//battle.pbDisplay(_INTL("{1} transformed into the {2} type!", attacker.pbThis, typename))
			return null; //ToDo: Wasnt sure what to return, so put null
		}
	}

	/// <summary>
	/// Changes user's type to a random one that resists/is immune to the last move
	/// used by the target. (Conversion 2)
	/// <summary>
	public class PokeBattle_Move_05F : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.Ability == Abilities.MULTITYPE)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			if (opponent.lastMoveUsed <= 0 //||
				//PBTypes.isPseudoType(new Move ((Moves)opponent.lastMoveUsed).Type)
			   )
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			if (opponent.OwnSide.CraftyShield)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			List<Types> types = new List<Types>();//[]

			Types atype = opponent.lastMoveUsedType;
			if (atype < 0)
			{

				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			for (int i = 0; i < Enum.GetValues(typeof(Types)).Length; i++)
			{
				//if (PBTypes.isPseudoType(i)) continue; //next
				if (attacker.pbHasType((Types)i)) continue; //next
				if (PBTypes.getEffectiveness(atype, (Types)i) < 2) types.Add((Types)i);
			}
			if (types.Count == 0)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			Types newtype = types[this.battle.pbRandom(types.Count)];
			attacker.Type1 = newtype;
			attacker.Type2 = newtype;

			attacker.effects.Type3 = Types.NONE; //-1;

			string typename = PBTypes.getName(newtype);

			//battle.pbDisplay(_INTL("{1} transformed into the {2} type!", attacker.pbThis, typename))
			return 0;
		}
	}

	/// <summary>
	/// Changes user's type depending on the environment. (Camouflage)
	/// <summary>
	public class PokeBattle_Move_060 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.Ability == Abilities.MULTITYPE)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			Types type = Types.NORMAL;
			switch (this.battle.environment)
			{
				case PokemonUnity.Environment.None: type = Types.NORMAL; break;
				case PokemonUnity.Environment.Grass: type = Types.GRASS; break;
				case PokemonUnity.Environment.TallGrass: type = Types.GRASS; break;
				case PokemonUnity.Environment.MovingWater: type = Types.WATER; break;
				case PokemonUnity.Environment.StillWater: type = Types.WATER; break;
				case PokemonUnity.Environment.Underwater: type = Types.WATER; break;
				case PokemonUnity.Environment.Cave: type = Types.ROCK; break;
				case PokemonUnity.Environment.Rock: type = Types.GROUND; break;
				case PokemonUnity.Environment.Sand: type = Types.GROUND; break;
				case PokemonUnity.Environment.Forest: type = Types.BUG; break;
				case PokemonUnity.Environment.Snow: type = Types.ICE; break;
				case PokemonUnity.Environment.Volcano: type = Types.FIRE; break;
				case PokemonUnity.Environment.Graveyard: type = Types.GHOST; break;
				case PokemonUnity.Environment.Sky: type = Types.FLYING; break;
				case PokemonUnity.Environment.Space: type = Types.DRAGON; break;
				default: break;
			}
			if (this.battle.field.ElectricTerrain > 0)
			{
				type = Types.ELECTRIC;
			}
			else if (this.battle.field.GrassyTerrain > 0)
			{
				type = Types.GRASS;
			}
			else if (this.battle.field.MistyTerrain > 0)
			{
				type = Types.FAIRY;
			}
			if (attacker.pbHasType(type))
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			attacker.Type1 = type;
			attacker.Type2 = type;
			attacker.effects.Type3 = Types.NONE; //-1;

			string typename = PBTypes.getName(type);

			//battle.pbDisplay(_INTL("{1} transformed into the {2} type!", attacker.pbThis, typename))  
			return 0;
		}
	}

	/// <summary>
	/// Target becomes Water type. (Soak)
	/// <summary>
	public class PokeBattle_Move_061 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (opponent.effects.Substitute > 0 && !ignoresSubstitute(attacker))
			{
				//battle.pbDisplay(_INTL("But it failed!"))  
				return -1;
			}
			if (pbTypeImmunityByAbility(pbType(this.type, attacker, opponent), attacker, opponent)) return -1;
			if (opponent.Ability == Abilities.MULTITYPE)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);
			if (opponent.Type1 == Types.WATER &&
			   opponent.Type2 == Types.WATER &&
			   (opponent.effects.Type3 < 0 ||
			   opponent.effects.Type3 == Types.WATER))
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			opponent.Type1 = Types.WATER;

			opponent.Type2 = Types.WATER;

			opponent.effects.Type3 = Types.NONE; //-1;
			string typename = PBTypes.getName(Types.WATER);
			//battle.pbDisplay(_INTL("{1} transformed into the {2} type!",opponent.pbThis,typename))
			return 0;
		}
	}

	/// <summary>
	/// User copes target's types. (Reflect Type)
	/// <summary>
	public class PokeBattle_Move_062 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.Ability == Abilities.MULTITYPE)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			if (attacker.pbHasType(opponent.Type1) &&
			   attacker.pbHasType(opponent.Type2) &&
			   attacker.pbHasType(opponent.effects.Type3) &&
			   opponent.pbHasType(attacker.Type1) &&
			   opponent.pbHasType(attacker.Type2) &&
			   opponent.pbHasType(attacker.effects.Type3))
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			attacker.Type1 = opponent.Type1;
			attacker.Type2 = opponent.Type2;
			attacker.effects.Type3 = Types.NONE; //-1;

			//battle.pbDisplay(_INTL("{1}'s type changed to match {2}'s!", attacker.pbThis, opponent.pbThis(true)))
			return 0;
		}
	}

	/// <summary>
	/// Target's ability becomes Simple. (Simple Beam)
	/// <summary>
	public class PokeBattle_Move_063 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (opponent.effects.Substitute > 0 && !ignoresSubstitute(attacker))
			{
				//battle.pbDisplay(_INTL("But it failed!"))  
				return -1;
			}
			if (opponent.Ability == Abilities.MULTITYPE ||
			   opponent.Ability == Abilities.SIMPLE ||
			   opponent.Ability == Abilities.STANCE_CHANGE ||
			   opponent.Ability == Abilities.TRUANT)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			Abilities oldabil = opponent.Ability;
			opponent.Ability = Abilities.SIMPLE;
			string abilityname = PBAbilities.getName(Abilities.SIMPLE);
			//battle.pbDisplay(_INTL("{1} acquired {2}!",opponent.pbThis,abilityname))
			if (opponent.effects.Illusion.Species != Pokemons.NONE && oldabil == Abilities.ILLUSION)
			{
				//PBDebug.log("[Ability triggered] #{opponent.pbThis}'s Illusion ended")    
				opponent.effects.Illusion = null;
				this.battle.scene.pbChangePokemon(opponent, opponent.Species);

				//battle.pbDisplay(_INTL("{1}'s {2} wore off!",opponent.pbThis,PBAbilities.getName(oldabil)))
			}
			return 0;
		}
	}

	/// <summary>
	/// Target's ability becomes Insomnia. (Worry Seed)
	/// <summary>
	public class PokeBattle_Move_064 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (opponent.effects.Substitute > 0 && !ignoresSubstitute(attacker))
			{
				//battle.pbDisplay(_INTL("But it failed!"))  
				return -1;
			}
			if (pbTypeImmunityByAbility(pbType(this.type, attacker, opponent), attacker, opponent)) return -1;
			if (opponent.Ability == Abilities.MULTITYPE ||
			   opponent.Ability == Abilities.INSOMNIA ||
			   opponent.Ability == Abilities.STANCE_CHANGE ||
			   opponent.Ability == Abilities.TRUANT)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			Abilities oldabil = opponent.Ability;
			opponent.Ability = Abilities.INSOMNIA;
			string abilityname = PBAbilities.getName(Abilities.INSOMNIA);
			//battle.pbDisplay(_INTL("{1} acquired {2}!",opponent.pbThis,abilityname))
			if (opponent.effects.Illusion.Species != Pokemons.NONE && oldabil == Abilities.ILLUSION)
			{
				//PBDebug.log("[Ability triggered] #{opponent.pbThis}'s Illusion ended")    
				opponent.effects.Illusion = null;
				this.battle.scene.pbChangePokemon(opponent, opponent.Species);

				//battle.pbDisplay(_INTL("{1}'s {2} wore off!",opponent.pbThis,PBAbilities.getName(oldabil)))
			}
			return 0;
		}
	}

	/// <summary>
	/// User copies target's ability. (Role Play)
	/// <summary>
	public class PokeBattle_Move_065 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (opponent.OwnSide.CraftyShield)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			if (opponent.Ability == 0 ||
			   attacker.Ability == opponent.Ability ||
			   attacker.Ability == Abilities.MULTITYPE ||
			   attacker.Ability == Abilities.STANCE_CHANGE ||
			   opponent.Ability == Abilities.FLOWER_GIFT ||
			   opponent.Ability == Abilities.FORECAST ||
			   opponent.Ability == Abilities.ILLUSION ||
			   opponent.Ability == Abilities.IMPOSTER ||
			   opponent.Ability == Abilities.MULTITYPE ||
			   opponent.Ability == Abilities.STANCE_CHANGE ||
			   opponent.Ability == Abilities.TRACE ||
			   opponent.Ability == Abilities.WONDER_GUARD ||
			   opponent.Ability == Abilities.ZEN_MODE)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			Abilities oldabil = attacker.Ability;
			attacker.Ability = opponent.Ability;
			string abilityname = PBAbilities.getName(opponent.Ability);

			//battle.pbDisplay(_INTL("{1} copied {2}'s {3}!", attacker.pbThis, opponent.pbThis(true),abilityname))
			if (attacker.effects.Illusion.Species != Pokemons.NONE && oldabil == Abilities.ILLUSION)
			{
				//PBDebug.log("[Ability triggered] #{attacker.pbThis}'s Illusion ended")    
				attacker.effects.Illusion = null;
				this.battle.scene.pbChangePokemon(attacker, attacker.Species);

				//battle.pbDisplay(_INTL("{1}'s {2} wore off!",attacker.pbThis,PBAbilities.getName(oldabil)))
			}
			return 0;
		}
	}

	/// <summary>
	/// Target copies user's ability. (Entrainment)
	/// <summary>
	public class PokeBattle_Move_066 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (opponent.effects.Substitute > 0 && !ignoresSubstitute(attacker))
			{
				//battle.pbDisplay(_INTL("But it failed!"))  
				return -1;
			}
			if (opponent.OwnSide.CraftyShield)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			if (attacker.Ability == 0 ||
			   attacker.Ability == opponent.Ability ||
			   opponent.Ability == Abilities.FLOWER_GIFT ||
			   opponent.Ability == Abilities.IMPOSTER ||
			   opponent.Ability == Abilities.MULTITYPE ||
			   opponent.Ability == Abilities.STANCE_CHANGE ||
			   opponent.Ability == Abilities.TRACE ||
			   opponent.Ability == Abilities.TRUANT ||
			   opponent.Ability == Abilities.ZEN_MODE ||
			   attacker.Ability == Abilities.FLOWER_GIFT ||
			   attacker.Ability == Abilities.FORECAST ||
			   attacker.Ability == Abilities.ILLUSION ||
			   attacker.Ability == Abilities.IMPOSTER ||
			   attacker.Ability == Abilities.MULTITYPE ||
			   attacker.Ability == Abilities.STANCE_CHANGE ||
			   attacker.Ability == Abilities.TRACE ||
			   attacker.Ability == Abilities.ZEN_MODE)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			Abilities oldabil = opponent.Ability;
			opponent.Ability = attacker.Ability;
			string abilityname = PBAbilities.getName(attacker.Ability);

			//battle.pbDisplay(_INTL("{1} acquired {2}!", opponent.pbThis, abilityname))
			if (opponent.effects.Illusion.Species != Pokemons.NONE && oldabil == Abilities.ILLUSION)
			{
				//PBDebug.log("[Ability triggered] #{opponent.pbThis}'s Illusion ended")    
				opponent.effects.Illusion = null;
				this.battle.scene.pbChangePokemon(opponent, opponent.Species);

				//battle.pbDisplay(_INTL("{1}'s {2} wore off!",opponent.pbThis,PBAbilities.getName(oldabil)))
			}
			return 0;
		}
	}

	/// <summary>
	/// User and target swap abilities. (Skill Swap)
	/// <summary>
	public class PokeBattle_Move_067 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if ((attacker.Ability == 0 && opponent.Ability == 0) ||
			   (attacker.Ability == opponent.Ability && !Settings.USENEWBATTLEMECHANICS) ||
			   attacker.Ability == Abilities.ILLUSION ||
			   opponent.Ability == Abilities.ILLUSION ||
			   attacker.Ability == Abilities.MULTITYPE ||
			   opponent.Ability == Abilities.MULTITYPE ||
			   attacker.Ability == Abilities.STANCE_CHANGE ||
			   opponent.Ability == Abilities.STANCE_CHANGE ||
			   attacker.Ability == Abilities.WONDER_GUARD ||
			   opponent.Ability == Abilities.WONDER_GUARD)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			Abilities tmp = attacker.Ability;
			attacker.Ability = opponent.Ability;
			opponent.Ability = tmp;

			//battle.pbDisplay(_INTL("{1} swapped its {2} Ability with its target's {3} Ability!",
			//   attacker.pbThis, PBAbilities.getName(opponent.Ability),
			//   PBAbilities.getName(attacker.Ability)))
			attacker.pbAbilitiesOnSwitchIn(true);
			opponent.pbAbilitiesOnSwitchIn(true);
			return 0;
		}
	}

	/// <summary>
	/// Target's ability is negated. (Gastro Acid)
	/// <summary>
	public class PokeBattle_Move_068 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (opponent.effects.Substitute > 0 && !ignoresSubstitute(attacker))
			{
				//battle.pbDisplay(_INTL("But it failed!"))  
				return -1;
			}
			if (opponent.Ability == Abilities.MULTITYPE ||
			   opponent.Ability == Abilities.STANCE_CHANGE)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			Abilities oldabil = opponent.Ability;
			opponent.effects.GastroAcid = true;

			opponent.effects.Truant = false;

			//battle.pbDisplay(_INTL("{1}'s Ability was suppressed!", opponent.pbThis)) 
			if (opponent.effects.Illusion.Species != Pokemons.NONE && oldabil == Abilities.ILLUSION)
			{
				//PBDebug.log("[Ability triggered] #{opponent.pbThis}'s Illusion ended")    
				opponent.effects.Illusion = null;
				this.battle.scene.pbChangePokemon(opponent, opponent.Species);

				//battle.pbDisplay(_INTL("{1}'s {2} wore off!",opponent.pbThis,PBAbilities.getName(oldabil)))
			}
			return 0;
		}
	}

	/// <summary>
	/// User transforms into the target. (Transform)
	/// <summary>
	public class PokeBattle_Move_069 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{

			List<Move.Effect> blacklist = new List<Move.Effect>{
		   (Move.Effect)0xC9,   // Fly
		   (Move.Effect)0xCA,   // Dig
		   (Move.Effect)0xCB,   // Dive
		   (Move.Effect)0xCC,   // Bounce
		   (Move.Effect)0xCD,   // Shadow Force
		   (Move.Effect)0xCE,   // Sky Drop
		   (Move.Effect)0x14D   // Phantom Force
		};
			if (attacker.effects.Transform ||
			   opponent.effects.Transform ||
			   opponent.effects.Illusion.Species != Pokemons.NONE ||
			   (opponent.effects.Substitute > 0 && !ignoresSubstitute(attacker)) ||
			   opponent.effects.SkyDrop ||
			   blacklist.Contains(new Move((Moves)opponent.effects.TwoTurnAttack).Function))
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			if (opponent.OwnSide.CraftyShield)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			attacker.effects.Transform = true;
			attacker.Type1 = opponent.Type1;
			attacker.Type2 = opponent.Type2;
			attacker.effects.Type3 = Types.NONE; //-1;

			attacker.Ability = opponent.Ability;

			attacker.attack = opponent.attack;

			attacker.defense = opponent.defense;

			attacker.speed = opponent.speed;

			attacker.spatk = opponent.spatk;

			attacker.spdef = opponent.spdef;
			foreach (var i in new[] { Stats.ATTACK, Stats.DEFENSE, Stats.SPEED,
				  Stats.SPATK, Stats.SPDEF, Stats.ACCURACY, Stats.EVASION })
			{
				attacker.stages[(byte)i] = opponent.stages[(byte)i];

			}
			for (int i = 0; i < 4; i++)
			{

				attacker.moves[i] = new Battle.InBattleMove(//this.battle,
				   opponent.moves[i].MoveId);

				attacker.moves[i].PP = 5;
				attacker.moves[i].TotalPP = 5;
			}
			attacker.effects.Disable = 0;

			attacker.effects.DisableMove = 0;

			//battle.pbDisplay(_INTL("{1} transformed into {2}!", attacker.pbThis, opponent.pbThis(true)))
			return 0;
		}
	}

	/// <summary>
	/// Inflicts a fixed 20HP damage. (SonicBoom)
	/// <summary>
	public class PokeBattle_Move_06A : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			return pbEffectFixedDamage(20, attacker, opponent, hitnum, alltargets, showanimation);
		}
	}

	/// <summary>
	/// Inflicts a fixed 40HP damage. (Dragon Rage)
	/// <summary>
	public class PokeBattle_Move_06B : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			return pbEffectFixedDamage(40, attacker, opponent, hitnum, alltargets, showanimation);
		}
	}

	/// <summary>
	/// Halves the target's current HP. (Super Fang)
	/// <summary>
	public class PokeBattle_Move_06C : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			return pbEffectFixedDamage((int)Math.Max(Math.Floor(opponent.HP / 2f), 1), attacker, opponent, hitnum, alltargets, showanimation);
		}
	}

	/// <summary>
	/// Inflicts damage equal to the user's level. (Night Shade, Seismic Toss)
	/// <summary>
	public class PokeBattle_Move_06D : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			return pbEffectFixedDamage(attacker.level, attacker, opponent, hitnum, alltargets, showanimation);
		}
	}

	/// <summary>
	/// Inflicts damage to bring the target's HP down to equal the user's HP. (Endeavor)
	/// <summary>
	public class PokeBattle_Move_06E : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.HP >= opponent.HP)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			return pbEffectFixedDamage(opponent.HP - attacker.HP, attacker, opponent, hitnum, alltargets, showanimation);
		}
	}

	/// <summary>
	/// Inflicts damage between 0.5 and 1.5 times the user's level. (Psywave)
	/// <summary>
	public class PokeBattle_Move_06F : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{

			int dmg = (int)Math.Max((attacker.level * (int)Math.Floor(this.battle.pbRandom(101) + 50f) / 100f), 1);
			return pbEffectFixedDamage(dmg, attacker, opponent, hitnum, alltargets, showanimation);
		}
	}

	/// <summary>
	/// OHKO. Accuracy increases by difference between levels of user and target.
	/// <summary>
	public class PokeBattle_Move_070 : PokeBattle_Move
	{
		public override object pbAccuracyCheck(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (!attacker.hasMoldBreaker() && opponent.hasWorkingAbility(Abilities.STURDY))
			{
				//battle.pbDisplay(_INTL("{1} was protected by {2}!",opponent.pbThis,PBAbilities.getName(opponent.Ability)))  
				return false;
			}
			if (opponent.level > attacker.level)
			{
				//battle.pbDisplay(_INTL("{1} is unaffected!", opponent.pbThis))
				return false;
			}
			int acc = this.accuracy + attacker.level - opponent.level;
			return this.battle.pbRandom(100) < acc;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{

			int damage = pbEffectFixedDamage(opponent.TotalHP, attacker, opponent, hitnum, alltargets, showanimation);
			if (opponent.isFainted())
			{
				//battle.pbDisplay(_INTL("It's a one-hit KO!"))
			}
			return damage;
		}
	}

	/// <summary>
	/// Counters a physical move used against the user this round, with 2x the power. (Counter)
	/// <summary>
	public class PokeBattle_Move_071 : PokeBattle_Move
	{
		public void pbAddTarget(byte targets, Battle.Battler attacker)
		{
			if (attacker.effects.CounterTarget >= 0 &&
			   attacker.IsOpposing(attacker.effects.CounterTarget))
			{
				if (!attacker.pbAddTarget(targets, this.battle.battlers[attacker.effects.CounterTarget]))
				{
					attacker.pbRandomTarget(targets);

				}
			}
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.effects.Counter < 0 || opponent.Species == Pokemons.NONE)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			object ret = pbEffectFixedDamage(Math.Max(attacker.effects.Counter * 2, 1), attacker, opponent, hitnum, alltargets, showanimation);
			return ret;
		}
	}

	/// <summary>
	/// Counters a specical move used against the user this round, with 2x the power. (Mirror Coat)
	/// <summary>
	public class PokeBattle_Move_072 : PokeBattle_Move
	{
		public void pbAddTarget(byte targets, Battle.Battler attacker)
		{
			if (attacker.effects.MirrorCoatTarget >= 0 &&
			   attacker.IsOpposing(attacker.effects.MirrorCoatTarget))
			{
				if (!attacker.pbAddTarget(targets, this.battle.battlers[attacker.effects.MirrorCoatTarget]))
				{
					attacker.pbRandomTarget(targets);

				}
			}
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.effects.MirrorCoat < 0 || opponent.Species == Pokemons.NONE)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			object ret = pbEffectFixedDamage(Math.Max(attacker.effects.MirrorCoat * 2, 1), attacker, opponent, hitnum, alltargets, showanimation);
			return ret;
		}
	}

	/// <summary>
	/// Counters the last damaging move used against the user this round, with 1.5x
	/// the power. (Metal Burst)
	/// <summary>
	public class PokeBattle_Move_073 : PokeBattle_Move
	{
		public void pbAddTarget(byte targets, Battle.Battler attacker)
		{
			if (attacker.lastAttacker.Count > 0)
			{
				byte lastattacker = attacker.lastAttacker[attacker.lastAttacker.Count - 1];
				if (lastattacker >= 0 && attacker.IsOpposing(lastattacker))
				{
					if (!attacker.pbAddTarget(targets, this.battle.battlers[lastattacker]))
					{
						attacker.pbRandomTarget(targets);

					}
				}

			}
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.lastHPLost == 0 || opponent.Species == Pokemons.NONE)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			object ret = pbEffectFixedDamage((int)Math.Max(Math.Floor(attacker.lastHPLost * 1.5f), 1), attacker, opponent, hitnum, alltargets, showanimation);
			return ret;
		}
	}

	/// <summary>
	/// The target's ally loses 1/16 of its max HP. (Flame Burst)
	/// <summary>
	public class PokeBattle_Move_074 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{

			object ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (opponent.damagestate.CalcDamage > 0)
			{
				if (opponent.Partner.Species != Pokemons.NONE && !opponent.Partner.isFainted() &&
				   !opponent.Partner.hasWorkingAbility(Abilities.MAGIC_GUARD))
				{
					opponent.Partner.ReduceHP((int)Math.Floor(opponent.Partner.TotalHP / 16f));
					//battle.pbDisplay(_INTL("The bursting flame hit {1}!",opponent.Partner.pbThis(true)))
				}
			}
			return ret;
		}
	}

	/// <summary>
	/// Power is doubled if the target is using Dive. (Surf)
	/// (Handled in Battler's pbSuccessCheck): Hits some semi-invulnerable targets.
	/// <summary>
	public class PokeBattle_Move_075 : PokeBattle_Move
	{
		public object pbModifyDamage(int damagemult, Battle.Battler attacker, Battle.Battler opponent)
		{
			if ((int)new Move((Moves)opponent.effects.TwoTurnAttack).Function == 0xCB)	// Dive
			{ 
				return (int)Math.Round(damagemult * 2.0f);
			}
			return damagemult;
		}
	}

	/// <summary>
	/// Power is doubled if the target is using Dig. Power is halved if Grassy Terrain
	/// is in effect. (Earthquake)
	/// (Handled in Battler's pbSuccessCheck): Hits some semi-invulnerable targets.
	/// <summary>
	public class PokeBattle_Move_076 : PokeBattle_Move
	{
		public object pbModifyDamage(int damagemult, Battle.Battler attacker, Battle.Battler opponent)
		{

			object ret = damagemult;
			if ((int)new Move((Moves)opponent.effects.TwoTurnAttack).Function == 0xCA)	// Dig
			{ 
				ret = (int)Math.Round(damagemult * 2.0f);
			}
			if (this.battle.field.GrassyTerrain > 0)
			{
				ret = (int)Math.Round(damagemult / 2.0f);
			}
			return ret;
		}
	}

	/// <summary>
	/// Power is doubled if the target is using Bounce, Fly or Sky Drop. (Gust)
	/// (Handled in Battler's pbSuccessCheck): Hits some semi-invulnerable targets.
	/// <summary>
	public class PokeBattle_Move_077 : PokeBattle_Move
	{
		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{
			if ((int)new Move((Moves)opponent.effects.TwoTurnAttack).Function == 0xC9 || // Fly
			   (int)new Move((Moves)opponent.effects.TwoTurnAttack).Function == 0xCC || // Bounce
			   (int)new Move((Moves)opponent.effects.TwoTurnAttack).Function == 0xCE || // Sky Drop
			   opponent.effects.SkyDrop)
			{
				return basedmg * 2;
			}
			return basedmg;
		}
	}

	/// <summary>
	/// Power is doubled if the target is using Bounce, Fly or Sky Drop. (Twister)
	/// May make the target flinch.
	/// (Handled in Battler's pbSuccessCheck): Hits some semi-invulnerable targets.
	/// <summary>
	public class PokeBattle_Move_078 : PokeBattle_Move
	{
		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{
			if ((int)new Move((Moves)opponent.effects.TwoTurnAttack).Function == 0xC9 || // Fly
			   (int)new Move((Moves)opponent.effects.TwoTurnAttack).Function == 0xCC || // Bounce
			   (int)new Move((Moves)opponent.effects.TwoTurnAttack).Function == 0xCE || // Sky Drop
			   opponent.effects.SkyDrop)
			{
				return basedmg * 2;
			}
			return basedmg;
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			opponent.pbFlinch(attacker);
		}
	}

	/// <summary>
	/// Power is doubled if Fusion Flare has already been used this round. (Fusion Bolt)
	/// <summary>
	public class PokeBattle_Move_079 : PokeBattle_Move
	{
		public object pbBaseDamageMultiplier(int damagemult, Battle.Battler attacker, Battle.Battler opponent)
		{
			if (this.battle.field.FusionBolt)
			{
				this.battle.field.FusionBolt = false;

				this.doubled = true;
				return (int)Math.Round(damagemult * 2.0f);
			}
			return damagemult;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			this.doubled = false;
			object ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (opponent.damagestate.CalcDamage > 0)
			{
				this.battle.field.FusionFlare = true;
			}
			return ret;
		}

		public override IEnumerator pbShowAnimation(Moves id, Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (opponent.damagestate.Critical || this.doubled)
			{
				return base.pbShowAnimation(id, attacker, opponent, 1, alltargets, showanimation); // Charged anim;
			}
			return base.pbShowAnimation(id, attacker, opponent, hitnum, alltargets, showanimation);
		}
	}

	/// <summary>
	/// Power is doubled if Fusion Bolt has already been used this round. (Fusion Flare)
	/// <summary>
	public class PokeBattle_Move_07A : PokeBattle_Move
	{
		public object pbBaseDamageMultiplier(int damagemult, Battle.Battler attacker, Battle.Battler opponent)
		{
			if (battle.field.FusionFlare)
			{
				this.battle.field.FusionFlare = false;
				return (int)Math.Round(damagemult * 2.0f);
			}
			return damagemult;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{

			object ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (opponent.damagestate.CalcDamage > 0)
			{
				this.battle.field.FusionBolt = true;
			}
			return ret;
		}

		public override IEnumerator pbShowAnimation(Moves id, Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (opponent.damagestate.Critical || this.doubled)
			{
				return base.pbShowAnimation(id, attacker, opponent, 1, alltargets, showanimation); // Charged anim;
			}
			return base.pbShowAnimation(id, attacker, opponent, hitnum, alltargets, showanimation);
		}
	}

	/// <summary>
	/// Power is doubled if the target is poisoned. (Venoshock)
	/// <summary>
	public class PokeBattle_Move_07B : PokeBattle_Move
	{
		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.Status == Status.POISON &&
			   (opponent.effects.Substitute == 0 || ignoresSubstitute(attacker)))
			{
				return basedmg * 2;
			}
			return basedmg;
		}
	}

	/// <summary>
	/// Power is doubled if the target is paralyzed. Cures the target of paralysis.
	/// (SmellingSalt)
	/// <summary>
	public class PokeBattle_Move_07C : PokeBattle_Move
	{
		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.Status == Status.PARALYSIS &&
			   (opponent.effects.Substitute == 0 || ignoresSubstitute(attacker)))
			{
				return basedmg * 2;
			}
			return basedmg;
		}

		public void pbEffectAfterHit(Battle.Battler attacker, Battle.Battler opponent, Effects.Move turneffects)
		{
			if (!opponent.isFainted() && opponent.damagestate.CalcDamage > 0 &&
			   !opponent.damagestate.Substitute && opponent.Status == Status.PARALYSIS)
			{
				opponent.pbCureStatus();

			}
		}
	}

	/// <summary>
	/// Power is doubled if the target is asleep. Wakes the target up. (Wake-Up Slap)
	/// <summary>
	public class PokeBattle_Move_07D : PokeBattle_Move
	{
		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.Status == Status.SLEEP &&
			   (opponent.effects.Substitute == 0 || ignoresSubstitute(attacker)))
			{
				return basedmg * 2;
			}
			return basedmg;
		}

		public void pbEffectAfterHit(Battle.Battler attacker, Battle.Battler opponent, Effects.Move turneffects)
		{
			if (!opponent.isFainted() && opponent.damagestate.CalcDamage > 0 &&
			   !opponent.damagestate.Substitute && opponent.Status == Status.SLEEP)
			{
				opponent.pbCureStatus();

			}
		}
	}

	/// <summary>
	/// Power is doubled if the user is burned, poisoned or paralyzed. (Facade)
	/// <summary>
	public class PokeBattle_Move_07E : PokeBattle_Move
	{
		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{
			if (attacker.Status == Status.POISON ||
			   attacker.Status == Status.BURN ||
			   attacker.Status == Status.PARALYSIS)
			{
				return basedmg * 2;
			}
			return basedmg;
		}
	}

	/// <summary>
	/// Power is doubled if the target has a status problem. (Hex)
	/// <summary>
	public class PokeBattle_Move_07F : PokeBattle_Move
	{
		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.Status > 0 &&
			   (opponent.effects.Substitute == 0 || ignoresSubstitute(attacker)))
			{
				return basedmg * 2;
			}
			return basedmg;
		}
	}

	/// <summary>
	/// Power is doubled if the target's HP is down to 1/2 or less. (Brine)
	/// <summary>
	public class PokeBattle_Move_080 : PokeBattle_Move
	{
		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.HP <= opponent.TotalHP / 2)
			{
				return basedmg * 2;
			}
			return basedmg;
		}
	}

	/// <summary>
	/// Power is doubled if the user has lost HP due to the target's move this round.
	/// (Revenge, Avalanche)
	/// <summary>
	public class PokeBattle_Move_081 : PokeBattle_Move
	{
		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{
			if (attacker.lastHPLost > 0 && attacker.lastAttacker.Contains((byte)opponent.Index))
			{
				return basedmg * 2;
			}
			return basedmg;
		}
	}

	/// <summary>
	/// Power is doubled if the target has already lost HP this round. (Assurance)
	/// <summary>
	public class PokeBattle_Move_082 : PokeBattle_Move
	{
		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.tookDamage)
			{
				return basedmg * 2;
			}
			return basedmg;
		}
	}

	/// <summary>
	/// Power is doubled if a user's ally has already used this move this round. (Round)
	/// If an ally is about to use the same move, make it go next, ignoring priority.
	/// <summary>
	public class PokeBattle_Move_083 : PokeBattle_Move
	{
		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{

			int ret = basedmg;
			for (int i = 0; i <= attacker.OwnSide.Round; i++)
			{

				ret *= 2;

			}
			return ret;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{

			object ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (opponent.damagestate.CalcDamage > 0)
			{
				attacker.OwnSide.Round += 1;
				if (attacker.Partner.Species != Pokemons.NONE && !attacker.Partner.hasMovedThisRound())
				{
					if ((int)this.battle.choices[attacker.Partner.Index].Action == 1)	// Will use a move
					{ 
						Battle.InBattleMove partnermove = this.battle.choices[attacker.Partner.Index].Move;
						if (partnermove.Function == this.function)
						{
							attacker.Partner.effects.MoveNext = true;

							attacker.Partner.effects.Quash = false;

						}
					}

				}
			}
			return ret;
		}
	}

	/// <summary>
	/// Power is doubled if the target has already moved this round. (Payback)
	/// <summary>
	public class PokeBattle_Move_084 : PokeBattle_Move
	{
		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{
			if ((int)this.battle.choices[opponent.Index].Action != 1 || // Didn't choose a move
			   opponent.hasMovedThisRound())	// Used a move already
			{ 
				return basedmg * 2;
			}
			return basedmg;
		}
	}

	/// <summary>
	/// Power is doubled if a user's teammate fainted last round. (Retaliate)
	/// <summary>
	public class PokeBattle_Move_085 : PokeBattle_Move
	{
		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{
			if (attacker.OwnSide.LastRoundFainted >= 0 &&
			   attacker.OwnSide.LastRoundFainted == this.battle.turncount - 1)
			{
				return basedmg * 2;
			}
			return basedmg;
		}
	}

	/// <summary>
	/// Power is doubled if the user has no held item. (Acrobatics)
	/// <summary>
	public class PokeBattle_Move_086 : PokeBattle_Move
	{
		public object pbBaseDamageMultiplier(int damagemult, Battle.Battler attacker, Battle.Battler opponent)
		{
			if (attacker.Item == 0)
			{
				return (int)Math.Round(damagemult * 2.0f);
			}
			return damagemult;
		}
	}

	/// <summary>
	/// Power is doubled in weather. Type changes depending on the weather. (Weather Ball)
	/// <summary>
	public class PokeBattle_Move_087 : PokeBattle_Move
	{
		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{
			if (this.battle.Weather != 0)
			{
				return basedmg * 2;
			}
			return basedmg;
		}

		public override Types pbModifyType(Types type, Battle.Battler attacker, Battle.Battler opponent)
		{

			type = Types.NORMAL;
			switch (this.battle.Weather)
			{
				case Weather.SUNNYDAY:
				case Weather.HARSHSUN:
					type = (Types.FIRE); break;

				case Weather.RAINDANCE:
				case Weather.HEAVYRAIN:
					type = (Types.WATER); break;

				case Weather.SANDSTORM:
					type = (Types.ROCK); break;

				case Weather.HAIL:
					type = (Types.ICE); break;
				default: break;
			}
			return type;
		}
	}

	/// <summary>
	/// Power is doubled if a foe tries to switch out or use U-turn/Volt Switch/
	/// Parting Shot. (Pursuit)
	/// (Handled in Battle's pbAttackPhase): Makes this attack happen before switching.
	/// <summary>
	public class PokeBattle_Move_088 : PokeBattle_Move
	{
		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{
			if (this.battle.switching)
			{
				return basedmg * 2;
			}
			return basedmg;
		}

		public override object pbAccuracyCheck(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (this.battle.switching) return true;
			return base.pbAccuracyCheck(attacker, opponent);
		}
	}

	/// <summary>
	/// Power increases with the user's happiness. (Return)
	/// <summary>
	public class PokeBattle_Move_089 : PokeBattle_Move
	{
		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{
			return Math.Max(Math.Floor(attacker.happiness * 2 / 5f), 1);
		}
	}

	/// <summary>
	/// Power decreases with the user's happiness. (Frustration)
	/// <summary>
	public class PokeBattle_Move_08A : PokeBattle_Move
	{
		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{
			return Math.Max(Math.Floor((255 - attacker.happiness) * 2f / 5f), 1);
		}
	}

	/// <summary>
	/// Power increases with the user's HP. (Eruption, Water Spout)
	/// <summary>
	public class PokeBattle_Move_08B : PokeBattle_Move
	{
		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{
			return Math.Max(Math.Floor(150f * attacker.HP / attacker.TotalHP), 1);
		}
	}

	/// <summary>
	/// Power increases with the target's HP. (Crush Grip, Wring Out)
	/// <summary>
	public class PokeBattle_Move_08C : PokeBattle_Move
	{
		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{
			return Math.Max(Math.Floor(120f * opponent.HP / opponent.TotalHP), 1);
		}
	}

	/// <summary>
	/// Power increases the quicker the target is than the user. (Gyro Ball)
	/// <summary>
	public class PokeBattle_Move_08D : PokeBattle_Move
	{
		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{
			return Math.Max(Math.Min(Math.Floor(25f * opponent.pbSpeed / attacker.pbSpeed), 150), 1);
		}
	}

	/// <summary>
	/// Power increases with the user's positive stat changes (ignores negative ones).
	/// (Stored Power)
	/// <summary>
	public class PokeBattle_Move_08E : PokeBattle_Move
	{
		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{

			int mult = 1;
			foreach (Stats i in new[] { Stats.ATTACK, Stats.DEFENSE, Stats.SPEED,
				  Stats.SPATK, Stats.SPDEF, Stats.ACCURACY, Stats.EVASION })
			{
				if (attacker.stages[(byte)i] > 0) mult += attacker.stages[(byte)i];
			}
			return 20 * mult;
		}
	}

	/// <summary>
	/// Power increases with the target's positive stat changes (ignores negative ones).
	/// (Punishment)
	/// <summary>
	public class PokeBattle_Move_08F : PokeBattle_Move
	{
		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{
			int mult = 3;
			foreach (var i in new[] { Stats.ATTACK, Stats.DEFENSE, Stats.SPEED,
					  Stats.SPATK, Stats.SPDEF, Stats.ACCURACY, Stats.EVASION })
			{
				if (opponent.stages[(byte)i] > 0) mult += opponent.stages[(byte)i];
			}
			return Math.Min(20 * mult, 200);
		}
	}

	/// <summary>
	/// Power and type depends on the user's IVs. (Hidden Power)
	/// <summary>
	public class PokeBattle_Move_090 : PokeBattle_Move
	{
		public override Types pbModifyType(Types type, Battle.Battler attacker, Battle.Battler opponent)
		{
			int[] hp = pbHiddenPower(attacker.IV);

			type = (Types)hp[0];
			return type;
		}

		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{
			if (Settings.USENEWBATTLEMECHANICS) return 60;
			int[] hp = pbHiddenPower(attacker.IV);
			return hp[1];
		}
	}

	public static int[] pbHiddenPower(byte[] iv)
	{
		byte powermin = 30;
		byte powermax = 70;
		int type = 0; int baseY = 0;
		List<Types> types = new List<Types>();
		for (int i = 0; i < Enum.GetValues(typeof(Types)).Length; i++)
		{
			if (//!PBTypes.isPseudoType(i) &&
				(Types)i == Types.NORMAL && (Types)i == Types.SHADOW) types.Add((Types)i);
		}
		type |= (iv[(byte)Stats.HP] & 1);
		type |= (iv[(byte)Stats.ATTACK] & 1) << 1;
		type |= (iv[(byte)Stats.DEFENSE] & 1) << 2;
		type |= (iv[(byte)Stats.SPEED] & 1) << 3;
		type |= (iv[(byte)Stats.SPATK] & 1) << 4;
		type |= (iv[(byte)Stats.SPDEF] & 1) << 5;
		type = (int)Math.Floor(type * (types.Count - 1f) / 63f);
		Types hptype = types[type];
		baseY |= (iv[(byte)Stats.HP] & 2) >> 1;
		baseY |= (iv[(byte)Stats.ATTACK] & 2);
		baseY |= (iv[(byte)Stats.DEFENSE] & 2) << 1;
		baseY |= (iv[(byte)Stats.SPEED] & 2) << 2;
		baseY |= (iv[(byte)Stats.SPATK] & 2) << 3;
		baseY |= (iv[(byte)Stats.SPDEF] & 2) << 4;
		baseY = (int)Math.Floor(baseY * (powermax - powermin) / 63f) + powermin;
		return new int[] { (int)hptype, baseY }; //return type, and power
	}

	/// <summary>
	/// Power doubles for each consecutive use. (Fury Cutter)
	/// <summary>
	public class PokeBattle_Move_091 : PokeBattle_Move
	{
		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{

			basedmg = basedmg << (attacker.effects.FuryCutter - 1); // can be 1 to 4
			return basedmg;
		}
	}

	/// <summary>
	/// Power is multiplied by the number of consecutive rounds in which this move was
	/// used by any Pokémon on the user's side. (Echoed Voice)
	/// <summary>
	public class PokeBattle_Move_092 : PokeBattle_Move
	{
		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{

			basedmg *= attacker.OwnSide.EchoedVoiceCounter; // can be 1 to 5
			return basedmg;
		}
	}

	/// <summary>
	/// User rages until the start of a round in which they don't use this move. (Rage)
	/// (Handled in Battler's pbProcessMoveAgainstTarget): Ups rager's Attack by 1
	/// stage each time it loses HP due to a move.
	/// <summary>
	public class PokeBattle_Move_093 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{

			int ret = (int)base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);

			if (ret > 0) attacker.effects.Rage = true;
			return ret;
		}
	}

	/// <summary>
	/// Randomly damages or heals the target. (Present)
	/// <summary>
	public class PokeBattle_Move_094 : PokeBattle_Move
	{
		public object pbOnStartUse(Battle.Battler attacker)
		{
			// Just to ensure that Parental Bond's second hit damages if the first hit does
			this.forcedamage = false;
			return true;
		}

		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{
			this.forcedamage = true;
			return this.calcbasedmg;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			this.calcbasedmg = 1;
			byte r = (byte)this.battle.pbRandom((this.forcedamage) ? 8 : 10);
			if (r < 4)
				this.calcbasedmg = 40;
			else if (r < 7)
				this.calcbasedmg = 80;
			else if (r < 8)
				this.calcbasedmg = 120;
			else
			{
				if (pbTypeModifier(pbType(this.type, attacker, opponent), attacker, opponent) == 0)
				{
					//battle.pbDisplay(_INTL("It doesn't affect {1}...",opponent.pbThis(true)))
					return -1;
				}
				if (opponent.HP == opponent.TotalHP)
				{
					//battle.pbDisplay(_INTL("But it failed!"))
					return -1;
				}
				int damage = pbCalcDamage(attacker, opponent); // Consumes Gems even if it will heal
				pbShowAnimation(this.id, attacker, opponent, 1, alltargets, showanimation); // Healing animation;
				opponent.RecoverHP((int)Math.Floor(opponent.TotalHP / 4f), true);
				//battle.pbDisplay(_INTL("{1} had its HP restored.",opponent.pbThis))   
				return 0;
			}
			return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
		}
	}

	/// <summary>
	/// Power is chosen at random. Power is doubled if the target is using Dig. (Magnitude)
	/// (Handled in Battler's pbSuccessCheck): Hits some semi-invulnerable targets.
	/// <summary>
	public class PokeBattle_Move_095 : PokeBattle_Move
	{
		public object pbOnStartUse(Battle.Battler attacker)
		{

			byte[] basedmg = new byte[] { 10, 30, 50, 70, 90, 110, 150 };
			byte[] magnitudes = new byte[] {
		   4,
		   5,5,
		   6,6,6,6,
		   7,7,7,7,7,7,
		   8,8,8,8,
		   9,9,
		   10
		};
			byte magni = magnitudes[this.battle.pbRandom(magnitudes.Length)];
			this.calcbasedmg = basedmg[magni - 4];

			//battle.pbDisplay(_INTL("Magnitude {1}!",magni))
			return true;
		}

		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{

			int ret = this.calcbasedmg;
			if ((int)new Move((Moves)opponent.effects.TwoTurnAttack).Function == 0xCA)	// Dig
			{ 
				ret *= 2;
			}
			if (this.battle.field.GrassyTerrain > 0)
			{
				ret = (int)Math.Round(ret / 2.0f);
			}
			return ret;
		}
	}

	/// <summary>
	/// Power and type depend on the user's held berry. Destroys the berry. (Natural Gift)
	/// <summary>
	public class PokeBattle_Move_096 : PokeBattle_Move
	{
		public object pbOnStartUse(Battle.Battler attacker)
		{
			if (!pbIsBerry(attacker.Item) ||
			   attacker.effects.Embargo > 0 ||
			   this.battle.field.MagicRoom > 0 ||
			   attacker.hasWorkingAbility(Abilities.KLUTZ) ||
			   attacker.pbOpposing1.hasWorkingAbility(Abilities.UNNERVE) ||
			   attacker.pbOpposing2.hasWorkingAbility(Abilities.UNNERVE))
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return false;
			}
			this.berry = attacker.Item;
			return true;
		}

		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{

			Dictionary<Items, byte> damagearray = new Dictionary<Items, byte>{
		   //60 => [	
					{ Items.CHERI_BERRY, 60 },  { Items.CHESTO_BERRY, 60 }, { Items.PECHA_BERRY, 60 },  { Items.RAWST_BERRY, 60 },  { Items.ASPEAR_BERRY, 60 },
					{ Items.LEPPA_BERRY, 60 },  { Items.ORAN_BERRY, 60 },   { Items.PERSIM_BERRY, 60 }, { Items.LUM_BERRY, 60 },    { Items.SITRUS_BERRY, 60 },
					{ Items.FIGY_BERRY, 60 },   { Items.WIKI_BERRY, 60 },   { Items.MAGO_BERRY, 60 },   { Items.AGUAV_BERRY, 60 },  { Items.IAPAPA_BERRY, 60 },
					{ Items.RAZZ_BERRY, 60 },   { Items.OCCA_BERRY, 60 },   { Items.PASSHO_BERRY, 60 }, { Items.WACAN_BERRY, 60 },  { Items.RINDO_BERRY, 60 },
					{ Items.YACHE_BERRY, 60 },  { Items.CHOPLE_BERRY, 60 }, { Items.KEBIA_BERRY, 60 },  { Items.SHUCA_BERRY, 60 },  { Items.COBA_BERRY, 60 },
					{ Items.PAYAPA_BERRY, 60 }, { Items.TANGA_BERRY, 60 },  { Items.CHARTI_BERRY, 60 }, { Items.KASIB_BERRY, 60 },  { Items.HABAN_BERRY, 60 },
					{ Items.COLBUR_BERRY, 60 }, { Items.BABIRI_BERRY, 60 }, { Items.CHILAN_BERRY, 60 }, { Items.ROSELI_BERRY, 60 },	
		   //70 => [	
					{ Items.BLUK_BERRY, 70 },   { Items.NANAB_BERRY, 70 },  { Items.WEPEAR_BERRY, 70 }, { Items.PINAP_BERRY, 70 },  { Items.POMEG_BERRY, 70 },
					{ Items.KELPSY_BERRY, 70 }, { Items.QUALOT_BERRY, 70 }, { Items.HONDEW_BERRY, 70 }, { Items.GREPA_BERRY, 70 },  { Items.TAMATO_BERRY, 70 },
					{ Items.CORNN_BERRY, 70 },  { Items.MAGOST_BERRY, 70 }, { Items.RABUTA_BERRY, 70 }, { Items.NOMEL_BERRY, 70 },  { Items.SPELON_BERRY, 70 },
					{ Items.PAMTRE_BERRY, 70 },	
		   //80 => [	
					{ Items.WATMEL_BERRY, 80 }, { Items.DURIN_BERRY, 80 },  { Items.BELUE_BERRY, 80 },  { Items.LIECHI_BERRY, 80 }, { Items.GANLON_BERRY, 80 },
					{ Items.SALAC_BERRY, 80 },  { Items.PETAYA_BERRY, 80 }, { Items.APICOT_BERRY, 80 }, { Items.LANSAT_BERRY, 80 }, { Items.STARF_BERRY, 80 },
					{ Items.ENIGMA_BERRY, 80 }, { Items.MICLE_BERRY, 80 },  { Items.CUSTAP_BERRY, 80 }, { Items.JABOCA_BERRY, 80 }, { Items.ROWAP_BERRY, 80 },
					{ Items.KEE_BERRY, 80 },    { Items.MARANGA_BERRY, 80 }
		};
			foreach (Items i in damagearray.Keys)
			{
				//byte data = damagearray[i];
				//if (data){
				//	foreach (var j in data){ 
						if (this.berry == i)
						{
							int ret = damagearray[i];

							if (Settings.USENEWBATTLEMECHANICS) ret += 20;
							return ret;
						}
				//	}
				//}
			}
			return 1;
		}

		public override Types pbModifyType(Types type, Battle.Battler attacker, Battle.Battler opponent)
		{

			type = Types.NORMAL;
			Dictionary<Items, Types> typearray = new Dictionary<Items, Types> {
		   //:NORMAL =>	
						{ Items.CHILAN_BERRY, Types.NORMAL },	
		   //:FIRE	 =>	
						{ Items.CHERI_BERRY, Types.FIRE },  { Items.BLUK_BERRY, Types.FIRE },   { Items.WATMEL_BERRY, Types.FIRE }, { Items.OCCA_BERRY, Types.FIRE },	
		   //:WATER	 =>	
						{ Items.CHESTO_BERRY, Types.WATER },    { Items.NANAB_BERRY, Types.WATER }, { Items.DURIN_BERRY, Types.WATER }, { Items.PASSHO_BERRY, Types.WATER },	
		   //:ELECTRIC =>	
						{ Items.PECHA_BERRY, Types.ELECTRIC },  { Items.WEPEAR_BERRY, Types.ELECTRIC }, { Items.BELUE_BERRY, Types.ELECTRIC },  { Items.WACAN_BERRY, Types.ELECTRIC },	
		   //:GRASS	 =>	
						{ Items.RAWST_BERRY, Types.GRASS }, { Items.PINAP_BERRY, Types.GRASS }, { Items.RINDO_BERRY, Types.GRASS }, { Items.LIECHI_BERRY, Types.GRASS },	
		   //:ICE	 =>	
						{ Items.ASPEAR_BERRY, Types.ICE },  { Items.POMEG_BERRY, Types.ICE },   { Items.YACHE_BERRY, Types.ICE },   { Items.GANLON_BERRY, Types.ICE },	
		   //:FIGHTING =>	
						{ Items.LEPPA_BERRY, Types.FIGHTING },  { Items.KELPSY_BERRY, Types.FIGHTING }, { Items.CHOPLE_BERRY, Types.FIGHTING }, { Items.SALAC_BERRY, Types.FIGHTING },	
		   //:POISON =>	
						{ Items.ORAN_BERRY, Types.POISON }, { Items.QUALOT_BERRY, Types.POISON },   { Items.KEBIA_BERRY, Types.POISON },    { Items.PETAYA_BERRY, Types.POISON },	
		   //:GROUND =>	
						{ Items.PERSIM_BERRY, Types.GROUND },   { Items.HONDEW_BERRY, Types.GROUND },   { Items.SHUCA_BERRY, Types.GROUND },    { Items.APICOT_BERRY, Types.GROUND },	
		   //:FLYING =>	
						{ Items.LUM_BERRY, Types.FLYING },  { Items.GREPA_BERRY, Types.FLYING },    { Items.COBA_BERRY, Types.FLYING }, { Items.LANSAT_BERRY, Types.FLYING },	
		   //:PSYCHIC=>	
						{ Items.SITRUS_BERRY, Types.PSYCHIC },  { Items.TAMATO_BERRY, Types.PSYCHIC },  { Items.PAYAPA_BERRY, Types.PSYCHIC },  { Items.STARF_BERRY, Types.PSYCHIC },	
		   //:BUG	 =>	
						{ Items.FIGY_BERRY, Types.BUG },    { Items.CORNN_BERRY, Types.BUG },   { Items.TANGA_BERRY, Types.BUG },   { Items.ENIGMA_BERRY, Types.BUG },	
		   //:ROCK	 =>	
						{ Items.WIKI_BERRY, Types.ROCK },   { Items.MAGOST_BERRY, Types.ROCK }, { Items.CHARTI_BERRY, Types.ROCK }, { Items.MICLE_BERRY, Types.ROCK },	
		   //:GHOST	 =>	
						{ Items.MAGO_BERRY, Types.GHOST },  { Items.RABUTA_BERRY, Types.GHOST },    { Items.KASIB_BERRY, Types.GHOST }, { Items.CUSTAP_BERRY, Types.GHOST },	
		   //:DRAGON =>	
						{ Items.AGUAV_BERRY, Types.DRAGON },    { Items.NOMEL_BERRY, Types.DRAGON },    { Items.HABAN_BERRY, Types.DRAGON },    { Items.JABOCA_BERRY, Types.DRAGON },	
		   //:DARK	 =>	
						{ Items.IAPAPA_BERRY, Types.DARK }, { Items.SPELON_BERRY, Types.DARK }, { Items.COLBUR_BERRY, Types.DARK }, { Items.ROWAP_BERRY, Types.DARK },  { Items.MARANGA_BERRY, Types.DARK },	
		   //:STEEL	 =>	
						{ Items.RAZZ_BERRY, Types.STEEL },  { Items.PAMTRE_BERRY, Types.STEEL },    { Items.BABIRI_BERRY, Types.STEEL },	
		   //:FAIRY	 =>	
						{ Items.ROSELI_BERRY, Types.FAIRY },    { Items.KEE_BERRY, Types.FAIRY }
		};
			foreach (Items i in typearray.Keys)
			{
				//data = typearray[i];;
				//if (data){
				//	foreach (var j in data){ 
						if (this.berry == i)
						{
							type = typearray[i];//i;
						}
				//	}
				//}
			}
			return type;
		}

		public void pbEffectAfterHit(Battle.Battler attacker, Battle.Battler opponent, Effects.Move turneffects)
		{
			if (turneffects.TotalDamage > 0)
			{
				attacker.pbConsumeItem();
			}
		}
	}

	/// <summary>
	/// Power increases the less PP this move has. (Trump Card)
	/// <summary>
	public class PokeBattle_Move_097 : PokeBattle_Move
	{
		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{
			int[] dmgs = new int[] { 200, 80, 60, 50, 40 };

			byte ppleft = Math.Min(this.PP, (byte)4);  // PP is reduced before the move is used
			basedmg = dmgs[ppleft];
			return basedmg;
		}
	}

	/// <summary>
	/// Power increases the less HP the user has. (Flail, Reversal)
	/// <summary>
	public class PokeBattle_Move_098 : PokeBattle_Move
	{
		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{
			byte n = (byte)Math.Floor(48f * attacker.HP / attacker.TotalHP);

			object ret = 20;
			if (n < 33) ret = 40;
			if (n < 17) ret = 80;
			if (n < 10) ret = 100;
			if (n < 5) ret = 150;
			if (n < 2) ret = 200;
			return ret;
		}
	}

	/// <summary>
	/// Power increases the quicker the user is than the target. (Electro Ball)
	/// <summary>
	public class PokeBattle_Move_099 : PokeBattle_Move
	{
		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{
			int n = (int)Math.Floor(Math.Max(attacker.pbSpeed, 1f) / Math.Max(opponent.pbSpeed, 1f));

			object ret = 60;
			if (n >= 2) ret = 80;
			if (n >= 3) ret = 120;
			if (n >= 4) ret = 150;
			return ret;
		}
	}

	/// <summary>
	/// Power increases the heavier the target is. (Grass Knot, Low Kick)
	/// <summary>
	public class PokeBattle_Move_09A : PokeBattle_Move
	{
		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{
			float weight = opponent.Weight(attacker);

			object ret = 20;
			if (weight >= 100) ret = 40;
			if (weight >= 250) ret = 60;
			if (weight >= 500) ret = 80;
			if (weight >= 1000) ret = 100;
			if (weight >= 2000) ret = 120;
			return ret;
		}
	}

	/// <summary>
	/// Power increases the heavier the user is than the target. (Heat Crash, Heavy Slam)
	/// <summary>
	public class PokeBattle_Move_09B : PokeBattle_Move
	{
		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{
			int n = (int)Math.Floor((float)attacker.weight / (float)opponent.Weight(attacker));

			object ret = 40;
			if (n >= 2) ret = 60;
			if (n >= 3) ret = 80;
			if (n >= 4) ret = 100;
			if (n >= 5) ret = 120;
			return ret;
		}
	}

	/// <summary>
	/// Powers up the ally's attack this round by 1.5. (Helping Hand)
	/// <summary>
	public class PokeBattle_Move_09C : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (!this.battle.doublebattle || opponent.isFainted() ||
			   (int)this.battle.choices[opponent.Index].Action != 1 || // Didn't choose a move;
			   opponent.hasMovedThisRound() ||
			   opponent.effects.HelpingHand)
			{
				//battle.pbDisplay(_INTL("But it failed!"))  
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			opponent.effects.HelpingHand = true;
			//battle.pbDisplay(_INTL("{1} is ready to help {2}!",attacker.pbThis,opponent.pbThis(true)))
			return 0;
		}
	}

	/// <summary>
	/// Weakens Electric attacks. (Mud Sport)
	/// <summary>
	public class PokeBattle_Move_09D : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (Settings.USENEWBATTLEMECHANICS)
			{
				if (this.battle.field.MudSportField > 0)
				{
					//battle.pbDisplay(_INTL("But it failed!"))
					return -1;
				}
				pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

				this.battle.field.MudSportField = 5;
				//battle.pbDisplay(_INTL("Electricity's power was weakened!"))
				return 0;
			}
			else
			{
				for (int i = 0; i < 4; i++)
				{
					if (attacker.battle.battlers[i].effects.MudSport)
					{
						//battle.pbDisplay(_INTL("But it failed!"))
						return -1;
					}
				}

				pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

				attacker.effects.MudSport = true;
				//battle.pbDisplay(_INTL("Electricity's power was weakened!"))
				return 0;
			}
			return -1;
		}
	}

	/// <summary>
	/// Weakens Fire attacks. (Water Sport)
	/// <summary>
	public class PokeBattle_Move_09E : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (Settings.USENEWBATTLEMECHANICS)
			{
				if (this.battle.field.WaterSportField > 0)
				{
					//battle.pbDisplay(_INTL("But it failed!"))
					return -1;
				}
				pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

				this.battle.field.WaterSportField = 5;
				//battle.pbDisplay(_INTL("Fire's power was weakened!"))
				return 0;
			}
			else
			{
				for (int i = 0; i < 4; i++)
				{
					if (attacker.battle.battlers[i].effects.WaterSport)
					{
						//battle.pbDisplay(_INTL("But it failed!"))
						return -1;
					}
				}

				pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

				attacker.effects.WaterSport = true;
				//battle.pbDisplay(_INTL("Fire's power was weakened!"))
				return 0;
			}
		}
	}

	/// <summary>
	/// Type depends on the user's held item. (Judgment, Techno Blast)
	/// <summary>
	public class PokeBattle_Move_09F : PokeBattle_Move
	{
		public override Types pbModifyType(Types type, Battle.Battler attacker, Battle.Battler opponent)
		{
			if (this.id == Moves.JUDGMENT)
			{
				if (attacker.Item == Items.FIST_PLATE) return (Types.FIGHTING);
				if (attacker.Item == Items.SKY_PLATE) return (Types.FLYING);
				if (attacker.Item == Items.TOXIC_PLATE) return (Types.POISON);
				if (attacker.Item == Items.EARTH_PLATE) return (Types.GROUND);
				if (attacker.Item == Items.STONE_PLATE) return (Types.ROCK);
				if (attacker.Item == Items.INSECT_PLATE) return (Types.BUG);
				if (attacker.Item == Items.SPOOKY_PLATE) return (Types.GHOST);
				if (attacker.Item == Items.IRON_PLATE) return (Types.STEEL);
				if (attacker.Item == Items.FLAME_PLATE) return (Types.FIRE);
				if (attacker.Item == Items.SPLASH_PLATE) return (Types.WATER);
				if (attacker.Item == Items.MEADOW_PLATE) return (Types.GRASS);
				if (attacker.Item == Items.ZAP_PLATE) return (Types.ELECTRIC);
				if (attacker.Item == Items.MIND_PLATE) return (Types.PSYCHIC);
				if (attacker.Item == Items.ICICLE_PLATE) return (Types.ICE);
				if (attacker.Item == Items.DRACO_PLATE) return (Types.DRAGON);
				if (attacker.Item == Items.DREAD_PLATE) return (Types.DARK);
				if (attacker.Item == Items.PIXIE_PLATE) return (Types.FAIRY);
			}
			else if (this.id == Moves.TECHNO_BLAST)
			{
				if (attacker.Item == Items.SHOCK_DRIVE) return Types.ELECTRIC;
				if (attacker.Item == Items.BURN_DRIVE) return Types.FIRE;
				if (attacker.Item == Items.CHILL_DRIVE) return Types.ICE;
				if (attacker.Item == Items.DOUSE_DRIVE) return Types.WATER;
			}
			return (Types.NORMAL);
		}

		public override IEnumerator pbShowAnimation(Moves id, Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (id == Moves.TECHNO_BLAST)
			{
				byte anim = 0;
				if (pbType(this.type, attacker, opponent) == Types.ELECTRIC) anim = 1;
				if (pbType(this.type, attacker, opponent) == Types.FIRE) anim = 2;
				if (pbType(this.type, attacker, opponent) == Types.ICE) anim = 3;
				if (pbType(this.type, attacker, opponent) == Types.WATER) anim = 4;
				return base.pbShowAnimation(id, attacker, opponent, anim, alltargets, showanimation); // Type-specific anim
			}
			return base.pbShowAnimation(id, attacker, opponent, hitnum, alltargets, showanimation);
		}
	}

	/// <summary>
	/// This attack is always a critical hit. (Frost Breath, Storm Throw)
	/// <summary>
	public class PokeBattle_Move_0A0 : PokeBattle_Move
	{
		public object pbCritialOverride(Battle.Battler attacker, Battle.Battler opponent)
		{
			return true;
		}
	}

	/// <summary>
	/// For 5 rounds, foes' attacks cannot become critical hits. (Lucky Chant)
	/// <summary>
	public class PokeBattle_Move_0A1 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.OwnSide.LuckyChant > 0)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			attacker.OwnSide.LuckyChant = 5;
			if (!this.battle.isOpposing(attacker.Index))
			{
				//battle.pbDisplay(_INTL("The Lucky Chant shielded your team from critical hits!"))
			}
			else
			{
				//battle.pbDisplay(_INTL("The Lucky Chant shielded the opposing team from critical hits!"))
			}
			return 0;
		}
	}

	/// <summary>
	/// For 5 rounds, lowers power of physical attacks against the user's side. (Reflect)
	/// <summary>
	public class PokeBattle_Move_0A2 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.OwnSide.Reflect > 0)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			attacker.OwnSide.Reflect = 5;
			if (attacker.hasWorkingItem(Items.LIGHT_CLAY)) attacker.OwnSide.Reflect = 8;
			if (!this.battle.isOpposing(attacker.Index))
			{
				//battle.pbDisplay(_INTL("Reflect raised your team's Defense!"))
			}
			else
			{
				//battle.pbDisplay(_INTL("Reflect raised the opposing team's Defense!"))
			}
			return 0;
		}
	}

	/// <summary>
	/// For 5 rounds, lowers power of special attacks against the user's side. (Light Screen)
	/// <summary>
	public class PokeBattle_Move_0A3 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.OwnSide.LightScreen > 0)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			attacker.OwnSide.LightScreen = 5;
			if (attacker.hasWorkingItem(Items.LIGHT_CLAY)) attacker.OwnSide.Reflect = 8;
			if (!this.battle.isOpposing(attacker.Index))
			{
				//battle.pbDisplay(_INTL("Light Screen raised your team's Special Defense!"))
			}
			else
			{
				//battle.pbDisplay(_INTL("Light Screen raised the opposing team's Special Defense!"))
			}
			return 0;
		}
	}

	/// <summary>
	/// Effect depends on the environment. (Secret Power)
	/// <summary>
	public class PokeBattle_Move_0A4 : PokeBattle_Move
	{
		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			if (this.battle.field.ElectricTerrain > 0)
			{
				if (opponent.pbCanParalyze(attacker, false, this))
				{
					opponent.pbParalyze(attacker);
				}
				return;
			}
			else if (this.battle.field.GrassyTerrain > 0)
			{
				if (opponent.pbCanSleep(attacker, false, this))
				{

					opponent.pbSleep();
				}
				return;
			}
			else if (this.battle.field.MistyTerrain > 0)
			{
				if (opponent.pbCanReduceStatStage(Stats.SPATK, attacker, false, this))
				{

					opponent.pbReduceStat(Stats.SPATK, 1, attacker, false, this);
				}
				return;
			}
			switch (this.battle.environment)
			{
				case PokemonUnity.Environment.Grass:
				case PokemonUnity.Environment.TallGrass:
				case PokemonUnity.Environment.Forest:
					if (opponent.pbCanSleep(attacker, false, this))
					{
						opponent.pbSleep();
					}
					break;
				case PokemonUnity.Environment.MovingWater:
				case PokemonUnity.Environment.Underwater:
					if (opponent.pbCanReduceStatStage(Stats.ATTACK, attacker, false, this))
					{

						opponent.pbReduceStat(Stats.ATTACK, 1, attacker, false, this);
					}
					break;
				case PokemonUnity.Environment.StillWater:
				case PokemonUnity.Environment.Sky:
					if (opponent.pbCanReduceStatStage(Stats.SPEED, attacker, false, this))
					{
						opponent.pbReduceStat(Stats.SPEED, 1, attacker, false, this);
					}
					break;
				case PokemonUnity.Environment.Sand:
					if (opponent.pbCanReduceStatStage(Stats.ACCURACY, attacker, false, this))
					{
						opponent.pbReduceStat(Stats.ACCURACY, 1, attacker, false, this);
					}
					break;
				case PokemonUnity.Environment.Rock:
					if (Settings.USENEWBATTLEMECHANICS)
					{
						if (opponent.pbCanReduceStatStage(Stats.ACCURACY, attacker, false, this))
						{
							opponent.pbReduceStat(Stats.ACCURACY, 1, attacker, false, this);
						}
					}
					else
					  if (opponent.effects.Substitute == 0 || ignoresSubstitute(attacker))
					{
						opponent.pbFlinch(attacker);

					}
					break;

				case PokemonUnity.Environment.Cave:
				case PokemonUnity.Environment.Graveyard:
				case PokemonUnity.Environment.Space:
					if (opponent.effects.Substitute == 0 || ignoresSubstitute(attacker))
					{
						opponent.pbFlinch(attacker);
					}
					break;

				case PokemonUnity.Environment.Snow:
					if (opponent.pbCanFreeze(attacker, false, this))
					{

						opponent.pbFreeze();
					}
					break;

				case PokemonUnity.Environment.Volcano:
					if (opponent.pbCanBurn(attacker, false, this))
					{

						opponent.pbBurn(attacker);
					}
					else
					if (opponent.pbCanParalyze(attacker, false, this))
					{
						opponent.pbParalyze(attacker);
					}
					break;
				default:
					break;
			}
		}

		public override IEnumerator pbShowAnimation(Moves id, Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{

			id = Moves.BODY_SLAM;
			if (this.battle.field.ElectricTerrain > 0)
				id = Moves.THUNDER_SHOCK;
			else if (this.battle.field.GrassyTerrain > 0)
				id = Moves.VINE_WHIP;
			else if (this.battle.field.MistyTerrain > 0)
				id = Moves.FAIRY_WIND;
			else
				switch (this.battle.environment)
				{
					case PokemonUnity.Environment.Grass:
					case PokemonUnity.Environment.TallGrass:
						id = (Settings.USENEWBATTLEMECHANICS) ? Moves.VINE_WHIP : Moves.NEEDLE_ARM; break;

					case PokemonUnity.Environment.MovingWater: id = Moves.WATER_PULSE; break;
					case PokemonUnity.Environment.StillWater: id = Moves.MUD_SHOT; break;
					case PokemonUnity.Environment.Underwater: id = Moves.WATER_PULSE; break;
					case PokemonUnity.Environment.Cave: id = Moves.ROCK_THROW; break;
					case PokemonUnity.Environment.Rock: id = Moves.MUD_SLAP; break;
					case PokemonUnity.Environment.Sand: id = Moves.MUD_SLAP; break;
					case PokemonUnity.Environment.Forest: id = Moves.RAZOR_LEAF; break;
					// Ice tiles in Gen 6 should be Ice Shard
					case PokemonUnity.Environment.Snow: id = Moves.AVALANCHE; break;
					case PokemonUnity.Environment.Volcano: id = Moves.INCINERATE; break;
					case PokemonUnity.Environment.Graveyard: id = Moves.SHADOW_SNEAK; break;
					case PokemonUnity.Environment.Sky: id = Moves.GUST; break;
					case PokemonUnity.Environment.Space: id = Moves.SWIFT; break;
					default: break;
				}

			return base.pbShowAnimation(id, attacker, opponent, hitnum, alltargets, showanimation); // Environment-specific anim;
		}
	}

	/// <summary>
	/// Always hits.
	/// <summary>
	public class PokeBattle_Move_0A5 : PokeBattle_Move
	{
		public override object pbAccuracyCheck(Battle.Battler attacker, Battle.Battler opponent)
		{
			return true;
		}
	}

	/// <summary>
	/// User's attack next round against the target will definitely hit. (Lock-On, Mind Reader)
	/// <summary>
	public class PokeBattle_Move_0A6 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (opponent.effects.Substitute > 0 && !ignoresSubstitute(attacker))
			{
				//battle.pbDisplay(_INTL("But it failed!"))  
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			opponent.effects.LockOn = 2;
			opponent.effects.LockOnPos = attacker.Index;
			//battle.pbDisplay(_INTL("{1} took aim at {2}!", attacker.pbThis, opponent.pbThis(true)))
			return 0;
		}
	}

	/// <summary>
	/// Target's evasion stat changes are ignored from now on. (Foresight, Odor Sleuth)
	/// Normal and Fighting moves have normal effectiveness against the Ghost-type target.
	/// <summary>
	public class PokeBattle_Move_0A7 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (opponent.OwnSide.CraftyShield)
			{
				//battle.pbDisplay(_INTL("But it failed!"))  
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			opponent.effects.Foresight = true;
			//battle.pbDisplay(_INTL("{1} was identified!",opponent.pbThis))
			return 0;
		}
	}

	/// <summary>
	/// Target's evasion stat changes are ignored from now on. (Miracle Eye)
	/// Psychic moves have normal effectiveness against the Dark-type target.
	/// <summary>
	public class PokeBattle_Move_0A8 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (opponent.OwnSide.CraftyShield)
			{
				//battle.pbDisplay(_INTL("But it failed!"))  
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			opponent.effects.MiracleEye = true;
			//battle.pbDisplay(_INTL("{1} was identified!",opponent.pbThis))
			return 0;
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
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			List<int> ratesharers = new List<int> {
		   0xAA,   // Detect, Protect
		   0xAB,   // Quick Guard
		   0xAC,   // Wide Guard
		   0xE8,   // Endure
		   0x14B,  // King's Shield
		   0x14C   // Spiky Shield
		 };
			if (!ratesharers.Contains((int)new Move((Moves)attacker.lastMoveUsed).Function))
			{
				attacker.effects.ProtectRate = 1;
			}
			bool unmoved = false;
			foreach (Battle.Battler poke in this.battle.battlers)
			{
				if (poke.Index == attacker.Index) continue; //next
				if ((int)this.battle.choices[poke.Index].Action == 1 && // Chose a move
				   !poke.hasMovedThisRound())
				{
					unmoved = true; break;
				}
			}
			if (!unmoved ||
			   this.battle.pbRandom(65536) >= Math.Floor(65536f / attacker.effects.ProtectRate))
			{
				attacker.effects.ProtectRate = 1;

				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			attacker.effects.Protect = true;
			attacker.effects.ProtectRate *= 2;
			//battle.pbDisplay(_INTL("{1} protected itself!",attacker.pbThis))
			return 0;
		}
	}

	/// <summary>
	/// User's side is protected against moves with priority greater than 0 this round.
	/// (Quick Guard)
	/// <summary>
	public class PokeBattle_Move_0AB : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.OwnSide.QuickGuard)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			List<int> ratesharers = new List<int> {
		   0xAA,   // Detect, Protect
		   0xAB,   // Quick Guard
		   0xAC,   // Wide Guard
		   0xE8,   // Endure
		   0x14B,  // King's Shield
		   0x14C   // Spiky Shield
		 };
			if (!ratesharers.Contains((int)new Move((Moves)attacker.lastMoveUsed).Function))
			{
				attacker.effects.ProtectRate = 1;
			}
			bool unmoved = false;
			foreach (Battle.Battler poke in this.battle.battlers)
			{
				if (poke.Index == attacker.Index) continue; //next
				if ((int)this.battle.choices[poke.Index].Action == 1 && // Chose a move
				   !poke.hasMovedThisRound())
				{
					unmoved = true; break;
				}
			}
			if (!unmoved ||
			   (!Settings.USENEWBATTLEMECHANICS &&
			   this.battle.pbRandom(65536) >= Math.Floor(65536f / attacker.effects.ProtectRate)))
			{
				attacker.effects.ProtectRate = 1;
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			attacker.OwnSide.QuickGuard = true;
			attacker.effects.ProtectRate *= 2;
			if (!this.battle.isOpposing(attacker.Index))
			{
				//battle.pbDisplay(_INTL("Quick Guard protected your team!"))
			}
			else
			{
				//battle.pbDisplay(_INTL("Quick Guard protected the opposing team!"))
			}
			return 0;
		}
	}

	/// <summary>
	/// User's side is protected against moves that target multiple battlers this round.
	/// (Wide Guard)
	/// <summary>
	public class PokeBattle_Move_0AC : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.OwnSide.WideGuard)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			List<int> ratesharers = new List<int> {
		   0xAA,   // Detect, Protect
		   0xAB,   // Quick Guard
		   0xAC,   // Wide Guard
		   0xE8,   // Endure
		   0x14B,  // King's Shield
		   0x14C   // Spiky Shield
		 };
			if (!ratesharers.Contains((int)new Move((Moves)attacker.lastMoveUsed).Function))
			{
				attacker.effects.ProtectRate = 1;
			}
			bool unmoved = false;
			foreach (Battle.Battler poke in this.battle.battlers)
			{
				if (poke.Index == attacker.Index) continue; //next
				if ((int)this.battle.choices[poke.Index].Action == 1 && // Chose a move
				   !poke.hasMovedThisRound())
				{
					unmoved = true; break;
				}
			}
			if (!unmoved ||
			   (!Settings.USENEWBATTLEMECHANICS &&
			   this.battle.pbRandom(65536) >= (int)Math.Floor(65536f / attacker.effects.ProtectRate)))
			{
				attacker.effects.ProtectRate = 1;
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			attacker.OwnSide.WideGuard = true;
			attacker.effects.ProtectRate *= 2;
			if (!this.battle.isOpposing(attacker.Index))
			{
				//battle.pbDisplay(_INTL("Wide Guard protected your team!"))
			}
			else
			{
				//battle.pbDisplay(_INTL("Wide Guard protected the opposing team!"))
			}
			return 0;
		}
	}

	/// <summary>
	/// Ignores target's protections. If successful, all other moves this round
	/// ignore them too. (Feint)
	/// <summary>
	public class PokeBattle_Move_0AD : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			int ret = (int)base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (ret > 0)
			{
				opponent.effects.ProtectNegation = true;
				opponent.OwnSide.CraftyShield = false;
			}
			return ret;
		}
	}

	/// <summary>
	/// Uses the last move that the target used. (Mirror Move)
	/// <summary>
	public class PokeBattle_Move_0AE : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (opponent.lastMoveUsed <= 0 || //(
			   !new Move((Moves)opponent.lastMoveUsed).Flag.Mirror //& 0x10)==0
			   ) // flag e: Copyable by Mirror Move
			{ 
			  //battle.pbDisplay(_INTL("The mirror move failed!"))
				return -1;
			}
			attacker.pbUseMoveSimple(opponent.lastMoveUsed, -1, opponent.Index);
			return 0;
		}
	}

	/// <summary>
	/// Uses the last move that was used. (Copycat)
	/// <summary>
	public class PokeBattle_Move_0AF : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			List<Move.Effect> blacklist = new List<Move.Effect> {
		   (Move.Effect)0x02,    // Struggle
		   (Move.Effect)0x69,    // Transform
		   (Move.Effect)0x71,    // Counter
		   (Move.Effect)0x72,    // Mirror Coat
		   (Move.Effect)0x73,    // Metal Burst
		   (Move.Effect)0x9C,    // Helping Hand
		   (Move.Effect)0xAA,    // Detect, Protect
		   (Move.Effect)0xAD,    // Feint
		   (Move.Effect)0xAE,    // Mirror Move
		   (Move.Effect)0xAF,    // Copycat
		   (Move.Effect)0xB2,    // Snatch
		   (Move.Effect)0xE7,    // Destiny Bond
		   (Move.Effect)0xE8,    // Endure
		   (Move.Effect)0xEC,    // Circle Throw, Dragon Tail
		   (Move.Effect)0xF1,    // Covet, Thief
		   (Move.Effect)0xF2,    // Switcheroo, Trick
		   (Move.Effect)0xF3,    // Bestow
		   (Move.Effect)0x115,   // Focus Punch
		   (Move.Effect)0x117,   // Follow Me, Rage Powder
		   (Move.Effect)0x158    // Belch
		 };
			if (Settings.USENEWBATTLEMECHANICS)
			{
				blacklist.AddRange(new List<Move.Effect> {
			 (Move.Effect)0xEB,		// Roar, Whirlwind
									// Two-turn attacks
			 (Move.Effect)0xC3,		// Razor Wind
			 (Move.Effect)0xC4,		// SolarBeam
			 (Move.Effect)0xC5,		// Freeze Shock
			 (Move.Effect)0xC6,		// Ice Burn
			 (Move.Effect)0xC7,		// Sky Attack
			 (Move.Effect)0xC8,		// Skull Bash
			 (Move.Effect)0xC9,		// Fly
			 (Move.Effect)0xCA,		// Dig
			 (Move.Effect)0xCB,		// Dive
			 (Move.Effect)0xCC,		// Bounce
			 (Move.Effect)0xCD,		// Shadow Force
			 (Move.Effect)0xCE,		// Sky Drop
			 (Move.Effect)0x14D,	// Phantom Force
			 (Move.Effect)0x14E		// Geomancy
		   });
			}
			if (this.battle.lastMoveUsed <= 0 ||
			   blacklist.Contains(new Move((Moves)this.battle.lastMoveUsed).Function))
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			attacker.pbUseMoveSimple(this.battle.lastMoveUsed);
			return 0;
		}
	}

	/// <summary>
	/// Uses the move the target was about to use this round, with 1.5x power. (Me First)
	/// <summary>
	public class PokeBattle_Move_0B0 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			List<Move.Effect> blacklist = new List<Move.Effect> {
		   (Move.Effect)0x02,    // Struggle
		   (Move.Effect)0x14,    // Chatter
		   (Move.Effect)0x71,    // Counter
		   (Move.Effect)0x72,    // Mirror Coat
		   (Move.Effect)0x73,    // Metal Burst
		   (Move.Effect)0xB0,    // Me First
		   (Move.Effect)0xF1,    // Covet, Thief
		   (Move.Effect)0x115,   // Focus Punch
		   (Move.Effect)0x158    // Belch
		 };
			Battle.InBattleMove oppmove = this.battle.choices[opponent.Index].Move;
			if ((int)this.battle.choices[opponent.Index].Action != 1 || // Didn't choose a move
			   opponent.hasMovedThisRound() ||
			   oppmove.MoveId == Moves.NONE || oppmove.MoveId <= 0 ||
			   oppmove.pbIsStatus() ||
			   blacklist.Contains(oppmove.Function))
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			attacker.effects.MeFirst = true;

			attacker.pbUseMoveSimple(oppmove.MoveId);
			attacker.effects.MeFirst = false;
			return 0;
		}
	}

	/// <summary>
	/// This round, reflects all moves with the "C" flag targeting the user back at
	/// their origin. (Magic Coat)
	/// <summary>
	public class PokeBattle_Move_0B1 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			attacker.effects.MagicCoat = true;
			//battle.pbDisplay(_INTL("{1} shrouded itself with Magic Coat!",attacker.pbThis))
			return 0;
		}
	}

	/// <summary>
	/// This round, snatches all used moves with the "D" flag. (Snatch)
	/// <summary>
	public class PokeBattle_Move_0B2 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			attacker.effects.Snatch = true;
			//battle.pbDisplay(_INTL("{1} waits for a target to make a move!",attacker.pbThis))
			return 0;
		}
	}

	/// <summary>
	/// Uses a different move depending on the environment. (Nature Power)
	/// <summary>
	public class PokeBattle_Move_0B3 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			Moves move = Moves.TRI_ATTACK;
			switch (this.battle.environment)
			{
				case PokemonUnity.Environment.Grass:
				case PokemonUnity.Environment.TallGrass:
				case PokemonUnity.Environment.Forest:
					move = (Settings.USENEWBATTLEMECHANICS) ? Moves.ENERGY_BALL : Moves.SEED_BOMB; break;

				case PokemonUnity.Environment.MovingWater: move = Moves.HYDRO_PUMP; break;
				case PokemonUnity.Environment.StillWater: move = Moves.MUD_BOMB; break;
				case PokemonUnity.Environment.Underwater: move = Moves.HYDRO_PUMP; break;
				case PokemonUnity.Environment.Cave:
					move = (Settings.USENEWBATTLEMECHANICS) ? Moves.POWER_GEM : Moves.ROCK_SLIDE; break;

				case PokemonUnity.Environment.Rock:

					move = (Settings.USENEWBATTLEMECHANICS) ? Moves.EARTH_POWER : Moves.ROCK_SLIDE; break;

				case PokemonUnity.Environment.Sand:
					move = (Settings.USENEWBATTLEMECHANICS) ? Moves.EARTH_POWER : Moves.EARTHQUAKE; break;
				// Ice tiles in Gen 6 should be Ice Beam
				case PokemonUnity.Environment.Snow:
					move = (Settings.USENEWBATTLEMECHANICS) ? Moves.FROST_BREATH : Moves.ICE_BEAM; break;

				case PokemonUnity.Environment.Volcano: move = Moves.LAVA_PLUME; break;
				case PokemonUnity.Environment.Graveyard: move = Moves.SHADOW_BALL; break;
				case PokemonUnity.Environment.Sky: move = Moves.AIR_SLASH; break;
				case PokemonUnity.Environment.Space: move = Moves.DRACO_METEOR; break;
			}
			if (this.battle.field.ElectricTerrain > 0)
			{
				move = Moves.THUNDERBOLT;
			}
			else if (this.battle.field.GrassyTerrain > 0)
			{
				move = Moves.ENERGY_BALL;
			}
			else if (this.battle.field.MistyTerrain > 0)
			{
				move = Moves.MOONBLAST;
			}
			if (move == 0)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			string thismovename = PBMoves.getName(this.id);

			string movename = PBMoves.getName(move);
			//battle.pbDisplay(_INTL("{1} turned into {2}!", thismovename, movename))
			int target = (Settings.USENEWBATTLEMECHANICS && opponent.Species != Pokemons.NONE) ? opponent.Index : -1;
			attacker.pbUseMoveSimple(move, -1, target);
			return 0;
		}
	}

	/// <summary>
	/// Uses a random move the user knows. Fails if user is not asleep. (Sleep Talk)
	/// <summary>
	public class PokeBattle_Move_0B4 : PokeBattle_Move
	{
		public bool pbCanUseWhileAsleep()
		{
			return true;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.Status != Status.SLEEP)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			List<Move.Effect> blacklist = new List<Move.Effect> {
		   (Move.Effect)0x02,	// Struggle
		   (Move.Effect)0x14,	// Chatter
		   (Move.Effect)0x5C,	// Mimic
		   (Move.Effect)0x5D,	// Sketch
		   (Move.Effect)0xAE,	// Mirror Move
		   (Move.Effect)0xAF,	// Copycat
		   (Move.Effect)0xB0,	// Me First
		   (Move.Effect)0xB3,	// Nature Power
		   (Move.Effect)0xB4,	// Sleep Talk
		   (Move.Effect)0xB5,	// Assist
		   (Move.Effect)0xB6,	// Metronome
		   (Move.Effect)0xD1,	// Uproar
		   (Move.Effect)0xD4,	// Bide
		   (Move.Effect)0x115,	// Focus Punch
								// Two-turn attacks
		   (Move.Effect)0xC3,	// Razor Wind
		   (Move.Effect)0xC4,	// SolarBeam
		   (Move.Effect)0xC5,	// Freeze Shock
		   (Move.Effect)0xC6,	// Ice Burn
		   (Move.Effect)0xC7,	// Sky Attack
		   (Move.Effect)0xC8,	// Skull Bash
		   (Move.Effect)0xC9,	// Fly
		   (Move.Effect)0xCA,	// Dig
		   (Move.Effect)0xCB,	// Dive
		   (Move.Effect)0xCC,	// Bounce
		   (Move.Effect)0xCD,	// Shadow Force
		   (Move.Effect)0xCE,	// Sky Drop
		   (Move.Effect)0x14D,	// Phantom Force
		   (Move.Effect)0x14E	// Geomancy
		 };

			List<int> choices = new List<int>(); //[];
			for (int i = 0; i < 4; i++)
			{
				bool found = false;
				if (attacker.moves[i].MoveId == 0) continue; //next
				if (blacklist.Contains(attacker.moves[i].Function)) found = true;
				if (found) continue; //next
				if (this.battle.pbCanChooseMove(attacker.Index, i, false, true)) choices.Add(i);
			}
			if (choices.Count == 0)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			int choice = choices[this.battle.pbRandom(choices.Count)];
			attacker.pbUseMoveSimple(attacker.moves[choice].MoveId, -1, attacker.pbOppositeOpposing.Index);
			return 0;
		}
	}

	/// <summary>
	/// Uses a random move known by any non-user Pokémon in the user's party. (Assist)
	/// <summary>
	public class PokeBattle_Move_0B5 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			List<Move.Effect> blacklist = new List<Move.Effect> {
		   (Move.Effect)0x02,		// Struggle
		   (Move.Effect)0x14,		// Chatter
		   (Move.Effect)0x5C,		// Mimic
		   (Move.Effect)0x5D,		// Sketch
		   (Move.Effect)0x69,		// Transform
		   (Move.Effect)0x71,		// Counter
		   (Move.Effect)0x72,		// Mirror Coat
		   (Move.Effect)0x73,		// Metal Burst
		   (Move.Effect)0x9C,		// Helping Hand
		   (Move.Effect)0xAA,		// Detect, Protect
		   (Move.Effect)0xAD,		// Feint
		   (Move.Effect)0xAE,		// Mirror Move
		   (Move.Effect)0xAF,		// Copycat
		   (Move.Effect)0xB0,		// Me First
		   (Move.Effect)0xB2,		// Snatch
		   (Move.Effect)0xB3,		// Nature Power
		   (Move.Effect)0xB4,		// Sleep Talk
		   (Move.Effect)0xB5,		// Assist
		   (Move.Effect)0xB6,		// Metronome
		   (Move.Effect)0xCD,		// Shadow Force
		   (Move.Effect)0xE7,		// Destiny Bond
		   (Move.Effect)0xE8,		// Endure
		   (Move.Effect)0xEB,		// Roar, Whirlwind
		   (Move.Effect)0xEC,		// Circle Throw, Dragon Tail
		   (Move.Effect)0xF1,		// Covet, Thief
		   (Move.Effect)0xF2,		// Switcheroo, Trick
		   (Move.Effect)0xF3,		// Bestow
		   (Move.Effect)0x115,		// Focus Punch
		   (Move.Effect)0x117,		// Follow Me, Rage Powder
		   (Move.Effect)0x149,		// Mat Block
		   (Move.Effect)0x14B,		// King's Shield
		   (Move.Effect)0x14C,		// Spiky Shield
		   (Move.Effect)0x14D,		// Phantom Force
		   (Move.Effect)0x158		// Belch
		};
			if (Settings.USENEWBATTLEMECHANICS)
			{
				blacklist.AddRange(new List<Move.Effect>{
									// Two-turn attacks
			 (Move.Effect)0xC3,		// Razor Wind
			 (Move.Effect)0xC4,		// SolarBeam
			 (Move.Effect)0xC5,		// Freeze Shock
			 (Move.Effect)0xC6,		// Ice Burn
			 (Move.Effect)0xC7,		// Sky Attack
			 (Move.Effect)0xC8,		// Skull Bash
			 (Move.Effect)0xC9,		// Fly
			 (Move.Effect)0xCA,		// Dig
			 (Move.Effect)0xCB,		// Dive
			 (Move.Effect)0xCC,		// Bounce
			 (Move.Effect)0xCD,		// Shadow Force
			 (Move.Effect)0xCE,		// Sky Drop
			 (Move.Effect)0x14D,	// Phantom Force
			 (Move.Effect)0x14E		// Geomancy
		  });
			}
			List<Moves> moves = new List<Moves>();

			Battle.Battler[] party = this.battle.pbParty(attacker.Index); // NOTE: pbParty is common to both allies in multi battles
			for (int i = 0; i < party.Length; i++)
			{
				if (i != attacker.pokemonIndex && party[i].Species != Pokemons.NONE && !(Settings.USENEWBATTLEMECHANICS && party[i].isEgg))
				{
					foreach (var j in party[i].moves)
					{
						if (j.Type == Types.SHADOW) continue; //next
						if (j.MoveId == 0) continue; //next
						//bool found=false;
						if (!blacklist.Contains(new Move((Moves)j.MoveId).Function)) moves.Add(j.MoveId);
					}
				}

			}
			if (moves.Count == 0)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			Moves move = moves[this.battle.pbRandom(moves.Count)];
			attacker.pbUseMoveSimple(move);
			return 0;
		}
	}

	/// <summary>
	/// Uses a random move that exists. (Metronome)
	/// <summary>
	public class PokeBattle_Move_0B6 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			List<Move.Effect> blacklist = new List<Move.Effect> {
		   (Move.Effect)0x02,    // Struggle
		   (Move.Effect)0x11,    // Snore
		   (Move.Effect)0x14,    // Chatter
		   (Move.Effect)0x5C,    // Mimic
		   (Move.Effect)0x5D,    // Sketch
		   (Move.Effect)0x69,    // Transform
		   (Move.Effect)0x71,    // Counter
		   (Move.Effect)0x72,    // Mirror Coat
		   (Move.Effect)0x73,    // Metal Burst
		   (Move.Effect)0x9C,    // Helping Hand
		   (Move.Effect)0xAA,    // Detect, Protect
		   (Move.Effect)0xAB,    // Quick Guard
		   (Move.Effect)0xAC,    // Wide Guard
		   (Move.Effect)0xAD,    // Feint
		   (Move.Effect)0xAE,    // Mirror Move
		   (Move.Effect)0xAF,    // Copycat
		   (Move.Effect)0xB0,    // Me First
		   (Move.Effect)0xB2,    // Snatch
		   (Move.Effect)0xB3,    // Nature Power
		   (Move.Effect)0xB4,    // Sleep Talk
		   (Move.Effect)0xB5,    // Assist
		   (Move.Effect)0xB6,    // Metronome
		   (Move.Effect)0xE7,    // Destiny Bond
		   (Move.Effect)0xE8,    // Endure
		   (Move.Effect)0xF1,    // Covet, Thief
		   (Move.Effect)0xF2,    // Switcheroo, Trick
		   (Move.Effect)0xF3,    // Bestow
		   (Move.Effect)0x115,   // Focus Punch
		   (Move.Effect)0x117,   // Follow Me, Rage Powder
		   (Move.Effect)0x11D,   // After You
		   (Move.Effect)0x11E    // Quash
		};
			List<Moves> blacklistmoves = new List<Moves> {
		   Moves.FREEZE_SHOCK,
		   Moves.ICE_BURN,
		   Moves.RELIC_SONG,
		   Moves.SECRET_SWORD,
		   Moves.SNARL,
		   Moves.TECHNO_BLAST,
		   Moves.V_CREATE,
		   Moves.GEOMANCY
		};
			for (int i = 0; i < 1000; i++) //loop do break unless i<1000
			{
				Moves move = (Moves)(this.battle.pbRandom(Enum.GetValues(typeof(Moves)).Length) + 1);
				if (new Move((Moves)move).Type == Types.SHADOW) continue; //next
				bool found = false;
				if (blacklist.Contains(new Move((Moves)move).Function))
					found = true;
				else
				{
					foreach (Moves j in blacklistmoves)
					{
						if (move == j)
						{
							found = true;
							break;
						}
					}

				}
				if (!found)
				{
					pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

					attacker.pbUseMoveSimple(move);
					return 0;
				}
				i += 1;
			}
			//battle.pbDisplay(_INTL("But it failed!"))
			return -1;
		}
	}

	/// <summary>
	/// The target can no longer use the same move twice in a row. (Torment)
	/// <summary>
	public class PokeBattle_Move_0B7 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (opponent.effects.Torment)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			if (!attacker.hasMoldBreaker())
			{
				if (opponent.hasWorkingAbility(Abilities.AROMA_VEIL))
				{
					//battle.pbDisplay(_INTL("But it failed because of {1}'s {2}!",
					//   opponent.pbThis,PBAbilities.getName(opponent.Ability)));
					return -1;
				}
				else if (opponent.Partner.hasWorkingAbility(Abilities.AROMA_VEIL))
				{

					//battle.pbDisplay(_INTL("But it failed because of {1}'s {2}!",
					//   opponent.Partner.pbThis,PBAbilities.getName(opponent.Partner.Ability)));
					return -1;
				}
			}

			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			opponent.effects.Torment = true;
			//battle.pbDisplay(_INTL("{1} was subjected to torment!",opponent.pbThis))
			return 0;
		}
	}

	/// <summary>
	/// Disables all target's moves that the user also knows. (Imprison)
	/// <summary>
	public class PokeBattle_Move_0B8 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.effects.Imprison)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			attacker.effects.Imprison = true;
			//battle.pbDisplay(_INTL("{1} sealed the opponent's move(s)!",attacker.pbThis))
			return 0;
		}
	}

	/// <summary>
	/// For 5 rounds, disables the last move the target used. (Disable)
	/// <summary>
	public class PokeBattle_Move_0B9 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (opponent.effects.Disable > 0)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			if (!attacker.hasMoldBreaker())
			{
				if (opponent.hasWorkingAbility(Abilities.AROMA_VEIL))
				{
					//battle.pbDisplay(_INTL("But it failed because of {1}'s {2}!",
					//   opponent.pbThis,PBAbilities.getName(opponent.Ability)));
					return -1;
				}
				else if (opponent.Partner.hasWorkingAbility(Abilities.AROMA_VEIL))
				{

					//battle.pbDisplay(_INTL("But it failed because of {1}'s {2}!",
					//   opponent.Partner.pbThis,PBAbilities.getName(opponent.Partner.Ability)));
					return -1;
				}
			}
			foreach (var i in opponent.moves)
			{
				if (i.MoveId > 0 && i.MoveId == opponent.lastMoveUsed && (i.PP > 0 || i.TotalPP == 0))
				{
					pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

					opponent.effects.Disable = 5;
					opponent.effects.DisableMove = opponent.lastMoveUsed;
					//battle.pbDisplay(_INTL("{1}'s {2} was disabled!", opponent.pbThis, i.name))
					return 0;
				}
			}

			//battle.pbDisplay(_INTL("But it failed!"))
			return -1;
		}
	}

	/// <summary>
	/// For 4 rounds, disables the target's non-damaging moves. (Taunt)
	/// <summary>
	public class PokeBattle_Move_0BA : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (opponent.effects.Taunt > 0 ||
			   (Settings.USENEWBATTLEMECHANICS &&
			   !attacker.hasMoldBreaker() && opponent.hasWorkingAbility(Abilities.OBLIVIOUS)))
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			if (!attacker.hasMoldBreaker())
			{
				if (opponent.hasWorkingAbility(Abilities.AROMA_VEIL))
				{
					//battle.pbDisplay(_INTL("But it failed because of {1}'s {2}!",
					//   opponent.pbThis,PBAbilities.getName(opponent.Ability)));
					return -1;
				}
				else if (opponent.Partner.hasWorkingAbility(Abilities.AROMA_VEIL))
				{

					//battle.pbDisplay(_INTL("But it failed because of {1}'s {2}!",
					//   opponent.Partner.pbThis,PBAbilities.getName(opponent.Partner.Ability)));
					return -1;
				}
			}

			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			opponent.effects.Taunt = 4;
			//battle.pbDisplay(_INTL("{1} fell for the taunt!",opponent.pbThis))
			return 0;
		}
	}

	/// <summary>
	/// For 5 rounds, disables the target's healing moves. (Heal Block)
	/// <summary>
	public class PokeBattle_Move_0BB : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (opponent.effects.HealBlock > 0)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			if (!attacker.hasMoldBreaker())
			{
				if (opponent.hasWorkingAbility(Abilities.AROMA_VEIL))
				{
					//battle.pbDisplay(_INTL("But it failed because of {1}'s {2}!",
					//   opponent.pbThis,PBAbilities.getName(opponent.Ability)));
					return -1;
				}
				else if (opponent.Partner.hasWorkingAbility(Abilities.AROMA_VEIL))
				{

					//battle.pbDisplay(_INTL("But it failed because of {1}'s {2}!",
					//   opponent.Partner.pbThis,PBAbilities.getName(opponent.Partner.Ability)));
					return -1;
				}
			}

			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			opponent.effects.HealBlock = 5;
			//battle.pbDisplay(_INTL("{1} was prevented from healing!",opponent.pbThis))
			return 0;
		}
	}

	/// <summary>
	/// For 4 rounds, the target must use the same move each round. (Encore)
	/// <summary>
	public class PokeBattle_Move_0BC : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			List<Move.Effect> blacklist = new List<Move.Effect> {
		   (Move.Effect)0x02,    // Struggle
		   (Move.Effect)0x5C,    // Mimic
		   (Move.Effect)0x5D,    // Sketch
		   (Move.Effect)0x69,    // Transform
		   (Move.Effect)0xAE,    // Mirror Move
		   (Move.Effect)0xBC     // Encore
		};
			if (opponent.effects.Encore > 0)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			if (opponent.lastMoveUsed <= 0 ||
			   blacklist.Contains(new Move((Moves)opponent.lastMoveUsed).Function))
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			if (!attacker.hasMoldBreaker())
			{
				if (opponent.hasWorkingAbility(Abilities.AROMA_VEIL))
				{
					//battle.pbDisplay(_INTL("But it failed because of {1}'s {2}!",
					//   opponent.pbThis,PBAbilities.getName(opponent.Ability)));
					return -1;
				}
				else if (opponent.Partner.hasWorkingAbility(Abilities.AROMA_VEIL))
				{

					//battle.pbDisplay(_INTL("But it failed because of {1}'s {2}!",
					//   opponent.Partner.pbThis,PBAbilities.getName(opponent.Partner.Ability)));
					return -1;
				}
			}
			for (int i = 0; i < 4; i++)
			{
				if (opponent.lastMoveUsed == opponent.moves[i].MoveId &&
				   (opponent.moves[i].PP > 0 || opponent.moves[i].TotalPP == 0))
				{
					pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

					opponent.effects.Encore = 4;
					opponent.effects.EncoreIndex = i;
					opponent.effects.EncoreMove = opponent.moves[i].MoveId;

					//battle.pbDisplay(_INTL("{1} received an encore!", opponent.pbThis))
					return 0;
				}
			}

			//battle.pbDisplay(_INTL("But it failed!"))
			return -1;
		}
	}

	/// <summary>
	/// Hits twice.
	/// <summary>
	public class PokeBattle_Move_0BD : PokeBattle_Move
	{
		public object pbIsMultiHit()
		{
			return true;
		}

		public object pbNumHits(Battle.Battler attacker)
		{
			return 2;
		}
	}

	/// <summary>
	/// Hits twice. May poison the target on each hit. (Twineedle)
	/// <summary>
	public class PokeBattle_Move_0BE : PokeBattle_Move
	{
		public object pbIsMultiHit()
		{
			return true;
		}

		public object pbNumHits(Battle.Battler attacker)
		{
			return 2;
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			if (opponent.pbCanPoison(attacker, false, this))
			{
				opponent.pbPoison(attacker);
			}
		}
	}

	/// <summary>
	/// Hits 3 times. Power is multiplied by the hit number. (Triple Kick)
	/// An accuracy check is performed for each hit.
	/// <summary>
	public class PokeBattle_Move_0BF : PokeBattle_Move
	{
		public object pbIsMultiHit()
		{
			return true;
		}

		public object pbNumHits(Battle.Battler attacker)
		{
			return 3;
		}

		public bool successCheckPerHit()
		{
			return this.checks;
		}

		public object pbOnStartUse(Battle.Battler attacker)
		{

			this.calcbasedmg = this.basedamage;
			this.checks = !attacker.hasWorkingAbility(Abilities.SKILL_LINK);
			return true;
		}

		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{

			int ret = this.calcbasedmg;
			this.calcbasedmg += basedmg;
			return ret;
		}
	}

	/// <summary>
	/// Hits 2-5 times.
	/// <summary>
	public class PokeBattle_Move_0C0 : PokeBattle_Move
	{
		public object pbIsMultiHit()
		{
			return true;
		}

		public object pbNumHits(Battle.Battler attacker)
		{
			int[] hitchances = new int[] { 2, 2, 3, 3, 4, 5 };

			object ret = hitchances[this.battle.pbRandom(hitchances.Length)];
			if (attacker.hasWorkingAbility(Abilities.SKILL_LINK)) ret = 5;
			return ret;
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
		public object pbIsMultiHit()
		{
			return true;
		}

		public object pbNumHits(Battle.Battler attacker)
		{
			return this.participants.Count;
		}

		public object pbOnStartUse(Battle.Battler attacker)
		{

			Battle.Battler[] party = this.battle.pbParty(attacker.Index);
			this.participants = new List<byte>();
			for (byte i = 0; i < party.Length; i++)
			{
				if (attacker.pokemonIndex == i)
				{
					this.participants.Add(i);

				}
				else if (party[i].Species != Pokemons.NONE && !party[i].isEgg && party[i].HP > 0 && party[i].Status == 0)
				{
					this.participants.Add(i);
				}

			}
			if (this.participants.Count == 0)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return false;
			}
			return true;
		}

		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{

			Battle.Battler[] party = this.battle.pbParty(attacker.Index);
			byte atk = party[this.participants[0]].baseStats[1];

			this.participants.RemoveAt(0);//[0]=null; //this.participants.compact!;
			return 5 + (atk / 10);
		}
	}

	/// <summary>
	/// Two turn attack. Attacks first turn, skips second turn (if successful).
	/// <summary>
	public class PokeBattle_Move_0C2 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			object ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (opponent.damagestate.CalcDamage > 0)
			{
				attacker.effects.HyperBeam = 2;
				attacker.currentMove = this.id;
			}
			return ret;
		}
	}

	/// <summary>
	/// Two turn attack. Skips first turn, attacks second turn. (Razor Wind)
	/// <summary>
	public class PokeBattle_Move_0C3 : PokeBattle_Move
	{
		public object pbTwoTurnAttack(Battle.Battler attacker)
		{
			this.immediate = false;
			if (!this.immediate && attacker.hasWorkingItem(Items.POWER_HERB))
			{
				this.immediate = true;
			}
			if (this.immediate) return false;
			return attacker.effects.TwoTurnAttack == 0;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (this.immediate || attacker.effects.TwoTurnAttack > 0)
			{
				pbShowAnimation(this.id, attacker, opponent, 1, alltargets, showanimation); // Charging anim;
				//battle.pbDisplay(_INTL("{1} whipped up a whirlwind!",attacker.pbThis))
			}
			if (this.immediate)
			{
				this.battle.pbCommonAnimation("UseItem", attacker, null);

				//battle.pbDisplay(_INTL("{1} became fully charged due to its Power Herb!",attacker.pbThis))
				attacker.pbConsumeItem();
			}
			if (attacker.effects.TwoTurnAttack > 0) return 0;
			return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
		}
	}

	/// <summary>
	/// Two turn attack. Skips first turn, attacks second turn. (SolarBeam)
	/// Power halved in all weather except sunshine. In sunshine, takes 1 turn instead.
	/// <summary>
	public class PokeBattle_Move_0C4 : PokeBattle_Move
	{
		public object pbTwoTurnAttack(Battle.Battler attacker)
		{
			this.immediate = false; this.sunny = false;
			if (attacker.effects.TwoTurnAttack == 0)
			{
				if (this.battle.Weather == Weather.SUNNYDAY ||
				   this.battle.Weather == Weather.HARSHSUN)
				{
					this.immediate = true; this.sunny = true;
				}
			}
			if (!this.immediate && attacker.hasWorkingItem(Items.POWER_HERB))
			{
				this.immediate = true;
			}
			if (this.immediate) return false;
			return attacker.effects.TwoTurnAttack == 0;
		}

		public object pbBaseDamageMultiplier(int damagemult, Battle.Battler attacker, Battle.Battler opponent)
		{
			if (this.battle.Weather != 0 &&
			   this.battle.Weather != Weather.SUNNYDAY &&
			   this.battle.Weather != Weather.HARSHSUN)
			{
				return (int)Math.Round(damagemult * 0.5f);
			}
			return damagemult;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (this.immediate || attacker.effects.TwoTurnAttack > 0)
			{
				pbShowAnimation(this.id, attacker, opponent, 1, alltargets, showanimation); // Charging anim
				//battle.pbDisplay(_INTL("{1} took in sunlight!",attacker.pbThis))
			}
			if (this.immediate && !this.sunny)
			{
				this.battle.pbCommonAnimation("UseItem", attacker, null);

				//battle.pbDisplay(_INTL("{1} became fully charged due to its Power Herb!",attacker.pbThis))
				attacker.pbConsumeItem();
			}
			if (attacker.effects.TwoTurnAttack > 0) return 0;
			return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
		}
	}

	/// <summary>
	/// Two turn attack. Skips first turn, attacks second turn. (Freeze Shock)
	/// May paralyze the target.
	/// <summary>
	public class PokeBattle_Move_0C5 : PokeBattle_Move
	{
		public object pbTwoTurnAttack(Battle.Battler attacker)
		{
			this.immediate = false;
			if (!this.immediate && attacker.hasWorkingItem(Items.POWER_HERB))
			{
				this.immediate = true;
			}
			if (this.immediate) return false;
			return attacker.effects.TwoTurnAttack == 0;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (this.immediate || attacker.effects.TwoTurnAttack > 0)
			{
				pbShowAnimation(this.id, attacker, opponent, 1, alltargets, showanimation); // Charging anim
				//battle.pbDisplay(_INTL("{1} became cloaked in a freezing light!",attacker.pbThis))
			}
			if (this.immediate)
			{
				this.battle.pbCommonAnimation("UseItem", attacker, null);

				//battle.pbDisplay(_INTL("{1} became fully charged due to its Power Herb!",attacker.pbThis))
				attacker.pbConsumeItem();
			}
			if (attacker.effects.TwoTurnAttack > 0) return 0;
			return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			if (opponent.pbCanParalyze(attacker, false, this))
			{
				opponent.pbParalyze(attacker);
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
		{
			this.immediate = false;
			if (!this.immediate && attacker.hasWorkingItem(Items.POWER_HERB))
			{
				this.immediate = true;
			}
			if (this.immediate) return false;
			return attacker.effects.TwoTurnAttack == 0;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (this.immediate || attacker.effects.TwoTurnAttack > 0)
			{
				pbShowAnimation(this.id, attacker, opponent, 1, alltargets, showanimation); // Charging anim
				//battle.pbDisplay(_INTL("{1} became cloaked in freezing air!",attacker.pbThis))
			}
			if (this.immediate)
			{
				this.battle.pbCommonAnimation("UseItem", attacker, null);

				//battle.pbDisplay(_INTL("{1} became fully charged due to its Power Herb!",attacker.pbThis))
				attacker.pbConsumeItem();
			}
			if (attacker.effects.TwoTurnAttack > 0) return 0;
			return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			if (opponent.pbCanBurn(attacker, false, this))
			{
				opponent.pbBurn(attacker);
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
		{
			this.immediate = false;
			if (!this.immediate && attacker.hasWorkingItem(Items.POWER_HERB))
			{
				this.immediate = true;
			}
			if (this.immediate) return false;
			return attacker.effects.TwoTurnAttack == 0;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (this.immediate || attacker.effects.TwoTurnAttack > 0)
			{
				pbShowAnimation(this.id, attacker, opponent, 1, alltargets, showanimation); // Charging anim
				//battle.pbDisplay(_INTL("{1} became cloaked in a harsh light!",attacker.pbThis))
			}
			if (this.immediate)
			{
				this.battle.pbCommonAnimation("UseItem", attacker, null);

				//battle.pbDisplay(_INTL("{1} became fully charged due to its Power Herb!",attacker.pbThis))
				attacker.pbConsumeItem();
			}
			if (attacker.effects.TwoTurnAttack > 0) return 0;
			return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			opponent.pbFlinch(attacker);
		}
	}

	/// <summary>
	/// Two turn attack. Ups user's Defense by 1 stage first turn, attacks second turn.
	/// (Skull Bash)
	/// <summary>
	public class PokeBattle_Move_0C8 : PokeBattle_Move
	{
		public object pbTwoTurnAttack(Battle.Battler attacker)
		{
			this.immediate = false;
			if (!this.immediate && attacker.hasWorkingItem(Items.POWER_HERB))
			{
				this.immediate = true;
			}
			if (this.immediate) return false;
			return attacker.effects.TwoTurnAttack == 0;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (this.immediate || attacker.effects.TwoTurnAttack > 0)
			{
				pbShowAnimation(this.id, attacker, opponent, 1, alltargets, showanimation); // Charging anim
				//battle.pbDisplay(_INTL("{1} tucked in its head!",attacker.pbThis))
				if (attacker.pbCanIncreaseStatStage(Stats.DEFENSE, attacker, false, this))
				{
					attacker.pbIncreaseStat(Stats.DEFENSE, 1, attacker, false, this);
				}
			}
			if (this.immediate)
			{
				this.battle.pbCommonAnimation("UseItem", attacker, null);

				//battle.pbDisplay(_INTL("{1} became fully charged due to its Power Herb!",attacker.pbThis))
				attacker.pbConsumeItem();
			}
			if (attacker.effects.TwoTurnAttack > 0) return 0;
			return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
		}
	}

	/// <summary>
	/// Two turn attack. Skips first turn, attacks second turn. (Fly)
	/// (Handled in Battler's pbSuccessCheck): Is semi-invulnerable during use.
	/// <summary>
	public class PokeBattle_Move_0C9 : PokeBattle_Move
	{
		public bool unusableInGravity()
		{
			return true;
		}

		public object pbTwoTurnAttack(Battle.Battler attacker)
		{
			this.immediate = false;
			if (!this.immediate && attacker.hasWorkingItem(Items.POWER_HERB))
			{
				this.immediate = true;
			}
			if (this.immediate) return false;
			return attacker.effects.TwoTurnAttack == 0;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (this.immediate || attacker.effects.TwoTurnAttack > 0)
			{
				pbShowAnimation(this.id, attacker, opponent, 1, alltargets, showanimation); // Charging anim
				//battle.pbDisplay(_INTL("{1} flew up high!",attacker.pbThis))
			}
			if (this.immediate)
			{
				this.battle.pbCommonAnimation("UseItem", attacker, null);

				//battle.pbDisplay(_INTL("{1} became fully charged due to its Power Herb!",attacker.pbThis))
				attacker.pbConsumeItem();
			}
			if (attacker.effects.TwoTurnAttack > 0) return 0;
			return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
		}
	}

	/// <summary>
	/// Two turn attack. Skips first turn, attacks second turn. (Dig)
	/// (Handled in Battler's pbSuccessCheck): Is semi-invulnerable during use.
	/// <summary>
	public class PokeBattle_Move_0CA : PokeBattle_Move
	{
		public object pbTwoTurnAttack(Battle.Battler attacker)
		{
			this.immediate = false;
			if (!this.immediate && attacker.hasWorkingItem(Items.POWER_HERB))
			{
				this.immediate = true;
			}
			if (this.immediate) return false;
			return attacker.effects.TwoTurnAttack == 0;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (this.immediate || attacker.effects.TwoTurnAttack > 0)
			{
				pbShowAnimation(this.id, attacker, opponent, 1, alltargets, showanimation); // Charging anim
				//battle.pbDisplay(_INTL("{1} burrowed its way under the ground!",attacker.pbThis))
			}
			if (this.immediate)
			{
				this.battle.pbCommonAnimation("UseItem", attacker, null);

				//battle.pbDisplay(_INTL("{1} became fully charged due to its Power Herb!",attacker.pbThis))
				attacker.pbConsumeItem();
			}
			if (attacker.effects.TwoTurnAttack > 0) return 0;
			return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
		}
	}

	/// <summary>
	/// Two turn attack. Skips first turn, attacks second turn. (Dive)
	/// (Handled in Battler's pbSuccessCheck): Is semi-invulnerable during use.
	/// <summary>
	public class PokeBattle_Move_0CB : PokeBattle_Move
	{
		public object pbTwoTurnAttack(Battle.Battler attacker)
		{
			this.immediate = false;
			if (!this.immediate && attacker.hasWorkingItem(Items.POWER_HERB))
			{
				this.immediate = true;
			}
			if (this.immediate) return false;
			return attacker.effects.TwoTurnAttack == 0;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (this.immediate || attacker.effects.TwoTurnAttack > 0)
			{
				pbShowAnimation(this.id, attacker, opponent, 1, alltargets, showanimation); // Charging anim
				//battle.pbDisplay(_INTL("{1} hid underwater!",attacker.pbThis))
			}
			if (this.immediate)
			{
				this.battle.pbCommonAnimation("UseItem", attacker, null);

				//battle.pbDisplay(_INTL("{1} became fully charged due to its Power Herb!",attacker.pbThis))
				attacker.pbConsumeItem();
			}
			if (attacker.effects.TwoTurnAttack > 0) return 0;
			return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
		}
	}

	/// <summary>
	/// Two turn attack. Skips first turn, attacks second turn. (Bounce)
	/// May paralyze the target.
	/// (Handled in Battler's pbSuccessCheck): Is semi-invulnerable during use.
	/// <summary>
	public class PokeBattle_Move_0CC : PokeBattle_Move
	{
		public bool unusableInGravity()
		{
			return true;
		}

		public object pbTwoTurnAttack(Battle.Battler attacker)
		{
			this.immediate = false;
			if (!this.immediate && attacker.hasWorkingItem(Items.POWER_HERB))
			{
				this.immediate = true;
			}
			if (this.immediate) return false;
			return attacker.effects.TwoTurnAttack == 0;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (this.immediate || attacker.effects.TwoTurnAttack > 0)
			{
				pbShowAnimation(this.id, attacker, opponent, 1, alltargets, showanimation); // Charging anim
				//battle.pbDisplay(_INTL("{1} sprang up!",attacker.pbThis))
			}
			if (this.immediate)
			{
				this.battle.pbCommonAnimation("UseItem", attacker, null);

				//battle.pbDisplay(_INTL("{1} became fully charged due to its Power Herb!",attacker.pbThis))
				attacker.pbConsumeItem();
			}
			if (attacker.effects.TwoTurnAttack > 0) return 0;
			return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			if (opponent.pbCanParalyze(attacker, false, this))
			{
				opponent.pbParalyze(attacker);
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
		{
			this.immediate = false;
			if (!this.immediate && attacker.hasWorkingItem(Items.POWER_HERB))
			{
				this.immediate = true;
			}
			if (this.immediate) return false;
			return attacker.effects.TwoTurnAttack == 0;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (this.immediate || attacker.effects.TwoTurnAttack > 0)
			{
				pbShowAnimation(this.id, attacker, opponent, 1, alltargets, showanimation); // Charging anim
				//battle.pbDisplay(_INTL("{1} vanished instantly!",attacker.pbThis))
			}
			if (this.immediate)
			{
				this.battle.pbCommonAnimation("UseItem", attacker, null);

				//battle.pbDisplay(_INTL("{1} became fully charged due to its Power Herb!",attacker.pbThis))
				attacker.pbConsumeItem();
			}
			if (attacker.effects.TwoTurnAttack > 0) return 0;
			int ret = (int)base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (ret > 0)
			{
				opponent.effects.ProtectNegation = true;
				opponent.OwnSide.CraftyShield = false;
			}
			return ret;
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
		public bool unusableInGravity()
		{
			return true;
		}

		public object pbMoveFailed(Battle.Battler attacker, Battle.Battler opponent)
		{
			object ret = false;

			if (opponent.effects.Substitute > 0 && !ignoresSubstitute(attacker)) ret = true;
			if (opponent.effects.TwoTurnAttack > 0) ret = true;
			if (opponent.effects.SkyDrop && attacker.effects.TwoTurnAttack > 0) ret = true;
			if (!opponent.IsOpposing(attacker.Index)) ret = true;
			if (Settings.USENEWBATTLEMECHANICS && opponent.Weight(attacker) >= 2000) ret = true;
			return ret;
		}

		public object pbTwoTurnAttack(Battle.Battler attacker)
		{
			return attacker.effects.TwoTurnAttack == 0;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.effects.TwoTurnAttack > 0)
			{
				pbShowAnimation(this.id, attacker, opponent, 1, alltargets, showanimation); // Charging anim;
				//battle.pbDisplay(_INTL("{1} took {2} into the sky!",attacker.pbThis,opponent.pbThis(true)))
				opponent.effects.SkyDrop = true;
			}
			if (attacker.effects.TwoTurnAttack > 0) return 0;
			object ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			//battle.pbDisplay(_INTL("{1} was freed from the Sky Drop!", opponent.pbThis))
			opponent.effects.SkyDrop = false;
			return ret;
		}

		public override Types pbTypeModifier(Types type, Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.hasType(Types.FLYING)) return 0;
			if (!attacker.hasMoldBreaker() &&
			   opponent.hasWorkingAbility(Abilities.LEVITATE) &&
			   !opponent.effects.SmackDown) return 0;
			return base.pbTypeModifier(type, attacker, opponent);
		}
	}

	/// <summary>
	/// Trapping move. Traps for 5 or 6 rounds. Trapped Pokémon lose 1/16 of max HP
	/// at end of each round.
	/// <summary>
	public class PokeBattle_Move_0CF : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			object ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (opponent.damagestate.CalcDamage > 0 && !opponent.isFainted() &&
			   !opponent.damagestate.Substitute)
			{
				if (opponent.effects.MultiTurn == 0)
				{
					opponent.effects.MultiTurn = 5 + this.battle.pbRandom(2);
					if (attacker.hasWorkingItem(Items.GRIP_CLAW))
					{
						opponent.effects.MultiTurn = (Settings.USENEWBATTLEMECHANICS) ? 8 : 6;
					}
					opponent.effects.MultiTurnAttack = this.id;

					opponent.effects.MultiTurnUser = attacker.Index;
					if (id == Moves.BIND)
					{
						//battle.pbDisplay(_INTL("{1} was squeezed by {2}!",opponent.pbThis,attacker.pbThis(true)))
					}
					else if (id == Moves.CLAMP)
					{
						//battle.pbDisplay(_INTL("{1} clamped {2}!",attacker.pbThis,opponent.pbThis(true)))
					}
					else if (id == Moves.FIRE_SPIN)
					{
						//battle.pbDisplay(_INTL("{1} was trapped in the fiery vortex!",opponent.pbThis))
					}
					else if (id == Moves.MAGMA_STORM)
					{
						//battle.pbDisplay(_INTL("{1} became trapped by Magma Storm!",opponent.pbThis))
					}
					else if (id == Moves.SAND_TOMB)
					{
						//battle.pbDisplay(_INTL("{1} became trapped by Sand Tomb!",opponent.pbThis))
					}
					else if (id == Moves.WRAP)
					{
						//battle.pbDisplay(_INTL("{1} was wrapped by {2}!",opponent.pbThis,attacker.pbThis(true)))
					}
					else if (id == Moves.INFESTATION)
					{
						//battle.pbDisplay(_INTL("{1} has been afflicted with an infestation by {2}!",opponent.pbThis,attacker.pbThis(true)))
					}
					else
					{
						//battle.pbDisplay(_INTL("{1} was trapped in the vortex!",opponent.pbThis))
					}
				}

			}
			return ret;
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
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			object ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (opponent.damagestate.CalcDamage > 0 && !opponent.isFainted() &&
			   !opponent.damagestate.Substitute)
			{
				if (opponent.effects.MultiTurn == 0)
				{
					opponent.effects.MultiTurn = 5 + this.battle.pbRandom(2);
					if (attacker.hasWorkingItem(Items.GRIP_CLAW))
					{
						opponent.effects.MultiTurn = (Settings.USENEWBATTLEMECHANICS) ? 8 : 6;
					}
					opponent.effects.MultiTurnAttack = this.id;

					opponent.effects.MultiTurnUser = attacker.Index;

					//battle.pbDisplay(_INTL("{1} became trapped in the vortex!", opponent.pbThis))
				}
			}
			return ret;
		}

		public object pbModifyDamage(int damagemult, Battle.Battler attacker, Battle.Battler opponent)
		{
			if ((int)new Move((Moves)opponent.effects.TwoTurnAttack).Function == 0xCB)	// Dive
			{ 
				return (int)Math.Round(damagemult * 2.0f);
			}
			return damagemult;
		}
	}

	/// <summary>
	/// User must use this move for 2 more rounds. No battlers can sleep. (Uproar)
	/// <summary>
	public class PokeBattle_Move_0D1 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			object ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (opponent.damagestate.CalcDamage > 0)
			{
				if (attacker.effects.Uproar == 0)
				{
					attacker.effects.Uproar = 3;
					//battle.pbDisplay(_INTL("{1} caused an uproar!",attacker.pbThis))
					attacker.currentMove = this.id;
				}

			}
			return ret;
		}
	}

	/// <summary>
	/// User must use this move for 1 or 2 more rounds. At end, user becomes confused.
	/// (Outrage, Petal Dange, Thrash)
	/// <summary>
	public class PokeBattle_Move_0D2 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			object ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (opponent.damagestate.CalcDamage > 0 &&
			   attacker.effects.Outrage == 0 &&
			   attacker.Status != Status.SLEEP)
			{
				attacker.effects.Outrage = 2 + this.battle.pbRandom(2);

				attacker.currentMove = this.id;

			}
			else if (pbTypeModifier(this.type, attacker, opponent) == 0)
			{
				// Cancel effect if attack is ineffective
				attacker.effects.Outrage = 0;

			}
			if (attacker.effects.Outrage > 0)
			{

				attacker.effects.Outrage -= 1;
				if (attacker.effects.Outrage == 0 && attacker.pbCanConfuseSelf(false))
				{

					attacker.pbConfuse();
					//battle.pbDisplay(_INTL("{1} became confused due to fatigue!", attacker.pbThis))
				}
			}
			return ret;
		}
	}

	/// <summary>
	/// User must use this move for 4 more rounds. Power doubles each round.
	/// Power is also doubled if user has curled up. (Ice Ball, Rollout)
	/// <summary>
	public class PokeBattle_Move_0D3 : PokeBattle_Move
	{
		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{
			byte shift = (byte)(4 - attacker.effects.Rollout); // from 0 through 4, 0 is most powerful
			if (attacker.effects.DefenseCurl) shift += 1;
			basedmg = basedmg << shift;
			return basedmg;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{

			if (attacker.effects.Rollout == 0) attacker.effects.Rollout = 5;
			attacker.effects.Rollout -= 1;
			attacker.currentMove = thismove.MoveId;
			object ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (opponent.damagestate.CalcDamage == 0 ||
			   pbTypeModifier(this.type, attacker, opponent) == 0 ||
			   attacker.Status == Status.SLEEP)
			{
				// Cancel effect if attack is ineffective
				attacker.effects.Rollout = 0;

			}
			return ret;
		}
	}

	/// <summary>
	/// User bides its time this round and next round. The round after, deals 2x the
	/// total damage it took while biding to the last battler that damaged it. (Bide)
	/// <summary>
	public class PokeBattle_Move_0D4 : PokeBattle_Move
	{
		public override object pbDisplayUseMessage(Battle.Battler attacker)
		{
			if (attacker.effects.Bide == 0)
			{
				//battle.pbDisplayBrief(_INTL("{1} used\r\n{2}!",attacker.pbThis,name))
				attacker.effects.Bide = 2;
				attacker.effects.BideDamage = 0;
				attacker.effects.BideTarget = -1;
				attacker.currentMove = this.id;
				pbShowAnimation(this.id, attacker, null);
				return 1;
			}
			else
			{
				attacker.effects.Bide -= 1;
				if (attacker.effects.Bide == 0)
				{
					//battle.pbDisplayBrief(_INTL("{1} unleashed energy!",attacker.pbThis))
					return 0;
				}
				else
				{
					//battle.pbDisplayBrief(_INTL("{1} is storing energy!",attacker.pbThis))
					return 2;
				}
			}
		}

		public void pbAddTarget(byte targets, Battle.Battler attacker)
		{
			if (attacker.effects.BideTarget >= 0)
			{
				if (!attacker.pbAddTarget(targets, this.battle.battlers[attacker.effects.BideTarget]))
				{
					attacker.pbRandomTarget(targets);

				}
			}
			else
			{
				attacker.pbRandomTarget(targets);
			}
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.effects.BideDamage == 0 || opponent.Species == Pokemons.NONE)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			if (Settings.USENEWBATTLEMECHANICS)
			{
				Types typemod = pbTypeModifier(pbType(this.type, attacker, opponent), attacker, opponent);
				if (typemod == 0)
				{
					//battle.pbDisplay(_INTL("It doesn't affect {1}...",opponent.pbThis(true)))
					return -1;
				}
			}

			object ret = pbEffectFixedDamage(attacker.effects.BideDamage * 2, attacker, opponent, hitnum, alltargets, showanimation);
			return ret;
		}
	}

	/// <summary>
	/// Heals user by 1/2 of its max HP.
	/// <summary>
	public class PokeBattle_Move_0D5 : PokeBattle_Move
	{
		public bool isHealingMove()
		{
			return true;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.HP == attacker.TotalHP)
			{
				//battle.pbDisplay(_INTL("{1}'s HP is full!", attacker.pbThis))
				return -1;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			attacker.RecoverHP((int)Math.Floor((attacker.TotalHP + 1) / 2f), true);
			//battle.pbDisplay(_INTL("{1}'s HP was restored.",attacker.pbThis))
			return 0;
		}
	}

	/// <summary>
	/// Heals user by 1/2 of its max HP. (Roost)
	/// User roosts, and its Flying type is ignored for attacks used against it.
	/// <summary>
	public class PokeBattle_Move_0D6 : PokeBattle_Move
	{
		public bool isHealingMove()
		{
			return true;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.HP == attacker.TotalHP)
			{
				//battle.pbDisplay(_INTL("{1}'s HP is full!", attacker.pbThis))
				return -1;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			attacker.RecoverHP((int)Math.Floor((attacker.TotalHP + 1) / 2f), true);
			attacker.effects.Roost = true;
			//battle.pbDisplay(_INTL("{1}'s HP was restored.",attacker.pbThis))
			return 0;
		}
	}

	/// <summary>
	/// Battler in user's position is healed by 1/2 of its max HP, at the end of the
	/// next round. (Wish)
	/// <summary>
	public class PokeBattle_Move_0D7 : PokeBattle_Move
	{
		public bool isHealingMove()
		{
			return true;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.effects.Wish > 0)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			attacker.effects.Wish = 2;
			attacker.effects.WishAmount = (int)Math.Floor((attacker.TotalHP + 1) / 2f);
			attacker.effects.WishMaker = attacker.pokemonIndex;
			return 0;
		}
	}

	/// <summary>
	/// Heals user by an amount depending on the weather. (Moonlight, Morning Sun,
	/// Synthesis)
	/// <summary>
	public class PokeBattle_Move_0D8 : PokeBattle_Move
	{
		public bool isHealingMove()
		{
			return true;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.HP == attacker.TotalHP)
			{
				//battle.pbDisplay(_INTL("{1}'s HP is full!", attacker.pbThis))
				return -1;
			}
			int hpgain = 0;
			if (this.battle.Weather == Weather.SUNNYDAY ||
			   this.battle.Weather == Weather.HARSHSUN)
			{
				hpgain = (int)Math.Floor(attacker.TotalHP * 2 / 3f);

			}
			else if (this.battle.Weather != 0)
			{
				hpgain = (int)Math.Floor(attacker.TotalHP / 4f);
			}
			else
			{

				hpgain = (int)Math.Floor(attacker.TotalHP / 2f);

			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);
			attacker.RecoverHP(hpgain, true);

			//battle.pbDisplay(_INTL("{1}'s HP was restored.",attacker.pbThis))
			return 0;
		}
	}

	/// <summary>
	/// Heals user to full HP. User falls asleep for 2 more rounds. (Rest)
	/// <summary>
	public class PokeBattle_Move_0D9 : PokeBattle_Move
	{
		public bool isHealingMove()
		{
			return true;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (!attacker.pbCanSleep(attacker, true, this, true))
			{
				return -1;
			}
			if (attacker.Status == Status.SLEEP)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			if (attacker.HP == attacker.TotalHP)
			{
				//battle.pbDisplay(_INTL("{1}'s HP is full!", attacker.pbThis))
				return -1;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			attacker.pbSleepSelf(3);
			//battle.pbDisplay(_INTL("{1} slept and became healthy!",attacker.pbThis))
			int hp = attacker.RecoverHP(attacker.TotalHP - attacker.HP, true);
			//battle.pbDisplay(_INTL("{1}'s HP was restored.",attacker.pbThis)) if hp>0
			return 0;
		}
	}

	/// <summary>
	/// Rings the user. Ringed Pokémon gain 1/16 of max HP at the end of each round.
	/// (Aqua Ring)
	/// <summary>
	public class PokeBattle_Move_0DA : PokeBattle_Move
	{
		public bool isHealingMove()
		{
			return true;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.effects.AquaRing)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			attacker.effects.AquaRing = true;
			//battle.pbDisplay(_INTL("{1} surrounded itself with a veil of water!",attacker.pbThis))
			return 0;
		}
	}

	/// <summary>
	/// Ingrains the user. Ingrained Pokémon gain 1/16 of max HP at the end of each
	/// round, and cannot flee or switch out. (Ingrain)
	/// <summary>
	public class PokeBattle_Move_0DB : PokeBattle_Move
	{
		public bool isHealingMove()
		{
			return true;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.effects.Ingrain)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			attacker.effects.Ingrain = true;
			//battle.pbDisplay(_INTL("{1} planted its roots!",attacker.pbThis))
			return 0;
		}
	}

	/// <summary>
	/// Seeds the target. Seeded Pokémon lose 1/8 of max HP at the end of each round,
	/// and the Pokémon in the user's position gains the same amount. (Leech Seed)
	/// <summary>
	public class PokeBattle_Move_0DC : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (opponent.effects.Substitute > 0 && !ignoresSubstitute(attacker))
			{
				//battle.pbDisplay(_INTL("But it failed!"))  
				return -1;
			}
			if (pbTypeImmunityByAbility(pbType(this.type, attacker, opponent), attacker, opponent)) return -1;
			if (opponent.effects.LeechSeed >= 0)
			{
				//battle.pbDisplay(_INTL("{1} evaded the attack!",opponent.pbThis))
				return -1;
			}
			if (opponent.hasType(Types.GRASS))
			{
				//battle.pbDisplay(_INTL("It doesn't affect {1}...",opponent.pbThis(true)))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			opponent.effects.LeechSeed = attacker.Index;
			//battle.pbDisplay(_INTL("{1} was seeded!", opponent.pbThis))
			return 0;
		}
	}

	/// <summary>
	/// User gains half the HP it inflicts as damage.
	/// <summary>
	public class PokeBattle_Move_0DD : PokeBattle_Move
	{
		public bool isHealingMove()
		{
			return Settings.USENEWBATTLEMECHANICS;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			object ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (opponent.damagestate.CalcDamage > 0)
			{
				int hpgain = (int)Math.Round(opponent.damagestate.HPLost / 2f);
				if (opponent.hasWorkingAbility(Abilities.LIQUID_OOZE))
				{
					attacker.ReduceHP(hpgain, true);
					//battle.pbDisplay(_INTL("{1} sucked up the liquid ooze!",attacker.pbThis))
				}
				else if (attacker.effects.HealBlock == 0)
				{

					if (attacker.hasWorkingItem(Items.BIG_ROOT)) hpgain = (int)Math.Floor(hpgain * 1.3f);

					attacker.RecoverHP(hpgain, true);
					//battle.pbDisplay(_INTL("{1} had its energy drained!",opponent.pbThis))
				}
			}
			return ret;
		}
	}

	/// <summary>
	/// User gains half the HP it inflicts as damage. (Dream Eater)
	/// (Handled in Battler's pbSuccessCheck): Fails if target is not asleep.
	/// <summary>
	public class PokeBattle_Move_0DE : PokeBattle_Move
	{
		public bool isHealingMove()
		{
			return Settings.USENEWBATTLEMECHANICS;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			object ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (opponent.damagestate.CalcDamage > 0)
			{
				int hpgain = (int)Math.Round(opponent.damagestate.HPLost / 2f);
				if (opponent.hasWorkingAbility(Abilities.LIQUID_OOZE))
				{
					attacker.ReduceHP(hpgain, true);
					//battle.pbDisplay(_INTL("{1} sucked up the liquid ooze!",attacker.pbThis))
				}
				else if (attacker.effects.HealBlock == 0)
				{

					if (attacker.hasWorkingItem(Items.BIG_ROOT)) hpgain = (int)Math.Floor(hpgain * 1.3f);

					attacker.RecoverHP(hpgain, true);
					//battle.pbDisplay(_INTL("{1} had its energy drained!",opponent.pbThis))
				}
			}
			return ret;
		}
	}

	/// <summary>
	/// Heals target by 1/2 of its max HP. (Heal Pulse)
	/// <summary>
	public class PokeBattle_Move_0DF : PokeBattle_Move
	{
		public bool isHealingMove()
		{
			return true;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (opponent.effects.Substitute > 0 && !ignoresSubstitute(attacker))
			{
				//battle.pbDisplay(_INTL("But it failed!"))  
				return -1;
			}
			if (opponent.HP == opponent.TotalHP)
			{
				//battle.pbDisplay(_INTL("{1}'s HP is full!", opponent.pbThis))  
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			int hpgain = (int)Math.Floor((opponent.TotalHP + 1) / 2f);
			if (attacker.hasWorkingAbility(Abilities.MEGA_LAUNCHER)) hpgain = (int)Math.Round(opponent.TotalHP * 3 / 4f);
			opponent.RecoverHP(hpgain, true);
			//battle.pbDisplay(_INTL("{1}'s HP was restored.",opponent.pbThis))  
			return 0;
		}
	}

	/// <summary>
	/// User faints. (Explosion, Selfdestruct)
	/// <summary>
	public class PokeBattle_Move_0E0 : PokeBattle_Move
	{
		public object pbOnStartUse(Battle.Battler attacker)
		{
			if (!attacker.hasMoldBreaker())
			{
				Battle.Battler bearer = this.battle.pbCheckGlobalAbility(Abilities.DAMP);
				if (bearer != null)
				{
					//battle.pbDisplay(_INTL("{1}'s {2} prevents {3} from using {4}!",
					//   bearer.pbThis, PBAbilities.getName(bearer.Ability), attacker.pbThis(true),this.name))
					return false;
				}
			}
			return true;
		}

		public override IEnumerator pbShowAnimation(Moves id, Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{

			base.pbShowAnimation(id, attacker, opponent, hitnum, alltargets, showanimation);
			if (!attacker.isFainted())
			{
				attacker.ReduceHP(attacker.HP);

				if (attacker.isFainted()) attacker.pbFaint();
			}
			return null;//ToDo: Added this here, double check to see if there better options 
		}
	}

	/// <summary>
	/// Inflicts fixed damage equal to user's current HP. (Final Gambit)
	/// User faints (if successful).
	/// <summary>
	public class PokeBattle_Move_0E1 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			Types typemod = pbTypeModifier(pbType(this.type, attacker, opponent), attacker, opponent);
			if (typemod == 0)
			{
				//battle.pbDisplay(_INTL("It doesn't affect {1}...",opponent.pbThis(true)))
				return -1;
			}
			object ret = pbEffectFixedDamage(attacker.HP, attacker, opponent, hitnum, alltargets, showanimation);
			return ret;
		}

		public override IEnumerator pbShowAnimation(Moves id, Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{

			base.pbShowAnimation(id, attacker, opponent, hitnum, alltargets, showanimation);
			if (!attacker.isFainted())
			{
				attacker.ReduceHP(attacker.HP);

				if (attacker.isFainted()) attacker.pbFaint();
			}
			return null;//ToDo: Added this here, double check to see if there better options 
		}
	}

	/// <summary>
	/// Decreases the target's Attack and Special Attack by 2 stages each. (Memento)
	/// User faints (even if effect does nothing).
	/// <summary>
	public class PokeBattle_Move_0E2 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (opponent.effects.Substitute > 0 && !ignoresSubstitute(attacker))
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			object ret = -1; bool showanim = true;
			if (opponent.pbReduceStat(Stats.ATTACK, 2, attacker, false, this, showanim))
			{
				ret = 0; showanim = false;
			}
			if (opponent.pbReduceStat(Stats.SPATK, 2, attacker, false, this, showanim))
			{
				ret = 0; showanim = false;
			}
			attacker.ReduceHP(attacker.HP);
			return ret;
		}
	}

	/// <summary>
	/// User faints. The Pokémon that replaces the user is fully healed (HP and
	/// status). Fails if user won't be replaced. (Healing Wish)
	/// <summary>
	public class PokeBattle_Move_0E3 : PokeBattle_Move
	{
		public bool isHealingMove()
		{
			return true;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (!this.battle.pbCanChooseNonActive(attacker.Index))
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			attacker.ReduceHP(attacker.HP);
			attacker.effects.HealingWish = true;
			return 0;
		}
	}

	/// <summary>
	/// User faints. The Pokémon that replaces the user is fully healed (HP, PP and
	/// status). Fails if user won't be replaced. (Lunar Dance)
	/// <summary>
	public class PokeBattle_Move_0E4 : PokeBattle_Move
	{
		public bool isHealingMove()
		{
			return true;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (!this.battle.pbCanChooseNonActive(attacker.Index))
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			attacker.ReduceHP(attacker.HP);
			attacker.effects.LunarDance = true;
			return 0;
		}
	}

	/// <summary>
	/// All current battlers will perish after 3 more rounds. (Perish Song)
	/// <summary>
	public class PokeBattle_Move_0E5 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			bool failed = true;
			for (int i = 0; i < 4; i++)
			{
				if (this.battle.battlers[i].effects.PerishSong == 0 &&
				   (attacker.hasMoldBreaker() ||
				   !this.battle.battlers[i].hasWorkingAbility(Abilities.SOUNDPROOF)))
				{
					failed = false; break;
				}
			}
			if (failed)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			//battle.pbDisplay(_INTL("All Pokémon that hear the song will faint in three turns!"))
			for (int i = 0; i < 4; i++)
			{
				if (this.battle.battlers[i].effects.PerishSong == 0)
				{
					if (!attacker.hasMoldBreaker() && this.battle.battlers[i].hasWorkingAbility(Abilities.SOUNDPROOF))
					{

						//battle.pbDisplay(_INTL("{1}'s {2} blocks {3}!",this.battle.battlers[i].pbThis,
						//	 PBAbilities.getName(this.battle.battlers[i].Ability),this.name))
					}
					else
					{
						this.battle.battlers[i].effects.PerishSong = 4;
						this.battle.battlers[i].effects.PerishSongUser = attacker.Index;
					}

				}
			}
			return 0;
		}
	}

	/// <summary>
	/// If user is KO'd before it next moves, the attack that caused it loses all PP.
	/// (Grudge)
	/// <summary>
	public class PokeBattle_Move_0E6 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			attacker.effects.Grudge = true;
			//battle.pbDisplay(_INTL("{1} wants its target to bear a grudge!",attacker.pbThis))
			return 0;
		}
	}

	/// <summary>
	/// If user is KO'd before it next moves, the battler that caused it also faints.
	/// (Destiny Bond)
	/// <summary>
	public class PokeBattle_Move_0E7 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			attacker.effects.DestinyBond = true;
			//battle.pbDisplay(_INTL("{1} is trying to take its foe down with it!",attacker.pbThis))
			return 0;
		}
	}

	/// <summary>
	/// If user would be KO'd this round, it survives with 1HP instead. (Endure)
	/// <summary>
	public class PokeBattle_Move_0E8 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			List<Move.Effect> ratesharers = new List<Move.Effect> {
		   (Move.Effect)0xAA,   // Detect, Protect
		   (Move.Effect)0xAB,   // Quick Guard
		   (Move.Effect)0xAC,   // Wide Guard
		   (Move.Effect)0xE8,   // Endure
		   (Move.Effect)0x14B,  // King's Shield
		   (Move.Effect)0x14C   // Spiky Shield
		};
			if (!ratesharers.Contains(new Move((Moves)attacker.lastMoveUsed).Function))
			{
				attacker.effects.ProtectRate = 1;
			}
			bool unmoved = false;
			foreach (Battle.Battler poke in this.battle.battlers)
			{
				if (poke.Index == attacker.Index) continue; //next
				if ((int)this.battle.choices[poke.Index].Action == 1 && // Chose a move
				   !poke.hasMovedThisRound())
				{
					unmoved = true; break;
				}
			}
			if (!unmoved ||
			   this.battle.pbRandom(65536) > Math.Floor(65536f / attacker.effects.ProtectRate))
			{
				attacker.effects.ProtectRate = 1;

				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			attacker.effects.Endure = true;
			attacker.effects.ProtectRate *= 2;
			//battle.pbDisplay(_INTL("{1} braced itself!",attacker.pbThis))
			return 0;
		}
	}

	/// <summary>
	/// If target would be KO'd by this attack, it survives with 1HP instead. (False Swipe)
	/// <summary>
	public class PokeBattle_Move_0E9 : PokeBattle_Move
	{
		// Handled in superclass public object ReduceHPDamage, do not edit!
	}

	/// <summary>
	/// User flees from battle. Fails in trainer battles. (Teleport)
	/// <summary>
	public class PokeBattle_Move_0EA : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (this.battle.opponent.ID != TrainerTypes.WildPokemon ||
			   !this.battle.pbCanRun(attacker.Index))
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			//battle.pbDisplay(_INTL("{1} fled from battle!",attacker.pbThis))
			this.battle.decision = (Battle.BattleResults)3;
			return 0;
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
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (!attacker.hasMoldBreaker() && opponent.hasWorkingAbility(Abilities.SUCTION_CUPS))
			{
				//battle.pbDisplay(_INTL("{1} anchored itself with {2}!",opponent.pbThis,PBAbilities.getName(opponent.Ability)))  
				return -1;
			}
			if (opponent.effects.Ingrain)
			{
				//battle.pbDisplay(_INTL("{1} anchored itself with its roots!", opponent.pbThis))  
				return -1;
			}
			if (this.battle.opponent.ID == TrainerTypes.WildPokemon)
			{
				if (opponent.level > attacker.level)
				{
					//battle.pbDisplay(_INTL("But it failed!"))
					return -1;
				}
				pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

				this.battle.decision = (Battle.BattleResults)3; // Set decision to escaped;
				return 0;
			}
			else
			{
				bool choices = false;
				Battle.Battler[] party = this.battle.pbParty(opponent.Index);
				for (int i = 0; i < party.Length; i++)
				{
					if (this.battle.pbCanSwitch(opponent.Index, i, false, true))
					{
						choices = true;
						break;
					}
				}
				if (!choices)
				{
					//battle.pbDisplay(_INTL("But it failed!"))
					return -1;
				}
				pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

				opponent.effects.Roar = true;
				return 0;
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
		public void pbEffectAfterHit(Battle.Battler attacker, Battle.Battler opponent, Effects.Move turneffects)
		{
			if (!attacker.isFainted() && !opponent.isFainted() &&
			   opponent.damagestate.CalcDamage > 0 && !opponent.damagestate.Substitute &&
			   (attacker.hasMoldBreaker() || !opponent.hasWorkingAbility(Abilities.SUCTION_CUPS)) &&
			   !opponent.effects.Ingrain)
			{
				if (this.battle.opponent.ID == TrainerTypes.WildPokemon)//Wild Pokemon Battle
				{
					if (opponent.level <= attacker.level)
					{
						this.battle.decision = (Battle.BattleResults)3; // Set decision to escaped;
					}
				}
				else
				{

					Battle.Battler[] party = this.battle.pbParty(opponent.Index);
					for (int i = 0; i < party.Length - 1; i++)	//ToDo: Double check this
					{ 
						if (this.battle.pbCanSwitch(opponent.Index, i, false))
						{

							opponent.effects.Roar = true;
							break;
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
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (!this.battle.pbCanChooseNonActive(attacker.Index))
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			attacker.effects.BatonPass = true;
			return 0;
		}
	}

	/// <summary>
	/// After inflicting damage, user switches out. Ignores trapping moves.
	/// (U-turn, Volt Switch)
	/// TODO: Pursuit should interrupt this move.
	/// <summary>
	public class PokeBattle_Move_0EE : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			object ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (!attacker.isFainted() && opponent.damagestate.CalcDamage > 0 &&
			   this.battle.pbCanChooseNonActive(attacker.Index) &&
			   !this.battle.pbAllFainted(this.battle.pbParty(opponent.Index)))
			{
				attacker.effects.Uturn = true;
			}
			return ret;
		}
	}

	/// <summary>
	/// Target can no longer switch out or flee, as long as the user remains active.
	/// (Block, Mean Look, Spider Web, Thousand Waves)
	/// <summary>
	public class PokeBattle_Move_0EF : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (pbIsDamaging())
			{
				object ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
				if (opponent.damagestate.CalcDamage > 0 && !opponent.damagestate.Substitute &&
				   !opponent.isFainted())
				{
					if (opponent.effects.MeanLook < 0 &&
					   (!Settings.USENEWBATTLEMECHANICS || !opponent.hasType(Types.GHOST)))
					{
						opponent.effects.MeanLook = attacker.Index;
						//battle.pbDisplay(_INTL("{1} can no longer escape!", opponent.pbThis))
					}
				}
				return ret;
			}
			if (opponent.effects.MeanLook >= 0 ||
			   (opponent.effects.Substitute > 0 && !ignoresSubstitute(attacker)))
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			if (Settings.USENEWBATTLEMECHANICS && opponent.hasType(Types.GHOST))
			{
				//battle.pbDisplay(_INTL("It doesn't affect {1}...",opponent.pbThis(true)))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			opponent.effects.MeanLook = attacker.Index;
			//battle.pbDisplay(_INTL("{1} can no longer escape!", opponent.pbThis))
			return 0;
		}
	}

	/// <summary>
	/// Target drops its item. It regains the item at the end of the battle. (Knock Off)
	/// If target has a losable item, damage is multiplied by 1.5.
	/// <summary>
	public class PokeBattle_Move_0F0 : PokeBattle_Move
	{
		public void pbEffectAfterHit(Battle.Battler attacker, Battle.Battler opponent, Effects.Move turneffects)
		{
			if (!attacker.isFainted() && !opponent.isFainted() && opponent.Item != 0 &&
			   opponent.damagestate.CalcDamage > 0 && !opponent.damagestate.Substitute)
			{
				if (!attacker.hasMoldBreaker() && opponent.hasWorkingAbility(Abilities.STICKY_HOLD))
				{
					string abilityname = PBAbilities.getName(opponent.Ability);
					//battle.pbDisplay(_INTL("{1}'s {2} made {3} ineffective!", opponent.pbThis, abilityname, this.name))
				}
				else if (!this.battle.pbIsUnlosableItem(opponent, opponent.Item))
				{
					string itemname = PBItems.getName(opponent.Item);

					opponent.Item = 0;
					opponent.effects.ChoiceBand = Moves.NONE;//-1;
					opponent.effects.Unburden = true;
					//battle.pbDisplay(_INTL("{1} dropped its {2}!",opponent.pbThis,itemname))
				}
			}
		}

		public object pbModifyDamage(int damagemult, Battle.Battler attacker, Battle.Battler opponent)
		{
			if (Settings.USENEWBATTLEMECHANICS &&
			   !this.battle.pbIsUnlosableItem(opponent, opponent.Item))
			{
				// Still boosts damage even if opponent has Sticky Hold
				return (int)Math.Round(damagemult * 1.5f);
			}
			return damagemult;
		}
	}

	/// <summary>
	/// User steals the target's item, if the user has none itself. (Covet, Thief)
	/// Items stolen from wild Pokémon are kept after the battle.
	/// <summary>
	public class PokeBattle_Move_0F1 : PokeBattle_Move
	{
		public void pbEffectAfterHit(Battle.Battler attacker, Battle.Battler opponent, Effects.Move turneffects)
		{
			if (!attacker.isFainted() && !opponent.isFainted() && opponent.Item != 0 &&
			   opponent.damagestate.CalcDamage > 0 && !opponent.damagestate.Substitute)
			{
				if (!attacker.hasMoldBreaker() && opponent.hasWorkingAbility(Abilities.STICKY_HOLD))
				{
					string abilityname = PBAbilities.getName(opponent.Ability);
					//battle.pbDisplay(_INTL("{1}'s {2} made {3} ineffective!", opponent.pbThis, abilityname, this.name))
				}
				else if (!this.battle.pbIsUnlosableItem(opponent, opponent.Item) &&
					!this.battle.pbIsUnlosableItem(attacker, opponent.Item) &&
					attacker.Item == 0 &&
					(this.battle.opponent.ID == TrainerTypes.WildPokemon || !this.battle.isOpposing(attacker.Index)))
				{
					string itemname = PBItems.getName(opponent.Item);
					attacker.Item = opponent.Item;
					opponent.Item = 0;

					opponent.effects.ChoiceBand = Moves.NONE;//-1;

					opponent.effects.Unburden = true;
					if (this.battle.opponent.ID == TrainerTypes.WildPokemon && // In a wild battle
					   attacker.itemInitial == 0 &&

					   opponent.itemInitial == attacker.Item)
					{
						attacker.itemInitial = attacker.Item;

						opponent.itemInitial = 0;

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
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if ((opponent.effects.Substitute > 0 && !ignoresSubstitute(attacker)) ||
			   (attacker.Item == 0 && opponent.Item == 0) ||
			   (this.battle.opponent.ID == TrainerTypes.WildPokemon && this.battle.isOpposing(attacker.Index)))
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			if (this.battle.pbIsUnlosableItem(opponent, opponent.Item) ||
			   this.battle.pbIsUnlosableItem(attacker, opponent.Item) ||
			   this.battle.pbIsUnlosableItem(opponent, attacker.Item) ||
			   this.battle.pbIsUnlosableItem(attacker, attacker.Item))
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			if (!attacker.hasMoldBreaker() && opponent.hasWorkingAbility(Abilities.STICKY_HOLD))
			{
				string abilityname = PBAbilities.getName(opponent.Ability);
				//battle.pbDisplay(_INTL("{1}'s {2} made {3} ineffective!", opponent.pbThis, abilityname, name))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			Items oldattitem = attacker.Item;
			Items oldoppitem = opponent.Item;

			string oldattitemname = PBItems.getName(oldattitem);
			string oldoppitemname = PBItems.getName(oldoppitem);

			Items tmpitem = attacker.Item;
			attacker.Item = opponent.Item;
			opponent.Item = tmpitem;
			if (this.battle.opponent.ID == TrainerTypes.WildPokemon && // In a wild battle
			   attacker.itemInitial == oldattitem &&

			   opponent.itemInitial == oldoppitem)
			{
				attacker.itemInitial = oldoppitem;

				opponent.itemInitial = oldattitem;

			}
			//battle.pbDisplay(_INTL("{1} switched items with its opponent!", attacker.pbThis))
			if (oldoppitem > 0 && oldattitem > 0)
			{
				//battle.pbDisplayPaused(_INTL("{1} obtained {2}.",attacker.pbThis,oldoppitemname))
				//battle.pbDisplay(_INTL("{1} obtained {2}.",opponent.pbThis,oldattitemname))
			}
			else
			{
				//battle.pbDisplay(_INTL("{1} obtained {2}.",attacker.pbThis,oldoppitemname)) if (oldoppitem>0)
				//battle.pbDisplay(_INTL("{1} obtained {2}.",opponent.pbThis,oldattitemname)) if (oldattitem>0)
			}
			attacker.effects.ChoiceBand = Moves.NONE;//-1;

			opponent.effects.ChoiceBand = Moves.NONE;//-1;
			return 0;
		}
	}

	/// <summary>
	/// User gives its item to the target. The item remains given after wild battles.
	/// (Bestow)
	/// <summary>
	public class PokeBattle_Move_0F3 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if ((opponent.effects.Substitute > 0 && !ignoresSubstitute(attacker)) ||
			   attacker.Item == 0 || opponent.Item != 0)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			if (this.battle.pbIsUnlosableItem(attacker, attacker.Item) ||
			   this.battle.pbIsUnlosableItem(opponent, attacker.Item))
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			string itemname = PBItems.getName(attacker.Item);
			opponent.Item = attacker.Item;
			attacker.Item = 0;

			attacker.effects.ChoiceBand = Moves.NONE;//-1;

			attacker.effects.Unburden = true;
			if (this.battle.opponent.ID == TrainerTypes.WildPokemon && // In a wild battle
			   opponent.itemInitial == 0 &&

			   attacker.itemInitial == opponent.Item)
			{
				opponent.itemInitial = opponent.Item;

				attacker.itemInitial = 0;

			}
			//battle.pbDisplay(_INTL("{1} received {2} from {3}!", opponent.pbThis, itemname, attacker.pbThis(true)))
			return 0;
		}
	}

	/// <summary>
	/// User consumes target's berry and gains its effect. (Bug Bite, Pluck)
	/// <summary>
	public class PokeBattle_Move_0F4 : PokeBattle_Move
	{
		public void pbEffectAfterHit(Battle.Battler attacker, Battle.Battler opponent, Effects.Move turneffects)
		{
			if (!attacker.isFainted() && !opponent.isFainted() && pbIsBerry(opponent.Item) &&
			   opponent.damagestate.CalcDamage > 0 && !opponent.damagestate.Substitute)
			{
				if (attacker.hasMoldBreaker() || !opponent.hasWorkingAbility(Abilities.STICKY_HOLD))
				{
					Items item = opponent.Item;
					string itemname = PBItems.getName(item);

					opponent.pbConsumeItem(false, false);
					//battle.pbDisplay(_INTL("{1} stole and ate its target's {2}!",attacker.pbThis,itemname))
					if (!attacker.hasWorkingAbility(Abilities.KLUTZ) &&
					   attacker.effects.Embargo == 0)
					{
						attacker.pbActivateBerryEffect(item, false);
					}
					// Symbiosis
					if (attacker.Item == 0 &&
					   attacker.Partner.Species != Pokemons.NONE && attacker.Partner.hasWorkingAbility(Abilities.SYMBIOSIS))
					{
						Battle.Battler partner = attacker.Partner;
						if (partner.Item > 0 &&
						   !this.battle.pbIsUnlosableItem(partner, partner.Item) &&
						   !this.battle.pbIsUnlosableItem(attacker, partner.Item))
						{
							//battle.pbDisplay(_INTL("{1}'s {2} let it share its {3} with {4}!",
							//   partner.pbThis, PBAbilities.getName(partner.Ability),
							//   PBItems.getName(partner.item), attacker.pbThis(true)));
							attacker.Item = partner.Item;
							partner.Item = 0;
							partner.effects.Unburden = true;

							attacker.pbBerryCureCheck();
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
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			object ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (!attacker.isFainted() && opponent.damagestate.CalcDamage > 0 &&
			   !opponent.damagestate.Substitute &&
			   (pbIsBerry(opponent.Item) || (Settings.USENEWBATTLEMECHANICS && pbIsGem(opponent.Item))))
			{
				string itemname = PBItems.getName(opponent.Item);
				opponent.pbConsumeItem(false, false);

				//battle.pbDisplay(_INTL("{1}'s {2} was incinerated!",opponent.pbThis,itemname))
			}
			return ret;
		}
	}

	/// <summary>
	/// User recovers the last item it held and consumed. (Recycle)
	/// <summary>
	public class PokeBattle_Move_0F6 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.Species == Pokemons.NONE || attacker.itemRecycle == 0)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			Items item = attacker.itemRecycle;
			string itemname = PBItems.getName(item);

			attacker.Item = item;
			if (this.battle.opponent.ID == TrainerTypes.WildPokemon)	// In a wild battle
			{ 
				if (attacker.itemInitial == 0) attacker.itemInitial = item;

			}
			attacker.itemRecycle = 0;

			attacker.effects.PickupItem = 0;

			attacker.effects.PickupUse = 0;

			//battle.pbDisplay(_INTL("{1} found one {2}!", attacker.pbThis, itemname))
			return 0;
		}
	}

	/// <summary>
	/// User flings its item at the target. Power and effect depend on the item. (Fling)
	/// <summary>
	public class PokeBattle_Move_0F7 : PokeBattle_Move
	{
		public Dictionary<Items, byte> flingarray
		{
			get
			{
				return new Dictionary<Items, byte> { 
		   //130 => 
					{ Items.IRON_BALL, 130 },
		   //100 => 
					{ Items.ARMOR_FOSSIL, 100 },    { Items.CLAW_FOSSIL, 100 }, { Items.COVER_FOSSIL, 100 },    { Items.DOME_FOSSIL, 100 }, { Items.HARD_STONE, 100 },
					{ Items.HELIX_FOSSIL, 100 },    { Items.JAW_FOSSIL, 100 },  { Items.OLD_AMBER, 100 },   { Items.PLUME_FOSSIL, 100 },    { Items.RARE_BONE, 100 },
					{ Items.ROOT_FOSSIL, 100 }, { Items.SAIL_FOSSIL, 100 }, { Items.SKULL_FOSSIL, 100 },	
			//90 => 
					{ Items.DEEP_SEA_TOOTH, 90 },   { Items.DRACO_PLATE, 90 },  { Items.DREAD_PLATE, 90 },  { Items.EARTH_PLATE, 90 },  { Items.FIST_PLATE, 90 },
					{ Items.FLAME_PLATE, 90 },  { Items.GRIP_CLAW, 90 },        { Items.ICICLE_PLATE, 90 }, { Items.INSECT_PLATE, 90 }, { Items.IRON_PLATE, 90 },
					{ Items.MEADOW_PLATE, 90 }, { Items.MIND_PLATE, 90 },   { Items.PIXIE_PLATE, 90 },  { Items.SKY_PLATE, 90 },        { Items.SPLASH_PLATE, 90 },
					{ Items.SPOOKY_PLATE, 90 }, { Items.STONE_PLATE, 90 },  { Items.THICK_CLUB, 90 },   { Items.TOXIC_PLATE, 90 },  { Items.ZAP_PLATE, 90 },	
			//80 => 
					{ Items.ASSAULT_VEST, 80 }, { Items.DAWN_STONE, 80 },   { Items.DUSK_STONE, 80 },   { Items.ELECTIRIZER, 80 },  { Items.MAGMARIZER, 80 },
					{ Items.ODD_KEYSTONE, 80 }, { Items.OVAL_STONE, 80 },   { Items.PROTECTOR, 80 },    { Items.QUICK_CLAW, 80 },   { Items.RAZOR_CLAW, 80 },
					{ Items.SAFETY_GOGGLES, 80 },   { Items.SHINY_STONE, 80 },  { Items.STICKY_BARB, 80 },  { Items.WEAKNESS_POLICY, 80 },	
			//70 => 
					{ Items.BURN_DRIVE, 70 },       { Items.CHILL_DRIVE, 70 },  { Items.DOUSE_DRIVE, 70 },  { Items.DRAGON_FANG, 70 },  { Items.POISON_BARB, 70 },
					{ Items.POWER_ANKLET, 70 }, { Items.POWER_BAND, 70 },   { Items.POWER_BELT, 70 },   { Items.POWER_BRACER, 70 }, { Items.POWER_LENS, 70 },
					{ Items.POWER_WEIGHT, 70 }, { Items.SHOCK_DRIVE, 70 },	
			//60 => 
					{ Items.ADAMANT_ORB, 60 },  { Items.DAMP_ROCK, 60 },    { Items.GRISEOUS_ORB, 60 }, { Items.HEAT_ROCK, 60 },    { Items.LUSTROUS_ORB, 60 },
					{ Items.MACHO_BRACE, 60 },  { Items.ROCKY_HELMET, 60 }, { Items.STICK, 60 },	
			//50 => 
					{ Items.DUBIOUS_DISC, 50 }, { Items.SHARP_BEAK, 50 },	
			//40 => 
					{ Items.EVIOLITE, 40 }, { Items.ICY_ROCK, 40 }, { Items.LUCKY_PUNCH, 40 },	
			//30 => 
					{ Items.ABILITY_CAPSULE, 30 },  { Items.ABILITY_URGE, 30 }, { Items.ABSORB_BULB, 30 },  { Items.AMAZE_MULCH, 30 },  { Items.AMULET_COIN, 30 },
					{ Items.ANTIDOTE, 30 }, { Items.AWAKENING, 30 },    { Items.BALM_MUSHROOM, 30 },    { Items.BERRY_JUICE, 30 },  { Items.BIG_MUSHROOM, 30 },
					{ Items.BIG_NUGGET, 30 },   { Items.BIG_PEARL, 30 },    { Items.BINDING_BAND, 30 }, { Items.BLACK_BELT, 30 },   { Items.BLACK_FLUTE, 30 },
					{ Items.BLACK_GLASSES, 30 },    { Items.BLACK_SLUDGE, 30 }, { Items.BLUE_FLUTE, 30 },   { Items.BLUE_SHARD, 30 },   { Items.BOOST_MULCH, 30 },
					{ Items.BURN_HEAL, 30 },    { Items.CALCIUM, 30 },  { Items.CARBOS, 30 },   { Items.CASTELIACONE, 30 }, { Items.CELL_BATTERY, 30 },
					{ Items.CHARCOAL, 30 }, { Items.CLEANSE_TAG, 30 },  { Items.COMET_SHARD, 30 },  { Items.DAMP_MULCH, 30 },   { Items.DEEP_SEA_SCALE, 30 },
					{ Items.DIRE_HIT, 30 }, { Items.DIRE_HIT_2, 30 },   { Items.DIRE_HIT_3, 30 },   { Items.DRAGON_SCALE, 30 }, { Items.EJECT_BUTTON, 30 },
					{ Items.ELIXIR, 30 },   { Items.ENERGY_POWDER, 30 },    { Items.ENERGY_ROOT, 30 },  { Items.ESCAPE_ROPE, 30 },  { Items.ETHER, 30 },
					{ Items.EVERSTONE, 30 },    { Items.EXP_SHARE, 30 },    { Items.FIRE_STONE, 30 },   { Items.FLAME_ORB, 30 },    { Items.FLOAT_STONE, 30 },
					{ Items.FLUFFY_TAIL, 30 },  { Items.FRESH_WATER, 30 },  { Items.FULL_HEAL, 30 },    { Items.FULL_RESTORE, 30 }, { Items.GOOEY_MULCH, 30 },
					{ Items.GREEN_SHARD, 30 },  { Items.GROWTH_MULCH, 30 }, { Items.GUARD_SPEC, 30 },   { Items.HEAL_POWDER, 30 },  { Items.HEART_SCALE, 30 },
					{ Items.HONEY, 30 },    { Items.HP_UP, 30 },    { Items.HYPER_POTION, 30 }, { Items.ICE_HEAL, 30 }, { Items.IRON, 30 },
					{ Items.ITEM_DROP, 30 },    { Items.ITEM_URGE, 30 },    { Items.KINGS_ROCK, 30 },   { Items.LAVA_COOKIE, 30 },  { Items.LEAF_STONE, 30 },
					{ Items.LEMONADE, 30 }, { Items.LIFE_ORB, 30 }, { Items.LIGHT_BALL, 30 },   { Items.LIGHT_CLAY, 30 },   { Items.LUCKY_EGG, 30 },
					{ Items.LUMINOUS_MOSS, 30 },    { Items.LUMIOSE_GALETTE, 30 },  { Items.MAGNET, 30 },   { Items.MAX_ELIXIR, 30 },   { Items.MAX_ETHER, 30 },
					{ Items.MAX_POTION, 30 },   { Items.MAX_REPEL, 30 },    { Items.MAX_REVIVE, 30 },   { Items.METAL_COAT, 30 },   { Items.METRONOME, 30 },
					{ Items.MIRACLE_SEED, 30 }, { Items.MOOMOO_MILK, 30 },  { Items.MOON_STONE, 30 },   { Items.MYSTIC_WATER, 30 }, { Items.NEVER_MELT_ICE, 30 },
					{ Items.NUGGET, 30 },   { Items.OLD_GATEAU, 30 },   { Items.PARALYZE_HEAL, 30 },    { Items.PARALYZE_HEAL, 30 },    { Items.PASS_ORB, 30 },
					{ Items.PEARL, 30 },    { Items.PEARL_STRING, 30 }, { Items.POKE_DOLL, 30 },    { Items.POKE_TOY, 30 }, { Items.POTION, 30 },
					{ Items.PP_MAX, 30 },   { Items.PP_UP, 30 },    { Items.PRISM_SCALE, 30 },  { Items.PROTEIN, 30 },  { Items.RAGE_CANDY_BAR, 30 },
					{ Items.RARE_CANDY, 30 },   { Items.RAZOR_FANG, 30 },   { Items.RED_FLUTE, 30 },    { Items.RED_SHARD, 30 },    { Items.RELIC_BAND, 30 },
					{ Items.RELIC_COPPER, 30 }, { Items.RELIC_CROWN, 30 },  { Items.RELIC_GOLD, 30 },   { Items.RELIC_SILVER, 30 }, { Items.RELIC_STATUE, 30 },
					{ Items.RELIC_VASE, 30 },   { Items.REPEL, 30 },    { Items.RESET_URGE, 30 },   { Items.REVIVAL_HERB, 30 }, { Items.REVIVE, 30 },
					{ Items.RICH_MULCH, 30 },   { Items.SACHET, 30 },   { Items.SACRED_ASH, 30 },   { Items.SCOPE_LENS, 30 },   { Items.SHALOUR_SABLE, 30 },
					{ Items.SHELL_BELL, 30 },   { Items.SHOAL_SALT, 30 },   { Items.SHOAL_SHELL, 30 },  { Items.SMOKE_BALL, 30 },   { Items.SNOWBALL, 30 },
					{ Items.SODA_POP, 30 }, { Items.SOUL_DEW, 30 }, { Items.SPELL_TAG, 30 },    { Items.STABLE_MULCH, 30 }, { Items.STARDUST, 30 },
					{ Items.STAR_PIECE, 30 },   { Items.SUN_STONE, 30 },    { Items.SUPER_POTION, 30 }, { Items.SUPER_REPEL, 30 },  { Items.SURPRISE_MULCH, 30 },
					{ Items.SWEET_HEART, 30 },  { Items.THUNDER_STONE, 30 },    { Items.TINY_MUSHROOM, 30 },    { Items.TOXIC_ORB, 30 },    { Items.TWISTED_SPOON, 30 },
					{ Items.UP_GRADE, 30 }, { Items.WATER_STONE, 30 },  { Items.WHIPPED_DREAM, 30 },    { Items.WHITE_FLUTE, 30 },  { Items.X_ACCURACY, 30 },
					{ Items.X_ACCURACY_2, 30 }, { Items.X_ACCURACY_3, 30 }, { Items.X_ACCURACY_6, 30 }, { Items.X_ATTACK, 30 }, { Items.X_ATTACK_2, 30 },
					{ Items.X_ATTACK_3, 30 },   { Items.X_ATTACK_6, 30 },   { Items.X_DEFENSE, 30 },    { Items.X_DEFENSE_2, 30 },  { Items.X_DEFENSE_3, 30 },
					{ Items.X_DEFENSE_6, 30 },  { Items.X_SP_DEF, 30 }, { Items.X_SP_DEF_2, 30 },   { Items.X_SP_DEF_3, 30 },   { Items.X_SP_DEF_6, 30 },
					{ Items.X_SP_ATK, 30 },     { Items.X_SP_ATK_2, 30 },   { Items.X_SP_ATK_3, 30 },   { Items.X_SP_ATK_6, 30 },   { Items.X_SPEED, 30 },
					{ Items.X_SPEED_2, 30 },    { Items.X_SPEED_3, 30 },    { Items.X_SPEED_6, 30 },    { Items.YELLOW_FLUTE, 30 }, { Items.YELLOW_SHARD, 30 }, { Items.ZINC, 30 },	
					//{ Items.X_SPECIAL, 30 },	{ Items.X_SPECIAL2, 30 },	{ Items.X_SPECIAL_3, 30 },	{ Items.X_SPECIAL_6, 30 },	//ToDo: Is X-Special a thing?
			//20 => 
					{ Items.CLEVER_WING, 20 },  { Items.GENIUS_WING, 20 },  { Items.HEALTH_WING, 20 },  { Items.MUSCLE_WING, 20 },  { Items.PRETTY_WING, 20 },
					{ Items.RESIST_WING, 20 },  { Items.SWIFT_WING, 20 },	
			//10 => 
					{ Items.AIR_BALLOON, 10 },  { Items.BIG_ROOT, 10 }, { Items.BLUE_SCARF, 10 },   { Items.BRIGHT_POWDER, 10 },    { Items.CHOICE_BAND, 10 },
					{ Items.CHOICE_SCARF, 10 }, { Items.CHOICE_SPECS, 10 }, { Items.DESTINY_KNOT, 10 }, { Items.EXPERT_BELT, 10 },  { Items.FOCUS_BAND, 10 },
					{ Items.FOCUS_SASH, 10 },   { Items.FULL_INCENSE, 10 }, { Items.GREEN_SCARF, 10 },  { Items.LAGGING_TAIL, 10 }, { Items.LAX_INCENSE, 10 },
					{ Items.LEFTOVERS, 10 },    { Items.LUCK_INCENSE, 10 }, { Items.MENTAL_HERB, 10 },  { Items.METAL_POWDER, 10 }, { Items.MUSCLE_BAND, 10 },
					{ Items.ODD_INCENSE, 10 },  { Items.PINK_SCARF, 10 },   { Items.POWER_HERB, 10 },   { Items.PURE_INCENSE, 10 }, { Items.QUICK_POWDER, 10 },
					{ Items.REAPER_CLOTH, 10 }, { Items.RED_CARD, 10 }, { Items.RED_SCARF, 10 },    { Items.RING_TARGET, 10 },  { Items.ROCK_INCENSE, 10 },
					{ Items.ROSE_INCENSE, 10 }, { Items.SEA_INCENSE, 10 },  { Items.SHED_SHELL, 10 },   { Items.SILK_SCARF, 10 },   { Items.SILVER_POWDER, 10 },
					{ Items.SMOOTH_ROCK, 10 },  { Items.SOFT_SAND, 10 },    { Items.SOOTHE_BELL, 10 },  { Items.WAVE_INCENSE, 10 }, { Items.WHITE_HERB, 10 },
					{ Items.WIDE_LENS, 10 },    { Items.WISE_GLASSES, 10 }, { Items.YELLOW_SCARF, 10 }, { Items.ZOOM_LENS, 10 }
			};
			}
		}

		public object pbMoveFailed(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (attacker.Item == 0 ||
						   this.battle.pbIsUnlosableItem(attacker, attacker.Item) ||
						   pbIsPokeBall(attacker.Item) ||
						   this.battle.field.MagicRoom > 0 ||
						   attacker.hasWorkingAbility(Abilities.KLUTZ) ||
						   attacker.effects.Embargo > 0) return true;
			foreach (Items i in flingarray.Keys)
			{
				//if (flingarray[i]){
				//	foreach (var j in flingarray[i]){ 
						if (attacker.Item == i) return false;
				//	}
				//}
			}
			if (pbIsBerry(attacker.Item) &&
							!attacker.pbOpposing1.hasWorkingAbility(Abilities.UNNERVE) &&
							!attacker.pbOpposing2.hasWorkingAbility(Abilities.UNNERVE)) return false;
			return true;
		}

		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{
			if (pbIsBerry(attacker.Item)) return 10;
			if (pbIsMegaStone(attacker.Item)) return 80;
			foreach (Items i in flingarray.Keys)
			{
				//if (flingarray[i]){
				//	foreach (var j in flingarray[i]){ 
						if (attacker.Item == i) return i;
				//	}
				//}
			}
			return 1;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.Item == 0)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return 0;
			}
			attacker.effects.Unburden = true;

			//battle.pbDisplay(_INTL("{1} flung its {2}!", attacker.pbThis, PBItems.getName(attacker.Item)))
			object ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (opponent.damagestate.CalcDamage > 0 && !opponent.damagestate.Substitute &&
			   (attacker.hasMoldBreaker() || !opponent.hasWorkingAbility(Abilities.SHIELD_DUST)))
			{
				if (attacker.hasWorkingBerry())
				{
					opponent.pbActivateBerryEffect(attacker.Item, false);

				}
				else if (attacker.hasWorkingItem(Items.FLAME_ORB))
				{
					if (opponent.pbCanBurn(attacker, false, this))
					{
						opponent.pbBurn(attacker);
					}

				}
				else if (attacker.hasWorkingItem(Items.KINGS_ROCK) ||
					 attacker.hasWorkingItem(Items.RAZOR_FANG))
				{
					opponent.pbFlinch(attacker);
				}
				else if (attacker.hasWorkingItem(Items.LIGHT_BALL))
				{
					if (opponent.pbCanParalyze(attacker, false, this))
					{
						opponent.pbParalyze(attacker);
					}

				}
				else if (attacker.hasWorkingItem(Items.MENTAL_HERB))
				{
					if (opponent.effects.Attract >= 0)
					{
						opponent.pbCureAttract();
						//battle.pbDisplay(_INTL("{1} got over its infatuation.", opponent.pbThis))
					}
					if (opponent.effects.Taunt > 0)
					{
						opponent.effects.Taunt = 0;
						//battle.pbDisplay(_INTL("{1}'s taunt wore off!",opponent.pbThis))
					}
					if (opponent.effects.Encore > 0)
					{
						opponent.effects.Encore = 0;
						opponent.effects.EncoreMove = 0;
						opponent.effects.EncoreIndex = 0;
						//battle.pbDisplay(_INTL("{1}'s encore ended!",opponent.pbThis))
					}
					if (opponent.effects.Torment)
					{
						opponent.effects.Torment = false;

						//battle.pbDisplay(_INTL("{1}'s torment wore off!", opponent.pbThis))
					}
					if (opponent.effects.Disable > 0)
					{
						opponent.effects.Disable = 0;
						//battle.pbDisplay(_INTL("{1} is no longer disabled!",opponent.pbThis))
					}
					if (opponent.effects.HealBlock > 0)
					{
						opponent.effects.HealBlock = 0;
						//battle.pbDisplay(_INTL("{1}'s Heal Block wore off!",opponent.pbThis))
					}
				}
				else if (attacker.hasWorkingItem(Items.POISON_BARB))
				{
					if (opponent.pbCanPoison(attacker, false, this))
					{
						opponent.pbPoison(attacker);
					}

				}
				else if (attacker.hasWorkingItem(Items.TOXIC_ORB))
				{
					if (opponent.pbCanPoison(attacker, false, this))
					{
						opponent.pbPoison(attacker, null, true);
					}
				}
				else if (attacker.hasWorkingItem(Items.WHITE_HERB))
				{
					while (true)
					{
						bool reducedstats = false;
						foreach (var i in new[] { Stats.ATTACK, Stats.DEFENSE,
						Stats.SPEED, Stats.SPATK, Stats.SPDEF,
						Stats.EVASION, Stats.ACCURACY })
						{
							if (opponent.stages[(byte)i] < 0)
							{
								opponent.stages[(byte)i] = 0; reducedstats = true;
							}
						}
						if (!reducedstats) break;
						//battle.pbDisplay(_INTL("{1}'s status is returned to normal!",
						//	opponent.pbThis(true)))
					}
				}

			}
			attacker.pbConsumeItem();
			return ret;
		}
	}

	/// <summary>
	/// For 5 rounds, the target cannnot use its held item, its held item has no
	/// effect, and no items can be used on it. (Embargo)
	/// <summary>
	public class PokeBattle_Move_0F8 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (opponent.effects.Embargo > 0)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			opponent.effects.Embargo = 5;
			//battle.pbDisplay(_INTL("{1} can't use items anymore!",opponent.pbThis))
			return 0;
		}
	}

	/// <summary>
	/// For 5 rounds, all held items cannot be used in any way and have no effect.
	/// Held items can still change hands, but can't be thrown. (Magic Room)
	/// <summary>
	public class PokeBattle_Move_0F9 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (this.battle.field.MagicRoom > 0)
			{
				this.battle.field.MagicRoom = 0;
				//battle.pbDisplay(_INTL("The area returned to normal!"))
			}
			else
			{
				pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

				this.battle.field.MagicRoom = 5;
				//battle.pbDisplay(_INTL("It created a bizarre area in which Pokémon's held items lose their effects!"))
			}
			return 0;
		}
	}

	/// <summary>
	/// User takes recoil damage equal to 1/4 of the damage this move dealt.
	/// <summary>
	public class PokeBattle_Move_0FA : PokeBattle_Move
	{
		public bool isRecoilMove()
		{
			return true;
		}

		public void pbEffectAfterHit(Battle.Battler attacker, Battle.Battler opponent, Effects.Move turneffects)
		{
			if (!attacker.isFainted() && turneffects.TotalDamage > 0)
			{
				if (!attacker.hasWorkingAbility(Abilities.ROCK_HEAD) &&
				   !attacker.hasWorkingAbility(Abilities.MAGIC_GUARD))
				{
					attacker.ReduceHP((int)Math.Round(turneffects.TotalDamage / 4.0f));
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
		public bool isRecoilMove()
		{
			return true;
		}

		public void pbEffectAfterHit(Battle.Battler attacker, Battle.Battler opponent, Effects.Move turneffects)
		{
			if (!attacker.isFainted() && turneffects.TotalDamage > 0)
			{
				if (!attacker.hasWorkingAbility(Abilities.ROCK_HEAD) &&
				   !attacker.hasWorkingAbility(Abilities.MAGIC_GUARD))
				{
					attacker.ReduceHP((int)Math.Round(turneffects.TotalDamage / 3.0f));
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
		public bool isRecoilMove()
		{
			return true;
		}

		public void pbEffectAfterHit(Battle.Battler attacker, Battle.Battler opponent, Effects.Move turneffects)
		{
			if (!attacker.isFainted() && turneffects.TotalDamage > 0)
			{
				if (!attacker.hasWorkingAbility(Abilities.ROCK_HEAD) &&
				   !attacker.hasWorkingAbility(Abilities.MAGIC_GUARD))
				{
					attacker.ReduceHP((int)Math.Round(turneffects.TotalDamage / 2.0f));
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
		public bool isRecoilMove()
		{
			return true;
		}

		public void pbEffectAfterHit(Battle.Battler attacker, Battle.Battler opponent, Effects.Move turneffects)
		{
			if (!attacker.isFainted() && turneffects.TotalDamage > 0)
			{
				if (!attacker.hasWorkingAbility(Abilities.ROCK_HEAD) &&
				   !attacker.hasWorkingAbility(Abilities.MAGIC_GUARD))
				{
					attacker.ReduceHP((int)Math.Round(turneffects.TotalDamage / 3.0f));
					//battle.pbDisplay(_INTL("{1} is damaged by recoil!",attacker.pbThis))
				}
			}
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			if (opponent.pbCanParalyze(attacker, false, this))
			{
				opponent.pbParalyze(attacker);
			}
		}
	}

	/// <summary>
	/// User takes recoil damage equal to 1/3 of the damage this move dealt.
	/// May burn the target. (Flare Blitz)
	/// <summary>
	public class PokeBattle_Move_0FE : PokeBattle_Move
	{
		public bool isRecoilMove()
		{
			return true;
		}

		public void pbEffectAfterHit(Battle.Battler attacker, Battle.Battler opponent, Effects.Move turneffects)
		{
			if (!attacker.isFainted() && turneffects.TotalDamage > 0)
			{
				if (!attacker.hasWorkingAbility(Abilities.ROCK_HEAD) &&
				   !attacker.hasWorkingAbility(Abilities.MAGIC_GUARD))
				{
					attacker.ReduceHP((int)Math.Round(turneffects.TotalDamage / 3.0f));
					//battle.pbDisplay(_INTL("{1} is damaged by recoil!",attacker.pbThis))
				}
			}
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			if (opponent.pbCanBurn(attacker, false, this))
			{
				opponent.pbBurn(attacker);
			}
		}
	}

	/// <summary>
	/// Starts sunny weather. (Sunny Day)
	/// <summary>
	public class PokeBattle_Move_0FF : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			switch (this.battle.Weather)
			{
				case Weather.HEAVYRAIN:
					//battle.pbDisplay(_INTL("There is no relief from this heavy rain!"))
					return -1;
				case Weather.HARSHSUN:

					//battle.pbDisplay(_INTL("The extremely harsh sunlight was not lessened at all!"))
					return -1;
				case Weather.STRONGWINDS:

					//battle.pbDisplay(_INTL("The mysterious air current blows on regardless!"))
					return -1;
				case Weather.SUNNYDAY:

					//battle.pbDisplay(_INTL("But it failed!"))
					return -1;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			this.battle.weather = Weather.SUNNYDAY;
			this.battle.weatherduration = 5;
			if (attacker.hasWorkingItem(Items.HEAT_ROCK)) this.battle.weatherduration = 8;

			this.battle.pbCommonAnimation("Sunny", null, null);
			//battle.pbDisplay(_INTL("The sunlight turned harsh!"))
			return 0;
		}
	}

	/// <summary>
	/// Starts rainy weather. (Rain Dance)
	/// <summary>
	public class PokeBattle_Move_100 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			switch (this.battle.weather)
			{
				case Weather.HEAVYRAIN:
					//battle.pbDisplay(_INTL("There is no relief from this heavy rain!"))
					return -1;
				case Weather.HARSHSUN:

					//battle.pbDisplay(_INTL("The extremely harsh sunlight was not lessened at all!"))
					return -1;
				case Weather.STRONGWINDS:

					//battle.pbDisplay(_INTL("The mysterious air current blows on regardless!"))
					return -1;
				case Weather.RAINDANCE:

					//battle.pbDisplay(_INTL("But it failed!"))
					return -1;
				default: break;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			this.battle.weather = Weather.RAINDANCE;
			this.battle.weatherduration = 5;
			if (attacker.hasWorkingItem(Items.DAMP_ROCK)) this.battle.weatherduration = 8;

			this.battle.pbCommonAnimation("Rain", null, null);
			//battle.pbDisplay(_INTL("It started to rain!"))
			return 0;
		}
	}

	/// <summary>
	/// Starts sandstorm weather. (Sandstorm)
	/// <summary>
	public class PokeBattle_Move_101 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			switch (this.battle.Weather)
			{
				case Weather.HEAVYRAIN:
					//battle.pbDisplay(_INTL("There is no relief from this heavy rain!"))
					return -1;
				case Weather.HARSHSUN:

					//battle.pbDisplay(_INTL("The extremely harsh sunlight was not lessened at all!"))
					return -1;
				case Weather.STRONGWINDS:

					//battle.pbDisplay(_INTL("The mysterious air current blows on regardless!"))
					return -1;
				case Weather.SANDSTORM:

					//battle.pbDisplay(_INTL("But it failed!"))
					return -1;
				default: break;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			this.battle.weather = Weather.SANDSTORM;
			this.battle.weatherduration = 5;
			if (attacker.hasWorkingItem(Items.SMOOTH_ROCK)) this.battle.weatherduration = 8;

			this.battle.pbCommonAnimation("Sandstorm", null, null);
			//battle.pbDisplay(_INTL("A sandstorm brewed!"))
			return 0;
		}
	}

	/// <summary>
	/// Starts hail weather. (Hail)
	/// <summary>
	public class PokeBattle_Move_102 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			switch (this.battle.weather)
			{
				case Weather.HEAVYRAIN:
					//battle.pbDisplay(_INTL("There is no relief from this heavy rain!"))
					return -1;
				case Weather.HARSHSUN:

					//battle.pbDisplay(_INTL("The extremely harsh sunlight was not lessened at all!"))
					return -1;
				case Weather.STRONGWINDS:

					//battle.pbDisplay(_INTL("The mysterious air current blows on regardless!"))
					return -1;
				case Weather.HAIL:

					//battle.pbDisplay(_INTL("But it failed!"))
					return -1;
				default: break;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			this.battle.weather = Weather.HAIL;
			this.battle.weatherduration = 5;
			if (attacker.hasWorkingItem(Items.ICY_ROCK)) this.battle.weatherduration = 8;

			this.battle.pbCommonAnimation("Hail", null, null);
			//battle.pbDisplay(_INTL("It started to hail!"))
			return 0;
		}
	}

	/// <summary>
	/// Entry hazard. Lays spikes on the opposing side (max. 3 layers). (Spikes)
	/// <summary>
	public class PokeBattle_Move_103 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.OpposingSide.Spikes >= 3)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			attacker.OpposingSide.Spikes += 1;
			if (!this.battle.isOpposing(attacker.Index))
			{
				//battle.pbDisplay(_INTL("Spikes were scattered all around the opposing team's feet!"))
			}
			else
			{
				//battle.pbDisplay(_INTL("Spikes were scattered all around your team's feet!"))
			}
			return 0;
		}
	}

	/// <summary>
	/// Entry hazard. Lays poison spikes on the opposing side (max. 2 layers).
	/// (Toxic Spikes)
	/// <summary>
	public class PokeBattle_Move_104 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.OpposingSide.ToxicSpikes >= 2)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			attacker.OpposingSide.ToxicSpikes += 1;
			if (!this.battle.isOpposing(attacker.Index))
			{
				//battle.pbDisplay(_INTL("Poison spikes were scattered all around the opposing team's feet!"))
			}
			else
			{
				//battle.pbDisplay(_INTL("Poison spikes were scattered all around your team's feet!"))
			}
			return 0;
		}
	}

	/// <summary>
	/// Entry hazard. Lays stealth rocks on the opposing side. (Stealth Rock)
	/// <summary>
	public class PokeBattle_Move_105 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.OpposingSide.StealthRock)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			attacker.OpposingSide.StealthRock = true;
			if (!this.battle.isOpposing(attacker.Index))
			{
				//battle.pbDisplay(_INTL("Pointed stones float in the air around the opposing team!"))
			}
			else
			{
				//battle.pbDisplay(_INTL("Pointed stones float in the air around your team!"))
			}
			return 0;
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
		{
			this.doubledamage = false; this.overridetype = false;
			if ((int)attacker.effects.FirstPledge == 0x107 ||   // Fire Pledge
			   (int)attacker.effects.FirstPledge == 0x108)		// Water Pledge
			{     
				//battle.pbDisplay(_INTL("The two moves have become one! It's a combined move!"))
				this.doubledamage = true;
				if ((int)attacker.effects.FirstPledge == 0x107)	// Fire Pledge
				{   
					this.overridetype = true;
				}
			}
			return true;
		}

		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{
			if (this.doubledamage)
			{
				return basedmg * 2;
			}
			return basedmg;
		}

		public override Types pbModifyType(Types type, Battle.Battler attacker, Battle.Battler opponent)
		{
			if (this.overridetype)
			{
				type = Types.FIRE;

			}
			return base.pbModifyType(type, attacker, opponent);
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (!this.battle.doublebattle || attacker.Partner.Species == Pokemons.NONE || attacker.Partner.isFainted())
			{
				attacker.effects.FirstPledge = 0;
				return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			}
			// Combined move's effect
			if ((int)attacker.effects.FirstPledge == 0x107)	// Fire Pledge
			{   
				object ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
				if (opponent.damagestate.CalcDamage > 0)
				{

					attacker.OpposingSide.SeaOfFire = 4;
					if (!this.battle.isOpposing(attacker.Index))
					{
						//battle.pbDisplay(_INTL("A sea of fire enveloped the opposing team!"))
						this.battle.pbCommonAnimation("SeaOfFireOpp", null, null);
					}
					else
					{
						//battle.pbDisplay(_INTL("A sea of fire enveloped your team!"))
						this.battle.pbCommonAnimation("SeaOfFire", null, null);
					}
				}

				attacker.effects.FirstPledge = 0;
				return ret;
			}
			else if ((int)attacker.effects.FirstPledge == 0x108)// Water Pledge
			{   
				object ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
				if (opponent.damagestate.CalcDamage > 0)
				{
					attacker.OpposingSide.Swamp = 4;
					if (!this.battle.isOpposing(attacker.Index))
					{
						//battle.pbDisplay(_INTL("A swamp enveloped the opposing team!"))
						this.battle.pbCommonAnimation("SwampOpp", null, null);
					}
					else
					{
						//battle.pbDisplay(_INTL("A swamp enveloped your team!"))
						this.battle.pbCommonAnimation("Swamp", null, null);
					}
				}

				attacker.effects.FirstPledge = 0;
				return ret;
			}
			// Set up partner for a combined move
			attacker.effects.FirstPledge = 0;
			Move.Effect partnermove = Move.Effect.UnimplementedMove; //-1;
			if ((int)this.battle.choices[attacker.Partner.Index].Action == 1)	// Chose a move
			{ 
				if (!attacker.Partner.hasMovedThisRound())
				{
					Moves move = this.battle.choices[attacker.Partner.Index].Move.MoveId;
					if (move > 0)
					{
						partnermove = this.battle.choices[attacker.Partner.Index].Move.Function;
					}

				}
			}
			if ((int)partnermove == 0x107 ||	// Fire Pledge
			   (int)partnermove == 0x108)		// Water Pledge
			{     
				//battle.pbDisplay(_INTL("{1} is waiting for {2}'s move...",attacker.pbThis,attacker.Partner.pbThis(true)))
				attacker.Partner.effects.FirstPledge = this.function;
				attacker.Partner.effects.MoveNext = true;
				return 0;

			}
			// Use the move on its own
			return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
		}

		public override IEnumerator pbShowAnimation(Moves id, Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (this.overridetype)
			{
				return base.pbShowAnimation(Moves.FIRE_PLEDGE, attacker, opponent, hitnum, alltargets, showanimation);
			}
			return base.pbShowAnimation(id, attacker, opponent, hitnum, alltargets, showanimation);
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
		{
			this.doubledamage = false; this.overridetype = false;
			if ((int)attacker.effects.FirstPledge == 0x106 ||   // Grass Pledge
			   (int)attacker.effects.FirstPledge == 0x108)		// Water Pledge
			{       
				//battle.pbDisplay(_INTL("The two moves have become one! It's a combined move!"))
				this.doubledamage = true;
				if ((int)attacker.effects.FirstPledge == 0x108)	// Water Pledge
				{   
					this.overridetype = true;
				}
			}
			return true;
		}

		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{
			if (this.doubledamage)
			{
				return basedmg * 2;
			}
			return basedmg;
		}

		public override Types pbModifyType(Types type, Battle.Battler attacker, Battle.Battler opponent)
		{
			if (this.overridetype)
			{
				type = Types.WATER;

			}
			return base.pbModifyType(type, attacker, opponent);
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (!this.battle.doublebattle || attacker.Partner.Species == Pokemons.NONE || attacker.Partner.isFainted())
			{
				attacker.effects.FirstPledge = 0;
				return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			}
			// Combined move's effect
			if ((int)attacker.effects.FirstPledge == 0x106)	// Grass Pledge
			{   
				object ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
				if (opponent.damagestate.CalcDamage > 0)
				{

					attacker.OpposingSide.SeaOfFire = 4;
					if (!this.battle.isOpposing(attacker.Index))
					{
						//battle.pbDisplay(_INTL("A sea of fire enveloped the opposing team!"))
						this.battle.pbCommonAnimation("SeaOfFireOpp", null, null);
					}
					else
					{
						//battle.pbDisplay(_INTL("A sea of fire enveloped your team!"))
						this.battle.pbCommonAnimation("SeaOfFire", null, null);
					}
				}

				attacker.effects.FirstPledge = 0;
				return ret;
			}
			else if ((int)attacker.effects.FirstPledge == 0x108)	// Water Pledge
			{   
				object ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
				if (opponent.damagestate.CalcDamage > 0)
				{
					attacker.OwnSide.Rainbow = 4;
					if (!this.battle.isOpposing(attacker.Index))
					{
						//battle.pbDisplay(_INTL("A rainbow appeared in the sky on your team's side!"))
						this.battle.pbCommonAnimation("Rainbow", null, null);
					}
					else
					{
						//battle.pbDisplay(_INTL("A rainbow appeared in the sky on the opposing team's side!"))
						this.battle.pbCommonAnimation("RainbowOpp", null, null);
					}
				}

				attacker.effects.FirstPledge = 0;
				return ret;
			}
			// Set up partner for a combined move
			attacker.effects.FirstPledge = 0;
			Move.Effect partnermove = Move.Effect.UnimplementedMove; //-1;
			if ((int)this.battle.choices[attacker.Partner.Index].Action == 1)	// Chose a move
			{ 
				if (!attacker.Partner.hasMovedThisRound())
				{
					Moves move = this.battle.choices[attacker.Partner.Index].Move.MoveId;
					if (move > 0)	//move && 
					{
						partnermove = this.battle.choices[attacker.Partner.Index].Move.Function;
					}

				}
			}
			if ((int)partnermove == 0x106 ||	// Grass Pledge
			   (int)partnermove == 0x108)		// Water Pledge
			{      
				//battle.pbDisplay(_INTL("{1} is waiting for {2}'s move...",attacker.pbThis,attacker.Partner.pbThis(true)))
				attacker.Partner.effects.FirstPledge = this.function;
				attacker.Partner.effects.MoveNext = true;
				return 0;

			}
			// Use the move on its own
			return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
		}

		public override IEnumerator pbShowAnimation(Moves id, Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (this.overridetype)
			{
				return base.pbShowAnimation(Moves.WATER_PLEDGE, attacker, opponent, hitnum, alltargets, showanimation);
			}
			return base.pbShowAnimation(id, attacker, opponent, hitnum, alltargets, showanimation);
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
		{
			this.doubledamage = false; this.overridetype = false;
			if ((int)attacker.effects.FirstPledge == 0x106 ||   // Grass Pledge
			   (int)attacker.effects.FirstPledge == 0x107)		// Fire Pledge
			{      
				//battle.pbDisplay(_INTL("The two moves have become one! It's a combined move!"))
				this.doubledamage = true;
				if ((int)attacker.effects.FirstPledge == 0x106)	// Grass Pledge
				{   
					this.overridetype = true;
				}
			}
			return true;
		}

		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{
			if (this.doubledamage)
			{
				return basedmg * 2;
			}
			return basedmg;
		}

		public override Types pbModifyType(Types type, Battle.Battler attacker, Battle.Battler opponent)
		{
			if (this.overridetype)
			{
				type = Types.GRASS;

			}
			return base.pbModifyType(type, attacker, opponent);
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (!this.battle.doublebattle || attacker.Partner.Species == Pokemons.NONE || attacker.Partner.isFainted())
			{
				attacker.effects.FirstPledge = 0;
				return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			}
			// Combined move's effect
			if ((int)attacker.effects.FirstPledge == 0x106)	// Grass Pledge
			{   
				object ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
				if (opponent.damagestate.CalcDamage > 0)
				{

					attacker.OpposingSide.Swamp = 4;
					if (!this.battle.isOpposing(attacker.Index))
					{
						//battle.pbDisplay(_INTL("A swamp enveloped the opposing team!"))
						this.battle.pbCommonAnimation("SwampOpp", null, null);
					}
					else
					{
						//battle.pbDisplay(_INTL("A swamp enveloped your team!"))
						this.battle.pbCommonAnimation("Swamp", null, null);
					}
				}

				attacker.effects.FirstPledge = 0;
				return ret;
			}
			else if ((int)attacker.effects.FirstPledge == 0x107)	// Fire Pledge
			{   
				object ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
				if (opponent.damagestate.CalcDamage > 0)
				{
					attacker.OwnSide.Rainbow = 4;
					if (!this.battle.isOpposing(attacker.Index))
					{
						//battle.pbDisplay(_INTL("A rainbow appeared in the sky on your team's side!"))
						this.battle.pbCommonAnimation("Rainbow", null, null);
					}
					else
					{
						//battle.pbDisplay(_INTL("A rainbow appeared in the sky on the opposing team's side!"))
						this.battle.pbCommonAnimation("RainbowOpp", null, null);
					}
				}

				attacker.effects.FirstPledge = 0;
				return ret;
			}
			// Set up partner for a combined move
			attacker.effects.FirstPledge = 0;
			Move.Effect partnermove = Move.Effect.UnimplementedMove; //-1;
			if ((int)this.battle.choices[attacker.Partner.Index].Action == 1)	// Chose a move
			{ 
				if (!attacker.Partner.hasMovedThisRound())
				{
					Moves move = this.battle.choices[attacker.Partner.Index].Move.MoveId;
					if (move > 0)	//move && 
					{
						partnermove = this.battle.choices[attacker.Partner.Index].Move.Function;
					}

				}
			}
			if ((int)partnermove == 0x106 ||	// Grass Pledge
			   (int)partnermove == 0x107)		// Fire Pledge
			{      
				//battle.pbDisplay(_INTL("{1} is waiting for {2}'s move...",attacker.pbThis,attacker.Partner.pbThis(true)))
				attacker.Partner.effects.FirstPledge = this.function;
				attacker.Partner.effects.MoveNext = true;
				return 0;

			}
			// Use the move on its own
			return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
		}

		public override IEnumerator pbShowAnimation(Moves id, Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (this.overridetype)
			{
				return base.pbShowAnimation(Moves.GRASS_PLEDGE, attacker, opponent, hitnum, alltargets, showanimation);
			}
			return base.pbShowAnimation(id, attacker, opponent, hitnum, alltargets, showanimation);
		}
	}

	/// <summary>
	/// Scatters coins that the player picks up after winning the battle. (Pay Day)
	/// <summary>
	public class PokeBattle_Move_109 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			object ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (opponent.damagestate.CalcDamage > 0)
			{
				if (this.battle.pbOwnedByPlayer(attacker.Index))
				{
					this.battle.extramoney += 5 * attacker.level;
					if (this.battle.extramoney > Settings.MAXMONEY) this.battle.extramoney = Settings.MAXMONEY;
				}

				//battle.pbDisplay(_INTL("Coins were scattered everywhere!"))
			}
			return ret;
		}
	}

	/// <summary>
	/// Ends the opposing side's Light Screen and Reflect. (Brick Break)
	/// <summary>
	public class PokeBattle_Move_10A : PokeBattle_Move
	{
		public override int pbCalcDamage(Battle.Battler attacker, Battle.Battler opponent)
		{
			return base.pbCalcDamage(attacker, opponent, thismove.NOREFLECT);
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{

			object ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (attacker.OpposingSide.Reflect > 0)
			{
				attacker.OpposingSide.Reflect = 0;
				if (!this.battle.isOpposing(attacker.Index))
				{
					//battle.pbDisplay(_INTL("The opposing team's Reflect wore off!"))
				}
				else
				{
					//battle.pbDisplayPaused(_INTL("Your team's Reflect wore off!"))
				}
			}
			if (attacker.OpposingSide.LightScreen > 0)
			{
				attacker.OpposingSide.LightScreen = 0;
				if (!this.battle.isOpposing(attacker.Index))
				{
					//battle.pbDisplay(_INTL("The opposing team's Light Screen wore off!"))
				}
				else
				{
					//battle.pbDisplay(_INTL("Your team's Light Screen wore off!"))
				}
			}
			return ret;
		}

		public override IEnumerator pbShowAnimation(Moves id, Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.OpposingSide.Reflect > 0 ||
			   attacker.OpposingSide.LightScreen > 0)
			{
				return base.pbShowAnimation(id, attacker, opponent, 1, alltargets, showanimation); // Wall-breaking anim;
			}
			return base.pbShowAnimation(id, attacker, opponent, hitnum, alltargets, showanimation);
		}
	}

	/// <summary>
	/// If attack misses, user takes crash damage of 1/2 of max HP.
	/// (Hi Jump Kick, Jump Kick)
	/// <summary>
	public class PokeBattle_Move_10B : PokeBattle_Move
	{
		public bool isRecoilMove()
		{
			return true;
		}

		public bool unusableInGravity()
		{
			return true;
		}
	}

	/// <summary>
	/// User turns 1/4 of max HP into a substitute. (Substitute)
	/// <summary>
	public class PokeBattle_Move_10C : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.effects.Substitute > 0)
			{
				//battle.pbDisplay(_INTL("{1} already has a substitute!",attacker.pbThis))
				return -1;
			}
			int sublife = (int)Math.Max(Math.Floor(attacker.TotalHP / 4f), 1);
			if (attacker.HP <= sublife)
			{
				//battle.pbDisplay(_INTL("It was too weak to make a substitute!"))
				return -1;
			}
			attacker.ReduceHP(sublife, false, false);

			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			attacker.effects.MultiTurn = 0;
			attacker.effects.MultiTurnAttack = 0;
			attacker.effects.Substitute = sublife;
			//battle.pbDisplay(_INTL("{1} put in a substitute!", attacker.pbThis))
			return 0;
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
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			bool failed = false;
			if (attacker.hasType(Types.GHOST))
			{
				if (opponent.effects.Curse ||
				   opponent.OwnSide.CraftyShield)
				{
					failed = true;
				}
				else
				{
					pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

					//battle.pbDisplay(_INTL("{1} cut its own HP and laid a curse on {2}!",attacker.pbThis,opponent.pbThis(true)))
					opponent.effects.Curse = true;
					attacker.ReduceHP((int)Math.Floor(attacker.TotalHP / 2f));
				}
			}
			else
			{
				bool lowerspeed = attacker.pbCanReduceStatStage(Stats.SPEED, attacker, false, this);
				bool raiseatk = attacker.pbCanIncreaseStatStage(Stats.ATTACK, attacker, false, this);
				bool raisedef = attacker.pbCanIncreaseStatStage(Stats.DEFENSE, attacker, false, this);
				if (!lowerspeed && !raiseatk && !raisedef)
				{
					failed = true;
				}
				else
				{
					pbShowAnimation(this.id, attacker, null, 1, alltargets, showanimation); // Non-Ghost move animation;
					if (lowerspeed)
					{
						attacker.pbReduceStat(Stats.SPEED, 1, attacker, false, this);

					}
					bool showanim = true;
					if (raiseatk)
					{
						attacker.pbIncreaseStat(Stats.ATTACK, 1, attacker, false, this, showanim);

						showanim = false;
					}
					if (raisedef)
					{
						attacker.pbIncreaseStat(Stats.DEFENSE, 1, attacker, false, this, showanim);

						showanim = false;
					}
				}

			}
			if (failed)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
			}
			return failed ? -1 : 0;
		}
	}

	/// <summary>
	/// Target's last move used loses 4 PP. (Spite)
	/// <summary>
	public class PokeBattle_Move_10E : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			foreach (var i in opponent.moves)
			{
				if (i.MoveId == opponent.lastMoveUsed && i.MoveId > 0 && i.PP > 0)
				{
					pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

					byte reduction = Math.Min((byte)4, i.PP);
					opponent.pbSetPP(i.MoveId, i.PP - reduction);

					//battle.pbDisplay(_INTL("It reduced the PP of {1}'s {2} by {3}!",opponent.pbThis(true),i.name,reduction))
					return 0;
				}
			}

			//battle.pbDisplay(_INTL("But it failed!"))
			return -1;
		}
	}

	/// <summary>
	/// Target will lose 1/4 of max HP at end of each round, while asleep. (Nightmare)
	/// <summary>
	public class PokeBattle_Move_10F : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (opponent.Status != Status.SLEEP || opponent.effects.Nightmare ||
			   (opponent.effects.Substitute > 0 && !ignoresSubstitute(attacker)))
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			opponent.effects.Nightmare = true;
			//battle.pbDisplay(_INTL("{1} began having a nightmare!",opponent.pbThis))
			return 0;
		}
	}

	/// <summary>
	/// Removes trapping moves, entry hazards and Leech Seed on user/user's side.
	/// (Rapid Spin)
	/// <summary>
	public class PokeBattle_Move_110 : PokeBattle_Move
	{
		public void pbEffectAfterHit(Battle.Battler attacker, Battle.Battler opponent, Effects.Move turneffects)
		{
			if (!attacker.isFainted() && turneffects.TotalDamage > 0)
			{
				if (attacker.effects.MultiTurn > 0)
				{
					string mtattack = PBMoves.getName(attacker.effects.MultiTurnAttack);
					Battle.Battler mtuser = this.battle.battlers[attacker.effects.MultiTurnUser];

					//battle.pbDisplay(_INTL("{1} got free of {2}'s {3}!",attacker.pbThis,mtuser.pbThis(true),mtattack))
					attacker.effects.MultiTurn = 0;
					attacker.effects.MultiTurnAttack = 0;
					attacker.effects.MultiTurnUser = -1;
				}
				if (attacker.effects.LeechSeed >= 0)
				{
					attacker.effects.LeechSeed = -1;
					//battle.pbDisplay(_INTL("{1} shed Leech Seed!",attacker.pbThis))   
				}
				if (attacker.OwnSide.StealthRock)
				{
					attacker.OwnSide.StealthRock = false;

					//battle.pbDisplay(_INTL("{1} blew away stealth rocks!", attacker.pbThis))     
				}
				if (attacker.OwnSide.Spikes > 0)
				{
					attacker.OwnSide.Spikes = 0;
					//battle.pbDisplay(_INTL("{1} blew away Spikes!",attacker.pbThis))     
				}
				if (attacker.OwnSide.ToxicSpikes > 0)
				{
					attacker.OwnSide.ToxicSpikes = 0;
					//battle.pbDisplay(_INTL("{1} blew away poison spikes!",attacker.pbThis))     
				}
				if (attacker.OwnSide.StickyWeb)
				{
					attacker.OwnSide.StickyWeb = false;

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
		public override object pbDisplayUseMessage(Battle.Battler attacker)
		{
			if (this.battle.futuresight) return 0;
			return base.pbDisplayUseMessage(attacker);
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (opponent.effects.FutureSight > 0)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			if (this.battle.futuresight)
			{
				// Attack hits
				return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);

			}
			/// Attack is launched
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			opponent.effects.FutureSight = 3;
			opponent.effects.FutureSightMove = this.id;
			opponent.effects.FutureSightUser = attacker.pokemonIndex;

			opponent.effects.FutureSightUserPos = attacker.Index;
			if (id == Moves.FUTURE_SIGHT)
			{

				//battle.pbDisplay(_INTL("{1} foresaw an attack!",attacker.pbThis))
			}
			else
			{
				//battle.pbDisplay(_INTL("{1} chose Doom Desire as its destiny!",attacker.pbThis))
			}
			return 0;
		}

		public override IEnumerator pbShowAnimation(Moves id, Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (this.battle.futuresight)
			{
				return base.pbShowAnimation(id, attacker, opponent, 1, alltargets, showanimation); // Hit opponent anim;
			}
			return base.pbShowAnimation(id, attacker, opponent, hitnum, alltargets, showanimation);
		}
	}

	/// <summary>
	/// Increases the user's Defense and Special Defense by 1 stage each. Ups the
	/// user's stockpile by 1 (max. 3). (Stockpile)
	/// <summary>
	public class PokeBattle_Move_112 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.effects.Stockpile >= 3)
			{
				//battle.pbDisplay(_INTL("{1} can't stockpile any more!",attacker.pbThis))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			attacker.effects.Stockpile += 1;
			//battle.pbDisplay(_INTL("{1} stockpiled {2}!",attacker.pbThis,
			//	attacker.effects.Stockpile));
			bool showanim = true;
			if (attacker.pbCanIncreaseStatStage(Stats.DEFENSE, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.DEFENSE, 1, attacker, false, this, showanim);
				attacker.effects.StockpileDef += 1;
				showanim = false;
			}
			if (attacker.pbCanIncreaseStatStage(Stats.SPDEF, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.SPDEF, 1, attacker, false, this, showanim);
				attacker.effects.StockpileSpDef += 1;
				showanim = false;
			}
			return 0;
		}
	}

	/// <summary>
	/// Power is 100 multiplied by the user's stockpile (X). Resets the stockpile to
	/// 0. Decreases the user's Defense and Special Defense by X stages each. (Spit Up)
	/// <summary>
	public class PokeBattle_Move_113 : PokeBattle_Move
	{
		public object pbMoveFailed(Battle.Battler attacker, Battle.Battler opponent)
		{
			return (attacker.effects.Stockpile == 0);
		}

		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{
			return 100 * attacker.effects.Stockpile;
		}

		public void pbEffectAfterHit(Battle.Battler attacker, Battle.Battler opponent, Effects.Move turneffects)
		{
			if (!attacker.isFainted() && turneffects.TotalDamage > 0)
			{

				bool showanim = true;
				if (attacker.effects.StockpileDef > 0)
				{
					if (attacker.pbCanReduceStatStage(Stats.DEFENSE, attacker, false, this))
					{

						attacker.pbReduceStat(Stats.DEFENSE, attacker.effects.StockpileDef,
						   attacker, false, this, showanim);
						showanim = false;
					}
				}
				if (attacker.effects.StockpileSpDef > 0)
				{
					if (attacker.pbCanReduceStatStage(Stats.SPDEF, attacker, false, this))
					{
						attacker.pbReduceStat(Stats.SPDEF, attacker.effects.StockpileSpDef,
						   attacker, false, this, showanim);
						showanim = false;
					}
				}

				attacker.effects.Stockpile = 0;
				attacker.effects.StockpileDef = 0;
				attacker.effects.StockpileSpDef = 0;
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
		public object isHealingMove()
		{
			return true;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			int hpgain = 0;
			switch (attacker.effects.Stockpile)
			{
				case 0:
					//battle.pbDisplay(_INTL("But it failed to swallow a thing!"))
					return -1;
				case 1:
					hpgain = (int)Math.Floor(attacker.TotalHP / 4f); break;
				case 2:
					hpgain = (int)Math.Floor(attacker.TotalHP / 2f); break;
				case 3:
					hpgain = attacker.TotalHP; break;
				default: break;
			}
			if (attacker.HP == attacker.TotalHP &&
			   attacker.effects.StockpileDef == 0 &&
			   attacker.effects.StockpileSpDef == 0)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);
			if (attacker.RecoverHP(hpgain, true) > 0)
			{
				//battle.pbDisplay(_INTL("{1}'s HP was restored.",attacker.pbThis))
			}
			bool showanim = true;
			if (attacker.effects.StockpileDef > 0)
			{
				if (attacker.pbCanReduceStatStage(Stats.DEFENSE, attacker, false, this))
				{
					attacker.pbReduceStat(Stats.DEFENSE, attacker.effects.StockpileDef,
					   attacker, false, this, showanim);
					showanim = false;
				}
			}
			if (attacker.effects.StockpileSpDef > 0)
			{
				if (attacker.pbCanReduceStatStage(Stats.SPDEF, attacker, false, this))
				{
					attacker.pbReduceStat(Stats.SPDEF, attacker.effects.StockpileSpDef,
					   attacker, false, this, showanim);
					showanim = false;
				}
			}

			attacker.effects.Stockpile = 0;
			attacker.effects.StockpileDef = 0;
			attacker.effects.StockpileSpDef = 0;
			//battle.pbDisplay(_INTL("{1}'s stockpiled effect wore off!",attacker.pbThis))
			return 0;
		}
	}

	/// <summary>
	/// Fails if user was hit by a damaging move this round. (Focus Punch)
	/// <summary>
	public class PokeBattle_Move_115 : PokeBattle_Move
	{
		public override object pbDisplayUseMessage(Battle.Battler attacker)
		{
			if (attacker.lastHPLost > 0)
			{
				//battle.pbDisplayBrief(_INTL("{1} lost its focus and couldn't move!",attacker.pbThis))
				return -1;
			}
			return base.pbDisplayUseMessage(attacker);
		}
	}

	/// <summary>
	/// Fails if the target didn't chose a damaging move to use this round, or has
	/// already moved. (Sucker Punch)
	/// <summary>
	public class PokeBattle_Move_116 : PokeBattle_Move
	{
		public object pbMoveFailed(Battle.Battler attacker, Battle.Battler opponent)
		{
			if ((int)this.battle.choices[opponent.Index].Action != 1) return true; // Didn't choose a move
			Battle.InBattleMove oppmove = this.battle.choices[opponent.Index].Move;
			if (oppmove.MoveId <= 0 || oppmove.pbIsStatus()) return true;
			if (opponent.hasMovedThisRound() && (int)oppmove.Function != 0xB0) return true; // Me First
			return false;
		}
	}

	/// <summary>
	/// This round, user becomes the target of attacks that have single targets.
	/// (Follow Me, Rage Powder)
	/// <summary>
	public class PokeBattle_Move_117 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (!this.battle.doublebattle)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			attacker.effects.FollowMe = 1;
			if (!attacker.Partner.isFainted() && attacker.Partner.effects.FollowMe > 0)
			{
				attacker.effects.FollowMe = attacker.Partner.effects.FollowMe + 1;
			}
			//battle.pbDisplay(_INTL("{1} became the center of attention!", attacker.pbThis))
			return 0;
		}
	}

	/// <summary>
	/// For 5 rounds, increases gravity on the field. Pokémon cannot become airborne.
	/// (Gravity)
	/// <summary>
	public class PokeBattle_Move_118 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (this.battle.field.Gravity > 0)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			this.battle.field.Gravity = 5;
			for (int i = 0; i < 4; i++)
			{
				Battle.Battler poke = this.battle.battlers[i];
				if (poke.Species == Pokemons.NONE) continue; //next
				if ((int)new Move((Moves)poke.effects.TwoTurnAttack).Function == 0xC9 ||// Fly
				   (int)new Move((Moves)poke.effects.TwoTurnAttack).Function == 0xCC ||	// Bounce
				   (int)new Move((Moves)poke.effects.TwoTurnAttack).Function == 0xCE)	// Sky Drop
				{    
					poke.effects.TwoTurnAttack = 0;
				}
				if (poke.effects.SkyDrop)
				{
					poke.effects.SkyDrop = false;

				}
				if (poke.effects.MagnetRise > 0)
				{

					poke.effects.MagnetRise = 0;

				}
				if (poke.effects.Telekinesis > 0)
				{

					poke.effects.Telekinesis = 0;

				}
			}

			//battle.pbDisplay(_INTL("Gravity intensified!"))
			return 0;
		}
	}

	/// <summary>
	/// For 5 rounds, user becomes airborne. (Magnet Rise)
	/// <summary>
	public class PokeBattle_Move_119 : PokeBattle_Move
	{
		public bool unusableInGravity()
		{
			return true;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.effects.Ingrain ||
			   attacker.effects.SmackDown ||
			   attacker.effects.MagnetRise > 0)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			attacker.effects.MagnetRise = 5;
			//battle.pbDisplay(_INTL("{1} levitated with electromagnetism!",attacker.pbThis))
			return 0;
		}
	}

	/// <summary>
	/// For 3 rounds, target becomes airborne and can always be hit. (Telekinesis)
	/// <summary>
	public class PokeBattle_Move_11A : PokeBattle_Move
	{
		public bool unusableInGravity()
		{
			return true;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (opponent.effects.Ingrain ||
			   opponent.effects.SmackDown ||
			   opponent.effects.Telekinesis > 0)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			opponent.effects.Telekinesis = 3;
			//battle.pbDisplay(_INTL("{1} was hurled into the air!",opponent.pbThis))
			return 0;
		}
	}

	/// <summary>
	/// Hits airborne semi-invulnerable targets. (Sky Uppercut)
	/// <summary>
	public class PokeBattle_Move_11B : PokeBattle_Move
	{
		// Handled in Battler's pbSuccessCheck, do not edit!
	}

	/// <summary>
	/// Grounds the target while it remains active. (Smack Down, Thousand Arrows)
	/// (Handled in Battler's pbSuccessCheck): Hits some semi-invulnerable targets.
	/// <summary>
	public class PokeBattle_Move_11C : PokeBattle_Move
	{
		public object pbBaseDamage(int basedmg, Battle.Battler attacker, Battle.Battler opponent)
		{
			if ((int)new Move((Moves)opponent.effects.TwoTurnAttack).Function == 0xC9 ||// Fly
			   (int)new Move((Moves)opponent.effects.TwoTurnAttack).Function == 0xCC || // Bounce
			   (int)new Move((Moves)opponent.effects.TwoTurnAttack).Function == 0xCE || // Sky Drop
			   opponent.effects.SkyDrop)
			{
				return basedmg * 2;
			}
			return basedmg;
		}
		//ToDo: Double check this one
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{

			object ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (opponent.damagestate.CalcDamage > 0 &&
				!opponent.damagestate.Substitute &&
				!opponent.effects.Roost)
			{
				opponent.effects.SmackDown = true;

				bool showmsg = (opponent.hasType(Types.FLYING) ||
						 opponent.hasWorkingAbility(Abilities.LEVITATE));
				if ((int)new Move((Moves)opponent.effects.TwoTurnAttack).Function == 0xC9 ||// Fly
				   (int)new Move((Moves)opponent.effects.TwoTurnAttack).Function == 0xCC)	// Bounce
				{    
					opponent.effects.TwoTurnAttack = 0; showmsg = true;
				}
				if (opponent.effects.MagnetRise > 0)
				{
					opponent.effects.MagnetRise = 0; showmsg = true;
				}
				if (opponent.effects.Telekinesis > 0)
				{
					opponent.effects.Telekinesis = 0; showmsg = true;
				}
				//if (showmsg)battle.pbDisplay(_INTL("{1} fell straight down!", opponent.pbThis));
			}
			return ret;
		}
	}

	/// <summary>
	/// Target moves immediately after the user, ignoring priority/speed. (After You)
	/// <summary>
	public class PokeBattle_Move_11D : PokeBattle_Move
	{
		public object pbMoveFailed(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.effects.MoveNext) return true;
			if ((int)this.battle.choices[opponent.Index].Action != 1) return true; // Didn't choose a move
			Moves oppmove = this.battle.choices[opponent.Index].Move.MoveId;
			if (oppmove <= 0) return true;
			if (opponent.hasMovedThisRound()) return true;
			return false;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{

			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			opponent.effects.MoveNext = true;
			opponent.effects.Quash = false;
			//battle.pbDisplay(_INTL("{1} took the kind offer!",opponent.pbThis))
			return 0;
		}
	}

	/// <summary>
	/// Target moves last this round, ignoring priority/speed. (Quash)
	/// <summary>
	public class PokeBattle_Move_11E : PokeBattle_Move
	{
		public object pbMoveFailed(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.effects.Quash) return true;
			if ((int)this.battle.choices[opponent.Index].Action != 1) return true; // Didn't choose a move
			Moves oppmove = this.battle.choices[opponent.Index].Move.MoveId;
			if (oppmove <= 0) return true;
			if (opponent.hasMovedThisRound()) return true;
			return false;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{

			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			opponent.effects.Quash = true;
			opponent.effects.MoveNext = false;
			//battle.pbDisplay(_INTL("{1}'s move was postponed!",opponent.pbThis))
			return 0;
		}
	}

	/// <summary>
	/// For 5 rounds, for each priority bracket, slow Pokémon move before fast ones.
	/// (Trick Room)
	/// <summary>
	public class PokeBattle_Move_11F : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (this.battle.field.TrickRoom > 0)
			{
				this.battle.field.TrickRoom = 0;
				//battle.pbDisplay(_INTL("{1} reverted the dimensions!",attacker.pbThis))
			}
			else
			{
				pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

				this.battle.field.TrickRoom = 5;
				//battle.pbDisplay(_INTL("{1} twisted the dimensions!",attacker.pbThis))
			}
			return 0;
		}
	}

	/// <summary>
	/// User switches places with its ally. (Ally Switch)
	/// <summary>
	public class PokeBattle_Move_120 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (!this.battle.doublebattle ||
			   attacker.Partner.Species == Pokemons.NONE ||
			   attacker.Partner.isFainted())
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			Battle.Battler a = this.battle.battlers[attacker.Index];
			Battle.Battler b = this.battle.battlers[attacker.Partner.Index];

			Battle.Battler temp = a; a = b; b = temp;
			// Swap effects that point at the position rather than the Pokémon
			// NOT PerishSongUser (no need to swap), Attract, MultiTurnUser
			List<BattlerEffects> effectstoswap = new List<BattlerEffects>{
						BattlerEffects.BideTarget,
						BattlerEffects.CounterTarget,
						BattlerEffects.LeechSeed,
						BattlerEffects.LockOnPos,
						BattlerEffects.MeanLook,
						BattlerEffects.MirrorCoatTarget
						};
			//ToDo: Must assign each variable one by one.
			foreach (BattlerEffects i in effectstoswap)
			{
				//ToDo: create temp variables then override source
				//a.effects[i], b.effects[i]= b.effects[i], a.effects[i];
			}

			attacker.Update(true);

			opponent.Update(true);
			//battle.pbDisplay(_INTL("{1} and {2} switched places!",opponent.pbThis,attacker.pbThis(true)))
			return null;//ToDo: Not sure what to return here, so i added null
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
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (!opponent.pbHasType(attacker.Type1) &&
			   !opponent.pbHasType(attacker.Type2) &&
			   !opponent.pbHasType(attacker.effects.Type3))
			{
				//battle.pbDisplay(_INTL("{1} was unaffected!", opponent.pbThis))
				return -1;
			}
			return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
		}
	}

	/// <summary>
	/// For 5 rounds, swaps all battlers' base Defense with base Special Defense.
	/// (Wonder Room)
	/// <summary>
	public class PokeBattle_Move_124 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (this.battle.field.WonderRoom > 0)
			{
				this.battle.field.WonderRoom = 0;
				//battle.pbDisplay(_INTL("Wonder Room wore off, and the Defense and Sp. Def stats returned to normal!"))
			}
			else
			{
				pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

				this.battle.field.WonderRoom = 5;
				//battle.pbDisplay(_INTL("It created a bizarre area in which the Defense and Sp. Def stats are swapped!"))
			}
			return 0;
		}
	}

	/// <summary>
	/// Fails unless user has already used all other moves it knows. (Last Resort)
	/// <summary>
	public class PokeBattle_Move_125 : PokeBattle_Move
	{
		public object pbMoveFailed(Battle.Battler attacker, Battle.Battler opponent)
		{
			byte counter = 0; byte nummoves = 0;
			foreach (var move in attacker.moves)
			{
				if (move.MoveId <= 0) continue; //next
				if (move.MoveId != this.id && !attacker.movesUsed.Contains(move.MoveId)) counter += 1;
				nummoves += 1;
			}
			return counter != 0 || nummoves == 1;
		}
	}

	//===============================================================================
	// NOTE: Shadow moves use function codes 126-132 inclusive.
	//===============================================================================

	/// <summary>
	/// Does absolutely nothing. (Hold Hands)
	/// <summary>
	public class PokeBattle_Move_133 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (!this.battle.doublebattle ||
			   attacker.Partner.Species == Pokemons.NONE || attacker.Partner.isFainted())
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);
			return 0;
		}
	}

	/// <summary>
	/// Does absolutely nothing. Shows a special message. (Celebrate)
	/// <summary>
	public class PokeBattle_Move_134 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			//battle.pbDisplay(_INTL("Congratulations, {1}!",this.battle.pbGetOwner(attacker.Index).name))
			return 0;
		}
	}

	/// <summary>
	/// Freezes the target. (Freeze-Dry)
	/// (Superclass's pbTypeModifier): Effectiveness against Water-type is 2x.
	/// <summary>
	public class PokeBattle_Move_135 : PokeBattle_Move
	{
		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			if (opponent.pbCanFreeze(attacker, false, this))
			{
				opponent.pbFreeze();
			}
		}
	}

	/// <summary>
	/// Increases the user's Defense by 1 stage for each target hit. (Diamond Storm)
	/// <summary>
	public class PokeBattle_Move_136 : Function.PokeBattle_Move_01D
	{
		// No difference to function code 01D. It may need to be separate in future.
	}

	/// <summary>
	/// Increases the user's and its ally's Defense and Special Defense by 1 stage
	/// each, if they have Plus or Minus. (Magnetic Flux)
	/// <summary>
	public class PokeBattle_Move_137 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			bool didsomething = false;
			foreach (Battle.Battler i in new Battle.Battler[] { attacker, attacker.Partner })
			{
				if (i.Species == Pokemons.NONE || i.isFainted()) continue; //next
				if (!i.hasWorkingAbility(Abilities.PLUS) && !i.hasWorkingAbility(Abilities.MINUS)) continue; //next 
				if (!i.pbCanIncreaseStatStage(Stats.DEFENSE, attacker, false, this) &&
						!i.pbCanIncreaseStatStage(Stats.SPDEF, attacker, false, this)) continue; //next 
				if (!didsomething) pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);
				didsomething = true;

				bool showanim = true;
				if (i.pbCanIncreaseStatStage(Stats.DEFENSE, attacker, false, this))
				{
					i.pbIncreaseStat(Stats.DEFENSE, 1, attacker, false, this, showanim);
					showanim = false;
				}
				if (i.pbCanIncreaseStatStage(Stats.SPDEF, attacker, false, this))
				{
					i.pbIncreaseStat(Stats.SPDEF, 1, attacker, false, this, showanim);
					showanim = false;
				}
			}
			if (!didsomething)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			return 0;
		}
	}

	/// <summary>
	/// Increases ally's Special Defense by 1 stage. (Aromatic Mist)
	/// <summary>
	public class PokeBattle_Move_138 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (!this.battle.doublebattle || opponent.Species == Pokemons.NONE ||
			   !opponent.pbCanIncreaseStatStage(Stats.SPDEF, attacker, false, this))
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool ret = attacker.pbIncreaseStat(Stats.SPDEF, 1, attacker, false, this);
			return ret ? 0 : -1;
		}
	}

	/// <summary>
	/// Decreases the target's Attack by 1 stage. Always hits. (Play Nice)
	/// <summary>
	public class PokeBattle_Move_139 : PokeBattle_Move
	{
		public override object pbAccuracyCheck(Battle.Battler attacker, Battle.Battler opponent)
		{
			return true;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (!opponent.pbCanReduceStatStage(Stats.ATTACK, attacker, true, this)) return -1;
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool ret = opponent.pbReduceStat(Stats.ATTACK, 1, attacker, false, this);
			return ret ? 0 : -1;
		}
	}

	/// <summary>
	/// Decreases the target's Attack and Special Attack by 1 stage each. (Noble Roar)
	/// <summary>
	public class PokeBattle_Move_13A : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			// Replicates pbCanReduceStatStage? so that certain messages aren't shown
			// multiple times
			if (opponent.effects.Substitute > 0 && !ignoresSubstitute(attacker))
			{
				//battle.pbDisplay(_INTL("{1}'s attack missed!", attacker.pbThis))
				return -1;
			}
			if (opponent.pbTooLow(Stats.ATTACK) &&
			   opponent.pbTooLow(Stats.SPATK))
			{
				//battle.pbDisplay(_INTL("{1}'s stats won't go any lower!", opponent.pbThis))
				return -1;
			}
			if (opponent.OwnSide.Mist > 0)
			{
				//battle.pbDisplay(_INTL("{1} is protected by Mist!",opponent.pbThis))
				return -1;
			}
			if (!attacker.hasMoldBreaker())
			{
				if (opponent.hasWorkingAbility(Abilities.CLEAR_BODY) ||
				   opponent.hasWorkingAbility(Abilities.WHITE_SMOKE))
				{
					//battle.pbDisplay(_INTL("{1}'s {2} prevents stat loss!",opponent.pbThis,
					//   PBAbilities.getName(opponent.Ability)))
					return -1;
				}
			}

			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			object ret = -1; bool showanim = true;
			if (!attacker.hasMoldBreaker() && opponent.hasWorkingAbility(Abilities.HYPER_CUTTER))
			{
				string abilityname = PBAbilities.getName(opponent.Ability);
				//battle.pbDisplay(_INTL("{1}'s {2} prevents Attack loss!", opponent.pbThis, abilityname))
			}
			else if (opponent.pbReduceStat(Stats.ATTACK, 1, attacker, false, this, showanim))
			{
				ret = 0; showanim = false;
			}
			if (opponent.pbReduceStat(Stats.SPATK, 1, attacker, false, this, showanim))
			{
				ret = 0; showanim = false;
			}
			return ret;
		}
	}

	/// <summary>
	/// Decreases the target's Defense by 1 stage. Always hits. (Hyperspace Fury)
	/// <summary>
	public class PokeBattle_Move_13B : PokeBattle_Move
	{
		public object pbMoveFailed(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (attacker.Species == Pokemons.HOOPA) return true;
			if (attacker.Form != 1) return true;
			return false;
		}

		public override object pbAccuracyCheck(Battle.Battler attacker, Battle.Battler opponent)
		{
			return true;
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			if (opponent.pbCanReduceStatStage(Stats.DEFENSE, attacker, false, this))
			{
				opponent.pbReduceStat(Stats.DEFENSE, 1, attacker, false, this);
			}
		}
	}

	/// <summary>
	/// Decreases the target's Special Attack by 1 stage. Always hits. (Confide)
	/// <summary>
	public class PokeBattle_Move_13C : PokeBattle_Move
	{
		public override object pbAccuracyCheck(Battle.Battler attacker, Battle.Battler opponent)
		{
			return true;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (!opponent.pbCanReduceStatStage(Stats.SPATK, attacker, true, this)) return -1;
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool ret = opponent.pbReduceStat(Stats.SPATK, 1, attacker, false, this);
			return ret ? 0 : -1;
		}
	}

	/// <summary>
	/// Decreases the target's Special Attack by 2 stages. (Eerie Impulse)
	/// <summary>
	public class PokeBattle_Move_13D : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (pbIsDamaging()) return base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (pbTypeImmunityByAbility(pbType(this.type, attacker, opponent), attacker, opponent)) return -1;
			if (!opponent.pbCanReduceStatStage(Stats.SPATK, attacker, true, this)) return -1;
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool ret = opponent.pbReduceStat(Stats.SPATK, 2, attacker, false, this);
			return ret ? 0 : -1;
		}

		public void pbAdditionalEffect(Battle.Battler attacker, Battle.Battler opponent)
		{
			if (opponent.damagestate.Substitute) return;
			if (opponent.pbCanReduceStatStage(Stats.SPATK, attacker, false, this))
			{
				opponent.pbReduceStat(Stats.SPATK, 2, attacker, false, this);
			}
		}
	}

	/// <summary>
	/// Increases the Attack and Special Attack of all Grass-type Pokémon on the field
	/// by 1 stage each. Doesn't affect airborne Pokémon. (Rototiller)
	/// <summary>
	public class PokeBattle_Move_13E : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			bool didsomething = false;
			foreach (Battle.Battler i in new[] { attacker, attacker.Partner, attacker.pbOpposing1, attacker.pbOpposing2 })
			{
				if (i.Species == Pokemons.NONE || i.isFainted()) continue; //next
				if (!i.hasType(Types.GRASS)) continue; //next
				if (i.isAirborne(attacker.hasMoldBreaker())) continue; //next
				if (!i.pbCanIncreaseStatStage(Stats.ATTACK, attacker, false, this) &&
					  !i.pbCanIncreaseStatStage(Stats.SPATK, attacker, false, this)) continue;//next
				if (!didsomething) pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);
				didsomething = true;

				bool showanim = true;
				if (i.pbCanIncreaseStatStage(Stats.ATTACK, attacker, false, this))
				{
					i.pbIncreaseStat(Stats.ATTACK, 1, attacker, false, this, showanim);
					showanim = false;
				}
				if (i.pbCanIncreaseStatStage(Stats.SPATK, attacker, false, this))
				{
					i.pbIncreaseStat(Stats.SPATK, 1, attacker, false, this, showanim);
					showanim = false;
				}
			}
			if (!didsomething)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			return 0;
		}
	}

	/// <summary>
	/// Increases the Defense of all Grass-type Pokémon on the field by 1 stage each.
	/// (Flower Shield)
	/// <summary>
	public class PokeBattle_Move_13F : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			bool didsomething = false;
			foreach (var i in new[] { attacker, attacker.Partner, attacker.pbOpposing1, attacker.pbOpposing2 })
			{
				if (i.Species == Pokemons.NONE || i.isFainted()) continue; //next
				if (!i.hasType(Types.GRASS)) continue; //next
				if (!i.pbCanIncreaseStatStage(Stats.ATTACK, attacker, false, this)) continue; //next
				if (!didsomething) pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);
				didsomething = true;

				bool showanim = true;
				if (i.pbCanIncreaseStatStage(Stats.DEFENSE, attacker, false, this))
				{
					i.pbIncreaseStat(Stats.DEFENSE, 1, attacker, false, this, showanim);
					showanim = false;
				}
			}
			if (!didsomething)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			return 0;
		}
	}

	/// <summary>
	/// Decreases the Attack, Special Attack and Speed of all poisoned opponents by 1
	/// stage each. (Venom Drench)
	/// <summary>
	public class PokeBattle_Move_140 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			bool didsomething = false;
			foreach (var i in new[] { attacker.pbOpposing1, attacker.pbOpposing2 })
			{
				if (i.Species == Pokemons.NONE || i.isFainted()) continue; //next
				if (i.Status != Status.POISON) continue; //next
				if (!i.pbCanReduceStatStage(Stats.ATTACK, attacker, false, this) &&
							  !i.pbCanReduceStatStage(Stats.SPATK, attacker, false, this) &&
							  !i.pbCanReduceStatStage(Stats.SPEED, attacker, false, this)) continue; //next
				if (!didsomething) pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);
				didsomething = true;

				bool showanim = true;
				if (i.pbCanReduceStatStage(Stats.ATTACK, attacker, false, this))
				{
					i.pbReduceStat(Stats.ATTACK, 1, attacker, false, this, showanim);
					showanim = false;
				}
				if (i.pbCanReduceStatStage(Stats.SPATK, attacker, false, this))
				{
					i.pbReduceStat(Stats.SPATK, 1, attacker, false, this, showanim);
					showanim = false;
				}
				if (i.pbCanReduceStatStage(Stats.SPEED, attacker, false, this))
				{
					i.pbReduceStat(Stats.SPEED, 1, attacker, false, this, showanim);
					showanim = false;
				}
			}
			if (!didsomething)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			return 0;
		}
	}

	/// <summary>
	/// Reverses all stat changes of the target. (Topsy-Turvy)
	/// <summary>
	public class PokeBattle_Move_141 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			bool nonzero = false;
			foreach (var i in new[] { Stats.ATTACK, Stats.DEFENSE, Stats.SPEED,
				  Stats.SPATK, Stats.SPDEF, Stats.ACCURACY, Stats.EVASION })
			{
				if (opponent.stages[(byte)i] != 0)
				{
					nonzero = true; break;
				}
			}
			if (!nonzero)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);
			foreach (var i in new[] { Stats.ATTACK, Stats.DEFENSE, Stats.SPEED,
				  Stats.SPATK, Stats.SPDEF, Stats.ACCURACY, Stats.EVASION })
			{
				opponent.stages[(byte)i] *= -1;
			}
			//battle.pbDisplay(_INTL("{1}'s stats were reversed!", opponent.pbThis))
			return 0;
		}
	}

	/// <summary>
	/// Gives target the Ghost type. (Trick-or-Treat)
	/// <summary>
	public class PokeBattle_Move_142 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if ((opponent.effects.Substitute > 0 && !ignoresSubstitute(attacker)) ||
			   opponent.hasType(Types.GHOST) ||
			   opponent.Ability == Abilities.MULTITYPE)
			{
				//battle.pbDisplay(_INTL("But it failed!"))  
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			opponent.effects.Type3 = Types.GHOST;

			string typename = PBTypes.getName(Types.GHOST);
			//battle.pbDisplay(_INTL("{1} transformed into the {2} type!",opponent.pbThis,typename))
			return 0;
		}
	}

	/// <summary>
	/// Gives target the Grass type. (Forest's Curse)
	/// <summary>
	public class PokeBattle_Move_143 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (opponent.effects.Substitute > 0 && !ignoresSubstitute(attacker))
			{
				//battle.pbDisplay(_INTL("But it failed!"))  
				return -1;
			}
			if (pbTypeImmunityByAbility(pbType(this.type, attacker, opponent), attacker, opponent)) return -1;
			if (opponent.effects.LeechSeed >= 0)
			{
				//battle.pbDisplay(_INTL("{1} evaded the attack!",opponent.pbThis))
				return -1;
			}
			if (opponent.hasType(Types.GRASS) ||
			   opponent.Ability == Abilities.MULTITYPE)
			{
				//battle.pbDisplay(_INTL("But it failed!"))  
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			opponent.effects.Type3 = Types.GRASS;

			string typename = PBTypes.getName(Types.GRASS);
			//battle.pbDisplay(_INTL("{1} transformed into the {2} type!",opponent.pbThis,typename))
			return 0;
		}
	}

	/// <summary>
	/// Damage is multiplied by Flying's effectiveness against the target. Does double
	/// damage and has perfect accuracy if the target is Minimized. (Flying Press)
	/// <summary>
	public class PokeBattle_Move_144 : PokeBattle_Move
	{
		public object pbModifyDamage(int damagemult, Battle.Battler attacker, Battle.Battler opponent)
		{
			type = Types.FLYING;// || -1
			if (type >= 0)
			{
				int mult = PBTypes.getCombinedEffectiveness(type,
				   opponent.Type1, opponent.Type2, opponent.effects.Type3);
				return (int)Math.Round((damagemult * mult) / 8f);
			}
			return damagemult;
		}

		public bool tramplesMinimize(byte param = 1)
		{
			if (param == 1 && Settings.USENEWBATTLEMECHANICS) return true; // Perfect accuracy
			if (param == 2) return true; // Double damage
			return false;
		}
	}

	/// <summary>
	/// Target's moves become Electric-type for the rest of the round. (Electrify)
	/// <summary>
	public class PokeBattle_Move_145 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (pbTypeImmunityByAbility(pbType(this.type, attacker, opponent), attacker, opponent)) return -1;
			if (opponent.effects.Electrify)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			if ((int)this.battle.choices[opponent.Index].Action != 1 || // Didn't choose a move
				//!this.battle.choices[opponent.Index].Move ||
				this.battle.choices[opponent.Index].Move.MoveId <= 0 ||
				opponent.hasMovedThisRound())
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			opponent.effects.Electrify = true;
			//battle.pbDisplay(_INTL("{1} was electrified!",opponent.pbThis))
			return 0;
		}
	}

	/// <summary>
	/// All Normal-type moves become Electric-type for the rest of the round.
	/// (Ion Deluge)
	/// <summary>
	public class PokeBattle_Move_146 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			bool unmoved = false;
			foreach (Battle.Battler poke in this.battle.battlers)
			{
				if (poke.Index == attacker.Index) continue; //next
				if ((int)this.battle.choices[poke.Index].Action == 1 && // Chose a move
				   !poke.hasMovedThisRound())
				{
					unmoved = true; break;
				}
			}
			if (!unmoved || this.battle.field.IonDeluge)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			this.battle.field.IonDeluge = true;
			//battle.pbDisplay(_INTL("The Ion Deluge started!"))
			return 0;
		}
	}

	/// <summary>
	/// Always hits. (Hyperspace Hole)
	/// TODO: Hits through various shields.
	/// <summary>
	public class PokeBattle_Move_147 : PokeBattle_Move
	{
		public override object pbAccuracyCheck(Battle.Battler attacker, Battle.Battler opponent)
		{
			return true;
		}
	}

	/// <summary>
	/// Powders the foe. This round, if it uses a Fire move, it loses 1/4 of its max
	/// HP instead. (Powder)
	/// <summary>
	public class PokeBattle_Move_148 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (opponent.effects.Powder)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			opponent.effects.Powder = true;

			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);
			//battle.pbDisplay(_INTL("{1} is covered in powder!", attacker.pbThis))
			return 0;
		}
	}

	/// <summary>
	/// This round, the user's side is unaffected by damaging moves. (Mat Block)
	/// <summary>
	public class PokeBattle_Move_149 : PokeBattle_Move
	{
		public object pbMoveFailed(Battle.Battler attacker, Battle.Battler opponent)
		{
			return (attacker.turncount > 1);
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{

			attacker.OwnSide.MatBlock = true;
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			//battle.pbDisplay(_INTL("{1} intends to flip up a mat and block incoming attacks!",attacker.pbThis))
			return 0;
		}
	}

	/// <summary>
	/// User's side is protected against status moves this round. (Crafty Shield)
	/// <summary>
	public class PokeBattle_Move_14A : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.OwnSide.CraftyShield)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			bool unmoved = false;
			foreach (Battle.Battler poke in this.battle.battlers)
			{
				if (poke.Index == attacker.Index) continue; //next
				if ((int)this.battle.choices[poke.Index].Action == 1 && // Chose a move
				   !poke.hasMovedThisRound())
				{
					unmoved = true; break;
				}
			}
			if (!unmoved)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			attacker.OwnSide.CraftyShield = true;
			if (!this.battle.isOpposing(attacker.Index))
			{
				//battle.pbDisplay(_INTL("Crafty Shield protected your team!"))
			}
			else
			{
				//battle.pbDisplay(_INTL("Crafty Shield protected the opposing team!"))
			}
			return 0;
		}
	}

	/// <summary>
	/// User is protected against damaging moves this round. Decreases the Attack of
	/// the user of a stopped contact move by 2 stages. (King's Shield)
	/// <summary>
	public class PokeBattle_Move_14B : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.effects.KingsShield)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			List<Move.Effect> ratesharers = new List<Move.Effect> {
			(Move.Effect)0xAA,   // Detect, Protect
			(Move.Effect)0xAB,   // Quick Guard
			(Move.Effect)0xAC,   // Wide Guard
			(Move.Effect)0xE8,   // Endure
			(Move.Effect)0x14B,  // King's Shield
			(Move.Effect)0x14C   // Spiky Shield
		};
			if (!ratesharers.Contains(new Move((Moves)attacker.lastMoveUsed).Function))
			{
				attacker.effects.ProtectRate = 1;
			}
			bool unmoved = false;
			foreach (Battle.Battler poke in this.battle.battlers)
			{
				if (poke.Index == attacker.Index) continue; //next
				if ((int)this.battle.choices[poke.Index].Action == 1 && // Chose a move
				   !poke.hasMovedThisRound())
				{
					unmoved = true; break;
				}
			}
			if (!unmoved ||
			   (!Settings.USENEWBATTLEMECHANICS &&
			   this.battle.pbRandom(65536) >= Math.Floor(65536f / attacker.effects.ProtectRate)))
			{
				attacker.effects.ProtectRate = 1;
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			attacker.effects.KingsShield = true;
			attacker.effects.ProtectRate *= 2;
			//battle.pbDisplay(_INTL("{1} protected itself!",attacker.pbThis))
			return 0;
		}
	}

	/// <summary>
	/// User is protected against moves that target it this round. Damages the user of
	/// a stopped contact move by 1/8 of its max HP. (Spiky Shield)
	/// <summary>
	public class PokeBattle_Move_14C : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.effects.SpikyShield)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			List<Move.Effect> ratesharers = new List<Move.Effect> {
		   (Move.Effect)0xAA,   // Detect, Protect
		   (Move.Effect)0xAB,   // Quick Guard
		   (Move.Effect)0xAC,   // Wide Guard
		   (Move.Effect)0xE8,   // Endure
		   (Move.Effect)0x14B,  // King's Shield
		   (Move.Effect)0x14C   // Spiky Shield
		};
			if (!ratesharers.Contains(new Move((Moves)attacker.lastMoveUsed).Function))
			{
				attacker.effects.ProtectRate = 1;
			}
			bool unmoved = false;
			foreach (Battle.Battler poke in this.battle.battlers)
			{
				if (poke.Index == attacker.Index) continue; //next
				if ((int)this.battle.choices[poke.Index].Action == 1 && // Chose a move
				   !poke.hasMovedThisRound())
				{
					unmoved = true; break;
				}
			}
			if (!unmoved ||
			   this.battle.pbRandom(65536) >= Math.Floor(65536f / attacker.effects.ProtectRate))
			{
				attacker.effects.ProtectRate = 1;

				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			attacker.effects.SpikyShield = true;
			attacker.effects.ProtectRate *= 2;
			//battle.pbDisplay(_INTL("{1} protected itself!",attacker.pbThis))
			return 0;
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
		{
			this.immediate = false;
			if (!this.immediate && attacker.hasWorkingItem(Items.POWER_HERB))
			{
				this.immediate = true;
			}
			if (this.immediate) return false;
			return attacker.effects.TwoTurnAttack == 0;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (this.immediate || attacker.effects.TwoTurnAttack > 0)
			{
				pbShowAnimation(this.id, attacker, opponent, 1, alltargets, showanimation); // Charging anim
				//battle.pbDisplay(_INTL("{1} vanished instantly!",attacker.pbThis))
			}
			if (this.immediate)
			{
				this.battle.pbCommonAnimation("UseItem", attacker, null);

				//battle.pbDisplay(_INTL("{1} became fully charged due to its Power Herb!",attacker.pbThis))
				attacker.pbConsumeItem();
			}
			if (attacker.effects.TwoTurnAttack > 0) return 0;
			int ret = (int)base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (ret > 0)
			{
				opponent.effects.ProtectNegation = true;
				opponent.OwnSide.CraftyShield = false;
			}
			return ret;
		}

		public bool tramplesMinimize(byte param = 1)
		{
			if (param == 1 && Settings.USENEWBATTLEMECHANICS) return true; // Perfect accuracy
			if (param == 2) return true; // Double damage
			return false;
		}
	}

	/// <summary>
	/// Two turn attack. Skips first turn, increases the user's Special Attack,
	/// Special Defense and Speed by 2 stages each second turn. (Geomancy)
	/// <summary>
	public class PokeBattle_Move_14E : PokeBattle_Move
	{
		public object pbTwoTurnAttack(Battle.Battler attacker)
		{
			this.immediate = false;
			if (!this.immediate && attacker.hasWorkingItem(Items.POWER_HERB))
			{
				this.immediate = true;
			}
			if (this.immediate) return false;
			return attacker.effects.TwoTurnAttack == 0;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (this.immediate || attacker.effects.TwoTurnAttack > 0)
			{
				pbShowAnimation(this.id, attacker, opponent, 1, alltargets, showanimation); // Charging anim
				//battle.pbDisplay(_INTL("{1} is absorbing power!",attacker.pbThis))
			}
			if (this.immediate)
			{
				this.battle.pbCommonAnimation("UseItem", attacker, null);

				//battle.pbDisplay(_INTL("{1} became fully charged due to its Power Herb!",attacker.pbThis))
				attacker.pbConsumeItem();
			}
			if (attacker.effects.TwoTurnAttack > 0) return 0;
			if (!attacker.pbCanIncreaseStatStage(Stats.SPATK, attacker, false, this) &&
			   !attacker.pbCanIncreaseStatStage(Stats.SPDEF, attacker, false, this) &&
			   !attacker.pbCanIncreaseStatStage(Stats.SPEED, attacker, false, this))
			{
				//battle.pbDisplay(_INTL("{1}'s stats won't go any higher!",attacker.pbThis))
				return -1;
			}
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);

			bool showanim = true;
			if (attacker.pbCanIncreaseStatStage(Stats.SPATK, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.SPATK, 2, attacker, false, this, showanim);
				showanim = false;
			}
			if (attacker.pbCanIncreaseStatStage(Stats.SPDEF, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.SPDEF, 2, attacker, false, this, showanim);
				showanim = false;
			}
			if (attacker.pbCanIncreaseStatStage(Stats.SPEED, attacker, false, this))
			{
				attacker.pbIncreaseStat(Stats.SPEED, 2, attacker, false, this, showanim);
				showanim = false;
			}
			return 0;
		}
	}

	/// <summary>
	/// User gains 3/4 the HP it inflicts as damage. (Draining Kiss, Oblivion Wing)
	/// <summary>
	public class PokeBattle_Move_14F : PokeBattle_Move
	{
		public object isHealingMove()
		{
			return Settings.USENEWBATTLEMECHANICS;
		}

		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			object ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (opponent.damagestate.CalcDamage > 0)
			{
				int hpgain = (int)Math.Round(opponent.damagestate.HPLost * 3 / 4f);
				if (opponent.hasWorkingAbility(Abilities.LIQUID_OOZE))
				{
					attacker.ReduceHP(hpgain, true);
					//battle.pbDisplay(_INTL("{1} sucked up the liquid ooze!",attacker.pbThis))
				}
				else if (attacker.effects.HealBlock == 0)
				{

					if (attacker.hasWorkingItem(Items.BIG_ROOT)) hpgain = (int)Math.Floor(hpgain * 1.3f);

					attacker.RecoverHP(hpgain, true);
					//battle.pbDisplay(_INTL("{1} had its energy drained!",opponent.pbThis))
				}
			}
			return ret;
		}
	}

	/// <summary>
	/// If this move KO's the target, increases the user's Attack by 2 stages.
	/// (Fell Stinger)
	/// <summary>
	public class PokeBattle_Move_150 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			object ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (opponent.damagestate.CalcDamage > 0 && opponent.isFainted())
			{
				if (attacker.pbCanIncreaseStatStage(Stats.ATTACK, attacker, false, this))
				{
					attacker.pbIncreaseStat(Stats.ATTACK, 2, attacker, false, this);
				}
			}
			return ret;
		}
	}

	/// <summary>
	/// Decreases the target's Attack and Special Attack by 1 stage each. Then, user
	/// switches out. Ignores trapping moves. (Parting Shot)
	/// TODO: Pursuit should interrupt this move.
	/// <summary>
	public class PokeBattle_Move_151 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			object ret = -1;
			pbShowAnimation(this.id, attacker, opponent, hitnum, alltargets, showanimation);
			if (!this.isSoundBased() ||
			   attacker.hasMoldBreaker() || !opponent.hasWorkingAbility(Abilities.SOUNDPROOF))
			{
				bool showanim = true;
				if (opponent.pbReduceStat(Stats.ATTACK, 1, attacker, false, this, showanim))
				{
					showanim = false; ret = 0;
				}
				if (opponent.pbReduceStat(Stats.SPATK, 1, attacker, false, this, showanim))
				{
					showanim = false; ret = 0;
				}
			}
			if (!attacker.isFainted() &&
			   this.battle.pbCanChooseNonActive(attacker.Index) &&
			   !this.battle.pbAllFainted(this.battle.pbParty(opponent.Index)))
			{
				attacker.effects.Uturn = true; ret = 0;
			}
			return ret;
		}
	}

	/// <summary>
	/// No Pokémon can switch out or flee until the end of the next round, as long as
	/// the user remains active. (Fairy Lock)
	/// <summary>
	public class PokeBattle_Move_152 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (this.battle.field.FairyLock > 0)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			this.battle.field.FairyLock = 2;
			//battle.pbDisplay(_INTL("No one will be able to run away during the next turn!"))
			return 0;
		}
	}

	/// <summary>
	/// Entry hazard. Lays stealth rocks on the opposing side. (Sticky Web)
	/// <summary>
	public class PokeBattle_Move_153 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (attacker.OpposingSide.StickyWeb)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			attacker.OpposingSide.StickyWeb = true;
			if (!this.battle.isOpposing(attacker.Index))
			{
				//battle.pbDisplay(_INTL("A sticky web has been laid out beneath the opposing team's feet!"))
			}
			else
			{
				//battle.pbDisplay(_INTL("A sticky web has been laid out beneath your team's feet!"))
			}
			return 0;
		}
	}

	/// <summary>
	/// For 5 rounds, creates an electric terrain which boosts Electric-type moves and
	/// prevents Pokémon from falling asleep. Affects non-airborne Pokémon only.
	/// (Electric Terrain)
	/// <summary>
	public class PokeBattle_Move_154 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (this.battle.field.ElectricTerrain > 0)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			this.battle.field.GrassyTerrain = 0;
			this.battle.field.MistyTerrain = 0;
			this.battle.field.ElectricTerrain = 5;
			//battle.pbDisplay(_INTL("An electric current runs across the battlefield!"))
			return 0;
		}
	}

	/// <summary>
	/// For 5 rounds, creates a grassy terrain which boosts Grass-type moves and heals
	/// Pokémon at the end of each round. Affects non-airborne Pokémon only.
	/// (Grassy Terrain)
	/// <summary>
	public class PokeBattle_Move_155 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (this.battle.field.GrassyTerrain > 0)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			this.battle.field.ElectricTerrain = 0;
			this.battle.field.MistyTerrain = 0;
			this.battle.field.GrassyTerrain = 5;
			//battle.pbDisplay(_INTL("Grass grew to cover the battlefield!"))
			return 0;
		}
	}

	/// <summary>
	/// For 5 rounds, creates a misty terrain which weakens Dragon-type moves and
	/// protects Pokémon from status problems. Affects non-airborne Pokémon only.
	/// (Misty Terrain)
	/// <summary>
	public class PokeBattle_Move_156 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (this.battle.field.MistyTerrain > 0)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			this.battle.field.ElectricTerrain = 0;
			this.battle.field.GrassyTerrain = 0;
			this.battle.field.MistyTerrain = 5;
			//battle.pbDisplay(_INTL("Mist swirled about the battlefield!"))
			return 0;
		}
	}

	/// <summary>
	/// Doubles the prize money the player gets after winning the battle. (Happy Hour)
	/// </summary>
	public class PokeBattle_Move_157 : PokeBattle_Move
	{
		public override object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		{
			if (this.battle.isOpposing(attacker.Index) || this.battle.doublemoney)
			{
				//battle.pbDisplay(_INTL("But it failed!"))
				return -1;
			}
			pbShowAnimation(this.id, attacker, null, hitnum, alltargets, showanimation);

			this.battle.doublemoney = true;
			//battle.pbDisplay(_INTL("Everyone is caught up in the happy atmosphere!"))
			return 0;
		}
	}

	/// <summary>
	/// Fails unless user has consumed a berry at some point. (Belch)
	/// </summary>
	public class PokeBattle_Move_158 : PokeBattle_Move
	{
		public object pbMoveFailed(Battle.Battler attacker, Battle.Battler opponent)
		{
			return attacker.Species == Pokemons.NONE || !attacker.belch;
		}
	}
	#endregion
	//===============================================================================
	// NOTE: If you're inventing new move effects, use function code 159 and onwards.
	//===============================================================================
}

//ToDo: Inhereit from InBattleMove
public abstract class PokeBattle_Move : IPokeBattle_Move
{
	public bool overridetype { get; set; }
	public bool sunny { get; set; }
	public bool checks { get; set; }
	public bool doubled { get; set; }
	public bool forcedamage { get; set; }
	public bool immediate { get; set; }
	public bool doubledamage { get; set; }
	public int basedamage { get; set; }
	public int calcbasedmg { get; set; }
	public byte PP { get; set; }
	public byte accuracy { get; set; }
	public List<byte> participants { get; set; }
	public Move.Effect function { get; set; }
	public Items berry { get; set; }
	public Moves id { get; set; }
	public Types type { get; set; }
	public Battle battle { get; set; }
	public virtual int AddlEffect { get { return thismove.AddlEffect; } }
	public Battle.InBattleMove thismove { get; set; }
	public bool isSoundBased()
	{
		return true;
	}
	public bool IsDamaging()
	{
		return true;
	}
	public bool pbIsDamaging()
	{
		return true;
	}
	public bool pbIsPokeBall(Items item)
	{
		return true;
	}
	public bool pbIsBerry(Items item)
	{
		return true;
	}
	public bool pbIsGem(Items item)
	{
		return true;
	}
	public bool pbIsMegaStone(Items item)
	{
		return true;
	}
	public bool ignoresSubstitute(Battle.Battler atk)
	{
		return true;
	}
	public virtual object pbDisplayUseMessage(Battle.Battler attacker)
	{
		return true;
	}
	public Types pbType(Types type, Battle.Battler atk, Battle.Battler opp)
	{
		return Types.NONE;
	}
	public virtual Types pbModifyType(Types type, Battle.Battler atk, Battle.Battler opp)
	{
		return Types.NONE;
	}
	public virtual Types pbTypeModifier(Types type, Battle.Battler atk, Battle.Battler opp)
	{
		return Types.NONE;
	}
	public bool pbTypeImmunityByAbility(Types type, Battle.Battler atk, Battle.Battler opp)
	{
		return true;
	}
	/// <summary>
	/// </summary>
	/// ToDo: I think this only returns null (void) or `-1`; so all codes check for `x < zero`
	public virtual object pbEffect(Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	{
		return null;
	}
	public virtual int pbEffectFixedDamage(int dmg, Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	{
		//return pbEffectFixedDamage(20, attacker, opponent, hitnum, alltargets, showanimation);
		return 0;
	}
	public virtual object pbEffectMessages(Battle.Battler attacker, Battle.Battler opponent, bool ignoretype = false)
	{
		return null;
	}
	public virtual object pbAccuracyCheck(Battle.Battler attacker, Battle.Battler opponent)
	{
		return null;
	}
	public virtual int pbCalcDamage(Battle.Battler attacker, Battle.Battler opponent)
	{
		return 0;
	}
	public virtual int pbCalcDamage(Battle.Battler attacker, Battle.Battler opponent, bool somethinghere) //ToDo: Fix this
	{
		return 0;
	}
	public virtual IEnumerator pbShowAnimation(Moves id, Battle.Battler attacker, Battle.Battler opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	{
		return null;
	}
}

public interface IPokeBattle_Move
{

}