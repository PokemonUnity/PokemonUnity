using System;

namespace PokemonUnity.Client
{
	internal class DisposableAction : IDisposable
	{
		private readonly Action m_action;

		public DisposableAction(System.Action action)
		{
			m_action = action;
		}

		public void Dispose()
		{
			m_action();
		}
	}
}
