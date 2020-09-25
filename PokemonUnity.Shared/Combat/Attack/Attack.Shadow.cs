using System;
using System.Collections.Generic;

namespace PokemonUnity.Combat
{
#pragma warning disable 0162 //Warning CS0162  Unreachable code detected 
	#region Shadow Moves
	//===============================================================================
	// NOTE: Shadow moves use function codes 126-132 inclusive.
	//===============================================================================

	/// <summary>
	/// No additional effect. (Shadow Blast, Shadow Blitz, Shadow Break, Shadow Rave,
	/// Shadow Rush, Shadow Wave)
	/// </summary>
	public class PokeBattle_Move_126 : PokeBattle_Move_000
	{
		public PokeBattle_Move_126() : base() { }
		//public PokeBattle_Move_126(Battle battle, Attack.Move move) : base(battle, move) { }
		public override int pbEffect(Pokemon attacker, Pokemon opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true)
		{
			int ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (ret >= 0) attacker.pbHyperMode();
			return ret;
		}
	}

	/// <summary>
	/// Paralyzes the target. (Shadow Bolt)
	/// </summary>
	public class PokeBattle_Move_127 : PokeBattle_Move_007
	{
		public PokeBattle_Move_127() : base() { }
		//public PokeBattle_Move_127(Battle battle, Attack.Move move) : base(battle, move) { }
		public override int pbEffect(Pokemon attacker, Pokemon opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true)
		{
			int ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (ret >= 0) attacker.pbHyperMode();
			return ret;
		}
	}

	/// <summary>
	/// Burns the target. (Shadow Fire)
	/// </summary>
	public class PokeBattle_Move_128 : PokeBattle_Move_00A
	{
		public PokeBattle_Move_128() : base() { }
		//public PokeBattle_Move_128(Battle battle, Attack.Move move) : base(battle, move) { }
		public override int pbEffect(Pokemon attacker, Pokemon opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true)
		{
			int ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (ret >= 0) attacker.pbHyperMode();
			return ret;
		}
	}

	/// <summary>
	/// Freezes the target. (Shadow Chill)
	/// </summary>
	public class PokeBattle_Move_129 : PokeBattle_Move_00C
	{
		public PokeBattle_Move_129() : base() { }
		//public PokeBattle_Move_129(Battle battle, Attack.Move move) : base(battle, move) { }
		public override int pbEffect(Pokemon attacker, Pokemon opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true)
		{
			int ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (ret >= 0) attacker.pbHyperMode();
			return ret;
		}
	}

	/// <summary>
	/// Confuses the target. (Shadow Panic)
	/// </summary>
	public class PokeBattle_Move_12A : PokeBattle_Move_013
	{
		public PokeBattle_Move_12A() : base() { }
		//public PokeBattle_Move_12A(Battle battle, Attack.Move move) : base(battle, move) { }
		public override int pbEffect(Pokemon attacker, Pokemon opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true)
		{
			int ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (ret >= 0) attacker.pbHyperMode();
			return ret;
		}
	}

	/// <summary>
	/// Decreases the target's Defense by 2 stages. (Shadow Down)
	/// </summary>
	public class PokeBattle_Move_12B : PokeBattle_Move_04C
	{
		public PokeBattle_Move_12B() : base() { }
		//public PokeBattle_Move_12B(Battle battle, Attack.Move move) : base(battle, move) { }
		public override int pbEffect(Pokemon attacker, Pokemon opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true)
		{
			int ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (ret >= 0) attacker.pbHyperMode();
			return ret;
		}
	}

	/// <summary>
	/// Decreases the target's evasion by 2 stages. (Shadow Mist)
	/// </summary>
	public class PokeBattle_Move_12C : PokeBattle_Move
	{
		public PokeBattle_Move_12C() : base() { }
		//public PokeBattle_Move_12C(Battle battle, Attack.Move move) : base(battle, move) { }
		public override int pbEffect(Pokemon attacker, Pokemon opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true)
		{
			if (!opponent.pbCanReduceStatStage(Stats.EVASION, attacker, true, this)) return -1;
			pbShowAnimation(MoveId, attacker, opponent, hitnum, alltargets, showanimation);
			bool ret = opponent.pbReduceStat(Stats.EVASION, increment: 2, attacker: attacker, showMessages: false);
			if (ret) attacker.pbHyperMode();
			return ret ? 0 : -1;
		}
	}

	/// <summary>
	/// Power is doubled if the target is using Dive. (Shadow Storm)
	/// </summary>
	public class PokeBattle_Move_12D : PokeBattle_Move_075
	{
		public PokeBattle_Move_12D() : base() { }
		//public PokeBattle_Move_12D(Battle battle, Attack.Move move) : base(battle, move) { }
		public override int pbEffect(Pokemon attacker, Pokemon opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true)
		{
			int ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (ret >= 0) attacker.pbHyperMode();
			return ret;
		}
	}

	/// <summary>
	/// Two turn attack.  On first turn, halves the HP of all active Pokémon.
	/// Skips second turn (if successful). (Shadow Half)
	/// </summary>
	public class PokeBattle_Move_12E : PokeBattle_Move
	{
		public PokeBattle_Move_12E() : base() { }
		//public PokeBattle_Move_12E(Battle battle, Attack.Move move) : base(battle, move) { }
		public override int pbEffect(Pokemon attacker, Pokemon opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true)
		{
			List<int> affected = new List<int>();
			for (int i = 0; i < 4; i++)
			{
				if (this.Battle.battlers[i].HP > 1) affected.Add(i);
			}
			if (affected.Count == 0)
			{
				this.Battle.pbDisplay(Game._INTL("But it failed!"));
				return -1;
			}
			pbShowAnimation(MoveId, attacker, opponent, hitnum, alltargets, showanimation);
			for (int i = 0; i < affected.Count; i++)
			{
				this.Battle.battlers[i].ReduceHP((int)Math.Floor(this.Battle.battlers[i].HP / 2d));
			}
			this.Battle.pbDisplay(Game._INTL("Each Pokemon's HP was halved!"));
			attacker.effects.HyperBeam = 2;
			attacker.currentMove = MoveId;
			return 0;
		}
	}

	/// <summary>
	/// Target can no longer switch out or flee, as long as the user remains active.
	/// (Shadow Hold)
	/// </summary>
	public class PokeBattle_Move_12F : PokeBattle_Move_0EF
	{
		public PokeBattle_Move_12F() : base() { }
		//public PokeBattle_Move_12F(Battle battle, Attack.Move move) : base(battle, move) { }
		public override int pbEffect(Pokemon attacker, Pokemon opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true)
		{
			int ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (ret >= 0) attacker.pbHyperMode();
			return ret;
		}
	}

	/// <summary>
	/// User takes recoil damage equal to 1/2 of its current HP. (Shadow End)
	/// </summary>
	public class PokeBattle_Move_130 : PokeBattle_Move
	{
		public PokeBattle_Move_130() : base() { }
		//public PokeBattle_Move_130(Battle battle, Attack.Move move) : base(battle, move) { }
		public override int pbEffect(Pokemon attacker, Pokemon opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true)
		{
			int ret = base.pbEffect(attacker, opponent, hitnum, alltargets, showanimation);
			if (ret >= 0) attacker.pbHyperMode();
			return ret;
		}

		public override void pbEffectAfterHit(Pokemon attacker, Pokemon opponent, Effects.Move turneffects)
		{
			if (!attacker.isFainted() && turneffects.TotalDamage > 0)
			{
				attacker.ReduceHP((int)Math.Round(attacker.HP / 2.0));
				this.Battle.pbDisplay(Game._INTL("{1} is damaged by recoil!", attacker.ToString()));
			}
		}
	}

	/// <summary>
	/// Starts shadow weather. (Shadow Sky)
	/// </summary>
	public class PokeBattle_Move_131 : PokeBattle_Move
	{
		public PokeBattle_Move_131() : base() { }
		//public PokeBattle_Move_131(Battle battle, Attack.Move move) : base(battle, move) { }
		public override int pbEffect(Pokemon attacker, Pokemon opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true)
		{
			switch (this.Battle.weather)
			{
				case Weather.HEAVYRAIN:
					this.Battle.pbDisplay(Game._INTL("There is no relief from this heavy rain!"));
					return -1;
				case Weather.HARSHSUN:
					this.Battle.pbDisplay(Game._INTL("The extremely harsh sunlight was not lessened at all!"));
					return -1;
				case Weather.SHADOWSKY:
					this.Battle.pbDisplay(Game._INTL("But it failed!"));
					return -1;
			}
			pbShowAnimation(MoveId, attacker, null, hitnum, alltargets, showanimation);
			this.Battle.weather = Weather.SHADOWSKY;
			this.Battle.weatherduration = 5;
			this.Battle.pbCommonAnimation("ShadowSky", null, null);
			this.Battle.pbDisplay(Game._INTL("A shadow sky appeared!"));
			return 0;
		}
	}

	/// <summary>
	/// Ends the effects of Light Screen, Reflect and Safeguard on both sides.
	/// (Shadow Shed)
	/// </summary>
	public class PokeBattle_Move_132 : PokeBattle_Move
	{
		public PokeBattle_Move_132() : base() { }
		//public PokeBattle_Move_132(Battle battle, Attack.Move move) : base(battle, move) { }
		public override int pbEffect(Pokemon attacker, Pokemon opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true)
		{
			if (this.Battle.sides[0].Reflect > 0 ||
			   this.Battle.sides[1].Reflect > 0 ||
			   this.Battle.sides[0].LightScreen > 0 ||
			   this.Battle.sides[1].LightScreen > 0 ||
			   this.Battle.sides[0].Safeguard > 0 ||
			   this.Battle.sides[1].Safeguard > 0)
			{
				pbShowAnimation(MoveId, attacker, null, hitnum, alltargets, showanimation);
				this.Battle.sides[0].Reflect = 0;
				this.Battle.sides[1].Reflect = 0;
				this.Battle.sides[0].LightScreen = 0;
				this.Battle.sides[1].LightScreen = 0;
				this.Battle.sides[0].Safeguard = 0;
				this.Battle.sides[1].Safeguard = 0;
				this.Battle.pbDisplay(Game._INTL("It broke all barriers!"));
				attacker.pbHyperMode();
				return 0;
			}
			else
			{
				this.Battle.pbDisplay(Game._INTL("But it failed!"));
				return -1;
			}
		}
	}
	#endregion
#pragma warning restore 0162 //Warning CS0162  Unreachable code detected 
	//===============================================================================
	// NOTE: If you're inventing new move effects, use function code 159 and onwards.
	//===============================================================================
}