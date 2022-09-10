using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using PokemonUnity.Client.Transports;
using SignalR.Infrastructure;

namespace PokemonUnity.Client.Http
{
	internal static class HttpHelper
	{
		public static EventSignal<CallbackDetail<HttpWebResponse>> PostAsync(string url)
		{
			return PostInternal(url, null, null);
		}

		public static void PostAsync(string url, IDictionary<string, string> postData)
		{
			PostInternal(url, null, postData);
		}

		public static EventSignal<CallbackDetail<HttpWebResponse>> PostAsync(
			string url,
			Action<HttpWebRequest> requestPreparer)
		{
			return PostInternal(url, requestPreparer, null);
		}

		public static EventSignal<CallbackDetail<HttpWebResponse>> PostAsync(
			string url,
			Action<HttpWebRequest> requestPreparer,
			IDictionary<string, string> postData)
		{
			return PostInternal(url, requestPreparer, postData);
		}

		public static string ReadAsString(HttpWebResponse response)
		{
			try
			{
				using (response)
				{
					using (Stream stream = response.GetResponseStream())
					{
						using (var reader = new StreamReader(stream))
						{
							return reader.ReadToEnd();
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(string.Format("Failed to read response: {0}", ex));

				// Swallow exceptions when reading the response stream and just try again.
				return null;
			}
		}

		private static EventSignal<CallbackDetail<HttpWebResponse>> PostInternal(
			string url,
			Action<HttpWebRequest> requestPreparer,
			IDictionary<string, string> postData)
		{
			var _request = (HttpWebRequest)HttpWebRequest.Create(url);

			if (requestPreparer != null)
				requestPreparer(_request);

			byte[] _buffer = ProcessPostData(postData);

			_request.Method = "POST";
			_request.ContentType = "application/x-www-form-urlencoded";
			// Set the content length if the buffer is non-null
			_request.ContentLength = _buffer != null ? _buffer.LongLength : 0;

			EventSignal<CallbackDetail<HttpWebResponse>> _signal =
				new EventSignal<CallbackDetail<HttpWebResponse>>();

			if (_buffer == null)
			{
				// If there's nothing to be written to the request then just get the response
				GetResponseAsync(_request, _signal);
				return _signal;
			}

			RequestState _requestState = new RequestState
			{
				PostData = _buffer,
				Request = _request,
				Response = _signal
			};

			try
			{
				_request.BeginGetRequestStream(GetRequestStreamCallback, _requestState);
			}
			catch (Exception ex)
			{
				_signal.OnFinish(new CallbackDetail<HttpWebResponse> { IsFaulted = true, Exception = ex });
			}
			return _signal;
		}

		public static EventSignal<CallbackDetail<HttpWebResponse>> GetAsync(string url)
		{
			return GetAsync(url, null);
		}

		public static EventSignal<CallbackDetail<HttpWebResponse>> GetAsync(string url, Action<HttpWebRequest> requestPreparer)
		{
			var _request = (HttpWebRequest)HttpWebRequest.Create(url);
			if (requestPreparer != null)
			{
				requestPreparer(_request);
			}
			var signal = new EventSignal<CallbackDetail<HttpWebResponse>>();
			GetResponseAsync(_request, signal);
			return signal;
		}

		public static void GetResponseAsync(HttpWebRequest request, EventSignal<CallbackDetail<HttpWebResponse>> signal)
		{
			try
			{
				request.BeginGetResponse(
					GetResponseCallback,
					new RequestState
					{
						Request = request,
						PostData = new byte[] { },
						Response = signal
					});
			}
			catch (Exception ex)
			{
				signal.OnFinish(new CallbackDetail<HttpWebResponse> { Exception = ex, IsFaulted = true });
			}
		}

		private static void GetRequestStreamCallback(IAsyncResult asynchronousResult)
		{
			RequestState _requestState = (RequestState)asynchronousResult.AsyncState;

			// End the operation
			try
			{
				Stream _postStream = _requestState.Request.EndGetRequestStream(asynchronousResult);

				// Write to the request stream.
				_postStream.Write(_requestState.PostData, 0, _requestState.PostData.Length);
				_postStream.Close();
			}
			catch (WebException exception)
			{
				_requestState.Response.OnFinish(new CallbackDetail<HttpWebResponse>
				{
					IsFaulted = true,
					Exception = exception
				});
				return;
			}

			// Start the asynchronous operation to get the response
			_requestState.Request.BeginGetResponse(GetResponseCallback, _requestState);
		}

		private static void GetResponseCallback(IAsyncResult asynchronousResult)
		{
			RequestState _requestState = (RequestState)asynchronousResult.AsyncState;

			// End the operation
			try
			{
				HttpWebResponse _response = (HttpWebResponse)_requestState.Request.EndGetResponse(asynchronousResult);
				_requestState.Response.OnFinish(new CallbackDetail<HttpWebResponse>
				{
					Result = _response
				});
			}
			catch (Exception ex)
			{
				Debug.WriteLine("GetResponseCallback: error occurred - " + ex.Message);
				_requestState.Response.OnFinish(new CallbackDetail<HttpWebResponse> { IsFaulted = true, Exception = ex });
			}
		}

		private static byte[] ProcessPostData(IDictionary<string, string> postData)
		{
			if (postData == null || postData.Count == 0)
				return null;

			var _stringB = new StringBuilder();
			foreach (var pair in postData)
			{
				if (_stringB.Length > 0)
					_stringB.Append("&");

				if (String.IsNullOrEmpty(pair.Value))
					continue;
				_stringB.AppendFormat("{0}={1}", pair.Key, UriQueryUtility.UrlEncode(pair.Value));
			}
			return Encoding.UTF8.GetBytes(_stringB.ToString());
		}
	}
}