using PokemonUnity;
using PokemonUnity.Attack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonUnity.Combat
{
	public partial class Effects
	{	   
		/// <summary>
		/// These effects apply to a side
		/// </summary>
		public class Side : PokemonEssentials.Interface.PokeBattle.Effects.IEffectsSide { 
			public bool CraftyShield		{ get; set; }
			public byte EchoedVoiceCounter	{ get; set; }
			public bool EchoedVoiceUsed		{ get; set; }
			public int LastRoundFainted	{ get; set; }
			public byte LightScreen			{ get; set; }
			public byte LuckyChant			{ get; set; }
			public bool MatBlock			{ get; set; }
			public byte Mist				{ get; set; }
			public bool QuickGuard			{ get; set; }
			public byte Rainbow				{ get; set; }
			public byte Reflect				{ get; set; }
			public byte Round				{ get; set; }
			public byte Safeguard			{ get; set; }
			public byte SeaOfFire			{ get; set; }
			public byte Spikes				{ get; set; }
			public bool StealthRock			{ get; set; }
			public bool StickyWeb			{ get; set; }
			public byte Swamp				{ get; set; }
			public byte Tailwind			{ get; set; }
			public byte ToxicSpikes			{ get; set; }
			public bool WideGuard			{ get; set; }
		}
	}
}