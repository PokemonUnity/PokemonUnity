using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonUnity.Client.Transports
{
	internal class CancellationTokenSource
	{
		public bool IsCancellationRequested { get; private set; }

		public void Cancel()
		{
			IsCancellationRequested = true;
		}
	}
}
