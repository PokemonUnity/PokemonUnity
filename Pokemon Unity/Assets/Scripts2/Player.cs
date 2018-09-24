using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Pokemon;
using PokemonUnity.Move;
using PokemonUnity.Item;

[Serializable]
public class Player
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

	Player()
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

	public void LoadTrainer(Player trainerSaveData)
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