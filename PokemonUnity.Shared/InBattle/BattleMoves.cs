
	public class PokeBattle_UnimplementedMove : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_FailedMove : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Confusion : PokeBattle_Move
		public bool IsPhysical() { return true; }
		public bool IsSpecial() { return false; }
		public override int pbCalcDamage(Pokemon attacker, Pokemon opponent)
		public override object pbEffectMessages(Pokemon attacker, Pokemon opponent, bool ignoretype = false)
	public class PokeBattle_Struggle : PokeBattle_Move
		public bool IsPhysical() { return true; }
		public bool IsSpecial() { return false; }
		public void pbEffectAfterHit(Pokemon attacker, Pokemon opponent, Effects.Move turneffects)
		public override int pbCalcDamage(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_000 : PokeBattle_Move
	public class PokeBattle_Move_001 : PokeBattle_Move
		public bool unusableInGravity()
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_002 : PokeBattle_Struggle
	public class PokeBattle_Move_003 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
		public void pbEffectAfterHit(Pokemon attacker, Pokemon opponent, Effects.Move turneffects)
	public class PokeBattle_Move_004 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_005 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_006 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_007 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_008 : PokeBattle_Move
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
		public object pbModifyBaseAccuracy(byte baseaccuracy, Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_009 : PokeBattle_Move
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_00A : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_00B : PokeBattle_Move
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_00C : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_00D : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
		public object pbModifyBaseAccuracy(byte baseaccuracy, Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_00E : PokeBattle_Move
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_00F : PokeBattle_Move
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_010 : PokeBattle_Move
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
		public object tramplesMinimize(byte param = 1)
	public class PokeBattle_Move_011 : PokeBattle_Move
		public object pbCanUseWhileAsleep()
		public object pbMoveFailed(Pokemon attacker, Pokemon opponent)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_012 : PokeBattle_Move
		public object pbMoveFailed(Pokemon attacker, Pokemon opponent)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_013 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_014 : PokeBattle_Move
		public override int AddlEffect
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_015 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
		public object pbModifyBaseAccuracy(byte baseaccuracy, Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_016 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_017 : PokeBattle_Move
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_018 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_019 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_01A : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_01B : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_01C : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_01D : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_01E : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_01F : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_020 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_021 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_022 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_023 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_024 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_025 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_026 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_027 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_028 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_029 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_02A : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_02B : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_02C : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_02D : PokeBattle_Move
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_02E : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_02F : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_030 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_031 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_032 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_033 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_034 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_035 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_036 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_037 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_038 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_039 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_03A : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_03B : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_03C : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_03D : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_03E : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_03F : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_040 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_041 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_042 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_043 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_044 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
		public object pbModifyDamage(int damagemult, Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_045 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_046 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_047 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_048 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_049 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_04A : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_04B : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_04C : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_04D : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_04E : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_04F : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_050 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_051 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_052 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_053 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_054 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_055 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_056 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_057 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_058 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_059 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_05A : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_05B : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_05C : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_05D : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_05E : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_05F : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_060 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_061 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_062 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_063 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_064 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_065 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_066 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_067 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_068 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_069 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_06A : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_06B : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_06C : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_06D : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_06E : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_06F : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_070 : PokeBattle_Move
		public override object pbAccuracyCheck(Pokemon attacker, Pokemon opponent)
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_071 : PokeBattle_Move
		public void pbAddTarget(byte targets, Pokemon attacker)
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_072 : PokeBattle_Move
		public void pbAddTarget(byte targets, Pokemon attacker)
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_073 : PokeBattle_Move
		public void pbAddTarget(byte targets, Pokemon attacker)
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_074 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_075 : PokeBattle_Move
		public object pbModifyDamage(int damagemult, Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_076 : PokeBattle_Move
		public object pbModifyDamage(int damagemult, Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_077 : PokeBattle_Move
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_078 : PokeBattle_Move
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_079 : PokeBattle_Move
		public object pbBaseDamageMultiplier(int damagemult, Pokemon attacker, Pokemon opponent)
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public override IEnumerator pbShowAnimation(Moves id, Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_07A : PokeBattle_Move
		public object pbBaseDamageMultiplier(int damagemult, Pokemon attacker, Pokemon opponent)
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public override IEnumerator pbShowAnimation(Moves id, Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_07B : PokeBattle_Move
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_07C : PokeBattle_Move
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
		public void pbEffectAfterHit(Pokemon attacker, Pokemon opponent, Effects.Move turneffects)
	public class PokeBattle_Move_07D : PokeBattle_Move
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
		public void pbEffectAfterHit(Pokemon attacker, Pokemon opponent, Effects.Move turneffects)
	public class PokeBattle_Move_07E : PokeBattle_Move
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_07F : PokeBattle_Move
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_080 : PokeBattle_Move
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_081 : PokeBattle_Move
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_082 : PokeBattle_Move
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_083 : PokeBattle_Move
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_084 : PokeBattle_Move
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_085 : PokeBattle_Move
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_086 : PokeBattle_Move
		public object pbBaseDamageMultiplier(int damagemult, Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_087 : PokeBattle_Move
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
		public override Types pbModifyType(Types type, Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_088 : PokeBattle_Move
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
		public override object pbAccuracyCheck(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_089 : PokeBattle_Move
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_08A : PokeBattle_Move
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_08B : PokeBattle_Move
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_08C : PokeBattle_Move
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_08D : PokeBattle_Move
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_08E : PokeBattle_Move
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_08F : PokeBattle_Move
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_090 : PokeBattle_Move
		public override Types pbModifyType(Types type, Pokemon attacker, Pokemon opponent)
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
		public static int[] pbHiddenPower(byte[] iv)
	public class PokeBattle_Move_091 : PokeBattle_Move
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_092 : PokeBattle_Move
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_093 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_094 : PokeBattle_Move
		public object pbOnStartUse(Pokemon attacker)
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_095 : PokeBattle_Move
		public object pbOnStartUse(Pokemon attacker)
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_096 : PokeBattle_Move
		public object pbOnStartUse(Pokemon attacker)
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
		public override Types pbModifyType(Types type, Pokemon attacker, Pokemon opponent)
		public void pbEffectAfterHit(Pokemon attacker, Pokemon opponent, Effects.Move turneffects)
	public class PokeBattle_Move_097 : PokeBattle_Move
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_098 : PokeBattle_Move
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_099 : PokeBattle_Move
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_09A : PokeBattle_Move
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_09B : PokeBattle_Move
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_09C : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_09D : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_09E : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_09F : PokeBattle_Move
		public override Types pbModifyType(Types type, Pokemon attacker, Pokemon opponent)
		public override IEnumerator pbShowAnimation(Moves id, Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0A0 : PokeBattle_Move
		public object pbCritialOverride(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_0A1 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0A2 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0A3 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0A4 : PokeBattle_Move
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
		public override IEnumerator pbShowAnimation(Moves id, Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0A5 : PokeBattle_Move
		public override object pbAccuracyCheck(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_0A6 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0A7 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0A8 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0A9 : PokeBattle_Move
	public class PokeBattle_Move_0AA : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0AB : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0AC : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0AD : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0AE : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0AF : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0B0 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0B1 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0B2 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0B3 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0B4 : PokeBattle_Move
		public bool pbCanUseWhileAsleep()
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0B5 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0B6 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0B7 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0B8 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0B9 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0BA : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0BB : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0BC : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0BD : PokeBattle_Move
		public object pbIsMultiHit()
		public object pbNumHits(Pokemon attacker)
	public class PokeBattle_Move_0BE : PokeBattle_Move
		public object pbIsMultiHit()
		public object pbNumHits(Pokemon attacker)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_0BF : PokeBattle_Move
		public object pbIsMultiHit()
		public object pbNumHits(Pokemon attacker)
		public bool successCheckPerHit()
		public object pbOnStartUse(Pokemon attacker)
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_0C0 : PokeBattle_Move
		public object pbIsMultiHit()
		public object pbNumHits(Pokemon attacker)
	public class PokeBattle_Move_0C1 : PokeBattle_Move
		public object pbIsMultiHit()
		public object pbNumHits(Pokemon attacker)
		public object pbOnStartUse(Pokemon attacker)
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_0C2 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0C3 : PokeBattle_Move
		public object pbTwoTurnAttack(Pokemon attacker)
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0C4 : PokeBattle_Move
		public object pbTwoTurnAttack(Pokemon attacker)
		public object pbBaseDamageMultiplier(int damagemult, Pokemon attacker, Pokemon opponent)
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0C5 : PokeBattle_Move
		public object pbTwoTurnAttack(Pokemon attacker)
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_0C6 : PokeBattle_Move
		public object pbTwoTurnAttack(Pokemon attacker)
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_0C7 : PokeBattle_Move
		public object pbTwoTurnAttack(Pokemon attacker)
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_0C8 : PokeBattle_Move
		public object pbTwoTurnAttack(Pokemon attacker)
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0C9 : PokeBattle_Move
		public bool unusableInGravity()
		public object pbTwoTurnAttack(Pokemon attacker)
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0CA : PokeBattle_Move
		public object pbTwoTurnAttack(Pokemon attacker)
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0CB : PokeBattle_Move
		public object pbTwoTurnAttack(Pokemon attacker)
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0CC : PokeBattle_Move
		public bool unusableInGravity()
		public object pbTwoTurnAttack(Pokemon attacker)
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_0CD : PokeBattle_Move
		public object pbTwoTurnAttack(Pokemon attacker)
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0CE : PokeBattle_Move
		public bool unusableInGravity()
		public object pbMoveFailed(Pokemon attacker, Pokemon opponent)
		public object pbTwoTurnAttack(Pokemon attacker)
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public override Types pbTypeModifier(Types type, Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_0CF : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0D0 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public object pbModifyDamage(int damagemult, Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_0D1 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0D2 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0D3 : PokeBattle_Move
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0D4 : PokeBattle_Move
		public override object pbDisplayUseMessage(Pokemon attacker)
		public void pbAddTarget(byte targets, Pokemon attacker)
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0D5 : PokeBattle_Move
		public bool isHealingMove()
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0D6 : PokeBattle_Move
		public bool isHealingMove()
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0D7 : PokeBattle_Move
		public bool isHealingMove()
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0D8 : PokeBattle_Move
		public bool isHealingMove()
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0D9 : PokeBattle_Move
		public bool isHealingMove()
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0DA : PokeBattle_Move
		public bool isHealingMove()
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0DB : PokeBattle_Move
		public bool isHealingMove()
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0DC : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0DD : PokeBattle_Move
		public bool isHealingMove()
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0DE : PokeBattle_Move
		public bool isHealingMove()
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0DF : PokeBattle_Move
		public bool isHealingMove()
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0E0 : PokeBattle_Move
		public object pbOnStartUse(Pokemon attacker)
		public override IEnumerator pbShowAnimation(Moves id, Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0E1 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public override IEnumerator pbShowAnimation(Moves id, Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0E2 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0E3 : PokeBattle_Move
		public bool isHealingMove()
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0E4 : PokeBattle_Move
		public bool isHealingMove()
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0E5 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0E6 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0E7 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0E8 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0E9 : PokeBattle_Move
	public class PokeBattle_Move_0EA : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0EB : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0EC : PokeBattle_Move
		public void pbEffectAfterHit(Pokemon attacker, Pokemon opponent, Effects.Move turneffects)
	public class PokeBattle_Move_0ED : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0EE : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0EF : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0F0 : PokeBattle_Move
		public void pbEffectAfterHit(Pokemon attacker, Pokemon opponent, Effects.Move turneffects)
		public object pbModifyDamage(int damagemult, Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_0F1 : PokeBattle_Move
		public void pbEffectAfterHit(Pokemon attacker, Pokemon opponent, Effects.Move turneffects)
	public class PokeBattle_Move_0F2 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0F3 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0F4 : PokeBattle_Move
		public void pbEffectAfterHit(Pokemon attacker, Pokemon opponent, Effects.Move turneffects)
	public class PokeBattle_Move_0F5 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0F6 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0F7 : PokeBattle_Move
		public Dictionary<Items, byte> flingarray
		public object pbMoveFailed(Pokemon attacker, Pokemon opponent)
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0F8 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0F9 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_0FA : PokeBattle_Move
		public bool isRecoilMove()
		public void pbEffectAfterHit(Pokemon attacker, Pokemon opponent, Effects.Move turneffects)
	public class PokeBattle_Move_0FB : PokeBattle_Move
		public bool isRecoilMove()
		public void pbEffectAfterHit(Pokemon attacker, Pokemon opponent, Effects.Move turneffects)
	public class PokeBattle_Move_0FC : PokeBattle_Move
		public bool isRecoilMove()
		public void pbEffectAfterHit(Pokemon attacker, Pokemon opponent, Effects.Move turneffects)
	public class PokeBattle_Move_0FD : PokeBattle_Move
		public bool isRecoilMove()
		public void pbEffectAfterHit(Pokemon attacker, Pokemon opponent, Effects.Move turneffects)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_0FE : PokeBattle_Move
		public bool isRecoilMove()
		public void pbEffectAfterHit(Pokemon attacker, Pokemon opponent, Effects.Move turneffects)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_0FF : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_100 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_101 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_102 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_103 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_104 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_105 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_106 : PokeBattle_Move
		public object pbOnStartUse(Pokemon attacker)
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
		public override Types pbModifyType(Types type, Pokemon attacker, Pokemon opponent)
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public override IEnumerator pbShowAnimation(Moves id, Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_107 : PokeBattle_Move
		public object pbOnStartUse(Pokemon attacker)
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
		public override Types pbModifyType(Types type, Pokemon attacker, Pokemon opponent)
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public override IEnumerator pbShowAnimation(Moves id, Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_108 : PokeBattle_Move
		public object pbOnStartUse(Pokemon attacker)
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
		public override Types pbModifyType(Types type, Pokemon attacker, Pokemon opponent)
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public override IEnumerator pbShowAnimation(Moves id, Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_109 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_10A : PokeBattle_Move
		public override int pbCalcDamage(Pokemon attacker, Pokemon opponent)
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public override IEnumerator pbShowAnimation(Moves id, Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_10B : PokeBattle_Move
		public bool isRecoilMove()
		public bool unusableInGravity()
	public class PokeBattle_Move_10C : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_10D : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_10E : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_10F : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_110 : PokeBattle_Move
		public void pbEffectAfterHit(Pokemon attacker, Pokemon opponent, Effects.Move turneffects)
	public class PokeBattle_Move_111 : PokeBattle_Move
		public override object pbDisplayUseMessage(Pokemon attacker)
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public override IEnumerator pbShowAnimation(Moves id, Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_112 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_113 : PokeBattle_Move
		public object pbMoveFailed(Pokemon attacker, Pokemon opponent)
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
		public void pbEffectAfterHit(Pokemon attacker, Pokemon opponent, Effects.Move turneffects)
	public class PokeBattle_Move_114 : PokeBattle_Move
		public object isHealingMove()
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_115 : PokeBattle_Move
		public override object pbDisplayUseMessage(Pokemon attacker)
	public class PokeBattle_Move_116 : PokeBattle_Move
		public object pbMoveFailed(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_117 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_118 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_119 : PokeBattle_Move
		public bool unusableInGravity()
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_11A : PokeBattle_Move
		public bool unusableInGravity()
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_11B : PokeBattle_Move
	public class PokeBattle_Move_11C : PokeBattle_Move
		public object pbBaseDamage(int basedmg, Pokemon attacker, Pokemon opponent)
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_11D : PokeBattle_Move
		public object pbMoveFailed(Pokemon attacker, Pokemon opponent)
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_11E : PokeBattle_Move
		public object pbMoveFailed(Pokemon attacker, Pokemon opponent)
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_11F : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_120 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_121 : PokeBattle_Move
	public class PokeBattle_Move_122 : PokeBattle_Move
	public class PokeBattle_Move_123 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_124 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_125 : PokeBattle_Move
		public object pbMoveFailed(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_133 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_134 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_135 : PokeBattle_Move
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_136 : PokeBattle_Move_01D
	public class PokeBattle_Move_137 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_138 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_139 : PokeBattle_Move
		public override object pbAccuracyCheck(Pokemon attacker, Pokemon opponent)
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_13A : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_13B : PokeBattle_Move
		public object pbMoveFailed(Pokemon attacker, Pokemon opponent)
		public override object pbAccuracyCheck(Pokemon attacker, Pokemon opponent)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_13C : PokeBattle_Move
		public override object pbAccuracyCheck(Pokemon attacker, Pokemon opponent)
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_13D : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public void pbAdditionalEffect(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_13E : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_13F : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_140 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_141 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_142 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_143 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_144 : PokeBattle_Move
		public object pbModifyDamage(int damagemult, Pokemon attacker, Pokemon opponent)
		public bool tramplesMinimize(byte param = 1)
	public class PokeBattle_Move_145 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_146 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_147 : PokeBattle_Move
		public override object pbAccuracyCheck(Pokemon attacker, Pokemon opponent)
	public class PokeBattle_Move_148 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_149 : PokeBattle_Move
		public object pbMoveFailed(Pokemon attacker, Pokemon opponent)
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_14A : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_14B : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_14C : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_14D : PokeBattle_Move
		public object pbTwoTurnAttack(Pokemon attacker)
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
		public bool tramplesMinimize(byte param = 1)
	public class PokeBattle_Move_14E : PokeBattle_Move
		public object pbTwoTurnAttack(Pokemon attacker)
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_14F : PokeBattle_Move
		public object isHealingMove()
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_150 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_151 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_152 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_153 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_154 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_155 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_156 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_157 : PokeBattle_Move
		public override object pbEffect(Pokemon attacker, Pokemon opponent, byte hitnum = 0, byte? alltargets = null, bool showanimation = true)
	public class PokeBattle_Move_158 : PokeBattle_Move
		public object pbMoveFailed(Pokemon attacker, Pokemon opponent)