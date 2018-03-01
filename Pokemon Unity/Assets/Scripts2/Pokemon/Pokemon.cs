using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    /// Personal ID
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
    private int status;
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
    private readonly eMoves.Move[] moves = new eMoves.Move[4]; //{ eMoves.Move.NONE, eMoves.Move.NONE, eMoves.Move.NONE, eMoves.Move.NONE };
    /// <summary>
    /// The moves known when this Pokemon was obtained
    /// </summary>
    private readonly eMoves.Move[] firstMoves = new eMoves.Move[4];
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
    private readonly eAbility.Ability[] abilityFlag = new eAbility.Ability[3];
    /// <summary>
    /// </summary>
    /// <remarks>
    /// isMale; null = genderless?
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
    private bool shinyFlag;
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
    public Pokemon(PokemonData.Pokemon pokemon) //: base(pokemon)
    { _base = PokemonData.GetPokemon(pokemon); } //ToDo: Redo PokemonDatabase/PokemonData -- DONE

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
    #endregion

    #region Ability
    #endregion

    #region Nature
    #endregion

    #region Shininess
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
    #endregion

    #region Moves
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
        status = 0; statusCount = 0; //remove status ailments
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
}