using System.Collections;

public static class PBTypes
{
    //public static string ToString(this PokemonUnity.Types type)
	//{
	//	switch (type)
	//	{
	//		case PokemonUnity.Types.NONE:
	//			break;
	//		case PokemonUnity.Types.NORMAL:
	//			break;
	//		case PokemonUnity.Types.FIGHTING:
	//			break;
	//		case PokemonUnity.Types.FLYING:
	//			break;
	//		case PokemonUnity.Types.POISON:
	//			break;
	//		case PokemonUnity.Types.GROUND:
	//			break;
	//		case PokemonUnity.Types.ROCK:
	//			break;
	//		case PokemonUnity.Types.BUG:
	//			break;
	//		case PokemonUnity.Types.GHOST:
	//			break;
	//		case PokemonUnity.Types.STEEL:
	//			break;
	//		case PokemonUnity.Types.FIRE:
	//			break;
	//		case PokemonUnity.Types.WATER:
	//			break;
	//		case PokemonUnity.Types.GRASS:
	//			break;
	//		case PokemonUnity.Types.ELECTRIC:
	//			break;
	//		case PokemonUnity.Types.PSYCHIC:
	//			break;
	//		case PokemonUnity.Types.ICE:
	//			break;
	//		case PokemonUnity.Types.DRAGON:
	//			break;
	//		case PokemonUnity.Types.DARK:
	//			break;
	//		case PokemonUnity.Types.FAIRY:
	//			break;
	//		case PokemonUnity.Types.UNKNOWN:
	//			break;
	//		case PokemonUnity.Types.SHADOW:
	//			break;
	//		default:
	//			break;
	//	}
	//	return string.Empty;
	//}
    public static string getName(PokemonUnity.Types type)
	{
		return string.Empty;
	}
	public static int maxValue { get; set; } //{ get { return MoveData.Count; } }
	public static int getEffectiveness(PokemonUnity.Types type1, PokemonUnity.Types type2)
	{
		return 0;
	}
	public static int getCombinedEffectiveness(PokemonUnity.Types type1, PokemonUnity.Types type2, PokemonUnity.Types type3, PokemonUnity.Types type4)
	{
		return 0;
	}
	public static bool isIneffective(PokemonUnity.Types type1, PokemonUnity.Types type2)
	{
		return false;
	}
	public static bool isSuperEffective(PokemonUnity.Types type1, PokemonUnity.Types type2)
	{
		return false;
	}
	public static bool isSpecialType(PokemonUnity.Types type)
	{
		return false;
	}
}

namespace PokemonUnity.Monster
{
	public class Type
	{
    
	}
}