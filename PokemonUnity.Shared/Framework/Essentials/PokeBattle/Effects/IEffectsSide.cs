using PokemonUnity;
using PokemonUnity.Attack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonEssentials.Interface.PokeBattle.Effects
{
	/// <summary>
	/// These effects apply to a side
	/// </summary>
	public interface IEffectsSide
	{
		bool CraftyShield { get; set; }
		byte EchoedVoiceCounter { get; set; }
		bool EchoedVoiceUsed { get; set; }
		int LastRoundFainted { get; set; }
		byte LightScreen { get; set; }
		byte LuckyChant { get; set; }
		bool MatBlock { get; set; }
		byte Mist { get; set; }
		bool QuickGuard { get; set; }
		byte Rainbow { get; set; }
		byte Reflect { get; set; }
		byte Round { get; set; }
		byte Safeguard { get; set; }
		byte SeaOfFire { get; set; }
		byte Spikes { get; set; }
		bool StealthRock { get; set; }
		bool StickyWeb { get; set; }
		byte Swamp { get; set; }
		byte Tailwind { get; set; }
		byte ToxicSpikes { get; set; }
		bool WideGuard { get; set; }
	}
}