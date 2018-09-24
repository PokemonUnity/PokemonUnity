using System.Collections;
using PokemonUnity;

/// <summary>
/// It is important to note that the player also has a trainer type, 
/// and it is defined in exactly the same way as any other trainer type. 
/// </summary>
public class Trainer
{
	//public int ID { get; set; }
	/// <summary>
	/// This is how the scripts refer to the trainer type. 
	/// Typically this is the trainer type's name, 
	/// but written in all capital letters and with no spaces or symbols. 
	/// The internal name is never seen by the player.
	/// </summary>
	public TrainerTypes ID { get; set; }
	/// <summary>
	/// The name of the trainer type, as seen by the player. 
	/// Multiple trainer types can have the same display name, 
	/// although they cannot share ID numbers or internal names.
	/// </summary>
	public string Name { get; set; }
	/// <summary>
	/// The amount of money earned from defeating a trainer of this type. 
	/// The base money value is multiplied by the highest Level among all the trainer's 
	/// Pokémon to produce the actual amount of money gained (assuming no other modifiers). 
	/// Must be a number between 0 and 255.	
	/// Optional. If undefined, the default is 30.
	/// </summary>
	public byte BaseMoney { get; set; }
	/// <summary>
	/// The name of a background music (BGM) file in the folder "Audio/BGM". 
	/// The music that plays during battles against trainers of this type. 
	/// Typically only defined for Gym Leaders, Elite Four members and rivals.	
	/// Optional. If undefined, the default BGM is used.
	/// </summary>
	public int 	BattleBGM { get; set; }
	/// <summary>
	/// The name of a background music (BGM) file in the folder "Audio/BGM". 
	/// The victory background music that plays upon defeat of trainers of this type.	
	/// Optional. If undefined, the default victory BGM is used.
	/// </summary>
	public int 	VictoryBGM { get; set; }
	/// <summary>
	/// The name of a music effect (ME) file in the folder "Audio/ME". 
	/// The music that plays before the battle begins, while still talking to the trainer.	
	/// Optional. If undefined, the default ME is used.
	/// </summary>
	public int 	IntroME { get; set; }
	/// <summary>
	/// The gender of all trainers of this type. Is one of:
	/// Male, Female, Mixed(i.e. if the type shows a pair of trainers)
	/// Optional. If undefined, the default is "Mixed".
	/// </summary>
	public bool?	Gender { get; set; }
	/// <summary>
	/// The skill level of all trainers of this type, used for battle AI. 
	/// Higher numbers represent higher skill levels. 
	/// Must be a number between 0 and 255.	
	/// Optional. If undefined, default is equal to the base money value.
	/// </summary>
	public byte	SkillLevel { get; set; }
	/// <summary>
	/// A text field which can be used to modify the AI behaviour of all trainers of this type. 
	/// No such modifiers are defined by default, and there is no standard format. 
	/// See the page Battle AI for more details.
	/// Optional. If undefined, the default is blank.
	/// </summary>
	public int?	SkillCodes { get; set; }

	public Trainer(TrainerTypes trainer)
    {
		GetTrainer(trainer);
		//if trainer is another player
		//Change name being loaded
    }

	public Trainer(Player trainer) : this(TrainerTypes.PLAYER)
    {
    }

	void GetTrainer(TrainerTypes type)
	{
		//Using a switch class to make sure that no one is being left out
		#region Attempt1
		/*foreach (NPC trainer in Types)
		{
			if (trainer.ID == type)
				return trainer;
		}*/
		#endregion
		#region Gender
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
				break;
		}
		#endregion
	}

	/// <summary>
	/// List of all the NPC trainers that will be used throughout the game.
	/// NPCs should be identified by their Array[Index] when programming.
	/// </summary>
	public static Trainer[] Database
	{
		get
		{
			return new Trainer[]
			{
			};
		}
	}
}
namespace PokemonUnity
{
	public enum TrainerTypes
	{
		/// <summary>
		/// Custom designs or just generic character played by another user
		/// </summary>
		PLAYER,
		POKEMONTRAINER_Red,
		POKEMONTRAINER_Leaf,
		POKEMONTRAINER_Brendan,
		POKEMONTRAINER_May,
		RIVAL1,
		RIVAL2,
		AROMALADY,
		BEAUTY,
		BIKER,
		BIRDKEEPER,
		BUGCATCHER,
		BURGLAR,
		CHANELLER,
		CUEBALL,
		ENGINEER,
		FISHERMAN,
		GAMBLER,
		GENTLEMAN,
		HIKER,
		JUGGLER,
		LADY,
		PAINTER,
		POKEMANIAC,
		POKEMONBREEDER,
		PROFESSOR,
		ROCKER,
		RUINMANIAC,
		SAILOR,
		SCIENTIST,
		SUPERNERD,
		TAMER,
		BLACKBELT,
		CRUSHGIRL,
		CAMPER,
		PICNICKER,
		COOLTRAINER_M,
		COOLTRAINER_F,
		YOUNGSTER,
		LASS,
		POKEMONRANGER_M,
		POKEMONRANGER_F,
		PSYCHIC_M,
		PSYCHIC_F,
		SWIMMER_M,
		SWIMMER_F,
		SWIMMER2_M,
		SWIMMER2_F,
		TUBER_M,
		TUBER_F,
		TUBER2_M,
		TUBER2_F,
		COOLCOUPLE,
		CRUSHKIN,
		SISANDBRO,
		TWINS,
		YOUNGCOUPLE,
		TEAMROCKET_M,
		TEAMROCKET_F,
		ROCKETBOSS,
		LEADER_Brock,
		LEADER_Misty,
		LEADER_Surge,
		LEADER_Erika,
		LEADER_Koga,
		LEADER_Sabrina,
		LEADER_Blaine,
		LEADER_Giovanni,
		ELITEFOUR_Lorelei,
		ELITEFOUR_Bruno,
		ELITEFOUR_Agatha,
		ELITEFOUR_Lance,
		CHAMPION
	}
}