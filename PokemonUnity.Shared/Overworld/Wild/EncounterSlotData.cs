using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity.Monster;
//using PokemonUnity.Overworld.EncounterData;

namespace PokemonUnity.Overworld
{
	[Serializable]
	public struct EncounterSlotData
	{
		#region Variables
		/// <summary>
		/// </summary>
		//public int Id { get; private set; }
		/// <summary>
		/// </summary>
		//public int Area { get; private set; }
		//public Method Method { get; private set; }
		//public Slots Slot { get; } //ToDo { get { return Game.EncounterSlotData[Generation][SlotId]; } }
		//public int SlotId { get; private set; }
		/// <summary>
		/// </summary>
		public Pokemons Pokemon { get; private set; }
		//public Pokemons[] Pokemon { get; private set; }
		/// <summary>
		/// </summary>
		//public ConditionValue[] Conditions { get; private set; }
		/// <summary>
		/// </summary>
		//public Versions[] Versions { get; private set; }
		/// <summary>
		/// </summary>
		//public int Generation { get; private set; }
		/// <summary>
		/// </summary>
		public int MinLevel { get; private set; }
		/// <summary>
		/// </summary>
		public int MaxLevel { get; private set; }
		/// <summary>
		/// Frequency means high likely to see the same pokemon again (in a random shuffle)
		/// </summary>
		//public int Rarity { get; private set; }
		public int Frequency { get; private set; }
		#endregion

		//public EncounterSlotData(int id, int area, Method method, int slotId, Pokemons pokemon = Pokemons.NONE, ConditionValue[] conditions = null, int generation = 0, int minLevel = 1, int maxLevel = 1, int rarity = 0, Versions[] versions = null)
		public EncounterSlotData(Pokemons pokemon = Pokemons.NONE, int minLevel = 1, int maxLevel = 1, int frequency = 0)
		{
			//Id = id;
			//Area = area;
			Pokemon = pokemon;// ?? new Pokemons[] { Pokemons.NONE };
			//Conditions = conditions;// ?? throw new ArgumentNullException(nameof(conditions));
			//Generation = generation;
			MinLevel = Math.Min(minLevel, maxLevel);
			MaxLevel = Math.Max(minLevel, maxLevel);
			//Method = method;
			//Rarity = rarity;
			Frequency = frequency;
			//SlotId = slotId;//versions.Contains(x => (int)x == 15) ? slotId : slotId - 1; //if 15, first slot is 0
			//Versions = versions;
		}

		//public override int GetHashCode()
		//{
		//	//return Id.GetHashCode();
		//	return Area.GetHashCode() ^ Method.GetHashCode() ^ SlotId.GetHashCode();
		//}
		//public override bool Equals(object obj)
		//{
		//	if (obj == null || GetType() != obj.GetType())
		//	{
		//		return false;
		//	}
		//	return Equals((Tuple<T, U, W>)obj);
		//}
		//public bool Equals(Tuple<T, U, W> other)
		//{
		//	return other.first.Equals(Area) && other.second.Equals(Method) && other.third.Equals(SlotId);
		//}
	}
}