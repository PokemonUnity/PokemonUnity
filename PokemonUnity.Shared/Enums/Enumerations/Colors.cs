namespace PokemonUnity.Shared.Enums
{
	public class Colors : Enumeration
	{
		protected Colors(int id, string name) : base(id, name) { }
		public static readonly Colors NONE				= new Colors(0,		"NONE");
		public static readonly Colors RED				= new Colors(8,		"RED");
		public static readonly Colors BLUE				= new Colors(2,		"BLUE");
		public static readonly Colors YELLOW			= new Colors(10,	"YELLOW");
		public static readonly Colors GREEN				= new Colors(5,		"GREEN");
		public static readonly Colors BLACK				= new Colors(1,		"BLACK");
		public static readonly Colors BROWN				= new Colors(3,		"BROWN");
		public static readonly Colors PURPLE			= new Colors(7,		"PURPLE");
		public static readonly Colors GRAY				= new Colors(4,		"GRAY");
		public static readonly Colors WHITE				= new Colors(9,		"WHITE");
		public static readonly Colors PINK				= new Colors(6,		"PINK");
	};
}