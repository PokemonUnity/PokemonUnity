using System.Collections;

namespace PokemonUnity
{
	public static class PokemonExtension
	{
		public static bool IsNotNullOrNone(this PokemonUnity.Monster.Pokemon pokemon)
		{
			return pokemon != null && pokemon.Species != Pokemons.NONE; 
		}
		public static bool IsNotNullOrNone(this PokemonUnity.Combat.Pokemon pokemon)
		{
			return pokemon != null && pokemon.Species != Pokemons.NONE; 
		}
		public static string ToString(this PokemonUnity.Monster.Stats stat, TextScripts text)
		{
			//create a switch, and return Locale Name or Description
			return stat.ToString();
		}
		public static string ToString(this PokemonUnity.Combat.Stats stat, TextScripts text)
		{
			//create a switch, and return Locale Name or Description
			return stat.ToString();
		}
		public static string ToString(this PokemonUnity.Monster.Forms form, TextScripts text)
		{
			//create an operator and return Locale Name
			return form.ToString();
		}
		public static string ToString(this PokemonUnity.Pokemons pokemon, TextScripts text)
		{
			//create a switch, and return Locale Name or Description
			return pokemon.ToString();
		}
	}

	public static class PokemonPartyExtension
	{
		public static void PackParty(this Combat.Pokemon[] Party)
		{
			Combat.Pokemon[] packedArray = new Combat.Pokemon[Party.Length];
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
		public static void PackParty(this PokemonUnity.Monster.Pokemon[] Party)
		{
			PokemonUnity.Monster.Pokemon[] packedArray = new PokemonUnity.Monster.Pokemon[Party.Length];
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
		public static bool HasSpace(this PokemonUnity.Monster.Pokemon[] partyOrPC, int limit)
		{
			if (partyOrPC.GetCount() < limit) return true; //partyOrPC.GetCount().HasValue &&
			else return false;
		}

		public static int GetCount(this PokemonUnity.Monster.Pokemon[] partyOrPC)
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
}