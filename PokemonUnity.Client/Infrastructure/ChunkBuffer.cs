using System;
using System.Text;

namespace PokemonUnity.Client.Infrastructure
{
	public class ChunkBuffer
	{
		private int m_offset;
		private readonly StringBuilder m_buffer;
		private readonly StringBuilder m_lineBuilder;

		public ChunkBuffer()
		{
			m_buffer = new StringBuilder();
			m_lineBuilder = new StringBuilder();
		}

		public bool HasChunks
		{
			get
			{
				return m_offset < m_buffer.Length;
			}
		}

		public string ReadLine()
		{
			// Lock while reading so that we can make safe assumptions about the buffer indices
			lock (m_buffer)
			{
				for (int i = m_offset; i < m_buffer.Length; i++, m_offset++)
				{
					if (m_buffer[i] == '\n')
					{
						m_buffer.Remove(0, m_offset + 1);

						string _line = m_lineBuilder.ToString();
						m_lineBuilder.Length = 0;
						m_offset = 0;
						return _line;
					}
					m_lineBuilder.Append(m_buffer[i]);
				}
				return null;
			}
		}

		public void Add(byte[] buffer, int length)
		{
			lock (m_buffer)
			{
				m_buffer.Append(Encoding.UTF8.GetString(buffer, 0, length));
			}
		}

		public void Add(ArraySegment<byte> buffer)
		{
			m_buffer.Append(Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count));
		}
	}
}
