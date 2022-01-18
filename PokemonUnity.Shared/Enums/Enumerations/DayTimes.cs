namespace PokemonUnity.Shared.Enums
{
	public class DayTimes : Enumeration
	{
		protected DayTimes(int id, string name) : base(id, name) { }
		public static readonly DayTimes Night			= new DayTimes(0,	"Night");
		public static readonly DayTimes Morning			= new DayTimes(1,	"Morning");
		public static readonly DayTimes Day				= new DayTimes(2,	"Day");
		public static readonly DayTimes Evening			= new DayTimes(3,	"Evening");
	}
}