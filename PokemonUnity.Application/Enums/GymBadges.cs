namespace PokemonUnity.Enums
{
	/// <summary>
	/// Whatever gym badges you want to incorporate throughout your game.
	/// This is where the ID of the badges are stored, and the game can
	/// validate certain actions based on said ID or # of badges...
	/// </summary>
	public class GymBadges : PokemonUnity.Shared.Enums.GymBadges
	{
		public int RegionId { get; private set; }
		/// <summary>
		/// Pokemons below this level will not disobey Trainer commands
		/// </summary>
		public int Level { get; private set; }
		protected GymBadges(int id, string name) : base(id, name) { }
		public static readonly GymBadges Boulder		= new GymBadges(0,	"Gym_Badge_Boulder"); //Rock
		public static readonly GymBadges Cascade		= new GymBadges(0,	"Gym_Badge_Cascade"); // Water
		public static readonly GymBadges Thunder		= new GymBadges(0,	"Gym_Badge_Thunder");
		public static readonly GymBadges Rainbow		= new GymBadges(0,	"Gym_Badge_Rainbow");
		public static readonly GymBadges Soul			= new GymBadges(0,	"Gym_Badge_Soul");
		public static readonly GymBadges Marsh			= new GymBadges(0,	"Gym_Badge_Marsh");
		public static readonly GymBadges Volcano		= new GymBadges(0,	"Gym_Badge_Volcano");
		public static readonly GymBadges Earth			= new GymBadges(0,	"Gym_Badge_Earth");
																			 
		public static readonly GymBadges Zephyr			= new GymBadges(0,	"Gym_Badge_Zephyr");
		public static readonly GymBadges Hive			= new GymBadges(0,	"Gym_Badge_Hive");
		public static readonly GymBadges Plain			= new GymBadges(0,	"Gym_Badge_Plain");
		public static readonly GymBadges Fog			= new GymBadges(0,	"Gym_Badge_Fog");
		public static readonly GymBadges Storm			= new GymBadges(0,	"Gym_Badge_Storm");
		public static readonly GymBadges Mineral		= new GymBadges(0,	"Gym_Badge_Mineral");
		public static readonly GymBadges Glacier		= new GymBadges(0,	"Gym_Badge_Glacier");
		public static readonly GymBadges Rising			= new GymBadges(0,	"Gym_Badge_Rising");
																			 
		public static readonly GymBadges Stone			= new GymBadges(0,	"Gym_Badge_Stone");
		public static readonly GymBadges Knuckle		= new GymBadges(0,	"Gym_Badge_Knuckle");
		public static readonly GymBadges Dynamo			= new GymBadges(0,	"Gym_Badge_Dynamo");
		public static readonly GymBadges Heat			= new GymBadges(0,	"Gym_Badge_Heat");
		public static readonly GymBadges Balance		= new GymBadges(0,	"Gym_Badge_Balance");
		public static readonly GymBadges Feather		= new GymBadges(0,	"Gym_Badge_Feather");
		public static readonly GymBadges Mind			= new GymBadges(0,	"Gym_Badge_Mind");
		public static readonly GymBadges Rain			= new GymBadges(0,	"Gym_Badge_Rain");
																			 
		public static readonly GymBadges Coal			= new GymBadges(0,	"Gym_Badge_Coal");
		public static readonly GymBadges Forest			= new GymBadges(0,	"Gym_Badge_Forest");
		public static readonly GymBadges Cobble			= new GymBadges(0,	"Gym_Badge_Cobble");
		public static readonly GymBadges Fen			= new GymBadges(0,	"Gym_Badge_Fen");
		public static readonly GymBadges Relic			= new GymBadges(0,	"Gym_Badge_Relic");
		public static readonly GymBadges Mine			= new GymBadges(0,	"Gym_Badge_Mine");
		public static readonly GymBadges Icicle			= new GymBadges(0,	"Gym_Badge_Icicle");
		public static readonly GymBadges Beacon			= new GymBadges(0,	"Gym_Badge_Beacon");
																			 
		public static readonly GymBadges Trio			= new GymBadges(0,	"Gym_Badge_Trio");
		public static readonly GymBadges Basic			= new GymBadges(0,	"Gym_Badge_Basic");
		public static readonly GymBadges Insect			= new GymBadges(0,	"Gym_Badge_Insect");
		public static readonly GymBadges Bolt			= new GymBadges(0,	"Gym_Badge_Bolt");
		public static readonly GymBadges Quake			= new GymBadges(0,	"Gym_Badge_Quake");
		public static readonly GymBadges Jet			= new GymBadges(0,	"Gym_Badge_Jet");
		public static readonly GymBadges Freeze			= new GymBadges(0,	"Gym_Badge_Freeze");
		public static readonly GymBadges Legend			= new GymBadges(0,	"Gym_Badge_Legend");
																			 
		public static readonly GymBadges Toxic			= new GymBadges(0,	"Gym_Badge_Toxic");
		public static readonly GymBadges Wave			= new GymBadges(0,	"Gym_Badge_Wave");
																			 
		public static readonly GymBadges Bug			= new GymBadges(0,	"Gym_Badge_Bug");
		public static readonly GymBadges Cliff			= new GymBadges(0,	"Gym_Badge_Cliff");
		public static readonly GymBadges Rumble			= new GymBadges(0,	"Gym_Badge_Rumble");
		public static readonly GymBadges Plant			= new GymBadges(0,	"Gym_Badge_Plant");
		public static readonly GymBadges Voltage		= new GymBadges(0,	"Gym_Badge_Voltage");
		public static readonly GymBadges Fairy			= new GymBadges(0,	"Gym_Badge_Fairy");
		public static readonly GymBadges Psychic		= new GymBadges(0,	"Gym_Badge_Psychic");
		public static readonly GymBadges Iceberg		= new GymBadges(0,	"Gym_Badge_Iceberg");
																			 
		public static readonly GymBadges Grass			= new GymBadges(0,	"Gym_Badge_Grass");
		public static readonly GymBadges Water			= new GymBadges(0,	"Gym_Badge_Water");
		public static readonly GymBadges Fire			= new GymBadges(0,	"Gym_Badge_Fire");
		public static readonly GymBadges Fighting		= new GymBadges(0,	"Gym_Badge_Fighting");
		public static readonly GymBadges Ghost			= new GymBadges(0,	"Gym_Badge_Ghost");
		public static readonly GymBadges Fairy_2		= new GymBadges(0,	"Gym_Badge_Fairy_2");
		public static readonly GymBadges Rock			= new GymBadges(0,	"Gym_Badge_Rock");
		public static readonly GymBadges Ice			= new GymBadges(0,	"Gym_Badge_Ice");
		public static readonly GymBadges Dark			= new GymBadges(0,	"Gym_Badge_Dark");
		public static readonly GymBadges Dragon			= new GymBadges(0,	"Gym_Badge_Dragon");
																			 
		public static readonly GymBadges Coral_Eye		= new GymBadges(0,	"Gym_Badge_Coral_Eye");
		public static readonly GymBadges Sea_Ruby		= new GymBadges(0,	"Gym_Badge_Sea_Ruby");
		public static readonly GymBadges Spike_Shell	= new GymBadges(0,	"Gym_Badge_Spike_Shell");
		public static readonly GymBadges Jade_Star		= new GymBadges(0,	"Gym_Badge_Jade_Star");
																			 
		public static readonly GymBadges Tranquility	= new GymBadges(0,	"Gym_Badge_Tranquility");
		public static readonly GymBadges Freedom		= new GymBadges(0,	"Gym_Badge_Freedom");
		public static readonly GymBadges Patience		= new GymBadges(0,	"Gym_Badge_Patience");
		public static readonly GymBadges Harmony		= new GymBadges(0,	"Gym_Badge_Harmony");
		public static readonly GymBadges Pride			= new GymBadges(0,	"Gym_Badge_Pride");
	}
}