using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity.Monster;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonUnity.Overworld
{
	/// <summary>
	/// Sample snippet to be copy and pasted in unity project to attach
	/// </summary>
	/// inherit monobehaviour and create a manager class here...
	public partial class PokemonEncounter
	{
		//const int LandTerrainSlots = 12;
		//const int WaterTerrainSlots = 5;

		//public Method EncounterMethod { get; private set; }
		//public Dictionary<Method, Encounter> EncounterCollection { get; private set; }
		public Encounter PokemonEncounterCollection { get; private set; }

		#region Misc Fishing Rod Encounter Rates
		/// <summary>
		/// How often or likely to engage and encounter a pokemon
		/// </summary>
		public int Rate { get; set; }
		/// <summary>
		/// How often or likely to engage and encounter a pokemon.
		/// Default 25%
		/// </summary>
		public int OldRod { get; set; }
		/// <summary>
		/// How often or likely to engage and encounter a pokemon.
		/// Default 50%
		/// </summary>
		public int GoodRod { get; set; }
		/// <summary>
		/// How often or likely to engage and encounter a pokemon.
		/// Default 75%
		/// </summary>
		public int SuperRod { get; set; }
		#endregion
		//Sample; feel free to use your own numbers below
		#region Encounter Chances
		/// <summary>
		/// Tall grass or cave
		/// </summary>
		public int[] Walk
		{
			get
			{
				return
					//this number totals up to 100%, so each index is chance to encounter
					new int[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 5, 4, 1 };
			}
		}
		public int[] DarkSpots
		{
			get
			{
				return
					//this number totals up to 100%, so each index is chance to encounter
					new int[] { 20, 20, 10, 10, 10, 10, 5, 5, 4, 4, 1, 1 };
			}
		}
		/// <summary>
		/// Rock smash or headbutt
		/// </summary>
		public int[] Hidden
		{
			get
			{
				return
					//this number totals up to 100%, so each index is chance to encounter
					new int[] { 50, 30, 15, 4, 1 };
			}
		}
		public int[] Surf
		{
			get
			{
				return
					//this number totals up to 100%, so each index is chance to encounter
					new int[] { 60, 30, 5, 4, 1 };
			}
		}
		#endregion
		#region Misc Species Encounter Chances
		/// <summary>
		/// Two slots of 20% each
		/// </summary>
		public int[] SwarmSlot
		{
			get
			{
				return
					new int[] { 20, 20 };
			}
		}
		/// <summary>
		/// Two slots of 10% each
		/// </summary>
		public int[] DaySlot
		{
			get
			{
				return
					new int[] { 10, 10 };
			}
		}
		/// <summary>
		/// Two slots of 10% each
		/// </summary>
		public int[] NightSlot
		{
			get
			{
				return
					new int[] { 10, 10 };
			}
		}
		/// <summary>
		/// Four slots of 10% each
		/// </summary>
		public int[] RadarSlot
		{
			get
			{
				return
					new int[] { 10, 10, 10, 10 };
			}
		}
		#endregion

		#region Methods
		//public void LoadMapEncounterData(EncounterData[] data)
		//{
		//	//EncounterCollection = new Dictionary<Method, Encounter>();
		//	//EncounterCollection.Add(Method.WALK, new Encounter(TallGrass, grass));
		//	//EncounterCollection.Add(Surf.met, new Encounter(Surf, surf));
		//	//EncounterCollection.Add(Method.HEADBUTT, new Encounter(Headbutt, new EncounterData[0]));
		//	//PokemonEncounterCollection = new Encounter(data: data);
		//	foreach (EncounterData encounter in data)
		//		Game.EncounterMethodData[encounter.Method] = encounter;
		//}
		//public IPokemon TriggerWildEncounter(Method method)
		//{
		//	int roll = 0;
		//	//Pull a number from 1 out of a 100
		//	roll = Core.Rand.Next(101); //0- 100
		//	#region Check if number is winner
		//	if (method == Method.OLD_ROD)
		//	{
		//		if (roll > OldRod)
		//			return new Pokemon(Pokemons.NONE);
		//	}
		//	else if (method == Method.GOOD_ROD)
		//	{
		//		if (roll > GoodRod)
		//			return new Pokemon(Pokemons.NONE);
		//	}
		//	else if (method == Method.SUPER_ROD || method == Method.SUPER_ROD_SPOTS)
		//	{
		//		if (roll > SuperRod)
		//			return new Pokemon(Pokemons.NONE);
		//	}
		//	else
		//	{
		//		if (roll > Rate)
		//			return new Pokemon(Pokemons.NONE);
		//	}
		//	#endregion
		//	IList<IMapEncounterMetadata> pool = new List<IMapEncounterMetadata>();
		//	IMapEncounterMetadata[] mapdata = new IMapEncounterMetadata[0]; //Pass Map Array Data here
		//	int[] slots = new int[0];
		//	#region Separate and filter pokemons into smaller groups
		//	foreach (EncounterData data in mapdata)
		//		if (data.Method == method)
		//			pool.Add(data);
		//	switch (method)
		//	{
		//		case Method.YELLOW_FLOWERS:
		//		case Method.PURPLE_FLOWERS:
		//		case Method.RED_FLOWERS:
		//		case Method.ROUGH_TERRAIN:
		//		case Method.WALK:
		//			slots = Walk;
		//			break;
		//		case Method.HEADBUTT:
		//		case Method.ROCK_SMASH:
		//			slots = Hidden;
		//			break;
		//		case Method.DARK_GRASS:
		//		case Method.GRASS_SPOTS:
		//		case Method.CAVE_SPOTS:
		//		case Method.BRIDGE_SPOTS:
		//			slots = DarkSpots;
		//			break;
		//		case Method.OLD_ROD:
		//		case Method.GOOD_ROD:
		//		case Method.SUPER_ROD:
		//		case Method.SUPER_ROD_SPOTS:
		//		case Method.SURF:
		//		case Method.SURF_SPOTS:
		//			slots = Surf;
		//			break;
		//		default:
		//			break;
		//	}
		//	#endregion
		//	PokemonEncounterCollection = new Encounter(slots: slots, data: pool.ToArray());
		//	//return PokemonEncounterCollection.GetWildPokemon(method);
		//	return PokemonEncounterCollection.GetWildPokemon();
		//}
		#endregion
	}

	/*public class LandWalkingGrassEncounter : Encounter
	{
		public override int[] Chances
		{
			get
			{
				return
					//this number totals up to 100%, so each index is chance to encounter
					new int[] { 20, 20, 10, 10, 10, 10, 5, 5, 4, 4, 1, 1 };
			}
		}

		public LandWalkingGrassEncounter() : base()
		{
			int i = 0;
			foreach (int slot in Chances)
			{
				Slot.Add(i, new List<Pokemons>()); i++;
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="slots"></param>
		public LandWalkingGrassEncounter(EncounterData[] slots)
		{
			Encounters = slots;
		}
	}

	public class LandWalkingHeadbuttEncounter : Encounter
	{
		public override int[] Chances
		{
			get
			{
				return
					//this number totals up to 100%, so each index is chance to encounter
					new int[] { 50, 30, 15, 4, 1 };
			}
		}

		public LandWalkingHeadbuttEncounter() : base()
		{
			int i = 0;
			foreach (int slot in Chances)
			{
				Slot.Add(i, new List<Pokemons>()); i++;
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="slots"></param>
		public LandWalkingHeadbuttEncounter(EncounterData[] slots)
		{
			Encounters = slots;
		}
	}

	public class WaterSurfingEncounter : Encounter
	{
		public override int[] Chances
		{
			get
			{
				return
					//this number totals up to 100%, so each index is chance to encounter
					new int[] { 60, 30, 5, 4, 1 };
			}
		}

		public WaterSurfingEncounter() : base()
		{
			int i = 0;
			foreach (int slot in Chances)
			{
				Slot.Add(i, new List<Pokemons>()); i++;
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="slots"></param>
		public WaterSurfingEncounter(EncounterData[] slots)
		{
			Encounters = slots;
		}
	}*/
}