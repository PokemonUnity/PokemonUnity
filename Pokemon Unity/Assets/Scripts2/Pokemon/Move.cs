﻿using System;
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
	#region Properties
	protected MoveDataDex _base { get; private set; }
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
	public Move(Moves move = Moves.NONE) { /*if (move != Moves.NONE)*/ _base = new MoveDataDex().getMove(move); PP = _base.PP; }

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
		private MoveDataDex _baseData { get; set; }
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

namespace PokemonShowdown
{
    public class Flags
    {
        /// <summary>
        /// Ignores a target's substitute.
        /// </summary>
        public bool authentic { get; private set; }
        /// <summary>
        /// The move is a biting move (powered up by the ability Strong Jaw).
        /// </summary>
		/// Power is multiplied by 1.5 when used by a Pokemon with the Ability Strong Jaw.
        public bool bite { get; private set; }
		/// <summary>
		/// The move is a bomb-based move (resisted by the ability Bulletproof).
		/// </summary>
		/// Has no effect on Pokemon with the Ability Bulletproof.
		public bool bullet { get; private set; }
		/// <summary>
		/// The user is unable to make a move between turns.
		/// </summary>
		public bool charge { get; private set; }
		/// <summary>
		/// The move makes physical contact with the target
		/// </summary>
		/// Makes contact.
		public bool contact { get; private set; }
		/// <summary>
		/// When used by a Pokemon, other Pokemon with the Ability Dancer can attempt to execute the same move.
		/// </summary>
		public bool dance { get; private set; }
		/// <summary>
		/// If the user is <see cref="Status.Frozen"/>, the move will thaw it out before it is used.
		/// </summary>
		/// Thaws the user if executed successfully while the user is frozen.
		public bool defrost { get; private set; }
		/// <summary>
		/// Can target a Pokemon positioned anywhere in a Triple Battle.
		/// </summary>
		public bool distance { get; private set; }
		/// <summary>
		/// Prevented from being executed or selected during Gravity's effect.
		/// </summary>
		public bool gravity { get; private set; }
		/// <summary>
		/// Prevented from being executed or selected during Heal Block's effect.
		/// </summary>
		public bool heal { get; private set; }
		/// <summary>
		/// The move can be copied by <see cref="Moves.Mirror_Move"/>.
		/// </summary>
		/// Can be copied by Mirror Move.
		public bool mirror { get; private set; }
		/// <summary>
		/// Unknown effect.
		/// </summary>
		public bool mystery { get; private set; }
		/// <summary>
		/// Prevented from being executed or selected in a Sky Battle.
		/// </summary>
		public bool nonsky { get; private set; }
		/// <summary>
		/// The move is a powder-based move (Grass-type Pokémon are immune to them).
		/// </summary>
		/// Has no effect on Grass-type Pokemon, Pokemon with the Ability Overcoat, and Pokemon holding Safety Goggles.
		public bool powder { get; private set; }
		/// <summary>
		/// The target can use <see cref="Moves.Protect"/> or <see cref="Moves.Detect"/> to protect itself from the move
		/// </summary>
		/// Blocked by Detect, Protect, Spiky Shield, and if not a Status move, King's Shield.
		public bool protect { get; private set; }
		/// <summary>
		/// The move is a pulse-based move (powered up by the ability Mega Launcher).
		/// </summary>
		/// Power is multiplied by 1.5 when used by a Pokemon with the Ability Mega Launcher.
		public bool pulse { get; private set; }
        /// <summary>
        /// The move is a punching move (powered up by the ability Iron Fist).
        /// </summary>
		/// Power is multiplied by 1.2 when used by a Pokemon with the Ability Iron Fist.
        public bool punch { get; private set; }
		/// <summary>
		/// If this move is successful, the user must recharge on the following turn and cannot make a move.
		/// </summary>
		public bool recharge { get; private set; }
		/// <summary>
		/// The target can use <see cref="Moves.Magic_Coat"/> to redirect the effect of the move. 
		/// Use this flag if the move deals no damage but causes a negative effect on the target.
		/// (Flags <see cref="MagicCoat"/> and <see cref="Snatch"/> are mutually exclusive.)
		/// </summary>
		/// Bounced back to the original user by Magic Coat or the Ability Magic Bounce.
		public bool reflectable { get; private set; }
		/// <summary>
		/// The target can use <see cref="Moves.Snatch"/> to steal the effect of the move. 
		/// Use this flag for most moves that target the user.
		/// (Flags <see cref="MagicCoat"/> and <see cref="Snatch"/> are mutually exclusive.)
		/// </summary>
		/// Can be stolen from the original user and instead used by another Pokemon using Snatch.
		public bool snatch { get; private set; }
        /// <summary>
        /// The move is a sound-based move.
        /// </summary>
		/// Has no effect on Pokemon with the Ability Soundproof.
        public bool sound { get; private set; }
        public Flags(bool authentic = false, bool bite = false, bool bullet = false, bool charge = false, bool contact = false, bool dance = false, bool defrost = false,
					bool distance = false, bool gravity = false, bool heal = false, bool mirror = false, bool mystery = false, bool nonsky = false, bool powder = false, 
					bool protect = false, bool pulse = false, bool punch = false, bool recharge = false, bool reflectable = false, bool snatch = false, bool sound = false) {
			this.authentic = authentic;
			this.bite = bite;
			this.bullet = bullet;
			this.charge = charge;
			this.contact = contact;
			this.dance = dance;
			this.defrost = defrost;
			this.distance = distance;
			this.gravity = gravity;
			this.heal = heal;
			this.mirror = mirror;
			this.mystery = mystery;
			this.nonsky = nonsky;
			this.powder = powder;
			this.protect = protect;
			this.pulse = pulse;
			this.punch = punch;
			this.recharge = recharge;
			this.reflectable = reflectable;
			this.snatch = snatch;
			this.sound = sound;
		}
    }
    public class Boosts
	{

	}
	public class Secondary
	{
	}
	public class BattleMovedex
	{
		private static readonly BattleMovedex[] Database;

		#region Variables
		public Category category { get; private set; }
		public int num { get; private set; }
		public int? accuracy { get; private set; }
		public bool isViable { get; private set; }
		public int basePower { get; private set; }
		public Moves id { get; private set; }
		public int pp { get; private set; }
		public int priority { get; private set; }
		public Flags flags { get; private set; }
		public string isZ { get; private set; }
		public int critRatio { get; private set; }
		public Secondary secondary { get; private set; }
		public Target target { get; private set; }
		public Types type { get; private set; }
		public Contest contestType { get; private set; }
		public Boosts boosts { get; private set; }
		public Effects.Move effect { get; private set; }
		public int[] drain { get; private set; }
		public int[] multihit { get; private set; }
		public int[] heal { get; private set; }
		
		public int zMovePower { get; private set; }
		public string zMoveEffect { get; private set; }
		public Boosts zMoveBoost { get; private set; }
		public string selfSwitch { get; private set; }
		public string volatileStatus { get; private set; }
		#endregion

		public BattleMovedex(){}

		static BattleMovedex()
		{
Database = new BattleMovedex[] {
	#region Move Database
	/*new BattleMovedex() {
		num = 719,
		//accuracy = true,
		basePower = 195,
		category = Category.SPECIAL,
		//desc = "Has a very high chance for a critical hit.",
		//shortDesc = "Very high critical hit ratio.",
		id = Moves.10000000VOLTTHUNDERBOLT,
		//name = "10,000,000 Volt Thunderbolt",
		pp = 1,
		priority = 0,
		flags = new Flags (),
		isZ = "pikashuniumz",
		critRatio = 3,
		//secondary = false,
		target = Target.Normal,
		type = Types.ELECTRIC,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 71,
		accuracy = 100,
		basePower = 20,
		category = Category.SPECIAL,
		//desc = "The user recovers 1/2 the HP lost by the target, rounded half up. If Big Root is held by the user, the HP recovered is 1.3x normal, rounded half down.",
		//shortDesc = "User recovers 50% of the damage dealt.",
		id = Moves.ABSORB,
		//name = "Absorb",
		pp = 25,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, heal: true),
		drain = new int[] {1, 2},
		//secondary = false,
		target = Target.Normal,
		type = Types.GRASS,
		zMovePower = 100,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 709,
		accuracy = 100,
		basePower = 40,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "Usually goes first.",
		id = Moves.ACCELEROCK,
		isViable = true,
		//name = "Accelerock",
		pp = 20,
		priority = 1,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.ROCK,
		zMovePower = 100,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 51,
		accuracy = 100,
		basePower = 40,
		category = Category.SPECIAL,
		//desc = "Has a 10% chance to lower the target's Special Defense by 1 stage.",
		//shortDesc = "10% chance to lower the foe(s) Sp. Def by 1.",
		id = Moves.ACID,
		//name = "Acid",
		pp = 30,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 10,
			boosts = new Boosts() {
				spd = -1
			}
		},
		target = Target.AllAdjacentFoes,
		type = Types.POISON,
		zMovePower = 100,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 151,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the user's Defense by 2 stages.",
		//shortDesc = "Raises the user's Defense by 2.",
		id = Moves.ACID_ARMOR,
		//name = "Acid Armor",
		pp = 20,
		priority = 0,
		flags = new Flags (snatch: true),
		boosts = new Boosts() {
			def = 2
		},
		//secondary = false,
		target = Target.Self,
		type = Types.POISON,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 628,
		//accuracy = true,
		basePower = 1,
		category = Category.PHYSICAL,
		//shortDesc = "Power is equal to the base move's Z-Power.",
		id = Moves.ACID_DOWNPOUR__PHYSICAL,
		isViable = true,
		//name = "Acid Downpour",
		pp = 1,
		priority = 0,
		flags = new Flags (),
		isZ = "poisoniumz",
		//secondary = false,
		target = Target.Normal,
		type = Types.POISON,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 491,
		accuracy = 100,
		basePower = 40,
		category = Category.SPECIAL,
		//desc = "Has a 100% chance to lower the target's Special Defense by 2 stages.",
		//shortDesc = "100% chance to lower the target's Sp. Def by 2.",
		id = Moves.ACID_SPRAY,
		isViable = true,
		//name = "Acid Spray",
		pp = 20,
		priority = 0,
		flags = new Flags (bullet: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 100,
			boosts = new Boosts() {
				spd = -2
			}
		},
		target = Target.Normal,
		type = Types.POISON,
		zMovePower = 100,
		contestType = Contest.BEAUTIFUL
	},
	new BattleMovedex() {
		num = 512,
		accuracy = 100,
		basePower = 55,
		//basePowerCallback = function (pokemon, target, move) {
		//	if (!pokemon.item) {
		//		this.debug("Power doubled for no item");
		//		return move.basePower * 2;
		//	}
		//	return move.basePower;
		//},
		category = Category.PHYSICAL,
		//desc = "Power doubles if the user has no held item.",
		//shortDesc = "Power doubles if the user has no held item.",
		id = Moves.ACROBATICS,
		isViable = true,
		//name = "Acrobatics",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true, distance: true),
		//secondary = false,
		target = Target.Any,
		type = Types.FLYING,
		zMovePower = 100,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 367,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises a random stat by 2 stages as long as the stat is not already at stage 6. The user can choose to use this move on itself or an adjacent ally. Fails if no stat stage can be raised or if used on an ally with a substitute.",
		//shortDesc = "Raises a random stat of the user or an ally by 2.",
		id = Moves.ACUPRESSURE,
		//name = "Acupressure",
		pp = 30,
		priority = 0,
		flags = new Flags (),
		//onHit = function (target) {
		//	let stats = [];
		//	for (let stat in target.Boosts) {
		//		if (target.Boosts[stat] < 6) {
		//			stats.push(stat);
		//		}
		//	}
		//	if (stats.length) {
		//		let randomStat = this.sample(stats);
		//		let boost = {};
		//		boost[randomStat] = 2;
		//		this.boost(boost);
		//	} else {
		//		return false;
		//	}
		//},
		//secondary = false,
		target = Target.AdjacentAllyOrSelf,
		type = Types.NORMAL,
		zMoveEffect = "crit2",
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 332,
		//accuracy = true,
		basePower = 60,
		category = Category.PHYSICAL,
		//desc = "This move does not check accuracy.",
		//shortDesc = "This move does not check accuracy.",
		id = Moves.AERIALACE,
		isViable = true,
		//name = "Aerial Ace",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true, distance: true),
		//secondary = false,
		target = Target.Any,
		type = Types.FLYING,
		zMovePower = 120,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 177,
		accuracy = 95,
		basePower = 100,
		category = Category.SPECIAL,
		//desc = "Has a higher chance for a critical hit.",
		//shortDesc = "High critical hit ratio.",
		id = Moves.AEROBLAST,
		isViable = true,
		//name = "Aeroblast",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, distance: true),
		critRatio = 2,
		//secondary = false,
		target = Target.Any,
		type = Types.FLYING,
		zMovePower = 180,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 495,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The target makes its move immediately after the user this turn, no matter the priority of its selected move. Fails if the target would have moved next anyway, or if the target already moved this turn.",
		//shortDesc = "The target makes its move right after the user.",
		id = Moves.AFTERYOU,
		//name = "After You",
		pp = 15,
		priority = 0,
		flags = new Flags (authentic: true, mystery: true),
		//onHit = function (target) {
		//	if (target.Side.active.length < 2) return false; // fails in singles
		//	let action = this.willMove(target);
		//	if (action) {
		//		this.cancelMove(target);
		//		this.queue.unshift(action);
		//		this.add('-activate', target, 'move = After You');
		//	} else {
		//		return false;
		//	}
		//},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMoveBoost = {spe = 1},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 97,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the user's Speed by 2 stages.",
		//shortDesc = "Raises the user's Speed by 2.",
		id = Moves.AGILITY,
		isViable = true,
		//name = "Agility",
		pp = 30,
		priority = 0,
		flags = new Flags (snatch: true),
		boosts = new Boosts() {
			spe = 2
		},
		//secondary = false,
		target = Target.Self,
		type = Types.PSYCHIC,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 314,
		accuracy = 95,
		basePower = 60,
		category = Category.SPECIAL,
		//desc = "Has a higher chance for a critical hit.",
		//shortDesc = "High critical hit ratio. Hits adjacent foes.",
		id = Moves.AIRCUTTER,
		//name = "Air Cutter",
		pp = 25,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		critRatio = 2,
		//secondary = false,
		target = Target.AllAdjacentFoes,
		type = Types.FLYING,
		zMovePower = 120,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 403,
		accuracy = 95,
		basePower = 75,
		category = Category.SPECIAL,
		//desc = "Has a 30% chance to flinch the target.",
		//shortDesc = "30% chance to flinch the target.",
		id = Moves.AIRSLASH,
		isViable = true,
		//name = "Air Slash",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, distance: true),
		secondary = new Secondary() {
			chance = 30,
			volatileStatus = "flinch"
		},
		target = Target.Any,
		type = Types.FLYING,
		zMovePower = 140,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 624,
		//accuracy = true,
		basePower = 1,
		category = Category.PHYSICAL,
		//shortDesc = "Power is equal to the base move's Z-Power.",
		id = Moves.ALLOUTPUMMELING,
		isViable = true,
		//name = "All-Out Pummeling",
		pp = 1,
		priority = 0,
		flags = new Flags (),
		isZ = "fightiniumz",
		//secondary = false,
		target = Target.Normal,
		type = Types.FIGHTING,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 502,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user swaps positions with its ally. Fails if the user is the only Pokemon on its side.",
		//shortDesc = "The user swaps positions with its ally.",
		id = Moves.ALLYSWITCH,
		//name = "Ally Switch",
		pp = 15,
		priority = 2,
		flags = new Flags (),
		//onTryHit = function (source) {
		//	if (source.side.active.length === 1) return false;
		//	if (source.side.active.length === 3 && source.position === 1) return false;
		//},
		//onHit = function (pokemon) {
		//	let newPosition = (pokemon.position === 0 ? pokemon.side.active.length - 1  = 0);
		//	if (!pokemon.side.active[newPosition]) return false;
		//	if (pokemon.side.active[newPosition].fainted) return false;
		//	this.swapPosition(pokemon, newPosition, '[from] move = Ally Switch');
		//},
		//secondary = false,
		target = Target.Self,
		type = Types.PSYCHIC,
		zMoveBoost = new Boosts() {spe = 2},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 133,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the user's Special Defense by 2 stages.",
		//shortDesc = "Raises the user's Sp. Def by 2.",
		id = Moves.AMNESIA,
		//name = "Amnesia",
		pp = 20,
		priority = 0,
		flags = new Flags (snatch: true),
		boosts = new Boosts() {
			spd = 2
		},
		//secondary = false,
		target = Target.Self,
		type = Types.PSYCHIC,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 677,
		accuracy = 100,
		basePower = 80,
		category = Category.PHYSICAL,
		//desc = "Prevents the target From switching out. The target Can still switch out if it is holding Shed Shell or uses Baton Pass, Parting Shot, U-turn, or Volt Switch. If the target Leaves the field using Baton Pass, the replacement will remain trapped. The effect ends if the user leaves the field.",
		//shortDesc = "Prevents the target From switching out.",
		id = Moves.ANCHORSHOT,
		isViable = true,
		//name = "Anchor Shot",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 100,
			//onHit = function (target, source, move) {
			//	if (source.isActive) target.AddVolatile("trapped", source, move, "trapper");
			//}
		},
		target = Target.Normal,
		type = Types.STEEL,
		zMovePower = 160,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 246,
		accuracy = 100,
		basePower = 60,
		category = Category.SPECIAL,
		//desc = "Has a 10% chance to raise the user's Attack, Defense, Special Attack, Special Defense, and Speed by 1 stage.",
		//shortDesc = "10% chance to raise all stats by 1 (not acc/eva).",
		id = Moves.ANCIENTPOWER,
		//name = "Ancient Power",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 10,
			self = {
				boosts = new Boosts() {
					atk = 1,
					def = 1,
					spa = 1,
					spd = 1,
					spe = 1
				}
			}
		},
		target = Target.Normal,
		type = Types.ROCK,
		zMovePower = 120,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 453,
		accuracy = 100,
		basePower = 40,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "Usually goes first.",
		id = Moves.AQUAJET,
		isViable = true,
		//name = "Aqua Jet",
		pp = 20,
		priority = 1,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.WATER,
		zMovePower = 100,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 392,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user has 1/16 of its maximum HP, rounded down, restored at the end of each turn while it remains active. If the user uses Baton Pass, the replacement will receive the healing effect.",
		//shortDesc = "User recovers 1/16 max HP per turn.",
		id = Moves.AQUARING,
		//name = "Aqua Ring",
		pp = 20,
		priority = 0,
		flags = new Flags (snatch: true),
		volatileStatus = "aquaring",
		effect = {
			//onStart = function (pokemon) {
			//	this.add('-start', pokemon, 'Aqua Ring');
			//},
			//onResidualOrder = 6,
			//onResidual = function (pokemon) {
			//	this.heal(pokemon.maxhp / 16);
			//}
		},
		//secondary = false,
		target = Target.Self,
		type = Types.WATER,
		zMoveBoost = new Boosts() {def = 1},
		contestType = Contest.BEAUTIFUL
	},
	new BattleMovedex() {
		num = 401,
		accuracy = 90,
		basePower = 90,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect.",
		id = Moves.AQUATAIL,
		isViable = true,
		//name = "Aqua Tail",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.WATER,
		zMovePower = 175,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 292,
		accuracy = 100,
		basePower = 15,
		category = Category.PHYSICAL,
		//desc = "Hits two to five times. Has a 1/3 chance to hit two or three times, and a 1/6 chance to hit four or five times. If one of the hits breaks the target'S substitute, it will take damage for the remaining hits. If the user has the Ability Skill Link, this move will always hit five times.",
		//shortDesc = "Hits 2-5 times in one turn.",
		id = Moves.ARMTHRUST,
		//name = "Arm Thrust",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		multihit = new int[] {2, 5},
		//secondary = false,
		target = Target.Normal,
		type = Types.FIGHTING,
		zMovePower = 100,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 312,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Every Pokemon in the user's party is cured of its major status condition. Active Pokemon with the Ability Sap Sipper are not cured, unless they are the user.",
		//shortDesc = "Cures the user's party of all status conditions.",
		id = Moves.AROMATHERAPY,
		isViable = true,
		//name = "Aromatherapy",
		pp = 5,
		priority = 0,
		flags = new Flags (snatch: true, distance: true),
		//onHit = function (pokemon, source, move) {
		//	this.add('-activate', source, 'move = Aromatherapy');
		//	let success = false;
		//	for (const ally of pokemon.side.pokemon) {
		//		if (ally !== source && ((ally.hasAbility("sapsipper")) ||
		//				(ally.volatiles["substitute"] && !move.infiltrates))) {
		//			continue;
		//		}
		//		if (ally.cureStatus()) success = true;
		//	}
		//	return success;
		//},
		target = Target.AllyTeam,
		type = Types.GRASS,
		zMoveEffect = "heal",
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 597,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the target'S Special Defense by 1 stage. Fails if there is no ally adjacent to the user.",
		//shortDesc = "Raises an ally's Sp. Def by 1.",
		id = Moves.AROMATICMIST,
		//name = "Aromatic Mist",
		pp = 20,
		priority = 0,
		flags = new Flags (authentic: true),
		boosts = new Boosts() {
			spd = 1
		},
		//secondary = false,
		target = Target.AdjacentAlly,
		type = Types.FAIRY,
		zMoveBoost = new Boosts() {spd = 2},
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 274,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "A random move among those known by the user's party members is selected for use. Does not select Assist, Baneful Bunker, Beak Blast, Belch, Bestow, Bounce, Chatter, Circle Throw, Copycat, Counter, Covet, Destiny Bond, Detect, Dig, Dive, Dragon Tail, Endure, Feint, Fly, Focus Punch, Follow Me, Helping Hand, Hold Hands, King's Shield, Mat Block, Me First, Metronome, Mimic, Mirror Coat, Mirror Move, Nature Power, Phantom Force, Protect, Rage Powder, Roar, Shadow Force, Shell Trap, Sketch, Sky Drop, Sleep Talk, Snatch, Spiky Shield, Spotlight, Struggle, Switcheroo, Thief, Transform, Trick, Whirlwind, or Z-Moves.",
		//shortDesc = "Uses a random move known by a team member.",
		id = Moves.ASSIST,
		//name = "Assist",
		pp = 20,
		priority = 0,
		flags = new Flags (),
		//onHit = function (target) {
		//	let moves = [];
		//	for (const pokemon of target.Side.pokemon) {
		//		if (pokemon === target) continue;
		//		for (const moveSlOT of pokemon.moveSlOTS) {
		//			let move = moveSlOT.id;
		//			let noAssist = [
		//				"assist", "banefulbunker", "beakblast", "belch", "bestow", "bounce", "chatter", "circlethrow", "copycat", "counter", "covet", "destinybond", "detect", "dig", "dive", "dragontail", "endure", "feint", "fly", "focuspunch", "followme", "helpinghand", "holdhands", "kingsshield", "matblock", "mefirst", "metronome", "mimic", "mirrorcoat", "mirrormove", "naturepower", "phantomforce", "protect", "ragepowder", "roar", "shadowforce", "shelltrap", "sketch", "skydrop", "sleeptalk", "snatch", "spikyshield", "spotlight", "struggle", "switcheroo", "thief", "transform", "trick", "whirlwind",
		//			];
		//			if (!noAssist.includes(move) && !this.getMove(move).isZ) {
		//				moves.PUSH(move);
		//			}
		//		}
		//	}
		//	let randomMove = "";
		//	if (moves.LENGTH) randomMove = this.sample(moves);
		//	if (!randomMove) {
		//		return false;
		//	}
		//	this.useMove(randomMove, target);
		//},
		//secondary = false,
		target = Target.Self,
		type = Types.NORMAL,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 372,
		accuracy = 100,
		basePower = 60,
		//basePowerCallback = function (pokemon, target, move) {
		//	if (pokemon.volatiles.assurance && pokemon.volatiles.assurance.hurt) {
		//		this.debug('Boosted for being damaged this turn');
		//		return move.basePower * 2;
		//	}
		//	return move.basePower;
		//},
		category = Category.PHYSICAL,
		//desc = "Power doubles if the target Has already taken damage this turn, other than direct damage from Belly Drum, confusion, Curse, or Pain Split.",
		//shortDesc = "Power doubles if target Was damaged this turn.",
		id = Moves.ASSURANCE,
		//name = "Assurance",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//beforeTurnCallback = function (pokemon, target) {
		//	pokemon.addVolatile("assurance");
		//	pokemon.volatiles.assurance.position = target.Position;
		//},
		effect = {
			duration = 1,
			//onFoeAfterDamage = function (damage, target) {
			//	if (target.Position === this.effectData.position) {
			//		this.debug('damaged this turn');
			//		this.effectData.hurt = true;
			//	}
			//},
			//onFoeSwitchOut = function (pokemon) {
			//	if (pokemon.position === this.effectData.position) {
			//		this.effectData.hurt = false;
			//	}
			//}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.DARK,
		zMovePower = 120,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 310,
		accuracy = 100,
		basePower = 30,
		category = Category.PHYSICAL,
		//desc = "Has a 30% chance to flinch the target.",
		//shortDesc = "30% chance to flinch the target.",
		id = Moves.ASTONISH,
		//name = "Astonish",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 30,
			volatileStatus = "flinch"
		},
		target = Target.Normal,
		type = Types.GHOST,
		zMovePower = 100,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 454,
		accuracy = 100,
		basePower = 90,
		category = Category.PHYSICAL,
		//desc = "Has a higher chance for a critical hit.",
		//shortDesc = "High critical hit ratio.",
		id = Moves.ATTACKORDER,
		isViable = true,
		//name = "Attack Order",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		critRatio = 2,
		//secondary = false,
		target = Target.Normal,
		type = Types.BUG,
		zMovePower = 175,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 213,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Causes the target To become infatuated, making it unable to attack 50% of the time. Fails if both the user and the target Are the same gender, if either is genderless, or if the target Is already infatuated. The effect ends when either the user or the target Is no longer active. Pokemon with the Ability Oblivious or protected by the Ability Aroma Veil are immune.",
		//shortDesc = "A target Of the opposite gender gets infatuated.",
		id = Moves.ATTRACT,
		//name = "Attract",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true, authentic: true),
		volatileStatus = "attract",
		effect = {
			noCopy = true, // doesn't get copied by Baton Pass
			//onStart = function (pokemon, source, effect) {
			//	if (!(pokemon.gender === "M" && source.gender === "F") && !(pokemon.gender === "F" && source.gender === "M")) {
			//		this.debug('incompatible gender');
			//		return false;
			//	}
			//	if (!this.runEvent("Attract", pokemon, source)) {
			//		this.debug('Attract event failed');
			//		return false;
			//	}

			//	if (effect.id === "cutecharm") {
			//		this.add('-start', pokemon, "Attract", '[from] ability = Cute Charm', '[of] ' + source);
			//	} else if (effect.id === "destinyknot") {
			//		this.add('-start', pokemon, "Attract", '[from] item = Destiny Knot', '[of] ' + source);
			//	} else {
			//		this.add('-start', pokemon, "Attract");
			//	}
			//},
			//onUpdate = function (pokemon) {
			//	if (this.effectData.source && !this.effectData.source.isActive && pokemon.volatiles["attract"]) {
			//		this.debug('Removing Attract volatile on ' + pokemon);
			//		pokemon.removeVolatile("attract");
			//	}
			//},
			//onBeforeMovePriority = 2,
			//onBeforeMove = function (pokemon, target, move) {
			//	this.add('-activate', pokemon, 'move = Attract', '[of] ' + this.effectData.source);
			//	if (this.randomChance(1, 2)) {
			//		this.add("cant", pokemon, "Attract");
			//		return false;
			//	}
			//},
			//onEnd = function (pokemon) {
			//	this.add('-end', pokemon, "Attract", '[silent]');
			//}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 396,
		//accuracy = true,
		basePower = 80,
		category = Category.SPECIAL,
		//desc = "This move does not check accuracy.",
		//shortDesc = "This move does not check accuracy.",
		id = Moves.AURASPHERE,
		isViable = true,
		//name = "Aura Sphere",
		pp = 20,
		priority = 0,
		flags = new Flags (bullet: true, protect: true, pulse: true, mirror: true, distance: true),
		//secondary = false,
		target = Target.Any,
		type = Types.FIGHTING,
		zMovePower = 160,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 62,
		accuracy = 100,
		basePower = 65,
		category = Category.SPECIAL,
		//desc = "Has a 10% chance to lower the target'S Attack by 1 stage.",
		//shortDesc = "10% chance to lower the foe's Attack by 1.",
		id = Moves.AURORABEAM,
		//name = "Aurora Beam",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 10,
			boosts = new Boosts() {
				atk = -1
			}
		},
		target = Target.Normal,
		type = Types.ICE,
		zMovePower = 120,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 694,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "For 5 turns, the user and its party members take 0.5x damage from physical and special attacks, or 0.66x damage if in a Double Battle; does not reduce damage further with Reflect or Light Screen. Critical hits ignore this protection. It is removed from the user's side if the user or an ally is successfully hit by Brick Break, Psychic Fangs, or Defog. Brick Break and Psychic Fangs remove the effect before damage is calculated. Lasts for 8 turns if the user is holding Light Clay. Fails unless the weather is Hail.",
		//shortDesc = "For 5 turns, damage to allies is halved. Hail only.",
		id = Moves.AURORAVEIL,
		isViable = true,
		//name = "Aurora Veil",
		pp = 20,
		priority = 0,
		flags = new Flags (snatch: true),
		sideCondition = "auroraveil",
		//onTryHitSide = function () {
		//	if (!this.isWeather("hail")) return false;
		//},
		effect = {
			duration = 5,
			//durationCallback = function (target, source, effect) {
			//	if (source && source.hasItem("lightclay")) {
			//		return 8;
			//	}
			//	return 5;
			//},
			//onAnyModifyDamage = function (damage, source, target, move) {
			//	if (target !== source && target.Side === this.effectData.target) {
			//		if ((target.Side.sideConditions["reflect"] && this.getCategory(move) === "Physical") ||
			//				(target.Side.sideConditions["lightscreen"] && this.getCategory(move) === "Special")) {
			//			return;
			//		}
			//		if (!move.crit && !move.infiltrates) {
			//			this.debug('Aurora Veil weaken');
			//			if (target.Side.active.length > 1) return this.chainModify([0xAAC, 0x1000]);
			//			return this.chainModify(0.5);
			//		}
			//	}
			//},
			//onStart = function (side) {
			//	this.add('-sidestart', side, 'move = Aurora Veil');
			//},
			//onResidualOrder = 21,
			//onResidualSubOrder = 1,
			//onEnd = function (side) {
			//	this.add('-sideend', side, 'move = Aurora Veil');
			//}
		},
		//secondary = false,
		target = Target.AllySide,
		type = Types.ICE,
		zMoveBoost = new Boosts() {spe = 1},
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 475,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the user's Speed by 2 stages. If the user's Speed was changed, the user's weight is reduced by 100 kg as long as it remains active. This effect is stackable but cannot reduce the user's weight to less than 0.1 kg.",
		//shortDesc = "Raises the user's Speed by 2; user loses 100 kg.",
		id = Moves.AUTOTOMIZE,
		isViable = true,
		//name = "Autotomize",
		pp = 15,
		priority = 0,
		flags = new Flags (snatch: true),
		//onTryHit = function (pokemon) {
		//	let hasContrary = pokemon.hasAbility("contrary");
		//	if ((!hasContrary && pokemon.boosts.spe === 6) || (hasContrary && pokemon.boosts.spe === -6)) {
		//		return false;
		//	}
		//},
		boosts = new Boosts() {
			spe = 2
		},
		volatileStatus = "autotomize",
		effect = {
			noCopy = true, // doesn't get copied by Baton Pass
			//onStart = function (pokemon) {
			//	if (pokemon.template.weightkg > 0.1) {
			//		this.effectData.multiplier = 1;
			//		this.add('-start', pokemon, "Autotomize");
			//	}
			//},
			//onRestart = function (pokemon) {
			//	if (pokemon.template.weightkg - (this.effectData.multiplier * 100) > 0.1) {
			//		this.effectData.multiplier++;
			//		this.add('-start', pokemon, "Autotomize");
			//	}
			//},
			//onModifyWeightPriority = 1,
			//onModifyWeight = function (weight, pokemon) {
			//	if (this.effectData.multiplier) {
			//		weight -= this.effectData.multiplier * 100;
			//		if (weight < 0.1) weight = 0.1;
			//		return weight;
			//	}
			//}
		},
		//secondary = false,
		target = Target.Self,
		type = Types.STEEL,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 419,
		accuracy = 100,
		basePower = 60,
		//basePowerCallback = function (pokemon, target, move) {
		//	if (target.LastDamage > 0 && pokemon.lastAttackedBy && pokemon.lastAttackedBy.thisTurn && pokemon.lastAttackedBy.pokemon === target) {
		//		this.debug('Boosted for getting hit by ' + pokemon.lastAttackedBy.move);
		//		return move.basePower * 2;
		//	}
		//	return move.basePower;
		//},
		category = Category.PHYSICAL,
		//desc = "Power doubles if the user was hit by the target This turn.",
		//shortDesc = "Power doubles if user is damaged by the target.",
		id = Moves.AVALANCHE,
		isViable = true,
		//name = "Avalanche",
		pp = 10,
		priority = -4,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.ICE,
		zMovePower = 120,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 608,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Lowers the target'S Attack by 1 stage.",
		//shortDesc = "Lowers the target'S Attack by 1.",
		id = Moves.BABYDOLLEYES,
		//name = "Baby-Doll Eyes",
		pp = 30,
		priority = 1,
		flags = new Flags (protect: true, reflectable: true, mirror: true, mystery: true),
		boosts = new Boosts() {
			atk = -1
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.FAIRY,
		zMoveBoost = new Boosts() {def = 1},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 661,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user is protected from most attacks made by other Pokemon during this turn, and Pokemon making contact with the user become poisoned. This move has a 1/X chance of being successful, where X starts at 1 and triples each time this move is successfully used. X resets to 1 if this move fails or if the user's last move used is not Baneful Bunker, Detect, Endure, King's Shield, Protect, Quick Guard, Spiky Shield, or Wide Guard. Fails if the user moves LAST this turn.",
		//shortDesc = "Protects from moves. Contact = poison.",
		id = Moves.BANEFULBUNKER,
		isViable = true,
		//name = "Baneful Bunker",
		pp = 10,
		priority = 4,
		flags = new Flags (),
		stallingMove = true,
		volatileStatus = "banefulbunker",
		//onTryHit = function (target, source, move) {
		//	return !!this.willAct() && this.runEvent("StallMove", target);
		//},
		//onHit = function (pokemon) {
		//	pokemon.addVolatile("stall");
		//},
		effect = {
			duration = 1,
			//onStart = function (target) {
			//	this.add('-singleturn', target, 'move = Protect');
			//},
			//onTryHitPriority = 3,
			//onTryHit = function (target, source, move) {
			//	if (!move.flags["protect"]) {
			//		if (move.isZ) move.zBrokeProtect = true;
			//		return;
			//	}
			//	this.add('-activate', target, 'move = Protect');
			//	source.moveThisTurnResult = true;
			//	let lockedmove = source.getVolatile("lockedmove");
			//	if (lockedmove) {
			//		// Outrage counter is reset
			//		if (source.volatiles["lockedmove"].duration === 2) {
			//			delete source.volatiles["lockedmove"];
			//		}
			//	}
			//	if (move.flags["contact"]) {
			//		source.trySetStatus("psn", target);
			//	}
			//	return null;
			//},
			//onHit = function (target, source, move) {
			//	if (move.zPowered && move.flags["contact"]) {
			//		source.trySetStatus("psn", target);
			//	}
			//}
		},
		//secondary = false,
		target = Target.Self,
		type = Types.POISON,
		zMoveBoost = new Boosts() {def = 1},
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 140,
		accuracy = 85,
		basePower = 15,
		category = Category.PHYSICAL,
		//desc = "Hits two to five times. Has a 1/3 chance to hit two or three times, and a 1/6 chance to hit four or five times. If one of the hits breaks the target'S substitute, it will take damage for the remaining hits. If the user has the Ability Skill Link, this move will always hit five times.",
		//shortDesc = "Hits 2-5 times in one turn.",
		id = Moves.BARRAGE,
		//name = "Barrage",
		pp = 20,
		priority = 0,
		flags = new Flags (bullet: true, protect: true, mirror: true),
		multihit = new int[] {2, 5},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 100,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 112,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the user's Defense by 2 stages.",
		//shortDesc = "Raises the user's Defense by 2.",
		id = Moves.BARRIER,
		//name = "Barrier",
		pp = 20,
		priority = 0,
		flags = new Flags (snatch: true),
		boosts = new Boosts() {
			def = 2
		},
		//secondary = false,
		target = Target.Self,
		type = Types.PSYCHIC,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 226,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user is replaced with another Pokemon in its party. The selected Pokemon has the user's stat stage changes, confusion, and certain move effects transferred to it.",
		//shortDesc = "User switches, passing stat changes and more.",
		id = Moves.BATONPASS,
		isViable = true,
		//name = "Baton Pass",
		pp = 40,
		priority = 0,
		flags = new Flags (),
		selfSwitch = "copyvolatile",
		//secondary = false,
		target = Target.Self,
		type = Types.NORMAL,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 690,
		accuracy = 100,
		basePower = 100,
		category = Category.PHYSICAL,
		//desc = "If the user is hit by a contact move this turn before it can execute this move, the attacker is burned.",
		//shortDesc = "Burns on contact with the user before it moves.",
		id = Moves.BEAKBLAST,
		isViable = true,
		//name = "Beak Blast",
		pp = 15,
		priority = -3,
		flags = new Flags (bullet: true, protect: true),
		//beforeTurnCallback = function (pokemon) {
		//	pokemon.addVolatile("beakblast");
		//},
		effect = {
			duration = 1,
			//onStart = function (pokemon) {
			//	this.add('-singleturn', pokemon, 'move = Beak Blast');
			//},
			//onHit = function (pokemon, source, move) {
			//	if (move.flags["contact"]) {
			//		source.trySetStatus("brn", pokemon);
			//	}
			//}
		},
		//onMoveAborted = function (pokemon) {
		//	pokemon.removeVolatile("beakblast");
		//},
		//onAfterMove = function (pokemon) {
		//	pokemon.removeVolatile("beakblast");
		//},
		//secondary = false,
		target = Target.Normal,
		type = Types.FLYING,
		zMovePower = 180,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 251,
		accuracy = 100,
		basePower = 0,
		//basePowerCallback = function (pokemon, target, move) {
		//	// @ts-ignore
		//	return 5 + Math.floor(move.allies.shift().template.baseStats.atk / 10);
		//},
		category = Category.PHYSICAL,
		//desc = "Hits one time for the user and one time for each unfainted Pokemon without a major status condition in the user's party. The power of each hit is equal to 5+(X/10), where X is each participating Pokemon's base Attack; each hit is considered to come from the user.",
		//shortDesc = "All healthy allies aid in damaging the target.",
		id = Moves.BEATUP,
		//name = "Beat Up",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, mystery: true),
		//onModifyMove = function (move, pokemon) {
		//	move.allies = pokemon.side.pokemon.filter(ally => ally === pokemon || !ally.fainted && !ally.status);
		//	move.multihit = move.allies.length;
		//},
		//secondary = false,
		target = Target.Normal,
		type = Types.DARK,
		zMovePower = 100,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 562,
		accuracy = 90,
		basePower = 120,
		category = Category.SPECIAL,
		//desc = "This move cannot be selected until the user eats a Berry, either by eating one that was held, stealing and eating one off another Pokemon with Bug Bite or Pluck, or eating one that was thrown at it with Fling. Once the condition is met, this move can be selected and used for the rest of the battle even if the user gains or uses another item or switches out. Consuming a Berry with Natural Gift does not count for the purposes of eating one.",
		//shortDesc = "Cannot be selected until the user eats a Berry.",
		id = Moves.BELCH,
		//name = "Belch",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true),
		// Move disabling implemented in Battle#nextTurn in sim/battle.js
		//secondary = false,
		target = Target.Normal,
		type = Types.POISON,
		zMovePower = 190,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 187,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the user's Attack by 12 stages in exchange for the user losing 1/2 of its maximum HP, rounded down. Fails if the user would faint or if its Attack stat stage is 6.",
		//shortDesc = "User loses 50% max HP. Maximizes Attack.",
		id = Moves.BELLYDRUM,
		//name = "Belly Drum",
		pp = 10,
		priority = 0,
		flags = new Flags (snatch: true),
		//onHit = function (target) {
		//	if (target.Hp <= target.Maxhp / 2 || target.Boosts.atk >= 6 || target.Maxhp === 1) { // Shedinja clause
		//		return false;
		//	}
		//	this.directDamage(target.Maxhp / 2);
		//	this.boost({atk = 12}, target);
		//},
		//secondary = false,
		target = Target.Self,
		type = Types.NORMAL,
		zMoveEffect = "heal",
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 516,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The target Receives the user's held item. Fails if the user has no item or is holding a Mail or Z-Crystal, if the target Is already holding an item, if the user is a Kyogre holding a Blue Orb, a Groudon holding a Red Orb, a Giratina holding a Griseous Orb, an Arceus holding a Plate, a Genesect holding a Drive, a Silvally holding a Memory, a Pokemon that can Mega Evolve holding the Mega Stone for its species, or if the target Is one of those Pokemon and the user is holding the respective item.",
		//shortDesc = "User passes its held item to the target.",
		id = Moves.BESTOW,
		//name = "Bestow",
		pp = 15,
		priority = 0,
		flags = new Flags (mirror: true, authentic: true, mystery: true),
		//onHit = function (target, source, move) {
		//	if (target.Item) {
		//		return false;
		//	}
		//	let myItem = source.takeItem();
		//	if (!myItem) return false;
		//	if (!this.singleEvent("TakeItem", myItem, source.itemData, target, source, move, myItem) || !target.SetItem(myItem)) {
		//		source.item = myItem.id;
		//		return false;
		//	}
		//	this.add('-item', target, myItem.name, '[from] move = Bestow', '[of] ' + source);
		//},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {spe = 2},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 117,
		//accuracy = true,
		basePower = 0,
		category = Category.PHYSICAL,
		//desc = "The user spends two turns locked into this move and then, on the second turn after using this move, the user attacks the last Pokemon that hit it, inflicting double the damage in HP it lost during the two turns. If the last Pokemon that hit it is no longer on the field, the user attacks a random foe instead. If the user is prevented from moving during this move's use, the effect ends. This move does not check accuracy.",
		//shortDesc = "Waits 2 turns; deals double the damage taken.",
		id = Moves.BIDE,
		//name = "Bide",
		pp = 10,
		priority = 1,
		flags = new Flags (contact: true, protect: true),
		volatileStatus = "bide",
		ignoreImmunity = true,
		//beforeMoveCallback = function (pokemon) {
		//	if (pokemon.volatiles["bide"]) return true;
		//},
		effect = {
			duration = 3,
			//onLockMove = "bide",
			//onStart = function (pokemon) {
			//	this.effectData.totalDamage = 0;
			//	this.add('-start', pokemon, 'move = Bide');
			//},
			//onDamagePriority = -101,
			//onDamage = function (damage, target, source, move) {
			//	if (!move || move.effectType !== "Move" || !source) return;
			//	this.effectData.totalDamage += damage;
			//	this.effectData.lastDamageSource = source;
			//},
			//onBeforeMove = function (pokemon, target, move) {
			//	if (this.effectData.duration === 1) {
			//		this.add('-end', pokemon, 'move = Bide');
			//		target = this.effectData.lastDamageSource;
			//		if (!target || !this.effectData.totalDamage) {
			//			this.attrLastMove('[still]');
			//			this.add('-fail', pokemon);
			//			return false;
			//		}
			//		if (!target.IsActive) target = this.resolveTarget(Pokemon, this.getMove("pound"));
			//		if (!this.isAdjacent(pokemon, target)) {
			//			this.add('-miss', pokemon, target);
			//			return false;
			//		}
			//		let moveData = {
			//			id = "bide",
			//			//name = "Bide",
			//			//accuracy = true,
			//			damage = this.effectData.totalDamage * 2,
			//			category = Category.PHYSICAL,
			//			priority = 1,
			//			flags = new Flags (contact: true, protect: true),
			//			effectType = "Move",
			//			type = "Normal"
			//		};
			//		// @ts-ignore
			//		this.tryMoveHit(target, pokemon, moveData);
			//		return false;
			//	}
			//	this.add('-activate', pokemon, 'move = Bide');
			//},
			//onMoveAborted = function (pokemon) {
			//	pokemon.removeVolatile("bide");
			//},
			//onEnd = function (pokemon) {
			//	this.add('-end', pokemon, 'move = Bide', '[silent]');
			//}
		},
		//secondary = false,
		target = Target.Self,
		type = Types.NORMAL,
		zMovePower = 100,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 20,
		accuracy = 85,
		basePower = 15,
		category = Category.PHYSICAL,
		//desc = "Prevents the target From switching for four or five turns; seven turns if the user is holding Grip Claw. Causes damage to the target Equal to 1/8 of its maximum HP (1/6 if the user is holding Binding Band), rounded down, at the end of each turn during effect. The target Can still switch out if it is holding Shed Shell or uses Baton Pass, Parting Shot, U-turn, or Volt Switch. The effect ends if either the user or the target Leaves the field, or if the target Uses Rapid Spin or Substitute. This effect is not stackable or reset by using this or another partial-trapping move.",
		//shortDesc = "Traps and damages the target For 4-5 turns.",
		id = Moves.BIND,
		//name = "Bind",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		volatileStatus = "partiallytrapped",
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 100,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 44,
		accuracy = 100,
		basePower = 60,
		category = Category.PHYSICAL,
		//desc = "Has a 30% chance to flinch the target.",
		//shortDesc = "30% chance to flinch the target.",
		id = Moves.BITE,
		//name = "Bite",
		pp = 25,
		priority = 0,
		flags = new Flags (bite: true, contact: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 30,
			volatileStatus = "flinch"
		},
		target = Target.Normal,
		type = Types.DARK,
		zMovePower = 120,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 654,
		//accuracy = true,
		basePower = 1,
		category = Category.PHYSICAL,
		//shortDesc = "Power is equal to the base move's Z-Power.",
		id = Moves.BLACKHOLEECLIPSE,
		isViable = true,
		//name = "Black Hole Eclipse",
		pp = 1,
		priority = 0,
		flags = new Flags (),
		isZ = "darkiniumz",
		//secondary = false,
		target = Target.Normal,
		type = Types.DARK,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 307,
		accuracy = 90,
		basePower = 150,
		category = Category.SPECIAL,
		//desc = "If this move is successful, the user must recharge on the following turn and cannot make a move.",
		//shortDesc = "User cannot move next turn.",
		id = Moves.BLASTBURN,
		//name = "Blast Burn",
		pp = 5,
		priority = 0,
		flags = new Flags (recharge: true, protect: true, mirror: true),
		self = {
			volatileStatus = "mustrecharge"
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.FIRE,
		zMovePower = 200,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 299,
		accuracy = 90,
		basePower = 85,
		category = Category.PHYSICAL,
		//desc = "Has a 10% chance to burn the target And a higher chance for a critical hit.",
		//shortDesc = "High critical hit ratio. 10% chance to burn.",
		id = Moves.BLAZEKICK,
		isViable = true,
		//name = "Blaze Kick",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		critRatio = 2,
		secondary = new Secondary() {
			chance = 10,
			status = "brn"
		},
		target = Target.Normal,
		type = Types.FIRE,
		zMovePower = 160,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 59,
		accuracy = 70,
		basePower = 110,
		category = Category.SPECIAL,
		//desc = "Has a 10% chance to freeze the target. If the weather is Hail, this move does not check accuracy.",
		//shortDesc = "10% chance to freeze foe(s). Can't miss in hail.",
		id = Moves.BLIZZARD,
		isViable = true,
		//name = "Blizzard",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//onModifyMove = function (move) {
		//	if (this.isWeather("hail")) move.//accuracy = true;
		//},
		secondary = new Secondary() {
			chance = 10,
			status = "frz"
		},
		target = Target.AllAdjacentFoes,
		type = Types.ICE,
		zMovePower = 185,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 335,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Prevents the target From switching out. The target Can still switch out if it is holding Shed Shell or uses Baton Pass, Parting Shot, U-turn, or Volt Switch. If the target Leaves the field using Baton Pass, the replacement will remain trapped. The effect ends if the user leaves the field.",
		//shortDesc = "The target Cannot switch out.",
		id = Moves.BLOCK,
		//name = "Block",
		pp = 5,
		priority = 0,
		flags = new Flags (reflectable: true, mirror: true),
		//onHit = function (target, source, move) {
		//	return target.AddVolatile("trapped", source, move, "trapper");
		//},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {def = 1},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 644,
		//accuracy = true,
		basePower = 1,
		category = Category.PHYSICAL,
		//shortDesc = "Power is equal to the base move's Z-Power.",
		id = Moves.BLOOMDOOM,
		isViable = true,
		//name = "Bloom Doom",
		pp = 1,
		priority = 0,
		flags = new Flags (),
		isZ = "grassiumz",
		//secondary = false,
		target = Target.Normal,
		type = Types.GRASS,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 551,
		accuracy = 85,
		basePower = 130,
		category = Category.SPECIAL,
		//desc = "Has a 20% chance to burn the target.",
		//shortDesc = "20% chance to burn the target.",
		id = Moves.BLUEFLARE,
		isViable = true,
		//name = "Blue Flare",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 20,
			status = "brn"
		},
		target = Target.Normal,
		type = Types.FIRE,
		zMovePower = 195,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 34,
		accuracy = 100,
		basePower = 85,
		category = Category.PHYSICAL,
		//desc = "Has a 30% chance to paralyze the target. Damage doubles and no accuracy check is done if the target Has used Minimize while active.",
		//shortDesc = "30% chance to paralyze the target.",
		id = Moves.BODYSLAM,
		isViable = true,
		//name = "Body Slam",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true, nonsky: true),
		secondary = new Secondary() {
			chance = 30,
			status = "par"
		},
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 160,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 550,
		accuracy = 85,
		basePower = 130,
		category = Category.PHYSICAL,
		//desc = "Has a 20% chance to paralyze the target.",
		//shortDesc = "20% chance to paralyze the target.",
		id = Moves.BOLTSTRIKE,
		isViable = true,
		//name = "Bolt Strike",
		pp = 5,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 20,
			status = "par"
		},
		target = Target.Normal,
		type = Types.ELECTRIC,
		zMovePower = 195,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 125,
		accuracy = 85,
		basePower = 65,
		category = Category.PHYSICAL,
		//desc = "Has a 10% chance to flinch the target.",
		//shortDesc = "10% chance to flinch the target.",
		id = Moves.BONECLUB,
		//name = "Bone Club",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 10,
			volatileStatus = "flinch"
		},
		target = Target.Normal,
		type = Types.GROUND,
		zMovePower = 120,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 198,
		accuracy = 90,
		basePower = 25,
		category = Category.PHYSICAL,
		//desc = "Hits two to five times. Has a 1/3 chance to hit two or three times, and a 1/6 chance to hit four or five times. If one of the hits breaks the target'S substitute, it will take damage for the remaining hits. If the user has the Ability Skill Link, this move will always hit five times.",
		//shortDesc = "Hits 2-5 times in one turn.",
		id = Moves.BONERUSH,
		//name = "Bone Rush",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		multihit = new int[] {2, 5},
		//secondary = false,
		target = Target.Normal,
		type = Types.GROUND,
		zMovePower = 140,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 155,
		accuracy = 90,
		basePower = 50,
		category = Category.PHYSICAL,
		//desc = "Hits twice. If the first hit breaks the target'S substitute, it will take damage for the second hit.",
		//shortDesc = "Hits 2 times in one turn.",
		id = Moves.BONEMERANG,
		isViable = true,
		//name = "Bonemerang",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		multihit = 2,
		//secondary = false,
		target = Target.Normal,
		type = Types.GROUND,
		zMovePower = 100,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 586,
		accuracy = 100,
		basePower = 140,
		category = Category.SPECIAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect. Hits adjacent Pokemon.",
		id = Moves.BOOMBURST,
		isViable = true,
		//name = "Boomburst",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, sound: true, authentic: true),
		//secondary = false,
		target = Target.AllAdjacent,
		type = Types.NORMAL,
		zMovePower = 200,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 340,
		accuracy = 85,
		basePower = 85,
		category = Category.PHYSICAL,
		//desc = "Has a 30% chance to paralyze the target. This attack charges on the first turn and executes on the second. On the first turn, the user avoids all attacks other than Gust, Hurricane, Sky Uppercut, Smack Down, Thousand Arrows, Thunder, and Twister. If the user is holding a Power Herb, the move completes in one turn.",
		//shortDesc = "Bounces turn 1. Hits turn 2. 30% paralyze.",
		id = Moves.BOUNCE,
		//name = "Bounce",
		pp = 5,
		priority = 0,
		flags = new Flags (contact: true, charge: true, protect: true, mirror: true, gravity: true, distance: true),
		//onTry = function (attacker, defender, move) {
		//	if (attacker.removeVolatile(move.id)) {
		//		return;
		//	}
		//	this.add('-prepare', attacker, move.name, defender);
		//	if (!this.runEvent("ChargeMove", attacker, defender, move)) {
		//		this.add('-anim', attacker, move.name, defender);
		//		return;
		//	}
		//	attacker.addVolatile("twoturnmove", defender);
		//	return null;
		//},
		effect = {
			duration = 2,
			//onTryImmunity = function (target, source, move) {
			//	if (move.id === "gust" || move.id === "twister") {
			//		return;
			//	}
			//	if (move.id === "skyuppercut" || move.id === "thunder" || move.id === "hurricane" || move.id === "smackdown" || move.id === "thousandarrows" || move.id === "helpinghand") {
			//		return;
			//	}
			//	if (source.hasAbility("noguard") || target.HasAbility("noguard")) {
			//		return;
			//	}
			//	if (source.volatiles["lockon"] && target === source.volatiles["lockon"].source) return;
			//	return false;
			//},
			//onSourceBasePower = function (basePower, target, source, move) {
			//	if (move.id === "gust" || move.id === "twister") {
			//		return this.chainModify(2);
			//	}
			//}
		},
		secondary = new Secondary() {
			chance = 30,
			status = "par"
		},
		target = Target.Any,
		type = Types.FLYING,
		zMovePower = 160,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 413,
		accuracy = 100,
		basePower = 120,
		category = Category.PHYSICAL,
		//desc = "If the target Lost HP, the user takes recoil damage equal to 33% the HP lost by the target, rounded half up, but not less than 1 HP.",
		//shortDesc = "Has 33% recoil.",
		id = Moves.BRAVEBIRD,
		isViable = true,
		//name = "Brave Bird",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true, distance: true),
		recoil = new int[] {33, 100},
		//secondary = false,
		target = Target.Any,
		type = Types.FLYING,
		zMovePower = 190,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 622,
		//accuracy = true,
		basePower = 1,
		category = Category.PHYSICAL,
		//shortDesc = "Power is equal to the base move's Z-Power.",
		id = Moves.BREAKNECKBLITZ,
		isViable = true,
		//name = "Breakneck Blitz",
		pp = 1,
		priority = 0,
		flags = new Flags (),
		isZ = "normaliumz",
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 280,
		accuracy = 100,
		basePower = 75,
		category = Category.PHYSICAL,
		//desc = "If this attack does not miss, the effects of Reflect, Light Screen, and Aurora Veil end for the target'S side of the field before damage is calculated.",
		//shortDesc = "Destroys screens, unless the target Is immune.",
		id = Moves.BRICKBREAK,
		isViable = true,
		//name = "Brick Break",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//onTryHit = function (pokemon) {
		//	// will shatter screens through sub, before you hit
		//	if (pokemon.runImmunity("Fighting")) {
		//		pokemon.side.removeSiDECONDITION("reflect");
		//		pokemon.side.removeSiDECONDITION("lightscreen");
		//		pokemon.side.removeSiDECONDITION("auroraveil");
		//	}
		//},
		//secondary = false,
		target = Target.Normal,
		type = Types.FIGHTING,
		zMovePower = 140,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 362,
		accuracy = 100,
		basePower = 65,
		category = Category.SPECIAL,
		//desc = "Power doubles if the target Has less than or equal to half of its maximum HP remaining.",
		//shortDesc = "Power doubles if the target'S HP is 50% or less.",
		id = Moves.BRINE,
		//name = "Brine",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		onBasePowerPriority = 4,
		//onBasePower = function (basePower, pokemon, target) {
		//	if (target.Hp * 2 <= target.Maxhp) {
		//		return this.chainModify(2);
		//	}
		//},
		//secondary = false,
		target = Target.Normal,
		type = Types.WATER,
		zMovePower = 120,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 693,
		accuracy = 100,
		basePower = 60,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect. Hits adjacent Pokemon.",
		id = Moves.BRUTALSWING,
		//name = "Brutal Swing",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.AllAdjacent,
		type = Types.DARK,
		zMovePower = 120,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 145,
		accuracy = 100,
		basePower = 40,
		category = Category.SPECIAL,
		//desc = "Has a 10% chance to lower the target'S Speed by 1 stage.",
		//shortDesc = "10% chance to lower the foe(s) Speed by 1.",
		id = Moves.BUBBLE,
		//name = "Bubble",
		pp = 30,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 10,
			boosts = new Boosts() {
				spe = -1
			}
		},
		target = Target.AllAdjacentFoes,
		type = Types.WATER,
		zMovePower = 100,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 61,
		accuracy = 100,
		basePower = 65,
		category = Category.SPECIAL,
		//desc = "Has a 10% chance to lower the target'S Speed by 1 stage.",
		//shortDesc = "10% chance to lower the target'S Speed by 1.",
		id = Moves.BUBBLEBEAM,
		//name = "Bubble Beam",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 10,
			boosts = new Boosts() {
				spe = -1
			}
		},
		target = Target.Normal,
		type = Types.WATER,
		zMovePower = 120,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 450,
		accuracy = 100,
		basePower = 60,
		category = Category.PHYSICAL,
		//desc = "If this move is successful and the user has not fainted, it steals the target'S held Berry if it is holding one and eats it immediately. Items lost to this move cannot be regained with Recycle or the Ability Harvest.",
		//shortDesc = "User steals and eats the target'S Berry.",
		id = Moves.BUGBITE,
		//name = "Bug Bite",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//onHit = function (target, source) {
		//	let item = target.GetItem();
		//	if (source.hp && item.isBerry && target.TakeItem(source)) {
		//		this.add('-enditem', target, item.name, '[from] stealeat', '[move] Bug Bite', '[of] ' + source);
		//		if (this.singleEvent("Eat", item, null, source, null, null)) {
		//			this.runEvent("EatItem", source, null, null, item);
		//		}
		//		if (item.onEat) source.ateBerry = true;
		//	}
		//},
		//secondary = false,
		target = Target.Normal,
		type = Types.BUG,
		zMovePower = 120,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 405,
		accuracy = 100,
		basePower = 90,
		category = Category.SPECIAL,
		//desc = "Has a 10% chance to lower the target'S Special Defense by 1 stage.",
		//shortDesc = "10% chance to lower the target'S Sp. Def by 1.",
		id = Moves.BUGBUZZ,
		isViable = true,
		//name = "Bug Buzz",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, sound: true, authentic: true),
		secondary = new Secondary() {
			chance = 10,
			boosts = new Boosts() {
				spd = -1
			}
		},
		target = Target.Normal,
		type = Types.BUG,
		zMovePower = 175,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 339,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the user's Attack and Defense by 1 stage.",
		//shortDesc = "Raises the user's Attack and Defense by 1.",
		id = Moves.BULKUP,
		isViable = true,
		//name = "Bulk Up",
		pp = 20,
		priority = 0,
		flags = new Flags (snatch: true),
		boosts = new Boosts() {
			atk = 1,
			def = 1
		},
		//secondary = false,
		target = Target.Self,
		type = Types.FIGHTING,
		zMoveBoost = new Boosts() {atk = 1},
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 523,
		accuracy = 100,
		basePower = 60,
		category = Category.PHYSICAL,
		//desc = "Has a 100% chance to lower the target'S Speed by 1 stage.",
		//shortDesc = "100% chance to lower adjacent Pkmn Speed by 1.",
		id = Moves.BULLDOZE,
		//name = "Bulldoze",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, nonsky: true),
		secondary = new Secondary() {
			chance = 100,
			boosts = new Boosts() {
				spe = -1
			}
		},
		target = Target.AllAdjacent,
		type = Types.GROUND,
		zMovePower = 120,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 418,
		accuracy = 100,
		basePower = 40,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "Usually goes first.",
		id = Moves.BULLETPUNCH,
		isViable = true,
		//name = "Bullet Punch",
		pp = 30,
		priority = 1,
		flags = new Flags (contact: true, protect: true, mirror: true, punch: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.STEEL,
		zMovePower = 100,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 331,
		accuracy = 100,
		basePower = 25,
		category = Category.PHYSICAL,
		//desc = "Hits two to five times. Has a 1/3 chance to hit two or three times, and a 1/6 chance to hit four or five times. If one of the hits breaks the target'S substitute, it will take damage for the remaining hits. If the user has the Ability Skill Link, this move will always hit five times.",
		//shortDesc = "Hits 2-5 times in one turn.",
		id = Moves.BULLETSEED,
		isViable = true,
		//name = "Bullet Seed",
		pp = 30,
		priority = 0,
		flags = new Flags (bullet: true, protect: true, mirror: true),
		multihit = new int[] {2, 5},
		//secondary = false,
		target = Target.Normal,
		type = Types.GRASS,
		zMovePower = 140,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 682,
		accuracy = 100,
		basePower = 130,
		category = Category.SPECIAL,
		//desc = "Fails unless the user is a Fire type. If this move is successful, the user's Fire type becomes typeless as long as it remains active.",
		//shortDesc = "User's Fire type becomes typeless; must be Fire.",
		id = Moves.BURNUP,
		//name = "Burn Up",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, defrost: true),
		//onTryMove = function (pokemon, target, move) {
		//	if (pokemon.hasType("Fire")) return;
		//	this.add('-fail', pokemon, 'move = Burn Up');
		//	return null;
		//},
		//self = {
		//	onHit = function (pokemon) {
		//		pokemon.setType(pokemon.getTypes(true).map(type => type === "Fire" ? "???"  = type));
		//		this.add('-start', pokemon, "typechange", pokemon.types.JOIN('/'), '[from] move = Burn Up');
		//	}
		//},
		//secondary = false,
		target = Target.Normal,
		type = Types.FIRE,
		zMovePower = 195,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 347,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the user's Special Attack and Special Defense by 1 stage.",
		//shortDesc = "Raises the user's Sp. Atk and Sp. Def by 1.",
		id = Moves.CALMMIND,
		isViable = true,
		//name = "Calm Mind",
		pp = 20,
		priority = 0,
		flags = new Flags (snatch: true),
		boosts = new Boosts() {
			spa = 1,
			spd = 1
		},
		//secondary = false,
		target = Target.Self,
		type = Types.PSYCHIC,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 293,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user's type changes based on the battle terrain. Normal type on the regular Wi-Fi terrain, Electric type during Electric Terrain, Fairy type during Misty Terrain, Grass type during Grassy Terrain, and Psychic type during Psychic Terrain. Fails if the user's type cannot be changed or if the user is already purely that type.",
		//shortDesc = "Changes user's type by terrain (default Normal).",
		id = Moves.CAMOUFLAGE,
		//name = "Camouflage",
		pp = 20,
		priority = 0,
		flags = new Flags (snatch: true),
		//onHit = function (target) {
		//	let newType = "Normal";
		//	if (this.isTerrain("electricterrain")) {
		//		newType = "Electric";
		//	} else if (this.isTerrain("grassyterrain")) {
		//		newType = "Grass";
		//	} else if (this.isTerrain("mistyterrain")) {
		//		newType = "Fairy";
		//	} else if (this.isTerrain("psychicterrain")) {
		//		newType = "Psychic";
		//	}

		//	if (!target.SetType(newType)) return false;
		//	this.add('-start', target, "typechange", newType);
		//},
		//secondary = false,
		target = Target.Self,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {evasion = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 445,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Lowers the target'S Special Attack by 2 stages. The target Is unaffected if both the user and the target Are the same gender, or if either is genderless. Pokemon with the Ability Oblivious are immune.",
		//shortDesc = "Lowers the foe(s) Sp. Atk by 2 if opposite gender.",
		id = Moves.CAPTIVATE,
		//name = "Captivate",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true),
		//onTryHit = function (pokemon, source) {
		//	if ((pokemon.gender === "M" && source.gender === "F") || (pokemon.gender === "F" && source.gender === "M")) {
		//		return;
		//	}
		//	return false;
		//},
		boosts = new Boosts() {
			spa = -2
		},
		//secondary = false,
		target = Target.AllAdjacentFoes,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {spd = 2},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 658,
		//accuracy = true,
		basePower = 210,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect.",
		id = Moves.CATASTROPIKA,
		//name = "Catastropika",
		pp = 1,
		priority = 0,
		flags = new Flags (contact: true),
		isZ = "pikaniumz",
		//secondary = false,
		target = Target.Normal,
		type = Types.ELECTRIC,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 606,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "It is your birthday.",
		//shortDesc = "No competitive use. Or any use.",
		id = Moves.CELEBRATE,
		//name = "Celebrate",
		pp = 40,
		priority = 0,
		flags = new Flags (),
		//onTryHit = function (target, source) {
		//	this.add('-activate', target, 'move = Celebrate');
		//},
		//secondary = false,
		target = Target.Self,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {atk = 1, def = 1, spa = 1, spd = 1, spe = 1},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 268,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the user's Special Defense by 1 stage. If the user uses an Electric-type attack on the next turn, its power will be doubled.",
		//shortDesc = "Boosts next Electric move and user's Sp. Def by 1.",
		id = Moves.CHARGE,
		//name = "Charge",
		pp = 20,
		priority = 0,
		flags = new Flags (snatch: true),
		volatileStatus = "charge",
		//onHit = function (pokemon) {
		//	this.add('-activate', pokemon, 'move = Charge');
		//},
		effect = {
			duration = 2,
			//onRestart = function (pokemon) {
			//	this.effectData.duration = 2;
			//},
			//onBasePowerPriority = 3,
			//onBasePower = function (basePower, attacker, defender, move) {
			//	if (move.type === "Electric") {
			//		this.debug('charge boost');
			//		return this.chainModify(2);
			//	}
			//}
		},
		boosts = new Boosts() {
			spd = 1
		},
		//secondary = false,
		target = Target.Self,
		type = Types.ELECTRIC,
		zMoveBoost = new Boosts() {spd = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 451,
		accuracy = 90,
		basePower = 50,
		category = Category.SPECIAL,
		//desc = "Has a 70% chance to raise the user's Special Attack by 1 stage.",
		//shortDesc = "70% chance to raise the user's Sp. Atk by 1.",
		id = Moves.CHARGEBEAM,
		//name = "Charge Beam",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 70,
			self = {
				boosts = new Boosts() {
					spa = 1
				}
			}
		},
		target = Target.Normal,
		type = Types.ELECTRIC,
		zMovePower = 100,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 204,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Lowers the target'S Attack by 2 stages.",
		//shortDesc = "Lowers the target'S Attack by 2.",
		id = Moves.CHARM,
		//name = "Charm",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true, mystery: true),
		boosts = new Boosts() {
			atk = -2
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.FAIRY,
		zMoveBoost = new Boosts() {def = 1},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 448,
		accuracy = 100,
		basePower = 65,
		category = Category.SPECIAL,
		//desc = "Has a 100% chance to confuse the target.",
		//shortDesc = "100% chance to confuse the target.",
		id = Moves.CHATTER,
		isViable = true,
		//name = "Chatter",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, sound: true, distance: true, authentic: true),
		noSketch = true,
		secondary = new Secondary() {
			chance = 100,
			volatileStatus = "confusion"
		},
		target = Target.Any,
		type = Types.FLYING,
		zMovePower = 120,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 498,
		accuracy = 100,
		basePower = 70,
		category = Category.PHYSICAL,
		//desc = "Ignores the target'S stat stage changes, including evasiveness.",
		//shortDesc = "Ignores the target'S stat stage changes.",
		id = Moves.CHIPAWAY,
		//name = "Chip Away",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		ignoreDefensive = true,
		ignoreEvasion = true,
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 140,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 509,
		accuracy = 90,
		basePower = 60,
		category = Category.PHYSICAL,
		//desc = "If both the user and the target Have not fainted, the target Is forced to switch out and be replaced with a random unfainted ally. This effect fails if the target Used Ingrain previously, has the Ability Suction Cups, or this move hit a substitute.",
		//shortDesc = "Forces the target To switch to a random ally.",
		id = Moves.CIRCLETHROW,
		isViable = true,
		//name = "Circle Throw",
		pp = 10,
		priority = -6,
		flags = new Flags (contact: true, protect: true, mirror: true),
		forceSwitch = true,
		target = Target.Normal,
		type = Types.FIGHTING,
		zMovePower = 120,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 128,
		accuracy = 85,
		basePower = 35,
		category = Category.PHYSICAL,
		//desc = "Prevents the target From switching for four or five turns; seven turns if the user is holding Grip Claw. Causes damage to the target Equal to 1/8 of its maximum HP (1/6 if the user is holding Binding Band), rounded down, at the end of each turn during effect. The target Can still switch out if it is holding Shed Shell or uses Baton Pass, Parting Shot, U-turn, or Volt Switch. The effect ends if either the user or the target Leaves the field, or if the target Uses Rapid Spin or Substitute. This effect is not stackable or reset by using this or another partial-trapping move.",
		//shortDesc = "Traps and damages the target For 4-5 turns.",
		id = Moves.CLAMP,
		//name = "Clamp",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		volatileStatus = "partiallytrapped",
		//secondary = false,
		target = Target.Normal,
		type = Types.WATER,
		zMovePower = 100,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 691,
		accuracy = 100,
		basePower = 110,
		category = Category.SPECIAL,
		//desc = "Lowers the user's Defense by 1 stage.",
		//shortDesc = "Lowers the user's Defense by 1.",
		id = Moves.CLANGINGSCALES,
		isViable = true,
		//name = "Clanging Scales",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, sound: true, authentic: true),
		selfBoost = {
			boosts = new Boosts() {
				def = -1
			}
		},
		//secondary = false,
		target = Target.AllAdjacentFoes,
		type = Types.DRAGON,
		zMovePower = 185,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 728,
		//accuracy = true,
		basePower = 185,
		category = Category.SPECIAL,
		//desc = "Raises the user's Attack, Defense, Special Attack, Special Defense, and Speed by 1 stage.",
		//shortDesc = "Raises the user's Atk/Def/SpAtk/SpDef/Spe by 1.",
		id = Moves.CLANGOROUSSOULBLAZE,
		//name = "Clangorous Soulblaze",
		pp = 1,
		priority = 0,
		flags = new Flags (sound: true, authentic: true),
		selfBoost = {
			boosts = new Boosts() {
				atk = 1,
				def = 1,
				spa = 1,
				spd = 1,
				spe = 1
			}
		},
		isZ = "kommoniumz",
		secondary = new Secondary() {
			// Sheer Force negates the selfBoost even though it is not secondary
		},
		target = Target.AllAdjacentFoes,
		type = Types.DRAGON,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 499,
		//accuracy = true,
		basePower = 50,
		category = Category.SPECIAL,
		//desc = "Resets all of the target'S stat stages to 0.",
		//shortDesc = "Eliminates the target'S stat changes.",
		id = Moves.CLEARSMOG,
		isViable = true,
		//name = "Clear Smog",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//onHit = function (target) {
		//	target.ClearBoosts();
		//	this.add('-clearboost', target);
		//},
		//secondary = false,
		target = Target.Normal,
		type = Types.POISON,
		zMovePower = 100,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 370,
		accuracy = 100,
		basePower = 120,
		category = Category.PHYSICAL,
		//desc = "Lowers the user's Defense and Special Defense by 1 stage.",
		//shortDesc = "Lowers the user's Defense and Sp. Def by 1.",
		id = Moves.CLOSECOMBAT,
		isViable = true,
		//name = "Close Combat",
		pp = 5,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		self = {
			boosts = new Boosts() {
				def = -1,
				spd = -1
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.FIGHTING,
		zMovePower = 190,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 489,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the user's Attack, Defense, and accuracy by 1 stage.",
		//shortDesc = "Raises user's Attack, Defense, and accuracy by 1.",
		id = Moves.COIL,
		isViable = true,
		//name = "Coil",
		pp = 20,
		priority = 0,
		flags = new Flags (snatch: true),
		boosts = new Boosts() {
			atk = 1,
			def = 1,
			accuracy = 1
		},
		//secondary = false,
		target = Target.Self,
		type = Types.POISON,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 4,
		accuracy = 85,
		basePower = 18,
		category = Category.PHYSICAL,
		//desc = "Hits two to five times. Has a 1/3 chance to hit two or three times, and a 1/6 chance to hit four or five times. If one of the hits breaks the target'S substitute, it will take damage for the remaining hits. If the user has the Ability Skill Link, this move will always hit five times.",
		//shortDesc = "Hits 2-5 times in one turn.",
		id = Moves.COMETPUNCH,
		//name = "Comet Punch",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true, punch: true),
		multihit = new int[] {2, 5},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 100,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 590,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Lowers the target'S Special Attack by 1 stage.",
		//shortDesc = "Lowers the target'S Sp. Atk by 1.",
		id = Moves.CONFIDE,
		//name = "Confide",
		pp = 20,
		priority = 0,
		flags = new Flags (reflectable: true, mirror: true, sound: true, authentic: true),
		boosts = new Boosts() {
			spa = -1
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {spd = 1},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 109,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Causes the target To become confused.",
		//shortDesc = "Confuses the target.",
		id = Moves.CONFUSERAY,
		//name = "Confuse Ray",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true),
		volatileStatus = "confusion",
		//secondary = false,
		target = Target.Normal,
		type = Types.GHOST,
		zMoveBoost = new Boosts() {spa = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 93,
		accuracy = 100,
		basePower = 50,
		category = Category.SPECIAL,
		//desc = "Has a 10% chance to confuse the target.",
		//shortDesc = "10% chance to confuse the target.",
		id = Moves.CONFUSION,
		//name = "Confusion",
		pp = 25,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 10,
			volatileStatus = "confusion"
		},
		target = Target.Normal,
		type = Types.PSYCHIC,
		zMovePower = 100,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 132,
		accuracy = 100,
		basePower = 10,
		category = Category.PHYSICAL,
		//desc = "Has a 10% chance to lower the target'S Speed by 1 stage.",
		//shortDesc = "10% chance to lower the target'S Speed by 1.",
		id = Moves.CONSTRICT,
		//name = "Constrict",
		pp = 35,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 10,
			boosts = new Boosts() {
				spe = -1
			}
		},
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 100,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 632,
		//accuracy = true,
		basePower = 1,
		category = Category.PHYSICAL,
		//shortDesc = "Power is equal to the base move's Z-Power.",
		id = Moves.CONTINENTALCRUSH,
		isViable = true,
		//name = "Continental Crush",
		pp = 1,
		priority = 0,
		flags = new Flags (),
		isZ = "rockiumz",
		//secondary = false,
		target = Target.Normal,
		type = Types.ROCK,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 160,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user's type changes to match the original type of the move in its first move slot. Fails if the user cannot change its type, or if the type is one of the user's current types.",
		//shortDesc = "Changes user's type to match its first move.",
		id = Moves.CONVERSION,
		//name = "Conversion",
		pp = 30,
		priority = 0,
		flags = new Flags (snatch: true),
		//onHit = function (target) {
		//	let type = this.getMove(target.MoveSlOTS[0].id).type;
		//	if (target.HasType(type) || !target.SetType(type)) return false;
		//	this.add('-start', target, "typechange", type);
		//},
		//secondary = false,
		target = Target.Self,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {atk = 1, def = 1, spa = 1, spd = 1, spe = 1},
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 176,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user's type changes to match a type that resists or is immune to the type of the last move used by the target, but not either of its current types. The determined type of the move is used rather than the original type. Fails if the target Has not made a move, if the user cannot change its type, or if this move would only be able to select one of the user's current types.",
		//shortDesc = "Changes user's type to resist target'S last move.",
		id = Moves.CONVERSION2,
		//name = "Conversion 2",
		pp = 30,
		priority = 0,
		flags = new Flags (authentic: true),
		//onHit = function (target, source) {
		//	if (!target.LastMove) {
		//		return false;
		//	}
		//	let possibleTypes = [];
		//	let attackType = target.LastMove.type;
		//	for (let type in this.data.TypeChart) {
		//		if (source.hasType(type) || target.HasType(type)) continue;
		//		let typeCheck = this.data.TypeChart[type].damageTaken[attackType];
		//		if (typeCheck === 2 || typeCheck === 3) {
		//			possibleTypes.PUSH(type);
		//		}
		//	}
		//	if (!possibleTypes.LENGTH) {
		//		return false;
		//	}
		//	let randomType = this.sample(possibleTypes);

		//	if (!source.setType(randomType)) return false;
		//	this.add('-start', source, "typechange", randomType);
		//},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMoveEffect = "heal",
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 383,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user uses the last move used by any Pokemon, including itself. Fails if no move has been used, or if the last move used was Assist, Baneful Bunker, Belch, Bestow, Chatter, Circle Throw, Copycat, Counter, Covet, Destiny Bond, Detect, Dragon Tail, Endure, Feint, Focus Punch, Follow Me, Helping Hand, Hold Hands, King's Shield, Mat Block, Me First, Metronome, Mimic, Mirror Coat, Mirror Move, Nature Power, Protect, Rage Powder, Roar, Sketch, Sleep Talk, Snatch, Spiky Shield, Struggle, Switcheroo, Thief, Transform, Trick, or Whirlwind.",
		//shortDesc = "Uses the last move used in the battle.",
		id = Moves.COPYCAT,
		//name = "Copycat",
		pp = 20,
		priority = 0,
		flags = new Flags (),
		//onHit = function (pokemon) {
		//	let noCopycat = ["assist", "banefulbunker", "bestow", "chatter", "circlethrow", "copycat", "counter", "covet", "destinybond", "detect", "dragontail", "endure", "feint", "focuspunch", "followme", "helpinghand", "mefirst", "metronome", "mimic", "mirrorcoat", "mirrormove", "naturepower", "protect", "ragepowder", "roar", "sketch", "sleeptalk", "snatch", "struggle", "switcheroo", "thief", "transform", "trick", "whirlwind"];
		//	if (!this.lastMove || noCopycat.includes(this.lastMove.id) || this.lastMove.isZ) {
		//		return false;
		//	}
		//	this.useMove(this.lastMove.id, pokemon);
		//},
		//secondary = false,
		target = Target.Self,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {accuracy = 1},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 687,
		accuracy = 100,
		basePower = 100,
		category = Category.SPECIAL,
		//desc = "If the user moves AFTER the target, the target'S Ability is rendered ineffective as long as it remains active. If the target Uses Baton Pass, the replacement will remain under this effect. If the target'S Ability is Multitype or Stance Change, this effect does not happen.",
		//shortDesc = "Nullifies the foe(s) Ability if the target Moves FIRST.",
		id = Moves.COREENFORCER,
		isViable = true,
		//name = "Core Enforcer",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//onHit = function (target) {
		//	if (["battlebond", "comatose", "disguise", "multitype", "powerconstruct", "rkssystem", "schooling", "shieldsdown", "stancechange"].includes(target.Ability)) return;
		//	if (target.NewlySwitched || this.willMove(target)) return;
		//	target.AddVolatile("gastroacid");
		//},
		//onAfterSubDamage = function (target) {
		//	if (["battlebond", "comatose", "disguise", "multitype", "powerconstruct", "rkssystem", "schooling", "shieldsdown", "stancechange"].includes(target.Ability)) return;
		//	if (target.NewlySwitched || this.willMove(target)) return;
		//	target.AddVolatile("gastroacid");
		//},
		//secondary = false,
		target = Target.AllAdjacentFoes,
		type = Types.DRAGON,
		zMovePower = 140,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 638,
		//accuracy = true,
		basePower = 1,
		category = Category.PHYSICAL,
		//shortDesc = "Power is equal to the base move's Z-Power.",
		id = Moves.CORKSCREWCRASH,
		isViable = true,
		//name = "Corkscrew Crash",
		pp = 1,
		priority = 0,
		flags = new Flags (),
		isZ = "steeliumz",
		//secondary = false,
		target = Target.Normal,
		type = Types.STEEL,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 322,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the user's Defense and Special Defense by 1 stage.",
		//shortDesc = "Raises the user's Defense and Sp. Def by 1.",
		id = Moves.COSMICPOWER,
		//name = "Cosmic Power",
		pp = 20,
		priority = 0,
		flags = new Flags (snatch: true),
		boosts = new Boosts() {
			def = 1,
			spd = 1
		},
		//secondary = false,
		target = Target.Self,
		type = Types.PSYCHIC,
		zMoveBoost = new Boosts() {spd = 1},
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 538,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the user's Defense by 3 stages.",
		//shortDesc = "Raises the user's Defense by 3.",
		id = Moves.COTTONGUARD,
		isViable = true,
		//name = "Cotton Guard",
		pp = 10,
		priority = 0,
		flags = new Flags (snatch: true),
		boosts = new Boosts() {
			def = 3
		},
		//secondary = false,
		target = Target.Self,
		type = Types.GRASS,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 178,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Lowers the target'S Speed by 2 stages.",
		//shortDesc = "Lowers the target'S Speed by 2.",
		id = Moves.COTTONSPORE,
		//name = "Cotton Spore",
		pp = 40,
		priority = 0,
		flags = new Flags (powder: true, protect: true, reflectable: true, mirror: true),
		boosts = new Boosts() {
			spe = -2
		},
		//secondary = false,
		target = Target.AllAdjacentFoes,
		type = Types.GRASS,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 68,
		accuracy = 100,
		basePower = 0,
		//damageCallback = function (pokemon) {
		//	if (!pokemon.volatiles["counter"]) return 0;
		//	return pokemon.volatiles["counter"].damage || 1;
		//},
		category = Category.PHYSICAL,
		//desc = "Deals damage to the last foe to hit the user with a physical attack this turn equal to twice the HP lost by the user from that attack. If the user did not lose HP from the attack, this move deals damage with a Base Power of 1 instead. If that foe's position is no longer in use, the damage is done to a random foe in range. Only the last hit of a multi-hit attack is counted. Fails if the user was not hit by a foe's physical attack this turn.",
		//shortDesc = "If hit by physical attack, returns double damage.",
		id = Moves.COUNTER,
		//name = "Counter",
		pp = 20,
		priority = -5,
		flags = new Flags (contact: true, protect: true),
		//beforeTurnCallback = function (pokemon) {
		//	pokemon.addVolatile("counter");
		//},
		//onTryHit = function (target, source, move) {
		//	if (!source.volatiles["counter"]) return false;
		//	if (source.volatiles["counter"].position === null) return false;
		//},
		effect = {
			duration = 1,
			noCopy = true,
			//onStart = function (target, source, source2, move) {
			//	this.effectData.position = null;
			//	this.effectData.damage = 0;
			//},
			//onRedirectTargetPRiority = -1,
			//onRedirectTarget = function (target, source, source2) {
			//	if (source !== this.effectData.target) return;
			//	return source.side.foe.active[this.effectData.position];
			//},
			//onAfterDamage = function (damage, target, source, effect) {
			//	// @ts-ignore
			//	if (effect && effect.effectType === "Move" && source.side !== target.Side && this.getCategory(effect) === "Physical") {
			//		this.effectData.position = source.position;
			//		this.effectData.damage = 2 * damage;
			//	}
			//}
		},
		//secondary = false,
		target = Target.Scripted,
		type = Types.FIGHTING,
		zMovePower = 100,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 343,
		accuracy = 100,
		basePower = 60,
		category = Category.PHYSICAL,
		//desc = "If this attack was successful and the user has not fainted, it steals the target'S held item if the user is not holding one. The target'S item is not stolen if it is a Mail or Z-Crystal, or if the target Is a Kyogre holding a Blue Orb, a Groudon holding a Red Orb, a Giratina holding a Griseous Orb, an Arceus holding a Plate, a Genesect holding a Drive, a Silvally holding a Memory, or a Pokemon that can Mega Evolve holding the Mega Stone for its species. Items lost to this move cannot be regained with Recycle or the Ability Harvest.",
		//shortDesc = "If the user has no item, it steals the target'S.",
		id = Moves.COVET,
		//name = "Covet",
		pp = 25,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//onAfterHit = function (target, source, move) {
		//	if (source.item || source.volatiles["gem"]) {
		//		return;
		//	}
		//	let yourItem = target.TakeItem(source);
		//	if (!yourItem) {
		//		return;
		//	}
		//	if (!this.singleEvent("TakeItem", yourItem, target.ItemData, source, target, move, yourItem) || !source.setItem(yourItem)) {
		//		target.Item = yourItem.id; // bypass setItem so we don't break choicelock or anything
		//		return;
		//	}
		//	this.add('-item', source, yourItem, '[from] move = Covet', '[of] ' + target);
		//},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 120,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 152,
		accuracy = 90,
		basePower = 100,
		category = Category.PHYSICAL,
		//desc = "Has a higher chance for a critical hit.",
		//shortDesc = "High critical hit ratio.",
		id = Moves.CRABHAMMER,
		isViable = true,
		//name = "Crabhammer",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		critRatio = 2,
		//secondary = false,
		target = Target.Normal,
		type = Types.WATER,
		zMovePower = 180,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 578,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user and its party members are protected from non-damaging attacks made by other Pokemon, including allies, during this turn. Fails if the user moves LAST this turn or if this move is already in effect for the user's side.",
		//shortDesc = "Protects allies from Status moves THIS turn.",
		id = Moves.CRAFTYSHIELD,
		//name = "Crafty Shield",
		pp = 10,
		priority = 3,
		flags = new Flags (),
		sideCondition = "craftyshield",
		//onTryHitSide = function (side, source) {
		//	return !!this.willAct();
		//},
		effect = {
			duration = 1,
			//onStart = function (target, source) {
			//	this.add('-singleturn', source, 'Crafty Shield');
			//},
			//onTryHitPriority = 3,
			//onTryHit = function (target, source, move) {
			//	if (move && (move.target === "self" || move.category !== "Status")) return;
			//	this.add('-activate', target, 'move = Crafty Shield');
			//	source.moveThisTurnResult = true;
			//	return null;
			//}
		},
		//secondary = false,
		target = Target.AllySide,
		type = Types.FAIRY,
		zMoveBoost = new Boosts() {spd = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 238,
		accuracy = 80,
		basePower = 100,
		category = Category.PHYSICAL,
		//desc = "Has a higher chance for a critical hit.",
		//shortDesc = "High critical hit ratio.",
		id = Moves.CROSSCHOP,
		isViable = true,
		//name = "Cross Chop",
		pp = 5,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		critRatio = 2,
		//secondary = false,
		target = Target.Normal,
		type = Types.FIGHTING,
		zMovePower = 180,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 440,
		accuracy = 100,
		basePower = 70,
		category = Category.PHYSICAL,
		//desc = "Has a 10% chance to poison the target And a higher chance for a critical hit.",
		//shortDesc = "High critical hit ratio. 10% chance to poison.",
		id = Moves.CROSSPOISON,
		//name = "Cross Poison",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 10,
			status = "psn"
		},
		critRatio = 2,
		target = Target.Normal,
		type = Types.POISON,
		zMovePower = 140,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 242,
		accuracy = 100,
		basePower = 80,
		category = Category.PHYSICAL,
		//desc = "Has a 20% chance to lower the target'S Defense by 1 stage.",
		//shortDesc = "20% chance to lower the target'S Defense by 1.",
		id = Moves.CRUNCH,
		isViable = true,
		//name = "Crunch",
		pp = 15,
		priority = 0,
		flags = new Flags (bite: true, contact: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 20,
			boosts = new Boosts() {
				def = -1
			}
		},
		target = Target.Normal,
		type = Types.DARK,
		zMovePower = 160,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 306,
		accuracy = 95,
		basePower = 75,
		category = Category.PHYSICAL,
		//desc = "Has a 50% chance to lower the target'S Defense by 1 stage.",
		//shortDesc = "50% chance to lower the target'S Defense by 1.",
		id = Moves.CRUSHCLAW,
		//name = "Crush Claw",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 50,
			boosts = new Boosts() {
				def = -1
			}
		},
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 140,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 462,
		accuracy = 100,
		basePower = 0,
		//basePowerCallback = function (pokemon, target) {
		//	return Math.floor(Math.floor((120 * (100 * Math.floor(target.Hp * 4096 / target.Maxhp)) + 2048 - 1) / 4096) / 100) || 1;
		//},
		category = Category.PHYSICAL,
		//desc = "Power is equal to 120 * (target'S current HP / target'S maximum HP), rounded half down, but not less than 1.",
		//shortDesc = "More power the more HP the target Has left.",
		id = Moves.CRUSHGRIP,
		//name = "Crush Grip",
		pp = 5,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 190,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 174,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "If the user is not a Ghost type, lowers the user's Speed by 1 stage and raises the user's Attack and Defense by 1 stage. If the user is a Ghost type, the user loses 1/2 of its maximum HP, rounded down and even if it would cause fainting, in exchange for the target Losing 1/4 of its maximum HP, rounded down, at the end of each turn while it is active. If the target Uses Baton Pass, the replacement will continue to be affected. Fails if there is no target Or if the target Is already affected.",
		//shortDesc = "Curses if Ghost, else +1 Atk, +1 Def, -1 Spe.",
		id = Moves.CURSE,
		//name = "Curse",
		pp = 10,
		priority = 0,
		flags = new Flags (authentic: true),
		volatileStatus = "curse",
		//onModifyMove = function (move, source, target) {
		//	if (!source.hasType("Ghost")) {
		//		// @ts-ignore
		//		move.target = move.nonGhostTarget;
		//	}
		//},
		//onTryHit = function (target, source, move) {
		//	if (!source.hasType("Ghost")) {
		//		delete move.volatileStatus;
		//		delete move.onHit;
		//		move.self = {boosts = new Boosts() {spe = -1, atk = 1, def = 1}};
		//	} else if (move.volatileStatus && target.Volatiles.curse) {
		//		return false;
		//	}
		//},
		//onHit = function (target, source) {
		//	this.directDamage(source.maxhp / 2, source, source);
		//},
		//effect = {
		//	onStart = function (pokemon, source) {
		//		this.add('-start', pokemon, "Curse", '[of] ' + source);
		//	},
		//	onResidualOrder = 10,
		//	onResidual = function (pokemon) {
		//		this.damage(pokemon.maxhp / 4);
		//	}
		//},
		//secondary = false,
		target = Target.Normal,
		nonGhosttarget = Target.Self,
		type = Types.GHOST,
		zMoveEffect = "curse",
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 15,
		accuracy = 95,
		basePower = 50,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect.",
		id = Moves.CUT,
		//name = "Cut",
		pp = 30,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 100,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 399,
		accuracy = 100,
		basePower = 80,
		category = Category.SPECIAL,
		//desc = "Has a 20% chance to flinch the target.",
		//shortDesc = "20% chance to flinch the target.",
		id = Moves.DARKPULSE,
		isViable = true,
		//name = "Dark Pulse",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, pulse: true, mirror: true, distance: true),
		secondary = new Secondary() {
			chance = 20,
			volatileStatus = "flinch"
		},
		target = Target.Any,
		type = Types.DARK,
		zMovePower = 160,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 464,
		accuracy = 50,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Causes the target To fall asleep. This move cannot be used successfully unless the user's current form, while considering Transform, is Darkrai.",
		//shortDesc = "Darkrai = Puts the foe(s) to sleep.",
		id = Moves.DARKVOID,
		isViable = true,
		//name = "Dark Void",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true),
		status = "slp",
		//onTryMove = function (pokemon, target, move) {
		//	if (pokemon.template.species === "Darkrai" || move.hasBounced) {
		//		return;
		//	}
		//	this.add('-fail', pokemon, 'move = Dark Void');
		//	this.add('-hint', "Only a Pokemon whose form is Darkrai can use this move.");
		//	return null;
		//},
		//secondary = false,
		target = Target.AllAdjacentFoes,
		type = Types.DARK,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 663,
		accuracy = 100,
		basePower = 85,
		category = Category.PHYSICAL,
		//desc = "Ignores the target'S stat stage changes, including evasiveness.",
		//shortDesc = "Ignores the target'S stat stage changes.",
		id = Moves.DARKESTLARIAT,
		isViable = true,
		//name = "Darkest Lariat",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		ignoreEvasion = true,
		ignoreDefensive = true,
		//secondary = false,
		target = Target.Normal,
		type = Types.DARK,
		zMovePower = 160,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 605,
		accuracy = 100,
		basePower = 80,
		category = Category.SPECIAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect. Hits adjacent foes.",
		id = Moves.DAZZLINGGLEAM,
		isViable = true,
		//name = "Dazzling Gleam",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.AllAdjacentFoes,
		type = Types.FAIRY,
		zMovePower = 160,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 455,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the user's Defense and Special Defense by 1 stage.",
		//shortDesc = "Raises the user's Defense and Sp. Def by 1.",
		id = Moves.DEFENDORDER,
		isViable = true,
		//name = "Defend Order",
		pp = 10,
		priority = 0,
		flags = new Flags (snatch: true),
		boosts = new Boosts() {
			def = 1,
			spd = 1
		},
		//secondary = false,
		target = Target.Self,
		type = Types.BUG,
		zMoveBoost = new Boosts() {def = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 111,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the user's Defense by 1 stage. As long as the user remains active, the power of the user's Ice Ball and Rollout will be doubled (this effect is not stackable).",
		//shortDesc = "Raises the user's Defense by 1.",
		id = Moves.DEFENSECURL,
		//name = "Defense Curl",
		pp = 40,
		priority = 0,
		flags = new Flags (snatch: true),
		boosts = new Boosts() {
			def = 1
		},
		volatileStatus = "defensecurl",
		effect = {
			noCopy = true
		},
		//secondary = false,
		target = Target.Self,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {accuracy = 1},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 432,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Lowers the target'S evasiveness by 1 stage. If this move is successful and whether or not the target'S evasiveness was affected, the effects of Reflect, Light Screen, Aurora Veil, Safeguard, Mist, Spikes, Toxic Spikes, Stealth Rock, and Sticky Web end for the target'S side, and the effects of Spikes, Toxic Spikes, Stealth Rock, and Sticky Web end for the user's side. Ignores a target'S substitute, although a substitute will still block the lowering of evasiveness.",
		//shortDesc = "-1 evasion; clears user and target Side's hazards.",
		id = Moves.DEFOG,
		isViable = true,
		//name = "Defog",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true, authentic: true),
		//onHit = function (target, source, move) {
		//	let success = false;
		//	if (!target.Volatiles["substitute"] || move.infiltrates) success = this.boost({evasion = -1});
		//	let removeTarget = ["reflect", "lightscreen", "auroraveil", "safeguard", "mist", "spikes", "toxicspikes", "stealthrock", "stickyweb"];
		//	let removeAll = ["spikes", "toxicspikes", "stealthrock", "stickyweb"];
		//	for (const targetCOndition of removeTarget) {
		//		if (target.Side.removeSiDECONDITION(targetCOndition)) {
		//			if (!removeAll.includes(targetCOndition)) continue;
		//			this.add('-sideend', target.Side, this.getEffect(targetCOndition).name, '[from] move = Defog', '[of] ' + target);
		//			success = true;
		//		}
		//	}
		//	for (const sideCondition of removeAll) {
		//		if (source.side.removeSiDECONDITION(sideCondition)) {
		//			this.add('-sideend', source.side, this.getEffect(sideCondition).name, '[from] move = Defog', '[of] ' + source);
		//			success = true;
		//		}
		//	}
		//	return success;
		//},
		//secondary = false,
		target = Target.Normal,
		type = Types.FLYING,
		zMoveBoost = new Boosts() {accuracy = 1},
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 194,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Until the user's next turn, if an opposing Pokemon's attack knocks the user out, that Pokemon faints as well, unless the attack was Doom Desire or Future Sight. Fails if the user used this move successfully last turn.",
		//shortDesc = "If an opponent knocks out the user, it also faints.",
		id = Moves.DESTINYBOND,
		isViable = true,
		//name = "Destiny Bond",
		pp = 5,
		priority = 0,
		flags = new Flags (authentic: true),
		volatileStatus = "destinybond",
		//onPrepareHit = function (pokemon) {
		//	return !pokemon.removeVolatile("destinybond");
		//},
		//effect = {
		//	onStart = function (pokemon) {
		//		this.add('-singlemove', pokemon, 'Destiny Bond');
		//	},
		//	onFaint = function (target, source, effect) {
		//		if (!source || !effect || target.Side === source.side) return;
		//		if (effect.effectType === "Move" && !effect.isFutureMove) {
		//			this.add('-activate', target, 'move = Destiny Bond');
		//			source.faint();
		//		}
		//	},
		//	onBeforeMovePriority = -1,
		//	onBeforeMove = function (pokemon, target, move) {
		//		if (move.id === "destinybond") return;
		//		this.debug('removing Destiny Bond before attack');
		//		pokemon.removeVolatile("destinybond");
		//	},
		//	onMoveAborted = function (pokemon, target, move) {
		//		pokemon.removeVolatile("destinybond");
		//	},
		//	onBeforeSwitchOutPriority = 1,
		//	onBeforeSwitchOut = function (pokemon) {
		//		pokemon.removeVolatile("destinybond");
		//	}
		//},
		//secondary = false,
		target = Target.Self,
		type = Types.GHOST,
		zMoveEffect = "redirect",
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 197,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user is protected from most attacks made by other Pokemon during this turn. This move has a 1/X chance of being successful, where X starts at 1 and triples each time this move is successfully used. X resets to 1 if this move fails or if the user's last move used is not Baneful Bunker, Detect, Endure, King's Shield, Protect, Quick Guard, Spiky Shield, or Wide Guard. Fails if the user moves LAST this turn.",
		//shortDesc = "Prevents moves FROM affecting the user this turn.",
		id = Moves.DETECT,
		isViable = true,
		//name = "Detect",
		pp = 5,
		priority = 4,
		flags = new Flags (),
		stallingMove = true,
		volatileStatus = "protect",
		//onPrepareHit = function (pokemon) {
		//	return !!this.willAct() && this.runEvent("StallMove", pokemon);
		//},
		//onHit = function (pokemon) {
		//	pokemon.addVolatile("stall");
		//},
		//secondary = false,
		target = Target.Self,
		type = Types.FIGHTING,
		zMoveBoost = new Boosts() {evasion = 1},
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 652,
		//accuracy = true,
		basePower = 1,
		category = Category.PHYSICAL,
		//shortDesc = "Power is equal to the base move's Z-Power.",
		id = Moves.DEVASTATINGDRAKE,
		//name = "Devastating Drake",
		pp = 1,
		priority = 0,
		flags = new Flags (),
		isZ = "dragoniumz",
		//secondary = false,
		target = Target.Normal,
		type = Types.DRAGON,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 591,
		accuracy = 95,
		basePower = 100,
		category = Category.PHYSICAL,
		//desc = "Has a 50% chance to raise the user's Defense by 2 stages.",
		//shortDesc = "50% chance to raise user's Def by 2 for each hit.",
		id = Moves.DIAMONDSTORM,
		isViable = true,
		//name = "Diamond Storm",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 50,
			self = {
				boosts = new Boosts() {
					def = 2
				}
			}
		},
		target = Target.AllAdjacentFoes,
		type = Types.ROCK,
		zMovePower = 180,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 91,
		accuracy = 100,
		basePower = 80,
		category = Category.PHYSICAL,
		//desc = "This attack charges on the first turn and executes on the second. On the first turn, the user avoids all attacks other than Earthquake and Magnitude but takes double damage from them, and is also unaffected by weather. If the user is holding a Power Herb, the move completes in one turn.",
		//shortDesc = "Digs underground turn 1, strikes turn 2.",
		id = Moves.DIG,
		//name = "Dig",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, charge: true, protect: true, mirror: true, nonsky: true),
		//onTry = function (attacker, defender, move) {
		//	if (attacker.removeVolatile(move.id)) {
		//		return;
		//	}
		//	this.add('-prepare', attacker, move.name, defender);
		//	if (!this.runEvent("ChargeMove", attacker, defender, move)) {
		//		this.add('-anim', attacker, move.name, defender);
		//		return;
		//	}
		//	attacker.addVolatile("twoturnmove", defender);
		//	return null;
		//},
		effect = {
			duration = 2,
			//onImmunity = function (type, pokemon) {
			//	if (type === "sandstorm" || type === "hail") return false;
			//},
			//onTryImmunity = function (target, source, move) {
			//	if (move.id === "earthquake" || move.id === "magnitude" || move.id === "helpinghand") {
			//		return;
			//	}
			//	if (source.hasAbility("noguard") || target.HasAbility("noguard")) {
			//		return;
			//	}
			//	if (source.volatiles["lockon"] && target === source.volatiles["lockon"].source) return;
			//	return false;
			//},
			//onSourceModifyDamage = function (damage, source, target, move) {
			//	if (move.id === "earthquake" || move.id === "magnitude") {
			//		return this.chainModify(2);
			//	}
			//}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.GROUND,
		zMovePower = 160,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 50,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "For 4 turns, the target'S last move used becomes disabled. Fails if one of the target'S moves IS already disabled, if the target Has not made a move, or if the target No longer knows the move.",
		//shortDesc = "For 4 turns, disables the target'S last move used.",
		id = Moves.DISABLE,
		isViable = true,
		//name = "Disable",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true, authentic: true),
		volatileStatus = "disable",
		//onTryHit = function (target) {
		//	if (!target.LastMove || target.LastMove.isZ) {
		//		return false;
		//	}
		//},
		effect = {
			duration = 4,
			noCopy = true, // doesn't get copied by Baton Pass
			//onStart = function (pokemon, source, effect) {
			//	if (!this.willMove(pokemon)) {
			//		this.effectData.duration++;
			//	}
			//	if (!pokemon.lastMove) {
			//		this.debug('pokemon hasn\'t moved yet');
			//		return false;
			//	}
			//	for (const moveSlOT of pokemon.moveSlOTS) {
			//		if (moveSlOT.id === pokemon.lastMove.id) {
			//			if (!moveSlOT.pp) {
			//				this.debug('Move out of PP');
			//				return false;
			//			} else {
			//				if (effect.id === "cursedbody") {
			//					this.add('-start', pokemon, "Disable", moveSlOT.move, '[from] ability = Cursed Body', '[of] ' + source);
			//				} else {
			//					this.add('-start', pokemon, "Disable", moveSlOT.move);
			//				}
			//				this.effectData.move = pokemon.lastMove.id;
			//				return;
			//			}
			//		}
			//	}
			//	// this can happen if Disable works on a Z-move
			//	return false;
			//},
			//onResidualOrder = 14,
			//onEnd = function (pokemon) {
			//	this.add('-end', pokemon, "Disable");
			//},
			//onBeforeMovePriority = 7,
			//onBeforeMove = function (attacker, defender, move) {
			//	if (move.id === this.effectData.move) {
			//		this.add("cant", attacker, "Disable", move);
			//		return false;
			//	}
			//},
			//onDisableMove = function (pokemon) {
			//	for (const moveSlOT of pokemon.moveSlOTS) {
			//		if (moveSlOT.id === this.effectData.move) {
			//			pokemon.disableMove(moveSlOT.id);
			//		}
			//	}
			//}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 574,
		//accuracy = true,
		basePower = 40,
		category = Category.SPECIAL,
		//desc = "This move does not check accuracy.",
		//shortDesc = "This move does not check accuracy. Hits foes.",
		id = Moves.DISARMINGVOICE,
		//name = "Disarming Voice",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, sound: true, authentic: true),
		//secondary = false,
		target = Target.AllAdjacentFoes,
		type = Types.FAIRY,
		zMovePower = 100,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 435,
		accuracy = 100,
		basePower = 80,
		category = Category.SPECIAL,
		//desc = "Has a 30% chance to paralyze the target.",
		//shortDesc = "30% chance to paralyze adjacent Pokemon.",
		id = Moves.DISCHARGE,
		isViable = true,
		//name = "Discharge",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 30,
			status = "par"
		},
		target = Target.AllAdjacent,
		type = Types.ELECTRIC,
		zMovePower = 160,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 291,
		accuracy = 100,
		basePower = 80,
		category = Category.PHYSICAL,
		//desc = "This attack charges on the first turn and executes on the second. On the first turn, the user avoids all attacks other than Surf and Whirlpool but takes double damage from them, and is also unaffected by weather. If the user is holding a Power Herb, the move completes in one turn.",
		//shortDesc = "Dives underwater turn 1, strikes turn 2.",
		id = Moves.DIVE,
		//name = "Dive",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, charge: true, protect: true, mirror: true, nonsky: true),
		//onTry = function (attacker, defender, move) {
		//	if (attacker.removeVolatile(move.id)) {
		//		return;
		//	}
		//	this.add('-prepare', attacker, move.name, defender);
		//	if (!this.runEvent("ChargeMove", attacker, defender, move)) {
		//		this.add('-anim', attacker, move.name, defender);
		//		return;
		//	}
		//	attacker.addVolatile("twoturnmove", defender);
		//	return null;
		//},
		effect = {
			duration = 2,
			//onImmunity = function (type, pokemon) {
			//	if (type === "sandstorm" || type === "hail") return false;
			//},
			//onTryImmunity = function (target, source, move) {
			//	if (move.id === "surf" || move.id === "whirlpool" || move.id === "helpinghand") {
			//		return;
			//	}
			//	if (source.hasAbility("noguard") || target.HasAbility("noguard")) {
			//		return;
			//	}
			//	if (source.volatiles["lockon"] && target === source.volatiles["lockon"].source) return;
			//	return false;
			//},
			//onSourceModifyDamage = function (damage, source, target, move) {
			//	if (move.id === "surf" || move.id === "whirlpool") {
			//		return this.chainModify(2);
			//	}
			//}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.WATER,
		zMovePower = 160,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 146,
		accuracy = 100,
		basePower = 70,
		category = Category.PHYSICAL,
		//desc = "Has a 20% chance to confuse the target.",
		//shortDesc = "20% chance to confuse the target.",
		id = Moves.DIZZYPUNCH,
		//name = "Dizzy Punch",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true, punch: true),
		secondary = new Secondary() {
			chance = 20,
			volatileStatus = "confusion"
		},
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 140,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 353,
		accuracy = 100,
		basePower = 140,
		category = Category.SPECIAL,
		//desc = "Deals damage two turns after this move is used. At the end of that turn, the damage is calculated at that time and dealt to the Pokemon at the position the target Had when the move was used. If the user is no longer active at the time, damage is calculated based on the user's natural Special Attack stat, types, and level, with no boosts from its held item or Ability. Fails if this move or Future Sight is already in effect for the target'S position.",
		//shortDesc = "Hits two turns after being used.",
		id = Moves.DOOMDESIRE,
		//name = "Doom Desire",
		pp = 5,
		priority = 0,
		flags = new Flags (),
		isFutureMove = true,
		//onTry = function (source, target) {
		//	target.Side.addSideCondition("futuremove");
		//	if (target.Side.sideConditions["futuremove"].positions[target.Position]) {
		//		return false;
		//	}
		//	target.Side.sideConditions["futuremove"].positions[target.Position] = {
		//		duration = 3,
		//		move = "doomdesire",
		//		source = source,
		//		moveData = {
		//			id = "doomdesire",
		//			//name = "Doom Desire",
		//			accuracy = 100,
		//			basePower = 140,
		//			category = Category.SPECIAL,
		//			priority = 0,
		//			flags = new Flags (),
		//			effectType = "Move",
		//			isFutureMove = true,
		//			type = "Steel"
		//		}
		//	};
		//	this.add('-start', source, 'Doom Desire');
		//	return null;
		//},
		//secondary = false,
		target = Target.Normal,
		type = Types.STEEL,
		zMovePower = 200,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 38,
		accuracy = 100,
		basePower = 120,
		category = Category.PHYSICAL,
		//desc = "If the target Lost HP, the user takes recoil damage equal to 33% the HP lost by the target, rounded half up, but not less than 1 HP.",
		//shortDesc = "Has 33% recoil.",
		id = Moves.DOUBLEEDGE,
		isViable = true,
		//name = "Double-Edge",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		recoil = new int[] {33, 100},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 190,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 458,
		accuracy = 90,
		basePower = 35,
		category = Category.PHYSICAL,
		//desc = "Hits twice. If the first hit breaks the target'S substitute, it will take damage for the second hit.",
		//shortDesc = "Hits 2 times in one turn.",
		id = Moves.DOUBLEHIT,
		//name = "Double Hit",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		multihit = 2,
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 140,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 24,
		accuracy = 100,
		basePower = 30,
		category = Category.PHYSICAL,
		//desc = "Hits twice. If the first hit breaks the target'S substitute, it will take damage for the second hit.",
		//shortDesc = "Hits 2 times in one turn.",
		id = Moves.DOUBLEKICK,
		//name = "Double Kick",
		pp = 30,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		multihit = 2,
		//secondary = false,
		target = Target.Normal,
		type = Types.FIGHTING,
		zMovePower = 100,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 3,
		accuracy = 85,
		basePower = 15,
		category = Category.PHYSICAL,
		//desc = "Hits two to five times. Has a 1/3 chance to hit two or three times, and a 1/6 chance to hit four or five times. If one of the hits breaks the target'S substitute, it will take damage for the remaining hits. If the user has the Ability Skill Link, this move will always hit five times.",
		//shortDesc = "Hits 2-5 times in one turn.",
		id = Moves.DOUBLESLAP,
		//name = "Double Slap",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		multihit = new int[] {2, 5},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 100,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 104,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the user's evasiveness by 1 stage.",
		//shortDesc = "Raises the user's evasiveness by 1.",
		id = Moves.DOUBLETEAM,
		//name = "Double Team",
		pp = 15,
		priority = 0,
		flags = new Flags (snatch: true),
		boosts = new Boosts() {
			evasion = 1
		},
		//secondary = false,
		target = Target.Self,
		type = Types.NORMAL,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 434,
		accuracy = 90,
		basePower = 130,
		category = Category.SPECIAL,
		//desc = "Lowers the user's Special Attack by 2 stages.",
		//shortDesc = "Lowers the user's Sp. Atk by 2.",
		id = Moves.DRACOMETEOR,
		isViable = true,
		//name = "Draco Meteor",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		self = {
			boosts = new Boosts() {
				spa = -2
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.DRAGON,
		zMovePower = 195,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 620,
		accuracy = 100,
		basePower = 120,
		category = Category.PHYSICAL,
		//desc = "Lowers the user's Defense and Special Defense by 1 stage.",
		//shortDesc = "Lowers the user's Defense and Sp. Def by 1.",
		id = Moves.DRAGONASCENT,
		isViable = true,
		//name = "Dragon Ascent",
		pp = 5,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true, distance: true),
		self = {
			boosts = new Boosts() {
				def = -1,
				spd = -1
			}
		},
		target = Target.Any,
		type = Types.FLYING,
		zMovePower = 190,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 225,
		accuracy = 100,
		basePower = 60,
		category = Category.SPECIAL,
		//desc = "Has a 30% chance to paralyze the target.",
		//shortDesc = "30% chance to paralyze the target.",
		id = Moves.DRAGONBREATH,
		//name = "Dragon Breath",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 30,
			status = "par"
		},
		target = Target.Normal,
		type = Types.DRAGON,
		zMovePower = 120,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 337,
		accuracy = 100,
		basePower = 80,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect.",
		id = Moves.DRAGONCLAW,
		isViable = true,
		//name = "Dragon Claw",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.DRAGON,
		zMovePower = 160,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 349,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the user's Attack and Speed by 1 stage.",
		//shortDesc = "Raises the user's Attack and Speed by 1.",
		id = Moves.DRAGONDANCE,
		isViable = true,
		//name = "Dragon Dance",
		pp = 20,
		priority = 0,
		flags = new Flags (snatch: true, dance: true),
		boosts = new Boosts() {
			atk = 1,
			spe = 1
		},
		//secondary = false,
		target = Target.Self,
		type = Types.DRAGON,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 692,
		accuracy = 100,
		basePower = 90,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect.",
		id = Moves.DRAGONHAMMER,
		isViable = true,
		//name = "Dragon Hammer",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.DRAGON,
		zMovePower = 175,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 406,
		accuracy = 100,
		basePower = 85,
		category = Category.SPECIAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect.",
		id = Moves.DRAGONPULSE,
		isViable = true,
		//name = "Dragon Pulse",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, pulse: true, mirror: true, distance: true),
		//secondary = false,
		target = Target.Any,
		type = Types.DRAGON,
		zMovePower = 160,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 82,
		accuracy = 100,
		basePower = 0,
		damage = 40,
		category = Category.SPECIAL,
		//desc = "Deals 40 HP of damage to the target.",
		//shortDesc = "Always does 40 HP of damage.",
		id = Moves.DRAGONRAGE,
		//name = "Dragon Rage",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.DRAGON,
		zMovePower = 100,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 407,
		accuracy = 75,
		basePower = 100,
		category = Category.PHYSICAL,
		//desc = "Has a 20% chance to flinch the target. Damage doubles and no accuracy check is done if the target Has used Minimize while active.",
		//shortDesc = "20% chance to flinch the target.",
		id = Moves.DRAGONRUSH,
		//name = "Dragon Rush",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 20,
			volatileStatus = "flinch"
		},
		target = Target.Normal,
		type = Types.DRAGON,
		zMovePower = 180,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 525,
		accuracy = 90,
		basePower = 60,
		category = Category.PHYSICAL,
		//desc = "If both the user and the target Have not fainted, the target Is forced to switch out and be replaced with a random unfainted ally. This effect fails if the target Used Ingrain previously, has the Ability Suction Cups, or this move hit a substitute.",
		//shortDesc = "Forces the target To switch to a random ally.",
		id = Moves.DRAGONTAIL,
		isViable = true,
		//name = "Dragon Tail",
		pp = 10,
		priority = -6,
		flags = new Flags (contact: true, protect: true, mirror: true),
		forceSwitch = true,
		target = Target.Normal,
		type = Types.DRAGON,
		zMovePower = 120,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 577,
		accuracy = 100,
		basePower = 50,
		category = Category.SPECIAL,
		//desc = "The user recovers 3/4 the HP lost by the target, rounded half up. If Big Root is held by the user, the HP recovered is 1.3x normal, rounded half down.",
		//shortDesc = "User recovers 75% of the damage dealt.",
		id = Moves.DRAININGKISS,
		//name = "Draining Kiss",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true, heal: true),
		drain = new int[] {3, 4},
		//secondary = false,
		target = Target.Normal,
		type = Types.FAIRY,
		zMovePower = 100,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 409,
		accuracy = 100,
		basePower = 75,
		category = Category.PHYSICAL,
		//desc = "The user recovers 1/2 the HP lost by the target, rounded half up. If Big Root is held by the user, the HP recovered is 1.3x normal, rounded half down.",
		//shortDesc = "User recovers 50% of the damage dealt.",
		id = Moves.DRAINPUNCH,
		isViable = true,
		//name = "Drain Punch",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true, punch: true, heal: true),
		drain = new int[] {1, 2},
		//secondary = false,
		target = Target.Normal,
		type = Types.FIGHTING,
		zMovePower = 140,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 138,
		accuracy = 100,
		basePower = 100,
		category = Category.SPECIAL,
		//desc = "The target Is unaffected by this move unless it is asleep. The user recovers 1/2 the HP lost by the target, rounded half up. If Big Root is held by the user, the HP recovered is 1.3x normal, rounded half down.",
		//shortDesc = "User gains 1/2 HP inflicted. Sleeping target Only.",
		id = Moves.DREAMEATER,
		//name = "Dream Eater",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, heal: true),
		drain = new int[] {1, 2},
		//onTryHit = function (target) {
		//	if (target.Status !== "slp" && !target.HasAbility("comatose")) {
		//		this.add('-immune', target, '[msg]');
		//		return null;
		//	}
		//},
		//secondary = false,
		target = Target.Normal,
		type = Types.PSYCHIC,
		zMovePower = 180,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 65,
		accuracy = 100,
		basePower = 80,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect.",
		id = Moves.DRILLPECK,
		isViable = true,
		//name = "Drill Peck",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true, distance: true),
		//secondary = false,
		target = Target.Any,
		type = Types.FLYING,
		zMovePower = 160,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 529,
		accuracy = 95,
		basePower = 80,
		category = Category.PHYSICAL,
		//desc = "Has a higher chance for a critical hit.",
		//shortDesc = "High critical hit ratio.",
		id = Moves.DRILLRUN,
		isViable = true,
		//name = "Drill Run",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		critRatio = 2,
		//secondary = false,
		target = Target.Normal,
		type = Types.GROUND,
		zMovePower = 160,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 530,
		accuracy = 90,
		basePower = 40,
		category = Category.PHYSICAL,
		//desc = "Hits twice. If the first hit breaks the target'S substitute, it will take damage for the second hit.",
		//shortDesc = "Hits 2 times in one turn.",
		id = Moves.DUALCHOP,
		//name = "Dual Chop",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		multihit = 2,
		//secondary = false,
		target = Target.Normal,
		type = Types.DRAGON,
		zMovePower = 100,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 223,
		accuracy = 50,
		basePower = 100,
		category = Category.PHYSICAL,
		//desc = "Has a 100% chance to confuse the target.",
		//shortDesc = "100% chance to confuse the target.",
		id = Moves.DYNAMICPUNCH,
		//name = "Dynamic Punch",
		pp = 5,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true, punch: true),
		secondary = new Secondary() {
			chance = 100,
			volatileStatus = "confusion"
		},
		target = Target.Normal,
		type = Types.FIGHTING,
		zMovePower = 180,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 414,
		accuracy = 100,
		basePower = 90,
		category = Category.SPECIAL,
		//desc = "Has a 10% chance to lower the target'S Special Defense by 1 stage.",
		//shortDesc = "10% chance to lower the target'S Sp. Def by 1.",
		id = Moves.EARTHPOWER,
		isViable = true,
		//name = "Earth Power",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, nonsky: true),
		secondary = new Secondary() {
			chance = 10,
			boosts = new Boosts() {
				spd = -1
			}
		},
		target = Target.Normal,
		type = Types.GROUND,
		zMovePower = 175,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 89,
		accuracy = 100,
		basePower = 100,
		category = Category.PHYSICAL,
		//desc = "Damage doubles if the target Is using Dig.",
		//shortDesc = "Hits adjacent Pokemon. Power doubles on Dig.",
		id = Moves.EARTHQUAKE,
		isViable = true,
		//name = "Earthquake",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, nonsky: true),
		//secondary = false,
		target = Target.AllAdjacent,
		type = Types.GROUND,
		zMovePower = 180,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 497,
		accuracy = 100,
		basePower = 40,
		//basePowerCallback = function () {
		//	if (this.pseudoWeather.echoedvoice) {
		//		return 40 * this.pseudoWeather.echoedvoice.multiplier;
		//	}
		//	return 40;
		//},
		category = Category.SPECIAL,
		//desc = "For every consecutive turn that this move is used by at least one Pokemon, this move's power is multiplied by the number of turns to pass, but not more than 5.",
		//shortDesc = "Power increases when used on consecutive turns.",
		id = Moves.ECHOEDVOICE,
		//name = "Echoed Voice",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, sound: true, authentic: true),
		//onTry = function () {
		//	this.addPseudoWeather("echoedvoice");
		//},
		effect = {
			duration = 2,
			//onStart = function () {
			//	this.effectData.multiplier = 1;
			//},
			//onRestart = function () {
			//	if (this.effectData.duration !== 2) {
			//		this.effectData.duration = 2;
			//		if (this.effectData.multiplier < 5) {
			//			this.effectData.multiplier++;
			//		}
			//	}
			//}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 100,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 598,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Lowers the target'S Special Attack by 2 stages.",
		//shortDesc = "Lowers the target'S Sp. Atk by 2.",
		id = Moves.EERIEIMPULSE,
		//name = "Eerie Impulse",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true),
		boosts = new Boosts() {
			spa = -2
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.ELECTRIC,
		zMoveBoost = new Boosts() {spd = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 121,
		accuracy = 75,
		basePower = 100,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect.",
		id = Moves.EGGBOMB,
		//name = "Egg Bomb",
		pp = 10,
		priority = 0,
		flags = new Flags (bullet: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 180,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 604,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "For 5 turns, the terrain becomes Electric Terrain. During the effect, the power of Electric-type attacks made by grounded Pokemon is multiplied by 1.5 and grounded Pokemon cannot fall asleep; Pokemon already asleep do not wake up. Camouflage transforms the user into an Electric type, Nature Power becomes Thunderbolt, and Secret Power has a 30% chance to cause paralysis. Fails if the current terrain is Electric Terrain.",
		//shortDesc = "5 turns. Grounded = +Electric power, can't sleep.",
		id = Moves.ELECTRICTERRAIN,
		//name = "Electric Terrain",
		pp = 10,
		priority = 0,
		flags = new Flags (nonsky: true),
		terrain = "electricterrain",
		effect = {
			duration = 5
			//durationCallback = function (source, effect) {
			//	if (source && source.hasItem("terrainextender")) {
			//		return 8;
			//	}
			//	return 5;
			//},
			//onSetStatus = function (status, target, source, effect) {
			//	if (status.id === "slp" && target.IsGrounded() && !target.IsSemiInvulnerable()) {
			//		if (effect.effectType === "Move" && !effect.secondaries) {
			//			this.add('-activate', target, 'move = Electric Terrain');
			//		}
			//		return false;
			//	}
			//},
			//onTryAddVolatile = function (status, target) {
			//	if (!target.IsGrounded() || target.IsSemiInvulnerable()) return;
			//	if (status.id === "yawn") {
			//		this.add('-activate', target, 'move = Electric Terrain');
			//		return null;
			//	}
			//},
			//onBasePower = function (basePower, attacker, defender, move) {
			//	if (move.type === "Electric" && attacker.isGrounded() && !attacker.isSemiInvulnerable()) {
			//		this.debug('electric terrain boost');
			//		return this.chainModify(1.5);
			//	}
			//},
			//onStart = function (battle, source, effect) {
			//	if (effect && effect.effectType === "Ability") {
			//		this.add('-fieldstart', 'move = Electric Terrain', '[from] ability = ' + effect, '[of] ' + source);
			//	} else {
			//		this.add('-fieldstart', 'move = Electric Terrain');
			//	}
			//},
			//onResidualOrder = 21,
			//onResidualSubOrder = 2,
			//onEnd = function () {
			//	this.add('-fieldend', 'move = Electric Terrain');
			//}
		},
		//secondary = false,
		target = Target.All,
		type = Types.ELECTRIC,
		zMoveBoost = new Boosts() {spe = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 582,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Causes the target'S move to become Electric type this turn. Fails if the target Already moved this turn.",
		//shortDesc = "Changes the target'S move to Electric this turn.",
		id = Moves.ELECTRIFY,
		//name = "Electrify",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, mystery: true),
		volatileStatus = "electrify",
		//onTryHit = function (target) {
		//	if (!this.willMove(target) && target.ActiveTurns) return false;
		//},
		effect = {
			duration = 1
			//onStart = function (target) {
			//	this.add('-singleturn', target, 'move = Electrify');
			//},
			//onModifyMovePriority = -2,
			//onModifyMove = function (move) {
			//	this.debug('Electrify making move type electric');
			//	move.type = "Electric";
			//}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.ELECTRIC,
		zMoveBoost = new Boosts() {spa = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 486,
		accuracy = 100,
		basePower = 0,
		//basePowerCallback = function (pokemon, target) {
		//	let ratio = (pokemon.getStat("spe") / target.GetStat("spe"));
		//	this.debug([40, 60, 80, 120, 150][(Math.floor(ratio) > 4 ? 4  = Math.floor(ratio))] + ' bp');
		//	if (ratio >= 4) {
		//	return 150;
		//	}
		//	if (ratio >= 3) {
		//		return 120;
		//	}
		//	if (ratio >= 2) {
		//	return 80;
		//	}
		//	if (ratio >= 1) {
		//		return 60;
		//	}
		//	return 40;
		//},
		category = Category.SPECIAL,
		//desc = "The power of this move depends on (user's current Speed / target'S current Speed), rounded down. Power is equal to 150 if the result is 4 or more, 120 if 3, 80 if 2, 60 if 1, 40 if less than 1.",
		//shortDesc = "More power the faster the user is than the target.",
		id = Moves.ELECTROBALL,
		//name = "Electro Ball",
		pp = 10,
		priority = 0,
		flags = new Flags (bullet: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.ELECTRIC,
		zMovePower = 160,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 527,
		accuracy = 95,
		basePower = 55,
		category = Category.SPECIAL,
		//desc = "Has a 100% chance to lower the target'S Speed by 1 stage.",
		//shortDesc = "100% chance to lower the foe(s) Speed by 1.",
		id = Moves.ELECTROWEB,
		//name = "Electroweb",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 100,
			boosts = new Boosts() {
				spe = -1
			}
		},
		target = Target.AllAdjacentFoes,
		type = Types.ELECTRIC,
		zMovePower = 100,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 373,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "For 5 turns, the target'S held item has no effect. An item's effect of causing forme changes is unaffected, but any other effects from such items are negated. During the effect, Fling and Natural Gift are prevented from being used by the target. Items thrown at the target With Fling will still activate for it. If the target Uses Baton Pass, the replacement will remain unable to use items.",
		//shortDesc = "For 5 turns, the target'S item has no effect.",
		id = Moves.EMBARGO,
		//name = "Embargo",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true),
		volatileStatus = "embargo",
		effect = {
			duration = 5,
			onStart = function (pokemon) {
				this.add('-start', pokemon, "Embargo");
			},
			// Item suppression implemented in Pokemon.ignoringItem() within sim/pokemon.js
			onResidualOrder = 18,
			onEnd = function (pokemon) {
				this.add('-end', pokemon, "Embargo");
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.DARK,
		zMoveBoost = new Boosts() {spa = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 52,
		accuracy = 100,
		basePower = 40,
		category = Category.SPECIAL,
		//desc = "Has a 10% chance to burn the target.",
		//shortDesc = "10% chance to burn the target.",
		id = Moves.EMBER,
		//name = "Ember",
		pp = 25,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 10,
			status = "brn"
		},
		target = Target.Normal,
		type = Types.FIRE,
		zMovePower = 100,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 227,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "For 3 turns, the target Is forced to repeat its last move used. If the affected move runs out of PP, the effect ends. Fails if the target Is already under this effect, if it has not made a move, if the move has 0 PP, or if the move is Assist, Copycat, Encore, Me First, Metronome, Mimic, Mirror Move, Nature Power, Sketch, Sleep Talk, Struggle, Transform, or a Z-Move.",
		//shortDesc = "The target Repeats its last move for 3 turns.",
		id = Moves.ENCORE,
		isViable = true,
		//name = "Encore",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true, authentic: true),
		volatileStatus = "encore",
		effect = {
			duration = 3,
			noCopy = true, // doesn't get copied by Z-Baton Pass
			onStart = function (target) {
				let noEncore = ["assist", "copycat", "encore", "mefirst", "metronome", "mimic", "mirrormove", "naturepower", "sketch", "sleeptalk", "struggle", "transform"];
				let moveIndex = target.LastMove ? target.Moves.INDEXOF(target.LastMove.id)  = -1;
				if (!target.LastMove || target.LastMove.isZ || noEncore.includes(target.LastMove.id) || !target.MoveSlOTS[moveIndex] || target.MoveSlOTS[moveIndex].pp <= 0) {
					// it failed
					delete target.Volatiles["encore"];
					return false;
				}
				this.effectData.move = target.LastMove.id;
				this.add('-start', target, "Encore");
				if (!this.willMove(target)) {
					this.effectData.duration++;
				}
			},
			onOverrideAction = function (pokemon, target, move) {
				if (move.id !== this.effectData.move) return this.effectData.move;
			},
			onResidualOrder = 13,
			onResidual = function (target) {
				if (target.Moves.INCLUDES(this.effectData.move) && target.MoveSlOTS[target.Moves.INDEXOF(this.effectData.move)].pp <= 0) { // early termination if you run out of PP
					delete target.Volatiles.encore;
					this.add('-end', target, "Encore");
				}
			},
			onEnd = function (target) {
				this.add('-end', target, "Encore");
			},
			onDisableMove = function (pokemon) {
				if (!this.effectData.move || !pokemon.hasMove(this.effectData.move)) {
					return;
				}
				for (const moveSlOT of pokemon.moveSlOTS) {
					if (moveSlOT.id !== this.effectData.move) {
						pokemon.disableMove(moveSlOT.id);
					}
				}
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {spe = 1},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 283,
		accuracy = 100,
		basePower = 0,
		damageCallback = function (pokemon, target) {
			return target.Hp - pokemon.hp;
		},
		category = Category.PHYSICAL,
		//desc = "Deals damage to the target Equal to (target'S current HP - user's current HP). The target Is unaffected if its current HP is less than or equal to the user's current HP.",
		//shortDesc = "Lowers the target'S HP to the user's HP.",
		id = Moves.ENDEAVOR,
		isViable = true,
		//name = "Endeavor",
		pp = 5,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		onTry = function (pokemon, target) {
			if (pokemon.hp >= target.Hp) {
				this.add('-immune', target, '[msg]');
				return null;
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 160,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 203,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user will survive attacks made by other Pokemon during this turn with at least 1 HP. This move has a 1/X chance of being successful, where X starts at 1 and triples each time this move is successfully used. X resets to 1 if this move fails or if the user's last move used is not Baneful Bunker, Detect, Endure, King's Shield, Protect, Quick Guard, Spiky Shield, or Wide Guard. Fails if the user moves LAST this turn.",
		//shortDesc = "The user survives the next hit with at least 1 HP.",
		id = Moves.ENDURE,
		//name = "Endure",
		pp = 10,
		priority = 4,
		flags = new Flags (),
		stallingMove = true,
		volatileStatus = "endure",
		onTryHit = function (pokemon) {
			return this.willAct() && this.runEvent("StallMove", pokemon);
		},
		onHit = function (pokemon) {
			pokemon.addVolatile("stall");
		},
		effect = {
			duration = 1,
			onStart = function (target) {
				this.add('-singleturn', target, 'move = Endure');
			},
			onDamagePriority = -10,
			onDamage = function (damage, target, source, effect) {
				if (effect && effect.effectType === "Move" && damage >= target.Hp) {
					this.add('-activate', target, 'move = Endure');
					return target.Hp - 1;
				}
			}
		},
		//secondary = false,
		target = Target.Self,
		type = Types.NORMAL,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 412,
		accuracy = 100,
		basePower = 90,
		category = Category.SPECIAL,
		//desc = "Has a 10% chance to lower the target'S Special Defense by 1 stage.",
		//shortDesc = "10% chance to lower the target'S Sp. Def by 1.",
		id = Moves.ENERGYBALL,
		isViable = true,
		//name = "Energy Ball",
		pp = 10,
		priority = 0,
		flags = new Flags (bullet: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 10,
			boosts = new Boosts() {
				spd = -1
			}
		},
		target = Target.Normal,
		type = Types.GRASS,
		zMovePower = 175,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 494,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Causes the target'S Ability to become the same as the user's. Fails if the target'S Ability is Multitype, Stance Change, Truant, or the same Ability as the user, or if the user's Ability is Flower Gift, Forecast, Illusion, Imposter, Multitype, Stance Change, Trace, or Zen Mode.",
		//shortDesc = "The target'S Ability changes to match the user's.",
		id = Moves.ENTRAINMENT,
		//name = "Entrainment",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true, mystery: true),
		onTryHit = function (target, source) {
			if (target === source) return false;
			let bannedTargetABilities = ["battlebond", "comatose", "disguise", "multitype", "powerconstruct", "rkssystem", "schooling", "shieldsdown", "stancechange", "truant"];
			let bannedSourceAbilities = ["battlebond", "comatose", "disguise", "flowergift", "forecast", "illusion", "imposter", "multitype", "powerconstruct", "powerofalchemy", "receiver", "rkssystem", "schooling", "shieldsdown", "stancechange", "trace", "zenmode"];
			if (bannedTargetABilities.includes(target.Ability) || bannedSourceAbilities.includes(source.ability) || target.Ability === source.ability) {
				return false;
			}
		},
		onHit = function (target, source) {
			let oldAbility = target.SetAbility(source.ability);
			if (oldAbility) {
				this.add('-ability', target, this.getAbility(target.Ability).name, '[from] move = Entrainment');
				return;
			}
			return false;
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {spd = 1},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 284,
		accuracy = 100,
		basePower = 150,
		basePowerCallback = function (pokemon, target, move) {
			return move.basePower * pokemon.hp / pokemon.maxhp;
		},
		category = Category.SPECIAL,
		//desc = "Power is equal to (user's current HP * 150 / user's maximum HP), rounded down, but not less than 1.",
		//shortDesc = "Less power as user's HP decreases. Hits foe(s).",
		id = Moves.ERUPTION,
		isViable = true,
		//name = "Eruption",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.AllAdjacentFoes,
		type = Types.FIRE,
		zMovePower = 200,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 153,
		accuracy = 100,
		basePower = 250,
		category = Category.PHYSICAL,
		//desc = "The user faints after using this move, even if this move fails for having no target. This move is prevented from executing if any active Pokemon has the Ability Damp.",
		//shortDesc = "Hits adjacent Pokemon. The user faints.",
		id = Moves.EXPLOSION,
		isViable = true,
		//name = "Explosion",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		selfdestruct = "always",
		//secondary = false,
		target = Target.AllAdjacent,
		type = Types.NORMAL,
		zMovePower = 200,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 326,
		accuracy = 100,
		basePower = 80,
		category = Category.SPECIAL,
		//desc = "Has a 10% chance to flinch the target.",
		//shortDesc = "10% chance to flinch the target.",
		id = Moves.EXTRASENSORY,
		isViable = true,
		//name = "Extrasensory",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 10,
			volatileStatus = "flinch"
		},
		target = Target.Normal,
		type = Types.PSYCHIC,
		zMovePower = 160,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 702,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the user's Attack, Defense, Special Attack, Special Defense, and Speed by 2 stages.",
		//shortDesc = "Raises user's Atk, Def, SpA, SpD, and Spe by 2.",
		id = Moves.EXTREMEEVOBOOST,
		isViable = true,
		//name = "Extreme Evoboost",
		pp = 1,
		priority = 0,
		flags = new Flags (),
		isZ = "eeviumz",
		boosts = new Boosts() {
			atk = 2,
			def = 2,
			spa = 2,
			spd = 2,
			spe = 2
		},
		//secondary = false,
		target = Target.Self,
		type = Types.NORMAL,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 245,
		accuracy = 100,
		basePower = 80,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "Nearly always goes first.",
		id = Moves.EXTREMESPEED,
		isViable = true,
		//name = "Extreme Speed",
		pp = 5,
		priority = 2,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 160,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 263,
		accuracy = 100,
		basePower = 70,
		category = Category.PHYSICAL,
		//desc = "Power doubles if the user is burned, paralyzed, or poisoned. The physical damage halving effect from the user's burn is ignored.",
		//shortDesc = "Power doubles if user is burn/poison/paralyzed.",
		id = Moves.FACADE,
		isViable = true,
		//name = "Facade",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		onBasePowerPriority = 4,
		onBasePower = function (basePower, pokemon) {
			if (pokemon.status && pokemon.status !== "slp") {
				return this.chainModify(2);
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 140,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 185,
		//accuracy = true,
		basePower = 60,
		category = Category.PHYSICAL,
		//desc = "This move does not check accuracy.",
		//shortDesc = "This move does not check accuracy.",
		id = Moves.FEINTATTACK,
		//name = "Feint Attack",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.DARK,
		zMovePower = 120,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 587,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Prevents all active Pokemon from switching next turn. A Pokemon can still switch out if it is holding Shed Shell or uses Baton Pass, Parting Shot, U-turn, or Volt Switch. Fails if this move was used successfully last turn.",
		//shortDesc = "Prevents all Pokemon from switching next turn.",
		id = Moves.FAIRYLOCK,
		//name = "Fairy Lock",
		pp = 10,
		priority = 0,
		flags = new Flags (mirror: true, authentic: true),
		pseudoWeather = "fairylock",
		effect = {
			duration = 2,
			onStart = function (target) {
				this.add('-activate', target, 'move = Fairy Lock');
			},
			onTrapPokemon = function (pokemon) {
				pokemon.tryTrap();
			}
		},
		//secondary = false,
		target = Target.All,
		type = Types.FAIRY,
		zMoveBoost = new Boosts() {def = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 584,
		accuracy = 100,
		basePower = 40,
		category = Category.SPECIAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect.",
		id = Moves.FAIRYWIND,
		//name = "Fairy Wind",
		pp = 30,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.FAIRY,
		zMovePower = 100,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 252,
		accuracy = 100,
		basePower = 40,
		category = Category.PHYSICAL,
		//desc = "Has a 100% chance to flinch the target. Fails unless it is the user's first turn on the field.",
		//shortDesc = "Hits first. First turn out only. 100% flinch chance.",
		id = Moves.FAKEOUT,
		isViable = true,
		//name = "Fake Out",
		pp = 10,
		priority = 3,
		flags = new Flags (contact: true, protect: true, mirror: true),
		onTry = function (pokemon, target) {
			if (pokemon.activeTurns > 1) {
				this.attrLastMove('[still]');
				this.add('-fail', pokemon);
				this.add('-hint', "Fake Out only works on your first turn out.");
				return null;
			}
		},
		secondary = new Secondary() {
			chance = 100,
			volatileStatus = "flinch"
		},
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 100,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 313,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Lowers the target'S Special Defense by 2 stages.",
		//shortDesc = "Lowers the target'S Sp. Def by 2.",
		id = Moves.FAKETEARS,
		//name = "Fake Tears",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true, mystery: true),
		boosts = new Boosts() {
			spd = -2
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.DARK,
		zMoveBoost = new Boosts() {spa = 1},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 206,
		accuracy = 100,
		basePower = 40,
		category = Category.PHYSICAL,
		//desc = "Leaves the target With at least 1 HP.",
		//shortDesc = "Always leaves the target With at least 1 HP.",
		id = Moves.FALSESWIPE,
		//name = "False Swipe",
		pp = 40,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		noFaint = true,
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 100,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 297,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Lowers the target'S Attack by 2 stages.",
		//shortDesc = "Lowers the target'S Attack by 2.",
		id = Moves.FEATHERDANCE,
		//name = "Feather Dance",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true, mystery: true, dance: true),
		boosts = new Boosts() {
			atk = -2
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.FLYING,
		zMoveBoost = new Boosts() {def = 1},
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 364,
		accuracy = 100,
		basePower = 30,
		category = Category.PHYSICAL,
		//desc = "If this move is successful, it breaks through the target'S Baneful Bunker, Detect, King's Shield, Protect, or Spiky Shield for this turn, allowing other Pokemon to attack the target Normally. If the target'S side is protected by Crafty Shield, Mat Block, Quick Guard, or Wide Guard, that protection is also broken for this turn and other Pokemon may attack the target'S side normally.",
		//shortDesc = "Nullifies Detect, Protect, and Quick/Wide Guard.",
		id = Moves.FEINT,
		//name = "Feint",
		pp = 10,
		priority = 2,
		flags = new Flags (mirror: true),
		breaksProtect = true,
		// Breaking protection implemented in scripts.js
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 100,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 565,
		accuracy = 100,
		basePower = 50,
		category = Category.PHYSICAL,
		//desc = "Raises the user's Attack by 3 stages if this move knocks out the target.",
		//shortDesc = "Raises user's Attack by 3 if this KOes the target.",
		id = Moves.FELLSTINGER,
		//name = "Fell Stinger",
		pp = 25,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		onAfterMoveSeCONDARYSELF = function (pokemon, target, move) {
			if (!target || target.Fainted || target.Hp <= 0) this.boost({atk = 3}, pokemon, pokemon, move);
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.BUG,
		zMovePower = 100,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 552,
		accuracy = 100,
		basePower = 80,
		category = Category.SPECIAL,
		//desc = "Has a 50% chance to raise the user's Special Attack by 1 stage.",
		//shortDesc = "50% chance to raise the user's Sp. Atk by 1.",
		id = Moves.FIERYDANCE,
		isViable = true,
		//name = "Fiery Dance",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, dance: true),
		secondary = new Secondary() {
			chance = 50,
			self = {
				boosts = new Boosts() {
					spa = 1
				}
			}
		},
		target = Target.Normal,
		type = Types.FIRE,
		zMovePower = 160,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 515,
		accuracy = 100,
		basePower = 0,
		damageCallback = function (pokemon) {
			let damage = pokemon.hp;
			pokemon.faint();
			return damage;
		},
		category = Category.SPECIAL,
		//desc = "Deals damage to the target Equal to the user's current HP. If this move is successful, the user faints.",
		//shortDesc = "Does damage equal to the user's HP. User faints.",
		id = Moves.FINALGAMBIT,
		isViable = true,
		//name = "Final Gambit",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true),
		selfdestruct = "ifHit",
		//secondary = false,
		target = Target.Normal,
		type = Types.FIGHTING,
		zMovePower = 180,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 126,
		accuracy = 85,
		basePower = 110,
		category = Category.SPECIAL,
		//desc = "Has a 10% chance to burn the target.",
		//shortDesc = "10% chance to burn the target.",
		id = Moves.FIREBLAST,
		isViable = true,
		//name = "Fire Blast",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 10,
			status = "brn"
		},
		target = Target.Normal,
		type = Types.FIRE,
		zMovePower = 185,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 424,
		accuracy = 95,
		basePower = 65,
		category = Category.PHYSICAL,
		//desc = "Has a 10% chance to burn the target And a 10% chance to flinch it.",
		//shortDesc = "10% chance to burn. 10% chance to flinch.",
		id = Moves.FIREFANG,
		isViable = true,
		//name = "Fire Fang",
		pp = 15,
		priority = 0,
		flags = new Flags (bite: true, contact: true, protect: true, mirror: true),
		secondaries = [
			{
				chance = 10,
				status = "brn"
			}, {
				chance = 10,
				volatileStatus = "flinch"
			},
		],
		target = Target.Normal,
		type = Types.FIRE,
		zMovePower = 120,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 680,
		accuracy = 100,
		basePower = 80,
		category = Category.PHYSICAL,
		//desc = "Has a 100% chance to lower the target'S Defense by 1 stage.",
		//shortDesc = "100% chance to lower the target'S Defense by 1.",
		id = Moves.FIRELASH,
		isViable = true,
		//name = "Fire Lash",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 100,
			boosts = new Boosts() {
				def = -1
			}
		},
		target = Target.Normal,
		type = Types.FIRE,
		zMovePower = 160,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 519,
		accuracy = 100,
		basePower = 80,
		basePowerCallback = function (target, source, move) {
			if (["grasspledge", "waterpledge"].includes(move.sourceEffect)) {
				this.add('-combine');
				return 150;
			}
			return 80;
		},
		category = Category.SPECIAL,
		//desc = "If one of the user's allies chose to use Grass Pledge or Water Pledge this turn and has not moved yet, it takes its turn immediately after the user and the user's move does nothing. If combined with Grass Pledge, the ally uses Fire Pledge with 150 Base Power and a sea of fire appears on the target'S side for 4 turns, which causes damage to non-Fire types equal to 1/8 of their maximum HP, rounded down, at the end of each turn during effect. If combined with Water Pledge, the ally uses Water Pledge with 150 Base Power and a rainbow appears on the user's side for 4 turns, which doubles secondary effect chances but does not stack with the Ability Serene Grace. When used as a combined move, this move gains STAB no matter what the user's type is. This move does not consume the user's Fire Gem.",
		//shortDesc = "Use with Grass or Water Pledge for added effect.",
		id = Moves.FIREPLEDGE,
		//name = "Fire Pledge",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, nonsky: true),
		onPrepareHit = function (target, source, move) {
			for (const action of this.queue) {
				// @ts-ignore
				if (!action.move || !action.pokemon || !action.pokemon.isActive || action.pokemon.fainted) continue;
				// @ts-ignore
				if (action.pokemon.side === source.side && ["grasspledge", "waterpledge"].includes(action.move.id)) {
					// @ts-ignore
					this.prioritizeAction(action);
					this.add('-waiting', source, action.pokemon);
					return null;
				}
			}
		},
		onModifyMove = function (move) {
			if (move.sourceEffect === "waterpledge") {
				move.type = "Water";
				move.hasSTAB = true;
			}
			if (move.sourceEffect === "grasspledge") {
				move.type = "Fire";
				move.hasSTAB = true;
			}
		},
		onHit = function (target, source, move) {
			if (move.sourceEffect === "grasspledge") {
				target.Side.addSideCondition("firepledge");
			}
			if (move.sourceEffect === "waterpledge") {
				source.side.addSideCondition("waterpledge");
			}
		},
		effect = {
			duration = 4,
			onStart = function (targetSIde) {
				this.add('-sidestart', targetSIde, 'Fire Pledge');
			},
			onEnd = function (targetSIde) {
				this.add('-sideend', targetSIde, 'Fire Pledge');
			},
			onResidual = function (side) {
				// @ts-ignore
				for (const pokemon of side.active) {
					if (pokemon && !pokemon.hasType("Fire")) {
						this.damage(pokemon.maxhp / 8, pokemon);
					}
				}
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.FIRE,
		zMovePower = 160,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 7,
		accuracy = 100,
		basePower = 75,
		category = Category.PHYSICAL,
		//desc = "Has a 10% chance to burn the target.",
		//shortDesc = "10% chance to burn the target.",
		id = Moves.FIREPUNCH,
		isViable = true,
		//name = "Fire Punch",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true, punch: true),
		secondary = new Secondary() {
			chance = 10,
			status = "brn"
		},
		target = Target.Normal,
		type = Types.FIRE,
		zMovePower = 140,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 83,
		accuracy = 85,
		basePower = 35,
		category = Category.SPECIAL,
		//desc = "Prevents the target From switching for four or five turns; seven turns if the user is holding Grip Claw. Causes damage to the target Equal to 1/8 of its maximum HP (1/6 if the user is holding Binding Band), rounded down, at the end of each turn during effect. The target Can still switch out if it is holding Shed Shell or uses Baton Pass, Parting Shot, U-turn, or Volt Switch. The effect ends if either the user or the target Leaves the field, or if the target Uses Rapid Spin or Substitute. This effect is not stackable or reset by using this or another partial-trapping move.",
		//shortDesc = "Traps and damages the target For 4-5 turns.",
		id = Moves.FIRESPIN,
		//name = "Fire Spin",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		volatileStatus = "partiallytrapped",
		//secondary = false,
		target = Target.Normal,
		type = Types.FIRE,
		zMovePower = 100,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 660,
		accuracy = 100,
		basePower = 90,
		category = Category.PHYSICAL,
		//desc = "Fails unless it is the user's first turn on the field.",
		//shortDesc = "Hits first. First turn out only.",
		id = Moves.FIRSTIMPRESSION,
		isViable = true,
		//name = "First Impression",
		pp = 10,
		priority = 2,
		flags = new Flags (contact: true, protect: true, mirror: true),
		onTry = function (pokemon, target) {
			if (pokemon.activeTurns > 1) {
				this.add('-fail', pokemon);
				this.add('-hint', "First Impression only works on your first turn out.");
				return null;
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.BUG,
		zMovePower = 175,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 90,
		accuracy = 30,
		basePower = 0,
		category = Category.PHYSICAL,
		//desc = "Deals damage to the target Equal to the target'S maximum HP. Ignores accuracy and evasiveness modifiers. This attack's accuracy is equal to (user's level - target'S level + 30)%, and fails if the target Is at a higher level. Pokemon with the Ability Sturdy are immune.",
		//shortDesc = "OHKOs the target. Fails if user is a lower level.",
		id = Moves.FISSURE,
		//name = "Fissure",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, nonsky: true),
		ohko = true,
		//secondary = false,
		target = Target.Normal,
		type = Types.GROUND,
		zMovePower = 180,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 175,
		accuracy = 100,
		basePower = 0,
		basePowerCallback = function (pokemon, target) {
			let ratio = pokemon.hp * 48 / pokemon.maxhp;
			if (ratio < 2) {
				return 200;
			}
			if (ratio < 5) {
				return 150;
			}
			if (ratio < 10) {
				return 100;
			}
			if (ratio < 17) {
				return 80;
			}
			if (ratio < 33) {
				return 40;
			}
			return 20;
		},
		category = Category.PHYSICAL,
		//desc = "Deals damage to the target Based on the amount of HP the user has left. X is equal to (user's current HP * 48 / user's maximum HP), rounded down; the base power of this attack is 20 if X is 33 to 48, 40 if X is 17 to 32, 80 if X is 10 to 16, 100 if X is 5 to 9, 150 if X is 2 to 4, and 200 if X is 0 or 1.",
		//shortDesc = "More power the less HP the user has left.",
		id = Moves.FLAIL,
		//name = "Flail",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 160,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 481,
		accuracy = 100,
		basePower = 70,
		category = Category.SPECIAL,
		//desc = "If this move is successful, each ally adjacent to the target Loses 1/16 of its maximum HP, rounded down, unless it has the Ability Magic Guard.",
		//shortDesc = "Damages Pokemon next to the target As well.",
		id = Moves.FLAMEBURST,
		//name = "Flame Burst",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		onHit = function (target, source) {
			if (target.Side.active.length === 1) {
				return;
			}
			for (const ally of target.Side.active) {
				if (ally && this.isAdjacent(target, ally)) {
					this.damage(ally.maxhp / 16, ally, source, "flameburst");
				}
			}
		},
		onAfterSubDamage = function (target, source) {
			if (target.Side.active.length === 1) {
				return;
			}
			for (const ally of target.Side.active) {
				if (ally && this.isAdjacent(target, ally)) {
					this.damage(ally.maxhp / 16, ally, source, "flameburst");
				}
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.FIRE,
		zMovePower = 140,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 488,
		accuracy = 100,
		basePower = 50,
		category = Category.PHYSICAL,
		//desc = "Has a 100% chance to raise the user's Speed by 1 stage.",
		//shortDesc = "100% chance to raise the user's Speed by 1.",
		id = Moves.FLAMECHARGE,
		isViable = true,
		//name = "Flame Charge",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 100,
			self = {
				boosts = new Boosts() {
					spe = 1
				}
			}
		},
		target = Target.Normal,
		type = Types.FIRE,
		zMovePower = 100,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 172,
		accuracy = 100,
		basePower = 60,
		category = Category.PHYSICAL,
		//desc = "Has a 10% chance to burn the target.",
		//shortDesc = "10% chance to burn the target. Thaws user.",
		id = Moves.FLAMEWHEEL,
		//name = "Flame Wheel",
		pp = 25,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true, defrost: true),
		secondary = new Secondary() {
			chance = 10,
			status = "brn"
		},
		target = Target.Normal,
		type = Types.FIRE,
		zMovePower = 120,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 53,
		accuracy = 100,
		basePower = 90,
		category = Category.SPECIAL,
		//desc = "Has a 10% chance to burn the target.",
		//shortDesc = "10% chance to burn the target.",
		id = Moves.FLAMETHROWER,
		isViable = true,
		//name = "Flamethrower",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 10,
			status = "brn"
		},
		target = Target.Normal,
		type = Types.FIRE,
		zMovePower = 175,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 394,
		accuracy = 100,
		basePower = 120,
		category = Category.PHYSICAL,
		//desc = "Has a 10% chance to burn the target. If the target Lost HP, the user takes recoil damage equal to 33% the HP lost by the target, rounded half up, but not less than 1 HP.",
		//shortDesc = "Has 33% recoil. 10% chance to burn. Thaws user.",
		id = Moves.FLAREBLITZ,
		isViable = true,
		//name = "Flare Blitz",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true, defrost: true),
		recoil = new int[] {33, 100},
		secondary = new Secondary() {
			chance = 10,
			status = "brn"
		},
		target = Target.Normal,
		type = Types.FIRE,
		zMovePower = 190,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 148,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Lowers the target'S accuracy by 1 stage.",
		//shortDesc = "Lowers the target'S accuracy by 1.",
		id = Moves.FLASH,
		//name = "Flash",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true),
		boosts = new Boosts() {
			accuracy = -1
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {evasion = 1},
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 430,
		accuracy = 100,
		basePower = 80,
		category = Category.SPECIAL,
		//desc = "Has a 10% chance to lower the target'S Special Defense by 1 stage.",
		//shortDesc = "10% chance to lower the target'S Sp. Def by 1.",
		id = Moves.FLASHCANNON,
		isViable = true,
		//name = "Flash Cannon",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 10,
			boosts = new Boosts() {
				spd = -1
			}
		},
		target = Target.Normal,
		type = Types.STEEL,
		zMovePower = 160,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 260,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the target'S Special Attack by 1 stage and confuses it.",
		//shortDesc = "Raises the target'S Sp. Atk by 1 and confuses it.",
		id = Moves.FLATTER,
		//name = "Flatter",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true, mystery: true),
		volatileStatus = "confusion",
		boosts = new Boosts() {
			spa = 1
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.DARK,
		zMoveBoost = new Boosts() {spd = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 705,
		accuracy = 90,
		basePower = 130,
		category = Category.SPECIAL,
		//desc = "Lowers the user's Special Attack by 2 stages.",
		//shortDesc = "Lowers the user's Sp. Atk by 2.",
		id = Moves.FLEURCANNON,
		isViable = true,
		//name = "Fleur Cannon",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		self = {
			boosts = new Boosts() {
				spa = -2
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.FAIRY,
		zMovePower = 195,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 374,
		accuracy = 100,
		basePower = 0,
		category = Category.PHYSICAL,
		//desc = "The power of this move is based on the user's held item. The held item is lost and it activates for the target If applicable. If there is no target Or the target Avoids this move by protecting itself, the user's held item is still lost. The user can regain a thrown item with Recycle or the Ability Harvest. Fails if the user has no held item, if the held item cannot be thrown, if the user is under the effect of Embargo or Magic Room, or if the user has the Ability Klutz.",
		//shortDesc = "Flings the user's item at the target. Power varies.",
		id = Moves.FLING,
		//name = "Fling",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, mystery: true),
		onPrepareHit = function (target, source, move) {
			if (source.ignoringItem()) return false;
			let item = source.getItem();
			if (!this.singleEvent("TakeItem", item, source.itemData, source, source, move, item)) return false;
			if (!item.fling) return false;
			move.basePower = item.fling.basePower;
			if (item.isBerry) {
				move.onHit = function (foe) {
					if (this.singleEvent("Eat", item, null, foe, null, null)) {
						this.runEvent("EatItem", foe, null, null, item);
					}
					if (item.onEat) foe.ateBerry = true;
				};
			} else if (item.fling.effect) {
				move.onHit = item.fling.effect;
			} else {
				if (!move.secondaries) move.secondaries = [];
				if (item.fling.status) {
					move.secondaries.push({status = item.fling.status});
				} else if (item.fling.volatileStatus) {
					move.secondaries.push({volatileStatus = item.fling.volatileStatus});
				}
			}
			source.setItem("");
			source.lastItem = item.id;
			source.usedItemThisTurn = true;
			this.add("-enditem", source, item.name, '[from] move = Fling');
			this.runEvent("AfterUseItem", source, null, null, item);
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.DARK,
		zMovePower = 100,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 666,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The target Restores 1/2 of its maximum HP, rounded half up. If the terrain is Grassy Terrain, the target Instead restores 2/3 of its maximum HP, rounded half down.",
		//shortDesc = "Heals the target By 50% of its max HP.",
		id = Moves.FLORALHEALING,
		//name = "Floral Healing",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, heal: true, mystery: true),
		onHit = function (target) {
			if (this.isTerrain("grassyterrain")) {
				return this.heal(this.modify(target.Maxhp, 0.667)); // TODO = find out the real value
			} else {
				return this.heal(Math.ceil(target.Maxhp * 0.5));
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.FAIRY,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 579,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the Defense of all Grass-type Pokemon on the field by 1 stage.",
		//shortDesc = "Raises Defense by 1 of all active Grass types.",
		id = Moves.FLOWERSHIELD,
		//name = "Flower Shield",
		pp = 10,
		priority = 0,
		flags = new Flags (distance: true),
		onHitField = function (target, source) {
			let targets = [];
			for (const side of this.sides) {
				for (const pokemon of side.active) {
					if (pokemon && pokemon.isActive && pokemon.hasType("Grass")) {
						// This move affects every Grass-type Pokemon in play.
						targets.push(pokemon);
					}
				}
			}
			let success = false;
			for (const target Of targets) {
				success = this.boost({def = 1}, target, source, this.getMove('Flower Shield')) || success;
			}
			return success;
		},
		//secondary = false,
		target = Target.All,
		type = Types.FAIRY,
		zMoveBoost = new Boosts() {def = 1},
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 19,
		accuracy = 95,
		basePower = 90,
		category = Category.PHYSICAL,
		//desc = "This attack charges on the first turn and executes on the second. On the first turn, the user avoids all attacks other than Gust, Hurricane, Sky Uppercut, Smack Down, Thousand Arrows, Thunder, and Twister. If the user is holding a Power Herb, the move completes in one turn.",
		//shortDesc = "Flies up on first turn, then strikes the next turn.",
		id = Moves.FLY,
		//name = "Fly",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, charge: true, protect: true, mirror: true, gravity: true, distance: true),
		onTry = function (attacker, defender, move) {
			if (attacker.removeVolatile(move.id)) {
				return;
			}
			this.add('-prepare', attacker, move.name, defender);
			if (!this.runEvent("ChargeMove", attacker, defender, move)) {
				this.add('-anim', attacker, move.name, defender);
				return;
			}
			attacker.addVolatile("twoturnmove", defender);
			return null;
		},
		effect = {
			duration = 2,
			onTryImmunity = function (target, source, move) {
				if (move.id === "gust" || move.id === "twister") {
					return;
				}
				if (move.id === "skyuppercut" || move.id === "thunder" || move.id === "hurricane" || move.id === "smackdown" || move.id === "thousandarrows" || move.id === "helpinghand") {
					return;
				}
				if (source.hasAbility("noguard") || target.HasAbility("noguard")) {
					return;
				}
				if (source.volatiles["lockon"] && target === source.volatiles["lockon"].source) return;
				return false;
			},
			onSourceModifyDamage = function (damage, source, target, move) {
				if (move.id === "gust" || move.id === "twister") {
					return this.chainModify(2);
				}
			}
		},
		//secondary = false,
		target = Target.Any,
		type = Types.FLYING,
		zMovePower = 175,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 560,
		accuracy = 95,
		basePower = 100,
		category = Category.PHYSICAL,
		//desc = "This move combines Flying in its type effectiveness against the target. Damage doubles and no accuracy check is done if the target Has used Minimize while active.",
		//shortDesc = "Combines Flying in its type effectiveness.",
		id = Moves.FLYINGPRESS,
		//name = "Flying Press",
		pp = 10,
		flags = new Flags (contact: true, protect: true, mirror: true, gravity: true, distance: true, nonsky: true),
		onEffectiveness = function (typeMod, type, move) {
			// @ts-ignore
			return typeMod + this.getEffectiveness("Flying", type);
		},
		priority = 0,
		//secondary = false,
		target = Target.Any,
		type = Types.FIGHTING,
		zMovePower = 170,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 411,
		accuracy = 70,
		basePower = 120,
		category = Category.SPECIAL,
		//desc = "Has a 10% chance to lower the target'S Special Defense by 1 stage.",
		//shortDesc = "10% chance to lower the target'S Sp. Def by 1.",
		id = Moves.FOCUSBLAST,
		isViable = true,
		//name = "Focus Blast",
		pp = 5,
		priority = 0,
		flags = new Flags (bullet: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 10,
			boosts = new Boosts() {
				spd = -1
			}
		},
		target = Target.Normal,
		type = Types.FIGHTING,
		zMovePower = 190,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 116,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the user's chance for a critical hit by 2 stages. Fails if the user already has the effect. Baton Pass can be used to transfer this effect to an ally.",
		//shortDesc = "Raises the user's critical hit ratio by 2.",
		id = Moves.FOCUSENERGY,
		//name = "Focus Energy",
		pp = 30,
		priority = 0,
		flags = new Flags (snatch: true),
		volatileStatus = "focusenergy",
		effect = {
			onStart = function (target, source, effect) {
				if (effect && effect.id === "zpower") {
					this.add('-start', target, 'move = Focus Energy', '[zeffect]');
				} else if (effect && (effect.id === "transform" || effect.id === "imposter")) {
					this.add('-start', target, 'move = Focus Energy', '[silent]');
				} else {
					this.add('-start', target, 'move = Focus Energy');
				}
			},
			onModifyCritRatio = function (critRatio) {
				return critRatio + 2;
			}
		},
		//secondary = false,
		target = Target.Self,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {accuracy = 1},
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 264,
		accuracy = 100,
		basePower = 150,
		category = Category.PHYSICAL,
		//desc = "The user loses its focus and does nothing if it is hit by a damaging attack this turn before it can execute the move.",
		//shortDesc = "Fails if the user takes damage before it hits.",
		id = Moves.FOCUSPUNCH,
		isViable = true,
		//name = "Focus Punch",
		pp = 20,
		priority = -3,
		flags = new Flags (contact: true, protect: true, punch: true),
		beforeTurnCallback = function (pokemon) {
			pokemon.addVolatile("focuspunch");
		},
		beforeMoveCallback = function (pokemon) {
			if (pokemon.volatiles["focuspunch"] && pokemon.volatiles["focuspunch"].lostFocus) {
				this.add("cant", pokemon, 'Focus Punch', 'Focus Punch');
				return true;
			}
		},
		effect = {
			duration = 1,
			onStart = function (pokemon) {
				this.add('-singleturn', pokemon, 'move = Focus Punch');
			},
			onHit = function (pokemon, source, move) {
				if (move.category !== "Status") {
					pokemon.volatiles["focuspunch"].lostFocus = true;
				}
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.FIGHTING,
		zMovePower = 200,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 266,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Until the end of the turn, all single-target Attacks from the foe's team are redirected to the user if they are in range. Such attacks are redirected to the user before they can be reflected by Magic Coat or the Ability Magic Bounce, or drawn in by the Abilities Lightning Rod or Storm Drain. Fails if it is not a Double or Triple Battle.",
		//shortDesc = "The foes' moves TARGET The user on the turn used.",
		id = Moves.FOLLOWME,
		//name = "Follow Me",
		pp = 20,
		priority = 2,
		flags = new Flags (),
		volatileStatus = "followme",
		onTryHit = function (target) {
			if (target.Side.active.length < 2) return false;
		},
		effect = {
			duration = 1,
			onStart = function (target, source, effect) {
				if (effect && effect.id === "zpower") {
					this.add('-singleturn', target, 'move = Follow Me', '[zeffect]');
				} else {
					this.add('-singleturn', target, 'move = Follow Me');
				}
			},
			onFoeRedirectTargetPRiority = 1,
			onFoeRedirectTarget = function (target, source, source2, move) {
				if (!this.effectData.target.IsSkyDropped() && this.validTarget(This.effectData.target, source, move.target)) {
					this.debug("Follow Me redirected target Of move");
					return this.effectData.target;
				}
			}
		},
		//secondary = false,
		target = Target.Self,
		type = Types.NORMAL,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 395,
		accuracy = 100,
		basePower = 60,
		category = Category.PHYSICAL,
		//desc = "Has a 30% chance to paralyze the target.",
		//shortDesc = "30% chance to paralyze the target.",
		id = Moves.FORCEPALM,
		//name = "Force Palm",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 30,
			status = "par"
		},
		target = Target.Normal,
		type = Types.FIGHTING,
		zMovePower = 120,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 193,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Causes the target To have its positive evasiveness stat stage ignored while it is active. Normal- and Fighting-type attacks can hit the target If it is a Ghost type. The effect ends when the target Is no longer active. Fails if the target Is already affected, or affected by Miracle Eye or Odor Sleuth.",
		//shortDesc = "Fighting, Normal hit Ghost. Evasiveness ignored.",
		id = Moves.FORESIGHT,
		//name = "Foresight",
		pp = 40,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true, authentic: true),
		volatileStatus = "foresight",
		onTryHit = function (target) {
			if (target.Volatiles["miracleeye"]) return false;
		},
		effect = {
			noCopy = true,
			onStart = function (pokemon) {
				this.add('-start', pokemon, "Foresight");
			},
			onNegateImmunity = function (pokemon, type) {
				if (pokemon.hasType("Ghost") && ["Normal", "Fighting"].includes(type)) return false;
			},
			onModifyBoost = function (boosts) {
				if (boosts.evasion && boosts.evasion > 0) {
					boosts.evasion = 0;
				}
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMoveEffect = "crit2",
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 571,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Causes the Grass type to be added to the target, effectively making it have two or three types. Fails if the target Is already a Grass type. If Trick-or-Treat adds a type to the target, it replaces the type added by this move and vice versa.",
		//shortDesc = "Adds Grass to the target'S type(s).",
		id = Moves.FORESTSCURSE,
		//name = "Forest's Curse",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true, mystery: true),
		onHit = function (target) {
			if (target.HasType("Grass")) return false;
			if (!target.AddType("Grass")) return false;
			this.add('-start', target, "typeadd", "Grass", '[from] move = Forest\'s Curse');
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.GRASS,
		zMoveBoost = new Boosts() {atk = 1, def = 1, spa = 1, spd = 1, spe = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 492,
		accuracy = 100,
		basePower = 95,
		category = Category.PHYSICAL,
		//desc = "Damage is calculated using the target'S Attack stat, including stat stage changes. The user's Ability, item, and burn are used as normal.",
		//shortDesc = "Uses target'S Attack stat in damage calculation.",
		id = Moves.FOULPLAY,
		isViable = true,
		//name = "Foul Play",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		useTargetOFfensive = true,
		//secondary = false,
		target = Target.Normal,
		type = Types.DARK,
		zMovePower = 175,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 573,
		accuracy = 100,
		basePower = 70,
		category = Category.SPECIAL,
		//desc = "Has a 10% chance to freeze the target. This move's type effectiveness against Water is changed to be super effective no matter what this move's type is.",
		//shortDesc = "10% chance to freeze. Super effective on Water.",
		id = Moves.FREEZEDRY,
		isViable = true,
		//name = "Freeze-Dry",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		onEffectiveness = function (typeMod, type) {
			if (type === "Water") return 1;
		},
		secondary = new Secondary() {
			chance = 10,
			status = "frz"
		},
		target = Target.Normal,
		type = Types.ICE,
		zMovePower = 140,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 553,
		accuracy = 90,
		basePower = 140,
		category = Category.PHYSICAL,
		//desc = "Has a 30% chance to paralyze the target. This attack charges on the first turn and executes on the second. If the user is holding a Power Herb, the move completes in one turn.",
		//shortDesc = "Charges turn 1. Hits turn 2. 30% paralyze.",
		id = Moves.FREEZESHOCK,
		//name = "Freeze Shock",
		pp = 5,
		priority = 0,
		flags = new Flags (charge: true, protect: true, mirror: true),
		onTry = function (attacker, defender, move) {
			if (attacker.removeVolatile(move.id)) {
				return;
			}
			this.add('-prepare', attacker, move.name, defender);
			if (!this.runEvent("ChargeMove", attacker, defender, move)) {
				this.add('-anim', attacker, move.name, defender);
				return;
			}
			attacker.addVolatile("twoturnmove", defender);
			return null;
		},
		secondary = new Secondary() {
			chance = 30,
			status = "par"
		},
		target = Target.Normal,
		type = Types.ICE,
		zMovePower = 200,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 338,
		accuracy = 90,
		basePower = 150,
		category = Category.SPECIAL,
		//desc = "If this move is successful, the user must recharge on the following turn and cannot make a move.",
		//shortDesc = "User cannot move next turn.",
		id = Moves.FRENZYPLANT,
		//name = "Frenzy Plant",
		pp = 5,
		priority = 0,
		flags = new Flags (recharge: true, protect: true, mirror: true, nonsky: true),
		self = {
			volatileStatus = "mustrecharge"
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.GRASS,
		zMovePower = 200,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 524,
		accuracy = 90,
		basePower = 60,
		category = Category.SPECIAL,
		//desc = "This move is always a critical hit unless the target Is under the effect of Lucky Chant or has the Abilities Battle Armor or Shell Armor.",
		//shortDesc = "Always results in a critical hit.",
		id = Moves.FROSTBREATH,
		//name = "Frost Breath",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		willCrit = true,
		//secondary = false,
		target = Target.Normal,
		type = Types.ICE,
		zMovePower = 120,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 218,
		accuracy = 100,
		basePower = 0,
		basePowerCallback = function (pokemon) {
			return Math.floor(((255 - pokemon.happiness) * 10) / 25) || 1;
		},
		category = Category.PHYSICAL,
		//desc = "Power is equal to the greater of ((255 - user's Happiness) * 2/5), rounded down, or 1.",
		//shortDesc = "Max 102 power at minimum Happiness.",
		id = Moves.FRUSTRATION,
		isViable = true,
		//name = "Frustration",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 160,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 31,
		accuracy = 85,
		basePower = 15,
		category = Category.PHYSICAL,
		//desc = "Hits two to five times. Has a 1/3 chance to hit two or three times, and a 1/6 chance to hit four or five times. If one of the hits breaks the target'S substitute, it will take damage for the remaining hits. If the user has the Ability Skill Link, this move will always hit five times.",
		//shortDesc = "Hits 2-5 times in one turn.",
		id = Moves.FURYATTACK,
		//name = "Fury Attack",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		multihit = new int[] {2, 5},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 100,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 210,
		accuracy = 95,
		basePower = 40,
		basePowerCallback = function (pokemon, target, move) {
			if (!pokemon.volatiles.furycutter) {
				pokemon.addVolatile("furycutter");
			}
			return this.clampIntRange(move.basePower * pokemon.volatiles.furycutter.multiplier, 1, 160);
		},
		category = Category.PHYSICAL,
		//desc = "Power doubles with each successful hit, up to a maximum of 160 power; power is reset if the move misses or another move is used.",
		//shortDesc = "Power doubles with each hit, up to 160.",
		id = Moves.FURYCUTTER,
		//name = "Fury Cutter",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		onHit = function (target, source) {
			source.addVolatile("furycutter");
		},
		effect = {
			duration = 2,
			onStart = function () {
				this.effectData.multiplier = 1;
			},
			onRestart = function () {
				if (this.effectData.multiplier < 4) {
					this.effectData.multiplier <<= 1;
				}
				this.effectData.duration = 2;
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.BUG,
		zMovePower = 100,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 154,
		accuracy = 80,
		basePower = 18,
		category = Category.PHYSICAL,
		//desc = "Hits two to five times. Has a 1/3 chance to hit two or three times, and a 1/6 chance to hit four or five times. If one of the hits breaks the target'S substitute, it will take damage for the remaining hits. If the user has the Ability Skill Link, this move will always hit five times.",
		//shortDesc = "Hits 2-5 times in one turn.",
		id = Moves.FURYSWIPES,
		//name = "Fury Swipes",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		multihit = new int[] {2, 5},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 100,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 559,
		accuracy = 100,
		basePower = 100,
		category = Category.PHYSICAL,
		//desc = "Power doubles if the last move used by any Pokemon this turn was Fusion Flare.",
		//shortDesc = "Power doubles if used after Fusion Flare.",
		id = Moves.FUSIONBOLT,
		isViable = true,
		//name = "Fusion Bolt",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		onBasePowerPriority = 4,
		onBasePower = function (basePower, pokemon) {
			for (const active of pokemon.side.active) {
				if (active && active.moveThisTurn === "fusionflare") {
					this.debug('double power');
					return this.chainModify(2);
				}
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.ELECTRIC,
		zMovePower = 180,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 558,
		accuracy = 100,
		basePower = 100,
		category = Category.SPECIAL,
		//desc = "Power doubles if the last move used by any Pokemon this turn was Fusion Bolt.",
		//shortDesc = "Power doubles if used after Fusion Bolt.",
		id = Moves.FUSIONFLARE,
		isViable = true,
		//name = "Fusion Flare",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, defrost: true),
		onBasePowerPriority = 4,
		onBasePower = function (basePower, pokemon) {
			for (const active of pokemon.side.active) {
				if (active && active.moveThisTurn === "fusionbolt") {
					this.debug('double power');
					return this.chainModify(2);
				}
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.FIRE,
		zMovePower = 180,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 248,
		accuracy = 100,
		basePower = 120,
		category = Category.SPECIAL,
		//desc = "Deals damage two turns after this move is used. At the end of that turn, the damage is calculated at that time and dealt to the Pokemon at the position the target Had when the move was used. If the user is no longer active at the time, damage is calculated based on the user's natural Special Attack stat, types, and level, with no boosts from its held item or Ability. Fails if this move or Doom Desire is already in effect for the target'S position.",
		//shortDesc = "Hits two turns after being used.",
		id = Moves.FUTURESIGHT,
		//name = "Future Sight",
		pp = 10,
		priority = 0,
		flags = new Flags (),
		ignoreImmunity = true,
		isFutureMove = true,
		onTry = function (source, target) {
			target.Side.addSideCondition("futuremove");
			if (target.Side.sideConditions["futuremove"].positions[target.Position]) {
				return false;
			}
			target.Side.sideConditions["futuremove"].positions[target.Position] = {
				duration = 3,
				move = "futuresight",
				source = source,
				moveData = {
					id = "futuresight",
					//name = "Future Sight",
					accuracy = 100,
					basePower = 120,
					category = Category.SPECIAL,
					priority = 0,
					flags = new Flags (),
					ignoreImmunity = false,
					effectType = "Move",
					isFutureMove = true,
					type = "Psychic"
				}
			};
			this.add('-start', source, 'move = Future Sight');
			return null;
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.PSYCHIC,
		zMovePower = 190,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 380,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Causes the target'S Ability to be rendered ineffective as long as it remains active. If the target Uses Baton Pass, the replacement will remain under this effect. Fails if the target'S Ability is Multitype or Stance Change.",
		//shortDesc = "Nullifies the target'S Ability.",
		id = Moves.GASTROACID,
		//name = "Gastro Acid",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true, mystery: true),
		volatileStatus = "gastroacid",
		onTryHit = function (pokemon) {
			let bannedAbilities = ["battlebond", "comatose", "disguise", "multitype", "powerconstruct", "rkssystem", "schooling", "shieldsdown", "stancechange"];
			if (bannedAbilities.includes(pokemon.ability)) {
				return false;
			}
		},
		effect = {
			// Ability suppression implemented in Pokemon.ignoringAbility() within sim/pokemon.js
			onStart = function (pokemon) {
				this.add('-endability', pokemon);
				this.singleEvent("End", this.getAbility(pokemon.ability), pokemon.abilityData, pokemon, pokemon, "gastroacid");
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.POISON,
		zMoveBoost = new Boosts() {spe = 1},
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 544,
		accuracy = 85,
		basePower = 50,
		category = Category.PHYSICAL,
		//desc = "Hits twice. If the first hit breaks the target'S substitute, it will take damage for the second hit.",
		//shortDesc = "Hits 2 times in one turn.",
		id = Moves.GEARGRIND,
		isViable = true,
		//name = "Gear Grind",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		multihit = 2,
		//secondary = false,
		target = Target.Normal,
		type = Types.STEEL,
		zMovePower = 180,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 674,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the Attack and Special Attack of Pokemon on the user's side with the Abilities Plus or Minus by 1 stage.",
		//shortDesc = "Raises Atk, Sp. Atk of allies with Plus/Minus by 1.",
		id = Moves.GEARUP,
		//name = "Gear Up",
		pp = 20,
		priority = 0,
		flags = new Flags (snatch: true, authentic: true),
		onHitSide = function (side, source) {
			let targets = [];
			for (let p in side.active) {
				if (side.active[p].hasAbility(["plus", "minus"])) {
					targets.push(side.active[p]);
				}
			}
			if (!targets.length) return false;
			for (const target Of targets) {
				this.boost({atk = 1, spa = 1}, target, source, 'move = Gear Up');
			}
		},
		//secondary = false,
		target = Target.AllySide,
		type = Types.STEEL,
		zMoveBoost = new Boosts() {spa = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 703,
		//accuracy = true,
		basePower = 185,
		category = Category.SPECIAL,
		//desc = "The terrain becomes Psychic Terrain.",
		//shortDesc = "Summons Psychic Terrain.",
		id = Moves.GENESISSUPERNOVA,
		//name = "Genesis Supernova",
		pp = 1,
		priority = 0,
		flags = new Flags (),
		isZ = "mewniumz",
		secondary = new Secondary() {
			chance = 100,
			self = {
				onHit = function () {
					this.setTerrain("psychicterrain");
				}
			}
		},
		target = Target.Normal,
		type = Types.PSYCHIC,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 601,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the user's Special Attack, Special Defense, and Speed by 2 stages. This attack charges on the first turn and executes on the second. If the user is holding a Power Herb, the move completes in one turn.",
		//shortDesc = "Charges, then raises SpA, SpD, Spe by 2 turn 2.",
		id = Moves.GEOMANCY,
		isViable = true,
		//name = "Geomancy",
		pp = 10,
		priority = 0,
		flags = new Flags (charge: true, nonsky: true),
		onTry = function (attacker, defender, move) {
			if (attacker.removeVolatile(move.id)) {
				return;
			}
			this.add('-prepare', attacker, move.name, defender);
			if (!this.runEvent("ChargeMove", attacker, defender, move)) {
				this.add('-anim', attacker, move.name, defender);
				attacker.removeVolatile(move.id);
				return;
			}
			attacker.addVolatile("twoturnmove", defender);
			return null;
		},
		boosts = new Boosts() {
			spa = 2,
			spd = 2,
			spe = 2
		},
		//secondary = false,
		target = Target.Self,
		type = Types.FAIRY,
		zMoveBoost = new Boosts() {atk = 1, def = 1, spa = 1, spd = 1, spe = 1},
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 202,
		accuracy = 100,
		basePower = 75,
		category = Category.SPECIAL,
		//desc = "The user recovers 1/2 the HP lost by the target, rounded half up. If Big Root is held by the user, the HP recovered is 1.3x normal, rounded half down.",
		//shortDesc = "User recovers 50% of the damage dealt.",
		id = Moves.GIGADRAIN,
		isViable = true,
		//name = "Giga Drain",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, heal: true),
		drain = new int[] {1, 2},
		//secondary = false,
		target = Target.Normal,
		type = Types.GRASS,
		zMovePower = 140,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 416,
		accuracy = 90,
		basePower = 150,
		category = Category.PHYSICAL,
		//desc = "If this move is successful, the user must recharge on the following turn and cannot make a move.",
		//shortDesc = "User cannot move next turn.",
		id = Moves.GIGAIMPACT,
		//name = "Giga Impact",
		pp = 5,
		priority = 0,
		flags = new Flags (contact: true, recharge: true, protect: true, mirror: true),
		self = {
			volatileStatus = "mustrecharge"
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 200,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 646,
		//accuracy = true,
		basePower = 1,
		category = Category.PHYSICAL,
		//shortDesc = "Power is equal to the base move's Z-Power.",
		id = Moves.GIGAVOLTHAVOC,
		isViable = true,
		//name = "Gigavolt Havoc",
		pp = 1,
		priority = 0,
		flags = new Flags (),
		isZ = "electriumz",
		//secondary = false,
		target = Target.Normal,
		type = Types.ELECTRIC,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 549,
		accuracy = 95,
		basePower = 65,
		category = Category.SPECIAL,
		//desc = "Has a 100% chance to lower the target'S Speed by 1 stage.",
		//shortDesc = "100% chance to lower the foe(s) Speed by 1.",
		id = Moves.GLACIATE,
		//name = "Glaciate",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 100,
			boosts = new Boosts() {
				spe = -1
			}
		},
		target = Target.AllAdjacentFoes,
		type = Types.ICE,
		zMovePower = 120,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 137,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Paralyzes the target.",
		//shortDesc = "Paralyzes the target.",
		id = Moves.GLARE,
		isViable = true,
		//name = "Glare",
		pp = 30,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true),
		status = "par",
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {spd = 1},
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 447,
		accuracy = 100,
		basePower = 0,
		basePowerCallback = function (pokemon, target) {
			let targetWEight = target.GetWeight();
			if (targetWEight >= 200) {
				this.debug('120 bp');
				return 120;
			}
			if (targetWEight >= 100) {
				this.debug('100 bp');
				return 100;
			}
			if (targetWEight >= 50) {
				this.debug('80 bp');
				return 80;
			}
			if (targetWEight >= 25) {
				this.debug('60 bp');
				return 60;
			}
			if (targetWEight >= 10) {
				this.debug('40 bp');
				return 40;
			}
			this.debug('20 bp');
			return 20;
		},
		category = Category.SPECIAL,
		//desc = "Deals damage to the target Based on its weight. Power is 20 if less than 10 kg, 40 if less than 25 kg, 60 if less than 50 kg, 80 if less than 100 kg, 100 if less than 200 kg, and 120 if greater than or equal to 200 kg.",
		//shortDesc = "More power the heavier the target.",
		id = Moves.GRASSKNOT,
		isViable = true,
		//name = "Grass Knot",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true, nonsky: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.GRASS,
		zMovePower = 160,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 520,
		accuracy = 100,
		basePower = 80,
		basePowerCallback = function (target, source, move) {
			if (["waterpledge", "firepledge"].includes(move.sourceEffect)) {
				this.add('-combine');
				return 150;
			}
			return 80;
		},
		category = Category.SPECIAL,
		//desc = "If one of the user's allies chose to use Fire Pledge or Water Pledge this turn and has not moved yet, it takes its turn immediately after the user and the user's move does nothing. If combined with Fire Pledge, the ally uses Fire Pledge with 150 Base Power and a sea of fire appears on the target'S side for 4 turns, which causes damage to non-Fire types equal to 1/8 of their maximum HP, rounded down, at the end of each turn during effect. If combined with Water Pledge, the ally uses Grass Pledge with 150 Base Power and a swamp appears on the target'S side for 4 turns, which quarters the Speed of each Pokemon on that side. When used as a combined move, this move gains STAB no matter what the user's type is. This move does not consume the user's Grass Gem.",
		//shortDesc = "Use with Fire or Water Pledge for added effect.",
		id = Moves.GRASSPLEDGE,
		//name = "Grass Pledge",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, nonsky: true),
		onPrepareHit = function (target, source, move) {
			for (const action of this.queue) {
				// @ts-ignore
				if (!action.move || !action.pokemon || !action.pokemon.isActive || action.pokemon.fainted) continue;
				// @ts-ignore
				if (action.pokemon.side === source.side && ["waterpledge", "firepledge"].includes(action.move.id)) {
					// @ts-ignore
					this.prioritizeAction(action);
					this.add('-waiting', source, action.pokemon);
					return null;
				}
			}
		},
		onModifyMove = function (move) {
			if (move.sourceEffect === "waterpledge") {
				move.type = "Grass";
				move.hasSTAB = true;
			}
			if (move.sourceEffect === "firepledge") {
				move.type = "Fire";
				move.hasSTAB = true;
			}
		},
		onHit = function (target, source, move) {
			if (move.sourceEffect === "waterpledge") {
				target.Side.addSideCondition("grasspledge");
			}
			if (move.sourceEffect === "firepledge") {
				target.Side.addSideCondition("firepledge");
			}
		},
		effect = {
			duration = 4,
			onStart = function (targetSIde) {
				this.add('-sidestart', targetSIde, 'Grass Pledge');
			},
			onEnd = function (targetSIde) {
				this.add('-sideend', targetSIde, 'Grass Pledge');
			},
			onModifySpe = function (spe, pokemon) {
				return this.chainModify(0.25);
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.GRASS,
		zMovePower = 160,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 320,
		accuracy = 55,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Causes the target To fall asleep.",
		//shortDesc = "Puts the target To sleep.",
		id = Moves.GRASSWHISTLE,
		//name = "Grass Whistle",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true, sound: true, authentic: true),
		status = "slp",
		//secondary = false,
		target = Target.Normal,
		type = Types.GRASS,
		zMoveBoost = new Boosts() {spe = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 580,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "For 5 turns, the terrain becomes Grassy Terrain. During the effect, the power of Grass-type attacks used by grounded Pokemon is multiplied by 1.5, the power of Bulldoze, Earthquake, and Magnitude used against grounded Pokemon is multiplied by 0.5, and grounded Pokemon have 1/16 of their maximum HP, rounded down, restored at the end of each turn, including the last turn. Camouflage transforms the user into a Grass type, Nature Power becomes Energy Ball, and Secret Power has a 30% chance to cause sleep. Fails if the current terrain is Grassy Terrain.",
		//shortDesc = "5 turns. Grounded = +Grass power,+1/16 max HP.",
		id = Moves.GRASSYTERRAIN,
		//name = "Grassy Terrain",
		pp = 10,
		priority = 0,
		flags = new Flags (nonsky: true),
		terrain = "grassyterrain",
		effect = {
			duration = 5,
			durationCallback = function (source, effect) {
				if (source && source.hasItem("terrainextender")) {
					return 8;
				}
				return 5;
			},
			onBasePower = function (basePower, attacker, defender, move) {
				let weakenedMoves = ["earthquake", "bulldoze", "magnitude"];
				if (weakenedMoves.INCLUDES(move.id)) {
					this.debug('move weakened by grassy terrain');
					return this.chainModify(0.5);
				}
				if (move.type === "Grass" && attacker.isGrounded()) {
					this.debug('grassy terrain boost');
					return this.chainModify(1.5);
				}
			},
			onStart = function (battle, source, effect) {
				if (effect && effect.effectType === "Ability") {
					this.add('-fieldstart', 'move = Grassy Terrain', '[from] ability = ' + effect, '[of] ' + source);
				} else {
					this.add('-fieldstart', 'move = Grassy Terrain');
				}
			},
			onResidualOrder = 5,
			onResidualSubOrder = 3,
			onResidual = function () {
				this.eachEvent("Terrain");
			},
			onTerrain = function (pokemon) {
				if (pokemon.isGrounded() && !pokemon.isSemiInvulnerable()) {
					this.debug('Pokemon is grounded, healing through Grassy Terrain.');
					this.heal(pokemon.maxhp / 16, pokemon, pokemon);
				}
			},
			onEnd = function () {
				if (!this.effectData.duration) this.eachEvent("Terrain");
				this.add('-fieldend', 'move = Grassy Terrain');
			}
		},
		//secondary = false,
		target = Target.All,
		type = Types.GRASS,
		zMoveBoost = new Boosts() {def = 1},
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 356,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "For 5 turns, the evasiveness of all active Pokemon is multiplied by 0.6. At the time of use, Bounce, Fly, Magnet Rise, Sky Drop, and Telekinesis end immediately for all active Pokemon. During the effect, Bounce, Fly, Flying Press, High Jump Kick, Jump Kick, Magnet Rise, Sky Drop, Splash, and Telekinesis are prevented from being used by all active Pokemon. Ground-type attacks, Spikes, Toxic Spikes, Sticky Web, and the Ability Arena Trap can affect Flying types or Pokemon with the Ability Levitate. Fails if this move is already in effect.",
		//shortDesc = "For 5 turns, negates all Ground immunities.",
		id = Moves.GRAVITY,
		//name = "Gravity",
		pp = 5,
		priority = 0,
		flags = new Flags (nonsky: true),
		pseudoWeather = "gravity",
		effect = {
			duration = 5,
			durationCallback = function (source, effect) {
				if (source && source.hasAbility("persistent")) {
					this.add('-activate', source, 'ability = Persistent', effect);
					return 7;
				}
				return 5;
			},
			onStart = function () {
				this.add('-fieldstart', 'move = Gravity');
				for (const pokemon of this.sides[0].active.concat(this.sides[1].active)) {
					let applies = false;
					if (pokemon.removeVolatile("bounce") || pokemon.removeVolatile("fly")) {
						applies = true;
						this.cancelMove(pokemon);
						pokemon.removeVolatile("twoturnmove");
					}
					if (pokemon.volatiles["skydrop"]) {
						applies = true;
						this.cancelMove(pokemon);

						if (pokemon.volatiles["skydrop"].source) {
							this.add('-end', pokemon.volatiles["twoturnmove"].source, 'Sky Drop', '[interrupt]');
						}
						pokemon.removeVolatile("skydrop");
						pokemon.removeVolatile("twoturnmove");
					}
					if (pokemon.volatiles["magnetrise"]) {
						applies = true;
						delete pokemon.volatiles["magnetrise"];
					}
					if (pokemon.volatiles["telekinesis"]) {
						applies = true;
						delete pokemon.volatiles["telekinesis"];
					}
					if (applies) this.add('-activate', pokemon, 'move = Gravity');
				}
			},
			onModify//Accuracy = function (accuracy) {
				if (typeof accuracy !== "number") return;
				return accuracy * 5 / 3;
			},
			onDisableMove = function (pokemon) {
				for (const moveSlOT of pokemon.moveSlOTS) {
					if (this.getMove(moveSlOT.id).flags["gravity"]) {
						pokemon.disableMove(moveSlOT.id);
					}
				}
			},
			// groundedness implemented in battle.engine.js:BattlePokemon#isGrounded
			onBeforeMovePriority = 6,
			onBeforeMove = function (pokemon, target, move) {
				if (move.flags["gravity"]) {
					this.add("cant", pokemon, 'move = Gravity', move);
					return false;
				}
			},
			onResidualOrder = 22,
			onEnd = function () {
				this.add('-fieldend', 'move = Gravity');
			}
		},
		//secondary = false,
		target = Target.All,
		type = Types.PSYCHIC,
		zMoveBoost = new Boosts() {spa = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 45,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Lowers the target'S Attack by 1 stage.",
		//shortDesc = "Lowers the foe(s) Attack by 1.",
		id = Moves.GROWL,
		//name = "Growl",
		pp = 40,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true, sound: true, authentic: true),
		boosts = new Boosts() {
			atk = -1
		},
		//secondary = false,
		target = Target.AllAdjacentFoes,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {def = 1},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 74,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the user's Attack and Special Attack by 1 stage. If the weather is Sunny Day, raises the user's Attack and Special Attack by 2 stages.",
		//shortDesc = "Raises user's Attack and Sp. Atk by 1; 2 in Sun.",
		id = Moves.GROWTH,
		//name = "Growth",
		pp = 20,
		priority = 0,
		flags = new Flags (snatch: true),
		onModifyMove = function (move) {
			if (this.isWeather(["sunnyday", "desolateland"])) move.boosts = {atk = 2, spa = 2};
		},
		boosts = new Boosts() {
			atk = 1,
			spa = 1
		},
		//secondary = false,
		target = Target.Self,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {spa = 1},
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 288,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Until the user's next turn, if a foe's attack knocks the user out, that foe loses all remaining PP for that attack.",
		//shortDesc = "If the user faints, the attack used loses all its PP.",
		id = Moves.GRUDGE,
		//name = "Grudge",
		pp = 5,
		priority = 0,
		flags = new Flags (authentic: true),
		volatileStatus = "grudge",
		effect = {
			onStart = function (pokemon) {
				this.add('-singlemove', pokemon, "Grudge");
			},
			onFaint = function (target, source, effect) {
				if (!source || source.fainted || !effect) return;
				if (effect.effectType === "Move" && !effect.isFutureMove && source.lastMove) {
					for (const moveSlOT of source.moveSlOTS) {
						if (moveSlOT.id === source.lastMove.id) {
							moveSlOT.pp = 0;
							this.add('-activate', source, 'move = Grudge', this.getMove(source.lastMove.id).name);
						}
					}
				}
			},
			onBeforeMovePriority = 100,
			onBeforeMove = function (pokemon) {
				this.debug('removing Grudge before attack');
				pokemon.removeVolatile("grudge");
			}
		},
		//secondary = false,
		target = Target.Self,
		type = Types.GHOST,
		zMoveEffect = "redirect",
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 470,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user and the target Have their Defense and Special Defense stats set to be equal to the average of the user and the target'S Defense and Special Defense stats, respectively, rounded down. Stat stage changes are unaffected.",
		//shortDesc = "Averages Defense and Sp. Def stats with target.",
		id = Moves.GUARDSPLIT,
		//name = "Guard Split",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mystery: true),
		onHit = function (target, source) {
			let newdef = Math.floor((target.Stats.def + source.stats.def) / 2);
			target.Stats.def = newdef;
			source.stats.def = newdef;
			let newspd = Math.floor((target.Stats.spd + source.stats.spd) / 2);
			target.Stats.spd = newspd;
			source.stats.spd = newspd;
			this.add('-activate', source, 'move = Guard Split', '[of] ' + target);
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.PSYCHIC,
		zMoveBoost = new Boosts() {spe = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 385,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user swaps its Defense and Special Defense stat stage changes with the target.",
		//shortDesc = "Swaps Defense and Sp. Def changes with target.",
		id = Moves.GUARDSWAP,
		//name = "Guard Swap",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, authentic: true, mystery: true),
		onHit = function (target, source) {
			let targetBOosts = {};
			let sourceBoosts = {};

			for (const stat of ["def", "spd"]) {
				targetBOosts[stat] = target.Boosts[stat];
				sourceBoosts[stat] = source.boosts[stat];
			}

			source.setBoost(targetBOosts);
			target.SetBoost(sourceBoosts);

			this.add('-swapboost', source, target, 'def, spd', '[from] move = Guard Swap');
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.PSYCHIC,
		zMoveBoost = new Boosts() {spe = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 698,
		//accuracy = true,
		basePower = 0,
		damageCallback = function (pokemon, target) {
			if (target.Volatiles["banefulbunker"] || target.Volatiles["kingsshield"] || target.Side.sideConditions["matblock"] || target.Volatiles["protect"] || target.Volatiles["spikyshield"]) {
				this.add('-zbroken', target);
				return this.clampIntRange(Math.ceil(Math.floor(target.Hp * 3 / 4) / 4 - 0.5), 1);
			}
			return this.clampIntRange(Math.floor(target.Hp * 3 / 4), 1);
		},
		category = Category.SPECIAL,
		//desc = "Deals damage to the target Equal to 3/4 of its current HP, rounded down, but not less than 1 HP.",
		//shortDesc = "Does damage equal to 3/4 target'S current HP.",
		id = Moves.GUARDIANOFALOLA,
		//name = "Guardian of Alola",
		pp = 1,
		priority = 0,
		flags = new Flags (),
		isZ = "tapuniumz",
		//secondary = false,
		target = Target.Normal,
		type = Types.FAIRY,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 12,
		accuracy = 30,
		basePower = 0,
		category = Category.PHYSICAL,
		//desc = "Deals damage to the target Equal to the target'S maximum HP. Ignores accuracy and evasiveness modifiers. This attack's accuracy is equal to (user's level - target'S level + 30)%, and fails if the target Is at a higher level. Pokemon with the Ability Sturdy are immune.",
		//shortDesc = "OHKOs the target. Fails if user is a lower level.",
		id = Moves.GUILLOTINE,
		//name = "Guillotine",
		pp = 5,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		ohko = true,
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 180,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 441,
		accuracy = 80,
		basePower = 120,
		category = Category.PHYSICAL,
		//desc = "Has a 30% chance to poison the target.",
		//shortDesc = "30% chance to poison the target.",
		id = Moves.GUNKSHOT,
		isViable = true,
		//name = "Gunk Shot",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 30,
			status = "psn"
		},
		target = Target.Normal,
		type = Types.POISON,
		zMovePower = 190,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 16,
		accuracy = 100,
		basePower = 40,
		category = Category.SPECIAL,
		//desc = "Damage doubles if the target Is using Bounce, Fly, or Sky Drop.",
		//shortDesc = "Power doubles during Fly, Bounce, and Sky Drop.",
		id = Moves.GUST,
		//name = "Gust",
		pp = 35,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, distance: true),
		//secondary = false,
		target = Target.Any,
		type = Types.FLYING,
		zMovePower = 100,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 360,
		accuracy = 100,
		basePower = 0,
		basePowerCallback = function (pokemon, target) {
			let power = (Math.floor(25 * target.GetStat("spe") / pokemon.getStat("spe")) || 1);
			if (power > 150) power = 150;
			this.debug("" + power + ' bp');
			return power;
		},
		category = Category.PHYSICAL,
		//desc = "Power is equal to (25 * target'S current Speed / user's current Speed), rounded down, + 1, but not more than 150.",
		//shortDesc = "More power the slower the user than the target.",
		id = Moves.GYROBALL,
		isViable = true,
		//name = "Gyro Ball",
		pp = 5,
		priority = 0,
		flags = new Flags (bullet: true, contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.STEEL,
		zMovePower = 160,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 258,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "For 5 turns, the weather becomes Hail. At the end of each turn except the last, all active Pokemon lose 1/16 of their maximum HP, rounded down, unless they are an Ice type, or have the Abilities Ice Body, Magic Guard, Overcoat, or Snow Cloak. Lasts for 8 turns if the user is holding Icy Rock. Fails if the current weather is Hail.",
		//shortDesc = "For 5 turns, hail crashes down.",
		id = Moves.HAIL,
		//name = "Hail",
		pp = 10,
		priority = 0,
		flags = new Flags (),
		weather = "hail",
		//secondary = false,
		target = Target.All,
		type = Types.ICE,
		zMoveBoost = new Boosts() {spe = 1},
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 359,
		accuracy = 90,
		basePower = 100,
		category = Category.PHYSICAL,
		//desc = "Lowers the user's Speed by 1 stage.",
		//shortDesc = "Lowers the user's Speed by 1.",
		id = Moves.HAMMERARM,
		isViable = true,
		//name = "Hammer Arm",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true, punch: true),
		self = {
			boosts = new Boosts() {
				spe = -1
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.FIGHTING,
		zMovePower = 180,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 603,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Money falls from the sky.",
		//shortDesc = "No competitive use.",
		id = Moves.HAPPYHOUR,
		//name = "Happy Hour",
		pp = 30,
		priority = 0,
		flags = new Flags (),
		onTryHit = function (target, source) {
			this.add('-activate', target, 'move = Happy Hour');
		},
		//secondary = false,
		target = Target.AllySide,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {atk = 1, def = 1, spa = 1, spd = 1, spe = 1},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 106,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the user's Defense by 1 stage.",
		//shortDesc = "Raises the user's Defense by 1.",
		id = Moves.HARDEN,
		//name = "Harden",
		pp = 30,
		priority = 0,
		flags = new Flags (snatch: true),
		boosts = new Boosts() {
			def = 1
		},
		//secondary = false,
		target = Target.Self,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {def = 1},
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 114,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Resets the stat stages of all active Pokemon to 0.",
		//shortDesc = "Eliminates all stat changes.",
		id = Moves.HAZE,
		isViable = true,
		//name = "Haze",
		pp = 30,
		priority = 0,
		flags = new Flags (authentic: true),
		onHitField = function () {
			this.add('-clearallboost');
			for (const side of this.sides) {
				for (const pokemon of side.active) {
					if (pokemon && pokemon.isActive) pokemon.clearBoosts();
				}
			}
		},
		//secondary = false,
		target = Target.All,
		type = Types.ICE,
		zMoveEffect = "heal",
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 543,
		accuracy = 100,
		basePower = 120,
		category = Category.PHYSICAL,
		//desc = "If the target Lost HP, the user takes recoil damage equal to 1/4 the HP lost by the target, rounded half up, but not less than 1 HP.",
		//shortDesc = "Has 1/4 recoil.",
		id = Moves.HEADCHARGE,
		isViable = true,
		//name = "Head Charge",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		recoil = new int[] {1, 4},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 190,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 457,
		accuracy = 80,
		basePower = 150,
		category = Category.PHYSICAL,
		//desc = "If the target Lost HP, the user takes recoil damage equal to 1/2 the HP lost by the target, rounded half up, but not less than 1 HP.",
		//shortDesc = "Has 1/2 recoil.",
		id = Moves.HEADSMASH,
		isViable = true,
		//name = "Head Smash",
		pp = 5,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		recoil = new int[] {1, 2},
		//secondary = false,
		target = Target.Normal,
		type = Types.ROCK,
		zMovePower = 200,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 29,
		accuracy = 100,
		basePower = 70,
		category = Category.PHYSICAL,
		//desc = "Has a 30% chance to flinch the target.",
		//shortDesc = "30% chance to flinch the target.",
		id = Moves.HEADBUTT,
		//name = "Headbutt",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 30,
			volatileStatus = "flinch"
		},
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 140,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 215,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Every Pokemon in the user's party is cured of its major status condition. Active Pokemon with the Ability Soundproof are not cured.",
		//shortDesc = "Cures the user's party of all status conditions.",
		id = Moves.HEALBELL,
		isViable = true,
		//name = "Heal Bell",
		pp = 5,
		priority = 0,
		flags = new Flags (snatch: true, sound: true, distance: true, authentic: true),
		onHit = function (pokemon, source) {
			this.add('-activate', source, 'move = Heal Bell');
			let side = pokemon.side;
			let success = false;
			for (const ally of side.pokemon) {
				if (ally.hasAbility("soundproof")) continue;
				if (ally.cureStatus()) success = true;
			}
			return success;
		},
		target = Target.AllyTeam,
		type = Types.NORMAL,
		zMoveEffect = "heal",
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 377,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "For 5 turns, the target Is prevented from restoring any HP as long as it remains active. During the effect, healing and draining moves ARE unusable, and Abilities and items that grant healing will not heal the user. If an affected Pokemon uses Baton Pass, the replacement will remain unable to restore its HP. Pain Split and the Ability Regenerator are unaffected.",
		//shortDesc = "For 5 turns, the foe(s) is prevented from healing.",
		id = Moves.HEALBLOCK,
		//name = "Heal Block",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true),
		volatileStatus = "healblock",
		effect = {
			duration = 5,
			durationCallback = function (target, source, effect) {
				if (source && source.hasAbility("persistent")) {
					this.add('-activate', source, 'ability = Persistent', effect);
					return 7;
				}
				return 5;
			},
			onStart = function (pokemon) {
				this.add('-start', pokemon, 'move = Heal Block');
			},
			onDisableMove = function (pokemon) {
				for (const moveSlOT of pokemon.moveSlOTS) {
					if (this.getMove(moveSlOT.id).flags["heal"]) {
						pokemon.disableMove(moveSlOT.id);
					}
				}
			},
			onBeforeMovePriority = 6,
			onBeforeMove = function (pokemon, target, move) {
				if (move.flags["heal"] && !move.isZ) {
					this.add("cant", pokemon, 'move = Heal Block', move);
					return false;
				}
			},
			onResidualOrder = 17,
			onEnd = function (pokemon) {
				this.add('-end', pokemon, 'move = Heal Block');
			},
			onTryHeal = function (damage, target, source, effect) {
				if ((effect && effect.id === "zpower") || this.effectData.isZ) return damage;
				return false;
			}
		},
		//secondary = false,
		target = Target.AllAdjacentFoes,
		type = Types.PSYCHIC,
		zMoveBoost = new Boosts() {spa = 2},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 456,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user restores 1/2 of its maximum HP, rounded half up.",
		//shortDesc = "Heals the user by 50% of its max HP.",
		id = Moves.HEALORDER,
		isViable = true,
		//name = "Heal Order",
		pp = 10,
		priority = 0,
		flags = new Flags (snatch: true, heal: true),
		heal = new int[] {1, 2},
		//secondary = false,
		target = Target.Self,
		type = Types.BUG,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 505,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The target Restores 1/2 of its maximum HP, rounded half up. If the user has the Ability Mega Launcher, the target Instead restores 3/4 of its maximum HP, rounded half down.",
		//shortDesc = "Heals the target By 50% of its max HP.",
		id = Moves.HEALPULSE,
		//name = "Heal Pulse",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, pulse: true, reflectable: true, distance: true, heal: true, mystery: true),
		onHit = function (target, source) {
			if (source.hasAbility("megalauncher")) {
				return this.heal(this.modify(target.Maxhp, 0.75));
			} else {
				return this.heal(Math.ceil(target.Maxhp * 0.5));
			}
		},
		//secondary = false,
		target = Target.Any,
		type = Types.PSYCHIC,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 361,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user faints and the Pokemon brought out to replace it has its HP fully restored along with having any major status condition cured. Fails if the user is the last unfainted Pokemon in its party.",
		//shortDesc = "User faints. Replacement is fully healed.",
		id = Moves.HEALINGWISH,
		isViable = true,
		//name = "Healing Wish",
		pp = 10,
		priority = 0,
		flags = new Flags (snatch: true, heal: true),
		onTryHit = function (pokemon, target, move) {
			if (!this.canSwitch(pokemon.side)) {
				delete move.selfdestruct;
				return false;
			}
		},
		selfdestruct = "ifHit",
		sideCondition = "healingwish",
		effect = {
			duration = 2,
			onStart = function (side, source) {
				this.debug('Healing Wish started on ' + side.name);
				this.effectData.positions = [];
				// @ts-ignore
				for (let i = 0; i < side.active.length; i++) {
					this.effectData.positions[i] = false;
				}
				this.effectData.positions[source.position] = true;
			},
			onRestart = function (side, source) {
				this.effectData.positions[source.position] = true;
			},
			onSwitchInPriority = 1,
			onSwitchIn = function (target) {
				const positions = //@type {boolean[]}  
					(this.effectData.positions);
				if (!positions[target.Position]) {
					return;
				}
				if (!target.Fainted) {
					target.Heal(target.Maxhp);
					target.SetStatus("");
					this.add('-heal', target, target.GetHealth, '[from] move = Healing Wish');
					positions[target.Position] = false;
				}
				if (!positions.some(affected => affected === true)) {
					target.Side.removeSiDECONDITION("healingwish");
				}
			}
		},
		//secondary = false,
		target = Target.Self,
		type = Types.PSYCHIC,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 531,
		accuracy = 100,
		basePower = 60,
		category = Category.PHYSICAL,
		//desc = "Has a 30% chance to flinch the target.",
		//shortDesc = "30% chance to flinch the target.",
		id = Moves.HEARTSTAMP,
		//name = "Heart Stamp",
		pp = 25,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 30,
			volatileStatus = "flinch"
		},
		target = Target.Normal,
		type = Types.PSYCHIC,
		zMovePower = 120,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 391,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user swaps all its stat stage changes with the target.",
		//shortDesc = "Swaps all stat changes with target.",
		id = Moves.HEARTSWAP,
		//name = "Heart Swap",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, authentic: true, mystery: true),
		onHit = function (target, source) {
			let targetBOosts = {};
			let sourceBoosts = {};

			for (let i in target.Boosts) {
				targetBOosts[i] = target.Boosts[i];
				sourceBoosts[i] = source.boosts[i];
			}

			target.SetBoost(sourceBoosts);
			source.setBoost(targetBOosts);

			this.add('-swapboost', source, target, '[from] move = Heart Swap');
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.PSYCHIC,
		zMoveEffect = "crit2",
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 535,
		accuracy = 100,
		basePower = 0,
		basePowerCallback = function (pokemon, target) {
			let targetWEight = target.GetWeight();
			let pokemonWeight = pokemon.getWeight();
			if (pokemonWeight > targetWEight * 5) {
				return 120;
			}
			if (pokemonWeight > targetWEight * 4) {
				return 100;
			}
			if (pokemonWeight > targetWEight * 3) {
				return 80;
			}
			if (pokemonWeight > targetWEight * 2) {
				return 60;
			}
			return 40;
		},
		category = Category.PHYSICAL,
		//desc = "The power of this move depends on (user's weight / target'S weight), rounded down. Power is equal to 120 if the result is 5 or more, 100 if 4, 80 if 3, 60 if 2, and 40 if 1 or less. Damage doubles and no accuracy check is done if the target Has used Minimize while active.",
		//shortDesc = "More power the heavier the user than the target.",
		id = Moves.HEATCRASH,
		//name = "Heat Crash",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true, nonsky: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.FIRE,
		zMovePower = 160,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 257,
		accuracy = 90,
		basePower = 95,
		category = Category.SPECIAL,
		//desc = "Has a 10% chance to burn the target.",
		//shortDesc = "10% chance to burn the foe(s).",
		id = Moves.HEATWAVE,
		isViable = true,
		//name = "Heat Wave",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 10,
			status = "brn"
		},
		target = Target.AllAdjacentFoes,
		type = Types.FIRE,
		zMovePower = 175,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 484,
		accuracy = 100,
		basePower = 0,
		basePowerCallback = function (pokemon, target) {
			let targetWEight = target.GetWeight();
			let pokemonWeight = pokemon.getWeight();
			if (pokemonWeight > targetWEight * 5) {
				return 120;
			}
			if (pokemonWeight > targetWEight * 4) {
				return 100;
			}
			if (pokemonWeight > targetWEight * 3) {
				return 80;
			}
			if (pokemonWeight > targetWEight * 2) {
				return 60;
			}
			return 40;
		},
		category = Category.PHYSICAL,
		//desc = "The power of this move depends on (user's weight / target'S weight), rounded down. Power is equal to 120 if the result is 5 or more, 100 if 4, 80 if 3, 60 if 2, and 40 if 1 or less. Damage doubles and no accuracy check is done if the target Has used Minimize while active.",
		//shortDesc = "More power the heavier the user than the target.",
		id = Moves.HEAVYSLAM,
		isViable = true,
		//name = "Heavy Slam",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true, nonsky: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.STEEL,
		zMovePower = 160,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 270,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The power of the target'S attack this turn is multiplied by 1.5 (this effect is stackable). Fails if there is no ally adjacent to the user or if the ally already moved this turn, but does not fail if the ally is using a two-turn move.",
		//shortDesc = "One adjacent ally's move power is 1.5x this turn.",
		id = Moves.HELPINGHAND,
		//name = "Helping Hand",
		pp = 20,
		priority = 5,
		flags = new Flags (authentic: true),
		volatileStatus = "helpinghand",
		onTryHit = function (target) {
			if (!target.NewlySwitched && !this.willMove(target)) return false;
		},
		effect = {
			duration = 1,
			onStart = function (target, source) {
				this.effectData.multiplier = 1.5;
				this.add('-singleturn', target, 'Helping Hand', '[of] ' + source);
			},
			onRestart = function (target, source) {
				this.effectData.multiplier *= 1.5;
				this.add('-singleturn', target, 'Helping Hand', '[of] ' + source);
			},
			onBasePowerPriority = 3,
			onBasePower = function (basePower) {
				this.debug('Boosting from Helping Hand = ' + this.effectData.multiplier);
				return this.chainModify(this.effectData.multiplier);
			}
		},
		//secondary = false,
		target = Target.AdjacentAlly,
		type = Types.NORMAL,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 506,
		accuracy = 100,
		basePower = 65,
		basePowerCallback = function (pokemon, target, move) {
			if (target.Status || target.HasAbility("comatose")) return move.basePower * 2;
			return move.basePower;
		},
		category = Category.SPECIAL,
		//desc = "Power doubles if the target Has a major status condition.",
		//shortDesc = "Power doubles if the target Has a status ailment.",
		id = Moves.HEX,
		isViable = true,
		//name = "Hex",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.GHOST,
		zMovePower = 160,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 237,
		accuracy = 100,
		basePower = 60,
		category = Category.SPECIAL,
		//desc = "This move's type depends on the user's individual values (IVs), and can be any type but Fairy and Normal.",
		//shortDesc = "Varies in type based on the user's IVs.",
		id = Moves.HIDDENPOWER,
		//name = "Hidden Power",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		onModifyMove = function (move, pokemon) {
			move.type = pokemon.hpType || "Dark";
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 120,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 237,
		accuracy = 100,
		basePower = 60,
		category = Category.SPECIAL,
		//desc = "",
		//shortDesc = "",
		id = Moves.HIDDENPOWER,
		//name = "Hidden Power Bug",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.BUG,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 237,
		accuracy = 100,
		basePower = 60,
		category = Category.SPECIAL,
		//desc = "",
		//shortDesc = "",
		id = Moves.HIDDENPOWER,
		//name = "Hidden Power Dark",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.DARK,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 237,
		accuracy = 100,
		basePower = 60,
		category = Category.SPECIAL,
		//desc = "",
		//shortDesc = "",
		id = Moves.HIDDENPOWER,
		//name = "Hidden Power Dragon",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.DRAGON,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 237,
		accuracy = 100,
		basePower = 60,
		category = Category.SPECIAL,
		//desc = "",
		//shortDesc = "",
		id = Moves.HIDDENPOWER,
		isViable = true,
		//name = "Hidden Power Electric",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.ELECTRIC,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 237,
		accuracy = 100,
		basePower = 60,
		category = Category.SPECIAL,
		//desc = "",
		//shortDesc = "",
		id = Moves.HIDDENPOWER,
		isViable = true,
		//name = "Hidden Power Fighting",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.FIGHTING,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 237,
		accuracy = 100,
		basePower = 60,
		category = Category.SPECIAL,
		//desc = "",
		//shortDesc = "",
		id = Moves.HIDDENPOWER,
		isViable = true,
		//name = "Hidden Power Fire",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.FIRE,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 237,
		accuracy = 100,
		basePower = 60,
		category = Category.SPECIAL,
		//desc = "",
		//shortDesc = "",
		id = Moves.HIDDENPOWER,
		//name = "Hidden Power Flying",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.FLYING,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 237,
		accuracy = 100,
		basePower = 60,
		category = Category.SPECIAL,
		//desc = "",
		//shortDesc = "",
		id = Moves.HIDDENPOWER,
		//name = "Hidden Power Ghost",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.GHOST,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 237,
		accuracy = 100,
		basePower = 60,
		category = Category.SPECIAL,
		//desc = "",
		//shortDesc = "",
		id = Moves.HIDDENPOWER,
		isViable = true,
		//name = "Hidden Power Grass",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.GRASS,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 237,
		accuracy = 100,
		basePower = 60,
		category = Category.SPECIAL,
		//desc = "",
		//shortDesc = "",
		id = Moves.HIDDENPOWER,
		//name = "Hidden Power Ground",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.GROUND,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 237,
		accuracy = 100,
		basePower = 60,
		category = Category.SPECIAL,
		//desc = "",
		//shortDesc = "",
		id = Moves.HIDDENPOWER,
		isViable = true,
		//name = "Hidden Power Ice",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.ICE,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 237,
		accuracy = 100,
		basePower = 60,
		category = Category.SPECIAL,
		//desc = "",
		//shortDesc = "",
		id = Moves.HIDDENPOWER,
		//name = "Hidden Power Poison",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.POISON,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 237,
		accuracy = 100,
		basePower = 60,
		category = Category.SPECIAL,
		//desc = "",
		//shortDesc = "",
		id = Moves.HIDDENPOWER,
		//name = "Hidden Power Psychic",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.PSYCHIC,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 237,
		accuracy = 100,
		basePower = 60,
		category = Category.SPECIAL,
		//desc = "",
		//shortDesc = "",
		id = Moves.HIDDENPOWER,
		//name = "Hidden Power Rock",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.ROCK,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 237,
		accuracy = 100,
		basePower = 60,
		category = Category.SPECIAL,
		//desc = "",
		//shortDesc = "",
		id = Moves.HIDDENPOWER,
		//name = "Hidden Power Steel",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.STEEL,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 237,
		accuracy = 100,
		basePower = 60,
		category = Category.SPECIAL,
		//desc = "",
		//shortDesc = "",
		id = Moves.HIDDENPOWER,
		//name = "Hidden Power Water",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.WATER,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 667,
		accuracy = 95,
		basePower = 95,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect.",
		id = Moves.HIGHHORSEPOWER,
		//name = "High Horsepower",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.GROUND,
		zMovePower = 175,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 136,
		accuracy = 90,
		basePower = 130,
		category = Category.PHYSICAL,
		//desc = "If this attack is not successful, the user loses half of its maximum HP, rounded down, as crash damage. Pokemon with the Ability Magic Guard are unaffected by crash damage.",
		//shortDesc = "User is hurt by 50% of its max HP if it misses.",
		id = Moves.HIGHJUMPKICK,
		isViable = true,
		//name = "High Jump Kick",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true, gravity: true),
		hasCustomRecoil = true,
		onMoveFail = function (target, source, move) {
			this.damage(source.maxhp / 2, source, source, "highjumpkick");
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.FIGHTING,
		zMovePower = 195,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 610,
		accuracy = 100,
		basePower = 40,
		category = Category.PHYSICAL,
		//desc = "Leaves the target With at least 1 HP.",
		//shortDesc = "Always leaves the target With at least 1 HP.",
		id = Moves.HOLDBACK,
		//name = "Hold Back",
		pp = 40,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		noFaint = true,
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 100,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 615,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Nothing happens... Fails if there is no ally adjacent to the user.",
		//shortDesc = "No competitive use. Or any use.",
		id = Moves.HOLDHANDS,
		//name = "Hold Hands",
		pp = 40,
		priority = 0,
		flags = new Flags (authentic: true),
		//secondary = false,
		target = Target.AdjacentAlly,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {atk = 1, def = 1, spa = 1, spd = 1, spe = 1},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 468,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the user's Attack and accuracy by 1 stage.",
		//shortDesc = "Raises the user's Attack and accuracy by 1.",
		id = Moves.HONECLAWS,
		isViable = true,
		//name = "Hone Claws",
		pp = 15,
		priority = 0,
		flags = new Flags (snatch: true),
		boosts = new Boosts() {
			atk = 1,
			accuracy = 1
		},
		//secondary = false,
		target = Target.Self,
		type = Types.DARK,
		zMoveBoost = new Boosts() {atk = 1},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 30,
		accuracy = 100,
		basePower = 65,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect.",
		id = Moves.HORNATTACK,
		//name = "Horn Attack",
		pp = 25,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 120,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 32,
		accuracy = 30,
		basePower = 0,
		category = Category.PHYSICAL,
		//desc = "Deals damage to the target Equal to the target'S maximum HP. Ignores accuracy and evasiveness modifiers. This attack's accuracy is equal to (user's level - target'S level + 30)%, and fails if the target Is at a higher level. Pokemon with the Ability Sturdy are immune.",
		//shortDesc = "OHKOs the target. Fails if user is a lower level.",
		id = Moves.HORNDRILL,
		//name = "Horn Drill",
		pp = 5,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		ohko = true,
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 180,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 532,
		accuracy = 100,
		basePower = 75,
		category = Category.PHYSICAL,
		//desc = "The user recovers 1/2 the HP lost by the target, rounded half up. If Big Root is held by the user, the HP recovered is 1.3x normal, rounded half down.",
		//shortDesc = "User recovers 50% of the damage dealt.",
		id = Moves.HORNLEECH,
		isViable = true,
		//name = "Horn Leech",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true, heal: true),
		drain = new int[] {1, 2},
		//secondary = false,
		target = Target.Normal,
		type = Types.GRASS,
		zMovePower = 140,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 336,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the user's Attack by 1 stage.",
		//shortDesc = "Raises the user's Attack by 1.",
		id = Moves.HOWL,
		//name = "Howl",
		pp = 40,
		priority = 0,
		flags = new Flags (snatch: true),
		boosts = new Boosts() {
			atk = 1
		},
		//secondary = false,
		target = Target.Self,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {atk = 1},
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 542,
		accuracy = 70,
		basePower = 110,
		category = Category.SPECIAL,
		//desc = "Has a 30% chance to confuse the target. This move can hit a target Using Bounce, Fly, or Sky Drop. If the weather is Rain Dance, this move does not check accuracy. If the weather is Sunny Day, this move's accuracy is 50%.",
		//shortDesc = "30% chance to confuse target. Can't miss in rain.",
		id = Moves.HURRICANE,
		isViable = true,
		//name = "Hurricane",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, distance: true),
		onModifyMove = function (move) {
			if (this.isWeather(["raindance", "primordialsea"])) {
				move.//accuracy = true;
			} else if (this.isWeather(["sunnyday", "desolateland"])) {
				move.accuracy = 50;
			}
		},
		secondary = new Secondary() {
			chance = 30,
			volatileStatus = "confusion"
		},
		target = Target.Any,
		type = Types.FLYING,
		zMovePower = 185,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 308,
		accuracy = 90,
		basePower = 150,
		category = Category.SPECIAL,
		//desc = "If this move is successful, the user must recharge on the following turn and cannot make a move.",
		//shortDesc = "User cannot move next turn.",
		id = Moves.HYDROCANNON,
		//name = "Hydro Cannon",
		pp = 5,
		priority = 0,
		flags = new Flags (recharge: true, protect: true, mirror: true),
		self = {
			volatileStatus = "mustrecharge"
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.WATER,
		zMovePower = 200,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 56,
		accuracy = 80,
		basePower = 110,
		category = Category.SPECIAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect.",
		id = Moves.HYDROPUMP,
		isViable = true,
		//name = "Hydro Pump",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.WATER,
		zMovePower = 185,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 642,
		//accuracy = true,
		basePower = 1,
		category = Category.PHYSICAL,
		//shortDesc = "Power is equal to the base move's Z-Power.",
		id = Moves.HYDROVORTEX,
		isViable = true,
		//name = "Hydro Vortex",
		pp = 1,
		priority = 0,
		flags = new Flags (),
		isZ = "wateriumz",
		//secondary = false,
		target = Target.Normal,
		type = Types.WATER,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 63,
		accuracy = 90,
		basePower = 150,
		category = Category.SPECIAL,
		//desc = "If this move is successful, the user must recharge on the following turn and cannot make a move.",
		//shortDesc = "User cannot move next turn.",
		id = Moves.HYPERBEAM,
		//name = "Hyper Beam",
		pp = 5,
		priority = 0,
		flags = new Flags (recharge: true, protect: true, mirror: true),
		self = {
			volatileStatus = "mustrecharge"
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 200,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 158,
		accuracy = 90,
		basePower = 80,
		category = Category.PHYSICAL,
		//desc = "Has a 10% chance to flinch the target.",
		//shortDesc = "10% chance to flinch the target.",
		id = Moves.HYPERFANG,
		//name = "Hyper Fang",
		pp = 15,
		priority = 0,
		flags = new Flags (bite: true, contact: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 10,
			volatileStatus = "flinch"
		},
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 160,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 621,
		//accuracy = true,
		basePower = 100,
		category = Category.PHYSICAL,
		//desc = "Lowers the user's Defense by 1 stage. This move cannot be used successfully unless the user's current form, while considering Transform, is Hoopa Unbound. If this move is successful, it breaks through the target'S Baneful Bunker, Detect, King's Shield, Protect, or Spiky Shield for this turn, allowing other Pokemon to attack the target Normally. If the target'S side is protected by Crafty Shield, Mat Block, Quick Guard, or Wide Guard, that protection is also broken for this turn and other Pokemon may attack the target'S side normally.",
		//shortDesc = "Hoopa-U = Lowers user's Def by 1; breaks protection.",
		id = Moves.HYPERSPACEFURY,
		isViable = true,
		//name = "Hyperspace Fury",
		pp = 5,
		priority = 0,
		flags = new Flags (mirror: true, authentic: true),
		breaksProtect = true,
		onTry = function (pokemon) {
			if (pokemon.template.species === 'Hoopa-Unbound') {
				return;
			}
			this.add('-hint', "Only a Pokemon whose form is Hoopa Unbound can use this move.");
			if (pokemon.template.species === "Hoopa") {
				this.add('-fail', pokemon, 'move = Hyperspace Fury', '[forme]');
				return null;
			}
			this.add('-fail', pokemon, 'move = Hyperspace Fury');
			return null;
		},
		self = {
			boosts = new Boosts() {
				def = -1
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.DARK,
		zMovePower = 180,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 593,
		//accuracy = true,
		basePower = 80,
		category = Category.SPECIAL,
		//desc = "If this move is successful, it breaks through the target'S Baneful Bunker, Detect, King's Shield, Protect, or Spiky Shield for this turn, allowing other Pokemon to attack the target Normally. If the target'S side is protected by Crafty Shield, Mat Block, Quick Guard, or Wide Guard, that protection is also broken for this turn and other Pokemon may attack the target'S side normally.",
		//shortDesc = "Breaks the target'S protection for this turn.",
		id = Moves.HYPERSPACEHOLE,
		//name = "Hyperspace Hole",
		pp = 5,
		priority = 0,
		flags = new Flags (mirror: true, authentic: true),
		breaksProtect = true,
		//secondary = false,
		target = Target.Normal,
		type = Types.PSYCHIC,
		zMovePower = 160,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 304,
		accuracy = 100,
		basePower = 90,
		category = Category.SPECIAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect. Hits adjacent foes.",
		id = Moves.HYPERVOICE,
		isViable = true,
		//name = "Hyper Voice",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, sound: true, authentic: true),
		//secondary = false,
		target = Target.AllAdjacentFoes,
		type = Types.NORMAL,
		zMovePower = 175,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 95,
		accuracy = 60,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Causes the target To fall asleep.",
		//shortDesc = "Puts the target To sleep.",
		id = Moves.HYPNOSIS,
		//name = "Hypnosis",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true),
		status = "slp",
		//secondary = false,
		target = Target.Normal,
		type = Types.PSYCHIC,
		zMoveBoost = new Boosts() {spe = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 301,
		accuracy = 90,
		basePower = 30,
		basePowerCallback = function (pokemon, target, move) {
			let bp = move.basePower;
			if (pokemon.volatiles.iceball && pokemon.volatiles.iceball.hitCount) {
				bp *= Math.pow(2, pokemon.volatiles.iceball.hitCount);
			}
			if (pokemon.status !== "slp") pokemon.addVolatile("iceball");
			if (pokemon.volatiles.defensecurl) {
				bp *= 2;
			}
			this.debug("Ice Ball bp = " + bp);
			return bp;
		},
		category = Category.PHYSICAL,
		//desc = "If this move is successful, the user is locked into this move and cannot make another move until it misses, 5 turns have passed, or the attack cannot be used. Power doubles with each successful hit of this move and doubles again if Defense Curl was used previously by the user. If this move is called by Sleep Talk, the move is used for one turn.",
		//shortDesc = "Power doubles with each hit. Repeats for 5 turns.",
		id = Moves.ICEBALL,
		//name = "Ice Ball",
		pp = 20,
		priority = 0,
		flags = new Flags (bullet: true, contact: true, protect: true, mirror: true),
		effect = {
			duration = 2,
			onLockMove = "iceball",
			onStart = function () {
				this.effectData.hitCount = 1;
			},
			onRestart = function () {
				this.effectData.hitCount++;
				if (this.effectData.hitCount < 5) {
					this.effectData.duration = 2;
				}
			},
			onResidual = function (target) {
				if (target.LastMove && target.LastMove.id === "struggle") {
					// don't lock
					delete target.Volatiles["iceball"];
				}
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.ICE,
		zMovePower = 100,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 58,
		accuracy = 100,
		basePower = 90,
		category = Category.SPECIAL,
		//desc = "Has a 10% chance to freeze the target.",
		//shortDesc = "10% chance to freeze the target.",
		id = Moves.ICEBEAM,
		isViable = true,
		//name = "Ice Beam",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 10,
			status = "frz"
		},
		target = Target.Normal,
		type = Types.ICE,
		zMovePower = 175,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 554,
		accuracy = 90,
		basePower = 140,
		category = Category.SPECIAL,
		//desc = "Has a 30% chance to burn the target. This attack charges on the first turn and executes on the second. If the user is holding a Power Herb, the move completes in one turn.",
		//shortDesc = "Charges turn 1. Hits turn 2. 30% burn.",
		id = Moves.ICEBURN,
		//name = "Ice Burn",
		pp = 5,
		priority = 0,
		flags = new Flags (charge: true, protect: true, mirror: true),
		onTry = function (attacker, defender, move) {
			if (attacker.removeVolatile(move.id)) {
				return;
			}
			this.add('-prepare', attacker, move.name, defender);
			if (!this.runEvent("ChargeMove", attacker, defender, move)) {
				this.add('-anim', attacker, move.name, defender);
				return;
			}
			attacker.addVolatile("twoturnmove", defender);
			return null;
		},
		secondary = new Secondary() {
			chance = 30,
			status = "brn"
		},
		target = Target.Normal,
		type = Types.ICE,
		zMovePower = 200,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 423,
		accuracy = 95,
		basePower = 65,
		category = Category.PHYSICAL,
		//desc = "Has a 10% chance to freeze the target And a 10% chance to flinch it.",
		//shortDesc = "10% chance to freeze. 10% chance to flinch.",
		id = Moves.ICEFANG,
		isViable = true,
		//name = "Ice Fang",
		pp = 15,
		priority = 0,
		flags = new Flags (bite: true, contact: true, protect: true, mirror: true),
		secondaries = [
			{
				chance = 10,
				status = "frz"
			}, {
				chance = 10,
				volatileStatus = "flinch"
			},
		],
		target = Target.Normal,
		type = Types.ICE,
		zMovePower = 120,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 665,
		accuracy = 90,
		basePower = 100,
		category = Category.PHYSICAL,
		//desc = "Lowers the user's Speed by 1 stage.",
		//shortDesc = "Lowers the user's Speed by 1.",
		id = Moves.ICEHAMMER,
		isViable = true,
		//name = "Ice Hammer",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true, punch: true),
		self = {
			boosts = new Boosts() {
				spe = -1
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.ICE,
		zMovePower = 180,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 8,
		accuracy = 100,
		basePower = 75,
		category = Category.PHYSICAL,
		//desc = "Has a 10% chance to freeze the target.",
		//shortDesc = "10% chance to freeze the target.",
		id = Moves.ICEPUNCH,
		isViable = true,
		//name = "Ice Punch",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true, punch: true),
		secondary = new Secondary() {
			chance = 10,
			status = "frz"
		},
		target = Target.Normal,
		type = Types.ICE,
		zMovePower = 140,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 420,
		accuracy = 100,
		basePower = 40,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "Usually goes first.",
		id = Moves.ICESHARD,
		isViable = true,
		//name = "Ice Shard",
		pp = 30,
		priority = 1,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.ICE,
		zMovePower = 100,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 556,
		accuracy = 90,
		basePower = 85,
		category = Category.PHYSICAL,
		//desc = "Has a 30% chance to flinch the target.",
		//shortDesc = "30% chance to flinch the target.",
		id = Moves.ICICLECRASH,
		isViable = true,
		//name = "Icicle Crash",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 30,
			volatileStatus = "flinch"
		},
		target = Target.Normal,
		type = Types.ICE,
		zMovePower = 160,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 333,
		accuracy = 100,
		basePower = 25,
		category = Category.PHYSICAL,
		//desc = "Hits two to five times. Has a 1/3 chance to hit two or three times, and a 1/6 chance to hit four or five times. If one of the hits breaks the target'S substitute, it will take damage for the remaining hits. If the user has the Ability Skill Link, this move will always hit five times.",
		//shortDesc = "Hits 2-5 times in one turn.",
		id = Moves.ICICLESPEAR,
		isViable = true,
		//name = "Icicle Spear",
		pp = 30,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		multihit = new int[] {2, 5},
		//secondary = false,
		target = Target.Normal,
		type = Types.ICE,
		zMovePower = 140,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 196,
		accuracy = 95,
		basePower = 55,
		category = Category.SPECIAL,
		//desc = "Has a 100% chance to lower the target'S Speed by 1 stage.",
		//shortDesc = "100% chance to lower the foe(s) Speed by 1.",
		id = Moves.ICYWIND,
		//name = "Icy Wind",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 100,
			boosts = new Boosts() {
				spe = -1
			}
		},
		target = Target.AllAdjacentFoes,
		type = Types.ICE,
		zMovePower = 100,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 286,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user prevents all of its foes from using any moves THAT the user also knows as long as the user remains active.",
		//shortDesc = "No foe can use any move known by the user.",
		id = Moves.IMPRISON,
		//name = "Imprison",
		pp = 10,
		priority = 0,
		flags = new Flags (snatch: true, authentic: true),
		volatileStatus = "imprison",
		effect = {
			noCopy = true,
			onStart = function (target) {
				this.add('-start', target, 'move = Imprison');
			},
			onFoeDisableMove = function (pokemon) {
				for (const moveSlOT of this.effectData.source.moveSlOTS) {
					if (moveSlOT.id === "struggle") continue;
					pokemon.disableMove(moveSlOT.id, "hidden");
				}
				pokemon.maybeDisabled = true;
			},
			onFoeBeforeMovePriority = 4,
			onFoeBeforeMove = function (attacker, defender, move) {
				if (move.id !== "struggle" && this.effectData.source.hasMove(move.id) && !move.isZ) {
					this.add("cant", attacker, 'move = Imprison', move);
					return false;
				}
			}
		},
		//secondary = false,
		pressuretarget = Target.FoeSide,
		target = Target.Self,
		type = Types.PSYCHIC,
		zMoveBoost = new Boosts() {spd = 2},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 510,
		accuracy = 100,
		basePower = 60,
		category = Category.SPECIAL,
		//desc = "The target Loses its held item if it is a Berry or a Gem. This move cannot cause Pokemon with the Ability Sticky Hold to lose their held item. Items lost to this move cannot be regained with Recycle or the Ability Harvest.",
		//shortDesc = "Destroys the foe(s) Berry/Gem.",
		id = Moves.INCINERATE,
		//name = "Incinerate",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		onHit = function (pokemon, source) {
			let item = pokemon.getItem();
			if ((item.isBerry || item.isGem) && pokemon.takeItem(source)) {
				this.add('-enditem', pokemon, item.name, '[from] move = Incinerate');
			}
		},
		//secondary = false,
		target = Target.AllAdjacentFoes,
		type = Types.FIRE,
		zMovePower = 120,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 517,
		accuracy = 50,
		basePower = 100,
		category = Category.SPECIAL,
		//desc = "Has a 100% chance to burn the target.",
		//shortDesc = "100% chance to burn the target.",
		id = Moves.INFERNO,
		//name = "Inferno",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 100,
			status = "brn"
		},
		target = Target.Normal,
		type = Types.FIRE,
		zMovePower = 180,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 640,
		//accuracy = true,
		basePower = 1,
		category = Category.PHYSICAL,
		//shortDesc = "Power is equal to the base move's Z-Power.",
		id = Moves.INFERNOOVERDRIVE,
		isViable = true,
		//name = "Inferno Overdrive",
		pp = 1,
		priority = 0,
		flags = new Flags (),
		isZ = "firiumz",
		//secondary = false,
		target = Target.Normal,
		type = Types.FIRE,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 611,
		accuracy = 100,
		basePower = 20,
		category = Category.SPECIAL,
		//desc = "Prevents the target From switching for four or five turns; seven turns if the user is holding Grip Claw. Causes damage to the target Equal to 1/8 of its maximum HP (1/6 if the user is holding Binding Band), rounded down, at the end of each turn during effect. The target Can still switch out if it is holding Shed Shell or uses Baton Pass, Parting Shot, U-turn, or Volt Switch. The effect ends if either the user or the target Leaves the field, or if the target Uses Rapid Spin or Substitute. This effect is not stackable or reset by using this or another partial-trapping move.",
		//shortDesc = "Traps and damages the target For 4-5 turns.",
		id = Moves.INFESTATION,
		//name = "Infestation",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		volatileStatus = "partiallytrapped",
		//secondary = false,
		target = Target.Normal,
		type = Types.BUG,
		zMovePower = 100,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 275,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user has 1/16 of its maximum HP restored at the end of each turn, but it is prevented from switching out and other Pokemon cannot force the user to switch out. The user can still switch out if it uses Baton Pass, Parting Shot, U-turn, or Volt Switch. If the user leaves the field using Baton Pass, the replacement will remain trapped and still receive the healing effect. During the effect, the user can be hit normally by Ground-type attacks and be affected by Spikes, Toxic Spikes, and Sticky Web, even if the user is a Flying type or has the Ability Levitate.",
		//shortDesc = "Traps/grounds user; heals 1/16 max HP per turn.",
		id = Moves.INGRAIN,
		//name = "Ingrain",
		pp = 20,
		priority = 0,
		flags = new Flags (snatch: true, nonsky: true),
		volatileStatus = "ingrain",
		effect = {
			onStart = function (pokemon) {
				this.add('-start', pokemon, 'move = Ingrain');
			},
			onResidualOrder = 7,
			onResidual = function (pokemon) {
				this.heal(pokemon.maxhp / 16);
			},
			onTrapPokemon = function (pokemon) {
				pokemon.tryTrap();
			},
			// groundedness implemented in battle.engine.js:BattlePokemon#isGrounded
			onDragOut = function (pokemon) {
				this.add('-activate', pokemon, 'move = Ingrain');
				return null;
			}
		},
		//secondary = false,
		target = Target.Self,
		type = Types.GRASS,
		zMoveBoost = new Boosts() {spd = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 689,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The target Immediately uses its last used move. Fails if the target Has not made a move, if the move has 0 PP, if the target Is preparing to use Beak Blast, Focus Punch, or Shell Trap, or if the move is Assist, Beak Blast, Bide, Copycat, Focus Punch, Ice Ball, Instruct, Me First, Metronome, Mimic, Mirror Move, Nature Power, Outrage, Petal Dance, Rollout, Shell Trap, Sketch, Sleep Talk, Thrash, Transform, a charge move, a recharge move, or a Z-Move.",
		//shortDesc = "The target Immediately uses its last used move.",
		id = Moves.INSTRUCT,
		//name = "Instruct",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, authentic: true, mystery: true),
		onHit = function (target, source) {
			if (!target.LastMove) return false;
			let lastMove = target.LastMove;
			let moveIndex = target.Moves.INDEXOF(lastMove.id);
			let noInstruct = [
				"assist", "beakblast", "bide", "copycat", "focuspunch", "iceball", "instruct", "mefirst", "metronome", "mimic", "mirrormove", "naturepower", "outrage", "petaldance", "rollout", "shelltrap", "sketch", "sleeptalk", "thrash", "transform",
			];
			if (noInstruct.includes(lastMove.id) || lastMove.isZ || lastMove.flags["charge"] || lastMove.flags["recharge"] || target.Volatiles["beakblast"] || target.Volatiles["focuspunch"] || target.Volatiles["shelltrap"] || (target.MoveSlOTS[moveIndex] && target.MoveSlOTS[moveIndex].pp <= 0)) {
				return false;
			}
			this.add('-singleturn', target, 'move = Instruct', '[of] ' + source);
			this.runMove(target.LastMove.id, target, target.LastMoveTargetLOc);
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.PSYCHIC,
		zMoveBoost = new Boosts() {spa = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 569,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Causes Normal-type moves TO become Electric type this turn.",
		//shortDesc = "Normal moves BECOME Electric type this turn.",
		id = Moves.IONDELUGE,
		//name = "Ion Deluge",
		pp = 25,
		priority = 1,
		flags = new Flags (),
		pseudoWeather = "iondeluge",
		effect = {
			duration = 1,
			onStart = function (target) {
				this.add('-fieldactivate', 'move = Ion Deluge');
			},
			onModifyMovePriority = -2,
			onModifyMove = function (move) {
				if (move.type === "Normal") {
					move.type = "Electric";
					this.debug(move.name + "'s type changed to Electric");
				}
			}
		},
		//secondary = false,
		target = Target.All,
		type = Types.ELECTRIC,
		zMoveBoost = new Boosts() {spa = 1},
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 334,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the user's Defense by 2 stages.",
		//shortDesc = "Raises the user's Defense by 2.",
		id = Moves.IRONDEFENSE,
		//name = "Iron Defense",
		pp = 15,
		priority = 0,
		flags = new Flags (snatch: true),
		boosts = new Boosts() {
			def = 2
		},
		//secondary = false,
		target = Target.Self,
		type = Types.STEEL,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 442,
		accuracy = 100,
		basePower = 80,
		category = Category.PHYSICAL,
		//desc = "Has a 30% chance to flinch the target.",
		//shortDesc = "30% chance to flinch the target.",
		id = Moves.IRONHEAD,
		isViable = true,
		//name = "Iron Head",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 30,
			volatileStatus = "flinch"
		},
		target = Target.Normal,
		type = Types.STEEL,
		zMovePower = 160,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 231,
		accuracy = 75,
		basePower = 100,
		category = Category.PHYSICAL,
		//desc = "Has a 30% chance to lower the target'S Defense by 1 stage.",
		//shortDesc = "30% chance to lower the target'S Defense by 1.",
		id = Moves.IRONTAIL,
		//name = "Iron Tail",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 30,
			boosts = new Boosts() {
				def = -1
			}
		},
		target = Target.Normal,
		type = Types.STEEL,
		zMovePower = 180,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 449,
		accuracy = 100,
		basePower = 100,
		category = Category.SPECIAL,
		//desc = "This move's type depends on the user's held Plate.",
		//shortDesc = "Type varies based on the held Plate.",
		id = Moves.JUDGMENT,
		isViable = true,
		//name = "Judgment",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		onModifyMove = function (move, pokemon) {
			const item = pokemon.getItem();
			if (item.id && item.onPlate && !item.zMove) {
				move.type = item.onPlate;
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 180,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 26,
		accuracy = 95,
		basePower = 100,
		category = Category.PHYSICAL,
		//desc = "If this attack is not successful, the user loses half of its maximum HP, rounded down, as crash damage. Pokemon with the Ability Magic Guard are unaffected by crash damage.",
		//shortDesc = "User is hurt by 50% of its max HP if it misses.",
		id = Moves.JUMPKICK,
		isViable = true,
		//name = "Jump Kick",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true, gravity: true),
		hasCustomRecoil = true,
		onMoveFail = function (target, source, move) {
			this.damage(source.maxhp / 2, source, source, "jumpkick");
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.FIGHTING,
		zMovePower = 180,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 2,
		accuracy = 100,
		basePower = 50,
		category = Category.PHYSICAL,
		//desc = "Has a higher chance for a critical hit.",
		//shortDesc = "High critical hit ratio.",
		id = Moves.KARATECHOP,
		//name = "Karate Chop",
		pp = 25,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		critRatio = 2,
		//secondary = false,
		target = Target.Normal,
		type = Types.FIGHTING,
		zMovePower = 100,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 134,
		accuracy = 80,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Lowers the target'S accuracy by 1 stage.",
		//shortDesc = "Lowers the target'S accuracy by 1.",
		id = Moves.KINESIS,
		//name = "Kinesis",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true),
		boosts = new Boosts() {
			accuracy = -1
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.PSYCHIC,
		zMoveBoost = new Boosts() {evasion = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 588,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user is protected from most attacks made by other Pokemon during this turn, and Pokemon trying to make contact with the user have their Attack lowered by 2 stages. Non-damaging moves GO through this protection. This move has a 1/X chance of being successful, where X starts at 1 and triples each time this move is successfully used. X resets to 1 if this move fails or if the user's last move used is not Baneful Bunker, Detect, Endure, King's Shield, Protect, Quick Guard, Spiky Shield, or Wide Guard. Fails if the user moves LAST this turn.",
		//shortDesc = "Protects from attacks. Contact = lowers Atk by 2.",
		id = Moves.KINGSSHIELD,
		isViable = true,
		//name = "King's Shield",
		pp = 10,
		priority = 4,
		flags = new Flags (),
		stallingMove = true,
		volatileStatus = "kingsshield",
		onTryHit = function (pokemon) {
			return !!this.willAct() && this.runEvent("StallMove", pokemon);
		},
		onHit = function (pokemon) {
			pokemon.addVolatile("stall");
		},
		effect = {
			duration = 1,
			onStart = function (target) {
				this.add('-singleturn', target, "Protect");
			},
			onTryHitPriority = 3,
			onTryHit = function (target, source, move) {
				if (!move.flags["protect"] || move.category === "Status") {
					if (move.isZ) move.zBrokeProtect = true;
					return;
				}
				this.add('-activate', target, 'move = Protect');
				source.moveThisTurnResult = true;
				let lockedmove = source.getVolatile("lockedmove");
				if (lockedmove) {
					// Outrage counter is reset
					if (source.volatiles["lockedmove"].duration === 2) {
						delete source.volatiles["lockedmove"];
					}
				}
				if (move.flags["contact"]) {
					this.boost({atk = -2}, source, target, this.getMove("King's Shield"));
				}
				return null;
			},
			onHit = function (target, source, move) {
				if (move.zPowered && move.flags["contact"]) {
					this.boost({atk = -2}, source, target, this.getMove("King's Shield"));
				}
			}
		},
		//secondary = false,
		target = Target.Self,
		type = Types.STEEL,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 282,
		accuracy = 100,
		basePower = 65,
		category = Category.PHYSICAL,
		//desc = "If the target Is holding an item that can be removed from it, ignoring the Ability Sticky Hold, this move's power is multiplied by 1.5. If the user has not fainted, the target Loses its held item. This move cannot remove Z-Crystals, cause Pokemon with the Ability Sticky Hold to lose their held item, cause Pokemon that can Mega Evolve to lose the Mega Stone for their species, or cause a Kyogre, a Groudon, a Giratina, an Arceus, a Genesect, or a Silvally to lose their Blue Orb, Red Orb, Griseous Orb, Plate, Drive, or Memory respectively. Items lost to this move cannot be regained with Recycle or the Ability Harvest.",
		//shortDesc = "1.5x damage if foe holds an item. Removes ITEM.",
		id = Moves.KNOCKOFF,
		isViable = true,
		//name = "Knock Off",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		onBasePowerPriority = 4,
		onBasePower = function (basePower, source, target, move) {
			let item = target.GetItem();
			if (!this.singleEvent("TakeItem", item, target.ItemData, target, source, move, item)) return;
			if (item.id) {
				return this.chainModify(1.5);
			}
		},
		onAfterHit = function (target, source) {
			if (source.hp) {
				let item = target.TakeItem();
				if (item) {
					this.add('-enditem', target, item.name, '[from] move = Knock Off', '[of] ' + source);
				}
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.DARK,
		zMovePower = 120,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 616,
		accuracy = 100,
		basePower = 90,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect. Hits adjacent foes.",
		id = Moves.LANDSWRATH,
		//name = "Land's Wrath",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, nonsky: true),
		//secondary = false,
		target = Target.AllAdjacentFoes,
		type = Types.GROUND,
		zMovePower = 185,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 673,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "On the next turn, the user's attack will be a critical hit. Fails if the user already has the effect.",
		//shortDesc = "Next turn, the user's attack will be a critical hit.",
		id = Moves.LASERFOCUS,
		//name = "Laser Focus",
		pp = 30,
		priority = 0,
		flags = new Flags (snatch: true),
		volatileStatus = "laserfocus",
		effect = {
			duration = 2,
			onStart = function (pokemon) {
				this.add('-start', pokemon, 'move = Laser Focus');
			},
			onModifyCritRatio = function (critRatio) {
				return 5;
			},
			onEnd = function (pokemon) {
				this.add('-end', pokemon, 'move = Laser Focus', '[silent]');
			}
		},
		//secondary = false,
		target = Target.Self,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {atk = 1},
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 387,
		accuracy = 100,
		basePower = 140,
		category = Category.PHYSICAL,
		//desc = "This move fails unless the user knows this move and at least one other move, and has used all the other moves IT knows at least once each since it became active or Transformed.",
		//shortDesc = "Fails unless each known move has been used.",
		id = Moves.LASTRESORT,
		//name = "Last Resort",
		pp = 5,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		onTryHit = function (target, source) {
			if (source.moveSlOTS.length < 2) return false; // Last Resort fails unless the user knows at least 2 moves
			let hasLastResort = false; // User must actually have Last Resort for it to succeed
			for (const moveSlOT of source.moveSlOTS) {
				if (moveSlOT.id === "lastresort") {
					hasLastResort = true;
					continue;
				}
				if (!moveSlOT.used) return false;
			}
			return hasLastResort;
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 200,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 436,
		accuracy = 100,
		basePower = 80,
		category = Category.SPECIAL,
		//desc = "Has a 30% chance to burn the target.",
		//shortDesc = "30% chance to burn adjacent Pokemon.",
		id = Moves.LAVAPLUME,
		isViable = true,
		//name = "Lava Plume",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 30,
			status = "brn"
		},
		target = Target.AllAdjacent,
		type = Types.FIRE,
		zMovePower = 160,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 348,
		accuracy = 100,
		basePower = 90,
		category = Category.PHYSICAL,
		//desc = "Has a higher chance for a critical hit.",
		//shortDesc = "High critical hit ratio.",
		id = Moves.LEAFBLADE,
		isViable = true,
		//name = "Leaf Blade",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		critRatio = 2,
		//secondary = false,
		target = Target.Normal,
		type = Types.GRASS,
		zMovePower = 175,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 437,
		accuracy = 90,
		basePower = 130,
		category = Category.SPECIAL,
		//desc = "Lowers the user's Special Attack by 2 stages.",
		//shortDesc = "Lowers the user's Sp. Atk by 2.",
		id = Moves.LEAFSTORM,
		isViable = true,
		//name = "Leaf Storm",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		self = {
			boosts = new Boosts() {
				spa = -2
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.GRASS,
		zMovePower = 195,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 536,
		accuracy = 90,
		basePower = 65,
		category = Category.SPECIAL,
		//desc = "Has a 50% chance to lower the target'S accuracy by 1 stage.",
		//shortDesc = "50% chance to lower the target'S accuracy by 1.",
		id = Moves.LEAFTORNADO,
		//name = "Leaf Tornado",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 50,
			boosts = new Boosts() {
				accuracy = -1
			}
		},
		target = Target.Normal,
		type = Types.GRASS,
		zMovePower = 120,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 670,
		accuracy = 100,
		basePower = 40,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect.",
		id = Moves.LEAFAGE,
		//name = "Leafage",
		pp = 40,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.GRASS,
		zMovePower = 100,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 141,
		accuracy = 100,
		basePower = 80,
		category = Category.PHYSICAL,
		//desc = "The user recovers 1/2 the HP lost by the target, rounded half up. If Big Root is held by the user, the HP recovered is 1.3x normal, rounded half down.",
		//shortDesc = "User recovers 50% of the damage dealt.",
		id = Moves.LEECHLIFE,
		isViable = true,
		//name = "Leech Life",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true, heal: true),
		drain = new int[] {1, 2},
		//secondary = false,
		target = Target.Normal,
		type = Types.BUG,
		zMovePower = 160,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 73,
		accuracy = 90,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The Pokemon at the user's position steals 1/8 of the target'S maximum HP, rounded down, at the end of each turn. If Big Root is held by the recipient, the HP recovered is 1.3x normal, rounded half down. If the target Uses Baton Pass, the replacement will continue being leeched. If the target Switches out or uses Rapid Spin, the effect ends. Grass-type Pokemon are immune to this move on use, but not its effect.",
		//shortDesc = "1/8 of target'S HP is restored to user every turn.",
		id = Moves.LEECHSEED,
		isViable = true,
		//name = "Leech Seed",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true),
		volatileStatus = "leechseed",
		effect = {
			onStart = function (target) {
				this.add('-start', target, 'move = Leech Seed');
			},
			onResidualOrder = 8,
			onResidual = function (pokemon) {
				let target = this.effectData.source.side.active[pokemon.volatiles["leechseed"].sourcePosition];
				if (!target || target.Fainted || target.Hp <= 0) {
					this.debug('Nothing to leech into');
					return;
				}
				let damage = this.damage(pokemon.maxhp / 8, pokemon, target);
				if (damage) {
					this.heal(damage, target, pokemon);
				}
			}
		},
		onTryHit = function (target) {
			if (target.HasType("Grass")) {
				this.add('-immune', target, '[msg]');
				return null;
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.GRASS,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 43,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Lowers the target'S Defense by 1 stage.",
		//shortDesc = "Lowers the foe(s) Defense by 1.",
		id = Moves.LEER,
		//name = "Leer",
		pp = 30,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true),
		boosts = new Boosts() {
			def = -1
		},
		//secondary = false,
		target = Target.AllAdjacentFoes,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {atk = 1},
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 726,
		//accuracy = true,
		basePower = 190,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect.",
		id = Moves.LETSSNUGGLEFOREVER,
		//name = "Let's Snuggle Forever",
		pp = 1,
		priority = 0,
		flags = new Flags (contact: true),
		isZ = "mimikiumz",
		//secondary = false,
		target = Target.Normal,
		type = Types.FAIRY,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 122,
		accuracy = 100,
		basePower = 30,
		category = Category.PHYSICAL,
		//desc = "Has a 30% chance to paralyze the target.",
		//shortDesc = "30% chance to paralyze the target.",
		id = Moves.LICK,
		//name = "Lick",
		pp = 30,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 30,
			status = "par"
		},
		target = Target.Normal,
		type = Types.GHOST,
		zMovePower = 100,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 617,
		accuracy = 90,
		basePower = 140,
		category = Category.SPECIAL,
		//desc = "If the target Lost HP, the user takes recoil damage equal to 1/2 the HP lost by the target, rounded half up, but not less than 1 HP.",
		//shortDesc = "Has 1/2 recoil.",
		id = Moves.LIGHTOFRUIN,
		isViable = true,
		//name = "Light of Ruin",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		isUnreleased = true,
		recoil = new int[] {1, 2},
		//secondary = false,
		target = Target.Normal,
		type = Types.FAIRY,
		zMovePower = 200,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 113,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "For 5 turns, the user and its party members take 0.5x damage from special attacks, or 0.66x damage if in a Double Battle; does not reduce damage further with Aurora Veil. Critical hits ignore this protection. It is removed from the user's side if the user or an ally is successfully hit by Brick Break, Psychic Fangs, or Defog. Lasts for 8 turns if the user is holding Light Clay.",
		//shortDesc = "For 5 turns, special damage to allies is halved.",
		id = Moves.LIGHTSCREEN,
		isViable = true,
		//name = "Light Screen",
		pp = 30,
		priority = 0,
		flags = new Flags (snatch: true),
		sideCondition = "lightscreen",
		effect = {
			duration = 5,
			durationCallback = function (target, source, effect) {
				if (source && source.hasItem("lightclay")) {
					return 8;
				}
				return 5;
			},
			onAnyModifyDamage = function (damage, source, target, move) {
				if (target !== source && target.Side === this.effectData.target && this.getCategory(move) === "Special") {
					if (!move.crit && !move.infiltrates) {
						this.debug('Light Screen weaken');
						if (target.Side.active.length > 1) return this.chainModify([0xAAC, 0x1000]);
						return this.chainModify(0.5);
					}
				}
			},
			onStart = function (side) {
				this.add('-sidestart', side, 'move = Light Screen');
			},
			onResidualOrder = 21,
			onResidualSubOrder = 1,
			onEnd = function (side) {
				this.add('-sideend', side, 'move = Light Screen');
			}
		},
		//secondary = false,
		target = Target.AllySide,
		type = Types.PSYCHIC,
		zMoveBoost = new Boosts() {spd = 1},
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 723,
		//accuracy = true,
		basePower = 200,
		category = Category.SPECIAL,
		//desc = "This move becomes a physical attack if the user's Attack is greater than its Special Attack, including stat stage changes. This move and its effects ignore the Abilities of other Pokemon.",
		//shortDesc = "Physical if user's Atk > Sp. Atk. Ignores Abilities.",
		id = Moves.LIGHTTHATBURNSTHESKY,
		//name = "Light That Burns the Sky",
		pp = 1,
		priority = 0,
		flags = new Flags (),
		onModifyMove = function (move, pokemon) {
			if (pokemon.getStat("atk", false, true) > pokemon.getStat("spa", false, true)) move.category = "Physical";
		},
		ignoreAbility = true,
		isZ = "ultranecroziumz",
		//secondary = false,
		target = Target.Normal,
		type = Types.PSYCHIC,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 710,
		accuracy = 100,
		basePower = 85,
		category = Category.PHYSICAL,
		//desc = "Has a 20% chance to lower the target'S Defense by 1 stage.",
		//shortDesc = "20% chance to lower the target'S Defense by 1.",
		id = Moves.LIQUIDATION,
		isViable = true,
		//name = "Liquidation",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 20,
			boosts = new Boosts() {
				def = -1
			}
		},
		target = Target.Normal,
		type = Types.WATER,
		zMovePower = 160,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 199,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "On the following turn, the target Cannot avoid the user's moves, even if the target Is in the middle of a two-turn move. Fails if the user used this move successfully last turn and that target Is still active.",
		//shortDesc = "User's next move will not miss the target.",
		id = Moves.LOCKON,
		//name = "Lock-On",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		onTryHit = function (target, source) {
			if (source.volatiles["lockon"]) return false;
		},
		onHit = function (target, source) {
			source.addVolatile("lockon", target);
			this.add('-activate', source, 'move = Lock-On', '[of] ' + target);
		},
		effect = {
			noCopy = true, // doesn't get copied by Baton Pass
			duration = 2,
			onSource//Accuracy = function (accuracy, target, source, move) {
				if (move && source === this.effectData.target && target === this.effectData.source) return true;
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {spe = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 142,
		accuracy = 75,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Causes the target To fall asleep.",
		//shortDesc = "Puts the target To sleep.",
		id = Moves.LOVELYKISS,
		isViable = true,
		//name = "Lovely Kiss",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true),
		status = "slp",
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {spe = 1},
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 67,
		accuracy = 100,
		basePower = 0,
		basePowerCallback = function (pokemon, target) {
			let targetWEight = target.GetWeight();
			if (targetWEight >= 200) {
				return 120;
			}
			if (targetWEight >= 100) {
				return 100;
			}
			if (targetWEight >= 50) {
				return 80;
			}
			if (targetWEight >= 25) {
				return 60;
			}
			if (targetWEight >= 10) {
				return 40;
			}
			return 20;
		},
		category = Category.PHYSICAL,
		//desc = "Deals damage to the target Based on its weight. Power is 20 if less than 10 kg, 40 if less than 25 kg, 60 if less than 50 kg, 80 if less than 100 kg, 100 if less than 200 kg, and 120 if greater than or equal to 200 kg.",
		//shortDesc = "More power the heavier the target.",
		id = Moves.LOWKICK,
		isViable = true,
		//name = "Low Kick",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.FIGHTING,
		zMovePower = 160,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 490,
		accuracy = 100,
		basePower = 65,
		category = Category.PHYSICAL,
		//desc = "Has a 100% chance to lower the target'S Speed by 1 stage.",
		//shortDesc = "100% chance to lower the target'S Speed by 1.",
		id = Moves.LOWSWEEP,
		//name = "Low Sweep",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 100,
			boosts = new Boosts() {
				spe = -1
			}
		},
		target = Target.Normal,
		type = Types.FIGHTING,
		zMovePower = 120,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 381,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "For 5 turns, the user and its party members cannot be struck by a critical hit. Fails if this move is already in effect for the user's side.",
		//shortDesc = "For 5 turns, shields user's party from critical hits.",
		id = Moves.LUCKYCHANT,
		//name = "Lucky Chant",
		pp = 30,
		priority = 0,
		flags = new Flags (snatch: true),
		sideCondition = "luckychant",
		effect = {
			duration = 5,
			onStart = function (side) {
				this.add('-sidestart', side, 'move = Lucky Chant'); // "The Lucky Chant shielded [side.name]'s team from critical hits!"
			},
			onCriticalHit = false,
			onResidualOrder = 21,
			onResidualSubOrder = 5,
			onEnd = function (side) {
				this.add('-sideend', side, 'move = Lucky Chant'); // "[side.name]'s team's Lucky Chant wore off!"
			}
		},
		//secondary = false,
		target = Target.AllySide,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {evasion = 1},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 461,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user faints and the Pokemon brought out to replace it has its HP and PP fully restored along with having any major status condition cured. Fails if the user is the last unfainted Pokemon in its party.",
		//shortDesc = "User faints. Replacement is fully healed, with PP.",
		id = Moves.LUNARDANCE,
		isViable = true,
		//name = "Lunar Dance",
		pp = 10,
		priority = 0,
		flags = new Flags (snatch: true, heal: true, dance: true),
		onTryHit = function (pokemon, target, move) {
			if (!this.canSwitch(pokemon.side)) {
				delete move.selfdestruct;
				return false;
			}
		},
		selfdestruct = "ifHit",
		sideCondition = "lunardance",
		effect = {
			duration = 2,
			onStart = function (side, source) {
				this.debug('Lunar Dance started on ' + side.name);
				this.effectData.positions = [];
				// @ts-ignore
				for (const i of side.active.keys()) {
					this.effectData.positions[i] = false;
				}
				this.effectData.positions[source.position] = true;
			},
			onRestart = function (side, source) {
				this.effectData.positions[source.position] = true;
			},
			onSwitchInPriority = 1,
			onSwitchIn = function (target) {
				const positions = //@type {boolean[]}  
					(this.effectData.positions);
				if (target.Position !== this.effectData.sourcePosition) {
					return;
				}
				if (!target.Fainted) {
					target.Heal(target.Maxhp);
					target.SetStatus("");
					for (const moveSlOT of target.MoveSlOTS) {
						moveSlOT.pp = moveSlOT.maxpp;
					}
					this.add('-heal', target, target.GetHealth, '[from] move = Lunar Dance');
					positions[target.Position] = false;
				}
				if (!positions.some(affected => affected === true)) {
					target.Side.removeSiDECONDITION("lunardance");
				}
			}
		},
		//secondary = false,
		target = Target.Self,
		type = Types.PSYCHIC,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 679,
		accuracy = 100,
		basePower = 80,
		category = Category.PHYSICAL,
		//desc = "Has a 100% chance to lower the target'S Attack by 1 stage.",
		//shortDesc = "100% chance to lower the target'S Attack by 1.",
		id = Moves.LUNGE,
		isViable = true,
		//name = "Lunge",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 100,
			boosts = new Boosts() {
				atk = -1
			}
		},
		target = Target.Normal,
		type = Types.BUG,
		zMovePower = 160,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 295,
		accuracy = 100,
		basePower = 70,
		category = Category.SPECIAL,
		//desc = "Has a 50% chance to lower the target'S Special Defense by 1 stage.",
		//shortDesc = "50% chance to lower the target'S Sp. Def by 1.",
		id = Moves.LUSTERPURGE,
		//name = "Luster Purge",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 50,
			boosts = new Boosts() {
				spd = -1
			}
		},
		target = Target.Normal,
		type = Types.PSYCHIC,
		zMovePower = 140,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 183,
		accuracy = 100,
		basePower = 40,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "Usually goes first.",
		id = Moves.MACHPUNCH,
		isViable = true,
		//name = "Mach Punch",
		pp = 30,
		priority = 1,
		flags = new Flags (contact: true, protect: true, mirror: true, punch: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.FIGHTING,
		zMovePower = 100,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 277,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Until the end of the turn, the user is unaffected by certain non-damaging moves DIRECTED at it and will instead use such moves AGAINST the original user. Moves REFLECTED in this way are unable to be reflected again by this or the Ability Magic Bounce's effect. Spikes, Stealth Rock, Sticky Web, and Toxic Spikes can only be reflected once per side, by the leftmost Pokemon under this or the Ability Magic Bounce's effect. If the user has the Ability Soundproof, this move's effect happens before a sound-based move can be nullified. The Abilities Lightning Rod and Storm Drain redirect their respective moves BEFORE this move takes effect.",
		//shortDesc = "Bounces back certain non-damaging moves.",
		id = Moves.MAGICCOAT,
		isViable = true,
		//name = "Magic Coat",
		pp = 15,
		priority = 4,
		flags = new Flags (),
		volatileStatus = "magiccoat",
		effect = {
			duration = 1,
			onStart = function (target, source, effect) {
				this.add('-singleturn', target, 'move = Magic Coat');
				if (effect && effect.effectType === "Move") {
					this.effectData.pranksterBoosted = effect.pranksterBoosted;
				}
			},
			onTryHitPriority = 2,
			onTryHit = function (target, source, move) {
				if (target === source || move.hasBounced || !move.flags["reflectable"]) {
					return;
				}
				let newMove = this.getMoveCopy(move.id);
				newMove.hasBounced = true;
				newMove.pranksterBoosted = this.effectData.pranksterBoosted;
				this.useMove(newMove, target, source);
				return null;
			},
			onAllyTryHitSide = function (target, source, move) {
				if (target.Side === source.side || move.hasBounced || !move.flags["reflectable"]) {
					return;
				}
				let newMove = this.getMoveCopy(move.id);
				newMove.hasBounced = true;
				newMove.pranksterBoosted = false;
				this.useMove(newMove, this.effectData.target, source);
				return null;
			}
		},
		//secondary = false,
		target = Target.Self,
		type = Types.PSYCHIC,
		zMoveBoost = new Boosts() {spd = 2},
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 478,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "For 5 turns, the held items of all active Pokemon have no effect. An item's effect of causing forme changes is unaffected, but any other effects from such items are negated. During the effect, Fling and Natural Gift are prevented from being used by all active Pokemon. If this move is used during the effect, the effect ends.",
		//shortDesc = "For 5 turns, all held items have no effect.",
		id = Moves.MAGICROOM,
		//name = "Magic Room",
		pp = 10,
		priority = 0,
		flags = new Flags (mirror: true),
		pseudoWeather = "magicroom",
		effect = {
			duration = 5,
			durationCallback = function (source, effect) {
				if (source && source.hasAbility("persistent")) {
					this.add('-activate', source, 'ability = Persistent', effect);
					return 7;
				}
				return 5;
			},
			onStart = function (target, source) {
				this.add('-fieldstart', 'move = Magic Room', '[of] ' + source);
			},
			onRestart = function (target, source) {
				this.removePseudoWeather("magicroom");
			},
			// Item suppression implemented in Pokemon.ignoringItem() within sim/pokemon.js
			onResidualOrder = 25,
			onEnd = function () {
				this.add('-fieldend', 'move = Magic Room', '[of] ' + this.effectData.source);
			}
		},
		//secondary = false,
		target = Target.All,
		type = Types.PSYCHIC,
		zMoveBoost = new Boosts() {spd = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 345,
		//accuracy = true,
		basePower = 60,
		category = Category.SPECIAL,
		//desc = "This move does not check accuracy.",
		//shortDesc = "This move does not check accuracy.",
		id = Moves.MAGICALLEAF,
		//name = "Magical Leaf",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.GRASS,
		zMovePower = 120,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 463,
		accuracy = 75,
		basePower = 100,
		category = Category.SPECIAL,
		//desc = "Prevents the target From switching for four or five turns; seven turns if the user is holding Grip Claw. Causes damage to the target Equal to 1/8 of its maximum HP (1/6 if the user is holding Binding Band), rounded down, at the end of each turn during effect. The target Can still switch out if it is holding Shed Shell or uses Baton Pass, Parting Shot, U-turn, or Volt Switch. The effect ends if either the user or the target Leaves the field, or if the target Uses Rapid Spin or Substitute. This effect is not stackable or reset by using this or another partial-trapping move.",
		//shortDesc = "Traps and damages the target For 4-5 turns.",
		id = Moves.MAGMASTORM,
		isViable = true,
		//name = "Magma Storm",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		volatileStatus = "partiallytrapped",
		//secondary = false,
		target = Target.Normal,
		type = Types.FIRE,
		zMovePower = 180,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 443,
		//accuracy = true,
		basePower = 60,
		category = Category.PHYSICAL,
		//desc = "This move does not check accuracy.",
		//shortDesc = "This move does not check accuracy.",
		id = Moves.MAGNETBOMB,
		//name = "Magnet Bomb",
		pp = 20,
		priority = 0,
		flags = new Flags (bullet: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.STEEL,
		zMovePower = 120,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 602,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the Defense and Special Defense of Pokemon on the user's side with the Abilities Plus or Minus by 1 stage.",
		//shortDesc = "Raises Def, Sp. Def of allies with Plus/Minus by 1.",
		id = Moves.MAGNETICFLUX,
		//name = "Magnetic Flux",
		pp = 20,
		priority = 0,
		flags = new Flags (snatch: true, distance: true, authentic: true),
		onHitSide = function (side, source) {
			let targets = [];
			for (let p in side.active) {
				if (side.active[p].hasAbility(["plus", "minus"])) {
					targets.push(side.active[p]);
				}
			}
			if (!targets.length) return false;
			for (const target Of targets) {
				this.boost({def = 1, spd = 1}, target, source, 'move = Magnetic Flux');
			}
		},
		//secondary = false,
		target = Target.AllySide,
		type = Types.ELECTRIC,
		zMoveBoost = new Boosts() {spd = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 393,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "For 5 turns, the user is immune to Ground-type attacks and the effects of Spikes, Toxic Spikes, Sticky Web, and the Ability Arena Trap as long as it remains active. If the user uses Baton Pass, the replacement will gain the effect. Ingrain, Smack Down, Thousand Arrows, and Iron Ball override this move if the user is under any of their effects. Fails if the user is already under this effect or the effects of Ingrain, Smack Down, or Thousand Arrows.",
		//shortDesc = "For 5 turns, the user is immune to Ground moves.",
		id = Moves.MAGNETRISE,
		//name = "Magnet Rise",
		pp = 10,
		priority = 0,
		flags = new Flags (snatch: true, gravity: true),
		volatileStatus = "magnetrise",
		effect = {
			duration = 5,
			onStart = function (target) {
				if (target.Volatiles["smackdown"] || target.Volatiles["ingrain"]) return false;
				this.add('-start', target, 'Magnet Rise');
			},
			onImmunity = function (type) {
				if (type === "Ground") return false;
			},
			onResidualOrder = 15,
			onEnd = function (target) {
				this.add('-end', target, 'Magnet Rise');
			}
		},
		//secondary = false,
		target = Target.Self,
		type = Types.ELECTRIC,
		zMoveBoost = new Boosts() {evasion = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 222,
		accuracy = 100,
		basePower = 0,
		category = Category.PHYSICAL,
		//desc = "The power of this move varies; 5% chances for 10 and 150 power, 10% chances for 30 and 110 power, 20% chances for 50 and 90 power, and 30% chance for 70 power. Damage doubles if the target Is using Dig.",
		//shortDesc = "Hits adjacent Pokemon. Power varies; 2x on Dig.",
		id = Moves.MAGNITUDE,
		//name = "Magnitude",
		pp = 30,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, nonsky: true),
		onModifyMove = function (move, pokemon) {
			let i = this.random(100);
			if (i < 5) {
				move.magnitude = 4;
				move.basePower = 10;
			} else if (i < 15) {
				move.magnitude = 5;
				move.basePower = 30;
			} else if (i < 35) {
				move.magnitude = 6;
				move.basePower = 50;
			} else if (i < 65) {
				move.magnitude = 7;
				move.basePower = 70;
			} else if (i < 85) {
				move.magnitude = 8;
				move.basePower = 90;
			} else if (i < 95) {
				move.magnitude = 9;
				move.basePower = 110;
			} else {
				move.magnitude = 10;
				move.basePower = 150;
			}
		},
		onUseMoveMessage = function (pokemon, target, move) {
			// @ts-ignore
			this.add('-activate', pokemon, 'move = Magnitude', move.magnitude);
		},
		//secondary = false,
		target = Target.AllAdjacent,
		type = Types.GROUND,
		zMovePower = 140,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 696,
		//accuracy = true,
		basePower = 180,
		category = Category.PHYSICAL,
		//desc = "Damage doubles and no accuracy check is done if the target Has used Minimize while active.",
		//shortDesc = "Damage doubles if the target Used Minimize.",
		id = Moves.MALICIOUSMOONSAULT,
		//name = "Malicious Moonsault",
		pp = 1,
		priority = 0,
		flags = new Flags (contact: true),
		isZ = "inciniumz",
		//secondary = false,
		target = Target.Normal,
		type = Types.DARK,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 561,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user and its party members are protected from damaging attacks made by other Pokemon, including allies, during this turn. Fails unless it is the user's first turn on the field, if the user moves LAST this turn, or if this move is already in effect for the user's side.",
		//shortDesc = "Protects allies from attacks. First turn out only.",
		id = Moves.MATBLOCK,
		//name = "Mat Block",
		pp = 10,
		priority = 0,
		flags = new Flags (snatch: true, nonsky: true),
		stallingMove = true,
		sideCondition = "matblock",
		onTryHitSide = function (side, source) {
			if (source.activeTurns > 1) {
				this.add('-hint', "Mat Block only works on your first turn out.");
				return false;
			}
		},
		effect = {
			duration = 1,
			onStart = function (target, source) {
				this.add('-singleturn', source, 'Mat Block');
			},
			onTryHitPriority = 3,
			onTryHit = function (target, source, move) {
				if (!move.flags["protect"]) {
					if (move.isZ) move.zBrokeProtect = true;
					return;
				}
				if (move && (move.target === "self" || move.category === "Status")) return;
				this.add('-activate', target, 'move = Mat Block', move.name);
				source.moveThisTurnResult = true;
				let lockedmove = source.getVolatile("lockedmove");
				if (lockedmove) {
					// Outrage counter is reset
					if (source.volatiles["lockedmove"].duration === 2) {
						delete source.volatiles["lockedmove"];
					}
				}
				return null;
			}
		},
		//secondary = false,
		target = Target.AllySide,
		type = Types.FIGHTING,
		zMoveBoost = new Boosts() {def = 1},
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 382,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user uses the move the target Chose for use this turn against it, if possible, with its power multiplied by 1.5. The move must be a damaging move other than Chatter, Counter, Covet, Focus Punch, Me First, Metal Burst, Mirror Coat, Thief, or Struggle. Fails if the target Moves BEFORE the user. Ignores the target'S substitute for the purpose of copying the move.",
		//shortDesc = "Copies a foe at 1.5x power. User must be faster.",
		id = Moves.MEFIRST,
		//name = "Me First",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, authentic: true),
		onTryHit = function (target, pokemon) {
			let action = this.willMove(target);
			if (action) {
				let noMeFirst = [
					"chatter", "counter", "covet", "focuspunch", "mefirst", "metalburst", "mirrorcoat", "struggle", "thief",
				];
				let move = this.getMoveCopy(action.move.id);
				if (move.category !== "Status" && !noMeFirst.includes(move.id)) {
					pokemon.addVolatile("mefirst");
					this.useMove(move, pokemon, target);
					return null;
				}
			}
			return false;
		},
		effect = {
			duration = 1,
			onBasePowerPriority = 4,
			onBasePower = function (basePower) {
				return this.chainModify(1.5);
			}
		},
		//secondary = false,
		target = Target.AdjacentFoe,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {spe = 2},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 212,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Prevents the target From switching out. The target Can still switch out if it is holding Shed Shell or uses Baton Pass, Parting Shot, U-turn, or Volt Switch. If the target Leaves the field using Baton Pass, the replacement will remain trapped. The effect ends if the user leaves the field.",
		//shortDesc = "The target Cannot switch out.",
		id = Moves.MEANLOOK,
		//name = "Mean Look",
		pp = 5,
		priority = 0,
		flags = new Flags (reflectable: true, mirror: true),
		onHit = function (target, source, move) {
			return target.AddVolatile("trapped", source, move, "trapper");
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {spd = 1},
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 96,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the user's Attack by 1 stage.",
		//shortDesc = "Raises the user's Attack by 1.",
		id = Moves.MEDITATE,
		//name = "Meditate",
		pp = 40,
		priority = 0,
		flags = new Flags (snatch: true),
		boosts = new Boosts() {
			atk = 1
		},
		//secondary = false,
		target = Target.Self,
		type = Types.PSYCHIC,
		zMoveBoost = new Boosts() {atk = 1},
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 72,
		accuracy = 100,
		basePower = 40,
		category = Category.SPECIAL,
		//desc = "The user recovers 1/2 the HP lost by the target, rounded half up. If Big Root is held by the user, the HP recovered is 1.3x normal, rounded half down.",
		//shortDesc = "User recovers 50% of the damage dealt.",
		id = Moves.MEGADRAIN,
		//name = "Mega Drain",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, heal: true),
		drain = new int[] {1, 2},
		//secondary = false,
		target = Target.Normal,
		type = Types.GRASS,
		zMovePower = 120,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 25,
		accuracy = 75,
		basePower = 120,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect.",
		id = Moves.MEGAKICK,
		//name = "Mega Kick",
		pp = 5,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 190,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 5,
		accuracy = 85,
		basePower = 80,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect.",
		id = Moves.MEGAPUNCH,
		//name = "Mega Punch",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true, punch: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 160,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 224,
		accuracy = 85,
		basePower = 120,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect.",
		id = Moves.MEGAHORN,
		isViable = true,
		//name = "Megahorn",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.BUG,
		zMovePower = 190,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 262,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Lowers the target'S Attack and Special Attack by 2 stages. The user faints unless this move misses or there is no target. Fails entirely if this move hits a substitute, but does not fail if the target'S stats cannot be changed.",
		//shortDesc = "Lowers target'S Attack, Sp. Atk by 2. User faints.",
		id = Moves.MEMENTO,
		isViable = true,
		//name = "Memento",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		boosts = new Boosts() {
			atk = -2,
			spa = -2
		},
		selfdestruct = "ifHit",
		//secondary = false,
		target = Target.Normal,
		type = Types.DARK,
		zMoveEffect = "healreplacement",
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 725,
		//accuracy = true,
		basePower = 200,
		category = Category.SPECIAL,
		//desc = "This move and its effects ignore the Abilities of other Pokemon.",
		//shortDesc = "Ignores the Abilities of other Pokemon.",
		id = Moves.MENACINGMOONRAZEMAELSTROM,
		//name = "Menacing Moonraze Maelstrom",
		pp = 1,
		priority = 0,
		flags = new Flags (),
		isZ = "lunaliumz",
		ignoreAbility = true,
		//secondary = false,
		target = Target.Normal,
		type = Types.GHOST,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 368,
		accuracy = 100,
		basePower = 0,
		damageCallback = function (pokemon) {
			if (!pokemon.volatiles["metalburst"]) return 0;
			return pokemon.volatiles["metalburst"].damage || 1;
		},
		category = Category.PHYSICAL,
		//desc = "Deals damage to the last foe to hit the user with an attack this turn equal to 1.5 times the HP lost by the user from that attack. If the user did not lose HP from the attack, this move deals damage with a Base Power of 1 instead. If that foe's position is no longer in use, the damage is done to a random foe in range. Only the last hit of a multi-hit attack is counted. Fails if the user was not hit by a foe's attack this turn.",
		//shortDesc = "If hit by an attack, returns 1.5x damage.",
		id = Moves.METALBURST,
		//name = "Metal Burst",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		beforeTurnCallback = function (pokemon) {
			pokemon.addVolatile("metalburst");
		},
		onTryHit = function (target, source, move) {
			if (!source.volatiles["metalburst"]) return false;
			if (source.volatiles["metalburst"].position === null) return false;
		},
		effect = {
			duration = 1,
			noCopy = true,
			onStart = function (target, source, source2, move) {
				this.effectData.position = null;
				this.effectData.damage = 0;
			},
			onRedirectTargetPRiority = -1,
			onRedirectTarget = function (target, source, source2) {
				if (source !== this.effectData.target) return;
				return source.side.foe.active[this.effectData.position];
			},
			onAfterDamage = function (damage, target, source, effect) {
				if (effect && effect.effectType === "Move" && source.side !== target.Side) {
					this.effectData.position = source.position;
					this.effectData.damage = 1.5 * damage;
				}
			}
		},
		//secondary = false,
		target = Target.Scripted,
		type = Types.STEEL,
		zMovePower = 100,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 232,
		accuracy = 95,
		basePower = 50,
		category = Category.PHYSICAL,
		//desc = "Has a 10% chance to raise the user's Attack by 1 stage.",
		//shortDesc = "10% chance to raise the user's Attack by 1.",
		id = Moves.METALCLAW,
		//name = "Metal Claw",
		pp = 35,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 10,
			self = {
				boosts = new Boosts() {
					atk = 1
				}
			}
		},
		target = Target.Normal,
		type = Types.STEEL,
		zMovePower = 100,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 319,
		accuracy = 85,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Lowers the target'S Special Defense by 2 stages.",
		//shortDesc = "Lowers the target'S Sp. Def by 2.",
		id = Moves.METALSOUND,
		//name = "Metal Sound",
		pp = 40,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true, sound: true, authentic: true, mystery: true),
		boosts = new Boosts() {
			spd = -2
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.STEEL,
		zMoveBoost = new Boosts() {spa = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 309,
		accuracy = 90,
		basePower = 90,
		category = Category.PHYSICAL,
		//desc = "Has a 20% chance to raise the user's Attack by 1 stage.",
		//shortDesc = "20% chance to raise the user's Attack by 1.",
		id = Moves.METEORMASH,
		isViable = true,
		//name = "Meteor Mash",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true, punch: true),
		secondary = new Secondary() {
			chance = 20,
			self = {
				boosts = new Boosts() {
					atk = 1
				}
			}
		},
		target = Target.Normal,
		type = Types.STEEL,
		zMovePower = 175,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 118,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "A random move is selected for use, other than After You, Assist, Baneful Bunker, Beak Blast, Belch, Bestow, Celebrate, Chatter, Copycat, Counter, Covet, Crafty Shield, Destiny Bond, Detect, Diamond Storm, Endure, Feint, Fleur Cannon, Focus Punch, Follow Me, Freeze Shock, Helping Hand, Hold Hands, Hyperspace Hole, Ice Burn, Instruct, King's Shield, Light of Ruin, Mat Block, Me First, Metronome, Mimic, Mind Blown, Mirror Coat, Mirror Move, Nature Power, Photon Geyser, Plasma Fists, Protect, Quash, Quick Guard, Rage Powder, Relic Song, Secret Sword, Shell Trap, Sketch, Sleep Talk, Snarl, Snatch, Snore, Spectral Thief, Spiky Shield, Spotlight, Steam Eruption, Struggle, Switcheroo, Techno Blast, Thief, Thousand Arrows, Thousand Waves, Transform, Trick, V-create, or Wide Guard.",
		//shortDesc = "Picks a random move.",
		id = Moves.METRONOME,
		//name = "Metronome",
		pp = 10,
		priority = 0,
		flags = new Flags (),
		noMetronome = ["afteryou", "assist", "banefulbunker", "beakblast", "belch", "bestow", "celebrate", "chatter", "copycat", "counter", "covet", "craftyshield", "destinybond", "detect", "diamondstorm", "dragonascent", "endure", "feint", "fleurcannon", "focuspunch", "followme", "freezeshock", "helpinghand", "holdhands", "hyperspacefury", "hyperspacehole", "iceburn", "instruct", "kingsshield", "lightofruin", "matblock", "mefirst", "metronome", "mimic", "mindblown", "mirrorcoat", "mirrormove", "naturepower", "originpulse", "photongeyser", "plasmafists", "precipiceblades", "protect", "quash", "quickguard", "ragepowder", "relicsong", "secretsword", "shelltrap", "sketch", "sleeptalk", "snarl", "snatch", "snore", "spectralthief", "spikyshield", "spotlight", "steameruption", "struggle", "switcheroo", "technoblast", "thief", "thousandarrows", "thousandwaves", "transform", "trick", "vcreate", "wideguard"],
		onHit = function (target, source, effect) {
			let moves = [];
			for (let i in exports.BattleMovedex) {
				let move = exports.BattleMovedex[i];
				if (i !== move.id) continue;
				if (move.isZ || move.isNonstandard) continue;
				// @ts-ignore
				if (effect.noMetronome.includes(move.id)) continue;
				if (this.getMove(i).gen > this.gen) continue;
				moves.PUSH(move);
			}
			let randomMove = "";
			if (moves.LENGTH) {
				moves.SORT((a, b) => a.num - b.num);
				randomMove = this.sample(moves).id;
			}
			if (!randomMove) {
				return false;
			}
			this.useMove(randomMove, target);
		},
		//secondary = false,
		target = Target.Self,
		type = Types.NORMAL,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 208,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user restores 1/2 of its maximum HP, rounded half up.",
		//shortDesc = "Heals the user by 50% of its max HP.",
		id = Moves.MILKDRINK,
		isViable = true,
		//name = "Milk Drink",
		pp = 10,
		priority = 0,
		flags = new Flags (snatch: true, heal: true),
		heal = new int[] {1, 2},
		//secondary = false,
		target = Target.Self,
		type = Types.NORMAL,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 102,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "This move is replaced by the last move used by the target. The copied move has the maximum PP for that move. Fails if the target Has not made a move, if the user has Transformed, or if the move is Chatter, Mimic, Sketch, Struggle, or Transform.",
		//shortDesc = "The last move the target Used replaces this one.",
		id = Moves.MIMIC,
		//name = "Mimic",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, authentic: true, mystery: true),
		onHit = function (target, source) {
			let disallowedMoves = ["chatter", "mimic", "sketch", "struggle", "transform"];
			if (source.transformed || !target.LastMove || disallowedMoves.INCLUDES(target.LastMove.id) || source.moves.INDEXOF(target.LastMove.id) >= 0 || target.LastMove.isZ) return false;
			let mimicIndex = source.moves.INDEXOF("mimic");
			if (mimicIndex < 0) return false;
			let move = this.getMove(target.LastMove.id);
			source.moveSlOTS[mimicIndex] = {
				move = move.name,
				id = move.id,
				pp = move.pp,
				maxpp = move.pp,
				target = move.target,
				disabled = false,
				used = false,
				virtual = true
			};
			this.add('-start', source, "Mimic", move.name);
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {accuracy = 1},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 720,
		accuracy = 100,
		basePower = 150,
		category = Category.SPECIAL,
		//desc = "Whether or not this move is successful, the user loses 1/2 of its maximum HP, rounded up, unless the user has the Ability Magic Guard. This move is prevented from executing and the user does not lose HP if any active Pokemon has the Ability Damp, if the user is affected by Powder, or Primordial Sea is in effect.",
		//shortDesc = "User loses 50% max HP. Hits adjacent Pokemon.",
		id = Moves.MINDBLOWN,
		isViable = true,
		//name = "Mind Blown",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		mindBlownRecoil = true,
		onAfterMove = function (pokemon, target, move) {
			if (move.mindBlownRecoil && !move.multihit) {
				this.damage(Math.round(pokemon.maxhp / 2), pokemon, pokemon, this.getEffect('Mind Blown'), true);
			}
		},
		//secondary = false,
		target = Target.AllAdjacent,
		type = Types.FIRE,
		zMovePower = 200,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 170,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "On the following turn, the target Cannot avoid the user's moves, even if the target Is in the middle of a two-turn move. Fails if the user used this move successfully last turn and that target Is still active.",
		//shortDesc = "User's next move will not miss the target.",
		id = Moves.MINDREADER,
		//name = "Mind Reader",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		onTryHit = function (target, source) {
			if (source.volatiles["lockon"]) return false;
		},
		onHit = function (target, source) {
			source.addVolatile("lockon", target);
			this.add('-activate', source, 'move = Mind Reader', '[of] ' + target);
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {spa = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 107,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the user's evasiveness by 2 stages. Whether or not the user's evasiveness was changed, Body Slam, Dragon Rush, Flying Press, Heat Crash, Heavy Slam, Malicious Moonsault, Phantom Force, Shadow Force, Steamroller, and Stomp will not check accuracy and have their damage doubled if used against the user while it is active.",
		//shortDesc = "Raises the user's evasiveness by 2.",
		id = Moves.MINIMIZE,
		//name = "Minimize",
		pp = 10,
		priority = 0,
		flags = new Flags (snatch: true),
		volatileStatus = "minimize",
		effect = {
			noCopy = true,
			onSourceModifyDamage = function (damage, source, target, move) {
				if (["stomp", "steamroller", "bodyslam", "flyingpress", "dragonrush", "phantomforce", "heatcrash", "shadowforce", "heavyslam", "maliciousmoonsault"].includes(move.id)) {
					return this.chainModify(2);
				}
			},
			on//Accuracy = function (accuracy, target, source, move) {
				if (["stomp", "steamroller", "bodyslam", "flyingpress", "dragonrush", "phantomforce", "heatcrash", "shadowforce", "heavyslam", "maliciousmoonsault"].includes(move.id)) {
					return true;
				}
				return accuracy;
			}
		},
		boosts = new Boosts() {
			evasion = 2
		},
		//secondary = false,
		target = Target.Self,
		type = Types.NORMAL,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 357,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Causes the target To have its positive evasiveness stat stage ignored while it is active. Psychic-type attacks can hit the target If it is a Dark type. The effect ends when the target Is no longer active. Fails if the target Is already affected, or affected by Foresight or Odor Sleuth.",
		//shortDesc = "Psychic hits Dark. Evasiveness ignored.",
		id = Moves.MIRACLEEYE,
		//name = "Miracle Eye",
		pp = 40,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true, authentic: true),
		volatileStatus = "miracleeye",
		onTryHit = function (target) {
			if (target.Volatiles["foresight"]) return false;
		},
		effect = {
			noCopy = true,
			onStart = function (pokemon) {
				this.add('-start', pokemon, 'Miracle Eye');
			},
			onNegateImmunity = function (pokemon, type) {
				if (pokemon.hasType("Dark") && type === "Psychic") return false;
			},
			onModifyBoost = function (boosts) {
				if (boosts.evasion && boosts.evasion > 0) {
					boosts.evasion = 0;
				}
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.PSYCHIC,
		zMoveBoost = new Boosts() {spa = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 243,
		accuracy = 100,
		basePower = 0,
		damageCallback = function (pokemon) {
			if (!pokemon.volatiles["mirrorcoat"]) return 0;
			return pokemon.volatiles["mirrorcoat"].damage || 1;
		},
		category = Category.SPECIAL,
		//desc = "Deals damage to the last foe to hit the user with a special attack this turn equal to twice the HP lost by the user from that attack. If the user did not lose HP from the attack, this move deals damage with a Base Power of 1 instead. If that foe's position is no longer in use, the damage is done to a random foe in range. Only the last hit of a multi-hit attack is counted. Fails if the user was not hit by a foe's special attack this turn.",
		//shortDesc = "If hit by special attack, returns double damage.",
		id = Moves.MIRRORCOAT,
		//name = "Mirror Coat",
		pp = 20,
		priority = -5,
		flags = new Flags (protect: true),
		beforeTurnCallback = function (pokemon) {
			pokemon.addVolatile("mirrorcoat");
		},
		onTryHit = function (target, source, move) {
			if (!source.volatiles["mirrorcoat"]) return false;
			if (source.volatiles["mirrorcoat"].position === null) return false;
		},
		effect = {
			duration = 1,
			noCopy = true,
			onStart = function (target, source, source2, move) {
				this.effectData.position = null;
				this.effectData.damage = 0;
			},
			onRedirectTargetPRiority = -1,
			onRedirectTarget = function (target, source, source2) {
				if (source !== this.effectData.target) return;
				return source.side.foe.active[this.effectData.position];
			},
			onAfterDamage = function (damage, target, source, effect) {
				// @ts-ignore
				if (effect && effect.effectType === "Move" && source.side !== target.Side && this.getCategory(effect) === "Special") {
					this.effectData.position = source.position;
					this.effectData.damage = 2 * damage;
				}
			}
		},
		//secondary = false,
		target = Target.Scripted,
		type = Types.PSYCHIC,
		zMovePower = 100,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 119,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user uses the last move used by the target. The copied move is used against that target, if possible. Fails if the target Has not made a move, or the last move used was Acupressure, After You, Aromatherapy, Aromatic Mist, Belch, Conversion 2, Counter, Crafty Shield, Curse, Doom Desire, Electric Terrain, Final Gambit, Flower Shield, Focus Punch, Future Sight, Grassy Terrain, Gravity, Guard Split, Hail, Happy Hour, Haze, Heal Bell, Heal Pulse, Helping Hand, Hold Hands, Ion Deluge, Light Screen, Lucky Chant, Magnetic Flux, Mat Block, Me First, Mimic, Mirror Coat, Mirror Move, Mist, Misty Terrain, Mud Sport, Nature Power, Perish Song, Power Split, Psych Up, Quick Guard, Rain Dance, Reflect, Reflect Type, Role Play, Rototiller, Safeguard, Sandstorm, Sketch, Spikes, Spit Up, Stealth Rock, Sticky Web, Struggle, Sunny Day, Tailwind, Toxic Spikes, Transform, Water Sport, Wide Guard, or any move that is self-targetiNg.",
		//shortDesc = "User uses the target'S last used move against it.",
		id = Moves.MIRRORMOVE,
		//name = "Mirror Move",
		pp = 20,
		priority = 0,
		flags = new Flags (),
		onTryHit = function (target, pokemon) {
			if (!target.LastMove || !target.LastMove.flags["mirror"] || target.LastMove.isZ) {
				return false;
			}
			this.useMove(target.LastMove.id, pokemon, target);
			return null;
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.FLYING,
		zMoveBoost = new Boosts() {atk = 2},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 429,
		accuracy = 85,
		basePower = 65,
		category = Category.SPECIAL,
		//desc = "Has a 30% chance to lower the target'S accuracy by 1 stage.",
		//shortDesc = "30% chance to lower the target'S accuracy by 1.",
		id = Moves.MIRRORSHOT,
		//name = "Mirror Shot",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 30,
			boosts = new Boosts() {
				accuracy = -1
			}
		},
		target = Target.Normal,
		type = Types.STEEL,
		zMovePower = 120,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 54,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "For 5 turns, the user and its party members are protected from having their stats lowered by other Pokemon.",
		//shortDesc = "For 5 turns, protects user's party from stat drops.",
		id = Moves.MIST,
		//name = "Mist",
		pp = 30,
		priority = 0,
		flags = new Flags (snatch: true),
		sideCondition = "mist",
		effect = {
			duration = 5,
			onBoost = function (boost, target, source, effect) {
				if (source && target !== source && (!effect.infiltrates || target.Side === source.side)) {
					let showMsg = false;
					for (let i in boost) {
						// @ts-ignore
						if (boost[i] < 0) {
							// @ts-ignore
							delete boost[i];
							showMsg = true;
						}
					}
					if (showMsg && !effect.secondaries) this.add('-activate', target, 'move = Mist');
				}
			},
			onStart = function (side) {
				this.add('-sidestart', side, "Mist");
			},
			onResidualOrder = 21,
			onResidualSubOrder = 3,
			onEnd = function (side) {
				this.add('-sideend', side, "Mist");
			}
		},
		//secondary = false,
		target = Target.AllySide,
		type = Types.ICE,
		zMoveEffect = "heal",
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 296,
		accuracy = 100,
		basePower = 70,
		category = Category.SPECIAL,
		//desc = "Has a 50% chance to lower the target'S Special Attack by 1 stage.",
		//shortDesc = "50% chance to lower the target'S Sp. Atk by 1.",
		id = Moves.MISTBALL,
		//name = "Mist Ball",
		pp = 5,
		priority = 0,
		flags = new Flags (bullet: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 50,
			boosts = new Boosts() {
				spa = -1
			}
		},
		target = Target.Normal,
		type = Types.PSYCHIC,
		zMovePower = 140,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 581,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "For 5 turns, the terrain becomes Misty Terrain. During the effect, the power of Dragon-type attacks used against grounded Pokemon is multiplied by 0.5 and grounded Pokemon cannot be inflicted with a major status condition nor confusion. Camouflage transforms the user into a Fairy type, Nature Power becomes Moonblast, and Secret Power has a 30% chance to lower Special Attack by 1 stage. Fails if the current terrain is Misty Terrain.",
		//shortDesc = "5 turns. Can't status,-Dragon power vs grounded.",
		id = Moves.MISTYTERRAIN,
		//name = "Misty Terrain",
		pp = 10,
		priority = 0,
		flags = new Flags (nonsky: true),
		terrain = "mistyterrain",
		effect = {
			duration = 5,
			durationCallback = function (source, effect) {
				if (source && source.hasItem("terrainextender")) {
					return 8;
				}
				return 5;
			},
			onSetStatus = function (status, target, source, effect) {
				if (!target.IsGrounded() || target.IsSemiInvulnerable()) return;
				if (effect && effect.status) {
					this.add('-activate', target, 'move = Misty Terrain');
				}
				return false;
			},
			onTryAddVolatile = function (status, target, source, effect) {
				if (!target.IsGrounded() || target.IsSemiInvulnerable()) return;
				if (status.id === "confusion") {
					if (effect.effectType === "Move" && !effect.secondaries) this.add('-activate', target, 'move = Misty Terrain');
					return null;
				}
			},
			onBasePower = function (basePower, attacker, defender, move) {
				if (move.type === "Dragon" && defender.isGrounded() && !defender.isSemiInvulnerable()) {
					this.debug('misty terrain weaken');
					return this.chainModify(0.5);
				}
			},
			onStart = function (battle, source, effect) {
				if (effect && effect.effectType === "Ability") {
					this.add('-fieldstart', 'move = Misty Terrain', '[from] ability = ' + effect, '[of] ' + source);
				} else {
					this.add('-fieldstart', 'move = Misty Terrain');
				}
			},
			onResidualOrder = 21,
			onResidualSubOrder = 2,
			onEnd = function (side) {
				this.add('-fieldend', 'Misty Terrain');
			}
		},
		//secondary = false,
		target = Target.All,
		type = Types.FAIRY,
		zMoveBoost = new Boosts() {spd = 1},
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 585,
		accuracy = 100,
		basePower = 95,
		category = Category.SPECIAL,
		//desc = "Has a 30% chance to lower the target'S Special Attack by 1 stage.",
		//shortDesc = "30% chance to lower the target'S Sp. Atk by 1.",
		id = Moves.MOONBLAST,
		isViable = true,
		//name = "Moonblast",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 30,
			boosts = new Boosts() {
				spa = -1
			}
		},
		target = Target.Normal,
		type = Types.FAIRY,
		zMovePower = 175,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 714,
		accuracy = 100,
		basePower = 100,
		category = Category.SPECIAL,
		//desc = "This move and its effects ignore the Abilities of other Pokemon.",
		//shortDesc = "Ignores the Abilities of other Pokemon.",
		id = Moves.MOONGEISTBEAM,
		isViable = true,
		//name = "Moongeist Beam",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		ignoreAbility = true,
		//secondary = false,
		target = Target.Normal,
		type = Types.GHOST,
		zMovePower = 180,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 236,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user restores 1/2 of its maximum HP if no weather conditions are in effect, 2/3 of its maximum HP if the weather is Sunny Day, and 1/4 of its maximum HP if the weather is Hail, Rain Dance, or Sandstorm, all rounded half down.",
		//shortDesc = "Heals the user by a weather-dependent amount.",
		id = Moves.MOONLIGHT,
		isViable = true,
		//name = "Moonlight",
		pp = 5,
		priority = 0,
		flags = new Flags (snatch: true, heal: true),
		onHit = function (pokemon) {
			if (this.isWeather(["sunnyday", "desolateland"])) {
				return this.heal(this.modify(pokemon.maxhp, 0.667));
			} else if (this.isWeather(["raindance", "primordialsea", "sandstorm", "hail"])) {
				return this.heal(this.modify(pokemon.maxhp, 0.25));
			} else {
				return this.heal(this.modify(pokemon.maxhp, 0.5));
			}
		},
		//secondary = false,
		target = Target.Self,
		type = Types.FAIRY,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 234,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user restores 1/2 of its maximum HP if no weather conditions are in effect, 2/3 of its maximum HP if the weather is Sunny Day, and 1/4 of its maximum HP if the weather is Hail, Rain Dance, or Sandstorm, all rounded half down.",
		//shortDesc = "Heals the user by a weather-dependent amount.",
		id = Moves.MORNINGSUN,
		isViable = true,
		//name = "Morning Sun",
		pp = 5,
		priority = 0,
		flags = new Flags (snatch: true, heal: true),
		onHit = function (pokemon) {
			if (this.isWeather(["sunnyday", "desolateland"])) {
				return this.heal(this.modify(pokemon.maxhp, 0.667));
			} else if (this.isWeather(["raindance", "primordialsea", "sandstorm", "hail"])) {
				return this.heal(this.modify(pokemon.maxhp, 0.25));
			} else {
				return this.heal(this.modify(pokemon.maxhp, 0.5));
			}
		},
		//secondary = false,
		target = Target.Self,
		type = Types.NORMAL,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 189,
		accuracy = 100,
		basePower = 20,
		category = Category.SPECIAL,
		//desc = "Has a 100% chance to lower the target'S accuracy by 1 stage.",
		//shortDesc = "100% chance to lower the target'S accuracy by 1.",
		id = Moves.MUDSLAP,
		//name = "Mud-Slap",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 100,
			boosts = new Boosts() {
				accuracy = -1
			}
		},
		target = Target.Normal,
		type = Types.GROUND,
		zMovePower = 100,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 426,
		accuracy = 85,
		basePower = 65,
		category = Category.SPECIAL,
		//desc = "Has a 30% chance to lower the target'S accuracy by 1 stage.",
		//shortDesc = "30% chance to lower the target'S accuracy by 1.",
		id = Moves.MUDBOMB,
		//name = "Mud Bomb",
		pp = 10,
		priority = 0,
		flags = new Flags (bullet: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 30,
			boosts = new Boosts() {
				accuracy = -1
			}
		},
		target = Target.Normal,
		type = Types.GROUND,
		zMovePower = 120,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 341,
		accuracy = 95,
		basePower = 55,
		category = Category.SPECIAL,
		//desc = "Has a 100% chance to lower the target'S Speed by 1 stage.",
		//shortDesc = "100% chance to lower the target'S Speed by 1.",
		id = Moves.MUDSHOT,
		//name = "Mud Shot",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 100,
			boosts = new Boosts() {
				spe = -1
			}
		},
		target = Target.Normal,
		type = Types.GROUND,
		zMovePower = 100,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 300,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "For 5 turns, all Electric-type attacks used by any active Pokemon have their power reduced to 0.33x. Fails if this move is already in effect.",
		//shortDesc = "For 5 turns, Electric-type attacks have 1/3 power.",
		id = Moves.MUDSPORT,
		//name = "Mud Sport",
		pp = 15,
		priority = 0,
		flags = new Flags (nonsky: true),
		pseudoWeather = "mudsport",
		effect = {
			duration = 5,
			onStart = function (side, source) {
				this.add('-fieldstart', 'move = Mud Sport', '[of] ' + source);
			},
			onBasePowerPriority = 1,
			onBasePower = function (basePower, attacker, defender, move) {
				if (move.type === "Electric") {
					this.debug('mud sport weaken');
					return this.chainModify([0x548, 0x1000]);
				}
			},
			onResidualOrder = 21,
			onEnd = function () {
				this.add('-fieldend', 'move = Mud Sport');
			}
		},
		//secondary = false,
		target = Target.All,
		type = Types.GROUND,
		zMoveBoost = new Boosts() {spd = 1},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 330,
		accuracy = 85,
		basePower = 90,
		category = Category.SPECIAL,
		//desc = "Has a 30% chance to lower the target'S accuracy by 1 stage.",
		//shortDesc = "30% chance to lower the foe(s) accuracy by 1.",
		id = Moves.MUDDYWATER,
		//name = "Muddy Water",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, nonsky: true),
		secondary = new Secondary() {
			chance = 30,
			boosts = new Boosts() {
				accuracy = -1
			}
		},
		target = Target.AllAdjacentFoes,
		type = Types.WATER,
		zMovePower = 175,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 718,
		accuracy = 100,
		basePower = 90,
		category = Category.PHYSICAL,
		//desc = "This move's type depends on the user's held Memory.",
		//shortDesc = "Type varies based on the held Memory.",
		id = Moves.MULTIATTACK,
		isViable = true,
		//name = "Multi-Attack",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		onModifyMove = function (move, pokemon) {
			move.type = this.runEvent("Memory", pokemon, null, "multiattack", "Normal");
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 185,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 595,
		accuracy = 100,
		basePower = 75,
		category = Category.SPECIAL,
		//desc = "Has a 100% chance to lower the target'S Special Attack by 1 stage.",
		//shortDesc = "100% chance to lower the target'S Sp. Atk by 1.",
		id = Moves.MYSTICALFIRE,
		//name = "Mystical Fire",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 100,
			boosts = new Boosts() {
				spa = -1
			}
		},
		target = Target.Normal,
		type = Types.FIRE,
		zMovePower = 140,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 417,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the user's Special Attack by 2 stages.",
		//shortDesc = "Raises the user's Sp. Atk by 2.",
		id = Moves.NASTYPLOT,
		isViable = true,
		//name = "Nasty Plot",
		pp = 20,
		priority = 0,
		flags = new Flags (snatch: true),
		boosts = new Boosts() {
			spa = 2
		},
		//secondary = false,
		target = Target.Self,
		type = Types.DARK,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 363,
		accuracy = 100,
		basePower = 0,
		category = Category.PHYSICAL,
		//desc = "The type and power of this move depend on the user's held Berry, and the Berry is lost. Fails if the user is not holding a Berry, if the user has the Ability Klutz, or if Embargo or Magic Room is in effect for the user.",
		//shortDesc = "Power and type depends on the user's Berry.",
		id = Moves.NATURALGIFT,
		//name = "Natural Gift",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		onPrepareHit = function (target, pokemon, move) {
			if (pokemon.ignoringItem()) return false;
			let item = pokemon.getItem();
			if (!item.naturalGift) return false;
			move.basePower = item.naturalGift.basePower;
			move.type = item.naturalGift.type;
			pokemon.setItem("");
			pokemon.lastItem = item.id;
			pokemon.usedItemThisTurn = true;
			this.runEvent("AfterUseItem", pokemon, null, null, item);
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 160,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 267,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "This move calls another move for use based on the battle terrain. Tri Attack on the regular Wi-Fi terrain, Thunderbolt during Electric Terrain, Moonblast during Misty Terrain, Energy Ball during Grassy Terrain, and Psychic during Psychic Terrain.",
		//shortDesc = "Attack depends on terrain (default Tri Attack).",
		id = Moves.NATUREPOWER,
		isViable = true,
		//name = "Nature Power",
		pp = 20,
		priority = 0,
		flags = new Flags (),
		onTryHit = function (target, pokemon) {
			let move = "triattack";
			if (this.isTerrain("electricterrain")) {
				move = "thunderbolt";
			} else if (this.isTerrain("grassyterrain")) {
				move = "energyball";
			} else if (this.isTerrain("mistyterrain")) {
				move = "moonblast";
			} else if (this.isTerrain("psychicterrain")) {
				move = "psychic";
			}
			this.useMove(move, pokemon, target);
			return null;
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 717,
		accuracy = 90,
		basePower = 0,
		damageCallback = function (pokemon, target) {
			return this.clampIntRange(Math.floor(target.Hp / 2), 1);
		},
		category = Category.SPECIAL,
		//desc = "Deals damage to the target Equal to half of its current HP, rounded down, but not less than 1 HP.",
		//shortDesc = "Does damage equal to 1/2 target'S current HP.",
		id = Moves.NATURESMADNESS,
		isViable = true,
		//name = "Nature's Madness",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.FAIRY,
		zMovePower = 100,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 302,
		accuracy = 100,
		basePower = 60,
		category = Category.PHYSICAL,
		//desc = "Has a 30% chance to flinch the target.",
		//shortDesc = "30% chance to flinch the target.",
		id = Moves.NEEDLEARM,
		//name = "Needle Arm",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 30,
			volatileStatus = "flinch"
		},
		target = Target.Normal,
		type = Types.GRASS,
		zMovePower = 120,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 636,
		//accuracy = true,
		basePower = 1,
		category = Category.PHYSICAL,
		//shortDesc = "Power is equal to the base move's Z-Power.",
		id = Moves.NEVERENDINGNIGHTMARE,
		isViable = true,
		//name = "Never-Ending Nightmare",
		pp = 1,
		priority = 0,
		flags = new Flags (),
		isZ = "ghostiumz",
		//secondary = false,
		target = Target.Normal,
		type = Types.GHOST,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 539,
		accuracy = 95,
		basePower = 85,
		category = Category.SPECIAL,
		//desc = "Has a 40% chance to lower the target'S accuracy by 1 stage.",
		//shortDesc = "40% chance to lower the target'S accuracy by 1.",
		id = Moves.NIGHTDAZE,
		//name = "Night Daze",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 40,
			boosts = new Boosts() {
				accuracy = -1
			}
		},
		target = Target.Normal,
		type = Types.DARK,
		zMovePower = 160,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 101,
		accuracy = 100,
		basePower = 0,
		damage = "level",
		category = Category.SPECIAL,
		//desc = "Deals damage to the target Equal to the user's level.",
		//shortDesc = "Does damage equal to the user's level.",
		id = Moves.NIGHTSHADE,
		isViable = true,
		//name = "Night Shade",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.GHOST,
		zMovePower = 100,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 400,
		accuracy = 100,
		basePower = 70,
		category = Category.PHYSICAL,
		//desc = "Has a higher chance for a critical hit.",
		//shortDesc = "High critical hit ratio.",
		id = Moves.NIGHTSLASH,
		isViable = true,
		//name = "Night Slash",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		critRatio = 2,
		//secondary = false,
		target = Target.Normal,
		type = Types.DARK,
		zMovePower = 140,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 171,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Causes the target To lose 1/4 of its maximum HP, rounded down, at the end of each turn as long as it is asleep. This move does not affect the target Unless it is asleep. The effect ends when the target Wakes up, even if it falls asleep again in the same turn.",
		//shortDesc = "A sleeping target Is hurt by 1/4 max HP per turn.",
		id = Moves.NIGHTMARE,
		//name = "Nightmare",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		volatileStatus = "nightmare",
		effect = {
			noCopy = true,
			onStart = function (pokemon) {
				if (pokemon.status !== "slp" && !pokemon.hasAbility("comatose")) {
					return false;
				}
				this.add('-start', pokemon, "Nightmare");
			},
			onResidualOrder = 9,
			onResidual = function (pokemon) {
				this.damage(pokemon.maxhp / 4);
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.GHOST,
		zMoveBoost = new Boosts() {spa = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 568,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Lowers the target'S Attack and Special Attack by 1 stage.",
		//shortDesc = "Lowers the target'S Attack and Sp. Atk by 1.",
		id = Moves.NOBLEROAR,
		//name = "Noble Roar",
		pp = 30,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true, sound: true, authentic: true),
		boosts = new Boosts() {
			atk = -1,
			spa = -1
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {def = 1},
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 609,
		accuracy = 100,
		basePower = 20,
		category = Category.PHYSICAL,
		//desc = "Has a 100% chance to paralyze the target.",
		//shortDesc = "100% chance to paralyze the target.",
		id = Moves.NUZZLE,
		isViable = true,
		//name = "Nuzzle",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 100,
			status = "par"
		},
		target = Target.Normal,
		type = Types.ELECTRIC,
		zMovePower = 100,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 613,
		accuracy = 100,
		basePower = 80,
		category = Category.SPECIAL,
		//desc = "The user recovers 3/4 the HP lost by the target, rounded half up. If Big Root is held by the user, the HP recovered is 1.3x normal, rounded half down.",
		//shortDesc = "User recovers 75% of the damage dealt.",
		id = Moves.OBLIVIONWING,
		isViable = true,
		//name = "Oblivion Wing",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, distance: true, heal: true),
		drain = new int[] {3, 4},
		//secondary = false,
		target = Target.Any,
		type = Types.FLYING,
		zMovePower = 160,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 697,
		//accuracy = true,
		basePower = 195,
		category = Category.SPECIAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect.",
		id = Moves.OCEANICOPERETTA,
		//name = "Oceanic Operetta",
		pp = 1,
		priority = 0,
		flags = new Flags (),
		isZ = "primariumz",
		//secondary = false,
		target = Target.Normal,
		type = Types.WATER,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 190,
		accuracy = 85,
		basePower = 65,
		category = Category.SPECIAL,
		//desc = "Has a 50% chance to lower the target'S accuracy by 1 stage.",
		//shortDesc = "50% chance to lower the target'S accuracy by 1.",
		id = Moves.OCTAZOOKA,
		//name = "Octazooka",
		pp = 10,
		priority = 0,
		flags = new Flags (bullet: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 50,
			boosts = new Boosts() {
				accuracy = -1
			}
		},
		target = Target.Normal,
		type = Types.WATER,
		zMovePower = 120,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 316,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Causes the target To have its positive evasiveness stat stage ignored while it is active. Normal- and Fighting-type attacks can hit the target If it is a Ghost type. The effect ends when the target Is no longer active. Fails if the target Is already affected, or affected by Foresight or Miracle Eye.",
		//shortDesc = "Fighting, Normal hit Ghost. Evasiveness ignored.",
		id = Moves.ODORSLEUTH,
		//name = "Odor Sleuth",
		pp = 40,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true, authentic: true, mystery: true),
		volatileStatus = "foresight",
		onTryHit = function (target) {
			if (target.Volatiles["miracleeye"]) return false;
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {atk = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 466,
		accuracy = 100,
		basePower = 60,
		category = Category.SPECIAL,
		//desc = "Has a 10% chance to raise the user's Attack, Defense, Special Attack, Special Defense, and Speed by 1 stage.",
		//shortDesc = "10% chance to raise all stats by 1 (not acc/eva).",
		id = Moves.OMINOUSWIND,
		//name = "Ominous Wind",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 10,
			self = {
				boosts = new Boosts() {
					atk = 1,
					def = 1,
					spa = 1,
					spd = 1,
					spe = 1
				}
			}
		},
		target = Target.Normal,
		type = Types.GHOST,
		zMovePower = 120,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 618,
		accuracy = 85,
		basePower = 110,
		category = Category.SPECIAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect. Hits adjacent foes.",
		id = Moves.ORIGINPULSE,
		isViable = true,
		//name = "Origin Pulse",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, pulse: true, mirror: true),
		target = Target.AllAdjacentFoes,
		type = Types.WATER,
		zMovePower = 185,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 200,
		accuracy = 100,
		basePower = 120,
		category = Category.PHYSICAL,
		//desc = "Deals damage to one adjacent foe at random. The user spends two or three turns locked into this move and becomes confused after the last turn of the effect if it is not already. If the user is prevented from moving or the attack is not successful against the target On the first turn of the effect or the second turn of a three-turn effect, the effect ends without causing confusion. If this move is called by Sleep Talk, the move is used for one turn and does not confuse the user.",
		//shortDesc = "Lasts 2-3 turns. Confuses the user afterwards.",
		id = Moves.OUTRAGE,
		isViable = true,
		//name = "Outrage",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		self = {
			volatileStatus = "lockedmove"
		},
		onAfterMove = function (pokemon) {
			if (pokemon.volatiles["lockedmove"] && pokemon.volatiles["lockedmove"].duration === 1) {
				pokemon.removeVolatile("lockedmove");
			}
		},
		//secondary = false,
		target = Target.RandomNormal,
		type = Types.DRAGON,
		zMovePower = 190,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 315,
		accuracy = 90,
		basePower = 130,
		category = Category.SPECIAL,
		//desc = "Lowers the user's Special Attack by 2 stages.",
		//shortDesc = "Lowers the user's Sp. Atk by 2.",
		id = Moves.OVERHEAT,
		isViable = true,
		//name = "Overheat",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		self = {
			boosts = new Boosts() {
				spa = -2
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.FIRE,
		zMovePower = 195,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 220,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user and the target'S HP become the average of their current HP, rounded down, but not more than the maximum HP of either one.",
		//shortDesc = "Shares HP of user and target Equally.",
		id = Moves.PAINSPLIT,
		isViable = true,
		//name = "Pain Split",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, mystery: true),
		onHit = function (target, pokemon) {
			let averagehp = Math.floor((target.Hp + pokemon.hp) / 2) || 1;
			target.Sethp(averagehp);
			pokemon.sethp(averagehp);
			this.add('-sethp', target, target.GetHealth, pokemon, pokemon.getHealth, '[from] move = Pain Split');
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {def = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 570,
		accuracy = 100,
		basePower = 65,
		category = Category.SPECIAL,
		//desc = "The user recovers 1/2 the HP lost by the target, rounded half up. If Big Root is held by the user, the HP recovered is 1.3x normal, rounded half down.",
		//shortDesc = "User recovers 50% of the damage dealt.",
		id = Moves.PARABOLICCHARGE,
		//name = "Parabolic Charge",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, heal: true),
		drain = new int[] {1, 2},
		//secondary = false,
		target = Target.AllAdjacent,
		type = Types.ELECTRIC,
		zMovePower = 120,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 575,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Lowers the target'S Attack and Special Attack by 1 stage. If this move is successful, the user switches out even if it is trapped and is replaced immediately by a selected party member. The user does not switch out if there are no unfainted party members.",
		//shortDesc = "Lowers target'S Atk, Sp. Atk by 1. User switches.",
		id = Moves.PARTINGSHOT,
		isViable = true,
		//name = "Parting Shot",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true, sound: true, authentic: true),
		selfSwitch = true,
		boosts = new Boosts() {
			atk = -1,
			spa = -1
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.DARK,
		zMoveEffect = "healreplacement",
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 6,
		accuracy = 100,
		basePower = 40,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "Scatters coins.",
		id = Moves.PAYDAY,
		//name = "Pay Day",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		onHit = function () {
			this.add('-fieldactivate', 'move = Pay Day');
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 100,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 371,
		accuracy = 100,
		basePower = 50,
		basePowerCallback = function (pokemon, target, move) {
			if (target.NewlySwitched || this.willMove(target)) {
				this.debug('Payback NOT boosted');
				return move.basePower;
			}
			this.debug('Payback damage boost');
			return move.basePower * 2;
		},
		category = Category.PHYSICAL,
		//desc = "Power doubles if the target Moves BEFORE the user; power is not doubled if the target Switches out.",
		//shortDesc = "Power doubles if the user moves AFTER the target.",
		id = Moves.PAYBACK,
		//name = "Payback",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.DARK,
		zMovePower = 100,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 64,
		accuracy = 100,
		basePower = 35,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect.",
		id = Moves.PECK,
		//name = "Peck",
		pp = 35,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true, distance: true),
		//secondary = false,
		target = Target.Any,
		type = Types.FLYING,
		zMovePower = 100,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 195,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Each active Pokemon receives a perish count of 4 if it doesn't already have a perish count. At the end of each turn including the turn used, the perish count of all active Pokemon lowers by 1 and Pokemon faint if the number reaches 0. The perish count is removed from Pokemon that switch out. If a Pokemon uses Baton Pass while it has a perish count, the replacement will gain the perish count and continue to count down.",
		//shortDesc = "All active Pokemon will faint in 3 turns.",
		id = Moves.PERISHSONG,
		isViable = true,
		//name = "Perish Song",
		pp = 5,
		priority = 0,
		flags = new Flags (sound: true, distance: true, authentic: true),
		onHitField = function (target, source, move) {
			let result = false;
			let message = false;
			for (const side of this.sides) {
				for (const pokemon of side.active) {
					if (pokemon && pokemon.isActive) {
						if (!this.runEvent("Accuracy", pokemon, source, move, true)) {
							this.add('-miss', source, pokemon);
							result = true;
						} else if (this.runEvent("TryHit", pokemon, source, move) === null) {
							result = true;
						} else if (!pokemon.volatiles["perishsong"]) {
							pokemon.addVolatile("perishsong");
							this.add('-start', pokemon, "perish3", '[silent]');
							result = true;
							message = true;
						}
					}
				}
			}
			if (!result) return false;
			if (message) this.add('-fieldactivate', 'move = Perish Song');
		},
		effect = {
			duration = 4,
			onEnd = function (target) {
				this.add('-start', target, "perish0");
				target.Faint();
			},
			onResidualOrder = 20,
			onResidual = function (pokemon) {
				let duration = pokemon.volatiles["perishsong"].duration;
				this.add('-start', pokemon, "perish" + duration);
			}
		},
		//secondary = false,
		target = Target.All,
		type = Types.NORMAL,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 572,
		accuracy = 100,
		basePower = 90,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect. Hits adjacent Pokemon.",
		id = Moves.PETALBLIZZARD,
		isViable = true,
		//name = "Petal Blizzard",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.AllAdjacent,
		type = Types.GRASS,
		zMovePower = 175,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 80,
		accuracy = 100,
		basePower = 120,
		category = Category.SPECIAL,
		//desc = "Deals damage to one adjacent foe at random. The user spends two or three turns locked into this move and becomes confused after the last turn of the effect if it is not already. If the user is prevented from moving or the attack is not successful against the target On the first turn of the effect or the second turn of a three-turn effect, the effect ends without causing confusion. If this move is called by Sleep Talk, the move is used for one turn and does not confuse the user.",
		//shortDesc = "Lasts 2-3 turns. Confuses the user afterwards.",
		id = Moves.PETALDANCE,
		//name = "Petal Dance",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true, dance: true),
		self = {
			volatileStatus = "lockedmove"
		},
		onAfterMove = function (pokemon) {
			if (pokemon.volatiles["lockedmove"] && pokemon.volatiles["lockedmove"].duration === 1) {
				pokemon.removeVolatile("lockedmove");
			}
		},
		//secondary = false,
		target = Target.RandomNormal,
		type = Types.GRASS,
		zMovePower = 190,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 566,
		accuracy = 100,
		basePower = 90,
		category = Category.PHYSICAL,
		//desc = "If this move is successful, it breaks through the target'S Baneful Bunker, Detect, King's Shield, Protect, or Spiky Shield for this turn, allowing other Pokemon to attack the target Normally. If the target'S side is protected by Crafty Shield, Mat Block, Quick Guard, or Wide Guard, that protection is also broken for this turn and other Pokemon may attack the target'S side normally. This attack charges on the first turn and executes on the second. On the first turn, the user avoids all attacks. If the user is holding a Power Herb, the move completes in one turn. Damage doubles and no accuracy check is done if the target Has used Minimize while active.",
		//shortDesc = "Disappears turn 1. Hits turn 2. Breaks protection.",
		id = Moves.PHANTOMFORCE,
		//name = "Phantom Force",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, charge: true, mirror: true),
		breaksProtect = true,
		onTry = function (attacker, defender, move) {
			if (attacker.removeVolatile(move.id)) {
				return;
			}
			this.add('-prepare', attacker, move.name, defender);
			if (!this.runEvent("ChargeMove", attacker, defender, move)) {
				this.add('-anim', attacker, move.name, defender);
				return;
			}
			attacker.addVolatile("twoturnmove", defender);
			return null;
		},
		effect = {
			duration = 2,
			onTryImmunity = function (target, source, move) {
				if (move.id === "helpinghand") {
					return;
				}
				if (source.hasAbility("noguard") || target.HasAbility("noguard")) {
					return;
				}
				if (source.volatiles["lockon"] && target === source.volatiles["lockon"].source) return;
				return false;
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.GHOST,
		zMovePower = 175,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 722,
		accuracy = 100,
		basePower = 100,
		category = Category.SPECIAL,
		//desc = "This move becomes a physical attack if the user's Attack is greater than its Special Attack, including stat stage changes. This move and its effects ignore the Abilities of other Pokemon.",
		//shortDesc = "Physical if user's Atk > Sp. Atk. Ignores Abilities.",
		id = Moves.PHOTONGEYSER,
		isViable = true,
		//name = "Photon Geyser",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		onModifyMove = function (move, pokemon) {
			if (pokemon.getStat("atk", false, true) > pokemon.getStat("spa", false, true)) move.category = "Physical";
		},
		ignoreAbility = true,
		//secondary = false,
		target = Target.Normal,
		type = Types.PSYCHIC,
		zMovePower = 180,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 42,
		accuracy = 95,
		basePower = 25,
		category = Category.PHYSICAL,
		//desc = "Hits two to five times. Has a 1/3 chance to hit two or three times, and a 1/6 chance to hit four or five times. If one of the hits breaks the target'S substitute, it will take damage for the remaining hits. If the user has the Ability Skill Link, this move will always hit five times.",
		//shortDesc = "Hits 2-5 times in one turn.",
		id = Moves.PINMISSILE,
		//name = "Pin Missile",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		multihit = new int[] {2, 5},
		//secondary = false,
		target = Target.Normal,
		type = Types.BUG,
		zMovePower = 140,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 721,
		accuracy = 100,
		basePower = 100,
		category = Category.PHYSICAL,
		//desc = "If this move is successful, causes Normal-type moves TO become Electric type this turn.",
		//shortDesc = "Normal moves BECOME Electric type this turn.",
		id = Moves.PLASMAFISTS,
		isViable = true,
		//name = "Plasma Fists",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true, punch: true),
		pseudoWeather = "iondeluge",
		//secondary = false,
		target = Target.Normal,
		type = Types.ELECTRIC,
		zMovePower = 180,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 589,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Lowers the target'S Attack by 1 stage.",
		//shortDesc = "Lowers the target'S Attack by 1.",
		id = Moves.PLAYNICE,
		//name = "Play Nice",
		pp = 20,
		priority = 0,
		flags = new Flags (reflectable: true, mirror: true, authentic: true),
		boosts = new Boosts() {
			atk = -1
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {def = 1},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 583,
		accuracy = 90,
		basePower = 90,
		category = Category.PHYSICAL,
		//desc = "Has a 10% chance to lower the target'S Attack by 1 stage.",
		//shortDesc = "10% chance to lower the target'S Attack by 1.",
		id = Moves.PLAYROUGH,
		isViable = true,
		//name = "Play Rough",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 10,
			boosts = new Boosts() {
				atk = -1
			}
		},
		target = Target.Normal,
		type = Types.FAIRY,
		zMovePower = 175,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 365,
		accuracy = 100,
		basePower = 60,
		category = Category.PHYSICAL,
		//desc = "If this move is successful and the user has not fainted, it steals the target'S held Berry if it is holding one and eats it immediately. Items lost to this move cannot be regained with Recycle or the Ability Harvest.",
		//shortDesc = "User steals and eats the target'S Berry.",
		id = Moves.PLUCK,
		//name = "Pluck",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true, distance: true),
		onHit = function (target, source) {
			let item = target.GetItem();
			if (source.hp && item.isBerry && target.TakeItem(source)) {
				this.add('-enditem', target, item.name, '[from] stealeat', '[move] Pluck', '[of] ' + source);
				if (this.singleEvent("Eat", item, null, source, null, null)) {
					this.runEvent("EatItem", source, null, null, item);
				}
				if (item.onEat) source.ateBerry = true;
			}
		},
		//secondary = false,
		target = Target.Any,
		type = Types.FLYING,
		zMovePower = 120,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 305,
		accuracy = 100,
		basePower = 50,
		category = Category.PHYSICAL,
		//desc = "Has a 50% chance to badly poison the target.",
		//shortDesc = "50% chance to badly poison the target.",
		id = Moves.POISONFANG,
		//name = "Poison Fang",
		pp = 15,
		priority = 0,
		flags = new Flags (bite: true, contact: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 50,
			status = "tox"
		},
		target = Target.Normal,
		type = Types.POISON,
		zMovePower = 100,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 139,
		accuracy = 90,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Poisons the target.",
		//shortDesc = "Poisons the foe(s).",
		id = Moves.POISONGAS,
		//name = "Poison Gas",
		pp = 40,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true),
		status = "psn",
		//secondary = false,
		target = Target.AllAdjacentFoes,
		type = Types.POISON,
		zMoveBoost = new Boosts() {def = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 398,
		accuracy = 100,
		basePower = 80,
		category = Category.PHYSICAL,
		//desc = "Has a 30% chance to poison the target.",
		//shortDesc = "30% chance to poison the target.",
		id = Moves.POISONJAB,
		isViable = true,
		//name = "Poison Jab",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 30,
			status = "psn"
		},
		target = Target.Normal,
		type = Types.POISON,
		zMovePower = 160,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 77,
		accuracy = 75,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Poisons the target.",
		//shortDesc = "Poisons the target.",
		id = Moves.POISONPOWDER,
		//name = "Poison Powder",
		pp = 35,
		priority = 0,
		flags = new Flags (powder: true, protect: true, reflectable: true, mirror: true),
		status = "psn",
		//secondary = false,
		target = Target.Normal,
		type = Types.POISON,
		zMoveBoost = new Boosts() {def = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 40,
		accuracy = 100,
		basePower = 15,
		category = Category.PHYSICAL,
		//desc = "Has a 30% chance to poison the target.",
		//shortDesc = "30% chance to poison the target.",
		id = Moves.POISONSTING,
		//name = "Poison Sting",
		pp = 35,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 30,
			status = "psn"
		},
		target = Target.Normal,
		type = Types.POISON,
		zMovePower = 100,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 342,
		accuracy = 100,
		basePower = 50,
		category = Category.PHYSICAL,
		//desc = "Has a 10% chance to poison the target And a higher chance for a critical hit.",
		//shortDesc = "High critical hit ratio. 10% chance to poison.",
		id = Moves.POISONTAIL,
		//name = "Poison Tail",
		pp = 25,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		critRatio = 2,
		secondary = new Secondary() {
			chance = 10,
			status = "psn"
		},
		target = Target.Normal,
		type = Types.POISON,
		zMovePower = 100,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 676,
		accuracy = 100,
		basePower = 90,
		category = Category.SPECIAL,
		//desc = "If the target Is an ally, this move restores 1/2 of its maximum HP, rounded down, instead of dealing damage.",
		//shortDesc = "If the target Is an ally, heals 50% of its max HP.",
		id = Moves.POLLENPUFF,
		//name = "Pollen Puff",
		pp = 15,
		priority = 0,
		flags = new Flags (bullet: true, protect: true, mirror: true),
		onTryHit = function (target, source, move) {
			if (source.side === target.Side) {
				move.basePower = 0;
				move.heal = [1, 2];
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.BUG,
		zMovePower = 175,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 1,
		accuracy = 100,
		basePower = 40,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect.",
		id = Moves.POUND,
		//name = "Pound",
		pp = 35,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 100,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 600,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "If the target Uses a Fire-type move this turn, it is prevented from executing and the target Loses 1/4 of its maximum HP, rounded half up. This effect does not happen if the Fire-type move is prevented by Primordial Sea.",
		//shortDesc = "If using a Fire move, target Loses 1/4 max HP.",
		id = Moves.POWDER,
		//name = "Powder",
		pp = 20,
		priority = 1,
		flags = new Flags (powder: true, protect: true, reflectable: true, mirror: true, authentic: true),
		volatileStatus = "powder",
		effect = {
			duration = 1,
			onStart = function (target) {
				this.add('-singleturn', target, "Powder");
			},
			onTryMovePriority = -1,
			onTryMove = function (pokemon, target, move) {
				if (move.type === "Fire") {
					this.add('-activate', pokemon, 'move = Powder');
					this.damage(this.clampIntRange(Math.round(pokemon.maxhp / 4), 1));
					return false;
				}
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.BUG,
		zMoveBoost = new Boosts() {spd = 2},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 181,
		accuracy = 100,
		basePower = 40,
		category = Category.SPECIAL,
		//desc = "Has a 10% chance to freeze the target.",
		//shortDesc = "10% chance to freeze the foe(s).",
		id = Moves.POWDERSNOW,
		//name = "Powder Snow",
		pp = 25,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 10,
			status = "frz"
		},
		target = Target.AllAdjacentFoes,
		type = Types.ICE,
		zMovePower = 100,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 408,
		accuracy = 100,
		basePower = 80,
		category = Category.SPECIAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect.",
		id = Moves.POWERGEM,
		isViable = true,
		//name = "Power Gem",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.ROCK,
		zMovePower = 160,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 471,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user and the target Have their Attack and Special Attack stats set to be equal to the average of the user and the target'S Attack and Special Attack stats, respectively, rounded down. Stat stage changes are unaffected.",
		//shortDesc = "Averages Attack and Sp. Atk stats with target.",
		id = Moves.POWERSPLIT,
		//name = "Power Split",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mystery: true),
		onHit = function (target, source) {
			let newatk = Math.floor((target.Stats.atk + source.stats.atk) / 2);
			target.Stats.atk = newatk;
			source.stats.atk = newatk;
			let newspa = Math.floor((target.Stats.spa + source.stats.spa) / 2);
			target.Stats.spa = newspa;
			source.stats.spa = newspa;
			this.add('-activate', source, 'move = Power Split', '[of] ' + target);
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.PSYCHIC,
		zMoveBoost = new Boosts() {spe = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 384,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user swaps its Attack and Special Attack stat stage changes with the target.",
		//shortDesc = "Swaps Attack and Sp. Atk stat stages with target.",
		id = Moves.POWERSWAP,
		//name = "Power Swap",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, authentic: true, mystery: true),
		onHit = function (target, source) {
			let targetBOosts = {};
			let sourceBoosts = {};

			for (const stat of ["atk", "spa"]) {
				targetBOosts[stat] = target.Boosts[stat];
				sourceBoosts[stat] = source.boosts[stat];
			}

			source.setBoost(targetBOosts);
			target.SetBoost(sourceBoosts);

			this.add('-swapboost', source, target, 'atk, spa', '[from] move = Power Swap');
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.PSYCHIC,
		zMoveBoost = new Boosts() {spe = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 379,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user swaps its Attack and Defense stats; stat stage changes remain on their respective stats. This move can be used again to swap the stats back. If the user uses Baton Pass, the replacement will have its Attack and Defense stats swapped if the effect is active. If the user has its stats recalculated by changing forme while its stats are swapped, this effect is ignored but is still active for the purposes of Baton Pass.",
		//shortDesc = "Switches user's Attack and Defense stats.",
		id = Moves.POWERTRICK,
		//name = "Power Trick",
		pp = 10,
		priority = 0,
		flags = new Flags (snatch: true),
		volatileStatus = "powertrick",
		effect = {
			onStart = function (pokemon) {
				this.add('-start', pokemon, 'Power Trick');
				let newatk = pokemon.stats.def;
				let newdef = pokemon.stats.atk;
				pokemon.stats.atk = newatk;
				pokemon.stats.def = newdef;
			},
			onCopy = function (pokemon) {
				let newatk = pokemon.stats.def;
				let newdef = pokemon.stats.atk;
				pokemon.stats.atk = newatk;
				pokemon.stats.def = newdef;
			},
			onEnd = function (pokemon) {
				this.add('-end', pokemon, 'Power Trick');
				let newatk = pokemon.stats.def;
				let newdef = pokemon.stats.atk;
				pokemon.stats.atk = newatk;
				pokemon.stats.def = newdef;
			},
			onRestart = function (pokemon) {
				pokemon.removeVolatile('Power Trick');
			}
		},
		//secondary = false,
		target = Target.Self,
		type = Types.PSYCHIC,
		zMoveBoost = new Boosts() {atk = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 681,
		accuracy = 100,
		basePower = 20,
		basePowerCallback = function (pokemon, target, move) {
			return move.basePower + 20 * pokemon.positiveBoosts();
		},
		category = Category.PHYSICAL,
		//desc = "Power is equal to 20+(X*20), where X is the user's total stat stage changes that are greater than 0.",
		//shortDesc = " + 20 power for each of the user's stat boosts.",
		id = Moves.POWERTRIP,
		//name = "Power Trip",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.DARK,
		zMovePower = 160,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 612,
		accuracy = 100,
		basePower = 40,
		category = Category.PHYSICAL,
		//desc = "Has a 100% chance to raise the user's Attack by 1 stage.",
		//shortDesc = "100% chance to raise the user's Attack by 1.",
		id = Moves.POWERUPPUNCH,
		isViable = true,
		//name = "Power-Up Punch",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true, punch: true),
		secondary = new Secondary() {
			chance = 100,
			self = {
				boosts = new Boosts() {
					atk = 1
				}
			}
		},
		target = Target.Normal,
		type = Types.FIGHTING,
		zMovePower = 100,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 438,
		accuracy = 85,
		basePower = 120,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect.",
		id = Moves.POWERWHIP,
		isViable = true,
		//name = "Power Whip",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.GRASS,
		zMovePower = 190,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 619,
		accuracy = 85,
		basePower = 120,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect. Hits adjacent foes.",
		id = Moves.PRECIPICEBLADES,
		isViable = true,
		//name = "Precipice Blades",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, nonsky: true),
		target = Target.AllAdjacentFoes,
		type = Types.GROUND,
		zMovePower = 190,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 217,
		accuracy = 90,
		basePower = 0,
		category = Category.PHYSICAL,
		//desc = "Deals damage or heals the target. 40% chance for 40 power, 30% chance for 80 power, 10% chance for 120 power, and 20% chance to heal the target By 1/4 of its maximum HP, rounded down. This move must hit to be effective.",
		//shortDesc = "40, 80, 120 power, or heals target By 1/4 max HP.",
		id = Moves.PRESENT,
		//name = "Present",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		onModifyMove = function (move, pokemon, target) {
			let rand = this.random(10);
			if (rand < 2) {
				move.heal = [1, 4];
			} else if (rand < 6) {
				move.basePower = 40;
			} else if (rand < 9) {
				move.basePower = 80;
			} else {
				move.basePower = 120;
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 100,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 711,
		accuracy = 100,
		basePower = 160,
		category = Category.SPECIAL,
		//desc = "If this move is successful, the user must recharge on the following turn and cannot make a move.",
		//shortDesc = "User cannot move next turn.",
		id = Moves.PRISMATICLASER,
		//name = "Prismatic Laser",
		pp = 10,
		priority = 0,
		flags = new Flags (recharge: true, protect: true, mirror: true),
		self = {
			volatileStatus = "mustrecharge"
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.PSYCHIC,
		zMovePower = 200,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 182,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user is protected from most attacks made by other Pokemon during this turn. This move has a 1/X chance of being successful, where X starts at 1 and triples each time this move is successfully used. X resets to 1 if this move fails or if the user's last move used is not Baneful Bunker, Detect, Endure, King's Shield, Protect, Quick Guard, Spiky Shield, or Wide Guard. Fails if the user moves LAST this turn.",
		//shortDesc = "Prevents moves FROM affecting the user this turn.",
		id = Moves.PROTECT,
		isViable = true,
		//name = "Protect",
		pp = 10,
		priority = 4,
		flags = new Flags (),
		stallingMove = true,
		volatileStatus = "protect",
		onPrepareHit = function (pokemon) {
			return !!this.willAct() && this.runEvent("StallMove", pokemon);
		},
		onHit = function (pokemon) {
			pokemon.addVolatile("stall");
		},
		effect = {
			duration = 1,
			onStart = function (target) {
				this.add('-singleturn', target, "Protect");
			},
			onTryHitPriority = 3,
			onTryHit = function (target, source, move) {
				if (!move.flags["protect"]) {
					if (move.isZ) move.zBrokeProtect = true;
					return;
				}
				this.add('-activate', target, 'move = Protect');
				source.moveThisTurnResult = true;
				let lockedmove = source.getVolatile("lockedmove");
				if (lockedmove) {
					// Outrage counter is reset
					if (source.volatiles["lockedmove"].duration === 2) {
						delete source.volatiles["lockedmove"];
					}
				}
				return null;
			}
		},
		//secondary = false,
		target = Target.Self,
		type = Types.NORMAL,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 60,
		accuracy = 100,
		basePower = 65,
		category = Category.SPECIAL,
		//desc = "Has a 10% chance to confuse the target.",
		//shortDesc = "10% chance to confuse the target.",
		id = Moves.PSYBEAM,
		//name = "Psybeam",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 10,
			volatileStatus = "confusion"
		},
		target = Target.Normal,
		type = Types.PSYCHIC,
		zMovePower = 120,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 244,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user copies all of the target'S current stat stage changes.",
		//shortDesc = "Copies the target'S current stat stages.",
		id = Moves.PSYCHUP,
		//name = "Psych Up",
		pp = 10,
		priority = 0,
		flags = new Flags (authentic: true, mystery: true),
		onHit = function (target, source) {
			for (let i in target.Boosts) {
				source.boosts[i] = target.Boosts[i];
			}
			this.add('-copyboost', source, target, '[from] move = Psych Up');
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMoveEffect = "heal",
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 94,
		accuracy = 100,
		basePower = 90,
		category = Category.SPECIAL,
		//desc = "Has a 10% chance to lower the target'S Special Defense by 1 stage.",
		//shortDesc = "10% chance to lower the target'S Sp. Def by 1.",
		id = Moves.PSYCHIC,
		isViable = true,
		//name = "Psychic",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 10,
			boosts = new Boosts() {
				spd = -1
			}
		},
		target = Target.Normal,
		type = Types.PSYCHIC,
		zMovePower = 175,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 706,
		accuracy = 100,
		basePower = 85,
		category = Category.PHYSICAL,
		//desc = "If this attack does not miss, the effects of Reflect, Light Screen, and Aurora Veil end for the target'S side of the field before damage is calculated.",
		//shortDesc = "Destroys screens, unless the target Is immune.",
		id = Moves.PSYCHICFANGS,
		isViable = true,
		//name = "Psychic Fangs",
		pp = 10,
		priority = 0,
		flags = new Flags (bite: true, contact: true, protect: true, mirror: true),
		onTryHit = function (pokemon) {
			// will shatter screens through sub, before you hit
			if (pokemon.runImmunity("Psychic")) {
				pokemon.side.removeSiDECONDITION("reflect");
				pokemon.side.removeSiDECONDITION("lightscreen");
				pokemon.side.removeSiDECONDITION("auroraveil");
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.PSYCHIC,
		zMovePower = 160,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 678,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "For 5 turns, the terrain becomes Psychic Terrain. During the effect, the power of Psychic-type attacks made by grounded Pokemon is multiplied by 1.5 and grounded Pokemon cannot be hit by moves WITH priority greater than 0, unless the target Is an ally. Camouflage transforms the user into a Psychic type, Nature Power becomes Psychic, and Secret Power has a 30% chance to lower the target'S Speed by 1 stage. Fails if the current terrain is Psychic Terrain.",
		//shortDesc = "5 turns. Grounded = +Psychic power, priority-safe.",
		id = Moves.PSYCHICTERRAIN,
		//name = "Psychic Terrain",
		pp = 10,
		priority = 0,
		flags = new Flags (nonsky: true),
		terrain = "psychicterrain",
		effect = {
			duration = 5,
			durationCallback = function (source, effect) {
				if (source && source.hasItem("terrainextender")) {
					return 8;
				}
				return 5;
			},
			onTryHitPriority = 4,
			onTryHit = function (target, source, effect) {
				if (!target.IsGrounded() || target.IsSemiInvulnerable() || target.Side === source.side) return;
				if (effect && (effect.priority <= 0.1 || effect.target === "self")) {
					return;
				}
				this.add('-activate', target, 'move = Psychic Terrain');
				return null;
			},
			onBasePower = function (basePower, attacker, defender, move) {
				if (move.type === "Psychic" && attacker.isGrounded() && !attacker.isSemiInvulnerable()) {
					this.debug('psychic terrain boost');
					return this.chainModify(1.5);
				}
			},
			onStart = function (battle, source, effect) {
				if (effect && effect.effectType === "Ability") {
					this.add('-fieldstart', 'move = Psychic Terrain', '[from] ability = ' + effect, '[of] ' + source);
				} else {
					this.add('-fieldstart', 'move = Psychic Terrain');
				}
			},
			onResidualOrder = 21,
			onResidualSubOrder = 2,
			onEnd = function () {
				this.add('-fieldend', 'move = Psychic Terrain');
			}
		},
		//secondary = false,
		target = Target.All,
		type = Types.PSYCHIC,
		zMoveBoost = new Boosts() {spa = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 354,
		accuracy = 90,
		basePower = 140,
		category = Category.SPECIAL,
		//desc = "Lowers the user's Special Attack by 2 stages.",
		//shortDesc = "Lowers the user's Sp. Atk by 2.",
		id = Moves.PSYCHOBOOST,
		isViable = true,
		//name = "Psycho Boost",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		self = {
			boosts = new Boosts() {
				spa = -2
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.PSYCHIC,
		zMovePower = 200,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 427,
		accuracy = 100,
		basePower = 70,
		category = Category.PHYSICAL,
		//desc = "Has a higher chance for a critical hit.",
		//shortDesc = "High critical hit ratio.",
		id = Moves.PSYCHOCUT,
		isViable = true,
		//name = "Psycho Cut",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		critRatio = 2,
		//secondary = false,
		target = Target.Normal,
		type = Types.PSYCHIC,
		zMovePower = 140,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 375,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user's major status condition is transferred to the target, and the user is then cured. Fails if the target Already has a major status condition.",
		//shortDesc = "Transfers the user's status ailment to the target.",
		id = Moves.PSYCHOSHIFT,
		//name = "Psycho Shift",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		onPrepareHit = function (target, source, move) {
			if (!source.status) return false;
			move.status = source.status;
		},
		self = {
			onHit = function (pokemon) {
				pokemon.cureStatus();
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.PSYCHIC,
		zMoveBoost = new Boosts() {spa = 2},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 473,
		accuracy = 100,
		basePower = 80,
		category = Category.SPECIAL,
		defensivecategory = Category.PHYSICAL,
		//desc = "Deals damage to the target Based on its Defense instead of Special Defense.",
		//shortDesc = "Damages target Based on Defense, not Sp. Def.",
		id = Moves.PSYSHOCK,
		isViable = true,
		//name = "Psyshock",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.PSYCHIC,
		zMovePower = 160,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 540,
		accuracy = 100,
		basePower = 100,
		category = Category.SPECIAL,
		defensivecategory = Category.PHYSICAL,
		//desc = "Deals damage to the target Based on its Defense instead of Special Defense.",
		//shortDesc = "Damages target Based on Defense, not Sp. Def.",
		id = Moves.PSYSTRIKE,
		isViable = true,
		//name = "Psystrike",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.PSYCHIC,
		zMovePower = 180,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 149,
		accuracy = 100,
		basePower = 0,
		damageCallback = function (pokemon) {
			return (this.random(50, 151) * pokemon.level) / 100;
		},
		category = Category.SPECIAL,
		//desc = "Deals damage to the target Equal to (user's level) * (X+50) / 100, where X is a random number from 0 to 100, rounded down, but not less than 1 HP.",
		//shortDesc = "Random damage equal to 0.5x-1.5x user's level.",
		id = Moves.PSYWAVE,
		//name = "Psywave",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.PSYCHIC,
		zMovePower = 100,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 701,
		//accuracy = true,
		basePower = 210,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect.",
		id = Moves.PULVERIZINGPANCAKE,
		//name = "Pulverizing Pancake",
		pp = 1,
		priority = 0,
		flags = new Flags (contact: true),
		isZ = "snorliumz",
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 386,
		accuracy = 100,
		basePower = 0,
		basePowerCallback = function (pokemon, target) {
			let power = 60 + 20 * target.PositiveBoosts();
			if (power > 200) power = 200;
			return power;
		},
		category = Category.PHYSICAL,
		//desc = "Power is equal to 60+(X*20), where X is the target'S total stat stage changes that are greater than 0, but not more than 200 power.",
		//shortDesc = "60 power +20 for each of the target'S stat boosts.",
		id = Moves.PUNISHMENT,
		//name = "Punishment",
		pp = 5,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.DARK,
		zMovePower = 160,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 685,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The target Is cured if it has a major status condition. If the target Was cured, the user restores 1/2 of its maximum HP, rounded half up.",
		//shortDesc = "Cures target'S status; heals user 1/2 maxHP if so.",
		id = Moves.PURIFY,
		//name = "Purify",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, heal: true),
		onHit = function (target, source) {
			if (!target.CureStatus()) return false;
			this.heal(Math.ceil(source.maxhp * 0.5), source);
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.POISON,
		zMoveBoost = new Boosts() {atk = 1, def = 1, spa = 1, spd = 1, spe = 1},
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 228,
		accuracy = 100,
		basePower = 40,
		basePowerCallback = function (pokemon, target, move) {
			// You can't get here unless the pursuit succeeds
			if (target.BeingCalledBack) {
				this.debug('Pursuit damage boost');
				return move.basePower * 2;
			}
			return move.basePower;
		},
		category = Category.PHYSICAL,
		//desc = "If an adjacent foe switches out this turn, this move hits that Pokemon before it leaves the field, even if it was not the original target. If the user moves AFTER a foe using Parting Shot, U-turn, or Volt Switch, but not Baton Pass, it will hit that foe before it leaves the field. Power doubles and no accuracy check is done if the user hits a foe switching out, and the user's turn is over; if a foe faints from this, the replacement Pokemon does not become active until the end of the turn.",
		//shortDesc = "Power doubles if a foe is switching out.",
		id = Moves.PURSUIT,
		isViable = true,
		//name = "Pursuit",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		beforeTurnCallback = function (pokemon) {
			for (const side of this.sides) {
				if (side === pokemon.side) continue;
				side.addSideCondition("pursuit", pokemon);
				if (!side.sideConditions["pursuit"].sources) {
					side.sideConditions["pursuit"].sources = [];
				}
				side.sideConditions["pursuit"].sources.push(pokemon);
			}
		},
		onModifyMove = function (move, source, target) {
			if (target && target.BeingCalledBack) move.//accuracy = true;
		},
		onTryHit = function (target, pokemon) {
			target.Side.removeSiDECONDITION("pursuit");
		},
		effect = {
			duration = 1,
			onBeforeSwitchOut = function (pokemon) {
				this.debug('Pursuit start');
				let alreadyAdded = false;
				for (const source of this.effectData.sources) {
					if (!this.cancelMove(source) || !source.hp) continue;
					if (!alreadyAdded) {
						this.add('-activate', pokemon, 'move = Pursuit');
						alreadyAdded = true;
					}
					// Run through each action in queue to check if the Pursuit user is supposed to Mega Evolve this turn.
					// If it is, then Mega Evolve before moving.
					if (source.canMegaEvo || source.canUltraBurst) {
						for (const [actionIndex, action] of this.queue.entries()) {
							if (action.pokemon === source && action.choice === "megaEvo") {
								this.runMegaEvo(source);
								this.queue.splice(actionIndex, 1);
								break;
							}
						}
					}
					this.runMove("pursuit", source, this.getTargetLOc(pokemon, source));
				}
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.DARK,
		zMovePower = 100,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 511,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Causes the target To take its turn after all other Pokemon this turn, no matter the priority of its selected move. Fails if the target Already moved this turn.",
		//shortDesc = "Forces the target To move last this turn.",
		id = Moves.QUASH,
		//name = "Quash",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		onHit = function (target) {
			if (target.Side.active.length < 2) return false; // fails in singles
			let action = this.willMove(target);
			if (action) {
				action.priority = -7.1;
				this.cancelMove(target);
				for (let i = this.queue.length - 1; i >= 0; i--) {
					if (this.queue[i].choice === "residual") {
						this.queue.splice(i, 0, action);
						break;
					}
				}
				this.add('-activate', target, 'move = Quash');
			} else {
				return false;
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.DARK,
		zMoveBoost = new Boosts() {spe = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 98,
		accuracy = 100,
		basePower = 40,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "Usually goes first.",
		id = Moves.QUICKATTACK,
		isViable = true,
		//name = "Quick Attack",
		pp = 30,
		priority = 1,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 100,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 501,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user and its party members are protected from attacks with original or altered priority greater than 0 made by other Pokemon, including allies, during this turn. This move modifies the same 1/X chance of being successful used by other protection moves, where X starts at 1 and triples each time this move is successfully used, but does not use the chance to check for failure. X resets to 1 if this move fails or if the user's last move used is not Baneful Bunker, Detect, Endure, King's Shield, Protect, Quick Guard, Spiky Shield, or Wide Guard. Fails if the user moves LAST this turn or if this move is already in effect for the user's side.",
		//shortDesc = "Protects allies from priority attacks this turn.",
		id = Moves.QUICKGUARD,
		//name = "Quick Guard",
		pp = 15,
		priority = 3,
		flags = new Flags (snatch: true),
		sideCondition = "quickguard",
		onTryHitSide = function (side, source) {
			return this.willAct();
		},
		onHitSide = function (side, source) {
			source.addVolatile("stall");
		},
		effect = {
			duration = 1,
			onStart = function (target, source) {
				this.add('-singleturn', source, 'Quick Guard');
			},
			onTryHitPriority = 4,
			onTryHit = function (target, source, move) {
				// Quick Guard blocks moves WITH positive priority, even those given increased priority by Prankster or Gale Wings.
				// (e.g. it blocks 0 priority moves BOOSTED by Prankster or Gale Wings; Quick Claw/Custap Berry do not count)
				if (move.priority <= 0.1) return;
				if (!move.flags["protect"]) {
					if (move.isZ) move.zBrokeProtect = true;
					return;
				}
				this.add('-activate', target, 'move = Quick Guard');
				source.moveThisTurnResult = true;
				let lockedmove = source.getVolatile("lockedmove");
				if (lockedmove) {
					// Outrage counter is reset
					if (source.volatiles["lockedmove"].duration === 2) {
						delete source.volatiles["lockedmove"];
					}
				}
				return null;
			}
		},
		//secondary = false,
		target = Target.AllySide,
		type = Types.FIGHTING,
		zMoveBoost = new Boosts() {def = 1},
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 483,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the user's Special Attack, Special Defense, and Speed by 1 stage.",
		//shortDesc = "Raises the user's Sp. Atk, Sp. Def, Speed by 1.",
		id = Moves.QUIVERDANCE,
		isViable = true,
		//name = "Quiver Dance",
		pp = 20,
		priority = 0,
		flags = new Flags (snatch: true, dance: true),
		boosts = new Boosts() {
			spa = 1,
			spd = 1,
			spe = 1
		},
		//secondary = false,
		target = Target.Self,
		type = Types.BUG,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 99,
		accuracy = 100,
		basePower = 20,
		category = Category.PHYSICAL,
		//desc = "Once this move is successfuly used, the user's Attack is raised by 1 stage every time it is hit by another Pokemon's attack as long as this move is chosen for use.",
		//shortDesc = "Raises the user's Attack by 1 if hit during use.",
		id = Moves.RAGE,
		//name = "Rage",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		self = {
			volatileStatus = "rage"
		},
		effect = {
			onStart = function (pokemon) {
				this.add('-singlemove', pokemon, "Rage");
			},
			onHit = function (target, source, move) {
				if (target !== source && move.category !== "Status") {
					this.boost({atk = 1});
				}
			},
			onBeforeMovePriority = 100,
			onBeforeMove = function (pokemon) {
				this.debug('removing Rage before attack');
				pokemon.removeVolatile("rage");
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 100,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 476,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Until the end of the turn, all single-target Attacks from the foe's team are redirected to the user if they are in range. Such attacks are redirected to the user before they can be reflected by Magic Coat or the Ability Magic Bounce, or drawn in by the Abilities Lightning Rod or Storm Drain. Fails if it is not a Double or Triple Battle.",
		//shortDesc = "The foes' moves TARGET The user on the turn used.",
		id = Moves.RAGEPOWDER,
		//name = "Rage Powder",
		pp = 20,
		priority = 2,
		flags = new Flags (powder: true),
		volatileStatus = "ragepowder",
		onTryHit = function (target) {
			if (target.Side.active.length < 2) return false;
		},
		effect = {
			duration = 1,
			onStart = function (pokemon) {
				this.add('-singleturn', pokemon, 'move = Rage Powder');
			},
			onFoeRedirectTargetPRiority = 1,
			onFoeRedirectTarget = function (target, source, source2, move) {
				if (!this.effectData.target.IsSkyDropped() && source.runStatusImmunity("powder") && this.validTarget(This.effectData.target, source, move.target)) {
					this.debug("Rage Powder redirected target Of move");
					return this.effectData.target;
				}
			}
		},
		//secondary = false,
		target = Target.Self,
		type = Types.BUG,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 240,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "For 5 turns, the weather becomes Rain Dance. The damage of Water-type attacks is multiplied by 1.5 and the damage of Fire-type attacks is multiplied by 0.5 during the effect. Lasts for 8 turns if the user is holding Damp Rock. Fails if the current weather is Rain Dance.",
		//shortDesc = "For 5 turns, heavy rain powers Water moves.",
		id = Moves.RAINDANCE,
		//name = "Rain Dance",
		pp = 5,
		priority = 0,
		flags = new Flags (),
		weather = "RainDance",
		//secondary = false,
		target = Target.All,
		type = Types.WATER,
		zMoveBoost = new Boosts() {spe = 1},
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 229,
		accuracy = 100,
		basePower = 20,
		category = Category.PHYSICAL,
		//desc = "If this move is successful and the user has not fainted, the effects of Leech Seed and partial-trapping moves END for the user, and all hazards are removed from the user's side of the field.",
		//shortDesc = "Frees user from hazards/partial trap/Leech Seed.",
		id = Moves.RAPIDSPIN,
		isViable = true,
		//name = "Rapid Spin",
		pp = 40,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		self = {
			onHit = function (pokemon) {
				if (pokemon.hp && pokemon.removeVolatile("leechseed")) {
					this.add('-end', pokemon, 'Leech Seed', '[from] move = Rapid Spin', '[of] ' + pokemon);
				}
				let sideConditions = ["spikes", "toxicspikes", "stealthrock", "stickyweb"];
				for (const condition of sideConditions) {
					if (pokemon.hp && pokemon.side.removeSiDECONDITION(condition)) {
						this.add('-sideend', pokemon.side, this.getEffect(condition).name, '[from] move = Rapid Spin', '[of] ' + pokemon);
					}
				}
				if (pokemon.hp && pokemon.volatiles["partiallytrapped"]) {
					pokemon.removeVolatile("partiallytrapped");
				}
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 100,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 75,
		accuracy = 95,
		basePower = 55,
		category = Category.PHYSICAL,
		//desc = "Has a higher chance for a critical hit.",
		//shortDesc = "High critical hit ratio. Hits adjacent foes.",
		id = Moves.RAZORLEAF,
		//name = "Razor Leaf",
		pp = 25,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		critRatio = 2,
		//secondary = false,
		target = Target.AllAdjacentFoes,
		type = Types.GRASS,
		zMovePower = 100,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 534,
		accuracy = 95,
		basePower = 75,
		category = Category.PHYSICAL,
		//desc = "Has a 50% chance to lower the target'S Defense by 1 stage.",
		//shortDesc = "50% chance to lower the target'S Defense by 1.",
		id = Moves.RAZORSHELL,
		isViable = true,
		//name = "Razor Shell",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 50,
			boosts = new Boosts() {
				def = -1
			}
		},
		target = Target.Normal,
		type = Types.WATER,
		zMovePower = 140,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 13,
		accuracy = 100,
		basePower = 80,
		category = Category.SPECIAL,
		//desc = "Has a higher chance for a critical hit. This attack charges on the first turn and executes on the second. If the user is holding a Power Herb, the move completes in one turn.",
		//shortDesc = "Charges, then hits foe(s) turn 2. High crit ratio.",
		id = Moves.RAZORWIND,
		//name = "Razor Wind",
		pp = 10,
		priority = 0,
		flags = new Flags (charge: true, protect: true, mirror: true),
		onTry = function (attacker, defender, move) {
			if (attacker.volatiles["twoturnmove"]) {
				if (attacker.volatiles["twoturnmove"].duration === 2) return null;
				attacker.removeVolatile(move.id);
				return;
			}
			this.add('-prepare', attacker, move.name, defender);
			if (!this.runEvent("ChargeMove", attacker, defender, move)) {
				this.add('-anim', attacker, move.name, defender);
				if (move.spreadHit) {
					attacker.addVolatile("twoturnmove", defender);
					attacker.volatiles["twoturnmove"].duration = 1;
				}
				return;
			}
			attacker.addVolatile("twoturnmove", defender);
			return null;
		},
		critRatio = 2,
		//secondary = false,
		target = Target.AllAdjacentFoes,
		type = Types.NORMAL,
		zMovePower = 160,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 105,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user restores 1/2 of its maximum HP, rounded half up.",
		//shortDesc = "Heals the user by 50% of its max HP.",
		id = Moves.RECOVER,
		isViable = true,
		//name = "Recover",
		pp = 10,
		priority = 0,
		flags = new Flags (snatch: true, heal: true),
		heal = new int[] {1, 2},
		//secondary = false,
		target = Target.Self,
		type = Types.NORMAL,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 278,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user regains the item it last used, if it is not holding an item. Fails if the user was not holding an item, if the item was a popped Air Balloon, if the item was picked up by a Pokemon with the Ability Pickup, or if the item was lost to Bug Bite, Covet, Incinerate, Knock Off, Pluck, or Thief. Items thrown with Fling can be regained.",
		//shortDesc = "Restores the item the user last used.",
		id = Moves.RECYCLE,
		//name = "Recycle",
		pp = 10,
		priority = 0,
		flags = new Flags (snatch: true),
		onHit = function (pokemon) {
			if (pokemon.item || !pokemon.lastItem) return false;
			pokemon.setItem(pokemon.lastItem);
			pokemon.lastItem = "";
			this.add('-item', pokemon, pokemon.getItem(), '[from] move = Recycle');
		},
		//secondary = false,
		target = Target.Self,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {spe = 2},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 115,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "For 5 turns, the user and its party members take 0.5x damage from physical attacks, or 0.66x damage if in a Double Battle; does not reduce damage further with Aurora Veil. Critical hits ignore this protection. It is removed from the user's side if the user or an ally is successfully hit by Brick Break, Psychic Fangs, or Defog. Brick Break and Psychic Fangs remove the effect before damage is calculated. Lasts for 8 turns if the user is holding Light Clay.",
		//shortDesc = "For 5 turns, physical damage to allies is halved.",
		id = Moves.REFLECT,
		isViable = true,
		//name = "Reflect",
		pp = 20,
		priority = 0,
		flags = new Flags (snatch: true),
		sideCondition = "reflect",
		effect = {
			duration = 5,
			durationCallback = function (target, source, effect) {
				if (source && source.hasItem("lightclay")) {
					return 8;
				}
				return 5;
			},
			onAnyModifyDamage = function (damage, source, target, move) {
				if (target !== source && target.Side === this.effectData.target && this.getCategory(move) === "Physical") {
					if (!move.crit && !move.infiltrates) {
						this.debug('Reflect weaken');
						if (target.Side.active.length > 1) return this.chainModify([0xAAC, 0x1000]);
						return this.chainModify(0.5);
					}
				}
			},
			onStart = function (side) {
				this.add('-sidestart', side, "Reflect");
			},
			onResidualOrder = 21,
			onEnd = function (side) {
				this.add('-sideend', side, "Reflect");
			}
		},
		//secondary = false,
		target = Target.AllySide,
		type = Types.PSYCHIC,
		zMoveBoost = new Boosts() {def = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 513,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Causes the user's types to become the same as the current types of the target. If the target'S current types include typeless and a non-added type, typeless is ignored. If the target'S current types include typeless and an added type from Forest's Curse or Trick-or-Treat, typeless is copied as the Normal type instead. Fails if the user is an Arceus or a Silvally, or if the target'S current type is typeless alone.",
		//shortDesc = "User becomes the same type as the target.",
		id = Moves.REFLECTTYPE,
		//name = "Reflect Type",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, authentic: true, mystery: true),
		onHit = function (target, source) {
			if (source.template && (source.template.num === 493 || source.template.num === 773)) return false;
			this.add('-start', source, "typechange", '[from] move = Reflect Type', '[of] ' + target);
			let newBaseTypes = target.GetTypes(true).filter(type => type !== '???');
			if (!newBaseTypes.LENGTH) {
				if (target.AddedType) {
					newBaseTypes = ["Normal"];
				} else {
					return false;
				}
			}
			source.setType(newBaseTypes);
			source.addedType = target.AddedType;
			source.knownType = target.Side === source.side && target.KnownType;
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {spa = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 287,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user cures its burn, poison, or paralysis.",
		//shortDesc = "User cures its burn, poison, or paralysis.",
		id = Moves.REFRESH,
		isViable = true,
		//name = "Refresh",
		pp = 20,
		priority = 0,
		flags = new Flags (snatch: true),
		onHit = function (pokemon) {
			if (["", "slp", "frz"].includes(pokemon.status)) return false;
			pokemon.cureStatus();
		},
		//secondary = false,
		target = Target.Self,
		type = Types.NORMAL,
		zMoveEffect = "heal",
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 547,
		accuracy = 100,
		basePower = 75,
		category = Category.SPECIAL,
		//desc = "Has a 10% chance to cause the target To fall asleep. If this move is successful on at least one target And the user is a Meloetta, it changes to Pirouette Forme if it is currently in Aria Forme, or changes to Aria Forme if it is currently in Pirouette Forme. This forme change does not happen if the Meloetta has the Ability Sheer Force. The Pirouette Forme reverts to Aria Forme when Meloetta is not active.",
		//shortDesc = "10% chance to sleep foe(s). Meloetta transforms.",
		id = Moves.RELICSONG,
		isViable = true,
		//name = "Relic Song",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, sound: true, authentic: true),
		secondary = new Secondary() {
			chance = 10,
			status = "slp"
		},
		onHit = function (target, pokemon, move) {
			if (pokemon.baseTemplate.baseSpecies === "Meloetta" && !pokemon.transformed) {
				move.willChangeForme = true;
			}
		},
		onAfterMoveSeCONDARYSELF = function (pokemon, target, move) {
			if (move.willChangeForme) {
				pokemon.formeChange(pokemon.template.speciesid === "meloettapirouette" ? "Meloetta"  = 'Meloetta-Pirouette', this.effect, false, '[msg]');
			}
		},
		target = Target.AllAdjacentFoes,
		type = Types.NORMAL,
		zMovePower = 140,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 156,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user falls asleep for the next two turns and restores all of its HP, curing itself of any major status condition in the process. Fails if the user has full HP, is already asleep, or if another effect is preventing sleep.",
		//shortDesc = "User sleeps 2 turns and restores HP and status.",
		id = Moves.REST,
		isViable = true,
		//name = "Rest",
		pp = 10,
		priority = 0,
		flags = new Flags (snatch: true, heal: true),
		onTryMove = function (pokemon) {
			if (pokemon.hp < pokemon.maxhp && pokemon.status !== "slp" && !pokemon.hasAbility("comatose")) return;
			this.add('-fail', pokemon);
			return null;
		},
		onHit = function (target) {
			if (!target.SetStatus("slp")) return false;
			target.StatusData.time = 3;
			target.StatusData.startTime = 3;
			this.heal(target.Maxhp); //Aeshetic only as the healing happens after you fall asleep in-game
			this.add('-status', target, "slp", '[from] move = Rest');
		},
		//secondary = false,
		target = Target.Self,
		type = Types.PSYCHIC,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 514,
		accuracy = 100,
		basePower = 70,
		category = Category.PHYSICAL,
		//desc = "Power doubles if one of the user's party members fainted last turn.",
		//shortDesc = "Power doubles if an ally fainted last turn.",
		id = Moves.RETALIATE,
		//name = "Retaliate",
		pp = 5,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		onBasePowerPriority = 4,
		onBasePower = function (basePower, pokemon) {
			if (pokemon.side.faintedLastTurn) {
				this.debug('Boosted for a faint last turn');
				return this.chainModify(2);
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 140,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 216,
		accuracy = 100,
		basePower = 0,
		basePowerCallback = function (pokemon) {
			return Math.floor((pokemon.happiness * 10) / 25) || 1;
		},
		category = Category.PHYSICAL,
		//desc = "Power is equal to the greater of (user's Happiness * 2/5), rounded down, or 1.",
		//shortDesc = "Max 102 power at maximum Happiness.",
		id = Moves.RETURN,
		isViable = true,
		//name = "Return",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 160,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 686,
		accuracy = 100,
		basePower = 90,
		category = Category.SPECIAL,
		//desc = "This move's type depends on the user's primary type.",
		//shortDesc = "Type varies based on the user's primary type.",
		id = Moves.REVELATIONDANCE,
		isViable = true,
		//name = "Revelation Dance",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, dance: true),
		onModifyMove = function (move, pokemon) {
			let type = pokemon.types[0];
			if (type === "Bird") type = "???";
			move.type = type;
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 175,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 279,
		accuracy = 100,
		basePower = 60,
		basePowerCallback = function (pokemon, target, move) {
			if (target.LastDamage > 0 && pokemon.lastAttackedBy && pokemon.lastAttackedBy.thisTurn && pokemon.lastAttackedBy.pokemon === target) {
				this.debug('Boosted for getting hit by ' + pokemon.lastAttackedBy.move);
				return move.basePower * 2;
			}
			return move.basePower;
		},
		category = Category.PHYSICAL,
		//desc = "Power doubles if the user was hit by the target This turn.",
		//shortDesc = "Power doubles if user is damaged by the target.",
		id = Moves.REVENGE,
		//name = "Revenge",
		pp = 10,
		priority = -4,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.FIGHTING,
		zMovePower = 120,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 179,
		accuracy = 100,
		basePower = 0,
		basePowerCallback = function (pokemon, target) {
			let ratio = pokemon.hp * 48 / pokemon.maxhp;
			if (ratio < 2) {
				return 200;
			}
			if (ratio < 5) {
				return 150;
			}
			if (ratio < 10) {
				return 100;
			}
			if (ratio < 17) {
				return 80;
			}
			if (ratio < 33) {
				return 40;
			}
			return 20;
		},
		category = Category.PHYSICAL,
		//desc = "Deals damage to the target Based on the amount of HP the user has left. X is equal to (user's current HP * 48 / user's maximum HP), rounded down; the base power of this attack is 20 if X is 33 to 48, 40 if X is 17 to 32, 80 if X is 10 to 16, 100 if X is 5 to 9, 150 if X is 2 to 4, and 200 if X is 0 or 1.",
		//shortDesc = "More power the less HP the user has left.",
		id = Moves.REVERSAL,
		//name = "Reversal",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.FIGHTING,
		zMovePower = 160,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 46,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The target Is forced to switch out and be replaced with a random unfainted ally. Fails if the target Used Ingrain previously or has the Ability Suction Cups.",
		//shortDesc = "Forces the target To switch to a random ally.",
		id = Moves.ROAR,
		isViable = true,
		//name = "Roar",
		pp = 20,
		priority = -6,
		flags = new Flags (reflectable: true, mirror: true, sound: true, authentic: true, mystery: true),
		forceSwitch = true,
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {def = 1},
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 459,
		accuracy = 90,
		basePower = 150,
		category = Category.SPECIAL,
		//desc = "If this move is successful, the user must recharge on the following turn and cannot make a move.",
		//shortDesc = "User cannot move next turn.",
		id = Moves.ROAROFTIME,
		//name = "Roar of Time",
		pp = 5,
		priority = 0,
		flags = new Flags (recharge: true, protect: true, mirror: true),
		self = {
			volatileStatus = "mustrecharge"
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.DRAGON,
		zMovePower = 200,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 350,
		accuracy = 90,
		basePower = 25,
		category = Category.PHYSICAL,
		//desc = "Hits two to five times. Has a 1/3 chance to hit two or three times, and a 1/6 chance to hit four or five times. If one of the hits breaks the target'S substitute, it will take damage for the remaining hits. If the user has the Ability Skill Link, this move will always hit five times.",
		//shortDesc = "Hits 2-5 times in one turn.",
		id = Moves.ROCKBLAST,
		isViable = true,
		//name = "Rock Blast",
		pp = 10,
		priority = 0,
		flags = new Flags (bullet: true, protect: true, mirror: true),
		multihit = new int[] {2, 5},
		//secondary = false,
		target = Target.Normal,
		type = Types.ROCK,
		zMovePower = 140,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 431,
		accuracy = 85,
		basePower = 90,
		category = Category.PHYSICAL,
		//desc = "Has a 20% chance to confuse the target.",
		//shortDesc = "20% chance to confuse the target.",
		id = Moves.ROCKCLIMB,
		//name = "Rock Climb",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 20,
			volatileStatus = "confusion"
		},
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 175,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 397,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the user's Speed by 2 stages.",
		//shortDesc = "Raises the user's Speed by 2.",
		id = Moves.ROCKPOLISH,
		isViable = true,
		//name = "Rock Polish",
		pp = 20,
		priority = 0,
		flags = new Flags (snatch: true),
		boosts = new Boosts() {
			spe = 2
		},
		//secondary = false,
		target = Target.Self,
		type = Types.ROCK,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 157,
		accuracy = 90,
		basePower = 75,
		category = Category.PHYSICAL,
		//desc = "Has a 30% chance to flinch the target.",
		//shortDesc = "30% chance to flinch the foe(s).",
		id = Moves.ROCKSLIDE,
		isViable = true,
		//name = "Rock Slide",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 30,
			volatileStatus = "flinch"
		},
		target = Target.AllAdjacentFoes,
		type = Types.ROCK,
		zMovePower = 140,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 249,
		accuracy = 100,
		basePower = 40,
		category = Category.PHYSICAL,
		//desc = "Has a 50% chance to lower the target'S Defense by 1 stage.",
		//shortDesc = "50% chance to lower the target'S Defense by 1.",
		id = Moves.ROCKSMASH,
		//name = "Rock Smash",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 50,
			boosts = new Boosts() {
				def = -1
			}
		},
		target = Target.Normal,
		type = Types.FIGHTING,
		zMovePower = 100,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 88,
		accuracy = 90,
		basePower = 50,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect.",
		id = Moves.ROCKTHROW,
		//name = "Rock Throw",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.ROCK,
		zMovePower = 100,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 317,
		accuracy = 95,
		basePower = 60,
		category = Category.PHYSICAL,
		//desc = "Has a 100% chance to lower the target'S Speed by 1 stage.",
		//shortDesc = "100% chance to lower the target'S Speed by 1.",
		id = Moves.ROCKTOMB,
		//name = "Rock Tomb",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 100,
			boosts = new Boosts() {
				spe = -1
			}
		},
		target = Target.Normal,
		type = Types.ROCK,
		zMovePower = 120,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 439,
		accuracy = 90,
		basePower = 150,
		category = Category.PHYSICAL,
		//desc = "If this move is successful, the user must recharge on the following turn and cannot make a move.",
		//shortDesc = "User cannot move next turn.",
		id = Moves.ROCKWRECKER,
		//name = "Rock Wrecker",
		pp = 5,
		priority = 0,
		flags = new Flags (bullet: true, recharge: true, protect: true, mirror: true),
		self = {
			volatileStatus = "mustrecharge"
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.ROCK,
		zMovePower = 200,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 272,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user's Ability changes to match the target'S Ability. Fails if the user's Ability is Multitype, Stance Change, or already matches the target, or if the target'S Ability is Flower Gift, Forecast, Illusion, Imposter, Multitype, Stance Change, Trace, Wonder Guard, or Zen Mode.",
		//shortDesc = "User replaces its Ability with the target'S.",
		id = Moves.ROLEPLAY,
		//name = "Role Play",
		pp = 10,
		priority = 0,
		flags = new Flags (authentic: true, mystery: true),
		onTryHit = function (target, source) {
			let bannedTargetABilities = ["battlebond", "comatose", "disguise", "flowergift", "forecast", "illusion", "imposter", "multitype", "powerconstruct", "powerofalchemy", "receiver", "rkssystem", "schooling", "shieldsdown", "stancechange", "trace", "wonderguard", "zenmode"];
			let bannedSourceAbilities = ["battlebond", "comatose", "disguise", "multitype", "powerconstruct", "rkssystem", "schooling", "shieldsdown", "stancechange"];
			if (bannedTargetABilities.includes(target.Ability) || bannedSourceAbilities.includes(source.ability) || target.Ability === source.ability) {
				return false;
			}
		},
		onHit = function (target, source) {
			let oldAbility = source.setAbility(target.Ability);
			if (oldAbility) {
				this.add('-ability', source, this.getAbility(source.ability).name, '[from] move = Role Play', '[of] ' + target);
				return;
			}
			return false;
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.PSYCHIC,
		zMoveBoost = new Boosts() {spe = 1},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 27,
		accuracy = 85,
		basePower = 60,
		category = Category.PHYSICAL,
		//desc = "Has a 30% chance to flinch the target.",
		//shortDesc = "30% chance to flinch the target.",
		id = Moves.ROLLINGKICK,
		//name = "Rolling Kick",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 30,
			volatileStatus = "flinch"
		},
		target = Target.Normal,
		type = Types.FIGHTING,
		zMovePower = 120,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 205,
		accuracy = 90,
		basePower = 30,
		basePowerCallback = function (pokemon, target, move) {
			let bp = move.basePower;
			if (pokemon.volatiles.rollout && pokemon.volatiles.rollout.hitCount) {
				bp *= Math.pow(2, pokemon.volatiles.rollout.hitCount);
			}
			if (pokemon.status !== "slp") pokemon.addVolatile("rollout");
			if (pokemon.volatiles.defensecurl) {
				bp *= 2;
			}
			this.debug("Rollout bp = " + bp);
			return bp;
		},
		category = Category.PHYSICAL,
		//desc = "If this move is successful, the user is locked into this move and cannot make another move until it misses, 5 turns have passed, or the attack cannot be used. Power doubles with each successful hit of this move and doubles again if Defense Curl was used previously by the user. If this move is called by Sleep Talk, the move is used for one turn.",
		//shortDesc = "Power doubles with each hit. Repeats for 5 turns.",
		id = Moves.ROLLOUT,
		//name = "Rollout",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		effect = {
			duration = 2,
			onLockMove = "rollout",
			onStart = function () {
				this.effectData.hitCount = 1;
			},
			onRestart = function () {
				this.effectData.hitCount++;
				if (this.effectData.hitCount < 5) {
					this.effectData.duration = 2;
				}
			},
			onResidual = function (target) {
				if (target.LastMove && target.LastMove.id === "struggle") {
					// don't lock
					delete target.Volatiles["rollout"];
				}
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.ROCK,
		zMovePower = 100,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 355,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user restores 1/2 of its maximum HP, rounded half up. Until the end of the turn, Flying-type users lose their Flying type and pure Flying-type users become Normal type. Does nothing if the user's HP is full.",
		//shortDesc = "Heals 50% HP. Flying-type removed 'til turn ends.",
		id = Moves.ROOST,
		isViable = true,
		//name = "Roost",
		pp = 10,
		priority = 0,
		flags = new Flags (snatch: true, heal: true),
		heal = new int[] {1, 2},
		self = {
			volatileStatus = "roost"
		},
		effect = {
			duration = 1,
			onResidualOrder = 20,
			onTypePriority = -1,
			onType = function (types, pokemon) {
				this.effectData.typeWas = types;
				return types.FILTER(type => type !== "Flying");
			}
		},
		//secondary = false,
		target = Target.Self,
		type = Types.FLYING,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 563,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the Attack and Special Attack of all grounded Grass-type Pokemon on the field by 1 stage.",
		//shortDesc = "Raises Atk, Sp. Atk of grounded Grass types by 1.",
		id = Moves.ROTOTILLER,
		//name = "Rototiller",
		pp = 10,
		priority = 0,
		flags = new Flags (distance: true, nonsky: true),
		onHitField = function (target, source) {
			let targets = [];
			let anyAirborne = false;
			for (const side of this.sides) {
				for (const pokemon of side.active) {
					if (!pokemon || !pokemon.isActive) continue;
					if (!pokemon.runImmunity("Ground")) {
						this.add('-immune', pokemon, '[msg]');
						anyAirborne = true;
						continue;
					}
					if (pokemon.hasType("Grass")) {
						// This move affects every grounded Grass-type Pokemon in play.
						targets.push(pokemon);
					}
				}
			}
			if (!targets.length && !anyAirborne) return false; // Fails when there are no grounded Grass types or airborne Pokemon
			for (const pokemon of targets) {
				this.boost({atk = 1, spa = 1}, pokemon, source);
			}
		},
		//secondary = false,
		target = Target.All,
		type = Types.GROUND,
		zMoveBoost = new Boosts() {atk = 1},
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 496,
		accuracy = 100,
		basePower = 60,
		basePowerCallback = function (target, source, move) {
			if (move.sourceEffect === "round") {
				return move.basePower * 2;
			}
			return move.basePower;
		},
		category = Category.SPECIAL,
		//desc = "If there are other active Pokemon that chose this move for use this turn, those Pokemon take their turn immediately after the user, in Speed order, and this move's power is 120 for each other user.",
		//shortDesc = "Power doubles if others used Round this turn.",
		id = Moves.ROUND,
		//name = "Round",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, sound: true, authentic: true),
		onTry = function () {
			for (const action of this.queue) {
				// @ts-ignore
				if (!action.pokemon || !action.move) continue;
				// @ts-ignore
				if (action.move.id === "round") {
					// @ts-ignore
					this.prioritizeAction(action);
					return;
				}
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 120,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 221,
		accuracy = 95,
		basePower = 100,
		category = Category.PHYSICAL,
		//desc = "Has a 50% chance to burn the target.",
		//shortDesc = "50% chance to burn the target. Thaws user.",
		id = Moves.SACREDFIRE,
		isViable = true,
		//name = "Sacred Fire",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, defrost: true),
		secondary = new Secondary() {
			chance = 50,
			status = "brn"
		},
		target = Target.Normal,
		type = Types.FIRE,
		zMovePower = 180,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 533,
		accuracy = 100,
		basePower = 90,
		category = Category.PHYSICAL,
		//desc = "Ignores the target'S stat stage changes, including evasiveness.",
		//shortDesc = "Ignores the target'S stat stage changes.",
		id = Moves.SACREDSWORD,
		isViable = true,
		//name = "Sacred Sword",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		ignoreEvasion = true,
		ignoreDefensive = true,
		//secondary = false,
		target = Target.Normal,
		type = Types.FIGHTING,
		zMovePower = 175,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 219,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "For 5 turns, the user and its party members cannot have major status conditions or confusion inflicted on them by other Pokemon. It is removed from the user's side if the user or an ally is successfully hit by Defog.",
		//shortDesc = "For 5 turns, protects user's party from status.",
		id = Moves.SAFEGUARD,
		//name = "Safeguard",
		pp = 25,
		priority = 0,
		flags = new Flags (snatch: true),
		sideCondition = "safeguard",
		effect = {
			duration = 5,
			durationCallback = function (target, source, effect) {
				if (source && source.hasAbility("persistent")) {
					this.add('-activate', source, 'ability = Persistent', effect);
					return 7;
				}
				return 5;
			},
			onSetStatus = function (status, target, source, effect) {
				if (source && target !== source && effect && (!effect.infiltrates || target.Side === source.side)) {
					this.debug('interrupting setStatus');
					if (effect.id === "synchronize" || (effect.effectType === "Move" && !effect.secondaries)) {
						this.add('-activate', target, 'move = Safeguard');
					}
					return null;
				}
			},
			onTryAddVolatile = function (status, target, source, effect) {
				if ((status.id === "confusion" || status.id === "yawn") && source && target !== source && effect && (!effect.infiltrates || target.Side === source.side)) {
					if (effect.effectType === "Move" && !effect.secondaries) this.add('-activate', target, 'move = Safeguard');
					return null;
				}
			},
			onStart = function (side) {
				this.add('-sidestart', side, "Safeguard");
			},
			onResidualOrder = 21,
			onResidualSubOrder = 2,
			onEnd = function (side) {
				this.add('-sideend', side, "Safeguard");
			}
		},
		//secondary = false,
		target = Target.AllySide,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {spe = 1},
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 28,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Lowers the target'S accuracy by 1 stage.",
		//shortDesc = "Lowers the target'S accuracy by 1.",
		id = Moves.SANDATTACK,
		//name = "Sand Attack",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true),
		boosts = new Boosts() {
			accuracy = -1
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.GROUND,
		zMoveBoost = new Boosts() {evasion = 1},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 328,
		accuracy = 85,
		basePower = 35,
		category = Category.PHYSICAL,
		//desc = "Prevents the target From switching for four or five turns; seven turns if the user is holding Grip Claw. Causes damage to the target Equal to 1/8 of its maximum HP (1/6 if the user is holding Binding Band), rounded down, at the end of each turn during effect. The target Can still switch out if it is holding Shed Shell or uses Baton Pass, Parting Shot, U-turn, or Volt Switch. The effect ends if either the user or the target Leaves the field, or if the target Uses Rapid Spin or Substitute. This effect is not stackable or reset by using this or another partial-trapping move.",
		//shortDesc = "Traps and damages the target For 4-5 turns.",
		id = Moves.SANDTOMB,
		//name = "Sand Tomb",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		volatileStatus = "partiallytrapped",
		//secondary = false,
		target = Target.Normal,
		type = Types.GROUND,
		zMovePower = 100,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 201,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "For 5 turns, the weather becomes Sandstorm. At the end of each turn except the last, all active Pokemon lose 1/16 of their maximum HP, rounded down, unless they are a Ground, Rock, or Steel type, or have the Abilities Magic Guard, Overcoat, Sand Force, Sand Rush, or Sand Veil. The Special Defense of Rock-type Pokemon is multiplied by 1.5 when taking damage from a special attack during the effect. Lasts for 8 turns if the user is holding Smooth Rock. Fails if the current weather is Sandstorm.",
		//shortDesc = "For 5 turns, a sandstorm rages.",
		id = Moves.SANDSTORM,
		//name = "Sandstorm",
		pp = 10,
		priority = 0,
		flags = new Flags (),
		weather = "Sandstorm",
		//secondary = false,
		target = Target.All,
		type = Types.ROCK,
		zMoveBoost = new Boosts() {spe = 1},
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 634,
		//accuracy = true,
		basePower = 1,
		category = Category.PHYSICAL,
		//shortDesc = "Power is equal to the base move's Z-Power.",
		id = Moves.SAVAGESPINOUT,
		//name = "Savage Spin-Out",
		pp = 1,
		priority = 0,
		flags = new Flags (),
		isZ = "buginiumz",
		//secondary = false,
		target = Target.Normal,
		type = Types.BUG,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 503,
		accuracy = 100,
		basePower = 80,
		category = Category.SPECIAL,
		//desc = "Has a 30% chance to burn the target. The target Thaws out if it is frozen.",
		//shortDesc = "30% chance to burn the target. Thaws target.",
		id = Moves.SCALD,
		isViable = true,
		//name = "Scald",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, defrost: true),
		thawsTarget = true,
		secondary = new Secondary() {
			chance = 30,
			status = "brn"
		},
		target = Target.Normal,
		type = Types.WATER,
		zMovePower = 160,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 184,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Lowers the target'S Speed by 2 stages.",
		//shortDesc = "Lowers the target'S Speed by 2.",
		id = Moves.SCARYFACE,
		//name = "Scary Face",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true, mystery: true),
		boosts = new Boosts() {
			spe = -2
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {spe = 1},
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 10,
		accuracy = 100,
		basePower = 40,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect.",
		id = Moves.SCRATCH,
		//name = "Scratch",
		pp = 35,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 100,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 103,
		accuracy = 85,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Lowers the target'S Defense by 2 stages.",
		//shortDesc = "Lowers the target'S Defense by 2.",
		id = Moves.SCREECH,
		//name = "Screech",
		pp = 40,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true, sound: true, authentic: true, mystery: true),
		boosts = new Boosts() {
			def = -2
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {atk = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 545,
		accuracy = 100,
		basePower = 100,
		category = Category.SPECIAL,
		//desc = "Has a 30% chance to burn the target.",
		//shortDesc = "30% chance to burn adjacent Pokemon.",
		id = Moves.SEARINGSHOT,
		isViable = true,
		//name = "Searing Shot",
		pp = 5,
		priority = 0,
		flags = new Flags (bullet: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 30,
			status = "brn"
		},
		target = Target.AllAdjacent,
		type = Types.FIRE,
		zMovePower = 180,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 724,
		//accuracy = true,
		basePower = 200,
		category = Category.PHYSICAL,
		//desc = "This move and its effects ignore the Abilities of other Pokemon.",
		//shortDesc = "Ignores the Abilities of other Pokemon.",
		id = Moves.SEARINGSUNRAZESMASH,
		//name = "Searing Sunraze Smash",
		pp = 1,
		priority = 0,
		flags = new Flags (contact: true),
		isZ = "solganiumz",
		ignoreAbility = true,
		//secondary = false,
		target = Target.Normal,
		type = Types.STEEL,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 290,
		accuracy = 100,
		basePower = 70,
		category = Category.PHYSICAL,
		//desc = "Has a 30% chance to cause a secondary effect on the target Based on the battle terrain. Causes paralysis on the regular Wi-Fi terrain, causes paralysis during Electric Terrain, lowers Special Attack by 1 stage during Misty Terrain, causes sleep during Grassy Terrain and lowers Speed by 1 stage during Psychic Terrain.",
		//shortDesc = "Effect varies with terrain. (30% paralysis chance)",
		id = Moves.SECRETPOWER,
		//name = "Secret Power",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		onModifyMove = function (move, pokemon) {
			if (this.isTerrain("")) return;
			move.secondaries = [];
			if (this.isTerrain("electricterrain")) {
				move.secondaries.push({
					chance = 30,
					status = "par"
				});
			} else if (this.isTerrain("grassyterrain")) {
				move.secondaries.push({
					chance = 30,
					status = "slp"
				});
			} else if (this.isTerrain("mistyterrain")) {
				move.secondaries.push({
					chance = 30,
					boosts = new Boosts() {
						spa = -1
					}
				});
			} else if (this.isTerrain("psychicterrain")) {
				move.secondaries.push({
					chance = 30,
					boosts = new Boosts() {
						spe = -1
					}
				});
			}
		},
		secondary = new Secondary() {
			chance = 30,
			status = "par"
		},
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 140,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 548,
		accuracy = 100,
		basePower = 85,
		category = Category.SPECIAL,
		defensivecategory = Category.PHYSICAL,
		//desc = "Deals damage to the target Based on its Defense instead of Special Defense.",
		//shortDesc = "Damages target Based on Defense, not Sp. Def.",
		id = Moves.SECRETSWORD,
		isViable = true,
		//name = "Secret Sword",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.FIGHTING,
		zMovePower = 160,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 402,
		accuracy = 100,
		basePower = 80,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect.",
		id = Moves.SEEDBOMB,
		isViable = true,
		//name = "Seed Bomb",
		pp = 15,
		priority = 0,
		flags = new Flags (bullet: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.GRASS,
		zMovePower = 160,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 465,
		accuracy = 85,
		basePower = 120,
		category = Category.SPECIAL,
		//desc = "Has a 40% chance to lower the target'S Special Defense by 2 stages.",
		//shortDesc = "40% chance to lower the target'S Sp. Def by 2.",
		id = Moves.SEEDFLARE,
		isViable = true,
		//name = "Seed Flare",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 40,
			boosts = new Boosts() {
				spd = -2
			}
		},
		target = Target.Normal,
		type = Types.GRASS,
		zMovePower = 190,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 69,
		accuracy = 100,
		basePower = 0,
		damage = "level",
		category = Category.PHYSICAL,
		//desc = "Deals damage to the target Equal to the user's level.",
		//shortDesc = "Does damage equal to the user's level.",
		id = Moves.SEISMICTOSS,
		isViable = true,
		//name = "Seismic Toss",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true, nonsky: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.FIGHTING,
		zMovePower = 100,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 120,
		accuracy = 100,
		basePower = 200,
		category = Category.PHYSICAL,
		//desc = "The user faints after using this move, even if this move fails for having no target. This move is prevented from executing if any active Pokemon has the Ability Damp.",
		//shortDesc = "Hits adjacent Pokemon. The user faints.",
		id = Moves.SELFDESTRUCT,
		//name = "Self-Destruct",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		selfdestruct = "always",
		//secondary = false,
		target = Target.AllAdjacent,
		type = Types.NORMAL,
		zMovePower = 200,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 247,
		accuracy = 100,
		basePower = 80,
		category = Category.SPECIAL,
		//desc = "Has a 20% chance to lower the target'S Special Defense by 1 stage.",
		//shortDesc = "20% chance to lower the target'S Sp. Def by 1.",
		id = Moves.SHADOWBALL,
		isViable = true,
		//name = "Shadow Ball",
		pp = 15,
		priority = 0,
		flags = new Flags (bullet: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 20,
			boosts = new Boosts() {
				spd = -1
			}
		},
		target = Target.Normal,
		type = Types.GHOST,
		zMovePower = 160,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 708,
		accuracy = 100,
		basePower = 85,
		category = Category.PHYSICAL,
		//desc = "Has a 20% chance to lower the target'S Defense by 1 stage.",
		//shortDesc = "20% chance to lower the target'S Defense by 1.",
		id = Moves.SHADOWBONE,
		isViable = true,
		//name = "Shadow Bone",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 20,
			boosts = new Boosts() {
				def = -1
			}
		},
		target = Target.Normal,
		type = Types.GHOST,
		zMovePower = 160,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 421,
		accuracy = 100,
		basePower = 70,
		category = Category.PHYSICAL,
		//desc = "Has a higher chance for a critical hit.",
		//shortDesc = "High critical hit ratio.",
		id = Moves.SHADOWCLAW,
		isViable = true,
		//name = "Shadow Claw",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		critRatio = 2,
		//secondary = false,
		target = Target.Normal,
		type = Types.GHOST,
		zMovePower = 140,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 467,
		accuracy = 100,
		basePower = 120,
		category = Category.PHYSICAL,
		//desc = "If this move is successful, it breaks through the target'S Baneful Bunker, Detect, King's Shield, Protect, or Spiky Shield for this turn, allowing other Pokemon to attack the target Normally. If the target'S side is protected by Crafty Shield, Mat Block, Quick Guard, or Wide Guard, that protection is also broken for this turn and other Pokemon may attack the target'S side normally. This attack charges on the first turn and executes on the second. On the first turn, the user avoids all attacks. If the user is holding a Power Herb, the move completes in one turn. Damage doubles and no accuracy check is done if the target Has used Minimize while active.",
		//shortDesc = "Disappears turn 1. Hits turn 2. Breaks protection.",
		id = Moves.SHADOWFORCE,
		isViable = true,
		//name = "Shadow Force",
		pp = 5,
		priority = 0,
		flags = new Flags (contact: true, charge: true, mirror: true),
		breaksProtect = true,
		onTry = function (attacker, defender, move) {
			if (attacker.removeVolatile(move.id)) {
				return;
			}
			this.add('-prepare', attacker, move.name, defender);
			if (!this.runEvent("ChargeMove", attacker, defender, move)) {
				this.add('-anim', attacker, move.name, defender);
				return;
			}
			attacker.addVolatile("twoturnmove", defender);
			return null;
		},
		effect = {
			duration = 2,
			onTryImmunity = function (target, source, move) {
				if (move.id === "helpinghand") {
					return;
				}
				if (source.hasAbility("noguard") || target.HasAbility("noguard")) {
					return;
				}
				if (source.volatiles["lockon"] && target === source.volatiles["lockon"].source) return;
				return false;
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.GHOST,
		zMovePower = 190,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 325,
		//accuracy = true,
		basePower = 60,
		category = Category.PHYSICAL,
		//desc = "This move does not check accuracy.",
		//shortDesc = "This move does not check accuracy.",
		id = Moves.SHADOWPUNCH,
		isViable = true,
		//name = "Shadow Punch",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true, punch: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.GHOST,
		zMovePower = 120,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 425,
		accuracy = 100,
		basePower = 40,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "Usually goes first.",
		id = Moves.SHADOWSNEAK,
		isViable = true,
		//name = "Shadow Sneak",
		pp = 30,
		priority = 1,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.GHOST,
		zMovePower = 100,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 159,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the user's Attack by 1 stage.",
		//shortDesc = "Raises the user's Attack by 1.",
		id = Moves.SHARPEN,
		//name = "Sharpen",
		pp = 30,
		priority = 0,
		flags = new Flags (snatch: true),
		boosts = new Boosts() {
			atk = 1
		},
		//secondary = false,
		target = Target.Self,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {atk = 1},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 648,
		//accuracy = true,
		basePower = 1,
		category = Category.PHYSICAL,
		//shortDesc = "Power is equal to the base move's Z-Power.",
		id = Moves.SHATTEREDPSYCHE,
		isViable = true,
		//name = "Shattered Psyche",
		pp = 1,
		priority = 0,
		flags = new Flags (),
		isZ = "psychiumz",
		//secondary = false,
		target = Target.Normal,
		type = Types.PSYCHIC,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 329,
		accuracy = 30,
		basePower = 0,
		category = Category.SPECIAL,
		//desc = "Deals damage to the target Equal to the target'S maximum HP. Ignores accuracy and evasiveness modifiers. This attack's accuracy is equal to (user's level - target'S level + X)%, where X is 30 if the user is an Ice type and 20 otherwise, and fails if the target Is at a higher level. Ice-type Pokemon and Pokemon with the Ability Sturdy are immune.",
		//shortDesc = "OHKOs non-Ice targets. Fails if user's lower level.",
		id = Moves.SHEERCOLD,
		//name = "Sheer Cold",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		ohko = "Ice",
		target = Target.Normal,
		type = Types.ICE,
		zMovePower = 180,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 504,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Lowers the user's Defense and Special Defense by 1 stage. Raises the user's Attack, Special Attack, and Speed by 2 stages.",
		//shortDesc = "Lowers Def, SpD by 1; raises Atk, SpA, Spe by 2.",
		id = Moves.SHELLSMASH,
		isViable = true,
		//name = "Shell Smash",
		pp = 15,
		priority = 0,
		flags = new Flags (snatch: true),
		boosts = new Boosts() {
			def = -1,
			spd = -1,
			atk = 2,
			spa = 2,
			spe = 2
		},
		//secondary = false,
		target = Target.Self,
		type = Types.NORMAL,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 704,
		accuracy = 100,
		basePower = 150,
		category = Category.SPECIAL,
		//desc = "Fails unless the user is hit by a physical attack from an opponent this turn before it can execute the move.",
		//shortDesc = "User must take physical damage before moving.",
		id = Moves.SHELLTRAP,
		//name = "Shell Trap",
		pp = 5,
		priority = -3,
		flags = new Flags (protect: true),
		beforeTurnCallback = function (pokemon) {
			pokemon.addVolatile("shelltrap");
		},
		beforeMoveCallback = function (pokemon) {
			if (pokemon.volatiles["shelltrap"] && !pokemon.volatiles["shelltrap"].gotHit) {
				this.add("cant", pokemon, 'Shell Trap', 'Shell Trap');
				return true;
			}
		},
		effect = {
			duration = 1,
			onStart = function (pokemon) {
				this.add('-singleturn', pokemon, 'move = Shell Trap');
			},
			onHit = function (pokemon, source, move) {
				if (pokemon.side !== source.side && move.category === "Physical") {
					pokemon.volatiles["shelltrap"].gotHit = true;
				}
			}
		},
		//secondary = false,
		target = Target.AllAdjacentFoes,
		type = Types.FIRE,
		zMovePower = 200,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 508,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the user's Speed by 2 stages and its Attack by 1 stage.",
		//shortDesc = "Raises the user's Speed by 2 and Attack by 1.",
		id = Moves.SHIFTGEAR,
		isViable = true,
		//name = "Shift Gear",
		pp = 10,
		priority = 0,
		flags = new Flags (snatch: true),
		boosts = new Boosts() {
			spe = 2,
			atk = 1
		},
		//secondary = false,
		target = Target.Self,
		type = Types.STEEL,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 351,
		//accuracy = true,
		basePower = 60,
		category = Category.SPECIAL,
		//desc = "This move does not check accuracy.",
		//shortDesc = "This move does not check accuracy.",
		id = Moves.SHOCKWAVE,
		//name = "Shock Wave",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.ELECTRIC,
		zMovePower = 120,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 659,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user restores 1/2 of its maximum HP, rounded half down. If the weather is Sandstorm, the user instead restores 2/3 of its maximum HP, rounded half down.",
		//shortDesc = "User restores 1/2 its max HP; 2/3 in Sandstorm.",
		id = Moves.SHOREUP,
		isViable = true,
		//name = "Shore Up",
		pp = 10,
		priority = 0,
		flags = new Flags (snatch: true, heal: true),
		onHit = function (pokemon) {
			if (this.isWeather("sandstorm")) {
				return this.heal(this.modify(pokemon.maxhp, 0.667));
			} else {
				return this.heal(this.modify(pokemon.maxhp, 0.5));
			}
		},
		//secondary = false,
		target = Target.Self,
		type = Types.GROUND,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 324,
		accuracy = 100,
		basePower = 75,
		category = Category.SPECIAL,
		//desc = "Has a 10% chance to confuse the target.",
		//shortDesc = "10% chance to confuse the target.",
		id = Moves.SIGNALBEAM,
		isViable = true,
		//name = "Signal Beam",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 10,
			volatileStatus = "confusion"
		},
		target = Target.Normal,
		type = Types.BUG,
		zMovePower = 140,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 318,
		accuracy = 100,
		basePower = 60,
		category = Category.SPECIAL,
		//desc = "Has a 10% chance to raise the user's Attack, Defense, Special Attack, Special Defense, and Speed by 1 stage.",
		//shortDesc = "10% chance to raise all stats by 1 (not acc/eva).",
		id = Moves.SILVERWIND,
		//name = "Silver Wind",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 10,
			self = {
				boosts = new Boosts() {
					atk = 1,
					def = 1,
					spa = 1,
					spd = 1,
					spe = 1
				}
			}
		},
		target = Target.Normal,
		type = Types.BUG,
		zMovePower = 120,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 493,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Causes the target'S Ability to become Simple. Fails if the target'S Ability is Multitype, Simple, Stance Change, or Truant.",
		//shortDesc = "The target'S Ability becomes Simple.",
		id = Moves.SIMPLEBEAM,
		//name = "Simple Beam",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true, mystery: true),
		onTryHit = function (pokemon) {
			let bannedAbilities = ["battlebond", "comatose", "disguise", "multitype", "powerconstruct", "rkssystem", "schooling", "shieldsdown", "simple", "stancechange", "truant"];
			if (bannedAbilities.includes(pokemon.ability)) {
				return false;
			}
		},
		onHit = function (pokemon) {
			let oldAbility = pokemon.setAbility("simple");
			if (oldAbility) {
				this.add('-ability', pokemon, "Simple", '[from] move = Simple Beam');
				return;
			}
			return false;
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {spa = 1},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 47,
		accuracy = 55,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Causes the target To fall asleep.",
		//shortDesc = "Puts the target To sleep.",
		id = Moves.SING,
		//name = "Sing",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true, sound: true, authentic: true),
		status = "slp",
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {spe = 1},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 695,
		//accuracy = true,
		basePower = 180,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect.",
		id = Moves.SINISTERARROWRAID,
		//name = "Sinister Arrow Raid",
		pp = 1,
		priority = 0,
		flags = new Flags (),
		isZ = "decidiumz",
		//secondary = false,
		target = Target.Normal,
		type = Types.GHOST,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 166,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "This move is permanently replaced by the last move used by the target. The copied move has the maximum PP for that move. Fails if the target Has not made a move, if the user has Transformed, or if the move is Chatter, Sketch, Struggle, or any move the user knows.",
		//shortDesc = "Permanently copies the last move target Used.",
		id = Moves.SKETCH,
		//name = "Sketch",
		pp = 1,
		noPPBoosts = true,
		priority = 0,
		flags = new Flags (authentic: true, mystery: true),
		onHit = function (target, source) {
			let disallowedMoves = ["chatter", "sketch", "struggle"];
			if (source.transformed || !target.LastMove || disallowedMoves.INCLUDES(target.LastMove.id) || source.moves.INDEXOF(target.LastMove.id) >= 0 || target.LastMove.isZ) return false;
			let sketchIndex = source.moves.INDEXOF("sketch");
			if (sketchIndex < 0) return false;
			let move = this.getMove(target.LastMove);
			let sketchedMove = {
				move = move.name,
				id = move.id,
				pp = move.pp,
				maxpp = move.pp,
				target = move.target,
				disabled = false,
				used = false
			};
			source.moveSlOTS[sketchIndex] = sketchedMove;
			source.baseMoveSlOTS[sketchIndex] = sketchedMove;
			this.add('-activate', source, 'move = Sketch', move.name);
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {atk = 1, def = 1, spa = 1, spd = 1, spe = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 285,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user swaps its Ability with the target'S Ability. Fails if either the user or the target'S Ability is Illusion, Multitype, Stance Change, or Wonder Guard, but does not fail if both have the same Ability.",
		//shortDesc = "The user and the target Trade Abilities.",
		id = Moves.SKILLSWAP,
		//name = "Skill Swap",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, authentic: true, mystery: true),
		onTryHit = function (target, source) {
			let bannedAbilities = ["battlebond", "comatose", "disguise", "illusion", "multitype", "powerconstruct", "rkssystem", "schooling", "shieldsdown", "stancechange", "wonderguard"];
			if (bannedAbilities.includes(target.Ability) || bannedAbilities.includes(source.ability)) {
				return false;
			}
		},
		onHit = function (target, source, move) {
			let targetABility = this.getAbility(target.Ability);
			let sourceAbility = this.getAbility(source.ability);
			if (target.Side === source.side) {
				this.add('-activate', source, 'move = Skill Swap', "", "", '[of] ' + target);
			} else {
				this.add('-activate', source, 'move = Skill Swap', targetABility, sourceAbility, '[of] ' + target);
			}
			this.singleEvent("End", sourceAbility, source.abilityData, source);
			this.singleEvent("End", targetABility, target.AbilityData, target);
			if (targetABility.id !== sourceAbility.id) {
				source.ability = targetABility.id;
				target.Ability = sourceAbility.id;
				source.abilityData = {id = toId(source.ability), target = source};
				target.AbilityData = {id = toId(target.Ability), target = target};
			}
			this.singleEvent("Start", targetABility, source.abilityData, source);
			this.singleEvent("Start", sourceAbility, target.AbilityData, target);
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.PSYCHIC,
		zMoveBoost = new Boosts() {spe = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 130,
		accuracy = 100,
		basePower = 130,
		category = Category.PHYSICAL,
		//desc = "This attack charges on the first turn and executes on the second. Raises the user's Defense by 1 stage on the first turn. If the user is holding a Power Herb, the move completes in one turn.",
		//shortDesc = "Raises user's Defense by 1 on turn 1. Hits turn 2.",
		id = Moves.SKULLBASH,
		//name = "Skull Bash",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, charge: true, protect: true, mirror: true),
		onTry = function (attacker, defender, move) {
			if (attacker.removeVolatile(move.id)) {
				return;
			}
			this.add('-prepare', attacker, move.name, defender);
			this.boost({def = 1}, attacker, attacker, this.getMove("skullbash"));
			if (!this.runEvent("ChargeMove", attacker, defender, move)) {
				this.add('-anim', attacker, move.name, defender);
				attacker.removeVolatile(move.id);
				return;
			}
			attacker.addVolatile("twoturnmove", defender);
			return null;
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 195,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 143,
		accuracy = 90,
		basePower = 140,
		category = Category.PHYSICAL,
		//desc = "Has a 30% chance to flinch the target And a higher chance for a critical hit. This attack charges on the first turn and executes on the second. If the user is holding a Power Herb, the move completes in one turn.",
		//shortDesc = "Charges, then hits turn 2. 30% flinch. High crit.",
		id = Moves.SKYATTACK,
		//name = "Sky Attack",
		pp = 5,
		priority = 0,
		flags = new Flags (charge: true, protect: true, mirror: true, distance: true),
		critRatio = 2,
		onTry = function (attacker, defender, move) {
			if (attacker.removeVolatile(move.id)) {
				return;
			}
			this.add('-prepare', attacker, move.name, defender);
			if (!this.runEvent("ChargeMove", attacker, defender, move)) {
				this.add('-anim', attacker, move.name, defender);
				return;
			}
			attacker.addVolatile("twoturnmove", defender);
			return null;
		},
		secondary = new Secondary() {
			chance = 30,
			volatileStatus = "flinch"
		},
		target = Target.Any,
		type = Types.FLYING,
		zMovePower = 200,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 507,
		accuracy = 100,
		basePower = 60,
		category = Category.PHYSICAL,
		//desc = "This attack takes the target Into the air with the user on the first turn and executes on the second. Pokemon weighing 200 kg or more cannot be lifted. On the first turn, the user and the target Avoid all attacks other than Gust, Hurricane, Sky Uppercut, Smack Down, Thousand Arrows, Thunder, and Twister. The user and the target Cannot make a move between turns, but the target Can select a move to use. This move cannot damage Flying-type Pokemon. Fails on the first turn if the target Is an ally or if the target Has a substitute.",
		//shortDesc = "User and foe fly up turn 1. Damages on turn 2.",
		id = Moves.SKYDROP,
		//name = "Sky Drop",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, charge: true, protect: true, mirror: true, gravity: true, distance: true),
		onModifyMove = function (move, source) {
			if (!source.volatiles["skydrop"]) {
				move.//accuracy = true;
				// @ts-ignore
				move.flags.contact = 0;
			}
		},
		onMoveFail = function (target, source) {
			if (source.volatiles["twoturnmove"] && source.volatiles["twoturnmove"].duration === 1) {
				source.removeVolatile("skydrop");
				source.removeVolatile("twoturnmove");
				this.add('-end', target, 'Sky Drop', '[interrupt]');
			}
		},
		onTryHit = function (target, source, move) {
			if (target.Fainted) return false;
			if (source.removeVolatile(move.id)) {
				if (target !== source.volatiles["twoturnmove"].source) return false;

				if (target.HasType("Flying")) {
					this.add('-immune', target, '[msg]');
					this.add('-end', target, 'Sky Drop');
					return null;
				}
			} else {
				if (target.Volatiles["substitute"] || target.Side === source.side) {
					return false;
				}
				if (target.GetWeight() >= 200) {
					this.add('-fail', target, 'move = Sky Drop', '[heavy]');
					return null;
				}

				this.add('-prepare', source, move.name, target);
				source.addVolatile("twoturnmove", target);
				return null;
			}
		},
		onHit = function (target, source) {
			this.add('-end', target, 'Sky Drop');
		},
		effect = {
			duration = 2,
			onAnyDragOut = function (pokemon) {
				if (pokemon === this.effectData.target || pokemon === this.effectData.source) return false;
			},
			onFoeTrapPokemonPriority = -15,
			onFoeTrapPokemon = function (defender) {
				if (defender !== this.effectData.source) return;
				defender.trapped = true;
			},
			onFoeBeforeMovePriority = 12,
			onFoeBeforeMove = function (attacker, defender, move) {
				if (attacker === this.effectData.source) {
					this.effectData.source.activeTurns--;
					this.debug('Sky drop nullifying.');
					return null;
				}
			},
			onRedirectTargetPRiority = 99,
			onRedirectTarget = function (target, source, source2) {
				if (source !== this.effectData.target) return;
				if (this.effectData.source.fainted) return;
				return this.effectData.source;
			},
			onAnyTryImmunity = function (target, source, move) {
				if (target !== this.effectData.target && target !== this.effectData.source) {
					return;
				}
				if (source === this.effectData.target && target === this.effectData.source) {
					return;
				}
				if (move.id === "gust" || move.id === "twister") {
					return;
				}
				if (move.id === "skyuppercut" || move.id === "thunder" || move.id === "hurricane" || move.id === "smackdown" || move.id === "thousandarrows" || move.id === "helpinghand") {
					return;
				}
				if (source.hasAbility("noguard") || target.HasAbility("noguard")) {
					return;
				}
				if (source.volatiles["lockon"] && target === source.volatiles["lockon"].source) return;
				return false;
			},
			onAnyBasePower = function (basePower, target, source, move) {
				if (target !== this.effectData.target && target !== this.effectData.source) {
					return;
				}
				if (source === this.effectData.target && target === this.effectData.source) {
					return;
				}
				if (move.id === "gust" || move.id === "twister") {
					return this.chainModify(2);
				}
			},
			onFaint = function (target) {
				if (target.Volatiles["skydrop"] && target.Volatiles["twoturnmove"].source) {
					this.add('-end', target.Volatiles["twoturnmove"].source, 'Sky Drop', '[interrupt]');
				}
			}
		},
		//secondary = false,
		target = Target.Any,
		type = Types.FLYING,
		zMovePower = 120,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 327,
		accuracy = 90,
		basePower = 85,
		category = Category.PHYSICAL,
		//desc = "This move can hit a target Using Bounce, Fly, or Sky Drop.",
		//shortDesc = "Can hit Pokemon using Bounce, Fly, or Sky Drop.",
		id = Moves.SKYUPPERCUT,
		//name = "Sky Uppercut",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true, punch: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.FIGHTING,
		zMovePower = 160,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 303,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user restores 1/2 of its maximum HP, rounded half up.",
		//shortDesc = "Heals the user by 50% of its max HP.",
		id = Moves.SLACKOFF,
		isViable = true,
		//name = "Slack Off",
		pp = 10,
		priority = 0,
		flags = new Flags (snatch: true, heal: true),
		heal = new int[] {1, 2},
		//secondary = false,
		target = Target.Self,
		type = Types.NORMAL,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 21,
		accuracy = 75,
		basePower = 80,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect.",
		id = Moves.SLAM,
		//name = "Slam",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true, nonsky: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 160,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 163,
		accuracy = 100,
		basePower = 70,
		category = Category.PHYSICAL,
		//desc = "Has a higher chance for a critical hit.",
		//shortDesc = "High critical hit ratio.",
		id = Moves.SLASH,
		//name = "Slash",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		critRatio = 2,
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 140,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 79,
		accuracy = 75,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Causes the target To fall asleep.",
		//shortDesc = "Puts the target To sleep.",
		id = Moves.SLEEPPOWDER,
		isViable = true,
		//name = "Sleep Powder",
		pp = 15,
		priority = 0,
		flags = new Flags (powder: true, protect: true, reflectable: true, mirror: true),
		status = "slp",
		//secondary = false,
		target = Target.Normal,
		type = Types.GRASS,
		zMoveBoost = new Boosts() {spe = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 214,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "One of the user's known moves, besides this move, is selected for use at random. Fails if the user is not asleep. The selected move does not have PP deducted from it, and can currently have 0 PP. This move cannot select Assist, Belch, Bide, Chatter, Copycat, Focus Punch, Hold Hands, Me First, Metronome, Mimic, Mirror Move, Nature Power, Sketch, Sleep Talk, Struggle, Uproar, or any two-turn move.",
		//shortDesc = "User must be asleep. Uses another known move.",
		id = Moves.SLEEPTALK,
		isViable = true,
		//name = "Sleep Talk",
		pp = 10,
		priority = 0,
		flags = new Flags (),
		sleepUsable = true,
		onTryHit = function (pokemon) {
			if (pokemon.status !== "slp" && !pokemon.hasAbility("comatose")) return false;
		},
		onHit = function (pokemon) {
			let moves = [];
			for (const moveSlOT of pokemon.moveSlOTS) {
				const move = moveSlOT.id;
				const noSleepTalk = [
					"assist", "beakblast", "belch", "bide", "chatter", "copycat", "focuspunch", "mefirst", "metronome", "mimic", "mirrormove", "naturepower", "shelltrap", "sketch", "sleeptalk", "uproar",
				];
				if (move && !(noSleepTalk.includes(move) || this.getMove(move).flags["charge"] || (this.getMove(move).isZ && this.getMove(move).basePower !== 1))) {
					moves.PUSH(move);
				}
			}
			let randomMove = "";
			if (moves.LENGTH) randomMove = this.sample(moves);
			if (!randomMove) {
				return false;
			}
			this.useMove(randomMove, pokemon);
		},
		//secondary = false,
		target = Target.Self,
		type = Types.NORMAL,
		zMoveEffect = "crit2",
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 124,
		accuracy = 100,
		basePower = 65,
		category = Category.SPECIAL,
		//desc = "Has a 30% chance to poison the target.",
		//shortDesc = "30% chance to poison the target.",
		id = Moves.SLUDGE,
		//name = "Sludge",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 30,
			status = "psn"
		},
		target = Target.Normal,
		type = Types.POISON,
		zMovePower = 120,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 188,
		accuracy = 100,
		basePower = 90,
		category = Category.SPECIAL,
		//desc = "Has a 30% chance to poison the target.",
		//shortDesc = "30% chance to poison the target.",
		id = Moves.SLUDGEBOMB,
		isViable = true,
		//name = "Sludge Bomb",
		pp = 10,
		priority = 0,
		flags = new Flags (bullet: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 30,
			status = "psn"
		},
		target = Target.Normal,
		type = Types.POISON,
		zMovePower = 175,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 482,
		accuracy = 100,
		basePower = 95,
		category = Category.SPECIAL,
		//desc = "Has a 10% chance to poison the target.",
		//shortDesc = "10% chance to poison adjacent Pokemon.",
		id = Moves.SLUDGEWAVE,
		isViable = true,
		//name = "Sludge Wave",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 10,
			status = "psn"
		},
		target = Target.AllAdjacent,
		type = Types.POISON,
		zMovePower = 175,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 479,
		accuracy = 100,
		basePower = 50,
		category = Category.PHYSICAL,
		//desc = "This move can hit a target Using Bounce, Fly, or Sky Drop. If this move hits a target Under the effect of Bounce, Fly, Magnet Rise, or Telekinesis, the effect ends. If the target Is a Flying type that has not used Roost this turn or a Pokemon with the Ability Levitate, it loses its immunity to Ground-type attacks and the Ability Arena Trap as long as it remains active. During the effect, Magnet Rise fails for the target And Telekinesis fails against the target.",
		//shortDesc = "Removes THE target'S Ground immunity.",
		id = Moves.SMACKDOWN,
		//name = "Smack Down",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, nonsky: true),
		volatileStatus = "smackdown",
		effect = {
			noCopy = true,
			onStart = function (pokemon) {
				let applies = false;
				if (pokemon.hasType("Flying") || pokemon.hasAbility("levitate")) applies = true;
				if (pokemon.hasItem("ironball") || pokemon.volatiles["ingrain"] || this.getPseudoWeather("gravity")) applies = false;
				if (pokemon.removeVolatile("fly") || pokemon.removeVolatile("bounce")) {
					applies = true;
					this.cancelMove(pokemon);
					pokemon.removeVolatile("twoturnmove");
				}
				if (pokemon.volatiles["magnetrise"]) {
					applies = true;
					delete pokemon.volatiles["magnetrise"];
				}
				if (pokemon.volatiles["telekinesis"]) {
					applies = true;
					delete pokemon.volatiles["telekinesis"];
				}
				if (!applies) return false;
				this.add('-start', pokemon, 'Smack Down');
			},
			onRestart = function (pokemon) {
				if (pokemon.removeVolatile("fly") || pokemon.removeVolatile("bounce")) {
					this.cancelMove(pokemon);
					this.add('-start', pokemon, 'Smack Down');
				}
			},
			// groundedness implemented in battle.engine.js:BattlePokemon#isGrounded
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.ROCK,
		zMovePower = 100,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 684,
		//accuracy = true,
		basePower = 70,
		category = Category.PHYSICAL,
		//desc = "This move does not check accuracy.",
		//shortDesc = "This move does not check accuracy.",
		id = Moves.SMARTSTRIKE,
		//name = "Smart Strike",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.STEEL,
		zMovePower = 140,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 265,
		accuracy = 100,
		basePower = 70,
		basePowerCallback = function (pokemon, target, move) {
			if (target.Status === "par") return move.basePower * 2;
			return move.basePower;
		},
		category = Category.PHYSICAL,
		//desc = "Power doubles if the target Is paralyzed. If the user has not fainted, the target Is cured of paralysis.",
		//shortDesc = "Power doubles if target Is paralyzed, and cures it.",
		id = Moves.SMELLINGSALTS,
		//name = "Smelling Salts",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		onHit = function (target) {
			if (target.Status === "par") target.CureStatus();
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 140,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 123,
		accuracy = 70,
		basePower = 30,
		category = Category.SPECIAL,
		//desc = "Has a 40% chance to poison the target.",
		//shortDesc = "40% chance to poison the target.",
		id = Moves.SMOG,
		//name = "Smog",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 40,
			status = "psn"
		},
		target = Target.Normal,
		type = Types.POISON,
		zMovePower = 100,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 108,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Lowers the target'S accuracy by 1 stage.",
		//shortDesc = "Lowers the target'S accuracy by 1.",
		id = Moves.SMOKESCREEN,
		//name = "Smokescreen",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true),
		boosts = new Boosts() {
			accuracy = -1
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {evasion = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 555,
		accuracy = 95,
		basePower = 55,
		category = Category.SPECIAL,
		//desc = "Has a 100% chance to lower the target'S Special Attack by 1 stage.",
		//shortDesc = "100% chance to lower the foe(s) Sp. Atk by 1.",
		id = Moves.SNARL,
		//name = "Snarl",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, sound: true, authentic: true),
		secondary = new Secondary() {
			chance = 100,
			boosts = new Boosts() {
				spa = -1
			}
		},
		target = Target.AllAdjacentFoes,
		type = Types.DARK,
		zMovePower = 100,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 289,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "If another Pokemon uses one of the following moves THIS turn, the user steals that move to use itself. If multiple Pokemon use this move in the same turn, the applicable moves WILL be stolen in turn order; a move cannot be stolen multiple times. Aqua Ring, Aromatherapy, Autotomize, Belly Drum, Camouflage, Charge, Conversion, Cosmic Power, Cotton Guard, Heal Bell, Healing Wish, Imprison, Ingrain, Light Screen, Lucky Chant, Lunar Dance, Magnet Rise, Mist, Power Trick, Quick Guard, Recycle, Reflect, Refresh, Safeguard, Stockpile, Substitute, Swallow, Tailwind, Wide Guard, Wish, and any move that has a primary effect of raising the user's stats or healing the user.",
		//shortDesc = "User steals certain support moves TO use itself.",
		id = Moves.SNATCH,
		//name = "Snatch",
		pp = 10,
		priority = 4,
		flags = new Flags (authentic: true),
		volatileStatus = "snatch",
		effect = {
			duration = 1,
			onStart = function (pokemon) {
				this.add('-singleturn', pokemon, "Snatch");
			},
			onAnyTryMove = function (source, target, move) {
				let snatchUser = this.effectData.source;
				if (snatchUser.isSkyDropped()) return;
				if (move && !move.isZ && move.flags["snatch"] && move.sourceEffect !== "snatch") {
					snatchUser.removeVolatile("snatch");
					this.add('-activate', snatchUser, 'move = Snatch', '[of] ' + source);
					this.useMove(move.id, snatchUser);
					return null;
				}
			}
		},
		//secondary = false,
		pressuretarget = Target.FoeSide,
		target = Target.Self,
		type = Types.DARK,
		zMoveBoost = new Boosts() {spe = 2},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 173,
		accuracy = 100,
		basePower = 50,
		category = Category.SPECIAL,
		//desc = "Has a 30% chance to flinch the target. Fails if the user is not asleep.",
		//shortDesc = "User must be asleep. 30% chance to flinch target.",
		id = Moves.SNORE,
		//name = "Snore",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, sound: true, authentic: true),
		sleepUsable = true,
		onTryHit = function (target, source) {
			if (source.status !== "slp" && !source.hasAbility("comatose")) return false;
		},
		secondary = new Secondary() {
			chance = 30,
			volatileStatus = "flinch"
		},
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 100,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 712,
		accuracy = 100,
		basePower = 90,
		category = Category.PHYSICAL,
		//desc = "The target'S raised stat stages are stolen from it and applied to the user before dealing damage.",
		//shortDesc = "Steals target'S boosts before dealing damage.",
		id = Moves.SPECTRALTHIEF,
		isViable = true,
		//name = "Spectral Thief",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true, authentic: true),
		stealsBoosts = true,
		// Boost stealing implemented in scripts.js
		//secondary = false,
		target = Target.Normal,
		type = Types.GHOST,
		zMovePower = 175,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 683,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user swaps its raw Speed stat with the target.",
		//shortDesc = "Swaps Speed stat with target.",
		id = Moves.SPEEDSWAP,
		//name = "Speed Swap",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, authentic: true, mystery: true),
		onHit = function (target, source) {
			const targetSPe = target.Stats.spe;
			target.Stats.spe = source.stats.spe;
			source.stats.spe = targetSPe;
			this.add('-activate', source, 'move = Speed Swap', '[of] ' + target);
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.PSYCHIC,
		zMoveBoost = new Boosts() {spe = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 596,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user is protected from most attacks made by other Pokemon during this turn, and Pokemon making contact with the user lose 1/8 of their maximum HP, rounded down. This move has a 1/X chance of being successful, where X starts at 1 and triples each time this move is successfully used. X resets to 1 if this move fails or if the user's last move used is not Baneful Bunker, Detect, Endure, King's Shield, Protect, Quick Guard, Spiky Shield, or Wide Guard. Fails if the user moves LAST this turn.",
		//shortDesc = "Protects from moves. Contact = loses 1/8 max HP.",
		id = Moves.SPIKYSHIELD,
		isViable = true,
		//name = "Spiky Shield",
		pp = 10,
		priority = 4,
		flags = new Flags (),
		stallingMove = true,
		volatileStatus = "spikyshield",
		onTryHit = function (target, source, move) {
			return !!this.willAct() && this.runEvent("StallMove", target);
		},
		onHit = function (pokemon) {
			pokemon.addVolatile("stall");
		},
		effect = {
			duration = 1,
			onStart = function (target) {
				this.add('-singleturn', target, 'move = Protect');
			},
			onTryHitPriority = 3,
			onTryHit = function (target, source, move) {
				if (!move.flags["protect"]) {
					if (move.isZ) move.zBrokeProtect = true;
					return;
				}
				this.add('-activate', target, 'move = Protect');
				source.moveThisTurnResult = true;
				let lockedmove = source.getVolatile("lockedmove");
				if (lockedmove) {
					// Outrage counter is reset
					if (source.volatiles["lockedmove"].duration === 2) {
						delete source.volatiles["lockedmove"];
					}
				}
				if (move.flags["contact"]) {
					this.damage(source.maxhp / 8, source, target);
				}
				return null;
			},
			onHit = function (target, source, move) {
				if (move.zPowered && move.flags["contact"]) {
					this.damage(source.maxhp / 8, source, target);
				}
			}
		},
		//secondary = false,
		target = Target.Self,
		type = Types.GRASS,
		zMoveBoost = new Boosts() {def = 1},
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 662,
		accuracy = 100,
		basePower = 80,
		category = Category.PHYSICAL,
		//desc = "Prevents the target From switching out. The target Can still switch out if it is holding Shed Shell or uses Baton Pass, Parting Shot, U-turn, or Volt Switch. If the target Leaves the field using Baton Pass, the replacement will remain trapped. The effect ends if the user leaves the field.",
		//shortDesc = "Prevents the target From switching out.",
		id = Moves.SPIRITSHACKLE,
		isViable = true,
		//name = "Spirit Shackle",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 100,
			onHit = function (target, source, move) {
				if (source.isActive) target.AddVolatile("trapped", source, move, "trapper");
			}
		},
		target = Target.Normal,
		type = Types.GHOST,
		zMovePower = 160,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 487,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Causes the target To become a Water type. Fails if the target Is an Arceus.",
		//shortDesc = "Changes the target'S type to Water.",
		id = Moves.SOAK,
		//name = "Soak",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true, mystery: true),
		onHit = function (target) {
			if (!target.SetType("Water")) return false;
			this.add('-start', target, "typechange", "Water");
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.WATER,
		zMoveBoost = new Boosts() {spa = 1},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 135,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user restores 1/2 of its maximum HP, rounded half up.",
		//shortDesc = "Heals the user by 50% of its max HP.",
		id = Moves.SOFTBOILED,
		isViable = true,
		//name = "Soft-Boiled",
		pp = 10,
		priority = 0,
		flags = new Flags (snatch: true, heal: true),
		heal = new int[] {1, 2},
		//secondary = false,
		target = Target.Self,
		type = Types.NORMAL,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 76,
		accuracy = 100,
		basePower = 120,
		category = Category.SPECIAL,
		//desc = "This attack charges on the first turn and executes on the second. Power is halved if the weather is Hail, Rain Dance, or Sandstorm. If the user is holding a Power Herb or the weather is Sunny Day, the move completes in one turn.",
		//shortDesc = "Charges turn 1. Hits turn 2. No charge in sunlight.",
		id = Moves.SOLARBEAM,
		//name = "Solar Beam",
		pp = 10,
		priority = 0,
		flags = new Flags (charge: true, protect: true, mirror: true),
		onTry = function (attacker, defender, move) {
			if (attacker.removeVolatile(move.id)) {
				return;
			}
			this.add('-prepare', attacker, move.name, defender);
			if (this.isWeather(["sunnyday", "desolateland"]) || !this.runEvent("ChargeMove", attacker, defender, move)) {
				this.add('-anim', attacker, move.name, defender);
				return;
			}
			attacker.addVolatile("twoturnmove", defender);
			return null;
		},
		onBasePowerPriority = 4,
		onBasePower = function (basePower, pokemon, target) {
			if (this.isWeather(["raindance", "primordialsea", "sandstorm", "hail"])) {
				this.debug('weakened by weather');
				return this.chainModify(0.5);
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.GRASS,
		zMovePower = 190,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 669,
		accuracy = 100,
		basePower = 125,
		category = Category.PHYSICAL,
		//desc = "This attack charges on the first turn and executes on the second. Power is halved if the weather is Hail, Rain Dance, or Sandstorm. If the user is holding a Power Herb or the weather is Sunny Day, the move completes in one turn.",
		//shortDesc = "Charges turn 1. Hits turn 2. No charge in sunlight.",
		id = Moves.SOLARBLADE,
		//name = "Solar Blade",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, charge: true, protect: true, mirror: true),
		onTry = function (attacker, defender, move) {
			if (attacker.removeVolatile(move.id)) {
				return;
			}
			this.add('-prepare', attacker, move.name, defender);
			if (this.isWeather(["sunnyday", "desolateland"]) || !this.runEvent("ChargeMove", attacker, defender, move)) {
				this.add('-anim', attacker, move.name, defender);
				return;
			}
			attacker.addVolatile("twoturnmove", defender);
			return null;
		},
		onBasePowerPriority = 4,
		onBasePower = function (basePower, pokemon, target) {
			if (this.isWeather(["raindance", "primordialsea", "sandstorm", "hail"])) {
				this.debug('weakened by weather');
				return this.chainModify(0.5);
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.GRASS,
		zMovePower = 190,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 49,
		accuracy = 90,
		basePower = 0,
		damage = 20,
		category = Category.SPECIAL,
		//desc = "Deals 20 HP of damage to the target.",
		//shortDesc = "Always does 20 HP of damage.",
		id = Moves.SONICBOOM,
		//name = "Sonic Boom",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 100,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 699,
		//accuracy = true,
		basePower = 195,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect.",
		id = Moves.SOULSTEALING7STARSTRIKE,
		//name = "Soul-Stealing 7-Star Strike",
		pp = 1,
		priority = 0,
		flags = new Flags (contact: true),
		isZ = "marshadiumz",
		//secondary = false,
		target = Target.Normal,
		type = Types.GHOST,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 460,
		accuracy = 95,
		basePower = 100,
		category = Category.SPECIAL,
		//desc = "Has a higher chance for a critical hit.",
		//shortDesc = "High critical hit ratio.",
		id = Moves.SPACIALREND,
		isViable = true,
		//name = "Spacial Rend",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		critRatio = 2,
		//secondary = false,
		target = Target.Normal,
		type = Types.DRAGON,
		zMovePower = 180,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 209,
		accuracy = 100,
		basePower = 65,
		category = Category.PHYSICAL,
		//desc = "Has a 30% chance to paralyze the target.",
		//shortDesc = "30% chance to paralyze the target.",
		id = Moves.SPARK,
		//name = "Spark",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 30,
			status = "par"
		},
		target = Target.Normal,
		type = Types.ELECTRIC,
		zMovePower = 120,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 664,
		accuracy = 100,
		basePower = 90,
		category = Category.SPECIAL,
		//desc = "If the user has not fainted, the target Is cured of its burn.",
		//shortDesc = "The target Is cured of its burn.",
		id = Moves.SPARKLINGARIA,
		//name = "Sparkling Aria",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, sound: true, authentic: true),
		secondary = new Secondary() {
			dustproof = true,
			chance = 100,
			onHit = function (target) {
				if (target.Status === "brn") target.CureStatus();
			}
		},
		target = Target.AllAdjacent,
		type = Types.WATER,
		zMovePower = 175,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 169,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Prevents the target From switching out. The target Can still switch out if it is holding Shed Shell or uses Baton Pass, Parting Shot, U-turn, or Volt Switch. If the target Leaves the field using Baton Pass, the replacement will remain trapped. The effect ends if the user leaves the field.",
		//shortDesc = "The target Cannot switch out.",
		id = Moves.SPIDERWEB,
		//name = "Spider Web",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true),
		onHit = function (target, source, move) {
			return target.AddVolatile("trapped", source, move, "trapper");
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.BUG,
		zMoveBoost = new Boosts() {def = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 131,
		accuracy = 100,
		basePower = 20,
		category = Category.PHYSICAL,
		//desc = "Hits two to five times. Has a 1/3 chance to hit two or three times, and a 1/6 chance to hit four or five times. If one of the hits breaks the target'S substitute, it will take damage for the remaining hits. If the user has the Ability Skill Link, this move will always hit five times.",
		//shortDesc = "Hits 2-5 times in one turn.",
		id = Moves.SPIKECANNON,
		//name = "Spike Cannon",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		multihit = new int[] {2, 5},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 100,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 191,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Sets up a hazard on the foe's side of the field, damaging each foe that switches in, unless it is a Flying-type Pokemon or has the Ability Levitate. Can be used up to three times before failing. Foes lose 1/8 of their maximum HP with one layer, 1/6 of their maximum HP with two layers, and 1/4 of their maximum HP with three layers, all rounded down. Can be removed from the foe's side if any foe uses Rapid Spin or Defog, or is hit by Defog.",
		//shortDesc = "Hurts grounded foes on switch-in. Max 3 layers.",
		id = Moves.SPIKES,
		isViable = true,
		//name = "Spikes",
		pp = 20,
		priority = 0,
		flags = new Flags (reflectable: true, nonsky: true),
		sideCondition = "spikes",
		effect = {
			// this is a side condition
			onStart = function (side) {
				this.add('-sidestart', side, "Spikes");
				this.effectData.layers = 1;
			},
			onRestart = function (side) {
				if (this.effectData.layers >= 3) return false;
				this.add('-sidestart', side, "Spikes");
				this.effectData.layers++;
			},
			onSwitchIn = function (pokemon) {
				if (!pokemon.isGrounded()) return;
				let damageAmounts = [0, 3, 4, 6]; // 1/8, 1/6, 1/4
				this.damage(damageAmounts[this.effectData.layers] * pokemon.maxhp / 24);
			}
		},
		//secondary = false,
		target = Target.FoeSide,
		type = Types.GROUND,
		zMoveBoost = new Boosts() {def = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 255,
		accuracy = 100,
		basePower = 0,
		basePowerCallback = function (pokemon) {
			if (!pokemon.volatiles["stockpile"] || !pokemon.volatiles["stockpile"].layers) return false;
			return pokemon.volatiles["stockpile"].layers * 100;
		},
		category = Category.SPECIAL,
		//desc = "Power is equal to 100 times the user's Stockpile count. Fails if the user's Stockpile count is 0. Whether or not this move is successful, the user's Defense and Special Defense decrease by as many stages as Stockpile had increased them, and the user's Stockpile count resets to 0.",
		//shortDesc = "More power with more uses of Stockpile.",
		id = Moves.SPITUP,
		//name = "Spit Up",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true),
		onTry = function (pokemon) {
			if (!pokemon.volatiles["stockpile"]) {
				return false;
			}
		},
		onAfterMove = function (pokemon) {
			pokemon.removeVolatile("stockpile");
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 100,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 180,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Causes the target'S last move used to lose 4 PP. Fails if the target Has not made a move, if the move has 0 PP, or if it no longer knows the move.",
		//shortDesc = "Lowers the PP of the target'S last move by 4.",
		id = Moves.SPITE,
		//name = "Spite",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true, authentic: true),
		onHit = function (target) {
			if (target.LastMove && !target.LastMove.isZ) {
				let ppDeducted = target.DeductPP(target.LastMove.id, 4);
				if (ppDeducted) {
					this.add("-activate", target, 'move = Spite', this.getMove(target.LastMove.id).name, ppDeducted);
					return;
				}
			}
			return false;
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.GHOST,
		zMoveEffect = "heal",
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 150,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Nothing happens...",
		//shortDesc = "Does nothing (but we still love it).",
		id = Moves.SPLASH,
		//name = "Splash",
		pp = 40,
		priority = 0,
		flags = new Flags (gravity: true),
		onTryHit = function (target, source) {
			this.add('-nothing');
		},
		//secondary = false,
		target = Target.Self,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {atk = 3},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 727,
		//accuracy = true,
		basePower = 190,
		category = Category.PHYSICAL,
		//desc = "Ends the effects of Electric Terrain, Grassy Terrain, Misty Terrain, and Psychic Terrain.",
		//shortDesc = "Ends the effects of Terrain.",
		id = Moves.SPLINTEREDSTORMSHARDS,
		//name = "Splintered Stormshards",
		pp = 1,
		priority = 0,
		flags = new Flags (),
		onHit = function () {
			this.clearTerrain();
		},
		isZ = "lycaniumz",
		//secondary = false,
		target = Target.Normal,
		type = Types.ROCK,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 147,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Causes the target To fall asleep.",
		//shortDesc = "Puts the target To sleep.",
		id = Moves.SPORE,
		isViable = true,
		//name = "Spore",
		pp = 15,
		priority = 0,
		flags = new Flags (powder: true, protect: true, reflectable: true, mirror: true),
		status = "slp",
		//secondary = false,
		target = Target.Normal,
		type = Types.GRASS,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 671,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Until the end of the turn, all single-target Attacks from opponents of the target Are redirected to the target. Such attacks are redirected to the target Before they can be reflected by Magic Coat or the Ability Magic Bounce, or drawn in by the Abilities Lightning Rod or Storm Drain. Fails if it is not a Double Battle or Battle Royale.",
		//shortDesc = "Target'S foes' moves ARE redirected to it this turn.",
		id = Moves.SPOTLIGHT,
		//name = "Spotlight",
		pp = 15,
		priority = 3,
		flags = new Flags (protect: true, reflectable: true, mystery: true),
		volatileStatus = "spotlight",
		onTryHit = function (target) {
			if (target.Side.active.length < 2) return false;
		},
		effect = {
			duration = 1,
			onStart = function (pokemon) {
				this.add('-singleturn', pokemon, 'move = Spotlight');
			},
			onFoeRedirectTargetPRiority = 2,
			onFoeRedirectTarget = function (target, source, source2, move) {
				if (this.validTarget(This.effectData.target, source, move.target)) {
					this.debug("Spotlight redirected target Of move");
					return this.effectData.target;
				}
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {spd = 1},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 446,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Sets up a hazard on the foe's side of the field, damaging each foe that switches in. Can be used only once before failing. Foes lose 1/32, 1/16, 1/8, 1/4, or 1/2 of their maximum HP, rounded down, based on their weakness to the Rock type; 0.25x, 0.5x, neutral, 2x, or 4x, respectively. Can be removed from the foe's side if any foe uses Rapid Spin or Defog, or is hit by Defog.",
		//shortDesc = "Hurts foes on switch-in. Factors Rock weakness.",
		id = Moves.STEALTHROCK,
		isViable = true,
		//name = "Stealth Rock",
		pp = 20,
		priority = 0,
		flags = new Flags (reflectable: true),
		sideCondition = "stealthrock",
		effect = {
			// this is a side condition
			onStart = function (side) {
				this.add('-sidestart', side, 'move = Stealth Rock');
			},
			onSwitchIn = function (pokemon) {
				let typeMod = this.clampIntRange(pokemon.runEffectiveness("Rock"), -6, 6);
				this.damage(pokemon.maxhp * Math.pow(2, typeMod) / 8);
			}
		},
		//secondary = false,
		target = Target.FoeSide,
		type = Types.ROCK,
		zMoveBoost = new Boosts() {def = 1},
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 592,
		accuracy = 95,
		basePower = 110,
		category = Category.SPECIAL,
		//desc = "Has a 30% chance to burn the target. The target Thaws out if it is frozen.",
		//shortDesc = "30% chance to burn the target.",
		id = Moves.STEAMERUPTION,
		isViable = true,
		//name = "Steam Eruption",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, defrost: true),
		thawsTarget = true,
		secondary = new Secondary() {
			chance = 30,
			status = "brn"
		},
		target = Target.Normal,
		type = Types.WATER,
		zMovePower = 185,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 211,
		accuracy = 90,
		basePower = 70,
		category = Category.PHYSICAL,
		//desc = "Has a 10% chance to raise the user's Defense by 1 stage.",
		//shortDesc = "10% chance to raise the user's Defense by 1.",
		id = Moves.STEELWING,
		//name = "Steel Wing",
		pp = 25,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 10,
			self = {
				boosts = new Boosts() {
					def = 1
				}
			}
		},
		target = Target.Normal,
		type = Types.STEEL,
		zMovePower = 140,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 564,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Sets up a hazard on the foe's side of the field, lowering the Speed by 1 stage of each foe that switches in, unless it is a Flying-type Pokemon or has the Ability Levitate. Can be used only once before failing. Can be removed from the foe's side if any foe uses Rapid Spin or Defog, or is hit by Defog.",
		//shortDesc = "Lowers Speed of grounded foes by 1 on switch-in.",
		id = Moves.STICKYWEB,
		isViable = true,
		//name = "Sticky Web",
		pp = 20,
		priority = 0,
		flags = new Flags (reflectable: true),
		sideCondition = "stickyweb",
		effect = {
			onStart = function (side) {
				this.add('-sidestart', side, 'move = Sticky Web');
			},
			onSwitchIn = function (pokemon) {
				if (!pokemon.isGrounded()) return;
				this.add('-activate', pokemon, 'move = Sticky Web');
				this.boost({spe = -1}, pokemon, pokemon.side.foe.active[0], this.getMove("stickyweb"));
			}
		},
		//secondary = false,
		target = Target.FoeSide,
		type = Types.BUG,
		zMoveBoost = new Boosts() {spe = 1},
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 254,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the user's Defense and Special Defense by 1 stage. The user's Stockpile count increases by 1. Fails if the user's Stockpile count is 3.",
		//shortDesc = "Raises user's Defense, Sp. Def by 1. Max 3 uses.",
		id = Moves.STOCKPILE,
		//name = "Stockpile",
		pp = 20,
		priority = 0,
		flags = new Flags (snatch: true),
		onTryHit = function (pokemon) {
			if (pokemon.volatiles["stockpile"] && pokemon.volatiles["stockpile"].layers >= 3) return false;
		},
		volatileStatus = "stockpile",
		effect = {
			noCopy = true,
			onStart = function (target) {
				this.effectData.layers = 1;
				this.add('-start', target, "stockpile" + this.effectData.layers);
				this.boost({def = 1, spd = 1}, target, target, this.getMove("stockpile"));
			},
			onRestart = function (target) {
				if (this.effectData.layers >= 3) return false;
				this.effectData.layers++;
				this.add('-start', target, "stockpile" + this.effectData.layers);
				this.boost({def = 1, spd = 1}, target, target, this.getMove("stockpile"));
			},
			onEnd = function (target) {
				let layers = this.effectData.layers * -1;
				this.effectData.layers = 0;
				this.boost({def = layers, spd = layers}, target, target, this.getMove("stockpile"));
				this.add('-end', target, "Stockpile");
			}
		},
		//secondary = false,
		target = Target.Self,
		type = Types.NORMAL,
		zMoveEffect = "heal",
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 700,
		//accuracy = true,
		basePower = 175,
		category = Category.SPECIAL,
		//desc = "Has a 100% chance to paralyze the target.",
		//shortDesc = "100% chance to paralyze the target.",
		id = Moves.STOKEDSPARKSURFER,
		//name = "Stoked Sparksurfer",
		pp = 1,
		priority = 0,
		flags = new Flags (),
		isZ = "aloraichiumz",
		secondary = new Secondary() {
			chance = 100,
			status = "par"
		},
		target = Target.Normal,
		type = Types.ELECTRIC,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 23,
		accuracy = 100,
		basePower = 65,
		category = Category.PHYSICAL,
		//desc = "Has a 30% chance to flinch the target. Damage doubles and no accuracy check is done if the target Has used Minimize while active.",
		//shortDesc = "30% chance to flinch the target.",
		id = Moves.STOMP,
		//name = "Stomp",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true, nonsky: true),
		secondary = new Secondary() {
			chance = 30,
			volatileStatus = "flinch"
		},
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 120,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 707,
		accuracy = 100,
		basePower = 75,
		basePowerCallback = function (pokemon, target, move) {
			if (pokemon.moveLastTurnResult === false) return move.basePower * 2;
			return move.basePower;
		},
		category = Category.PHYSICAL,
		//desc = "Power doubles if the user's last move on the previous turn, including moves CALLED by other moves OR those used out of turn through Instruct, Snatch, or Magic Coat or the Abilities Dancer or Magic Bounce, failed to do any of its normal effects, not counting damage from an unsuccessful Mind Blown, Jump Kick, or High Jump Kick, or if the user was prevented from moving by any effect other than recharging or Sky Drop. A move that was blocked by Baneful Bunker, Detect, King's Shield, Protect, Spiky Shield, Crafty Shield, Mat Block, Quick Guard, or Wide Guard will not double this move's power, nor will Bounce or Fly ending early due to the effect of Gravity, Smack Down, or Thousand Arrows.",
		//shortDesc = "Power doubles if the user's last move failed.",
		id = Moves.STOMPINGTANTRUM,
		//name = "Stomping Tantrum",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.GROUND,
		zMovePower = 140,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 444,
		accuracy = 80,
		basePower = 100,
		category = Category.PHYSICAL,
		//desc = "Has a higher chance for a critical hit.",
		//shortDesc = "High critical hit ratio.",
		id = Moves.STONEEDGE,
		isViable = true,
		//name = "Stone Edge",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		critRatio = 2,
		//secondary = false,
		target = Target.Normal,
		type = Types.ROCK,
		zMovePower = 180,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 500,
		accuracy = 100,
		basePower = 20,
		basePowerCallback = function (pokemon, target, move) {
			return move.basePower + 20 * pokemon.positiveBoosts();
		},
		category = Category.SPECIAL,
		//desc = "Power is equal to 20+(X*20), where X is the user's total stat stage changes that are greater than 0.",
		//shortDesc = " + 20 power for each of the user's stat boosts.",
		id = Moves.STOREDPOWER,
		//name = "Stored Power",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.PSYCHIC,
		zMovePower = 160,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 480,
		accuracy = 100,
		basePower = 60,
		category = Category.PHYSICAL,
		//desc = "This move is always a critical hit unless the target Is under the effect of Lucky Chant or has the Abilities Battle Armor or Shell Armor.",
		//shortDesc = "Always results in a critical hit.",
		id = Moves.STORMTHROW,
		isViable = true,
		//name = "Storm Throw",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		willCrit = true,
		//secondary = false,
		target = Target.Normal,
		type = Types.FIGHTING,
		zMovePower = 120,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 537,
		accuracy = 100,
		basePower = 65,
		category = Category.PHYSICAL,
		//desc = "Has a 30% chance to flinch the target. Damage doubles and no accuracy check is done if the target Has used Minimize while active.",
		//shortDesc = "30% chance to flinch the target.",
		id = Moves.STEAMROLLER,
		//name = "Steamroller",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 30,
			volatileStatus = "flinch"
		},
		target = Target.Normal,
		type = Types.BUG,
		zMovePower = 120,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 70,
		accuracy = 100,
		basePower = 80,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect.",
		id = Moves.STRENGTH,
		//name = "Strength",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 160,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 668,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Lowers the target'S Attack by 1 stage. The user restores its HP equal to the target'S Attack stat calculated with its stat stage before this move was used. If Big Root is held by the user, the HP recovered is 1.3x normal, rounded half down. Fails if the target'S Attack stat stage is -6.",
		//shortDesc = "User heals HP=target'S Atk stat. Lowers Atk by 1.",
		id = Moves.STRENGTHSAP,
		isViable = true,
		//name = "Strength Sap",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true, heal: true),
		onHit = function (target, source) {
			if (target.Boosts.atk === -6) return false;
			let atk = target.GetStat("atk", false, true);
			let success = this.boost({atk = -1}, target, source, null, false, true);
			return this.heal(atk, source, target) || success;
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.GRASS,
		zMoveBoost = new Boosts() {def = 1},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 81,
		accuracy = 95,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Lowers the target'S Speed by 2 stages.",
		//shortDesc = "Lowers the foe(s) Speed by 2.",
		id = Moves.STRINGSHOT,
		//name = "String Shot",
		pp = 40,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true),
		boosts = new Boosts() {
			spe = -2
		},
		//secondary = false,
		target = Target.AllAdjacentFoes,
		type = Types.BUG,
		zMoveBoost = new Boosts() {spe = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 165,
		//accuracy = true,
		basePower = 50,
		category = Category.PHYSICAL,
		//desc = "Deals typeless damage to one adjacent foe at random. If this move was successful, the user loses 1/4 of its maximum HP, rounded half up; the Ability Rock Head does not prevent this. This move can only be used if none of the user's known moves CAN be selected.",
		//shortDesc = "User loses 25% of its max HP as recoil.",
		id = Moves.STRUGGLE,
		//name = "Struggle",
		pp = 1,
		noPPBoosts = true,
		priority = 0,
		flags = new Flags (contact: true, protect: true),
		noSketch = true,
		onModifyMove = function (move, pokemon, target) {
			move.type = '???';
			this.add('-activate', pokemon, 'move = Struggle');
		},
		struggleRecoil = true,
		//secondary = false,
		target = Target.RandomNormal,
		type = Types.NORMAL,
		zMovePower = 1,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 522,
		accuracy = 100,
		basePower = 50,
		category = Category.SPECIAL,
		//desc = "Has a 100% chance to lower the target'S Special Attack by 1 stage.",
		//shortDesc = "100% chance to lower the foe(s) Sp. Atk by 1.",
		id = Moves.STRUGGLEBUG,
		//name = "Struggle Bug",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 100,
			boosts = new Boosts() {
				spa = -1
			}
		},
		target = Target.AllAdjacentFoes,
		type = Types.BUG,
		zMovePower = 100,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 78,
		accuracy = 75,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Paralyzes the target.",
		//shortDesc = "Paralyzes the target.",
		id = Moves.STUNSPORE,
		//name = "Stun Spore",
		pp = 30,
		priority = 0,
		flags = new Flags (powder: true, protect: true, reflectable: true, mirror: true),
		status = "par",
		//secondary = false,
		target = Target.Normal,
		type = Types.GRASS,
		zMoveBoost = new Boosts() {spd = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 66,
		accuracy = 80,
		basePower = 80,
		category = Category.PHYSICAL,
		//desc = "If the target Lost HP, the user takes recoil damage equal to 1/4 the HP lost by the target, rounded half up, but not less than 1 HP.",
		//shortDesc = "Has 1/4 recoil.",
		id = Moves.SUBMISSION,
		//name = "Submission",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		recoil = new int[] {1, 4},
		//secondary = false,
		target = Target.Normal,
		type = Types.FIGHTING,
		zMovePower = 160,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 164,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user takes 1/4 of its maximum HP, rounded down, and puts it into a substitute to take its place in battle. The substitute is removed once enough damage is inflicted on it, or if the user switches out or faints. Baton Pass can be used to transfer the substitute to an ally, and the substitute will keep its remaining HP. Until the substitute is broken, it receives damage from all attacks made by other Pokemon and shields the user from status effects and stat stage changes caused by other Pokemon. Sound-based moves AND Pokemon with the Ability Infiltrator ignore substitutes. The user still takes normal damage from weather and status effects while behind its substitute. If the substitute breaks during a multi-hit attack, the user will take damage from any remaining hits. If a substitute is created while the user is partially trapped, the partial-trapping effect ends immediately. This move fails if the user does not have enough HP remaining to create a substitute, or if it already has a substitute.",
		//shortDesc = "User takes 1/4 its max HP to put in a Substitute.",
		id = Moves.SUBSTITUTE,
		isViable = true,
		//name = "Substitute",
		pp = 10,
		priority = 0,
		flags = new Flags (snatch: true, nonsky: true),
		volatileStatus = "Substitute",
		onTryHit = function (target) {
			if (target.Volatiles["substitute"]) {
				this.add('-fail', target, 'move = Substitute');
				return null;
			}
			if (target.Hp <= target.Maxhp / 4 || target.Maxhp === 1) { // Shedinja clause
				this.add('-fail', target, 'move = Substitute', '[weak]');
				return null;
			}
		},
		onHit = function (target) {
			this.directDamage(target.Maxhp / 4);
		},
		effect = {
			onStart = function (target) {
				this.add('-start', target, "Substitute");
				this.effectData.hp = Math.floor(target.Maxhp / 4);
				delete target.Volatiles["partiallytrapped"];
			},
			onTryPrimaryHitPriority = -1,
			onTryPrimaryHit = function (target, source, move) {
				if (target === source || move.flags["authentic"] || move.infiltrates) {
					return;
				}
				let damage = this.getDamage(source, target, move);
				if (!damage && damage !== 0) {
					this.add('-fail', target);
					return null;
				}
				damage = this.runEvent("SubDamage", target, source, move, damage);
				if (!damage) {
					return damage;
				}
				if (damage > target.Volatiles["substitute"].hp) {
					damage = target.Volatiles["substitute"].hp;
				}
				target.Volatiles["substitute"].hp -= damage;
				source.lastDamage = damage;
				if (target.Volatiles["substitute"].hp <= 0) {
					target.RemoveVolatile("substitute");
				} else {
					this.add('-activate', target, 'move = Substitute', '[damage]');
				}
				if (move.recoil) {
					this.damage(this.calcRecoilDamage(damage, move), source, target, "recoil");
				}
				if (move.drain) {
					this.heal(Math.ceil(damage * move.drain[0] / move.drain[1]), source, target, "drain");
				}
				this.singleEvent("AfterSubDamage", move, null, target, source, move);
				this.runEvent("AfterSubDamage", target, source, move, damage);
				return 0; // hit
			},
			onEnd = function (target) {
				this.add('-end', target, "Substitute");
			}
		},
		//secondary = false,
		target = Target.Self,
		type = Types.NORMAL,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 650,
		//accuracy = true,
		basePower = 1,
		category = Category.PHYSICAL,
		//shortDesc = "Power is equal to the base move's Z-Power.",
		id = Moves.SUBZEROSLAMMER,
		isViable = true,
		//name = "Subzero Slammer",
		pp = 1,
		priority = 0,
		flags = new Flags (),
		isZ = "iciumz",
		//secondary = false,
		target = Target.Normal,
		type = Types.ICE,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 389,
		accuracy = 100,
		basePower = 70,
		category = Category.PHYSICAL,
		//desc = "Fails if the target Did not select a physical attack, special attack, or Me First for use this turn, or if the target Moves BEFORE the user.",
		//shortDesc = "Usually goes first. Fails if target Is not attacking.",
		id = Moves.SUCKERPUNCH,
		isViable = true,
		//name = "Sucker Punch",
		pp = 5,
		priority = 1,
		flags = new Flags (contact: true, protect: true, mirror: true),
		onTry = function (source, target) {
			let action = this.willMove(target);
			if (!action || action.choice !== "move" || (action.move.category === "Status" && action.move.id !== "mefirst") || target.Volatiles.mustrecharge) {
				this.attrLastMove('[still]');
				this.add('-fail', source);
				return null;
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.DARK,
		zMovePower = 140,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 241,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "For 5 turns, the weather becomes Sunny Day. The damage of Fire-type attacks is multiplied by 1.5 and the damage of Water-type attacks is multiplied by 0.5 during the effect. Lasts for 8 turns if the user is holding Heat Rock. Fails if the current weather is Sunny Day.",
		//shortDesc = "For 5 turns, intense sunlight powers Fire moves.",
		id = Moves.SUNNYDAY,
		//name = "Sunny Day",
		pp = 5,
		priority = 0,
		flags = new Flags (),
		weather = "sunnyday",
		//secondary = false,
		target = Target.All,
		type = Types.FIRE,
		zMoveBoost = new Boosts() {spe = 1},
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 713,
		accuracy = 100,
		basePower = 100,
		category = Category.PHYSICAL,
		//desc = "This move and its effects ignore the Abilities of other Pokemon.",
		//shortDesc = "Ignores the Abilities of other Pokemon.",
		id = Moves.SUNSTEELSTRIKE,
		isViable = true,
		//name = "Sunsteel Strike",
		pp = 5,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		ignoreAbility = true,
		//secondary = false,
		target = Target.Normal,
		type = Types.STEEL,
		zMovePower = 180,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 162,
		accuracy = 90,
		basePower = 0,
		damageCallback = function (pokemon, target) {
			return this.clampIntRange(Math.floor(target.Hp / 2), 1);
		},
		category = Category.PHYSICAL,
		//desc = "Deals damage to the target Equal to half of its current HP, rounded down, but not less than 1 HP.",
		//shortDesc = "Does damage equal to 1/2 target'S current HP.",
		id = Moves.SUPERFANG,
		isViable = true,
		//name = "Super Fang",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 100,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 276,
		accuracy = 100,
		basePower = 120,
		category = Category.PHYSICAL,
		//desc = "Lowers the user's Attack and Defense by 1 stage.",
		//shortDesc = "Lowers the user's Attack and Defense by 1.",
		id = Moves.SUPERPOWER,
		isViable = true,
		//name = "Superpower",
		pp = 5,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		self = {
			boosts = new Boosts() {
				atk = -1,
				def = -1
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.FIGHTING,
		zMovePower = 190,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 48,
		accuracy = 55,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Causes the target To become confused.",
		//shortDesc = "Confuses the target.",
		id = Moves.SUPERSONIC,
		//name = "Supersonic",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true, sound: true, authentic: true),
		volatileStatus = "confusion",
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {spe = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 626,
		//accuracy = true,
		basePower = 1,
		category = Category.PHYSICAL,
		//shortDesc = "Power is equal to the base move's Z-Power.",
		id = Moves.SUPERSONICSKYSTRIKE,
		isViable = true,
		//name = "Supersonic Skystrike",
		pp = 1,
		priority = 0,
		flags = new Flags (),
		isZ = "flyiniumz",
		//secondary = false,
		target = Target.Normal,
		type = Types.FLYING,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 57,
		accuracy = 100,
		basePower = 90,
		category = Category.SPECIAL,
		//desc = "Damage doubles if the target Is using Dive.",
		//shortDesc = "Hits adjacent Pokemon. Power doubles on Dive.",
		id = Moves.SURF,
		isViable = true,
		//name = "Surf",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, nonsky: true),
		//secondary = false,
		target = Target.AllAdjacent,
		type = Types.WATER,
		zMovePower = 175,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 207,
		accuracy = 85,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the target'S Attack by 2 stages and confuses it.",
		//shortDesc = "Raises the target'S Attack by 2 and confuses it.",
		id = Moves.SWAGGER,
		//name = "Swagger",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true, mystery: true),
		volatileStatus = "confusion",
		boosts = new Boosts() {
			atk = 2
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 256,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user restores its HP based on its Stockpile count. Restores 1/4 of its maximum HP if it's 1, 1/2 of its maximum HP if it's 2, both rounded half down, and all of its HP if it's 3. Fails if the user's Stockpile count is 0. The user's Defense and Special Defense decrease by as many stages as Stockpile had increased them, and the user's Stockpile count resets to 0.",
		//shortDesc = "Heals the user based on uses of Stockpile.",
		id = Moves.SWALLOW,
		//name = "Swallow",
		pp = 10,
		priority = 0,
		flags = new Flags (snatch: true, heal: true),
		onTryHit = function (pokemon) {
			if (!pokemon.volatiles["stockpile"] || !pokemon.volatiles["stockpile"].layers) return false;
		},
		onHit = function (pokemon) {
			let healAmount = [0.25, 0.5, 1];
			let healedBy = this.heal(this.modify(pokemon.maxhp, healAmount[(pokemon.volatiles["stockpile"].layers - 1)]));
			pokemon.removeVolatile("stockpile");
			return healedBy;
		},
		//secondary = false,
		target = Target.Self,
		type = Types.NORMAL,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 186,
		accuracy = 75,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Causes the target To become confused.",
		//shortDesc = "Confuses the target.",
		id = Moves.SWEETKISS,
		//name = "Sweet Kiss",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true),
		volatileStatus = "confusion",
		//secondary = false,
		target = Target.Normal,
		type = Types.FAIRY,
		zMoveBoost = new Boosts() {spa = 1},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 230,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Lowers the target'S evasiveness by 2 stages.",
		//shortDesc = "Lowers the foe(s) evasiveness by 2.",
		id = Moves.SWEETSCENT,
		//name = "Sweet Scent",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true),
		boosts = new Boosts() {
			evasion = -2
		},
		//secondary = false,
		target = Target.AllAdjacentFoes,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {accuracy = 1},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 129,
		//accuracy = true,
		basePower = 60,
		category = Category.SPECIAL,
		//desc = "This move does not check accuracy.",
		//shortDesc = "This move does not check accuracy. Hits foes.",
		id = Moves.SWIFT,
		//name = "Swift",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.AllAdjacentFoes,
		type = Types.NORMAL,
		zMovePower = 120,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 415,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user swaps its held item with the target'S held item. Fails if either the user or the target Is holding a Mail or Z-Crystal, if neither is holding an item, if the user is trying to give or take a Mega Stone to or from the species that can Mega Evolve with it, or if the user is trying to give or take a Blue Orb, a Red Orb, a Griseous Orb, a Plate, a Drive, or a Memory to or from a Kyogre, a Groudon, a Giratina, an Arceus, a Genesect, or a Silvally, respectively. Pokemon with the Ability Sticky Hold are immune.",
		//shortDesc = "User switches its held item with the target'S.",
		id = Moves.SWITCHEROO,
		isViable = true,
		//name = "Switcheroo",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, mystery: true),
		onTryHit = function (target) {
			if (target.HasAbility("stickyhold")) {
				this.add('-immune', target, '[msg]');
				return null;
			}
		},
		onHit = function (target, source, move) {
			let yourItem = target.TakeItem(source);
			let myItem = source.takeItem();
			if (target.Item || source.item || (!yourItem && !myItem)) {
				if (yourItem) target.Item = yourItem.id;
				if (myItem) source.item = myItem.id;
				return false;
			}
			if ((myItem && !this.singleEvent("TakeItem", myItem, source.itemData, target, source, move, myItem)) || (yourItem && !this.singleEvent("TakeItem", yourItem, target.ItemData, source, target, move, yourItem))) {
				if (yourItem) target.Item = yourItem.id;
				if (myItem) source.item = myItem.id;
				return false;
			}
			this.add('-activate', source, 'move = Trick', '[of] ' + target);
			if (myItem) {
				target.SetItem(myItem);
				this.add('-item', target, myItem, '[from] move = Switcheroo');
			} else {
				this.add('-enditem', target, yourItem, '[silent]', '[from] move = Switcheroo');
			}
			if (yourItem) {
				source.setItem(yourItem);
				this.add('-item', source, yourItem, '[from] move = Switcheroo');
			} else {
				this.add('-enditem', source, myItem, '[silent]', '[from] move = Switcheroo');
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.DARK,
		zMoveBoost = new Boosts() {spe = 2},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 14,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the user's Attack by 2 stages.",
		//shortDesc = "Raises the user's Attack by 2.",
		id = Moves.SWORDSDANCE,
		isViable = true,
		//name = "Swords Dance",
		pp = 20,
		priority = 0,
		flags = new Flags (snatch: true, dance: true),
		boosts = new Boosts() {
			atk = 2
		},
		//secondary = false,
		target = Target.Self,
		type = Types.NORMAL,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 485,
		accuracy = 100,
		basePower = 120,
		category = Category.SPECIAL,
		//desc = "The target Is immune if it does not share a type with the user.",
		//shortDesc = "Hits adjacent Pokemon sharing the user's type.",
		id = Moves.SYNCHRONOISE,
		//name = "Synchronoise",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		onTryHit = function (target, source) {
			if (!target.HasType(source.getTypes())) {
				this.add('-immune', target, '[msg]');
				return null;
			}
		},
		//secondary = false,
		target = Target.AllAdjacent,
		type = Types.PSYCHIC,
		zMovePower = 190,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 235,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user restores 1/2 of its maximum HP if no weather conditions are in effect, 2/3 of its maximum HP if the weather is Sunny Day, and 1/4 of its maximum HP if the weather is Hail, Rain Dance, or Sandstorm, all rounded half down.",
		//shortDesc = "Heals the user by a weather-dependent amount.",
		id = Moves.SYNTHESIS,
		isViable = true,
		//name = "Synthesis",
		pp = 5,
		priority = 0,
		flags = new Flags (snatch: true, heal: true),
		onHit = function (pokemon) {
			if (this.isWeather(["sunnyday", "desolateland"])) {
				return this.heal(this.modify(pokemon.maxhp, 0.667));
			} else if (this.isWeather(["raindance", "primordialsea", "sandstorm", "hail"])) {
				return this.heal(this.modify(pokemon.maxhp, 0.25));
			} else {
				return this.heal(this.modify(pokemon.maxhp, 0.5));
			}
		},
		//secondary = false,
		target = Target.Self,
		type = Types.GRASS,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 33,
		accuracy = 100,
		basePower = 40,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect.",
		id = Moves.TACKLE,
		//name = "Tackle",
		pp = 35,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 100,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 294,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the user's Special Attack by 3 stages.",
		//shortDesc = "Raises the user's Sp. Atk by 3.",
		id = Moves.TAILGLOW,
		isViable = true,
		//name = "Tail Glow",
		pp = 20,
		priority = 0,
		flags = new Flags (snatch: true),
		boosts = new Boosts() {
			spa = 3
		},
		//secondary = false,
		target = Target.Self,
		type = Types.BUG,
		zMoveEffect = "clearnegativeboost",
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 541,
		accuracy = 85,
		basePower = 25,
		category = Category.PHYSICAL,
		//desc = "Hits two to five times. Has a 1/3 chance to hit two or three times, and a 1/6 chance to hit four or five times. If one of the hits breaks the target'S substitute, it will take damage for the remaining hits. If the user has the Ability Skill Link, this move will always hit five times.",
		//shortDesc = "Hits 2-5 times in one turn.",
		id = Moves.TAILSLAP,
		isViable = true,
		//name = "Tail Slap",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		multihit = new int[] {2, 5},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 140,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 39,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Lowers the target'S Defense by 1 stage.",
		//shortDesc = "Lowers the foe(s) Defense by 1.",
		id = Moves.TAILWHIP,
		//name = "Tail Whip",
		pp = 30,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true),
		boosts = new Boosts() {
			def = -1
		},
		//secondary = false,
		target = Target.AllAdjacentFoes,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {atk = 1},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 366,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "For 4 turns, the user and its party members have their Speed doubled. Fails if this move is already in effect for the user's side.",
		//shortDesc = "For 4 turns, allies' Speed is doubled.",
		id = Moves.TAILWIND,
		isViable = true,
		//name = "Tailwind",
		pp = 15,
		priority = 0,
		flags = new Flags (snatch: true),
		sideCondition = "tailwind",
		effect = {
			duration = 4,
			durationCallback = function (target, source, effect) {
				if (source && source.hasAbility("persistent")) {
					this.add('-activate', source, 'ability = Persistent', effect);
					return 6;
				}
				return 4;
			},
			onStart = function (side) {
				this.add('-sidestart', side, 'move = Tailwind');
			},
			onModifySpe = function (spe, pokemon) {
				return this.chainModify(2);
			},
			onResidualOrder = 21,
			onResidualSubOrder = 4,
			onEnd = function (side) {
				this.add('-sideend', side, 'move = Tailwind');
			}
		},
		//secondary = false,
		target = Target.AllySide,
		type = Types.FLYING,
		zMoveEffect = "crit2",
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 36,
		accuracy = 85,
		basePower = 90,
		category = Category.PHYSICAL,
		//desc = "If the target Lost HP, the user takes recoil damage equal to 1/4 the HP lost by the target, rounded half up, but not less than 1 HP.",
		//shortDesc = "Has 1/4 recoil.",
		id = Moves.TAKEDOWN,
		//name = "Take Down",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		recoil = new int[] {1, 4},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 175,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 269,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Prevents the target From using non-damaging moves FOR its next three turns. Pokemon with the Ability Oblivious or protected by the Ability Aroma Veil are immune.",
		//shortDesc = "For 3 turns, the target Can't use status moves.",
		id = Moves.TAUNT,
		isViable = true,
		//name = "Taunt",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true, authentic: true),
		volatileStatus = "taunt",
		effect = {
			duration = 3,
			onStart = function (target) {
				if (target.ActiveTurns && !this.willMove(target)) {
					this.effectData.duration++;
				}
				this.add('-start', target, 'move = Taunt');
			},
			onResidualOrder = 12,
			onEnd = function (target) {
				this.add('-end', target, 'move = Taunt');
			},
			onDisableMove = function (pokemon) {
				for (const moveSlOT of pokemon.moveSlOTS) {
					if (this.getMove(moveSlOT.id).category === "Status") {
						pokemon.disableMove(moveSlOT.id);
					}
				}
			},
			onBeforeMovePriority = 5,
			onBeforeMove = function (attacker, defender, move) {
				if (!move.isZ && move.category === "Status") {
					this.add("cant", attacker, 'move = Taunt', move);
					return false;
				}
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.DARK,
		zMoveBoost = new Boosts() {atk = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 715,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Lowers the target'S Attack and Special Attack by 1 stage.",
		//shortDesc = "Lowers the target'S Attack and Sp. Atk by 1.",
		id = Moves.TEARFULLOOK,
		//name = "Tearful Look",
		pp = 20,
		priority = 0,
		flags = new Flags (reflectable: true, mirror: true),
		boosts = new Boosts() {
			atk = -1,
			spa = -1
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {def = 1},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 546,
		accuracy = 100,
		basePower = 120,
		category = Category.SPECIAL,
		//desc = "This move's type depends on the user's held Drive.",
		//shortDesc = "Type varies based on the held Drive.",
		id = Moves.TECHNOBLAST,
		isViable = true,
		//name = "Techno Blast",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		onModifyMove = function (move, pokemon) {
			move.type = this.runEvent("Drive", pokemon, null, "technoblast", "Normal");
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 190,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 630,
		//accuracy = true,
		basePower = 1,
		category = Category.PHYSICAL,
		//shortDesc = "Power is equal to the base move's Z-Power.",
		id = Moves.TECTONICRAGE,
		isViable = true,
		//name = "Tectonic Rage",
		pp = 1,
		priority = 0,
		flags = new Flags (),
		isZ = "groundiumz",
		//secondary = false,
		target = Target.Normal,
		type = Types.GROUND,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 298,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Causes the target To become confused.",
		//shortDesc = "Confuses adjacent Pokemon.",
		id = Moves.TEETERDANCE,
		//name = "Teeter Dance",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, dance: true),
		volatileStatus = "confusion",
		//secondary = false,
		target = Target.AllAdjacent,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {spa = 1},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 477,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "For 3 turns, the target Cannot avoid any attacks made against it, other than OHKO moves, as long as it remains active. During the effect, the target Is immune to Ground-type attacks and the effects of Spikes, Toxic Spikes, Sticky Web, and the Ability Arena Trap as long as it remains active. If the target Uses Baton Pass, the replacement will gain the effect. Ingrain, Smack Down, Thousand Arrows, and Iron Ball override this move if the target Is under any of their effects. Fails if the target Is already under this effect or the effects of Ingrain, Smack Down, or Thousand Arrows. The target Is immune to this move on use if its species is Diglett, Dugtrio, Alolan Diglett, Alolan Dugtrio, Sandygast, Palossand, or Gengar while Mega-Evolved. Mega Gengar cannot be under this effect by any means.",
		//shortDesc = "For 3 turns, target Floats but moves CAN't miss it.",
		id = Moves.TELEKINESIS,
		//name = "Telekinesis",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true, gravity: true, mystery: true),
		volatileStatus = "telekinesis",
		effect = {
			duration = 3,
			onStart = function (target) {
				if (["Diglett", "Dugtrio", "Palossand", "Sandygast"].includes(target.BaseTemplate.baseSpecies) ||
						target.BaseTemplate.species === 'Gengar-Mega') {
					this.add('-immune', target, '[msg]');
					return null;
				}
				if (target.Volatiles["smackdown"] || target.Volatiles["ingrain"]) return false;
				this.add('-start', target, "Telekinesis");
			},
			onAccuracyPriority = -1,
			on//Accuracy = function (accuracy, target, source, move) {
				if (move && !move.ohko) return true;
			},
			onImmunity = function (type) {
				if (type === "Ground") return false;
			},
			onUpdate = function (pokemon) {
				if (pokemon.baseTemplate.species === 'Gengar-Mega') {
					delete pokemon.volatiles["telekinesis"];
					this.add('-end', pokemon, "Telekinesis", '[silent]');
				}
			},
			onResidualOrder = 16,
			onEnd = function (target) {
				this.add('-end', target, "Telekinesis");
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.PSYCHIC,
		zMoveBoost = new Boosts() {spa = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 100,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Fails when used.",
		//shortDesc = "Flee from wild Pokemon battles.",
		id = Moves.TELEPORT,
		//name = "Teleport",
		pp = 20,
		priority = 0,
		flags = new Flags (),
		onTryHit = false,
		//secondary = false,
		target = Target.Self,
		type = Types.PSYCHIC,
		zMoveEffect = "heal",
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 168,
		accuracy = 100,
		basePower = 60,
		category = Category.PHYSICAL,
		//desc = "If this attack was successful and the user has not fainted, it steals the target'S held item if the user is not holding one. The target'S item is not stolen if it is a Mail or Z-Crystal, or if the target Is a Kyogre holding a Blue Orb, a Groudon holding a Red Orb, a Giratina holding a Griseous Orb, an Arceus holding a Plate, a Genesect holding a Drive, a Silvally holding a Memory, or a Pokemon that can Mega Evolve holding the Mega Stone for its species. Items lost to this move cannot be regained with Recycle or the Ability Harvest.",
		//shortDesc = "If the user has no item, it steals the target'S.",
		id = Moves.THIEF,
		//name = "Thief",
		pp = 25,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		onAfterHit = function (target, source, move) {
			if (source.item || source.volatiles["gem"]) {
				return;
			}
			let yourItem = target.TakeItem(source);
			if (!yourItem) {
				return;
			}
			if (!this.singleEvent("TakeItem", yourItem, target.ItemData, source, target, move, yourItem) || !source.setItem(yourItem)) {
				target.Item = yourItem.id; // bypass setItem so we don't break choicelock or anything
				return;
			}
			this.add('-item', source, yourItem, '[from] move = Thief', '[of] ' + target);
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.DARK,
		zMovePower = 120,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 614,
		accuracy = 100,
		basePower = 90,
		category = Category.PHYSICAL,
		//desc = "This move can hit airborne Pokemon, which includes Flying-type Pokemon, Pokemon with the Ability Levitate, Pokemon holding an Air Balloon, and Pokemon under the effect of Magnet Rise or Telekinesis. If the target Is a Flying type and is not already grounded, this move deals neutral damage regardless of its other type(s). This move can hit a target Using Bounce, Fly, or Sky Drop. If this move hits a target Under the effect of Bounce, Fly, Magnet Rise, or Telekinesis, the effect ends. If the target Is a Flying type that has not used Roost this turn or a Pokemon with the Ability Levitate, it loses its immunity to Ground-type attacks and the Ability Arena Trap as long as it remains active. During the effect, Magnet Rise fails for the target And Telekinesis fails against the target.",
		//shortDesc = "Grounds adjacent foes. First hit neutral on Flying.",
		id = Moves.THOUSANDARROWS,
		isViable = true,
		//name = "Thousand Arrows",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, nonsky: true),
		onEffectiveness = function (typeMod, type, move) {
			// @ts-ignore
			if (move.type !== "Ground") return;
			let target = this.activeTarget;
			if (!target) return; // avoid crashing when called from a chat plugin
			// ignore effectiveness if the target Is Flying type and immune to Ground
			if (!target.RunImmunity("Ground")) {
				if (target.HasType("Flying")) return 0;
			}
		},
		volatileStatus = "smackdown",
		ignoreImmunity = {"Ground" = true},
		//secondary = false,
		target = Target.AllAdjacentFoes,
		type = Types.GROUND,
		zMovePower = 180,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 615,
		accuracy = 100,
		basePower = 90,
		category = Category.PHYSICAL,
		//desc = "Prevents the target From switching out. The target Can still switch out if it is holding Shed Shell or uses Baton Pass, Parting Shot, U-turn, or Volt Switch. If the target Leaves the field using Baton Pass, the replacement will remain trapped. The effect ends if the user leaves the field.",
		//shortDesc = "Hits adjacent foes. Prevents them from switching.",
		id = Moves.THOUSANDWAVES,
		//name = "Thousand Waves",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, nonsky: true),
		onHit = function (target, source, move) {
			if (source.isActive) target.AddVolatile("trapped", source, move, "trapper");
		},
		//secondary = false,
		target = Target.AllAdjacentFoes,
		type = Types.GROUND,
		zMovePower = 175,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 37,
		accuracy = 100,
		basePower = 120,
		category = Category.PHYSICAL,
		//desc = "Deals damage to one adjacent foe at random. The user spends two or three turns locked into this move and becomes confused after the last turn of the effect if it is not already. If the user is prevented from moving or the attack is not successful against the target On the first turn of the effect or the second turn of a three-turn effect, the effect ends without causing confusion. If this move is called by Sleep Talk, the move is used for one turn and does not confuse the user.",
		//shortDesc = "Lasts 2-3 turns. Confuses the user afterwards.",
		id = Moves.THRASH,
		//name = "Thrash",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		self = {
			volatileStatus = "lockedmove"
		},
		onAfterMove = function (pokemon) {
			if (pokemon.volatiles["lockedmove"] && pokemon.volatiles["lockedmove"].duration === 1) {
				pokemon.removeVolatile("lockedmove");
			}
		},
		//secondary = false,
		target = Target.RandomNormal,
		type = Types.NORMAL,
		zMovePower = 190,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 675,
		accuracy = 100,
		basePower = 80,
		category = Category.PHYSICAL,
		//desc = "For 2 turns, the target Cannot use sound-based moves.",
		//shortDesc = "For 2 turns, the target Cannot use sound moves.",
		id = Moves.THROATCHOP,
		//name = "Throat Chop",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		effect = {
			duration = 2,
			onStart = function (target) {
				this.add('-start', target, 'Throat Chop', '[silent]');
			},
			onDisableMove = function (pokemon) {
				for (const moveSlOT of pokemon.moveSlOTS) {
					if (this.getMove(moveSlOT.id).flags["sound"]) {
						pokemon.disableMove(moveSlOT.id);
					}
				}
			},
			onBeforeMovePriority = 6,
			onBeforeMove = function (pokemon, target, move) {
				if (!move.isZ && move.flags["sound"]) {
					this.add("cant", pokemon, 'move = Throat Chop');
					return false;
				}
			},
			onResidualOrder = 22,
			onEnd = function (target) {
				this.add('-end', target, 'Throat Chop', '[silent]');
			}
		},
		secondary = new Secondary() {
			chance = 100,
			onHit = function (target) {
				target.AddVolatile("throatchop");
			}
		},
		target = Target.Normal,
		type = Types.DARK,
		zMovePower = 160,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 87,
		accuracy = 70,
		basePower = 110,
		category = Category.SPECIAL,
		//desc = "Has a 30% chance to paralyze the target. This move can hit a target Using Bounce, Fly, or Sky Drop. If the weather is Rain Dance, this move does not check accuracy. If the weather is Sunny Day, this move's accuracy is 50%.",
		//shortDesc = "30% chance to paralyze target. Can't miss in rain.",
		id = Moves.THUNDER,
		isViable = true,
		//name = "Thunder",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		onModifyMove = function (move) {
			if (this.isWeather(["raindance", "primordialsea"])) {
				move.//accuracy = true;
			} else if (this.isWeather(["sunnyday", "desolateland"])) {
				move.accuracy = 50;
			}
		},
		secondary = new Secondary() {
			chance = 30,
			status = "par"
		},
		target = Target.Normal,
		type = Types.ELECTRIC,
		zMovePower = 185,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 422,
		accuracy = 95,
		basePower = 65,
		category = Category.PHYSICAL,
		//desc = "Has a 10% chance to paralyze the target And a 10% chance to flinch it.",
		//shortDesc = "10% chance to paralyze. 10% chance to flinch.",
		id = Moves.THUNDERFANG,
		//name = "Thunder Fang",
		pp = 15,
		priority = 0,
		flags = new Flags (bite: true, contact: true, protect: true, mirror: true),
		secondaries = [
			{
				chance = 10,
				status = "par"
			}, {
				chance = 10,
				volatileStatus = "flinch"
			},
		],
		target = Target.Normal,
		type = Types.ELECTRIC,
		zMovePower = 120,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 9,
		accuracy = 100,
		basePower = 75,
		category = Category.PHYSICAL,
		//desc = "Has a 10% chance to paralyze the target.",
		//shortDesc = "10% chance to paralyze the target.",
		id = Moves.THUNDERPUNCH,
		isViable = true,
		//name = "Thunder Punch",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true, punch: true),
		secondary = new Secondary() {
			chance = 10,
			status = "par"
		},
		target = Target.Normal,
		type = Types.ELECTRIC,
		zMovePower = 140,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 84,
		accuracy = 100,
		basePower = 40,
		category = Category.SPECIAL,
		//desc = "Has a 10% chance to paralyze the target.",
		//shortDesc = "10% chance to paralyze the target.",
		id = Moves.THUNDERSHOCK,
		//name = "Thunder Shock",
		pp = 30,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 10,
			status = "par"
		},
		target = Target.Normal,
		type = Types.ELECTRIC,
		zMovePower = 100,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 86,
		accuracy = 90,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Paralyzes the target.",
		//shortDesc = "Paralyzes the target.",
		id = Moves.THUNDERWAVE,
		isViable = true,
		//name = "Thunder Wave",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true),
		status = "par",
		ignoreImmunity = false,
		//secondary = false,
		target = Target.Normal,
		type = Types.ELECTRIC,
		zMoveBoost = new Boosts() {spd = 1},
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 85,
		accuracy = 100,
		basePower = 90,
		category = Category.SPECIAL,
		//desc = "Has a 10% chance to paralyze the target.",
		//shortDesc = "10% chance to paralyze the target.",
		id = Moves.THUNDERBOLT,
		isViable = true,
		//name = "Thunderbolt",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 10,
			status = "par"
		},
		target = Target.Normal,
		type = Types.ELECTRIC,
		zMovePower = 175,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 321,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Lowers the target'S Attack and Defense by 1 stage.",
		//shortDesc = "Lowers the target'S Attack and Defense by 1.",
		id = Moves.TICKLE,
		//name = "Tickle",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true, mystery: true),
		boosts = new Boosts() {
			atk = -1,
			def = -1
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {def = 1},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 576,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The target'S positive stat stages become negative and vice versa. Fails if all of the target'S stat stages are 0.",
		//shortDesc = "Inverts the target'S stat stages.",
		id = Moves.TOPSYTURVY,
		//name = "Topsy-Turvy",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true, mystery: true),
		onHit = function (target) {
			let success = false;
			for (let i in target.Boosts) {
				if (target.Boosts[i] === 0) continue;
				target.Boosts[i] = -target.Boosts[i];
				success = true;
			}
			if (!success) return false;
			this.add('-invertboost', target, '[from] move = Topsy-Turvy');
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.DARK,
		zMoveBoost = new Boosts() {atk = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 259,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Prevents the target From selecting the same move for use two turns in a row. This effect lasts until the target Leaves the field.",
		//shortDesc = "Target Can't select the same move twice in a row.",
		id = Moves.TORMENT,
		//name = "Torment",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true, authentic: true),
		volatileStatus = "torment",
		effect = {
			noCopy = true,
			onStart = function (pokemon) {
				this.add('-start', pokemon, "Torment");
			},
			onEnd = function (pokemon) {
				this.add('-end', pokemon, "Torment");
			},
			onDisableMove = function (pokemon) {
				if (pokemon.lastMove && pokemon.lastMove.id !== "struggle") pokemon.disableMove(pokemon.lastMove.id);
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.DARK,
		zMoveBoost = new Boosts() {def = 1},
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 92,
		accuracy = 90,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Badly poisons the target. If a Poison-type Pokemon uses this move, the target Cannot avoid the attack, even if the target Is in the middle of a two-turn move.",
		//shortDesc = "Badly poisons the target.",
		id = Moves.TOXIC,
		isViable = true,
		//name = "Toxic",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true),
		// No Guard-like effect for Poison-type users implemented in BattleScripts#tryMoveHit
		status = "tox",
		//secondary = false,
		target = Target.Normal,
		type = Types.POISON,
		zMoveBoost = new Boosts() {def = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 390,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Sets up a hazard on the foe's side of the field, poisoning each foe that switches in, unless it is a Flying-type Pokemon or has the Ability Levitate. Can be used up to two times before failing. Foes become poisoned with one layer and badly poisoned with two layers. Can be removed from the foe's side if any foe uses Rapid Spin or Defog, is hit by Defog, or a grounded Poison-type Pokemon switches in. Safeguard prevents the foe's party from being poisoned on switch-in, but a substitute does not.",
		//shortDesc = "Poisons grounded foes on switch-in. Max 2 layers.",
		id = Moves.TOXICSPIKES,
		isViable = true,
		//name = "Toxic Spikes",
		pp = 20,
		priority = 0,
		flags = new Flags (reflectable: true, nonsky: true),
		sideCondition = "toxicspikes",
		effect = {
			// this is a side condition
			onStart = function (side) {
				this.add('-sidestart', side, 'move = Toxic Spikes');
				this.effectData.layers = 1;
			},
			onRestart = function (side) {
				if (this.effectData.layers >= 2) return false;
				this.add('-sidestart', side, 'move = Toxic Spikes');
				this.effectData.layers++;
			},
			onSwitchIn = function (pokemon) {
				if (!pokemon.isGrounded()) return;
				if (!pokemon.runImmunity("Poison")) return;
				if (pokemon.hasType("Poison")) {
					this.add('-sideend', pokemon.side, 'move = Toxic Spikes', '[of] ' + pokemon);
					pokemon.side.removeSiDECONDITION("toxicspikes");
				} else if (this.effectData.layers >= 2) {
					pokemon.trySetStatus("tox", pokemon.side.foe.active[0]);
				} else {
					pokemon.trySetStatus("psn", pokemon.side.foe.active[0]);
				}
			}
		},
		//secondary = false,
		target = Target.FoeSide,
		type = Types.POISON,
		zMoveBoost = new Boosts() {def = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 672,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Lowers the target'S Speed by 1 stage and poisons it.",
		//shortDesc = "Lowers the target'S Speed by 1 and poisons it.",
		id = Moves.TOXICTHREAD,
		isViable = true,
		//name = "Toxic Thread",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true),
		status = "psn",
		boosts = new Boosts() {
			spe = -1
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.POISON,
		zMoveBoost = new Boosts() {spe = 1},
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 144,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user transforms into the target. The target'S current stats, stat stages, types, moves, Ability, weight, gender, and sprite are copied. The user's level and HP remain the same and each copied move receives only 5 PP, with a maximum of 5 PP each. The user can no longer change formes if it would have the ability to do so. This move fails if the target Has a substitute, if either the user or the target Is already transformed, or if either is behind an Illusion.",
		//shortDesc = "Copies target'S stats, moves, types, and Ability.",
		id = Moves.TRANSFORM,
		//name = "Transform",
		pp = 10,
		priority = 0,
		flags = new Flags (mystery: true),
		onHit = function (target, pokemon) {
			if (!pokemon.transformInto(target, pokemon)) {
				return false;
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMoveEffect = "heal",
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 161,
		accuracy = 100,
		basePower = 80,
		category = Category.SPECIAL,
		//desc = "Has a 20% chance to either burn, freeze, or paralyze the target.",
		//shortDesc = "20% chance to paralyze or burn or freeze target.",
		id = Moves.TRIATTACK,
		isViable = true,
		//name = "Tri Attack",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 20,
			onHit = function (target, source) {
				let result = this.random(3);
				if (result === 0) {
					target.TrySetStatus("brn", source);
				} else if (result === 1) {
					target.TrySetStatus("par", source);
				} else {
					target.TrySetStatus("frz", source);
				}
			}
		},
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 160,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 271,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user swaps its held item with the target'S held item. Fails if either the user or the target Is holding a Mail or Z-Crystal, if neither is holding an item, if the user is trying to give or take a Mega Stone to or from the species that can Mega Evolve with it, or if the user is trying to give or take a Blue Orb, a Red Orb, a Griseous Orb, a Plate, a Drive, or a Memory to or from a Kyogre, a Groudon, a Giratina, an Arceus, a Genesect, or a Silvally, respectively. Pokemon with the Ability Sticky Hold are immune.",
		//shortDesc = "User switches its held item with the target'S.",
		id = Moves.TRICK,
		isViable = true,
		//name = "Trick",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, mystery: true),
		onTryHit = function (target) {
			if (target.HasAbility("stickyhold")) {
				this.add('-immune', target, '[msg]');
				return null;
			}
		},
		onHit = function (target, source, move) {
			let yourItem = target.TakeItem(source);
			let myItem = source.takeItem();
			if (target.Item || source.item || (!yourItem && !myItem)) {
				if (yourItem) target.Item = yourItem.id;
				if (myItem) source.item = myItem.id;
				return false;
			}
			if ((myItem && !this.singleEvent("TakeItem", myItem, source.itemData, target, source, move, myItem)) || (yourItem && !this.singleEvent("TakeItem", yourItem, target.ItemData, source, target, move, yourItem))) {
				if (yourItem) target.Item = yourItem.id;
				if (myItem) source.item = myItem.id;
				return false;
			}
			this.add('-activate', source, 'move = Trick', '[of] ' + target);
			if (myItem) {
				target.SetItem(myItem);
				this.add('-item', target, myItem, '[from] move = Trick');
			} else {
				this.add('-enditem', target, yourItem, '[silent]', '[from] move = Trick');
			}
			if (yourItem) {
				source.setItem(yourItem);
				this.add('-item', source, yourItem, '[from] move = Trick');
			} else {
				this.add('-enditem', source, myItem, '[silent]', '[from] move = Trick');
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.PSYCHIC,
		zMoveBoost = new Boosts() {spe = 2},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 567,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Causes the Ghost type to be added to the target, effectively making it have two or three types. Fails if the target Is already a Ghost type. If Forest's Curse adds a type to the target, it replaces the type added by this move and vice versa.",
		//shortDesc = "Adds Ghost to the target'S type(s).",
		id = Moves.TRICKORTREAT,
		//name = "Trick-or-Treat",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true, mystery: true),
		onHit = function (target) {
			if (target.HasType("Ghost")) return false;
			if (!target.AddType("Ghost")) return false;
			this.add('-start', target, "typeadd", "Ghost", '[from] move = Trick-or-Treat');

			if (target.Side.active.length === 2 && target.Position === 1) {
				// Curse Glitch
				const action = this.willMove(target);
				if (action && action.move.id === "curse") {
					action.targetLOc = -1;
				}
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.GHOST,
		zMoveBoost = new Boosts() {atk = 1, def = 1, spa = 1, spd = 1, spe = 1},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 433,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "For 5 turns, all active Pokemon with lower Speed will move before those with higher Speed, within their priority brackets. If this move is used during the effect, the effect ends.",
		//shortDesc = "For 5 turns, slower Pokemon move first.",
		id = Moves.TRICKROOM,
		//name = "Trick Room",
		pp = 5,
		priority = -7,
		flags = new Flags (mirror: true),
		pseudoWeather = "trickroom",
		effect = {
			duration = 5,
			durationCallback = function (source, effect) {
				if (source && source.hasAbility("persistent")) {
					this.add('-activate', source, 'ability = Persistent', effect);
					return 7;
				}
				return 5;
			},
			onStart = function (target, source) {
				this.add('-fieldstart', 'move = Trick Room', '[of] ' + source);
			},
			onRestart = function (target, source) {
				this.removePseudoWeather("trickroom");
			},
			// Speed modification is changed in Pokemon.getActionSpeed() in sim/pokemon.js
			onResidualOrder = 23,
			onEnd = function () {
				this.add('-fieldend', 'move = Trick Room');
			}
		},
		//secondary = false,
		target = Target.All,
		type = Types.PSYCHIC,
		zMoveBoost = new Boosts() {accuracy = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 167,
		accuracy = 90,
		basePower = 10,
		basePowerCallback = function (pokemon, target, move) {
			if (move.hit) {
				move.hit++;
			} else {
				move.hit = 1;
			}
			return 10 * move.hit;
		},
		category = Category.PHYSICAL,
		//desc = "Hits three times. Power increases to 20 for the second hit and 30 for the third. This move checks accuracy for each hit, and the attack ends if the target Avoids any of the hits. If one of the hits breaks the target'S substitute, it will take damage for the remaining hits. If the user has the Ability Skill Link, this move will always hit three times.",
		//shortDesc = "Hits 3 times. Each hit can miss, but power rises.",
		id = Moves.TRIPLEKICK,
		//name = "Triple Kick",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		multihit = 3,
		multi//accuracy = true,
		//secondary = false,
		target = Target.Normal,
		type = Types.FIGHTING,
		zMovePower = 120,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 688,
		accuracy = 100,
		basePower = 70,
		category = Category.PHYSICAL,
		//desc = "Has a 100% chance to lower the target'S Attack by 1 stage.",
		//shortDesc = "100% chance to lower the target'S Attack by 1.",
		id = Moves.TROPKICK,
		//name = "Trop Kick",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 100,
			boosts = new Boosts() {
				atk = -1
			}
		},
		target = Target.Normal,
		type = Types.GRASS,
		zMovePower = 140,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 376,
		//accuracy = true,
		basePower = 0,
		basePowerCallback = function (source, target, move) {
			const callerMoveId = move.sourceEffect || move.id;
			const moveSlOT = callerMoveId === "instruct" ? source.getMoveData(move.id)  = source.getMoveData(callerMoveId);
			// @ts-ignore
			switch (moveSlOT.pp) {
			case 0:
				return 200;
			case 1:
				return 80;
			case 2:
				return 60;
			case 3:
				return 50;
			default =
				return 40;
			}
		},
		category = Category.SPECIAL,
		//desc = "This move does not check accuracy. The power of this move is based on the amount of PP remaining after normal PP reduction and the Ability Pressure resolve. 200 power for 0 PP, 80 power for 1 PP, 60 power for 2 PP, 50 power for 3 PP, and 40 power for 4 or more PP.",
		//shortDesc = "More power the fewer PP this move has left.",
		id = Moves.TRUMPCARD,
		//name = "Trump Card",
		pp = 5,
		noPPBoosts = true,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 160,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 41,
		accuracy = 100,
		basePower = 25,
		category = Category.PHYSICAL,
		//desc = "Hits twice, with each hit having a 20% chance to poison the target. If the first hit breaks the target'S substitute, it will take damage for the second hit.",
		//shortDesc = "Hits 2 times. Each hit has 20% chance to poison.",
		id = Moves.TWINEEDLE,
		//name = "Twineedle",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		multihit = 2,
		secondary = new Secondary() {
			chance = 20,
			status = "psn"
		},
		target = Target.Normal,
		type = Types.BUG,
		zMovePower = 100,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 656,
		//accuracy = true,
		basePower = 1,
		category = Category.PHYSICAL,
		//shortDesc = "Power is equal to the base move's Z-Power.",
		id = Moves.TWINKLETACKLE,
		isViable = true,
		//name = "Twinkle Tackle",
		pp = 1,
		priority = 0,
		flags = new Flags (),
		isZ = "fairiumz",
		//secondary = false,
		target = Target.Normal,
		type = Types.FAIRY,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 239,
		accuracy = 100,
		basePower = 40,
		category = Category.SPECIAL,
		//desc = "Has a 20% chance to flinch the target. Damage doubles if the target Is using Bounce, Fly, or Sky Drop.",
		//shortDesc = "20% chance to flinch the foe(s).",
		id = Moves.TWISTER,
		//name = "Twister",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 20,
			volatileStatus = "flinch"
		},
		target = Target.AllAdjacentFoes,
		type = Types.DRAGON,
		zMovePower = 100,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 369,
		accuracy = 100,
		basePower = 70,
		category = Category.PHYSICAL,
		//desc = "If this move is successful and the user has not fainted, the user switches out even if it is trapped and is replaced immediately by a selected party member. The user does not switch out if there are no unfainted party members, or if the target Switched out using an Eject Button.",
		//shortDesc = "User switches out after damaging the target.",
		id = Moves.UTURN,
		isViable = true,
		//name = "U-turn",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		selfSwitch = true,
		//secondary = false,
		target = Target.Normal,
		type = Types.BUG,
		zMovePower = 140,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 253,
		accuracy = 100,
		basePower = 90,
		category = Category.SPECIAL,
		//desc = "Deals damage to one adjacent foe at random. The user spends three turns locked into this move. On the first of the three turns, all sleeping active Pokemon wake up. During the three turns, no active Pokemon can fall asleep by any means; Pokemon switched in during the effect do not wake up. If the user is prevented from moving or the attack is not successful against the target During one of the turns, the effect ends.",
		//shortDesc = "Lasts 3 turns. Active Pokemon cannot fall asleep.",
		id = Moves.UPROAR,
		//name = "Uproar",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, sound: true, authentic: true),
		self = {
			volatileStatus = "uproar"
		},
		onTryHit = function (target) {
			for (const [i, allyActive] of target.Side.active.entries()) {
				if (allyActive && allyActive.status === "slp") allyActive.cureStatus();
				let foeActive = target.Side.foe.active[i];
				if (foeActive && foeActive.status === "slp") foeActive.cureStatus();
			}
		},
		effect = {
			duration = 3,
			onStart = function (target) {
				this.add('-start', target, "Uproar");
			},
			onResidual = function (target) {
				if (target.LastMove && target.LastMove.id === "struggle") {
					// don't lock
					delete target.Volatiles["uproar"];
				}
				this.add('-start', target, "Uproar", '[upkeep]');
			},
			onEnd = function (target) {
				this.add('-end', target, "Uproar");
			},
			onLockMove = "uproar",
			onAnySetStatus = function (status, pokemon) {
				if (status.id === "slp") {
					if (pokemon === this.effectData.target) {
						this.add('-fail', pokemon, "slp", '[from] Uproar', '[msg]');
					} else {
						this.add('-fail', pokemon, "slp", '[from] Uproar');
					}
					return null;
				}
			}
		},
		//secondary = false,
		target = Target.RandomNormal,
		type = Types.NORMAL,
		zMovePower = 175,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 557,
		accuracy = 95,
		basePower = 180,
		category = Category.PHYSICAL,
		//desc = "Lowers the user's Speed, Defense, and Special Defense by 1 stage.",
		//shortDesc = "Lowers the user's Defense, Sp. Def, Speed by 1.",
		id = Moves.VCREATE,
		isViable = true,
		//name = "V-create",
		pp = 5,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		self = {
			boosts = new Boosts() {
				spe = -1,
				def = -1,
				spd = -1
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.FIRE,
		zMovePower = 220,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 410,
		accuracy = 100,
		basePower = 40,
		category = Category.SPECIAL,
		//desc = "No additional effect.",
		//shortDesc = "Usually goes first.",
		id = Moves.VACUUMWAVE,
		//name = "Vacuum Wave",
		pp = 30,
		priority = 1,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.FIGHTING,
		zMovePower = 100,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 599,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Lowers the target'S Attack, Special Attack, and Speed by 1 stage if the target Is poisoned. Fails if the target Is not poisoned.",
		//shortDesc = "Lowers Atk, Sp. Atk, Speed of poisoned foes by 1.",
		id = Moves.VENOMDRENCH,
		//name = "Venom Drench",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true),
		onHit = function (target, source, move) {
			if (target.Status === "psn" || target.Status === "tox") {
				return this.boost({atk = -1, spa = -1, spe = -1}, target, source, move);
			}
			return false;
		},
		//secondary = false,
		target = Target.AllAdjacentFoes,
		type = Types.POISON,
		zMoveBoost = new Boosts() {def = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 474,
		accuracy = 100,
		basePower = 65,
		category = Category.SPECIAL,
		//desc = "Power doubles if the target Is poisoned.",
		//shortDesc = "Power doubles if the target Is poisoned.",
		id = Moves.VENOSHOCK,
		//name = "Venoshock",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		onBasePowerPriority = 4,
		onBasePower = function (basePower, pokemon, target) {
			if (target.Status === "psn" || target.Status === "tox") {
				return this.chainModify(2);
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.POISON,
		zMovePower = 120,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 11,
		accuracy = 100,
		basePower = 55,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect.",
		id = Moves.VICEGRIP,
		//name = "Vice Grip",
		pp = 30,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 100,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 22,
		accuracy = 100,
		basePower = 45,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect.",
		id = Moves.VINEWHIP,
		//name = "Vine Whip",
		pp = 25,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.GRASS,
		zMovePower = 100,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 233,
		//accuracy = true,
		basePower = 70,
		category = Category.PHYSICAL,
		//desc = "This move does not check accuracy.",
		//shortDesc = "This move does not check accuracy. Goes last.",
		id = Moves.VITALTHROW,
		//name = "Vital Throw",
		pp = 10,
		priority = -1,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.FIGHTING,
		zMovePower = 140,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 521,
		accuracy = 100,
		basePower = 70,
		category = Category.SPECIAL,
		//desc = "If this move is successful and the user has not fainted, the user switches out even if it is trapped and is replaced immediately by a selected party member. The user does not switch out if there are no unfainted party members, or if the target Switched out using an Eject Button.",
		//shortDesc = "User switches out after damaging the target.",
		id = Moves.VOLTSWITCH,
		isViable = true,
		//name = "Volt Switch",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		selfSwitch = true,
		//secondary = false,
		target = Target.Normal,
		type = Types.ELECTRIC,
		zMovePower = 140,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 344,
		accuracy = 100,
		basePower = 120,
		category = Category.PHYSICAL,
		//desc = "Has a 10% chance to paralyze the target. If the target Lost HP, the user takes recoil damage equal to 33% the HP lost by the target, rounded half up, but not less than 1 HP.",
		//shortDesc = "Has 33% recoil. 10% chance to paralyze target.",
		id = Moves.VOLTTACKLE,
		isViable = true,
		//name = "Volt Tackle",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		recoil = new int[] {33, 100},
		secondary = new Secondary() {
			chance = 10,
			status = "par"
		},
		target = Target.Normal,
		type = Types.ELECTRIC,
		zMovePower = 190,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 358,
		accuracy = 100,
		basePower = 70,
		basePowerCallback = function (pokemon, target, move) {
			if (target.Status === "slp" || target.HasAbility("comatose")) return move.basePower * 2;
			return move.basePower;
		},
		category = Category.PHYSICAL,
		//desc = "Power doubles if the target Is asleep. If the user has not fainted, the target Wakes up.",
		//shortDesc = "Power doubles if target Is asleep, and wakes it.",
		id = Moves.WAKEUPSLAP,
		//name = "Wake-Up Slap",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		onHit = function (target) {
			if (target.Status === "slp") target.CureStatus();
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.FIGHTING,
		zMovePower = 140,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 55,
		accuracy = 100,
		basePower = 40,
		category = Category.SPECIAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect.",
		id = Moves.WATERGUN,
		//name = "Water Gun",
		pp = 25,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.WATER,
		zMovePower = 100,
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 518,
		accuracy = 100,
		basePower = 80,
		basePowerCallback = function (target, source, move) {
			if (["firepledge", "grasspledge"].includes(move.sourceEffect)) {
				this.add('-combine');
				return 150;
			}
			return 80;
		},
		category = Category.SPECIAL,
		//desc = "If one of the user's allies chose to use Fire Pledge or Grass Pledge this turn and has not moved yet, it takes its turn immediately after the user and the user's move does nothing. If combined with Fire Pledge, the ally uses Water Pledge with 150 Base Power and a rainbow appears on the user's side for 4 turns, which doubles secondary effect chances but does not stack with the Ability Serene Grace. If combined with Grass Pledge, the ally uses Grass Pledge with 150 Base Power and a swamp appears on the target'S side for 4 turns, which quarters the Speed of each Pokemon on that side. When used as a combined move, this move gains STAB no matter what the user's type is. This move does not consume the user's Water Gem, and cannot be redirected by the Ability Storm Drain.",
		//shortDesc = "Use with Grass or Fire Pledge for added effect.",
		id = Moves.WATERPLEDGE,
		//name = "Water Pledge",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, mirror: true, nonsky: true),
		onPrepareHit = function (target, source, move) {
			for (const action of this.queue) {
				// @ts-ignore
				if (!action.move || !action.pokemon || !action.pokemon.isActive || action.pokemon.fainted) continue;
				// @ts-ignore
				if (action.pokemon.side === source.side && ["firepledge", "grasspledge"].includes(action.move.id)) {
					// @ts-ignore
					this.prioritizeAction(action);
					this.add('-waiting', source, action.pokemon);
					return null;
				}
			}
		},
		onModifyMove = function (move) {
			if (move.sourceEffect === "grasspledge") {
				move.type = "Grass";
				move.hasSTAB = true;
			}
			if (move.sourceEffect === "firepledge") {
				move.type = "Water";
				move.hasSTAB = true;
			}
		},
		onHit = function (target, source, move) {
			if (move.sourceEffect === "firepledge") {
				source.side.addSideCondition("waterpledge");
			}
			if (move.sourceEffect === "grasspledge") {
				target.Side.addSideCondition("grasspledge");
			}
		},
		effect = {
			duration = 4,
			onStart = function (targetSIde) {
				this.add('-sidestart', targetSIde, 'Water Pledge');
			},
			onEnd = function (targetSIde) {
				this.add('-sideend', targetSIde, 'Water Pledge');
			},
			onModifyMove = function (move) {
				if (move.secondaries && move.id !== "secretpower") {
					this.debug('doubling secondary chance');
					for (const secondary of move.secondaries) {
						// @ts-ignore
						secondary.chance *= 2;
					}
				}
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.WATER,
		zMovePower = 160,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 352,
		accuracy = 100,
		basePower = 60,
		category = Category.SPECIAL,
		//desc = "Has a 20% chance to confuse the target.",
		//shortDesc = "20% chance to confuse the target.",
		id = Moves.WATERPULSE,
		//name = "Water Pulse",
		pp = 20,
		priority = 0,
		flags = new Flags (protect: true, pulse: true, mirror: true, distance: true),
		secondary = new Secondary() {
			chance = 20,
			volatileStatus = "confusion"
		},
		target = Target.Any,
		type = Types.WATER,
		zMovePower = 120,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 346,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "For 5 turns, all Fire-type attacks used by any active Pokemon have their power reduced to 0.33x. Fails if this move is already in effect.",
		//shortDesc = "For 5 turns, Fire-type attacks have 1/3 power.",
		id = Moves.WATERSPORT,
		//name = "Water Sport",
		pp = 15,
		priority = 0,
		flags = new Flags (nonsky: true),
		pseudoWeather = "watersport",
		effect = {
			duration = 5,
			onStart = function (side, source) {
				this.add('-fieldstart', 'move = Water Sport', '[of] ' + source);
			},
			onBasePowerPriority = 1,
			onBasePower = function (basePower, attacker, defender, move) {
				if (move.type === "Fire") {
					this.debug('water sport weaken');
					return this.chainModify([0x548, 0x1000]);
				}
			},
			onResidualOrder = 21,
			onEnd = function () {
				this.add('-fieldend', 'move = Water Sport');
			}
		},
		//secondary = false,
		target = Target.All,
		type = Types.WATER,
		zMoveBoost = new Boosts() {spd = 1},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 323,
		accuracy = 100,
		basePower = 150,
		basePowerCallback = function (pokemon, target, move) {
			return move.basePower * pokemon.hp / pokemon.maxhp;
		},
		category = Category.SPECIAL,
		//desc = "Power is equal to (user's current HP * 150 / user's maximum HP), rounded down, but not less than 1.",
		//shortDesc = "Less power as user's HP decreases. Hits foe(s).",
		id = Moves.WATERSPOUT,
		isViable = true,
		//name = "Water Spout",
		pp = 5,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		//secondary = false,
		target = Target.AllAdjacentFoes,
		type = Types.WATER,
		zMovePower = 200,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 127,
		accuracy = 100,
		basePower = 80,
		category = Category.PHYSICAL,
		//desc = "Has a 20% chance to flinch the target.",
		//shortDesc = "20% chance to flinch the target.",
		id = Moves.WATERFALL,
		isViable = true,
		//name = "Waterfall",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 20,
			volatileStatus = "flinch"
		},
		target = Target.Normal,
		type = Types.WATER,
		zMovePower = 160,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 594,
		accuracy = 100,
		basePower = 15,
		basePowerCallback = function (pokemon, target, move) {
			if (pokemon.template.species === 'Greninja-Ash' && pokemon.hasAbility("battlebond")) {
				return move.basePower + 5;
			}
			return move.basePower;
		},
		category = Category.SPECIAL,
		//desc = "Hits two to five times. Has a 1/3 chance to hit two or three times, and a 1/6 chance to hit four or five times. If one of the hits breaks the target'S substitute, it will take damage for the remaining hits. If the user has the Ability Skill Link, this move will always hit five times. Base power is 20 if the user is Ash-Greninja.",
		//shortDesc = "Usually goes first. Hits 2-5 times in one turn.",
		id = Moves.WATERSHURIKEN,
		isViable = true,
		//name = "Water Shuriken",
		pp = 20,
		priority = 1,
		flags = new Flags (protect: true, mirror: true),
		multihit = new int[] {2, 5},
		//secondary = false,
		target = Target.Normal,
		type = Types.WATER,
		zMovePower = 100,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 311,
		accuracy = 100,
		basePower = 50,
		category = Category.SPECIAL,
		//desc = "Power doubles during weather effects (except strong winds) and this move's type changes to match; Ice type during Hail, Water type during Rain Dance, Rock type during Sandstorm, and Fire type during Sunny Day.",
		//shortDesc = "Power doubles and type varies in each weather.",
		id = Moves.WEATHERBALL,
		//name = "Weather Ball",
		pp = 10,
		priority = 0,
		flags = new Flags (bullet: true, protect: true, mirror: true),
		onModifyMove = function (move) {
			switch (this.effectiveWeather()) {
			case "sunnyday":
			case "desolateland":
				move.type = "Fire";
				move.basePower *= 2;
				break;
			case "raindance":
			case "primordialsea":
				move.type = "Water";
				move.basePower *= 2;
				break;
			case "sandstorm":
				move.type = "Rock";
				move.basePower *= 2;
				break;
			case "hail":
				move.type = "Ice";
				move.basePower *= 2;
				break;
			}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 160,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 250,
		accuracy = 85,
		basePower = 35,
		category = Category.SPECIAL,
		//desc = "Prevents the target From switching for four or five turns; seven turns if the user is holding Grip Claw. Causes damage to the target Equal to 1/8 of its maximum HP (1/6 if the user is holding Binding Band), rounded down, at the end of each turn during effect. The target Can still switch out if it is holding Shed Shell or uses Baton Pass, Parting Shot, U-turn, or Volt Switch. The effect ends if either the user or the target Leaves the field, or if the target Uses Rapid Spin or Substitute. This effect is not stackable or reset by using this or another partial-trapping move.",
		//shortDesc = "Traps and damages the target For 4-5 turns.",
		id = Moves.WHIRLPOOL,
		//name = "Whirlpool",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		volatileStatus = "partiallytrapped",
		//secondary = false,
		target = Target.Normal,
		type = Types.WATER,
		zMovePower = 100,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 18,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The target Is forced to switch out and be replaced with a random unfainted ally. Fails if the target Used Ingrain previously or has the Ability Suction Cups.",
		//shortDesc = "Forces the target To switch to a random ally.",
		id = Moves.WHIRLWIND,
		isViable = true,
		//name = "Whirlwind",
		pp = 20,
		priority = -6,
		flags = new Flags (reflectable: true, mirror: true, authentic: true, mystery: true),
		forceSwitch = true,
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {spd = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 469,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "The user and its party members are protected from moves MADE by other Pokemon, including allies, during this turn that target All adjacent foes or all adjacent Pokemon. This move modifies the same 1/X chance of being successful used by other protection moves, where X starts at 1 and triples each time this move is successfully used, but does not use the chance to check for failure. X resets to 1 if this move fails or if the user's last move used is not Baneful Bunker, Detect, Endure, King's Shield, Protect, Quick Guard, Spiky Shield, or Wide Guard. Fails if the user moves LAST this turn or if this move is already in effect for the user's side.",
		//shortDesc = "Protects allies from multi-target Moves THIS turn.",
		id = Moves.WIDEGUARD,
		//name = "Wide Guard",
		pp = 10,
		priority = 3,
		flags = new Flags (snatch: true),
		sideCondition = "wideguard",
		onTryHitSide = function (side, source) {
			return this.willAct();
		},
		onHitSide = function (side, source) {
			source.addVolatile("stall");
		},
		effect = {
			duration = 1,
			onStart = function (target, source) {
				this.add('-singleturn', source, 'Wide Guard');
			},
			onTryHitPriority = 4,
			onTryHit = function (target, source, move) {
				// Wide Guard blocks all spread moves
				if (move && move.target !== "allAdjacent" && move.target !== "allAdjacentFoes") {
					return;
				}
				if (move.isZ) {
					move.zBrokeProtect = true;
					return;
				}
				this.add('-activate', target, 'move = Wide Guard');
				source.moveThisTurnResult = true;
				let lockedmove = source.getVolatile("lockedmove");
				if (lockedmove) {
					// Outrage counter is reset
					if (source.volatiles["lockedmove"].duration === 2) {
						delete source.volatiles["lockedmove"];
					}
				}
				return null;
			}
		},
		//secondary = false,
		target = Target.AllySide,
		type = Types.ROCK,
		zMoveBoost = new Boosts() {def = 1},
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 528,
		accuracy = 100,
		basePower = 90,
		category = Category.PHYSICAL,
		//desc = "If the target Lost HP, the user takes recoil damage equal to 1/4 the HP lost by the target, rounded half up, but not less than 1 HP.",
		//shortDesc = "Has 1/4 recoil.",
		id = Moves.WILDCHARGE,
		isViable = true,
		//name = "Wild Charge",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		recoil = new int[] {1, 4},
		//secondary = false,
		target = Target.Normal,
		type = Types.ELECTRIC,
		zMovePower = 175,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 261,
		accuracy = 85,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Burns the target.",
		//shortDesc = "Burns the target.",
		id = Moves.WILLOWISP,
		isViable = true,
		//name = "Will-O-Wisp",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true),
		status = "brn",
		//secondary = false,
		target = Target.Normal,
		type = Types.FIRE,
		zMoveBoost = new Boosts() {atk = 1},
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 17,
		accuracy = 100,
		basePower = 60,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect.",
		id = Moves.WINGATTACK,
		//name = "Wing Attack",
		pp = 35,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true, distance: true),
		//secondary = false,
		target = Target.Any,
		type = Types.FLYING,
		zMovePower = 120,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 273,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "At the end of the next turn, the Pokemon at the user's position has 1/2 of the user's maximum HP restored to it, rounded half up. Fails if this move is already in effect for the user's position.",
		//shortDesc = "Next turn, 50% of the user's max HP is restored.",
		id = Moves.WISH,
		isViable = true,
		//name = "Wish",
		pp = 10,
		priority = 0,
		flags = new Flags (snatch: true, heal: true),
		sideCondition = "Wish",
		effect = {
			duration = 2,
			onStart = function (side, source) {
				this.effectData.hp = source.maxhp / 2;
			},
			onResidualOrder = 4,
			onEnd = function (side) {
				// @ts-ignore
				let target = side.active[this.effectData.sourcePosition];
				if (target && !target.Fainted) {
					let source = this.effectData.source;
					let damage = this.heal(this.effectData.hp, target, target);
					if (damage) this.add('-heal', target, target.GetHealth, '[from] move = Wish', '[wisher] ' + source.name);
				}
			}
		},
		//secondary = false,
		target = Target.Self,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {spd = 1},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 110,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the user's Defense by 1 stage.",
		//shortDesc = "Raises the user's Defense by 1.",
		id = Moves.WITHDRAW,
		//name = "Withdraw",
		pp = 40,
		priority = 0,
		flags = new Flags (snatch: true),
		boosts = new Boosts() {
			def = 1
		},
		//secondary = false,
		target = Target.Self,
		type = Types.WATER,
		zMoveBoost = new Boosts() {def = 1},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 472,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "For 5 turns, all active Pokemon have their Defense and Special Defense stats swapped. Stat stage changes are unaffected. If this move is used during the effect, the effect ends.",
		//shortDesc = "For 5 turns, all Defense and Sp. Def stats switch.",
		id = Moves.WONDERROOM,
		//name = "Wonder Room",
		pp = 10,
		priority = 0,
		flags = new Flags (mirror: true),
		pseudoWeather = "wonderroom",
		effect = {
			duration = 5,
			durationCallback = function (source, effect) {
				if (source && source.hasAbility("persistent")) {
					this.add('-activate', source, 'ability = Persistent', effect);
					return 7;
				}
				return 5;
			},
			onStart = function (side, source) {
				this.add('-fieldstart', 'move = Wonder Room', '[of] ' + source);
			},
			onRestart = function (target, source) {
				this.removePseudoWeather("wonderroom");
			},
			// Swapping defenses implemented in sim/pokemon.js:Pokemon#calculateStat and Pokemon#getStat
			onResidualOrder = 24,
			onEnd = function () {
				this.add('-fieldend', 'move = Wonder Room');
			}
		},
		//secondary = false,
		target = Target.All,
		type = Types.PSYCHIC,
		zMoveBoost = new Boosts() {spd = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 452,
		accuracy = 100,
		basePower = 120,
		category = Category.PHYSICAL,
		//desc = "If the target Lost HP, the user takes recoil damage equal to 33% the HP lost by the target, rounded half up, but not less than 1 HP.",
		//shortDesc = "Has 33% recoil.",
		id = Moves.WOODHAMMER,
		isViable = true,
		//name = "Wood Hammer",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		recoil = new int[] {33, 100},
		//secondary = false,
		target = Target.Normal,
		type = Types.GRASS,
		zMovePower = 190,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 526,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Raises the user's Attack and Special Attack by 1 stage.",
		//shortDesc = "Raises the user's Attack and Sp. Atk by 1.",
		id = Moves.WORKUP,
		//name = "Work Up",
		pp = 30,
		priority = 0,
		flags = new Flags (snatch: true),
		boosts = new Boosts() {
			atk = 1,
			spa = 1
		},
		//secondary = false,
		target = Target.Self,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {atk = 1},
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 388,
		accuracy = 100,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Causes the target'S Ability to become Insomnia. Fails if the target'S Ability is Insomnia, Multitype, Stance Change, or Truant.",
		//shortDesc = "The target'S Ability becomes Insomnia.",
		id = Moves.WORRYSEED,
		//name = "Worry Seed",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true, mystery: true),
		onTryHit = function (pokemon) {
			let bannedAbilities = ["battlebond", "comatose", "disguise", "insomnia", "multitype", "powerconstruct", "rkssystem", "schooling", "shieldsdown", "stancechange", "truant"];
			if (bannedAbilities.includes(pokemon.ability)) {
				return false;
			}
		},
		onHit = function (pokemon) {
			let oldAbility = pokemon.setAbility("insomnia");
			if (oldAbility) {
				this.add('-ability', pokemon, "Insomnia", '[from] move = Worry Seed');
				if (pokemon.status === "slp") {
					pokemon.cureStatus();
				}
				return;
			}
			return false;
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.GRASS,
		zMoveBoost = new Boosts() {spe = 1},
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 35,
		accuracy = 90,
		basePower = 15,
		category = Category.PHYSICAL,
		//desc = "Prevents the target From switching for four or five turns; seven turns if the user is holding Grip Claw. Causes damage to the target Equal to 1/8 of its maximum HP (1/6 if the user is holding Binding Band), rounded down, at the end of each turn during effect. The target Can still switch out if it is holding Shed Shell or uses Baton Pass, Parting Shot, U-turn, or Volt Switch. The effect ends if either the user or the target Leaves the field, or if the target Uses Rapid Spin or Substitute. This effect is not stackable or reset by using this or another partial-trapping move.",
		//shortDesc = "Traps and damages the target For 4-5 turns.",
		id = Moves.WRAP,
		//name = "Wrap",
		pp = 20,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		volatileStatus = "partiallytrapped",
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 100,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 378,
		accuracy = 100,
		basePower = 0,
		basePowerCallback = function (pokemon, target) {
			return Math.floor(Math.floor((120 * (100 * Math.floor(target.Hp * 4096 / target.Maxhp)) + 2048 - 1) / 4096) / 100) || 1;
		},
		category = Category.SPECIAL,
		//desc = "Power is equal to 120 * (target'S current HP / target'S maximum HP), rounded half down, but not less than 1.",
		//shortDesc = "More power the more HP the target Has left.",
		id = Moves.WRINGOUT,
		//name = "Wring Out",
		pp = 5,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMovePower = 190,
		contestType = Contest.TOUGH
	},
	new BattleMovedex() {
		num = 404,
		accuracy = 100,
		basePower = 80,
		category = Category.PHYSICAL,
		//desc = "No additional effect.",
		//shortDesc = "No additional effect.",
		id = Moves.XSCISSOR,
		isViable = true,
		//name = "X-Scissor",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		//secondary = false,
		target = Target.Normal,
		type = Types.BUG,
		zMovePower = 160,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 281,
		//accuracy = true,
		basePower = 0,
		category = Category.STATUS,
		//desc = "Causes the target To fall asleep at the end of the next turn. Fails when used if the target Cannot fall asleep or if it already has a major status condition. At the end of the next turn, if the target Is still on the field, does not have a major status condition, and can fall asleep, it falls asleep. If the target Becomes affected, this effect cannot be prevented by Safeguard or a substitute, or by falling asleep and waking up during the effect.",
		//shortDesc = "Puts the target To sleep after 1 turn.",
		id = Moves.YAWN,
		//name = "Yawn",
		pp = 10,
		priority = 0,
		flags = new Flags (protect: true, reflectable: true, mirror: true),
		volatileStatus = "yawn",
		//onTryHit = function (target) {
		//	if (target.Status || !target.RunStatusImmunity("slp")) {
		//		return false;
		//	}
		//},
		effect = {
			noCopy = true, // doesn't get copied by Baton Pass
			duration = 2,
			//onStart = function (target, source) {
			//	this.add('-start', target, 'move = Yawn', '[of] ' + source);
			//},
			//onEnd = function (target) {
			//	this.add('-end', target, 'move = Yawn', '[silent]');
			//	target.TrySetStatus("slp");
			//}
		},
		//secondary = false,
		target = Target.Normal,
		type = Types.NORMAL,
		zMoveBoost = new Boosts() {spe = 1},
		contestType = Contest.CUTE
	},
	new BattleMovedex() {
		num = 192,
		accuracy = 50,
		basePower = 120,
		category = Category.SPECIAL,
		//desc = "Has a 100% chance to paralyze the target.",
		//shortDesc = "100% chance to paralyze the target.",
		id = Moves.ZAPCANNON,
		//name = "Zap Cannon",
		pp = 5,
		priority = 0,
		flags = new Flags (bullet: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 100,
			status = "par"
		},
		target = Target.Normal,
		type = Types.ELECTRIC,
		zMovePower = 190,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 428,
		accuracy = 90,
		basePower = 80,
		category = Category.PHYSICAL,
		//desc = "Has a 20% chance to flinch the target.",
		//shortDesc = "20% chance to flinch the target.",
		id = Moves.ZENHEADBUTT,
		isViable = true,
		//name = "Zen Headbutt",
		pp = 15,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 20,
			volatileStatus = "flinch"
		},
		target = Target.Normal,
		type = Types.PSYCHIC,
		zMovePower = 160,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 716,
		accuracy = 100,
		basePower = 80,
		category = Category.PHYSICAL,
		//desc = "Has a 30% chance to flinch the target.",
		//shortDesc = "30% chance to flinch the target.",
		id = Moves.ZINGZAP,
		isViable = true,
		//name = "Zing Zap",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 30,
			volatileStatus = "flinch"
		},
		target = Target.Normal,
		type = Types.ELECTRIC,
		zMovePower = 160,
		contestType = Contest.COOL
	},
	new BattleMovedex() {
		num = 0,
		accuracy = 100,
		basePower = 85,
		category = Category.SPECIAL,
		//desc = "Has a 20% chance to lower the target'S Attack by 1 stage.",
		//shortDesc = "20% chance to lower the target'S Attack by 1.",
		id = Moves.PALEOWAVE,
		isNonstandard = true,
		isViable = true,
		//name = "Paleo Wave",
		pp = 15,
		priority = 0,
		flags = new Flags (protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 20,
			boosts = new Boosts() {
				atk = -1
			}
		},
		target = Target.Normal,
		type = Types.ROCK,
		zMovePower = 160,
		contestType = Contest.BEAUTY
	},
	new BattleMovedex() {
		num = 0,
		accuracy = 95,
		basePower = 80,
		category = Category.PHYSICAL,
		//desc = "Has a 50% chance to lower the target'S Defense by 1 stage.",
		//shortDesc = "50% chance to lower the target'S Defense by 1.",
		id = Moves.SHADOWSTRIKE,
		isNonstandard = true,
		isViable = true,
		//name = "Shadow Strike",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, protect: true, mirror: true),
		secondary = new Secondary() {
			chance = 50,
			boosts = new Boosts() {
				def = -1
			}
		},
		target = Target.Normal,
		type = Types.GHOST,
		zMovePower = 160,
		contestType = Contest.CLEVER
	},
	new BattleMovedex() {
		num = 0,
		//accuracy = true,
		basePower = 120,
		category = Category.PHYSICAL,
		//desc = "Has a 100% chance to confuse the target And lower its Defense and Special Attack by 1 stage. The user recovers 1/2 the HP lost by the target, rounded half up. If Big Root is held by the user, the HP recovered is 1.3x normal, rounded half down. The user steals the foe's boosts. If this move is successful, the weather changes to rain unless it is already in effect, and the user gains the effects of Aqua Ring and Magic Coat.",
		//shortDesc = "Does many things turn 1. Can't move turn 2.",
		id = Moves.MAGIKARPSREVENGE,
		isNonstandard = true,
		//name = "Magikarp's Revenge",
		pp = 10,
		priority = 0,
		flags = new Flags (contact: true, recharge: true, protect: true, mirror: true, heal: true),
		noSketch = true,
		drain = new int[] {1, 2},
		onTry = function (pokemon) {
			if (pokemon.template.name !== "Magikarp") {
				this.add('-fail', pokemon, 'move = Magikarp\'s Revenge');
				return null;
			}
		},
		self = {
			onHit = function (source) {
				this.setWeather("raindance");
				source.addVolatile("magiccoat");
				source.addVolatile("aquaring");
			},
			volatileStatus = "mustrecharge"
		},
		secondary = new Secondary() {
			chance = 100,
			volatileStatus = "confusion",
			boosts = new Boosts() {
				def = -1,
				spa = -1
			}
		},
		stealsBoosts = true,
		target = Target.Normal,
		type = Types.WATER,
		zMovePower = 190,
		contestType = Contest.CUTE
	}*/
	#endregion
};
		}
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
        /// <see cref="eItems.Item.KINGS_ROCK"/>/<see cref="eItems.Item.RAZOR_FANG"/>. 
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
	public class MoveDataDex
	{
		private static readonly MoveDataDex[] Database;

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

		public MoveDataDex()
		{
			Name = LanguageExtension.Translate(Text.Moves, ID.ToString()).Name;
			Description = LanguageExtension.Translate(Text.Moves, ID.ToString()).Value;
		}

		internal MoveDataDex getMove(Moves ID)
		{
			foreach (MoveDataDex move in Database)
			{
				if (move.ID == ID) return move;
			}
			throw new System.Exception("Move ID doesnt exist in the database. Please check MoveData constructor.");
		}

		static MoveDataDex()
		{
Database = new MoveDataDex[] {
	// ^(\d*),(\w*),([\w\s-]*),(\w*),(\d*),(\w*),(\w*),(\d*),(\d*),(\d*),(\d*),([-\d]*),(\w*),("[\w\s-,.'é]*")
	// new MoveDataDex() {\n\tnum = $1, \n\tid = Moves.$2, \n\t//name = "$3", \n\t//function = $4, \n\tbasePower = $5, \n\ttype = Types.$6, \n\tcategory = Category.$7, \n\taccuracy = $8, \n\tpp = $9, \n\teffects = $10, \n\ttarget = Target.$11, \n\tpriority = $12, \n\tflags = new Flags() { $13 } \n\t//,description = $14\n},
	//(flags = new Flags\(\) \{\s[\w]*)l([,]|[\w\s=,]*\s\}) 
new MoveDataDex() {
	num = 0,
	ID = Moves.NONE, 
	//name = "null", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 0,
	Type = Types.NONE,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 0,
	Effects = 0,
	Target = Target.NoTarget,
	Priority = 0,
	Flags = new Flags( ) 
	//,description = "Placeholder for empty/null moveset."
},
	#region Database
new MoveDataDex() {
	num = 1,
	ID = Moves.MEGAHORN, 
	//name = "Megahorn", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 120,
	Type = Types.BUG,
	Category = Category.PHYSICAL,
	Accuracy = 85,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "Using its tough and impressive horn, the user rams into the target with no letup."
},
new MoveDataDex() {
	num = 2,
	ID = Moves.ATTACK_ORDER, 
	//name = "Attack Order", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 90,
	Type = Types.BUG,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true,bite: true ) { }
	//,description = "The user calls out its underlings to pummel the target. Critical hits land more easily."
},
new MoveDataDex() {
	num = 3,
	ID = Moves.BUG_BUZZ, 
	//name = "Bug Buzz", 
	Function = 0x046, FunctionAsString = "046", 
	BaseDamage = 90,
	Type = Types.BUG,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 10,
	Effects = 10,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,sound: true ) { }
	//,description = "The user vibrates its wings to generate a damaging sound wave. It may also lower the target's Sp. Def stat."
},
new MoveDataDex() {
	num = 4,
	ID = Moves.X_SCISSOR, 
	//name = "X-Scissor", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 80,
	Type = Types.BUG,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user slashes at the foe by crossing its scythes or claws as if they were a pair of scissors."
},
new MoveDataDex() {
	num = 5,
	ID = Moves.SIGNAL_BEAM, 
	//name = "Signal Beam", 
	Function = 0x013, FunctionAsString = "013", 
	BaseDamage = 75,
	Type = Types.BUG,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 15,
	Effects = 10,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user attacks with a sinister beam of light. It may also confuse the target."
},
new MoveDataDex() {
	num = 6,
	ID = Moves.U_TURN, 
	//name = "U-turn", 
	Function = 0x0EE, FunctionAsString = "0EE", 
	BaseDamage = 70,
	Type = Types.BUG,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "After making its attack, the user rushes back to switch places with a party Pokémon in waiting."
},
new MoveDataDex() {
	num = 7,
	ID = Moves.STEAMROLLER, 
	//name = "Steamroller", 
	Function = 0x010, FunctionAsString = "010", 
	BaseDamage = 65,
	Type = Types.BUG,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 20,
	Effects = 30,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true ) { }
	//,description = "The user crushes its foes by rolling over them. This attack may make the target flinch."
},
new MoveDataDex() {
	num = 8,
	ID = Moves.BUG_BITE, 
	//name = "Bug Bite", 
	Function = 0x0F4, FunctionAsString = "0F4", 
	BaseDamage = 60,
	Type = Types.BUG,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user bites the target. If the target is holding a Berry, the user eats it and gains its effect."
},
new MoveDataDex() {
	num = 9,
	ID = Moves.SILVER_WIND, 
	//name = "Silver Wind", 
	Function = 0x02D, FunctionAsString = "02D", 
	BaseDamage = 60,
	Type = Types.BUG,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 5,
	Effects = 10,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The foe is attacked with powdery scales blown by wind. It may also raise all the user's stats."
},
new MoveDataDex() {
	num = 10,
	ID = Moves.STRUGGLE_BUG, 
	//name = "Struggle Bug", 
	Function = 0x045, FunctionAsString = "045", 
	BaseDamage = 30,
	Type = Types.BUG,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 20,
	Effects = 100,
	Target = Target.AllOpposing,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "While resisting, the user attacks the opposing Pokémon. The targets' Sp. Atk stat is reduced."
},
new MoveDataDex() {
	num = 11,
	ID = Moves.TWINEEDLE, 
	//name = "Twineedle", 
	Function = 0x0BE, FunctionAsString = "0BE", 
	BaseDamage = 25,
	Type = Types.BUG,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 20,
	Effects = 20,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The foe is stabbed twice by a pair of stingers. It may also poison the target."
},
new MoveDataDex() {
	num = 12,
	ID = Moves.FURY_CUTTER, 
	//name = "Fury Cutter", 
	Function = 0x091, FunctionAsString = "091", 
	BaseDamage = 20,
	Type = Types.BUG,
	Category = Category.PHYSICAL,
	Accuracy = 95,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The target is slashed with scythes or claws. Its power increases if it hits in succession."
},
new MoveDataDex() {
	num = 13,
	ID = Moves.LEECH_LIFE, 
	//name = "Leech Life", 
	Function = 0x0DD, FunctionAsString = "0DD", 
	BaseDamage = 20,
	Type = Types.BUG,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true ) { }
	//,description = "The user drains the target's blood. The user's HP is restored by half the damage taken by the target."
},
new MoveDataDex() {
	num = 14,
	ID = Moves.PIN_MISSILE, 
	//name = "Pin Missile", 
	Function = 0x0C0, FunctionAsString = "0C0", 
	BaseDamage = 14,
	Type = Types.BUG,
	Category = Category.PHYSICAL,
	Accuracy = 85,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "Sharp spikes are shot at the target in rapid succession. They hit two to five times in a row."
},
new MoveDataDex() {
	num = 15,
	ID = Moves.DEFEND_ORDER, 
	//name = "Defend Order", 
	Function = 0x02A, FunctionAsString = "02A", 
	BaseDamage = 0,
	Type = Types.BUG,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user calls out its underlings to shield its body, raising its Defense and Sp. Def stats."
},
new MoveDataDex() {
	num = 16,
	ID = Moves.HEAL_ORDER, 
	//name = "Heal Order", 
	Function = 0x0D5, FunctionAsString = "0D5", 
	BaseDamage = 0,
	Type = Types.BUG,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true, bite: true ) { }
	//,description = "The user calls out its underlings to heal it. The user regains up to half of its max HP."
},
new MoveDataDex() {
	num = 17,
	ID = Moves.QUIVER_DANCE, 
	//name = "Quiver Dance", 
	Function = 0x02B, FunctionAsString = "02B", 
	BaseDamage = 0,
	Type = Types.BUG,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 20,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user performs a beautiful dance. It boosts the user's Sp. Atk, Sp. Def, and Speed stats."
},
new MoveDataDex() {
	num = 18,
	ID = Moves.RAGE_POWDER, 
	//name = "Rage Powder", 
	Function = 0x117, FunctionAsString = "117", 
	BaseDamage = 0,
	Type = Types.BUG,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 20,
	Effects = 0,
	Target = Target.User,
	Priority = 3,
	Flags = new Flags(  ) { } 
	//,description = "The user scatters irritating powder to draw attention to itself. Opponents aim only at the user."
},
new MoveDataDex() {
	num = 19,
	ID = Moves.SPIDER_WEB, 
	//name = "Spider Web", 
	Function = 0x0EF, FunctionAsString = "0EF", 
	BaseDamage = 0,
	Type = Types.BUG,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "The user ensnares the target with thin, gooey silk so it can't flee from battle."
},
new MoveDataDex() {
	num = 20,
	ID = Moves.STRING_SHOT, 
	//name = "String Shot", 
	Function = 0x044, FunctionAsString = "044", 
	BaseDamage = 0,
	Type = Types.BUG,
	Category = Category.STATUS,
	Accuracy = 95,
	PP = 40,
	Effects = 0,
	Target = Target.AllOpposing,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "The foe is bound with silk blown from the user's mouth. This silk reduces the target's Speed."
},
new MoveDataDex() {
	num = 21,
	ID = Moves.TAIL_GLOW, 
	//name = "Tail Glow", 
	Function = 0x039, FunctionAsString = "039", 
	BaseDamage = 0,
	Type = Types.BUG,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 20,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user stares at flashing lights to focus its mind, drastically raising its Sp. Atk stat."
},
new MoveDataDex() {
	num = 22,
	ID = Moves.FOUL_PLAY, 
	//name = "Foul Play", 
	Function = 0x121, FunctionAsString = "121", 
	BaseDamage = 95,
	Type = Types.DARK,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user turns the foe's power against it. It does more damage the higher the target's Attack stat."
},
new MoveDataDex() {
	num = 23,
	ID = Moves.NIGHT_DAZE, 
	//name = "Night Daze", 
	Function = 0x047, FunctionAsString = "047", 
	BaseDamage = 85,
	Type = Types.DARK,
	Category = Category.SPECIAL,
	Accuracy = 95,
	PP = 10,
	Effects = 40,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user lets loose a pitch-black shock wave at its target. It may also lower the target's accuracy."
},
new MoveDataDex() {
	num = 24,
	ID = Moves.CRUNCH, 
	//name = "Crunch", 
	Function = 0x043, FunctionAsString = "043", 
	BaseDamage = 80,
	Type = Types.DARK,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 15,
	Effects = 20,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true ) { }
	//,description = "The user crunches up the target with sharp fangs. It may also lower the target's Defense stat."
},
new MoveDataDex() {
	num = 25,
	ID = Moves.DARK_PULSE, 
	//name = "Dark Pulse", 
	Function = 0x00F, FunctionAsString = "00F", 
	BaseDamage = 80,
	Type = Types.DARK,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 15,
	Effects = 20,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user releases a horrible aura imbued with dark thoughts. It may also make the target flinch."
},
new MoveDataDex() {
	num = 26,
	ID = Moves.SUCKER_PUNCH, 
	//name = "Sucker Punch", 
	Function = 0x116, FunctionAsString = "116", 
	BaseDamage = 80,
	Type = Types.DARK,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 1,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "This move enables the user to attack first. It fails if the target is not readying an attack, however."
},
new MoveDataDex() {
	num = 27,
	ID = Moves.NIGHT_SLASH, 
	//name = "Night Slash", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 70,
	Type = Types.DARK,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true,bite: true ) { }
	//,description = "The user slashes the target the instant an opportunity arises. Critical hits land more easily."
},
new MoveDataDex() {
	num = 28,
	ID = Moves.BITE, 
	//name = "Bite", 
	Function = 0x00F, FunctionAsString = "00F", 
	BaseDamage = 60,
	Type = Types.DARK,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 25,
	Effects = 30,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true ) { }
	//,description = "The target is bitten with viciously sharp fangs. It may make the target flinch."
},
new MoveDataDex() {
	num = 29,
	ID = Moves.FEINT_ATTACK, 
	//name = "Faint Attack", 
	Function = 0x0A5, FunctionAsString = "0A5", 
	BaseDamage = 60,
	Type = Types.DARK,
	Category = Category.PHYSICAL,
	Accuracy = 0,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user draws up to the foe disarmingly, then throws a sucker punch. It hits without fail."
},
new MoveDataDex() {
	num = 30,
	ID = Moves.SNARL, 
	//name = "Snarl", 
	Function = 0x045, FunctionAsString = "045", 
	BaseDamage = 55,
	Type = Types.DARK,
	Category = Category.PHYSICAL,
	Accuracy = 95,
	PP = 15,
	Effects = 100,
	Target = Target.AllOpposing,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true,sound: true ) { }
	//,description = "The user yells as if it is ranting about something, making the target's Sp. Atk stat decrease."
},
new MoveDataDex() {
	num = 31,
	ID = Moves.ASSURANCE, 
	//name = "Assurance", 
	Function = 0x082, FunctionAsString = "082", 
	BaseDamage = 50,
	Type = Types.DARK,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "If the target has already taken some damage in the same turn, this attack's power is doubled."
},
new MoveDataDex() {
	num = 32,
	ID = Moves.PAYBACK, 
	//name = "Payback", 
	Function = 0x084, FunctionAsString = "084", 
	BaseDamage = 50,
	Type = Types.DARK,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "If the user moves after the target, this attack's power will be doubled."
},
new MoveDataDex() {
	num = 33,
	ID = Moves.PURSUIT, 
	//name = "Pursuit", 
	Function = 0x088, FunctionAsString = "088", 
	BaseDamage = 40,
	Type = Types.DARK,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true ) { }
	//,description = "An attack move that inflicts double damage if used on a target that is switching out of battle."
},
new MoveDataDex() {
	num = 34,
	ID = Moves.THIEF, 
	//name = "Thief", 
	Function = 0x0F1, FunctionAsString = "0F1", 
	BaseDamage = 40,
	Type = Types.DARK,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true ) { }
	//,description = "The user attacks and steals the foe's held item simultaneously. It can't steal if the user holds an item."
},
new MoveDataDex() {
	num = 35,
	ID = Moves.KNOCK_OFF, 
	//name = "Knock Off", 
	Function = 0x0F0, FunctionAsString = "0F0", 
	BaseDamage = 20,
	Type = Types.DARK,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true ) { }
	//,description = "The user slaps down the target's held item, preventing that item from being used in the battle."
},
new MoveDataDex() {
	num = 36,
	ID = Moves.BEAT_UP, 
	//name = "Beat Up", 
	Function = 0x0C1, FunctionAsString = "0C1", 
	BaseDamage = 1,
	Type = Types.DARK,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user gets all the party Pokémon to attack the foe. The more party Pokémon, the more damage."
},
new MoveDataDex() {
	num = 37,
	ID = Moves.FLING, 
	//name = "Fling", 
	Function = 0x0F7, FunctionAsString = "0F7", 
	BaseDamage = 1,
	Type = Types.DARK,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user flings its held item at the target to attack. Its power and effects depend on the item."
},
new MoveDataDex() {
	num = 38,
	ID = Moves.PUNISHMENT, 
	//name = "Punishment", 
	Function = 0x08F, FunctionAsString = "08F", 
	BaseDamage = 1,
	Type = Types.DARK,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "This attack's power increases the more the target has powered up with stat changes."
},
new MoveDataDex() {
	num = 39,
	ID = Moves.DARK_VOID, 
	//name = "Dark Void", 
	Function = 0x003, FunctionAsString = "003", 
	BaseDamage = 0,
	Type = Types.DARK,
	Category = Category.STATUS,
	Accuracy = 80,
	PP = 10,
	Effects = 0,
	Target = Target.AllOpposing,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "Opposing Pokémon are dragged into a world of total darkness that makes them sleep."
},
new MoveDataDex() {
	num = 40,
	ID = Moves.EMBARGO, 
	//name = "Embargo", 
	Function = 0x0F8, FunctionAsString = "0F8", 
	BaseDamage = 0,
	Type = Types.DARK,
	Category = Category.STATUS,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "It prevents the target from using its held item. Its Trainer is also prevented from using items on it."
},
new MoveDataDex() {
	num = 41,
	ID = Moves.FAKE_TEARS, 
	//name = "Fake Tears", 
	Function = 0x04F, FunctionAsString = "04F", 
	BaseDamage = 0,
	Type = Types.DARK,
	Category = Category.STATUS,
	Accuracy = 100,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "The user feigns crying to fluster the target, harshly lowering its Sp. Def stat."
},
new MoveDataDex() {
	num = 42,
	ID = Moves.FLATTER, 
	//name = "Flatter", 
	Function = 0x040, FunctionAsString = "040", 
	BaseDamage = 0,
	Type = Types.DARK,
	Category = Category.STATUS,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "Flattery is used to confuse the target. However, it also raises the target's Sp. Atk stat."
},
new MoveDataDex() {
	num = 43,
	ID = Moves.HONE_CLAWS, 
	//name = "Hone Claws", 
	Function = 0x029, FunctionAsString = "029", 
	BaseDamage = 0,
	Type = Types.DARK,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 15,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user sharpens its claws to boost its Attack stat and accuracy."
},
new MoveDataDex() {
	num = 44,
	ID = Moves.MEMENTO, 
	//name = "Memento", 
	Function = 0x0E2, FunctionAsString = "0E2", 
	BaseDamage = 0,
	Type = Types.DARK,
	Category = Category.STATUS,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( mirror: true ) { }
	//,description = "The user faints when using this move. In return, it harshly lowers the target's Attack and Sp. Atk."
},
new MoveDataDex() {
	num = 45,
	ID = Moves.NASTY_PLOT, 
	//name = "Nasty Plot", 
	Function = 0x032, FunctionAsString = "032", 
	BaseDamage = 0,
	Type = Types.DARK,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 20,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user stimulates its brain by thinking bad thoughts. It sharply raises the user's Sp. Atk."
},
new MoveDataDex() {
	num = 46,
	ID = Moves.QUASH, 
	//name = "Quash", 
	Function = 0x11E, FunctionAsString = "11E", 
	BaseDamage = 0,
	Type = Types.DARK,
	Category = Category.STATUS,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The user suppresses the target and makes its move go last."
},
new MoveDataDex() {
	num = 47,
	ID = Moves.SNATCH, 
	//name = "Snatch", 
	Function = 0x0B2, FunctionAsString = "0B2", 
	BaseDamage = 0,
	Type = Types.DARK,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.User,
	Priority = 4,
	Flags = new Flags(  ) { } 
	//,description = "The user steals the effects of any healing or stat-changing move the foe attempts to use."
},
new MoveDataDex() {
	num = 48,
	ID = Moves.SWITCHEROO, 
	//name = "Switcheroo", 
	Function = 0x0F2, FunctionAsString = "0F2", 
	BaseDamage = 0,
	Type = Types.DARK,
	Category = Category.STATUS,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The user trades held items with the target faster than the eye can follow."
},
new MoveDataDex() {
	num = 49,
	ID = Moves.TAUNT, 
	//name = "Taunt", 
	Function = 0x0BA, FunctionAsString = "0BA", 
	BaseDamage = 0,
	Type = Types.DARK,
	Category = Category.STATUS,
	Accuracy = 100,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "The target is taunted into a rage that allows it to use only attack moves for three turns."
},
new MoveDataDex() {
	num = 50,
	ID = Moves.TORMENT, 
	//name = "Torment", 
	Function = 0x0B7, FunctionAsString = "0B7", 
	BaseDamage = 0,
	Type = Types.DARK,
	Category = Category.STATUS,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "The user torments and enrages the foe, making it incapable of using the same move twice in a row."
},
new MoveDataDex() {
	num = 51,
	ID = Moves.ROAR_OF_TIME, 
	//name = "Roar of Time", 
	Function = 0x0C2, FunctionAsString = "0C2", 
	BaseDamage = 150,
	Type = Types.DRAGON,
	Category = Category.SPECIAL,
	Accuracy = 90,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user blasts the target with power that distorts even time. The user must rest on the next turn."
},
new MoveDataDex() {
	num = 52,
	ID = Moves.DRACO_METEOR, 
	//name = "Draco Meteor", 
	Function = 0x03F, FunctionAsString = "03F", 
	BaseDamage = 140,
	Type = Types.DRAGON,
	Category = Category.SPECIAL,
	Accuracy = 90,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "Comets are summoned down from the sky. The attack's recoil harshly reduces the user's Sp. Atk stat."
},
new MoveDataDex() {
	num = 53,
	ID = Moves.OUTRAGE, 
	//name = "Outrage", 
	Function = 0x0D2, FunctionAsString = "0D2", 
	BaseDamage = 120,
	Type = Types.DRAGON,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.RandomOpposing,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user rampages and attacks for two to three turns. It then becomes confused, however."
},
new MoveDataDex() {
	num = 54,
	ID = Moves.DRAGON_RUSH, 
	//name = "Dragon Rush", 
	Function = 0x00F, FunctionAsString = "00F", 
	BaseDamage = 100,
	Type = Types.DRAGON,
	Category = Category.PHYSICAL,
	Accuracy = 75,
	PP = 10,
	Effects = 20,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user tackles the foe while exhibiting overwhelming menace. It may also make the target flinch."
},
new MoveDataDex() {
	num = 55,
	ID = Moves.SPACIAL_REND, 
	//name = "Spacial Rend", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 100,
	Type = Types.DRAGON,
	Category = Category.SPECIAL,
	Accuracy = 95,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true,bite: true ) { }
	//,description = "The user tears the target along with the space around it. Critical hits land more easily."
},
new MoveDataDex() {
	num = 56,
	ID = Moves.DRAGON_PULSE, 
	//name = "Dragon Pulse", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 90,
	Type = Types.DRAGON,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The target is attacked with a shock wave generated by the user's gaping mouth."
},
new MoveDataDex() {
	num = 57,
	ID = Moves.DRAGON_CLAW, 
	//name = "Dragon Claw", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 80,
	Type = Types.DRAGON,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user slashes the target with huge, sharp claws."
},
new MoveDataDex() {
	num = 58,
	ID = Moves.DRAGON_TAIL, 
	//name = "Dragon Tail", 
	Function = 0x0EC, FunctionAsString = "0EC", 
	BaseDamage = 60,
	Type = Types.DRAGON,
	Category = Category.PHYSICAL,
	Accuracy = 90,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = -6,
	Flags = new Flags( contact: true,protect: true,mirror: true ) { }
	//,description = "The user knocks away the target and drags out another Pokémon in its party. In the wild, the battle ends."
},
new MoveDataDex() {
	num = 59,
	ID = Moves.DRAGON_BREATH, 
	//name = "DragonBreath", 
	Function = 0x007, FunctionAsString = "007", 
	BaseDamage = 60,
	Type = Types.DRAGON,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 20,
	Effects = 30,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user exhales a mighty gust that inflicts damage. It may also leave the target with paralysis."
},
new MoveDataDex() {
	num = 60,
	ID = Moves.DUAL_CHOP, 
	//name = "Dual Chop", 
	Function = 0x0BD, FunctionAsString = "0BD", 
	BaseDamage = 40,
	Type = Types.DRAGON,
	Category = Category.PHYSICAL,
	Accuracy = 90,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user attacks its target by hitting it with brutal strikes. The target is hit twice in a row."
},
new MoveDataDex() {
	num = 61,
	ID = Moves.TWISTER, 
	//name = "Twister", 
	Function = 0x078, FunctionAsString = "078", 
	BaseDamage = 40,
	Type = Types.DRAGON,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 20,
	Effects = 20,
	Target = Target.AllOpposing,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user whips up a vicious tornado to tear at the opposing team. It may also make targets flinch."
},
new MoveDataDex() {
	num = 62,
	ID = Moves.DRAGON_RAGE, 
	//name = "Dragon Rage", 
	Function = 0x06B, FunctionAsString = "06B", 
	BaseDamage = 1,
	Type = Types.DRAGON,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "This attack hits the target with a shock wave of pure rage. This attack always inflicts 40 HP damage."
},
new MoveDataDex() {
	num = 63,
	ID = Moves.DRAGON_DANCE, 
	//name = "Dragon Dance", 
	Function = 0x026, FunctionAsString = "026", 
	BaseDamage = 0,
	Type = Types.DRAGON,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 20,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user vigorously performs a mystic, powerful dance that boosts its Attack and Speed stats."
},
new MoveDataDex() {
	num = 64,
	ID = Moves.BOLT_STRIKE, 
	//name = "Bolt Strike", 
	Function = 0x007, FunctionAsString = "007", 
	BaseDamage = 130,
	Type = Types.ELECTRIC,
	Category = Category.PHYSICAL,
	Accuracy = 85,
	PP = 5,
	Effects = 20,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user charges at its foe, surrounding itself with lightning. It may also leave the target paralyzed."
},
new MoveDataDex() {
	num = 65,
	ID = Moves.THUNDER, 
	//name = "Thunder", 
	Function = 0x008, FunctionAsString = "008", 
	BaseDamage = 120,
	Type = Types.ELECTRIC,
	Category = Category.SPECIAL,
	Accuracy = 70,
	PP = 10,
	Effects = 30,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "A wicked thunderbolt is dropped on the foe to inflict damage. It may also leave the target paralyzed."
},
new MoveDataDex() {
	num = 66,
	ID = Moves.VOLT_TACKLE, 
	//name = "Volt Tackle", 
	Function = 0x0FD, FunctionAsString = "0FD", 
	BaseDamage = 120,
	Type = Types.ELECTRIC,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 15,
	Effects = 10,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user electrifies itself, then charges at the foe. It causes considerable damage to the user as well."
},
new MoveDataDex() {
	num = 67,
	ID = Moves.ZAP_CANNON, 
	//name = "Zap Cannon", 
	Function = 0x007, FunctionAsString = "007", 
	BaseDamage = 120,
	Type = Types.ELECTRIC,
	Category = Category.SPECIAL,
	Accuracy = 50,
	PP = 5,
	Effects = 100,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The user fires an electric blast like a cannon to inflict damage and cause paralysis."
},
new MoveDataDex() {
	num = 68,
	ID = Moves.FUSION_BOLT, 
	//name = "Fusion Bolt", 
	Function = 0x079, FunctionAsString = "079", 
	BaseDamage = 100,
	Type = Types.ELECTRIC,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user throws down a giant thunderbolt. It does more damage if influenced by an enormous flame."
},
new MoveDataDex() {
	num = 69,
	ID = Moves.THUNDERBOLT, 
	//name = "Thunderbolt", 
	Function = 0x007, FunctionAsString = "007", 
	BaseDamage = 95,
	Type = Types.ELECTRIC,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 15,
	Effects = 10,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "A strong electric blast is loosed at the target. It may also leave the target with paralysis."
},
new MoveDataDex() {
	num = 70,
	ID = Moves.WILD_CHARGE, 
	//name = "Wild Charge", 
	Function = 0x0FA, FunctionAsString = "0FA", 
	BaseDamage = 90,
	Type = Types.ELECTRIC,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user shrouds itself in electricity and smashes into its foe. It also damages the user a little."
},
new MoveDataDex() {
	num = 71,
	ID = Moves.DISCHARGE, 
	//name = "Discharge", 
	Function = 0x007, FunctionAsString = "007", 
	BaseDamage = 80,
	Type = Types.ELECTRIC,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 15,
	Effects = 30,
	Target = Target.AllNonUsers,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "A flare of electricity is loosed to strike the area around the user. It may also cause paralysis."
},
new MoveDataDex() {
	num = 72,
	ID = Moves.THUNDER_PUNCH, 
	//name = "ThunderPunch", 
	Function = 0x007, FunctionAsString = "007", 
	BaseDamage = 75,
	Type = Types.ELECTRIC,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 15,
	Effects = 10,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,punch: true ) { }
	//,description = "The target is punched with an electrified fist. It may also leave the target with paralysis."
},
new MoveDataDex() {
	num = 73,
	ID = Moves.VOLT_SWITCH, 
	//name = "Volt Switch", 
	Function = 0x0EE, FunctionAsString = "0EE", 
	BaseDamage = 70,
	Type = Types.ELECTRIC,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "After making its attack, the user rushes back to switch places with a party Pokémon in waiting."
},
new MoveDataDex() {
	num = 74,
	ID = Moves.SPARK, 
	//name = "Spark", 
	Function = 0x007, FunctionAsString = "007", 
	BaseDamage = 65,
	Type = Types.ELECTRIC,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 20,
	Effects = 30,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true ) { }
	//,description = "The user throws an electrically charged tackle at the target. It may also leave the target with paralysis."
},
new MoveDataDex() {
	num = 75,
	ID = Moves.THUNDER_FANG, 
	//name = "Thunder Fang", 
	Function = 0x009, FunctionAsString = "009", 
	BaseDamage = 65,
	Type = Types.ELECTRIC,
	Category = Category.PHYSICAL,
	Accuracy = 95,
	PP = 15,
	Effects = 100,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user bites with electrified fangs. It may also make the target flinch or leave it with paralysis."
},
new MoveDataDex() {
	num = 76,
	ID = Moves.SHOCK_WAVE, 
	//name = "Shock Wave", 
	Function = 0x0A5, FunctionAsString = "0A5", 
	BaseDamage = 60,
	Type = Types.ELECTRIC,
	Category = Category.SPECIAL,
	Accuracy = 0,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user strikes the target with a quick jolt of electricity. This attack cannot be evaded."
},
new MoveDataDex() {
	num = 77,
	ID = Moves.ELECTROWEB, 
	//name = "Electroweb", 
	Function = 0x044, FunctionAsString = "044", 
	BaseDamage = 55,
	Type = Types.ELECTRIC,
	Category = Category.SPECIAL,
	Accuracy = 95,
	PP = 15,
	Effects = 100,
	Target = Target.AllOpposing,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user captures and attacks foes by using an electric net, which lowers their Speed stat."
},
new MoveDataDex() {
	num = 78,
	ID = Moves.CHARGE_BEAM, 
	//name = "Charge Beam", 
	Function = 0x020, FunctionAsString = "020", 
	BaseDamage = 50,
	Type = Types.ELECTRIC,
	Category = Category.SPECIAL,
	Accuracy = 90,
	PP = 10,
	Effects = 70,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user fires a concentrated bundle of electricity. It may also raise the user's Sp. Atk stat."
},
new MoveDataDex() {
	num = 79,
	ID = Moves.THUNDER_SHOCK, 
	//name = "ThunderShock", 
	Function = 0x007, FunctionAsString = "007", 
	BaseDamage = 40,
	Type = Types.ELECTRIC,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 30,
	Effects = 10,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "A jolt of electricity is hurled at the foe to inflict damage. It may also leave the target with paralysis."
},
new MoveDataDex() {
	num = 80,
	ID = Moves.ELECTRO_BALL, 
	//name = "Electro Ball", 
	Function = 0x099, FunctionAsString = "099", 
	BaseDamage = 1,
	Type = Types.ELECTRIC,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user hurls an electric orb at the foe. It does more damage the faster the user is."
},
new MoveDataDex() {
	num = 81,
	ID = Moves.CHARGE, 
	//name = "Charge", 
	Function = 0x021, FunctionAsString = "021", 
	BaseDamage = 0,
	Type = Types.ELECTRIC,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 20,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user boosts the power of the Electric move it uses next. It also raises the user's Sp. Def stat."
},
new MoveDataDex() {
	num = 82,
	ID = Moves.MAGNET_RISE, 
	//name = "Magnet Rise", 
	Function = 0x119, FunctionAsString = "119", 
	BaseDamage = 0,
	Type = Types.ELECTRIC,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true,powder: true ) { }
	//,description = "The user levitates using electrically generated magnetism for five turns."
},
new MoveDataDex() {
	num = 83,
	ID = Moves.THUNDER_WAVE, 
	//name = "Thunder Wave", 
	Function = 0x007, FunctionAsString = "007", 
	BaseDamage = 0,
	Type = Types.ELECTRIC,
	Category = Category.STATUS,
	Accuracy = 100,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "A weak electric charge is launched at the target. It causes paralysis if it hits."
},
new MoveDataDex() {
	num = 84,
	ID = Moves.FOCUS_PUNCH, 
	//name = "Focus Punch", 
	Function = 0x115, FunctionAsString = "115", 
	BaseDamage = 150,
	Type = Types.FIGHTING,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = -3,
	Flags = new Flags( contact: true,protect: true,punch: true ) { }
	//,description = "The user focuses its mind before launching a punch. It will fail if the user is hit before it is used."
},
new MoveDataDex() {
	num = 85,
	ID = Moves.HIGH_JUMP_KICK, 
	//name = "Hi Jump Kick", 
	Function = 0x10B, FunctionAsString = "10B", 
	BaseDamage = 130,
	Type = Types.FIGHTING,
	Category = Category.PHYSICAL,
	Accuracy = 90,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true,powder: true ) { }
	//,description = "The target is attacked with a knee kick from a jump. If it misses, the user is hurt instead."
},
new MoveDataDex() {
	num = 86,
	ID = Moves.CLOSE_COMBAT, 
	//name = "Close Combat", 
	Function = 0x03C, FunctionAsString = "03C", 
	BaseDamage = 120,
	Type = Types.FIGHTING,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user fights the foe up close without guarding itself. It also cuts the user's Defense and Sp. Def."
},
new MoveDataDex() {
	num = 87,
	ID = Moves.FOCUS_BLAST, 
	//name = "Focus Blast", 
	Function = 0x046, FunctionAsString = "046", 
	BaseDamage = 120,
	Type = Types.FIGHTING,
	Category = Category.SPECIAL,
	Accuracy = 70,
	PP = 5,
	Effects = 10,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The user heightens its mental focus and unleashes its power. It may also lower the target's Sp. Def."
},
new MoveDataDex() {
	num = 88,
	ID = Moves.SUPERPOWER, 
	//name = "Superpower", 
	Function = 0x03B, FunctionAsString = "03B", 
	BaseDamage = 120,
	Type = Types.FIGHTING,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true ) { }
	//,description = "The user attacks the target with great power. However, it also lowers the user's Attack and Defense."
},
new MoveDataDex() {
	num = 89,
	ID = Moves.CROSS_CHOP, 
	//name = "Cross Chop", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 100,
	Type = Types.FIGHTING,
	Category = Category.PHYSICAL,
	Accuracy = 80,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true,bite: true ) { }
	//,description = "The user delivers a double chop with its forearms crossed. Critical hits land more easily."
},
new MoveDataDex() {
	num = 90,
	ID = Moves.DYNAMIC_PUNCH, 
	//name = "DynamicPunch", 
	Function = 0x013, FunctionAsString = "013", 
	BaseDamage = 100,
	Type = Types.FIGHTING,
	Category = Category.PHYSICAL,
	Accuracy = 50,
	PP = 5,
	Effects = 100,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,punch: true ) { }
	//,description = "The user punches the target with full, concentrated power. It confuses the target if it hits."
},
new MoveDataDex() {
	num = 91,
	ID = Moves.HAMMER_ARM, 
	//name = "Hammer Arm", 
	Function = 0x03E, FunctionAsString = "03E", 
	BaseDamage = 100,
	Type = Types.FIGHTING,
	Category = Category.PHYSICAL,
	Accuracy = 90,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true,punch: true ) { }
	//,description = "The user swings and hits with its strong and heavy fist. It lowers the user's Speed, however."
},
new MoveDataDex() {
	num = 92,
	ID = Moves.JUMP_KICK, 
	//name = "Jump Kick", 
	Function = 0x10B, FunctionAsString = "10B", 
	BaseDamage = 100,
	Type = Types.FIGHTING,
	Category = Category.PHYSICAL,
	Accuracy = 95,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true,powder: true ) { }
	//,description = "The user jumps up high, then strikes with a kick. If the kick misses, the user hurts itself."
},
new MoveDataDex() {
	num = 93,
	ID = Moves.AURA_SPHERE, 
	//name = "Aura Sphere", 
	Function = 0x0A5, FunctionAsString = "0A5", 
	BaseDamage = 90,
	Type = Types.FIGHTING,
	Category = Category.SPECIAL,
	Accuracy = 0,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user looses a blast of aura power from deep within its body. This move is certain to hit."
},
new MoveDataDex() {
	num = 94,
	ID = Moves.SACRED_SWORD, 
	//name = "Sacred Sword", 
	Function = 0x0A9, FunctionAsString = "0A9", 
	BaseDamage = 90,
	Type = Types.FIGHTING,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user attacks by slicing with its long horns. The target's stat changes don't affect the damage."
},
new MoveDataDex() {
	num = 95,
	ID = Moves.SECRET_SWORD, 
	//name = "Secret Sword", 
	Function = 0x122, FunctionAsString = "122", 
	BaseDamage = 85,
	Type = Types.FIGHTING,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user cuts with its long horn. The odd power contained in it does physical damage to the foe."
},
new MoveDataDex() {
	num = 96,
	ID = Moves.SKY_UPPERCUT, 
	//name = "Sky Uppercut", 
	Function = 0x11B, FunctionAsString = "11B", 
	BaseDamage = 85,
	Type = Types.FIGHTING,
	Category = Category.PHYSICAL,
	Accuracy = 90,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true,punch: true ) { }
	//,description = "The user attacks the target with an uppercut thrown skyward with force."
},
new MoveDataDex() {
	num = 97,
	ID = Moves.SUBMISSION, 
	//name = "Submission", 
	Function = 0x0FA, FunctionAsString = "0FA", 
	BaseDamage = 80,
	Type = Types.FIGHTING,
	Category = Category.PHYSICAL,
	Accuracy = 80,
	PP = 25,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user grabs the target and recklessly dives for the ground. It also hurts the user slightly."
},
new MoveDataDex() {
	num = 98,
	ID = Moves.BRICK_BREAK, 
	//name = "Brick Break", 
	Function = 0x10A, FunctionAsString = "10A", 
	BaseDamage = 75,
	Type = Types.FIGHTING,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user attacks with a swift chop. It can also break any barrier such as Light Screen and Reflect."
},
new MoveDataDex() {
	num = 99,
	ID = Moves.DRAIN_PUNCH, 
	//name = "Drain Punch", 
	Function = 0x0DD, FunctionAsString = "0DD", 
	BaseDamage = 75,
	Type = Types.FIGHTING,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true,punch: true ) { }
	//,description = "An energy-draining punch. The user's HP is restored by half the damage taken by the target."
},
new MoveDataDex() {
	num = 100,
	ID = Moves.VITAL_THROW, 
	//name = "Vital Throw", 
	Function = 0x0A5, FunctionAsString = "0A5", 
	BaseDamage = 70,
	Type = Types.FIGHTING,
	Category = Category.PHYSICAL,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = -1,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user attacks last. In return, this throw move is guaranteed not to miss."
},
new MoveDataDex() {
	num = 101,
	ID = Moves.CIRCLE_THROW, 
	//name = "Circle Throw", 
	Function = 0x0EC, FunctionAsString = "0EC", 
	BaseDamage = 60,
	Type = Types.FIGHTING,
	Category = Category.PHYSICAL,
	Accuracy = 90,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = -6,
	Flags = new Flags( contact: true,protect: true,mirror: true ) { }
	//,description = "The user throws the target and drags out another Pokémon in its party. In the wild, the battle ends."
},
new MoveDataDex() {
	num = 102,
	ID = Moves.FORCE_PALM, 
	//name = "Force Palm", 
	Function = 0x007, FunctionAsString = "007", 
	BaseDamage = 60,
	Type = Types.FIGHTING,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 10,
	Effects = 30,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The target is attacked with a shock wave. It may also leave the target with paralysis."
},
new MoveDataDex() {
	num = 103,
	ID = Moves.LOW_SWEEP, 
	//name = "Low Sweep", 
	Function = 0x044, FunctionAsString = "044", 
	BaseDamage = 60,
	Type = Types.FIGHTING,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 20,
	Effects = 100,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user attacks the target's legs swiftly, reducing the target's Speed stat."
},
new MoveDataDex() {
	num = 104,
	ID = Moves.REVENGE, 
	//name = "Revenge", 
	Function = 0x081, FunctionAsString = "081", 
	BaseDamage = 60,
	Type = Types.FIGHTING,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = -4,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "An attack move that inflicts double the damage if the user has been hurt by the foe in the same turn."
},
new MoveDataDex() {
	num = 105,
	ID = Moves.ROLLING_KICK, 
	//name = "Rolling Kick", 
	Function = 0x00F, FunctionAsString = "00F", 
	BaseDamage = 60,
	Type = Types.FIGHTING,
	Category = Category.PHYSICAL,
	Accuracy = 85,
	PP = 15,
	Effects = 30,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user lashes out with a quick, spinning kick. It may also make the target flinch."
},
new MoveDataDex() {
	num = 106,
	ID = Moves.WAKE_UP_SLAP, 
	//name = "Wake-Up Slap", 
	Function = 0x07D, FunctionAsString = "07D", 
	BaseDamage = 60,
	Type = Types.FIGHTING,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "This attack inflicts big damage on a sleeping target. It also wakes the target up, however."
},
new MoveDataDex() {
	num = 107,
	ID = Moves.KARATE_CHOP, 
	//name = "Karate Chop", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 50,
	Type = Types.FIGHTING,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 25,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true,bite: true ) { }
	//,description = "The target is attacked with a sharp chop. Critical hits land more easily."
},
new MoveDataDex() {
	num = 108,
	ID = Moves.MACH_PUNCH, 
	//name = "Mach Punch", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 40,
	Type = Types.FIGHTING,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 30,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 1,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true,punch: true ) { }
	//,description = "The user throws a punch at blinding speed. It is certain to strike first."
},
new MoveDataDex() {
	num = 109,
	ID = Moves.ROCK_SMASH, 
	//name = "Rock Smash", 
	Function = 0x043, FunctionAsString = "043", 
	BaseDamage = 40,
	Type = Types.FIGHTING,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 15,
	Effects = 50,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true ) { }
	//,description = "The user attacks with a punch that can shatter a rock. It may also lower the foe's Defense stat."
},
new MoveDataDex() {
	num = 110,
	ID = Moves.STORM_THROW, 
	//name = "Storm Throw", 
	Function = 0x0A0, FunctionAsString = "0A0", 
	BaseDamage = 40,
	Type = Types.FIGHTING,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user strikes the target with a fierce blow. This attack always results in a critical hit."
},
new MoveDataDex() {
	num = 111,
	ID = Moves.VACUUM_WAVE, 
	//name = "Vacuum Wave", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 40,
	Type = Types.FIGHTING,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 30,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 1,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user whirls its fists to send a wave of pure vacuum at the target. This move always goes first."
},
new MoveDataDex() {
	num = 112,
	ID = Moves.DOUBLE_KICK, 
	//name = "Double Kick", 
	Function = 0x0BD, FunctionAsString = "0BD", 
	BaseDamage = 30,
	Type = Types.FIGHTING,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 30,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The target is quickly kicked twice in succession using both feet."
},
new MoveDataDex() {
	num = 113,
	ID = Moves.ARM_THRUST, 
	//name = "Arm Thrust", 
	Function = 0x0C0, FunctionAsString = "0C0", 
	BaseDamage = 15,
	Type = Types.FIGHTING,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user looses a flurry of open-palmed arm thrusts that hit two to five times in a row."
},
new MoveDataDex() {
	num = 114,
	ID = Moves.TRIPLE_KICK, 
	//name = "Triple Kick", 
	Function = 0x0BF, FunctionAsString = "0BF", 
	BaseDamage = 10,
	Type = Types.FIGHTING,
	Category = Category.PHYSICAL,
	Accuracy = 90,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "A consecutive three-kick attack that becomes more powerful with each successive hit."
},
new MoveDataDex() {
	num = 115,
	ID = Moves.COUNTER, 
	//name = "Counter", 
	Function = 0x071, FunctionAsString = "071", 
	BaseDamage = 1,
	Type = Types.FIGHTING,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 20,
	Effects = 0,
	Target = Target.NoTarget,
	Priority = -5,
	Flags = new Flags( contact: true,protect: true ) { }
	//,description = "A retaliation move that counters any physical attack, inflicting double the damage taken."
},
new MoveDataDex() {
	num = 116,
	ID = Moves.FINAL_GAMBIT, 
	//name = "Final Gambit", 
	Function = 0x0E1, FunctionAsString = "0E1", 
	BaseDamage = 1,
	Type = Types.FIGHTING,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true ) { }
	//,description = "The user risks all to attack the foe. The user faints but does damage equal to its HP."
},
new MoveDataDex() {
	num = 117,
	ID = Moves.LOW_KICK, 
	//name = "Low Kick", 
	Function = 0x09A, FunctionAsString = "09A", 
	BaseDamage = 1,
	Type = Types.FIGHTING,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "A powerful low kick that makes the foe fall over. It inflicts greater damage on heavier foes."
},
new MoveDataDex() {
	num = 118,
	ID = Moves.REVERSAL, 
	//name = "Reversal", 
	Function = 0x098, FunctionAsString = "098", 
	BaseDamage = 1,
	Type = Types.FIGHTING,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "An all-out attack that becomes more powerful the less HP the user has."
},
new MoveDataDex() {
	num = 119,
	ID = Moves.SEISMIC_TOSS, 
	//name = "Seismic Toss", 
	Function = 0x06D, FunctionAsString = "06D", 
	BaseDamage = 1,
	Type = Types.FIGHTING,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The target is thrown using the power of gravity. It inflicts damage equal to the user's level."
},
new MoveDataDex() {
	num = 120,
	ID = Moves.BULK_UP, 
	//name = "Bulk Up", 
	Function = 0x024, FunctionAsString = "024", 
	BaseDamage = 0,
	Type = Types.FIGHTING,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 20,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user tenses its muscles to bulk up its body, boosting both its Attack and Defense stats."
},
new MoveDataDex() {
	num = 121,
	ID = Moves.DETECT, 
	//name = "Detect", 
	Function = 0x0AA, FunctionAsString = "0AA", 
	BaseDamage = 0,
	Type = Types.FIGHTING,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 5,
	Effects = 0,
	Target = Target.User,
	Priority = 3,
	Flags = new Flags(  ) { } 
	//,description = "It enables the user to evade all attacks. Its chance of failing rises if it is used in succession."
},
new MoveDataDex() {
	num = 122,
	ID = Moves.QUICK_GUARD, 
	//name = "Quick Guard", 
	Function = 0x0AB, FunctionAsString = "0AB", 
	BaseDamage = 0,
	Type = Types.FIGHTING,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 15,
	Effects = 0,
	Target = Target.UserSide,
	Priority = 3,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user protects itself and its allies from priority moves. If may fail if used in succession."
},
new MoveDataDex() {
	num = 123,
	ID = Moves.V_CREATE, 
	//name = "V-create", 
	Function = 0x03D, FunctionAsString = "03D", 
	BaseDamage = 180,
	Type = Types.FIRE,
	Category = Category.PHYSICAL,
	Accuracy = 95,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "With a fiery forehead, the user hurls itself at the foe. It lowers the user's Defense, Sp. Def, and Speed."
},
new MoveDataDex() {
	num = 124,
	ID = Moves.BLAST_BURN, 
	//name = "Blast Burn", 
	Function = 0x0C2, FunctionAsString = "0C2", 
	BaseDamage = 150,
	Type = Types.FIRE,
	Category = Category.SPECIAL,
	Accuracy = 90,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The target is razed by a fiery explosion. The user must rest on the next turn, however."
},
new MoveDataDex() {
	num = 125,
	ID = Moves.ERUPTION, 
	//name = "Eruption", 
	Function = 0x08B, FunctionAsString = "08B", 
	BaseDamage = 150,
	Type = Types.FIRE,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 5,
	Effects = 0,
	Target = Target.AllOpposing,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user attacks in an explosive fury. The lower the user's HP, the less powerful this attack becomes."
},
new MoveDataDex() {
	num = 126,
	ID = Moves.OVERHEAT, 
	//name = "Overheat", 
	Function = 0x03F, FunctionAsString = "03F", 
	BaseDamage = 140,
	Type = Types.FIRE,
	Category = Category.SPECIAL,
	Accuracy = 90,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user attacks the target at full power. The attack's recoil sharply reduces the user's Sp. Atk stat."
},
new MoveDataDex() {
	num = 127,
	ID = Moves.BLUE_FLARE, 
	//name = "Blue Flare", 
	Function = 0x00A, FunctionAsString = "00A", 
	BaseDamage = 130,
	Type = Types.FIRE,
	Category = Category.SPECIAL,
	Accuracy = 85,
	PP = 5,
	Effects = 20,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user attacks by engulfing the foe in a beautiful, yet intense, blue flame. It may also burn the foe."
},
new MoveDataDex() {
	num = 128,
	ID = Moves.FIRE_BLAST, 
	//name = "Fire Blast", 
	Function = 0x00A, FunctionAsString = "00A", 
	BaseDamage = 120,
	Type = Types.FIRE,
	Category = Category.SPECIAL,
	Accuracy = 85,
	PP = 5,
	Effects = 10,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The foe is attacked with an intense blast of all-consuming fire. It may also leave the target with a burn."
},
new MoveDataDex() {
	num = 129,
	ID = Moves.FLARE_BLITZ, 
	//name = "Flare Blitz", 
	Function = 0x0FE, FunctionAsString = "0FE", 
	BaseDamage = 120,
	Type = Types.FIRE,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 15,
	Effects = 10,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true,defrost: true ) { }
	//,description = "The user cloaks itself in fire and charges at the foe. The user also takes damage and may burn the target."
},
new MoveDataDex() {
	num = 130,
	ID = Moves.MAGMA_STORM, 
	//name = "Magma Storm", 
	Function = 0x0CF, FunctionAsString = "0CF", 
	BaseDamage = 120,
	Type = Types.FIRE,
	Category = Category.SPECIAL,
	Accuracy = 75,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The target becomes trapped within a maelstrom of fire that rages for four to five turns."
},
new MoveDataDex() {
	num = 131,
	ID = Moves.FUSION_FLARE, 
	//name = "Fusion Flare", 
	Function = 0x07A, FunctionAsString = "07A", 
	BaseDamage = 100,
	Type = Types.FIRE,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true,defrost: true ) { }
	//,description = "The user brings down a giant flame. It does more damage if influenced by an enormous thunderbolt."
},
new MoveDataDex() {
	num = 132,
	ID = Moves.HEAT_WAVE, 
	//name = "Heat Wave", 
	Function = 0x00A, FunctionAsString = "00A", 
	BaseDamage = 100,
	Type = Types.FIRE,
	Category = Category.SPECIAL,
	Accuracy = 90,
	PP = 10,
	Effects = 10,
	Target = Target.AllOpposing,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The user attacks by exhaling hot breath on the opposing team. It may also leave targets with a burn."
},
new MoveDataDex() {
	num = 133,
	ID = Moves.INFERNO, 
	//name = "Inferno", 
	Function = 0x00A, FunctionAsString = "00A", 
	BaseDamage = 100,
	Type = Types.FIRE,
	Category = Category.SPECIAL,
	Accuracy = 50,
	PP = 5,
	Effects = 100,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The user attacks by engulfing the target in an intense fire. It leaves the target with a burn."
},
new MoveDataDex() {
	num = 134,
	ID = Moves.SACRED_FIRE, 
	//name = "Sacred Fire", 
	Function = 0x00A, FunctionAsString = "00A", 
	BaseDamage = 100,
	Type = Types.FIRE,
	Category = Category.PHYSICAL,
	Accuracy = 95,
	PP = 5,
	Effects = 50,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,defrost: true ) { }
	//,description = "The target is razed with a mystical fire of great intensity. It may also leave the target with a burn."
},
new MoveDataDex() {
	num = 135,
	ID = Moves.SEARING_SHOT, 
	//name = "Searing Shot", 
	Function = 0x00A, FunctionAsString = "00A", 
	BaseDamage = 100,
	Type = Types.FIRE,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 5,
	Effects = 30,
	Target = Target.AllNonUsers,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "An inferno of scarlet flames torches everything around the user. It may leave the foe with a burn."
},
new MoveDataDex() {
	num = 136,
	ID = Moves.FLAMETHROWER, 
	//name = "Flamethrower", 
	Function = 0x00A, FunctionAsString = "00A", 
	BaseDamage = 95,
	Type = Types.FIRE,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 15,
	Effects = 10,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The target is scorched with an intense blast of fire. It may also leave the target with a burn."
},
new MoveDataDex() {
	num = 137,
	ID = Moves.BLAZE_KICK, 
	//name = "Blaze Kick", 
	Function = 0x00A, FunctionAsString = "00A", 
	BaseDamage = 85,
	Type = Types.FIRE,
	Category = Category.PHYSICAL,
	Accuracy = 90,
	PP = 10,
	Effects = 10,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,bite: true ) { }
	//,description = "The user launches a kick with a high critical-hit ratio. It may also leave the target with a burn."
},
new MoveDataDex() {
	num = 138,
	ID = Moves.FIERY_DANCE, 
	//name = "Fiery Dance", 
	Function = 0x020, FunctionAsString = "020", 
	BaseDamage = 80,
	Type = Types.FIRE,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 10,
	Effects = 50,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "Cloaked in flames, the user dances and flaps its wings. It may also raise the user's Sp. Atk stat."
},
new MoveDataDex() {
	num = 139,
	ID = Moves.LAVA_PLUME, 
	//name = "Lava Plume", 
	Function = 0x00A, FunctionAsString = "00A", 
	BaseDamage = 80,
	Type = Types.FIRE,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 15,
	Effects = 30,
	Target = Target.AllNonUsers,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "An inferno of scarlet flames torches everything around the user. It may leave targets with a burn."
},
new MoveDataDex() {
	num = 140,
	ID = Moves.FIRE_PUNCH, 
	//name = "Fire Punch", 
	Function = 0x00A, FunctionAsString = "00A", 
	BaseDamage = 75,
	Type = Types.FIRE,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 15,
	Effects = 10,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,punch: true ) { }
	//,description = "The target is punched with a fiery fist. It may leave the target with a burn."
},
new MoveDataDex() {
	num = 141,
	ID = Moves.FLAME_BURST, 
	//name = "Flame Burst", 
	Function = 0x074, FunctionAsString = "074", 
	BaseDamage = 70,
	Type = Types.FIRE,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user attacks the foe with a bursting flame. It also damages Pokémon next to the target."
},
new MoveDataDex() {
	num = 142,
	ID = Moves.FIRE_FANG, 
	//name = "Fire Fang", 
	Function = 0x00B, FunctionAsString = "00B", 
	BaseDamage = 65,
	Type = Types.FIRE,
	Category = Category.PHYSICAL,
	Accuracy = 95,
	PP = 15,
	Effects = 10,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user bites with flame-cloaked fangs. It may also make the target flinch or leave it burned."
},
new MoveDataDex() {
	num = 143,
	ID = Moves.FLAME_WHEEL, 
	//name = "Flame Wheel", 
	Function = 0x00A, FunctionAsString = "00A", 
	BaseDamage = 60,
	Type = Types.FIRE,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 25,
	Effects = 10,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,defrost: true ) { }
	//,description = "The user cloaks itself in fire and charges at the target. It may also leave the target with a burn."
},
new MoveDataDex() {
	num = 144,
	ID = Moves.FIRE_PLEDGE, 
	//name = "Fire Pledge", 
	Function = 0x107, FunctionAsString = "107", 
	BaseDamage = 50,
	Type = Types.FIRE,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "A column of fire hits opposing Pokémon. When used with its Grass equivalent, it makes a sea of fire."
},
new MoveDataDex() {
	num = 145,
	ID = Moves.FLAME_CHARGE, 
	//name = "Flame Charge", 
	Function = 0x01F, FunctionAsString = "01F", 
	BaseDamage = 50,
	Type = Types.FIRE,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 20,
	Effects = 100,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user cloaks itself with flame and attacks. Building up more power, it raises the user's Speed stat."
},
new MoveDataDex() {
	num = 146,
	ID = Moves.EMBER, 
	//name = "Ember", 
	Function = 0x00A, FunctionAsString = "00A", 
	BaseDamage = 40,
	Type = Types.FIRE,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 25,
	Effects = 10,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The target is attacked with small flames. It may also leave the target with a burn."
},
new MoveDataDex() {
	num = 147,
	ID = Moves.FIRE_SPIN, 
	//name = "Fire Spin", 
	Function = 0x0CF, FunctionAsString = "0CF", 
	BaseDamage = 35,
	Type = Types.FIRE,
	Category = Category.SPECIAL,
	Accuracy = 85,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The target becomes trapped within a fierce vortex of fire that rages for four to five turns."
},
new MoveDataDex() {
	num = 148,
	ID = Moves.INCINERATE, 
	//name = "Incinerate", 
	Function = 0x0F5, FunctionAsString = "0F5", 
	BaseDamage = 30,
	Type = Types.FIRE,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.AllOpposing,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The user attacks the foe with fire. If the target is holding a Berry, it becomes burnt up and unusable."
},
new MoveDataDex() {
	num = 149,
	ID = Moves.HEAT_CRASH, 
	//name = "Heat Crash", 
	Function = 0x09B, FunctionAsString = "09B", 
	BaseDamage = 1,
	Type = Types.FIRE,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user slams the foe with its flaming body. The heavier the user is, the greater the damage."
},
new MoveDataDex() {
	num = 150,
	ID = Moves.SUNNY_DAY, 
	//name = "Sunny Day", 
	Function = 0x0FF, FunctionAsString = "0FF", 
	BaseDamage = 0,
	Type = Types.FIRE,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 5,
	Effects = 0,
	Target = Target.BothSides,
	Priority = 0,
	Flags = new Flags(  ) { } 
	//,description = "The user intensifies the sun for five turns, powering up Fire-type moves."
},
new MoveDataDex() {
	num = 151,
	ID = Moves.WILL_O_WISP, 
	//name = "Will-O-Wisp", 
	Function = 0x00A, FunctionAsString = "00A", 
	BaseDamage = 0,
	Type = Types.FIRE,
	Category = Category.STATUS,
	Accuracy = 75,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "The user shoots a sinister, bluish-white flame at the target to inflict a burn."
},
new MoveDataDex() {
	num = 152,
	ID = Moves.SKY_ATTACK, 
	//name = "Sky Attack", 
	Function = 0x0C7, FunctionAsString = "0C7", 
	BaseDamage = 140,
	Type = Types.FLYING,
	Category = Category.PHYSICAL,
	Accuracy = 90,
	PP = 5,
	Effects = 30,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true,bite: true ) { }
	//,description = "A second-turn attack move where critical hits land more easily. It may also make the target flinch."
},
new MoveDataDex() {
	num = 153,
	ID = Moves.BRAVE_BIRD, 
	//name = "Brave Bird", 
	Function = 0x0FB, FunctionAsString = "0FB", 
	BaseDamage = 120,
	Type = Types.FLYING,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user tucks in its wings and charges from a low altitude. The user also takes serious damage."
},
new MoveDataDex() {
	num = 154,
	ID = Moves.HURRICANE, 
	//name = "Hurricane", 
	Function = 0x015, FunctionAsString = "015", 
	BaseDamage = 120,
	Type = Types.FLYING,
	Category = Category.SPECIAL,
	Accuracy = 70,
	PP = 10,
	Effects = 30,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user wraps its foe in a fierce wind that flies up into the sky. It may also confuse the foe."
},
new MoveDataDex() {
	num = 155,
	ID = Moves.AEROBLAST, 
	//name = "Aeroblast", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 100,
	Type = Types.FLYING,
	Category = Category.SPECIAL,
	Accuracy = 95,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true,bite: true ) { }
	//,description = "A vortex of air is shot at the target to inflict damage. Critical hits land more easily."
},
new MoveDataDex() {
	num = 156,
	ID = Moves.FLY, 
	//name = "Fly", 
	Function = 0x0C9, FunctionAsString = "0C9", 
	BaseDamage = 90,
	Type = Types.FLYING,
	Category = Category.PHYSICAL,
	Accuracy = 95,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true,powder: true ) { }
	//,description = "The user soars, then strikes on the second turn. It can also be used for flying to any familiar town."
},
new MoveDataDex() {
	num = 157,
	ID = Moves.BOUNCE, 
	//name = "Bounce", 
	Function = 0x0CC, FunctionAsString = "0CC", 
	BaseDamage = 85,
	Type = Types.FLYING,
	Category = Category.PHYSICAL,
	Accuracy = 85,
	PP = 5,
	Effects = 30,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true,powder: true ) { }
	//,description = "The user bounces up high, then drops on the foe on the second turn. It may also paralyze the foe."
},
new MoveDataDex() {
	num = 158,
	ID = Moves.DRILL_PECK, 
	//name = "Drill Peck", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 80,
	Type = Types.FLYING,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "A corkscrewing attack with the sharp beak acting as a drill."
},
new MoveDataDex() {
	num = 159,
	ID = Moves.AIR_SLASH, 
	//name = "Air Slash", 
	Function = 0x00F, FunctionAsString = "00F", 
	BaseDamage = 75,
	Type = Types.FLYING,
	Category = Category.SPECIAL,
	Accuracy = 95,
	PP = 20,
	Effects = 30,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user attacks with a blade of air that slices even the sky. It may also make the target flinch."
},
new MoveDataDex() {
	num = 160,
	ID = Moves.AERIAL_ACE, 
	//name = "Aerial Ace", 
	Function = 0x0A5, FunctionAsString = "0A5", 
	BaseDamage = 60,
	Type = Types.FLYING,
	Category = Category.PHYSICAL,
	Accuracy = 0,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user confounds the foe with speed, then slashes. The attack lands without fail."
},
new MoveDataDex() {
	num = 161,
	ID = Moves.CHATTER, 
	//name = "Chatter", 
	Function = 0x014, FunctionAsString = "014", 
	BaseDamage = 60,
	Type = Types.FLYING,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 20,
	Effects = 100,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,sound: true ) { }
	//,description = "The user attacks using a sound wave based on words it has learned. It may also confuse the target."
},
new MoveDataDex() {
	num = 162,
	ID = Moves.PLUCK, 
	//name = "Pluck", 
	Function = 0x0F4, FunctionAsString = "0F4", 
	BaseDamage = 60,
	Type = Types.FLYING,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user pecks the target. If the target is holding a Berry, the user eats it and gains its effect."
},
new MoveDataDex() {
	num = 163,
	ID = Moves.SKY_DROP, 
	//name = "Sky Drop", 
	Function = 0x0CE, FunctionAsString = "0CE", 
	BaseDamage = 60,
	Type = Types.FLYING,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true,powder: true ) { }
	//,description = "The user takes the foe into the sky, then drops it on the next turn. The foe cannot attack while airborne."
},
new MoveDataDex() {
	num = 164,
	ID = Moves.WING_ATTACK, 
	//name = "Wing Attack", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 60,
	Type = Types.FLYING,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 35,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The target is struck with large, imposing wings spread wide to inflict damage."
},
new MoveDataDex() {
	num = 165,
	ID = Moves.ACROBATICS, 
	//name = "Acrobatics", 
	Function = 0x086, FunctionAsString = "086", 
	BaseDamage = 55,
	Type = Types.FLYING,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user nimbly strikes the foe. This attack does more damage if the user is not holding an item."
},
new MoveDataDex() {
	num = 166,
	ID = Moves.AIR_CUTTER, 
	//name = "Air Cutter", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 55,
	Type = Types.FLYING,
	Category = Category.SPECIAL,
	Accuracy = 95,
	PP = 25,
	Effects = 0,
	Target = Target.AllOpposing,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true,bite: true ) { }
	//,description = "The user launches razor-like wind to slash the opposing team. Critical hits land more easily."
},
new MoveDataDex() {
	num = 167,
	ID = Moves.GUST, 
	//name = "Gust", 
	Function = 0x077, FunctionAsString = "077", 
	BaseDamage = 40,
	Type = Types.FLYING,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 35,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "A gust of wind is whipped up by wings and launched at the target to inflict damage."
},
new MoveDataDex() {
	num = 168,
	ID = Moves.PECK, 
	//name = "Peck", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 35,
	Type = Types.FLYING,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 35,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The target is jabbed with a sharply pointed beak or horn."
},
new MoveDataDex() {
	num = 169,
	ID = Moves.DEFOG, 
	//name = "Defog", 
	Function = 0x049, FunctionAsString = "049", 
	BaseDamage = 0,
	Type = Types.FLYING,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "A strong wind blows away the foe's obstacles such as Light Screen. It also lowers their evasion."
},
new MoveDataDex() {
	num = 170,
	ID = Moves.FEATHER_DANCE, 
	//name = "FeatherDance", 
	Function = 0x04B, FunctionAsString = "04B", 
	BaseDamage = 0,
	Type = Types.FLYING,
	Category = Category.STATUS,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "The user covers the target's body with a mass of down that harshly lowers its Attack stat."
},
new MoveDataDex() {
	num = 171,
	ID = Moves.MIRROR_MOVE, 
	//name = "Mirror Move", 
	Function = 0x0AE, FunctionAsString = "0AE", 
	BaseDamage = 0,
	Type = Types.FLYING,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags(  ) { } 
	//,description = "The user counters the target by mimicking the target's last move."
},
new MoveDataDex() {
	num = 172,
	ID = Moves.ROOST, 
	//name = "Roost", 
	Function = 0x0D6, FunctionAsString = "0D6", 
	BaseDamage = 0,
	Type = Types.FLYING,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true,bite: true ) { }
	//,description = "The user lands and rests its body. It restores the user's HP by up to half of its max HP."
},
new MoveDataDex() {
	num = 173,
	ID = Moves.TAILWIND, 
	//name = "Tailwind", 
	Function = 0x05B, FunctionAsString = "05B", 
	BaseDamage = 0,
	Type = Types.FLYING,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 30,
	Effects = 0,
	Target = Target.UserSide,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user whips up a turbulent whirlwind that ups the Speed of all party Pokémon for four turns."
},
new MoveDataDex() {
	num = 174,
	ID = Moves.SHADOW_FORCE, 
	//name = "Shadow Force", 
	Function = 0x0CD, FunctionAsString = "0CD", 
	BaseDamage = 120,
	Type = Types.GHOST,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,mirror: true,flinch: true ) { }
	//,description = "The user disappears, then strikes the foe on the second turn. It hits even if the foe protects itself."
},
new MoveDataDex() {
	num = 175,
	ID = Moves.SHADOW_BALL, 
	//name = "Shadow Ball", 
	Function = 0x046, FunctionAsString = "046", 
	BaseDamage = 80,
	Type = Types.GHOST,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 15,
	Effects = 20,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The user hurls a shadowy blob at the target. It may also lower the target's Sp. Def stat."
},
new MoveDataDex() {
	num = 176,
	ID = Moves.SHADOW_CLAW, 
	//name = "Shadow Claw", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 70,
	Type = Types.GHOST,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true,bite: true ) { }
	//,description = "The user slashes with a sharp claw made from shadows. Critical hits land more easily."
},
new MoveDataDex() {
	num = 177,
	ID = Moves.OMINOUS_WIND, 
	//name = "Ominous Wind", 
	Function = 0x02D, FunctionAsString = "02D", 
	BaseDamage = 60,
	Type = Types.GHOST,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 5,
	Effects = 10,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user blasts the target with a gust of repulsive wind. It may also raise all the user's stats at once."
},
new MoveDataDex() {
	num = 178,
	ID = Moves.SHADOW_PUNCH, 
	//name = "Shadow Punch", 
	Function = 0x0A5, FunctionAsString = "0A5", 
	BaseDamage = 60,
	Type = Types.GHOST,
	Category = Category.PHYSICAL,
	Accuracy = 0,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true,punch: true ) { }
	//,description = "The user throws a punch from the shadows. The punch lands without fail."
},
new MoveDataDex() {
	num = 179,
	ID = Moves.HEX, 
	//name = "Hex", 
	Function = 0x07F, FunctionAsString = "07F", 
	BaseDamage = 50,
	Type = Types.GHOST,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "This relentless attack does massive damage to a target affected by status problems."
},
new MoveDataDex() {
	num = 180,
	ID = Moves.SHADOW_SNEAK, 
	//name = "Shadow Sneak", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 40,
	Type = Types.GHOST,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 30,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 1,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user extends its shadow and attacks the target from behind. This move always goes first."
},
new MoveDataDex() {
	num = 181,
	ID = Moves.ASTONISH, 
	//name = "Astonish", 
	Function = 0x00F, FunctionAsString = "00F", 
	BaseDamage = 30,
	Type = Types.GHOST,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 15,
	Effects = 30,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true ) { }
	//,description = "The user attacks the target while shouting in a startling fashion. It may also make the target flinch."
},
new MoveDataDex() {
	num = 182,
	ID = Moves.LICK, 
	//name = "Lick", 
	Function = 0x007, FunctionAsString = "007", 
	BaseDamage = 20,
	Type = Types.GHOST,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 30,
	Effects = 30,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true ) { }
	//,description = "The target is licked with a long tongue, causing damage. It may also leave the target with paralysis."
},
new MoveDataDex() {
	num = 183,
	ID = Moves.NIGHT_SHADE, 
	//name = "Night Shade", 
	Function = 0x06D, FunctionAsString = "06D", 
	BaseDamage = 1,
	Type = Types.GHOST,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user makes the foe see a frightening mirage. It inflicts damage matching the user's level."
},
new MoveDataDex() {
	num = 184,
	ID = Moves.CONFUSE_RAY, 
	//name = "Confuse Ray", 
	Function = 0x013, FunctionAsString = "013", 
	BaseDamage = 0,
	Type = Types.GHOST,
	Category = Category.STATUS,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "The target is exposed to a sinister ray that triggers confusion."
},
new MoveDataDex() {
	num = 185,
	ID = Moves.CURSE, 
	//name = "Curse", 
	Function = 0x10D, FunctionAsString = "10D", 
	BaseDamage = 0,
	Type = Types.GHOST,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags(  ) { } 
	//,description = "A move that works differently for the Ghost type than for all the other types."
},
new MoveDataDex() {
	num = 186,
	ID = Moves.DESTINY_BOND, 
	//name = "Destiny Bond", 
	Function = 0x0E7, FunctionAsString = "0E7", 
	BaseDamage = 0,
	Type = Types.GHOST,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 5,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags(  ) { } 
	//,description = "When this move is used, if the user faints, the foe that landed the knockout hit also faints."
},
new MoveDataDex() {
	num = 187,
	ID = Moves.GRUDGE, 
	//name = "Grudge", 
	Function = 0x0E6, FunctionAsString = "0E6", 
	BaseDamage = 0,
	Type = Types.GHOST,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 5,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags(  ) { } 
	//,description = "If the user faints, the user's grudge fully depletes the PP of the foe's move that knocked it out."
},
new MoveDataDex() {
	num = 188,
	ID = Moves.NIGHTMARE, 
	//name = "Nightmare", 
	Function = 0x10F, FunctionAsString = "10F", 
	BaseDamage = 0,
	Type = Types.GHOST,
	Category = Category.STATUS,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "A sleeping target sees a nightmare that inflicts some damage every turn."
},
new MoveDataDex() {
	num = 189,
	ID = Moves.SPITE, 
	//name = "Spite", 
	Function = 0x10E, FunctionAsString = "10E", 
	BaseDamage = 0,
	Type = Types.GHOST,
	Category = Category.STATUS,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "The user unleashes its grudge on the move last used by the target by cutting 4 PP from it."
},
new MoveDataDex() {
	num = 190,
	ID = Moves.FRENZY_PLANT, 
	//name = "Frenzy Plant", 
	Function = 0x0C2, FunctionAsString = "0C2", 
	BaseDamage = 150,
	Type = Types.GRASS,
	Category = Category.SPECIAL,
	Accuracy = 90,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user slams the target with an enormous tree. The user can't move on the next turn."
},
new MoveDataDex() {
	num = 191,
	ID = Moves.LEAF_STORM, 
	//name = "Leaf Storm", 
	Function = 0x03F, FunctionAsString = "03F", 
	BaseDamage = 140,
	Type = Types.GRASS,
	Category = Category.SPECIAL,
	Accuracy = 90,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "A storm of sharp is whipped up. The attack's recoil harshly reduces the user's Sp. Atk stat."
},
new MoveDataDex() {
	num = 192,
	ID = Moves.PETAL_DANCE, 
	//name = "Petal Dance", 
	Function = 0x0D2, FunctionAsString = "0D2", 
	BaseDamage = 120,
	Type = Types.GRASS,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.RandomOpposing,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user attacks by scattering petals for two to three turns. The user then becomes confused."
},
new MoveDataDex() {
	num = 193,
	ID = Moves.POWER_WHIP, 
	//name = "Power Whip", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 120,
	Type = Types.GRASS,
	Category = Category.PHYSICAL,
	Accuracy = 85,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user violently whirls its vines or tentacles to harshly lash the target."
},
new MoveDataDex() {
	num = 194,
	ID = Moves.SEED_FLARE, 
	//name = "Seed Flare", 
	Function = 0x04F, FunctionAsString = "04F", 
	BaseDamage = 120,
	Type = Types.GRASS,
	Category = Category.SPECIAL,
	Accuracy = 85,
	PP = 5,
	Effects = 40,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user generates a shock wave from within its body. It may harshly lower the target's Sp. Def."
},
new MoveDataDex() {
	num = 195,
	ID = Moves.SOLAR_BEAM, 
	//name = "SolarBeam", 
	Function = 0x0C4, FunctionAsString = "0C4", 
	BaseDamage = 120,
	Type = Types.GRASS,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "A two-turn attack. The user gathers light, then blasts a bundled beam on the second turn."
},
new MoveDataDex() {
	num = 196,
	ID = Moves.WOOD_HAMMER, 
	//name = "Wood Hammer", 
	Function = 0x0FB, FunctionAsString = "0FB", 
	BaseDamage = 120,
	Type = Types.GRASS,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user slams its rugged body into the target to attack. The user also sustains serious damage."
},
new MoveDataDex() {
	num = 197,
	ID = Moves.LEAF_BLADE, 
	//name = "Leaf Blade", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 90,
	Type = Types.GRASS,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true,bite: true ) { }
	//,description = "The user handles a sharp leaf like a sword and attacks by slashing. It has a high critical-hit ratio."
},
new MoveDataDex() {
	num = 198,
	ID = Moves.ENERGY_BALL, 
	//name = "Energy Ball", 
	Function = 0x046, FunctionAsString = "046", 
	BaseDamage = 80,
	Type = Types.GRASS,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 10,
	Effects = 10,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The user draws power from nature and fires it at the target. It may also lower the target's Sp. Def."
},
new MoveDataDex() {
	num = 199,
	ID = Moves.SEED_BOMB, 
	//name = "Seed Bomb", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 80,
	Type = Types.GRASS,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user slams a barrage of hard-shelled seeds down on the target from above."
},
new MoveDataDex() {
	num = 200,
	ID = Moves.GIGA_DRAIN, 
	//name = "Giga Drain", 
	Function = 0x0DD, FunctionAsString = "0DD", 
	BaseDamage = 75,
	Type = Types.GRASS,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "A nutrient-draining attack. The user's HP is restored by half the damage taken by the target."
},
new MoveDataDex() {
	num = 201,
	ID = Moves.HORN_LEECH, 
	//name = "Horn Leech", 
	Function = 0x0DD, FunctionAsString = "0DD", 
	BaseDamage = 75,
	Type = Types.GRASS,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true ) { }
	//,description = "The user drains the foe's energy with its horns. The user's HP is restored by half the damage inflicted."
},
new MoveDataDex() {
	num = 202,
	ID = Moves.LEAF_TORNADO, 
	//name = "Leaf Tornado", 
	Function = 0x047, FunctionAsString = "047", 
	BaseDamage = 65,
	Type = Types.GRASS,
	Category = Category.SPECIAL,
	Accuracy = 90,
	PP = 10,
	Effects = 30,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The user attacks its foe by encircling it in sharp leaves. This attack may also lower the foe's accuracy."
},
new MoveDataDex() {
	num = 203,
	ID = Moves.MAGICAL_LEAF, 
	//name = "Magical Leaf", 
	Function = 0x0A5, FunctionAsString = "0A5", 
	BaseDamage = 60,
	Type = Types.GRASS,
	Category = Category.SPECIAL,
	Accuracy = 0,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user scatters curious leaves that chase the target. This attack will not miss."
},
new MoveDataDex() {
	num = 204,
	ID = Moves.NEEDLE_ARM, 
	//name = "Needle Arm", 
	Function = 0x00F, FunctionAsString = "00F", 
	BaseDamage = 60,
	Type = Types.GRASS,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 15,
	Effects = 30,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true ) { }
	//,description = "The user attacks by wildly swinging its thorny arms. It may also make the target flinch."
},
new MoveDataDex() {
	num = 205,
	ID = Moves.RAZOR_LEAF, 
	//name = "Razor Leaf", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 55,
	Type = Types.GRASS,
	Category = Category.PHYSICAL,
	Accuracy = 95,
	PP = 25,
	Effects = 0,
	Target = Target.AllOpposing,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true,bite: true ) { }
	//,description = "Sharp-edged leaves are launched to slash at the opposing team. Critical hits land more easily."
},
new MoveDataDex() {
	num = 206,
	ID = Moves.GRASS_PLEDGE, 
	//name = "Grass Pledge", 
	Function = 0x106, FunctionAsString = "106", 
	BaseDamage = 50,
	Type = Types.GRASS,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "A column of grass hits the foes. When used with its water equivalent, it creates a vast swamp."
},
new MoveDataDex() {
	num = 207,
	ID = Moves.MEGA_DRAIN, 
	//name = "Mega Drain", 
	Function = 0x0DD, FunctionAsString = "0DD", 
	BaseDamage = 40,
	Type = Types.GRASS,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "A nutrient-draining attack. The user's HP is restored by half the damage taken by the target."
},
new MoveDataDex() {
	num = 208,
	ID = Moves.VINE_WHIP, 
	//name = "Vine Whip", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 35,
	Type = Types.GRASS,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The target is struck with slender, whiplike vines to inflict damage."
},
new MoveDataDex() {
	num = 209,
	ID = Moves.BULLET_SEED, 
	//name = "Bullet Seed", 
	Function = 0x0C0, FunctionAsString = "0C0", 
	BaseDamage = 25,
	Type = Types.GRASS,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 30,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user forcefully shoots seeds at the target. Two to five seeds are shot in rapid succession."
},
new MoveDataDex() {
	num = 210,
	ID = Moves.ABSORB, 
	//name = "Absorb", 
	Function = 0x0DD, FunctionAsString = "0DD", 
	BaseDamage = 20,
	Type = Types.GRASS,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 25,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "A nutrient-draining attack. The user's HP is restored by half the damage taken by the target."
},
new MoveDataDex() {
	num = 211,
	ID = Moves.GRASS_KNOT, 
	//name = "Grass Knot", 
	Function = 0x09A, FunctionAsString = "09A", 
	BaseDamage = 1,
	Type = Types.GRASS,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user snares the target with grass and trips it. The heavier the target, the greater the damage."
},
new MoveDataDex() {
	num = 212,
	ID = Moves.AROMATHERAPY, 
	//name = "Aromatherapy", 
	Function = 0x019, FunctionAsString = "019", 
	BaseDamage = 0,
	Type = Types.GRASS,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 5,
	Effects = 0,
	Target = Target.UserSide,
	Priority = 0,
	Flags = new Flags( snatch: true,sound: true ) { }
	//,description = "The user releases a soothing scent that heals all status problems affecting the user's party."
},
new MoveDataDex() {
	num = 213,
	ID = Moves.COTTON_GUARD, 
	//name = "Cotton Guard", 
	Function = 0x038, FunctionAsString = "038", 
	BaseDamage = 0,
	Type = Types.GRASS,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user protects itself by wrapping its body in soft cotton, drastically raising its Defense stat."
},
new MoveDataDex() {
	num = 214,
	ID = Moves.COTTON_SPORE, 
	//name = "Cotton Spore", 
	Function = 0x04D, FunctionAsString = "04D", 
	BaseDamage = 0,
	Type = Types.GRASS,
	Category = Category.STATUS,
	Accuracy = 100,
	PP = 40,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "The user releases cotton-like spores that cling to the foe, harshly reducing its Speed stat."
},
new MoveDataDex() {
	num = 215,
	ID = Moves.GRASS_WHISTLE, 
	//name = "GrassWhistle", 
	Function = 0x003, FunctionAsString = "003", 
	BaseDamage = 0,
	Type = Types.GRASS,
	Category = Category.STATUS,
	Accuracy = 55,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true,sound: true ) { }
	//,description = "The user plays a pleasant melody that lulls the target into a deep sleep."
},
new MoveDataDex() {
	num = 216,
	ID = Moves.INGRAIN, 
	//name = "Ingrain", 
	Function = 0x0DB, FunctionAsString = "0DB", 
	BaseDamage = 0,
	Type = Types.GRASS,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 20,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true,bite: true ) { }
	//,description = "The user lays roots that restore its HP on every turn. Because it is rooted, it can't switch out."
},
new MoveDataDex() {
	num = 217,
	ID = Moves.LEECH_SEED, 
	//name = "Leech Seed", 
	Function = 0x0DC, FunctionAsString = "0DC", 
	BaseDamage = 0,
	Type = Types.GRASS,
	Category = Category.STATUS,
	Accuracy = 90,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "A seed is planted on the target. It steals some HP from the target every turn."
},
new MoveDataDex() {
	num = 218,
	ID = Moves.SLEEP_POWDER, 
	//name = "Sleep Powder", 
	Function = 0x003, FunctionAsString = "003", 
	BaseDamage = 0,
	Type = Types.GRASS,
	Category = Category.STATUS,
	Accuracy = 75,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "The user scatters a big cloud of sleep-inducing dust around the target."
},
new MoveDataDex() {
	num = 219,
	ID = Moves.SPORE, 
	//name = "Spore", 
	Function = 0x003, FunctionAsString = "003", 
	BaseDamage = 0,
	Type = Types.GRASS,
	Category = Category.STATUS,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "The user scatters bursts of spores that induce sleep."
},
new MoveDataDex() {
	num = 220,
	ID = Moves.STUN_SPORE, 
	//name = "Stun Spore", 
	Function = 0x007, FunctionAsString = "007", 
	BaseDamage = 0,
	Type = Types.GRASS,
	Category = Category.STATUS,
	Accuracy = 75,
	PP = 30,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "The user scatters a cloud of paralyzing powder. It may leave the target with paralysis."
},
new MoveDataDex() {
	num = 221,
	ID = Moves.SYNTHESIS, 
	//name = "Synthesis", 
	Function = 0x0D8, FunctionAsString = "0D8", 
	BaseDamage = 0,
	Type = Types.GRASS,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 5,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true,bite: true ) { }
	//,description = "The user restores its own HP. The amount of HP regained varies with the weather."
},
new MoveDataDex() {
	num = 222,
	ID = Moves.WORRY_SEED, 
	//name = "Worry Seed", 
	Function = 0x064, FunctionAsString = "064", 
	BaseDamage = 0,
	Type = Types.GRASS,
	Category = Category.STATUS,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "A seed that causes worry is planted on the foe. It prevents sleep by making its Ability Insomnia."
},
new MoveDataDex() {
	num = 223,
	ID = Moves.EARTHQUAKE, 
	//name = "Earthquake", 
	Function = 0x076, FunctionAsString = "076", 
	BaseDamage = 100,
	Type = Types.GROUND,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.AllNonUsers,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user sets off an earthquake that strikes every Pokémon around it."
},
new MoveDataDex() {
	num = 224,
	ID = Moves.EARTH_POWER, 
	//name = "Earth Power", 
	Function = 0x046, FunctionAsString = "046", 
	BaseDamage = 90,
	Type = Types.GROUND,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 10,
	Effects = 10,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user makes the ground under the foe erupt with power. It may also lower the target's Sp. Def."
},
new MoveDataDex() {
	num = 225,
	ID = Moves.DIG, 
	//name = "Dig", 
	Function = 0x0CA, FunctionAsString = "0CA", 
	BaseDamage = 80,
	Type = Types.GROUND,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user burrows, then attacks on the second turn. It can also be used to exit dungeons."
},
new MoveDataDex() {
	num = 226,
	ID = Moves.DRILL_RUN, 
	//name = "Drill Run", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 80,
	Type = Types.GROUND,
	Category = Category.PHYSICAL,
	Accuracy = 95,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true,bite: true ) { }
	//,description = "The user crashes into its target while rotating its body like a drill. Critical hits land more easily."
},
new MoveDataDex() {
	num = 227,
	ID = Moves.BONE_CLUB, 
	//name = "Bone Club", 
	Function = 0x00F, FunctionAsString = "00F", 
	BaseDamage = 65,
	Type = Types.GROUND,
	Category = Category.PHYSICAL,
	Accuracy = 85,
	PP = 20,
	Effects = 10,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The user clubs the target with a bone. It may also make the target flinch."
},
new MoveDataDex() {
	num = 228,
	ID = Moves.MUD_BOMB, 
	//name = "Mud Bomb", 
	Function = 0x047, FunctionAsString = "047", 
	BaseDamage = 65,
	Type = Types.GROUND,
	Category = Category.SPECIAL,
	Accuracy = 85,
	PP = 10,
	Effects = 30,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user launches a hard-packed mud ball to attack. It may also lower the target's accuracy."
},
new MoveDataDex() {
	num = 229,
	ID = Moves.BULLDOZE, 
	//name = "Bulldoze", 
	Function = 0x044, FunctionAsString = "044", 
	BaseDamage = 60,
	Type = Types.GROUND,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 20,
	Effects = 100,
	Target = Target.AllNonUsers,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user strikes everything around it by stomping on the ground. It reduces hit Pokémon's Speed."
},
new MoveDataDex() {
	num = 230,
	ID = Moves.MUD_SHOT, 
	//name = "Mud Shot", 
	Function = 0x044, FunctionAsString = "044", 
	BaseDamage = 55,
	Type = Types.GROUND,
	Category = Category.SPECIAL,
	Accuracy = 95,
	PP = 15,
	Effects = 100,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user attacks by hurling a blob of mud at the target. It also reduces the target's Speed."
},
new MoveDataDex() {
	num = 231,
	ID = Moves.BONEMERANG, 
	//name = "Bonemerang", 
	Function = 0x0BD, FunctionAsString = "0BD", 
	BaseDamage = 50,
	Type = Types.GROUND,
	Category = Category.PHYSICAL,
	Accuracy = 90,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user throws the bone it holds. The bone loops to hit the target twice, coming and going."
},
new MoveDataDex() {
	num = 232,
	ID = Moves.SAND_TOMB, 
	//name = "Sand Tomb", 
	Function = 0x0CF, FunctionAsString = "0CF", 
	BaseDamage = 35,
	Type = Types.GROUND,
	Category = Category.PHYSICAL,
	Accuracy = 85,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user traps the target inside a harshly raging sandstorm for four to five turns."
},
new MoveDataDex() {
	num = 233,
	ID = Moves.BONE_RUSH, 
	//name = "Bone Rush", 
	Function = 0x0C0, FunctionAsString = "0C0", 
	BaseDamage = 25,
	Type = Types.GROUND,
	Category = Category.PHYSICAL,
	Accuracy = 90,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user strikes the target with a hard bone two to five times in a row."
},
new MoveDataDex() {
	num = 234,
	ID = Moves.MUD_SLAP, 
	//name = "Mud-Slap", 
	Function = 0x047, FunctionAsString = "047", 
	BaseDamage = 20,
	Type = Types.GROUND,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 10,
	Effects = 100,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The user hurls mud in the target's face to inflict damage and lower its accuracy."
},
new MoveDataDex() {
	num = 235,
	ID = Moves.FISSURE, 
	//name = "Fissure", 
	Function = 0x070, FunctionAsString = "070", 
	BaseDamage = 1,
	Type = Types.GROUND,
	Category = Category.PHYSICAL,
	Accuracy = 30,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The user opens up a fissure in the ground and drops the foe in. The target instantly faints if it hits."
},
new MoveDataDex() {
	num = 236,
	ID = Moves.MAGNITUDE, 
	//name = "Magnitude", 
	Function = 0x095, FunctionAsString = "095", 
	BaseDamage = 1,
	Type = Types.GROUND,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 30,
	Effects = 0,
	Target = Target.AllNonUsers,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user looses a ground-shaking quake affecting everyone around the user. Its power varies."
},
new MoveDataDex() {
	num = 237,
	ID = Moves.MUD_SPORT, 
	//name = "Mud Sport", 
	Function = 0x09D, FunctionAsString = "09D", 
	BaseDamage = 0,
	Type = Types.GROUND,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 15,
	Effects = 0,
	Target = Target.UserSide,
	Priority = 0,
	Flags = new Flags(  ) { } 
	//,description = "The user covers itself with mud. It weakens Electric-type moves while the user is in the battle."
},
new MoveDataDex() {
	num = 238,
	ID = Moves.SAND_ATTACK, 
	//name = "Sand-Attack", 
	Function = 0x047, FunctionAsString = "047", 
	BaseDamage = 0,
	Type = Types.GROUND,
	Category = Category.STATUS,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "Sand is hurled in the target's face, reducing its accuracy."
},
new MoveDataDex() {
	num = 239,
	ID = Moves.SPIKES, 
	//name = "Spikes", 
	Function = 0x103, FunctionAsString = "103", 
	BaseDamage = 0,
	Type = Types.GROUND,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 20,
	Effects = 0,
	Target = Target.OpposingSide,
	Priority = 0,
	Flags = new Flags( reflectable: true ) { }
	//,description = "The user lays a trap of spikes at the foe's feet. The trap hurts foes that switch into battle."
},
new MoveDataDex() {
	num = 240,
	ID = Moves.FREEZE_SHOCK, 
	//name = "Freeze Shock", 
	Function = 0x0C5, FunctionAsString = "0C5", 
	BaseDamage = 140,
	Type = Types.ICE,
	Category = Category.PHYSICAL,
	Accuracy = 90,
	PP = 5,
	Effects = 30,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "On the second turn, the user hits the foe with electrically charged ice. It may also paralyze the foe."
},
new MoveDataDex() {
	num = 241,
	ID = Moves.ICE_BURN, 
	//name = "Ice Burn", 
	Function = 0x0C6, FunctionAsString = "0C6", 
	BaseDamage = 140,
	Type = Types.ICE,
	Category = Category.SPECIAL,
	Accuracy = 90,
	PP = 5,
	Effects = 30,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "On the second turn, an ultracold, freezing wind surrounds the foe. This may leave it with a burn."
},
new MoveDataDex() {
	num = 242,
	ID = Moves.BLIZZARD, 
	//name = "Blizzard", 
	Function = 0x00D, FunctionAsString = "00D", 
	BaseDamage = 120,
	Type = Types.ICE,
	Category = Category.SPECIAL,
	Accuracy = 70,
	PP = 5,
	Effects = 10,
	Target = Target.AllOpposing,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "A howling blizzard is summoned to strike the opposing team. It may also freeze them solid."
},
new MoveDataDex() {
	num = 243,
	ID = Moves.ICE_BEAM, 
	//name = "Ice Beam", 
	Function = 0x00C, FunctionAsString = "00C", 
	BaseDamage = 95,
	Type = Types.ICE,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 10,
	Effects = 10,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The target is struck with an icy-cold beam of energy. It may also freeze the target solid."
},
new MoveDataDex() {
	num = 244,
	ID = Moves.ICICLE_CRASH, 
	//name = "Icicle Crash", 
	Function = 0x00F, FunctionAsString = "00F", 
	BaseDamage = 85,
	Type = Types.ICE,
	Category = Category.PHYSICAL,
	Accuracy = 90,
	PP = 10,
	Effects = 30,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The user attacks by harshly dropping an icicle onto the foe. It may also make the target flinch."
},
new MoveDataDex() {
	num = 245,
	ID = Moves.ICE_PUNCH, 
	//name = "Ice Punch", 
	Function = 0x00C, FunctionAsString = "00C", 
	BaseDamage = 75,
	Type = Types.ICE,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 15,
	Effects = 10,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,punch: true ) { }
	//,description = "The target is punched with an icy fist. It may also leave the target frozen."
},
new MoveDataDex() {
	num = 246,
	ID = Moves.AURORA_BEAM, 
	//name = "Aurora Beam", 
	Function = 0x042, FunctionAsString = "042", 
	BaseDamage = 65,
	Type = Types.ICE,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 20,
	Effects = 10,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The target is hit with a rainbow-colored beam. This may also lower the target's Attack stat."
},
new MoveDataDex() {
	num = 247,
	ID = Moves.GLACIATE, 
	//name = "Glaciate", 
	Function = 0x044, FunctionAsString = "044", 
	BaseDamage = 65,
	Type = Types.ICE,
	Category = Category.SPECIAL,
	Accuracy = 95,
	PP = 10,
	Effects = 100,
	Target = Target.AllOpposing,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The user attacks by blowing freezing cold air at the foe. This attack reduces the targets' Speed stat."
},
new MoveDataDex() {
	num = 248,
	ID = Moves.ICE_FANG, 
	//name = "Ice Fang", 
	Function = 0x00E, FunctionAsString = "00E", 
	BaseDamage = 65,
	Type = Types.ICE,
	Category = Category.PHYSICAL,
	Accuracy = 95,
	PP = 15,
	Effects = 10,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user bites with cold-infused fangs. It may also make the target flinch or leave it frozen."
},
new MoveDataDex() {
	num = 249,
	ID = Moves.AVALANCHE, 
	//name = "Avalanche", 
	Function = 0x081, FunctionAsString = "081", 
	BaseDamage = 60,
	Type = Types.ICE,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = -4,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "An attack move that inflicts double the damage if the user has been hurt by the foe in the same turn."
},
new MoveDataDex() {
	num = 250,
	ID = Moves.ICY_WIND, 
	//name = "Icy Wind", 
	Function = 0x044, FunctionAsString = "044", 
	BaseDamage = 55,
	Type = Types.ICE,
	Category = Category.SPECIAL,
	Accuracy = 95,
	PP = 15,
	Effects = 100,
	Target = Target.AllOpposing,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The user attacks with a gust of chilled air. It also lowers the targets' Speed stat."
},
new MoveDataDex() {
	num = 251,
	ID = Moves.FROST_BREATH, 
	//name = "Frost Breath", 
	Function = 0x0A0, FunctionAsString = "0A0", 
	BaseDamage = 40,
	Type = Types.ICE,
	Category = Category.SPECIAL,
	Accuracy = 90,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user blows a cold breath on the target. This attack always results in a critical hit."
},
new MoveDataDex() {
	num = 252,
	ID = Moves.ICE_SHARD, 
	//name = "Ice Shard", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 40,
	Type = Types.ICE,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 30,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 1,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user flash freezes chunks of ice and hurls them at the target. This move always goes first."
},
new MoveDataDex() {
	num = 253,
	ID = Moves.POWDER_SNOW, 
	//name = "Powder Snow", 
	Function = 0x00C, FunctionAsString = "00C", 
	BaseDamage = 40,
	Type = Types.ICE,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 25,
	Effects = 10,
	Target = Target.AllOpposing,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The user attacks with a chilling gust of powdery snow. It may also freeze the targets."
},
new MoveDataDex() {
	num = 254,
	ID = Moves.ICE_BALL, 
	//name = "Ice Ball", 
	Function = 0x0D3, FunctionAsString = "0D3", 
	BaseDamage = 30,
	Type = Types.ICE,
	Category = Category.PHYSICAL,
	Accuracy = 90,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user continually rolls into the target over five turns. It becomes stronger each time it hits."
},
new MoveDataDex() {
	num = 255,
	ID = Moves.ICICLE_SPEAR, 
	//name = "Icicle Spear", 
	Function = 0x0C0, FunctionAsString = "0C0", 
	BaseDamage = 25,
	Type = Types.ICE,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 30,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user launches sharp icicles at the target. It strikes two to five times in a row."
},
new MoveDataDex() {
	num = 256,
	ID = Moves.SHEER_COLD, 
	//name = "Sheer Cold", 
	Function = 0x070, FunctionAsString = "070", 
	BaseDamage = 1,
	Type = Types.ICE,
	Category = Category.SPECIAL,
	Accuracy = 30,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The foe is attacked with a blast of absolute-zero cold. The target instantly faints if it hits."
},
new MoveDataDex() {
	num = 257,
	ID = Moves.HAIL, 
	//name = "Hail", 
	Function = 0x102, FunctionAsString = "102", 
	BaseDamage = 0,
	Type = Types.ICE,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.BothSides,
	Priority = 0,
	Flags = new Flags(  ) { } 
	//,description = "The user summons a hail storm lasting five turns. It damages all Pokémon except the Ice type."
},
new MoveDataDex() {
	num = 258,
	ID = Moves.HAZE, 
	//name = "Haze", 
	Function = 0x051, FunctionAsString = "051", 
	BaseDamage = 0,
	Type = Types.ICE,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 30,
	Effects = 0,
	Target = Target.BothSides,
	Priority = 0,
	Flags = new Flags(  ) { } 
	//,description = "The user creates a haze that eliminates every stat change among all the Pokémon engaged in battle."
},
new MoveDataDex() {
	num = 259,
	ID = Moves.MIST, 
	//name = "Mist", 
	Function = 0x056, FunctionAsString = "056", 
	BaseDamage = 0,
	Type = Types.ICE,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 30,
	Effects = 0,
	Target = Target.UserSide,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user cloaks its body with a white mist that prevents any of its stats from being cut for five turns."
},
new MoveDataDex() {
	num = 260,
	ID = Moves.EXPLOSION, 
	//name = "Explosion", 
	Function = 0x0E0, FunctionAsString = "0E0", 
	BaseDamage = 250,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 5,
	Effects = 0,
	Target = Target.AllNonUsers,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user explodes to inflict damage on those around it. The user faints upon using this move."
},
new MoveDataDex() {
	num = 261,
	ID = Moves.SELF_DESTRUCT, 
	//name = "Selfdestruct", 
	Function = 0x0E0, FunctionAsString = "0E0", 
	BaseDamage = 200,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 5,
	Effects = 0,
	Target = Target.AllNonUsers,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user blows up to inflict damage on all Pokémon in battle. The user faints upon using this move."
},
new MoveDataDex() {
	num = 262,
	ID = Moves.GIGA_IMPACT, 
	//name = "Giga Impact", 
	Function = 0x0C2, FunctionAsString = "0C2", 
	BaseDamage = 150,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 90,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user charges at the target using every bit of its power. The user must rest on the next turn."
},
new MoveDataDex() {
	num = 263,
	ID = Moves.HYPER_BEAM, 
	//name = "Hyper Beam", 
	Function = 0x0C2, FunctionAsString = "0C2", 
	BaseDamage = 150,
	Type = Types.NORMAL,
	Category = Category.SPECIAL,
	Accuracy = 90,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The foe is attacked with a powerful beam. The user must rest on the next turn to regain its energy."
},
new MoveDataDex() {
	num = 264,
	ID = Moves.LAST_RESORT, 
	//name = "Last Resort", 
	Function = 0x125, FunctionAsString = "125", 
	BaseDamage = 140,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "This move can be used only after the user has used all the other moves it knows in the battle."
},
new MoveDataDex() {
	num = 265,
	ID = Moves.DOUBLE_EDGE, 
	//name = "Double-Edge", 
	Function = 0x0FB, FunctionAsString = "0FB", 
	BaseDamage = 120,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "A reckless, life-risking tackle. It also damages the user by a fairly large amount, however."
},
new MoveDataDex() {
	num = 266,
	ID = Moves.HEAD_CHARGE, 
	//name = "Head Charge", 
	Function = 0x0FA, FunctionAsString = "0FA", 
	BaseDamage = 120,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user charges its head into the foe, using its powerful guard hair. The user also takes damage."
},
new MoveDataDex() {
	num = 267,
	ID = Moves.MEGA_KICK, 
	//name = "Mega Kick", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 120,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 75,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The target is attacked by a kick launched with muscle-packed power."
},
new MoveDataDex() {
	num = 268,
	ID = Moves.THRASH, 
	//name = "Thrash", 
	Function = 0x0D2, FunctionAsString = "0D2", 
	BaseDamage = 120,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.RandomOpposing,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user rampages and attacks for two to three turns. It then becomes confused, however."
},
new MoveDataDex() {
	num = 269,
	ID = Moves.EGG_BOMB, 
	//name = "Egg Bomb", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 100,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 75,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "A large egg is hurled at the target with maximum force to inflict damage."
},
new MoveDataDex() {
	num = 270,
	ID = Moves.JUDGMENT, 
	//name = "Judgment", 
	Function = 0x09F, FunctionAsString = "09F", 
	BaseDamage = 100,
	Type = Types.NORMAL,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user releases countless shots of light. Its type varies with the kind of Plate the user is holding."
},
new MoveDataDex() {
	num = 271,
	ID = Moves.SKULL_BASH, 
	//name = "Skull Bash", 
	Function = 0x0C8, FunctionAsString = "0C8", 
	BaseDamage = 100,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user tucks in its head to raise its Defense in the first turn, then rams the foe on the next turn."
},
new MoveDataDex() {
	num = 272,
	ID = Moves.HYPER_VOICE, 
	//name = "Hyper Voice", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 90,
	Type = Types.NORMAL,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.AllOpposing,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,sound: true ) { }
	//,description = "The user lets loose a horribly echoing shout with the power to inflict damage."
},
new MoveDataDex() {
	num = 273,
	ID = Moves.ROCK_CLIMB, 
	//name = "Rock Climb", 
	Function = 0x013, FunctionAsString = "013", 
	BaseDamage = 90,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 85,
	PP = 20,
	Effects = 20,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user attacks the target by smashing into it with incredible force. It may also confuse the target."
},
new MoveDataDex() {
	num = 274,
	ID = Moves.TAKE_DOWN, 
	//name = "Take Down", 
	Function = 0x0FA, FunctionAsString = "0FA", 
	BaseDamage = 90,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 85,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "A reckless, full-body charge attack for slamming into the foe. It also damages the user a little."
},
new MoveDataDex() {
	num = 275,
	ID = Moves.UPROAR, 
	//name = "Uproar", 
	Function = 0x0D1, FunctionAsString = "0D1", 
	BaseDamage = 90,
	Type = Types.NORMAL,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.RandomOpposing,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true,sound: true ) { }
	//,description = "The user attacks in an uproar for three turns. Over that time, no one can fall asleep."
},
new MoveDataDex() {
	num = 276,
	ID = Moves.BODY_SLAM, 
	//name = "Body Slam", 
	Function = 0x007, FunctionAsString = "007", 
	BaseDamage = 85,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 15,
	Effects = 30,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true ) { }
	//,description = "The user drops onto the target with its full body weight. It may leave the target with paralysis."
},
new MoveDataDex() {
	num = 277,
	ID = Moves.TECHNO_BLAST, 
	//name = "Techno Blast", 
	Function = 0x09F, FunctionAsString = "09F", 
	BaseDamage = 85,
	Type = Types.NORMAL,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user fires a beam of light at its target. The type changes depending on the Drive the user holds."
},
new MoveDataDex() {
	num = 278,
	ID = Moves.EXTREME_SPEED, 
	//name = "ExtremeSpeed", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 80,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 2,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user charges the target at blinding speed. This attack always goes before any other move."
},
new MoveDataDex() {
	num = 279,
	ID = Moves.HYPER_FANG, 
	//name = "Hyper Fang", 
	Function = 0x00F, FunctionAsString = "00F", 
	BaseDamage = 80,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 90,
	PP = 15,
	Effects = 10,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true ) { }
	//,description = "The user bites hard on the target with its sharp front fangs. It may also make the target flinch."
},
new MoveDataDex() {
	num = 280,
	ID = Moves.MEGA_PUNCH, 
	//name = "Mega Punch", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 80,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 85,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true,punch: true ) { }
	//,description = "The target is slugged by a punch thrown with muscle-packed power."
},
new MoveDataDex() {
	num = 281,
	ID = Moves.RAZOR_WIND, 
	//name = "Razor Wind", 
	Function = 0x0C3, FunctionAsString = "0C3", 
	BaseDamage = 80,
	Type = Types.NORMAL,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.AllOpposing,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true,bite: true ) { }
	//,description = "A two-turn attack. Blades of wind hit the foe on the second turn. Critical hits land more easily."
},
new MoveDataDex() {
	num = 282,
	ID = Moves.SLAM, 
	//name = "Slam", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 80,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 75,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The target is slammed with a long tail, vines, etc., to inflict damage."
},
new MoveDataDex() {
	num = 283,
	ID = Moves.STRENGTH, 
	//name = "Strength", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 80,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The target is slugged with a punch thrown at maximum power. It can also be used to move heavy boulders."
},
new MoveDataDex() {
	num = 284,
	ID = Moves.TRI_ATTACK, 
	//name = "Tri Attack", 
	Function = 0x017, FunctionAsString = "017", 
	BaseDamage = 80,
	Type = Types.NORMAL,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 10,
	Effects = 20,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The user strikes with a simultaneous three-beam attack. May also paralyze, burn, or freeze the target."
},
new MoveDataDex() {
	num = 285,
	ID = Moves.CRUSH_CLAW, 
	//name = "Crush Claw", 
	Function = 0x043, FunctionAsString = "043", 
	BaseDamage = 75,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 95,
	PP = 10,
	Effects = 50,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true ) { }
	//,description = "The user slashes the target with hard and sharp claws. It may also lower the target's Defense."
},
new MoveDataDex() {
	num = 286,
	ID = Moves.RELIC_SONG, 
	//name = "Relic Song", 
	Function = 0x003, FunctionAsString = "003", 
	BaseDamage = 75,
	Type = Types.NORMAL,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 10,
	Effects = 100,
	Target = Target.AllOpposing,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,sound: true ) { }
	//,description = "An ancient song appeals to the hearts of those listening. It may also induce sleep."
},
new MoveDataDex() {
	num = 287,
	ID = Moves.CHIP_AWAY, 
	//name = "Chip Away", 
	Function = 0x0A9, FunctionAsString = "0A9", 
	BaseDamage = 70,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "Seeking an opening, the user strikes continually. The foe's stat changes don't affect the damage."
},
new MoveDataDex() {
	num = 288,
	ID = Moves.DIZZY_PUNCH, 
	//name = "Dizzy Punch", 
	Function = 0x013, FunctionAsString = "013", 
	BaseDamage = 70,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 10,
	Effects = 20,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,punch: true ) { }
	//,description = "The target is hit with rhythmically launched punches that may also leave it confused."
},
new MoveDataDex() {
	num = 289,
	ID = Moves.FACADE, 
	//name = "Facade", 
	Function = 0x07E, FunctionAsString = "07E", 
	BaseDamage = 70,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true ) { }
	//,description = "An attack move that doubles its power if the user is poisoned, burned, or has paralysis."
},
new MoveDataDex() {
	num = 290,
	ID = Moves.HEADBUTT, 
	//name = "Headbutt", 
	Function = 0x00F, FunctionAsString = "00F", 
	BaseDamage = 70,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 15,
	Effects = 30,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true ) { }
	//,description = "The user sticks out its head and attacks by charging into the foe. It may also make the target flinch."
},
new MoveDataDex() {
	num = 291,
	ID = Moves.RETALIATE, 
	//name = "Retaliate", 
	Function = 0x085, FunctionAsString = "085", 
	BaseDamage = 70,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "Gets revenge for a fainted ally. If an ally fainted in the last turn, this attack's damage increases."
},
new MoveDataDex() {
	num = 292,
	ID = Moves.SECRET_POWER, 
	//name = "Secret Power", 
	Function = 0x0A4, FunctionAsString = "0A4", 
	BaseDamage = 70,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 20,
	Effects = 30,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The user attacks with a secret power. Its added effects vary depending on the user's environment."
},
new MoveDataDex() {
	num = 293,
	ID = Moves.SLASH, 
	//name = "Slash", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 70,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true,bite: true ) { }
	//,description = "The target is attacked with a slash of claws or blades. Critical hits land more easily."
},
new MoveDataDex() {
	num = 294,
	ID = Moves.HORN_ATTACK, 
	//name = "Horn Attack", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 65,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 25,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The target is jabbed with a sharply pointed horn to inflict damage."
},
new MoveDataDex() {
	num = 295,
	ID = Moves.STOMP, 
	//name = "Stomp", 
	Function = 0x010, FunctionAsString = "010", 
	BaseDamage = 65,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 20,
	Effects = 30,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true ) { }
	//,description = "The target is stomped with a big foot. It may also make the target flinch."
},
new MoveDataDex() {
	num = 296,
	ID = Moves.COVET, 
	//name = "Covet", 
	Function = 0x0F1, FunctionAsString = "0F1", 
	BaseDamage = 60,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 40,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true ) { }
	//,description = "The user endearingly approaches the target, then steals the target's held item."
},
new MoveDataDex() {
	num = 297,
	ID = Moves.ROUND, 
	//name = "Round", 
	Function = 0x083, FunctionAsString = "083", 
	BaseDamage = 60,
	Type = Types.NORMAL,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true,sound: true ) { }
	//,description = "The user attacks with a song. Others can join in the Round and make the attack do greater damage."
},
new MoveDataDex() {
	num = 298,
	ID = Moves.SMELLING_SALTS, 
	//name = "SmellingSalt", 
	Function = 0x07C, FunctionAsString = "07C", 
	BaseDamage = 60,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true ) { }
	//,description = "This attack inflicts double damage on a paralyzed foe. It also cures the target's paralysis, however."
},
new MoveDataDex() {
	num = 299,
	ID = Moves.SWIFT, 
	//name = "Swift", 
	Function = 0x0A5, FunctionAsString = "0A5", 
	BaseDamage = 60,
	Type = Types.NORMAL,
	Category = Category.SPECIAL,
	Accuracy = 0,
	PP = 20,
	Effects = 0,
	Target = Target.AllOpposing,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "Star-shaped rays are shot at the opposing team. This attack never misses."
},
new MoveDataDex() {
	num = 300,
	ID = Moves.VICE_GRIP, 
	//name = "ViceGrip", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 55,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 30,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The target is gripped and squeezed from both sides to inflict damage."
},
new MoveDataDex() {
	num = 301,
	ID = Moves.CUT, 
	//name = "Cut", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 50,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 95,
	PP = 30,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The target is cut with a scythe or a claw. It can also be used to cut down thin trees."
},
new MoveDataDex() {
	num = 302,
	ID = Moves.STRUGGLE, 
	//name = "Struggle", 
	Function = 0x002, FunctionAsString = "002", 
	BaseDamage = 50,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 0,
	PP = 1,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,flinch: true ) { }
	//,description = "An attack that is used in desperation only if the user has no PP. It also hurts the user slightly."
},
new MoveDataDex() {
	num = 303,
	ID = Moves.TACKLE, 
	//name = "Tackle", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 50,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 35,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "A physical attack in which the user charges and slams into the target with its whole body."
},
new MoveDataDex() {
	num = 304,
	ID = Moves.WEATHER_BALL, 
	//name = "Weather Ball", 
	Function = 0x087, FunctionAsString = "087", 
	BaseDamage = 50,
	Type = Types.NORMAL,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "An attack move that varies in power and type depending on the weather."
},
new MoveDataDex() {
	num = 305,
	ID = Moves.ECHOED_VOICE, 
	//name = "Echoed Voice", 
	Function = 0x092, FunctionAsString = "092", 
	BaseDamage = 40,
	Type = Types.NORMAL,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true,sound: true ) { }
	//,description = "The user attacks the foe with an echoing voice. If this move is used every turn, it does greater damage."
},
new MoveDataDex() {
	num = 306,
	ID = Moves.FAKE_OUT, 
	//name = "Fake Out", 
	Function = 0x012, FunctionAsString = "012", 
	BaseDamage = 40,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 10,
	Effects = 100,
	Target = Target.SingleNonUser,
	Priority = 3,
	Flags = new Flags( contact: true,protect: true,mirror: true ) { }
	//,description = "An attack that hits first and makes the target flinch. It only works the first turn the user is in battle."
},
new MoveDataDex() {
	num = 307,
	ID = Moves.FALSE_SWIPE, 
	//name = "False Swipe", 
	Function = 0x0E9, FunctionAsString = "0E9", 
	BaseDamage = 40,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 40,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "A restrained attack that prevents the target from fainting. The target is left with at least 1 HP."
},
new MoveDataDex() {
	num = 308,
	ID = Moves.PAY_DAY, 
	//name = "Pay Day", 
	Function = 0x109, FunctionAsString = "109", 
	BaseDamage = 40,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "Numerous coins are hurled at the target to inflict damage. Money is earned after battle."
},
new MoveDataDex() {
	num = 309,
	ID = Moves.POUND, 
	//name = "Pound", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 40,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 35,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The target is physically pounded with a long tail or a foreleg, etc."
},
new MoveDataDex() {
	num = 310,
	ID = Moves.QUICK_ATTACK, 
	//name = "Quick Attack", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 40,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 30,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 1,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user lunges at the target at a speed that makes it almost invisible. It is sure to strike first."
},
new MoveDataDex() {
	num = 311,
	ID = Moves.SCRATCH, 
	//name = "Scratch", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 40,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 35,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "Hard, pointed, and sharp claws rake the target to inflict damage."
},
new MoveDataDex() {
	num = 312,
	ID = Moves.SNORE, 
	//name = "Snore", 
	Function = 0x011, FunctionAsString = "011", 
	BaseDamage = 40,
	Type = Types.NORMAL,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 15,
	Effects = 30,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true,sound: true ) { }
	//,description = "An attack that can be used only if the user is asleep. The harsh noise may also make the target flinch."
},
new MoveDataDex() {
	num = 313,
	ID = Moves.DOUBLE_HIT, 
	//name = "Double Hit", 
	Function = 0x0BD, FunctionAsString = "0BD", 
	BaseDamage = 35,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 90,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user slams the target with a long tail, vines, or tentacle. The target is hit twice in a row."
},
new MoveDataDex() {
	num = 314,
	ID = Moves.FEINT, 
	//name = "Feint", 
	Function = 0x0AD, FunctionAsString = "0AD", 
	BaseDamage = 30,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 2,
	Flags = new Flags(  ) { } 
	//,description = "An attack that hits a target using Protect or Detect. It also lifts the effects of those moves."
},
new MoveDataDex() {
	num = 315,
	ID = Moves.TAIL_SLAP, 
	//name = "Tail Slap", 
	Function = 0x0C0, FunctionAsString = "0C0", 
	BaseDamage = 25,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 85,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user attacks by striking the target with its hard tail. It hits the Pokémon two to five times in a row."
},
new MoveDataDex() {
	num = 316,
	ID = Moves.RAGE, 
	//name = "Rage", 
	Function = 0x093, FunctionAsString = "093", 
	BaseDamage = 20,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "As long as this move is in use, the user's Attack rises each time the user is hit in battle."
},
new MoveDataDex() {
	num = 317,
	ID = Moves.RAPID_SPIN, 
	//name = "Rapid Spin", 
	Function = 0x110, FunctionAsString = "110", 
	BaseDamage = 20,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 40,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "A spin attack that can also eliminate such moves as Bind, Wrap, Leech Seed, and Spikes."
},
new MoveDataDex() {
	num = 318,
	ID = Moves.SPIKE_CANNON, 
	//name = "Spike Cannon", 
	Function = 0x0C0, FunctionAsString = "0C0", 
	BaseDamage = 20,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "Sharp spikes are shot at the target in rapid succession. They hit two to five times in a row."
},
new MoveDataDex() {
	num = 319,
	ID = Moves.COMET_PUNCH, 
	//name = "Comet Punch", 
	Function = 0x0C0, FunctionAsString = "0C0", 
	BaseDamage = 18,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 85,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true,punch: true ) { }
	//,description = "The target is hit with a flurry of punches that strike two to five times in a row."
},
new MoveDataDex() {
	num = 320,
	ID = Moves.FURY_SWIPES, 
	//name = "Fury Swipes", 
	Function = 0x0C0, FunctionAsString = "0C0", 
	BaseDamage = 18,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 80,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The target is raked with sharp claws or scythes for two to five times in quick succession."
},
new MoveDataDex() {
	num = 321,
	ID = Moves.BARRAGE, 
	//name = "Barrage", 
	Function = 0x0C0, FunctionAsString = "0C0", 
	BaseDamage = 15,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 85,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "Round objects are hurled at the target to strike two to five times in a row."
},
new MoveDataDex() {
	num = 322,
	ID = Moves.BIND, 
	//name = "Bind", 
	Function = 0x0CF, FunctionAsString = "0CF", 
	BaseDamage = 15,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 85,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "Things such as long bodies or tentacles are used to bind and squeeze the foe for four to five turns."
},
new MoveDataDex() {
	num = 323,
	ID = Moves.DOUBLE_SLAP, 
	//name = "DoubleSlap", 
	Function = 0x0C0, FunctionAsString = "0C0", 
	BaseDamage = 15,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 85,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The target is slapped repeatedly, back and forth, two to five times in a row."
},
new MoveDataDex() {
	num = 324,
	ID = Moves.FURY_ATTACK, 
	//name = "Fury Attack", 
	Function = 0x0C0, FunctionAsString = "0C0", 
	BaseDamage = 15,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 85,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The target is jabbed repeatedly with a horn or beak two to five times in a row."
},
new MoveDataDex() {
	num = 325,
	ID = Moves.WRAP, 
	//name = "Wrap", 
	Function = 0x0CF, FunctionAsString = "0CF", 
	BaseDamage = 15,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 90,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "A long body or vines are used to wrap and squeeze the target for four to five turns."
},
new MoveDataDex() {
	num = 326,
	ID = Moves.CONSTRICT, 
	//name = "Constrict", 
	Function = 0x044, FunctionAsString = "044", 
	BaseDamage = 10,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 35,
	Effects = 10,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true ) { }
	//,description = "The foe is attacked with long, creeping tentacles or vines. It may also lower the target's Speed."
},
new MoveDataDex() {
	num = 327,
	ID = Moves.BIDE, 
	//name = "Bide", 
	Function = 0x0D4, FunctionAsString = "0D4", 
	BaseDamage = 1,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.User,
	Priority = 1,
	Flags = new Flags( contact: true,protect: true,flinch: true ) { }
	//,description = "The user endures attacks for two turns, then strikes back to cause double the damage taken."
},
new MoveDataDex() {
	num = 328,
	ID = Moves.CRUSH_GRIP, 
	//name = "Crush Grip", 
	Function = 0x08C, FunctionAsString = "08C", 
	BaseDamage = 1,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The target is crushed with great force. The attack is more powerful the more HP the target has left."
},
new MoveDataDex() {
	num = 329,
	ID = Moves.ENDEAVOR, 
	//name = "Endeavor", 
	Function = 0x06E, FunctionAsString = "06E", 
	BaseDamage = 1,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "An attack move that cuts down the target's HP to equal the user's HP."
},
new MoveDataDex() {
	num = 330,
	ID = Moves.FLAIL, 
	//name = "Flail", 
	Function = 0x098, FunctionAsString = "098", 
	BaseDamage = 1,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user flails about aimlessly to attack. It becomes more powerful the less HP the user has."
},
new MoveDataDex() {
	num = 331,
	ID = Moves.FRUSTRATION, 
	//name = "Frustration", 
	Function = 0x08A, FunctionAsString = "08A", 
	BaseDamage = 1,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "A full-power attack that grows more powerful the less the user likes its Trainer."
},
new MoveDataDex() {
	num = 332,
	ID = Moves.GUILLOTINE, 
	//name = "Guillotine", 
	Function = 0x070, FunctionAsString = "070", 
	BaseDamage = 1,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 30,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true ) { }
	//,description = "A vicious, tearing attack with big pincers. The target will faint instantly if this attack hits."
},
new MoveDataDex() {
	num = 333,
	ID = Moves.HIDDEN_POWER, 
	//name = "Hidden Power", 
	Function = 0x090, FunctionAsString = "090", 
	BaseDamage = 1,
	Type = Types.NORMAL,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "A unique attack that varies in type and intensity depending on the Pokémon using it."
},
new MoveDataDex() {
	num = 334,
	ID = Moves.HORN_DRILL, 
	//name = "Horn Drill", 
	Function = 0x070, FunctionAsString = "070", 
	BaseDamage = 1,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 30,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true ) { }
	//,description = "The user stabs the foe with a horn that rotates like a drill. If it hits, the target faints instantly."
},
new MoveDataDex() {
	num = 335,
	ID = Moves.NATURAL_GIFT, 
	//name = "Natural Gift", 
	Function = 0x096, FunctionAsString = "096", 
	BaseDamage = 1,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The user draws power to attack by using its held Berry. The Berry determines its type and power."
},
new MoveDataDex() {
	num = 336,
	ID = Moves.PRESENT, 
	//name = "Present", 
	Function = 0x094, FunctionAsString = "094", 
	BaseDamage = 1,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 90,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The user attacks by giving the target a gift with a hidden trap. It restores HP sometimes, however."
},
new MoveDataDex() {
	num = 337,
	ID = Moves.RETURN, 
	//name = "Return", 
	Function = 0x089, FunctionAsString = "089", 
	BaseDamage = 1,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "A full-power attack that grows more powerful the more the user likes its Trainer."
},
new MoveDataDex() {
	num = 338,
	ID = Moves.SONIC_BOOM, 
	//name = "SonicBoom", 
	Function = 0x06A, FunctionAsString = "06A", 
	BaseDamage = 1,
	Type = Types.NORMAL,
	Category = Category.SPECIAL,
	Accuracy = 90,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The target is hit with a destructive shock wave that always inflicts 20 HP damage."
},
new MoveDataDex() {
	num = 339,
	ID = Moves.SPIT_UP, 
	//name = "Spit Up", 
	Function = 0x113, FunctionAsString = "113", 
	BaseDamage = 1,
	Type = Types.NORMAL,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,flinch: true ) { }
	//,description = "The power stored using the move Stockpile is released all at once in an attack."
},
new MoveDataDex() {
	num = 340,
	ID = Moves.SUPER_FANG, 
	//name = "Super Fang", 
	Function = 0x06C, FunctionAsString = "06C", 
	BaseDamage = 1,
	Type = Types.NORMAL,
	Category = Category.PHYSICAL,
	Accuracy = 90,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true ) { }
	//,description = "The user chomps hard on the target with its sharp front fangs. It cuts the target's HP to half."
},
new MoveDataDex() {
	num = 341,
	ID = Moves.TRUMP_CARD, 
	//name = "Trump Card", 
	Function = 0x097, FunctionAsString = "097", 
	BaseDamage = 1,
	Type = Types.NORMAL,
	Category = Category.SPECIAL,
	Accuracy = 0,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The fewer PP this move has, the greater its attack power."
},
new MoveDataDex() {
	num = 342,
	ID = Moves.WRING_OUT, 
	//name = "Wring Out", 
	Function = 0x08C, FunctionAsString = "08C", 
	BaseDamage = 1,
	Type = Types.NORMAL,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user powerfully wrings the foe. The more HP the foe has, the greater this attack's power."
},
new MoveDataDex() {
	num = 343,
	ID = Moves.ACUPRESSURE, 
	//name = "Acupressure", 
	Function = 0x037, FunctionAsString = "037", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 30,
	Effects = 0,
	Target = Target.UserOrPartner,
	Priority = 0,
	Flags = new Flags(  ) { } 
	//,description = "The user applies pressure to stress points, sharply boosting one of its stats."
},
new MoveDataDex() {
	num = 344,
	ID = Moves.AFTER_YOU, 
	//name = "After You", 
	Function = 0x11D, FunctionAsString = "11D", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags(  ) { } 
	//,description = "The user helps the target and makes it use its move right after the user."
},
new MoveDataDex() {
	num = 345,
	ID = Moves.ASSIST, 
	//name = "Assist", 
	Function = 0x0B5, FunctionAsString = "0B5", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 20,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags(  ) { } 
	//,description = "The user hurriedly and randomly uses a move among those known by other Pokémon in the party."
},
new MoveDataDex() {
	num = 346,
	ID = Moves.ATTRACT, 
	//name = "Attract", 
	Function = 0x016, FunctionAsString = "016", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "If it is the opposite gender of the user, the target becomes infatuated and less likely to attack."
},
new MoveDataDex() {
	num = 347,
	ID = Moves.BATON_PASS, 
	//name = "Baton Pass", 
	Function = 0x0ED, FunctionAsString = "0ED", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 40,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags(  ) { } 
	//,description = "The user switches places with a party Pokémon in waiting, passing along any stat changes."
},
new MoveDataDex() {
	num = 348,
	ID = Moves.BELLY_DRUM, 
	//name = "Belly Drum", 
	Function = 0x03A, FunctionAsString = "03A", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user maximizes its Attack stat in exchange for HP equal to half its max HP."
},
new MoveDataDex() {
	num = 349,
	ID = Moves.BESTOW, 
	//name = "Bestow", 
	Function = 0x0F3, FunctionAsString = "0F3", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The user passes its held item to the target when the target isn't holding an item."
},
new MoveDataDex() {
	num = 350,
	ID = Moves.BLOCK, 
	//name = "Block", 
	Function = 0x0EF, FunctionAsString = "0EF", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "The user blocks the target's way with arms spread wide to prevent escape."
},
new MoveDataDex() {
	num = 351,
	ID = Moves.CAMOUFLAGE, 
	//name = "Camouflage", 
	Function = 0x060, FunctionAsString = "060", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 20,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user's type is changed depending on its environment, such as at water's edge, in grass, or in a cave."
},
new MoveDataDex() {
	num = 352,
	ID = Moves.CAPTIVATE, 
	//name = "Captivate", 
	Function = 0x04E, FunctionAsString = "04E", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 100,
	PP = 20,
	Effects = 0,
	Target = Target.AllOpposing,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "If it is the opposite gender of the user, the target is charmed into harshly lowering its Sp. Atk stat."
},
new MoveDataDex() {
	num = 353,
	ID = Moves.CHARM, 
	//name = "Charm", 
	Function = 0x04B, FunctionAsString = "04B", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 100,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "The user charmingly gazes at the foe, making it less wary. The target's Attack is harshly lowered."
},
new MoveDataDex() {
	num = 354,
	ID = Moves.CONVERSION, 
	//name = "Conversion", 
	Function = 0x05E, FunctionAsString = "05E", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 30,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user changes its type to become the same type as one of its moves."
},
new MoveDataDex() {
	num = 355,
	ID = Moves.CONVERSION_2, 
	//name = "Conversion 2", 
	Function = 0x05F, FunctionAsString = "05F", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 30,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags(  ) { } 
	//,description = "The user changes its type to make itself resistant to the type of the attack the opponent used last."
},
new MoveDataDex() {
	num = 356,
	ID = Moves.COPYCAT, 
	//name = "Copycat", 
	Function = 0x0AF, FunctionAsString = "0AF", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 20,
	Effects = 0,
	Target = Target.NoTarget,
	Priority = 0,
	Flags = new Flags(  ) { } 
	//,description = "The user mimics the move used immediately before it. The move fails if no other move has been used yet."
},
new MoveDataDex() {
	num = 357,
	ID = Moves.DEFENSE_CURL, 
	//name = "Defense Curl", 
	Function = 0x01E, FunctionAsString = "01E", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 40,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user curls up to conceal weak spots and raise its Defense stat."
},
new MoveDataDex() {
	num = 358,
	ID = Moves.DISABLE, 
	//name = "Disable", 
	Function = 0x0B9, FunctionAsString = "0B9", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 100,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "For four turns, this move prevents the target from using the move it last used."
},
new MoveDataDex() {
	num = 359,
	ID = Moves.DOUBLE_TEAM, 
	//name = "Double Team", 
	Function = 0x022, FunctionAsString = "022", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 15,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "By moving rapidly, the user makes illusory copies of itself to raise its evasiveness."
},
new MoveDataDex() {
	num = 360,
	ID = Moves.ENCORE, 
	//name = "Encore", 
	Function = 0x0BC, FunctionAsString = "0BC", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 100,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "The user compels the target to keep using only the move it last used for three turns."
},
new MoveDataDex() {
	num = 361,
	ID = Moves.ENDURE, 
	//name = "Endure", 
	Function = 0x0E8, FunctionAsString = "0E8", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.User,
	Priority = 3,
	Flags = new Flags(  ) { } 
	//,description = "The user endures any attack with at least 1 HP. Its chance of failing rises if it is used in succession."
},
new MoveDataDex() {
	num = 362,
	ID = Moves.ENTRAINMENT, 
	//name = "Entrainment", 
	Function = 0x066, FunctionAsString = "066", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "The user dances to compel the target to mimic it, making the target's Ability the same as the user's."
},
new MoveDataDex() {
	num = 363,
	ID = Moves.FLASH, 
	//name = "Flash", 
	Function = 0x047, FunctionAsString = "047", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 100,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "The user flashes a light that cuts the target's accuracy. It can also be used to illuminate caves."
},
new MoveDataDex() {
	num = 364,
	ID = Moves.FOCUS_ENERGY, 
	//name = "Focus Energy", 
	Function = 0x023, FunctionAsString = "023", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 30,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user takes a deep breath and focuses so that critical hits land more easily."
},
new MoveDataDex() {
	num = 365,
	ID = Moves.FOLLOW_ME, 
	//name = "Follow Me", 
	Function = 0x117, FunctionAsString = "117", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 20,
	Effects = 0,
	Target = Target.User,
	Priority = 3,
	Flags = new Flags(  ) { } 
	//,description = "The user draws attention to itself, making all targets take aim only at the user."
},
new MoveDataDex() {
	num = 366,
	ID = Moves.FORESIGHT, 
	//name = "Foresight", 
	Function = 0x0A7, FunctionAsString = "0A7", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 40,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "Enables the user to hit a Ghost type with any kind of move. It also enables the user to hit an evasive foe."
},
new MoveDataDex() {
	num = 367,
	ID = Moves.GLARE, 
	//name = "Glare", 
	Function = 0x007, FunctionAsString = "007", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 90,
	PP = 30,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "The user intimidates the target with the pattern on its belly to cause paralysis."
},
new MoveDataDex() {
	num = 368,
	ID = Moves.GROWL, 
	//name = "Growl", 
	Function = 0x042, FunctionAsString = "042", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 100,
	PP = 40,
	Effects = 0,
	Target = Target.AllOpposing,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true,sound: true ) { }
	//,description = "The user growls in an endearing way, making the foe less wary. The foe's Attack stat is lowered."
},
new MoveDataDex() {
	num = 369,
	ID = Moves.GROWTH, 
	//name = "Growth", 
	Function = 0x028, FunctionAsString = "028", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 40,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user's body grows all at once, raising the Atk and Sp. Atk stats."
},
new MoveDataDex() {
	num = 370,
	ID = Moves.HARDEN, 
	//name = "Harden", 
	Function = 0x01D, FunctionAsString = "01D", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 30,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user stiffens all the muscles in its body to raise its Defense stat."
},
new MoveDataDex() {
	num = 371,
	ID = Moves.HEAL_BELL, 
	//name = "Heal Bell", 
	Function = 0x019, FunctionAsString = "019", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 5,
	Effects = 0,
	Target = Target.UserSide,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user makes a soothing bell chime to heal the status problems of all the party Pokémon."
},
new MoveDataDex() {
	num = 372,
	ID = Moves.HELPING_HAND, 
	//name = "Helping Hand", 
	Function = 0x09C, FunctionAsString = "09C", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 20,
	Effects = 0,
	Target = Target.Partner,
	Priority = 5,
	Flags = new Flags(  ) { } 
	//,description = "The user assists an ally by boosting the power of its attack."
},
new MoveDataDex() {
	num = 373,
	ID = Moves.HOWL, 
	//name = "Howl", 
	Function = 0x01C, FunctionAsString = "01C", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 40,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user howls loudly to raise its spirit, boosting its Attack stat."
},
new MoveDataDex() {
	num = 374,
	ID = Moves.LEER, 
	//name = "Leer", 
	Function = 0x043, FunctionAsString = "043", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 100,
	PP = 30,
	Effects = 0,
	Target = Target.AllOpposing,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "The user gains an intimidating leer with sharp eyes. The target's Defense stat is reduced."
},
new MoveDataDex() {
	num = 375,
	ID = Moves.LOCK_ON, 
	//name = "Lock-On", 
	Function = 0x0A6, FunctionAsString = "0A6", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The user takes sure aim at the target. It ensures the next attack does not fail to hit the target."
},
new MoveDataDex() {
	num = 376,
	ID = Moves.LOVELY_KISS, 
	//name = "Lovely Kiss", 
	Function = 0x003, FunctionAsString = "003", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 75,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "With a scary face, the user tries to force a kiss on the target. If it suceeds, the target falls asleep."
},
new MoveDataDex() {
	num = 377,
	ID = Moves.LUCKY_CHANT, 
	//name = "Lucky Chant", 
	Function = 0x0A1, FunctionAsString = "0A1", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 30,
	Effects = 0,
	Target = Target.UserSide,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user chants an incantation toward the sky, preventing the foe from landing critical hits."
},
new MoveDataDex() {
	num = 378,
	ID = Moves.ME_FIRST, 
	//name = "Me First", 
	Function = 0x0B0, FunctionAsString = "0B0", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 20,
	Effects = 0,
	Target = Target.SingleOpposing,
	Priority = 0,
	Flags = new Flags( protect: true ) { }
	//,description = "The user tries to cut ahead of the foe to steal and use the foe's intended move with greater power."
},
new MoveDataDex() {
	num = 379,
	ID = Moves.MEAN_LOOK, 
	//name = "Mean Look", 
	Function = 0x0EF, FunctionAsString = "0EF", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "The user pins the target with a dark, arresting look. The target becomes unable to flee."
},
new MoveDataDex() {
	num = 380,
	ID = Moves.METRONOME, 
	//name = "Metronome", 
	Function = 0x0B6, FunctionAsString = "0B6", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.NoTarget,
	Priority = 0,
	Flags = new Flags(  ) { } 
	//,description = "The user waggles a finger and stimulates its brain into randomly using nearly any move."
},
new MoveDataDex() {
	num = 381,
	ID = Moves.MILK_DRINK, 
	//name = "Milk Drink", 
	Function = 0x0D5, FunctionAsString = "0D5", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true,bite: true ) { }
	//,description = "The user restores its own HP by up to half of its maximum HP. May also be used in the field to heal HP."
},
new MoveDataDex() {
	num = 382,
	ID = Moves.MIMIC, 
	//name = "Mimic", 
	Function = 0x05C, FunctionAsString = "05C", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true ) { }
	//,description = "The user copies the move last used by the foe. The move can be used until the user is switched out."
},
new MoveDataDex() {
	num = 383,
	ID = Moves.MIND_READER, 
	//name = "Mind Reader", 
	Function = 0x0A6, FunctionAsString = "0A6", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The user senses the foe's movements with its mind to ensure its next attack does not miss the foe."
},
new MoveDataDex() {
	num = 384,
	ID = Moves.MINIMIZE, 
	//name = "Minimize", 
	Function = 0x034, FunctionAsString = "034", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 20,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user compresses its body to make itself look smaller, which sharply raises its evasiveness."
},
new MoveDataDex() {
	num = 385,
	ID = Moves.MOONLIGHT, 
	//name = "Moonlight", 
	Function = 0x0D8, FunctionAsString = "0D8", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 5,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true,bite: true ) { }
	//,description = "The user restores its own HP. The amount of HP regained varies with the weather."
},
new MoveDataDex() {
	num = 386,
	ID = Moves.MORNING_SUN, 
	//name = "Morning Sun", 
	Function = 0x0D8, FunctionAsString = "0D8", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 5,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true,bite: true ) { }
	//,description = "The user restores its own HP. The amount of HP regained varies with the weather."
},
new MoveDataDex() {
	num = 387,
	ID = Moves.NATURE_POWER, 
	//name = "Nature Power", 
	Function = 0x0B3, FunctionAsString = "0B3", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 20,
	Effects = 0,
	Target = Target.NoTarget,
	Priority = 0,
	Flags = new Flags(  ) { } 
	//,description = "An attack that makes use of nature's power. Its effects vary depending on the user's environment."
},
new MoveDataDex() {
	num = 388,
	ID = Moves.ODOR_SLEUTH, 
	//name = "Odor Sleuth", 
	Function = 0x0A7, FunctionAsString = "0A7", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 40,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "Enables the user to hit a Ghost type with any type of move. It also enables the user to hit an evasive foe."
},
new MoveDataDex() {
	num = 389,
	ID = Moves.PAIN_SPLIT, 
	//name = "Pain Split", 
	Function = 0x05A, FunctionAsString = "05A", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The user adds its HP to the target's HP, then equally shares the combined HP with the target."
},
new MoveDataDex() {
	num = 390,
	ID = Moves.PERISH_SONG, 
	//name = "Perish Song", 
	Function = 0x0E5, FunctionAsString = "0E5", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 5,
	Effects = 0,
	Target = Target.BothSides,
	Priority = 0,
	Flags = new Flags( sound: true ) { }
	//,description = "Any Pokémon that hears this song faints in three turns, unless it switches out of battle."
},
new MoveDataDex() {
	num = 391,
	ID = Moves.PROTECT, 
	//name = "Protect", 
	Function = 0x0AA, FunctionAsString = "0AA", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.User,
	Priority = 4,
	Flags = new Flags(  ) { } 
	//,description = "It enables the user to evade all attacks. Its chance of failing rises if it is used in succession."
},
new MoveDataDex() {
	num = 392,
	ID = Moves.PSYCH_UP, 
	//name = "Psych Up", 
	Function = 0x055, FunctionAsString = "055", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags(  ) { } 
	//,description = "The user hypnotizes itself into copying any stat change made by the target."
},
new MoveDataDex() {
	num = 393,
	ID = Moves.RECOVER, 
	//name = "Recover", 
	Function = 0x0D5, FunctionAsString = "0D5", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true,bite: true ) { }
	//,description = "Restoring its own cells, the user restores its own HP by half of its max HP."
},
new MoveDataDex() {
	num = 394,
	ID = Moves.RECYCLE, 
	//name = "Recycle", 
	Function = 0x0F6, FunctionAsString = "0F6", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user recycles a held item that has been used in battle so it can be used again."
},
new MoveDataDex() {
	num = 395,
	ID = Moves.REFLECT_TYPE, 
	//name = "Reflect Type", 
	Function = 0x062, FunctionAsString = "062", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true ) { }
	//,description = "The user reflects the target's type, making it the same type as the target."
},
new MoveDataDex() {
	num = 396,
	ID = Moves.REFRESH, 
	//name = "Refresh", 
	Function = 0x018, FunctionAsString = "018", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 20,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user rests to cure itself of a poisoning, burn, or paralysis."
},
new MoveDataDex() {
	num = 397,
	ID = Moves.ROAR, 
	//name = "Roar", 
	Function = 0x0EB, FunctionAsString = "0EB", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 100,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = -6,
	Flags = new Flags( protect: true,reflectable: true,mirror: true,sound: true ) { }
	//,description = "The target is scared off and replaced by another Pokémon in its party. In the wild, the battle ends."
},
new MoveDataDex() {
	num = 398,
	ID = Moves.SAFEGUARD, 
	//name = "Safeguard", 
	Function = 0x01A, FunctionAsString = "01A", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 25,
	Effects = 0,
	Target = Target.UserSide,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user creates a protective field that prevents status problems for five turns."
},
new MoveDataDex() {
	num = 399,
	ID = Moves.SCARY_FACE, 
	//name = "Scary Face", 
	Function = 0x04D, FunctionAsString = "04D", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "The user frightens the target with a scary face to harshly reduce its Speed stat."
},
new MoveDataDex() {
	num = 400,
	ID = Moves.SCREECH, 
	//name = "Screech", 
	Function = 0x04C, FunctionAsString = "04C", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 85,
	PP = 40,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true,sound: true ) { }
	//,description = "An earsplitting screech harshly reduces the target's Defense stat."
},
new MoveDataDex() {
	num = 401,
	ID = Moves.SHARPEN, 
	//name = "Sharpen", 
	Function = 0x01C, FunctionAsString = "01C", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 30,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user reduces its polygon count to make itself more jagged, raising the Attack stat."
},
new MoveDataDex() {
	num = 402,
	ID = Moves.SHELL_SMASH, 
	//name = "Shell Smash", 
	Function = 0x035, FunctionAsString = "035", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 15,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user breaks its shell, lowering its defenses but sharply raising attacking and Speed stats."
},
new MoveDataDex() {
	num = 403,
	ID = Moves.SIMPLE_BEAM, 
	//name = "Simple Beam", 
	Function = 0x063, FunctionAsString = "063", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 10,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "The user's mysterious psychic wave changes the target's Ability to Simple."
},
new MoveDataDex() {
	num = 404,
	ID = Moves.SING, 
	//name = "Sing", 
	Function = 0x003, FunctionAsString = "003", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 55,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true,sound: true ) { }
	//,description = "A soothing lullaby is sung in a calming voice that puts the target into a deep slumber."
},
new MoveDataDex() {
	num = 405,
	ID = Moves.SKETCH, 
	//name = "Sketch", 
	Function = 0x05D, FunctionAsString = "05D", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 1,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags(  ) { } 
	//,description = "It enables the user to permanently learn the move last used by the foe. Once used, Sketch disappears."
},
new MoveDataDex() {
	num = 406,
	ID = Moves.SLACK_OFF, 
	//name = "Slack Off", 
	Function = 0x0D5, FunctionAsString = "0D5", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true,bite: true ) { }
	//,description = "The user slacks off, restoring its own HP by up to half of its maximum HP."
},
new MoveDataDex() {
	num = 407,
	ID = Moves.SLEEP_TALK, 
	//name = "Sleep Talk", 
	Function = 0x0B4, FunctionAsString = "0B4", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.NoTarget,
	Priority = 0,
	Flags = new Flags(  ) { } 
	//,description = "While it is asleep, the user randomly uses one of the moves it knows."
},
new MoveDataDex() {
	num = 408,
	ID = Moves.SMOKESCREEN, 
	//name = "SmokeScreen", 
	Function = 0x047, FunctionAsString = "047", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 100,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "The user releases an obscuring cloud of smoke or ink. It reduces the target's accuracy."
},
new MoveDataDex() {
	num = 409,
	ID = Moves.SOFT_BOILED, 
	//name = "Softboiled", 
	Function = 0x0D5, FunctionAsString = "0D5", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true,bite: true ) { }
	//,description = "The user restores its own HP by up to half of its maximum HP. May also be used in the field to heal HP."
},
new MoveDataDex() {
	num = 410,
	ID = Moves.SPLASH, 
	//name = "Splash", 
	Function = 0x001, FunctionAsString = "001", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 40,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( powder: true ) { }
	//,description = "The user just flops and splashes around to no effect at all..."
},
new MoveDataDex() {
	num = 411,
	ID = Moves.STOCKPILE, 
	//name = "Stockpile", 
	Function = 0x112, FunctionAsString = "112", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 20,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user charges up power and raises both its Defense and Sp. Def. The move can be used three times."
},
new MoveDataDex() {
	num = 412,
	ID = Moves.SUBSTITUTE, 
	//name = "Substitute", 
	Function = 0x10C, FunctionAsString = "10C", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user makes a copy of itself using some of its HP. The copy serves as the user's decoy."
},
new MoveDataDex() {
	num = 413,
	ID = Moves.SUPERSONIC, 
	//name = "Supersonic", 
	Function = 0x013, FunctionAsString = "013", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 55,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true,sound: true ) { }
	//,description = "The user generates odd sound waves from its body. It may confuse the target."
},
new MoveDataDex() {
	num = 414,
	ID = Moves.SWAGGER, 
	//name = "Swagger", 
	Function = 0x041, FunctionAsString = "041", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 90,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "The user enrages and confuses the target. However, it also sharply raises the target's Attack stat."
},
new MoveDataDex() {
	num = 415,
	ID = Moves.SWALLOW, 
	//name = "Swallow", 
	Function = 0x114, FunctionAsString = "114", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true,bite: true ) { }
	//,description = "The power stored using the move Stockpile is absorbed by the user to heal its HP."
},
new MoveDataDex() {
	num = 416,
	ID = Moves.SWEET_KISS, 
	//name = "Sweet Kiss", 
	Function = 0x013, FunctionAsString = "013", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 75,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "The user kisses the target with a sweet, angelic cuteness that causes confusion."
},
new MoveDataDex() {
	num = 417,
	ID = Moves.SWEET_SCENT, 
	//name = "Sweet Scent", 
	Function = 0x048, FunctionAsString = "048", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 100,
	PP = 20,
	Effects = 0,
	Target = Target.AllOpposing,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "A sweet scent that lowers the foe's evasiveness. It also lures wild Pokémon if used in grass, etc."
},
new MoveDataDex() {
	num = 418,
	ID = Moves.SWORDS_DANCE, 
	//name = "Swords Dance", 
	Function = 0x02E, FunctionAsString = "02E", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 30,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "A frenetic dance to uplift the fighting spirit. It sharply raises the user's Attack stat."
},
new MoveDataDex() {
	num = 419,
	ID = Moves.TAIL_WHIP, 
	//name = "Tail Whip", 
	Function = 0x043, FunctionAsString = "043", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 100,
	PP = 30,
	Effects = 0,
	Target = Target.AllOpposing,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "The user wags its tail cutely, making opposing Pokémon less wary and lowering their Defense stat."
},
new MoveDataDex() {
	num = 420,
	ID = Moves.TEETER_DANCE, 
	//name = "Teeter Dance", 
	Function = 0x013, FunctionAsString = "013", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 100,
	PP = 20,
	Effects = 0,
	Target = Target.AllNonUsers,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The user performs a wobbly dance that confuses the Pokémon around it."
},
new MoveDataDex() {
	num = 421,
	ID = Moves.TICKLE, 
	//name = "Tickle", 
	Function = 0x04A, FunctionAsString = "04A", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 100,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "The user tickles the target into laughing, reducing its Attack and Defense stats."
},
new MoveDataDex() {
	num = 422,
	ID = Moves.TRANSFORM, 
	//name = "Transform", 
	Function = 0x069, FunctionAsString = "069", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags(  ) { } 
	//,description = "The user transforms into a copy of the target right down to having the same move set."
},
new MoveDataDex() {
	num = 423,
	ID = Moves.WHIRLWIND, 
	//name = "Whirlwind", 
	Function = 0x0EB, FunctionAsString = "0EB", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 100,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = -6,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "The foe is blown away, to be replaced by another Pokémon in its party. In the wild, the battle ends."
},
new MoveDataDex() {
	num = 424,
	ID = Moves.WISH, 
	//name = "Wish", 
	Function = 0x0D7, FunctionAsString = "0D7", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true,bite: true ) { }
	//,description = "One turn after this move is used, the target's HP is restored by half the user's maximum HP."
},
new MoveDataDex() {
	num = 425,
	ID = Moves.WORK_UP, 
	//name = "Work Up", 
	Function = 0x027, FunctionAsString = "027", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 30,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user is roused, and its Attack and Sp. Atk stats increase."
},
new MoveDataDex() {
	num = 426,
	ID = Moves.YAWN, 
	//name = "Yawn", 
	Function = 0x004, FunctionAsString = "004", 
	BaseDamage = 0,
	Type = Types.NORMAL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "The user lets loose a huge yawn that lulls the target into falling asleep on the next turn."
},
new MoveDataDex() {
	num = 427,
	ID = Moves.GUNK_SHOT, 
	//name = "Gunk Shot", 
	Function = 0x005, FunctionAsString = "005", 
	BaseDamage = 120,
	Type = Types.POISON,
	Category = Category.PHYSICAL,
	Accuracy = 70,
	PP = 5,
	Effects = 30,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user shoots filthy garbage at the target to attack. It may also poison the target."
},
new MoveDataDex() {
	num = 428,
	ID = Moves.SLUDGE_WAVE, 
	//name = "Sludge Wave", 
	Function = 0x005, FunctionAsString = "005", 
	BaseDamage = 95,
	Type = Types.POISON,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 10,
	Effects = 10,
	Target = Target.AllNonUsers,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "It swamps the area around the user with a giant sludge wave. It may also poison those hit."
},
new MoveDataDex() {
	num = 429,
	ID = Moves.SLUDGE_BOMB, 
	//name = "Sludge Bomb", 
	Function = 0x005, FunctionAsString = "005", 
	BaseDamage = 90,
	Type = Types.POISON,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 10,
	Effects = 30,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "Unsanitary sludge is hurled at the target. It may also poison the target."
},
new MoveDataDex() {
	num = 430,
	ID = Moves.POISON_JAB, 
	//name = "Poison Jab", 
	Function = 0x005, FunctionAsString = "005", 
	BaseDamage = 80,
	Type = Types.POISON,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 20,
	Effects = 30,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The target is stabbed with a tentacle or arm seeped with poison. It may also poison the target."
},
new MoveDataDex() {
	num = 431,
	ID = Moves.CROSS_POISON, 
	//name = "Cross Poison", 
	Function = 0x005, FunctionAsString = "005", 
	BaseDamage = 70,
	Type = Types.POISON,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 20,
	Effects = 10,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true,bite: true ) { }
	//,description = "A slashing attack with a poisonous blade that may also poison the foe. Critical hits land more easily."
},
new MoveDataDex() {
	num = 432,
	ID = Moves.SLUDGE, 
	//name = "Sludge", 
	Function = 0x005, FunctionAsString = "005", 
	BaseDamage = 65,
	Type = Types.POISON,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 20,
	Effects = 30,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "Unsanitary sludge is hurled at the target. It may also poison the target."
},
new MoveDataDex() {
	num = 433,
	ID = Moves.VENOSHOCK, 
	//name = "Venoshock", 
	Function = 0x07B, FunctionAsString = "07B", 
	BaseDamage = 65,
	Type = Types.POISON,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user drenches the foe in a special poisonous liquid. Its power doubles if the target is poisoned."
},
new MoveDataDex() {
	num = 434,
	ID = Moves.CLEAR_SMOG, 
	//name = "Clear Smog", 
	Function = 0x050, FunctionAsString = "050", 
	BaseDamage = 50,
	Type = Types.POISON,
	Category = Category.SPECIAL,
	Accuracy = 0,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user attacks by throwing a clump of special mud. All status changes are returned to normal."
},
new MoveDataDex() {
	num = 435,
	ID = Moves.POISON_FANG, 
	//name = "Poison Fang", 
	Function = 0x006, FunctionAsString = "006", 
	BaseDamage = 50,
	Type = Types.POISON,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 15,
	Effects = 30,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true ) { }
	//,description = "The user bites the target with toxic fangs. It may also leave the target badly poisoned."
},
new MoveDataDex() {
	num = 436,
	ID = Moves.POISON_TAIL, 
	//name = "Poison Tail", 
	Function = 0x005, FunctionAsString = "005", 
	BaseDamage = 50,
	Type = Types.POISON,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 25,
	Effects = 10,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true,bite: true ) { }
	//,description = "The user hits the target with its tail. It may also poison the target. Critical hits land more easily."
},
new MoveDataDex() {
	num = 437,
	ID = Moves.ACID, 
	//name = "Acid", 
	Function = 0x046, FunctionAsString = "046", 
	BaseDamage = 40,
	Type = Types.POISON,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 30,
	Effects = 10,
	Target = Target.AllOpposing,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The foe is attacked with a spray of harsh acid. It may also lower the target's Sp. Def stat."
},
new MoveDataDex() {
	num = 438,
	ID = Moves.ACID_SPRAY, 
	//name = "Acid Spray", 
	Function = 0x04F, FunctionAsString = "04F", 
	BaseDamage = 40,
	Type = Types.POISON,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 20,
	Effects = 100,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The user spits fluid that works to melt the target. This harshly reduces the target's Sp. Def stat."
},
new MoveDataDex() {
	num = 439,
	ID = Moves.SMOG, 
	//name = "Smog", 
	Function = 0x005, FunctionAsString = "005", 
	BaseDamage = 20,
	Type = Types.POISON,
	Category = Category.SPECIAL,
	Accuracy = 70,
	PP = 20,
	Effects = 40,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The target is attacked with a discharge of filthy gases. It may also poison the target."
},
new MoveDataDex() {
	num = 440,
	ID = Moves.POISON_STING, 
	//name = "Poison Sting", 
	Function = 0x005, FunctionAsString = "005", 
	BaseDamage = 15,
	Type = Types.POISON,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 35,
	Effects = 30,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The user stabs the target with a poisonous stinger. This may also poison the target."
},
new MoveDataDex() {
	num = 441,
	ID = Moves.ACID_ARMOR, 
	//name = "Acid Armor", 
	Function = 0x02F, FunctionAsString = "02F", 
	BaseDamage = 0,
	Type = Types.POISON,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 40,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user alters its cellular structure to liquefy itself, sharply raising its Defense stat."
},
new MoveDataDex() {
	num = 442,
	ID = Moves.COIL, 
	//name = "Coil", 
	Function = 0x025, FunctionAsString = "025", 
	BaseDamage = 0,
	Type = Types.POISON,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 20,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user coils up and concentrates. This raises its Attack and Defense stats as well as its accuracy."
},
new MoveDataDex() {
	num = 443,
	ID = Moves.GASTRO_ACID, 
	//name = "Gastro Acid", 
	Function = 0x068, FunctionAsString = "068", 
	BaseDamage = 0,
	Type = Types.POISON,
	Category = Category.STATUS,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "The user hurls up its stomach acids on the foe. The fluid negates the effect of the target's Ability."
},
new MoveDataDex() {
	num = 444,
	ID = Moves.POISON_GAS, 
	//name = "Poison Gas", 
	Function = 0x005, FunctionAsString = "005", 
	BaseDamage = 0,
	Type = Types.POISON,
	Category = Category.STATUS,
	Accuracy = 80,
	PP = 40,
	Effects = 0,
	Target = Target.AllOpposing,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "A cloud of poison gas is sprayed in the face of opposing Pokémon. It may poison those hit."
},
new MoveDataDex() {
	num = 445,
	ID = Moves.POISON_POWDER, 
	//name = "PoisonPowder", 
	Function = 0x005, FunctionAsString = "005", 
	BaseDamage = 0,
	Type = Types.POISON,
	Category = Category.STATUS,
	Accuracy = 75,
	PP = 35,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "The user scatters a cloud of poisonous dust on the target. It may poison the target."
},
new MoveDataDex() {
	num = 446,
	ID = Moves.TOXIC, 
	//name = "Toxic", 
	Function = 0x006, FunctionAsString = "006", 
	BaseDamage = 0,
	Type = Types.POISON,
	Category = Category.STATUS,
	Accuracy = 90,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "A move that leaves the target badly poisoned. Its poison damage worsens every turn."
},
new MoveDataDex() {
	num = 447,
	ID = Moves.TOXIC_SPIKES, 
	//name = "Toxic Spikes", 
	Function = 0x104, FunctionAsString = "104", 
	BaseDamage = 0,
	Type = Types.POISON,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 20,
	Effects = 0,
	Target = Target.OpposingSide,
	Priority = 0,
	Flags = new Flags( reflectable: true ) { }
	//,description = "The user lays a trap of poison spikes at the foe's feet. They poison foes that switch into battle."
},
new MoveDataDex() {
	num = 448,
	ID = Moves.PSYCHO_BOOST, 
	//name = "Psycho Boost", 
	Function = 0x03F, FunctionAsString = "03F", 
	BaseDamage = 140,
	Type = Types.PSYCHIC,
	Category = Category.SPECIAL,
	Accuracy = 90,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user attacks the target at full power. The attack's recoil harshly reduces the user's Sp. Atk stat."
},
new MoveDataDex() {
	num = 449,
	ID = Moves.DREAM_EATER, 
	//name = "Dream Eater", 
	Function = 0x0DE, FunctionAsString = "0DE", 
	BaseDamage = 100,
	Type = Types.PSYCHIC,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The user eats the dreams of a sleeping foe. It absorbs half the damage caused to heal the user's HP."
},
new MoveDataDex() {
	num = 450,
	ID = Moves.FUTURE_SIGHT, 
	//name = "Future Sight", 
	Function = 0x111, FunctionAsString = "111", 
	BaseDamage = 100,
	Type = Types.PSYCHIC,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags(  ) { } 
	//,description = "Two turns after this move is used, a hunk of psychic energy attacks the target."
},
new MoveDataDex() {
	num = 451,
	ID = Moves.PSYSTRIKE, 
	//name = "Psystrike", 
	Function = 0x122, FunctionAsString = "122", 
	BaseDamage = 100,
	Type = Types.PSYCHIC,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user materializes an odd psychic wave to attack the target. This attack does physical damage."
},
new MoveDataDex() {
	num = 452,
	ID = Moves.PSYCHIC, 
	//name = "Psychic", 
	Function = 0x046, FunctionAsString = "046", 
	BaseDamage = 90,
	Type = Types.PSYCHIC,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 10,
	Effects = 10,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The target is hit by a strong telekinetic force. It may also reduce the target's Sp. Def stat."
},
new MoveDataDex() {
	num = 453,
	ID = Moves.EXTRASENSORY, 
	//name = "Extrasensory", 
	Function = 0x00F, FunctionAsString = "00F", 
	BaseDamage = 80,
	Type = Types.PSYCHIC,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 30,
	Effects = 10,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The user attacks with an odd, unseeable power. It may also make the target flinch."
},
new MoveDataDex() {
	num = 454,
	ID = Moves.PSYSHOCK, 
	//name = "Psyshock", 
	Function = 0x122, FunctionAsString = "122", 
	BaseDamage = 80,
	Type = Types.PSYCHIC,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user materializes an odd psychic wave to attack the target. This attack does physical damage."
},
new MoveDataDex() {
	num = 455,
	ID = Moves.ZEN_HEADBUTT, 
	//name = "Zen Headbutt", 
	Function = 0x00F, FunctionAsString = "00F", 
	BaseDamage = 80,
	Type = Types.PSYCHIC,
	Category = Category.PHYSICAL,
	Accuracy = 90,
	PP = 15,
	Effects = 20,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user focuses its willpower to its head and attacks the foe. It may also make the target flinch."
},
new MoveDataDex() {
	num = 456,
	ID = Moves.LUSTER_PURGE, 
	//name = "Luster Purge", 
	Function = 0x046, FunctionAsString = "046", 
	BaseDamage = 70,
	Type = Types.PSYCHIC,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 5,
	Effects = 50,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The user lets loose a damaging burst of light. It may also reduce the target's Sp. Def stat."
},
new MoveDataDex() {
	num = 457,
	ID = Moves.MIST_BALL, 
	//name = "Mist Ball", 
	Function = 0x045, FunctionAsString = "045", 
	BaseDamage = 70,
	Type = Types.PSYCHIC,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 5,
	Effects = 50,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "A mistlike flurry of down envelops and damages the target. It may also lower the target's Sp. Atk."
},
new MoveDataDex() {
	num = 458,
	ID = Moves.PSYCHO_CUT, 
	//name = "Psycho Cut", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 70,
	Type = Types.PSYCHIC,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true,bite: true ) { }
	//,description = "The user tears at the target with blades formed by psychic power. Critical hits land more easily."
},
new MoveDataDex() {
	num = 459,
	ID = Moves.SYNCHRONOISE, 
	//name = "Synchronoise", 
	Function = 0x123, FunctionAsString = "123", 
	BaseDamage = 70,
	Type = Types.PSYCHIC,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.AllNonUsers,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "Using an odd shock wave, the user damages any Pokémon of the same type as the user."
},
new MoveDataDex() {
	num = 460,
	ID = Moves.PSYBEAM, 
	//name = "Psybeam", 
	Function = 0x013, FunctionAsString = "013", 
	BaseDamage = 65,
	Type = Types.PSYCHIC,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 20,
	Effects = 10,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The target is attacked with a peculiar ray. It may also cause confusion."
},
new MoveDataDex() {
	num = 461,
	ID = Moves.HEART_STAMP, 
	//name = "Heart Stamp", 
	Function = 0x00F, FunctionAsString = "00F", 
	BaseDamage = 60,
	Type = Types.PSYCHIC,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 25,
	Effects = 30,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true ) { }
	//,description = "The user unleashes a vicious blow after its cute act makes the foe less wary. It may also cause flinching."
},
new MoveDataDex() {
	num = 462,
	ID = Moves.CONFUSION, 
	//name = "Confusion", 
	Function = 0x013, FunctionAsString = "013", 
	BaseDamage = 50,
	Type = Types.PSYCHIC,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 25,
	Effects = 10,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The target is hit by a weak telekinetic force. It may also leave the target confused."
},
new MoveDataDex() {
	num = 463,
	ID = Moves.MIRROR_COAT, 
	//name = "Mirror Coat", 
	Function = 0x072, FunctionAsString = "072", 
	BaseDamage = 1,
	Type = Types.PSYCHIC,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 20,
	Effects = 0,
	Target = Target.NoTarget,
	Priority = -5,
	Flags = new Flags( protect: true ) { }
	//,description = "A retaliation move that counters any special attack, inflicting double the damage taken."
},
new MoveDataDex() {
	num = 464,
	ID = Moves.PSYWAVE, 
	//name = "Psywave", 
	Function = 0x06F, FunctionAsString = "06F", 
	BaseDamage = 1,
	Type = Types.PSYCHIC,
	Category = Category.SPECIAL,
	Accuracy = 80,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The target is attacked with an odd psychic wave. The attack varies in intensity."
},
new MoveDataDex() {
	num = 465,
	ID = Moves.STORED_POWER, 
	//name = "Stored Power", 
	Function = 0x08E, FunctionAsString = "08E", 
	BaseDamage = 1,
	Type = Types.PSYCHIC,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user attacks with stored power. The more the user's stats are raised, the greater the damage."
},
new MoveDataDex() {
	num = 466,
	ID = Moves.AGILITY, 
	//name = "Agility", 
	Function = 0x030, FunctionAsString = "030", 
	BaseDamage = 0,
	Type = Types.PSYCHIC,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 30,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user relaxes and lightens its body to move faster. It sharply boosts the Speed stat."
},
new MoveDataDex() {
	num = 467,
	ID = Moves.ALLY_SWITCH, 
	//name = "Ally Switch", 
	Function = 0x120, FunctionAsString = "120", 
	BaseDamage = 0,
	Type = Types.PSYCHIC,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 15,
	Effects = 0,
	Target = Target.User,
	Priority = 1,
	Flags = new Flags(  ) { } 
	//,description = "The user teleports using a strange power and switches its place with one of its allies."
},
new MoveDataDex() {
	num = 468,
	ID = Moves.AMNESIA, 
	//name = "Amnesia", 
	Function = 0x033, FunctionAsString = "033", 
	BaseDamage = 0,
	Type = Types.PSYCHIC,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 20,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user temporarily empties its mind to forget its concerns. It sharply raises the user's Sp. Def stat."
},
new MoveDataDex() {
	num = 469,
	ID = Moves.BARRIER, 
	//name = "Barrier", 
	Function = 0x02F, FunctionAsString = "02F", 
	BaseDamage = 0,
	Type = Types.PSYCHIC,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 30,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user throws up a sturdy wall that sharply raises its Defense stat."
},
new MoveDataDex() {
	num = 470,
	ID = Moves.CALM_MIND, 
	//name = "Calm Mind", 
	Function = 0x02C, FunctionAsString = "02C", 
	BaseDamage = 0,
	Type = Types.PSYCHIC,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 20,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user quietly focuses its mind and calms its spirit to raise its Sp. Atk and Sp. Def stats."
},
new MoveDataDex() {
	num = 471,
	ID = Moves.COSMIC_POWER, 
	//name = "Cosmic Power", 
	Function = 0x02A, FunctionAsString = "02A", 
	BaseDamage = 0,
	Type = Types.PSYCHIC,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 20,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user absorbs a mystical power from space to raise its Defense and Sp. Def stats."
},
new MoveDataDex() {
	num = 472,
	ID = Moves.GRAVITY, 
	//name = "Gravity", 
	Function = 0x118, FunctionAsString = "118", 
	BaseDamage = 0,
	Type = Types.PSYCHIC,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 5,
	Effects = 0,
	Target = Target.BothSides,
	Priority = 0,
	Flags = new Flags(  ) { } 
	//,description = "Gravity is intensified for five turns, making moves involving flying unusable and negating Levitation."
},
new MoveDataDex() {
	num = 473,
	ID = Moves.GUARD_SPLIT, 
	//name = "Guard Split", 
	Function = 0x059, FunctionAsString = "059", 
	BaseDamage = 0,
	Type = Types.PSYCHIC,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true ) { }
	//,description = "The user employs its psychic power to average its Defense and Sp. Def stats with those of its target's."
},
new MoveDataDex() {
	num = 474,
	ID = Moves.GUARD_SWAP, 
	//name = "Guard Swap", 
	Function = 0x053, FunctionAsString = "053", 
	BaseDamage = 0,
	Type = Types.PSYCHIC,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The user employs its psychic power to switch changes to its Defense and Sp. Def with the target."
},
new MoveDataDex() {
	num = 475,
	ID = Moves.HEAL_BLOCK, 
	//name = "Heal Block", 
	Function = 0x0BB, FunctionAsString = "0BB", 
	BaseDamage = 0,
	Type = Types.PSYCHIC,
	Category = Category.STATUS,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.AllOpposing,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "For five turns, the foe is prevented from using any moves, Abilities, or held items that recover HP."
},
new MoveDataDex() {
	num = 476,
	ID = Moves.HEAL_PULSE, 
	//name = "Heal Pulse", 
	Function = 0x0DF, FunctionAsString = "0DF", 
	BaseDamage = 0,
	Type = Types.PSYCHIC,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,bite: true ) { }
	//,description = "The user emits a healing pulse which restores the target's HP by up to half of its max HP."
},
new MoveDataDex() {
	num = 477,
	ID = Moves.HEALING_WISH, 
	//name = "Healing Wish", 
	Function = 0x0E3, FunctionAsString = "0E3", 
	BaseDamage = 0,
	Type = Types.PSYCHIC,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true,bite: true ) { }
	//,description = "The user faints. In return, the Pokémon taking its place will have its HP restored and status cured."
},
new MoveDataDex() {
	num = 478,
	ID = Moves.HEART_SWAP, 
	//name = "Heart Swap", 
	Function = 0x054, FunctionAsString = "054", 
	BaseDamage = 0,
	Type = Types.PSYCHIC,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The user employs its psychic power to switch stat changes with the target."
},
new MoveDataDex() {
	num = 479,
	ID = Moves.HYPNOSIS, 
	//name = "Hypnosis", 
	Function = 0x003, FunctionAsString = "003", 
	BaseDamage = 0,
	Type = Types.PSYCHIC,
	Category = Category.STATUS,
	Accuracy = 60,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "The user employs hypnotic suggestion to make the target fall into a deep sleep."
},
new MoveDataDex() {
	num = 480,
	ID = Moves.IMPRISON, 
	//name = "Imprison", 
	Function = 0x0B8, FunctionAsString = "0B8", 
	BaseDamage = 0,
	Type = Types.PSYCHIC,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "If the foe knows any move also known by the user, the foe is prevented from using it."
},
new MoveDataDex() {
	num = 481,
	ID = Moves.KINESIS, 
	//name = "Kinesis", 
	Function = 0x047, FunctionAsString = "047", 
	BaseDamage = 0,
	Type = Types.PSYCHIC,
	Category = Category.STATUS,
	Accuracy = 80,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "The user distracts the target by bending a spoon. It lowers the target's accuracy."
},
new MoveDataDex() {
	num = 482,
	ID = Moves.LIGHT_SCREEN, 
	//name = "Light Screen", 
	Function = 0x0A3, FunctionAsString = "0A3", 
	BaseDamage = 0,
	Type = Types.PSYCHIC,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 30,
	Effects = 0,
	Target = Target.UserSide,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "A wondrous wall of light is put up to suppress damage from special attacks for five turns."
},
new MoveDataDex() {
	num = 483,
	ID = Moves.LUNAR_DANCE, 
	//name = "Lunar Dance", 
	Function = 0x0E4, FunctionAsString = "0E4", 
	BaseDamage = 0,
	Type = Types.PSYCHIC,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true,bite: true ) { }
	//,description = "The user faints. In return, the Pokémon taking its place will have its status and HP fully restored."
},
new MoveDataDex() {
	num = 484,
	ID = Moves.MAGIC_COAT, 
	//name = "Magic Coat", 
	Function = 0x0B1, FunctionAsString = "0B1", 
	BaseDamage = 0,
	Type = Types.PSYCHIC,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 15,
	Effects = 0,
	Target = Target.User,
	Priority = 4,
	Flags = new Flags(  ) { } 
	//,description = "A barrier reflects back to the target moves like Leech Seed and moves that damage status."
},
new MoveDataDex() {
	num = 485,
	ID = Moves.MAGIC_ROOM, 
	//name = "Magic Room", 
	Function = 0x0F9, FunctionAsString = "0F9", 
	BaseDamage = 0,
	Type = Types.PSYCHIC,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.BothSides,
	Priority = -7,
	Flags = new Flags( mirror: true ) { }
	//,description = "The user creates a bizarre area in which Pokémon's held items lose their effects for five turns."
},
new MoveDataDex() {
	num = 486,
	ID = Moves.MEDITATE, 
	//name = "Meditate", 
	Function = 0x01C, FunctionAsString = "01C", 
	BaseDamage = 0,
	Type = Types.PSYCHIC,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 40,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user meditates to awaken the power deep within its body and raise its Attack stat."
},
new MoveDataDex() {
	num = 487,
	ID = Moves.MIRACLE_EYE, 
	//name = "Miracle Eye", 
	Function = 0x0A8, FunctionAsString = "0A8", 
	BaseDamage = 0,
	Type = Types.PSYCHIC,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 40,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "Enables the user to hit a Dark type with any type of move. It also enables the user to hit an evasive foe."
},
new MoveDataDex() {
	num = 488,
	ID = Moves.POWER_SPLIT, 
	//name = "Power Split", 
	Function = 0x058, FunctionAsString = "058", 
	BaseDamage = 0,
	Type = Types.PSYCHIC,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true ) { }
	//,description = "The user employs its psychic power to average its Attack and Sp. Atk stats with those of the target's."
},
new MoveDataDex() {
	num = 489,
	ID = Moves.POWER_SWAP, 
	//name = "Power Swap", 
	Function = 0x052, FunctionAsString = "052", 
	BaseDamage = 0,
	Type = Types.PSYCHIC,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The user employs its psychic power to switch changes to its Attack and Sp. Atk with the target."
},
new MoveDataDex() {
	num = 490,
	ID = Moves.POWER_TRICK, 
	//name = "Power Trick", 
	Function = 0x057, FunctionAsString = "057", 
	BaseDamage = 0,
	Type = Types.PSYCHIC,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user employs its psychic power to switch its Attack with its Defense stat."
},
new MoveDataDex() {
	num = 491,
	ID = Moves.PSYCHO_SHIFT, 
	//name = "Psycho Shift", 
	Function = 0x01B, FunctionAsString = "01B", 
	BaseDamage = 0,
	Type = Types.PSYCHIC,
	Category = Category.STATUS,
	Accuracy = 90,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "Using its psychic power of suggestion, the user transfers its status problems to the target."
},
new MoveDataDex() {
	num = 492,
	ID = Moves.REFLECT, 
	//name = "Reflect", 
	Function = 0x0A2, FunctionAsString = "0A2", 
	BaseDamage = 0,
	Type = Types.PSYCHIC,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 20,
	Effects = 0,
	Target = Target.UserSide,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "A wondrous wall of light is put up to suppress damage from physical attacks for five turns."
},
new MoveDataDex() {
	num = 493,
	ID = Moves.REST, 
	//name = "Rest", 
	Function = 0x0D9, FunctionAsString = "0D9", 
	BaseDamage = 0,
	Type = Types.PSYCHIC,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true,bite: true ) { }
	//,description = "The user goes to sleep for two turns. It fully restores the user's HP and heals any status problem."
},
new MoveDataDex() {
	num = 494,
	ID = Moves.ROLE_PLAY, 
	//name = "Role Play", 
	Function = 0x065, FunctionAsString = "065", 
	BaseDamage = 0,
	Type = Types.PSYCHIC,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags(  ) { } 
	//,description = "The user mimics the target completely, copying the target's natural Ability."
},
new MoveDataDex() {
	num = 495,
	ID = Moves.SKILL_SWAP, 
	//name = "Skill Swap", 
	Function = 0x067, FunctionAsString = "067", 
	BaseDamage = 0,
	Type = Types.PSYCHIC,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The user employs its psychic power to exchange Abilities with the target."
},
new MoveDataDex() {
	num = 496,
	ID = Moves.TELEKINESIS, 
	//name = "Telekinesis", 
	Function = 0x11A, FunctionAsString = "11A", 
	BaseDamage = 0,
	Type = Types.PSYCHIC,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true,powder: true ) { }
	//,description = "The user makes the target float with its psychic power. The target is easier to hit for three turns."
},
new MoveDataDex() {
	num = 497,
	ID = Moves.TELEPORT, 
	//name = "Teleport", 
	Function = 0x0EA, FunctionAsString = "0EA", 
	BaseDamage = 0,
	Type = Types.PSYCHIC,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 20,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags(  ) { } 
	//,description = "Use it to flee from any wild Pokémon. It can also warp to the last Pokémon Center visited."
},
new MoveDataDex() {
	num = 498,
	ID = Moves.TRICK, 
	//name = "Trick", 
	Function = 0x0F2, FunctionAsString = "0F2", 
	BaseDamage = 0,
	Type = Types.PSYCHIC,
	Category = Category.STATUS,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The user catches the target off guard and swaps its held item with its own."
},
new MoveDataDex() {
	num = 499,
	ID = Moves.TRICK_ROOM, 
	//name = "Trick Room", 
	Function = 0x11F, FunctionAsString = "11F", 
	BaseDamage = 0,
	Type = Types.PSYCHIC,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 5,
	Effects = 0,
	Target = Target.BothSides,
	Priority = -7,
	Flags = new Flags( mirror: true ) { }
	//,description = "The user creates a bizarre area in which slower Pokémon get to move first for five turns."
},
new MoveDataDex() {
	num = 500,
	ID = Moves.WONDER_ROOM, 
	//name = "Wonder Room", 
	Function = 0x124, FunctionAsString = "124", 
	BaseDamage = 0,
	Type = Types.PSYCHIC,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.BothSides,
	Priority = -7,
	Flags = new Flags( mirror: true ) { }
	//,description = "The user creates a bizarre area in which Pokémon's Defense and Sp. Def stats are swapped for 5 turns."
},
new MoveDataDex() {
	num = 501,
	ID = Moves.HEAD_SMASH, 
	//name = "Head Smash", 
	Function = 0x0FC, FunctionAsString = "0FC", 
	BaseDamage = 150,
	Type = Types.ROCK,
	Category = Category.PHYSICAL,
	Accuracy = 80,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user attacks the foe with a hazardous, full-power headbutt. The user also takes terrible damage."
},
new MoveDataDex() {
	num = 502,
	ID = Moves.ROCK_WRECKER, 
	//name = "Rock Wrecker", 
	Function = 0x0C2, FunctionAsString = "0C2", 
	BaseDamage = 150,
	Type = Types.ROCK,
	Category = Category.PHYSICAL,
	Accuracy = 90,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user launches a huge boulder at the target to attack. It must rest on the next turn, however."
},
new MoveDataDex() {
	num = 503,
	ID = Moves.STONE_EDGE, 
	//name = "Stone Edge", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 100,
	Type = Types.ROCK,
	Category = Category.PHYSICAL,
	Accuracy = 80,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true,bite: true ) { }
	//,description = "The user stabs the foe with sharpened stones from below. It has a high critical-hit ratio."
},
new MoveDataDex() {
	num = 504,
	ID = Moves.ROCK_SLIDE, 
	//name = "Rock Slide", 
	Function = 0x00F, FunctionAsString = "00F", 
	BaseDamage = 75,
	Type = Types.ROCK,
	Category = Category.PHYSICAL,
	Accuracy = 90,
	PP = 10,
	Effects = 30,
	Target = Target.AllOpposing,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "Large boulders are hurled at the foes to inflict damage. It may also make the targets flinch."
},
new MoveDataDex() {
	num = 505,
	ID = Moves.POWER_GEM, 
	//name = "Power Gem", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 70,
	Type = Types.ROCK,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user attacks with a ray of light that sparkles as if it were made of gemstones."
},
new MoveDataDex() {
	num = 506,
	ID = Moves.ANCIENT_POWER, 
	//name = "AncientPower", 
	Function = 0x02D, FunctionAsString = "02D", 
	BaseDamage = 60,
	Type = Types.ROCK,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 5,
	Effects = 10,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The user attacks with a prehistoric power. It may also raise all the user's stats at once."
},
new MoveDataDex() {
	num = 507,
	ID = Moves.ROCK_THROW, 
	//name = "Rock Throw", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 50,
	Type = Types.ROCK,
	Category = Category.PHYSICAL,
	Accuracy = 90,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user picks up and throws a small rock at the target to attack."
},
new MoveDataDex() {
	num = 508,
	ID = Moves.ROCK_TOMB, 
	//name = "Rock Tomb", 
	Function = 0x044, FunctionAsString = "044", 
	BaseDamage = 50,
	Type = Types.ROCK,
	Category = Category.PHYSICAL,
	Accuracy = 80,
	PP = 10,
	Effects = 100,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "Boulders are hurled at the target. It also lowers the target's Speed by preventing its movement."
},
new MoveDataDex() {
	num = 509,
	ID = Moves.SMACK_DOWN, 
	//name = "Smack Down", 
	Function = 0x11C, FunctionAsString = "11C", 
	BaseDamage = 50,
	Type = Types.ROCK,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user throws a stone or projectile to attack. A flying Pokémon will fall to the ground when hit."
},
new MoveDataDex() {
	num = 510,
	ID = Moves.ROLLOUT, 
	//name = "Rollout", 
	Function = 0x0D3, FunctionAsString = "0D3", 
	BaseDamage = 30,
	Type = Types.ROCK,
	Category = Category.PHYSICAL,
	Accuracy = 90,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user continually rolls into the target over five turns. It becomes stronger each time it hits."
},
new MoveDataDex() {
	num = 511,
	ID = Moves.ROCK_BLAST, 
	//name = "Rock Blast", 
	Function = 0x0C0, FunctionAsString = "0C0", 
	BaseDamage = 25,
	Type = Types.ROCK,
	Category = Category.PHYSICAL,
	Accuracy = 90,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user hurls hard rocks at the target. Two to five rocks are launched in quick succession."
},
new MoveDataDex() {
	num = 512,
	ID = Moves.ROCK_POLISH, 
	//name = "Rock Polish", 
	Function = 0x030, FunctionAsString = "030", 
	BaseDamage = 0,
	Type = Types.ROCK,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 20,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user polishes its body to reduce drag. It can sharply raise the Speed stat."
},
new MoveDataDex() {
	num = 513,
	ID = Moves.SANDSTORM, 
	//name = "Sandstorm", 
	Function = 0x101, FunctionAsString = "101", 
	BaseDamage = 0,
	Type = Types.ROCK,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.BothSides,
	Priority = 0,
	Flags = new Flags(  ) { } 
	//,description = "Summons a five-turn sandstorm to hurt all combatants except the Rock, Ground, and Steel types."
},
new MoveDataDex() {
	num = 514,
	ID = Moves.STEALTH_ROCK, 
	//name = "Stealth Rock", 
	Function = 0x105, FunctionAsString = "105", 
	BaseDamage = 0,
	Type = Types.ROCK,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 20,
	Effects = 0,
	Target = Target.OpposingSide,
	Priority = 0,
	Flags = new Flags( reflectable: true ) { }
	//,description = "The user lays a trap of levitating stones around the foe. The trap hurts foes that switch into battle."
},
new MoveDataDex() {
	num = 515,
	ID = Moves.WIDE_GUARD, 
	//name = "Wide Guard", 
	Function = 0x0AC, FunctionAsString = "0AC", 
	BaseDamage = 0,
	Type = Types.ROCK,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.UserSide,
	Priority = 3,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user and its allies are protected from wide-ranging attacks for a turn. May fail if used in succession."
},
new MoveDataDex() {
	num = 516,
	ID = Moves.DOOM_DESIRE, 
	//name = "Doom Desire", 
	Function = 0x111, FunctionAsString = "111", 
	BaseDamage = 140,
	Type = Types.STEEL,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags(  ) { } 
	//,description = "Two turns after this move is used, the user blasts the target with a concentrated bundle of light."
},
new MoveDataDex() {
	num = 517,
	ID = Moves.IRON_TAIL, 
	//name = "Iron Tail", 
	Function = 0x043, FunctionAsString = "043", 
	BaseDamage = 100,
	Type = Types.STEEL,
	Category = Category.PHYSICAL,
	Accuracy = 75,
	PP = 15,
	Effects = 30,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true ) { }
	//,description = "The target is slammed with a steel-hard tail. It may also lower the target's Defense stat."
},
new MoveDataDex() {
	num = 518,
	ID = Moves.METEOR_MASH, 
	//name = "Meteor Mash", 
	Function = 0x01C, FunctionAsString = "01C", 
	BaseDamage = 100,
	Type = Types.STEEL,
	Category = Category.PHYSICAL,
	Accuracy = 85,
	PP = 10,
	Effects = 20,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true,punch: true ) { }
	//,description = "The target is hit with a hard punch fired like a meteor. It may also raise the user's Attack."
},
new MoveDataDex() {
	num = 519,
	ID = Moves.FLASH_CANNON, 
	//name = "Flash Cannon", 
	Function = 0x046, FunctionAsString = "046", 
	BaseDamage = 80,
	Type = Types.STEEL,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 10,
	Effects = 10,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user gathers all its light energy and releases it at once. It may also lower the target's Sp. Def stat."
},
new MoveDataDex() {
	num = 520,
	ID = Moves.IRON_HEAD, 
	//name = "Iron Head", 
	Function = 0x00F, FunctionAsString = "00F", 
	BaseDamage = 80,
	Type = Types.STEEL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 15,
	Effects = 30,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The foe slams the target with its steel-hard head. It may also make the target flinch."
},
new MoveDataDex() {
	num = 521,
	ID = Moves.STEEL_WING, 
	//name = "Steel Wing", 
	Function = 0x01D, FunctionAsString = "01D", 
	BaseDamage = 70,
	Type = Types.STEEL,
	Category = Category.PHYSICAL,
	Accuracy = 90,
	PP = 25,
	Effects = 10,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The target is hit with wings of steel. It may also raise the user's Defense stat."
},
new MoveDataDex() {
	num = 522,
	ID = Moves.MIRROR_SHOT, 
	//name = "Mirror Shot", 
	Function = 0x047, FunctionAsString = "047", 
	BaseDamage = 65,
	Type = Types.STEEL,
	Category = Category.SPECIAL,
	Accuracy = 85,
	PP = 10,
	Effects = 30,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user looses a flash of energy from its polished body. It may also lower the target's accuracy."
},
new MoveDataDex() {
	num = 523,
	ID = Moves.MAGNET_BOMB, 
	//name = "Magnet Bomb", 
	Function = 0x0A5, FunctionAsString = "0A5", 
	BaseDamage = 60,
	Type = Types.STEEL,
	Category = Category.PHYSICAL,
	Accuracy = 0,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user launches steel bombs that stick to the target. This attack will not miss."
},
new MoveDataDex() {
	num = 524,
	ID = Moves.GEAR_GRIND, 
	//name = "Gear Grind", 
	Function = 0x0BD, FunctionAsString = "0BD", 
	BaseDamage = 50,
	Type = Types.STEEL,
	Category = Category.PHYSICAL,
	Accuracy = 85,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user attacks by throwing two steel gears at its target."
},
new MoveDataDex() {
	num = 525,
	ID = Moves.METAL_CLAW, 
	//name = "Metal Claw", 
	Function = 0x01C, FunctionAsString = "01C", 
	BaseDamage = 50,
	Type = Types.STEEL,
	Category = Category.PHYSICAL,
	Accuracy = 95,
	PP = 35,
	Effects = 10,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true ) { }
	//,description = "The target is raked with steel claws. It may also raise the user's Attack stat."
},
new MoveDataDex() {
	num = 526,
	ID = Moves.BULLET_PUNCH, 
	//name = "Bullet Punch", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 40,
	Type = Types.STEEL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 30,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 1,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true,punch: true ) { }
	//,description = "The user strikes the target with tough punches as fast as bullets. This move always goes first."
},
new MoveDataDex() {
	num = 527,
	ID = Moves.GYRO_BALL, 
	//name = "Gyro Ball", 
	Function = 0x08D, FunctionAsString = "08D", 
	BaseDamage = 1,
	Type = Types.STEEL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user tackles the target with a high-speed spin. The slower the user, the greater the damage."
},
new MoveDataDex() {
	num = 528,
	ID = Moves.HEAVY_SLAM, 
	//name = "Heavy Slam", 
	Function = 0x09B, FunctionAsString = "09B", 
	BaseDamage = 1,
	Type = Types.STEEL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user slams into the foe with its heavy body. The heavier the user, the greater the damage."
},
new MoveDataDex() {
	num = 529,
	ID = Moves.METAL_BURST, 
	//name = "Metal Burst", 
	Function = 0x073, FunctionAsString = "073", 
	BaseDamage = 1,
	Type = Types.STEEL,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.NoTarget,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The user retaliates with much greater power against the target that last inflicted damage on it."
},
new MoveDataDex() {
	num = 530,
	ID = Moves.AUTOTOMIZE, 
	//name = "Autotomize", 
	Function = 0x031, FunctionAsString = "031", 
	BaseDamage = 0,
	Type = Types.STEEL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 15,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user sheds part of its body to make itself lighter and sharply raise its Speed stat."
},
new MoveDataDex() {
	num = 531,
	ID = Moves.IRON_DEFENSE, 
	//name = "Iron Defense", 
	Function = 0x02F, FunctionAsString = "02F", 
	BaseDamage = 0,
	Type = Types.STEEL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 15,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user hardens its body's surface like iron, sharply raising its Defense stat."
},
new MoveDataDex() {
	num = 532,
	ID = Moves.METAL_SOUND, 
	//name = "Metal Sound", 
	Function = 0x04F, FunctionAsString = "04F", 
	BaseDamage = 0,
	Type = Types.STEEL,
	Category = Category.STATUS,
	Accuracy = 85,
	PP = 40,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true,sound: true ) { }
	//,description = "A horrible sound like scraping metal harshly reduces the target's Sp. Def stat."
},
new MoveDataDex() {
	num = 533,
	ID = Moves.SHIFT_GEAR, 
	//name = "Shift Gear", 
	Function = 0x036, FunctionAsString = "036", 
	BaseDamage = 0,
	Type = Types.STEEL,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 10,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user rotates its gears, raising its Attack and sharply raising its Speed."
},
new MoveDataDex() {
	num = 534,
	ID = Moves.HYDRO_CANNON, 
	//name = "Hydro Cannon", 
	Function = 0x0C2, FunctionAsString = "0C2", 
	BaseDamage = 150,
	Type = Types.WATER,
	Category = Category.SPECIAL,
	Accuracy = 90,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The target is hit with a watery blast. The user must rest on the next turn, however."
},
new MoveDataDex() {
	num = 535,
	ID = Moves.WATER_SPOUT, 
	//name = "Water Spout", 
	Function = 0x08B, FunctionAsString = "08B", 
	BaseDamage = 150,
	Type = Types.WATER,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 5,
	Effects = 0,
	Target = Target.AllOpposing,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The user spouts water to damage the foe. The lower the user's HP, the less powerful it becomes."
},
new MoveDataDex() {
	num = 536,
	ID = Moves.HYDRO_PUMP, 
	//name = "Hydro Pump", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 120,
	Type = Types.WATER,
	Category = Category.SPECIAL,
	Accuracy = 80,
	PP = 5,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The target is blasted by a huge volume of water launched under great pressure."
},
new MoveDataDex() {
	num = 537,
	ID = Moves.MUDDY_WATER, 
	//name = "Muddy Water", 
	Function = 0x047, FunctionAsString = "047", 
	BaseDamage = 95,
	Type = Types.WATER,
	Category = Category.SPECIAL,
	Accuracy = 85,
	PP = 10,
	Effects = 30,
	Target = Target.AllOpposing,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user attacks by shooting muddy water at the opposing team. It may also lower the target's accuracy."
},
new MoveDataDex() {
	num = 538,
	ID = Moves.SURF, 
	//name = "Surf", 
	Function = 0x075, FunctionAsString = "075", 
	BaseDamage = 95,
	Type = Types.WATER,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 15,
	Effects = 0,
	Target = Target.AllNonUsers,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "It swamps the area around the user with a giant wave. It can also be used for crossing water."
},
new MoveDataDex() {
	num = 539,
	ID = Moves.AQUA_TAIL, 
	//name = "Aqua Tail", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 90,
	Type = Types.WATER,
	Category = Category.PHYSICAL,
	Accuracy = 90,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user attacks by swinging its tail as if it were a vicious wave in a raging storm."
},
new MoveDataDex() {
	num = 540,
	ID = Moves.CRABHAMMER, 
	//name = "Crabhammer", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 90,
	Type = Types.WATER,
	Category = Category.PHYSICAL,
	Accuracy = 90,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true,bite: true ) { }
	//,description = "The target is hammered with a large pincer. Critical hits land more easily."
},
new MoveDataDex() {
	num = 541,
	ID = Moves.DIVE, 
	//name = "Dive", 
	Function = 0x0CB, FunctionAsString = "0CB", 
	BaseDamage = 80,
	Type = Types.WATER,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "Diving on the first turn, the user rises and hits on the next turn. It can be used to dive in the ocean."
},
new MoveDataDex() {
	num = 542,
	ID = Moves.SCALD, 
	//name = "Scald", 
	Function = 0x00A, FunctionAsString = "00A", 
	BaseDamage = 80,
	Type = Types.WATER,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 15,
	Effects = 30,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true,defrost: true ) { }
	//,description = "The user shoots boiling hot water at its target. It may also leave the target with a burn."
},
new MoveDataDex() {
	num = 543,
	ID = Moves.WATERFALL, 
	//name = "Waterfall", 
	Function = 0x00F, FunctionAsString = "00F", 
	BaseDamage = 80,
	Type = Types.WATER,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 15,
	Effects = 20,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user charges at the target and may make it flinch. It can also be used to climb a waterfall."
},
new MoveDataDex() {
	num = 544,
	ID = Moves.RAZOR_SHELL, 
	//name = "Razor Shell", 
	Function = 0x043, FunctionAsString = "043", 
	BaseDamage = 75,
	Type = Types.WATER,
	Category = Category.PHYSICAL,
	Accuracy = 95,
	PP = 10,
	Effects = 50,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true ) { }
	//,description = "The user cuts the foe with sharp shells. It may also lower the target's Defense stat."
},
new MoveDataDex() {
	num = 545,
	ID = Moves.BRINE, 
	//name = "Brine", 
	Function = 0x080, FunctionAsString = "080", 
	BaseDamage = 65,
	Type = Types.WATER,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "If the target's HP is down to about half, this attack will hit with double the power."
},
new MoveDataDex() {
	num = 546,
	ID = Moves.BUBBLE_BEAM, 
	//name = "BubbleBeam", 
	Function = 0x044, FunctionAsString = "044", 
	BaseDamage = 65,
	Type = Types.WATER,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 20,
	Effects = 10,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "A spray of bubbles is forcefully ejected at the target. It may also lower its Speed stat."
},
new MoveDataDex() {
	num = 547,
	ID = Moves.OCTAZOOKA, 
	//name = "Octazooka", 
	Function = 0x047, FunctionAsString = "047", 
	BaseDamage = 65,
	Type = Types.WATER,
	Category = Category.SPECIAL,
	Accuracy = 85,
	PP = 10,
	Effects = 50,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "The user attacks by spraying ink in the foe's face or eyes. It may also lower the target's accuracy."
},
new MoveDataDex() {
	num = 548,
	ID = Moves.WATER_PULSE, 
	//name = "Water Pulse", 
	Function = 0x013, FunctionAsString = "013", 
	BaseDamage = 60,
	Type = Types.WATER,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 20,
	Effects = 20,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The user attacks the target with a pulsing blast of water. It may also confuse the target."
},
new MoveDataDex() {
	num = 549,
	ID = Moves.WATER_PLEDGE, 
	//name = "Water Pledge", 
	Function = 0x108, FunctionAsString = "108", 
	BaseDamage = 50,
	Type = Types.WATER,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "A column of water strikes the target. When combined with its fire equivalent, it makes a rainbow."
},
new MoveDataDex() {
	num = 550,
	ID = Moves.AQUA_JET, 
	//name = "Aqua Jet", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 40,
	Type = Types.WATER,
	Category = Category.PHYSICAL,
	Accuracy = 100,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 1,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The user lunges at the target at a speed that makes it almost invisible. It is sure to strike first."
},
new MoveDataDex() {
	num = 551,
	ID = Moves.WATER_GUN, 
	//name = "Water Gun", 
	Function = 0x000, FunctionAsString = "000", 
	BaseDamage = 40,
	Type = Types.WATER,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 25,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "The target is blasted with a forceful shot of water."
},
new MoveDataDex() {
	num = 552,
	ID = Moves.CLAMP, 
	//name = "Clamp", 
	Function = 0x0CF, FunctionAsString = "0CF", 
	BaseDamage = 35,
	Type = Types.WATER,
	Category = Category.PHYSICAL,
	Accuracy = 85,
	PP = 10,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( contact: true,protect: true,mirror: true,flinch: true ) { }
	//,description = "The target is clamped and squeezed by the user's very thick and sturdy shell for four to five turns."
},
new MoveDataDex() {
	num = 553,
	ID = Moves.WHIRLPOOL, 
	//name = "Whirlpool", 
	Function = 0x0D0, FunctionAsString = "0D0", 
	BaseDamage = 35,
	Type = Types.WATER,
	Category = Category.SPECIAL,
	Accuracy = 85,
	PP = 15,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true,flinch: true ) { }
	//,description = "Traps foes in a violent swirling whirlpool for four to five turns."
},
new MoveDataDex() {
	num = 554,
	ID = Moves.BUBBLE, 
	//name = "Bubble", 
	Function = 0x044, FunctionAsString = "044", 
	BaseDamage = 20,
	Type = Types.WATER,
	Category = Category.SPECIAL,
	Accuracy = 100,
	PP = 30,
	Effects = 10,
	Target = Target.AllOpposing,
	Priority = 0,
	Flags = new Flags( protect: true,mirror: true ) { }
	//,description = "A spray of countless bubbles is jetted at the opposing team. It may also lower the targets' Speed stats."
},
new MoveDataDex() {
	num = 555,
	ID = Moves.AQUA_RING, 
	//name = "Aqua Ring", 
	Function = 0x0DA, FunctionAsString = "0DA", 
	BaseDamage = 0,
	Type = Types.WATER,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 20,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true,bite: true ) { }
	//,description = "The user envelops itself in a veil made of water. It regains some HP on every turn."
},
new MoveDataDex() {
	num = 556,
	ID = Moves.RAIN_DANCE, 
	//name = "Rain Dance", 
	Function = 0x100, FunctionAsString = "100", 
	BaseDamage = 0,
	Type = Types.WATER,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 5,
	Effects = 0,
	Target = Target.BothSides,
	Priority = 0,
	Flags = new Flags(  ) { } 
	//,description = "The user summons a heavy rain that falls for five turns, powering up Water-type moves."
},
new MoveDataDex() {
	num = 557,
	ID = Moves.SOAK, 
	//name = "Soak", 
	Function = 0x061, FunctionAsString = "061", 
	BaseDamage = 0,
	Type = Types.WATER,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 20,
	Effects = 0,
	Target = Target.SingleNonUser,
	Priority = 0,
	Flags = new Flags( protect: true,reflectable: true,mirror: true ) { }
	//,description = "The user shoots a torrent of water at the target and changes the target's type to Water."
},
new MoveDataDex() {
	num = 558,
	ID = Moves.WATER_SPORT, 
	//name = "Water Sport", 
	Function = 0x09E, FunctionAsString = "09E", 
	BaseDamage = 0,
	Type = Types.WATER,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 15,
	Effects = 0,
	Target = Target.BothSides,
	Priority = 0,
	Flags = new Flags(  ) { } 
	//,description = "The user soaks itself with water. The move weakens Fire-type moves while the user is in the battle."
},
new MoveDataDex() {
	num = 559,
	ID = Moves.WITHDRAW, 
	//name = "Withdraw", 
	Function = 0x01D, FunctionAsString = "01D", 
	BaseDamage = 0,
	Type = Types.WATER,
	Category = Category.STATUS,
	Accuracy = 0,
	PP = 40,
	Effects = 0,
	Target = Target.User,
	Priority = 0,
	Flags = new Flags( snatch: true ) { }
	//,description = "The user withdraws its body into its hard shell, raising its Defense stat."
}
	#endregion
};
		}
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
		/// The target Can use <see cref="Move.Protect"/> or <see cref="Move.Detect"/> to protect itself from the move
		/// </summary>
		Protectable,
		/// <summary>
		/// The target Can use <see cref="Move.Magic_Coat"/> to redirect the effect of the move. 
		/// Use this flag if the move deals no damage but causes a negative effect on the target.
		/// (Flags <see cref="MagicCoat"/> and <see cref="Snatch"/> are mutually exclusive.)
		/// </summary>
		MagicCoat,
		/// <summary>
		/// The target Can use <see cref="Move.Snatch"/> to steal the effect of the move. 
		/// Use this flag for most moves THAT target The user.
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
		/// Use this flag for all damaging moves THAT don't already have a flinching effect.
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