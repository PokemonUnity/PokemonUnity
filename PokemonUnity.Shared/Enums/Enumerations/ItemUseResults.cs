namespace PokemonUnity.Shared.Enums
{
	/// <summary>
	/// Item Category determines both the item's effect
	/// and the bag-pocket that the item belongs to.
	/// </summary>
	/// <remarks>
	/// Can determine an <see cref="itemEffect"/> by the category it belongs to.
	/// </remarks>
	public class ItemUseResults : Enumeration
	{
		protected ItemUseResults(int id, string name) : base(id, name) {  }
		/// <summary>
		/// not used
		/// </summary>
		public static readonly ItemUseResults NotUsed					= new ItemUseResults(0,	"NotUsed");
		/// <summary>
		/// used, item not consumed
		/// </summary>
		public static readonly ItemUseResults UsedNotConsumed			= new ItemUseResults(1,	"UsedNotConsumed");
		/// <summary>
		/// close the Bag to use, item not consumed
		/// </summary>
		public static readonly ItemUseResults CloseBagNotConsumed		= new ItemUseResults(2,	"CloseBagNotConsumed");
		/// <summary>
		/// used, item consumed
		/// </summary>
		public static readonly ItemUseResults UsedItemConsumed			= new ItemUseResults(3,	"UsedItemConsumed");
		/// <summary>
		/// close the Bag to use, item consumed
		/// </summary>
		public static readonly ItemUseResults CloseBagItemConsumed		= new ItemUseResults(4,	"CloseBagItemConsumed");
	}
}