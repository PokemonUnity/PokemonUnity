using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonUnity.Client.Hubs
{
	public interface IObserver<T>
	{
		void OnNext(T value);
	}
}
