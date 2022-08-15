using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonUnity.Client.Transports
{
	public class OptionalEventSignal<T> : EventSignal<T>
	{
		protected override void handleNoEventHandler()
		{
		}
	}
}
