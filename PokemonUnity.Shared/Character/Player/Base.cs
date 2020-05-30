using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Attack;
using PokemonUnity.Inventory;
using PokemonUnity.Saving.SerializableClasses;
using PokemonUnity.Character;
using PokemonUnity.Utility;

namespace PokemonUnity.Character
{	
	[Serializable] 
	public class Player
	{
		#region Variables
		/// <summary>
		/// Please use the values stored in <see cref="TrainerId.TrainerID"/>
		/// </summary>
		private int? trainerId { get; set; }
		/// <summary>
		/// Please use the values stored in <see cref="TrainerId.SecretID"/>
		/// </summary>
		private int? secretId { get; set; }
		public string Name { get; private set; }
		public bool IsMale { get; private set; }
		/// <summary>
		/// Player's Pokemon Party is stored in Player class, 
		/// and then reflected in Trainer, to match what occurs
		/// </summary>
		/// Didn't think about it it till now but the player should
		/// hold the `Trainer` data, and instantiate a new Trainer
		/// whenever it's needed...
		public Pokemon[] Party { get; private set; }
		public TrainerId Trainer { get { return new TrainerId(name: Name, gender: IsMale, tID: trainerId, sID: secretId); } }
		/// <summary>
		/// When displaying items in bag, do a foreach loop and filter by item category
		/// </summary>
		public Bag Bag { get; private set; }//{ get { return new Bag(this.Items); } }
		public PC PC { get; private set; }//{ get { return new Game.TrainerPC(this, ActivePcBox); } }
				
		#region Player and Overworld Data
		public Regions Region { get; private set; }
		public Locations Location { get; private set; }
		/// <summary>
		/// </summary>
		public int Area { get; private set; }
		public Vector Position { get; set; }
		/// <summary>
		/// Rotation of player model in overworld scene
		/// </summary>
		/// not sure direction is even needed in save/load profile...
		/// Game should load player facing camera by default.
		public Quaternion Direction { get; set; }
		/// <summary>
		/// Last town or Pokemon Center visited, that's used as Respawn Point upon a Player's Defeat
		/// </summary>
		public Locations Checkpoint { get; set; }
		//public Locations respawnCenterId { get; set; }
		//public SeriV3 respawnScenePosition;
		public int RepelSteps { get; set; } // Should not stack (encourage users to deplete excessive money); reset count based on repel used.
		//public static int RepelType { get; set; } // Maybe instead of this, use Encounter.Rate or... Different repel only changes number of steps, not potency
		//public int SurfPokemon { get { int i = 0; foreach (Pokemon p in Party) if (p.knowsMove(Moves.SURF)) i++; return i; } }
		//public Pokemon GetWalkPokemon() { return null; }
		//public Forms GetWalkPokemon() { return Forms.NONE; } //ToDo: IsShiny?
		public byte FollowPokemon { get; set; } //ToDo: Player.Party[FollowPokemon]
		#endregion

		#region Private Records of Player Storage Data
		public bool IsCreator { get; private set; }
		//ToDo: Berry Field Data (0x18 per tree, 36 trees)
		//ToDo: Honey Tree, smearing honey on tree will spawn pokemon in 6hrs, for 24hrs (21 trees)
		//Honey tree timer is done in minutes (1440, spawns at 1080), only goes down while playing...
		//ToDo: a bool variable for PC background (if texture is unlocked) `bool[]`
		//public static string PlayerDayCareData { get; set; } 
		public DayCare DayCare { get; private set; } 
		public static string PlayerItemData { get; set; }
		public static string PlayerBerryData { get; set; }
		public static string PlayerNPCData { get; set; }
		public static string PlayerApricornData { get; set; }
		#endregion

		#region Player Records
		public string RivalName { get; private set; }
		/// <summary>
		/// </summary>
		public int Money { get { return playerMoney; } set { playerMoney = value > Core.MAXMONEY ? Core.MAXMONEY : value; } }
		public int Coins { get { return playerCoins; } set { playerCoins = value > Core.MAXCOINS ? Core.MAXCOINS : value; } }
		public int Savings { get; private set; }
		private int playerMoney { get; set; }
		private int playerCoins { get; set; }
		//private int playerSavings { get; set; }

		/// <summary>
		/// Usage:<para>
		/// <code>playerPokedex[1,0] == 0; means pokemonId #1 not seen</code>
		/// </para>
		/// <code>playerPokedex[1,1] == 0; means pokemonId #1 not captured</code>
		/// <para></para>
		/// <code>playerPokedex[1,2] == 3; means the 3rd form of pokemonId was first to be scanned into pokedex</code>
		/// </summary>
		/// <remarks>
		/// value [1,2] = Pokemon.Form, other array values remain same.
		/// Or can be int?[pokedex.count,1]. if null, not seen or captured
		/// </remarks>
		//ToDo: Add variable for "Shiny"?... sbyte and `playerPokedex[1,2] == -1;` equals shiny?
		public byte[,] Pokedex { get; private set; }
		//public int PokedexCaught { get { return (from int index in Enumerable.Range(0, Pokedex.GetUpperBound(0)) where Pokedex[index, 1] == 1 select Pokedex[index, 1]).Count(); } }
		//public int PokedexSeen { get { return (from int index in Enumerable.Range(0, Pokedex.GetUpperBound(0)) where Pokedex[index, 0] == 1 select Pokedex[index, 0]).Count(); } }
		public int PokedexCaught
		{
			get
			{
				int x = 0;
				for (int i = 0; i < Pokedex.GetUpperBound(0); i++)
					if (Pokedex[i, 1] == 1) x += 1; 
				return x; //Enumerable.Range(0, Pokedex.GetUpperBound(0)).Where(x => Pokedex[x, 1] == 1).ToArray().Length; } }//.Select( y => Pokedex[y, 1])
			} 
		}
		public int PokedexSeen
		{
			get
			{
				int x = 0;
				for (int i = 0; i < Pokedex.GetUpperBound(0); i++)
					if (Pokedex[i, 0] == 1) x += 1;
				return x; //Enumerable.Range(0, Pokedex.GetUpperBound(0)).Where(x => Pokedex[x, 0] == 1).ToArray().Length; } }//.Where(x => Pokedex[x, 0] == 1)
			}
		}
		public System.DateTime StartDate { get; private set; }
		public System.TimeSpan PlayTime { get; private set; }

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Each Badge in <see cref="GymBadges"/> is a Key/Value,
		/// regardless of how they're set in game. One value per badge.
		/// </remarks>
		public Dictionary<GymBadges, System.DateTime?> GymsBeatTime { get; private set; }
		public GymBadges[] Badges { get { return GymsBeatTime.Where(x => x.Value.HasValue).Select(x => x.Key).ToArray(); } }
		public int BadgesCount { get { return (from gyms in GymsBeatTime where gyms.Value.HasValue select gyms).Count(); } }
		#endregion

		#region Player Customization
		/// <summary>
		/// Active player design
		/// </summary>
		/// Rather than adding another variable for `Item` data...
		/// Not sure if player custom designs are an `Item` type or a custom enum...
		//public int playerOutfit	{ get; set; }
		//public int playerShirt	{ get; set; }
		//public int playerPant		{ get; set; }
		//public int playerMisc		{ get; set; }
		//public int playerHat		{ get; set; }
		public PlayerOutfit Outfit	{ get; set; }
		#endregion
		#endregion

		#region Constructor
		public Player()
		{
			//playerPokedex = new bool?[Pokemon.PokemonData.Database.Length];
			Pokedex = new byte[Game.PokemonData.Where(x => x.Value.IsDefault).Count(), 3];
			StartDate = DateTime.UtcNow; //new DateTime();
			PlayTime = new TimeSpan();
			Position = new Vector();
			Bag = new Bag();
			PC = new PC();
			Party = new Pokemon[]
			{
				new Pokemon(Pokemons.NONE),
				new Pokemon(Pokemons.NONE),
				new Pokemon(Pokemons.NONE),
				new Pokemon(Pokemons.NONE),
				new Pokemon(Pokemons.NONE),
				new Pokemon(Pokemons.NONE)
			};

			//List<GymBadges> gymBadges = new List<GymBadges>();
			GymsBeatTime = new Dictionary<GymBadges, DateTime?>();
			foreach (GymBadges i in (GymBadges[])Enum.GetValues(typeof(GymBadges)))
			{
				//gymBadges.Add(i);
				GymsBeatTime.Add(i, null);
			}
			//gymsEncountered = new bool[gymBadges.Count];
			//gymsBeaten = new bool[gymBadges.Count];
			//gymsBeatTime = new System.DateTime?[gymBadges.Count];
			//GymsBeatTime = new System.DateTime?[gymBadges.Count];
			Checkpoint		= Locations.PALLET_TOWN;
		}

		public Player(string name, bool gender, Pokemon[] party = null, Items[] bag = null, Pokemon[][] pc_poke = null, KeyValuePair<Items,int>[] pc_items = null, 
			byte? pc_box = null, string[] pc_names = null, int[] pc_textures = null, int? trainerid = null, int? secretid = null,
			int? money = null, int? coin = null, int? bank = null, int? repel = null, string rival = null,
			byte[][] dex = null, TimeSpan? time = null, Vector? position = null, byte? follower = null,
			bool? creator = null, int? map = null, int? pokecenter = null, KeyValuePair<GymBadges, DateTime?>[] gym = null) : this()
		{
			Name = name;
			IsMale = gender;
			Party = party ?? Party;
			Bag = new Bag(bag ?? new Items[0]);
			PC = new PC(pkmns: pc_poke, items: pc_items, box: pc_box, names: pc_names, textures: pc_textures);
			if(trainerid != null)this.trainerId	= trainerid;
			if(secretid != null) this.secretId	= secretid;
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
				foreach (KeyValuePair<GymBadges, DateTime?> g in gym)
					if (!GymsBeatTime.ContainsKey(g.Key))
						GymsBeatTime.Add(g.Key,g.Value);

			//byte[][] dex		= pokedex		?? new byte[Game.PokemonData.Where(x => x.Value.IsDefault).Count()][]; 
			//Pokedex = new byte[dex.Length,3];
			if(dex != null)
				for (int i = 0; i < dex.Length; i++)
					//Pokedex[i] = new byte[dex[i].Length];
					for (int j = 0; j < dex[i].Length; j++)
						Pokedex[i,j] = (byte)dex[i][j];
		}

		public Player(string name, bool gender, Pokemon[] party, Bag bag = null, PC pc = null, int? trainerid = null, int? secretid = null,
			int? money = null, int? coin = null, int? bank = null, int? repel = null, string rival = null,
			byte[][] dex = null, TimeSpan? time = null, Vector? position = null, byte? follower = null,
			bool? creator = null, int? map = null, int? pokecenter = null, KeyValuePair<GymBadges, DateTime?>[] gym = null)
			: this (name: name, gender: gender, party: party, bag: bag != null ? bag.Contents : null, 
				  pc_poke: pc != null ? pc.AllBoxes : null, pc_items: pc != null ? pc.Items : null, 
				  pc_names: pc != null ? pc.BoxNames : null, pc_textures: pc != null ? pc.BoxTextures : null, 
				  pc_box: pc != null ? pc.ActiveBox : (byte?)null, trainerid: trainerid, secretid: secretid,
				  money:money, coin:coin, bank:bank,repel:repel, rival:rival, dex: dex, time:time, position:position, follower:follower,
				  creator:creator, map:map, pokecenter:pokecenter, gym:gym)
		{
		}

		public Player(TrainerId trainer, Pokemon[] party = null) 
			: this (name: trainer.Name, gender: trainer.Gender, party: party, pc: null, trainerid: trainer.TrainerID, secretid: trainer.SecretID)
		{
		}
		#endregion

		#region Methods
		/// <summary>
		/// Skims every available box player has, and attempts to add pokemon.
		/// </summary>
		/// <param name="pokemon"></param>
		/// <returns>returns storage location of caught pokemon</returns>
		public int? addPokemon(Pokemon pokemon)
		{
			//attempt to add to party first. pack the party array if space available.
			if (Party.HasSpace(Party.Length))
			{
				Party.PackParty();
				Party[Party.Length - 1] = pokemon;
				Party.PackParty();
				return -1; //true
			}
			else
				//attempt to add to the earliest available PC box. 
				for (int numOfBoxes = 0, curBox = PC.ActiveBox; numOfBoxes < PC.AllBoxes.Length; numOfBoxes++, curBox++)
				{
					bool added = PC[(byte)(curBox % Core.STORAGEBOXES)].addPokemon(pokemon);
					if (added)
						//Returns the box pokemon was stored to
						return curBox; //true
					if (!Game.GameData.Features.OverflowPokemonsIntoNextBox) break; //else PC.ActiveBox = curBox; //change active box too?
				}
			//Could not be stored in PC because all boxes full
			return null;
		}

		/// <summary>
		/// Checks if the player is able to perform a certain Hidden Machine move.
		/// </summary>
		/// <param name="HM">The Hidden Machine move the player tries to use.</param>
		//ToDO: Redo this whole thing... hidden move checklist changes based on the HM and usage
		public bool CanUseHMMove(HiddenMoves HM)
		{
			//int[] trainerBadges = Game.GameData.Player.Badges;
			foreach (GymBadges b in Badges)
			{
				//if (curBox.HMs.Contains(HM) & Badges.Contains(curBox.ID) | curBox.ID == 0)
				if (Game.HiddenMoveData[HM].Badge == b)
					return true;
			}
			return false;
		}
		#endregion
	}
}