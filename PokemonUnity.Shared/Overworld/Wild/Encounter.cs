using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity.Monster;

namespace PokemonUnity.Overworld
{
	public class Encounter
	{
		#region Variables
		protected List<Pokemon> PokemonPool { get; private set; }
		public EncounterData[] Encounters { get; protected set; }
		//public ConditionValue[] Conditions { get; private set; }
		/// <summary>
		/// Key: SlotId | Value: Pokemon Array
		/// </summary>
		public Dictionary<int, List<Pokemons>> Slot { get; protected set; }

		/// <summary>
		/// This array of numbers total up to 100%, 
		/// and represents a pool of pokemon that has chance to appear
		/// </summary>
		public virtual int[] Chances { get; }
		/// <summary>
		/// How often or likely to engage and encounter a pokemon
		/// </summary>
		//public static int Rate { get; set; }
		#endregion

		/// <summary>
		/// </summary>
		public Encounter()
		{
			PokemonPool = new List<Pokemon>();
			Encounters = new EncounterData[0];
			//Conditions = new ConditionValue[0];
			Slot = new Dictionary<int, List<Pokemons>>();
		}

		public Encounter(int[] slots, EncounterData[] data) : this()
		{
			Chances = slots;
			int i = 0;
			foreach (int slot in Chances)
			{
				Slot.Add(i, new List<Pokemons>()); i++;
			}
			Encounters = data;
		}

		/// <summary>
		/// </summary>
		/// <returns>returns pokemon for battle scene</returns>
		/// ToDo: Consider increasing chance on rarity as encounter increases 
		public Pokemon GetWildPokemon()//Method method
		{
			//int x = Game.MethodData[method].Length;
			//if(x > 0)
			//{
				//Reset list of pokemons each time encounter is triggered
				//List<Pokemon> PokemonPool = new List<Pokemon>();
				PokemonPool = new List<Pokemon>();
				//KeyValuePair<int, List<Pokemons>> slot = new KeyValuePair<int, List<Pokemons>>();
				//Dictionary<int, List<Pokemons>> slot = new Dictionary<int, List<Pokemons>>();
				Dictionary<int, int> min = new Dictionary<int, int>();
				Dictionary<int, int> max = new Dictionary<int, int>();
				Dictionary<int, int> rarity = new Dictionary<int, int>();
				
				//Sort through pokemons and group them by chance of encountering
				//foreach (int n in Game.MethodData[method])
				foreach (EncounterData item in Encounters)
				{
					//EncounterData item = Game.EncounterData[n];
					//for (int i = 0; i < x; i++)
					//{
					//	if(item.SlotId == i) // && Conditions.Contains()
					//		slot = new KeyValuePair<int, List<Pokemons>>(i, new List<Pokemons>());
					//		slot.Value.Add();
						//if(!slot.ContainsKey(item.SlotId)){ // && Conditions.Contains() {
						//	slot.Add(item.SlotId, new List<Pokemons>());
						//	min.Add(item.SlotId, item.MinLevel);
						//	max.Add(item.SlotId, item.MaxLevel);
						//	max.Add(item.SlotId, item.Rarity);
						//}
						//else
						if(Slot.ContainsKey(item.SlotId)) //item.Method == method &&
						{
							if (item.Conditions == null || item.Conditions.Length == 0 || EncounterConditionsMet(item.Conditions))
							{ 
								if(item.Pokemon.Length > 1)
								{
									Slot[item.SlotId].Add(item.Pokemon[Core.Rand.Next(item.Pokemon.Length)]);
								}
								else Slot[item.SlotId].Add(item.Pokemon[0]);
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

				//Run through each group of pokemon by slots they're in
				for (int y = 0; y < Slot.Count; y++)
				{
					Pokemons poke = Pokemons.NONE;
					if(Slot[y].Count > 1)
					{
						poke = Slot[y][Core.Rand.Next(Slot.Count)];
					}
					else if (Slot[y].Count == 1)
					{
						poke = Slot[y][0];
					}
					for (int n = 0; n < rarity[y]; n++)
					{
						//p.Add(new Pokemon(poke, level: (byte)Core.Rand.Next(item.MinLevel, item.MaxLevel), isEgg: false));
						PokemonPool.Add(new Pokemon(poke, level: (byte)Core.Rand.Next(min[y], max[y]), isEgg: false));
					}
				}

				//If encounter pool is not empty
				if (PokemonPool.Count > 0)
				{
					for (int z = PokemonPool.Count; z < 100; z++)
					{
						PokemonPool.Add(new Pokemon(Pokemons.NONE));
					}
					Pokemon pkmn = PokemonPool[Core.Rand.Next(PokemonPool.Count)];
					pkmn.SwapItem(Monster.Data.PokemonWildItems.GetWildHoldItem(pkmn.Species));
					return pkmn;
				}
			//}
			return new Pokemon(Pokemons.NONE);
		}

		public static bool EncounterConditionsMet(ConditionValue[] encounter)
		{
			//Game g = new Game();
			//If conditions are SWARMING we do not want encounter to contain NO_SWARM
			if (encounter.Contains(!Game.Swarm?ConditionValue.SWARM_YES:ConditionValue.SWARM_NO))
				return false;
			if (encounter.Contains(!Game.Radar?ConditionValue.RADAR_ON:ConditionValue.RADAR_OFF))
				return false;
			if (//g.Radio == (byte)ConditionValue.RADIO_OFF && (
				(encounter.Contains(ConditionValue.RADIO_HOENN) ||
				encounter.Contains(ConditionValue.RADIO_SINNOH))
				//encounter.Contains((ConditionValue)Game.Radio) 
				&& Game.Radio == (byte)ConditionValue.RADIO_OFF 
			)
				return false;
			if (encounter.Contains((ConditionValue)Game.Slot))
				return false; //Not sure how slot encounter is supposed to work yet...
			if (Game.Season == Season.Spring && (( //if has a season
				encounter.Contains(ConditionValue.SEASON_SUMMER) ||
				encounter.Contains(ConditionValue.SEASON_AUTUMN) ||
				encounter.Contains(ConditionValue.SEASON_WINTER))
				//but does not have....
				&& !encounter.Contains(ConditionValue.SEASON_SPRING))
			)
				return false;
			if (Game.Season == Season.Summer && (( //if has a season
				encounter.Contains(ConditionValue.SEASON_SPRING) ||
				encounter.Contains(ConditionValue.SEASON_AUTUMN) ||
				encounter.Contains(ConditionValue.SEASON_WINTER))
				//but does not have....
				&& !encounter.Contains(ConditionValue.SEASON_SUMMER))
			)
				return false;
			if (Game.Season == Season.Fall && (( //if has a season
				encounter.Contains(ConditionValue.SEASON_SUMMER) ||
				encounter.Contains(ConditionValue.SEASON_SPRING) ||
				encounter.Contains(ConditionValue.SEASON_WINTER))
				//but does not have....
				&& !encounter.Contains(ConditionValue.SEASON_AUTUMN))
			)
				return false;
			if (Game.Season == Season.Winter && (( //if has a season
				encounter.Contains(ConditionValue.SEASON_SUMMER) ||
				encounter.Contains(ConditionValue.SEASON_AUTUMN) ||
				encounter.Contains(ConditionValue.SEASON_SPRING))
				//but does not have....
				&& !encounter.Contains(ConditionValue.SEASON_WINTER))
			)
				return false;
			//Otherwise, there's no special condition and will always appear
			return true;
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