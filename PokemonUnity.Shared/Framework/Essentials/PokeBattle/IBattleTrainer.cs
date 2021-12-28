using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;
using PokemonUnity.Combat.Data;
using PokemonUnity.Character;
using PokemonUnity.Saving;
using PokemonUnity.Saving.SerializableClasses;
using PokemonUnity.Utility;

namespace PokemonEssentials.Interface.PokeBattle
{
	public interface ITrainer
	{
		string name { get; set; }
		int id { get; set; }
		int? metaID { get; set; }
		TrainerTypes trainertype { get; set; }
		int? outfit { get; set; }
		bool[] badges { get; }
		int Money { get; set; }
		IDictionary<Pokemons, bool> seen { get; }
		IDictionary<Pokemons,bool> owned { get; }
		int?[][] formseen { get; }
		KeyValuePair<int,int>[] formlastseen { get; }
		IList<Pokemons> shadowcaught { get; set; }
		IPokemon[] party { get; set; }
		/// <summary>
		/// Whether the Pokédex was obtained
		/// </summary>
		bool pokedex { get; set; }
		/// <summary>
		/// Whether the Pokégear was obtained
		/// </summary>
		bool pokegear { get; set; }
		Languages? language { get; }
		/// <summary>
		/// Name of this trainer type (localized) 
		/// </summary>
		string trainerTypeName { get; }

		string fullname { get; }

		int publicID(int? id = null);

		int secretID(int? id = null);

		int getForeignID();

		void setForeignID(ITrainer other);

		//int metaID();

		int moneyEarned { get; }

		int skill { get; }

		string skillCode { get; }

		bool hasSkillCode(string code);

		int numbadges { get; }

		int gender { get; }

		bool isMale { get; }
		bool isFemale { get; }

		IEnumerable<IPokemon> pokemonParty { get; }

		IEnumerable<IPokemon> ablePokemonParty { get; }

		int partyCount { get; }

		int pokemonCount { get; }

		int ablePokemonCount { get; }

		IPokemon firstParty { get; }

		IPokemon firstPokemon { get; }

		IPokemon firstAblePokemon { get; }

		IPokemon lastParty { get; }

		IPokemon lastPokemon { get; }

		IPokemon lastAblePokemon { get; }

		int pokedexSeen(Regions? region = null); //int region = -1

		int pokedexOwned(Regions? region = null); //int region = -1

		int numFormsSeen(Pokemons species);

		bool hasSeen(Pokemons species);

		bool hasOwned(Pokemons species);

		void setSeen(Pokemons species);

		void setOwned(Pokemons species);

		void clearPokedex();

		ITrainer initialize(string name, TrainerTypes trainertype);
	}
}