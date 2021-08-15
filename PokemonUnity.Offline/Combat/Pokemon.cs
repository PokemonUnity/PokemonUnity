using PokemonUnity;
//using PokemonUnity.Pokemon;
using PokemonUnity.Inventory;
//using PokemonUnity.Attack;
using PokemonUnity.Localization;
using PokemonUnity.Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonUnity.Offline.Combat
{
	public partial class Pokemon : PokemonUnity.Combat.Pokemon
	{
		#region Constructors
		public Pokemon(Battle btl, sbyte idx) : base(btl, idx) 
		{
		}
		#endregion
	}
}