namespace PokemonUnity.Shared.Enums
{
	public class TrainerTypes : Enumeration
	{
		#region Variables
		/// <summary>
		/// The amount of money earned from defeating a trainer of this type. 
		/// The base money value is multiplied by the highest Level among all the trainer's 
		/// Pokémon to produce the actual amount of money gained (assuming no other modifiers). 
		/// Must be a number between 0 and 255.	
		/// Optional. If undefined, the default is 30.
		/// </summary>
		//https://bulbapedia.bulbagarden.net/wiki/Prize_money
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
		#endregion

		#region NPC Details
		public bool Double { get; private set; }
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
		public override string Name {  get { return "Trainer_Type_{0}"; } }
		protected TrainerTypes(int id, string name, bool @double, byte baseMoney = 0, bool? gender = null, byte skillLevel = 0, int? skillCodes = null, string introME = null, string battleBGM = null, string victoryBGM = null, string scriptIdle = null, string scriptBattleIntro = null, string scriptBattleEnd = null) : base(id, name)
		{
			//Id = id;
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
		}
		public static readonly TrainerTypes WildPokemon					= new TrainerTypes(-1,	"WildPokemon"				, @double: false,	baseMoney: 0,	gender: null,	skillLevel: 0,	skillCodes: null);
		/// <summary>
		/// Custom designs or just generic character played by another user
		/// </summary>
		public static readonly TrainerTypes PLAYER						= new TrainerTypes(0,	"PLAYER"					, @double: false,	baseMoney: 30,	gender: null,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes PLAYER_M					= new TrainerTypes(0,	"PLAYER_M"					, @double: false,	baseMoney: 30,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes PLAYER_F					= new TrainerTypes(0,	"PLAYER_F"					, @double: false,	baseMoney: 30,	gender: false,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes POKEMONTRAINER_Red			= new TrainerTypes(0,	"POKEMONTRAINER_Red"		, @double: false,	baseMoney: 30,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes POKEMONTRAINER_Leaf			= new TrainerTypes(0,	"POKEMONTRAINER_Leaf"		, @double: false,	baseMoney: 30,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes POKEMONTRAINER_Brendan		= new TrainerTypes(0,	"POKEMONTRAINER_Brendan"	, @double: false,	baseMoney: 30,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes POKEMONTRAINER_May			= new TrainerTypes(0,	"POKEMONTRAINER_May"		, @double: false,	baseMoney: 30,	gender: false,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes RIVAL1						= new TrainerTypes(0,	"RIVAL1"					, @double: false,	baseMoney: 16,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes RIVAL2						= new TrainerTypes(0,	"RIVAL2"					, @double: false,	baseMoney: 36,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes AROMALADY					= new TrainerTypes(0,	"AROMALADY"					, @double: false,	baseMoney: 32,	gender: false,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes BEAUTY						= new TrainerTypes(0,	"BEAUTY"					, @double: false,	baseMoney: 30,	gender: false,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes BIKER						= new TrainerTypes(0,	"BIKER"						, @double: false,	baseMoney: 32,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes BIRDKEEPER					= new TrainerTypes(0,	"BIRDKEEPER"				, @double: false,	baseMoney: 32,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes BUGCATCHER					= new TrainerTypes(0,	"BUGCATCHER"				, @double: false,	baseMoney: 16,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes BURGLAR						= new TrainerTypes(0,	"BURGLAR"					, @double: false,	baseMoney: 30,	gender: true,	skillLevel: 32,	skillCodes: null);
		public static readonly TrainerTypes CHANELLER					= new TrainerTypes(0,	"CHANELLER"					, @double: false,	baseMoney: 32,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes CUEBALL						= new TrainerTypes(0,	"CUEBALL"					, @double: false,	baseMoney: 24,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes ENGINEER					= new TrainerTypes(0,	"ENGINEER"					, @double: false,	baseMoney: 30,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes FISHERMAN					= new TrainerTypes(0,	"FISHERMAN"					, @double: false,	baseMoney: 32,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes GAMBLER						= new TrainerTypes(0,	"GAMBLER"					, @double: false,	baseMoney: 30,	gender: true,	skillLevel: 32,	skillCodes: null);
		public static readonly TrainerTypes GENTLEMAN					= new TrainerTypes(0,	"GENTLEMAN"					, @double: false,	baseMoney: 30,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes HIKER						= new TrainerTypes(0,	"HIKER"						, @double: false,	baseMoney: 32,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes JUGGLER						= new TrainerTypes(0,	"JUGGLER"					, @double: false,	baseMoney: 32,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes LADY						= new TrainerTypes(0,	"LADY"						, @double: false,	baseMoney: 30,	gender: false,	skillLevel: 72,	skillCodes: null);
		public static readonly TrainerTypes PAINTER						= new TrainerTypes(0,	"PAINTER"					, @double: false,	baseMoney: 16,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes POKEMANIAC					= new TrainerTypes(0,	"POKEMANIAC"				, @double: false,	baseMoney: 30,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes POKEMONBREEDER				= new TrainerTypes(0,	"POKEMONBREEDER"			, @double: false,	baseMoney: 30,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes PROFESSOR					= new TrainerTypes(0,	"PROFESSOR"					, @double: false,	baseMoney: 30,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes ROCKER						= new TrainerTypes(0,	"ROCKER"					, @double: false,	baseMoney: 24,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes RUINMANIAC					= new TrainerTypes(0,	"RUINMANIAC"				, @double: false,	baseMoney: 30,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes SAILOR						= new TrainerTypes(0,	"SAILOR"					, @double: false,	baseMoney: 32,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes SCIENTIST					= new TrainerTypes(0,	"SCIENTIST"					, @double: false,	baseMoney: 30,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes SUPERNERD					= new TrainerTypes(0,	"SUPERNERD"					, @double: false,	baseMoney: 30,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes TAMER						= new TrainerTypes(0,	"TAMER"						, @double: false,	baseMoney: 48,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes BLACKBELT					= new TrainerTypes(0,	"BLACKBELT"					, @double: false,	baseMoney: 32,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes CRUSHGIRL					= new TrainerTypes(0,	"CRUSHGIRL"					, @double: false,	baseMoney: 24,	gender: false,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes CAMPER						= new TrainerTypes(0,	"CAMPER"					, @double: false,	baseMoney: 16,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes PICNICKER					= new TrainerTypes(0,	"PICNICKER"					, @double: false,	baseMoney: 16,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes COOLTRAINER_M				= new TrainerTypes(0,	"COOLTRAINER_M"				, @double: false,	baseMoney: 30,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes COOLTRAINER_F				= new TrainerTypes(0,	"COOLTRAINER_F"				, @double: false,	baseMoney: 30,	gender: false,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes YOUNGSTER					= new TrainerTypes(0,	"YOUNGSTER"					, @double: false,	baseMoney: 16,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes LASS						= new TrainerTypes(0,	"LASS"						, @double: false,	baseMoney: 16,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes POKEMONRANGER_M				= new TrainerTypes(0,	"POKEMONRANGER_M"			, @double: false,	baseMoney: 30,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes POKEMONRANGER_F				= new TrainerTypes(0,	"POKEMONRANGER_F"			, @double: false,	baseMoney: 30,	gender: false,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes PSYCHIC_M					= new TrainerTypes(0,	"PSYCHIC_M"					, @double: false,	baseMoney: 32,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes PSYCHIC_F					= new TrainerTypes(0,	"PSYCHIC_F"					, @double: false,	baseMoney: 32,	gender: false,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes SWIMMER_M					= new TrainerTypes(0,	"SWIMMER_M"					, @double: false,	baseMoney: 16,	gender: true,	skillLevel: 32,	skillCodes: null);
		public static readonly TrainerTypes SWIMMER_F					= new TrainerTypes(0,	"SWIMMER_F"					, @double: false,	baseMoney: 16,	gender: false,	skillLevel: 32,	skillCodes: null);
		public static readonly TrainerTypes SWIMMER2_M					= new TrainerTypes(0,	"SWIMMER2_M"				, @double: false,	baseMoney: 16,	gender: true,	skillLevel: 32,	skillCodes: null);
		public static readonly TrainerTypes SWIMMER2_F					= new TrainerTypes(0,	"SWIMMER2_F"				, @double: false,	baseMoney: 16,	gender: false,	skillLevel: 32,	skillCodes: null);
		public static readonly TrainerTypes TUBER_M						= new TrainerTypes(0,	"TUBER_M"					, @double: false,	baseMoney: 4,	gender: true,	skillLevel: 16,	skillCodes: null);
		public static readonly TrainerTypes TUBER_F						= new TrainerTypes(0,	"TUBER_F"					, @double: false,	baseMoney: 4,	gender: false,	skillLevel: 16,	skillCodes: null);
		public static readonly TrainerTypes TUBER2_M					= new TrainerTypes(0,	"TUBER2_M"					, @double: false,	baseMoney: 4,	gender: true,	skillLevel: 16,	skillCodes: null);
		public static readonly TrainerTypes TUBER2_F					= new TrainerTypes(0,	"TUBER2_F"					, @double: false,	baseMoney: 4,	gender: false,	skillLevel: 16,	skillCodes: null);
		public static readonly TrainerTypes COOLCOUPLE					= new TrainerTypes(0,	"COOLCOUPLE"				, @double: true,	baseMoney: 30,	gender: null,	skillLevel: 48,	skillCodes: null);
		public static readonly TrainerTypes CRUSHKIN					= new TrainerTypes(0,	"CRUSHKIN"					, @double: false,	baseMoney: 48,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes SISANDBRO					= new TrainerTypes(0,	"SISANDBRO"					, @double: true,	baseMoney: 16,	gender: true,	skillLevel: 48,	skillCodes: null);
		public static readonly TrainerTypes TWINS						= new TrainerTypes(0,	"TWINS"						, @double: true,	baseMoney: 24,	gender: null,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes YOUNGCOUPLE					= new TrainerTypes(0,	"YOUNGCOUPLE"				, @double: true,	baseMoney: 30,	gender: null,	skillLevel: 32,	skillCodes: null);
		public static readonly TrainerTypes TEAMROCKET_M				= new TrainerTypes(0,	"TEAMROCKET_M"				, @double: false,	baseMoney: 32,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes TEAMROCKET_F				= new TrainerTypes(0,	"TEAMROCKET_F"				, @double: false,	baseMoney: 32,	gender: false,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes ROCKETBOSS					= new TrainerTypes(0,	"ROCKETBOSS"				, @double: false,	baseMoney: 30,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes LEADER_Brock				= new TrainerTypes(0,	"LEADER_Brock"				, @double: false,	baseMoney: 30,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes LEADER_Misty				= new TrainerTypes(0,	"LEADER_Misty"				, @double: false,	baseMoney: 30,	gender: false,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes LEADER_Surge				= new TrainerTypes(0,	"LEADER_Surge"				, @double: false,	baseMoney: 30,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes LEADER_Erika				= new TrainerTypes(0,	"LEADER_Erika"				, @double: false,	baseMoney: 30,	gender: false,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes LEADER_Koga					= new TrainerTypes(0,	"LEADER_Koga"				, @double: false,	baseMoney: 30,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes LEADER_Sabrina				= new TrainerTypes(0,	"LEADER_Sabrina"			, @double: false,	baseMoney: 30,	gender: false,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes LEADER_Blaine				= new TrainerTypes(0,	"LEADER_Blaine"				, @double: false,	baseMoney: 30,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes LEADER_Giovanni				= new TrainerTypes(0,	"LEADER_Giovanni"			, @double: false,	baseMoney: 30,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes ELITEFOUR_Lorelei			= new TrainerTypes(0,	"ELITEFOUR_Lorelei"			, @double: false,	baseMoney: 30,	gender: false,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes ELITEFOUR_Bruno				= new TrainerTypes(0,	"ELITEFOUR_Bruno"			, @double: false,	baseMoney: 30,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes ELITEFOUR_Agatha			= new TrainerTypes(0,	"ELITEFOUR_Agatha"			, @double: false,	baseMoney: 30,	gender: false,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes ELITEFOUR_Lance				= new TrainerTypes(0,	"ELITEFOUR_Lance"			, @double: false,	baseMoney: 30,	gender: true,	skillLevel: 1,	skillCodes: null);
		public static readonly TrainerTypes CHAMPION					= new TrainerTypes(0,	"CHAMPION"					, @double: false,	baseMoney: 30,	gender: true,	skillLevel: 1,	skillCodes: null);
	}
}