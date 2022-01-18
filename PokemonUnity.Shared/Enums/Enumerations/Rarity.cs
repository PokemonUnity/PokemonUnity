namespace PokemonUnity.Shared.Enums
{
	public class Rarity : Enumeration
	{
		protected Rarity(int id, string name) : base(id, name) { }
		public static readonly Rarity Common						= new Rarity(0,	"Common");
		/// <summary>
		/// Legendary Pokémon are a group of incredibly rare and often very powerful Pokémon, generally featured prominently in the legends and myths of the Pokémon world.
		/// </summary>
		public static readonly Rarity Legendary						= new Rarity(0,	"Legendary");
		/// <summary>
		/// Mythical Pokémon are a group of Pokémon, which are usually event-exclusive (similar to legendary, but separate) 
		/// </summary>
		public static readonly Rarity Mythical						= new Rarity(0,	"Mythical");
	}
}