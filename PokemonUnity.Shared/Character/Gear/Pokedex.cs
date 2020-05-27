using System.Collections;

namespace PokemonUnity.Character
{
	/// <summary>
	/// The names of each Dex list in the game, in order and with National Dex at
	///    the end. This is also the order that PokemonGlobal.pokedexUnlocked is
	///    in, which records which Dexes have been unlocked (first is unlocked by
	///    default).<para></para>
	///    You can define which region a particular Dex list is linked to. This
	///    means the area map shown while viewing that Dex list will ALWAYS be that
	///    of the defined region, rather than whichever region the player is
	///    currently in. To define this, put the Dex name and the region number in
	///    an array, like the Kanto and Johto Dexes are. The National Dex isn't in
	///    an array with a region number, therefore its area map is whichever region
	///    the player is currently in.
	/// </summary>
	public struct Pokedex
	{
		//[_INTL("Kanto Pokédex"),0],
		//[_INTL("Johto Pokédex"),1],
		//_INTL("National Pokédex")

		//public string this[int id].Name { get { return null; } }

		/// <summary>
		/// Name of the Pokedex
		/// </summary>
		/// Translated from DB
		public string Name { get { return null; } }
		/// <summary>
		/// Map Region this Pokedex covers
		/// </summary>
		/// Generation
		/// Equal to or greater than 0 assigns to region id
		public PokemonUnity.Regions Region { get; private set; }
		//int PokedexId;
		/// <summary>
		/// An array of numbers, where each number is that of a Dex list (National Dex
		///    is -1). All Dex lists included here have the species numbers in them
		///    reduced by 1, thus making the first listed species have a species number
		///    of 0 (e.g. Victini in Unova's Dex).
		/// </summary>
		/// <remarks>
		/// The pokedex created using individual pokemon entries.
		/// The order of pokemons loaded, determines the Id of the pokemon, in the pokedex.
		/// </remarks>
		/// Easier to program what you want it to do, when it's just a list of pokemoon Ids
		public PokemonUnity.Pokemons[] PokemonIndex { get; private set; }
		//int DEXINDEXOFFSETS;//    = []
	}
}