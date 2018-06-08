using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Pokemon;
using PokemonUnity.Move;
using Veekun;
using PokemonEssential;

public class Move //: MoveData
{
	#region Variables
	//private string name;
	//private string description;
	//private string fieldEffect;

	//private int pp;
	//private int ppups;
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
	public int PP { get; private set; }
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
	public int PPups { get; private set; }
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

	//public Move() { }

	/// <summary>
	/// Initializes this object to the specified move ID.
	/// </summary>
	public Move(Moves move) { _base = new MoveData().getMove(move); PP = _base.PP; }

	#region Enumerator
    [Obsolete("Will replace IIcolour's with Pokemon Showdown's version")]
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
	#endregion

	#region Methods
	//public Move Clone() { }
	#endregion

	#region Nested Classes
	public class MoveData
	{
		#region Variables
		//private int pp { get; set; }
		//private Moves id { get; set; }
		//private Types type;
		//private Target target;
        /// <summary>
        /// [Deprecated]
        /// </summary>
        /// ToDo: Instead of an Enum;
        /// Make into a class of bool values defaulted to false
        /// OR find a way to set/default uncalled values in array
        [Obsolete]
		private Veekun.Flags[] flagsEnum;
        private Flags flags = new Flags();
        //everything below is influenced from pokemon-showdown
        private bool secondary; // { int chance; boosts; status/effect }
        public Contest ContestType { get; private set; }
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
		public Category Category { get; private set; }
		/// <summary>
		/// totalpp
		/// </summary>
        public int PP { get; private set; }
		public Moves ID { get; private set; }
		/// <summary>
		/// The Pokémon that the move will strike.
		/// </summary>
		public Target Target { get; private set; }
		public Types Type { get; private set; }
		public string Name { get; private set; }
		public string Description { get; private set; }
        public Flags Flags { get { return this.flags; } }
		/// <summary>
		/// The move's base power value. Status moves have a base power of 0, 
		/// while moves with a variable base power are defined here with a base power of 1. 
		/// For multi-hit moves, this is the base power of a single hit.
		/// </summary>
		public int BaseDamage { get; private set; }
		/// <summary>
		/// The move's accuracy, as a percentage. 
		/// An accuracy of 0 means the move doesn't perform an accuracy check 
		/// (i.e. it cannot be evaded).
		/// </summary>
		public int Accuracy { get; private set; }
		/// <summary>
		/// The probability that the move's additional effect occurs, as a percentage. 
		/// If the move has no additional effect (e.g. all status moves), this value is 0. 
		/// <para></para>
		/// Note that some moves have an additional effect chance of 100 (e.g.Acid Spray), 
		/// which is not the same thing as having an effect that will always occur.
		/// Abilities like Sheer Force and Shield Dust only affect additional effects, 
		/// not regular effects.
		/// </summary>
		public float AddlEffect { get; private set; }
		/// <summary>
		/// The move's priority, between -6 and 6 inclusive. This is usually 0. 
		/// A higher priority move will be used before all moves of lower priority, 
		/// regardless of Speed calculations. 
		/// Moves with equal priority will be used depending on which move user is faster.
		/// <example>For example, Quick Attack has a priority of 1.</example>
		/// </summary>
		public int Priority { get; private set; }
		#endregion

		#region Enumerator
		#endregion

		#region Methods
		#region Database
		private static readonly MoveData[] Database = new MoveData[]
		{
			null,
			CreateMoveData(Moves.ABSORB, Types.GRASS, Category.SPECIAL, 20, 1f, 25, TargetB.ADJACENT,
				0, false, true, false, false, new Effect[] {Effect.HPDrain}, new float[] {1},
				Contest.CLEVER, 4, 0),
			CreateMoveData(Moves.ACROBATICS, Types.FLYING, Category.PHYSICAL, 55, 1f, 15, TargetB.ANY,
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
	/// <summary>
    /// Clones Pokemon's Move stats, and uses those values for pokemon battles.
	/// </summary>
	public class MoveBattle
	{
		#region Variables
		private MoveData _baseData { get; set; }
		private Move _baseMove { get; set; }
        private string _baseBattle { get; set; }

		//function   = movedata.function
		private int basedamage { get { return _baseData.BaseDamage; } }     //= movedata.BaseDamage
        private Types type { get { return _baseData.Type; } }               //= movedata.type
        private int accuracy { get { return _baseData.Accuracy; } }         //= movedata.accuracy
        private float addlEffect { get { return _baseData.AddlEffect; } }   //= movedata.addlEffect
        private Target target { get { return _baseData.Target; } }          //= movedata.target
        private int priority { get { return _baseData.Priority; } }         //= movedata.priority
        private Flags flags { get { return _baseData.Flags; } }             //= movedata.flags
	    private Category category { get { return _baseData.Category; }  }	//= movedata.category
        private Move thismove { get; set; }	                                //= move
		private int totalpp { get; set; }
		/// <summary>
		/// For Aerilate, Pixilate, Refrigerate
		/// </summary>
		private bool powerboost;		
        #endregion

        #region Property
		/// <summary>
		/// Can be changed with Mimic/Transform
		/// </summary>
		public int PP { get; private set; }			                        //= move.pp
        public int TotalPP { get
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
        public bool Authentic { get; private set; }
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
	int Effect(Pokemon attacker, Pokemon opponent, int hitnum, Target alltargets, bool showanimation);
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
		POUND = 1,
		KARATE_CHOP = 2,
		DOUBLE_SLAP = 3,
		COMET_PUNCH = 4,
		MEGA_PUNCH = 5,
		PAY_DAY = 6,
		FIRE_PUNCH = 7,
		ICE_PUNCH = 8,
		THUNDER_PUNCH = 9,
		SCRATCH = 10,
		VICE_GRIP = 11,
		GUILLOTINE = 12,
		RAZOR_WIND = 13,
		SWORDS_DANCE = 14,
		CUT = 15,
		GUST = 16,
		WING_ATTACK = 17,
		WHIRLWIND = 18,
		FLY = 19,
		BIND = 20,
		SLAM = 21,
		VINE_WHIP = 22,
		STOMP = 23,
		DOUBLE_KICK = 24,
		MEGA_KICK = 25,
		JUMP_KICK = 26,
		ROLLING_KICK = 27,
		SAND_ATTACK = 28,
		HEADBUTT = 29,
		HORN_ATTACK = 30,
		FURY_ATTACK = 31,
		HORN_DRILL = 32,
		TACKLE = 33,
		BODY_SLAM = 34,
		WRAP = 35,
		TAKE_DOWN = 36,
		THRASH = 37,
		DOUBLE_EDGE = 38,
		TAIL_WHIP = 39,
		POISON_STING = 40,
		TWINEEDLE = 41,
		PIN_MISSILE = 42,
		LEER = 43,
		BITE = 44,
		GROWL = 45,
		ROAR = 46,
		SING = 47,
		SUPERSONIC = 48,
		SONIC_BOOM = 49,
		DISABLE = 50,
		ACID = 51,
		EMBER = 52,
		FLAMETHROWER = 53,
		MIST = 54,
		WATER_GUN = 55,
		HYDRO_PUMP = 56,
		SURF = 57,
		ICE_BEAM = 58,
		BLIZZARD = 59,
		PSYBEAM = 60,
		BUBBLE_BEAM = 61,
		AURORA_BEAM = 62,
		HYPER_BEAM = 63,
		PECK = 64,
		DRILL_PECK = 65,
		SUBMISSION = 66,
		LOW_KICK = 67,
		COUNTER = 68,
		SEISMIC_TOSS = 69,
		STRENGTH = 70,
		ABSORB = 71,
		MEGA_DRAIN = 72,
		LEECH_SEED = 73,
		GROWTH = 74,
		RAZOR_LEAF = 75,
		SOLAR_BEAM = 76,
		POISON_POWDER = 77,
		STUN_SPORE = 78,
		SLEEP_POWDER = 79,
		PETAL_DANCE = 80,
		STRING_SHOT = 81,
        /// <summary>
        /// "The foe is stricken by a shock wave. This attack always inflicts 40 HP damage."
        /// </summary>
        DRAGON_RAGE = 82,
		FIRE_SPIN = 83,
		THUNDER_SHOCK = 84,
		THUNDERBOLT = 85,
		THUNDER_WAVE = 86,
		THUNDER = 87,
		ROCK_THROW = 88,
		EARTHQUAKE = 89,
		FISSURE = 90,
		DIG = 91,
		TOXIC = 92,
		CONFUSION = 93,
		PSYCHIC = 94,
		HYPNOSIS = 95,
		MEDITATE = 96,
		AGILITY = 97,
		QUICK_ATTACK = 98,
		RAGE = 99,
		TELEPORT = 100,
		NIGHT_SHADE = 101,
		MIMIC = 102,
		SCREECH = 103,
		DOUBLE_TEAM = 104,
		RECOVER = 105,
		HARDEN = 106,
		MINIMIZE = 107,
		SMOKESCREEN = 108,
		CONFUSE_RAY = 109,
		WITHDRAW = 110,
		DEFENSE_CURL = 111,
		BARRIER = 112,
		LIGHT_SCREEN = 113,
		HAZE = 114,
		REFLECT = 115,
		FOCUS_ENERGY = 116,
		BIDE = 117,
		METRONOME = 118,
		MIRROR_MOVE = 119,
		SELF_DESTRUCT = 120,
		EGG_BOMB = 121,
		LICK = 122,
		SMOG = 123,
		SLUDGE = 124,
		BONE_CLUB = 125,
		FIRE_BLAST = 126,
		WATERFALL = 127,
		CLAMP = 128,
		SWIFT = 129,
		SKULL_BASH = 130,
		SPIKE_CANNON = 131,
		CONSTRICT = 132,
		AMNESIA = 133,
		KINESIS = 134,
		SOFT_BOILED = 135,
		HIGH_JUMP_KICK = 136,
		GLARE = 137,
		DREAM_EATER = 138,
		POISON_GAS = 139,
		BARRAGE = 140,
		LEECH_LIFE = 141,
		LOVELY_KISS = 142,
		SKY_ATTACK = 143,
		TRANSFORM = 144,
		BUBBLE = 145,
		DIZZY_PUNCH = 146,
		SPORE = 147,
		FLASH = 148,
		PSYWAVE = 149,
		SPLASH = 150,
		ACID_ARMOR = 151,
		CRABHAMMER = 152,
		EXPLOSION = 153,
		FURY_SWIPES = 154,
		BONEMERANG = 155,
		REST = 156,
		ROCK_SLIDE = 157,
		HYPER_FANG = 158,
		SHARPEN = 159,
		CONVERSION = 160,
		TRI_ATTACK = 161,
		SUPER_FANG = 162,
		SLASH = 163,
		SUBSTITUTE = 164,
        /// <summary>
        /// "An attack that is used in desperation only if the user has no PP. It also hurts the user slightly."
        /// </summary>
        STRUGGLE = 165,
		SKETCH = 166,
		TRIPLE_KICK = 167,
		THIEF = 168,
		SPIDER_WEB = 169,
		MIND_READER = 170,
		NIGHTMARE = 171,
		FLAME_WHEEL = 172,
		SNORE = 173,
		CURSE = 174,
		FLAIL = 175,
		CONVERSION_2 = 176,
		AEROBLAST = 177,
		COTTON_SPORE = 178,
		REVERSAL = 179,
		SPITE = 180,
		POWDER_SNOW = 181,
		PROTECT = 182,
		MACH_PUNCH = 183,
		SCARY_FACE = 184,
		FEINT_ATTACK = 185,
		SWEET_KISS = 186,
		BELLY_DRUM = 187,
		SLUDGE_BOMB = 188,
		MUD_SLAP = 189,
		OCTAZOOKA = 190,
		SPIKES = 191,
		ZAP_CANNON = 192,
		FORESIGHT = 193,
		DESTINY_BOND = 194,
		PERISH_SONG = 195,
		ICY_WIND = 196,
		DETECT = 197,
		BONE_RUSH = 198,
		LOCK_ON = 199,
		OUTRAGE = 200,
		SANDSTORM = 201,
		GIGA_DRAIN = 202,
		ENDURE = 203,
		CHARM = 204,
		ROLLOUT = 205,
		FALSE_SWIPE = 206,
		SWAGGER = 207,
		MILK_DRINK = 208,
		SPARK = 209,
		FURY_CUTTER = 210,
		STEEL_WING = 211,
		MEAN_LOOK = 212,
		ATTRACT = 213,
		SLEEP_TALK = 214,
		HEAL_BELL = 215,
		RETURN = 216,
		PRESENT = 217,
		FRUSTRATION = 218,
		SAFEGUARD = 219,
		PAIN_SPLIT = 220,
		SACRED_FIRE = 221,
		MAGNITUDE = 222,
		DYNAMIC_PUNCH = 223,
		MEGAHORN = 224,
		DRAGON_BREATH = 225,
		BATON_PASS = 226,
		ENCORE = 227,
		PURSUIT = 228,
		RAPID_SPIN = 229,
		SWEET_SCENT = 230,
		IRON_TAIL = 231,
		METAL_CLAW = 232,
		VITAL_THROW = 233,
		MORNING_SUN = 234,
		SYNTHESIS = 235,
		MOONLIGHT = 236,
		HIDDEN_POWER = 237,
		CROSS_CHOP = 238,
		TWISTER = 239,
		RAIN_DANCE = 240,
		SUNNY_DAY = 241,
		CRUNCH = 242,
		MIRROR_COAT = 243,
		PSYCH_UP = 244,
		EXTREME_SPEED = 245,
		ANCIENT_POWER = 246,
		SHADOW_BALL = 247,
		FUTURE_SIGHT = 248,
		ROCK_SMASH = 249,
		WHIRLPOOL = 250,
		BEAT_UP = 251,
		FAKE_OUT = 252,
		UPROAR = 253,
		STOCKPILE = 254,
		SPIT_UP = 255,
		SWALLOW = 256,
		HEAT_WAVE = 257,
		HAIL = 258,
		TORMENT = 259,
		FLATTER = 260,
		WILL_O_WISP = 261,
		MEMENTO = 262,
		FACADE = 263,
		FOCUS_PUNCH = 264,
		SMELLING_SALTS = 265,
		FOLLOW_ME = 266,
		NATURE_POWER = 267,
		CHARGE = 268,
		TAUNT = 269,
		HELPING_HAND = 270,
		TRICK = 271,
		ROLE_PLAY = 272,
		WISH = 273,
		ASSIST = 274,
		INGRAIN = 275,
		SUPERPOWER = 276,
		MAGIC_COAT = 277,
		RECYCLE = 278,
		REVENGE = 279,
		BRICK_BREAK = 280,
		YAWN = 281,
		KNOCK_OFF = 282,
		ENDEAVOR = 283,
		ERUPTION = 284,
		SKILL_SWAP = 285,
		IMPRISON = 286,
		REFRESH = 287,
		GRUDGE = 288,
		SNATCH = 289,
		SECRET_POWER = 290,
		DIVE = 291,
		ARM_THRUST = 292,
		CAMOUFLAGE = 293,
		TAIL_GLOW = 294,
		LUSTER_PURGE = 295,
		MIST_BALL = 296,
		FEATHER_DANCE = 297,
		TEETER_DANCE = 298,
		BLAZE_KICK = 299,
		MUD_SPORT = 300,
		ICE_BALL = 301,
		NEEDLE_ARM = 302,
		SLACK_OFF = 303,
		HYPER_VOICE = 304,
		POISON_FANG = 305,
		CRUSH_CLAW = 306,
		BLAST_BURN = 307,
		HYDRO_CANNON = 308,
		METEOR_MASH = 309,
		ASTONISH = 310,
		WEATHER_BALL = 311,
		AROMATHERAPY = 312,
		FAKE_TEARS = 313,
		AIR_CUTTER = 314,
		OVERHEAT = 315,
		ODOR_SLEUTH = 316,
		ROCK_TOMB = 317,
		SILVER_WIND = 318,
		METAL_SOUND = 319,
		GRASS_WHISTLE = 320,
		TICKLE = 321,
		COSMIC_POWER = 322,
		WATER_SPOUT = 323,
		SIGNAL_BEAM = 324,
		SHADOW_PUNCH = 325,
		EXTRASENSORY = 326,
		SKY_UPPERCUT = 327,
		SAND_TOMB = 328,
		SHEER_COLD = 329,
		MUDDY_WATER = 330,
		BULLET_SEED = 331,
		AERIAL_ACE = 332,
		ICICLE_SPEAR = 333,
		IRON_DEFENSE = 334,
		BLOCK = 335,
		HOWL = 336,
		DRAGON_CLAW = 337,
		FRENZY_PLANT = 338,
		BULK_UP = 339,
		BOUNCE = 340,
		MUD_SHOT = 341,
		POISON_TAIL = 342,
		COVET = 343,
		VOLT_TACKLE = 344,
		MAGICAL_LEAF = 345,
		WATER_SPORT = 346,
		CALM_MIND = 347,
		LEAF_BLADE = 348,
		DRAGON_DANCE = 349,
		ROCK_BLAST = 350,
		SHOCK_WAVE = 351,
		WATER_PULSE = 352,
		DOOM_DESIRE = 353,
		PSYCHO_BOOST = 354,
		ROOST = 355,
		GRAVITY = 356,
		MIRACLE_EYE = 357,
		WAKE_UP_SLAP = 358,
		HAMMER_ARM = 359,
		GYRO_BALL = 360,
		HEALING_WISH = 361,
		BRINE = 362,
		NATURAL_GIFT = 363,
		FEINT = 364,
		PLUCK = 365,
		TAILWIND = 366,
		ACUPRESSURE = 367,
		METAL_BURST = 368,
		U_TURN = 369,
		CLOSE_COMBAT = 370,
		PAYBACK = 371,
		ASSURANCE = 372,
		EMBARGO = 373,
		FLING = 374,
		PSYCHO_SHIFT = 375,
		TRUMP_CARD = 376,
		HEAL_BLOCK = 377,
		WRING_OUT = 378,
		POWER_TRICK = 379,
		GASTRO_ACID = 380,
		LUCKY_CHANT = 381,
		ME_FIRST = 382,
		COPYCAT = 383,
		POWER_SWAP = 384,
		GUARD_SWAP = 385,
		PUNISHMENT = 386,
		LAST_RESORT = 387,
		WORRY_SEED = 388,
		SUCKER_PUNCH = 389,
		TOXIC_SPIKES = 390,
		HEART_SWAP = 391,
		AQUA_RING = 392,
		MAGNET_RISE = 393,
		FLARE_BLITZ = 394,
		FORCE_PALM = 395,
		AURA_SPHERE = 396,
		ROCK_POLISH = 397,
		POISON_JAB = 398,
		DARK_PULSE = 399,
		NIGHT_SLASH = 400,
		AQUA_TAIL = 401,
		SEED_BOMB = 402,
		AIR_SLASH = 403,
		X_SCISSOR = 404,
		BUG_BUZZ = 405,
		DRAGON_PULSE = 406,
		DRAGON_RUSH = 407,
		POWER_GEM = 408,
		DRAIN_PUNCH = 409,
		VACUUM_WAVE = 410,
		FOCUS_BLAST = 411,
		ENERGY_BALL = 412,
		BRAVE_BIRD = 413,
		EARTH_POWER = 414,
		SWITCHEROO = 415,
		GIGA_IMPACT = 416,
		NASTY_PLOT = 417,
		BULLET_PUNCH = 418,
		AVALANCHE = 419,
		ICE_SHARD = 420,
		SHADOW_CLAW = 421,
		THUNDER_FANG = 422,
		ICE_FANG = 423,
        /// <summary>
        /// "The user bites with flame-cloaked \nfangs. It may also make the foe \nflinch or sustain a burn."
        /// </summary>
        FIRE_FANG = 424,
		SHADOW_SNEAK = 425,
		MUD_BOMB = 426,
		PSYCHO_CUT = 427,
		ZEN_HEADBUTT = 428,
		MIRROR_SHOT = 429,
		FLASH_CANNON = 430,
		ROCK_CLIMB = 431,
		DEFOG = 432,
		TRICK_ROOM = 433,
		DRACO_METEOR = 434,
		DISCHARGE = 435,
		LAVA_PLUME = 436,
		LEAF_STORM = 437,
		POWER_WHIP = 438,
		ROCK_WRECKER = 439,
		CROSS_POISON = 440,
		GUNK_SHOT = 441,
		IRON_HEAD = 442,
		MAGNET_BOMB = 443,
		STONE_EDGE = 444,
		CAPTIVATE = 445,
		STEALTH_ROCK = 446,
		GRASS_KNOT = 447,
		CHATTER = 448,
		JUDGMENT = 449,
		BUG_BITE = 450,
		CHARGE_BEAM = 451,
		WOOD_HAMMER = 452,
		AQUA_JET = 453,
		ATTACK_ORDER = 454,
		DEFEND_ORDER = 455,
		HEAL_ORDER = 456,
		HEAD_SMASH = 457,
		DOUBLE_HIT = 458,
		ROAR_OF_TIME = 459,
		SPACIAL_REND = 460,
		LUNAR_DANCE = 461,
		CRUSH_GRIP = 462,
		MAGMA_STORM = 463,
		DARK_VOID = 464,
		SEED_FLARE = 465,
        /// <summary>
        /// "The user creates a gust of repulsive wind. It may also raise all the user's stats at once."
        /// </summary>
        OMINOUS_WIND = 466,
		SHADOW_FORCE = 467,
		HONE_CLAWS = 468,
		WIDE_GUARD = 469,
		GUARD_SPLIT = 470,
		POWER_SPLIT = 471,
		WONDER_ROOM = 472,
		PSYSHOCK = 473,
		VENOSHOCK = 474,
		AUTOTOMIZE = 475,
		RAGE_POWDER = 476,
		TELEKINESIS = 477,
		MAGIC_ROOM = 478,
		SMACK_DOWN = 479,
		STORM_THROW = 480,
		FLAME_BURST = 481,
		SLUDGE_WAVE = 482,
		QUIVER_DANCE = 483,
		HEAVY_SLAM = 484,
		SYNCHRONOISE = 485,
		ELECTRO_BALL = 486,
		SOAK = 487,
		FLAME_CHARGE = 488,
		COIL = 489,
		LOW_SWEEP = 490,
		ACID_SPRAY = 491,
		FOUL_PLAY = 492,
		SIMPLE_BEAM = 493,
		ENTRAINMENT = 494,
		AFTER_YOU = 495,
		ROUND = 496,
		ECHOED_VOICE = 497,
		CHIP_AWAY = 498,
		CLEAR_SMOG = 499,
		STORED_POWER = 500,
		QUICK_GUARD = 501,
		ALLY_SWITCH = 502,
		SCALD = 503,
		SHELL_SMASH = 504,
        /// <summary>
        /// "The user emits a healing pulse which restores the target's HP by up to half of its max HP."
        /// </summary>
        HEAL_PULSE = 505,
		HEX = 506,
		SKY_DROP = 507,
		SHIFT_GEAR = 508,
		CIRCLE_THROW = 509,
		INCINERATE = 510,
		QUASH = 511,
		ACROBATICS = 512,
		REFLECT_TYPE = 513,
		RETALIATE = 514,
		FINAL_GAMBIT = 515,
		BESTOW = 516,
		INFERNO = 517,
		WATER_PLEDGE = 518,
		FIRE_PLEDGE = 519,
		GRASS_PLEDGE = 520,
		VOLT_SWITCH = 521,
		STRUGGLE_BUG = 522,
		BULLDOZE = 523,
		FROST_BREATH = 524,
		DRAGON_TAIL = 525,
		WORK_UP = 526,
		ELECTROWEB = 527,
		WILD_CHARGE = 528,
		DRILL_RUN = 529,
		DUAL_CHOP = 530,
		HEART_STAMP = 531,
		HORN_LEECH = 532,
		SACRED_SWORD = 533,
		RAZOR_SHELL = 534,
		HEAT_CRASH = 535,
		LEAF_TORNADO = 536,
		STEAMROLLER = 537,
		COTTON_GUARD = 538,
		NIGHT_DAZE = 539,
		PSYSTRIKE = 540,
		TAIL_SLAP = 541,
		HURRICANE = 542,
		HEAD_CHARGE = 543,
		GEAR_GRIND = 544,
		SEARING_SHOT = 545,
		TECHNO_BLAST = 546,
		RELIC_SONG = 547,
		SECRET_SWORD = 548,
		GLACIATE = 549,
		BOLT_STRIKE = 550,
		BLUE_FLARE = 551,
		FIERY_DANCE = 552,
		FREEZE_SHOCK = 553,
		ICE_BURN = 554,
		SNARL = 555,
		ICICLE_CRASH = 556,
		V_CREATE = 557,
		FUSION_FLARE = 558,
		FUSION_BOLT = 559,
		FLYING_PRESS = 560,
		MAT_BLOCK = 561,
		BELCH = 562,
		ROTOTILLER = 563,
		STICKY_WEB = 564,
		FELL_STINGER = 565,
		PHANTOM_FORCE = 566,
		TRICK_OR_TREAT = 567,
		NOBLE_ROAR = 568,
		ION_DELUGE = 569,
		PARABOLIC_CHARGE = 570,
        /// <summary>
        /// Forest's_Curse
        /// </summary>
        FORESTS_CURSE = 571,
		PETAL_BLIZZARD = 572,
		FREEZE_DRY = 573,
		DISARMING_VOICE = 574,
		PARTING_SHOT = 575,
		TOPSY_TURVY = 576,
		DRAINING_KISS = 577,
		CRAFTY_SHIELD = 578,
		FLOWER_SHIELD = 579,
		GRASSY_TERRAIN = 580,
		MISTY_TERRAIN = 581,
		ELECTRIFY = 582,
		PLAY_ROUGH = 583,
		FAIRY_WIND = 584,
		MOONBLAST = 585,
		BOOMBURST = 586,
		FAIRY_LOCK = 587,
        /// <summary>
        /// King's_Shield
        /// </summary>
        KINGS_SHIELD = 588,
		PLAY_NICE = 589,
		CONFIDE = 590,
		DIAMOND_STORM = 591,
		STEAM_ERUPTION = 592,
		HYPERSPACE_HOLE = 593,
		WATER_SHURIKEN = 594,
		MYSTICAL_FIRE = 595,
		SPIKY_SHIELD = 596,
		AROMATIC_MIST = 597,
		EERIE_IMPULSE = 598,
		VENOM_DRENCH = 599,
		POWDER = 600,
		GEOMANCY = 601,
		MAGNETIC_FLUX = 602,
		HAPPY_HOUR = 603,
		ELECTRIC_TERRAIN = 604,
		DAZZLING_GLEAM = 605,
		CELEBRATE = 606,
		HOLD_HANDS = 607,
		BABY_DOLL_EYES = 608,
		NUZZLE = 609,
		HOLD_BACK = 610,
		INFESTATION = 611,
		POWER_UP_PUNCH = 612,
		OBLIVION_WING = 613,
		THOUSAND_ARROWS = 614,
		THOUSAND_WAVES = 615,
        /// <summary>
        /// Land's_Wrath
        /// </summary>
        LANDS_WRATH = 616,
		LIGHT_OF_RUIN = 617,
		ORIGIN_PULSE = 618,
		PRECIPICE_BLADES = 619,
		DRAGON_ASCENT = 620,
		HYPERSPACE_FURY = 621,
        //Everythng beyond here is new
        BREAKNECK_BLITZ__PHYSICAL = 622,
        BREAKNECK_BLITZ__SPECIAL = 623,
        ALL_OUT_PUMMELING__PHYSICAL = 624,
        ALL_OUT_PUMMELING__SPECIAL = 625,
        SUPERSONIC_SKYSTRIKE__PHYSICAL = 626,
        SUPERSONIC_SKYSTRIKE__SPECIAL = 627,
        ACID_DOWNPOUR__PHYSICAL = 628,
        ACID_DOWNPOUR__SPECIAL = 629,
        TECTONIC_RAGE__PHYSICAL = 630,
        TECTONIC_RAGE__SPECIAL = 631,
        CONTINENTAL_CRUSH__PHYSICAL = 632,
        CONTINENTAL_CRUSH__SPECIAL = 633,
        SAVAGE_SPIN_OUT__PHYSICAL = 634,
        SAVAGE_SPIN_OUT__SPECIAL = 635,
        NEVER_ENDING_NIGHTMARE__PHYSICAL = 636,
        NEVER_ENDING_NIGHTMARE__SPECIAL = 637,
        CORKSCREW_CRASH__PHYSICAL = 638,
        CORKSCREW_CRASH__SPECIAL = 639,
        INFERNO_OVERDRIVE__PHYSICAL = 640,
        INFERNO_OVERDRIVE__SPECIAL = 641,
        HYDRO_VORTEX__PHYSICAL = 642,
        HYDRO_VORTEX__SPECIAL = 643,
        BLOOM_DOOM__PHYSICAL = 644,
        BLOOM_DOOM__SPECIAL = 645,
        GIGAVOLT_HAVOC__PHYSICAL = 646,
        GIGAVOLT_HAVOC__SPECIAL = 647,
        SHATTERED_PSYCHE__PHYSICAL = 648,
        SHATTERED_PSYCHE__SPECIAL = 649,
        SUBZERO_SLAMMER__PHYSICAL = 650,
        SUBZERO_SLAMMER__SPECIAL = 651,
        DEVASTATING_DRAKE__PHYSICAL = 652,
        DEVASTATING_DRAKE__SPECIAL = 653,
        BLACK_HOLE_ECLIPSE__PHYSICAL = 654,
        BLACK_HOLE_ECLIPSE__SPECIAL = 655,
        TWINKLE_TACKLE__PHYSICAL = 656,
        TWINKLE_TACKLE__SPECIAL = 657,
        CATASTROPIKA = 658,
        SHORE_UP = 659,
        FIRST_IMPRESSION = 660,
        BANEFUL_BUNKER = 661,
        SPIRIT_SHACKLE = 662,
        DARKEST_LARIAT = 663,
        SPARKLING_ARIA = 664,
        ICE_HAMMER = 665,
        FLORAL_HEALING = 666,
        HIGH_HORSEPOWER = 667,
        STRENGTH_SAP = 668,
        SOLAR_BLADE = 669,
        LEAFAGE = 670,
        SPOTLIGHT = 671,
        TOXIC_THREAD = 672,
        LASER_FOCUS = 673,
        GEAR_UP = 674,
        THROAT_CHOP = 675,
        POLLEN_PUFF = 676,
        ANCHOR_SHOT = 677,
        PSYCHIC_TERRAIN = 678,
        LUNGE = 679,
        FIRE_LASH = 680,
        POWER_TRIP = 681,
        BURN_UP = 682,
        SPEED_SWAP = 683,
        SMART_STRIKE = 684,
        PURIFY = 685,
        REVELATION_DANCE = 686,
        CORE_ENFORCER = 687,
        TROP_KICK = 688,
        INSTRUCT = 689,
        BEAK_BLAST = 690,
        CLANGING_SCALES = 691,
        DRAGON_HAMMER = 692,
        BRUTAL_SWING = 693,
        AURORA_VEIL = 694,
        SINISTER_ARROW_RAID = 695,
        MALICIOUS_MOONSAULT = 696,
        OCEANIC_OPERETTA = 697,
        GUARDIAN_OF_ALOLA = 698,
        SOUL_STEALING_7_STAR_STRIKE = 699,
        STOKED_SPARKSURFER = 700,
        PULVERIZING_PANCAKE = 701,
        EXTREME_EVOBOOST = 702,
        GENESIS_SUPERNOVA = 703,
        SHELL_TRAP = 704,
        FLEUR_CANNON = 705,
        PSYCHIC_FANGS = 706,
        STOMPING_TANTRUM = 707,
        SHADOW_BONE = 708,
        ACCELEROCK = 709,
        LIQUIDATION = 710,
        PRISMATIC_LASER = 711,
        SPECTRAL_THIEF = 712,
        SUNSTEEL_STRIKE = 713,
        MOONGEIST_BEAM = 714,
        TEARFUL_LOOK = 715,
        ZING_ZAP = 716,
        NATURES_MADNESS = 717,
        MULTI_ATTACK = 718,
        10_000_000_VOLT_THUNDERBOLT = 719,
        MIND_BLOWN = 720,
        PLASMA_FISTS = 721,
        PHOTON_GEYSER = 722,
        LIGHT_THAT_BURNS_THE_SKY = 723,
        SEARING_SUNRAZE_SMASH = 724,
        MENACING_MOONRAZE_MAELSTROM = 725,
        LETS_SNUGGLE_FOREVER = 726,
        SPLINTERED_STORMSHARDS = 727,
        CLANGOROUS_SOULBLAZE = 728,
        SHADOW_RUSH = 10001,
        SHADOW_BLAST = 10002,
        SHADOW_BLITZ = 10003,
        SHADOW_BOLT = 10004,
        SHADOW_BREAK = 10005,
        SHADOW_CHILL = 10006,
        SHADOW_END = 10007,
        SHADOW_FIRE = 10008,
        SHADOW_RAVE = 10009,
        SHADOW_STORM = 10010,
        SHADOW_WAVE = 10011,
        SHADOW_DOWN = 10012,
        SHADOW_HALF = 10013,
        SHADOW_HOLD = 10014,
        SHADOW_MIST = 10015,
        SHADOW_PANIC = 10016,
        SHADOW_SHED = 10017,
        SHADOW_SKY = 10018
    }
}

namespace PokemonShowdown
{

}

namespace PokemonEssential
{
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
	/// <summary>
	/// </summary>
    /// ToDo: Create Pokemon Showdown Move class
    /// [Obsolete("Will replace Pokemon Essential's with Pokemon Showdown's version")]
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
}

namespace Veekun
{
    [Obsolete("Will replace Veekun's with Pokemon Showdown's version")]
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
    [Obsolete("Will replace Veekun's with Pokemon Essential's version")]
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