using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
		//C     = 13,
		//X     = 14,
		//Y     = 15,
		//Z     = 16,
		//L     = 17,
		//R     = 18,
		//SHIFT = 21,
		//CTRL  = 22,
		//ALT   = 23,
		//F5    = 25,
		//F6    = 26,
		//F7    = 27,
		//F8    = 28,
		//F9    = 29,
		LeftMouseKey  = -1,
		RightMouseKey = -2,
		ACTION	= 11,
		CANCEL	= 12,
		DEBUG	= 0
	}
}
