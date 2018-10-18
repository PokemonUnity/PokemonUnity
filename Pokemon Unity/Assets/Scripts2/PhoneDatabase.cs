using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class PhoneDatabase
{
	public string[] generics;
	public string[] greetings;
	public string[] greetingsMorning;
	public string[] greetingsEvening;
	public string[] bodies1;
	public string[] bodies2;
	public string[] battleRequests;
	public string[] trainers;

	enum PhoneMsgType
	{
		Generic			= 0,
		Greeting		= 1,
		Body			= 2,
		BattleRequest	= 3
	}
}

namespace PokemonUnity
{
}
