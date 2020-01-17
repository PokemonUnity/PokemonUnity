using System.Collections;
using PokemonUnity;
using PokemonUnity.Monster;

namespace PokemonUnity.Character
{
	/// <summary>
	/// MetaData struct for Trainer, Secret, Gender (use for pokemons, and players)
	/// </summary>
	public struct TrainerId
	{
		#region Variables
		/// <summary>
		/// The name of the trainer type, as seen by the player. 
		/// Multiple trainer types can have the same display name, 
		/// although they cannot share ID numbers or internal names.
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// The gender of all trainers of this type. Is one of:
		/// Male, Female, Mixed(i.e. if the type shows a pair of trainers)
		/// Optional. If undefined, the default is "Mixed".
		/// </summary>
		public bool Gender { get; private set; }
		/// <summary>
		/// IDfinal = (IDtrainer + IDsecret × 65536).Last6
		/// </summary>
		/// <remarks>
		/// only the last six digits are used so the Trainer Card will display an ID No.
		/// </remarks>
		public string PlayerID
		{
			get
			{
				//return GetHashCode().ToString().Substring(GetHashCode().ToString().Length - 6, GetHashCode().ToString().Length);
				ulong n = (ulong)(TrainerID + SecretID * 65536);
				string x = n.ToString();
				//x = x.PadLeft(6, '0');
				return x.Substring(x.Length - 6, 6);
			}
		}
		public uint TrainerID { get; private set; }
		public uint SecretID { get; private set; }
		//public int[] Region { get; private set; }
		#endregion

		#region Constructor
		public TrainerId(string name, bool gender)
		{
			TrainerID = (uint)Core.Rand.Next(1000000); //random number between 0 and 999999, including 0
			SecretID = (uint)Core.Rand.Next(1000000); //random number between 0 and 999999, including 0
			//Region = Game.Features.ToCharArray();
			Name = name;
			Gender = gender;
		}
		#endregion

		#region Explicit Operators
		public static bool operator ==(TrainerId x, TrainerId y)
		{
			return ((x.Gender == y.Gender) && (x.TrainerID == y.TrainerID) && (x.SecretID == y.SecretID)) & (x.Name == y.Name);
		}
		public static bool operator !=(TrainerId x, TrainerId y)
		{
			return ((x.Gender != y.Gender) || (x.TrainerID != y.TrainerID) || (x.SecretID != y.SecretID)) | (x.Name == y.Name);
		}
		//public bool Equals(Pokemon obj)
		//{
		//	return this == obj.OT; 
		//}
		//public bool Equals(Character.Player obj)
		//{
		//	return this == obj.Trainer; //Equals(obj.Trainer);
		//}
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}
		public override int GetHashCode()
		{
			return ((ulong)(TrainerID + SecretID * 65536)).GetHashCode();
		}
		#endregion
	}
}