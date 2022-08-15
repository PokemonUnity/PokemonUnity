using System;
using System.Threading;

namespace PokemonUnity.Client.Transports
{
	public class EventSignal<T>
	{
		private int m_attemptCount;
		private readonly int m_maxAttempts;

		public event EventHandler<CustomEventArgs<T>> Finished;

		public EventSignal(int maxAttempts)
		{
			m_maxAttempts = maxAttempts;
		}

		public EventSignal()
			: this(5)
		{
		}

		public void OnFinish(T result)
		{
			var _handler = Finished;

			if (_handler == null)
			{
				if (maxAttemptsReached())
				{
					handleNoEventHandler();
					return;
				}
				m_attemptCount++;
				Thread.SpinWait(1000);
				OnFinish(result);
				return;
			}

			_handler.Invoke(this, new CustomEventArgs<T>
			{
				Result = result
			});
		}

		protected virtual void handleNoEventHandler()
		{
			throw new InvalidOperationException("You must attach an event handler to the event signal within a reasonable amount of time.");
		}

		private bool maxAttemptsReached()
		{
			return m_attemptCount > m_maxAttempts;
		}
	}
}