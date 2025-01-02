﻿namespace PokemonUnity.Inventory
{
	/// <summary>
	/// Item ids are connected to XML file. 
	/// </summary>
	/// <remarks>
	/// Running off of genVI. 
	/// Be sure to overwrite both if modifying.
	/// Replace "[HP]{mechanic:hp}" in summary-tags with
	/// "<see cref="Pokemon.HP"/>" or "<see cref="Pokemon.TotalHP"/>"
	/// </remarks>
	/// Custom Values are in negative?
	public enum Items
	{
		EXP_ALL_OFF = -3,
		EXP_ALL = -2,
		RUNNING_SHOES = -1,
		NONE = 0,
		/// <summary>Used in battle :   [Catches]{mechanic:catch} a wild Pokémon without fail.      If used in a trainer battle, nothing happens and the ball is lost.</summary>
		MASTER_BALL = 1,
		/// <summary>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon, using a catch rate of 2×.      If used in a trainer battle, nothing happens and the ball is lost.</summary>
		ULTRA_BALL = 2,
		/// <summary>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon, using a catch rate of 1.5×.      If used in a trainer battle, nothing happens and the ball is lost.</summary>
		GREAT_BALL = 3,
		/// <summary>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon, using a catch rate of 1×.      If used in a trainer battle, nothing happens and the ball is lost.</summary>
		POKE_BALL = 4,
		/// <summary>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon, using a catch rate of 1.5×.  This item can only be used in the []{location:great-marsh} or []{location:kanto-safari-zone}.</summary>
		SAFARI_BALL = 5,
		/// <summary>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.  If the wild Pokémon is []{type:water}- or []{type:bug}-type, this ball has a catch rate of 3×.  Otherwise, it has a catch rate of 1×.      If used in a trainer battle, nothing happens and the ball is lost.</summary>
		NET_BALL = 6,
		/// <summary>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.  If the wild Pokémon was encountered by surfing or fishing, this ball has a catch rate of 3.5×.  Otherwise, it has a catch rate of 1×.      If used in a trainer battle, nothing happens and the ball is lost.</summary>
		DIVE_BALL = 7,
		/// <summary>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.  Has a catch rate of given by `(40 - level) / 10`, where `level` is the wild Pokémon's level, to a maximum of 3.9× for level 1 Pokémon.  If the wild Pokémon's level is higher than 30, this ball has a catch rate of 1×.      If used in a trainer battle, nothing happens and the ball is lost.</summary>
		NEST_BALL = 8,
		/// <summary>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.  If the wild Pokémon's species is marked as caught in the trainer's Pokédex, this ball has a catch rate of 3×.  Otherwise, it has a catch rate of 1×.      If used in a trainer battle, nothing happens and the ball is lost.</summary>
		REPEAT_BALL = 9,
		/// <summary>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.  Has a catch rate of 1.1× on the first turn of the battle and increases by 0.1× every turn, to a maximum of 4× on turn 30.      If used in a trainer battle, nothing happens and the ball is lost.</summary>
		TIMER_BALL = 10,
		/// <summary>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon, using a catch rate of 1×.  Whenever the caught Pokémon's [happiness]{mechanic:happiness} increases, it increases by one extra point.      If used in a trainer battle, nothing happens and the ball is lost.</summary>
		LUXURY_BALL = 11,
		/// <summary>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon, using a catch rate of 1×.      If used in a trainer battle, nothing happens and the ball is lost.</summary>
		PREMIER_BALL = 12,
		/// <summary>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.  If it's currently nighttime or the wild Pokémon was encountered while walking in a cave, this ball has a catch rate of 3.5×.  Otherwise, it has a catch rate of 1×.      If used in a trainer battle, nothing happens and the ball is lost.</summary>
		DUSK_BALL = 13,
		/// <summary>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon, using a catch rate of 1×.  The caught Pokémon's [HP]{mechanic:hp} is immediately restored, [PP]{mechanic:pp} for all its moves is restored, and any [status ailment]{mechanic:status-ailment} is cured.      If used in a trainer battle, nothing happens and the ball is lost.</summary>
		HEAL_BALL = 14,
		/// <summary>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon, using a catch rate of 4× on the first turn of a battle, but 1× any other time.      If used in a trainer battle, nothing happens and the ball is lost.</summary>
		QUICK_BALL = 15,
		/// <summary>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon, using a catch rate of 1×.      If used in a trainer battle, nothing happens and the ball is lost.</summary>
		CHERISH_BALL = 16,
		/// <summary>Used on a friendly Pokémon :   Restores 20 [HP]{mechanic:hp}.</summary>
		POTION = 17,
		/// <summary>Used on a party Pokémon :   Cures [poison]{mechanic:poison}.</summary>
		ANTIDOTE = 18,
		/// <summary>Used on a party Pokémon :   Cures a [burn]{mechanic:burn}.</summary>
		BURN_HEAL = 19,
		/// <summary>Used on a party Pokémon :   Cures [freezing]{mechanic:freezing}.</summary>
		ICE_HEAL = 20,
		/// <summary>Used on a party Pokémon :   Cures [sleep]{mechanic:sleep}.</summary>
		AWAKENING = 21,
		/// <summary>Used on a party Pokémon :   Cures [paralysis]{mechanic:paralysis}.</summary>
		PARALYZE_HEAL = 22,
		/// <summary>Used on a party Pokémon :   Restores [HP]{mechanic:hp} to full and cures any [status ailment]{mechanic:status-ailment} and [confusion]{mechanic:confusion}.</summary>
		FULL_RESTORE = 23,
		/// <summary>Used on a party Pokémon :   Restores [HP]{mechanic:hp} to full.</summary>
		MAX_POTION = 24,
		/// <summary>Used on a party Pokémon :   Restores 200 [HP]{mechanic:hp}.</summary>
		HYPER_POTION = 25,
		/// <summary>Used on a party Pokémon :   Restores 50 [HP]{mechanic:hp}.</summary>
		SUPER_POTION = 26,
		/// <summary>Used on a party Pokémon :   Cures any [status ailment]{mechanic:status-ailment} and [confusion]{mechanic:confusion}.</summary>
		FULL_HEAL = 27,
		/// <summary>Used on a party Pokémon :   Revives the Pokémon and restores half its [HP]{mechanic:hp}.</summary>
		REVIVE = 28,
		/// <summary>Used on a party Pokémon :   Revives the Pokémon and restores its [HP]{mechanic:hp} to full.</summary>
		MAX_REVIVE = 29,
		/// <summary>Used on a party Pokémon :   Restores 50 [HP]{mechanic:hp}.</summary>
		FRESH_WATER = 30,
		/// <summary>Used on a party Pokémon :   Restores 60 [HP]{mechanic:hp}.</summary>
		SODA_POP = 31,
		/// <summary>Used on a party Pokémon :   Restores 80 [HP]{mechanic:hp}.</summary>
		LEMONADE = 32,
		/// <summary>Used on a party Pokémon :   Restores 100 [HP]{mechanic:hp}.</summary>
		MOOMOO_MILK = 33,
		/// <summary>Used on a party Pokémon :   Restores 50 [HP]{mechanic:hp}.  Decreases [happiness]{mechanic:happiness} by 5/5/10.</summary>
		ENERGY_POWDER = 34,
		/// <summary>Used on a party Pokémon :   Restores 200 [HP]{mechanic:hp}.  Decreases [happiness]{mechanic:happiness} by 10/10/15.</summary>
		ENERGY_ROOT = 35,
		/// <summary>Used on a party Pokémon :   Cures any [status ailment]{mechanic:status-ailment}.  Decreases [happiness]{mechanic:happiness} by 5/5/10.</summary>
		HEAL_POWDER = 36,
		/// <summary>Used on a party Pokémon :   Revives a [fainted]{mechanic:faint} Pokémon and restores its [HP]{mechanic:hp} to full.  Decreases [happiness]{mechanic:happiness} by 10/10/15.</summary>
		REVIVAL_HERB = 37,
		/// <summary>Used on a party Pokémon :   Restores 10 [PP]{mechanic:pp} for a selected move.</summary>
		ETHER = 38,
		/// <summary>Used on a party Pokémon :   Restores [PP]{mechanic:pp} to full for a selected move.</summary>
		MAX_ETHER = 39,
		/// <summary>Used on a party Pokémon :   Restores 10 [PP]{mechanic:pp} for each move.</summary>
		ELIXIR = 40,
		/// <summary>Used on a party Pokémon :   Restores [PP]{mechanic:pp} to full for each move.</summary>
		MAX_ELIXIR = 41,
		/// <summary>Used on a party Pokémon :   Cures any [status ailment]{mechanic:status-ailment} and [confusion]{mechanic:confusion}.</summary>
		LAVA_COOKIE = 42,
		/// <summary>Used on a party Pokémon :   Restores 20 [HP]{mechanic:hp}.</summary>
		BERRY_JUICE = 43,
		/// <summary>Used :   Revives all [fainted]{mechanic:faint} Pokémon in the party and restores their [HP]{mechanic:hp} to full.</summary>
		SACRED_ASH = 44,
		/// <summary>Used on a party Pokémon :   Increases [HP]{mechanic:hp} [effort]{mechanic:effort} by 10, but won't increase it beyond 100.  Increases [happiness]{mechanic:happiness} by 5/3/2.</summary>
		HP_UP = 45,
		/// <summary>Used on a party Pokémon :   Increases [Attack]{mechanic:attack} [effort]{mechanic:effort} by 10, but won't increase it beyond 100.  Increases [happiness]{mechanic:happiness} by 5/3/2.</summary>
		PROTEIN = 46,
		/// <summary>Used on a party Pokémon :   Increases [Defense]{mechanic:defense} [effort]{mechanic:effort} by 10, but won't increase it beyond 100.  Increases [happiness]{mechanic:happiness} by 5/3/2.</summary>
		IRON = 47,
		/// <summary>Used on a party Pokémon :   Increases [Speed]{mechanic:speed} [effort]{mechanic:effort} by 10, but won't increase it beyond 100.  Increases [happiness]{mechanic:happiness} by 5/3/2.</summary>
		CARBOS = 48,
		/// <summary>Used on a party Pokémon :   Increases [Special Attack]{mechanic:special-attack} [effort]{mechanic:effort} by 10, but won't increase it beyond 100.  Increases [happiness]{mechanic:happiness} by 5/3/2.</summary>
		CALCIUM = 49,
		/// <summary>Used on a party Pokémon :   Increases level by 1.  Increases [happiness]{mechanic:happiness} by 5/3/2.</summary>
		RARE_CANDY = 50,
		/// <summary>Used on a party Pokémon :   Increases a selected move's max [PP]{mechanic:pp} by 20% its original max PP, to a maximum of 1.6×.  Increases [happiness]{mechanic:happiness} by 5/3/2.</summary>
		PP_UP = 51,
		/// <summary>Used on a party Pokémon :   Increases [Special Defense]{mechanic:special-defense} [effort]{mechanic:effort} by 10, but won't increase it beyond 100.  Increases [happiness]{mechanic:happiness} by 5/3/2.</summary>
		ZINC = 52,
		/// <summary>Used on a party Pokémon :   Increases a selected move's max [PP]{mechanic:pp} to 1.6× its original max PP.  Increases [happiness]{mechanic:happiness} by 5/3/2.</summary>
		PP_MAX = 53,
		/// <summary>Used on a party Pokémon :   Cures any [status ailment]{mechanic:status-ailment} and [confusion]{mechanic:confusion}.</summary>
		OLD_GATEAU = 54,
		/// <summary>Used on a party Pokémon in battle :   Protects the target's stats from being [lowered]{mechanic:lower} for the next five turns.  Increases happiness by 1/1/0.</summary>
		GUARD_SPEC = 55,
		/// <summary>Used on a party Pokémon in battle :   Increases the target's [critical hit chance]{mechanic:critical-hit-chance} by one stage until it leaves the field.  Increases happiness by 1/1/0.</summary>
		DIRE_HIT = 56,
		/// <summary>Used on a party Pokémon in battle :   [Raises]{mechanic:raise} the target's [Attack]{mechanic:attack} by one stage.  Increases happiness by 1/1/0.</summary>
		X_ATTACK = 57,
		/// <summary>Used on a party Pokémon in battle :   [Raises]{mechanic:raise} the target's [Defense]{mechanic:defense} by one stage.  Increases happiness by 1/1/0.</summary>
		X_DEFENSE = 58,
		/// <summary>Used on a party Pokémon in battle :   [Raises]{mechanic:raise} the target's [Speed]{mechanic:speed} by one stage.  Increases happiness by 1/1/0.</summary>
		X_SPEED = 59,
		/// <summary>Used on a party Pokémon in battle :   [Raises]{mechanic:raise} the target's [accuracy]{mechanic:accuracy} by one stage.  Increases happiness by 1/1/0.</summary>
		X_ACCURACY = 60,
		/// <summary>Used on a party Pokémon in battle :   [Raises]{mechanic:raise} the target's [Special Attack]{mechanic:special-attack} by one stage.  Increases happiness by 1/1/0.</summary>
		X_SP_ATK = 61,
		/// <summary>Used on a party Pokémon in battle :   [Raises]{mechanic:raise} the target's [Special Defense]{mechanic:special-defense} by one stage.  Increases happiness by 1/1/0.</summary>
		X_SP_DEF = 62,
		/// <summary>Used in battle :   Ends a wild battle.  Cannot be used in trainer battles.</summary>
		POKE_DOLL = 63,
		/// <summary>Used in battle :   Ends a wild battle.  Cannot be used in trainer battles.</summary>
		FLUFFY_TAIL = 64,
		/// <summary>Used on a party Pokémon :   Cures [sleep]{mechanic:sleep}.</summary>
		BLUE_FLUTE = 65,
		/// <summary>Used on a party Pokémon in battle :   Cures [confusion]{mechanic:confusion}.</summary>
		YELLOW_FLUTE = 66,
		/// <summary>Used on a party Pokémon in battle :   Cures [attraction]{mechanic:attraction}.</summary>
		RED_FLUTE = 67,
		/// <summary>Used outside of battle :   Decreases the wild Pokémon encounter rate by 50%.</summary>
		BLACK_FLUTE = 68,
		/// <summary>Used outside of battle :   Doubles the wild Pokémon encounter rate.</summary>
		WHITE_FLUTE = 69,
		/// <summary>No effect.</summary>
		SHOAL_SALT = 70,
		/// <summary>No effect.</summary>
		SHOAL_SHELL = 71,
		/// <summary>No effect.  In Diamond and Pearl, trade ten for a []{move:sunny-day} [TM]{item:tm11} in the house midway along the southern section of []{location:sinnoh-route-212}.  In Platinum, trade to [move tutors]{mechanic:move-tutor} on []{location:sinnoh-route-212}, in []{location:snowpoint-city}, and in the []{location:survival-area}.  Eight shards total are required per tutelage, but the particular combination of colors varies by move.  In HeartGold and SoulSilver, trade one for a []{item:cheri-berry}, a []{item:leppa-berry}, and a []{item:pecha-berry} with the Juggler near the Pokémon Center in []{location:violet-city}.  In HeartGold and SoulSilver, trade one for a []{item:persim-berry}, a []{item:pomeg-berry}, and a []{item:razz-berry} with the Juggler near the []{location:pal-park} entrance in []{location:fuchsia-city}.</summary>
		RED_SHARD = 72,
		/// <summary>No effect.  In Diamond and Pearl, trade ten for a []{move:rain-dance} [TM]{item:tm18} in the house midway along the southern section of []{location:sinnoh-route-212}.  In Platinum, trade to [move tutors]{mechanic:move-tutor} on []{location:sinnoh-route-212}, in []{location:snowpoint-city}, and in the []{location:survival-area}.  Eight shards total are required per tutelage, but the particular combination of colors varies by move.  In HeartGold and SoulSilver, trade one for a []{item:chesto-berry}, an []{item:oran-berry}, and a []{item:wiki-berry} with the Juggler near the Pokémon Center in []{location:violet-city}.  In HeartGold and SoulSilver, trade one for a []{item:bluk-berry}, a []{item:cornn-berry}, and a []{item:kelpsy-berry} with the Juggler near the []{location:pal-park} entrance in []{location:fuchsia-city}.</summary>
		BLUE_SHARD = 73,
		/// <summary>No effect.  In Diamond and Pearl, trade ten for a []{move:sandstorm} [TM]{item:tm37} in the house midway along the southern section of []{location:sinnoh-route-212}.  In Platinum, trade to [move tutors]{mechanic:move-tutor} on []{location:sinnoh-route-212}, in []{location:snowpoint-city}, and in the []{location:survival-area}.  Eight shards total are required per tutelage, but the particular combination of colors varies by move.  In HeartGold and SoulSilver, trade one for an []{item:aspear-berry}, a []{item:iapapa-berry}, and a []{item:sitrus-berry} with the Juggler near the Pokémon Center in []{location:violet-city}.  In HeartGold and SoulSilver, trade one for a []{item:grepa-berry}, a []{item:nomel-berry}, and a []{item:pinap-berry} with the Juggler near the []{location:pal-park} entrance in []{location:fuchsia-city}.</summary>
		YELLOW_SHARD = 74,
		/// <summary>No effect.  In Diamond and Pearl, trade ten for a []{move:hail} [TM]{item:tm07} in the house midway along the southern section of []{location:sinnoh-route-212}.  In Platinum, trade to [move tutors]{mechanic:move-tutor} on []{location:sinnoh-route-212}, in []{location:snowpoint-city}, and in the []{location:survival-area}.  Eight shards total are required per tutelage, but the particular combination of colors varies by move.  In HeartGold and SoulSilver, trade one for an []{item:aguav-berry}, a []{item:lum-berry}, and a []{item:rawst-berry} with the Juggler near the Pokémon Center in []{location:violet-city}.  In HeartGold and SoulSilver, trade one for a []{item:durin-berry}, a []{item:hondew-berry}, and a []{item:wepear-berry} with the Juggler near the []{location:pal-park} entrance in []{location:fuchsia-city}.</summary>
		GREEN_SHARD = 75,
		/// <summary>Used outside of battle :   Trainer will skip encounters with wild Pokémon of a lower level than the lead party Pokémon.  This effect wears off after the trainer takes 200 steps.</summary>
		SUPER_REPEL = 76,
		/// <summary>Used outside of battle :   Trainer will skip encounters with wild Pokémon of a lower level than the lead party Pokémon.  This effect wears off after the trainer takes 250 steps.</summary>
		MAX_REPEL = 77,
		/// <summary>Used outside of battle :   Transports the trainer to the last-entered dungeon entrance.  Cannot be used outside, in buildings, or in []{location:distortion-world}, []{location:sinnoh-hall-of-origin-1}, []{location:spear-pillar}, or []{location:turnback-cave}.</summary>
		ESCAPE_ROPE = 78,
		/// <summary>Used outside of battle :   Trainer will skip encounters with wild Pokémon of a lower level than the lead party Pokémon.  This effect wears off after the trainer takes 100 steps.</summary>
		REPEL = 79,
		/// <summary>Used on a party Pokémon :   Evolves a []{pokemon:cottonee} into []{pokemon:whimsicott}, a []{pokemon:gloom} into []{pokemon:bellossom}, a []{pokemon:petilil} into []{pokemon:lilligant}, or a []{pokemon:sunkern} into []{pokemon:sunflora}.</summary>
		SUN_STONE = 80,
		/// <summary>Used on a party Pokémon :   Evolves a []{pokemon:clefairy} into []{pokemon:clefable}, a []{pokemon:jigglypuff} into []{pokemon:wigglytuff}, a []{pokemon:munna} into []{pokemon:musharna}, a []{pokemon:nidorina} into []{pokemon:nidoqueen}, a []{pokemon:nidorino} into []{pokemon:nidoking}, or a []{pokemon:skitty} into []{pokemon:delcatty}.</summary>
		MOON_STONE = 81,
		/// <summary>Used on a party Pokémon :   Evolves an []{pokemon:eevee} into []{pokemon:flareon}, a []{pokemon:growlithe} into []{pokemon:arcanine}, a []{pokemon:pansear} into []{pokemon:simisear}, or a []{pokemon:vulpix} into []{pokemon:ninetales}.</summary>
		FIRE_STONE = 82,
		/// <summary>Used on a party Pokémon :   Evolves an []{pokemon:eelektrik} into []{pokemon:eelektross}, an []{pokemon:eevee} into []{pokemon:jolteon}, or a []{pokemon:pikachu} into []{pokemon:raichu}.</summary>
		THUNDER_STONE = 83,
		/// <summary>Used on a party Pokémon :   Evolves an []{pokemon:eevee} into []{pokemon:vaporeon}, a []{pokemon:lombre} into []{pokemon:ludicolo}, a []{pokemon:panpour} into []{pokemon:simipour}, a []{pokemon:poliwhirl} into []{pokemon:poliwrath}, a []{pokemon:shellder} into []{pokemon:cloyster}, or a []{pokemon:staryu} into []{pokemon:starmie}.</summary>
		WATER_STONE = 84,
		/// <summary>Used on a party Pokémon :   Evolves an []{pokemon:exeggcute} into []{pokemon:exeggutor}, a []{pokemon:gloom} into []{pokemon:vileplume}, a []{pokemon:nuzleaf} into []{pokemon:shiftry}, a []{pokemon:pansage} into []{pokemon:simisage}, or a []{pokemon:weepinbell} into []{pokemon:victreebel}.</summary>
		LEAF_STONE = 85,
		/// <summary>Vendor trash.</summary>
		TINY_MUSHROOM = 86,
		/// <summary>Vendor trash.</summary>
		BIG_MUSHROOM = 87,
		/// <summary>Vendor trash.</summary>
		PEARL = 88,
		/// <summary>Vendor trash.</summary>
		BIG_PEARL = 89,
		/// <summary>Vendor trash.</summary>
		STARDUST = 90,
		/// <summary>Vendor trash.</summary>
		STAR_PIECE = 91,
		/// <summary>Vendor trash.</summary>
		NUGGET = 92,
		/// <summary>Trade one to the Move Relearner near the shore in []{location:pastoria-city} or with the Move Deleter in []{location:blackthorn-city} to teach one party Pokémon a prior level-up move.</summary>
		HEART_SCALE = 93,
		/// <summary>Used outside of battle :   Immediately triggers a wild Pokémon battle, as long as the trainer is somewhere with wild Pokémon—i.e., in tall grass, in a cave, or surfing.  Can be smeared on sweet-smelling trees to attract tree-dwelling Pokémon after six hours.</summary>
		HONEY = 94,
		/// <summary>Used on a patch of soil :   Plant's growth stages will each last 25% less time.  Dries soil out more quickly.</summary>
		GROWTH_MULCH = 95,
		/// <summary>Used on a patch of soil :   Plant's growth stages will each last 25% more time.  Dries soil out more slowly.</summary>
		DAMP_MULCH = 96,
		/// <summary>Used on a patch of soil :   Fully-grown plant will last 25% longer before dying and possibly regrowing.</summary>
		STABLE_MULCH = 97,
		/// <summary>Used on a path of soil :   Plant will regrow after dying 25% more times.</summary>
		GOOEY_MULCH = 98,
		/// <summary>Give to a scientist in the []{location:mining-museum} in []{location:oreburgh-city} or the Museum of Science in []{location:pewter-city} to receive a []{pokemon:lileep}.</summary>
		ROOT_FOSSIL = 99,
		/// <summary>Give to a scientist in the []{location:mining-museum} in []{location:oreburgh-city} or the Museum of Science in []{location:pewter-city} to receive a []{pokemon:anorith}.</summary>
		CLAW_FOSSIL = 100,
		/// <summary>Give to a scientist in the []{location:mining-museum} in []{location:oreburgh-city} or the Museum of Science in []{location:pewter-city} to receive a []{pokemon:omanyte}.</summary>
		HELIX_FOSSIL = 101,
		/// <summary>Give to a scientist in the []{location:mining-museum} in []{location:oreburgh-city} or the Museum of Science in []{location:pewter-city} to receive a []{pokemon:kabuto}.</summary>
		DOME_FOSSIL = 102,
		/// <summary>Give to a scientist in the []{location:mining-museum} in []{location:oreburgh-city} or the Museum of Science in []{location:pewter-city} to receive a []{pokemon:aerodactyl}.</summary>
		OLD_AMBER = 103,
		/// <summary>Give to a scientist in the []{location:mining-museum} in []{location:oreburgh-city} or the Museum of Science in []{location:pewter-city} to receive a []{pokemon:shieldon}.</summary>
		ARMOR_FOSSIL = 104,
		/// <summary>Give to a scientist in the []{location:mining-museum} in []{location:oreburgh-city} or the Museum of Science in []{location:pewter-city} to receive a []{pokemon:cranidos}.</summary>
		SKULL_FOSSIL = 105,
		/// <summary>Vendor trash.</summary>
		RARE_BONE = 106,
		/// <summary>Used on a party Pokémon :   Evolves a []{pokemon:minccino} into []{pokemon:cinccino}, a []{pokemon:roselia} into []{pokemon:roserade}, or a []{pokemon:togetic} into []{pokemon:togekiss}.</summary>
		SHINY_STONE = 107,
		/// <summary>Used on a party Pokémon :   Evolves a []{pokemon:lampent} into []{pokemon:chandelure}, a []{pokemon:misdreavus} into []{pokemon:mismagius}, or a []{pokemon:murkrow} into []{pokemon:honchkrow}.</summary>
		DUSK_STONE = 108,
		/// <summary>Used on a party Pokémon :   Evolves a male []{pokemon:kirlia} into []{pokemon:gallade} or a female []{pokemon:snorunt} into []{pokemon:froslass}.</summary>
		DAWN_STONE = 109,
		/// <summary>Held by []{pokemon:happiny} :   Holder evolves into []{pokemon:chansey} when it levels up during the daytime.</summary>
		OVAL_STONE = 110,
		/// <summary>Place in the tower on []{location:sinnoh-route-209}.  Check the stone to encounter a []{pokemon:spiritomb}, as long as the trainer's Underground status card counts at least 32 greetings.</summary>
		ODD_KEYSTONE = 111,
		/// <summary>Held by []{pokemon:dialga} :   Holder's []{type:dragon}- and []{type:steel}-type moves have 1.2× their usual power.</summary>
		ADAMANT_ORB = 112,
		/// <summary>Held by []{pokemon:palkia} :   Holder's []{type:dragon}- and []{type:water}-type moves have 1.2× their usual power.</summary>
		LUSTROUS_ORB = 113,
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		GRASS_MAIL = 114,
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		FLAME_MAIL = 115,
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		BUBBLE_MAIL = 116,
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		BLOOM_MAIL = 117,
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		TUNNEL_MAIL = 118,
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		STEEL_MAIL = 119,
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		HEART_MAIL = 120,
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		SNOW_MAIL = 121,
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		SPACE_MAIL = 122,
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		AIR_MAIL = 123,
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		MOSAIC_MAIL = 124,
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		BRICK_MAIL = 125,
		/// <summary>Held in battle :   When the holder is [paralyzed]{mechanic:paralysis}, it consumes this item to cure the paralysis.  Used on a party Pokémon :   Cures [paralysis]{mechanic:paralysis}.</summary>
		CHERI_BERRY = 126,
		/// <summary>Held in battle :   When the holder is [asleep]{mechanic:sleep}, it consumes this item to wake up.  Used on a party Pokémon :   Cures [sleep]{mechanic:sleep}.</summary>
		CHESTO_BERRY = 127,
		/// <summary>Held in battle :   When the holder is [poisoned]{mechanic:poison}, it consumes this item to cure the poison.  Used on a party Pokémon :   Cures [poison]{mechanic:poison}.</summary>
		PECHA_BERRY = 128,
		/// <summary>Held in battle :   When the holder is [burned]{mechanic:burn}, it consumes this item to cure the burn.  Used on a party Pokémon :   Cures a [burn]{mechanic:burn}.</summary>
		RAWST_BERRY = 129,
		/// <summary>Held in battle :   When the holder is [frozen]{mechanic:freezing}, it consumes this item to thaw itself.  Used on a party Pokémon :   Cures [freezing]{mechanic:freezing}.</summary>
		ASPEAR_BERRY = 130,
		/// <summary>Held in battle :   When the holder is out of [PP]{mechanic:pp} for one of its moves, it consumes this item to restore 10 of that move's PP.  Used on a party Pokémon :   Restores 10 [PP]{mechanic:pp} for a selected move.</summary>
		LEPPA_BERRY = 131,
		/// <summary>Held in battle :   When the holder has 1/2 its max [HP]{mechanic:hp} remaining or less, it consumes this item to restore 10 HP.  Used on a party Pokémon :   Restores 10 [HP]{mechanic:hp}.</summary>
		ORAN_BERRY = 132,
		/// <summary>Held in battle :   When the holder is [confused]{mechanic:confusion}, it consumes this item to cure the confusion.  Used on a party Pokémon :   Cures [confusion]{mechanic:confusion}.</summary>
		PERSIM_BERRY = 133,
		/// <summary>Held in battle :   When the holder is afflicted with a [major status ailment]{mechanic:major-status-ailment}, it consumes this item to cure the ailment.  Used on a party Pokémon :   Cures any [major status ailment]{mechanic:major-status-ailment}.</summary>
		LUM_BERRY = 134,
		/// <summary>Held in battle :   When the holder has 1/2 its max [HP]{mechanic:hp} remaining or less, it consumes this item to restore 1/4 its max HP.  Used on a party Pokémon :   Restores 1/4 the Pokémon's max [HP]{mechanic:hp}.</summary>
		SITRUS_BERRY = 135,
		/// <summary>Held in battle :   When the holder has 1/2 its max [HP]{mechanic:hp} remaining or less, it consumes this item to restore 1/8 its max HP.  If the holder dislikes spicy flavors (i.e., has a nature that lowers [Attack]{mechanic:attack}), it will also become [confused]{mechanic:confusion}.</summary>
		FIGY_BERRY = 136,
		/// <summary>Held in battle :   When the holder has 1/2 its max [HP]{mechanic:hp} remaining or less, it consumes this item to restore 1/8 its max HP.  If the holder dislikes dry flavors (i.e., has a nature that lowers [Special Attack]{mechanic:special-attack}), it will also become [confused]{mechanic:confusion}.</summary>
		WIKI_BERRY = 137,
		/// <summary>Held in battle :   When the holder has 1/2 its max [HP]{mechanic:hp} remaining or less, it consumes this item to restore 1/8 its max HP.  If the holder dislikes sweet flavors (i.e., has a nature that lowers [Speed]{mechanic:speed}), it will also become [confused]{mechanic:confusion}.</summary>
		MAGO_BERRY = 138,
		/// <summary>Held in battle :   When the holder has 1/2 its max [HP]{mechanic:hp} remaining or less, it consumes this item to restore 1/8 its max HP.  If the holder dislikes bitter flavors (i.e., has a nature that lowers [Special Defense]{mechanic:special-defense}), it will also become [confused]{mechanic:confusion}.</summary>
		AGUAV_BERRY = 139,
		/// <summary>Held in battle :   When the holder has 1/2 its max [HP]{mechanic:hp} remaining or less, it consumes this item to restore 1/8 its max HP.  If the holder dislikes sour flavors (i.e., has a nature that lowers [Defense]{mechanic:defense}), it will also become [confused]{mechanic:confusion}.</summary>
		IAPAPA_BERRY = 140,
		/// <summary>No effect; only useful for planting and cooking.</summary>
		RAZZ_BERRY = 141,
		/// <summary>No effect; only useful for planting and cooking.</summary>
		BLUK_BERRY = 142,
		/// <summary>No effect; only useful for planting and cooking.</summary>
		NANAB_BERRY = 143,
		/// <summary>No effect; only useful for planting and cooking.</summary>
		WEPEAR_BERRY = 144,
		/// <summary>No effect; only useful for planting and cooking.</summary>
		PINAP_BERRY = 145,
		/// <summary>Used on a party Pokémon :   Increases [happiness]{mechanic:happiness} by 10/5/2.  Lowers [HP]{mechanic:hp} [effort]{mechanic:effort} by 10.</summary>
		POMEG_BERRY = 146,
		/// <summary>Used on a party Pokémon :   Increases [happiness]{mechanic:happiness} by 10/5/2.  Lowers [Attack]{mechanic:attack} [effort]{mechanic:effort} by 10.</summary>
		KELPSY_BERRY = 147,
		/// <summary>Used on a party Pokémon :   Increases [happiness]{mechanic:happiness} by 10/5/2.  Lowers [Defense]{mechanic:defense} [effort]{mechanic:effort} by 10.</summary>
		QUALOT_BERRY = 148,
		/// <summary>Used on a party Pokémon :   Increases [happiness]{mechanic:happiness} by 10/5/2.  Lowers [Special Attack]{mechanic:special-attack} [effort]{mechanic:effort} by 10.</summary>
		HONDEW_BERRY = 149,
		/// <summary>Used on a party Pokémon :   Increases [happiness]{mechanic:happiness} by 10/5/2.  Lowers [Special Defense]{mechanic:special-defense} [effort]{mechanic:effort} by 10.</summary>
		GREPA_BERRY = 150,
		/// <summary>Used on a party Pokémon :   Increases [happiness]{mechanic:happiness} by 10/5/2.  Lowers [Speed]{mechanic:speed} [effort]{mechanic:effort} by 10.</summary>
		TAMATO_BERRY = 151,
		/// <summary>No effect; only useful for planting and cooking.</summary>
		CORNN_BERRY = 152,
		/// <summary>No effect; only useful for planting and cooking.</summary>
		MAGOST_BERRY = 153,
		/// <summary>No effect; only useful for planting and cooking.</summary>
		RABUTA_BERRY = 154,
		/// <summary>No effect; only useful for planting and cooking.</summary>
		NOMEL_BERRY = 155,
		/// <summary>No effect; only useful for planting and cooking.</summary>
		SPELON_BERRY = 156,
		/// <summary>No effect; only useful for planting and cooking.</summary>
		PAMTRE_BERRY = 157,
		/// <summary>No effect; only useful for planting and cooking.</summary>
		WATMEL_BERRY = 158,
		/// <summary>No effect; only useful for planting and cooking.</summary>
		DURIN_BERRY = 159,
		/// <summary>No effect; only useful for planting and cooking.</summary>
		BELUE_BERRY = 160,
		/// <summary>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:fire}-type damage, it consumes this item to halve the amount of damage taken.</summary>
		OCCA_BERRY = 161,
		/// <summary>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:water}-type damage, it consumes this item to halve the amount of damage taken.</summary>
		PASSHO_BERRY = 162,
		/// <summary>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:electric}-type damage, it consumes this item to halve the amount of damage taken.</summary>
		WACAN_BERRY = 163,
		/// <summary>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:grass}-type damage, it consumes this item to halve the amount of damage taken.</summary>
		RINDO_BERRY = 164,
		/// <summary>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:ice}-type damage, it consumes this item to halve the amount of damage taken.</summary>
		YACHE_BERRY = 165,
		/// <summary>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:fighting}-type damage, it consumes this item to halve the amount of damage taken.</summary>
		CHOPLE_BERRY = 166,
		/// <summary>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:poison}-type damage, it consumes this item to halve the amount of damage taken.</summary>
		KEBIA_BERRY = 167,
		/// <summary>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:ground}-type damage, it consumes this item to halve the amount of damage taken.</summary>
		SHUCA_BERRY = 168,
		/// <summary>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:flying}-type damage, it consumes this item to halve the amount of damage taken.</summary>
		COBA_BERRY = 169,
		/// <summary>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:psychic}-type damage, it consumes this item to halve the amount of damage taken.</summary>
		PAYAPA_BERRY = 170,
		/// <summary>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:bug}-type damage, it consumes this item to halve the amount of damage taken.</summary>
		TANGA_BERRY = 171,
		/// <summary>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:rock}-type damage, it consumes this item to halve the amount of damage taken.</summary>
		CHARTI_BERRY = 172,
		/// <summary>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:ghost}-type damage, it consumes this item to halve the amount of damage taken.</summary>
		KASIB_BERRY = 173,
		/// <summary>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:dragon}-type damage, it consumes this item to halve the amount of damage taken.</summary>
		HABAN_BERRY = 174,
		/// <summary>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:dark}-type damage, it consumes this item to halve the amount of damage taken.</summary>
		COLBUR_BERRY = 175,
		/// <summary>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:steel}-type damage, it consumes this item to halve the amount of damage taken.</summary>
		BABIRI_BERRY = 176,
		/// <summary>Held in battle :   When the holder would take []{type:normal}-type damage, it consumes this item to halve the amount of damage taken.</summary>
		CHILAN_BERRY = 177,
		/// <summary>Held in battle :   When the holder has 1/4 its max [HP]{mechanic:hp} remaining or less, it consumes this item to [raise]{mechanic:raise} its [Attack]{mechanic:attack} by one stage.</summary>
		LIECHI_BERRY = 178,
		/// <summary>Held in battle :   When the holder has 1/4 its max [HP]{mechanic:hp} remaining or less, it consumes this item to [raise]{mechanic:raise} its [Defense]{mechanic:defense} by one stage.</summary>
		GANLON_BERRY = 179,
		/// <summary>Held in battle :   When the holder has 1/4 its max [HP]{mechanic:hp} remaining or less, it consumes this item to [raise]{mechanic:raise} its [Speed]{mechanic:speed} by one stage.</summary>
		SALAC_BERRY = 180,
		/// <summary>Held in battle :   When the holder has 1/4 its max [HP]{mechanic:hp} remaining or less, it consumes this item to [raise]{mechanic:raise} its [Special Attack]{mechanic:special-attack} by one stage.</summary>
		PETAYA_BERRY = 181,
		/// <summary>Held in battle :   When the holder has 1/4 its max [HP]{mechanic:hp} remaining or less, it consumes this item to [raise]{mechanic:raise} its [Special Defense]{mechanic:special-defense} by one stage.</summary>
		APICOT_BERRY = 182,
		/// <summary>Held in battle :   When the holder has 1/4 its max [HP]{mechanic:hp} remaining or less, it consumes this item to [raise]{mechanic:raise} its [critical hit chance]{mechanic:critical-hit-chance} by one stage.</summary>
		LANSAT_BERRY = 183,
		/// <summary>Held in battle :   When the holder has 1/4 its max [HP]{mechanic:hp} remaining or less, it consumes this item to [raise]{mechanic:raise} a random stat by two stages.</summary>
		STARF_BERRY = 184,
		/// <summary>Held in battle :   When the holder takes [super-effective]{mechanic:super-effective} damage, it consumes this item to restore 1/4 its max [HP]{mechanic:hp}.</summary>
		ENIGMA_BERRY = 185,
		/// <summary>Held in battle :   When the holder has 1/4 its max [HP]{mechanic:hp} remaining or less, it consumes this item, and its next used move has 1.2× its normal accuracy.</summary>
		MICLE_BERRY = 186,
		/// <summary>Held in battle :   When the holder has 1/4 its max [HP]{mechanic:hp} remaining or less, it consumes this item.  On the following turn, the holder will act first among moves with the same [priority]{mechanic:priority}, regardless of [Speed]{mechanic:speed}.</summary>
		CUSTAP_BERRY = 187,
		/// <summary>Held in battle :   When the holder takes [physical]{mechanic:physical} damage, it consumes this item to damage the attacking Pokémon for 1/8 its max [HP]{mechanic:hp}.</summary>
		JABOCA_BERRY = 188,
		/// <summary>Held in battle :   When the holder takes [special]{mechanic:special} damage, it consumes this item to damage the attacking Pokémon for 1/8 its max [HP]{mechanic:hp}.</summary>
		ROWAP_BERRY = 189,
		/// <summary>Held in battle :   Moves targeting the holder have 0.9× [chance to hit]{mechanic:chance-to-hit}.</summary>
		BRIGHT_POWDER = 190,
		/// <summary>Held in battle :   At the end of each turn, if any of the holder's stats have a negative [stat modifier]{mechanic:stat-modifier}, the holder consumes this item to remove the modifiers from those stats.</summary>
		WHITE_HERB = 191,
		/// <summary>Held :   When the holder would gain [effort]{mechanic:effort} due to battle, it gains double that effort instead.  Held in battle :   Holder has half its [Speed]{mechanic:speed}.</summary>
		MACHO_BRACE = 192,
		/// <summary>Held :   [Experience]{mechanic:experience} is split across two groups: Pokémon who participated in battle, and Pokémon holding this item.  Each Pokémon earns experience as though it had battled alone, divided by the number of Pokémon in its group, then divided by the number of groups. Pokémon holding this item who also participated in battle effectively earn experience twice.      [Fainted]{mechanic:fainted} Pokémon never earn experience, and empty groups are ignored; thus, if a single Pokémon is holding this item and the only Pokémon who battled faints from []{move:explosion}, the holder will gain full experience.</summary>
		EXP_SHARE = 193,
		/// <summary>Held in battle :   Whenever the holder attempts to use a move, it has a 3/16 chance to act first among moves with the same [priority]{mechanic:priority}.  If multiple Pokémon have this effect at the same time, [Speed]{mechanic:speed} is the tie-breaker as normal, but the effect of []{move:trick-room} is ignored.</summary>
		QUICK_CLAW = 194,
		/// <summary>Held :   When the holder would earn [happiness]{mechanic:happiness} for any reason, it earns twice that amount instead.</summary>
		SOOTHE_BELL = 195,
		/// <summary>Held in battle :   When the holder is [attracted]{move:attract}, it consumes this item to cure the attraction.</summary>
		MENTAL_HERB = 196,
		/// <summary>Held in battle :   Holder has 1.5× its [Attack]{mechanic:attack}.  When the holder attempts to use a move, all its other moves are disabled until it leaves battle or loses this item.      The restriction ends even if this item is swapped for another Choice item via []{move:trick} or []{move:switcheroo}.</summary>
		CHOICE_BAND = 197,
		/// <summary>Held in battle :   Holder's damaging moves have a 10% chance to make their target [flinch]{mechanic:flinch}.  This chance applies independently to each hit of a multi-hit move.      This item's chance is rolled independently of any other move effects; e.g., a move with a 30% chance to flinch normally will have a 37% total chance to flinch when used with this item, because 3% of the time, both effects activate.  Held by []{pokemon:poliwhirl} or []{pokemon:slowbro} :   Holder evolves into []{pokemon:politoed} or []{pokemon:slowking}, respectively, when traded.</summary>
		KINGS_ROCK = 198,
		/// <summary>Held in battle :   Holder's []{type:bug}-type moves have 1.2× their power. </summary>
		SILVER_POWDER = 199,
		/// <summary>Held :   If the holder participated in a trainer battle, the trainer earns twice the usual prize money.  This effect applies even if the holder [fainted]{mechanic:fainted}.      This effect does not stack with any other similar effect.</summary>
		AMULET_COIN = 200,
		/// <summary>Held by lead Pokémon: Prevents wild battles with Pokémon that are lower level than the holder.</summary>
		CLEANSE_TAG = 201,
		/// <summary>Held by Latias or Latios: Increases the holder's Special Attack and Special Defense by 50%.</summary>
		SOUL_DEW = 202,
		/// <summary>Held by Clamperl: Doubles the holder's Special Attack.  Evolves the holder into Huntail when traded.</summary>
		DEEP_SEA_TOOTH = 203,
		/// <summary>Held by Clamperl: Doubles the holder's Special Defense.  Evolves the holder into Gorebyss when traded.</summary>
		DEEP_SEA_SCALE = 204,
		/// <summary>Held: In wild battles, attempts to run away on the holder's turn will always succeed.</summary>
		SMOKE_BALL = 205,
		/// <summary>Held: Prevents the holder from evolving naturally.  Evolution initiated by the trainer (Stones, etc) will still work.</summary>
		EVERSTONE = 206,
		/// <summary>Held: If the holder is attacked for regular damage that would faint it, this item has a 10% chance to prevent the holder's HP from lowering below 1.</summary>
		FOCUS_BAND = 207,
		/// <summary>Held: Increases any Exp the holder gains by 50%.</summary>
		LUCKY_EGG = 208,
		/// <summary>Held: Raises the holder's critical hit counter by 1.</summary>
		SCOPE_LENS = 209,
		/// <summary>Held: Increases the power of the holder's Steel moves by 20%. Held by Onix or Scyther: Evolves the holder into Steelix or Scizor when traded, respectively.</summary>
		METAL_COAT = 210,
		/// <summary>Held: Heals the holder by 1/16 its max HP at the end of each turn.</summary>
		LEFTOVERS = 211,
		/// <summary>Held by Seadra: Evolves the holder into Kingdra when traded.</summary>
		DRAGON_SCALE = 212,
		/// <summary>Held by Pikachu: Doubles the holder's initial Attack and Special Attack.</summary>
		LIGHT_BALL = 213,
		/// <summary>Held: Increases the power of the holder's Ground moves by 20%.</summary>
		SOFT_SAND = 214,
		/// <summary>Held: Increases the power of the holder's Rock moves by 20%.</summary>
		HARD_STONE = 215,
		/// <summary>Held: Increases the power of the holder's Grass moves by 20%.</summary>
		MIRACLE_SEED = 216,
		/// <summary>Held: Increases the power of the holder's Dark moves by 20%.</summary>
		BLACK_GLASSES = 217,
		/// <summary>Held: Increases the power of the holder's Fighting moves by 20%.</summary>
		BLACK_BELT = 218,
		/// <summary>Held: Increases the power of the holder's Electric moves by 20%.</summary>
		MAGNET = 219,
		/// <summary>Held: Increases the power of the holder's Water moves by 20%.</summary>
		MYSTIC_WATER = 220,
		/// <summary>Held: Increases the power of the holder's Flying moves by 20%.</summary>
		SHARP_BEAK = 221,
		/// <summary>Held: Increases the power of the holder's Poison moves by 20%.</summary>
		POISON_BARB = 222,
		/// <summary>Held: Increases the power of the holder's Ice moves by 20%.</summary>
		NEVER_MELT_ICE = 223,
		/// <summary>Held: Increases the power of the holder's Ghost moves by 20%.</summary>
		SPELL_TAG = 224,
		/// <summary>Held: Increases the power of the holder's Psychic moves by 20%.</summary>
		TWISTED_SPOON = 225,
		/// <summary>Held: Increases the power of the holder's Fire moves by 20%.</summary>
		CHARCOAL = 226,
		/// <summary>Held: Increases the power of the holder's Dragon moves by 20%.</summary>
		DRAGON_FANG = 227,
		/// <summary>Held: Increases the power of the holder's Normal moves by 20%.</summary>
		SILK_SCARF = 228,
		/// <summary>Held by Porygon: Evolves the holder into Porygon2 when traded.</summary>
		UP_GRADE = 229,
		/// <summary>Held: Heals the holder by 1/8 of any damage it inflicts.</summary>
		SHELL_BELL = 230,
		/// <summary>Held: Increases the power of the holder's Water moves by 20%.</summary>
		SEA_INCENSE = 231,
		/// <summary>Held: Increases the holder's Evasion by 5%.</summary>
		LAX_INCENSE = 232,
		/// <summary>Held by Chansey: Raises the holder's critical hit counter by 2.</summary>
		LUCKY_PUNCH = 233,
		/// <summary>Held by Ditto: Increases the holder's initial Defense and Special Defense by 50%.</summary>
		METAL_POWDER = 234,
		/// <summary>Held by Cubone or Marowak: Doubles the holder's Attack.</summary>
		THICK_CLUB = 235,
		/// <summary>Held by Farfetch'd: Raises the holder's critical hit counter by 2.</summary>
		STICK = 236,
		/// <summary>Held: Increases the holder's Coolness during a Super Contest's Visual Competition.</summary>
		RED_SCARF = 237,
		/// <summary>Held: Increases the holder's Beauty during a Super Contest's Visual Competition.</summary>
		BLUE_SCARF = 238,
		/// <summary>Held: Increases the holder's Cuteness during a Super Contest's Visual Competition.</summary>
		PINK_SCARF = 239,
		/// <summary>Held: Increases the holder's Smartness during a Super Contest's Visual Competition.</summary>
		GREEN_SCARF = 240,
		/// <summary>Held: Increases the holder's Toughness during a Super Contest's Visual Competition.</summary>
		YELLOW_SCARF = 241,
		/// <summary>Held: Increases the accuracy of any move the holder uses by 10% (multiplied; i.e. 70% accuracy is increased to 77%).</summary>
		WIDE_LENS = 242,
		/// <summary>Held: Increases the power of the holder's physical moves by 10%.</summary>
		MUSCLE_BAND = 243,
		/// <summary>Held: Increases the power of the holder's special moves by 10%.</summary>
		WISE_GLASSES = 244,
		/// <summary>Held: When the holder hits with a super-effective move, its power is raised by 20%.</summary>
		EXPERT_BELT = 245,
		/// <summary>Held: The holder's Reflect and Light Screen will create effects lasting for eight turns rather than five.  As this item affects the move rather than the barrier itself, the effect is not lost if the holder leaves battle or drops this item.</summary>
		LIGHT_CLAY = 246,
		/// <summary>Held: Damage from the holder's moves is increased by 30%.  On each turn the holder uses a damage-inflicting move, it takes 10% its max HP in damage.</summary>
		LIFE_ORB = 247,
		/// <summary>Held: Whenever the holder uses a move that requires a turn to charge first (Bounce, Dig, Dive, Fly, Razor Wind, Skull Bash, Sky Attack, or Solarbeam), this item is consumed and the charge is skipped.  Skull Bash still provides a Defense boost.</summary>
		POWER_HERB = 248,
		/// <summary>Held: Badly poisons the holder at the end of each turn.</summary>
		TOXIC_ORB = 249,
		/// <summary>Held: Burns the holder at the end of each turn.</summary>
		FLAME_ORB = 250,
		/// <summary>Held by Ditto: Doubles the holder's initial Speed.</summary>
		QUICK_POWDER = 251,
		/// <summary>Held: If the holder has full HP and is attacked for regular damage that would faint it, this item is consumed and prevents the holder's HP from lowering below 1.  This effect works against multi-hit attacks, but does not work against the effects of Doom Desire or Future Sight.</summary>
		FOCUS_SASH = 252,
		/// <summary>Held: Raises the holder's Accuracy by 20% when it goes last. Ingame description is incorrect.</summary>
		ZOOM_LENS = 253,
		/// <summary>Held: Each time the holder uses the same move consecutively, its power is increased by another 10% of its original, to a maximum of 100%.</summary>
		METRONOME = 254,
		/// <summary>Held: Decreases the holder's Speed by 50%.  If the holder is Flying or has Levitate, it takes regular damage from Ground attacks and is suspectible to Spikes and Toxic Spikes.</summary>
		IRON_BALL = 255,
		/// <summary>Held: The holder will go last within its move's priority bracket, regardless of Speed.  If multiple Pokémon within the same priority bracket are subject to this effect, the slower Pokémon will go first.  The holder will move after Pokémon with Stall.  If the holder has Stall, Stall is ignored.  This item ignores Trick Room.</summary>
		LAGGING_TAIL = 256,
		/// <summary>Held: When the holder becomes Attracted, the Pokémon it is Attracted to becomes Attracted back.</summary>
		DESTINY_KNOT = 257,
		/// <summary>Held: If the holder is Poison-type, restores 1/16 max HP at the end of each turn.  Otherwise, damages the holder by 1/16 its max HP at the end of each turn.</summary>
		BLACK_SLUDGE = 258,
		/// <summary>Held: The holder's Hail will create a hailstorm lasting for eight turns rather than five.  As this item affects the move rather than the weather itself, the effect is not lost if the holder leaves battle or drops this item.</summary>
		ICY_ROCK = 259,
		/// <summary>Held: The holder's Sandstorm will create a sandstorm lasting for eight turns rather than five.  As this item affects the move rather than the weather itself, the effect is not lost if the holder leaves battle or drops this item.</summary>
		SMOOTH_ROCK = 260,
		/// <summary>Held: The holder's Sunny Day will create sunshine lasting for eight turns rather than five.  As this item affects the move rather than the weather itself, the effect is not lost if the holder leaves battle or drops this item.</summary>
		HEAT_ROCK = 261,
		/// <summary>Held: The holder's Rain Dance will create rain lasting for eight turns rather than five.  As this item affects the move rather than the weather itself, the effect is not lost if the holder leaves battle or drops this item.</summary>
		DAMP_ROCK = 262,
		/// <summary>Held: Increases the duration of the holder's multiturn (2-5 turn) moves by three turns.</summary>
		GRIP_CLAW = 263,
		/// <summary>Held: Increases the holder's Speed by 50%, but restricts it to the first move it uses until it leaves battle or loses this item.  If this item is swapped for another Choice item via Trick or Switcheroo, the holder's restriction is still lifted, but it will again be restricted to the next move it uses. (Quirk: If the holder is switched in by U-Turn and it also knows U-Turn, U-Turn becomes its restricted move.)</summary>
		CHOICE_SCARF = 264,
		/// <summary>Held: Damaged the holder for 1/8 its max HP.  When the holder is struck by a contact move, damages the attacker for 1/8 its max HP; if the attacker is not holding an item, it will take this item.</summary>
		STICKY_BARB = 265,
		/// <summary>Held: Decreases the holder's Speed by 50%.  Whenever the holder gains Attack effort from battle, increases that effort by 4; this applies before the PokéRUS doubling effect.</summary>
		POWER_BRACER = 266,
		/// <summary>Held: Decreases the holder's Speed by 50%.  Whenever the holder gains Defense effort from battle, increases that effort by 4; this applies before the PokéRUS doubling effect.</summary>
		POWER_BELT = 267,
		/// <summary>Held: Decreases the holder's Speed by 50%.  Whenever the holder gains Special Attack effort from battle, increases that effort by 4; this applies before the PokéRUS doubling effect.</summary>
		POWER_LENS = 268,
		/// <summary>Held: Decreases the holder's Speed by 50%.  Whenever the holder gains Special Defense effort from battle, increases that effort by 4; this applies before the PokéRUS doubling effect.</summary>
		POWER_BAND = 269,
		/// <summary>Held: Decreases the holder's Speed by 50%.  Whenever the holder gains Speed effort from battle, increases that effort by 4; this applies before the PokéRUS doubling effect.</summary>
		POWER_ANKLET = 270,
		/// <summary>Held: Decreases the holder's Speed by 50%.  Whenever the holder gains HP effort from battle, increases that effort by 4; this applies before the PokéRUS doubling effect.</summary>
		POWER_WEIGHT = 271,
		/// <summary>Held: The holder is unaffected by any moves or abilities that would prevent it from actively leaving battle.</summary>
		SHED_SHELL = 272,
		/// <summary>Held: HP restored from Absorb, Aqua Ring, Drain Punch, Dream Eater, Giga Drain, Ingrain, Leech Life, Leech Seed, and Mega Drain is increased by 30%.  Damage inflicted is not affected.</summary>
		BIG_ROOT = 273,
		/// <summary>Held: Increases the holder's Special Attack by 50%, but restricts it to the first move it uses until it leaves battle or loses this item.  If this item is swapped for another Choice item via Trick or Switcheroo, the holder's restriction is still lifted, but it will again be restricted to the next move it uses. (Quirk: If the holder is switched in by U-Turn and it also knows U-Turn, U-Turn becomes its restricted move.)</summary>
		CHOICE_SPECS = 274,
		/// <summary>Held: Increases the power of the holder's Fire moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Fire.</summary>
		FLAME_PLATE = 275,
		/// <summary>Held: Increases the power of the holder's Water moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Water.</summary>
		SPLASH_PLATE = 276,
		/// <summary>Held: Increases the power of the holder's Electric moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Electric.</summary>
		ZAP_PLATE = 277,
		/// <summary>Held: Increases the power of the holder's Grass moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Grass.</summary>
		MEADOW_PLATE = 278,
		/// <summary>Held: Increases the power of the holder's Ice moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Ice.</summary>
		ICICLE_PLATE = 279,
		/// <summary>Held: Increases the power of the holder's Fighting moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Fighting.</summary>
		FIST_PLATE = 280,
		/// <summary>Held: Increases the power of the holder's Poison moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Poison.</summary>
		TOXIC_PLATE = 281,
		/// <summary>Held: Increases the power of the holder's Ground moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Ground.</summary>
		EARTH_PLATE = 282,
		/// <summary>Held: Increases the power of the holder's Flying moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Flying.</summary>
		SKY_PLATE = 283,
		/// <summary>Held: Increases the power of the holder's Psychic moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Psychic.</summary>
		MIND_PLATE = 284,
		/// <summary>Held: Increases the power of the holder's Bug moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Bug.</summary>
		INSECT_PLATE = 285,
		/// <summary>Held: Increases the power of the holder's Rock moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Rock.</summary>
		STONE_PLATE = 286,
		/// <summary>Held: Increases the power of the holder's Ghost moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Ghost.</summary>
		SPOOKY_PLATE = 287,
		/// <summary>Held: Increases the power of the holder's Dragon moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Dragon.</summary>
		DRACO_PLATE = 288,
		/// <summary>Held: Increases the power of the holder's Dark moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Dark.</summary>
		DREAD_PLATE = 289,
		/// <summary>Held: Increases the power of the holder's Steel moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Steel.</summary>
		IRON_PLATE = 290,
		/// <summary>Held: Increases the power of the holder's Psychic moves by 20%.</summary>
		ODD_INCENSE = 291,
		/// <summary>Held: Increases the power of the holder's Rock moves by 20%.</summary>
		ROCK_INCENSE = 292,
		/// <summary>Held: The holder will go last within its move's priority bracket, regardless of Speed.  If multiple Pokémon within the same priority bracket are subject to this effect, the slower Pokémon will go first.  The holder will move after Pokémon with Stall.  If the holder has Stall, Stall is ignored.  This item ignores Trick Room.</summary>
		FULL_INCENSE = 293,
		/// <summary>Held: Increases the power of the holder's Water moves by 20%.</summary>
		WAVE_INCENSE = 294,
		/// <summary>Held: Increases the power of the holder's Grass moves by 20%.</summary>
		ROSE_INCENSE = 295,
		/// <summary>Held: Doubles the money the trainer receives after an in-game trainer battle.  This effect cannot apply more than once to the same battle.</summary>
		LUCK_INCENSE = 296,
		/// <summary>Held by lead Pokémon: Prevents wild battles with Pokémon that are lower level than the holder.</summary>
		PURE_INCENSE = 297,
		/// <summary>Held by Rhydon: Evolves the holder into Rhyperior when traded.</summary>
		PROTECTOR = 298,
		/// <summary>Held by Electabuzz: Evolves the holder into Electivire when traded.</summary>
		ELECTIRIZER = 299,
		/// <summary>Held by Magmar: Evolves the holder into Magmortar when traded.</summary>
		MAGMARIZER = 300,
		/// <summary>Held by Porygon2: Evolves the holder into Porygon-Z when traded.</summary>
		DUBIOUS_DISC = 301,
		/// <summary>Held by Dusclops: Evolves the holder into Dusknoir when traded.</summary>
		REAPER_CLOTH = 302,
		/// <summary>Held: Raises the holder's critical hit counter by 1. Held by Sneasel: Evolves the holder into Weavile when it levels up during the night.</summary>
		RAZOR_CLAW = 303,
		/// <summary>Held: When the holder attacks with most damaging moves, provides an extra 11.7% (30/256) chance for the target to flinch. Held by Gligar: Evolves the holder into Gliscor when it levels up.</summary>
		RAZOR_FANG = 304,
		/// <summary>Use for any and all TMs, to remove limit on version generation...</summary>
		TM_ALL = 305,
		/*// <summary>Teaches Focus Punch to a compatible Pokémon.</summary>
		TM01 = 305,
		/// <summary>Teaches Dragon Claw to a compatible Pokémon.</summary>
		TM02 = 306,
		/// <summary>Teaches Water Pulse to a compatible Pokémon.</summary>
		TM03 = 307,
		/// <summary>Teaches Calm Mind to a compatible Pokémon.</summary>
		TM04 = 308,
		/// <summary>Teaches Roar to a compatible Pokémon.</summary>
		TM05 = 309,
		/// <summary>Teaches Toxic to a compatible Pokémon.</summary>
		TM06 = 310,
		/// <summary>Teaches Hail to a compatible Pokémon.</summary>
		TM07 = 311,
		/// <summary>Teaches Bulk Up to a compatible Pokémon.</summary>
		TM08 = 312,
		/// <summary>Teaches Bullet Seed to a compatible Pokémon.</summary>
		TM09 = 313,
		/// <summary>Teaches Hidden Power to a compatible Pokémon.</summary>
		TM10 = 314,
		/// <summary>Teaches Sunny Day to a compatible Pokémon.</summary>
		TM11 = 315,
		/// <summary>Teaches Taunt to a compatible Pokémon.</summary>
		TM12 = 316,
		/// <summary>Teaches Ice Beam to a compatible Pokémon.</summary>
		TM13 = 317,
		/// <summary>Teaches Blizzard to a compatible Pokémon.</summary>
		TM14 = 318,
		/// <summary>Teaches Hyper Beam to a compatible Pokémon.</summary>
		TM15 = 319,
		/// <summary>Teaches Light Screen to a compatible Pokémon.</summary>
		TM16 = 320,
		/// <summary>Teaches Protect to a compatible Pokémon.</summary>
		TM17 = 321,
		/// <summary>Teaches Rain Dance to a compatible Pokémon.</summary>
		TM18 = 322,
		/// <summary>Teaches Giga Drain to a compatible Pokémon.</summary>
		TM19 = 323,
		/// <summary>Teaches Safeguard to a compatible Pokémon.</summary>
		TM20 = 324,
		/// <summary>Teaches Frustration to a compatible Pokémon.</summary>
		TM21 = 325,
		/// <summary>Teaches SolarBeam to a compatible Pokémon.</summary>
		TM22 = 326,
		/// <summary>Teaches Iron Tail to a compatible Pokémon.</summary>
		TM23 = 327,
		/// <summary>Teaches Thunderbolt to a compatible Pokémon.</summary>
		TM24 = 328,
		/// <summary>Teaches Thunder to a compatible Pokémon.</summary>
		TM25 = 329,
		/// <summary>Teaches Earthquake to a compatible Pokémon.</summary>
		TM26 = 330,
		/// <summary>Teaches Return to a compatible Pokémon.</summary>
		TM27 = 331,
		/// <summary>Teaches Dig to a compatible Pokémon.</summary>
		TM28 = 332,
		/// <summary>Teaches Psychic to a compatible Pokémon.</summary>
		TM29 = 333,
		/// <summary>Teaches Shadow Ball to a compatible Pokémon.</summary>
		TM30 = 334,
		/// <summary>Teaches Brick Break to a compatible Pokémon.</summary>
		TM31 = 335,
		/// <summary>Teaches Double Team to a compatible Pokémon.</summary>
		TM32 = 336,
		/// <summary>Teaches Reflect to a compatible Pokémon.</summary>
		TM33 = 337,
		/// <summary>Teaches Shock Wave to a compatible Pokémon.</summary>
		TM34 = 338,
		/// <summary>Teaches Flamethrower to a compatible Pokémon.</summary>
		TM35 = 339,
		/// <summary>Teaches Sludge Bomb to a compatible Pokémon.</summary>
		TM36 = 340,
		/// <summary>Teaches Sandstorm to a compatible Pokémon.</summary>
		TM37 = 341,
		/// <summary>Teaches Fire Blast to a compatible Pokémon.</summary>
		TM38 = 342,
		/// <summary>Teaches Rock Tomb to a compatible Pokémon.</summary>
		TM39 = 343,
		/// <summary>Teaches Aerial Ace to a compatible Pokémon.</summary>
		TM40 = 344,
		/// <summary>Teaches Torment to a compatible Pokémon.</summary>
		TM41 = 345,
		/// <summary>Teaches Facade to a compatible Pokémon.</summary>
		TM42 = 346,
		/// <summary>Teaches Secret Power to a compatible Pokémon.</summary>
		TM43 = 347,
		/// <summary>Teaches Rest to a compatible Pokémon.</summary>
		TM44 = 348,
		/// <summary>Teaches Attract to a compatible Pokémon.</summary>
		TM45 = 349,
		/// <summary>Teaches Thief to a compatible Pokémon.</summary>
		TM46 = 350,
		/// <summary>Teaches Steel Wing to a compatible Pokémon.</summary>
		TM47 = 351,
		/// <summary>Teaches Skill Swap to a compatible Pokémon.</summary>
		TM48 = 352,
		/// <summary>Teaches Snatch to a compatible Pokémon.</summary>
		TM49 = 353,
		/// <summary>Teaches Overheat to a compatible Pokémon.</summary>
		TM50 = 354,
		/// <summary>Teaches Roost to a compatible Pokémon.</summary>
		TM51 = 355,
		/// <summary>Teaches Focus Blast to a compatible Pokémon.</summary>
		TM52 = 356,
		/// <summary>Teaches Energy Ball to a compatible Pokémon.</summary>
		TM53 = 357,
		/// <summary>Teaches False Swipe to a compatible Pokémon.</summary>
		TM54 = 358,
		/// <summary>Teaches Brine to a compatible Pokémon.</summary>
		TM55 = 359,
		/// <summary>Teaches Fling to a compatible Pokémon.</summary>
		TM56 = 360,
		/// <summary>Teaches Charge Beam to a compatible Pokémon.</summary>
		TM57 = 361,
		/// <summary>Teaches Endure to a compatible Pokémon.</summary>
		TM58 = 362,
		/// <summary>Teaches Dragon Pulse to a compatible Pokémon.</summary>
		TM59 = 363,
		/// <summary>Teaches Drain Punch to a compatible Pokémon.</summary>
		TM60 = 364,
		/// <summary>Teaches Will-O-Wisp to a compatible Pokémon.</summary>
		TM61 = 365,
		/// <summary>Teaches Silver Wind to a compatible Pokémon.</summary>
		TM62 = 366,
		/// <summary>Teaches Embargo to a compatible Pokémon.</summary>
		TM63 = 367,
		/// <summary>Teaches Explosion to a compatible Pokémon.</summary>
		TM64 = 368,
		/// <summary>Teaches Shadow Claw to a compatible Pokémon.</summary>
		TM65 = 369,
		/// <summary>Teaches Payback to a compatible Pokémon.</summary>
		TM66 = 370,
		/// <summary>Teaches Recycle to a compatible Pokémon.</summary>
		TM67 = 371,
		/// <summary>Teaches Giga Impact to a compatible Pokémon.</summary>
		TM68 = 372,
		/// <summary>Teaches Rock Polish to a compatible Pokémon.</summary>
		TM69 = 373,
		/// <summary>Teaches Flash to a compatible Pokémon.</summary>
		TM70 = 374,
		/// <summary>Teaches Stone Edge to a compatible Pokémon.</summary>
		TM71 = 375,
		/// <summary>Teaches Avalanche to a compatible Pokémon.</summary>
		TM72 = 376,
		/// <summary>Teaches Thunder Wave to a compatible Pokémon.</summary>
		TM73 = 377,
		/// <summary>Teaches Gyro Ball to a compatible Pokémon.</summary>
		TM74 = 378,
		/// <summary>Teaches Swords Dance to a compatible Pokémon.</summary>
		TM75 = 379,
		/// <summary>Teaches Stealth Rock to a compatible Pokémon.</summary>
		TM76 = 380,
		/// <summary>Teaches Psych Up to a compatible Pokémon.</summary>
		TM77 = 381,
		/// <summary>Teaches Captivate to a compatible Pokémon.</summary>
		TM78 = 382,
		/// <summary>Teaches Dark Pulse to a compatible Pokémon.</summary>
		TM79 = 383,
		/// <summary>Teaches Rock Slide to a compatible Pokémon.</summary>
		TM80 = 384,
		/// <summary>Teaches X-Scissor to a compatible Pokémon.</summary>
		TM81 = 385,
		/// <summary>Teaches Sleep Talk to a compatible Pokémon.</summary>
		TM82 = 386,
		/// <summary>Teaches Natural Gift to a compatible Pokémon.</summary>
		TM83 = 387,
		/// <summary>Teaches Poison Jab to a compatible Pokémon.</summary>
		TM84 = 388,
		/// <summary>Teaches Dream Eater to a compatible Pokémon.</summary>
		TM85 = 389,
		/// <summary>Teaches Grass Knot to a compatible Pokémon.</summary>
		TM86 = 390,
		/// <summary>Teaches Swagger to a compatible Pokémon.</summary>
		TM87 = 391,
		/// <summary>Teaches Pluck to a compatible Pokémon.</summary>
		TM88 = 392,
		/// <summary>Teaches U-Turn to a compatible Pokémon.</summary>
		TM89 = 393,
		/// <summary>Teaches Substitute to a compatible Pokémon.</summary>
		TM90 = 394,
		/// <summary>Teaches Flash Cannon to a compatible Pokémon.</summary>
		TM91 = 395,
		/// <summary>Teaches Trick Room to a compatible Pokémon.</summary>
		TM92 = 396,*/
		/// <summary>Teaches Cut to a compatible Pokémon.</summary>
		HM01 = 397,
		/// <summary>Teaches Fly to a compatible Pokémon.</summary>
		HM02 = 398,
		/// <summary>Teaches Surf to a compatible Pokémon.</summary>
		HM03 = 399,
		/// <summary>Teaches Strength to a compatible Pokémon.</summary>
		HM04 = 400,
		/// <summary>Teaches Defog to a compatible Pokémon.</summary>
		HM05 = 401,
		/// <summary>Teaches Rock Smash to a compatible Pokémon.</summary>
		HM06 = 402,
		/// <summary>Teaches Waterfall to a compatible Pokémon.</summary>
		HM07 = 403,
		/// <summary>Teaches Rock Climb to a compatible Pokémon.</summary>
		HM08 = 404,
		/// <summary>Sends the trainer to the Underground.  Only usable outside.</summary>
		EXPLORER_KIT = 405,
		/// <summary>Unused.</summary>
		LOOT_SACK = 406,
		/// <summary>Unused.</summary>
		RULE_BOOK = 407,
		/// <summary>Designates several nearby patches of grass as containing Pokémon, some of which may be special radar-only Pokémon.  Successive uses in a certain way create chains of encounters with the same species; longer chains increase the chance that a shiny Pokémon of that species will appear.</summary>
		POKE_RADAR = 408,
		/// <summary>Tracks Battle Points.</summary>
		POINT_CARD = 409,
		/// <summary>Records some of the trainer's activities for the day.</summary>
		JOURNAL = 410,
		/// <summary>Contains Seals used for decorating Pokéballs.</summary>
		SEAL_CASE = 411,
		/// <summary>Contains Pokémon Accessories.</summary>
		FASHION_CASE = 412,
		/// <summary>Unused.</summary>
		SEAL_BAG = 413,
		/// <summary>Contains friend codes for up to 32 other players, as well as their sprite, gender, and basic statistics for those that have been seen on WFC.</summary>
		PAL_PAD = 414,
		/// <summary>Opens the front door of the Valley Windworks.  Reusable.</summary>
		WORKS_KEY = 415,
		/// <summary>Given to Cynthia's grandmother to get the Surf HM.</summary>
		OLD_CHARM = 416,
		/// <summary>Grants access to Galactic HQ in Veilstone City.</summary>
		GALACTIC_KEY = 417,
		/// <summary>Unused.</summary>
		RED_CHAIN = 418,
		/// <summary>Displays a map of the region including the trainer's position, location names, visited towns, gym locations, and where the trainer has been walking recently.</summary>
		TOWN_MAP = 419,
		/// <summary>Reveals trainers who want a rematch, by showing !! over their heads.  Each use drains the battery; requires 100 steps to charge.</summary>
		VS_SEEKER = 420,
		/// <summary>Contains the Coins used by the Game Corner, to a maximum of 50,000.</summary>
		COIN_CASE = 421,
		/// <summary>Used to find Pokémon on the Old Rod list for an area, which are generally Magikarp or similar.</summary>
		OLD_ROD = 422,
		/// <summary>Used to find Pokémon on the Good Rod list for an area, which are generally mediocre.</summary>
		GOOD_ROD = 423,
		/// <summary>Used to find Pokémon on the Super Rod list for an area, which are generally the best available there.</summary>
		SUPER_ROD = 424,
		/// <summary>Waters Berry plants.</summary>
		SPRAYDUCK = 425,
		/// <summary>Contains up to 100 Poffins.</summary>
		POFFIN_CASE = 426,
		/// <summary>Increases movement speed outside or in caves.  In high gear, allows the trainer to hop over some rocks and ascend muddy slopes.</summary>
		BICYCLE = 427,
		/// <summary>Opens the locked building in the lakeside resort.</summary>
		SUITE_KEY = 428,
		/// <summary>Grants access to Flower Paradise and Shaymin.</summary>
		OAKS_LETTER = 429,
		/// <summary>Cures the sailor's son of his nightmares; no reward, only a side effect of seeing Cresselia.</summary>
		LUNAR_WING = 430,
		/// <summary>Provides access to Newmoon Island and Darkrai.</summary>
		MEMBER_CARD = 431,
		/// <summary>Supposedly related to t</summary>
		AZURE_FLUTE = 432,
		/// <summary>Allows passage on a ferry.  The same item is used for different ferries between different games.</summary>
		SS_TICKET = 433,
		/// <summary>Allows the trainer to enter Contests.</summary>
		CONTEST_PASS = 434,
		/// <summary>Causes Heatran to appear at Reversal Mountain.  Unused prior to Black and White 2.</summary>
		MAGMA_STONE = 435,
		/// <summary>Given to the trainer's rival in Jubilife City.  Contains two Town Maps, one of which is given to the trainer upon delivery.</summary>
		PARCEL = 436,
		/// <summary>One of three coupons needed to receive a Pokétch.</summary>
		COUPON_1 = 437,
		/// <summary>One of three coupons needed to receive a Pokétch.</summary>
		COUPON_2 = 438,
		/// <summary>One of three coupons needed to receive a Pokétch.</summary>
		COUPON_3 = 439,
		/// <summary>Grants access to the Team Galactic warehouse in Veilstone City.</summary>
		STORAGE_KEY = 440,
		/// <summary>Required to cure the Psyducks blocking Route 210 of their chronic headaches.</summary>
		SECRET_POTION = 441,
		/// <summary>Held by []{pokemon:giratina} :   Holder's []{type:dragon} and []{type:ghost} moves have 1.2× their base power.      Holder is in Origin Forme.  This item cannot be held by any Pokémon but Giratina.  When you enter the Union Room or connect to Wi-Fi, this item returns to your bag.</summary>
		GRISEOUS_ORB = 442,
		/// <summary>Optionally records wireless, Wi-Fi, and Battle Frontier battles.  Tracks Battle Points earned in the Battle Frontier, and stores commemorative prints.</summary>
		VS_RECORDER = 443,
		/// <summary>Used by trainer on a []{pokemon:shaymin} :   Changes the target Shaymin from Land Forme to Sky Forme.      This item cannot be used on a [frozen]{mechanic:freezing} Shaymin or at night.  Sky Forme Shaymin will revert to Land Forme overnight, when frozen, and upon entering a link battle.  This item must be used again to change it back.</summary>
		GRACIDEA = 444,
		/// <summary>Used by trainer in the Galactic Eterna Building, on the ground floor, to the left of the TV :   Unlocks the secret []{pokemon:rotom} room, in which there are five appliances which can change Rotom's form.</summary>
		SECRET_KEY = 445,
		/// <summary>Stores Apricorns.</summary>
		APRICORN_BOX = 446,
		/// <summary>Contains four portable pots of soil suitable for growing berries.</summary>
		BERRY_POTS = 447,
		/// <summary>Required to water berries within the []{item:berry-pots}.  Required to battle the []{pokemon:sudowoodo} on []{location:johto-route-36}.  This item cannot be directly used from the bag.</summary>
		SQUIRT_BOTTLE = 448,
		/// <summary>Used by trainer in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.  If used in a trainer battle, nothing happens and the ball is lost.      If the wild Pokémon was encountered by fishing, the wild Pokémon's catch rate is 3× normal.</summary>
		LURE_BALL = 449,
		/// <summary>Used by trainer in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.  If used in a trainer battle, nothing happens and the ball is lost.      If the trainer's Pokémon's level is higher than:      * four times the wild Pokémon's, the wild Pokémon's catch rate is 8× normal.     * than twice the wild Pokémon's, the wild Pokémon's catch rate is 4× normal.     * the wild Pokémon's, the wild Pokémon's catch rate is 2× normal.</summary>
		LEVEL_BALL = 450,
		/// <summary>Used by trainer in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.  If used in a trainer battle, nothing happens and the ball is lost.      If the wild Pokémon is a []{pokemon:clefairy}, []{pokemon:nidoran-m}, []{pokemon:nidoran-f}, []{pokemon:jigglypuff}, []{pokemon:skitty}, or any evolution thereof, the wild Pokémon has 4× its catch rate.</summary>
		MOON_BALL = 451,
		/// <summary>Used by a trainer in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.  If used in a trainer battle, nothing happens and the ball is lost.      If the wild Pokémon weighs:      * 409.6 kg (903.0 lb) or more, its catch rate is 40 more than normal.     * 307.2 kg (677.3 lb) or more, its catch rate is 30 more than normal.     * 204.8 kg (451.5 lb) or more, its catch rate is 20 more than normal.     * less than 204.8 kg (451.5 lb), its catch rate is 20 less than normal.</summary>
		HEAVY_BALL = 452,
		/// <summary>Used by a trainer in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.  If used in a trainer battle, nothing happens and the ball is lost.  :   If the wild Pokémon's base [speed]{mechanic:speed} is 100 or more, its catch rate is 4× normal.</summary>
		FAST_BALL = 453,
		/// <summary>Used by a trainer in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.  If used in a trainer battle, nothing happens and the ball is lost.      If caught, the wild Pokémon's [happiness]{mechanic:happiness} starts at 200.</summary>
		FRIEND_BALL = 454,
		/// <summary>Used by a trainer in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.  If used in a trainer battle, nothing happens and the ball is lost.      If the trainer's Pokémon and wild Pokémon are of the same species but opposite genders, the wild Pokémon's catch rate is 8× normal.</summary>
		LOVE_BALL = 455,
		/// <summary>Used by a trainer in battle :   [Catches]{mechanic:catch} a wild Pokémon.  This item can only be used in []{location:pal-park}.</summary>
		PARK_BALL = 456,
		/// <summary>Used by a trainer in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.      The wild Pokémon's catch rate is 1.5× normal.</summary>
		SPORT_BALL = 457,
		/// <summary>May be given to Kurt in []{location:azalea-town} to produce a []{item:level-ball}.</summary>
		RED_APRICORN = 458,
		/// <summary>May be given to Kurt in []{location:azalea-town} to produce a []{item:lure-ball}.</summary>
		BLUE_APRICORN = 459,
		/// <summary>May be given to Kurt in []{location:azalea-town} to produce a []{item:moon-ball}.</summary>
		YELLOW_APRICORN = 460,
		/// <summary>May be given to Kurt in []{location:azalea-town} to produce a []{item:friend-ball}.</summary>
		GREEN_APRICORN = 461,
		/// <summary>May be given to Kurt in []{location:azalea-town} to produce a []{item:love-ball}.</summary>
		PINK_APRICORN = 462,
		/// <summary>May be given to Kurt in []{location:azalea-town} to produce a []{item:fast-ball}.</summary>
		WHITE_APRICORN = 463,
		/// <summary>May be given to Kurt in []{location:azalea-town} to produce a []{item:heavy-ball}.</summary>
		BLACK_APRICORN = 464,
		/// <summary>Used by trainer outside of battle :   Searches for hidden items.</summary>
		DOWSING_MACHINE = 465,
		/// <summary>May be traded for a []{item:tm64} in the vertical Underground Path.</summary>
		RAGE_CANDY_BAR = 466,
		/// <summary>Causes []{pokemon:groudon} to appear in the []{location:embedded-tower}.</summary>
		RED_ORB = 467,
		/// <summary>Causes []{pokemon:kyogre} to appear in the []{location:embedded-tower}.</summary>
		BLUE_ORB = 468,
		/// <summary>Causes []{pokemon:rayquaza} to appear in the []{location:embedded-tower}.</summary>
		JADE_ORB = 469,
		/// <summary>When taken to the []{location:pewter-city} museum, causes []{pokemon:latias} or []{pokemon:latios} to attack the trainer.  The Pokémon to appear will be whicher can't be encountered roaming in the wild.</summary>
		ENIGMA_STONE = 470,
		/// <summary>Lists which []{pokemon:unown} forms the trainer has caught.</summary>
		UNOWN_REPORT = 471,
		/// <summary>Allows the trainer to answer the daily question on Buena's radio show.  Records the points earned for correct answers.</summary>
		BLUE_CARD = 472,
		/// <summary>Does nothing.</summary>
		SLOWPOKE_TAIL = 473,
		/// <summary>May be given to the Kimono Girls to summon []{pokemon:ho-oh} to the top of the []{location:bell-tower}.</summary>
		CLEAR_BELL = 474,
		/// <summary>Used by trainer outside of battle :   Opens doors in the []{location:goldenrod-city} Radio Tower.</summary>
		CARD_KEY = 475,
		/// <summary>Used by trainer outside of battle :   Opens the door to the basement tunnel under []{location:goldenrod-city}.</summary>
		BASEMENT_KEY = 476,
		/// <summary>May be traded to Mr. Pokémon for an []{item:exp-share}.</summary>
		RED_SCALE = 477,
		/// <summary>May be traded to the Copycat for a []{item:pass}.</summary>
		LOST_ITEM = 478,
		/// <summary>Allows the trainer to ride the Magnet Train between []{location:goldenrod-city} and []{location:saffron-city}.</summary>
		PASS = 479,
		/// <summary>Must be replaced in the []{location:power-plant} to power the Magnet Train.</summary>
		MACHINE_PART = 480,
		/// <summary>Causes []{pokemon:lugia} to appear in the []{location:whirl-islands}.</summary>
		SILVER_WING = 481,
		/// <summary>Causes []{pokemon:ho-oh} to appear at the top of []{location:bell-tower}.</summary>
		RAINBOW_WING = 482,
		/// <summary>Must be obtained to trigger the break-in at Professor Elm's lab, the first rival battle, and access to []{location:johto-route-31}.</summary>
		MYSTERY_EGG = 483,
		/// <summary>Used by trainer outside of battle :   Changes the background music to the equivalent 8-bit music.</summary>
		GB_SOUNDS = 484,
		/// <summary>May be given to the Kimono Girls to summon []{pokemon:lugia} to the top of the []{location:bell-tower}.</summary>
		TIDAL_BELL = 485,
		/// <summary>Records the number of times the trainer has come in first place overall in the Pokéathlon.</summary>
		DATA_CARD_01 = 486,
		/// <summary>Records the number of times the trainer has come in last place overall in the Pokéathlon.</summary>
		DATA_CARD_02 = 487,
		/// <summary>Records the number of times the trainer's Pokémon have dashed in the Pokéathlon.</summary>
		DATA_CARD_03 = 488,
		/// <summary>Records the number of times the trainer's Pokémon have jumped in the Pokéathlon.</summary>
		DATA_CARD_04 = 489,
		/// <summary>Records the number of times the trainer has come in first in the Pokéathlon Hurdle Dash.</summary>
		DATA_CARD_05 = 490,
		/// <summary>Records the number of times the trainer has come in first in the Pokéathlon Relay Run.</summary>
		DATA_CARD_06 = 491,
		/// <summary>Records the number of times the trainer has come in first in the Pokéathlon Pennant Capture.</summary>
		DATA_CARD_07 = 492,
		/// <summary>Records the number of times the trainer has come in first in the Pokéathlon Block Smash.</summary>
		DATA_CARD_08 = 493,
		/// <summary>Records the number of times the trainer has come in first in the Pokéathlon Disc Catch.</summary>
		DATA_CARD_09 = 494,
		/// <summary>Records the number of times the trainer has come in first in the Pokéathlon Snow Throw.</summary>
		DATA_CARD_10 = 495,
		/// <summary>Records the number of points the trainer has earned in the Pokéathlon.</summary>
		DATA_CARD_11 = 496,
		/// <summary>Records the number of times the trainer's Pokémon have messed up in the Pokéathlon.</summary>
		DATA_CARD_12 = 497,
		/// <summary>Records the number of times the trainer's Pokémon have defeated themselves in the Pokéathlon.</summary>
		DATA_CARD_13 = 498,
		/// <summary>Records the number of times the trainer's Pokémon have tackled in the Pokéathlon.</summary>
		DATA_CARD_14 = 499,
		/// <summary>Records the number of times the trainer's Pokémon have fallen in the Pokéathlon.</summary>
		DATA_CARD_15 = 500,
		/// <summary>Records the number of times the trainer has come in first in the Pokéathlon Ring Drop.</summary>
		DATA_CARD_16 = 501,
		/// <summary>Records the number of times the trainer has come in first in the Pokéathlon Lamp Jump.</summary>
		DATA_CARD_17 = 502,
		/// <summary>Records the number of times the trainer has come in first in the Pokéathlon Circle Push.</summary>
		DATA_CARD_18 = 503,
		/// <summary>Records the number of times the trainer has come in first place overall in the Pokéathlon over wirelss.</summary>
		DATA_CARD_19 = 504,
		/// <summary>Records the number of times the trainer has come in last place overall in the Pokéathlon over wireless.</summary>
		DATA_CARD_20 = 505,
		/// <summary>Records the number of times the trainer has come in first across all Pokéathlon events.</summary>
		DATA_CARD_21 = 506,
		/// <summary>Records the number of times the trainer has come in last across all Pokéathlon events.</summary>
		DATA_CARD_22 = 507,
		/// <summary>Records the number of times the trainer has switched Pokémon in the Pokéathlon.</summary>
		DATA_CARD_23 = 508,
		/// <summary>Records the number of times the trainer has come in first in the Pokéathlon Goal Roll.</summary>
		DATA_CARD_24 = 509,
		/// <summary>Records the number of times the trainer's Pokémon received prizes in the Pokéathlon.</summary>
		DATA_CARD_25 = 510,
		/// <summary>Records the number of times the trainer has instructed Pokémon in the Pokéathlon.</summary>
		DATA_CARD_26 = 511,
		/// <summary>Records the total time spent in the Pokéathlon.</summary>
		DATA_CARD_27 = 512,
		/// <summary>Does nothing.</summary>
		LOCK_CAPSULE = 513,
		/// <summary>Does nothing.</summary>
		PHOTO_ALBUM = 514,
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		ORANGE_MAIL = 515,
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		HARBOR_MAIL = 516,
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		GLITTER_MAIL = 517,
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		MECH_MAIL = 518,
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		WOOD_MAIL = 519,
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		WAVE_MAIL = 520,
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		BEAD_MAIL = 521,
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		SHADOW_MAIL = 522,
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		TROPIC_MAIL = 523,
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		DREAM_MAIL = 524,
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		FAB_MAIL = 525,
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		RETRO_MAIL = 526,
		/// <summary>Increases movement speed outside or in caves.  Faster than the []{item:acro-bike}.  Allows the trainer to ascend muddy slopes.</summary>
		MACH_BIKE = 527,
		/// <summary>Increases movement speed outside or in caves.  Slower than the []{item:mach-bike}.  Can perform various tricks, allowing the trainer to reach certain special areas.</summary>
		ACRO_BIKE = 528,
		/// <summary>Waters Berry plants.</summary>
		WAILMER_PAIL = 529,
		/// <summary>Contains a machine part to be delivered to Captain Stern.</summary>
		DEVON_GOODS = 530,
		/// <summary>Collects soot when walking through tall grass on []{location:hoenn-route-113}.</summary>
		SOOT_SACK = 531,
		/// <summary>Stores Pokéblocks.</summary>
		POKEBLOCK_CASE = 532,
		/// <summary>Contains a letter to be delivered to Steven.</summary>
		LETTER = 533,
		/// <summary>Provides access to []{location:southern-island} and either []{pokemon:latias} or []{pokemon:latios}, whichever is not available roaming around Hoenn.</summary>
		EON_TICKET = 534,
		/// <summary>May be traded to Captain Stern for a []{item:deep-sea-tooth} or a []{item:deep-sea-scale}.</summary>
		SCANNER = 535,
		/// <summary>Allows the trainer to enter the desert on []{location:hoenn-route-111}.</summary>
		GO_GOGGLES = 536,
		/// <summary>RSE: May be traded to Professor Cozmo for []{item:tm27}.  FRLG: A meteorite to be delivered to Lostelle's father.</summary>
		METEORITE = 537,
		/// <summary>Unlocks room 1 on the []{location:abandoned-ship}.</summary>
		RM_1_KEY = 538,
		/// <summary>Unlocks room 2 on the []{location:abandoned-ship}.</summary>
		RM_2_KEY = 539,
		/// <summary>Unlocks room 4 on the []{location:abandoned-ship}.</summary>
		RM_4_KEY = 540,
		/// <summary>Unlocks room 6 on the []{location:abandoned-ship}.</summary>
		RM_6_KEY = 541,
		/// <summary>Reveals invisble []{pokemon:kecleon} on the overworld.</summary>
		DEVON_SCOPE = 542,
		/// <summary>A parcel to be delivered to Professor Oak for a Pokédex.</summary>
		OAKS_PARCEL = 543,
		/// <summary>Wakes up [sleeping]{mechanic:sleep} Pokémon.  Required to wake up sleeping []{pokemon:snorlax} on the overworld.</summary>
		POKE_FLUTE = 544,
		/// <summary>May be traded for a []{item:bicycle}.</summary>
		BIKE_VOUCHER = 545,
		/// <summary>The Safari Zone warden's teeth, to be returned to him for []{item:hm04}.</summary>
		GOLD_TEETH = 546,
		/// <summary>Operates the elevator in the Celadon Rocket Hideout.</summary>
		LIFT_KEY = 547,
		/// <summary>Identifies ghosts in []{location:pokemon-tower}.</summary>
		SILPH_SCOPE = 548,
		/// <summary>Records information on various famous people.</summary>
		FAME_CHECKER = 549,
		/// <summary>Stores TMs and HMs.</summary>
		TM_CASE = 550,
		/// <summary>Stores Berries.</summary>
		BERRY_POUCH = 551,
		/// <summary>Teaches beginning trainers basic information.</summary>
		TEACHY_TV = 552,
		/// <summary>Provides access to the first three Sevii Islands.</summary>
		TRI_PASS = 553,
		/// <summary>Provides access to the Sevii Islands.</summary>
		RAINBOW_PASS = 554,
		/// <summary>Used to bribe the []{location:saffron-city} guards for entry to the city.</summary>
		TEA = 555,
		/// <summary>Provides access to Navel Rock, []{pokemon:ho-oh}, and []{pokemon:lugia}.</summary>
		MYSTICTICKET = 556,
		/// <summary>Provides access to Birth Island and []{pokemon:deoxys}.</summary>
		AURORATICKET = 557,
		/// <summary>Holds Berry Powder from Berry Crushing.</summary>
		POWDER_JAR = 558,
		/// <summary>Deliver to Celio for use in the Network Machine.</summary>
		RUBY = 559,
		/// <summary>Deliver to Celio for use in the Network Machine.</summary>
		SAPPHIRE = 560,
		/// <summary>Provides access to the []{location:magma-hideout} in the []{location:jagged-pass}.</summary>
		MAGMA_EMBLEM = 561,
		/// <summary>Provides access to Faraway Island and []{pokemon:mew}.</summary>
		OLD_SEA_MAP = 562,
		/// <summary>Held by []{pokemon:genesect} :   Holder's buster is blue, and its []{move:techno-blast} is []{type:water}-type.</summary>
		DOUSE_DRIVE = 563,
		/// <summary>Held by []{pokemon:genesect} :   Holder's buster is yellow, and its []{move:techno-blast} is []{type:electric}-type.</summary>
		SHOCK_DRIVE = 564,
		/// <summary>Held by []{pokemon:genesect} :   Holder's buster is red, and its []{move:techno-blast} is []{type:fire}-type.</summary>
		BURN_DRIVE = 565,
		/// <summary>Held by []{pokemon:genesect} :   Holder's buster is white, and its []{move:techno-blast} becomes []{type:ice}-type.</summary>
		CHILL_DRIVE = 566,
		/// <summary>Used on a friendly Pokémon :   Restores 20 [HP]{mechanic:hp}.</summary>
		SWEET_HEART = 567,
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		GREET_MAIL = 568,
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		FAVORED_MAIL = 569,
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		RSVP_MAIL = 570,
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		THANKS_MAIL = 571,
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		INQUIRY_MAIL = 572,
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		LIKE_MAIL = 573,
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		REPLY_MAIL = 574,
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		BRIDGE_MAIL_S = 575,
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		BRIDGE_MAIL_D = 576,
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		BRIDGE_MAIL_T = 577,
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		BRIDGE_MAIL_V = 578,
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		BRIDGE_MAIL_M = 579,
		/// <summary>Held by []{pokemon:feebas} :   Holder evolves into []{pokemon:milotic} when traded.</summary>
		PRISM_SCALE = 580,
		/// <summary>Held by a Pokémon that is not fully evolved :   Holder has 1.5× [Defense]{mechanic:defense} and [Special Defense]{mechanic:special-defense}.</summary>
		EVIOLITE = 581,
		/// <summary>Held :   Holder has 0.5× weight.</summary>
		FLOAT_STONE = 582,
		/// <summary>Held :   When the holder is hit by a [contact]{mechanic:contact} move, the attacking Pokémon takes 1/6 its max [HP]{mechanic:hp} in damage.</summary>
		ROCKY_HELMET = 583,
		/// <summary>Held :   Holder is immune to []{type:ground}-type moves, []{move:spikes}, []{move:toxic-spikes}, and []{ability:arena-trap}.      This effect does not apply during []{move:gravity} or []{move:ingrain}.      When the holder takes damage from a move, this item is consumed.</summary>
		AIR_BALLOON = 584,
		/// <summary>Held :   When the holder takes damage directly from a move and does not faint, it [switches out]{mechanic:switching-out} for another random, non-fainted Pokémon in its party. This effect does not activate if another effect prevents the holder from switching out.</summary>
		RED_CARD = 585,
		/// <summary>Held :   When one of the user's types would render it immune to damage, that type is ignored for damage calculation.</summary>
		RING_TARGET = 586,
		/// <summary>Held :   Moves used by the holder that trap and damage a target for multiple turns (e.g. []{move:bind}, []{move:fire-spin}) inflict twice their usual per-turn damage.</summary>
		BINDING_BAND = 587,
		/// <summary>Held :   When the holder takes []{type:water}-type damage from a move, its [Special Attack]{mechanic:special-attack} rises by one [stage]{mechanic:stage} and this item is consumed.</summary>
		ABSORB_BULB = 588,
		/// <summary>Held :   When the holder takes []{type:electric}-type damage from a move, its [Attack]{mechanic:attack} rises by one [stage]{mechanic:stage} and this item is consumed.</summary>
		CELL_BATTERY = 589,
		/// <summary>Held :   When the holder takes damage directly from a move and does not faint, it [switches out]{mechanic:switching-out} for another non-fainted Pokémon in its party, as chosen by the Trainer. This effect does not activate if another effect prevents the holder from switching out.</summary>
		EJECT_BUTTON = 590,
		/// <summary>Held :   When the holder uses a damaging []{type:fire}-type move, the move has 1.5× power and this item is consumed.</summary>
		FIRE_GEM = 591,
		/// <summary>Held :   When the holder uses a damaging []{type:water}-type move, the move has 1.5× power and this item is consumed.</summary>
		WATER_GEM = 592,
		/// <summary>Held :   When the holder uses a damaging []{type:electric}-type move, the move has 1.5× power and this item is consumed.</summary>
		ELECTRIC_GEM = 593,
		/// <summary>Held :   When the holder uses a damaging []{type:grass}-type move, the move has 1.5× power and this item is consumed.</summary>
		GRASS_GEM = 594,
		/// <summary>Held :   When the holder uses a damaging []{type:ice}-type move, the move has 1.5× power and this item is consumed.</summary>
		ICE_GEM = 595,
		/// <summary>Held :   When the holder uses a damaging []{type:fighting}-type move, the move has 1.5× power and this item is consumed.</summary>
		FIGHTING_GEM = 596,
		/// <summary>Held :   When the holder uses a damaging []{type:poison}-type move, the move has 1.5× power and this item is consumed.</summary>
		POISON_GEM = 597,
		/// <summary>Held :   When the holder uses a damaging []{type:ground}-type move, the move has 1.5× power and this item is consumed.</summary>
		GROUND_GEM = 598,
		/// <summary>Held :   When the holder uses a damaging []{type:flying}-type move, the move has 1.5× power and this item is consumed.</summary>
		FLYING_GEM = 599,
		/// <summary>Held :   When the holder uses a damaging []{type:psychic}-type move, the move has 1.5× power and this item is consumed.</summary>
		PSYCHIC_GEM = 600,
		/// <summary>Held :   When the holder uses a damaging []{type:bug}-type move, the move has 1.5× power and this item is consumed.</summary>
		BUG_GEM = 601,
		/// <summary>Held :   When the holder uses a damaging []{type:rock}-type move, the move has 1.5× power and this item is consumed.</summary>
		ROCK_GEM = 602,
		/// <summary>Held :   When the holder uses a damaging []{type:ghost}-type move, the move has 1.5× power and this item is consumed.</summary>
		GHOST_GEM = 603,
		/// <summary>Held :   When the holder uses a damaging []{type:dark}-type move, the move has 1.5× power and this item is consumed.</summary>
		DARK_GEM = 604,
		/// <summary>Held :   When the holder uses a damaging []{type:steel}-type move, the move has 1.5× power and this item is consumed.</summary>
		STEEL_GEM = 605,
		/// <summary>Used on a party Pokémon :   Increases the target's [HP]{mechanic:hp} [effort]{mechanic:effort} by 1.</summary>
		HEALTH_WING = 606,
		/// <summary>Used on a party Pokémon :   Increases the target's [Attack]{mechanic:attack} [effort]{mechanic:effort} by 1.</summary>
		MUSCLE_WING = 607,
		/// <summary>Used on a party Pokémon :   Increases the target's [Defense]{mechanic:defense} [effort]{mechanic:effort} by 1.</summary>
		RESIST_WING = 608,
		/// <summary>Used on a party Pokémon :   Increases the target's [Special Attack]{mechanic:special-attack} [effort]{mechanic:effort} by 1.</summary>
		GENIUS_WING = 609,
		/// <summary>Used on a party Pokémon :   Increases the target's [Special Defense]{mechanic:special-defense} [effort]{mechanic:effort} by 1.</summary>
		CLEVER_WING = 610,
		/// <summary>Used on a party Pokémon :   Increases the target's [Speed]{mechanic:speed} [effort]{mechanic:effort} by 1.</summary>
		SWIFT_WING = 611,
		/// <summary>Vendor trash.</summary>
		PRETTY_WING = 612,
		/// <summary>Give to a scientist in a museum to receive a []{pokemon:tirtouga}.</summary>
		COVER_FOSSIL = 613,
		/// <summary>Give to a scientist in a museum to receive a []{pokemon:archen}.</summary>
		PLUME_FOSSIL = 614,
		/// <summary>Allows passage on the []{location:castelia-city} ship, which leads to []{location:liberty-garden} and []{pokemon:victini}.</summary>
		LIBERTY_PASS = 615,
		/// <summary>Acts as currency to activate Pass Powers in the Entralink.</summary>
		PASS_ORB = 616,
		/// <summary>Can only be used in Entree Forest, to catch Pokémon encountered in the Dream World.  Used in battle :   [Catches]{mechanic:catch} a wild Pokémon without fail.</summary>
		DREAM_BALL = 617,
		/// <summary>Used in battle :   Ends a wild battle.  Cannot be used in trainer battles.</summary>
		POKE_TOY = 618,
		/// <summary>Stores props for the Pokémon Musical.</summary>
		PROP_CASE = 619,
		/// <summary>Only used as a plot point; this item is given to the player and taken away in the same cutscene.</summary>
		DRAGON_SKULL = 620,
		/// <summary>Cult vendor trash.</summary>
		BALM_MUSHROOM = 621,
		/// <summary>Cult vendor trash.</summary>
		BIG_NUGGET = 622,
		/// <summary>Cult vendor trash.</summary>
		PEARL_STRING = 623,
		/// <summary>Cult vendor trash.</summary>
		COMET_SHARD = 624,
		/// <summary>Cult vendor trash.</summary>
		RELIC_COPPER = 625,
		/// <summary>Cult vendor trash.</summary>
		RELIC_SILVER = 626,
		/// <summary>Cult vendor trash.</summary>
		RELIC_GOLD = 627,
		/// <summary>Cult vendor trash.</summary>
		RELIC_VASE = 628,
		/// <summary>Cult vendor trash.</summary>
		RELIC_BAND = 629,
		/// <summary>Cult vendor trash.</summary>
		RELIC_STATUE = 630,
		/// <summary>Cult vendor trash.</summary>
		RELIC_CROWN = 631,
		/// <summary>Used on a party Pokémon :   Cures any [status ailment]{mechanic:status-ailment} and [confusion]{mechanic:confusion}.</summary>
		CASTELIACONE = 632,
		/// <summary>Used on a party Pokémon in battle :   Raises the target's [critical hit]{mechanic:critical-hit} rate by two [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summary>
		DIRE_HIT_2 = 633,
		/// <summary>Used on a party Pokémon in battle :   Raises the target's [Speed]{mechanic:speed} by two [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summary>
		X_SPEED_2 = 634,
		/// <summary>Used on a party Pokémon in battle :   Raises the target's [Special Attack]{mechanic:special-attack} by two [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summary>
		X_SP_ATK_2 = 635,
		/// <summary>Used on a party Pokémon in battle :   Raises the target's [Special Defense]{mechanic:special-defense} by two [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summary>
		X_SP_DEF_2 = 636,
		/// <summary>Used on a party Pokémon in battle :   Raises the target's [Defense]{mechanic:defense} by two [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summary>
		X_DEFENSE_2 = 637,
		/// <summary>Used on a party Pokémon in battle :   Raises the target's [Attack]{mechanic:attack} by two [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summary>
		X_ATTACK_2 = 638,
		/// <summary>Used on a party Pokémon in battle :   Raises the target's [accuracy]{mechanic:accuracy} by two [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summary>
		X_ACCURACY_2 = 639,
		/// <summary>Used on a party Pokémon in battle :   Raises the target's [Speed]{mechanic:speed} by three [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summary>
		X_SPEED_3 = 640,
		/// <summary>Used on a party Pokémon in battle :   Raises the target's [Special Attack]{mechanic:special-attack} by three [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summary>
		X_SP_ATK_3 = 641,
		/// <summary>Used on a party Pokémon in battle :   Raises the target's [Special Defense]{mechanic:special-defense} by three [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summary>
		X_SP_DEF_3 = 642,
		/// <summary>Used on a party Pokémon in battle :   Raises the target's [Defense]{mechanic:defense} by three [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summary>
		X_DEFENSE_3 = 643,
		/// <summary>Used on a party Pokémon in battle :   Raises the target's [Attack]{mechanic:attack} by three [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summary>
		X_ATTACK_3 = 644,
		/// <summary>Used on a party Pokémon in battle :   Raises the target's [accuracy]{mechanic:accuracy} by three [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summary>
		X_ACCURACY_3 = 645,
		/// <summary>Used on a party Pokémon in battle :   Raises the target's [Speed]{mechanic:speed} by six [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summary>
		X_SPEED_6 = 646,
		/// <summary>Used on a party Pokémon in battle :   Raises the target's [Special Attack]{mechanic:special-attack} by six [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summary>
		X_SP_ATK_6 = 647,
		/// <summary>Used on a party Pokémon in battle :   Raises the target's [Special Defense]{mechanic:special-defense} by six [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summary>
		X_SP_DEF_6 = 648,
		/// <summary>Used on a party Pokémon in battle :   Raises the target's [Defense]{mechanic:defense} by six [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summary>
		X_DEFENSE_6 = 649,
		/// <summary>Used on a party Pokémon in battle :   Raises the target's [Attack]{mechanic:attack} by six [stages]{mechanic:stage}.  This item can only be obtained or used via the Wonder Launcher.</summary>
		X_ATTACK_6 = 650,
		/// <summary>Used on a party Pokémon in battle :   Raises the target's [accuracy]{mechanic:accuracy} by six [stages]{mechanic:stage}.  This item can only be obtained or used via the Wonder Launcher.</summary>
		X_ACCURACY_6 = 651,
		/// <summary>Used on a party Pokémon in battle :   Selects another friendly Pokémon at random.  If that Pokémon's ability is normally activated by some condition—i.e., is not continuous and passive—its effect is forcibly activated.  This item can only be obtained or used via the Wonder Launcher.</summary>
		ABILITY_URGE = 652,
		/// <summary>Used on a party Pokémon in battle :   Selects another friendly Pokémon at random.  If that Pokémon is holding an item, that item is removed for the duration of the battle.  This item can only be obtained or used via the Wonder Launcher.</summary>
		ITEM_DROP = 653,
		/// <summary>Used on a party Pokémon in battle :   Selects another friendly Pokémon at random.  If that Pokémon is holding an item normally activated by some condition—i.e., not continuous and passive—its effect is forcibly activated.  This item can only be obtained or used via the Wonder Launcher.</summary>
		ITEM_URGE = 654,
		/// <summary>Used on a party Pokémon in battle :   Selects another friendly Pokémon at random.  Removes all of that Pokémon's stat changes.  This item can only be obtained or used via the Wonder Launcher.</summary>
		RESET_URGE = 655,
		/// <summary>Used on a party Pokémon in battle :   Raises the target's [critical hit]{mechanic:critical-hit} rate by three [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summary>
		DIRE_HIT_3 = 656,
		/// <summary>Summons []{pokemon:reshiram} for the final battle against N.</summary>
		LIGHT_STONE = 657,
		/// <summary>Summons []{pokemon:zekrom} for the final battle against N.</summary>
		DARK_STONE = 658,
		/*// <summary>Teaches []{move:wild-charge} to a compatible Pokémon.</summary>
		TM93 = 659,
		/// <summary>Teaches []{move:rock-smash} to a compatible Pokémon.</summary>
		TM94 = 660,
		/// <summary>Teaches []{move:snarl} to a compatible Pokémon.</summary>
		TM95 = 661,*/
		/// <summary>Makes four-way video calls.  Used for plot advancement in-game, but also works with other players via the C-Gear.</summary>
		XTRANSCEIVER = 662,
		/// <summary>Unknown.  Currently unused.</summary>
		GOD_STONE = 663,
		/// <summary>Give to the []{pokemon:wingull} on []{location:unova-route-13}, along with []{item:gram-2} and []{item:gram-3}, to receive []{item:tm89}.</summary>
		GRAM_1 = 664,
		/// <summary>Give to the []{pokemon:wingull} on []{location:unova-route-13}, along with []{item:gram-1} and []{item:gram-3}, to receive []{item:tm89}.</summary>
		GRAM_2 = 665,
		/// <summary>Give to the []{pokemon:wingull} on []{location:unova-route-13}, along with []{item:gram-1} and []{item:gram-2}, to receive []{item:tm89}.</summary>
		GRAM_3 = 666,
		/// <summary>Held :   When the holder uses a damaging []{type:dragon}-type move, the move has 1.5× power and this item is consumed.</summary>
		DRAGON_GEM = 668,
		/// <summary>Held :   When the holder uses a damaging []{type:normal}-type move, the move has 1.5× power and this item is consumed.</summary>
		NORMAL_GEM = 669,
		/// <summary>Holds medals recieved in the medal rally.</summary>
		MEDAL_BOX = 670,
		/// <summary>Fuses Kyurem with Reshiram or Zekrom, or splits them apart again.</summary>
		DNA_SPLICERS = 671,
		/// <summary>Grants access to the Nature Preserve.</summary>
		PERMIT = 673,
		/// <summary>Doubles the chance of two Pokémon producing an egg at the daycare every 255 steps.</summary>
		OVAL_CHARM = 674,
		/// <summary>Raises the chance of finding a shiny Pokémon.</summary>
		SHINY_CHARM = 675,
		/// <summary>Required to progress in the Plasma Frigate.</summary>
		PLASMA_CARD = 676,
		/// <summary>Appears in the Café Warehouse on Sunday; return to the customer with a Patrat on Thursday.</summary>
		GRUBBY_HANKY = 677,
		/// <summary>Wakes up the Crustle blocking the way in Seaside Cave.</summary>
		COLRESS_MACHINE = 678,
		/// <summary>Returned to Curtis or Yancy as part of a sidequest.</summary>
		DROPPED_ITEM = 679,
		/// <summary>Switches Tornadus, Thundurus, and Landorus between Incarnate and Therian Forme.</summary>
		REVEAL_GLASS = 681,
		/// <summary>An item to be held by a Pokémon. Attack and Sp. Atk sharply increase if the holder is hit with a move it’s weak to.</summary>
		WEAKNESS_POLICY = 682,
		/// <summary>An item to be held by a Pokémon. This offensive vest raises Sp. Def but prevents the use of status moves.</summary>
		ASSAULT_VEST = 683,
		/// <summary>An item to be held by a Pokémon. It is a stone tablet that boosts the power of Fairy-type moves.</summary>
		PIXIE_PLATE = 684,
		/// <summary>A capsule that allows a Pokémon with two Abilities to switch between these Abilities when it is used.</summary>
		ABILITY_CAPSULE = 685,
		/// <summary>A soft and sweet treat made of fluffy, puffy, whipped and whirled cream. It’s loved by a certain Pokémon.</summary>
		WHIPPED_DREAM = 686,
		/// <summary>A sachet filled with fragrant perfumes that are just slightly too overwhelming. Yet it’s loved by a certain Pokémon.</summary>
		SACHET = 687,
		/// <summary>An item to be held by a Pokémon. It boosts Sp. Def if hit with a Water- type attack. It can only be used once.</summary>
		LUMINOUS_MOSS = 688,
		/// <summary>An item to be held by a Pokémon. It boosts Attack if hit with an Ice-type attack. It can only be used once.</summary>
		SNOWBALL = 689,
		/// <summary>An item to be held by a Pokémon. These goggles protect the holder from both weather-related damage and powder.</summary>
		SAFETY_GOGGLES = 690,
		/// <summary>Mulch to be used in a Berry field. It increases the Berry harvest without the need for particularly diligent care.</summary>
		RICH_MULCH = 691,
		/// <summary>Mulch to be used in a Berry field. It causes strange, sudden mutations based on the combination of Berries.</summary>
		SURPRISE_MULCH = 692,
		/// <summary>Mulch to be used in a Berry field. It increases the Berry harvest that can be grown by diligent watering.</summary>
		BOOST_MULCH = 693,
		/// <summary>Mulch to be used in a Berry field. An amazing Mulch with the effects of Rich, Surprise, and Boost Mulch.</summary>
		AMAZE_MULCH = 694,
		/// <summary>One variety of the mysterious Mega Stones. Have Gengar hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		GENGARITE = 695,
		/// <summary>One variety of the mysterious Mega Stones. Have Gardevoir hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		GARDEVOIRITE = 696,
		/// <summary>One variety of the mysterious Mega Stones. Have Ampharos hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		AMPHAROSITE = 697,
		/// <summary>One variety of the mysterious Mega Stones. Have Venusaur hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		VENUSAURITE = 698,
		/// <summary>One variety of the mysterious Mega Stones. Have Charizard hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		CHARIZARDITE_X = 699,
		/// <summary>One variety of the mysterious Mega Stones. Have Blastoise hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		BLASTOISINITE = 700,
		/// <summary>One variety of the mysterious Mega Stones. Have Mewtwo hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		MEWTWONITE_X = 701,
		/// <summary>One variety of the mysterious Mega Stones. Have Mewtwo hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		MEWTWONITE_Y = 702,
		/// <summary>One variety of the mysterious Mega Stones. Have Blaziken hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		BLAZIKENITE = 703,
		/// <summary>One variety of the mysterious Mega Stones. Have Medicham hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		MEDICHAMITE = 704,
		/// <summary>One variety of the mysterious Mega Stones. Have Houndoom hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		HOUNDOOMINITE = 705,
		/// <summary>One variety of the mysterious Mega Stones. Have Aggron hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		AGGRONITE = 706,
		/// <summary>One variety of the mysterious Mega Stones. Have Banette hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		BANETTITE = 707,
		/// <summary>One variety of the mysterious Mega Stones. Have Tyranitar hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		TYRANITARITE = 708,
		/// <summary>One variety of the mysterious Mega Stones. Have Scizor hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		SCIZORITE = 709,
		/// <summary>One variety of the mysterious Mega Stones. Have Pinsir hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		PINSIRITE = 710,
		/// <summary>One variety of the mysterious Mega Stones. Have Aerodactyl hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		AERODACTYLITE = 711,
		/// <summary>One variety of the mysterious Mega Stones. Have Lucario hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		LUCARIONITE = 712,
		/// <summary>One variety of the mysterious Mega Stones. Have Abomasnow hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		ABOMASITE = 713,
		/// <summary>One variety of the mysterious Mega Stones. Have Kangaskhan hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		KANGASKHANITE = 714,
		/// <summary>One variety of the mysterious Mega Stones. Have Gyarados hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		GYARADOSITE = 715,
		/// <summary>One variety of the mysterious Mega Stones. Have Absol hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		ABSOLITE = 716,
		/// <summary>One variety of the mysterious Mega Stones. Have Charizard hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		CHARIZARDITE_Y = 717,
		/// <summary>One variety of the mysterious Mega Stones. Have Alakazam hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		ALAKAZITE = 718,
		/// <summary>One variety of the mysterious Mega Stones. Have Heracross hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		HERACRONITE = 719,
		/// <summary>One variety of the mysterious Mega Stones. Have Mawile hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		MAWILITE = 720,
		/// <summary>One variety of the mysterious Mega Stones. Have Manectric hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		MANECTITE = 721,
		/// <summary>One variety of the mysterious Mega Stones. Have Garchomp hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		GARCHOMPITE = 722,
		/// <summary>If held by a Pokémon, this Berry will lessen the damage taken from one supereffective Fairy-type attack.</summary>
		ROSELI_BERRY = 723,
		/// <summary>If held by a Pokémon, this Berry will increase the holder’s Defense if it’s hit with a physical move.</summary>
		KEE_BERRY = 724,
		/// <summary>If held by a Pokémon, this Berry will increase the holder’s Sp. Def if it’s hit with a special move.</summary>
		MARANGA_BERRY = 725,
		/// <summary>This special coupon allows you to buy items at a discount when you are shopping at a boutique.</summary>
		DISCOUNT_COUPON = 726,
		/// <summary>An ornament depicting a Pokémon that is venerated as a protector in some region far from Kalos.</summary>
		STRANGE_SOUVENIR = 727,
		/// <summary>A popular treat in Lumiose City. It can be used once to heal all the status conditions of a Pokémon.</summary>
		LUMIOSE_GALETTE = 728,
		/// <summary>A fossil from a prehistoric Pokémon that once lived on the land. It looks as if it could be a piece of a large jaw.</summary>
		JAW_FOSSIL = 729,
		/// <summary>A fossil from a prehistoric Pokémon that once lived on the land. It looks like the impression from a skin sail.</summary>
		SAIL_FOSSIL = 730,
		/// <summary>A gem with an essence of the fey. When held, it strengthens the power of a Fairy-type move one time.</summary>
		FAIRY_GEM = 731,
		/// <summary>This book contains all the points a new Trainer needs to know on a journey. It was handmade by a kind friend.</summary>
		ADVENTURE_RULES = 732,
		/// <summary>A card key that activates the elevator in Lysandre Labs. It is emblazoned with Team Flare’s logo.</summary>
		ELEVATOR_KEY = 733,
		/// <summary>A device that allows users to receive and view hologram clips at any time. It is also used to chat with others.</summary>
		HOLO_CASTER = 734,
		/// <summary>A precious symbol that is awarded only to an individual who has done great things for the Kalos region.</summary>
		HONOR_OF_KALOS = 735,
		/// <summary>A rather curious stone that might appear to be valuable to some. It’s all in the eye of the beholder.</summary>
		INTRIGUING_STONE = 736,
		/// <summary>A rather chic-looking case for carrying contact lenses.</summary>
		LENS_CASE = 737,
		/// <summary>A ticket that was handmade by Looker. It’s decorated with a liberal amount of glittery paint.</summary>
		LOOKER_TICKET = 738,
		/// <summary>This ring contains an untold power that somehow enables Pokémon carrying Mega Stones to Mega Evolve in battle.</summary>
		MEGA_RING = 739,
		/// <summary>This pass serves as an ID card for gaining access to the power plant that lies along Route 13.</summary>
		POWER_PLANT_PASS = 740,
		/// <summary>A letter that Professor Sycamore wrote to your mother. A faint but pleasant perfume seems to cling to the paper.</summary>
		PROFS_LETTER = 741,
		/// <summary>Attaches roller skates to the bottom of your shoes, allowing you to glide quickly around and perform tricks.</summary>
		ROLLER_SKATES = 742,
		/// <summary>A watering can shaped like a Lotad. It helps promote the healthy growth of any Berries planted in good, soft soil.</summary>
		SPRINKLOTAD = 743,
		/// <summary>A commuter pass that allows the holder to ride the TMV between Lumiose City and Kiloude City at any time.</summary>
		TMV_PASS = 744,
		/*// <summary>An attack that makes use of nature’s power. Its effects vary depending on the user’s environment.</summary>
		TM96 = 745,
		/// <summary>The user releases a horrible aura imbued with dark thoughts. This may also make the target flinch.</summary>
		TM97 = 746,
		/// <summary>Striking opponents over and over makes the user’s fists harder. Hitting a target raises the Attack stat.</summary>
		TM98 = 747,
		/// <summary>The user damages opposing Pokémon by emitting a powerful flash.</summary>
		TM99 = 748,
		/// <summary>The user tells the target a secret, and the target loses its ability to concentrate. This lowers the target’s Sp. Atk stat.</summary>
		TM100 = 749,*/
		/// <summary>Held: Allows Latias to Mega Evolve into Mega Latias. : 	Held: Allows Latias to Mega Evolve into Mega Latias.</summary>
		LATIASITE = 760,
		/// <summary>Held: Allows Latios to Mega Evolve into Mega Latios. : 	Held: Allows Latios to Mega Evolve into Mega Latios.</summary>
		LATIOSITE = 761,
		/// <summary>Unknown. : 	Unknown.</summary>
		COMMON_STONE = 762,
		/// <summary>Allows the player to change their lipstick color. : 	Allows the player to change their lipstick color.</summary>
		MAKEUP_BAG = 763,
		/// <summary>Unobtainable, but allows the player to change clothes anywhere. : 	Unobtainable, but allows the player to change clothes anywhere.</summary>
		TRAVEL_TRUNK = 764,
		/// <summary>Cures any major status ailment and confusion. : 	Cures any major status ailment and confusion.</summary>
		SHALOUR_SABLE = 765,
		/// <summary>Unused.  This appears as the girlplayer's Mega Bracelet in Pokémon Contests, but it cannot actually be obtained. : 	Unused Key Stone.</summary>
		MEGA_CHARM = 768,
		/// <summary>Unused.  This is Korrina's Key Stone in X and Y, but it cannot be obtained by the player. : 	Unused NPC Key Stone.</summary>
		MEGA_GLOVE = 769,
		/// <summary>Allows Captain Stern to set out on his expedition. : 	Allows Captain Stern to set out on his expedition.</summary>
		DEVON_PARTS = 770,
		/// <summary>Creates and stores Pokéblocks. : 	Creates and stores Pokéblocks.</summary>
		POKEBLOCK_KIT = 772,
		/// <summary>Unlocks the door to Room 1 in Sea Mauville. : 	Unlocks the door to Room 1 in Sea Mauville.</summary>
		KEY_TO_ROOM_1 = 773,
		/// <summary>Unlocks the door to Room 2 in Sea Mauville. : 	Unlocks the door to Room 2 in Sea Mauville.</summary>
		KEY_TO_ROOM_2 = 774,
		/// <summary>Unlocks the door to Room 4 in Sea Mauville. : 	Unlocks the door to Room 4 in Sea Mauville.</summary>
		KEY_TO_ROOM_4 = 775,
		/// <summary>Unlocks the door to Room 6 in Sea Mauville. : 	Unlocks the door to Room 6 in Sea Mauville.</summary>
		KEY_TO_ROOM_6 = 776,
		/// <summary>Worn by the player while underwater via Dive in Omega Ruby and Alpha Sapphire. : 	Worn by the player while underwater.</summary>
		DEVON_SCUBA_GEAR = 779,
		/// <summary>Worn during Pokémon Contests. : 	Worn during Pokémon Contests.</summary>
		CONTEST_COSTUME__JACKET = 780,
		/// <summary>Allows the player to ride Groudon in the Cave of Origin. : 	Allows the player to ride Groudon in the Cave of Origin.</summary>
		MAGMA_SUIT = 782,
		/// <summary>Allows the player to ride Kyogre in the Cave of Origin. : 	Allows the player to ride Kyogre in the Cave of Origin.</summary>
		AQUA_SUIT = 783,
		/// <summary>Allows the player and their mother to see the star show in the Mossdeep Space Center. : 	Allows the player and their mother to see the star show in the Mossdeep Space Center.</summary>
		PAIR_OF_TICKETS = 784,
		/// <summary>Allows the player's Pokémon to Mega Evolve. : 	Allows the player's Pokémon to Mega Evolve.</summary>
		MEGA_BRACELET = 785,
		/// <summary>Unused.  This is Wally's Key Stone in Omega Ruby and Alpha Sapphire, but it cannot be obtained by the player. : 	Unused NPC Key Stone.</summary>
		MEGA_PENDANT = 786,
		/// <summary>Unused.  This is Maxie's Key Stone in Omega Ruby and Alpha Sapphire, but it cannot be obtained by the player. : 	Unused NPC Key Stone.</summary>
		MEGA_GLASSES = 787,
		/// <summary>Unused.  This is Archie's Key Stone in Omega Ruby and Alpha Sapphire, but it cannot be obtained by the player. : 	Unused NPC Key Stone.</summary>
		MEGA_ANCHOR = 788,
		/// <summary>Unused.  This is Steven's Key Stone in Omega Ruby and Alpha Sapphire, but it cannot be obtained by the player. : 	Unused NPC Key Stone.</summary>
		MEGA_STICKPIN = 789,
		/// <summary>Unused.  This is Lisia's Key Stone in Omega Ruby and Alpha Sapphire, but it cannot be obtained by the player. : 	Unused NPC Key Stone.</summary>
		MEGA_TIARA = 790,
		/// <summary>Unused.  This is Zinnia's Key Stone in Omega Ruby and Alpha Sapphire, but it cannot be obtained by the player. : 	Unused NPC Key Stone.</summary>
		MEGA_ANKLET = 791,
		/// <summary>Held: Allows Swampert to Mega Evolve into Mega Swampert. : 	Held: Allows Swampert to Mega Evolve into Mega Swampert.</summary>
		SWAMPERTITE = 793,
		/// <summary>Held: Allows Sceptile to Mega Evolve into Mega Sceptile. : 	Held: Allows Sceptile to Mega Evolve into Mega Sceptile.</summary>
		SCEPTILITE = 794,
		/// <summary>Held: Allows Sableye to Mega Evolve into Mega Sableye. : 	Held: Allows Sableye to Mega Evolve into Mega Sableye.</summary>
		SABLENITE = 795,
		/// <summary>Held: Allows Altaria to Mega Evolve into Mega Altaria. : 	Held: Allows Altaria to Mega Evolve into Mega Altaria.</summary>
		ALTARIANITE = 796,
		/// <summary>Held: Allows Gallade to Mega Evolve into Mega Gallade. : 	Held: Allows Gallade to Mega Evolve into Mega Gallade.</summary>
		GALLADITE = 797,
		/// <summary>Held: Allows Audino to Mega Evolve into Mega Audino. : 	Held: Allows Audino to Mega Evolve into Mega Audino.</summary>
		AUDINITE = 798,
		/// <summary>Held: Allows Metagross to Mega Evolve into Mega Metagross. : 	Held: Allows Metagross to Mega Evolve into Mega Metagross.</summary>
		METAGROSSITE = 799,
		/// <summary>Held: Allows Sharpedo to Mega Evolve into Mega Sharpedo. : 	Held: Allows Sharpedo to Mega Evolve into Mega Sharpedo.</summary>
		SHARPEDONITE = 800,
		/// <summary>Held: Allows Slowbro to Mega Evolve into Mega Slowbro. : 	Held: Allows Slowbro to Mega Evolve into Mega Slowbro.</summary>
		SLOWBRONITE = 801,
		/// <summary>Held: Allows Steelix to Mega Evolve into Mega Steelix. : 	Held: Allows Steelix to Mega Evolve into Mega Steelix.</summary>
		STEELIXITE = 802,
		/// <summary>Held: Allows Pidgeot to Mega Evolve into Mega Pidgeot. : 	Held: Allows Pidgeot to Mega Evolve into Mega Pidgeot.</summary>
		PIDGEOTITE = 803,
		/// <summary>Held: Allows Glalie to Mega Evolve into Mega Glalie. : 	Held: Allows Glalie to Mega Evolve into Mega Glalie.</summary>
		GLALITITE = 804,
		/// <summary>Held: Allows Diancie to Mega Evolve into Mega Diancie. : 	Held: Allows Diancie to Mega Evolve into Mega Diancie.</summary>
		DIANCITE = 805,
		/// <summary>Transforms Hoopa into its Unbound form for up to three days. : 	Transforms Hoopa into its Unbound form for up to three days.</summary>
		PRISON_BOTTLE = 806,
		/// <summary>Unused.  This appears as the boy player's Mega Bracelet in Pokémon Contests, but it cannot actually be obtained. : 	Unused Key Stone.</summary>
		MEGA_CUFF = 807,
		/// <summary>Held: Allows Camerupt to Mega Evolve into Mega Camerupt. : 	Held: Allows Camerupt to Mega Evolve into Mega Camerupt.</summary>
		CAMERUPTITE = 808,
		/// <summary>Held: Allows Lopunny to Mega Evolve into Mega Lopunny. : 	Held: Allows Lopunny to Mega Evolve into Mega Lopunny.</summary>
		LOPUNNITE = 809,
		/// <summary>Held: Allows Salamence to Mega Evolve into Mega Salamence. : 	Held: Allows Salamence to Mega Evolve into Mega Salamence.</summary>
		SALAMENCITE = 810,
		/// <summary>Held: Allows Beedrill to Mega Evolve into Mega Beedrill. : 	Held: Allows Beedrill to Mega Evolve into Mega Beedrill.</summary>
		BEEDRILLITE = 811,
		/// <summary>Allows the player's Pokémon to Mega Evolve. : 	Allows the player's Pokémon to Mega Evolve.</summary>
		KEY_STONE = 814,
		/// <summary>Causes the Meteorite to transform to its final form, allowing Rayquaza to Mega Evolve. : 	Causes the Meteorite to transform to its final form, allowing Rayquaza to Mega Evolve.</summary>
		METEORITE_SHARD = 815,
		/// <summary>Summons Latias or Latios for a ride. : 	Summons Latias or Latios for a ride.</summary>
		EON_FLUTE = 816,
		/// <summary>Held: Allows a Pokémon to use the Z-move equivalents of its Normal moves. : 	Held: Allows a Pokémon to use the Z-move equivalents of its Normal moves.</summary>
		NORMALIUM_Z__HELD = 817,
		/// <summary>Held: Allows a Pokémon to use the Z-move equivalents of its Fire moves. : 	Held: Allows a Pokémon to use the Z-move equivalents of its Fire moves.</summary>
		FIRIUM_Z__HELD = 818,
		/// <summary>Held: Allows a Pokémon to use the Z-move equivalents of its Water moves. : 	Held: Allows a Pokémon to use the Z-move equivalents of its Water moves.</summary>
		WATERIUM_Z__HELD = 819,
		/// <summary>Held: Allows a Pokémon to use the Z-move equivalents of its Electric moves. : 	Held: Allows a Pokémon to use the Z-move equivalents of its Electric moves.</summary>
		ELECTRIUM_Z__HELD = 820,
		/// <summary>Held: Allows a Pokémon to use the Z-move equivalents of its Grass moves. : 	Held: Allows a Pokémon to use the Z-move equivalents of its Grass moves.</summary>
		GRASSIUM_Z__HELD = 821,
		/// <summary>Held: Allows a Pokémon to use the Z-move equivalents of its Ice moves. : 	Held: Allows a Pokémon to use the Z-move equivalents of its Ice moves.</summary>
		ICIUM_Z__HELD = 822,
		/// <summary>Held: Allows a Pokémon to use the Z-move equivalents of its Fighting moves. : 	Held: Allows a Pokémon to use the Z-move equivalents of its Fighting moves.</summary>
		FIGHTINIUM_Z__HELD = 823,
		/// <summary>Held: Allows a Pokémon to use the Z-move equivalents of its Poison moves. : 	Held: Allows a Pokémon to use the Z-move equivalents of its Poison moves.</summary>
		POISONIUM_Z__HELD = 824,
		/// <summary>Held: Allows a Pokémon to use the Z-move equivalents of its Ground moves. : 	Held: Allows a Pokémon to use the Z-move equivalents of its Ground moves.</summary>
		GROUNDIUM_Z__HELD = 825,
		/// <summary>Held: Allows a Pokémon to use the Z-move equivalents of its Flying moves. : 	Held: Allows a Pokémon to use the Z-move equivalents of its Flying moves.</summary>
		FLYINIUM_Z__HELD = 826,
		/// <summary>Held: Allows a Pokémon to use the Z-move equivalents of its Psychic moves. : 	Held: Allows a Pokémon to use the Z-move equivalents of its Psychic moves.</summary>
		PSYCHIUM_Z__HELD = 827,
		/// <summary>Held: Allows a Pokémon to use the Z-move equivalents of its Bug moves. : 	Held: Allows a Pokémon to use the Z-move equivalents of its Bug moves.</summary>
		BUGINIUM_Z__HELD = 828,
		/// <summary>Held: Allows a Pokémon to use the Z-move equivalents of its Rock moves. : 	Held: Allows a Pokémon to use the Z-move equivalents of its Rock moves.</summary>
		ROCKIUM_Z__HELD = 829,
		/// <summary>Held: Allows a Pokémon to use the Z-move equivalents of its Ghost moves. : 	Held: Allows a Pokémon to use the Z-move equivalents of its Ghost moves.</summary>
		GHOSTIUM_Z__HELD = 830,
		/// <summary>Held: Allows a Pokémon to use the Z-move equivalents of its Dragon moves. : 	Held: Allows a Pokémon to use the Z-move equivalents of its Dragon moves.</summary>
		DRAGONIUM_Z__HELD = 831,
		/// <summary>Held: Allows a Pokémon to use the Z-move equivalents of its Dark moves. : 	Held: Allows a Pokémon to use the Z-move equivalents of its Dark moves.</summary>
		DARKINIUM_Z__HELD = 832,
		/// <summary>Held: Allows a Pokémon to use the Z-move equivalents of its Steel moves. : 	Held: Allows a Pokémon to use the Z-move equivalents of its Steel moves.</summary>
		STEELIUM_Z__HELD = 833,
		/// <summary>Held: Allows a Pokémon to use the Z-move equivalents of its Fairy moves. : 	Held: Allows a Pokémon to use the Z-move equivalents of its Fairy moves.</summary>
		FAIRIUM_Z__HELD = 834,
		/// <summary>Held: Allows []{pokemon:pikachu} to upgrade []{move:volt-tackle} into []{move:catastropika}. : 	Held: Allows Pikachu to upgrade Volt Tackle into Catastropika.</summary>
		PIKANIUM_Z__HELD = 835,
		/// <summary>Trade to Mr. Hyper to set one of a Pokémon's genes to 31. : 	Trade to Mr. Hyper to maximize one of a Pokémon's genes.</summary>
		BOTTLE_CAP = 836,
		/// <summary>Trade to Mr. Hyper to set all of a Pokémon's genes to 31. : 	Trade to Mr. Hyper to maximize all of a Pokémon's genes.</summary>
		GOLD_BOTTLE_CAP = 837,
		/// <summary>Allows the player's Pokémon to use Z-moves. : 	Allows the player's Pokémon to use Z-moves.</summary>
		Z_RING = 838,
		/// <summary>Held: Allows []{pokemon:decidueye} to upgrade []{move:spirit-shackle} into []{move:sinister-arrow-raid}. : 	Held: Allows Decidueye to upgrade Spirit Shackle into Sinister Arrow Raid.</summary>
		DECIDIUM_Z__HELD = 839,
		/// <summary>Held: Allows []{pokemon:incineroar} to upgrade []{move:darkest-lariat} into []{move:malicious-moonsault}. : 	Held: Allows Incineroar to upgrade Darkest Lariat into Malicious Moonsault.</summary>
		INCINIUM_Z__HELD = 840,
		/// <summary>Held: Allows []{pokemon:primarina} to upgrade []{move:sparkling-aria} into []{move:oceanic-operetta}. : 	Held: Allows Primarina to upgrade Sparkling Aria into Oceanic Operetta.</summary>
		PRIMARIUM_Z__HELD = 841,
		/// <summary>Held: Allows []{pokemon:tapu-koko}, []{pokemon:tapu-lele}, []{pokemon:tapu-bulu}, and []{pokemon:tapu-fini} to upgrade []{move:natures-madness} into []{move:guardian-of-alola}. : 	Held: Allows the Tapus to upgrade Nature's Madness into Guardian of Alola.</summary>
		TAPUNIUM_Z__HELD = 842,
		/// <summary>Held: Allows []{pokemon:marshadow} to upgrade []{move:spectral-thief} into []{move:soul-stealing-7-star-strike}. : 	Held: Allows Marshadow to upgrade Spectral Thief into Soul-Stealing 7 Star Strike.</summary>
		MARSHADIUM_Z__HELD = 843,
		/// <summary>Held: Allows Alola []{pokemon:raichu} to upgrade []{move:thunderbolt} into []{move:stoked-sparksurfer}. : 	Held: Allows Alola Raichu to upgrade Thunderbolt into Stoked Sparksurfer.</summary>
		ALORAICHIUM_Z__HELD = 844,
		/// <summary>Held: Allows []{pokemon:snorlax} to upgrade []{move:giga-impact} into []{move:pulverizing-pancake}. : 	Held: Allows Snorlax to upgrade Giga Impact into Pulverizing Pancake.</summary>
		SNORLIUM_Z__HELD = 845,
		/// <summary>Held: Allows []{pokemon:eevee} to upgrade []{move:last-resort} into []{move:extreme-evoboost}. : 	Held: Allows Eevee to upgrade Last Resort into Extreme Evoboost.</summary>
		EEVIUM_Z__HELD = 846,
		/// <summary>Held: Allows []{pokemon:mew} to upgrade []{move:psychic} into []{move:genesis-supernova}. : 	Held: Allows Mew to upgrade Psychic into Genesis Supernova.</summary>
		MEWNIUM_Z__HELD = 847,
		/// <summary>Held: Allows cap-wearing []{pokemon:pikachu} to upgrade []{move:thunderbolt} into []{move:10-000-000-volt-thunderbolt}. : 	Held: Allows cap-wearing Pikachu to upgrade Thunderbolt into 10,000,000 Volt Thunderbolt.</summary>
		PIKASHUNIUM_Z__HELD = 877,
		/// <summary>Holds ingredients during Mallow's trial. : 	Holds ingredients during Mallow's trial.</summary>
		FORAGE_BAG = 878,
		/// <summary>Allows the player to fish for Pokémon. : 	Allows the player to fish for Pokémon.</summary>
		FISHING_ROD = 879,
		/// <summary>Lost by Professor Kukui. : 	Lost by Professor Kukui.</summary>
		PROFESSORS_MASK = 880,
		/// <summary>Hosts a mission in Festival Plaza. : 	Hosts a mission in Festival Plaza.</summary>
		FESTIVAL_TICKET = 881,
		/// <summary>Required to obtain a Z-Ring. : 	Required to obtain a Z-Ring.</summary>
		SPARKLING_STONE = 882,
		/// <summary>Makes wild Pokémon more likely to summon allies.  Held: increases the holder's Speed by one stage when affected by Intimidate. : 	Makes wild Pokémon more likely to summon allies.  Held: increases the holder's Speed by one stage when affected by Intimidate.</summary>
		ADRENALINE_ORB = 883,
		/// <summary>Contains collected Zygarde cells/cores.  Can teach Zygarde moves. : 	Contains collected Zygarde cells/cores.  Can teach Zygarde moves.</summary>
		ZYGARDE_CUBE = 884,
		/// <summary>Used on a party Pokémon :   Evolves an Alola []{pokemon:sandshrew} into Alola[]{pokemon:sandslash} or an Alola[]{pokemon:vulpix} into Alola[]{pokemon:ninetales}. : 	Evolves an Alola Sandshrew into Alola Sandslash or an Alola Vulpix into Alola Ninetales.</summary>
		ICE_STONE = 885,
		/// <summary>Allows the player to summon a Ride Pokémon.  Unused, as this can be done simply by pressing Y. : 	Allows the player to summon a Ride Pokémon.</summary>
		RIDE_PAGER = 886,
		/// <summary>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.If the wild Pokémon is an Ultra Beast, this ball has a catch rate of 5Ã—.  Otherwise, it has a catch rate of 0.1Ã—. If used in a trainer battle, nothing happens and the ball is lost. : 	Tries to catch a wild Pokémon.Success rate is 5Ã— for Ultra Beasts and 0.1Ã— for all other Pokémon.</ summary >
		BEAST_BALL = 887,
		/// <summary>Cures major status ailments and confusion. : 	Cures major status ailments and confusion.</summary>
		BIG_MALASADA = 888,
		/// <summary>Changes Oricorio to Baile Style.  Single-use and cannot be used in battle. : 	Changes Oricorio to Baile Style.</summary>
		RED_NECTAR = 889,
		/// <summary>Changes Oricorio to Pom-Pom Style.  Single-use and cannot be used in battle. : 	Changes Oricorio to Pom-Pom Style.</summary>
		YELLOW_NECTAR = 890,
		/// <summary>Changes Oricorio to Paâ€™u Style.  Single-use and cannot be used in battle. : 	Changes Oricorio to Paâ€™u Style.</summary>
		PINK_NECTAR = 891,
		/// <summary>Changes Oricorio to Sensu Style.  Single-use and cannot be used in battle. : 	Changes Oricorio to Sensu Style.</summary>
		PURPLE_NECTAR = 892,
		/// <summary>Evolves Nebby into Solgaleo when used at the Altar of the Sunne. : 	Evolves Nebby into Solgaleo when used at the Altar of the Sunne.</summary>
		SUN_FLUTE = 893,
		/// <summary>Evolves Nebby into Lunala when used at the Altar of the Moone. : 	Evolves Nebby into Lunala when used at the Altar of the Moone.</summary>
		MOON_FLUTE = 894,
		/// <summary>Unlocks Looker's motel room on Route 8. : 	Unlocks Looker's motel room on Route 8.</summary>
		ENIGMATIC_CARD = 895,
		/// <summary>Held: When the holder changes the Terrain (whether by move or ability), it will last  8 turns instead of 5. : 	Held: Extends the holder's Terrain effects to 8 turns.</summary>
		TERRAIN_EXTENDER = 896,
		/// <summary>Held: Prevents side effects of contact moves used on the holder. : 	Held: Prevents side effects of contact moves used on the holder.</summary>
		PROTECTIVE_PADS = 897,
		/// <summary>Held: If the holder enters battle during Electric Terrain, or if Electric Terrain is activated while the holder is in battle, this item is consumed and the holder's Defense raises by one stage. : 	Held: Consumed on Electric Terrain and raises the holder's Defense by one stage.</summary>
		ELECTRIC_SEED = 898,
		/// <summary>Held: If the holder enters battle during Psychic Terrain, or if Psychic Terrain is activated while the holder is in battle, this item is consumed and the holder's Special Defense raises by one stage. : 	Held: Consumed on Psychic Terrain and raises the holder's Special Defense by one stage.</summary>
		PSYCHIC_SEED = 899,
		/// <summary>Held: If the holder enters battle during Misty Terrain, or if Misty Terrain is activated while the holder is in battle, this item is consumed and the holder's Special Defense raises by one stage. : 	Held: Consumed on Misty Terrain and raises the holder's Special Defense by one stage.</summary>
		MISTY_SEED = 900,
		/// <summary>Held: If the holder enters battle during Grassy Terrain, or if Grassy Terrain is activated while the holder is in battle, this item is consumed and the holder's Defense raises by one stage. : 	Held: Consumed on Grassy Terrain and raises the holder's Defense by one stage.</summary>
		GRASSY_SEED = 901,
		/// <summary>Held: Changes Silvally to its Fighting form.  Changes Multi-Attack's type to Fighting. : 	Held: Changes Silvally to its Fighting form.  Changes Multi-Attack's type to Fighting.</summary>
		FIGHTING_MEMORY = 902,
		/// <summary>Held: Changes Silvally to its Flying form.  Changes Multi-Attack's type to Flying. : 	Held: Changes Silvally to its Flying form.  Changes Multi-Attack's type to Flying.</summary>
		FLYING_MEMORY = 903,
		/// <summary>Held: Changes Silvally to its Poison form.  Changes Multi-Attack's type to Poison. : 	Held: Changes Silvally to its Poison form.  Changes Multi-Attack's type to Poison.</summary>
		POISON_MEMORY = 904,
		/// <summary>Held: Changes Silvally to its Ground form.  Changes Multi-Attack's type to Ground. : 	Held: Changes Silvally to its Ground form.  Changes Multi-Attack's type to Ground.</summary>
		GROUND_MEMORY = 905,
		/// <summary>Held: Changes Silvally to its Rock form.  Changes Multi-Attack's type to Rock. : 	Held: Changes Silvally to its Rock form.  Changes Multi-Attack's type to Rock.</summary>
		ROCK_MEMORY = 906,
		/// <summary>Held: Changes Silvally to its Bug form.  Changes Multi-Attack's type to Bug. : 	Held: Changes Silvally to its Bug form.  Changes Multi-Attack's type to Bug.</summary>
		BUG_MEMORY = 907,
		/// <summary>Held: Changes Silvally to its Ghost form.  Changes Multi-Attack's type to Ghost. : 	Held: Changes Silvally to its Ghost form.  Changes Multi-Attack's type to Ghost.</summary>
		GHOST_MEMORY = 908,
		/// <summary>Held: Changes Silvally to its Steel form.  Changes Multi-Attack's type to Steel. : 	Held: Changes Silvally to its Steel form.  Changes Multi-Attack's type to Steel.</summary>
		STEEL_MEMORY = 909,
		/// <summary>Held: Changes Silvally to its Fire form.  Changes Multi-Attack's type to Fire. : 	Held: Changes Silvally to its Fire form.  Changes Multi-Attack's type to Fire.</summary>
		FIRE_MEMORY = 910,
		/// <summary>Held: Changes Silvally to its Water form.  Changes Multi-Attack's type to Water. : 	Held: Changes Silvally to its Water form.  Changes Multi-Attack's type to Water.</summary>
		WATER_MEMORY = 911,
		/// <summary>Held: Changes Silvally to its Grass form.  Changes Multi-Attack's type to Grass. : 	Held: Changes Silvally to its Grass form.  Changes Multi-Attack's type to Grass.</summary>
		GRASS_MEMORY = 912,
		/// <summary>Held: Changes Silvally to its Electric form.  Changes Multi-Attack's type to Electric. : 	Held: Changes Silvally to its Electric form.  Changes Multi-Attack's type to Electric.</summary>
		ELECTRIC_MEMORY = 913,
		/// <summary>Held: Changes Silvally to its Psychic form.  Changes Multi-Attack's type to Psychic. : 	Held: Changes Silvally to its Psychic form.  Changes Multi-Attack's type to Psychic.</summary>
		PSYCHIC_MEMORY = 914,
		/// <summary>Held: Changes Silvally to its Ice form.  Changes Multi-Attack's type to Ice. : 	Held: Changes Silvally to its Ice form.  Changes Multi-Attack's type to Ice.</summary>
		ICE_MEMORY = 915,
		/// <summary>Held: Changes Silvally to its Dragon form.  Changes Multi-Attack's type to Dragon. : 	Held: Changes Silvally to its Dragon form.  Changes Multi-Attack's type to Dragon.</summary>
		DRAGON_MEMORY = 916,
		/// <summary>Held: Changes Silvally to its Dark form.  Changes Multi-Attack's type to Dark. : 	Held: Changes Silvally to its Dark form.  Changes Multi-Attack's type to Dark.</summary>
		DARK_MEMORY = 917,
		/// <summary>Held: Changes Silvally to its Fairy form.  Changes Multi-Attack's type to Fairy. : 	Held: Changes Silvally to its Fairy form.  Changes Multi-Attack's type to Fairy.</summary>
		FAIRY_MEMORY = 918,
		/// <summary>XXX new effect for bike--green : 	XXX new effect for bike--green</summary>
		BIKE__GREEN = 919,
		/// <summary>XXX new effect for storage-key--galactic-warehouse : 	XXX new effect for storage-key--galactic-warehouse</summary>
		STORAGE_KEY__GALACTIC_WAREHOUSE = 920,
		/// <summary>XXX new effect for basement-key--goldenrod : 	XXX new effect for basement-key--goldenrod</summary>
		BASEMENT_KEY__GOLDENROD = 921,
		/// <summary>XXX new effect for xtranceiver--red : 	XXX new effect for xtranceiver--red</summary>
		XTRANCEIVER__RED = 922,
		/// <summary>XXX new effect for xtranceiver--yellow : 	XXX new effect for xtranceiver--yellow</summary>
		XTRANCEIVER__YELLOW = 923,
		/// <summary>XXX new effect for dna-splicers--merge : 	XXX new effect for dna-splicers--merge</summary>
		DNA_SPLICERS__MERGE = 924,
		/// <summary>XXX new effect for dna-splicers--split : 	XXX new effect for dna-splicers--split</summary>
		DNA_SPLICERS__SPLIT = 925,
		/// <summary>XXX new effect for dropped-item--red : 	XXX new effect for dropped-item--red</summary>
		DROPPED_ITEM__RED = 926,
		/// <summary>XXX new effect for dropped-item--yellow : 	XXX new effect for dropped-item--yellow</summary>
		DROPPED_ITEM__YELLOW = 927,
		/// <summary>XXX new effect for holo-caster--green : 	XXX new effect for holo-caster--green</summary>
		HOLO_CASTER__GREEN = 928,
		/// <summary>XXX new effect for bike--yellow : 	XXX new effect for bike--yellow</summary>
		BIKE__YELLOW = 929,
		/// <summary>XXX new effect for holo-caster--red : 	XXX new effect for holo-caster--red</summary>
		HOLO_CASTER__RED = 930,
		/// <summary>XXX new effect for basement-key--new-mauville : 	XXX new effect for basement-key--new-mauville</summary>
		BASEMENT_KEY__NEW_MAUVILLE = 931,
		/// <summary>XXX new effect for storage-key--sea-mauville : 	XXX new effect for storage-key--sea-mauville</summary>
		STORAGE_KEY__SEA_MAUVILLE = 932,
		/// <summary>XXX new effect for ss-ticket--hoenn : 	XXX new effect for ss-ticket--hoenn</summary>
		SS_TICKET__HOENN = 933,
		/// <summary>XXX new effect for contest-costume--dress : 	XXX new effect for contest-costume--dress</summary>
		CONTEST_COSTUME__DRESS = 934,
		/// <summary>XXX new effect for meteorite--2 : 	XXX new effect for meteorite--2</summary>
		METEORITE__2 = 935,
		/// <summary>XXX new effect for meteorite--3 : 	XXX new effect for meteorite--3</summary>
		METEORITE__3 = 936,
		/// <summary>XXX new effect for meteorite--4 : 	XXX new effect for meteorite--4</summary>
		METEORITE__4 = 937,
		/// <summary>XXX new effect for normalium-z--bag : 	XXX new effect for normalium-z--bag</summary>
		NORMALIUM_Z__BAG = 938,
		/// <summary>XXX new effect for firium-z--bag : 	XXX new effect for firium-z--bag</summary>
		FIRIUM_Z__BAG = 939,
		/// <summary>XXX new effect for waterium-z--bag : 	XXX new effect for waterium-z--bag</summary>
		WATERIUM_Z__BAG = 940,
		/// <summary>XXX new effect for electrium-z--bag : 	XXX new effect for electrium-z--bag</summary>
		ELECTRIUM_Z__BAG = 941,
		/// <summary>XXX new effect for grassium-z--bag : 	XXX new effect for grassium-z--bag</summary>
		GRASSIUM_Z__BAG = 942,
		/// <summary>XXX new effect for icium-z--bag : 	XXX new effect for icium-z--bag</summary>
		ICIUM_Z__BAG = 943,
		/// <summary>XXX new effect for fightinium-z--bag : 	XXX new effect for fightinium-z--bag</summary>
		FIGHTINIUM_Z__BAG = 944,
		/// <summary>XXX new effect for poisonium-z--bag : 	XXX new effect for poisonium-z--bag</summary>
		POISONIUM_Z__BAG = 945,
		/// <summary>XXX new effect for groundium-z--bag : 	XXX new effect for groundium-z--bag</summary>
		GROUNDIUM_Z__BAG = 946,
		/// <summary>XXX new effect for flyinium-z--bag : 	XXX new effect for flyinium-z--bag</summary>
		FLYINIUM_Z__BAG = 947,
		/// <summary>XXX new effect for psychium-z--bag : 	XXX new effect for psychium-z--bag</summary>
		PSYCHIUM_Z__BAG = 948,
		/// <summary>XXX new effect for buginium-z--bag : 	XXX new effect for buginium-z--bag</summary>
		BUGINIUM_Z__BAG = 949,
		/// <summary>XXX new effect for rockium-z--bag : 	XXX new effect for rockium-z--bag</summary>
		ROCKIUM_Z__BAG = 950,
		/// <summary>XXX new effect for ghostium-z--bag : 	XXX new effect for ghostium-z--bag</summary>
		GHOSTIUM_Z__BAG = 951,
		/// <summary>XXX new effect for dragonium-z--bag : 	XXX new effect for dragonium-z--bag</summary>
		DRAGONIUM_Z__BAG = 952,
		/// <summary>XXX new effect for darkinium-z--bag : 	XXX new effect for darkinium-z--bag</summary>
		DARKINIUM_Z__BAG = 953,
		/// <summary>XXX new effect for steelium-z--bag : 	XXX new effect for steelium-z--bag</summary>
		STEELIUM_Z__BAG = 954,
		/// <summary>XXX new effect for fairium-z--bag : 	XXX new effect for fairium-z--bag</summary>
		FAIRIUM_Z__BAG = 955,
		/// <summary>XXX new effect for pikanium-z--bag : 	XXX new effect for pikanium-z--bag</summary>
		PIKANIUM_Z__BAG = 956,
		/// <summary>XXX new effect for decidium-z--bag : 	XXX new effect for decidium-z--bag</summary>
		DECIDIUM_Z__BAG = 957,
		/// <summary>XXX new effect for incinium-z--bag : 	XXX new effect for incinium-z--bag</summary>
		INCINIUM_Z__BAG = 958,
		/// <summary>XXX new effect for primarium-z--bag : 	XXX new effect for primarium-z--bag</summary>
		PRIMARIUM_Z__BAG = 959,
		/// <summary>XXX new effect for tapunium-z--bag : 	XXX new effect for tapunium-z--bag</summary>
		TAPUNIUM_Z__BAG = 960,
		/// <summary>XXX new effect for marshadium-z--bag : 	XXX new effect for marshadium-z--bag</summary>
		MARSHADIUM_Z__BAG = 961,
		/// <summary>XXX new effect for aloraichium-z--bag : 	XXX new effect for aloraichium-z--bag</summary>
		ALORAICHIUM_Z__BAG = 962,
		/// <summary>XXX new effect for snorlium-z--bag : 	XXX new effect for snorlium-z--bag</summary>
		SNORLIUM_Z__BAG = 963,
		/// <summary>XXX new effect for eevium-z--bag : 	XXX new effect for eevium-z--bag</summary>
		EEVIUM_Z__BAG = 964,
		/// <summary>XXX new effect for mewnium-z--bag : 	XXX new effect for mewnium-z--bag</summary>
		MEWNIUM_Z__BAG = 965,
		/// <summary>XXX new effect for pikashunium-z--bag : 	XXX new effect for pikashunium-z--bag</summary>
		PIKASHUNIUM_Z__BAG = 966,
		/// <summary>XXX new effect for solganium-z--held : 	XXX new effect for solganium-z--held</summary>
		SOLGANIUM_Z__HELD = 967,
		/// <summary>XXX new effect for lunalium-z--held : 	XXX new effect for lunalium-z--held</summary>
		LUNALIUM_Z__HELD = 968,
		/// <summary>XXX new effect for ultranecrozium-z--held : 	XXX new effect for ultranecrozium-z--held</summary>
		ULTRANECROZIUM_Z__HELD = 969,
		/// <summary>XXX new effect for mimikium-z--held : 	XXX new effect for mimikium-z--held</summary>
		MIMIKIUM_Z__HELD = 970,
		/// <summary>XXX new effect for lycanium-z--held : 	XXX new effect for lycanium-z--held</summary>
		LYCANIUM_Z__HELD = 971,
		/// <summary>XXX new effect for kommonium-z--held : 	XXX new effect for kommonium-z--held</summary>
		KOMMONIUM_Z__HELD = 972,
		/// <summary>XXX new effect for solganium-z--bag : 	XXX new effect for solganium-z--bag</summary>
		SOLGANIUM_Z__BAG = 973,
		/// <summary>XXX new effect for lunalium-z--bag : 	XXX new effect for lunalium-z--bag</summary>
		LUNALIUM_Z__BAG = 974,
		/// <summary>XXX new effect for ultranecrozium-z--bag : 	XXX new effect for ultranecrozium-z--bag</summary>
		ULTRANECROZIUM_Z__BAG = 975,
		/// <summary>XXX new effect for mimikium-z--bag : 	XXX new effect for mimikium-z--bag</summary>
		MIMIKIUM_Z__BAG = 976,
		/// <summary>XXX new effect for lycanium-z--bag : 	XXX new effect for lycanium-z--bag</summary>
		LYCANIUM_Z__BAG = 977,
		/// <summary>XXX new effect for kommonium-z--bag : 	XXX new effect for kommonium-z--bag</summary>
		KOMMONIUM_Z__BAG = 978,
		/// <summary>XXX new effect for z-power-ring : 	XXX new effect for z-power-ring</summary>
		Z_POWER_RING = 979,
		/// <summary>XXX new effect for pink-petal : 	XXX new effect for pink-petal</summary>
		PINK_PETAL = 980,
		/// <summary>XXX new effect for orange-petal : 	XXX new effect for orange-petal</summary>
		ORANGE_PETAL = 981,
		/// <summary>XXX new effect for blue-petal : 	XXX new effect for blue-petal</summary>
		BLUE_PETAL = 982,
		/// <summary>XXX new effect for red-petal : 	XXX new effect for red-petal</summary>
		RED_PETAL = 983,
		/// <summary>XXX new effect for green-petal : 	XXX new effect for green-petal</summary>
		GREEN_PETAL = 984,
		/// <summary>XXX new effect for yellow-petal : 	XXX new effect for yellow-petal</summary>
		YELLOW_PETAL = 985,
		/// <summary>XXX new effect for purple-petal : 	XXX new effect for purple-petal</summary>
		PURPLE_PETAL = 986,
		/// <summary>XXX new effect for rainbow-flower : 	XXX new effect for rainbow-flower</summary>
		RAINBOW_FLOWER = 987,
		/// <summary>XXX new effect for surge-badge : 	XXX new effect for surge-badge</summary>
		SURGE_BADGE = 988,
		/// <summary>XXX new effect for n-solarizer--merge : 	XXX new effect for n-solarizer--merge</summary>
		N_SOLARIZER__MERGE = 989,
		/// <summary>XXX new effect for n-lunarizer--merge : 	XXX new effect for n-lunarizer--merge</summary>
		N_LUNARIZER__MERGE = 990,
		/// <summary>XXX new effect for n-solarizer--split : 	XXX new effect for n-solarizer--split</summary>
		N_SOLARIZER__SPLIT = 991,
		/// <summary>XXX new effect for n-lunarizer--split : 	XXX new effect for n-lunarizer--split</summary>
		N_LUNARIZER__SPLIT = 992,
		/// <summary>XXX new effect for ilimas-normalium-z : 	XXX new effect for ilimas-normalium-z</summary>
		ILIMAS_NORMALIUM_Z = 993,
		/// <summary>XXX new effect for left-poke-ball : 	XXX new effect for left-poke-ball</summary>
		LEFT_POKE_BALL = 994,
		/// <summary>XXX new effect for roto-hatch : 	XXX new effect for roto-hatch</summary>
		ROTO_HATCH = 995,
		/// <summary>XXX new effect for roto-bargain : 	XXX new effect for roto-bargain</summary>
		ROTO_BARGAIN = 996,
		/// <summary>XXX new effect for roto-prize-money : 	XXX new effect for roto-prize-money</summary>
		ROTO_PRIZE_MONEY = 997,
		/// <summary>XXX new effect for roto-exp-points : 	XXX new effect for roto-exp-points</summary>
		ROTO_EXP_POINTS = 998,
		/// <summary>XXX new effect for roto-friendship : 	XXX new effect for roto-friendship</summary>
		ROTO_FRIENDSHIP = 999,
		/// <summary>XXX new effect for roto-encounter : 	XXX new effect for roto-encounter</summary>
		ROTO_ENCOUNTER = 1000,
		/// <summary>XXX new effect for roto-stealth : 	XXX new effect for roto-stealth</summary>
		ROTO_STEALTH = 1001,
		/// <summary>XXX new effect for roto-hp-restore : 	XXX new effect for roto-hp-restore</summary>
		ROTO_HP_RESTORE = 1002,
		/// <summary>XXX new effect for roto-pp-restore : 	XXX new effect for roto-pp-restore</summary>
		ROTO_PP_RESTORE = 1003,
		/// <summary>XXX new effect for roto-boost : 	XXX new effect for roto-boost</summary>
		ROTO_BOOST = 1004,
		/// <summary>XXX new effect for roto-catch : 	XXX new effect for roto-catch</summary>
		ROTO_CATCH = 1005
	}

	public enum ItemPockets
	{
		MISC = 1,
		MEDICINE = 2,
		POKEBALL = 3,
		/// <summary>
		/// TMs
		/// </summary>
		MACHINE = 4,
		/// <summary>
		/// Plants?
		/// </summary>
		BERRY = 5,
		MAIL = 6,
		BATTLE = 7,
		/// <summary>
		/// Items that are not 'stackable' and therefore should take up 1 individual item spot per (if multiples)
		/// Or possibly limit and restrict to one per user
		/// </summary>
		KEY = 8
	}

	/// <summary>
	/// Item Category determines both the item's effect
	/// and the bag-pocket that the item belongs to.
	/// </summary>
	/// <remarks>
	/// Can determine an <see cref="itemEffect"/> by the category it belongs to.
	/// </remarks>
	public enum ItemCategory
	{
		COLLECTIBLES = 9, //PocketId = 1
		EVOLUTION = 10, //PocketId = 1
		SPELUNKING = 11, //PocketId = 1
		HELD_ITEMS = 12, //PocketId = 1
		CHOICE = 13, //PocketId = 1
		EFFORT_TRAINING = 14, //PocketId = 1
		BAD_HELD_ITEMS = 15, //PocketId = 1
		TRAINING = 16, //PocketId = 1
		PLATES = 17, //PocketId = 1
		SPECIES_SPECIFIC = 18, //PocketId = 1
		TYPE_ENHANCEMENT = 19, //PocketId = 1
		LOOT = 24, //PocketId = 1
		MULCH = 32, //PocketId = 1
		DEX_COMPLETION = 35, //PocketId = 1
		SCARVES = 36, //PocketId = 1
		JEWELS = 42, //PocketId = 1
		MEGA_STONES = 44, //PocketId = 1

		VITAMINS = 26, //PocketId = 2
		HEALING = 27, //PocketId = 2
		PP_RECOVERY = 28, //PocketId = 2
		REVIVAL = 29, //PocketId = 2
		STATUS_CURES = 30, //PocketId = 2

		SPECIAL_BALLS = 33, //PocketId = 3
		STANDARD_BALLS = 34, //PocketId = 3
		APRICORN_BALLS = 39, //PocketId = 3

		ALL_MACHINES = 37, //PocketId = 4

		EFFORT_DROP = 2, //PocketId = 5
		MEDICINE = 3, //PocketId = 5
		OTHER = 4, //PocketId = 5
		IN_A_PINCH = 5, //PocketId = 5
		PICKY_HEALING = 6, //PocketId = 5
		TYPE_PROTECTION = 7, //PocketId = 5
		BAKING_ONLY = 8, //PocketId = 5

		ALL_MAIL = 25, //PocketId = 6

		STAT_BOOSTS = 1, //PocketId = 7
		FLUTES = 38, //PocketId = 7
		MIRACLE_SHOOTER = 43, //PocketId = 7

		EVENT_ITEMS = 20, //PocketId = 8
		GAMEPLAY = 21, //PocketId = 8
		PLOT_ADVANCEMENT = 22, //PocketId = 8
		UNUSED = 23, //PocketId = 8
		APRICORN_BOX = 40, //PocketId = 8
		DATA_CARDS = 41, //PocketId = 8
		XY_UNKNOWN = 10001, //PocketId = 8
	}

	/// <summary>
	/// Effects that occur when items are thrown at target.
	/// Not all items can be thrown(?)...
	/// </summary>
	/// <remarks>Didnt have access to variable names...</remarks>
	public enum ItemFlingEffect
	{
		NONE,
		/// <summary>
		/// Badly poisons the target.
		/// </summary>
		BADLY_POISONS = 1,
		/// <summary>
		/// Burns the target.
		/// </summary>
		BURNS = 2,
		/// <summary>
		/// Immediately activates the berry's effect on the target.
		/// </summary>
		USE_BERRY = 3,
		/// <summary>
		/// Immediately activates the herb's effect on the target.
		/// </summary>
		USE_HERB = 4,
		/// <summary>
		/// Paralyzes the target.
		/// </summary>
		PARALYZES = 5,
		/// <summary>
		/// Poisons the target.
		/// </summary>
		POISONS = 6,
		/// <summary>
		/// Target will flinch if it has not yet gone this turn.
		/// </summary>
		FLINCH = 7
	}
	public enum ItemUseResults
	{
		/// <summary>
		/// not used
		/// </summary>
		NotUsed = 0,
		/// <summary>
		/// used, item not consumed
		/// </summary>
		UsedNotConsumed = 1,
		/// <summary>
		/// close the Bag to use, item not consumed
		/// </summary>
		CloseBagNotConsumed = 2,
		/// <summary>
		/// used, item consumed
		/// </summary>
		UsedItemConsumed = 3,
		/// <summary>
		/// close the Bag to use, item consumed
		/// </summary>
		CloseBagItemConsumed = 4
	}

	namespace Plants
	{
		public enum Flavours
		{
			Spicy	= 1
			,Dry	= 2
			,Sweet	= 3
			,Bitter	= 4
			,Sour	= 5
		}

		public enum FirmnessLevel
		{
			VerySoft,
			Soft,
			Hard,
			VeryHard,
			SuperHard
		}
	}
}