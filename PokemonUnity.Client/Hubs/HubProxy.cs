using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PokemonUnity.Client.Transports;

namespace PokemonUnity.Client.Hubs
{
	public class HubProxy : IHubProxy
	{
		private readonly string m_hubName;
		private readonly IConnection m_connection;
		private readonly Dictionary<string, object> m_state = 
			new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
		private readonly Dictionary<string, Subscription> m_subscriptions = 
			new Dictionary<string, Subscription>(StringComparer.OrdinalIgnoreCase);

		public HubProxy(IConnection connection, string hubName)
		{
			m_connection = connection;
			m_hubName = hubName;
		}

		public object this[string name]
		{
			get
			{
				object value;
				m_state.TryGetValue(name, out value);
				return value;
			}
			set
			{
				m_state[name] = value;
			}
		}

		public Subscription Subscribe(string eventName)
		{
			if (eventName == null)
				throw new ArgumentNullException("eventName");

			Subscription _subscription;
			if (!m_subscriptions.TryGetValue(eventName, out _subscription))
			{
				_subscription = new Subscription();
				m_subscriptions.Add(eventName, _subscription);
			}

			return _subscription;
		}

		public EventSignal<object> Invoke(string method, params object[] args)
		{
			return Invoke<object>(method, args);
		}

		public EventSignal<T> Invoke<T>(string method, params object[] args)
		{
			if (method == null)
				throw new ArgumentNullException("method");

			var hubData = new HubInvocation
			{
				Hub = m_hubName,
				Method = method,
				Args = args,
				State = m_state,
				CallbackId = "1"
			};

			var _value = JsonConvert.SerializeObject(hubData);
			var _newSignal = new OptionalEventSignal<T>();
			var _signal = m_connection.Send<HubResult<T>>(_value);

			_signal.Finished += (sender, e) =>
			{
				if (e.Result != null)
				{
					if (e.Result.Error != null)
						throw new InvalidOperationException(e.Result.Error);

					HubResult<T> _hubResult = e.Result;
					if (_hubResult.State != null)
					{
						foreach (var pair in _hubResult.State)
						{
							this[pair.Key] = pair.Value;
						}
					}

					_newSignal.OnFinish(_hubResult.Result);
				}
				else
				{
					_newSignal.OnFinish(default(T));
				}
			};
			return _newSignal;
		}

		public void InvokeEvent(string eventName, object[] args)
		{
			Subscription eventObj;
			if (m_subscriptions.TryGetValue(eventName, out eventObj))
				eventObj.OnData(args);
		}

		public IEnumerable<string> GetSubscriptions()
		{
			return m_subscriptions.Keys;
		}
	}
}
