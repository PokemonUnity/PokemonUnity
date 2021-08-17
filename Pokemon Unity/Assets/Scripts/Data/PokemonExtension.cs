using PokemonUnity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Tempo Use PKU's
public static class PokemonExtension
{
	public static bool IsNotNullOrNone(this Pokemon pokemon)
	{
		return pokemon != null && (Pokemons)pokemon.getID() != Pokemons.NONE;
	}

	public static void PackParty(this Pokemon[] Party)
	{
		Pokemon[] packedArray = new Pokemon[Party.Length];
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

	public static bool HasSpace(this Pokemon[] partyOrPC, int limit)
	{
		if (partyOrPC.GetCount() < limit) return true; //partyOrPC.GetCount().HasValue &&
		else return false;
	}

	public static int GetCount(this Pokemon[] partyOrPC)
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
}
