using System;
using System.Collections;

namespace PokemonEssentials.Interface
{
	public interface ISpriteTimer : IDisposable
	{
		ISpriteTimer initialize(IViewport viewport = null);

		void dispose();
		
		bool disposed { get; }
		
		IEnumerator update();
	}
}