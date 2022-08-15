using System;
using System.IO;

namespace PokemonUnity.Client.Http
{
	public interface IResponse
	{
		string ReadAsString();

		Stream GetResponseStream();

		void Close();

		bool IsFaulted { get; set; }

		Exception Exception { get; set; }
	}
}
