using System;
using System.Collections.Generic;
using System.Linq;
using PokemonUnity.Inventory;
using PokemonUnity.Saving.SerializableClasses;
using PokemonUnity.Application;
using PokemonUnity.Utility;

namespace PokemonUnity.Saving
{

	[System.Serializable]
	public struct GameState : IEquatable<GameState>, IEqualityComparer<GameState>
	{
		#region ImportantInfo
		//public string BuildVersion { get; private set; } //SaveManager.GetBuildVersion();
		public DateTime TimeCreated { get; private set; }
		public Feature Features { get; private set; }
		public Challenges Challenge { get; private set; }
		//public GameModes Mode { get; private set; }
		#endregion

		#region Player
		//public Character.Player Player
		//{
		//	get
		//	{
		//		return new Character.Player(
		//			name: PlayerName
		//			, gender: IsMale
		//			, party: PlayerParty.Deserialize()
		//			, bag: PlayerBag
		//			, pc_poke: PlayerPC.Pokemons.Deserialize()//GetPokemonsFromSeri()
		//			, pc_items: PlayerPC.GetItemsFromSeri().Compress()
		//			, pc_box: PlayerPC.ActiveBox//ActivePcBox
		//			, pc_names: PlayerPC.BoxNames//PlayerPcNames
		//			, pc_textures: PlayerPC.BoxTextures//PlayerPcTextures
		//			, trainerid: TrainerID
		//			, secretid: SecretID
		//		);
		//	}
		//}	
		public string PlayerName { get; private set; }	
		public int TrainerID { get; private set; }
		public int SecretID { get; private set; }
		public bool IsMale { get; private set; }
		#endregion

		#region PlayerInfo
		public byte[][] Pokedex { get; private set; }
		/// <summary>
		/// Duration of time accumulated on save file
		/// </summary>
		public System.TimeSpan PlayTime { get; private set; }
		public int PlayerMoney { get; private set; }
		public int PlayerCoins { get; private set; }
		public int PlayerSavings { get; private set; }
		public bool IsCreator { get; private set; }
		public KeyValuePair<GymBadges,DateTime?>[] GymsChallenged { get; private set; }
		#endregion

		#region Overworld
		/// <summary>
		/// When the Player blacks out they need to return to the last visited Pokemon Center
		/// </summary>
		public int PokeCenterId { get; private set; }
		public int ActiveMapId { get; private set; }
		public Vector PlayerPosition { get; private set; }
		/// <summary>
		/// Which pokemon from player party is out in overworld with player;
		/// 0 => None, 1-6 => Trainer's Party
		/// </summary>
		public byte FollowerPokemon { get; private set; }
		public int RepelSteps { get; private set; }
		//public string OverworldItemData { get; private set; }
		//public string DayCareData { get; private set; } //KeyValuePair<Pokemon,steps>[]
		//public string BerryData { get; private set; }
		//public string NPCData { get; private set; }
		//public string ApricornData { get; private set; }
		#endregion

		#region SeriClasses
		public SeriPokemon[] PlayerParty { get; private set; }
		public SeriPC PlayerPC { get; private set; }
		public Items[] PlayerBag { get; private set; }
		#endregion

		#region Constructors
		//public GameState (Player player, int? money = null, int? coin = null, byte[,] pokedex = null, 
		//	TimeSpan? time = null, Vector position = new Vector(), float? direction = null, int? scene = null, 
		//	int? pokecenter = null, KeyValuePair<GymBadges, DateTime?>[] gym = null, Items[] bag = null, 
		//	SeriPC pc = null
		//	) 
		//		: this(name: player.Name, money: money, coin: coin, trainer: player.Trainer.TrainerID,
		//				secret: player.Trainer.SecretID, gender: player.Trainer.Gender, pokedex: pokedex,
		//				time: time, position: position, direction: direction, map: scene, pokecenter: pokecenter,
		//				gym: gym, bag: bag, party: player.Trainer.Party.Serialize()
		//			)
		//{
		//}

		//public GameState (string name, Challenges? challenge = null, Feature? features = null, int? money = null, int? coin = null, int? trainer = null, int? secret = null, 
		//	bool? gender = null, byte[,] pokedex = null, TimeSpan? time = null, Vector? position = null, float? direction = null, byte? follower = null,
		//	bool? creator = null, int? map = null, byte? box = null, int? pokecenter = null, KeyValuePair<GymBadges, DateTime?>[] gym = null, Items[] bag = null, 
		//	SeriPokemon[] party = null, Character.PC pc = null
		//	)
		//{
		//	//BuildVersion		= SaveManager.BuildVersion;//.GetBuildVersion();
		//	TimeCreated			= DateTime.UtcNow;
		//	Challenge			= challenge		?? Challenges.Classic;
		//	Features			= features		?? new Feature();
		//
		//	PlayerName			= name;//		?? 												;//Game.Player.Name;
		//	PlayerMoney			= money			?? 0											;//Game.Player.Money;
		//	PlayerCoins			= coin			?? 0											;//Game.Player.Coins;
		//	GymsChallenged		= gym			?? new KeyValuePair<GymBadges, DateTime?>[0]	;//Game.Player.GymsBeatTime;
		//	PlayerBag			= bag			?? new Items[0]									;//Game.Bag_Items;//Player.Bag; //playerBag;
		//	TrainerID			= trainer		?? 0											;//Game.Player.Trainer.TrainerID;
		//	SecretID			= secret		?? 0											;//Game.Player.Trainer.SecretID;
		//	IsMale				= gender		?? true											;//Game.Player.isMale;
		//	IsCreator			= creator		?? false										;//Game.Player.IsCreator;
		//	//Pokedex2			= pokedex		?? 												;//Game.Player.Pokedex;
		//	PlayTime			= time			?? new TimeSpan()								;//Game.Player.PlayTime;
		//	PlayerPosition		= position		?? new Vector()									;//Game.PlayerPosition; //Game.Player.playerPosition;
		//	//PlayerDirection	= direction		?? 0											;//Game.PlayerDirection;//Game.Player.playerDirection;
		//	FollowerPokemon		= follower		?? 0											;//Game.Player.followerPokemon;
		//	ActiveMapId			= map			?? 0											;//Game.Area;//Game.Player.mapName;.activeScene;
		//	//ActivePcBox		= box			?? 0											;//Game.Player.ActivePcBox;
		//	//PlayerPcNames		= boxnames		?? new string[Core.STORAGEBOXES]				;//Game.Player.PC.BoxNAmes;
		//	//PlayerPcTextures	= boxtextures	?? new int[Core.STORAGEBOXES]					;//Game.Player.PC.BoxNAmes;
		//	PokeCenterId		= pokecenter	?? 0											;//(int)Game.Checkpoint;//Game.Player.Checkpoint;pkmnCenter;
		//	PlayerParty			= party			?? 												//Game.Player.Trainer.Party.Serialize();
		//		new SeriPokemon[6];//[player.Party.Length];
		//	for (int i = 0; i < PlayerParty.Length; i++)
		//	{
		//		//PlayerParty[i]= Game.Player.Trainer.Party[i];
		//		PlayerParty[i]	= new SeriPokemon();
		//	}
		//
		//	byte[,] dex2		= pokedex		?? new byte[Game.PokemonData.Where(x => x.Value.IsDefault).Count(), 3];  //Game.Player.Pokedex;
		//	Pokedex = new byte[dex2.GetLength(0)][];
		//	for (int i = 0; i < Pokedex.Length; i++)
		//	{
		//		Pokedex[i] = new byte[dex2.GetLength(1)];
		//		for (int j = 0; j < Pokedex[i].Length; j++)
		//		{
		//			Pokedex[i][j] = (byte)dex2[i, j];
		//		}
		//	}
		//		
		//	PlayerPC = pc != null 
		//		? new SeriPC(pc) 
		//		: new SeriPC(new Monster.Pokemon[Core.STORAGEBOXES, 30], new string[Core.STORAGEBOXES], new int[Core.STORAGEBOXES], new List<Items>());
		//}

		public GameState (Character.Player player, Challenges? challenge = null, Feature? features = null,
			string overworld = null, string daycare = null, string berry = null, string apricon = null, string npc = null
			//, string name, int? money = null, int? coin = null, int? trainer = null, int? secret = null, 
			//bool? gender = null, byte[,] pokedex = null, TimeSpan? time = null, Vector? position = null, float? direction = null, byte? follower = null,
			//bool? creator = null, int? map = null, byte? box = null, int? pokecenter = null, KeyValuePair<GymBadges, DateTime?>[] gym = null, Items[] bag = null, 
			//SeriPokemon[] party = null, Character.PC pc = null
			)
		{
			//BuildVersion		= SaveManager.BuildVersion;//.GetBuildVersion();
			TimeCreated			= DateTime.UtcNow;
			Challenge			= challenge		?? Challenges.Classic;
			Features			= features		?? new Feature();

			PlayerName			= player.Name;//		?? 												;//Game.Player.Name;
			PlayerMoney			= player.Money;//		?? 0											;//Game.Player.Money;
			PlayerCoins			= player.Coins;//		?? 0											;//Game.Player.Coins;
			PlayerSavings		= player.Savings;//		?? 0											;
			RepelSteps			= player.RepelSteps;//	?? 0											;
			GymsChallenged		= player.GymsBeatTime.ToArray();//			?? new KeyValuePair<GymBadges, DateTime?>[0]	;//Game.Player.GymsBeatTime;
			PlayerBag			= player.Bag.Contents;//			?? new Items[0]									;//Game.Bag_Items;//Player.Bag; //playerBag;
			TrainerID			= player.Trainer.TrainerID					;//Game.Player.Trainer.TrainerID;
			SecretID			= player.Trainer.SecretID						;//Game.Player.Trainer.SecretID;
			IsMale				= player.IsMale;//		?? true											;//Game.Player.isMale;
			IsCreator			= player.IsCreator;//		?? false										;//Game.Player.IsCreator;
			PlayTime			= player.PlayTime;//			?? new TimeSpan()								;//Game.Player.PlayTime;
			PlayerPosition		= player.Position;//		?? new Vector()									;//Game.PlayerPosition; //Game.Player.playerPosition;
			FollowerPokemon		= player.FollowPokemon;//		?? 0											;//Game.Player.followerPokemon;
			ActiveMapId			= player.Area;//			?? 0											;//Game.Area;//Game.Player.mapName;.activeScene;
			//ActivePcBox		= player.box			?? 0											;//Game.Player.ActivePcBox;
			//PlayerPcNames		= player.boxnames		?? new string[Core.STORAGEBOXES]				;//Game.Player.PC.BoxNAmes;
			//PlayerPcTextures	= player.boxtextures	?? new int[Core.STORAGEBOXES]					;//Game.Player.PC.BoxNAmes;
			PokeCenterId		= (int)player.Checkpoint;//	?? 0											;//(int)Game.Checkpoint;//Game.Player.Checkpoint;pkmnCenter;
			PlayerParty			= //player.Party			?? 												//Game.Player.Trainer.Party.Serialize();
				new SeriPokemon[player.Party.Length];
			for (int i = 0; i < PlayerParty.Length; i++)
			{
				//PlayerParty[i]= Game.Player.Trainer.Party[i];
				PlayerParty[i]	= (SeriPokemon)player.Party[i];
			}

			//byte[,] dex2		= player.Pokedex;//		?? new byte[Game.PokemonData.Where(x => x.Value.IsDefault).Count(), 3];  //Game.Player.Pokedex;
			Pokedex = new byte[player.Pokedex.GetLength(0)][];
			for (int i = 0; i < Pokedex.Length; i++)
			{
				Pokedex[i] = new byte[player.Pokedex.GetLength(1)];
				for (int j = 0; j < Pokedex[i].Length; j++)
				{
					Pokedex[i][j] = (byte)player.Pokedex[i, j];
				}
			}
				
			PlayerPC = player.PC != null 
				? new SeriPC(player.PC) 
				: new SeriPC(new Monster.Pokemon[Core.STORAGEBOXES, 30], new string[Core.STORAGEBOXES], new int[Core.STORAGEBOXES], new List<Items>());
		}
		#endregion

		#region Methods
		public Character.Player GetPlayer()
		{
			return new Character.Player(
				name: PlayerName
				, gender: IsMale
				, party: PlayerParty.Deserialize()
				, bag: PlayerBag
				, pc_poke: PlayerPC.Pokemons.Deserialize()//GetPokemonsFromSeri()
				, pc_items: PlayerPC.GetItemsFromSeri().Compress()
				, pc_box: PlayerPC.ActiveBox//ActivePcBox
				, pc_names: PlayerPC.BoxNames//PlayerPcNames
				, pc_textures: PlayerPC.BoxTextures//PlayerPcTextures
				, trainerid: TrainerID
				, secretid: SecretID
				, money: PlayerMoney
				, coin: PlayerCoins
				, bank: PlayerSavings
				, repel: RepelSteps
				, time: PlayTime
				, position: PlayerPosition
				, follower: FollowerPokemon
				, creator: IsCreator
				//, map: 
				, pokecenter: PokeCenterId
				, gym: GymsChallenged
			);
		}
		#endregion

		#region Explicit Operators
		public static implicit operator GameState(Game game)
		{
			GameState state = new GameState(
				game.Player
				,game.Challenge
				,game.Features
			);
			return state;
		}
		public static bool operator ==(GameState x, GameState y)
		{
			//return ((x.IsMale == y.IsMale) && (x.TrainerID == y.TrainerID) && (x.SecretID == y.SecretID)) & (x.Player.Name == y.Player.Name);
			//Same person, same game? (Different from same game, same Character)
			//return ((x.TrainerID == y.TrainerID) && (x.SecretID == y.SecretID)); 
			return ((x.TrainerID == y.TrainerID) && (x.SecretID == y.SecretID)); 
			//What about date created?... to check if the saved profiles are the same
		}
		public static bool operator !=(GameState x, GameState y)
		{
			//return ((x.IsMale != y.IsMale) || (x.TrainerID != y.TrainerID) || (x.SecretID != y.SecretID)) | (x.Player.Name == y.Player.Name);
			//return ((x.TrainerID != y.TrainerID) || (x.SecretID != y.SecretID));
			return ((x.TrainerID != y.TrainerID) || (x.SecretID != y.SecretID));
		}
		//public bool Equals(Game obj)
		//{
		//	return this == obj.Trainer; //Equals(obj.Trainer);
		//}
		public bool Equals(GameState obj)
		{
			return this == obj; //Equals(obj.Trainer);
		}
		public override bool Equals(object obj)
		{
			if (typeof(GameState) == obj.GetType())
				return Equals((GameState)obj);
			return base.Equals(obj);
		}
		bool IEquatable<GameState>.Equals(GameState other)
		{
			return Equals(obj: (object)other);
		}
		public override int GetHashCode()
		{
			return (TrainerID + SecretID * 65536).GetHashCode();
		}

		bool IEqualityComparer<GameState>.Equals(GameState x, GameState y)
		{
			return x == y;
		}

		int IEqualityComparer<GameState>.GetHashCode(GameState obj)
		{
			return obj.GetHashCode();
		}
		#endregion
	}
}