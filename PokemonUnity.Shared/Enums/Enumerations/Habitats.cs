namespace PokemonUnity.Shared.Enums
{
	public class Habitats : Enumeration
	{
		protected Habitats(int id, string name) : base(id, name) { }
		public static readonly Habitats NONE						= new Habitats(0,	"NONE");
		public static readonly Habitats CAVE						= new Habitats(1,	"CAVE");
		public static readonly Habitats FOREST						= new Habitats(2,	"FOREST");
		public static readonly Habitats GRASSLAND					= new Habitats(3,	"GRASSLAND");
		public static readonly Habitats MOUNTAIN					= new Habitats(4,	"MOUNTAIN");
		public static readonly Habitats RARE						= new Habitats(5,	"RARE");
		public static readonly Habitats ROUGH_TERRAIN				= new Habitats(6,	"ROUGH_TERRAIN");
		public static readonly Habitats SEA							= new Habitats(7,	"SEA");
		public static readonly Habitats URBAN						= new Habitats(8,	"URBAN");
		public static readonly Habitats WATERS_EDGE					= new Habitats(9,	"WATERS_EDGE");
	}
}