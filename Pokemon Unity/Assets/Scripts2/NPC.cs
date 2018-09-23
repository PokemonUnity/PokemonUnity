using System.Collections;
using PokemonUnity;

/// <summary>
/// It is important to note that the player also has a trainer type, 
/// and it is defined in exactly the same way as any other trainer type. 
/// </summary>
public class NPC
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
	public int 	BaseMoney { get; set; }
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
	public int?	Gender { get; set; }
	/// <summary>
	/// The skill level of all trainers of this type, used for battle AI. 
	/// Higher numbers represent higher skill levels. 
	/// Must be a number between 0 and 255.	
	/// Optional. If undefined, default is equal to the base money value.
	/// </summary>
	public int 	SkillLevel { get; set; }
	/// <summary>
	/// A text field which can be used to modify the AI behaviour of all trainers of this type. 
	/// No such modifiers are defined by default, and there is no standard format. 
	/// See the page Battle AI for more details.
	/// Optional. If undefined, the default is blank.
	/// </summary>
	public int 	SkillCodes { get; set; }

	public NPC()
    {
    }

    public static NPC[] Types
	{
		get
		{
			return new NPC[]
			{
			};
		}
	}
}