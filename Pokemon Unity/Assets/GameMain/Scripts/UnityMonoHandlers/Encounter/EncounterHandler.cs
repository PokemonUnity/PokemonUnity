using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Overworld;

/// <summary>
/// </summary>
public class PokemonEncounter : UnityEngine.MonoBehaviour
{
	//public Method EncounterMethod { get; private set; }
	//public Dictionary<Method, Encounter> EncounterCollection { get; private set; }
	public Encounter PokemonEncounterCollection { get; private set; }

	#region Misc Fishing Rod Encounter Rates
	/// <summary>
	/// How often or likely to engage and encounter a pokemon
	/// </summary>
	/// Walking Default 25% 
	/// Surfing Default 25%
	//public int Rate { get; set; }
	public int Ground = 25;
	public int Water = 25;
	/// <summary>
	/// How often or likely to engage and encounter a pokemon.
	/// Default 25%
	/// </summary>
	public int OldRod = 25;
	/// <summary>
	/// How often or likely to engage and encounter a pokemon.
	/// Default 50%
	/// </summary>
	public int GoodRod = 50;
	/// <summary>
	/// How often or likely to engage and encounter a pokemon.
	/// Default 75%
	/// </summary>
	public int SuperRod = 75;
	#endregion
	//Sample; feel free to use your own numbers below
	#region Encounter Chances
	/// <summary>
	/// Tall grass or cave
	/// </summary>
	public int[] Walk = new int[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 5, 4, 1 };
	public int[] DarkSpots = new int[] { 20, 20, 10, 10, 10, 10, 5, 5, 4, 4, 1, 1 };
	/// <summary>
	/// Rock smash or headbutt
	/// </summary>
	//public int[] Hidden = new int[] { 50, 30, 15, 4, 1 };
	public int[] Headbutt = new int[] { 50, 30, 15, 4, 1 };
	public int[] RockSmash = new int[] { 90, 10 };
	public int[] Surf = new int[] { 60, 30, 5, 4, 1 };
	#endregion
	#region Misc Species Encounter Chances
	/// <summary>
	/// Two slots of 20% each
	/// </summary>
	public int[] SwarmSlot = new int[] { 20, 20 };
	/// <summary>
	/// Two slots of 10% each
	/// </summary>
	public int[] DaySlot = new int[] { 10, 10 };
	/// <summary>
	/// Two slots of 10% each
	/// </summary>
	public int[] NightSlot = new int[] { 10, 10 };
	/// <summary>
	/// Four slots of 10% each
	/// </summary>
	/// There are four rings in pokemon radar, maybe each ring has it's own pokemon encounter chance/rate?
	/// this is triggered manually using a pokegear, so it isnt in wild encounter method
	public int[] RadarSlot = new int[] { 10, 10, 10, 10 };
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
	public Pokemon TriggerWildEncounter(Method method)
	{
		int roll = 0;
		//Pull a number from 1 out of a 100
		roll = Core.Rand.Next(101); //0- 100
		#region Check if number is winner
		if (method == Method.SURF || method == Method.SURF_SPOTS)
			if (roll > Water)
				return new Pokemon(Pokemons.NONE);
		else if (method == Method.OLD_ROD)
			if (roll > OldRod)
				return new Pokemon(Pokemons.NONE);
		else if (method == Method.GOOD_ROD)
			if (roll > GoodRod)
				return new Pokemon(Pokemons.NONE);
		else if (method == Method.SUPER_ROD || method == Method.SUPER_ROD_SPOTS)
			if (roll > SuperRod)
				return new Pokemon(Pokemons.NONE);
		else
			if (roll > Ground)
				return new Pokemon(Pokemons.NONE);
		#endregion
		List<EncounterData> pool = new List<EncounterData>();
		EncounterData[] mapdata = new EncounterData[0]; //Pass Map Array Data here
		int[] slots = new int[0];
		#region Sepaprate and filter pokemons into smaller groups
		foreach (EncounterData data in mapdata)
			if (data.Method == method)
				pool.Add(data);
		switch (method)
		{
			case Method.YELLOW_FLOWERS:
			case Method.PURPLE_FLOWERS:
			case Method.RED_FLOWERS:
			case Method.ROUGH_TERRAIN:
			case Method.WALK:
				slots = Walk;
				break;
			case Method.HEADBUTT:
				slots = Headbutt;
				break;
			case Method.ROCK_SMASH:
				slots = RockSmash;
				break;
			case Method.DARK_GRASS:
			case Method.GRASS_SPOTS:
			case Method.CAVE_SPOTS:
			case Method.BRIDGE_SPOTS:
				slots = DarkSpots;
				break;
			case Method.OLD_ROD:
			case Method.GOOD_ROD:
			case Method.SUPER_ROD:
			case Method.SUPER_ROD_SPOTS:
			case Method.SURF:
			case Method.SURF_SPOTS:
				slots = Surf;
				break;
			default:
				break;
		}
		#endregion
		PokemonEncounterCollection = new Encounter(slots: slots, data: pool.ToArray());
		//return PokemonEncounterCollection.GetWildPokemon(method);
		return PokemonEncounterCollection.GetWildPokemon();
	}
	#endregion
}