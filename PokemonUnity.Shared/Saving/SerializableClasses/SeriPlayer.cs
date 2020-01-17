/*namespace PokemonUnity.Saving.SerializableClasses
{
    using System;
    using System.Collections.Generic;
    using PokemonUnity.Inventory;
    using PokemonUnity.Monster;
    using PokemonUnity.Utility;

    [System.Serializable]
	public struct SeriPlayer
	{
		#region Variables
		/// <summary>
		/// Please use the values stored in <see cref="Trainer.TrainerID"/>
		/// </summary>
		private uint trainerId { get; set; }
		/// <summary>
		/// Please use the values stored in <see cref="Trainer.SecretID"/>
		/// </summary>
		private uint secretId { get; set; }
		public string Name { get; private set; }
		public bool IsMale { get; private set; }
		/// <summary>
		/// Player's Pokemon Party is stored in Player class, 
		/// and then reflected in Trainer, to match what occurs
		/// </summary>
		/// Didn't think about it it till now but the player should
		/// hold the `Trainer` data, and instantiate a new Trainer
		/// whenever it's needed...
		public SeriPokemon[] Party { get; private set; }
		/// <summary>
		/// When displaying items in bag, do a foreach loop and filter by item category
		/// </summary>
		public Items[] Bag { get; private set; }
		public SeriPC PC { get; private set; }
				
		#region Player and Overworld Data
		public Regions Region { get; private set; }
		public Locations Location { get; private set; }
		/// <summary>
		/// </summary>
		public int Area { get; private set; }
		public Vector Position { get; set; }
		/// <summary>
		/// Last town or Pokemon Center visited, that's used as Respawn Point upon a Player's Defeat
		/// </summary>
		public Locations Checkpoint { get; set; }
		//public Locations respawnCenterId { get; set; }
		//public SeriV3 respawnScenePosition;
		public int RepelSteps { get; set; } 
		public byte FollowPokemon { get; set; } 
		#endregion

		#region Private Records of Player Storage Data
		public bool IsCreator { get; private set; }
		//ToDo: Berry Field Data (0x18 per tree, 36 trees)
		//ToDo: Honey Tree, smearing honey on tree will spawn pokemon in 6hrs, for 24hrs (21 trees)
		//Honey tree timer is done in minutes (1440, spawns at 1080), only goes down while playing...
		//ToDo: Missing Variable for DayCare, maybe `Pokemon[,]` for multipe locations?
		//Daycare Data
		//(Slot 1) Occupied Flag 
		//(Slot 1) Steps Taken Since Depositing 
		//(Slot 1) Box EK6 1 
		//(Slot 2) Occupied Flag 
		//(Slot 2) Steps Taken Since Depositing2 
		//(Slot 2) Box EK6 2 
		//Flag (egg available) 
		//RNG Seed
		//ToDo: a bool variable for PC background (if texture is unlocked) `bool[]`
		public static string PlayerItemData { get; set; }
		public static string PlayerDayCareData { get; set; } //KeyValuePair<Pokemon,steps>[]
		public static string PlayerBerryData { get; set; }
		public static string PlayerNPCData { get; set; }
		public static string PlayerApricornData { get; set; }
		#endregion

		#region Player Records
		public string RivalName { get; private set; }
		/// <summary>
		/// </summary>
		public int Money { get; private set; }
		public int Coins { get; private set; }
		public int Savings { get; private set; }

		/// <summary>
		/// Usage:<para>
		/// <code>playerPokedex[1,0] == 0; means pokemonId #1 not seen</code>
		/// </para>
		/// <code>playerPokedex[1,1] == 0; means pokemonId #1 not captured</code>
		/// <para></para>
		/// <code>playerPokedex[1,2] == 3; means the 3rd form of pokemonId was first to be scanned into pokedex</code>
		/// </summary>
		/// <remarks>Or can be int?[pokedex.count,1]. if null, not seen or captured</remarks>
		public byte[,] Pokedex { get; private set; }
		public System.TimeSpan PlayTime { get; private set; }

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Each Badge in <see cref="GymBadges"/> is a Key/Value,
		/// regardless of how they're set in game. One value per badge.
		/// </remarks>
		public KeyValuePair<GymBadges, System.DateTime?>[] GymsBeatTime { get; private set; }
		#endregion

		#region Player Customization
		///// <summary>
		///// Active player design
		///// </summary>
		///// ToDo: Player outfits should be stored and loaded from the player PC?
		///// Rather than adding another variable for `Item` data...
		///// Not sure if player custom designs are an `Item` type or a custom enum...
		//public int playerOutfit	{ get; set; }
		//public int playerScore	{ get; set; }
		//public int playerShirt	{ get; set; }
		//public int playerMisc	{ get; set; }
		//public int playerHat	{ get; set; }
		#endregion
		#endregion

		#region Constructor
		//public SeriPlayer(string name, bool gender, Pokemon[] party = null, Items[] bag = null, Pokemon[][] pc_poke = null, KeyValuePair<Items,int>[] pc_items = null, 
		//	byte? pc_box = null, string[] pc_names = null, int[] pc_textures = null, int? trainerid = null, int? secretid = null,
		//	int? money = null, int? coin = null, int? bank = null, int? repel = null, string rival = null,
		//	byte[][] dex = null, TimeSpan? time = null, Vector? position = null, byte? follower = null,
		//	bool? creator = null, int? map = null, int? pokecenter = null, KeyValuePair<GymBadges, DateTime?>[] gym = null) : this()
		//{
		//	//playerPokedex = new bool?[Pokemon.PokemonData.Database.Length];
		//	Pokedex = new byte[Game.PokemonData.Where(x => x.Value.IsDefault).Count(), 3];
		//	PlayTime = new TimeSpan();
		//	Position = new Vector();
		//	Bag = new Bag();
		//	PC = new PC();
		//	Party = new Pokemon[]
		//	{
		//		new Pokemon(Pokemons.NONE),
		//		new Pokemon(Pokemons.NONE),
		//		new Pokemon(Pokemons.NONE),
		//		new Pokemon(Pokemons.NONE),
		//		new Pokemon(Pokemons.NONE),
		//		new Pokemon(Pokemons.NONE)
		//	};
		//
		//	//List<GymBadges> gymBadges = new List<GymBadges>();
		//	GymsBeatTime = new Dictionary<GymBadges, DateTime?>();
		//	foreach (GymBadges i in (GymBadges[])Enum.GetValues(typeof(GymBadges)))
		//	{
		//		//gymBadges.Add(i);
		//		GymsBeatTime.Add(i, null);
		//	}
		//	//gymsEncountered = new bool[gymBadges.Count];
		//	//gymsBeaten = new bool[gymBadges.Count];
		//	//gymsBeatTime = new System.DateTime?[gymBadges.Count];
		//	//GymsBeatTime = new System.DateTime?[gymBadges.Count];
		//	Checkpoint		= Locations.PALLET_TOWN;
		//	Name = name;
		//	IsMale = gender;
		//	Party = party ?? Party;
		//	Bag = new Bag(bag ?? new Items[0]);
		//	PC = new PC(pkmns: pc_poke, items: pc_items, box: pc_box, names: pc_names, textures: pc_textures);
		//	if(trainerid != null)this.trainerId	= trainerid;
		//	if(secretid != null) this.secretId	= secretid;
		//	if(time != null) this.PlayTime		= time.Value;
		//	if(pokecenter != null) this.Checkpoint = (Locations)pokecenter;
		//	if(position != null) this.Position	= position.Value;
		//	//Position			= position		?? new Vector()									;
		//	Money				= money			?? 0											;
		//	Coins				= coin			?? 0											;
		//	Savings				= bank			?? 0											;
		//	IsCreator			= creator		?? false										;
		//	FollowPokemon		= follower		?? 0											;
		//	Area				= map			?? 0											;
		//	RepelSteps			= repel			?? 0											;
		//	RivalName			= rival			?? null											;
		//
		//	if(gym != null)
		//		foreach (KeyValuePair<GymBadges, DateTime?> g in gym)
		//			if (!GymsBeatTime.ContainsKey(g.Key))
		//				GymsBeatTime.Add(g.Key,g.Value);
		//
		//	//byte[][] dex		= pokedex		?? new byte[Game.PokemonData.Where(x => x.Value.IsDefault).Count()][]; 
		//	//Pokedex = new byte[dex.Length,3];
		//	if(dex != null)
		//		for (int i = 0; i < dex.Length; i++)
		//			//Pokedex[i] = new byte[dex[i].Length];
		//			for (int j = 0; j < dex[i].Length; j++)
		//				Pokedex[i,j] = (byte)dex[i][j];
		//}

		public SeriPlayer(string name, bool gender, SeriPokemon[] party, Items[] bag = null, SeriPC pc = null, uint? trainerid = null, uint? secretid = null,
			int? money = null, int? coin = null, int? bank = null, int? repel = null, string rival = null,
			byte[][] dex = null, TimeSpan? time = null, Vector? position = null, byte? follower = null,
			bool? creator = null, int? map = null, int? pokecenter = null, KeyValuePair<GymBadges, DateTime?>[] gym = null)
			//: this (name: name, gender: gender, party: party, bag: bag != null ? bag.Contents : null, 
			//	  pc_poke: pc != null ? pc.AllBoxes : null, pc_items: pc != null ? pc.Items : null, 
			//	  pc_names: pc != null ? pc.BoxNames : null, pc_textures: pc != null ? pc.BoxTextures : null, 
			//	  pc_box: pc != null ? pc.ActiveBox : (byte?)null, trainerid: trainerid, secretid: secretid,
			//	  money:money, coin:coin, bank:bank,repel:repel, rival:rival, dex: dex, time:time, position:position, follower:follower,
			//	  creator:creator, map:map, pokecenter:pokecenter, gym:gym)
		{
			//playerPokedex = new bool?[Pokemon.PokemonData.Database.Length];
			//Pokedex = new byte[Game.PokemonData.Where(x => x.Value.IsDefault).Count(), 3];
			PlayTime = new TimeSpan();
			Position = new Vector();
			//Bag = new Bag();
			PC = new SeriPC();
			Party = new SeriPokemon[]
			{
				new Pokemon(Pokemons.NONE),
				new Pokemon(Pokemons.NONE),
				new Pokemon(Pokemons.NONE),
				new Pokemon(Pokemons.NONE),
				new Pokemon(Pokemons.NONE),
				new Pokemon(Pokemons.NONE)
			};

			//GymsBeatTime = new Dictionary<GymBadges, DateTime?>();
			//foreach (GymBadges i in (GymBadges[])Enum.GetValues(typeof(GymBadges)))
			//{
			//	//gymBadges.Add(i);
			//	GymsBeatTime.Add(i, null);
			//}
			GymsBeatTime = new KeyValuePair<GymBadges, DateTime?>[Enum.GetValues(typeof(GymBadges)).Length];
			Checkpoint		= Locations.PALLET_TOWN;
			Name = name;
			IsMale = gender;
			Party = party ?? this.Party;
			Bag = bag ?? new Items[0];
			//PC = new SeriPC(pkmns: pc_poke, items: pc_items, box: pc_box, names: pc_names, textures: pc_textures);
			if(trainerid != null)this.trainerId	= trainerid.Value;
			if(secretid != null) this.secretId	= secretid.Value;
			if(time != null) this.PlayTime		= time.Value;
			if(pokecenter != null) this.Checkpoint = (Locations)pokecenter;
			if(position != null) this.Position	= position.Value;
			//Position			= position		?? new Vector()									;
			Money				= money			?? 0											;
			Coins				= coin			?? 0											;
			Savings				= bank			?? 0											;
			IsCreator			= creator		?? false										;
			FollowPokemon		= follower		?? 0											;
			Area				= map			?? 0											;
			RepelSteps			= repel			?? 0											;
			RivalName			= rival			?? null											;

			if(gym != null)
			//	foreach (KeyValuePair<GymBadges, DateTime?> g in gym)
			//		if (!GymsBeatTime.ContainsKey(g.Key))
			//			GymsBeatTime.Add(g.Key,g.Value);
				GymsBeatTime = gym;

			//byte[][] dex		= pokedex		?? new byte[Game.PokemonData.Where(x => x.Value.IsDefault).Count()][]; 
			//Pokedex = new byte[dex.Length,3];
			if(dex != null)
				for (int i = 0; i < dex.Length; i++)
					//Pokedex[i] = new byte[dex[i].Length];
					for (int j = 0; j < dex[i].Length; j++)
						Pokedex[i,j] = (byte)dex[i][j];
		}
		#endregion		
	}
}*/