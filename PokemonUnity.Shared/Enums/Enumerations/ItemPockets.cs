namespace PokemonUnity.Shared.Enums
{
	public class ItemPockets : Enumeration
	{
		protected ItemPockets(int id, string name) : base(id, name) {  }
		public static readonly ItemPockets MISC			= new ItemPockets(1,	"MISC");
		public static readonly ItemPockets MEDICINE		= new ItemPockets(2,	"MEDICINE");
		public static readonly ItemPockets POKEBALL		= new ItemPockets(3,	"POKEBALL");
		/// <summary>
		/// TMs
		/// </summary>
		public static readonly ItemPockets MACHINE		= new ItemPockets(4,	"MACHINE");
		/// <summary>
		/// Plants?
		/// </summary>
		public static readonly ItemPockets BERRY		= new ItemPockets(5,	"BERRY");
		public static readonly ItemPockets MAIL			= new ItemPockets(6,	"MAIL");
		public static readonly ItemPockets BATTLE		= new ItemPockets(7,	"BATTLE");
		/// <summary>
		/// Items that are not 'stackable' and therefore should take up 1 individual item spot per (if multiples)
		/// Or possibly limit and restrict to one per user
		/// </summary>
		public static readonly ItemPockets KEY			= new ItemPockets(8,	"KEY");
	}
}