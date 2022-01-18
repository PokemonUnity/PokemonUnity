namespace PokemonUnity.Shared.Enums
{
	public class MoveMethods : Enumeration
	{
		protected MoveMethods(int id, string name) : base(id, name) { }
		public static readonly MoveMethods levelup						= new MoveMethods(1,	"levelup");
		public static readonly MoveMethods egg							= new MoveMethods(2,	"egg");
		public static readonly MoveMethods tutor						= new MoveMethods(3,	"tutor");
		public static readonly MoveMethods machine						= new MoveMethods(4,	"machine");
		public static readonly MoveMethods stadium_surfing_pikachu		= new MoveMethods(5,	"stadium_surfing_pikachu");
		public static readonly MoveMethods light_ball_egg				= new MoveMethods(6,	"light_ball_egg");
		public static readonly MoveMethods colosseum_purification		= new MoveMethods(7,	"colosseum_purification");
		public static readonly MoveMethods xd_shadow					= new MoveMethods(8,	"xd_shadow");
		public static readonly MoveMethods xd_purification				= new MoveMethods(9,	"xd_purification");
		public static readonly MoveMethods form_change					= new MoveMethods(10,	"form_change");
		public static readonly MoveMethods shadow						= new MoveMethods(8,	"shadow");
		public static readonly MoveMethods purification					= new MoveMethods(9,	"purification");
	}
}