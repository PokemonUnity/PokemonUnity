using System;
using System.IO;
using System.Net;
using PokemonUnity.Client.Infrastructure;

namespace PokemonUnity.Client.Http
{
	public class HttpWebResponseWrapper : IResponse
	{
		private readonly HttpWebResponse m_response;

		public HttpWebResponseWrapper(HttpWebResponse response)
		{
			m_response = response;
		}

		public string ReadAsString()
		{
			return HttpHelper.ReadAsString(m_response);
		}

		public Stream GetResponseStream()
		{
			return m_response.GetResponseStream();
		}

		public void Close()
		{
			((IDisposable)m_response).Dispose();
		}

		public bool IsFaulted { get; set; }

		public Exception Exception { get; set; }
	}
}
