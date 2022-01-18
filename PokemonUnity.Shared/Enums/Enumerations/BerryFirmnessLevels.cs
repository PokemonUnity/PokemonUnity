namespace PokemonUnity.Shared.Enums
{
	/// <summary>
	/// Item Category determines both the item's effect
	/// and the bag-pocket that the item belongs to.
	/// </summary>
	/// <remarks>
	/// Can determine an <see cref="itemEffect"/> by the category it belongs to.
	/// </remarks>
	public class BerryFirmnessLevels : Enumeration
	{
		protected BerryFirmnessLevels(int id, string name) : base(id, name) {  }
		public static readonly BerryFirmnessLevels VerySoft			= new BerryFirmnessLevels(1,	"VerySoft");
		public static readonly BerryFirmnessLevels Soft	 			= new BerryFirmnessLevels(2,	"Soft");
		public static readonly BerryFirmnessLevels Hard	 			= new BerryFirmnessLevels(3,	"Hard");
		public static readonly BerryFirmnessLevels VeryHard			= new BerryFirmnessLevels(4,	"VeryHard");
		public static readonly BerryFirmnessLevels SuperHard		= new BerryFirmnessLevels(5,	"SuperHard");
	}
}