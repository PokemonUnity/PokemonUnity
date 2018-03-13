using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 
/// </summary>
/// ToDo: Consider nesting PokemonData class?
[System.Serializable]
public class Pokemon //: ePokemons //PokemonData
{
	#region Variables
	/// <summary>
	/// Current Total HP
	/// </summary>
	public int TotalHP { get { return totalHP; } }
	private int totalHP = 1;
	/// <summary>
	/// Current HP
	/// </summary>
	private int hp = 1;
	/// <summary>
	/// Current Attack Stat
	/// </summary>
	private int ATK;
	/// <summary>
	/// Current Defense stat
	/// </summary>
	private int DEF;
	/// <summary>
	/// Current Special Attack Stat
	/// </summary>
	private int SPA;
	/// <summary>
	/// Current Special Defense Stat
	/// </summary>
	private int SPD;
	/// <summary>
	/// Current Speed Stat
	/// </summary>
	private int SPE;
	/// <summary>
	/// Array of 6 Individual Values for HP, Atk, Def, Speed, Sp Atk, and Sp Def
	/// </summary>
	private readonly int[] IV = new int[6];
	/// <summary>
	/// Effort Values
	/// </summary>
	private readonly int[] EV = new int[6]; //{ 0, 0, 0, 0, 0, 0 }; //same thing
	/// <summary>
	/// Species (National Pokedex number)
	/// </summary>
	/// ToDo: Fetch from PokemonData : _base.PokeId
	private int species;
	/// <summary>
	/// Personal/Pokemon ID
	/// </summary>
	/// ToDo: String value?
	private int PersonalId;
	/// <summary>
	/// 32-bit Trainer ID (the secret ID is in the upper 16-bits);
	/// Deprecated
	/// </summary>
	/// ToDo: Remove this, and fetch from Trainer Class?
	/// Can also store hexadecimal/binary values in int
	private int TrainerId;
	/// <summary>
	/// Pokerus strain and infection time
	/// </summary>
	/// <remarks>
	/// ToDo: Custom class?
	/// 3 Values; Not Infected, Cured, Infected.
	/// [0] = Pokerus Strain; [1] = Days until cured.
	/// if ([0] && [1] == 0) => Not Infected
	/// </remarks>
	private readonly int[] pokerus = new int[2]; //{ 0, 0 };
	/// <summary>
	/// Held item
	/// </summary>
	private eItems.Item item;
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
	private bool belch;
	/// <summary>
	/// Mail?...
	/// </summary>
	private bool? mail;
	/// <summary>
	/// The pokemon fused into this one.
	/// </summary>
	private int fused;
	/// <summary>
	/// Nickname
	/// </summary>
	private string name;
	/// <summary>
	/// Current experience points
	/// </summary>
	private int exp;
	/// <summary>
	/// Current happiness
	/// </summary>
	/// <remarks>
	/// This is the samething as "friendship";
	/// </remarks>
	private int happiness;
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
	/// Status problem (PBStatuses)
	/// </summary>
	/// ToDo: Status Class
	private bStatus status;
	public enum bStatus
	{
		None,
		Sleep,
		Poison,
		Paralysis,
		Burn,
		Frozen
	}
	/// <summary>
	/// Sleep count/Toxic flag
	/// </summary>
	/// ToDo: Add to Status Class
	private int statusCount;
	/// <summary>
	/// Steps to hatch egg, 0 if Pokemon is not an egg
	/// </summary>
	private int eggSteps;
	/// <summary>
	/// Moves (PBMove)
	/// </summary>
	/// ToDo Move class, not enum
	private Move[] moves = new Move[4]; //{ Move.MoveData.Move.NONE, Move.MoveData.Move.NONE, Move.MoveData.Move.NONE, Move.MoveData.Move.NONE };
	/// <summary>
	/// The moves known when this Pokemon was obtained
	/// </summary>
	private Move.MoveData.Move[] firstMoves = new Move.MoveData.Move[4];
	/// <summary>
	/// Ball used
	/// </summary>
	private eItems.Item ballUsed = (eItems.Item)0; //ToDo: None?
	/// <summary>
	/// Markings
	/// </summary>
	private readonly bool[] markings = new bool[6]; //{ false, false, false, false, false, false };
	/// <summary>
	/// Manner Obtained:
	/// </summary>
	private ObtainedMethod ObtainedMode;
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
	private int obtainMap;
	/// <summary>
	/// Replaces the obtain map's name if not null
	/// </summary>
	private string obtainString;
	/// <remarks>
	/// Wouldnt this change again when traded to another trainer?
	/// </remarks>
	private int obtainLevel; // = 0;
	private System.DateTimeOffset obtainWhen;
	private System.DateTimeOffset hatchedWhen;
	/// <summary>
	/// Original Trainer's Name
	/// </summary>
	/// <remarks>
	/// ToDo: PlayerTrainer Class here
	/// </remarks>
	private Trainer OT;
	/// <summary>
	/// Forces the first/second/hidden (0/1/2) ability
	/// </summary>
	private eAbility.Ability[] abilityFlag = new eAbility.Ability[2];// readonly
	/// <summary>
	/// </summary>
	/// <remarks>
	/// isMale; null = genderless?
	/// Should consider gender as byte? bool takes up same amount of space
	/// </remarks>
	private bool? genderFlag;
	/// <summary>
	/// Forces a particular nature
	/// </summary>
	/// ToDo: Redo NatureDatabase Class
	private NatureDatabase.Nature natureFlag;
	/// <summary>
	/// Forces the shininess
	/// </summary>
	private bool? shinyFlag;
	/// <summary>
	/// Array of ribbons
	/// </summary>
	/// <remarks>
	/// Make 2d Array (Array[,]) separated by regions/Gens
	/// </remarks>
	private bool[] ribbons; //= new bool[numberOfRegions,RibbonsPerRegion];
	/// <summary>
	/// Contest stats
	/// </summary>
	private int cool, beauty, cute, smart, tough, sheen;
	private PokemonData _base;
	/// <summary>
	/// Max total EVs
	/// </summary>
	const int EVLIMIT = 510; //static readonly
	/// <summary>
	/// Max EVs that a single stat can have
	/// </summary>
	/// ToDo: use const instead?
	/// Can be referenced as [Attribute] if is a const value
	const int EVSTATLIMIT = 252; //static readonly
	/// <summary>
	/// Maximum length a Pokemon's nickname can be
	/// </summary>
	const int NAMELIMIT = 10; //static readonly
	#endregion

	public Pokemon() { }

	/// <summary>
	/// Uses PokemonData to initialize a Pokemon from base stats
	/// </summary>
	/// <param name="pokemon"></param>
	/// ToDo: Inherit PokemonData 
	public Pokemon(PokemonData.Pokemon pokemon) //ToDo: Redo PokemonDatabase/PokemonData -- DONE
	{
		//PersonalId = 
		_base = PokemonData.GetPokemon(pokemon);
		//Gender = isMale();
		Abilities[0] = _base.Abilities[1] == eAbility.Ability.NONE ? _base.Abilities[0] : _base.Abilities[new Random().Next(0, 2)];
		Nature = (NatureDatabase.Nature)(new Random().Next(0, 24));
		//IsShiny
	}

	#region Ownership, obtained information
	/// <summary>
	/// Returns whether or not the specified Trainer is the NOT this Pokemon's original trainer
	/// </summary>
	/// <param name="trainer"></param>
	/// <returns></returns>
	public bool isForeign(Trainer trainer) {
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
	/*public int ObtainLevel()
    {
        //int level = 0; int.TryParse(this.obtainLevel, out level);
        return this.obtainLevel;
    }*/
	public int ObtainLevel
	{
		get { return this.obtainLevel; }
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
			return 0;
			// ToDo: return Experience.GetLevelFromExperience(this.exp, this.GrowthRate)
		}
		set
		{
			if (value < 1 || value > 100) //Experience.MAXLEVEL
				throw new Exception(string.Format("The level number {0} is invalid", value));
			// ToDo: return Experience.GetStartExperience(value, this.GrowthRate)
		}
	}

	public bool isEgg
	{
		get
		{
			return eggSteps > 0;
		}
	}

	public PokemonData.LevelingRate GrowthRate
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
	/// <summary>
	/// Returns this Pokemons gender. male; female; genderless.
	/// Sets this Pokemon's gender to a particular gender (if possible)
	/// </summary>
	public bool? Gender { get { return this.genderFlag; } }

	/*// <summary>
    /// Helper function that determines whether the input values would make a female.
    /// </summary>
    /// ToDo: isMale; isFemale; isGenderless... properties?
    public bool? isMale/isFemale/isGenderless//(float genderRate = this._base.MaleRatio)
    {
        get
        {
            if (genderRate == 100f) return true; 
            if (genderRate == 100f) return false; //Always female
            return null; //genderless
        }
    }*/

	/// <summary>
	/// Returns whether this Pokemon species is restricted to only ever being one gender (or genderless)
	/// </summary>
	public bool isSingleGendered { get { return true; } }
	#endregion

	#region Ability
	/// <summary>
	/// Returns the ID of the Pokemons Ability.
	/// </summary>
	/// ToDo: Sets this Pokemon's ability to a particular ability (if possible)
	/// ToDo: Ability 1 or 2, never both...
	/// ToDo: Error on non-compatible ability?
	public eAbility.Ability[] Abilities { get { return abilityFlag; } set { abilityFlag = value; } }//ToDo: Check against getAbilityList()?

	/// <summary>
	/// Returns whether this Pokemon has a partiular ability
	/// </summary>
	/// <param name="ability"></param>
	/// <returns></returns>
	public bool hasAbility(eAbility.Ability ability = eAbility.Ability.NONE)
	{
		if (ability == eAbility.Ability.NONE) return (int)Abilities[0] > 0 || (int)Abilities[1] > 0;// || (int)Abilities[2] > 0;
		else
		{
			return Abilities[0] == ability || Abilities[1] == ability;// || Abilities[2] == ability;
		}
		//return false;
	}

	public bool hasHiddenAbility()
	{
		return Abilities[1] != eAbility.Ability.NONE;
	}

	/// <summary>
	/// Returns a list of abilities this Pokemon can have.
	/// </summary>
	/// <returns></returns>
	/// Is there a list of abilities a pokemon can have outside of "default" values?
	public eAbility.Ability[] getAbilityList()
	{
		//List<eAbility.Ability> abilList;
		//foreach(){ list.add() }
		//eAbility.Ability[] abilities = abilList.ToArray();
		//return abilities;
		return this._base.Abilities; //ToDo: List of abilities?
	}
	#endregion

	#region Nature
	/// <summary>
	/// Returns the ID of this Pokemon's nature
	/// Sets this Pokemon's nature to a particular nature (and calculates the stat modifications).
	/// </summary>
	public NatureDatabase.Nature Nature { get { return this.natureFlag; } set { this.natureFlag = value; /*nature.calcStats();*/ } }//ToDo:

	/// <summary>
	/// Returns whether this Pokemon has a particular nature
	/// </summary>
	/// <param name="nature"></param>
	/// <returns></returns>
	public bool hasNature(NatureDatabase.Nature nature = 0) //None
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
	public bool isShiny()
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
	public bool IsShiny
	{
		//If not manually set, use math to figure out.
		//ToDo: Store results to save on redoing future execution? 
		get { return shinyFlag ?? isShiny()/*false*/; }
		set { shinyFlag = value; }
	}
	#endregion

	#region Pokerus
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
	public bool hasType(PokemonData.Type type)
	{
		return this._base.Types[0] == type || this._base.Types[1] == type;
	}

	/// <summary>
	/// Returns this Pokemon's first type
	/// </summary>
	public PokemonData.Type Type1 { get { return this._base.Types[0]; } }

	/// <summary>
	/// Returns this Pokemon's second type
	/// </summary>
	public PokemonData.Type Type2 { get { return this._base.Types[1]; } }
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
	public bool hasMove(Move.MoveData.Move move) {
		//if (move <= 0) return false;//move == null ||
		for (int i = 0; i < 4; i++)
		{
			if (this.moves[i].MoveId == move) return true;
		}
		return false;
	}

	public bool knowsMove(Move.MoveData.Move move) { return this.hasMove (move); }

	/// <summary>
    /// Returns the list of moves this Pokémon can learn by levelling up.
    /// </summary>
    /// ToDo: Custom<int Level, eMove move> Class
    public Move.MoveData.Move[] getMoveList() {
		Move.MoveData.Move[] movelist = _base.MovesetMoves;
        for (int k = 0; k < movelist.Length - 1; k++)
		{
			//Array to List/Dictionary
			//separate into Move.value and Pokemon.Level 
			//needed to learn the skill
			//movelist([level, move])}
		}
        return movelist;
     }

	/*// <summary>
    /// Sets this Pokémon's movelist to the default movelist it originally had.
    /// </summary>
    void resetMoves()
    {
        //Move.MoveData.Move moves = this.getMoveList();
        Move.MoveData.Move[] movelist;
        foreach(var i in moves) {//for (int i = 0; i < moveList; i++){
            if (i[0] <= this.level)
            {
                movelist[movelist.length] = i[1];
            }
        }
        //movelist|=[] // Remove duplicates
        int listend = movelist.length - 4;
        listend = listend < 0 ? 0 : listend;
        int j = 0;
        for (int i = 0; i < listend + 4; i++) { //i in listend...listend+4
            moveid = (i >= movelist.length) ? 0 : movelist[i];
            @moves[j] = PBMove.new(moveid);
            j += 1;
        }
    }*/

	/*// <summary>
	/// Silently learns the given move. Will erase the first known move if it has to.
	/// </summary>
	/// <param name=""></param>
	/// <returns></returns>
	void LearnMove(Move.MoveData.Move move) {
		if ((int)move <= 0) return;
		for (int i = 0; i < 4; i++) {
			if (moves[i].id == move) {
				int j = i + 1;
				while (j < 4) {
					if (moves[j].id == 0) break;
					tmp = @moves[j];
					@moves[j] = @moves[j - 1];
					@moves[j - 1] = tmp;
					j += 1;
				}
				return;
			}
		}
		for (int i = 0; i < 4; i++) {
			if (@moves[i].id == 0) {
				@moves[i] = new Move(move);
				return;
			}
		}
		@moves[0] = @moves[1];
		@moves[1] = @moves[2];
		@moves[2] = @moves[3];
		@moves[3] = new Move(move);
	}*/

	/*// <summary>
	/// Deletes the given move from the Pokémon.
	/// </summary>
	/// <param name=""></param>
	/// <returns></returns>
	void pbDeleteMove(move) {
		return if !move || move <= 0
		newmoves =[]
		for (int i = 0; i < 4; i++) { 
			if (moves[i].id != move) newmoves.push(@moves[i]);
		}

		newmoves.push(PBMove.new(0));
		for (int i = 0; i< 4; i++) {
			@moves[i] = newmoves[i];
		}
	 }*/

	/*// <summary>
	/// Deletes the move at the given index from the Pokémon.
	/// </summary>
	/// <param name=""></param>
	/// <returns></returns>
	void DeleteMoveAtIndex(index) {
		newmoves =[];

		for (int i = 0; i < 4; i++) {
			if (i != index) newmoves.push(@moves[i]);
		}

		newmoves.push(PBMove.new(0));

		for (int i = 0; i < 4; i++) {
			@moves[i] = newmoves[i];
		}
	}*/

	/*// <summary>
	/// Deletes all moves from the Pokémon.
	/// </summary>
	void DeleteAllMoves() { 
		//for (int i = 0; i< 4; i++) { 
		//	moves[i]= new Move(0);
		//}
		moves = new Move.MoveData.Move[4];
	}*/

	/*// <summary>
	/// Copies currently known moves into a separate array, for Move Relearner.
	/// </summary>
	void RecordFirstMoves() {
		//for (int i = 0; i < 4; i++) {
		//	if (moves[i].id > 0) firstmoves.push(moves[i].id);
		//}
		//firstmoves = moves;
	}*/

	/*void AddFirstMove(Move.MoveData.Move move) {
		if (move > 0 && !firstMoves.include(move)) firstMoves.push(move);
		return;
	}*/

	/*void RemoveFirstMove(Move.MoveData.Move move) {
		//if (move > 0) firstMoves.delete(move); 
		return;
	}*/

	/*void ClearFirstMoves() {
		firstMoves = new Move.MoveData.Move[4];
	}*/

	/*bool isCompatibleWithMove(move) {
		return SpeciesCompatible(this.species, move);
	}*/

	/// <summary>
	/// Reduce the global clutter, and add to more readable 
	/// and maintainable code by encapsulation to logically 
	/// group classes that are only used in one place.
	/// </summary>
	internal class Moves
	{

	}
    #endregion

    #region Contest attributes, ribbons
    #endregion

    #region Items
    #endregion

    #region Other
    /// <summary>
    /// Nickname; 
    /// Returns Pokemon species name if not nicknamed.
    /// </summary>
    public string Name { get { return name ?? _base.Name; } }

    public int Form { set { /*if(value <= _base.Forms)*/_base.Form = value; } }//ToDo: Fix Forms and uncomment

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
    }*/
    public bool[] Markings { get { return this.markings; } }

    /// <summary>
    /// Returns a string stathing the Unown form of this Pokemon
    /// </summary>
    /// <returns></returns>
    public char UnknownShape()
    {
        return "ABCDEFGHIJKLMNOPQRSTUVWXYZ?!".ToCharArray()[_base.ArrayId]; //ToDo: FormId; "if pokemon is an Unknown"
    }

    /// <summary>
    /// Returns the EV yield of this Pokemon
    /// </summary>
    /*// <returns></returns>
    public int[] evYield()
    {
        return EV;
    }*/
    public int[] evYield { get { return this.EV; } }//_base.getBaseStats();

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
            this.hp = value < 0 ? 0 : value > this.TotalHP ? TotalHP : value;
            if (this.hp == 0) this.status = 0; // statusCount = 0; //ToDo: Fainted
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
        this.status = 0; statusCount = 0; //remove status ailments
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
                gain += happiness < 200 ? 1 : 0;
                //gain += this.metMap.Id == currentMap.Id ? 1 : 0; //change to "obtainMap"?
                break;
            case HappinessMethods.LEVELUP:
                gain = 2;
                if (happiness < 200) gain = 3;
                if (happiness < 100) gain = 5;
                luxury = true;
                break;
            case HappinessMethods.GROOM:
                gain = 4;
                if (happiness < 200) gain = 10;
                luxury = true;
                break;
            case HappinessMethods.FAINT:
                gain = -1;
                break;
            case HappinessMethods.VITAMIN:
                gain = 2;
                if (happiness < 200) gain = 3;
                if (happiness < 100) gain = 5;
                break;
            case HappinessMethods.EVBERRY:
                gain = 2;
                if (happiness < 200) gain = 5;
                if (happiness < 100) gain = 10;
                break;
            case HappinessMethods.POWDER:
                gain = -10;
                if (happiness < 200) gain = -5;
                break;
            case HappinessMethods.ENERGYROOT:
                gain = -15;
                if (happiness < 200) gain = -10;
                break;
            case HappinessMethods.REVIVALHERB:
                gain = -20;
                if (happiness < 200) gain = -15;
                break;
            default:
                break;
        }
        gain += luxury && this.ballUsed == eItems.Item.LUXURY_BALL ? 1 : 0;
        if (this.item == eItems.Item.SOOTHE_BELL && gain > 0)
            gain = (int)Math.Floor(gain * 1.5f);
        happiness += gain;
        happiness = happiness > 255 ? 255 : happiness < 0 ? 0 : happiness; //Max 255, Min 0
    }
	#endregion

	#region Stat calculations, Pokemon creation
	#endregion

	#region Nested Classes
	public class PokemonData
	{
		#region Variables
		/// <summary>
		/// Id is the database value for specific pokemon+form
		/// Different Pokemon forms share the same Pokedex number. 
		/// Values are loaded from <see cref="Pokemon"/>, where each form is registered to an Id.
		/// </summary>
		/// <example>
		/// Deoxys Pokedex# can be 1,
		/// but Deoxys-Power id# can be 32
		/// </example>
		/// <remarks>If game event/gm wants to "give" player Deoxys-Power form and not Speed form</remarks>
		private Pokemon id;
		/// <summary>
		/// Different Gens assign different pokedex num
		/// </summary>
		/// <remarks>Think there is 3 pokedex</remarks>
		private int[] regionalPokedex;
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
		private string name;
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
		private string species;
		/// <summary>
		/// </summary>
		private string pokedexEntry;
		/// ToDo: Maybe PokemonData contains count of # of forms?
		private int forms; 
		/// <summary>
		/// Represents CURRENT form, if no form is active, current or does not exist
		/// then value is 0.
		/// </summary>
		/// ToDo: Make a PokemonForm class, that establishes the rule for 
		/// <see cref="Pokemon"/> and <see cref="Form"/>
		private int form;// = 0; 

		private Type type1;
		private Type type2;
		/// <summary>
		/// All three pokemon abilities 
		/// (Abiltiy1, Ability2, HiddenAbility).
		/// </summary>
		///	<remarks>
		/// Should be [int? a1,int? a2,int? a3]
		/// instead of above...
		/// </remarks> 
		private readonly eAbility.Ability[] abilities = new eAbility.Ability[3];

		/// <summary>
		/// The male ratio.
		/// </summary>
		/// instead of a float value...
		private GenderRatio maleRatio;
		/// <summary>max is 255</summary> 
		private int catchRate;
		private EggGroups eggGroup1;
		private EggGroups eggGroup2;
		private int hatchTime;

		//private float hitboxWidth; //used for 3d battles; just use collision detection from models
		private float height;
		private float weight;
		/// ToDo: Make this an enum
		private int shapeID; 

		private int baseExpYield;
		private LevelingRate levelingRate;

		private Color pokedexColor;
		/// <summary>
		/// Friendship levels is the same as pokemon Happiness.
		/// </summary>
		private int baseFriendship;

		private int baseStatsHP;
		private int baseStatsATK;
		private int baseStatsDEF;
		private int baseStatsSPA;
		private int baseStatsSPD;
		private int baseStatsSPE;

		private float luminance;
		private UnityEngine.Color lightColor;

		/// <summary>
		/// [item id,% chance]
		/// </summary>
		/// <example>int[,] heldItems = { {1,2,3},{4,5,6} }
		/// <para>heldItems[1,0] == 4 as int</para>
		/// </example>
		/// <remarks>
		/// Or maybe...[item id,% chance,generationId/regionId]
		/// Custom Class needed here... <see cref="eItems.Item"/> as itemId
		/// </remarks>
		private int[,] heldItem;

		private int[] movesetLevels;
		private Move.MoveData.Move[] movesetMoves;

		//private string[] tmList; //Will be done thru ItemsClass

		private int[] evolutionID;
		/// ToDo: Evolution class type array here
		PokemonEvolution[] evolutions;
		private string[] evolutionRequirements;
		/// <summary>
		/// The item that needs to be held by a parent when breeding in order for the egg to be this species. 
		/// If neither parent is holding the required item, the egg will be the next evolved species instead.
		/// <para></para>
		/// The only species that should have this line are ones which cannot breed, 
		/// but evolve into a species which can. That is, the species should be a "baby" species.
		/// Not all baby species need this line.
		/// </summary>
		private eItems.Item Incense;
		#endregion

		/// ToDo: Should any of the property values be Set-able?
		#region Properties
		/// <summary>
		/// Id is the database value for specific pokemon+form
		/// Different Pokemon forms share the same Pokedex number. 
		/// Values are loaded from <see cref="Pokemon"/>, where each form is registered to an Id.
		/// </summary>
		/// <example>
		/// Deoxys Pokedex# can be 1,
		/// but Deoxys-Power id# can be 32
		/// </example>
		/// <remarks>If game event/gm wants to "give" player Deoxys-Power form and not Speed form</remarks>
		public Pokemon ID { get { return this.id; } }
		/// <summary>
		/// Different Gens assign different pokedex num
		/// example: Bulbasaur = [1,231]
		/// </summary>
		/// <remarks>Think there is 3 pokedex</remarks>
		public int[] RegionalPokedex { get { return this.regionalPokedex; } }
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
		public string Name { get { return PokemonData.GetPokedexTranslation(this.ID).Forms[this.Form] ?? this.name; } }
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
		public string Species { get { return this.species; } }
		/// <summary>
		/// </summary>
		public string PokedexEntry { get { return this.pokedexEntry; } }
		/// <summary>
		/// Form is the same Pokemon Pokedex entry. 
		/// Changing forms should change name value
		/// </summary>
		/// but a different PokemonId
		/// If null, returns this.Pokemon.Id
		/// ToDo: Changing forms should change name value
		public int Form { get { return this.form; } set { this.form = value; } }
		/// ToDo: I should use the # of Forms from the .xml rather than from the database initializer/constructor
		public int Forms { get { return this.forms; } }

		//public virtual Type Type1 { get { return this.type1; } }
		//public virtual Type Type2 { get { return this.type2; } }
		//Maybe use this instead?
		public virtual Type[] Types { get { return new PokemonData.Type[] { this.type1, this.type2 }; } }
		/// <summary>
		/// All three pokemon abilities 
		/// (Abiltiy1, Ability2, HiddenAbility).
		/// </summary>
		///	<remarks>
		/// Should be [int? a1,int? a2,int? a3]
		/// instead of above...
		/// </remarks> 
		public eAbility.Ability[] Abilities { get { return this.abilities; } }

		/// <summary>
		/// The male ratio.
		/// <value>-1f is interpreted as genderless</value>
		/// </summary>
		public GenderRatio MaleRatio { get { return this.maleRatio; } }
		public float ShinyChance { get; set; }
		/// <summary>
		/// The catch rate of the species. 
		/// Is a number between 0 and 255 inclusive. 
		/// The higher the number, the more likely a capture 
		/// (0 means it cannot be caught by anything except a Master Ball).
		/// </summary> 
		/// Also known as Pokemon's "Rareness"...
		public int CatchRate { get { return this.catchRate; } }
		public EggGroups[] EggGroup { get { return new EggGroups[] { this.eggGroup1, this.eggGroup2 }; } }
		//public EggGroup EggGroup2 { get { return this.eggGroup2; } }
		public int HatchTime { get { return this.hatchTime; } }

		//public float hitboxWidth { get { return this.hitboxWidth; } } //used for 3d battles just use collision detection from models
		public float Height { get { return this.height; } }
		public float Weight { get { return this.weight; } }
		/// ToDo: Make this an enum
		public int ShapeID { get { return this.shapeID; } } 

		public int BaseExpYield { get { return this.baseExpYield; } }
		public LevelingRate GrowthRate { get { return this.levelingRate; } }

		public Color PokedexColor { get { return this.pokedexColor; } }
		/// <summary>
		/// Friendship levels is the same as pokemon Happiness.
		/// </summary>
		public int BaseFriendship { get { return this.baseFriendship; } }

		public int BaseStatsHP { get { return this.baseStatsHP; } }
		public int BaseStatsATK { get { return this.baseStatsATK; } }
		public int BaseStatsDEF { get { return this.baseStatsDEF; } }
		public int BaseStatsSPA { get { return this.baseStatsSPA; } }
		public int BaseStatsSPD { get { return this.baseStatsSPD; } }
		public int BaseStatsSPE { get { return this.baseStatsSPE; } }

		public float Luminance { get { return this.luminance; } }
		public UnityEngine.Color LightColor { get { return this.lightColor; } }

		/// <summary>
		/// [item id,% chance]
		/// </summary>
		/// <example>int[,] heldItems = { {1,2,3},{4,5,6} }
		/// <para>heldItems[1,0] == 4 as int</para>
		/// </example>
		/// <remarks>
		/// Or maybe...[item id,% chance,generationId/regionId]
		/// Custom Class needed here... <see cref="eItems.Item"/> as itemId
		/// </remarks>
		public int[,] HeldItem { get { return this.heldItem; } }

		public int[] MovesetLevels { get { return this.movesetLevels; } }
		public Move.MoveData.Move[] MovesetMoves { get { return this.movesetMoves; } }

		public int[] EvolutionID { get { return this.evolutionID; } }
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
		public string[] EvolutionRequirements { get { return this.evolutionRequirements; } }
		#endregion

		#region Enumerators
		public enum Type
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
		/// <summary>
		/// The likelihood of a Pokémon of the species being a certain gender.
		/// </summary>
		public enum GenderRatio
		{
			AlwaysMale,
			FemaleOneEighth,
			Female25Percent,
			Female50Percent,
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
		/// <summary>
		/// Pokemon ids are connected to XML file.
		/// </summary>
		/// <remarks>Can now code with strings or int and
		/// access the same value.</remarks>
		public enum Pokemon
		{
			NONE = 0
		}
		#endregion

		#region Constructors
		public PokemonData() { }// this.name = PokemonData.GetPokedexTranslation(this.ID).Forms[this.Form] ?? this.Name; } //name equals form name unless there is none.

		public PokemonData(Pokemon Id, int[] regionalDex/*, string name*/, Type? type1, Type? type2, eAbility.Ability[] abilities, //eAbility.Ability? ability1, eAbility.Ability? ability2, eAbility.Ability? hiddenAbility,
							GenderRatio maleRatio, int catchRate, EggGroups eggGroup1, EggGroups eggGroup2, int hatchTime,
							float height, float weight, int baseExpYield, LevelingRate levelingRate,
							/*int? evYieldHP, int? evYieldATK, int? evYieldDEF, int? evYieldSPA, int? evYieldSPD, int? evYieldSPE,*/
							Color pokedexColor, int baseFriendship,//* / string species, string pokedexEntry,*/
							int baseStatsHP, int baseStatsATK, int baseStatsDEF, int baseStatsSPA, int baseStatsSPD, int baseStatsSPE,
							float luminance, /*Color lightColor,*/ int[] movesetLevels, Move.MoveData.Move[] movesetMoves, int[] tmList,
							int[] evolutionID, int[] evolutionLevel, int[] evolutionMethod, /*string[] evolutionRequirements,*/ int forms,
							int[,] heldItem = null)
		{//new PokemonData(1,1,"Bulbasaur",12,4,65,null,34,45,1,7,20,7f,69f,64,4,PokemonData.PokedexColor.GREEN,"Seed","\"Bulbasaur can be seen napping in bright sunlight. There is a seed on its back. By soaking up the sun’s rays, the seed grows progressively larger.\"",45,49,49,65,65,45,0f,new int[]{1,3,7,9,13,13,15,19,21,25,27,31,33,37},new int[]{33,45,73,22,77,79,36,75,230,74,38,388,235,402},new int[]{14,15,70,76,92,104,113,148,156,164,182,188,207,213,214,216,218,219,237,241,249,263,267,290,412,447,474,496,497,590},new int[]{2},new int[]{16},new int[]{1})

			PokedexTranslation translation = PokemonData.GetPokedexTranslation(Id);
			this.id = Id;
			this.regionalPokedex = regionalDex;
			this.name = translation.Name;
			this.species = translation.Species;
			this.pokedexEntry = translation.PokedexEntry;
			//this.forms = forms; //| new Pokemon[] { Id }; //ToDo: need new mechanic for how this should work

			this.type1 = type1 != null ? (PokemonData.Type)type1 : PokemonData.Type.NONE;
			this.type2 = type2 != null ? (PokemonData.Type)type2 : PokemonData.Type.NONE;
			this.abilities = abilities;
			//this.ability1Id = (eAbility.Ability)ability1;
			//this.ability2Id = (eAbility.Ability)ability2;
			//this.hiddenAbilityId = (eAbility.Ability)hiddenAbility;

			this.maleRatio = maleRatio; //ToDo
			this.catchRate = catchRate;
			this.eggGroup1 = eggGroup1;
			this.eggGroup2 = eggGroup2;
			this.hatchTime = hatchTime;

			this.height = height;
			this.weight = weight;
			this.baseExpYield = baseExpYield;
			this.levelingRate = (LevelingRate)levelingRate; //== null ? (PokemonData.LevelingRate)levelingRate : PokemonData.LevelingRate.NONE;

			this.baseStatsHP = baseStatsHP;
			this.baseStatsATK = baseStatsATK;
			this.baseStatsDEF = baseStatsDEF;
			this.baseStatsSPA = baseStatsSPA;
			this.baseStatsSPD = baseStatsSPD;
			this.baseStatsSPE = baseStatsSPE;
			this.baseFriendship = baseFriendship; //ToDo: forgot to implement when transfering database

			this.luminance = luminance;
			//this.lightColor = lightColor;
			this.pokedexColor = pokedexColor | PokemonData.Color.NONE;

			//ToDo: wild pokemon held items not yet implemented
			this.heldItem = heldItem; //[item id,% chance]

			this.movesetLevels = movesetLevels;
			this.movesetMoves = movesetMoves; //ToDo: Array Cast conversion
											  //this.tmList = tmList; //ToDo: Need new item database array/enum for this; one that's regional/generation dependant

			this.evolutionID = evolutionID;
			//this.evolutionMethod = evolutionMethod; //ToDo:
			//this.evolutionRequirements = evolutionRequirements;
		}

		public static PokemonData CreatePokemonData(Pokemon Id, int[] PokeId/*, string name*/, int? type1, int? type2, int? ability1, int? ability2, int? hiddenAbility,
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
				(Pokemon)Id,
				PokeId,
				(PokemonData.Type)type1 | PokemonData.Type.NONE,//!= null ? (PokemonData.Type)type1 : PokemonData.Type.NONE,
				(PokemonData.Type)type2 | PokemonData.Type.NONE,//!= null ? (PokemonData.Type)type2 : PokemonData.Type.NONE,
																//new eAbility.Ability[] { 
					(eAbility.Ability)ability1 | eAbility.Ability.NONE,//!= null ? (eAbility.Ability)ability1 : eAbility.Ability.NONE,
					(eAbility.Ability)ability2 | eAbility.Ability.NONE,//!= null ? (eAbility.Ability)ability2 : eAbility.Ability.NONE,
					(eAbility.Ability)hiddenAbility | eAbility.Ability.NONE,//!= null ? (eAbility.Ability)hiddenAbility : eAbility.Ability.NONE
																			//}, 
				0,//ToDo: maleRatio, 
				catchRate,
				(EggGroups)eggGroup1 | PokemonData.EggGroups.NONE,//!= null ? (EggGroups)eggGroup1 : PokemonData.EggGroup.NONE, 
				(EggGroups)eggGroup2 | PokemonData.EggGroups.NONE,//!= null ? (EggGroups)eggGroup2 : PokemonData.EggGroup.NONE, 
				hatchTime,
				height,
				weight,
				baseExpYield,
				(LevelingRate)levelingRate,
				pokedexColor | PokemonData.Color.NONE,
				baseFriendship, baseStatsHP, baseStatsATK, baseStatsDEF, baseStatsSPA, baseStatsSPD, baseStatsSPE,
				luminance, movesetLevels, System.Array.ConvertAll(movesetMoves, move => (Move.MoveData.Move)move), tmList,
				evolutionID, evolutionLevel, evolutionMethod, forms, heldItem);//
		}

		public static PokemonData CreatePokemonData(Pokemon Id, int[] PokeId/*, string name*/, Type type1, Type type2, eAbility.Ability ability1, eAbility.Ability ability2, eAbility.Ability hiddenAbility,
							GenderRatio maleRatio, int catchRate, EggGroups eggGroup1, EggGroups eggGroup2, int hatchTime,
							float height, float weight, int baseExpYield, LevelingRate levelingRate,
							/*int? evYieldHP, int? evYieldATK, int? evYieldDEF, int? evYieldSPA, int? evYieldSPD, int? evYieldSPE,*/
							Color pokedexColor, int baseFriendship,//*/ string species, string pokedexEntry,
							int baseStatsHP, int baseStatsATK, int baseStatsDEF, int baseStatsSPA, int baseStatsSPD, int baseStatsSPE,
							float luminance, /*Color lightColor,*/ int[] movesetLevels, Move.MoveData.Move[] movesetMoves, int[] tmList,
							int[] evolutionID, int[] evolutionLevel, int[] evolutionMethod, /*string[] evolutionRequirements,*/ int forms,
							int[,] heldItem = null)
		{
			return new PokemonData(
				Id,
				PokeId,
				type1, //| PokemonData.Type.NONE,//!= null ? (PokemonData.Type)type1 : PokemonData.Type.NONE,
				type2, //| PokemonData.Type.NONE,//!= null ? (PokemonData.Type)type2 : PokemonData.Type.NONE,
				new eAbility.Ability[] {
				ability1, //| eAbility.Ability.NONE,//!= null ? (eAbility.Ability)ability1 : eAbility.Ability.NONE,
                ability2, //| eAbility.Ability.NONE,//!= null ? (eAbility.Ability)ability2 : eAbility.Ability.NONE,
                hiddenAbility, //| eAbility.Ability.NONE,//!= null ? (eAbility.Ability)hiddenAbility : eAbility.Ability.NONE
				},
				maleRatio,
				catchRate,
				eggGroup1, //| PokemonData.EggGroups.NONE,//!= null ? (EggGroups)eggGroup1 : PokemonData.EggGroup.NONE, 
				eggGroup2, //| PokemonData.EggGroups.NONE,//!= null ? (EggGroups)eggGroup2 : PokemonData.EggGroup.NONE, 
				hatchTime,
				height,
				weight,
				baseExpYield,
				levelingRate,
				pokedexColor | PokemonData.Color.NONE,
				baseFriendship, baseStatsHP, baseStatsATK, baseStatsDEF, baseStatsSPA, baseStatsSPD, baseStatsSPE,
				luminance, movesetLevels, movesetMoves, /*System.Array.ConvertAll(movesetMoves, move => (Move.MoveData.Move)move),*/ tmList,
				evolutionID, evolutionLevel, evolutionMethod, forms, heldItem);//
		}

		/// Not const because translation values
		public static readonly PokemonData[] Database = new PokemonData[] {
            //null
            //  PokemonData.CreatePokemonData(ID, NAME, PokemonData.Type.TYPE1, PokemonData.Type.TYPE2, Ability1, Ability2, HiddenAbility,
            //				MaleRatio, CatchRate, PokemonData.EggGroup.EGGGROUP1, PokemonData.EggGroup.EGGGROUP2, HatchTime, Height, Weight,
            //				EXPYield, PokemonData.LevelingRate.LEVELINGRATE, evYieldHP,ATK,DEF,SPA,SPD,SPE, PokemonData.PokedexColor.COLOR, BaseFriendship,
            //				Species, PokedexEntry (choose your favourite) //needs to be loaded seperately...
            //				baseStatsHP,ATK,DEF,SPA,SPD,SPE, Luminance (0 if unknown), LightColor (Color.clear if unknown)
            //				new int[]{ level, level, level, etc...}
            //				new string[]{ "move", "move", "move", etc...} ), //needs to be loaded separately...
            //				new int[]{pokemonID}, 
            //				new string[]{"Method,Parameter"}),
            PokemonData.CreatePokemonData(Pokemon.NONE, new int[1], Type.NONE, Type.NONE, eAbility.Ability.NONE, eAbility.Ability.NONE, eAbility.Ability.NONE,
						0f, 100, EggGroups.NONE, EggGroups.NONE, 1000,
						10f, 150f, 15, LevelingRate.ERRATIC,
                        /*int? evYieldHP, int? evYieldATK, int? evYieldDEF, int? evYieldSPA, int? evYieldSPD, int? evYieldSPE,*/
                        Color.NONE, 50,
						10, 5, 5, 5, 5, 5,
						0f, new int[] { 1,2,3 }, new Move.MoveData.Move[4], null,//int[] tmList,
                        null, null, null, 4,//int[] evolutionID, int[] evolutionLevel, int[] evolutionMethod, //int forms, 
                        null) //Test
        };
		/*static PokemonData()
		{
			Database = new PokemonData[] {
				//null
				//  PokemonData.CreatePokemonData(ID, NAME, PokemonData.Type.TYPE1, PokemonData.Type.TYPE2, Ability1, Ability2, HiddenAbility,
				//				MaleRatio, CatchRate, PokemonData.EggGroup.EGGGROUP1, PokemonData.EggGroup.EGGGROUP2, HatchTime, Height, Weight,
				//				EXPYield, PokemonData.LevelingRate.LEVELINGRATE, evYieldHP,ATK,DEF,SPA,SPD,SPE, PokemonData.PokedexColor.COLOR, BaseFriendship,
				//				Species, PokedexEntry (choose your favourite) //needs to be loaded seperately...
				//				baseStatsHP,ATK,DEF,SPA,SPD,SPE, Luminance (0 if unknown), LightColor (Color.clear if unknown)
				//				new int[]{ level, level, level, etc...}
				//				new string[]{ "move", "move", "move", etc...} ), //needs to be loaded separately...
				//				new int[]{pokemonID}, 
				//				new string[]{"Method,Parameter"}),
				PokemonData.CreatePokemonData(Pokemon.NONE, new int[1], Type.NONE, Type.NONE, eAbility.Ability.NONE, eAbility.Ability.NONE, eAbility.Ability.NONE,
							0f, 100, EggGroups.NONE, EggGroups.NONE, 1000,
							10f, 150f, 15, LevelingRate.ERRATIC,
							//*int? evYieldHP, int? evYieldATK, int? evYieldDEF, int? evYieldSPA, int? evYieldSPD, int? evYieldSPE,* /
							Color.NONE, 50,
							10, 5, 5, 5, 5, 5,
							0f, new int[] { 1,2,3 }, new Move.MoveData.Move[4], null,//int[] tmList,
							null, null, null, 4,//int[] evolutionID, int[] evolutionLevel, int[] evolutionMethod, //int forms, 
							null) //Test
			};
		}*/

#if DEBUG
		private static Dictionary<int, PokedexTranslation> _pokeTranslations;// = LoadPokedexTranslations();
#else
    private static Dictionary<int, PokedexTranslation> _pokeTranslations;// = LoadPokedexTranslations(SaveData.currentSave.playerLanguage | Settings.Language.English);
#endif
		/// <summary>
		/// 
		/// </summary>
		private static Dictionary<int, PokedexTranslation> _pokeEnglishTranslations;// = LoadEnglishPokedexTranslations();
		/// <summary>
		/// 
		/// </summary>
		///ToDo: Should be a void that stores value to _pokeTranslations instead of returning...
		public static void/*Dictionary<int, PokedexTranslation>*/ LoadPokedexTranslations(Settings.Language language = Settings.Language.English)//, int form = 0
		{
			var data = new Dictionary<int, PokedexTranslation>();

			string fileLanguage;
			switch (language)
			{
				case Settings.Language.English:
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
		public static PokedexTranslation GetPokedexTranslation(Pokemon id, Settings.Language language = Settings.Language.English)// int form = 0,
		{
			if (_pokeTranslations == null) //should return english if player's default language is null
			{
				LoadPokedexTranslations(language);//, form
			}

			int arrayId = (int)id;// GetPokemon(id).ArrayId; //unless db is set, it'll keep looping null...
			if (!_pokeTranslations.ContainsKey(arrayId) && language == Settings.Language.English)
			{
				//Debug.LogError("Failed to load pokedex translation for pokemon with id: " + (int)id); //ToDo: Throw exception error
				throw new System.Exception(string.Format("Failed to load pokedex translation for pokemon with id: {0}_{1}", (int)id, id.ToString()));
				//return new PokedexTranslation();
			}
			//ToDo: Show english text for missing data on foreign languages 
			else if (!_pokeTranslations.ContainsKey(arrayId) && language != Settings.Language.English)
			{
				return _pokeEnglishTranslations[arrayId];
			}

			return _pokeTranslations[arrayId];// int id
		}
		#endregion

		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="ID"></param>
		/// <returns></returns>
		public static PokemonData GetPokemon(Pokemon ID)
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

		static int getPokemonArrayId(Pokemon ID)
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
		#endregion

	}
	/// <summary>
	/// The moves that all Pokémon of the species learn as they level up. 
	/// </summary>
	public class PokemonMoveset
	{
		/// <summary>
		/// Level at which the move is learned 
		/// (0 means the move can only be learned when 
		/// a Pokémon evolves into this species).
		/// </summary>
		/// Level needed to learn Move
		public int Level;
		/// <summary>
		/// Move learned upon leveling-up
		/// </summary>
		public Move.MoveData.Move MoveId;
		public PokemonMoveset() { }
		public PokemonMoveset(int level, Move.MoveData.Move move)
		{
			this.Level = level;
			this.MoveId = move;
		}
	}
	/// <summary>
	/// The moves that all Pokémon of the species learn as they level up. 
	/// </summary>
	public class PokemonMoveTree
	{
		/// <summary>
		/// Level at which the move is learned 
		/// (0 means the move can only be learned when 
		/// a Pokémon evolves into this species).
		/// </summary>
		/// Level needed to learn Move
		public int Level;
		/// <summary>
		/// Move learned upon leveling-up
		/// </summary>
		public Move.MoveData.Move MoveId;
	}
	/// <summary>
	/// The evolution paths this species can take. 
	/// For each possible evolution of this species, 
	/// there are three parts
	/// </summary>
	public class PokemonEvolution
	{
		/// <summary>
		/// no parameter,
		/// Positive integer,
		/// Item  = <see cref="eItems.Item"/>,
		/// Move = <see cref="Move.MoveData.Move"/>,
		/// Species = <see cref="PokemonData.Pokemon"/>,
		/// Type = <see cref="PokemonData.Type"/>
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
			///	if time is between 9PM and 4AM time is "Night". else time is "Day".
			///	if time is equal to string dayNight (either Day, or Night).
			///	<code>Time,DatetimeOffset/bool dayNight</code>
			/// </summary>
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
			Shedinja
		}
		/// <summary>
		/// The PokemonId of the evolved species.
		/// The PokemonId of the species this pokemon evolves into.
		/// </summary>
		public PokemonData.Pokemon Species;
		/// <summary>
		/// The evolution method.
		/// </summary>
		public EvolutionMethod EvolveMethod;
		
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
		public T EvolveValue;
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
	}
	/// <summary>
	/// </summary>
	/// Experience can be it's own class away from Pokemon. But there's no need for it to be global.
	/// ToDo: Consider making Experience class a Pokemon extension class...
	private static class Experience
	{
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

		private static int GetExperience(Pokemon.PokemonData.LevelingRate levelingRate, int currentLevel)
		{
			int exp = 0;
			//if (currentLevel > 100) currentLevel = 100; 
			//if (currentLevel > Settings.MAXIMUMLEVEL) currentLevel = Settings.MAXIMUMLEVEL; 
			if (levelingRate == PokemonData.LevelingRate.ERRATIC)
			{
				if (currentLevel > 100)
				{
					//exp = (int)System.Math.Floor((System.Math.Pow(currentLevel, 3)) * (160 - currentLevel) / 100);
					exp = (int)System.Math.Floor((System.Math.Pow(currentLevel, 3)) * (currentLevel * 6 / 10) / 100);
				}
				else exp = expTableErratic[currentLevel - 1]; //Because the array starts at 0, not 1.
			}
			else if (levelingRate == PokemonData.LevelingRate.FAST)
			{
				if (currentLevel > 100)
				{
					exp = (int)System.Math.Floor(System.Math.Pow(currentLevel, 3) * (4 / 5));
				}
				else exp = expTableFast[currentLevel - 1];
			}
			else if (levelingRate == PokemonData.LevelingRate.MEDIUMFAST)
			{
				if (currentLevel > 100)
				{
					exp = (int)System.Math.Floor(System.Math.Pow(currentLevel, 3));
				}
				else exp = expTableMediumFast[currentLevel - 1];
			}
			else if (levelingRate == PokemonData.LevelingRate.MEDIUMSLOW)
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
			else if (levelingRate == PokemonData.LevelingRate.SLOW)
			{
				if (currentLevel > 100)
				{
					exp = (int)System.Math.Floor(System.Math.Pow(currentLevel, 3) * (5 / 4));
				}
				else exp = expTableSlow[currentLevel - 1];
			}
			else if (levelingRate == PokemonData.LevelingRate.FLUCTUATING)
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
		public static int GetMaxExperience(Pokemon.PokemonData.LevelingRate levelingRate)
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
		public static int GetStartExperience(Pokemon.PokemonData.LevelingRate levelingRate, int currentLevel)
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
		public static int AddExperience(Pokemon.PokemonData.LevelingRate levelingRate, int currentExperience, int experienceGain)
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
		public static int GetLevelFromExperience(Pokemon.PokemonData.LevelingRate levelingRate, int experiencePoints)
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
	}
	#endregion
}