using System.Collections;
using System.Collections.Generic;

namespace PokemonUnity
{
	#region Roaming Pokemons
	/// <summary>
	/// A list of maps used by roaming Pokémon. Each map has an array of other maps
	///    it can lead to.
	/// </summary>
	public class RoamingAreas
	{
		public int MapId;
		public int[] ConnectedMaps;
		private Dictionary<int, int[]> _areas;
		//ToDo: Make Arraychart below global, but still have instantiable variables
		private static Dictionary<int, int[]> Areas;
		//public static int[] this[int map] { get { return Areas[map]; } }
		//RoamingAreas = {
		//	5  => [21,28,31,39,41,44,47,66,69],
		//	21 => [5,28,31,39,41,44,47,66,69],
		//	28 => [5,21,31,39,41,44,47,66,69],
		//	31 => [5,21,28,39,41,44,47,66,69],
		//	39 => [5,21,28,31,41,44,47,66,69],
		//	41 => [5,21,28,31,39,44,47,66,69],
		//	44 => [5,21,28,31,39,41,47,66,69],
		//	47 => [5,21,28,31,39,41,44,66,69],
		//	66 => [5,21,28,31,39,41,44,47,69],
		//	69 => [5,21,28,31,39,41,44,47,66]
		//}
	}
	/// <summary>
	/// A set of arrays each containing the details of a roaming Pokémon. The
	///    information within is as follows:<para></para>
	///    - Species.<para></para>
	///    - Level.<para></para>
	///    - Global Switch; the Pokémon roams while this is ON.<para></para>
	///    - Encounter type (0=any, 1=grass/walking in cave, 2=surfing, 3=fishing,<para></para>
	///         4=surfing/fishing). See bottom of PField_RoamingPokemon for lists.<para></para>
	///    - Name of BGM to play for that encounter (optional).<para></para>
	///    - Roaming areas specifically for this Pokémon (optional).
	/// </summary>
	public class RoamingSpecies
	{
		public Pokemons Species;
		public int Level;
		public int EncounterType;
		public int BGM;
		public RoamingAreas MapId;
		//RoamingSpecies = [
		//	[:LATIAS, 30, 53, 0, "Battle roaming"],
		//	[:LATIOS, 30, 53, 0, "Battle roaming"],
		//	[:KYOGRE, 40, 54, 2, nil, {
		//		2  => [21,31],
		//		21 => [2,31,69],
		//		31 => [2,21,69],
		//		69 => [21,31]
		//		}],
		//	[:ENTEI, 40, 55, 1, nil]
		//]
		/// <summary>
		/// A set of arrays each containing the details of a roaming Pokémon.
		/// </summary>
		/// <param name="pkmn">Species.</param>
		/// <param name="level">Level.</param>
		/// <param name="encounter">Encounter type (0=any, 1=grass/walking in cave, 2=surfing, 
		/// 3=fishing, 4=surfing/fishing).</param>
		/// <param name="music">Name of BGM to play for that encounter (optional).</param>
		/// <param name="map">Roaming areas specifically for this Pokémon (optional).</param>
		public RoamingSpecies(Pokemons pkmn, int level, int encounter, int? music = null, RoamingAreas map = null)
		{
			Species = pkmn;
			Level = level;
			EncounterType = encounter;
			if (music.HasValue) BGM = music.Value;
			MapId = map;
		}
	}
	#endregion
}