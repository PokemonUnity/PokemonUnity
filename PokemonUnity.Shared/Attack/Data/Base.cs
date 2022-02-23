using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Inventory;
using PokemonUnity.Attack;

namespace PokemonUnity.Attack.Data
{
	public struct MoveData
	{
		#region Variables
		public Category Category { get; private set; }
		public Moves ID { get; private set; }
		public int GenerationId { get; private set; }
		/// <summary>
		/// The move's accuracy, as a percentage. 
		/// An accuracy of 0 means the move doesn't perform an accuracy check 
		/// (i.e. it cannot be evaded).
		/// </summary>
		public int? Accuracy { get; private set; }
		/// <summary>
		/// Base Damage
		/// </summary>
		public int? Power { get; private set; }
		/// <summary>
		/// </summary>
		/// <remarks>If <see cref="Types.SHADOW"/> PP is EqualTo NULL</remarks>
		public byte PP { get; private set; }
		public int Priority { get; private set; }
		public Flag Flags { get; private set; }
		public Targets Target { get; private set; }
		public Types Type { get; private set; }
		public Contests? ContestType { get; private set; }
		//public short Function { get; private set; }
		//public string FunctionAsString { get; private set; }
		/// <summary>
		/// The probability that the move's additional effect occurs, as a percentage. 
		/// If the move has no additional effect (e.g. all status moves), this value is 0.
		/// Note that some moves have an additional effect chance of 100 (e.g.Acid Spray), 
		/// which is not the same thing as having an effect that will always occur. 
		/// Abilities like Sheer Force and Shield Dust only affect additional effects, not regular effects.
		/// </summary>
		public Effects Effect { get; private set; }
		public int? EffectChance { get; private set; }
		//public int? ContestEffect { get; private set; }
		//public int? ContestSuperEffect { get; private set; }
		//public string Name { get; private set; }
		//public string Description { get; private set; }
		public int? SuperAppeal { get; private set; }
		public int? Appeal { get; private set; }
		public int? Jamming { get; private set; }
		public string Name { get { return ToString(TextScripts.Name); } }
		public string Description { get { return ToString(TextScripts.Description); } }
		#endregion

		public MoveData(Moves id = Moves.NONE, Category category = Category.STATUS, int generationId = 0, 
			int? accuracy = null, int? baseDamage = null, byte pp = 0, int priority = 0, Flag? flags = null, 
			Targets target = Targets.SPECIFIC_MOVE, Types type = Types.NONE, Contests? contestType = null,//Contests.NONE, 
			//ToDo: ContestEffects contestEffects, SuperContestEffects superContestEffects,
			short function = 0, string functionAsString = null, Effects effects = Effects.NONE, int? chance = null, 
			string name = null, string description = null, int? appeal = null, int? superAppeal = null, int? jamming = null)
		{
			Category = category;
			ID = id;
			GenerationId = generationId;
			Accuracy = accuracy;
			Power = baseDamage;
			PP = pp;
			Priority = priority;
			Flags = flags ?? new Flag();
			Target = target;
			Type = type;
			ContestType = contestType;
			//Function = function;
			//FunctionAsString = functionAsString;
			Effect = effects;
			EffectChance = chance;
			//Name = name;
			//Description = description;
			Appeal = appeal;
			SuperAppeal = superAppeal;
			Jamming = jamming;
		}

		public string ToString(TextScripts text)
		{
			//ToDo: Create an Interface and Move Function to Application Library
			//return ID.ToString(text);
			return ID.ToString();
		}
	}

	public enum Effects
	{
		NONE = 0,
		/// <summary>
		/// Inflicts regular damage with no additional effect.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.
		/// </summary>	
		/// <remarks>
		/// POUND,MEGA-PUNCH,SCRATCH,VICE-GRIP,CUT,WING-ATTACK,SLAM,VINE-WHIP,MEGA-KICK,HORN-ATTACK,TACKLE,WATER-GUN,HYDRO-PUMP,PECK,DRILL-PECK,STRENGTH,ROCK-THROW,EGG-BOMB,MEGAHORN,HYPER-VOICE,DRAGON-CLAW,AQUA-TAIL,SEED-BOMB,X-SCISSOR,DRAGON-PULSE,POWER-GEM,POWER-WHIP,FAIRY-WIND,DAZZLING-GLEAM,LANDS-WRATH,ORIGIN-PULSE,PRECIPICE-BLADES,BREAKNECK-BLITZ--PHYSICAL,BREAKNECK-BLITZ--SPECIAL,ALL-OUT-PUMMELING--PHYSICAL,ALL-OUT-PUMMELING--SPECIAL,SUPERSONIC-SKYSTRIKE--PHYSICAL,SUPERSONIC-SKYSTRIKE--SPECIAL,ACID-DOWNPOUR--PHYSICAL,ACID-DOWNPOUR--SPECIAL,TECTONIC-RAGE--PHYSICAL,TECTONIC-RAGE--SPECIAL,CONTINENTAL-CRUSH--PHYSICAL,CONTINENTAL-CRUSH--SPECIAL,SAVAGE-SPIN-OUT--PHYSICAL,SAVAGE-SPIN-OUT--SPECIAL,NEVER-ENDING-NIGHTMARE--PHYSICAL,NEVER-ENDING-NIGHTMARE--SPECIAL,CORKSCREW-CRASH--PHYSICAL,CORKSCREW-CRASH--SPECIAL,INFERNO-OVERDRIVE--PHYSICAL,INFERNO-OVERDRIVE--SPECIAL,HYDRO-VORTEX--PHYSICAL,HYDRO-VORTEX--SPECIAL,BLOOM-DOOM--PHYSICAL,BLOOM-DOOM--SPECIAL,GIGAVOLT-HAVOC--PHYSICAL,GIGAVOLT-HAVOC--SPECIAL,SHATTERED-PSYCHE--PHYSICAL,SHATTERED-PSYCHE--SPECIAL,SUBZERO-SLAMMER--PHYSICAL,SUBZERO-SLAMMER--SPECIAL,DEVASTATING-DRAKE--PHYSICAL,DEVASTATING-DRAKE--SPECIAL,BLACK-HOLE-ECLIPSE--PHYSICAL,BLACK-HOLE-ECLIPSE--SPECIAL,TWINKLE-TACKLE--PHYSICAL,TWINKLE-TACKLE--SPECIAL,CATASTROPIKA,HIGH-HORSEPOWER,LEAFAGE,DRAGON-HAMMER,SINISTER-ARROW-RAID,MALICIOUS-MOONSAULT,OCEANIC-OPERETTA,SOUL-STEALING-7-STAR-STRIKE,PULVERIZING-PANCAKE,10-000-000-VOLT-THUNDERBOLT,PLASMA-FISTS,PHOTON-GEYSER,SHADOW-BLITZ,SHADOW-BREAK,SHADOW-RAVE,SHADOW-STORM,SHADOW-WAVE
		/// </remarks>	
		x001 = 1,
		/// <summary>
		/// Puts the target to sleep.<para></para> Puts the target to <see cref="Attack.MoveMetaAilments.SLEEP"/>.
		/// </summary>	
		/// <remarks>
		/// SING,SLEEP-POWDER,HYPNOSIS,LOVELY-KISS,SPORE,GRASS-WHISTLE,DARK-VOID
		/// </remarks>	
		x002 = 2,
		/// <summary>
		/// Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to  <see cref="Attack.MoveMetaAilments.POISON"/> the target.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to <see cref="Attack.MoveMetaAilments.POISON"/> the target.
		/// </summary>	
		/// <remarks>
		/// POISON-STING,SMOG,SLUDGE,SLUDGE-BOMB,POISON-JAB,GUNK-SHOT,SLUDGE-WAVE
		/// </remarks>	
		x003 = 3,
		/// <summary>
		/// Drains half the damage inflicted to heal the user.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  [Drains]{mechanic:drain} half the damage inflicted to heal the user.
		/// </summary>	
		/// <remarks>
		/// ABSORB,MEGA-DRAIN,LEECH-LIFE,GIGA-DRAIN,DRAIN-PUNCH,HORN-LEECH
		/// </remarks>	
		x004 = 4,
		/// <summary>
		/// Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to <see cref="Attack.MoveMetaAilments.BURN"/> the target.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to <see cref="Attack.MoveMetaAilments.BURN"/> the target.
		/// </summary>	
		/// <remarks>
		/// FIRE-PUNCH,EMBER,FLAMETHROWER,FIRE-BLAST,HEAT-WAVE,LAVA-PLUME,SCALD,INFERNO,SEARING-SHOT,BLUE-FLARE,STEAM-ERUPTION,SHADOW-FIRE
		/// </remarks>	
		x005 = 5,
		/// <summary>
		/// Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to <see cref="Attack.MoveMetaAilments.FREEZE"/> the target.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to <see cref="Attack.MoveMetaAilments.FREEZE"/> the target.
		/// </summary>	
		/// <remarks>
		/// ICE-PUNCH,ICE-BEAM,POWDER-SNOW,SHADOW-CHILL
		/// </remarks>	
		x006 = 6,
		/// <summary>
		/// Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to <see cref="Attack.MoveMetaAilments.PARALYSIS"/> the target.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to [paralyze]{mechanic:paralyze} the target.
		/// </summary>	
		/// <remarks>
		/// THUNDER-PUNCH,BODY-SLAM,THUNDER-SHOCK,THUNDERBOLT,LICK,ZAP-CANNON,SPARK,DRAGON-BREATH,FORCE-PALM,DISCHARGE,BOLT-STRIKE,STOKED-SPARKSURFER,SHADOW-BOLT
		/// </remarks>	
		x007 = 7,
		/// <summary>
		/// User faints.<para></para> User [faint]{mechanic:faint}s, even if the attack [fail]{mechanic:fail}s or [miss]{mechanic:miss}es.  Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.
		/// </summary>	
		/// <remarks>
		/// SELF-DESTRUCT,EXPLOSION
		/// </remarks>	
		x008 = 8,
		/// <summary>
		/// Only works on sleeping Pokémon.  Drains half the damage inflicted to heal the user.<para></para> [Fails]{mechanic:fail} if not used on a <see cref="Attack.MoveMetaAilments.SLEEP"/>ing Pokémon.  Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  [Drains]{mechanic:drain} half the damage inflicted to heal the user.
		/// </summary>	
		/// <remarks>
		/// DREAM-EATER
		/// </remarks>	
		x009 = 9,
		/// <summary>
		/// Uses the target's last used move.<para></para> Uses the last move targeted at the user by a Pokémon still on the [field]{mechanic:field}.  A move counts as targeting the user even if it hit multiple Pokémon, as long as the user was one of them; however, moves targeting the [field]{mechanic:field} itself do not count.  If the user has not been targeted by an appropriate move since entering the [field]{mechanic:field}, or if no Pokémon that targeted the user remains on the [field]{mechanic:field}, this move will [fail]{mechanic:fail}.
		/// 
		/// Moves that [fail]{mechanic:fail}ed, [miss]{mechanic:miss}ed, had [no effect]{mechanic:no-effect}, or were [block]{mechanic:block}ed are still copied.
		/// 
		/// Assist moves, time-delayed moves, “meta” moves that operate on other moves/Pokémon/abilities, and some other special moves cannot be copied and are ignored; if the last move to hit the user was such a move, the previous move will be used instead.  The full list of ignored moves is: <seealso cref="Moves.ACID_ARMOR"/>, <seealso cref="Moves.ACUPRESSURE"/>, <seealso cref="Moves.AFTER_YOU"/>, <seealso cref="Moves.AGILITY"/>, <seealso cref="Moves.ALLY_SWITCH"/>, <seealso cref="Moves.AMNESIA"/>, <seealso cref="Moves.AQUA_RING"/>, <seealso cref="Moves.AROMATHERAPY"/>, <seealso cref="Moves.AROMATIC_MIST"/>, <seealso cref="Moves.ASSIST"/>, <seealso cref="Moves.AUTOTOMIZE"/>, <seealso cref="Moves.BARRIER"/>, <seealso cref="Moves.BATON_PASS"/>, <seealso cref="Moves.BELCH"/>, <seealso cref="Moves.BELLY_DRUM"/>, <seealso cref="Moves.BIDE"/>, <seealso cref="Moves.BULK_UP"/>, <seealso cref="Moves.CALM_MIND"/>, <seealso cref="Moves.CAMOUFLAGE"/>, <seealso cref="Moves.CELEBRATE"/>, <seealso cref="Moves.CHARGE"/>, <seealso cref="Moves.COIL"/>, <seealso cref="Moves.CONVERSION"/>, <seealso cref="Moves.CONVERSION_2"/>, <seealso cref="Moves.COPYCAT"/>, <seealso cref="Moves.COSMIC_POWER"/>, <seealso cref="Moves.COTTON_GUARD"/>, <seealso cref="Moves.COUNTER"/>, <seealso cref="Moves.CRAFTY_SHIELD"/>, <seealso cref="Moves.CURSE"/>, <seealso cref="Moves.DEFEND_ORDER"/>, <seealso cref="Moves.DEFENSE_CURL"/>, <seealso cref="Moves.DESTINY_BOND"/>, <seealso cref="Moves.DETECT"/>, <seealso cref="Moves.DOOM_DESIRE"/>, <seealso cref="Moves.DOUBLE_TEAM"/>, <seealso cref="Moves.DRAGON_DANCE"/>, <seealso cref="Moves.ELECTRIC_TERRAIN"/>, <seealso cref="Moves.ENDURE"/>, <seealso cref="Moves.FINAL_GAMBIT"/>, <seealso cref="Moves.FLOWER_SHIELD"/>, <seealso cref="Moves.FOCUS_ENERGY"/>, <seealso cref="Moves.FOCUS_PUNCH"/>, <seealso cref="Moves.FOLLOW_ME"/>, <seealso cref="Moves.FUTURE_SIGHT"/>, <seealso cref="Moves.GEOMANCY"/>, <seealso cref="Moves.GRASSY_TERRAIN"/>, <seealso cref="Moves.GRAVITY"/>, <seealso cref="Moves.GROWTH"/>, <seealso cref="Moves.GRUDGE"/>, <seealso cref="Moves.GUARD_SPLIT"/>, <seealso cref="Moves.HAIL"/>, <seealso cref="Moves.HAPPY_HOUR"/>, <seealso cref="Moves.HARDEN"/>, <seealso cref="Moves.HAZE"/>, <seealso cref="Moves.HEAL_BELL"/>, <seealso cref="Moves.HEAL_ORDER"/>, <seealso cref="Moves.HEAL_PULSE"/>, <seealso cref="Moves.HEALING_WISH"/>, <seealso cref="Moves.HELPING_HAND"/>, <seealso cref="Moves.HOLD_HANDS"/>, <seealso cref="Moves.HONE_CLAWS"/>, <seealso cref="Moves.HOWL"/>, <seealso cref="Moves.IMPRISON"/>, <seealso cref="Moves.INGRAIN"/>, <seealso cref="Moves.ION_DELUGE"/>, <seealso cref="Moves.IRON_DEFENSE"/>, <seealso cref="Moves.KINGS_SHIELD"/>, <seealso cref="Moves.LIGHT_SCREEN"/>, <seealso cref="Moves.LUCKY_CHANT"/>, <seealso cref="Moves.LUNAR_DANCE"/>, <seealso cref="Moves.MAGIC_COAT"/>, <seealso cref="Moves.MAGNET_RISE"/>, <seealso cref="Moves.MAGNETIC_FLUX"/>, <seealso cref="Moves.MAT_BLOCK"/>, <seealso cref="Moves.ME_FIRST"/>, <seealso cref="Moves.MEDITATE"/>, <seealso cref="Moves.METRONOME"/>, <seealso cref="Moves.MILK_DRINK"/>, <seealso cref="Moves.MIMIC"/>, <seealso cref="Moves.MINIMIZE"/>, <seealso cref="Moves.MIRROR_COAT"/>, <seealso cref="Moves.MIRROR_MOVE"/>, <seealso cref="Moves.MIST"/>, <seealso cref="Moves.MISTY_TERRAIN"/>, <seealso cref="Moves.MOONLIGHT"/>, <seealso cref="Moves.MORNING_SUN"/>, <seealso cref="Moves.MUD_SPORT"/>, <seealso cref="Moves.NASTY_PLOT"/>, <seealso cref="Moves.NATURE_POWER"/>, <seealso cref="Moves.PERISH_SONG"/>, <seealso cref="Moves.POWER_SPLIT"/>, <seealso cref="Moves.POWER_TRICK"/>, <seealso cref="Moves.PROTECT"/>, <seealso cref="Moves.PSYCH_UP"/>, <seealso cref="Moves.QUICK_GUARD"/>, <seealso cref="Moves.QUIVER_DANCE"/>, <seealso cref="Moves.RAGE_POWDER"/>, <seealso cref="Moves.RAIN_DANCE"/>, <seealso cref="Moves.RECOVER"/>, <seealso cref="Moves.RECYCLE"/>, <seealso cref="Moves.REFLECT"/>, <seealso cref="Moves.REFLECT_TYPE"/>, <seealso cref="Moves.REFRESH"/>, <seealso cref="Moves.REST"/>, <seealso cref="Moves.ROCK_POLISH"/>, <seealso cref="Moves.ROLE_PLAY"/>, <seealso cref="Moves.ROOST"/>, <seealso cref="Moves.ROTOTILLER"/>, <seealso cref="Moves.SAFEGUARD"/>, <seealso cref="Moves.SANDSTORM"/>, <seealso cref="Moves.SHADOW_BLAST"/>, <seealso cref="Moves.SHADOW_BOLT"/>, <seealso cref="Moves.SHADOW_HALF"/>, <seealso cref="Moves.SHADOW_RUSH"/>, <seealso cref="Moves.SHADOW_SHED"/>, <seealso cref="Moves.SHADOW_SKY"/>, <seealso cref="Moves.SHADOW_STORM"/>, <seealso cref="Moves.SHADOW_WAVE"/>, <seealso cref="Moves.SHARPEN"/>, <seealso cref="Moves.SHELL_SMASH"/>, <seealso cref="Moves.SHIFT_GEAR"/>, <seealso cref="Moves.SKETCH"/>, <seealso cref="Moves.SLACK_OFF"/>, <seealso cref="Moves.SLEEP_TALK"/>, <seealso cref="Moves.SNATCH"/>, <seealso cref="Moves.SOFT_BOILED"/>, <seealso cref="Moves.SPIKES"/>, <seealso cref="Moves.SPIKY_SHIELD"/>, <seealso cref="Moves.SPIT_UP"/>, <seealso cref="Moves.SPLASH"/>, <seealso cref="Moves.STEALTH_ROCK"/>, <seealso cref="Moves.STICKY_WEB"/>, <seealso cref="Moves.STOCKPILE"/>, <seealso cref="Moves.STRUGGLE"/>, <seealso cref="Moves.SUBSTITUTE"/>, <seealso cref="Moves.SUNNY_DAY"/>, <seealso cref="Moves.SWALLOW"/>, <seealso cref="Moves.SWORDS_DANCE"/>, <seealso cref="Moves.SYNTHESIS"/>, <seealso cref="Moves.TAIL_GLOW"/>, <seealso cref="Moves.TAILWIND"/>, <seealso cref="Moves.TELEPORT"/>, <seealso cref="Moves.TOXIC_SPIKES"/>, <seealso cref="Moves.TRANSFORM"/>, <seealso cref="Moves.WATER_SPORT"/>, <seealso cref="Moves.WIDE_GUARD"/>, <seealso cref="Moves.WISH"/>, <seealso cref="Moves.WITHDRAW"/> and <seealso cref="Moves.WORK_UP"/>.
		/// 
		/// This move cannot be selected by <seealso cref="Moves.ASSIST"/>, <seealso cref="Moves.METRONOME"/>, or <seealso cref="Moves.SLEEP_TALK"/>, nor forced by <seealso cref="Moves.ENCORE"/>.
		/// </summary>	
		/// <remarks>
		/// MIRROR-MOVE
		/// </remarks>	
		x00A = 10,
		/// <summary>
		/// Raises the user's Attack by one stage.<para></para> Raises the user's [Attack]{mechanic:attack} by one [stage]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// MEDITATE,SHARPEN,HOWL
		/// </remarks>	
		x00B = 11,
		/// <summary>
		/// Raises the user's Defense by one stage.<para></para> Raises the user's [Defense]{mechanic:defense} by one [stage]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// HARDEN,WITHDRAW
		/// </remarks>	
		x00C = 12,
		/// <summary>
		/// Raises the user's Special Attack by one stage.<para></para> Raises the user's [Special Attack]{mechanic:special-attack} by one [stage]{mechanic:stage}.
		/// </summary>
		x00E = 14,
		/// <summary>
		/// Raises the user's evasion by one stage.<para></para> Raises the user's [evasion]{mechanic:evasion} by one [stage]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// DOUBLE-TEAM
		/// </remarks>	
		x011 = 17,
		/// <summary>
		/// Never misses.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Ignores [accuracy]{mechanic:accuracy} and [evasion]{mechanic:evasion} modifiers.
		/// </summary>	
		/// <remarks>
		/// SWIFT,FEINT-ATTACK,SHADOW-PUNCH,AERIAL-ACE,MAGICAL-LEAF,SHOCK-WAVE,AURA-SPHERE,MAGNET-BOMB,SMART-STRIKE
		/// </remarks>	
		x012 = 18,
		/// <summary>
		/// Lowers the target's Attack by one stage.<para></para> Lowers the target's [Attack]{mechanic:attack} by one [stage]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// GROWL
		/// </remarks>	
		x013 = 19,
		/// <summary>
		/// Lowers the target's Defense by one stage.<para></para> Lowers the target's [Defense]{mechanic:defense} by one [stage]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// TAIL-WHIP,LEER
		/// </remarks>	
		x014 = 20,
		/// <summary>
		/// Lowers the target's Speed by one stage.<para></para> Lowers the target's [Speed]{mechanic:speed} by one [stage]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// LOW-SWEEP,ELECTROWEB
		/// </remarks>	
		x015 = 21,
		/// <summary>
		/// Lowers the target's accuracy by one stage.<para></para> Lowers the target's [accuracy]{mechanic:accuracy} by one [stage]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// SAND-ATTACK,SMOKESCREEN,KINESIS,FLASH
		/// </remarks>	
		x018 = 24,
		/// <summary>
		/// Lowers the target's evasion by one stage.<para></para> Lowers the target's [evasion]{mechanic:evasion} by one [stage]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// SWEET-SCENT
		/// </remarks>	
		x019 = 25,
		/// <summary>
		/// Resets all Pokémon's stats, accuracy, and evasion.<para></para> Removes [stat]{mechanic:stat}, [accuracy]{mechanic:accuracy}, and [evasion]{mechanic:evasion} modifiers from every Pokémon on the [field]{mechanic:field}.
		/// 
		/// This does not count as a stat reduction for the purposes of <seealso cref="Abilities.CLEAR_BODY"/> or <seealso cref="Abilities.WHITE_SMOKE"/>.
		/// </summary>
		/// <remarks>
		/// HAZE
		/// </remarks>	
		x01A = 26,
		/// <summary>
		/// User waits for two turns, then hits back for twice the damage it took.<para></para> User waits for two turns.  On the second turn, the user inflicts twice the damage it accumulated on the last Pokémon to hit it.  Damage inflicted is [typeless]{mechanic:typeless}.
		/// 
		/// This move cannot be selected by <seealso cref="Moves.SLEEP_TALK"/>.
		/// </summary>	
		/// <remarks>
		/// BIDE
		/// </remarks>	
		x01B = 27,
		/// <summary>
		/// Hits every turn for 2-3 turns, then confuses the user.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  User is forced to attack with this move for 2–3 turns,selected at random.  After the last hit, the user becomes [confused]{mechanic:confused}.
		/// 
		/// <seealso cref="Moves.SAFEGUARD"/> does not protect against the <see cref="Attack.MoveMetaAilments.CONFUSION"/> from this move.
		/// </summary>
		/// <remarks>
		/// THRASH,PETAL-DANCE,OUTRAGE
		/// </remarks>	
		x01C = 28,
		/// <summary>
		/// Immediately ends wild battles.  Forces trainers to switch Pokémon.<para></para> [Switch]{mechanic:switch}es the target out for another of its trainer's Pokémon selected at random.  Wild battles end immediately.
		/// 
		/// Doesn't affect Pokémon with <seealso cref="Abilities.SUCTION_CUPS"/> or under the effect of <seealso cref="Moves.INGRAIN"/>.
		/// </summary>
		/// <remarks>
		/// WHIRLWIND,ROAR
		/// </remarks>	
		x01D = 29,
		/// <summary>
		/// Hits 2-5 times in one turn.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Hits 2–5 times in one turn.
		/// 
		/// Has a 3/8 chance each to hit 2 or 3 times, and a 1/8 chance each to hit 4 or 5 times.  Averages to 3 hits per use.
		/// </summary>
		/// <remarks>
		/// DOUBLE-SLAP,COMET-PUNCH,FURY-ATTACK,PIN-MISSILE,SPIKE-CANNON,BARRAGE,FURY-SWIPES,BONE-RUSH,ARM-THRUST,BULLET-SEED,ICICLE-SPEAR,ROCK-BLAST,TAIL-SLAP
		/// </remarks>	
		x01E = 30,
		/// <summary>
		/// User's type changes to the type of one of its moves at random.<para></para> User's [type]{mechanic:type} changes to the [type]{mechanic:type} of one of its moves, selected at random.  <seealso cref="Moves.HIDDEN_POWER"/> and <seealso cref="Moves.WEATHER_BALL"/> are treated as <seealso cref="Types.NORMAL"/>.  Only moves with a different [type]{mechanic:type} are eligible, and <seealso cref="Moves.CURSE"/> is never eligible.  If the user has no suitable moves, this move will [fail]{mechanic:fail}.
		/// </summary>
		/// <remarks>
		/// CONVERSION
		/// </remarks>	
		x01F = 31,
		/// <summary>
		/// Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to make the target flinch.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to make the target <seealso cref="Combat.BattlerEffects.Flinch"/>.
		/// </summary>
		/// <remarks>
		/// ROLLING-KICK,HEADBUTT,BITE,BONE-CLUB,WATERFALL,ROCK-SLIDE,HYPER-FANG,NEEDLE-ARM,ASTONISH,EXTRASENSORY,DARK-PULSE,AIR-SLASH,DRAGON-RUSH,ZEN-HEADBUTT,IRON-HEAD,HEART-STAMP,ICICLE-CRASH,ZING-ZAP
		/// </remarks>	
		x020 = 32,
		/// <summary>
		/// Heals the user by half its max HP.<para></para> Heals the user for half its max [HP]{mechanic:hp}.
		/// </summary>	
		/// <remarks>
		/// RECOVER,SOFT-BOILED,MILK-DRINK,SLACK-OFF,HEAL-ORDER
		/// </remarks>	
		x021 = 33,
		/// <summary>
		/// Badly poisons the target, inflicting more damage every turn.<para></para> [Badly poisons]{mechanic:badly-poisons} the target.  Never misses when used by a <seealso cref="Types.POISON"/>-type Pokémon.
		/// </summary>
		/// <remarks>
		/// TOXIC
		/// </remarks>	
		x022 = 34,
		/// <summary>
		/// Scatters money on the ground worth five times the user's level.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  After the battle ends, the winner receives five times the user's level in extra money for each time this move was used.
		/// </summary>	
		/// <remarks>
		/// PAY-DAY
		/// </remarks>	
		x023 = 35,
		/// <summary>
		/// Reduces damage from special attacks by 50% for five turns.<para></para> Erects a barrier around the user's side of the field that reduces damage from [special]{mechanic:special} attacks by half for five turns.  In double battles, the reduction is 1/3.  [Critical hit]{mechanic:critical-hit}s are not affected by the barrier.
		/// 
		/// If the user is holding <seealso cref="Items.LIGHT_CLAY"/>, the barrier lasts for eight turns.
		/// 
		/// <seealso cref="Moves.BRICK_BREAK"/> or <seealso cref="Moves.DEFOG"/> used by an opponent will destroy the barrier.
		/// </summary>
		/// <remarks>
		/// LIGHT-SCREEN
		/// </remarks>	
		x024 = 36,
		/// <summary>
		/// Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to <see cref="Attack.MoveMetaAilments.BURN"/>, <see cref="Attack.MoveMetaAilments.FREEZE"/>, or <see cref="Attack.MoveMetaAilments.PARALYSIS"/> the target.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to <see cref="Attack.MoveMetaAilments.BURN"/>, <see cref="Attack.MoveMetaAilments.FREEZE"/>, or [paralyze]{mechanic:paralyze} the target.  One of these effects is selected at random; they do not each have independent chances to occur.
		/// </summary>	
		/// <remarks>
		/// TRI-ATTACK
		/// </remarks>	
		x025 = 37,
		/// <summary>
		/// User sleeps for two turns, completely healing itself.<para></para> User falls to <see cref="Attack.MoveMetaAilments.SLEEP"/> and immediately regains all its [HP]{mechanic:hp}.  If the user has another [major status effect]{mechanic:major-status-effect}, <see cref="Attack.MoveMetaAilments.SLEEP"/> will replace it.  The user will always wake up after two turns, or one turn with <seealso cref="Abilities.EARLY_BIRD"/>.
		/// 
		/// This move [fail]{mechanic:fail}s if the Pokémon cannot fall asleep due to <seealso cref="Moves.UPROAR"/>, <seealso cref="Abilities.INSOMNIA"/>, or <seealso cref="Abilities.VITAL_SPIRIT"/>.  It also fails if the Pokémon is at full health or is already asleep.
		/// </summary>
		/// <remarks>
		/// REST
		/// </remarks>	
		x026 = 38,
		/// <summary>
		/// Causes a one-hit KO.<para></para> Inflicts damage equal to the target's max [HP]{mechanic:hp}.  Ignores [accuracy]{mechanic:accuracy} and [evasion]{mechanic:evasion} modifiers.  This move's [accuracy]{mechanic:accuracy} is 30% plus 1% for each level the user is higher than the target.  If the user is a lower level than the target, this move will [fail]{mechanic:fail}.
		/// 
		/// Because this move inflicts a specific and finite amount of damage, <seealso cref="Moves.ENDURE"/> still prevents the target from fainting.
		/// 
		/// The effects of <seealso cref="Moves.LOCK_ON"/>, <seealso cref="Moves.MIND_READER"/>, and <seealso cref="Abilities.NO_GUARD"/> still apply, as long as the user is equal or higher level than the target.  However, they will not give this move a chance to break through <seealso cref="Moves.DETECT"/> or <seealso cref="Moves.PROTECT"/>.
		/// </summary>	
		/// <remarks>
		/// GUILLOTINE,HORN-DRILL,FISSURE,SHEER-COLD
		/// </remarks>	
		x027 = 39,
		/// <summary>
		/// Requires a turn to charge before attacking.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  User's [critical hit]{mechanic:critical-hit} rate is one level higher when using this move.  User charges for one turn before attacking.
		/// 
		/// This move cannot be selected by <seealso cref="Moves.SLEEP_TALK"/>.
		/// </summary>
		/// <remarks>
		/// RAZOR-WIND
		/// </remarks>	
		x028 = 40,
		/// <summary>
		/// Inflicts damage equal to half the target's HP.<para></para> Inflicts [typeless]{mechanic:typeless} damage equal to half the target's remaining [HP]{mechanic:hp}.
		/// </summary>	
		/// <remarks>
		/// SUPER-FANG,NATURES-MADNESS
		/// </remarks>	
		x029 = 41,
		/// <summary>
		/// Inflicts 40 points of damage.<para></para> Inflicts exactly 40 damage.
		/// </summary>	
		/// <remarks>
		/// DRAGON-RAGE
		/// </remarks>	
		x02A = 42,
		/// <summary>
		/// Prevents the target from fleeing and inflicts damage for 2-5 turns.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  For the next 2–5 turns, the target cannot leave the field and is damaged for 1/16 its max HP at the end of each turn.  The user continues to use other moves during this time.  If the user leaves the [field]{mechanic:field}, this effect ends.
		/// 
		/// Has a 3/8 chance each to hit 2 or 3 times, and a 1/8 chance each to hit 4 or 5 times.  Averages to 3 hits per use.
		/// 
		/// <seealso cref="Moves.RAPID_SPIN"/> cancels this effect.
		/// </summary>
		/// <remarks>
		/// BIND,WRAP,FIRE-SPIN,CLAMP,SAND-TOMB,MAGMA-STORM,INFESTATION
		/// </remarks>	
		x02B = 43,
		/// <summary>
		/// Has an increased chance for a critical hit.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  User's [critical hit]{mechanic:critical-hit} rate is one level higher when using this move.
		/// </summary>	
		/// <remarks>
		/// KARATE-CHOP,RAZOR-LEAF,CRABHAMMER,SLASH,AEROBLAST,CROSS-CHOP,AIR-CUTTER,LEAF-BLADE,NIGHT-SLASH,SHADOW-CLAW,PSYCHO-CUT,STONE-EDGE,ATTACK-ORDER,SPACIAL-REND,DRILL-RUN,SHADOW-BLAST
		/// </remarks>	
		x02C = 44,
		/// <summary>
		/// Hits twice in one turn.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Hits twice in one turn.
		/// </summary>	
		/// <remarks>
		/// DOUBLE-KICK,BONEMERANG,DOUBLE-HIT,DUAL-CHOP,GEAR-GRIND
		/// </remarks>	
		x02D = 45,
		/// <summary>
		/// If the user misses, it takes half the damage it would have inflicted in recoil.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  If this move misses, is blocked by <seealso cref="Moves.PROTECT"/> or <seealso cref="Moves.DETECT"/>, or has no effect, the user takes half the damage it would have inflicted in recoil.  This recoil damage will not exceed half the user's max HP.
		/// 
		/// This move cannot be used while <seealso cref="Moves.GRAVITY"/> is in effect.
		/// </summary>
		/// <remarks>
		/// JUMP-KICK,HIGH-JUMP-KICK
		/// </remarks>	
		x02E = 46,
		/// <summary>
		/// Protects the user's stats from being changed by enemy moves.<para></para> Pokémon on the user's side of the [field]{mechanic:field} are immune to stat-lowering effects for five turns.
		/// 
		/// <seealso cref="Moves.GUARD_SWAP"/>, <seealso cref="Moves.HEART_SWAP"/>, and <seealso cref="Moves.POWER_SWAP"/> may still be used.
		/// 
		/// <seealso cref="Moves.DEFOG"/> used by an opponent will end this effect.
		/// </summary>
		/// <remarks>
		/// MIST
		/// </remarks>	
		x02F = 47,
		/// <summary>
		/// Increases the user's chance to score a critical hit.<para></para> User's [critical hit]{mechanic:critical-hit} rate is two levels higher until it leaves the field.  If the user has already used <seealso cref="Moves.FOCUS_ENERGY"/> since entering the field, this move will [fail]{mechanic:fail}.
		/// 
		/// This effect is passed on by <seealso cref="Moves.BATON_PASS"/>.
		/// </summary>
		/// <remarks>
		/// FOCUS-ENERGY
		/// </remarks>	
		x030 = 48,
		/// <summary>
		/// User receives 1/4 the damage it inflicts in recoil.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  User takes 1/4 the damage it inflicts in recoil.
		/// </summary>	
		/// <remarks>
		/// TAKE-DOWN,SUBMISSION,WILD-CHARGE,HEAD-CHARGE
		/// </remarks>	
		x031 = 49,
		/// <summary>
		/// Confuses the target.<para></para> [Confuse]{mechanic:confuse}s the target.
		/// </summary>	
		/// <remarks>
		/// SUPERSONIC,CONFUSE-RAY,SWEET-KISS,SHADOW-PANIC
		/// </remarks>	
		x032 = 50,
		/// <summary>
		/// Raises the user's Attack by two stages.<para></para> Raises the user's [Attack]{mechanic:attack} by two [stages]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// SWORDS-DANCE
		/// </remarks>	
		x033 = 51,
		/// <summary>
		/// Raises the user's Defense by two stages.<para></para> Raises the user's [Defense]{mechanic:defense} by two [stages]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// BARRIER,ACID-ARMOR,IRON-DEFENSE
		/// </remarks>	
		x034 = 52,
		/// <summary>
		/// Raises the user's Speed by two stages.<para></para> Raises the user's [Speed]{mechanic:speed} by two [stages]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// AGILITY,ROCK-POLISH
		/// </remarks>	
		x035 = 53,
		/// <summary>
		/// Raises the user's Special Attack by two stages.<para></para> Raises the user's [Special Attack]{mechanic:special-attack} by two [stages]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// NASTY-PLOT
		/// </remarks>	
		x036 = 54,
		/// <summary>
		/// Raises the user's Special Defense by two stages.<para></para> Raises the user's [Special Defense]{mechanic:special-defense} by two [stages]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// AMNESIA
		/// </remarks>	
		x037 = 55,
		/// <summary>
		/// User becomes a copy of the target until it leaves battle.<para></para> User copies the target's species, weight, type, [ability]{mechanic:ability}, [calculated stats]{mechanic:calculated-stats} (except [HP]{mechanic:hp}), and moves.  Copied moves will all have 5 [PP]{mechanic:pp} remaining.  [IV]{mechanic:iv}s are copied for the purposes of <seealso cref="Moves.HIDDEN_POWER"/>, but stats are not recalculated.
		/// 
		/// <seealso cref="Items.CHOICE_BAND"/>, <seealso cref="Items.CHOICE_SCARF"/>, and <seealso cref="Items.CHOICE_SPECS"/> stay in effect, and the user must select a new move.
		/// 
		/// This move cannot be copied by <seealso cref="Moves.MIRROR_MOVE"/>, nor forced by <seealso cref="Moves.ENCORE"/>.
		/// </summary>
		/// <remarks>
		/// TRANSFORM
		/// </remarks>	
		x03A = 58,
		/// <summary>
		/// Lowers the target's Attack by two stages.<para></para> Lowers the target's [Attack]{mechanic:attack} by two [stages]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// CHARM,FEATHER-DANCE
		/// </remarks>	
		x03B = 59,
		/// <summary>
		/// Lowers the target's Defense by two stages.<para></para> Lowers the target's [Defense]{mechanic:defense} by two [stages]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// SCREECH,SHADOW-DOWN
		/// </remarks>	
		x03C = 60,
		/// <summary>
		/// Lowers the target's Speed by two stages.<para></para> Lowers the target's [Speed]{mechanic:speed} by two [stages]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// STRING-SHOT,COTTON-SPORE,SCARY-FACE
		/// </remarks>	
		x03D = 61,
		/// <summary>
		/// Lowers the target's Special Attack by two stages.<para></para> Lowers the target's Special Attack by two stages.
		/// </summary>	
		/// <remarks>
		/// EERIE-IMPULSE
		/// </remarks>	
		x03E = 62,
		/// <summary>
		/// Lowers the target's Special Defense by two stages.<para></para> Lowers the target's [Special Defense]{mechanic:special-defense} by two [stages]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// FAKE-TEARS,METAL-SOUND
		/// </remarks>	
		x03F = 63,
		/// <summary>
		/// Reduces damage from physical attacks by half.<para></para> Erects a barrier around the user's side of the field that reduces damage from [physical]{mechanic:physical} attacks by half for five turns.  In double battles, the reduction is 1/3.  [Critical hit]{mechanic:critical-hit}s are not affected by the barrier.
		/// 
		/// If the user is holding <seealso cref="Items.LIGHT_CLAY"/>, the barrier lasts for eight turns.
		/// 
		/// <seealso cref="Moves.BRICK_BREAK"/> or <seealso cref="Moves.DEFOG"/> used by an opponent will destroy the barrier.
		/// </summary>
		/// <remarks>
		/// REFLECT
		/// </remarks>	
		x042 = 66,
		/// <summary>
		/// Poisons the target.<para></para> [Poisons]{mechanic:poisons} the target.
		/// </summary>	
		/// <remarks>
		/// POISON-POWDER,POISON-GAS
		/// </remarks>	
		x043 = 67,
		/// <summary>
		/// Paralyzes the target.<para></para> [Paralyzes]{mechanic:paralyzes} the target.
		/// </summary>	
		/// <remarks>
		/// STUN-SPORE,THUNDER-WAVE,GLARE
		/// </remarks>	
		x044 = 68,
		/// <summary>
		/// Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to lower the target's Attack by one stage.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to lower the target's [Attack]{mechanic:attack} by one [stage]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// AURORA-BEAM,PLAY-ROUGH
		/// </remarks>	
		x045 = 69,
		/// <summary>
		/// Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to lower the target's Defense by one stage.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to lower the target's [Defense]{mechanic:defense} by one [stage]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// IRON-TAIL,CRUNCH,ROCK-SMASH,CRUSH-CLAW,RAZOR-SHELL,SHADOW-BONE,LIQUIDATION
		/// </remarks>	
		x046 = 70,
		/// <summary>
		/// Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to lower the target's Speed by one stage.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to lower the target's [Speed]{mechanic:speed} by one [stage]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// BUBBLE-BEAM,CONSTRICT,BUBBLE,ICY-WIND,ROCK-TOMB,MUD-SHOT,BULLDOZE
		/// </remarks>	
		x047 = 71,
		/// <summary>
		/// Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to lower the target's Special Attack by one stage.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to lower the target's [Special Attack]{mechanic:special-attack} by one [stage]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// MIST-BALL,STRUGGLE-BUG,SNARL,MOONBLAST,MYSTICAL-FIRE
		/// </remarks>	
		x048 = 72,
		/// <summary>
		/// Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to lower the target's Special Defense by one stage.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to lower the target's [Special Defense]{mechanic:special-defense} by one [stage]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// ACID,PSYCHIC,SHADOW-BALL,LUSTER-PURGE,BUG-BUZZ,FOCUS-BLAST,ENERGY-BALL,EARTH-POWER,FLASH-CANNON
		/// </remarks>	
		x049 = 73,
		/// <summary>
		/// Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to lower the target's accuracy by one stage.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to lower the target's [accuracy]{mechanic:accuracy} by one [stage]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// MUD-SLAP,OCTAZOOKA,MUDDY-WATER,MUD-BOMB,MIRROR-SHOT,LEAF-TORNADO,NIGHT-DAZE
		/// </remarks>	
		x04A = 74,
		/// <summary>
		/// XXX new effect for mystical-fire<para></para> XXX new effect for mystical-fire
		/// </summary>
		x04B = 75,
		/// <summary>
		/// User charges for one turn before attacking.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to make the target flinch.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  User charges for one turn before attacking.  [Critical hit chance]{mechanic:critical-hit-chance} is one level higher than normal.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to make the target [flinch]{mechanic:flinch}.
		/// 
		/// This move cannot be selected by <seealso cref="Moves.SLEEP_TALK"/>.
		/// </summary>
		/// <remarks>
		/// SKY-ATTACK
		/// </remarks>	
		x04C = 76,
		/// <summary>
		/// Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to confuse the target.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to [confuse]{mechanic:confuse} the target.
		/// </summary>	
		/// <remarks>
		/// PSYBEAM,CONFUSION,DIZZY-PUNCH,DYNAMIC-PUNCH,SIGNAL-BEAM,WATER-PULSE,ROCK-CLIMB
		/// </remarks>	
		x04D = 77,
		/// <summary>
		/// Hits twice in the same turn.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to poison the target.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Hits twice in the same turn.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to <see cref="Attack.MoveMetaAilments.POISON"/> the target.
		/// </summary>	
		/// <remarks>
		/// TWINEEDLE
		/// </remarks>	
		x04E = 78,
		/// <summary>
		/// Never misses.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Ignores [accuracy]{mechanic:accuracy} and [evasion]{mechanic:evasion} modifiers.
		/// </summary>	
		/// <remarks>
		/// VITAL-THROW
		/// </remarks>	
		x04F = 79,
		/// <summary>
		/// Transfers 1/4 of the user's max HP into a doll, protecting the user from further damage or status changes until it breaks.<para></para> Transfers 1/4 the user's max [HP]{mechanic:hp} into a doll that absorbs damage and causes most negative move effects to [fail]{mechanic:fail}.  If the user leaves the [field]{mechanic:field}, the doll will vanish.  If the user cannot pay the [HP]{mechanic:hp} cost, this move will fail.
		/// 
		/// The doll takes damage as normal, using the user's stats and types, and will break when its [HP]{mechanic:hp} reaches zero.  Self-inflicted damage from <see cref="Attack.MoveMetaAilments.CONFUSION"/> or recoil is not absorbed.  Healing effects from opponents ignore the doll and heal the user as normal.  Moves that work based on the user's [HP]{mechanic:hp} still do so; the doll's [HP]{mechanic:hp} does not influence any move.
		/// 
		/// The doll will block [major status effect]{mechanic:major-status-effect}s, <see cref="Attack.MoveMetaAilments.CONFUSION"/>, and [flinch]{mechanic:flinch}ing.  The effects of <seealso cref="Moves.SMELLING_SALTS"/> and <seealso cref="Moves.WAKE_UP-SLAP"/> do not trigger against a doll, even if the Pokémon behind the doll has the appropriate [major status effect]{mechanic:major-status-effect}.  Multi-turn trapping moves like <seealso cref="Moves.WRAP"/> will hit the doll for their <seealso cref="Attack.MetaCategory.DAMAGE"/>, but the multi-turn trapping and damage effects will not activate.
		/// 
		/// Moves blocked or damage absorbed by the doll do not count as hitting the user or inflicting damage for any effects that respond to such, e.g., <seealso cref="Moves.AVALANCHE"/>, <seealso cref="Moves.COUNTER"/>, or a <seealso cref="Items.ROWAP_BERRY"/>.  <seealso cref="Moves.MAGIC_COAT"/> still works as normal, even against moves the doll would block.  Opposing Pokémon that damage the doll with a leech move like <seealso cref="Moves.ABSORB"/> are healed as normal.
		/// 
		/// It will also block <seealso cref="Moves.ACUPRESSURE"/>, <seealso cref="Moves.BLOCK"/>, the curse effect of <seealso cref="Moves.CURSE"/>, <seealso cref="Moves.DREAM_EATER"/>, <seealso cref="Moves.EMBARGO"/>, <seealso cref="Moves.FLATTER"/>, <seealso cref="Moves.GASTRO_ACID"/>, <seealso cref="Moves.GRUDGE"/>, <seealso cref="Moves.HEAL_BLOCK"/>, <seealso cref="Moves.LEECH_SEED"/>, <seealso cref="Moves.LOCK_ON"/>, <seealso cref="Moves.MEAN_LOOK"/>, <seealso cref="Moves.MIMIC"/>, <seealso cref="Moves.MIND_READER"/>, <seealso cref="Moves.NIGHTMARE"/>, <seealso cref="Moves.PAIN_SPLIT"/>, <seealso cref="Moves.PSYCHO_SHIFT"/>, <seealso cref="Moves.SPIDER_WEB"/>, <seealso cref="Moves.SKETCH"/>, <seealso cref="Moves.SWAGGER"/>, <seealso cref="Moves.SWITCHEROO"/>, <seealso cref="Moves.TRICK"/>, <seealso cref="Moves.WORRY_SEED"/>, and <seealso cref="Moves.YAWN"/>.  A Pokémon affected by <seealso cref="Moves.YAWN"/> before summoning the doll will still fall to <see cref="Attack.MoveMetaAilments.SLEEP"/>.
		/// 
		/// The doll blocks <seealso cref="Abilities.INTIMIDATE"/>, but all other abilities act as though the doll did not exist.  If the user has an ability that absorbs moves of a certain type for [HP]{mechanic:hp} (such as <seealso cref="Abilities.VOLT_ABSORB"/> absorbing <seealso cref="Moves.THUNDER_WAVE"/>), such moves will not be blocked.
		/// 
		/// <seealso cref="Items.LIFE_ORB"/> and berries that cause <see cref="Attack.MoveMetaAilments.CONFUSION"/> still work as normal, but their respective [HP]{mechanic:hp} loss and <see cref="Attack.MoveMetaAilments.CONFUSION"/> are absorbed/blocked by the doll.
		/// 
		/// The user is still vulnerable to damage inflicted when entering or leaving the [field]{mechanic:field}, such as by <seealso cref="Moves.PURSUIT"/> or <seealso cref="Moves.SPIKES"/>; however, the doll will block the <see cref="Attack.MoveMetaAilments.POISON"/> effect of <seealso cref="Moves.TOXIC_SPIKES"/>.
		/// 
		/// The doll is passed on by <seealso cref="Moves.BATON_PASS"/>.  It keeps its existing [HP]{mechanic:hp}, but uses the replacement Pokémon's stats and types for damage calculation.
		/// 
		/// All other effects work as normal.
		/// </summary>
		/// <remarks>
		/// SUBSTITUTE
		/// </remarks>	
		x050 = 80,
		/// <summary>
		/// User foregoes its next turn to recharge.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  User loses its next turn to "recharge", and cannot attack or [switch]{mechanic:switch} out during that turn.
		/// </summary>	
		/// <remarks>
		/// HYPER-BEAM,BLAST-BURN,HYDRO-CANNON,FRENZY-PLANT,GIGA-IMPACT,ROCK-WRECKER,ROAR-OF-TIME,PRISMATIC-LASER
		/// </remarks>	
		x051 = 81,
		/// <summary>
		/// If the user is hit after using this move, its Attack rises by one stage.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Every time the user is hit after it uses this move but before its next action, its [Attack]{mechanic:attack} raises by one [stage]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// RAGE
		/// </remarks>	
		x052 = 82,
		/// <summary>
		/// Copies the target's last used move.<para></para> This move is replaced by the target's last successfully used move, and its PP changes to 5.  If the target hasn't used a move since entering the field, if it tried to use a move this turn and [failed]{mechanic:fail}, or if the user already knows the targeted move, this move will fail.  This effect vanishes when the user leaves the field.
		/// 
		/// If <seealso cref="Moves.CHATTER"/>, <seealso cref="Moves.METRONOME"/>, <seealso cref="Moves.MIMIC"/>, <seealso cref="Moves.SKETCH"/>, or <seealso cref="Moves.STRUGGLE"/> is selected, this move will [fail]{mechanic:fail}.
		/// 
		/// This move cannot be copied by <seealso cref="Moves.MIRROR_MOVE"/>, nor selected by <seealso cref="Moves.ASSIST"/> or <seealso cref="Moves.METRONOME"/>, nor forced by <seealso cref="Moves.ENCORE"/>.
		/// </summary>
		/// <remarks>
		/// MIMIC
		/// </remarks>	
		x053 = 83,
		/// <summary>
		/// Randomly selects and uses any move in the game.<para></para> Selects any move at random and uses it.  Moves the user already knows are not eligible.  Assist, meta, protection, and reflection moves are also not eligible; specifically, <seealso cref="Moves.ASSIST"/>, <seealso cref="Moves.CHATTER"/>, <seealso cref="Moves.COPYCAT"/>, <seealso cref="Moves.COUNTER"/>, <seealso cref="Moves.COVET"/>, <seealso cref="Moves.DESTINY_BOND"/>, <seealso cref="Moves.DETECT"/>, <seealso cref="Moves.ENDURE"/>, <seealso cref="Moves.FEINT"/>, <seealso cref="Moves.FOCUS_PUNCH"/>, <seealso cref="Moves.FOLLOW_ME"/>, <seealso cref="Moves.HELPING_HAND"/>, <seealso cref="Moves.ME_FIRST"/>, <seealso cref="Moves.METRONOME"/>, <seealso cref="Moves.MIMIC"/>, <seealso cref="Moves.MIRROR_COAT"/>, <seealso cref="Moves.MIRROR_MOVE"/>, <seealso cref="Moves.PROTECT"/>, <seealso cref="Moves.QUICK_GUARD"/>, <seealso cref="Moves.SKETCH"/>, <seealso cref="Moves.SLEEP_TALK"/>, <seealso cref="Moves.SNATCH"/>, <seealso cref="Moves.STRUGGLE"/>, <seealso cref="Moves.SWITCHEROO"/>, <seealso cref="Moves.THIEF"/>, <seealso cref="Moves.TRICK"/>, and <seealso cref="Moves.WIDE_GUARD"/> will not be selected by this move.
		/// 
		/// This move cannot be copied by <seealso cref="Moves.MIMIC"/> or <seealso cref="Moves.MIRROR_MOVE"/>, nor selected by <seealso cref="Moves.ASSIST"/>, <seealso cref="Moves.METRONOME"/>, or <seealso cref="Moves.SLEEP_TALK"/>.
		/// </summary>
		/// <remarks>
		/// METRONOME
		/// </remarks>	
		x054 = 84,
		/// <summary>
		/// Seeds the target, stealing HP from it every turn.<para></para> Plants a seed on the target that [drains]{mechanic:drain} 1/8 of its max [HP]{mechanic:hp} at the end of every turn and heals the user for the amount taken.  Has no effect on <seealso cref="Types.GRASS"/> Pokémon.  The seed remains until the target leaves the field.
		/// 
		/// The user takes damage instead of being healed if the target has <seealso cref="Abilities.LIQUID_OOZE"/>.
		/// 
		/// <seealso cref="Moves.RAPID_SPIN"/> will remove this effect.
		/// 
		/// This effect is passed on by <seealso cref="Moves.BATON_PASS"/>.
		/// </summary>
		/// <remarks>
		/// LEECH-SEED
		/// </remarks>	
		x055 = 85,
		/// <summary>
		/// Does nothing.<para></para> Does nothing.
		/// 
		/// This move cannot be used while <seealso cref="Moves.GRAVITY"/> is in effect.
		/// </summary>	
		/// <remarks>
		/// SPLASH
		/// </remarks>	
		x056 = 86,
		/// <summary>
		/// Disables the target's last used move for 1-8 turns.<para></para> Disables the target's last used move, preventing its use for 4–7 turns, selected at random, or until the target leaves the [field]{mechanic:field}.  If the target hasn't used a move since entering the [field]{mechanic:field}, if it tried to use a move this turn and [failed]{mechanic:failed},  if its last used move has 0 PP remaining, or if it already has a move disabled, this move will fail.
		/// </summary>	
		/// <remarks>
		/// DISABLE
		/// </remarks>	
		x057 = 87,
		/// <summary>
		/// Inflicts damage equal to the user's level.<para></para> Inflicts damage equal to the user's level.  Type immunity applies, but other type effects are ignored.
		/// </summary>	
		/// <remarks>
		/// SEISMIC-TOSS,NIGHT-SHADE
		/// </remarks>	
		x058 = 88,
		/// <summary>
		/// Inflicts damage between 50% and 150% of the user's level.<para></para> Inflicts [typeless]{mechanic:typeless} damage between 50% and 150% of the user's level, selected at random in increments of 10%.
		/// </summary>	
		/// <remarks>
		/// PSYWAVE
		/// </remarks>	
		x059 = 89,
		/// <summary>
		/// Inflicts twice the damage the user received from the last physical hit it took.<para></para> Targets the last opposing Pokémon to hit the user with a [physical]{mechanic:physical} move this turn.  Inflicts twice the damage that move did to the user.  If there is no eligible target, this move will [fail]{mechanic:fail}.  Type immunity applies, but other type effects are ignored.
		/// 
		/// This move cannot be copied by <seealso cref="Moves.MIRROR_MOVE"/>, nor selected by <seealso cref="Moves.ASSIST"/> or <seealso cref="Moves.METRONOME"/>.
		/// </summary>	
		/// <remarks>
		/// COUNTER
		/// </remarks>	
		x05A = 90,
		/// <summary>
		/// Forces the target to repeat its last used move every turn for 2 to 6 turns.<para></para> The next 4–8 times (selected at random) the target attempts to move, it is forced to repeat its last used move.  If the selected move allows the trainer to select a target, an opponent will be selected at random each turn.  If the target is prevented from using the selected move by some other effect, <seealso cref="Moves.STRUGGLE"/> will be used instead.  This effect ends if the selected move runs out of [PP]{mechanic:pp}.
		/// 
		/// If the target hasn't used a move since entering the [field]{mechanic:field}, if it tried to use a move this turn and [failed]{mechanic:failed}, if it does not know the selected move, or if the selected move has 0 [PP]{mechanic:pp} remaining, this move will fail.  If the target's last used move was <seealso cref="Moves.ENCORE"/>, <seealso cref="Moves.MIMIC"/>, <seealso cref="Moves.MIRROR_MOVE"/>, <seealso cref="Moves.SKETCH"/>, <seealso cref="Moves.STRUGGLE"/>, or <seealso cref="Moves.TRANSFORM"/>, this move will fail.
		/// </summary>
		/// <remarks>
		/// ENCORE
		/// </remarks>	
		x05B = 91,
		/// <summary>
		/// Sets the user's and targets's HP to the average of their current HP.<para></para> Changes the user's and target's remaining [HP]{mechanic:hp} to the average of their current remaining [HP]{mechanic:hp}.  Ignores [accuracy]{mechanic:accuracy} and [evasion]{mechanic:evasion} modifiers.  This effect does not count as inflicting damage for other moves and effects that respond to damage taken.
		/// 
		/// This effect [fail]{mechanic:fail}s against a <seealso cref="Moves.SUBSTITUTE"/>.
		/// </summary>
		/// <remarks>
		/// PAIN-SPLIT
		/// </remarks>	
		x05C = 92,
		/// <summary>
		/// Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to make the target flinch.  Only works if the user is sleeping.<para></para> Only usable if the user is <see cref="Attack.MoveMetaAilments.SLEEP"/>ing.  Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to make the target <seealso cref="Mechanics.FLINCH"/>.
		/// </summary>
		/// <remarks>
		/// SNORE
		/// </remarks>	
		x05D = 93,
		/// <summary>
		/// Changes the user's type to a random type either resistant or immune to the last move used against it.<para></para> Changes the user's type to a type either resistant or immune to the last damaging move that hit it.  The new type is selected at random and cannot be a type the user already is.  If there is no eligible new type, this move will [fail]{mechanic:fail}.
		/// </summary>	
		/// <remarks>
		/// CONVERSION-2
		/// </remarks>	
		x05E = 94,
		/// <summary>
		/// Ensures that the user's next move will hit the target.<para></para> If the user targets the same target again before the end of the next turn, the move it uses is guaranteed to hit.  This move itself also ignores [accuracy]{mechanic:accuracy} and [evasion]{mechanic:evasion} modifiers.
		/// 
		/// One-hit KO moves are also guaranteed to hit, as long as the user is equal or higher level than the target.  This effect also allows the user to hit Pokémon that are off the field due to moves such as <seealso cref="Moves.DIG"/> or <seealso cref="Moves.FLY"/>.
		/// 
		/// If the target uses <seealso cref="Moves.DETECT"/> or <seealso cref="Moves.PROTECT"/> while under the effect of this move, the user is not guaranteed to hit, but has a (100 - accuracy)% chance to break through the protection.
		/// 
		/// This effect is passed on by <seealso cref="Moves.BATON_PASS"/>.
		/// </summary>	
		/// <remarks>
		/// MIND-READER,LOCK-ON
		/// </remarks>	
		x05F = 95,
		/// <summary>
		/// Permanently becomes the target's last used move.<para></para> Permanently replaces itself with the target's last used move.  If that move is <seealso cref="Moves.CHATTER"/> or <seealso cref="Moves.STRUGGLE"/>, this move will [fail]{mechanic:fail}.
		/// 
		/// This move cannot be copied by <seealso cref="Moves.MIMIC"/> or <seealso cref="Moves.MIRROR_MOVE"/>, nor selected by <seealso cref="Moves.ASSIST"/> or <seealso cref="Moves.METRONOME"/>, nor forced by <seealso cref="Moves.ENCORE"/>.
		/// </summary>
		/// <remarks>
		/// SKETCH
		/// </remarks>	
		x060 = 96,
		/// <summary>
		/// Randomly uses one of the user's other three moves.  Only works if the user is sleeping.<para></para> Only usable if the user is <see cref="Attack.MoveMetaAilments.SLEEP"/>ing.  Randomly selects and uses one of the user's other three moves.  Use of the selected move requires and costs 0 [PP]{mechanic:pp}.
		/// 
		/// This move will not select <seealso cref="Moves.ASSIST"/>, <seealso cref="Moves.BIDE"/>, <seealso cref="Moves.BOUNCE"/>, <seealso cref="Moves.CHATTER"/>, <seealso cref="Moves.COPYCAT"/>, <seealso cref="Moves.DIG"/>, <seealso cref="Moves.DIVE"/>, <seealso cref="Moves.FLY"/>, <seealso cref="Moves.FOCUS_PUNCH"/>, <seealso cref="Moves.ME_FIRST"/>, <seealso cref="Moves.METRONOME"/>, <seealso cref="Moves.MIRROR_MOVE"/>, <seealso cref="Moves.SHADOW_FORCE"/>, <seealso cref="Moves.SKULL_BASH"/>, <seealso cref="Moves.SKY_ATTACK"/>, <seealso cref="Moves.SKY_DROP"/>, <seealso cref="Moves.SLEEP_TALK"/>, <seealso cref="Moves.SOLAR_BEAM"/>, <seealso cref="Moves.RAZOR_WIND"/>, or <seealso cref="Moves.UPROAR"/>.
		/// 
		/// If the selected move requires a recharge turn—i.e., one of <seealso cref="Moves.BLAST_BURN"/>, <seealso cref="Moves.FRENZY_PLANT"/>, <seealso cref="Moves.GIGA_IMPACT"/>, <seealso cref="Moves.HYDRO_CANNON"/>, <seealso cref="Moves.HYPER_BEAM"/>, <seealso cref="Moves.ROAR_OF-TIME"/>, or <seealso cref="Moves.ROCK_WRECKER"/>—and the user is still <see cref="Attack.MoveMetaAilments.SLEEP"/>ing next turn, then it's forced to use this move again and pay another [PP]{mechanic:pp} for the recharge turn.
		/// 
		/// This move cannot be copied by <seealso cref="Moves.MIRROR_MOVE"/>, nor selected by <seealso cref="Moves.ASSIST"/>, <seealso cref="Moves.METRONOME"/>, or <seealso cref="Moves.SLEEP_TALK"/>.
		/// </summary>
		/// <remarks>
		/// SLEEP-TALK
		/// </remarks>	
		x062 = 98,
		/// <summary>
		/// If the user faints this turn, the target automatically will, too.<para></para> If the user [faints]{mechanic:faints} before its next move, the Pokémon that [faint]{mechanic:faint}ed it will automatically [faint]{mechanic:faint}.  End-of-turn damage is ignored.
		/// 
		/// This move cannot be selected by <seealso cref="Moves.ASSIST"/> or <seealso cref="Moves.METRONOME"/>.
		/// </summary>
		/// <remarks>
		/// DESTINY-BOND
		/// </remarks>	
		x063 = 99,
		/// <summary>
		/// Inflicts more damage when the user has less HP remaining, with a maximum of 200 power.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Power varies inversely with the user's proportional remaining [HP]{mechanic:hp}.
		/// 
		/// 64 * current HP / max HP | Power
		/// -----------------------: | ----:
		///  0– 1                    |  200
		///  2– 5                    |  150
		///  6–12                    |  100
		/// 13–21                    |   80
		/// 22–42                    |   40
		/// 43–64                    |   20
		/// </summary>
		/// <remarks>
		/// FLAIL,REVERSAL
		/// </remarks>	
		x064 = 100,
		/// <summary>
		/// Lowers the PP of the target's last used move by 4.<para></para> Lowers the PP of the target's last used move by 4.  If the target hasn't used a move since entering the [field]{mechanic:field}, if it tried to use a move this turn and [failed]{mechanic:failed}, or if its last used move has 0 PP remaining, this move will fail.
		/// </summary>	
		/// <remarks>
		/// SPITE
		/// </remarks>	
		x065 = 101,
		/// <summary>
		/// Cannot lower the target's HP below 1.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Will not reduce the target's [HP]{mechanic:hp} below 1.
		/// </summary>	
		/// <remarks>
		/// FALSE-SWIPE,HOLD-BACK
		/// </remarks>	
		x066 = 102,
		/// <summary>
		/// Cures the entire party of major status effects.<para></para> Removes [major status effects]{mechanic:major-status-effects} and <see cref="Attack.MoveMetaAilments.CONFUSION"/> from every Pokémon in the user's party.
		/// </summary>	
		/// <remarks>
		/// HEAL-BELL,AROMATHERAPY
		/// </remarks>	
		x067 = 103,
		/// <summary>
		/// Inflicts regular damage with no additional effect.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.
		/// </summary>	
		/// <remarks>
		/// QUICK-ATTACK,MACH-PUNCH,EXTREME-SPEED,VACUUM-WAVE,BULLET-PUNCH,ICE-SHARD,SHADOW-SNEAK,AQUA-JET,ACCELEROCK
		/// </remarks>	
		x068 = 104,
		/// <summary>
		/// Hits three times, increasing power by 100% with each successful hit.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Hits three times in the same turn.  The second hit has double power, and the third hit has triple power.  Each hit has a separate [accuracy]{mechanic:accuracy} check, and this move stops if a hit misses.
		/// 
		/// <seealso cref="Abilities.SKILL_LINK"/> does not apply.
		/// </summary>
		/// <remarks>
		/// TRIPLE-KICK
		/// </remarks>	
		x069 = 105,
		/// <summary>
		/// Takes the target's item.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  If the target is holding an item and the user is not, the user will permanently take the item.  Damage is still inflicted if an item cannot be taken.
		/// 
		/// Pokémon with <seealso cref="Abilities.STICKY_HOLD"/> or <seealso cref="Abilities.MULTITYPE"/> are immune to the item theft effect.
		/// 
		/// The target cannot recover its item with <seealso cref="Moves.RECYCLE"/>.
		/// 
		/// This move cannot be selected by <seealso cref="Moves.ASSIST"/> or <seealso cref="Moves.METRONOME"/>.
		/// </summary>
		/// <remarks>
		/// THIEF,COVET
		/// </remarks>	
		x06A = 106,
		/// <summary>
		/// Prevents the target from leaving battle.<para></para> The target cannot [switch out]{mechanic:switch-out} normally.  Ignores [accuracy]{mechanic:accuracy} and [evasion]{mechanic:evasion} modifiers.  This effect ends when the user leaves the [field]{mechanic:field}.
		/// 
		/// The target may still escape by using <seealso cref="Moves.BATON_PASS"/>, <seealso cref="Moves.U_TURN"/>, or a <seealso cref="Items.SHED_SHELL"/>.
		/// 
		/// Both the user and the target pass on this effect with <seealso cref="Moves.BATON_PASS"/>.
		/// </summary>
		/// <remarks>
		/// SPIDER-WEB,MEAN-LOOK,BLOCK,SHADOW-HOLD
		/// </remarks>	
		x06B = 107,
		/// <summary>
		/// Target loses 1/4 its max HP every turn as long as it's asleep.<para></para> Only works on <see cref="Attack.MoveMetaAilments.SLEEP"/>ing Pokémon.  Gives the target a nightmare, damaging it for 1/4 its max [HP]{mechanic:hp} every turn.  If the target wakes up or leaves the [field]{mechanic:field}, this effect ends.
		/// </summary>	
		/// <remarks>
		/// NIGHTMARE
		/// </remarks>	
		x06C = 108,
		/// <summary>
		/// Raises the user's evasion by two stages.<para></para> Raises the user's [evasion]{mechanic:evasion} by two [stages]{mechanic:stage}.
		/// 
		/// <seealso cref="Moves.STOMP"/> and <seealso cref="Moves.STEAMROLLER"/> have double power against Pokémon that have used this move since entering the [field]{mechanic:field}.
		/// </summary>
		/// <remarks>
		/// MINIMIZE
		/// </remarks>	
		x06D = 109,
		/// <summary>
		/// Ghosts pay half their max HP to hurt the target every turn.  Others decrease Speed but raise Attack and Defense.<para></para> If the user is a <seealso cref="Types.GHOST"/>: user pays half its max [HP]{mechanic:hp} to place a curse on the target, damaging it for 1/4 its max [HP]{mechanic:hp} every turn.
		/// Otherwise: Lowers the user's [Speed]{mechanic:speed} by one [stage]{mechanic:stage}, and raises its [Attack]{mechanic:attack} and [Defense]{mechanic:defense} by one [stage]{mechanic:stage} each.
		/// 
		/// The curse effect is passed on by <seealso cref="Moves.BATON_PASS"/>.
		/// 
		/// This move cannot be copied by <seealso cref="Moves.MIRROR_MOVE"/>.
		/// </summary>
		/// <remarks>
		/// CURSE
		/// </remarks>	
		x06E = 110,
		/// <summary>
		/// Prevents any moves from hitting the user this turn.<para></para> No moves will hit the user for the remainder of this turn.  If the user is last to act this turn, this move will [fail]{mechanic:fail}.
		/// 
		/// If the user successfully used <seealso cref="Moves.DETECT"/>, <seealso cref="Moves.ENDURE"/>, <seealso cref="Moves.PROTECT"/>, <seealso cref="Moves.QUICK_GUARD"/>, or <seealso cref="Moves.WIDE_GUARD"/> on the last turn, this move has a 50% chance to [fail]{mechanic:fail}.
		/// 
		/// <seealso cref="Moves.LOCK_ON"/>, <seealso cref="Moves.MIND_READER"/>, and <seealso cref="Abilities.NO_GUARD"/> provide a (100 – accuracy)% chance for moves to break through this move.  This does not apply to one-hit KO moves (<seealso cref="Moves.FISSURE"/>, <seealso cref="Moves.GUILLOTINE"/>, <seealso cref="Moves.HORN_DRILL"/>, and <seealso cref="Moves.SHEER_COLD"/>); those are always blocked by this move.
		/// 
		/// <seealso cref="Moves.THUNDER"/> during <seealso cref="Moves.RAIN_DANCE"/> and <seealso cref="Moves.BLIZZARD"/> during <seealso cref="Moves.HAIL"/> have a 30% chance to break through this move.
		/// 
		/// The following effects are not prevented by this move:
		/// 
		/// * <seealso cref="Moves.ACUPRESSURE"/> from an ally
		/// * <seealso cref="Moves.CURSE"/>'s curse effect
		/// * Delayed damage from <seealso cref="Moves.DOOM_DESIRE"/> and <seealso cref="Moves.FUTURE_SIGHT"/>; however, these moves will be prevented if they are used this turn
		/// * <seealso cref="Moves.FEINT"/>, which will also end this move's protection after it hits
		/// * <seealso cref="Moves.IMPRISON"/>
		/// * <seealso cref="Moves.PERISH_SONG"/>
		/// * <seealso cref="Moves.SHADOW_FORCE"/>
		/// * Moves that merely copy the user, such as <seealso cref="Moves.TRANSFORM"/> or <seealso cref="Moves.PSYCH_UP"/>
		/// 
		/// This move cannot be selected by <seealso cref="Moves.ASSIST"/> or <seealso cref="Moves.METRONOME"/>.
		/// </summary>
		/// <remarks>
		/// PROTECT,DETECT
		/// </remarks>	
		x070 = 112,
		/// <summary>
		/// Scatters Spikes, hurting opposing Pokémon that switch in.<para></para> Scatters spikes around the opposing [field]{mechanic:field}, which damage opposing Pokémon that enter the [field]{mechanic:field} for 1/8 of their max [HP]{mechanic:hp}.  Pokémon immune to <seealso cref="Types.GROUND"/> moves are immune to this damage, except during <seealso cref="Moves.GRAVITY"/>.  Up to three layers of spikes may be laid down, adding 1/16 to the damage done: two layers of spikes damage for 3/16 max [HP]{mechanic:hp}, and three layers damage for 1/4 max [HP]{mechanic:hp}.
		/// 
		/// <seealso cref="Abilities.WONDER_GUARD"/> does not block damage from this effect.
		/// 
		/// <seealso cref="Moves.RAPID_SPIN"/> removes this effect from its user's side of the [field]{mechanic:field}.  <seealso cref="Moves.DEFOG"/> removes this effect from its target's side of the [field]{mechanic:field}.
		/// </summary>
		/// <remarks>
		/// SPIKES
		/// </remarks>	
		x071 = 113,
		/// <summary>
		/// Forces the target to have no Evade, and allows it to be hit by Normal and Fighting moves even if it's a Ghost.<para></para> Resets the target's [evasion]{mechanic:evasion} to normal and prevents any further boosting until the target leaves the [field]{mechanic:field}.  A <seealso cref="Types.GHOST"/> under this effect takes normal damage from <seealso cref="Types.NORMAL"/> and <seealso cref="Types.FIGHTING"/> moves.  This move itself ignores [accuracy]{mechanic:accuracy} and [evasion]{mechanic:evasion} modifiers.
		/// </summary>
		/// <remarks>
		/// FORESIGHT,ODOR-SLEUTH
		/// </remarks>	
		x072 = 114,
		/// <summary>
		/// User and target both faint after three turns.<para></para> Every Pokémon is given a counter that starts at 3 and decreases by 1 at the end of every turn, including this one.  When a Pokémon's counter reaches zero, that Pokémon [faint]{mechanic:faint}s.  A Pokémon that leaves the [field]{mechanic:field} will lose its counter; its replacement does not inherit the effect, and other Pokémon's counters remain.
		/// 
		/// This effect is passed on by <seealso cref="Moves.BATON_PASS"/>.
		/// 
		/// This move cannot be copied by <seealso cref="Moves.MIRROR_MOVE"/>.
		/// </summary>
		/// <remarks>
		/// PERISH-SONG
		/// </remarks>	
		x073 = 115,
		/// <summary>
		/// Changes the weather to a sandstorm for five turns.<para></para> Changes the weather to a sandstorm for five turns.  Pokémon that are not <seealso cref="Types.GROUND"/>, <seealso cref="Types.ROCK"/>, or <seealso cref="Types.STEEL"/> take 1/16 their max [HP]{mechanic:hp} at the end of every turn.  Every <seealso cref="Types.ROCK"/> Pokémon's original [Special Defense]{mechanic:special-defense} is raised by 50% for the duration of this effect.
		/// 
		/// <seealso cref="Moves.SOLAR_BEAM"/>'s power is halved.
		/// 
		/// <seealso cref="Moves.MOONLIGHT"/>, <seealso cref="Moves.MORNING_SUN"/>, and <seealso cref="Moves.SYNTHESIS"/> only heal 1/4 the user's max [HP]{mechanic:hp}.
		/// </summary>
		/// <remarks>
		/// SANDSTORM
		/// </remarks>	
		x074 = 116,
		/// <summary>
		/// Prevents the user's HP from lowering below 1 this turn.<para></para> The user's [HP]{mechanic:hp} cannot be lowered below 1 by any means for the remainder of this turn.
		/// 
		/// If the user successfully used <seealso cref="Moves.DETECT"/>, <seealso cref="Moves.ENDURE"/>, <seealso cref="Moves.PROTECT"/>, <seealso cref="Moves.QUICK_GUARD"/>, or <seealso cref="Moves.WIDE_GUARD"/> on the last turn, this move has a 50% chance to [fail]{mechanic:fail}.
		/// 
		/// This move cannot be selected by <seealso cref="Moves.ASSIST"/> or <seealso cref="Moves.METRONOME"/>.
		/// </summary>
		/// <remarks>
		/// ENDURE
		/// </remarks>	
		x075 = 117,
		/// <summary>
		/// Power doubles every turn this move is used in succession after the first, resetting after five turns.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  User is forced to use this move for five turns.  Power doubles every time this move is used in succession to a maximum of 16x, and resets to normal after the lock-in ends.  If this move misses or becomes unusable, the lock-in ends.
		/// 
		/// If the user has used <seealso cref="Moves.DEFENSE_CURL"/> since entering the field, this move has double power.
		/// </summary>
		/// <remarks>
		/// ROLLOUT,ICE-BALL
		/// </remarks>	
		x076 = 118,
		/// <summary>
		/// Raises the target's Attack by two stages and confuses the target.<para></para> Raises the target's [Attack]{mechanic:attack} by two [stages]{mechanic:stage}, then [confuses]{mechanic:confuses} it.  If the target's [Attack]{mechanic:attack} cannot be [raised]{mechanic:raised} by two [stages]{mechanic:stage}, the <see cref="Attack.MoveMetaAilments.CONFUSION"/> is not applied.
		/// </summary>	
		/// <remarks>
		/// SWAGGER
		/// </remarks>	
		x077 = 119,
		/// <summary>
		/// Power doubles every turn this move is used in succession after the first, maxing out after five turns.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Power doubles after every time this move is used, whether consecutively or not, maxing out at 16x.  If this move misses or the user leaves the [field]{mechanic:field}, power resets.
		/// </summary>	
		/// <remarks>
		/// FURY-CUTTER
		/// </remarks>	
		x078 = 120,
		/// <summary>
		/// Target falls in love if it has the opposite gender, and has a 50% chance to refuse attacking the user.<para></para> Causes the target to fall in love with the user, giving it a 50% chance to do nothing each turn.  If the user and target are the same gender, or either is genderless, this move will [fail]{mechanic:fail}.  If either Pokémon leaves the [field]{mechanic:field}, this effect ends.
		/// </summary>	
		/// <remarks>
		/// ATTRACT
		/// </remarks>	
		x079 = 121,
		/// <summary>
		/// Power increases with happiness, up to a maximum of 102.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Power increases with [happiness]{mechanic:happiness}, given by `happiness * 2 / 5`, to a maximum of 102.  Power bottoms out at 1.
		/// </summary>	
		/// <remarks>
		/// RETURN
		/// </remarks>	
		x07A = 122,
		/// <summary>
		/// Randomly inflicts damage with power from 40 to 120 or heals the target for 1/4 its max HP.<para></para> Randomly uses one of the following effects.
		/// 
		/// Effect                                             | Chance
		/// -------------------------------------------------- | -----:
		/// Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/> with 40 power  |    40%
		/// Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/> with 80 power  |    30%
		/// Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/> with 120 power |    10%
		/// Heals the target for 1/4 its max [HP]{mechanic:hp}    |    20%
		/// 
		/// On average, this move inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/> with 52 power and heals the target for 1/20 its max [HP]{mechanic:hp}.
		/// </summary>
		/// <remarks>
		/// PRESENT
		/// </remarks>	
		x07B = 123,
		/// <summary>
		/// Power increases as happiness decreases, up to a maximum of 102.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Power increases inversely with [happiness]{mechanic:happiness}, given by `(255 - happiness) * 2 / 5`, to a maximum of 102.  Power bottoms out at 1.
		/// </summary>	
		/// <remarks>
		/// FRUSTRATION
		/// </remarks>	
		x07C = 124,
		/// <summary>
		/// Protects the user's field from major status ailments and confusion for five turns.<para></para> Protects Pokémon on the user's side of the [field]{mechanic:field} from [major status]{mechanic:major-status} effects and <see cref="Attack.MoveMetaAilments.CONFUSION"/> for five turns.  Does not cancel existing ailments.  This effect remains even if the user leaves the [field]{mechanic:field}.
		/// 
		/// If <seealso cref="Moves.YAWN"/> is used while this move is in effect, it will immediately [fail]{mechanic:fail}.
		/// 
		/// <seealso cref="Moves.DEFOG"/> used by an opponent will end this effect.
		/// 
		/// This effect does not prevent the <see cref="Attack.MoveMetaAilments.CONFUSION"/> caused by <seealso cref="Moves.OUTRAGE"/>, <seealso cref="Moves.PETAL_DANCE"/>, or <seealso cref="Moves.THRASH"/>.
		/// </summary>	
		/// <remarks>
		/// SAFEGUARD
		/// </remarks>	
		x07D = 125,
		/// <summary>
		/// Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to <see cref="Attack.MoveMetaAilments.BURN"/> the target.  Lets frozen Pokémon thaw themselves.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to <see cref="Attack.MoveMetaAilments.BURN"/> the target.  [Frozen]{mechanic:frozen} Pokémon may use this move, in which case they will thaw.
		/// </summary>	
		/// <remarks>
		/// FLAME-WHEEL,SACRED-FIRE
		/// </remarks>	
		x07E = 126,
		/// <summary>
		/// Power varies randomly from 10 to 150.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Power is selected at random between 10 and 150, with an average of 71:
		/// 
		/// Magnitude | Power | Chance
		/// --------: | ----: | -----:
		///         4 |    10 |     5%
		///         5 |    30 |    10%
		///         6 |    50 |    20%
		///         7 |    70 |    30%
		///         8 |    90 |    20%
		///         9 |   110 |    10%
		///        10 |   150 |     5%
		/// 
		/// This move has double power against Pokémon currently underground due to <seealso cref="Moves.DIG"/>.
		/// </summary>
		/// <remarks>
		/// MAGNITUDE
		/// </remarks>	
		x07F = 127,
		/// <summary>
		/// Allows the trainer to switch out the user and pass effects along to its replacement.<para></para> User [switches out]{mechanic:switches-out}, and the trainer selects a replacement Pokémon from the party.  [Stat]{mechanic:stat} changes, <see cref="Attack.MoveMetaAilments.CONFUSION"/>, and persistent move effects are passed along to the replacement Pokémon.
		/// 
		/// The following move effects are passed:
		/// 
		/// * <seealso cref="Moves.AQUA_RING"/>
		/// * both the user's and target's effect of <seealso cref="Moves.BLOCK"/>, <seealso cref="Moves.MEAN_LOOK"/>, and <seealso cref="Moves.SPIDER_WEB"/>
		/// * the curse effect of <seealso cref="Moves.CURSE"/>
		/// * <seealso cref="Moves.EMBARGO"/>
		/// * <seealso cref="Moves.FOCUS_ENERGY"/> or an activated <seealso cref="Items.LANSAT_BERRY"/>
		/// * <seealso cref="Moves.GASTRO_ACID"/>
		/// * <seealso cref="Moves.INGRAIN"/>
		/// * being sapped by <seealso cref="Moves.LEECH_SEED"/>
		/// * being targeted by <seealso cref="Moves.LOCK_ON"/> or <seealso cref="Moves.MIND_READER"/>
		/// * <seealso cref="Moves.MAGNET_RISE"/>
		/// * <seealso cref="Moves.PERISH_SONG"/>'s counter
		/// * <seealso cref="Moves.POWER_TRICK"/>
		/// * <seealso cref="Moves.SUBSTITUTE"/>; the doll's HP is unchanged
		/// 
		/// The replacement Pokémon does not trigger effects that respond to Pokémon switching in.
		/// </summary>
		/// <remarks>
		/// BATON-PASS
		/// </remarks>	
		x080 = 128,
		/// <summary>
		/// Has double power against, and can hit, Pokémon attempting to switch out.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  If the target attempts to [switch out]{mechanic:switch-out} this turn before the user acts, this move hits the target before it leaves and has double power.
		/// 
		/// This effect can still hit a Pokémon that [switches out]{mechanic:switches-out} when it has a <seealso cref="Moves.SUBSTITUTE"/> up or when an ally has used <seealso cref="Moves.FOLLOW_ME"/>.
		/// </summary>
		/// <remarks>
		/// PURSUIT
		/// </remarks>	
		x081 = 129,
		/// <summary>
		/// Frees the user from binding moves, removes Leech Seed, and blows away Spikes.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Removes <seealso cref="Moves.LEECH_SEED"/> from the user, frees the user from <seealso cref="Moves.BIND"/>, <seealso cref="Moves.CLAMP"/>, <seealso cref="Moves.FIRE_SPIN"/>, <seealso cref="Moves.MAGMA_STORM"/>, <seealso cref="Moves.SAND_TOMB"/>, <seealso cref="Moves.WHIRLPOOL"/>, and <seealso cref="Moves.WRAP"/>, and clears <seealso cref="Moves.SPIKES"/>, <seealso cref="Moves.STEALTH_ROCK"/>, and <seealso cref="Moves.TOXIC_SPIKES"/> from the user's side of the [field]{mechanic:field}.  If this move misses or has [no effect]{mechanic:no-effect}, its effect doesn't activate.
		/// </summary>
		/// <remarks>
		/// RAPID-SPIN
		/// </remarks>	
		x082 = 130,
		/// <summary>
		/// Inflicts 20 points of damage.<para></para> Inflicts exactly 20 damage.
		/// </summary>	
		/// <remarks>
		/// SONIC-BOOM
		/// </remarks>	
		x083 = 131,
		/// <summary>
		/// Heals the user by half its max HP.  Affected by weather.<para></para> Heals the user for half its max [HP]{mechanic:hp}.
		/// 
		/// During <seealso cref="Moves.SUNNY_DAY"/>, the healing is increased to 2/3 max [HP]{mechanic:hp}.
		/// 
		/// During <seealso cref="Moves.HAIL"/>, <seealso cref="Moves.RAIN_DANCE"/>, or <seealso cref="Moves.SANDSTORM"/>, the healing is decreased to 1/4 max [HP]{mechanic:hp}.
		/// </summary>
		/// <remarks>
		/// MORNING-SUN,SYNTHESIS,MOONLIGHT
		/// </remarks>	
		x085 = 133,
		/// <summary>
		/// Power and type depend upon user's IVs.  Power can range from 30 to 70.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Power and type are determined by the user's [IV]{mechanic:iv}s.
		/// 
		/// Power is given by `x * 40 / 63 + 30`.  `x` is obtained by arranging bit 1 from the [IV]{mechanic:iv} for each of [Special Defense]{mechanic:special-defense}, [Special Attack]{mechanic:special-attack}, [Speed]{mechanic:speed}, [Defense]{mechanic:defense}, [Attack]{mechanic:attack}, and [HP]{mechanic:hp} in that order.  (Bit 1 is 1 if the [IV]{mechanic:iv} is of the form `4n + 2` or `4n + 3`.  `x` is then 64 * [Special Defense]{mechanic:special-defense} [IV]{mechanic:iv} bit 1, plus 32 * [Special Attack]{mechanic:special-attack} [IV]{mechanic:iv} bit 1, etc.)
		/// 
		/// Power is always between 30 and 70, inclusive.  Average power is 49.5.
		/// 
		/// Type is given by `y * 15 / 63`, where `y` is similar to `x` above, except constructed from bit 0.  (Bit 0 is 1 if the [IV]{mechanic:iv} is odd.)  The result is looked up in the following table.
		/// 
		/// Value | Type
		/// ----: | --------
		///     0 | <seealso cref="Types.FIGHTING"/>
		///     1 | <seealso cref="Types.FLYING"/>
		///     2 | <seealso cref="Types.POISON"/>
		///     3 | <seealso cref="Types.GROUND"/>
		///     4 | <seealso cref="Types.ROCK"/>
		///     5 | <seealso cref="Types.BUG"/>
		///     6 | <seealso cref="Types.GHOST"/>
		///     7 | <seealso cref="Types.STEEL"/>
		///     8 | <seealso cref="Types.FIRE"/>
		///     9 | <seealso cref="Types.WATER"/>
		///    10 | <seealso cref="Types.GRASS"/>
		///    11 | <seealso cref="Types.ELECTRIC"/>
		///    12 | <seealso cref="Types.PSYCHIC"/>
		///    13 | <seealso cref="Types.ICE"/>
		///    14 | <seealso cref="Types.DRAGON"/>
		///    15 | <seealso cref="Types.DARK"/>
		/// 
		/// This move thus cannot be <seealso cref="Types.NORMAL"/>.  Most other types have an equal 1/16 chance to be selected, given random [IV]{mechanic:iv}s.  However, due to the flooring used here, <seealso cref="Types.BUG"/>, <seealso cref="Types.FIGHTING"/>, and <seealso cref="Types.GRASS"/> appear 5/64 of the time, and <seealso cref="Types.DARK"/> only 1/64 of the time.
		/// </summary>
		/// <remarks>
		/// HIDDEN-POWER
		/// </remarks>	
		x088 = 136,
		/// <summary>
		/// Changes the weather to rain for five turns.<para></para> Changes the weather to rain for five turns, during which <seealso cref="Types.WATER"/> moves inflict 50% extra damage, and <seealso cref="Types.FIRE"/> moves inflict half damage.
		/// 
		/// If the user is holding <seealso cref="Items.DAMP_ROCK"/>, this effect lasts for eight turns.
		/// 
		/// <seealso cref="Moves.THUNDER"/> has 100% accuracy.  If the target has used <seealso cref="Moves.DETECT"/> or <seealso cref="Moves.PROTECT"/>, <seealso cref="Moves.THUNDER"/> has a (100 - accuracy)% chance to break through the protection.
		/// 
		/// <seealso cref="Moves.SOLAR_BEAM"/> has half power.
		/// 
		/// <seealso cref="Moves.MOONLIGHT"/>, <seealso cref="Moves.MORNING_SUN"/>, and <seealso cref="Moves.SYNTHESIS"/> heal only 1/4 of the user's max [HP]{mechanic:hp}.
		/// 
		/// Pokémon with <seealso cref="Abilities.SWIFT_SWIM"/> have doubled original [Speed]{mechanic:speed}.
		/// 
		/// Pokémon with <seealso cref="Abilities.FORECAST"/> become <seealso cref="Types.WATER"/>.
		/// 
		/// Pokémon with <seealso cref="Abilities.DRY_SKIN"/> heal 1/8 max [HP]{mechanic:hp}, Pokémon with <seealso cref="Abilities.HYDRATION"/> are cured of [major status effects]{mechanic:major-status-effects}, and Pokémon with <seealso cref="Abilities.RAIN_DISH"/> heal 1/16 max [HP]{mechanic:hp} at the end of each turn.
		/// </summary>
		/// <remarks>
		/// RAIN-DANCE
		/// </remarks>	
		x089 = 137,
		/// <summary>
		/// Changes the weather to sunny for five turns.<para></para> Changes the weather to sunshine for five turns, during which <seealso cref="Types.FIRE"/> moves inflict 50% extra damage, and <seealso cref="Types.WATER"/> moves inflict half damage.
		/// 
		/// If the user is holding <seealso cref="Items.HEAT_ROCK"/>, this effect lasts for eight turns.
		/// 
		/// Pokémon cannot become [frozen]{mechanic:frozen}.
		/// 
		/// <seealso cref="Moves.THUNDER"/> has 50% accuracy.
		/// 
		/// <seealso cref="Moves.SOLAR_BEAM"/> skips its charge turn.
		/// 
		/// <seealso cref="Moves.MOONLIGHT"/>, <seealso cref="Moves.MORNING_SUN"/>, and <seealso cref="Moves.SYNTHESIS"/> heal 2/3 of the user's max [HP]{mechanic:hp}.
		/// 
		/// Pokémon with <seealso cref="Abilities.CHLOROPHYLL"/> have doubled original [Speed]{mechanic:speed}.
		/// 
		/// Pokémon with <seealso cref="Abilities.FORECAST"/> become <seealso cref="Types.FIRE"/>.
		/// 
		/// Pokémon with <seealso cref="Abilities.LEAF_GUARD"/> are not affected by [major status effects]{mechanic:major-status-effects}.
		/// 
		/// Pokémon with <seealso cref="Abilities.FLOWER_GIFT"/> change form; every Pokémon on their side of the [field]{mechanic:field} have their original [Attack]{mechanic:attack} and [Special Attack]{mechanic:special-attack} increased by 50%.
		/// 
		/// Pokémon with <seealso cref="Abilities.DRY_SKIN"/> lose 1/8 max [HP]{mechanic:hp} at the end of each turn.
		/// 
		/// Pokémon with <seealso cref="Abilities.SOLAR_POWER"/> have their original [Special Attack]{mechanic:special-attack} raised by 50% but lose 1/8 their max [HP]{mechanic:hp} at the end of each turn.
		/// </summary>	
		/// <remarks>
		/// SUNNY-DAY
		/// </remarks>	
		x08A = 138,
		/// <summary>
		/// Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to raise the user's Defense by one stage.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>. Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to raise the user's [Defense]{mechanic:defense} one [stage]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// STEEL-WING
		/// </remarks>	
		x08B = 139,
		/// <summary>
		/// Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to raise the user's Attack by one stage.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>. Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to raise the user's [Attack]{mechanic:attack} one [stage]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// METAL-CLAW,METEOR-MASH
		/// </remarks>	
		x08C = 140,
		/// <summary>
		/// Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to raise all of the user's stats by one stage.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>. Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to raise all of the user's stats one [stage]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// ANCIENT-POWER,SILVER-WIND,OMINOUS-WIND
		/// </remarks>	
		x08D = 141,
		/// <summary>
		/// User pays half its max HP to max out its Attack.<para></para> User pays half its max [HP]{mechanic:hp} to raise its [Attack]{mechanic:attack} to +6 [stages]{mechanic:stage}.  If the user cannot pay the [HP]{mechanic:hp} cost, this move will [fail]{mechanic:fail}.
		/// </summary>	
		/// <remarks>
		/// BELLY-DRUM
		/// </remarks>	
		x08F = 143,
		/// <summary>
		/// Discards the user's stat changes and copies the target's.<para></para> Discards the user's [stat changes]{mechanic:stat-changes} and copies the target's.
		/// 
		/// This move cannot be copied by <seealso cref="Moves.MIRROR_MOVE"/>.
		/// </summary>
		/// <remarks>
		/// PSYCH-UP
		/// </remarks>	
		x090 = 144,
		/// <summary>
		/// Inflicts twice the damage the user received from the last special hit it took.<para></para> Targets the last opposing Pokémon to hit the user with a [special]{mechanic:special} move this turn.  Inflicts twice the damage that move did to the user.  If there is no eligible target, this move will [fail]{mechanic:fail}.  Type immunity applies, but other type effects are ignored.
		/// 
		/// This move cannot be copied by <seealso cref="Moves.MIRROR_MOVE"/>, nor selected by <seealso cref="Moves.ASSIST"/> or <seealso cref="Moves.METRONOME"/>.
		/// </summary>
		/// <remarks>
		/// MIRROR-COAT
		/// </remarks>	
		x091 = 145,
		/// <summary>
		/// Raises the user's Defense by one stage.  User charges for one turn before attacking.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Raises the user's [Defense]{mechanic:defense} by one [stage]{mechanic:stage}.  User then charges for one turn before attacking.
		/// 
		/// This move cannot be selected by <seealso cref="Moves.SLEEP_TALK"/>.
		/// </summary>	
		/// <remarks>
		/// SKULL-BASH
		/// </remarks>	
		x092 = 146,
		/// <summary>
		/// Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to make the target flinch.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to make each target [flinch]{mechanic:flinch}.
		/// 
		/// If the target is under the effect of <seealso cref="Moves.BOUNCE"/>, <seealso cref="Moves.FLY"/>, or <seealso cref="Moves.SKY_DROP"/>, this move will hit with double power.
		/// </summary>
		/// <remarks>
		/// TWISTER
		/// </remarks>	
		x093 = 147,
		/// <summary>
		/// Inflicts regular damage and can hit Dig users.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.
		/// 
		/// If the target is in the first turn of <seealso cref="Moves.DIG"/>, this move will hit with double power.
		/// </summary>
		/// <remarks>
		/// EARTHQUAKE
		/// </remarks>	
		x094 = 148,
		/// <summary>
		/// Hits the target two turns later.<para></para> Inflicts [typeless]{mechanic:typeless} <seealso cref="Attack.MetaCategory.DAMAGE"/> at the end of the third turn, starting with this one.  This move cannot score a [critical hit]{mechanic:critical-hit}.  If the target [switches out]{mechanic:switches-out}, its replacement will be hit instead.  Damage is calculated at the time this move is used; [stat changes]{mechanic:stat-changes} and [switching out]{mechanic:switching-out} during the delay won't change the damage inflicted.  No move with this effect can be used against the same target again until after the end of the third turn.
		/// 
		/// This effect breaks through <seealso cref="Abilities.WONDER_GUARD"/>.
		/// 
		/// If the target is protected by <seealso cref="Moves.PROTECT"/> or <seealso cref="Moves.DETECT"/> on the turn this move is used, this move will [fail]{mechanic:fail}.  However, the damage on the third turn will break through protection.
		/// 
		/// The damage is applied at the end of the turn, so it ignores <seealso cref="Moves.ENDURE"/> and <seealso cref="Items.FOCUS_SASH"/>.
		/// 
		/// This move cannot be copied by <seealso cref="Moves.MIRROR_MOVE"/>.
		/// </summary>
		/// <remarks>
		/// FUTURE-SIGHT,DOOM-DESIRE
		/// </remarks>	
		x095 = 149,
		/// <summary>
		/// Inflicts regular damage and can hit Pokémon in the air.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.
		/// 
		/// If the target is under the effect of <seealso cref="Moves.BOUNCE"/>, <seealso cref="Moves.FLY"/>, or <seealso cref="Moves.SKY_DROP"/>, this move will hit with double power.
		/// </summary>
		/// <remarks>
		/// GUST
		/// </remarks>	
		x096 = 150,
		/// <summary>
		/// Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to make the target flinch.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to make the target [flinch]{mechanic:flinch}.
		/// 
		/// Power is doubled against Pokémon that have used <seealso cref="Moves.MINIMIZE"/> since entering the [field]{mechanic:field}.
		/// </summary>
		/// <remarks>
		/// STOMP,STEAMROLLER
		/// </remarks>	
		x097 = 151,
		/// <summary>
		/// Requires a turn to charge before attacking.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  User charges for one turn before attacking.
		/// 
		/// During <seealso cref="Moves.SUNNY_DAY"/>, the charge turn is skipped.
		/// 
		/// During <seealso cref="Moves.HAIL"/>, <seealso cref="Moves.RAIN_DANCE"/>, or <seealso cref="Moves.SANDSTORM"/>, power is halved.
		/// 
		/// This move cannot be selected by <seealso cref="Moves.SLEEP_TALK"/>.
		/// </summary>
		/// <remarks>
		/// SOLAR-BEAM,SOLAR-BLADE
		/// </remarks>	
		x098 = 152,
		/// <summary>
		/// Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to paralyze the target.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to [paralyze]{mechanic:paralyze} the target.
		/// 
		/// During <seealso cref="Moves.RAIN_DANCE"/>, this move has 100% accuracy.  It also has a (100 - accuracy)% chance to break through the protection of <seealso cref="Moves.PROTECT"/> and <seealso cref="Moves.DETECT"/>.
		/// 
		/// During <seealso cref="Moves.SUNNY_DAY"/>, this move has 50% accuracy.
		/// </summary>	
		/// <remarks>
		/// THUNDER
		/// </remarks>	
		x099 = 153,
		/// <summary>
		/// Immediately ends wild battles.  No effect otherwise.<para></para> Does nothing.  Wild battles end immediately.
		/// </summary>	
		/// <remarks>
		/// TELEPORT
		/// </remarks>	
		x09A = 154,
		/// <summary>
		/// Hits once for every conscious Pokémon the trainer has.<para></para> Inflicts [typeless]{mechanic:typeless} <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Every Pokémon in the user's party, excepting those that have fainted or have a [major status effect]{mechanic:major-status-effect}, attacks the target.  Calculated stats are ignored; the base stats for the target and assorted attackers are used instead.  The random factor in the damage formula is not used.  <seealso cref="Types.DARK"/> Pokémon still get [STAB]{mechanic:stab}.
		/// 
		/// This effect breaks through <seealso cref="Abilities.WONDER_GUARD"/>.
		/// </summary>	
		/// <remarks>
		/// BEAT-UP
		/// </remarks>	
		x09B = 155,
		/// <summary>
		/// User flies high into the air, dodging all attacks, and hits next turn.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  User flies high into the air for one turn, becoming immune to attack, and hits on the second turn.
		/// 
		/// During the immune turn, <seealso cref="Moves.GUST"/>, <seealso cref="Moves.HURRICANE"/>, <seealso cref="Moves.SKY_UPPERCUT"/>, <seealso cref="Moves.SMACK_DOWN"/>, <seealso cref="Moves.THUNDER"/>, <seealso cref="Moves.TWISTER"/>, and <seealso cref="Moves.WHIRLWIND"/> still hit the user normally.  <seealso cref="Moves.GUST"/> and <seealso cref="Moves.TWISTER"/> also have double power against the user.
		/// 
		/// The damage from <seealso cref="Moves.HAIL"/> and <seealso cref="Moves.SANDSTORM"/> still applies during the immune turn.
		/// 
		/// The user may be hit during its immune turn if under the effect of <seealso cref="Moves.LOCK_ON"/>, <seealso cref="Moves.MIND_READER"/>, or <seealso cref="Abilities.NO_GUARD"/>.
		/// 
		/// This move cannot be used while <seealso cref="Moves.GRAVITY"/> is in effect.
		/// 
		/// This move cannot be selected by <seealso cref="Moves.SLEEP_TALK"/>.
		/// </summary>
		/// <remarks>
		/// FLY
		/// </remarks>	
		x09C = 156,
		/// <summary>
		/// Raises user's Defense by one stage.<para></para> Raises user's [Defense]{mechanic:defense} by one [stage]{mechanic:stage}.
		/// 
		/// After this move is used, the power of <seealso cref="Moves.ICE_BALL"/> and <seealso cref="Moves.ROLLOUT"/> are doubled until the user leaves the [field]{mechanic:field}.
		/// </summary>
		/// <remarks>
		/// DEFENSE-CURL
		/// </remarks>	
		x09D = 157,
		/// <summary>
		/// Can only be used as the first move after the user enters battle.  Causes the target to flinch.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Causes the target to <seealso cref="Mechanics.FLINCH"/>.  Can only be used on the user's first turn after entering the [field]{mechanic:field}.
		/// </summary>
		/// <remarks>
		/// FAKE-OUT
		/// </remarks>	
		x09F = 159,
		/// <summary>
		/// Forced to use this move for several turns.  Pokémon cannot fall asleep in that time.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  User is forced to use this move for 2–5 turns, selected at random.  All Pokémon on the [field]{mechanic:field} wake up, and none can fall to <see cref="Attack.MoveMetaAilments.SLEEP"/> until the lock-in ends.
		/// 
		/// Pokémon cannot use <seealso cref="Moves.REST"/> during this effect.
		/// 
		/// This move cannot be selected by <seealso cref="Moves.SLEEP_TALK"/>.
		/// </summary>
		/// <remarks>
		/// UPROAR
		/// </remarks>	
		x0A0 = 160,
		/// <summary>
		/// Stores energy up to three times for use with Spit Up and Swallow.<para></para> Raises the user's [Defense]{mechanic:defense} and [Special Defense]{mechanic:special-defense} by one [stage]{mechanic:stage} each.  Stores energy for use with <seealso cref="Moves.SPIT_UP"/> and <seealso cref="Moves.SWALLOW"/>.  Up to three levels of energy can be stored, and all are lost if the user leaves the [field]{mechanic:field}.  Energy is still stored even if the stat boosts cannot be applied.
		/// 
		/// If the user uses <seealso cref="Moves.BATON_PASS"/>, the stat boosts are passed as normal, but the stored energy is not.
		/// </summary>	
		/// <remarks>
		/// STOCKPILE
		/// </remarks>	
		x0A1 = 161,
		/// <summary>
		/// Power is 100 times the amount of energy Stockpiled.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Power is equal to 100 times the amount of energy stored by <seealso cref="Moves.STOCKPILE"/>.  Ignores the random factor in the damage formula.  Stored energy is consumed, and the user's [Defense]{mechanic:defense} and [Special Defense]{mechanic:special-defense} are reset to what they would be if <seealso cref="Moves.STOCKPILE"/> had not been used.  If the user has no energy stored, this move will [fail]{mechanic:fail}.
		/// 
		/// This move cannot be copied by <seealso cref="Moves.MIRROR_MOVE"/>.
		/// </summary>
		/// <remarks>
		/// SPIT-UP
		/// </remarks>	
		x0A2 = 162,
		/// <summary>
		/// Recovers 1/4 HP after one Stockpile, 1/2 HP after two Stockpiles, or full HP after three Stockpiles.<para></para> Heals the user depending on the amount of energy stored by <seealso cref="Moves.STOCKPILE"/>: 1/4 its max [HP]{mechanic:hp} after one use, 1/2 its max [HP]{mechanic:hp} after two uses, or fully after three uses.  Stored energy is consumed, and the user's [Defense]{mechanic:defense} and [Special Defense]{mechanic:special-defense} are reset to what they would be if <seealso cref="Moves.STOCKPILE"/> had not been used.  If the user has no energy stored, this move will [fail]{mechanic:fail}.
		/// </summary>	
		/// <remarks>
		/// SWALLOW
		/// </remarks>	
		x0A3 = 163,
		/// <summary>
		/// Changes the weather to a hailstorm for five turns.<para></para> Changes the weather to hail for five turns, during which non-<seealso cref="Types.ICE"/> Pokémon are damaged for 1/16 their max [HP]{mechanic:hp} at the end of every turn.
		/// 
		/// If the user is holding <seealso cref="Items.ICY_ROCK"/>, this effect lasts for eight turns.
		/// 
		/// <seealso cref="Moves.BLIZZARD"/> has 100% accuracy.  If the target has used <seealso cref="Moves.DETECT"/> or <seealso cref="Moves.PROTECT"/>, <seealso cref="Moves.BLIZZARD"/> has a (100 - accuracy)% chance to break through the protection.
		/// 
		/// <seealso cref="Moves.MOONLIGHT"/>, <seealso cref="Moves.MORNING_SUN"/>, and <seealso cref="Moves.SYNTHESIS"/> heal only 1/4 of the user's max [HP]{mechanic:hp}.
		/// 
		/// Pokémon with <seealso cref="Abilities.SNOW_CLOAK"/> are exempt from this effect's damage.
		/// </summary>
		/// <remarks>
		/// HAIL
		/// </remarks>	
		x0A5 = 165,
		/// <summary>
		/// Prevents the target from using the same move twice in a row.<para></para> Prevents the target from attempting to use the same move twice in a row.  When the target leaves the [field]{mechanic:field}, this effect ends.
		/// 
		/// If the target is forced to attempt a repeated move due to <seealso cref="Items.CHOICE_BAND"/>, <seealso cref="Items.CHOICE_SCARF"/>, <seealso cref="Items.CHOICE_SPECS"/>, <seealso cref="Moves.DISABLE"/>, <seealso cref="Moves.ENCORE"/>, <seealso cref="Moves.TAUNT"/>, only having [PP]{mechanic:pp} remaining for one move, or any other effect, the target will use <seealso cref="Moves.STRUGGLE"/> instead.  The target is then free to use the forced move next turn, as it didn't use that move this turn.
		/// </summary>
		/// <remarks>
		/// TORMENT
		/// </remarks>	
		x0A6 = 166,
		/// <summary>
		/// Raises the target's Special Attack by one stage and confuses the target.<para></para> Raises the target's [Special Attack]{mechanic:special-attack} by one [stage]{mechanic:stage}, then [confuses]{mechanic:confuses} it.
		/// </summary>	
		/// <remarks>
		/// FLATTER
		/// </remarks>	
		x0A7 = 167,
		/// <summary>
		/// Burns the target.<para></para> [Burns]{mechanic:burns} the target.
		/// </summary>	
		/// <remarks>
		/// WILL-O-WISP
		/// </remarks>	
		x0A8 = 168,
		/// <summary>
		/// Lowers the target's Attack and Special Attack by two stages.  User faints.<para></para> Lowers the target's [Attack]{mechanic:attack} and [Special Attack]{mechanic:special-attack} by two [stages]{mechanic:stage}.  User faints.
		/// </summary>	
		/// <remarks>
		/// MEMENTO
		/// </remarks>	
		x0A9 = 169,
		/// <summary>
		/// Power doubles if user is burned, paralyzed, or poisoned.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  If the user is [burned]{mechanic:burned}, [paralyzed]{mechanic:paralyzed}, or [poisoned]{mechanic:poisoned}, this move has double power.
		/// </summary>	
		/// <remarks>
		/// FACADE
		/// </remarks>	
		x0AA = 170,
		/// <summary>
		/// If the user takes damage before attacking, the attack is canceled.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  If the user takes damage this turn before hitting, this move will [fail]{mechanic:fail}.
		/// 
		/// This move cannot be copied by <seealso cref="Moves.MIRROR_MOVE"/>, nor selected by <seealso cref="Moves.ASSIST"/>, <seealso cref="Moves.METRONOME"/>, or <seealso cref="Moves.SLEEP_TALK"/>.
		/// </summary>
		/// <remarks>
		/// FOCUS-PUNCH
		/// </remarks>	
		x0AB = 171,
		/// <summary>
		/// If the target is paralyzed, inflicts double damage and cures the paralysis.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  If the target is [paralyzed]{mechanic:paralyzed}, this move has double power, and the target is cured of its <see cref="Attack.MoveMetaAilments.PARALYSIS"/>.
		/// </summary>	
		/// <remarks>
		/// SMELLING-SALTS
		/// </remarks>	
		x0AC = 172,
		/// <summary>
		/// Redirects the target's single-target effects to the user for this turn.<para></para> Until the end of this turn, any moves that opposing Pokémon target solely at the user's ally will instead target the user.  If both Pokémon on the same side of the [field]{mechanic:field} use this move on the same turn, the Pokémon that uses it last will become the target.
		/// 
		/// This effect takes priority over <seealso cref="Abilities.LIGHTNING_ROD"/> and <seealso cref="Abilities.STORM_DRAIN"/>.
		/// 
		/// If the user's ally [switches out]{mechanic:switches-out}, opposing Pokémon may still hit it with <seealso cref="Moves.PURSUIT"/>.
		/// 
		/// This move cannot be selected by <seealso cref="Moves.ASSIST"/> or <seealso cref="Moves.METRONOME"/>.
		/// </summary>
		/// <remarks>
		/// FOLLOW-ME,RAGE-POWDER
		/// </remarks>	
		x0AD = 173,
		/// <summary>
		/// Uses a move which depends upon the terrain.<para></para> Uses another move chosen according to the terrain.
		/// 
		/// Terrain                   | Selected move
		/// ------------------------- | ------------------
		/// Building                  | <seealso cref="Moves.TRI_ATTACK"/>
		/// Cave                      | <seealso cref="Moves.ROCK_SLIDE"/>
		/// Deep water                | <seealso cref="Moves.HYDRO_PUMP"/>
		/// Desert                    | <seealso cref="Moves.EARTHQUAKE"/>
		/// Grass                     | <seealso cref="Moves.SEED_BOMB"/>
		/// Mountain                  | <seealso cref="Moves.ROCK_SLIDE"/>
		/// Road                      | <seealso cref="Moves.EARTHQUAKE"/>
		/// Shallow water             | <seealso cref="Moves.HYDRO_PUMP"/>
		/// Snow                      | <seealso cref="Moves.BLIZZARD"/>
		/// Tall grass                | <seealso cref="Moves.SEED_BOMB"/>
		/// <seealso cref="Moves.ELECTRIC_TERRAIN"/> | <seealso cref="Moves.THUNDERBOLT"/>
		/// <seealso cref="Moves.GRASSY_TERRAIN"/>   | <seealso cref="Moves.ENERGY_BALL"/>
		/// <seealso cref="Moves.MISTY_TERRAIN"/>    | <seealso cref="Moves.MOONBLAST"/>
		/// 
		/// In Pokémon Battle Revolution:
		/// 
		/// Terrain        | Selected move
		/// -------------- | ------------------
		/// Courtyard      | <seealso cref="Moves.TRI_ATTACK"/>
		/// Crystal        | <seealso cref="Moves.ROCK_SLIDE"/>
		/// Gateway        | <seealso cref="Moves.HYDRO_PUMP"/>
		/// Magma          | <seealso cref="Moves.ROCK_SLIDE"/>
		/// Main Street    | <seealso cref="Moves.TRI_ATTACK"/>
		/// Neon           | <seealso cref="Moves.TRI_ATTACK"/>
		/// Stargazer      | <seealso cref="Moves.ROCK_SLIDE"/>
		/// Sunny Park     | <seealso cref="Moves.SEED_BOMB"/>
		/// Sunset         | <seealso cref="Moves.EARTHQUAKE"/>
		/// Waterfall      | <seealso cref="Moves.SEED_BOMB"/>
		/// 
		/// This move cannot be copied by <seealso cref="Moves.MIRROR_MOVE"/>.
		/// </summary>
		/// <remarks>
		/// NATURE-POWER
		/// </remarks>	
		x0AE = 174,
		/// <summary>
		/// Raises the user's Special Defense by one stage.  User's Electric moves have doubled power next turn.<para></para> Raises the user's [Special Defense]{mechanic:special-defense} by one [stage]{mechanic:stage}.  If the user uses an <seealso cref="Types.ELECTRIC"/> move next turn, its power will be doubled.
		/// </summary>	
		/// <remarks>
		/// CHARGE
		/// </remarks>	
		x0AF = 175,
		/// <summary>
		/// For the next few turns, the target can only use damaging moves.<para></para> Target is forced to only use damaging moves for the next 3–5 turns, selected at random.  Moves that select other moves not known in advance do not count as damaging.
		/// 
		/// <seealso cref="Moves.ASSIST"/>, <seealso cref="Moves.COPYCAT"/>, <seealso cref="Moves.ME_FIRST"/>, <seealso cref="Moves.METRONOME"/>, <seealso cref="Moves.MIRROR_MOVE"/>, and <seealso cref="Moves.SLEEP_TALK"/> do not directly inflict damage and thus may not be used.
		/// 
		/// <seealso cref="Moves.BIDE"/>, <seealso cref="Moves.COUNTER"/>, <seealso cref="Moves.ENDEAVOR"/>, <seealso cref="Moves.METAL_BURST"/>, and <seealso cref="Moves.MIRROR_COAT"/> are allowed.
		/// </summary>
		/// <remarks>
		/// TAUNT
		/// </remarks>	
		x0B0 = 176,
		/// <summary>
		/// Ally's next move inflicts half more damage.<para></para> Boosts the power of the target's moves by 50% until the end of this turn.
		/// 
		/// This move cannot be copied by <seealso cref="Moves.MIRROR_MOVE"/>, nor selected by <seealso cref="Moves.ASSIST"/> or <seealso cref="Moves.METRONOME"/>.
		/// </summary>
		/// <remarks>
		/// HELPING-HAND
		/// </remarks>	
		x0B1 = 177,
		/// <summary>
		/// User and target swap items.<para></para> User and target permanently swap [held item]{mechanic:held-item}s.  Works even if one of the Pokémon isn't holding anything.  If either Pokémon is holding mail, this move will [fail]{mechanic:fail}.
		/// 
		/// If either Pokémon has <seealso cref="Abilities.MULTITYPE"/> or <seealso cref="Abilities.STICKY_HOLD"/>, this move will [fail]{mechanic:fail}.
		/// 
		/// If this move results in a Pokémon obtaining <seealso cref="Items.CHOICE_BAND"/>, <seealso cref="Items.CHOICE_SCARF"/>, or <seealso cref="Items.CHOICE_SPECS"/>, and that Pokémon was the latter of the pair to move this turn, then the move it used this turn becomes its chosen forced move.  This applies even if both Pokémon had a choice item before this move was used.  If the first of the two Pokémon gains a choice item, it may select whatever choice move it wishes next turn.
		/// 
		/// Neither the user nor the target can recover its item with <seealso cref="Moves.RECYCLE"/>.
		/// 
		/// This move cannot be selected by <seealso cref="Moves.ASSIST"/> or <seealso cref="Moves.METRONOME"/>.
		/// </summary>
		/// <remarks>
		/// TRICK,SWITCHEROO
		/// </remarks>	
		x0B2 = 178,
		/// <summary>
		/// Copies the target's ability.<para></para> User's ability is replaced with the target's until the user leaves the [field]{mechanic:field}.  Ignores [accuracy]{mechanic:accuracy} and [evasion]{mechanic:evasion} modifiers.
		/// 
		/// If the target has <seealso cref="Abilities.FLOWER_GIFT"/>, <seealso cref="Abilities.FORECAST"/>, <seealso cref="Abilities.ILLUSION"/>, <seealso cref="Abilities.IMPOSTER"/>, <seealso cref="Abilities.MULTITYPE"/>, <seealso cref="Abilities.STANCE_CHANGE"/>, <seealso cref="Abilities.TRACE"/>, <seealso cref="Abilities.WONDER_GUARD"/>, or <seealso cref="Abilities.ZEN_MODE"/>, this move will [fail]{mechanic:fail}.
		/// 
		/// This move cannot be copied by <seealso cref="Moves.MIRROR_MOVE"/>.
		/// </summary>
		/// <remarks>
		/// ROLE-PLAY
		/// </remarks>	
		x0B3 = 179,
		/// <summary>
		/// User will recover half its max HP at the end of the next turn.<para></para> At the end of the next turn, user will be healed for half its max [HP]{mechanic:hp}.  If the user is [switched out]{mechanic:switched-out}, its replacement will be healed instead for half of the user's max HP.  If the user [faint]{mechanic:faint}s or is forcefully switched by <seealso cref="Moves.ROAR"/> or <seealso cref="Moves.WHIRLWIND"/>, this effect will not activate.
		/// </summary>
		/// <remarks>
		/// WISH
		/// </remarks>	
		x0B4 = 180,
		/// <summary>
		/// Randomly selects and uses one of the trainer's other Pokémon's moves.<para></para> Uses a move from another Pokémon in the user's party, both selected at random.  Moves from fainted Pokémon can be used.  If there are no eligible Pokémon or moves, this move will [fail]{mechanic:fail}.
		/// 
		/// This move will not select <seealso cref="Moves.ASSIST"/>, <seealso cref="Moves.CHATTER"/>, <seealso cref="Moves.CIRCLE_THROW"/>, <seealso cref="Moves.COPYCAT"/>, <seealso cref="Moves.COUNTER"/>, <seealso cref="Moves.COVET"/>, <seealso cref="Moves.DESTINY_BOND"/>, <seealso cref="Moves.DETECT"/>, <seealso cref="Moves.DIG"/>, <seealso cref="Moves.DIVE"/>, <seealso cref="Moves.DRAGON_TAIL"/>, <seealso cref="Moves.ENDURE"/>, <seealso cref="Moves.FEINT"/>, <seealso cref="Moves.FLY"/> <seealso cref="Moves.FOCUS_PUNCH"/>, <seealso cref="Moves.FOLLOW_ME"/>, <seealso cref="Moves.HELPING_HAND"/>, <seealso cref="Moves.ME_FIRST"/>, <seealso cref="Moves.METRONOME"/>, <seealso cref="Moves.MIMIC"/>, <seealso cref="Moves.MIRROR_COAT"/>, <seealso cref="Moves.MIRROR_MOVE"/>, <seealso cref="Moves.PHANTOM_FORCE"/> <seealso cref="Moves.PROTECT"/>, <seealso cref="Moves.QUICK_GUARD"/>, <seealso cref="Moves.ROAR"/> <seealso cref="Moves.SHADOW_FORCE"/>, <seealso cref="Moves.SKETCH"/>, <seealso cref="Moves.SLEEP_TALK"/>, <seealso cref="Moves.SNATCH"/>, <seealso cref="Moves.STRUGGLE"/>, <seealso cref="Moves.SWITCHEROO"/>, <seealso cref="Moves.THIEF"/>, <seealso cref="Moves.TRICK"/>, <seealso cref="Moves.WHIRLWIND"/>, or <seealso cref="Moves.WIDE_GUARD"/>.
		/// 
		/// This move cannot be copied by <seealso cref="Moves.MIRROR_MOVE"/>, nor selected by <seealso cref="Moves.METRONOME"/> or <seealso cref="Moves.SLEEP_TALK"/>.
		/// </summary>
		/// <remarks>
		/// ASSIST
		/// </remarks>	
		x0B5 = 181,
		/// <summary>
		/// Prevents the user from leaving battle.  User regains 1/16 of its max HP every turn.<para></para> Prevents the user from [switching out]{mechanic:switching-out}.  User regains 1/16 of its max [HP]{mechanic:hp} at the end of every turn.  If the user was immune to <seealso cref="Types.GROUND"/> attacks, it will now take normal damage from them.
		/// 
		/// <seealso cref="Moves.ROAR"/> and <seealso cref="Moves.WHIRLWIND"/> will not affect the user.  The user cannot use <seealso cref="Moves.MAGNET_RISE"/>.
		/// 
		/// The user may still use <seealso cref="Moves.U_TURN"/> to leave the [field]{mechanic:field}.
		/// 
		/// This effect can be passed with <seealso cref="Moves.BATON_PASS"/>.
		/// </summary>
		/// <remarks>
		/// INGRAIN
		/// </remarks>	
		x0B6 = 182,
		/// <summary>
		/// Lowers the user's Attack and Defense by one stage after inflicting damage.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>, then lowers the user's [Attack]{mechanic:attack} and [Defense]{mechanic:defense} by one [stage]{mechanic:stage} each.
		/// </summary>	
		/// <remarks>
		/// SUPERPOWER
		/// </remarks>	
		x0B7 = 183,
		/// <summary>
		/// Reflects back the first effect move used on the user this turn.<para></para> The first non-damaging move targeting the user this turn that inflicts [major status effect]{mechanic:major-status-effect}s, [stat change]{mechanic:stat-change}s, or [trap]{mechanic:trap}ping effects will be reflected at its user.
		/// 
		/// <seealso cref="Moves.DEFOG"/>, <seealso cref="Moves.MEMENTO"/>, and <seealso cref="Moves.TEETER_DANCE"/> are not reflected.
		/// 
		/// <seealso cref="Moves.ATTRACT"/>, <seealso cref="Moves.FLATTER"/>, <seealso cref="Moves.GASTRO_ACID"/>, <seealso cref="Moves.LEECH_SEED"/>, <seealso cref="Moves.SWAGGER"/>, <seealso cref="Moves.WORRY_SEED"/>, and <seealso cref="Moves.YAWN"/> are reflected.
		/// 
		/// This move cannot be copied by <seealso cref="Moves.MIRROR_MOVE"/>.
		/// </summary>
		/// <remarks>
		/// MAGIC-COAT
		/// </remarks>	
		x0B8 = 184,
		/// <summary>
		/// User recovers the item it last used up.<para></para> User recovers the last item consumed by the user or a Pokémon in its position on the [field]{mechanic:field}.  The item must be used again before it can be recovered by this move again.  If the user is holding an item, this move [fail]{mechanic:fail}s.
		/// 
		/// Items taken or given away by <seealso cref="Moves.COVET"/>, <seealso cref="Moves.KNOCK_OFF"/>, <seealso cref="Moves.SWITCHEROO"/>, <seealso cref="Moves.THIEF"/>, or <seealso cref="Moves.TRICK"/> may not be recovered.
		/// </summary>
		/// <remarks>
		/// RECYCLE
		/// </remarks>	
		x0B9 = 185,
		/// <summary>
		/// Inflicts double damage if the user takes damage before attacking this turn.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  If the target damaged the user this turn and was the last to do so, this move has double power.
		/// 
		/// <seealso cref="Moves.PAIN_SPLIT"/> does not count as damaging the user.
		/// </summary>
		/// <remarks>
		/// REVENGE,AVALANCHE
		/// </remarks>	
		x0BA = 186,
		/// <summary>
		/// Destroys Reflect and Light Screen.<para></para> Destroys any <seealso cref="Moves.LIGHT_SCREEN"/> or <seealso cref="Moves.REFLECT"/> on the target's side of the [field]{mechanic:field}, then inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.
		/// </summary>
		/// <remarks>
		/// BRICK-BREAK,PSYCHIC-FANGS
		/// </remarks>	
		x0BB = 187,
		/// <summary>
		/// Target sleeps at the end of the next turn.<para></para> Puts the target to <see cref="Attack.MoveMetaAilments.SLEEP"/> at the end of the next turn.  Ignores [accuracy]{mechanic:accuracy} and [evasion]{mechanic:evasion} modifiers.  If the target leaves the [field]{mechanic:field}, this effect is canceled.  If the target has a status effect when this move is used, this move will [fail]{mechanic:fail}.
		/// 
		/// If the target is protected by <seealso cref="Moves.SAFEGUARD"/> when this move is used, this move will [fail]{mechanic:fail}.
		/// 
		/// <seealso cref="Abilities.INSOMNIA"/> and <seealso cref="Abilities.VITAL_SPIRIT"/> prevent the <see cref="Attack.MoveMetaAilments.SLEEP"/> if the target has either at the end of the next turn, but will not cause this move to [fail]{mechanic:fail} on use.
		/// </summary>
		/// <remarks>
		/// YAWN
		/// </remarks>	
		x0BC = 188,
		/// <summary>
		/// Target drops its held item.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Target loses its [held item]{mechanic:held-item}.
		/// 
		/// Neither the user nor the target can recover its item with <seealso cref="Moves.RECYCLE"/>.
		/// 
		/// If the target has <seealso cref="Abilities.MULTITYPE"/> or <seealso cref="Abilities.STICKY_HOLD"/>, it will take damage but not lose its item.
		/// </summary>
		/// <remarks>
		/// KNOCK-OFF
		/// </remarks>	
		x0BD = 189,
		/// <summary>
		/// Lowers the target's HP to equal the user's.<para></para> Inflicts exactly enough damage to lower the target's [HP]{mechanic:hp} to equal the user's.  If the target's HP is not higher than the user's, this move has no effect.  Type immunity applies, but other type effects are ignored.  This effect counts as damage for moves that respond to damage.
		/// </summary>	
		/// <remarks>
		/// ENDEAVOR
		/// </remarks>	
		x0BE = 190,
		/// <summary>
		/// Inflicts more damage when the user has more HP remaining, with a maximum of 150 power.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Power increases with the user's remaining [HP]{mechanic:hp} and is given by `150 * HP / max HP`, to a maximum of 150 when the user has full HP.
		/// </summary>	
		/// <remarks>
		/// ERUPTION,WATER-SPOUT
		/// </remarks>	
		x0BF = 191,
		/// <summary>
		/// User and target swap abilities.<para></para> User and target switch abilities.  Ignores [accuracy]{mechanic:accuracy} and [evasion]{mechanic:evasion} modifiers.
		/// 
		/// If either Pokémon has <seealso cref="Abilities.MULTITYPE"/> or <seealso cref="Abilities.WONDER_GUARD"/>, this move will [fail]{mechanic:fail}.
		/// </summary>
		/// <remarks>
		/// SKILL-SWAP
		/// </remarks>	
		x0C0 = 192,
		/// <summary>
		/// Prevents the target from using any moves that the user also knows.<para></para> Prevents any Pokémon on the opposing side of the [field]{mechanic:field} from using any move the user knows until the user leaves the [field]{mechanic:field}.  This effect is live; if the user obtains new moves while on the [field]{mechanic:field}, these moves become restricted.  If no opposing Pokémon knows any of the user's moves when this move is used, this move will [fail]{mechanic:fail}.
		/// </summary>	
		/// <remarks>
		/// IMPRISON
		/// </remarks>	
		x0C1 = 193,
		/// <summary>
		/// Cleanses the user of a burn, paralysis, or poison.<para></para> Removes a <see cref="Attack.MoveMetaAilments.BURN"/>, <see cref="Attack.MoveMetaAilments.PARALYSIS"/>, or <see cref="Attack.MoveMetaAilments.POISON"/> from the user.
		/// </summary>	
		/// <remarks>
		/// REFRESH
		/// </remarks>	
		x0C2 = 194,
		/// <summary>
		/// If the user faints this turn, the PP of the move that fainted it drops to 0.<para></para> If the user [faint]{mechanic:faint}s before it next acts, the move that fainted it will have its [PP]{mechanic:pp} dropped to 0.  End-of-turn damage does not trigger this effect.
		/// </summary>	
		/// <remarks>
		/// GRUDGE
		/// </remarks>	
		x0C3 = 195,
		/// <summary>
		/// Steals the target's move, if it's self-targeted.<para></para> The next time a Pokémon uses a beneficial move on itself or itself and its ally this turn, the user of this move will steal the move and use it itself.  Moves which may be stolen by this move are identified by the "snatchable" flag.
		/// 
		/// If two Pokémon use this move on the same turn, the faster Pokémon will steal the first beneficial move, and the slower Pokémon will then steal it again—thus, only the slowest Pokémon using this move ultimately gains a stolen move's effect.
		/// 
		/// If the user steals <seealso cref="Moves.PSYCH_UP"/>, it will target the Pokémon that used <seealso cref="Moves.PSYCH_UP"/>.  If the user was the original target of <seealso cref="Moves.PSYCH_UP"/>, and the Pokémon that originally used it's affected by <seealso cref="Abilities.PRESSURE"/>, it will only lose 1 [PP]{mechanic:pp}.
		/// 
		/// This move cannot be copied by <seealso cref="Moves.MIRROR_MOVE"/>, nor selected by <seealso cref="Moves.ASSIST"/> or <seealso cref="Moves.METRONOME"/>.
		/// </summary>
		/// <remarks>
		/// SNATCH
		/// </remarks>	
		x0C4 = 196,
		/// <summary>
		/// Inflicts more damage to heavier targets, with a maximum of 120 power.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Power increases with the target's weight in kilograms, to a maximum of 120.
		/// 
		/// Target's weight | Power
		/// --------------- | ----:
		/// Up to 10kg      |    20
		/// Up to 25kg      |    40
		/// Up to 50kg      |    60
		/// Up to 100kg     |    80
		/// Up to 200kg     |   100
		/// Above 200kg     |   120
		/// </summary>
		/// <remarks>
		/// LOW-KICK,GRASS-KNOT
		/// </remarks>	
		x0C5 = 197,
		/// <summary>
		/// Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to inflict a status effect which depends upon the terrain.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to inflict an effect chosen according to the terrain.
		/// 
		/// Terrain        | Effect
		/// -------------- | -------------------------------------------------------------
		/// Building       | [Paralyze]{mechanic:paralyze}s target
		/// Cave           | Makes target [flinch]{mechanic:flinch}
		/// Deep water     | Lowers target's [Attack]{mechanic:attack} by one [stage]{mechanic:stage}
		/// Desert         | Lowers target's [accuracy]{mechanic:accuracy} by one [stage]{mechanic:stage}
		/// Grass          | Puts target to <see cref="Attack.MoveMetaAilments.SLEEP"/>
		/// Mountain       | Makes target [flinch]{mechanic:flinch}
		/// Road           | Lowers target's [accuracy]{mechanic:accuracy} by one [stage]{mechanic:stage}
		/// Shallow water  | Lowers target's [Attack]{mechanic:attack} by one [stage]{mechanic:stage}
		/// Snow           | [Freezes]{mechanic:freezes} target
		/// Tall grass     | Puts target to <see cref="Attack.MoveMetaAilments.SLEEP"/>
		/// 
		/// In Pokémon Battle Revolution:
		/// 
		/// Terrain        | Effect
		/// -------------- | -------------------------------------------------------------
		/// Courtyard      | [Paralyze]{mechanic:paralyze}s target
		/// Crystal        | Makes target [flinch]{mechanic:flinch}
		/// Gateway        | Lowers target's [Attack]{mechanic:attack} by one [stage]{mechanic:stage}
		/// Magma          | Makes target [flinch]{mechanic:flinch}
		/// Main Street    | [Paralyze]{mechanic:paralyze}s target
		/// Neon           | [Paralyze]{mechanic:paralyze}s target
		/// Stargazer      | Makes target [flinch]{mechanic:flinch}
		/// Sunny Park     | Puts target to <see cref="Attack.MoveMetaAilments.SLEEP"/>
		/// Sunset         | Lowers target's [accuracy]{mechanic:accuracy} by one [stage]{mechanic:stage}
		/// Waterfall      | Puts target to <see cref="Attack.MoveMetaAilments.SLEEP"/>
		/// </summary>
		/// <remarks>
		/// SECRET-POWER
		/// </remarks>	
		x0C6 = 198,
		/// <summary>
		/// User receives 1/3 the damage inflicted in recoil.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  User takes 1/3 the damage it inflicts in recoil.
		/// </summary>	
		/// <remarks>
		/// DOUBLE-EDGE,BRAVE-BIRD,WOOD-HAMMER
		/// </remarks>	
		x0C7 = 199,
		/// <summary>
		/// Confuses the target.<para></para> [Confuse]{mechanic:confuse}s all targets.
		/// </summary>	
		/// <remarks>
		/// TEETER-DANCE
		/// </remarks>	
		x0C8 = 200,
		/// <summary>
		/// Has an increased chance for a critical hit and a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to <see cref="Attack.MoveMetaAilments.BURN"/> the target.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  User's [critical hit]{mechanic:critical-hit} rate is one level higher when using this move. Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to <see cref="Attack.MoveMetaAilments.BURN"/> the target.
		/// </summary>	
		/// <remarks>
		/// BLAZE-KICK
		/// </remarks>	
		x0C9 = 201,
		/// <summary>
		/// Halves all Electric-type damage.<para></para> <seealso cref="Types.ELECTRIC"/> moves inflict half damage, regardless of target.  If the user leaves the [field]{mechanic:field}, this effect ends.
		/// </summary>
		/// <remarks>
		/// MUD-SPORT
		/// </remarks>	
		x0CA = 202,
		/// <summary>
		/// Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to badly poison the target.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to [badly poison]{mechanic:badly-poison} the target.
		/// </summary>	
		/// <remarks>
		/// POISON-FANG
		/// </remarks>	
		x0CB = 203,
		/// <summary>
		/// If there be weather, this move has doubled power and the weather's type.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  If a weather move is active, this move has double power, and its type becomes the type of the weather move.  <seealso cref="Moves.SHADOW_SKY"/> is typeless for the purposes of this move.
		/// </summary>
		/// <remarks>
		/// WEATHER-BALL
		/// </remarks>	
		x0CC = 204,
		/// <summary>
		/// Lowers the user's Special Attack by two stages after inflicting damage.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>, then lowers the user's [Special Attack]{mechanic:special-attack} by two [stages]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// OVERHEAT,PSYCHO-BOOST,DRACO-METEOR,LEAF-STORM,FLEUR-CANNON
		/// </remarks>	
		x0CD = 205,
		/// <summary>
		/// Lowers the target's Attack and Defense by one stage.<para></para> Lowers the target's [Attack]{mechanic:attack} and [Defense]{mechanic:defense} by one [stage]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// TICKLE
		/// </remarks>	
		x0CE = 206,
		/// <summary>
		/// Raises the user's Defense and Special Defense by one stage.<para></para> Raises the user's [Defense]{mechanic:defense} and [Special Defense]{mechanic:special-defense} by one [stage]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// COSMIC-POWER,DEFEND-ORDER
		/// </remarks>	
		x0CF = 207,
		/// <summary>
		/// Inflicts regular damage and can hit Bounce and Fly users.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.
		/// 
		/// This move can hit Pokémon under the effect of <seealso cref="Moves.BOUNCE"/>, <seealso cref="Moves.FLY"/>, or <seealso cref="Moves.SKY_DROP"/>.
		/// </summary>
		/// <remarks>
		/// SKY-UPPERCUT
		/// </remarks>	
		x0D0 = 208,
		/// <summary>
		/// Raises the user's Attack and Defense by one stage.<para></para> Raises the user's [Attack]{mechanic:attack} and [Defense]{mechanic:defense} by one [stage]{mechanic:stage} each.
		/// </summary>	
		/// <remarks>
		/// BULK-UP
		/// </remarks>	
		x0D1 = 209,
		/// <summary>
		/// Has an increased chance for a critical hit and a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to poison the target.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  User's [critical hit]{mechanic:critical-hit} rate is one level higher when using this move. Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to <see cref="Attack.MoveMetaAilments.POISON"/> the target.
		/// </summary>	
		/// <remarks>
		/// POISON-TAIL,CROSS-POISON
		/// </remarks>	
		x0D2 = 210,
		/// <summary>
		/// Halves all Fire-type damage.<para></para> <seealso cref="Types.FIRE"/> moves inflict half damage, regardless of target.  If the user leaves the [field]{mechanic:field}, this effect ends.
		/// </summary>
		/// <remarks>
		/// WATER-SPORT
		/// </remarks>	
		x0D3 = 211,
		/// <summary>
		/// Raises the user's Special Attack and Special Defense by one stage.<para></para> Raises the user's [Special Attack]{mechanic:special-attack} and [Special Defense]{mechanic:special-defense} by one [stage]{mechanic:stage} each.
		/// </summary>	
		/// <remarks>
		/// CALM-MIND
		/// </remarks>	
		x0D4 = 212,
		/// <summary>
		/// Raises the user's Attack and Speed by one stage.<para></para> Raises the user's [Attack]{mechanic:attack} and [Speed]{mechanic:speed} by one [stage]{mechanic:stage} each.
		/// </summary>	
		/// <remarks>
		/// DRAGON-DANCE
		/// </remarks>	
		x0D5 = 213,
		/// <summary>
		/// User's type changes to match the terrain.<para></para> User's type changes according to the terrain.
		/// 
		/// Terrain        | New type
		/// -------------- | --------------
		/// Building       | <seealso cref="Types.NORMAL"/>
		/// Cave           | <seealso cref="Types.ROCK"/>
		/// Desert         | <seealso cref="Types.GROUND"/>
		/// Grass          | <seealso cref="Types.GRASS"/>
		/// Mountain       | <seealso cref="Types.ROCK"/>
		/// Ocean          | <seealso cref="Types.WATER"/>
		/// Pond           | <seealso cref="Types.WATER"/>
		/// Road           | <seealso cref="Types.GROUND"/>
		/// Snow           | <seealso cref="Types.ICE"/>
		/// Tall grass     | <seealso cref="Types.GRASS"/>
		/// 
		/// In Pokémon Battle Revolution:
		/// 
		/// Terrain        | New type
		/// -------------- | --------------
		/// Courtyard      | <seealso cref="Types.NORMAL"/>
		/// Crystal        | <seealso cref="Types.ROCK"/>
		/// Gateway        | <seealso cref="Types.WATER"/>
		/// Magma          | <seealso cref="Types.ROCK"/>
		/// Main Street    | <seealso cref="Types.NORMAL"/>
		/// Neon           | <seealso cref="Types.NORMAL"/>
		/// Stargazer      | <seealso cref="Types.ROCK"/>
		/// Sunny Park     | <seealso cref="Types.GRASS"/>
		/// Sunset         | <seealso cref="Types.GROUND"/>
		/// Waterfall      | <seealso cref="Types.GRASS"/>
		/// </summary>
		/// <remarks>
		/// CAMOUFLAGE
		/// </remarks>	
		x0D6 = 214,
		/// <summary>
		/// Heals the user by half its max HP.<para></para> Heals the user for half its max [HP]{mechanic:hp}.  If the user is <seealso cref="Types.FLYING"/>, its <seealso cref="Types.FLYING"/> type is ignored until the end of this turn.
		/// </summary>
		/// <remarks>
		/// ROOST
		/// </remarks>	
		x0D7 = 215,
		/// <summary>
		/// Disables moves and immunities that involve flying or levitating for five turns.<para></para> For five turns (including this one), all immunities to <seealso cref="Types.GROUND"/> moves are disabled.  For the duration of this effect, the [evasion]{mechanic:evasion} of every Pokémon on the field is lowered by two [stages]{mechanic:stage}.  Cancels the effects of <seealso cref="Moves.BOUNCE"/>, <seealso cref="Moves.FLY"/>, and <seealso cref="Moves.SKY_DROP"/>.
		/// 
		/// Specifically, <seealso cref="Types.FLYING"/> Pokémon and those with <seealso cref="Abilities.LEVITATE"/> or that have used <seealso cref="Moves.MAGNET_RISE"/> are no longer immune to <seealso cref="Types.GROUND"/> attacks, <seealso cref="Abilities.ARENA_TRAP"/>, <seealso cref="Moves.SPIKES"/>, or <seealso cref="Moves.TOXIC_SPIKES"/>.
		/// 
		/// <seealso cref="Moves.BOUNCE"/>, <seealso cref="Moves.FLY"/>, <seealso cref="Moves.SKY_DROP"/>, <seealso cref="Moves.HIGH_JUMP-KICK"/>, <seealso cref="Moves.JUMP_KICK"/>, and <seealso cref="Moves.SPLASH"/> cannot be used while this move is in effect.
		/// 
		/// *Bug*: If this move is used during a double or triple battle while Pokémon are under the effect of <seealso cref="Moves.SKY_DROP"/>, Sky Drop's effect is not correctly canceled on its target, and it remains high in the air indefinitely.  As Sky Drop prevents the target from acting, the only way to subsequently remove it from the field is to faint it.
		/// </summary>
		/// <remarks>
		/// GRAVITY
		/// </remarks>	
		x0D8 = 216,
		/// <summary>
		/// Forces the target to have no evasion, and allows it to be hit by Psychic moves even if it's Dark.<para></para> Resets the target's [evasion]{mechanic:evasion} to normal and prevents any further boosting until the target leaves the [field]{mechanic:field}.  A <seealso cref="Types.DARK"/> Pokémon under this effect takes normal damage from <seealso cref="Types.PSYCHIC"/> moves.  This move itself ignores [accuracy]{mechanic:accuracy} and [evasion]{mechanic:evasion} modifiers.
		/// </summary>
		/// <remarks>
		/// MIRACLE-EYE
		/// </remarks>	
		x0D9 = 217,
		/// <summary>
		/// If the target is asleep, has double power and wakes it up.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  If the target is <see cref="Attack.MoveMetaAilments.SLEEP"/>ing, this move has double power, and the target wakes up.
		/// </summary>	
		/// <remarks>
		/// WAKE-UP-SLAP
		/// </remarks>	
		x0DA = 218,
		/// <summary>
		/// Lowers user's Speed by one stage.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>, then lowers the user's [Speed]{mechanic:speed} by one [stage]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// HAMMER-ARM,ICE-HAMMER
		/// </remarks>	
		x0DB = 219,
		/// <summary>
		/// Power raises when the user has lower Speed, up to a maximum of 150.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Power increases with the target's current [Speed]{mechanic:speed} compared to the user, given by `1 + 25 * target Speed / user Speed`, capped at 150.
		/// </summary>	
		/// <remarks>
		/// GYRO-BALL
		/// </remarks>	
		x0DC = 220,
		/// <summary>
		/// User faints.  Its replacement has its HP fully restored and any major status effect removed.<para></para> User faints.  Its replacement's [HP]{mechanic:hp} is fully restored, and any [major status effect]{mechanic:major-status-effect} is removed.  If the replacement Pokémon is immediately fainted by a switch-in effect, the next replacement is healed by this move instead.
		/// </summary>	
		/// <remarks>
		/// HEALING-WISH
		/// </remarks>	
		x0DD = 221,
		/// <summary>
		/// Has double power against Pokémon that have less than half their max HP remaining.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  If the target has less than half its max [HP]{mechanic:hp} remaining, this move has double power.
		/// </summary>	
		/// <remarks>
		/// BRINE
		/// </remarks>	
		x0DE = 222,
		/// <summary>
		/// Power and type depend on the held berry.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Power and type are determined by the user's held berry.  The berry is consumed.  If the user is not holding a berry, this move will [fail]{mechanic:fail}.
		/// </summary>	
		/// <remarks>
		/// NATURAL-GIFT
		/// </remarks>	
		x0DF = 223,
		/// <summary>
		/// Hits through Protect and Detect.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Removes the effects of <seealso cref="Moves.DETECT"/> or <seealso cref="Moves.PROTECT"/> from the target before hitting.
		/// 
		/// This move cannot be copied by <seealso cref="Moves.MIRROR_MOVE"/>, nor selected by <seealso cref="Moves.ASSIST"/> or <seealso cref="Moves.METRONOME"/>.
		/// </summary>
		/// <remarks>
		/// FEINT
		/// </remarks>	
		x0E0 = 224,
		/// <summary>
		/// If target has a berry, inflicts double damage and uses the berry.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  If the target is holding a berry, this move has double power, and the user takes the berry and uses it immediately.
		/// 
		/// If the target is holding a <seealso cref="Items.JABOCA_BERRY"/> or <seealso cref="Items.ROWAP_BERRY"/>, the berry is still removed, but has no effect.
		/// 
		/// If this move is [super effective]{mechanic:super-effective} and the target is holding a berry that can reduce this move's damage, it will do so, and will not be stolen.
		/// </summary>
		/// <remarks>
		/// PLUCK,BUG-BITE
		/// </remarks>	
		x0E1 = 225,
		/// <summary>
		/// For three turns, friendly Pokémon have doubled Speed.<para></para> For the next three turns, all Pokémon on the user's side of the [field]{mechanic:field} have their original [Speed]{mechanic:speed} doubled.  This effect remains if the user leaves the [field]{mechanic:field}.
		/// </summary>	
		/// <remarks>
		/// TAILWIND
		/// </remarks>	
		x0E2 = 226,
		/// <summary>
		/// Raises one of a friendly Pokémon's stats at random by two stages.<para></para> Raises one of the target's stats by two [stages]{mechanic:stage}.  The raised stat is chosen at random from any stats that can be raised by two stages.  If no stat is eligible, this move will [fail]{mechanic:fail}.
		/// 
		/// If the target has a <seealso cref="Moves.SUBSTITUTE"/>, this move will have no effect, even if the user is the target.
		/// 
		/// This move cannot be copied by <seealso cref="Moves.MIRROR_MOVE"/>.
		/// </summary>
		/// <remarks>
		/// ACUPRESSURE
		/// </remarks>	
		x0E3 = 227,
		/// <summary>
		/// Strikes back at the last Pokémon to hit the user this turn with 1.5× the damage.<para></para> Targets the last opposing Pokémon to hit the user with a damaging move this turn.  Inflicts 1.5× the damage that move did to the user.  If there is no eligible target, this move will [fail]{mechanic:fail}.  Type immunity applies, but other type effects are ignored.
		/// </summary>	
		/// <remarks>
		/// METAL-BURST
		/// </remarks>	
		x0E4 = 228,
		/// <summary>
		/// User must switch out after attacking.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>, then the user immediately [switches out]{mechanic:switches-out}, and the trainer selects a replacement Pokémon from the party.  If the target [faint]{mechanic:faint}s from this attack, the user's trainer selects the new Pokémon to send out first.  If the user is the last Pokémon in its party that can battle, it will not [switch out]{mechanic:switch-out}.
		/// 
		/// The user may be hit by <seealso cref="Moves.PURSUIT"/> when it [switches out]{mechanic:switches-out}, if it has been targeted and <seealso cref="Moves.PURSUIT"/> has not yet been used.
		/// 
		/// This move may be used even if the user is under the effect of <seealso cref="Moves.INGRAIN"/>.  <seealso cref="Moves.INGRAIN"/>'s effect will end.
		/// </summary>
		/// <remarks>
		/// U-TURN,VOLT-SWITCH
		/// </remarks>	
		x0E5 = 229,
		/// <summary>
		/// Lowers the user's Defense and Special Defense by one stage after inflicting damage.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>, then lowers the user's [Defense]{mechanic:defense} and [Special Defense]{mechanic:special-defense} by one [stage]{mechanic:stage} each.
		/// </summary>	
		/// <remarks>
		/// CLOSE-COMBAT,DRAGON-ASCENT
		/// </remarks>	
		x0E6 = 230,
		/// <summary>
		/// Power is doubled if the target has already moved this turn.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  If the target uses a move or [switches out]{mechanic:switches-out} this turn before this move is used, this move has double power.
		/// </summary>	
		/// <remarks>
		/// PAYBACK
		/// </remarks>	
		x0E7 = 231,
		/// <summary>
		/// Power is doubled if the target has already received damage this turn.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  If the target takes damage this turn for any reason before this move is used, this move has double power.
		/// </summary>	
		/// <remarks>
		/// ASSURANCE
		/// </remarks>	
		x0E8 = 232,
		/// <summary>
		/// Target cannot use held items.<para></para> Target cannot use its held item for five turns.  If the target leaves the [field]{mechanic:field}, this effect ends.
		/// 
		/// If a Pokémon under this effect uses <seealso cref="Moves.BUG_BITE"/> or <seealso cref="Moves.PLUCK"/> on a Pokémon holding a berry, the berry is destroyed but not used.  If a Pokémon under this effect uses <seealso cref="Moves.FLING"/>, it will [fail]{mechanic:fail}.
		/// 
		/// This effect is passed by <seealso cref="Moves.BATON_PASS"/>.
		/// </summary>
		/// <remarks>
		/// EMBARGO
		/// </remarks>	
		x0E9 = 233,
		/// <summary>
		/// Throws held item at the target; power depends on the item.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Power and type are determined by the user's [held item]{mechanic:held-item}.  The item is consumed.  If the user is not holding an item, or its item has no set type and power, this move will [fail]{mechanic:fail}.
		/// 
		/// This move ignores <seealso cref="Abilities.STICKY_HOLD"/>.
		/// 
		/// If the user is under the effect of <seealso cref="Moves.EMBARGO"/>, this move will [fail]{mechanic:fail}.
		/// </summary>
		/// <remarks>
		/// FLING
		/// </remarks>	
		x0EA = 234,
		/// <summary>
		/// Transfers the user's major status effect to the target.<para></para> If the user has a [major status effect]{mechanic:major-status-effect} and the target does not, the user's status is transferred to the target.
		/// </summary>	
		/// <remarks>
		/// PSYCHO-SHIFT
		/// </remarks>	
		x0EB = 235,
		/// <summary>
		/// Power increases when this move has less PP, up to a maximum of 200.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Power is determined by the [PP]{mechanic:pp} remaining for this move, after its [PP]{mechanic:pp} cost is deducted.  Ignores [accuracy]{mechanic:accuracy} and [evasion]{mechanic:evasion} modifiers.
		/// 
		/// PP remaining | Power
		/// ------------ | ----:
		/// 4 or more    |    40
		/// 3            |    50
		/// 2            |    60
		/// 1            |    80
		/// 0            |   200
		/// 
		/// If this move is activated by another move, the activating move's [PP]{mechanic:pp} is used to calculate power.
		/// </summary>
		/// <remarks>
		/// TRUMP-CARD
		/// </remarks>	
		x0EC = 236,
		/// <summary>
		/// Prevents target from restoring its HP for five turns.<para></para> For the next five turns, the target may not use any moves that only restore [HP]{mechanic:hp}, and move effects that heal the target are disabled.  Moves that steal [HP]{mechanic:hp} may still be used, but will only inflict damage and not heal the target.
		/// </summary>	
		/// <remarks>
		/// HEAL-BLOCK
		/// </remarks>	
		x0ED = 237,
		/// <summary>
		/// Power increases against targets with more HP remaining, up to a maximum of 121 power.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Power directly relates to the target's relative remaining [HP]{mechanic:hp}, given by `1 + 120 * current HP / max HP`, to a maximum of 121.
		/// </summary>	
		/// <remarks>
		/// WRING-OUT,CRUSH-GRIP
		/// </remarks>	
		x0EE = 238,
		/// <summary>
		/// User swaps Attack and Defense.<para></para> The user's original [Attack]{mechanic:attack} and [Defense]{mechanic:defense} are swapped.
		/// 
		/// This effect is passed on by <seealso cref="Moves.BATON_PASS"/>.
		/// </summary>
		/// <remarks>
		/// POWER-TRICK
		/// </remarks>	
		x0EF = 239,
		/// <summary>
		/// Nullifies target's ability until it leaves battle.<para></para> The target's ability is disabled as long as it remains on the [field]{mechanic:field}.
		/// 
		/// This effect is passed on by <seealso cref="Moves.BATON_PASS"/>.
		/// </summary>
		/// <remarks>
		/// GASTRO-ACID
		/// </remarks>	
		x0F0 = 240,
		/// <summary>
		/// Prevents the target from scoring critical hits for five turns.<para></para> For five turns, opposing Pokémon cannot score [critical hits]{mechanic:critical-hit}.
		/// </summary>	
		/// <remarks>
		/// LUCKY-CHANT
		/// </remarks>	
		x0F1 = 241,
		/// <summary>
		/// Uses the target's move against it before it attacks, with power increased by half.<para></para> If the target has selected a damaging move this turn, the user will copy that move and use it against the target, with a 50% increase in power.
		/// 
		/// If the target moves before the user, this move will [fail]{mechanic:fail}.
		/// 
		/// This move cannot be copied by <seealso cref="Moves.MIRROR_MOVE"/>, nor selected by <seealso cref="Moves.ASSIST"/>, <seealso cref="Moves.METRONOME"/>, or <seealso cref="Moves.SLEEP_TALK"/>.
		/// </summary>
		/// <remarks>
		/// ME-FIRST
		/// </remarks>	
		x0F2 = 242,
		/// <summary>
		/// Uses the target's last used move.<para></para> Uses the last move that was used successfully by any Pokémon, including the user.
		/// 
		/// This move cannot copy itself, nor <seealso cref="Moves.ROAR"/> nor <seealso cref="Moves.WHIRLWIND"/>.
		/// 
		/// This move cannot be copied by <seealso cref="Moves.MIRROR_MOVE"/>, nor selected by <seealso cref="Moves.ASSIST"/>, <seealso cref="Moves.METRONOME"/>, or <seealso cref="Moves.SLEEP_TALK"/>.
		/// </summary>
		/// <remarks>
		/// COPYCAT
		/// </remarks>	
		x0F3 = 243,
		/// <summary>
		/// User swaps Attack and Special Attack changes with the target.<para></para> User swaps its [Attack]{mechanic:attack} and [Special Attack]{mechanic:special-attack} [stat modifiers]{mechanic:stat-modifiers} modifiers with the target.
		/// </summary>	
		/// <remarks>
		/// POWER-SWAP
		/// </remarks>	
		x0F4 = 244,
		/// <summary>
		/// User swaps Defense and Special Defense changes with the target.<para></para> User swaps its [Defense]{mechanic:defense} and [Special Defense]{mechanic:special-defense} modifiers with the target.
		/// </summary>	
		/// <remarks>
		/// GUARD-SWAP
		/// </remarks>	
		x0F5 = 245,
		/// <summary>
		/// Power increases against targets with more raised stats, up to a maximum of 200.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Power starts at 60 and is increased by 20 for every [stage]{mechanic:stage} any of the target's stats has been raised, capping at 200.  [Accuracy]{mechanic:accuracy} and [evasion]{mechanic:evasion} modifiers do not increase this move's power.
		/// </summary>	
		/// <remarks>
		/// PUNISHMENT
		/// </remarks>	
		x0F6 = 246,
		/// <summary>
		/// Can only be used after all of the user's other moves have been used.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  This move can only be used if each of the user's other moves has been used at least once since the user entered the [field]{mechanic:field}.  If this is the user's only move, this move will [fail]{mechanic:fail}.
		/// </summary>	
		/// <remarks>
		/// LAST-RESORT
		/// </remarks>	
		x0F7 = 247,
		/// <summary>
		/// Changes the target's ability to Insomnia.<para></para> Changes the target's ability to <seealso cref="Abilities.INSOMNIA"/>.
		/// 
		/// If the target's ability is <seealso cref="Abilities.TRUANT"/> or <seealso cref="Abilities.MULTITYPE"/>, this move will [fail]{mechanic:fail}.
		/// </summary>
		/// <remarks>
		/// WORRY-SEED
		/// </remarks>	
		x0F8 = 248,
		/// <summary>
		/// Only works if the target is about to use a damaging move.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  If the target has not selected a damaging move this turn, or if the target has already acted this turn, this move will [fail]{mechanic:fail}.
		/// 
		/// This move is not affected by <seealso cref="Abilities.IRON_FIST"/>.
		/// </summary>
		/// <remarks>
		/// SUCKER-PUNCH
		/// </remarks>	
		x0F9 = 249,
		/// <summary>
		/// Scatters poisoned spikes, poisoning opposing Pokémon that switch in.<para></para> Scatters poisoned spikes around the opposing [field]{mechanic:field}, which <see cref="Attack.MoveMetaAilments.POISON"/> opposing Pokémon that enter the [field]{mechanic:field}.  A second layer of these spikes may be laid down, in which case Pokémon will be [badly poison]{mechanic:badly-poison}ed instead.  Pokémon immune to either <seealso cref="Types.GROUND"/> moves or being <see cref="Attack.MoveMetaAilments.POISON"/>ed are immune to this effect.  Pokémon otherwise immune to <seealso cref="Types.GROUND"/> moves are affected during <seealso cref="Moves.GRAVITY"/>.
		/// 
		/// If a <seealso cref="Types.POISON"/> Pokémon not immune to <seealso cref="Types.GROUND"/> moves enters a [field]{mechanic:field} covered with poisoned spikes, the spikes are removed.
		/// 
		/// <seealso cref="Moves.RAPID_SPIN"/> will remove this effect from its user's side of the [field]{mechanic:field}.  <seealso cref="Moves.DEFOG"/> will remove this effect from its target's side of the [field]{mechanic:field}.
		/// 
		/// This move does not trigger <seealso cref="Abilities.SYNCHRONIZE"/>, unless the Pokémon with <seealso cref="Abilities.SYNCHRONIZE"/> was forced to enter the [field]{mechanic:field} by another effect such as <seealso cref="Moves.ROAR"/>.
		/// 
		/// Pokémon entering the [field]{mechanic:field} due to <seealso cref="Moves.BATON_PASS"/> are not affected by this effect.
		/// </summary>
		/// <remarks>
		/// TOXIC-SPIKES
		/// </remarks>	
		x0FA = 250,
		/// <summary>
		/// User and target swap stat changes.<para></para> User swaps its [stat modifiers]{mechanic:stat-modifiers} with the target.
		/// </summary>	
		/// <remarks>
		/// HEART-SWAP
		/// </remarks>	
		x0FB = 251,
		/// <summary>
		/// Restores 1/16 of the user's max HP each turn.<para></para> Restores 1/16 of the user's max [HP]{mechanic:hp} at the end of each turn.  If the user leaves the [field]{mechanic:field}, this effect ends.
		/// 
		/// This effect is passed on by <seealso cref="Moves.BATON_PASS"/>.
		/// </summary>
		/// <remarks>
		/// AQUA-RING
		/// </remarks>	
		x0FC = 252,
		/// <summary>
		/// User is immune to Ground moves and effects for five turns.<para></para> For five turns, the user is immune to <seealso cref="Types.GROUND"/> moves.
		/// 
		/// If the user is under the effect of <seealso cref="Moves.INGRAIN"/> or has <seealso cref="Abilities.LEVITATE"/>, this move will [fail]{mechanic:fail}.
		/// 
		/// This effect is temporarily disabled by and cannot be used during <seealso cref="Moves.GRAVITY"/>.
		/// 
		/// This effect is passed on by <seealso cref="Moves.BATON_PASS"/>.
		/// </summary>
		/// <remarks>
		/// MAGNET-RISE
		/// </remarks>	
		x0FD = 253,
		/// <summary>
		/// User takes 1/3 the damage inflicted in recoil.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to <see cref="Attack.MoveMetaAilments.BURN"/> the target.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  User takes 1/3 the damage it inflicts in recoil.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to <see cref="Attack.MoveMetaAilments.BURN"/> the target.  [Frozen]{mechanic:frozen} Pokémon may use this move, in which case they will thaw.
		/// </summary>	
		/// <remarks>
		/// FLARE-BLITZ
		/// </remarks>	
		x0FE = 254,
		/// <summary>
		/// User takes 1/4 its max HP in recoil.<para></para> Inflicts [typeless]{mechanic:typeless} <seealso cref="Attack.MetaCategory.DAMAGE"/>.  User takes 1/4 its max [HP]{mechanic:hp} in recoil.  Ignores [accuracy]{mechanic:accuracy} and [evasion]{mechanic:evasion} modifiers.
		/// 
		/// This move is used automatically when a Pokémon cannot use any other move legally, e.g., due to having no [PP]{mechanic:pp} remaining or being under the effect of both <seealso cref="Moves.ENCORE"/> and <seealso cref="Moves.TORMENT"/> at the same time.
		/// 
		/// This move's recoil is not treated as recoil for the purposes of anything that affects recoil, such as the ability <seealso cref="Abilities.ROCK_HEAD"/>.  It also is not prevented by <seealso cref="Abilities.MAGIC_GUARD"/>.
		/// 
		/// This move cannot be copied by <seealso cref="Moves.MIMIC"/>, <seealso cref="Moves.MIRROR_MOVE"/>, or <seealso cref="Moves.SKETCH"/>, nor selected by <seealso cref="Moves.ASSIST"/> or <seealso cref="Moves.METRONOME"/>, nor forced by <seealso cref="Moves.ENCORE"/>.
		/// </summary>
		/// <remarks>
		/// STRUGGLE
		/// </remarks>	
		x0FF = 255,
		/// <summary>
		/// User dives underwater, dodging all attacks, and hits next turn.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  User dives underwater for one turn, becoming immune to attack, and hits on the second turn.
		/// 
		/// During the immune turn, <seealso cref="Moves.SURF"/>, and <seealso cref="Moves.WHIRLPOOL"/> still hit the user normally, and their power is doubled if appropriate.
		/// 
		/// The user may be hit during its immune turn if under the effect of <seealso cref="Moves.LOCK_ON"/>, <seealso cref="Moves.MIND_READER"/>, or <seealso cref="Abilities.NO_GUARD"/>.
		/// 
		/// This move cannot be selected by <seealso cref="Moves.SLEEP_TALK"/>.
		/// </summary>
		/// <remarks>
		/// DIVE
		/// </remarks>	
		x100 = 256,
		/// <summary>
		/// User digs underground, dodging all attacks, and hits next turn.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  User digs underground for one turn, becoming immune to attack, and hits on the second turn.
		/// 
		/// During the immune turn, <seealso cref="Moves.EARTHQUAKE"/>, <seealso cref="Moves.FISSURE"/>, and <seealso cref="Moves.MAGNITUDE"/> still hit the user normally, and their power is doubled if appropriate.
		/// 
		/// The user may be hit during its immune turn if under the effect of <seealso cref="Moves.LOCK_ON"/>, <seealso cref="Moves.MIND_READER"/>, or <seealso cref="Abilities.NO_GUARD"/>.
		/// 
		/// This move cannot be selected by <seealso cref="Moves.SLEEP_TALK"/>.
		/// </summary>
		/// <remarks>
		/// DIG
		/// </remarks>	
		x101 = 257,
		/// <summary>
		/// Inflicts regular damage and can hit Dive users.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.
		/// 
		/// If the target is in the first turn of <seealso cref="Moves.DIVE"/>, this move will hit with double power.
		/// </summary>
		/// <remarks>
		/// SURF
		/// </remarks>	
		x102 = 258,
		/// <summary>
		/// Lowers the target's evasion by one stage.  Removes field effects from the enemy field.<para></para> Lowers the target's [evasion]{mechanic:evasion} by one [stage]{mechanic:stage}.  Clears away fog.  Removes the effects of <seealso cref="Moves.MIST"/>, <seealso cref="Moves.LIGHT_SCREEN"/>, <seealso cref="Moves.REFLECT"/>, <seealso cref="Moves.SAFEGUARD"/>, <seealso cref="Moves.SPIKES"/>, <seealso cref="Moves.STEALTH_ROCK"/>, and <seealso cref="Moves.TOXIC_SPIKES"/> from the target's side of the [field]{mechanic:field}.
		/// 
		/// If the target is protected by <seealso cref="Moves.MIST"/>, it will prevent the [evasion]{mechanic:evasion} change, then be removed by this move.
		/// </summary>
		/// <remarks>
		/// DEFOG
		/// </remarks>	
		x103 = 259,
		/// <summary>
		/// For five turns, slower Pokémon will act before faster Pokémon.<para></para> For five turns (including this one), slower Pokémon will act before faster Pokémon.  Move priority is not affected.  Using this move when its effect is already active will end the effect.
		/// 
		/// Pokémon holding <seealso cref="Items.FULL_INCENSE"/>, <seealso cref="Items.LAGGING_TAIL"/>, or <seealso cref="Items.QUICK_CLAW"/> and Pokémon with <seealso cref="Abilities.STALL"/> ignore this effect.
		/// </summary>
		/// <remarks>
		/// TRICK-ROOM
		/// </remarks>	
		x104 = 260,
		/// <summary>
		/// Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to freeze the target.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to <see cref="Attack.MoveMetaAilments.FREEZE"/> the target.
		/// 
		/// During <seealso cref="Moves.HAIL"/>, this move has 100% accuracy.  It also has a (100 - accuracy)% chance to break through the protection of <seealso cref="Moves.PROTECT"/> and <seealso cref="Moves.DETECT"/>.
		/// </summary>
		/// <remarks>
		/// BLIZZARD
		/// </remarks>	
		x105 = 261,
		/// <summary>
		/// Prevents the target from leaving battle and inflicts 1/16 its max HP in damage for 2-5 turns.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  For the next 2–5 turns, the target cannot leave the field and is damaged for 1/16 its max HP at the end of each turn.  The user continues to use other moves during this time.  If the user leaves the [field]{mechanic:field}, this effect ends.
		/// 
		/// Has a 3/8 chance each to hit 2 or 3 times, and a 1/8 chance each to hit 4 or 5 times.  Averages to 3 hits per use.
		/// 
		/// If the target is in the first turn of <seealso cref="Moves.DIVE"/>, this move will hit with double power.
		/// </summary>
		/// <remarks>
		/// WHIRLPOOL
		/// </remarks>	
		x106 = 262,
		/// <summary>
		/// User takes 1/3 the damage inflicted in recoil.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to paralyze the target.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  User takes 1/3 the damage it inflicts in recoil.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to [paralyze]{mechanic:paralyze} the target.
		/// </summary>	
		/// <remarks>
		/// VOLT-TACKLE
		/// </remarks>	
		x107 = 263,
		/// <summary>
		/// User bounces high into the air, dodging all attacks, and hits next turn.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  User bounces high into the air for one turn, becoming immune to attack, and hits on the second turn.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to [paralyze]{mechanic:paralyze} the target.
		/// 
		/// During the immune turn, <seealso cref="Moves.GUST"/>, <seealso cref="Moves.HURRICANE"/>, <seealso cref="Moves.SKY_UPPERCUT"/>, <seealso cref="Moves.SMACK_DOWN"/>, <seealso cref="Moves.THUNDER"/>, and <seealso cref="Moves.TWISTER"/> still hit the user normally.  <seealso cref="Moves.GUST"/> and <seealso cref="Moves.TWISTER"/> also have double power against the user.
		/// 
		/// The damage from <seealso cref="Moves.HAIL"/> and <seealso cref="Moves.SANDSTORM"/> still applies during the immune turn.
		/// 
		/// The user may be hit during its immune turn if under the effect of <seealso cref="Moves.LOCK_ON"/>, <seealso cref="Moves.MIND_READER"/>, or <seealso cref="Abilities.NO_GUARD"/>.
		/// 
		/// This move cannot be used while <seealso cref="Moves.GRAVITY"/> is in effect.
		/// 
		/// This move cannot be selected by <seealso cref="Moves.SLEEP_TALK"/>.
		/// </summary>
		/// <remarks>
		/// BOUNCE
		/// </remarks>	
		x108 = 264,
		/// <summary>
		/// Lowers the target's Special Attack by two stages if it's the opposite gender.<para></para> Lowers the target's [Special Attack]{mechanic:special-attack} by two [stages]{mechanic:stage}.  If the user and target are the same gender, or either is genderless, this move will [fail]{mechanic:fail}.
		/// </summary>	
		/// <remarks>
		/// CAPTIVATE
		/// </remarks>	
		x10A = 266,
		/// <summary>
		/// Causes damage when opposing Pokémon switch in.<para></para> Spreads sharp rocks around the opposing [field]{mechanic:field}, damaging any Pokémon that enters the [field]{mechanic:field} for 1/8 its max [HP]{mechanic:hp}.  This damage is affected by the entering Pokémon's susceptibility to <seealso cref="Types.ROCK"/> moves.
		/// 
		/// <seealso cref="Moves.RAPID_SPIN"/> removes this effect from its user's side of the [field]{mechanic:field}.
		/// </summary>
		/// <remarks>
		/// STEALTH-ROCK
		/// </remarks>	
		x10B = 267,
		/// <summary>
		/// Has a higher chance to confuse the target when the recorded sound is louder.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Has either a 1%, 11%, or 31% chance to [confuse]{mechanic:confuse} the target, based on the volume of the recording made for this move; louder recordings increase the chance of <see cref="Attack.MoveMetaAilments.CONFUSION"/>.  If the user is not a <seealso cref="Pokemons.CHATOT"/>, this move will not cause <see cref="Attack.MoveMetaAilments.CONFUSION"/>.
		/// 
		/// This move cannot be copied by <seealso cref="Moves.MIMIC"/>, <seealso cref="Moves.MIRROR_MOVE"/>, or <seealso cref="Moves.SKETCH"/>, nor selected by <seealso cref="Moves.ASSIST"/>, <seealso cref="Moves.METRONOME"/>, or <seealso cref="Moves.SLEEP_TALK"/>.
		/// </summary>
		/// <remarks>
		/// CHATTER
		/// </remarks>	
		x10C = 268,
		/// <summary>
		/// If the user is holding a appropriate plate or drive, the damage inflicted will match it.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  If the user is holding a plate or a drive, this move's type is the type corresponding to that item.
		/// 
		/// Note: This effect is technically shared by both <seealso cref="Moves.TECHNO_BLAST"/> and <seealso cref="Moves.JUDGMENT"/>; however, Techno Blast is only affected by drives, and Judgment is only affected by plates.
		/// </summary>
		/// <remarks>
		/// JUDGMENT,TECHNO-BLAST,MULTI-ATTACK
		/// </remarks>	
		x10D = 269,
		/// <summary>
		/// User receives 1/2 the damage inflicted in recoil.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  User takes 1/2 the damage it inflicts in recoil.
		/// </summary>	
		/// <remarks>
		/// HEAD-SMASH,LIGHT-OF-RUIN
		/// </remarks>	
		x10E = 270,
		/// <summary>
		/// User faints, and its replacement is fully healed.<para></para> User [faint]{mechanic:faint}s.  Its replacement's [HP]{mechanic:hp} and [PP]{mechanic:pp} are fully restored, and any [major status effect]{mechanic:major-status-effect} is removed.
		/// </summary>	
		/// <remarks>
		/// LUNAR-DANCE
		/// </remarks>	
		x10F = 271,
		/// <summary>
		/// Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to lower the target's Special Defense by two stages.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to lower the target's [Special Defense]{mechanic:special-defense} by two [stages]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// SEED-FLARE
		/// </remarks>	
		x110 = 272,
		/// <summary>
		/// User vanishes, dodging all attacks, and hits next turn.  Hits through Protect and Detect.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  User vanishes for one turn, becoming immune to attack, and hits on the second turn.
		/// 
		/// This move ignores the effects of <seealso cref="Moves.DETECT"/> and <seealso cref="Moves.PROTECT"/>.
		/// 
		/// This move cannot be selected by <seealso cref="Moves.SLEEP_TALK"/>.
		/// </summary>
		/// <remarks>
		/// SHADOW-FORCE,PHANTOM-FORCE
		/// </remarks>	
		x111 = 273,
		/// <summary>
		/// Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to <see cref="Attack.MoveMetaAilments.BURN"/> the target and a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to make the target flinch.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to <see cref="Attack.MoveMetaAilments.BURN"/> the target and a separate <see cref="Attack.Data.MetaData.AilmentChance"/> chance to make the target [flinch]{mechanic:flinch}.
		/// </summary>	
		/// <remarks>
		/// FIRE-FANG
		/// </remarks>	
		x112 = 274,
		/// <summary>
		/// Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to freeze the target and a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to make the target flinch.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to <see cref="Attack.MoveMetaAilments.FREEZE"/> the target and a separate <see cref="Attack.Data.MetaData.AilmentChance"/> chance to make the target [flinch]{mechanic:flinch}.
		/// </summary>	
		/// <remarks>
		/// ICE-FANG
		/// </remarks>	
		x113 = 275,
		/// <summary>
		/// Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to paralyze the target and a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to make the target flinch.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to [paralyze]{mechanic:paralyze} the target and a separate <see cref="Attack.Data.MetaData.AilmentChance"/> chance to make the target [flinch]{mechanic:flinch}.
		/// </summary>	
		/// <remarks>
		/// THUNDER-FANG
		/// </remarks>	
		x114 = 276,
		/// <summary>
		/// Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to raise the user's Special Attack by one stage.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to raise the user's [Special Attack]{mechanic:special-attack} by one [stage]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// CHARGE-BEAM,FIERY-DANCE
		/// </remarks>	
		x115 = 277,
		/// <summary>
		/// Raises the user's Attack and accuracy by one stage.<para></para> Raises the user's [Attack]{mechanic:attack} and [accuracy]{mechanic:accuracy} by one [stage]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// HONE-CLAWS
		/// </remarks>	
		x116 = 278,
		/// <summary>
		/// Prevents any multi-target moves from hitting friendly Pokémon this turn.<para></para> Moves with multiple targets will not hit friendly Pokémon for the remainder of this turn.  If the user is last to act this turn, this move will [fail]{mechanic:fail}.
		/// 
		/// This move cannot be selected by <seealso cref="Moves.ASSIST"/> or <seealso cref="Moves.METRONOME"/>.
		/// </summary>
		/// <remarks>
		/// WIDE-GUARD
		/// </remarks>	
		x117 = 279,
		/// <summary>
		/// Averages Defense and Special Defense with the target.<para></para> Averages the user's unmodified [Defense]{mechanic:defense} with the target's unmodified Defense; the value becomes the unmodified Defense for both Pokémon. Unmodified [Special Defense]{mechanic:special-defense} is averaged the same way.
		/// </summary>	
		/// <remarks>
		/// GUARD-SPLIT
		/// </remarks>	
		x118 = 280,
		/// <summary>
		/// Averages Attack and Special Attack with the target.<para></para> Averages the user's unmodified [Attack]{mechanic:attack} with the target's unmodified Attack; the value becomes the unmodified Attack for both Pokémon. Unmodified [Special Attack]{mechanic:special-attack} is averaged the same way.
		/// 
		/// This effect applies before any other persistent changes to unmodified Attack or Special Attack, such as <seealso cref="Abilities.FLOWER_GIFT"/> during <seealso cref="Moves.SUNNY_DAY"/>.
		/// </summary>
		/// <remarks>
		/// POWER-SPLIT
		/// </remarks>	
		x119 = 281,
		/// <summary>
		/// All Pokémon's Defense and Special Defense are swapped for 5 turns.<para></para> For five turns (including this one), every Pokémon's [Defense]{mechanic:defense} and [Special Defense]{mechanic:special-defense} are swapped.
		/// </summary>	
		/// <remarks>
		/// WONDER-ROOM
		/// </remarks>	
		x11A = 282,
		/// <summary>
		/// Inflicts damage based on the target's Defense, not Special Defense.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Damage calculation always uses the target's [Defense]{mechanic:defense}, regardless of this move's damage class.
		/// </summary>	
		/// <remarks>
		/// PSYSHOCK,PSYSTRIKE,SECRET-SWORD
		/// </remarks>	
		x11B = 283,
		/// <summary>
		/// Inflicts double damage if the target is Poisoned.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  If the target is [poisoned]{mechanic:poisoned}, this move has double power.
		/// </summary>	
		/// <remarks>
		/// VENOSHOCK
		/// </remarks>	
		x11C = 284,
		/// <summary>
		/// Raises the user's Speed by two stages and halves the user's weight.<para></para> Raises the user's [Speed]{mechanic:speed} by two [stages]{mechanic:stage}.  Halves the user's weight; this effect does not stack.
		/// </summary>	
		/// <remarks>
		/// AUTOTOMIZE
		/// </remarks>	
		x11D = 285,
		/// <summary>
		/// Moves have 100% accuracy against the target for three turns.<para></para> For three turns (including this one), moves used against the target have 100% [accuracy]{mechanic:accuracy}, but the target is immune to <seealso cref="Types.GROUND"/> damage.  Accuracy of one-hit KO moves is exempt from this effect.
		/// 
		/// This effect is removed by <seealso cref="Moves.GRAVITY"/>.  If Gravity is already in effect, this move will [fail]{mechanic:fail}.
		/// </summary>
		/// <remarks>
		/// TELEKINESIS
		/// </remarks>	
		x11E = 286,
		/// <summary>
		/// Negates held items for five turns.<para></para> For five turns (including this one), passive effects of held items are ignored, and Pokémon will not use their held items.
		/// </summary>	
		/// <remarks>
		/// MAGIC-ROOM
		/// </remarks>	
		x11F = 287,
		/// <summary>
		/// Removes any immunity to Ground damage.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Removes the target's [immunity]{mechanic:immune} to <seealso cref="Types.GROUND"/>-type damage.  This effect removes any existing Ground immunity due to <seealso cref="Abilities.LEVITATE"/>, <seealso cref="Moves.MAGNET_RISE"/>, or <seealso cref="Moves.TELEKINESIS"/>, and causes the target's <seealso cref="Types.FLYING"/> type to be ignored when it takes Ground damage.
		/// 
		/// If the target isn't immune to Ground damage, this move will [fail]{mechanic:fail}.
		/// 
		/// This move can hit Pokémon under the effect of <seealso cref="Moves.BOUNCE"/>, <seealso cref="Moves.FLY"/>, or <seealso cref="Moves.SKY_DROP"/>, and ends the effect of Bounce or Fly.
		/// </summary>
		/// <remarks>
		/// SMACK-DOWN
		/// </remarks>	
		x120 = 288,
		/// <summary>
		/// Always scores a critical hit.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Always scores a [critical hit]{mechanic:critical-hit}.
		/// </summary>	
		/// <remarks>
		/// STORM-THROW,FROST-BREATH
		/// </remarks>	
		x121 = 289,
		/// <summary>
		/// Deals splash damage to Pokémon next to the target.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  If this move successfully hits the target, any Pokémon adjacent to the target are damaged for 1/16 their max [HP]{mechanic:hp}.
		/// </summary>	
		/// <remarks>
		/// FLAME-BURST
		/// </remarks>	
		x122 = 290,
		/// <summary>
		/// Raises the user's Special Attack, Special Defense, and Speed by one stage each.<para></para> Raises the user's [Special Attack]{mechanic:special-attack}, [Special Defense]{mechanic:special-defense}, and [Speed]{mechanic:speed} by one [stage]{mechanic:stage} each.
		/// </summary>	
		/// <remarks>
		/// QUIVER-DANCE
		/// </remarks>	
		x123 = 291,
		/// <summary>
		/// Power is higher when the user weighs more than the target, up to a maximum of 120.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  The greater the user's weight compared to the target's, the higher power this move has, to a maximum of 120.
		/// 
		/// User's weight                    | Power
		/// -------------------------------- | ----:
		/// Up to 2× the target's weight     |    40
		/// Up to 3× the target's weight     |    60
		/// Up to 4× the target's weight     |    80
		/// Up to 5× the target's weight     |   100
		/// More than 5× the target's weight |   120
		/// </summary>
		/// <remarks>
		/// HEAVY-SLAM,HEAT-CRASH
		/// </remarks>	
		x124 = 292,
		/// <summary>
		/// Hits any Pokémon that shares a type with the user.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Only Pokémon that share a type with the user will take damage from this move.
		/// </summary>	
		/// <remarks>
		/// SYNCHRONOISE
		/// </remarks>	
		x125 = 293,
		/// <summary>
		/// Power is higher when the user has greater Speed than the target, up to a maximum of 150.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  The greater the user's [Speed]{mechanic:speed} compared to the target's, the higher power this move has, to a maximum of 150.
		/// 
		/// User's Speed                     | Power
		/// -------------------------------- | ----:
		/// Up to 2× the target's Speed      |    60
		/// Up to 3× the target's Speed      |    80
		/// Up to 4× the target's Speed      |   120
		/// More than 4× the target's Speed  |   150
		/// </summary>
		/// <remarks>
		/// ELECTRO-BALL
		/// </remarks>	
		x126 = 294,
		/// <summary>
		/// Changes the target's type to Water.<para></para> Changes the target to pure <seealso cref="Types.WATER"/>-type until it leaves the field.  If the target has <seealso cref="Abilities.MULTITYPE"/>, this move will [fail]{mechanic:fail}.
		/// </summary>
		/// <remarks>
		/// SOAK
		/// </remarks>	
		x127 = 295,
		/// <summary>
		/// Inflicts regular damage.  Raises the user's Speed by one stage.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Raises the user's [Speed]{mechanic:speed} by one [stage]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// FLAME-CHARGE
		/// </remarks>	
		x128 = 296,
		/// <summary>
		/// Lowers the target's Special Defense by two stages.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Lowers the target's [Special Defense]{mechanic:special-defense} by two [stages]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// ACID-SPRAY
		/// </remarks>	
		x129 = 297,
		/// <summary>
		/// Calculates damage with the target's attacking stat.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Damage is calculated using the target's attacking stat rather than the user's.
		/// </summary>	
		/// <remarks>
		/// FOUL-PLAY
		/// </remarks>	
		x12A = 298,
		/// <summary>
		/// Changes the target's ability to Simple.<para></para> Changes the target's ability to <seealso cref="Abilities.SIMPLE"/>.
		/// </summary>
		/// <remarks>
		/// SIMPLE-BEAM
		/// </remarks>	
		x12B = 299,
		/// <summary>
		/// Copies the user's ability onto the target.<para></para> Changes the target's ability to match the user's.  This effect ends when the target leaves battle.
		/// </summary>	
		/// <remarks>
		/// ENTRAINMENT
		/// </remarks>	
		x12C = 300,
		/// <summary>
		/// Makes the target act next this turn.<para></para> The target will act next this turn, regardless of [Speed]{mechanic:speed} or move priority.
		/// If the target has already acted this turn, this move will [fail]{mechanic:fail}.
		/// </summary>
		/// <remarks>
		/// AFTER-YOU
		/// </remarks>	
		x12D = 301,
		/// <summary>
		/// Has double power if it's used more than once per turn.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  If <seealso cref="Moves.ROUND"/> has already been used this turn, this move's power is doubled.  After this move is used, any other Pokémon using it this turn will immediately do so (in the order they would otherwise act), regardless of [Speed]{mechanic:speed} or priority.  Pokémon using other moves will then continue to act as usual.
		/// </summary>
		/// <remarks>
		/// ROUND
		/// </remarks>	
		x12E = 302,
		/// <summary>
		/// Power increases by 100% for each consecutive use by any friendly Pokémon, to a maximum of 200.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  If any friendly Pokémon used this move earlier this turn or on the previous turn, that use's power is added to this move's power, to a maximum of 200.
		/// </summary>	
		/// <remarks>
		/// ECHOED-VOICE
		/// </remarks>	
		x12F = 303,
		/// <summary>
		/// Ignores the target's stat modifiers.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Damage calculation ignores the target's [stat modifiers]{mechanic:stat-modifiers}, including [evasion]{mechanic:evasion}.
		/// </summary>	
		/// <remarks>
		/// CHIP-AWAY,SACRED-SWORD,DARKEST-LARIAT
		/// </remarks>	
		x130 = 304,
		/// <summary>
		/// Removes all of the target's stat modifiers.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  All of the target's [stat modifiers]{mechanic:stat-modifiers} are reset to zero.
		/// </summary>	
		/// <remarks>
		/// CLEAR-SMOG
		/// </remarks>	
		x131 = 305,
		/// <summary>
		/// Power is higher the more the user's stats have been raised, to a maximum of 31×.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Power is increased by 100% its original value for every [stage]{mechanic:stage} any of the user's stats have been raised.  [Accuracy]{mechanic:accuracy}, [evasion]{mechanic:evasion}, and lowered stats do not affect this move's power.  For a Pokémon with all five stats modified to +6, this move's power is 31×.
		/// </summary>	
		/// <remarks>
		/// STORED-POWER,POWER-TRIP
		/// </remarks>	
		x132 = 306,
		/// <summary>
		/// Prevents any priority moves from hitting friendly Pokémon this turn.<para></para> Moves with priority greater than 0 will not hit friendly Pokémon for the remainder of this turn.  If the user is last to act this turn, this move will [fail]{mechanic:fail}.
		/// 
		/// This move cannot be selected by <seealso cref="Moves.ASSIST"/> or <seealso cref="Moves.METRONOME"/>.
		/// </summary>
		/// <remarks>
		/// QUICK-GUARD
		/// </remarks>	
		x133 = 307,
		/// <summary>
		/// User switches places with the friendly Pokémon opposite it.<para></para> User switches position on the field with the friendly Pokémon opposite it.  If the user is in the middle position in a triple battle, or there are no other friendly Pokémon, this move will [fail]{mechanic:fail}.
		/// </summary>	
		/// <remarks>
		/// ALLY-SWITCH
		/// </remarks>	
		x134 = 308,
		/// <summary>
		/// Raises user's Attack, Special Attack, and Speed by two stages.  Lower user's Defense and Special Defense by one stage.<para></para> Raises the user's [Attack]{mechanic:attack}, [Special Attack]{mechanic:special-attack}, and [Speed]{mechanic:speed} by two [stages]{mechanic:stage} each.  Lowers the user's [Defense]{mechanic:defense} and [Special Defense]{mechanic:special-defense} by one <seealso cref="Mechanics.STAGE"/> each.
		/// </summary>
		/// <remarks>
		/// SHELL-SMASH
		/// </remarks>	
		x135 = 309,
		/// <summary>
		/// Heals the target for half its max HP.<para></para> Heals the target for half its max [HP]{mechanic:hp}.
		/// </summary>	
		/// <remarks>
		/// HEAL-PULSE
		/// </remarks>	
		x136 = 310,
		/// <summary>
		/// Has double power if the target has a major status ailment.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  If the target has a [major status ailment]{mechanic:major-status-ailment}, this move has double power.
		/// </summary>	
		/// <remarks>
		/// HEX
		/// </remarks>	
		x137 = 311,
		/// <summary>
		/// Carries the target high into the air, dodging all attacks against either, and drops it next turn.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  User carries the target high into the air for one turn, during which no moves will hit either Pokémon and neither can act.  On the following turn, the user drops the target, inflicting damage and ending the effect.
		/// 
		/// If the target is <seealso cref="Types.FLYING"/>-type, this move will function as normal but inflict no damage.
		/// 
		/// <seealso cref="Moves.GUST"/>, <seealso cref="Moves.HURRICANE"/>, <seealso cref="Moves.SKY_UPPERCUT"/>, <seealso cref="Moves.SMACK_DOWN"/>, <seealso cref="Moves.THUNDER"/>, <seealso cref="Moves.TWISTER"/>, and <seealso cref="Moves.WHIRLWIND"/> can hit both the user and the target during this effect.  <seealso cref="Moves.GUST"/> and <seealso cref="Moves.TWISTER"/> will additionally have double power.
		/// 
		/// The damage from <seealso cref="Moves.HAIL"/> and <seealso cref="Moves.SANDSTORM"/> still applies during this effect.
		/// 
		/// Either Pokémon may be hit during this effect if also under the effect of <seealso cref="Moves.LOCK_ON"/>, <seealso cref="Moves.MIND_READER"/>, or <seealso cref="Abilities.NO_GUARD"/>.
		/// 
		/// This move cannot be used while <seealso cref="Moves.GRAVITY"/> is in effect.
		/// 
		/// This move cannot be selected by <seealso cref="Moves.SLEEP_TALK"/>.
		/// 
		/// *Bug*: If <seealso cref="Moves.GRAVITY"/> is used during a double or triple battle while this move is in effect, this move is not correctly canceled on the target, and it remains high in the air indefinitely.  As this move prevents the target from acting, the only way to subsequently remove it from the field is to faint it.
		/// </summary>
		/// <remarks>
		/// SKY-DROP
		/// </remarks>	
		x138 = 312,
		/// <summary>
		/// Raises the user's Attack by one stage and its Speed by two stages.<para></para> Raises the user's [Attack]{mechanic:attack} by one [stage]{mechanic:stage} and its [Speed]{mechanic:speed} by two stages.
		/// </summary>	
		/// <remarks>
		/// SHIFT-GEAR
		/// </remarks>	
		x139 = 313,
		/// <summary>
		/// Ends wild battles.  Forces trainers to switch Pokémon.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>, then [switches]{mechanic:switch-out} the target out for another of its trainer's Pokémon, selected at random.
		/// 
		/// If the target is under the effect of <seealso cref="Moves.INGRAIN"/> or <seealso cref="Abilities.SUCTION_CUPS"/>, or it has a <seealso cref="Moves.SUBSTITUTE"/>, or its Trainer has no more usable Pokémon, it will not be switched out.  If the target is a wild Pokémon, the battle ends instead.
		/// </summary>
		/// <remarks>
		/// CIRCLE-THROW,DRAGON-TAIL
		/// </remarks>	
		x13A = 314,
		/// <summary>
		/// Destroys the target's held berry.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  If the target is [holding]{mechanic:held-item} a [berry]{mechanic:berry}, it's destroyed and cannot be used in response to this move.
		/// </summary>	
		/// <remarks>
		/// INCINERATE
		/// </remarks>	
		x13B = 315,
		/// <summary>
		/// Makes the target act last this turn.<para></para> Forces the target to act last this turn, regardless of [Speed]{mechanic:speed} or move [priority]{mechanic:priority}.  If the target has already acted this turn, this move will [fail]{mechanic:fail}.
		/// </summary>	
		/// <remarks>
		/// QUASH
		/// </remarks>	
		x13C = 316,
		/// <summary>
		/// Raises the user's Attack and Special Attack by one stage.<para></para> Raises the user's [Attack]{mechanic:attack} and [Special Attack]{mechanic:special-attack} by one [stage]{mechanic:stage} each.  During <seealso cref="Moves.SUNNY_DAY"/>, raises both stats by two stages.
		/// </summary>
		/// <remarks>
		/// GROWTH
		/// </remarks>	
		x13D = 317,
		/// <summary>
		/// Has double power if the user has no held item.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  If the user has no [held item]{mechanic:held-item}, this move has double power.
		/// </summary>	
		/// <remarks>
		/// ACROBATICS
		/// </remarks>	
		x13E = 318,
		/// <summary>
		/// User becomes the target's type.<para></para> User's type changes to match the target's.
		/// </summary>	
		/// <remarks>
		/// REFLECT-TYPE
		/// </remarks>	
		x13F = 319,
		/// <summary>
		/// Has double power if a friendly Pokémon fainted last turn.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  If a friendly Pokémon fainted on the previous turn, this move has double power.
		/// </summary>	
		/// <remarks>
		/// RETALIATE
		/// </remarks>	
		x140 = 320,
		/// <summary>
		/// Inflicts damage equal to the user's remaining HP.  User faints.<para></para> Inflicts damage equal to the user's remaining [HP]{mechanic:hp}.  User faints.
		/// </summary>	
		/// <remarks>
		/// FINAL-GAMBIT
		/// </remarks>	
		x141 = 321,
		/// <summary>
		/// Raises the user's Special Attack by three stages.<para></para> Raises the user's [Special Attack]{mechanic:special-attack} by three [stages]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// TAIL-GLOW
		/// </remarks>	
		x142 = 322,
		/// <summary>
		/// Raises the user's Attack, Defense, and accuracy by one stage each.<para></para> Raises the user's [Attack]{mechanic:attack}, [Defense]{mechanic:defense}, and [accuracy]{mechanic:accuracy} by one [stage]{mechanic:stage} each.
		/// </summary>	
		/// <remarks>
		/// COIL
		/// </remarks>	
		x143 = 323,
		/// <summary>
		/// Gives the user's held item to the target.<para></para> Transfers the user's [held item]{mechanic:held-item} to the target.  If the user has no held item, or the target already has a held item, this move will [fail]{mechanic:fail}.
		/// </summary>	
		/// <remarks>
		/// BESTOW
		/// </remarks>	
		x144 = 324,
		/// <summary>
		/// With <seealso cref="Moves.GRASS_PLEDGE"/>, halves opposing Pokémon's Speed for four turns.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  If a friendly Pokémon used <seealso cref="Moves.GRASS_PLEDGE"/> earlier this turn, all opposing Pokémon have halved [Speed]{mechanic:speed} for four turns (including this one).
		/// </summary>
		/// <remarks>
		/// WATER-PLEDGE
		/// </remarks>	
		x145 = 325,
		/// <summary>
		/// With <seealso cref="Moves.WATER_PLEDGE"/>, doubles the effect chance of friendly Pokémon's moves for four turns.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  If a friendly Pokémon used <seealso cref="Moves.WATER_PLEDGE"/> earlier this turn, moves used by any friendly Pokémon have doubled effect chance for four turns (including this one).
		/// </summary>
		/// <remarks>
		/// FIRE-PLEDGE
		/// </remarks>	
		x146 = 326,
		/// <summary>
		/// With <seealso cref="Moves.FIRE_PLEDGE"/>, damages opposing Pokémon for 1/8 their max HP every turn for four turns.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  If a friendly Pokémon used <seealso cref="Moves.FIRE_PLEDGE"/> earlier this turn, all opposing Pokémon will take 1/8 their max [HP]{mechanic:hp} in damage at the end of every turn for four turns (including this one).
		/// </summary>
		/// <remarks>
		/// GRASS-PLEDGE
		/// </remarks>	
		x147 = 327,
		/// <summary>
		/// Raises the user's Attack and Special Attack by one stage each.<para></para> Raises the user's [Attack]{mechanic:attack} and [Special Attack]{mechanic:special-attack} by one [stage]{mechanic:stage} each.
		/// </summary>	
		/// <remarks>
		/// WORK-UP
		/// </remarks>	
		x148 = 328,
		/// <summary>
		/// Raises the user's Defense by three stages.<para></para> Raises the user's [Defense]{mechanic:defense} by three [stages]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// COTTON-GUARD
		/// </remarks>	
		x149 = 329,
		/// <summary>
		/// Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to put the target to sleep.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to put the target to <see cref="Attack.MoveMetaAilments.SLEEP"/>.
		/// If the user is a <seealso cref="Pokemons.MELOETTA"/>, it will toggle between Aria and Pirouette Forme.
		/// </summary>
		/// <remarks>
		/// RELIC-SONG
		/// </remarks>	
		x14A = 330,
		/// <summary>
		/// Lowers the target's Speed by one stage.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Lowers the target's [Speed]{mechanic:speed} by one [stage]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// GLACIATE
		/// </remarks>	
		x14B = 331,
		/// <summary>
		/// Requires a turn to charge before attacking.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to paralyze the target.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to <see cref="Attack.MoveMetaAilments.PARALYSIS"/> the target.  User charges for one turn before attacking.
		/// </summary>	
		/// <remarks>
		/// FREEZE-SHOCK
		/// </remarks>	
		x14C = 332,
		/// <summary>
		/// Requires a turn to charge before attacking.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to <see cref="Attack.MoveMetaAilments.BURN"/> the target.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to <see cref="Attack.MoveMetaAilments.BURN"/> the target.  User charges for one turn before attacking.
		/// </summary>	
		/// <remarks>
		/// ICE-BURN
		/// </remarks>	
		x14D = 333,
		/// <summary>
		/// Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to confuse the target.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to [confuse]{mechanic:confuse} the target.
		/// 
		/// This move can hit Pokémon under the effect of <seealso cref="Moves.BOUNCE"/>, <seealso cref="Moves.FLY"/>, or <seealso cref="Moves.SKY_DROP"/>.
		/// 
		/// During <seealso cref="Moves.RAIN_DANCE"/>, this move has 100% accuracy.  During <seealso cref="Moves.SUNNY_DAY"/>, this move has 50% accuracy.
		/// </summary>
		/// <remarks>
		/// HURRICANE
		/// </remarks>	
		x14E = 334,
		/// <summary>
		/// Lowers the user's Defense, Special Defense, and Speed by one stage each.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Lowers the user's [Defense]{mechanic:defense}, [Special Defense]{mechanic:special-defense}, and [Speed]{mechanic:speed} by one [stage]{mechanic:stage} each.
		/// </summary>	
		/// <remarks>
		/// V-CREATE
		/// </remarks>	
		x14F = 335,
		/// <summary>
		/// With <seealso cref="Moves.FUSION_BOLT"/>, inflicts double damage.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  If a friendly Pokémon used <seealso cref="Moves.FUSION_BOLT"/> earlier this turn, this move has double power.
		/// </summary>
		/// <remarks>
		/// FUSION-FLARE
		/// </remarks>	
		x150 = 336,
		/// <summary>
		/// With <seealso cref="Moves.FUSION_FLARE"/>, inflicts double damage.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  If a friendly Pokémon used <seealso cref="Moves.FUSION_FLARE"/> earlier this turn, this move has double power.
		/// </summary>
		/// <remarks>
		/// FUSION-BOLT
		/// </remarks>	
		x151 = 337,
		/// <summary>
		/// Deals both <seealso cref="Types.FIGHTING"/> and <seealso cref="Types.FLYING"/>-type damage.<para></para> Inflicts regular damage.  For the purposes of type effectiveness, this move is both <seealso cref="Types.FIGHTING"/>- and <seealso cref="Types.FLYING"/>-type: its final effectiveness is determined by multiplying the effectiveness of each type against each of the target's types.
		/// 
		/// For all other purposes, this move is pure Fighting-type.  If this move's type is changed, its Fighting typing is overwritten, and its secondary type remains Flying.
		/// 
		/// If the target has used <seealso cref="Moves.MINIMIZE"/> since entering battle, this move has double power and will never miss.
		/// </summary>
		/// <remarks>
		/// FLYING-PRESS
		/// </remarks>	
		x152 = 338,
		/// <summary>
		/// Can only be used after the user has eaten a berry.<para></para> Inflicts regular damage.  Can only be used if the user has eaten a berry since the beginning of the battle.
		/// 
		/// After the user eats a berry, it may use this move any number of times until the end of the battle, even if it switches out.  Eating a held berry, eating a berry via <seealso cref="Moves.BUG_BITE"/> or <seealso cref="Moves.PLUCK"/>, or being the target of a [Flung]<seealso cref="Moves.FLING"/> berry will enable this move.  Feeding a Pokémon a berry from the bag or using <seealso cref="Moves.NATURAL_GIFT"/> will not.
		/// 
		/// If the trainer chooses this move when it cannot be used, the choice is rejected outright and the trainer must choose another move.
		/// </summary>
		/// <remarks>
		/// BELCH
		/// </remarks>	
		x153 = 339,
		/// <summary>
		/// Raises the Attack and Special Attack of all <seealso cref="Types.GRASS"/> Pokémon in battle.<para></para> Raises the Attack and Special Attack of all <seealso cref="Types.GRASS"/> Pokémon in battle.
		/// </summary>
		/// <remarks>
		/// ROTOTILLER
		/// </remarks>	
		x154 = 340,
		/// <summary>
		/// Covers the opposing field, lowering opponents' Speed by one stage upon switching in.<para></para> Shoots a web over the opponents' side of the field, which lowers the Speed of any opposing Pokémon that enters the field by one stage.
		/// 
		/// Pokémon in the air, such as <seealso cref="Types.FLYING"/>-types and those with <seealso cref="Abilities.LEVITATE"/>, are unaffected.  <seealso cref="Moves.RAPID_SPIN"/> removes Sticky Web from the user's side of the field; <seealso cref="Moves.DEFOG"/> removes it from both sides.
		/// </summary>
		/// <remarks>
		/// STICKY-WEB
		/// </remarks>	
		x155 = 341,
		/// <summary>
		/// Raises the user's Attack by two stages if it KOs the target.<para></para> Inflicts regular damage.  Raises the user's Attack by two stages if it KOs the target.
		/// </summary>	
		/// <remarks>
		/// FELL-STINGER
		/// </remarks>	
		x156 = 342,
		/// <summary>
		/// Adds <seealso cref="Types.GHOST"/> to the target's types.<para></para> Adds <seealso cref="Types.GHOST"/> to the target's types.
		/// </summary>
		/// <remarks>
		/// TRICK-OR-TREAT
		/// </remarks>	
		x157 = 343,
		/// <summary>
		/// Lowers the target's Attack and Special Attack by one stage.<para></para> Lowers the target's Attack and Special Attack by one stage.
		/// </summary>	
		/// <remarks>
		/// NOBLE-ROAR
		/// </remarks>	
		x158 = 344,
		/// <summary>
		/// Changes all <seealso cref="Types.NORMAL"/> moves to <seealso cref="Types.ELECTRIC"/> moves for the rest of the turn.<para></para> Changes all Pokémon's <seealso cref="Types.NORMAL"/> moves to <seealso cref="Types.ELECTRIC"/> moves for the rest of the turn.
		/// </summary>
		/// <remarks>
		/// ION-DELUGE
		/// </remarks>	
		x159 = 345,
		/// <summary>
		/// Heals the user for half the total damage dealt to all targets.<para></para> Heals the user for half the total damage dealt to all targets.
		/// </summary>	
		/// <remarks>
		/// PARABOLIC-CHARGE
		/// </remarks>	
		x15A = 346,
		/// <summary>
		/// Lowers all targets' Attack and Special Attack by one stage.  Makes the user switch out.<para></para> Lowers all targets' Attack and Special Attack by one stage.  Makes the user switch out.
		/// </summary>	
		/// <remarks>
		/// PARTING-SHOT
		/// </remarks>	
		x15B = 347,
		/// <summary>
		/// Inverts the target's stat modifiers.<para></para> Inverts the target's stat modifiers.
		/// </summary>	
		/// <remarks>
		/// TOPSY-TURVY
		/// </remarks>	
		x15C = 348,
		/// <summary>
		/// Drains 75% of the damage inflicted to heal the user.<para></para> Deals regular damage.  Drains 75% of the damage inflicted to heal the user.
		/// </summary>	
		/// <remarks>
		/// DRAINING-KISS,OBLIVION-WING
		/// </remarks>	
		x15D = 349,
		/// <summary>
		/// Protects all friendly Pokémon from non-damaging moves.<para></para> Protects all friendly Pokémon from non-damaging moves for the rest of the turn.
		/// 
		/// Unlike other blocking moves, this move may be used consecutively without its chance of success falling.
		/// </summary>
		/// <remarks>
		/// CRAFTY-SHIELD
		/// </remarks>	
		x15E = 350,
		/// <summary>
		/// Raises the Defense of all <seealso cref="Types.GRASS"/> Pokémon in battle.<para></para> Raises the Defense of all <seealso cref="Types.GRASS"/> Pokémon in battle.
		/// </summary>
		/// <remarks>
		/// FLOWER-SHIELD
		/// </remarks>	
		x15F = 351,
		/// <summary>
		/// For five turns, heals all Pokémon on the ground for 1/16 max HP each turn and strengthens their <seealso cref="Types.GRASS"/> moves to 1.5× their power.<para></para> For five turns, heals all Pokémon on the ground for 1/16 their max HP each turn and strengthens their <seealso cref="Types.GRASS"/> moves to 1.5× their power.
		/// 
		/// Changes <seealso cref="Moves.NATURE_POWER"/> to <seealso cref="Moves.ENERGY_BALL"/>.
		/// </summary>
		/// <remarks>
		/// GRASSY-TERRAIN
		/// </remarks>	
		x160 = 352,
		/// <summary>
		/// For five turns, protects all Pokémon on the ground from major status ailments and confusion, and halves the power of incoming <seealso cref="Types.DRAGON"/> moves.<para></para> For five turns, protects all Pokémon on the ground from major status ailments and confusion and weakens <seealso cref="Types.DRAGON"/> moves used against them to 0.5× their power.
		/// 
		/// Changes <seealso cref="Moves.NATURE_POWER"/> to <seealso cref="Moves.MOONBLAST"/>.
		/// </summary>
		/// <remarks>
		/// MISTY-TERRAIN
		/// </remarks>	
		x161 = 353,
		/// <summary>
		/// Changes the target's move's type to <seealso cref="Types.ELECTRIC"/> if it hasn't moved yet this turn.<para></para> Changes the target's move's type to <seealso cref="Types.ELECTRIC"/> if it hasn't moved yet this turn.
		/// </summary>
		/// <remarks>
		/// ELECTRIFY
		/// </remarks>	
		x162 = 354,
		/// <summary>
		/// Prevents all Pokémon from fleeing or switching out during the next turn.<para></para> Prevents all Pokémon from fleeing or switching out during the next turn.
		/// </summary>	
		/// <remarks>
		/// FAIRY-LOCK
		/// </remarks>	
		x163 = 355,
		/// <summary>
		/// Blocks damaging attacks and lowers attacking Pokémon's Attack by two stages on contact.  Switches Aegislash to Shield Forme.<para></para> Blocks damaging attacks and lowers attacking Pokémon's Attack by two stages on contact.  Switches Aegislash to Shield Forme.
		/// </summary>	
		/// <remarks>
		/// KINGS-SHIELD
		/// </remarks>	
		x164 = 356,
		/// <summary>
		/// Lowers the target's Attack by one stage.<para></para> Lowers the target's [Attack]{mechanic:attack} by one [stage]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// PLAY-NICE
		/// </remarks>	
		x165 = 357,
		/// <summary>
		/// Lowers the target's Special Attack by one stage.<para></para> Lowers the target's Special Attack by one stage.
		/// </summary>	
		/// <remarks>
		/// CONFIDE
		/// </remarks>	
		x166 = 358,
		/// <summary>
		/// Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to raise the user's Defense by two stages for each target hit.<para></para> Inflicts regular damage.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to raise the user's Defense by two stages for each target hit.
		/// </summary>	
		/// <remarks>
		/// DIAMOND-STORM
		/// </remarks>	
		x167 = 359,
		/// <summary>
		/// Ignores and destroys protection effects.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Bypasses and removes any protection effect on the target.
		/// </summary>	
		/// <remarks>
		/// HYPERSPACE-HOLE,HYPERSPACE-FURY
		/// </remarks>	
		x168 = 360,
		/// <summary>
		/// Hits 2–5 times.<para></para> Inflicts regular damage.  Hits 2–5 times.
		/// </summary>	
		/// <remarks>
		/// WATER-SHURIKEN
		/// </remarks>	
		x169 = 361,
		/// <summary>
		/// Blocks damaging attacks and damages attacking Pokémon for 1/8 their max HP.<para></para> Blocks damaging attacks and damages attacking Pokémon for 1/8 their max HP.
		/// </summary>	
		/// <remarks>
		/// SPIKY-SHIELD
		/// </remarks>	
		x16A = 362,
		/// <summary>
		/// Raises a selected ally's Special Defense by one stage.<para></para> Raises a selected ally's Special Defense by one stage.
		/// </summary>	
		/// <remarks>
		/// AROMATIC-MIST
		/// </remarks>	
		x16B = 363,
		/// <summary>
		/// Lowers the target's Attack, Special Attack, and Speed by one stage if it is poisoned.<para></para> Lowers the target's Attack, Special Attack, and Speed by one stage if it is poisoned.
		/// </summary>	
		/// <remarks>
		/// VENOM-DRENCH
		/// </remarks>	
		x16C = 364,
		/// <summary>
		/// Lowers the target's Attack by one stage.<para></para> Lowers the target's Attack by one stage.
		/// </summary>	
		/// <remarks>
		/// BABY-DOLL-EYES
		/// </remarks>	
		x16D = 365,
		/// <summary>
		/// Takes one turn to charge, then raises the user's Special Attack, Special Defense, and Speed by two stages.<para></para> Takes one turn to charge, then raises the user's Special Attack, Special Defense, and Speed by two stages.
		/// </summary>	
		/// <remarks>
		/// GEOMANCY
		/// </remarks>	
		x16E = 366,
		/// <summary>
		/// Raises the Defense and Special Defense of all friendly Pokémon with <seealso cref="Abilities.PLUS"/> or <seealso cref="Abilities.MINUS"/> by one stage.<para></para> Raises the Defense and Special Defense of all friendly Pokémon with <seealso cref="Abilities.PLUS"/> or <seealso cref="Abilities.MINUS"/> by one stage.
		/// </summary>
		/// <remarks>
		/// MAGNETIC-FLUX
		/// </remarks>	
		x16F = 367,
		/// <summary>
		/// Doubles prize money.<para></para> Doubles prize money.
		/// 
		/// Stacks with a held item.  Only works once per battle.
		/// </summary>
		/// <remarks>
		/// HAPPY-HOUR
		/// </remarks>	
		x170 = 368,
		/// <summary>
		/// For five turns, prevents all Pokémon on the ground from sleeping and strengthens their <seealso cref="Types.ELECTRIC"/> moves to 1.5× their power.<para></para> For five turns, prevents all Pokémon on the ground from sleeping and strengthens their <seealso cref="Types.ELECTRIC"/> moves to 1.5× their power.
		/// 
		/// Changes <seealso cref="Moves.NATURE_POWER"/> to <seealso cref="Moves.THUNDERBOLT"/>.
		/// </summary>
		/// <remarks>
		/// ELECTRIC-TERRAIN
		/// </remarks>	
		x171 = 369,
		/// <summary>
		/// Does nothing.<para></para> Does nothing.
		/// </summary>	
		/// <remarks>
		/// CELEBRATE
		/// </remarks>	
		x172 = 370,
		/// <summary>
		/// Does nothing.<para></para> Does nothing.
		/// </summary>	
		/// <remarks>
		/// HOLD-HANDS
		/// </remarks>	
		x173 = 371,
		/// <summary>
		/// Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to paralyze the target.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Has a <see cref="Attack.Data.MetaData.AilmentChance"/> chance to <see cref="Attack.MoveMetaAilments.PARALYSIS"/> the target.
		/// </summary>	
		/// <remarks>
		/// NUZZLE
		/// </remarks>	
		x174 = 372,
		/// <summary>
		/// Grounds the target, and hits even Flying-type or levitating Pokémon.<para></para> Inflicts regular damage.  Grounds the target until it leaves battle.  Ignores levitation effects and the immunity of <seealso cref="Types.FLYING"/>-type Pokémon.
		/// </summary>
		/// <remarks>
		/// THOUSAND-ARROWS
		/// </remarks>	
		x175 = 373,
		/// <summary>
		/// Prevents the target from leaving battle.<para></para> Inflicts regular damage.  Traps the target.
		/// </summary>	
		/// <remarks>
		/// THOUSAND-WAVES
		/// </remarks>	
		x176 = 374,
		/// <summary>
		/// Raises the user's Attack by one stage after inflicting damage.<para></para> Inflicts regular damage.  Raises the user's Attack by one stage.
		/// </summary>	
		/// <remarks>
		/// POWER-UP-PUNCH
		/// </remarks>	
		x177 = 375,
		/// <summary>
		/// Adds <seealso cref="Types.GRASS"/> to the target's types.<para></para> Adds <seealso cref="Types.GRASS"/> to the target's types.
		/// </summary>
		/// <remarks>
		/// FORESTS-CURSE
		/// </remarks>	
		x178 = 376,
		/// <summary>
		/// Protects all friendly Pokémon from damaging moves.  Only works on the first turn after the user is sent out.<para></para> Protects all friendly Pokémon from damaging moves.  Only works on the first turn after the user is sent out.
		/// </summary>	
		/// <remarks>
		/// MAT-BLOCK
		/// </remarks>	
		x179 = 377,
		/// <summary>
		/// Explodes if the target uses a <seealso cref="Types.FIRE"/> move this turn, damaging it for 1/4 its max HP and preventing the move.<para></para> Explodes if the target uses a <seealso cref="Types.FIRE"/> move this turn, damaging it for 1/4 its max HP and preventing the move.
		/// </summary>
		/// <remarks>
		/// POWDER
		/// </remarks>	
		x17A = 378,
		/// <summary>
		/// Inflicts regular damage.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.
		/// </summary>	
		/// <remarks>
		/// PETAL-BLIZZARD,BOOMBURST
		/// </remarks>	
		x17B = 379,
		/// <summary>
		/// Super-effective against <seealso cref="Types.WATER"/>.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  This move is super-effective against the <seealso cref="Types.WATER"/> type.
		/// 
		/// The target's other type will affect damage as usual.  If this move's type is changed, it remains super-effective against Water regardless of its type.
		/// </summary>
		/// <remarks>
		/// FREEZE-DRY
		/// </remarks>	
		x17C = 380,
		/// <summary>
		/// Never misses.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Ignores [accuracy]{mechanic:accuracy} and [evasion]{mechanic:evasion}.
		/// </summary>	
		/// <remarks>
		/// DISARMING-VOICE
		/// </remarks>	
		x17D = 381,
		/// <summary>
		/// Heals the user for ½ its max HP, or ⅔ during a sandstorm.<para></para> Heals the user for ½ its max HP.  During a <seealso cref="Moves.SANDSTORM"/>, the healing is increased to ⅔.
		/// </summary>
		/// <remarks>
		/// SHORE-UP
		/// </remarks>	
		x17E = 382,
		/// <summary>
		/// Can only be used as the first move after the user enters battle.<para></para> Inflicts regular damage. Can only be used on the user's first turn after entering the field.
		/// </summary>	
		/// <remarks>
		/// FIRST-IMPRESSION
		/// </remarks>	
		x17F = 383,
		/// <summary>
		/// Grants the user protection for the rest of the turn and poisons any Pokémon that tries to use a contact move on it.<para></para> Grants the user protection for the rest of the turn.  If a Pokémon attempts to use a move that makes contact with the user, that Pokémon will be poisoned.  This move's chance of success halves every time it's used consecutively with any other protection move.
		/// </summary>	
		/// <remarks>
		/// BANEFUL-BUNKER
		/// </remarks>	
		x180 = 384,
		/// <summary>
		/// Traps the target.<para></para> Inflicts regular damage.  Traps the target.
		/// </summary>	
		/// <remarks>
		/// SPIRIT-SHACKLE,ANCHOR-SHOT
		/// </remarks>	
		x181 = 385,
		/// <summary>
		/// Cures the target of burns.<para></para> Inflicts regular damage.  If the target has a burn and takes damage from this move, its burn is healed.
		/// </summary>	
		/// <remarks>
		/// SPARKLING-ARIA
		/// </remarks>	
		x182 = 386,
		/// <summary>
		/// Heals the target for ½ its max HP, or ⅔ on Grassy Terrain.<para></para> Heals the target for ½ its max HP.  If <seealso cref="Moves.GRASSY_TERRAIN"/> is in effect, heals for ⅔ instead.
		/// </summary>
		/// <remarks>
		/// FLORAL-HEALING
		/// </remarks>	
		x183 = 387,
		/// <summary>
		/// Heals the user by the target's current Attack stat and lowers the target's Attack by one stage.<para></para> Lowers the target's Attack by one stage.  Heals the user by the target's current Attack, including modifiers, but not including this move's Attack-lowering effect.
		/// 
		/// If the target's Attack is already at -6, this move will fail.  In any other situation that would prevent a stat modification, the healing will still succeed.
		/// </summary>
		/// <remarks>
		/// STRENGTH-SAP
		/// </remarks>	
		x184 = 388,
		/// <summary>
		/// Forces the target's opponents to aim at the target for the rest of the turn.<para></para> For the duration of the turn, if the target is also a valid target for a move used by one of its opponents, that move will target it instead.
		/// </summary>	
		/// <remarks>
		/// SPOTLIGHT
		/// </remarks>	
		x185 = 389,
		/// <summary>
		/// Poisons the target and lowers its Speed by one stage.<para></para> Poisons the target and lowers its Speed by one stage.
		/// </summary>	
		/// <remarks>
		/// TOXIC-THREAD
		/// </remarks>	
		x186 = 390,
		/// <summary>
		/// Guarantees a critical hit with the user's next move.<para></para> The user's next move will result in a [critical hit]{mechanic:critical-hit}.
		/// </summary>	
		/// <remarks>
		/// LASER-FOCUS
		/// </remarks>	
		x187 = 391,
		/// <summary>
		/// Raises the Attack and Special Attack of all friendly Pokémon with <seealso cref="Abilities.PLUS"/> or <seealso cref="Abilities.MINUS"/> by one stage.<para></para> Raises the Attack and Special Attack of all friendly Pokémon with <seealso cref="Abilities.PLUS"/> or <seealso cref="Abilities.MINUS"/> by one stage.
		/// </summary>
		/// <remarks>
		/// GEAR-UP
		/// </remarks>	
		x188 = 392,
		/// <summary>
		/// Prevents the target from using sound-based moves for two turns.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Silences the target for two turns, preventing it from using any sound-based moves.
		/// </summary>	
		/// <remarks>
		/// THROAT-CHOP
		/// </remarks>	
		x189 = 393,
		/// <summary>
		/// Damages opponents, but heals allies for 50% of their max HP.<para></para> If the target is an opponent, inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  If the target is an ally, heals the target for 50% of its max HP.
		/// </summary>	
		/// <remarks>
		/// POLLEN-PUFF
		/// </remarks>	
		x18A = 394,
		/// <summary>
		/// Protects Pokémon on the ground from priority moves and increases the power of their  Psychic moves by 50%.<para></para> Changes the terrain to Psychic Terrain for 5 turns.  Overrides <seealso cref="Moves.ELECTRIC_TERRAIN"/>, <seealso cref="Moves.GRASSY_TERRAIN"/>, and <seealso cref="Moves.MISTY_TERRAIN"/>.
		/// 
		/// All Pokémon on the ground are immune to moves with priority greater than 0.  (Moves that target the field rather than individual Pokémon, such as <seealso cref="Moves.SPIKES"/>, are not affected.)  Additionally, when a Pokémon on the ground uses a <seealso cref="Types.PSYCHIC"/>-type move, that move's power is increased to 1.5×.
		/// 
		/// If a Pokémon is holding a Terrain Extender when creating Psychic Terrain (by any means), the effect lasts for 8 turns instead of 5.
		/// </summary>
		/// <remarks>
		/// PSYCHIC-TERRAIN
		/// </remarks>	
		x18B = 395,
		/// <summary>
		/// Lowers the target's Attack by one stage after inflicting damage.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Lowers the target's [Attack]{mechanic:attack} by one [stage]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// LUNGE,TROP-KICK
		/// </remarks>	
		x18C = 396,
		/// <summary>
		/// Lowers the target's Defense by one stage after inflicting damage.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Lowers the target's [Defense]{mechanic:defense} by one [stage]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// FIRE-LASH
		/// </remarks>	
		x18D = 397,
		/// <summary>
		/// Removes the user's <seealso cref="Types.FIRE"/> type after inflicting damage.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Removes the user's <seealso cref="Types.FIRE"/> type after damage calculation.  If the user is not <seealso cref="Types.FIRE"/>-type, this move will fail.
		/// </summary>
		/// <remarks>
		/// BURN-UP
		/// </remarks>	
		x18E = 398,
		/// <summary>
		/// Exchanges the user's Speed with the target's.<para></para> Exchanges the original [Speed]{mechanic:speed} stats of the user and target.
		/// </summary>	
		/// <remarks>
		/// SPEED-SWAP
		/// </remarks>	
		x18F = 399,
		/// <summary>
		/// Cures the target of a major status ailment and heals the user for 50% of its max HP.<para></para> Cures the target of its major status ailment and heals the user for 50% of its max HP.  If the target has no major status ailment, this move will fail.
		/// </summary>	
		/// <remarks>
		/// PURIFY
		/// </remarks>	
		x190 = 400,
		/// <summary>
		/// Has the same type as the user.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  This move's type matches the user's first type, if any; otherwise, it's typeless.
		/// </summary>	
		/// <remarks>
		/// REVELATION-DANCE
		/// </remarks>	
		x191 = 401,
		/// <summary>
		/// Nullifies the target's ability if it moves earlier.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  If the target has already moved this turn, its ability is nullified.
		/// </summary>	
		/// <remarks>
		/// CORE-ENFORCER
		/// </remarks>	
		x192 = 402,
		/// <summary>
		/// Forces the target to repeat its last used move.<para></para> The target immediately uses its most recently-used move.  This is independent of the target's normal action for the turn (i.e., it may end up moving twice), but otherwise functions as usual, including deduction of PP.  This effect works for disabled moves and ignores <seealso cref="Moves.TORMENT"/>.
		/// </summary>
		/// <remarks>
		/// INSTRUCT
		/// </remarks>	
		x193 = 403,
		/// <summary>
		/// Inflicts a burn on any Pokémon that makes contact before the attack.<para></para> Begins charging at the start of the turn, then attacks as normal.  Any Pokémon that makes contact with the user while charging is [burned]{mechanic:burns}.  The charging is not affected by accuracy, sleep, paralysis, or any other effect that would interfere with a move.
		/// </summary>	
		/// <remarks>
		/// BEAK-BLAST
		/// </remarks>	
		x194 = 404,
		/// <summary>
		/// Lowers the user's Defense by one stage after inflicting damage.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Lowers the user's [Defense]{mechanic:defense} by one [stage]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// CLANGING-SCALES
		/// </remarks>	
		x195 = 405,
		/// <summary>
		/// Inflicts regular damage with no additional effect.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.
		/// </summary>	
		/// <remarks>
		/// BRUTAL-SWING
		/// </remarks>	
		x196 = 406,
		/// <summary>
		/// Reduces damage five turns, but must be used during hail.<para></para> Places the Aurora Veil effect on the user's side of the field for the next 5 turns.  If the weather is not <seealso cref="Moves.HAIL"/>, or the weather is disabled by the effects of <seealso cref="Abilities.CLOUD_NINE"/> or <seealso cref="Abilities.AIR_LOCK"/>, this move will fail.
		/// 
		/// Any <seealso cref="Attack.MetaCategory.DAMAGE"/> dealt to an affected Pokémon is reduced by ½.  (If there are multiple Pokémon on the affected field, the reduction is ⅓.)
		/// </summary>
		/// <remarks>
		/// AURORA-VEIL
		/// </remarks>	
		x197 = 407,
		/// <summary>
		/// Only inflicts damage if the user was hit by a physical move this turn.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  If the user was not yet hit by an opponent's physical move this turn, this move will fail.
		/// </summary>	
		/// <remarks>
		/// SHELL-TRAP
		/// </remarks>	
		x198 = 408,
		/// <summary>
		/// Has double power if the user's last move failed.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Power is doubled if the user's last move failed for any reason (i.e., produced the message "But it failed!") or was ineffective due to types.
		/// </summary>	
		/// <remarks>
		/// STOMPING-TANTRUM
		/// </remarks>	
		x199 = 409,
		/// <summary>
		/// Steals the target's stat increases, then inflicts damage.<para></para> Steals the target's stat increases, then inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Will not steal stat increases that would put any of the user's stats at more than +6; any excess is left on the target.  Stolen increases are affected by abilities as normal.
		/// </summary>	
		/// <remarks>
		/// SPECTRAL-THIEF
		/// </remarks>	
		x19A = 410,
		/// <summary>
		/// Cannot be disrupted by abilities.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  Other Pokémon's abilities cannot activate in response to this move.  In particular, it hits through <seealso cref="Abilities.DISGUISE"/>
		/// </summary>
		/// <remarks>
		/// SUNSTEEL-STRIKE,MOONGEIST-BEAM,SEARING-SUNRAZE-SMASH,MENACING-MOONRAZE-MAELSTROM
		/// </remarks>	
		x19B = 411,
		/// <summary>
		/// Lowers the target's Attack and Special Attack by one stage.<para></para> Lowers the target's [Attack]{mechanic:attack} and [Special Attack]{mechanic:special-attack} by one stage each.
		/// </summary>	
		/// <remarks>
		/// TEARFUL-LOOK
		/// </remarks>	
		x19C = 412,
		/// <summary>
		/// Damages the target for 75% of its remaining HP.<para></para> Inflicts direct damage equal to ¾ of the target's remaining HP.
		/// </summary>	
		/// <remarks>
		/// GUARDIAN-OF-ALOLA
		/// </remarks>	
		x19D = 413,
		/// <summary>
		/// Raises all of the user's stats by two stages.<para></para> Raises the user's [Attack]{mechanic:attack}, [Defense]{mechanic:defense}, [Special Attack]{mechanic:special-attack}, [Special Defense]{mechanic:special-defense}, and [Speed]{mechanic:speed} by two [stages]{mechanic:stage} each.
		/// </summary>	
		/// <remarks>
		/// EXTREME-EVOBOOST
		/// </remarks>	
		x19E = 414,
		/// <summary>
		/// Changes the terrain to Psychic Terrain after inflicting damage.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>, then (if successful) changes the terrain to <seealso cref="Moves.PSYCHIC_TERRAIN"/>.
		/// </summary>
		/// <remarks>
		/// GENESIS-SUPERNOVA
		/// </remarks>	
		x19F = 415,
		/// <summary>
		/// XXX new effect for light-that-burns-the-sky<para></para> XXX new effect for light-that-burns-the-sky
		/// </summary>	
		/// <remarks>
		/// LIGHT-THAT-BURNS-THE-SKY
		/// </remarks>	
		x1A0 = 416,
		/// <summary>
		/// XXX new effect for lets-snuggle-forever<para></para> XXX new effect for lets-snuggle-forever
		/// </summary>	
		/// <remarks>
		/// LETS-SNUGGLE-FOREVER
		/// </remarks>	
		x1A1 = 417,
		/// <summary>
		/// XXX new effect for splintered-stormshards<para></para> XXX new effect for splintered-stormshards
		/// </summary>	
		/// <remarks>
		/// SPLINTERED-STORMSHARDS
		/// </remarks>	
		x1A2 = 418,
		/// <summary>
		/// XXX new effect for clangorous-soulblaze<para></para> XXX new effect for clangorous-soulblaze
		/// </summary>	
		/// <remarks>
		/// CLANGOROUS-SOULBLAZE
		/// </remarks>	
		x1A3 = 419,
		/// <summary>
		/// XXX new effect for mind-blown<para></para> XXX new effect for mind-blown
		/// </summary>	
		/// <remarks>
		/// MIND-BLOWN
		/// </remarks>	
		x1A4 = 420,
		/// <summary>
		/// Has an increased chance for a critical hit in Hyper Mode.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  User's [critical hit]{mechanic:critical-hit} rate is one level higher when using this move while in [hyper mode]{mechanic:hyper-mode}.
		/// </summary>	
		/// <remarks>
		/// SHADOW-RUSH
		/// </remarks>	
		x711 = 10001,
		/// <summary>
		/// User receives 1/2 its HP in recoil.<para></para> Inflicts <seealso cref="Attack.MetaCategory.DAMAGE"/>.  User takes 1/2 of its current [HP]{mechanic:hp} in recoil.
		/// </summary>	
		/// <remarks>
		/// SHADOW-END
		/// </remarks>	
		x712 = 10002,
		/// <summary>
		/// Halves HP of all Pokémon on the field.  Must recharge<para></para> Halves [HP]{mechanic:hp} of all Pokémon on the field.  User loses its next turn to "recharge", and cannot attack or [switch]{mechanic:switch} out during that turn.
		/// </summary>	
		/// <remarks>
		/// SHADOW-HALF
		/// </remarks>	
		x713 = 10003,
		/// <summary>
		/// Lowers the target's evasion by two stages.<para></para> Lowers the target's [evasion]{mechanic:evasion} by two [stages]{mechanic:stage}.
		/// </summary>	
		/// <remarks>
		/// SHADOW-MIST
		/// </remarks>	
		x714 = 10004,
		/// <summary>
		/// Removes <seealso cref="Moves.LIGHT_SCREEN"/>, <seealso cref="Moves.REFLECT"/>, and <seealso cref="Moves.SAFEGUARD"/>.<para></para> Removes the effects of <seealso cref="Moves.LIGHT_SCREEN"/>, <seealso cref="Moves.REFLECT"/>, and <seealso cref="Moves.SAFEGUARD"/>.
		/// </summary>	
		/// <remarks>
		/// SHADOW-SHED
		/// </remarks>	
		x715 = 10005,
		/// <summary>
		/// Changes the weather to Shadow Sky for five turns.<para></para> Changes the weather to Shadow Sky for five turns.  Pokémon other than []{type:shadow} Pokémon take 1/16 their max [HP]{mechanic:hp} at the end of every turn.  This move is typeless for the purposes of <seealso cref="Moves.WEATHER_BALL"/>.
		/// </summary>	
		/// <remarks>
		/// SHADOW-SKY
		/// </remarks>	
		x716 = 10006
	}
}