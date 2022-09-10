using PokemonUnity.Client.Http;
using PokemonUnity.Client.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace PokemonUnity.Client.Transports
{
	public class AsyncStreamReader
	{
		private readonly Stream m_stream;
		private readonly ChunkBuffer m_buffer;
		private readonly Action m_initializeCallback;
		private readonly Action m_closeCallback;
		private readonly IConnection m_connection;
		private int m_processingQueue;
		private int m_reading;
		private bool m_processingBuffer;

		public AsyncStreamReader(Stream stream,
			IConnection connection,
			Action initializeCallback,
			Action closeCallback)
		{
			m_initializeCallback = initializeCallback;
			m_closeCallback = closeCallback;
			m_stream = stream;
			m_connection = connection;
			m_buffer = new ChunkBuffer();
		}

		public bool Reading
		{
			get
			{
				return m_reading == 1;
			}
		}

		public void StartReading()
		{
			Debug.WriteLine("StartReading");
			if (Interlocked.Exchange(ref m_reading, 1) == 0)
				ReadLoop();
		}

		public void StopReading(bool raiseCloseCallback)
		{
			if (Interlocked.Exchange(ref m_reading, 0) == 1
				&& raiseCloseCallback)
				m_closeCallback();
		}

		private void ReadLoop()
		{
			if (!Reading)
				return;

			var _buffer = new byte[1024];
			var _signal = new EventSignal<CallbackDetail<int>>();

			_signal.Finished += (sender, e) =>
			{
				if (e.Result.IsFaulted)
				{
					Exception exception = e.Result.Exception.GetBaseException();

					if (!HttpBasedTransport.IsRequestAborted(exception))
					{
						if (!(exception is IOException))
							m_connection.OnError(exception);
						StopReading(true);
					}
					return;
				}

				int _read = e.Result.Result;

				if (_read > 0)
					// Put chunks in the buffer
					m_buffer.Add(_buffer, _read);

				if (_read == 0)
				{
					// Stop any reading we're doing
					StopReading(true);
					return;
				}

				// Keep reading the next set of data
				ReadLoop();

				if (_read <= _buffer.Length)
					// If we read less than we wanted or if we filled the buffer, process it
					ProcessBuffer();
			};
			StreamExtensions.ReadAsync(_signal, m_stream, _buffer);
		}

		private void ProcessBuffer()
		{
			if (!Reading)
				return;

			if (m_processingBuffer)
			{
				// Increment the number of times we should process messages
				m_processingQueue++;
				return;
			}

			m_processingBuffer = true;

			int _total = Math.Max(1, m_processingQueue);

			for (int i = 0; i < _total; i++)
			{
				if (!Reading)
					return;
				ProcessChunks();
			}

			if (m_processingQueue > 0)
				m_processingQueue -= _total;

			m_processingBuffer = false;
		}

		private void ProcessChunks()
		{
			Debug.WriteLine("ProcessChunks");
			while (Reading && m_buffer.HasChunks)
			{
				string _line = m_buffer.ReadLine();

				// No new lines in the buffer so stop processing
				if (_line == null)
					break;

				if (!Reading)
					return;

				// Try parsing the sseEvent
				SseEvent _sseEvent;
				if (!SseEvent.TryParse(_line, out _sseEvent))
					continue;

				if (!Reading)
					return;

				Debug.WriteLine("SSE READ: " + _sseEvent);

				switch (_sseEvent.Type)
				{
					case EventType.Id:
						m_connection.MessageId = _sseEvent.Data;
						break;
					case EventType.Data:
						if (_sseEvent.Data.Equals("initialized", StringComparison.OrdinalIgnoreCase))
						{
							if (m_initializeCallback != null)
								// Mark the connection as started
								m_initializeCallback();
						}
						else
						{
							if (Reading)
							{
								// We don't care about timeout messages here since it will just reconnect
								// as part of being a long running request
								bool _timedOutReceived;
								bool _disconnectReceived;

								HttpBasedTransport.ProcessResponse(
									m_connection,
									_sseEvent.Data,
									out _timedOutReceived,
									out _disconnectReceived);

								if (_disconnectReceived)
									m_connection.Stop();

								if (_timedOutReceived)
									return;
							}
						}
						break;
				}
			}
		}
	}
}
