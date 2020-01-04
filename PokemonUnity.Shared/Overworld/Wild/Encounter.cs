using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity.Monster;
//using PokemonUnity.Overworld.EncounterData;

namespace PokemonUnity.Overworld
{
	//ToDo { get { return Game.EncounterData[Location][SlotId]; } }
	public struct Encounter
	{
		/// <summary>
		/// </summary>
		public int Id { get; private set; }
		/// <summary>
		/// </summary>
		public int Area { get; private set; }
		public Method Method { get; private set; }
		//public Slots Slot { get; } //ToDo { get { return Game.EncounterSlotData[Generation][SlotId]; } }
		public int SlotId { get; private set; }
		/// <summary>
		/// </summary>
		//public Pokemons Pokemon { get; private set; }
		public Pokemons[] Pokemon { get; private set; }
		/// <summary>
		/// </summary>
		public ConditionValue[] Conditions { get; private set; }
		/// <summary>
		/// </summary>
		public Versions[] Versions { get; private set; }
		/// <summary>
		/// </summary>
		public int Generation { get; private set; }
		/// <summary>
		/// </summary>
		public int MinLevel { get; private set; }
		/// <summary>
		/// </summary>
		public int MaxLevel { get; private set; }
		/// <summary>
		/// </summary>
		public int Rarity { get; private set; }

		public Encounter(int id, int area, Method method, int slotId, Pokemons[] pokemon = null, ConditionValue[] conditions = null, int generation = 0, int minLevel = 1, int maxLevel = 1, int rarity = 0, Versions[] versions = null)
		{
			Id = id;
			Area = area;
			Pokemon = pokemon ?? new Pokemons[] { Pokemons.NONE };
			Conditions = conditions;// ?? throw new ArgumentNullException(nameof(conditions));
			Generation = generation;
			MinLevel = minLevel;
			MaxLevel = maxLevel;
			Method = method;
			Rarity = rarity;
			SlotId = slotId;
			Versions = versions;
		}

		public static bool EncounterConditionsMet(ConditionValue[] encounter)
		{
			Game g = new Game();
			//If conditions are SWARMING we do not want encounter to contain NO_SWARM
			if (encounter.Contains(g.Swarm?ConditionValue.SWARM_YES:ConditionValue.SWARM_NO))
				return true;
			if (encounter.Contains(g.Radar?ConditionValue.RADAR_ON:ConditionValue.RADAR_OFF))
				return true;
			if (//g.Radio == (byte)ConditionValue.RADIO_OFF && (
				//	encounter.Contains(ConditionValue.RADIO_HOENN) ||
				//	encounter.Contains(ConditionValue.RADIO_SINNOH) 
					encounter.Contains((ConditionValue)g.Radio) 
				//)
			)
				return true;
			if (encounter.Contains((ConditionValue)g.Slot))
				return true;
			if (g.Season == Season.Spring && (
					//encounter.Contains(ConditionValue.SEASON_SUMMER) ||
					//encounter.Contains(ConditionValue.SEASON_AUTUMN) ||
					//encounter.Contains(ConditionValue.SEASON_WINTER)
					encounter.Contains(ConditionValue.SEASON_SPRING)
				)
			)
				return true;
			if (g.Season == Season.Summer && (
					//encounter.Contains(ConditionValue.SEASON_SPRING) ||
					//encounter.Contains(ConditionValue.SEASON_AUTUMN) ||
					//encounter.Contains(ConditionValue.SEASON_WINTER)
					encounter.Contains(ConditionValue.SEASON_SUMMER)
				)
			)
				return true;
			if (g.Season == Season.Fall && (
					//encounter.Contains(ConditionValue.SEASON_SUMMER) ||
					//encounter.Contains(ConditionValue.SEASON_SPRING) ||
					//encounter.Contains(ConditionValue.SEASON_WINTER)
					encounter.Contains(ConditionValue.SEASON_AUTUMN)
				)
			)
				return true;
			if (g.Season == Season.Winter && (
					//encounter.Contains(ConditionValue.SEASON_SUMMER) ||
					//encounter.Contains(ConditionValue.SEASON_AUTUMN) ||
					//encounter.Contains(ConditionValue.SEASON_SPRING)
					encounter.Contains(ConditionValue.SEASON_WINTER)
				)
			)
				return true;
			return false;
		}
		public override int GetHashCode()
		{
			//return Id.GetHashCode();
			return Area.GetHashCode() ^ Method.GetHashCode() ^ SlotId.GetHashCode();
		}
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