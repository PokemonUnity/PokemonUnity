using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Pokemon;
using PokemonUnity.Move;
using PokemonEssential;

public class Move //: MoveData
{
	#region Properties
	protected MoveData _base { get; private set; }
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
	public PokemonEssential.Flags Flag { get { return _base.Flags; } }
	public Effect Function { get { return (Effect)_base.Function; } }
	public string FunctionAsString { get { return _base.FunctionAsString; } }
	public string Name { get { return _base.Name; } }
	public string Description { get { return _base.Description; } }
	#endregion

	//public Move() { }

	/// <summary>
	/// Initializes this object to the specified move ID.
	/// </summary>
	public Move(Moves move = Moves.NONE) { /*if (move != Moves.NONE)*/ _base = new MoveData().getMove(move); PP = _base.PP; }

	#region Enumerator
	public enum Effect
	{
		/// <summary>
		/// Superclass that handles moves using a non-existent function code.
		/// Damaging moves just do damage with no additional effect.
		/// Non-damaging moves always fail.
		/// </summary>
		UnimplementedMove = -1,
		/// <summary>
		/// Superclass for a failed move. Always fails.
		/// This class is unused.
		/// </summary>
		FailedMove = -2,
		/// <summary>
		/// Pseudomove for confusion damage.
		/// </summary>
		Confusion = -3,
		/// <summary>
		/// Implements the move Struggle.
		/// For cases where the real move named Struggle is not defined.
		/// </summary>
		Struggle = -4,
		//[Description("0x000")]
		/// <summary>
		/// No additional effect.
		/// </summary>
		x000 = 0x000,
		/// <summary>
		/// Does absolutely nothing. (Splash)
		/// </summary>
		x001 = 0x001,

		/// <summary>
		/// Struggle. Overrides the default Struggle effect above.
		/// </summary>
		x002 = 0x002,

		/// <summary>
		/// Puts the target to sleep.
		/// </summary>
		x003 = 0x003,

		/// <summary>
		/// Makes the target drowsy; it will fall asleep at the  of the next turn. (Yawn)
		/// </summary>
		x004 = 0x004,

		/// <summary>
		/// Poisons the target.
		/// </summary>
		x005 = 0x005,

		/// <summary>
		/// Badly poisons the target. (Poison Fang, Toxic)
		/// (Handled in Battler's pbSuccessCheck): Hits semi-invulnerable targets if user
		/// is Poison-type and move is status move.
		/// </summary>
		x006 = 0x006,

		/// <summary>
		/// Paralyzes the target.
		/// Thunder Wave: Doesn't affect target if move's type has no effect on it.
		/// Bolt Strike: Powers up the next Fusion Flare used this round.
		/// </summary>
		x007 = 0x007,

		/// <summary>
		/// Paralyzes the target. Accuracy perfect in rain, 50% in sunshine. (Thunder)
		/// (Handled in Battler's pbSuccessCheck): Hits some semi-invulnerable targets.
		/// </summary>
		x008 = 0x008,

		/// <summary>
		/// Paralyzes the target. May cause the target to flinch. (Thunder Fang)
		/// </summary>
		x009 = 0x009,

		/// <summary>
		/// Burns the target.
		/// Blue Flare: Powers up the next Fusion Bolt used this round.
		/// </summary>
		x00A = 0x00A,

		/// <summary>
		/// Burns the target. May cause the target to flinch. (Fire Fang)
		/// </summary>
		x00B = 0x00B,

		/// <summary>
		/// Freezes the target.
		/// </summary>
		x00C = 0x00C,

		/// <summary>
		/// Freezes the target. Accuracy perfect in hail. (Blizzard)
		/// </summary>
		x00D = 0x00D,

		/// <summary>
		/// Freezes the target. May cause the target to flinch. (Ice Fang)
		/// </summary>
		x00E = 0x00E,

		/// <summary>
		/// Causes the target to flinch.
		/// </summary>
		x00F = 0x00F,

		/// <summary>
		/// Causes the target to flinch. Does double damage and has perfect accuracy if
		/// the target is Minimized.
		/// </summary>
		x010 = 0x010,

		/// <summary>
		/// Causes the target to flinch. Fails if the user is not asleep. (Snore)
		/// </summary>
		x011 = 0x011,

		/// <summary>
		/// Causes the target to flinch. Fails if this isn't the user's first turn. (Fake Out)
		/// </summary>
		x012 = 0x012,

		/// <summary>
		/// Confuses the target.
		/// </summary>
		x013 = 0x013,

		/// <summary>
		/// Confuses the target. Chance of causing confusion depends on the cry's volume.
		/// Confusion chance is 0% if user doesn't have a recorded cry. (Chatter)
		/// TODO: Play the actual chatter cry as part of the move animation
		///       @battle.scene.pbChatter(attacker,opponent) // Just plays cry
		/// </summary>
		x014 = 0x014,

		/// <summary>
		/// Confuses the target. Accuracy perfect in rain, 50% in sunshine. (Hurricane)
		/// (Handled in Battler's pbSuccessCheck): Hits some semi-invulnerable targets.
		/// </summary>
		x015 = 0x015,

		/// <summary>
		/// Attracts the target. (Attract)
		/// </summary>
		x016 = 0x016,

		/// <summary>
		/// Burns, freezes or paralyzes the target. (Tri Attack)
		/// </summary>
		x017 = 0x017,

		/// <summary>
		/// Cures user of burn, poison and paralysis. (Refresh)
		/// </summary>
		x018 = 0x018,

		/// <summary>
		/// Cures all party Pokémon of permanent status problems. (Aromatherapy, Heal Bell)
		/// </summary>
		x019 = 0x019,

		/// <summary>
		/// Safeguards the user's side from being inflicted with status problems. (Safeguard)
		/// </summary>
		x01A = 0x01A,

		/// <summary>
		/// User passes its status problem to the target. (Psycho Shift)
		/// </summary>
		x01B = 0x01B,

		/// <summary>
		/// Increases the user's Attack by 1 stage.
		/// </summary>
		x01C = 0x01C,

		/// <summary>
		/// Increases the user's Defense by 1 stage.
		/// </summary>
		x01D = 0x01D,

		/// <summary>
		/// Increases the user's Defense by 1 stage. User curls up. (Defense Curl)
		/// </summary>
		x01E = 0x01E,

		/// <summary>
		/// Increases the user's Speed by 1 stage.
		/// </summary>
		x01F = 0x01F,

		/// <summary>
		/// Increases the user's Special Attack by 1 stage.
		/// </summary>
		x020 = 0x020,

		/// <summary>
		/// Increases the user's Special Defense by 1 stage.
		/// Charges up user's next attack if it is Electric-type. (Charge)
		/// </summary>
		x021 = 0x021,

		/// <summary>
		/// Increases the user's evasion by 1 stage.
		/// </summary>
		x022 = 0x022,

		/// <summary>
		/// Increases the user's critical hit rate. (Focus Energy)
		/// </summary>
		x023 = 0x023,

		/// <summary>
		/// Increases the user's Attack and Defense by 1 stage each. (Bulk Up)
		/// </summary>
		x024 = 0x024,

		/// <summary>
		/// Increases the user's Attack, Defense and accuracy by 1 stage each. (Coil)
		/// </summary>
		x025 = 0x025,

		/// <summary>
		/// Increases the user's Attack and Speed by 1 stage each. (Dragon Dance)
		/// </summary>
		x026 = 0x026,

		/// <summary>
		/// Increases the user's Attack and Special Attack by 1 stage each. (Work Up)
		/// </summary>
		x027 = 0x027,

		/// <summary>
		/// Increases the user's Attack and Sp. Attack by 1 stage each.
		/// In sunny weather, increase is 2 stages each instead. (Growth)
		/// </summary>
		x028 = 0x028,

		/// <summary>
		/// Increases the user's Attack and accuracy by 1 stage each. (Hone Claws)
		/// </summary>
		x029 = 0x029,

		/// <summary>
		/// Increases the user's Defense and Special Defense by 1 stage each. (Cosmic Power)
		/// </summary>
		x02A = 0x02A,

		/// <summary>
		/// Increases the user's Sp. Attack, Sp. Defense and Speed by 1 stage each. (Quiver Dance)
		/// </summary>
		x02B = 0x02B,

		/// <summary>
		/// Increases the user's Sp. Attack and Sp. Defense by 1 stage each. (Calm Mind)
		/// </summary>
		x02C = 0x02C,

		/// <summary>
		/// Increases the user's Attack, Defense, Speed, Special Attack and Special Defense
		/// by 1 stage each. (AncientPower, Ominous Wind, Silver Wind)
		/// </summary>
		x02D = 0x02D,

		/// <summary>
		/// Increases the user's Attack by 2 stages.
		/// </summary>
		x02E = 0x02E,

		/// <summary>
		/// Increases the user's Defense by 2 stages.
		/// </summary>
		x02F = 0x02F,

		/// <summary>
		/// Increases the user's Speed by 2 stages.
		/// </summary>
		x030 = 0x030,

		/// <summary>
		/// Increases the user's Speed by 2 stages. Lowers user's weight by 100kg. (Autotomize)
		/// </summary>
		x031 = 0x031,

		/// <summary>
		/// Increases the user's Special Attack by 2 stages.
		/// </summary>
		x032 = 0x032,

		/// <summary>
		/// Increases the user's Special Defense by 2 stages.
		/// </summary>
		x033 = 0x033,

		/// <summary>
		/// Increases the user's evasion by 2 stages. Minimizes the user. (Minimize)
		/// </summary>
		x034 = 0x034,

		/// <summary>
		/// Decreases the user's Defense and Special Defense by 1 stage each. (Shell Smash)
		/// Increases the user's Attack, Speed and Special Attack by 2 stages each.
		/// </summary>
		x035 = 0x035,

		/// <summary>
		/// Increases the user's Speed by 2 stages, and its Attack by 1 stage. (Shift Gear)
		/// </summary>
		x036 = 0x036,

		/// <summary>
		/// Increases one random stat of the user by 2 stages (except HP). (Acupressure)
		/// </summary>
		x037 = 0x037,

		/// <summary>
		/// Increases the user's Defense by 3 stages.
		/// </summary>
		x038 = 0x038,

		/// <summary>
		/// Increases the user's Special Attack by 3 stages.
		/// </summary>
		x039 = 0x039,

		/// <summary>
		/// Reduces the user's HP by half of max, and sets its Attack to maximum. (Belly Drum)
		/// </summary>
		x03A = 0x03A,

		/// <summary>
		/// Decreases the user's Attack and Defense by 1 stage each. (Superpower)
		/// </summary>
		x03B = 0x03B,

		/// <summary>
		/// Decreases the user's Defense and Special Defense by 1 stage each. (Close Combat)
		/// </summary>
		x03C = 0x03C,

		/// <summary>
		/// Decreases the user's Defense, Special Defense and Speed by 1 stage each.
		/// User's ally loses 1/16 of its total HP. (V-create)
		/// </summary>
		x03D = 0x03D,

		/// <summary>
		/// Decreases the user's Speed by 1 stage.
		/// </summary>
		x03E = 0x03E,

		/// <summary>
		/// Decreases the user's Special Attack by 2 stages.
		/// </summary>
		x03F = 0x03F,

		/// <summary>
		/// Increases the target's Special Attack by 1 stage. Confuses the target. (Flatter)
		/// </summary>
		x040 = 0x040,

		/// <summary>
		/// Increases the target's Attack by 2 stages. Confuses the target. (Swagger)
		/// </summary>
		x041 = 0x041,

		/// <summary>
		/// Decreases the target's Attack by 1 stage.
		/// </summary>
		x042 = 0x042,

		/// <summary>
		/// Decreases the target's Defense by 1 stage.
		/// </summary>
		x043 = 0x043,

		/// <summary>
		/// Decreases the target's Speed by 1 stage.
		/// </summary>
		x044 = 0x044,

		/// <summary>
		/// Decreases the target's Special Attack by 1 stage.
		/// </summary>
		x045 = 0x045,

		/// <summary>
		/// Decreases the target's Special Defense by 1 stage.
		/// </summary>
		x046 = 0x046,

		/// <summary>
		/// Decreases the target's accuracy by 1 stage.
		/// </summary>
		x047 = 0x047,

		/// <summary>
		/// Decreases the target's evasion by 1 stage OR 2 stages. (Sweet Scent)
		/// </summary>
		x048 = 0x048,

		/// <summary>
		/// Decreases the target's evasion by 1 stage. Ends all barriers and entry
		/// hazards for the target's side OR on both sides. (Defog)
		/// </summary>
		x049 = 0x049,

		/// <summary>
		/// Decreases the target's Attack and Defense by 1 stage each. (Tickle)
		/// </summary>
		x04A = 0x04A,

		/// <summary>
		/// Decreases the target's Attack by 2 stages.
		/// </summary>
		x04B = 0x04B,

		/// <summary>
		/// Decreases the target's Defense by 2 stages. (Screech)
		/// </summary>
		x04C = 0x04C,

		/// <summary>
		/// Decreases the target's Speed by 2 stages. (Cotton Spore, Scary Face, String Shot)
		/// </summary>
		x04D = 0x04D,

		/// <summary>
		/// Decreases the target's Special Attack by 2 stages. Only works on the opposite
		/// gender. (Captivate)
		/// </summary>
		x04E = 0x04E,

		/// <summary>
		/// Decreases the target's Special Defense by 2 stages.
		/// </summary>
		x04F = 0x04F,

		/// <summary>
		/// Resets all target's stat stages to 0. (Clear Smog)
		/// </summary>
		x050 = 0x050,

		/// <summary>
		/// Resets all stat stages for all battlers to 0. (Haze)
		/// </summary>
		x051 = 0x051,

		/// <summary>
		/// User and target swap their Attack and Special Attack stat stages. (Power Swap)
		/// </summary>
		x052 = 0x052,

		/// <summary>
		/// User and target swap their Defense and Special Defense stat stages. (Guard Swap)
		/// </summary>
		x053 = 0x053,

		/// <summary>
		/// User and target swap all their stat stages. (Heart Swap)
		/// </summary>
		x054 = 0x054,

		/// <summary>
		/// User copies the target's stat stages. (Psych Up)
		/// </summary>
		x055 = 0x055,

		/// <summary>
		/// For 5 rounds, user's and ally's stat stages cannot be lowered by foes. (Mist)
		/// </summary>
		x056 = 0x056,

		/// <summary>
		/// Swaps the user's Attack and Defense stats. (Power Trick)
		/// </summary>
		x057 = 0x057,

		/// <summary>
		/// Averages the user's and target's Attack.
		/// Averages the user's and target's Special Attack. (Power Split)
		/// </summary>
		x058 = 0x058,

		/// <summary>
		/// Averages the user's and target's Defense.
		/// Averages the user's and target's Special Defense. (Guard Split)
		/// </summary>
		x059 = 0x059,

		/// <summary>
		/// Averages the user's and target's current HP. (Pain Split)
		/// </summary>
		x05A = 0x05A,

		/// <summary>
		/// For 4 rounds, doubles the Speed of all battlers on the user's side. (Tailwind)
		/// </summary>
		x05B = 0x05B,

		/// <summary>
		/// This move turns into the last move used by the target, until user switches
		/// out. (Mimic)
		/// </summary>
		x05C = 0x05C,

		/// <summary>
		/// This move permanently turns into the last move used by the target. (Sketch)
		/// </summary>
		x05D = 0x05D,

		/// <summary>
		/// Changes user's type to that of a random user's move, except this one, OR the
		/// user's first move's type. (Conversion)
		/// </summary>
		x05E = 0x05E,

		/// <summary>
		/// Changes user's type to a random one that resists/is immune to the last move
		/// used by the target. (Conversion 2)
		/// </summary>
		x05F = 0x05F,

		/// <summary>
		/// Changes user's type depending on the environment. (Camouflage)
		/// </summary>
		x060 = 0x060,

		/// <summary>
		/// Target becomes Water type. (Soak)
		/// </summary>
		x061 = 0x061,

		/// <summary>
		/// User copes target's types. (Reflect Type)
		/// </summary>
		x062 = 0x062,

		/// <summary>
		/// Target's ability becomes Simple. (Simple Beam)
		/// </summary>
		x063 = 0x063,

		/// <summary>
		/// Target's ability becomes Insomnia. (Worry Seed)
		/// </summary>
		x064 = 0x064,

		/// <summary>
		/// User copies target's ability. (Role Play)
		/// </summary>
		x065 = 0x065,

		/// <summary>
		/// Target copies user's ability. (Entrainment)
		/// </summary>
		x066 = 0x066,

		/// <summary>
		/// User and target swap abilities. (Skill Swap)
		/// </summary>
		x067 = 0x067,

		/// <summary>
		/// Target's ability is negated. (Gastro Acid)
		/// </summary>
		x068 = 0x068,

		/// <summary>
		/// User transforms into the target. (Transform)
		/// </summary>
		x069 = 0x069,

		/// <summary>
		/// Inflicts a fixed 20HP damage. (SonicBoom)
		/// </summary>
		x06A = 0x06A,

		/// <summary>
		/// Inflicts a fixed 40HP damage. (Dragon Rage)
		/// </summary>
		x06B = 0x06B,

		/// <summary>
		/// Halves the target's current HP. (Super Fang)
		/// </summary>
		x06C = 0x06C,

		/// <summary>
		/// Inflicts damage equal to the user's level. (Night Shade, Seismic Toss)
		/// </summary>
		x06D = 0x06D,

		/// <summary>
		/// Inflicts damage to bring the target's HP down to equal the user's HP. (Endeavor)
		/// </summary>
		x06E = 0x06E,

		/// <summary>
		/// Inflicts damage between 0.5 and 1.5 times the user's level. (Psywave)
		/// </summary>
		x06F = 0x06F,

		/// <summary>
		/// OHKO. Accuracy increases by difference between levels of user and target.
		/// </summary>
		x070 = 0x070,

		/// <summary>
		/// Counters a physical move used against the user this round, with 2x the power. (Counter)
		/// </summary>
		x071 = 0x071,

		/// <summary>
		/// Counters a specical move used against the user this round, with 2x the power. (Mirror Coat)
		/// </summary>
		x072 = 0x072,

		/// <summary>
		/// Counters the last damaging move used against the user this round, with 1.5x
		/// the power. (Metal Burst)
		/// </summary>
		x073 = 0x073,

		/// <summary>
		/// The target's ally loses 1/16 of its max HP. (Flame Burst)
		/// </summary>
		x074 = 0x074,

		/// <summary>
		/// Power is doubled if the target is using Dive. (Surf)
		/// (Handled in Battler's pbSuccessCheck): Hits some semi-invulnerable targets.
		/// </summary>
		x075 = 0x075,

		/// <summary>
		/// Power is doubled if the target is using Dig. Power is halved if Grassy Terrain
		/// is in effect. (Earthquake)
		/// (Handled in Battler's pbSuccessCheck): Hits some semi-invulnerable targets.
		/// </summary>
		x076 = 0x076,

		/// <summary>
		/// Power is doubled if the target is using Bounce, Fly or Sky Drop. (Gust)
		/// (Handled in Battler's pbSuccessCheck): Hits some semi-invulnerable targets.
		/// </summary>
		x077 = 0x077,

		/// <summary>
		/// Power is doubled if the target is using Bounce, Fly or Sky Drop. (Twister)
		/// May make the target flinch.
		/// (Handled in Battler's pbSuccessCheck): Hits some semi-invulnerable targets.
		/// </summary>
		x078 = 0x078,

		/// <summary>
		/// Power is doubled if Fusion Flare has already been used this round. (Fusion Bolt)
		/// </summary>
		x079 = 0x079,

		/// <summary>
		/// Power is doubled if Fusion Bolt has already been used this round. (Fusion Flare)
		/// </summary>
		x07A = 0x07A,

		/// <summary>
		/// Power is doubled if the target is poisoned. (Venoshock)
		/// </summary>
		x07B = 0x07B,

		/// <summary>
		/// Power is doubled if the target is paralyzed. Cures the target of paralysis.
		/// (SmellingSalt)
		/// </summary>
		x07C = 0x07C,

		/// <summary>
		/// Power is doubled if the target is asleep. Wakes the target up. (Wake-Up Slap)
		/// </summary>
		x07D = 0x07D,

		/// <summary>
		/// Power is doubled if the user is burned, poisoned or paralyzed. (Facade)
		/// </summary>
		x07E = 0x07E,

		/// <summary>
		/// Power is doubled if the target has a status problem. (Hex)
		/// </summary>
		x07F = 0x07F,

		/// <summary>
		/// Power is doubled if the target's HP is down to 1/2 or less. (Brine)
		/// </summary>
		x080 = 0x080,

		/// <summary>
		/// Power is doubled if the user has lost HP due to the target's move this round.
		/// (Revenge, Avalanche)
		/// </summary>
		x081 = 0x081,

		/// <summary>
		/// Power is doubled if the target has already lost HP this round. (Assurance)
		/// </summary>
		x082 = 0x082,

		/// <summary>
		/// Power is doubled if a user's ally has already used this move this round. (Round)
		/// If an ally is about to use the same move, make it go next, ignoring priority.
		/// </summary>
		x083 = 0x083,

		/// <summary>
		/// Power is doubled if the target has already moved this round. (Payback)
		/// </summary>
		x084 = 0x084,

		/// <summary>
		/// Power is doubled if a user's teammate fainted last round. (Retaliate)
		/// </summary>
		x085 = 0x085,

		/// <summary>
		/// Power is doubled if the user has no held item. (Acrobatics)
		/// </summary>
		x086 = 0x086,

		/// <summary>
		/// Power is doubled in weather. Type changes depending on the weather. (Weather Ball)
		/// </summary>
		x087 = 0x087,

		/// <summary>
		/// Power is doubled if a foe tries to switch out or use U-turn/Volt Switch/
		/// Parting Shot. (Pursuit)
		/// (Handled in Battle's pbAttackPhase): Makes this attack happen before switching.
		/// </summary>
		x088 = 0x088,

		/// <summary>
		/// Power increases with the user's happiness. (Return)
		/// </summary>
		x089 = 0x089,

		/// <summary>
		/// Power decreases with the user's happiness. (Frustration)
		/// </summary>
		x08A = 0x08A,

		/// <summary>
		/// Power increases with the user's HP. (Eruption, Water Spout)
		/// </summary>
		x08B = 0x08B,

		/// <summary>
		/// Power increases with the target's HP. (Crush Grip, Wring Out)
		/// </summary>
		x08C = 0x08C,

		/// <summary>
		/// Power increases the quicker the target is than the user. (Gyro Ball)
		/// </summary>
		x08D = 0x08D,

		/// <summary>
		/// Power increases with the user's positive stat changes (ignores negative ones).
		/// (Stored Power)
		/// </summary>
		x08E = 0x08E,

		/// <summary>
		/// Power increases with the target's positive stat changes (ignores negative ones).
		/// (Punishment)
		/// </summary>
		x08F = 0x08F,

		/// <summary>
		/// Power and type depends on the user's IVs. (Hidden Power)
		/// </summary>
		x090 = 0x090,

		/// <summary>
		/// Power doubles for each consecutive use. (Fury Cutter)
		/// </summary>
		x091 = 0x091,

		/// <summary>
		/// Power is multiplied by the number of consecutive rounds in which this move was
		/// used by any Pokémon on the user's side. (Echoed Voice)
		/// </summary>
		x092 = 0x092,

		/// <summary>
		/// User rages until the start of a round in which they don't use this move. (Rage)
		/// (Handled in Battler's pbProcessMoveAgainstTarget): Ups rager's Attack by 1
		/// stage each time it loses HP due to a move.
		/// </summary>
		x093 = 0x093,

		/// <summary>
		/// Randomly damages or heals the target. (Present)
		/// </summary>
		x094 = 0x094,

		/// <summary>
		/// Power is chosen at random. Power is doubled if the target is using Dig. (Magnitude)
		/// (Handled in Battler's pbSuccessCheck): Hits some semi-invulnerable targets.
		/// </summary>
		x095 = 0x095,

		/// <summary>
		/// Power and type depend on the user's held berry. Destroys the berry. (Natural Gift)
		/// </summary>
		x096 = 0x096,

		/// <summary>
		/// Power increases the less PP this move has. (Trump Card)
		/// </summary>
		x097 = 0x097,

		/// <summary>
		/// Power increases the less HP the user has. (Flail, Reversal)
		/// </summary>
		x098 = 0x098,

		/// <summary>
		/// Power increases the quicker the user is than the target. (Electro Ball)
		/// </summary>
		x099 = 0x099,

		/// <summary>
		/// Power increases the heavier the target is. (Grass Knot, Low Kick)
		/// </summary>
		x09A = 0x09A,

		/// <summary>
		/// Power increases the heavier the user is than the target. (Heat Crash, Heavy Slam)
		/// </summary>
		x09B = 0x09B,

		/// <summary>
		/// Powers up the ally's attack this round by 1.5. (Helping Hand)
		/// </summary>
		x09C = 0x09C,

		/// <summary>
		/// Weakens Electric attacks. (Mud Sport)
		/// </summary>
		x09D = 0x09D,

		/// <summary>
		/// Weakens Fire attacks. (Water Sport)
		/// </summary>
		x09E = 0x09E,

		/// <summary>
		/// Type depends on the user's held item. (Judgment, Techno Blast)
		/// </summary>
		x09F = 0x09F,

		/// <summary>
		/// This attack is always a critical hit. (Frost Breath, Storm Throw)
		/// </summary>
		x0A0 = 0x0A0,

		/// <summary>
		/// For 5 rounds, foes' attacks cannot become critical hits. (Lucky Chant)
		/// </summary>
		x0A1 = 0x0A1,

		/// <summary>
		/// For 5 rounds, lowers power of physical attacks against the user's side. (Reflect)
		/// </summary>
		x0A2 = 0x0A2,

		/// <summary>
		/// For 5 rounds, lowers power of special attacks against the user's side. (Light Screen)
		/// </summary>
		x0A3 = 0x0A3,

		/// <summary>
		/// Effect depends on the environment. (Secret Power)
		/// </summary>
		x0A4 = 0x0A4,

		/// <summary>
		/// Always hits.
		/// </summary>
		x0A5 = 0x0A5,

		/// <summary>
		/// User's attack next round against the target will definitely hit. (Lock-On, Mind Reader)
		/// </summary>
		x0A6 = 0x0A6,

		/// <summary>
		/// Target's evasion stat changes are ignored from now on. (Foresight, Odor Sleuth)
		/// Normal and Fighting moves have normal effectiveness against the Ghost-type target.
		/// </summary>
		x0A7 = 0x0A7,

		/// <summary>
		/// Target's evasion stat changes are ignored from now on. (Miracle Eye)
		/// Psychic moves have normal effectiveness against the Dark-type target.
		/// </summary>
		x0A8 = 0x0A8,

		/// <summary>
		/// This move ignores target's Defense, Special Defense and evasion stat changes.
		/// (Chip Away, Sacred Sword)
		/// </summary>
		x0A9 = 0x0A9,
		/// Handled in superclass public object pbAccuracyCheck and public object pbCalcDamage, do not edit!

		/// <summary>
		/// User is protected against moves with the "B" flag this round. (Detect, Protect)
		/// </summary>
		x0AA = 0x0AA,

		/// <summary>
		/// User's side is protected against moves with priority greater than 0 this round.
		/// (Quick Guard)
		/// </summary>
		x0AB = 0x0AB,

		/// <summary>
		/// User's side is protected against moves that target multiple battlers this round.
		/// (Wide Guard)
		/// </summary>
		x0AC = 0x0AC,

		/// <summary>
		/// Ignores target's protections. If successful, all other moves this round
		/// ignore them too. (Feint)
		/// </summary>
		x0AD = 0x0AD,

		/// <summary>
		/// Uses the last move that the target used. (Mirror Move)
		/// </summary>
		x0AE = 0x0AE,

		/// <summary>
		/// Uses the last move that was used. (Copycat)
		/// </summary>
		x0AF = 0x0AF,

		/// <summary>
		/// Uses the move the target was about to use this round, with 1.5x power. (Me First)
		/// </summary>
		x0B0 = 0x0B0,

		/// <summary>
		/// This round, reflects all moves with the "C" flag targeting the user back at
		/// their origin. (Magic Coat)
		/// </summary>
		x0B1 = 0x0B1,

		/// <summary>
		/// This round, snatches all used moves with the "D" flag. (Snatch)
		/// </summary>
		x0B2 = 0x0B2,

		/// <summary>
		/// Uses a different move depending on the environment. (Nature Power)
		/// </summary>
		x0B3 = 0x0B3,

		/// <summary>
		/// Uses a random move the user knows. Fails if user is not asleep. (Sleep Talk)
		/// </summary>
		x0B4 = 0x0B4,

		/// <summary>
		/// Uses a random move known by any non-user Pokémon in the user's party. (Assist)
		/// </summary>
		x0B5 = 0x0B5,

		/// <summary>
		/// Uses a random move that exists. (Metronome)
		/// </summary>
		x0B6 = 0x0B6,

		/// <summary>
		/// The target can no longer use the same move twice in a row. (Torment)
		/// </summary>
		x0B7 = 0x0B7,

		/// <summary>
		/// Disables all target's moves that the user also knows. (Imprison)
		/// </summary>
		x0B8 = 0x0B8,

		/// <summary>
		/// For 5 rounds, disables the last move the target used. (Disable)
		/// </summary>
		x0B9 = 0x0B9,

		/// <summary>
		/// For 4 rounds, disables the target's non-damaging moves. (Taunt)
		/// </summary>
		x0BA = 0x0BA,

		/// <summary>
		/// For 5 rounds, disables the target's healing moves. (Heal Block)
		/// </summary>
		x0BB = 0x0BB,

		/// <summary>
		/// For 4 rounds, the target must use the same move each round. (Encore)
		/// </summary>
		x0BC = 0x0BC,

		/// <summary>
		/// Hits twice.
		/// </summary>
		x0BD = 0x0BD,

		/// <summary>
		/// Hits twice. May poison the target on each hit. (Twineedle)
		/// </summary>
		x0BE = 0x0BE,

		/// <summary>
		/// Hits 3 times. Power is multiplied by the hit number. (Triple Kick)
		/// An accuracy check is performed for each hit.
		/// </summary>
		x0BF = 0x0BF,

		/// <summary>
		/// Hits 2-5 times.
		/// </summary>
		x0C0 = 0x0C0,

		/// <summary>
		/// Hits X times, where X is 1 (the user) plus the number of non-user unfainted
		/// status-free Pokémon in the user's party (the participants). Fails if X is 0.
		/// Base power of each hit depends on the base Attack stat for the species of that
		/// hit's participant. (Beat Up)
		/// </summary>
		x0C1 = 0x0C1,

		/// <summary>
		/// Two turn attack. Attacks first turn, skips second turn (if successful).
		/// </summary>
		x0C2 = 0x0C2,

		/// <summary>
		/// Two turn attack. Skips first turn, attacks second turn. (Razor Wind)
		/// </summary>
		x0C3 = 0x0C3,

		/// <summary>
		/// Two turn attack. Skips first turn, attacks second turn. (SolarBeam)
		/// Power halved in all weather except sunshine. In sunshine, takes 1 turn instead.
		/// </summary>
		x0C4 = 0x0C4,

		/// <summary>
		/// Two turn attack. Skips first turn, attacks second turn. (Freeze Shock)
		/// May paralyze the target.
		/// </summary>
		x0C5 = 0x0C5,

		/// <summary>
		/// Two turn attack. Skips first turn, attacks second turn. (Ice Burn)
		/// May burn the target.
		/// </summary>
		x0C6 = 0x0C6,

		/// <summary>
		/// Two turn attack. Skips first turn, attacks second turn. (Sky Attack)
		/// May make the target flinch.
		/// </summary>
		x0C7 = 0x0C7,

		/// <summary>
		/// Two turn attack. Ups user's Defense by 1 stage first turn, attacks second turn.
		/// (Skull Bash)
		/// </summary>
		x0C8 = 0x0C8,

		/// <summary>
		/// Two turn attack. Skips first turn, attacks second turn. (Fly)
		/// (Handled in Battler's pbSuccessCheck): Is semi-invulnerable during use.
		/// </summary>
		x0C9 = 0x0C9,

		/// <summary>
		/// Two turn attack. Skips first turn, attacks second turn. (Dig)
		/// (Handled in Battler's pbSuccessCheck): Is semi-invulnerable during use.
		/// </summary>
		x0CA = 0x0CA,

		/// <summary>
		/// Two turn attack. Skips first turn, attacks second turn. (Dive)
		/// (Handled in Battler's pbSuccessCheck): Is semi-invulnerable during use.
		/// </summary>
		x0CB = 0x0CB,

		/// <summary>
		/// Two turn attack. Skips first turn, attacks second turn. (Bounce)
		/// May paralyze the target.
		/// (Handled in Battler's pbSuccessCheck): Is semi-invulnerable during use.
		/// </summary>
		x0CC = 0x0CC,

		/// <summary>
		/// Two turn attack. Skips first turn, attacks second turn. (Shadow Force)
		/// Is invulnerable during use.
		/// Ignores target's Detect, King's Shield, Mat Block, Protect and Spiky Shield
		/// this round. If successful, negates them this round.
		/// </summary>
		x0CD = 0x0CD,

		/// <summary>
		/// Two turn attack. Skips first turn, attacks second turn. (Sky Drop)
		/// (Handled in Battler's pbSuccessCheck):  Is semi-invulnerable during use.
		/// Target is also semi-invulnerable during use, and can't take any action.
		/// Doesn't damage airborne Pokémon (but still makes them unable to move during).
		/// </summary>
		x0CE = 0x0CE,

		/// <summary>
		/// Trapping move. Traps for 5 or 6 rounds. Trapped Pokémon lose 1/16 of max HP
		/// at  of each round.
		/// </summary>
		x0CF = 0x0CF,

		/// <summary>
		/// Trapping move. Traps for 5 or 6 rounds. Trapped Pokémon lose 1/16 of max HP
		/// at  of each round. (Whirlpool)
		/// Power is doubled if target is using Dive.
		/// (Handled in Battler's pbSuccessCheck): Hits some semi-invulnerable targets.
		/// </summary>
		x0D0 = 0x0D0,

		/// <summary>
		/// User must use this move for 2 more rounds. No battlers can sleep. (Uproar)
		/// </summary>
		x0D1 = 0x0D1,

		/// <summary>
		/// User must use this move for 1 or 2 more rounds. At , user becomes confused.
		/// (Outrage, Petal Dange, Thrash)
		/// </summary>
		x0D2 = 0x0D2,

		/// <summary>
		/// User must use this move for 4 more rounds. Power doubles each round.
		/// Power is also doubled if user has curled up. (Ice Ball, Rollout)
		/// </summary>
		x0D3 = 0x0D3,

		/// <summary>
		/// User bides its time this round and next round. The round after, deals 2x the
		/// total damage it took while biding to the last battler that damaged it. (Bide)
		/// </summary>
		x0D4 = 0x0D4,

		/// <summary>
		/// Heals user by 1/2 of its max HP.
		/// </summary>
		x0D5 = 0x0D5,

		/// <summary>
		/// Heals user by 1/2 of its max HP. (Roost)
		/// User roosts, and its Flying type is ignored for attacks used against it.
		/// </summary>
		x0D6 = 0x0D6,

		/// <summary>
		/// Battler in user's position is healed by 1/2 of its max HP, at the  of the
		/// next round. (Wish)
		/// </summary>
		x0D7 = 0x0D7,

		/// <summary>
		/// Heals user by an amount depending on the weather. (Moonlight, Morning Sun,
		/// Synthesis)
		/// </summary>
		x0D8 = 0x0D8,

		/// <summary>
		/// Heals user to full HP. User falls asleep for 2 more rounds. (Rest)
		/// </summary>
		x0D9 = 0x0D9,

		/// <summary>
		/// Rings the user. Ringed Pokémon gain 1/16 of max HP at the  of each round.
		/// (Aqua Ring)
		/// </summary>
		x0DA = 0x0DA,

		/// <summary>
		/// Ingrains the user. Ingrained Pokémon gain 1/16 of max HP at the  of each
		/// round, and cannot flee or switch out. (Ingrain)
		/// </summary>
		x0DB = 0x0DB,

		/// <summary>
		/// Seeds the target. Seeded Pokémon lose 1/8 of max HP at the  of each round,
		/// and the Pokémon in the user's position gains the same amount. (Leech Seed)
		/// </summary>
		x0DC = 0x0DC,

		/// <summary>
		/// User gains half the HP it inflicts as damage.
		/// </summary>
		x0DD = 0x0DD,

		/// <summary>
		/// User gains half the HP it inflicts as damage. (Dream Eater)
		/// (Handled in Battler's pbSuccessCheck): Fails if target is not asleep.
		/// </summary>
		x0DE = 0x0DE,

		/// <summary>
		/// Heals target by 1/2 of its max HP. (Heal Pulse)
		/// </summary>
		x0DF = 0x0DF,

		/// <summary>
		/// User faints. (Explosion, Selfdestruct)
		/// </summary>
		x0E0 = 0x0E0,

		/// <summary>
		/// Inflicts fixed damage equal to user's current HP. (Final Gambit)
		/// User faints (if successful).
		/// </summary>
		x0E1 = 0x0E1,

		/// <summary>
		/// Decreases the target's Attack and Special Attack by 2 stages each. (Memento)
		/// User faints (even if effect does nothing).
		/// </summary>
		x0E2 = 0x0E2,

		/// <summary>
		/// User faints. The Pokémon that replaces the user is fully healed (HP and
		/// status). Fails if user won't be replaced. (Healing Wish)
		/// </summary>
		x0E3 = 0x0E3,

		/// <summary>
		/// User faints. The Pokémon that replaces the user is fully healed (HP, PP and
		/// status). Fails if user won't be replaced. (Lunar Dance)
		/// </summary>
		x0E4 = 0x0E4,

		/// <summary>
		/// All current battlers will perish after 3 more rounds. (Perish Song)
		/// </summary>
		x0E5 = 0x0E5,

		/// <summary>
		/// If user is KO'd before it next moves, the attack that caused it loses all PP.
		/// (Grudge)
		/// </summary>
		x0E6 = 0x0E6,

		/// <summary>
		/// If user is KO'd before it next moves, the battler that caused it also faints.
		/// (Destiny Bond)
		/// </summary>
		x0E7 = 0x0E7,

		/// <summary>
		/// If user would be KO'd this round, it survives with 1HP instead. (Endure)
		/// </summary>
		x0E8 = 0x0E8,

		/// <summary>
		/// If target would be KO'd by this attack, it survives with 1HP instead. (False Swipe)
		/// </summary>
		x0E9 = 0x0E9,
		/// Handled in superclass public object pbReduceHPDamage, do not edit!

		/// <summary>
		/// User flees from battle. Fails in trainer battles. (Teleport)
		/// </summary>
		x0EA = 0x0EA,

		/// <summary>
		/// In wild battles, makes target flee. Fails if target is a higher level than the
		/// user.
		/// In trainer battles, target switches out.
		/// For status moves. (Roar, Whirlwind)
		/// </summary>
		x0EB = 0x0EB,

		/// <summary>
		/// In wild battles, makes target flee. Fails if target is a higher level than the
		/// user.
		/// In trainer battles, target switches out.
		/// For damaging moves. (Circle Throw, Dragon Tail)
		/// </summary>
		x0EC = 0x0EC,

		/// <summary>
		/// User switches out. Various effects affecting the user are passed to the
		/// replacement. (Baton Pass)
		/// </summary>
		x0ED = 0x0ED,

		/// <summary>
		/// After inflicting damage, user switches out. Ignores trapping moves.
		/// (U-turn, Volt Switch)
		/// TODO: Pursuit should interrupt this move.
		/// </summary>
		x0EE = 0x0EE,

		/// <summary>
		/// Target can no longer switch out or flee, as long as the user remains active.
		/// (Block, Mean Look, Spider Web, Thousand Waves)
		/// </summary>
		x0EF = 0x0EF,

		/// <summary>
		/// Target drops its item. It regains the item at the  of the battle. (Knock Off)
		/// If target has a losable item, damage is multiplied by 1.5.
		/// </summary>
		x0F0 = 0x0F0,

		/// <summary>
		/// User steals the target's item, if the user has none itself. (Covet, Thief)
		/// Items stolen from wild Pokémon are kept after the battle.
		/// </summary>
		x0F1 = 0x0F1,

		/// <summary>
		/// User and target swap items. They remain swapped after wild battles.
		/// (Switcheroo, Trick)
		/// </summary>
		x0F2 = 0x0F2,

		/// <summary>
		/// User gives its item to the target. The item remains given after wild battles.
		/// (Bestow)
		/// </summary>
		x0F3 = 0x0F3,

		/// <summary>
		/// User consumes target's berry and gains its effect. (Bug Bite, Pluck)
		/// </summary>
		x0F4 = 0x0F4,

		/// <summary>
		/// Target's berry is destroyed. (Incinerate)
		/// </summary>
		x0F5 = 0x0F5,

		/// <summary>
		/// User recovers the last item it held and consumed. (Recycle)
		/// </summary>
		x0F6 = 0x0F6,

		/// <summary>
		/// User flings its item at the target. Power and effect depend on the item. (Fling)
		/// </summary>
		x0F7 = 0x0F7,

		/// <summary>
		/// For 5 rounds, the target cannnot use its held item, its held item has no
		/// effect, and no items can be used on it. (Embargo)
		/// </summary>
		x0F8 = 0x0F8,

		/// <summary>
		/// For 5 rounds, all held items cannot be used in any way and have no effect.
		/// Held items can still change hands, but can't be thrown. (Magic Room)
		/// </summary>
		x0F9 = 0x0F9,

		/// <summary>
		/// User takes recoil damage equal to 1/4 of the damage this move dealt.
		/// </summary>
		x0FA = 0x0FA,

		/// <summary>
		/// User takes recoil damage equal to 1/3 of the damage this move dealt.
		/// </summary>
		x0FB = 0x0FB,

		/// <summary>
		/// User takes recoil damage equal to 1/2 of the damage this move dealt.
		/// (Head Smash)
		/// </summary>
		x0FC = 0x0FC,

		/// <summary>
		/// User takes recoil damage equal to 1/3 of the damage this move dealt.
		/// May paralyze the target. (Volt Tackle)
		/// </summary>
		x0FD = 0x0FD,

		/// <summary>
		/// User takes recoil damage equal to 1/3 of the damage this move dealt.
		/// May burn the target. (Flare Blitz)
		/// </summary>
		x0FE = 0x0FE,

		/// <summary>
		/// Starts sunny weather. (Sunny Day)
		/// </summary>
		x0FF = 0x0FF,

		/// <summary>
		/// Starts rainy weather. (Rain Dance)
		/// </summary>
		x100 = 0x100,

		/// <summary>
		/// Starts sandstorm weather. (Sandstorm)
		/// </summary>
		x101 = 0x101,

		/// <summary>
		/// Starts hail weather. (Hail)
		/// </summary>
		x102 = 0x102,

		/// <summary>
		/// Entry hazard. Lays spikes on the opposing side (max. 3 layers). (Spikes)
		/// </summary>
		x103 = 0x103,

		/// <summary>
		/// Entry hazard. Lays poison spikes on the opposing side (max. 2 layers).
		/// (Toxic Spikes)
		/// </summary>
		x104 = 0x104,

		/// <summary>
		/// Entry hazard. Lays stealth rocks on the opposing side. (Stealth Rock)
		/// </summary>
		x105 = 0x105,

		/// <summary>
		/// Forces ally's Pledge move to be used next, if it hasn't already. (Grass Pledge)
		/// Combo's with ally's Pledge move if it was just used. Power is doubled, and
		/// causes either a sea of fire or a swamp on the opposing side.
		/// </summary>
		x106 = 0x106,

		/// <summary>
		/// Forces ally's Pledge move to be used next, if it hasn't already. (Fire Pledge)
		/// Combo's with ally's Pledge move if it was just used. Power is doubled, and
		/// causes either a sea of fire on the opposing side or a rainbow on the user's side.
		/// </summary>
		x107 = 0x107,

		/// <summary>
		/// Forces ally's Pledge move to be used next, if it hasn't already. (Water Pledge)
		/// Combo's with ally's Pledge move if it was just used. Power is doubled, and
		/// causes either a swamp on the opposing side or a rainbow on the user's side.
		/// </summary>
		x108 = 0x108,

		/// <summary>
		/// Scatters coins that the player picks up after winning the battle. (Pay Day)
		/// </summary>
		x109 = 0x109,

		/// <summary>
		/// Ends the opposing side's Light Screen and Reflect. (Brick Break)
		/// </summary>
		x10A = 0x10A,

		/// <summary>
		/// If attack misses, user takes crash damage of 1/2 of max HP.
		/// (Hi Jump Kick, Jump Kick)
		/// </summary>
		x10B = 0x10B,

		/// <summary>
		/// User turns 1/4 of max HP into a substitute. (Substitute)
		/// </summary>
		x10C = 0x10C,

		/// <summary>
		/// User is not Ghost: Decreases the user's Speed, increases the user's Attack &
		/// Defense by 1 stage each.
		/// User is Ghost: User loses 1/2 of max HP, and curses the target.
		/// Cursed Pokémon lose 1/4 of their max HP at the  of each round.
		/// (Curse)
		/// </summary>
		x10D = 0x10D,

		/// <summary>
		/// Target's last move used loses 4 PP. (Spite)
		/// </summary>
		x10E = 0x10E,

		/// <summary>
		/// Target will lose 1/4 of max HP at  of each round, while asleep. (Nightmare)
		/// </summary>
		x10F = 0x10F,

		/// <summary>
		/// Removes trapping moves, entry hazards and Leech Seed on user/user's side.
		/// (Rapid Spin)
		/// </summary>
		x110 = 0x110,

		/// <summary>
		/// Attacks 2 rounds in the future. (Doom Desire, Future Sight)
		/// </summary>
		x111 = 0x111,

		/// Attack is launched

		/// <summary>
		/// Increases the user's Defense and Special Defense by 1 stage each. Ups the
		/// user's stockpile by 1 (max. 3). (Stockpile)
		/// </summary>
		x112 = 0x112,

		/// <summary>
		/// Power is 100 multiplied by the user's stockpile (X). Resets the stockpile to
		/// 0. Decreases the user's Defense and Special Defense by X stages each. (Spit Up)
		/// </summary>
		x113 = 0x113,

		/// <summary>
		/// Heals user depending on the user's stockpile (X). Resets the stockpile to 0.
		/// Decreases the user's Defense and Special Defense by X stages each. (Swallow)
		/// </summary>
		x114 = 0x114,

		/// <summary>
		/// Fails if user was hit by a damaging move this round. (Focus Punch)
		/// </summary>
		x115 = 0x115,

		/// <summary>
		/// Fails if the target didn't chose a damaging move to use this round, or has
		/// already moved. (Sucker Punch)
		/// </summary>
		x116 = 0x116,

		/// <summary>
		/// This round, user becomes the target of attacks that have single targets.
		/// (Follow Me, Rage Powder)
		/// </summary>
		x117 = 0x117,

		/// <summary>
		/// For 5 rounds, increases gravity on the field. Pokémon cannot become airborne.
		/// (Gravity)
		/// </summary>
		x118 = 0x118,

		/// <summary>
		/// For 5 rounds, user becomes airborne. (Magnet Rise)
		/// </summary>
		x119 = 0x119,

		/// <summary>
		/// For 3 rounds, target becomes airborne and can always be hit. (Telekinesis)
		/// </summary>
		x11A = 0x11A,

		/// <summary>
		/// Hits airborne semi-invulnerable targets. (Sky Uppercut)
		/// </summary>
		x11B = 0x11B,
		/// Handled in Battler's pbSuccessCheck, do not edit!

		/// <summary>
		/// Grounds the target while it remains active. (Smack Down, Thousand Arrows)
		/// (Handled in Battler's pbSuccessCheck): Hits some semi-invulnerable targets.
		/// </summary>
		x11C = 0x11C,

		/// <summary>
		/// Target moves immediately after the user, ignoring priority/speed. (After You)
		/// </summary>
		x11D = 0x11D,

		/// <summary>
		/// Target moves last this round, ignoring priority/speed. (Quash)
		/// </summary>
		x11E = 0x11E,

		/// <summary>
		/// For 5 rounds, for each priority bracket, slow Pokémon move before fast ones.
		/// (Trick Room)
		/// </summary>
		x11F = 0x11F,

		/// <summary>
		/// User switches places with its ally. (Ally Switch)
		/// </summary>
		/// Swap effects that point at the position rather than the Pokémon
		/// NOT PerishSongUser (no need to swap), Attract, MultiTurnUser
		x120 = 0x120,

		/// <summary>
		/// Target's Attack is used instead of user's Attack for this move's calculations.
		/// (Foul Play)
		/// </summary>
		/// Handled in superclass public object pbCalcDamage, do not edit!
		x121 = 0x121,

		/// <summary>
		/// Target's Defense is used instead of its Special Defense for this move's
		/// calculations. (Psyshock, Psystrike, Secret Sword)
		/// </summary>
		/// Handled in superclass public object pbCalcDamage, do not edit!
		x122 = 0x122,

		/// <summary>
		/// Only damages Pokémon that share a type with the user. (Synchronoise)
		/// </summary>
		x123 = 0x123,

		/// <summary>
		/// For 5 rounds, swaps all battlers' base Defense with base Special Defense.
		/// (Wonder Room)
		/// </summary>
		x124 = 0x124,

		/// <summary>
		/// Fails unless user has already used all other moves it knows. (Last Resort)
		/// </summary>
		x125 = 0x125,

		//===============================================================================
		// NOTE: Shadow moves use function codes 126-132 inclusive.
		//===============================================================================

		/// <summary>
		/// Does absolutely nothing. (Hold Hands)
		/// </summary>
		x133 = 0x133,

		/// <summary>
		/// Does absolutely nothing. Shows a special message. (Celebrate)
		/// </summary>
		x134 = 0x134,

		/// <summary>
		/// Freezes the target. (Freeze-Dry)
		/// (Superclass's pbTypeModifier): Effectiveness against Water-type is 2x.
		/// </summary>
		x135 = 0x135,

		/// <summary>
		/// Increases the user's Defense by 1 stage for each target hit. (Diamond Storm)
		/// <summary>
		/// ToDo: No difference to function code 01D. It may need to be separate in future.
		/// Includes/Inherits <see cref="x01D"/>
		x136 = 0x136,

		/// <summary>
		/// Increases the user's and its ally's Defense and Special Defense by 1 stage
		/// each, if they have Plus or Minus. (Magnetic Flux)
		/// </summary>
		x137 = 0x137,

		/// <summary>
		/// Increases ally's Special Defense by 1 stage. (Aromatic Mist)
		/// </summary>
		x138 = 0x138,

		/// <summary>
		/// Decreases the target's Attack by 1 stage. Always hits. (Play Nice)
		/// </summary>
		x139 = 0x139,

		/// <summary>
		/// Decreases the target's Attack and Special Attack by 1 stage each. (Noble Roar)
		/// </summary>
		x13A = 0x13A,

		/// <summary>
		/// Decreases the target's Defense by 1 stage. Always hits. (Hyperspace Fury)
		/// </summary>
		x13B = 0x13B,

		/// <summary>
		/// Decreases the target's Special Attack by 1 stage. Always hits. (Confide)
		/// </summary>
		x13C = 0x13C,

		/// <summary>
		/// Decreases the target's Special Attack by 2 stages. (Eerie Impulse)
		/// </summary>
		x13D = 0x13D,

		/// <summary>
		/// Increases the Attack and Special Attack of all Grass-type Pokémon on the field
		/// by 1 stage each. Doesn't affect airborne Pokémon. (Rototiller)
		/// </summary>
		x13E = 0x13E,

		/// <summary>
		/// Increases the Defense of all Grass-type Pokémon on the field by 1 stage each.
		/// (Flower Shield)
		/// </summary>
		x13F = 0x13F,

		/// <summary>
		/// Decreases the Attack, Special Attack and Speed of all poisoned opponents by 1
		/// stage each. (Venom Drench)
		/// </summary>
		x140 = 0x140,

		/// <summary>
		/// Reverses all stat changes of the target. (Topsy-Turvy)
		/// </summary>
		x141 = 0x141,

		/// <summary>
		/// Gives target the Ghost type. (Trick-or-Treat)
		/// </summary>
		x142 = 0x142,

		/// <summary>
		/// Gives target the Grass type. (Forest's Curse)
		/// </summary>
		x143 = 0x143,

		/// <summary>
		/// Damage is multiplied by Flying's effectiveness against the target. Does double
		/// damage and has perfect accuracy if the target is Minimized. (Flying Press)
		/// </summary>
		x144 = 0x144,

		/// <summary>
		/// Target's moves become Electric-type for the rest of the round. (Electrify)
		/// </summary>
		x145 = 0x145,

		/// <summary>
		/// All Normal-type moves become Electric-type for the rest of the round.
		/// (Ion Deluge)
		/// </summary>
		x146 = 0x146,

		/// <summary>
		/// Always hits. (Hyperspace Hole)
		/// TODO: Hits through various shields.
		/// </summary>
		x147 = 0x147,

		/// <summary>
		/// Powders the foe. This round, if it uses a Fire move, it loses 1/4 of its max
		/// HP instead. (Powder)
		/// </summary>
		x148 = 0x148,

		/// <summary>
		/// This round, the user's side is unaffected by damaging moves. (Mat Block)
		/// </summary>
		x149 = 0x149,

		/// <summary>
		/// User's side is protected against status moves this round. (Crafty Shield)
		/// </summary>
		x14A = 0x14A,

		/// <summary>
		/// User is protected against damaging moves this round. Decreases the Attack of
		/// the user of a stopped contact move by 2 stages. (King's Shield)
		/// </summary>
		x14B = 0x14B,

		/// <summary>
		/// User is protected against moves that target it this round. Damages the user of
		/// a stopped contact move by 1/8 of its max HP. (Spiky Shield)
		/// </summary>
		x14C = 0x14C,

		/// <summary>
		/// Two turn attack. Skips first turn, attacks second turn. (Phantom Force)
		/// Is invulnerable during use.
		/// Ignores target's Detect, King's Shield, Mat Block, Protect and Spiky Shield
		/// this round. If successful, negates them this round.
		/// Does double damage and has perfect accuracy if the target is Minimized.
		/// </summary>
		x14D = 0x14D,

		/// <summary>
		/// Two turn attack. Skips first turn, increases the user's Special Attack,
		/// Special Defense and Speed by 2 stages each second turn. (Geomancy)
		/// </summary>
		x14E = 0x14E,

		/// <summary>
		/// User gains 3/4 the HP it inflicts as damage. (Draining Kiss, Oblivion Wing)
		/// </summary>
		x14F = 0x14F,

		/// <summary>
		/// If this move KO's the target, increases the user's Attack by 2 stages.
		/// (Fell Stinger)
		/// </summary>
		x150 = 0x150,

		/// <summary>
		/// Decreases the target's Attack and Special Attack by 1 stage each. Then, user
		/// switches out. Ignores trapping moves. (Parting Shot)
		/// TODO: Pursuit should interrupt this move.
		/// </summary>
		x151 = 0x151,

		/// <summary>
		/// No Pokémon can switch out or flee until the  of the next round, as long as
		/// the user remains active. (Fairy Lock)
		/// </summary>
		x152 = 0x152,

		/// <summary>
		/// Entry hazard. Lays stealth rocks on the opposing side. (Sticky Web)
		/// </summary>
		x153 = 0x153,

		/// <summary>
		/// For 5 rounds, creates an electric terrain which boosts Electric-type moves and
		/// prevents Pokémon from falling asleep. Affects non-airborne Pokémon only.
		/// (Electric Terrain)
		/// </summary>
		x154 = 0x154,

		/// <summary>
		/// For 5 rounds, creates a grassy terrain which boosts Grass-type moves and heals
		/// Pokémon at the  of each round. Affects non-airborne Pokémon only.
		/// (Grassy Terrain)
		/// </summary>
		x155 = 0x155,

		/// <summary>
		/// For 5 rounds, creates a misty terrain which weakens Dragon-type moves and
		/// protects Pokémon from status problems. Affects non-airborne Pokémon only.
		/// (Misty Terrain)
		/// </summary>
		x156 = 0x156,

		/// <summary>
		/// Doubles the prize money the player gets after winning the battle. (Happy Hour)
		/// </summary>
		x157 = 0x157,

		/// <summary>
		/// Fails unless user has consumed a berry at some point. (Belch)
		/// </summary>
		x158
	}
	#endregion

	#region Methods
	//public Move Clone() { }
	#endregion

	#region Nested Classes
	/// <summary>
	/// Clones Pokemon's Move stats, and uses those values for pokemon battles.
	/// </summary>
	public class MoveBattle
	{
		#region Variables
		private MoveData _baseData { get; set; }
		private Move _baseMove { get; set; }
        private string _baseBattle { get; set; }

		//private string function { get { return _baseData.Function; } }		//= movedata.function
		private int basedamage { get { return _baseData.BaseDamage; } }     //= movedata.BaseDamage
        private Types type { get { return _baseData.Type; } }               //= movedata.type
        private int accuracy { get { return _baseData.Accuracy; } }         //= movedata.accuracy
        //private float addlEffect { get { return _baseData.AddlEffect; } }   //= movedata.addlEffect
        private Target target { get { return _baseData.Target; } }          //= movedata.target
        private int priority { get { return _baseData.Priority; } }         //= movedata.priority
        private PokemonEssential.Flags flags { get { return _baseData.Flags; } }             //= movedata.flags
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
	
        //ToDo = Interface to call Move's function
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
        //10_000_000_VOLT_THUNDERBOLT = 719,
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
}

namespace PokemonEssential
{
    public class Flags
    {
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
        /// <summary>
        /// The move has a 10% chance of making the opponent flinch if the user is holding a 
        /// <see cref="PokemonUnity.Item.Items.KINGS_ROCK"/>/<see cref="PokemonUnity.Item.Items.RAZOR_FANG"/>. 
        /// Use this flag for all damaging moves that don't already have a flinching effect.
        /// </summary>
        public bool Flinch;
        /// <summary>
        /// If the user is <see cref="Status.Frozen"/>, the move will thaw it out before it is used.
        /// </summary>
        /// Thaw
        public bool Defrost;
        /// <summary>
        /// The move has a high critical hit rate.
        /// </summary>
        public bool Crit;
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
		public Flags(bool authentic = false, bool bite = false, bool bullet = false, bool charge = false, bool contact = false, bool crit = false, bool dance = false, bool defrost = false,
					bool distance = false, bool flinch = false, bool gravity = false, bool heal = false, bool mirror = false, bool mystery = false, bool nonsky = false, bool powder = false,
					bool protect = false, bool pulse = false, bool punch = false, bool recharge = false, bool reflectable = false, bool snatch = false, bool sound = false)
		{
			//this.Authentic = authentic;
			this.Bite = bite;
			this.BombBased = bullet;
			//this.Charge = charge;
			this.Contact = contact;
			this.Crit = crit;
			//this.Dance = dance;
			this.Defrost = defrost;
			//this.Distance = distance;
			this.Flinch = flinch;
			//this.Gravity = gravity;
			//this.Heal = heal;
			this.Mirror = mirror;
			//this.Mystery = mystery;
			//this.Nonsky = nonsky;
			this.PowderBased = powder;
			this.Protectable = protect;
			this.PulseBased = pulse;
			this.Punching = punch;
			//this.Recharge = recharge;
			this.Reflectable = reflectable;
			this.Snatch = snatch;
			this.SoundBased = sound;
		}
	}
	//NOTYPE          = 0x01,
	//IGNOREPKMNTYPES = 0x02,
	//NOWEIGHTING     = 0x04,
	//NOCRITICAL      = 0x08,
	//NOREFLECT       = 0x10,
	//SELFCONFUSE     = 0x20
    public enum SpecialCondition
    {
        NOTYPE			= 0x01,
        IGNOREPKMNTYPES	= 0x02,
        NOWEIGHTING		= 0x04,
        NOCRITICAL		= 0x08,
        NOREFLECT		= 0x10,
        SELFCONFUSE		= 0x20
    }
	/// <summary>
	/// </summary>
    /// ToDo = Create Pokemon Showdown Move class
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
	public partial class MoveData
	{
		#region Variables
		public Category Category { get; private set; }
		public int num { get; private set; }
		public Moves ID { get; private set; }
		/// <summary>
		/// The move's accuracy, as a percentage. 
		/// An accuracy of 0 means the move doesn't perform an accuracy check 
		/// (i.e. it cannot be evaded).
		/// </summary>
		public int Accuracy { get; private set; }
		public int BaseDamage { get; private set; }
		public int PP { get; private set; }
		public int Priority { get; private set; }
		public Flags Flags { get; private set; }
		public Target Target { get; private set; }
		public Types Type { get; private set; }
		public Contest ContestType { get; private set; }
		public short Function { get; private set; }
		public string FunctionAsString { get; private set; }
		/// <summary>
		/// The probability that the move's additional effect occurs, as a percentage. 
		/// If the move has no additional effect (e.g. all status moves), this value is 0.
		/// Note that some moves have an additional effect chance of 100 (e.g.Acid Spray), 
		/// which is not the same thing as having an effect that will always occur. 
		/// Abilities like Sheer Force and Shield Dust only affect additional effects, not regular effects.
		/// </summary>
		public int Effects { get; private set; }
		public string Name { get; private set; }
		public string Description { get; private set; }
		//ToDo: Missing from Database
		public int Appeal { get; private set; }
		public int Jamming { get; private set; }
		#endregion

		public MoveData()
		{
			Name = LanguageExtension.Translate(Text.Moves, ID.ToString()).Name;
			Description = LanguageExtension.Translate(Text.Moves, ID.ToString()).Value;
		}

		internal MoveData getMove(Moves ID)
		{
			foreach (MoveData move in Database)
			{
				if (move.ID == ID) return move;
			}
			throw new System.Exception("Move ID doesnt exist in the database. Please check MoveData constructor.");
		}
	}
}