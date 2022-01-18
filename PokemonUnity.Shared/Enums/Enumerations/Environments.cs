namespace PokemonUnity.Shared.Enums
{
	/// <summary>
	/// Used in battle system to reflect background theme and design
	/// </summary>
	public class Environments : Enumeration
	{
		protected Environments(int id, string name) : base(id, name) { }
		public static readonly Environments None				= new Environments(0,	"None");
		/// <summary>
		/// Normal Grass, and Sooty Tall Grass, are both grass but different colors
		/// </summary>
		public static readonly Environments Grass				= new Environments(1,	"Grass");
		public static readonly Environments Cave				= new Environments(6,	"Cave");
		public static readonly Environments Sand				= new Environments(8,	"Sand");
		public static readonly Environments Rock				= new Environments(7,	"Rock");
		public static readonly Environments MovingWater			= new Environments(3,	"MovingWater");
		public static readonly Environments StillWater			= new Environments(4,	"StillWater");
		public static readonly Environments Underwater			= new Environments(5,	"Underwater");
		/// <summary>
		/// Tall Grass
		/// </summary>
		public static readonly Environments TallGrass			= new Environments(2,	"TallGrass");
		public static readonly Environments Forest				= new Environments(9,	"Forest");
		public static readonly Environments Snow				= new Environments(10,	"Snow");
		public static readonly Environments Volcano				= new Environments(11,	"Volcano");
		public static readonly Environments Graveyard			= new Environments(12,	"Graveyard");
		public static readonly Environments Sky					= new Environments(13,	"Sky");
		public static readonly Environments Space				= new Environments(14,	"Space");
	}
}