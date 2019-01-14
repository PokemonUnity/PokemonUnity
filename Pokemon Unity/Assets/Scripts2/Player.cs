using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Pokemon;
using PokemonUnity.Attack;
using PokemonUnity.Item;

[Serializable]
public class Player
{
	#region Variables
	//public int 
	//public Pokemon[] Party { get; private set; }
	public Trainer Trainer { get { return new Trainer(this); } }
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
	/// <para></para>
	/// <code>playerPokedex[1,2] == 3; means the 3rd form of pokemonId was first to be scanned into pokedex</code>
	/// </summary>
	/// <remarks>Or can be int?[pokedex.count,1]. if null, not seen or captured</remarks>
	public byte[,] playerPokedex2 = new byte[Pokemon.PokemonData.Database.Length, 3];
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
	public int PokedexCaught { get { return (from int index in Enumerable.Range(0, playerPokedex2.GetUpperBound(0)) select playerPokedex2[index, 1] == 1).Count(); } }
	public int PokedexSeen { get { return (from int index in Enumerable.Range(0, playerPokedex2.GetUpperBound(0)) select playerPokedex2[index, 0] == 1).Count(); } }

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
	[Obsolete]
    public bool[,] gymsEncountered { get; private set; }
	/// <summary>
	/// if <see cref="gymsBeatTime"/> is null, then value is false.
	/// </summary>
	/// <remarks>This isnt needed...</remarks>
	[Obsolete]
	public bool[,] gymsBeaten { get; private set; }
	public int BadgesCount { get { return (from gyms in GymsBeatTime where gyms.Value.HasValue select gyms).Count(); } }
	[Obsolete]
	public System.DateTime?[,] gymsBeatTime { get; private set; }
	/// <summary>
	/// Each Badge in <see cref="GymBadges"/> is a Key/Value,
	/// regardless of how they're set in game. One value per badge.
	/// </summary>
	//public System.DateTime?[] GymsBeatTime { get; private set; }
	public Dictionary<GymBadges, System.DateTime?> GymsBeatTime { get; private set; }
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
		//Party = new Pokemon[6];

		//List<GymBadges> gymBadges = new List<GymBadges>();
		foreach(GymBadges i in (GymBadges[])Enum.GetValues(typeof(GymBadges)))
		{
			//gymBadges.Add(i);
			GymsBeatTime.Add(i, null);
		}
		//gymsEncountered = new bool[gymBadges.Count];
		//gymsBeaten = new bool[gymBadges.Count];
		//gymsBeatTime = new System.DateTime?[gymBadges.Count];
		//GymsBeatTime = new System.DateTime?[gymBadges.Count];
	}

	public void LoadTrainer(Player trainerSaveData)
	{

	}

	#region Methods
	/// <summary>
	/// Skims every available box player has, and attempts to add pokemon.
	/// </summary>
	/// <param name="pokemon"></param>
	/// <returns>returns storage location of caught pokemon</returns>
	public int? addPokemon(Pokemon pokemon)
	{
		//attempt to add to party first. pack the party array if space available.
		if (Trainer.Party.HasSpace(Trainer.Party.Length))
		{
			Trainer.Party.PackParty();
			Trainer.Party[Trainer.Party.Length - 1] = pokemon;
			Trainer.Party.PackParty();
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
namespace PokemonUnity
{	
}
