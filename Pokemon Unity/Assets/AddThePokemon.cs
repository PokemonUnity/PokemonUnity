using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PokemonUnity;
using PokemonUnity.Monster;

/// <summary>
/// </summary>
/// https://gist.github.com/MyzTyn/ca7fd014619069d7be6d16f35bd6fb27
/// https://gist.github.com/MyzTyn/7023d3c62f6c859ecaa117567e13f5e8
[ExecuteInEditMode]
public class AddThePokemon : MonoBehaviour
{
	/// <summary>
	/// For starter pokemon
	/// </summary>
	public Pokemons Starters;
	//Byte
	public byte starterlevel = 5;
	//Bool
	bool starterpokemonchoice = true;
	bool Message = true;
	//END
	public Pokemon pokemon = null;
	public Pokemon pkmn 
	{
		set
		{
			if (value != null)
			{
				//Display Unity's UI text for Pokemon Name
				//names.text = value.Name; //Name is already string, dont need `ToString()`
				Name = string.Format("{0}{1}", value.Name[0], value.Name.Substring(1).ToLowerInvariant()); 
				//Convert from int to string and Display Unity's text for Health
				//current.text = value.HP.ToString();
				hp = value.HP;
				//healths.text = value.TotalHP.ToString();
				maxhp = value.TotalHP;
				//Convert from int to string and Display Unity's text for Level
				//levels.text = value.Level.ToString();
				level = value.Level;
				move1 = value.moves[0].MoveId;
				move2 = value.moves[1].MoveId;
				move3 = value.moves[2].MoveId;
				move4 = value.moves[3].MoveId;
				IV1 = value.IV[0];
				IV2 = value.IV[1];
				IV3 = value.IV[2];
				IV4 = value.IV[3];
				IV5 = value.IV[4];
				IV6 = value.IV[5];
				ATK = value.ATK;
				DEF = value.DEF;
				SPA = value.SPA;
				SPD = value.SPD;
				SPE = value.SPE;
				type1 = value.Type1;
				type2 = value.Type2;
				ability = value.Ability;
				nature = value.Nature;
				gender = value.Gender == true ? "male" : (value.Gender == false ? "female" : null);
				exp = value.Exp;
				eggsteps = value.EggSteps;
			}
			else
			{
				//Display Unity's UI text for Pokemon Name
				//names.text = null;
				Name = null;
				//Convert from int to string and Display Unity's text for Health
				//current.text = null;
				hp = 0;
				//healths.text = null;
				maxhp = 0;
				//Convert from int to string and Display Unity's text for Level
				//levels.text = null;
				level = 0;
				move1 = Moves.NONE;
				move2 = Moves.NONE;
				move3 = Moves.NONE;
				move4 = Moves.NONE;
			}
		}
	}
	// Only serialized values are displayed in the inspector...
	//public PokemonUnity.Saving.SerializableClasses.SeriPokemon seriPokemon = null;
	//Strings and UI
	/// <summary>
	/// For display Name
	/// </summary>
	public Text names;
	public string Name; //inspector
	/// <summary>
	/// For display Health
	/// </summary>
	public Text current;
	public int hp; //inspector
	/// <summary>
	/// For display Health
	/// </summary>
	public Text healths;
	public int maxhp; //inspector
	/// <summary>
	/// For display Level
	/// </summary>
	public Text levels;
	public int level; //inspector
	public int exp; //inspector
	public int eggsteps; //inspector
	public string gender; //inspector
	public Natures nature; //inspector
	public Abilities ability; //inspector
	public PokemonUnity.Types type1; //inspector
	public PokemonUnity.Types type2; //inspector
	public Moves move1; //inspector
	public Moves move2; //inspector
	public Moves move3; //inspector
	public Moves move4; //inspector
	public int IV1; //inspector
	public int IV2; //inspector
	public int IV3; //inspector
	public int IV4; //inspector
	public int IV5; //inspector
	public int IV6; //inspector
	public int ATK; //inspector
	public int DEF; //inspector
	public int SPA; //inspector
	public int SPD; //inspector
	public int SPE; //inspector
	
    public void starterpokemon()
    {
        if (Input.GetKeyDown(KeyCode.Q)) //If user input the keyboard(Q)
        {
            Starters = Pokemons.EEVEE; //Use Eevee as starter pokemon
            pokemonChoices();
        }
        if (Input.GetKeyDown(KeyCode.W)) //If user input the keyboard(W)
        {               
            Starters = Pokemons.PIKACHU; //Use Pikachu as starter pokemon
            pokemonChoices();
        }
        if (Input.GetKeyDown(KeyCode.E)) //If user input the keyboard(W)
        {               
            //Starters = Pokemons.PIKACHU; //Use INSPECTOR as starter pokemon
            pokemonChoices();
        }
    }
    void pokemonChoices()
    {
        Debug.Log("button pressed");
        pokemon = new Pokemon(Starters, level: starterlevel, true); //To create Pokemon
		pokemon.HatchEgg();
        Debug.Log("You had pick " + pokemon.Name);
        //names.text = pokemon.Name; //Display Unity's UI text for Pokemon Name
        //int exp = PokemonUnity.Monster.Data.Experience.GetStartExperience(pokemon.GrowthRate, starterlevel);
        //Debug.Log(exp);
		//pokemon.Exp = exp;
		//pokemon.AddExperience(exp, false);
        //pokemon.Heal(); //Heal the Pokemon
		Debug.Log(pokemon.Name + ": " + pokemon.HP + "/" + pokemon.TotalHP);
        //healths.text = pokemon.HP.ToString(); //Convert from int to string and Display Unity's text for Health
        //levels.text = pokemon.Level.ToString(); //Convert from int to string and Display Unity's text for Level
		pkmn = pokemon;
        starterpokemonchoice = false;
    }
	
	void Awake()
	{
		Debug.Log("waking up...");
		//Debug.Log(System.IO.File.Exists(@"..\..\..\\veekun-pokedex.sqlite"));
		//Debug.Log(System.IO.Path.GetFullPath(@"."));
		//Debug.Log(System.IO.Path.GetFullPath(@"..\"));
		//Debug.Log(System.IO.Path.GetFullPath(@"..\..\"));
		//Debug.Log(System.IO.Path.GetFullPath(@"..\..\..\"));
		//PokemonUnity.Game.InitTypes();
		//PokemonUnity.Game.InitNatures();
		//Debug.Log("Pokemons loaded: " + PokemonUnity.Game.InitPokemons());
		//PokemonUnity.Game.InitPokemonForms();
		//PokemonUnity.Game.InitPokemonMoves();
		//PokemonUnity.Game.InitPokemonEvolutions();
		//PokemonUnity.Game.InitPokemonItems();
		//Debug.Log("Moves loaded: " + PokemonUnity.Game.InitMoves());
		//Debug.Log("Items loaded: " + PokemonUnity.Game.InitItems());
		//PokemonUnity.Game.InitBerries();
		//PokemonUnity.Game.InitTrainers();
		//PokemonUnity.Game.InitRegions();
		//PokemonUnity.Game.InitLocations();
		Game initializeNewGame = new Game();
		Debug.Log("Awake!");
		//Debug.Log("Pokemon Database Count: " + Game.PokemonData.Count);
		//Debug.Log("Move Database Count: " + Game.MoveData.Count);
		//Debug.Log("Item Database Count: " + Game.ItemData.Count);
	}

	private void GameDebug_OnDebug(object sender, GameDebug.OnDebugEventArgs e)
	{
		if (e.Error == true)
			Debug.LogError(e.Message);
		else if (e.Error == false)
			Debug.LogWarning(e.Message);
		else
			Debug.Log(e.Message);
	}

	// Start is called before the first frame update
	void Start()
	{
		Debug.Log("Start!");
	}

	void OnEnable()
	{
		Debug.Log("Enabled!");
		GameDebug.OnDebug += GameDebug_OnDebug;
	}

	void OnDisable()
	{
		Debug.Log("Disabled!");
		GameDebug.OnDebug -= GameDebug_OnDebug;
	}

	void OnDestroy()
	{

	}

	void Update()
	{
		if (Message) //message is already set as true, and therefore is going to continue if equal to true
		{
			Debug.Log("Eevee or Pikachu");
			Message = false;
		}
		else //if (!Message) //if false
		{
			starterpokemon();
		}
	}
}