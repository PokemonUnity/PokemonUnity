using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Attack;
using PokemonUnity.Attack.Data;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;
using PokemonUnity.Combat.Data;
using PokemonUnity.Character;
using PokemonUnity.Utility;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
//using PokemonEssentials.Interface.PokeBattle.Rules;
using PokemonEssentials.Interface.EventArg;

namespace PokemonEssentials.Interface.PokeBattle
{
	public interface IPokeBattle_Move
	{
		/// <summary>
		/// This move's ID
		/// </summary>
		Moves id { get; set; }
		IBattle battle { get; set; }
		string name { get; }
		//int function { get; }
		PokemonUnity.Attack.Data.Effects Effect { get; }
		int basedamage { get; set; }
		/// <summary>
		/// Gets this move's type.
		/// </summary>
		Types type { get; set; }
		int accuracy { get; set; }
		int addlEffect { get; }
		PokemonUnity.Attack.Data.Targets target { get; set; }
		int priority { get; set; }
		PokemonUnity.Attack.Data.Flag flags { get; set; }
		IMove thismove { get; }
		int pp { get; set; }
		int totalpp { get; set; }
		//PokemonUnity.Attack.Category category { get; set; }


		// ###############################################################################
		// Creating a move
		// ###############################################################################
		IPokeBattle_Move initialize(IBattle battle, IMove move);

		/// <summary>
		/// This is the code actually used to generate a PokeBattle_Move object.  The
		/// object generated is a subclass of this one which depends on the move's
		/// function code (found in the script section PokeBattle_MoveEffect).
		/// </summary>
		/// <param name="battle"></param>
		/// <param name="move"></param>
		/// <returns></returns>
		IPokeBattle_Move pbFromPBMove(IBattle battle, IMove move);

		// ###############################################################################
		// About the move
		// ###############################################################################
		Types pbModifyType(Types type, IBattler attacker, IBattler opponent);

		Types pbType(Types type, IBattler attacker, IBattler opponent);

		bool pbIsPhysical(Types type);

		bool pbIsSpecial(Types type);

		bool pbIsStatus();

		bool pbIsDamaging();

		bool pbTargetsMultiple(IBattler attacker);

		int pbPriority(IBattler attacker);

		int pbNumHits(IBattler attacker);

		// not the same as pbNumHits>1
		bool pbIsMultiHit();

		bool pbTwoTurnAttack(IBattler attacker);

		void pbAdditionalEffect(IBattler attacker, IBattler opponent);

		bool pbCanUseWhileAsleep();

		bool isHealingMove();

		bool isRecoilMove();

		bool unusableInGravity();

		bool isContactMove();

		bool canProtectAgainst();

		bool canMagicCoat();

		bool canSnatch();

		bool canMirrorMove();

		bool canKingsRock();

		bool canThawUser();

		bool hasHighCriticalRate();

		bool isBitingMove();

		bool isPunchingMove();

		bool isSoundBased();

		bool isPowderMove();

		bool isPulseMove();

		bool isBombMove();

		// Causes perfect accuracy and double damage
		bool tramplesMinimize(int param = 1);

		bool successCheckPerHit();

		bool ignoresSubstitute(IBattler attacker);

		// ###############################################################################
		// This move's type effectiveness
		// ###############################################################################
		bool pbTypeImmunityByAbility(Types type, IBattler attacker, IBattler opponent);

		float pbTypeModifier(Types type, IBattler attacker, IBattler opponent);

		double pbTypeModMessages(Types type, IBattler attacker, IBattler opponent);

		// ###############################################################################
		// This move's accuracy check
		// ###############################################################################
		int pbModifyBaseAccuracy(int baseaccuracy, IBattler attacker, IBattler opponent);

		bool pbAccuracyCheck(IBattler attacker, IBattler opponent);

		// ###############################################################################
		// Damage calculation and modifiers
		// ###############################################################################
		bool pbCritialOverride(IBattler attacker, IBattler opponent);

		bool pbIsCritical(IBattler attacker, IBattler opponent);

		int pbBaseDamage(int basedmg, IBattler attacker, IBattler opponent);

		double pbBaseDamageMultiplier(double damagemult, IBattler attacker, IBattler opponent);

		double pbModifyDamage(double damagemult, IBattler attacker, IBattler opponent);

		int pbCalcDamage(IBattler attacker, IBattler opponent, params byte[] options); //options = 0

		int pbReduceHPDamage(int damage, IBattler attacker, IBattler opponent);

		// ###############################################################################
		// Effects
		// ###############################################################################
		void pbEffectMessages(IBattler attacker, IBattler opponent, bool ignoretype = false, int[] alltargets = null);

		int pbEffectFixedDamage(int damage, IBattler attacker, IBattler opponent, byte hitnum = 0, int[] alltargets = null, bool showanimation = true);

		int pbEffect(IBattler attacker, IBattler opponent, byte hitnum = 0, int[] alltargets = null, bool showanimation = true);

		void pbEffectAfterHit(IBattler attacker, IBattler opponent, IEffectsMove turneffects);

		// ###############################################################################
		// Using the move
		// ###############################################################################
		bool pbOnStartUse(IBattler attacker);

		void pbAddTarget(IBattler[] targets, IBattler attacker);

		int pbDisplayUseMessage(IBattler attacker);

		void pbShowAnimation(Moves id, IBattler attacker, IBattler opponent, byte hitnum = 0, int[] alltargets = null, bool showanimation = true);

		void pbOnDamageLost(int damage, IBattler attacker, IBattler opponent);

		bool pbMoveFailed(IBattler attacker, IBattler opponent);
	}
}