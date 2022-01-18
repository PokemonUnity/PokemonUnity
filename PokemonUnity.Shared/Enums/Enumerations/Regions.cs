namespace PokemonUnity.Shared.Enums
{
	/// <summary>
	/// Each "world" has their own smaller regions
	/// </summary>
	public class Regions : Enumeration
	{
		protected Regions(int id, string name) : base(id, name) { }
		public static readonly Regions NOT_IN_OVERWORLD		= new Regions(0,	"Region_Text_0");
		public static readonly Regions KANTO				= new Regions(1,	"Region_Text_1");
		public static readonly Regions JOHTO				= new Regions(2,	"Region_Text_2");
		public static readonly Regions HOENN				= new Regions(3,	"Region_Text_3");
		public static readonly Regions SINNOH				= new Regions(4,	"Region_Text_4");
		public static readonly Regions UNOVA				= new Regions(5,	"Region_Text_5");
		public static readonly Regions KALOS				= new Regions(6,	"Region_Text_6");
		public static readonly Regions ALOLA				= new Regions(7,	"Region_Text_7");
	}
}