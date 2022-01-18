namespace PokemonUnity.Shared.Enums
{
	/// <summary>
	/// Item Category determines both the item's effect
	/// and the bag-pocket that the item belongs to.
	/// </summary>
	/// <remarks>
	/// Can determine an <see cref="itemEffect"/> by the category it belongs to.
	/// </remarks>
	public class BerryFlavours : Enumeration
	{
		protected BerryFlavours(int id, string name) : base(id, name) {  }
		public static readonly BerryFlavours Spicy			= new BerryFlavours(1,	"Spicy");
		public static readonly BerryFlavours Dry			= new BerryFlavours(2,	"Dry");
		public static readonly BerryFlavours Sweet			= new BerryFlavours(3,	"Sweet");
		public static readonly BerryFlavours Bitter			= new BerryFlavours(4,	"Bitter");
		public static readonly BerryFlavours Sour			= new BerryFlavours(5,	"Sour");
	}
}