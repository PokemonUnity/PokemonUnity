using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//namespace DataEnums{
	public class eAbility{
		/// <summary>
		/// Ability ids are connected to XML file.
		/// </summary>
		/// <remarks>Can now code with strings or int and
		/// access the same value.</remarks>
		public enum Ability{
			STENCH = 1,
			DRIZZLE = 2,
			SPEED_BOOST = 3,
			BATTLE_ARMOR = 4,
			STURDY = 5,
			DAMP = 6,
			LIMBER = 7,
			SAND_VEIL = 8,
			STATIC = 9,
			VOLT_ABSORB = 10,
			WATER_ABSORB = 11,
			OBLIVIOUS = 12,
			CLOUD_NINE = 13,
			COMPOUND_EYES = 14,
			INSOMNIA = 15,
			COLOR_CHANGE = 16,
			IMMUNITY = 17,
			FLASH_FIRE = 18,
			SHIELD_DUST = 19,
			OWN_TEMPO = 20,
			SUCTION_CUPS = 21,
			INTIMIDATE = 22,
			SHADOW_TAG = 23,
			ROUGH_SKIN = 24,
			WONDER_GUARD = 25,
			LEVITATE = 26,
			EFFECT_SPORE = 27,
			SYNCHRONIZE = 28,
			CLEAR_BODY = 29,
			NATURAL_CURE = 30,
			LIGHTNING_ROD = 31,
			SERENE_GRACE = 32,
			SWIFT_SWIM = 33,
			CHLOROPHYLL = 34,
			ILLUMINATE = 35,
			TRACE = 36,
			HUGE_POWER = 37,
			POISON_POINT = 38,
			INNER_FOCUS = 39,
			MAGMA_ARMOR = 40,
			WATER_VEIL = 41,
			MAGNET_PULL = 42,
			SOUNDPROOF = 43,
			RAIN_DISH = 44,
			SAND_STREAM = 45,
			PRESSURE = 46,
			THICK_FAT = 47,
			EARLY_BIRD = 48,
			FLAME_BODY = 49,
			RUN_AWAY = 50,
			KEEN_EYE = 51,
			HYPER_CUTTER = 52,
			PICKUP = 53,
			TRUANT = 54,
			HUSTLE = 55,
			CUTE_CHARM = 56,
			PLUS = 57,
			MINUS = 58,
			FORECAST = 59,
			STICKY_HOLD = 60,
			SHED_SKIN = 61,
			GUTS = 62,
			MARVEL_SCALE = 63,
			LIQUID_OOZE = 64,
			OVERGROW = 65,
			BLAZE = 66,
			TORRENT = 67,
			SWARM = 68,
			ROCK_HEAD = 69,
			DROUGHT = 70,
			ARENA_TRAP = 71,
			VITAL_SPIRIT = 72,
			WHITE_SMOKE = 73,
			PURE_POWER = 74,
			SHELL_ARMOR = 75,
			AIR_LOCK = 76,
			TANGLED_FEET = 77,
			MOTOR_DRIVE = 78,
			RIVALRY = 79,
			STEADFAST = 80,
			SNOW_CLOAK = 81,
			GLUTTONY = 82,
			ANGER_POINT = 83,
			UNBURDEN = 84,
			HEATPROOF = 85,
			SIMPLE = 86,
			DRY_SKIN = 87,
			DOWNLOAD = 88,
			IRON_FIST = 89,
			POISON_HEAL = 90,
			ADAPTABILITY = 91,
			SKILL_LINK = 92,
			HYDRATION = 93,
			SOLAR_POWER = 94,
			QUICK_FEET = 95,
			NORMALIZE = 96,
			SNIPER = 97,
			MAGIC_GUARD = 98,
			NO_GUARD = 99,
			STALL = 100,
			TECHNICIAN = 101,
			LEAF_GUARD = 102,
			KLUTZ = 103,
			MOLD_BREAKER = 104,
			SUPER_LUCK = 105,
			AFTERMATH = 106,
			ANTICIPATION = 107,
			FOREWARN = 108,
			UNAWARE = 109,
			TINTED_LENS = 110,
			FILTER = 111,
			SLOW_START = 112,
			SCRAPPY = 113,
			STORM_DRAIN = 114,
			ICE_BODY = 115,
			SOLID_ROCK = 116,
			SNOW_WARNING = 117,
			HONEY_GATHER = 118,
			FRISK = 119,
			RECKLESS = 120,
			MULTITYPE = 121,
			FLOWER_GIFT = 122,
			BAD_DREAMS = 123,
			PICKPOCKET = 124,
			SHEER_FORCE = 125,
			CONTRARY = 126,
			UNNERVE = 127,
			DEFIANT = 128,
			DEFEATIST = 129,
			CURSED_BODY = 130,
			HEALER = 131,
			FRIEND_GUARD = 132,
			WEAK_ARMOR = 133,
			HEAVY_METAL = 134,
			LIGHT_METAL = 135,
			MULTISCALE = 136,
			TOXIC_BOOST = 137,
			FLARE_BOOST = 138,
			HARVEST = 139,
			TELEPATHY = 140,
			MOODY = 141,
			OVERCOAT = 142,
			POISON_TOUCH = 143,
			REGENERATOR = 144,
			BIG_PECKS = 145,
			SAND_RUSH = 146,
			WONDER_SKIN = 147,
			ANALYTIC = 148,
			ILLUSION = 149,
			IMPOSTER = 150,
			INFILTRATOR = 151,
			MUMMY = 152,
			MOXIE = 153,
			JUSTIFIED = 154,
			RATTLED = 155,
			MAGIC_BOUNCE = 156,
			SAP_SIPPER = 157,
			PRANKSTER = 158,
			SAND_FORCE = 159,
			IRON_BARBS = 160,
			ZEN_MODE = 161,
			VICTORY_STAR = 162,
			TURBOBLAZE = 163,
			TERAVOLT = 164,
			AROMA_VEIL = 165,
			FLOWER_VEIL = 166,
			CHEEK_POUCH = 167,
			PROTEAN = 168,
			FUR_COAT = 169,
			MAGICIAN = 170,
			BULLETPROOF = 171,
			COMPETITIVE = 172,
			STRONG_JAW = 173,
			REFRIGERATE = 174,
			SWEET_VEIL = 175,
			STANCE_CHANGE = 176,
			GALE_WINGS = 177,
			MEGA_LAUNCHER = 178,
			GRASS_PELT = 179,
			SYMBIOSIS = 180,
			TOUGH_CLAWS = 181,
			PIXILATE = 182,
			GOOEY = 183,
			AERILATE = 184,
			PARENTAL_BOND = 185,
			DARK_AURA = 186,
			FAIRY_AURA = 187,
			AURA_BREAK = 188,
			PRIMORDIAL_SEA = 189,
			DESOLATE_LAND = 190,
			DELTA_STREAM = 191
		}
	}

    public class eItems{
        /// <summary>
        /// Item ids are connected to XML file. 
        /// </summary>
        /// <remarks>
        /// Running off of genVI. 
        /// Be sure to overwrite both if modifying.
        /// Replace "[HP]{mechanic:hp}" in Summery-tags with
        /// "<see cref="Pokemon.HP"/>" or "<see cref="Pokemon.currentHP"/>"
        /// </remarks>
        public enum Item
        {
            /// <summery>Used in battle :   [Catches]{mechanic:catch} a wild Pokémon without fail.      If used in a trainer battle, nothing happens and the ball is lost.</summery>			MASTER_BALL = 1,
            /// <summery>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon, using a catch rate of 2×.      If used in a trainer battle, nothing happens and the ball is lost.</summery>			ULTRA_BALL = 2,
            /// <summery>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon, using a catch rate of 1.5×.      If used in a trainer battle, nothing happens and the ball is lost.</summery>			GREAT_BALL = 3,
            /// <summery>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon, using a catch rate of 1×.      If used in a trainer battle, nothing happens and the ball is lost.</summery>			POKE_BALL = 4,
            /// <summery>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon, using a catch rate of 1.5×.  This item can only be used in the []{location:great-marsh} or []{location:kanto-safari-zone}.</summery>			SAFARI_BALL = 5,
            /// <summery>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.  If the wild Pokémon is []{type:water}- or []{type:bug}-type, this ball has a catch rate of 3×.  Otherwise, it has a catch rate of 1×.      If used in a trainer battle, nothing happens and the ball is lost.</summery>			NET_BALL = 6,
            /// <summery>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.  If the wild Pokémon was encountered by surfing or fishing, this ball has a catch rate of 3.5×.  Otherwise, it has a catch rate of 1×.      If used in a trainer battle, nothing happens and the ball is lost.</summery>			DIVE_BALL = 7,
            /// <summery>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.  Has a catch rate of given by `(40 - level) / 10`, where `level` is the wild Pokémon's level, to a maximum of 3.9× for level 1 Pokémon.  If the wild Pokémon's level is higher than 30, this ball has a catch rate of 1×.      If used in a trainer battle, nothing happens and the ball is lost.</summery>			NEST_BALL = 8,
            /// <summery>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.  If the wild Pokémon's species is marked as caught in the trainer's Pokédex, this ball has a catch rate of 3×.  Otherwise, it has a catch rate of 1×.      If used in a trainer battle, nothing happens and the ball is lost.</summery>			REPEAT_BALL = 9,
            /// <summery>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.  Has a catch rate of 1.1× on the first turn of the battle and increases by 0.1× every turn, to a maximum of 4× on turn 30.      If used in a trainer battle, nothing happens and the ball is lost.</summery>			TIMER_BALL = 10,
            /// <summery>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon, using a catch rate of 1×.  Whenever the caught Pokémon's [happiness]{mechanic:happiness} increases, it increases by one extra point.      If used in a trainer battle, nothing happens and the ball is lost.</summery>			LUXURY_BALL = 11,
            /// <summery>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon, using a catch rate of 1×.      If used in a trainer battle, nothing happens and the ball is lost.</summery>			PREMIER_BALL = 12,
            /// <summery>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.  If it's currently nighttime or the wild Pokémon was encountered while walking in a cave, this ball has a catch rate of 3.5×.  Otherwise, it has a catch rate of 1×.      If used in a trainer battle, nothing happens and the ball is lost.</summery>			DUSK_BALL = 13,
            /// <summery>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon, using a catch rate of 1×.  The caught Pokémon's [HP]{mechanic:hp} is immediately restored, [PP]{mechanic:pp} for all its moves is restored, and any [status ailment]{mechanic:status-ailment} is cured.      If used in a trainer battle, nothing happens and the ball is lost.</summery>			HEAL_BALL = 14,
            /// <summery>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon, using a catch rate of 4× on the first turn of a battle, but 1× any other time.      If used in a trainer battle, nothing happens and the ball is lost.</summery>			QUICK_BALL = 15,
            /// <summery>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon, using a catch rate of 1×.      If used in a trainer battle, nothing happens and the ball is lost.</summery>			CHERISH_BALL = 16,
            /// <summery>Used on a friendly Pokémon :   Restores 20 [HP]{mechanic:hp}.</summery>			POTION = 17,
            /// <summery>Used on a party Pokémon :   Cures [poison]{mechanic:poison}.</summery>			ANTIDOTE = 18,
            /// <summery>Used on a party Pokémon :   Cures a [burn]{mechanic:burn}.</summery>			BURN_HEAL = 19,
            /// <summery>Used on a party Pokémon :   Cures [freezing]{mechanic:freezing}.</summery>			ICE_HEAL = 20,
            /// <summery>Used on a party Pokémon :   Cures [sleep]{mechanic:sleep}.</summery>			AWAKENING = 21,
            /// <summery>Used on a party Pokémon :   Cures [paralysis]{mechanic:paralysis}.</summery>			PARALYZE_HEAL = 22,
            /// <summery>Used on a party Pokémon :   Restores [HP]{mechanic:hp} to full and cures any [status ailment]{mechanic:status-ailment} and [confusion]{mechanic:confusion}.</summery>			FULL_RESTORE = 23,
            /// <summery>Used on a party Pokémon :   Restores [HP]{mechanic:hp} to full.</summery>			MAX_POTION = 24,
            /// <summery>Used on a party Pokémon :   Restores 200 [HP]{mechanic:hp}.</summery>			HYPER_POTION = 25,
            /// <summery>Used on a party Pokémon :   Restores 50 [HP]{mechanic:hp}.</summery>			SUPER_POTION = 26,
            /// <summery>Used on a party Pokémon :   Cures any [status ailment]{mechanic:status-ailment} and [confusion]{mechanic:confusion}.</summery>			FULL_HEAL = 27,
            /// <summery>Used on a party Pokémon :   Revives the Pokémon and restores half its [HP]{mechanic:hp}.</summery>			REVIVE = 28,
            /// <summery>Used on a party Pokémon :   Revives the Pokémon and restores its [HP]{mechanic:hp} to full.</summery>			MAX_REVIVE = 29,
            /// <summery>Used on a party Pokémon :   Restores 50 [HP]{mechanic:hp}.</summery>			FRESH_WATER = 30,
            /// <summery>Used on a party Pokémon :   Restores 60 [HP]{mechanic:hp}.</summery>			SODA_POP = 31,
            /// <summery>Used on a party Pokémon :   Restores 80 [HP]{mechanic:hp}.</summery>			LEMONADE = 32,
            /// <summery>Used on a party Pokémon :   Restores 100 [HP]{mechanic:hp}.</summery>			MOOMOO_MILK = 33,
            /// <summery>Used on a party Pokémon :   Restores 50 [HP]{mechanic:hp}.  Decreases [happiness]{mechanic:happiness} by 5/5/10.</summery>			ENERGY_POWDER = 34,
            /// <summery>Used on a party Pokémon :   Restores 200 [HP]{mechanic:hp}.  Decreases [happiness]{mechanic:happiness} by 10/10/15.</summery>			ENERGY_ROOT = 35,
            /// <summery>Used on a party Pokémon :   Cures any [status ailment]{mechanic:status-ailment}.  Decreases [happiness]{mechanic:happiness} by 5/5/10.</summery>			HEAL_POWDER = 36,
            /// <summery>Used on a party Pokémon :   Revives a [fainted]{mechanic:faint} Pokémon and restores its [HP]{mechanic:hp} to full.  Decreases [happiness]{mechanic:happiness} by 10/10/15.</summery>			REVIVAL_HERB = 37,
            /// <summery>Used on a party Pokémon :   Restores 10 [PP]{mechanic:pp} for a selected move.</summery>			ETHER = 38,
            /// <summery>Used on a party Pokémon :   Restores [PP]{mechanic:pp} to full for a selected move.</summery>			MAX_ETHER = 39,
            /// <summery>Used on a party Pokémon :   Restores 10 [PP]{mechanic:pp} for each move.</summery>			ELIXIR = 40,
            /// <summery>Used on a party Pokémon :   Restores [PP]{mechanic:pp} to full for each move.</summery>			MAX_ELIXIR = 41,
            /// <summery>Used on a party Pokémon :   Cures any [status ailment]{mechanic:status-ailment} and [confusion]{mechanic:confusion}.</summery>			LAVA_COOKIE = 42,
            /// <summery>Used on a party Pokémon :   Restores 20 [HP]{mechanic:hp}.</summery>			BERRY_JUICE = 43,
            /// <summery>Used :   Revives all [fainted]{mechanic:faint} Pokémon in the party and restores their [HP]{mechanic:hp} to full.</summery>			SACRED_ASH = 44,
            /// <summery>Used on a party Pokémon :   Increases [HP]{mechanic:hp} [effort]{mechanic:effort} by 10, but won't increase it beyond 100.  Increases [happiness]{mechanic:happiness} by 5/3/2.</summery>			HP_UP = 45,
            /// <summery>Used on a party Pokémon :   Increases [Attack]{mechanic:attack} [effort]{mechanic:effort} by 10, but won't increase it beyond 100.  Increases [happiness]{mechanic:happiness} by 5/3/2.</summery>			PROTEIN = 46,
            /// <summery>Used on a party Pokémon :   Increases [Defense]{mechanic:defense} [effort]{mechanic:effort} by 10, but won't increase it beyond 100.  Increases [happiness]{mechanic:happiness} by 5/3/2.</summery>			IRON = 47,
            /// <summery>Used on a party Pokémon :   Increases [Speed]{mechanic:speed} [effort]{mechanic:effort} by 10, but won't increase it beyond 100.  Increases [happiness]{mechanic:happiness} by 5/3/2.</summery>			CARBOS = 48,
            /// <summery>Used on a party Pokémon :   Increases [Special Attack]{mechanic:special-attack} [effort]{mechanic:effort} by 10, but won't increase it beyond 100.  Increases [happiness]{mechanic:happiness} by 5/3/2.</summery>			CALCIUM = 49,
            /// <summery>Used on a party Pokémon :   Increases level by 1.  Increases [happiness]{mechanic:happiness} by 5/3/2.</summery>			RARE_CANDY = 50,
            /// <summery>Used on a party Pokémon :   Increases a selected move's max [PP]{mechanic:pp} by 20% its original max PP, to a maximum of 1.6×.  Increases [happiness]{mechanic:happiness} by 5/3/2.</summery>			PP_UP = 51,
            /// <summery>Used on a party Pokémon :   Increases [Special Defense]{mechanic:special-defense} [effort]{mechanic:effort} by 10, but won't increase it beyond 100.  Increases [happiness]{mechanic:happiness} by 5/3/2.</summery>			ZINC = 52,
            /// <summery>Used on a party Pokémon :   Increases a selected move's max [PP]{mechanic:pp} to 1.6× its original max PP.  Increases [happiness]{mechanic:happiness} by 5/3/2.</summery>			PP_MAX = 53,
            /// <summery>Used on a party Pokémon :   Cures any [status ailment]{mechanic:status-ailment} and [confusion]{mechanic:confusion}.</summery>			OLD_GATEAU = 54,
            /// <summery>Used on a party Pokémon in battle :   Protects the target's stats from being [lowered]{mechanic:lower} for the next five turns.  Increases happiness by 1/1/0.</summery>			GUARD_SPEC = 55,
            /// <summery>Used on a party Pokémon in battle :   Increases the target's [critical hit chance]{mechanic:critical-hit-chance} by one stage until it leaves the field.  Increases happiness by 1/1/0.</summery>			DIRE_HIT = 56,
            /// <summery>Used on a party Pokémon in battle :   [Raises]{mechanic:raise} the target's [Attack]{mechanic:attack} by one stage.  Increases happiness by 1/1/0.</summery>			X_ATTACK = 57,
            /// <summery>Used on a party Pokémon in battle :   [Raises]{mechanic:raise} the target's [Defense]{mechanic:defense} by one stage.  Increases happiness by 1/1/0.</summery>			X_DEFENSE = 58,
            /// <summery>Used on a party Pokémon in battle :   [Raises]{mechanic:raise} the target's [Speed]{mechanic:speed} by one stage.  Increases happiness by 1/1/0.</summery>			X_SPEED = 59,
            /// <summery>Used on a party Pokémon in battle :   [Raises]{mechanic:raise} the target's [accuracy]{mechanic:accuracy} by one stage.  Increases happiness by 1/1/0.</summery>			X_ACCURACY = 60,
            /// <summery>Used on a party Pokémon in battle :   [Raises]{mechanic:raise} the target's [Special Attack]{mechanic:special-attack} by one stage.  Increases happiness by 1/1/0.</summery>			X_SP_ATK = 61,
            /// <summery>Used on a party Pokémon in battle :   [Raises]{mechanic:raise} the target's [Special Defense]{mechanic:special-defense} by one stage.  Increases happiness by 1/1/0.</summery>			X_SP_DEF = 62,
            /// <summery>Used in battle :   Ends a wild battle.  Cannot be used in trainer battles.</summery>			POKE_DOLL = 63,
            /// <summery>Used in battle :   Ends a wild battle.  Cannot be used in trainer battles.</summery>			FLUFFY_TAIL = 64,
            /// <summery>Used on a party Pokémon :   Cures [sleep]{mechanic:sleep}.</summery>			BLUE_FLUTE = 65,
            /// <summery>Used on a party Pokémon in battle :   Cures [confusion]{mechanic:confusion}.</summery>			YELLOW_FLUTE = 66,
            /// <summery>Used on a party Pokémon in battle :   Cures [attraction]{mechanic:attraction}.</summery>			RED_FLUTE = 67,
            /// <summery>Used outside of battle :   Decreases the wild Pokémon encounter rate by 50%.</summery>			BLACK_FLUTE = 68,
            /// <summery>Used outside of battle :   Doubles the wild Pokémon encounter rate.</summery>			WHITE_FLUTE = 69,
            /// <summery>No effect.</summery>			SHOAL_SALT = 70,
            /// <summery>No effect.</summery>			SHOAL_SHELL = 71,
            /// <summery>No effect.  In Diamond and Pearl, trade ten for a []{move:sunny-day} [TM]{item:tm11} in the house midway along the southern section of []{location:sinnoh-route-212}.  In Platinum, trade to [move tutors]{mechanic:move-tutor} on []{location:sinnoh-route-212}, in []{location:snowpoint-city}, and in the []{location:survival-area}.  Eight shards total are required per tutelage, but the particular combination of colors varies by move.  In HeartGold and SoulSilver, trade one for a []{item:cheri-berry}, a []{item:leppa-berry}, and a []{item:pecha-berry} with the Juggler near the Pokémon Center in []{location:violet-city}.  In HeartGold and SoulSilver, trade one for a []{item:persim-berry}, a []{item:pomeg-berry}, and a []{item:razz-berry} with the Juggler near the []{location:pal-park} entrance in []{location:fuchsia-city}.</summery>			RED_SHARD = 72,
            /// <summery>No effect.  In Diamond and Pearl, trade ten for a []{move:rain-dance} [TM]{item:tm18} in the house midway along the southern section of []{location:sinnoh-route-212}.  In Platinum, trade to [move tutors]{mechanic:move-tutor} on []{location:sinnoh-route-212}, in []{location:snowpoint-city}, and in the []{location:survival-area}.  Eight shards total are required per tutelage, but the particular combination of colors varies by move.  In HeartGold and SoulSilver, trade one for a []{item:chesto-berry}, an []{item:oran-berry}, and a []{item:wiki-berry} with the Juggler near the Pokémon Center in []{location:violet-city}.  In HeartGold and SoulSilver, trade one for a []{item:bluk-berry}, a []{item:cornn-berry}, and a []{item:kelpsy-berry} with the Juggler near the []{location:pal-park} entrance in []{location:fuchsia-city}.</summery>			BLUE_SHARD = 73,
            /// <summery>No effect.  In Diamond and Pearl, trade ten for a []{move:sandstorm} [TM]{item:tm37} in the house midway along the southern section of []{location:sinnoh-route-212}.  In Platinum, trade to [move tutors]{mechanic:move-tutor} on []{location:sinnoh-route-212}, in []{location:snowpoint-city}, and in the []{location:survival-area}.  Eight shards total are required per tutelage, but the particular combination of colors varies by move.  In HeartGold and SoulSilver, trade one for an []{item:aspear-berry}, a []{item:iapapa-berry}, and a []{item:sitrus-berry} with the Juggler near the Pokémon Center in []{location:violet-city}.  In HeartGold and SoulSilver, trade one for a []{item:grepa-berry}, a []{item:nomel-berry}, and a []{item:pinap-berry} with the Juggler near the []{location:pal-park} entrance in []{location:fuchsia-city}.</summery>			YELLOW_SHARD = 74,
            /// <summery>No effect.  In Diamond and Pearl, trade ten for a []{move:hail} [TM]{item:tm07} in the house midway along the southern section of []{location:sinnoh-route-212}.  In Platinum, trade to [move tutors]{mechanic:move-tutor} on []{location:sinnoh-route-212}, in []{location:snowpoint-city}, and in the []{location:survival-area}.  Eight shards total are required per tutelage, but the particular combination of colors varies by move.  In HeartGold and SoulSilver, trade one for an []{item:aguav-berry}, a []{item:lum-berry}, and a []{item:rawst-berry} with the Juggler near the Pokémon Center in []{location:violet-city}.  In HeartGold and SoulSilver, trade one for a []{item:durin-berry}, a []{item:hondew-berry}, and a []{item:wepear-berry} with the Juggler near the []{location:pal-park} entrance in []{location:fuchsia-city}.</summery>			GREEN_SHARD = 75,
            /// <summery>Used outside of battle :   Trainer will skip encounters with wild Pokémon of a lower level than the lead party Pokémon.  This effect wears off after the trainer takes 200 steps.</summery>			SUPER_REPEL = 76,
            /// <summery>Used outside of battle :   Trainer will skip encounters with wild Pokémon of a lower level than the lead party Pokémon.  This effect wears off after the trainer takes 250 steps.</summery>			MAX_REPEL = 77,
            /// <summery>Used outside of battle :   Transports the trainer to the last-entered dungeon entrance.  Cannot be used outside, in buildings, or in []{location:distortion-world}, []{location:sinnoh-hall-of-origin-1}, []{location:spear-pillar}, or []{location:turnback-cave}.</summery>			ESCAPE_ROPE = 78,
            /// <summery>Used outside of battle :   Trainer will skip encounters with wild Pokémon of a lower level than the lead party Pokémon.  This effect wears off after the trainer takes 100 steps.</summery>			REPEL = 79,
            /// <summery>Used on a party Pokémon :   Evolves a []{pokemon:cottonee} into []{pokemon:whimsicott}, a []{pokemon:gloom} into []{pokemon:bellossom}, a []{pokemon:petilil} into []{pokemon:lilligant}, or a []{pokemon:sunkern} into []{pokemon:sunflora}.</summery>			SUN_STONE = 80,
            /// <summery>Used on a party Pokémon :   Evolves a []{pokemon:clefairy} into []{pokemon:clefable}, a []{pokemon:jigglypuff} into []{pokemon:wigglytuff}, a []{pokemon:munna} into []{pokemon:musharna}, a []{pokemon:nidorina} into []{pokemon:nidoqueen}, a []{pokemon:nidorino} into []{pokemon:nidoking}, or a []{pokemon:skitty} into []{pokemon:delcatty}.</summery>			MOON_STONE = 81,
            /// <summery>Used on a party Pokémon :   Evolves an []{pokemon:eevee} into []{pokemon:flareon}, a []{pokemon:growlithe} into []{pokemon:arcanine}, a []{pokemon:pansear} into []{pokemon:simisear}, or a []{pokemon:vulpix} into []{pokemon:ninetales}.</summery>			FIRE_STONE = 82,
            /// <summery>Used on a party Pokémon :   Evolves an []{pokemon:eelektrik} into []{pokemon:eelektross}, an []{pokemon:eevee} into []{pokemon:jolteon}, or a []{pokemon:pikachu} into []{pokemon:raichu}.</summery>			THUNDER_STONE = 83,
            /// <summery>Used on a party Pokémon :   Evolves an []{pokemon:eevee} into []{pokemon:vaporeon}, a []{pokemon:lombre} into []{pokemon:ludicolo}, a []{pokemon:panpour} into []{pokemon:simipour}, a []{pokemon:poliwhirl} into []{pokemon:poliwrath}, a []{pokemon:shellder} into []{pokemon:cloyster}, or a []{pokemon:staryu} into []{pokemon:starmie}.</summery>			WATER_STONE = 84,
            /// <summery>Used on a party Pokémon :   Evolves an []{pokemon:exeggcute} into []{pokemon:exeggutor}, a []{pokemon:gloom} into []{pokemon:vileplume}, a []{pokemon:nuzleaf} into []{pokemon:shiftry}, a []{pokemon:pansage} into []{pokemon:simisage}, or a []{pokemon:weepinbell} into []{pokemon:victreebel}.</summery>			LEAF_STONE = 85,
            /// <summery>Vendor trash.</summery>			TINY_MUSHROOM = 86,
            /// <summery>Vendor trash.</summery>			BIG_MUSHROOM = 87,
            /// <summery>Vendor trash.</summery>			PEARL = 88,
            /// <summery>Vendor trash.</summery>			BIG_PEARL = 89,
            /// <summery>Vendor trash.</summery>			STARDUST = 90,
            /// <summery>Vendor trash.</summery>			STAR_PIECE = 91,
            /// <summery>Vendor trash.</summery>			NUGGET = 92,
            /// <summery>Trade one to the Move Relearner near the shore in []{location:pastoria-city} or with the Move Deleter in []{location:blackthorn-city} to teach one party Pokémon a prior level-up move.</summery>			HEART_SCALE = 93,
            /// <summery>Used outside of battle :   Immediately triggers a wild Pokémon battle, as long as the trainer is somewhere with wild Pokémon—i.e., in tall grass, in a cave, or surfing.  Can be smeared on sweet-smelling trees to attract tree-dwelling Pokémon after six hours.</summery>			HONEY = 94,
            /// <summery>Used on a patch of soil :   Plant's growth stages will each last 25% less time.  Dries soil out more quickly.</summery>			GROWTH_MULCH = 95,
            /// <summery>Used on a patch of soil :   Plant's growth stages will each last 25% more time.  Dries soil out more slowly.</summery>			DAMP_MULCH = 96,
            /// <summery>Used on a patch of soil :   Fully-grown plant will last 25% longer before dying and possibly regrowing.</summery>			STABLE_MULCH = 97,
            /// <summery>Used on a path of soil :   Plant will regrow after dying 25% more times.</summery>			GOOEY_MULCH = 98,
            /// <summery>Give to a scientist in the []{location:mining-museum} in []{location:oreburgh-city} or the Museum of Science in []{location:pewter-city} to receive a []{pokemon:lileep}.</summery>			ROOT_FOSSIL = 99,
            /// <summery>Give to a scientist in the []{location:mining-museum} in []{location:oreburgh-city} or the Museum of Science in []{location:pewter-city} to receive a []{pokemon:anorith}.</summery>			CLAW_FOSSIL = 100,
            /// <summery>Give to a scientist in the []{location:mining-museum} in []{location:oreburgh-city} or the Museum of Science in []{location:pewter-city} to receive a []{pokemon:omanyte}.</summery>			HELIX_FOSSIL = 101,
            /// <summery>Give to a scientist in the []{location:mining-museum} in []{location:oreburgh-city} or the Museum of Science in []{location:pewter-city} to receive a []{pokemon:kabuto}.</summery>			DOME_FOSSIL = 102,
            /// <summery>Give to a scientist in the []{location:mining-museum} in []{location:oreburgh-city} or the Museum of Science in []{location:pewter-city} to receive a []{pokemon:aerodactyl}.</summery>			OLD_AMBER = 103,
            /// <summery>Give to a scientist in the []{location:mining-museum} in []{location:oreburgh-city} or the Museum of Science in []{location:pewter-city} to receive a []{pokemon:shieldon}.</summery>			ARMOR_FOSSIL = 104,
            /// <summery>Give to a scientist in the []{location:mining-museum} in []{location:oreburgh-city} or the Museum of Science in []{location:pewter-city} to receive a []{pokemon:cranidos}.</summery>			SKULL_FOSSIL = 105,
            /// <summery>Vendor trash.</summery>			RARE_BONE = 106,
            /// <summery>Used on a party Pokémon :   Evolves a []{pokemon:minccino} into []{pokemon:cinccino}, a []{pokemon:roselia} into []{pokemon:roserade}, or a []{pokemon:togetic} into []{pokemon:togekiss}.</summery>			SHINY_STONE = 107,
            /// <summery>Used on a party Pokémon :   Evolves a []{pokemon:lampent} into []{pokemon:chandelure}, a []{pokemon:misdreavus} into []{pokemon:mismagius}, or a []{pokemon:murkrow} into []{pokemon:honchkrow}.</summery>			DUSK_STONE = 108,
            /// <summery>Used on a party Pokémon :   Evolves a male []{pokemon:kirlia} into []{pokemon:gallade} or a female []{pokemon:snorunt} into []{pokemon:froslass}.</summery>			DAWN_STONE = 109,
            /// <summery>Held by []{pokemon:happiny} :   Holder evolves into []{pokemon:chansey} when it levels up during the daytime.</summery>			OVAL_STONE = 110,
            /// <summery>Place in the tower on []{location:sinnoh-route-209}.  Check the stone to encounter a []{pokemon:spiritomb}, as long as the trainer's Underground status card counts at least 32 greetings.</summery>			ODD_KEYSTONE = 111,
            /// <summery>Held by []{pokemon:dialga} :   Holder's []{type:dragon}- and []{type:steel}-type moves have 1.2× their usual power.</summery>			ADAMANT_ORB = 112,
            /// <summery>Held by []{pokemon:palkia} :   Holder's []{type:dragon}- and []{type:water}-type moves have 1.2× their usual power.</summery>			LUSTROUS_ORB = 113,
            /// <summery>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summery>			GRASS_MAIL = 114,
            /// <summery>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summery>			FLAME_MAIL = 115,
            /// <summery>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summery>			BUBBLE_MAIL = 116,
            /// <summery>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summery>			BLOOM_MAIL = 117,
            /// <summery>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summery>			TUNNEL_MAIL = 118,
            /// <summery>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summery>			STEEL_MAIL = 119,
            /// <summery>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summery>			HEART_MAIL = 120,
            /// <summery>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summery>			SNOW_MAIL = 121,
            /// <summery>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summery>			SPACE_MAIL = 122,
            /// <summery>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summery>			AIR_MAIL = 123,
            /// <summery>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summery>			MOSAIC_MAIL = 124,
            /// <summery>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summery>			BRICK_MAIL = 125,
            /// <summery>Held in battle :   When the holder is [paralyzed]{mechanic:paralysis}, it consumes this item to cure the paralysis.  Used on a party Pokémon :   Cures [paralysis]{mechanic:paralysis}.</summery>			CHERI_BERRY = 126,
            /// <summery>Held in battle :   When the holder is [asleep]{mechanic:sleep}, it consumes this item to wake up.  Used on a party Pokémon :   Cures [sleep]{mechanic:sleep}.</summery>			CHESTO_BERRY = 127,
            /// <summery>Held in battle :   When the holder is [poisoned]{mechanic:poison}, it consumes this item to cure the poison.  Used on a party Pokémon :   Cures [poison]{mechanic:poison}.</summery>			PECHA_BERRY = 128,
            /// <summery>Held in battle :   When the holder is [burned]{mechanic:burn}, it consumes this item to cure the burn.  Used on a party Pokémon :   Cures a [burn]{mechanic:burn}.</summery>			RAWST_BERRY = 129,
            /// <summery>Held in battle :   When the holder is [frozen]{mechanic:freezing}, it consumes this item to thaw itself.  Used on a party Pokémon :   Cures [freezing]{mechanic:freezing}.</summery>			ASPEAR_BERRY = 130,
            /// <summery>Held in battle :   When the holder is out of [PP]{mechanic:pp} for one of its moves, it consumes this item to restore 10 of that move's PP.  Used on a party Pokémon :   Restores 10 [PP]{mechanic:pp} for a selected move.</summery>			LEPPA_BERRY = 131,
            /// <summery>Held in battle :   When the holder has 1/2 its max [HP]{mechanic:hp} remaining or less, it consumes this item to restore 10 HP.  Used on a party Pokémon :   Restores 10 [HP]{mechanic:hp}.</summery>			ORAN_BERRY = 132,
            /// <summery>Held in battle :   When the holder is [confused]{mechanic:confusion}, it consumes this item to cure the confusion.  Used on a party Pokémon :   Cures [confusion]{mechanic:confusion}.</summery>			PERSIM_BERRY = 133,
            /// <summery>Held in battle :   When the holder is afflicted with a [major status ailment]{mechanic:major-status-ailment}, it consumes this item to cure the ailment.  Used on a party Pokémon :   Cures any [major status ailment]{mechanic:major-status-ailment}.</summery>			LUM_BERRY = 134,
            /// <summery>Held in battle :   When the holder has 1/2 its max [HP]{mechanic:hp} remaining or less, it consumes this item to restore 1/4 its max HP.  Used on a party Pokémon :   Restores 1/4 the Pokémon's max [HP]{mechanic:hp}.</summery>			SITRUS_BERRY = 135,
            /// <summery>Held in battle :   When the holder has 1/2 its max [HP]{mechanic:hp} remaining or less, it consumes this item to restore 1/8 its max HP.  If the holder dislikes spicy flavors (i.e., has a nature that lowers [Attack]{mechanic:attack}), it will also become [confused]{mechanic:confusion}.</summery>			FIGY_BERRY = 136,
            /// <summery>Held in battle :   When the holder has 1/2 its max [HP]{mechanic:hp} remaining or less, it consumes this item to restore 1/8 its max HP.  If the holder dislikes dry flavors (i.e., has a nature that lowers [Special Attack]{mechanic:special-attack}), it will also become [confused]{mechanic:confusion}.</summery>			WIKI_BERRY = 137,
            /// <summery>Held in battle :   When the holder has 1/2 its max [HP]{mechanic:hp} remaining or less, it consumes this item to restore 1/8 its max HP.  If the holder dislikes sweet flavors (i.e., has a nature that lowers [Speed]{mechanic:speed}), it will also become [confused]{mechanic:confusion}.</summery>			MAGO_BERRY = 138,
            /// <summery>Held in battle :   When the holder has 1/2 its max [HP]{mechanic:hp} remaining or less, it consumes this item to restore 1/8 its max HP.  If the holder dislikes bitter flavors (i.e., has a nature that lowers [Special Defense]{mechanic:special-defense}), it will also become [confused]{mechanic:confusion}.</summery>			AGUAV_BERRY = 139,
            /// <summery>Held in battle :   When the holder has 1/2 its max [HP]{mechanic:hp} remaining or less, it consumes this item to restore 1/8 its max HP.  If the holder dislikes sour flavors (i.e., has a nature that lowers [Defense]{mechanic:defense}), it will also become [confused]{mechanic:confusion}.</summery>			IAPAPA_BERRY = 140,
            /// <summery>No effect; only useful for planting and cooking.</summery>			RAZZ_BERRY = 141,
            /// <summery>No effect; only useful for planting and cooking.</summery>			BLUK_BERRY = 142,
            /// <summery>No effect; only useful for planting and cooking.</summery>			NANAB_BERRY = 143,
            /// <summery>No effect; only useful for planting and cooking.</summery>			WEPEAR_BERRY = 144,
            /// <summery>No effect; only useful for planting and cooking.</summery>			PINAP_BERRY = 145,
            /// <summery>Used on a party Pokémon :   Increases [happiness]{mechanic:happiness} by 10/5/2.  Lowers [HP]{mechanic:hp} [effort]{mechanic:effort} by 10.</summery>			POMEG_BERRY = 146,
            /// <summery>Used on a party Pokémon :   Increases [happiness]{mechanic:happiness} by 10/5/2.  Lowers [Attack]{mechanic:attack} [effort]{mechanic:effort} by 10.</summery>			KELPSY_BERRY = 147,
            /// <summery>Used on a party Pokémon :   Increases [happiness]{mechanic:happiness} by 10/5/2.  Lowers [Defense]{mechanic:defense} [effort]{mechanic:effort} by 10.</summery>			QUALOT_BERRY = 148,
            /// <summery>Used on a party Pokémon :   Increases [happiness]{mechanic:happiness} by 10/5/2.  Lowers [Special Attack]{mechanic:special-attack} [effort]{mechanic:effort} by 10.</summery>			HONDEW_BERRY = 149,
            /// <summery>Used on a party Pokémon :   Increases [happiness]{mechanic:happiness} by 10/5/2.  Lowers [Special Defense]{mechanic:special-defense} [effort]{mechanic:effort} by 10.</summery>			GREPA_BERRY = 150,
            /// <summery>Used on a party Pokémon :   Increases [happiness]{mechanic:happiness} by 10/5/2.  Lowers [Speed]{mechanic:speed} [effort]{mechanic:effort} by 10.</summery>			TAMATO_BERRY = 151,
            /// <summery>No effect; only useful for planting and cooking.</summery>			CORNN_BERRY = 152,
            /// <summery>No effect; only useful for planting and cooking.</summery>			MAGOST_BERRY = 153,
            /// <summery>No effect; only useful for planting and cooking.</summery>			RABUTA_BERRY = 154,
            /// <summery>No effect; only useful for planting and cooking.</summery>			NOMEL_BERRY = 155,
            /// <summery>No effect; only useful for planting and cooking.</summery>			SPELON_BERRY = 156,
            /// <summery>No effect; only useful for planting and cooking.</summery>			PAMTRE_BERRY = 157,
            /// <summery>No effect; only useful for planting and cooking.</summery>			WATMEL_BERRY = 158,
            /// <summery>No effect; only useful for planting and cooking.</summery>			DURIN_BERRY = 159,
            /// <summery>No effect; only useful for planting and cooking.</summery>			BELUE_BERRY = 160,
            /// <summery>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:fire}-type damage, it consumes this item to halve the amount of damage taken.</summery>			OCCA_BERRY = 161,
            /// <summery>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:water}-type damage, it consumes this item to halve the amount of damage taken.</summery>			PASSHO_BERRY = 162,
            /// <summery>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:electric}-type damage, it consumes this item to halve the amount of damage taken.</summery>			WACAN_BERRY = 163,
            /// <summery>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:grass}-type damage, it consumes this item to halve the amount of damage taken.</summery>			RINDO_BERRY = 164,
            /// <summery>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:ice}-type damage, it consumes this item to halve the amount of damage taken.</summery>			YACHE_BERRY = 165,
            /// <summery>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:fighting}-type damage, it consumes this item to halve the amount of damage taken.</summery>			CHOPLE_BERRY = 166,
            /// <summery>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:poison}-type damage, it consumes this item to halve the amount of damage taken.</summery>			KEBIA_BERRY = 167,
            /// <summery>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:ground}-type damage, it consumes this item to halve the amount of damage taken.</summery>			SHUCA_BERRY = 168,
            /// <summery>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:flying}-type damage, it consumes this item to halve the amount of damage taken.</summery>			COBA_BERRY = 169,
            /// <summery>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:psychic}-type damage, it consumes this item to halve the amount of damage taken.</summery>			PAYAPA_BERRY = 170,
            /// <summery>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:bug}-type damage, it consumes this item to halve the amount of damage taken.</summery>			TANGA_BERRY = 171,
            /// <summery>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:rock}-type damage, it consumes this item to halve the amount of damage taken.</summery>			CHARTI_BERRY = 172,
            /// <summery>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:ghost}-type damage, it consumes this item to halve the amount of damage taken.</summery>			KASIB_BERRY = 173,
            /// <summery>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:dragon}-type damage, it consumes this item to halve the amount of damage taken.</summery>			HABAN_BERRY = 174,
            /// <summery>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:dark}-type damage, it consumes this item to halve the amount of damage taken.</summery>			COLBUR_BERRY = 175,
            /// <summery>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:steel}-type damage, it consumes this item to halve the amount of damage taken.</summery>			BABIRI_BERRY = 176,
            /// <summery>Held in battle :   When the holder would take []{type:normal}-type damage, it consumes this item to halve the amount of damage taken.</summery>			CHILAN_BERRY = 177,
            /// <summery>Held in battle :   When the holder has 1/4 its max [HP]{mechanic:hp} remaining or less, it consumes this item to [raise]{mechanic:raise} its [Attack]{mechanic:attack} by one stage.</summery>			LIECHI_BERRY = 178,
            /// <summery>Held in battle :   When the holder has 1/4 its max [HP]{mechanic:hp} remaining or less, it consumes this item to [raise]{mechanic:raise} its [Defense]{mechanic:defense} by one stage.</summery>			GANLON_BERRY = 179,
            /// <summery>Held in battle :   When the holder has 1/4 its max [HP]{mechanic:hp} remaining or less, it consumes this item to [raise]{mechanic:raise} its [Speed]{mechanic:speed} by one stage.</summery>			SALAC_BERRY = 180,
            /// <summery>Held in battle :   When the holder has 1/4 its max [HP]{mechanic:hp} remaining or less, it consumes this item to [raise]{mechanic:raise} its [Special Attack]{mechanic:special-attack} by one stage.</summery>			PETAYA_BERRY = 181,
            /// <summery>Held in battle :   When the holder has 1/4 its max [HP]{mechanic:hp} remaining or less, it consumes this item to [raise]{mechanic:raise} its [Special Defense]{mechanic:special-defense} by one stage.</summery>			APICOT_BERRY = 182,
            /// <summery>Held in battle :   When the holder has 1/4 its max [HP]{mechanic:hp} remaining or less, it consumes this item to [raise]{mechanic:raise} its [critical hit chance]{mechanic:critical-hit-chance} by one stage.</summery>			LANSAT_BERRY = 183,
            /// <summery>Held in battle :   When the holder has 1/4 its max [HP]{mechanic:hp} remaining or less, it consumes this item to [raise]{mechanic:raise} a random stat by two stages.</summery>			STARF_BERRY = 184,
            /// <summery>Held in battle :   When the holder takes [super-effective]{mechanic:super-effective} damage, it consumes this item to restore 1/4 its max [HP]{mechanic:hp}.</summery>			ENIGMA_BERRY = 185,
            /// <summery>Held in battle :   When the holder has 1/4 its max [HP]{mechanic:hp} remaining or less, it consumes this item, and its next used move has 1.2× its normal accuracy.</summery>			MICLE_BERRY = 186,
            /// <summery>Held in battle :   When the holder has 1/4 its max [HP]{mechanic:hp} remaining or less, it consumes this item.  On the following turn, the holder will act first among moves with the same [priority]{mechanic:priority}, regardless of [Speed]{mechanic:speed}.</summery>			CUSTAP_BERRY = 187,
            /// <summery>Held in battle :   When the holder takes [physical]{mechanic:physical} damage, it consumes this item to damage the attacking Pokémon for 1/8 its max [HP]{mechanic:hp}.</summery>			JABOCA_BERRY = 188,
            /// <summery>Held in battle :   When the holder takes [special]{mechanic:special} damage, it consumes this item to damage the attacking Pokémon for 1/8 its max [HP]{mechanic:hp}.</summery>			ROWAP_BERRY = 189,
            /// <summery>Held in battle :   Moves targeting the holder have 0.9× [chance to hit]{mechanic:chance-to-hit}.</summery>			BRIGHT_POWDER = 190,
            /// <summery>Held in battle :   At the end of each turn, if any of the holder's stats have a negative [stat modifier]{mechanic:stat-modifier}, the holder consumes this item to remove the modifiers from those stats.</summery>			WHITE_HERB = 191,
            /// <summery>Held :   When the holder would gain [effort]{mechanic:effort} due to battle, it gains double that effort instead.  Held in battle :   Holder has half its [Speed]{mechanic:speed}.</summery>			MACHO_BRACE = 192,
            /// <summery>Held :   [Experience]{mechanic:experience} is split across two groups: Pokémon who participated in battle, and Pokémon holding this item.  Each Pokémon earns experience as though it had battled alone, divided by the number of Pokémon in its group, then divided by the number of groups. Pokémon holding this item who also participated in battle effectively earn experience twice.      [Fainted]{mechanic:fainted} Pokémon never earn experience, and empty groups are ignored; thus, if a single Pokémon is holding this item and the only Pokémon who battled faints from []{move:explosion}, the holder will gain full experience.</summery>			EXP_SHARE = 193,
            /// <summery>Held in battle :   Whenever the holder attempts to use a move, it has a 3/16 chance to act first among moves with the same [priority]{mechanic:priority}.  If multiple Pokémon have this effect at the same time, [Speed]{mechanic:speed} is the tie-breaker as normal, but the effect of []{move:trick-room} is ignored.</summery>			QUICK_CLAW = 194,
            /// <summery>Held :   When the holder would earn [happiness]{mechanic:happiness} for any reason, it earns twice that amount instead.</summery>			SOOTHE_BELL = 195,
            /// <summery>Held in battle :   When the holder is [attracted]{move:attract}, it consumes this item to cure the attraction.</summery>			MENTAL_HERB = 196,
            /// <summery>Held in battle :   Holder has 1.5× its [Attack]{mechanic:attack}.  When the holder attempts to use a move, all its other moves are disabled until it leaves battle or loses this item.      The restriction ends even if this item is swapped for another Choice item via []{move:trick} or []{move:switcheroo}.</summery>			CHOICE_BAND = 197,
            /// <summery>Held in battle :   Holder's damaging moves have a 10% chance to make their target [flinch]{mechanic:flinch}.  This chance applies independently to each hit of a multi-hit move.      This item's chance is rolled independently of any other move effects; e.g., a move with a 30% chance to flinch normally will have a 37% total chance to flinch when used with this item, because 3% of the time, both effects activate.  Held by []{pokemon:poliwhirl} or []{pokemon:slowbro} :   Holder evolves into []{pokemon:politoed} or []{pokemon:slowking}, respectively, when traded.</summery>			KINGS_ROCK = 198,
            /// <summery>Held in battle :   Holder's []{type:bug}-type moves have 1.2× their power. </summery>			SILVER_POWDER = 199,
            /// <summery>Held :   If the holder participated in a trainer battle, the trainer earns twice the usual prize money.  This effect applies even if the holder [fainted]{mechanic:fainted}.      This effect does not stack with any other similar effect.</summery>			AMULET_COIN = 200,
            /// <summery>Held by lead Pokémon: Prevents wild battles with Pokémon that are lower level than the holder.</summery>			CLEANSE_TAG = 201,
            /// <summery>Held by Latias or Latios: Increases the holder's Special Attack and Special Defense by 50%.</summery>			SOUL_DEW = 202,
            /// <summery>Held by Clamperl: Doubles the holder's Special Attack.  Evolves the holder into Huntail when traded.</summery>			DEEP_SEA_TOOTH = 203,
            /// <summery>Held by Clamperl: Doubles the holder's Special Defense.  Evolves the holder into Gorebyss when traded.</summery>			DEEP_SEA_SCALE = 204,
            /// <summery>Held: In wild battles, attempts to run away on the holder's turn will always succeed.</summery>			SMOKE_BALL = 205,
            /// <summery>Held: Prevents the holder from evolving naturally.  Evolution initiated by the trainer (Stones, etc) will still work.</summery>			EVERSTONE = 206,
            /// <summery>Held: If the holder is attacked for regular damage that would faint it, this item has a 10% chance to prevent the holder's HP from lowering below 1.</summery>			FOCUS_BAND = 207,
            /// <summery>Held: Increases any Exp the holder gains by 50%.</summery>			LUCKY_EGG = 208,
            /// <summery>Held: Raises the holder's critical hit counter by 1.</summery>			SCOPE_LENS = 209,
            /// <summery>Held: Increases the power of the holder's Steel moves by 20%. Held by Onix or Scyther: Evolves the holder into Steelix or Scizor when traded, respectively.</summery>			METAL_COAT = 210,
            /// <summery>Held: Heals the holder by 1/16 its max HP at the end of each turn.</summery>			LEFTOVERS = 211,
            /// <summery>Held by Seadra: Evolves the holder into Kingdra when traded.</summery>			DRAGON_SCALE = 212,
            /// <summery>Held by Pikachu: Doubles the holder's initial Attack and Special Attack.</summery>			LIGHT_BALL = 213,
            /// <summery>Held: Increases the power of the holder's Ground moves by 20%.</summery>			SOFT_SAND = 214,
            /// <summery>Held: Increases the power of the holder's Rock moves by 20%.</summery>			HARD_STONE = 215,
            /// <summery>Held: Increases the power of the holder's Grass moves by 20%.</summery>			MIRACLE_SEED = 216,
            /// <summery>Held: Increases the power of the holder's Dark moves by 20%.</summery>			BLACK_GLASSES = 217,
            /// <summery>Held: Increases the power of the holder's Fighting moves by 20%.</summery>			BLACK_BELT = 218,
            /// <summery>Held: Increases the power of the holder's Electric moves by 20%.</summery>			MAGNET = 219,
            /// <summery>Held: Increases the power of the holder's Water moves by 20%.</summery>			MYSTIC_WATER = 220,
            /// <summery>Held: Increases the power of the holder's Flying moves by 20%.</summery>			SHARP_BEAK = 221,
            /// <summery>Held: Increases the power of the holder's Poison moves by 20%.</summery>			POISON_BARB = 222,
            /// <summery>Held: Increases the power of the holder's Ice moves by 20%.</summery>			NEVER_MELT_ICE = 223,
            /// <summery>Held: Increases the power of the holder's Ghost moves by 20%.</summery>			SPELL_TAG = 224,
            /// <summery>Held: Increases the power of the holder's Psychic moves by 20%.</summery>			TWISTED_SPOON = 225,
            /// <summery>Held: Increases the power of the holder's Fire moves by 20%.</summery>			CHARCOAL = 226,
            /// <summery>Held: Increases the power of the holder's Dragon moves by 20%.</summery>			DRAGON_FANG = 227,
            /// <summery>Held: Increases the power of the holder's Normal moves by 20%.</summery>			SILK_SCARF = 228,
            /// <summery>Held by Porygon: Evolves the holder into Porygon2 when traded.</summery>			UP_GRADE = 229,
            /// <summery>Held: Heals the holder by 1/8 of any damage it inflicts.</summery>			SHELL_BELL = 230,
            /// <summery>Held: Increases the power of the holder's Water moves by 20%.</summery>			SEA_INCENSE = 231,
            /// <summery>Held: Increases the holder's Evasion by 5%.</summery>			LAX_INCENSE = 232,
            /// <summery>Held by Chansey: Raises the holder's critical hit counter by 2.</summery>			LUCKY_PUNCH = 233,
            /// <summery>Held by Ditto: Increases the holder's initial Defense and Special Defense by 50%.</summery>			METAL_POWDER = 234,
            /// <summery>Held by Cubone or Marowak: Doubles the holder's Attack.</summery>			THICK_CLUB = 235,
            /// <summery>Held by Farfetch'd: Raises the holder's critical hit counter by 2.</summery>			STICK = 236,
            /// <summery>Held: Increases the holder's Coolness during a Super Contest's Visual Competition.</summery>			RED_SCARF = 237,
            /// <summery>Held: Increases the holder's Beauty during a Super Contest's Visual Competition.</summery>			BLUE_SCARF = 238,
            /// <summery>Held: Increases the holder's Cuteness during a Super Contest's Visual Competition.</summery>			PINK_SCARF = 239,
            /// <summery>Held: Increases the holder's Smartness during a Super Contest's Visual Competition.</summery>			GREEN_SCARF = 240,
            /// <summery>Held: Increases the holder's Toughness during a Super Contest's Visual Competition.</summery>			YELLOW_SCARF = 241,
            /// <summery>Held: Increases the accuracy of any move the holder uses by 10% (multiplied; i.e. 70% accuracy is increased to 77%).</summery>			WIDE_LENS = 242,
            /// <summery>Held: Increases the power of the holder's physical moves by 10%.</summery>			MUSCLE_BAND = 243,
            /// <summery>Held: Increases the power of the holder's special moves by 10%.</summery>			WISE_GLASSES = 244,
            /// <summery>Held: When the holder hits with a super-effective move, its power is raised by 20%.</summery>			EXPERT_BELT = 245,
            /// <summery>Held: The holder's Reflect and Light Screen will create effects lasting for eight turns rather than five.  As this item affects the move rather than the barrier itself, the effect is not lost if the holder leaves battle or drops this item.</summery>			LIGHT_CLAY = 246,
            /// <summery>Held: Damage from the holder's moves is increased by 30%.  On each turn the holder uses a damage-inflicting move, it takes 10% its max HP in damage.</summery>			LIFE_ORB = 247,
            /// <summery>Held: Whenever the holder uses a move that requires a turn to charge first (Bounce, Dig, Dive, Fly, Razor Wind, Skull Bash, Sky Attack, or Solarbeam), this item is consumed and the charge is skipped.  Skull Bash still provides a Defense boost.</summery>			POWER_HERB = 248,
            /// <summery>Held: Badly poisons the holder at the end of each turn.</summery>			TOXIC_ORB = 249,
            /// <summery>Held: Burns the holder at the end of each turn.</summery>			FLAME_ORB = 250,
            /// <summery>Held by Ditto: Doubles the holder's initial Speed.</summery>			QUICK_POWDER = 251,
            /// <summery>Held: If the holder has full HP and is attacked for regular damage that would faint it, this item is consumed and prevents the holder's HP from lowering below 1.  This effect works against multi-hit attacks, but does not work against the effects of Doom Desire or Future Sight.</summery>			FOCUS_SASH = 252,
            /// <summery>Held: Raises the holder's Accuracy by 20% when it goes last. Ingame description is incorrect.</summery>			ZOOM_LENS = 253,
            /// <summery>Held: Each time the holder uses the same move consecutively, its power is increased by another 10% of its original, to a maximum of 100%.</summery>			METRONOME = 254,
            /// <summery>Held: Decreases the holder's Speed by 50%.  If the holder is Flying or has Levitate, it takes regular damage from Ground attacks and is suspectible to Spikes and Toxic Spikes.</summery>			IRON_BALL = 255,
            /// <summery>Held: The holder will go last within its move's priority bracket, regardless of Speed.  If multiple Pokémon within the same priority bracket are subject to this effect, the slower Pokémon will go first.  The holder will move after Pokémon with Stall.  If the holder has Stall, Stall is ignored.  This item ignores Trick Room.</summery>			LAGGING_TAIL = 256,
            /// <summery>Held: When the holder becomes Attracted, the Pokémon it is Attracted to becomes Attracted back.</summery>			DESTINY_KNOT = 257,
            /// <summery>Held: If the holder is Poison-type, restores 1/16 max HP at the end of each turn.  Otherwise, damages the holder by 1/16 its max HP at the end of each turn.</summery>			BLACK_SLUDGE = 258,
            /// <summery>Held: The holder's Hail will create a hailstorm lasting for eight turns rather than five.  As this item affects the move rather than the weather itself, the effect is not lost if the holder leaves battle or drops this item.</summery>			ICY_ROCK = 259,
            /// <summery>Held: The holder's Sandstorm will create a sandstorm lasting for eight turns rather than five.  As this item affects the move rather than the weather itself, the effect is not lost if the holder leaves battle or drops this item.</summery>			SMOOTH_ROCK = 260,
            /// <summery>Held: The holder's Sunny Day will create sunshine lasting for eight turns rather than five.  As this item affects the move rather than the weather itself, the effect is not lost if the holder leaves battle or drops this item.</summery>			HEAT_ROCK = 261,
            /// <summery>Held: The holder's Rain Dance will create rain lasting for eight turns rather than five.  As this item affects the move rather than the weather itself, the effect is not lost if the holder leaves battle or drops this item.</summery>			DAMP_ROCK = 262,
            /// <summery>Held: Increases the duration of the holder's multiturn (2-5 turn) moves by three turns.</summery>			GRIP_CLAW = 263,
            /// <summery>Held: Increases the holder's Speed by 50%, but restricts it to the first move it uses until it leaves battle or loses this item.  If this item is swapped for another Choice item via Trick or Switcheroo, the holder's restriction is still lifted, but it will again be restricted to the next move it uses. (Quirk: If the holder is switched in by U-Turn and it also knows U-Turn, U-Turn becomes its restricted move.)</summery>			CHOICE_SCARF = 264,
            /// <summery>Held: Damaged the holder for 1/8 its max HP.  When the holder is struck by a contact move, damages the attacker for 1/8 its max HP; if the attacker is not holding an item, it will take this item.</summery>			STICKY_BARB = 265,
            /// <summery>Held: Decreases the holder's Speed by 50%.  Whenever the holder gains Attack effort from battle, increases that effort by 4; this applies before the PokéRUS doubling effect.</summery>			POWER_BRACER = 266,
            /// <summery>Held: Decreases the holder's Speed by 50%.  Whenever the holder gains Defense effort from battle, increases that effort by 4; this applies before the PokéRUS doubling effect.</summery>			POWER_BELT = 267,
            /// <summery>Held: Decreases the holder's Speed by 50%.  Whenever the holder gains Special Attack effort from battle, increases that effort by 4; this applies before the PokéRUS doubling effect.</summery>			POWER_LENS = 268,
            /// <summery>Held: Decreases the holder's Speed by 50%.  Whenever the holder gains Special Defense effort from battle, increases that effort by 4; this applies before the PokéRUS doubling effect.</summery>			POWER_BAND = 269,
            /// <summery>Held: Decreases the holder's Speed by 50%.  Whenever the holder gains Speed effort from battle, increases that effort by 4; this applies before the PokéRUS doubling effect.</summery>			POWER_ANKLET = 270,
            /// <summery>Held: Decreases the holder's Speed by 50%.  Whenever the holder gains HP effort from battle, increases that effort by 4; this applies before the PokéRUS doubling effect.</summery>			POWER_WEIGHT = 271,
            /// <summery>Held: The holder is unaffected by any moves or abilities that would prevent it from actively leaving battle.</summery>			SHED_SHELL = 272,
            /// <summery>Held: HP restored from Absorb, Aqua Ring, Drain Punch, Dream Eater, Giga Drain, Ingrain, Leech Life, Leech Seed, and Mega Drain is increased by 30%.  Damage inflicted is not affected.</summery>			BIG_ROOT = 273,
            /// <summery>Held: Increases the holder's Special Attack by 50%, but restricts it to the first move it uses until it leaves battle or loses this item.  If this item is swapped for another Choice item via Trick or Switcheroo, the holder's restriction is still lifted, but it will again be restricted to the next move it uses. (Quirk: If the holder is switched in by U-Turn and it also knows U-Turn, U-Turn becomes its restricted move.)</summery>			CHOICE_SPECS = 274,
            /// <summery>Held: Increases the power of the holder's Fire moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Fire.</summery>			FLAME_PLATE = 275,
            /// <summery>Held: Increases the power of the holder's Water moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Water.</summery>			SPLASH_PLATE = 276,
            /// <summery>Held: Increases the power of the holder's Electric moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Electric.</summery>			ZAP_PLATE = 277,
            /// <summery>Held: Increases the power of the holder's Grass moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Grass.</summery>			MEADOW_PLATE = 278,
            /// <summery>Held: Increases the power of the holder's Ice moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Ice.</summery>			ICICLE_PLATE = 279,
            /// <summery>Held: Increases the power of the holder's Fighting moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Fighting.</summery>			FIST_PLATE = 280,
            /// <summery>Held: Increases the power of the holder's Poison moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Poison.</summery>			TOXIC_PLATE = 281,
            /// <summery>Held: Increases the power of the holder's Ground moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Ground.</summery>			EARTH_PLATE = 282,
            /// <summery>Held: Increases the power of the holder's Flying moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Flying.</summery>			SKY_PLATE = 283,
            /// <summery>Held: Increases the power of the holder's Psychic moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Psychic.</summery>			MIND_PLATE = 284,
            /// <summery>Held: Increases the power of the holder's Bug moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Bug.</summery>			INSECT_PLATE = 285,
            /// <summery>Held: Increases the power of the holder's Rock moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Rock.</summery>			STONE_PLATE = 286,
            /// <summery>Held: Increases the power of the holder's Ghost moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Ghost.</summery>			SPOOKY_PLATE = 287,
            /// <summery>Held: Increases the power of the holder's Dragon moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Dragon.</summery>			DRACO_PLATE = 288,
            /// <summery>Held: Increases the power of the holder's Dark moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Dark.</summery>			DREAD_PLATE = 289,
            /// <summery>Held: Increases the power of the holder's Steel moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Steel.</summery>			IRON_PLATE = 290,
            /// <summery>Held: Increases the power of the holder's Psychic moves by 20%.</summery>			ODD_INCENSE = 291,
            /// <summery>Held: Increases the power of the holder's Rock moves by 20%.</summery>			ROCK_INCENSE = 292,
            /// <summery>Held: The holder will go last within its move's priority bracket, regardless of Speed.  If multiple Pokémon within the same priority bracket are subject to this effect, the slower Pokémon will go first.  The holder will move after Pokémon with Stall.  If the holder has Stall, Stall is ignored.  This item ignores Trick Room.</summery>			FULL_INCENSE = 293,
            /// <summery>Held: Increases the power of the holder's Water moves by 20%.</summery>			WAVE_INCENSE = 294,
            /// <summery>Held: Increases the power of the holder's Grass moves by 20%.</summery>			ROSE_INCENSE = 295,
            /// <summery>Held: Doubles the money the trainer receives after an in-game trainer battle.  This effect cannot apply more than once to the same battle.</summery>			LUCK_INCENSE = 296,
            /// <summery>Held by lead Pokémon: Prevents wild battles with Pokémon that are lower level than the holder.</summery>			PURE_INCENSE = 297,
            /// <summery>Held by Rhydon: Evolves the holder into Rhyperior when traded.</summery>			PROTECTOR = 298,
            /// <summery>Held by Electabuzz: Evolves the holder into Electivire when traded.</summery>			ELECTIRIZER = 299,
            /// <summery>Held by Magmar: Evolves the holder into Magmortar when traded.</summery>			MAGMARIZER = 300,
            /// <summery>Held by Porygon2: Evolves the holder into Porygon-Z when traded.</summery>			DUBIOUS_DISC = 301,
            /// <summery>Held by Dusclops: Evolves the holder into Dusknoir when traded.</summery>			REAPER_CLOTH = 302,
            /// <summery>Held: Raises the holder's critical hit counter by 1. Held by Sneasel: Evolves the holder into Weavile when it levels up during the night.</summery>			RAZOR_CLAW = 303,
            /// <summery>Held: When the holder attacks with most damaging moves, provides an extra 11.7% (30/256) chance for the target to flinch. Held by Gligar: Evolves the holder into Gliscor when it levels up.</summery>			RAZOR_FANG = 304,
            /// <summery>Teaches Focus Punch to a compatible Pokémon.</summery>			TM01 = 305,
            /// <summery>Teaches Dragon Claw to a compatible Pokémon.</summery>			TM02 = 306,
            /// <summery>Teaches Water Pulse to a compatible Pokémon.</summery>			TM03 = 307,
            /// <summery>Teaches Calm Mind to a compatible Pokémon.</summery>			TM04 = 308,
            /// <summery>Teaches Roar to a compatible Pokémon.</summery>			TM05 = 309,
            /// <summery>Teaches Toxic to a compatible Pokémon.</summery>			TM06 = 310,
            /// <summery>Teaches Hail to a compatible Pokémon.</summery>			TM07 = 311,
            /// <summery>Teaches Bulk Up to a compatible Pokémon.</summery>			TM08 = 312,
            /// <summery>Teaches Bullet Seed to a compatible Pokémon.</summery>			TM09 = 313,
            /// <summery>Teaches Hidden Power to a compatible Pokémon.</summery>			TM10 = 314,
            /// <summery>Teaches Sunny Day to a compatible Pokémon.</summery>			TM11 = 315,
            /// <summery>Teaches Taunt to a compatible Pokémon.</summery>			TM12 = 316,
            /// <summery>Teaches Ice Beam to a compatible Pokémon.</summery>			TM13 = 317,
            /// <summery>Teaches Blizzard to a compatible Pokémon.</summery>			TM14 = 318,
            /// <summery>Teaches Hyper Beam to a compatible Pokémon.</summery>			TM15 = 319,
            /// <summery>Teaches Light Screen to a compatible Pokémon.</summery>			TM16 = 320,
            /// <summery>Teaches Protect to a compatible Pokémon.</summery>			TM17 = 321,
            /// <summery>Teaches Rain Dance to a compatible Pokémon.</summery>			TM18 = 322,
            /// <summery>Teaches Giga Drain to a compatible Pokémon.</summery>			TM19 = 323,
            /// <summery>Teaches Safeguard to a compatible Pokémon.</summery>			TM20 = 324,
            /// <summery>Teaches Frustration to a compatible Pokémon.</summery>			TM21 = 325,
            /// <summery>Teaches SolarBeam to a compatible Pokémon.</summery>			TM22 = 326,
            /// <summery>Teaches Iron Tail to a compatible Pokémon.</summery>			TM23 = 327,
            /// <summery>Teaches Thunderbolt to a compatible Pokémon.</summery>			TM24 = 328,
            /// <summery>Teaches Thunder to a compatible Pokémon.</summery>			TM25 = 329,
            /// <summery>Teaches Earthquake to a compatible Pokémon.</summery>			TM26 = 330,
            /// <summery>Teaches Return to a compatible Pokémon.</summery>			TM27 = 331,
            /// <summery>Teaches Dig to a compatible Pokémon.</summery>			TM28 = 332,
            /// <summery>Teaches Psychic to a compatible Pokémon.</summery>			TM29 = 333,
            /// <summery>Teaches Shadow Ball to a compatible Pokémon.</summery>			TM30 = 334,
            /// <summery>Teaches Brick Break to a compatible Pokémon.</summery>			TM31 = 335,
            /// <summery>Teaches Double Team to a compatible Pokémon.</summery>			TM32 = 336,
            /// <summery>Teaches Reflect to a compatible Pokémon.</summery>			TM33 = 337,
            /// <summery>Teaches Shock Wave to a compatible Pokémon.</summery>			TM34 = 338,
            /// <summery>Teaches Flamethrower to a compatible Pokémon.</summery>			TM35 = 339,
            /// <summery>Teaches Sludge Bomb to a compatible Pokémon.</summery>			TM36 = 340,
            /// <summery>Teaches Sandstorm to a compatible Pokémon.</summery>			TM37 = 341,
            /// <summery>Teaches Fire Blast to a compatible Pokémon.</summery>			TM38 = 342,
            /// <summery>Teaches Rock Tomb to a compatible Pokémon.</summery>			TM39 = 343,
            /// <summery>Teaches Aerial Ace to a compatible Pokémon.</summery>			TM40 = 344,
            /// <summery>Teaches Torment to a compatible Pokémon.</summery>			TM41 = 345,
            /// <summery>Teaches Facade to a compatible Pokémon.</summery>			TM42 = 346,
            /// <summery>Teaches Secret Power to a compatible Pokémon.</summery>			TM43 = 347,
            /// <summery>Teaches Rest to a compatible Pokémon.</summery>			TM44 = 348,
            /// <summery>Teaches Attract to a compatible Pokémon.</summery>			TM45 = 349,
            /// <summery>Teaches Thief to a compatible Pokémon.</summery>			TM46 = 350,
            /// <summery>Teaches Steel Wing to a compatible Pokémon.</summery>			TM47 = 351,
            /// <summery>Teaches Skill Swap to a compatible Pokémon.</summery>			TM48 = 352,
            /// <summery>Teaches Snatch to a compatible Pokémon.</summery>			TM49 = 353,
            /// <summery>Teaches Overheat to a compatible Pokémon.</summery>			TM50 = 354,
            /// <summery>Teaches Roost to a compatible Pokémon.</summery>			TM51 = 355,
            /// <summery>Teaches Focus Blast to a compatible Pokémon.</summery>			TM52 = 356,
            /// <summery>Teaches Energy Ball to a compatible Pokémon.</summery>			TM53 = 357,
            /// <summery>Teaches False Swipe to a compatible Pokémon.</summery>			TM54 = 358,
            /// <summery>Teaches Brine to a compatible Pokémon.</summery>			TM55 = 359,
            /// <summery>Teaches Fling to a compatible Pokémon.</summery>			TM56 = 360,
            /// <summery>Teaches Charge Beam to a compatible Pokémon.</summery>			TM57 = 361,
            /// <summery>Teaches Endure to a compatible Pokémon.</summery>			TM58 = 362,
            /// <summery>Teaches Dragon Pulse to a compatible Pokémon.</summery>			TM59 = 363,
            /// <summery>Teaches Drain Punch to a compatible Pokémon.</summery>			TM60 = 364,
            /// <summery>Teaches Will-O-Wisp to a compatible Pokémon.</summery>			TM61 = 365,
            /// <summery>Teaches Silver Wind to a compatible Pokémon.</summery>			TM62 = 366,
            /// <summery>Teaches Embargo to a compatible Pokémon.</summery>			TM63 = 367,
            /// <summery>Teaches Explosion to a compatible Pokémon.</summery>			TM64 = 368,
            /// <summery>Teaches Shadow Claw to a compatible Pokémon.</summery>			TM65 = 369,
            /// <summery>Teaches Payback to a compatible Pokémon.</summery>			TM66 = 370,
            /// <summery>Teaches Recycle to a compatible Pokémon.</summery>			TM67 = 371,
            /// <summery>Teaches Giga Impact to a compatible Pokémon.</summery>			TM68 = 372,
            /// <summery>Teaches Rock Polish to a compatible Pokémon.</summery>			TM69 = 373,
            /// <summery>Teaches Flash to a compatible Pokémon.</summery>			TM70 = 374,
            /// <summery>Teaches Stone Edge to a compatible Pokémon.</summery>			TM71 = 375,
            /// <summery>Teaches Avalanche to a compatible Pokémon.</summery>			TM72 = 376,
            /// <summery>Teaches Thunder Wave to a compatible Pokémon.</summery>			TM73 = 377,
            /// <summery>Teaches Gyro Ball to a compatible Pokémon.</summery>			TM74 = 378,
            /// <summery>Teaches Swords Dance to a compatible Pokémon.</summery>			TM75 = 379,
            /// <summery>Teaches Stealth Rock to a compatible Pokémon.</summery>			TM76 = 380,
            /// <summery>Teaches Psych Up to a compatible Pokémon.</summery>			TM77 = 381,
            /// <summery>Teaches Captivate to a compatible Pokémon.</summery>			TM78 = 382,
            /// <summery>Teaches Dark Pulse to a compatible Pokémon.</summery>			TM79 = 383,
            /// <summery>Teaches Rock Slide to a compatible Pokémon.</summery>			TM80 = 384,
            /// <summery>Teaches X-Scissor to a compatible Pokémon.</summery>			TM81 = 385,
            /// <summery>Teaches Sleep Talk to a compatible Pokémon.</summery>			TM82 = 386,
            /// <summery>Teaches Natural Gift to a compatible Pokémon.</summery>			TM83 = 387,
            /// <summery>Teaches Poison Jab to a compatible Pokémon.</summery>			TM84 = 388,
            /// <summery>Teaches Dream Eater to a compatible Pokémon.</summery>			TM85 = 389,
            /// <summery>Teaches Grass Knot to a compatible Pokémon.</summery>			TM86 = 390,
            /// <summery>Teaches Swagger to a compatible Pokémon.</summery>			TM87 = 391,
            /// <summery>Teaches Pluck to a compatible Pokémon.</summery>			TM88 = 392,
            /// <summery>Teaches U-Turn to a compatible Pokémon.</summery>			TM89 = 393,
            /// <summery>Teaches Substitute to a compatible Pokémon.</summery>			TM90 = 394,
            /// <summery>Teaches Flash Cannon to a compatible Pokémon.</summery>			TM91 = 395,
            /// <summery>Teaches Trick Room to a compatible Pokémon.</summery>			TM92 = 396,
            /// <summery>Teaches Cut to a compatible Pokémon.</summery>			HM01 = 397,
            /// <summery>Teaches Fly to a compatible Pokémon.</summery>			HM02 = 398,
            /// <summery>Teaches Surf to a compatible Pokémon.</summery>			HM03 = 399,
            /// <summery>Teaches Strength to a compatible Pokémon.</summery>			HM04 = 400,
            /// <summery>Teaches Defog to a compatible Pokémon.</summery>			HM05 = 401,
            /// <summery>Teaches Rock Smash to a compatible Pokémon.</summery>			HM06 = 402,
            /// <summery>Teaches Waterfall to a compatible Pokémon.</summery>			HM07 = 403,
            /// <summery>Teaches Rock Climb to a compatible Pokémon.</summery>			HM08 = 404,
            /// <summery>Sends the trainer to the Underground.  Only usable outside.</summery>			EXPLORER_KIT = 405,
            /// <summery>Unused.</summery>			LOOT_SACK = 406,
            /// <summery>Unused.</summery>			RULE_BOOK = 407,
            /// <summery>Designates several nearby patches of grass as containing Pokémon, some of which may be special radar-only Pokémon.  Successive uses in a certain way create chains of encounters with the same species; longer chains increase the chance that a shiny Pokémon of that species will appear.</summery>			POKE_RADAR = 408,
            /// <summery>Tracks Battle Points.</summery>			POINT_CARD = 409,
            /// <summery>Records some of the trainer's activities for the day.</summery>			JOURNAL = 410,
            /// <summery>Contains Seals used for decorating Pokéballs.</summery>			SEAL_CASE = 411,
            /// <summery>Contains Pokémon Accessories.</summery>			FASHION_CASE = 412,
            /// <summery>Unused.</summery>			SEAL_BAG = 413,
            /// <summery>Contains friend codes for up to 32 other players, as well as their sprite, gender, and basic statistics for those that have been seen on WFC.</summery>			PAL_PAD = 414,
            /// <summery>Opens the front door of the Valley Windworks.  Reusable.</summery>			WORKS_KEY = 415,
            /// <summery>Given to Cynthia's grandmother to get the Surf HM.</summery>			OLD_CHARM = 416,
            /// <summery>Grants access to Galactic HQ in Veilstone City.</summery>			GALACTIC_KEY = 417,
            /// <summery>Unused.</summery>			RED_CHAIN = 418,
            /// <summery>Displays a map of the region including the trainer's position, location names, visited towns, gym locations, and where the trainer has been walking recently.</summery>			TOWN_MAP = 419,
            /// <summery>Reveals trainers who want a rematch, by showing !! over their heads.  Each use drains the battery; requires 100 steps to charge.</summery>			VS_SEEKER = 420,
            /// <summery>Contains the Coins used by the Game Corner, to a maximum of 50,000.</summery>			COIN_CASE = 421,
            /// <summery>Used to find Pokémon on the Old Rod list for an area, which are generally Magikarp or similar.</summery>			OLD_ROD = 422,
            /// <summery>Used to find Pokémon on the Good Rod list for an area, which are generally mediocre.</summery>			GOOD_ROD = 423,
            /// <summery>Used to find Pokémon on the Super Rod list for an area, which are generally the best available there.</summery>			SUPER_ROD = 424,
            /// <summery>Waters Berry plants.</summery>			SPRAYDUCK = 425,
            /// <summery>Contains up to 100 Poffins.</summery>			POFFIN_CASE = 426,
            /// <summery>Increases movement speed outside or in caves.  In high gear, allows the trainer to hop over some rocks and ascend muddy slopes.</summery>			BICYCLE = 427,
            /// <summery>Opens the locked building in the lakeside resort.</summery>			SUITE_KEY = 428,
            /// <summery>Grants access to Flower Paradise and Shaymin.</summery>			OAKS_LETTER = 429,
            /// <summery>Cures the sailor's son of his nightmares; no reward, only a side effect of seeing Cresselia.</summery>			LUNAR_WING = 430,
            /// <summery>Provides access to Newmoon Island and Darkrai.</summery>			MEMBER_CARD = 431,
            /// <summery>Supposedly related to t</summery>			AZURE_FLUTE = 432,
            /// <summery>Allows passage on a ferry.  The same item is used for different ferries between different games.</summery>			SS_TICKET = 433,
            /// <summery>Allows the trainer to enter Contests.</summery>			CONTEST_PASS = 434,
            /// <summery>Causes Heatran to appear at Reversal Mountain.  Unused prior to Black and White 2.</summery>			MAGMA_STONE = 435,
            /// <summery>Given to the trainer's rival in Jubilife City.  Contains two Town Maps, one of which is given to the trainer upon delivery.</summery>			PARCEL = 436,
            /// <summery>One of three coupons needed to receive a Pokétch.</summery>			COUPON_1 = 437,
            /// <summery>One of three coupons needed to receive a Pokétch.</summery>			COUPON_2 = 438,
            /// <summery>One of three coupons needed to receive a Pokétch.</summery>			COUPON_3 = 439,
            /// <summery>Grants access to the Team Galactic warehouse in Veilstone City.</summery>			STORAGE_KEY = 440,
            /// <summery>Required to cure the Psyducks blocking Route 210 of their chronic headaches.</summery>			SECRET_POTION = 441,
            /// <summery>Held by []{pokemon:giratina} :   Holder's []{type:dragon} and []{type:ghost} moves have 1.2× their base power.      Holder is in Origin Forme.  This item cannot be held by any Pokémon but Giratina.  When you enter the Union Room or connect to Wi-Fi, this item returns to your bag.</summery>			GRISEOUS_ORB = 442,
            /// <summery>Optionally records wireless, Wi-Fi, and Battle Frontier battles.  Tracks Battle Points earned in the Battle Frontier, and stores commemorative prints.</summery>			VS_RECORDER = 443,
            /// <summery>Used by trainer on a []{pokemon:shaymin} :   Changes the target Shaymin from Land Forme to Sky Forme.      This item cannot be used on a [frozen]{mechanic:freezing} Shaymin or at night.  Sky Forme Shaymin will revert to Land Forme overnight, when frozen, and upon entering a link battle.  This item must be used again to change it back.</summery>			GRACIDEA = 444,
            /// <summery>Used by trainer in the Galactic Eterna Building, on the ground floor, to the left of the TV :   Unlocks the secret []{pokemon:rotom} room, in which there are five appliances which can change Rotom's form.</summery>			SECRET_KEY = 445,
            /// <summery>Stores Apricorns.</summery>			APRICORN_BOX = 446,
            /// <summery>Contains four portable pots of soil suitable for growing berries.</summery>			BERRY_POTS = 447,
            /// <summery>Required to water berries within the []{item:berry-pots}.  Required to battle the []{pokemon:sudowoodo} on []{location:johto-route-36}.  This item cannot be directly used from the bag.</summery>			SQUIRT_BOTTLE = 448,
            /// <summery>Used by trainer in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.  If used in a trainer battle, nothing happens and the ball is lost.      If the wild Pokémon was encountered by fishing, the wild Pokémon's catch rate is 3× normal.</summery>			LURE_BALL = 449,
            /// <summery>Used by trainer in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.  If used in a trainer battle, nothing happens and the ball is lost.      If the trainer's Pokémon's level is higher than:      * four times the wild Pokémon's, the wild Pokémon's catch rate is 8× normal.     * than twice the wild Pokémon's, the wild Pokémon's catch rate is 4× normal.     * the wild Pokémon's, the wild Pokémon's catch rate is 2× normal.</summery>			LEVEL_BALL = 450,
            /// <summery>Used by trainer in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.  If used in a trainer battle, nothing happens and the ball is lost.      If the wild Pokémon is a []{pokemon:clefairy}, []{pokemon:nidoran-m}, []{pokemon:nidoran-f}, []{pokemon:jigglypuff}, []{pokemon:skitty}, or any evolution thereof, the wild Pokémon has 4× its catch rate.</summery>			MOON_BALL = 451,
            /// <summery>Used by a trainer in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.  If used in a trainer battle, nothing happens and the ball is lost.      If the wild Pokémon weighs:      * 409.6 kg (903.0 lb) or more, its catch rate is 40 more than normal.     * 307.2 kg (677.3 lb) or more, its catch rate is 30 more than normal.     * 204.8 kg (451.5 lb) or more, its catch rate is 20 more than normal.     * less than 204.8 kg (451.5 lb), its catch rate is 20 less than normal.</summery>			HEAVY_BALL = 452,
            /// <summery>Used by a trainer in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.  If used in a trainer battle, nothing happens and the ball is lost.  :   If the wild Pokémon's base [speed]{mechanic:speed} is 100 or more, its catch rate is 4× normal.</summery>			FAST_BALL = 453,
            /// <summery>Used by a trainer in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.  If used in a trainer battle, nothing happens and the ball is lost.      If caught, the wild Pokémon's [happiness]{mechanic:happiness} starts at 200.</summery>			FRIEND_BALL = 454,
            /// <summery>Used by a trainer in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.  If used in a trainer battle, nothing happens and the ball is lost.      If the trainer's Pokémon and wild Pokémon are of the same species but opposite genders, the wild Pokémon's catch rate is 8× normal.</summery>			LOVE_BALL = 455,
            /// <summery>Used by a trainer in battle :   [Catches]{mechanic:catch} a wild Pokémon.  This item can only be used in []{location:pal-park}.</summery>			PARK_BALL = 456,
            /// <summery>Used by a trainer in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.      The wild Pokémon's catch rate is 1.5× normal.</summery>			SPORT_BALL = 457,
            /// <summery>May be given to Kurt in []{location:azalea-town} to produce a []{item:level-ball}.</summery>			RED_APRICORN = 458,
            /// <summery>May be given to Kurt in []{location:azalea-town} to produce a []{item:lure-ball}.</summery>			BLUE_APRICORN = 459,
            /// <summery>May be given to Kurt in []{location:azalea-town} to produce a []{item:moon-ball}.</summery>			YELLOW_APRICORN = 460,
            /// <summery>May be given to Kurt in []{location:azalea-town} to produce a []{item:friend-ball}.</summery>			GREEN_APRICORN = 461,
            /// <summery>May be given to Kurt in []{location:azalea-town} to produce a []{item:love-ball}.</summery>			PINK_APRICORN = 462,
            /// <summery>May be given to Kurt in []{location:azalea-town} to produce a []{item:fast-ball}.</summery>			WHITE_APRICORN = 463,
            /// <summery>May be given to Kurt in []{location:azalea-town} to produce a []{item:heavy-ball}.</summery>			BLACK_APRICORN = 464,
            /// <summery>Used by trainer outside of battle :   Searches for hidden items.</summery>			DOWSING_MACHINE = 465,
            /// <summery>May be traded for a []{item:tm64} in the vertical Underground Path.</summery>			RAGE_CANDY_BAR = 466,
            /// <summery>Causes []{pokemon:groudon} to appear in the []{location:embedded-tower}.</summery>			RED_ORB = 467,
            /// <summery>Causes []{pokemon:kyogre} to appear in the []{location:embedded-tower}.</summery>			BLUE_ORB = 468,
            /// <summery>Causes []{pokemon:rayquaza} to appear in the []{location:embedded-tower}.</summery>			JADE_ORB = 469,
            /// <summery>When taken to the []{location:pewter-city} museum, causes []{pokemon:latias} or []{pokemon:latios} to attack the trainer.  The Pokémon to appear will be whicher can't be encountered roaming in the wild.</summery>			ENIGMA_STONE = 470,
            /// <summery>Lists which []{pokemon:unown} forms the trainer has caught.</summery>			UNOWN_REPORT = 471,
            /// <summery>Allows the trainer to answer the daily question on Buena's radio show.  Records the points earned for correct answers.</summery>			BLUE_CARD = 472,
            /// <summery>Does nothing.</summery>			SLOWPOKE_TAIL = 473,
            /// <summery>May be given to the Kimono Girls to summon []{pokemon:ho-oh} to the top of the []{location:bell-tower}.</summery>			CLEAR_BELL = 474,
            /// <summery>Used by trainer outside of battle :   Opens doors in the []{location:goldenrod-city} Radio Tower.</summery>			CARD_KEY = 475,
            /// <summery>Used by trainer outside of battle :   Opens the door to the basement tunnel under []{location:goldenrod-city}.</summery>			BASEMENT_KEY = 476,
            /// <summery>May be traded to Mr. Pokémon for an []{item:exp-share}.</summery>			RED_SCALE = 477,
            /// <summery>May be traded to the Copycat for a []{item:pass}.</summery>			LOST_ITEM = 478,
            /// <summery>Allows the trainer to ride the Magnet Train between []{location:goldenrod-city} and []{location:saffron-city}.</summery>			PASS = 479,
            /// <summery>Must be replaced in the []{location:power-plant} to power the Magnet Train.</summery>			MACHINE_PART = 480,
            /// <summery>Causes []{pokemon:lugia} to appear in the []{location:whirl-islands}.</summery>			SILVER_WING = 481,
            /// <summery>Causes []{pokemon:ho-oh} to appear at the top of []{location:bell-tower}.</summery>			RAINBOW_WING = 482,
            /// <summery>Must be obtained to trigger the break-in at Professor Elm's lab, the first rival battle, and access to []{location:johto-route-31}.</summery>			MYSTERY_EGG = 483,
            /// <summery>Used by trainer outside of battle :   Changes the background music to the equivalent 8-bit music.</summery>			GB_SOUNDS = 484,
            /// <summery>May be given to the Kimono Girls to summon []{pokemon:lugia} to the top of the []{location:bell-tower}.</summery>			TIDAL_BELL = 485,
            /// <summery>Records the number of times the trainer has come in first place overall in the Pokéathlon.</summery>			DATA_CARD_01 = 486,
            /// <summery>Records the number of times the trainer has come in last place overall in the Pokéathlon.</summery>			DATA_CARD_02 = 487,
            /// <summery>Records the number of times the trainer's Pokémon have dashed in the Pokéathlon.</summery>			DATA_CARD_03 = 488,
            /// <summery>Records the number of times the trainer's Pokémon have jumped in the Pokéathlon.</summery>			DATA_CARD_04 = 489,
            /// <summery>Records the number of times the trainer has come in first in the Pokéathlon Hurdle Dash.</summery>			DATA_CARD_05 = 490,
            /// <summery>Records the number of times the trainer has come in first in the Pokéathlon Relay Run.</summery>			DATA_CARD_06 = 491,
            /// <summery>Records the number of times the trainer has come in first in the Pokéathlon Pennant Capture.</summery>			DATA_CARD_07 = 492,
            /// <summery>Records the number of times the trainer has come in first in the Pokéathlon Block Smash.</summery>			DATA_CARD_08 = 493,
            /// <summery>Records the number of times the trainer has come in first in the Pokéathlon Disc Catch.</summery>			DATA_CARD_09 = 494,
            /// <summery>Records the number of times the trainer has come in first in the Pokéathlon Snow Throw.</summery>			DATA_CARD_10 = 495,
            /// <summery>Records the number of points the trainer has earned in the Pokéathlon.</summery>			DATA_CARD_11 = 496,
            /// <summery>Records the number of times the trainer's Pokémon have messed up in the Pokéathlon.</summery>			DATA_CARD_12 = 497,
            /// <summery>Records the number of times the trainer's Pokémon have defeated themselves in the Pokéathlon.</summery>			DATA_CARD_13 = 498,
            /// <summery>Records the number of times the trainer's Pokémon have tackled in the Pokéathlon.</summery>			DATA_CARD_14 = 499,
            /// <summery>Records the number of times the trainer's Pokémon have fallen in the Pokéathlon.</summery>			DATA_CARD_15 = 500,
            /// <summery>Records the number of times the trainer has come in first in the Pokéathlon Ring Drop.</summery>			DATA_CARD_16 = 501,
            /// <summery>Records the number of times the trainer has come in first in the Pokéathlon Lamp Jump.</summery>			DATA_CARD_17 = 502,
            /// <summery>Records the number of times the trainer has come in first in the Pokéathlon Circle Push.</summery>			DATA_CARD_18 = 503,
            /// <summery>Records the number of times the trainer has come in first place overall in the Pokéathlon over wirelss.</summery>			DATA_CARD_19 = 504,
            /// <summery>Records the number of times the trainer has come in last place overall in the Pokéathlon over wireless.</summery>			DATA_CARD_20 = 505,
            /// <summery>Records the number of times the trainer has come in first across all Pokéathlon events.</summery>			DATA_CARD_21 = 506,
            /// <summery>Records the number of times the trainer has come in last across all Pokéathlon events.</summery>			DATA_CARD_22 = 507,
            /// <summery>Records the number of times the trainer has switched Pokémon in the Pokéathlon.</summery>			DATA_CARD_23 = 508,
            /// <summery>Records the number of times the trainer has come in first in the Pokéathlon Goal Roll.</summery>			DATA_CARD_24 = 509,
            /// <summery>Records the number of times the trainer's Pokémon received prizes in the Pokéathlon.</summery>			DATA_CARD_25 = 510,
            /// <summery>Records the number of times the trainer has instructed Pokémon in the Pokéathlon.</summery>			DATA_CARD_26 = 511,
            /// <summery>Records the total time spent in the Pokéathlon.</summery>			DATA_CARD_27 = 512,
            /// <summery>Does nothing.</summery>			LOCK_CAPSULE = 513,
            /// <summery>Does nothing.</summery>			PHOTO_ALBUM = 514,
            /// <summery>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summery>			ORANGE_MAIL = 515,
            /// <summery>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summery>			HARBOR_MAIL = 516,
            /// <summery>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summery>			GLITTER_MAIL = 517,
            /// <summery>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summery>			MECH_MAIL = 518,
            /// <summery>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summery>			WOOD_MAIL = 519,
            /// <summery>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summery>			WAVE_MAIL = 520,
            /// <summery>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summery>			BEAD_MAIL = 521,
            /// <summery>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summery>			SHADOW_MAIL = 522,
            /// <summery>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summery>			TROPIC_MAIL = 523,
            /// <summery>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summery>			DREAM_MAIL = 524,
            /// <summery>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summery>			FAB_MAIL = 525,
            /// <summery>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summery>			RETRO_MAIL = 526,
            /// <summery>Increases movement speed outside or in caves.  Faster than the []{item:acro-bike}.  Allows the trainer to ascend muddy slopes.</summery>			MACH_BIKE = 527,
            /// <summery>Increases movement speed outside or in caves.  Slower than the []{item:mach-bike}.  Can perform various tricks, allowing the trainer to reach certain special areas.</summery>			ACRO_BIKE = 528,
            /// <summery>Waters Berry plants.</summery>			WAILMER_PAIL = 529,
            /// <summery>Contains a machine part to be delivered to Captain Stern.</summery>			DEVON_GOODS = 530,
            /// <summery>Collects soot when walking through tall grass on []{location:hoenn-route-113}.</summery>			SOOT_SACK = 531,
            /// <summery>Stores Pokéblocks.</summery>			POKEBLOCK_CASE = 532,
            /// <summery>Contains a letter to be delivered to Steven.</summery>			LETTER = 533,
            /// <summery>Provides access to []{location:southern-island} and either []{pokemon:latias} or []{pokemon:latios}, whichever is not available roaming around Hoenn.</summery>			EON_TICKET = 534,
            /// <summery>May be traded to Captain Stern for a []{item:deep-sea-tooth} or a []{item:deep-sea-scale}.</summery>			SCANNER = 535,
            /// <summery>Allows the trainer to enter the desert on []{location:hoenn-route-111}.</summery>			GO_GOGGLES = 536,
            /// <summery>RSE: May be traded to Professor Cozmo for []{item:tm27}.  FRLG: A meteorite to be delivered to Lostelle's father.</summery>			METEORITE = 537,
            /// <summery>Unlocks room 1 on the []{location:abandoned-ship}.</summery>			RM_1_KEY = 538,
            /// <summery>Unlocks room 2 on the []{location:abandoned-ship}.</summery>			RM_2_KEY = 539,
            /// <summery>Unlocks room 4 on the []{location:abandoned-ship}.</summery>			RM_4_KEY = 540,
            /// <summery>Unlocks room 6 on the []{location:abandoned-ship}.</summery>			RM_6_KEY = 541,
            /// <summery>Reveals invisble []{pokemon:kecleon} on the overworld.</summery>			DEVON_SCOPE = 542,
            /// <summery>A parcel to be delivered to Professor Oak for a Pokédex.</summery>			OAKS_PARCEL = 543,
            /// <summery>Wakes up [sleeping]{mechanic:sleep} Pokémon.  Required to wake up sleeping []{pokemon:snorlax} on the overworld.</summery>			POKE_FLUTE = 544,
            /// <summery>May be traded for a []{item:bicycle}.</summery>			BIKE_VOUCHER = 545,
            /// <summery>The Safari Zone warden's teeth, to be returned to him for []{item:hm04}.</summery>			GOLD_TEETH = 546,
            /// <summery>Operates the elevator in the Celadon Rocket Hideout.</summery>			LIFT_KEY = 547,
            /// <summery>Identifies ghosts in []{location:pokemon-tower}.</summery>			SILPH_SCOPE = 548,
            /// <summery>Records information on various famous people.</summery>			FAME_CHECKER = 549,
            /// <summery>Stores TMs and HMs.</summery>			TM_CASE = 550,
            /// <summery>Stores Berries.</summery>			BERRY_POUCH = 551,
            /// <summery>Teaches beginning trainers basic information.</summery>			TEACHY_TV = 552,
            /// <summery>Provides access to the first three Sevii Islands.</summery>			TRI_PASS = 553,
            /// <summery>Provides access to the Sevii Islands.</summery>			RAINBOW_PASS = 554,
            /// <summery>Used to bribe the []{location:saffron-city} guards for entry to the city.</summery>			TEA = 555,
            /// <summery>Provides access to Navel Rock, []{pokemon:ho-oh}, and []{pokemon:lugia}.</summery>			MYSTICTICKET = 556,
            /// <summery>Provides access to Birth Island and []{pokemon:deoxys}.</summery>			AURORATICKET = 557,
            /// <summery>Holds Berry Powder from Berry Crushing.</summery>			POWDER_JAR = 558,
            /// <summery>Deliver to Celio for use in the Network Machine.</summery>			RUBY = 559,
            /// <summery>Deliver to Celio for use in the Network Machine.</summery>			SAPPHIRE = 560,
            /// <summery>Provides access to the []{location:magma-hideout} in the []{location:jagged-pass}.</summery>			MAGMA_EMBLEM = 561,
            /// <summery>Provides access to Faraway Island and []{pokemon:mew}.</summery>			OLD_SEA_MAP = 562,
            /// <summery>Held by []{pokemon:genesect} :   Holder's buster is blue, and its []{move:techno-blast} is []{type:water}-type.</summery>			DOUSE_DRIVE = 563,
            /// <summery>Held by []{pokemon:genesect} :   Holder's buster is yellow, and its []{move:techno-blast} is []{type:electric}-type.</summery>			SHOCK_DRIVE = 564,
            /// <summery>Held by []{pokemon:genesect} :   Holder's buster is red, and its []{move:techno-blast} is []{type:fire}-type.</summery>			BURN_DRIVE = 565,
            /// <summery>Held by []{pokemon:genesect} :   Holder's buster is white, and its []{move:techno-blast} becomes []{type:ice}-type.</summery>			CHILL_DRIVE = 566,
            /// <summery>Used on a friendly Pokémon :   Restores 20 [HP]{mechanic:hp}.</summery>			SWEET_HEART = 567,
            /// <summery>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summery>			GREET_MAIL = 568,
            /// <summery>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summery>			FAVORED_MAIL = 569,
            /// <summery>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summery>			RSVP_MAIL = 570,
            /// <summery>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summery>			THANKS_MAIL = 571,
            /// <summery>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summery>			INQUIRY_MAIL = 572,
            /// <summery>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summery>			LIKE_MAIL = 573,
            /// <summery>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summery>			REPLY_MAIL = 574,
            /// <summery>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summery>			BRIDGE_MAIL_S = 575,
            /// <summery>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summery>			BRIDGE_MAIL_D = 576,
            /// <summery>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summery>			BRIDGE_MAIL_T = 577,
            /// <summery>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summery>			BRIDGE_MAIL_V = 578,
            /// <summery>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summery>			BRIDGE_MAIL_M = 579,
            /// <summery>Held by []{pokemon:feebas} :   Holder evolves into []{pokemon:milotic} when traded.</summery>			PRISM_SCALE = 580,
            /// <summery>Held by a Pokémon that is not fully evolved :   Holder has 1.5× [Defense]{mechanic:defense} and [Special Defense]{mechanic:special-defense}.</summery>			EVIOLITE = 581,
            /// <summery>Held :   Holder has 0.5× weight.</summery>			FLOAT_STONE = 582,
            /// <summery>Held :   When the holder is hit by a [contact]{mechanic:contact} move, the attacking Pokémon takes 1/6 its max [HP]{mechanic:hp} in damage.</summery>			ROCKY_HELMET = 583,
            /// <summery>Held :   Holder is immune to []{type:ground}-type moves, []{move:spikes}, []{move:toxic-spikes}, and []{ability:arena-trap}.      This effect does not apply during []{move:gravity} or []{move:ingrain}.      When the holder takes damage from a move, this item is consumed.</summery>			AIR_BALLOON = 584,
            /// <summery>Held :   When the holder takes damage directly from a move and does not faint, it [switches out]{mechanic:switching-out} for another random, non-fainted Pokémon in its party. This effect does not activate if another effect prevents the holder from switching out.</summery>			RED_CARD = 585,
            /// <summery>Held :   When one of the user's types would render it immune to damage, that type is ignored for damage calculation.</summery>			RING_TARGET = 586,
            /// <summery>Held :   Moves used by the holder that trap and damage a target for multiple turns (e.g. []{move:bind}, []{move:fire-spin}) inflict twice their usual per-turn damage.</summery>			BINDING_BAND = 587,
            /// <summery>Held :   When the holder takes []{type:water}-type damage from a move, its [Special Attack]{mechanic:special-attack} rises by one [stage]{mechanic:stage} and this item is consumed.</summery>			ABSORB_BULB = 588,
            /// <summery>Held :   When the holder takes []{type:electric}-type damage from a move, its [Attack]{mechanic:attack} rises by one [stage]{mechanic:stage} and this item is consumed.</summery>			CELL_BATTERY = 589,
            /// <summery>Held :   When the holder takes damage directly from a move and does not faint, it [switches out]{mechanic:switching-out} for another non-fainted Pokémon in its party, as chosen by the Trainer. This effect does not activate if another effect prevents the holder from switching out.</summery>			EJECT_BUTTON = 590,
            /// <summery>Held :   When the holder uses a damaging []{type:fire}-type move, the move has 1.5× power and this item is consumed.</summery>			FIRE_GEM = 591,
            /// <summery>Held :   When the holder uses a damaging []{type:water}-type move, the move has 1.5× power and this item is consumed.</summery>			WATER_GEM = 592,
            /// <summery>Held :   When the holder uses a damaging []{type:electric}-type move, the move has 1.5× power and this item is consumed.</summery>			ELECTRIC_GEM = 593,
            /// <summery>Held :   When the holder uses a damaging []{type:grass}-type move, the move has 1.5× power and this item is consumed.</summery>			GRASS_GEM = 594,
            /// <summery>Held :   When the holder uses a damaging []{type:ice}-type move, the move has 1.5× power and this item is consumed.</summery>			ICE_GEM = 595,
            /// <summery>Held :   When the holder uses a damaging []{type:fighting}-type move, the move has 1.5× power and this item is consumed.</summery>			FIGHTING_GEM = 596,
            /// <summery>Held :   When the holder uses a damaging []{type:poison}-type move, the move has 1.5× power and this item is consumed.</summery>			POISON_GEM = 597,
            /// <summery>Held :   When the holder uses a damaging []{type:ground}-type move, the move has 1.5× power and this item is consumed.</summery>			GROUND_GEM = 598,
            /// <summery>Held :   When the holder uses a damaging []{type:flying}-type move, the move has 1.5× power and this item is consumed.</summery>			FLYING_GEM = 599,
            /// <summery>Held :   When the holder uses a damaging []{type:psychic}-type move, the move has 1.5× power and this item is consumed.</summery>			PSYCHIC_GEM = 600,
            /// <summery>Held :   When the holder uses a damaging []{type:bug}-type move, the move has 1.5× power and this item is consumed.</summery>			BUG_GEM = 601,
            /// <summery>Held :   When the holder uses a damaging []{type:rock}-type move, the move has 1.5× power and this item is consumed.</summery>			ROCK_GEM = 602,
            /// <summery>Held :   When the holder uses a damaging []{type:ghost}-type move, the move has 1.5× power and this item is consumed.</summery>			GHOST_GEM = 603,
            /// <summery>Held :   When the holder uses a damaging []{type:dark}-type move, the move has 1.5× power and this item is consumed.</summery>			DARK_GEM = 604,
            /// <summery>Held :   When the holder uses a damaging []{type:steel}-type move, the move has 1.5× power and this item is consumed.</summery>			STEEL_GEM = 605,
            /// <summery>Used on a party Pokémon :   Increases the target's [HP]{mechanic:hp} [effort]{mechanic:effort} by 1.</summery>			HEALTH_WING = 606,
            /// <summery>Used on a party Pokémon :   Increases the target's [Attack]{mechanic:attack} [effort]{mechanic:effort} by 1.</summery>			MUSCLE_WING = 607,
            /// <summery>Used on a party Pokémon :   Increases the target's [Defense]{mechanic:defense} [effort]{mechanic:effort} by 1.</summery>			RESIST_WING = 608,
            /// <summery>Used on a party Pokémon :   Increases the target's [Special Attack]{mechanic:special-attack} [effort]{mechanic:effort} by 1.</summery>			GENIUS_WING = 609,
            /// <summery>Used on a party Pokémon :   Increases the target's [Special Defense]{mechanic:special-defense} [effort]{mechanic:effort} by 1.</summery>			CLEVER_WING = 610,
            /// <summery>Used on a party Pokémon :   Increases the target's [Speed]{mechanic:speed} [effort]{mechanic:effort} by 1.</summery>			SWIFT_WING = 611,
            /// <summery>Vendor trash.</summery>			PRETTY_WING = 612,
            /// <summery>Give to a scientist in a museum to receive a []{pokemon:tirtouga}.</summery>			COVER_FOSSIL = 613,
            /// <summery>Give to a scientist in a museum to receive a []{pokemon:archen}.</summery>			PLUME_FOSSIL = 614,
            /// <summery>Allows passage on the []{location:castelia-city} ship, which leads to []{location:liberty-garden} and []{pokemon:victini}.</summery>			LIBERTY_PASS = 615,
            /// <summery>Acts as currency to activate Pass Powers in the Entralink.</summery>			PASS_ORB = 616,
            /// <summery>Can only be used in Entree Forest, to catch Pokémon encountered in the Dream World.  Used in battle :   [Catches]{mechanic:catch} a wild Pokémon without fail.</summery>			DREAM_BALL = 617,
            /// <summery>Used in battle :   Ends a wild battle.  Cannot be used in trainer battles.</summery>			POKE_TOY = 618,
            /// <summery>Stores props for the Pokémon Musical.</summery>			PROP_CASE = 619,
            /// <summery>Only used as a plot point; this item is given to the player and taken away in the same cutscene.</summery>			DRAGON_SKULL = 620,
            /// <summery>Cult vendor trash.</summery>			BALM_MUSHROOM = 621,
            /// <summery>Cult vendor trash.</summery>			BIG_NUGGET = 622,
            /// <summery>Cult vendor trash.</summery>			PEARL_STRING = 623,
            /// <summery>Cult vendor trash.</summery>			COMET_SHARD = 624,
            /// <summery>Cult vendor trash.</summery>			RELIC_COPPER = 625,
            /// <summery>Cult vendor trash.</summery>			RELIC_SILVER = 626,
            /// <summery>Cult vendor trash.</summery>			RELIC_GOLD = 627,
            /// <summery>Cult vendor trash.</summery>			RELIC_VASE = 628,
            /// <summery>Cult vendor trash.</summery>			RELIC_BAND = 629,
            /// <summery>Cult vendor trash.</summery>			RELIC_STATUE = 630,
            /// <summery>Cult vendor trash.</summery>			RELIC_CROWN = 631,
            /// <summery>Used on a party Pokémon :   Cures any [status ailment]{mechanic:status-ailment} and [confusion]{mechanic:confusion}.</summery>			CASTELIACONE = 632,
            /// <summery>Used on a party Pokémon in battle :   Raises the target's [critical hit]{mechanic:critical-hit} rate by two [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summery>			DIRE_HIT_2 = 633,
            /// <summery>Used on a party Pokémon in battle :   Raises the target's [Speed]{mechanic:speed} by two [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summery>			X_SPEED_2 = 634,
            /// <summery>Used on a party Pokémon in battle :   Raises the target's [Special Attack]{mechanic:special-attack} by two [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summery>			X_SP_ATK_2 = 635,
            /// <summery>Used on a party Pokémon in battle :   Raises the target's [Special Defense]{mechanic:special-defense} by two [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summery>			X_SP_DEF_2 = 636,
            /// <summery>Used on a party Pokémon in battle :   Raises the target's [Defense]{mechanic:defense} by two [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summery>			X_DEFENSE_2 = 637,
            /// <summery>Used on a party Pokémon in battle :   Raises the target's [Attack]{mechanic:attack} by two [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summery>			X_ATTACK_2 = 638,
            /// <summery>Used on a party Pokémon in battle :   Raises the target's [accuracy]{mechanic:accuracy} by two [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summery>			X_ACCURACY_2 = 639,
            /// <summery>Used on a party Pokémon in battle :   Raises the target's [Speed]{mechanic:speed} by three [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summery>			X_SPEED_3 = 640,
            /// <summery>Used on a party Pokémon in battle :   Raises the target's [Special Attack]{mechanic:special-attack} by three [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summery>			X_SP_ATK_3 = 641,
            /// <summery>Used on a party Pokémon in battle :   Raises the target's [Special Defense]{mechanic:special-defense} by three [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summery>			X_SP_DEF_3 = 642,
            /// <summery>Used on a party Pokémon in battle :   Raises the target's [Defense]{mechanic:defense} by three [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summery>			X_DEFENSE_3 = 643,
            /// <summery>Used on a party Pokémon in battle :   Raises the target's [Attack]{mechanic:attack} by three [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summery>			X_ATTACK_3 = 644,
            /// <summery>Used on a party Pokémon in battle :   Raises the target's [accuracy]{mechanic:accuracy} by three [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summery>			X_ACCURACY_3 = 645,
            /// <summery>Used on a party Pokémon in battle :   Raises the target's [Speed]{mechanic:speed} by six [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summery>			X_SPEED_6 = 646,
            /// <summery>Used on a party Pokémon in battle :   Raises the target's [Special Attack]{mechanic:special-attack} by six [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summery>			X_SP_ATK_6 = 647,
            /// <summery>Used on a party Pokémon in battle :   Raises the target's [Special Defense]{mechanic:special-defense} by six [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summery>			X_SP_DEF_6 = 648,
            /// <summery>Used on a party Pokémon in battle :   Raises the target's [Defense]{mechanic:defense} by six [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summery>			X_DEFENSE_6 = 649,
            /// <summery>Used on a party Pokémon in battle :   Raises the target's [Attack]{mechanic:attack} by six [stages]{mechanic:stage}.  This item can only be obtained or used via the Wonder Launcher.</summery>			X_ATTACK_6 = 650,
            /// <summery>Used on a party Pokémon in battle :   Raises the target's [accuracy]{mechanic:accuracy} by six [stages]{mechanic:stage}.  This item can only be obtained or used via the Wonder Launcher.</summery>			X_ACCURACY_6 = 651,
            /// <summery>Used on a party Pokémon in battle :   Selects another friendly Pokémon at random.  If that Pokémon's ability is normally activated by some condition—i.e., is not continuous and passive—its effect is forcibly activated.  This item can only be obtained or used via the Wonder Launcher.</summery>			ABILITY_URGE = 652,
            /// <summery>Used on a party Pokémon in battle :   Selects another friendly Pokémon at random.  If that Pokémon is holding an item, that item is removed for the duration of the battle.  This item can only be obtained or used via the Wonder Launcher.</summery>			ITEM_DROP = 653,
            /// <summery>Used on a party Pokémon in battle :   Selects another friendly Pokémon at random.  If that Pokémon is holding an item normally activated by some condition—i.e., not continuous and passive—its effect is forcibly activated.  This item can only be obtained or used via the Wonder Launcher.</summery>			ITEM_URGE = 654,
            /// <summery>Used on a party Pokémon in battle :   Selects another friendly Pokémon at random.  Removes all of that Pokémon's stat changes.  This item can only be obtained or used via the Wonder Launcher.</summery>			RESET_URGE = 655,
            /// <summery>Used on a party Pokémon in battle :   Raises the target's [critical hit]{mechanic:critical-hit} rate by three [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summery>			DIRE_HIT_3 = 656,
            /// <summery>Summons []{pokemon:reshiram} for the final battle against N.</summery>			LIGHT_STONE = 657,
            /// <summery>Summons []{pokemon:zekrom} for the final battle against N.</summery>			DARK_STONE = 658,
            /// <summery>Teaches []{move:wild-charge} to a compatible Pokémon.</summery>			TM93 = 659,
            /// <summery>Teaches []{move:rock-smash} to a compatible Pokémon.</summery>			TM94 = 660,
            /// <summery>Teaches []{move:snarl} to a compatible Pokémon.</summery>			TM95 = 661,
            /// <summery>Makes four-way video calls.  Used for plot advancement in-game, but also works with other players via the C-Gear.</summery>			XTRANSCEIVER = 662,
            /// <summery>Unknown.  Currently unused.</summery>			GOD_STONE = 663,
            /// <summery>Give to the []{pokemon:wingull} on []{location:unova-route-13}, along with []{item:gram-2} and []{item:gram-3}, to receive []{item:tm89}.</summery>			GRAM_1 = 664,
            /// <summery>Give to the []{pokemon:wingull} on []{location:unova-route-13}, along with []{item:gram-1} and []{item:gram-3}, to receive []{item:tm89}.</summery>			GRAM_2 = 665,
            /// <summery>Give to the []{pokemon:wingull} on []{location:unova-route-13}, along with []{item:gram-1} and []{item:gram-2}, to receive []{item:tm89}.</summery>			GRAM_3 = 666,
            /// <summery>Held :   When the holder uses a damaging []{type:dragon}-type move, the move has 1.5× power and this item is consumed.</summery>			DRAGON_GEM = 668,
            /// <summery>Held :   When the holder uses a damaging []{type:normal}-type move, the move has 1.5× power and this item is consumed.</summery>			NORMAL_GEM = 669,
            /// <summery>Holds medals recieved in the medal rally.</summery>			MEDAL_BOX = 670,
            /// <summery>Fuses Kyurem with Reshiram or Zekrom, or splits them apart again.</summery>			DNA_SPLICERS = 671,
            /// <summery>Grants access to the Nature Preserve.</summery>			PERMIT = 673,
            /// <summery>Doubles the chance of two Pokémon producing an egg at the daycare every 255 steps.</summery>			OVAL_CHARM = 674,
            /// <summery>Raises the chance of finding a shiny Pokémon.</summery>			SHINY_CHARM = 675,
            /// <summery>Required to progress in the Plasma Frigate.</summery>			PLASMA_CARD = 676,
            /// <summery>Appears in the Café Warehouse on Sunday; return to the customer with a Patrat on Thursday.</summery>			GRUBBY_HANKY = 677,
            /// <summery>Wakes up the Crustle blocking the way in Seaside Cave.</summery>			COLRESS_MACHINE = 678,
            /// <summery>Returned to Curtis or Yancy as part of a sidequest.</summery>			DROPPED_ITEM = 679,
            /// <summery>Switches Tornadus, Thundurus, and Landorus between Incarnate and Therian Forme.</summery>			REVEAL_GLASS = 681,
            /// <summery>An item to be held by a Pokémon. Attack and Sp. Atk sharply increase if the holder is hit with a move it’s weak to.</summery>			WEAKNESS_POLICY = 682,
            /// <summery>An item to be held by a Pokémon. This offensive vest raises Sp. Def but prevents the use of status moves.</summery>			ASSAULT_VEST = 683,
            /// <summery>An item to be held by a Pokémon. It is a stone tablet that boosts the power of Fairy-type moves.</summery>			PIXIE_PLATE = 684,
            /// <summery>A capsule that allows a Pokémon with two Abilities to switch between these Abilities when it is used.</summery>			ABILITY_CAPSULE = 685,
            /// <summery>A soft and sweet treat made of fluffy, puffy, whipped and whirled cream. It’s loved by a certain Pokémon.</summery>			WHIPPED_DREAM = 686,
            /// <summery>A sachet filled with fragrant perfumes that are just slightly too overwhelming. Yet it’s loved by a certain Pokémon.</summery>			SACHET = 687,
            /// <summery>An item to be held by a Pokémon. It boosts Sp. Def if hit with a Water- type attack. It can only be used once.</summery>			LUMINOUS_MOSS = 688,
            /// <summery>An item to be held by a Pokémon. It boosts Attack if hit with an Ice-type attack. It can only be used once.</summery>			SNOWBALL = 689,
            /// <summery>An item to be held by a Pokémon. These goggles protect the holder from both weather-related damage and powder.</summery>			SAFETY_GOGGLES = 690,
            /// <summery>Mulch to be used in a Berry field. It increases the Berry harvest without the need for particularly diligent care.</summery>			RICH_MULCH = 691,
            /// <summery>Mulch to be used in a Berry field. It causes strange, sudden mutations based on the combination of Berries.</summery>			SURPRISE_MULCH = 692,
            /// <summery>Mulch to be used in a Berry field. It increases the Berry harvest that can be grown by diligent watering.</summery>			BOOST_MULCH = 693,
            /// <summery>Mulch to be used in a Berry field. An amazing Mulch with the effects of Rich, Surprise, and Boost Mulch.</summery>			AMAZE_MULCH = 694,
            /// <summery>One variety of the mysterious Mega Stones. Have Gengar hold it, and this stone will enable it to Mega Evolve during battle.</summery>			GENGARITE = 695,
            /// <summery>One variety of the mysterious Mega Stones. Have Gardevoir hold it, and this stone will enable it to Mega Evolve during battle.</summery>			GARDEVOIRITE = 696,
            /// <summery>One variety of the mysterious Mega Stones. Have Ampharos hold it, and this stone will enable it to Mega Evolve during battle.</summery>			AMPHAROSITE = 697,
            /// <summery>One variety of the mysterious Mega Stones. Have Venusaur hold it, and this stone will enable it to Mega Evolve during battle.</summery>			VENUSAURITE = 698,
            /// <summery>One variety of the mysterious Mega Stones. Have Charizard hold it, and this stone will enable it to Mega Evolve during battle.</summery>			CHARIZARDITE_X = 699,
            /// <summery>One variety of the mysterious Mega Stones. Have Blastoise hold it, and this stone will enable it to Mega Evolve during battle.</summery>			BLASTOISINITE = 700,
            /// <summery>One variety of the mysterious Mega Stones. Have Mewtwo hold it, and this stone will enable it to Mega Evolve during battle.</summery>			MEWTWONITE_X = 701,
            /// <summery>One variety of the mysterious Mega Stones. Have Mewtwo hold it, and this stone will enable it to Mega Evolve during battle.</summery>			MEWTWONITE_Y = 702,
            /// <summery>One variety of the mysterious Mega Stones. Have Blaziken hold it, and this stone will enable it to Mega Evolve during battle.</summery>			BLAZIKENITE = 703,
            /// <summery>One variety of the mysterious Mega Stones. Have Medicham hold it, and this stone will enable it to Mega Evolve during battle.</summery>			MEDICHAMITE = 704,
            /// <summery>One variety of the mysterious Mega Stones. Have Houndoom hold it, and this stone will enable it to Mega Evolve during battle.</summery>			HOUNDOOMINITE = 705,
            /// <summery>One variety of the mysterious Mega Stones. Have Aggron hold it, and this stone will enable it to Mega Evolve during battle.</summery>			AGGRONITE = 706,
            /// <summery>One variety of the mysterious Mega Stones. Have Banette hold it, and this stone will enable it to Mega Evolve during battle.</summery>			BANETTITE = 707,
            /// <summery>One variety of the mysterious Mega Stones. Have Tyranitar hold it, and this stone will enable it to Mega Evolve during battle.</summery>			TYRANITARITE = 708,
            /// <summery>One variety of the mysterious Mega Stones. Have Scizor hold it, and this stone will enable it to Mega Evolve during battle.</summery>			SCIZORITE = 709,
            /// <summery>One variety of the mysterious Mega Stones. Have Pinsir hold it, and this stone will enable it to Mega Evolve during battle.</summery>			PINSIRITE = 710,
            /// <summery>One variety of the mysterious Mega Stones. Have Aerodactyl hold it, and this stone will enable it to Mega Evolve during battle.</summery>			AERODACTYLITE = 711,
            /// <summery>One variety of the mysterious Mega Stones. Have Lucario hold it, and this stone will enable it to Mega Evolve during battle.</summery>			LUCARIONITE = 712,
            /// <summery>One variety of the mysterious Mega Stones. Have Abomasnow hold it, and this stone will enable it to Mega Evolve during battle.</summery>			ABOMASITE = 713,
            /// <summery>One variety of the mysterious Mega Stones. Have Kangaskhan hold it, and this stone will enable it to Mega Evolve during battle.</summery>			KANGASKHANITE = 714,
            /// <summery>One variety of the mysterious Mega Stones. Have Gyarados hold it, and this stone will enable it to Mega Evolve during battle.</summery>			GYARADOSITE = 715,
            /// <summery>One variety of the mysterious Mega Stones. Have Absol hold it, and this stone will enable it to Mega Evolve during battle.</summery>			ABSOLITE = 716,
            /// <summery>One variety of the mysterious Mega Stones. Have Charizard hold it, and this stone will enable it to Mega Evolve during battle.</summery>			CHARIZARDITE_Y = 717,
            /// <summery>One variety of the mysterious Mega Stones. Have Alakazam hold it, and this stone will enable it to Mega Evolve during battle.</summery>			ALAKAZITE = 718,
            /// <summery>One variety of the mysterious Mega Stones. Have Heracross hold it, and this stone will enable it to Mega Evolve during battle.</summery>			HERACRONITE = 719,
            /// <summery>One variety of the mysterious Mega Stones. Have Mawile hold it, and this stone will enable it to Mega Evolve during battle.</summery>			MAWILITE = 720,
            /// <summery>One variety of the mysterious Mega Stones. Have Manectric hold it, and this stone will enable it to Mega Evolve during battle.</summery>			MANECTITE = 721,
            /// <summery>One variety of the mysterious Mega Stones. Have Garchomp hold it, and this stone will enable it to Mega Evolve during battle.</summery>			GARCHOMPITE = 722,
            /// <summery>If held by a Pokémon, this Berry will lessen the damage taken from one supereffective Fairy-type attack.</summery>			ROSELI_BERRY = 723,
            /// <summery>If held by a Pokémon, this Berry will increase the holder’s Defense if it’s hit with a physical move.</summery>			KEE_BERRY = 724,
            /// <summery>If held by a Pokémon, this Berry will increase the holder’s Sp. Def if it’s hit with a special move.</summery>			MARANGA_BERRY = 725,
            /// <summery>This special coupon allows you to buy items at a discount when you are shopping at a boutique.</summery>			DISCOUNT_COUPON = 726,
            /// <summery>An ornament depicting a Pokémon that is venerated as a protector in some region far from Kalos.</summery>			STRANGE_SOUVENIR = 727,
            /// <summery>A popular treat in Lumiose City. It can be used once to heal all the status conditions of a Pokémon.</summery>			LUMIOSE_GALETTE = 728,
            /// <summery>A fossil from a prehistoric Pokémon that once lived on the land. It looks as if it could be a piece of a large jaw.</summery>			JAW_FOSSIL = 729,
            /// <summery>A fossil from a prehistoric Pokémon that once lived on the land. It looks like the impression from a skin sail.</summery>			SAIL_FOSSIL = 730,
            /// <summery>A gem with an essence of the fey. When held, it strengthens the power of a Fairy-type move one time.</summery>			FAIRY_GEM = 731,
            /// <summery>This book contains all the points a new Trainer needs to know on a journey. It was handmade by a kind friend.</summery>			ADVENTURE_RULES = 732,
            /// <summery>A card key that activates the elevator in Lysandre Labs. It is emblazoned with Team Flare’s logo.</summery>			ELEVATOR_KEY = 733,
            /// <summery>A device that allows users to receive and view hologram clips at any time. It is also used to chat with others.</summery>			HOLO_CASTER = 734,
            /// <summery>A precious symbol that is awarded only to an individual who has done great things for the Kalos region.</summery>			HONOR_OF_KALOS = 735,
            /// <summery>A rather curious stone that might appear to be valuable to some. It’s all in the eye of the beholder.</summery>			INTRIGUING_STONE = 736,
            /// <summery>A rather chic-looking case for carrying contact lenses.</summery>			LENS_CASE = 737,
            /// <summery>A ticket that was handmade by Looker. It’s decorated with a liberal amount of glittery paint.</summery>			LOOKER_TICKET = 738,
            /// <summery>This ring contains an untold power that somehow enables Pokémon carrying Mega Stones to Mega Evolve in battle.</summery>			MEGA_RING = 739,
            /// <summery>This pass serves as an ID card for gaining access to the power plant that lies along Route 13.</summery>			POWER_PLANT_PASS = 740,
            /// <summery>A letter that Professor Sycamore wrote to your mother. A faint but pleasant perfume seems to cling to the paper.</summery>			PROFS_LETTER = 741,
            /// <summery>Attaches roller skates to the bottom of your shoes, allowing you to glide quickly around and perform tricks.</summery>			ROLLER_SKATES = 742,
            /// <summery>A watering can shaped like a Lotad. It helps promote the healthy growth of any Berries planted in good, soft soil.</summery>			SPRINKLOTAD = 743,
            /// <summery>A commuter pass that allows the holder to ride the TMV between Lumiose City and Kiloude City at any time.</summery>			TMV_PASS = 744,
            /// <summery>An attack that makes use of nature’s power. Its effects vary depending on the user’s environment.</summery>			TM96 = 745,
            /// <summery>The user releases a horrible aura imbued with dark thoughts. This may also make the target flinch.</summery>			TM97 = 746,
            /// <summery>Striking opponents over and over makes the user’s fists harder. Hitting a target raises the Attack stat.</summery>			TM98 = 747,
            /// <summery>The user damages opposing Pokémon by emitting a powerful flash.</summery>			TM99 = 748,
            /// <summery>The user tells the target a secret, and the target loses its ability to concentrate. This lowers the target’s Sp. Atk stat.</summery>			TM100 = 749,
        }
    }
//}