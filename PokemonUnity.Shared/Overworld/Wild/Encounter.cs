using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity.Monster;
//using PokemonUnity.Overworld.EncounterData;

namespace PokemonUnity.Overworld
{
	public struct Encounter
	{
		#region Variables
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
		#endregion

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
			SlotId = slotId;//versions.Contains(x => (int)x == 15) ? slotId : slotId - 1; //if 15, first slot is 0
			Versions = versions;
		}

		/// <summary>
		/// </summary>
		/// <param name="method"></param>
		/// <returns></returns>
		/// ToDo: Consider increasing chance on rarity as encounter increases 
		public static Pokemon GetWildPokemon(Method method)
		{
			int x = Game.MethodData[method].Length;
			if(x > 0)
			{
				List<Pokemon> p = new List<Pokemon>();
				//KeyValuePair<int, List<Pokemons>> slot = new KeyValuePair<int, List<Pokemons>>(); 
				Dictionary<int, List<Pokemons>> slot = new Dictionary<int, List<Pokemons>>(); 
				Dictionary<int, int> min = new Dictionary<int, int>(); 
				Dictionary<int, int> max = new Dictionary<int, int>(); 
				Dictionary<int, int> rarity = new Dictionary<int, int>(); 
				foreach (int n in Game.MethodData[method])
				{
					Encounter item = Game.EncounterData[n];
					//for (int i = 0; i < x; i++)
					//{
					//	if(item.SlotId == i) // && Conditions.Contains()
					//		slot = new KeyValuePair<int, List<Pokemons>>(i, new List<Pokemons>());
					//		slot.Value.Add();
						if(!slot.ContainsKey(item.SlotId)){ // && Conditions.Contains() {
							slot.Add(item.SlotId, new List<Pokemons>());
							min.Add(item.SlotId, item.MinLevel);
							max.Add(item.SlotId, item.MaxLevel);
							max.Add(item.SlotId, item.Rarity);
						}
						else
						{
							if (item.Conditions == null || item.Conditions.Length == 0 || EncounterConditionsMet(item.Conditions))
							{ 
								if(item.Pokemon.Length > 1)
								{
									slot[item.SlotId].Add(item.Pokemon[Core.Rand.Next(item.Pokemon.Length)]);
								}
								else slot[item.SlotId].Add(item.Pokemon[0]);
							} 
							min[item.SlotId] = Math.Min(min[item.SlotId], item.MinLevel);
							max[item.SlotId] = Math.Max(max[item.SlotId], item.MaxLevel);
							//Pokemons poke = Pokemons.NONE;
							//if(item.Pokemon.Length > 1)
							//{
							//	poke = item.Pokemon[Core.Rand.Next(item.Pokemon.Length)];
							//}
							//else //if (item.Pokemon.Length == 1)
							//{
							//	poke = item.Pokemon[0];
							//}
							//for (int n = 0; n < item.Rarity; n++)
							//{
							//	//p.Add(new Pokemon(poke, level: (byte)Core.Rand.Next(item.MinLevel, item.MaxLevel),isEgg: false));
							//	p.Add(new Pokemon(poke, level: (byte)Core.Rand.Next(min[item.SlotId], max[item.SlotId]),isEgg: false));
							//}
						}
					//}
				}
				for (int y = 0; y < slot.Count; y++)
				{
					Pokemons poke = Pokemons.NONE;
					if(slot[y].Count > 1)
					{
						poke = slot[y][Core.Rand.Next(slot.Count)];
					}
					else if (slot[y].Count == 1)
					{
						poke = slot[y][0];
					}
					for (int n = 0; n < rarity[y]; n++)
					{
						//p.Add(new Pokemon(poke, level: (byte)Core.Rand.Next(item.MinLevel, item.MaxLevel),isEgg: false));
						p.Add(new Pokemon(poke, level: (byte)Core.Rand.Next(min[y], max[y]),isEgg: false));
					}
				}
				if (p.Count > 0)
				{
					for (int z = p.Count; z < 100; z++)
					{
						p.Add(new Pokemon(Pokemons.NONE));
					}
					Pokemon pkmn = p[Core.Rand.Next(p.Count)];
					pkmn.SwapItem(Monster.Data.PokemonWildItems.GetWildHoldItem(pkmn.Species));
					return pkmn;
				}
			}
			return new Pokemon(Pokemons.NONE);
		}
		//public static Pokemon WildPokemonRNG(Method method) //conditions
		//{
		//	//if(rand < area.rate)
		//	return GetWildPokemon(method);
		//}
		public static bool EncounterConditionsMet(ConditionValue[] encounter)
		{
			//Game g = new Game();
			//If conditions are SWARMING we do not want encounter to contain NO_SWARM
			if (encounter.Contains(Game.Swarm?ConditionValue.SWARM_YES:ConditionValue.SWARM_NO))
				return true;
			if (encounter.Contains(Game.Radar?ConditionValue.RADAR_ON:ConditionValue.RADAR_OFF))
				return true;
			if (//g.Radio == (byte)ConditionValue.RADIO_OFF && (
				//	encounter.Contains(ConditionValue.RADIO_HOENN) ||
				//	encounter.Contains(ConditionValue.RADIO_SINNOH) 
					encounter.Contains((ConditionValue)Game.Radio) 
				//)
			)
				return true;
			if (encounter.Contains((ConditionValue)Game.Slot))
				return true;
			if (Game.Season == Season.Spring && (
					//encounter.Contains(ConditionValue.SEASON_SUMMER) ||
					//encounter.Contains(ConditionValue.SEASON_AUTUMN) ||
					//encounter.Contains(ConditionValue.SEASON_WINTER)
					encounter.Contains(ConditionValue.SEASON_SPRING)
				)
			)
				return true;
			if (Game.Season == Season.Summer && (
					//encounter.Contains(ConditionValue.SEASON_SPRING) ||
					//encounter.Contains(ConditionValue.SEASON_AUTUMN) ||
					//encounter.Contains(ConditionValue.SEASON_WINTER)
					encounter.Contains(ConditionValue.SEASON_SUMMER)
				)
			)
				return true;
			if (Game.Season == Season.Fall && (
					//encounter.Contains(ConditionValue.SEASON_SUMMER) ||
					//encounter.Contains(ConditionValue.SEASON_SPRING) ||
					//encounter.Contains(ConditionValue.SEASON_WINTER)
					encounter.Contains(ConditionValue.SEASON_AUTUMN)
				)
			)
				return true;
			if (Game.Season == Season.Winter && (
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
	/*public struct EncounterSlotData
	{
		/// <summary>
		/// </summary>
		public Method Method { get; private set; }
		/// <summary>
		/// </summary>
		public int[] Slots { get; private set; }
		/// <summary>
		/// </summary>
		public int[] Generation { get; private set; }
		/// <summary>
		/// </summary>
		public int[] Rate { get; private set; }
		public int[] IdGroup { get; private set; }

		//foreach (EncounterSlotData encounter in Game.EncounterData[Method])
		public int this[int version, int slot] //if 15, first slot is 0; fixed in sql
		//{ get { return encounter.Generation.Contains(15) ? Rate[i] : Rate[i-1]; } }
		{ get { return Rate[slot-1]; } }
	}*/
}