using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Attack;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;

namespace PokemonUnity.Monster.Data
{
	/// <summary>
	/// </summary>
	public struct Type
	{
		#region Variables
		private Dictionary<Types,byte> Table { get; set; }
		private Types Base { get; set; }
		public PokemonUnity.Attack.Category? Category { get; private set; }
		public float this[Types target]
		{
			// 0 = takes no damage
			// 1 = takes reduced damage (half)
			// 2 = takes normal damage (1x)
			// 4 = takes extra damage (doubled)
			get { return Table[target] * 0.01f; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// </summary>
		/// <param name="atk"></param>
		/// <param name="target"></param>
		/// <returns>
		/// 0 = takes no damage
		/// 1 = takes reduced damage (half)
		/// 2 = takes normal damage (1x)
		/// 4 = takes extra damage (doubled)
		/// </returns>
		public static float GetEffectiveness(PokemonUnity.Types atk, PokemonUnity.Types target)
		{
			if (target == Types.NONE)
				return 2;//return 1?
			return Kernal.TypeData[atk][target]; //ToDo: Change from float to int
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
		public static Combat.TypeEffective GetCombinedEffectiveness(PokemonUnity.Types atk, PokemonUnity.Types target1, PokemonUnity.Types target2 = Types.NONE, PokemonUnity.Types target3 = Types.NONE)
		{
			float e = GetCombinedEffectivenessModifier(atk, target1, target2, target3);
			if (e <= 0) //Equal to zero, but would be considered a bug if negative number
				return Combat.TypeEffective.Ineffective;
			else if (e > 0 && e < 8)
				return Combat.TypeEffective.NotVeryEffective;
			else if (e == 8)
				return Combat.TypeEffective.NormalEffective;
			else //if (e > 8)
				return Combat.TypeEffective.SuperEffective;
			//return TypeEffective.Broken; //Less than 0...
		}
		public string ToString(TextScripts text)
		{
			return Base.ToString(text);
		}
		#endregion

		/// <summary>
		/// </summary>
		/// <param name="atk">attacking type used</param>
		/// <param name="table">
		/// recipient type damage before multiplied by 0.01f<para>
		/// 0 = takes no damage</para>
		/// 100 = takes reduced damage (half)<para>
		/// 200 = takes normal damage (1x)</para>
		/// 400 = takes extra damage (doubled)
		/// </param>
		/// <param name="category"></param>
		public Type(Types atk, IDictionary<Types,byte> table, PokemonUnity.Attack.Category? category = null)//Types target, int factor
		{
			Base = atk;
			Table = (Dictionary<Types,byte>)table;
			Category = category;
		}
	}
}