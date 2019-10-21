using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;

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
		/// User touches the target.  This triggers some abilities (e.g., []{ability:static}) and items (e.g., []{item:sticky-barb}).
		/// </summary>
		CONTACT			= 1,
		/// <summary>
		/// Has a charging turn<para></para>
		/// This move has a charging turn that can be skipped with a []{item:power-herb}.
		/// </summary>
		CHARGE			= 2,
		/// <summary>
		/// Must recharge<para></para>
		/// The turn after this move is used, the Pokémon's action is skipped so it can recharge.
		/// </summary>
		RECHARGE		= 3,
		/// <summary>
		/// Blocked by Detect and Protect<para></para>
		/// This move will not work if the target has used []{move:detect} or []{move:protect} this turn.
		/// </summary>
		PROTECT			= 4,
		/// <summary>
		/// Reflectable<para></para>
		/// This move may be reflected back at the user with []{move:magic-coat} or []{ability:magic-bounce}.
		/// </summary>
		REFLECTABLE		= 5,
		/// <summary>
		/// Snatchable<para></para>
		/// This move will be stolen if another Pokémon has used []{move:snatch} this turn.
		/// </summary>
		SNATCH			= 6,
		/// <summary>
		/// Copied by Mirror Move<para></para>
		/// A Pokémon targeted by this move can use []{move:mirror-move} to copy it.
		/// </summary>
		MIRROR			= 7,
		/// <summary>
		/// Punch-based<para></para>
		/// This move has 1.2× its usual power when used by a Pokémon with []{ability:iron-fist}.
		/// </summary>
		PUNCH			= 8,
		/// <summary>
		/// Sound-based<para></para>
		/// Pokémon with []{ability:soundproof} are immune to this move.
		/// </summary>
		SOUND			= 9,
		/// <summary>
		/// Unusable during Gravity<para></para>
		/// This move cannot be used in high []{move:gravity}.
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
		/// This move is blocked by []{move:heal-block}.
		/// </summary>
		HEAL			= 13,
		/// <summary>
		/// Ignores Substitute<para></para>
		/// This move ignores the target's []{move:substitute}.
		/// </summary>
		AUTHENTIC		= 14,
		/// <summary>
		/// Powder-based<para></para>
		/// Pokémon with []{ability:overcoat} and []{type:grass}-type Pokémon are immune to this move.
		/// </summary>
		POWDER			= 15,
		/// <summary>
		/// Jaw-based<para></para>
		/// This move has 1.5× its usual power when used by a Pokémon with []{ability:strong-jaw}.
		/// </summary>
		BITE			= 16,
		/// <summary>
		/// Pulse-based<para></para>
		/// This move has 1.5× its usual power when used by a Pokémon with []{ability:mega-launcher}.
		/// </summary>
		PULSE			= 17,
		/// <summary>
		/// Ballistics-based<para></para>
		/// This move is blocked by []{ability:bulletproof}.
		/// </summary>
		BALLISTICS		= 18,
		/// <summary>
		/// Mental effects<para></para>
		/// This move is blocked by []{ability:aroma-veil} and cured by []{item:mental-herb}.
		/// </summary>
		MENTAL			= 19,
		/// <summary>
		/// Unusable during Sky Battles<para></para>
		/// This move is unusable during Sky Battles.
		/// </summary>
		NON_SKY_BATTLE	= 20,
		/// <summary>
		/// Dance<para></para>
		/// This move triggers []{ability:dancer}.
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
}