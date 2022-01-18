namespace PokemonUnity.Shared.Enums
{
	public class Natures : Enumeration
	{
		public Natures(int id, string name) : base(id, name) { }
		//UNSET,
		public static readonly Natures HARDY			= new Natures(0,	"HARDY");
		public static readonly Natures BOLD			= new Natures(0,	"BOLD");
		public static readonly Natures MODEST			= new Natures(0,	"MODEST");
		public static readonly Natures CALM			= new Natures(0,	"CALM");
		public static readonly Natures TIMID			= new Natures(0,	"TIMID");
		public static readonly Natures LONELY			= new Natures(0,	"LONELY");
		public static readonly Natures DOCILE			= new Natures(0,	"DOCILE");
		public static readonly Natures MILD			= new Natures(0,	"MILD");
		public static readonly Natures GENTLE			= new Natures(0,	"GENTLE");
		public static readonly Natures HASTY			= new Natures(0,	"HASTY");
		public static readonly Natures ADAMANT			= new Natures(0,	"ADAMANT");
		public static readonly Natures IMPISH			= new Natures(0,	"IMPISH");
		public static readonly Natures BASHFUL			= new Natures(0,	"BASHFUL");
		public static readonly Natures CAREFUL			= new Natures(0,	"CAREFUL");
		public static readonly Natures RASH			= new Natures(0,	"RASH");
		public static readonly Natures JOLLY			= new Natures(0,	"JOLLY");
		public static readonly Natures NAUGHTY			= new Natures(0,	"NAUGHTY");
		public static readonly Natures LAX			= new Natures(0,	"LAX");
		public static readonly Natures QUIRKY			= new Natures(0,	"QUIRKY");
		public static readonly Natures NAIVE			= new Natures(0,	"NAIVE");
		public static readonly Natures BRAVE			= new Natures(0,	"BRAVE");
		public static readonly Natures RELAXED			= new Natures(0,	"RELAXED");
		public static readonly Natures QUIET			= new Natures(0,	"QUIET");
		public static readonly Natures SASSY			= new Natures(0,	"SASSY");
		public static readonly Natures SERIOUS			= new Natures(0,	"SERIOUS");
	}
}