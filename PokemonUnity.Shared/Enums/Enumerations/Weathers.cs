namespace PokemonUnity.Shared.Enums
{
	/// <summary>
	/// Weather options used in a pokemon <see cref="PokemonEssentials.Interface.PokeBattle.IBattle"/>
	/// </summary>
	public class Weathers : Enumeration
	{
		public Weathers(int id, string name) : base(id, name) { }
		public static readonly Weathers NONE							= new Weathers(0,	"Weather_Text_NONE");
		public static readonly Weathers RAINDANCE						= new Weathers(0,	"Weather_Text_RAINDANCE");
		public static readonly Weathers HEAVYRAIN						= new Weathers(0,	"Weather_Text_HEAVYRAIN");
		public static readonly Weathers SUNNYDAY						= new Weathers(0,	"Weather_Text_SUNNYDAY");
		public static readonly Weathers HARSHSUN						= new Weathers(0,	"Weather_Text_HARSHSUN");
		public static readonly Weathers SANDSTORM						= new Weathers(0,	"Weather_Text_SANDSTORM");
		public static readonly Weathers STRONGWINDS						= new Weathers(0,	"Weather_Text_STRONGWINDS");
		public static readonly Weathers HAIL							= new Weathers(0,	"Weather_Text_HAIL");
		public static readonly Weathers SHADOWSKY						= new Weathers(0,	"Weather_Text_SHADOWSKY");
	}
}