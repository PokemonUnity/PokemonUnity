using PokemonEssentials.Interface.Field;
using System.Collections;

namespace PokemonUnity
{
	#region PokeRadar
	public struct PokeRadarMetaData
	{
		//public Tile Grass; x,y,ring,rarity
		//public int[] Grass; //x,y,ring,rarity
		public PokeRadarGrassData[] Grass { get; private set; }
		public int ChainCount { get; private set; }
		public Pokemons Species { get; private set; }
		public int Level { get; private set; }
	}
	public struct PokeRadarGrassData
	{
		public int X { get; private set; }
		public int Y { get; private set; }
		/// <summary>
		/// (0-3 inner to outer)
		/// </summary>
		public int Ring { get; private set; }
		public int Rarity { get; private set; }
		public PokeRadarGrassData(int mapx, int mapy, int ring, int rarity)
		{
			X = mapx;
			Y = mapy;
			Ring = ring;
			Rarity = rarity;
		}
	}
	#endregion

	#region PokeRadarEncounter DB
	/// <summary>
	/// A set of arrays each containing details of a wild encounter that can only
	///    occur via using the Poké Radar. The information within is as follows:<para></para>
	///    - Map ID on which this encounter can occur.<para></para>
	///    - Species.<para></para>
	///    - Probability that this encounter will occur (as a percentage).<para></para>
	///    - Minimum possible level.<para></para>
	///    - Maximum possible level (optional).
	/// </summary>
	public struct PokeRadars : IEncounterPokemon
	{
		public static PokeRadars[] POKERADAREXCLUSIVES=new PokeRadars[] {
			new PokeRadars(5,	Pokemons.STARLY,		20,	12, 15),
			new PokeRadars(21,	Pokemons.STANTLER,		10,	14),
			new PokeRadars(28,	Pokemons.BUTTERFREE,	20,	15, 18),
			new PokeRadars(28,	Pokemons.BEEDRILL,		20,	15, 18)
		};
		public int MapId { get; private set; }
		public int EncounterChance { get; private set; }
		public Pokemons Species { get; private set; }	//Rename?
		Pokemons IEncounterPokemon.Pokemon { get { return Species; } }
		/// <summary>
		/// LevelMin is 0 in Array.
		/// LevelMax is 1 in Array.
		/// LevelMax is Optional.
		/// </summary>
		//public int[] LevelMinMax { get; private set; }
		/// <summary>
		/// </summary>
		public int MinLevel { get; private set; }
		/// <summary>
		/// </summary>
		public int MaxLevel { get; private set; }

		/// <summary>
		/// A set of arrays each containing details of a wild encounter that can only
		/// occur via using the Poké Radar.
		/// </summary>
		/// <param name="map">Map ID on which this encounter can occur.</param>
		/// <param name="pkmn">Species.</param>
		/// <param name="chance">Probability that this encounter will occur (as a percentage).</param>
		/// <param name="min">Minimum possible level.</param>
		/// <param name="max">Maximum possible level (optional).</param>
		public PokeRadars(int map, Pokemons pkmn, int chance, byte min, byte? max=null)
		{
			MapId = map;
			Species = pkmn;
			EncounterChance = chance;
			//LevelMinMax = new int[] { min, max??min };
			MinLevel = min;
			MaxLevel = max??min;
		}
		/// <summary>
		/// A set of arrays each containing details of a wild encounter that can only
		/// occur via using the Poké Radar.
		/// </summary>
		/// <param name="map">Map ID on which this encounter can occur.</param>
		/// <param name="pkmn">Species.</param>
		/// <param name="chance">Probability that this encounter will occur (as a percentage).</param>
		/// <param name="min">Minimum possible level. (Also used as max possible level)</param>
		//public PokeRadars(int map, Pokemons pkmn, int chance, byte min) //: this(map, pkmn, chance, min, min)
		//{
		//	MapId = map;
		//	Species = pkmn;
		//	EncounterChance = chance;
		//	//LevelMinMax = new int[] { min, min };
		//	MinLevel = min;
		//	MaxLevel = max;
		//}
	}
	#endregion
}