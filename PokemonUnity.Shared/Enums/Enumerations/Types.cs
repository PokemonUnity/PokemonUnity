namespace PokemonUnity.Shared.Enums
{
	public class Types : Enumeration
	{
		protected Types(int id, string name) : base(id, name) { }
		public static readonly Types NONE				= new Types(0,		"NONE");
		public static readonly Types NORMAL				= new Types(1,		"NORMAL");
		public static readonly Types FIGHTING			= new Types(2,		"FIGHTING");
		public static readonly Types FLYING				= new Types(3,		"FLYING");
		public static readonly Types POISON				= new Types(4,		"POISON");
		public static readonly Types GROUND				= new Types(5,		"GROUND");
		public static readonly Types ROCK				= new Types(6,		"ROCK");
		public static readonly Types BUG				= new Types(7,		"BUG");
		public static readonly Types GHOST				= new Types(8,		"GHOST");
		public static readonly Types STEEL				= new Types(9,		"STEEL");
		public static readonly Types FIRE				= new Types(10,		"FIRE");
		public static readonly Types WATER				= new Types(11,		"WATER");
		public static readonly Types GRASS				= new Types(12,		"GRASS");
		public static readonly Types ELECTRIC			= new Types(13,		"ELECTRIC");
		public static readonly Types PSYCHIC			= new Types(14,		"PSYCHIC");
		public static readonly Types ICE				= new Types(15,		"ICE");
		public static readonly Types DRAGON				= new Types(16,		"DRAGON");
		public static readonly Types DARK				= new Types(17,		"DARK");
		public static readonly Types FAIRY				= new Types(18,		"FAIRY");
		public static readonly Types UNKNOWN			= new Types(10001,	"UNKNOWN");
		public static readonly Types SHADOW				= new Types(10002,	"SHADOW");
	}
}