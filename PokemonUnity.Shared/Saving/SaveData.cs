using System;
using System.Collections.Generic;
using PokemonUnity.Inventory;
using PokemonUnity.Saving.SerializableClasses;
using PokemonUnity.Utility;

namespace PokemonUnity.Saving
{
    [System.Serializable]
    public class SaveData
    {
        public static HashSet<GameState> Saves { get; private set; }
		//[Newtonsoft.Json.JsonProperty]
        public GameState[] GameStates
		{
			get
			{
				GameState[] s = new GameState[Saves.Count];
				Saves.CopyTo(s);
				return s;
			}
			private set
			{
				//Saves = new HashSet<GameState>(value);
				Saves = new HashSet<GameState>();
				if (value != null)
					for (int i = 0; i < value.Length; i++)
						this[i] = value[i];
			}
		}
		/// <summary>
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public GameState this[int i]
		{
			get { return GameStates[i]; } //ToDo: Sort by date
			set
			{
				//There is no "Add new slot" it only overrides what's already present 
				//GameState[] s = new GameState[Saves.Count];
				//Saves.CopyTo(s);
				//s[i] = value;
				//Saves = new HashSet<GameState>(s);

				//Rewrote to override old save file with new one
				if (Saves.Contains(value))
					Saves.Remove(value);
				if(value.TimeCreated.Ticks != new DateTime().Ticks || (value.TrainerID != 0 && value.SecretID != 0))
					Saves.Add(value);
			}
		} 
        #region ImportantInfo
        public string BuildVersion { get; private set; } //SaveManager.GetBuildVersion();
        //public string SaveName { get; private set; }
		public DateTime TimeCreated { get; private set; }
		#endregion

		#region User Settings 
		//GameSettings is different from UserSettings, and should be loaded first before save state is even reached...
		public int Language { get; private set; }
		public byte WindowBorder { get; private set; }
		public byte DialogBorder { get; private set; }
		public byte TextSpeed { get; private set; }
		//public bool FullScreen { get; private set; }
		public float mVol { get; private set; }
		public float sVol { get; private set; }
		#endregion

		#region GameState Variables
		/*#region Player
		public string PlayerName { get; private set; }		
        //public int playerID { get { return (TrainerID + SecretID) * 65536; } }
        public int TrainerID { get; private set; }
        public int SecretID { get; private set; }
        public bool IsMale { get; private set; }
        //public Dictionary<GymBadges, System.DateTime?> GymBadges;
        #endregion

        #region PlayerInfo
        public bool?[] Pokedex { get; private set; }
        public byte[][] Pokedex2 { get; private set; }
		/// <summary>
		/// Duration of time accumulated on save file
		/// </summary>
		public System.TimeSpan PlayTime { get; private set; }
		public int PlayerMoney { get; private set; }
		public int PlayerCoins { get; private set; }
		public int ActiveMapId { get; private set; }
		//public int ActivePcBox { get; private set; }

		//Player
		public float PlayerDirection { get; private set; }
		public Vector PlayerPosition { get; private set; }
		////Follower
		///// <summary>
		///// </summary>
		///// i would really just make this an offset of player position, i.e. 1-back, 1-left. 
		///// don't think it's necessary to store full on x,y,z position of something that's always xy-distance away from player
		//public SeriV3 FollowerPosition { get; private set; }
		// Don't need to save follower direction... should just be the same direction player is facing...
		//public int FollowerDirection { get; private set; }
		/// <summary>
		/// Which pokemon from player party is out in overworld with player
		/// </summary>
		public int FollowerPokemon { get; private set; }
		public int RepelSteps { get; private set; }
		public string[] Rival { get; private set; }
		#endregion

		#region Pokemon Center
		/// <summary>
		/// When the Player blacks out they need to return to the last visited Pokemon Center
		/// </summary>
		public int PokeCenterId { get; private set; }
		//public int PokeCenterAreaId { get; private set; }
		//public int pCenterScene { get; private set; }
		//private SeriV3 pCenterPosition;
		//private int pCenterDirection;
		#endregion

		#region SeriClasses
		public SeriPokemon[] PlayerParty { get; private set; }
		public SeriPC PC { get; private set; }
		public Items[] PlayerBag { get; private set; }
		public Dictionary<GymBadges,DateTime?> GymsChallenged { get; private set; }
		//public List<SaveEvent> EventList { get; private set; }
		#endregion*/
		#endregion

		static SaveData()
		{
			Saves = new HashSet<GameState>();
		}

		//public SaveData (Character.Player player, int? money = null, int? coin = null, byte[,] pokedex = null, 
		//	TimeSpan? time = null, Vector position = new Vector(), 
		//	float? direction = null, int? scene = null, 
		//	int? pokecenter = null, Dictionary<GymBadges, DateTime?> gym = null, List<Items> bag = null, 
		//	SeriPC? pc = null//, List<SaveEvent> eventList = null
		//	) 
		//		: this(name: player.Name, money: money, coin: coin, trainer: player.Trainer.TrainerID,
		//			  secret: player.Trainer.SecretID, gender: player.Trainer.Gender, pokedex: pokedex,
		//			  time: time, position: position, 
		//			  direction: direction, map: scene, pokecenter: pokecenter,
		//			  gym: gym, bag: bag, party: player.Trainer.Party.Serialize()//, pc: pc, eventList: eventList
		//			  )
		//{
		//	// Just made this one for fun... might be useful in future, than doing things the long way, like below
		//}

		[Newtonsoft.Json.JsonConstructor]
		public SaveData (GameState[] state = null,//string name = null, int? money = null, int? coin = null, int? trainer = null, int? secret = null, 
			//bool? gender = null, byte[,] pokedex = null, TimeSpan? time = null, Vector? position = null,//new Vector(), 
			//string[] rival = null, int? repel = null, float? direction = null, 
			//int? map = null, byte box = 0, int? pokecenter = null, Dictionary<GymBadges, DateTime?> gym = null, List<Items> bag = null, 
			//SeriPokemon[] party = null, SeriPC? pc = null,// List<SaveEvent> eventList = null
			string build = null, string date = null,
			Languages? language = null, byte? windowBorder = null, byte? dialogBorder = null, byte? textSpeed = null, float? mvol = null, float? svol = null
			)
		{
			//SaveName			= saveName;
			BuildVersion		= build			?? SaveManager.BuildVersion;//.GetBuildVersion();
			TimeCreated			= date != null	?  DateTime.Parse(date) : DateTime.UtcNow;
			Language			= (int?)language?? (int)Game.UserLanguage;
			WindowBorder		= windowBorder	?? Game.WindowSkin;
			DialogBorder		= dialogBorder	?? Game.DialogSkin;
			TextSpeed			= textSpeed		?? Game.textSpeed;
			mVol				= mvol			?? Game.mVol;
			sVol				= svol			?? Game.sVol;

			/*PlayerName			= name			?? Game.GameData.Player.Name;
			PlayerMoney			= money			?? Game.GameData.Player.Money;
			PlayerCoins			= coin			?? Game.GameData.Player.Coins;
			GymsChallenged		= gym			?? Game.GameData.Player.GymsBeatTime;
			PlayerBag			= bag.ToArray()	?? Game.GameData.Player.Bag.Contents;//Player.Bag; //playerBag;
			TrainerID			= trainer		?? Game.GameData.Player.Trainer.TrainerID;
			SecretID			= secret		?? Game.GameData.Player.Trainer.SecretID;
			IsMale				= gender		?? Game.GameData.Player.IsMale;
			//Pokedex2			= pokedex		?? Game.GameData.Player.Pokedex;
			PlayTime			= time			?? Game.GameData.Player.PlayTime;
			RepelSteps			= repel			?? Game.GameData.RepelSteps;
			Rival				= rival			?? Game.GameData.Rival;
			PlayerPosition		= position		?? Game.GameData.PlayerPosition; //Game.GameData.Player.playerPosition;
			PlayerDirection		= direction		?? Game.GameData.PlayerDirection;//Game.GameData.Player.playerDirection;
			//FollowerPokemon	= Game.GameData.Player.followerPokemon;
			//FollowerPosition	= Game.GameData.Player.followerPosition;
			//FollowerDirection	= Game.GameData.Player.followerDirection;
			//ActivePcBox		= box;//		?? Game.GameData.Player.ActivePcBox;
			ActiveMapId			= map			?? Game.GameData.Area;//Game.GameData.Player.mapName;.activeScene;
			PokeCenterId		= pokecenter	?? (int)Game.GameData.Checkpoint;//Game.GameData.Player.Checkpoint;pkmnCenter;
			PlayerParty			= party			?? Game.GameData.Player.Trainer.Party.Serialize();
			//	new SeriPokemon[Game.GameData.Player.Trainer.Party.Length];
			//if(party != null)
			//	for (int i = 0; i < Game.GameData.Player.Trainer.Party.Length; i++)
			//	{
			//		PlayerParty[i]	= Game.GameData.Player.Trainer.Party[i];
			//	}

			byte[,] dex2		= pokedex		?? Game.GameData.Player.Pokedex;
			Pokedex2 = new byte[dex2.GetLength(0)][];
			for (int i = 0; i < Pokedex2.Length; i++)
			{
				Pokedex2[i] = new byte[dex2.GetLength(1)];
				for (int j = 0; j < Pokedex2[i].Length; j++)
				{
					Pokedex2[i][j] = (byte)dex2[i, j];
				}
			}
				
			//ToDo: Store user's Active PC
			PC = pc ?? new SeriPC(
				//Game.GameData.PC.AllBoxes, Game.GameData.PC.BoxNames, Game.GameData.PC.BoxTextures, Game.GameData.PC.Items
				new Character.PC(pkmns: Game.GameData.PC.AllBoxes, items: Game.GameData.PC.Items, box: box, names: Game.GameData.PC.BoxNames, textures: Game.GameData.PC.BoxTextures)
				);
			//EventList			= eventList; //Game.EventList;*/
			if (state != null) GameStates = state;
        }

		//public SaveData
        //    (
		//		string saveName,
		//
		//		Languages language,
		//		byte windowBorder,
		//		byte dialogBorder,
		//		byte textSpeed,
		//		float mvol,
		//		float svol,
		//
		//		string playerName,
		//		int trainerID,
		//		int secretID,
		//		bool isMale,
		//
		//		bool?[] pokedex,
		//		TimeSpan PlayTime,
		//
		//		SeriV3 playerPosition,
		//		int playerDirection,
		//		SeriV3 followerPosition,
		//		int followerDirection,
		//
		//		int activeScene,
		//
		//		Pokemon.Pokemon[] playerParty,
		//		SeriPC pc,
		//		List<Items> playerBag,
		//
		//		List<SaveEvent> eventList
        //    )
        //{
        //    SaveName = saveName;
		//
        //    //Language = (int)language;
        //    //WindowBorder = windowBorder;
        //    //DialogBorder = dialogBorder;
        //    //TextSpeed = textSpeed;
        //    //mVol = mvol;
        //    //sVol = svol;
		//
        //    Name = playerName;
        //    TrainerID = trainerID;
        //    SecretID = secretID;
        //    IsMale = isMale;
		//
        //    Pokedex = pokedex;
        //    PlayerTime = PlayTime;
		//
        //    PlayerPosition = playerPosition;
        //    PlayerDirection = playerDirection;
        //    FollowerPosition = followerPosition;
        //    FollowerDirection = followerDirection;
		//
        //    ActiveScene = activeScene;
		//
        //    PlayerParty = new SeriPokemon[playerParty.Length];
        //    for (int i = 0; i < playerParty.Length; i++)
        //    {
        //        PlayerParty[i] = playerParty[i];
        //    }
        //    PC = pc;
        //    PlayerBag = playerBag;
		//
        //    EventList = eventList;
        //}
    }
}