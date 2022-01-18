namespace PokemonUnity.Shared.Enums
{
	public class EggGroups : Enumeration
	{
		protected EggGroups(int id, string name) : base(id, name) { }
		public static readonly EggGroups NONE						= new EggGroups(0,	"NONE");
		public static readonly EggGroups MONSTER					= new EggGroups(1,	"MONSTER");
		public static readonly EggGroups WATER1						= new EggGroups(2,	"WATER1");
		public static readonly EggGroups BUG						= new EggGroups(3,	"BUG");
		public static readonly EggGroups FLYING						= new EggGroups(4,	"FLYING");
		public static readonly EggGroups FIELD						= new EggGroups(5,	"FIELD"); //"Ground"?
		public static readonly EggGroups FAIRY						= new EggGroups(6,	"FAIRY");
		public static readonly EggGroups GRASS						= new EggGroups(7,	"GRASS"); //"Plant"
		public static readonly EggGroups HUMANLIKE					= new EggGroups(8,	"HUMANLIKE"); //"humanshape"
		public static readonly EggGroups WATER3						= new EggGroups(9,	"WATER3");
		public static readonly EggGroups MINERAL					= new EggGroups(10,	"MINERAL");
		public static readonly EggGroups AMORPHOUS					= new EggGroups(11,	"AMORPHOUS"); //"indeterminate"
		public static readonly EggGroups WATER2						= new EggGroups(12,	"WATER2");
		public static readonly EggGroups DITTO						= new EggGroups(13,	"DITTO");
		public static readonly EggGroups DRAGON						= new EggGroups(14,	"DRAGON");
		public static readonly EggGroups UNDISCOVERED				= new EggGroups(15,	"UNDISCOVERED"); //"no-eggs"
	}
}