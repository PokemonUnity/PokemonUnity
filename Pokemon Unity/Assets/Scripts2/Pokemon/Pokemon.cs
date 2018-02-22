using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    class Pokemon //: ePokemons //PokemonData
{
    #region Variables
    /// <summary>
    /// Current Total HP
    /// </summary>
    private int TotalHP; 
    /// <summary>
    /// Current HP
    /// </summary>
    private int HP;
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
    private int[] IV = new int[6];
    /// <summary>
    /// Effort Values
    /// </summary>
    private int[] EV = new int[6];
    /// <summary>
    /// Species (National Pokedex number)
    /// </summary>
    private int Species;
    /// <summary>
    /// Personal ID
    /// </summary>
    private int PersonalId;
    /// <summary>
    /// 32-bit Trainer ID (the secret ID is in the upper 16-bits);
    /// Deprecated
    /// </summary>
    /// ToDo: Remove this, and fetch from Trainer Class?
    private int TrainerId;
    /// <summary>
    /// Pokerus strain and infection time
    /// </summary>
    /// <remarks>
    /// ToDo: Custom class?
    /// 3 Values; Not Infected, Cured, Infected.
    /// Can be a enum, for 1 of 3 values
    /// or if null, not infected; 
    /// true infected, and false cured?
    /// </remarks>
    private int Pokerus;
    /// <summary>
    /// Held item
    /// </summary>
    private eItems.Item Item;
    /// <summary>
    /// Consumed held item (used in battle only)
    /// </summary>
    private bool ItemRecycle;
    /// <summary>
    /// Resulting held item (used in battle only)
    /// </summary>
    private bool ItemInitial;
    /// <summary>
    /// Where Pokemon can use Belch (used in battle only)
    /// </summary>
    private bool Belch;
    /// <summary>
    /// Mail?...
    /// </summary>
    private bool Mail;
    /// <summary>
    /// The pokemon fused into this one.
    /// </summary>
    private int Fused;
    /// <summary>
    /// Nickname
    /// </summary>
    private string Name;
    /// <summary>
    /// Current experience points
    /// </summary>
    private int Exp;
    /// <summary>
    /// Current happiness
    /// </summary>
    private int Happiness;
    /// <summary>
    /// Status problem (PBStatuses)
    /// </summary>
    /// ToDo: Status Class
    private int Status;
    /// <summary>
    /// Sleep count/Toxic flag
    /// </summary>
    /// ToDo: Add to Status Class
    private int StatusCount;
    /// <summary>
    /// Steps to hatch egg, 0 if Pokemon is not an egg
    /// </summary>
    private int EggSteps;
    /// <summary>
    /// Moves (PBMove)
    /// </summary>
    /// ToDo Move class, not enum
    private eMoves.Move[] Moves = new eMoves.Move[] { eMoves.Move.NONE, eMoves.Move.NONE, eMoves.Move.NONE, eMoves.Move.NONE };
    /// <summary>
    /// The moves known when this Pokemon was obtained
    /// </summary>
    private eMoves.Move[] FirstMoves = new eMoves.Move[4];
    /// <summary>
    /// Ball used
    /// </summary>
    private eItems.Item BallUsed;
    /// <summary>
    /// Markings
    /// </summary>
    private bool[] Markings;
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
    private int ObtainMap;
    /// <summary>
    /// Replaces the obtain map's name if not null
    /// </summary>
    private string ObtainString;
    /// <remarks>
    /// Wouldnt this change again when traded to another trainer?
    /// </remarks>
    private int obtainLevel = 0;
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
    private eAbility.Ability[] AbilityFlag;
    /// <summary>
    /// </summary>
    /// <remarks>
    /// isMale; null = genderless?
    /// </remarks>
    private bool GenderFlag;
    /// <summary>
    /// Forces a particular nature
    /// </summary>
    /// ToDo: Redo NatureDatabase Class
    private NatureDatabase.Nature NatureFlag;
    /// <summary>
    /// Forces the shininess
    /// </summary>
    private bool ShinyFlag;
    /// <summary>
    /// Array of ribbons
    /// </summary>
    /// <remarks>
    /// Make 2d Array (Array[,]) separated by regions/Gens
    /// </remarks>
    private bool[] Ribbons;
    /// <summary>
    /// Contest stats
    /// </summary>
    private int Cool, Beauty, Cute, Smart, Tough, Sheen;
    /// <summary>
    /// Max total EVs
    /// </summary>
    static readonly int EVLIMIT = 510; 
    /// <summary>
    /// Max EVs that a single stat can have
    /// </summary>
    /// ToDo: use const instead?
    /// Can be referenced as [Attribute] if is a const value
    static readonly int EVSTATLIMIT = 252; 
    /// <summary>
    /// Maximum length a Pokemon's nickname can be
    /// </summary>
    static readonly int NAMELIMIT = 10;
    #endregion

    Pokemon() { }

    /// <summary>
    /// Uses PokemonData to initialize a Pokemon from base stats
    /// </summary>
    /// <param name="pokemon"></param>
    /// ToDo: Inherit PokemonData 
    Pokemon(ePokemons.Pokemon pokemon) //: base(pokemon)
    { }

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
    public string PublicId()
    {
        //return TrainerId.ToString();
        return OT.ToString(); //ToDo: TrainerId fix here
    }

    /// <summary>
    /// Returns this Pokemon's level when this Pokemon was obtained
    /// </summary>
    /// <returns></returns>
    public int ObtainLevel()
    {
        //int level = 0; int.TryParse(this.obtainLevel, out level);
        return this.obtainLevel;
    }

    /// <summary>
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
    }

    /// <summary>
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
    }
    #endregion

}