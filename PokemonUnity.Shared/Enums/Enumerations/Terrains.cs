namespace PokemonUnity.Shared.Enums
{
	/// <summary>
	/// Terrain Tags or Tiles a player can be stepping on;
	/// used to contruct map floor plane
	/// </summary>
	public class Terrains : Enumeration
	{
		protected Terrains(int id, string name) : base(id, name) { }
		public static readonly Terrains Ledge				= new Terrains(1,	"Ledge");
		public static readonly Terrains Grass				= new Terrains(2,	"Grass");
		public static readonly Terrains Sand				= new Terrains(3,	"Sand");
		public static readonly Terrains Rock				= new Terrains(4,	"Rock");
		public static readonly Terrains DeepWater			= new Terrains(5,	"DeepWater");
		public static readonly Terrains StillWater			= new Terrains(6,	"StillWater");
		public static readonly Terrains Water				= new Terrains(7,	"Water");
		public static readonly Terrains Waterfall			= new Terrains(8,	"Waterfall");
		public static readonly Terrains WaterfallCrest		= new Terrains(9,	"WaterfallCrest");
		public static readonly Terrains TallGrass			= new Terrains(10,	"TallGrass");
		public static readonly Terrains UnderwaterGrass		= new Terrains(11,	"UnderwaterGrass");
		public static readonly Terrains Ice					= new Terrains(12,	"Ice");
		public static readonly Terrains Neutral				= new Terrains(13,	"Neutral");
		public static readonly Terrains SootGrass			= new Terrains(14,	"SootGrass");
		public static readonly Terrains Bridge				= new Terrains(15,	"Bridge");
		public static readonly Terrains Puddle				= new Terrains(16,	"Puddle");
	}
}