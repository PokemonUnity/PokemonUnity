using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Attack;
using PokemonUnity.Inventory;
using PokemonUnity.Monster;
using PokemonUnity.Monster.Data;

namespace PokemonUnity.Monster
{
    public partial class Pokemon
    {
        #region Variables
		private Pokemons pokemons { get; set; }
        /// <summary>
        /// Current Total HP
        /// </summary>
        public int TotalHP
        {
            get
            {
				//Shedinja can be affected by HP-Up
                if (_base.BaseStatsHP == 1) return 1 + (EV[(int)Stats.HP] / 4);
                return //totalHP;
                    ((2 * _base.BaseStatsHP + IV[(int)Stats.HP] + (EV[(int)Stats.HP] / 4)) * Level) / 100 + Level + 10;
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
                return (int)Math.Floor((((2 * _base.BaseStatsATK + IV[(int)Stats.ATTACK] + (EV[(int)Stats.ATTACK] / 4)) * Level) / 100 + 5) * Game.NatureData[Nature].ATK);
            }
        }
        /// <summary>
        /// Current Defense stat
        /// </summary>
        public virtual int DEF
        {
            get
            {
                return (int)Math.Floor((((2 * _base.BaseStatsDEF + IV[(int)Stats.DEFENSE] + (EV[(int)Stats.DEFENSE] / 4)) * Level) / 100 + 5) * Game.NatureData[Nature].DEF);
            }
        }
        /// <summary>
        /// Current Special Attack Stat
        /// </summary>
        public virtual int SPA
        {
            get
            {
                return (int)Math.Floor((((2 * _base.BaseStatsSPA + IV[(int)Stats.SPATK] + (EV[(int)Stats.SPATK] / 4)) * Level) / 100 + 5) * Game.NatureData[Nature].SPA);
            }
        }
        /// <summary>
        /// Current Special Defense Stat
        /// </summary>
        public virtual int SPD
        {
            get
            {
                return (int)Math.Floor((((2 * _base.BaseStatsSPD + IV[(int)Stats.SPDEF] + (EV[(int)Stats.SPDEF] / 4)) * Level) / 100 + 5) * Game.NatureData[Nature].SPD);
            }
        }
        /// <summary>
        /// Current Speed Stat
        /// </summary>
        public virtual int SPE
        {
            get
            {
                return (int)Math.Floor((((2 * _base.BaseStatsSPE + IV[(int)Stats.SPEED] + (EV[(int)Stats.SPEED] / 4)) * Level) / 100 + 5) * Game.NatureData[Nature].SPE);
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
		/// Double as Friendship Meter after pokemon is hatched.
        /// </summary>
        /// ToDo: Sequence of events to occur when egg is hatching (roll dice for ability, moves, and animate hatching)
		/// ToDo: New variable for Friendship, Math.Abs(EggSteps)?
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
                    this.Level = Core.EGGINITIALLEVEL;
					//if hatching, generate new moves to include egg moves 
					//and everything from current level to below
                    this.GenerateMoveset(egg: true);
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
        //public IPokemonEvolution[] Evolutions { get { return _base.Evolutions; } }
        //protected PokemonData _base { get; private set; }
        protected Data.PokemonData _base { get { return Game.PokemonData[pokemons]; } }
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
        /// <summary>
        /// Uses PokemonData to initialize a Pokemon from base stats
        /// </summary>
        public Pokemon()
        {
            //_base = PokemonData.GetPokemon(Pokemons.NONE);
			pokemons = Pokemons.NONE;
            PersonalId = Core.Rand.Next(256);
            PersonalId |= Core.Rand.Next(256) << 8;
            PersonalId |= Core.Rand.Next(256) << 16;
            PersonalId |= Core.Rand.Next(256) << 24;
            Ability = Abilities.NONE;
            Nature = (Natures)new System.Random(Core.Seed()).Next(1, Game.NatureData.Count); //Monster.Nature.GetRandomNature()
			//ToDo: Maybe add TrainerId = <int> here, before isShiny()?
			//shinyFlag = IsShiny; //isShiny(); ToDo: Fix WildPokemon.TrainerId
			//Gender = isMale();
			//IV = new int[] { 10, 10, 10, 10, 10, 10 };
            IV = new byte[] { (byte)Core.Rand.Next(32), (byte)Core.Rand.Next(32), (byte)Core.Rand.Next(32), (byte)Core.Rand.Next(32), (byte)Core.Rand.Next(32), (byte)Core.Rand.Next(32) };
            EV = new byte[6];
            Contest = new byte[6];
            Exp = new Experience(GrowthRate);
			TempLevel = Level;
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
        public Pokemon(Pokemons pokemon) : this()
        {
            //_base = PokemonData.GetPokemon(pokemon);
			pokemons = pokemon;
            Exp = new Experience(GrowthRate);
			TempLevel = Level;
			eggSteps = _base.HatchTime;
            Ability = abilityFlag;
            Gender = gender; //GenderRatio.//Pokemon.PokemonData.GetPokemon(pokemon).MaleRatio
			//ToDo: Undo comment
			//Item = (Items)_base.HeldItem[0,Core.pokemonGeneration];
            GenerateMoveset();

            //calcStats();
        }

		/// <summary>
		/// Instializes a new Pokemon, with values at default. 
		/// Pokemon is created at the lowest possible level, 
		/// with all stats randomly generated/assigned (new roll)
		/// </summary>
		/// <param name="pkmn">Pokemon being generated</param>
		/// <param name="isEgg">Whether or not this level 
		/// <see cref="Settings.EGGINITIALLEVEL"/> pokemon is hatched.</param>
        public Pokemon(Pokemons pkmn, bool isEgg) : this(pkmn) { if (!isEgg) EggSteps = 0; }

		/// <summary>
		/// Instializes a new Pokemon, with values at default. 
		/// Pokemon is created at the level assigned in parameter, 
		/// with all stats randomly generated/assigned (new roll)
		/// </summary>
		/// <param name="pkmn">Pokemon being generated</param>
		/// <param name="level">Level this pokemon start ats</param>
		/// <param name="isEgg">Whether or not this pokemon is hatched; 
		/// if pokemon <see cref="isEgg"/> is false, it loses benefits 
		/// of learning egg moves</param>
		public Pokemon(Pokemons pkmn, byte level, bool isEgg = false) : this(pkmn, isEgg) { Level = level; GenerateMoveset(); }

		/// <summary>
		/// Instializes a new Pokemon, with values at default. 
		/// Pokemon is created at the level assigned in parameter, 
		/// with all stats randomly generated/assigned (new roll).
		/// </summary>
		/// <param name="pkmn">Pokemon being generated</param>
		/// <param name="original">Assigns original <see cref="Trainer"/> 
		/// of this pokemon. 
		/// Affects ability to command pokemon, if player is not OT</param>
		/// <param name="level">Level this pokemon start ats</param>
		public Pokemon(Pokemons pkmn, Trainer original, byte level = Core.EGGINITIALLEVEL) : this(pkmn, level: level, isEgg: false) { OT = original; }

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
            bool EGG = false,
            Items TPBALL = Items.NONE) : this(TPSPECIES, level: TPLEVEL, isEgg: EGG)
        {
            //Random rand = new Random(Core.Seed());//(int)TPSPECIES+TPLEVEL
            IV = TPIV ?? IV;
            //EV = new int[6];

			/*# Japanese Dream World Squirtle from http://projectpokemon.org/events
			blob = base64.b64decode('J2ZqBgAAICQHAAAAkOaKyTACAABGLAABAAAAAAAAAAAAAAAAA'
			'AAAACEAJwCRAG4AIx4eKAAAAAD171MHAAAAAAAAAQAAAAAAvDDLMKww4TD//wAAAAAAAA'
			'AAAAD//wAVAAAAAAAAAAAw/zD/T/9S/0f///8AAAAAAAAACgoOAABLAAAZCgAAAA==')
		{
			'ability': {'id': 44, 'name': u'Rain Dish'},
			'date met': '2010-10-14',
			'gender': 'male',
			'genes': {u'attack': 31,
					u'defense': 27,
					u'hp': 21,
					u'special attack': 21,
					u'special defense': 3,
					u'speed': 7},
			'happiness': 70,
			'level': 10,
			'met at level': 10,
			'met location': {'id_dp': 75, 'name': u'Spring Path'},
			'moves': [{'id': 33, 'name': u'Tackle', 'pp': 35},
					{'id': 39, 'name': u'Tail Whip', 'pp': 30},
					{'id': 145, 'name': u'Bubble', 'pp': 30},
					{'id': 110, 'name': u'Withdraw', 'pp': 40}],
			'nickname': u'ゼニガメ',
			'nickname trash': 'vDDLMKww4TD//wAAAAAAAAAAAAD//w==',
			'nicknamed': False,
			'oiginal trainer': {'gender': 'male',
								'id': 59024,
								'name': u'ＰＰｏｒｇ',
								'secret': 51594},
			'original country': 'jp',
			'original version': 21,
			'personality': 107636263,
			'pokeball': {'id_dppt': 25, 'name': u'Hyper Potion'},
			'species': {'id': 7, 'name': u'Squirtle'}
		}*/

			//calcStats();
		}

		/// <summary>
		/// This is used SPECIFICALLY for regenerating a pokemon from 
		/// <see cref="PokemonUnity.Saving.SerializableClasses.SeriPokemon"/>
		/// </summary>
		/// <param name="species"></param>
		/// <param name="original"></param>
		/// <param name="nickName"></param>
		/// <param name="form"></param>
		/// <param name="ability"></param>
		/// <param name="nature"></param>
		/// <param name="isShiny"></param>
		/// <param name="gender"></param>
		/// <param name="pokerus"></param>
		/// <param name="ishyper"></param>
		/// <param name="shadowLevel"></param>
		/// <param name="currentHp"></param>
		/// <param name="item"></param>
		/// <param name="iv"></param>
		/// <param name="ev"></param>
		/// <param name="obtainedLevel"></param>
		/// <param name="currentExp"></param>
		/// <param name="happiness"></param>
		/// <param name="status"></param>
		/// <param name="statusCount"></param>
		/// <param name="eggSteps"></param>
		/// <param name="ballUsed"></param>
		/// <param name="mail"></param>
		/// <param name="moves"></param>
		/// <param name="ribbons"></param>
		/// <param name="markings"></param>
		/// <param name="personalId"></param>
		/// <param name="obtainedMethod"></param>
		/// <param name="timeReceived"></param>
		/// <param name="timeEggHatched"></param>
		/// ToDo: Maybe make this private? Move implicit convert to Pokemon class
        public Pokemon(Pokemons species, 
			Trainer original,
			string nickName, int form,
            Abilities ability, Natures nature,
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
            DateTimeOffset timeReceived, DateTimeOffset? timeEggHatched) : this(species, original)
        {
            //Check to see if nickName is filled
			//ToDo: Check if whitespace or empty, then assign it to name, even if null...
            if (!string.IsNullOrEmpty(nickName))
                name = nickName;
            else
                name = null;

			//_base = Pokemon.PokemonData.GetPokemon(species);
			pokemons = species;
			Form = form;

            Ability = ability;
            Nature = nature;

            PersonalId = personalId;

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
			//ToDo: Load exp without triggering a level-up scene
            Exp.AddExperience(currentExp - Exp.Current);
			TempLevel = Level;

			Happiness = happiness;

            Status = status;
            StatusCount = statusCount;

            EggSteps = eggSteps;

            this.ballUsed = ballUsed;
            if (PokemonUnity.Inventory.Item.Mail.IsMail(item))
            {
                this.mail = new Inventory.Item.Mail((Items)item);
                this.mail.Message = mail;
            }

            this.moves = moves;

            this.ribbons = ribbons.ToList();
            Markings = markings;

            ObtainedMode = obtainedMethod;
            obtainWhen = timeReceived;
            hatchedWhen = timeEggHatched;
        }

		/// <summary>
		/// Use this constructor when capturing wild pokemons from a battle
		/// </summary>
		/// <param name="pkmn"></param>
		/// <param name="pokeball"></param>
		/// <param name="obtain"></param>
		/// <param name="nickname"></param>
		public Pokemon(Pokemon pkmn, Items pokeball, ObtainedMethod obtain = ObtainedMethod.MET, string nickname = null) 
			: this (
			species: pkmn.Species,
			original: Game.Player.Trainer,
			nickName: nickname, form: pkmn.Form,
			ability: pkmn.Ability, nature: pkmn.Nature,
			isShiny: pkmn.IsShiny, gender: pkmn.Gender,
			pokerus: pkmn.Pokerus, ishyper: pkmn.isHyperMode,
			shadowLevel: pkmn.ShadowLevel, currentHp: pkmn.HP, 
			item: pkmn.Item, iv: pkmn.IV, ev: pkmn.EV, 
			obtainedLevel: pkmn.Level, currentExp: pkmn.Exp.Current,
			happiness: pkmn.Happiness, status: pkmn.Status, 
			statusCount: pkmn.StatusCount, eggSteps: pkmn.EggSteps, 
			ballUsed: pokeball, mail: pkmn.Mail, moves: pkmn.moves,
			ribbons: pkmn.Ribbons.ToArray(), markings: pkmn.Markings,
			personalId: pkmn.PersonalId, obtainedMethod: obtain,
			timeReceived: DateTimeOffset.Now, timeEggHatched: null)
		{
			//ToDo: What to do about timeEggHatched and OT
			//Should make a new class for trading pokemons?
		}
		#endregion

		#region Ownership, obtained information
		/// <summary>
		/// Manner Obtained:
		/// </summary>
		public ObtainedMethod ObtainedMode { get; private set; }
		//ToDo: Change to EncounterType
        public enum ObtainedMethod
        {
            MET = 0,
            EGG = 1,
			//If EncounterType == Gift, then it's Traded
            TRADED = 2,
            /// <summary>
            /// NPC-Event?
            /// </summary>
            FATEFUL_ENCOUNTER = 4
        }
		//ToDo: Nintendo has variable for location where egg hatched
        /// <summary>
        /// Map where obtained (as egg, or in wild).
		/// Return no results if Traded/Gift...
        /// </summary>
        /// <remarks>
        /// Doubles as "HatchedMap"
        /// ToDo: Make this an enum
        /// </remarks>
        private int obtainMap { get; set; }
        /// <summary>
        /// Replaces the obtain map's name if not null
        /// </summary>
		/// ToDo: if (isOutside) return generic "got from player"
		/// else, all data stored in class remains unchanged to OT?
		/// Try: if (encounter == gift) return "got from player" (npcs too)
        private string obtainString { get; set; }
        //private int obtainLevel; // = 0;
		//Should obtainWhen be used for hatchedWhen? Eggs dont mean anything...
		//ToDo: Nintendo has date variable for when egg was received
        private System.DateTimeOffset obtainWhen { get; set; }
        private System.DateTimeOffset? hatchedWhen { get; set; }
        /// <summary>
        /// Original Trainer's Name
        /// </summary>
        /// <remarks>
        /// ToDo: PlayerTrainer's hash value instead of class; maybe GUID?
        /// </remarks>
        public Trainer OT { get; private set; }
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

		/// <summary>
		/// Sets the catch infos of the Pokémon. Uses the current map name and player name + OT.
		/// </summary>
		/// <param name="Ball">The Pokéball this Pokémon got captured in.</param>
		/// <param name="Method">The capture method.</param>
		public void SetCatchInfos(Items Ball, ObtainedMethod Method)
		{
			//ToDo: If OT != null, dont change it... Pokemon is already captured... Unless Pokeball.SnagBall?
			//this.obtainMap = Game.Level.MapName;
			//this.CatchTrainerName = Game.Player.Name;
			this.OT = Game.Player.Trainer;

			this.ObtainedMode = Method;
			this.ballUsed = Ball;
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
                if (value < 1 || value > 100) {//Experience.MAXLEVEL
                    //Game.DebugLog(string.Format("The level number {0} is invalid", value), true);
                } if (value > this.Level)
                    this.Exp.AddExperience(Experience.GetStartExperience(this.GrowthRate, value) - this.Exp.Current);
                else
                {
                    //ToDo: Not Designed to go backwards yet...
                    //Game.DebugLog(string.Format("The level number {0} is invalid", value), true);
                }
            }
		}
		/// <summary>
		/// Actual pokemon level is calaculated with <see cref="this.Level"/>.
		/// This is just a temp placeholder for Leveling-Up mechanic.
		/// </summary>
		/// ToDo: Move to platform engine
		public int TempLevel { get; private set; } 

		/// <summary>
		/// Gives the Pokémon experience points and levels it up.
		/// </summary>
		/// <param name="Exp">The amount of EXP.</param>
		/// <param name="LearnRandomAttack">If the Pokémon should learn an attack if it could learn one at level up.</param>
		public void AddExperience(int exp, bool LearnRandomAttack)
		{
			//this.Experience += exp;
			this.Exp.AddExperience(exp);
			//while (this.Exp.Current >= this.Exp.NeedExperience(this.Level + 1))
			//while (this.Exp.Current >= this.Exp.NextLevel)
			while (Level > TempLevel)
				this.LevelUp(LearnRandomAttack);
			this.TempLevel = TempLevel < 1 ? 1 : (TempLevel > Core.MAXIMUMLEVEL ? Core.MAXIMUMLEVEL : TempLevel);
			//this.TempLevel = Level.Clamp(1, Core.MAXIMUMLEVEL);
		}

		/// <summary>
		/// Rasies the Pokémon's level by one.
		/// </summary>
		/// <param name="LearnRandomAttack">If one attack of the Pokémon should be replaced by an attack potentially learned on the new level.</param>
		public void LevelUp(bool LearnRandomAttack)
		{
			this.TempLevel += 1;

			int currentMaxHP = this.TotalHP;

			//this.CalculateStats();

			// Heals the Pokémon by the HP difference.
			int HPDifference = this.TotalHP - currentMaxHP;
			if (HPDifference > 0)
				//this.Heal(HPDifference);
				HP += HPDifference;

			if (LearnRandomAttack)
				//this.LearnAttack(this.Level);
				LearnMove(TempLevel);
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
        /* ToDo: Fix this block
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
                        levels.Add((evolution as Data.PokemonEvolution<int>).EvolveValue);
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
        }*/
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
                if (_base.GenderEnum == GenderRatio.AlwaysMale) return true;
                //if (genderRate == 100f) return false; //Always female
                if (_base.GenderEnum == GenderRatio.AlwaysFemale) return false;
                if (_base.GenderEnum == GenderRatio.Genderless) return null; //genderless
                return getGender();
            }
        }

        private bool? getGender()
        {
            switch (_base.GenderEnum)
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
                    //byte n = (byte)(Core.Rand.Next(0, 100) + 1);
                    double n = (Core.Rand.NextDouble() * 100) + 1;
                    if		(_base.GenderEnum == GenderRatio.AlwaysFemale && n > 0f && n < 12.5f) return false;
                    else if (_base.GenderEnum == GenderRatio.FemaleSevenEighths && n >= 12.5f && n < 25f) return false;
                    else if (_base.GenderEnum == GenderRatio.Female75Percent && n >= 25f && n < 37.5f) return false;
                    else if (_base.GenderEnum == GenderRatio.Female75Percent && n >= 37.5f && n < 50f) return false;
                    else if (_base.GenderEnum == GenderRatio.Female50Percent && n >= 50f && n < 62.5f) return false;
                    else if (_base.GenderEnum == GenderRatio.Female50Percent && n >= 62.5f && n < 75f) return false;
                    else if (_base.GenderEnum == GenderRatio.Female25Percent && n >= 75f && n < 87.5f) return false;
                    else if (_base.GenderEnum == GenderRatio.FemaleOneEighth && n >= 87.5f && n < 100f) return false;
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
                        return _base.Ability[Core.Rand.Next(0, 3)];                            
                    }
                    else
                    {
                        return _base.Ability[Core.Rand.Next(0, 2) == 1 ? 2 : 0];
                    }
                }
                else
                {
                    return _base.Ability[Core.Rand.Next(0, 2)];
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
        /// Returns the ID of this Pokemon's nature or
        /// Sets this Pokemon's nature to a particular nature (and calculates the stat modifications).
        /// </summary>
        public Natures Nature { get; private set; }

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
		///// Could've just made the Game.NatureData[Nature] public if you needed access to it...
        //public Nature getNature()
        //{
        //    return Game.NatureData[Nature];
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
				//Don't bother to generate math for a null value; skip the process...
				if (Species == Pokemons.NONE) return false;
				//return shinyFlag ?? isShiny();
				if (shinyFlag != null && shinyFlag.HasValue) return shinyFlag.Value;
				if (Core.SHINY_WILD_POKEMON_SWITCH) return true;
				// Use this when rolling for shiny...
				// Honestly, without this math, i probably would've done something a lot more primative.
				// Look forward to primative math on wild pokemon encounter chances...
				//int a = OT == null? this.PersonalId : this.PersonalId ^ int.Parse(this.OT.PlayerID);//this.TrainerId; //Wild Pokemon TrainerId?
				//int b = a & 0xFFFF;
				//int c = (a >> 16) & 0xFFFF;
				//int d = b ^ c;
				//New Math Equation from Bulbapedia, gen 2 to 6...
				//int d = (OT.TrainerID ^ OT.SecretID) ^ (PersonalId / 65536) ^ (PersonalId % 65536);
				//int d = (Game.Player.Trainer.TrainerID ^ Game.Player.Trainer.SecretID) ^ (PersonalId / 65536) ^ (PersonalId % 65536);
				//If Pokemons are caught already `OT` -> the math should be set, else generate new values from current player
				int d = ((!OT.Equals((object)null)? OT.TrainerID : Game.Player.Trainer.TrainerID) 
					^ (!OT.Equals((object)null) ? OT.SecretID : Game.Player.Trainer.SecretID)) 
					^ ((Game.Player.Bag.GetItemAmount(Items.SHINY_CHARM) > 0 ? /*PersonalId*/Core.SHINYPOKEMONCHANCE * 3 : /*PersonalId*/Core.SHINYPOKEMONCHANCE) / 65536) 
					^ (PersonalId % 65536);
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
            if (strain <= 0 || strain >= 16) strain = Core.Rand.Next(1, 16);
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
            for (byte i = 0; i < 4; i++){//foreach(var move in this.moves){ 
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
                    return Game.PokemonMovesData[pokemons].Egg;
                case LearnMethod.levelup:
                    return Game.PokemonMovesData[pokemons].LevelUp.Where(x => x.Value <= this.Level).Select(x => x.Key).ToArray();
                case LearnMethod.machine:
                    return Game.PokemonMovesData[pokemons].Machine;
                case LearnMethod.tutor:
                    return Game.PokemonMovesData[pokemons].Tutor;
                case LearnMethod.shadow:
                case LearnMethod.xd_shadow:
                    List<Moves> s = new List<Moves>();
                    s.AddRange(Game.PokemonMovesData[pokemons].Shadow);
                    s.AddRange(Game.PokemonMovesData[pokemons].Shadow.Where(x => !s.Contains(x)).Select(x => x));
                    return s.ToArray();
                default:
                    List<Moves> list = new List<Moves>();
                    list.AddRange(Game.PokemonMovesData[pokemons].Egg);
                    list.AddRange(Game.PokemonMovesData[pokemons].Machine.Where(x => !list.Contains(x)).Select(x => x));
                    list.AddRange(Game.PokemonMovesData[pokemons].Tutor.Where(x => !list.Contains(x)).Select(x => x));
                    list.AddRange(Game.PokemonMovesData[pokemons].LevelUp.Where(x => !list.Contains(x.Key))//(x => x.Value <= this.Level)
						.Select(x => x.Key));
                    return list.ToArray();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="level"></param>
        /// <param name="egg">if moves being generated should contain egg only items</param>
        /// ToDo: Higher the pokemon's level, the greater the chances of generating a full moveset (4 moves)
        public void GenerateMoveset(int? level = null, bool egg = false)
        {
            if (level.HasValue && level.Value < 0)
                return;
            //if (!level.HasValue)
            //	level = -1;
            ClearFirstMoves();
			resetMoves();
            int numMove = Core.Rand.Next(4)+1; //number of moves pokemon will have, between 0 and 3
            List<Moves> movelist = new List<Moves>();
            if (isEgg || egg || Game.CatchPokemonsWithEggMoves) movelist.AddRange(Game.PokemonMovesData[pokemons].Egg);
			int?[] rejected = new int?[movelist.Count];
            switch (level)
            {
                #region sample from alpha version
                //Level 0 is only there so i have a sample of how version.alpha would have handled code
                case 0:
                    //Set moveset based off of the highest level moves possible.
                    //string[] moves = new string[4];
                    int i = Game.PokemonMovesData[pokemons].LevelUp.Count - 1; //work backwards so that the highest level move possible is grabbed first
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
                    movelist.AddRange(Game.PokemonMovesData[pokemons].LevelUp.Where(x => x.Value <= this.Level).Select(x => x.Key));
					rejected = new int?[movelist.Count];
                    for (int n = 0; n < movelist.Count; n++)
                    {
						bool err = false;
                        if (this.countMoves() < numMove)
                        {
							//For a truly random approach, instead of just adding moves in the order they're listed
							int x = Core.Rand.Next(movelist.Count);
							while(rejected.Contains(x))
								x = Core.Rand.Next(movelist.Count);
							rejected[n] = x;
							if (Convert.ToBoolean(Core.Rand.Next(2)))
							{
								LearnMove((Moves)movelist[x], out err);
							}
                        }
                        else
                            break;
                    }
                    break;
                default:
                    //if (isEgg || Core.CatchPokemonsWithEggMoves) movelist.AddRange(Game.PokemonMovesData[pokemons].Egg);
                    movelist.AddRange(Game.PokemonMovesData[pokemons].LevelUp.Where(x => x.Value <= level.Value).Select(x => x.Key));
					rejected = new int?[movelist.Count];
                    //int listend = movelist.Count - 4;
                    //listend = listend < 0 ? 0 : listend + 4;
                    //int j = 0; 
                    for (int n = 0; n < movelist.Count; n++)
                    {
						bool err = false;
                        if (this.countMoves() < numMove) //j
                        {
							//For a truly random approach, instead of just adding moves in the order they're listed
							int x = Core.Rand.Next(movelist.Count);
							while(rejected.Contains(x))
								x = Core.Rand.Next(movelist.Count);
							rejected[n] = x;
							if (Convert.ToBoolean(Core.Rand.Next(2)))
							{
								//this.moves[j] = new Move(movelist[n]);
								//j += 1;
								LearnMove((Moves)movelist[x], out err);
								//j += this.countMoves() < numMove ? 0 : 1;
							}
                        }
                        else
                            break;
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
            //for (int i = 0; i < _base.MoveSet.Length; i++){//foreach(var i in _base.MoveSet)
            //    if (_base.MoveSet[i].Level <= this.Level)
            //    {
            //        movelist.Add(_base.MoveSet[i].MoveId);
            //    }
            //}
            for (int i = 0; i < 4; i++)
            {
                this.moves[i] = new Move((i >= firstMoves.Count) ? 0 : firstMoves[i]);
            }
        }

        /// <summary>
        /// Silently learns the given move. Will erase the first known move if it has to.
        /// </summary>
        /// <param name="move"></param>
        /// <param name="silently"></param>
        /// <returns></returns>
		/// ToDo: Move "bool silent = false" to engine platform (requires UI) 
        public void LearnMove(Moves move, out bool success, bool silently = false)
        {
			success = false;
            if ((int)move <= 0) return;
            if (!getMoveList().Contains(move))
            {
                //Game.DebugLog("Move is not compatible");
                return;
            }
            //for (int i = 0; i < 4; i++) {
            //    if (moves[i].MoveId == move) { //Switch ordering of moves?
            //        int j = i + 1;
            //        while (j < 4) {
            //            if (moves[j].MoveId == 0) break;
            //            Move tmp = moves[j];
            //            moves[j] = moves[j - 1];
            //            moves[j - 1] = tmp;
            //            j += 1;
            //        }
            //        return;
            //    }
            //}
            if (hasMove(move))
            {
                //Game.DebugLog("Already knows move...");
                return;
            }
            for (int i = 0; i < 4; i++)
            {
                if (moves[i].MoveId == 0)
                {
                    moves[i] = new Move(move);
					success = true;
                    return;
                }
            }
            if (!silently)
			{
                //Game.DebugLog("Cannot learn move, pokmeon moveset is full", false);
			}
            else
            {
                moves[0] = moves[1];
                moves[1] = moves[2];
                moves[2] = moves[3];
                moves[3] = new Move(move);
				success = true;
            }
        }

		/// <summary>
		/// Replaces a random move of the Pokémon by one that it learns on a given level.
		/// </summary>
		/// <param name="level">The level the Pokémon learns the desired move on.</param>
		public void LearnMove(int level)
		{
			bool moveLearned = false;
			List<Moves> movelist = new List<Moves>();
			if (isEgg || Game.CatchPokemonsWithEggMoves) movelist.AddRange(Game.PokemonMovesData[pokemons].Egg);
			int?[] rejected = new int?[movelist.Count];
			//if (isEgg || Core.CatchPokemonsWithEggMoves) movelist.AddRange(Game.PokemonMovesData[pokemons].Egg);
			movelist.AddRange(Game.PokemonMovesData[pokemons].LevelUp.Where(x => x.Value == level).Select(x => x.Key));
			rejected = new int?[movelist.Count];
			//int listend = movelist.Count - 4;
			//listend = listend < 0 ? 0 : listend + 4;
			//int j = 0; 
			for (int n = 0; n < movelist.Count; n++)
			{
				if (!moveLearned) //j
				{
					//For a truly random approach, instead of just adding moves in the order they're listed
					int x = Core.Rand.Next(movelist.Count);
					while (rejected.Contains(x))
						x = Core.Rand.Next(movelist.Count);
					rejected[n] = x;
					if (Convert.ToBoolean(Core.Rand.Next(2)))
					{
						//this.moves[j] = new Move(movelist[n]);
						//j += 1;
						LearnMove((Moves)movelist[x], out moveLearned);
						//j += this.countMoves() < numMove ? 0 : 1;
					}
				}
				else
					break;
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
		/// ToDo: Should make into an Array, and use Enum to get value?
		public byte[] Contest { get; private set; }
        //public int Cool, Beauty, Cute, Smart, Tough, Sheen;
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
		public Items SwapItem(Items item)
		{
			Items old = Item;
			Item = item;
			return old;
		}

		#region Mail
		/*public void pbMoveToMailbox(pokemon)
		{
			//PokemonGlobal.mailbox = [] if !$PokemonGlobal.mailbox;
			PokemonGlobal.mailbox = !PokemonGlobal.mailbox ?? [];
			//return false if $PokemonGlobal.mailbox.length>=10
			return PokemonGlobal.mailbox.length >= 10 ?? false
				//return false if !pokemon.mail
			return !pokemon.mail ?? false
				PokemonGlobal.mailbox.push(pokemon.mail);
			pokemon.mail = null;
			return true
		}

		public void pbStoreMail(pkmn, item, message, poke1= nil, poke2= nil, poke3= nil)
		{
			//raise _INTL("Pokémon already has mail") if pkmn.mail
			raise pkmn.mail ?? _INTL("Pokémon already has mail");
			pkmn.mail = PokemonMail.new(item, message, Trainer.name, poke1, poke2, poke3)
		}

		public void pbTakeMail(pkmn)
		{
			if (!pkmn.hasItem)
			{
				//pbDisplay(_INTL("{1} isn't holding anything.",pkmn.name))
			}
			else if (!PokemonBag.pbCanStore(pkmn.item))
			{
				//pbDisplay(_INTL("The Bag is full.  The Pokémon's item could not be removed."))
			}
			elsif pkmn.mail
			{
			}

			if pbConfirm(_INTL("Send the removed mail to your PC?"))
			{
				if !pbMoveToMailbox(pkmn)
					pbDisplay(_INTL("Your PC's Mailbox is full."))

				else
					pbDisplay(_INTL("The mail was sent to your PC."))

				pkmn.setItem(0)
			}
			elsif pbConfirm(_INTL("If the mail is removed, the message will be lost.  OK?"))
			{
				pbDisplay(_INTL("Mail was taken from the Pokémon."))
				$PokemonBag.pbStoreItem(pkmn.item)
				pkmn.setItem(0)
				pkmn.mail=nil
			}
			else
			{
				$PokemonBag.pbStoreItem(pkmn.item)
				itemname=PBItems.getName(pkmn.item)

				pbDisplay(_INTL("Received the {1} from {2}.", itemname, pkmn.name))
				pkmn.setItem(0)
			}
		}

		public bool pbGiveMail(item, pkmn, pkmnid= 0)
		{
			thisitemname=PBItems.getName(item)
			if pkmn.isEgg?
			{
				pbDisplay(_INTL("Eggs can't hold items."))
				return false
			}
			if pkmn.mail
			{
				pbDisplay(_INTL("Mail must be removed before holding an item."))
				return false
			}
			if pkmn.item!=0
			{
				itemname=PBItems.getName(pkmn.item)

				pbDisplay(_INTL("{1} is already holding one {2}.\1", pkmn.name, itemname))
				if pbConfirm(_INTL("Would you like to switch the two items?"))
				{
					$PokemonBag.pbDeleteItem(item)
					if !$PokemonBag.pbStoreItem(pkmn.item)
					{
						if !$PokemonBag.pbStoreItem(item) // Compensate
						{
							raise _INTL("Can't re-store deleted item in bag")
						}

						pbDisplay(_INTL("The Bag is full.  The Pokémon's item could not be removed."))
					}
					else
					{
						if pbIsMail?(item)
						{
							if pbMailScreen(item, pkmn, pkmnid)
							{
								pkmn.setItem(item)

								pbDisplay(_INTL("The {1} was taken and replaced with the {2}.", itemname, thisitemname))
								return true
							}
							else
							{
								if !$PokemonBag.pbStoreItem(item) // Compensate
									raise _INTL("Can't re-store deleted item in bag")
								}
							}
						}
						else
						{
							pkmn.setItem(item)

							pbDisplay(_INTL("The {1} was taken and replaced with the {2}.", itemname, thisitemname))
							return true
						}
					}
				}
			else
			{
				if !pbIsMail?(item) || pbMailScreen(item, pkmn, pkmnid) // Open the mail screen if necessary
				{
					$PokemonBag.pbDeleteItem(item)
					pkmn.setItem(item)

					pbDisplay(_INTL("{1} was given the {2} to hold.", pkmn.name, thisitemname))
					return true
				}
			}
			return false
		}*/
		#endregion Mail
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
        private PokemonUnity.Inventory.Item.Mail mail { get; set; }
        /// <summary>
        /// Perform a null check; if anything other than null, there is a message
        /// </summary>
        /// ToDo: Item category
        public string Mail
        {
            get
            {
                if (this.mail == null || !PokemonUnity.Inventory.Item.Mail.IsMail(this.Item)) return null; //If empty return null
				//if (mail.Message.Length == 0 || this.Inventory == 0)//|| this.item.Category != Items.Category.Mail )
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
        public virtual string Name { get { if (this.EggSteps > 0) return "Egg"; return name ?? string.Empty;/*_base.Name;*/ } } //ToDo: Fix this

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
        ///	<see cref="Pokemons.PIKACHU"/>, 
        /// and MegaEvolutions?
        /// </remarks>
        /// ToDo: Fix Forms and uncomment
        /// Maybe a method, where when a form is changed
        /// checks pokemon value and overwrites name and stats
        /// Some forms have a SpeciesId and others are battle only
        /// Note: All of the forms appear to be registered as Enums already...
        /// Why not just change from `INT` to `(enum)Pokemons`?
        public int Form { get; set; }//{ get { return _base.Form; } set { if (value >= 0 && value <= _base.Forms) _base.Form = value; } }

        /*// <summary>
        /// Returns the species name of this Pokemon
        /// </summary>
        public string SpeciesName { get { return this._base.Species; } }*/

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
				//this.hp = (this.HP + value).Clamp(0, this.TotalHP);
				if (isFainted()) { this.Status = Status.FAINT; StatusCount = 0; } 
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
		/// <summary>
		/// Adds Effort values (EV) to this Pokémon after defeated another Pokémon, if possible.
		/// </summary>
		/// <param name="DefeatedPokemon">The defeated Pokémon.</param>
		public void GainEffort(Pokemons DefeatedPokemon)
		{
			int allEV = EV[(int)Stats.HP] + EV[(int)Stats.ATTACK] + EV[(int)Stats.DEFENSE] + EV[(int)Stats.SPATK] + EV[(int)Stats.SPDEF] + EV[(int)Stats.SPEED];
			if (allEV >= EVLIMIT)
				return;

			int maxEVgain = EVLIMIT - allEV;
			int totalEVgain = 0;

			// EV gains
			int gainEVHP = Game.PokemonData[DefeatedPokemon].evYieldHP;
			int gainEVAttack = Game.PokemonData[DefeatedPokemon].evYieldATK;
			int gainEVDefense = Game.PokemonData[DefeatedPokemon].evYieldDEF;
			int gainEVSpAttack = Game.PokemonData[DefeatedPokemon].evYieldSPA;
			int gainEVSpDefense = Game.PokemonData[DefeatedPokemon].evYieldSPD;
			int gainEVSpeed = Game.PokemonData[DefeatedPokemon].evYieldSPE;

			int EVfactor = 1;

			var itemNumber = 0;
			if (Item != Items.NONE)
				itemNumber = (int)Item;

			switch (itemNumber) //ToDo: Redo with Item.Enum, and correct id values below
			{
				case 581:
					{
						EVfactor *= 2;
						break;
					}
				case 582:
					{
						gainEVHP += 8;
						break;
					}
				case 583:
					{
						gainEVAttack += 8;
						break;
					}
				case 584:
					{
						gainEVDefense += 8;
						break;
					}
				case 585:
					{
						gainEVSpAttack += 8;
						break;
					}
				case 586:
					{
						gainEVSpDefense += 8;
						break;
					}
				case 587:
					{
						gainEVSpeed += 8;
						break;
					}
			}

			// HP gain
			if ((gainEVHP > 0 && EV[(int)Stats.HP] < EVSTATLIMIT && maxEVgain - totalEVgain > 0))
			{
				gainEVHP *= EVfactor;
				gainEVHP = gainEVHP < 0 ? 0 : (gainEVHP > EVSTATLIMIT - EV[(int)Stats.HP] ? EVSTATLIMIT - EV[(int)Stats.HP] : gainEVHP);
				gainEVHP = gainEVHP < 0 ? 0 : (gainEVHP > maxEVgain - totalEVgain ? maxEVgain - totalEVgain : gainEVHP);
				//gainEVHP = MathHelper.Clamp(gainEVHP, 0, EVSTATLIMIT - EV[(int)Stats.HP]);
				//gainEVHP = MathHelper.Clamp(gainEVHP, 0, maxEVgain - totalEVgain);
				EV[(int)Stats.HP] += (byte)gainEVHP;
				totalEVgain += gainEVHP;
			}

			// Attack gain
			if ((gainEVAttack > 0 && EV[(int)Stats.ATTACK] < EVSTATLIMIT && maxEVgain - totalEVgain > 0))
			{
				gainEVAttack *= EVfactor;
				gainEVAttack = gainEVAttack < 0 ? 0 : (gainEVAttack > EVSTATLIMIT - EV[(int)Stats.ATTACK] ? EVSTATLIMIT - EV[(int)Stats.ATTACK] : gainEVAttack);
				gainEVAttack = gainEVAttack < 0 ? 0 : (gainEVAttack > maxEVgain - totalEVgain ? maxEVgain - totalEVgain : gainEVAttack);
				//gainEVAttack = MathHelper.Clamp(gainEVAttack, 0, EVSTATLIMIT - EV[(int)Stats.ATTACK]);
				//gainEVAttack = MathHelper.Clamp(gainEVAttack, 0, maxEVgain - totalEVgain);
				EV[(int)Stats.ATTACK] += (byte)gainEVAttack;
				totalEVgain += gainEVAttack;
			}

			// Defense gain
			if ((gainEVDefense > 0 && EV[(int)Stats.DEFENSE] < EVSTATLIMIT && maxEVgain - totalEVgain > 0))
			{
				gainEVDefense *= EVfactor;
				gainEVDefense = gainEVDefense < 0 ? 0 : (gainEVDefense > EVSTATLIMIT - EV[(int)Stats.DEFENSE] ? EVSTATLIMIT - EV[(int)Stats.DEFENSE] : gainEVDefense);
				gainEVDefense = gainEVDefense < 0 ? 0 : (gainEVDefense > maxEVgain - totalEVgain ? maxEVgain - totalEVgain : gainEVDefense);
				//gainEVDefense = MathHelper.Clamp(gainEVDefense, 0, EVSTATLIMIT - EV[(int)Stats.DEFENSE]);
				//gainEVDefense = MathHelper.Clamp(gainEVDefense, 0, maxEVgain - totalEVgain);
				EV[(int)Stats.DEFENSE] += (byte)gainEVDefense;
				totalEVgain += gainEVDefense;
			}

			// SpAttack gain
			if ((gainEVSpAttack > 0 && EV[(int)Stats.SPATK] < EVSTATLIMIT && maxEVgain - totalEVgain > 0))
			{
				gainEVSpAttack *= EVfactor;
				gainEVSpAttack = gainEVSpAttack < 0 ? 0 : (gainEVSpAttack > EVSTATLIMIT - EV[(int)Stats.SPATK] ? EVSTATLIMIT - EV[(int)Stats.SPATK] : gainEVSpAttack);
				gainEVSpAttack = gainEVSpAttack < 0 ? 0 : (gainEVSpAttack > maxEVgain - totalEVgain ? maxEVgain - totalEVgain : gainEVSpAttack);
				//gainEVSpAttack = MathHelper.Clamp(gainEVSpAttack, 0, EVSTATLIMIT - EV[(int)Stats.SPATK]);
				//gainEVSpAttack = MathHelper.Clamp(gainEVSpAttack, 0, maxEVgain - totalEVgain);
				EV[(int)Stats.SPATK] += (byte)gainEVSpAttack;
				totalEVgain += gainEVSpAttack;
			}

			// SpDefense gain
			if ((gainEVSpDefense > 0 && EV[(int)Stats.SPDEF] < EVSTATLIMIT && maxEVgain - totalEVgain > 0))
			{
				gainEVSpDefense *= EVfactor;
				gainEVSpDefense = gainEVSpDefense < 0 ? 0 : (gainEVSpDefense > EVSTATLIMIT - EV[(int)Stats.SPDEF] ? EVSTATLIMIT - EV[(int)Stats.SPDEF] : gainEVSpDefense);
				gainEVSpDefense = gainEVSpDefense < 0 ? 0 : (gainEVSpDefense > maxEVgain - totalEVgain ? maxEVgain - totalEVgain : gainEVSpDefense);
				//gainEVSpDefense = MathHelper.Clamp(gainEVSpDefense, 0, EVSTATLIMIT - EV[(int)Stats.SPDEF]);
				//gainEVSpDefense = MathHelper.Clamp(gainEVSpDefense, 0, maxEVgain - totalEVgain);
				EV[(int)Stats.SPDEF] += (byte)gainEVSpDefense;
				totalEVgain += gainEVSpDefense;
			}

			// Speed gain
			if ((gainEVSpeed > 0 && EV[(int)Stats.SPEED] < EVSTATLIMIT && maxEVgain - totalEVgain > 0))
			{
				gainEVSpeed *= EVfactor;
				gainEVSpeed = gainEVSpeed < 0 ? 0 : (gainEVSpeed > EVSTATLIMIT - EV[(int)Stats.SPEED] ? EVSTATLIMIT - EV[(int)Stats.SPEED] : gainEVSpeed);
				gainEVSpeed = gainEVSpeed < 0 ? 0 : (gainEVSpeed > maxEVgain - totalEVgain ? maxEVgain - totalEVgain : gainEVSpeed);
				//gainEVSpeed = MathHelper.Clamp(gainEVSpeed, 0, EVSTATLIMIT - EV[(int)Stats.SPEED]);
				//gainEVSpeed = MathHelper.Clamp(gainEVSpeed, 0, maxEVgain - totalEVgain);
				EV[(int)Stats.SPEED] += (byte)gainEVSpeed;
				totalEVgain += gainEVSpeed;
			}
		}

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

		#region Unity Engine Stuff...
		/*ToDo: Move to unity...
		/// <summary>
		/// Plays the cry of this Pokémon.
		/// </summary>
		public void PlayCry()
		{
			float Pitch = 0.0f;
			int percent = 100;
			if (this.HP > 0 & this.TotalHP > 0)
				percent = System.Convert.ToInt32(Math.Ceiling(this.HP / (double)this.TotalHP) * 100);

			if (percent <= 50)
				Pitch = -0.4f;
			if (percent <= 15)
				Pitch = -0.8f;
			if (percent == 0)
				Pitch = -1.0f;

			//SoundManager.PlayPokemonCry(this.Species, Pitch, 0F);
		}*/
		#endregion
    }
    public interface IPokemonEvolution
    {
        Pokemons Species { get; }

        EvolutionMethod EvolveMethod { get; }
    }
}