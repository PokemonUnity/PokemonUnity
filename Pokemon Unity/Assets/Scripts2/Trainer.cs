using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Pokemon;
using PokemonUnity.Move;
using PokemonUnity.Item;

[Serializable]
public class Trainer
{
	#region Variables
	public Pokemon[] Party { get; private set; }
	/// <summary>
	/// When displaying items in bag, do a foreach loop and filter by item category
	/// </summary>
	public Trainer.TrainerBag Bag { get { return new Trainer.TrainerBag(this); } }
	public Trainer.TrainerPC PC { get { return new Trainer.TrainerPC(this); } }
	#region Private Records of Player Storage Data
	Pokemon[,] PC_Poke { get; set; }
	private string[] PC_boxNames { get; set; }
	private int[] PC_boxTexture { get; set; }
	private List<Item> PC_Items { get; set; }
	private List<Item> Bag_Items { get; set; }
	#endregion

	public int mapName;
	//public int levelName;
	public SeriV3 playerPosition;
	public int playerDirection;
	public int respawnScene;
	public SeriV3 respawnScenePosition;
	//public int respawnSceneDirection;

	#region Important player data
	/// <summary>
	/// IDfinal = IDtrainer + IDsecret × 65536
	/// </summary>
	/// <remarks>
	/// Should be private, use "get()" and perform math
	/// only the last six digits are used so the Trainer Card will display an ID No.
	/// </remarks>
	public int PlayerID { get { return TrainerID + SecretID * 65536; } }
	public int TrainerID { get; private set; }
	public int SecretID { get; private set; }
	#endregion

	#region Player Records
	public string PlayerName { get; private set; }
	/// <summary>
	/// 
	/// </summary>
	/// ToDo: consider create AddMoney(value)... 
	public int PlayerMoney { get { return playerMoney; } set { playerMoney = value > Settings.MAXMONEY ? Settings.MAXMONEY : value; } }
	public int PlayerCoins { get; set; }
	private int playerMoney { get; set; }
	private int playerCoins { get; set; }
	public bool isMale { get; private set; }

	/// <summary>
	/// Usage:<para>
	/// <code>playerPokedex[1,0] == 0; means pokemonId #1 not seen</code>
	/// </para>
	/// <code>playerPokedex[1,1] == 0; means pokemonId #1 not captured</code>
	/// </summary>
	/// <remarks>Or can be int?[pokedex.count,1]. if null, not seen or captured</remarks>
	public int[,] playerPokedex2 = new int[Pokemon.PokemonData.Database.Length, 2];
	/// <summary>
	/// Usage:<para>
	/// <code>playerPokedex[1] == false; means pokemonId #1 has been seen, and not captured</code>
	/// </para>
	/// <code>playerPokedex[1] == true; means pokemonId #1 has been captured</code>
	/// </summary>
	/// <remarks>if null, has not been seen or captured</remarks> 
	/// bool?[pokedexId][formId] = not encounted/null, seen/false, captured/true 
	public bool?[] playerPokedex = new bool?[Pokemon.PokemonData.Database.Length];
	public int pokedexCaught { get { return (from caught in playerPokedex where caught == true select caught).Count(); } }
	public int pokedexSeen  { get { return (from seen in playerPokedex where seen != null select seen).Count(); } }

    public System.TimeSpan playerTime { get; private set; }
    //public int playerHours;
    //public int playerMinutes;
    //public int playerSeconds;

    /// <summary>
    /// Multiple Gens/Regions can be looked-up using
    /// </summary>
    /// <remarks>I thought there were only 8 badges?</remarks>
    /// ToDo: Array[Region/MapId,GymBadge] / or Array[i,8]
    /// gymsEncountered[1,5] == 2nd gen/region, 6th gym badge
    public bool[,] gymsEncountered { get; private set; }
	/// <summary>
	/// if <see cref="gymsBeatTime"/> is null, then value is false.
	/// </summary>
	/// <remarks>This isnt needed...</remarks>
	public bool[,] gymsBeaten { get; private set; }
	public System.DateTime?[,] gymsBeatTime { get; private set; }
	#endregion

	#region Player Customization
	/// <summary>
	/// Active player design
	/// </summary>
	/// ToDo: Player outfits should be stored and loaded from the player PC?
	/// Rather than adding another variable for `Item` data...
	/// Not sure if player custom designs are an `Item` type or a custom enum...
	public int playerOutfit;
	public int playerScore;
	public int playerShirt;
	public int playerMisc;
	public int playerHat;
	#endregion
	#endregion

	static Trainer()
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

	Trainer()
	{
		Party = new Pokemon[6];
		PC_Poke = new Pokemon[Settings.STORAGEBOXES, 30];
		PC_boxNames = new string[Settings.STORAGEBOXES];
		PC_boxTexture = new int[Settings.STORAGEBOXES];

		PC_Items = new List<Item>();
		Bag_Items = new List<Item>();

		List<GymBadges> gymBadges = new List<GymBadges>();
		foreach(GymBadges i in (GymBadges[])Enum.GetValues(typeof(GymBadges)))
		{
			gymBadges.Add(i);
		}
		//gymsEncountered = new bool[gymBadges.Count];
		//gymsBeaten = new bool[gymBadges.Count];
		//gymsBeatTime = new System.DateTime?[gymBadges.Count];
	}

	public void LoadTrainer(Trainer trainerSaveData)
	{

	}

	#region Methods
	public void packParty()
	{
		Pokemon[] packedArray = new Pokemon[6];
		int i2 = 0; //counter for packed array
		for (int i = 0; i < 6; i++)
		{
			if (Party[i] != null || Party[i].Species != Pokemons.NONE)
			{
				//if next object in box has a value
				packedArray[i2] = Party[i]; //add to packed array
				i2 += 1; //ready packed array's next position
			}
		}
		Party = packedArray;
	}

	/// <summary>
	/// Skims every available box player has, and attempts to add pokemon.
	/// </summary>
	/// <param name="pokemon"></param>
	/// <returns>returns storage location of caught pokemon</returns>
	public int? addPokemon(Pokemon pokemon)
	{
		//attempt to add to party first. pack the party array if space available.
		if (hasSpace())
		{
			packParty();
			Party[Party.Length - 1] = pokemon;
			packParty();
			return -1; //true
		}
		else
			//attempt to add to the earliest available PC box. 
			for (int i = 1; i < PC_Poke.GetUpperBound(0); i++)
			{
				bool added = PC.addPokemon(pokemon);
				if (added)
				{
					return i; //true
				}
			}
		return null;
	}

	public bool hasSpace()
	{
		if (getPartyCount().HasValue && getPartyCount().Value < 30) return true;
		else return false;
	}

	public int? getPartyCount()
	{
		int result = 0;
		for (int i = 0; i < Party.Length; i++)
		{
			if (Party[i] != null || Party[i].Species != PokemonUnity.Pokemon.Pokemons.NONE)
			{
				result += 1;
			}
		}
		return result;
	}
	#endregion

	#region Nested Classes
	public class TrainerPC
	{
		//public static PC
		private Trainer trainer { get; set; }
		private int? activeBox { get; set; }
		public string Name { get; set; }
		public int Texture { get; set; }
		public Pokemon[] Pokemons { get; set; }
		/// <summary>
		/// </summary>
		/// ToDo: Add filter to add/remove items...
		public List<Item> Items { get { return trainer.PC_Items; } set { trainer.PC_Items = value; } }

		public TrainerPC this[int i] { get {
				this.activeBox = i;
				Pokemon[] p = new Pokemon[30];
				for (int t = 0; t < 30; t++)
				{
					p[t] = trainer.PC_Poke[i, t];
				}
				this.Pokemons = p;
				this.Texture = trainer.PC_boxTexture[i];
				this.Name = trainer.PC_boxNames[i] ?? "Box " + (i + 1).ToString();
				return this; } }

		public TrainerPC()
		{
		}

		public TrainerPC(Trainer t) : this()
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
				if (Pokemons[i] != null || Pokemons[i].Species != PokemonUnity.Pokemon.Pokemons.NONE)
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
				if (Pokemons[i] == null || Pokemons[i].Species == PokemonUnity.Pokemon.Pokemons.NONE)
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
				trainer.PC_Poke[activeBox.Value,getIndexOfFirstEmpty().Value] = acquiredPokemon;
				return true;
			}
			//if could not add a pokemon, return false. Party and PC are both full.
			return false;
		}

		public void swapPokemon(int box1, int pos1, int box2, int pos2)
		{
			Pokemon temp = trainer.PC_Poke[box1, pos1];
			trainer.PC_Poke[box1,pos1] = trainer.PC_Poke[box2,pos2];
			trainer.PC_Poke[box2,pos2] = temp;
		}
	}

	public class TrainerBag
	{
		private Trainer trainer { get; set; }
		/*// <summary>
		/// in combination with quantity[], 
		/// one holds the itemId and the other has amount
		/// </summary>
		/// <remarks>if use <see cref="Items"/> might be less on memory</remarks>
		/// <see cref="Items"/> stores quantity value
		//public List<Item> Items { get { return trainer.Bag_Items; } }*/
		public List<Item> Misc { get; private set; }
		public List<Item> Medicine { get; private set; }
		public List<Item> Pokeball { get; private set; }
		public List<Item> Machine { get; private set; }
		public List<Item> Berry { get; private set; }
		public List<Item> Mail { get; private set; }
		public List<Item> Battle { get; private set; }
		public List<Item> Key { get; private set; }
		private int[] quantity;

		public Item this[Items item]
		{
			get
			{
				return this.trainer.Bag_Items.FirstOrDefault(i => i.ItemId == item);
			}
		}

		public Item this[Item item]
		{
			get
			{
				return this[item.ItemId];
			}
		}

		public TrainerBag()
		{
			Misc = Medicine = Pokeball = Machine = Berry = Mail = Battle = Key = new List<Item>();
			//orderString = new string[ItemDatabaseOld.getItemsLength()];
			//quantity = new int[ItemDatabaseOld.getItemsLength()];
			foreach (Item item in trainer.Bag_Items)
			{
				switch (item.ItemPocket)
				{
					case ItemPockets.MISC:
						Misc.Add(item);
						break;
					case ItemPockets.MEDICINE:
						Medicine.Add(item);
						break;
					case ItemPockets.POKEBALL:
						Pokeball.Add(item);
						break;
					case ItemPockets.MACHINE:
						Machine.Add(item);
						break;
					case ItemPockets.BERRY:
						Berry.Add(item);
						break;
					case ItemPockets.MAIL:
						Mail.Add(item);
						break;
					case ItemPockets.BATTLE:
						Battle.Add(item);
						break;
					case ItemPockets.KEY:
						Key.Add(item);
						break;
					default:
						break;
				}
			}
		}

		public TrainerBag(Trainer t) : this()
		{
			trainer = t;
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
	#endregion
}
namespace PokemonUnity
{
	enum GymBadges
	{
		Rock
	}
}
