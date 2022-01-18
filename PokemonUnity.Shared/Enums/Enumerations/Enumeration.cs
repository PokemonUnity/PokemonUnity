using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using PokemonUnity;

namespace PokemonUnity.Shared.Enums
{
	public abstract class Enumeration : Enumeration<int>, IComparable, IEquatable<int>, IEqualityComparer<int>
	{
		protected Enumeration(int id, string name) : base(id, name)
		{
		}

		public bool Equals(int other)
		{
			return Id == other;
		}

		public bool Equals(int x, int y)
		{
			return x == y;
		}

		public int GetHashCode(int obj)
		{
			return obj.GetHashCode();
		}

		public static T FromValue<T>(int value) where T : Enumeration
		{
			var matchingItem = Parse<T, int>(value, "value", item => item.Id == value);
			return matchingItem;
		}

		public static T FromDisplayName<T>(string displayName) where T : Enumeration
		{
			//var matchingItem = Parse<T, string>(displayName, "display name", item => item.Name == displayName);
			var matchingItem = Parse<T, string>(displayName, "display name", item => item.Name.ToLower() == displayName.ToLower());
			return matchingItem;
		}

		private static T Parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration
		{
			var matchingItem = GetAll<T>().FirstOrDefault(predicate);

			if (matchingItem == null)
				throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(T)}");

			return matchingItem;
		}

		public static bool operator ==(Enumeration a, Enumeration b)
		{
			if ((object)a == null && (object)b == null) return true;
			if ((object)a == null || (object)b == null) return false;
			return a.Id == b.Id;
		}

		public static bool operator !=(Enumeration a, Enumeration b)
		{
			return !(a.Id == b.Id);
		}
	}

	public abstract class Enumeration<TValue> : IComparable
		where TValue : struct, IComparable//, IEquatable<TValue>, IEqualityComparer<TValue>
	{
		/// <summary>
		/// Localization string identifier
		/// </summary>
		public virtual string Name { get; private set; }

		public TValue Id { get; private set; }

		protected Enumeration(TValue id, string name)
		{
			Id = id;
			Name = name;
		}

		public override string ToString() { return Name; }

		public static IEnumerable<T> GetAll<T>() where T : Enumeration<TValue>
		{
			return
				typeof(T).GetFields(BindingFlags.Public |
									BindingFlags.Static |
									BindingFlags.DeclaredOnly)
						 .Select(f => f.GetValue(null))
						 .Cast<T>();
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Enumeration<TValue> otherValue))
			{
				return false;
			}

			var typeMatches = GetType().Equals(obj.GetType());
			var valueMatches = Id.Equals(otherValue.Id);

			return typeMatches && valueMatches;
		}

		public int CompareTo(object other) { return Id.CompareTo(((Enumeration<TValue>)other).Id); }
		//public abstract int CompareTo(object other);

		public override int GetHashCode() { return Id.GetHashCode(); }

		//public static T FromValue<T>(TValue value) where T : Enumeration<TValue>
		//{
		//	//var matchingItem = Parse<T, TValue>(value, "value", item => item.Id == value);
		//	var matchingItem = Parse<T, TValue>(value, "value", item => item.Id.Equals(value));
		//	return matchingItem;
		//}

		//public static T FromDisplayName<T>(string displayName) where T : Enumeration<TValue>
		//{
		//	var matchingItem = Parse<T, string>(displayName, "display name", item => item.Name == displayName);
		//	return matchingItem;
		//}

		//private static T Parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration<TValue>
		//{
		//	var matchingItem = GetAll<T>().FirstOrDefault(predicate);
		//
		//	if (matchingItem == null)
		//		throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(T)}");
		//
		//	return matchingItem;
		//}

		//public static bool operator ==(Enumeration<TValue> a, Enumeration<TValue> b)
		//{
		//	if ((object)a == null && (object)b == null) return true;
		//	if ((object)a == null || (object)b == null) return false;
		//	return a.Id == b.Id;
		//}
		//
		//public static bool operator !=(Enumeration<TValue> a, Enumeration<TValue> b)
		//{
		//	return !(a.Id == b.Id);
		//}
	}
}