using PokemonUnity;
using PokemonUnity.Attack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonUnity.Battle
{
	public partial class Effects
	{
		/// <summary>
		/// These effects apply to the usage of a move
		/// </summary>
		public class Move { 
			public int SkipAccuracyCheck { get; set; }
			public int SpecialUsage      { get; set; }
			public int PassedTrying      { get; set; }
			public int TotalDamage       { get; set; }
		}
	}
}