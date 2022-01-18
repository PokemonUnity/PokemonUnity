namespace PokemonUnity.Shared.Enums
{
	public class HappinessMethods : Enumeration
	{
		protected HappinessMethods(int id, string name) : base(id, name) { }
		public static readonly HappinessMethods WALKING				= new HappinessMethods(0,	"WALKING");
		public static readonly HappinessMethods LEVELUP				= new HappinessMethods(0,	"LEVELUP");
		public static readonly HappinessMethods GROOM				= new HappinessMethods(0,	"GROOM");
		public static readonly HappinessMethods FAINT				= new HappinessMethods(0,	"FAINT");
		public static readonly HappinessMethods VITAMIN				= new HappinessMethods(0,	"VITAMIN");
		public static readonly HappinessMethods EVBERRY				= new HappinessMethods(0,	"EVBERRY");
		public static readonly HappinessMethods POWDER				= new HappinessMethods(0,	"POWDER");
		public static readonly HappinessMethods ENERGYROOT			= new HappinessMethods(0,	"ENERGYROOT");
		public static readonly HappinessMethods REVIVALHERB			= new HappinessMethods(0,	"REVIVALHERB");
		public static readonly HappinessMethods FRIENDBALL			= new HappinessMethods(0,	"FRIENDBALL");
	}
}