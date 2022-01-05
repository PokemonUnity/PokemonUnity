using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Attack;
using PokemonUnity.Utility;
using PokemonUnity.Inventory;
using PokemonUnity.Monster;
using PokemonUnity.Character;
using PokemonUnity.Monster.Data;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.Item;

namespace PokemonUnity.Monster
{
	public partial class Pokemon : IPokemon, IEquatable<Pokemon>, IEqualityComparer<Pokemon>
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
		public int[] IV { get; private set; }
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
		/// Consumed held item (used in battle only)
		/// </summary>
		public Items itemRecycle { get; set; }
		/// <summary>
		/// Resulting held item (used in battle only)
		/// </summary>
		public Items itemInitial { get; set; }
		/// <summary>
		/// Where Pokemon can use Belch (used in battle only)
		/// </summary>
		public bool belch { get; set; }
		public Experience Experience { get; private set; }
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
		public Status Status { get; set; }
		/// <summary>
		/// Sleep count/Toxic flag
		/// </summary>
		/// ToDo: Add to Status Class or StatusTurn() method
		public int StatusCount { get; set; }
		/// <summary>
		/// Steps to hatch egg, 0 if Pokemon is not an egg.
		/// Pokemon moves auto reset when egg counter reaches 0.
		/// Double as Friendship Meter after pokemon is hatched.
		/// </summary>
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
					if (OT != null)
					{
						ObtainedMode = ObtainedMethod.EGG;
						hatchedWhen = new DateTimeOffset(DateTime.UtcNow);
					}
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
		public IMove[] moves { get; private set; }
		/// <summary>
		/// The moves known when this Pokemon was obtained
		/// </summary>
		private IList<Moves> firstMoves { get; set; }
		/// <summary>
		/// List of moves that this pokemon has learned, and is capable of relearning
		/// </summary>
		public Moves[] MoveArchive { get { return firstMoves.ToArray(); } }
		/// <summary>
		/// Ball used
		/// </summary>
		public Items ballUsed { get; set; }
		protected Data.PokemonData _base { get { return Game.PokemonData[Form.Pokemon]; } }
		/// <summary>
		/// Max total EVs
		/// </summary>
		public const int EVLIMIT = 510;
		/// <summary>
		/// Max EVs that a single stat can have
		/// </summary>
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
		/// ToDo: Make Private
		public Pokemon()
		{
			//_base = PokemonData.GetPokemon(Pokemons.NONE);
			pokemons = Pokemons.NONE;
			PersonalId = Core.Rand.Next(256);
			PersonalId |= Core.Rand.Next(256) << 8;
			PersonalId |= Core.Rand.Next(256) << 16;
			PersonalId |= Core.Rand.Next(256) << 24;
			Ability = Abilities.NONE;
			Nature = (Natures)(Core.Rand.Next(Kernal.NatureData.Count));//Game.NatureData.Keys.ToArray()[Core.Rand.Next(Game.NatureData.Keys.Count) + 1];
			shinyFlag = IsShiny; //isShiny(); 
			//Gender = isMale();
			//IV = new int[] { 10, 10, 10, 10, 10, 10 };
			IV = new int[] { (int)(Core.Rand.Next(33)), (int)(Core.Rand.Next(33)), (int)(Core.Rand.Next(33)), (int)(Core.Rand.Next(33)), (int)(Core.Rand.Next(33)), (int)(Core.Rand.Next(33)) };
			EV = new byte[6];
			Contest = new byte[6];
			Experience = new Experience(GrowthRate);
			firstMoves = new List<Moves>();
			moves = new Move[4] { new Move(Moves.NONE), new Move(Moves.NONE), new Move(Moves.NONE), new Move(Moves.NONE) };
			pokerus = new int[2];
			Markings = new bool[6]; //{ false, false, false, false, false, false };
			Status = Status.NONE;
			StatusCount = 0;
			ballUsed = Items.NONE;
			Item = Items.NONE;
			ribbons = new List<Ribbons>();
			//calcStats();
			//if (Game.GameData.GameMap != null)
			//{
			//	@ObtainMap = (Locations)Game.GameData.GameMap.map_id;
			//	//@ObtainText = null;
			//	@ObtainLevel = Level;
			//}
			//else
			//{
			//	@ObtainMap = 0;
			//	//@ObtainText = null;
			//	@ObtainLevel = Level;
			//}
			@ObtainedMode = ObtainedMethod.MET;   // Met
			//if (Game.GameData.GameSwitches != null && Game.GameData.GameSwitches[Core.FATEFUL_ENCOUNTER_SWITCH])
			if (Core.FATEFUL_ENCOUNTER_SWITCH) 
				@ObtainedMode = ObtainedMethod.FATEFUL_ENCOUNTER;
		}

		/// <summary>
		/// Uses PokemonData to initialize a Pokemon from base stats
		/// </summary>
		/// <param name="pokemon"></param>
		public Pokemon(Pokemons pokemon) : this()
		{
			//_base = PokemonData.GetPokemon(pokemon);
			pokemons = pokemon;
			eggSteps = 0;
			Ability = abilityFlag;
			Gender = gender; //GenderRatio.//Pokemon.PokemonData.GetPokemon(pokemon).MaleRatio
			if (Core.Rand.Next(65356) < Core.POKERUSCHANCE) GivePokerus();//pokerus
			Heal();
			//if (pokemons == Pokemons.UNOWN)
			//	formId = Core.Rand.Next(Game.PokemonFormsData[pokemons].Length);
			#region Initialize Forms
			Forms? f = MultipleForms.getFormOnCreation(pokemons, false);
			if (f != null)
			{
				//this.form = f;
				SetForm(f.Value);
				this.resetMoves();
			//} else
			//{
			//	SetForm((Forms)pokemons);
			//	this.resetMoves();
			}
			#endregion
			//ToDo: Move to Trainer.Wild Pokemon
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
		public Pokemon(Pokemons pkmn, bool isEgg) : this(pkmn) { if (isEgg) eggSteps = _base.HatchTime; }

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
		public Pokemon(Pokemons pkmn, byte level, bool isEgg = false) : this(pkmn, isEgg) { Level = level; GenerateMoveset(); Heal(); }

		/// <summary>
		/// Use this constructor when creating battle pokemon, for NPC Trainers
		/// </summary>
		/// <param name="pkmn"></param>
		/// <param name="level"></param>
		/// <param name="moves"></param>
		/// <param name="ivs">Increases difficulty be adding additional stats</param>
		/// <param name="pokeball">Pokeball used for animation</param>
		/// <param name="obtain">not sure why this matters...</param>
		/// <param name="nickname">Name to be displayed</param>
		public Pokemon(Pokemons pkmn, byte level, Move[] moves, int ivs = 0, Items pokeball = Items.POKE_BALL, ObtainedMethod obtain = ObtainedMethod.MET, string nickname = null)
			: this(pkmn, level: level, isEgg: false)
		//: this(
		//species: pkmn.Species,
		//original: Game.Player.Trainer,
		//nickName: nickname, form: pkmn.Form,
		//ability: pkmn.Ability, nature: pkmn.Nature,
		//isShiny: pkmn.IsShiny, gender: pkmn.Gender,
		//pokerus: pkmn.Pokerus, ishyper: pkmn.isHyperMode,
		//shadowLevel: pkmn.ShadowLevel, currentHp: pkmn.HP,
		//item: pkmn.Item, iv: pkmn.IV, ev: pkmn.EV,
		//obtainedLevel: pkmn.Level, currentExp: pkmn.Exp.Current,
		//happiness: pkmn.Happiness, status: pkmn.Status,
		//statusCount: pkmn.StatusCount, eggSteps: pkmn.EggSteps,
		//ballUsed: pokeball, mail: pkmn.Mail, moves: pkmn.moves,
		//ribbons: pkmn.Ribbons.ToArray(), markings: pkmn.Markings,
		//personalId: pkmn.PersonalId, obtainedMethod: obtain,
		//timeReceived: DateTimeOffset.Now, timeEggHatched: null)
		{
			if (pokeball == Items.NONE
				|| Game.ItemData[pokeball].Category == ItemCategory.STANDARD_BALLS
				|| Game.ItemData[pokeball].Category == ItemCategory.SPECIAL_BALLS
				|| Game.ItemData[pokeball].Category == ItemCategory.APRICORN_BALLS)
				this.ballUsed = pokeball;
			if (moves.Length == 4) this.moves = moves;
			this.name = nickname;
			IV = Utility.MathHelper.randSum(IV.Length, ivs);
		}

		/// <summary>
		/// Instializes a new Pokemon, with values at default. 
		/// Pokemon is created at the level assigned in parameter, 
		/// with all stats randomly generated/assigned (new roll).
		/// </summary>
		/// <param name="pkmn">Pokemon being generated</param>
		/// <param name="original">Assigns original <see cref="TrainerData"/> 
		/// of this pokemon. 
		/// Affects ability to command pokemon, if player is not OT</param>
		/// <param name="level">Level this pokemon start ats</param>
		/// <remarks>
		/// I think this is only need for tested, not sure of purpose inside framework
		/// </remarks>
		public Pokemon(Pokemons pkmn, ITrainer original, byte level = Core.EGGINITIALLEVEL) : this(pkmn, level: level, isEgg: false) { OT = original; }

		[System.Obsolete("Sample code for inspiration")]
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
			Natures TPNATURE = (Natures)0, //Natures.UNSET,
			int[] TPIV = null, //new int[6] { 10, 10, 10, 10, 10, 10 },
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
			blob = Convert.FromBase64String('J2ZqBgAAICQHAAAAkOaKyTACAABGLAABAAAAAAAAAAAAAAAAA'
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
			ITrainer original,
			string nickName, int form,
			Abilities ability, Natures nature,
			bool isShiny, bool? gender,
			int[] pokerus, int heartSize, //bool ishyper,
			int? shadowLevel,
			int currentHp, Items item,
			int[] iv, byte[] ev,
			int obtainedLevel, /*int currentLevel,*/ int currentExp,
			int happiness, Status status, int statusCount,
			int eggSteps, Items ballUsed,
			string mail, IMove[] moves, Moves[] history,
			Ribbons[] ribbons, bool[] markings,
			int personalId,
			ObtainedMethod obtainedMethod,
			DateTimeOffset timeReceived, DateTimeOffset? timeEggHatched) : this(species, original)
		{
			//Check to see if nickName is filled
			if (!string.IsNullOrEmpty(nickName))
				name = nickName.Trim();
			else
				name = null;

			//_base = Pokemon.PokemonData.GetPokemon(species);
			pokemons = species;
			formId = form;

			Ability = ability;
			Nature = nature;

			PersonalId = personalId;

			shinyFlag = isShiny;
			Gender = gender;

			this.pokerus = pokerus;

			//isHyperMode = ishyper;
			HeartGuageSize = heartSize;
			ShadowLevel = shadowLevel;

			IV = iv;
			EV = ev;

			ObtainLevel = obtainedLevel;
			//Level = currentLevel;
			//Experience.AddExperience(currentExp - Experience.Current);
			Exp = currentExp;

			//Current Hp cant be greater than Max hp, 
			//Level up pokemon first using exp points
			HP = currentHp; 
			Item = item;

			Happiness = happiness;

			Status = status;
			StatusCount = statusCount;

			EggSteps = eggSteps;

			this.ballUsed = ballUsed;
			if (Game.ItemData[item].IsLetter)
			{
				this.mail = new Inventory.Mail((Items)item);
				this.mail.Message = mail;
			}

			this.moves = moves;
			firstMoves = new List<Moves>(history);

			this.ribbons = ribbons.ToList();
			Markings = markings;

			ObtainedMode = obtainedMethod;
			obtainWhen = timeReceived;
			hatchedWhen = timeEggHatched;
		}

		[System.Obsolete("Use `SetCatchInfos()`?")]
		/// <summary>
		/// Use this constructor when capturing wild pokemons from a battle
		/// </summary>
		/// <param name="pkmn"></param>
		/// <param name="pokeball"></param>
		/// <param name="obtain"></param>
		/// <param name="nickname"></param>
		public Pokemon(Pokemon pkmn, Items pokeball, ObtainedMethod obtain = ObtainedMethod.MET, string nickname = null)
			: this(
			species: pkmn.Species,
			original: Game.GameData.Trainer,
			nickName: nickname, form: pkmn.FormId,
			ability: pkmn.Ability, nature: pkmn.Nature,
			isShiny: pkmn.IsShiny, gender: pkmn.Gender,
			pokerus: pkmn.Pokerus, heartSize: pkmn.HeartGuageSize, //ishyper: pkmn.isHyperMode,
			shadowLevel: pkmn.ShadowLevel, currentHp: pkmn.HP,
			item: pkmn.Item, iv: pkmn.IV, ev: pkmn.EV,
			obtainedLevel: pkmn.Level, currentExp: pkmn.Exp,
			happiness: pkmn.Happiness, status: pkmn.Status,
			statusCount: pkmn.StatusCount, eggSteps: pkmn.EggSteps,
			ballUsed: pokeball, mail: pkmn.Mail, moves: pkmn.moves, history: null,
			ribbons: pkmn.Ribbons.ToArray(), markings: pkmn.Markings,
			personalId: pkmn.PersonalId, obtainedMethod: obtain,
			timeReceived: DateTimeOffset.Now, timeEggHatched: null)
		{
			//ToDo: What to do about timeEggHatched and OT
			//Should make a new class for trading pokemons?
		}
		#endregion

#pragma warning disable 0162 //Warning CS0162  Unreachable code detected 
		#region Ownership, obtained information
		/// <summary>
		/// Manner Obtained:
		/// </summary>
		public ObtainedMethod ObtainedMode { get; private set; }
		//ToDo: Change to EncounterType
		public enum ObtainedMethod
		{
			/// <summary>
			/// Stole from another trainer, during battle
			/// </summary>
			SNAGGED = -1,
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
		/// </remarks>
		public Locations ObtainMap { get; private set; }
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
		public ITrainer OT { get; private set; }
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
		public bool isForeign(ITrainer trainer)
		{
			return trainer != this.OT;
		}
		//public bool isForeign(IGamePlayer trainer)
		//{
		//	return trainer.Trainer != this.OT;
		//}

		/// <summary>
		/// Returns the public portion of the original trainer's ID
		/// </summary>
		/// <returns></returns>
		public string TrainerId
		{
			get { return OT != null ? OT.publicID().ToString().PadLeft(5) : "00000"; }
		}

		/// <summary>
		/// Returns this Pokemon's level when this Pokemon was obtained
		/// </summary>
		/// <returns></returns>
		/// <remarks>
		/// Wouldnt this change again when traded to another trainer?
		/// </remarks>
		public int ObtainLevel
		{
			//get { if (obtainLevel == null) obtainLevel = 0; return obtainLevel; }
			get; private set;
		}

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
					//throw new Exception("Trainer did not acquire Pokemon as an egg.");
					return null;
			}
			//set { this.hatchedWhen = value; }
		}

		/// <summary>
		/// Sets the catch infos of the Pokémon. Uses the current map name and player name + OT.
		/// </summary>
		/// <param name="Ball">The Pokéball this Pokémon got captured in.</param>
		/// <param name="Method">The capture method.</param>
		public Pokemon SetCatchInfos(ITrainer Trainer, Items Ball = Items.POKE_BALL, ObtainedMethod Method = ObtainedMethod.MET)
		{
			//ToDo: If OT != null, dont change it... Pokemon is already captured... Unless Pokeball.SnagBall?
			//this.obtainMap = Game.GameData.Level.MapName;
			//this.CatchTrainerName = Game.GameData.Player.Name;
			//this.OT = Game.GameData.Player.Trainer;
			this.OT = Trainer;
			this.ObtainLevel = Level;
			this.ObtainedMode = Method;
			this.obtainWhen = DateTimeOffset.UtcNow;
			if (Game.ItemData[Ball].Pocket == ItemPockets.POKEBALL)
				this.ballUsed = Ball;
			else this.ballUsed = Items.POKE_BALL;

			pbRecordFirstMoves();
			return this;
		}
		#endregion

		#region Level
		/// <summary>
		/// Current experience points. 
		/// Total accumulated - Level minimum
		/// </summary>
		/// <example>
		/// lv1->lv2=5xp
		/// lv2->lv3=10xp
		/// if pokemon is lvl 3 and 0xp, it should have a total of 15xp
		/// but display counter should still say 0
		/// </example>
		private int _Exp
		{
			//get
			//{
			//	return this.Experience.Current;
			//}
			set 
			{
				if (value < 0) //|| value > this.Experience.GetMaxExperience(this.GrowthRate)
					GameDebug.LogError(string.Format("The experience number {0} is invalid", value));
				else //(value < this.Experience.GetMaxExperience(this.GrowthRate))
					//this.Experience.AddExperience(value - this.Experience.Current);
					Experience = new Experience(this.GrowthRate, value);
			}
		}

		/// <summary>
		/// </summary>
		public int Level
		{
			get
			{
				//return 1;
				return Experience.GetLevelFromExperience(this.GrowthRate, this.Experience.Total);
			}
			private set
			{
				if (value < 1 || value > Core.MAXIMUMLEVEL)
					GameDebug.LogError(string.Format("The level number {0} is invalid", value));
				if (value > this.Level) { 
					GameDebug.Log(string.Format("Pokemon level manually changed to {0}", value));
					//this.Experience.AddExperience(Experience.GetStartExperience(this.GrowthRate, value) - this.Experience.Total);
					Exp = Experience.GetStartExperience(this.GrowthRate, value);
				}
				else
				{
					GameDebug.LogWarning(string.Format("The level number has gone backwards and experience points is reset"));
					Exp = Experience.GetStartExperience(this.GrowthRate, value);
				}
			}
		}

		public void SetLevel (byte level) { Level = level; }

		/// <summary>
		/// Gives the Pokémon experience points and levels it up.
		/// </summary>
		/// <param name="Exp">The amount of EXP.</param>
		/// <param name="LearnRandomAttack">If the Pokémon should learn an attack if it could learn one at level up.</param>
		//public void AddExperience(int exp, bool LearnRandomAttack)
		//{
		//	//this.Experience += exp;
		//	this.Experience.AddExperience(exp);
		//	//while (this.Exp.Current >= this.Exp.NeedExperience(this.Level + 1))
		//	//while (this.Exp.Current >= this.Exp.NextLevel)
		//	//while (Level > TempLevel)
		//	//	this.LevelUp(LearnRandomAttack); //ToDo: Trigger EventHandler here
		//	//this.TempLevel = TempLevel < 1 ? 1 : (TempLevel > Core.MAXIMUMLEVEL ? Core.MAXIMUMLEVEL : TempLevel);
		//	//this.TempLevel = Level.Clamp(1, Core.MAXIMUMLEVEL);
		//}

		/// <summary>
		/// Rasies the Pokémon's level by one.
		/// </summary>
		/// <param name="LearnRandomAttack">If one attack of the Pokémon should be replaced by an attack potentially learned on the new level.</param>
		//public void LevelUp(bool LearnRandomAttack)
		//{
		//	this.TempLevel += 1;
		//	int currentMaxHP = this.TotalHP;
		//	bool success;
		//	//this.CalculateStats();
		//
		//	// Heals the Pokémon by the HP difference.
		//	int HPDifference = this.TotalHP - currentMaxHP;
		//	if (HPDifference > 0)
		//		//this.Heal(HPDifference);
		//		HP += HPDifference;
		//
		//	if (LearnRandomAttack)
		//		//this.LearnAttack(this.Level);
		//		LearnMove(GetMove(TempLevel), out success);
		//	ChangeHappiness(HappinessMethods.LEVELUP);
		//}

		/// <summary>
		/// Returns whether this Pokemon is an egg.
		/// </summary>
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
				return _base.GrowthRate;
			}
		}

		/// <summary>
		/// Returns this Pokemon's base Experience value.
		/// </summary>
		/// When this pokemon is defeated, this is the amount of experience points it offers
		//public int baseExp
		//{
		//    get
		//    {
		//        return _base.BaseExpYield; //Check Pokemon.Form too...
		//    }
		//}

		/// <summary>
		/// Use this with an event handler, that adds a step on event trigger
		/// </summary>
		/// <param name="steps"></param>
		public void AddSteps(byte steps = 1)
		{
			int i = EggSteps - steps;
			//Set EggSteps to 0 first, to trigger the hatching process... 
			EggSteps = i < 0 ? 0 : i;
			//then if we want to continue beyond 0 and into negative values, we may continue...
			if (i < 0) EggSteps = i;
			ChangeHappiness(HappinessMethods.WALKING);
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
			if (Game.PokemonEvolutionsData[pokemons].Length > 0)
			{
				List<int> levels = new List<int>();
				foreach (PokemonEvolution evolution in Game.PokemonEvolutionsData[pokemons])
				{
					if (evolution.EvolveMethod == EvolutionMethod.Level ||
						evolution.EvolveMethod == EvolutionMethod.LevelMale ||
						evolution.EvolveMethod == EvolutionMethod.LevelFemale)
					{
						levels.Add((int)evolution.EvolveValue);
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
			foreach (PokemonEvolution item in Game.PokemonEvolutionsData[pokemons])
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
			foreach (PokemonEvolution item in Game.PokemonEvolutionsData[pokemons])
			{
				if (!methods.Contains(item.EvolveMethod))
					methods.Add(item.EvolveMethod);
			}
			return methods.ToArray();
		}
		public bool hasEvolveMethod(EvolutionMethod method)
		{
			foreach (PokemonEvolution item in Game.PokemonEvolutionsData[pokemons])
			{
				if (item.EvolveMethod == method)
					return true;
			}
			return false;
		}
		/// <summary>
		/// Returns an array of all the levels this pokemons has to reach in order to evolve into species.
		/// Best if used in combination with <see cref="CanEvolveDuringBattle"/>.
		/// </summary>
		/// returns an array of all the species this pokemon can currently evolve into
		public Pokemons[] CanEvolveAfter(EvolutionMethod method)
		{
			if (!hasEvolveMethod(method))
				return new Pokemons[0];
			List<Pokemons> methods = new List<Pokemons>();
			switch (method)
			{
				case EvolutionMethod.Ninjask:
					foreach (PokemonEvolution item in Game.PokemonEvolutionsData[pokemons])
					{
						if (item.EvolveMethod == method)
							methods.Add(item.Species);
					}
					return methods.ToArray();
				default:
					return new Pokemons[0];
			}
		}
		public Pokemons[] CanEvolveAfter(EvolutionMethod method, int level)
		{
			if (!hasEvolveMethod(method))
				return new Pokemons[0];
			List<Pokemons> methods = new List<Pokemons>();
			switch (method)
			{
				case EvolutionMethod.Level:
				case EvolutionMethod.LevelMale:
				case EvolutionMethod.LevelFemale:
				//case EvolutionMethod.Ninjask:
				case EvolutionMethod.Lycanroc:
					foreach (PokemonEvolution item in Game.PokemonEvolutionsData[pokemons])
					{
						if ((int)item.EvolveValue < level && item.EvolveMethod == method)
							methods.Add(item.Species);
					}
					return methods.ToArray();
				case EvolutionMethod.Beauty:            //param int = beauty, not level
					foreach (PokemonEvolution item in Game.PokemonEvolutionsData[pokemons])
					{
						if ((int)item.EvolveValue < Contest[(int)Contests.Beauty] && item.EvolveMethod == method)
							methods.Add(item.Species);
					}
					return methods.ToArray();
				case EvolutionMethod.Hatred:            //param int = happiness, not level
				case EvolutionMethod.Happiness:         //param int = happiness, not level
				case EvolutionMethod.HappinessDay:      //param int = happiness, not level
				case EvolutionMethod.HappinessNight:    //param int = happiness, not level
					foreach (PokemonEvolution item in Game.PokemonEvolutionsData[pokemons])
					{
						if ((int)item.EvolveValue < Happiness && item.EvolveMethod == method)
							methods.Add(item.Species);
					}
					return methods.ToArray();
				default:
					return new Pokemons[0];
			}
		}
		public Pokemons[] CanEvolveAfter(EvolutionMethod method, Items itemUsed)
		{
			if (!hasEvolveMethod(method))
				return new Pokemons[0];
			switch (method)
			{
				case EvolutionMethod.Item:
				case EvolutionMethod.ItemMale:
				case EvolutionMethod.ItemFemale:
				case EvolutionMethod.TradeItem:
				case EvolutionMethod.HoldItem:
				case EvolutionMethod.HoldItemDay:
				case EvolutionMethod.HoldItemNight:
					List<Pokemons> methods = new List<Pokemons>();
					foreach (PokemonEvolution item in Game.PokemonEvolutionsData[pokemons])
					{
						if ((Items)item.EvolveValue == itemUsed && item.EvolveMethod == method)
							methods.Add(item.Species);
					}
					return methods.ToArray();
				default:
					return new Pokemons[0];
			}
		}
		public Pokemons[] CanEvolveAfter(EvolutionMethod method, Pokemons pkmn)
		{
			if (!hasEvolveMethod(method))
				return new Pokemons[0];
			switch (method)
			{
				case EvolutionMethod.Party:
				case EvolutionMethod.TradeSpecies:
				case EvolutionMethod.Shedinja:
					List<Pokemons> methods = new List<Pokemons>();
					foreach (PokemonEvolution item in Game.PokemonEvolutionsData[pokemons])
					{
						if ((Pokemons)item.EvolveValue == pkmn && item.EvolveMethod == method)
							methods.Add(item.Species);
					}
					return methods.ToArray();
				default:
					return new Pokemons[0];
			}
		}
		public Pokemons[] CanEvolveAfter(EvolutionMethod method, Moves move)
		{
			if (!hasEvolveMethod(method))
				return new Pokemons[0];
			switch (method)
			{
				case EvolutionMethod.Move:
					List<Pokemons> methods = new List<Pokemons>();
					foreach (PokemonEvolution item in Game.PokemonEvolutionsData[pokemons])
					{
						if ((Moves)item.EvolveValue == move && item.EvolveMethod == method)
							methods.Add(item.Species);
					}
					return methods.ToArray();
				default:
					return new Pokemons[0];
			}
		}
		public Pokemons[] CanEvolveAfter(EvolutionMethod method, Types type)
		{
			if (!hasEvolveMethod(method))
				return new Pokemons[0];
			switch (method)
			{
				case EvolutionMethod.Type:
				case EvolutionMethod.Affection:
					List<Pokemons> methods = new List<Pokemons>();
					foreach (PokemonEvolution item in Game.PokemonEvolutionsData[pokemons])
					{
						if ((Types)item.EvolveValue == type && item.EvolveMethod == method)
							methods.Add(item.Species);
					}
					return methods.ToArray();
				default:
					return new Pokemons[0];
			}
		}
		//public void EvolvePokemon(Pokemons evolveTo, PokemonEssentials.Interface.Screen.IPokeBattle_Scene scene = null)
		//{
		//	List<Pokemons> methods = new List<Pokemons>();
		//	foreach (PokemonEvolution item in Game.PokemonEvolutionsData[pokemons])
		//		methods.Add(item.Species);
		//	if (!methods.Contains(evolveTo)) return;
		//	int oldtotalhp	= TotalHP;
		//	int oldattack	= ATK;
		//	int olddefense	= DEF;
		//	int oldspeed	= SPE;
		//	int oldspatk	= SPA;
		//	int oldspdef	= SPD;
		//	pokemons		= evolveTo;
		//	
		//	if(scene != null)
		//	{
		//		calcStats(); //Refresh HP
		//		scene.pbRefresh();
		//		//ToDo: sort out issues here
		//		Game.GameData.pbDisplayPaused(Game._INTL("{1} evolved to {2}!", Name, Species.ToString(TextScripts.Name)));
		//		scene.pbLevelUp(this, null,//battler, 
		//			oldtotalhp, oldattack, olddefense, oldspeed, oldspatk, oldspdef);
		//	}
		//}
		#endregion

		#region Gender
		public virtual bool IsMale { get { return Gender == true; } }
		public virtual bool IsFemale { get { return Gender == false; } }
		public virtual bool IsGenderless { get { return Gender == null; } }
		public virtual bool IsSingleGendered { get { return _base.IsSingleGendered; } }
		/// <summary>
		/// Returns this Pokemons gender.
		/// Sets this Pokemon's gender to a particular gender (if possible)
		/// True is Male; False is Female; Null is Genderless.
		/// </summary>
		/// <remarks>
		/// Should consider gender as byte? bool takes up same amount of space
		/// </remarks>
		/// isMale; null = genderless?
		public virtual bool? Gender { get; private set; }

		/// <summary>
		/// Helper function that determines whether the input values would make a female. 
		/// </summary>
		/// isMale/isFemale/isGenderless
		private bool? gender //(float genderRate = this._base.MaleRatio)
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
					if (_base.GenderEnum == GenderRatio.AlwaysFemale && n > 0f && n < 12.5f) return false;
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

		/// <summary>
		/// Sets this Pokémon's gender to a particular gender (if possible).
		/// </summary>
		/// <param name="value"></param>
		public void setGender(bool? value)
		{
			//dexdata = pbOpenDexData;
			//pbDexDataOffset(dexdata, @species, 18);
			//genderbyte = dexdata.fgetb;
			//dexdata.close();
			//if (genderbyte != 255 && genderbyte != 0 && genderbyte != 254)
			//{
			//	@genderflag = value;
			//}
			Gender = value;
		}

		public void makeMale() { setGender(true); } //0
		public void makeFemale() { setGender(false); } //1
		#endregion

		#region Ability
		/// <summary>
		/// Returns the index of this Pokémon's ability.
		/// </summary>
		public int abilityIndex
		{
			get
			{
				//int abil = Ability != null ? @abilityflag : (@personalID & 1);
				//return abil;
				if (getAbilityList()[0] == Ability) return 0;
				else if (getAbilityList()[1] == Ability) return 1;
				else if (getAbilityList()[2] == Ability) return 2;
				else return 0;
			}
		}

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
		public Abilities Ability { get; set; }//{ get { return abilityFlag; } set { abilityFlag = value; } }

		/// <summary>
		/// Returns whether this Pokemon has a partiular ability
		/// </summary>
		/// <param name="ability"></param>
		/// <returns></returns>
		public bool hasAbility(Abilities ability = Abilities.NONE)
		{
			if (ability == Abilities.NONE) 
				//return (int)_base.Ability[0] > 0 || (int)_base.Ability[1] > 0;// || (int)_base.Ability[2] > 0;
				return (int)Ability > 0;
			else
			{
				//return _base.Ability[0] == ability || _base.Ability[1] == ability;// || Abilities[2] == ability;
				return Ability == ability;
			}
			//return false;
		}

		/// <summary>
		/// Sets this Pokémon's ability to a particular ability (if possible).
		/// </summary>
		/// <param name="value">Assigns ability based on the index in ability list</param>
		public void setAbility(int value)
		{
			//Ability = value;
			if (value >= 0  && value <= 2) Ability = _base.Ability[value];
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
			return this._base.Ability;
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

		/// <summary>
		/// Returns whether this Pokemon has a particular nature
		/// </summary>
		/// <param name="nature"></param>
		/// <returns></returns>
		public void setNature(Natures nature = 0) //None
		{
			//if ((int)nature < 1) return;
			this.Nature = nature;
		}
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
				if (shinyFlag.HasValue) return shinyFlag.Value;
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
				//int d = (Game.GameData.Player.Trainer.TrainerID ^ Game.GameData.Player.Trainer.SecretID) ^ (PersonalId / 65536) ^ (PersonalId % 65536);
				//If Pokemons are caught already `OT` -> the math should be set, else generate new values from current player
				int d = ((!OT.Equals((object)null) ? OT.publicID() : Game.GameData.Trainer.publicID())
					^ (!OT.Equals((object)null) ? OT.secretID() : Game.GameData.Trainer.secretID()))
					//^ ((Game.GameData.Player.Bag.GetItemAmount(Items.SHINY_CHARM) > 0 ? /*PersonalId*/Core.SHINYPOKEMONCHANCE * 3 : /*PersonalId*/Core.SHINYPOKEMONCHANCE) / 65536)
					^ ((Game.GameData.Trainer.party[0].Item == Items.SHINY_CHARM ? Core.SHINYPOKEMONCHANCE * 3 : Core.SHINYPOKEMONCHANCE) / 65536)
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

		/// <summary>
		/// Randomize Personal Id of pokemon, which will affect shiny flag
		/// </summary>
		public void shuffleShiny()
		{
			PersonalId = Core.Rand.Next(65536) | (Core.Rand.Next(65536) << 16);
		}

		/// <summary>
		/// Makes this Pokemon shiny.
		/// </summary>
		public void makeShiny()
		{
			shinyFlag = true;
		}

		/// <summary>
		/// Makes this Pokemon not shiny.
		/// </summary>
		public void makeNotShiny()
		{
			shinyFlag = false;
		}
		#endregion

		#region Pokerus
		/// <summary>
		/// Pokerus strain and infection time
		/// </summary
		/// <remarks>
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
		public int numMoves {  get { return countMoves(); } }
		/// <summary>
		/// Returns the number of moves known by the Pokémon.
		/// </summary>
		public int countMoves()
		{
			int ret = 0;
			for (int i = 0; i < 4; i++)
			{//foreach(var move in this.moves){ 
				if (this.moves[i].id != Moves.NONE) ret += 1;//move.id
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
				if (this.moves[i].id == move) return true;
			}
			return false;
		}

		public bool knowsMove(Moves move) { return this.hasMove(move); }

		/// <summary>
		/// Returns the list of moves this Pokémon can learn by training method.
		/// </summary>
		/// ToDo: Get Move list for pokemon based on their form?
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
		/// Idea: Higher the pokemon's level, the greater the chances of generating a full moveset (4 moves)
		public void GenerateMoveset(int? level = null, bool egg = false)
		{
			if (level.HasValue && level.Value < 0)
				return;
			ClearFirstMoves();
			resetMoves();
			int numMove = Core.Rand.Next(4) + 1; //number of moves pokemon will have, between 0 and 3
			List<Moves> movelist = new List<Moves>();
			if (isEgg || egg || (Game.GameData as Game).Features.CatchPokemonsWithEggMoves)
				movelist.AddRange(Game.PokemonMovesData[pokemons].Egg);
			int?[] rejected = new int?[movelist.Count]; //default null, to exclude `0`
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
				case null: //case -1:
					movelist.AddRange(Game.PokemonMovesData[pokemons].LevelUp.Where(x => x.Value <= this.Level).Select(x => x.Key));
					rejected = new int?[movelist.Count];
					for (int n = 0; n < movelist.Count; n++)
					{
						bool err = false;
						if (this.countMoves() < numMove)
						{
							//For a truly random approach, instead of just adding moves in the order they're listed
							int x;
							do { 
								x = Core.Rand.Next(movelist.Count);
							} while (rejected.Contains(x));
							rejected[n] = x;
							if (Core.Rand.Next(2) == 1) //0 false, 1 true
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
							int x;
							do { 
								x = Core.Rand.Next(movelist.Count);
							} while (rejected.Contains(x));
							rejected[n] = x;
							if (Core.Rand.Next(2) == 1) //0 false, 1 true
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
			pbRecordFirstMoves();
		}

		/// <summary>
		/// Sets this Pokémon's movelist to the default movelist it originally had.
		/// </summary>
		public void resetMoves()
		{
			//for (int i = 0; i < _base.MoveSet.Length; i++){//foreach(var i in _base.MoveSet)
			//    if (_base.MoveSet[i].Level <= this.Level)
			//    {
			//        movelist.Add(_base.MoveSet[i].id);
			//    }
			//}
			for (int i = 0; i < 4; i++)
			{
				this.moves[i] = new Move((i >= firstMoves.Count) ? 0 : firstMoves[i]);
			}
		}

		/// <summary>
		/// Teaches new move to pokemon. Will fail if pokemon is unable to learn.
		/// </summary>
		/// <param name="move"></param>
		/// <param name="silently">Forces move to be learned by pokemon by overriding fourth regardless of player's choice</param>
		/// <returns></returns>
		//ToDo: Change void to string, return errors as in-game prompts; 
		//remove `out bool success` or replace with `out string error`
		public void LearnMove(Moves move, out bool success, bool silently = false)
		{
			success = false;
			if ((int)move <= 0) return;
			if (!getMoveList().Contains(move))
			{
				GameDebug.Log("Move is not compatible");
				return;
			}
			//for (int i = 0; i < 4; i++) {
			//    if (moves[i].id == move) { //Switch ordering of moves?
			//        int j = i + 1;
			//        while (j < 4) {
			//            if (moves[j].id == 0) break;
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
				GameDebug.Log("Already knows move...");
				return;
			}
			for (int i = 0; i < 4; i++)
			{
				if (moves[i].id == Moves.NONE)
				{
					moves[i] = new Move(move);
					success = true;
					return;
				}
			}
			if (!silently)
			{
				GameDebug.LogWarning("Cannot learn move, pokmeon moveset is full");
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
		/// Silently learns the given move. Will erase the first known move if it has to.
		/// </summary>
		/// <param name="move"></param>
		/// <returns></returns>
		public void pbLearnMove(Moves move)
		{
			//if (move is String || move is Symbol)
			//{
			//	move = getID(PBMoves, move);
			//}
			if (move <= 0) return;
			for (int i = 0; i < 4; i++)
			{
				if (moves[i].id == move)
				{
					int j = i + 1; while (j < 4) { 
						if (@moves[j].id == 0) break;
						IMove tmp = @moves[j];
						@moves[j] = @moves[j - 1];
						@moves[j - 1] = tmp;
						j += 1;
					}
					return;
				}
			}
			for (int i = 0; i< 4; i++) {
				if (@moves[i].id==0) {
					@moves[i]=new PokemonUnity.Attack.Move(move);
					return;
				}
			}
			@moves[0] = @moves[1];
			@moves[1] = @moves[2];
			@moves[2] = @moves[3];
			@moves[3] = new PokemonUnity.Attack.Move(move);
		}

		/// <summary>
		/// Gets a random move of the Pokémon by one that it learns on a given level.
		/// </summary>
		/// <param name="level">The level the Pokémon learns the desired move on.</param>
		public Moves GetMove(int level, bool egg = false)
		{
			bool moveLearned = false;
			List<Moves> movelist = new List<Moves>();
			if (isEgg || egg) movelist.AddRange(Game.PokemonMovesData[pokemons].Egg);
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
						//j += this.countMoves() < numMove ? 0 : 1;
						//LearnMove((Moves)movelist[x], out moveLearned);
						return (Moves)movelist[x];
					}
				}
				else
					break;
			}
			return Moves.NONE;
		}

		/// <summary>
		/// Deletes the given move from the Pokémon.
		/// </summary>
		/// <param name=""></param>
		/// <returns></returns>
		public void DeleteMove(Moves move)
		{
			if (move <= 0) return;
			IList<IMove> newmoves = new List<IMove>();
			for (int i = 0; i < 4; i++)
			{
				if (moves[i].id != move) newmoves.Add(moves[i]);
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
			IList<IMove> newmoves = new List<IMove>();

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
		public void pbRecordFirstMoves()
		{
			for (int i = 0; i < 4; i++)
			{
				if (moves[i].id > 0) AddFirstMove(moves[i].id);
			}
		}

		protected void AddFirstMove(Moves move)
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

		public bool isCompatibleWithMove(Moves move)
		{
			//return pbSpeciesCompatible(this.Species, move);
			return getMoveList().Contains(move);
		}
		#endregion

		#region Contest attributes, ribbons
		//ToDo: This is actually a hashset, and not list, because only store one of each
		private List<Ribbons> ribbons { get; set; }
		/// <summary>
		/// Each region/ribbon sprite should have it's own Ribbon.EnumId
		/// </summary>
		/// <example>Pokemon acquired beauty ribbon in region 1 AND 2?</example>
		/// Make each ribbon into sets, where next number up is upgrade.
		/// Does it make a difference if pokemon won contest in different regions? 
		/// No -- ribbons are named after region; Do not need to record region data
		/// ToDo: Dictionary(Ribbon,struct[DateTime/Bool])
		public Ribbons[] Ribbons { get { return this.ribbons.ToArray(); } }
		/// <summary>
		/// Contest stats; Max value is 255
		/// </summary>
		public byte[] Contest { get; private set; }
		public int Cool		{ get { return Contest[(int)Contests.Cool]; } }
		public int Beauty	{ get { return Contest[(int)Contests.Beauty]; } }
		public int Cute		{ get { return Contest[(int)Contests.Cute]; } }
		public int Smart	{ get { return Contest[(int)Contests.Smart]; } }
		public int Tough	{ get { return Contest[(int)Contests.Tough]; } }
		public int Sheen	{ get { return Contest[(int)Contests.Sheen]; } }
		/// <summary>
		/// Returns the number of ribbons this Pokemon has.
		/// </summary>
		public int ribbonCount 
		{ 
			get
			{
				if (@ribbons == null) @ribbons = new List<Ribbons>();
				return @ribbons.Count;
			} 
		}
		/// <summary>
		/// Returns whether this Pokémon has the specified ribbon.
		/// </summary>
		/// <param name="ribbon"></param>
		/// <returns></returns>
		public bool hasRibbon(Ribbons ribbon)
		{
			if (@ribbons == null) @ribbons = new List<Ribbons>();
			if (ribbon == PokemonUnity.Ribbons.NONE) return false;
			//if (Ribbons.Count == 0) return false;
			return Ribbons.Contains(ribbon);
		}
		/// <summary>
		/// Gives this Pokémon the specified ribbon.
		/// </summary>
		/// <param name="ribbon"></param>
		public void giveRibbon(Ribbons ribbon)
		{
			if (@ribbons == null) @ribbons = new List<Ribbons>();
			if (ribbon == PokemonUnity.Ribbons.NONE) return;
			if (!Ribbons.Contains(ribbon)) this.ribbons.Add(ribbon);
		}
		/// <summary>
		/// Replaces one ribbon with the next one along, if possible.
		/// </summary>
		/// <param name="arg"></param>
		/// ToDo: Redo code below to something more hardcoded
		public Ribbons upgradeRibbon(params Ribbons[] arg)
		{
			if (@ribbons == null) @ribbons = new List<Ribbons>();
			for (int i = 0; i < arg.Length - 1; i++)
			{
				for (int j = 0; j < @ribbons.Count; j++)
				{
					Ribbons thisribbon = arg[i];
					if (@ribbons[j] == thisribbon)
					{
						Ribbons nextribbon = arg[i + 1];
						@ribbons[j] = nextribbon;
						return nextribbon;
					}
				}
			}
			if (!hasRibbon(arg[arg.Length - 1]))
			{
				Ribbons firstribbon = arg[0];
				giveRibbon(firstribbon);
				return firstribbon;
			}
			return 0;
		}
		/// <summary>
		/// Removes the specified ribbon from this Pokémon.
		/// </summary>
		/// <param name="ribbon"></param>
		public void takeRibbon(Ribbons ribbon)
		{
			if (@ribbons == null) return;
			if (ribbons.Count == 0) return;
			if (ribbon <= 0) return;
			for (int i = 0; i < ribbons.Count; i++)
			{
			    if (Ribbons[i] == ribbon)
			    {
			        //ribbons[i] = Ribbon.NONE;
			        ribbons.RemoveAt(i);
			        break;
			    }
			}
			//ribbons.Remove(Ribbon.NONE); 
			//ribbons.RemoveAll(r => r == Ribbon.NONE);
			//ribbons.RemoveAll(r => r == ribbon);
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
		/// <summary>
		/// Returns whether this Pokémon has a hold item.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool hasItem(Items value= 0)
		{
			if (value == 0)
			{
				return this.Item > 0;
			}
			else
			{
				return this.Item == value;
			}
			return false;
		}
		/// <summary>
		/// Sets this Pokémon's item. Accepts symbols.
		/// </summary>
		/// <param name="value"></param>
		public void setItem(Items value)
		{
			this.Item = value;
		}
		public Items SwapItem(Items item)
		{
			Items old = Item;
			Item = item;
			return old;
		}
		/// <summary>
		/// Returns the items this species can be found holding in the wild.
		/// </summary>
		/// <returns></returns>
		/// Not sure how i feel about this one... might consider removing
		public Items[] wildHoldItems()
		{
			if (!Game.PokemonItemsData.ContainsKey(pokemons)) return new Items[0];
			//_base.HeldItem
			PokemonWildItems[] dexdata = Game.PokemonItemsData[pokemons];
			//pbDexDataOffset(dexdata, @species, 48);
			Items itemcommon	= Items.NONE; //dexdata.fgetw;
			Items itemuncommon	= Items.NONE; //dexdata.fgetw;
			Items itemrare		= Items.NONE; //dexdata.fgetw;
			//dexdata.close();
			if (itemcommon == null) itemcommon = 0;
			if (itemuncommon == null) itemuncommon = 0;
			if (itemrare == null) itemrare = 0;
			return new Items[] { itemcommon, itemuncommon, itemrare };
		}

		#region Mail
		/// <summary>
		/// Mail?...
		/// </summary>
		private PokemonUnity.Inventory.Mail mail { get; set; }
		/// <summary>
		/// Perform a null check; if anything other than null, there is a message
		/// </summary>
		public string Mail
		{
			get
			{
				if (this.mail == null || !Game.ItemData[this.Item].IsLetter) return null; //If empty return null
				//if (mail.Message.Length == 0 || this.Inventory == 0)//|| this.item.Category != Items.Category.Mail )
				//{
				//    //mail = null;
				//	return null;
				//}
				return mail.Message;
			}
			//set { mail = value; }
		}
		/*public bool pbMoveToMailbox()
		{
			//if (PC.MailBox == null) PC.MailBox = [];
			if (PC.MailBox.Length>=10) return false;
			if (mail == null) return false;
				PC.MailBox.Add(Item);
			mail = null;
			Item = Items.NONE;
			return true;
		}

		public void pbStoreMail(string message, TrainerId sender)//pkmn, item, message, poke1= nil, poke2= nil, poke3= nil)
		{
			//raise Game._INTL("Pokémon already has mail") if pkmn.mail
			if (Mail == null) GameDebug.LogError("Pokémon already has mail");
			//Mail = new PokemonMail(item, message, Trainer.name, poke1, poke2, poke3)
			mail = new Mail(Item, message, sender);
		}

		public void pbTakeMail()
		{
			if (Item == Items.NONE)
			{
				pbDisplay(Game._INTL("{1} isn't holding anything.", Name));
			}
			else if (!Game.GameData.Player.Bag.pbCanStore(Item))
			{
				pbDisplay(Game._INTL("The Bag is full.  The Pokémon's item could not be removed."));
			}
			else if (mail != null)
			{
			}

			if (pbConfirm(Game._INTL("Send the removed mail to your PC?")))
			{
				if (!pbMoveToMailbox())
					pbDisplay(Game._INTL("Your PC's Mailbox is full."));

				else
					pbDisplay(Game._INTL("The mail was sent to your PC."));

				//setItem(0);
				Item = Items.NONE;
			}
			else if (pbConfirm(Game._INTL("If the mail is removed, the message will be lost.  OK?")))
			{
				pbDisplay(Game._INTL("Mail was taken from the Pokémon."));
				Game.GameData.Player.Bag.pbStoreItem(Item);
				//setItem(0);
				Item = Items.NONE;
				mail = null;
			}
			else
			{
				Game.GameData.Player.Bag.pbStoreItem(Item);
				string itemname = Item.ToString(TextScripts.Name);

				pbDisplay(Game._INTL("Received the {1} from {2}.", itemname, Name));
				//setItem(0);
				Item = Items.NONE;
			}
		}

		public bool pbGiveMail(Items item)//, pkmn, pkmnid= 0)
		{
			string thisitemname = item.ToString(TextScripts.Name);
			if (isEgg)
			{
				pbDisplay(Game._INTL("Eggs can't hold items."));
				return false;
			}
			if (mail != null)
			{
				pbDisplay(Game._INTL("Mail must be removed before holding an item."));
				return false;
			}
			if (item!=Items.NONE)
			{
				string itemname = Item.ToString(TextScripts.Name);

				pbDisplay(Game._INTL("{1} is already holding one {2}.\1", name, itemname));
				if (pbConfirm(Game._INTL("Would you like to switch the two items?")))
				{
					Game.GameData.Player.Bag.pbDeleteItem(item);
					if (!Game.GameData.Player.Bag.pbStoreItem(item))
					{
						if (!Game.GameData.Player.Bag.pbStoreItem(item)) // Compensate
						{
							//raise Game._INTL("Can't re-store deleted item in bag");
							GameDebug.LogError(Game._INTL("Can't re-store deleted item in bag"));
						}

						pbDisplay(Game._INTL("The Bag is full.  The Pokémon's item could not be removed."));
					}
					else
					{
						if (Game.ItemData[Item].IsLetter)//(pbIsMail(item))
						{
							//if (pbMailScreen(item, pkmn, pkmnid))
							if (pbMailScreen(item, this))
							{
								//setItem(item);
								Item = item;

								pbDisplay(Game._INTL("The {1} was taken and replaced with the {2}.", itemname, thisitemname));
								return true;
							}
							else
							{
								if (!Game.GameData.Player.Bag.pbStoreItem(item)) { // Compensate
									//raise Game._INTL("Can't re-store deleted item in bag");
									GameDebug.LogError(Game._INTL("Can't re-store deleted item in bag"));
								}
							}
						}
						else
						{
							//setItem(item);
							Item=item;

							pbDisplay(Game._INTL("The {1} was taken and replaced with the {2}.", itemname, thisitemname));
							return true;
						}
					}
				}
			}
			else
			{
				if (!pbIsMail(item) || pbMailScreen(item, pkmn, pkmnid)) // Open the mail screen if necessary
				if (!Game.ItemData[Item].IsLetter || pbMailScreen(item, this)) // Open the mail screen if necessary
				{
					Game.GameData.Player.Bag.pbDeleteItem(item);
					//setItem(item);
					Item=item;

					pbDisplay(Game._INTL("{1} was given the {2} to hold.", name, thisitemname));
					return true;
				}
			}
			return false;
		}*/
		#endregion Mail
		#endregion

		#region Other
		/// <summary>
		/// The pokemons fused into this one.
		/// </summary>
		/// was int before
		/// Store both pokemons separately
		/// Combine the stats of both to create a new hybrid
		/// When reverting back, return to original data, and split exp gained between fusion
		/// ExpGained = (FusedExp - MiddleExpOfPokemons) / #ofPokemons
		/// should remain null until needed
		public IPokemon[] fused { get; set; }
		/// <summary>
		/// Nickname
		/// </summary>
		private string name { get; set; }
		/// <summary>
		/// Nickname
		/// </summary>
		public bool IsNicknamed { get { return !string.IsNullOrEmpty(name); } }
		/// <summary>
		/// Nickname; 
		/// Returns Pokemon species name if not nicknamed.
		/// </summary>
		public virtual string Name { get { if (Species == Pokemons.NONE) return string.Empty; if (isEgg) return "Egg"; return IsNicknamed ? name : Species.ToString(TextScripts.Name); } set { name = value; } }
		public void SetNickname(string nick) { name = nick; }

		/// <summary>
		/// Returns the markings this Pokemon has checked
		/// </summary>
		/// <returns>●, ▲, ■, ♥︎, ★, and ♦︎.</returns>
		public bool[] Markings { get; set; }

		/// <summary>
		/// Returns a string stating the Unown form of this Pokemon
		/// </summary>
		/// <returns></returns>
		public char UnknownShape()
		{
			if (this.pokemons == Pokemons.UNOWN)
				return "ABCDEFGHIJKLMNOPQRSTUVWXYZ?!".ToCharArray()[FormId];
			else return '*';
		}

		/// <summary>
		/// Sets this Pokemon's HP;
		/// Changes status on fainted
		/// </summary>
		//public int HP
		//{
		//	get { if (hp > TotalHP) hp = TotalHP;  return this.hp; }
		//	set
		//	{
		//		this.hp = value < 0 ? 0 : (value > this.TotalHP ? TotalHP : value);
		//		//this.hp = (this.HP + value).Clamp(0, this.TotalHP);
		//		if (isFainted()) { this.Status = Status.FAINT; StatusCount = 0; ChangeHappiness(HappinessMethods.FAINT); }
		//	}
		//}

		///<summary>
		///</summary>
		///ToDo: Change from Method to Get-Only Property
		public bool isFainted()
		{
			return !this.isEgg //eggSteps == 0 //not an egg
				&& this.HP <= 0;//hp <= 0;
		}

		/// <summary>
		/// Heals all HP of this Pokemon
		/// </summary>
		public bool HealHP()
		{
			if (this.isEgg)
				return false;
			this.HP = TotalHP;
			return true;
		}

		/// <summary>
		/// Heals the status problem of this Pokemon
		/// </summary>
		public bool HealStatus()
		{
			if (this.isEgg)
				return false;
			this.Status = 0; StatusCount = 0; //remove status ailments
			return true;
		}

		/// <summary>
		/// Heals all PP of this Pokemon
		/// </summary>
		/// <param name="index"></param>
		public bool HealPP(int index = -1)
		{
			if (this.isEgg)
				return false;
			//if (index >= 0) moves[index] = new Move(moves[index].id, moves[index].PPups, moves[index].TotalPP);
			if (index >= 0) moves[index].PP = moves[index].TotalPP;
			else
			{
				for (int i = 0; i < 4; i++)
				{
					//moves[i] = new Move(moves[i].id, moves[i].PPups, moves[i].TotalPP);
					moves[i].PP = moves[i].TotalPP;
				}
			}
			return true;
		}

		/// <summary>
		/// Heals all HP, PP, and status problems of this Pokemon
		/// </summary>
		public bool Heal()
		{
			if (this.isEgg)
				return false;
			return
				HealHP() &&
				HealStatus() &&
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
					//ToDo: if trainer is on map pkmn was captured on, add more happiness on walk
					//gain += this.metMap.Id == currentMap.Id ? 1 : 0; //change to "obtainMap"?
					//if ((int)this.ObtainMap == Game.GameData.GameMap.map_id) gain += 1;
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
					if (Game.GameData is IGameMessage m) m.pbMessage(Game._INTL("Unknown happiness-changing method."));
					//break;
					//If not listed above, then stop
					//Otherwise rest of code will add points
					return;
			}
			gain += luxury && this.ballUsed == Items.LUXURY_BALL ? 1 : 0;
			if (this.Item == Items.SOOTHE_BELL && gain > 0)
				gain = (int)Math.Floor(gain * 1.5f);
			Happiness += gain;
			//Happiness = Math.Max(Math.Min(255, Happiness), 0);
			Happiness = //Max 255, Min 0
				Happiness > 255 ?   //if 
					255
				: Happiness < 0 ?   //if else
					0
				: Happiness;		//else
		}
		#endregion

		#region Stat calculations, Pokemon creation
		/// <summary>
		/// Returns this Pokémon's base stats.  
		/// </summary>
		/// <returns>An array of six values.</returns>
		public int[] baseStats
		{
			get
			{
				//dexdata = pbOpenDexData;
				//pbDexDataOffset(dexdata, @species, 10);
				int[] ret = new int[] {
				   _base.BaseStatsHP,	//dexdata.fgetb, // HP
				   _base.BaseStatsATK,	//dexdata.fgetb, // Attack
				   _base.BaseStatsDEF,	//dexdata.fgetb, // Defense
				   _base.BaseStatsSPE,	//dexdata.fgetb, // Speed
				   _base.BaseStatsSPA,	//dexdata.fgetb, // Special Attack
				   _base.BaseStatsSPD	//dexdata.fgetb  // Special Defense
				};
				//dexdata.close();
				return ret;
			}
		}

		/// <summary>
		/// Returns the maximum HP of this Pokémon.
		/// </summary>
		/// <param name="orig">Base HP</param>
		/// <param name="level"></param>
		/// <param name="iv"></param>
		/// <param name="ev"></param>
		/// <returns></returns>
		public int calcHP(int orig,int level,int iv,int ev)
		{
			if (orig == 1) return 1;
			return (int)Math.Floor((orig * 2 + iv + (ev >> 2)) * level / 100f) + level + 10;
		}
		/// <summary>
		/// Returns the specified stat of this Pokémon (not used for total HP).
		/// </summary>
		/// <param name="orig"></param>
		/// <param name="level"></param>
		/// <param name="iv"></param>
		/// <param name="ev"></param>
		/// <param name="pv"></param>
		/// <returns></returns>
		public int calcStat(int orig,int level,int iv,int ev,int pv)
		{
			return (int)Math.Floor((Math.Floor((orig * 2 + iv + (ev >> 2)) * level / 100f) + 5) * pv / 100);
		}
		/// <summary>
		/// Recalculates this Pokémon's stats.
		/// </summary>
		/// <remarks>
		/// Stats are already hardcoded, 
		/// will use this to adjust hp values instead
		/// </remarks>
		public void calcStats()
		{
			int nature = (int)this.Nature;
			int[] stats =new int[6];
			int[] pvalues = new int[] { 100, 100, 100, 100, 100 };
			int nd5 = (int)Math.Floor(nature / 5f);
			int nm5 = (int)Math.Floor(nature % 5f);
			if (nd5 != nm5)
			{
				pvalues[nd5] = 110;
				pvalues[nm5] = 90;
			}
			int level = this.Level;
			int[] bs = this.baseStats;
			for (int i = 0; i < 5; i++)
			{
				int orig = bs[i];
				if (i == (int)Stats.HP)
				{
					stats[i] = calcHP(orig, level, @IV[i], @EV[i]);
				}
				else
				{
					stats[i] = calcStat(orig, level, @IV[i], @EV[i], pvalues[i - 1]);
				}
			}
			int diff = @TotalHP - @HP;
			//@TotalHP = stats[0];
			@HP = @TotalHP - diff;
			if (@HP <= 0) @HP = 0;
			if (@HP > @TotalHP) @HP = @TotalHP;
			//@attack = stats[1];
			//@defense = stats[2];
			//@speed = stats[3];
			//@spatk = stats[4];
			//@spdef = stats[5];
		}
		/// <summary>
		/// Adds Effort values (EV) to this Pokémon after defeated another Pokémon, if possible.
		/// </summary>
		/// <param name="DefeatedPokemon">The defeated Pokémon.</param>
		/// <param name="SoS">Gen7 SoS Battle, where wild pokemon calls for reinforcements. Only `true` for the additional pokemon.</param>
		public void GainEffort(Pokemons DefeatedPokemon, bool SoS = false)
		{
			int allEV = EV[(int)Stats.HP] + EV[(int)Stats.ATTACK] + EV[(int)Stats.DEFENSE] + EV[(int)Stats.SPATK] + EV[(int)Stats.SPDEF] + EV[(int)Stats.SPEED];
			if (allEV >= EVLIMIT || isShadow) //Shadow Pkmns dont earn EV from battles?...
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

			int EVfactor = PokerusStage.HasValue && PokerusStage.Value ? 2 : 1; //if pokerus, ev values are doubled
			if (SoS) //If the SoS-Ally Pokemon is defeated, double EV gain
				EVfactor *= 2;

			switch (Item)
			{
				case Items.MACHO_BRACE:
				{
					EVfactor *= 2; //with pokerus, should be up to x4
					break;
				}
				case Items.POWER_WEIGHT:
				{
					gainEVHP += 8;
					break;
				}
				case Items.POWER_BRACER:
				{
					gainEVAttack += 8;
					break;
				}
				case Items.POWER_BELT:
				{
					gainEVDefense += 8;
					break;
				}
				case Items.POWER_LENS:
				{
					gainEVSpAttack += 8;
					break;
				}
				case Items.POWER_BAND:
				{
					gainEVSpDefense += 8;
					break;
				}
				case Items.POWER_ANKLET:
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
			else GameDebug.LogWarning($"Single-stat EV limit #{EVSTATLIMIT} exceeded.\r\nStat: #{Stats.HP.ToString()}  EV gain: #{gainEVHP}  EVs: #{EV.ToString()}");

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
			else GameDebug.LogWarning($"Single-stat EV limit #{EVSTATLIMIT} exceeded.\r\nStat: #{Stats.ATTACK.ToString()}  EV gain: #{gainEVAttack}  EVs: #{EV.ToString()}");

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
			else GameDebug.LogWarning($"Single-stat EV limit #{EVSTATLIMIT} exceeded.\r\nStat: #{Stats.DEFENSE.ToString()}  EV gain: #{gainEVDefense}  EVs: #{EV.ToString()}");

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
			else GameDebug.LogWarning($"Single-stat EV limit #{EVSTATLIMIT} exceeded.\r\nStat: #{Stats.SPATK.ToString()}  EV gain: #{gainEVSpAttack}  EVs: #{EV.ToString()}");

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
			else GameDebug.LogWarning($"Single-stat EV limit #{EVSTATLIMIT} exceeded.\r\nStat: #{Stats.SPDEF.ToString()}  EV gain: #{gainEVSpDefense}  EVs: #{EV.ToString()}");

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
			else GameDebug.LogWarning($"Single-stat EV limit #{EVSTATLIMIT} exceeded.\r\nStat: #{Stats.SPEED.ToString()}  EV gain: #{gainEVSpeed}  EVs: #{EV.ToString()}");
		}

		/// <summary>
		/// Adds Effort values (EV) to this Pokémon after using an Item, if possible.
		/// </summary>
		/// <param name="UsedItem">The item used on Pokémon.</param>
		/// ToDo: Test Function... Add Method for other Item usage
		private void GainEffort(Items UsedItem)
		{
			int allEV = EV[(int)Stats.HP] + EV[(int)Stats.ATTACK] + EV[(int)Stats.DEFENSE] + EV[(int)Stats.SPATK] + EV[(int)Stats.SPDEF] + EV[(int)Stats.SPEED];
			if (allEV >= EVLIMIT)
			{
				GameDebug.LogWarning($"EV limit #{Monster.Pokemon.EVLIMIT} exceeded.\r\nTotal EVs: #{allEV} EV gain: 0  EVs: #{EV.ToString()}");
				return;
			}

			int maxEVgain = EVLIMIT - allEV;
			int totalEVgain = 0;

			// EV gains
			int gainEVHP = 0; //Game.PokemonData[UsedItem].evYieldHP;
			int gainEVAttack = 0; //Game.PokemonData[UsedItem].evYieldATK;
			int gainEVDefense = 0; //Game.PokemonData[UsedItem].evYieldDEF;
			int gainEVSpAttack = 0; //Game.PokemonData[UsedItem].evYieldSPA;
			int gainEVSpDefense = 0; //Game.PokemonData[UsedItem].evYieldSPD;
			int gainEVSpeed = 0; //Game.PokemonData[UsedItem].evYieldSPE;

			int EVfactor = 1;

			switch (UsedItem)
			{
				case Items.HP_UP:
					{
						if (EV[(int)Stats.HP] < 100) gainEVHP += 10;
						ChangeHappiness(HappinessMethods.VITAMIN);
						break;
					}
				case Items.PROTEIN:
					{
						if (EV[(int)Stats.ATTACK] < 100) gainEVAttack += 10;
						ChangeHappiness(HappinessMethods.VITAMIN);
						break;
					}
				case Items.IRON:
					{
						if (EV[(int)Stats.DEFENSE] < 100) gainEVDefense += 10;
						ChangeHappiness(HappinessMethods.VITAMIN);
						break;
					}
				case Items.CALCIUM:
					{
						if (EV[(int)Stats.SPATK] < 100) gainEVSpAttack += 10;
						ChangeHappiness(HappinessMethods.VITAMIN);
						break;
					}
				case Items.ZINC:
					{
						if (EV[(int)Stats.SPDEF] < 100) gainEVSpDefense += 10;
						ChangeHappiness(HappinessMethods.VITAMIN);
						break;
					}
				case Items.CARBOS:
					{
						if (EV[(int)Stats.SPEED] < 100) gainEVSpeed += 10;
						ChangeHappiness(HappinessMethods.VITAMIN);
						break;
					}
				case Items.POMEG_BERRY:
					{
						gainEVHP -= 10;
						ChangeHappiness(HappinessMethods.EVBERRY);
						break;
					}
				case Items.KELPSY_BERRY:
					{
						gainEVAttack -= 10;
						ChangeHappiness(HappinessMethods.EVBERRY);
						break;
					}
				case Items.QUALOT_BERRY:
					{
						gainEVDefense -= 10;
						ChangeHappiness(HappinessMethods.EVBERRY);
						break;
					}
				case Items.HONDEW_BERRY:
					{
						gainEVSpAttack -= 10;
						ChangeHappiness(HappinessMethods.EVBERRY);
						break;
					}
				case Items.GREPA_BERRY:
					{
						gainEVSpDefense -= 10;
						ChangeHappiness(HappinessMethods.EVBERRY);
						break;
					}
				case Items.TAMATO_BERRY:
					{
						gainEVSpeed -= 10;
						ChangeHappiness(HappinessMethods.EVBERRY);
						break;
					}
				case Items.HEALTH_WING:
					{
						gainEVHP += 1;
						break;
					}
				case Items.MUSCLE_WING:
					{
						gainEVAttack += 1;
						break;
					}
				case Items.RESIST_WING:
					{
						gainEVDefense += 1;
						break;
					}
				case Items.GENIUS_WING:
					{
						gainEVSpAttack += 1;
						break;
					}
				case Items.CLEVER_WING:
					{
						gainEVSpDefense += 1;
						break;
					}
				case Items.SWIFT_WING:
					{
						gainEVSpeed += 1;
						break;
					}
			}

			// HP gain
			if ((gainEVHP > 0 && EV[(int)Stats.HP] < EVSTATLIMIT && maxEVgain - totalEVgain > 0))
			{
				gainEVHP *= EVfactor;
				gainEVHP = gainEVHP < 0 ? (0 > gainEVHP + EV[(int)Stats.HP] ? 0 - EV[(int)Stats.HP] : gainEVHP)
					: (gainEVHP > EVSTATLIMIT - EV[(int)Stats.HP] ? EVSTATLIMIT - EV[(int)Stats.HP] : gainEVHP);
				gainEVHP = gainEVHP < 0 ? (0 > gainEVHP + totalEVgain ? 0 - totalEVgain : gainEVHP)
					: (gainEVHP > maxEVgain - totalEVgain ? maxEVgain - totalEVgain : gainEVHP);
				//gainEVHP = MathHelper.Clamp(gainEVHP, 0, EVSTATLIMIT - EV[(int)Stats.HP]);
				//gainEVHP = MathHelper.Clamp(gainEVHP, 0, maxEVgain - totalEVgain);
				EV[(int)Stats.HP] += (byte)gainEVHP;
				totalEVgain += gainEVHP;
			}
			else GameDebug.LogWarning($"Single-stat EV limit #{EVSTATLIMIT} exceeded.\r\nStat: #{Stats.HP.ToString()}  EV gain: #{gainEVHP}  EVs: #{EV.ToString()}");

			// Attack gain
			if ((gainEVAttack > 0 && EV[(int)Stats.ATTACK] < EVSTATLIMIT && maxEVgain - totalEVgain > 0))
			{
				gainEVAttack *= EVfactor;
				gainEVAttack = gainEVAttack < 0 ? (0 > gainEVAttack + EV[(int)Stats.ATTACK] ? 0 - EV[(int)Stats.ATTACK] : gainEVAttack)
					: (gainEVAttack > EVSTATLIMIT - EV[(int)Stats.ATTACK] ? EVSTATLIMIT - EV[(int)Stats.ATTACK] : gainEVAttack);
				gainEVAttack = gainEVAttack < 0 ? (0 > gainEVAttack + totalEVgain ? 0 - totalEVgain : gainEVAttack)
					: (gainEVAttack > maxEVgain - totalEVgain ? maxEVgain - totalEVgain : gainEVAttack);
				//gainEVAttack = MathHelper.Clamp(gainEVAttack, 0, EVSTATLIMIT - EV[(int)Stats.ATTACK]);
				//gainEVAttack = MathHelper.Clamp(gainEVAttack, 0, maxEVgain - totalEVgain);
				EV[(int)Stats.ATTACK] += (byte)gainEVAttack;
				totalEVgain += gainEVAttack;
			}
			else GameDebug.LogWarning($"Single-stat EV limit #{EVSTATLIMIT} exceeded.\r\nStat: #{Stats.ATTACK.ToString()}  EV gain: #{gainEVAttack}  EVs: #{EV.ToString()}");

			// Defense gain
			if ((gainEVDefense > 0 && EV[(int)Stats.DEFENSE] < EVSTATLIMIT && maxEVgain - totalEVgain > 0))
			{
				gainEVDefense *= EVfactor;
				gainEVDefense = gainEVDefense < 0 ? (0 > gainEVDefense + EV[(int)Stats.DEFENSE] ? 0 - EV[(int)Stats.DEFENSE] : gainEVDefense)
					: (gainEVDefense > EVSTATLIMIT - EV[(int)Stats.DEFENSE] ? EVSTATLIMIT - EV[(int)Stats.DEFENSE] : gainEVDefense);
				gainEVDefense = gainEVDefense < 0 ? (0 > gainEVDefense + totalEVgain ? 0 - totalEVgain : gainEVDefense)
					: (gainEVDefense > maxEVgain - totalEVgain ? maxEVgain - totalEVgain : gainEVDefense);
				//gainEVDefense = MathHelper.Clamp(gainEVDefense, 0, EVSTATLIMIT - EV[(int)Stats.DEFENSE]);
				//gainEVDefense = MathHelper.Clamp(gainEVDefense, 0, maxEVgain - totalEVgain);
				EV[(int)Stats.DEFENSE] += (byte)gainEVDefense;
				totalEVgain += gainEVDefense;
			}
			else GameDebug.LogWarning($"Single-stat EV limit #{EVSTATLIMIT} exceeded.\r\nStat: #{Stats.DEFENSE.ToString()}  EV gain: #{gainEVDefense}  EVs: #{EV.ToString()}");

			// SpAttack gain
			if ((gainEVSpAttack > 0 && EV[(int)Stats.SPATK] < EVSTATLIMIT && maxEVgain - totalEVgain > 0))
			{
				gainEVSpAttack *= EVfactor;
				gainEVSpAttack = gainEVSpAttack < 0 ? (0 > gainEVSpAttack + EV[(int)Stats.SPATK] ? 0 - EV[(int)Stats.SPATK] : gainEVSpAttack)
					: (gainEVSpAttack > EVSTATLIMIT - EV[(int)Stats.SPATK] ? EVSTATLIMIT - EV[(int)Stats.SPATK] : gainEVSpAttack);
				gainEVSpAttack = gainEVSpAttack < 0 ? (0 > gainEVSpAttack + totalEVgain ? 0 - totalEVgain : gainEVSpAttack)
					: (gainEVSpAttack > maxEVgain - totalEVgain ? maxEVgain - totalEVgain : gainEVSpAttack);
				//gainEVSpAttack = MathHelper.Clamp(gainEVSpAttack, 0, EVSTATLIMIT - EV[(int)Stats.SPATK]);
				//gainEVSpAttack = MathHelper.Clamp(gainEVSpAttack, 0, maxEVgain - totalEVgain);
				EV[(int)Stats.SPATK] += (byte)gainEVSpAttack;
				totalEVgain += gainEVSpAttack;
			}
			else GameDebug.LogWarning($"Single-stat EV limit #{EVSTATLIMIT} exceeded.\r\nStat: #{Stats.SPATK.ToString()}  EV gain: #{gainEVSpAttack}  EVs: #{EV.ToString()}");

			// SpDefense gain
			if ((gainEVSpDefense > 0 && EV[(int)Stats.SPDEF] < EVSTATLIMIT && maxEVgain - totalEVgain > 0))
			{
				gainEVSpDefense *= EVfactor;
				gainEVSpDefense = gainEVSpDefense < 0 ? (0 > gainEVSpDefense + EV[(int)Stats.SPDEF] ? 0 - EV[(int)Stats.SPDEF] : gainEVSpDefense)
					: (gainEVSpDefense > EVSTATLIMIT - EV[(int)Stats.SPDEF] ? EVSTATLIMIT - EV[(int)Stats.SPDEF] : gainEVSpDefense);
				gainEVSpDefense = gainEVSpDefense < 0 ? (0 > gainEVSpDefense + totalEVgain ? 0 - totalEVgain : gainEVSpDefense)
					: (gainEVSpDefense > maxEVgain - totalEVgain ? maxEVgain - totalEVgain : gainEVSpDefense);
				//gainEVSpDefense = MathHelper.Clamp(gainEVSpDefense, 0, EVSTATLIMIT - EV[(int)Stats.SPDEF]);
				//gainEVSpDefense = MathHelper.Clamp(gainEVSpDefense, 0, maxEVgain - totalEVgain);
				EV[(int)Stats.SPDEF] += (byte)gainEVSpDefense;
				totalEVgain += gainEVSpDefense;
			}
			else GameDebug.LogWarning($"Single-stat EV limit #{EVSTATLIMIT} exceeded.\r\nStat: #{Stats.SPDEF.ToString()}  EV gain: #{gainEVSpDefense}  EVs: #{EV.ToString()}");

			// Speed gain
			if ((gainEVSpeed > 0 && EV[(int)Stats.SPEED] < EVSTATLIMIT && maxEVgain - totalEVgain > 0))
			{
				gainEVSpeed *= EVfactor;
				gainEVSpeed = gainEVSpeed < 0 ? (0 > gainEVSpeed + EV[(int)Stats.SPEED] ? 0 - EV[(int)Stats.SPEED] : gainEVSpeed)
					: (gainEVSpeed > EVSTATLIMIT - EV[(int)Stats.SPEED] ? EVSTATLIMIT - EV[(int)Stats.SPEED] : gainEVSpeed);
				gainEVSpeed = gainEVSpeed < 0 ? (0 > gainEVSpeed + totalEVgain ? 0 - totalEVgain : gainEVSpeed)
					: (gainEVSpeed > maxEVgain - totalEVgain ? maxEVgain - totalEVgain : gainEVSpeed);
				//gainEVSpeed = MathHelper.Clamp(gainEVSpeed, 0, EVSTATLIMIT - EV[(int)Stats.SPEED]);
				//gainEVSpeed = MathHelper.Clamp(gainEVSpeed, 0, maxEVgain - totalEVgain);
				EV[(int)Stats.SPEED] += (byte)gainEVSpeed;
				totalEVgain += gainEVSpeed;
			}
			else GameDebug.LogWarning($"Single-stat EV limit #{EVSTATLIMIT} exceeded.\r\nStat: #{Stats.SPEED.ToString()}  EV gain: #{gainEVSpeed}  EVs: #{EV.ToString()}");
		}
		#endregion
#pragma warning restore 0162 //Warning CS0162  Unreachable code detected 

		//ToDo: Finish migrating interface implimentation
		#region Explicit Interface Implemenation
		Pokemons IPokemon.Species { get; set; }
		int IPokemon.trainerID { get; set; }
		IMail IPokemon.mail { get; set; }
		IPokemon[] IPokemon.fused { get; set; }
		string IPokemon.Name { get; set; }
		int IPokemon.exp { get; set; }
		int IPokemon.eggsteps { get; set; }
		IList<Moves> IPokemon.firstmoves { get; set; }
		int IPokemon.obtainMode { get; set; }
		int IPokemon.obtainMap { get; set; }
		string IPokemon.obtainText { get; set; }
		int IPokemon.obtainLevel { get; set; }
		int IPokemon.hatchedMap { get; set; }
		int IPokemon.language { get; }
		string IPokemon.ot { get; set; }
		int IPokemon.otgender { get; set; }
		int IPokemon.abilityflag { get; set; }
		bool IPokemon.genderflag { get; set; }
		int IPokemon.natureflag { get; set; }
		bool IPokemon.shinyflag { get; set; }
		IList<Ribbons> IPokemon.ribbons { get; set; }
		int IPokemon.cool { get; set; }
		int IPokemon.beauty { get; set; }
		int IPokemon.cute { get; set; }
		int IPokemon.smart { get; set; }
		int IPokemon.tough { get; set; }
		int IPokemon.sheen { get; set; }
		int IPokemon.publicID { get; }
		DateTime? IPokemon.timeReceived { get; set; }
		DateTime? IPokemon.timeEggHatched { get; set; }
		bool IPokemon.egg { get; }
		int IPokemon.baseExp { get; }
		bool IPokemon.isSingleGendered { get; }
		char IPokemon.unownShape { get; }
		float IPokemon.height { get; }
		float IPokemon.weight { get; }
		int[] IPokemon.evYield { get; }
		string IPokemon.kind { get; }
		string IPokemon.dexEntry { get; }

		bool IPokemon.isForeign(ITrainer trainer)
		{
			throw new NotImplementedException();
		}

		bool IPokemon.isFemale(int b, int genderRate)
		{
			throw new NotImplementedException();
		}

		void IPokemon.setGender(int value)
		{
			throw new NotImplementedException();
		}

		bool IPokemon.hasNature(Natures? value)
		{
			throw new NotImplementedException();
		}

		void IPokemon.resetPokerusTime()
		{
			throw new NotImplementedException();
		}

		void IPokemon.lowerPokerusCount()
		{
			throw new NotImplementedException();
		}

		void IPokemon.pbDeleteMove(Moves move)
		{
			throw new NotImplementedException();
		}

		void IPokemon.pbDeleteMoveAtIndex(int index)
		{
			throw new NotImplementedException();
		}

		void IPokemon.pbDeleteAllMoves()
		{
			throw new NotImplementedException();
		}

		int IPokemon.ribbonCount()
		{
			throw new NotImplementedException();
		}

		int IPokemon.upgradeRibbon(params Ribbons[] arg)
		{
			throw new NotImplementedException();
		}

		void IPokemon.changeHappiness(HappinessMethods method)
		{
			throw new NotImplementedException();
		}

		void IPokemon.calcStat(int _base, int level, int iv, int ev, int pv)
		{
			throw new NotImplementedException();
		}

		IPokemon IPokemon.initialize(Pokemons species, int level, ITrainer player, bool withMoves)
		{
			throw new NotImplementedException();
		}
		#endregion	
		
		#region Explicit Operators
		public static bool operator ==(Pokemon x, Pokemon y)
		{
			if ((object)x == null && (object)y == null) return true;
			if ((object)x == null || (object)y == null) return false;
			return ((x.PersonalId == y.PersonalId) && (x.TrainerId == y.TrainerId) && (x.OT == y.OT)) & (x.Name == y.Name); //ToDo: If Gender is different, are pokemons the same? Check Date/Age of Pokemon?
		}
		public static bool operator !=(Pokemon x, Pokemon y)
		{
			if ((object)x == null && (object)y == null) return false;
			if ((object)x == null || (object)y == null) return true;
			return ((x.PersonalId != y.PersonalId) || (x.TrainerId != y.TrainerId) || (x.OT != y.OT)) | (x.Name == y.Name);
		}
		public bool Equals(Pokemon obj)
		{
			if (obj == null) return false;
			return this == obj;
		}
		//public bool Equals(PokemonUnity.Saving.SerializableClasses.SeriPokemon obj)
		//{
		//	//if (obj == null) return false;
		//	return this == obj; 
		//}
		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (obj.GetType() == typeof(Pokemon))
				return Equals((Pokemon)obj);
			//if (obj.GetType() == typeof(PokemonUnity.Saving.SerializableClasses.SeriPokemon))
			//	return Equals((PokemonUnity.Saving.SerializableClasses.SeriPokemon)obj);
			return base.Equals(obj);
		}
		public override int GetHashCode()
		{
			return PersonalId.GetHashCode();
		}
		bool IEquatable<Pokemon>.Equals(Pokemon other)
		{
			return Equals(obj: (object)other);
		}
		bool IEqualityComparer<Pokemon>.Equals(Pokemon x, Pokemon y)
		{
			return x == y;
		}
		int IEqualityComparer<Pokemon>.GetHashCode(Pokemon obj)
		{
			return obj.GetHashCode();
		}
		#endregion

		public override string ToString()
		{
			//return base.ToString();
			//if (pkmn is PokeBattle_Battler && !pkmn.pokemon) {
			//  return "";
			//}
			string status = "";
			//if (pkmn.HP <= 0)
			if (HP <= 0)
			{
				status = " [FNT]";
			}
			else
			{
				//switch (pkmn.Status) {
				switch (Status)
				{
					case Status.SLEEP:
						status = " [SLP]";
						break;
					case Status.FROZEN:
						status = " [FRZ]";
						break;
					case Status.BURN:
						status = " [BRN]";
						break;
					case Status.PARALYSIS:
						status = " [PAR]";
						break;
					case Status.POISON:
						status = " [PSN]";
						break;
				}
			}
			//return $"#{pkmn.Name} (Lv. #{pkmn.Level})#{status} HP: #{pkmn.HP}/#{pkmn.TotalHP}";
			return $"#{Name} (Lv. #{Level})#{status} HP: #{HP}/#{TotalHP}";
		}
	}
}