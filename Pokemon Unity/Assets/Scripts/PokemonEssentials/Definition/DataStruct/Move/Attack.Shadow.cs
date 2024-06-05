using System;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity.Combat;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;

namespace PokemonUnity.Interface.UnityEngine
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
		public override int GetEffect(IBattler attacker, IBattler opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true) { int r = -1; this.GetEffect(attacker, opponent, hitnum, alltargets, showanimation); return r; }
		public override IEnumerator GetEffect(IBattlerIE attacker, IBattlerIE opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true, System.Action<int> result=null)
		{
			int ret = -1; _host.StartCoroutine(base.GetEffect(attacker, opponent, hitnum, alltargets, showanimation, result:value=>ret=value)); result(ret);
			if (ret >= 0 && attacker is IBattlerShadowPokemon a) a.HyperMode();
			result(ret);
			yield break;
		}
	}

	/// <summary>
	/// Paralyzes the target. (Shadow Bolt)
	/// </summary>
	public class PokeBattle_Move_127 : PokeBattle_Move_007
	{
		public PokeBattle_Move_127() : base() { }
		//public PokeBattle_Move_127(Battle battle, Attack.Move move) : base(battle, move) { }
		public override int GetEffect(IBattler attacker, IBattler opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true) { int r = -1; this.GetEffect(attacker, opponent, hitnum, alltargets, showanimation); return r; }
		public override IEnumerator GetEffect(IBattlerIE attacker, IBattlerIE opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true, System.Action<int> result=null)
		{
			int ret = -1; _host.StartCoroutine(base.GetEffect(attacker, opponent, hitnum, alltargets, showanimation, result:value=>ret=value)); result(ret);
			if (ret >= 0 && attacker is IBattlerShadowPokemon a) a.HyperMode();
			result(ret);
			yield break;
		}
	}

	/// <summary>
	/// Burns the target. (Shadow Fire)
	/// </summary>
	public class PokeBattle_Move_128 : PokeBattle_Move_00A
	{
		public PokeBattle_Move_128() : base() { }
		//public PokeBattle_Move_128(Battle battle, Attack.Move move) : base(battle, move) { }
		public override int GetEffect(IBattler attacker, IBattler opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true) { int r = -1; this.GetEffect(attacker, opponent, hitnum, alltargets, showanimation); return r; }
		public override IEnumerator GetEffect(IBattlerIE attacker, IBattlerIE opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true, System.Action<int> result=null)
		{
			int ret = -1; _host.StartCoroutine(base.GetEffect(attacker, opponent, hitnum, alltargets, showanimation, result:value=>ret=value)); result(ret);
			if (ret >= 0 && attacker is IBattlerShadowPokemon a) a.HyperMode();
			result(ret);
			yield break;
		}
	}

	/// <summary>
	/// Freezes the target. (Shadow Chill)
	/// </summary>
	public class PokeBattle_Move_129 : PokeBattle_Move_00C
	{
		public PokeBattle_Move_129() : base() { }
		//public PokeBattle_Move_129(Battle battle, Attack.Move move) : base(battle, move) { }
		public override int GetEffect(IBattler attacker, IBattler opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true) { int r = -1; this.GetEffect(attacker, opponent, hitnum, alltargets, showanimation); return r; }
		public override IEnumerator GetEffect(IBattlerIE attacker, IBattlerIE opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true, System.Action<int> result=null)
		{
			int ret = -1; _host.StartCoroutine(base.GetEffect(attacker, opponent, hitnum, alltargets, showanimation, result:value=>ret=value)); result(ret);
			if (ret >= 0 && attacker is IBattlerShadowPokemon a) a.HyperMode();
			result(ret);
			yield break;
		}
	}

	/// <summary>
	/// Confuses the target. (Shadow Panic)
	/// </summary>
	public class PokeBattle_Move_12A : PokeBattle_Move_013
	{
		public PokeBattle_Move_12A() : base() { }
		//public PokeBattle_Move_12A(Battle battle, Attack.Move move) : base(battle, move) { }
		public override int GetEffect(IBattler attacker, IBattler opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true) { int r = -1; this.GetEffect(attacker, opponent, hitnum, alltargets, showanimation); return r; }
		public override IEnumerator GetEffect(IBattlerIE attacker, IBattlerIE opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true, System.Action<int> result=null)
		{
			int ret = -1; _host.StartCoroutine(base.GetEffect(attacker, opponent, hitnum, alltargets, showanimation, result:value=>ret=value)); result(ret);
			if (ret >= 0 && attacker is IBattlerShadowPokemon a) a.HyperMode();
			result(ret);
			yield break;
		}
	}

	/// <summary>
	/// Decreases the target's Defense by 2 stages. (Shadow Down)
	/// </summary>
	public class PokeBattle_Move_12B : PokeBattle_Move_04C
	{
		public PokeBattle_Move_12B() : base() { }
		//public PokeBattle_Move_12B(Battle battle, Attack.Move move) : base(battle, move) { }
		public override int GetEffect(IBattler attacker, IBattler opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true) { int r = -1; this.GetEffect(attacker, opponent, hitnum, alltargets, showanimation); return r; }
		public override IEnumerator GetEffect(IBattlerIE attacker, IBattlerIE opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true, System.Action<int> result=null)
		{
			int ret = -1; _host.StartCoroutine(base.GetEffect(attacker, opponent, hitnum, alltargets, showanimation, result:value=>ret=value)); result(ret);
			if (ret >= 0 && attacker is IBattlerShadowPokemon a) a.HyperMode();
			result(ret);
			yield break;
		}
	}

	/// <summary>
	/// Decreases the target's evasion by 2 stages. (Shadow Mist)
	/// </summary>
	public class PokeBattle_Move_12C : PokeBattle_Move
	{
		public PokeBattle_Move_12C() : base() { }
		//public PokeBattle_Move_12C(Battle battle, Attack.Move move) : base(battle, move) { }
		public override int GetEffect(IBattler attacker, IBattler opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true) { int r = -1; this.GetEffect(attacker, opponent, hitnum, alltargets, showanimation); return r; }
		public override IEnumerator GetEffect(IBattlerIE attacker, IBattlerIE opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true, System.Action<int> result=null)
		{
			bool ret = false;  if (opponent is IBattlerEffectIE b) b.CanReduceStatStage(Stats.EVASION, attacker, true, this, result:value=>ret=value); if(!ret) { result(-1); yield break; }
			_host.StartCoroutine(ShowAnimation(MoveId, attacker, opponent, hitnum, alltargets, showanimation));
			ret = false; if (opponent is IBattlerEffectIE b0) b0.ReduceStat(Stats.EVASION, increment: 2, attacker: attacker, showMessages: false, result: value=>ret=value);
			if (ret && attacker is IBattlerShadowPokemon a) a.HyperMode();
			result(ret ? 0 : -1);
			yield break;
		}
	}

	/// <summary>
	/// Power is doubled if the target is using Dive. (Shadow Storm)
	/// </summary>
	public class PokeBattle_Move_12D : PokeBattle_Move_075
	{
		public PokeBattle_Move_12D() : base() { }
		//public PokeBattle_Move_12D(Battle battle, Attack.Move move) : base(battle, move) { }
		public override int GetEffect(IBattler attacker, IBattler opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true) { int r = -1; this.GetEffect(attacker, opponent, hitnum, alltargets, showanimation); return r; }
		public override IEnumerator GetEffect(IBattlerIE attacker, IBattlerIE opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true, System.Action<int> result=null)
		{
			int ret = -1; _host.StartCoroutine(base.GetEffect(attacker, opponent, hitnum, alltargets, showanimation, result:value=>ret=value)); result(ret);
			if (ret >= 0 && attacker is IBattlerShadowPokemon a) a.HyperMode();
			result(ret);
			yield break;
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
		public override int GetEffect(IBattler attacker, IBattler opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true) { int r = -1; this.GetEffect(attacker, opponent, hitnum, alltargets, showanimation); return r; }
		public override IEnumerator GetEffect(IBattlerIE attacker, IBattlerIE opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true, System.Action<int> result=null)
		{
			IList<int> affected = new List<int>();
			for (int i = 0; i < 4; i++)
			{
				if (this.battle.battlers[i].HP > 1) affected.Add(i);
			}
			if (affected.Count == 0)
			{
				_host.StartCoroutine(this.battle.Display(Game._INTL("But it failed!")));
				result(-1);
				yield break;
			}
			_host.StartCoroutine(ShowAnimation(MoveId, attacker, opponent, hitnum, alltargets, showanimation));
			for (int i = 0; i < affected.Count; i++)
			{
				this.battle.battlers[i].ReduceHP((int)Math.Floor(this.battle.battlers[i].HP / 2d));
			}
			_host.StartCoroutine(this.battle.Display(Game._INTL("Each Pokemon's HP was halved!")));
			attacker.effects.HyperBeam = 2;
			attacker.currentMove = MoveId;
			result(0);
			yield break;
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
		public override int GetEffect(IBattler attacker, IBattler opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true) { int r = -1; this.GetEffect(attacker, opponent, hitnum, alltargets, showanimation); return r; }
		public override IEnumerator GetEffect(IBattlerIE attacker, IBattlerIE opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true, System.Action<int> result=null)
		{
			int ret = -1; _host.StartCoroutine(base.GetEffect(attacker, opponent, hitnum, alltargets, showanimation, result:value=>ret=value)); result(ret);
			if (ret >= 0 && attacker is IBattlerShadowPokemon a) a.HyperMode();
			result(ret);
			yield break;
		}
	}

	/// <summary>
	/// User takes recoil damage equal to 1/2 of its current HP. (Shadow End)
	/// </summary>
	public class PokeBattle_Move_130 : PokeBattle_Move
	{
		public PokeBattle_Move_130() : base() { }
		//public PokeBattle_Move_130(Battle battle, Attack.Move move) : base(battle, move) { }
		public override int GetEffect(IBattler attacker, IBattler opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true) { int r = -1; this.GetEffect(attacker, opponent, hitnum, alltargets, showanimation); return r; }
		public override IEnumerator GetEffect(IBattlerIE attacker, IBattlerIE opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true, System.Action<int> result=null)
		{
			int ret = -1; _host.StartCoroutine(base.GetEffect(attacker, opponent, hitnum, alltargets, showanimation, result:value=>ret=value)); result(ret);
			if (ret >= 0 && attacker is IBattlerShadowPokemon a) a.HyperMode();
			result(ret);
			yield break;
		}

		public override void EffectAfterHit(IBattler attacker, IBattler opponent, IEffectsMove turneffects) { this.EffectAfterHit((IBattlerIE)attacker, (IBattlerIE)opponent, turneffects); }
		public override IEnumerator EffectAfterHit(IBattlerIE attacker, IBattlerIE opponent, IEffectsMove turneffects)
		{
			if (!attacker.isFainted() && turneffects.TotalDamage > 0)
			{
				_host.StartCoroutine(attacker.ReduceHP((int)Math.Round(attacker.HP / 2.0)));
				_host.StartCoroutine(this.battle.Display(Game._INTL("{1} is damaged by recoil!", attacker.ToString())));
			}
			yield break;
		}
	}

	/// <summary>
	/// Starts shadow weather. (Shadow Sky)
	/// </summary>
	public class PokeBattle_Move_131 : PokeBattle_Move
	{
		public PokeBattle_Move_131() : base() { }
		//public PokeBattle_Move_131(Battle battle, Attack.Move move) : base(battle, move) { }
		public override int GetEffect(IBattler attacker, IBattler opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true) { int r = -1; this.GetEffect(attacker, opponent, hitnum, alltargets, showanimation); return r; }
		public override IEnumerator GetEffect(IBattlerIE attacker, IBattlerIE opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true, System.Action<int> result=null)
		{
			switch (this.battle.weather)
			{
				case Weather.HEAVYRAIN:
					_host.StartCoroutine(this.battle.Display(Game._INTL("There is no relief from this heavy rain!")));
					result(-1);
					yield break;
				case Weather.HARSHSUN:
					_host.StartCoroutine(this.battle.Display(Game._INTL("The extremely harsh sunlight was not lessened at all!")));
					result(-1);
					yield break;
				case Weather.SHADOWSKY:
					_host.StartCoroutine(this.battle.Display(Game._INTL("But it failed!")));
					result(-1);
					yield break;
			}
			_host.StartCoroutine(ShowAnimation(MoveId, attacker, null, hitnum, alltargets, showanimation));
			this.battle.weather = Weather.SHADOWSKY;
			this.battle.weatherduration = 5;
			_host.StartCoroutine(this.battle.CommonAnimation("ShadowSky", null, null));
			_host.StartCoroutine(this.battle.Display(Game._INTL("A shadow sky appeared!")));
			result(0);
			yield break;
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
		public override int GetEffect(IBattler attacker, IBattler opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true) { int r = -1; this.GetEffect(attacker, opponent, hitnum, alltargets, showanimation); return r; }
		public override IEnumerator GetEffect(IBattlerIE attacker, IBattlerIE opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true, System.Action<int> result=null)
		{
			if ((this.battle.sides[0].Reflect > 0 ||
			   this.battle.sides[1].Reflect > 0 ||
			   this.battle.sides[0].LightScreen > 0 ||
			   this.battle.sides[1].LightScreen > 0 ||
			   this.battle.sides[0].Safeguard > 0 ||
			   this.battle.sides[1].Safeguard > 0) && attacker is IBattlerShadowPokemon a)
			{
				_host.StartCoroutine(ShowAnimation(MoveId, attacker, null, hitnum, alltargets, showanimation));
				this.battle.sides[0].Reflect = 0;
				this.battle.sides[1].Reflect = 0;
				this.battle.sides[0].LightScreen = 0;
				this.battle.sides[1].LightScreen = 0;
				this.battle.sides[0].Safeguard = 0;
				this.battle.sides[1].Safeguard = 0;
				_host.StartCoroutine(this.battle.Display(Game._INTL("It broke all barriers!")));
				a.HyperMode();
				result(0);
				yield break;
			}
			else
			{
				_host.StartCoroutine(this.battle.Display(Game._INTL("But it failed!")));
				result(-1);
				yield break;
			}
		}
	}
	#endregion
#pragma warning restore 0162 //Warning CS0162  Unreachable code detected
	//===============================================================================
	// NOTE: If you're inventing new move effects, use function code 159 and onwards.
	//===============================================================================
}