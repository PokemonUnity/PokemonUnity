namespace PokemonUnity.Shared.Enums
{
	public class Shapes : Enumeration
	{
		protected Shapes(int id, string name) : base(id, name) { }
		public static readonly Shapes BALL							= new Shapes(1,		"BALL");
		public static readonly Shapes SQUIGGLE						= new Shapes(2,		"SQUIGGLE");
		public static readonly Shapes FISH							= new Shapes(3,		"FISH");
		public static readonly Shapes ARMS							= new Shapes(4,		"ARMS");
		public static readonly Shapes BLOB							= new Shapes(5,		"BLOB");
		public static readonly Shapes UPRIGHT						= new Shapes(6,		"UPRIGHT");
		public static readonly Shapes LEGS							= new Shapes(7,		"LEGS");
		public static readonly Shapes QUADRUPED						= new Shapes(8,		"QUADRUPED");
		public static readonly Shapes WINGS							= new Shapes(9,		"WINGS");
		public static readonly Shapes TENTACLES						= new Shapes(10,	"TENTACLES");
		public static readonly Shapes HEADS							= new Shapes(11,	"HEADS");
		public static readonly Shapes HUMANOID						= new Shapes(12,	"HUMANOID");
		public static readonly Shapes BUG_WINGS						= new Shapes(13,	"BUG_WINGS");
		public static readonly Shapes ARMOR							= new Shapes(14,	"ARMOR");
	}
}