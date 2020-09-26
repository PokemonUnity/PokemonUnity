using System;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Monster;

namespace PokemonUnity.Character
{
	/// <summary>
	/// MetaData struct for Trainer, Secret, Gender (use for pokemons, and players)
	/// </summary>
	public struct TrainerData : IEquatable<TrainerData>, IEqualityComparer<TrainerData>
	{
		#region Variables
		//public Inventory.Items[] Items { get; private set; }
		/// <summary>
		/// This is how the scripts refer to the trainer type. 
		/// Typically this is the trainer type's name, 
		/// but written in all capital letters and with no spaces or symbols. 
		/// The internal name is never seen by the player.
		/// </summary>
		public TrainerTypes ID { get; private set; }
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
		public bool? Gender { get; private set; }
		#endregion

		#region Important Trainer Data
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
				//(ulong) Does it matter if value is pos or negative, if only need last 5-6?
				//ulong n = (ulong)(TrainerID + SecretID * 65536);
				ulong n = (ulong)System.Math.Abs(TrainerID + SecretID * 65536);
				if (n == 0) return "000000";
				string x = n.ToString();
				// = x.PadLeft(6,'0');
				return x.Substring(x.Length - 6, 6);
			}
		}
		public int TrainerID { get; private set; }
		public int SecretID { get; private set; }
		//public int[] Region { get; private set; }
		#endregion

		#region Use TrainerMetaData Fill Data
		/// <summary>
		/// The amount of money earned from defeating a trainer of this type. 
		/// The base money value is multiplied by the highest Level among all the trainer's 
		/// Pokémon to produce the actual amount of money gained (assuming no other modifiers). 
		/// Must be a number between 0 and 255.	
		/// Optional. If undefined, the default is 30.
		/// </summary>
		//public byte BaseMoney { get; private set; }
		public byte BaseMoney { get { return Game.TrainerMetaData[ID].BaseMoney; } }
		/*public bool Double { get; private set; }
		public int AI { get; private set; }
		//public int IVs { get; private set; }
		/// <summary>
		/// The name of a background music (BGM) file in the folder "Audio/BGM". 
		/// The music that plays during battles against trainers of this type. 
		/// Typically only defined for Gym Leaders, Elite Four members and rivals.	
		/// Optional. If undefined, the default BGM is used.
		/// </summary>
		public int 	BattleBGM { get; private set; }
		/// <summary>
		/// The name of a background music (BGM) file in the folder "Audio/BGM". 
		/// The victory background music that plays upon defeat of trainers of this type.	
		/// Optional. If undefined, the default victory BGM is used.
		/// </summary>
		public int 	VictoryBGM { get; private set; }
		/// <summary>
		/// The name of a music effect (ME) file in the folder "Audio/ME". 
		/// The music that plays before the battle begins, while still talking to the trainer.	
		/// Optional. If undefined, the default ME is used.
		/// </summary>
		public int 	IntroME { get; private set; }*/
		#endregion

		#region Constructor
		public TrainerData(string name, bool gender, int? tID = null, int? sID = null) : this(trainer: TrainerTypes.PLAYER, name)
		{
			//ID = TrainerTypes.PLAYER;
			//TrainerID = (uint)Core.Rand.Next(1000000); //random number between 0 and 999999, including 0
			//SecretID = (uint)Core.Rand.Next(1000000); //random number between 0 and 999999, including 0
			TrainerID = tID				?? TrainerID; //Core.Rand.Next(1000000);
			SecretID = sID				?? SecretID; //Core.Rand.Next(1000000); 
			//Region = Game.Features.ToCharArray();
			Name = name;
			Gender = gender;
		}

		public TrainerData(TrainerTypes trainer, string name = null)//, Inventory.Items[] items = null)
		{
			ID = trainer;
			TrainerID = Core.Rand.Next(1000000); //random number between 0 and 999999, including 0
			SecretID = Core.Rand.Next(1000000); //random number between 0 and 999999, including 0
			//if(trainer != TrainerTypes.Player && string.IsNullOrEmpty(name)
			//if(trainer != TrainerTypes.WildPokemon
			Name = name ?? ID.ToString();
			//Items = items ?? new Inventory.Items[0];
			//Party = new Pokemon[]
			//{
			//	new Pokemon(Pokemons.NONE),
			//	new Pokemon(Pokemons.NONE),
			//	new Pokemon(Pokemons.NONE),
			//	new Pokemon(Pokemons.NONE),
			//	new Pokemon(Pokemons.NONE),
			//	new Pokemon(Pokemons.NONE)
			//};
			Gender = 				trainer == TrainerTypes.PLAYER ? (bool?)null : Game.TrainerMetaData[trainer].Gender;
			//Double =				false;
			//Double = 				Game.TrainerMetaData[trainer].Double;
			//BaseMoney = 			Game.TrainerMetaData[trainer].BaseMoney;
			//AI = 					Game.TrainerMetaData[trainer].SkillLevel;
			//BattleBGM = 			Game.TrainerMetaData[trainer].BattleBGM;
			//VictoryBGM =			Game.TrainerMetaData[trainer].VictoryBGM;
			//IntroME =				Game.TrainerMetaData[trainer].IntroME;
		}
		#endregion

		#region Explicit Operators
		public static bool operator ==(TrainerData x, TrainerData y)
		{
			return ((x.Gender == y.Gender) && (x.TrainerID == y.TrainerID) && (x.SecretID == y.SecretID)) & (x.Name == y.Name);
		}
		public static bool operator !=(TrainerData x, TrainerData y)
		{
			return ((x.Gender != y.Gender) || (x.TrainerID != y.TrainerID) || (x.SecretID != y.SecretID)) | (x.Name == y.Name);
		}
		public bool Equals(TrainerData obj)
		{
			if (obj == null) return false;
			return this == obj;
		}
		public bool Equals(Character.Player obj)
		{
			if (obj == null) return false;
			return this == obj.Trainer; //Equals(obj.Trainer);
		}
		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (obj.GetType() == typeof(Player))
				return Equals((Player)obj);
			if (obj.GetType() == typeof(TrainerData))
				return Equals((TrainerData)obj);
			return base.Equals(obj);
		}
		public override int GetHashCode()
		{
			return ((ulong)(TrainerID + SecretID * 65536)).GetHashCode();
		}
		bool IEquatable<TrainerData>.Equals(TrainerData other)
		{
			return Equals(obj: (object)other);
		}
		bool IEqualityComparer<TrainerData>.Equals(TrainerData x, TrainerData y)
		{
			return x == y;
		}
		int IEqualityComparer<TrainerData>.GetHashCode(TrainerData obj)
		{
			return obj.GetHashCode();
		}
		#endregion
	}
}