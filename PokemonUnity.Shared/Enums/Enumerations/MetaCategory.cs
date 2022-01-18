namespace PokemonUnity.Shared.Enums
{
	public class MetaCategory : Enumeration
	{
		protected MetaCategory(int id, string name) : base(id, name) { }
		/// <summary>
		/// Inflicts damage
		/// </summary>
		public static readonly MetaCategory DAMAGE					= new MetaCategory(0,	"DAMAGE");
		/// <summary>
		/// No damage; inflicts status ailment
		/// </summary>
		public static readonly MetaCategory AILMENT					= new MetaCategory(1,	"AILMENT");
		/// <summary>
		/// No damage; lowers target's stats or raises user's stats
		/// </summary>
		public static readonly MetaCategory NET_GOOD_STATS			= new MetaCategory(2,	"NET_GOOD_STATS");
		/// <summary>
		/// No damage; heals the user
		/// </summary>
		public static readonly MetaCategory HEAL					= new MetaCategory(3,	"HEAL");
		/// <summary>
		/// Inflicts damage; inflicts status ailment
		/// </summary>
		public static readonly MetaCategory DAMAGE_AILMENT			= new MetaCategory(4,	"DAMAGE_AILMENT");
		/// <summary>
		/// No damage; inflicts status ailment; raises target's stats
		/// </summary>
		public static readonly MetaCategory SWAGGER					= new MetaCategory(5,	"SWAGGER");
		/// <summary>
		/// Inflicts damage; lowers target's stats
		/// </summary>
		public static readonly MetaCategory DAMAGE_LOWER			= new MetaCategory(6,	"DAMAGE_LOWER");
		/// <summary>
		/// Inflicts damage; raises user's stats
		/// </summary>
		public static readonly MetaCategory DAMAGE_RAISE			= new MetaCategory(7,	"DAMAGE_RAISE");
		/// <summary>
		/// Inflicts damage; absorbs damage done to heal the user
		/// </summary>
		public static readonly MetaCategory DAMAGE_HEAL				= new MetaCategory(8,	"DAMAGE_HEAL");
		/// <summary>
		/// One-hit KO
		/// </summary>
		public static readonly MetaCategory OHKO					= new MetaCategory(9,	"OHKO");
		/// <summary>
		/// Effect on the whole field
		/// </summary>
		public static readonly MetaCategory WHOLE_FIELD_EFFECT		= new MetaCategory(10,	"WHOLE_FIELD_EFFECT");
		/// <summary>
		/// Effect on one side of the field
		/// </summary>
		public static readonly MetaCategory FIELD_EFFECT			= new MetaCategory(11,	"FIELD_EFFECT");
		/// <summary>
		/// Forces target to switch out
		/// </summary>
		public static readonly MetaCategory FORCE_SWITCH			= new MetaCategory(12,	"FORCE_SWITCH");
		/// <summary>
		/// Unique effect
		/// </summary>
		public static readonly MetaCategory UNIQUE					= new MetaCategory(13,	"UNIQUE");
	}
}