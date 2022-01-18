namespace PokemonUnity.Shared.Enums
{
	/// <summary>
	/// </summary>
	public class Conditions : Enumeration
	{
		protected Conditions(int id, string name) : base(id, name) { }
		public static readonly Conditions SWARM			= new Conditions(1,	"SWARM");
		public static readonly Conditions TIME			= new Conditions(2,	"TIME");
		public static readonly Conditions RADAR			= new Conditions(3,	"RADAR");
		public static readonly Conditions SLOT			= new Conditions(4,	"SLOT");
		public static readonly Conditions RADIO			= new Conditions(5,	"RADIO");
		public static readonly Conditions SEASON			= new Conditions(6,	"SEASON");
	}
}