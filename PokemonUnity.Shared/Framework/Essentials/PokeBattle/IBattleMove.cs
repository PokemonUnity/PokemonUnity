using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Combat.Data;
using PokemonUnity.Character;
using PokemonUnity.Saving;
using PokemonUnity.Saving.SerializableClasses;
using PokemonUnity.Utility;
using PokemonEssentials.Interface.PokeBattle.Effects;

namespace PokemonEssentials.Interface.PokeBattle
{
	public interface IBattleMove
	{
		Moves id { get; set; }
		IBattle battle { get; set; }
		string Name { get; }
		//int function { get; set; }
		PokemonUnity.Attack.Data.Effects Effect { get; }
		int basedamage { get; set; }
		Types Type { get; set; }
		int Accuracy { get; set; }
		int AddlEffect { get; }
		PokemonUnity.Attack.Category Category { get; } //ToDo: Move to application layer
		PokemonUnity.Attack.Data.Targets Target { get; set; }
		int Priority { get; set; }
		PokemonUnity.Attack.Data.Flag Flags { get; set; }
		IMove thismove { get; }
		int PP { get; set; }
		int TotalPP { get; set; }

		#region Creating a move
		IBattleMove initialize(IBattle battle, IMove move);

		/// <summary>
		/// This is the code actually used to generate a <see cref="IBattleMove"/> object.  
		/// </summary>
		/// <param name=""></param>
		/// <param name=""></param>
		/// <returns></returns>
		/// The object generated is a subclass of this one which depends on the move's
		/// function code (found in the script section PokeBattle_MoveEffect).
		IBattleMove pbFromPBMove(IBattle battle, IMove move);
		#endregion

		#region About the move
		//int totalpp();
		//int addlEffect();

		int ToInt();

		Types pbModifyType(Types type, IBattler attacker, IBattler opponent);

		Types pbType(Types type, IBattler attacker, IBattler opponent);

		bool pbIsPhysical(Types type);

		bool pbIsSpecial(Types type);

		bool pbIsStatus { get; }

		bool pbIsDamaging();

		bool pbTargetsMultiple(IBattler attacker);

		int pbPriority(IBattler attacker);

		int pbNumHits(IBattler attacker);

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

		bool tramplesMinimize(int param = 1);

		bool successCheckPerHit();

		bool ignoresSubstitute(IBattler attacker);
		#endregion

		#region This move's type effectiveness
		bool pbTypeImmunityByAbility(Types type, IBattler attacker, IBattler opponent);

		float pbTypeModifier(Types type, IBattler attacker, IBattler opponent);

		double pbTypeModMessages(Types type, IBattler attacker, IBattler opponent);
		#endregion

		#region This move's accuracy check
		int pbModifyBaseAccuracy(int baseaccuracy, IBattler attacker, IBattler opponent);

		bool pbAccuracyCheck(IBattler attacker, IBattler opponent);
		#endregion

		#region Damage calculation and modifiers
		bool pbCritialOverride(IBattler attacker, IBattler opponent);

		bool pbIsCritical(IBattler attacker, IBattler opponent);

		int pbBaseDamage(int basedmg, IBattler attacker, IBattler opponent);

		double pbBaseDamageMultiplier(double damagemult, IBattler attacker, IBattler opponent);

		double pbModifyDamage(double damagemult, IBattler attacker, IBattler opponent);

		int pbCalcDamage(IBattler attacker, IBattler opponent, params byte[] options); //= new int[] { 0 }

		int pbReduceHPDamage(int damage, IBattler attacker, IBattler opponent);
		#endregion

		#region Effects
		void pbEffectMessages(IBattler attacker, IBattler opponent, bool ignoretype = false, int[] alltargets = null);

		int pbEffectFixedDamage(int damage, IBattler attacker, IBattler opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true);

		int pbEffect(IBattler attacker, IBattler opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true);

		void pbEffectAfterHit(IBattler attacker, IBattler opponent, IEffectsMove turneffects);
		#endregion

		#region Using the move
		bool pbOnStartUse(IBattler attacker);

		void pbAddTarget(IList<IBattler> targets, IBattler attacker);

		int pbDisplayUseMessage(IBattler attacker);

		void pbShowAnimation(Moves id, IBattler attacker, IBattler opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true);

		void pbOnDamageLost(int damage, IBattler attacker, IBattler opponent);

		bool pbMoveFailed(IBattler attacker, IBattler opponent);
		#endregion
	}
}