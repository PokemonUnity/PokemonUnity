using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Pokemon;
using PokemonUnity.Attack;
using PokemonUnity.Item;
using PokemonUnity.Saving.SerializableClasses;

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
	public Trainer Trainer { get { return new Trainer(this, /*name: PlayerName, gender: isMale, party: Party,*/ tID: trainerId, sID: secretId); } }
	/// <summary>
	/// When displaying items in bag, do a foreach loop and filter by item category
	/// </summary>
	public GameVariables.TrainerBag Bag { get { return new GameVariables.TrainerBag(this); } }
	public GameVariables.TrainerPC PC { get { return new GameVariables.TrainerPC(this); } }

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

	#region Player Records
	public string PlayerName { get; private set; }
	/// <summary>
	/// 
	/// </summary>
	/// ToDo: consider create AddMoney(value)... 
	public int PlayerMoney { get { return playerMoney; } set { playerMoney = value > Settings.MAXMONEY ? Settings.MAXMONEY : value; } }
	public int PlayerCoins { get { return playerCoins; } set { playerCoins = value > Settings.MAXCOINS ? Settings.MAXCOINS : value; } }
	private int playerMoney { get; set; }
	private int playerCoins { get; set; }
	public bool isMale { get; private set; }

	/// <summary>
	/// Usage:<para>
	/// <code>playerPokedex[1,0] == 0; means pokemonId #1 not seen</code>
	/// </para>
	/// <code>playerPokedex[1,1] == 0; means pokemonId #1 not captured</code>
	/// <para></para>
	/// <code>playerPokedex[1,2] == 3; means the 3rd form of pokemonId was first to be scanned into pokedex</code>
	/// </summary>
	/// <remarks>Or can be int?[pokedex.count,1]. if null, not seen or captured</remarks>
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
	public int PokedexCaught { get { return (from int index in Enumerable.Range(0, PlayerPokedex.GetUpperBound(0)) select PlayerPokedex[index, 1] == 1).Count(); } }
	public int PokedexSeen { get { return (from int index in Enumerable.Range(0, PlayerPokedex.GetUpperBound(0)) select PlayerPokedex[index, 0] == 1).Count(); } }

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

	static Player()
	{
/*TPSPECIES	,
TPLEVEL		,
TPITEM		,
TPMOVE1		,
TPMOVE2		,
TPMOVE3		,
TPMOVE4		,
TPABILITY	,
TPGENDER	,
TPFORM		,
TPSHINY		,
TPNATURE	,
TPIV		,
TPHAPPINESS ,
TPNAME		,
TPSHADOW	,
TPBALL		,
TPDEFAULTS = [0, 10, 0, 0, 0, 0, 0, nil, nil, 0, false, nil, 10, 70, nil, false, 0]*/
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
		//for (int i = 0; i < /*GameVariables.playerTrainer.Trainer.*/Party.Length; i++)
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
			for (int i = 1; i < GameVariables.PC_Poke.GetUpperBound(0); i++)
			{
				bool added = this.PC.addPokemon(pokemon);
				if (added)
				{
					return i; //true
				}
			}
		return null;
	}
	#endregion
}

#region Trainer PC
public partial class GameVariables
{
	public class TrainerPC
	{
		//public static PC
		private Player trainer { get; set; }
		private int? activeBox { get; set; }
		public string Name { get; set; }
		public int Texture { get; set; }
		public Pokemon[] Pokemons { get; set; }
		/// <summary>
		/// </summary>
		/// ToDo: Add filter to add/remove items...
		public List<Item> Items { get { return GameVariables.PC_Items; } set { GameVariables.PC_Items = value; } }

		public TrainerPC this[int i]
		{
			get
			{
				this.activeBox = i;
				Pokemon[] p = new Pokemon[30];
				for (int t = 0; t < 30; t++)
				{
					p[t] = GameVariables.PC_Poke[i, t];
				}
				this.Pokemons = p;
				this.Texture = GameVariables.PC_boxTexture[i];
				this.Name = GameVariables.PC_boxNames[i] ?? "Box " + (i + 1).ToString();
				return this;
			}
		}

		public TrainerPC()
		{
		}

		public TrainerPC(Player t) : this()
		{
			trainer = t;
		}

		public bool hasSpace()
		{
			if (getBoxCount().HasValue && getBoxCount().Value < 30) return true;
			else return false;
		}

		public int? getBoxCount()
		{
			int result = 0;
			for (int i = 0; i < Pokemons.Length; i++)
			{
				if (Pokemons[i] != null || Pokemons[i].Species != PokemonUnity.Pokemons.NONE)
				{
					result += 1;
				}
			}
			return result;
		}

		public int? getIndexOfFirstEmpty()
		{
			//int result = 0;
			for (int i = 0; i < Pokemons.Length; i++)
			{
				if (Pokemons[i] == null || Pokemons[i].Species == PokemonUnity.Pokemons.NONE)
				{
					return i;
				}
			}
			return null;
		}

		/*public int getBoxCount(int box)
		{
			int result = 0;
			for (int i = 0; i < Pokemons[box].Length; i++)
			{
				if (Pokemons[box,i] != null || Party[i].Species != Pokemons.NONE)
				{
					result += 1;
				}
			}
			return result;
		}*/


		/// <summary>
		/// Add a new pokemon directly to active box. 
		/// If pokemon could not be added return false.
		/// </summary>
		/// <param name="acquiredPokemon"></param>
		/// <returns></returns>
		public bool addPokemon(Pokemon acquiredPokemon)
		{
			//attempt to add to the earliest available opening in active box. no array packing needed.
			if (hasSpace())
			{
				//Pokemons[getIndexOfFirstEmpty().Value] = acquiredPokemon;
				GameVariables.PC_Poke[activeBox.Value, getIndexOfFirstEmpty().Value] = acquiredPokemon;
				return true;
			}
			//if could not add a pokemon, return false. Party and PC are both full.
			return false;
		}

		public void swapPokemon(int box1, int pos1, int box2, int pos2)
		{
			Pokemon temp = GameVariables.PC_Poke[box1, pos1];
			GameVariables.PC_Poke[box1, pos1] = GameVariables.PC_Poke[box2, pos2];
			GameVariables.PC_Poke[box2, pos2] = temp;
		}
	}

	public class TrainerBag
	{
		private Player trainer { get; set; }
		/*// <summary>
		/// in combination with quantity[], 
		/// one holds the itemId and the other has amount
		/// </summary>
		/// <remarks>if use <see cref="Items"/> might be less on memory</remarks>
		/// <see cref="Items"/> stores quantity value
		//public List<Item> Items { get { return trainer.Bag_Items; } }*/
		public SortedList<Item, byte> Misc { get; private set; }
		public SortedList<Item, byte> Medicine { get; private set; }
		public SortedList<Item, byte> Pokeball { get; private set; }
		public SortedList<Item, byte> Machine { get; private set; }
		public SortedList<Item, byte> Berry { get; private set; }
		public SortedList<Item, byte> Mail { get; private set; }
		public SortedList<Item, byte> Battle { get; private set; }
		public SortedList<Item, byte> Key { get; private set; }
		private int[] quantity { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		/// ToDo: Return KeyValuePair<Item, Quantity>?
		public Item this[Items item]
		{
			get
			{
				return PokemonUnity.Item.Item.GetItem(GameVariables.Bag_Items.FirstOrDefault(i => i == item));
			}
		}

		public Item this[Item item]
		{
			get
			{
				return this[item.ItemId];
			}
		}

		public void GetBag()
		{
			Misc = Medicine = Pokeball = Machine = Berry = Mail = Battle = Key = new SortedList<Item, byte>();
			List<Items> misc, medicine, pokeball, machine, berry, mail, battle, key;// = new List<Item>();
			misc = medicine = pokeball = machine = berry = mail = battle = key = new List<Items>();
			//orderString = new string[ItemDatabaseOld.getItemsLength()];
			quantity = new int[Bag_Items.Count];
			//foreach (KeyValuePair<Items, byte> Item in GameVariables.Bag_Items)
			foreach (Items Item in GameVariables.Bag_Items)
			{
				//Item item = global::Item.GetItem(Item.Key);
				Item item = PokemonUnity.Item.Item.GetItem(Item);
				switch (item.ItemPocket)
				{
					case ItemPockets.MISC:
						if (!misc.Contains(Item))
						{
							Misc.Add(item, (byte)GameVariables.Bag_Items.Count(i => i == Item));
							misc.Add(Item);
						}
						break;
					case ItemPockets.MEDICINE:
						if (!machine.Contains(Item))
						{
							Medicine.Add(item, (byte)GameVariables.Bag_Items.Count(i => i == Item));
							machine.Add(Item);
						}
						break;
					case ItemPockets.POKEBALL:
						if (!pokeball.Contains(Item))
						{
							Pokeball.Add(item, (byte)GameVariables.Bag_Items.Count(i => i == Item));
							pokeball.Add(Item);
						}
						break;
					case ItemPockets.MACHINE:
						if (!machine.Contains(Item))
						{
							Machine.Add(item, (byte)GameVariables.Bag_Items.Count(i => i == Item));
							machine.Add(Item);
						}
						break;
					case ItemPockets.BERRY:
						if (!berry.Contains(Item))
						{
							Berry.Add(item, (byte)GameVariables.Bag_Items.Count(i => i == Item));
							berry.Add(Item);
						}
						break;
					case ItemPockets.MAIL:
						if (!mail.Contains(Item))
						{
							Mail.Add(item, (byte)GameVariables.Bag_Items.Count(i => i == Item));
							mail.Add(Item);
						}
						break;
					case ItemPockets.BATTLE:
						if (!battle.Contains(Item))
						{
							Battle.Add(item, (byte)GameVariables.Bag_Items.Count(i => i == Item));
							battle.Add(Item);
						}
						break;
					case ItemPockets.KEY:
						if (!key.Contains(Item))
						{
							Key.Add(item, (byte)GameVariables.Bag_Items.Count(i => i == Item));
							key.Add(Item);
						}
						break;
					default:
						break;
				}
			}
		}

		public TrainerBag(Player t) : this()
		{
			trainer = t;
		}

		public TrainerBag()
		{
			Misc = Medicine = Pokeball = Machine = Berry = Mail = Battle = Key = new SortedList<Item, byte>();
			GetBag();
		}

		/*public int getIndexOf(Item name)
		{
			for (int i = 0; i < order.Length; i++)
			{
				if (order[i] == name)
				{
					return i;
				}
			}
			return -1;
		}

		public void moveBehind(int targetIndex, int destinationIndex)
		{
			string temp = orderString[targetIndex];
			string[] packedOrder = new string[orderString.Length];

			orderString[targetIndex] = null;

			if (Mathf.Abs(targetIndex - destinationIndex) == 1)
			{
				orderString[targetIndex] = orderString[destinationIndex];
				orderString[destinationIndex] = temp;
			}
			else
			{
				int packedIndex = 0;
				for (int i = 0; i < orderString.Length; i++)
				{
					if (i == destinationIndex)
					{
						packedOrder[packedIndex] = temp;
						packedIndex += 1;
					}
					if (orderString[i] != null)
					{
						packedOrder[packedIndex] = orderString[i];
						packedIndex += 1;
					}
				}
				orderString = packedOrder;
			}
		}

		public void packOrder()
		{
			string[] packedOrder = new string[orderString.Length];
			int packedIndex = 0;
			for (int i = 0; i < orderString.Length; i++)
			{
				if (orderString[i] != null)
				{
					packedOrder[packedIndex] = orderString[i];
					packedIndex += 1;
				}
			}
			orderString = packedOrder;
		}

		///ToDo: Add to global bag, then use GetBag to pack and sort
		/// Max int count for a byte value is 255... Bag can hold more, but will only display max of 255?
		public bool addItem(Item itemName)
		{
			//returns false if will exceed the quantity limit (999)
			//packOrder();
			//string name = ItemDatabase.getItem(itemName).getName(); //ensures that the name is correct
			int index = getIndexOf(itemName);
			if (index == -1)
			{
				//item does not exist in bag, add it to the end
				index = getLength();
				order[index] = itemName;
			}
			index = Item.getIndexOf(order[index]);
			if (quantity[index] + itemName.Quantity > 999)
			{
				return false;
			}
			quantity[index] += amount;
			return true;
		}

		public bool addItem(Items itemName, int amount = 1)
		{
			//returns false if will exceed the quantity limit (999)
			packOrder();
			//string name = ItemDatabase.getItem(itemName).getName(); //ensures that the name is correct
			int index = getIndexOf(itemName);
			if (index == -1)
			{
				//item does not exist in bag, add it to the end
				index = getLength();
				order[index] = ItemDatabaseOld.getItem(itemName);
			}
			index = ItemDatabaseOld.getIndexOf(order[index]);
			if (quantity[index] + amount > 999)
			{
				return false;
			}
			quantity[index] += amount;
			return true;
		}

		public bool removeItem(Item itemName, int amount = 1) //ToDo: amount should default to 'ALL'
		{
			//returns false if trying to remove more items than exist
			packOrder();
			//string name = ItemDatabase.getItem(itemName).getName(); //ensures that the name is correct
			int index = getIndexOf(itemName);
			if (index == -1)
			{
				//item does not exist in bag
				return false;
			}
			index = ItemDatabaseOld.getIndexOf(orderString[index]);
			if (quantity[index] - amount < 0)
			{
				return false;
			}
			quantity[index] -= amount;
			if (quantity[index] == 0)
			{
				orderString[getIndexOf(itemName)] = null;
				packOrder();
			}
			return true;
		}

		public bool removeItem(Items itemName, int amount = 1)  //ToDo: amount should default to 'ALL'
		{
			//returns false if trying to remove more items than exist
			packOrder();
			//string name = ItemDatabase.getItem(itemName).getName(); //ensures that the name is correct
			int index = getIndexOf(itemName);
			if (index == -1)
			{
				//item does not exist in bag
				return false;
			}
			index = ItemDatabaseOld.getIndexOf(orderString[index]);
			if (quantity[index] - amount < 0)
			{
				return false;
			}
			quantity[index] -= amount;
			if (quantity[index] == 0)
			{
				orderString[getIndexOf(itemName)] = null;
				packOrder();
			}
			return true;
		}*/
	}
}
#endregion

namespace PokemonUnity
{	
}