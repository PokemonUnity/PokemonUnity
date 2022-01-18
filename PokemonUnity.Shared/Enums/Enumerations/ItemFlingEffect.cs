namespace PokemonUnity.Shared.Enums
{
	/// <summary>
	/// Item Category determines both the item's effect
	/// and the bag-pocket that the item belongs to.
	/// </summary>
	/// <remarks>
	/// Can determine an <see cref="itemEffect"/> by the category it belongs to.
	/// </remarks>
	/// <summary>
	/// Effects that occur when items are thrown at target.
	/// Not all items can be thrown(?)...
	/// </summary>
	/// <remarks>Didnt have access to variable names...</remarks>
	public class ItemFlingEffect : Enumeration
	{
		protected ItemFlingEffect(int id, string name) : base(id, name) { }
		public static readonly ItemFlingEffect NONE					= new ItemFlingEffect(1,	"NNONE");
		/// <summary>
		/// Badly poisons the target.
		/// </summary>
		public static readonly ItemFlingEffect BADLY_POISONS		= new ItemFlingEffect(1,	"BADLY_POISONS");
		/// <summary>
		/// Burns the target.
		/// </summary>
		public static readonly ItemFlingEffect BURNS				= new ItemFlingEffect(2,	"BURNS");
		/// <summary>
		/// Immediately activates the berry's effect on the target.
		/// </summary>
		public static readonly ItemFlingEffect USE_BERRY			= new ItemFlingEffect(3,	"USE_BERRY");
		/// <summary>
		/// Immediately activates the herb's effect on the target.
		/// </summary>
		public static readonly ItemFlingEffect USE_HERB				= new ItemFlingEffect(4,	"USE_HERB");
		/// <summary>
		/// Paralyzes the target.
		/// </summary>
		public static readonly ItemFlingEffect PARALYZES			= new ItemFlingEffect(5,	"PARALYZES");
		/// <summary>
		/// Poisons the target.
		/// </summary>
		public static readonly ItemFlingEffect POISONS				= new ItemFlingEffect(6,	"POISONS");
		/// <summary>
		/// Target will flinch if it has not yet gone this turn.
		/// </summary>
		public static readonly ItemFlingEffect FLINCH				= new ItemFlingEffect(7,	"FLINCH");
	}
}
