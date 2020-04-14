using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Attack;
using PokemonUnity.Inventory;

namespace PokemonUnity.Monster.Data
{
	/// <summary>
	/// </summary>
	public class Type
	{
		#region Variables
		private Dictionary<Types,byte> Table { get; set; }
		private Types Base { get; set; }
		public PokemonUnity.Attack.Category? Category { get; private set; }
		public float this[Types target]
		{
			get { return Table[target] * 0.01f; }
		}
		#endregion

		#region Methods
		public static float GetEffectiveness(PokemonUnity.Types atk, PokemonUnity.Types target)
		{
			if (target == Types.NONE)
				return 1;
			return Game.TypeData[atk][target];
		}
		public static float GetCombinedEffectivenessModifier(PokemonUnity.Types atk, PokemonUnity.Types target1, PokemonUnity.Types target2 = Types.NONE, PokemonUnity.Types target3 = Types.NONE)
		{
			float mod1 = GetEffectiveness(atk, target1);
			float mod2 = 1;
			if(target2 >= 0 && target1 != target2)
				mod2 = GetEffectiveness(atk, target2);
			float mod3 = 1;
			if(target3 >= 0 && target1 != target3 && target2 != target3)
				mod3 = GetEffectiveness(atk, target2);
			return mod1 * mod2 * mod3;
		}
		public static TypeEffective GetCombinedEffectiveness(PokemonUnity.Types atk, PokemonUnity.Types target1, PokemonUnity.Types target2 = Types.NONE, PokemonUnity.Types target3 = Types.NONE)
		{
			float e = GetCombinedEffectivenessModifier(atk, target1, target2, target3);
			if (e == 0)
				return TypeEffective.Ineffective;
			else if (e > 0 && e < 8)
				return TypeEffective.NotVeryEffective;
			else if (e == 8)
				return TypeEffective.NormalEffective;
			else //if (e > 8)
				return TypeEffective.SuperEffective;
			//return TypeEffective.Broken;
		}
		public string ToString(TextScripts text)
		{
			//create a switch, and return Locale Name or Description
			return Base.ToString(text);
		}
		#endregion

		public Type(Types atk, IDictionary<Types,byte> table, PokemonUnity.Attack.Category? category = null)//Types target, int factor
		{
			Base = atk;
			Table = (Dictionary<Types,byte>)table;
		}

		public enum TypeEffective
		{
			//Broken,
			Ineffective,
			NotVeryEffective,
			NormalEffective,
			SuperEffective
		}
	}
}