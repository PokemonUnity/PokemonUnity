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
	//public int 
	public Pokemon[] Party { get; private set; }
	/// <summary>
	/// When displaying items in bag, do a foreach loop and filter by item category
	/// </summary>
	public GameVariables.TrainerBag Bag { get { return new GameVariables.TrainerBag(this); } }
	public GameVariables.TrainerPC PC { get { return new GameVariables.TrainerPC(this); } }

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
}
namespace PokemonUnity
{
	public enum TrainerTypes
	{
		/// <summary>
		/// Custom designs or just generic character played by another user
		/// </summary>
		PLAYER,
		POKEMONTRAINER_Red,
		POKEMONTRAINER_Leaf,
		POKEMONTRAINER_Brendan,
		POKEMONTRAINER_May,
		RIVAL1,
		RIVAL2,
		AROMALADY,
		BEAUTY,
		BIKER,
		BIRDKEEPER,
		BUGCATCHER,
		BURGLAR,
		CHANELLER,
		CUEBALL,
		ENGINEER,
		FISHERMAN,
		GAMBLER,
		GENTLEMAN,
		HIKER,
		JUGGLER,
		LADY,
		PAINTER,
		POKEMANIAC,
		POKEMONBREEDER,
		PROFESSOR,
		ROCKER,
		RUINMANIAC,
		SAILOR,
		SCIENTIST,
		SUPERNERD,
		TAMER,
		BLACKBELT,
		CRUSHGIRL,
		CAMPER,
		PICNICKER,
		COOLTRAINER_M,
		COOLTRAINER_F,
		YOUNGSTER,
		LASS,
		POKEMONRANGER_M,
		POKEMONRANGER_F,
		PSYCHIC_M,
		PSYCHIC_F,
		SWIMMER_M,
		SWIMMER_F,
		SWIMMER2_M,
		SWIMMER2_F,
		TUBER_M,
		TUBER_F,
		TUBER2_M,
		TUBER2_F,
		COOLCOUPLE,
		CRUSHKIN,
		SISANDBRO,
		TWINS,
		YOUNGCOUPLE,
		TEAMROCKET_M,
		TEAMROCKET_F,
		ROCKETBOSS,
		LEADER_Brock,
		LEADER_Misty,
		LEADER_Surge,
		LEADER_Erika,
		LEADER_Koga,
		LEADER_Sabrina,
		LEADER_Blaine,
		LEADER_Giovanni,
		ELITEFOUR_Lorelei,
		ELITEFOUR_Bruno,
		ELITEFOUR_Agatha,
		ELITEFOUR_Lance,
		CHAMPION
	}

	enum GymBadges
	{
		Rock
	}
	
	/*// <summary>
	/// Extension methods for <see cref="MenuItemDefinition"/>.
	/// </summary>
	public static class MenuItemDefinitionExtensions
	{
		/// <summary>
		/// Moves a menu item to top in the list.
		/// </summary>
		/// <param name="menuItems">List of menu items</param>
		/// <param name="menuItemName">Name of the menu item to move</param>
		public static void MoveMenuItemToTop(this IList<MenuItemDefinition> menuItems, string menuItemName)
		{
			var menuItem = GetMenuItem(menuItems, menuItemName);
			menuItems.Remove(menuItem);
			menuItems.Insert(0, menuItem);
		}

		/// <summary>
		/// Moves a menu item to bottom in the list.
		/// </summary>
		/// <param name="menuItems">List of menu items</param>
		/// <param name="menuItemName">Name of the menu item to move</param>
		public static void MoveMenuItemToBottom(this IList<MenuItemDefinition> menuItems, string menuItemName)
		{
			var menuItem = GetMenuItem(menuItems, menuItemName);
			menuItems.Remove(menuItem);
			menuItems.Insert(menuItems.Count, menuItem);
		}

		/// <summary>
		/// Moves a menu item in the list after another menu item in the list.
		/// </summary>
		/// <param name="menuItems">List of menu items</param>
		/// <param name="menuItemName">Name of the menu item to move</param>
		/// <param name="targetMenuItemName">Target menu item (to move before it)</param>
		public static void MoveMenuItemBefore(this IList<MenuItemDefinition> menuItems, string menuItemName, string targetMenuItemName)
		{
			var menuItem = GetMenuItem(menuItems, menuItemName);
			var targetMenuItem = GetMenuItem(menuItems, targetMenuItemName);
			menuItems.Remove(menuItem);
			menuItems.Insert(menuItems.IndexOf(targetMenuItem), menuItem);
		}

		/// <summary>
		/// Moves a menu item in the list before another menu item in the list.
		/// </summary>
		/// <param name="menuItems">List of menu items</param>
		/// <param name="menuItemName">Name of the menu item to move</param>
		/// <param name="targetMenuItemName">Target menu item (to move after it)</param>
		public static void MoveMenuItemAfter(this IList<MenuItemDefinition> menuItems, string menuItemName, string targetMenuItemName)
		{
			var menuItem = GetMenuItem(menuItems, menuItemName);
			var targetMenuItem = GetMenuItem(menuItems, targetMenuItemName);
			menuItems.Remove(menuItem);
			menuItems.Insert(menuItems.IndexOf(targetMenuItem) + 1, menuItem);
		}

		private static MenuItemDefinition GetMenuItem(IEnumerable<MenuItemDefinition> menuItems, string menuItemName)
		{
			var menuItem = menuItems.FirstOrDefault(i => i.Name == menuItemName);
			if (menuItem == null)
			{
				throw new Exception("Can not find menu item: " + menuItemName);
			}

			return menuItem;
		}
	}*/
}
