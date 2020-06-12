using System.Collections;

namespace PokemonUnity
{
	#region PokeRadar
	public struct PokeRadarMetaData
	{
		//public Tile Grass; x,y,ring,rarity
		//public int[] Grass; //x,y,ring,rarity
		public PokeRadarGrassData[] Grass;
		public int ChainCount;
		public Pokemons Species;
		public int Level;
	}
	public struct PokeRadarGrassData
	{
		public int X; 
		public int Y;
		/// <summary>
		/// (0-3 inner to outer)
		/// </summary>
		public int Ring;
		public int Rarity;
		public PokeRadars(int mapx, int mapy, int ring, int rarity)
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
	public class PokeRadars
	{
		public int MapId;
		public int EncounterChance;
		public Pokemons Species;
		/// <summary>
		/// LevelMin is 0 in Array.
		/// LevelMax is 1 in Array.
		/// LevelMax is Optional.
		/// </summary>
		public int[] LevelMinMax;
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
		public PokeRadars(int map, Pokemons pkmn, int chance, byte min) : this(map, pkmn, chance, min, min)
		{

		}
	}
	#endregion
}