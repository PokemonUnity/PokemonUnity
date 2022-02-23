namespace PokemonUnity.UX
{
	public enum TextEntryTypes
	{
		NONE = 0,
		PLAYER = 1,
		POKEMON = 2,
		PC_BOX = 3,
		NPC = 4
	}
	public enum InputKeys : int
	{
		DOWN  = 2,
		LEFT  = 4,
		RIGHT = 6,
		UP    = 8,
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
		LeftMouseKey  = -1,
		RightMouseKey = -2,
		ACTION	= 11,
		CANCEL	= 12,
		DEBUG	= 0
		//F8    = 28, //Screenshot
		//F9    = 29, //DebugMenu
	}
}
