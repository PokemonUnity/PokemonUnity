namespace PokemonUnity.Enums
{
	public class Versions : PokemonUnity.Shared.Enums.Versions
	{
		public int GenerationId { get; private set; }
		protected Versions(int id, int genId, string name) : base(id, name) { }
		public static readonly Versions PokeUnity		= new Versions(0,	0,	"poke-unity");
		public static readonly Versions RED				= new Versions(1,	1,	"red");
		public static readonly Versions BLUE			= new Versions(2,	1,	"blue");
		public static readonly Versions YELLOW			= new Versions(3,	2,	"yellow");
		public static readonly Versions GOLD			= new Versions(4,	3,	"gold");
		public static readonly Versions SILVER			= new Versions(5,	3,	"silver");
		public static readonly Versions CRYSTAL			= new Versions(6,	4,	"crystal");
		public static readonly Versions RUBY			= new Versions(7,	5,	"ruby");
		public static readonly Versions SAPPHIRE		= new Versions(8,	5,	"sapphire");
		public static readonly Versions EMERALD			= new Versions(9,	6,	"emerald");
		public static readonly Versions FIRERED			= new Versions(10,	7,	"firered");
		public static readonly Versions LEAFGREEN		= new Versions(11,	7,	"leafgreen");
		public static readonly Versions DIAMOND			= new Versions(12,	8,	"diamond");
		public static readonly Versions PEARL			= new Versions(13,	8,	"pearl");
		public static readonly Versions PLATINUM		= new Versions(14,	9,	"platinum");
		public static readonly Versions HEARTGOLD		= new Versions(15,	10,	"heartgold");
		public static readonly Versions SOULSILVER		= new Versions(16,	10,	"soulsilver");
		public static readonly Versions BLACK			= new Versions(17,	11,	"black");
		public static readonly Versions WHITE			= new Versions(18,	11,	"white");
		public static readonly Versions COLOSSEUM		= new Versions(19,	12,	"colosseum");
		public static readonly Versions XD				= new Versions(20,	13,	"xd");
		public static readonly Versions BLACK_2			= new Versions(21,	14,	"black-2");
		public static readonly Versions WHITE_2			= new Versions(22,	14,	"white-2");
		public static readonly Versions X				= new Versions(23,	15,	"x");
		public static readonly Versions Y				= new Versions(24,	15,	"y");
		public static readonly Versions OMEGA_RUBY		= new Versions(25,	16,	"omega-ruby");
		public static readonly Versions ALPHA_SAPPHIRe	= new Versions(26,	16,	"alpha-sapphire");
	}
}