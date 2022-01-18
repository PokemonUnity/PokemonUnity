namespace PokemonUnity.Shared.Enums
{
	/// <summary>
	/// Ability ids are connected to XML file.
	/// </summary>
	/// <remarks>Can now code with strings or int and
	/// access the same value.</remarks>
	public class Abilities : Enumeration
	{
		public int GenerationId { get; private set; }
		protected Abilities(int id, string name) : base(id, name) { }
		public static readonly Abilities NONE			= new Abilities(0,	"NONE");
		/// <summary>
		/// Has a 10% chance of making target Pokémon [flinch]{mechanic:flinch} with each hit.<para></para> This Pokémon's damaging moves have a 10% chance to make the target [flinch]{mechanic:flinch} with each hit if they do not already cause flinching as a secondary effect.
		/// 
		/// This ability does not stack with a held item.
		/// 
		/// Overworld: The wild encounter rate is halved while this Pokémon is first in the party.
		/// </summary>
		public static readonly Abilities STENCH			= new Abilities(1,	"STENCH");
		/// <summary>
		/// Summons [rain]{mechanic:rain} that lasts indefinitely upon entering battle.<para></para> The [weather]{mechanic:weather} changes to [rain]{mechanic:rain} when this Pokémon enters battle and does not end unless replaced by another weather condition.
		/// 
		/// If multiple Pokémon with this ability, []{ability:drought}, []{ability:sand-stream}, or []{ability:snow-warning} are sent out at the same time, the abilities will activate in order of [Speed]{mechanic:speed}, respecting []{move:trick-room}.  Each ability's weather will cancel the previous weather, and only the weather summoned by the slowest of the Pokémon will stay.
		/// </summary>
		public static readonly Abilities DRIZZLE			= new Abilities(2,	"DRIZZLE");
		/// <summary>
		/// Raises [Speed]{mechanic:speed} one [stage]{mechanic:stat-modifier} after each turn.<para></para> This Pokémon's [Speed]{mechanic:speed} rises one [stage]{mechanic:stat-modifier} after each turn.
		/// </summary>
		public static readonly Abilities SPEED_BOOST			= new Abilities(3,	"SPEED_BOOST");
		/// <summary>
		/// Protects against [critical hits]{mechanic:critical-hit}.<para></para> Moves cannot score [critical hits]{mechanic:critical-hit} against this Pokémon.
		/// 
		/// This ability functions identically to []{ability:shell-armor}.
		/// </summary>
		public static readonly Abilities BATTLE_ARMOR			= new Abilities(4,	"BATTLE_ARMOR");
		/// <summary>
		/// Prevents being KOed from full [HP]{mechanic:hp}, leaving 1 HP instead.  Protects against the one-hit KO moves regardless of HP.<para></para> When this Pokémon is at full [HP]{mechanic:hp}, any hit that would knock it out will instead leave it with 1 HP.  Regardless of its current HP, it is also immune to the one-hit KO moves: []{move:fissure}, []{move:guillotine}, []{move:horn-drill}, and []{move:sheer-cold}.
		/// 
		/// If this Pokémon is holding a []{item:focus-sash}, this ability takes precedence and the item will not be consumed.
		/// </summary>
		public static readonly Abilities STURDY			= new Abilities(5,	"STURDY");
		/// <summary>
		/// Prevents []{move:self-destruct}, []{move:explosion}, and []{ability:aftermath} from working while the Pokémon is in battle.<para></para> While this Pokémon is in battle, []{move:self-destruct} and []{move:explosion} will fail and []{ability:aftermath} will not take effect.
		/// </summary>
		public static readonly Abilities DAMP			= new Abilities(6,	"DAMP");
		/// <summary>
		/// Prevents [paralysis]{mechanic:paralysis}.<para></para> This Pokémon cannot be [paralyzed]{mechanic:paralysis}.
		/// 
		/// If a Pokémon is paralyzed and acquires this ability, its paralysis is healed; this includes when regaining a lost ability upon leaving battle.
		/// </summary>
		public static readonly Abilities LIMBER			= new Abilities(7,	"LIMBER");
		/// <summary>
		/// Increases [evasion]{mechanic:evasion} to 1.25× during a [sandstorm]{mechanic:sandstorm}.  Protects against sandstorm damage.<para></para> During a [sandstorm]{mechanic:sandstorm}, this Pokémon has 1.25× its [evasion]{mechanic:evasion}, and it does not take sandstorm damage regardless of type.
		/// 
		/// The evasion bonus does not count as a [stat modifier]{mechanic:stat-modifier}.
		/// 
		/// Overworld: If the lead Pokémon has this ability, the wild encounter rate is halved in a sandstorm.
		/// </summary>
		public static readonly Abilities SAND_VEIL			= new Abilities(8,	"SAND_VEIL");
		/// <summary>
		/// Has a 30% chance of [paralyzing]{mechanic:paralysis} attacking Pokémon on contact.<para></para> Whenever a move makes contact with this Pokémon, the move's user has a 30% chance of being [paralyzed]{mechanic:paralysis}.
		/// 
		/// Pokémon that are immune to []{type:electric}-type moves can still be paralyzed by this ability.
		/// 
		/// Overworld: If the lead Pokémon has this ability, there is a 50% chance that encounters will be with an []{type:electric} Pokémon, if applicable.
		/// </summary>
		public static readonly Abilities STATIC			= new Abilities(9,	"STATIC");
		/// <summary>
		/// Absorbs []{type:electric} moves, healing for 1/4 max [HP]{mechanic:hp}.<para></para> Whenever an []{type:electric}-type move hits this Pokémon, it heals for 1/4 of its maximum [HP]{mechanic:hp}, negating any other effect on it.
		/// 
		/// This ability will not take effect if this Pokémon is []{type:ground}-type and thus immune to Electric moves.  Electric moves will ignore this Pokémon's []{move:substitute}.
		/// 
		/// This effect includes non-damaging moves, i.e. []{move:thunder-wave}.
		/// </summary>
		public static readonly Abilities VOLT_ABSORB			= new Abilities(10,	"VOLT_ABSORB");
		/// <summary>
		/// Absorbs []{type:water} moves, healing for 1/4 max [HP]{mechanic:hp}.<para></para> Whenever a []{type:water}-type move hits this Pokémon, it heals for 1/4 of its maximum [HP]{mechanic:hp}, negating any other effect on it.
		/// 
		/// Water moves will ignore this Pokémon's []{move:substitute}.
		/// </summary>
		public static readonly Abilities WATER_ABSORB			= new Abilities(11,	"WATER_ABSORB");
		/// <summary>
		/// Prevents [infatuation]{mechanic:infatuation} and protects against []{move:captivate}.<para></para> This Pokémon cannot be [infatuated]{mechanic:infatuation} and is immune to []{move:captivate}.
		/// 
		/// If a Pokémon is infatuated and acquires this ability, its infatuation is cleared.
		/// </summary>
		public static readonly Abilities OBLIVIOUS			= new Abilities(12,	"OBLIVIOUS");
		/// <summary>
		/// Negates all effects of [weather]{mechanic:weather}, but does not prevent the weather itself.<para></para> While this Pokémon is in battle, [weather]{mechanic:weather} can still be in play, but will not have any of its effects.
		/// 
		/// This ability functions identically to []{ability:air-lock}.
		/// </summary>
		public static readonly Abilities CLOUD_NINE			= new Abilities(13,	"CLOUD_NINE");
		/// <summary>
		/// Increases moves' [accuracy]{mechanic:accuracy} to 1.3×.<para></para> This Pokémon's moves have 1.3× their [accuracy]{mechanic:accuracy}.
		/// 
		/// This ability has no effect on the one-hit KO moves ([]{move:fissure}, []{move:guillotine}, []{move:horn-drill}, and []{move:sheer-cold}).
		/// 
		/// Overworld: If the first Pokémon in the party has this ability, the chance of a wild Pokémon holding a particular item is raised from 50%, 5%, or 1% to 60%, 20%, or 5%, respectively.
		/// </summary>
		public static readonly Abilities COMPOUND_EYES			= new Abilities(14,	"COMPOUND_EYES");
		/// <summary>
		/// Prevents [sleep]{mechanic:sleep}.<para></para> This Pokémon cannot be [asleep]{mechanic:sleep}.
		/// 
		/// This causes []{move:rest} to fail altogether.  If a Pokémon is asleep and acquires this ability, it will immediately wake up; this includes when regaining a lost ability upon leaving battle.
		/// 
		/// This ability functions identically to []{ability:vital-spirit} in battle.
		/// </summary>
		public static readonly Abilities INSOMNIA			= new Abilities(15,	"INSOMNIA");
		/// <summary>
		/// Changes type to match when hit by a damaging move.<para></para> Whenever this Pokémon takes damage from a move, the Pokémon's type changes to match the move.
		/// 
		/// If the Pokémon has two types, both are overridden.  The Pokémon must directly take damage; for example, moves blocked by a []{move:substitute} will not trigger this ability, nor will moves that deal damage indirectly, such as []{move:spikes}.
		/// 
		/// This ability takes effect on only the last hit of a multiple-hit attack.
		/// 
		/// In Pokémon Colosseum and XD: Gale of Darkness, this ability does not take effect on Shadow-type moves.
		/// </summary>
		public static readonly Abilities COLOR_CHANGE			= new Abilities(16,	"COLOR_CHANGE");
		/// <summary>
		/// Prevents [poison]{mechanic:poison}.<para></para> This Pokémon cannot be [poisoned]{mechanic:poison}.  This includes bad poison.
		/// 
		/// If a Pokémon is poisoned and acquires this ability, its poison is healed; this includes when regaining a lost ability upon leaving battle.
		/// </summary>
		public static readonly Abilities IMMUNITY			= new Abilities(17,	"IMMUNITY");
		/// <summary>
		/// Protects against []{type:fire} moves.  Once one has been blocked, the Pokémon's own Fire moves inflict 1.5× damage until it leaves battle.<para></para> This Pokémon is immune to []{type:fire}-type moves.  Once this Pokémon has been hit by a Fire move, its own Fire moves will inflict 1.5× as much damage until it leaves battle.
		/// 
		/// This ability has no effect while the Pokémon is [frozen]{mechanic:freezing}.  The Fire damage bonus is retained even if the Pokémon is frozen and thawed or the ability is lost or disabled.  Fire moves will ignore this Pokémon's []{move:substitute}.  This ability takes effect even on non-damaging moves, i.e. []{move:will-o-wisp}.
		/// </summary>
		public static readonly Abilities FLASH_FIRE			= new Abilities(18,	"FLASH_FIRE");
		/// <summary>
		/// Protects against incoming moves' extra effects.<para></para> This Pokémon is immune to the extra effects of moves used against it.
		/// 
		/// An extra effect is a move's chance, listed as an "effect chance", to inflict a [status ailment]{mechanic:status-ailment}, cause a [stat change]{mechanic:stat-modifier}, or make the target [flinch]{mechanic:flinching} in addition to the move's main effect.  For example, []{move:thunder-shock}'s [paralysis]{mechanic:paralysis} is an extra effect, but []{move:thunder-wave}'s is not, nor are []{move:knock-off}'s item removal and []{move:air-cutter}'s increased [critical hit]{mechanic:critial-hit} rate.
		/// </summary>
		public static readonly Abilities SHIELD_DUST			= new Abilities(19,	"SHIELD_DUST");
		/// <summary>
		/// Prevents [confusion]{mechanic:confusion}.<para></para> This Pokémon cannot be [confused]{mechanic:confusion}.
		/// 
		/// If a Pokémon is confused and acquires this ability, its confusion will immediately be healed.
		/// </summary>
		public static readonly Abilities OWN_TEMPO			= new Abilities(20,	"OWN_TEMPO");
		/// <summary>
		/// Prevents being forced out of battle by other Pokémon's moves.<para></para> This Pokémon cannot be forced out of battle by moves such as []{move:whirlwind}.
		/// 
		/// []{move:dragon-tail} and []{move:circle-throw} still inflict damage against this Pokémon.
		/// 
		/// Overworld: If the lead Pokémon has this ability, the success rate while fishing is increased.
		/// </summary>
		public static readonly Abilities SUCTION_CUPS			= new Abilities(21,	"SUCTION_CUPS");
		/// <summary>
		/// Lowers opponents' [Attack]{mechanic:attack} one [stage]{mechanic:stat-modifier} upon entering battle.<para></para> When this Pokémon enters battle, the opponent's [Attack]{mechanic:attack} is lowered by one [stage]{mechanic:stat-modifier}.  In a double battle, both opponents are affected.
		/// 
		/// This ability also takes effect when acquired during a battle, but will not take effect again if lost and reobtained without leaving battle.
		/// 
		/// This ability has no effect on an opponent that has a []{move:substitute}.
		/// 
		/// Overworld: If the first Pokémon in the party has this ability, any random encounter with a Pokémon five or more levels lower than it has a 50% chance of being skipped.
		/// </summary>
		public static readonly Abilities INTIMIDATE			= new Abilities(22,	"INTIMIDATE");
		/// <summary>
		/// Prevents opponents from fleeing or switching out.<para></para> While this Pokémon is in battle, opposing Pokémon cannot flee or switch out.
		/// 
		/// Other Pokémon with this ability are unaffected.  Pokémon with []{ability:run-away} can still flee.  Pokémon can still switch out with the use of a move or item.
		/// </summary>
		public static readonly Abilities SHADOW_TAG			= new Abilities(23,	"SHADOW_TAG");
		/// <summary>
		/// Damages attacking Pokémon for 1/8 their max [HP]{mechanic:hp} on contact.<para></para> Whenever a move makes contact with this Pokémon, the move's user takes 1/8 of its maximum [HP]{mechanic:hp} in damage.
		/// 
		/// This ability functions identically to []{ability:iron-barbs}.
		/// </summary>
		public static readonly Abilities ROUGH_SKIN			= new Abilities(24,	"ROUGH_SKIN");
		/// <summary>
		/// Protects against damaging moves that are not [super effective]{mechanic:super-effective}.<para></para> This Pokémon is immune to damaging moves that are not [super effective]{mechanic:super-effective} against it.
		/// 
		/// Moves that inflict fixed damage, such as []{move:night-shade} or []{move:seismic-toss}, are considered super effective if their types are.  Damage not directly dealt by moves, such as damage from [weather]{mechanic:weather}, a [status ailment]{mechanic:status-ailment}, or []{move:spikes}, is not prevented.
		/// 
		/// This ability cannot be copied with []{move:role-play} or traded away with []{move:skill-swap}, but it can be copied with []{ability:trace}, disabled with []{move:gastro-acid}, or changed with []{move:worry-seed}.  This Pokémon can still use Role Play itself to lose this ability, but not Skill Swap.
		/// 
		/// If this Pokémon has a []{move:substitute}, this ability will block moves as usual and any moves not blocked will react to the Substitute as usual.
		/// </summary>
		public static readonly Abilities WONDER_GUARD			= new Abilities(25,	"WONDER_GUARD");
		/// <summary>
		/// Evades []{type:ground} moves.<para></para> This Pokémon is immune to []{type:ground}-type moves, []{move:spikes}, []{move:toxic-spikes}, and []{ability:arena-trap}.
		/// 
		/// This ability is disabled during []{move:gravity} or []{move:ingrain}, or while holding an []{item:iron-ball}.  This ability is not disabled during []{move:roost}.
		/// </summary>
		public static readonly Abilities LEVITATE			= new Abilities(26,	"LEVITATE");
		/// <summary>
		/// Has a 30% chance of inflcting either [paralysis]{mechanic:paralysis}, [poison]{mechanic:poison}, or [sleep]{mechanic:sleep} on attacking Pokémon on contact.<para></para> Whenever a move makes contact with this Pokémon, the move's user has a 30% chance of being [paralyzed]{mechanic:paralysis}, [poisoned]{mechanic:poison}, or put to [sleep]{mechanic:sleep}, chosen at random.
		/// 
		/// Nothing is done to compensate if the move's user is immune to one of these ailments; there is simply a lower chance that the move's user will be affected.
		/// </summary>
		public static readonly Abilities EFFECT_SPORE			= new Abilities(27,	"EFFECT_SPORE");
		/// <summary>
		/// Copies [burns]{mechanic:burn}, [paralysis]{mechanic:paralysis}, and [poison]{mechanic:poison} received onto the Pokémon that inflicted them.<para></para> Whenever this Pokémon is [burned]{mechanic:burn}, [paralyzed]{mechanic:paralysis}, or [poisoned]{mechanic:poison}, the Pokémon who gave this Pokémon that ailment is also given the ailment.
		/// 
		/// This ability passes back bad poison when this Pokémon is badly poisoned.  This ability cannot pass on a status ailment that the Pokémon did not directly receive from another Pokémon, such as the poison from []{move:toxic-spikes} or the burn from a []{item:flame-orb}.
		/// 
		/// Overworld: If the lead Pokémon has this ability, wild Pokémon have a 50% chance of having the lead Pokémon's nature, and a 50% chance of being given a random nature as usual, including the lead Pokémon's nature.  This does not work on Pokémon received outside of battle or roaming legendaries.
		/// </summary>
		public static readonly Abilities SYNCHRONIZE			= new Abilities(28,	"SYNCHRONIZE");
		/// <summary>
		/// Prevents stats from being [lowered]{mechanic:stat-modifier} by other Pokémon.<para></para> This Pokémon cannot have its stats [lowered]{mechanic:stat-modifier} by other Pokémon.
		/// 
		/// This ability does not prevent any stat losses other than [stat modifiers]{mechanic:stat-modifiers}, such as the [Speed]{mechanic:speed} cut from [paralysis]{mechanic:paralysis}.  This Pokémon can still be passed negative stat modifiers through []{move:guard-swap}, []{move:heart-swap}, or []{move:power-swap}.
		/// 
		/// This ability functions identically to []{ability:white-smoke} in battle.
		/// </summary>
		public static readonly Abilities CLEAR_BODY			= new Abilities(29,	"CLEAR_BODY");
		/// <summary>
		/// Cures any [major status ailment]{mechanic:major-status-ailment} upon switching out.<para></para> This Pokémon is cured of any [major status ailment]{mechanic:major-status-ailment} when it is switched out for another Pokémon.
		/// 
		/// If this ability is acquired during battle, the Pokémon is cured upon leaving battle before losing the temporary ability.
		/// </summary>
		public static readonly Abilities NATURAL_CURE			= new Abilities(30,	"NATURAL_CURE");
		/// <summary>
		/// Redirects single-target []{type:electric} moves to this Pokémon where possible.  Absorbs Electric moves, raising [Special Attack]{mechanic:special-attack} one [stage]{mechanic:stat-modifier}.<para></para> All other Pokémon's single-target []{type:electric}-type moves are redirected to this Pokémon if it is an eligible target.  Other Pokémon's Electric moves raise this Pokémon's [Special Attack]{mechanic:special-attack} one [stage]{mechanic:stat-modifier}, negating any other effect on it, and cannot miss it.
		/// 
		/// If the move's intended target also has this ability, the move is not redirected.  When multiple Pokémon with this ability are possible targets for redirection, the move is redirected to the one with the highest [Speed]{mechanic:speed} stat, or, in the case of a tie, to a random tied Pokémon.  []{move:follow-me} takes precedence over this ability.
		/// 
		/// If the Pokémon is a []{type:ground}-type and thus immune to Electric moves, its immunity prevents the Special Attack boost.
		/// </summary>
		public static readonly Abilities LIGHTNING_ROD			= new Abilities(31,	"LIGHTNING_ROD");
		/// <summary>
		/// Doubles the chance of moves' extra effects occurring.<para></para> This Pokémon's moves have twice their usual effect chance.
		/// 
		/// An effect chance is a move's chance to inflict a [status ailment]{mechanic:status-ailment}, cause a [stat change]{mechanic:stat-modifier}, or make the target [flinch]{mechanic:flinching} in addition to the move's main effect.  For example, []{move:flamethrower}'s chance of [burning]{mechanic:burn} the target is doubled, but []{move:protect}'s chance of success and []{move:air-cutter}'s increased [critical hit]{mechanic:critical-hit} rate are unaffected.
		/// 
		/// []{move:secret-power} is unaffected.
		/// </summary>
		public static readonly Abilities SERENE_GRACE			= new Abilities(32,	"SERENE_GRACE");
		/// <summary>
		/// Doubles [Speed]{mechanic:speed} during [rain]{mechanic:rain}.<para></para> This Pokémon's [Speed]{mechanic:speed} is doubled during [rain]{mechanic:rain}.
		/// 
		/// This bonus does not count as a [stat modifier]{mechanic:stat-modifier}.
		/// </summary>
		public static readonly Abilities SWIFT_SWIM			= new Abilities(33,	"SWIFT_SWIM");
		/// <summary>
		/// Doubles [Speed]{mechanic:speed} during [strong sunlight]{mechanic:strong-sunlight}.<para></para> This Pokémon's [Speed]{mechanic:speed} is doubled during [strong sunlight]{mechanic:strong-sunlight}.
		/// 
		/// This bonus does not count as a [stat modifier]{mechanic:stat-modifier}.
		/// </summary>
		public static readonly Abilities CHLOROPHYLL			= new Abilities(34,	"CHLOROPHYLL");
		/// <summary>
		/// Doubles the wild encounter rate.<para></para> Overworld: If the lead Pokémon has this ability, the wild encounter rate is doubled.
		/// 
		/// This ability has no effect in battle.
		/// </summary>
		public static readonly Abilities ILLUMINATE			= new Abilities(35,	"ILLUMINATE");
		/// <summary>
		/// Copies an opponent's ability upon entering battle.<para></para> When this Pokémon enters battle, it copies a random opponent's ability.
		/// 
		/// This ability cannot copy []{ability:flower-gift}, []{ability:forecast}, []{ability:illusion}, []{ability:imposter}, []{ability:multitype}, []{ability:trace}, []{ability:wonder-guard}, or []{ability:zen-mode}.
		/// </summary>
		public static readonly Abilities TRACE			= new Abilities(36,	"TRACE");
		/// <summary>
		/// Doubles [Attack]{mechanic:attack} in battle.<para></para> This Pokémon's [Attack]{mechanic:attack} is doubled while in battle.
		/// 
		/// This bonus does not count as a [stat modifier]{mechanic:stat-modifier}.
		/// 
		/// This ability functions identically to []{ability:pure-power}.
		/// </summary>
		public static readonly Abilities HUGE_POWER			= new Abilities(37,	"HUGE_POWER");
		/// <summary>
		/// Has a 30% chance of [poisoning]{mechanic:poison} attacking Pokémon on contact.<para></para> Whenever a move makes contact with this Pokémon, the move's user has a 30% chance of being [poisoned]{mechanic:poison}.
		/// </summary>
		public static readonly Abilities POISON_POINT			= new Abilities(38,	"POISON_POINT");
		/// <summary>
		/// Prevents [flinching]{mechanic:flinching}.<para></para> This Pokémon cannot [flinch]{mechanic:flinching}.
		/// </summary>
		public static readonly Abilities INNER_FOCUS			= new Abilities(39,	"INNER_FOCUS");
		/// <summary>
		/// Prevents [freezing]{mechanic:freezing}.<para></para> This Pokémon cannot be [frozen]{mechanic:freezing}.
		/// 
		/// If a Pokémon is frozen and acquires this ability, it will immediately thaw out; this includes when regaining a lost ability upon leaving battle.
		/// 
		/// Overworld: If any Pokémon in the party has this ability, each egg in the party has its [hatch counter]{mechanic:hatch-counter} decreased by 2 (rather than 1) each [step cycle]{mechanic:step-cycle}, making eggs hatch roughly twice as quickly.  This effect does not stack if multiple Pokémon have this ability or []{ability:flame-body}.
		/// </summary>
		public static readonly Abilities MAGMA_ARMOR			= new Abilities(40,	"MAGMA_ARMOR");
		/// <summary>
		/// Prevents [burns]{mechanic:burn}.<para></para> This Pokémon cannot be [burned]{mechanic:burn}.
		/// 
		/// If a Pokémon is burned and acquires this ability, its burn is healed; this includes when regaining a lost ability upon leaving battle.
		/// </summary>
		public static readonly Abilities WATER_VEIL			= new Abilities(41,	"WATER_VEIL");
		/// <summary>
		/// Prevents []{type:steel} opponents from fleeing or switching out.<para></para> While this Pokémon is in battle, opposing []{type:steel}-type Pokémon cannot flee or switch out.
		/// 
		/// Pokémon with []{ability:run-away} can still flee.  Pokémon can still switch out with the use of a move or item.
		/// 
		/// Overworld: If the lead Pokémon has this ability, Steel-type Pokémon have a higher encounter rate.
		/// </summary>
		public static readonly Abilities MAGNET_PULL			= new Abilities(42,	"MAGNET_PULL");
		/// <summary>
		/// Protects against sound-based moves.<para></para> This Pokémon is immune to moves flagged as being sound-based.
		/// 
		/// []{move:heal-bell} is unaffected.  []{move:uproar} still prevents this Pokémon from [sleeping]{mechanic:sleep}.  This Pokémon can still receive a Perish Song counter through []{move:baton-pass}, and will retain a Perish Song counter if it acquires this ability after Perish Song is used.
		/// 
		/// []{move:howl}, []{move:roar-of-time}, []{move:sonic-boom}, and []{move:yawn} are not flagged as sound-based.
		/// </summary>
		public static readonly Abilities SOUNDPROOF			= new Abilities(43,	"SOUNDPROOF");
		/// <summary>
		/// Heals for 1/16 max [HP]{mechanic:hp} after each turn during [rain]{mechanic:rain}.<para></para> This Pokémon heals for 1/16 of its maximum [HP]{mechanic:hp} after each turn during [rain]{mechanic:rain}.
		/// </summary>
		public static readonly Abilities RAIN_DISH			= new Abilities(44,	"RAIN_DISH");
		/// <summary>
		/// Summons a [sandstorm]{mechanic:sandstorm} that lasts indefinitely upon entering battle.<para></para> The [weather]{mechanic:weather} changes to a [sandstorm]{mechanic:sandstorm} when this Pokémon enters battle and does not end unless cancelled by another weather condition.
		/// 
		/// If multiple Pokémon with this ability, []{ability:drizzle}, []{ability:drought}, or []{ability:snow-warning} are sent out at the same time, the abilities will activate in order of [Speed]{mechanic:speed}, respecting []{move:trick-room}.  Each ability's weather will cancel the previous weather, and only the weather summoned by the slowest of the Pokémon will stay.
		/// 
		/// Overworld: If the lead Pokémon has this ability, the wild encounter rate is halved in a sandstorm.
		/// </summary>
		public static readonly Abilities SAND_STREAM			= new Abilities(45,	"SAND_STREAM");
		/// <summary>
		/// Increases the [PP]{mechanic:pp} cost of moves targetting the Pokémon by one.<para></para> Moves targetting this Pokémon use one extra [PP]{mechanic:pp}.
		/// 
		/// This ability stacks if multiple targets have it.  This ability still affects moves that fail or miss.  This ability does not affect ally moves that target either the entire field or just its side, nor this Pokémon's self-targetted moves; it does, however, affect single-targetted ally moves aimed at this Pokémon, ally moves that target all other Pokémon, and opponents' moves that target the entire field.  If this ability raises a move's PP cost above its remaining PP, it will use all remaining PP.
		/// 
		/// When this Pokémon enters battle, all participating trainers are notified that it has this ability.
		/// 
		/// Overworld: If the lead Pokémon has this ability, higher-levelled Pokémon have their encounter rate increased.
		/// </summary>
		public static readonly Abilities PRESSURE			= new Abilities(46,	"PRESSURE");
		/// <summary>
		/// Halves damage from []{type:fire} and []{type:ice} moves.<para></para> This Pokémon takes half as much damage from []{type:fire}- and []{type:ice}-type moves.
		/// </summary>
		public static readonly Abilities THICK_FAT			= new Abilities(47,	"THICK_FAT");
		/// <summary>
		/// Makes [sleep]{mechanic:sleep} pass twice as quickly.<para></para> This Pokémon's remaining sleep turn count falls by 2 rather than 1.
		/// 
		/// If this Pokémon's sleep counter is at 1, it will fall to 0 and then the Pokémon will wake up.
		/// </summary>
		public static readonly Abilities EARLY_BIRD			= new Abilities(48,	"EARLY_BIRD");
		/// <summary>
		/// Has a 30% chance of [burning]{mechanic:burn} attacking Pokémon on contact.<para></para> Whenever a move makes contact with this Pokémon, the move's user has a 30% chance of being [burned]{mechanic:burn}.
		/// 
		/// Overworld: If any Pokémon in the party has this ability, each egg in the party has its [hatch counter]{mechanic:hatch-counter} decreased by 2 (rather than 1) each [step cycle]{mechanic:step-cycle}, making eggs hatch roughly twice as quickly.  This effect does not stack if multiple Pokémon have this ability or []{ability:magma-armor}.
		/// </summary>
		public static readonly Abilities FLAME_BODY			= new Abilities(49,	"FLAME_BODY");
		/// <summary>
		/// Ensures success fleeing from wild battles.<para></para> This Pokémon is always successful fleeing from wild battles, even if [trapped]{mechanic:trapped} by a move or ability.
		/// </summary>
		public static readonly Abilities RUN_AWAY			= new Abilities(50,	"RUN_AWAY");
		/// <summary>
		/// Prevents [accuracy]{mechanic:accuracy} from being [lowered]{mechanic:stat-modifier}.<para></para> This Pokémon cannot have its [accuracy]{mechanic:accuracy} [lowered]{mechanic:stat-modifier}.
		/// 
		/// This ability does not prevent any accuracy losses other than [stat modifiers]{mechanic:stat-modifiers}, such as the accuracy cut from [fog]{mechanic:fog}; nor does it prevent other Pokémon's [evasion]{mechanic:evasion} from making this Pokémon's moves less accurate.  This Pokémon can still be passed negative accuracy modifiers through []{move:heart-swap}.
		/// 
		/// Overworld: If the first Pokémon in the party has this ability, any random encounter with a Pokémon five or more levels lower than it has a 50% chance of being skipped.
		/// </summary>
		public static readonly Abilities KEEN_EYE			= new Abilities(51,	"KEEN_EYE");
		/// <summary>
		/// Prevents [Attack]{mechanic:attack} from being [lowered]{mechanic:stat-modifiers} by other Pokémon.<para></para> This Pokémon's [Attack]{mechanic:attack} cannot be [lowered]{mechanic:stat-modifier} by other Pokémon.
		/// 
		/// This ability does not prevent any Attack losses other than [stat modifiers]{mechanic:stat-modifiers}, such as the Attack cut from a [burn]{mechanic:burn}.  This Pokémon can still be passed negative Attack modifiers through []{move:heart-swap} or []{move:power-swap}.
		/// </summary>
		public static readonly Abilities HYPER_CUTTER			= new Abilities(52,	"HYPER_CUTTER");
		/// <summary>
		/// Picks up other Pokémon's used and [Flung]{move:fling} held items.  May also pick up an item after battle.<para></para> At the end of each turn, if another Pokémon consumed or [Flung]{move:fling} a held item that turn, this Pokémon picks up the item if it is not already holding one.  After each battle, this Pokémon has a 10% chance of picking up an item if it is not already holding one.
		/// 
		/// The []{item:air-balloon} and []{item:eject-button} cannot be picked up.
		/// 
		/// The items that may be found vary by game, and, since Pokémon Emerald, by the Pokémon's level.  This ability is checked after the battle ends, at which point any temporary ability changes have worn off.
		/// </summary>
		public static readonly Abilities PICKUP			= new Abilities(53,	"PICKUP");
		/// <summary>
		/// Skips every second turn.<para></para> Every second turn on which this Pokémon should attempt to use a move, it will instead do nothing ("loaf around").
		/// 
		/// Loafing around interrupts moves that take multiple turns the same way [paralysis]{mechanic:paralysis}, [flinching]{mechanic:flinching}, etc do.  Most such moves, for example []{move:bide} or []{move:rollout}, are simply cut off upon loafing around.  Attacks with a recharge turn, such as []{move:hyper-beam}, do not have to recharge; attacks with a preparation turn, such as []{move:fly}, do not end up being used.  Moves that are forced over multiple turns and keep going through failure, such as []{move:outrage}, []{move:uproar}, or any move forced by []{move:encore}, keep going as usual.
		/// 
		/// If this Pokémon is [confused]{mechanic:confusion}, its confusion is not checked when loafing around; the Pokémon cannot hurt itself, and its confusion does not end or come closer to ending.
		/// 
		/// If this Pokémon attempts to move but fails, e.g. because of [paralysis]{mechanic:paralysis} or []{move:gravity}, it still counts as having moved and will loaf around the next turn.  If it does not attempt to move, e.g. because it is [asleep]{mechanic:sleep} or [frozen]{mechanic:freezing}, whatever it would have done will be postponed until its next attempt; that is, it will either loaf around or move as usual, depending on what it last did.
		/// 
		/// This ability cannot be changed with []{move:worry-seed}, but it can be disabled with []{move:gastro-acid}, changed with []{move:role-play}, or traded away with []{move:skill-swap}.
		/// </summary>
		public static readonly Abilities TRUANT			= new Abilities(54,	"TRUANT");
		/// <summary>
		/// Strengthens physical moves to inflict 1.5× damage, but decreases their [accuracy]{mechanic:accuracy} to 0.8×.<para></para> This Pokémon's physical moves do 1.5× as much [regular damage]{mechanic:regular-damage}, but have 0.8× their usual [accuracy]{mechanic:accuracy}.
		/// 
		/// Special moves are unaffected.  Moves that do set damage, such as []{move:seismic-toss}, have their accuracy affected, but not their damage.
		/// 
		/// Overworld: If the lead Pokémon has this ability, higher-levelled Pokémon have their encounter rate increased.
		/// </summary>
		public static readonly Abilities HUSTLE			= new Abilities(55,	"HUSTLE");
		/// <summary>
		/// Has a 30% chance of [infatuating]{mechanic:infatuation} attacking Pokémon on contact.<para></para> Whenever a move makes contact with this Pokémon, the move's user has a 30% chance of being [infatuated]{mechanic:infatuation}.
		/// 
		/// Overworld: If the first Pokémon in the party has this ability, any wild Pokémon whose species can be either gender has a 2/3 chance of being set to the opposite gender, and a 1/3 chance of having a random gender as usual.
		/// </summary>
		public static readonly Abilities CUTE_CHARM			= new Abilities(56,	"CUTE_CHARM");
		/// <summary>
		/// Increases [Special Attack]{mechanic:special-attack} to 1.5× when a friendly Pokémon has []{ability:plus} or []{ability:minus}.<para></para> This Pokémon has 1.5× its [Special Attack]{mechanic:special-attack} if any friendly Pokémon has []{ability:plus} or []{ability:minus}.
		/// 
		/// This bonus does not count as a [stat modifier]{mechanic:stat-modifier}.  If either ability is disabled by []{move:gastro-acid}, both lose their effect.
		/// </summary>
		public static readonly Abilities PLUS			= new Abilities(57,	"PLUS");
		/// <summary>
		/// Increases [Special Attack]{mechanic:special-attack} to 1.5× when a friendly Pokémon has []{ability:plus} or []{ability:minus}.<para></para> This Pokémon has 1.5× its [Special Attack]{mechanic:special-attack} if any friendly Pokémon has []{ability:plus} or []{ability:minus}.
		/// 
		/// This bonus does not count as a [stat modifier]{mechanic:stat-modifier}.  If either ability is disabled by []{move:gastro-acid}, both lose their effect.
		/// </summary>
		public static readonly Abilities MINUS			= new Abilities(58,	"MINUS");
		/// <summary>
		/// Changes []{pokemon:castform}'s type and form to match the [weather]{mechanic:weather}.<para></para> During [rain]{mechanic:rain}, [strong sunlight]{mechanic:strong-sunlight}, or [hail]{mechanic:hail}, this Pokémon's type changes to []{type:water}, []{type:fire}, or []{type:ice}, respectively, and its form changes to match.
		/// 
		/// This ability has no effect for any Pokémon other than []{pokemon:castform}.
		/// 
		/// If the [weather]{mechanic:weather} ends or becomes anything that does not trigger this ability, or a Pokémon with []{ability:air-lock} or []{ability:cloud-nine} enters battle, this Pokémon's type and form revert to their default.  If this ability is lost or disabled, this Pokémon cannot change its current type and form until it regains its ability.
		/// </summary>
		public static readonly Abilities FORECAST			= new Abilities(59,	"FORECAST");
		/// <summary>
		/// Prevents a held item from being removed by other Pokémon.<para></para> This Pokémon's hold item cannot be removed by other Pokémon.
		/// 
		/// Damaging moves that would remove this Pokémon's item can still inflict damage against this Pokémon, e.g. []{move:knock-off} or []{move:pluck}.  This Pokémon can still use moves that involve the loss of its own item, e.g. []{move:fling} or []{move:trick}.
		/// 
		/// Overworld: If the lead Pokémon has this ability, the encounter rate while fishing is increased.
		/// </summary>
		public static readonly Abilities STICKY_HOLD			= new Abilities(60,	"STICKY_HOLD");
		/// <summary>
		/// Has a 33% chance of curing any [major status ailment]{mechanic:major-status-ailment} after each turn.<para></para> After each turn, this Pokémon has a 33% of being cured of any [major status ailment]{mechanic:major-status-ailment}.
		/// </summary>
		public static readonly Abilities SHED_SKIN			= new Abilities(61,	"SHED_SKIN");
		/// <summary>
		/// Increases [Attack]{mechanic:attack} to 1.5× with a [major status ailment]{mechanic:major-status-ailment}.<para></para> Whenever this Pokémon is [asleep]{mechanic:sleep}, [burned]{mechanic:burn}, [paralyzed]{mechanic:paralysis}, or [poisoned]{mechanic:poison}, it has 1.5× its [Attack]{mechanic:attack}.  This Pokémon is not affected by the usual Attack cut from a burn.
		/// 
		/// This bonus does not count as a [stat modifier]{mechanic:stat-modifier}.
		/// </summary>
		public static readonly Abilities GUTS			= new Abilities(62,	"GUTS");
		/// <summary>
		/// Increases [Defense]{mechanic:defense} to 1.5× with a [major status ailment]{mechanic:major-status-ailment}.<para></para> Whenever this Pokémon has a [major status ailment]{mechanic:major-status-ailment}, it has 1.5× its [Defense]{mechanic:defense}.
		/// 
		/// This bonus does not count as a [stat modifier]{mechanic:stat-modifier}.
		/// </summary>
		public static readonly Abilities MARVEL_SCALE			= new Abilities(63,	"MARVEL_SCALE");
		/// <summary>
		/// Damages opponents using leeching moves for as much as they would heal.<para></para> Whenever a Pokémon would heal after hitting this Pokémon with a leeching move like []{move:absorb}, it instead loses as many [HP]{mechanic:hp} as it would usually gain.
		/// 
		/// []{move:dream-eater} is unaffected.
		/// </summary>
		public static readonly Abilities LIQUID_OOZE			= new Abilities(64,	"LIQUID_OOZE");
		/// <summary>
		/// Strengthens []{type:grass} moves to inflict 1.5× damage at 1/3 max [HP]{mechanic:hp} or less.<para></para> When this Pokémon has 1/3 or less of its [HP]{mechanic:hp} remaining, its []{type:grass}-type moves inflict 1.5× as much [regular damage]{mechanic:regular-damage}.
		/// </summary>
		public static readonly Abilities OVERGROW			= new Abilities(65,	"OVERGROW");
		/// <summary>
		/// Strengthens []{type:fire} moves to inflict 1.5× damage at 1/3 max [HP]{mechanic:hp} or less.<para></para> When this Pokémon has 1/3 or less of its [HP]{mechanic:hp} remaining, its []{type:fire}-type moves inflict 1.5× as much [regular damage]{mechanic:regular-damage}.
		/// </summary>
		public static readonly Abilities BLAZE			= new Abilities(66,	"BLAZE");
		/// <summary>
		/// Strengthens []{type:water} moves to inflict 1.5× damage at 1/3 max [HP]{mechanic:hp} or less.<para></para> When this Pokémon has 1/3 or less of its [HP]{mechanic:hp} remaining, its []{type:water}-type moves inflict 1.5× as much [regular damage]{mechanic:regular-damage}.
		/// </summary>
		public static readonly Abilities TORRENT			= new Abilities(67,	"TORRENT");
		/// <summary>
		/// Strengthens []{type:bug} moves to inflict 1.5× damage at 1/3 max [HP]{mechanic:hp} or less.<para></para> When this Pokémon has 1/3 or less of its [HP]{mechanic:hp} remaining, its []{type:bug}-type moves inflict 1.5× as much [regular damage]{mechanic:regular-damage}.
		/// 
		/// Overworld: If the lead Pokémon has this ability, the wild encounter rate is increased.
		/// </summary>
		public static readonly Abilities SWARM			= new Abilities(68,	"SWARM");
		/// <summary>
		/// Protects against recoil damage.<para></para> This Pokémon does not receive recoil damage from its recoil moves.
		/// 
		/// []{move:struggle}'s recoil is unaffected.  This ability does not prevent crash damage from missing with []{move:jump-kick} or []{move:high-jump-kick}.
		/// </summary>
		public static readonly Abilities ROCK_HEAD			= new Abilities(69,	"ROCK_HEAD");
		/// <summary>
		/// Summons [strong sunlight]{mechanic:strong-sunlight} that lasts indefinitely upon entering battle.<para></para> The [weather]{mechanic:weather} changes to [strong sunlight]{mechanic:strong-sunlight} when this Pokémon enters battle and does not end unless cancelled by another weather condition.
		/// 
		/// If multiple Pokémon with this ability, []{ability:drizzle}, []{ability:sand-stream}, or []{ability:snow-warning} are sent out at the same time, the abilities will activate in order of [Speed]{mechanic:speed}, respecting []{move:trick-room}.  Each ability's weather will cancel the previous weather, and only the weather summoned by the slowest of the Pokémon will stay.
		/// </summary>
		public static readonly Abilities DROUGHT			= new Abilities(70,	"DROUGHT");
		/// <summary>
		/// Prevents opponents from fleeing or switching out.  Eluded by []{type:flying}-types and Pokémon in the air.<para></para> While this Pokémon is in battle, opposing Pokémon cannot flee or switch out.  []{type:flying}-type Pokémon and Pokémon in the air, e.g. due to []{ability:levitate} or []{move:magnet-rise}, are unaffected.
		/// 
		/// Pokémon with []{ability:run-away} can still flee.  Pokémon can still switch out with the use of a move or item.
		/// 
		/// Overworld: If the lead Pokémon has this ability, the wild encounter rate is doubled.
		/// </summary>
		public static readonly Abilities ARENA_TRAP			= new Abilities(71,	"ARENA_TRAP");
		/// <summary>
		/// Prevents [sleep]{mechanic:sleep}.<para></para> This Pokémon cannot be [asleep]{mechanic:sleep}.
		/// 
		/// This causes []{move:rest} to fail altogether.  If a Pokémon is asleep and acquires this ability, it will immediately wake up; this includes when regaining a lost ability upon leaving battle.
		/// 
		/// This ability functions identically to []{ability:insomnia} in battle.
		/// 
		/// Overworld: If the lead Pokémon has this ability, higher-levelled Pokémon have their encounter rate increased.
		/// </summary>
		public static readonly Abilities VITAL_SPIRIT			= new Abilities(72,	"VITAL_SPIRIT");
		/// <summary>
		/// Prevents stats from being [lowered]{mechanic:stat-modifier} by other Pokémon.<para></para> This Pokémon cannot have its stats [lowered]{mechanic:stat-modifier} by other Pokémon.
		/// 
		/// This ability does not prevent any stat losses other than [stat modifiers]{mechanic:stat-modifiers}, such as the [Speed]{mechanic:speed} cut from [paralysis]{mechanic:paralysis}; nor self-inflicted stat drops, such as the [Special Attack]{mechanic:special-attack} drop from []{move:overheat}; nor opponent-triggered stat boosts, such as the [Attack]{mechanic:attack} boost from []{move:swagger}.  This Pokémon can still be passed negative stat modifiers through []{move:guard-swap}, []{move:heart-swap}, or []{move:power-swap}.
		/// 
		/// This ability functions identically to []{ability:clear-body} in battle.
		/// 
		/// Overworld: If the lead Pokémon has this ability, the wild encounter rate is halved.
		/// </summary>
		public static readonly Abilities WHITE_SMOKE			= new Abilities(73,	"WHITE_SMOKE");
		/// <summary>
		/// Doubles [Attack]{mechanic:attack} in battle.<para></para> This Pokémon's [Attack]{mechanic:attack} is doubled in battle.
		/// 
		/// This bonus does not count as a [stat modifier]{mechanic:stat-modifier}.
		/// 
		/// This ability functions identically to []{ability:huge-power}.
		/// </summary>
		public static readonly Abilities PURE_POWER			= new Abilities(74,	"PURE_POWER");
		/// <summary>
		/// Protects against [critical hits]{mechanic:critical-hit}.<para></para> Moves cannot score [critical hits]{mechanic:critical-hit} against this Pokémon.
		/// 
		/// This ability functions identically to []{ability:battle-armor}.
		/// </summary>
		public static readonly Abilities SHELL_ARMOR			= new Abilities(75,	"SHELL_ARMOR");
		/// <summary>
		/// Negates all effects of [weather]{mechanic:weather}, but does not prevent the weather itself.<para></para> While this Pokémon is in battle, [weather]{mechanic:weather} can still be in play, but will not have any of its effects.
		/// 
		/// This ability functions identically to []{ability:cloud-nine}.
		/// </summary>
		public static readonly Abilities AIR_LOCK			= new Abilities(76,	"AIR_LOCK");
		/// <summary>
		/// Doubles [evasion]{mechanic:evasion} when [confused]{mechanic:confusion}.<para></para> When this Pokémon is [confused]{mechanic:confusion}, it has twice its [evasion]{mechanic:evasion}.
		/// </summary>
		public static readonly Abilities TANGLED_FEET			= new Abilities(77,	"TANGLED_FEET");
		/// <summary>
		/// Absorbs []{type:electric} moves, raising [Speed]{mechanic:speed} one [stage]{mechanic:stat-modifier}.<para></para> Whenever an []{type:electric}-type move hits this Pokémon, its [Speed]{mechanic:speed} rises one [stage]{mechanic:stat-modifier}, negating any other effect on it.
		/// 
		/// This ability will not take effect if this Pokémon is immune to Electric moves.  Electric moves will ignore this Pokémon's []{move:substitute}.
		/// 
		/// This effect includes non-damaging moves, i.e. []{move:thunder-wave}.
		/// </summary>
		public static readonly Abilities MOTOR_DRIVE			= new Abilities(78,	"MOTOR_DRIVE");
		/// <summary>
		/// Increases damage inflicted to 1.25× against Pokémon of the same gender, but decreases damage to 0.75× against the opposite gender.<para></para> This Pokémon inflicts 1.25× as much [regular damage]{mechanic:regular-damage} against Pokémon of the same gender and 0.75× as much regular damage against Pokémon of the opposite gender.
		/// 
		/// If either Pokémon is genderless, damage is unaffected.
		/// </summary>
		public static readonly Abilities RIVALRY			= new Abilities(79,	"RIVALRY");
		/// <summary>
		/// Raises [Speed]{mechanic:speed} one [stage]{mechanic:stat-modifier} upon [flinching]{mechanic:flinching}.<para></para> Whenever this Pokémon [flinches]{mechanic:flinching}, its [Speed]{mechanic:speed} rises one [stage]{mechanic:stat-modifier}.
		/// </summary>
		public static readonly Abilities STEADFAST			= new Abilities(80,	"STEADFAST");
		/// <summary>
		/// Increases evasion to 1.25× during [hail]{mechanic:hail}.  Protects against hail damage.<para></para> During [hail]{mechanic:hail}, this Pokémon has 1.25× its [evasion]{mechanic:evasion}, and it does not take hail damage regardless of type.
		/// 
		/// The evasion bonus does not count as a [stat modifier]{mechanic:stat-modifier}.
		/// 
		/// Overworld: If the lead Pokémon has this ability, the wild encounter rate is halved in snow.
		/// </summary>
		public static readonly Abilities SNOW_CLOAK			= new Abilities(81,	"SNOW_CLOAK");
		/// <summary>
		/// Makes the Pokémon eat any held Berry triggered by low [HP]{mechanic:hp} below 1/2 its max HP.<para></para> This Pokémon eats any held Berry triggered by low [HP]{mechanic:hp} when it falls below 50% of its HP, regardless of the Berry's usual threshold.
		/// </summary>
		public static readonly Abilities GLUTTONY			= new Abilities(82,	"GLUTTONY");
		/// <summary>
		/// Raises [Attack]{mechanic:attack} to the maximum of six [stages]{mechanic:stat-modifier} upon receiving a [critical hit]{mechanic:critical-hit}.<para></para> Whenever this Pokémon receives a [critical hit]{mechanic:critical-hit}, its [Attack]{mechanic:attack} rises to the maximum of 6 [stages]{mechanic:stat-modifier}.
		/// 
		/// This ability will still take effect if the critical hit is received by a []{move:substitute}.
		/// </summary>
		public static readonly Abilities ANGER_POINT			= new Abilities(83,	"ANGER_POINT");
		/// <summary>
		/// Doubles [Speed]{mechanic:speed} upon using or losing a held item.<para></para> When this Pokémon uses or loses its held item, its [Speed]{mechanic:speed} is doubled.  If it gains another item or leaves battle, this bonus is lost.
		/// 
		/// This includes when the Pokémon drops its item because of []{move:knock-off}.  This bonus does not count as a [stat modifier]{mechanic:stat-modifier}.  There is no notification when this ability takes effect.
		/// </summary>
		public static readonly Abilities UNBURDEN			= new Abilities(84,	"UNBURDEN");
		/// <summary>
		/// Halves damage from []{type:fire} moves and [burns]{mechanic:burn}.<para></para> This Pokémon takes half as much damage from []{type:fire}-type moves and [burns]{mechanic:burn}.
		/// </summary>
		public static readonly Abilities HEATPROOF			= new Abilities(85,	"HEATPROOF");
		/// <summary>
		/// Doubles the Pokémon's [stat modifiers]{mechanic:stat-modifiers}.  These doubled modifiers are still capped at -6 or 6 stages.<para></para> Each stage of this Pokémon's [stat modifiers]{mechanic:stat-modifiers} acts as two stages.  These doubled stages are still limited to a minimum of -6 and a maximum of 6.
		/// 
		/// This Pokémon can still accumulate less than -3 or more than 3 stages of stat modifiers, even though the extra ones have no effect after doubling.
		/// </summary>
		public static readonly Abilities SIMPLE			= new Abilities(86,	"SIMPLE");
		/// <summary>
		/// Causes 1/8 max [HP]{mechanic:hp} in damage each turn during [strong sunlight]{mechanic:strong-sunlight}, but heals for 1/8 max HP during [rain]{mechanic:rain}.  Increases damage from []{type:fire} moves to 1.25×, but absorbs []{type:water} moves, healing for 1/4 max HP.<para></para> This Pokémon takes 1/8 of its maximum HP in damage after each turn during [strong sunlight]{mechanic:strong-sunlight}, but it heals for 1/8 of its HP each turn during [rain]{mechanic:rain}.  This Pokémon takes 1.25× as much damage from []{type:fire}-type moves, but whenever a []{type:water} move hits it, it heals for 1/4 its maximum HP instead.
		/// </summary>
		public static readonly Abilities DRY_SKIN			= new Abilities(87,	"DRY_SKIN");
		/// <summary>
		/// Raises the attack stat corresponding to the opponents' weaker defense one [stage]{mechanic:stat-modifier} upon entering battle.<para></para> When this Pokémon enters battle, its [Attack]{mechanic:attack} or [Special Attack]{mechanic:special-attack}, whichever corresponds to its opponents' weaker total defensive stat, rises one [stage]{mechanic:stat-modifier}.  In the event of a tie, Special Attack is raised.
		/// 
		/// This ability also takes effect when acquired during a battle.
		/// </summary>
		public static readonly Abilities DOWNLOAD			= new Abilities(88,	"DOWNLOAD");
		/// <summary>
		/// Strengthens punch-based moves to 1.2× their power.<para></para> Moves flagged as being punch-based have 1.2× their base power for this Pokémon.
		/// 
		/// []{move:sucker-punch} is not flagged as punch-based; its original, Japanese name only means "surprise attack".
		/// </summary>
		public static readonly Abilities IRON_FIST			= new Abilities(89,	"IRON_FIST");
		/// <summary>
		/// Heals for 1/8 max [HP]{mechanic:hp} after each turn when [poisoned]{mechanic:poison} in place of damage.<para></para> If this Pokémon is [poisoned]{mechanic:poison}, it will heal for 1/8 of its maximum [HP]{mechanic:hp} after each turn rather than taking damage.  This includes bad poison.
		/// </summary>
		public static readonly Abilities POISON_HEAL			= new Abilities(90,	"POISON_HEAL");
		/// <summary>
		/// Increases the [same-type attack bonus]{mechanic:same-type-attack-bonus} from 1.5× to 2×.<para></para> This Pokémon inflicts twice as much damage with moves whose types match its own, rather than the usual [same-type attack bonus]{mechanic:same-type-attack-bonus} of 1.5×.
		/// </summary>
		public static readonly Abilities ADAPTABILITY			= new Abilities(91,	"ADAPTABILITY");
		/// <summary>
		/// Extends two-to-five-hit moves and []{move:triple-kick} to their full length every time.<para></para> This Pokémon always hits five times with two-to-five-hit moves, such as []{move:icicle-spear}.  It also bypasses the accuracy checks on []{move:triple-kick}'s second and third hits.
		/// </summary>
		public static readonly Abilities SKILL_LINK			= new Abilities(92,	"SKILL_LINK");
		/// <summary>
		/// Cures any [major status ailment]{mechanic:major-status-ailment} after each turn during [rain]{mechanic:rain}.<para></para> This Pokémon is cured of any [major status ailment]{mechanic:major-status-ailment} after each turn during [rain]{mechanic:rain}.
		/// </summary>
		public static readonly Abilities HYDRATION			= new Abilities(93,	"HYDRATION");
		/// <summary>
		/// Increases [Special Attack]{mechanic:special-attack} to 1.5× but costs 1/8 max [HP]{mechanic:hp} after each turn during [strong sunlight]{mechanic:strong-sunlight}.<para></para> During [strong sunlight]{mechanic:strong-sunlight}, this Pokémon has 1.5× its [Special Attack]{mechanic:special-attack} but takes 1/8 of its maximum [HP]{mechanic:hp} in damage after each turn.
		/// </summary>
		public static readonly Abilities SOLAR_POWER			= new Abilities(94,	"SOLAR_POWER");
		/// <summary>
		/// Increases [Speed]{mechanic:speed} to 1.5× with a [major status ailment]{mechanic:major-status-ailment}.<para></para> Whenever this Pokémon has a [major status ailment]{mechanic:major-status-ailment}, it has 1.5× its [Speed]{mechanic:speed}.  This Pokémon is not affected by the usual Speed cut from [paralysis]{mechanic:paralysis}.
		/// 
		/// Overworld: If the lead Pokémon has this ability, the wild encounter rate is halved.
		/// </summary>
		public static readonly Abilities QUICK_FEET			= new Abilities(95,	"QUICK_FEET");
		/// <summary>
		/// Makes the Pokémon's moves all act []{type:normal}-type.<para></para> This Pokémon's moves all act as if they were []{type:normal}-type.
		/// 
		/// Moves that inflict [typeless damage]{mechanic:typeless-damage} do so as usual.  Moves of variable type, such as []{move:hidden-power}, are affected.  They otherwise work as usual, however; []{move:weather-ball}, for example, is always forced to be Normal, but it still has doubled power and looks different during [weather]{mechanic:weather}.
		/// 
		/// As []{move:thunder-wave} is prevented by immunities, unlike most non-damaging moves, it does not affect []{type:ghost}-type Pokémon under the effect of this ability.
		/// </summary>
		public static readonly Abilities NORMALIZE			= new Abilities(96,	"NORMALIZE");
		/// <summary>
		/// Strengthens [critical hits]{mechanic:critical-hit} to inflict 3× damage rather than 2×.<para></para> This Pokémon inflicts triple damage with [critical hits]{mechanic:critical-hit}, rather than the usual double damage.
		/// </summary>
		public static readonly Abilities SNIPER			= new Abilities(97,	"SNIPER");
		/// <summary>
		/// Protects against damage not directly caused by a move.<para></para> This Pokémon is immune to damage not directly caused by a move.
		/// 
		/// For example, this Pokémon takes no damage from from [weather]{mechanic:weather}, recoil, [status ailments]{mechanic:status-ailments}, or []{move:spikes}, but it still suffers from the [Attack]{mechanic:attack} cut when [burned]{mechanic:burn}, and a []{item:life-orb} will still power up this Pokémon's moves without damaging it.  Anything that directly depends on such damage will also not happen; for example, []{move:leech-seed} will neither hurt this Pokémon nor heal the opponent, and Pokémon with a []{item:jaboca-berry} or []{item:rowap-berry} will not consume the berry when hit by this Pokémon.
		/// 
		/// The following are unaffected: []{move:struggle}, []{move:pain-split} (whether used by or against this Pokémon), []{move:belly-drum}, []{move:substitute}, []{move:curse}, moves that knock the user out, and damage from [confusion]{mechanic:confusion}.
		/// 
		/// This Pokémon will neither lose nor regain [HP]{mechanic:hp} if it drains HP from a Pokémon with []{ability:liquid-ooze}.
		/// 
		/// If this Pokémon is [badly poisoned]{mechanic:poison}, the poison counter is still increased each turn; if the Pokémon loses this ability, it will begin taking as much damage as it would be if it had been taking increasing damage each turn.
		/// </summary>
		public static readonly Abilities MAGIC_GUARD			= new Abilities(98,	"MAGIC_GUARD");
		/// <summary>
		/// Ensures all moves used by and against the Pokémon hit.<para></para> Moves used by or against this Pokémon never miss.
		/// 
		/// One-hit KO moves are unaffected.  Moves affected by this ability can hit Pokémon during the preparation turn of moves like []{move:dig} or []{move:fly}.
		/// 
		/// Overworld: If the lead Pokémon has this ability, the wild encounter rate is doubled.
		/// </summary>
		public static readonly Abilities NO_GUARD			= new Abilities(99,	"NO_GUARD");
		/// <summary>
		/// Makes the Pokémon move last within its move's priority bracket.<para></para> This Pokémon moves last within its priority bracket.
		/// 
		/// Multiple Pokémon with this ability move in order of [Speed]{mechanic:speed} amongst themselves.
		/// 
		/// The []{item:full-incense} and []{item:lagging-tail} take precedence over this ability; that is, Pokémon with these items move after Pokémon with this ability.  Pokémon with both this ability and one of these items are delayed as much as if they had only the item.
		/// 
		/// This ability works as usual during []{move:trick-room}: Pokémon with this ability will move in reverse order of Speed after Pokémon without it.
		/// </summary>
		public static readonly Abilities STALL			= new Abilities(100,	"STALL");
		/// <summary>
		/// Strengthens moves of 60 base power or less to 1.5× their power.<para></para> This Pokémon's moves have 1.5× their power if their base power is 60 or less.
		/// 
		/// This includes moves of variable power, such as []{move:hidden-power} and []{move:magnitude}, when their power is 60 or less.  []{move:helping-hand}'s power boost is taken into account for any move, as is []{move:defense-curl}'s power boost for []{move:rollout}.
		/// </summary>
		public static readonly Abilities TECHNICIAN			= new Abilities(101,	"TECHNICIAN");
		/// <summary>
		/// Protects against [major status ailments]{mechanic:major-status-ailments} during [strong sunlight]{mechanic:strong-sunlight}.<para></para> This Pokémon cannot be given a [major status ailment]{mechanic:major-status-ailment} during [strong sunlight]{mechanic:strong-sunlight}.
		/// 
		/// This ability does not heal prior status ailments.  []{move:rest} will fail altogether with this ability in effect.  []{move:yawn} will immediately fail if used on this Pokémon during strong sunlight, and an already-used Yawn will fail if the weather turns to strong sunlight in the meantime.
		/// </summary>
		public static readonly Abilities LEAF_GUARD			= new Abilities(102,	"LEAF_GUARD");
		/// <summary>
		/// Prevents the Pokémon from using its held item in battle.<para></para> In battle, this Pokémon cannot use its held item, nor will the item have any passive effect on the battle, positive or negative.  This Pokémon also cannot use []{move:fling}.
		/// 
		/// The [Speed]{mechanic:speed} cut from the []{item:iron-ball} and the effort items (the []{item:macho-brace}, []{item:power-weight}, []{item:power-bracer}, []{item:power-belt}, []{item:power-lens}, []{item:power-band}, and []{item:power-anklet}) is unaffected.  Items that do not directly affect the battle, such as the []{item:exp-share}, the []{item:amulet-coin}, or the []{item:soothe-bell}, work as usual.  All held items work as usual out of battle.
		/// 
		/// Other moves that use the held item, such as []{move:natural-gift} and []{move:switcheroo}, work as usual.
		/// </summary>
		public static readonly Abilities KLUTZ			= new Abilities(103,	"KLUTZ");
		/// <summary>
		/// Bypasses targets' abilities if they could hinder or prevent a move.<para></para> This Pokémon's moves completely ignore abilities that could hinder or prevent their effect on the target.
		/// 
		/// For example, this Pokémon's moves ignore abilities that would fully negate them, such as []{ability:water-absorb}; abilities that would prevent any of their effects, such as []{ability:clear-body}, []{ability:shell-armor}, or []{ability:sticky-hold}; and abilities that grant any general protective benefit, such as []{ability:simple}, []{ability:snow-cloak}, or []{ability:thick-fat}.  If an ability could either hinder or help this Pokémon's moves, e.g. []{ability:dry-skin} or []{ability:unaware}, the ability is ignored either way.
		/// 
		/// Abilities that do not fit this description, even if they could hinder moves in some other way, are not affected.  For example, []{ability:cursed-body} only affects potential future uses of the move, while []{ability:liquid-ooze} and []{ability:shadow-tag} can only hinder a move's effect on the user.  This ablity cannot ignore type or form changes granted by abilities, for example []{ability:color-change} or []{ability:forecast}; nor effects that were caused by abilities but are no longer tied to an ability, such as the rain from []{ability:drizzle}.  This ability cannot ignore []{ability:multitype} at all.
		/// 
		/// An ability ignored by this ability is only nullified while the move is being used.  For example, this Pokémon's moves can [paralyze]{mechanic:paralysis} a Pokémon with []{ability:limber}, but Limber will activate and heal the paralysis immediately thereafter, and this Pokémon's []{move:spikes} are not affected by this ability after they have been placed.
		/// 
		/// When this Pokémon enters battle, all participating trainers are notified that it has this ability.
		/// 
		/// This ability functions identically to []{ability:teravolt} and []{ability:turboblaze}.
		/// </summary>
		public static readonly Abilities MOLD_BREAKER			= new Abilities(104,	"MOLD_BREAKER");
		/// <summary>
		/// Raises moves' [critical hit]{mechanic:critical-hit} rates one stage.<para></para> This Pokémon's moves have [critical hit]{mechanic:critical-hit} rates one stage higher than normal.
		/// </summary>
		public static readonly Abilities SUPER_LUCK			= new Abilities(105,	"SUPER_LUCK");
		/// <summary>
		/// Damages the attacker for 1/4 its max [HP]{mechanic:hp} when knocked out by a contact move.<para></para> When this Pokémon is knocked out by a move that makes contact, the move's user takes 1/4 its maximum [HP]{mechanic:hp} in damage.
		/// </summary>
		public static readonly Abilities AFTERMATH			= new Abilities(106,	"AFTERMATH");
		/// <summary>
		/// Notifies all trainers upon entering battle if an opponent has a [super-effective]{mechanic:super-effective} move, []{move:self-destruct}, []{move:explosion}, or a one-hit KO move.<para></para> When this Pokémon enters battle, if one of its opponents has a move that is [super effective]{mechanic:super-effective} against it, []{move:self-destruct}, []{move:explosion}, or a one-hit knockout move, all participating trainers are notified.
		/// 
		/// The move itself is not revealed; only that there is such a move.  Moves that inflict [typeless damage]{mechanic:typeless-damage}, such as []{move:future-sight}, and moves of variable type, such as []{move:hidden-power}, count as their listed types.  []{move:counter}, []{move:metal-burst}, []{move:mirror-coat}, and one-hit KO moves to which this Pokémon is immune do not trigger this ability.
		/// </summary>
		public static readonly Abilities ANTICIPATION			= new Abilities(107,	"ANTICIPATION");
		/// <summary>
		/// Reveals the opponents' strongest move upon entering battle.<para></para> When this Pokémon enters battle, it reveals the move with the highest base power known by any opposing Pokémon to all participating trainers.
		/// 
		/// In the event of a tie, one is chosen at random.
		/// 
		/// Moves without a listed base power are assigned one as follows:
		/// 
		/// Power | Moves
		/// ----: | -----
		///   160 | One-hit KO moves: []{move:fissure}, []{move:guillotine}, []{move:horn-drill}, and []{move:sheer-cold}
		///   120 | Counter moves: []{move:counter}, []{move:metal-burst}, and []{move:mirror-coat}
		///    80 | Variable power or set damage: []{move:crush-grip}, []{move:dragon-rage}, []{move:electro-ball}, []{move:endeavor}, []{move:final-gambit}, []{move:flail}, []{move:frustration}, []{move:grass-knot}, []{move:gyro-ball}, []{move:heat-crash}, []{move:heavy-slam}, []{move:hidden-power}, []{move:low-kick}, []{move:natural-gift}, []{move:night-shade}, []{move:psywave}, []{move:return}, []{move:reversal}, []{move:seismic-toss}, []{move:sonic-boom}, []{move:trump-card}, and []{move:wring-out}
		///     0 | Any such move not listed
		/// </summary>
		public static readonly Abilities FOREWARN			= new Abilities(108,	"FOREWARN");
		/// <summary>
		/// Ignores other Pokémon's stat modifiers for damage and accuracy calculation.<para></para> This Pokémon ignores other Pokémon's [stat modifiers]{mechanic:stat-modifiers} for the purposes of damage and accuracy calculation.
		/// 
		/// Effectively, this affects modifiers of every stat except [Speed]{mechanic:speed}.
		/// 
		/// The power of []{move:punishment} and []{move:stored-power} is calculated as usual.  When this Pokémon hurts itself in [confusion]{mechanic:confusion}, its stat modifiers affect damage as usual.
		/// </summary>
		public static readonly Abilities UNAWARE			= new Abilities(109,	"UNAWARE");
		/// <summary>
		/// Doubles damage inflicted with [not-very-effective]{mechanic:not-very-effective} moves.<para></para> This Pokémon deals twice as much damage with moves that are [not very effective]{mechanic:not-very-effective} against the target.
		/// </summary>
		public static readonly Abilities TINTED_LENS			= new Abilities(110,	"TINTED_LENS");
		/// <summary>
		/// Decreases damage taken from [super-effective]{mechanic:super-effective} moves by 1/4.<para></para> This Pokémon takes 0.75× as much damage from moves that are [super effective]{mechanic:super-effective} against it.
		/// 
		/// This ability functions identically to []{ability:solid-rock}.
		/// </summary>
		public static readonly Abilities FILTER			= new Abilities(111,	"FILTER");
		/// <summary>
		/// Halves [Attack]{mechanic:attack} and [Speed]{mechanic:speed} for five turns upon entering battle.<para></para> This Pokémon's [Attack]{mechanic:attack} and [Speed]{mechanic:speed} are halved for five turns upon entering battle.
		/// 
		/// This ability also takes effect when acquired during battle.  If this Pokémon loses its ability before the five turns are up, its Attack and Speed return to normal; if it then regains this ability without leaving battle, its Attack and Speed are halved again, but the counter keeps counting from where it was.
		/// </summary>
		public static readonly Abilities SLOW_START			= new Abilities(112,	"SLOW_START");
		/// <summary>
		/// Lets the Pokémon's []{type:normal} and []{type:fighting} moves hit []{type:ghost} Pokémon.<para></para> This Pokémon ignores []{type:ghost}-type Pokémon's immunity to []{type:normal}- and []{type:fighting}-type moves.
		/// 
		/// Ghost Pokémon's other types affect damage as usual.
		/// </summary>
		public static readonly Abilities SCRAPPY			= new Abilities(113,	"SCRAPPY");
		/// <summary>
		/// Redirects single-target []{type:water} moves to this Pokémon where possible.  Absorbs Water moves, raising [Special Attack]{mechanic:special-attack} one [stage]{mechanic:stat-modifier}.<para></para> All other Pokémon's single-target []{type:water}-type moves are redirected to this Pokémon, if it is an eligible target.  Other Pokémon's Water moves raise this Pokémon's [Special Attack]{mechanic:special-attack} one [stage]{mechanic:stat-modifier}, negating any other effect on it, and cannot miss it.
		/// 
		/// If the move's intended target also has this ability, the move is not redirected.  When multiple Pokémon with this ability are possible targets for redirection, the move is redirected to the one with the highest [Speed]{mechanic:speed} stat, or, in the case of a tie, to a random tied Pokémon.  []{move:follow-me} takes precedence over this ability.
		/// </summary>
		public static readonly Abilities STORM_DRAIN			= new Abilities(114,	"STORM_DRAIN");
		/// <summary>
		/// Heals for 1/16 max [HP]{mechanic:hp} after each turn during hail.  Protects against hail damage.<para></para> This Pokémon heals for 1/16 of its maximum [HP]{mechanic:hp} after each turn during [hail]{mechanic:hail}, and it does not take hail damage regardless of type.
		/// </summary>
		public static readonly Abilities ICE_BODY			= new Abilities(115,	"ICE_BODY");
		/// <summary>
		/// Decreases damage taken from [super-effective]{mechanic:super-effective} moves by 1/4.<para></para> This Pokémon takes 0.75× as much damage from moves that are [super effective]{mechanic:super-effective} against it.
		/// 
		/// This ability functions identically to []{ability:filter}.
		/// </summary>
		public static readonly Abilities SOLID_ROCK			= new Abilities(116,	"SOLID_ROCK");
		/// <summary>
		/// Summons [hail]{mechanic:hail} that lasts indefinitely upon entering battle.<para></para> The [weather]{mechanic:weather} changes to [hail]{mechanic:hail} when this Pokémon enters battle and does not end unless cancelled by another weather condition.
		/// 
		/// If multiple Pokémon with this ability, []{ability:drizzle}, []{ability:drought}, or []{ability:sand-stream} are sent out at the same time, the abilities will activate in order of [Speed]{mechanic:speed}, respecting []{move:trick-room}.  Each ability's weather will cancel the previous weather, and only the weather summoned by the slowest of the Pokémon will stay.
		/// </summary>
		public static readonly Abilities SNOW_WARNING			= new Abilities(117,	"SNOW_WARNING");
		/// <summary>
		/// The Pokémon may pick up []{item:honey} after battle.<para></para> This Pokémon has a chance of picking up []{item:honey} after each battle.  This chance starts at 5% and rises another 5% after every tenth level: 5% from level 1–10, 10% from 11–20, and so on, up to 50% from 91–100.
		/// 
		/// This ability is checked after the battle ends, at which point any temporary ability changes have worn off.
		/// </summary>
		public static readonly Abilities HONEY_GATHER			= new Abilities(118,	"HONEY_GATHER");
		/// <summary>
		/// Reveals an opponent's held item upon entering battle.<para></para> When this Pokémon enters battle, it reveals an opposing Pokémon's held item to all participating trainers.
		/// 
		/// In a double battle, if one opponent has an item, this Pokémon will Frisk that Pokémon; if both have an item, it will Frisk one at random.
		/// </summary>
		public static readonly Abilities FRISK			= new Abilities(119,	"FRISK");
		/// <summary>
		/// Strengthens recoil moves to 1.2× their power.<para></para> This Pokémon's recoil moves and crash moves have 1.2× their base power.
		/// 
		/// []{move:struggle} is unaffected.
		/// 
		/// The "crash moves" are the moves that damage the user upon missing: []{move:jump-kick} and []{move:high-jump-kick}.
		/// </summary>
		public static readonly Abilities RECKLESS			= new Abilities(120,	"RECKLESS");
		/// <summary>
		/// Changes []{pokemon:arceus}'s type and form to match its held Plate.<para></para> If this Pokémon is holding an elemental Plate, its type and form change to match the Plate.
		/// 
		/// This Pokémon's held item, whether or not it is a Plate, cannot be taken by []{move:covet} or []{move:thief}, nor removed by []{move:knock-off}, nor traded by []{move:switcheroo} or []{move:trick}.  Covet, Thief, and Knock Off still inflict damage against this Pokémon.  Unlike with []{ability:sticky-hold}, this Pokémon cannot use []{move:fling}, Switcheroo, or Trick to lose its item itself, nor gain an item through Switcheroo or Trick if it does not have one.
		/// 
		/// This ability has no effect for any Pokémon other than []{pokemon:arceus}.  This ability cannot be traded with []{move:skill-swap}, nor copied with []{move:role-play} or []{ability:trace}, nor disabled with []{move:gastro-acid}, nor changed with []{move:worry-seed}.  This Pokémon cannot use Skill Swap or Role Play to lose its ability itself.  []{ability:mold-breaker} cannot ignore this ability.
		/// 
		/// If a Pokémon [Transforms]{move:transform} into an Arceus with this ability, it will Transform into Arceus's default, []{type:normal}-type form.  If the Transforming Pokémon is holding a Plate, this ability will then activate and change the Pokémon into the corresponding form.
		/// </summary>
		public static readonly Abilities MULTITYPE			= new Abilities(121,	"MULTITYPE");
		/// <summary>
		/// Increases friendly Pokémon's [Attack]{mechanic:attack} and [Special Defense]{mechanic:special-defense} to 1.5× during [strong sunlight]{mechanic:strong-sunlight}.<para></para> Friendly Pokémon have 1.5× their [Attack]{mechanic:attack} and [Special Defense]{mechanic:special-defense} during [strong sunlight]{mechanic:strong-sunlight} if any friendly Pokémon has this ability.
		/// 
		/// Unlike []{ability:forecast}, []{ability:multitype}, and []{ability:zen-mode}, this ability is not tied to its Pokémon's form change; []{pokemon:cherrim} will switch between its forms even if it loses this ability.  As such, this ability also works if obtained by a Pokémon other than Cherrim.
		/// </summary>
		public static readonly Abilities FLOWER_GIFT			= new Abilities(122,	"FLOWER_GIFT");
		/// <summary>
		/// Damages [sleeping]{mechanic:sleep} opponents for 1/8 their max [HP]{mechanic:hp} after each turn.<para></para> Opposing Pokémon take 1/8 of their maximum [HP]{mechanic:hp} in damage after each turn while they are [asleep]{mechanic:asleep}.
		/// </summary>
		public static readonly Abilities BAD_DREAMS			= new Abilities(123,	"BAD_DREAMS");
		/// <summary>
		/// Steals attacking Pokémon's held items on contact.<para></para> Whenever a move makes contact with this Pokémon, if it does not have a held item, it steals the attacker's held item.
		/// 
		/// This Pokémon cannot steal upon being knocked out.  It can steal if the attacker has a []{move:substitute}, but cannot steal when its own Substitute is hit.  If a move hits multiple times, only the last hit triggers this ability.  If this Pokémon is wild, it cannot steal from a trained Pokémon.
		/// </summary>
		public static readonly Abilities PICKPOCKET			= new Abilities(124,	"PICKPOCKET");
		/// <summary>
		/// Strengthens moves with extra effects to 1.3× their power, but prevents their extra effects.<para></para> This Pokémon's moves with extra effects have 1.3× their power, but lose their extra effects.
		/// 
		/// An effect chance is a move's chance to inflict a status ailment, cause a stat change, or make the target flinch in addition to the move's main effect. For example, []{move:thunder-shock}'s paralysis is an extra effect, but []{move:thunder-wave}'s is not, nor are []{move:knock-off}'s item removal and []{move:air-cutter}'s increased [critical hit]{mechanic:critical-hit} rate.
		/// 
		/// Moves that lower the user's stats are unaffected.
		/// </summary>
		public static readonly Abilities SHEER_FORCE			= new Abilities(125,	"SHEER_FORCE");
		/// <summary>
		/// Inverts [stat changes]{mechanic:stat-modifiers}.<para></para> Whenever this Pokémon's stats would be [raised]{mechanic:stat-modifiers}, they are instead lowered by the same amount, and vice versa.
		/// </summary>
		public static readonly Abilities CONTRARY			= new Abilities(126,	"CONTRARY");
		/// <summary>
		/// Prevents opposing Pokémon from eating held Berries.<para></para> Opposing Pokémon cannot eat held Berries while this Pokémon is in battle.
		/// 
		/// Affected Pokémon can still use []{move:bug-bite} or []{move:pluck} to eat a target's Berry.
		/// </summary>
		public static readonly Abilities UNNERVE			= new Abilities(127,	"UNNERVE");
		/// <summary>
		/// Raises [Attack]{mechanic:attack} two [stages]{mechanic:stat-modifier} upon having any stat lowered.<para></para> When any of this Pokémon's stats are [lowered]{mechanic:stat-modifier}, its [Attack]{mechanic:attack} rises by two stages.
		/// 
		/// If multiple stats are lowered at once, this ability takes effect with each stat lowered.
		/// </summary>
		public static readonly Abilities DEFIANT			= new Abilities(128,	"DEFIANT");
		/// <summary>
		/// Halves [Attack]{mechanic:attack} and [Special Attack]{mechanic:special-attack} at 50% max [HP]{mechanic:hp} or less.<para></para> This Pokémon's [Attack]{mechanic:attack} and [Special Attack]{mechanic:special-attack} are halved when it has half its [HP]{mechanic:hp} or less.
		/// </summary>
		public static readonly Abilities DEFEATIST			= new Abilities(129,	"DEFEATIST");
		/// <summary>
		/// Has a 30% chance of [Disabling]{move:disable} any move that hits the Pokémon.<para></para> Moves that hit this Pokémon have a 30% chance of being [Disabled]{move:disable} afterward.
		/// </summary>
		public static readonly Abilities CURSED_BODY			= new Abilities(130,	"CURSED_BODY");
		/// <summary>
		/// Has a 30% chance of curing each adjacent ally of any [major status ailment]{mechanic:major-status-ailment} after each turn.<para></para> Friendly Pokémon next to this Pokémon in double and triple battles each have a 30% chance of being cured of any [major status ailment]{mechanic:major-status-ailment} after each turn.
		/// </summary>
		public static readonly Abilities HEALER			= new Abilities(131,	"HEALER");
		/// <summary>
		/// Decreases all direct damage taken by friendly Pokémon to 0.75×.<para></para> All friendly Pokémon take 0.75× as much direct damage from moves while this Pokémon is in battle.
		/// 
		/// This effect stacks if multiple allied Pokémon have it.
		/// </summary>
		public static readonly Abilities FRIEND_GUARD			= new Abilities(132,	"FRIEND_GUARD");
		/// <summary>
		/// Raises [Speed]{mechanic:speed} and lowers [Defense]{mechanic:defense} by one stage each upon being hit by a physical move.<para></para> Whenever a physical move hits this Pokémon, its [Speed]{mechanic:speed} rises one stage and its [Defense]{mechanic:defense} falls one stage.
		/// 
		/// This ability triggers on every hit of a multiple-hit move.
		/// </summary>
		public static readonly Abilities WEAK_ARMOR			= new Abilities(133,	"WEAK_ARMOR");
		/// <summary>
		/// Doubles the Pokémon's weight.<para></para> This Pokémon has double the usual weight for its species.
		/// </summary>
		public static readonly Abilities HEAVY_METAL			= new Abilities(134,	"HEAVY_METAL");
		/// <summary>
		/// Halves the Pokémon's weight.<para></para> This Pokémon has half the usual weight for its species.
		/// </summary>
		public static readonly Abilities LIGHT_METAL			= new Abilities(135,	"LIGHT_METAL");
		/// <summary>
		/// Halves damage taken from full [HP]{mechanic:hp}.<para></para> This Pokémon takes half as much damage when it is hit having full [HP]{mechanic:hp}.
		/// </summary>
		public static readonly Abilities MULTISCALE			= new Abilities(136,	"MULTISCALE");
		/// <summary>
		/// Increases [Attack]{mechanic:attack} to 1.5× when [poisoned]{mechanic:poison}.<para></para> This Pokémon has 1.5× its [Attack]{mechanic:attack} when [poisoned]{mechanic:poison}.
		/// </summary>
		public static readonly Abilities TOXIC_BOOST			= new Abilities(137,	"TOXIC_BOOST");
		/// <summary>
		/// Increases [Special Attack]{mechanic:special-attack} to 1.5× when [burned]{mechanic:burn}.<para></para> This Pokémon has 1.5× its [Special Attack]{mechanic:special-attack} when [burned]{mechanic:burn}.
		/// </summary>
		public static readonly Abilities FLARE_BOOST			= new Abilities(138,	"FLARE_BOOST");
		/// <summary>
		/// Has a 50% chance of restoring a used Berry after each turn if the Pokémon has held no items in the meantime.<para></para> After each turn, if the last item this Pokémon consumed was a Berry and it is not currently holding an item, it has a 50% chance of regaining that Berry, or a 100% chance during [strong sunlight]{mechanic:strong-sunlight}.
		/// </summary>
		public static readonly Abilities HARVEST			= new Abilities(139,	"HARVEST");
		/// <summary>
		/// Protects against friendly Pokémon's damaging moves.<para></para> This Pokémon does not take damage from friendly Pokémon's moves, including single-target moves aimed at it.
		/// </summary>
		public static readonly Abilities TELEPATHY			= new Abilities(140,	"TELEPATHY");
		/// <summary>
		/// Raises a random stat two [stages]{mechanic:stat-modifier} and lowers another one stage after each turn.<para></para> After each turn, one of this Pokémon's stats at random rises two [stages]{mechanic:stat-modifier}, and another falls one stage.
		/// 
		/// If a stat is already at 6 or -6 stages, it will not be chosen to be increased or decreased, respectively.
		/// </summary>
		public static readonly Abilities MOODY			= new Abilities(141,	"MOODY");
		/// <summary>
		/// Protects against damage from [weather]{mechanic:weather}.<para></para> This Pokémon does not take damage from [weather]{mechanic:weather}.
		/// </summary>
		public static readonly Abilities OVERCOAT			= new Abilities(142,	"OVERCOAT");
		/// <summary>
		/// Has a 30% chance of [poisoning]{mechanic:poison} target Pokémon upon contact.<para></para> This Pokémon's contact moves have a 30% chance of [poisoning]{mechanic:poison} the target with each hit.
		/// 
		/// This counts as an extra effect for the purposes of []{ability:shield-dust}.  This ability takes effect before []{ability:mummy}.
		/// </summary>
		public static readonly Abilities POISON_TOUCH			= new Abilities(143,	"POISON_TOUCH");
		/// <summary>
		/// Heals for 1/3 max [HP]{mechanic:hp} upon switching out.<para></para> This Pokémon regains 1/3 of its maximum [HP]{mechanic:hp} when it is switched out for another Pokémon under any circumstances other than having fainted.
		/// 
		/// This ability does not take effect when a battle ends.
		/// </summary>
		public static readonly Abilities REGENERATOR			= new Abilities(144,	"REGENERATOR");
		/// <summary>
		/// Protects against [Defense]{mechanic:defense} drops.<para></para> This Pokémon's [Defense]{mechanic:defense} cannot be [lowered]{mechanic:stat-modifiers} by other Pokémon.
		/// 
		/// This Pokémon can still be passed negative Defense modifiers through []{move:heart-swap} or []{move:guard-swap}.
		/// </summary>
		public static readonly Abilities BIG_PECKS			= new Abilities(145,	"BIG_PECKS");
		/// <summary>
		/// Doubles [Speed]{mechanic:speed} during a [sandstorm]{mechanic:sandstorm}.  Protects against sandstorm damage.<para></para> This Pokémon's [Speed]{mechanic:speed} is doubled during a [sandstorm]{mechanic:sandstorm}, and it does not take sandstorm damage, regardless of type.
		/// </summary>
		public static readonly Abilities SAND_RUSH			= new Abilities(146,	"SAND_RUSH");
		/// <summary>
		/// Lowers incoming non-damaging moves' base [accuracy]{mechanic:accuracy} to exactly 50%.<para></para> Non-damaging moves have exactly 50% base [accuracy]{mechanic:accuracy} against this Pokémon.
		/// </summary>
		public static readonly Abilities WONDER_SKIN			= new Abilities(147,	"WONDER_SKIN");
		/// <summary>
		/// Strengthens moves to 1.3× their power when moving last.<para></para> This Pokémon's moves have 1.3× their power when it moves last in a turn.
		/// 
		/// []{move:future-sight} and []{move:doom-desire} are unaffected.
		/// </summary>
		public static readonly Abilities ANALYTIC			= new Abilities(148,	"ANALYTIC");
		/// <summary>
		/// Takes the appearance of the last conscious party Pokémon upon being sent out until hit by a damaging move.<para></para> This Pokémon, upon being sent out, appears to have the species, nickname, and Poké Ball of the last Pokémon in the party that is able to battle.  This illusion breaks upon being hit by a damaging move.
		/// 
		/// Other damage, e.g. from [weather]{mechanic:weather} or []{move:spikes}, does not break the illusion, nor does damage done to a []{move:substitute}.
		/// 
		/// If the party order becomes temporarily shuffled around as Pokémon are switched out in battle, this ability chooses the last Pokémon according to that shuffled order.
		/// </summary>
		public static readonly Abilities ILLUSION			= new Abilities(149,	"ILLUSION");
		/// <summary>
		/// [Transforms]{move:transform} upon entering battle.<para></para> This Pokémon transforms into a random opponent upon entering battle.  This effect is identical to the move []{move:transform}.
		/// </summary>
		public static readonly Abilities IMPOSTER			= new Abilities(150,	"IMPOSTER");
		/// <summary>
		/// Bypasses []{move:light-screen}, []{move:reflect}, and []{move:safeguard}.<para></para> This Pokémon's moves ignore []{move:light-screen}, []{move:reflect}, and []{move:safeguard}.
		/// </summary>
		public static readonly Abilities INFILTRATOR			= new Abilities(151,	"INFILTRATOR");
		/// <summary>
		/// Changes attacking Pokémon's abilities to Mummy on contact.<para></para> Whenever a contact move hits this Pokémon, the attacking Pokémon's ability changes to Mummy.
		/// 
		/// []{ability:multitype} is unaffected.  If a Pokémon with []{ability:moxie} knocks this Pokémon out, the former's ability will change without taking effect.
		/// </summary>
		public static readonly Abilities MUMMY			= new Abilities(152,	"MUMMY");
		/// <summary>
		/// Raises [Attack]{mechanic:attack} one stage upon KOing a Pokémon.<para></para> This Pokémon's [Attack]{mechanic:attack} rises one stage upon knocking out another Pokémon, even a friendly Pokémon.
		/// 
		/// This ability does not take effect when the Pokémon indirectly causes another Pokémon to faint, e.g. through [poison]{mechanic:poison} or []{move:spikes}.
		/// 
		/// If this Pokémon knocks out a Pokémon with []{ability:mummy}, the former's ability will change without taking effect.
		/// </summary>
		public static readonly Abilities MOXIE			= new Abilities(153,	"MOXIE");
		/// <summary>
		/// Raises [Attack]{mechanic:attack} one stage upon taking damage from a []{type:dark} move.<para></para> Whenever a []{type:dark}-type move hits this Pokémon, its [Attack]{mechanic:attack} rises one [stage]{mechanic:stat-modifier}.
		/// 
		/// The move is not negated in any way.
		/// </summary>
		public static readonly Abilities JUSTIFIED			= new Abilities(154,	"JUSTIFIED");
		/// <summary>
		/// Raises [Speed]{mechanic:speed} one [stage]{mechanic:stat-modifier} upon being hit by a []{type:dark}, []{type:ghost}, or []{type:bug} move.<para></para> This Pokémon's [Speed]{mechanic:speed} rises one stage with each hit from a damaging []{type:dark}-, []{type:ghost}-, or []{type:bug}-type move.
		/// </summary>
		public static readonly Abilities RATTLED			= new Abilities(155,	"RATTLED");
		/// <summary>
		/// Reflects most non-damaging moves back at their user.<para></para> When this Pokémon is targeted by a move flagged as being reflectable, the move is redirected to its user.
		/// 
		/// All reflectable moves are non-damaging, and most non-damaging moves that target other Pokémon are reflectable.
		/// 
		/// A move reflected by this ability or []{move:magic-coat} cannot be reflected back.
		/// </summary>
		public static readonly Abilities MAGIC_BOUNCE			= new Abilities(156,	"MAGIC_BOUNCE");
		/// <summary>
		/// Absorbs []{type:grass} moves, raising [Attack]{mechanic:attack} one [stage]{mechanic:stat-modifier}.<para></para> Whenever a []{type:grass}-type move hits this Pokémon, its [Attack]{mechanic:attack} rises one [stage]{mechanic:stat-modifier}, negating any other effect on it.
		/// </summary>
		public static readonly Abilities SAP_SIPPER			= new Abilities(157,	"SAP_SIPPER");
		/// <summary>
		/// Raises non-damaging moves' priority by one stage.<para></para> This Pokémon's non-damaging moves have their priority increased by one stage.
		/// </summary>
		public static readonly Abilities PRANKSTER			= new Abilities(158,	"PRANKSTER");
		/// <summary>
		/// Strengthens []{type:rock}, []{type:ground}, and []{type:steel} moves to 1.3× their power during a [sandstorm]{mechanic:sandstorm}.  Protects against sandstorm damage.<para></para> During a [sandstorm]{mechanic:sandstorm}, this Pokémon's []{type:rock}-, []{type:ground}-, and []{type:steel}-type moves have 1.3× their base power.  This Pokémon does not take sandstorm damage, regardless of type.
		/// </summary>
		public static readonly Abilities SAND_FORCE			= new Abilities(159,	"SAND_FORCE");
		/// <summary>
		/// Damages attacking Pokémon for 1/8 their max [HP]{mechanic:hp} on contact.<para></para> Whenever a move makes contact with this Pokémon, the move's user takes 1/8 of its maximum [HP]{mechanic:hp} in damage.
		/// 
		/// This ability functions identically to []{ability:rough-skin}.
		/// </summary>
		public static readonly Abilities IRON_BARBS			= new Abilities(160,	"IRON_BARBS");
		/// <summary>
		/// Changes []{pokemon:darmanitan}'s form after each turn depending on its [HP]{mechanic:hp}: Zen Mode below 50% max HP, and Standard Mode otherwise.<para></para> This Pokémon switches between Standard Mode and Zen Mode after each turn depending on its [HP]{mechanic:hp}.  Below 50% of its maximum HP, it switches to Zen Mode, and at 50% or above, it switches to Standard Mode.
		/// 
		/// This Pokémon returns to Standard Mode upon leaving battle or losing this ability.  This ability has no effect if this Pokémon is not a []{pokemon:darmanitan}.
		/// </summary>
		public static readonly Abilities ZEN_MODE			= new Abilities(161,	"ZEN_MODE");
		/// <summary>
		/// Increases moves' accuracy to 1.1× for friendly Pokémon.<para></para> All friendly Pokémon's moves, including this Pokémon's own moves, have 1.1× their usual accuracy while this Pokémon is in battle.
		/// </summary>
		public static readonly Abilities VICTORY_STAR			= new Abilities(162,	"VICTORY_STAR");
		/// <summary>
		/// Bypasses targets' abilities if they could hinder or prevent moves.<para></para> This Pokémon's moves completely ignore abilities that could hinder or prevent their effect on the target.
		/// 
		/// For example, this Pokémon's moves ignore abilities that would fully negate them, such as []{ability:water-absorb}; abilities that would prevent any of their effects, such as []{ability:clear-body}, []{ability:shell-armor}, or []{ability:sticky-hold}; and abilities that grant any general protective benefit, such as []{ability:simple}, []{ability:snow-cloak}, or []{ability:thick-fat}.  If an ability could either hinder or help this Pokémon's moves, e.g. []{ability:dry-skin} or []{ability:unaware}, the ability is ignored either way.
		/// 
		/// Abilities that do not fit this description, even if they could hinder moves in some other way, are not affected.  For example, []{ability:cursed-body} only affects potential future uses of the move, while []{ability:liquid-ooze} and []{ability:shadow-tag} can only hinder a move's effect on the user.  This ablity cannot ignore type or form changes granted by abilities, for example []{ability:color-change} or []{ability:forecast}; nor effects that were caused by abilities but are no longer tied to an ability, such as the rain from []{ability:drizzle}.  This ability cannot ignore []{ability:multitype} at all.
		/// 
		/// An ability ignored by this ability is only nullified while the move is being used.  For example, this Pokémon's moves can [paralyze]{mechanic:paralysis} a Pokémon with []{ability:limber}, but Limber will activate and heal the paralysis immediately thereafter, and this Pokémon's []{move:spikes} are not affected by this ability after they have been placed.
		/// 
		/// When this Pokémon enters battle, all participating trainers are notified that it has this ability.
		/// 
		/// This ability functions identically to []{ability:mold-breaker} and []{ability:teravolt}.
		/// </summary>
		public static readonly Abilities TURBOBLAZE			= new Abilities(163,	"TURBOBLAZE");
		/// <summary>
		/// Bypasses targets' abilities if they could hinder or prevent moves.<para></para> This Pokémon's moves completely ignore abilities that could hinder or prevent their effect on the target.
		/// 
		/// For example, this Pokémon's moves ignore abilities that would fully negate them, such as []{ability:water-absorb}; abilities that would prevent any of their effects, such as []{ability:clear-body}, []{ability:shell-armor}, or []{ability:sticky-hold}; and abilities that grant any general protective benefit, such as []{ability:simple}, []{ability:snow-cloak}, or []{ability:thick-fat}.  If an ability could either hinder or help this Pokémon's moves, e.g. []{ability:dry-skin} or []{ability:unaware}, the ability is ignored either way.
		/// 
		/// Abilities that do not fit this description, even if they could hinder moves in some other way, are not affected.  For example, []{ability:cursed-body} only affects potential future uses of the move, while []{ability:liquid-ooze} and []{ability:shadow-tag} can only hinder a move's effect on the user.  This ablity cannot ignore type or form changes granted by abilities, for example []{ability:color-change} or []{ability:forecast}; nor effects that were caused by abilities but are no longer tied to an ability, such as the rain from []{ability:drizzle}.  This ability cannot ignore []{ability:multitype} at all.
		/// 
		/// An ability ignored by this ability is only nullified while the move is being used.  For example, this Pokémon's moves can [paralyze]{mechanic:paralysis} a Pokémon with []{ability:limber}, but Limber will activate and heal the paralysis immediately thereafter, and this Pokémon's []{move:spikes} are not affected by this ability after they have been placed.
		/// 
		/// When this Pokémon enters battle, all participating trainers are notified that it has this ability.
		/// 
		/// This ability functions identically to []{ability:mold-breaker} and []{ability:turboblaze}.
		/// </summary>
		public static readonly Abilities TERAVOLT			= new Abilities(164,	"TERAVOLT");
		/// <summary>
		/// Protects allies against moves that affect their mental state.<para></para> Protects allies against moves that affect their mental state.
		/// </summary>
		public static readonly Abilities AROMA_VEIL			= new Abilities(165,	"AROMA_VEIL");
		/// <summary>
		/// Protects friendly []{type:grass} Pokémon from having their stats lowered by other Pokémon.<para></para> Protects friendly []{type:grass} Pokémon from having their stats lowered by other Pokémon.
		/// </summary>
		public static readonly Abilities FLOWER_VEIL			= new Abilities(166,	"FLOWER_VEIL");
		/// <summary>
		/// Restores HP upon eating a Berry, in addition to the Berry's effect.<para></para> Restores HP upon eating a Berry, in addition to the Berry's effect.
		/// </summary>
		public static readonly Abilities CHEEK_POUCH			= new Abilities(167,	"CHEEK_POUCH");
		/// <summary>
		/// Changes the bearer's type to match each move it uses.<para></para> Changes the bearer's type to match each move it uses.
		/// 
		/// The type change takes place just before the move is used.
		/// </summary>
		public static readonly Abilities PROTEAN			= new Abilities(168,	"PROTEAN");
		/// <summary>
		/// Halves damage from physical attacks.<para></para> Halves damage from physical attacks.
		/// </summary>
		public static readonly Abilities FUR_COAT			= new Abilities(169,	"FUR_COAT");
		/// <summary>
		/// Steals the target's held item when the bearer uses a damaging move.<para></para> Steals the target's held item when the bearer uses a damaging move.
		/// </summary>
		public static readonly Abilities MAGICIAN			= new Abilities(170,	"MAGICIAN");
		/// <summary>
		/// Protects against bullet, ball, and bomb-based moves.<para></para> Protects against bullet, ball, and bomb-based moves.
		/// </summary>
		public static readonly Abilities BULLETPROOF			= new Abilities(171,	"BULLETPROOF");
		/// <summary>
		/// Raises Special Attack by two stages upon having any stat lowered.<para></para> Raises Special Attack by two stages upon having any stat lowered.
		/// </summary>
		public static readonly Abilities COMPETITIVE			= new Abilities(172,	"COMPETITIVE");
		/// <summary>
		/// Strengthens biting moves to 1.5× their power.<para></para> Strengthens biting moves to 1.5× their power.
		/// </summary>
		public static readonly Abilities STRONG_JAW			= new Abilities(173,	"STRONG_JAW");
		/// <summary>
		/// Turns the bearer's []{type:normal} moves into []{type:ice} moves and strengthens them to 1.3× their power.<para></para> Turns the bearer's []{type:normal}-type moves into []{type:ice}-type moves.  Moves changed by this ability have 1.3× their power.
		/// </summary>
		public static readonly Abilities REFRIGERATE			= new Abilities(174,	"REFRIGERATE");
		/// <summary>
		/// Prevents friendly Pokémon from sleeping.<para></para> Prevents friendly Pokémon from sleeping.
		/// </summary>
		public static readonly Abilities SWEET_VEIL			= new Abilities(175,	"SWEET_VEIL");
		/// <summary>
		/// Changes []{pokemon:aegislash} to Blade Forme before using a damaging move, or Shield Forme before using []{move:kings-shield}.<para></para> Changes []{pokemon:aegislash} to Blade Forme before using a damaging move, or Shield Forme before using []{move:kings-shield}.
		/// </summary>
		public static readonly Abilities STANCE_CHANGE			= new Abilities(176,	"STANCE_CHANGE");
		/// <summary>
		/// Raises []{type:flying} moves' priority by one stage.<para></para> Raises []{type:flying} moves' priority by one stage.
		/// </summary>
		public static readonly Abilities GALE_WINGS			= new Abilities(177,	"GALE_WINGS");
		/// <summary>
		/// Strengthens aura and pulse moves to 1.5× their power.<para></para> Strengthens aura and pulse moves to 1.5× their power.
		/// </summary>
		public static readonly Abilities MEGA_LAUNCHER			= new Abilities(178,	"MEGA_LAUNCHER");
		/// <summary>
		/// Boosts Defense while []{move:grassy-terrain} is in effect.<para></para> Boosts Defense while []{move:grassy-terrain} is in effect.
		/// </summary>
		public static readonly Abilities GRASS_PELT			= new Abilities(179,	"GRASS_PELT");
		/// <summary>
		/// Passes the bearer's held item to an ally when the ally uses up its item.<para></para> Passes the bearer's held item to an ally when the ally uses up its item.
		/// </summary>
		public static readonly Abilities SYMBIOSIS			= new Abilities(180,	"SYMBIOSIS");
		/// <summary>
		/// Strengthens moves that make contact to 1.33× their power.<para></para> Strengthens moves that make contact to 1.33× their power.
		/// </summary>
		public static readonly Abilities TOUGH_CLAWS			= new Abilities(181,	"TOUGH_CLAWS");
		/// <summary>
		/// Turns the bearer's []{type:normal} moves into []{type:fairy} moves and strengthens them to 1.3× their power.<para></para> Turns the bearer's []{type:normal}-type moves into []{type:fairy} moves.  Moves changed by this ability have 1.3× their power.
		/// </summary>
		public static readonly Abilities PIXILATE			= new Abilities(182,	"PIXILATE");
		/// <summary>
		/// Lowers attacking Pokémon's Speed by one stage on contact.<para></para> Lowers attacking Pokémon's Speed by one stage on contact.
		/// </summary>
		public static readonly Abilities GOOEY			= new Abilities(183,	"GOOEY");
		/// <summary>
		/// Turns the bearer's []{type:normal} moves into []{type:flying} moves and strengthens them to 1.3× their power.<para></para> Turns the bearer's []{type:normal}-type moves into []{type:flying}-type moves.  Moves changed by this ability have 1.3× their power.
		/// </summary>
		public static readonly Abilities AERILATE			= new Abilities(184,	"AERILATE");
		/// <summary>
		/// Lets the bearer hit twice with damaging moves.  The second hit has half power.<para></para> Lets the bearer hit twice with damaging moves.  The second hit has half power.
		/// </summary>
		public static readonly Abilities PARENTAL_BOND			= new Abilities(185,	"PARENTAL_BOND");
		/// <summary>
		/// Strengthens []{type:dark} moves to 1.33× their power for all friendly and opposing Pokémon.<para></para> Strengthens []{type:dark} moves for all friendly and opposing Pokémon.
		/// </summary>
		public static readonly Abilities DARK_AURA			= new Abilities(186,	"DARK_AURA");
		/// <summary>
		/// Strengthens []{type:fairy} moves to 1.33× their power for all friendly and opposing Pokémon.<para></para> Strengthens []{type:fairy} moves for all friendly and opposing Pokémon.
		/// </summary>
		public static readonly Abilities FAIRY_AURA			= new Abilities(187,	"FAIRY_AURA");
		/// <summary>
		/// Makes []{ability:dark-aura} and []{ability:fairy-aura} weaken moves of their respective types.<para></para> While this Pokémon is on the field, []{ability:dark-aura} and []{ability:fairy-aura} weaken moves of their respective types to 2/3 their power, rather than strengthening them.
		/// </summary>
		public static readonly Abilities AURA_BREAK			= new Abilities(188,	"AURA_BREAK");
		/// <summary>
		/// Creates heavy rain, which has all the properties of Rain Dance, cannot be replaced, and causes damaging Fire moves to fail.<para></para> When this Pokémon enters battle or gains this ability, the weather becomes heavy rain.  Heavy rain has all the properties of []{move:rain-dance} and also causes damaging Fire moves to fail.
		/// 
		/// Heavy rain ends when this Pokémon leaves battle or loses this ability, or when this ability is nullified.  The weather cannot otherwise be changed except by the effects of []{ability:delta-stream} and []{ability:desolate-land}.
		/// 
		/// []{ability:air-lock} and []{ability:cloud-nine} will prevent the effects of heavy rain, including allowing Fire moves to work, but will not allow the weather to be changed.
		/// </summary>
		public static readonly Abilities PRIMORDIAL_SEA			= new Abilities(189,	"PRIMORDIAL_SEA");
		/// <summary>
		/// Creates extremely harsh sunlight, which has all the properties of Sunny Day, cannot be replaced, and causes damaging Water moves to fail.<para></para> When this Pokémon enters battle or gains this ability, the weather becomes extremely harsh sunlight.  Extremely harsh sunlight has all the properties of []{move:sunny-day} and also causes damaging Water moves to fail.
		/// 
		/// Extremely harsh sunlight ends when this Pokémon leaves battle or loses this ability, or when this ability is nullified.  The weather cannot otherwise be changed except by the effects of []{ability:delta-stream} and []{ability:primordial-sea}.
		/// 
		/// []{ability:air-lock} and []{ability:cloud-nine} will prevent the effects of extremely harsh sunlight, including allowing Water moves to work, but will not allow the weather to be changed.
		/// </summary>
		public static readonly Abilities DESOLATE_LAND			= new Abilities(190,	"DESOLATE_LAND");
		/// <summary>
		/// Creates a mysterious air current, which cannot be replaced and causes moves to never be super effective against Flying Pokémon.<para></para> When this Pokémon enters battle or gains this ability, the weather becomes a mysterious air current.  A mysterious air current causes moves to not be super effective against Flying; they do neutral damage instead.  []{ability:anticipation} and []{move:stealth-rock} are not affected.
		/// 
		/// The mysterious air current ends when this Pokémon leaves battle or loses this ability, or when this ability is nullified.  The weather cannot otherwise be changed except by the effects of []{ability:desolate-land} and []{ability:primordial-sea}.
		/// 
		/// []{ability:air-lock} and []{ability:cloud-nine} will prevent the effect of a mysterious air current, but will not allow the weather to be changed.
		/// </summary>
		public static readonly Abilities DELTA_STREAM			= new Abilities(191,	"DELTA_STREAM");
		/// <summary>
		/// Raises this Pokémon's Defense by one stage when it takes damage from a move.<para></para> Raises this Pokémon's Defense by one stage when it takes damage from a move.
		/// </summary>
		public static readonly Abilities STAMINA			= new Abilities(192,	"STAMINA");
		/// <summary>
		/// This Pokémon automatically switches out when its HP drops below half.<para></para> After this Pokémon is hit by a move, if that move caused this Pokémon's HP to drop below half, it switches out.
		/// </summary>
		public static readonly Abilities WIMP_OUT			= new Abilities(193,	"WIMP_OUT");
		/// <summary>
		/// This Pokémon automatically switches out when its HP drops below half.<para></para> After this Pokémon is hit by a move, if that move caused this Pokémon's HP to drop below half, it switches out.
		/// </summary>
		public static readonly Abilities EMERGENCY_EXIT			= new Abilities(194,	"EMERGENCY_EXIT");
		/// <summary>
		/// Raises this Pokémon's Defense by two stages when it's hit by a Water move.<para></para> Raises this Pokémon's Defense by two stages when it's hit by a Water move.
		/// </summary>
		public static readonly Abilities WATER_COMPACTION			= new Abilities(195,	"WATER_COMPACTION");
		/// <summary>
		/// This Pokémon's moves critical hit against poisoned targets.<para></para> This Pokémon's moves critical hit against poisoned targets.
		/// </summary>
		public static readonly Abilities MERCILESS			= new Abilities(196,	"MERCILESS");
		/// <summary>
		/// Transforms this Minior between Core Form and Meteor Form.  Prevents major status ailments and drowsiness while in Meteor Form.<para></para> When this Pokémon enters battle and at the end of each turn, if its HP is 50% or above, it changes into Meteor Form; otherwise, it changes into Core Form.  In Meteor Form, it cannot be given a major status ailment (though existing ones are not cured), cannot become drowsy from []{move:yawn}, and cannot use []{move:rest} (which will simply fail).
		/// 
		/// This ability cannot be copied, replaced, or nullified.  This ability only takes effect for Minior.
		/// </summary>
		public static readonly Abilities SHIELDS_DOWN			= new Abilities(197,	"SHIELDS_DOWN");
		/// <summary>
		/// This Pokémon's moves have double power against Pokémon that switched in this turn.<para></para> This Pokémon's moves have double power against Pokémon that switched in this turn.
		/// </summary>
		public static readonly Abilities STAKEOUT			= new Abilities(198,	"STAKEOUT");
		/// <summary>
		/// Halves damage from Fire moves, doubles damage of Water moves, and prevents burns.<para></para> When this Pokémon is hit by a Fire move, the damage is halved.  When this Pokémon uses a Water move, the power is doubled.  This Pokémon cannot be burned, and if it becomes burned, the  burn is immediately ured.
		/// </summary>
		public static readonly Abilities WATER_BUBBLE			= new Abilities(199,	"WATER_BUBBLE");
		/// <summary>
		/// This Pokémon's Steel moves have 1.5× power.<para></para> This Pokémon's Steel moves have 1.5× power.
		/// </summary>
		public static readonly Abilities STEELWORKER			= new Abilities(200,	"STEELWORKER");
		/// <summary>
		/// Raises this Pokémon's Special Attack by one stage every time its HP drops below half.<para></para> Whenever this Pokémon takes damage from a move that causes its HP to drop below 50%, its Special Attack rises by one stage.
		/// </summary>
		public static readonly Abilities BERSERK			= new Abilities(201,	"BERSERK");
		/// <summary>
		/// During Hail, this Pokémon has double Speed.<para></para> During Hail, this Pokémon has double Speed.
		/// </summary>
		public static readonly Abilities SLUSH_RUSH			= new Abilities(202,	"SLUSH_RUSH");
		/// <summary>
		/// This Pokémon's moves do not make contact.<para></para> A move used by this Pokémon will not make contact.
		/// </summary>
		public static readonly Abilities LONG_REACH			= new Abilities(203,	"LONG_REACH");
		/// <summary>
		/// Sound-based moves become Water-type.<para></para> When this Pokémon uses a move that is sound-based, that move's type is Water.
		/// </summary>
		public static readonly Abilities LIQUID_VOICE			= new Abilities(204,	"LIQUID_VOICE");
		/// <summary>
		/// This Pokémon's healing moves have their priority increased by 3.<para></para> This Pokémon's healing moves have their priority increased by 3.
		/// </summary>
		public static readonly Abilities TRIAGE			= new Abilities(205,	"TRIAGE");
		/// <summary>
		/// This Pokémon's Normal moves are Electric and have their power increased to 1.2×.<para></para> When this Pokémon uses a Normal moves, that move is Electric its power is 1.2×.
		/// </summary>
		public static readonly Abilities GALVANIZE			= new Abilities(206,	"GALVANIZE");
		/// <summary>
		/// Doubles this Pokémon's Speed on Electric Terrain.<para></para> Doubles this Pokémon's Speed on Electric Terrain.
		/// </summary>
		public static readonly Abilities SURGE_SURFER			= new Abilities(207,	"SURGE_SURFER");
		/// <summary>
		/// Wishiwashi becomes Schooling Form when its HP is 25% or higher.<para></para> If this Pokémon is a []{pokemon:wishiwashi} and level 20 or above, then when it enters battle and at the start of each turn, it becomes Schooling Form if its HP is 25% or higher and Solo Form otherwise.
		/// 
		/// This ability cannot be replaced, copied, or nullified.
		/// </summary>
		public static readonly Abilities SCHOOLING			= new Abilities(208,	"SCHOOLING");
		/// <summary>
		/// Prevents the first instance of battle damage.<para></para> If this Pokémon is in its Disguised Form and takes damage from a move, it switches to its Busted Form and the damage is prevented.  Other effects are not prevented.
		/// 
		/// This ability cannot be copied or replaced.  This ability only takes effect for Mimikyu.
		/// </summary>
		public static readonly Abilities DISGUISE			= new Abilities(209,	"DISGUISE");
		/// <summary>
		/// Transforms this Pokémon into Ash-Greninja after fainting an opponent.  Water Shuriken's power is 20 and always hits three times.<para></para> Transforms this Pokémon into Ash-Greninja after fainting an opponent.  Water Shuriken's power is 20 and always hits three times.
		/// 
		/// This ability cannot be copied or replaced.  This ability only takes effect for Greninja.
		/// </summary>
		public static readonly Abilities BATTLE_BOND			= new Abilities(210,	"BATTLE_BOND");
		/// <summary>
		/// Transforms 10% or 50% Zygarde into Complete Forme when its HP is below 50%.<para></para> Transforms 10% or 50% Zygarde into Complete Forme when its HP is below 50%.
		/// 
		/// This ability cannot be copied or replaced.  This ability only takes effect for Zygarde.
		/// </summary>
		public static readonly Abilities POWER_CONSTRUCT			= new Abilities(211,	"POWER_CONSTRUCT");
		/// <summary>
		/// This Pokémon can inflict poison on Poison and Steel Pokémon.<para></para> This Pokémon's moves and item ignore the usual immunity of Poison and Steel Pokémon when attempting to inflict poison.
		/// </summary>
		public static readonly Abilities CORROSION			= new Abilities(212,	"CORROSION");
		/// <summary>
		/// This Pokémon always  acts as though it were Asleep.<para></para> This Pokémon always acts as though it were Asleep.  It cannot be given another status ailment; it's unaffected by []{move:yawn}; it can use []{move:sleep-talk}; and so on.
		/// </summary>
		public static readonly Abilities COMATOSE			= new Abilities(213,	"COMATOSE");
		/// <summary>
		/// Opposing Pokémon cannot use priority attacks.<para></para> When an opposing Pokémon attempts to use a move that targets this Pokémon or an ally, and that move has priority, it will fail.
		/// </summary>
		public static readonly Abilities QUEENLY_MAJESTY			= new Abilities(214,	"QUEENLY_MAJESTY");
		/// <summary>
		/// When this Pokémon faints from an opponent's move, that opponent takes damage equal to the HP this Pokémon had remaining.<para></para> When this Pokémon faints from an opponent's move, that opponent takes damage equal to the HP this Pokémon had remaining.
		/// </summary>
		public static readonly Abilities INNARDS_OUT			= new Abilities(215,	"INNARDS_OUT");
		/// <summary>
		/// Whenever another Pokémon uses a dance move, this Pokémon will use the same move immediately afterwards.<para></para> Whenever another Pokémon uses a dance move, this Pokémon will use the same move immediately afterwards.
		/// </summary>
		public static readonly Abilities DANCER			= new Abilities(216,	"DANCER");
		/// <summary>
		/// Ally Pokémon's moves have their power increased to 1.3×.<para></para> Ally Pokémon's moves have their power increased to 1.3×.
		/// </summary>
		public static readonly Abilities BATTERY			= new Abilities(217,	"BATTERY");
		/// <summary>
		/// Damage from contact moves is halved.  Damage from Fire moves is doubled.<para></para> Damage from contact moves is halved.  Damage from Fire moves is doubled.
		/// </summary>
		public static readonly Abilities FLUFFY			= new Abilities(218,	"FLUFFY");
		/// <summary>
		/// Opposing Pokémon cannot use priority attacks.<para></para> When an opposing Pokémon attempts to use a move that targets this Pokémon or an ally, and that move has priority, it will fail.
		/// </summary>
		public static readonly Abilities DAZZLING			= new Abilities(219,	"DAZZLING");
		/// <summary>
		/// This Pokémon's Special Attack rises by one stage every time any Pokémon faints.<para></para> This Pokémon's Special Attack rises by one stage every time any Pokémon faints.
		/// </summary>
		public static readonly Abilities SOUL_HEART			= new Abilities(220,	"SOUL_HEART");
		/// <summary>
		/// When this Pokémon takes regular damage from a contact move, the attacking Pokémon's Speed lowers by one stage.<para></para> When this Pokémon takes regular damage from a contact move, the attacking Pokémon's Speed lowers by one stage.
		/// </summary>
		public static readonly Abilities TANGLING_HAIR			= new Abilities(221,	"TANGLING_HAIR");
		/// <summary>
		/// When an ally faints, this Pokémon gains its Ability.<para></para> When an ally faints, this Pokémon gains its Ability.
		/// </summary>
		public static readonly Abilities RECEIVER			= new Abilities(222,	"RECEIVER");
		/// <summary>
		/// When an ally faints, this Pokémon gains its Ability.<para></para> When an ally faints, this Pokémon gains its Ability.
		/// </summary>
		public static readonly Abilities POWER_OF_ALCHEMY			= new Abilities(223,	"POWER_OF_ALCHEMY");
		/// <summary>
		/// Raises this Pokémon's highest stat by one stage when it faints another Pokémon.<para></para> Raises this Pokémon's highest stat by one stage when it faints another Pokémon.
		/// </summary>
		public static readonly Abilities BEAST_BOOST			= new Abilities(224,	"BEAST_BOOST");
		/// <summary>
		/// Changes this Pokémon's type to match its held Memory.<para></para> Changes this Pokémon's type to match its held Memory.
		/// 
		/// This ability cannot be copied, replaced, or nullified.  This ability only takes effect for Silvally.
		/// </summary>
		public static readonly Abilities RKS_SYSTEM			= new Abilities(225,	"RKS_SYSTEM");
		/// <summary>
		/// When this Pokémon enters battle, it changes the terrain to Electric Terrain.<para></para> When this Pokémon enters battle, it changes the terrain to []{move:electric-terrain}.
		/// </summary>
		public static readonly Abilities ELECTRIC_SURGE			= new Abilities(226,	"ELECTRIC_SURGE");
		/// <summary>
		/// When this Pokémon enters battle, it changes the terrain to Psychic Terrain.<para></para> When this Pokémon enters battle, it changes the terrain to []{move:psychic-terrain}.
		/// </summary>
		public static readonly Abilities PSYCHIC_SURGE			= new Abilities(227,	"PSYCHIC_SURGE");
		/// <summary>
		/// When this Pokémon enters battle, it changes the terrain to Misty Terrain.<para></para> When this Pokémon enters battle, it changes the terrain to []{move:misty-terrain}.
		/// </summary>
		public static readonly Abilities MISTY_SURGE			= new Abilities(228,	"MISTY_SURGE");
		/// <summary>
		/// When this Pokémon enters battle, it changes the terrain to Grassy Terrain.<para></para> When this Pokémon enters battle, it changes the terrain to []{move:grassy-terrain}.
		/// </summary>
		public static readonly Abilities GRASSY_SURGE			= new Abilities(229,	"GRASSY_SURGE");
		/// <summary>
		/// Other Pokémon cannot lower this Pokémon's stats.<para></para> This Pokémon's stats cannot be lowered by other Pokémon's moves or abilities.  This effect only applies to normal stat modifications and not more exotic effects such as []{move:topsy-turvy} or []{move:power-swap}.
		/// 
		/// This Ability is not bypassed by []{ability:mold-breaker}, []{ability:teravolt}, or []{ability:turboblaze}.
		/// </summary>
		public static readonly Abilities FULL_METAL_BODY			= new Abilities(230,	"FULL_METAL_BODY");
		/// <summary>
		/// When this Pokémon has full HP, regular damage from moves is halved.<para></para> When this Pokémon has full HP, regular damage (not fixed damage!) from moves is halved.
		/// 
		/// This ability cannot be nullified.
		/// </summary>
		public static readonly Abilities SHADOW_SHIELD			= new Abilities(231,	"SHADOW_SHIELD");
		/// <summary>
		/// Reduces super-effective damage to 0.75×.<para></para> Super-effective damage this Pokémon takes is reduced to 0.75×.
		/// 
		/// This Ability is not bypassed by []{ability:mold-breaker}, []{ability:teravolt}, or []{ability:turboblaze}.
		/// </summary>
		public static readonly Abilities PRISM_ARMOR			= new Abilities(232,	"PRISM_ARMOR");
		/// <summary>
		/// XXX new effect for neuroforce<para></para> XXX new effect for neuroforce
		/// </summary>
		public static readonly Abilities NEUROFORCE			= new Abilities(233,	"NEUROFORCE");
	}
}