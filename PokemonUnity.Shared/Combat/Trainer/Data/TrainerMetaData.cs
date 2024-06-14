using PokemonEssentials.Interface.PokeBattle;
using System;
using System.Collections;
using System.Collections.Generic;

namespace PokemonUnity.Character
{
	public interface ITrainerChallengeData
	{
		/// <summary>
		/// Name of the file which lists all possible trainers in this set.
		/// </summary>
		string TrainerFile				{ get; set; }
		/// <summary>
		/// Name of the file which lists all possible Pokemon in this set.
		/// </summary>
		string PokemonFile				{ get; set; }
		/// <summary>
		/// The names of one or more challenges that the set of Pokémon/trainers will be used for.
		/// </summary>
		/// <remarks>
		/// Should not be defined if the section is <see cref="IsDefault"/>
		/// </remarks>
		/// Should not be defined if the section is [DefaultTrainerList].
		ITrainerData Challenges			{ get; set; }
		IList<ITrainerData> Trainers	{ get; set; }
		IList<IPokemon> Pokemons		{ get; set; }
		HashSet<int> Tags				{ get; set; }
		bool IsDefault					{ get; set; }
	}

	public interface ITrainerData
	{
		/// <summary>
		/// This is how the scripts refer to the trainer type.
		/// Typically this is the trainer type's name,
		/// but written in all capital letters and with no spaces or symbols.
		/// The internal name is never seen by the player.
		/// </summary>
		TrainerTypes ID { get; }
		/// <summary>
		/// A set of Pokémon that could be used by the trainer, <see cref="Kernal.TrainerPartyData"/>.
		/// </summary>
		/// <remarks>
		/// Each number points to one specific Pokémon in the associated Pokémon file for the same battle challenge.
		/// </remarks>
		IList<int> PokemonNos { get; }
		/// <summary>
		/// End of battle
		/// </summary>
		/// <remarks>
		/// [Win, Loss]
		/// </remarks>
		string[] ScriptBattleEnd { get; }
		/// <summary>
		/// Start of trainer encounter
		/// </summary>
		string ScriptBattleIntro { get; }
		/// <summary>
		/// After having defeated the trainer, when speaking to him/her again
		/// </summary>
		string ScriptIdle { get; }
		/// <summary>
		/// A text field which can be used to modify the AI behavior of all trainers of this type.
		/// No such modifiers are defined by default, and there is no standard format.
		/// See the page Battle AI for more details.
		/// Optional. If undefined, the default is blank.
		/// </summary>
		int? SkillCodes { get; }
		/// <summary>
		/// The skill level of all trainers of this type, used for battle AI.
		/// Higher numbers represent higher skill levels.
		/// Must be a number between 0 and 255.
		/// Optional. If undefined, default is equal to the base money value.
		/// </summary>
		byte SkillLevel { get; }
	}

	/// <summary>
	/// </summary>
	/// Maybe separate double, music, script to npc from trainer data?
	public struct TrainerMetaData : ITrainerData
	{
		#region Variables
		/// <summary>
		/// This is how the scripts refer to the trainer type.
		/// Typically this is the trainer type's name,
		/// but written in all capital letters and with no spaces or symbols.
		/// The internal name is never seen by the player.
		/// </summary>
		public TrainerTypes ID { get; private set; }
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
		public string BattleBGM { get; private set; }
		/// <summary>
		/// The name of a background music (BGM) file in the folder "Audio/BGM".
		/// The victory background music that plays upon defeat of trainers of this type.
		/// Optional. If undefined, the default victory BGM is used.
		/// </summary>
		public string VictoryBGM { get; private set; }
		/// <summary>
		/// The name of a music effect (ME) file in the folder "Audio/ME".
		/// The music that plays before the battle begins, while still talking to the trainer.
		/// Optional. If undefined, the default ME is used.
		/// </summary>
		public string IntroME { get; private set; }
		/// <summary>
		/// The gender of all trainers of this type. Is one of:
		/// Male, Female, Mixed(i.e. if the type shows a pair of trainers)
		/// Optional. If undefined, the default is "Mixed".
		/// </summary>
		public bool? Gender { get; private set; }
		/// <summary>
		/// Two trainer avatars counted as a single NPC.
		/// </summary>
		public bool Double { get; private set; }
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
		/// A text field which can be used to modify the AI behavior of all trainers of this type.
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
		/// <remarks>
		/// [Win, Loss]
		/// </remarks>
		public string[] ScriptBattleEnd { get; private set; }
		/// <summary>
		/// A set of Pokémon that could be used by the trainer, <see cref="Kernal.TrainerPartyData"/>.
		/// </summary>
		/// <remarks>
		/// Each number points to one specific Pokémon in the associated Pokémon file for the same battle challenge.
		/// </remarks>
		public IList<int> PokemonNos { get; private set; }
		#endregion

		public TrainerMetaData(TrainerTypes id, bool @double, byte baseMoney = 0, bool? gender = null, byte skillLevel = 0, int? skillCodes = null, string introME = null, string battleBGM = null, string victoryBGM = null, string scriptIdle = null, string scriptBattleIntro = null, string[] scriptBattleEnd = null)
		{
			ID = id;
			Double = @double;
			BaseMoney = baseMoney;
			Gender = gender;
			SkillLevel = skillLevel;
			SkillCodes = skillCodes;
			IntroME = introME;
			BattleBGM = battleBGM;
			VictoryBGM = victoryBGM;
			ScriptIdle = scriptIdle;
			ScriptBattleIntro = scriptBattleIntro;
			ScriptBattleEnd = scriptBattleEnd;
			PokemonNos = new List<int>();
		}
	}
}