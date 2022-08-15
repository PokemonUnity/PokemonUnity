using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonUnity.Client.Transports
{
	public class CustomEventArgs<T> : EventArgs
	{
		public T Result { get; set; }
	}
}
