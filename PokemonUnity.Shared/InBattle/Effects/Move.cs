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
			public bool SkipAccuracyCheck	{ get; set; }
			public bool SpecialUsage		{ get; set; }
			public bool PassedTrying		{ get; set; }
			public int TotalDamage			{ get; set; }
		}
	}
}