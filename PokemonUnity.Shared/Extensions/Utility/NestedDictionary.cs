using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace PokemonUnity.Utility
{
	public class NestedDictionary<K, V> : Dictionary<K, NestedDictionary<K, V>>
	{
		public V Value { get; set; }

		public new NestedDictionary<K, V> this[K key]
		{
			get
			{
				if (!base.Keys.Contains<K>(key))
				{
					base[key] = new NestedDictionary<K, V>();
				}
				return base[key];
			}
			set { base[key] = value; }
		}

		public void Add(K key, V value) {
			if (!base.Keys.Contains<K>(key))
			{
				base[key] = new NestedDictionary<K, V>() { Value = value };
			}
		}

		public static explicit operator NestedDictionary<K, V>(Dictionary<K, V> d)
		{
			NestedDictionary<K, V> n = new NestedDictionary<K, V>(); //{ Value = d.Values };
			foreach (KeyValuePair<K,V> kv in d) n.Add(kv.Key, kv.Value);
			return n;
		}

		public static explicit operator Dictionary<K, V>(NestedDictionary<K, V> d)
		{
			Dictionary<K, V> n = new Dictionary<K, V>(); //{ Value = d.Values };
			foreach (KeyValuePair<K,NestedDictionary<K,V>> kv in d) n.Add(kv.Key, kv.Value.Value);
			return n;
		}
	}
}