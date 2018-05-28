using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Pokemon;
using PokemonUnity.Move;
using Veekun;

public class Move //: MoveData
{
	#region Variables
	//private string name;
	//private string description;
	//private string fieldEffect;

	private int pp;
	private int ppups;
	/*private Pokemon.PokemonData.Type type;
	private Category category;
	private int power;
	private float accuracy;
	private Target target;
	private int priority;
	private bool contact;
	private bool protectable;
	private bool magicCoatable;
	private bool snatchable;
	private Effect[] moveEffects;
	private float[] moveParameters;
	private Contest contest;
	private int appeal;
	private int jamming;*/

	private MoveData _base;
	#endregion

	#region Properties
	/// <summary>
	/// The amount of PP remaining for this move
	/// </summary>
	public int PP { get { return this.pp; } }
	/// <summary>
	/// Gets the maximum PP for this move.
	/// </summary>
	public int TotalPP
	{
		get
		{
			return _base.PP + (int)Math.Floor(_base.PP * PPups / 5d);
		}
	}
	/// <summary>
	/// The number of PP Ups used for this move
	/// </summary>
	public int PPups { get { return this.ppups; } }
	/*// <summary>
	/// Gets this move's type.
	/// </summary>
	public Pokemon.PokemonData.Type Type { get { return _base.Type; } }*/
	public Target Targets { get { return _base.Target; } }
	public Types Type { get { return _base.Type; } }
	public Moves MoveId { get { return _base.ID; } }
	public string Name { get { return _base.Name; } }
	public string Description { get { return _base.Description; } }
	#endregion

	public Move() { }

	/// <summary>
	/// Initializes this object to the specified move ID.
	/// </summary>
	public Move(Moves move) { _base = new MoveData().getMove(move); pp = _base.PP; }

	#region Enumerator
	public enum Effect
	{
		/// <summary>
		/// if this check fails, no later effects will run
		/// </summary>
		Chance,
		/// <summary>
		/// for the very specific move effects
		/// </summary>
		Unique,
		Sound, Punch, Powder,
		/// <summary>
		/// (only works on 0= opposite, 1= opposite or genderless)
		/// </summary>
		Gender,
		Burn, Freeze, Paralyze, Poison, Sleep, Toxic,
		/// <summary>
		/// (chance)
		/// </summary>
		ATK, DEF, SPA, SPD, SPE, ACC, EVA,
		ATKself, DEFself, SPAself, SPDself, SPEself, ACCself, EVAself,
		/// <summary>
		/// (x=set, 0=2-5)
		/// </summary>
		xHits,
		/// <summary>
		/// (amount)
		/// </summary>
		Heal,
		/// <summary>
		/// (amount)
		/// </summary>
		HPDrain,
		/// <summary>
		/// (x=set, 0=level)
		/// </summary>
		SetDamage,
		/// <summary>
		/// based off of user's maxHp
		/// </summary>
		Critical, Recoil,
		Flinch, RecoilMax //Added these 2
	};
	/// <summary>
	/// Is one of the following:
	/// Physical, Special, or Status
	/// </summary>
	public enum Category
	{
		PHYSICAL,
		SPECIAL,
		STATUS
	};
	public enum Contest
	{
		COOL,
		BEAUTIFUL,
		CUTE,
		CLEVER,
		TOUGH
	}
	/// <summary>
	/// </summary>
	public enum Target
	{
		/// <summary>
		/// No target (i.e. Counter, Metal Burst, Mirror Coat, Curse)
		/// </summary>
		/// NONE
		NoTarget = 0,
		/// <summary>
		/// User
		/// </summary>
		/// SELF
		User,
		/// <summary>
		/// Single Pokémon other than the user
		/// </summary>
		/// SINGLEOTHER
		SingleNonUser,
		/// <summary>
		/// Single opposing Pokémon selected at random (i.e. Outrage, Petal Dance, Thrash, Uproar)
		/// </summary>
		/// SINGLERANDOMOPPONENT
		RandomOpposing,
		/// <summary>
		/// User's partner (i.e. Helping Hand)
		/// </summary>
		/// ADJACENTALLY
		Partner,
		/// <summary>
		/// Single opposing Pokémon (i.e. Me First)
		/// </summary>
		/// SINGLEOPPONENT
		SingleOpposing,
		/// <summary>
		/// Single opposing Pokémon directly opposite of user
		/// </summary>
		OppositeOpposing,
		/// <summary>
		/// Single Pokémon on user's side (i.e. Acupressure)
		/// </summary>
		/// SINGLEALLYSELF
		UserOrPartner,
		/// <summary>
		/// Both sides (e.g. Sunny Day, Trick Room)
		/// </summary>
		/// ALLFIELD
		BothSides,
		/// <summary>
		/// All Pokémon other than the user
		/// </summary>
		/// ALLOTHERS
		AllNonUsers,
		/// <summary>
		/// Opposing side (i.e. Spikes, Toxic Spikes, Stealth Rocks)
		/// </summary>
		/// ALLOPPONENTFIELD
		OpposingSide,
		/// <summary>
		/// All opposing Pokémon
		/// </summary>
		/// ALLOPPONENT
		AllOpposing,
		/// <summary>
		/// User's side (e.g. Light Screen, Mist)
		/// </summary>
		/// ALLALLYFIELD
		UserSide
	}
	#endregion

	#region Methods
	//public Move Clone() { }
	#endregion

	#region Nested Classes
	public class MoveData
	{
		#region Variables
		/// <summary>
		/// totalpp
		/// </summary>
		private int pp { get; set; }
		private Moves id { get; set; }
		/// <summary>
		/// The move's base power value. Status moves have a base power of 0, 
		/// while moves with a variable base power are defined here with a base power of 1. 
		/// For multi-hit moves, this is the base power of a single hit.
		/// </summary>
		private int basedamage;
		private Types type;
		/// <summary>
		/// The move's accuracy, as a percentage. 
		/// An accuracy of 0 means the move doesn't perform an accuracy check 
		/// (i.e. it cannot be evaded).
		/// </summary>
		private int accuracy;
		/// <summary>
		/// The probability that the move's additional effect occurs, as a percentage. 
		/// If the move has no additional effect (e.g. all status moves), this value is 0. 
		/// <para></para>
		/// Note that some moves have an additional effect chance of 100 (e.g.Acid Spray), 
		/// which is not the same thing as having an effect that will always occur.
		/// Abilities like Sheer Force and Shield Dust only affect additional effects, 
		/// not regular effects.
		/// </summary>
		private float addlEffect;
		/// <summary>
		/// The Pokémon that the move will strike.
		/// </summary>
		private Target target;
		/// <summary>
		/// The move's priority, between -6 and 6 inclusive. This is usually 0. 
		/// A higher priority move will be used before all moves of lower priority, 
		/// regardless of Speed calculations. 
		/// Moves with equal priority will be used depending on which move user is faster.
		/// <example>For example, Quick Attack has a priority of 1.</example>
		/// </summary>
		private int priority;
        /// <summary>
        /// [Deprecated]
        /// </summary>
        /// ToDo: Instead of an Enum;
        /// Make into a class of bool values defaulted to false
        /// OR find a way to set/default uncalled values in array
        [Obsolete]
		private Veekun.Flags[] flagsEnum;
        private Flags flags = new Flags();
		private Category category;
        //everything below is influenced from pokemon-showdown
        private bool secondary; // { int chance; boosts; status/effect }
        private Contest contestType;
        //enum volatileStatus
        //enum sideCondition
        //class effect
        //string isZ;
        //accuracy == true | 0-100?
        //boosts { int stats = 0; } else null?
        //bool isViable = false
        //enum zMoveEffect; int zMovePower; boost zMoveBoost 
        #endregion

        #region Properties
        public int PP { get { return pp; } }
		public Moves ID { get { return id; } }
		public Target Target { get { return target; } }
		public Types Type { get { return type; } }
		public string Name { get; private set; }
		public string Description { get; private set; }
		#endregion

		#region Enumerator
		#endregion

		#region Methods
		#region Database
		private static readonly MoveData[] Database = new MoveData[]
		{
			null,
			CreateMoveData(Moves.Absorb, Types.GRASS, Category.SPECIAL, 20, 1f, 25, TargetB.ADJACENT,
				0, false, true, false, false, new Effect[] {Effect.HPDrain}, new float[] {1},
				Contest.CLEVER, 4, 0),
			CreateMoveData(Moves.Acrobatics, Types.FLYING, Category.PHYSICAL, 55, 1f, 15, TargetB.ANY,
				0, true, true, false, false, new Effect[] {}, new float[] {}, Contest.COOL, 1, 0)/*,
			CreateMoveData(Move.Aerial_Ace, Pokemon.PokemonData.Type.FLYING, Category.PHYSICAL, 60, 0, 20, Contest.COOL,
				2, 0),
			CreateMoveData(Move.After_You, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0, 15, Contest.CUTE, 3,
				0),
			CreateMoveData(Move.Agility, Pokemon.PokemonData.Type.PSYCHIC, Category.STATUS, 0, 0, 30, TargetB.SELF, 0,
				false, false, false, true, new Effect[] {Effect.SPEself}, new float[] {2},
				Contest.COOL, 3, 0),
			CreateMoveData(Move.Air_Cutter, Pokemon.PokemonData.Type.FLYING, Category.SPECIAL, 60, 0.95f, 25,
				Contest.COOL, 4, 0, "Cut"),
			CreateMoveData(Move.Air_Slash, Pokemon.PokemonData.Type.FLYING, Category.SPECIAL, 75, 0.95f, 15,
				Contest.COOL, 1, 4, "Cut"),
			CreateMoveData(Move.Amnesia, Pokemon.PokemonData.Type.PSYCHIC, Category.STATUS, 0, 0, 20, Contest.CUTE, 1,
				0),
			CreateMoveData(Move.Ancient_Power, Pokemon.PokemonData.Type.ROCK, Category.SPECIAL, 60, 1f, 5,
				Contest.TOUGH, 1, 0),
			CreateMoveData(Move.Aqua_Jet, Pokemon.PokemonData.Type.WATER, Category.PHYSICAL, 40, 1f, 20,
				TargetB.ADJACENT, 1, true, true, false, false, new Effect[] {}, new float[] {},
				Contest.COOL, 3, 0),
			CreateMoveData(Move.Aqua_Ring, Pokemon.PokemonData.Type.WATER, Category.STATUS, 0, 0, 20, Contest.BEAUTIFUL,
				1, 0),
			CreateMoveData(Move.Aqua_Tail, Pokemon.PokemonData.Type.WATER, Category.PHYSICAL, 90, 0.90f, 10,
				Contest.BEAUTIFUL, 4, 0),
			CreateMoveData(Move.Arm_Thrust, Pokemon.PokemonData.Type.FIGHTING, Category.PHYSICAL, 15, 1f, 20,
				Contest.TOUGH, 1, 0),
			CreateMoveData(Move.Aromatherapy, Pokemon.PokemonData.Type.GRASS, Category.STATUS, 0, 0, 5, Contest.CLEVER,
				1, 0),
			CreateMoveData(Move.Assist, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0, 20, Contest.CUTE, 1, 0),
			CreateMoveData(Move.Assurance, Pokemon.PokemonData.Type.DARK, Category.PHYSICAL, 60, 1f, 10, Contest.CLEVER,
				1, 0),
			CreateMoveData(Move.Astonish, Pokemon.PokemonData.Type.GHOST, Category.PHYSICAL, 30, 1f, 15, Contest.CUTE,
				2, 3),
			CreateMoveData(Move.Attract, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 1f, 15, Contest.CUTE, 2,
				0),
			CreateMoveData(Move.Aura_Sphere, Pokemon.PokemonData.Type.FIGHTING, Category.SPECIAL, 80, 0, 20,
				Contest.BEAUTIFUL, 2, 0),
			CreateMoveData(Move.Baby_Doll_Eyes, Pokemon.PokemonData.Type.FAIRY, Category.STATUS, 0, 1f, 30,
				TargetB.ADJACENT, 1, false, true, true, true, new Effect[] {Effect.ATK},
				new float[] {-1}, Contest.CUTE, 3, 0),
			CreateMoveData(Move.Baton_Pass, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0, 40, Contest.CUTE, 1,
				0),
			CreateMoveData(Move.Belly_Drum, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0, 10, Contest.CUTE, 6,
				0),
			CreateMoveData(Move.Bestow, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0, 15, Contest.CUTE, 1, 0),
			CreateMoveData(Move.Bide, Pokemon.PokemonData.Type.NORMAL, Category.PHYSICAL, 0, 0, 10, Contest.TOUGH, 3, 0),
			CreateMoveData(Move.Bite, Pokemon.PokemonData.Type.DARK, Category.PHYSICAL, 60, 1f, 25, Contest.TOUGH, 2, 3),
			CreateMoveData(Move.Blaze_Kick, Pokemon.PokemonData.Type.FIRE, Category.PHYSICAL, 85, 0.90f, 10,
				Contest.COOL, 3, 0),
			CreateMoveData(Move.Block, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0, 5, Contest.CUTE, 2, 0),
			CreateMoveData(Move.Body_Slam, Pokemon.PokemonData.Type.NORMAL, Category.PHYSICAL, 85, 1f, 15,
				Contest.TOUGH, 1, 4),
			CreateMoveData(Move.Bone_Rush, Pokemon.PokemonData.Type.GROUND, Category.PHYSICAL, 25, 0.90f, 10,
				Contest.TOUGH, 1, 0),
			CreateMoveData(Move.Bounce, Pokemon.PokemonData.Type.FLYING, Category.PHYSICAL, 85, 0.85f, 5, Contest.CUTE,
				1, 0),
			CreateMoveData(Move.Brave_Bird, Pokemon.PokemonData.Type.FLYING, Category.PHYSICAL, 120, 1f, 15,
				Contest.COOL, 6, 0),
			CreateMoveData(Move.Brine, Pokemon.PokemonData.Type.WATER, Category.SPECIAL, 65, 1f, 10, Contest.TOUGH, 3,
				0),
			CreateMoveData(Move.Bubble, Pokemon.PokemonData.Type.WATER, Category.SPECIAL, 40, 1f, 30, Contest.CUTE, 4,
				0),
			CreateMoveData(Move.Bubble_Beam, Pokemon.PokemonData.Type.WATER, Category.SPECIAL, 65, 1f, 20,
				Contest.BEAUTIFUL, 2, 3),
			CreateMoveData(Move.Bug_Bite, Pokemon.PokemonData.Type.BUG, Category.PHYSICAL, 60, 1f, 20, Contest.CUTE, 3,
				0),
			CreateMoveData(Move.Bug_Buzz, Pokemon.PokemonData.Type.BUG, Category.SPECIAL, 90, 1f, 10, Contest.BEAUTIFUL,
				1, 4),
			CreateMoveData(Move.Bulk_Up, Pokemon.PokemonData.Type.FIGHTING, Category.STATUS, 0, 0, 20, Contest.COOL, 1,
				0),
			CreateMoveData(Move.Calm_Mind, Pokemon.PokemonData.Type.PSYCHIC, Category.STATUS, 0, 0, 20, TargetB.SELF, 0,
				false, false, false, true, new Effect[] {Effect.SPAself, Effect.SPDself},
				new float[] {1, 1}, Contest.CLEVER, 1, 0),
			CreateMoveData(Move.Captivate, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 1f, 20, TargetB.ADJACENT,
				0, false, true, true, false, new Effect[] {Effect.Gender, Effect.SPA},
				new float[] {1, -2}, Contest.CUTE, 4, 0),
			CreateMoveData(Move.Charm, Pokemon.PokemonData.Type.FAIRY, Category.STATUS, 0, 1f, 20, TargetB.ADJACENT, 0,
				false, true, true, false, new Effect[] {Effect.ATK}, new float[] {-2},
				Contest.CUTE, 2, 1),
			CreateMoveData(Move.Chip_Away, Pokemon.PokemonData.Type.NORMAL, Category.PHYSICAL, 70, 1f, 20,
				Contest.TOUGH, 2, 0),
			CreateMoveData(Move.Clear_Smog, Pokemon.PokemonData.Type.POISON, Category.SPECIAL, 50, 0, 15,
				Contest.BEAUTIFUL, 2, 0),
			CreateMoveData(Move.Close_Combat, Pokemon.PokemonData.Type.FIGHTING, Category.PHYSICAL, 120, 1f, 5,
				TargetB.ADJACENT, 0, true, true, false, false,
				new Effect[] {Effect.DEFself, Effect.SPDself}, new float[] {-1, -1},
				Contest.TOUGH, 6, 0),
			CreateMoveData(Move.Coil, Pokemon.PokemonData.Type.POISON, Category.STATUS, 0, 0, 20, TargetB.ADJACENT, 0,
				false, false, false, true,
				new Effect[] {Effect.ATKself, Effect.DEFself, Effect.ACCself},
				new float[] {1, 1, 1}, Contest.TOUGH, 1, 0),
			CreateMoveData(Move.Confuse_Ray, Pokemon.PokemonData.Type.GHOST, Category.STATUS, 0, 1f, 10, Contest.CLEVER,
				2, 1),
			CreateMoveData(Move.Confusion, Pokemon.PokemonData.Type.PSYCHIC, Category.SPECIAL, 50, 1f, 25,
				Contest.CLEVER, 4, 0),
			CreateMoveData(Move.Constrict, Pokemon.PokemonData.Type.NORMAL, Category.PHYSICAL, 10, 1f, 35,
				Contest.TOUGH, 3, 0),
			CreateMoveData(Move.Copycat, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0, 20, Contest.CUTE, 1, 0),
			CreateMoveData(Move.Counter, Pokemon.PokemonData.Type.FIGHTING, Category.PHYSICAL, 0, 1f, 20, Contest.TOUGH,
				2, 0),
			CreateMoveData(Move.Covet, Pokemon.PokemonData.Type.NORMAL, Category.PHYSICAL, 60, 1f, 25, Contest.CUTE, 1,
				0),
			CreateMoveData(Move.Cross_Chop, Pokemon.PokemonData.Type.FIGHTING, Category.PHYSICAL, 100, 0.80f, 5,
				Contest.COOL, 3, 0),
			CreateMoveData(Move.Cross_Poison, Pokemon.PokemonData.Type.POISON, Category.PHYSICAL, 70, 1f, 20,
				Contest.COOL, 2, 1),
			CreateMoveData(Move.Crunch, Pokemon.PokemonData.Type.DARK, Category.PHYSICAL, 80, 1f, 15, Contest.TOUGH, 1,
				4),
			CreateMoveData(Move.Curse, Pokemon.PokemonData.Type.GHOST, Category.STATUS, 0, 0, 10, Contest.TOUGH, 3, 0),
			CreateMoveData(Move.Dark_Pulse, Pokemon.PokemonData.Type.DARK, Category.SPECIAL, 80, 1f, 15, Contest.COOL,
				3, 0),
			CreateMoveData(Move.Defense_Curl, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0, 40, Contest.CUTE,
				2, 0),
			CreateMoveData(Move.Defog, Pokemon.PokemonData.Type.FLYING, Category.STATUS, 0, 0, 15, Contest.COOL, 2, 0),
			CreateMoveData(Move.Detect, Pokemon.PokemonData.Type.FIGHTING, Category.STATUS, 0, 0, 5, Contest.COOL, 1, 0),
			CreateMoveData(Move.Disarming_Voice, Pokemon.PokemonData.Type.FAIRY, Category.SPECIAL, 40, 1f, 15,
				Contest.CUTE, 2, 0),
			CreateMoveData(Move.Dizzy_Punch, Pokemon.PokemonData.Type.NORMAL, Category.PHYSICAL, 70, 1f, 10,
				Contest.CUTE, 3, 0),
			CreateMoveData(Move.Double_Edge, Pokemon.PokemonData.Type.NORMAL, Category.PHYSICAL, 120, 1f, 15,
				Contest.TOUGH, 6, 0),
			CreateMoveData(Move.Double_Kick, Pokemon.PokemonData.Type.FIGHTING, Category.PHYSICAL, 30, 1f, 30,
				Contest.COOL, 2, 1),
			CreateMoveData(Move.Double_Slap, Pokemon.PokemonData.Type.NORMAL, Category.PHYSICAL, 15, 0.85f, 10,
				Contest.CUTE, 1, 0),
			CreateMoveData(Move.Double_Team, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0, 15, Contest.COOL,
				1, 0),
			CreateMoveData(Move.Dragon_Claw, Pokemon.PokemonData.Type.DRAGON, Category.PHYSICAL, 80, 1f, 15,
				Contest.COOL, 4, 0, "Cut"),
			CreateMoveData(Move.Dragon_Dance, Pokemon.PokemonData.Type.DRAGON, Category.STATUS, 0, 0, 20, Contest.COOL,
				1, 0),
			CreateMoveData(Move.Dragon_Rage, Pokemon.PokemonData.Type.DRAGON, Category.SPECIAL, 0, 1f, 10,
				TargetB.ADJACENT, 0, false, true, false, false, new Effect[] {Effect.SetDamage},
				new float[] {40}, Contest.COOL, 3, 0,
				"The foe is stricken by a shock wave. This attack always inflicts 40 HP damage."),
			CreateMoveData(Move.Dragon_Pulse, Pokemon.PokemonData.Type.DRAGON, Category.SPECIAL, 85, 1f, 10,
				Contest.BEAUTIFUL, 4, 0),
			CreateMoveData(Move.Dream_Eater, Pokemon.PokemonData.Type.PSYCHIC, Category.SPECIAL, 100, 1f, 15,
				Contest.CLEVER, 2, 0),
			CreateMoveData(Move.Drill_Peck, Pokemon.PokemonData.Type.FLYING, Category.PHYSICAL, 80, 1f, 20,
				Contest.COOL, 3, 0),
			CreateMoveData(Move.Drill_Run, Pokemon.PokemonData.Type.GROUND, Category.PHYSICAL, 80, 0.95f, 10,
				Contest.TOUGH, 3, 0),
			CreateMoveData(Move.Dual_Chop, Pokemon.PokemonData.Type.DRAGON, Category.PHYSICAL, 40, 0.90f, 15,
				Contest.TOUGH, 2, 1),
			CreateMoveData(Move.Dynamic_Punch, Pokemon.PokemonData.Type.FIGHTING, Category.PHYSICAL, 100, 0.50f, 5,
				Contest.COOL, 2, 1),
			CreateMoveData(Move.Earth_Power, Pokemon.PokemonData.Type.GROUND, Category.SPECIAL, 90, 1f, 10,
				Contest.BEAUTIFUL, 4, 0),
			CreateMoveData(Move.Earthquake, Pokemon.PokemonData.Type.GROUND, Category.PHYSICAL, 100, 1f, 10,
				Contest.TOUGH, 2, 1),
			CreateMoveData(Move.Echoed_Voice, Pokemon.PokemonData.Type.NORMAL, Category.SPECIAL, 40, 1f, 15,
				Contest.BEAUTIFUL, 3, 0),
			CreateMoveData(Move.Ember, Pokemon.PokemonData.Type.FIRE, Category.SPECIAL, 40, 1f, 25, Contest.CUTE, 4, 0),
			CreateMoveData(Move.Encore, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 1f, 5, Contest.CUTE, 2, 0),
			CreateMoveData(Move.Endeavor, Pokemon.PokemonData.Type.NORMAL, Category.PHYSICAL, 0, 1f, 5, Contest.TOUGH,
				2, 1),
			CreateMoveData(Move.Endure, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0, 10, Contest.TOUGH, 3, 0),
			CreateMoveData(Move.Entrainment, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 1f, 15, Contest.CUTE,
				2, 1),
			CreateMoveData(Move.Eruption, Pokemon.PokemonData.Type.FIRE, Category.SPECIAL, 0, 1f, 5, Contest.BEAUTIFUL,
				6, 0),
			CreateMoveData(Move.Extrasensory, Pokemon.PokemonData.Type.PSYCHIC, Category.SPECIAL, 80, 1f, 20,
				Contest.COOL, 2, 1),
			CreateMoveData(Move.Extreme_Speed, Pokemon.PokemonData.Type.NORMAL, Category.PHYSICAL, 80, 1f, 5,
				TargetB.ADJACENT, 2, true, true, false, false, new Effect[] {}, new float[] {},
				Contest.COOL, 3, 0),
			CreateMoveData(Move.Fake_Out, Pokemon.PokemonData.Type.NORMAL, Category.PHYSICAL, 40, 1f, 10,
				TargetB.ADJACENT, 3, false, true, false, false, new Effect[] {}, new float[] {},
				Contest.CUTE, 2, 3),
			CreateMoveData(Move.False_Swipe, Pokemon.PokemonData.Type.NORMAL, Category.PHYSICAL, 40, 1f, 40,
				Contest.COOL, 4, 0),
			CreateMoveData(Move.Feather_Dance, Pokemon.PokemonData.Type.FLYING, Category.STATUS, 0, 1f, 15,
				Contest.BEAUTIFUL, 2, 0),
			CreateMoveData(Move.Feint, Pokemon.PokemonData.Type.NORMAL, Category.PHYSICAL, 30, 1f, 10, Contest.CLEVER,
				3, 0),
			CreateMoveData(Move.Feint_Attack, Pokemon.PokemonData.Type.DARK, Category.PHYSICAL, 60, 0, 20,
				Contest.CLEVER, 2, 0),
			CreateMoveData(Move.Fell_Stinger, Pokemon.PokemonData.Type.BUG, Category.PHYSICAL, 30, 1f, 25, Contest.COOL,
				1, 0),
			CreateMoveData(Move.Final_Gambit, Pokemon.PokemonData.Type.FIGHTING, Category.SPECIAL, 0, 1f, 5,
				Contest.TOUGH, 8, 0),
			CreateMoveData(Move.Fire_Blast, Pokemon.PokemonData.Type.FIRE, Category.SPECIAL, 110, 0.85f, 5,
				TargetB.ADJACENT, 0, false, true, false, false, new Effect[] {Effect.Burn},
				new float[] {0.1f}, Contest.BEAUTIFUL, 1, 0),
			CreateMoveData(Move.Fire_Punch, Pokemon.PokemonData.Type.FIRE, Category.PHYSICAL, 75, 1f, 15,
				TargetB.ADJACENT, 0, true, true, false, false, new Effect[] {Effect.Burn},
				new float[] {0.1f}, Contest.TOUGH, 4, 0),
			CreateMoveData(Move.Fire_Fang, Pokemon.PokemonData.Type.FIRE, Category.PHYSICAL, 65, 0.95f, 15,
				TargetB.ADJACENT, 0, true, true, false, false,
				new Effect[] {Effect.Burn, Effect.Flinch}, new float[] {0.1f, 0.1f},
				Contest.COOL, 4, 0,
				"The user bites with flame-cloaked \nfangs. It may also make the foe \nflinch or sustain a burn."),
			CreateMoveData(Move.Fire_Spin, Pokemon.PokemonData.Type.FIRE, Category.SPECIAL, 35, 0.85f, 15,
				Contest.BEAUTIFUL, 3, 0),
			CreateMoveData(Move.Fissure, Pokemon.PokemonData.Type.GROUND, Category.PHYSICAL, 0, 0, 5, Contest.TOUGH, 2,
				1),
			CreateMoveData(Move.Flail, Pokemon.PokemonData.Type.NORMAL, Category.PHYSICAL, 0, 1f, 15, Contest.CUTE, 1,
				0),
			CreateMoveData(Move.Flame_Charge, Pokemon.PokemonData.Type.FIRE, Category.PHYSICAL, 50, 1f, 20,
				TargetB.ADJACENT, 0, true, true, false, false, new Effect[] {Effect.SPEself},
				new float[] {1}, Contest.COOL, 1, 0),
			CreateMoveData(Move.Flame_Burst, Pokemon.PokemonData.Type.FIRE, Category.SPECIAL, 70, 1f, 15,
				Contest.BEAUTIFUL, 3, 0),
			CreateMoveData(Move.Flamethrower, Pokemon.PokemonData.Type.FIRE, Category.SPECIAL, 90, 1f, 15,
				TargetB.ADJACENT, 0, false, true, false, false, new Effect[] {Effect.Burn},
				new float[] {0.1f}, Contest.BEAUTIFUL, 4, 0),
			CreateMoveData(Move.Flame_Wheel, Pokemon.PokemonData.Type.FIRE, Category.PHYSICAL, 60, 1f, 25,
				TargetB.ADJACENT, 0, true, true, false, false, new Effect[] {Effect.Burn},
				new float[] {0.1f}, Contest.BEAUTIFUL, 3, 0),
			CreateMoveData(Move.Flare_Blitz, Pokemon.PokemonData.Type.FIRE, Category.PHYSICAL, 120, 1f, 15,
				Contest.COOL, 6, 0),
			CreateMoveData(Move.Flash_Cannon, Pokemon.PokemonData.Type.STEEL, Category.SPECIAL, 80, 1f, 10,
				Contest.BEAUTIFUL, 4, 0),
			CreateMoveData(Move.Flatter, Pokemon.PokemonData.Type.DARK, Category.STATUS, 0, 1f, 15, Contest.CLEVER, 2,
				0),
			CreateMoveData(Move.Focus_Energy, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0, 30, Contest.COOL,
				1, 0),
			CreateMoveData(Move.Follow_Me, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0, 20, Contest.CUTE, 3,
				0),
			CreateMoveData(Move.Force_Palm, Pokemon.PokemonData.Type.FIGHTING, Category.PHYSICAL, 60, 1f, 10,
				Contest.COOL, 4, 0),
			CreateMoveData(Move.Foresight, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0, 40, Contest.CLEVER,
				2, 1),
			CreateMoveData(Move.Frustration, Pokemon.PokemonData.Type.NORMAL, Category.PHYSICAL, 0, 1f, 20,
				Contest.CUTE, 2, 3),
			CreateMoveData(Move.Fury_Attack, Pokemon.PokemonData.Type.NORMAL, Category.PHYSICAL, 15, 0.85f, 20,
				Contest.COOL, 1, 0),
			CreateMoveData(Move.Fury_Cutter, Pokemon.PokemonData.Type.BUG, Category.PHYSICAL, 40, 0.95f, 20,
				Contest.COOL, 3, 0, "Cut"),
			CreateMoveData(Move.Fury_Swipes, Pokemon.PokemonData.Type.NORMAL, Category.PHYSICAL, 18, 0.80f, 15,
				Contest.TOUGH, 1, 0),
			CreateMoveData(Move.Future_Sight, Pokemon.PokemonData.Type.PSYCHIC, Category.SPECIAL, 120, 1f, 10,
				Contest.CLEVER, 2, 0),
			CreateMoveData(Move.Gastro_Acid, Pokemon.PokemonData.Type.POISON, Category.STATUS, 0, 1f, 10, Contest.TOUGH,
				3, 0),
			CreateMoveData(Move.Giga_Drain, Pokemon.PokemonData.Type.GRASS, Category.SPECIAL, 75, 1f, 10,
				TargetB.ADJACENT, 0, false, true, false, false, new Effect[] {Effect.HPDrain},
				new float[] {0.5f}, Contest.CLEVER, 1, 4),
			CreateMoveData(Move.Giga_Impact, Pokemon.PokemonData.Type.GRASS, Category.PHYSICAL, 150, 0.90f, 5,
				Contest.TOUGH, 4, 4),
			CreateMoveData(Move.Grassy_Terrain, Pokemon.PokemonData.Type.GRASS, Category.STATUS, 0, 0, 10,
				Contest.BEAUTIFUL, 3, 0),
			CreateMoveData(Move.Grass_Whistle, Pokemon.PokemonData.Type.GRASS, Category.STATUS, 0, 0.55f, 15,
				TargetB.ADJACENT, 0, false, true, true, false, new Effect[] {Effect.Sleep},
				new float[] {1}, Contest.CLEVER, 2, 0),
			CreateMoveData(Move.Growl, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 1f, 40,
				TargetB.ALLADJACENTOPPONENT, 0, false, true, true, false,
				new Effect[] {Effect.ATK, Effect.Sound}, new float[] {-1, 0},
				Contest.CUTE, 2, 0),
			CreateMoveData(Move.Growth, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0, 20, Contest.BEAUTIFUL,
				1, 0),
			CreateMoveData(Move.Guard_Swap, Pokemon.PokemonData.Type.PSYCHIC, Category.STATUS, 0, 0, 10, Contest.CLEVER,
				1, 0),
			CreateMoveData(Move.Gust, Pokemon.PokemonData.Type.FLYING, Category.SPECIAL, 40, 1f, 35, Contest.CLEVER, 2,
				3),
			CreateMoveData(Move.Gyro_Ball, Pokemon.PokemonData.Type.STEEL, Category.PHYSICAL, 0, 1f, 5, Contest.COOL, 1,
				0),
			CreateMoveData(Move.Hammer_Arm, Pokemon.PokemonData.Type.FIGHTING, Category.PHYSICAL, 100, 0.90f, 10,
				Contest.TOUGH, 6, 0),
			CreateMoveData(Move.Harden, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0, 30, Contest.TOUGH, 2, 0),
			CreateMoveData(Move.Haze, Pokemon.PokemonData.Type.ICE, Category.STATUS, 0, 0, 30, Contest.BEAUTIFUL, 3, 0),
			CreateMoveData(Move.Headbutt, Pokemon.PokemonData.Type.NORMAL, Category.PHYSICAL, 70, 1f, 15, Contest.TOUGH,
				4, 0),
			CreateMoveData(Move.Head_Smash, Pokemon.PokemonData.Type.ROCK, Category.PHYSICAL, 150, 0.80f, 5,
				Contest.TOUGH, 6, 0),
			CreateMoveData(Move.Heal_Bell, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0, 5, Contest.BEAUTIFUL,
				1, 0),
			CreateMoveData(Move.Healing_Wish, Pokemon.PokemonData.Type.PSYCHIC, Category.STATUS, 0, 0, 10,
				Contest.BEAUTIFUL, 8, 0),
			CreateMoveData(Move.Heal_Pulse, Pokemon.PokemonData.Type.PSYCHIC, Category.STATUS, 0, 0, 10, TargetB.ANY, 0,
				false, true, true, false, new Effect[] {Effect.Heal}, new float[] {0.5f},
				Contest.BEAUTIFUL, 2, 0,
				"The user emits a healing pulse which restores the target's HP by up to half of its max HP."),
			CreateMoveData(Move.Heat_Crash, Pokemon.PokemonData.Type.FIRE, Category.PHYSICAL, 0, 1f, 10, Contest.TOUGH,
				3, 0),
			CreateMoveData(Move.Heat_Wave, Pokemon.PokemonData.Type.FIRE, Category.SPECIAL, 95, 0.90f, 10,
				Contest.BEAUTIFUL, 2, 2),
			//CreateMoveData(Move.Heavy_Crash, Pokemon.PokemonData.Type.STEEL, Category.PHYSICAL, 0, 1f, 10,
			//	Contest.TOUGH, 3, 0),
			CreateMoveData(Move.Helping_Hand, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0, 20,
				Contest.CLEVER, 4, 0),
			CreateMoveData(Move.Hex, Pokemon.PokemonData.Type.GHOST, Category.SPECIAL, 6, 1f, 10, Contest.CLEVER, 2, 1),
			CreateMoveData(Move.High_Jump_Kick, Pokemon.PokemonData.Type.FIGHTING, Category.PHYSICAL, 130, 0.90f, 10,
				Contest.COOL, 6, 0),
			CreateMoveData(Move.Horn_Attack, Pokemon.PokemonData.Type.NORMAL, Category.PHYSICAL, 65, 1f, 25,
				Contest.COOL, 4, 0),
			CreateMoveData(Move.Horn_Drill, Pokemon.PokemonData.Type.NORMAL, Category.PHYSICAL, 0, 0.30f, 5,
				Contest.COOL, 2, 0),
			CreateMoveData(Move.Howl, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0, 40, Contest.COOL, 2, 0),
			CreateMoveData(Move.Hurricane, Pokemon.PokemonData.Type.FLYING, Category.SPECIAL, 110, 0.70f, 10,
				Contest.TOUGH, 2, 1),
			CreateMoveData(Move.Hydro_Pump, Pokemon.PokemonData.Type.WATER, Category.SPECIAL, 110, 0.80f, 5,
				Contest.BEAUTIFUL, 1, 0),
			CreateMoveData(Move.Hyper_Beam, Pokemon.PokemonData.Type.NORMAL, Category.SPECIAL, 150, 0.90f, 5,
				Contest.COOL, 4, 4),
			CreateMoveData(Move.Hyper_Fang, Pokemon.PokemonData.Type.NORMAL, Category.PHYSICAL, 80, 0.90f, 15,
				Contest.COOL, 3, 0),
			CreateMoveData(Move.Hyper_Voice, Pokemon.PokemonData.Type.NORMAL, Category.SPECIAL, 90, 1f, 10,
				Contest.COOL, 2, 2),
			CreateMoveData(Move.Hypnosis, Pokemon.PokemonData.Type.PSYCHIC, Category.STATUS, 0, 0.60f, 20,
				TargetB.ADJACENT, 0, false, true, true, false, new Effect[] {Effect.Sleep},
				new float[] {1}, Contest.CLEVER, 1, 3),
			CreateMoveData(Move.Ice_Fang, Pokemon.PokemonData.Type.ICE, Category.PHYSICAL, 65, 0.95f, 15,
				TargetB.ADJACENT, 0, true, true, false, false,
				new Effect[] {Effect.Freeze, Effect.Flinch}, new float[] {0.1f, 0.1f},
				Contest.COOL, 4, 0),
			CreateMoveData(Move.Imprison, Pokemon.PokemonData.Type.PSYCHIC, Category.STATUS, 0, 0, 10, Contest.CLEVER,
				3, 0),
			CreateMoveData(Move.Incinerate, Pokemon.PokemonData.Type.FIRE, Category.SPECIAL, 60, 1f, 15, Contest.TOUGH,
				3, 0),
			CreateMoveData(Move.Inferno, Pokemon.PokemonData.Type.FIRE, Category.SPECIAL, 100, 0.5f, 5,
				Contest.BEAUTIFUL, 1, 4),
			CreateMoveData(Move.Ingrain, Pokemon.PokemonData.Type.GRASS, Category.STATUS, 0, 0, 20, Contest.CLEVER, 1,
				0),
			CreateMoveData(Move.Iron_Defense, Pokemon.PokemonData.Type.STEEL, Category.STATUS, 0, 0, 15, Contest.TOUGH,
				1, 0),
			CreateMoveData(Move.Jump_Kick, Pokemon.PokemonData.Type.FIGHTING, Category.PHYSICAL, 100, 0.95f, 10,
				Contest.COOL, 6, 0),
			CreateMoveData(Move.Karate_Chop, Pokemon.PokemonData.Type.FIGHTING, Category.PHYSICAL, 50, 1f, 25,
				Contest.TOUGH, 4, 0),
			CreateMoveData(Move.Knock_Off, Pokemon.PokemonData.Type.DARK, Category.PHYSICAL, 65, 1f, 25, Contest.CLEVER,
				2, 3),
			CreateMoveData(Move.Last_Resort, Pokemon.PokemonData.Type.NORMAL, Category.PHYSICAL, 140, 1f, 5,
				Contest.CUTE, 1, 0),
			CreateMoveData(Move.Lava_Plume, Pokemon.PokemonData.Type.FIRE, Category.SPECIAL, 80, 1f, 15, Contest.TOUGH,
				2, 2),
			CreateMoveData(Move.Leaf_Blade, Pokemon.PokemonData.Type.GRASS, Category.PHYSICAL, 90, 1f, 15, Contest.COOL,
				3, 0, "Cut"),
			CreateMoveData(Move.Leaf_Storm, Pokemon.PokemonData.Type.GRASS, Category.SPECIAL, 130, 0.90f, 5,
				Contest.BEAUTIFUL, 6, 0),
			CreateMoveData(Move.Leaf_Tornado, Pokemon.PokemonData.Type.GRASS, Category.SPECIAL, 65, 0.9f, 10,
				Contest.COOL, 3, 0),
			CreateMoveData(Move.Leech_Life, Pokemon.PokemonData.Type.BUG, Category.PHYSICAL, 20, 1f, 15, Contest.CLEVER,
				1, 0),
			CreateMoveData(Move.Leech_Seed, Pokemon.PokemonData.Type.GRASS, Category.STATUS, 0, 0.9f, 10,
				Contest.CLEVER, 1, 0),
			CreateMoveData(Move.Leer, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 1f, 30, Contest.COOL, 2, 1),
			CreateMoveData(Move.Lick, Pokemon.PokemonData.Type.GHOST, Category.PHYSICAL, 30, 1f, 30, Contest.TOUGH, 2,
				3),
			CreateMoveData(Move.Light_Screen, Pokemon.PokemonData.Type.PSYCHIC, Category.STATUS, 0, 0, 30,
				Contest.BEAUTIFUL, 2, 0),
			CreateMoveData(Move.Low_Kick, Pokemon.PokemonData.Type.FIGHTING, Category.PHYSICAL, 0, 1f, 20,
				Contest.TOUGH, 1, 0),
			CreateMoveData(Move.Low_Sweep, Pokemon.PokemonData.Type.FIGHTING, Category.PHYSICAL, 65, 1f, 20,
				Contest.CLEVER, 2, 3),
			CreateMoveData(Move.Lucky_Chant, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0, 30, Contest.CUTE,
				1, 0),
			CreateMoveData(Move.Mach_Punch, Pokemon.PokemonData.Type.FIGHTING, Category.PHYSICAL, 40, 1f, 30,
				TargetB.ADJACENT, 1, true, true, false, false, new Effect[] {}, new float[] {},
				Contest.COOL, 3, 0),
			CreateMoveData(Move.Magical_Leaf, Pokemon.PokemonData.Type.GRASS, Category.SPECIAL, 60, 0, 20,
				Contest.BEAUTIFUL, 2, 0),
			CreateMoveData(Move.Magic_Coat, Pokemon.PokemonData.Type.PSYCHIC, Category.SPECIAL, 0, 1f, 20,
				Contest.BEAUTIFUL, 2, 0),
			CreateMoveData(Move.Magic_Room, Pokemon.PokemonData.Type.PSYCHIC, Category.STATUS, 0, 0, 10, Contest.CLEVER,
				3, 0),
			CreateMoveData(Move.Magnitude, Pokemon.PokemonData.Type.GROUND, Category.PHYSICAL, 0, 1f, 30, Contest.TOUGH,
				1, 0),
			CreateMoveData(Move.Mat_Block, Pokemon.PokemonData.Type.FIGHTING, Category.STATUS, 0, 0, 10, Contest.COOL,
				1, 3),
			CreateMoveData(Move.Me_First, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0, 20, Contest.CLEVER, 3,
				0),
			CreateMoveData(Move.Mean_Look, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0, 5, Contest.BEAUTIFUL,
				2, 0),
			CreateMoveData(Move.Mega_Drain, Pokemon.PokemonData.Type.GRASS, Category.SPECIAL, 40, 1f, 15,
				Contest.CLEVER, 2, 3),
			CreateMoveData(Move.Megahorn, Pokemon.PokemonData.Type.BUG, Category.PHYSICAL, 120, 0.85f, 10, Contest.COOL,
				3, 0),
			CreateMoveData(Move.Memento, Pokemon.PokemonData.Type.DARK, Category.STATUS, 0, 1f, 10, Contest.TOUGH, 8, 0),
			CreateMoveData(Move.Metal_Burst, Pokemon.PokemonData.Type.STEEL, Category.PHYSICAL, 0, 1f, 10, Contest.COOL,
				2, 0),
			CreateMoveData(Move.Metal_Claw, Pokemon.PokemonData.Type.STEEL, Category.PHYSICAL, 50, 0.95f, 35,
				Contest.COOL, 4, 0, "Cut"),
			CreateMoveData(Move.Metal_Sound, Pokemon.PokemonData.Type.STEEL, Category.STATUS, 0, 0.85f, 40,
				Contest.CLEVER, 1, 3),
			CreateMoveData(Move.Minimize, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0, 10, Contest.CUTE, 1,
				0),
			CreateMoveData(Move.Mirror_Coat, Pokemon.PokemonData.Type.PSYCHIC, Category.STATUS, 0, 0, 15,
				Contest.BEAUTIFUL, 2, 0),
			CreateMoveData(Move.Mirror_Move, Pokemon.PokemonData.Type.FLYING, Category.STATUS, 0, 0, 20, Contest.CLEVER,
				1, 0),
			CreateMoveData(Move.Mist, Pokemon.PokemonData.Type.ICE, Category.STATUS, 0, 0, 30, Contest.BEAUTIFUL, 1, 0),
			CreateMoveData(Move.Moonlight, Pokemon.PokemonData.Type.FAIRY, Category.STATUS, 0, 0, 5, TargetB.SELF, 0,
				false, false, false, true, new Effect[] {Effect.Heal}, new float[] {0.5f},
				Contest.BEAUTIFUL, 2, 0),
			CreateMoveData(Move.Morning_Sun, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0, 5, TargetB.SELF, 0,
				false, false, false, true, new Effect[] {Effect.Heal}, new float[] {0.5f},
				Contest.BEAUTIFUL, 1, 0),
			CreateMoveData(Move.Mud_Bomb, Pokemon.PokemonData.Type.GROUND, Category.SPECIAL, 65, 0.85f, 10,
				Contest.CUTE, 2, 3),
			CreateMoveData(Move.Muddy_Water, Pokemon.PokemonData.Type.WATER, Category.SPECIAL, 90, 0.85f, 10,
				Contest.TOUGH, 2, 2),
			CreateMoveData(Move.Mud_Shot, Pokemon.PokemonData.Type.GROUND, Category.SPECIAL, 55, 0.95f, 15,
				Contest.TOUGH, 4, 0),
			CreateMoveData(Move.Mud_Slap, Pokemon.PokemonData.Type.GROUND, Category.SPECIAL, 20, 1f, 10, Contest.CUTE,
				3, 0),
			CreateMoveData(Move.Mud_Sport, Pokemon.PokemonData.Type.GROUND, Category.STATUS, 0, 0, 15, Contest.CUTE, 2,
				0),
			CreateMoveData(Move.Mystical_Fire, Pokemon.PokemonData.Type.FIRE, Category.SPECIAL, 65, 1f, 10,
				Contest.BEAUTIFUL, 3, 0),
			CreateMoveData(Move.Nasty_Plot, Pokemon.PokemonData.Type.DARK, Category.STATUS, 0, 0, 20, Contest.CLEVER, 1,
				0),
			CreateMoveData(Move.Natural_Gift, Pokemon.PokemonData.Type.NORMAL, Category.PHYSICAL, 0, 1f, 15,
				Contest.CLEVER, 1, 0),
			CreateMoveData(Move.Needle_Arm, Pokemon.PokemonData.Type.GRASS, Category.PHYSICAL, 60, 1f, 15,
				Contest.CLEVER, 4, 0),
			CreateMoveData(Move.Night_Shade, Pokemon.PokemonData.Type.GHOST, Category.SPECIAL, 0, 1f, 15,
				TargetB.ADJACENT, 0, false, true, false, false, new Effect[] {Effect.SetDamage},
				new float[] {0}, Contest.CLEVER, 3, 0),
			CreateMoveData(Move.Night_Slash, Pokemon.PokemonData.Type.DARK, Category.PHYSICAL, 70, 1f, 15, Contest.COOL,
				3, 0, "Cut"),
			CreateMoveData(Move.Odor_Sleuth, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0, 40, Contest.CLEVER,
				2, 0),
			CreateMoveData(Move.Ominous_Wind, Pokemon.PokemonData.Type.GHOST, Category.SPECIAL, 60, 1f, 5,
				TargetB.ADJACENT, 0, false, true, false, false,
				new Effect[]
				{
					Effect.Chance, Effect.ATKself, Effect.DEFself, Effect.SPAself,
					Effect.SPDself, Effect.SPEself
				}, new float[] {0.1f, 1, 1, 1, 1, 1},
				Contest.BEAUTIFUL, 1, 0,
				"The user creates a gust of repulsive wind. It may also raise all the user's stats at once."),
			CreateMoveData(Move.Pain_Split, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0, 20, Contest.CLEVER,
				1, 0),
			CreateMoveData(Move.Payback, Pokemon.PokemonData.Type.DARK, Category.PHYSICAL, 50, 1f, 10, Contest.TOUGH, 2,
				0),
			CreateMoveData(Move.Peck, Pokemon.PokemonData.Type.FLYING, Category.PHYSICAL, 35, 1f, 35, Contest.COOL, 4,
				0),
			CreateMoveData(Move.Petal_Blizzard, Pokemon.PokemonData.Type.GRASS, Category.PHYSICAL, 90, 1f, 15,
				Contest.BEAUTIFUL, 2, 2),
			CreateMoveData(Move.Petal_Dance, Pokemon.PokemonData.Type.GRASS, Category.SPECIAL, 120, 1f, 10,
				Contest.BEAUTIFUL, 6, 0),
			CreateMoveData(Move.Pin_Missile, Pokemon.PokemonData.Type.BUG, Category.PHYSICAL, 25, 0.95f, 20,
				Contest.COOL, 1, 0),
			CreateMoveData(Move.Play_Rough, Pokemon.PokemonData.Type.FAIRY, Category.PHYSICAL, 90, 0.90f, 10,
				Contest.CUTE, 3, 0),
			CreateMoveData(Move.Pluck, Pokemon.PokemonData.Type.FLYING, Category.PHYSICAL, 60, 1f, 20, Contest.CUTE, 3,
				0),
			CreateMoveData(Move.Pound, Pokemon.PokemonData.Type.NORMAL, Category.PHYSICAL, 40, 1f, 35, Contest.TOUGH, 4,
				0),
			CreateMoveData(Move.Poison_Jab, Pokemon.PokemonData.Type.POISON, Category.PHYSICAL, 80, 1f, 20,
				Contest.TOUGH, 4, 0),
			CreateMoveData(Move.Poison_Powder, Pokemon.PokemonData.Type.POISON, Category.STATUS, 0, 0.75f, 35,
				Contest.CLEVER, 3, 0),
			CreateMoveData(Move.Poison_Sting, Pokemon.PokemonData.Type.POISON, Category.PHYSICAL, 15, 1f, 35,
				Contest.CLEVER, 2, 3),
			CreateMoveData(Move.Power_Swap, Pokemon.PokemonData.Type.PSYCHIC, Category.STATUS, 0, 0, 10, Contest.CLEVER,
				1, 0),
			CreateMoveData(Move.Power_Up_Punch, Pokemon.PokemonData.Type.FIGHTING, Category.PHYSICAL, 4, 1f, 20,
				TargetB.ADJACENT, 0, true, true, false, false, new Effect[] {Effect.ATKself},
				new float[] {1}, Contest.TOUGH, 1, 0),
			CreateMoveData(Move.Protect, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0, 10, Contest.CUTE, 2, 0),
			CreateMoveData(Move.Psybeam, Pokemon.PokemonData.Type.PSYCHIC, Category.SPECIAL, 65, 1f, 20,
				Contest.BEAUTIFUL, 3, 0),
			CreateMoveData(Move.Psychic, Pokemon.PokemonData.Type.PSYCHIC, Category.SPECIAL, 90, 1f, 10, Contest.CLEVER,
				4, 0),
			CreateMoveData(Move.Psycho_Shift, Pokemon.PokemonData.Type.PSYCHIC, Category.STATUS, 0, 1f, 10,
				Contest.CLEVER, 2, 0),
			CreateMoveData(Move.Psych_Up, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0, 10, Contest.CLEVER, 4,
				0),
			CreateMoveData(Move.Psyshock, Pokemon.PokemonData.Type.PSYCHIC, Category.SPECIAL, 80, 1f, 10,
				Contest.BEAUTIFUL, 1, 4),
			CreateMoveData(Move.Punishment, Pokemon.PokemonData.Type.DARK, Category.PHYSICAL, 0, 1f, 5, Contest.COOL, 2,
				1),
			CreateMoveData(Move.Pursuit, Pokemon.PokemonData.Type.DARK, Category.PHYSICAL, 40, 1f, 20, Contest.CLEVER,
				2, 1),
			CreateMoveData(Move.Quick_Attack, Pokemon.PokemonData.Type.NORMAL, Category.PHYSICAL, 40, 1f, 30,
				TargetB.ADJACENT, 1, true, true, false, false, new Effect[] {}, new float[] {},
				Contest.COOL, 3, 0),
			CreateMoveData(Move.Quick_Guard, Pokemon.PokemonData.Type.FIGHTING, Category.STATUS, 0, 0, 15, Contest.COOL,
				2, 0),
			CreateMoveData(Move.Quiver_Dance, Pokemon.PokemonData.Type.BUG, Category.STATUS, 0, 0, 20,
				Contest.BEAUTIFUL, 1, 0),
			CreateMoveData(Move.Rage, Pokemon.PokemonData.Type.NORMAL, Category.PHYSICAL, 20, 1f, 20, Contest.TOUGH, 1,
				3),
			CreateMoveData(Move.Rage_Powder, Pokemon.PokemonData.Type.BUG, Category.STATUS, 0, 0, 20, Contest.CLEVER, 3,
				0),
			CreateMoveData(Move.Rain_Dance, Pokemon.PokemonData.Type.WATER, Category.STATUS, 0, 0, 5, Contest.BEAUTIFUL,
				1, 0),
			CreateMoveData(Move.Rapid_Spin, Pokemon.PokemonData.Type.NORMAL, Category.PHYSICAL, 20, 1f, 40,
				Contest.COOL, 1, 0),
			CreateMoveData(Move.Razor_Leaf, Pokemon.PokemonData.Type.GRASS, Category.PHYSICAL, 55, 0.95f, 25,
				Contest.COOL, 4, 0, "Cut"),
			CreateMoveData(Move.Razor_Shell, Pokemon.PokemonData.Type.WATER, Category.PHYSICAL, 75, 0.95f, 10,
				Contest.COOL, 3, 0, "Cut"),
			CreateMoveData(Move.Razor_Wind, Pokemon.PokemonData.Type.NORMAL, Category.SPECIAL, 80, 1f, 10, Contest.COOL,
				3, 0, "Cut"),
			CreateMoveData(Move.Recover, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0, 10, TargetB.SELF, 0,
				false, false, false, true, new Effect[] {Effect.Heal}, new float[] {0.5f},
				Contest.CLEVER, 2, 0),
			CreateMoveData(Move.Reflect, Pokemon.PokemonData.Type.PSYCHIC, Category.STATUS, 0, 0, 20, Contest.CLEVER, 2,
				0),
			CreateMoveData(Move.Refresh, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0, 20, Contest.CUTE, 2, 0),
			CreateMoveData(Move.Rest, Pokemon.PokemonData.Type.PSYCHIC, Category.STATUS, 0, 0, 10, Contest.CUTE, 1, 0),
			CreateMoveData(Move.Retaliate, Pokemon.PokemonData.Type.NORMAL, Category.PHYSICAL, 70, 1f, 5, Contest.COOL,
				3, 0),
			CreateMoveData(Move.Return, Pokemon.PokemonData.Type.NORMAL, Category.PHYSICAL, 0, 1f, 20, Contest.CUTE, 4,
				0),
			CreateMoveData(Move.Revenge, Pokemon.PokemonData.Type.FIGHTING, Category.PHYSICAL, 60, 1f, 10,
				Contest.TOUGH, 2, 0),
			CreateMoveData(Move.Reversal, Pokemon.PokemonData.Type.FIGHTING, Category.PHYSICAL, 0, 1f, 15, Contest.COOL,
				1, 0),
			CreateMoveData(Move.Roar, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 1f, 20, Contest.COOL, 3, 0),
			CreateMoveData(Move.Rock_Slide, Pokemon.PokemonData.Type.ROCK, Category.PHYSICAL, 75, 0.90f, 10,
				Contest.TOUGH, 2, 2),
			CreateMoveData(Move.Rock_Throw, Pokemon.PokemonData.Type.ROCK, Category.PHYSICAL, 50, 0.90f, 15,
				Contest.TOUGH, 4, 0),
			CreateMoveData(Move.Role_Play, Pokemon.PokemonData.Type.PSYCHIC, Category.STATUS, 0, 0, 10, Contest.CUTE, 1,
				0),
			CreateMoveData(Move.Rollout, Pokemon.PokemonData.Type.ROCK, Category.PHYSICAL, 30, 0.90f, 20, Contest.CUTE,
				3, 0),
			CreateMoveData(Move.Roost, Pokemon.PokemonData.Type.FLYING, Category.STATUS, 0, 0, 10, Contest.CLEVER, 4, 0),
			CreateMoveData(Move.Rototiller, Pokemon.PokemonData.Type.GROUND, Category.STATUS, 0, 0, 10, Contest.TOUGH,
				1, 0),
			CreateMoveData(Move.Safe_Guard, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0, 25,
				Contest.BEAUTIFUL, 2, 0),
			CreateMoveData(Move.Sand_Attack, Pokemon.PokemonData.Type.GROUND, Category.STATUS, 0, 1f, 15, Contest.CUTE,
				3, 0),
			CreateMoveData(Move.Sandstorm, Pokemon.PokemonData.Type.ROCK, Category.STATUS, 0, 0, 10, Contest.TOUGH, 2,
				1),
			CreateMoveData(Move.Scary_Face, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 1f, 10, Contest.TOUGH,
				3, 0),
			CreateMoveData(Move.Scratch, Pokemon.PokemonData.Type.NORMAL, Category.PHYSICAL, 40, 1f, 35, Contest.TOUGH,
				4, 0),
			CreateMoveData(Move.Screech, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0.85f, 40, Contest.CLEVER,
				3, 0),
			CreateMoveData(Move.Seed_Bomb, Pokemon.PokemonData.Type.GRASS, Category.PHYSICAL, 80, 1f, 15, Contest.TOUGH,
				4, 0),
			CreateMoveData(Move.Seismic_Toss, Pokemon.PokemonData.Type.FIGHTING, Category.PHYSICAL, 0, 1f, 20,
				TargetB.ADJACENT, 0, true, true, false, false, new Effect[] {Effect.SetDamage},
				new float[] {0}, Contest.TOUGH, 3, 0),
			CreateMoveData(Move.Shadow_Ball, Pokemon.PokemonData.Type.GHOST, Category.SPECIAL, 80, 1f, 15,
				Contest.CLEVER, 4, 0),
			CreateMoveData(Move.Shadow_Claw, Pokemon.PokemonData.Type.GHOST, Category.PHYSICAL, 70, 1f, 15,
				Contest.COOL, 4, 0, "Cut"),
			CreateMoveData(Move.Shadow_Sneak, Pokemon.PokemonData.Type.GHOST, Category.PHYSICAL, 40, 1f, 30,
				Contest.CLEVER, 3, 0),
			CreateMoveData(Move.Shell_Smash, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0, 15, Contest.TOUGH,
				3, 0),
			CreateMoveData(Move.Silver_Wind, Pokemon.PokemonData.Type.BUG, Category.SPECIAL, 60, 1f, 5,
				Contest.BEAUTIFUL, 1, 0),
			CreateMoveData(Move.Sing, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0.55f, 15, TargetB.ADJACENT,
				0, false, true, true, false, new Effect[] {Effect.Sleep}, new float[] {1},
				Contest.CUTE, 2, 0),
			CreateMoveData(Move.Skull_Bash, Pokemon.PokemonData.Type.NORMAL, Category.PHYSICAL, 130, 1f, 10,
				Contest.TOUGH, 3, 0),
			CreateMoveData(Move.Sky_Attack, Pokemon.PokemonData.Type.FLYING, Category.PHYSICAL, 140, 0.90f, 5,
				Contest.COOL, 3, 0),
			CreateMoveData(Move.Sky_Uppercut, Pokemon.PokemonData.Type.FIGHTING, Category.PHYSICAL, 85, 0.90f, 15,
				Contest.COOL, 2, 1),
			CreateMoveData(Move.Slack_Off, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0, 10, Contest.CUTE, 4,
				0),
			CreateMoveData(Move.Slam, Pokemon.PokemonData.Type.NORMAL, Category.PHYSICAL, 80, 0.75f, 20, Contest.TOUGH,
				4, 0),
			CreateMoveData(Move.Slash, Pokemon.PokemonData.Type.NORMAL, Category.PHYSICAL, 70, 1f, 20, Contest.COOL, 4,
				0, "Cut"),
			CreateMoveData(Move.Sleep_Powder, Pokemon.PokemonData.Type.GRASS, Category.STATUS, 0, 0.75f, 15,
				Contest.CLEVER, 1, 3),
			CreateMoveData(Move.Smog, Pokemon.PokemonData.Type.POISON, Category.SPECIAL, 30, 0.70f, 20, Contest.TOUGH,
				2, 0),
			CreateMoveData(Move.Smokescreen, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 1f, 40,
				Contest.CLEVER, 2, 3),
			CreateMoveData(Move.Snore, Pokemon.PokemonData.Type.NORMAL, Category.SPECIAL, 50, 1f, 15, Contest.CUTE, 1,
				0),
			CreateMoveData(Move.Soak, Pokemon.PokemonData.Type.WATER, Category.STATUS, 0, 1f, 20, Contest.CUTE, 2, 0),
			CreateMoveData(Move.Solar_Beam, Pokemon.PokemonData.Type.GRASS, Category.SPECIAL, 120, 1f, 10, Contest.COOL,
				3, 0),
			CreateMoveData(Move.Spider_Web, Pokemon.PokemonData.Type.BUG, Category.STATUS, 0, 0, 10, Contest.CLEVER, 2,
				0),
			CreateMoveData(Move.Spikes, Pokemon.PokemonData.Type.GROUND, Category.STATUS, 0, 0, 20, Contest.CLEVER, 2,
				0),
			CreateMoveData(Move.Spiky_Shield, Pokemon.PokemonData.Type.GRASS, Category.STATUS, 0, 0, 10, Contest.TOUGH,
				1, 0),
			CreateMoveData(Move.Spit_Up, Pokemon.PokemonData.Type.NORMAL, Category.SPECIAL, 0, 1f, 10, Contest.TOUGH, 1,
				0),
			CreateMoveData(Move.Splash, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0, 40, Contest.CUTE, 4, 0),
			CreateMoveData(Move.Sticky_Web, Pokemon.PokemonData.Type.BUG, Category.STATUS, 0, 0, 20, Contest.TOUGH, 2,
				1),
			CreateMoveData(Move.Stockpile, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0, 20, Contest.COOL, 1,
				0),
			CreateMoveData(Move.Stone_Edge, Pokemon.PokemonData.Type.ROCK, Category.PHYSICAL, 100, 0.80f, 5,
				Contest.TOUGH, 3, 0),
			CreateMoveData(Move.String_Shot, Pokemon.PokemonData.Type.BUG, Category.STATUS, 0, 0.95f, 40,
				Contest.CLEVER, 2, 3),
			CreateMoveData(Move.Struggle, Pokemon.PokemonData.Type.NONE, Category.PHYSICAL, 50, 0, 1,
				TargetB.ADJACENTOPPONENT, 0, true, true, false, false,
				new Effect[] {Effect.RecoilMax}, new float[] {0.25f}, Contest.COOL, 4, 0,
				"An attack that is used in desperation only if the user has no PP. It also hurts the user slightly."),
			CreateMoveData(Move.Stun_Spore, Pokemon.PokemonData.Type.GRASS, Category.STATUS, 0, 0.75f, 30,
				Contest.CLEVER, 2, 1),
			CreateMoveData(Move.Submission, Pokemon.PokemonData.Type.FIGHTING, Category.PHYSICAL, 80, 0.80f, 25,
				Contest.COOL, 6, 0),
			CreateMoveData(Move.Substitute, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0, 10, Contest.CUTE, 2,
				0),
			CreateMoveData(Move.Sucker_Punch, Pokemon.PokemonData.Type.DARK, Category.PHYSICAL, 80, 1f, 5,
				TargetB.ADJACENT, 1, true, true, false, false, new Effect[] {Effect.Unique},
				new float[] {0}, Contest.CLEVER, 3, 0),
			CreateMoveData(Move.Sunny_Day, Pokemon.PokemonData.Type.FIRE, Category.STATUS, 0, 0, 5, Contest.BEAUTIFUL,
				1, 0),
			CreateMoveData(Move.Super_Fang, Pokemon.PokemonData.Type.NORMAL, Category.PHYSICAL, 0, 0.90f, 10,
				Contest.TOUGH, 2, 1),
			CreateMoveData(Move.Superpower, Pokemon.PokemonData.Type.FIGHTING, Category.PHYSICAL, 120, 1f, 5,
				Contest.TOUGH, 6, 0),
			CreateMoveData(Move.Supersonic, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0.55f, 20,
				Contest.CLEVER, 3, 0),
			CreateMoveData(Move.Surf, Pokemon.PokemonData.Type.WATER, Category.SPECIAL, 90, 1f, 15, Contest.BEAUTIFUL,
				2, 2, "Surf"),
			CreateMoveData(Move.Swagger, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0.90f, 15, Contest.CUTE,
				3, 0),
			CreateMoveData(Move.Swallow, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0, 10, Contest.TOUGH, 2,
				0),
			CreateMoveData(Move.Sweet_Scent, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 1f, 20, Contest.CUTE,
				2, 0),
			CreateMoveData(Move.Swift, Pokemon.PokemonData.Type.NORMAL, Category.SPECIAL, 60, 0, 20, Contest.COOL, 2, 0),
			CreateMoveData(Move.Switcheroo, Pokemon.PokemonData.Type.DARK, Category.STATUS, 0, 1f, 10, Contest.CLEVER,
				2, 1),
			CreateMoveData(Move.Swords_Dance, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0, 20, TargetB.SELF,
				0, false, false, false, true, new Effect[] {Effect.ATKself}, new float[] {2},
				Contest.BEAUTIFUL, 1, 0),
			CreateMoveData(Move.Synchronoise, Pokemon.PokemonData.Type.PSYCHIC, Category.SPECIAL, 120, 1f, 15,
				Contest.CLEVER, 2, 0),
			CreateMoveData(Move.Synthesis, Pokemon.PokemonData.Type.GRASS, Category.STATUS, 0, 0, 5, Contest.CLEVER, 1,
				0),
			CreateMoveData(Move.Tackle, Pokemon.PokemonData.Type.NORMAL, Category.PHYSICAL, 50, 1f, 35, Contest.TOUGH,
				4, 0),
			CreateMoveData(Move.Tail_Whip, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 1f, 30,
				TargetB.ALLADJACENTOPPONENT, 0, false, true, true, false,
				new Effect[] {Effect.DEF}, new float[] {-1}, Contest.CUTE, 2, 0),
			CreateMoveData(Move.Tailwind, Pokemon.PokemonData.Type.FLYING, Category.STATUS, 0, 0, 15, Contest.COOL, 3,
				0),
			CreateMoveData(Move.Take_Down, Pokemon.PokemonData.Type.NORMAL, Category.PHYSICAL, 90, 0.85f, 20,
				Contest.TOUGH, 6, 0),
			CreateMoveData(Move.Taunt, Pokemon.PokemonData.Type.DARK, Category.STATUS, 0, 1f, 20, Contest.CLEVER, 2, 1),
			CreateMoveData(Move.Thrash, Pokemon.PokemonData.Type.NORMAL, Category.PHYSICAL, 120, 1f, 10, Contest.TOUGH,
				6, 0),
			CreateMoveData(Move.Thunder_Fang, Pokemon.PokemonData.Type.ELECTRIC, Category.PHYSICAL, 65, 0.95f, 15,
				TargetB.ADJACENT, 0, true, true, false, false,
				new Effect[] {Effect.Paralyze, Effect.Flinch}, new float[] {0.1f, 0.1f},
				Contest.COOL, 4, 0),
			CreateMoveData(Move.Thunder_Punch, Pokemon.PokemonData.Type.ELECTRIC, Category.PHYSICAL, 75, 1f, 15,
				TargetB.ADJACENT, 0, true, true, false, false, new Effect[] {Effect.Paralyze},
				new float[] {0.1f}, Contest.COOL, 4, 0),
			CreateMoveData(Move.Tickle, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 1f, 20, Contest.CUTE, 3, 0),
			CreateMoveData(Move.Toxic, Pokemon.PokemonData.Type.POISON, Category.STATUS, 0, 0.90f, 10, Contest.CLEVER,
				3, 0),
			CreateMoveData(Move.Toxic_Spikes, Pokemon.PokemonData.Type.POISON, Category.STATUS, 0, 0, 20,
				Contest.CLEVER, 2, 0),
			CreateMoveData(Move.Trump_Card, Pokemon.PokemonData.Type.NORMAL, Category.SPECIAL, 0, 0, 5, Contest.COOL, 1,
				0),
			CreateMoveData(Move.Twineedle, Pokemon.PokemonData.Type.BUG, Category.PHYSICAL, 25, 1f, 20, Contest.COOL, 2,
				1),
			CreateMoveData(Move.Twister, Pokemon.PokemonData.Type.DRAGON, Category.SPECIAL, 40, 1f, 20, Contest.COOL, 4,
				0),
			CreateMoveData(Move.Uproar, Pokemon.PokemonData.Type.NORMAL, Category.SPECIAL, 90, 1f, 10, Contest.CUTE, 2,
				1),
			CreateMoveData(Move.Venom_Drench, Pokemon.PokemonData.Type.POISON, Category.STATUS, 0, 1f, 20,
				Contest.CLEVER, 3, 0),
			CreateMoveData(Move.Venoshock, Pokemon.PokemonData.Type.POISON, Category.SPECIAL, 65, 1f, 10,
				Contest.BEAUTIFUL, 2, 0),
			CreateMoveData(Move.Vine_Whip, Pokemon.PokemonData.Type.GRASS, Category.PHYSICAL, 45, 1f, 25, Contest.COOL,
				4, 0),
			CreateMoveData(Move.Vital_Throw, Pokemon.PokemonData.Type.FIGHTING, Category.PHYSICAL, 70, 0, 10,
				TargetB.ADJACENT, -1, true, true, false, false, new Effect[] {}, new float[] {},
				Contest.COOL, 2, 0, "Strength"),
			CreateMoveData(Move.Wake_Up_Slap, Pokemon.PokemonData.Type.FIGHTING, Category.PHYSICAL, 70, 1f, 10,
				Contest.TOUGH, 3, 0),
			CreateMoveData(Move.Water_Gun, Pokemon.PokemonData.Type.WATER, Category.SPECIAL, 40, 1f, 25, Contest.CUTE,
				4, 0),
			CreateMoveData(Move.Water_Pulse, Pokemon.PokemonData.Type.WATER, Category.SPECIAL, 60, 1f, 20,
				Contest.BEAUTIFUL, 3, 0),
			CreateMoveData(Move.Water_Shuriken, Pokemon.PokemonData.Type.WATER, Category.PHYSICAL, 15, 1f, 20,
				Contest.COOL, 3, 0),
			CreateMoveData(Move.Water_Sport, Pokemon.PokemonData.Type.WATER, Category.STATUS, 0, 0, 15, Contest.CUTE, 2,
				0),
			CreateMoveData(Move.Weather_Ball, Pokemon.PokemonData.Type.NORMAL, Category.SPECIAL, 50, 1f, 10,
				Contest.BEAUTIFUL, 3, 0),
			CreateMoveData(Move.Whirlpool, Pokemon.PokemonData.Type.WATER, Category.SPECIAL, 35, 0.85f, 15,
				Contest.BEAUTIFUL, 3, 0),
			CreateMoveData(Move.Whirlwind, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0, 20, Contest.CLEVER,
				3, 0),
			CreateMoveData(Move.Wide_Guard, Pokemon.PokemonData.Type.ROCK, Category.STATUS, 0, 0, 10, Contest.TOUGH, 1,
				0),
			CreateMoveData(Move.Will_O_Wisp, Pokemon.PokemonData.Type.FIRE, Category.STATUS, 0, 0.85f, 15,
				TargetB.ADJACENT, 0, false, true, true, false, new Effect[] {Effect.Burn},
				new float[] {1}, Contest.BEAUTIFUL, 3, 0),
			CreateMoveData(Move.Wing_Attack, Pokemon.PokemonData.Type.FLYING, Category.PHYSICAL, 60, 1f, 35,
				Contest.COOL, 4, 0),
			CreateMoveData(Move.Withdraw, Pokemon.PokemonData.Type.WATER, Category.STATUS, 0, 0, 40, Contest.CUTE, 2, 0),
			CreateMoveData(Move.Wood_Hammer, Pokemon.PokemonData.Type.GRASS, Category.PHYSICAL, 120, 1f, 15,
				Contest.TOUGH, 6, 0),
			CreateMoveData(Move.Worry_Seed, Pokemon.PokemonData.Type.GRASS, Category.STATUS, 0, 1f, 10, Contest.CLEVER,
				2, 0),
			CreateMoveData(Move.Wrap, Pokemon.PokemonData.Type.NORMAL, Category.PHYSICAL, 15, 0.90f, 20, Contest.TOUGH,
				3, 0),
			CreateMoveData(Move.Wring_Out, Pokemon.PokemonData.Type.NORMAL, Category.SPECIAL, 0, 1f, 5, Contest.TOUGH,
				2, 1),
			CreateMoveData(Move.X_Scissor, Pokemon.PokemonData.Type.BUG, Category.PHYSICAL, 80, 1f, 15, Contest.COOL, 2,
				1, "Cut"),
			CreateMoveData(Move.Yawn, Pokemon.PokemonData.Type.NORMAL, Category.STATUS, 0, 0, 10, Contest.CUTE, 2, 0),
			CreateMoveData(Move.Zen_Headbutt, Pokemon.PokemonData.Type.PSYCHIC, Category.PHYSICAL, 80, 0.90f, 15,
				Contest.CLEVER, 4, 0)*/
		};
		#endregion
		public static MoveData CreateMoveData(Moves internalName, Types type, Category category, int power, float accuracy, int PP, 
					Target target, int priority, Veekun.Flags[] flag, float addlEffect, Effect[] moveEffects, float[] moveParameters,
					Contest contest, int appeal, int jamming/*, string fieldEffect*/)
		{
			return new MoveData();
			/*/this.name = name;
			this.type = type;
			this.category = category;
			this.power = power;
			this.accuracy = accuracy;
			this.PP = PP;
			this.target = target;
			this.priority = priority;
			this.contact = contact;
			this.protectable = protectable;
			this.magicCoatable = magicCoatable;
			this.snatchable = snatchable;
			this.moveEffects = moveEffects;
			this.moveParameters = moveParameters;
			this.contest = contest;
			this.appeal = appeal;
			this.jamming = jamming;
			//this.description = description;
			//this.fieldEffect = fieldEffect;*/
		}
		public static MoveData CreateMoveData(Moves internalName, Types type, Category category, int power, float accuracy, int PP, TargetB target,
					int priority, bool contact, bool protectable, bool magicCoatable, bool snatchable,
					Effect[] moveEffects, float[] moveParameters,
					Contest contest, int appeal, int jamming/*, string description, string fieldEffect*/)
		{
			return new MoveData();
			/*/this.name = name;
			this.type = type;
			this.category = category;
			this.power = power;
			this.accuracy = accuracy;
			this.PP = PP;
			this.target = target;
			this.priority = priority;
			this.contact = contact;
			this.protectable = protectable;
			this.magicCoatable = magicCoatable;
			this.snatchable = snatchable;
			this.moveEffects = moveEffects;
			this.moveParameters = moveParameters;
			this.contest = contest;
			this.appeal = appeal;
			this.jamming = jamming;
			//this.description = description;
			//this.fieldEffect = fieldEffect;*/
		}
		internal MoveData getMove(Moves ID)
		{
			foreach (MoveData move in Database)
			{
				if (move.ID == ID) return move;
			}
			throw new System.Exception("Move ID doesnt exist in the database. Please check MoveData constructor.");
		}

        #region Deprecated/Obsolete		
#if DEBUG
		private static Dictionary<int, MoveTranslation> _moveTranslations;// = LoadMoveTranslations();
#else
		private static Dictionary<int, MoveTranslation> _moveTranslations;// = LoadMoveTranslations(SaveData.currentSave.playerLanguage | Settings.Language.English);
#endif
		/// <summary>
		/// </summary> 
		private static Dictionary<int, MoveTranslation> _moveEnglishTranslations;
		/// <summary>
		/// </summary>
		public static void LoadMoveTranslations(Settings.Languages language = Settings.Languages.English)//, int form = 0
		{
			var data = new Dictionary<int, MoveTranslation>();

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
			//ToDo: Consider "/Resources/Database/MoveTranslations/Move_"
#if DEBUG
			string file = @"..\..\..\\Pokemon Unity\Assets\Resources\Database\Moves\Move" + fileLanguage + ".xml"; 
#else
			string file = UnityEngine.Application.dataPath + "/Resources/Database/Moves/Move" + fileLanguage + ".xml"; //Use for production
#endif
			System.IO.FileStream fs = new System.IO.FileStream(file, System.IO.FileMode.Open);
			xmlDoc.Load(fs);

			if (xmlDoc.HasChildNodes)
			{
				foreach (System.Xml.XmlNode node in xmlDoc.GetElementsByTagName("Move"))
				{
					var translation = new MoveTranslation();
					translation.Name = node.Attributes["name"].Value; 
					translation.Description = node.InnerText;
					data.Add(int.Parse(node.Attributes["id"].Value), translation); //Is this safe? Possible overwritting of values with bad entries
				}
			}

			//ToDo: Is filestream still open or does it need to be closed and disposed of?
			fs.Dispose(); fs.Close();
			_moveTranslations = data;//return data;
		}

		/// <summary>
		/// </summary>
		/// <param name="id"></param>
		/// <param name="language"></param>
		/// <returns></returns>
		/// <remarks>ToDo: If not in foreign language, check and load in English; else...</remarks>
		public static MoveTranslation GetMoveTranslation(Moves id, Settings.Languages language = Settings.Languages.English)// int form = 0,
		{
			if (_moveTranslations == null) //should return english if player's default language is null
			{
				LoadMoveTranslations(language);//, form
			}

			int arrayId = (int)id;// GetPokemon(id).ArrayId; //unless db is set, it'll keep looping null...
			if (!_moveTranslations.ContainsKey(arrayId) && language == Settings.Languages.English)
			{
				//Debug.LogError("Failed to load pokedex translation for pokemon with id: " + (int)id); //ToDo: Throw exception error
				throw new System.Exception(string.Format("Failed to load move translation for move with id: {0}_{1}", (int)id, id.ToString()));
			}
			//ToDo: Show english text for missing data on foreign languages 
			else if (!_moveTranslations.ContainsKey(arrayId) && language != Settings.Languages.English)
			{
				return _moveEnglishTranslations[arrayId];
			}

			return _moveTranslations[arrayId];// int id
		}
		#endregion
	    #endregion
	}
	public class MoveBattle
	{
		#region Variables
		private MoveData _baseData;
		private Move _baseMove;
        private string _baseBattle;

		//function   = movedata.function
		private int basedamage; //= movedata.basedamage
		private Types type;       //= movedata.type
		private int accuracy;	//= movedata.accuracy
		private int addlEffect; //= movedata.addlEffect
		private Move.Target target;		//= movedata.target
		private int priority;	//= movedata.priority
		private Move.Flags flags;		//= movedata.flags
		private int category;	//= movedata.category
		private Move thismove;	//= move
		/// <summary>
		/// Can be changed with Mimic/Transform
		/// </summary>
		private int pp;			//= move.pp
		private int totalpp;
		/// <summary>
		/// For Aerilate, Pixilate, Refrigerate
		/// </summary>
		private bool powerboost;
		
		//NOTYPE          = 0x01
		//IGNOREPKMNTYPES = 0x02
		//NOWEIGHTING     = 0x04
		//NOCRITICAL      = 0x08
		//NOREFLECT       = 0x10
		//SELFCONFUSE     = 0x20
        public enum SpecialCondition
        {
            NOTYPE,
            IGNOREPKMNTYPES,
            NOWEIGHTING,
            NOCRITICAL,
            NOREFLECT,
            SELFCONFUSE
        }
        #endregion

        #region Property
        int TotalPP { get
            {
                if (totalpp > 0) return totalpp;
                if (thismove != null) return thismove.TotalPP;
                return 0;
            } }

        Moves Id { get { return thismove.MoveId; } }
        #endregion

        //ToDo: Interface to call Move's function
        //public MoveBattle 
    }
    public class MoveTarget
	{
		public bool hasMultipleTargets(Move move)
		{
			return move.Targets == Target.AllOpposing || move.Targets == Target.AllNonUsers;
		}
		public bool targetsOneOpponent(Move move)
		{
			return move.Targets == Target.SingleNonUser || move.Targets == Target.RandomOpposing
				|| move.Targets == Target.SingleOpposing || move.Targets == Target.OppositeOpposing;
		}
	}
    public class Flags
    {
        /// <summary>
        /// Ignores a target's substitute.
        /// </summary>
        public bool Authentic { get; set; }
        //= false;
        /// <summary>
        /// The move makes physical contact with the target
        /// </summary>
        public bool Contact;
        /// <summary>
        /// The target can use <see cref="Moves.Protect"/> or <see cref="Moves.Detect"/> to protect itself from the move
        /// </summary>
        public bool Protectable;
        /// <summary>
        /// The target can use <see cref="Moves.Magic_Coat"/> to redirect the effect of the move. 
        /// Use this flag if the move deals no damage but causes a negative effect on the target.
        /// (Flags <see cref="MagicCoat"/> and <see cref="Snatch"/> are mutually exclusive.)
        /// </summary>
        public bool Reflectable;
        /// <summary>
        /// The target can use <see cref="Moves.Snatch"/> to steal the effect of the move. 
        /// Use this flag for most moves that target the user.
        /// (Flags <see cref="MagicCoat"/> and <see cref="Snatch"/> are mutually exclusive.)
        /// </summary>
        public bool Snatch;
        /// <summary>
        /// The move can be copied by <see cref="Moves.Mirror_Move"/>.
        /// </summary>
        public bool Mirror;
        // <summary>
        // The move has a 10% chance of making the opponent flinch if the user is holding a 
        // <see cref="eItems.Item.KINGS_ROCK"/>/<see cref="eItems.Item.RAZOR_FANG"/>. 
        // Use this flag for all damaging moves that don't already have a flinching effect.
        // </summary>
        //public bool TriggerFlinch;
        /// <summary>
        /// If the user is <see cref="Status.Frozen"/>, the move will thaw it out before it is used.
        /// </summary>
        /// Thaw
        public bool Defrost;
        // <summary>
        // The move has a high critical hit rate.
        // </summary>
        //public bool Crit;
        /// <summary>
        /// The move is a biting move (powered up by the ability Strong Jaw).
        /// </summary>
        public bool Bite;
        /// <summary>
        /// The move is a punching move (powered up by the ability Iron Fist).
        /// </summary>
        public bool Punching;
        /// <summary>
        /// The move is a sound-based move.
        /// </summary>
        public bool SoundBased;
        /// <summary>
        /// The move is a powder-based move (Grass-type Pokémon are immune to them).
        /// </summary>
        public bool PowderBased;
        /// <summary>
        /// The move is a pulse-based move (powered up by the ability Mega Launcher).
        /// </summary>
        public bool PulseBased;
        /// <summary>
        /// The move is a bomb-based move (resisted by the ability Bulletproof).
        /// </summary>
        public bool BombBased;
    }
    /*public class MoveEffects
    {
        delegate void MoveEffectDelegate();//MoveEffect
    }*/
	#endregion
}

#region Move Interfaces
public interface IMoveEffect
{
	int Effect(Pokemon attacker, Pokemon opponent, int hitnum, Move.Target alltargets, bool showanimation);
}
public interface IMoveAdditionalEffect
{
	void AdditionalEffect(Pokemon attacker, Pokemon opponent);
}
public interface IMoveModifyAccuracy
{
	void ModifyAccuracy(int moveAccuracy, Pokemon attacker, Pokemon opponent);
}
#endregion

/// <summary>
/// Namespace to nest all Pokemon Move Enums
/// </summary>
namespace PokemonUnity.Move
{
	public enum Status
	{
		None,
		Sleep,
		Poison,
		Paralysis,
		Burn,
		Frozen
	}
		/// <summary>
		/// Move ids are connected to XML file.
		/// </summary>
		/// <remarks>Can now code with strings or int and
		/// access the same value.</remarks>
		/// ToDo: Needs to be redone. Alphabetical and no Id tags -- Done
		public enum Moves
		{
			/// <summary>
			/// null
			/// </summary>
			NONE = 0,
			Pound = 1,
			Karate_Chop = 2,
			Double_Slap = 3,
			Comet_Punch = 4,
			Mega_Punch = 5,
			Pay_Day = 6,
			Fire_Punch = 7,
			Ice_Punch = 8,
			Thunder_Punch = 9,
			Scratch = 10,
			Vice_Grip = 11,
			Guillotine = 12,
			Razor_Wind = 13,
			Swords_Dance = 14,
			Cut = 15,
			Gust = 16,
			Wing_Attack = 17,
			Whirlwind = 18,
			Fly = 19,
			Bind = 20,
			Slam = 21,
			Vine_Whip = 22,
			Stomp = 23,
			Double_Kick = 24,
			Mega_Kick = 25,
			Jump_Kick = 26,
			Rolling_Kick = 27,
			Sand_Attack = 28,
			Headbutt = 29,
			Horn_Attack = 30,
			Fury_Attack = 31,
			Horn_Drill = 32,
			Tackle = 33,
			Body_Slam = 34,
			Wrap = 35,
			Take_Down = 36,
			Thrash = 37,
			Double_Edge = 38,
			Tail_Whip = 39,
			Poison_Sting = 40,
			Twineedle = 41,
			Pin_Missile = 42,
			Leer = 43,
			Bite = 44,
			Growl = 45,
			Roar = 46,
			Sing = 47,
			Supersonic = 48,
			Sonic_Boom = 49,
			Disable = 50,
			Acid = 51,
			Ember = 52,
			Flamethrower = 53,
			Mist = 54,
			Water_Gun = 55,
			Hydro_Pump = 56,
			Surf = 57,
			Ice_Beam = 58,
			Blizzard = 59,
			Psybeam = 60,
			Bubble_Beam = 61,
			Aurora_Beam = 62,
			Hyper_Beam = 63,
			Peck = 64,
			Drill_Peck = 65,
			Submission = 66,
			Low_Kick = 67,
			Counter = 68,
			Seismic_Toss = 69,
			Strength = 70,
			Absorb = 71,
			Mega_Drain = 72,
			Leech_Seed = 73,
			Growth = 74,
			Razor_Leaf = 75,
			Solar_Beam = 76,
			Poison_Powder = 77,
			Stun_Spore = 78,
			Sleep_Powder = 79,
			Petal_Dance = 80,
			String_Shot = 81,
			/// <summary>
			/// "The foe is stricken by a shock wave. This attack always inflicts 40 HP damage."
			/// </summary>
			Dragon_Rage = 82,
			Fire_Spin = 83,
			Thunder_Shock = 84,
			Thunderbolt = 85,
			Thunder_Wave = 86,
			Thunder = 87,
			Rock_Throw = 88,
			Earthquake = 89,
			Fissure = 90,
			Dig = 91,
			Toxic = 92,
			Confusion = 93,
			Psychic = 94,
			Hypnosis = 95,
			Meditate = 96,
			Agility = 97,
			Quick_Attack = 98,
			Rage = 99,
			Teleport = 100,
			Night_Shade = 101,
			Mimic = 102,
			Screech = 103,
			Double_Team = 104,
			Recover = 105,
			Harden = 106,
			Minimize = 107,
			Smokescreen = 108,
			Confuse_Ray = 109,
			Withdraw = 110,
			Defense_Curl = 111,
			Barrier = 112,
			Light_Screen = 113,
			Haze = 114,
			Reflect = 115,
			Focus_Energy = 116,
			Bide = 117,
			Metronome = 118,
			Mirror_Move = 119,
			Self_Destruct = 120,
			Egg_Bomb = 121,
			Lick = 122,
			Smog = 123,
			Sludge = 124,
			Bone_Club = 125,
			Fire_Blast = 126,
			Waterfall = 127,
			Clamp = 128,
			Swift = 129,
			Skull_Bash = 130,
			Spike_Cannon = 131,
			Constrict = 132,
			Amnesia = 133,
			Kinesis = 134,
			Soft_Boiled = 135,
			High_Jump_Kick = 136,
			Glare = 137,
			Dream_Eater = 138,
			Poison_Gas = 139,
			Barrage = 140,
			Leech_Life = 141,
			Lovely_Kiss = 142,
			Sky_Attack = 143,
			Transform = 144,
			Bubble = 145,
			Dizzy_Punch = 146,
			Spore = 147,
			Flash = 148,
			Psywave = 149,
			Splash = 150,
			Acid_Armor = 151,
			Crabhammer = 152,
			Explosion = 153,
			Fury_Swipes = 154,
			Bonemerang = 155,
			Rest = 156,
			Rock_Slide = 157,
			Hyper_Fang = 158,
			Sharpen = 159,
			Conversion = 160,
			Tri_Attack = 161,
			Super_Fang = 162,
			Slash = 163,
			Substitute = 164,
			/// <summary>
			/// "An attack that is used in desperation only if the user has no PP. It also hurts the user slightly."
			/// </summary>
			Struggle = 165,
			Sketch = 166,
			Triple_Kick = 167,
			Thief = 168,
			Spider_Web = 169,
			Mind_Reader = 170,
			Nightmare = 171,
			Flame_Wheel = 172,
			Snore = 173,
			Curse = 174,
			Flail = 175,
			Conversion_2 = 176,
			Aeroblast = 177,
			Cotton_Spore = 178,
			Reversal = 179,
			Spite = 180,
			Powder_Snow = 181,
			Protect = 182,
			Mach_Punch = 183,
			Scary_Face = 184,
			Feint_Attack = 185,
			Sweet_Kiss = 186,
			Belly_Drum = 187,
			Sludge_Bomb = 188,
			Mud_Slap = 189,
			Octazooka = 190,
			Spikes = 191,
			Zap_Cannon = 192,
			Foresight = 193,
			Destiny_Bond = 194,
			Perish_Song = 195,
			Icy_Wind = 196,
			Detect = 197,
			Bone_Rush = 198,
			Lock_On = 199,
			Outrage = 200,
			Sandstorm = 201,
			Giga_Drain = 202,
			Endure = 203,
			Charm = 204,
			Rollout = 205,
			False_Swipe = 206,
			Swagger = 207,
			Milk_Drink = 208,
			Spark = 209,
			Fury_Cutter = 210,
			Steel_Wing = 211,
			Mean_Look = 212,
			Attract = 213,
			Sleep_Talk = 214,
			Heal_Bell = 215,
			Return = 216,
			Present = 217,
			Frustration = 218,
			Safeguard = 219,
			Pain_Split = 220,
			Sacred_Fire = 221,
			Magnitude = 222,
			Dynamic_Punch = 223,
			Megahorn = 224,
			Dragon_Breath = 225,
			Baton_Pass = 226,
			Encore = 227,
			Pursuit = 228,
			Rapid_Spin = 229,
			Sweet_Scent = 230,
			Iron_Tail = 231,
			Metal_Claw = 232,
			Vital_Throw = 233,
			Morning_Sun = 234,
			Synthesis = 235,
			Moonlight = 236,
			Hidden_Power = 237,
			Cross_Chop = 238,
			Twister = 239,
			Rain_Dance = 240,
			Sunny_Day = 241,
			Crunch = 242,
			Mirror_Coat = 243,
			Psych_Up = 244,
			Extreme_Speed = 245,
			Ancient_Power = 246,
			Shadow_Ball = 247,
			Future_Sight = 248,
			Rock_Smash = 249,
			Whirlpool = 250,
			Beat_Up = 251,
			Fake_Out = 252,
			Uproar = 253,
			Stockpile = 254,
			Spit_Up = 255,
			Swallow = 256,
			Heat_Wave = 257,
			Hail = 258,
			Torment = 259,
			Flatter = 260,
			Will_O_Wisp = 261,
			Memento = 262,
			Facade = 263,
			Focus_Punch = 264,
			Smelling_Salts = 265,
			Follow_Me = 266,
			Nature_Power = 267,
			Charge = 268,
			Taunt = 269,
			Helping_Hand = 270,
			Trick = 271,
			Role_Play = 272,
			Wish = 273,
			Assist = 274,
			Ingrain = 275,
			Superpower = 276,
			Magic_Coat = 277,
			Recycle = 278,
			Revenge = 279,
			Brick_Break = 280,
			Yawn = 281,
			Knock_Off = 282,
			Endeavor = 283,
			Eruption = 284,
			Skill_Swap = 285,
			Imprison = 286,
			Refresh = 287,
			Grudge = 288,
			Snatch = 289,
			Secret_Power = 290,
			Dive = 291,
			Arm_Thrust = 292,
			Camouflage = 293,
			Tail_Glow = 294,
			Luster_Purge = 295,
			Mist_Ball = 296,
			Feather_Dance = 297,
			Teeter_Dance = 298,
			Blaze_Kick = 299,
			Mud_Sport = 300,
			Ice_Ball = 301,
			Needle_Arm = 302,
			Slack_Off = 303,
			Hyper_Voice = 304,
			Poison_Fang = 305,
			Crush_Claw = 306,
			Blast_Burn = 307,
			Hydro_Cannon = 308,
			Meteor_Mash = 309,
			Astonish = 310,
			Weather_Ball = 311,
			Aromatherapy = 312,
			Fake_Tears = 313,
			Air_Cutter = 314,
			Overheat = 315,
			Odor_Sleuth = 316,
			Rock_Tomb = 317,
			Silver_Wind = 318,
			Metal_Sound = 319,
			Grass_Whistle = 320,
			Tickle = 321,
			Cosmic_Power = 322,
			Water_Spout = 323,
			Signal_Beam = 324,
			Shadow_Punch = 325,
			Extrasensory = 326,
			Sky_Uppercut = 327,
			Sand_Tomb = 328,
			Sheer_Cold = 329,
			Muddy_Water = 330,
			Bullet_Seed = 331,
			Aerial_Ace = 332,
			Icicle_Spear = 333,
			Iron_Defense = 334,
			Block = 335,
			Howl = 336,
			Dragon_Claw = 337,
			Frenzy_Plant = 338,
			Bulk_Up = 339,
			Bounce = 340,
			Mud_Shot = 341,
			Poison_Tail = 342,
			Covet = 343,
			Volt_Tackle = 344,
			Magical_Leaf = 345,
			Water_Sport = 346,
			Calm_Mind = 347,
			Leaf_Blade = 348,
			Dragon_Dance = 349,
			Rock_Blast = 350,
			Shock_Wave = 351,
			Water_Pulse = 352,
			Doom_Desire = 353,
			Psycho_Boost = 354,
			Roost = 355,
			Gravity = 356,
			Miracle_Eye = 357,
			Wake_Up_Slap = 358,
			Hammer_Arm = 359,
			Gyro_Ball = 360,
			Healing_Wish = 361,
			Brine = 362,
			Natural_Gift = 363,
			Feint = 364,
			Pluck = 365,
			Tailwind = 366,
			Acupressure = 367,
			Metal_Burst = 368,
			U_turn = 369,
			Close_Combat = 370,
			Payback = 371,
			Assurance = 372,
			Embargo = 373,
			Fling = 374,
			Psycho_Shift = 375,
			Trump_Card = 376,
			Heal_Block = 377,
			Wring_Out = 378,
			Power_Trick = 379,
			Gastro_Acid = 380,
			Lucky_Chant = 381,
			Me_First = 382,
			Copycat = 383,
			Power_Swap = 384,
			Guard_Swap = 385,
			Punishment = 386,
			Last_Resort = 387,
			Worry_Seed = 388,
			Sucker_Punch = 389,
			Toxic_Spikes = 390,
			Heart_Swap = 391,
			Aqua_Ring = 392,
			Magnet_Rise = 393,
			Flare_Blitz = 394,
			Force_Palm = 395,
			Aura_Sphere = 396,
			Rock_Polish = 397,
			Poison_Jab = 398,
			Dark_Pulse = 399,
			Night_Slash = 400,
			Aqua_Tail = 401,
			Seed_Bomb = 402,
			Air_Slash = 403,
			X_Scissor = 404,
			Bug_Buzz = 405,
			Dragon_Pulse = 406,
			Dragon_Rush = 407,
			Power_Gem = 408,
			Drain_Punch = 409,
			Vacuum_Wave = 410,
			Focus_Blast = 411,
			Energy_Ball = 412,
			Brave_Bird = 413,
			Earth_Power = 414,
			Switcheroo = 415,
			Giga_Impact = 416,
			Nasty_Plot = 417,
			Bullet_Punch = 418,
			Avalanche = 419,
			Ice_Shard = 420,
			Shadow_Claw = 421,
			Thunder_Fang = 422,
			Ice_Fang = 423,
			/// <summary>
			/// "The user bites with flame-cloaked \nfangs. It may also make the foe \nflinch or sustain a burn."
			/// </summary>
			Fire_Fang = 424,
			Shadow_Sneak = 425,
			Mud_Bomb = 426,
			Psycho_Cut = 427,
			Zen_Headbutt = 428,
			Mirror_Shot = 429,
			Flash_Cannon = 430,
			Rock_Climb = 431,
			Defog = 432,
			Trick_Room = 433,
			Draco_Meteor = 434,
			Discharge = 435,
			Lava_Plume = 436,
			Leaf_Storm = 437,
			Power_Whip = 438,
			Rock_Wrecker = 439,
			Cross_Poison = 440,
			Gunk_Shot = 441,
			Iron_Head = 442,
			Magnet_Bomb = 443,
			Stone_Edge = 444,
			Captivate = 445,
			Stealth_Rock = 446,
			Grass_Knot = 447,
			Chatter = 448,
			Judgment = 449,
			Bug_Bite = 450,
			Charge_Beam = 451,
			Wood_Hammer = 452,
			Aqua_Jet = 453,
			Attack_Order = 454,
			Defend_Order = 455,
			Heal_Order = 456,
			Head_Smash = 457,
			Double_Hit = 458,
			Roar_of_Time = 459,
			Spacial_Rend = 460,
			Lunar_Dance = 461,
			Crush_Grip = 462,
			Magma_Storm = 463,
			Dark_Void = 464,
			Seed_Flare = 465,
			/// <summary>
			/// "The user creates a gust of repulsive wind. It may also raise all the user's stats at once."
			/// </summary>
			Ominous_Wind = 466,
			Shadow_Force = 467,
			Hone_Claws = 468,
			Wide_Guard = 469,
			Guard_Split = 470,
			Power_Split = 471,
			Wonder_Room = 472,
			Psyshock = 473,
			Venoshock = 474,
			Autotomize = 475,
			Rage_Powder = 476,
			Telekinesis = 477,
			Magic_Room = 478,
			Smack_Down = 479,
			Storm_Throw = 480,
			Flame_Burst = 481,
			Sludge_Wave = 482,
			Quiver_Dance = 483,
			Heavy_Slam = 484,
			Synchronoise = 485,
			Electro_Ball = 486,
			Soak = 487,
			Flame_Charge = 488,
			Coil = 489,
			Low_Sweep = 490,
			Acid_Spray = 491,
			Foul_Play = 492,
			Simple_Beam = 493,
			Entrainment = 494,
			After_You = 495,
			Round = 496,
			Echoed_Voice = 497,
			Chip_Away = 498,
			Clear_Smog = 499,
			Stored_Power = 500,
			Quick_Guard = 501,
			Ally_Switch = 502,
			Scald = 503,
			Shell_Smash = 504,
			/// <summary>
			/// "The user emits a healing pulse which restores the target's HP by up to half of its max HP."
			/// </summary>
			Heal_Pulse = 505,
			Hex = 506,
			Sky_Drop = 507,
			Shift_Gear = 508,
			Circle_Throw = 509,
			Incinerate = 510,
			Quash = 511,
			Acrobatics = 512,
			Reflect_Type = 513,
			Retaliate = 514,
			Final_Gambit = 515,
			Bestow = 516,
			Inferno = 517,
			Water_Pledge = 518,
			Fire_Pledge = 519,
			Grass_Pledge = 520,
			Volt_Switch = 521,
			Struggle_Bug = 522,
			Bulldoze = 523,
			Frost_Breath = 524,
			Dragon_Tail = 525,
			Work_Up = 526,
			Electroweb = 527,
			Wild_Charge = 528,
			Drill_Run = 529,
			Dual_Chop = 530,
			Heart_Stamp = 531,
			Horn_Leech = 532,
			Sacred_Sword = 533,
			Razor_Shell = 534,
			Heat_Crash = 535,
			Leaf_Tornado = 536,
			Steamroller = 537,
			Cotton_Guard = 538,
			Night_Daze = 539,
			Psystrike = 540,
			Tail_Slap = 541,
			Hurricane = 542,
			Head_Charge = 543,
			Gear_Grind = 544,
			Searing_Shot = 545,
			Techno_Blast = 546,
			Relic_Song = 547,
			Secret_Sword = 548,
			Glaciate = 549,
			Bolt_Strike = 550,
			Blue_Flare = 551,
			Fiery_Dance = 552,
			Freeze_Shock = 553,
			Ice_Burn = 554,
			Snarl = 555,
			Icicle_Crash = 556,
			V_create = 557,
			Fusion_Flare = 558,
			Fusion_Bolt = 559,
			Flying_Press = 560,
			Mat_Block = 561,
			Belch = 562,
			Rototiller = 563,
			Sticky_Web = 564,
			Fell_Stinger = 565,
			Phantom_Force = 566,
			Trick_or_Treat = 567,
			Noble_Roar = 568,
			Ion_Deluge = 569,
			Parabolic_Charge = 570,
			/// <summary>
			/// Forest's_Curse
			/// </summary>
			Forests_Curse = 571,
			Petal_Blizzard = 572,
			Freeze_Dry = 573,
			Disarming_Voice = 574,
			Parting_Shot = 575,
			Topsy_Turvy = 576,
			Draining_Kiss = 577,
			Crafty_Shield = 578,
			Flower_Shield = 579,
			Grassy_Terrain = 580,
			Misty_Terrain = 581,
			Electrify = 582,
			Play_Rough = 583,
			Fairy_Wind = 584,
			Moonblast = 585,
			Boomburst = 586,
			Fairy_Lock = 587,
			/// <summary>
			/// King's_Shield
			/// </summary>
			Kings_Shield = 588,
			Play_Nice = 589,
			Confide = 590,
			Diamond_Storm = 591,
			Steam_Eruption = 592,
			Hyperspace_Hole = 593,
			Water_Shuriken = 594,
			Mystical_Fire = 595,
			Spiky_Shield = 596,
			Aromatic_Mist = 597,
			Eerie_Impulse = 598,
			Venom_Drench = 599,
			Powder = 600,
			Geomancy = 601,
			Magnetic_Flux = 602,
			Happy_Hour = 603,
			Electric_Terrain = 604,
			Dazzling_Gleam = 605,
			Celebrate = 606,
			Hold_Hands = 607,
			Baby_Doll_Eyes = 608,
			Nuzzle = 609,
			Hold_Back = 610,
			Infestation = 611,
			Power_Up_Punch = 612,
			Oblivion_Wing = 613,
			Thousand_Arrows = 614,
			Thousand_Waves = 615,
			/// <summary>
			/// Land's_Wrath
			/// </summary>
			Lands_Wrath = 616,
			Light_of_Ruin = 617,
			Origin_Pulse = 618,
			Precipice_Blades = 619,
			Dragon_Ascent = 620,
			Hyperspace_Fury = 621,
		}

}

namespace Veekun
{
    [Obsolete]
	public enum Flags
	{
		/// <summary>
		/// The move makes physical contact with the target
		/// </summary>
		Contact,
		/// <summary>
		/// The target can use <see cref="Move.Protect"/> or <see cref="Move.Detect"/> to protect itself from the move
		/// </summary>
		Protectable,
		/// <summary>
		/// The target can use <see cref="Move.Magic_Coat"/> to redirect the effect of the move. 
		/// Use this flag if the move deals no damage but causes a negative effect on the target.
		/// (Flags <see cref="MagicCoat"/> and <see cref="Snatch"/> are mutually exclusive.)
		/// </summary>
		MagicCoat,
		/// <summary>
		/// The target can use <see cref="Move.Snatch"/> to steal the effect of the move. 
		/// Use this flag for most moves that target the user.
		/// (Flags <see cref="MagicCoat"/> and <see cref="Snatch"/> are mutually exclusive.)
		/// </summary>
		Snatchable,
		/// <summary>
		/// The move can be copied by <see cref="Move.Mirror_Move"/>.
		/// </summary>
		MirrorMove,
		/// <summary>
		/// The move has a 10% chance of making the opponent flinch if the user is holding a 
		/// <see cref="eItems.Item.KINGS_ROCK"/>/<see cref="eItems.Item.RAZOR_FANG"/>. 
		/// Use this flag for all damaging moves that don't already have a flinching effect.
		/// </summary>
		TriggerFlinch,
		/// <summary>
		/// If the user is <see cref="Pokemon.bStatus.Frozen"/>, the move will thaw it out before it is used.
		/// </summary>
		Thaw,
		/// <summary>
		/// The move has a high critical hit rate.
		/// </summary>
		Crit,
		/// <summary>
		/// The move is a biting move (powered up by the ability Strong Jaw).
		/// </summary>
		Biting,
		/// <summary>
		/// The move is a punching move (powered up by the ability Iron Fist).
		/// </summary>
		Punching,
		/// <summary>
		/// The move is a sound-based move.
		/// </summary>
		SoundBased,
		/// <summary>
		/// The move is a powder-based move (Grass-type Pokémon are immune to them).
		/// </summary>
		PowderBased,
		/// <summary>
		/// The move is a pulse-based move (powered up by the ability Mega Launcher).
		/// </summary>
		PulseBased,
		/// <summary>
		/// The move is a bomb-based move (resisted by the ability Bulletproof).
		/// </summary>
		BombBased

	}
	/// <summary>
	/// Version from Veekun's Pokedex, needs to be redone
	/// </summary>
	public enum TargetB
	{
		/// <summary>
		/// No target (i.e. Counter, Metal Burst, Mirror Coat, Curse)
		/// </summary>
		/// Single opposing Pokémon selected at random (i.e. Outrage, Petal Dance, Thrash, Uproar)
		/// Single opposing Pokémon directly opposite of user
		NONE = 0,
		/// <summary>
		/// User
		/// </summary>
		SELF,
		/// <summary>
		/// Single Pokémon other than the user
		/// </summary>
		ADJACENT,
		/// <summary>
		/// </summary>
		ANY,
		/// <summary>
		/// User's partner (i.e. Helping Hand)
		/// </summary>
		ADJACENTALLY,
		/// <summary>
		/// Single opposing Pokémon (i.e. Me First)
		/// </summary>
		ADJACENTOPPONENT,
		/// <summary>
		/// Single Pokémon on user's side (i.e. Acupressure)
		/// </summary>
		ADJACENTALLYSELF,
		/// <summary>
		/// Both sides (e.g. Sunny Day, Trick Room)
		/// </summary>
		ALL,
		/// <summary>
		/// All Pokémon other than the user
		/// </summary>
		ALLADJACENT,
		/// <summary>
		/// Opposing side (i.e. Spikes, Toxic Spikes, Stealth Rocks)
		/// </summary>
		ALLADJACENTOPPONENT,
		/// <summary>
		/// All opposing Pokémon
		/// </summary>
		ALLOPPONENT,
		/// <summary>
		/// User's side (e.g. Light Screen, Mist)
		/// </summary>
		ALLALLY
	}

}