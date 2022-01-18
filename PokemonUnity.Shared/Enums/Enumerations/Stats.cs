namespace PokemonUnity.Shared.Enums
{
	public class Stats : Enumeration
	{
		public Stats(int id, string name) : base(id, name) { }
		public static readonly Stats HP									= new Stats(0,	"");
		public static readonly Stats ATTACK								= new Stats(1,	"");
		public static readonly Stats DEFENSE							= new Stats(2,	"");
		public static readonly Stats SPEED								= new Stats(3,	"");
		public static readonly Stats SPATK								= new Stats(4,	"");
		public static readonly Stats SPDEF								= new Stats(5,	"");
		/// <summary>
		/// battle only stat
		/// </summary>
		//public static readonly Stats ACCURACY							= new Stats(6,	"");
		/// <summary>
		/// battle only stat
		/// </summary>
		//public static readonly Stats EVASION							= new Stats(7,	"");
	}
}