using PokemonUnity;
//using PokemonUnity.Pokemon;
using PokemonUnity.Inventory;
//using PokemonUnity.Attack;
using PokemonUnity.Battle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonUnity.Battle
{
	public partial class Battle 
	{
		/// <summary>
		/// Success state (used for Battle Arena)
		/// </summary>
		public class SuccessState
		{
			/// <summary>
			/// Type effectiveness
			/// </summary>
			public int TypeMod { get; set; }
			/// <summary>
			/// null - not used, 0 - failed, 1 - succeeded
			/// </summary>
			/// instead of an int or enum
			/// 0 - not used, 1 - failed, 2 - succeeded
			public bool? UseState { get; set; }
			public bool Protected { get; set; }
			public int Skill { get; private set; }

			public SuccessState()
			{
				Clear();
			}

			public void Clear()
			{
				TypeMod		= 4;
				UseState	= null;
				Protected	= false;
				Skill		= 0;
			}

			public void UpdateSkill()
			{
				if (!UseState.Value && !Protected)
					Skill -= 2;
				else if (UseState.Value)
				{
					if (TypeMod > 4)
						Skill += 2; // "Super effective"
					else if (TypeMod >= 1 && TypeMod < 4)
						Skill -= 1; // "Not very effective"
					else if (TypeMod == 0)
						Skill -= 2; // Ineffective
					else
						Skill += 1;
				}
				TypeMod = 4;
				UseState = false;
				Protected = false;
			}
		}
	}
}