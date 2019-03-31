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

namespace PokemonUnity
{	
	[Serializable]
	public class Player
	{
		#region Variables
		/// <summary>
		/// Please use the values stored in <see cref="Trainer.TrainerID"/>
		/// </summary>
		private int? trainerId { get; set; }
		/// <summary>
		/// Please use the values stored in <see cref="Trainer.SecretID"/>
		/// </summary>
		private int? secretId { get; set; }
		/// <summary>
		/// Player's Pokemon Party is stored in Player class, 
		/// and then reflected in Trainer, to match what occurs
		/// </summary>
		/// Didn't think about it it till now but the player should
		/// hold the `Trainer` data, and instantiate a new Trainer
		/// whenever it's needed...
		public Pokemon[] Party { get; private set; }
		public Trainer Trainer { get { return new Trainer(this, /*name: PlayerName, gender: isMale,*/ party: Party, tID: trainerId, sID: secretId); } }
		/// <summary>
		/// When displaying items in bag, do a foreach loop and filter by item category
		/// </summary>
		public Game.TrainerBag Bag { get { return new Game.TrainerBag(this); } }
		public Game.TrainerPC PC { get { return new Game.TrainerPC(this, ActivePcBox); } }
		//ToDo: Missing (Trainer)Player.Rival variable
		public string ItemData { get; set; }
		public string BerryData { get; set; }
		public string NPCData { get; set; }
		public string ApricornData { get; set; }
		public UnityEngine.Vector3 LastPokemonPosition { get; set; }
		public bool SandBoxMode { get; set; }
		public bool IsRunning() { return false; }
		public bool startSurfing { get; set; }
		public int RepelSteps { get; set; }
		public int SurfPokemon { get { int i = 0; foreach (Pokemon p in Party) if (p.knowsMove(Moves.SURF)) i++; return i; } }
		public Pokemon GetWalkPokemon() { return null; }

		#region UI/Game Engine
		public int mapName { get; set; }
		//public int levelName;
		public SeriV3 playerPosition { get; set; }
		/// <summary>
		/// Rotation of player model in overworld scene
		/// </summary>
		/// Might be useful if the game is in 2d, but if in 3d... will need to use x,y... dont need 3rd axis
		/// scratch that... only need rotation on single quantization axis...
		public float playerDirection { get; set; }
		public int respawnScene { get; set; }
		//public SeriV3 respawnScenePosition;
		//public int respawnSceneDirection;
		#endregion

		#region Player Records
		public string PlayerName { get; private set; }
		public string RivalName { get; private set; }
		/// <summary>
		/// </summary>
		/// ToDo: consider create AddMoney(value)... 
		public int PlayerMoney { get { return playerMoney; } set { playerMoney = value > Settings.MAXMONEY ? Settings.MAXMONEY : value; } }
		public int PlayerCoins { get { return playerCoins; } set { playerCoins = value > Settings.MAXCOINS ? Settings.MAXCOINS : value; } }
		private int playerMoney { get; set; }
		private int playerCoins { get; set; }
		private int playerSavings { get; set; }
		public bool isMale { get; private set; }
		public int ActivePcBox { get; private set; }

		/// <summary>
		/// Usage:<para>
		/// <code>playerPokedex[1,0] == 0; means pokemonId #1 not seen</code>
		/// </para>
		/// <code>playerPokedex[1,1] == 0; means pokemonId #1 not captured</code>
		/// <para></para>
		/// <code>playerPokedex[1,2] == 3; means the 3rd form of pokemonId was first to be scanned into pokedex</code>
		/// </summary>
		/// <remarks>Or can be int?[pokedex.count,1]. if null, not seen or captured</remarks>
		/// ToDo: Add variable for "Shiny"?...
		public byte[,] PlayerPokedex { get; private set; }
		///// <summary>
		///// Usage:<para>
		///// <code>playerPokedex[1] == false; means pokemonId #1 has been seen, and not captured</code>
		///// </para>
		///// <code>playerPokedex[1] == true; means pokemonId #1 has been captured</code>
		///// </summary>
		///// <remarks>if null, has not been seen or captured</remarks> 
		///// bool?[pokedexId][formId] = not encounted/null, seen/false, captured/true 
		//public bool?[] playerPokedex { get; set; }
		//public int pokedexCaught { get { return (from caught in playerPokedex where caught == true select caught).Count(); } }
		//public int pokedexSeen  { get { return (from seen in playerPokedex where seen != null select seen).Count(); } }
		public int PokedexCaught { get { return (from int index in Enumerable.Range(0, PlayerPokedex.GetUpperBound(0)) where PlayerPokedex[index, 1] == 1 select PlayerPokedex[index, 1]).Count(); } }
		public int PokedexSeen { get { return (from int index in Enumerable.Range(0, PlayerPokedex.GetUpperBound(0)) where PlayerPokedex[index, 0] == 1 select PlayerPokedex[index, 0]).Count(); } }
		//ToDo: Adventure Start Date
		public System.TimeSpan playerTime { get; private set; }
		//public int playerHours;
		//public int playerMinutes;
		//public int playerSeconds;

		///// <summary>
		///// Multiple Gens/Regions can be looked-up using
		///// </summary>
		///// <remarks>I thought there were only 8 badges?</remarks>
		///// ToDo: Array[Region/MapId,GymBadge] / or Array[i,8]
		///// gymsEncountered[1,5] == 2nd gen/region, 6th gym badge
		//[Obsolete]
		//public bool[,] gymsEncountered { get; private set; }
		///// <summary>
		///// if <see cref="gymsBeatTime"/> is null, then value is false.
		///// </summary>
		///// <remarks>This isnt needed...</remarks>
		//[Obsolete]
		//public bool[,] gymsBeaten { get; private set; }
		public int BadgesCount { get { return (from gyms in GymsBeatTime where gyms.Value.HasValue select gyms).Count(); } }
		//[Obsolete]
		//public System.DateTime?[,] gymsBeatTime { get; private set; }
		//public System.DateTime?[] GymsBeatTime { get; private set; }
		/// <summary>
		/// Each Badge in <see cref="GymBadges"/> is a Key/Value,
		/// regardless of how they're set in game. One value per badge.
		/// </summary>
		public Dictionary<GymBadges, System.DateTime?> GymsBeatTime { get; private set; }
		//public Badge Badges { get; set; }
		public int[] Badges { get { return GymsBeatTime.Where(x => x.Value.HasValue).Select(x => (int)x.Key).ToArray(); } }
		#endregion

		#region Player Customization
		//ToDo: Is there a reason to store user's Favorite Pkmn?
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
		public Player()
		{
			//playerPokedex = new bool?[Pokemon.PokemonData.Database.Length];
			PlayerPokedex = new byte[Pokemon.PokemonData.Database.Length, 3];
			playerTime = new TimeSpan();
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
		}

		public Player(string name, bool gender, Pokemon[] party = null) : this()
		{
			PlayerName = name;
			isMale = gender;
			Party = party ?? Party;
		}

		public Player(Trainer trainer, Pokemon[] party = null) 
			: this (name: trainer.Name, gender: trainer.Gender.Value, party: party ?? trainer.Party)
		{
			trainerId = trainer.TrainerID;
			secretId = trainer.SecretID;
		}
		#endregion

		#region Methods
		//public void LoadTrainer(Player trainerSaveData) { }
		public void LoadTrainer(PokemonUnity.Saving.SaveData trainerSaveData)
		{
			trainerId = trainerSaveData.TrainerID;
			secretId = trainerSaveData.SecretID;
			mapName = trainerSaveData.ActiveScene;
			playerPosition = trainerSaveData.PlayerPosition;
			playerDirection = trainerSaveData.PlayerDirection;
			respawnScene = trainerSaveData.pCenterScene;
			PlayerMoney = trainerSaveData.PlayerMoney;
			PlayerCoins = trainerSaveData.PlayerCoins;
			PlayerPokedex = trainerSaveData.Pokedex2;
			PlayerName = trainerSaveData.PlayerName;
			playerTime = trainerSaveData.PlayerTime;
			isMale = trainerSaveData.IsMale;
			GymsBeatTime = trainerSaveData.GymsChallenged;
			//for (int i = 0; i < /*Game.Player.Trainer.*/Party.Length; i++)
			//{
			//	Party[i] = trainerSaveData.PlayerParty[i];
			//}
			Party = trainerSaveData.PlayerParty.Deserialize();
		}

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
				Party[Trainer.Party.Length - 1] = pokemon;
				Party.PackParty();
				return -1; //true
			}
			else
				//attempt to add to the earliest available PC box. 
				for (int i = 0, b = ActivePcBox; i < Game.PC_Poke.GetUpperBound(0); i++, b++)
				{
					bool added = this.PC[b % Settings.STORAGEBOXES].addPokemon(pokemon);
					if (added)
					{
						return b; //true
					}
				}
			return null;
		}
		#endregion
	}
}