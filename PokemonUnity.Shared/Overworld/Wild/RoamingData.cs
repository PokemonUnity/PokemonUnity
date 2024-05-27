using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonUnity.Overworld
{
	[Serializable]
	public struct RoamingEncounterData
	{
		#region Variables
		/// <summary>
		/// </summary>
		//public int Id { get; private set; }
		/// <summary>
		/// Roaming areas specifically for this <see cref="Pokemon"/>.
		/// </summary>
		public IDictionary<int, int[]> Areas { get; private set; }
		/// <summary>
		/// Encounter type (0=any, 1=grass/walking in cave, 2=surfing, 3=fishing, 4=surfing/fishing).
		/// <see cref="PokemonEssentials.Interface.Field.IGlobalMetadataRoaming"/>
		/// </summary>
		/// <remarks>
		/// <seealso cref="Method"/>(s) used to encounter this <see cref="Pokemon"/>.
		/// </remarks>
		public EncounterTypes EncounterType { get; private set; }
		/// <summary>
		/// </summary>
		public Pokemons Pokemon { get; private set; }
		/// <summary>
		/// Name of <seealso cref="PokemonEssentials.Interface.IAudioBGM"/> to play for that encounter
		/// </summary>
		public string BGM { get; private set; }
		/// <summary>
		/// Encounter Level Pokemon spawns at.
		/// </summary>
		public int Level { get; private set; }
		/// <summary>
		/// Global Switch Id; the Pokémon roams while this is ON.
		/// <see cref="PokemonEssentials.Interface.IGameSwitches"/>
		/// </summary>
		public int SwitchId { get; private set; }
		#endregion

		/// <summary>
		/// A set of arrays each containing the details of a roaming Pokémon.
		/// </summary>
		/// <param name="pokemon"></param>
		/// <param name="level"></param>
		/// <param name="switchId">Global Switch; the Pokémon roams while this is ON.</param>
		/// <param name="encounterType">Encounter type (0=any, 1=grass/walking in cave, 2=surfing, 3=fishing, 4=surfing/fishing). <see cref="PokemonEssentials.Interface.Field.IGlobalMetadataRoaming"/></param>
		/// <param name="bgm">Name of BGM to play for that encounter (optional).</param>
		/// <param name="areas">Roaming areas specifically for this Pokémon (optional).</param>
		public RoamingEncounterData(Pokemons pokemon, int level, int switchId, EncounterTypes encounterType, string bgm = null, IDictionary<int,int[]> areas = null)
		{
			//Id = id;
			Areas = areas;
			Pokemon = pokemon;
			Level = level;
			EncounterType = encounterType;
			SwitchId = switchId;
			BGM = bgm;
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