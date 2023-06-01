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
		IBattleMove FromMove(IBattle battle, IMove move);
		#endregion

		#region About the move
		//int totalpp();
		//int addlEffect();

		int ToInt();

		Types ModifyType(Types type, IBattler attacker, IBattler opponent);

		Types GetType(Types type, IBattler attacker, IBattler opponent);

		bool IsPhysical(Types type);

		bool IsSpecial(Types type);

		bool IsStatus { get; }

		bool IsDamaging();

		bool TargetsMultiple(IBattler attacker);

		int GetPriority(IBattler attacker);

		int NumHits(IBattler attacker);

		bool IsMultiHit();

		bool TwoTurnAttack(IBattler attacker);

		void AdditionalEffect(IBattler attacker, IBattler opponent);

		bool CanUseWhileAsleep();

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
		bool TypeImmunityByAbility(Types type, IBattler attacker, IBattler opponent);

		float TypeModifier(Types type, IBattler attacker, IBattler opponent);

		double TypeModMessages(Types type, IBattler attacker, IBattler opponent);
		#endregion

		#region This move's accuracy check
		int ModifyBaseAccuracy(int baseaccuracy, IBattler attacker, IBattler opponent);

		bool AccuracyCheck(IBattler attacker, IBattler opponent);
		#endregion

		#region Damage calculation and modifiers
		bool CritialOverride(IBattler attacker, IBattler opponent);

		bool IsCritical(IBattler attacker, IBattler opponent);

		int BaseDamage(int basedmg, IBattler attacker, IBattler opponent);

		double BaseDamageMultiplier(double damagemult, IBattler attacker, IBattler opponent);

		double ModifyDamage(double damagemult, IBattler attacker, IBattler opponent);

		int CalcDamage(IBattler attacker, IBattler opponent, params byte[] options); //= new int[] { 0 }

		int ReduceHPDamage(int damage, IBattler attacker, IBattler opponent);
		#endregion

		#region Effects
		void EffectMessages(IBattler attacker, IBattler opponent, bool ignoretype = false, int[] alltargets = null);

		int EffectFixedDamage(int damage, IBattler attacker, IBattler opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true);

		int GetEffect(IBattler attacker, IBattler opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true);

		void EffectAfterHit(IBattler attacker, IBattler opponent, IEffectsMove turneffects);
		#endregion

		#region Using the move
		bool OnStartUse(IBattler attacker);

		void AddTarget(IList<IBattler> targets, IBattler attacker);

		int DisplayUseMessage(IBattler attacker);

		void ShowAnimation(Moves id, IBattler attacker, IBattler opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true);

		void OnDamageLost(int damage, IBattler attacker, IBattler opponent);

		bool MoveFailed(IBattler attacker, IBattler opponent);
		#endregion
	}
}