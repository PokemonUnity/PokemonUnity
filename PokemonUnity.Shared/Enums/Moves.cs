using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Inventory;

/// <summary>
/// Namespace to nest all Pokemon Move Enums
/// </summary>
namespace PokemonUnity.Attack
{
	/// <summary>
	/// Is one of the following:
	/// Physical, Special, or Status
	/// </summary>
	public enum Category
	{
		PHYSICAL= 2,
		SPECIAL	= 3,
		STATUS	= 1
	};
	public enum MetaCategory
	{
		/// <summary>
		/// Inflicts damage
		/// </summary>
		DAMAGE				= 0,
		/// <summary>
		/// No damage; inflicts status ailment
		/// </summary>
		AILMENT				= 1,
		/// <summary>
		/// No damage; lowers target's stats or raises user's stats
		/// </summary>
		NET_GOOD_STATS		= 2,
		/// <summary>
		/// No damage; heals the user
		/// </summary>
		HEAL				= 3,
		/// <summary>
		/// Inflicts damage; inflicts status ailment
		/// </summary>
		DAMAGE_AILMENT		= 4,
		/// <summary>
		/// No damage; inflicts status ailment; raises target's stats
		/// </summary>
		SWAGGER				= 5,
		/// <summary>
		/// Inflicts damage; lowers target's stats
		/// </summary>
		DAMAGE_LOWER		= 6,
		/// <summary>
		/// Inflicts damage; raises user's stats
		/// </summary>
		DAMAGE_RAISE		= 7,
		/// <summary>
		/// Inflicts damage; absorbs damage done to heal the user
		/// </summary>
		DAMAGE_HEAL			= 8,
		/// <summary>
		/// One-hit KO
		/// </summary>
		OHKO				= 9,
		/// <summary>
		/// Effect on the whole field
		/// </summary>
		WHOLE_FIELD_EFFECT	= 10,
		/// <summary>
		/// Effect on one side of the field
		/// </summary>
		FIELD_EFFECT		= 11,
		/// <summary>
		/// Forces target to switch out
		/// </summary>
		FORCE_SWITCH		= 12,
		/// <summary>
		/// Unique effect
		/// </summary>
		UNIQUE				= 13
	}
	public enum MoveFlags
	{
		/// <summary>
		/// Makes contact<para></para>
		/// User touches the target.  This triggers some abilities (e.g., <seealso cref="Abilities.STATIC"/>) and items (e.g., <seealso cref="Items.STICKY_BARB"/>).
		/// </summary>
		CONTACT			= 1,
		/// <summary>
		/// Has a charging turn<para></para>
		/// This move has a charging turn that can be skipped with a <seealso cref="Items.POWER_HERB"/>.
		/// </summary>
		CHARGE			= 2,
		/// <summary>
		/// Must recharge<para></para>
		/// The turn after this move is used, the Pokémon's action is skipped so it can recharge.
		/// </summary>
		RECHARGE		= 3,
		/// <summary>
		/// Blocked by Detect and Protect<para></para>
		/// This move will not work if the target has used <seealso cref="Moves.DETECT"/> or <seealso cref="Moves.PROTECT"/> this turn.
		/// </summary>
		PROTECT			= 4,
		/// <summary>
		/// Reflectable<para></para>
		/// This move may be reflected back at the user with <seealso cref="Moves.MAGIC_COAT"/> or <seealso cref="Abilities.MAGIC_BOUNCE"/>.
		/// </summary>
		REFLECTABLE		= 5,
		/// <summary>
		/// Snatchable<para></para>
		/// This move will be stolen if another Pokémon has used <seealso cref="Moves.SNATCH"/> this turn.
		/// </summary>
		SNATCH			= 6,
		/// <summary>
		/// Copied by Mirror Move<para></para>
		/// A Pokémon targeted by this move can use <seealso cref="Moves.MIRROR_MOVE"/> to copy it.
		/// </summary>
		MIRROR			= 7,
		/// <summary>
		/// Punch-based<para></para>
		/// This move has 1.2× its usual power when used by a Pokémon with <seealso cref="Abilities.IRON_FIST"/>.
		/// </summary>
		PUNCH			= 8,
		/// <summary>
		/// Sound-based<para></para>
		/// Pokémon with <seealso cref="Abilities.SOUNDPROOF"/> are immune to this move.
		/// </summary>
		SOUND			= 9,
		/// <summary>
		/// Unusable during Gravity<para></para>
		/// This move cannot be used in high <seealso cref="Moves.GRAVITY"/>.
		/// </summary>
		GRAVITY			= 10,
		/// <summary>
		/// Defrosts when used<para></para>
		/// This move can be used while frozen to force the Pokémon to defrost.
		/// </summary>
		DEFROST			= 11,
		/// <summary>
		/// Targets opposite side in triples<para></para>
		/// In triple battles, this move can be used on either side to target the farthest away opposing Pokémon.
		/// </summary>
		DISTANCE		= 12,
		/// <summary>
		/// Heals<para></para>
		/// This move is blocked by <seealso cref="Moves.HEAL_BLOCK"/>.
		/// </summary>
		HEAL			= 13,
		/// <summary>
		/// Ignores Substitute<para></para>
		/// This move ignores the target's <seealso cref="Moves.SUBSTITUTE"/>.
		/// </summary>
		AUTHENTIC		= 14,
		/// <summary>
		/// Powder-based<para></para>
		/// Pokémon with <seealso cref="Abilities.OVERCOAT"/> and <seealso cref="Types.GRASS"/>-type Pokémon are immune to this move.
		/// </summary>
		POWDER			= 15,
		/// <summary>
		/// Jaw-based<para></para>
		/// This move has 1.5× its usual power when used by a Pokémon with <seealso cref="Abilities.STRONG_JAW"/>.
		/// </summary>
		BITE			= 16,
		/// <summary>
		/// Pulse-based<para></para>
		/// This move has 1.5× its usual power when used by a Pokémon with <seealso cref="Abilities.MEGA_LAUNCHER"/>.
		/// </summary>
		PULSE			= 17,
		/// <summary>
		/// Ballistics-based<para></para>
		/// This move is blocked by <seealso cref="Abilities.BULLETPROOF"/>.
		/// </summary>
		BALLISTICS		= 18,
		/// <summary>
		/// Mental effects<para></para>
		/// This move is blocked by <seealso cref="Abilities.AROMA_VEIL"/> and cured by <seealso cref="Items.MENTAL_HERB"/>.
		/// </summary>
		MENTAL			= 19,
		/// <summary>
		/// Unusable during Sky Battles<para></para>
		/// This move is unusable during Sky Battles.
		/// </summary>
		NON_SKY_BATTLE	= 20,
		/// <summary>
		/// Dance<para></para>
		/// This move triggers <seealso cref="Abilities.DANCER"/>.
		/// </summary>
		DANCE			= 21
	}
	public enum MoveMetaAilments
	{
		UNKNOWN				= -1,
		NONE				= 0,
		PARALYSIS			= 1,
		SLEEP				= 2,
		FREEZE				= 3,
		BURN				= 4,
		POISON				= 5,
		CONFUSION			= 6,
		INFATUATION			= 7,
		TRAP				= 8,
		NIGHTMARE			= 9,
		TORMENT				= 12,
		DISABLE				= 13,
		YAWN				= 14,
		HEAL_BLOCK			= 15,
		NO_TYPE_IMMUNITY	= 17,
		LEECH_SEED			= 18,
		EMBARGO				= 19,
		PERISH_SONG			= 20,
		INGRAIN				= 21,
		SILENCE				= 24,
	}

	#region Pokemon Essentials
	/// <summary>
	/// Pokemon Essentials Targeting Enum
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
		/// Makes the target drowsy; it will fall asleep at the end of the next turn. (Yawn)
		/// </summary>
		x004 = 0x004,

		/// <summary>
		/// Poisons the target.
		/// </summary>
		x005 = 0x005,

		/// <summary>
		/// Badly poisons the target. (Poison Fang, Toxic)
		/// (Handled in Pokemon's pbSuccessCheck): Hits semi-invulnerable targets if user
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
		/// (Handled in Pokemon's pbSuccessCheck): Hits some semi-invulnerable targets.
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
		/// </summary>
		/// TODO: Play the actual chatter cry as part of the move animation
		///       @battle.scene.pbChatter(attacker,opponent) // Just plays cry
		x014 = 0x014,

		/// <summary>
		/// Confuses the target. Accuracy perfect in rain, 50% in sunshine. (Hurricane)
		/// (Handled in Pokemon's pbSuccessCheck): Hits some semi-invulnerable targets.
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
		/// (Handled in Pokemon's pbSuccessCheck): Hits some semi-invulnerable targets.
		/// </summary>
		x075 = 0x075,

		/// <summary>
		/// Power is doubled if the target is using Dig. Power is halved if Grassy Terrain
		/// is in effect. (Earthquake)
		/// (Handled in Pokemon's pbSuccessCheck): Hits some semi-invulnerable targets.
		/// </summary>
		x076 = 0x076,

		/// <summary>
		/// Power is doubled if the target is using Bounce, Fly or Sky Drop. (Gust)
		/// (Handled in Pokemon's pbSuccessCheck): Hits some semi-invulnerable targets.
		/// </summary>
		x077 = 0x077,

		/// <summary>
		/// Power is doubled if the target is using Bounce, Fly or Sky Drop. (Twister)
		/// May make the target flinch.
		/// (Handled in Pokemon's pbSuccessCheck): Hits some semi-invulnerable targets.
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
		/// (Handled in Pokemon's pbProcessMoveAgainstTarget): Ups rager's Attack by 1
		/// stage each time it loses HP due to a move.
		/// </summary>
		x093 = 0x093,

		/// <summary>
		/// Randomly damages or heals the target. (Present)
		/// </summary>
		x094 = 0x094,

		/// <summary>
		/// Power is chosen at random. Power is doubled if the target is using Dig. (Magnitude)
		/// (Handled in Pokemon's pbSuccessCheck): Hits some semi-invulnerable targets.
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
		/// Handled in superclass public object pbAccuracyCheck and public object pbCalcDamage, do not edit!
		/// </summary>
		x0A9 = 0x0A9,

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
		/// (Handled in Pokemon's pbSuccessCheck): Is semi-invulnerable during use.
		/// </summary>
		x0C9 = 0x0C9,

		/// <summary>
		/// Two turn attack. Skips first turn, attacks second turn. (Dig)
		/// (Handled in Pokemon's pbSuccessCheck): Is semi-invulnerable during use.
		/// </summary>
		x0CA = 0x0CA,

		/// <summary>
		/// Two turn attack. Skips first turn, attacks second turn. (Dive)
		/// (Handled in Pokemon's pbSuccessCheck): Is semi-invulnerable during use.
		/// </summary>
		x0CB = 0x0CB,

		/// <summary>
		/// Two turn attack. Skips first turn, attacks second turn. (Bounce)
		/// May paralyze the target.
		/// (Handled in Pokemon's pbSuccessCheck): Is semi-invulnerable during use.
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
		/// (Handled in Pokemon's pbSuccessCheck):  Is semi-invulnerable during use.
		/// Target is also semi-invulnerable during use, and can't take any action.
		/// Doesn't damage airborne Pokémon (but still makes them unable to move during).
		/// </summary>
		x0CE = 0x0CE,

		/// <summary>
		/// Trapping move. Traps for 5 or 6 rounds. Trapped Pokémon lose 1/16 of max HP
		/// at end of each round.
		/// </summary>
		x0CF = 0x0CF,

		/// <summary>
		/// Trapping move. Traps for 5 or 6 rounds. Trapped Pokémon lose 1/16 of max HP
		/// at end of each round. (Whirlpool)
		/// Power is doubled if target is using Dive.
		/// (Handled in Pokemon's pbSuccessCheck): Hits some semi-invulnerable targets.
		/// </summary>
		x0D0 = 0x0D0,

		/// <summary>
		/// User must use this move for 2 more rounds. No battlers can sleep. (Uproar)
		/// </summary>
		x0D1 = 0x0D1,

		/// <summary>
		/// User must use this move for 1 or 2 more rounds. At end, user becomes confused.
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
		/// Pokemon in user's position is healed by 1/2 of its max HP, at the end of the
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
		/// Rings the user. Ringed Pokémon gain 1/16 of max HP at the end of each round.
		/// (Aqua Ring)
		/// </summary>
		x0DA = 0x0DA,

		/// <summary>
		/// Ingrains the user. Ingrained Pokémon gain 1/16 of max HP at the end of each
		/// round, and cannot flee or switch out. (Ingrain)
		/// </summary>
		x0DB = 0x0DB,

		/// <summary>
		/// Seeds the target. Seeded Pokémon lose 1/8 of max HP at the end of each round,
		/// and the Pokémon in the user's position gains the same amount. (Leech Seed)
		/// </summary>
		x0DC = 0x0DC,

		/// <summary>
		/// User gains half the HP it inflicts as damage.
		/// </summary>
		x0DD = 0x0DD,

		/// <summary>
		/// User gains half the HP it inflicts as damage. (Dream Eater)
		/// (Handled in Pokemon's pbSuccessCheck): Fails if target is not asleep.
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
		/// Handled in superclass public object pbReduceHPDamage, do not edit!
		/// </summary>
		x0E9 = 0x0E9,

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
		/// </summary>
		/// TODO: Pursuit should interrupt this move.
		x0EE = 0x0EE,

		/// <summary>
		/// Target can no longer switch out or flee, as long as the user remains active.
		/// (Block, Mean Look, Spider Web, Thousand Waves)
		/// </summary>
		x0EF = 0x0EF,

		/// <summary>
		/// Target drops its item. It regains the item at the end of the battle. (Knock Off)
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
		/// Cursed Pokémon lose 1/4 of their max HP at the end of each round.
		/// (Curse)
		/// </summary>
		x10D = 0x10D,

		/// <summary>
		/// Target's last move used loses 4 PP. (Spite)
		/// </summary>
		x10E = 0x10E,

		/// <summary>
		/// Target will lose 1/4 of max HP at end of each round, while asleep. (Nightmare)
		/// </summary>
		x10F = 0x10F,

		/// <summary>
		/// Removes trapping moves, entry hazards and Leech Seed on user/user's side.
		/// (Rapid Spin)
		/// </summary>
		x110 = 0x110,

		/// <summary>
		/// Attacks 2 rounds in the future. (Doom Desire, Future Sight)
		/// Attack is launched
		/// </summary>
		x111 = 0x111,


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
		/// Handled in Pokemon's pbSuccessCheck, do not edit!
		x11B = 0x11B,

		/// <summary>
		/// Grounds the target while it remains active. (Smack Down, Thousand Arrows)
		/// (Handled in Pokemon's pbSuccessCheck): Hits some semi-invulnerable targets.
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
		/// No Pokémon can switch out or flee until the end of the next round, as long as
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
		/// Pokémon at the end of each round. Affects non-airborne Pokémon only.
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
		x158 = 0x158
	}
	#endregion
}