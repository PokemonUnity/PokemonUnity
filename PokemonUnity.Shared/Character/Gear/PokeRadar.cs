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
	///    - Probability that this encounter will occur (as a percentage).<para></para>
	///    - Species.<para></para>
	///    - Minimum possible level.<para></para>
	///    - Maximum possible level (optional).
	/// </summary>
	public struct PokeRadars
	{
		public int MapId { get; private set; }
		public int EncounterChance { get; private set; }
		public Pokemons Species { get; private set; }
		/// <summary>
		/// LevelMin is 0 in Array.
		/// LevelMax is 1 in Array.
		/// LevelMax is Optional.
		/// </summary>
		public int[] LevelMinMax { get; private set; }
		/*POKERADAREXCLUSIVES=[
			[5,  20, :STARLY,     12, 15],
			[21, 10, :STANTLER,   14],
			[28, 20, :BUTTERFREE, 15, 18],
			[28, 20, :BEEDRILL,   15, 18]
		]*/
		/// <summary>
		/// A set of arrays each containing details of a wild encounter that can only
		/// occur via using the Poké Radar.
		/// </summary>
		/// <param name="map">Map ID on which this encounter can occur.</param>
		/// <param name="pkmn">Species.</param>
		/// <param name="chance">Probability that this encounter will occur (as a percentage).</param>
		/// <param name="min">Minimum possible level.</param>
		/// <param name="max">Maximum possible level (optional).</param>
		public PokeRadars(int map, Pokemons pkmn, int chance, byte min, byte max)
		{
			MapId = map;
			Species = pkmn;
			EncounterChance = chance;
			LevelMinMax = new int[] { min, max };
		}
		/// <summary>
		/// A set of arrays each containing details of a wild encounter that can only
		/// occur via using the Poké Radar.
		/// </summary>
		/// <param name="map">Map ID on which this encounter can occur.</param>
		/// <param name="pkmn">Species.</param>
		/// <param name="chance">Probability that this encounter will occur (as a percentage).</param>
		/// <param name="min">Minimum possible level. (Also used as max possible level)</param>
		public PokeRadars(int map, Pokemons pkmn, int chance, byte min) //: this(map, pkmn, chance, min, min)
		{
			MapId = map;
			Species = pkmn;
			EncounterChance = chance;
			LevelMinMax = new int[] { min, min };
		}
	}
	#endregion
}