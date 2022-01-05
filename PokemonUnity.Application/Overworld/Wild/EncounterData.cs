using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity.Monster;

namespace PokemonUnity.Overworld
{
	/// <summary>
	/// Data records of pokemon profiles used for pooling together 
	/// a random chance enounter for a battle against wild pokemons
	/// </summary>
	public struct EncounterData : IEncounterData
	{
		#region Variables
		/// <summary>
		/// Identifier for this Encounter (Used for record lookup, and tracking/storage)
		/// </summary>
		public int Id { get; }
		/// <summary>
		/// Map or Scene/Level this encounter record belongs to (associated with)
		/// </summary>
		public int MapId { get; }
		public Method Method { get; }
		//Slots Slot { get; } //ToDo { get { return Kernal.EncounterSlotData[Generation][SlotId]; } }
		public int SlotId { get; }
		/// <summary>
		/// </summary>
		//Pokemons Pokemon { get; }
		public Pokemons[] Pokemon { get; }
		/// <summary>
		/// </summary>
		public ConditionValue[] Conditions { get; }
		/// <summary>
		/// </summary>
		public Versions[] Versions { get; }
		/// <summary>
		/// </summary>
		public int Generation { get; }
		/// <summary>
		/// </summary>
		public int MinLevel { get; }
		/// <summary>
		/// </summary>
		public int MaxLevel { get; }
		/// <summary>
		/// </summary>
		public int Rarity { get; }
		#endregion

		public EncounterData(int id, int mapId, Method method, int slotId, Pokemons[] pokemon = null, ConditionValue[] conditions = null, int generation = 0, int minLevel = 1, int maxLevel = 1, int rarity = 0, Versions[] versions = null)
		{
			Id = id;
			MapId = mapId;
			Pokemon = pokemon ?? new Pokemons[] { Pokemons.NONE };
			Conditions = conditions;// ?? throw new ArgumentNullException(nameof(conditions));
			Generation = generation;
			MinLevel = Math.Min(minLevel, maxLevel);
			MaxLevel = Math.Max(minLevel, maxLevel);
			Method = method;
			Rarity = rarity;
			SlotId = slotId;//versions.Contains(x => (int)x == 15) ? slotId : slotId - 1; //if 15, first slot is 0
			Versions = versions;
		}

		/// <summary>
		/// </summary>
		/// <param name="method"></param>
		/// <returns></returns>
		/// ToDo: Consider increasing chance on rarity as encounter increases 
		//static PokemonEssentials.Interface.PokeBattle.IPokemon GetWildPokemon(Method method);

		//static Pokemon WildPokemonRNG(Method method); //conditions
		//static bool EncounterConditionsMet(ConditionValue[] encounter);
		//override int GetHashCode();
		//override bool Equals(object obj);
		//bool Equals(Tuple<T, U, W> other);
	}
}