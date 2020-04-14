using System.Collections;
using PokemonUnity.Monster.Data;

namespace PokemonUnity.Monster
{
	public static class TypeExtension
	{		
		public static string ToString(this PokemonUnity.Types type, TextScripts text)
		{
			//create a switch, and return Locale Name or Description
			return type.ToString();
		}
		public static float GetEffectiveness(this PokemonUnity.Types atk, PokemonUnity.Types target)
		{
			return PokemonUnity.Monster.Data.Type.GetEffectiveness(atk, target);
		}
		public static float GetCombinedEffectivenessModifier(this PokemonUnity.Types atk, PokemonUnity.Types target1, PokemonUnity.Types target2 = Types.NONE, PokemonUnity.Types target3 = Types.NONE)
		{
			return PokemonUnity.Monster.Data.Type.GetCombinedEffectivenessModifier(atk, target1, target2, target3);
		}
		public static PokemonUnity.Monster.Data.Type.TypeEffective GetCombinedEffectiveness(this PokemonUnity.Types atk, PokemonUnity.Types target1, PokemonUnity.Types target2 = Types.NONE, PokemonUnity.Types target3 = Types.NONE)
		{
			return PokemonUnity.Monster.Data.Type.GetCombinedEffectiveness(atk, target1, target2, target3);
		}
	}
}