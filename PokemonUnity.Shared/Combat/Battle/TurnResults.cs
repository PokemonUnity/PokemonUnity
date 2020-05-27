using PokemonUnity;
//using PokemonUnity.Pokemon;
using PokemonUnity.Inventory;
//using PokemonUnity.Attack;
using PokemonUnity.Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonUnity.Combat
{
	/// <summary>
	/// </summary>
	public struct Turn
	{
		public Battle.Choice[] Choices { get; private set; }
		public Battle.SuccessState[] SuccessStates { get; private set; }
		public Battle.DamageState[] DamageStates { get; private set; }
		public int[] FinalDamages { get; private set; }
		public int[] Orders { get; private set; }
	}
}