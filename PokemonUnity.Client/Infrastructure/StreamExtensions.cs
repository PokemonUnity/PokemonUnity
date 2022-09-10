using System;
using System.IO;
using PokemonUnity.Client.Http;
using PokemonUnity.Client.Transports;

namespace PokemonUnity.Client.Infrastructure
{
	internal static class StreamExtensions
	{
		public static void ReadAsync(EventSignal<CallbackDetail<int>> signal, Stream stream, byte[] buffer)
		{
			var _state = new StreamState
			{
				Stream = stream,
				Response = signal,
				Buffer = buffer
			};

			ReadAsyncInternal(_state);
		}

		internal static void ReadAsyncInternal(StreamState streamState)
		{
			try
			{
				streamState.Stream.BeginRead(
					streamState.Buffer,
					0,
					streamState.Buffer.Length,
					GetResponseCallback,
					streamState);
			}
			catch (Exception exception)
			{
				streamState.Response.OnFinish(new CallbackDetail<int>
				{
					IsFaulted = true,
					Exception = exception
				});
			}
		}

		private static void GetResponseCallback(IAsyncResult asynchronousResult)
		{
			StreamState streamState = (StreamState)asynchronousResult.AsyncState;

			// End the operation
			try
			{
				var response = streamState.Stream.EndRead(asynchronousResult);
				streamState.Response.OnFinish(new CallbackDetail<int>
				{
					Result = response
				});
			}
			catch (Exception ex)
			{
				try
				{
					ReadAsyncInternal(streamState);
				}
				catch (Exception)
				{
					streamState.Response.OnFinish(new CallbackDetail<int>
					{
						IsFaulted = true,
						Exception = ex
					});
				}
			}
		}
	}
}
