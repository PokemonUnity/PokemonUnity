namespace PokemonUnity.Shared.Enums
{
	public class InputKeys : Enumeration
	{
		protected InputKeys(int id, string name) : base(id, name) { }
		public static readonly InputKeys DOWN		= new InputKeys(2,	"DOWN");
		public static readonly InputKeys LEFT		= new InputKeys(4,	"LEFT");
		public static readonly InputKeys RIGHT		= new InputKeys(6,	"RIGHT");
		public static readonly InputKeys UP			= new InputKeys(8,	"UP");
		//A     = 11,
		//B     = 12,
		//C     = 13, //Use
		//X     = 14, //Back
		//Y     = 15,
		//Z     = 16, //Action
		//L     = 17,
		//R     = 18,
		//SHIFT = 21, //Action
		//CTRL  = 22,
		//ALT   = 23,
		//F5    = 25,
		//F6    = 26,
		//F7    = 27,
		//F8    = 28, //Screenshot
		//F9    = 29, //DebugMenu
		public static readonly InputKeys LeftMouseKey	= new InputKeys(-1,	"LeftMouseKey");
		public static readonly InputKeys RightMouseKey	= new InputKeys(-2,	"RightMouseKey");
		public static readonly InputKeys USE			= new InputKeys(10,	"USE");
		public static readonly InputKeys ACTION			= new InputKeys(11,	"ACTION");
		public static readonly InputKeys CANCEL			= new InputKeys(12,	"CANCEL");
		public static readonly InputKeys DEBUG			= new InputKeys(0, 	"DEBUG");
	}
}
