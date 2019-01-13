using System.Collections;


public class PBTypes
{
    public static string getName(PokemonUnity.Pokemon.Types type)
	{
		return string.Empty;
	}
	public static int maxValue { get; set; } //{ get { return MoveData.Count; } }
	public static int getEffectiveness(PokemonUnity.Pokemon.Types type1, PokemonUnity.Pokemon.Types type2)
	{
		return 0;
	}
	public static int getCombinedEffectiveness(PokemonUnity.Pokemon.Types type1, PokemonUnity.Pokemon.Types type2, PokemonUnity.Pokemon.Types type3, PokemonUnity.Pokemon.Types type4)
	{
		return 0;
	}
}

namespace PokemonUnity.Pokemon
{
	public class Type
	{
    
	}
	public enum Types
	{
		NONE = 0,
		NORMAL = 1,
		FIGHTING = 2,
		FLYING = 3,
		POISON = 4,
		GROUND = 5,
		ROCK = 6,
		BUG = 7,
		GHOST = 8,
		STEEL = 9,
		FIRE = 10,
		WATER = 11,
		GRASS = 12,
		ELECTRIC = 13,
		PSYCHIC = 14,
		ICE = 15,
		DRAGON = 16,
		DARK = 17,
		FAIRY = 18,
		UNKNOWN = 10001,
		SHADOW = 10002
	};
}