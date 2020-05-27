using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PokemonUnity;
using PokemonUnity.Monster;

namespace PokemonUnity.Character
{
	/// <summary>
	/// </summary>
	public struct PlayerOutfit : IEquatable<PlayerOutfit>, IEqualityComparer<PlayerOutfit>
	{
		#region Variables
		public int Outfit	{ get; private set; }
		public int Shirt	{ get; private set; }
		public int Pant		{ get; private set; }
		public int Misc		{ get; private set; }
		public int Hat		{ get; private set; }
		#endregion

		#region Constructor
		public PlayerOutfit(int outfit, int shirt, int pant, int misc, int hat)
		{
			Outfit	= outfit;
			Shirt	= shirt;
			Pant	= pant;
			Misc	= misc;
			Hat		= hat;
		}
		#endregion

		#region Explicit Operators
		public static bool operator ==(PlayerOutfit x, PlayerOutfit y)
		{
			return true;
		}
		public static bool operator !=(PlayerOutfit x, PlayerOutfit y)
		{
			return false;
		}
		public bool Equals(PlayerOutfit obj)
		{
			if (obj == null) return false;
			return this == obj; 
		}
		public bool Equals(Character.Player obj)
		{
			if (obj == null) return false;
			return this == obj.Outfit; 
		}
		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (obj.GetType() == typeof(Player))
				return Equals((Player)obj);
			if (obj.GetType() == typeof(PlayerOutfit))
				return Equals((PlayerOutfit)obj);
			return base.Equals(obj);
		}
		public override int GetHashCode()
		{
			return (Outfit).GetHashCode();
		}
		bool IEquatable<PlayerOutfit>.Equals(PlayerOutfit other)
		{
			return Equals(obj: (object)other);
		}
		bool IEqualityComparer<PlayerOutfit>.Equals(PlayerOutfit x, PlayerOutfit y)
		{
			return x == y;
		}
		int IEqualityComparer<PlayerOutfit>.GetHashCode(PlayerOutfit obj)
		{
			return obj.GetHashCode();
		}
		#endregion
	}
}