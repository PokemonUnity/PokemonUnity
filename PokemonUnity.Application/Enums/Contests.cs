namespace PokemonUnity.Enums
{
	public class Contests : PokemonUnity.Shared.Enums.Contests
	{
		protected Contests(int id, string name) : base(id, name) { }
		public static readonly Contests COOL			= new Contests(0,	"Cool"); 
		public static readonly Contests BEAUTY			= new Contests(0,	"Beauty"); 
		public static readonly Contests CUTE			= new Contests(0,	"Cute"); 
		public static readonly Contests SMART			= new Contests(0,	"Smart"); 
		public static readonly Contests TOUGH			= new Contests(0,	"Tough"); 
		public static readonly Contests SHEEN			= new Contests(0,	"Sheen");
	}
}