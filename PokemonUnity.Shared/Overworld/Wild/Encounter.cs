using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity.Monster;

namespace PokemonUnity.Overworld
{
	[Serializable]
	public class Encounter
	{
		#region Variables
		//protected virtual List<Pokemon> PokemonPool { get; private set; }
		public virtual EncounterData[] Encounters { get; protected set; }
		//public ConditionValue[] Conditions { get; private set; }
		/// <summary>
		/// Key: SlotId | Value: Pokemon Array
		/// </summary>
		public virtual Dictionary<int, List<Pokemons>> Slot { get; protected set; }

		/// <summary>
		/// This array of numbers total up to 100%, 
		/// and represents a pool of pokemon that has chance to appear
		/// </summary>
		public virtual int[] Chances { get; protected set; }
		/// <summary>
		/// How often or likely to engage and encounter a pokemon
		/// </summary>
		public virtual int Rate { get; set; }
		//public virtual EncounterTypes Method { get; set; }
		public virtual Method Method { get; set; }
		#endregion

		/// <summary>
		/// </summary>
		public Encounter()
		{
			//PokemonPool = new List<Pokemon>();
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
		/// 
		/// </summary>
		/// <param name="method"></param>
		/// <param name="density">
		/// Dense pack of pokemons will mean more encounters.
		/// Fewer steps, more encounters; or more steps, fewer enounters.
		/// </param>
		/// <param name="slots"></param>
		public Encounter(Method method, int density, params EncounterSlotData[] slots) : this()
		{
			Method = method;
			Rate = density;
			List<int> chance = new List<int>();
			List<EncounterSlotData> encounters = new List<EncounterSlotData>();
			for (int i = 100, n = 0; i >= 0; i--)
			{
				for (; n < slots.Length; n++)
				{
					if(slots[n].Frequency == 0)
						continue;
					if(i - slots[n].Frequency >= 0)
					{ 
						encounters.Add(slots[n]);
						chance.Add(slots[n].Frequency);
						i = i - slots[n].Frequency;
						if(i == 0){
							i = -1;
							break;
						}
					}
					else
					{
						//slots[n].Frequency = i;
						EncounterSlotData data = new EncounterSlotData(pokemon:slots[n].Pokemon, minLevel:slots[n].MinLevel, maxLevel:slots[n].MaxLevel, frequency:slots[n].Frequency);
						encounters.Add(data);
						chance.Add(i);
						i = -1;
						break;
					}
				}
				if (i == -1)
					break;
				else
					//encounters.Add(null);
					encounters.Add(new EncounterSlotData());
			}
			//Encounters = encounters.ToArray();
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