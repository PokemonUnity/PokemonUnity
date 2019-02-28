using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Pokemon;
using PokemonUnity.Attack;
using PokemonUnity.Item;

namespace PokemonUnity.Pokemon
{
    public partial class Pokemon
    {
        #region Variables
        /// <summary>
        /// Current Total HP
        /// </summary>
        public int TotalHP
        {
            get
            {
                if (_base.BaseStatsHP == 1) return 1;
                return //totalHP;
                    ((2 * _base.BaseStatsHP + IV[0] + (EV[0] / 4)) * Level) / 100 + Level + 10;
            }
        }
        /// <summary>
        /// Current HP
        /// </summary>
        private int hp { get; set; }
        /// <summary>
        /// Current Attack Stat
        /// </summary>
        public virtual int ATK
        {
            get
            {
                return (int)Math.Floor((((2 * _base.BaseStatsATK + IV[1] + (EV[1] / 4)) * Level) / 100 + 5) * natureFlag.ATK);
            }
        }
        /// <summary>
        /// Current Defense stat
        /// </summary>
        public virtual int DEF
        {
            get
            {
                return (int)Math.Floor((((2 * _base.BaseStatsDEF + IV[2] + (EV[2] / 4)) * Level) / 100 + 5) * natureFlag.DEF);
            }
        }
        /// <summary>
        /// Current Special Attack Stat
        /// </summary>
        public virtual int SPA
        {
            get
            {
                return (int)Math.Floor((((2 * _base.BaseStatsSPA + IV[4] + (EV[4] / 4)) * Level) / 100 + 5) * natureFlag.SPA);
            }
        }
        /// <summary>
        /// Current Special Defense Stat
        /// </summary>
        public virtual int SPD
        {
            get
            {
                return (int)Math.Floor((((2 * _base.BaseStatsSPD + IV[5] + (EV[5] / 4)) * Level) / 100 + 5) * natureFlag.SPD);
            }
        }
        /// <summary>
        /// Current Speed Stat
        /// </summary>
        public virtual int SPE
        {
            get
            {
                return (int)Math.Floor((((2 * _base.BaseStatsSPE + IV[3] + (EV[3] / 4)) * Level) / 100 + 5) * natureFlag.SPE);
            }
        }
        /// <summary>
        /// Array of 6 Individual Values for HP, Atk, Def, Speed, Sp Atk, and Sp Def
        /// </summary>
        public byte[] IV { get; private set; }
        /// <summary>
        /// Effort Values
        /// </summary>
        /// <see cref="EVSTATLIMIT"/>
        /// new int[6] = { 0, 0, 0, 0, 0, 0 }; //same thing
        public byte[] EV { get; private set; }
        /// <summary>
        /// Species (National Pokedex number)
        /// </summary>
        public Pokemons Species { get { return _base.ID; } }
        /// <summary>
        /// Held item
        /// </summary>
        public Items Item { get; private set; }
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
        public int StatusCount { get; protected set; }
        /// <summary>
        /// Steps to hatch egg, 0 if Pokemon is not an egg.
        /// Pokemon moves auto reset when egg counter reaches 0.
        /// </summary>
        /// ToDo: Sequence of events to occur when egg is hatching (roll dice for ability, moves, and animate hatching)
        public int EggSteps
        {
            get
            {
                return eggSteps;
            }
            private set
            {
                if (eggSteps > 0 && value == 0)
                {
                    this.Level = Settings.EGGINITIALLEVEL;
                    this.GenerateMoveset();
                }
                eggSteps =
                    //if egg hatch counter is going up in positive count
                    //eggSteps + 
                    value > eggSteps
                    ? //return the same value
                        eggSteps
                    : //else return new value
                        value;
            }
        }
        private int eggSteps { get; set; }
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
        //public int[] EvolveLevels { get { return _base.Evolutions.} }
        public IPokemonEvolution[] Evolutions { get { return _base.Evolutions; } }
        protected PokemonData _base { get; private set; }
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
            PersonalId = Settings.Rand.Next(256);
            PersonalId |= Settings.Rand.Next(256) << 8;
            PersonalId |= Settings.Rand.Next(256) << 16;
            PersonalId |= Settings.Rand.Next(256) << 24;
            Ability = Abilities.NONE;
            natureFlag = new Nature();//(Natures)(Settings.Rand.Next(0, 24));
			//ToDo: Maybe add TrainerId = <int> here, before isShiny()?
			//shinyFlag = isShiny(); ToDo: Fix WildPokemon.TrainerId
			//Gender = isMale();
			//IV = new int[] { 10, 10, 10, 10, 10, 10 };
            IV = new byte[] { (byte)Settings.Rand.Next(32), (byte)Settings.Rand.Next(32), (byte)Settings.Rand.Next(32), (byte)Settings.Rand.Next(32), (byte)Settings.Rand.Next(32), (byte)Settings.Rand.Next(32) };
            EV = new byte[6];
            Exp = new Experience(GrowthRate);
            moves = new Move[4] { new Move(Moves.NONE), new Move(Moves.NONE), new Move(Moves.NONE), new Move(Moves.NONE) };
            pokerus = new int[2];
            Markings = new bool[6]; //{ false, false, false, false, false, false };
            Status = Status.NONE;
            StatusCount = 0;
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
            Exp = new Experience(GrowthRate);
            eggSteps = _base.HatchTime;
            Ability = abilityFlag;
            Gender = gender; //GenderRatio.//Pokemon.PokemonData.GetPokemon(pokemon).MaleRatio
            GenerateMoveset();

            //calcStats();
        }

        public Pokemon(Pokemons pkmn, bool isEgg) : this(pkmn) { if (!isEgg) EggSteps = 0; }

        public Pokemon(Pokemons pkmn, byte level, bool isEgg = false) : this(pkmn, isEgg) { Level = level; } //Exp.AddExperience(Experience.GetStartExperience(GrowthRate, level));

        //public Pokemon(Pokemons pkmn, byte loLevel, byte hiLevel, bool isEgg = false) : this(pkmn, isEgg) {  }

        public Pokemon(Pokemons TPSPECIES = Pokemons.NONE,
            byte TPLEVEL = 10,
            Items TPITEM = Items.NONE,
            Moves TPMOVE1 = Moves.NONE,
            Moves TPMOVE2 = Moves.NONE,
            Moves TPMOVE3 = Moves.NONE,
            Moves TPMOVE4 = Moves.NONE,
            Abilities TPABILITY = Abilities.NONE,
            int? TPGENDER = null,
            int TPFORM = 0,
            bool TPSHINY = false,
            Natures TPNATURE = Natures.UNSET,
            byte[] TPIV = null, //new int[6] { 10, 10, 10, 10, 10, 10 },
            int TPHAPPINESS = 70,
            string TPNAME = null,
            bool TPSHADOW = false,
            //bool EGG = false,
            Items TPBALL = Items.NONE) : this(TPSPECIES, level: TPLEVEL)
        {
            //Random rand = new Random(Settings.Seed());//(int)TPSPECIES+TPLEVEL
            IV = TPIV ?? IV;
            //EV = new int[6];

            //calcStats();
        }

        public Pokemon(string nickName, int form,
            Pokemons species, Abilities ability,
            Natures nature,
            bool isShiny, bool? gender,
            int[] pokerus, bool ishyper,
            int? shadowLevel,
            int currentHp, Items item,
            byte[] iv, byte[] ev, 
            int obtainedLevel, /*int currentLevel,*/ int currentExp,
            int happiness, Status status, int statusCount,
            int eggSteps, Items ballUsed,
            string mail, Move[] moves,
            Ribbon[] ribbons, bool[] markings,
            int personalId,
            ObtainedMethod obtainedMethod,
            DateTimeOffset timeReceived, DateTimeOffset? timeEggHatched) : this(species)
        {
            //Check to see if nickName is filled
            if (nickName != null || nickName != string.Empty)
            {
                name = nickName;
            }
            else
            {
                name = null;
            }

            Form = form;
            //_base = Pokemon.PokemonData.GetPokemon(species);

            Ability = ability;
            natureFlag = new Nature(nature);

            shinyFlag = isShiny;
            Gender = gender;

            this.pokerus = pokerus;

            isHyperMode = ishyper;
            ShadowLevel = shadowLevel;

            HP = currentHp;
            Item = item;

            IV = iv;
            EV = ev;

            ObtainLevel = obtainedLevel;
            //Level = currentLevel;
            Exp.AddExperience(currentExp);

            Happiness = happiness;

            Status = status;
            StatusCount = statusCount;

            EggSteps = eggSteps;

            this.ballUsed = ballUsed;
            if (PokemonUnity.Item.Item.Mail.IsMail(item))
            {
                this.mail = new Item.Item.Mail((Items)item);
                this.mail.Message = mail;
            }

            this.moves = moves;

            this.ribbons = ribbons.ToList();
            Markings = markings;

            PersonalId = personalId;

            ObtainedMode = obtainedMethod;
            obtainWhen = timeReceived;
            hatchedWhen = timeEggHatched;
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
        private System.DateTimeOffset? hatchedWhen { get; set; }
        /// <summary>
        /// Original Trainer's Name
        /// </summary>
        /// <remarks>
        /// ToDo: PlayerTrainer's hash value instead of class; maybe GUID?
        /// </remarks>
        private Trainer OT { get; set; }
        /// <summary>
        /// Personal/Pokemon ID
        /// </summary>
        /// ToDo: String value?
        public int PersonalId { get; private set; }

        /// <summary>
        /// Returns whether or not the specified Trainer is NOT this Pokemon's original trainer
        /// </summary>
        /// <param name="trainer"></param>
        /// <returns></returns>
        public bool isOutsider(Player trainer)
        {
            return trainer.Trainer != this.OT; 
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
            //set { this.obtainWhen = value; }
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
        /// Sets or Returns the time when this Pokemon hatched.
		/// If trainer did not hatch this pokemon, age will remain unknown.
        /// </summary>
        public DateTimeOffset? TimeEggHatched
        {
            get
            {
				if (this.ObtainedMode == ObtainedMethod.EGG)
				{
					//if (hatchedWhen == null) this.hatchedWhen = DateTimeOffset.UtcNow;
					return this.hatchedWhen;
				}
				else
					//return DateTimeOffset.UtcNow; //ToDo: Something else? Maybe error?
					//throw new Exception("Trainer did not acquire Pokemon as an egg."); //No Exceptions...
					return null;
            }
            //set { this.hatchedWhen = value; }
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
            private set
            {
                if (value < 1 || value > 100) //Experience.MAXLEVEL
                    GameVariables.DebugLog(string.Format("The level number {0} is invalid", value), true);
                if (value > this.Level)
                    this.Exp.AddExperience(Experience.GetStartExperience(this.GrowthRate, value) - this.Exp.Current);
                else
                {
                    //ToDo: Not Designed to go backwards yet...
                    GameVariables.DebugLog(string.Format("The level number {0} is invalid", value), true);
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

        /// <summary>
        /// When this pokemon is defeated, this is the amount of experience points it offers
        /// </summary>
        public int baseExp
        {
            get
            {
                return _base.BaseExpYield; //ToDo: 
            }
        }

        public void AddSteps(byte steps = 1)
        {
            int i = EggSteps - steps;
            //Set EggSteps to 0 first, to trigger the hatching process... 
            EggSteps = i < 0 ? 0 : i;
            //then if we want to continue beyond 0 and into negative values, we may continue...
            if (i < 0) EggSteps = i;
        }

        public void HatchEgg()
        {
            if (!isEgg) return;
            //bring value all the way down to 1, regardless of where it's set
            EggSteps -= EggSteps > 1 ? EggSteps - 1 : 0;
            //take final step on counter to trigger all the RNGs
            AddSteps();
        }
        #endregion

        #region Evolution
        /// <summary>
        /// Returns an array of all the levels this pokemons has to reach in order to evolve into species.
        /// Best if used in combination with <see cref="CanEvolveDuringBattle"/>.
        /// </summary>
        /// <returns>null means no evolves for pokemon, int[].Count == 0 means evolutions are not specific to leveling</returns>
        public int[] GetEvolutionLevels()
        {
            if (_base.Evolutions.Length > 0)
            {
                List<int> levels = new List<int>();
                foreach (IPokemonEvolution evolution in _base.Evolutions)
                {
                    if (evolution.EvolveMethod == EvolutionMethod.Level ||
                        evolution.EvolveMethod == EvolutionMethod.LevelMale ||
                        evolution.EvolveMethod == EvolutionMethod.LevelFemale)
                    {
                        levels.Add((evolution as Pokemon.PokemonData.PokemonEvolution<int>).EvolveValue);
                    }
                }
                if (levels.Count == 0)// && _base.Evolutions.Length > 0
                    return new int[0];
                else
                    return levels.ToArray();
            }
            else
                return null;
        }
        /// <summary>
        /// Returns true or false if evolution occurs during fight or after pokemon survives.
        /// </summary>
        public bool CanEvolveDuringBattle()
        {
            foreach (IPokemonEvolution item in _base.Evolutions)
            {
                switch (item.EvolveMethod)
                {
                    case EvolutionMethod.Level:
                    case EvolutionMethod.LevelMale:
                    case EvolutionMethod.LevelFemale:
                    case EvolutionMethod.Beauty:
                    case EvolutionMethod.Move:
                    case EvolutionMethod.AttackGreater:
                    case EvolutionMethod.DefenseGreater:
                    case EvolutionMethod.AtkDefEqual:
                    case EvolutionMethod.Ninjask:
                    case EvolutionMethod.Shedinja:
                    case EvolutionMethod.Party:
                    case EvolutionMethod.HoldItem:
                    case EvolutionMethod.Shiny:
                    case EvolutionMethod.Hatred:
                    case EvolutionMethod.Happiness:
                    case EvolutionMethod.Silcoon:
                    case EvolutionMethod.Cascoon:
                        return true;
                    case EvolutionMethod.Item:
                    case EvolutionMethod.ItemMale:
                    case EvolutionMethod.ItemFemale:
                    case EvolutionMethod.Trade:
                    case EvolutionMethod.TradeItem:
                    case EvolutionMethod.TradeSpecies:
                    case EvolutionMethod.HappinessDay:
                    case EvolutionMethod.HappinessNight:
                    case EvolutionMethod.Time:
                    case EvolutionMethod.Season:
                    case EvolutionMethod.HoldItemDay:
                    case EvolutionMethod.HoldItemNight:
                    case EvolutionMethod.Type:
                    case EvolutionMethod.Location:
                    case EvolutionMethod.Weather:
                    case EvolutionMethod.Deaths:
                    default:
                        break;
                }
            }
            return false;
        }
        public EvolutionMethod[] GetEvolutionMethods()
        {
            List<EvolutionMethod> methods = new List<EvolutionMethod>();
            foreach (IPokemonEvolution item in _base.Evolutions)
            {
                if (!methods.Contains(item.EvolveMethod))
                    methods.Add(item.EvolveMethod);
            }
            return methods.ToArray();
        }
        public bool hasEvolveMethod(EvolutionMethod method)
        {
            foreach (IPokemonEvolution item in _base.Evolutions)
            {
                if (item.EvolveMethod == method)
                    return true;
            }
            return false;
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

        private bool? getGender()
        {
            switch (_base.MaleRatio)
            {
                //case GenderRatio.FemaleOneEighth:
                //	break;
                //case GenderRatio.Female25Percent:
                //	break;
                //case GenderRatio.Female50Percent:
                //	break;
                //case GenderRatio.Female75Percent:
                //	break;
                //case GenderRatio.FemaleSevenEighths:
                //	break;
                case GenderRatio.AlwaysMale:
                    return true;
                case GenderRatio.AlwaysFemale:
                    return false;
                case GenderRatio.Genderless:
                    return null;
                default:
                    //byte n = (byte)(Settings.Rand.Next(0, 100) + 1);
                    double n = (Settings.Rand.NextDouble() * 100) + 1;
                    if (_base.MaleRatio == GenderRatio.AlwaysFemale && n > 0f && n < 12.5f) return false;
                    else if (_base.MaleRatio == GenderRatio.FemaleSevenEighths && n >= 12.5f && n < 25f) return false;
                    else if (_base.MaleRatio == GenderRatio.Female75Percent && n >= 25f && n < 37.5f) return false;
                    else if (_base.MaleRatio == GenderRatio.Female75Percent && n >= 37.5f && n < 50f) return false;
                    else if (_base.MaleRatio == GenderRatio.Female50Percent && n >= 50f && n < 62.5f) return false;
                    else if (_base.MaleRatio == GenderRatio.Female50Percent && n >= 62.5f && n < 75f) return false;
                    else if (_base.MaleRatio == GenderRatio.Female25Percent && n >= 75f && n < 87.5f) return false;
                    else if (_base.MaleRatio == GenderRatio.FemaleOneEighth && n >= 87.5f && n < 100f) return false;
                    else return true;
            }
        }
        #endregion

        #region Ability
        /// <summary>
        /// RNG forces the first/second/hidden (0/1/2) ability
        /// </summary>
        private Abilities abilityFlag
        {
            get
            {

                if (isEgg && hasHiddenAbility())
                {
                    if (_base.Ability[1] != Abilities.NONE)
                    {
                        return _base.Ability[Settings.Rand.Next(0, 3)];                            
                    }
                    else
                    {
                        return _base.Ability[Settings.Rand.Next(0, 2) == 1 ? 2 : 0];
                    }
                }
                else
                {
                    return _base.Ability[Settings.Rand.Next(0, 2)];
                }
                
            }
        }
        /// <summary>
        /// Returns the ID of the Pokemons Ability.
        /// </summary>
        /// ToDo: Sets this Pokemon's ability to a particular ability (if possible)
        /// ToDo: Ability 1 or 2, never both...
        /// ToDo: Error on non-compatible ability?
        public Abilities Ability { get; set; }//{ get { return abilityFlag; } set { abilityFlag = value; } }//ToDo: Check against getAbilityList()?

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

        ///// <summary>
        ///// Returns the Nature for the SeriPokemon class to serialize Nature
        ///// </summary>
        ///// <returns></returns>
		///// Could've just made the natureFlag public if you needed access to it...
        //public Nature getNature()
        //{
        //    return natureFlag;
        //}
        #endregion

        #region Shininess
        /// <summary>
        /// Returns whether this Pokemon is shiny (differently colored)
        /// </summary>
        public virtual bool IsShiny
        { 
			//Uses math to determine if Pokemon is shiny.
            get
			{
				//return shinyFlag ?? isShiny();
				if (shinyFlag.HasValue) return shinyFlag.Value;
				// Use this when rolling for shiny...
				// Honestly, without this math, i probably would've done something a lot more primative.
				// Look forward to primative math on wild pokemon encounter chances...
				int a = this.PersonalId ^ int.Parse(this.OT.PlayerID);//this.TrainerId; //Wild Pokemon TrainerId?
				int b = a & 0xFFFF;
				int c = (a >> 16) & 0xFFFF;
				int d = b ^ c;
				shinyFlag = d < _base.ShinyChance;
				return shinyFlag.Value;
			}
            //set { shinyFlag = value; }
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
            if (strain <= 0 || strain >= 16) strain = Settings.Rand.Next(1, 16);
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
        public byte countMoves()
        {
            byte ret = 0;
            for (byte i = 0; i < 4; i++)
            {//foreach(var move in this.moves){ 
                if ((int)this.moves[i].MoveId != 0) ret += 1;//move.id
            }
            return ret;
        }

        /// <summary>
        /// Returns true if the Pokémon knows the given move.
        /// </summary>
        public bool hasMove(Moves move)
        {
            //Checking if pokemon has a NONE placeholder in moveset might come in handy
            //if (move == Moves.NONE || move <= 0) return false;
            for (int i = 0; i < 4; i++)
            {
                if (this.moves[i].MoveId == move) return true;
            }
            return false;
        }

        public bool knowsMove(Moves move) { return this.hasMove(move); }

        /// <summary>
        /// Returns the list of moves this Pokémon can learn by training method.
        /// </summary>
        public Moves[] getMoveList(LearnMethod? method = null)
        {
            switch (method)
            {
                case LearnMethod.egg:
                    return _base.MoveTree.Egg;
                case LearnMethod.levelup:
                    return _base.MoveTree.LevelUp.Where(x => x.Value <= this.Level).Select(x => x.Key).ToArray();
                case LearnMethod.machine:
                    return _base.MoveTree.Machine;
                case LearnMethod.tutor:
                    return _base.MoveTree.Tutor;
                case LearnMethod.shadow:
                case LearnMethod.xd_shadow:
                    List<Moves> s = new List<Moves>();
                    s.AddRange(_base.MoveTree.Shadow);
                    s.AddRange(_base.MoveTree.Shadow.Where(x => !s.Contains(x)).Select(x => x));
                    return s.ToArray();
                default:
                    List<Moves> list = new List<Moves>();
                    list.AddRange(_base.MoveTree.Egg);
                    list.AddRange(_base.MoveTree.Machine.Where(x => !list.Contains(x)).Select(x => x));
                    list.AddRange(_base.MoveTree.Tutor.Where(x => !list.Contains(x)).Select(x => x));
                    list.AddRange(_base.MoveTree.LevelUp.Where(x => !list.Contains(x.Key))/*(x => x.Value <= this.Level)*/.Select(x => x.Key));
                    return list.ToArray();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// ToDo: Higher the pokemon's level, the greater the chances of generating a full moveset (4 moves)
        public void GenerateMoveset(int? level = null)
        {
            if (level.HasValue && level.Value < 0)
                return;
            //if (!level.HasValue)
            //	level = -1;
            ClearFirstMoves();
            int numMove = Settings.Rand.Next(4); //number of moves pokemon will have, between 0 and 3
            List<Moves> movelist = new List<Moves>();
            if (isEgg || Settings.CatchPokemonsWithEggMoves) movelist.AddRange(_base.MoveTree.Egg);
            switch (level)
            {
                #region sample from alpha version
                //Level 0 is only there so i have a sample of how version.alpha would have handled code
                case 0:
                    //Set moveset based off of the highest level moves possible.
                    //string[] moves = new string[4];
                    int i = _base.MoveTree.LevelUp.Count - 1; //work backwards so that the highest level move possible is grabbed first
                    int[,] movesetLevels = new int[1, 2]; //[index,{moveId,level}]
                    while (moves[3] == null)
                    {
                        if (movesetLevels[i, 0] <= level.Value)
                        {
                            //moves[3] = movesetMovesStrings[i];
                        }
                        i -= 1;
                    }
                    if (i >= 0)
                    { //if i is at least 0 still, then you can grab the next move down.
                      //moves[2] = movesetMovesStrings[i];
                        i -= 1;
                        if (i >= 0)
                        { //if i is at least 0 still, then you can grab the next move down.
                          //moves[1] = movesetMovesStrings[i];
                            i -= 1;
                            if (i >= 0)
                            { //if i is at least 0 still, then you can grab the last move down.
                              //moves[0] = movesetMovesStrings[i];
                                i -= 1;
                            }
                        }
                    }
                    i = 0;
                    int i2 = 0;         //if the first move is null, then the array will need to be packed down
                    if (moves[0] == null)
                    {       //(nulls moved to the end of the array)
                        while (i < 4)
                        {
                            while (moves[i] == null)
                            {
                                i += 1;
                            }
                            moves[i2] = moves[i];
                            moves[i] = null;
                            i2 += 1;
                        }
                    }
                    //return moveset;
                    break;
                #endregion
                case null:
                    //case -1:
                    movelist.AddRange(_base.MoveTree.LevelUp.Where(x => x.Value <= this.Level).Select(x => x.Key));
                    for (int n = 0; n < movelist.Count; n++)
                    {
                        if (Convert.ToBoolean(Settings.Rand.Next(2)))
                        {
                            if (this.countMoves() < numMove + 1)
                            {
                                LearnMove(movelist[n]);
                            }
                            else
                                break;
                        }
                    }
                    break;
                default:
                    //if (isEgg || Settings.CatchPokemonsWithEggMoves) movelist.AddRange(_base.MoveTree.Egg);
                    movelist.AddRange(_base.MoveTree.LevelUp.Where(x => x.Value <= level.Value).Select(x => x.Key));
                    //int listend = movelist.Count - 4;
                    //listend = listend < 0 ? 0 : listend + 4;
                    //int j = 0; 
                    for (int n = 0; n < movelist.Count; n++)
                    {
                        if (Convert.ToBoolean(Settings.Rand.Next(2)))
                        {
                            if (this.countMoves() < numMove + 1) //j
                            {
                                //this.moves[j] = new Move(movelist[n]);
                                //j += 1;
                                LearnMove(movelist[n]);
                                //j += this.countMoves() < numMove ? 0 : 1;
                            }
                            else
                                break;
                        }
                    }
                    break;
            }
            RecordFirstMoves();
        }

        /// <summary>
        /// Sets this Pokémon's movelist to the default movelist it originally had.
        /// </summary>
        public void resetMoves()
        {
            /*for (int i = 0; i < _base.MoveSet.Length; i++){//foreach(var i in _base.MoveSet)
                if (_base.MoveSet[i].Level <= this.Level)
                {
                    movelist.Add(_base.MoveSet[i].MoveId);
                }
            }*/
            for (int i = 0; i < 4; i++)
            {
                this.moves[i] = new Move((i >= firstMoves.Count) ? 0 : firstMoves[i]);
            }
        }

        /// <summary>
        /// Silently learns the given move. Will erase the first known move if it has to.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public void LearnMove(Moves move, bool silently = false)
        {
            if ((int)move <= 0) return;
            if (!getMoveList().Contains(move))
            {
                GameVariables.DebugLog("Move is not compatible");
                return;
            }
            /*for (int i = 0; i < 4; i++) {
                if (moves[i].MoveId == move) { //Switch ordering of moves?
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
            }*/
            if (hasMove(move))
            {
                GameVariables.DebugLog("Already knows move...");
                return;
            }
            for (int i = 0; i < 4; i++)
            {
                if (moves[i].MoveId == 0)
                {
                    moves[i] = new Move(move);
                    return;
                }
            }
            if (!silently)
                GameVariables.DebugLog("Cannot learn move, pokmeon moveset is full", false);
            else
            {
                moves[0] = moves[1];
                moves[1] = moves[2];
                moves[2] = moves[3];
                moves[3] = new Move(move);
            }
        }

        /// <summary>
        /// Deletes the given move from the Pokémon.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public void DeleteMove(Moves move)
        {
            if (move <= 0) return;
            List<Move> newmoves = new List<Move>();
            for (int i = 0; i < 4; i++)
            {
                if (moves[i].MoveId != move) newmoves.Add(moves[i]);
            }

            newmoves.Add(new Move(0));
            for (int i = 0; i < 4; i++)
            {
                moves[i] = newmoves[i];
            }
        }

        /// <summary>
        /// Deletes the move at the given index from the Pokémon.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public void DeleteMoveAtIndex(int index)
        {
            List<Move> newmoves = new List<Move>();

            for (int i = 0; i < 4; i++)
            {
                if (i != index) newmoves.Add(moves[i]);
            }

            newmoves.Add(new Move(0));

            for (int i = 0; i < 4; i++)
            {
                moves[i] = newmoves[i];
            }
        }

        /// <summary>
        /// Deletes all moves from the Pokémon.
        /// </summary>
        public void DeleteAllMoves()
        {
            //moves = new Move.MoveData.Move[4];
            for (int i = 0; i < 4; i++)
            {
                moves[i] = new Move(0);
            }
        }

        /// <summary>
        /// Copies currently known moves into a separate array, for Move Relearner.
        /// </summary>
        public void RecordFirstMoves()
        {
            for (int i = 0; i < 4; i++)
            {
                if (moves[i].MoveId > 0) AddFirstMove(moves[i].MoveId);
            }
        }

        private void AddFirstMove(Moves move)
        {
            if (move > 0 && !firstMoves.Contains(move)) firstMoves.Add(move);
            return;
        }

        public void RemoveFirstMove(Moves move)
        {
            if (move > 0) firstMoves.Remove(move);
            return;
        }

        public void ClearFirstMoves()
        {
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
            for (int i = 0; i < ribbons.Count; i++)
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
        // ToDo: IsPurified, and "IsAbleToPurify" 
        public bool isShadow
        {
            get
            {
                if (!ShadowLevel.HasValue || ShadowLevel.Value == -1) return false;
                else return true;
            }
        }
        /// <summary>
        /// Heart Gauge.
        /// The Heart Gauge is split into five equal bars. When a Shadow Pokémon is first snagged, all five bars are full.
        /// </summary>
        /// If pokemon is purified, shadow level should be equal to -1
        /// If pokemon has never been shadowed, then value should be null
        /// HeartGuage max size should be determined by _base.database
        public int? ShadowLevel { get; private set; }
        //public int? HeartGuage { get { return ShadowLevel; } }

        void decreaseShadowLevel(pokemonActions Action)
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
                    if (this.Nature == Natures.ADAMANT) points = 150;
                    if (this.Nature == Natures.BASHFUL) points = 50;
                    if (this.Nature == Natures.BOLD) points = 150;
                    if (this.Nature == Natures.BRAVE) points = 200;
                    if (this.Nature == Natures.CALM) points = 50;
                    if (this.Nature == Natures.CAREFUL) points = 75;
                    if (this.Nature == Natures.DOCILE) points = 75;
                    if (this.Nature == Natures.GENTLE) points = 50;
                    if (this.Nature == Natures.HARDY) points = 150;
                    if (this.Nature == Natures.HASTY) points = 200;
                    if (this.Nature == Natures.IMPISH) points = 200;
                    if (this.Nature == Natures.JOLLY) points = 150;
                    if (this.Nature == Natures.LAX) points = 100;
                    if (this.Nature == Natures.LONELY) points = 50;
                    if (this.Nature == Natures.MILD) points = 75;
                    if (this.Nature == Natures.MODEST) points = 75;
                    if (this.Nature == Natures.NAIVE) points = 100;
                    if (this.Nature == Natures.NAUGHTY) points = 150;
                    if (this.Nature == Natures.QUIET) points = 100;
                    if (this.Nature == Natures.QUIRKY) points = 200;
                    if (this.Nature == Natures.RASH) points = 75;
                    if (this.Nature == Natures.RELAXED) points = 75;
                    if (this.Nature == Natures.SASSY) points = 200;
                    if (this.Nature == Natures.SERIOUS) points = 100;
                    if (this.Nature == Natures.TIMID) points = 50;
                    break;
                case pokemonActions.CallTo:
                    if (this.Nature == Natures.ADAMANT) points = 225;
                    if (this.Nature == Natures.BASHFUL) points = 300;
                    if (this.Nature == Natures.BOLD) points = 225;
                    if (this.Nature == Natures.BRAVE) points = 225;
                    if (this.Nature == Natures.CALM) points = 300;
                    if (this.Nature == Natures.CAREFUL) points = 300;
                    if (this.Nature == Natures.DOCILE) points = 600;
                    if (this.Nature == Natures.GENTLE) points = 300;
                    if (this.Nature == Natures.HARDY) points = 300;
                    if (this.Nature == Natures.HASTY) points = 300;
                    if (this.Nature == Natures.IMPISH) points = 300;
                    if (this.Nature == Natures.JOLLY) points = 300;
                    if (this.Nature == Natures.LAX) points = 225;
                    if (this.Nature == Natures.LONELY) points = 450;
                    if (this.Nature == Natures.MILD) points = 225;
                    if (this.Nature == Natures.MODEST) points = 300;
                    if (this.Nature == Natures.NAIVE) points = 300;
                    if (this.Nature == Natures.NAUGHTY) points = 225;
                    if (this.Nature == Natures.QUIET) points = 300;
                    if (this.Nature == Natures.QUIRKY) points = 225;
                    if (this.Nature == Natures.RASH) points = 300;
                    if (this.Nature == Natures.RELAXED) points = 225;
                    if (this.Nature == Natures.SASSY) points = 150;
                    if (this.Nature == Natures.SERIOUS) points = 450;
                    if (this.Nature == Natures.TIMID) points = 450;
                    break;
                case pokemonActions.Party:
                    if (this.Nature == Natures.ADAMANT) points = 150;
                    if (this.Nature == Natures.BASHFUL) points = 75;
                    if (this.Nature == Natures.BOLD) points = 200;
                    if (this.Nature == Natures.BRAVE) points = 150;
                    if (this.Nature == Natures.CALM) points = 100;
                    if (this.Nature == Natures.CAREFUL) points = 75;
                    if (this.Nature == Natures.DOCILE) points = 100;
                    if (this.Nature == Natures.GENTLE) points = 75;
                    if (this.Nature == Natures.HARDY) points = 150;
                    if (this.Nature == Natures.HASTY) points = 75;
                    if (this.Nature == Natures.IMPISH) points = 150;
                    if (this.Nature == Natures.JOLLY) points = 100;
                    if (this.Nature == Natures.LAX) points = 150;
                    if (this.Nature == Natures.LONELY) points = 150;
                    if (this.Nature == Natures.MILD) points = 75;
                    if (this.Nature == Natures.MODEST) points = 75;
                    if (this.Nature == Natures.NAIVE) points = 150;
                    if (this.Nature == Natures.NAUGHTY) points = 200;
                    if (this.Nature == Natures.QUIET) points = 100;
                    if (this.Nature == Natures.QUIRKY) points = 50;
                    if (this.Nature == Natures.RASH) points = 100;
                    if (this.Nature == Natures.RELAXED) points = 75;
                    if (this.Nature == Natures.SASSY) points = 150;
                    if (this.Nature == Natures.SERIOUS) points = 100;
                    if (this.Nature == Natures.TIMID) points = 50;
                    break;
                case pokemonActions.DayCare:
                case pokemonActions.MysteryAction:
                    if (this.Nature == Natures.ADAMANT) points = 300;
                    if (this.Nature == Natures.BASHFUL) points = 450;
                    if (this.Nature == Natures.BOLD) points = 300;
                    if (this.Nature == Natures.BRAVE) points = 225;
                    if (this.Nature == Natures.CALM) points = 450;
                    if (this.Nature == Natures.CAREFUL) points = 450;
                    if (this.Nature == Natures.DOCILE) points = 225;
                    if (this.Nature == Natures.GENTLE) points = 600;
                    if (this.Nature == Natures.HARDY) points = 150;
                    if (this.Nature == Natures.HASTY) points = 150;
                    if (this.Nature == Natures.IMPISH) points = 150;
                    if (this.Nature == Natures.JOLLY) points = 150;
                    if (this.Nature == Natures.LAX) points = 225;
                    if (this.Nature == Natures.LONELY) points = 150;
                    if (this.Nature == Natures.MILD) points = 450;
                    if (this.Nature == Natures.MODEST) points = 600;
                    if (this.Nature == Natures.NAIVE) points = 225;
                    if (this.Nature == Natures.NAUGHTY) points = 225;
                    if (this.Nature == Natures.QUIET) points = 300;
                    if (this.Nature == Natures.QUIRKY) points = 600;
                    if (this.Nature == Natures.RASH) points = 300;
                    if (this.Nature == Natures.RELAXED) points = 600;
                    if (this.Nature == Natures.SASSY) points = 225;
                    if (this.Nature == Natures.SERIOUS) points = 300;
                    if (this.Nature == Natures.TIMID) points = 600;
                    break;
                case pokemonActions.Scent:
                    if (this.Nature == Natures.ADAMANT) points = 75;
                    if (this.Nature == Natures.BASHFUL) points = 200;
                    if (this.Nature == Natures.BOLD) points = 50;
                    if (this.Nature == Natures.BRAVE) points = 75;
                    if (this.Nature == Natures.CALM) points = 150;
                    if (this.Nature == Natures.CAREFUL) points = 150;
                    if (this.Nature == Natures.DOCILE) points = 100;
                    if (this.Nature == Natures.GENTLE) points = 150;
                    if (this.Nature == Natures.HARDY) points = 100;
                    if (this.Nature == Natures.HASTY) points = 150;
                    if (this.Nature == Natures.IMPISH) points = 75;
                    if (this.Nature == Natures.JOLLY) points = 150;
                    if (this.Nature == Natures.LAX) points = 150;
                    if (this.Nature == Natures.LONELY) points = 200;
                    if (this.Nature == Natures.MILD) points = 200;
                    if (this.Nature == Natures.MODEST) points = 100;
                    if (this.Nature == Natures.NAIVE) points = 100;
                    if (this.Nature == Natures.NAUGHTY) points = 75;
                    if (this.Nature == Natures.QUIET) points = 100;
                    if (this.Nature == Natures.QUIRKY) points = 75;
                    if (this.Nature == Natures.RASH) points = 150;
                    if (this.Nature == Natures.RELAXED) points = 150;
                    if (this.Nature == Natures.SASSY) points = 100;
                    if (this.Nature == Natures.SERIOUS) points = 75;
                    if (this.Nature == Natures.TIMID) points = 150;
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
        private PokemonUnity.Item.Item.Mail mail { get; set; }
        /// <summary>
        /// Perform a null check; if anything other than null, there is a message
        /// </summary>
        /// ToDo: Item category
        public string Mail
        {
            get
            {
                if (this.mail == null || !PokemonUnity.Item.Item.Mail.IsMail(this.Item)) return null; //If empty return null
                                                                                                      //if (mail.Message.Length == 0 || this.Item == 0)//|| this.item.Category != Items.Category.Mail )
                                                                                                      //{
                                                                                                      //    //mail = null;
                                                                                                      //	return null;
                                                                                                      //}
                                                                                                      //ToDo: Return the string or class?
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
        ///	Used only for a few pokemon to specify what form it's in. 
        /// </summary>
        /// <remarks>
        ///	<see cref="Pokemons.UNOWN"/> = letter of the alphabet.
        ///	<see cref="Pokemons.DEOXYS"/> = which of the four forms.
        ///	<see cref="Pokemons.BURMY"/>/<see cref="Pokemons.WORMADAM"/> = cloak type. Does not change for Wormadam.
        ///	<see cref="Pokemons.SHELLOS"/>/<see cref="Pokemons.GASTRODON"/> = west/east alt colours.
        ///	<see cref="Pokemons.ROTOM"/> = different possesed appliance forms.
        ///	<see cref="Pokemons.GIRATINA"/> = Origin/Altered form.
        ///	<see cref="Pokemons.SHAYMIN"/> = Land/Sky form.
        ///	<see cref="Pokemons.ARCEUS"/> = Type.
        ///	<see cref="Pokemons.BASCULIN"/> = appearance.
        ///	<see cref="Pokemons.DEERLING"/>/<see cref="Pokemons.SAWSBUCK"/> = appearance.
        ///	<see cref="Pokemons.TORNADUS"/>/<see cref="Pokemons.THUNDURUS"/>/<see cref="Pokemons.LANDORUS"/> = Incarnate/Therian forms.
        ///	<see cref="Pokemons.KYUREM"/> = Normal/White/Black forms.
        ///	<see cref="Pokemons.KELDEO"/> = Ordinary/Resolute forms.
        ///	<see cref="Pokemons.MELOETTA"/> = Aria/Pirouette forms.
        ///	<see cref="Pokemons.GENESECT"/> = different Drives.
        ///	<see cref="Pokemons.VIVILLON"/> = different Patterns.
        ///	<see cref="Pokemons.FLABEBE"/>/<see cref="Pokemons.FLOETTE"/>/<see cref="Pokemons.FLORGES"/> = Flower colour.
        ///	<see cref="Pokemons.FURFROU"/> = haircut.
        ///	<see cref="Pokemons.PUMPKABOO"/>/<see cref="Pokemons.GOURGEIST"/> = small/average/large/super sizes. 
        ///	<see cref="Pokemons.HOOPA"/> = Confined/Unbound forms.
        ///	<see cref="Pokemons.CASTFORM"/>? = different weather forms
        ///	<see cref="Pokemons.PIKACHU"/>, and MegaEvolutions?
        /// </remarks>
        /// ToDo: Fix Forms and uncomment
        /// Maybe a method, where when a form is changed
        /// checks pokemon value and overwrites name and stats
        /// Some forms have a SpeciesId and others are battle only
        public int Form { get { return _base.Form; } set { if (value >= 0 && value <= _base.Forms) _base.Form = value; } }

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
            return "ABCDEFGHIJKLMNOPQRSTUVWXYZ?!".ToCharArray()[Form]; //ToDo: FormId; "if pokemon is an Unknown"
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
                if (this.hp == 0) this.Status = Status.NONE; // statusCount = 0; //ToDo: Fainted
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
            this.HP = TotalHP;          //ToDo: Return 'true' on success?
        }

        /// <summary>
        /// Heals the status problem of this Pokemon
        /// </summary>
        public void HealStatus()
        {
            if (this.isEgg) return;
            this.Status = 0; StatusCount = 0; //remove status ailments
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
                for (int i = 0; i < 4; i++)
                {
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
            public string Name
            {
                get
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
                    return this.forms[this.Form] ?? this.name;
                }
            }
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
            /// <summary>
            /// Best used in battle simulator. 
            /// Check to see if pokemons are legendary, and exclude from battle
            /// </summary>
            public Rarity Rarity { get; set; }
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
            /// <summary>
            /// Returns whether this Pokemon species is restricted to only ever being one gender (or genderless)
            /// </summary>
            public bool IsSingleGendered
            {
                get
                {
                    switch (MaleRatio)
                    {
                        case GenderRatio.AlwaysMale:
                        case GenderRatio.AlwaysFemale:
                        case GenderRatio.Genderless:
                            return true;
                        default:
                            return false;
                    }
                }
            }

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
            public LevelingRate GrowthRate { get; private set; }
            public Color PokedexColor { get; private set; }

            /// <summary>
            /// Friendship levels is the same as pokemon Happiness.
            /// </summary>
            public int BaseFriendship { get; private set; }
            public int BaseExpYield { get; private set; }
            public int BaseStatsHP { get; private set; }
            public int BaseStatsATK { get; private set; }
            public int BaseStatsDEF { get; private set; }
            public int BaseStatsSPA { get; private set; }
            public int BaseStatsSPD { get; private set; }
            public int BaseStatsSPE { get; private set; }
            public int evYieldHP { get; private set; }
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
            /// <summary>
            /// All the moves this pokemon species can learn, and the methods by which they learn them
            /// </summary>
            public PokemonMoveTree MoveTree { get; private set; }
            /// ToDo: Evolution class type array here
            public IPokemonEvolution[] Evolutions { get; private set; }

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
                if (formvalues.Count != 0)
                {
                    this.forms = formvalues.ToArray();
                }
                else
                {
                    this.forms = new string[1] { null };
                }
            }

            public PokemonData(Pokemons Id = Pokemons.NONE, int[] regionalDex = null, //string name, 
                                Types type1 = Types.NONE, Types type2 = Types.NONE, Abilities ability1 = Abilities.NONE, Abilities ability2 = Abilities.NONE, Abilities hiddenAbility = Abilities.NONE,//Abilities[] abilities,
                                GenderRatio genderRatio = GenderRatio.Genderless, float? maleRatio = null, int catchRate = 1, EggGroups eggGroup1 = EggGroups.NONE, EggGroups eggGroup2 = EggGroups.NONE, int hatchTime = 0,
                                float height = 0f, float weight = 0f, int baseExpYield = 0, LevelingRate levelingRate = LevelingRate.MEDIUMFAST,
                                //int? evYieldHP, int? evYieldATK, int? evYieldDEF, int? evYieldSPA, int? evYieldSPD, int? evYieldSPE,
                                int evHP = 0, int evATK = 0, int evDEF = 0, int evSPA = 0, int evSPD = 0, int evSPE = 0,
                                Color pokedexColor = Color.NONE, int baseFriendship = 0,//* / string species, string pokedexEntry,*/
                                int baseStatsHP = 0, int baseStatsATK = 0, int baseStatsDEF = 0, int baseStatsSPA = 0, int baseStatsSPD = 0, int baseStatsSPE = 0,
                                Rarity rarity = Rarity.Common, float luminance = 0f, //Color lightColor,
                                PokemonMoveset[] movesetmoves = null,
                                int[] movesetLevels = null, Moves[] movesetMoves = null, int[] tmList = null,
                                IPokemonEvolution[] evolution = null,
                                int[] evolutionID = null, int[] evolutionLevel = null, int[] evolutionMethod = null, /*string[] evolutionRequirements,*/
                                                                                                                     //ToDo: What if: `Pokemons form` to point back to base pokemon, and Pokemons.NONE, if they are the base form?
                                                                                                                     //that way, we can assign values to pokemons with forms that give stat bonuses...
                                                                                                                     //want to find a way to add pokemon froms from a different method. Maybe something like overwriting the `Database` values to match those of base pokemon for values that are duplicated.
                                                                                                                     //Or I'll just add it at the bottom towards end of array using copy-paste method.
                                Pokemons baseForm = Pokemons.NONE, //int forms = 0, 
                                int[,] heldItem = null) : this(Id)
            {//new PokemonData(1,1,"Bulbasaur",12,4,65,null,34,45,1,7,20,7f,69f,64,4,PokemonData.PokedexColor.GREEN,"Seed","\"Bulbasaur can be seen napping in bright sunlight. There is a seed on its back. By soaking up the sun’s rays, the seed grows progressively larger.\"",45,49,49,65,65,45,0f,new int[]{1,3,7,9,13,13,15,19,21,25,27,31,33,37},new int[]{33,45,73,22,77,79,36,75,230,74,38,388,235,402},new int[]{14,15,70,76,92,104,113,148,156,164,182,188,207,213,214,216,218,219,237,241,249,263,267,290,412,447,474,496,497,590},new int[]{2},new int[]{16},new int[]{1})
                this.RegionalPokedex = regionalDex;

                this.type1 = type1; //!= null ? (Types)type1 : Types.NONE;
                this.type2 = type2; //!= null ? (Types)type2 : Types.NONE;
                                    //this.ability = abilities;
                this.ability1 = (Abilities)ability1;
                this.ability2 = (Abilities)ability2;
                this.abilityh = (Abilities)hiddenAbility;

                this.MaleRatio = maleRatio.HasValue ? getGenderRatio(maleRatio.Value) : genderRatio; //ToDo: maleRatio; maybe `GenderRatio genderRatio(maleRatio);`
                this.CatchRate = catchRate;
                this.eggGroup1 = eggGroup1;
                this.eggGroup2 = eggGroup2;
                this.HatchTime = hatchTime;

                this.Height = height;
                this.Weight = weight;
                this.BaseExpYield = baseExpYield;
                this.GrowthRate = (LevelingRate)levelingRate; //== null ? (LevelingRate)levelingRate : LevelingRate.NONE;

                this.evYieldHP = evHP;
                this.evYieldATK = evATK;
                this.evYieldDEF = evDEF;
                this.evYieldSPA = evSPA;
                this.evYieldSPD = evSPD;
                this.evYieldSPE = evSPE;

                this.BaseStatsHP = baseStatsHP;
                this.BaseStatsATK = baseStatsATK;
                this.BaseStatsDEF = baseStatsDEF;
                this.BaseStatsSPA = baseStatsSPA;
                this.BaseStatsSPD = baseStatsSPD;
                this.BaseStatsSPE = baseStatsSPE;
                this.BaseFriendship = baseFriendship;

                this.Rarity = rarity;
                this.Luminance = luminance;
                //this.lightColor = lightColor;
                this.PokedexColor = pokedexColor | Color.NONE;

                //ToDo: wild pokemon held items not yet implemented
                this.HeldItem = heldItem; //[item id,% chance]

                this.MoveTree = new PokemonMoveTree(movesetmoves);
                //this.MovesetLevels = movesetLevels;
                //this.MovesetMoves = movesetMoves; 
                //this.tmList = tmList; 

                this.Evolutions = evolution ?? new IPokemonEvolution[0];
                //this.EvolutionID = evolutionID;
                //this.evolutionMethod = evolutionMethod; 
                //this.evolutionRequirements = evolutionRequirements;
            }
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

            /// <summary>
            /// Returns the list of moves this Pokémon can learn by training method.
            /// </summary>
            public Moves[] GetMoveList(LearnMethod? method = null)
            {
                switch (method)
                {
                    case LearnMethod.egg:
                        return MoveTree.Egg;
                    case LearnMethod.levelup:
                        return MoveTree.LevelUp.Select(x => x.Key).ToArray();
                    case LearnMethod.machine:
                        return MoveTree.Machine;
                    case LearnMethod.tutor:
                        return MoveTree.Tutor;
                    case LearnMethod.shadow:
                    case LearnMethod.xd_shadow:
                        List<Moves> s = new List<Moves>();
                        s.AddRange(MoveTree.Shadow);
                        s.AddRange(MoveTree.Shadow.Where(x => !s.Contains(x)).Select(x => x));
                        return s.ToArray();
                    default:
                        List<Moves> list = new List<Moves>();
                        list.AddRange(MoveTree.Egg);
                        list.AddRange(MoveTree.Machine.Where(x => !list.Contains(x)).Select(x => x));
                        list.AddRange(MoveTree.Tutor.Where(x => !list.Contains(x)).Select(x => x));
                        list.AddRange(MoveTree.LevelUp.Where(x => !list.Contains(x.Key))/*(x => x.Value <= this.Level)*/.Select(x => x.Key));
                        return list.ToArray();
                }
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
                public PokemonEvolution(Pokemons EvolveTo, EvolutionMethod EvolveHow)
                {
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

                public PokemonEvolution(Pokemons EvolveTo, EvolutionMethod EvolveHow, T Value) : base(EvolveTo: EvolveTo, EvolveHow: EvolveHow)
                {
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
            #region Variables
            private byte level { get { return GetLevelFromExperience(Growth, Current); } }
            /// <summary>
            /// Current accumalitive experience points gained (and collected) by this Pokemon.
            /// </summary>
            /// <remarks>Exp points should not be able to go into negatives?</remarks>
            /// ToDo: Make shadow pokemons exp points negative?... 
            /// no, cause they still need to level up after purified
            /// ToDo: Make exp points less than 0 equal to 0
            public int Current { get; private set; }
            public int NextLevel { get { return GetExperience(Growth, level + 1); } }
            private LevelingRate Growth { get; set; }
            #endregion

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

            #region Methods
            /// <summary>
            /// Adds experience points to total Exp. Points.
            /// </summary>
            /// <param name="experienceGain">Exp. Points to add</param>
            public void AddExperience(int experienceGain)
            {
                Current += experienceGain;
                int maxexp = GetExperience(Growth, Settings.MAXIMUMLEVEL);
                if (Current > maxexp) Current = maxexp;
            }
            // <summary>
            // Adds experience points ensuring that the new total doesn't
            // exceed the maximum Exp. Points for the given growth rate.
            // </summary>
            // <param name="levelingRate">Growth rate.</param>
            // <param name="currentExperience">Current Exp Points.</param>
            // <param name="experienceGain">Exp. Points to add</param>
            // <returns></returns>
            // Subtract ceiling exp for left over experience points remaining after level up?...
            //public int AddExperience(LevelingRate levelingRate, int currentExperience, int experienceGain)
            //{
            //	int exp = currentExperience + experienceGain;
            //	int maxexp = GetExperience(levelingRate, Settings.MAXIMUMLEVEL);
            //	if (exp > maxexp) exp = maxexp;
            //	return exp;
            //}

            #region Static Methods
            private static int GetExperience(LevelingRate levelingRate, int currentLevel)
            {
                int exp = 0;
                //if (currentLevel > 100) currentLevel = 100; 
                //if (currentLevel > Settings.MAXIMUMLEVEL) currentLevel = Settings.MAXIMUMLEVEL; 
                if (levelingRate == LevelingRate.ERRATIC)
                {
                    if (currentLevel > 99)
                    {
                        //exp = (int)System.Math.Floor((System.Math.Pow(currentLevel, 3)) * (160 - currentLevel) / 100);
                        exp = (int)System.Math.Floor((System.Math.Pow(currentLevel, 3)) * (currentLevel * 6 / 10) / 100);
                    }
                    else exp = expTableErratic[currentLevel]; //Because the array starts at 0, not 1.
                }
                else if (levelingRate == LevelingRate.FAST)
                {
                    if (currentLevel > 99)
                    {
                        exp = (int)System.Math.Floor(System.Math.Pow(currentLevel, 3) * (4 / 5));
                    }
                    else exp = expTableFast[currentLevel];
                }
                else if (levelingRate == LevelingRate.MEDIUMFAST)
                {
                    if (currentLevel > 99)
                    {
                        exp = (int)System.Math.Floor(System.Math.Pow(currentLevel, 3));
                    }
                    else exp = expTableMediumFast[currentLevel];
                }
                else if (levelingRate == LevelingRate.MEDIUMSLOW)
                {
                    if (currentLevel > 99)
                    {
                        //Dont remember why currentlevel minus 1 is in formula...
                        //I think it has to deal with formula table glitch for lvl 1-2 pokemons...
                        //exp = (int)System.Math.Floor(((6 / 5) * System.Math.Pow(currentLevel - 1, 3)) - (15 * System.Math.Pow(currentLevel - 1, 3)) + (100 * (currentLevel - 1)) - 140);
                        exp = (int)System.Math.Floor((6 * System.Math.Pow(currentLevel - 1, 3) / 5) - 15 * System.Math.Pow(currentLevel - 1, 2) + 100 * (currentLevel - 1) - 140);
                    }
                    else exp = expTableMediumSlow[currentLevel];
                }
                else if (levelingRate == LevelingRate.SLOW)
                {
                    if (currentLevel > 99)
                    {
                        exp = (int)System.Math.Floor(System.Math.Pow(currentLevel, 3) * (5 / 4));
                    }
                    else exp = expTableSlow[currentLevel];
                }
                else if (levelingRate == LevelingRate.FLUCTUATING)
                {
                    if (currentLevel > 99)
                    {
                        int rate = 82;
                        //Slow rate with increasing level
                        rate -= (currentLevel - 100) / 2;
                        if (rate < 40) rate = 40;

                        //exp = (int)System.Math.Floor(System.Math.Pow(currentLevel, 3) * ((System.Math.Floor(System.Math.Pow(currentLevel, 3) / 2) + 32) / 50));
                        exp = (int)System.Math.Floor(System.Math.Pow(currentLevel, 3) * (((currentLevel * rate / 100) / 50)));
                    }
                    else exp = expTableFluctuating[currentLevel];
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
            /// Calculates a level given the number of Exp Points and growth rate.
            /// </summary>
            /// <param name="levelingRate">Growth rate.</param>
            /// <param name="experiencePoints">Current Experience Points</param>
            /// <returns></returns>
            public static byte GetLevelFromExperience(LevelingRate levelingRate, int experiencePoints)
            {
                if (experiencePoints <= 0) return 0;
                int maxexp = GetExperience(levelingRate, Settings.MAXIMUMLEVEL);
                if (experiencePoints > maxexp) experiencePoints = maxexp;
                for (byte i = 0; i < Settings.MAXIMUMLEVEL; i++)
                {
                    if (GetExperience(levelingRate, Settings.MAXIMUMLEVEL) == experiencePoints) return Settings.MAXIMUMLEVEL;
                    if (GetExperience(levelingRate, Settings.MAXIMUMLEVEL) < experiencePoints) return Settings.MAXIMUMLEVEL;
                    if (GetExperience(levelingRate, i) > experiencePoints) return (byte)(i - 1);
                }
                return 0;
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
            #endregion
            #endregion

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
}