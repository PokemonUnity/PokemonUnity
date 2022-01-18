namespace PokemonUnity.Shared.Enums
{
	public class LevelingRate : Enumeration
	{
		protected LevelingRate(int id, string name) : base(id, name) { }
		public static readonly LevelingRate ERRATIC					= new LevelingRate(6,	"ERRATIC"); //fast then very slow?
		public static readonly LevelingRate FAST					= new LevelingRate(3,	"FAST");
		public static readonly LevelingRate MEDIUMFAST				= new LevelingRate(2,	"MEDIUMFAST"); //Medium?
		public static readonly LevelingRate MEDIUMSLOW				= new LevelingRate(4,	"MEDIUMSLOW");
		public static readonly LevelingRate SLOW					= new LevelingRate(1,	"SLOW");
		public static readonly LevelingRate FLUCTUATING				= new LevelingRate(5,	"FLUCTUATING"); //slow then fast?
	}
}