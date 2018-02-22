using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    class Pokemon
{
    #region Variables
    /// <summary>
    /// Current Total HP
    /// </summary>
    public int TotalHP; 
    /// <summary>
    /// Current HP
    /// </summary>
    public int HP;
    /// <summary>
    /// Current Attack Stat
    /// </summary>
    public int ATK;
    /// <summary>
    /// Current Defense stat
    /// </summary>
    public int DEF;
    /// <summary>
    /// Current Special Attack Stat
    /// </summary>
    public int SPA;
    /// <summary>
    /// Current Special Defense Stat
    /// </summary>
    public int SPD;
    /// <summary>
    /// Current Speed Stat
    /// </summary>
    public int SPE;
    /// <summary>
    /// Array of 6 Individual Values for HP, Atk, Def, Speed, Sp Atk, and Sp Def
    /// </summary>
    public int[] IV = new int[6];
    /// <summary>
    /// Effort Values
    /// </summary>
    public int EV;
    /// <summary>
    /// Species (National Pokedex number)
    /// </summary>
    public int Species;
    /// <summary>
    /// Personal ID
    /// </summary>
    public int PersonalId;
    /// <summary>
    /// 32-bit Trainer ID (the secret ID is in the upper 16-bits)
    /// </summary>
    public int TrainerId;
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
    public int Pokerus;
    /// <summary>
    /// Held item
    /// </summary>
    public eItems.Item Item;
    /// <summary>
    /// Consumed held item (used in battle only)
    /// </summary>
    public bool ItemRecycle;
    /// <summary>
    /// Resulting held item (used in battle only)
    /// </summary>
    public bool ItemInitial;
    /// <summary>
    /// Where Pokemon can use Belch (used in battle only)
    /// </summary>
    public bool Belch;
    /// <summary>
    /// Mail?...
    /// </summary>
    public bool Mail;
    /// <summary>
    /// The pokemon fused into this one.
    /// </summary>
    public int Fused;
    /// <summary>
    /// Nickname
    /// </summary>
    public string Name;
    /// <summary>
    /// Current experience points
    /// </summary>
    public int Exp;
    /// <summary>
    /// Current happiness
    /// </summary>
    public int Happiness;
    /// <summary>
    /// Status problem (PBStatuses)
    /// </summary>
    public int Status;
    /// <summary>
    /// Sleep count/Toxic flag
    /// </summary>
    public int StatusCount;
    /// <summary>
    /// Steps to hatch egg, 0 if Pokemon is not an egg
    /// </summary>
    public int EggSteps;
    /// <summary>
    /// Moves (PBMove)
    /// </summary>
    public eMoves.Move[] Moves = new eMoves.Move[] { eMoves.Move.NONE, eMoves.Move.NONE, eMoves.Move.NONE, eMoves.Move.NONE };
    /// <summary>
    /// The moves known when this Pokemon was obtained
    /// </summary>
    public eMoves.Move[] FirstMoves;
    /// <summary>
    /// Ball used
    /// </summary>
    public eItems.Item BallUsed;
    /// <summary>
    /// Markings
    /// </summary>
    public bool[] Markings;
    /// <summary>
    /// Manner Obtained:
    /// </summary>
    public ObtainedMethod ObtainedMode;
    public enum ObtainedMethod
    {
        MET = 0,
        EGG = 1,
        TRADED = 2,
        /// <summary>
        /// NPC?
        /// </summary>
        FATEFUL_ENCOUNTER = 4
    }
    /// <summary>
    /// Map where obtained
    /// </summary>
    /// <remarks>
    /// Doubles as "HatchedMap"
    /// </remarks>
    public int ObtainMap;
    /// <summary>
    /// Replaces the obtain map's name if not null
    /// </summary>
    public string ObtainString;
    public int ObtainLevel;
    /// <summary>
    /// Original Trainer's Name
    /// </summary>
    /// <remarks>
    /// ToDo: PlayerTrainer Class here
    /// </remarks>
    public int OT;
    /// <summary>
    /// Forces the first/second/hidden (0/1/2) ability
    /// </summary>
    public eAbility.Ability[] AbilityFlag;
    /// <summary>
    /// </summary>
    /// <remarks>
    /// isMale; null = genderless?
    /// </remarks>
    public bool GenderFlag;
    /// <summary>
    /// Forces a particular nature
    /// </summary>
    /// ToDo: enum
    public int NatureFlag;
    /// <summary>
    /// Forces the shininess
    /// </summary>
    public bool ShinyFlag;
    /// <summary>
    /// Array of ribbons
    /// </summary>
    /// <remarks>
    /// Make 2d Array (Array[,]) separated by regions/Gens
    /// </remarks>
    public bool[] Ribbons;
    /// <summary>
    /// Contest stats
    /// </summary>
    public int Cool, Beauty, Cute, Smart, Tough, Sheen;
    //readonly const int EVLIMIT = 510; //Max total EVs
    //readonly const int EVSTATLIMIT = 252; //Max EVs that a single stat can have
    //readonly const int NAMELIMIT = 10; // Maximum length a Pokemon's nickname can be
    #endregion

    #region Ownership, obtained information
    #endregion

}