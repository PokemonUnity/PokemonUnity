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
	public class DayCare
	{
		#region Variables
		//Daycare Data
		//(Slot 1) Occupied Flag 
		//(Slot 1) Steps Taken Since Depositing 
		//(Slot 1) Box EK6 1 
		//(Slot 2) Occupied Flag 
		//(Slot 2) Steps Taken Since Depositing2 
		//(Slot 2) Box EK6 2 
		//Flag (egg available) 
		//RNG Seed
		/// <summary>
		/// </summary>
		// KeyValuePair<Pokemon,steps>[]
		public KeyValuePair<Pokemon, int>[] Slot	{ get; private set; }
		public bool HasEgg	{ get; }
		#endregion

		#region Constructor
		public DayCare(int slots)
		{
			Slot = new KeyValuePair<Pokemon, int>[slots];
		}
		#endregion

		#region Explicit Operators
		/*public static bool operator ==(DayCare x, DayCare y)
		{
			return ((x.Gender == y.Gender) && (x.TrainerID == y.TrainerID) && (x.SecretID == y.SecretID)) & (x.Name == y.Name);
		}
		public static bool operator !=(DayCare x, DayCare y)
		{
			return ((x.Gender != y.Gender) || (x.TrainerID != y.TrainerID) || (x.SecretID != y.SecretID)) | (x.Name == y.Name);
		}
		public bool Equals(DayCare obj)
		{
			if (obj == null) return false;
			return this == obj; 
		}
		public bool Equals(Character.Player obj)
		{
			if (obj == null) return false;
			return this == obj; 
		}
		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (obj.GetType() == typeof(Player))
				return Equals((Player)obj);
			if (obj.GetType() == typeof(TrainerId))
				return Equals((TrainerId)obj);
			return base.Equals(obj);
		}
		public override int GetHashCode()
		{
			return ((ulong)(TrainerID + SecretID * 65536)).GetHashCode();
		}
		bool IEquatable<DayCare>.Equals(DayCare other)
		{
			return Equals(obj: (object)other);
		}
		bool IEqualityComparer<DayCare>.Equals(DayCare x, DayCare y)
		{
			return x == y;
		}
		int IEqualityComparer<DayCare>.GetHashCode(DayCare obj)
		{
			return obj.GetHashCode();
		}*/
		#endregion
	}
}