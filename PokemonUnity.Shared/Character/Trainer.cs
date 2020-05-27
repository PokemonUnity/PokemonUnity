using System.Collections;
using PokemonUnity;
using PokemonUnity.Monster;

namespace PokemonUnity.Character
{
	//ToDo: MetaData struct for Secret,Trainer,Gender (use for pokemons)
	public struct Trainer
	{
		#region Variables
		public Inventory.Items[] Items { get; private set; }
		public Pokemon[] Party { get; private set; } //ToDo: Remove Party from Trainer 
		public bool Double { get; private set; }
		public int AI { get; private set; }
		//public int IVs { get; private set; }
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
		public string fullname { get { return Name; } }
		/// <summary>
		/// The amount of money earned from defeating a trainer of this type. 
		/// The base money value is multiplied by the highest Level among all the trainer's 
		/// Pokémon to produce the actual amount of money gained (assuming no other modifiers). 
		/// Must be a number between 0 and 255.	
		/// Optional. If undefined, the default is 30.
		/// </summary>
		public byte BaseMoney { get; private set; }
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
		public int 	IntroME { get; private set; }
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
				ulong n = (ulong)(TrainerID + SecretID * 65536);
				if (n == 0) return "0";
				string x = n.ToString();
				// = x.PadLeft(6,'0');
				//return GetHashCode().ToString().Substring(GetHashCode().ToString().Length - 6, GetHashCode().ToString().Length);
				return x.Substring(x.Length - 6, 6);
			}
		}
		public int TrainerID { get; private set; }
		public int SecretID { get; private set; }
		#endregion

		#region NPC Details
		/*// <summary>
		/// After having defeated the trainer, when speaking to him/her again
		/// </summary>
		public string ScriptIdle { get; private set; }
		/// <summary>
		/// Start of trainer encounter
		/// </summary>
		public string ScriptBattleIntro { get; private set; }
		/// <summary>
		/// End of battle
		/// </summary>
		public string ScriptBattleEnd { get; private set; }*/
		#endregion

		#region Constructor
		public Trainer(TrainerTypes trainer)
		{
			ID = trainer;
			TrainerID = Core.Rand.Next(1000000); //random number between 0 and 999999, including 0
			SecretID = Core.Rand.Next(1000000); //random number between 0 and 999999, including 0
			Double = false;
			Party = new Pokemon[]
			{
				new Pokemon(Pokemons.NONE),
				new Pokemon(Pokemons.NONE),
				new Pokemon(Pokemons.NONE),
				new Pokemon(Pokemons.NONE),
				new Pokemon(Pokemons.NONE),
				new Pokemon(Pokemons.NONE)
			};
			Items = new Inventory.Items[0];
			//ScriptIdle = idle
			//ScriptBattleIntro = intro
			//ScriptBattleEnd = end
			//if(trainer != TrainerTypes.Player && string.IsNullOrEmpty(name)
			//if(trainer != TrainerTypes.WildPokemon
			Name = trainer.ToString();
			Gender = 				trainer == TrainerTypes.PLAYER ? (bool?)null : Game.TrainerMetaData[trainer].Gender; //ToDo: if wild?...
			Double = 				Game.TrainerMetaData[trainer].Double;
			BaseMoney = 			Game.TrainerMetaData[trainer].BaseMoney;
			AI = 					Game.TrainerMetaData[trainer].SkillLevel;
			BattleBGM = 			Game.TrainerMetaData[trainer].BattleBGM;
			VictoryBGM =			Game.TrainerMetaData[trainer].VictoryBGM;
			IntroME =				Game.TrainerMetaData[trainer].IntroME;
		}

		public Trainer(TrainerTypes trainer, Pokemon[] party, string name = null, Inventory.Items[] items = null, bool? dub = null, bool? gender = null, 
			int? tID = null, int? sID = null, int? intro = null, int? end = null, int? idle = null, byte? baseMoney = null, byte? skillLevel = null, 
			int? battleMusic = null, int? victoryMusic = null, int? introMusic = null) //: this(TrainerTypes.PLAYER)
		{
			ID = trainer;
			TrainerID = tID				?? Core.Rand.Next(1000000); //random number between 0 and 999999, including 0
			SecretID = sID				?? Core.Rand.Next(1000000); //random number between 0 and 999999, including 0
			Party = party;
			Items = items				?? new Inventory.Items[0];
			Gender = gender				?? trainer == TrainerTypes.PLAYER ? true //Game.GameData.Player.IsMale 
				: Game.TrainerMetaData[trainer].Gender; //ToDo: Gender = null?
			Double = dub				?? Game.TrainerMetaData[trainer].Double;
			BaseMoney = baseMoney		?? Game.TrainerMetaData[trainer].BaseMoney;
			AI = skillLevel				?? Game.TrainerMetaData[trainer].SkillLevel;
			BattleBGM = battleMusic		?? Game.TrainerMetaData[trainer].BattleBGM;
			VictoryBGM = victoryMusic	?? Game.TrainerMetaData[trainer].VictoryBGM;
			IntroME = introMusic		?? Game.TrainerMetaData[trainer].IntroME;
			//ScriptIdle = idle
			//ScriptBattleIntro = intro
			//ScriptBattleEnd = end
			//if(trainer != TrainerTypes.Player && string.IsNullOrEmpty(name)
			//if(trainer != TrainerTypes.WildPokemon
			Name = name					?? ID.ToString();
		}

		public Trainer(Player trainer, Pokemon[] party = null, int? tID = null, int? sID = null) 
			: this(TrainerTypes.PLAYER, name: trainer.Name, gender: trainer.IsMale, party: party ?? /*trainer.Trainer.Party*/new Pokemon[]
			{
				new Pokemon(Pokemons.NONE), new Pokemon(Pokemons.NONE), new Pokemon(Pokemons.NONE),
				new Pokemon(Pokemons.NONE), new Pokemon(Pokemons.NONE), new Pokemon(Pokemons.NONE)
			})
		{
			//if trainer is another player
			//if (tID.HasValue) TrainerID = tID.Value; //trainer.Trainer.TrainerID;
			//if (sID.HasValue) SecretID = sID.Value; //trainer.Trainer.SecretID;
			TrainerID = tID ?? trainer.Trainer.TrainerID;
			SecretID =  sID ?? trainer.Trainer.SecretID;
			//Change name being loaded
			//Name = trainer.Name; //name;
			//Load player's gender as well
			//Gender = trainer.IsMale; //gender;
		}

		//Sample from Pokemon Crystal
		//public Trainer(string name, TrainerTypes trainer, Pokemon[] party) { }
		#endregion

		#region Explicit Operators
		public static bool operator ==(Trainer t1, Trainer t2)
		{
			return ((t1.Gender == t2.Gender) && (t1.TrainerID == t2.TrainerID) && (t1.SecretID == t2.SecretID)) & (t1.Name == t2.Name);
		}
		public static bool operator !=(Trainer t1, Trainer t2)
		{
			return ((t1.Gender != t2.Gender) || (t1.TrainerID != t2.TrainerID) || (t1.SecretID != t2.SecretID)) | (t1.Name == t2.Name);
		}
		//public bool Equals(Character.Player obj)
		//{
		//	return this == obj.Trainer; //Equals(obj.Trainer);
		//}
		public override bool Equals(object obj)
		{
			//ToDo: If null, TrainerType => WildPokemon
			if (obj == null) return false; 
			return base.Equals(obj);
		}
		public override int GetHashCode()
		{
			return (TrainerID + SecretID * 65536).GetHashCode();
		}
		#endregion
	}
	public struct TrainerData
	{
		#region Variables
		/// <summary>
		/// This is how the scripts refer to the trainer type. 
		/// Typically this is the trainer type's name, 
		/// but written in all capital letters and with no spaces or symbols. 
		/// The internal name is never seen by the player.
		/// </summary>
		public TrainerTypes ID { get; private set; }
		public bool Double { get; private set; }
		/// <summary>
		/// The amount of money earned from defeating a trainer of this type. 
		/// The base money value is multiplied by the highest Level among all the trainer's 
		/// Pokémon to produce the actual amount of money gained (assuming no other modifiers). 
		/// Must be a number between 0 and 255.	
		/// Optional. If undefined, the default is 30.
		/// </summary>
		public byte BaseMoney { get; private set; }
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
		public int 	IntroME { get; private set; }
		/// <summary>
		/// The gender of all trainers of this type. Is one of:
		/// Male, Female, Mixed(i.e. if the type shows a pair of trainers)
		/// Optional. If undefined, the default is "Mixed".
		/// </summary>
		public bool? Gender { get; private set; }
		#endregion

		#region NPC Details
		/// <summary>
		/// The skill level of all trainers of this type, used for battle AI. 
		/// Higher numbers represent higher skill levels. 
		/// Must be a number between 0 and 255.	
		/// Optional. If undefined, default is equal to the base money value.
		/// </summary>
		public byte SkillLevel { get; private set; }
		/// <summary>
		/// A text field which can be used to modify the AI behaviour of all trainers of this type. 
		/// No such modifiers are defined by default, and there is no standard format. 
		/// See the page Battle AI for more details.
		/// Optional. If undefined, the default is blank.
		/// </summary>
		public int? SkillCodes { get; private set; }
		/// <summary>
		/// After having defeated the trainer, when speaking to him/her again
		/// </summary>
		public string ScriptIdle { get; private set; }
		/// <summary>
		/// Start of trainer encounter
		/// </summary>
		public string ScriptBattleIntro { get; private set; }
		/// <summary>
		/// End of battle
		/// </summary>
		public string ScriptBattleEnd { get; private set; }
		#endregion
	}
}

namespace PokemonUnity
{
	/*// <summary>
	/// It is important to note that the player also has a trainer type, 
	/// and it is defined in exactly the same way as any other trainer type. 
	/// </summary>
		public partial class Trainer
	{
		#region Variables
		public Pokemon[] Party { get; private set; }
		//ToDo: Add Trainer's Bag, NPCs can use items too.
		//public int ID { get; set; }
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
		/// The amount of money earned from defeating a trainer of this type. 
		/// The base money value is multiplied by the highest Level among all the trainer's 
		/// Pokémon to produce the actual amount of money gained (assuming no other modifiers). 
		/// Must be a number between 0 and 255.	
		/// Optional. If undefined, the default is 30.
		/// </summary>
		public byte BaseMoney { get; private set; }
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
		public int 	IntroME { get; private set; }
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
		public string PlayerID { get { return GetHashCode().ToString().Substring(GetHashCode().ToString().Length-6,GetHashCode().ToString().Length); } }
		public int TrainerID { get; private set; }
		public int SecretID { get; private set; }
		#endregion

		#region Wild Pokemon
		/// <summary>
		/// True is Double Battle, False is Single Battle, and Null is Wild Pokemon Encounter
		/// </summary>
		public bool? IsDouble { get; set; }
		public bool IsSwarm { get; private set; }
		#endregion

		#region NPC Details
		/// <summary>
		/// The skill level of all trainers of this type, used for battle AI. 
		/// Higher numbers represent higher skill levels. 
		/// Must be a number between 0 and 255.	
		/// Optional. If undefined, default is equal to the base money value.
		/// </summary>
		public byte	SkillLevel { get; private set; }
		/// <summary>
		/// A text field which can be used to modify the AI behaviour of all trainers of this type. 
		/// No such modifiers are defined by default, and there is no standard format. 
		/// See the page Battle AI for more details.
		/// Optional. If undefined, the default is blank.
		/// </summary>
		public int?	SkillCodes { get; private set; }
		/// <summary>
		/// After having defeated the trainer, when speaking to him/her again
		/// </summary>
		public string ScriptIdle { get; private set; }
		/// <summary>
		/// Start of trainer encounter
		/// </summary>
		public string ScriptBattleIntro { get; private set; }
		/// <summary>
		/// End of battle
		/// </summary>
		public string ScriptBattleEnd { get; private set; }
		#endregion

		#region Constructor
		/// <summary>
		/// Constructor used to create a Trainer class for Wild Pokemon Battles,
		/// since by default a trainer does not exist, but one is needed for battle to function
		/// </summary>
		/// <param name="wildPkmn"></param>
		/// <param name="isSwarm"></param>
		public Trainer(Pokemon[] wildPkmn, bool isSwarm = false) //: this (TrainerTypes.WildPokemon)
		{
			ID = TrainerTypes.WildPokemon;
			wildPkmn.PackParty();
			IsSwarm = isSwarm;
			if (wildPkmn.GetCount() > 1)
				IsDouble = true;
			else
				IsDouble = null;
		}

		public Trainer(TrainerTypes trainer)
		{
			TrainerID = Core.Rand.Next(1000000); //random number between 0 and 999999, including 0
			SecretID = Core.Rand.Next(1000000); //random number between 0 and 999999, including 0
			IsDouble = false;
			Party = new Pokemon[]
			{
				new Pokemon(Pokemons.NONE),
				new Pokemon(Pokemons.NONE),
				new Pokemon(Pokemons.NONE),
				new Pokemon(Pokemons.NONE),
				new Pokemon(Pokemons.NONE),
				new Pokemon(Pokemons.NONE)
			};
			GetTrainer(trainer);
		}

		public Trainer(TrainerTypes trainer, Pokemon[] party, int? intro = null, int? end = null, int? idle = null, byte? baseMoney = null, byte? skillLevel = null, int? battleMusic = null, int? victoryMusic = null, int? introMusic = null) : this(TrainerTypes.PLAYER)
		{
			Party = party;
			BaseMoney = baseMoney ?? BaseMoney;
			SkillLevel = skillLevel ?? SkillLevel;
			BattleBGM = battleMusic ?? BattleBGM;
			VictoryBGM = victoryMusic ?? VictoryBGM;
			IntroME = introMusic ?? IntroME;
			//ScriptIdle = 
			//ScriptBattleIntro = 
			//ScriptBattleEnd = 
		}

		public Trainer(Player trainer, /*string name, bool gender,* / Pokemon[] party = null, int? tID = null, int? sID = null) 
			: this(TrainerTypes.PLAYER, /*trainer.Trainer.Party* /party ?? new Pokemon[]
			{
				new Pokemon(Pokemons.NONE), new Pokemon(Pokemons.NONE), new Pokemon(Pokemons.NONE),
				new Pokemon(Pokemons.NONE), new Pokemon(Pokemons.NONE), new Pokemon(Pokemons.NONE)
			})
		{
			//if trainer is another player
			if (tID.HasValue) TrainerID = tID.Value; //trainer.Trainer.TrainerID;
			if (sID.HasValue) SecretID = sID.Value; //trainer.Trainer.SecretID;
			//Change name being loaded
			Name = trainer.Name; //name;
			//Load player's gender as well
			Gender = trainer.isMale; //gender;
		}
		#endregion

		void GetTrainer(TrainerTypes type)
		{
			//Using a switch class to make sure that no one is being left out
			#region Attempt1
			//foreach (NPC trainer in Types)
			//{
			//	if (trainer.ID == type)
			//		return trainer;
			//}
			#endregion
			#region Gender & IsDouble
			#endregion
			#region Base Money
			switch (type)
			{
				case TrainerTypes.TUBER_M:
				case TrainerTypes.TUBER2_M:
				case TrainerTypes.TUBER_F:
				case TrainerTypes.TUBER2_F:
					BaseMoney = 4;
					break;
				case TrainerTypes.RIVAL1:
				case TrainerTypes.BUGCATCHER:
				case TrainerTypes.PAINTER:
				case TrainerTypes.CAMPER:
				case TrainerTypes.LASS:
				case TrainerTypes.PICNICKER:
				case TrainerTypes.SISANDBRO:
				case TrainerTypes.SWIMMER_M:
				case TrainerTypes.SWIMMER_F:
				case TrainerTypes.SWIMMER2_M:
				case TrainerTypes.SWIMMER2_F:
				case TrainerTypes.YOUNGSTER:
					BaseMoney = 16;
					break;
				case TrainerTypes.CUEBALL:
				case TrainerTypes.CRUSHGIRL:
				case TrainerTypes.ROCKER:
				case TrainerTypes.TWINS:
					BaseMoney = 24;
					break;
				case TrainerTypes.AROMALADY:
				case TrainerTypes.BIKER:
				case TrainerTypes.BIRDKEEPER:
				case TrainerTypes.BLACKBELT:
				case TrainerTypes.CHANELLER:
				case TrainerTypes.FISHERMAN:
				case TrainerTypes.HIKER:
				case TrainerTypes.JUGGLER:
				case TrainerTypes.PSYCHIC_M:
				case TrainerTypes.PSYCHIC_F:
				case TrainerTypes.SAILOR:
				case TrainerTypes.TEAMROCKET_M:
				case TrainerTypes.TEAMROCKET_F:
					BaseMoney = 32;
					break;
				case TrainerTypes.RIVAL2:
					BaseMoney = 36;
					break;
				case TrainerTypes.CRUSHKIN:
				case TrainerTypes.TAMER:
					BaseMoney = 48;
					break;
				case TrainerTypes.ENGINEER:
				case TrainerTypes.POKEMONBREEDER:
				case TrainerTypes.RUINMANIAC:
				case TrainerTypes.SCIENTIST:
				case TrainerTypes.SUPERNERD:
					BaseMoney = 48;
					break;
				case TrainerTypes.BEAUTY:
					BaseMoney = 56;
					break;
				case TrainerTypes.PLAYER:
				case TrainerTypes.POKEMONTRAINER_Red:
				case TrainerTypes.POKEMONTRAINER_Brendan:
				case TrainerTypes.POKEMONTRAINER_Leaf:
				case TrainerTypes.POKEMONTRAINER_May:
				case TrainerTypes.COOLTRAINER_M:
				case TrainerTypes.COOLTRAINER_F:
				case TrainerTypes.POKEMONRANGER_M:
				case TrainerTypes.POKEMONRANGER_F:
				case TrainerTypes.YOUNGCOUPLE:
					BaseMoney = 60;
					break;
				case TrainerTypes.POKEMANIAC:
					BaseMoney = 64;
					break;
				case TrainerTypes.COOLCOUPLE:
				case TrainerTypes.GAMBLER:
				case TrainerTypes.GENTLEMAN:
					BaseMoney = 72;
					break;
				case TrainerTypes.BURGLAR:
					BaseMoney = 88;
					break;
				case TrainerTypes.CHAMPION:
				case TrainerTypes.LEADER_Brock:
				case TrainerTypes.LEADER_Surge:
				case TrainerTypes.LEADER_Koga:
				case TrainerTypes.LEADER_Blaine:
				case TrainerTypes.LEADER_Giovanni:
				case TrainerTypes.LEADER_Misty:
				case TrainerTypes.LEADER_Erika:
				case TrainerTypes.LEADER_Sabrina:
				case TrainerTypes.ELITEFOUR_Lorelei:
				case TrainerTypes.ELITEFOUR_Agatha:
				case TrainerTypes.ELITEFOUR_Bruno:
				case TrainerTypes.ELITEFOUR_Lance:
				case TrainerTypes.ROCKETBOSS:
				case TrainerTypes.PROFESSOR:
					BaseMoney = 100;
					break;
				case TrainerTypes.LADY:
					BaseMoney = 160;
					break;
				default:
					BaseMoney = 30;
					break;
			}
			#endregion
			#region Skill Level
			switch (type)
			{
				case TrainerTypes.BURGLAR:
				case TrainerTypes.GAMBLER:
					SkillLevel = 32;
					break;
				case TrainerTypes.LADY:
					SkillLevel = 72;
					break;
				case TrainerTypes.SWIMMER_M:
				case TrainerTypes.SWIMMER_F:
				case TrainerTypes.SWIMMER2_M:
				case TrainerTypes.SWIMMER2_F:
					SkillLevel = 32;
					break;
				case TrainerTypes.TUBER_M:
				case TrainerTypes.TUBER_F:
				case TrainerTypes.TUBER2_M:
				case TrainerTypes.TUBER2_F:
					SkillLevel = 16;
					break;
				case TrainerTypes.COOLCOUPLE:
				case TrainerTypes.SISANDBRO:
					SkillLevel = 48;
					break;
				case TrainerTypes.YOUNGCOUPLE:
					SkillLevel = 32;
					break;
				default:
					SkillLevel = BaseMoney;
					break;
			}
			#endregion
		}

		#region Party
		//This isnt needed, there's one already in battle class
		//public PokemonUnity.Battle.Pokemon[] GetBattleParty()
		//public PokemonUnity.Battle.Pokemon[] BattleParty
		//{
		//	get
		//	{
		//		return new PokemonUnity.Battle.Pokemon[] {
		//			//new PokemonUnity.Battle.Pokemon(Party[0])
		//		};
		//	}
		//}
		#endregion

		#region Explicit Operators
		public static bool operator == (Trainer t1, Trainer t2)
		{
			return ((t1.Gender == t2.Gender) && (t1.TrainerID == t2.TrainerID) && (t1.SecretID == t2.SecretID)) & (t1.Name == t2.Name);
		}
		public static bool operator != (Trainer t1, Trainer t2)
		{
			return ((t1.Gender != t2.Gender) || (t1.TrainerID != t2.TrainerID) || (t1.SecretID != t2.SecretID)) | (t1.Name == t2.Name);
		}
		public bool Equals(Player obj)
		{
			return this == obj.Trainer; //Equals(obj.Trainer);
		}
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}
		public override int GetHashCode()
		{
			return TrainerID + SecretID * 65536;
		}
		#endregion
	}*/

	public static class PokemonPartyExtension
	{
		public static void PackParty(this Combat.Pokemon[] Party)
		{
			Combat.Pokemon[] packedArray = new Combat.Pokemon[Party.Length];
			int i2 = 0; //counter for packed array
			for (int i = 0; i < Party.Length; i++)
			{
				if (Party[i].IsNotNullOrNone())// != null || Party[i].Species != Pokemons.NONE)
				{
					//if next object in box has a value
					packedArray[i2] = Party[i]; //add to packed array
					i2 += 1; //ready packed array's next position
				}
			}
			for (int i = 0; i < Party.Length; i++)
				Party[i] = packedArray[i];
		}
		public static void PackParty(this Pokemon[] Party)
		{
			Pokemon[] packedArray = new Pokemon[Party.Length];
			int i2 = 0; //counter for packed array
			for (int i = 0; i < Party.Length; i++)
			{
				if (Party[i].IsNotNullOrNone())// != null || Party[i].Species != Pokemons.NONE)
				{
					//if next object in box has a value
					packedArray[i2] = Party[i]; //add to packed array
					i2 += 1; //ready packed array's next position
				}
			}
			for (int i = 0; i < Party.Length; i++)
				Party[i] = packedArray[i];
		}
		public static bool HasSpace(this Pokemon[] partyOrPC, int limit)
		{
			if (partyOrPC.GetCount() < limit) return true; //partyOrPC.GetCount().HasValue &&
			else return false;
		}

		public static int GetCount(this Pokemon[] partyOrPC)
		{
			int result = 0;
			for (int i = 0; i < partyOrPC.Length; i++)
			{
				if (partyOrPC[i].IsNotNullOrNone())// != null || partyOrPC[i].Species != Pokemons.NONE)
				{
					result += 1;
				}
			}
			return result;
		}
	}
}