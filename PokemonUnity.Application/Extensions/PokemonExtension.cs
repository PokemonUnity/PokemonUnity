﻿using System.Collections;
using PokemonUnity;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.Field;

namespace PokemonUnity
{
	public static class PokemonExtension
	{
		public static bool IsNotNullOrNone(this IEncounter pokemon)
		{
			return pokemon != null && pokemon.Pokemon != Pokemons.NONE; 
		}
		public static bool IsNotNullOrNone(this IPokemon pokemon)
		{
			return pokemon != null && pokemon.Species != Pokemons.NONE; 
		}
		public static bool IsNotNullOrNone(this IBattler pokemon)
		{
			return pokemon != null && pokemon.Species != Pokemons.NONE; 
		}
		//public static string ToString(this PokemonUnity.Monster.Stats stat, TextScripts text)
		//{
		//	//create a switch, and return Locale Name or Description
		//	return stat.ToString();
		//}
		//public static string ToString(this PokemonUnity.Combat.Stats stat, TextScripts text)
		//{
		//	//create a switch, and return Locale Name or Description
		//	return stat.ToString();
		//}
		//public static string ToString(this PokemonUnity.Monster.Forms form, TextScripts text)
		//{
		//	//create an operator and return Locale Name
		//	return form.ToString();
		//}
		//public static string ToString(this PokemonUnity.Pokemons pokemon, TextScripts text)
		//{
		//	//create a switch, and return Locale Name or Description
		//	return pokemon.ToString();
		//}
		//
		//public static string ToIdString(this PokemonUnity.Pokemons pokemon, int digits = 3)
		//{
		//	return System.Text.RegularExpressions.Regex.Replace(((int)pokemon).ToString(), "[0-9]+", match => match.Value.PadLeft(digits, '0'));
		//}
	}

	public static class PokemonPartyExtension
	{
		public static void PackParty(this IBattler[] Party)
		{
			IBattler[] packedArray = new IBattler[Party.Length];
			int i2 = 0; //counter for packed array
			for (int i = 0; i < Party.Length; i++)
			{
				if (Party[i].IsNotNullOrNone())// != null || Party[i].Species != Pokemons.NONE)
				{
					//if next object in box has a value
					packedArray[i2] = Party[i]; //add to packed array
					i2 += 1; //ready packed array's next position
				}
			}
			for (int i = 0; i < Party.Length; i++)
				Party[i] = packedArray[i];
		}
		public static void PackParty(this IPokemon[] Party)
		{
			IPokemon[] packedArray = new IPokemon[Party.Length];
			int i2 = 0; //counter for packed array
			for (int i = 0; i < Party.Length; i++)
			{
				if (Party[i].IsNotNullOrNone())// != null || Party[i].Species != Pokemons.NONE)
				{
					//if next object in box has a value
					packedArray[i2] = Party[i]; //add to packed array
					i2 += 1; //ready packed array's next position
				}
			}
			for (int i = 0; i < Party.Length; i++)
				Party[i] = packedArray[i];
		}
		public static bool HasSpace(this IPokemon[] partyOrPC, int limit)
		{
			if (partyOrPC.GetCount() < limit) return true; //partyOrPC.GetCount().HasValue &&
			else return false;
		}

		public static int GetCount(this IPokemon[] partyOrPC)
		{
			int result = 0;
			for (int i = 0; i < partyOrPC.Length; i++)
			{
				if (partyOrPC[i].IsNotNullOrNone())// != null || partyOrPC[i].Species != Pokemons.NONE)
				{
					result += 1;
				}
			}
			return result;
		}

		public static int GetBattleCount(this IPokemon[] partyOrPC)
		{
			int result = 0;
			for (int i = 0; i < partyOrPC.Length; i++)
			{
				if (partyOrPC[i].IsNotNullOrNone() && !partyOrPC[i].isEgg)// != null || partyOrPC[i].Species != Pokemons.NONE)
				{
					result += 1;
				}
			}
			return result;
		}
	}
}