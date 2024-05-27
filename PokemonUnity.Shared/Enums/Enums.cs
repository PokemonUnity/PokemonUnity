using PokemonUnity.Combat;
using PokemonUnity.Inventory;

namespace PokemonUnity
{
	#region Player
	/// <summary>
	/// Whatever gym badges you want to incorporate throughout your game.
	/// This is where the ID of the badges are stored, and the game can
	/// validate certain actions based on said ID or # of badges...
	/// </summary>
	public enum GymBadges
	{
		Boulder, //Rock
		Cascade, // Water
		Thunder,
		Rainbow,
		Soul,
		Marsh,
		Volcano,
		Earth,

		Zephyr,
		Hive,
		Plain,
		Fog,
		Storm,
		Mineral,
		Glacier,
		Rising,

		Stone,
		Knuckle,
		Dynamo,
		Heat,
		Balance,
		Feather,
		Mind,
		Rain,

		Coal,
		Forest,
		Cobble,
		Fen,
		Relic,
		Mine,
		Icicle,
		Beacon,

		Trio,
		Basic,
		Insect,
		Bolt,
		Quake,
		Jet,
		Freeze,
		Legend,

		Toxic,
		Wave,

		Bug,
		Cliff,
		Rumble,
		Plant,
		Voltage,
		Fairy,
		Psychic,
		Iceberg,

		Grass,
		Water,
		Fire,
		Fighting,
		Ghost,
		Fairy_2,
		Rock,
		Ice,
		Dark,
		Dragon,

		Coral_Eye,
		Sea_Ruby,
		Spike_Shell,
		Jade_Star,

		Tranquility,
		Freedom,
		Patience,
		Harmony,
		Pride
	}
	#endregion
	#region Type
	public enum Types
	{
		NONE = 0,
		NORMAL = 1,
		FIGHTING = 2,
		FLYING = 3,
		POISON = 4,
		GROUND = 5,
		ROCK = 6,
		BUG = 7,
		GHOST = 8,
		STEEL = 9,
		FIRE = 10,
		WATER = 11,
		GRASS = 12,
		ELECTRIC = 13,
		PSYCHIC = 14,
		ICE = 15,
		DRAGON = 16,
		DARK = 17,
		FAIRY = 18,
		UNKNOWN = 10001,
		SHADOW = 10002
	};
	#endregion
	#region Ability
	/// <summary>
	/// Ability ids are connected to XML file.
	/// </summary>
	/// <remarks>Can now code with strings or int and
	/// access the same value.</remarks>
	public enum Abilities
	{
		NONE = 0,
		/// <summary>
		/// Has a 10% chance of making target Pokémon [flinch]{mechanic:flinch} with each hit.<para></para> This Pokémon's damaging moves have a 10% chance to make the target [flinch]{mechanic:flinch} with each hit if they do not already cause flinching as a secondary effect.
		///
		/// This ability does not stack with a held item.
		///
		/// Overworld: The wild encounter rate is halved while this Pokémon is first in the party.
		/// </summary>
		STENCH = 1,
		/// <summary>
		/// Summons [rain]{mechanic:rain} that lasts indefinitely upon entering battle.<para></para> The [weather]{mechanic:weather} changes to [rain]{mechanic:rain} when this Pokémon enters battle and does not end unless replaced by another weather condition.
		///
		/// If multiple Pokémon with this ability, <see cref="Abilities.DROUGHT"/>, <see cref="Abilities.SAND_STREAM"/>, or <see cref="Abilities.SNOW_WARNING"/> are sent out at the same time, the abilities will activate in order of <see cref="Stats.SPEED"/>, respecting <see cref="Moves.TRICK_ROOM"/>. Each ability's weather will cancel the previous weather, and only the weather summoned by the slowest of the Pokémon will stay.
		/// </summary>
		DRIZZLE = 2,
		/// <summary>
		/// Raises <see cref="Stats.SPEED"/> one [stage]{mechanic:stat_modifier} after each turn.<para></para> This Pokémon's <see cref="Stats.SPEED"/> rises one [stage]{mechanic:stat_modifier} after each turn.
		/// </summary>
		SPEED_BOOST = 3,
		/// <summary>
		/// Protects against [critical hits]{mechanic:critical_hit}.<para></para> Moves cannot score [critical hits]{mechanic:critical_hit} against this Pokémon.
		///
		/// This ability functions identically to <see cref="Abilities.SHELL_ARMOR"/>.
		/// </summary>
		BATTLE_ARMOR = 4,
		/// <summary>
		/// Prevents being KOed from full <see cref="Stats.HP"/>, leaving 1 HP instead. Protects against the one-hit KO moves regardless of HP.<para></para> When this Pokémon is at full <see cref="Stats.HP"/>, any hit that would knock it out will instead leave it with 1 HP. Regardless of its current HP, it is also immune to the one-hit KO moves: <see cref="Moves.FISSURE"/>, <see cref="Moves.GUILLOTINE"/>, <see cref="Moves.HORN_DRILL"/>, and <see cref="Moves.SHEER_COLD"/>.
		///
		/// If this Pokémon is holding a <see cref="Items.FOCUS_SASH"/>, this ability takes precedence and the item will not be consumed.
		/// </summary>
		STURDY = 5,
		/// <summary>
		/// Prevents <see cref="Moves.SELF_DESTRUCT"/>, <see cref="Moves.EXPLOSION"/>, and <see cref="Abilities.AFTERMATH"/> from working while the Pokémon is in battle.<para></para> While this Pokémon is in battle, <see cref="Moves.SELF_DESTRUCT"/> and <see cref="Moves.EXPLOSION"/> will fail and <see cref="Abilities.AFTERMATH"/> will not take effect.
		/// </summary>
		DAMP = 6,
		/// <summary>
		/// Prevents <see cref="Status.PARALYSIS"/>.<para></para> This Pokémon cannot be <see cref="Status.PARALYSIS"/>.
		///
		/// If a Pokémon is paralyzed and acquires this ability, its paralysis is healed; this includes when regaining a lost ability upon leaving battle.
		/// </summary>
		LIMBER = 7,
		/// <summary>
		/// Increases <see cref="Stats.EVASION"/> to 1.25× during a <see cref="Weather.SANDSTORM"/>. Protects against sandstorm damage.<para></para> During a <see cref="Weather.SANDSTORM"/>, this Pokémon has 1.25× its <see cref="Stats.EVASION"/>, and it does not take sandstorm damage regardless of type.
		///
		/// The evasion bonus does not count as a [stat modifier]{mechanic:stat_modifier}.
		///
		/// Overworld: If the lead Pokémon has this ability, the wild encounter rate is halved in a sandstorm.
		/// </summary>
		SAND_VEIL = 8,
		/// <summary>
		/// Has a 30% chance of <see cref="Status.PARALYSIS"/> attacking Pokémon on contact.<para></para> Whenever a move makes contact with this Pokémon, the move's user has a 30% chance of being <see cref="Status.PARALYSIS"/>.
		///
		/// Pokémon that are immune to <see cref="Types.ELECTRIC"/>-type moves can still be paralyzed by this ability.
		///
		/// Overworld: If the lead Pokémon has this ability, there is a 50% chance that encounters will be with an <see cref="Types.ELECTRIC"/> Pokémon, if applicable.
		/// </summary>
		STATIC = 9,
		/// <summary>
		/// Absorbs <see cref="Types.ELECTRIC"/> moves, healing for 1/4 max <see cref="Stats.HP"/>.<para></para> Whenever an <see cref="Types.ELECTRIC"/>-type move hits this Pokémon, it heals for 1/4 of its maximum <see cref="Stats.HP"/>, negating any other effect on it.
		///
		/// This ability will not take effect if this Pokémon is <see cref="Types.GROUND"/>-type and thus immune to Electric moves. Electric moves will ignore this Pokémon's <see cref="Moves.SUBSTITUTE"/>.
		///
		/// This effect includes non-damaging moves, i.e. <see cref="Moves.THUNDER_WAVE"/>.
		/// </summary>
		VOLT_ABSORB = 10,
		/// <summary>
		/// Absorbs <see cref="Types.WATER"/> moves, healing for 1/4 max <see cref="Stats.HP"/>.<para></para> Whenever a <see cref="Types.WATER"/>-type move hits this Pokémon, it heals for 1/4 of its maximum <see cref="Stats.HP"/>, negating any other effect on it.
		///
		/// Water moves will ignore this Pokémon's <see cref="Moves.SUBSTITUTE"/>.
		/// </summary>
		WATER_ABSORB = 11,
		/// <summary>
		/// Prevents [infatuation]{mechanic:infatuation} and protects against <see cref="Moves.CAPTIVATE"/>.<para></para> This Pokémon cannot be [infatuated]{mechanic:infatuation} and is immune to <see cref="Moves.CAPTIVATE"/>.
		///
		/// If a Pokémon is infatuated and acquires this ability, its infatuation is cleared.
		/// </summary>
		OBLIVIOUS = 12,
		/// <summary>
		/// Negates all effects of [weather]{mechanic:weather}, but does not prevent the weather itself.<para></para> While this Pokémon is in battle, [weather]{mechanic:weather} can still be in play, but will not have any of its effects.
		///
		/// This ability functions identically to <see cref="Abilities.AIR_LOCK"/>.
		/// </summary>
		CLOUD_NINE = 13,
		/// <summary>
		/// Increases moves' <see cref="Stats.ACCURACY"/> to 1.3×.<para></para> This Pokémon's moves have 1.3× their <see cref="Stats.ACCURACY"/>.
		///
		/// This ability has no effect on the one-hit KO moves (<see cref="Moves.FISSURE"/>, <see cref="Moves.GUILLOTINE"/>, <see cref="Moves.HORN_DRILL"/>, and <see cref="Moves.SHEER_COLD"/>).
		///
		/// Overworld: If the first Pokémon in the party has this ability, the chance of a wild Pokémon holding a particular item is raised from 50%, 5%, or 1% to 60%, 20%, or 5%, respectively.
		/// </summary>
		COMPOUND_EYES = 14,
		/// <summary>
		/// Prevents <see cref="Status.SLEEP"/>.<para></para> This Pokémon cannot be <see cref="Status.SLEEP"/>.
		///
		/// This causes <see cref="Moves.REST"/> to fail altogether. If a Pokémon is asleep and acquires this ability, it will immediately wake up; this includes when regaining a lost ability upon leaving battle.
		///
		/// This ability functions identically to <see cref="Abilities.VITAL_SPIRIT"/> in battle.
		/// </summary>
		INSOMNIA = 15,
		/// <summary>
		/// Changes type to match when hit by a damaging move.<para></para> Whenever this Pokémon takes damage from a move, the Pokémon's type changes to match the move.
		///
		/// If the Pokémon has two types, both are overridden. The Pokémon must directly take damage; for example, moves blocked by a <see cref="Moves.SUBSTITUTE"/> will not trigger this ability, nor will moves that deal damage indirectly, such as <see cref="Moves.SPIKES"/>.
		///
		/// This ability takes effect on only the last hit of a multiple-hit attack.
		///
		/// In Pokémon Colosseum and XD: Gale of Darkness, this ability does not take effect on Shadow-type moves.
		/// </summary>
		COLOR_CHANGE = 16,
		/// <summary>
		/// Prevents <see cref="Status.POISON"/>.<para></para> This Pokémon cannot be <see cref="Status.POISON"/>. This includes bad poison.
		///
		/// If a Pokémon is poisoned and acquires this ability, its poison is healed; this includes when regaining a lost ability upon leaving battle.
		/// </summary>
		IMMUNITY = 17,
		/// <summary>
		/// Protects against <see cref="Types.FIRE"/> moves. Once one has been blocked, the Pokémon's own Fire moves inflict 1.5× damage until it leaves battle.<para></para> This Pokémon is immune to <see cref="Types.FIRE"/>-type moves. Once this Pokémon has been hit by a Fire move, its own Fire moves will inflict 1.5× as much damage until it leaves battle.
		///
		/// This ability has no effect while the Pokémon is <see cref="Status.FROZEN"/>. The Fire damage bonus is retained even if the Pokémon is frozen and thawed or the ability is lost or disabled. Fire moves will ignore this Pokémon's <see cref="Moves.SUBSTITUTE"/>. This ability takes effect even on non-damaging moves, i.e. <see cref="Moves.WILL_O_WISP"/>.
		/// </summary>
		FLASH_FIRE = 18,
		/// <summary>
		/// Protects against incoming moves' extra effects.<para></para> This Pokémon is immune to the extra effects of moves used against it.
		///
		/// An extra effect is a move's chance, listed as an "effect chance", to inflict a <see cref="IBattler.Status"/>, cause a [stat change]{mechanic:stat_modifier}, or make the target [flinch]{mechanic:flinching} in addition to the move's main effect. For example, <see cref="Moves.THUNDER_SHOCK"/>'s <see cref="Status.PARALYSIS"/> is an extra effect, but <see cref="Moves.THUNDER_WAVE"/>'s is not, nor are <see cref="Moves.KNOCK_OFF"/>'s item removal and <see cref="Moves.AIR_CUTTER"/>'s increased [critical hit]{mechanic:critial_hit} rate.
		/// </summary>
		SHIELD_DUST = 19,
		/// <summary>
		/// Prevents <see cref="IEffectsBattler.Confusion"/>.<para></para> This Pokémon cannot be <see cref="IEffectsBattler.Confusion"/>.
		///
		/// If a Pokémon is confused and acquires this ability, its confusion will immediately be healed.
		/// </summary>
		OWN_TEMPO = 20,
		/// <summary>
		/// Prevents being forced out of battle by other Pokémon's moves.<para></para> This Pokémon cannot be forced out of battle by moves such as <see cref="Moves.WHIRLWIND"/>.
		///
		/// <see cref="Moves.DRAGON_TAIL"/> and <see cref="Moves.CIRCLE_THROW"/> still inflict damage against this Pokémon.
		///
		/// Overworld: If the lead Pokémon has this ability, the success rate while fishing is increased.
		/// </summary>
		SUCTION_CUPS = 21,
		/// <summary>
		/// Lowers opponents' <see cref="Stats.ATTACK"/> one [stage]{mechanic:stat_modifier} upon entering battle.<para></para> When this Pokémon enters battle, the opponent's <see cref="Stats.ATTACK"/> is lowered by one [stage]{mechanic:stat_modifier}. In a double battle, both opponents are affected.
		///
		/// This ability also takes effect when acquired during a battle, but will not take effect again if lost and reobtained without leaving battle.
		///
		/// This ability has no effect on an opponent that has a <see cref="Moves.SUBSTITUTE"/>.
		///
		/// Overworld: If the first Pokémon in the party has this ability, any random encounter with a Pokémon five or more levels lower than it has a 50% chance of being skipped.
		/// </summary>
		INTIMIDATE = 22,
		/// <summary>
		/// Prevents opponents from fleeing or switching out.<para></para> While this Pokémon is in battle, opposing Pokémon cannot flee or switch out.
		///
		/// Other Pokémon with this ability are unaffected. Pokémon with <see cref="Abilities.RUN_AWAY"/> can still flee. Pokémon can still switch out with the use of a move or item.
		/// </summary>
		SHADOW_TAG = 23,
		/// <summary>
		/// Damages attacking Pokémon for 1/8 their max <see cref="Stats.HP"/> on contact.<para></para> Whenever a move makes contact with this Pokémon, the move's user takes 1/8 of its maximum <see cref="Stats.HP"/> in damage.
		///
		/// This ability functions identically to <see cref="Abilities.IRON_BARBS"/>.
		/// </summary>
		ROUGH_SKIN = 24,
		/// <summary>
		/// Protects against damaging moves that are not <see cref="TypeEffective.SuperEffective"/>.<para></para> This Pokémon is immune to damaging moves that are not <see cref="TypeEffective.SuperEffective"/> against it.
		///
		/// Moves that inflict fixed damage, such as <see cref="Moves.NIGHT_SHADE"/> or <see cref="Moves.SEISMIC_TOSS"/>, are considered super effective if their types are. Damage not directly dealt by moves, such as damage from [weather]{mechanic:weather}, a <see cref="IBattler.Status"/>, or <see cref="Moves.SPIKES"/>, is not prevented.
		///
		/// This ability cannot be copied with <see cref="Moves.ROLE_PLAY"/> or traded away with <see cref="Moves.SKILL_SWAP"/>, but it can be copied with <see cref="Abilities.TRACE"/>, disabled with <see cref="Moves.GASTRO_ACID"/>, or changed with <see cref="Moves.WORRY_SEED"/>. This Pokémon can still use Role Play itself to lose this ability, but not Skill Swap.
		///
		/// If this Pokémon has a <see cref="Moves.SUBSTITUTE"/>, this ability will block moves as usual and any moves not blocked will react to the Substitute as usual.
		/// </summary>
		WONDER_GUARD = 25,
		/// <summary>
		/// Evades <see cref="Types.GROUND"/> moves.<para></para> This Pokémon is immune to <see cref="Types.GROUND"/>-type moves, <see cref="Moves.SPIKES"/>, <see cref="Moves.TOXIC_SPIKES"/>, and <see cref="Abilities.ARENA_TRAP"/>.
		///
		/// This ability is disabled during <see cref="Moves.GRAVITY"/> or <see cref="Moves.INGRAIN"/>, or while holding an <see cref="Items.IRON_BALL"/>. This ability is not disabled during <see cref="Moves.ROOST"/>.
		/// </summary>
		LEVITATE = 26,
		/// <summary>
		/// Has a 30% chance of inflicting either <see cref="Status.PARALYSIS"/>, <see cref="Status.POISON"/>, or <see cref="Status.SLEEP"/> on attacking Pokémon on contact.<para></para> Whenever a move makes contact with this Pokémon, the move's user has a 30% chance of being <see cref="Status.PARALYSIS"/>, <see cref="Status.POISON"/>, or put to <see cref="Status.SLEEP"/>, chosen at random.
		///
		/// Nothing is done to compensate if the move's user is immune to one of these ailments; there is simply a lower chance that the move's user will be affected.
		/// </summary>
		EFFECT_SPORE = 27,
		/// <summary>
		/// Copies <see cref="Status.BURN"/>, <see cref="Status.PARALYSIS"/>, and <see cref="Status.POISON"/> received onto the Pokémon that inflicted them.<para></para> Whenever this Pokémon is <see cref="Status.BURN"/>, <see cref="Status.PARALYSIS"/>, or <see cref="Status.POISON"/>, the Pokémon who gave this Pokémon that ailment is also given the ailment.
		///
		/// This ability passes back bad poison when this Pokémon is badly poisoned. This ability cannot pass on a status ailment that the Pokémon did not directly receive from another Pokémon, such as the poison from <see cref="Moves.TOXIC_SPIKES"/> or the burn from a <see cref="Items.FLAME_ORB"/>.
		///
		/// Overworld: If the lead Pokémon has this ability, wild Pokémon have a 50% chance of having the lead Pokémon's nature, and a 50% chance of being given a random nature as usual, including the lead Pokémon's nature. This does not work on Pokémon received outside of battle or roaming legendaries.
		/// </summary>
		SYNCHRONIZE = 28,
		/// <summary>
		/// Prevents stats from being [lowered]{mechanic:stat_modifier} by other Pokémon.<para></para> This Pokémon cannot have its stats [lowered]{mechanic:stat_modifier} by other Pokémon.
		///
		/// This ability does not prevent any stat losses other than [stat modifiers]{mechanic:stat_modifiers}, such as the <see cref="Stats.SPEED"/> cut from <see cref="Status.PARALYSIS"/>. This Pokémon can still be passed negative stat modifiers through <see cref="Moves.GUARD_SWAP"/>, <see cref="Moves.HEART_SWAP"/>, or <see cref="Moves.POWER_SWAP"/>.
		///
		/// This ability functions identically to <see cref="Abilities.WHITE_SMOKE"/> in battle.
		/// </summary>
		CLEAR_BODY = 29,
		/// <summary>
		/// Cures any [major status ailment]{mechanic:major_status_ailment} upon switching out.<para></para> This Pokémon is cured of any [major status ailment]{mechanic:major_status_ailment} when it is switched out for another Pokémon.
		///
		/// If this ability is acquired during battle, the Pokémon is cured upon leaving battle before losing the temporary ability.
		/// </summary>
		NATURAL_CURE = 30,
		/// <summary>
		/// Redirects single-target <see cref="Types.ELECTRIC"/> moves to this Pokémon where possible. Absorbs Electric moves, raising <see cref="Stats.SPATK"/> one [stage]{mechanic:stat_modifier}.<para></para> All other Pokémon's single-target <see cref="Types.ELECTRIC"/>-type moves are redirected to this Pokémon if it is an eligible target. Other Pokémon's Electric moves raise this Pokémon's <see cref="Stats.SPATK"/> one [stage]{mechanic:stat_modifier}, negating any other effect on it, and cannot miss it.
		///
		/// If the move's intended target also has this ability, the move is not redirected. When multiple Pokémon with this ability are possible targets for redirection, the move is redirected to the one with the highest <see cref="Stats.SPEED"/> stat, or, in the case of a tie, to a random tied Pokémon.  <see cref="Moves.FOLLOW_ME"/> takes precedence over this ability.
		///
		/// If the Pokémon is a <see cref="Types.GROUND"/>-type and thus immune to Electric moves, its immunity prevents the Special Attack boost.
		/// </summary>
		LIGHTNING_ROD = 31,
		/// <summary>
		/// Doubles the chance of moves' extra effects occurring.<para></para> This Pokémon's moves have twice their usual effect chance.
		///
		/// An effect chance is a move's chance to inflict a <see cref="IBattler.Status"/>, cause a [stat change]{mechanic:stat_modifier}, or make the target [flinch]{mechanic:flinching} in addition to the move's main effect. For example, <see cref="Moves.FLAMETHROWER"/>'s chance of <see cref="Status.BURN"/> the target is doubled, but <see cref="Moves.PROTECT"/>'s chance of success and <see cref="Moves.AIR_CUTTER"/>'s increased [critical hit]{mechanic:critical_hit} rate are unaffected.
		///
		/// <see cref="Moves.SECRET_POWER"/> is unaffected.
		/// </summary>
		SERENE_GRACE = 32,
		/// <summary>
		/// Doubles <see cref="Stats.SPEED"/> during [rain]{mechanic:rain}.<para></para> This Pokémon's <see cref="Stats.SPEED"/> is doubled during [rain]{mechanic:rain}.
		///
		/// This bonus does not count as a [stat modifier]{mechanic:stat_modifier}.
		/// </summary>
		SWIFT_SWIM = 33,
		/// <summary>
		/// Doubles <see cref="Stats.SPEED"/> during [strong sunlight]{mechanic:strong_sunlight}.<para></para> This Pokémon's <see cref="Stats.SPEED"/> is doubled during [strong sunlight]{mechanic:strong_sunlight}.
		///
		/// This bonus does not count as a [stat modifier]{mechanic:stat_modifier}.
		/// </summary>
		CHLOROPHYLL = 34,
		/// <summary>
		/// Doubles the wild encounter rate.<para></para> Overworld: If the lead Pokémon has this ability, the wild encounter rate is doubled.
		///
		/// This ability has no effect in battle.
		/// </summary>
		ILLUMINATE = 35,
		/// <summary>
		/// Copies an opponent's ability upon entering battle.<para></para> When this Pokémon enters battle, it copies a random opponent's ability.
		///
		/// This ability cannot copy <see cref="Abilities.FLOWER_GIFT"/>, <see cref="Abilities.FORECAST"/>, <see cref="Abilities.ILLUSION"/>, <see cref="Abilities.IMPOSTER"/>, <see cref="Abilities.MULTITYPE"/>, <see cref="Abilities.TRACE"/>, <see cref="Abilities.WONDER_GUARD"/>, or <see cref="Abilities.ZEN_MODE"/>.
		/// </summary>
		TRACE = 36,
		/// <summary>
		/// Doubles <see cref="Stats.ATTACK"/> in battle.<para></para> This Pokémon's <see cref="Stats.ATTACK"/> is doubled while in battle.
		///
		/// This bonus does not count as a [stat modifier]{mechanic:stat_modifier}.
		///
		/// This ability functions identically to <see cref="Abilities.PURE_POWER"/>.
		/// </summary>
		HUGE_POWER = 37,
		/// <summary>
		/// Has a 30% chance of <see cref="Status.POISON"/> attacking Pokémon on contact.<para></para> Whenever a move makes contact with this Pokémon, the move's user has a 30% chance of being <see cref="Status.POISON"/>.
		/// </summary>
		POISON_POINT = 38,
		/// <summary>
		/// Prevents [flinching]{mechanic:flinching}.<para></para> This Pokémon cannot [flinch]{mechanic:flinching}.
		/// </summary>
		INNER_FOCUS = 39,
		/// <summary>
		/// Prevents <see cref="Status.FROZEN"/>.<para></para> This Pokémon cannot be <see cref="Status.FROZEN"/>.
		///
		/// If a Pokémon is frozen and acquires this ability, it will immediately thaw out; this includes when regaining a lost ability upon leaving battle.
		///
		/// Overworld: If any Pokémon in the party has this ability, each egg in the party has its [hatch counter]{mechanic:hatch_counter} decreased by 2 (rather than 1) each [step cycle]{mechanic:step_cycle}, making eggs hatch roughly twice as quickly. This effect does not stack if multiple Pokémon have this ability or <see cref="Abilities.FLAME_BODY"/>.
		/// </summary>
		MAGMA_ARMOR = 40,
		/// <summary>
		/// Prevents <see cref="Status.BURN"/>.<para></para> This Pokémon cannot be <see cref="Status.BURN"/>.
		///
		/// If a Pokémon is burned and acquires this ability, its burn is healed; this includes when regaining a lost ability upon leaving battle.
		/// </summary>
		WATER_VEIL = 41,
		/// <summary>
		/// Prevents <see cref="Types.STEEL"/> opponents from fleeing or switching out.<para></para> While this Pokémon is in battle, opposing <see cref="Types.STEEL"/>-type Pokémon cannot flee or switch out.
		///
		/// Pokémon with <see cref="Abilities.RUN_AWAY"/> can still flee. Pokémon can still switch out with the use of a move or item.
		///
		/// Overworld: If the lead Pokémon has this ability, Steel-type Pokémon have a higher encounter rate.
		/// </summary>
		MAGNET_PULL = 42,
		/// <summary>
		/// Protects against sound-based moves.<para></para> This Pokémon is immune to moves flagged as being sound-based.
		///
		/// <see cref="Moves.HEAL_BELL"/> is unaffected.  <see cref="Moves.UPROAR"/> still prevents this Pokémon from <see cref="Status.SLEEP"/>. This Pokémon can still receive a Perish Song counter through <see cref="Moves.BATON_PASS"/>, and will retain a Perish Song counter if it acquires this ability after Perish Song is used.
		///
		/// <see cref="Moves.HOWL"/>, <see cref="Moves.ROAR_OF_TIME"/>, <see cref="Moves.SONIC_BOOM"/>, and <see cref="Moves.YAWN"/> are not flagged as sound-based.
		/// </summary>
		SOUNDPROOF = 43,
		/// <summary>
		/// Heals for 1/16 max <see cref="Stats.HP"/> after each turn during [rain]{mechanic:rain}.<para></para> This Pokémon heals for 1/16 of its maximum <see cref="Stats.HP"/> after each turn during [rain]{mechanic:rain}.
		/// </summary>
		RAIN_DISH = 44,
		/// <summary>
		/// Summons a <see cref="Weather.SANDSTORM"/> that lasts indefinitely upon entering battle.<para></para> The [weather]{mechanic:weather} changes to a <see cref="Weather.SANDSTORM"/> when this Pokémon enters battle and does not end unless cancelled by another weather condition.
		///
		/// If multiple Pokémon with this ability, <see cref="Abilities.DRIZZLE"/>, <see cref="Abilities.DROUGHT"/>, or <see cref="Abilities.SNOW_WARNING"/> are sent out at the same time, the abilities will activate in order of <see cref="Stats.SPEED"/>, respecting <see cref="Moves.TRICK_ROOM"/>. Each ability's weather will cancel the previous weather, and only the weather summoned by the slowest of the Pokémon will stay.
		///
		/// Overworld: If the lead Pokémon has this ability, the wild encounter rate is halved in a sandstorm.
		/// </summary>
		SAND_STREAM = 45,
		/// <summary>
		/// Increases the <see cref="IMove.PP"/> cost of moves targeting the Pokémon by one.<para></para> Moves targeting this Pokémon use one extra <see cref="IMove.PP"/>.
		///
		/// This ability stacks if multiple targets have it. This ability still affects moves that fail or miss. This ability does not affect ally moves that target either the entire field or just its side, nor this Pokémon's self-targetted moves; it does, however, affect single-targetted ally moves aimed at this Pokémon, ally moves that target all other Pokémon, and opponents' moves that target the entire field. If this ability raises a move's PP cost above its remaining PP, it will use all remaining PP.
		///
		/// When this Pokémon enters battle, all participating trainers are notified that it has this ability.
		///
		/// Overworld: If the lead Pokémon has this ability, higher-leveled Pokémon have their encounter rate increased.
		/// </summary>
		PRESSURE = 46,
		/// <summary>
		/// Halves damage from <see cref="Types.FIRE"/> and <see cref="Types.ICE"/> moves.<para></para> This Pokémon takes half as much damage from <see cref="Types.FIRE"/>- and <see cref="Types.ICE"/>-type moves.
		/// </summary>
		THICK_FAT = 47,
		/// <summary>
		/// Makes <see cref="Status.SLEEP"/> pass twice as quickly.<para></para> This Pokémon's remaining sleep turn count falls by 2 rather than 1.
		///
		/// If this Pokémon's sleep counter is at 1, it will fall to 0 and then the Pokémon will wake up.
		/// </summary>
		EARLY_BIRD = 48,
		/// <summary>
		/// Has a 30% chance of <see cref="Status.BURN"/> attacking Pokémon on contact.<para></para> Whenever a move makes contact with this Pokémon, the move's user has a 30% chance of being <see cref="Status.BURN"/>.
		///
		/// Overworld: If any Pokémon in the party has this ability, each egg in the party has its [hatch counter]{mechanic:hatch_counter} decreased by 2 (rather than 1) each [step cycle]{mechanic:step_cycle}, making eggs hatch roughly twice as quickly. This effect does not stack if multiple Pokémon have this ability or <see cref="Abilities.MAGMA_ARMOR"/>.
		/// </summary>
		FLAME_BODY = 49,
		/// <summary>
		/// Ensures success fleeing from wild battles.<para></para> This Pokémon is always successful fleeing from wild battles, even if [trapped]{mechanic:trapped} by a move or ability.
		/// </summary>
		RUN_AWAY = 50,
		/// <summary>
		/// Prevents <see cref="Stats.ACCURACY"/> from being [lowered]{mechanic:stat_modifier}.<para></para> This Pokémon cannot have its <see cref="Stats.ACCURACY"/> [lowered]{mechanic:stat_modifier}.
		///
		/// This ability does not prevent any accuracy losses other than [stat modifiers]{mechanic:stat_modifiers}, such as the accuracy cut from [fog]{mechanic:fog}; nor does it prevent other Pokémon's <see cref="Stats.EVASION"/> from making this Pokémon's moves less accurate. This Pokémon can still be passed negative accuracy modifiers through <see cref="Moves.HEART_SWAP"/>.
		///
		/// Overworld: If the first Pokémon in the party has this ability, any random encounter with a Pokémon five or more levels lower than it has a 50% chance of being skipped.
		/// </summary>
		KEEN_EYE = 51,
		/// <summary>
		/// Prevents <see cref="Stats.ATTACK"/> from being [lowered]{mechanic:stat_modifiers} by other Pokémon.<para></para> This Pokémon's <see cref="Stats.ATTACK"/> cannot be [lowered]{mechanic:stat_modifier} by other Pokémon.
		///
		/// This ability does not prevent any Attack losses other than [stat modifiers]{mechanic:stat_modifiers}, such as the Attack cut from a <see cref="Status.BURN"/>. This Pokémon can still be passed negative Attack modifiers through <see cref="Moves.HEART_SWAP"/> or <see cref="Moves.POWER_SWAP"/>.
		/// </summary>
		HYPER_CUTTER = 52,
		/// <summary>
		/// Picks up other Pokémon's used and [Flung]{move:fling} held items. May also pick up an item after battle.<para></para> At the end of each turn, if another Pokémon consumed or [Flung]{move:fling} a held item that turn, this Pokémon picks up the item if it is not already holding one. After each battle, this Pokémon has a 10% chance of picking up an item if it is not already holding one.
		///
		/// The <see cref="Items.AIR_BALLOON"/> and <see cref="Items.EJECT_BUTTON"/> cannot be picked up.
		///
		/// The items that may be found vary by game, and, since Pokémon Emerald, by the Pokémon's level. This ability is checked after the battle ends, at which point any temporary ability changes have worn off.
		/// </summary>
		PICKUP = 53,
		/// <summary>
		/// Skips every second turn.<para></para> Every second turn on which this Pokémon should attempt to use a move, it will instead do nothing ("loaf around").
		///
		/// Loafing around interrupts moves that take multiple turns the same way <see cref="Status.PARALYSIS"/>, [flinching]{mechanic:flinching}, etc do. Most such moves, for example <see cref="Moves.BIDE"/> or <see cref="Moves.ROLLOUT"/>, are simply cut off upon loafing around. Attacks with a recharge turn, such as <see cref="Moves.HYPER_BEAM"/>, do not have to recharge; attacks with a preparation turn, such as <see cref="Moves.FLY"/>, do not end up being used. Moves that are forced over multiple turns and keep going through failure, such as <see cref="Moves.OUTRAGE"/>, <see cref="Moves.UPROAR"/>, or any move forced by <see cref="Moves.ENCORE"/>, keep going as usual.
		///
		/// If this Pokémon is <see cref="IEffectsBattler.Confusion"/>, its confusion is not checked when loafing around; the Pokémon cannot hurt itself, and its confusion does not end or come closer to ending.
		///
		/// If this Pokémon attempts to move but fails, e.g. because of <see cref="Status.PARALYSIS"/> or <see cref="Moves.GRAVITY"/>, it still counts as having moved and will loaf around the next turn. If it does not attempt to move, e.g. because it is <see cref="Status.SLEEP"/> or <see cref="Status.FROZEN"/>, whatever it would have done will be postponed until its next attempt; that is, it will either loaf around or move as usual, depending on what it last did.
		///
		/// This ability cannot be changed with <see cref="Moves.WORRY_SEED"/>, but it can be disabled with <see cref="Moves.GASTRO_ACID"/>, changed with <see cref="Moves.ROLE_PLAY"/>, or traded away with <see cref="Moves.SKILL_SWAP"/>.
		/// </summary>
		TRUANT = 54,
		/// <summary>
		/// Strengthens physical moves to inflict 1.5× damage, but decreases their <see cref="Stats.ACCURACY"/> to 0.8×.<para></para> This Pokémon's physical moves do 1.5× as much [regular damage]{mechanic:regular_damage}, but have 0.8× their usual <see cref="Stats.ACCURACY"/>.
		///
		/// Special moves are unaffected. Moves that do set damage, such as <see cref="Moves.SEISMIC_TOSS"/>, have their accuracy affected, but not their damage.
		///
		/// Overworld: If the lead Pokémon has this ability, higher-leveled Pokémon have their encounter rate increased.
		/// </summary>
		HUSTLE = 55,
		/// <summary>
		/// Has a 30% chance of [infatuating]{mechanic:infatuation} attacking Pokémon on contact.<para></para> Whenever a move makes contact with this Pokémon, the move's user has a 30% chance of being [infatuated]{mechanic:infatuation}.
		///
		/// Overworld: If the first Pokémon in the party has this ability, any wild Pokémon whose species can be either gender has a 2/3 chance of being set to the opposite gender, and a 1/3 chance of having a random gender as usual.
		/// </summary>
		CUTE_CHARM = 56,
		/// <summary>
		/// Increases <see cref="Stats.SPATK"/> to 1.5× when a friendly Pokémon has <see cref="Abilities.PLUS"/> or <see cref="Abilities.MINUS"/>.<para></para> This Pokémon has 1.5× its <see cref="Stats.SPATK"/> if any friendly Pokémon has <see cref="Abilities.PLUS"/> or <see cref="Abilities.MINUS"/>.
		///
		/// This bonus does not count as a [stat modifier]{mechanic:stat_modifier}. If either ability is disabled by <see cref="Moves.GASTRO_ACID"/>, both lose their effect.
		/// </summary>
		PLUS = 57,
		/// <summary>
		/// Increases <see cref="Stats.SPATK"/> to 1.5× when a friendly Pokémon has <see cref="Abilities.PLUS"/> or <see cref="Abilities.MINUS"/>.<para></para> This Pokémon has 1.5× its <see cref="Stats.SPATK"/> if any friendly Pokémon has <see cref="Abilities.PLUS"/> or <see cref="Abilities.MINUS"/>.
		///
		/// This bonus does not count as a [stat modifier]{mechanic:stat_modifier}. If either ability is disabled by <see cref="Moves.GASTRO_ACID"/>, both lose their effect.
		/// </summary>
		MINUS = 58,
		/// <summary>
		/// Changes <see cref="Pokemons.CASTFORM"/>'s type and form to match the [weather]{mechanic:weather}.<para></para> During [rain]{mechanic:rain}, [strong sunlight]{mechanic:strong_sunlight}, or <see cref="Weather.HAIL"/>, this Pokémon's type changes to <see cref="Types.WATER"/>, <see cref="Types.FIRE"/>, or <see cref="Types.ICE"/>, respectively, and its form changes to match.
		///
		/// This ability has no effect for any Pokémon other than <see cref="Pokemons.CASTFORM"/>.
		///
		/// If the [weather]{mechanic:weather} ends or becomes anything that does not trigger this ability, or a Pokémon with <see cref="Abilities.AIR_LOCK"/> or <see cref="Abilities.CLOUD_NINE"/> enters battle, this Pokémon's type and form revert to their default. If this ability is lost or disabled, this Pokémon cannot change its current type and form until it regains its ability.
		/// </summary>
		FORECAST = 59,
		/// <summary>
		/// Prevents a held item from being removed by other Pokémon.<para></para> This Pokémon's hold item cannot be removed by other Pokémon.
		///
		/// Damaging moves that would remove this Pokémon's item can still inflict damage against this Pokémon, e.g. <see cref="Moves.KNOCK_OFF"/> or <see cref="Moves.PLUCK"/>. This Pokémon can still use moves that involve the loss of its own item, e.g. <see cref="Moves.FLING"/> or <see cref="Moves.TRICK"/>.
		///
		/// Overworld: If the lead Pokémon has this ability, the encounter rate while fishing is increased.
		/// </summary>
		STICKY_HOLD = 60,
		/// <summary>
		/// Has a 33% chance of curing any [major status ailment]{mechanic:major_status_ailment} after each turn.<para></para> After each turn, this Pokémon has a 33% of being cured of any [major status ailment]{mechanic:major_status_ailment}.
		/// </summary>
		SHED_SKIN = 61,
		/// <summary>
		/// Increases <see cref="Stats.ATTACK"/> to 1.5× with a [major status ailment]{mechanic:major_status_ailment}.<para></para> Whenever this Pokémon is <see cref="Status.SLEEP"/>, <see cref="Status.BURN"/>, <see cref="Status.PARALYSIS"/>, or <see cref="Status.POISON"/>, it has 1.5× its <see cref="Stats.ATTACK"/>. This Pokémon is not affected by the usual Attack cut from a burn.
		///
		/// This bonus does not count as a [stat modifier]{mechanic:stat_modifier}.
		/// </summary>
		GUTS = 62,
		/// <summary>
		/// Increases <see cref="Stats.DEFENSE"/> to 1.5× with a [major status ailment]{mechanic:major_status_ailment}.<para></para> Whenever this Pokémon has a [major status ailment]{mechanic:major_status_ailment}, it has 1.5× its <see cref="Stats.DEFENSE"/>.
		///
		/// This bonus does not count as a [stat modifier]{mechanic:stat_modifier}.
		/// </summary>
		MARVEL_SCALE = 63,
		/// <summary>
		/// Damages opponents using leeching moves for as much as they would heal.<para></para> Whenever a Pokémon would heal after hitting this Pokémon with a leeching move like <see cref="Moves.ABSORB"/>, it instead loses as many <see cref="Stats.HP"/> as it would usually gain.
		///
		/// <see cref="Moves.DREAM_EATER"/> is unaffected.
		/// </summary>
		LIQUID_OOZE = 64,
		/// <summary>
		/// Strengthens <see cref="Types.GRASS"/> moves to inflict 1.5× damage at 1/3 max <see cref="Stats.HP"/> or less.<para></para> When this Pokémon has 1/3 or less of its <see cref="Stats.HP"/> remaining, its <see cref="Types.GRASS"/>-type moves inflict 1.5× as much [regular damage]{mechanic:regular_damage}.
		/// </summary>
		OVERGROW = 65,
		/// <summary>
		/// Strengthens <see cref="Types.FIRE"/> moves to inflict 1.5× damage at 1/3 max <see cref="Stats.HP"/> or less.<para></para> When this Pokémon has 1/3 or less of its <see cref="Stats.HP"/> remaining, its <see cref="Types.FIRE"/>-type moves inflict 1.5× as much [regular damage]{mechanic:regular_damage}.
		/// </summary>
		BLAZE = 66,
		/// <summary>
		/// Strengthens <see cref="Types.WATER"/> moves to inflict 1.5× damage at 1/3 max <see cref="Stats.HP"/> or less.<para></para> When this Pokémon has 1/3 or less of its <see cref="Stats.HP"/> remaining, its <see cref="Types.WATER"/>-type moves inflict 1.5× as much [regular damage]{mechanic:regular_damage}.
		/// </summary>
		TORRENT = 67,
		/// <summary>
		/// Strengthens <see cref="Types.BUG"/> moves to inflict 1.5× damage at 1/3 max <see cref="Stats.HP"/> or less.<para></para> When this Pokémon has 1/3 or less of its <see cref="Stats.HP"/> remaining, its <see cref="Types.BUG"/>-type moves inflict 1.5× as much [regular damage]{mechanic:regular_damage}.
		///
		/// Overworld: If the lead Pokémon has this ability, the wild encounter rate is increased.
		/// </summary>
		SWARM = 68,
		/// <summary>
		/// Protects against recoil damage.<para></para> This Pokémon does not receive recoil damage from its recoil moves.
		///
		/// <see cref="Moves.STRUGGLE"/>'s recoil is unaffected. This ability does not prevent crash damage from missing with <see cref="Moves.JUMP_KICK"/> or <see cref="Moves.HIGH_JUMP_KICK"/>.
		/// </summary>
		ROCK_HEAD = 69,
		/// <summary>
		/// Summons [strong sunlight]{mechanic:strong_sunlight} that lasts indefinitely upon entering battle.<para></para> The [weather]{mechanic:weather} changes to [strong sunlight]{mechanic:strong_sunlight} when this Pokémon enters battle and does not end unless cancelled by another weather condition.
		///
		/// If multiple Pokémon with this ability, <see cref="Abilities.DRIZZLE"/>, <see cref="Abilities.SAND_STREAM"/>, or <see cref="Abilities.SNOW_WARNING"/> are sent out at the same time, the abilities will activate in order of <see cref="Stats.SPEED"/>, respecting <see cref="Moves.TRICK_ROOM"/>. Each ability's weather will cancel the previous weather, and only the weather summoned by the slowest of the Pokémon will stay.
		/// </summary>
		DROUGHT = 70,
		/// <summary>
		/// Prevents opponents from fleeing or switching out. Eluded by <see cref="Types.FLYING"/>-types and Pokémon in the air.<para></para> While this Pokémon is in battle, opposing Pokémon cannot flee or switch out.  <see cref="Types.FLYING"/>-type Pokémon and Pokémon in the air, e.g. due to <see cref="Abilities.LEVITATE"/> or <see cref="Moves.MAGNET_RISE"/>, are unaffected.
		///
		/// Pokémon with <see cref="Abilities.RUN_AWAY"/> can still flee. Pokémon can still switch out with the use of a move or item.
		///
		/// Overworld: If the lead Pokémon has this ability, the wild encounter rate is doubled.
		/// </summary>
		ARENA_TRAP = 71,
		/// <summary>
		/// Prevents <see cref="Status.SLEEP"/>.<para></para> This Pokémon cannot be <see cref="Status.SLEEP"/>.
		///
		/// This causes <see cref="Moves.REST"/> to fail altogether. If a Pokémon is asleep and acquires this ability, it will immediately wake up; this includes when regaining a lost ability upon leaving battle.
		///
		/// This ability functions identically to <see cref="Abilities.INSOMNIA"/> in battle.
		///
		/// Overworld: If the lead Pokémon has this ability, higher-leveled Pokémon have their encounter rate increased.
		/// </summary>
		VITAL_SPIRIT = 72,
		/// <summary>
		/// Prevents stats from being [lowered]{mechanic:stat_modifier} by other Pokémon.<para></para> This Pokémon cannot have its stats [lowered]{mechanic:stat_modifier} by other Pokémon.
		///
		/// This ability does not prevent any stat losses other than [stat modifiers]{mechanic:stat_modifiers}, such as the <see cref="Stats.SPEED"/> cut from <see cref="Status.PARALYSIS"/>; nor self-inflicted stat drops, such as the <see cref="Stats.SPATK"/> drop from <see cref="Moves.OVERHEAT"/>; nor opponent-triggered stat boosts, such as the <see cref="Stats.ATTACK"/> boost from <see cref="Moves.SWAGGER"/>. This Pokémon can still be passed negative stat modifiers through <see cref="Moves.GUARD_SWAP"/>, <see cref="Moves.HEART_SWAP"/>, or <see cref="Moves.POWER_SWAP"/>.
		///
		/// This ability functions identically to <see cref="Abilities.CLEAR_BODY"/> in battle.
		///
		/// Overworld: If the lead Pokémon has this ability, the wild encounter rate is halved.
		/// </summary>
		WHITE_SMOKE = 73,
		/// <summary>
		/// Doubles <see cref="Stats.ATTACK"/> in battle.<para></para> This Pokémon's <see cref="Stats.ATTACK"/> is doubled in battle.
		///
		/// This bonus does not count as a [stat modifier]{mechanic:stat_modifier}.
		///
		/// This ability functions identically to <see cref="Abilities.HUGE_POWER"/>.
		/// </summary>
		PURE_POWER = 74,
		/// <summary>
		/// Protects against [critical hits]{mechanic:critical_hit}.<para></para> Moves cannot score [critical hits]{mechanic:critical_hit} against this Pokémon.
		///
		/// This ability functions identically to <see cref="Abilities.BATTLE_ARMOR"/>.
		/// </summary>
		SHELL_ARMOR = 75,
		/// <summary>
		/// Negates all effects of [weather]{mechanic:weather}, but does not prevent the weather itself.<para></para> While this Pokémon is in battle, [weather]{mechanic:weather} can still be in play, but will not have any of its effects.
		///
		/// This ability functions identically to <see cref="Abilities.CLOUD_NINE"/>.
		/// </summary>
		AIR_LOCK = 76,
		/// <summary>
		/// Doubles <see cref="Stats.EVASION"/> when <see cref="IEffectsBattler.Confusion"/>.<para></para> When this Pokémon is <see cref="IEffectsBattler.Confusion"/>, it has twice its <see cref="Stats.EVASION"/>.
		/// </summary>
		TANGLED_FEET = 77,
		/// <summary>
		/// Absorbs <see cref="Types.ELECTRIC"/> moves, raising <see cref="Stats.SPEED"/> one [stage]{mechanic:stat_modifier}.<para></para> Whenever an <see cref="Types.ELECTRIC"/>-type move hits this Pokémon, its <see cref="Stats.SPEED"/> rises one [stage]{mechanic:stat_modifier}, negating any other effect on it.
		///
		/// This ability will not take effect if this Pokémon is immune to Electric moves. Electric moves will ignore this Pokémon's <see cref="Moves.SUBSTITUTE"/>.
		///
		/// This effect includes non-damaging moves, i.e. <see cref="Moves.THUNDER_WAVE"/>.
		/// </summary>
		MOTOR_DRIVE = 78,
		/// <summary>
		/// Increases damage inflicted to 1.25× against Pokémon of the same gender, but decreases damage to 0.75× against the opposite gender.<para></para> This Pokémon inflicts 1.25× as much [regular damage]{mechanic:regular_damage} against Pokémon of the same gender and 0.75× as much regular damage against Pokémon of the opposite gender.
		///
		/// If either Pokémon is genderless, damage is unaffected.
		/// </summary>
		RIVALRY = 79,
		/// <summary>
		/// Raises <see cref="Stats.SPEED"/> one [stage]{mechanic:stat_modifier} upon [flinching]{mechanic:flinching}.<para></para> Whenever this Pokémon [flinches]{mechanic:flinching}, its <see cref="Stats.SPEED"/> rises one [stage]{mechanic:stat_modifier}.
		/// </summary>
		STEADFAST = 80,
		/// <summary>
		/// Increases evasion to 1.25× during <see cref="Weather.HAIL"/>. Protects against hail damage.<para></para> During <see cref="Weather.HAIL"/>, this Pokémon has 1.25× its <see cref="Stats.EVASION"/>, and it does not take hail damage regardless of type.
		///
		/// The evasion bonus does not count as a [stat modifier]{mechanic:stat_modifier}.
		///
		/// Overworld: If the lead Pokémon has this ability, the wild encounter rate is halved in snow.
		/// </summary>
		SNOW_CLOAK = 81,
		/// <summary>
		/// Makes the Pokémon eat any held Berry triggered by low <see cref="Stats.HP"/> below 1/2 its max HP.<para></para> This Pokémon eats any held Berry triggered by low <see cref="Stats.HP"/> when it falls below 50% of its HP, regardless of the Berry's usual threshold.
		/// </summary>
		GLUTTONY = 82,
		/// <summary>
		/// Raises <see cref="Stats.ATTACK"/> to the maximum of six [stages]{mechanic:stat_modifier} upon receiving a [critical hit]{mechanic:critical_hit}.<para></para> Whenever this Pokémon receives a [critical hit]{mechanic:critical_hit}, its <see cref="Stats.ATTACK"/> rises to the maximum of 6 [stages]{mechanic:stat_modifier}.
		///
		/// This ability will still take effect if the critical hit is received by a <see cref="Moves.SUBSTITUTE"/>.
		/// </summary>
		ANGER_POINT = 83,
		/// <summary>
		/// Doubles <see cref="Stats.SPEED"/> upon using or losing a held item.<para></para> When this Pokémon uses or loses its held item, its <see cref="Stats.SPEED"/> is doubled. If it gains another item or leaves battle, this bonus is lost.
		///
		/// This includes when the Pokémon drops its item because of <see cref="Moves.KNOCK_OFF"/>. This bonus does not count as a [stat modifier]{mechanic:stat_modifier}. There is no notification when this ability takes effect.
		/// </summary>
		UNBURDEN = 84,
		/// <summary>
		/// Halves damage from <see cref="Types.FIRE"/> moves and <see cref="Status.BURN"/>.<para></para> This Pokémon takes half as much damage from <see cref="Types.FIRE"/>-type moves and <see cref="Status.BURN"/>.
		/// </summary>
		HEATPROOF = 85,
		/// <summary>
		/// Doubles the Pokémon's [stat modifiers]{mechanic:stat_modifiers}. These doubled modifiers are still capped at -6 or 6 stages.<para></para> Each stage of this Pokémon's [stat modifiers]{mechanic:stat_modifiers} acts as two stages. These doubled stages are still limited to a minimum of -6 and a maximum of 6.
		///
		/// This Pokémon can still accumulate less than -3 or more than 3 stages of stat modifiers, even though the extra ones have no effect after doubling.
		/// </summary>
		SIMPLE = 86,
		/// <summary>
		/// Causes 1/8 max <see cref="Stats.HP"/> in damage each turn during [strong sunlight]{mechanic:strong_sunlight}, but heals for 1/8 max HP during [rain]{mechanic:rain}. Increases damage from <see cref="Types.FIRE"/> moves to 1.25×, but absorbs <see cref="Types.WATER"/> moves, healing for 1/4 max HP.<para></para> This Pokémon takes 1/8 of its maximum HP in damage after each turn during [strong sunlight]{mechanic:strong_sunlight}, but it heals for 1/8 of its HP each turn during [rain]{mechanic:rain}. This Pokémon takes 1.25× as much damage from <see cref="Types.FIRE"/>-type moves, but whenever a <see cref="Types.WATER"/> move hits it, it heals for 1/4 its maximum HP instead.
		/// </summary>
		DRY_SKIN = 87,
		/// <summary>
		/// Raises the attack stat corresponding to the opponents' weaker defense one [stage]{mechanic:stat_modifier} upon entering battle.<para></para> When this Pokémon enters battle, its <see cref="Stats.ATTACK"/> or <see cref="Stats.SPATK"/>, whichever corresponds to its opponents' weaker total defensive stat, rises one [stage]{mechanic:stat_modifier}. In the event of a tie, Special Attack is raised.
		///
		/// This ability also takes effect when acquired during a battle.
		/// </summary>
		DOWNLOAD = 88,
		/// <summary>
		/// Strengthens punch-based moves to 1.2× their power.<para></para> Moves flagged as being punch-based have 1.2× their base power for this Pokémon.
		///
		/// <see cref="Moves.SUCKER_PUNCH"/> is not flagged as punch-based; its original, Japanese name only means "surprise attack".
		/// </summary>
		IRON_FIST = 89,
		/// <summary>
		/// Heals for 1/8 max <see cref="Stats.HP"/> after each turn when <see cref="Status.POISON"/> in place of damage.<para></para> If this Pokémon is <see cref="Status.POISON"/>, it will heal for 1/8 of its maximum <see cref="Stats.HP"/> after each turn rather than taking damage. This includes bad poison.
		/// </summary>
		POISON_HEAL = 90,
		/// <summary>
		/// Increases the [same-type attack bonus]{mechanic:same-type-attack-bonus} from 1.5× to 2×.<para></para> This Pokémon inflicts twice as much damage with moves whose types match its own, rather than the usual [same-type attack bonus]{mechanic:same-type-attack-bonus} of 1.5×.
		/// </summary>
		ADAPTABILITY = 91,
		/// <summary>
		/// Extends two-to-five-hit moves and <see cref="Moves.TRIPLE_KICK"/> to their full length every time.<para></para> This Pokémon always hits five times with two-to-five-hit moves, such as <see cref="Moves.ICICLE_SPEAR"/>. It also bypasses the accuracy checks on <see cref="Moves.TRIPLE_KICK"/>'s second and third hits.
		/// </summary>
		SKILL_LINK = 92,
		/// <summary>
		/// Cures any [major status ailment]{mechanic:major_status_ailment} after each turn during [rain]{mechanic:rain}.<para></para> This Pokémon is cured of any [major status ailment]{mechanic:major_status_ailment} after each turn during [rain]{mechanic:rain}.
		/// </summary>
		HYDRATION = 93,
		/// <summary>
		/// Increases <see cref="Stats.SPATK"/> to 1.5× but costs 1/8 max <see cref="Stats.HP"/> after each turn during [strong sunlight]{mechanic:strong_sunlight}.<para></para> During [strong sunlight]{mechanic:strong_sunlight}, this Pokémon has 1.5× its <see cref="Stats.SPATK"/> but takes 1/8 of its maximum <see cref="Stats.HP"/> in damage after each turn.
		/// </summary>
		SOLAR_POWER = 94,
		/// <summary>
		/// Increases <see cref="Stats.SPEED"/> to 1.5× with a [major status ailment]{mechanic:major_status_ailment}.<para></para> Whenever this Pokémon has a [major status ailment]{mechanic:major_status_ailment}, it has 1.5× its <see cref="Stats.SPEED"/>. This Pokémon is not affected by the usual Speed cut from <see cref="Status.PARALYSIS"/>.
		///
		/// Overworld: If the lead Pokémon has this ability, the wild encounter rate is halved.
		/// </summary>
		QUICK_FEET = 95,
		/// <summary>
		/// Makes the Pokémon's moves all act <see cref="Types.NORMAL"/>-type.<para></para> This Pokémon's moves all act as if they were <see cref="Types.NORMAL"/>-type.
		///
		/// Moves that inflict [typeless damage]{mechanic:typeless_damage} do so as usual. Moves of variable type, such as <see cref="Moves.HIDDEN_POWER"/>, are affected. They otherwise work as usual, however; <see cref="Moves.WEATHER_BALL"/>, for example, is always forced to be Normal, but it still has doubled power and looks different during [weather]{mechanic:weather}.
		///
		/// As <see cref="Moves.THUNDER_WAVE"/> is prevented by immunities, unlike most non-damaging moves, it does not affect <see cref="Types.GHOST"/>-type Pokémon under the effect of this ability.
		/// </summary>
		NORMALIZE = 96,
		/// <summary>
		/// Strengthens [critical hits]{mechanic:critical_hit} to inflict 3× damage rather than 2×.<para></para> This Pokémon inflicts triple damage with [critical hits]{mechanic:critical_hit}, rather than the usual double damage.
		/// </summary>
		SNIPER = 97,
		/// <summary>
		/// Protects against damage not directly caused by a move.<para></para> This Pokémon is immune to damage not directly caused by a move.
		///
		/// For example, this Pokémon takes no damage from from [weather]{mechanic:weather}, recoil, [status ailments]{mechanic:status_ailments}, or <see cref="Moves.SPIKES"/>, but it still suffers from the <see cref="Stats.ATTACK"/> cut when <see cref="Status.BURN"/>, and a <see cref="Items.LIFE_ORB"/> will still power up this Pokémon's moves without damaging it. Anything that directly depends on such damage will also not happen; for example, <see cref="Moves.LEECH_SEED"/> will neither hurt this Pokémon nor heal the opponent, and Pokémon with a <see cref="Items.JABOCA_BERRY"/> or <see cref="Items.ROWAP_BERRY"/> will not consume the berry when hit by this Pokémon.
		///
		/// The following are unaffected: <see cref="Moves.STRUGGLE"/>, <see cref="Moves.PAIN_SPLIT"/> (whether used by or against this Pokémon), <see cref="Moves.BELLY_DRUM"/>, <see cref="Moves.SUBSTITUTE"/>, <see cref="Moves.CURSE"/>, moves that knock the user out, and damage from <see cref="IEffectsBattler.Confusion"/>.
		///
		/// This Pokémon will neither lose nor regain <see cref="Stats.HP"/> if it drains HP from a Pokémon with <see cref="Abilities.LIQUID_OOZE"/>.
		///
		/// If this Pokémon is <see cref="Status.POISON"/>, the poison counter is still increased each turn; if the Pokémon loses this ability, it will begin taking as much damage as it would be if it had been taking increasing damage each turn.
		/// </summary>
		MAGIC_GUARD = 98,
		/// <summary>
		/// Ensures all moves used by and against the Pokémon hit.<para></para> Moves used by or against this Pokémon never miss.
		///
		/// One-hit KO moves are unaffected. Moves affected by this ability can hit Pokémon during the preparation turn of moves like <see cref="Moves.DIG"/> or <see cref="Moves.FLY"/>.
		///
		/// Overworld: If the lead Pokémon has this ability, the wild encounter rate is doubled.
		/// </summary>
		NO_GUARD = 99,
		/// <summary>
		/// Makes the Pokémon move last within its move's priority bracket.<para></para> This Pokémon moves last within its priority bracket.
		///
		/// Multiple Pokémon with this ability move in order of <see cref="Stats.SPEED"/> amongst themselves.
		///
		/// The <see cref="Items.FULL_INCENSE"/> and <see cref="Items.LAGGING_TAIL"/> take precedence over this ability; that is, Pokémon with these items move after Pokémon with this ability. Pokémon with both this ability and one of these items are delayed as much as if they had only the item.
		///
		/// This ability works as usual during <see cref="Moves.TRICK_ROOM"/>: Pokémon with this ability will move in reverse order of Speed after Pokémon without it.
		/// </summary>
		STALL = 100,
		/// <summary>
		/// Strengthens moves of 60 base power or less to 1.5× their power.<para></para> This Pokémon's moves have 1.5× their power if their base power is 60 or less.
		///
		/// This includes moves of variable power, such as <see cref="Moves.HIDDEN_POWER"/> and <see cref="Moves.MAGNITUDE"/>, when their power is 60 or less.  <see cref="Moves.HELPING_HAND"/>'s power boost is taken into account for any move, as is <see cref="Moves.DEFENSE_CURL"/>'s power boost for <see cref="Moves.ROLLOUT"/>.
		/// </summary>
		TECHNICIAN = 101,
		/// <summary>
		/// Protects against [major status ailments]{mechanic:major_status_ailments} during [strong sunlight]{mechanic:strong_sunlight}.<para></para> This Pokémon cannot be given a [major status ailment]{mechanic:major_status_ailment} during [strong sunlight]{mechanic:strong_sunlight}.
		///
		/// This ability does not heal prior status ailments.  <see cref="Moves.REST"/> will fail altogether with this ability in effect.  <see cref="Moves.YAWN"/> will immediately fail if used on this Pokémon during strong sunlight, and an already-used Yawn will fail if the weather turns to strong sunlight in the meantime.
		/// </summary>
		LEAF_GUARD = 102,
		/// <summary>
		/// Prevents the Pokémon from using its held item in battle.<para></para> In battle, this Pokémon cannot use its held item, nor will the item have any passive effect on the battle, positive or negative. This Pokémon also cannot use <see cref="Moves.FLING"/>.
		///
		/// The <see cref="Stats.SPEED"/> cut from the <see cref="Items.IRON_BALL"/> and the effort items (the <see cref="Items.MACHO_BRACE"/>, <see cref="Items.POWER_WEIGHT"/>, <see cref="Items.POWER_BRACER"/>, <see cref="Items.POWER_BELT"/>, <see cref="Items.POWER_LENS"/>, <see cref="Items.POWER_BAND"/>, and <see cref="Items.POWER_ANKLET"/>) is unaffected. Items that do not directly affect the battle, such as the <see cref="Items.EXP_SHARE"/>, the <see cref="Items.AMULET_COIN"/>, or the <see cref="Items.SOOTHE_BELL"/>, work as usual. All held items work as usual out of battle.
		///
		/// Other moves that use the held item, such as <see cref="Moves.NATURAL_GIFT"/> and <see cref="Moves.SWITCHEROO"/>, work as usual.
		/// </summary>
		KLUTZ = 103,
		/// <summary>
		/// Bypasses targets' abilities if they could hinder or prevent a move.<para></para> This Pokémon's moves completely ignore abilities that could hinder or prevent their effect on the target.
		///
		/// For example, this Pokémon's moves ignore abilities that would fully negate them, such as <see cref="Abilities.WATER_ABSORB"/>; abilities that would prevent any of their effects, such as <see cref="Abilities.CLEAR_BODY"/>, <see cref="Abilities.SHELL_ARMOR"/>, or <see cref="Abilities.STICKY_HOLD"/>; and abilities that grant any general protective benefit, such as <see cref="Abilities.SIMPLE"/>, <see cref="Abilities.SNOW_CLOAK"/>, or <see cref="Abilities.THICK_FAT"/>. If an ability could either hinder or help this Pokémon's moves, e.g. <see cref="Abilities.DRY_SKIN"/> or <see cref="Abilities.UNAWARE"/>, the ability is ignored either way.
		///
		/// Abilities that do not fit this description, even if they could hinder moves in some other way, are not affected. For example, <see cref="Abilities.CURSED_BODY"/> only affects potential future uses of the move, while <see cref="Abilities.LIQUID_OOZE"/> and <see cref="Abilities.SHADOW_TAG"/> can only hinder a move's effect on the user. This ablity cannot ignore type or form changes granted by abilities, for example <see cref="Abilities.COLOR_CHANGE"/> or <see cref="Abilities.FORECAST"/>; nor effects that were caused by abilities but are no longer tied to an ability, such as the rain from <see cref="Abilities.DRIZZLE"/>. This ability cannot ignore <see cref="Abilities.MULTITYPE"/> at all.
		///
		/// An ability ignored by this ability is only nullified while the move is being used. For example, this Pokémon's moves can <see cref="Status.PARALYSIS"/> a Pokémon with <see cref="Abilities.LIMBER"/>, but Limber will activate and heal the paralysis immediately thereafter, and this Pokémon's <see cref="Moves.SPIKES"/> are not affected by this ability after they have been placed.
		///
		/// When this Pokémon enters battle, all participating trainers are notified that it has this ability.
		///
		/// This ability functions identically to <see cref="Abilities.TERAVOLT"/> and <see cref="Abilities.TURBOBLAZE"/>.
		/// </summary>
		MOLD_BREAKER = 104,
		/// <summary>
		/// Raises moves' [critical hit]{mechanic:critical_hit} rates one stage.<para></para> This Pokémon's moves have [critical hit]{mechanic:critical_hit} rates one stage higher than normal.
		/// </summary>
		SUPER_LUCK = 105,
		/// <summary>
		/// Damages the attacker for 1/4 its max <see cref="Stats.HP"/> when knocked out by a contact move.<para></para> When this Pokémon is knocked out by a move that makes contact, the move's user takes 1/4 its maximum <see cref="Stats.HP"/> in damage.
		/// </summary>
		AFTERMATH = 106,
		/// <summary>
		/// Notifies all trainers upon entering battle if an opponent has a <see cref="TypeEffective.SuperEffective"/> move, <see cref="Moves.SELF_DESTRUCT"/>, <see cref="Moves.EXPLOSION"/>, or a one-hit KO move.<para></para> When this Pokémon enters battle, if one of its opponents has a move that is <see cref="TypeEffective.SuperEffective"/> against it, <see cref="Moves.SELF_DESTRUCT"/>, <see cref="Moves.EXPLOSION"/>, or a one-hit knockout move, all participating trainers are notified.
		///
		/// The move itself is not revealed; only that there is such a move. Moves that inflict [typeless damage]{mechanic:typeless_damage}, such as <see cref="Moves.FUTURE_SIGHT"/>, and moves of variable type, such as <see cref="Moves.HIDDEN_POWER"/>, count as their listed types.  <see cref="Moves.COUNTER"/>, <see cref="Moves.METAL_BURST"/>, <see cref="Moves.MIRROR_COAT"/>, and one-hit KO moves to which this Pokémon is immune do not trigger this ability.
		/// </summary>
		ANTICIPATION = 107,
		/// <summary>
		/// Reveals the opponents' strongest move upon entering battle.<para></para> When this Pokémon enters battle, it reveals the move with the highest base power known by any opposing Pokémon to all participating trainers.
		///
		/// In the event of a tie, one is chosen at random.
		///
		/// Moves without a listed base power are assigned one as follows:
		///
		/// Power | Moves
		/// ----: | -----
		///   160 | One-hit KO moves: <see cref="Moves.FISSURE"/>, <see cref="Moves.GUILLOTINE"/>, <see cref="Moves.HORN_DRILL"/>, and <see cref="Moves.SHEER_COLD"/>
		///   120 | Counter moves: <see cref="Moves.COUNTER"/>, <see cref="Moves.METAL_BURST"/>, and <see cref="Moves.MIRROR_COAT"/>
		///    80 | Variable power or set damage: <see cref="Moves.CRUSH_GRIP"/>, <see cref="Moves.DRAGON_RAGE"/>, <see cref="Moves.ELECTRO_BALL"/>, <see cref="Moves.ENDEAVOR"/>, <see cref="Moves.FINAL_GAMBIT"/>, <see cref="Moves.FLAIL"/>, <see cref="Moves.FRUSTRATION"/>, <see cref="Moves.GRASS_KNOT"/>, <see cref="Moves.GYRO_BALL"/>, <see cref="Moves.HEAT_CRASH"/>, <see cref="Moves.HEAVY_SLAM"/>, <see cref="Moves.HIDDEN_POWER"/>, <see cref="Moves.LOW_KICK"/>, <see cref="Moves.NATURAL_GIFT"/>, <see cref="Moves.NIGHT_SHADE"/>, <see cref="Moves.PSYWAVE"/>, <see cref="Moves.RETURN"/>, <see cref="Moves.REVERSAL"/>, <see cref="Moves.SEISMIC_TOSS"/>, <see cref="Moves.SONIC_BOOM"/>, <see cref="Moves.TRUMP_CARD"/>, and <see cref="Moves.WRING_OUT"/>
		///     0 | Any such move not listed
		/// </summary>
		FOREWARN = 108,
		/// <summary>
		/// Ignores other Pokémon's stat modifiers for damage and accuracy calculation.<para></para> This Pokémon ignores other Pokémon's [stat modifiers]{mechanic:stat_modifiers} for the purposes of damage and accuracy calculation.
		///
		/// Effectively, this affects modifiers of every stat except <see cref="Stats.SPEED"/>.
		///
		/// The power of <see cref="Moves.PUNISHMENT"/> and <see cref="Moves.STORED_POWER"/> is calculated as usual. When this Pokémon hurts itself in <see cref="IEffectsBattler.Confusion"/>, its stat modifiers affect damage as usual.
		/// </summary>
		UNAWARE = 109,
		/// <summary>
		/// Doubles damage inflicted with [not-very-effective]{mechanic:not_very_effective} moves.<para></para> This Pokémon deals twice as much damage with moves that are [not very effective]{mechanic:not_very_effective} against the target.
		/// </summary>
		TINTED_LENS = 110,
		/// <summary>
		/// Decreases damage taken from <see cref="TypeEffective.SuperEffective"/> moves by 1/4.<para></para> This Pokémon takes 0.75× as much damage from moves that are <see cref="TypeEffective.SuperEffective"/> against it.
		///
		/// This ability functions identically to <see cref="Abilities.SOLID_ROCK"/>.
		/// </summary>
		FILTER = 111,
		/// <summary>
		/// Halves <see cref="Stats.ATTACK"/> and <see cref="Stats.SPEED"/> for five turns upon entering battle.<para></para> This Pokémon's <see cref="Stats.ATTACK"/> and <see cref="Stats.SPEED"/> are halved for five turns upon entering battle.
		///
		/// This ability also takes effect when acquired during battle. If this Pokémon loses its ability before the five turns are up, its Attack and Speed return to normal; if it then regains this ability without leaving battle, its Attack and Speed are halved again, but the counter keeps counting from where it was.
		/// </summary>
		SLOW_START = 112,
		/// <summary>
		/// Lets the Pokémon's <see cref="Types.NORMAL"/> and <see cref="Types.FIGHTING"/> moves hit <see cref="Types.GHOST"/> Pokémon.<para></para> This Pokémon ignores <see cref="Types.GHOST"/>-type Pokémon's immunity to <see cref="Types.NORMAL"/>- and <see cref="Types.FIGHTING"/>-type moves.
		///
		/// Ghost Pokémon's other types affect damage as usual.
		/// </summary>
		SCRAPPY = 113,
		/// <summary>
		/// Redirects single-target <see cref="Types.WATER"/> moves to this Pokémon where possible. Absorbs Water moves, raising <see cref="Stats.SPATK"/> one [stage]{mechanic:stat_modifier}.<para></para> All other Pokémon's single-target <see cref="Types.WATER"/>-type moves are redirected to this Pokémon, if it is an eligible target. Other Pokémon's Water moves raise this Pokémon's <see cref="Stats.SPATK"/> one [stage]{mechanic:stat_modifier}, negating any other effect on it, and cannot miss it.
		///
		/// If the move's intended target also has this ability, the move is not redirected. When multiple Pokémon with this ability are possible targets for redirection, the move is redirected to the one with the highest <see cref="Stats.SPEED"/> stat, or, in the case of a tie, to a random tied Pokémon.  <see cref="Moves.FOLLOW_ME"/> takes precedence over this ability.
		/// </summary>
		STORM_DRAIN = 114,
		/// <summary>
		/// Heals for 1/16 max <see cref="Stats.HP"/> after each turn during hail. Protects against hail damage.<para></para> This Pokémon heals for 1/16 of its maximum <see cref="Stats.HP"/> after each turn during <see cref="Weather.HAIL"/>, and it does not take hail damage regardless of type.
		/// </summary>
		ICE_BODY = 115,
		/// <summary>
		/// Decreases damage taken from <see cref="TypeEffective.SuperEffective"/> moves by 1/4.<para></para> This Pokémon takes 0.75× as much damage from moves that are <see cref="TypeEffective.SuperEffective"/> against it.
		///
		/// This ability functions identically to <see cref="Abilities.FILTER"/>.
		/// </summary>
		SOLID_ROCK = 116,
		/// <summary>
		/// Summons <see cref="Weather.HAIL"/> that lasts indefinitely upon entering battle.<para></para> The [weather]{mechanic:weather} changes to <see cref="Weather.HAIL"/> when this Pokémon enters battle and does not end unless cancelled by another weather condition.
		///
		/// If multiple Pokémon with this ability, <see cref="Abilities.DRIZZLE"/>, <see cref="Abilities.DROUGHT"/>, or <see cref="Abilities.SAND_STREAM"/> are sent out at the same time, the abilities will activate in order of <see cref="Stats.SPEED"/>, respecting <see cref="Moves.TRICK_ROOM"/>. Each ability's weather will cancel the previous weather, and only the weather summoned by the slowest of the Pokémon will stay.
		/// </summary>
		SNOW_WARNING = 117,
		/// <summary>
		/// The Pokémon may pick up <see cref="Items.HONEY"/> after battle.<para></para> This Pokémon has a chance of picking up <see cref="Items.HONEY"/> after each battle. This chance starts at 5% and rises another 5% after every tenth level: 5% from level 1–10, 10% from 11–20, and so on, up to 50% from 91–100.
		///
		/// This ability is checked after the battle ends, at which point any temporary ability changes have worn off.
		/// </summary>
		HONEY_GATHER = 118,
		/// <summary>
		/// Reveals an opponent's held item upon entering battle.<para></para> When this Pokémon enters battle, it reveals an opposing Pokémon's held item to all participating trainers.
		///
		/// In a double battle, if one opponent has an item, this Pokémon will Frisk that Pokémon; if both have an item, it will Frisk one at random.
		/// </summary>
		FRISK = 119,
		/// <summary>
		/// Strengthens recoil moves to 1.2× their power.<para></para> This Pokémon's recoil moves and crash moves have 1.2× their base power.
		///
		/// <see cref="Moves.STRUGGLE"/> is unaffected.
		///
		/// The "crash moves" are the moves that damage the user upon missing: <see cref="Moves.JUMP_KICK"/> and <see cref="Moves.HIGH_JUMP_KICK"/>.
		/// </summary>
		RECKLESS = 120,
		/// <summary>
		/// Changes <see cref="Pokemons.ARCEUS"/>'s type and form to match its held Plate.<para></para> If this Pokémon is holding an elemental Plate, its type and form change to match the Plate.
		///
		/// This Pokémon's held item, whether or not it is a Plate, cannot be taken by <see cref="Moves.COVET"/> or <see cref="Moves.THIEF"/>, nor removed by <see cref="Moves.KNOCK_OFF"/>, nor traded by <see cref="Moves.SWITCHEROO"/> or <see cref="Moves.TRICK"/>. Covet, Thief, and Knock Off still inflict damage against this Pokémon. Unlike with <see cref="Abilities.STICKY_HOLD"/>, this Pokémon cannot use <see cref="Moves.FLING"/>, Switcheroo, or Trick to lose its item itself, nor gain an item through Switcheroo or Trick if it does not have one.
		///
		/// This ability has no effect for any Pokémon other than <see cref="Pokemons.ARCEUS"/>. This ability cannot be traded with <see cref="Moves.SKILL_SWAP"/>, nor copied with <see cref="Moves.ROLE_PLAY"/> or <see cref="Abilities.TRACE"/>, nor disabled with <see cref="Moves.GASTRO_ACID"/>, nor changed with <see cref="Moves.WORRY_SEED"/>. This Pokémon cannot use Skill Swap or Role Play to lose its ability itself.  <see cref="Abilities.MOLD_BREAKER"/> cannot ignore this ability.
		///
		/// If a Pokémon [Transforms]{move:transform} into an Arceus with this ability, it will Transform into Arceus's default, <see cref="Types.NORMAL"/>-type form. If the Transforming Pokémon is holding a Plate, this ability will then activate and change the Pokémon into the corresponding form.
		/// </summary>
		MULTITYPE = 121,
		/// <summary>
		/// Increases friendly Pokémon's <see cref="Stats.ATTACK"/> and <see cref="Stats.SPDEF"/> to 1.5× during [strong sunlight]{mechanic:strong_sunlight}.<para></para> Friendly Pokémon have 1.5× their <see cref="Stats.ATTACK"/> and <see cref="Stats.SPDEF"/> during [strong sunlight]{mechanic:strong_sunlight} if any friendly Pokémon has this ability.
		///
		/// Unlike <see cref="Abilities.FORECAST"/>, <see cref="Abilities.MULTITYPE"/>, and <see cref="Abilities.ZEN_MODE"/>, this ability is not tied to its Pokémon's form change; <see cref="Pokemons.CHERRIM"/> will switch between its forms even if it loses this ability. As such, this ability also works if obtained by a Pokémon other than Cherrim.
		/// </summary>
		FLOWER_GIFT = 122,
		/// <summary>
		/// Damages <see cref="Status.SLEEP"/> opponents for 1/8 their max <see cref="Stats.HP"/> after each turn.<para></para> Opposing Pokémon take 1/8 of their maximum <see cref="Stats.HP"/> in damage after each turn while they are [asleep]{mechanic:asleep}.
		/// </summary>
		BAD_DREAMS = 123,
		/// <summary>
		/// Steals attacking Pokémon's held items on contact.<para></para> Whenever a move makes contact with this Pokémon, if it does not have a held item, it steals the attacker's held item.
		///
		/// This Pokémon cannot steal upon being knocked out. It can steal if the attacker has a <see cref="Moves.SUBSTITUTE"/>, but cannot steal when its own Substitute is hit. If a move hits multiple times, only the last hit triggers this ability. If this Pokémon is wild, it cannot steal from a trained Pokémon.
		/// </summary>
		PICKPOCKET = 124,
		/// <summary>
		/// Strengthens moves with extra effects to 1.3× their power, but prevents their extra effects.<para></para> This Pokémon's moves with extra effects have 1.3× their power, but lose their extra effects.
		///
		/// An effect chance is a move's chance to inflict a status ailment, cause a stat change, or make the target flinch in addition to the move's main effect. For example, <see cref="Moves.THUNDER_SHOCK"/>'s paralysis is an extra effect, but <see cref="Moves.THUNDER_WAVE"/>'s is not, nor are <see cref="Moves.KNOCK_OFF"/>'s item removal and <see cref="Moves.AIR_CUTTER"/>'s increased [critical hit]{mechanic:critical_hit} rate.
		///
		/// Moves that lower the user's stats are unaffected.
		/// </summary>
		SHEER_FORCE = 125,
		/// <summary>
		/// Inverts [stat changes]{mechanic:stat_modifiers}.<para></para> Whenever this Pokémon's stats would be [raised]{mechanic:stat_modifiers}, they are instead lowered by the same amount, and vice versa.
		/// </summary>
		CONTRARY = 126,
		/// <summary>
		/// Prevents opposing Pokémon from eating held Berries.<para></para> Opposing Pokémon cannot eat held Berries while this Pokémon is in battle.
		///
		/// Affected Pokémon can still use <see cref="Moves.BUG_BITE"/> or <see cref="Moves.PLUCK"/> to eat a target's Berry.
		/// </summary>
		UNNERVE = 127,
		/// <summary>
		/// Raises <see cref="Stats.ATTACK"/> two [stages]{mechanic:stat_modifier} upon having any stat lowered.<para></para> When any of this Pokémon's stats are [lowered]{mechanic:stat_modifier}, its <see cref="Stats.ATTACK"/> rises by two stages.
		///
		/// If multiple stats are lowered at once, this ability takes effect with each stat lowered.
		/// </summary>
		DEFIANT = 128,
		/// <summary>
		/// Halves <see cref="Stats.ATTACK"/> and <see cref="Stats.SPATK"/> at 50% max <see cref="Stats.HP"/> or less.<para></para> This Pokémon's <see cref="Stats.ATTACK"/> and <see cref="Stats.SPATK"/> are halved when it has half its <see cref="Stats.HP"/> or less.
		/// </summary>
		DEFEATIST = 129,
		/// <summary>
		/// Has a 30% chance of [Disabling]{move:disable} any move that hits the Pokémon.<para></para> Moves that hit this Pokémon have a 30% chance of being [Disabled]{move:disable} afterward.
		/// </summary>
		CURSED_BODY = 130,
		/// <summary>
		/// Has a 30% chance of curing each adjacent ally of any [major status ailment]{mechanic:major_status_ailment} after each turn.<para></para> Friendly Pokémon next to this Pokémon in double and triple battles each have a 30% chance of being cured of any [major status ailment]{mechanic:major_status_ailment} after each turn.
		/// </summary>
		HEALER = 131,
		/// <summary>
		/// Decreases all direct damage taken by friendly Pokémon to 0.75×.<para></para> All friendly Pokémon take 0.75× as much direct damage from moves while this Pokémon is in battle.
		///
		/// This effect stacks if multiple allied Pokémon have it.
		/// </summary>
		FRIEND_GUARD = 132,
		/// <summary>
		/// Raises <see cref="Stats.SPEED"/> and lowers <see cref="Stats.DEFENSE"/> by one stage each upon being hit by a physical move.<para></para> Whenever a physical move hits this Pokémon, its <see cref="Stats.SPEED"/> rises one stage and its <see cref="Stats.DEFENSE"/> falls one stage.
		///
		/// This ability triggers on every hit of a multiple-hit move.
		/// </summary>
		WEAK_ARMOR = 133,
		/// <summary>
		/// Doubles the Pokémon's weight.<para></para> This Pokémon has double the usual weight for its species.
		/// </summary>
		HEAVY_METAL = 134,
		/// <summary>
		/// Halves the Pokémon's weight.<para></para> This Pokémon has half the usual weight for its species.
		/// </summary>
		LIGHT_METAL = 135,
		/// <summary>
		/// Halves damage taken from full <see cref="Stats.HP"/>.<para></para> This Pokémon takes half as much damage when it is hit having full <see cref="Stats.HP"/>.
		/// </summary>
		MULTISCALE = 136,
		/// <summary>
		/// Increases <see cref="Stats.ATTACK"/> to 1.5× when <see cref="Status.POISON"/>.<para></para> This Pokémon has 1.5× its <see cref="Stats.ATTACK"/> when <see cref="Status.POISON"/>.
		/// </summary>
		TOXIC_BOOST = 137,
		/// <summary>
		/// Increases <see cref="Stats.SPATK"/> to 1.5× when <see cref="Status.BURN"/>.<para></para> This Pokémon has 1.5× its <see cref="Stats.SPATK"/> when <see cref="Status.BURN"/>.
		/// </summary>
		FLARE_BOOST = 138,
		/// <summary>
		/// Has a 50% chance of restoring a used Berry after each turn if the Pokémon has held no items in the meantime.<para></para> After each turn, if the last item this Pokémon consumed was a Berry and it is not currently holding an item, it has a 50% chance of regaining that Berry, or a 100% chance during [strong sunlight]{mechanic:strong_sunlight}.
		/// </summary>
		HARVEST = 139,
		/// <summary>
		/// Protects against friendly Pokémon's damaging moves.<para></para> This Pokémon does not take damage from friendly Pokémon's moves, including single-target moves aimed at it.
		/// </summary>
		TELEPATHY = 140,
		/// <summary>
		/// Raises a random stat two [stages]{mechanic:stat_modifier} and lowers another one stage after each turn.<para></para> After each turn, one of this Pokémon's stats at random rises two [stages]{mechanic:stat_modifier}, and another falls one stage.
		///
		/// If a stat is already at 6 or -6 stages, it will not be chosen to be increased or decreased, respectively.
		/// </summary>
		MOODY = 141,
		/// <summary>
		/// Protects against damage from [weather]{mechanic:weather}.<para></para> This Pokémon does not take damage from [weather]{mechanic:weather}.
		/// </summary>
		OVERCOAT = 142,
		/// <summary>
		/// Has a 30% chance of <see cref="Status.POISON"/> target Pokémon upon contact.<para></para> This Pokémon's contact moves have a 30% chance of <see cref="Status.POISON"/> the target with each hit.
		///
		/// This counts as an extra effect for the purposes of <see cref="Abilities.SHIELD_DUST"/>. This ability takes effect before <see cref="Abilities.MUMMY"/>.
		/// </summary>
		POISON_TOUCH = 143,
		/// <summary>
		/// Heals for 1/3 max <see cref="Stats.HP"/> upon switching out.<para></para> This Pokémon regains 1/3 of its maximum <see cref="Stats.HP"/> when it is switched out for another Pokémon under any circumstances other than having fainted.
		///
		/// This ability does not take effect when a battle ends.
		/// </summary>
		REGENERATOR = 144,
		/// <summary>
		/// Protects against <see cref="Stats.DEFENSE"/> drops.<para></para> This Pokémon's <see cref="Stats.DEFENSE"/> cannot be [lowered]{mechanic:stat_modifiers} by other Pokémon.
		///
		/// This Pokémon can still be passed negative Defense modifiers through <see cref="Moves.HEART_SWAP"/> or <see cref="Moves.GUARD_SWAP"/>.
		/// </summary>
		BIG_PECKS = 145,
		/// <summary>
		/// Doubles <see cref="Stats.SPEED"/> during a <see cref="Weather.SANDSTORM"/>. Protects against sandstorm damage.<para></para> This Pokémon's <see cref="Stats.SPEED"/> is doubled during a <see cref="Weather.SANDSTORM"/>, and it does not take sandstorm damage, regardless of type.
		/// </summary>
		SAND_RUSH = 146,
		/// <summary>
		/// Lowers incoming non-damaging moves' base <see cref="Stats.ACCURACY"/> to exactly 50%.<para></para> Non-damaging moves have exactly 50% base <see cref="Stats.ACCURACY"/> against this Pokémon.
		/// </summary>
		WONDER_SKIN = 147,
		/// <summary>
		/// Strengthens moves to 1.3× their power when moving last.<para></para> This Pokémon's moves have 1.3× their power when it moves last in a turn.
		///
		/// <see cref="Moves.FUTURE_SIGHT"/> and <see cref="Moves.DOOM_DESIRE"/> are unaffected.
		/// </summary>
		ANALYTIC = 148,
		/// <summary>
		/// Takes the appearance of the last conscious party Pokémon upon being sent out until hit by a damaging move.<para></para> This Pokémon, upon being sent out, appears to have the species, nickname, and Poké Ball of the last Pokémon in the party that is able to battle. This illusion breaks upon being hit by a damaging move.
		///
		/// Other damage, e.g. from [weather]{mechanic:weather} or <see cref="Moves.SPIKES"/>, does not break the illusion, nor does damage done to a <see cref="Moves.SUBSTITUTE"/>.
		///
		/// If the party order becomes temporarily shuffled around as Pokémon are switched out in battle, this ability chooses the last Pokémon according to that shuffled order.
		/// </summary>
		ILLUSION = 149,
		/// <summary>
		/// [Transforms]{move:transform} upon entering battle.<para></para> This Pokémon transforms into a random opponent upon entering battle. This effect is identical to the move <see cref="Moves.TRANSFORM"/>.
		/// </summary>
		IMPOSTER = 150,
		/// <summary>
		/// Bypasses <see cref="Moves.LIGHT_SCREEN"/>, <see cref="Moves.REFLECT"/>, and <see cref="Moves.SAFEGUARD"/>.<para></para> This Pokémon's moves ignore <see cref="Moves.LIGHT_SCREEN"/>, <see cref="Moves.REFLECT"/>, and <see cref="Moves.SAFEGUARD"/>.
		/// </summary>
		INFILTRATOR = 151,
		/// <summary>
		/// Changes attacking Pokémon's abilities to Mummy on contact.<para></para> Whenever a contact move hits this Pokémon, the attacking Pokémon's ability changes to Mummy.
		///
		/// <see cref="Abilities.MULTITYPE"/> is unaffected. If a Pokémon with <see cref="Abilities.MOXIE"/> knocks this Pokémon out, the former's ability will change without taking effect.
		/// </summary>
		MUMMY = 152,
		/// <summary>
		/// Raises <see cref="Stats.ATTACK"/> one stage upon KOing a Pokémon.<para></para> This Pokémon's <see cref="Stats.ATTACK"/> rises one stage upon knocking out another Pokémon, even a friendly Pokémon.
		///
		/// This ability does not take effect when the Pokémon indirectly causes another Pokémon to faint, e.g. through <see cref="Status.POISON"/> or <see cref="Moves.SPIKES"/>.
		///
		/// If this Pokémon knocks out a Pokémon with <see cref="Abilities.MUMMY"/>, the former's ability will change without taking effect.
		/// </summary>
		MOXIE = 153,
		/// <summary>
		/// Raises <see cref="Stats.ATTACK"/> one stage upon taking damage from a <see cref="Types.DARK"/> move.<para></para> Whenever a <see cref="Types.DARK"/>-type move hits this Pokémon, its <see cref="Stats.ATTACK"/> rises one [stage]{mechanic:stat_modifier}.
		///
		/// The move is not negated in any way.
		/// </summary>
		JUSTIFIED = 154,
		/// <summary>
		/// Raises <see cref="Stats.SPEED"/> one [stage]{mechanic:stat_modifier} upon being hit by a <see cref="Types.DARK"/>, <see cref="Types.GHOST"/>, or <see cref="Types.BUG"/> move.<para></para> This Pokémon's <see cref="Stats.SPEED"/> rises one stage with each hit from a damaging <see cref="Types.DARK"/>-, <see cref="Types.GHOST"/>-, or <see cref="Types.BUG"/>-type move.
		/// </summary>
		RATTLED = 155,
		/// <summary>
		/// Reflects most non-damaging moves back at their user.<para></para> When this Pokémon is targeted by a move flagged as being reflectable, the move is redirected to its user.
		///
		/// All reflectable moves are non-damaging, and most non-damaging moves that target other Pokémon are reflectable.
		///
		/// A move reflected by this ability or <see cref="Moves.MAGIC_COAT"/> cannot be reflected back.
		/// </summary>
		MAGIC_BOUNCE = 156,
		/// <summary>
		/// Absorbs <see cref="Types.GRASS"/> moves, raising <see cref="Stats.ATTACK"/> one [stage]{mechanic:stat_modifier}.<para></para> Whenever a <see cref="Types.GRASS"/>-type move hits this Pokémon, its <see cref="Stats.ATTACK"/> rises one [stage]{mechanic:stat_modifier}, negating any other effect on it.
		/// </summary>
		SAP_SIPPER = 157,
		/// <summary>
		/// Raises non-damaging moves' priority by one stage.<para></para> This Pokémon's non-damaging moves have their priority increased by one stage.
		/// </summary>
		PRANKSTER = 158,
		/// <summary>
		/// Strengthens <see cref="Types.ROCK"/>, <see cref="Types.GROUND"/>, and <see cref="Types.STEEL"/> moves to 1.3× their power during a <see cref="Weather.SANDSTORM"/>. Protects against sandstorm damage.<para></para> During a <see cref="Weather.SANDSTORM"/>, this Pokémon's <see cref="Types.ROCK"/>-, <see cref="Types.GROUND"/>-, and <see cref="Types.STEEL"/>-type moves have 1.3× their base power. This Pokémon does not take sandstorm damage, regardless of type.
		/// </summary>
		SAND_FORCE = 159,
		/// <summary>
		/// Damages attacking Pokémon for 1/8 their max <see cref="Stats.HP"/> on contact.<para></para> Whenever a move makes contact with this Pokémon, the move's user takes 1/8 of its maximum <see cref="Stats.HP"/> in damage.
		///
		/// This ability functions identically to <see cref="Abilities.ROUGH_SKIN"/>.
		/// </summary>
		IRON_BARBS = 160,
		/// <summary>
		/// Changes <see cref="Pokemons.DARMANITAN"/>'s form after each turn depending on its <see cref="Stats.HP"/>: Zen Mode below 50% max HP, and Standard Mode otherwise.<para></para> This Pokémon switches between Standard Mode and Zen Mode after each turn depending on its <see cref="Stats.HP"/>. Below 50% of its maximum HP, it switches to Zen Mode, and at 50% or above, it switches to Standard Mode.
		///
		/// This Pokémon returns to Standard Mode upon leaving battle or losing this ability. This ability has no effect if this Pokémon is not a <see cref="Pokemons.DARMANITAN"/>.
		/// </summary>
		ZEN_MODE = 161,
		/// <summary>
		/// Increases moves' accuracy to 1.1× for friendly Pokémon.<para></para> All friendly Pokémon's moves, including this Pokémon's own moves, have 1.1× their usual accuracy while this Pokémon is in battle.
		/// </summary>
		VICTORY_STAR = 162,
		/// <summary>
		/// Bypasses targets' abilities if they could hinder or prevent moves.<para></para> This Pokémon's moves completely ignore abilities that could hinder or prevent their effect on the target.
		///
		/// For example, this Pokémon's moves ignore abilities that would fully negate them, such as <see cref="Abilities.WATER_ABSORB"/>; abilities that would prevent any of their effects, such as <see cref="Abilities.CLEAR_BODY"/>, <see cref="Abilities.SHELL_ARMOR"/>, or <see cref="Abilities.STICKY_HOLD"/>; and abilities that grant any general protective benefit, such as <see cref="Abilities.SIMPLE"/>, <see cref="Abilities.SNOW_CLOAK"/>, or <see cref="Abilities.THICK_FAT"/>. If an ability could either hinder or help this Pokémon's moves, e.g. <see cref="Abilities.DRY_SKIN"/> or <see cref="Abilities.UNAWARE"/>, the ability is ignored either way.
		///
		/// Abilities that do not fit this description, even if they could hinder moves in some other way, are not affected. For example, <see cref="Abilities.CURSED_BODY"/> only affects potential future uses of the move, while <see cref="Abilities.LIQUID_OOZE"/> and <see cref="Abilities.SHADOW_TAG"/> can only hinder a move's effect on the user. This ablity cannot ignore type or form changes granted by abilities, for example <see cref="Abilities.COLOR_CHANGE"/> or <see cref="Abilities.FORECAST"/>; nor effects that were caused by abilities but are no longer tied to an ability, such as the rain from <see cref="Abilities.DRIZZLE"/>. This ability cannot ignore <see cref="Abilities.MULTITYPE"/> at all.
		///
		/// An ability ignored by this ability is only nullified while the move is being used. For example, this Pokémon's moves can <see cref="Status.PARALYSIS"/> a Pokémon with <see cref="Abilities.LIMBER"/>, but Limber will activate and heal the paralysis immediately thereafter, and this Pokémon's <see cref="Moves.SPIKES"/> are not affected by this ability after they have been placed.
		///
		/// When this Pokémon enters battle, all participating trainers are notified that it has this ability.
		///
		/// This ability functions identically to <see cref="Abilities.MOLD_BREAKER"/> and <see cref="Abilities.TERAVOLT"/>.
		/// </summary>
		TURBOBLAZE = 163,
		/// <summary>
		/// Bypasses targets' abilities if they could hinder or prevent moves.<para></para> This Pokémon's moves completely ignore abilities that could hinder or prevent their effect on the target.
		///
		/// For example, this Pokémon's moves ignore abilities that would fully negate them, such as <see cref="Abilities.WATER_ABSORB"/>; abilities that would prevent any of their effects, such as <see cref="Abilities.CLEAR_BODY"/>, <see cref="Abilities.SHELL_ARMOR"/>, or <see cref="Abilities.STICKY_HOLD"/>; and abilities that grant any general protective benefit, such as <see cref="Abilities.SIMPLE"/>, <see cref="Abilities.SNOW_CLOAK"/>, or <see cref="Abilities.THICK_FAT"/>. If an ability could either hinder or help this Pokémon's moves, e.g. <see cref="Abilities.DRY_SKIN"/> or <see cref="Abilities.UNAWARE"/>, the ability is ignored either way.
		///
		/// Abilities that do not fit this description, even if they could hinder moves in some other way, are not affected. For example, <see cref="Abilities.CURSED_BODY"/> only affects potential future uses of the move, while <see cref="Abilities.LIQUID_OOZE"/> and <see cref="Abilities.SHADOW_TAG"/> can only hinder a move's effect on the user. This ablity cannot ignore type or form changes granted by abilities, for example <see cref="Abilities.COLOR_CHANGE"/> or <see cref="Abilities.FORECAST"/>; nor effects that were caused by abilities but are no longer tied to an ability, such as the rain from <see cref="Abilities.DRIZZLE"/>. This ability cannot ignore <see cref="Abilities.MULTITYPE"/> at all.
		///
		/// An ability ignored by this ability is only nullified while the move is being used. For example, this Pokémon's moves can <see cref="Status.PARALYSIS"/> a Pokémon with <see cref="Abilities.LIMBER"/>, but Limber will activate and heal the paralysis immediately thereafter, and this Pokémon's <see cref="Moves.SPIKES"/> are not affected by this ability after they have been placed.
		///
		/// When this Pokémon enters battle, all participating trainers are notified that it has this ability.
		///
		/// This ability functions identically to <see cref="Abilities.MOLD_BREAKER"/> and <see cref="Abilities.TURBOBLAZE"/>.
		/// </summary>
		TERAVOLT = 164,
		/// <summary>
		/// Protects allies against moves that affect their mental state.<para></para> Protects allies against moves that affect their mental state.
		/// </summary>
		AROMA_VEIL = 165,
		/// <summary>
		/// Protects friendly <see cref="Types.GRASS"/> Pokémon from having their stats lowered by other Pokémon.<para></para> Protects friendly <see cref="Types.GRASS"/> Pokémon from having their stats lowered by other Pokémon.
		/// </summary>
		FLOWER_VEIL = 166,
		/// <summary>
		/// Restores HP upon eating a Berry, in addition to the Berry's effect.<para></para> Restores HP upon eating a Berry, in addition to the Berry's effect.
		/// </summary>
		CHEEK_POUCH = 167,
		/// <summary>
		/// Changes the bearer's type to match each move it uses.<para></para> Changes the bearer's type to match each move it uses.
		///
		/// The type change takes place just before the move is used.
		/// </summary>
		PROTEAN = 168,
		/// <summary>
		/// Halves damage from physical attacks.<para></para> Halves damage from physical attacks.
		/// </summary>
		FUR_COAT = 169,
		/// <summary>
		/// Steals the target's held item when the bearer uses a damaging move.<para></para> Steals the target's held item when the bearer uses a damaging move.
		/// </summary>
		MAGICIAN = 170,
		/// <summary>
		/// Protects against bullet, ball, and bomb-based moves.<para></para> Protects against bullet, ball, and bomb-based moves.
		/// </summary>
		BULLETPROOF = 171,
		/// <summary>
		/// Raises Special Attack by two stages upon having any stat lowered.<para></para> Raises Special Attack by two stages upon having any stat lowered.
		/// </summary>
		COMPETITIVE = 172,
		/// <summary>
		/// Strengthens biting moves to 1.5× their power.<para></para> Strengthens biting moves to 1.5× their power.
		/// </summary>
		STRONG_JAW = 173,
		/// <summary>
		/// Turns the bearer's <see cref="Types.NORMAL"/> moves into <see cref="Types.ICE"/> moves and strengthens them to 1.3× their power.<para></para> Turns the bearer's <see cref="Types.NORMAL"/>-type moves into <see cref="Types.ICE"/>-type moves. Moves changed by this ability have 1.3× their power.
		/// </summary>
		REFRIGERATE = 174,
		/// <summary>
		/// Prevents friendly Pokémon from sleeping.<para></para> Prevents friendly Pokémon from sleeping.
		/// </summary>
		SWEET_VEIL = 175,
		/// <summary>
		/// Changes <see cref="Pokemons.AEGISLASH"/> to Blade Forme before using a damaging move, or Shield Forme before using <see cref="Moves.KINGS_SHIELD"/>.<para></para> Changes <see cref="Pokemons.AEGISLASH"/> to Blade Forme before using a damaging move, or Shield Forme before using <see cref="Moves.KINGS_SHIELD"/>.
		/// </summary>
		STANCE_CHANGE = 176,
		/// <summary>
		/// Raises <see cref="Types.FLYING"/> moves' priority by one stage.<para></para> Raises <see cref="Types.FLYING"/> moves' priority by one stage.
		/// </summary>
		GALE_WINGS = 177,
		/// <summary>
		/// Strengthens aura and pulse moves to 1.5× their power.<para></para> Strengthens aura and pulse moves to 1.5× their power.
		/// </summary>
		MEGA_LAUNCHER = 178,
		/// <summary>
		/// Boosts Defense while <see cref="Moves.GRASSY_TERRAIN"/> is in effect.<para></para> Boosts Defense while <see cref="Moves.GRASSY_TERRAIN"/> is in effect.
		/// </summary>
		GRASS_PELT = 179,
		/// <summary>
		/// Passes the bearer's held item to an ally when the ally uses up its item.<para></para> Passes the bearer's held item to an ally when the ally uses up its item.
		/// </summary>
		SYMBIOSIS = 180,
		/// <summary>
		/// Strengthens moves that make contact to 1.33× their power.<para></para> Strengthens moves that make contact to 1.33× their power.
		/// </summary>
		TOUGH_CLAWS = 181,
		/// <summary>
		/// Turns the bearer's <see cref="Types.NORMAL"/> moves into <see cref="Types.FAIRY"/> moves and strengthens them to 1.3× their power.<para></para> Turns the bearer's <see cref="Types.NORMAL"/>-type moves into <see cref="Types.FAIRY"/> moves. Moves changed by this ability have 1.3× their power.
		/// </summary>
		PIXILATE = 182,
		/// <summary>
		/// Lowers attacking Pokémon's Speed by one stage on contact.<para></para> Lowers attacking Pokémon's Speed by one stage on contact.
		/// </summary>
		GOOEY = 183,
		/// <summary>
		/// Turns the bearer's <see cref="Types.NORMAL"/> moves into <see cref="Types.FLYING"/> moves and strengthens them to 1.3× their power.<para></para> Turns the bearer's <see cref="Types.NORMAL"/>-type moves into <see cref="Types.FLYING"/>-type moves. Moves changed by this ability have 1.3× their power.
		/// </summary>
		AERILATE = 184,
		/// <summary>
		/// Lets the bearer hit twice with damaging moves. The second hit has half power.<para></para> Lets the bearer hit twice with damaging moves. The second hit has half power.
		/// </summary>
		PARENTAL_BOND = 185,
		/// <summary>
		/// Strengthens <see cref="Types.DARK"/> moves to 1.33× their power for all friendly and opposing Pokémon.<para></para> Strengthens <see cref="Types.DARK"/> moves for all friendly and opposing Pokémon.
		/// </summary>
		DARK_AURA = 186,
		/// <summary>
		/// Strengthens <see cref="Types.FAIRY"/> moves to 1.33× their power for all friendly and opposing Pokémon.<para></para> Strengthens <see cref="Types.FAIRY"/> moves for all friendly and opposing Pokémon.
		/// </summary>
		FAIRY_AURA = 187,
		/// <summary>
		/// Makes <see cref="Abilities.DARK_AURA"/> and <see cref="Abilities.FAIRY_AURA"/> weaken moves of their respective types.<para></para> While this Pokémon is on the field, <see cref="Abilities.DARK_AURA"/> and <see cref="Abilities.FAIRY_AURA"/> weaken moves of their respective types to 2/3 their power, rather than strengthening them.
		/// </summary>
		AURA_BREAK = 188,
		/// <summary>
		/// Creates heavy rain, which has all the properties of Rain Dance, cannot be replaced, and causes damaging Fire moves to fail.<para></para> When this Pokémon enters battle or gains this ability, the weather becomes heavy rain. Heavy rain has all the properties of <see cref="Moves.RAIN_DANCE"/> and also causes damaging Fire moves to fail.
		///
		/// Heavy rain ends when this Pokémon leaves battle or loses this ability, or when this ability is nullified. The weather cannot otherwise be changed except by the effects of <see cref="Abilities.DELTA_STREAM"/> and <see cref="Abilities.DESOLATE_LAND"/>.
		///
		/// <see cref="Abilities.AIR_LOCK"/> and <see cref="Abilities.CLOUD_NINE"/> will prevent the effects of heavy rain, including allowing Fire moves to work, but will not allow the weather to be changed.
		/// </summary>
		PRIMORDIAL_SEA = 189,
		/// <summary>
		/// Creates extremely harsh sunlight, which has all the properties of Sunny Day, cannot be replaced, and causes damaging Water moves to fail.<para></para> When this Pokémon enters battle or gains this ability, the weather becomes extremely harsh sunlight. Extremely harsh sunlight has all the properties of <see cref="Moves.SUNNY_DAY"/> and also causes damaging Water moves to fail.
		///
		/// Extremely harsh sunlight ends when this Pokémon leaves battle or loses this ability, or when this ability is nullified. The weather cannot otherwise be changed except by the effects of <see cref="Abilities.DELTA_STREAM"/> and <see cref="Abilities.PRIMORDIAL_SEA"/>.
		///
		/// <see cref="Abilities.AIR_LOCK"/> and <see cref="Abilities.CLOUD_NINE"/> will prevent the effects of extremely harsh sunlight, including allowing Water moves to work, but will not allow the weather to be changed.
		/// </summary>
		DESOLATE_LAND = 190,
		/// <summary>
		/// Creates a mysterious air current, which cannot be replaced and causes moves to never be super effective against Flying Pokémon.<para></para> When this Pokémon enters battle or gains this ability, the weather becomes a mysterious air current. A mysterious air current causes moves to not be super effective against Flying; they do neutral damage instead.  <see cref="Abilities.ANTICIPATION"/> and <see cref="Moves.STEALTH_ROCK"/> are not affected.
		///
		/// The mysterious air current ends when this Pokémon leaves battle or loses this ability, or when this ability is nullified. The weather cannot otherwise be changed except by the effects of <see cref="Abilities.DESOLATE_LAND"/> and <see cref="Abilities.PRIMORDIAL_SEA"/>.
		///
		/// <see cref="Abilities.AIR_LOCK"/> and <see cref="Abilities.CLOUD_NINE"/> will prevent the effect of a mysterious air current, but will not allow the weather to be changed.
		/// </summary>
		DELTA_STREAM = 191,
		/// <summary>
		/// Raises this Pokémon's Defense by one stage when it takes damage from a move.<para></para> Raises this Pokémon's Defense by one stage when it takes damage from a move.
		/// </summary>
		STAMINA = 192,
		/// <summary>
		/// This Pokémon automatically switches out when its HP drops below half.<para></para> After this Pokémon is hit by a move, if that move caused this Pokémon's HP to drop below half, it switches out.
		/// </summary>
		WIMP_OUT = 193,
		/// <summary>
		/// This Pokémon automatically switches out when its HP drops below half.<para></para> After this Pokémon is hit by a move, if that move caused this Pokémon's HP to drop below half, it switches out.
		/// </summary>
		EMERGENCY_EXIT = 194,
		/// <summary>
		/// Raises this Pokémon's Defense by two stages when it's hit by a Water move.<para></para> Raises this Pokémon's Defense by two stages when it's hit by a Water move.
		/// </summary>
		WATER_COMPACTION = 195,
		/// <summary>
		/// This Pokémon's moves critical hit against poisoned targets.<para></para> This Pokémon's moves critical hit against poisoned targets.
		/// </summary>
		MERCILESS = 196,
		/// <summary>
		/// Transforms this Minior between Core Form and Meteor Form. Prevents major status ailments and drowsiness while in Meteor Form.<para></para> When this Pokémon enters battle and at the end of each turn, if its HP is 50% or above, it changes into Meteor Form; otherwise, it changes into Core Form. In Meteor Form, it cannot be given a major status ailment (though existing ones are not cured), cannot become drowsy from <see cref="Moves.YAWN"/>, and cannot use <see cref="Moves.REST"/> (which will simply fail).
		///
		/// This ability cannot be copied, replaced, or nullified. This ability only takes effect for <see cref="Pokemons.MINIOR_RED_METEOR"/>.
		/// </summary>
		SHIELDS_DOWN = 197,
		/// <summary>
		/// This Pokémon's moves have double power against Pokémon that switched in this turn.<para></para> This Pokémon's moves have double power against Pokémon that switched in this turn.
		/// </summary>
		STAKEOUT = 198,
		/// <summary>
		/// Halves damage from Fire moves, doubles damage of Water moves, and prevents burns.<para></para> When this Pokémon is hit by a Fire move, the damage is halved. When this Pokémon uses a Water move, the power is doubled. This Pokémon cannot be burned, and if it becomes burned, the burn is immediately ured.
		/// </summary>
		WATER_BUBBLE = 199,
		/// <summary>
		/// This Pokémon's Steel moves have 1.5× power.<para></para> This Pokémon's Steel moves have 1.5× power.
		/// </summary>
		STEELWORKER = 200,
		/// <summary>
		/// Raises this Pokémon's Special Attack by one stage every time its HP drops below half.<para></para> Whenever this Pokémon takes damage from a move that causes its HP to drop below 50%, its Special Attack rises by one stage.
		/// </summary>
		BERSERK = 201,
		/// <summary>
		/// During Hail, this Pokémon has double Speed.<para></para> During Hail, this Pokémon has double Speed.
		/// </summary>
		SLUSH_RUSH = 202,
		/// <summary>
		/// This Pokémon's moves do not make contact.<para></para> A move used by this Pokémon will not make contact.
		/// </summary>
		LONG_REACH = 203,
		/// <summary>
		/// Sound-based moves become Water-type.<para></para> When this Pokémon uses a move that is sound-based, that move's type is Water.
		/// </summary>
		LIQUID_VOICE = 204,
		/// <summary>
		/// This Pokémon's healing moves have their priority increased by 3.<para></para> This Pokémon's healing moves have their priority increased by 3.
		/// </summary>
		TRIAGE = 205,
		/// <summary>
		/// This Pokémon's Normal moves are Electric and have their power increased to 1.2×.<para></para> When this Pokémon uses a Normal moves, that move is Electric its power is 1.2×.
		/// </summary>
		GALVANIZE = 206,
		/// <summary>
		/// Doubles this Pokémon's Speed on Electric Terrain.<para></para> Doubles this Pokémon's Speed on Electric Terrain.
		/// </summary>
		SURGE_SURFER = 207,
		/// <summary>
		/// Wishiwashi becomes Schooling Form when its HP is 25% or higher.<para></para> If this Pokémon is a <see cref="Pokemons.WISHIWASHI_SOLO"/> and level 20 or above, then when it enters battle and at the start of each turn, it becomes Schooling Form if its HP is 25% or higher and Solo Form otherwise.
		///
		/// This ability cannot be replaced, copied, or nullified.
		/// </summary>
		SCHOOLING = 208,
		/// <summary>
		/// Prevents the first instance of battle damage.<para></para> If this Pokémon is in its Disguised Form and takes damage from a move, it switches to its Busted Form and the damage is prevented. Other effects are not prevented.
		///
		/// This ability cannot be copied or replaced. This ability only takes effect for <see cref="Pokemons.MIMIKYU_DISGUISED"/>.
		/// </summary>
		DISGUISE = 209,
		/// <summary>
		/// Transforms this Pokémon into Ash-Greninja after fainting an opponent. Water Shuriken's power is 20 and always hits three times.<para></para> Transforms this Pokémon into Ash-Greninja after fainting an opponent. Water Shuriken's power is 20 and always hits three times.
		///
		/// This ability cannot be copied or replaced. This ability only takes effect for <see cref="Pokemons.GRENINJA"/>.
		/// </summary>
		BATTLE_BOND = 210,
		/// <summary>
		/// Transforms 10% or 50% Zygarde into Complete Forme when its HP is below 50%.<para></para> Transforms 10% or 50% Zygarde into Complete Forme when its HP is below 50%.
		///
		/// This ability cannot be copied or replaced. This ability only takes effect for <see cref="Pokemons.ZYGARDE"/>.
		/// </summary>
		POWER_CONSTRUCT = 211,
		/// <summary>
		/// This Pokémon can inflict poison on Poison and Steel Pokémon.<para></para> This Pokémon's moves and item ignore the usual immunity of Poison and Steel Pokémon when attempting to inflict poison.
		/// </summary>
		CORROSION = 212,
		/// <summary>
		/// This Pokémon always acts as though it were Asleep.<para></para> This Pokémon always acts as though it were Asleep. It cannot be given another status ailment; it's unaffected by <see cref="Moves.YAWN"/>; it can use <see cref="Moves.SLEEP_TALK"/>; and so on.
		/// </summary>
		COMATOSE = 213,
		/// <summary>
		/// Opposing Pokémon cannot use priority attacks.<para></para> When an opposing Pokémon attempts to use a move that targets this Pokémon or an ally, and that move has priority, it will fail.
		/// </summary>
		QUEENLY_MAJESTY = 214,
		/// <summary>
		/// When this Pokémon faints from an opponent's move, that opponent takes damage equal to the HP this Pokémon had remaining.<para></para> When this Pokémon faints from an opponent's move, that opponent takes damage equal to the HP this Pokémon had remaining.
		/// </summary>
		INNARDS_OUT = 215,
		/// <summary>
		/// Whenever another Pokémon uses a dance move, this Pokémon will use the same move immediately afterwards.<para></para> Whenever another Pokémon uses a dance move, this Pokémon will use the same move immediately afterwards.
		/// </summary>
		DANCER = 216,
		/// <summary>
		/// Ally Pokémon's moves have their power increased to 1.3×.<para></para> Ally Pokémon's moves have their power increased to 1.3×.
		/// </summary>
		BATTERY = 217,
		/// <summary>
		/// Damage from contact moves is halved. Damage from Fire moves is doubled.<para></para> Damage from contact moves is halved. Damage from Fire moves is doubled.
		/// </summary>
		FLUFFY = 218,
		/// <summary>
		/// Opposing Pokémon cannot use priority attacks.<para></para> When an opposing Pokémon attempts to use a move that targets this Pokémon or an ally, and that move has priority, it will fail.
		/// </summary>
		DAZZLING = 219,
		/// <summary>
		/// This Pokémon's Special Attack rises by one stage every time any Pokémon faints.<para></para> This Pokémon's Special Attack rises by one stage every time any Pokémon faints.
		/// </summary>
		SOUL_HEART = 220,
		/// <summary>
		/// When this Pokémon takes regular damage from a contact move, the attacking Pokémon's Speed lowers by one stage.<para></para> When this Pokémon takes regular damage from a contact move, the attacking Pokémon's Speed lowers by one stage.
		/// </summary>
		TANGLING_HAIR = 221,
		/// <summary>
		/// When an ally faints, this Pokémon gains its Ability.<para></para> When an ally faints, this Pokémon gains its Ability.
		/// </summary>
		RECEIVER = 222,
		/// <summary>
		/// When an ally faints, this Pokémon gains its Ability.<para></para> When an ally faints, this Pokémon gains its Ability.
		/// </summary>
		POWER_OF_ALCHEMY = 223,
		/// <summary>
		/// Raises this Pokémon's highest stat by one stage when it faints another Pokémon.<para></para> Raises this Pokémon's highest stat by one stage when it faints another Pokémon.
		/// </summary>
		BEAST_BOOST = 224,
		/// <summary>
		/// Changes this Pokémon's type to match its held Memory.<para></para> Changes this Pokémon's type to match its held Memory.
		///
		/// This ability cannot be copied, replaced, or nullified. This ability only takes effect for <see cref="Pokemons.SILVALLY"/>.
		/// </summary>
		RKS_SYSTEM = 225,
		/// <summary>
		/// When this Pokémon enters battle, it changes the terrain to Electric Terrain.<para></para> When this Pokémon enters battle, it changes the terrain to <see cref="Moves.ELECTRIC_TERRAIN"/>.
		/// </summary>
		ELECTRIC_SURGE = 226,
		/// <summary>
		/// When this Pokémon enters battle, it changes the terrain to Psychic Terrain.<para></para> When this Pokémon enters battle, it changes the terrain to <see cref="Moves.PSYCHIC_TERRAIN"/>.
		/// </summary>
		PSYCHIC_SURGE = 227,
		/// <summary>
		/// When this Pokémon enters battle, it changes the terrain to Misty Terrain.<para></para> When this Pokémon enters battle, it changes the terrain to <see cref="Moves.MISTY_TERRAIN"/>.
		/// </summary>
		MISTY_SURGE = 228,
		/// <summary>
		/// When this Pokémon enters battle, it changes the terrain to Grassy Terrain.<para></para> When this Pokémon enters battle, it changes the terrain to <see cref="Moves.GRASSY_TERRAIN"/>.
		/// </summary>
		GRASSY_SURGE = 229,
		/// <summary>
		/// Other Pokémon cannot lower this Pokémon's stats.<para></para> This Pokémon's stats cannot be lowered by other Pokémon's moves or abilities. This effect only applies to normal stat modifications and not more exotic effects such as <see cref="Moves.TOPSY_TURVY"/> or <see cref="Moves.POWER_SWAP"/>.
		///
		/// This Ability is not bypassed by <see cref="Abilities.MOLD_BREAKER"/>, <see cref="Abilities.TERAVOLT"/>, or <see cref="Abilities.TURBOBLAZE"/>.
		/// </summary>
		FULL_METAL_BODY = 230,
		/// <summary>
		/// When this Pokémon has full HP, regular damage from moves is halved.<para></para> When this Pokémon has full HP, regular damage (not fixed damage!) from moves is halved.
		///
		/// This ability cannot be nullified.
		/// </summary>
		SHADOW_SHIELD = 231,
		/// <summary>
		/// Reduces super-effective damage to 0.75×.<para></para> Super-effective damage this Pokémon takes is reduced to 0.75×.
		///
		/// This Ability is not bypassed by <see cref="Abilities.MOLD_BREAKER"/>, <see cref="Abilities.TERAVOLT"/>, or <see cref="Abilities.TURBOBLAZE"/>.
		/// </summary>
		PRISM_ARMOR = 232,
		/// <summary>
		/// XXX new effect for neuroforce<para></para> XXX new effect for neuroforce
		/// </summary>
		NEUROFORCE = 233
	}
	#endregion
	#region
	public enum Ribbons
	{
		NONE = 0,
		HOENNCOOL = 1,
		HOENNCOOLSUPER = 2,
		HOENNCOOLHYPER = 3,
		HOENNCOOLMASTER = 4,
		HOENNBEAUTY = 5,
		HOENNBEAUTYSUPER = 6,
		HOENNBEAUTYHYPER = 7,
		HOENNBEAUTYMASTER = 8,
		HOENNCUTE = 9,
		HOENNCUTESUPER = 10,
		HOENNCUTEHYPER = 11,
		HOENNCUTEMASTER = 12,
		HOENNSMART = 13,
		HOENNSMARTSUPER = 14,
		HOENNSMARTHYPER = 15,
		HOENNSMARTMASTER = 16,
		HOENNTOUGH = 17,
		HOENNTOUGHSUPER = 18,
		HOENNTOUGHHYPER = 19,
		HOENNTOUGHMASTER = 20,
		SINNOHCOOL = 21,
		SINNOHCOOLSUPER = 22,
		SINNOHCOOLHYPER = 23,
		SINNOHCOOLMASTER = 24,
		SINNOHBEAUTY = 25,
		SINNOHBEAUTYSUPER = 26,
		SINNOHBEAUTYHYPER = 27,
		SINNOHBEAUTYMASTER = 28,
		SINNOHCUTE = 29,
		SINNOHCUTESUPER = 30,
		SINNOHCUTEHYPER = 31,
		SINNOHCUTEMASTER = 32,
		SINNOHSMART = 33,
		SINNOHSMARTSUPER = 34,
		SINNOHSMARTHYPER = 35,
		SINNOHSMARTMASTER = 36,
		SINNOHTOUGH = 37,
		SINNOHTOUGHSUPER = 38,
		SINNOHTOUGHHYPER = 39,
		SINNOHTOUGHMASTER = 40,
		WINNING = 41,
		VICTORY = 42,
		ABILITY = 43,
		GREATABILITY = 44,
		DOUBLEABILITY = 45,
		MULTIABILITY = 46,
		PAIRABILITY = 47,
		WORLDABILITY = 48,
		CHAMPION = 49,
		SINNOHCHAMP = 50,
		RECORD = 51,
		EVENT = 52,
		LEGEND = 53,
		GORGEOUS = 54,
		ROYAL = 55,
		GORGEOUSROYAL = 56,
		ALERT = 57,
		SHOCK = 58,
		DOWNCAST = 59,
		CARELESS = 60,
		RELAX = 61,
		SNOOZE = 62,
		SMILE = 63,
		FOOTPRINT = 64,
		ARTIST = 65,
		EFFORT = 66,
		BIRTHDAY = 67,
		SPECIAL = 68,
		CLASSIC = 69,
		PREMIER = 70,
		SOUVENIR = 71,
		WISHING = 72,
		NATIONAL = 73,
		COUNTRY = 74,
		BATTLECHAMPION = 75,
		REGIONALCHAMPION = 76,
		EARTH = 77,
		WORLD = 78,
		NATIONALCHAMPION = 79,
		WORLDCHAMPION = 80
	}
	public enum TrainerTypes
	{
		WildPokemon = -1,
		/// <summary>
		/// Custom designs or just generic character played by another user
		/// </summary>
		PLAYER = 0,
		//PLAYER_M,
		//PLAYER_F,
		POKEMONTRAINER_Red,
		POKEMONTRAINER_Leaf,
		POKEMONTRAINER_Brendan,
		POKEMONTRAINER_May,
		RIVAL1,
		RIVAL2,
		AROMALADY,
		BEAUTY,
		BIKER,
		BIRDKEEPER,
		BUGCATCHER,
		BURGLAR,
		CHANELLER,
		CUEBALL,
		ENGINEER,
		FISHERMAN,
		GAMBLER,
		GENTLEMAN,
		HIKER,
		JUGGLER,
		LADY,
		PAINTER,
		POKEMANIAC,
		POKEMONBREEDER,
		PROFESSOR,
		ROCKER,
		RUINMANIAC,
		SAILOR,
		SCIENTIST,
		SUPERNERD,
		TAMER,
		BLACKBELT,
		CRUSHGIRL,
		CAMPER,
		PICNICKER,
		COOLTRAINER_M,
		COOLTRAINER_F,
		YOUNGSTER,
		LASS,
		POKEMONRANGER_M,
		POKEMONRANGER_F,
		PSYCHIC_M,
		PSYCHIC_F,
		SWIMMER_M,
		SWIMMER_F,
		SWIMMER2_M,
		SWIMMER2_F,
		TUBER_M,
		TUBER_F,
		TUBER2_M,
		TUBER2_F,
		COOLCOUPLE,
		CRUSHKIN,
		SISANDBRO,
		TWINS,
		YOUNGCOUPLE,
		TEAMROCKET_M,
		TEAMROCKET_F,
		ROCKETBOSS,
		LEADER_Brock,
		LEADER_Misty,
		LEADER_Surge,
		LEADER_Erika,
		LEADER_Koga,
		LEADER_Sabrina,
		LEADER_Blaine,
		LEADER_Giovanni,
		ELITEFOUR_Lorelei,
		ELITEFOUR_Bruno,
		ELITEFOUR_Agatha,
		ELITEFOUR_Lance,
		CHAMPION
	}
	public enum Colors
	{
		RED = 8,
		BLUE = 2,
		YELLOW = 10,
		GREEN = 5,
		BLACK = 1,
		BROWN = 3,
		PURPLE = 7,
		GRAY = 4,
		WHITE = 9,
		PINK = 6,
		NONE = 0
	};
	public enum HappinessMethods
	{
		WALKING,
		LEVELUP,
		GROOM,
		FAINT,
		VITAMIN,
		EVBERRY,
		POWDER,
		ENERGYROOT,
		REVIVALHERB,
		FRIENDBALL
	}
	#endregion
	#region Game Variables
	/// <summary>
	/// Still need to sort out Language Enums
	/// </summary>
	public enum Languages
	{
		/// <summary>
		/// US English
		/// </summary>
		English = 9
	}
	/// <summary>
	/// Scripted Text stored and available for Locale translations
	/// </summary>
	public enum TextScripts
	{
		Name,
		NamePlural,
		Description
	}
	/// <summary>
	/// Different category sections a piece of translated text could belong to
	/// </summary>
	public enum TextLocalization
	{
		Species           = 1 ,
		Kinds             = 2 ,
		Entries           = 3 ,
		FormNames         = 4 ,
		Moves             = 5 ,
		MoveDescriptions  = 6 ,
		Items             = 7 ,
		ItemPlurals       = 8 ,
		ItemDescriptions  = 9 ,
		Abilities         = 10,
		AbilityDescs      = 11,
		Types             = 12,
		TrainerTypes      = 13,
		TrainerNames      = 14,
		BeginSpeech       = 15,
		EndSpeechWin      = 16,
		EndSpeechLose     = 17,
		RegionNames       = 18,
		PlaceNames        = 19,
		PlaceDescriptions = 20,
		MapNames          = 21,
		PhoneMessages     = 22,
		ScriptTexts       = 23,
		Errors			  = 24
	}
	public enum Generation
	{
		All = 0,
		RedBlueYellow = 1,
		GoldSilverCrystal = 2,
		RubySapphireEmerald = 3,
		//FireRedLeafGreen,
		DiamondPearlPlatinum = 4,
		//HeartgoldSoulsilver,
		BlackWhite = 5,
		//BlackWhite2,
		XY = 6,
		//OmegaRubyAlphaSapphire,
		SunMoon = 7,
		//UltraSunUltraMoon
		Custom = -1
	}
	/// <summary>
	/// </summary>
	/// Use value here to represent `version_group_id` in Veekun's DB
	public enum Versions
	{
		PKUE = 0
		, RED_BLUE					= 1
		, YELLOW					= 2
		, GOLD_SILVER				= 3
		, CRYSTAL					= 4
		, RUBY_SAPPHIRE				= 5
		, EMERALD					= 6
		, FIRERED_LEAFGREEN			= 7
		, DIAMOND_PEARL				= 8
		, PLATINUM					= 9
		, HEARTGOLD_SOULSILVER		= 10
		, BLACK_WHITE				= 11
		, COLOSSEUM					= 12
		, XD						= 13
		, BLACK_2_WHITE_2			= 14
		, X_Y						= 15
		, OMEGA_RUBY_ALPHA_SAPPHIRE	= 16
		, SUN_MOON					//=
		, ULTRA_SUN_ULTRA_MOON		//=
	}
	#endregion
	#region Move
	public enum Status
	{
		NONE,
		SLEEP,
		POISON,
		PARALYSIS,
		BURN,
		FROZEN,
		FAINT
	}
	/// <summary>
	/// Move ids are connected to XML file.
	/// </summary>
	/// <remarks>Can now code with strings or int and
	/// access the same value.</remarks>
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
		//Everythng beyond here is new; used by non-canonical pokemon game
		/*BREAKNECK_BLITZ__PHYSICAL = 622,
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
		CLANGOROUS_SOULBLAZE = 728,*/
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
	#endregion
	#region Pokemon
	public enum Contests { Cool, Beauty, Cute, Smart, Tough, Sheen }
	/// <summary>
	/// Pokemon ids are connected to XML file.
	/// </summary>
	/// <remarks>Can now code with strings or int and
	/// access the same value.</remarks>
	public enum Pokemons
	{
		NONE = 0,
		BULBASAUR = 1,
		IVYSAUR = 2,
		VENUSAUR = 3,
		CHARMANDER = 4,
		CHARMELEON = 5,
		CHARIZARD = 6,
		SQUIRTLE = 7,
		WARTORTLE = 8,
		BLASTOISE = 9,
		CATERPIE = 10,
		METAPOD = 11,
		BUTTERFREE = 12,
		WEEDLE = 13,
		KAKUNA = 14,
		BEEDRILL = 15,
		PIDGEY = 16,
		PIDGEOTTO = 17,
		PIDGEOT = 18,
		RATTATA = 19,
		RATICATE = 20,
		SPEAROW = 21,
		FEAROW = 22,
		EKANS = 23,
		ARBOK = 24,
		PIKACHU = 25,
		RAICHU = 26,
		SANDSHREW = 27,
		SANDSLASH = 28,
		/// <SUMMARY>
		/// NIDORANFEMALE
		/// </SUMMARY>
		NIDORAN_F = 29,
		NIDORINA = 30,
		NIDOQUEEN = 31,
		/// <SUMMARY>
		/// NIDORANMALE
		/// </SUMMARY>
		NIDORAN_M = 32,
		NIDORINO = 33,
		NIDOKING = 34,
		CLEFAIRY = 35,
		CLEFABLE = 36,
		VULPIX = 37,
		NINETALES = 38,
		JIGGLYPUFF = 39,
		WIGGLYTUFF = 40,
		ZUBAT = 41,
		GOLBAT = 42,
		ODDISH = 43,
		GLOOM = 44,
		VILEPLUME = 45,
		PARAS = 46,
		PARASECT = 47,
		VENONAT = 48,
		VENOMOTH = 49,
		DIGLETT = 50,
		DUGTRIO = 51,
		MEOWTH = 52,
		PERSIAN = 53,
		PSYDUCK = 54,
		GOLDUCK = 55,
		MANKEY = 56,
		PRIMEAPE = 57,
		GROWLITHE = 58,
		ARCANINE = 59,
		POLIWAG = 60,
		POLIWHIRL = 61,
		POLIWRATH = 62,
		ABRA = 63,
		KADABRA = 64,
		ALAKAZAM = 65,
		MACHOP = 66,
		MACHOKE = 67,
		MACHAMP = 68,
		BELLSPROUT = 69,
		WEEPINBELL = 70,
		VICTREEBEL = 71,
		TENTACOOL = 72,
		TENTACRUEL = 73,
		GEODUDE = 74,
		GRAVELER = 75,
		GOLEM = 76,
		PONYTA = 77,
		RAPIDASH = 78,
		SLOWPOKE = 79,
		SLOWBRO = 80,
		MAGNEMITE = 81,
		MAGNETON = 82,
		FARFETCHD = 83,
		DODUO = 84,
		DODRIO = 85,
		SEEL = 86,
		DEWGONG = 87,
		GRIMER = 88,
		MUK = 89,
		SHELLDER = 90,
		CLOYSTER = 91,
		GASTLY = 92,
		HAUNTER = 93,
		GENGAR = 94,
		ONIX = 95,
		DROWZEE = 96,
		HYPNO = 97,
		KRABBY = 98,
		KINGLER = 99,
		VOLTORB = 100,
		ELECTRODE = 101,
		EXEGGCUTE = 102,
		EXEGGUTOR = 103,
		CUBONE = 104,
		MAROWAK = 105,
		HITMONLEE = 106,
		HITMONCHAN = 107,
		LICKITUNG = 108,
		KOFFING = 109,
		WEEZING = 110,
		RHYHORN = 111,
		RHYDON = 112,
		CHANSEY = 113,
		TANGELA = 114,
		KANGASKHAN = 115,
		HORSEA = 116,
		SEADRA = 117,
		GOLDEEN = 118,
		SEAKING = 119,
		STARYU = 120,
		STARMIE = 121,
		MR_MIME = 122,
		SCYTHER = 123,
		JYNX = 124,
		ELECTABUZZ = 125,
		MAGMAR = 126,
		PINSIR = 127,
		TAUROS = 128,
		MAGIKARP = 129,
		GYARADOS = 130,
		LAPRAS = 131,
		DITTO = 132,
		EEVEE = 133,
		VAPOREON = 134,
		JOLTEON = 135,
		FLAREON = 136,
		PORYGON = 137,
		OMANYTE = 138,
		OMASTAR = 139,
		KABUTO = 140,
		KABUTOPS = 141,
		AERODACTYL = 142,
		SNORLAX = 143,
		ARTICUNO = 144,
		ZAPDOS = 145,
		MOLTRES = 146,
		DRATINI = 147,
		DRAGONAIR = 148,
		DRAGONITE = 149,
		MEWTWO = 150,
		MEW = 151,
		CHIKORITA = 152,
		BAYLEEF = 153,
		MEGANIUM = 154,
		CYNDAQUIL = 155,
		QUILAVA = 156,
		TYPHLOSION = 157,
		TOTODILE = 158,
		CROCONAW = 159,
		FERALIGATR = 160,
		SENTRET = 161,
		FURRET = 162,
		HOOTHOOT = 163,
		NOCTOWL = 164,
		LEDYBA = 165,
		LEDIAN = 166,
		SPINARAK = 167,
		ARIADOS = 168,
		CROBAT = 169,
		CHINCHOU = 170,
		LANTURN = 171,
		PICHU = 172,
		CLEFFA = 173,
		IGGLYBUFF = 174,
		TOGEPI = 175,
		TOGETIC = 176,
		NATU = 177,
		XATU = 178,
		MAREEP = 179,
		FLAAFFY = 180,
		AMPHAROS = 181,
		BELLOSSOM = 182,
		MARILL = 183,
		AZUMARILL = 184,
		SUDOWOODO = 185,
		POLITOED = 186,
		HOPPIP = 187,
		SKIPLOOM = 188,
		JUMPLUFF = 189,
		AIPOM = 190,
		SUNKERN = 191,
		SUNFLORA = 192,
		YANMA = 193,
		WOOPER = 194,
		QUAGSIRE = 195,
		ESPEON = 196,
		UMBREON = 197,
		MURKROW = 198,
		SLOWKING = 199,
		MISDREAVUS = 200,
		UNOWN = 201,
		WOBBUFFET = 202,
		GIRAFARIG = 203,
		PINECO = 204,
		FORRETRESS = 205,
		DUNSPARCE = 206,
		GLIGAR = 207,
		STEELIX = 208,
		SNUBBULL = 209,
		GRANBULL = 210,
		QWILFISH = 211,
		SCIZOR = 212,
		SHUCKLE = 213,
		HERACROSS = 214,
		SNEASEL = 215,
		TEDDIURSA = 216,
		URSARING = 217,
		SLUGMA = 218,
		MAGCARGO = 219,
		SWINUB = 220,
		PILOSWINE = 221,
		CORSOLA = 222,
		REMORAID = 223,
		OCTILLERY = 224,
		DELIBIRD = 225,
		MANTINE = 226,
		SKARMORY = 227,
		HOUNDOUR = 228,
		HOUNDOOM = 229,
		KINGDRA = 230,
		PHANPY = 231,
		DONPHAN = 232,
		PORYGON2 = 233,
		STANTLER = 234,
		SMEARGLE = 235,
		TYROGUE = 236,
		HITMONTOP = 237,
		SMOOCHUM = 238,
		ELEKID = 239,
		MAGBY = 240,
		MILTANK = 241,
		BLISSEY = 242,
		RAIKOU = 243,
		ENTEI = 244,
		SUICUNE = 245,
		LARVITAR = 246,
		PUPITAR = 247,
		TYRANITAR = 248,
		LUGIA = 249,
		HO_OH = 250,
		CELEBI = 251,
		TREECKO = 252,
		GROVYLE = 253,
		SCEPTILE = 254,
		TORCHIC = 255,
		COMBUSKEN = 256,
		BLAZIKEN = 257,
		MUDKIP = 258,
		MARSHTOMP = 259,
		SWAMPERT = 260,
		POOCHYENA = 261,
		MIGHTYENA = 262,
		ZIGZAGOON = 263,
		LINOONE = 264,
		WURMPLE = 265,
		SILCOON = 266,
		BEAUTIFLY = 267,
		CASCOON = 268,
		DUSTOX = 269,
		LOTAD = 270,
		LOMBRE = 271,
		LUDICOLO = 272,
		SEEDOT = 273,
		NUZLEAF = 274,
		SHIFTRY = 275,
		TAILLOW = 276,
		SWELLOW = 277,
		WINGULL = 278,
		PELIPPER = 279,
		RALTS = 280,
		KIRLIA = 281,
		GARDEVOIR = 282,
		SURSKIT = 283,
		MASQUERAIN = 284,
		SHROOMISH = 285,
		BRELOOM = 286,
		SLAKOTH = 287,
		VIGOROTH = 288,
		SLAKING = 289,
		NINCADA = 290,
		NINJASK = 291,
		SHEDINJA = 292,
		WHISMUR = 293,
		LOUDRED = 294,
		EXPLOUD = 295,
		MAKUHITA = 296,
		HARIYAMA = 297,
		AZURILL = 298,
		NOSEPASS = 299,
		SKITTY = 300,
		DELCATTY = 301,
		SABLEYE = 302,
		MAWILE = 303,
		ARON = 304,
		LAIRON = 305,
		AGGRON = 306,
		MEDITITE = 307,
		MEDICHAM = 308,
		ELECTRIKE = 309,
		MANECTRIC = 310,
		PLUSLE = 311,
		MINUN = 312,
		VOLBEAT = 313,
		ILLUMISE = 314,
		ROSELIA = 315,
		GULPIN = 316,
		SWALOT = 317,
		CARVANHA = 318,
		SHARPEDO = 319,
		WAILMER = 320,
		WAILORD = 321,
		NUMEL = 322,
		CAMERUPT = 323,
		TORKOAL = 324,
		SPOINK = 325,
		GRUMPIG = 326,
		SPINDA = 327,
		TRAPINCH = 328,
		VIBRAVA = 329,
		FLYGON = 330,
		CACNEA = 331,
		CACTURNE = 332,
		SWABLU = 333,
		ALTARIA = 334,
		ZANGOOSE = 335,
		SEVIPER = 336,
		LUNATONE = 337,
		SOLROCK = 338,
		BARBOACH = 339,
		WHISCASH = 340,
		CORPHISH = 341,
		CRAWDAUNT = 342,
		BALTOY = 343,
		CLAYDOL = 344,
		LILEEP = 345,
		CRADILY = 346,
		ANORITH = 347,
		ARMALDO = 348,
		FEEBAS = 349,
		MILOTIC = 350,
		CASTFORM = 351,
		KECLEON = 352,
		SHUPPET = 353,
		BANETTE = 354,
		DUSKULL = 355,
		DUSCLOPS = 356,
		TROPIUS = 357,
		CHIMECHO = 358,
		ABSOL = 359,
		WYNAUT = 360,
		SNORUNT = 361,
		GLALIE = 362,
		SPHEAL = 363,
		SEALEO = 364,
		WALREIN = 365,
		CLAMPERL = 366,
		HUNTAIL = 367,
		GOREBYSS = 368,
		RELICANTH = 369,
		LUVDISC = 370,
		BAGON = 371,
		SHELGON = 372,
		SALAMENCE = 373,
		BELDUM = 374,
		METANG = 375,
		METAGROSS = 376,
		REGIROCK = 377,
		REGICE = 378,
		REGISTEEL = 379,
		LATIAS = 380,
		LATIOS = 381,
		KYOGRE = 382,
		GROUDON = 383,
		RAYQUAZA = 384,
		JIRACHI = 385,
		/// <summary>
		/// "DEOXYS_NORMAL"
		/// </summary>
		DEOXYS = 386,
		TURTWIG = 387,
		GROTLE = 388,
		TORTERRA = 389,
		CHIMCHAR = 390,
		MONFERNO = 391,
		INFERNAPE = 392,
		PIPLUP = 393,
		PRINPLUP = 394,
		EMPOLEON = 395,
		STARLY = 396,
		STARAVIA = 397,
		STARAPTOR = 398,
		BIDOOF = 399,
		BIBAREL = 400,
		KRICKETOT = 401,
		KRICKETUNE = 402,
		SHINX = 403,
		LUXIO = 404,
		LUXRAY = 405,
		BUDEW = 406,
		ROSERADE = 407,
		CRANIDOS = 408,
		RAMPARDOS = 409,
		SHIELDON = 410,
		BASTIODON = 411,
		BURMY = 412,
		/// <summary>
		/// "WORMADAM_PLANT"?
		/// </summary>
		WORMADAM = 413,
		MOTHIM = 414,
		COMBEE = 415,
		VESPIQUEN = 416,
		PACHIRISU = 417,
		BUIZEL = 418,
		FLOATZEL = 419,
		CHERUBI = 420,
		CHERRIM = 421,
		SHELLOS = 422,
		GASTRODON = 423,
		AMBIPOM = 424,
		DRIFLOON = 425,
		DRIFBLIM = 426,
		BUNEARY = 427,
		LOPUNNY = 428,
		MISMAGIUS = 429,
		HONCHKROW = 430,
		GLAMEOW = 431,
		PURUGLY = 432,
		CHINGLING = 433,
		STUNKY = 434,
		SKUNTANK = 435,
		BRONZOR = 436,
		BRONZONG = 437,
		BONSLY = 438,
		MIME_JR = 439,
		HAPPINY = 440,
		CHATOT = 441,
		SPIRITOMB = 442,
		GIBLE = 443,
		GABITE = 444,
		GARCHOMP = 445,
		MUNCHLAX = 446,
		RIOLU = 447,
		LUCARIO = 448,
		HIPPOPOTAS = 449,
		HIPPOWDON = 450,
		SKORUPI = 451,
		DRAPION = 452,
		CROAGUNK = 453,
		TOXICROAK = 454,
		CARNIVINE = 455,
		FINNEON = 456,
		LUMINEON = 457,
		MANTYKE = 458,
		SNOVER = 459,
		ABOMASNOW = 460,
		WEAVILE = 461,
		MAGNEZONE = 462,
		LICKILICKY = 463,
		RHYPERIOR = 464,
		TANGROWTH = 465,
		ELECTIVIRE = 466,
		MAGMORTAR = 467,
		TOGEKISS = 468,
		YANMEGA = 469,
		LEAFEON = 470,
		GLACEON = 471,
		GLISCOR = 472,
		MAMOSWINE = 473,
		PORYGON_Z = 474,
		GALLADE = 475,
		PROBOPASS = 476,
		DUSKNOIR = 477,
		FROSLASS = 478,
		ROTOM = 479,
		UXIE = 480,
		MESPRIT = 481,
		AZELF = 482,
		DIALGA = 483,
		PALKIA = 484,
		HEATRAN = 485,
		REGIGIGAS = 486,
		/// <summary>
		/// "GIRATINA_ALTERED"?
		/// </summary>
		GIRATINA = 487,
		CRESSELIA = 488,
		PHIONE = 489,
		MANAPHY = 490,
		DARKRAI = 491,
		/// <summary>
		/// "SHAYMIN_LAND"?
		/// </summary>
		SHAYMIN = 492,
		ARCEUS = 493,
		VICTINI = 494,
		SNIVY = 495,
		SERVINE = 496,
		SERPERIOR = 497,
		TEPIG = 498,
		PIGNITE = 499,
		EMBOAR = 500,
		OSHAWOTT = 501,
		DEWOTT = 502,
		SAMUROTT = 503,
		PATRAT = 504,
		WATCHOG = 505,
		LILLIPUP = 506,
		HERDIER = 507,
		STOUTLAND = 508,
		PURRLOIN = 509,
		LIEPARD = 510,
		PANSAGE = 511,
		SIMISAGE = 512,
		PANSEAR = 513,
		SIMISEAR = 514,
		PANPOUR = 515,
		SIMIPOUR = 516,
		MUNNA = 517,
		MUSHARNA = 518,
		PIDOVE = 519,
		TRANQUILL = 520,
		UNFEZANT = 521,
		BLITZLE = 522,
		ZEBSTRIKA = 523,
		ROGGENROLA = 524,
		BOLDORE = 525,
		GIGALITH = 526,
		WOOBAT = 527,
		SWOOBAT = 528,
		DRILBUR = 529,
		EXCADRILL = 530,
		AUDINO = 531,
		TIMBURR = 532,
		GURDURR = 533,
		CONKELDURR = 534,
		TYMPOLE = 535,
		PALPITOAD = 536,
		SEISMITOAD = 537,
		THROH = 538,
		SAWK = 539,
		SEWADDLE = 540,
		SWADLOON = 541,
		LEAVANNY = 542,
		VENIPEDE = 543,
		WHIRLIPEDE = 544,
		SCOLIPEDE = 545,
		COTTONEE = 546,
		WHIMSICOTT = 547,
		PETILIL = 548,
		LILLIGANT = 549,
		/// <summary>
		/// "BASCULIN_RED_STRIPED"
		/// </summary>
		BASCULIN = 550,
		SANDILE = 551,
		KROKOROK = 552,
		KROOKODILE = 553,
		DARUMAKA = 554,
		/// <summary>
		/// DARMANITAN_STANDARD
		/// </summary>
		DARMANITAN = 555,
		MARACTUS = 556,
		DWEBBLE = 557,
		CRUSTLE = 558,
		SCRAGGY = 559,
		SCRAFTY = 560,
		SIGILYPH = 561,
		YAMASK = 562,
		COFAGRIGUS = 563,
		TIRTOUGA = 564,
		CARRACOSTA = 565,
		ARCHEN = 566,
		ARCHEOPS = 567,
		TRUBBISH = 568,
		GARBODOR = 569,
		ZORUA = 570,
		ZOROARK = 571,
		MINCCINO = 572,
		CINCCINO = 573,
		GOTHITA = 574,
		GOTHORITA = 575,
		GOTHITELLE = 576,
		SOLOSIS = 577,
		DUOSION = 578,
		REUNICLUS = 579,
		DUCKLETT = 580,
		SWANNA = 581,
		VANILLITE = 582,
		VANILLISH = 583,
		VANILLUXE = 584,
		DEERLING = 585,
		SAWSBUCK = 586,
		EMOLGA = 587,
		KARRABLAST = 588,
		ESCAVALIER = 589,
		FOONGUS = 590,
		AMOONGUSS = 591,
		FRILLISH = 592,
		JELLICENT = 593,
		ALOMOMOLA = 594,
		JOLTIK = 595,
		GALVANTULA = 596,
		FERROSEED = 597,
		FERROTHORN = 598,
		KLINK = 599,
		KLANG = 600,
		KLINKLANG = 601,
		TYNAMO = 602,
		EELEKTRIK = 603,
		EELEKTROSS = 604,
		ELGYEM = 605,
		BEHEEYEM = 606,
		LITWICK = 607,
		LAMPENT = 608,
		CHANDELURE = 609,
		AXEW = 610,
		FRAXURE = 611,
		HAXORUS = 612,
		CUBCHOO = 613,
		BEARTIC = 614,
		CRYOGONAL = 615,
		SHELMET = 616,
		ACCELGOR = 617,
		STUNFISK = 618,
		MIENFOO = 619,
		MIENSHAO = 620,
		DRUDDIGON = 621,
		GOLETT = 622,
		GOLURK = 623,
		PAWNIARD = 624,
		BISHARP = 625,
		BOUFFALANT = 626,
		RUFFLET = 627,
		BRAVIARY = 628,
		VULLABY = 629,
		MANDIBUZZ = 630,
		HEATMOR = 631,
		DURANT = 632,
		DEINO = 633,
		ZWEILOUS = 634,
		HYDREIGON = 635,
		LARVESTA = 636,
		VOLCARONA = 637,
		COBALION = 638,
		TERRAKION = 639,
		VIRIZION = 640,
		/// <summary>
		/// "INCARNATE"?
		/// </summary>
		TORNADUS = 641,
		/// <summary>
		/// "INCARNATE"?
		/// </summary>
		THUNDURUS = 642,
		RESHIRAM = 643,
		ZEKROM = 644,
		/// <summary>
		/// "INCARNATE"?
		/// </summary>
		LANDORUS = 645,
		KYUREM = 646,
		/// <summary>
		/// KELDEO_ORDINARY
		/// </summary>
		KELDEO = 647,
		/// <summary>
		/// "MELOETTA_ARIA"
		/// </summary>
		MELOETTA = 648,
		GENESECT = 649,
		CHESPIN = 650,
		QUILLADIN = 651,
		CHESNAUGHT = 652,
		FENNEKIN = 653,
		BRAIXEN = 654,
		DELPHOX = 655,
		FROAKIE = 656,
		FROGADIER = 657,
		GRENINJA = 658,
		BUNNELBY = 659,
		DIGGERSBY = 660,
		FLETCHLING = 661,
		FLETCHINDER = 662,
		TALONFLAME = 663,
		SCATTERBUG = 664,
		SPEWPA = 665,
		VIVILLON = 666,
		LITLEO = 667,
		PYROAR = 668,
		FLABEBE = 669,
		FLOETTE = 670,
		FLORGES = 671,
		SKIDDO = 672,
		GOGOAT = 673,
		PANCHAM = 674,
		PANGORO = 675,
		FURFROU = 676,
		ESPURR = 677,
		/// <summary>
		/// "MEOWSTIC_MALE"
		/// </summary>
		MEOWSTIC = 678,
		HONEDGE = 679,
		DOUBLADE = 680,
		/// <summary>
		/// AEGISLASH_SHIELD
		/// </summary>
		AEGISLASH = 681,
		SPRITZEE = 682,
		AROMATISSE = 683,
		SWIRLIX = 684,
		SLURPUFF = 685,
		INKAY = 686,
		MALAMAR = 687,
		BINACLE = 688,
		BARBARACLE = 689,
		SKRELP = 690,
		DRAGALGE = 691,
		CLAUNCHER = 692,
		CLAWITZER = 693,
		HELIOPTILE = 694,
		HELIOLISK = 695,
		TYRUNT = 696,
		TYRANTRUM = 697,
		AMAURA = 698,
		AURORUS = 699,
		SYLVEON = 700,
		HAWLUCHA = 701,
		DEDENNE = 702,
		CARBINK = 703,
		GOOMY = 704,
		SLIGGOO = 705,
		GOODRA = 706,
		KLEFKI = 707,
		PHANTUMP = 708,
		TREVENANT = 709,
		/// <summary>
		/// "PUMPKABOO_AVERAGE"
		/// </summary>
		PUMPKABOO = 710,
		/// <summary>
		/// "GOURGEIST_AVERAGE"
		/// </summary>
		GOURGEIST = 711,
		BERGMITE = 712,
		AVALUGG = 713,
		NOIBAT = 714,
		NOIVERN = 715,
		XERNEAS = 716,
		YVELTAL = 717,
		ZYGARDE = 718,
		DIANCIE = 719,
		HOOPA = 720,
		VOLCANION = 721,
		//Everything beyond here is new
		ROWLET = 722,
		DARTRIX = 723,
		DECIDUEYE = 724,
		LITTEN = 725,
		TORRACAT = 726,
		INCINEROAR = 727,
		POPPLIO = 728,
		BRIONNE = 729,
		PRIMARINA = 730,
		PIKIPEK = 731,
		TRUMBEAK = 732,
		TOUCANNON = 733,
		YUNGOOS = 734,
		GUMSHOOS = 735,
		GRUBBIN = 736,
		CHARJABUG = 737,
		VIKAVOLT = 738,
		CRABRAWLER = 739,
		CRABOMINABLE = 740,
		ORICORIO_BAILE = 741,
		CUTIEFLY = 742,
		RIBOMBEE = 743,
		ROCKRUFF = 744,
		LYCANROC_MIDDAY = 745,
		WISHIWASHI_SOLO = 746,
		MAREANIE = 747,
		TOXAPEX = 748,
		MUDBRAY = 749,
		MUDSDALE = 750,
		DEWPIDER = 751,
		ARAQUANID = 752,
		FOMANTIS = 753,
		LURANTIS = 754,
		MORELULL = 755,
		SHIINOTIC = 756,
		SALANDIT = 757,
		SALAZZLE = 758,
		STUFFUL = 759,
		BEWEAR = 760,
		BOUNSWEET = 761,
		STEENEE = 762,
		TSAREENA = 763,
		COMFEY = 764,
		ORANGURU = 765,
		PASSIMIAN = 766,
		WIMPOD = 767,
		GOLISOPOD = 768,
		SANDYGAST = 769,
		PALOSSAND = 770,
		PYUKUMUKU = 771,
		TYPE_NULL = 772,
		SILVALLY = 773,
		MINIOR_RED_METEOR = 774,
		KOMALA = 775,
		TURTONATOR = 776,
		TOGEDEMARU = 777,
		MIMIKYU_DISGUISED = 778,
		BRUXISH = 779,
		DRAMPA = 780,
		DHELMISE = 781,
		JANGMO_O = 782,
		HAKAMO_O = 783,
		KOMMO_O = 784,
		TAPU_KOKO = 785,
		TAPU_LELE = 786,
		TAPU_BULU = 787,
		TAPU_FINI = 788,
		COSMOG = 789,
		COSMOEM = 790,
		SOLGALEO = 791,
		LUNALA = 792,
		NIHILEGO = 793,
		BUZZWOLE = 794,
		PHEROMOSA = 795,
		XURKITREE = 796,
		CELESTEELA = 797,
		KARTANA = 798,
		GUZZLORD = 799,
		NECROZMA = 800,
		MAGEARNA = 801,
		MARSHADOW = 802,
		POIPOLE = 803,
		NAGANADEL = 804,
		STAKATAKA = 805,
		BLACEPHALON = 806,
		ZERAORA = 807,
		DEOXYS_ATTACK = 10001,
		DEOXYS_DEFENSE = 10002,
		DEOXYS_SPEED = 10003,
		WORMADAM_SANDY = 10004,
		WORMADAM_TRASH = 10005,
		SHAYMIN_SKY = 10006,
		GIRATINA_ORIGIN = 10007,
		ROTOM_HEAT = 10008,
		ROTOM_WASH = 10009,
		ROTOM_FROST = 10010,
		ROTOM_FAN = 10011,
		ROTOM_MOW = 10012,
		CASTFORM_SUNNY = 10013,
		CASTFORM_RAINY = 10014,
		CASTFORM_SNOWY = 10015,
		BASCULIN_BLUE_STRIPED = 10016,
		DARMANITAN_ZEN = 10017,
		MELOETTA_PIROUETTE = 10018,
		TORNADUS_THERIAN = 10019,
		THUNDURUS_THERIAN = 10020,
		LANDORUS_THERIAN = 10021,
		KYUREM_BLACK = 10022,
		KYUREM_WHITE = 10023,
		KELDEO_RESOLUTE = 10024,
		MEOWSTIC_FEMALE = 10025,
		AEGISLASH_BLADE = 10026,
		PUMPKABOO_SMALL = 10027,
		PUMPKABOO_LARGE = 10028,
		PUMPKABOO_SUPER = 10029,
		GOURGEIST_SMALL = 10030,
		GOURGEIST_LARGE = 10031,
		GOURGEIST_SUPER = 10032,
		VENUSAUR_MEGA = 10033,
		CHARIZARD_MEGA_X = 10034,
		CHARIZARD_MEGA_Y = 10035,
		BLASTOISE_MEGA = 10036,
		ALAKAZAM_MEGA = 10037,
		GENGAR_MEGA = 10038,
		KANGASKHAN_MEGA = 10039,
		PINSIR_MEGA = 10040,
		GYARADOS_MEGA = 10041,
		AERODACTYL_MEGA = 10042,
		MEWTWO_MEGA_X = 10043,
		MEWTWO_MEGA_Y = 10044,
		AMPHAROS_MEGA = 10045,
		SCIZOR_MEGA = 10046,
		HERACROSS_MEGA = 10047,
		HOUNDOOM_MEGA = 10048,
		TYRANITAR_MEGA = 10049,
		BLAZIKEN_MEGA = 10050,
		GARDEVOIR_MEGA = 10051,
		MAWILE_MEGA = 10052,
		AGGRON_MEGA = 10053,
		MEDICHAM_MEGA = 10054,
		MANECTRIC_MEGA = 10055,
		BANETTE_MEGA = 10056,
		ABSOL_MEGA = 10057,
		GARCHOMP_MEGA = 10058,
		LUCARIO_MEGA = 10059,
		ABOMASNOW_MEGA = 10060,
		FLOETTE_ETERNAL = 10061,
		LATIAS_MEGA = 10062,
		LATIOS_MEGA = 10063,
		SWAMPERT_MEGA = 10064,
		SCEPTILE_MEGA = 10065,
		SABLEYE_MEGA = 10066,
		ALTARIA_MEGA = 10067,
		GALLADE_MEGA = 10068,
		AUDINO_MEGA = 10069,
		SHARPEDO_MEGA = 10070,
		SLOWBRO_MEGA = 10071,
		STEELIX_MEGA = 10072,
		PIDGEOT_MEGA = 10073,
		GLALIE_MEGA = 10074,
		DIANCIE_MEGA = 10075,
		METAGROSS_MEGA = 10076,
		KYOGRE_PRIMAL = 10077,
		GROUDON_PRIMAL = 10078,
		RAYQUAZA_MEGA = 10079,
		PIKACHU_ROCK_STAR = 10080,
		PIKACHU_BELLE = 10081,
		PIKACHU_POP_STAR = 10082,
		PIKACHU_PHD = 10083,
		PIKACHU_LIBRE = 10084,
		PIKACHU_COSPLAY = 10085,
		HOOPA_UNBOUND = 10086,
		CAMERUPT_MEGA = 10087,
		LOPUNNY_MEGA = 10088,
		SALAMENCE_MEGA = 10089,
		BEEDRILL_MEGA = 10090,
		RATTATA_ALOLA = 10091,
		RATICATE_ALOLA = 10092,
		RATICATE_TOTEM_ALOLA = 10093,
		PIKACHU_ORIGINAL_CAP = 10094,
		PIKACHU_HOENN_CAP = 10095,
		PIKACHU_SINNOH_CAP = 10096,
		PIKACHU_UNOVA_CAP = 10097,
		PIKACHU_KALOS_CAP = 10098,
		PIKACHU_ALOLA_CAP = 10099,
		RAICHU_ALOLA = 10100,
		SANDSHREW_ALOLA = 10101,
		SANDSLASH_ALOLA = 10102,
		VULPIX_ALOLA = 10103,
		NINETALES_ALOLA = 10104,
		DIGLETT_ALOLA = 10105,
		DUGTRIO_ALOLA = 10106,
		MEOWTH_ALOLA = 10107,
		PERSIAN_ALOLA = 10108,
		GEODUDE_ALOLA = 10109,
		GRAVELER_ALOLA = 10110,
		GOLEM_ALOLA = 10111,
		GRIMER_ALOLA = 10112,
		MUK_ALOLA = 10113,
		EXEGGUTOR_ALOLA = 10114,
		MAROWAK_ALOLA = 10115,
		GRENINJA_BATTLE_BOND = 10116,
		GRENINJA_ASH = 10117,
		ZYGARDE_10 = 10118,
		ZYGARDE_50 = 10119,
		ZYGARDE_COMPLETE = 10120,
		GUMSHOOS_TOTEM = 10121,
		VIKAVOLT_TOTEM = 10122,
		ORICORIO_POM_POM = 10123,
		ORICORIO_PAU = 10124,
		ORICORIO_SENSU = 10125,
		LYCANROC_MIDNIGHT = 10126,
		WISHIWASHI_SCHOOL = 10127,
		LURANTIS_TOTEM = 10128,
		SALAZZLE_TOTEM = 10129,
		MINIOR_ORANGE_METEOR = 10130,
		MINIOR_YELLOW_METEOR = 10131,
		MINIOR_GREEN_METEOR = 10132,
		MINIOR_BLUE_METEOR = 10133,
		MINIOR_INDIGO_METEOR = 10134,
		MINIOR_VIOLET_METEOR = 10135,
		MINIOR_RED = 10136,
		MINIOR_ORANGE = 10137,
		MINIOR_YELLOW = 10138,
		MINIOR_GREEN = 10139,
		MINIOR_BLUE = 10140,
		MINIOR_INDIGO = 10141,
		MINIOR_VIOLET = 10142,
		MIMIKYU_BUSTED = 10143,
		MIMIKYU_TOTEM_DISGUISED = 10144,
		MIMIKYU_TOTEM_BUSTED = 10145,
		KOMMO_O_TOTEM = 10146,
		MAGEARNA_ORIGINAL = 10147,
		PIKACHU_PARTNER_CAP = 10148,
		MAROWAK_TOTEM = 10149,
		RIBOMBEE_TOTEM = 10150,
		ROCKRUFF_OWN_TEMPO = 10151,
		LYCANROC_DUSK = 10152,
		ARAQUANID_TOTEM = 10153,
		TOGEDEMARU_TOTEM = 10154,
		NECROZMA_DUSK = 10155,
		NECROZMA_DAWN = 10156,
		NECROZMA_ULTRA = 10157
	}
	#endregion
}