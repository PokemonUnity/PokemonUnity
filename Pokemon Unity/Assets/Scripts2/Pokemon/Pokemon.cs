using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Pokemon;
using PokemonUnity.Move;
using PokemonUnity.Item;

/// <summary>
/// 
/// </summary>
/// ToDo: Consider nesting PokemonData class?
[System.Serializable]
public partial class Pokemon //: ePokemons //PokemonData
{
    #region Variables
    /// <summary>
    /// Current Total HP
    /// </summary>
    public int TotalHP { get {
			return //totalHP;
				((2 * _base.BaseStatsHP + IV[0] + (EV[0] / 4)) * Level) / 100 + Level + 10; } }
	[Obsolete("Default HP should be auto generated, and not provided thru placeholder value")]
    private int totalHP = 1;
    /// <summary>
    /// Current HP
    /// </summary>
    private int hp = 1;
    /// <summary>
    /// Current Attack Stat
    /// </summary>
    public virtual int ATK {
		get
		{
			return (int)Math.Floor((((2 * _base.BaseStatsATK + IV[0] + (EV[0] / 4)) * Level) / 100 + 5) * natureFlag.ATK);
		}
	}
    /// <summary>
    /// Current Defense stat
    /// </summary>
    public virtual int DEF { get
		{
			return (int)Math.Floor((((2 * _base.BaseStatsDEF + IV[0] + (EV[0] / 4)) * Level) / 100 + 5) * natureFlag.DEF);
		}
	}
    /// <summary>
    /// Current Special Attack Stat
    /// </summary>
    public virtual int SPA {
		get
		{
			return (int)Math.Floor((((2 * _base.BaseStatsSPA + IV[0] + (EV[0] / 4)) * Level) / 100 + 5) * natureFlag.SPA);
		}
	}
    /// <summary>
    /// Current Special Defense Stat
    /// </summary>
    public virtual int SPD {
		get
		{
			return (int)Math.Floor((((2 * _base.BaseStatsSPD + IV[0] + (EV[0] / 4)) * Level) / 100 + 5) * natureFlag.SPD);
		}
	}
    /// <summary>
    /// Current Speed Stat
    /// </summary>
    public virtual int SPE {
		get
		{
			return (int)Math.Floor((((2 * _base.BaseStatsSPE + IV[0] + (EV[0] / 4)) * Level) / 100 + 5) * natureFlag.SPE);
		}
	}
    /// <summary>
    /// Array of 6 Individual Values for HP, Atk, Def, Speed, Sp Atk, and Sp Def
    /// </summary>
    public int[] IV { get; private set; }
	/// <summary>
	/// Effort Values
	/// </summary>
	/// <see cref="EVSTATLIMIT"/>
	/// new int[6] = { 0, 0, 0, 0, 0, 0 }; //same thing
	public int[] EV { get; private set; }
	/// <summary>
	/// Species (National Pokedex number)
	/// </summary>
	/// ToDo: Fetch from PokemonData : _base.PokeId
	public Pokemons Species { get { return _base.ID; } }
    /// <summary>
    /// Held item
    /// </summary>
    public Items Item { get; private set; }
	#region Move to PokemonBattle Class
	/// <summary>
	/// Consumed held item (used in battle only)
	/// </summary>
	private bool itemRecycle;
    /// <summary>
    /// Resulting held item (used in battle only)
    /// </summary>
    private bool itemInitial;
    /// <summary>
    /// Where Pokemon can use Belch (used in battle only)
    /// </summary>
	/// ToDo: Move to pkemonBattle class
    private bool belch;
	#endregion
	/// <summary>
	/// Current experience points
	/// </summary>
	/// <example>
	/// lv1->lv2=5xp
	/// lv2->lv3=10xp
	/// if pokemon is lvl 3 and 0xp, it should have a total of 15xp
	/// but display counter should still say 0
	/// </example>
	/// <remarks>
	/// experience should accumulate accross past levels.
	/// Should also rename to "currentExp"?
	/// </remarks>
	public Experience Exp { get; private set; }
	/// <summary>
	/// Current happiness
	/// </summary>
	/// <remarks>
	/// This is the samething as "friendship";
	/// </remarks>
	public int Happiness { get; private set; }
    /// <summary>
    /// Status problem (PBStatuses)
    /// </summary>
    /// ToDo: Status Class
    public Status Status { get; set; }
    /// <summary>
    /// Sleep count/Toxic flag
    /// </summary>
    /// ToDo: Add to Status Class or StatusTurn() method
    public int statusCount { get; private set; }
    /// <summary>
    /// Steps to hatch egg, 0 if Pokemon is not an egg
    /// </summary>
    public int eggSteps { get; private set; }
	/// <summary>
	/// Moves (PBMove)
	/// </summary>
	/// ToDo Move class, not enum
	public Move[] moves { get; private set; } 
	/// <summary>
	/// The moves known when this Pokemon was obtained
	/// </summary>
	private List<Moves> firstMoves = new List<Moves>();
	/// <summary>
	/// Ball used
	/// </summary>
	/// ToDo: Interface for Pokeball Only item?
	/// ToDo: None?
	public Items ballUsed { get; private set; }
	private PokemonData _base { get; set; }
	/// <summary>
	/// Max total EVs
	/// </summary>
	public const int EVLIMIT = 510; 
	/// <summary>
	/// Max EVs that a single stat can have
	/// </summary>
	/// ToDo: use const instead?
	/// Can be referenced as [Attribute] if is a const value
	public const int EVSTATLIMIT = 252; 
	/// <summary>
	/// Maximum length a Pokemon's nickname can be
	/// </summary>
	public const int NAMELIMIT = 10;
	#endregion

	#region Constructor
	public Pokemon()
	{
		_base = PokemonData.GetPokemon(Pokemons.NONE);
		PersonalId = new Random().Next(256);
		PersonalId |= new Random().Next(256) << 8;
		PersonalId |= new Random().Next(256) << 16;
		PersonalId |= new Random().Next(256) << 24;
		Ability = Abilities.NONE;
        natureFlag = new Nature();//(Natures)(new Random().Next(0, 24));
		//ToDo: Maybe add TrainerId = <int> here, before isShiny()?
		//shinyFlag = isShiny(); ToDo: Fix WildPokemon.TrainerId
		//Gender = isMale();
		//IV = new int[] { 10, 10, 10, 10, 10, 10 };
		IV = new int[] { Settings.Rand.Next(32), Settings.Rand.Next(32), Settings.Rand.Next(32), Settings.Rand.Next(32), Settings.Rand.Next(32), Settings.Rand.Next(32) };
		EV = new int[6];
		Exp = new Experience(GrowthRate);
		moves = new Move[4];// { new Move(Moves.NONE), new Move(Moves.NONE), new Move(Moves.NONE), new Move(Moves.NONE) };
		pokerus = new int[2];
		Markings = new bool[6]; //{ false, false, false, false, false, false };
		Status = Status.None;
		statusCount = 0;
		ballUsed = Items.NONE;
		Item = Items.NONE;
		//calcStats();
	}

	/// <summary>
	/// Uses PokemonData to initialize a Pokemon from base stats
	/// </summary>
	/// <param name="pokemon"></param>
	/// ToDo: Inherit PokemonData 
	public Pokemon(Pokemons pokemon) : this()
	{
		_base = PokemonData.GetPokemon(pokemon);
		Ability = _base.Ability[1] == Abilities.NONE ? _base.Ability[0] : _base.Ability[new Random().Next(0, 2)];
		//Gender = GenderRatio.//Pokemon.PokemonData.GetPokemon(pokemon).MaleRatio

		//calcStats();
	}

	public Pokemon(Pokemons TPSPECIES = Pokemons.NONE,
		int TPLEVEL = 10,
		Items TPITEM = Items.NONE,
		Moves TPMOVE1 = Moves.NONE,
		Moves TPMOVE2 = Moves.NONE,
		Moves TPMOVE3 = Moves.NONE,
		Moves TPMOVE4 = Moves.NONE,
		Abilities TPABILITY = Abilities.NONE,
		int? TPGENDER = null,
		int TPFORM = 0,
		bool TPSHINY = false,
		//Natures TPNATURE,
		int[] TPIV = null, //new int[6] { 10, 10, 10, 10, 10, 10 },
		int TPHAPPINESS = 70,
		string TPNAME = null,
		bool TPSHADOW = false,
		Items TPBALL = Items.NONE) : this(TPSPECIES)
	{
		//Random rand = new Random(Settings.Seed());//(int)TPSPECIES+TPLEVEL
		IV = TPIV ?? IV;
		//EV = new int[6];
		
		//calcStats();
	}
	#endregion

	#region Ownership, obtained information
	/// <summary>
	/// Manner Obtained:
	/// </summary>
	public ObtainedMethod ObtainedMode { get; private set; }
	public enum ObtainedMethod
	{
		MET = 0,
		EGG = 1,
		TRADED = 2,
		/// <summary>
		/// NPC-Event?
		/// </summary>
		FATEFUL_ENCOUNTER = 4
	}
	/// <summary>
	/// Map where obtained
	/// </summary>
	/// <remarks>
	/// Doubles as "HatchedMap"
	/// ToDo: Make this an enum
	/// </remarks>
	private int obtainMap { get; set; }
	/// <summary>
	/// Replaces the obtain map's name if not null
	/// </summary>
	private string obtainString { get; set; }
	//private int obtainLevel; // = 0;
	private System.DateTimeOffset obtainWhen { get; set; }
	private System.DateTimeOffset hatchedWhen { get; set; }
	/// <summary>
	/// Original Trainer's Name
	/// </summary>
	/// <remarks>
	/// ToDo: PlayerTrainer's hash value instead of class; maybe GUID?
	/// </remarks>
	private Player OT { get; set; }
    /// <summary>
    /// Personal/Pokemon ID
    /// </summary>
    /// ToDo: String value?
    public int PersonalId { get; private set; }
    /// <summary>
    /// 32-bit Trainer ID (the secret ID is in the upper 16-bits);
    /// Deprecated
    /// </summary>
    /// ToDo: Remove this, and fetch from Trainer Class?
    /// Can also store hexadecimal/binary values in int
	[Obsolete("Use <Trainer>Pokemon.OT to fetch trainer information.")]
    public int TrainerId { get; private set; }

	/// <summary>
	/// Returns whether or not the specified Trainer is the NOT this Pokemon's original trainer
	/// </summary>
	/// <param name="trainer"></param>
	/// <returns></returns>
	public bool isForeign(Player trainer) {
		return trainer != this.OT; //ToDo: Match HashId 
	}

	/// <summary>
	/// Returns the public portion of the original trainer's ID
	/// </summary>
	/// <returns></returns>
	/*public string PublicId()
    {
        //return TrainerId.ToString();
        return OT.ToString(); //ToDo: TrainerId fix here
    }*/
	public string PublicId
	{
		get { return OT.ToString(); }
	}

	/// <summary>
	/// Returns this Pokemon's level when this Pokemon was obtained
	/// </summary>
	/// <returns></returns>
	/// <remarks>
	/// Wouldnt this change again when traded to another trainer?
	/// </remarks>
	/*public int ObtainLevel()
    {
        //int level = 0; int.TryParse(this.obtainLevel, out level);
        return this.obtainLevel;
    }*/
	public int ObtainLevel
	{
        get; private set;
	}

	/*// <summary>
    /// Returns the time when this Pokemon was obtained
    /// </summary>
    /// <returns></returns>
    public System.DateTimeOffset TimeReceived()
    {
        if (obtainWhen == null) TimeReceived(DateTimeOffset.UtcNow); //ToDo: Global DateTime Variable
        return obtainWhen;
    }

    /// <summary>
    /// Set the time when this Pokemon was obtained
    /// </summary>
    /// <param name="UTCdate"></param>
    public void TimeReceived(DateTimeOffset UTCdate)
    {
        obtainWhen = UTCdate;
    }*/
	/// <summary>
	/// Sets or Returns the time when this Pokemon was obtained
	/// </summary>
	public DateTimeOffset TimeReceived
	{
		get
		{
			if (obtainWhen == null) this.obtainWhen = DateTimeOffset.UtcNow;
			return this.obtainWhen;
		}
		set { this.obtainWhen = value; }
	}

	/*// <summary>
    /// Returns the time when this Pokemon hatched
    /// </summary>
    /// <returns></returns>
    public DateTimeOffset TimeEggHatched()
    {
        if (this.ObtainedMode == ObtainedMethod.EGG)
        {
            if (hatchedWhen == null) TimeEggHatched(DateTimeOffset.UtcNow);
            return hatchedWhen;
        }
        else
            return DateTimeOffset.UtcNow; //ToDo: Something else?
    }

    /// <summary>
    /// Set the time when this Pokemon hatched
    /// </summary>
    /// <param name="UTCdate"></param>
    public void TimeEggHatched (DateTimeOffset UTCdate)
    {
        hatchedWhen = UTCdate;
    }*/
	/// <summary>
	/// Sets or Returns the time when this Pokemon hatched
	/// </summary>
	public DateTimeOffset TimeEggHatched
	{
		get
		{
			if (this.ObtainedMode == ObtainedMethod.EGG)
			{
				if (hatchedWhen == null) this.hatchedWhen = DateTimeOffset.UtcNow;
				return this.hatchedWhen;
			}
			else
				//return DateTimeOffset.UtcNow; //ToDo: Something else? Maybe error?
				throw new Exception("Trainer did not acquire Pokemon as an egg.");
		}
		set { this.hatchedWhen = value; }
	}
	#endregion

	#region Level
	public int Level
	{
		get
		{
			//return 1;
			return Experience.GetLevelFromExperience(this.GrowthRate, this.Exp.Current);
		}
		set
		{
			if (value < 1 || value > 100) //Experience.MAXLEVEL
				//ToDo: Instead if throwing exception error, do nothing?...
				throw new Exception(string.Format("The level number {0} is invalid", value));
			if(value > this.Level)
				this.Exp.AddExperience(this.GrowthRate, this.Exp.Current, Experience.GetStartExperience(this.GrowthRate, value) - this.Exp.Current);
			else
			{
				//ToDo: Not Designed to go backwards yet...
				//Instead if throwing exception error, do nothing?...
				throw new Exception(string.Format("The level number {0} is invalid", value));
			}			
		}
	}

	public bool isEgg
	{
		get
		{
			return eggSteps > 0;
		}
	}

	public LevelingRate GrowthRate
	{
		get
		{
			return _base.GrowthRate; //ToDo: Return as int?
		}
	}

	int baseExp
	{
		get
		{
			return _base.BaseExpYield; //ToDo: 
		}
	}
	#endregion

	#region Gender
	//private bool? gender;
	/// <summary>
	/// Returns this Pokemons gender. male; female; genderless.
	/// Sets this Pokemon's gender to a particular gender (if possible)
	/// </summary>
	/// <remarks>
	/// isMale; null = genderless?
	/// Should consider gender as byte? bool takes up same amount of space
	/// </remarks>
	public virtual bool? Gender { get; private set; }

	/// <summary>
    /// Helper function that determines whether the input values would make a female.
    /// </summary>
    /// ToDo: isMale; isFemale; isGenderless... properties?
    private bool? gender//isMale/isFemale/isGenderless//(float genderRate = this._base.MaleRatio)
    {
        get
        {
            //if (genderRate == 100f) return true; 
            if (_base.MaleRatio == GenderRatio.AlwaysMale) return true; 
            //if (genderRate == 100f) return false; //Always female
            if (_base.MaleRatio == GenderRatio.AlwaysFemale) return false; 
			if (_base.MaleRatio == GenderRatio.Genderless) return null; //genderless
			return getGender();
        }
    }

	bool? getGender()
	{
		switch (_base.MaleRatio)
		{
			case GenderRatio.FemaleOneEighth:
				break;
			case GenderRatio.Female25Percent:
				break;
			case GenderRatio.Female50Percent:
				break;
			case GenderRatio.Female75Percent:
				break;
			case GenderRatio.FemaleSevenEighths:
				break;
			case GenderRatio.AlwaysMale:
				return true;
			case GenderRatio.AlwaysFemale:
				return false;
			case GenderRatio.Genderless:
			default:
				return null;
		}
		return null;
	}

	/// <summary>
	/// Returns whether this Pokemon species is restricted to only ever being one gender (or genderless)
	/// </summary>
	public bool isSingleGendered { get { return true; } }
	#endregion

	#region Ability
	/// <summary>
	/// Forces the first/second/hidden (0/1/2) ability
	/// </summary>
	[Obsolete]
	private Abilities abilityFlag;
	/// <summary>
	/// Returns the ID of the Pokemons Ability.
	/// </summary>
	/// ToDo: Sets this Pokemon's ability to a particular ability (if possible)
	/// ToDo: Ability 1 or 2, never both...
	/// ToDo: Error on non-compatible ability?
	public Abilities Ability { get; private set; }//{ get { return abilityFlag; } set { abilityFlag = value; } }//ToDo: Check against getAbilityList()?

	/// <summary>
	/// Returns whether this Pokemon has a partiular ability
	/// </summary>
	/// <param name="ability"></param>
	/// <returns></returns>
	public bool hasAbility(Abilities ability = Abilities.NONE)
	{
		if (ability == Abilities.NONE) return (int)_base.Ability[0] > 0 || (int)_base.Ability[1] > 0;// || (int)Abilities[2] > 0;
		else
		{
			return _base.Ability[0] == ability || _base.Ability[1] == ability;// || Abilities[2] == ability;
		}
		//return false;
	}

	public bool hasHiddenAbility()
	{
		return _base.Ability[2] != Abilities.NONE;
	}

	/// <summary>
	/// Returns a list of abilities this Pokemon can have.
	/// </summary>
	/// <returns></returns>
	/// Is there a list of abilities a pokemon can have outside of "default" values?
	public Abilities[] getAbilityList()
	{
		//List<Abilities> abilList;
		//foreach(){ list.add() }
		//Abilities[] abilities = abilList.ToArray();
		//return abilities;
		return this._base.Ability; //ToDo: List of abilities?
	}
	#endregion

	#region Nature
	/// <summary>
	/// Forces a particular nature
	/// </summary>
	/// ToDo: Redo NatureDatabase Class
	private Nature natureFlag { get; set; }
	/// <summary>
	/// Returns the ID of this Pokemon's nature or
	/// Sets this Pokemon's nature to a particular nature (and calculates the stat modifications).
	/// </summary>
	public Natures Nature { get { return this.natureFlag.Natures; } } //set { this.natureFlag = value; calcStats(); } }

	/// <summary>
	/// Returns whether this Pokemon has a particular nature
	/// </summary>
	/// <param name="nature"></param>
	/// <returns></returns>
	public bool hasNature(Natures nature = 0) //None
	{
		if ((int)nature < 1) return (int)this.Nature >= 1;
		else
		{
			return this.Nature == nature;
		}
	}
	#endregion

	#region Shininess
	/// <summary>
	/// Uses math to determine if Pokemon is shiny.
	/// Returns whether this Pokemon is shiny (differently colored)
	/// </summary>
	/// <returns></returns>
	/// Use this when rolling for shiny...
	/// Honestly, without this math, i probably would've done something a lot more primative.
	/// Look forward to primative math on wild pokemon encounter chances...
	private bool isShiny()
	{
		if (shinyFlag.HasValue) return shinyFlag.Value;
		int a = this.PersonalId ^ this.TrainerId; //Wild Pokemon TrainerId?
		int b = a & 0xFFFF;
		int c = (a >> 16) & 0xFFFF;
		int d = b ^ c;
		return d < _base.ShinyChance;
	}

	/// <summary>
	/// Makes this Pokemon shiny or not shiny
	/// </summary>
	public virtual bool IsShiny
	{
		//If not manually set, use math to figure out.
		//ToDo: Store results to save on redoing future execution? 
		get { return shinyFlag ?? isShiny()/*false*/; }
		set { shinyFlag = value; }
	}
	/// <summary>
	/// Forces the shininess
	/// </summary>
	private bool? shinyFlag { get; set; }
	#endregion

	#region Pokerus
    /// <summary>
    /// Pokerus strain and infection time
    /// </summary>
    /// { 0, 0 };
    /// <example>
    /// </example>
    /// <remarks>
    /// ToDo: Custom class?
    /// 3 Values; Not Infected, Cured, Infected.
    /// [0] = Pokerus Strain; [1] = Days until cured.
    /// if ([0] && [1] == 0) => Not Infected
    /// </remarks>
    private int[] pokerus { get; set; }

	/// <summary>
	/// Returns the full value of this Pokemon's Pokerus
	/// </summary>
	/// <returns>
	/// 3 Values; Not Infected, Cured, Infected.
	/// [0] = Pokerus Strain; [1] = Days until cured.
	/// if ([0] && [1] == 0) => Not Infected
	/// </returns>
	/*public int[] Pokerus()
    {
        return this.pokerus;
    }*/
	public int[] Pokerus { get { return this.pokerus; } }

	/// <summary>
	/// Returns the Pokerus infection stage for this Pokemon
	/// </summary>
	/// <returns></returns>
	/*public int PokerusStrain()
    {
        return this.pokerus[0] / 16;
    }*/
	public int PokerusStrain { get { return this.pokerus[0] / 16; } }

	/// <summary>
	/// Returns the Pokerus infection stage for this Pokemon
	/// </summary>
	/// <returns>
	/// if null, not infected; 
	/// true infected, and false cured?
	/// </returns>
	/*public bool? PokerusStage()
    {
        if (pokerus[0] == 0 && pokerus[1] == 0) return null;        // Not Infected
        if (pokerus[0] > 0 && pokerus[1] == 0) return false;        // Cured; (pokerus[0] % 16) == 0
        return true;                                                // Infected
    }*/
	public bool? PokerusStage
	{
		get
		{
			if (pokerus[0] == 0 && pokerus[1] == 0) return null;        // Not Infected
			if (pokerus[0] > 0 && pokerus[1] == 0) return false;        // Cured; (pokerus[0] % 16) == 0
			return true;                                                // Infected
		}
	}

	/// <summary>
	/// Gives this Pokemon Pokerus (either the specified strain or a random one)
	/// </summary>
	/// <param name="strain"></param>
	public void GivePokerus(int strain = 0)
	{
		if (this.PokerusStage.HasValue ? !this.PokerusStage.Value : false) return; // Cant re-infect a cured Pokemon
		if (strain <= 0 || strain >= 16) strain = new Random().Next(1, 16);
		pokerus[1] = 1 + (strain % 4);
		pokerus[0] |= strain; //strain << 4
	}

	/// <summary>
	/// Resets the infection time for this Pokemon's Pokerus (even if cured)
	/// </summary>
	public void ResetPokerusTime()
	{
		if (pokerus[0] == 0) return;
		pokerus[1] = 1 + (pokerus[0] % 4);
	}

	/// <summary>
	/// Reduces the time remaining for this Pokemon's Pokerus (if infected)
	/// </summary>
	public void LowerPokerusCount()
	{
		if (this.PokerusStage.HasValue ? !this.PokerusStage.Value : true) return;
		pokerus[1] -= 1;
	}
	#endregion

	#region Types
	/// <summary>
	/// Returns whether this Pokemon has the specified type.
	/// </summary>
	/// <param name="type"></param>
	/// <returns></returns>
	public bool hasType(Types type)
	{
		return this._base.Type[0] == type || this._base.Type[1] == type;
	}

	/// <summary>
	/// Returns this Pokemon's first type
	/// </summary>
	public Types Type1 { get { return this._base.Type[0]; } }

	/// <summary>
	/// Returns this Pokemon's second type
	/// </summary>
	public Types Type2 { get { return this._base.Type[1]; } }
	#endregion

	#region Moves
	/// <summary>
	/// Returns the number of moves known by the Pokémon.
	/// </summary>
	public int numMoves()
	{
		int ret = 0;
		for (int i = 0; i < 4; i++) {//foreach(var move in this.moves){ 
			if ((int)this.moves[i].MoveId != 0) ret += 1;//move.id
		}
		return ret;
	}

	/// <summary>
	/// Returns true if the Pokémon knows the given move.
	/// </summary>
	public bool hasMove(Moves move) {
		//if (move <= 0) return false;//move == null ||
		for (int i = 0; i < 4; i++)
		{
			if (this.moves[i].MoveId == move) return true;
		}
		return false;
	}

	public bool knowsMove(Moves move) { return this.hasMove (move); }

	/*// <summary>
    /// Returns the list of moves this Pokémon can learn by levelling up.
    /// </summary>
    /// ToDo: Custom<int Level, eMove move> Class
    public PokemonMoveset[] getMoveList() {
		//Move.MoveData.Move[] movelist = _base.MovesetMoves;
		//for (int k = 0; k < movelist.Length - 1; k++)
		//{
		//	//Array to List/Dictionary
		//	//separate into Move.value and Pokemon.Level 
		//	//needed to learn the skill
		//	//movelist([level, move])}
		//}
		//return movelist;
		return _base.MoveSet;
     }*/

	/// <summary>
    /// Sets this Pokémon's movelist to the default movelist it originally had.
    /// </summary>
    public void resetMoves()
    {
		//Move.MoveData.Move moves = this.getMoveList();
		//Move.MoveData.Move[] movelist = new Move.MoveData.Move[4];
		List<Moves> movelist = new List<Moves>(); 
        /*for (int i = 0; i < _base.MoveSet.Length; i++){//foreach(var i in _base.MoveSet)
            if (_base.MoveSet[i].Level <= this.Level)
            {
                movelist.Add(_base.MoveSet[i].MoveId);
            }
        }*/
        movelist.AddRange(_base.MoveTree.LevelUp.Where(x => x.Value <= this.Level).Select(x => x.Key));
        //movelist|=[] // Remove duplicates
        int listend = movelist.Count - 4;
        listend = listend < 0 ? 0 : listend;
        int j = 0;
        for (int i = 0; i < listend + 4; i++) { 
            Moves moveid = (i >= movelist.Count) ? 0 : movelist[i];
			this.moves[j] = new Move(moveid);
            //moves[j] = (i >= movelist.Count) ? 0 : new Move(movelist[i]);
            j += 1;
        }
    }

	/// <summary>
	/// Silently learns the given move. Will erase the first known move if it has to.
	/// </summary>
	/// <param name=""></param>
	/// <returns></returns>
	public void LearnMove(Moves move) {
		if ((int)move <= 0) return;
		for (int i = 0; i < 4; i++) {
			if (moves[i].MoveId == move) {
				int j = i + 1;
				while (j < 4) {
					if (moves[j].MoveId == 0) break;
					Move tmp = moves[j];
					moves[j] = moves[j - 1];
					moves[j - 1] = tmp;
					j += 1;
				}
				return;
			}
		}
		for (int i = 0; i < 4; i++) {
			if (moves[i].MoveId == 0) {
				moves[i] = new Move(move);
				return;
			}
		}
		moves[0] = moves[1];
		moves[1] = moves[2];
		moves[2] = moves[3];
		moves[3] = new Move(move);
	}

	/// <summary>
	/// Deletes the given move from the Pokémon.
	/// </summary>
	/// <param name=""></param>
	/// <returns></returns>
	public void DeleteMove(Moves move) {
		if (move <= 0) return;
		List<Move> newmoves = new List<Move>();
		for (int i = 0; i < 4; i++) { 
			if (moves[i].MoveId != move) newmoves.Add(moves[i]);
		}

		newmoves.Add(new Move(0));
		for (int i = 0; i< 4; i++) {
			moves[i] = newmoves[i];
		}
	 }

	/// <summary>
	/// Deletes the move at the given index from the Pokémon.
	/// </summary>
	/// <param name=""></param>
	/// <returns></returns>
	public void DeleteMoveAtIndex(int index) {
		List<Move> newmoves = new List<Move>();

		for (int i = 0; i < 4; i++) {
			if (i != index) newmoves.Add(moves[i]);
		}
		
		newmoves.Add(new Move(0));

		for (int i = 0; i < 4; i++) {
			moves[i] = newmoves[i];
		}
	}

	/// <summary>
	/// Deletes all moves from the Pokémon.
	/// </summary>
	public void DeleteAllMoves() { 
		//moves = new Move.MoveData.Move[4];
		for (int i = 0; i< 4; i++) { 
			moves[i]= new Move(0);
		}
	}

	/// <summary>
	/// Copies currently known moves into a separate array, for Move Relearner.
	/// </summary>
	public void RecordFirstMoves() {
		for (int i = 0; i < 4; i++) {
			if (moves[i].MoveId > 0) firstMoves.Add(moves[i].MoveId);
		}
	}

	public void AddFirstMove(Moves move) {
		if (move > 0 && !firstMoves.Contains(move)) firstMoves.Add(move);
		return;
	}

	public void RemoveFirstMove(Moves move) {
		if (move > 0) firstMoves.Remove(move); 
		return;
	}

	public void ClearFirstMoves() {
		firstMoves.Clear(); //= new Move.MoveData.Move[4];
	}

	/*bool isCompatibleWithMove(Move.MoveData.Move move) {
		return SpeciesCompatible(this.species, move);
	}*/

	/*// <summary>
	/// Reduce the global clutter, and add to more readable 
	/// and maintainable code by encapsulation to logically 
	/// group classes that are only used in one place.
	/// </summary>
	internal class Moves
	{

	}*/
	#endregion

	#region Contest attributes, ribbons
	/// <summary>
	/// Deprecated; Array of ribbons
	/// </summary>
	/// <remarks>
	/// Make 2d Array (Array[,]) separated by regions/Gens
	/// </remarks>
	[Obsolete]
	private bool[] ribbon; //= new bool[numberOfRegions,RibbonsPerRegion];
	private List<Ribbon> ribbons { get; set; } 
	/// <summary>
	/// Each region/ribbon sprite should have it's own Ribbon.EnumId
	/// </summary>
	/// <example>Pokemon acquired beauty ribbon in region1 AND 2?</example>
	/// I didnt know ribbons could be upgraded...
	/// Make each ribbon into sets, where next number up is upgrade? (or multiply?)
	/// Does it make a difference if pokemon won contest in different regions?
	public List<Ribbon> Ribbons { get { return this.ribbons; } }
	/// <summary>
	/// Contest stats; Max value is 255
	/// </summary>
	private int cool, beauty, cute, smart, tough, sheen;
	/// <summary>
	/// Returns whether this Pokémon has the specified ribbon.
	/// </summary>
	/// <param name="ribbon"></param>
	/// <returns></returns>
	public bool hasRibbon(Ribbon ribbon)
	{
		if (Ribbons.Count == 0) return false;
		return Ribbons.Contains(ribbon);
	}
	/// <summary>
	/// Gives this Pokémon the specified ribbon.
	/// </summary>
	/// <param name="ribbon"></param>
	public void giveRibbon(Ribbon ribbon)
	{
		if (ribbon <= 0) return;
		if (!Ribbons.Contains(ribbon)) this.ribbons.Add(ribbon);
	}
	/// <summary>
	/// Replaces one ribbon with the next one along, if possible.
	/// </summary>
	/// <param name="ribbon"></param>
	/// ToDo: Not finished here...
	public void upgradeRibbon(params Ribbon[] ribbon)//(Ribbon ribbon, Ribbon? upgradedRibbon = null)
	{
		//if(Ribbons.Count)
		//for(int i = 0; i < ribbon.Length)
	}
	/// <summary>
	/// Removes the specified ribbon from this Pokémon.
	/// </summary>
	/// <param name="ribbon"></param>
	public void takeRibbon(Ribbon ribbon)
	{
		if (ribbons.Count == 0) return;
		if (ribbon <= 0) return;
		for(int i = 0; i < ribbons.Count; i++)
		{
			if (Ribbons[i] == ribbon)
			{
				ribbons[i] = Ribbon.NONE;
				break;
			}
		}
		ribbons.Remove(Ribbon.NONE); //ToDo: List.RemoveAll(Ribbon == NONE)
	}
	/// <summary>
	/// Removes all ribbons from this Pokémon.
	/// </summary>
	public void clearAllRibbons()
	{
		ribbons.Clear();
	}
	#endregion

	#region Items
	#endregion

	#region Shadow
	/// <summary>
	/// Shadow Pokémon in the first game sometimes enter Hyper Mode, but in XD they enter Reverse Mode instead.
	/// </summary>
	/// In Hyper Mode, a Pokémon may attack its Trainer, but in Reverse Mode, they will not.
	/// While in Reverse Mode, a Pokémon hurts itself after every turn, whereas a Pokémon in Hyper Mode incurs no self-damage
	public bool isHyperMode { get; private set; }
	/// <summary>
	/// Shadow Pokémon don't have a set Nature or a set gender, but once encountered, the personality value, 
	/// Nature and IVs are saved to the memory for the Shadow Monitor to be able to keep track of their exact status and location. 
	/// This means that once a Shadow Pokémon is encountered for the first time, its Nature, IVs and gender will remain the same for the rest of the game, 
	/// even if the player fails to capture it or is forced to re-battle it later.
	/// </summary>
	public bool isShadow { get
		{
			if (!ShadowLevel.HasValue || ShadowLevel.Value == -1) return false;
			else return true;
		} }
	/// <summary>
	/// Heart Gauge.
	/// The Heart Gauge is split into five equal bars. When a Shadow Pokémon is first snagged, all five bars are full.
	/// </summary>
	/// If pokemon is purified, shadow level should be equal to -1
	/// If pokemon has never been shadowed, then value should be null
	/// HeartGuage max size should be determined by _base.database
	public int? ShadowLevel { get; private set; }
	//public int? HeartGuage { get { return ShadowLevel; } }

	void decreaseShadowLevel (pokemonActions Action)
	{
		int points = 0;

		#region Pokemon Colosseum Shadow Switch
		/* Values from Pokémon Colosseum
		Nature	Battle	Callto	Party	DayCare	Scent
		Adamant 150  	225  	150  	300  	75
		Bashful 50  	300  	75  	450  	200
		Bold  	150  	225  	200  	300  	50
		Brave   200  	225  	150  	225  	75
		Calm  	50  	300  	100  	450  	150
		Careful 75  	300  	75  	450  	150
		Docile  75  	600  	100  	225  	100
		Gentle  50  	300  	75  	600  	150
		Hardy   150  	300  	150  	150  	100
		Hasty   200  	300  	75  	150  	150
		Impish  200  	300  	150  	150  	75
		Jolly   150  	300  	100  	150  	150
		Lax  	100  	225  	150  	225  	150
		Lonely  50  	450  	150  	150  	200
		Mild  	75  	225  	75  	450  	200
		Modest  75  	300  	75  	600  	100
		Naive   100  	300  	150  	225  	100
		Naughty 150  	225  	200  	225  	75
		Quiet   100  	300  	100  	300  	100
		Quirky  200  	225  	50  	600  	75
		Rash  	75  	300  	100  	300  	150
		Relaxed 75  	225  	75  	600  	150
		Sassy   200  	150  	150  	225  	100
		Serious 100  	450  	100  	300  	75
		Timid   50  	450  	50  	600  	150
		*/
		switch (Action)
		{
			case pokemonActions.Battle:
				if(this.Nature == Natures.ADAMANT ) points = 150;
				if(this.Nature == Natures.BASHFUL ) points = 50	;
				if(this.Nature == Natures.BOLD    ) points = 150;
				if(this.Nature == Natures.BRAVE   ) points = 200;
				if(this.Nature == Natures.CALM    ) points = 50	;
				if(this.Nature == Natures.CAREFUL ) points = 75	;
				if(this.Nature == Natures.DOCILE  ) points = 75	;
				if(this.Nature == Natures.GENTLE  ) points = 50	;
				if(this.Nature == Natures.HARDY   ) points = 150;
				if(this.Nature == Natures.HASTY   ) points = 200;
				if(this.Nature == Natures.IMPISH  ) points = 200;
				if(this.Nature == Natures.JOLLY   ) points = 150;
				if(this.Nature == Natures.LAX     ) points = 100;
				if(this.Nature == Natures.LONELY  ) points = 50	;
				if(this.Nature == Natures.MILD    ) points = 75	;
				if(this.Nature == Natures.MODEST  ) points = 75	;
				if(this.Nature == Natures.NAIVE   ) points = 100;
				if(this.Nature == Natures.NAUGHTY ) points = 150;
				if(this.Nature == Natures.QUIET   ) points = 100;
				if(this.Nature == Natures.QUIRKY  ) points = 200;
				if(this.Nature == Natures.RASH    ) points = 75	;
				if(this.Nature == Natures.RELAXED ) points = 75	;
				if(this.Nature == Natures.SASSY   ) points = 200;
				if(this.Nature == Natures.SERIOUS ) points = 100;
				if(this.Nature == Natures.TIMID   ) points = 50	;
				break;
			case pokemonActions.CallTo:
				if (this.Nature == Natures.ADAMANT) points = 225;   
				if(this.Nature == Natures.BASHFUL ) points = 300;   
				if(this.Nature == Natures.BOLD    ) points = 225;   
				if(this.Nature == Natures.BRAVE   ) points = 225;   
				if(this.Nature == Natures.CALM    ) points = 300;   
				if(this.Nature == Natures.CAREFUL ) points = 300;   
				if(this.Nature == Natures.DOCILE  ) points = 600;   
				if(this.Nature == Natures.GENTLE  ) points = 300;   
				if(this.Nature == Natures.HARDY   ) points = 300;   
				if(this.Nature == Natures.HASTY   ) points = 300;   
				if(this.Nature == Natures.IMPISH  ) points = 300;   
				if(this.Nature == Natures.JOLLY   ) points = 300;   
				if(this.Nature == Natures.LAX     ) points = 225;   
				if(this.Nature == Natures.LONELY  ) points = 450;   
				if(this.Nature == Natures.MILD    ) points = 225;   
				if(this.Nature == Natures.MODEST  ) points = 300;   
				if(this.Nature == Natures.NAIVE   ) points = 300;   
				if(this.Nature == Natures.NAUGHTY ) points = 225;   
				if(this.Nature == Natures.QUIET   ) points = 300;   
				if(this.Nature == Natures.QUIRKY  ) points = 225;   
				if(this.Nature == Natures.RASH    ) points = 300;   
				if(this.Nature == Natures.RELAXED ) points = 225;   
				if(this.Nature == Natures.SASSY   ) points = 150;   
				if(this.Nature == Natures.SERIOUS ) points = 450;
				if (this.Nature == Natures.TIMID  ) points = 450;   	  
				break;											
			case pokemonActions.Party:   						
				if(this.Nature == Natures.ADAMANT ) points = 150;   
				if(this.Nature == Natures.BASHFUL ) points = 75 ;   
				if(this.Nature == Natures.BOLD    ) points = 200;   
				if(this.Nature == Natures.BRAVE   ) points = 150;   
				if(this.Nature == Natures.CALM    ) points = 100;   
				if(this.Nature == Natures.CAREFUL ) points = 75 ;   
				if(this.Nature == Natures.DOCILE  ) points = 100;   
				if(this.Nature == Natures.GENTLE  ) points = 75 ;   
				if(this.Nature == Natures.HARDY   ) points = 150;   
				if(this.Nature == Natures.HASTY   ) points = 75 ;   
				if(this.Nature == Natures.IMPISH  ) points = 150;   
				if(this.Nature == Natures.JOLLY   ) points = 100;   
				if(this.Nature == Natures.LAX     ) points = 150;   
				if(this.Nature == Natures.LONELY  ) points = 150;   
				if(this.Nature == Natures.MILD    ) points = 75 ;   
				if(this.Nature == Natures.MODEST  ) points = 75 ;   
				if(this.Nature == Natures.NAIVE   ) points = 150;   
				if(this.Nature == Natures.NAUGHTY ) points = 200;   
				if(this.Nature == Natures.QUIET   ) points = 100;   
				if(this.Nature == Natures.QUIRKY  ) points = 50 ;   
				if(this.Nature == Natures.RASH    ) points = 100;   
				if(this.Nature == Natures.RELAXED ) points = 75 ;   
				if(this.Nature == Natures.SASSY   ) points = 150;   
				if(this.Nature == Natures.SERIOUS ) points = 100;
				if (this.Nature == Natures.TIMID  ) points = 50 ;   
				break;											
			case pokemonActions.DayCare:						
			case pokemonActions.MysteryAction:					
				if(this.Nature == Natures.ADAMANT ) points = 300;   
				if(this.Nature == Natures.BASHFUL ) points = 450;   
				if(this.Nature == Natures.BOLD    ) points = 300;   
				if(this.Nature == Natures.BRAVE   ) points = 225;   
				if(this.Nature == Natures.CALM    ) points = 450;   
				if(this.Nature == Natures.CAREFUL ) points = 450;   
				if(this.Nature == Natures.DOCILE  ) points = 225;   
				if(this.Nature == Natures.GENTLE  ) points = 600;   
				if(this.Nature == Natures.HARDY   ) points = 150;   
				if(this.Nature == Natures.HASTY   ) points = 150;   
				if(this.Nature == Natures.IMPISH  ) points = 150;   
				if(this.Nature == Natures.JOLLY   ) points = 150;   
				if(this.Nature == Natures.LAX     ) points = 225;   
				if(this.Nature == Natures.LONELY  ) points = 150;   
				if(this.Nature == Natures.MILD    ) points = 450;   
				if(this.Nature == Natures.MODEST  ) points = 600;   
				if(this.Nature == Natures.NAIVE   ) points = 225;   
				if(this.Nature == Natures.NAUGHTY ) points = 225;   
				if(this.Nature == Natures.QUIET   ) points = 300;   
				if(this.Nature == Natures.QUIRKY  ) points = 600;   
				if(this.Nature == Natures.RASH    ) points = 300;   
				if(this.Nature == Natures.RELAXED ) points = 600;   
				if(this.Nature == Natures.SASSY   ) points = 225;   
				if(this.Nature == Natures.SERIOUS ) points = 300;
				if (this.Nature == Natures.TIMID  ) points = 600;   
				break;											
			case pokemonActions.Scent:							
				if(this.Nature == Natures.ADAMANT ) points = 75	;
				if(this.Nature == Natures.BASHFUL ) points = 200;
				if(this.Nature == Natures.BOLD    ) points = 50	;
				if(this.Nature == Natures.BRAVE   ) points = 75	;
				if(this.Nature == Natures.CALM    ) points = 150;
				if(this.Nature == Natures.CAREFUL ) points = 150;
				if(this.Nature == Natures.DOCILE  ) points = 100;
				if(this.Nature == Natures.GENTLE  ) points = 150;
				if(this.Nature == Natures.HARDY   ) points = 100;
				if(this.Nature == Natures.HASTY   ) points = 150;
				if(this.Nature == Natures.IMPISH  ) points = 75	;
				if(this.Nature == Natures.JOLLY   ) points = 150;
				if(this.Nature == Natures.LAX     ) points = 150;
				if(this.Nature == Natures.LONELY  ) points = 200;
				if(this.Nature == Natures.MILD    ) points = 200;
				if(this.Nature == Natures.MODEST  ) points = 100;
				if(this.Nature == Natures.NAIVE   ) points = 100;
				if(this.Nature == Natures.NAUGHTY ) points = 75	;
				if(this.Nature == Natures.QUIET   ) points = 100;
				if(this.Nature == Natures.QUIRKY  ) points = 75	;
				if(this.Nature == Natures.RASH    ) points = 150;
				if(this.Nature == Natures.RELAXED ) points = 150;
				if(this.Nature == Natures.SASSY   ) points = 100;
				if(this.Nature == Natures.SERIOUS ) points = 75	;
				if(this.Nature == Natures.TIMID   ) points = 150;
				break;
			default:
				break;
		}
		#endregion

		#region Pokemon XD Shadow Switch
		/* Values from Pokémon XD
		Nature	Battle	Callto	Party	???		Scent
		Adamant 110  	270  	110  	300  	80
		Bashful 80  	300  	90  	330  	130
		Bold    110  	270  	90  	300  	100
		Brave   130  	270  	90  	270  	80
		Calm    80  	300  	110  	330  	110
		Careful 90  	300  	100  	330  	110
		Docile  100  	360  	80  	270  	120
		Gentle  70  	300  	130  	360  	100
		Hardy   110  	300  	100  	240  	90
		Hasty   130  	300  	70  	240  	100
		Impish  120  	300  	100  	240  	80
		Jolly   120  	300  	90  	240  	90
		Lax     100  	270  	90  	270  	110
		Lonely  70  	330  	100  	240  	130
		Mild    80  	270  	100  	330  	120
		Modest  70  	300  	120  	360  	110
		Naive   100  	300  	120  	270  	80
		Naughty 120  	270  	110  	270  	70
		Quiet   100  	300  	100  	300  	100
		Quirky  130  	270  	80  	360  	90
		Rash    90  	300  	90  	300  	120
		Relaxed 90  	270  	110  	360  	100
		Sassy   130  	240  	100  	270  	70
		Serious 100  	330  	110  	300  	90
		Timid   70  	330  	110  	360  	120
		*
		switch (Action)
		{
			case pokemonActions.Battle:
				break;
			case pokemonActions.CallTo:
				break;
			case pokemonActions.Party:
				break;
			case pokemonActions.DayCare:
			case pokemonActions.MysteryAction:
				break;
			case pokemonActions.Scent:
				break;
			default:
				break;
		}*/
		#endregion
		if (ShadowLevel.Value > 0)
			ShadowLevel = (ShadowLevel.Value - points) < 0 ? 0 : ShadowLevel.Value - points;
	}

	enum pokemonActions
	{
		Battle, CallTo, Party, DayCare, MysteryAction, Scent
	}
	#endregion

	#region Other
    /// <summary>
    /// Mail?...
    /// </summary>
    private Mail mail { get; set; }
    /// <summary>
    /// Perform a null check; if anything other than null, there is a message
    /// </summary>
    /// ToDo: Item category
    public string Mail
    {
        get {
            if (this.mail == null) return null; //If empty return null
            if (mail.Message.Length == 0 || this.Item == 0)//|| this.item.Category != Items.Category.Mail )
            {
                //mail = null;
				return null;
            }
            return mail.Message;
        }
        //set { mail = value; }
    }
    /// <summary>
    /// The pokemons fused into this one.
    /// </summary>
	/// was int before
	/// Store both pokemons separately
	/// Combine the stats of both to create a new hybrid
	/// When reverting back, return to original data, and split exp gained between fusion
	/// ExpGained = (FusedExp - MiddleExpOfPokemons) / #ofPokemons
	/// should remain null until needed
    private Pokemon[] fused { get; set; }
	/// <summary>
	/// Nickname
	/// </summary>
	private string name { get; set; }
	/// <summary>
	/// Nickname; 
	/// Returns Pokemon species name if not nicknamed.
	/// </summary>
	public virtual string Name { get { return name ?? _base.Name; } }

	/// <summary>
	/// </summary>
	/// ToDo: Fix Forms and uncomment
	/// Maybe a method, where when a form is changed
	/// checks pokemon value and overwrites name and stats
    public int Form { set { if (value >= 0 && value <= _base.Forms) _base.Form = value; } }

    /// <summary>
    /// Returns the species name of this Pokemon
    /// </summary>
    /*// <returns></returns>
    public string SpeciesName()
    {
        return _base.getName();
    }*/
    public string SpeciesName { get { return this._base.Species; } }

	/// <summary>
	/// Returns the markings this Pokemon has
	/// </summary>
	/// <returns>6 Markings</returns>
	/*public bool[] Markings()
    {
        return markings;
    }
	public bool[] Markings { get { return this.markings; } }
	/// <summary>
	/// Markings
	/// </summary>*/
	public bool[] Markings { get; set; }

    /// <summary>
    /// Returns a string stathing the Unown form of this Pokemon
    /// </summary>
    /// <returns></returns>
    public char UnknownShape()
    {
        return "ABCDEFGHIJKLMNOPQRSTUVWXYZ?!".ToCharArray()[_base.ArrayId]; //ToDo: FormId; "if pokemon is an Unknown"
    }

	/// <summary>
	/// Sets this Pokemon's HP;
	/// Changes status on fainted
	/// </summary>
	/*// <param name="value"></param>
    public void HP(int value)
    {
        hp = value < 0 ? 0 : value;
        if (hp == 0) status = 0; // statusCount = 0; //ToDo: Fainted
    }*/
	public int HP
    {
        get { return this.hp; } //ToDo: If greater than totalHP throw error?
        set
        {
            this.hp = value < 0 ? 0 : (value > this.TotalHP ? TotalHP : value);
            if (this.hp == 0) this.Status = Status.None; // statusCount = 0; //ToDo: Fainted
        }
    }

    public bool isFainted()
    {
        return !this.isEgg //eggSteps == 0 //not an egg
            && this.HP <= 0;//hp <= 0;
    }

    /// <summary>
    /// Heals all HP of this Pokemon
    /// </summary>
    public void HealHP()
    {
        if (this.isEgg) return;     //ToDo: Throw exception error on returns
        this.HP = totalHP;          //ToDo: Return 'true' on success?
    }

    /// <summary>
    /// Heals the status problem of this Pokemon
    /// </summary>
    public void HealStatus()
    {
        if (this.isEgg) return;
        this.Status = 0; statusCount = 0; //remove status ailments
    }

    /// <summary>
    /// Heals all PP of this Pokemon
    /// </summary>
    /// <param name="index"></param>
    public void HealPP(int index = -1)
    {
        if (this.isEgg) return;
        if (index >= 0) moves[index] = moves[index]; // ToDo: pp = totalpp
        else
        {
            for (int i = 0; i < 3; i++){
                moves[index] = moves[index]; // ToDo: pp = totalpp
            }
        }
    }

    /// <summary>
    /// Heals all HP, PP, and status problems of this Pokemon
    /// </summary>
    public void Heal()
    {
        if (this.isEgg) return;
        HealHP();
        HealStatus();
        HealPP();
    }

    /// <summary>
    /// Changes the happiness of this Pokemon depending on what happened to change it
    /// </summary>
    /// <param name="method"></param>
    public void ChangeHappiness(HappinessMethods method)
    {
        int gain = 0; bool luxury = false;
        switch (method)
        {
            case HappinessMethods.WALKING:
                gain = 1;
                gain += Happiness < 200 ? 1 : 0;
                //gain += this.metMap.Id == currentMap.Id ? 1 : 0; //change to "obtainMap"?
                break;
            case HappinessMethods.LEVELUP:
                gain = 2;
                if (Happiness < 200) gain = 3;
                if (Happiness < 100) gain = 5;
                luxury = true;
                break;
            case HappinessMethods.GROOM:
                gain = 4;
                if (Happiness < 200) gain = 10;
                luxury = true;
                break;
            case HappinessMethods.FAINT:
                gain = -1;
                break;
            case HappinessMethods.VITAMIN:
                gain = 2;
                if (Happiness < 200) gain = 3;
                if (Happiness < 100) gain = 5;
                break;
            case HappinessMethods.EVBERRY:
                gain = 2;
                if (Happiness < 200) gain = 5;
                if (Happiness < 100) gain = 10;
                break;
            case HappinessMethods.POWDER:
                gain = -10;
                if (Happiness < 200) gain = -5;
                break;
            case HappinessMethods.ENERGYROOT:
                gain = -15;
                if (Happiness < 200) gain = -10;
                break;
            case HappinessMethods.REVIVALHERB:
                gain = -20;
                if (Happiness < 200) gain = -15;
                break;
            default:
                break;
        }
        gain += luxury && this.ballUsed == Items.LUXURY_BALL ? 1 : 0;
        if (this.Item == Items.SOOTHE_BELL && gain > 0)
            gain = (int)Math.Floor(gain * 1.5f);
        Happiness += gain;
        Happiness = Happiness > 255 ? 255 : Happiness < 0 ? 0 : Happiness; //Max 255, Min 0
    }
	#endregion

	#region Stat calculations, Pokemon creation
    /*// <summary>
    /// Returns the EV yield of this Pokemon
    /// </summary>
    /// <returns></returns>
    public int[] evYield()
    {
        return EV;
    }*
	//_base.getBaseStats();
    public int[] evYield { get { return this.EV; } }*/

	/*public bool addEVs(string stat, float amount)
	{
		int intAmount = Mathf.FloorToInt(amount);
		int evTotal = EV_HP + EV_ATK + EV_DEF + EV_SPA + EV_SPD + EV_SPE;
		if (evTotal < 510)
		{ //if total EV cap is already reached.
			if (evTotal + intAmount > 510)
			{ //if this addition will pass the total EV cap.
				intAmount = 510 - evTotal; //set intAmount to be the remaining points before cap is reached.
			}
			if (stat == "HP")
			{ //if adding to HP.
				if (EV_HP < 252)
				{ //if HP is not full.
					EV_HP += intAmount;
					if (EV_HP > 252)
					{ //if single stat EV cap is passed.
						EV_HP = 252;
					} //set stat back to the cap.
					return true;
				}
			}
			else if (stat == "ATK")
			{ //if adding to ATK.
				if (EV_ATK < 252)
				{ //if ATK is not full.
					EV_ATK += intAmount;
					if (EV_ATK > 252)
					{ //if single stat EV cap is passed.
						EV_ATK = 252;
					} //set stat back to the cap.
					return true;
				}
			}
			else if (stat == "DEF")
			{ //if adding to DEF.
				if (EV_DEF < 252)
				{ //if DEF is not full.
					EV_DEF += intAmount;
					if (EV_DEF > 252)
					{ //if single stat EV cap is passed.
						EV_DEF = 252;
					} //set stat back to the cap.
					return true;
				}
			}
			else if (stat == "SPA")
			{ //if adding to SPA.
				if (EV_SPA < 252)
				{ //if SPA is not full.
					EV_SPA += intAmount;
					if (EV_SPA > 252)
					{ //if single stat EV cap is passed.
						EV_SPA = 252;
					} //set stat back to the cap.
					return true;
				}
			}
			else if (stat == "SPD")
			{ //if adding to SPD.
				if (EV_SPD < 252)
				{ //if SPD is not full.
					EV_SPD += intAmount;
					if (EV_SPD > 252)
					{ //if single stat EV cap is passed.
						EV_SPD = 252;
					} //set stat back to the cap.
					return true;
				}
			}
			else if (stat == "SPE")
			{ //if adding to SPE.
				if (EV_SPE < 252)
				{ //if SPE is not full.
					EV_SPE += intAmount;
					if (EV_SPE > 252)
					{ //if single stat EV cap is passed.
						EV_SPE = 252;
					} //set stat back to the cap.
					return true;
				}
			}
		}
		return false; //returns false if total or relevant EV cap was reached before running.
	}*/
	/// <summary>
	/// Returns the maximum HP of this Pokémon.
	/// </summary>
	/// <param name="baseHP"></param>
	/// <param name="level"></param>
	/// <param name="iv"></param>
	/// <param name="ev"></param>
	/// <returns></returns>
	public int calcHP(int baseHP, int level, int iv, int ev)
	{
		if (baseHP == 1) return 1;
		return (int)Math.Floor((decimal)(baseHP * 2 + iv + (ev >> 2)) * level / 100) + level + 10;
	}
	/// <summary>
	/// Returns the specified stat of this Pokémon (not used for total HP).
	/// </summary>
	/// <param name="baseStat"></param>
	/// <param name="level"></param>
	/// <param name="iv"></param>
	/// <param name="ev"></param>
	/// <param name="pv"></param>
	/// <returns></returns>
	public int calcStat(int baseStat, int level, int iv, int ev, int pv)
	{
		return (int)Math.Floor((Math.Floor((decimal)(baseStat * 2 + iv + (ev >> 2)) * level / 100) + 5) * pv / 100);
	}
	public void calcStats()
	{
		int[] pvalues = new int[] { 100, 100, 100, 100, 100 };
		//Nature
	}
	#endregion

	#region Nested Classes
	public partial class PokemonData
	{
		#region Variables
		//private Pokemons id;
		//private int[] regionalPokedex;
		//private enum habitat; //ToDo: Grassland, Mountains...
		/// <summary>
		/// Name of the specific pokemon+form
		/// for given Id in database
		/// <para>Charizard is a Species. 
		/// But megaCharizard is a Form.
		/// </para>
		/// </summary>
		/// <example>
		/// Deoxys Pokedex# can be 1,
		/// but Deoxys-Power id# can be 32
		/// </example>
		private string name { get; set; }
		//private string species;
		//private string pokedexEntry;
		//private int forms; 
		/// <summary>
		/// 
		/// </summary>
		/// ToDo: Instead of string, what if it was pokemon enum?
		/// Enum would sync with xml translation, 
		/// and help with linking pokemon forms with original
		/// Or a new form class, to help with changing base data
		private string[] forms = new string[0]; 
		/*// <summary>
		/// Represents CURRENT form, if no form is active, current or does not exist
		/// then value is 0.
		/// </summary>
		/// ToDo: Make a PokemonForm class, that establishes the rule for 
		/// <see cref="Pokemons"/> and <see cref="Form"/>
		private int form;// = 0; */

		private Types type1 = Types.NONE;
		private Types type2 = Types.NONE;
		/// <summary>
		/// All three pokemon abilities 
		/// (Abiltiy1, Ability2, HiddenAbility).
		/// </summary>
		///	<remarks>
		/// Should be [int? a1,int? a2,int? a3]
		/// instead of above...
		/// </remarks> 
		private readonly Abilities[] ability = new Abilities[3];
		private Abilities ability1 = Abilities.NONE;
		private Abilities ability2 = Abilities.NONE;
		private Abilities abilityh = Abilities.NONE;

		//private GenderRatio maleRatio;
		//private int catchRate;
		private EggGroups eggGroup1 = EggGroups.NONE;
		private EggGroups eggGroup2 = EggGroups.NONE;
		//private int hatchTime;

		//private float hitboxWidth; //used for 3d battles; just use collision detection from models
		//private float height;
		//private float weight;
		//private int shapeID; 

		//private int baseExpYield;
		//private LevelingRate levelingRate;

		//private Color pokedexColor;
		//private int baseFriendship;

		//private int baseStatsHP;
		//private int baseStatsATK;
		//private int baseStatsDEF;
		//private int baseStatsSPA;
		//private int baseStatsSPD;
		//private int baseStatsSPE;

		//private float luminance;
		//private UnityEngine.Color lightColor;
        
		//private int[,] heldItem;

		//private int[] movesetLevels;
		//private Moves[] movesetMoves;
		//private PokemonMoveset[] moveSet;

		//private string[] tmList; //Will be done thru ItemsClass

		//private int[] evolutionID;
		//private string[] evolutionRequirements;
		#endregion
            
		#region Properties
		/// <summary>
		/// Id is the database value for specific pokemon+form
		/// Different Pokemon forms share the same Pokedex number. 
		/// Values are loaded from <see cref="Pokemons"/>, where each form is registered to an Id.
		/// </summary>
		/// <example>
		/// Deoxys Pokedex# can be 1,
		/// but Deoxys-Power id# can be 32
		/// </example>
		/// <remarks>If game event/gm wants to "give" player Deoxys-Power form and not Speed form</remarks>
		public Pokemons ID { get; private set; }
		/// <summary>
		/// Different Gens assign different pokedex num
		/// example: Bulbasaur = [1,231]
		/// </summary>
		/// <remarks>Think there is 3 pokedex</remarks>
		public int[] RegionalPokedex { get; private set; }
		/// <summary>
		/// Name of the specific pokemon+form
		/// for given Id in database
		/// <para>Charizard is a Species. 
		/// But megaCharizard is a Form.
		/// </para>
		/// </summary>
		/// <example>
		/// Deoxys Pokedex# can be 1,
		/// but Deoxys-Power id# can be 32
		/// </example>
		/// ToDo: Form = 0 should return null
		/// public string Name { get { return PokemonData.GetPokedexTranslation(this.ID).Forms[this.Form] ?? this.name; } }
		public string Name { get
            {
                /*List<string> formvalues = new List<string>();
                foreach (var formValue in this.ID.ToString().Translate().FieldNames) {
                    //fieldnames.Add(field);
                    if (formValue.Key.Contains("form")){
                        //_dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value = string.Format(_dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value, fieldnames.ToArray());//(_dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value, _dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].FieldNames)
                        //_dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value = fieldnames.Where() .JoinAsString("; ") +"\n"+ _dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value;//(_dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value, _dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].FieldNames)
                        formvalues.Add(formValue.Value);
                    }
                }
                return formvalues.ToArray()[this.Form] ?? this.name*/
                return this.forms[this.Form] ?? this.name; } }
		/// <summary>
		/// Species is the pokemon breed/genus
		/// <para>Charizard is a Species. 
		/// But megaCharizard is a Form.
		/// </para>
		/// </summary>
		/// <example>
		/// Deoxys-Power, Speed, Defense
		/// are all part of the same species.
		/// </example>
		/// <remarks>Should be an int, not a string</remarks>
		public string Species { get; private set; }
		/// <summary>
		/// </summary>
		public string PokedexEntry { get; private set; }
        /// <summary>
        /// Form is the same Pokemon Pokedex entry. 
        /// Changing forms should change name value.
        /// Represents CURRENT form, if no form is active, current or does not exist
        /// then value is 0.
        /// </summary>
        /// but a different PokemonId
        /// If null, returns this.Pokemon.Id
        /// ToDo: Make a PokemonForm class, that establishes the rule for 
        /// <see cref="Pokemons"/> and <see cref="Form"/>
        /// Maybe the stats set/reated for pokemon of this form #?
        /// Ex. Form 1 would have +10 more HP than base form...
        public int Form { get; set; }
		/// ToDo: I should use the # of Forms from the .xml rather than from the database initializer/constructor
		public int Forms { get { return this.forms.Length; } }

		public EggGroups[] EggGroup { get { return new EggGroups[] { this.eggGroup1, this.eggGroup2 }; } }
		//public EggGroup EggGroup2 { get { return this.eggGroup2; } }
		//public virtual Type Type1 { get { return this.type1; } }
		//public virtual Type Type2 { get { return this.type2; } }
		//Maybe use this instead?
		public virtual Types[] Type { get { return new Types[] { this.type1, this.type2 }; } }
		/// <summary>
		/// All three pokemon abilities 
		/// (Abiltiy1, Ability2, HiddenAbility).
		/// </summary>
		///	<remarks>
		/// Should be [int? a1,int? a2,int? a3]
		/// instead of above...
		/// </remarks> 
        /// ToDo: this.ability;
		public Abilities[] Ability { get { return new Abilities[] { this.ability1, this.ability2, this.abilityh }; } }

		/// <summary>
		/// The male ratio.
		/// <value>-1f is interpreted as genderless</value>
		/// </summary>
		public GenderRatio MaleRatio { get; private set; }
		public float ShinyChance { get; set; }
		/// <summary>
		/// The catch rate of the species. 
		/// Is a number between 0 and 255 inclusive. 
		/// The higher the number, the more likely a capture 
		/// (0 means it cannot be caught by anything except a Master Ball).
		/// </summary> 
		/// Also known as Pokemon's "Rareness"...
		public int CatchRate { get; private set; }
		public int HatchTime { get; private set; }

		//public float hitboxWidth { get { return this.hitboxWidth; } } //used for 3d battles just use collision detection from models
		public float Height { get; private set; }
		public float Weight { get; private set; }
		/// ToDo: Make this an enum
		public int ShapeID { get; private set; } 
        [Obsolete("Please, just use a VOID call to solve color conversion")]
		public UnityEngine.Color LightColor { get; private set; }
		public LevelingRate GrowthRate { get; private set; }
		public Color PokedexColor { get; private set; }

		/// <summary>
		/// Friendship levels is the same as pokemon Happiness.
		/// </summary>
		public int BaseFriendship { get; private set; }
		public int BaseExpYield { get; private set; }
		public int BaseStatsHP  { get; private set; }
		public int BaseStatsATK { get; private set; }
		public int BaseStatsDEF { get; private set; }
		public int BaseStatsSPA { get; private set; }
		public int BaseStatsSPD { get; private set; }
		public int BaseStatsSPE { get; private set; }
		public int evYieldHP  { get; private set; }
		public int evYieldATK { get; private set; }
		public int evYieldDEF { get; private set; }
		public int evYieldSPA { get; private set; }
		public int evYieldSPD { get; private set; }
		public int evYieldSPE { get; private set; }
        /// <summary>
        /// </summary>
        /// Not quite sure what this is for...
		public float Luminance { get; private set; }

		/// <summary>
		/// Returns the items this species can be found holding in the wild.
		/// [item id,% chance]
		/// </summary>
		/// <example>int[,] heldItems = { {1,2,3},{4,5,6} }
		/// <para>heldItems[1,0] == 4 as int</para>
		/// </example>
		/// <remarks>
		/// Or maybe...[item id,% chance,generationId/regionId]
		/// Custom Class needed here... <see cref="Items"/> as itemId
		/// </remarks>
		/// ToDo: Consider [itemcommon || 0,itemuncommon || 0,itemrare || 0]
		public int[,] HeldItem { get; private set; }
        [Obsolete("Please use PokemonMoveTree to query any moves that a pokemon can learn")]
		public int[] MovesetLevels { get; private set; }
        [Obsolete("Please use PokemonMoveTree to query any moves that a pokemon can learn")]
		public Moves[] MovesetMoves { get; private set; }
        /// <summary>
        /// </summary>
        /// Pokemon.cs Line 725: ResetMoves()
        [Obsolete("Please use PokemonMoveTree to query any moves that a pokemon can learn")]
		public PokemonMoveset[] MoveSet { get; private set; }
	    /// <summary>
	    /// All the moves this pokemon species can learn, and the methods by which they learn them
	    /// </summary>
		public PokemonMoveTree MoveTree { get; private set; }
		/// ToDo: Evolution class type array here
		public IPokemonEvolution[] Evolutions { get; private set; }
        [Obsolete("Please use IPokemonEvolution to query any pokemons that a pokemon can evolve into")]
		public int[] EvolutionID { get; private set; }
		/// <summary>
		/// <example>
		/// E.G.	Poliwhirl(61)
		///		<code>new int[]{62,186},
		///		new string[]{"Stone,Water Stone","Trade\Item,King's Rock"}),</code>
		/// <para>
		/// E.G. to evolve to sylveon
		///		<code>new int[]{..., 700},
		///		new string[]{..., "Amie\Move,2\Fairy"}),</code>
		/// </para> 
		/// </example>
		/// <list type="bullet"> 
		/// <item>
		/// <term>Level,int level</term>
		///	<description>if pokemon's level is greater or equal to int level</description>
		///	<item>
		/// <term>Stone,string itemName</term>
		///	<description>if name of stone is equal to string itemName</description>
		///	</item>
		/// <item>
		/// <term>Trade</term>
		///	<description>if currently trading pokemon</description>
		///	</item>
		/// <item>
		/// <term>Friendship</term>
		///	<description>if pokemon's friendship is greater or equal to 220</description>
		///	</item>
		/// <item>
		/// <term>Item,string itemName</term>
		///	<description>if pokemon's heldItem is equal to string itemName</description>
		/// </item>
		///	<item>
		/// <term>Gender,Pokemon.Gender</term>
		/// <description>if pokemon's gender is equal to Pokemon.Gender</description>
		/// </item>
		///	<item>
		/// <term>Move,string moveName</term>
		///	<description>if pokemon has a move thats name or typing is equal to string moveName</description>
		/// </item>
		///	<item>
		///	<term>Map,string mapName</term>
		///	<description>if currentMap is equal to string mapName</description>
		/// </item>
		///	<item>
		///	<term>Time,string dayNight</term>
		///	<description>if time is between 9PM and 4AM time is "Night". else time is "Day".
		///	if time is equal to string dayNight (either Day, or Night)</description>
		/// </item>
		/// <listheader><term>Exceptions</term><description>
		///		Unique evolution methods:
		/// </description></listheader>
		///	<item>
		/// <term>Mantine</term>
		///	<description>if party contains a Remoraid</description>
		/// </item>
		///	<item>
		///	<term>Pangoro</term>
		///	<description>if party contains a dark pokemon</description>
		/// </item>
		///	<item>
		///	<term>Goodra</term>
		///	<description>if currentMap's weather is rain</description>
		/// </item>
		///	<item>
		///	<term>Hitmonlee</term>
		///	<description>if pokemon's ATK is greater than DEF</description>
		/// </item>
		///	<item>
		///	<term>Hitmonchan</term>
		///	<description>if pokemon's ATK is lower than DEF</description>
		/// </item>
		///	<item>
		///	<term>Hitmontop</term>
		/// <description>if pokemon's ATK is equal to DEF</description>
		/// </item>
		///	<item>
		///	<term>Silcoon</term>
		/// <description>if pokemon's shinyValue divided by 2's remainder is equal to 0</description>
		/// </item>
		///	<item>
		///	<term>Cascoon</term>
		///	<description>if pokemon's shinyValue divided by 2's remainder is equal to 1</description>
		/// </item>
		/// </list>
		/// </summary>
        [Obsolete("Please use IPokemonEvolution to query any pokemons that a pokemon can evolve into")]
		public string[] EvolutionRequirements { get; private set; }

		/// <summary>
		/// The item that needs to be held by a parent when breeding in order for the egg to be this species. 
		/// If neither parent is holding the required item, the egg will be the next evolved species instead.
		/// <para></para>
		/// The only species that should have this line are ones which cannot breed, 
		/// but evolve into a species which can. That is, the species should be a "baby" species.
		/// Not all baby species need this line.
		/// </summary>
		public Items Incense { get; private set; }
		#endregion

		#region Constructors
		//public PokemonData() { }// this.name = PokemonData.GetPokedexTranslation(this.ID).Forms[this.Form] ?? this.Name; } //name equals form name unless there is none.
		public PokemonData(Pokemons Id) //: this()
        {
            //PokedexTranslation translation = PokemonData.GetPokedexTranslation(Id);
            var translation = Id.ToString().Translate();
            this.ID = Id;
            //this.name = translation.Name;
            //this.species = translation.Species;
            //this.pokedexEntry = translation.PokedexEntry;
            this.PokedexEntry = translation.Value.Trim('\n');
            //this.forms = forms; //| new Pokemon[] { Id }; //ToDo: need new mechanic for how this should work
            List<string> formvalues = new List<string>();
            foreach (var fieldValue in translation.FieldNames)
            {
                //fieldnames.Add(field);
                if (fieldValue.Key.Contains("form"))
                {
                    //_dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value = string.Format(_dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value, fieldnames.ToArray());//(_dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value, _dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].FieldNames)
                    //_dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value = fieldnames.Where() .JoinAsString("; ") +"\n"+ _dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value;//(_dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value, _dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].FieldNames)
                    formvalues.Add(fieldValue.Value);
                }
                if (fieldValue.Key.Contains("genus"))
                {
                    this.Species = fieldValue.Value;
                }
                if (fieldValue.Key.Contains("name"))
                {
                    this.name = fieldValue.Value ?? translation.Identifier;
                }
            }
            this.forms = formvalues.ToArray();
        }

		public PokemonData(Pokemons Id = Pokemons.NONE, int[] regionalDex = null, //string name, 
                            Types type1 = Types.NONE, Types type2 = Types.NONE, Abilities ability1 = Abilities.NONE, Abilities ability2 = Abilities.NONE, Abilities hiddenAbility = Abilities.NONE,//Abilities[] abilities,
							GenderRatio genderRatio = GenderRatio.Genderless, float? maleRatio = null, int catchRate = 1, EggGroups eggGroup1 = EggGroups.NONE, EggGroups eggGroup2 = EggGroups.NONE, int hatchTime = 0,
							float height = 0f, float weight = 0f, int baseExpYield = 0, LevelingRate levelingRate = LevelingRate.MEDIUMFAST,
							//int? evYieldHP, int? evYieldATK, int? evYieldDEF, int? evYieldSPA, int? evYieldSPD, int? evYieldSPE,
							int evHP = 0, int evATK = 0, int evDEF = 0, int evSPA = 0, int evSPD = 0, int evSPE = 0,
							Color pokedexColor = Color.NONE, int baseFriendship = 0,//* / string species, string pokedexEntry,*/
							int baseStatsHP = 0, int baseStatsATK = 0, int baseStatsDEF = 0, int baseStatsSPA = 0, int baseStatsSPD = 0, int baseStatsSPE = 0,
							float luminance = 0f, //Color lightColor,
                            PokemonMoveset[] movesetmoves = null,
                            int[] movesetLevels = null, Moves[] movesetMoves = null, int[] tmList = null, 
                            IPokemonEvolution[] evolution = null,
                            int[] evolutionID = null, int[] evolutionLevel = null, int[] evolutionMethod = null, /*string[] evolutionRequirements,*/ 
                            //ToDo: What if: `Pokemons form` to point back to base pokemon, and Pokemons.NONE, if they are the base form?
                            //that way, we can assign values to pokemons with forms that give stat bonuses...
                            //want to find a way to add pokemon froms from a different method. Maybe something like overwriting the `Database` values to match those of base pokemon for values that are duplicated.
                            //Or I'll just add it at the bottom towards end of array using copy-paste method.
                            Pokemons baseForm = Pokemons.NONE, //int forms = 0, 
                            int[,] heldItem = null) : this (Id)
        {//new PokemonData(1,1,"Bulbasaur",12,4,65,null,34,45,1,7,20,7f,69f,64,4,PokemonData.PokedexColor.GREEN,"Seed","\"Bulbasaur can be seen napping in bright sunlight. There is a seed on its back. By soaking up the sun’s rays, the seed grows progressively larger.\"",45,49,49,65,65,45,0f,new int[]{1,3,7,9,13,13,15,19,21,25,27,31,33,37},new int[]{33,45,73,22,77,79,36,75,230,74,38,388,235,402},new int[]{14,15,70,76,92,104,113,148,156,164,182,188,207,213,214,216,218,219,237,241,249,263,267,290,412,447,474,496,497,590},new int[]{2},new int[]{16},new int[]{1})
            this.RegionalPokedex = regionalDex;

            this.type1 = type1; //!= null ? (Types)type1 : Types.NONE;
            this.type2 = type2; //!= null ? (Types)type2 : Types.NONE;
			//this.ability = abilities;
			this.ability1 = (Abilities)ability1;
			this.ability2 = (Abilities)ability2;
			this.abilityh = (Abilities)hiddenAbility;

			this.MaleRatio = maleRatio.HasValue? getGenderRatio(maleRatio.Value) : genderRatio; //ToDo: maleRatio; maybe `GenderRatio genderRatio(maleRatio);`
			this.CatchRate = catchRate;
			this.eggGroup1 = eggGroup1;
			this.eggGroup2 = eggGroup2;
			this.HatchTime = hatchTime;

			this.Height = height;
			this.Weight = weight;
			this.BaseExpYield = baseExpYield;
			this.GrowthRate = (LevelingRate)levelingRate; //== null ? (LevelingRate)levelingRate : LevelingRate.NONE;

			this.evYieldHP  = evHP;
			this.evYieldATK = evATK;
			this.evYieldDEF = evDEF;
			this.evYieldSPA = evSPA;
			this.evYieldSPD = evSPD;
			this.evYieldSPE = evSPE;

			this.BaseStatsHP  = baseStatsHP;
			this.BaseStatsATK = baseStatsATK;
			this.BaseStatsDEF = baseStatsDEF;
			this.BaseStatsSPA = baseStatsSPA;
			this.BaseStatsSPD = baseStatsSPD;
			this.BaseStatsSPE = baseStatsSPE;
			this.BaseFriendship = baseFriendship; 

			this.Luminance = luminance;
			//this.lightColor = lightColor;
			this.PokedexColor = pokedexColor | Color.NONE;

			//ToDo: wild pokemon held items not yet implemented
			this.HeldItem = heldItem; //[item id,% chance]

            this.MoveTree = new PokemonMoveTree(movesetmoves);
            //this.MovesetLevels = movesetLevels;
			//this.MovesetMoves = movesetMoves; 
            //this.tmList = tmList; 

            this.Evolutions = evolution;
			//this.EvolutionID = evolutionID;
			//this.evolutionMethod = evolutionMethod; 
			//this.evolutionRequirements = evolutionRequirements;
		}

        [Obsolete]
		public static PokemonData CreatePokemonData(Pokemons Id, int[] PokeId/*, string name*/, int? type1, int? type2, int? ability1, int? ability2, int? hiddenAbility,
							float maleRatio, int catchRate, int? eggGroup1, int? eggGroup2, int hatchTime,
							float height, float weight, int baseExpYield, int levelingRate,
							/*int? evYieldHP, int? evYieldATK, int? evYieldDEF, int? evYieldSPA, int? evYieldSPD, int? evYieldSPE,*/
							Color pokedexColor, int baseFriendship,//*/ string species, string pokedexEntry,
							int baseStatsHP, int baseStatsATK, int baseStatsDEF, int baseStatsSPA, int baseStatsSPD, int baseStatsSPE,
							float luminance, /*Color lightColor,*/ int[] movesetLevels, int[] movesetMoves, int[] tmList,
							int[] evolutionID, int[] evolutionLevel, int[] evolutionMethod, /*string[] evolutionRequirements,*/ int forms,
							int[,] heldItem = null)
		{
			return CreatePokemonData(//new PokemonData(
				(Pokemons)Id,
				PokeId,
				(Types)type1 | Types.NONE,//!= null ? (PokemonData.Type)type1 : PokemonData.Type.NONE,
				(Types)type2 | Types.NONE,//!= null ? (PokemonData.Type)type2 : PokemonData.Type.NONE,
                //new Abilities[] { 
					(Abilities)ability1 | Abilities.NONE,//!= null ? (Abilities)ability1 : Abilities.NONE,
					(Abilities)ability2 | Abilities.NONE,//!= null ? (Abilities)ability2 : Abilities.NONE,
					(Abilities)hiddenAbility | Abilities.NONE,//!= null ? (Abilities)hiddenAbility : Abilities.NONE
                //}, 
				0,//ToDo: maleRatio, 
				catchRate,
				(EggGroups)eggGroup1 | EggGroups.NONE,//!= null ? (EggGroups)eggGroup1 : PokemonData.EggGroup.NONE, 
				(EggGroups)eggGroup2 | EggGroups.NONE,//!= null ? (EggGroups)eggGroup2 : PokemonData.EggGroup.NONE, 
				hatchTime,
				height,
				weight,
				baseExpYield,
				(LevelingRate)levelingRate,
				pokedexColor | Color.NONE,
				baseFriendship, baseStatsHP, baseStatsATK, baseStatsDEF, baseStatsSPA, baseStatsSPD, baseStatsSPE,
				luminance, movesetLevels, System.Array.ConvertAll(movesetMoves, move => (Moves)move), tmList,
				evolutionID, evolutionLevel, evolutionMethod, forms, heldItem);//
		}
        [Obsolete]
		public static PokemonData CreatePokemonData(Pokemons Id, int[] PokeId/*, string name*/, Types type1, Types type2, Abilities ability1, Abilities ability2, Abilities hiddenAbility,
							GenderRatio maleRatio, int catchRate, EggGroups eggGroup1, EggGroups eggGroup2, int hatchTime,
							float height, float weight, int baseExpYield, LevelingRate levelingRate,
							/*int? evYieldHP, int? evYieldATK, int? evYieldDEF, int? evYieldSPA, int? evYieldSPD, int? evYieldSPE,*/
							Color pokedexColor, int baseFriendship,//*/ string species, string pokedexEntry,
							int baseStatsHP, int baseStatsATK, int baseStatsDEF, int baseStatsSPA, int baseStatsSPD, int baseStatsSPE,
							float luminance, /*Color lightColor,*/ int[] movesetLevels, Moves[] movesetMoves, int[] tmList,
							int[] evolutionID, int[] evolutionLevel, int[] evolutionMethod, /*string[] evolutionRequirements,*/ int forms,
							int[,] heldItem = null)
		{
			return new PokemonData(
				Id,
				PokeId,
				type1, //| PokemonData.Type.NONE,//!= null ? (PokemonData.Type)type1 : PokemonData.Type.NONE,
				type2, //| PokemonData.Type.NONE,//!= null ? (PokemonData.Type)type2 : PokemonData.Type.NONE,
				//new Abilities[] {
				ability1, //| Abilities.NONE,//!= null ? (Abilities)ability1 : Abilities.NONE,
                ability2, //| Abilities.NONE,//!= null ? (Abilities)ability2 : Abilities.NONE,
                hiddenAbility, //| Abilities.NONE,//!= null ? (Abilities)hiddenAbility : Abilities.NONE
				//},
				maleRatio,  //gender
                0f,         //gender
				catchRate,
				eggGroup1, //| PokemonData.EggGroups.NONE,//!= null ? (EggGroups)eggGroup1 : PokemonData.EggGroup.NONE, 
				eggGroup2, //| PokemonData.EggGroups.NONE,//!= null ? (EggGroups)eggGroup2 : PokemonData.EggGroup.NONE, 
				hatchTime,
				height,
				weight,
				baseExpYield,
				levelingRate,
                0,0,0,0,0,0,
				pokedexColor | Color.NONE,
				baseFriendship, baseStatsHP, baseStatsATK, baseStatsDEF, baseStatsSPA, baseStatsSPD, baseStatsSPE,
				luminance, null, movesetLevels, movesetMoves, /*System.Array.ConvertAll(movesetMoves, move => (Move.MoveData.Move)move),*/ tmList, null,
				evolutionID, evolutionLevel, evolutionMethod, (Pokemons)forms, heldItem);//
		}
        #region Obsolete Database Values
        /*// Not const because translation values
        public static readonly PokemonData[] Database = new PokemonData[] {
            new PokemonData( Id: Pokemons.NONE, regionalDex: new int[1], type1: Types.NONE, type2: Types.NONE, ability1: Abilities.NONE, ability2: Abilities.NONE, hiddenAbility: Abilities.NONE,
                        maleRatio: GenderRatio.AlwaysMale /*0f*, catchRate: 100, eggGroup1: EggGroups.NONE, eggGroup2: EggGroups.NONE, hatchTime: 1000,
                        height: 10f, weight: 150f, baseExpYield: 15, levelingRate: LevelingRate.ERRATIC,
                        //int? evYieldHP, int? evYieldATK, int? evYieldDEF, int? evYieldSPA, int? evYieldSPD, int? evYieldSPE,
                        pokedexColor: Color.NONE, baseFriendship: 50,
                        baseStatsHP: 10, baseStatsATK: 5, baseStatsDEF: 5, baseStatsSPA: 5, baseStatsSPD: 5, baseStatsSPE: 5,
                        luminance: 0f, 
                        movesetmoves: new PokemonMoveset[] { new PokemonMoveset(moveId: Moves.ACID_ARMOR, method: LearnMethod.levelup, level: 15) },
                        //movesetLevels: new int[] { 1,2,3 }, movesetMoves: new Moves[4], tmList: null, 
                        evolution: new IPokemonEvolution[] {  new PokemonEvolution(Pokemons.ABRA, EvolutionMethod.Deaths), new PokemonEvolution<int>(Pokemons.ABRA, EvolutionMethod.Deaths, 25) },
                        //evolutionID: null, evolutionLevel: null, evolutionMethod: null, 
                        forms: 4, heldItem: null) //Test
        };*/
        #endregion

        #region Obsolete Translation Method
#if DEBUG
        [Obsolete]
		private static Dictionary<int, PokedexTranslation> _pokeTranslations;// = LoadPokedexTranslations();
#else
        [Obsolete]
        private static Dictionary<int, PokedexTranslation> _pokeTranslations;// = LoadPokedexTranslations(SaveData.currentSave.playerLanguage | Settings.Language.English);
#endif
		/// <summary>
		/// 
		/// </summary>
        [Obsolete]
		private static Dictionary<int, PokedexTranslation> _pokeEnglishTranslations;// = LoadEnglishPokedexTranslations();
		/// <summary>
		/// 
		/// </summary>
		///ToDo: Should be a void that stores value to _pokeTranslations instead of returning...
        [Obsolete]
		public static void/*Dictionary<int, PokedexTranslation>*/ LoadPokedexTranslations(Settings.Languages language = Settings.Languages.English)//, int form = 0
		{
			var data = new Dictionary<int, PokedexTranslation>();

			string fileLanguage;
			switch (language)
			{
				case Settings.Languages.English:
					fileLanguage = "en-us";
					break;
				default: //Default in case new language is added to game but not programmed ahead of time here...
					fileLanguage = "en-us";
					break;
			}
			System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument(); // xmlDoc is the new xml document.
			//ToDo: Consider "/Resources/Database/PokemonTranslations/Pokemon_"
#if DEBUG
			string file = @"..\..\..\\Pokemon Unity\Assets\Resources\Database\Pokemon\Pokemon_" + fileLanguage + ".xml"; //TestProject\bin\Debug
			//string file = System.Environment.CurrentDirectory + @"\Resources\Database\Pokemon\Pokemon_" + fileLanguage + ".xml"; //TestProject\bin\Debug
			//string file =  @"$(SolutionDir)\Assets\Resources\Database\Pokemon\Pokemon_" + fileLanguage + ".xml"; //Doesnt work
#else
        string file = UnityEngine.Application.dataPath + "/Resources/Database/Pokemon/Pokemon_" + fileLanguage + ".xml"; //Use for production
#endif
			System.IO.FileStream fs = new System.IO.FileStream(file, System.IO.FileMode.Open);
			xmlDoc.Load(fs);

			if (xmlDoc.HasChildNodes)
			{
				foreach (System.Xml.XmlNode node in xmlDoc.GetElementsByTagName("Pokemon"))
				{
					var translation = new PokedexTranslation();
					translation.Name = node.Attributes["name"].Value; //ToDo: Name = "name" Where formId == 0; else name = formId.name
					//ToDo: Maybe add a forms array, and a new method for single name calls
					translation.Forms = new string[node.Attributes.Count - 3]; int n = 1;//only count forms?
					//int n = 1;translation.Forms[0] = node.Attributes["name"].Value;//that or return an empty array T[0]
					for (int i = 4; i < node.Attributes.Count; i++)//foreach(System.Xml.XmlAttribute attr in node)
					{
						//Skipping first 4 values will save processing
						if (node.Attributes[i].LocalName.Contains("form")) //Name vs LocalName?
						{
							//translation.Forms[i-4] = node.Attributes[i].Value; //limits xml to only 4 set values 
							translation.Forms[n] = node.Attributes[i].Value; n++;
						}
					}
					translation.Species = node.Attributes["genus"].Value;
					translation.PokedexEntry = node.InnerText;
					data.Add(int.Parse(node.Attributes["id"].Value), translation); //Is this safe? Possible overwritting of values with bad entries
				}
			}

			//ToDo: Is filestream still open or does it need to be closed and disposed of?
			fs.Dispose(); fs.Close();
			_pokeTranslations = data;//return data;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="language"></param>
		/// <returns></returns>
		/// <remarks>ToDo: If not in foreign language, check and load in English; else...</remarks>
        [Obsolete]
		public static PokedexTranslation GetPokedexTranslation(Pokemons id, Settings.Languages language = Settings.Languages.English)// int form = 0,
		{
			if (_pokeTranslations == null) //should return english if player's default language is null
			{
				LoadPokedexTranslations(language);//, form
			}

			int arrayId = (int)id;// GetPokemon(id).ArrayId; //unless db is set, it'll keep looping null...
			if (!_pokeTranslations.ContainsKey(arrayId) && language == Settings.Languages.English)
			{
				//Debug.LogError("Failed to load pokedex translation for pokemon with id: " + (int)id); //ToDo: Throw exception error
				throw new System.Exception(string.Format("Failed to load pokedex translation for pokemon with id: {0}_{1}", (int)id, id.ToString()));
				//return new PokedexTranslation();
			}
			//ToDo: Show english text for missing data on foreign languages 
			else if (!_pokeTranslations.ContainsKey(arrayId) && language != Settings.Languages.English)
			{
				return _pokeEnglishTranslations[arrayId];
			}

			return _pokeTranslations[arrayId];// int id
		}
		#endregion
        #endregion

		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="ID"></param>
		/// <returns></returns>
		public static PokemonData GetPokemon(Pokemons ID)
		{
			//Debug.Log("Get Pokemons");
			/*PokemonData result = null;
			int i = 1;
			while(result == null){
				if(Database[i].ID == ID){
					//Debug.Log("Pokemon DB Success");
					return result = Database[i];
				}
				i += 1;
				if(i >= Database.Length){
					Debug.Log("Pokemon DB Fail");
					return null;}
			}
			return result;*/
			foreach (PokemonData pokemon in Database)
			{
				if (pokemon.ID == ID) return pokemon;
			}
			throw new System.Exception("Pokemon ID doesnt exist in the database. Please check PokemonData constructor.");
			//return null;
		}

		static int getPokemonArrayId(Pokemons ID)
		{
			//Debug.Log("Get Pokemons");
			/*PokemonData result = null;
			int i = 1;
			while(result == null){
				if(Database[i].ID == ID){
					//Debug.Log("Pokemon DB Success");
					return result = Database[i];
				}
				i += 1;
				if(i >= Database.Length){
					Debug.Log("Pokemon DB Fail");
					return null;}
			}
			return result;*/
			/*foreach(PokemonData pokemon in Database)
			{
				if (pokemon.ID == ID) return pokemon;
			}*/
			for (int i = 0; i < Database.Length; i++)
			{
				if (Database[i].ID == ID)
				{
					return i;
				}
			}
			throw new System.Exception("Pokemon ID doesnt exist in the database. Please check PokemonData constructor.");
		}

		/// <summary>
		/// Returns int value of Pokemon from PokemonData[] <see cref="Database"/>
		/// </summary>
		public int ArrayId
		{//(Pokemon ID)
			get
			{
				//Debug.Log("Get Pokemons");
				/*PokemonData result = null;
				int i = 1;
				while(result == null){
					if(Database[i].ID == ID){
						//Debug.Log("Pokemon DB Success");
						return result = Database[i];
					}
					i += 1;
					if(i >= Database.Length){
						Debug.Log("Pokemon DB Fail");
						return null;}
				}
				return result;*/
				/*foreach(PokemonData pokemon in Database)
				{
					if (pokemon.ID == ID) return pokemon;
				}*/
				for (int i = 0; i < Database.Length; i++)
				{
					if (Database[i].ID == ID)
					{
						return i;
					}
				}
				throw new System.Exception("Pokemon ID doesnt exist in the database. Please check PokemonData constructor.");
			}
		}
        /*
		/// <summary>
		/// [ability1,ability2,hiddenAbility]
		/// </summary>
		/// <example>int[,] Abilities = getAbilities() = { {1,2,3} }
		/// <para>int hiddenAbility = Abilities[0,3]</para>
		/// </example>
		/// <remarks>Something i think would work better than a string</remarks>
		public string getAbility(int ability){
			switch (ability)
			{
				case 0:
					return ability1;
				case 1:
					return ability2;
				case 3:
					return hiddenAbility;
				default:
					return null;
			}
		}

		public string[] GenerateMoveset(int level){
			//Set moveset based off of the highest level moves possible.
			string[] moveset = new string[4];
			int i = movesetLevels.Length-1; //work backwards so that the highest level move possible is grabbed first
			while(moveset[3] == null){
				if(movesetLevels[i] <= level){
					moveset[3] = movesetMovesStrings[i];
				}
				i -= 1;
			}
			if(i >= 0){ //if i is at least 0 still, then you can grab the next move down.
				moveset[2] = movesetMovesStrings[i];
				i -= 1;
				if(i >= 0){ //if i is at least 0 still, then you can grab the next move down.
					moveset[1] = movesetMovesStrings[i];
					i -= 1;
					if(i >= 0){ //if i is at least 0 still, then you can grab the last move down.
						moveset[0] = movesetMovesStrings[i];
						i -= 1;
					}
				}
			}
			i = 0;
			int i2 = 0;			//if the first move is null, then the array will need to be packed down
			if (moveset[0] == null){ 		//(nulls moved to the end of the array)
				while(i < 3){ 
					while(moveset[i] == null){
						i += 1;
					}
					moveset[i2] = moveset[i];
					moveset[i] = null;
					i2 += 1;
				}
			}
			return moveset;
		}

		/// <summary>
		/// Converts the string of a Pokemon Type to a Color in Unity. 
		/// </summary>
		/// <param name="PokemonType">string of pokemon type or name of a color</param>
		/// <returns>return a Unity.Color</returns>
		/// <example>StringToColor(Electric)</example>
		/// <example>StringToColor(Yellow)</example>
		/// <remarks>might need to make a new enum in PokemonData, type = x.Color...</remarks>
		public Color StringToColor(string PokemonType) {
			//private System.Collections.Generic.Dictionary<string, Color> StringToColorDic = new System.Collections.Generic.Dictionary<string, Color>() {//Dictionary<PokemonData.Type, Color>
			//http://www.epidemicjohto.com/t882-type-colors-hex-colors
			//Normal Type: A8A77A
			//Fire Type:  EE8130
			//Water Type: 6390F0
			//Electric Type:  F7D02C
			//Grass Type: 7AC74C
			//Ice Type: 96D9D6
			//Fighting Type:  C22E28
			//Poison Type: A33EA1
			//Ground Type: E2BF65
			//Flying Type: A98FF3
			//Psychic Type: F95587
			//Bug Type: A6B91A
			//Rock Type: B6A136
			//Ghost Type: 735797
			//Dragon Type:  6F35FC
			//Dark Type: 705746
			//Steel Type:  B7B7CE
			//Fairy Type: D685AD

			//http://www.serebiiforums.com/showthread.php?289595-Pokemon-type-color-associations
			//Normal -white
			//Fire - red
			//Water -blue
			//Electric -yellow
			//Grass - green
			//Ice - cyan
			//Poison -purple
			//Psychic - magenta
			//Fighting - dark red
			//Ground - brown
			//Rock - gray
			//Bug - yellow green
			//Flying - light blue
			//Dragon - orange
			//Ghost - light purple
			//Steel - dark gray
			//Dark - black

			/*
			//{"Black",Color.black },//dark
			//{"", new Color() },//dark blue -> dark, 
			{ "Blue",Color.blue },//water
			{ "Clear",Color.clear },
			{ "Cyan",Color.cyan },
			{ "Gray",Color.gray },//grAy-American
			//{"Grey",Color.grey },//grEy-European
			//{"Grey",Color.grey },//dark grey -> rock,
			{ "Green",Color.green },//grass
			//{"", new Color() },//dark green -> bug,
			{ "Magenta",Color.magenta },//magenta, purple -> poison
			{ "Red",Color.red },//orange, dark red -> fire
			{ "White",Color.white },//normals
			{ "Yellow",Color.yellow },//electric
			{ "Purple", new Color() },//ghost
			{ "Brown", new Color() },//fighting
			{ "Pink", new Color() }//,//fairy
			//{"", new Color() },//pink, lavender -> psychic, 
			//{"", new Color() },//ocre, brown -> ground
			//{"", new Color() },
			//{"", new Color() },
			//{"", new Color() }//fly, drag, steel, psychic, ice, shadow, unknown, bug, ground, poison?
			* /
			return new UnityEngine.Color();//StringToColorDic[PokemonType];
		}
		/// <summary>
		/// Converts the Pokemon Type to a Color in Unity. 
		/// </summary>
		/// <param name="PokemonType">pokemon type</param>
		/// <returns>return a Unity.Color</returns>
		/// <example>StringToColor(Electric)</example>
		public Color StringToColor(PokemonData.Type PokemonType) {
			return StringToColor(PokemonType.ToString()); //Will fix later
		}
		/// <summary>
		/// Only an example. Do not use, will  not work.
		/// <para>Could be combined with database values 
		/// and used with ints instead of strings</para>
		/// <para>Convert the pokemon type into a color 
		/// that can be used with Unity's color lighting</para>
		/// </summary>
		/// <param name="color"></param>
		/// <returns></returns>
		public Color StringToColor(int color) {
			switch (color)
			{
				//case 1:
				//	return StringToColorDic["text"];
				default:
					return StringToColor(color.ToString());
			}
		}*/

        GenderRatio getGenderRatio(float maleRatioPercent)
        {
            /*switch ((int)maleRatioPercent)
            {
                case  -1:
                default:
                    return GenderRatio.Genderless;
                    //break;
            }*/
            if (maleRatioPercent == 100f) return GenderRatio.AlwaysMale;
            else if (maleRatioPercent == 0f) return GenderRatio.AlwaysFemale;
            else if (maleRatioPercent > 0f && maleRatioPercent < 12.5f) return GenderRatio.AlwaysFemale;
            else if (maleRatioPercent >= 12.5f && maleRatioPercent < 25f) return GenderRatio.FemaleSevenEighths;
            else if (maleRatioPercent >= 25f && maleRatioPercent < 37.5f) return GenderRatio.Female75Percent;
            else if (maleRatioPercent >= 37.5f && maleRatioPercent < 50f) return GenderRatio.Female75Percent;
            else if (maleRatioPercent >= 50f && maleRatioPercent < 62.5f) return GenderRatio.Female50Percent;
            else if (maleRatioPercent >= 62.5f && maleRatioPercent < 75f) return GenderRatio.Female50Percent;
            else if (maleRatioPercent >= 75f && maleRatioPercent < 87.5f) return GenderRatio.Female25Percent;
            else if (maleRatioPercent >= 87.5f && maleRatioPercent < 100f) return GenderRatio.FemaleOneEighth;
            else if (maleRatioPercent < 0 || maleRatioPercent > 100f) return GenderRatio.Genderless;
            else return GenderRatio.Genderless;
        }
        #endregion

        #region Nested Classes
        /// <summary>
        /// The moves that all Pokémon of the species learn as they level up. 
        /// </summary>
        public class PokemonMoveset
	    {
		    public LearnMethod TeachMethod;
		    /// <summary>
		    /// Level at which the move is learned 
		    /// (0 means the move can only be learned when 
		    /// a Pokémon evolves into this species).
            /// Default level is 0; 
            /// only really important if learn method is <see cref="LearnMethod.levelup"/>
		    /// </summary>
		    /// Level needed to learn Move
            /// leaves door open to new game designs, if players want to limit move methods to a level requirement
            /// use to set min or max level needed for designated learn method
		    public int Level;
		    /// <summary>
		    /// Move learned upon leveling-up
		    /// </summary>
		    public Moves MoveId;
			public int Generation;
		    //public PokemonMoveset() { }
		    /*public PokemonMoveset(Move.MoveData.Move move, int level)
		    {
			    this.Level = level;
			    this.MoveId = move;
		    }
		    public PokemonMoveset(Move.MoveData.Move move)
		    {
			    this.MoveId = move;
		    }*/
		    public PokemonMoveset(Moves moveId, LearnMethod method = LearnMethod.levelup, int level = 0, int? generation = null) //: this()
		    {
			    this.Level = level;
			    this.MoveId = moveId;
                this.TeachMethod = method;
				if (generation != null) this.Generation = generation.Value;
		    }
	    }
	    /// <summary>
	    /// All the moves this pokemon species can learn, and the methods by which they learn them
	    /// </summary>
	    public class PokemonMoveTree
	    {
            #region Properties
            /// <summary>
            /// to use: LevelUp.OrderBy(x => x.Value).ThenBy(x => x.Key)
            /// </summary>
            public SortedList<Moves, int> LevelUp { get; private set; }
		    public Moves[] Egg { get; private set; }
            public Moves[] Tutor { get; private set; }
		    public Moves[] Machine { get; private set; }
            //Teach pikachu surf?... do we really need it to know surf?... Maybe if "specal form" (surfboard pickachu) is added to game
            //public Move.MoveData.Move[] stadium_surfing_pikachu { get; private set; }
            /// <summary>
            /// If <see cref="Items.LIGHT_BALL"/> is held by either parent of a <see cref="Pokemons.Pichu"/> when the Egg is produced,
            /// the Pichu that hatches will know the move <see cref="Move.MoveData.Move.Volt_Tackle"/>.
            /// </summary>
            /// Not sure about this one
            public Moves[] light_ball_egg { get; private set; }
            //public Move.MoveData.Move[] colosseum_purification { get; private set; }
            //public Move.MoveData.Move[] xd_shadow { get; private set; }
            //public Move.MoveData.Move[] xd_purification { get; private set; }
            /// <summary>
            /// </summary>
            /// Merge both Colosseum and XD into one list
            public Moves[] Shadow { get; private set; }
            /// <summary>
            /// When a pokemon is purified from a shadow state, the moves they can potentially unlock?...
            /// </summary>
            /// Merge both Colosseum and XD into one list
            public Moves[] Purification { get; private set; }
            public Moves[] FormChange { get; private set; }
            #endregion
            //public PokemonMoveTree() { }
            public PokemonMoveTree(
                    SortedList<Moves, int> levelup = null,
                    Moves[] egg = null,
                    Moves[] tutor = null,
                    Moves[] machine = null,
                    //Move.MoveData.Move[] stadium_surfing_pikachu = null,
                    //Move.MoveData.Move[] light_ball_egg = null,
                    Moves[] shadow = null,
                    Moves[] purification = null,
                    Moves[] form_change = null
                )
            {
                this.LevelUp = levelup ?? new SortedList<Moves, int>();                       
			    this.Egg = egg ?? new Moves[0];
                this.Tutor = tutor ?? new Moves[0];
                this.Machine = machine ?? new Moves[0];
                //this.stadium_surfing_pikachu = 5,
                //this.light_ball_egg = light_ball_egg ?? new Move.MoveData.Move[0];
                this.Shadow = shadow ?? new Moves[0];
                this.Purification = purification ?? new Moves[0];
                this.FormChange = form_change ?? new Moves[0];
            }
            public PokemonMoveTree(PokemonMoveset[] moveset)
            { 
                #region Foreach-Loop
                SortedList<Moves, int> level = new SortedList<Moves, int>();
                List<Moves> egg = new List<Moves>();
                List<Moves> tutor = new List<Moves>();
                List<Moves> machine = new List<Moves>();
                List<Moves> shadow = new List<Moves>();
                List<Moves> purify = new List<Moves>();
                List<Moves> form = new List<Moves>();
                foreach (PokemonMoveset move in moveset)
                {
					//ToDo: Generation Filter
					//if(move.Generation == Settings.pokemonGeneration || Settings.pokemonGeneration < 1)
                    switch (move.TeachMethod)
                    {
                        case LearnMethod.levelup:
                            if (!level.ContainsKey(move.MoveId)) level.Add(move.MoveId, move.Level);
                            break;
                        case LearnMethod.egg:
                            if (!egg.Contains(move.MoveId)) egg.Add(move.MoveId);
                            break;
                        case LearnMethod.tutor:
                            if (!tutor.Contains(move.MoveId)) tutor.Add(move.MoveId);
                            break;
                        case LearnMethod.machine:
                            if (!machine.Contains(move.MoveId)) machine.Add(move.MoveId);
                            break;
                        case LearnMethod.stadium_surfing_pikachu:
                            break;
                        case LearnMethod.light_ball_egg:
                            break;
                        case LearnMethod.purification:
                        case LearnMethod.xd_purification:
                        case LearnMethod.colosseum_purification:
                            if (!purify.Contains(move.MoveId)) purify.Add(move.MoveId);
                            break;
                        case LearnMethod.shadow:
                        case LearnMethod.xd_shadow:
                            if (!shadow.Contains(move.MoveId)) shadow.Add(move.MoveId);
                            break;
                        case LearnMethod.form_change:
                            if (!form.Contains(move.MoveId)) form.Add(move.MoveId);
                            break;
                        default:
                            break;
                    }
                }
                #endregion
                /*return PokemonMoveTree(
                        levelup: level,
                        egg: egg.ToArray(),
                        tutor: tutor.ToArray(),
                        machine: machine.ToArray(),
                        shadow: shadow.ToArray(),
                        purification: purify.ToArray(),
                        form_change: form.ToArray()
                    );*/
                this.LevelUp = level;
                this.Egg = egg.ToArray();
                this.Tutor = tutor.ToArray();
                this.Machine = machine.ToArray();
                this.Shadow = shadow.ToArray();
                this.Purification = purify.ToArray();
                this.FormChange = form.ToArray();
            }
        }
	    /// <summary>
	    /// The evolution paths this species can take. 
	    /// For each possible evolution of this species, 
	    /// there are three parts
	    /// </summary>
	    public class PokemonEvolution : IPokemonEvolution //<T> where T : new()
	    {
		    /// <summary>
		    /// The PokemonId of the evolved species.
		    /// The PokemonId of the species this pokemon evolves into.
		    /// </summary>
		    public Pokemons Species { get; private set; }
		    /// <summary>
		    /// The evolution method.
		    /// </summary>
		    public EvolutionMethod EvolveMethod { get; private set; }
            //public object EvolutionMethodValue;
            //public PokemonEvolution<T> EvolutionMethodValue;
            //public class T { }
            //public PokemonEvolution(){}
            public PokemonEvolution(Pokemons EvolveTo, EvolutionMethod EvolveHow){
                this.Species = EvolveTo;
                this.EvolveMethod = EvolveHow;
            }
            /*public PokemonEvolution(Pokemons EvolveTo, EvolutionMethod EvolveHow, Type ValueType, object ObjectValue) 
            {
                PokemonEvolution<ValueType>(EvolveTo, EvolveHow, ObjectValue);
            }*/
            public virtual bool isGenericT()
            {
                return false;
            }
        }
	    public class PokemonEvolution<T> : PokemonEvolution
	    {
		    /*// <summary>
		    /// The PokemonId of the evolved species.
		    /// </summary>
		    public PokemonData.Pokemon EvolvesTo;
		    /// <summary>
		    /// The evolution method.
		    /// </summary>
		    public int EvolveMethod;
		    //public object EvolveValue;
		    //public T EvolveValue<T>() { return GetValue(); };*/
		    /// <summary>
		    /// The value-parameter to <see cref="EvolveMethod"/> as mentioned KEY.
		    /// </summary>
		    public T EvolveValue { get; private set; }

            //public PokemonEvolution<T> (T objects){}
            //void evolve(PokemonData.Pokemon EvolveTo, EvolutionMethod EvolveHow, T Value) { }

            public PokemonEvolution(Pokemons EvolveTo, EvolutionMethod EvolveHow, T Value) : base(EvolveTo: EvolveTo, EvolveHow: EvolveHow) {
                #region Switch
                //This should trigger after the class has been initialized, right?
                switch (EvolveHow)
                {
                    case EvolutionMethod.Level:
                    case EvolutionMethod.LevelFemale:
                    case EvolutionMethod.LevelMale:
                    case EvolutionMethod.Ninjask:
                    case EvolutionMethod.Beauty:
                    case EvolutionMethod.Happiness:
                    case EvolutionMethod.HappinessDay:
                    case EvolutionMethod.HappinessNight:
                    case EvolutionMethod.Hatred:
                        //if(typeof(T) || this.GetType() == typeof(string))
                        if (Value.GetType() != typeof(int))
                            //throw new Exception("Type not acceptable for Method-Value pair.");
                            //Instead of throwing an exception, i'll correct the problem instead?
                            Convert.ChangeType(Value, typeof(int));
                        Value = default(T);
                        break;
                    case EvolutionMethod.Item:
                    case EvolutionMethod.ItemFemale:
                    case EvolutionMethod.ItemMale:
                    case EvolutionMethod.TradeItem:
                    case EvolutionMethod.HoldItem:
                    case EvolutionMethod.HoldItemDay:
                    case EvolutionMethod.HoldItemNight:
                        if (this.EvolveValue.GetType() != typeof(Items))
                            Convert.ChangeType(Value, typeof(Items));
                        Value = default(T);
                        break;
                    case EvolutionMethod.TradeSpecies:
                    case EvolutionMethod.Party:
                    case EvolutionMethod.Shedinja:
                        if (this.EvolveValue.GetType() != typeof(Pokemons))
                            Convert.ChangeType(Value, typeof(Pokemons));
                        Value = default(T);
                        break;
                    case EvolutionMethod.Move:
                        if (this.EvolveValue.GetType() != typeof(Moves))
                            Convert.ChangeType(Value, typeof(Moves));
                        Value = default(T);
                        break;
                    case EvolutionMethod.Type:
                        if (this.EvolveValue.GetType() != typeof(Types))
                            Convert.ChangeType(Value, typeof(Types));
                        Value = default(T);
                        break;
                    case EvolutionMethod.Time:
                    case EvolutionMethod.Season:
                    case EvolutionMethod.Location:
                    case EvolutionMethod.Weather:
                    default:
                        //if there's no problem, just ignore it, and move on...
                        break;
                }
                #endregion
                this.EvolveValue = Value;
            }
            /*public PokemonEvolution(PokemonData.Pokemon EvolveTo, EvolutionMethod EvolveHow, T Value) : base(EvolveTo: EvolveTo, EvolveHow: EvolveHow) {
                #region Switch
                //This should trigger after the class has been initialized, right?
                switch (this.EvolveMethod)
                {
                    case EvolutionMethod.Level:
                    case EvolutionMethod.LevelFemale:
                    case EvolutionMethod.LevelMale:
                    case EvolutionMethod.Ninjask:
                    case EvolutionMethod.Beauty:
                    case EvolutionMethod.Happiness:
                    case EvolutionMethod.HappinessDay:
                    case EvolutionMethod.HappinessNight:
                    case EvolutionMethod.Hatred:
                        //if(typeof(T) || this.GetType() == typeof(string))
                        if (this.EvolveValue.GetType() != typeof(int))
                            //throw new Exception("Type not acceptable for Method-Value pair.");
                            //Instead of throwing an exception, i'll correct the problem instead?
                            Convert.ChangeType(this.EvolveValue, typeof(int));
                            this.EvolveValue = default(T);
                        break;
                    case EvolutionMethod.Item:
                    case EvolutionMethod.ItemFemale:
                    case EvolutionMethod.ItemMale:
                    case EvolutionMethod.TradeItem:
                    case EvolutionMethod.HoldItem:
                    case EvolutionMethod.HoldItemDay:
                    case EvolutionMethod.HoldItemNight:
                        if (this.EvolveValue.GetType() != typeof(eItems))
                            Convert.ChangeType(this.EvolveValue, typeof(eItems));
                            this.EvolveValue = default(T);
                        break;
                    case EvolutionMethod.TradeSpecies:
                    case EvolutionMethod.Party:
                    case EvolutionMethod.Shedinja:
                        if (this.EvolveValue.GetType() != typeof(PokemonData.Pokemon))
                            Convert.ChangeType(this.EvolveValue, typeof(PokemonData.Pokemon));
                            this.EvolveValue = default(T);
                        break;
                    case EvolutionMethod.Move:
                        if (this.EvolveValue.GetType() != typeof(Move.MoveData.Move))
                            Convert.ChangeType(this.EvolveValue, typeof(Move.MoveData.Move));
                            this.EvolveValue = default(T);
                        break;
                    case EvolutionMethod.Type:
                        if (this.EvolveValue.GetType() != typeof(PokemonData.Type))
                            Convert.ChangeType(this.EvolveValue, typeof(PokemonData.Type));
                            this.EvolveValue = default(T);
                        break;
                    case EvolutionMethod.Time:
                    case EvolutionMethod.Season:
                    case EvolutionMethod.Location:
                    case EvolutionMethod.Weather:
                    default:
                        //if there's no problem, just ignore it, and move on...
                        break;
                }
                #endregion
            }
            //private static Con<T2>(object ObjIn)
		    /*public int IntValue;
		    public string StringValue;
		    private int GetValue(int p)
		    {
			    return this.IntValue;
		    }
		    private string GetValue(string p)
		    {
			    return this.StringValue;
		    }*/

            public override bool isGenericT()
            {
                return true;// base.isGenericT();
            }
        }
        #endregion
    }
	/// <summary>
	/// </summary>
	/// Experience can be it's own class away from Pokemon. But there's no need for it to be global.
	/// ToDo: Consider making Experience class a Pokemon extension class...
	public class Experience
	{
		private int level { get { return GetLevelFromExperience(Growth, Current); } }
		public int Current { get; private set; }
		public int NextLevel { get { return GetExperience(Growth, level + 1); } }
		private LevelingRate Growth { get; set; }
		#region expTable
		private static int[] expTableErratic = new int[]{
			0,15,52,122,237,406,637,942,1326,1800,
			2369,3041,3822,4719,5737,6881,8155,9564,11111,12800,
			14632,16610,18737,21012,23437,26012,28737,31610,34632,37800,
			41111,44564,48155,51881,55737,59719,63822,68041,72369,76800,
			81326,85942,90637,95406,100237,105122,110052,115105,120001,125000,
			131324,137795,144410,151165,158056,165079,172229,179503,186894,194400,
			202013,209728,217540,225443,233431,241496,249633,257834,267406,276458,
			286328,296358,305767,316074,326531,336255,346965,357812,567807,378880,
			390077,400293,411686,423190,433572,445239,457001,467489,479378,491346,
			501878,513934,526049,536557,548720,560922,571333,583539,591882,600000};
		/// <summary>
		/// Medium (Medium Fast)
		/// </summary>
		private static int[] expTableFast = new int[]{
			0,6,21,51,100,172,274,409,583,800,
			1064,1382,1757,2195,2700,3276,3930,4665,5487,6400,
			7408,8518,9733,11059,12500,14060,15746,17561,19511,21600,
			23832,26214,28749,31443,34300,37324,40522,43897,47455,51200,
			55136,59270,63605,68147,72900,77868,83058,88473,94119,100000,
			106120,112486,119101,125971,133100,140492,148154,156089,164303,172800,
			181584,190662,200037,209715,219700,229996,240610,251545,262807,274400,
			286328,298598,311213,324179,337500,351180,365226,379641,394431,409600,
			425152,441094,457429,474163,491300,508844,526802,545177,563975,583200,
			602856,622950,643485,664467,685900,707788,730138,752953,776239,800000};

		private static int[] expTableMediumFast = new int[]{
			0,8,27,64,125,216,343,512,729,1000,
			1331,1728,2197,2744,3375,4096,4913,5832,6859,8000,
			9261,10648,12167,13824,15625,17576,19683,21952,24389,27000,
			29791,32768,35937,39304,42875,46656,50653,54872,59319,64000,
			68921,74088,79507,85184,91125,97336,103823,110592,117649,125000,
			132651,140608,148877,157464,166375,175616,185193,195112,205379,216000,
			226981,238328,250047,262144,274625,287496,300763,314432,328509,343000,
			357911,373248,389017,405224,421875,438976,456533,474552,493039,512000,
			531441,551368,571787,592704,614125,636056,658503,681472,704969,729000,
			753571,778688,804357,830584,857375,884736,912673,941192,970299,1000000};
		/// <summary>
		/// Parabolic (Medium Slow)
		/// </summary>
		private static int[] expTableMediumSlow = new int[]{
			0,9,57,96,135,179,236,314,419,560,
			742,973,1261,1612,2035,2535,3120,3798,4575,5460,
			6458,7577,8825,10208,11735,13411,15244,17242,19411,21760,
			24294,27021,29949,33084,36435,40007,43808,47846,52127,56660,
			61450,66505,71833,77440,83335,89523,96012,102810,109923,117360,
			125126,133229,141677,150476,159635,169159,179056,189334,199999,211060,
			222522,234393,246681,259392,272535,286115,300140,314618,329555,344960,
			360838,377197,394045,411388,429235,447591,466464,485862,505791,526260,
			547274,568841,590969,613664,636935,660787,685228,710266,735907,762160,
			789030,816525,844653,873420,902835,932903,963632,995030,1027103,1059860};

		private static int[] expTableSlow = new int[]{
			0,10,33,80,156,270,428,640,911,1250,
			1663,2160,2746,3430,4218,5120,6141,7290,8573,10000,
			11576,13310,15208,17280,19531,21970,24603,27440,30486,33750,
			37238,40960,44921,49130,53593,58320,63316,68590,74148,80000,
			86151,92610,99383,106480,113906,121670,129778,138240,147061,156250,
			165813,175760,186096,196830,207968,219520,231491,243890,256723,270000,
			283726,297910,312558,327680,343281,359370,375953,393040,410636,428750,
			447388,466560,486271,506530,527343,548720,570666,593190,616298,640000,
			664301,689210,714733,740880,767656,795070,823128,851840,881211,911250,
			941963,973360,1005446,1038230,1071718,1105920,1140841,1176490,1212873,1250000};

		private static int[] expTableFluctuating = new int[]{
			0,4,13,32,65,112,178,276,393,540,
			745,967,1230,1591,1957,2457,3046,3732,4526,5440,
			6482,7666,9003,10506,12187,14060,16140,18439,20974,23760,
			26811,30146,33780,37731,42017,46656,50653,55969,60505,66560,
			71677,78533,84277,91998,98415,107069,114205,123863,131766,142500,
			151222,163105,172697,185807,196322,210739,222231,238036,250562,267840,
			281456,300293,315059,335544,351520,373744,390991,415050,433631,459620,
			479600,507617,529063,559209,582187,614566,639146,673863,700115,737280,
			765275,804997,834809,877201,908905,954084,987754,1035837,1071552,1122660,
			1160499,1214753,1254796,1312322,1354652,1415577,1460276,1524731,1571884,1640000};
		#endregion

		private static int GetExperience(LevelingRate levelingRate, int currentLevel)
		{
			int exp = 0;
			//if (currentLevel > 100) currentLevel = 100; 
			//if (currentLevel > Settings.MAXIMUMLEVEL) currentLevel = Settings.MAXIMUMLEVEL; 
			if (levelingRate == LevelingRate.ERRATIC)
			{
				if (currentLevel > 100)
				{
					//exp = (int)System.Math.Floor((System.Math.Pow(currentLevel, 3)) * (160 - currentLevel) / 100);
					exp = (int)System.Math.Floor((System.Math.Pow(currentLevel, 3)) * (currentLevel * 6 / 10) / 100);
				}
				else exp = expTableErratic[currentLevel - 1]; //Because the array starts at 0, not 1.
			}
			else if (levelingRate == LevelingRate.FAST)
			{
				if (currentLevel > 100)
				{
					exp = (int)System.Math.Floor(System.Math.Pow(currentLevel, 3) * (4 / 5));
				}
				else exp = expTableFast[currentLevel - 1];
			}
			else if (levelingRate == LevelingRate.MEDIUMFAST)
			{
				if (currentLevel > 100)
				{
					exp = (int)System.Math.Floor(System.Math.Pow(currentLevel, 3));
				}
				else exp = expTableMediumFast[currentLevel - 1];
			}
			else if (levelingRate == LevelingRate.MEDIUMSLOW)
			{
				if (currentLevel > 100)
				{
					//Dont remember why currentlevel minus 1 is in formula...
					//I think it has to deal with formula table glitch for lvl 1-2 pokemons...
					//exp = (int)System.Math.Floor(((6 / 5) * System.Math.Pow(currentLevel - 1, 3)) - (15 * System.Math.Pow(currentLevel - 1, 3)) + (100 * (currentLevel - 1)) - 140);
					exp = (int)System.Math.Floor((6 * System.Math.Pow(currentLevel - 1, 3) / 5) - 15 * System.Math.Pow(currentLevel - 1, 2) + 100 * (currentLevel - 1) - 140);
				}
				else exp = expTableMediumSlow[currentLevel - 1];
			}
			else if (levelingRate == LevelingRate.SLOW)
			{
				if (currentLevel > 100)
				{
					exp = (int)System.Math.Floor(System.Math.Pow(currentLevel, 3) * (5 / 4));
				}
				else exp = expTableSlow[currentLevel - 1];
			}
			else if (levelingRate == LevelingRate.FLUCTUATING)
			{
				if (currentLevel > 100)
				{
					int rate = 82;
					//Slow rate with increasing level
					rate -= (currentLevel - 100) / 2;
					if (rate < 40) rate = 40;

					//exp = (int)System.Math.Floor(System.Math.Pow(currentLevel, 3) * ((System.Math.Floor(System.Math.Pow(currentLevel, 3) / 2) + 32) / 50));
					exp = (int)System.Math.Floor(System.Math.Pow(currentLevel, 3) * (((currentLevel * rate / 100) / 50)));
				}
				else exp = expTableFluctuating[currentLevel - 1];
			}

			return exp;


		}
		/// <summary>
		/// Gets the maximum Exp Points possible for the given growth rate.
		/// </summary>
		/// <param name="levelingRate"></param>
		/// <returns></returns>
		public static int GetMaxExperience(LevelingRate levelingRate)
		{
			return GetExperience(levelingRate, Settings.MAXIMUMLEVEL);
		}
		/// <summary>
		/// Gets the number of Exp Points needed to reach the given
		/// level with the given growth rate.
		/// </summary>
		/// <param name="levelingRate"></param>
		/// <param name="currentLevel"></param>
		/// <returns></returns>
		public static int GetStartExperience(LevelingRate levelingRate, int currentLevel)
		{
			if (currentLevel < 0) currentLevel = 1;
			if (currentLevel > Settings.MAXIMUMLEVEL) currentLevel = Settings.MAXIMUMLEVEL;
			return GetExperience(levelingRate, currentLevel);
		}
		/// <summary>
		/// Adds experience points ensuring that the new total doesn't
		/// exceed the maximum Exp. Points for the given growth rate.
		/// </summary>
		/// <param name="levelingRate">Growth rate.</param>
		/// <param name="currentExperience">Current Exp Points.</param>
		/// <param name="experienceGain">Exp. Points to add</param>
		/// <returns></returns>
		/// Subtract ceiling exp for left over experience points remaining after level up?...
		public int AddExperience(LevelingRate levelingRate, int currentExperience, int experienceGain)
		{
			int exp = currentExperience + experienceGain;
			int maxexp = GetExperience(levelingRate, Settings.MAXIMUMLEVEL);
			if (exp > maxexp) exp = maxexp;
			return exp;
		}
		/// <summary>
		/// Calculates a level given the number of Exp Points and growth rate.
		/// </summary>
		/// <param name="levelingRate">Growth rate.</param>
		/// <param name="experiencePoints">Current Experience Points</param>
		/// <returns></returns>
		public static int GetLevelFromExperience(LevelingRate levelingRate, int experiencePoints)
		{
			int maxexp = GetExperience(levelingRate, Settings.MAXIMUMLEVEL);
			if (experiencePoints > maxexp) experiencePoints = maxexp;
			for (int i = 0; i < Settings.MAXIMUMLEVEL; i++)
			{
				if (GetExperience(levelingRate, Settings.MAXIMUMLEVEL) == experiencePoints) return i;
				if (GetExperience(levelingRate, Settings.MAXIMUMLEVEL) > experiencePoints) return i-1;
			}
			return Settings.MAXIMUMLEVEL;
		}
		/*public static int GetLevelExperiencePoints(LevelingRate levelingRate, int currentLevel)
		{
			int exp = 0;
			if (currentLevel > 100)
			{
				currentLevel = 100;
			}
			if (levelingRate == LevelingRate.ERRATIC)
			{
				if (currentLevel > 100)
				{
					exp = (int)Mathf.Floor((Mathf.Pow(currentLevel, 3)) * (160 - currentLevel) / 100);
				}
				else exp = expTableErratic[currentLevel - 1]; //Because the array starts at 0, not 1.
			}
			else if (levelingRate == LevelingRate.FAST)
			{
				if (currentLevel > 100)
				{
					exp = (int)Mathf.Floor(Mathf.Pow(currentLevel, 3) * (4 / 5));
				}
				else exp = expTableFast[currentLevel - 1];
			}
			else if (levelingRate == LevelingRate.MEDIUMFAST)
			{
				if (currentLevel > 100)
				{
					exp = (int)Mathf.Floor(Mathf.Pow(currentLevel, 3));
				}
				else exp = expTableMediumFast[currentLevel - 1];
			}
			else if (levelingRate == LevelingRate.MEDIUMSLOW)
			{
				if (currentLevel > 100)
				{
					exp = (int)Mathf.Floor(((6 / 5) * Mathf.Pow(currentLevel - 1, 3)) - (15 * Mathf.Pow(currentLevel - 1, 3)) + (100 * (currentLevel - 1)) - 140);
				}
				else exp = expTableMediumSlow[currentLevel - 1];
			}
			else if (levelingRate == LevelingRate.SLOW)
			{
				if (currentLevel > 100)
				{
					exp = (int)Mathf.Floor(Mathf.Pow(currentLevel, 3) * (5 / 4));
				}
				else exp = expTableSlow[currentLevel - 1];
			}
			else if (levelingRate == LevelingRate.FLUCTUATING)
			{
				if (currentLevel > 100)
				{
					exp = (int)Mathf.Floor(Mathf.Pow(currentLevel, 3) * ((Mathf.Floor(Mathf.Pow(currentLevel, 3) / 2) + 32) / 50));
				}
				else exp = expTableFluctuating[currentLevel - 1];
			}

			return exp;
		}*/

		public Experience(LevelingRate rate)
		{
			Growth = rate;
		}
	}
	#endregion
}
public interface IPokemonEvolution
{
    Pokemons Species { get; }

    EvolutionMethod EvolveMethod { get; }
}
namespace PokemonUnity
{
	public enum Ribbon
	{
		NONE = 0,
		HOENNCOOL = 1,
		HOENNCOOLSUPER = 2,
		HOENNCOOLHYPER = 3,
		HOENNCOOLMASTER = 4,
		HOENNBEAUTY = 5,
		HOENNBEAUTYSUPER = 6,
		HOENNBEAUTYHYPER = 7,
		HOENNBEAUTYMASTER = 8,
		HOENNCUTE = 9,
		HOENNCUTESUPER = 10,
		HOENNCUTEHYPER = 11,
		HOENNCUTEMASTER = 12,
		HOENNSMART = 13,
		HOENNSMARTSUPER = 14,
		HOENNSMARTHYPER = 15,
		HOENNSMARTMASTER = 16,
		HOENNTOUGH = 17,
		HOENNTOUGHSUPER = 18,
		HOENNTOUGHHYPER = 19,
		HOENNTOUGHMASTER = 20,
		SINNOHCOOL = 21,
		SINNOHCOOLSUPER = 22,
		SINNOHCOOLHYPER = 23,
		SINNOHCOOLMASTER = 24,
		SINNOHBEAUTY = 25,
		SINNOHBEAUTYSUPER = 26,
		SINNOHBEAUTYHYPER = 27,
		SINNOHBEAUTYMASTER = 28,
		SINNOHCUTE = 29,
		SINNOHCUTESUPER = 30,
		SINNOHCUTEHYPER = 31,
		SINNOHCUTEMASTER = 32,
		SINNOHSMART = 33,
		SINNOHSMARTSUPER = 34,
		SINNOHSMARTHYPER = 35,
		SINNOHSMARTMASTER = 36,
		SINNOHTOUGH = 37,
		SINNOHTOUGHSUPER = 38,
		SINNOHTOUGHHYPER = 39,
		SINNOHTOUGHMASTER = 40,
		WINNING = 41,
		VICTORY = 42,
		ABILITY = 43,
		GREATABILITY = 44,
		DOUBLEABILITY = 45,
		MULTIABILITY = 46,
		PAIRABILITY = 47,
		WORLDABILITY = 48,
		CHAMPION = 49,
		SINNOHCHAMP = 50,
		RECORD = 51,
		EVENT = 52,
		LEGEND = 53,
		GORGEOUS = 54,
		ROYAL = 55,
		GORGEOUSROYAL = 56,
		ALERT = 57,
		SHOCK = 58,
		DOWNCAST = 59,
		CARELESS = 60,
		RELAX = 61,
		SNOOZE = 62,
		SMILE = 63,
		FOOTPRINT = 64,
		ARTIST = 65,
		EFFORT = 66,
		BIRTHDAY = 67,
		SPECIAL = 68,
		CLASSIC = 69,
		PREMIER = 70,
		SOUVENIR = 71,
		WISHING = 72,
		NATIONAL = 73,
		COUNTRY = 74,
		BATTLECHAMPION = 75,
		REGIONALCHAMPION = 76,
		EARTH = 77,
		WORLD = 78,
		NATIONALCHAMPION = 79,
		WORLDCHAMPION = 80
	}
	public enum Types
	{
		NONE = 0,
		NORMAL = 1,
		FIGHTING = 2,
		FLYING = 3,
		POISON = 4,
		GROUND = 5,
		ROCK = 6,
		BUG = 7,
		GHOST = 8,
		STEEL = 9,
		FIRE = 10,
		WATER = 11,
		GRASS = 12,
		ELECTRIC = 13,
		PSYCHIC = 14,
		ICE = 15,
		DRAGON = 16,
		DARK = 17,
		FAIRY = 18,
		UNKNOWN = 10001,
		SHADOW = 10002
	};
	public enum Color
	{
		RED = 8,
		BLUE = 2,
		YELLOW = 10,
		GREEN = 5,
		BLACK = 1,
		BROWN = 3,
		PURPLE = 7,
		GRAY = 4,
		WHITE = 9,
		PINK = 6,
		NONE = 0
	};
	public enum HappinessMethods
    {
        WALKING,
        LEVELUP,
        GROOM,
        FAINT,
        VITAMIN,
        EVBERRY,
        POWDER,
        ENERGYROOT,
        REVIVALHERB
    }
    /// <summary>
    /// Namespace to nest all Pokemon Enums
    /// </summary>
    namespace Pokemon
    {
		#region PokemonData Enumerators
		/// <summary>
		/// The likelihood of a Pokémon of the species being a certain gender.
		/// </summary>
		public enum GenderRatio
		{
			AlwaysMale,
			FemaleOneEighth,
			Female25Percent,
            //no inbetween?
			Female50Percent,
            //divided by 8 and missing values in between...
			Female75Percent,
			FemaleSevenEighths,
			AlwaysFemale,
			Genderless
		}
		public enum LevelingRate
		{
			ERRATIC = 6, //fast then very slow?
			FAST = 3,
			MEDIUMFAST = 2, //Medium?
			MEDIUMSLOW = 4,
			SLOW = 1,
			FLUCTUATING = 5 //slow then fast?
		};
		/// <summary>
		/// Pokemon ids are connected to XML file.
		/// </summary>
		/// <remarks>Can now code with strings or int and
		/// access the same value.</remarks>
		public enum Pokemons
		{
			NONE = 0,
            BULBASAUR = 1,
            IVYSAUR = 2,
            VENUSAUR = 3,
            CHARMANDER = 4,
            CHARMELEON = 5,
            CHARIZARD = 6,
            SQUIRTLE = 7,
            WARTORTLE = 8,
            BLASTOISE = 9,
            CATERPIE = 10,
            METAPOD = 11,
            BUTTERFREE = 12,
            WEEDLE = 13,
            KAKUNA = 14,
            BEEDRILL = 15,
            PIDGEY = 16,
            PIDGEOTTO = 17,
            PIDGEOT = 18,
            RATTATA = 19,
            RATICATE = 20,
            SPEAROW = 21,
            FEAROW = 22,
            EKANS = 23,
            ARBOK = 24,
            PIKACHU = 25,
            RAICHU = 26,
            SANDSHREW = 27,
            SANDSLASH = 28,
            /// <SUMMARY>
            /// NIDORANFEMALE
            /// </SUMMARY>
            NIDORAN_F = 29,
            NIDORINA = 30,
            NIDOQUEEN = 31,
            /// <SUMMARY>
            /// NIDORANMALE
            /// </SUMMARY>
            NIDORAN_M = 32,
            NIDORINO = 33,
            NIDOKING = 34,
            CLEFAIRY = 35,
            CLEFABLE = 36,
            VULPIX = 37,
            NINETALES = 38,
            JIGGLYPUFF = 39,
            WIGGLYTUFF = 40,
            ZUBAT = 41,
            GOLBAT = 42,
            ODDISH = 43,
            GLOOM = 44,
            VILEPLUME = 45,
            PARAS = 46,
            PARASECT = 47,
            VENONAT = 48,
            VENOMOTH = 49,
            DIGLETT = 50,
            DUGTRIO = 51,
            MEOWTH = 52,
            PERSIAN = 53,
            PSYDUCK = 54,
            GOLDUCK = 55,
            MANKEY = 56,
            PRIMEAPE = 57,
            GROWLITHE = 58,
            ARCANINE = 59,
            POLIWAG = 60,
            POLIWHIRL = 61,
            POLIWRATH = 62,
            ABRA = 63,
            KADABRA = 64,
            ALAKAZAM = 65,
            MACHOP = 66,
            MACHOKE = 67,
            MACHAMP = 68,
            BELLSPROUT = 69,
            WEEPINBELL = 70,
            VICTREEBEL = 71,
            TENTACOOL = 72,
            TENTACRUEL = 73,
            GEODUDE = 74,
            GRAVELER = 75,
            GOLEM = 76,
            PONYTA = 77,
            RAPIDASH = 78,
            SLOWPOKE = 79,
            SLOWBRO = 80,
            MAGNEMITE = 81,
            MAGNETON = 82,
            FARFETCHD = 83,
            DODUO = 84,
            DODRIO = 85,
            SEEL = 86,
            DEWGONG = 87,
            GRIMER = 88,
            MUK = 89,
            SHELLDER = 90,
            CLOYSTER = 91,
            GASTLY = 92,
            HAUNTER = 93,
            GENGAR = 94,
            ONIX = 95,
            DROWZEE = 96,
            HYPNO = 97,
            KRABBY = 98,
            KINGLER = 99,
            VOLTORB = 100,
            ELECTRODE = 101,
            EXEGGCUTE = 102,
            EXEGGUTOR = 103,
            CUBONE = 104,
            MAROWAK = 105,
            HITMONLEE = 106,
            HITMONCHAN = 107,
            LICKITUNG = 108,
            KOFFING = 109,
            WEEZING = 110,
            RHYHORN = 111,
            RHYDON = 112,
            CHANSEY = 113,
            TANGELA = 114,
            KANGASKHAN = 115,
            HORSEA = 116,
            SEADRA = 117,
            GOLDEEN = 118,
            SEAKING = 119,
            STARYU = 120,
            STARMIE = 121,
            MR_MIME = 122,
            SCYTHER = 123,
            JYNX = 124,
            ELECTABUZZ = 125,
            MAGMAR = 126,
            PINSIR = 127,
            TAUROS = 128,
            MAGIKARP = 129,
            GYARADOS = 130,
            LAPRAS = 131,
            DITTO = 132,
            EEVEE = 133,
            VAPOREON = 134,
            JOLTEON = 135,
            FLAREON = 136,
            PORYGON = 137,
            OMANYTE = 138,
            OMASTAR = 139,
            KABUTO = 140,
            KABUTOPS = 141,
            AERODACTYL = 142,
            SNORLAX = 143,
            ARTICUNO = 144,
            ZAPDOS = 145,
            MOLTRES = 146,
            DRATINI = 147,
            DRAGONAIR = 148,
            DRAGONITE = 149,
            MEWTWO = 150,
            MEW = 151,
            CHIKORITA = 152,
            BAYLEEF = 153,
            MEGANIUM = 154,
            CYNDAQUIL = 155,
            QUILAVA = 156,
            TYPHLOSION = 157,
            TOTODILE = 158,
            CROCONAW = 159,
            FERALIGATR = 160,
            SENTRET = 161,
            FURRET = 162,
            HOOTHOOT = 163,
            NOCTOWL = 164,
            LEDYBA = 165,
            LEDIAN = 166,
            SPINARAK = 167,
            ARIADOS = 168,
            CROBAT = 169,
            CHINCHOU = 170,
            LANTURN = 171,
            PICHU = 172,
            CLEFFA = 173,
            IGGLYBUFF = 174,
            TOGEPI = 175,
            TOGETIC = 176,
            NATU = 177,
            XATU = 178,
            MAREEP = 179,
            FLAAFFY = 180,
            AMPHAROS = 181,
            BELLOSSOM = 182,
            MARILL = 183,
            AZUMARILL = 184,
            SUDOWOODO = 185,
            POLITOED = 186,
            HOPPIP = 187,
            SKIPLOOM = 188,
            JUMPLUFF = 189,
            AIPOM = 190,
            SUNKERN = 191,
            SUNFLORA = 192,
            YANMA = 193,
            WOOPER = 194,
            QUAGSIRE = 195,
            ESPEON = 196,
            UMBREON = 197,
            MURKROW = 198,
            SLOWKING = 199,
            MISDREAVUS = 200,
            UNOWN = 201,
            WOBBUFFET = 202,
            GIRAFARIG = 203,
            PINECO = 204,
            FORRETRESS = 205,
            DUNSPARCE = 206,
            GLIGAR = 207,
            STEELIX = 208,
            SNUBBULL = 209,
            GRANBULL = 210,
            QWILFISH = 211,
            SCIZOR = 212,
            SHUCKLE = 213,
            HERACROSS = 214,
            SNEASEL = 215,
            TEDDIURSA = 216,
            URSARING = 217,
            SLUGMA = 218,
            MAGCARGO = 219,
            SWINUB = 220,
            PILOSWINE = 221,
            CORSOLA = 222,
            REMORAID = 223,
            OCTILLERY = 224,
            DELIBIRD = 225,
            MANTINE = 226,
            SKARMORY = 227,
            HOUNDOUR = 228,
            HOUNDOOM = 229,
            KINGDRA = 230,
            PHANPY = 231,
            DONPHAN = 232,
            PORYGON2 = 233,
            STANTLER = 234,
            SMEARGLE = 235,
            TYROGUE = 236,
            HITMONTOP = 237,
            SMOOCHUM = 238,
            ELEKID = 239,
            MAGBY = 240,
            MILTANK = 241,
            BLISSEY = 242,
            RAIKOU = 243,
            ENTEI = 244,
            SUICUNE = 245,
            LARVITAR = 246,
            PUPITAR = 247,
            TYRANITAR = 248,
            LUGIA = 249,
            HO_OH = 250,
            CELEBI = 251,
            TREECKO = 252,
            GROVYLE = 253,
            SCEPTILE = 254,
            TORCHIC = 255,
            COMBUSKEN = 256,
            BLAZIKEN = 257,
            MUDKIP = 258,
            MARSHTOMP = 259,
            SWAMPERT = 260,
            POOCHYENA = 261,
            MIGHTYENA = 262,
            ZIGZAGOON = 263,
            LINOONE = 264,
            WURMPLE = 265,
            SILCOON = 266,
            BEAUTIFLY = 267,
            CASCOON = 268,
            DUSTOX = 269,
            LOTAD = 270,
            LOMBRE = 271,
            LUDICOLO = 272,
            SEEDOT = 273,
            NUZLEAF = 274,
            SHIFTRY = 275,
            TAILLOW = 276,
            SWELLOW = 277,
            WINGULL = 278,
            PELIPPER = 279,
            RALTS = 280,
            KIRLIA = 281,
            GARDEVOIR = 282,
            SURSKIT = 283,
            MASQUERAIN = 284,
            SHROOMISH = 285,
            BRELOOM = 286,
            SLAKOTH = 287,
            VIGOROTH = 288,
            SLAKING = 289,
            NINCADA = 290,
            NINJASK = 291,
            SHEDINJA = 292,
            WHISMUR = 293,
            LOUDRED = 294,
            EXPLOUD = 295,
            MAKUHITA = 296,
            HARIYAMA = 297,
            AZURILL = 298,
            NOSEPASS = 299,
            SKITTY = 300,
            DELCATTY = 301,
            SABLEYE = 302,
            MAWILE = 303,
            ARON = 304,
            LAIRON = 305,
            AGGRON = 306,
            MEDITITE = 307,
            MEDICHAM = 308,
            ELECTRIKE = 309,
            MANECTRIC = 310,
            PLUSLE = 311,
            MINUN = 312,
            VOLBEAT = 313,
            ILLUMISE = 314,
            ROSELIA = 315,
            GULPIN = 316,
            SWALOT = 317,
            CARVANHA = 318,
            SHARPEDO = 319,
            WAILMER = 320,
            WAILORD = 321,
            NUMEL = 322,
            CAMERUPT = 323,
            TORKOAL = 324,
            SPOINK = 325,
            GRUMPIG = 326,
            SPINDA = 327,
            TRAPINCH = 328,
            VIBRAVA = 329,
            FLYGON = 330,
            CACNEA = 331,
            CACTURNE = 332,
            SWABLU = 333,
            ALTARIA = 334,
            ZANGOOSE = 335,
            SEVIPER = 336,
            LUNATONE = 337,
            SOLROCK = 338,
            BARBOACH = 339,
            WHISCASH = 340,
            CORPHISH = 341,
            CRAWDAUNT = 342,
            BALTOY = 343,
            CLAYDOL = 344,
            LILEEP = 345,
            CRADILY = 346,
            ANORITH = 347,
            ARMALDO = 348,
            FEEBAS = 349,
            MILOTIC = 350,
            CASTFORM = 351,
            KECLEON = 352,
            SHUPPET = 353,
            BANETTE = 354,
            DUSKULL = 355,
            DUSCLOPS = 356,
            TROPIUS = 357,
            CHIMECHO = 358,
            ABSOL = 359,
            WYNAUT = 360,
            SNORUNT = 361,
            GLALIE = 362,
            SPHEAL = 363,
            SEALEO = 364,
            WALREIN = 365,
            CLAMPERL = 366,
            HUNTAIL = 367,
            GOREBYSS = 368,
            RELICANTH = 369,
            LUVDISC = 370,
            BAGON = 371,
            SHELGON = 372,
            SALAMENCE = 373,
            BELDUM = 374,
            METANG = 375,
            METAGROSS = 376,
            REGIROCK = 377,
            REGICE = 378,
            REGISTEEL = 379,
            LATIAS = 380,
            LATIOS = 381,
            KYOGRE = 382,
            GROUDON = 383,
            RAYQUAZA = 384,
            JIRACHI = 385,
            /// <summary>
            /// "DEOXYS_NORMAL"
            /// </summary>
            DEOXYS = 386,
            TURTWIG = 387,
            GROTLE = 388,
            TORTERRA = 389,
            CHIMCHAR = 390,
            MONFERNO = 391,
            INFERNAPE = 392,
            PIPLUP = 393,
            PRINPLUP = 394,
            EMPOLEON = 395,
            STARLY = 396,
            STARAVIA = 397,
            STARAPTOR = 398,
            BIDOOF = 399,
            BIBAREL = 400,
            KRICKETOT = 401,
            KRICKETUNE = 402,
            SHINX = 403,
            LUXIO = 404,
            LUXRAY = 405,
            BUDEW = 406,
            ROSERADE = 407,
            CRANIDOS = 408,
            RAMPARDOS = 409,
            SHIELDON = 410,
            BASTIODON = 411,
            BURMY = 412,
            /// <summary>
            /// "WORMADAM_PLANT"?
            /// </summary>
            WORMADAM = 413,
            MOTHIM = 414,
            COMBEE = 415,
            VESPIQUEN = 416,
            PACHIRISU = 417,
            BUIZEL = 418,
            FLOATZEL = 419,
            CHERUBI = 420,
            CHERRIM = 421,
            SHELLOS = 422,
            GASTRODON = 423,
            AMBIPOM = 424,
            DRIFLOON = 425,
            DRIFBLIM = 426,
            BUNEARY = 427,
            LOPUNNY = 428,
            MISMAGIUS = 429,
            HONCHKROW = 430,
            GLAMEOW = 431,
            PURUGLY = 432,
            CHINGLING = 433,
            STUNKY = 434,
            SKUNTANK = 435,
            BRONZOR = 436,
            BRONZONG = 437,
            BONSLY = 438,
            MIME_JR = 439,
            HAPPINY = 440,
            CHATOT = 441,
            SPIRITOMB = 442,
            GIBLE = 443,
            GABITE = 444,
            GARCHOMP = 445,
            MUNCHLAX = 446,
            RIOLU = 447,
            LUCARIO = 448,
            HIPPOPOTAS = 449,
            HIPPOWDON = 450,
            SKORUPI = 451,
            DRAPION = 452,
            CROAGUNK = 453,
            TOXICROAK = 454,
            CARNIVINE = 455,
            FINNEON = 456,
            LUMINEON = 457,
            MANTYKE = 458,
            SNOVER = 459,
            ABOMASNOW = 460,
            WEAVILE = 461,
            MAGNEZONE = 462,
            LICKILICKY = 463,
            RHYPERIOR = 464,
            TANGROWTH = 465,
            ELECTIVIRE = 466,
            MAGMORTAR = 467,
            TOGEKISS = 468,
            YANMEGA = 469,
            LEAFEON = 470,
            GLACEON = 471,
            GLISCOR = 472,
            MAMOSWINE = 473,
            PORYGON_Z = 474,
            GALLADE = 475,
            PROBOPASS = 476,
            DUSKNOIR = 477,
            FROSLASS = 478,
            ROTOM = 479,
            UXIE = 480,
            MESPRIT = 481,
            AZELF = 482,
            DIALGA = 483,
            PALKIA = 484,
            HEATRAN = 485,
            REGIGIGAS = 486,
            /// <summary>
            /// "GIRATINA_ALTERED"?
            /// </summary>
            GIRATINA = 487,
            CRESSELIA = 488,
            PHIONE = 489,
            MANAPHY = 490,
            DARKRAI = 491,
            /// <summary>
            /// "SHAYMIN_LAND"?
            /// </summary>
            SHAYMIN = 492,
            ARCEUS = 493,
            VICTINI = 494,
            SNIVY = 495,
            SERVINE = 496,
            SERPERIOR = 497,
            TEPIG = 498,
            PIGNITE = 499,
            EMBOAR = 500,
            OSHAWOTT = 501,
            DEWOTT = 502,
            SAMUROTT = 503,
            PATRAT = 504,
            WATCHOG = 505,
            LILLIPUP = 506,
            HERDIER = 507,
            STOUTLAND = 508,
            PURRLOIN = 509,
            LIEPARD = 510,
            PANSAGE = 511,
            SIMISAGE = 512,
            PANSEAR = 513,
            SIMISEAR = 514,
            PANPOUR = 515,
            SIMIPOUR = 516,
            MUNNA = 517,
            MUSHARNA = 518,
            PIDOVE = 519,
            TRANQUILL = 520,
            UNFEZANT = 521,
            BLITZLE = 522,
            ZEBSTRIKA = 523,
            ROGGENROLA = 524,
            BOLDORE = 525,
            GIGALITH = 526,
            WOOBAT = 527,
            SWOOBAT = 528,
            DRILBUR = 529,
            EXCADRILL = 530,
            AUDINO = 531,
            TIMBURR = 532,
            GURDURR = 533,
            CONKELDURR = 534,
            TYMPOLE = 535,
            PALPITOAD = 536,
            SEISMITOAD = 537,
            THROH = 538,
            SAWK = 539,
            SEWADDLE = 540,
            SWADLOON = 541,
            LEAVANNY = 542,
            VENIPEDE = 543,
            WHIRLIPEDE = 544,
            SCOLIPEDE = 545,
            COTTONEE = 546,
            WHIMSICOTT = 547,
            PETILIL = 548,
            LILLIGANT = 549,
            /// <summary>
            /// "BASCULIN_RED_STRIPED"
            /// </summary>
            BASCULIN = 550,
            SANDILE = 551,
            KROKOROK = 552,
            KROOKODILE = 553,
            DARUMAKA = 554,
            /// <summary>
            /// DARMANITAN_STANDARD
            /// </summary>
            DARMANITAN = 555,
            MARACTUS = 556,
            DWEBBLE = 557,
            CRUSTLE = 558,
            SCRAGGY = 559,
            SCRAFTY = 560,
            SIGILYPH = 561,
            YAMASK = 562,
            COFAGRIGUS = 563,
            TIRTOUGA = 564,
            CARRACOSTA = 565,
            ARCHEN = 566,
            ARCHEOPS = 567,
            TRUBBISH = 568,
            GARBODOR = 569,
            ZORUA = 570,
            ZOROARK = 571,
            MINCCINO = 572,
            CINCCINO = 573,
            GOTHITA = 574,
            GOTHORITA = 575,
            GOTHITELLE = 576,
            SOLOSIS = 577,
            DUOSION = 578,
            REUNICLUS = 579,
            DUCKLETT = 580,
            SWANNA = 581,
            VANILLITE = 582,
            VANILLISH = 583,
            VANILLUXE = 584,
            DEERLING = 585,
            SAWSBUCK = 586,
            EMOLGA = 587,
            KARRABLAST = 588,
            ESCAVALIER = 589,
            FOONGUS = 590,
            AMOONGUSS = 591,
            FRILLISH = 592,
            JELLICENT = 593,
            ALOMOMOLA = 594,
            JOLTIK = 595,
            GALVANTULA = 596,
            FERROSEED = 597,
            FERROTHORN = 598,
            KLINK = 599,
            KLANG = 600,
            KLINKLANG = 601,
            TYNAMO = 602,
            EELEKTRIK = 603,
            EELEKTROSS = 604,
            ELGYEM = 605,
            BEHEEYEM = 606,
            LITWICK = 607,
            LAMPENT = 608,
            CHANDELURE = 609,
            AXEW = 610,
            FRAXURE = 611,
            HAXORUS = 612,
            CUBCHOO = 613,
            BEARTIC = 614,
            CRYOGONAL = 615,
            SHELMET = 616,
            ACCELGOR = 617,
            STUNFISK = 618,
            MIENFOO = 619,
            MIENSHAO = 620,
            DRUDDIGON = 621,
            GOLETT = 622,
            GOLURK = 623,
            PAWNIARD = 624,
            BISHARP = 625,
            BOUFFALANT = 626,
            RUFFLET = 627,
            BRAVIARY = 628,
            VULLABY = 629,
            MANDIBUZZ = 630,
            HEATMOR = 631,
            DURANT = 632,
            DEINO = 633,
            ZWEILOUS = 634,
            HYDREIGON = 635,
            LARVESTA = 636,
            VOLCARONA = 637,
            COBALION = 638,
            TERRAKION = 639,
            VIRIZION = 640,
            /// <summary>
            /// "INCARNATE"?
            /// </summary>
            TORNADUS = 641,
            /// <summary>
            /// "INCARNATE"?
            /// </summary>
            THUNDURUS = 642,
            RESHIRAM = 643,
            ZEKROM = 644,
            /// <summary>
            /// "INCARNATE"?
            /// </summary>
            LANDORUS = 645,
            KYUREM = 646,
            /// <summary>
            /// KELDEO_ORDINARY
            /// </summary>
            KELDEO = 647,
            /// <summary>
            /// "MELOETTA_ARIA"
            /// </summary>
            MELOETTA = 648,
            GENESECT = 649,
            CHESPIN = 650,
            QUILLADIN = 651,
            CHESNAUGHT = 652,
            FENNEKIN = 653,
            BRAIXEN = 654,
            DELPHOX = 655,
            FROAKIE = 656,
            FROGADIER = 657,
            GRENINJA = 658,
            BUNNELBY = 659,
            DIGGERSBY = 660,
            FLETCHLING = 661,
            FLETCHINDER = 662,
            TALONFLAME = 663,
            SCATTERBUG = 664,
            SPEWPA = 665,
            VIVILLON = 666,
            LITLEO = 667,
            PYROAR = 668,
            FLABEBE = 669,
            FLOETTE = 670,
            FLORGES = 671,
            SKIDDO = 672,
            GOGOAT = 673,
            PANCHAM = 674,
            PANGORO = 675,
            FURFROU = 676,
            ESPURR = 677,
            /// <summary>
            /// "MEOWSTIC_MALE"
            /// </summary>
            MEOWSTIC = 678,
            HONEDGE = 679,
            DOUBLADE = 680,
            /// <summary>
            /// AEGISLASH_SHIELD
            /// </summary>
            AEGISLASH = 681,
            SPRITZEE = 682,
            AROMATISSE = 683,
            SWIRLIX = 684,
            SLURPUFF = 685,
            INKAY = 686,
            MALAMAR = 687,
            BINACLE = 688,
            BARBARACLE = 689,
            SKRELP = 690,
            DRAGALGE = 691,
            CLAUNCHER = 692,
            CLAWITZER = 693,
            HELIOPTILE = 694,
            HELIOLISK = 695,
            TYRUNT = 696,
            TYRANTRUM = 697,
            AMAURA = 698,
            AURORUS = 699,
            SYLVEON = 700,
            HAWLUCHA = 701,
            DEDENNE = 702,
            CARBINK = 703,
            GOOMY = 704,
            SLIGGOO = 705,
            GOODRA = 706,
            KLEFKI = 707,
            PHANTUMP = 708,
            TREVENANT = 709,
            /// <summary>
            /// "PUMPKABOO_AVERAGE"
            /// </summary>
            PUMPKABOO = 710,
            /// <summary>
            /// "GOURGEIST_AVERAGE"
            /// </summary>
            GOURGEIST = 711,
            BERGMITE = 712,
            AVALUGG = 713,
            NOIBAT = 714,
            NOIVERN = 715,
            XERNEAS = 716,
            YVELTAL = 717,
            ZYGARDE = 718,
            DIANCIE = 719,
            HOOPA = 720,
            VOLCANION = 721,
            //Everything beyond here is new
            ROWLET = 722,
            DARTRIX = 723,
            DECIDUEYE = 724,
            LITTEN = 725,
            TORRACAT = 726,
            INCINEROAR = 727,
            POPPLIO = 728,
            BRIONNE = 729,
            PRIMARINA = 730,
            PIKIPEK = 731,
            TRUMBEAK = 732,
            TOUCANNON = 733,
            YUNGOOS = 734,
            GUMSHOOS = 735,
            GRUBBIN = 736,
            CHARJABUG = 737,
            VIKAVOLT = 738,
            CRABRAWLER = 739,
            CRABOMINABLE = 740,
            ORICORIO_BAILE = 741,
            CUTIEFLY = 742,
            RIBOMBEE = 743,
            ROCKRUFF = 744,
            LYCANROC_MIDDAY = 745,
            WISHIWASHI_SOLO = 746,
            MAREANIE = 747,
            TOXAPEX = 748,
            MUDBRAY = 749,
            MUDSDALE = 750,
            DEWPIDER = 751,
            ARAQUANID = 752,
            FOMANTIS = 753,
            LURANTIS = 754,
            MORELULL = 755,
            SHIINOTIC = 756,
            SALANDIT = 757,
            SALAZZLE = 758,
            STUFFUL = 759,
            BEWEAR = 760,
            BOUNSWEET = 761,
            STEENEE = 762,
            TSAREENA = 763,
            COMFEY = 764,
            ORANGURU = 765,
            PASSIMIAN = 766,
            WIMPOD = 767,
            GOLISOPOD = 768,
            SANDYGAST = 769,
            PALOSSAND = 770,
            PYUKUMUKU = 771,
            TYPE_NULL = 772,
            SILVALLY = 773,
            MINIOR_RED_METEOR = 774,
            KOMALA = 775,
            TURTONATOR = 776,
            TOGEDEMARU = 777,
            MIMIKYU_DISGUISED = 778,
            BRUXISH = 779,
            DRAMPA = 780,
            DHELMISE = 781,
            JANGMO_O = 782,
            HAKAMO_O = 783,
            KOMMO_O = 784,
            TAPU_KOKO = 785,
            TAPU_LELE = 786,
            TAPU_BULU = 787,
            TAPU_FINI = 788,
            COSMOG = 789,
            COSMOEM = 790,
            SOLGALEO = 791,
            LUNALA = 792,
            NIHILEGO = 793,
            BUZZWOLE = 794,
            PHEROMOSA = 795,
            XURKITREE = 796,
            CELESTEELA = 797,
            KARTANA = 798,
            GUZZLORD = 799,
            NECROZMA = 800,
            MAGEARNA = 801,
            MARSHADOW = 802,
            POIPOLE = 803,
            NAGANADEL = 804,
            STAKATAKA = 805,
            BLACEPHALON = 806,
            ZERAORA = 807,
            DEOXYS_ATTACK = 10001,
            DEOXYS_DEFENSE = 10002,
            DEOXYS_SPEED = 10003,
            WORMADAM_SANDY = 10004,
            WORMADAM_TRASH = 10005,
            SHAYMIN_SKY = 10006,
            GIRATINA_ORIGIN = 10007,
            ROTOM_HEAT = 10008,
            ROTOM_WASH = 10009,
            ROTOM_FROST = 10010,
            ROTOM_FAN = 10011,
            ROTOM_MOW = 10012,
            CASTFORM_SUNNY = 10013,
            CASTFORM_RAINY = 10014,
            CASTFORM_SNOWY = 10015,
            BASCULIN_BLUE_STRIPED = 10016,
            DARMANITAN_ZEN = 10017,
            MELOETTA_PIROUETTE = 10018,
            TORNADUS_THERIAN = 10019,
            THUNDURUS_THERIAN = 10020,
            LANDORUS_THERIAN = 10021,
            KYUREM_BLACK = 10022,
            KYUREM_WHITE = 10023,
            KELDEO_RESOLUTE = 10024,
            MEOWSTIC_FEMALE = 10025,
            AEGISLASH_BLADE = 10026,
            PUMPKABOO_SMALL = 10027,
            PUMPKABOO_LARGE = 10028,
            PUMPKABOO_SUPER = 10029,
            GOURGEIST_SMALL = 10030,
            GOURGEIST_LARGE = 10031,
            GOURGEIST_SUPER = 10032,
            VENUSAUR_MEGA = 10033,
            CHARIZARD_MEGA_X = 10034,
            CHARIZARD_MEGA_Y = 10035,
            BLASTOISE_MEGA = 10036,
            ALAKAZAM_MEGA = 10037,
            GENGAR_MEGA = 10038,
            KANGASKHAN_MEGA = 10039,
            PINSIR_MEGA = 10040,
            GYARADOS_MEGA = 10041,
            AERODACTYL_MEGA = 10042,
            MEWTWO_MEGA_X = 10043,
            MEWTWO_MEGA_Y = 10044,
            AMPHAROS_MEGA = 10045,
            SCIZOR_MEGA = 10046,
            HERACROSS_MEGA = 10047,
            HOUNDOOM_MEGA = 10048,
            TYRANITAR_MEGA = 10049,
            BLAZIKEN_MEGA = 10050,
            GARDEVOIR_MEGA = 10051,
            MAWILE_MEGA = 10052,
            AGGRON_MEGA = 10053,
            MEDICHAM_MEGA = 10054,
            MANECTRIC_MEGA = 10055,
            BANETTE_MEGA = 10056,
            ABSOL_MEGA = 10057,
            GARCHOMP_MEGA = 10058,
            LUCARIO_MEGA = 10059,
            ABOMASNOW_MEGA = 10060,
            FLOETTE_ETERNAL = 10061,
            LATIAS_MEGA = 10062,
            LATIOS_MEGA = 10063,
            SWAMPERT_MEGA = 10064,
            SCEPTILE_MEGA = 10065,
            SABLEYE_MEGA = 10066,
            ALTARIA_MEGA = 10067,
            GALLADE_MEGA = 10068,
            AUDINO_MEGA = 10069,
            SHARPEDO_MEGA = 10070,
            SLOWBRO_MEGA = 10071,
            STEELIX_MEGA = 10072,
            PIDGEOT_MEGA = 10073,
            GLALIE_MEGA = 10074,
            DIANCIE_MEGA = 10075,
            METAGROSS_MEGA = 10076,
            KYOGRE_PRIMAL = 10077,
            GROUDON_PRIMAL = 10078,
            RAYQUAZA_MEGA = 10079,
            PIKACHU_ROCK_STAR = 10080,
            PIKACHU_BELLE = 10081,
            PIKACHU_POP_STAR = 10082,
            PIKACHU_PHD = 10083,
            PIKACHU_LIBRE = 10084,
            PIKACHU_COSPLAY = 10085,
            HOOPA_UNBOUND = 10086,
            CAMERUPT_MEGA = 10087,
            LOPUNNY_MEGA = 10088,
            SALAMENCE_MEGA = 10089,
            BEEDRILL_MEGA = 10090,
            RATTATA_ALOLA = 10091,
            RATICATE_ALOLA = 10092,
            RATICATE_TOTEM_ALOLA = 10093,
            PIKACHU_ORIGINAL_CAP = 10094,
            PIKACHU_HOENN_CAP = 10095,
            PIKACHU_SINNOH_CAP = 10096,
            PIKACHU_UNOVA_CAP = 10097,
            PIKACHU_KALOS_CAP = 10098,
            PIKACHU_ALOLA_CAP = 10099,
            RAICHU_ALOLA = 10100,
            SANDSHREW_ALOLA = 10101,
            SANDSLASH_ALOLA = 10102,
            VULPIX_ALOLA = 10103,
            NINETALES_ALOLA = 10104,
            DIGLETT_ALOLA = 10105,
            DUGTRIO_ALOLA = 10106,
            MEOWTH_ALOLA = 10107,
            PERSIAN_ALOLA = 10108,
            GEODUDE_ALOLA = 10109,
            GRAVELER_ALOLA = 10110,
            GOLEM_ALOLA = 10111,
            GRIMER_ALOLA = 10112,
            MUK_ALOLA = 10113,
            EXEGGUTOR_ALOLA = 10114,
            MAROWAK_ALOLA = 10115,
            GRENINJA_BATTLE_BOND = 10116,
            GRENINJA_ASH = 10117,
            ZYGARDE_10 = 10118,
            ZYGARDE_50 = 10119,
            ZYGARDE_COMPLETE = 10120,
            GUMSHOOS_TOTEM = 10121,
            VIKAVOLT_TOTEM = 10122,
            ORICORIO_POM_POM = 10123,
            ORICORIO_PAU = 10124,
            ORICORIO_SENSU = 10125,
            LYCANROC_MIDNIGHT = 10126,
            WISHIWASHI_SCHOOL = 10127,
            LURANTIS_TOTEM = 10128,
            SALAZZLE_TOTEM = 10129,
            MINIOR_ORANGE_METEOR = 10130,
            MINIOR_YELLOW_METEOR = 10131,
            MINIOR_GREEN_METEOR = 10132,
            MINIOR_BLUE_METEOR = 10133,
            MINIOR_INDIGO_METEOR = 10134,
            MINIOR_VIOLET_METEOR = 10135,
            MINIOR_RED = 10136,
            MINIOR_ORANGE = 10137,
            MINIOR_YELLOW = 10138,
            MINIOR_GREEN = 10139,
            MINIOR_BLUE = 10140,
            MINIOR_INDIGO = 10141,
            MINIOR_VIOLET = 10142,
            MIMIKYU_BUSTED = 10143,
            MIMIKYU_TOTEM_DISGUISED = 10144,
            MIMIKYU_TOTEM_BUSTED = 10145,
            KOMMO_O_TOTEM = 10146,
            MAGEARNA_ORIGINAL = 10147,
            PIKACHU_PARTNER_CAP = 10148,
            MAROWAK_TOTEM = 10149,
            RIBOMBEE_TOTEM = 10150,
            ROCKRUFF_OWN_TEMPO = 10151,
            LYCANROC_DUSK = 10152,
            ARAQUANID_TOTEM = 10153,
            TOGEDEMARU_TOTEM = 10154,
            NECROZMA_DUSK = 10155,
            NECROZMA_DAWN = 10156,
            NECROZMA_ULTRA = 10157
        }
        public enum EggGroups
		{
			NONE = 0,
			MONSTER = 1,
			WATER1 = 2,
			BUG = 3,
			FLYING = 4,
			FIELD = 5, //"Ground"?
			FAIRY = 6,
			GRASS = 7, //"Plant"
			HUMANLIKE = 8, //"humanshape"
			WATER3 = 9,
			MINERAL = 10,
			AMORPHOUS = 11, //"indeterminate"
			WATER2 = 12,
			DITTO = 13,
			DRAGON = 14,
			UNDISCOVERED = 15 //"no-eggs"
		};
		#endregion
		public enum LearnMethod
		{
			levelup = 1,
			egg = 2,
			tutor = 3,
			machine = 4,
			stadium_surfing_pikachu = 5,
			light_ball_egg = 6,
			colosseum_purification = 7,
			xd_shadow = 8,
			xd_purification = 9,
			form_change = 10,
			shadow,// = 8,
			purification// = 7,
		}
		/// <summary>
		/// no parameter,
		/// Positive integer,
		/// Item  = <see cref="Items"/>,
		/// Move = <see cref="Moves"/>,
		/// Species = <see cref="Pokemons"/>,
		/// Type = <see cref="Types"/>
		/// </summary>
		/// <example>
		/// <para>E.G.	Poliwhirl(61)
		///		<code>new int[]{62,186},
		///		new string[]{"Stone,Water Stone","Trade\Item,King's Rock"}),</code></para> 
		/// <para>
		/// E.G. to evolve to sylveon
		///		<code>new int[]{..., 700},
		///		new string[]{..., "Amie\Move,2\Fairy"}),</code>
		/// </para> 
		/// </example>
		/// ToDo: Custom class => new Evolve(){ EvolveMethod.Item, Items.Item } 
		/// ToDo: If all conditions are met in "new Evolve()" instance, then evolve.
		/// Ideas:
		/// Merge two or more existing methods together into a new one.
		/// Evolution that depends on the Pokémon's nature or form.
		/// Fusion evolution(e.g. for Magnemite/Slowpoke). Check that there is a Shellder in the party, and if so, delete it and evolve the levelled-up Slowpoke.
		/// Check how many EVs the Pokémon has, and allows evolution only if that amount is greater than or equal to the EV threshold value set by the parameter.
		/// Shiny-Random?
		public enum EvolutionMethod
		{
			/// <summary>
			///	if pokemon's level is greater or equal to int level
			/// <code>Level,int level</code>
			/// </summary>
			Level,
			/// <summary>
			///	Exactly the same as "Level", except the Pokémon must also be male.
			/// <code>Level,int level</code>
			/// </summary>
			/// <example>Burmy</example>
			LevelMale,
			/// <summary>
			///	Exactly the same as "Level", except the Pokémon must also be female.
			/// <code>Level,int level</code>
			/// </summary>
			/// <example>Burmy, Combee</example>
			LevelFemale,
			///	<summary>
			///	The Pokémon will evolve if a particular item is used on it 
			///	(named by the parameter - typically an evolution stone).
			/// <code>Stone,string itemName</code>
			///	</summary>
			/// <example>Clefairy, Cottonee, Eelektrik, Eevee, Exeggcute, Gloom, 
			/// Growlithe, Jigglypuff, Lampent, Lombre, Minccino, Misdreavus, 
			/// Munna, Murkrow, Nidorina, Nidorino, Nuzleaf, Panpour, Pansage, 
			/// Pansear, Petilil, Pikachu, Poliwhirl, Roselia, Shellder, Skitty, 
			/// Staryu, Sunkern, Togetic, Vulpix, Weepinbell</example>
			Item,
			/// <summary>
			///	Exactly the same as "<see cref="Item"/>", 
			///	except the Pokémon must also be male.
			/// </summary>
			/// <example>Kirlia</example>
			ItemMale,
			/// <summary>
			///	Exactly the same as "<see cref="Item"/>", 
			///	except the Pokémon must also be female.
			/// </summary>
			/// <example>Snorunt</example>
			ItemFemale,
			/// <summary>
			///	The Pokémon will evolve immediately after it is traded.
			///	</summary>
			/// <example>Boldore, Graveler, Gurdurr, Haunter, Kadabra, Machoke</example>
			Trade,
			/// <summary>
			///	Exactly the same as "Trade", 
			///	except the Pokémon must also be holding a particular item 
			///	(named by the parameter). 
			///	That item is removed afterwards.
			///	</summary>
			/// <example>Clamperl, Dusclops, Electabuzz, Feebas, Magmar, 
			/// Onix, Porygon, Porygon2, Poliwhirl, Rhydon, Scyther, Seadra, 
			/// Slowpoke</example>
			TradeItem,
			/// <summary>
			///	Exactly the same as "Trade", 
			///	except the Pokémon must have been traded for a Pokémon of a certain species 
			///	(named by the parameter).
			///	</summary>
			/// <example>Karrablast, Shelmet</example>
			TradeSpecies,
			/// <summary>
			///	if pokemon's happiness is greater or equal to 220.
			///	Note: Happiness checks should come last before all other methods.
			///	</summary>
			/// <example>Azurill, Buneary, Chansey, Cleffa, Golbat, 
			/// Igglybuff, Munchlax, Pichu, Swadloon, Togepi, Woobat</example>
			Happiness,
			/// <summary>
			///	Exactly the same as "Happiness", 
			///	but will only evolve during the daytime.
			///	</summary>
			/// <example>Budew, Eevee, Riolu</example>
			HappinessDay,
			/// <summary>
			///	Exactly the same as "Happiness", 
			///	but will only evolve during the night-time.
			///	</summary>
			/// <example>Chingling, Eevee</example>
			HappinessNight,
			/// <summary>
			///	This method is almost identical to "Happiness", 
			///	with the sole changes of replacing the "greater than" sign 
			///	to a "less than" sign, and changing the threshold value. 
			///	If you use this method, 
			///	you may also want to make it easier to get a Pokémon to hate you in-game 
			///	(currently the only ways to do this are fainting and using herbal medicine, 
			///	which can easily be countered by the many more happiness-boosting methods).
			///	</summary>
			Hatred,
			/// <summary>
			///	if time is between 9PM and 4AM time is "Night". else time is "Day".
			///	if time is equal to string dayNight (either Day, or Night).
			///	<code>Time,DatetimeOffset/bool dayNight</code>
			/// </summary>
			Time,
			/// <summary>
			///	if date is between Day-Month or maybe certain 1/4 (quarter) of the year.
			///	if date is equal to string season (either Summer, Winter, Spring, or Fall).
			///	<code>Time,DatetimeOffset/bool season</code>
			/// </summary>
            /// is holiday to "spot-on" as an occasion or requirement for leveling-up?
			Season,
			/// <summary>
			///	if pokemon's heldItem is equal to string itemName
			/// <example>Item,string itemName</example>
			/// </summary>
			/// Holding a certain item after leveling-up?
			HoldItem,
			/// <summary>
			///	The Pokémon will evolve if it levels up during the daytime 
			///	while holding a particular item (named by the parameter).
			/// <code>Item,string itemName</code>
			/// </summary>
			/// <example>Happiny</example>
			HoldItemDay,
			/// <summary>
			///	The Pokémon will evolve if it levels up during the night-time 
			///	while holding a particular item (named by the parameter).
			/// <code>Item,string itemName</code>
			/// </summary>
			/// <example>Gligar, Sneasel</example>
			HoldItemNight,
			/// <summary>
			/// The Pokémon will evolve when it levels up, 
			/// if its beauty stat is greater than or equal to the parameter.
			/// </summary>
			/// <example>Feebas</example>
			Beauty,
			/// <summary>
			///	The Pokémon will evolve if it levels up while knowing a particular move (named by the parameter).
			/// <example>Move,string moveName</example>
			/// </summary>
			/// <example>Aipom, Bonsly, Lickitung, Mime Jr., Piloswine, Tangela, Yanma</example>
			Move,
			/// <summary>
			///	The Pokémon will evolve if it levels up while the player has a Pokémon of a certain species in their party 
			///	(named by the parameter). 
			///	The named Pokémon is unaffected.
			/// <example>Pokemon,string pokemonName</example>
			/// </summary>
			/// <example>Mantyke, Mantine</example>
			/// if party contains a Remoraid
			Party,
			/// <summary>
			///	The Pokémon will evolve if it levels up while the player has a Pokémon of a certain type in their party 
			///	(named by the parameter). 
			/// <example>Type,string pokemonTypeName</example>
			/// </summary>
			/// <example>Pangoro</example>
			/// if party contains a dark pokemon
			Type,
			/// <summary>
			/// The Pokémon will evolve when it levels up, 
			/// if the player is currently on the map given by the parameter.
			///	<code>Map,string mapName</code>
			/// </summary>
			Location,
			///	<summary>
			///	The Pokémon will evolve if it levels up only during a certain kind of overworld weather.
			///	</summary>
			/// <example>Goodra</example>
			///	if currentMap's weather is rain
			Weather,
			///	<summary>
			///	Exactly the same as "Level", 
			///	except the Pokémon's Attack stat must also be greater than its Defense stat.
			///	</summary>
			/// <example>Hitmonlee</example>
			AttackGreater,
			///	<summary>
			///	Exactly the same as "Level", 
			///	except the Pokémon's Attack stat must also be lower than its Defense stat.
			///	</summary>
			/// <example>Hitmonchan</example>
			DefenseGreater,
			/// <summary>
			/// Exactly the same as "Level", 
			/// except the Pokémon's Attack stat must also be equal to its Defense stat.
			/// </summary>
			/// <example>Hitmontop</example>
			AtkDefEqual,
			///	<summary>if pokemon's shinyValue divided by 2's remainder is equal to 0 or 1</summary>
			///	What about level parameter? Maybe "ShinyYes/ShinyNo" or "Shiny0/Shiny1", in combination with Level-parameter? 
			Shiny,
			/// <summary>Unique evolution methods: if pokemon's shinyValue divided by 2's remainder is equal to 0</summary>
			/// Shiny value? I thought it was based on "Friendship"
			Silcoon,
			///	<summary>Unique evolution methods: if pokemon's shinyValue divided by 2's remainder is equal to 1</summary>
			Cascoon,
			///	<summary>
			///	Unique evolution methods: 
			///	Exactly the same as "Level". 
			///	There is no difference between the two methods at all. 
			///	Is used alongside the method "Shedinja".
			///	</summary>
			Ninjask,
			///	<summary>
			///	Unique evolution methods: 
			///	Must be used with the method "Ninjask". 
			///	Duplicates the Pokémon that just evolved 
			///	(if there is an empty space in the party), 
			///	and changes the duplicate's species to the given species.
			///	</summary>
            ///	how is this different from "party"?
			Shedinja,
            /// <summary>
            /// </summary>
            /// Just wanted to see a requirement for after "fainting" too many times, your pokemon just died, and became a ghost-type...  
            Deaths
		}
    }
}