using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonUnity.Client.Http
{
	public class CallbackDetail<T>
	{
		public bool IsFaulted { get; set; }
		public Exception Exception { get; set; }
		public T Result { get; set; }
	}
}
