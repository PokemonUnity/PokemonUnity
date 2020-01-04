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
		/// Please use the values stored in <see cref="Trainer.TrainerID"/>
		/// </summary>
		private int? trainerId { get; set; }
		/// <summary>
		/// Please use the values stored in <see cref="Trainer.SecretID"/>
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
		public Trainer Trainer { get { return new Trainer(TrainerTypes.PLAYER, name: Name, gender: IsMale, party: Party, items: Bag.Contents, tID: trainerId, sID: secretId); } }
		/// <summary>
		/// When displaying items in bag, do a foreach loop and filter by item category
		/// </summary>
		public Bag Bag { get; private set; }//{ get { return new Bag(this.Items); } }
		public PC PC { get; private set; }//{ get { return new Game.TrainerPC(this, ActivePcBox); } }

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
		public Player()
		{
			//playerPokedex = new bool?[Pokemon.PokemonData.Database.Length];
			//Pokedex = new byte[Game.PokemonData.Where(x => x.Value.IsDefault).Count(), 3];
			//PlayTime = new TimeSpan();
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
			//GymsBeatTime = new Dictionary<GymBadges, DateTime?>();
			//foreach (GymBadges i in (GymBadges[])Enum.GetValues(typeof(GymBadges)))
			//{
			//	//gymBadges.Add(i);
			//	GymsBeatTime.Add(i, null);
			//}
			//gymsEncountered = new bool[gymBadges.Count];
			//gymsBeaten = new bool[gymBadges.Count];
			//gymsBeatTime = new System.DateTime?[gymBadges.Count];
			//GymsBeatTime = new System.DateTime?[gymBadges.Count];
		}

		public Player(string name, bool gender, Pokemon[] party = null, Items[] bag = null, Pokemon[][] pc_poke = null, KeyValuePair<Items,int>[] pc_items = null, byte? pc_box = null, string[] pc_names = null, int[] pc_textures = null, int? trainerid = null, int? secretid = null) : this()
		{
			Name = name;
			IsMale = gender;
			Party = party ?? Party;
			Bag = new Bag(bag);
			PC = new PC(pkmns: pc_poke, items: pc_items, box: pc_box, names: pc_names, textures: pc_textures);
			this.trainerId = trainerid;
			this.secretId = secretid;
		}

		public Player(string name, bool gender, Pokemon[] party = null, Bag bag = null, PC pc = null)
			: this(name: name, gender: gender, party: party, bag: bag != null ? bag.Contents : null, 
				  pc_poke: pc != null ? pc.AllBoxes : null, pc_items: pc != null ? pc.Items : null, 
				  pc_names: pc != null ? pc.BoxNames : null, pc_textures: pc != null ? pc.BoxTextures : null, 
				  pc_box: pc != null ? pc.ActiveBox : (byte?)null)
		{
		}

		//public Player(Trainer trainer, Pokemon[] party = null) 
		//	: this (name: trainer.Name, gender: trainer.Gender.Value, party: party ?? trainer.Party)
		//{
		//	trainerId = trainer.TrainerID;
		//	secretId = trainer.SecretID;
		//}
		#endregion

		#region Methods
		//public void LoadTrainer(Player trainerSaveData) { }
		public void LoadTrainer(PokemonUnity.Saving.SaveData trainerSaveData)
		{
			trainerId = trainerSaveData.TrainerID;
			secretId = trainerSaveData.SecretID;
			Name = trainerSaveData.PlayerName;
			IsMale = trainerSaveData.IsMale;
			//mapName = trainerSaveData.ActiveScene;
			//ActivePcBox = trainerSaveData.ActivePcBox;
			//playerPosition = trainerSaveData.PlayerPosition;
			//playerDirection = trainerSaveData.PlayerDirection;
			//Checkpoint = (Locations)trainerSaveData.PokeCenterId;
			//Money = trainerSaveData.PlayerMoney;// > Core.MAXMONEY ? Core.MAXMONEY : trainerSaveData.PlayerMoney;
			//Coins = trainerSaveData.PlayerCoins;// > Core.MAXCOINS ? Core.MAXCOINS : trainerSaveData.PlayerCoins;
			//Pokedex = trainerSaveData.Pokedex2; 
			//PlayTime = trainerSaveData.PlayTime;
			//GymsBeatTime = trainerSaveData.GymsChallenged;
			//Pokedex2 = new byte[dex2.GetLength(0)][];
			//for (int i = 0; i < Pokedex2.GetLength(0); i++)
			//{
			//	Pokedex2[i] = new byte[dex2.GetLength(1)];
			//	for (int j = 0; j < Pokedex2.GetLength(1); j++)
			//	{
			//		Pokedex2[i][j] = (byte)dex2[i, j];
			//	}
			//}
			//int FirstDim = trainerSaveData.Pokedex2.Length;
			//int SecondDim = trainerSaveData.Pokedex2.GroupBy(row => row.Length).Single().Key;
			//Pokedex = new byte[trainerSaveData.Pokedex2.GetLength(0),trainerSaveData.Pokedex2.GetLength(1)];
			//Pokedex = new byte[FirstDim, SecondDim];
			//for (int i = 0; i < FirstDim; ++i)
			//	for (int j = 0; j < SecondDim; ++j)
			//		Pokedex[i, j] = trainerSaveData.Pokedex2[i][j];
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
			//else
			//	//attempt to add to the earliest available PC box. 
			//	for (int i = 0, b = Game.ActivePcBox; i < Game.PC_Poke.GetUpperBound(0); i++, b++)
			//	{
			//		bool added = this.PC[b % Core.STORAGEBOXES].addPokemon(pokemon);
			//		if (added)
			//		{
			//			return b; //true
			//		}
			//	}
			return null;
		}
		#endregion
	}
}