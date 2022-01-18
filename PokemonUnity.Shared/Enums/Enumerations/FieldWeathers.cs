namespace PokemonUnity.Shared.Enums
{
	/// <summary>
	/// Weather option for overworld.
	/// For battle use weather, use <see cref="Combat.Weather"/>
	/// </summary>
	public class FieldWeathers : Enumeration
	{
		protected FieldWeathers(int id, string name) : base(id, name) { }
		public static readonly FieldWeathers Clear				= new FieldWeathers(0,	"Clear");
		/// <summary>
		/// light drizzle?
		/// </summary>
		public static readonly FieldWeathers Rain				= new FieldWeathers(1,	"Rain");
		public static readonly FieldWeathers Snow				= new FieldWeathers(2,	"Snow");
		public static readonly FieldWeathers Underwater			= new FieldWeathers(3,	"Underwater");
		/// <summary>
		/// Sun
		/// </summary>
		public static readonly FieldWeathers Sunny				= new FieldWeathers(4,	"Sunny");
		public static readonly FieldWeathers Fog				= new FieldWeathers(5,	"Fog");
		/// <summary>
		/// Storm
		/// </summary>
		public static readonly FieldWeathers Thunderstorm		= new FieldWeathers(6,	"Thunderstorm");
		public static readonly FieldWeathers Sandstorm			= new FieldWeathers(7,	"Sandstorm");
		public static readonly FieldWeathers Ash				= new FieldWeathers(8,	"Ash");
		public static readonly FieldWeathers Blizzard			= new FieldWeathers(9,	"Blizzard");
		public static readonly FieldWeathers HeavyRain			= new FieldWeathers(10,	"HeavyRain");
	}
}