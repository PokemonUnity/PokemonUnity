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
	public interface IEncounterData
	{
		#region Variables
		/// <summary>
		/// Identifier for this Encounter (Used for record lookup, and tracking/storage)
		/// </summary>
		int Id { get; }
		/// <summary>
		/// Map or Scene/Level this encounter record belongs to (associated with)
		/// </summary>
		int MapId { get; }
		Method Method { get; }
		//Slots Slot { get; } //ToDo { get { return Game.EncounterSlotData[Generation][SlotId]; } }
		int SlotId { get; }
		/// <summary>
		/// </summary>
		//Pokemons Pokemon { get; }
		Pokemons[] Pokemon { get; }
		/// <summary>
		/// </summary>
		ConditionValue[] Conditions { get; }
		/// <summary>
		/// </summary>
		Versions[] Versions { get; }
		/// <summary>
		/// </summary>
		int Generation { get; }
		/// <summary>
		/// </summary>
		int MinLevel { get; }
		/// <summary>
		/// </summary>
		int MaxLevel { get; }
		/// <summary>
		/// </summary>
		int Rarity { get; }
		#endregion

		//EncounterData(int id, int mapId, Method method, int slotId, Pokemons[] pokemon = null, ConditionValue[] conditions = null, int generation = 0, int minLevel = 1, int maxLevel = 1, int rarity = 0, Versions[] versions = null)
		//{
		//	Id = id;
		//	MapId = mapId;
		//	Pokemon = pokemon ?? new Pokemons[] { Pokemons.NONE };
		//	Conditions = conditions;// ?? throw new ArgumentNullException(nameof(conditions));
		//	Generation = generation;
		//	MinLevel = Math.Min(minLevel, maxLevel);
		//	MaxLevel = Math.Max(minLevel, maxLevel);
		//	Method = method;
		//	Rarity = rarity;
		//	SlotId = slotId;//versions.Contains(x => (int)x == 15) ? slotId : slotId - 1; //if 15, first slot is 0
		//	Versions = versions;
		//}

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
	/*public struct EncounterSlotData
	{
		/// <summary>
		/// </summary>
		Method Method { get; }
		/// <summary>
		/// </summary>
		int[] Slots { get; }
		/// <summary>
		/// </summary>
		int[] Generation { get; }
		/// <summary>
		/// </summary>
		int[] Rate { get; }
		int[] IdGroup { get; }

		//foreach (EncounterSlotData encounter in Game.EncounterData[Method])
		int this[int version, int slot] //if 15, first slot is 0; fixed in sql
		//{ get { return encounter.Generation.Contains(15) ? Rate[i] : Rate[i-1]; } }
		{ get { return Rate[slot-1]; } }
	}*/
}