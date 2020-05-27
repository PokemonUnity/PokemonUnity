using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonUnity.Character
{
	public class Phone
	{
		private string[] generics;
		private string[] greetings;
		private string[] greetingsMorning;
		private string[] greetingsEvening;
		private string[] bodies1;
		private string[] bodies2;
		private string[] battleRequests;
		private string[] trainers;

		public string this[int msgType, int msgId]
		{
			get
			{
				if ((PhoneMsgType)msgType == PhoneMsgType.Greeting)
				{
					//if it's morning, and greetingMorning is not null
					//if it's evening, and greetingEvening is not null
					//else
					return greetings[msgId];
				}
				else return generics[msgId];
			}
		}

		//Constructor (string[] => private variables)

		enum PhoneMsgType : int
		{
			Generic			= 0,
			Greeting		= 1,
			Body			= 2,
			BattleRequest	= 3
		}
	}
}