namespace PokemonUnity.Shared.Enums
{
	public class Statuses : Enumeration
	{
		protected Statuses(int id, string name) : base(id, name) { }
		public static readonly Statuses NONE				= new Statuses(0,	"NONE");
		public static readonly Statuses SLEEP				= new Statuses(0,	"SLEEP");
		public static readonly Statuses POISON				= new Statuses(0,	"POISON");
		public static readonly Statuses PARALYSIS			= new Statuses(0,	"PARALYSIS");
		public static readonly Statuses BURN				= new Statuses(0,	"BURN");
		public static readonly Statuses FROZEN				= new Statuses(0,	"FROZEN");
		public static readonly Statuses FAINT				= new Statuses(0,	"FAINT");
	}
}