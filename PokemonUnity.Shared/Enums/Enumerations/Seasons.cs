namespace PokemonUnity.Shared.Enums
{
	public class Seasons : Enumeration
	{
		protected Seasons(int id, string name) : base(id, name) { }
		public static readonly Seasons Summer				= new Seasons(0,	"Season_Text_Summer");
		public static readonly Seasons Winter				= new Seasons(0,	"Season_Text_Winter");
		public static readonly Seasons Fall					= new Seasons(0,	"Season_Text_Fall");
		public static readonly Seasons Spring				= new Seasons(0,	"Season_Text_Spring");
		public static readonly Seasons Volcanic				= new Seasons(0,	"Season_Text_Volcanic");
	}
}