using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonUnity.Client.Hubs
{
	public interface IObservable<T>
	{
		IDisposable Subscribe(IObserver<T> observer);
	}
}
