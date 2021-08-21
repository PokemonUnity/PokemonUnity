using System.Collections;
using PokemonUnity.Monster.Data;

namespace PokemonUnity
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
		public static PokemonUnity.Combat.TypeEffective GetCombinedEffectiveness(this PokemonUnity.Types atk, PokemonUnity.Types target1, PokemonUnity.Types target2 = Types.NONE, PokemonUnity.Types target3 = Types.NONE)
		{
			return PokemonUnity.Monster.Data.Type.GetCombinedEffectiveness(atk, target1, target2, target3);
		}
		/// <summary>
		/// Converts the string of a Pokemon Type to a color's hex value
		/// </summary>
		/// <param name="PokemonType">string of pokemon type or name of a color</param>
		/// <returns>returns color as hex to be used in Unity.Color</returns>
		/// <example>StringToColor(Electric)</example>
		/// <example>StringToColor(Yellow)</example>
		/// ToDo: Convert to Unity's Color? 
		public static string ToColorHEX(this PokemonUnity.Types type, int pallete = 0)
		{
			//private System.Collections.Generic.Dictionary<string, Color> StringToColorDic = new System.Collections.Generic.Dictionary<string, Color>() {//Dictionary<PokemonData.Type, Color>
			if(pallete == 1)
			//http://www.serebiiforums.com/showthread.php?289595-Pokemon-type-color-associations
			switch (type)
			{
				case Types.NORMAL:
					//Normal Type:		
					//Normal -white
					return "A8A77A";
				case Types.FIGHTING:
					//Fighting Type:	
					//Fighting - dark red
					return "C22E28";
				case Types.FLYING:
					//Flying Type:		
					//Flying - light blue
					return "A98FF3";
				case Types.POISON:
					//Poison Type:		
					//Poison -purple
					return "A33EA1";
				case Types.GROUND:
					//Ground Type:		
					//Ground - brown
					return "E2BF65";
				case Types.ROCK:
					//Rock Type:		
					//Rock - gray
					return "B6A136";
				case Types.BUG:
					//Bug Type:		
					//Bug - yellow green
					return "A6B91A";
				case Types.GHOST:
					//Ghost Type:		
					//Ghost - light purple
					return "735797";
				case Types.STEEL:
					//Steel Type:		
					//Steel - dark gray
					return "B7B7CE";
				case Types.FIRE:
					//Fire Type:		
					//Fire - red
					return "EE8130";
				case Types.WATER:
					//Water Type:		
					//Water -blue
					return "6390F0";
				case Types.GRASS:
					//Grass Type:		
					//Grass - green
					return "7AC74C";
				case Types.ELECTRIC:
					//Electric Type:	
					//Electric -yellow
					return "F7D02C";
				case Types.PSYCHIC:
					//Psychic Type:	
					//Psychic - magenta
					return "F95587";
				case Types.ICE:
					//Ice Type:		
					//Ice - cyan
					return "96D9D6";
				case Types.DRAGON:
					//Dragon Type:		
					//Dragon - orange
					return "6F35FC";
				case Types.DARK:
					//Dark Type:		
					//Dark - black
					return "705746";
				case Types.FAIRY:
					//Fairy Type:		
					return "D685AD";
				case Types.NONE:
				case Types.UNKNOWN:
				case Types.SHADOW:
				default:
					return string.Empty;
					break;
			}
			else 
			//http://www.epidemicjohto.com/t882-type-colors-hex-colors
			switch (type)
			{
				case Types.NORMAL:
					//Normal Type:		
					return "A8A77A";
				case Types.FIGHTING:
					//Fighting Type:	
					return "C22E28";
				case Types.FLYING:
					//Flying Type:		
					return "A98FF3";
				case Types.POISON:
					//Poison Type:		
					return "A33EA1";
				case Types.GROUND:
					//Ground Type:		
					return "E2BF65";
				case Types.ROCK:
					//Rock Type:		
					return "B6A136";
				case Types.BUG:
					//Bug Type:		
					return "A6B91A";
				case Types.GHOST:
					//Ghost Type:		
					return "735797";
				case Types.STEEL:
					//Steel Type:		
					return "B7B7CE";
				case Types.FIRE:
					//Fire Type:		
					return "EE8130";
				case Types.WATER:
					//Water Type:		
					return "6390F0";
				case Types.GRASS:
					//Grass Type:		
					return "7AC74C";
				case Types.ELECTRIC:
					//Electric Type:	
					return "F7D02C";
				case Types.PSYCHIC:
					//Psychic Type:	
					return "F95587";
				case Types.ICE:
					//Ice Type:		
					return "96D9D6";
				case Types.DRAGON:
					//Dragon Type:		
					return "6F35FC";
				case Types.DARK:
					//Dark Type:		
					return "705746";
				case Types.FAIRY:
					//Fairy Type:		
					return "D685AD";
				case Types.NONE:
				case Types.UNKNOWN:
				case Types.SHADOW:
				default:
					//Black?
					return string.Empty;
			}
		}
	}
}