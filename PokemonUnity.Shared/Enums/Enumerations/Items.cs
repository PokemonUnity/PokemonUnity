namespace PokemonUnity.Shared.Enums
{
	/// <summary>
	/// Item ids are connected to XML file. 
	/// </summary>
	/// <remarks>
	/// Running off of genVI. 
	/// Be sure to overwrite both if modifying.
	/// Replace "[HP]{mechanic:hp}" in summary-tags with
	/// "<see cref="PokemonEssentials.Interface.PokeBattle.IPokemon.HP"/>"
	/// </remarks>
	/// Custom Values are in negative?
	public class Items : Enumeration
	{
		public string Name { get { return string.Format("Item_Text_Name{0}", Id); } }
		public string Description { get { return string.Format("Item_Text_Description{0}", Id); } }
		protected Items(int id, string name) : base(id, name) { }
		public static readonly Items EXP_ALL_OFF			= new Items(-3,	"EXP_ALL_OFF");
		public static readonly Items EXP_ALL			= new Items(-2,	"EXP_ALL");
		public static readonly Items RUNNING_SHOES			= new Items(-1,	"RUNNING_SHOES");
		public static readonly Items NONE			= new Items(0,	"NONE");
		/// <summary>Used in battle :   [Catches]{mechanic:catch} a wild Pokémon without fail.      If used in a trainer battle, nothing happens and the ball is lost.</summary>
		public static readonly Items MASTER_BALL			= new Items(1,	"MASTER_BALL");
		/// <summary>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon, using a catch rate of 2×.      If used in a trainer battle, nothing happens and the ball is lost.</summary>
		public static readonly Items ULTRA_BALL			= new Items(2,	"ULTRA_BALL");
		/// <summary>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon, using a catch rate of 1.5×.      If used in a trainer battle, nothing happens and the ball is lost.</summary>
		public static readonly Items GREAT_BALL			= new Items(3,	"GREAT_BALL");
		/// <summary>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon, using a catch rate of 1×.      If used in a trainer battle, nothing happens and the ball is lost.</summary>
		public static readonly Items POKE_BALL			= new Items(4,	"POKE_BALL");
		/// <summary>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon, using a catch rate of 1.5×.  This item can only be used in the []{location:great-marsh} or []{location:kanto-safari-zone}.</summary>
		public static readonly Items SAFARI_BALL			= new Items(5,	"SAFARI_BALL");
		/// <summary>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.  If the wild Pokémon is []{type:water}- or []{type:bug}-type, this ball has a catch rate of 3×.  Otherwise, it has a catch rate of 1×.      If used in a trainer battle, nothing happens and the ball is lost.</summary>
		public static readonly Items NET_BALL			= new Items(6,	"NET_BALL");
		/// <summary>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.  If the wild Pokémon was encountered by surfing or fishing, this ball has a catch rate of 3.5×.  Otherwise, it has a catch rate of 1×.      If used in a trainer battle, nothing happens and the ball is lost.</summary>
		public static readonly Items DIVE_BALL			= new Items(7,	"DIVE_BALL");
		/// <summary>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.  Has a catch rate of given by `(40 - level) / 10`, where `level` is the wild Pokémon's level, to a maximum of 3.9× for level 1 Pokémon.  If the wild Pokémon's level is higher than 30, this ball has a catch rate of 1×.      If used in a trainer battle, nothing happens and the ball is lost.</summary>
		public static readonly Items NEST_BALL			= new Items(8,	"NEST_BALL");
		/// <summary>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.  If the wild Pokémon's species is marked as caught in the trainer's Pokédex, this ball has a catch rate of 3×.  Otherwise, it has a catch rate of 1×.      If used in a trainer battle, nothing happens and the ball is lost.</summary>
		public static readonly Items REPEAT_BALL			= new Items(9,	"REPEAT_BALL");
		/// <summary>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.  Has a catch rate of 1.1× on the first turn of the battle and increases by 0.1× every turn, to a maximum of 4× on turn 30.      If used in a trainer battle, nothing happens and the ball is lost.</summary>
		public static readonly Items TIMER_BALL			= new Items(10,	"TIMER_BALL");
		/// <summary>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon, using a catch rate of 1×.  Whenever the caught Pokémon's [happiness]{mechanic:happiness} increases, it increases by one extra point.      If used in a trainer battle, nothing happens and the ball is lost.</summary>
		public static readonly Items LUXURY_BALL			= new Items(11,	"LUXURY_BALL");
		/// <summary>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon, using a catch rate of 1×.      If used in a trainer battle, nothing happens and the ball is lost.</summary>
		public static readonly Items PREMIER_BALL			= new Items(12,	"PREMIER_BALL");
		/// <summary>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.  If it's currently nighttime or the wild Pokémon was encountered while walking in a cave, this ball has a catch rate of 3.5×.  Otherwise, it has a catch rate of 1×.      If used in a trainer battle, nothing happens and the ball is lost.</summary>
		public static readonly Items DUSK_BALL			= new Items(13,	"DUSK_BALL");
		/// <summary>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon, using a catch rate of 1×.  The caught Pokémon's [HP]{mechanic:hp} is immediately restored, [PP]{mechanic:pp} for all its moves is restored, and any [status ailment]{mechanic:status-ailment} is cured.      If used in a trainer battle, nothing happens and the ball is lost.</summary>
		public static readonly Items HEAL_BALL			= new Items(14,	"HEAL_BALL");
		/// <summary>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon, using a catch rate of 4× on the first turn of a battle, but 1× any other time.      If used in a trainer battle, nothing happens and the ball is lost.</summary>
		public static readonly Items QUICK_BALL			= new Items(15,	"QUICK_BALL");
		/// <summary>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon, using a catch rate of 1×.      If used in a trainer battle, nothing happens and the ball is lost.</summary>
		public static readonly Items CHERISH_BALL			= new Items(16,	"CHERISH_BALL");
		/// <summary>Used on a friendly Pokémon :   Restores 20 [HP]{mechanic:hp}.</summary>
		public static readonly Items POTION			= new Items(17,	"POTION");
		/// <summary>Used on a party Pokémon :   Cures [poison]{mechanic:poison}.</summary>
		public static readonly Items ANTIDOTE			= new Items(18,	"ANTIDOTE");
		/// <summary>Used on a party Pokémon :   Cures a [burn]{mechanic:burn}.</summary>
		public static readonly Items BURN_HEAL			= new Items(19,	"BURN_HEAL");
		/// <summary>Used on a party Pokémon :   Cures [freezing]{mechanic:freezing}.</summary>
		public static readonly Items ICE_HEAL			= new Items(20,	"ICE_HEAL");
		/// <summary>Used on a party Pokémon :   Cures [sleep]{mechanic:sleep}.</summary>
		public static readonly Items AWAKENING			= new Items(21,	"AWAKENING");
		/// <summary>Used on a party Pokémon :   Cures [paralysis]{mechanic:paralysis}.</summary>
		public static readonly Items PARALYZE_HEAL			= new Items(22,	"PARALYZE_HEAL");
		/// <summary>Used on a party Pokémon :   Restores [HP]{mechanic:hp} to full and cures any [status ailment]{mechanic:status-ailment} and [confusion]{mechanic:confusion}.</summary>
		public static readonly Items FULL_RESTORE			= new Items(23,	"FULL_RESTORE");
		/// <summary>Used on a party Pokémon :   Restores [HP]{mechanic:hp} to full.</summary>
		public static readonly Items MAX_POTION			= new Items(24,	"MAX_POTION");
		/// <summary>Used on a party Pokémon :   Restores 200 [HP]{mechanic:hp}.</summary>
		public static readonly Items HYPER_POTION			= new Items(25,	"HYPER_POTION");
		/// <summary>Used on a party Pokémon :   Restores 50 [HP]{mechanic:hp}.</summary>
		public static readonly Items SUPER_POTION			= new Items(26,	"SUPER_POTION");
		/// <summary>Used on a party Pokémon :   Cures any [status ailment]{mechanic:status-ailment} and [confusion]{mechanic:confusion}.</summary>
		public static readonly Items FULL_HEAL			= new Items(27,	"FULL_HEAL");
		/// <summary>Used on a party Pokémon :   Revives the Pokémon and restores half its [HP]{mechanic:hp}.</summary>
		public static readonly Items REVIVE			= new Items(28,	"REVIVE");
		/// <summary>Used on a party Pokémon :   Revives the Pokémon and restores its [HP]{mechanic:hp} to full.</summary>
		public static readonly Items MAX_REVIVE			= new Items(29,	"MAX_REVIVE");
		/// <summary>Used on a party Pokémon :   Restores 50 [HP]{mechanic:hp}.</summary>
		public static readonly Items FRESH_WATER			= new Items(30,	"FRESH_WATER");
		/// <summary>Used on a party Pokémon :   Restores 60 [HP]{mechanic:hp}.</summary>
		public static readonly Items SODA_POP			= new Items(31,	"SODA_POP");
		/// <summary>Used on a party Pokémon :   Restores 80 [HP]{mechanic:hp}.</summary>
		public static readonly Items LEMONADE			= new Items(32,	"LEMONADE");
		/// <summary>Used on a party Pokémon :   Restores 100 [HP]{mechanic:hp}.</summary>
		public static readonly Items MOOMOO_MILK			= new Items(33,	"MOOMOO_MILK");
		/// <summary>Used on a party Pokémon :   Restores 50 [HP]{mechanic:hp}.  Decreases [happiness]{mechanic:happiness} by 5/5/10.</summary>
		public static readonly Items ENERGY_POWDER			= new Items(34,	"ENERGY_POWDER");
		/// <summary>Used on a party Pokémon :   Restores 200 [HP]{mechanic:hp}.  Decreases [happiness]{mechanic:happiness} by 10/10/15.</summary>
		public static readonly Items ENERGY_ROOT			= new Items(35,	"ENERGY_ROOT");
		/// <summary>Used on a party Pokémon :   Cures any [status ailment]{mechanic:status-ailment}.  Decreases [happiness]{mechanic:happiness} by 5/5/10.</summary>
		public static readonly Items HEAL_POWDER			= new Items(36,	"HEAL_POWDER");
		/// <summary>Used on a party Pokémon :   Revives a [fainted]{mechanic:faint} Pokémon and restores its [HP]{mechanic:hp} to full.  Decreases [happiness]{mechanic:happiness} by 10/10/15.</summary>
		public static readonly Items REVIVAL_HERB			= new Items(37,	"REVIVAL_HERB");
		/// <summary>Used on a party Pokémon :   Restores 10 [PP]{mechanic:pp} for a selected move.</summary>
		public static readonly Items ETHER			= new Items(38,	"ETHER");
		/// <summary>Used on a party Pokémon :   Restores [PP]{mechanic:pp} to full for a selected move.</summary>
		public static readonly Items MAX_ETHER			= new Items(39,	"MAX_ETHER");
		/// <summary>Used on a party Pokémon :   Restores 10 [PP]{mechanic:pp} for each move.</summary>
		public static readonly Items ELIXIR			= new Items(40,	"ELIXIR");
		/// <summary>Used on a party Pokémon :   Restores [PP]{mechanic:pp} to full for each move.</summary>
		public static readonly Items MAX_ELIXIR			= new Items(41,	"MAX_ELIXIR");
		/// <summary>Used on a party Pokémon :   Cures any [status ailment]{mechanic:status-ailment} and [confusion]{mechanic:confusion}.</summary>
		public static readonly Items LAVA_COOKIE			= new Items(42,	"LAVA_COOKIE");
		/// <summary>Used on a party Pokémon :   Restores 20 [HP]{mechanic:hp}.</summary>
		public static readonly Items BERRY_JUICE			= new Items(43,	"BERRY_JUICE");
		/// <summary>Used :   Revives all [fainted]{mechanic:faint} Pokémon in the party and restores their [HP]{mechanic:hp} to full.</summary>
		public static readonly Items SACRED_ASH			= new Items(44,	"SACRED_ASH");
		/// <summary>Used on a party Pokémon :   Increases [HP]{mechanic:hp} [effort]{mechanic:effort} by 10, but won't increase it beyond 100.  Increases [happiness]{mechanic:happiness} by 5/3/2.</summary>
		public static readonly Items HP_UP			= new Items(45,	"HP_UP");
		/// <summary>Used on a party Pokémon :   Increases [Attack]{mechanic:attack} [effort]{mechanic:effort} by 10, but won't increase it beyond 100.  Increases [happiness]{mechanic:happiness} by 5/3/2.</summary>
		public static readonly Items PROTEIN			= new Items(46,	"PROTEIN");
		/// <summary>Used on a party Pokémon :   Increases [Defense]{mechanic:defense} [effort]{mechanic:effort} by 10, but won't increase it beyond 100.  Increases [happiness]{mechanic:happiness} by 5/3/2.</summary>
		public static readonly Items IRON			= new Items(47,	"IRON");
		/// <summary>Used on a party Pokémon :   Increases [Speed]{mechanic:speed} [effort]{mechanic:effort} by 10, but won't increase it beyond 100.  Increases [happiness]{mechanic:happiness} by 5/3/2.</summary>
		public static readonly Items CARBOS			= new Items(48,	"CARBOS");
		/// <summary>Used on a party Pokémon :   Increases [Special Attack]{mechanic:special-attack} [effort]{mechanic:effort} by 10, but won't increase it beyond 100.  Increases [happiness]{mechanic:happiness} by 5/3/2.</summary>
		public static readonly Items CALCIUM			= new Items(49,	"CALCIUM");
		/// <summary>Used on a party Pokémon :   Increases level by 1.  Increases [happiness]{mechanic:happiness} by 5/3/2.</summary>
		public static readonly Items RARE_CANDY			= new Items(50,	"RARE_CANDY");
		/// <summary>Used on a party Pokémon :   Increases a selected move's max [PP]{mechanic:pp} by 20% its original max PP, to a maximum of 1.6×.  Increases [happiness]{mechanic:happiness} by 5/3/2.</summary>
		public static readonly Items PP_UP			= new Items(51,	"PP_UP");
		/// <summary>Used on a party Pokémon :   Increases [Special Defense]{mechanic:special-defense} [effort]{mechanic:effort} by 10, but won't increase it beyond 100.  Increases [happiness]{mechanic:happiness} by 5/3/2.</summary>
		public static readonly Items ZINC			= new Items(52,	"ZINC");
		/// <summary>Used on a party Pokémon :   Increases a selected move's max [PP]{mechanic:pp} to 1.6× its original max PP.  Increases [happiness]{mechanic:happiness} by 5/3/2.</summary>
		public static readonly Items PP_MAX			= new Items(53,	"PP_MAX");
		/// <summary>Used on a party Pokémon :   Cures any [status ailment]{mechanic:status-ailment} and [confusion]{mechanic:confusion}.</summary>
		public static readonly Items OLD_GATEAU			= new Items(54,	"OLD_GATEAU");
		/// <summary>Used on a party Pokémon in battle :   Protects the target's stats from being [lowered]{mechanic:lower} for the next five turns.  Increases happiness by 1/1/0.</summary>
		public static readonly Items GUARD_SPEC			= new Items(55,	"GUARD_SPEC");
		/// <summary>Used on a party Pokémon in battle :   Increases the target's [critical hit chance]{mechanic:critical-hit-chance} by one stage until it leaves the field.  Increases happiness by 1/1/0.</summary>
		public static readonly Items DIRE_HIT			= new Items(56,	"DIRE_HIT");
		/// <summary>Used on a party Pokémon in battle :   [Raises]{mechanic:raise} the target's [Attack]{mechanic:attack} by one stage.  Increases happiness by 1/1/0.</summary>
		public static readonly Items X_ATTACK			= new Items(57,	"X_ATTACK");
		/// <summary>Used on a party Pokémon in battle :   [Raises]{mechanic:raise} the target's [Defense]{mechanic:defense} by one stage.  Increases happiness by 1/1/0.</summary>
		public static readonly Items X_DEFENSE			= new Items(58,	"X_DEFENSE");
		/// <summary>Used on a party Pokémon in battle :   [Raises]{mechanic:raise} the target's [Speed]{mechanic:speed} by one stage.  Increases happiness by 1/1/0.</summary>
		public static readonly Items X_SPEED			= new Items(59,	"X_SPEED");
		/// <summary>Used on a party Pokémon in battle :   [Raises]{mechanic:raise} the target's [accuracy]{mechanic:accuracy} by one stage.  Increases happiness by 1/1/0.</summary>
		public static readonly Items X_ACCURACY			= new Items(60,	"X_ACCURACY");
		/// <summary>Used on a party Pokémon in battle :   [Raises]{mechanic:raise} the target's [Special Attack]{mechanic:special-attack} by one stage.  Increases happiness by 1/1/0.</summary>
		public static readonly Items X_SP_ATK			= new Items(61,	"X_SP_ATK");
		/// <summary>Used on a party Pokémon in battle :   [Raises]{mechanic:raise} the target's [Special Defense]{mechanic:special-defense} by one stage.  Increases happiness by 1/1/0.</summary>
		public static readonly Items X_SP_DEF			= new Items(62,	"X_SP_DEF");
		/// <summary>Used in battle :   Ends a wild battle.  Cannot be used in trainer battles.</summary>
		public static readonly Items POKE_DOLL			= new Items(63,	"POKE_DOLL");
		/// <summary>Used in battle :   Ends a wild battle.  Cannot be used in trainer battles.</summary>
		public static readonly Items FLUFFY_TAIL			= new Items(64,	"FLUFFY_TAIL");
		/// <summary>Used on a party Pokémon :   Cures [sleep]{mechanic:sleep}.</summary>
		public static readonly Items BLUE_FLUTE			= new Items(65,	"BLUE_FLUTE");
		/// <summary>Used on a party Pokémon in battle :   Cures [confusion]{mechanic:confusion}.</summary>
		public static readonly Items YELLOW_FLUTE			= new Items(66,	"YELLOW_FLUTE");
		/// <summary>Used on a party Pokémon in battle :   Cures [attraction]{mechanic:attraction}.</summary>
		public static readonly Items RED_FLUTE			= new Items(67,	"RED_FLUTE");
		/// <summary>Used outside of battle :   Decreases the wild Pokémon encounter rate by 50%.</summary>
		public static readonly Items BLACK_FLUTE			= new Items(68,	"BLACK_FLUTE");
		/// <summary>Used outside of battle :   Doubles the wild Pokémon encounter rate.</summary>
		public static readonly Items WHITE_FLUTE			= new Items(69,	"WHITE_FLUTE");
		/// <summary>No effect.</summary>
		public static readonly Items SHOAL_SALT			= new Items(70,	"SHOAL_SALT");
		/// <summary>No effect.</summary>
		public static readonly Items SHOAL_SHELL			= new Items(71,	"SHOAL_SHELL");
		/// <summary>No effect.  In Diamond and Pearl, trade ten for a []{move:sunny-day} [TM]{item:tm11} in the house midway along the southern section of []{location:sinnoh-route-212}.  In Platinum, trade to [move tutors]{mechanic:move-tutor} on []{location:sinnoh-route-212}, in []{location:snowpoint-city}, and in the []{location:survival-area}.  Eight shards total are required per tutelage, but the particular combination of colors varies by move.  In HeartGold and SoulSilver, trade one for a []{item:cheri-berry}, a []{item:leppa-berry}, and a []{item:pecha-berry} with the Juggler near the Pokémon Center in []{location:violet-city}.  In HeartGold and SoulSilver, trade one for a []{item:persim-berry}, a []{item:pomeg-berry}, and a []{item:razz-berry} with the Juggler near the []{location:pal-park} entrance in []{location:fuchsia-city}.</summary>
		public static readonly Items RED_SHARD			= new Items(72,	"RED_SHARD");
		/// <summary>No effect.  In Diamond and Pearl, trade ten for a []{move:rain-dance} [TM]{item:tm18} in the house midway along the southern section of []{location:sinnoh-route-212}.  In Platinum, trade to [move tutors]{mechanic:move-tutor} on []{location:sinnoh-route-212}, in []{location:snowpoint-city}, and in the []{location:survival-area}.  Eight shards total are required per tutelage, but the particular combination of colors varies by move.  In HeartGold and SoulSilver, trade one for a []{item:chesto-berry}, an []{item:oran-berry}, and a []{item:wiki-berry} with the Juggler near the Pokémon Center in []{location:violet-city}.  In HeartGold and SoulSilver, trade one for a []{item:bluk-berry}, a []{item:cornn-berry}, and a []{item:kelpsy-berry} with the Juggler near the []{location:pal-park} entrance in []{location:fuchsia-city}.</summary>
		public static readonly Items BLUE_SHARD			= new Items(73,	"BLUE_SHARD");
		/// <summary>No effect.  In Diamond and Pearl, trade ten for a []{move:sandstorm} [TM]{item:tm37} in the house midway along the southern section of []{location:sinnoh-route-212}.  In Platinum, trade to [move tutors]{mechanic:move-tutor} on []{location:sinnoh-route-212}, in []{location:snowpoint-city}, and in the []{location:survival-area}.  Eight shards total are required per tutelage, but the particular combination of colors varies by move.  In HeartGold and SoulSilver, trade one for an []{item:aspear-berry}, a []{item:iapapa-berry}, and a []{item:sitrus-berry} with the Juggler near the Pokémon Center in []{location:violet-city}.  In HeartGold and SoulSilver, trade one for a []{item:grepa-berry}, a []{item:nomel-berry}, and a []{item:pinap-berry} with the Juggler near the []{location:pal-park} entrance in []{location:fuchsia-city}.</summary>
		public static readonly Items YELLOW_SHARD			= new Items(74,	"YELLOW_SHARD");
		/// <summary>No effect.  In Diamond and Pearl, trade ten for a []{move:hail} [TM]{item:tm07} in the house midway along the southern section of []{location:sinnoh-route-212}.  In Platinum, trade to [move tutors]{mechanic:move-tutor} on []{location:sinnoh-route-212}, in []{location:snowpoint-city}, and in the []{location:survival-area}.  Eight shards total are required per tutelage, but the particular combination of colors varies by move.  In HeartGold and SoulSilver, trade one for an []{item:aguav-berry}, a []{item:lum-berry}, and a []{item:rawst-berry} with the Juggler near the Pokémon Center in []{location:violet-city}.  In HeartGold and SoulSilver, trade one for a []{item:durin-berry}, a []{item:hondew-berry}, and a []{item:wepear-berry} with the Juggler near the []{location:pal-park} entrance in []{location:fuchsia-city}.</summary>
		public static readonly Items GREEN_SHARD			= new Items(75,	"GREEN_SHARD");
		/// <summary>Used outside of battle :   Trainer will skip encounters with wild Pokémon of a lower level than the lead party Pokémon.  This effect wears off after the trainer takes 200 steps.</summary>
		public static readonly Items SUPER_REPEL			= new Items(76,	"SUPER_REPEL");
		/// <summary>Used outside of battle :   Trainer will skip encounters with wild Pokémon of a lower level than the lead party Pokémon.  This effect wears off after the trainer takes 250 steps.</summary>
		public static readonly Items MAX_REPEL			= new Items(77,	"MAX_REPEL");
		/// <summary>Used outside of battle :   Transports the trainer to the last-entered dungeon entrance.  Cannot be used outside, in buildings, or in []{location:distortion-world}, []{location:sinnoh-hall-of-origin-1}, []{location:spear-pillar}, or []{location:turnback-cave}.</summary>
		public static readonly Items ESCAPE_ROPE			= new Items(78,	"ESCAPE_ROPE");
		/// <summary>Used outside of battle :   Trainer will skip encounters with wild Pokémon of a lower level than the lead party Pokémon.  This effect wears off after the trainer takes 100 steps.</summary>
		public static readonly Items REPEL			= new Items(79,	"REPEL");
		/// <summary>Used on a party Pokémon :   Evolves a []{pokemon:cottonee} into []{pokemon:whimsicott}, a []{pokemon:gloom} into []{pokemon:bellossom}, a []{pokemon:petilil} into []{pokemon:lilligant}, or a []{pokemon:sunkern} into []{pokemon:sunflora}.</summary>
		public static readonly Items SUN_STONE			= new Items(80,	"SUN_STONE");
		/// <summary>Used on a party Pokémon :   Evolves a []{pokemon:clefairy} into []{pokemon:clefable}, a []{pokemon:jigglypuff} into []{pokemon:wigglytuff}, a []{pokemon:munna} into []{pokemon:musharna}, a []{pokemon:nidorina} into []{pokemon:nidoqueen}, a []{pokemon:nidorino} into []{pokemon:nidoking}, or a []{pokemon:skitty} into []{pokemon:delcatty}.</summary>
		public static readonly Items MOON_STONE			= new Items(81,	"MOON_STONE");
		/// <summary>Used on a party Pokémon :   Evolves an []{pokemon:eevee} into []{pokemon:flareon}, a []{pokemon:growlithe} into []{pokemon:arcanine}, a []{pokemon:pansear} into []{pokemon:simisear}, or a []{pokemon:vulpix} into []{pokemon:ninetales}.</summary>
		public static readonly Items FIRE_STONE			= new Items(82,	"FIRE_STONE");
		/// <summary>Used on a party Pokémon :   Evolves an []{pokemon:eelektrik} into []{pokemon:eelektross}, an []{pokemon:eevee} into []{pokemon:jolteon}, or a []{pokemon:pikachu} into []{pokemon:raichu}.</summary>
		public static readonly Items THUNDER_STONE			= new Items(83,	"THUNDER_STONE");
		/// <summary>Used on a party Pokémon :   Evolves an []{pokemon:eevee} into []{pokemon:vaporeon}, a []{pokemon:lombre} into []{pokemon:ludicolo}, a []{pokemon:panpour} into []{pokemon:simipour}, a []{pokemon:poliwhirl} into []{pokemon:poliwrath}, a []{pokemon:shellder} into []{pokemon:cloyster}, or a []{pokemon:staryu} into []{pokemon:starmie}.</summary>
		public static readonly Items WATER_STONE			= new Items(84,	"WATER_STONE");
		/// <summary>Used on a party Pokémon :   Evolves an []{pokemon:exeggcute} into []{pokemon:exeggutor}, a []{pokemon:gloom} into []{pokemon:vileplume}, a []{pokemon:nuzleaf} into []{pokemon:shiftry}, a []{pokemon:pansage} into []{pokemon:simisage}, or a []{pokemon:weepinbell} into []{pokemon:victreebel}.</summary>
		public static readonly Items LEAF_STONE			= new Items(85,	"LEAF_STONE");
		/// <summary>Vendor trash.</summary>
		public static readonly Items TINY_MUSHROOM			= new Items(86,	"TINY_MUSHROOM");
		/// <summary>Vendor trash.</summary>
		public static readonly Items BIG_MUSHROOM			= new Items(87,	"BIG_MUSHROOM");
		/// <summary>Vendor trash.</summary>
		public static readonly Items PEARL			= new Items(88,	"PEARL");
		/// <summary>Vendor trash.</summary>
		public static readonly Items BIG_PEARL			= new Items(89,	"BIG_PEARL");
		/// <summary>Vendor trash.</summary>
		public static readonly Items STARDUST			= new Items(90,	"STARDUST");
		/// <summary>Vendor trash.</summary>
		public static readonly Items STAR_PIECE			= new Items(91,	"STAR_PIECE");
		/// <summary>Vendor trash.</summary>
		public static readonly Items NUGGET			= new Items(92,	"NUGGET");
		/// <summary>Trade one to the Move Relearner near the shore in []{location:pastoria-city} or with the Move Deleter in []{location:blackthorn-city} to teach one party Pokémon a prior level-up move.</summary>
		public static readonly Items HEART_SCALE			= new Items(93,	"HEART_SCALE");
		/// <summary>Used outside of battle :   Immediately triggers a wild Pokémon battle, as long as the trainer is somewhere with wild Pokémon—i.e., in tall grass, in a cave, or surfing.  Can be smeared on sweet-smelling trees to attract tree-dwelling Pokémon after six hours.</summary>
		public static readonly Items HONEY			= new Items(94,	"HONEY");
		/// <summary>Used on a patch of soil :   Plant's growth stages will each last 25% less time.  Dries soil out more quickly.</summary>
		public static readonly Items GROWTH_MULCH			= new Items(95,	"GROWTH_MULCH");
		/// <summary>Used on a patch of soil :   Plant's growth stages will each last 25% more time.  Dries soil out more slowly.</summary>
		public static readonly Items DAMP_MULCH			= new Items(96,	"DAMP_MULCH");
		/// <summary>Used on a patch of soil :   Fully-grown plant will last 25% longer before dying and possibly regrowing.</summary>
		public static readonly Items STABLE_MULCH			= new Items(97,	"STABLE_MULCH");
		/// <summary>Used on a path of soil :   Plant will regrow after dying 25% more times.</summary>
		public static readonly Items GOOEY_MULCH			= new Items(98,	"GOOEY_MULCH");
		/// <summary>Give to a scientist in the []{location:mining-museum} in []{location:oreburgh-city} or the Museum of Science in []{location:pewter-city} to receive a []{pokemon:lileep}.</summary>
		public static readonly Items ROOT_FOSSIL			= new Items(99,	"ROOT_FOSSIL");
		/// <summary>Give to a scientist in the []{location:mining-museum} in []{location:oreburgh-city} or the Museum of Science in []{location:pewter-city} to receive a []{pokemon:anorith}.</summary>
		public static readonly Items CLAW_FOSSIL			= new Items(100,	"CLAW_FOSSIL");
		/// <summary>Give to a scientist in the []{location:mining-museum} in []{location:oreburgh-city} or the Museum of Science in []{location:pewter-city} to receive a []{pokemon:omanyte}.</summary>
		public static readonly Items HELIX_FOSSIL			= new Items(101,	"HELIX_FOSSIL");
		/// <summary>Give to a scientist in the []{location:mining-museum} in []{location:oreburgh-city} or the Museum of Science in []{location:pewter-city} to receive a []{pokemon:kabuto}.</summary>
		public static readonly Items DOME_FOSSIL			= new Items(102,	"DOME_FOSSIL");
		/// <summary>Give to a scientist in the []{location:mining-museum} in []{location:oreburgh-city} or the Museum of Science in []{location:pewter-city} to receive a []{pokemon:aerodactyl}.</summary>
		public static readonly Items OLD_AMBER			= new Items(103,	"OLD_AMBER");
		/// <summary>Give to a scientist in the []{location:mining-museum} in []{location:oreburgh-city} or the Museum of Science in []{location:pewter-city} to receive a []{pokemon:shieldon}.</summary>
		public static readonly Items ARMOR_FOSSIL			= new Items(104,	"ARMOR_FOSSIL");
		/// <summary>Give to a scientist in the []{location:mining-museum} in []{location:oreburgh-city} or the Museum of Science in []{location:pewter-city} to receive a []{pokemon:cranidos}.</summary>
		public static readonly Items SKULL_FOSSIL			= new Items(105,	"SKULL_FOSSIL");
		/// <summary>Vendor trash.</summary>
		public static readonly Items RARE_BONE			= new Items(106,	"RARE_BONE");
		/// <summary>Used on a party Pokémon :   Evolves a []{pokemon:minccino} into []{pokemon:cinccino}, a []{pokemon:roselia} into []{pokemon:roserade}, or a []{pokemon:togetic} into []{pokemon:togekiss}.</summary>
		public static readonly Items SHINY_STONE			= new Items(107,	"SHINY_STONE");
		/// <summary>Used on a party Pokémon :   Evolves a []{pokemon:lampent} into []{pokemon:chandelure}, a []{pokemon:misdreavus} into []{pokemon:mismagius}, or a []{pokemon:murkrow} into []{pokemon:honchkrow}.</summary>
		public static readonly Items DUSK_STONE			= new Items(108,	"DUSK_STONE");
		/// <summary>Used on a party Pokémon :   Evolves a male []{pokemon:kirlia} into []{pokemon:gallade} or a female []{pokemon:snorunt} into []{pokemon:froslass}.</summary>
		public static readonly Items DAWN_STONE			= new Items(109,	"DAWN_STONE");
		/// <summary>Held by []{pokemon:happiny} :   Holder evolves into []{pokemon:chansey} when it levels up during the daytime.</summary>
		public static readonly Items OVAL_STONE			= new Items(110,	"OVAL_STONE");
		/// <summary>Place in the tower on []{location:sinnoh-route-209}.  Check the stone to encounter a []{pokemon:spiritomb}, as long as the trainer's Underground status card counts at least 32 greetings.</summary>
		public static readonly Items ODD_KEYSTONE			= new Items(111,	"ODD_KEYSTONE");
		/// <summary>Held by []{pokemon:dialga} :   Holder's []{type:dragon}- and []{type:steel}-type moves have 1.2× their usual power.</summary>
		public static readonly Items ADAMANT_ORB			= new Items(112,	"ADAMANT_ORB");
		/// <summary>Held by []{pokemon:palkia} :   Holder's []{type:dragon}- and []{type:water}-type moves have 1.2× their usual power.</summary>
		public static readonly Items LUSTROUS_ORB			= new Items(113,	"LUSTROUS_ORB");
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		public static readonly Items GRASS_MAIL			= new Items(114,	"GRASS_MAIL");
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		public static readonly Items FLAME_MAIL			= new Items(115,	"FLAME_MAIL");
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		public static readonly Items BUBBLE_MAIL			= new Items(116,	"BUBBLE_MAIL");
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		public static readonly Items BLOOM_MAIL			= new Items(117,	"BLOOM_MAIL");
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		public static readonly Items TUNNEL_MAIL			= new Items(118,	"TUNNEL_MAIL");
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		public static readonly Items STEEL_MAIL			= new Items(119,	"STEEL_MAIL");
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		public static readonly Items HEART_MAIL			= new Items(120,	"HEART_MAIL");
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		public static readonly Items SNOW_MAIL			= new Items(121,	"SNOW_MAIL");
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		public static readonly Items SPACE_MAIL			= new Items(122,	"SPACE_MAIL");
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		public static readonly Items AIR_MAIL			= new Items(123,	"AIR_MAIL");
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		public static readonly Items MOSAIC_MAIL			= new Items(124,	"MOSAIC_MAIL");
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		public static readonly Items BRICK_MAIL			= new Items(125,	"BRICK_MAIL");
		/// <summary>Held in battle :   When the holder is [paralyzed]{mechanic:paralysis}, it consumes this item to cure the paralysis.  Used on a party Pokémon :   Cures [paralysis]{mechanic:paralysis}.</summary>
		public static readonly Items CHERI_BERRY			= new Items(126,	"CHERI_BERRY");
		/// <summary>Held in battle :   When the holder is [asleep]{mechanic:sleep}, it consumes this item to wake up.  Used on a party Pokémon :   Cures [sleep]{mechanic:sleep}.</summary>
		public static readonly Items CHESTO_BERRY			= new Items(127,	"CHESTO_BERRY");
		/// <summary>Held in battle :   When the holder is [poisoned]{mechanic:poison}, it consumes this item to cure the poison.  Used on a party Pokémon :   Cures [poison]{mechanic:poison}.</summary>
		public static readonly Items PECHA_BERRY			= new Items(128,	"PECHA_BERRY");
		/// <summary>Held in battle :   When the holder is [burned]{mechanic:burn}, it consumes this item to cure the burn.  Used on a party Pokémon :   Cures a [burn]{mechanic:burn}.</summary>
		public static readonly Items RAWST_BERRY			= new Items(129,	"RAWST_BERRY");
		/// <summary>Held in battle :   When the holder is [frozen]{mechanic:freezing}, it consumes this item to thaw itself.  Used on a party Pokémon :   Cures [freezing]{mechanic:freezing}.</summary>
		public static readonly Items ASPEAR_BERRY			= new Items(130,	"ASPEAR_BERRY");
		/// <summary>Held in battle :   When the holder is out of [PP]{mechanic:pp} for one of its moves, it consumes this item to restore 10 of that move's PP.  Used on a party Pokémon :   Restores 10 [PP]{mechanic:pp} for a selected move.</summary>
		public static readonly Items LEPPA_BERRY			= new Items(131,	"LEPPA_BERRY");
		/// <summary>Held in battle :   When the holder has 1/2 its max [HP]{mechanic:hp} remaining or less, it consumes this item to restore 10 HP.  Used on a party Pokémon :   Restores 10 [HP]{mechanic:hp}.</summary>
		public static readonly Items ORAN_BERRY			= new Items(132,	"ORAN_BERRY");
		/// <summary>Held in battle :   When the holder is [confused]{mechanic:confusion}, it consumes this item to cure the confusion.  Used on a party Pokémon :   Cures [confusion]{mechanic:confusion}.</summary>
		public static readonly Items PERSIM_BERRY			= new Items(133,	"PERSIM_BERRY");
		/// <summary>Held in battle :   When the holder is afflicted with a [major status ailment]{mechanic:major-status-ailment}, it consumes this item to cure the ailment.  Used on a party Pokémon :   Cures any [major status ailment]{mechanic:major-status-ailment}.</summary>
		public static readonly Items LUM_BERRY			= new Items(134,	"LUM_BERRY");
		/// <summary>Held in battle :   When the holder has 1/2 its max [HP]{mechanic:hp} remaining or less, it consumes this item to restore 1/4 its max HP.  Used on a party Pokémon :   Restores 1/4 the Pokémon's max [HP]{mechanic:hp}.</summary>
		public static readonly Items SITRUS_BERRY			= new Items(135,	"SITRUS_BERRY");
		/// <summary>Held in battle :   When the holder has 1/2 its max [HP]{mechanic:hp} remaining or less, it consumes this item to restore 1/8 its max HP.  If the holder dislikes spicy flavors (i.e., has a nature that lowers [Attack]{mechanic:attack}), it will also become [confused]{mechanic:confusion}.</summary>
		public static readonly Items FIGY_BERRY			= new Items(136,	"FIGY_BERRY");
		/// <summary>Held in battle :   When the holder has 1/2 its max [HP]{mechanic:hp} remaining or less, it consumes this item to restore 1/8 its max HP.  If the holder dislikes dry flavors (i.e., has a nature that lowers [Special Attack]{mechanic:special-attack}), it will also become [confused]{mechanic:confusion}.</summary>
		public static readonly Items WIKI_BERRY			= new Items(137,	"WIKI_BERRY");
		/// <summary>Held in battle :   When the holder has 1/2 its max [HP]{mechanic:hp} remaining or less, it consumes this item to restore 1/8 its max HP.  If the holder dislikes sweet flavors (i.e., has a nature that lowers [Speed]{mechanic:speed}), it will also become [confused]{mechanic:confusion}.</summary>
		public static readonly Items MAGO_BERRY			= new Items(138,	"MAGO_BERRY");
		/// <summary>Held in battle :   When the holder has 1/2 its max [HP]{mechanic:hp} remaining or less, it consumes this item to restore 1/8 its max HP.  If the holder dislikes bitter flavors (i.e., has a nature that lowers [Special Defense]{mechanic:special-defense}), it will also become [confused]{mechanic:confusion}.</summary>
		public static readonly Items AGUAV_BERRY			= new Items(139,	"AGUAV_BERRY");
		/// <summary>Held in battle :   When the holder has 1/2 its max [HP]{mechanic:hp} remaining or less, it consumes this item to restore 1/8 its max HP.  If the holder dislikes sour flavors (i.e., has a nature that lowers [Defense]{mechanic:defense}), it will also become [confused]{mechanic:confusion}.</summary>
		public static readonly Items IAPAPA_BERRY			= new Items(140,	"IAPAPA_BERRY");
		/// <summary>No effect; only useful for planting and cooking.</summary>
		public static readonly Items RAZZ_BERRY			= new Items(141,	"RAZZ_BERRY");
		/// <summary>No effect; only useful for planting and cooking.</summary>
		public static readonly Items BLUK_BERRY			= new Items(142,	"BLUK_BERRY");
		/// <summary>No effect; only useful for planting and cooking.</summary>
		public static readonly Items NANAB_BERRY			= new Items(143,	"NANAB_BERRY");
		/// <summary>No effect; only useful for planting and cooking.</summary>
		public static readonly Items WEPEAR_BERRY			= new Items(144,	"WEPEAR_BERRY");
		/// <summary>No effect; only useful for planting and cooking.</summary>
		public static readonly Items PINAP_BERRY			= new Items(145,	"PINAP_BERRY");
		/// <summary>Used on a party Pokémon :   Increases [happiness]{mechanic:happiness} by 10/5/2.  Lowers [HP]{mechanic:hp} [effort]{mechanic:effort} by 10.</summary>
		public static readonly Items POMEG_BERRY			= new Items(146,	"POMEG_BERRY");
		/// <summary>Used on a party Pokémon :   Increases [happiness]{mechanic:happiness} by 10/5/2.  Lowers [Attack]{mechanic:attack} [effort]{mechanic:effort} by 10.</summary>
		public static readonly Items KELPSY_BERRY			= new Items(147,	"KELPSY_BERRY");
		/// <summary>Used on a party Pokémon :   Increases [happiness]{mechanic:happiness} by 10/5/2.  Lowers [Defense]{mechanic:defense} [effort]{mechanic:effort} by 10.</summary>
		public static readonly Items QUALOT_BERRY			= new Items(148,	"QUALOT_BERRY");
		/// <summary>Used on a party Pokémon :   Increases [happiness]{mechanic:happiness} by 10/5/2.  Lowers [Special Attack]{mechanic:special-attack} [effort]{mechanic:effort} by 10.</summary>
		public static readonly Items HONDEW_BERRY			= new Items(149,	"HONDEW_BERRY");
		/// <summary>Used on a party Pokémon :   Increases [happiness]{mechanic:happiness} by 10/5/2.  Lowers [Special Defense]{mechanic:special-defense} [effort]{mechanic:effort} by 10.</summary>
		public static readonly Items GREPA_BERRY			= new Items(150,	"GREPA_BERRY");
		/// <summary>Used on a party Pokémon :   Increases [happiness]{mechanic:happiness} by 10/5/2.  Lowers [Speed]{mechanic:speed} [effort]{mechanic:effort} by 10.</summary>
		public static readonly Items TAMATO_BERRY			= new Items(151,	"TAMATO_BERRY");
		/// <summary>No effect; only useful for planting and cooking.</summary>
		public static readonly Items CORNN_BERRY			= new Items(152,	"CORNN_BERRY");
		/// <summary>No effect; only useful for planting and cooking.</summary>
		public static readonly Items MAGOST_BERRY			= new Items(153,	"MAGOST_BERRY");
		/// <summary>No effect; only useful for planting and cooking.</summary>
		public static readonly Items RABUTA_BERRY			= new Items(154,	"RABUTA_BERRY");
		/// <summary>No effect; only useful for planting and cooking.</summary>
		public static readonly Items NOMEL_BERRY			= new Items(155,	"NOMEL_BERRY");
		/// <summary>No effect; only useful for planting and cooking.</summary>
		public static readonly Items SPELON_BERRY			= new Items(156,	"SPELON_BERRY");
		/// <summary>No effect; only useful for planting and cooking.</summary>
		public static readonly Items PAMTRE_BERRY			= new Items(157,	"PAMTRE_BERRY");
		/// <summary>No effect; only useful for planting and cooking.</summary>
		public static readonly Items WATMEL_BERRY			= new Items(158,	"WATMEL_BERRY");
		/// <summary>No effect; only useful for planting and cooking.</summary>
		public static readonly Items DURIN_BERRY			= new Items(159,	"DURIN_BERRY");
		/// <summary>No effect; only useful for planting and cooking.</summary>
		public static readonly Items BELUE_BERRY			= new Items(160,	"BELUE_BERRY");
		/// <summary>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:fire}-type damage, it consumes this item to halve the amount of damage taken.</summary>
		public static readonly Items OCCA_BERRY			= new Items(161,	"OCCA_BERRY");
		/// <summary>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:water}-type damage, it consumes this item to halve the amount of damage taken.</summary>
		public static readonly Items PASSHO_BERRY			= new Items(162,	"PASSHO_BERRY");
		/// <summary>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:electric}-type damage, it consumes this item to halve the amount of damage taken.</summary>
		public static readonly Items WACAN_BERRY			= new Items(163,	"WACAN_BERRY");
		/// <summary>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:grass}-type damage, it consumes this item to halve the amount of damage taken.</summary>
		public static readonly Items RINDO_BERRY			= new Items(164,	"RINDO_BERRY");
		/// <summary>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:ice}-type damage, it consumes this item to halve the amount of damage taken.</summary>
		public static readonly Items YACHE_BERRY			= new Items(165,	"YACHE_BERRY");
		/// <summary>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:fighting}-type damage, it consumes this item to halve the amount of damage taken.</summary>
		public static readonly Items CHOPLE_BERRY			= new Items(166,	"CHOPLE_BERRY");
		/// <summary>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:poison}-type damage, it consumes this item to halve the amount of damage taken.</summary>
		public static readonly Items KEBIA_BERRY			= new Items(167,	"KEBIA_BERRY");
		/// <summary>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:ground}-type damage, it consumes this item to halve the amount of damage taken.</summary>
		public static readonly Items SHUCA_BERRY			= new Items(168,	"SHUCA_BERRY");
		/// <summary>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:flying}-type damage, it consumes this item to halve the amount of damage taken.</summary>
		public static readonly Items COBA_BERRY			= new Items(169,	"COBA_BERRY");
		/// <summary>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:psychic}-type damage, it consumes this item to halve the amount of damage taken.</summary>
		public static readonly Items PAYAPA_BERRY			= new Items(170,	"PAYAPA_BERRY");
		/// <summary>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:bug}-type damage, it consumes this item to halve the amount of damage taken.</summary>
		public static readonly Items TANGA_BERRY			= new Items(171,	"TANGA_BERRY");
		/// <summary>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:rock}-type damage, it consumes this item to halve the amount of damage taken.</summary>
		public static readonly Items CHARTI_BERRY			= new Items(172,	"CHARTI_BERRY");
		/// <summary>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:ghost}-type damage, it consumes this item to halve the amount of damage taken.</summary>
		public static readonly Items KASIB_BERRY			= new Items(173,	"KASIB_BERRY");
		/// <summary>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:dragon}-type damage, it consumes this item to halve the amount of damage taken.</summary>
		public static readonly Items HABAN_BERRY			= new Items(174,	"HABAN_BERRY");
		/// <summary>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:dark}-type damage, it consumes this item to halve the amount of damage taken.</summary>
		public static readonly Items COLBUR_BERRY			= new Items(175,	"COLBUR_BERRY");
		/// <summary>Held in battle :   When the holder would take [super-effective]{mechanic:super-effective} []{type:steel}-type damage, it consumes this item to halve the amount of damage taken.</summary>
		public static readonly Items BABIRI_BERRY			= new Items(176,	"BABIRI_BERRY");
		/// <summary>Held in battle :   When the holder would take []{type:normal}-type damage, it consumes this item to halve the amount of damage taken.</summary>
		public static readonly Items CHILAN_BERRY			= new Items(177,	"CHILAN_BERRY");
		/// <summary>Held in battle :   When the holder has 1/4 its max [HP]{mechanic:hp} remaining or less, it consumes this item to [raise]{mechanic:raise} its [Attack]{mechanic:attack} by one stage.</summary>
		public static readonly Items LIECHI_BERRY			= new Items(178,	"LIECHI_BERRY");
		/// <summary>Held in battle :   When the holder has 1/4 its max [HP]{mechanic:hp} remaining or less, it consumes this item to [raise]{mechanic:raise} its [Defense]{mechanic:defense} by one stage.</summary>
		public static readonly Items GANLON_BERRY			= new Items(179,	"GANLON_BERRY");
		/// <summary>Held in battle :   When the holder has 1/4 its max [HP]{mechanic:hp} remaining or less, it consumes this item to [raise]{mechanic:raise} its [Speed]{mechanic:speed} by one stage.</summary>
		public static readonly Items SALAC_BERRY			= new Items(180,	"SALAC_BERRY");
		/// <summary>Held in battle :   When the holder has 1/4 its max [HP]{mechanic:hp} remaining or less, it consumes this item to [raise]{mechanic:raise} its [Special Attack]{mechanic:special-attack} by one stage.</summary>
		public static readonly Items PETAYA_BERRY			= new Items(181,	"PETAYA_BERRY");
		/// <summary>Held in battle :   When the holder has 1/4 its max [HP]{mechanic:hp} remaining or less, it consumes this item to [raise]{mechanic:raise} its [Special Defense]{mechanic:special-defense} by one stage.</summary>
		public static readonly Items APICOT_BERRY			= new Items(182,	"APICOT_BERRY");
		/// <summary>Held in battle :   When the holder has 1/4 its max [HP]{mechanic:hp} remaining or less, it consumes this item to [raise]{mechanic:raise} its [critical hit chance]{mechanic:critical-hit-chance} by one stage.</summary>
		public static readonly Items LANSAT_BERRY			= new Items(183,	"LANSAT_BERRY");
		/// <summary>Held in battle :   When the holder has 1/4 its max [HP]{mechanic:hp} remaining or less, it consumes this item to [raise]{mechanic:raise} a random stat by two stages.</summary>
		public static readonly Items STARF_BERRY			= new Items(184,	"STARF_BERRY");
		/// <summary>Held in battle :   When the holder takes [super-effective]{mechanic:super-effective} damage, it consumes this item to restore 1/4 its max [HP]{mechanic:hp}.</summary>
		public static readonly Items ENIGMA_BERRY			= new Items(185,	"ENIGMA_BERRY");
		/// <summary>Held in battle :   When the holder has 1/4 its max [HP]{mechanic:hp} remaining or less, it consumes this item, and its next used move has 1.2× its normal accuracy.</summary>
		public static readonly Items MICLE_BERRY			= new Items(186,	"MICLE_BERRY");
		/// <summary>Held in battle :   When the holder has 1/4 its max [HP]{mechanic:hp} remaining or less, it consumes this item.  On the following turn, the holder will act first among moves with the same [priority]{mechanic:priority}, regardless of [Speed]{mechanic:speed}.</summary>
		public static readonly Items CUSTAP_BERRY			= new Items(187,	"CUSTAP_BERRY");
		/// <summary>Held in battle :   When the holder takes [physical]{mechanic:physical} damage, it consumes this item to damage the attacking Pokémon for 1/8 its max [HP]{mechanic:hp}.</summary>
		public static readonly Items JABOCA_BERRY			= new Items(188,	"JABOCA_BERRY");
		/// <summary>Held in battle :   When the holder takes [special]{mechanic:special} damage, it consumes this item to damage the attacking Pokémon for 1/8 its max [HP]{mechanic:hp}.</summary>
		public static readonly Items ROWAP_BERRY			= new Items(189,	"ROWAP_BERRY");
		/// <summary>Held in battle :   Moves targeting the holder have 0.9× [chance to hit]{mechanic:chance-to-hit}.</summary>
		public static readonly Items BRIGHT_POWDER			= new Items(190,	"BRIGHT_POWDER");
		/// <summary>Held in battle :   At the end of each turn, if any of the holder's stats have a negative [stat modifier]{mechanic:stat-modifier}, the holder consumes this item to remove the modifiers from those stats.</summary>
		public static readonly Items WHITE_HERB			= new Items(191,	"WHITE_HERB");
		/// <summary>Held :   When the holder would gain [effort]{mechanic:effort} due to battle, it gains double that effort instead.  Held in battle :   Holder has half its [Speed]{mechanic:speed}.</summary>
		public static readonly Items MACHO_BRACE			= new Items(192,	"MACHO_BRACE");
		/// <summary>Held :   [Experience]{mechanic:experience} is split across two groups: Pokémon who participated in battle, and Pokémon holding this item.  Each Pokémon earns experience as though it had battled alone, divided by the number of Pokémon in its group, then divided by the number of groups. Pokémon holding this item who also participated in battle effectively earn experience twice.      [Fainted]{mechanic:fainted} Pokémon never earn experience, and empty groups are ignored; thus, if a single Pokémon is holding this item and the only Pokémon who battled faints from []{move:explosion}, the holder will gain full experience.</summary>
		public static readonly Items EXP_SHARE			= new Items(193,	"EXP_SHARE");
		/// <summary>Held in battle :   Whenever the holder attempts to use a move, it has a 3/16 chance to act first among moves with the same [priority]{mechanic:priority}.  If multiple Pokémon have this effect at the same time, [Speed]{mechanic:speed} is the tie-breaker as normal, but the effect of []{move:trick-room} is ignored.</summary>
		public static readonly Items QUICK_CLAW			= new Items(194,	"QUICK_CLAW");
		/// <summary>Held :   When the holder would earn [happiness]{mechanic:happiness} for any reason, it earns twice that amount instead.</summary>
		public static readonly Items SOOTHE_BELL			= new Items(195,	"SOOTHE_BELL");
		/// <summary>Held in battle :   When the holder is [attracted]{move:attract}, it consumes this item to cure the attraction.</summary>
		public static readonly Items MENTAL_HERB			= new Items(196,	"MENTAL_HERB");
		/// <summary>Held in battle :   Holder has 1.5× its [Attack]{mechanic:attack}.  When the holder attempts to use a move, all its other moves are disabled until it leaves battle or loses this item.      The restriction ends even if this item is swapped for another Choice item via []{move:trick} or []{move:switcheroo}.</summary>
		public static readonly Items CHOICE_BAND			= new Items(197,	"CHOICE_BAND");
		/// <summary>Held in battle :   Holder's damaging moves have a 10% chance to make their target [flinch]{mechanic:flinch}.  This chance applies independently to each hit of a multi-hit move.      This item's chance is rolled independently of any other move effects; e.g., a move with a 30% chance to flinch normally will have a 37% total chance to flinch when used with this item, because 3% of the time, both effects activate.  Held by []{pokemon:poliwhirl} or []{pokemon:slowbro} :   Holder evolves into []{pokemon:politoed} or []{pokemon:slowking}, respectively, when traded.</summary>
		public static readonly Items KINGS_ROCK			= new Items(198,	"KINGS_ROCK");
		/// <summary>Held in battle :   Holder's []{type:bug}-type moves have 1.2× their power. </summary>
		public static readonly Items SILVER_POWDER			= new Items(199,	"SILVER_POWDER");
		/// <summary>Held :   If the holder participated in a trainer battle, the trainer earns twice the usual prize money.  This effect applies even if the holder [fainted]{mechanic:fainted}.      This effect does not stack with any other similar effect.</summary>
		public static readonly Items AMULET_COIN			= new Items(200,	"AMULET_COIN");
		/// <summary>Held by lead Pokémon: Prevents wild battles with Pokémon that are lower level than the holder.</summary>
		public static readonly Items CLEANSE_TAG			= new Items(201,	"CLEANSE_TAG");
		/// <summary>Held by Latias or Latios: Increases the holder's Special Attack and Special Defense by 50%.</summary>
		public static readonly Items SOUL_DEW			= new Items(202,	"SOUL_DEW");
		/// <summary>Held by Clamperl: Doubles the holder's Special Attack.  Evolves the holder into Huntail when traded.</summary>
		public static readonly Items DEEP_SEA_TOOTH			= new Items(203,	"DEEP_SEA_TOOTH");
		/// <summary>Held by Clamperl: Doubles the holder's Special Defense.  Evolves the holder into Gorebyss when traded.</summary>
		public static readonly Items DEEP_SEA_SCALE			= new Items(204,	"DEEP_SEA_SCALE");
		/// <summary>Held: In wild battles, attempts to run away on the holder's turn will always succeed.</summary>
		public static readonly Items SMOKE_BALL			= new Items(205,	"SMOKE_BALL");
		/// <summary>Held: Prevents the holder from evolving naturally.  Evolution initiated by the trainer (Stones, etc) will still work.</summary>
		public static readonly Items EVERSTONE			= new Items(206,	"EVERSTONE");
		/// <summary>Held: If the holder is attacked for regular damage that would faint it, this item has a 10% chance to prevent the holder's HP from lowering below 1.</summary>
		public static readonly Items FOCUS_BAND			= new Items(207,	"FOCUS_BAND");
		/// <summary>Held: Increases any Exp the holder gains by 50%.</summary>
		public static readonly Items LUCKY_EGG			= new Items(208,	"LUCKY_EGG");
		/// <summary>Held: Raises the holder's critical hit counter by 1.</summary>
		public static readonly Items SCOPE_LENS			= new Items(209,	"SCOPE_LENS");
		/// <summary>Held: Increases the power of the holder's Steel moves by 20%. Held by Onix or Scyther: Evolves the holder into Steelix or Scizor when traded, respectively.</summary>
		public static readonly Items METAL_COAT			= new Items(210,	"METAL_COAT");
		/// <summary>Held: Heals the holder by 1/16 its max HP at the end of each turn.</summary>
		public static readonly Items LEFTOVERS			= new Items(211,	"LEFTOVERS");
		/// <summary>Held by Seadra: Evolves the holder into Kingdra when traded.</summary>
		public static readonly Items DRAGON_SCALE			= new Items(212,	"DRAGON_SCALE");
		/// <summary>Held by Pikachu: Doubles the holder's initial Attack and Special Attack.</summary>
		public static readonly Items LIGHT_BALL			= new Items(213,	"LIGHT_BALL");
		/// <summary>Held: Increases the power of the holder's Ground moves by 20%.</summary>
		public static readonly Items SOFT_SAND			= new Items(214,	"SOFT_SAND");
		/// <summary>Held: Increases the power of the holder's Rock moves by 20%.</summary>
		public static readonly Items HARD_STONE			= new Items(215,	"HARD_STONE");
		/// <summary>Held: Increases the power of the holder's Grass moves by 20%.</summary>
		public static readonly Items MIRACLE_SEED			= new Items(216,	"MIRACLE_SEED");
		/// <summary>Held: Increases the power of the holder's Dark moves by 20%.</summary>
		public static readonly Items BLACK_GLASSES			= new Items(217,	"BLACK_GLASSES");
		/// <summary>Held: Increases the power of the holder's Fighting moves by 20%.</summary>
		public static readonly Items BLACK_BELT			= new Items(218,	"BLACK_BELT");
		/// <summary>Held: Increases the power of the holder's Electric moves by 20%.</summary>
		public static readonly Items MAGNET			= new Items(219,	"MAGNET");
		/// <summary>Held: Increases the power of the holder's Water moves by 20%.</summary>
		public static readonly Items MYSTIC_WATER			= new Items(220,	"MYSTIC_WATER");
		/// <summary>Held: Increases the power of the holder's Flying moves by 20%.</summary>
		public static readonly Items SHARP_BEAK			= new Items(221,	"SHARP_BEAK");
		/// <summary>Held: Increases the power of the holder's Poison moves by 20%.</summary>
		public static readonly Items POISON_BARB			= new Items(222,	"POISON_BARB");
		/// <summary>Held: Increases the power of the holder's Ice moves by 20%.</summary>
		public static readonly Items NEVER_MELT_ICE			= new Items(223,	"NEVER_MELT_ICE");
		/// <summary>Held: Increases the power of the holder's Ghost moves by 20%.</summary>
		public static readonly Items SPELL_TAG			= new Items(224,	"SPELL_TAG");
		/// <summary>Held: Increases the power of the holder's Psychic moves by 20%.</summary>
		public static readonly Items TWISTED_SPOON			= new Items(225,	"TWISTED_SPOON");
		/// <summary>Held: Increases the power of the holder's Fire moves by 20%.</summary>
		public static readonly Items CHARCOAL			= new Items(226,	"CHARCOAL");
		/// <summary>Held: Increases the power of the holder's Dragon moves by 20%.</summary>
		public static readonly Items DRAGON_FANG			= new Items(227,	"DRAGON_FANG");
		/// <summary>Held: Increases the power of the holder's Normal moves by 20%.</summary>
		public static readonly Items SILK_SCARF			= new Items(228,	"SILK_SCARF");
		/// <summary>Held by Porygon: Evolves the holder into Porygon2 when traded.</summary>
		public static readonly Items UP_GRADE			= new Items(229,	"UP_GRADE");
		/// <summary>Held: Heals the holder by 1/8 of any damage it inflicts.</summary>
		public static readonly Items SHELL_BELL			= new Items(230,	"SHELL_BELL");
		/// <summary>Held: Increases the power of the holder's Water moves by 20%.</summary>
		public static readonly Items SEA_INCENSE			= new Items(231,	"SEA_INCENSE");
		/// <summary>Held: Increases the holder's Evasion by 5%.</summary>
		public static readonly Items LAX_INCENSE			= new Items(232,	"LAX_INCENSE");
		/// <summary>Held by Chansey: Raises the holder's critical hit counter by 2.</summary>
		public static readonly Items LUCKY_PUNCH			= new Items(233,	"LUCKY_PUNCH");
		/// <summary>Held by Ditto: Increases the holder's initial Defense and Special Defense by 50%.</summary>
		public static readonly Items METAL_POWDER			= new Items(234,	"METAL_POWDER");
		/// <summary>Held by Cubone or Marowak: Doubles the holder's Attack.</summary>
		public static readonly Items THICK_CLUB			= new Items(235,	"THICK_CLUB");
		/// <summary>Held by Farfetch'd: Raises the holder's critical hit counter by 2.</summary>
		public static readonly Items STICK			= new Items(236,	"STICK");
		/// <summary>Held: Increases the holder's Coolness during a Super Contest's Visual Competition.</summary>
		public static readonly Items RED_SCARF			= new Items(237,	"RED_SCARF");
		/// <summary>Held: Increases the holder's Beauty during a Super Contest's Visual Competition.</summary>
		public static readonly Items BLUE_SCARF			= new Items(238,	"BLUE_SCARF");
		/// <summary>Held: Increases the holder's Cuteness during a Super Contest's Visual Competition.</summary>
		public static readonly Items PINK_SCARF			= new Items(239,	"PINK_SCARF");
		/// <summary>Held: Increases the holder's Smartness during a Super Contest's Visual Competition.</summary>
		public static readonly Items GREEN_SCARF			= new Items(240,	"GREEN_SCARF");
		/// <summary>Held: Increases the holder's Toughness during a Super Contest's Visual Competition.</summary>
		public static readonly Items YELLOW_SCARF			= new Items(241,	"YELLOW_SCARF");
		/// <summary>Held: Increases the accuracy of any move the holder uses by 10% (multiplied; i.e. 70% accuracy is increased to 77%).</summary>
		public static readonly Items WIDE_LENS			= new Items(242,	"WIDE_LENS");
		/// <summary>Held: Increases the power of the holder's physical moves by 10%.</summary>
		public static readonly Items MUSCLE_BAND			= new Items(243,	"MUSCLE_BAND");
		/// <summary>Held: Increases the power of the holder's special moves by 10%.</summary>
		public static readonly Items WISE_GLASSES			= new Items(244,	"WISE_GLASSES");
		/// <summary>Held: When the holder hits with a super-effective move, its power is raised by 20%.</summary>
		public static readonly Items EXPERT_BELT			= new Items(245,	"EXPERT_BELT");
		/// <summary>Held: The holder's Reflect and Light Screen will create effects lasting for eight turns rather than five.  As this item affects the move rather than the barrier itself, the effect is not lost if the holder leaves battle or drops this item.</summary>
		public static readonly Items LIGHT_CLAY			= new Items(246,	"LIGHT_CLAY");
		/// <summary>Held: Damage from the holder's moves is increased by 30%.  On each turn the holder uses a damage-inflicting move, it takes 10% its max HP in damage.</summary>
		public static readonly Items LIFE_ORB			= new Items(247,	"LIFE_ORB");
		/// <summary>Held: Whenever the holder uses a move that requires a turn to charge first (Bounce, Dig, Dive, Fly, Razor Wind, Skull Bash, Sky Attack, or Solarbeam), this item is consumed and the charge is skipped.  Skull Bash still provides a Defense boost.</summary>
		public static readonly Items POWER_HERB			= new Items(248,	"POWER_HERB");
		/// <summary>Held: Badly poisons the holder at the end of each turn.</summary>
		public static readonly Items TOXIC_ORB			= new Items(249,	"TOXIC_ORB");
		/// <summary>Held: Burns the holder at the end of each turn.</summary>
		public static readonly Items FLAME_ORB			= new Items(250,	"FLAME_ORB");
		/// <summary>Held by Ditto: Doubles the holder's initial Speed.</summary>
		public static readonly Items QUICK_POWDER			= new Items(251,	"QUICK_POWDER");
		/// <summary>Held: If the holder has full HP and is attacked for regular damage that would faint it, this item is consumed and prevents the holder's HP from lowering below 1.  This effect works against multi-hit attacks, but does not work against the effects of Doom Desire or Future Sight.</summary>
		public static readonly Items FOCUS_SASH			= new Items(252,	"FOCUS_SASH");
		/// <summary>Held: Raises the holder's Accuracy by 20% when it goes last. Ingame description is incorrect.</summary>
		public static readonly Items ZOOM_LENS			= new Items(253,	"ZOOM_LENS");
		/// <summary>Held: Each time the holder uses the same move consecutively, its power is increased by another 10% of its original, to a maximum of 100%.</summary>
		public static readonly Items METRONOME			= new Items(254,	"METRONOME");
		/// <summary>Held: Decreases the holder's Speed by 50%.  If the holder is Flying or has Levitate, it takes regular damage from Ground attacks and is suspectible to Spikes and Toxic Spikes.</summary>
		public static readonly Items IRON_BALL			= new Items(255,	"IRON_BALL");
		/// <summary>Held: The holder will go last within its move's priority bracket, regardless of Speed.  If multiple Pokémon within the same priority bracket are subject to this effect, the slower Pokémon will go first.  The holder will move after Pokémon with Stall.  If the holder has Stall, Stall is ignored.  This item ignores Trick Room.</summary>
		public static readonly Items LAGGING_TAIL			= new Items(256,	"LAGGING_TAIL");
		/// <summary>Held: When the holder becomes Attracted, the Pokémon it is Attracted to becomes Attracted back.</summary>
		public static readonly Items DESTINY_KNOT			= new Items(257,	"DESTINY_KNOT");
		/// <summary>Held: If the holder is Poison-type, restores 1/16 max HP at the end of each turn.  Otherwise, damages the holder by 1/16 its max HP at the end of each turn.</summary>
		public static readonly Items BLACK_SLUDGE			= new Items(258,	"BLACK_SLUDGE");
		/// <summary>Held: The holder's Hail will create a hailstorm lasting for eight turns rather than five.  As this item affects the move rather than the weather itself, the effect is not lost if the holder leaves battle or drops this item.</summary>
		public static readonly Items ICY_ROCK			= new Items(259,	"ICY_ROCK");
		/// <summary>Held: The holder's Sandstorm will create a sandstorm lasting for eight turns rather than five.  As this item affects the move rather than the weather itself, the effect is not lost if the holder leaves battle or drops this item.</summary>
		public static readonly Items SMOOTH_ROCK			= new Items(260,	"SMOOTH_ROCK");
		/// <summary>Held: The holder's Sunny Day will create sunshine lasting for eight turns rather than five.  As this item affects the move rather than the weather itself, the effect is not lost if the holder leaves battle or drops this item.</summary>
		public static readonly Items HEAT_ROCK			= new Items(261,	"HEAT_ROCK");
		/// <summary>Held: The holder's Rain Dance will create rain lasting for eight turns rather than five.  As this item affects the move rather than the weather itself, the effect is not lost if the holder leaves battle or drops this item.</summary>
		public static readonly Items DAMP_ROCK			= new Items(262,	"DAMP_ROCK");
		/// <summary>Held: Increases the duration of the holder's multiturn (2-5 turn) moves by three turns.</summary>
		public static readonly Items GRIP_CLAW			= new Items(263,	"GRIP_CLAW");
		/// <summary>Held: Increases the holder's Speed by 50%, but restricts it to the first move it uses until it leaves battle or loses this item.  If this item is swapped for another Choice item via Trick or Switcheroo, the holder's restriction is still lifted, but it will again be restricted to the next move it uses. (Quirk: If the holder is switched in by U-Turn and it also knows U-Turn, U-Turn becomes its restricted move.)</summary>
		public static readonly Items CHOICE_SCARF			= new Items(264,	"CHOICE_SCARF");
		/// <summary>Held: Damaged the holder for 1/8 its max HP.  When the holder is struck by a contact move, damages the attacker for 1/8 its max HP; if the attacker is not holding an item, it will take this item.</summary>
		public static readonly Items STICKY_BARB			= new Items(265,	"STICKY_BARB");
		/// <summary>Held: Decreases the holder's Speed by 50%.  Whenever the holder gains Attack effort from battle, increases that effort by 4; this applies before the PokéRUS doubling effect.</summary>
		public static readonly Items POWER_BRACER			= new Items(266,	"POWER_BRACER");
		/// <summary>Held: Decreases the holder's Speed by 50%.  Whenever the holder gains Defense effort from battle, increases that effort by 4; this applies before the PokéRUS doubling effect.</summary>
		public static readonly Items POWER_BELT			= new Items(267,	"POWER_BELT");
		/// <summary>Held: Decreases the holder's Speed by 50%.  Whenever the holder gains Special Attack effort from battle, increases that effort by 4; this applies before the PokéRUS doubling effect.</summary>
		public static readonly Items POWER_LENS			= new Items(268,	"POWER_LENS");
		/// <summary>Held: Decreases the holder's Speed by 50%.  Whenever the holder gains Special Defense effort from battle, increases that effort by 4; this applies before the PokéRUS doubling effect.</summary>
		public static readonly Items POWER_BAND			= new Items(269,	"POWER_BAND");
		/// <summary>Held: Decreases the holder's Speed by 50%.  Whenever the holder gains Speed effort from battle, increases that effort by 4; this applies before the PokéRUS doubling effect.</summary>
		public static readonly Items POWER_ANKLET			= new Items(270,	"POWER_ANKLET");
		/// <summary>Held: Decreases the holder's Speed by 50%.  Whenever the holder gains HP effort from battle, increases that effort by 4; this applies before the PokéRUS doubling effect.</summary>
		public static readonly Items POWER_WEIGHT			= new Items(271,	"POWER_WEIGHT");
		/// <summary>Held: The holder is unaffected by any moves or abilities that would prevent it from actively leaving battle.</summary>
		public static readonly Items SHED_SHELL			= new Items(272,	"SHED_SHELL");
		/// <summary>Held: HP restored from Absorb, Aqua Ring, Drain Punch, Dream Eater, Giga Drain, Ingrain, Leech Life, Leech Seed, and Mega Drain is increased by 30%.  Damage inflicted is not affected.</summary>
		public static readonly Items BIG_ROOT			= new Items(273,	"BIG_ROOT");
		/// <summary>Held: Increases the holder's Special Attack by 50%, but restricts it to the first move it uses until it leaves battle or loses this item.  If this item is swapped for another Choice item via Trick or Switcheroo, the holder's restriction is still lifted, but it will again be restricted to the next move it uses. (Quirk: If the holder is switched in by U-Turn and it also knows U-Turn, U-Turn becomes its restricted move.)</summary>
		public static readonly Items CHOICE_SPECS			= new Items(274,	"CHOICE_SPECS");
		/// <summary>Held: Increases the power of the holder's Fire moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Fire.</summary>
		public static readonly Items FLAME_PLATE			= new Items(275,	"FLAME_PLATE");
		/// <summary>Held: Increases the power of the holder's Water moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Water.</summary>
		public static readonly Items SPLASH_PLATE			= new Items(276,	"SPLASH_PLATE");
		/// <summary>Held: Increases the power of the holder's Electric moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Electric.</summary>
		public static readonly Items ZAP_PLATE			= new Items(277,	"ZAP_PLATE");
		/// <summary>Held: Increases the power of the holder's Grass moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Grass.</summary>
		public static readonly Items MEADOW_PLATE			= new Items(278,	"MEADOW_PLATE");
		/// <summary>Held: Increases the power of the holder's Ice moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Ice.</summary>
		public static readonly Items ICICLE_PLATE			= new Items(279,	"ICICLE_PLATE");
		/// <summary>Held: Increases the power of the holder's Fighting moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Fighting.</summary>
		public static readonly Items FIST_PLATE			= new Items(280,	"FIST_PLATE");
		/// <summary>Held: Increases the power of the holder's Poison moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Poison.</summary>
		public static readonly Items TOXIC_PLATE			= new Items(281,	"TOXIC_PLATE");
		/// <summary>Held: Increases the power of the holder's Ground moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Ground.</summary>
		public static readonly Items EARTH_PLATE			= new Items(282,	"EARTH_PLATE");
		/// <summary>Held: Increases the power of the holder's Flying moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Flying.</summary>
		public static readonly Items SKY_PLATE			= new Items(283,	"SKY_PLATE");
		/// <summary>Held: Increases the power of the holder's Psychic moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Psychic.</summary>
		public static readonly Items MIND_PLATE			= new Items(284,	"MIND_PLATE");
		/// <summary>Held: Increases the power of the holder's Bug moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Bug.</summary>
		public static readonly Items INSECT_PLATE			= new Items(285,	"INSECT_PLATE");
		/// <summary>Held: Increases the power of the holder's Rock moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Rock.</summary>
		public static readonly Items STONE_PLATE			= new Items(286,	"STONE_PLATE");
		/// <summary>Held: Increases the power of the holder's Ghost moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Ghost.</summary>
		public static readonly Items SPOOKY_PLATE			= new Items(287,	"SPOOKY_PLATE");
		/// <summary>Held: Increases the power of the holder's Dragon moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Dragon.</summary>
		public static readonly Items DRACO_PLATE			= new Items(288,	"DRACO_PLATE");
		/// <summary>Held: Increases the power of the holder's Dark moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Dark.</summary>
		public static readonly Items DREAD_PLATE			= new Items(289,	"DREAD_PLATE");
		/// <summary>Held: Increases the power of the holder's Steel moves by 20%. Held by a Multitype Pokémon: Holder's type becomes Steel.</summary>
		public static readonly Items IRON_PLATE			= new Items(290,	"IRON_PLATE");
		/// <summary>Held: Increases the power of the holder's Psychic moves by 20%.</summary>
		public static readonly Items ODD_INCENSE			= new Items(291,	"ODD_INCENSE");
		/// <summary>Held: Increases the power of the holder's Rock moves by 20%.</summary>
		public static readonly Items ROCK_INCENSE			= new Items(292,	"ROCK_INCENSE");
		/// <summary>Held: The holder will go last within its move's priority bracket, regardless of Speed.  If multiple Pokémon within the same priority bracket are subject to this effect, the slower Pokémon will go first.  The holder will move after Pokémon with Stall.  If the holder has Stall, Stall is ignored.  This item ignores Trick Room.</summary>
		public static readonly Items FULL_INCENSE			= new Items(293,	"FULL_INCENSE");
		/// <summary>Held: Increases the power of the holder's Water moves by 20%.</summary>
		public static readonly Items WAVE_INCENSE			= new Items(294,	"WAVE_INCENSE");
		/// <summary>Held: Increases the power of the holder's Grass moves by 20%.</summary>
		public static readonly Items ROSE_INCENSE			= new Items(295,	"ROSE_INCENSE");
		/// <summary>Held: Doubles the money the trainer receives after an in-game trainer battle.  This effect cannot apply more than once to the same battle.</summary>
		public static readonly Items LUCK_INCENSE			= new Items(296,	"LUCK_INCENSE");
		/// <summary>Held by lead Pokémon: Prevents wild battles with Pokémon that are lower level than the holder.</summary>
		public static readonly Items PURE_INCENSE			= new Items(297,	"PURE_INCENSE");
		/// <summary>Held by Rhydon: Evolves the holder into Rhyperior when traded.</summary>
		public static readonly Items PROTECTOR			= new Items(298,	"PROTECTOR");
		/// <summary>Held by Electabuzz: Evolves the holder into Electivire when traded.</summary>
		public static readonly Items ELECTIRIZER			= new Items(299,	"ELECTIRIZER");
		/// <summary>Held by Magmar: Evolves the holder into Magmortar when traded.</summary>
		public static readonly Items MAGMARIZER			= new Items(300,	"MAGMARIZER");
		/// <summary>Held by Porygon2: Evolves the holder into Porygon-Z when traded.</summary>
		public static readonly Items DUBIOUS_DISC			= new Items(301,	"DUBIOUS_DISC");
		/// <summary>Held by Dusclops: Evolves the holder into Dusknoir when traded.</summary>
		public static readonly Items REAPER_CLOTH			= new Items(302,	"REAPER_CLOTH");
		/// <summary>Held: Raises the holder's critical hit counter by 1. Held by Sneasel: Evolves the holder into Weavile when it levels up during the night.</summary>
		public static readonly Items RAZOR_CLAW			= new Items(303,	"RAZOR_CLAW");
		/// <summary>Held: When the holder attacks with most damaging moves, provides an extra 11.7% (30/256) chance for the target to flinch. Held by Gligar: Evolves the holder into Gliscor when it levels up.</summary>
		public static readonly Items RAZOR_FANG			= new Items(304,	"RAZOR_FANG");
		/// <summary>Use for any and all TMs, to remove limit on version generation...</summary>
		public static readonly Items TM_ALL			= new Items(305,	"TM_ALL");
		/*// <summary>Teaches Focus Punch to a compatible Pokémon.</summary>
		public static readonly Items TM01			= new Items(305,	"TM01");
		/// <summary>Teaches Dragon Claw to a compatible Pokémon.</summary>
		public static readonly Items TM02			= new Items(306,	"TM02");
		/// <summary>Teaches Water Pulse to a compatible Pokémon.</summary>
		public static readonly Items TM03			= new Items(307,	"TM03");
		/// <summary>Teaches Calm Mind to a compatible Pokémon.</summary>
		public static readonly Items TM04			= new Items(308,	"TM04");
		/// <summary>Teaches Roar to a compatible Pokémon.</summary>
		public static readonly Items TM05			= new Items(309,	"TM05");
		/// <summary>Teaches Toxic to a compatible Pokémon.</summary>
		public static readonly Items TM06			= new Items(310,	"TM06");
		/// <summary>Teaches Hail to a compatible Pokémon.</summary>
		public static readonly Items TM07			= new Items(311,	"TM07");
		/// <summary>Teaches Bulk Up to a compatible Pokémon.</summary>
		public static readonly Items TM08			= new Items(312,	"TM08");
		/// <summary>Teaches Bullet Seed to a compatible Pokémon.</summary>
		public static readonly Items TM09			= new Items(313,	"TM09");
		/// <summary>Teaches Hidden Power to a compatible Pokémon.</summary>
		public static readonly Items TM10			= new Items(314,	"TM10");
		/// <summary>Teaches Sunny Day to a compatible Pokémon.</summary>
		public static readonly Items TM11			= new Items(315,	"TM11");
		/// <summary>Teaches Taunt to a compatible Pokémon.</summary>
		public static readonly Items TM12			= new Items(316,	"TM12");
		/// <summary>Teaches Ice Beam to a compatible Pokémon.</summary>
		public static readonly Items TM13			= new Items(317,	"TM13");
		/// <summary>Teaches Blizzard to a compatible Pokémon.</summary>
		public static readonly Items TM14			= new Items(318,	"TM14");
		/// <summary>Teaches Hyper Beam to a compatible Pokémon.</summary>
		public static readonly Items TM15			= new Items(319,	"TM15");
		/// <summary>Teaches Light Screen to a compatible Pokémon.</summary>
		public static readonly Items TM16			= new Items(320,	"TM16");
		/// <summary>Teaches Protect to a compatible Pokémon.</summary>
		public static readonly Items TM17			= new Items(321,	"TM17");
		/// <summary>Teaches Rain Dance to a compatible Pokémon.</summary>
		public static readonly Items TM18			= new Items(322,	"TM18");
		/// <summary>Teaches Giga Drain to a compatible Pokémon.</summary>
		public static readonly Items TM19			= new Items(323,	"TM19");
		/// <summary>Teaches Safeguard to a compatible Pokémon.</summary>
		public static readonly Items TM20			= new Items(324,	"TM20");
		/// <summary>Teaches Frustration to a compatible Pokémon.</summary>
		public static readonly Items TM21			= new Items(325,	"TM21");
		/// <summary>Teaches SolarBeam to a compatible Pokémon.</summary>
		public static readonly Items TM22			= new Items(326,	"TM22");
		/// <summary>Teaches Iron Tail to a compatible Pokémon.</summary>
		public static readonly Items TM23			= new Items(327,	"TM23");
		/// <summary>Teaches Thunderbolt to a compatible Pokémon.</summary>
		public static readonly Items TM24			= new Items(328,	"TM24");
		/// <summary>Teaches Thunder to a compatible Pokémon.</summary>
		public static readonly Items TM25			= new Items(329,	"TM25");
		/// <summary>Teaches Earthquake to a compatible Pokémon.</summary>
		public static readonly Items TM26			= new Items(330,	"TM26");
		/// <summary>Teaches Return to a compatible Pokémon.</summary>
		public static readonly Items TM27			= new Items(331,	"TM27");
		/// <summary>Teaches Dig to a compatible Pokémon.</summary>
		public static readonly Items TM28			= new Items(332,	"TM28");
		/// <summary>Teaches Psychic to a compatible Pokémon.</summary>
		public static readonly Items TM29			= new Items(333,	"TM29");
		/// <summary>Teaches Shadow Ball to a compatible Pokémon.</summary>
		public static readonly Items TM30			= new Items(334,	"TM30");
		/// <summary>Teaches Brick Break to a compatible Pokémon.</summary>
		public static readonly Items TM31			= new Items(335,	"TM31");
		/// <summary>Teaches Double Team to a compatible Pokémon.</summary>
		public static readonly Items TM32			= new Items(336,	"TM32");
		/// <summary>Teaches Reflect to a compatible Pokémon.</summary>
		public static readonly Items TM33			= new Items(337,	"TM33");
		/// <summary>Teaches Shock Wave to a compatible Pokémon.</summary>
		public static readonly Items TM34			= new Items(338,	"TM34");
		/// <summary>Teaches Flamethrower to a compatible Pokémon.</summary>
		public static readonly Items TM35			= new Items(339,	"TM35");
		/// <summary>Teaches Sludge Bomb to a compatible Pokémon.</summary>
		public static readonly Items TM36			= new Items(340,	"TM36");
		/// <summary>Teaches Sandstorm to a compatible Pokémon.</summary>
		public static readonly Items TM37			= new Items(341,	"TM37");
		/// <summary>Teaches Fire Blast to a compatible Pokémon.</summary>
		public static readonly Items TM38			= new Items(342,	"TM38");
		/// <summary>Teaches Rock Tomb to a compatible Pokémon.</summary>
		public static readonly Items TM39			= new Items(343,	"TM39");
		/// <summary>Teaches Aerial Ace to a compatible Pokémon.</summary>
		public static readonly Items TM40			= new Items(344,	"TM40");
		/// <summary>Teaches Torment to a compatible Pokémon.</summary>
		public static readonly Items TM41			= new Items(345,	"TM41");
		/// <summary>Teaches Facade to a compatible Pokémon.</summary>
		public static readonly Items TM42			= new Items(346,	"TM42");
		/// <summary>Teaches Secret Power to a compatible Pokémon.</summary>
		public static readonly Items TM43			= new Items(347,	"TM43");
		/// <summary>Teaches Rest to a compatible Pokémon.</summary>
		public static readonly Items TM44			= new Items(348,	"TM44");
		/// <summary>Teaches Attract to a compatible Pokémon.</summary>
		public static readonly Items TM45			= new Items(349,	"TM45");
		/// <summary>Teaches Thief to a compatible Pokémon.</summary>
		public static readonly Items TM46			= new Items(350,	"TM46");
		/// <summary>Teaches Steel Wing to a compatible Pokémon.</summary>
		public static readonly Items TM47			= new Items(351,	"TM47");
		/// <summary>Teaches Skill Swap to a compatible Pokémon.</summary>
		public static readonly Items TM48			= new Items(352,	"TM48");
		/// <summary>Teaches Snatch to a compatible Pokémon.</summary>
		public static readonly Items TM49			= new Items(353,	"TM49");
		/// <summary>Teaches Overheat to a compatible Pokémon.</summary>
		public static readonly Items TM50			= new Items(354,	"TM50");
		/// <summary>Teaches Roost to a compatible Pokémon.</summary>
		public static readonly Items TM51			= new Items(355,	"TM51");
		/// <summary>Teaches Focus Blast to a compatible Pokémon.</summary>
		public static readonly Items TM52			= new Items(356,	"TM52");
		/// <summary>Teaches Energy Ball to a compatible Pokémon.</summary>
		public static readonly Items TM53			= new Items(357,	"TM53");
		/// <summary>Teaches False Swipe to a compatible Pokémon.</summary>
		public static readonly Items TM54			= new Items(358,	"TM54");
		/// <summary>Teaches Brine to a compatible Pokémon.</summary>
		public static readonly Items TM55			= new Items(359,	"TM55");
		/// <summary>Teaches Fling to a compatible Pokémon.</summary>
		public static readonly Items TM56			= new Items(360,	"TM56");
		/// <summary>Teaches Charge Beam to a compatible Pokémon.</summary>
		public static readonly Items TM57			= new Items(361,	"TM57");
		/// <summary>Teaches Endure to a compatible Pokémon.</summary>
		public static readonly Items TM58			= new Items(362,	"TM58");
		/// <summary>Teaches Dragon Pulse to a compatible Pokémon.</summary>
		public static readonly Items TM59			= new Items(363,	"TM59");
		/// <summary>Teaches Drain Punch to a compatible Pokémon.</summary>
		public static readonly Items TM60			= new Items(364,	"TM60");
		/// <summary>Teaches Will-O-Wisp to a compatible Pokémon.</summary>
		public static readonly Items TM61			= new Items(365,	"TM61");
		/// <summary>Teaches Silver Wind to a compatible Pokémon.</summary>
		public static readonly Items TM62			= new Items(366,	"TM62");
		/// <summary>Teaches Embargo to a compatible Pokémon.</summary>
		public static readonly Items TM63			= new Items(367,	"TM63");
		/// <summary>Teaches Explosion to a compatible Pokémon.</summary>
		public static readonly Items TM64			= new Items(368,	"TM64");
		/// <summary>Teaches Shadow Claw to a compatible Pokémon.</summary>
		public static readonly Items TM65			= new Items(369,	"TM65");
		/// <summary>Teaches Payback to a compatible Pokémon.</summary>
		public static readonly Items TM66			= new Items(370,	"TM66");
		/// <summary>Teaches Recycle to a compatible Pokémon.</summary>
		public static readonly Items TM67			= new Items(371,	"TM67");
		/// <summary>Teaches Giga Impact to a compatible Pokémon.</summary>
		public static readonly Items TM68			= new Items(372,	"TM68");
		/// <summary>Teaches Rock Polish to a compatible Pokémon.</summary>
		public static readonly Items TM69			= new Items(373,	"TM69");
		/// <summary>Teaches Flash to a compatible Pokémon.</summary>
		public static readonly Items TM70			= new Items(374,	"TM70");
		/// <summary>Teaches Stone Edge to a compatible Pokémon.</summary>
		public static readonly Items TM71			= new Items(375,	"TM71");
		/// <summary>Teaches Avalanche to a compatible Pokémon.</summary>
		public static readonly Items TM72			= new Items(376,	"TM72");
		/// <summary>Teaches Thunder Wave to a compatible Pokémon.</summary>
		public static readonly Items TM73			= new Items(377,	"TM73");
		/// <summary>Teaches Gyro Ball to a compatible Pokémon.</summary>
		public static readonly Items TM74			= new Items(378,	"TM74");
		/// <summary>Teaches Swords Dance to a compatible Pokémon.</summary>
		public static readonly Items TM75			= new Items(379,	"TM75");
		/// <summary>Teaches Stealth Rock to a compatible Pokémon.</summary>
		public static readonly Items TM76			= new Items(380,	"TM76");
		/// <summary>Teaches Psych Up to a compatible Pokémon.</summary>
		public static readonly Items TM77			= new Items(381,	"TM77");
		/// <summary>Teaches Captivate to a compatible Pokémon.</summary>
		public static readonly Items TM78			= new Items(382,	"TM78");
		/// <summary>Teaches Dark Pulse to a compatible Pokémon.</summary>
		public static readonly Items TM79			= new Items(383,	"TM79");
		/// <summary>Teaches Rock Slide to a compatible Pokémon.</summary>
		public static readonly Items TM80			= new Items(384,	"TM80");
		/// <summary>Teaches X-Scissor to a compatible Pokémon.</summary>
		public static readonly Items TM81			= new Items(385,	"TM81");
		/// <summary>Teaches Sleep Talk to a compatible Pokémon.</summary>
		public static readonly Items TM82			= new Items(386,	"TM82");
		/// <summary>Teaches Natural Gift to a compatible Pokémon.</summary>
		public static readonly Items TM83			= new Items(387,	"TM83");
		/// <summary>Teaches Poison Jab to a compatible Pokémon.</summary>
		public static readonly Items TM84			= new Items(388,	"TM84");
		/// <summary>Teaches Dream Eater to a compatible Pokémon.</summary>
		public static readonly Items TM85			= new Items(389,	"TM85");
		/// <summary>Teaches Grass Knot to a compatible Pokémon.</summary>
		public static readonly Items TM86			= new Items(390,	"TM86");
		/// <summary>Teaches Swagger to a compatible Pokémon.</summary>
		public static readonly Items TM87			= new Items(391,	"TM87");
		/// <summary>Teaches Pluck to a compatible Pokémon.</summary>
		public static readonly Items TM88			= new Items(392,	"TM88");
		/// <summary>Teaches U-Turn to a compatible Pokémon.</summary>
		public static readonly Items TM89			= new Items(393,	"TM89");
		/// <summary>Teaches Substitute to a compatible Pokémon.</summary>
		public static readonly Items TM90			= new Items(394,	"TM90");
		/// <summary>Teaches Flash Cannon to a compatible Pokémon.</summary>
		public static readonly Items TM91			= new Items(395,	"TM91");
		/// <summary>Teaches Trick Room to a compatible Pokémon.</summary>
		public static readonly Items TM92			= new Items(396,	"TM92");*/
		/// <summary>Teaches Cut to a compatible Pokémon.</summary>
		public static readonly Items HM01			= new Items(397,	"HM01");
		/// <summary>Teaches Fly to a compatible Pokémon.</summary>
		public static readonly Items HM02			= new Items(398,	"HM02");
		/// <summary>Teaches Surf to a compatible Pokémon.</summary>
		public static readonly Items HM03			= new Items(399,	"HM03");
		/// <summary>Teaches Strength to a compatible Pokémon.</summary>
		public static readonly Items HM04			= new Items(400,	"HM04");
		/// <summary>Teaches Defog to a compatible Pokémon.</summary>
		public static readonly Items HM05			= new Items(401,	"HM05");
		/// <summary>Teaches Rock Smash to a compatible Pokémon.</summary>
		public static readonly Items HM06			= new Items(402,	"HM06");
		/// <summary>Teaches Waterfall to a compatible Pokémon.</summary>
		public static readonly Items HM07			= new Items(403,	"HM07");
		/// <summary>Teaches Rock Climb to a compatible Pokémon.</summary>
		public static readonly Items HM08			= new Items(404,	"HM08");
		/// <summary>Sends the trainer to the Underground.  Only usable outside.</summary>
		public static readonly Items EXPLORER_KIT			= new Items(405,	"EXPLORER_KIT");
		/// <summary>Unused.</summary>
		public static readonly Items LOOT_SACK			= new Items(406,	"LOOT_SACK");
		/// <summary>Unused.</summary>
		public static readonly Items RULE_BOOK			= new Items(407,	"RULE_BOOK");
		/// <summary>Designates several nearby patches of grass as containing Pokémon, some of which may be special radar-only Pokémon.  Successive uses in a certain way create chains of encounters with the same species; longer chains increase the chance that a shiny Pokémon of that species will appear.</summary>
		public static readonly Items POKE_RADAR			= new Items(408,	"POKE_RADAR");
		/// <summary>Tracks Battle Points.</summary>
		public static readonly Items POINT_CARD			= new Items(409,	"POINT_CARD");
		/// <summary>Records some of the trainer's activities for the day.</summary>
		public static readonly Items JOURNAL			= new Items(410,	"JOURNAL");
		/// <summary>Contains Seals used for decorating Pokéballs.</summary>
		public static readonly Items SEAL_CASE			= new Items(411,	"SEAL_CASE");
		/// <summary>Contains Pokémon Accessories.</summary>
		public static readonly Items FASHION_CASE			= new Items(412,	"FASHION_CASE");
		/// <summary>Unused.</summary>
		public static readonly Items SEAL_BAG			= new Items(413,	"SEAL_BAG");
		/// <summary>Contains friend codes for up to 32 other players, as well as their sprite, gender, and basic statistics for those that have been seen on WFC.</summary>
		public static readonly Items PAL_PAD			= new Items(414,	"PAL_PAD");
		/// <summary>Opens the front door of the Valley Windworks.  Reusable.</summary>
		public static readonly Items WORKS_KEY			= new Items(415,	"WORKS_KEY");
		/// <summary>Given to Cynthia's grandmother to get the Surf HM.</summary>
		public static readonly Items OLD_CHARM			= new Items(416,	"OLD_CHARM");
		/// <summary>Grants access to Galactic HQ in Veilstone City.</summary>
		public static readonly Items GALACTIC_KEY			= new Items(417,	"GALACTIC_KEY");
		/// <summary>Unused.</summary>
		public static readonly Items RED_CHAIN			= new Items(418,	"RED_CHAIN");
		/// <summary>Displays a map of the region including the trainer's position, location names, visited towns, gym locations, and where the trainer has been walking recently.</summary>
		public static readonly Items TOWN_MAP			= new Items(419,	"TOWN_MAP");
		/// <summary>Reveals trainers who want a rematch, by showing !! over their heads.  Each use drains the battery; requires 100 steps to charge.</summary>
		public static readonly Items VS_SEEKER			= new Items(420,	"VS_SEEKER");
		/// <summary>Contains the Coins used by the Game Corner, to a maximum of 50,000.</summary>
		public static readonly Items COIN_CASE			= new Items(421,	"COIN_CASE");
		/// <summary>Used to find Pokémon on the Old Rod list for an area, which are generally Magikarp or similar.</summary>
		public static readonly Items OLD_ROD			= new Items(422,	"OLD_ROD");
		/// <summary>Used to find Pokémon on the Good Rod list for an area, which are generally mediocre.</summary>
		public static readonly Items GOOD_ROD			= new Items(423,	"GOOD_ROD");
		/// <summary>Used to find Pokémon on the Super Rod list for an area, which are generally the best available there.</summary>
		public static readonly Items SUPER_ROD			= new Items(424,	"SUPER_ROD");
		/// <summary>Waters Berry plants.</summary>
		public static readonly Items SPRAYDUCK			= new Items(425,	"SPRAYDUCK");
		/// <summary>Contains up to 100 Poffins.</summary>
		public static readonly Items POFFIN_CASE			= new Items(426,	"POFFIN_CASE");
		/// <summary>Increases movement speed outside or in caves.  In high gear, allows the trainer to hop over some rocks and ascend muddy slopes.</summary>
		public static readonly Items BICYCLE			= new Items(427,	"BICYCLE");
		/// <summary>Opens the locked building in the lakeside resort.</summary>
		public static readonly Items SUITE_KEY			= new Items(428,	"SUITE_KEY");
		/// <summary>Grants access to Flower Paradise and Shaymin.</summary>
		public static readonly Items OAKS_LETTER			= new Items(429,	"OAKS_LETTER");
		/// <summary>Cures the sailor's son of his nightmares; no reward, only a side effect of seeing Cresselia.</summary>
		public static readonly Items LUNAR_WING			= new Items(430,	"LUNAR_WING");
		/// <summary>Provides access to Newmoon Island and Darkrai.</summary>
		public static readonly Items MEMBER_CARD			= new Items(431,	"MEMBER_CARD");
		/// <summary>Supposedly related to t</summary>
		public static readonly Items AZURE_FLUTE			= new Items(432,	"AZURE_FLUTE");
		/// <summary>Allows passage on a ferry.  The same item is used for different ferries between different games.</summary>
		public static readonly Items SS_TICKET			= new Items(433,	"SS_TICKET");
		/// <summary>Allows the trainer to enter Contests.</summary>
		public static readonly Items CONTEST_PASS			= new Items(434,	"CONTEST_PASS");
		/// <summary>Causes Heatran to appear at Reversal Mountain.  Unused prior to Black and White 2.</summary>
		public static readonly Items MAGMA_STONE			= new Items(435,	"MAGMA_STONE");
		/// <summary>Given to the trainer's rival in Jubilife City.  Contains two Town Maps, one of which is given to the trainer upon delivery.</summary>
		public static readonly Items PARCEL			= new Items(436,	"PARCEL");
		/// <summary>One of three coupons needed to receive a Pokétch.</summary>
		public static readonly Items COUPON_1			= new Items(437,	"COUPON_1");
		/// <summary>One of three coupons needed to receive a Pokétch.</summary>
		public static readonly Items COUPON_2			= new Items(438,	"COUPON_2");
		/// <summary>One of three coupons needed to receive a Pokétch.</summary>
		public static readonly Items COUPON_3			= new Items(439,	"COUPON_3");
		/// <summary>Grants access to the Team Galactic warehouse in Veilstone City.</summary>
		public static readonly Items STORAGE_KEY			= new Items(440,	"STORAGE_KEY");
		/// <summary>Required to cure the Psyducks blocking Route 210 of their chronic headaches.</summary>
		public static readonly Items SECRET_POTION			= new Items(441,	"SECRET_POTION");
		/// <summary>Held by []{pokemon:giratina} :   Holder's []{type:dragon} and []{type:ghost} moves have 1.2× their base power.      Holder is in Origin Forme.  This item cannot be held by any Pokémon but Giratina.  When you enter the Union Room or connect to Wi-Fi, this item returns to your bag.</summary>
		public static readonly Items GRISEOUS_ORB			= new Items(442,	"GRISEOUS_ORB");
		/// <summary>Optionally records wireless, Wi-Fi, and Battle Frontier battles.  Tracks Battle Points earned in the Battle Frontier, and stores commemorative prints.</summary>
		public static readonly Items VS_RECORDER			= new Items(443,	"VS_RECORDER");
		/// <summary>Used by trainer on a []{pokemon:shaymin} :   Changes the target Shaymin from Land Forme to Sky Forme.      This item cannot be used on a [frozen]{mechanic:freezing} Shaymin or at night.  Sky Forme Shaymin will revert to Land Forme overnight, when frozen, and upon entering a link battle.  This item must be used again to change it back.</summary>
		public static readonly Items GRACIDEA			= new Items(444,	"GRACIDEA");
		/// <summary>Used by trainer in the Galactic Eterna Building, on the ground floor, to the left of the TV :   Unlocks the secret []{pokemon:rotom} room, in which there are five appliances which can change Rotom's form.</summary>
		public static readonly Items SECRET_KEY			= new Items(445,	"SECRET_KEY");
		/// <summary>Stores Apricorns.</summary>
		public static readonly Items APRICORN_BOX			= new Items(446,	"APRICORN_BOX");
		/// <summary>Contains four portable pots of soil suitable for growing berries.</summary>
		public static readonly Items BERRY_POTS			= new Items(447,	"BERRY_POTS");
		/// <summary>Required to water berries within the []{item:berry-pots}.  Required to battle the []{pokemon:sudowoodo} on []{location:johto-route-36}.  This item cannot be directly used from the bag.</summary>
		public static readonly Items SQUIRT_BOTTLE			= new Items(448,	"SQUIRT_BOTTLE");
		/// <summary>Used by trainer in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.  If used in a trainer battle, nothing happens and the ball is lost.      If the wild Pokémon was encountered by fishing, the wild Pokémon's catch rate is 3× normal.</summary>
		public static readonly Items LURE_BALL			= new Items(449,	"LURE_BALL");
		/// <summary>Used by trainer in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.  If used in a trainer battle, nothing happens and the ball is lost.      If the trainer's Pokémon's level is higher than:      * four times the wild Pokémon's, the wild Pokémon's catch rate is 8× normal.     * than twice the wild Pokémon's, the wild Pokémon's catch rate is 4× normal.     * the wild Pokémon's, the wild Pokémon's catch rate is 2× normal.</summary>
		public static readonly Items LEVEL_BALL			= new Items(450,	"LEVEL_BALL");
		/// <summary>Used by trainer in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.  If used in a trainer battle, nothing happens and the ball is lost.      If the wild Pokémon is a []{pokemon:clefairy}, []{pokemon:nidoran-m}, []{pokemon:nidoran-f}, []{pokemon:jigglypuff}, []{pokemon:skitty}, or any evolution thereof, the wild Pokémon has 4× its catch rate.</summary>
		public static readonly Items MOON_BALL			= new Items(451,	"MOON_BALL");
		/// <summary>Used by a trainer in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.  If used in a trainer battle, nothing happens and the ball is lost.      If the wild Pokémon weighs:      * 409.6 kg (903.0 lb) or more, its catch rate is 40 more than normal.     * 307.2 kg (677.3 lb) or more, its catch rate is 30 more than normal.     * 204.8 kg (451.5 lb) or more, its catch rate is 20 more than normal.     * less than 204.8 kg (451.5 lb), its catch rate is 20 less than normal.</summary>
		public static readonly Items HEAVY_BALL			= new Items(452,	"HEAVY_BALL");
		/// <summary>Used by a trainer in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.  If used in a trainer battle, nothing happens and the ball is lost.  :   If the wild Pokémon's base [speed]{mechanic:speed} is 100 or more, its catch rate is 4× normal.</summary>
		public static readonly Items FAST_BALL			= new Items(453,	"FAST_BALL");
		/// <summary>Used by a trainer in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.  If used in a trainer battle, nothing happens and the ball is lost.      If caught, the wild Pokémon's [happiness]{mechanic:happiness} starts at 200.</summary>
		public static readonly Items FRIEND_BALL			= new Items(454,	"FRIEND_BALL");
		/// <summary>Used by a trainer in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.  If used in a trainer battle, nothing happens and the ball is lost.      If the trainer's Pokémon and wild Pokémon are of the same species but opposite genders, the wild Pokémon's catch rate is 8× normal.</summary>
		public static readonly Items LOVE_BALL			= new Items(455,	"LOVE_BALL");
		/// <summary>Used by a trainer in battle :   [Catches]{mechanic:catch} a wild Pokémon.  This item can only be used in []{location:pal-park}.</summary>
		public static readonly Items PARK_BALL			= new Items(456,	"PARK_BALL");
		/// <summary>Used by a trainer in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.      The wild Pokémon's catch rate is 1.5× normal.</summary>
		public static readonly Items SPORT_BALL			= new Items(457,	"SPORT_BALL");
		/// <summary>May be given to Kurt in []{location:azalea-town} to produce a []{item:level-ball}.</summary>
		public static readonly Items RED_APRICORN			= new Items(458,	"RED_APRICORN");
		/// <summary>May be given to Kurt in []{location:azalea-town} to produce a []{item:lure-ball}.</summary>
		public static readonly Items BLUE_APRICORN			= new Items(459,	"BLUE_APRICORN");
		/// <summary>May be given to Kurt in []{location:azalea-town} to produce a []{item:moon-ball}.</summary>
		public static readonly Items YELLOW_APRICORN			= new Items(460,	"YELLOW_APRICORN");
		/// <summary>May be given to Kurt in []{location:azalea-town} to produce a []{item:friend-ball}.</summary>
		public static readonly Items GREEN_APRICORN			= new Items(461,	"GREEN_APRICORN");
		/// <summary>May be given to Kurt in []{location:azalea-town} to produce a []{item:love-ball}.</summary>
		public static readonly Items PINK_APRICORN			= new Items(462,	"PINK_APRICORN");
		/// <summary>May be given to Kurt in []{location:azalea-town} to produce a []{item:fast-ball}.</summary>
		public static readonly Items WHITE_APRICORN			= new Items(463,	"WHITE_APRICORN");
		/// <summary>May be given to Kurt in []{location:azalea-town} to produce a []{item:heavy-ball}.</summary>
		public static readonly Items BLACK_APRICORN			= new Items(464,	"BLACK_APRICORN");
		/// <summary>Used by trainer outside of battle :   Searches for hidden items.</summary>
		public static readonly Items DOWSING_MACHINE			= new Items(465,	"DOWSING_MACHINE");
		/// <summary>May be traded for a []{item:tm64} in the vertical Underground Path.</summary>
		public static readonly Items RAGE_CANDY_BAR			= new Items(466,	"RAGE_CANDY_BAR");
		/// <summary>Causes []{pokemon:groudon} to appear in the []{location:embedded-tower}.</summary>
		public static readonly Items RED_ORB			= new Items(467,	"RED_ORB");
		/// <summary>Causes []{pokemon:kyogre} to appear in the []{location:embedded-tower}.</summary>
		public static readonly Items BLUE_ORB			= new Items(468,	"BLUE_ORB");
		/// <summary>Causes []{pokemon:rayquaza} to appear in the []{location:embedded-tower}.</summary>
		public static readonly Items JADE_ORB			= new Items(469,	"JADE_ORB");
		/// <summary>When taken to the []{location:pewter-city} museum, causes []{pokemon:latias} or []{pokemon:latios} to attack the trainer.  The Pokémon to appear will be whicher can't be encountered roaming in the wild.</summary>
		public static readonly Items ENIGMA_STONE			= new Items(470,	"ENIGMA_STONE");
		/// <summary>Lists which []{pokemon:unown} forms the trainer has caught.</summary>
		public static readonly Items UNOWN_REPORT			= new Items(471,	"UNOWN_REPORT");
		/// <summary>Allows the trainer to answer the daily question on Buena's radio show.  Records the points earned for correct answers.</summary>
		public static readonly Items BLUE_CARD			= new Items(472,	"BLUE_CARD");
		/// <summary>Does nothing.</summary>
		public static readonly Items SLOWPOKE_TAIL			= new Items(473,	"SLOWPOKE_TAIL");
		/// <summary>May be given to the Kimono Girls to summon []{pokemon:ho-oh} to the top of the []{location:bell-tower}.</summary>
		public static readonly Items CLEAR_BELL			= new Items(474,	"CLEAR_BELL");
		/// <summary>Used by trainer outside of battle :   Opens doors in the []{location:goldenrod-city} Radio Tower.</summary>
		public static readonly Items CARD_KEY			= new Items(475,	"CARD_KEY");
		/// <summary>Used by trainer outside of battle :   Opens the door to the basement tunnel under []{location:goldenrod-city}.</summary>
		public static readonly Items BASEMENT_KEY			= new Items(476,	"BASEMENT_KEY");
		/// <summary>May be traded to Mr. Pokémon for an []{item:exp-share}.</summary>
		public static readonly Items RED_SCALE			= new Items(477,	"RED_SCALE");
		/// <summary>May be traded to the Copycat for a []{item:pass}.</summary>
		public static readonly Items LOST_ITEM			= new Items(478,	"LOST_ITEM");
		/// <summary>Allows the trainer to ride the Magnet Train between []{location:goldenrod-city} and []{location:saffron-city}.</summary>
		public static readonly Items PASS			= new Items(479,	"PASS");
		/// <summary>Must be replaced in the []{location:power-plant} to power the Magnet Train.</summary>
		public static readonly Items MACHINE_PART			= new Items(480,	"MACHINE_PART");
		/// <summary>Causes []{pokemon:lugia} to appear in the []{location:whirl-islands}.</summary>
		public static readonly Items SILVER_WING			= new Items(481,	"SILVER_WING");
		/// <summary>Causes []{pokemon:ho-oh} to appear at the top of []{location:bell-tower}.</summary>
		public static readonly Items RAINBOW_WING			= new Items(482,	"RAINBOW_WING");
		/// <summary>Must be obtained to trigger the break-in at Professor Elm's lab, the first rival battle, and access to []{location:johto-route-31}.</summary>
		public static readonly Items MYSTERY_EGG			= new Items(483,	"MYSTERY_EGG");
		/// <summary>Used by trainer outside of battle :   Changes the background music to the equivalent 8-bit music.</summary>
		public static readonly Items GB_SOUNDS			= new Items(484,	"GB_SOUNDS");
		/// <summary>May be given to the Kimono Girls to summon []{pokemon:lugia} to the top of the []{location:bell-tower}.</summary>
		public static readonly Items TIDAL_BELL			= new Items(485,	"TIDAL_BELL");
		/// <summary>Records the number of times the trainer has come in first place overall in the Pokéathlon.</summary>
		public static readonly Items DATA_CARD_01			= new Items(486,	"DATA_CARD_01");
		/// <summary>Records the number of times the trainer has come in last place overall in the Pokéathlon.</summary>
		public static readonly Items DATA_CARD_02			= new Items(487,	"DATA_CARD_02");
		/// <summary>Records the number of times the trainer's Pokémon have dashed in the Pokéathlon.</summary>
		public static readonly Items DATA_CARD_03			= new Items(488,	"DATA_CARD_03");
		/// <summary>Records the number of times the trainer's Pokémon have jumped in the Pokéathlon.</summary>
		public static readonly Items DATA_CARD_04			= new Items(489,	"DATA_CARD_04");
		/// <summary>Records the number of times the trainer has come in first in the Pokéathlon Hurdle Dash.</summary>
		public static readonly Items DATA_CARD_05			= new Items(490,	"DATA_CARD_05");
		/// <summary>Records the number of times the trainer has come in first in the Pokéathlon Relay Run.</summary>
		public static readonly Items DATA_CARD_06			= new Items(491,	"DATA_CARD_06");
		/// <summary>Records the number of times the trainer has come in first in the Pokéathlon Pennant Capture.</summary>
		public static readonly Items DATA_CARD_07			= new Items(492,	"DATA_CARD_07");
		/// <summary>Records the number of times the trainer has come in first in the Pokéathlon Block Smash.</summary>
		public static readonly Items DATA_CARD_08			= new Items(493,	"DATA_CARD_08");
		/// <summary>Records the number of times the trainer has come in first in the Pokéathlon Disc Catch.</summary>
		public static readonly Items DATA_CARD_09			= new Items(494,	"DATA_CARD_09");
		/// <summary>Records the number of times the trainer has come in first in the Pokéathlon Snow Throw.</summary>
		public static readonly Items DATA_CARD_10			= new Items(495,	"DATA_CARD_10");
		/// <summary>Records the number of points the trainer has earned in the Pokéathlon.</summary>
		public static readonly Items DATA_CARD_11			= new Items(496,	"DATA_CARD_11");
		/// <summary>Records the number of times the trainer's Pokémon have messed up in the Pokéathlon.</summary>
		public static readonly Items DATA_CARD_12			= new Items(497,	"DATA_CARD_12");
		/// <summary>Records the number of times the trainer's Pokémon have defeated themselves in the Pokéathlon.</summary>
		public static readonly Items DATA_CARD_13			= new Items(498,	"DATA_CARD_13");
		/// <summary>Records the number of times the trainer's Pokémon have tackled in the Pokéathlon.</summary>
		public static readonly Items DATA_CARD_14			= new Items(499,	"DATA_CARD_14");
		/// <summary>Records the number of times the trainer's Pokémon have fallen in the Pokéathlon.</summary>
		public static readonly Items DATA_CARD_15			= new Items(500,	"DATA_CARD_15");
		/// <summary>Records the number of times the trainer has come in first in the Pokéathlon Ring Drop.</summary>
		public static readonly Items DATA_CARD_16			= new Items(501,	"DATA_CARD_16");
		/// <summary>Records the number of times the trainer has come in first in the Pokéathlon Lamp Jump.</summary>
		public static readonly Items DATA_CARD_17			= new Items(502,	"DATA_CARD_17");
		/// <summary>Records the number of times the trainer has come in first in the Pokéathlon Circle Push.</summary>
		public static readonly Items DATA_CARD_18			= new Items(503,	"DATA_CARD_18");
		/// <summary>Records the number of times the trainer has come in first place overall in the Pokéathlon over wirelss.</summary>
		public static readonly Items DATA_CARD_19			= new Items(504,	"DATA_CARD_19");
		/// <summary>Records the number of times the trainer has come in last place overall in the Pokéathlon over wireless.</summary>
		public static readonly Items DATA_CARD_20			= new Items(505,	"DATA_CARD_20");
		/// <summary>Records the number of times the trainer has come in first across all Pokéathlon events.</summary>
		public static readonly Items DATA_CARD_21			= new Items(506,	"DATA_CARD_21");
		/// <summary>Records the number of times the trainer has come in last across all Pokéathlon events.</summary>
		public static readonly Items DATA_CARD_22			= new Items(507,	"DATA_CARD_22");
		/// <summary>Records the number of times the trainer has switched Pokémon in the Pokéathlon.</summary>
		public static readonly Items DATA_CARD_23			= new Items(508,	"DATA_CARD_23");
		/// <summary>Records the number of times the trainer has come in first in the Pokéathlon Goal Roll.</summary>
		public static readonly Items DATA_CARD_24			= new Items(509,	"DATA_CARD_24");
		/// <summary>Records the number of times the trainer's Pokémon received prizes in the Pokéathlon.</summary>
		public static readonly Items DATA_CARD_25			= new Items(510,	"DATA_CARD_25");
		/// <summary>Records the number of times the trainer has instructed Pokémon in the Pokéathlon.</summary>
		public static readonly Items DATA_CARD_26			= new Items(511,	"DATA_CARD_26");
		/// <summary>Records the total time spent in the Pokéathlon.</summary>
		public static readonly Items DATA_CARD_27			= new Items(512,	"DATA_CARD_27");
		/// <summary>Does nothing.</summary>
		public static readonly Items LOCK_CAPSULE			= new Items(513,	"LOCK_CAPSULE");
		/// <summary>Does nothing.</summary>
		public static readonly Items PHOTO_ALBUM			= new Items(514,	"PHOTO_ALBUM");
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		public static readonly Items ORANGE_MAIL			= new Items(515,	"ORANGE_MAIL");
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		public static readonly Items HARBOR_MAIL			= new Items(516,	"HARBOR_MAIL");
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		public static readonly Items GLITTER_MAIL			= new Items(517,	"GLITTER_MAIL");
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		public static readonly Items MECH_MAIL			= new Items(518,	"MECH_MAIL");
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		public static readonly Items WOOD_MAIL			= new Items(519,	"WOOD_MAIL");
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		public static readonly Items WAVE_MAIL			= new Items(520,	"WAVE_MAIL");
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		public static readonly Items BEAD_MAIL			= new Items(521,	"BEAD_MAIL");
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		public static readonly Items SHADOW_MAIL			= new Items(522,	"SHADOW_MAIL");
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		public static readonly Items TROPIC_MAIL			= new Items(523,	"TROPIC_MAIL");
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		public static readonly Items DREAM_MAIL			= new Items(524,	"DREAM_MAIL");
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		public static readonly Items FAB_MAIL			= new Items(525,	"FAB_MAIL");
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		public static readonly Items RETRO_MAIL			= new Items(526,	"RETRO_MAIL");
		/// <summary>Increases movement speed outside or in caves.  Faster than the []{item:acro-bike}.  Allows the trainer to ascend muddy slopes.</summary>
		public static readonly Items MACH_BIKE			= new Items(527,	"MACH_BIKE");
		/// <summary>Increases movement speed outside or in caves.  Slower than the []{item:mach-bike}.  Can perform various tricks, allowing the trainer to reach certain special areas.</summary>
		public static readonly Items ACRO_BIKE			= new Items(528,	"ACRO_BIKE");
		/// <summary>Waters Berry plants.</summary>
		public static readonly Items WAILMER_PAIL			= new Items(529,	"WAILMER_PAIL");
		/// <summary>Contains a machine part to be delivered to Captain Stern.</summary>
		public static readonly Items DEVON_GOODS			= new Items(530,	"DEVON_GOODS");
		/// <summary>Collects soot when walking through tall grass on []{location:hoenn-route-113}.</summary>
		public static readonly Items SOOT_SACK			= new Items(531,	"SOOT_SACK");
		/// <summary>Stores Pokéblocks.</summary>
		public static readonly Items POKEBLOCK_CASE			= new Items(532,	"POKEBLOCK_CASE");
		/// <summary>Contains a letter to be delivered to Steven.</summary>
		public static readonly Items LETTER			= new Items(533,	"LETTER");
		/// <summary>Provides access to []{location:southern-island} and either []{pokemon:latias} or []{pokemon:latios}, whichever is not available roaming around Hoenn.</summary>
		public static readonly Items EON_TICKET			= new Items(534,	"EON_TICKET");
		/// <summary>May be traded to Captain Stern for a []{item:deep-sea-tooth} or a []{item:deep-sea-scale}.</summary>
		public static readonly Items SCANNER			= new Items(535,	"SCANNER");
		/// <summary>Allows the trainer to enter the desert on []{location:hoenn-route-111}.</summary>
		public static readonly Items GO_GOGGLES			= new Items(536,	"GO_GOGGLES");
		/// <summary>RSE: May be traded to Professor Cozmo for []{item:tm27}.  FRLG: A meteorite to be delivered to Lostelle's father.</summary>
		public static readonly Items METEORITE			= new Items(537,	"METEORITE");
		/// <summary>Unlocks room 1 on the []{location:abandoned-ship}.</summary>
		public static readonly Items RM_1_KEY			= new Items(538,	"RM_1_KEY");
		/// <summary>Unlocks room 2 on the []{location:abandoned-ship}.</summary>
		public static readonly Items RM_2_KEY			= new Items(539,	"RM_2_KEY");
		/// <summary>Unlocks room 4 on the []{location:abandoned-ship}.</summary>
		public static readonly Items RM_4_KEY			= new Items(540,	"RM_4_KEY");
		/// <summary>Unlocks room 6 on the []{location:abandoned-ship}.</summary>
		public static readonly Items RM_6_KEY			= new Items(541,	"RM_6_KEY");
		/// <summary>Reveals invisble []{pokemon:kecleon} on the overworld.</summary>
		public static readonly Items DEVON_SCOPE			= new Items(542,	"DEVON_SCOPE");
		/// <summary>A parcel to be delivered to Professor Oak for a Pokédex.</summary>
		public static readonly Items OAKS_PARCEL			= new Items(543,	"OAKS_PARCEL");
		/// <summary>Wakes up [sleeping]{mechanic:sleep} Pokémon.  Required to wake up sleeping []{pokemon:snorlax} on the overworld.</summary>
		public static readonly Items POKE_FLUTE			= new Items(544,	"POKE_FLUTE");
		/// <summary>May be traded for a []{item:bicycle}.</summary>
		public static readonly Items BIKE_VOUCHER			= new Items(545,	"BIKE_VOUCHER");
		/// <summary>The Safari Zone warden's teeth, to be returned to him for []{item:hm04}.</summary>
		public static readonly Items GOLD_TEETH			= new Items(546,	"GOLD_TEETH");
		/// <summary>Operates the elevator in the Celadon Rocket Hideout.</summary>
		public static readonly Items LIFT_KEY			= new Items(547,	"LIFT_KEY");
		/// <summary>Identifies ghosts in []{location:pokemon-tower}.</summary>
		public static readonly Items SILPH_SCOPE			= new Items(548,	"SILPH_SCOPE");
		/// <summary>Records information on various famous people.</summary>
		public static readonly Items FAME_CHECKER			= new Items(549,	"FAME_CHECKER");
		/// <summary>Stores TMs and HMs.</summary>
		public static readonly Items TM_CASE			= new Items(550,	"TM_CASE");
		/// <summary>Stores Berries.</summary>
		public static readonly Items BERRY_POUCH			= new Items(551,	"BERRY_POUCH");
		/// <summary>Teaches beginning trainers basic information.</summary>
		public static readonly Items TEACHY_TV			= new Items(552,	"TEACHY_TV");
		/// <summary>Provides access to the first three Sevii Islands.</summary>
		public static readonly Items TRI_PASS			= new Items(553,	"TRI_PASS");
		/// <summary>Provides access to the Sevii Islands.</summary>
		public static readonly Items RAINBOW_PASS			= new Items(554,	"RAINBOW_PASS");
		/// <summary>Used to bribe the []{location:saffron-city} guards for entry to the city.</summary>
		public static readonly Items TEA			= new Items(555,	"TEA");
		/// <summary>Provides access to Navel Rock, []{pokemon:ho-oh}, and []{pokemon:lugia}.</summary>
		public static readonly Items MYSTICTICKET			= new Items(556,	"MYSTICTICKET");
		/// <summary>Provides access to Birth Island and []{pokemon:deoxys}.</summary>
		public static readonly Items AURORATICKET			= new Items(557,	"AURORATICKET");
		/// <summary>Holds Berry Powder from Berry Crushing.</summary>
		public static readonly Items POWDER_JAR			= new Items(558,	"POWDER_JAR");
		/// <summary>Deliver to Celio for use in the Network Machine.</summary>
		public static readonly Items RUBY			= new Items(559,	"RUBY");
		/// <summary>Deliver to Celio for use in the Network Machine.</summary>
		public static readonly Items SAPPHIRE			= new Items(560,	"SAPPHIRE");
		/// <summary>Provides access to the []{location:magma-hideout} in the []{location:jagged-pass}.</summary>
		public static readonly Items MAGMA_EMBLEM			= new Items(561,	"MAGMA_EMBLEM");
		/// <summary>Provides access to Faraway Island and []{pokemon:mew}.</summary>
		public static readonly Items OLD_SEA_MAP			= new Items(562,	"OLD_SEA_MAP");
		/// <summary>Held by []{pokemon:genesect} :   Holder's buster is blue, and its []{move:techno-blast} is []{type:water}-type.</summary>
		public static readonly Items DOUSE_DRIVE			= new Items(563,	"DOUSE_DRIVE");
		/// <summary>Held by []{pokemon:genesect} :   Holder's buster is yellow, and its []{move:techno-blast} is []{type:electric}-type.</summary>
		public static readonly Items SHOCK_DRIVE			= new Items(564,	"SHOCK_DRIVE");
		/// <summary>Held by []{pokemon:genesect} :   Holder's buster is red, and its []{move:techno-blast} is []{type:fire}-type.</summary>
		public static readonly Items BURN_DRIVE			= new Items(565,	"BURN_DRIVE");
		/// <summary>Held by []{pokemon:genesect} :   Holder's buster is white, and its []{move:techno-blast} becomes []{type:ice}-type.</summary>
		public static readonly Items CHILL_DRIVE			= new Items(566,	"CHILL_DRIVE");
		/// <summary>Used on a friendly Pokémon :   Restores 20 [HP]{mechanic:hp}.</summary>
		public static readonly Items SWEET_HEART			= new Items(567,	"SWEET_HEART");
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		public static readonly Items GREET_MAIL			= new Items(568,	"GREET_MAIL");
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		public static readonly Items FAVORED_MAIL			= new Items(569,	"FAVORED_MAIL");
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		public static readonly Items RSVP_MAIL			= new Items(570,	"RSVP_MAIL");
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		public static readonly Items THANKS_MAIL			= new Items(571,	"THANKS_MAIL");
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		public static readonly Items INQUIRY_MAIL			= new Items(572,	"INQUIRY_MAIL");
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		public static readonly Items LIKE_MAIL			= new Items(573,	"LIKE_MAIL");
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		public static readonly Items REPLY_MAIL			= new Items(574,	"REPLY_MAIL");
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		public static readonly Items BRIDGE_MAIL_S			= new Items(575,	"BRIDGE_MAIL_S");
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		public static readonly Items BRIDGE_MAIL_D			= new Items(576,	"BRIDGE_MAIL_D");
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		public static readonly Items BRIDGE_MAIL_T			= new Items(577,	"BRIDGE_MAIL_T");
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		public static readonly Items BRIDGE_MAIL_V			= new Items(578,	"BRIDGE_MAIL_V");
		/// <summary>Used to send short messages to other players via Pokémon trading.  Trainer may compose a message from a finite list of words when giving this item to a Pokémon.  Once taken and read, a message may be erased and this item can be reused, or the message may be stored in the trainer's PC.  Held :   Holder cannot be placed in the PC.  Any move attempting to remove this item from the holder will fail.</summary>
		public static readonly Items BRIDGE_MAIL_M			= new Items(579,	"BRIDGE_MAIL_M");
		/// <summary>Held by []{pokemon:feebas} :   Holder evolves into []{pokemon:milotic} when traded.</summary>
		public static readonly Items PRISM_SCALE			= new Items(580,	"PRISM_SCALE");
		/// <summary>Held by a Pokémon that is not fully evolved :   Holder has 1.5× [Defense]{mechanic:defense} and [Special Defense]{mechanic:special-defense}.</summary>
		public static readonly Items EVIOLITE			= new Items(581,	"EVIOLITE");
		/// <summary>Held :   Holder has 0.5× weight.</summary>
		public static readonly Items FLOAT_STONE			= new Items(582,	"FLOAT_STONE");
		/// <summary>Held :   When the holder is hit by a [contact]{mechanic:contact} move, the attacking Pokémon takes 1/6 its max [HP]{mechanic:hp} in damage.</summary>
		public static readonly Items ROCKY_HELMET			= new Items(583,	"ROCKY_HELMET");
		/// <summary>Held :   Holder is immune to []{type:ground}-type moves, []{move:spikes}, []{move:toxic-spikes}, and []{ability:arena-trap}.      This effect does not apply during []{move:gravity} or []{move:ingrain}.      When the holder takes damage from a move, this item is consumed.</summary>
		public static readonly Items AIR_BALLOON			= new Items(584,	"AIR_BALLOON");
		/// <summary>Held :   When the holder takes damage directly from a move and does not faint, it [switches out]{mechanic:switching-out} for another random, non-fainted Pokémon in its party. This effect does not activate if another effect prevents the holder from switching out.</summary>
		public static readonly Items RED_CARD			= new Items(585,	"RED_CARD");
		/// <summary>Held :   When one of the user's types would render it immune to damage, that type is ignored for damage calculation.</summary>
		public static readonly Items RING_TARGET			= new Items(586,	"RING_TARGET");
		/// <summary>Held :   Moves used by the holder that trap and damage a target for multiple turns (e.g. []{move:bind}, []{move:fire-spin}) inflict twice their usual per-turn damage.</summary>
		public static readonly Items BINDING_BAND			= new Items(587,	"BINDING_BAND");
		/// <summary>Held :   When the holder takes []{type:water}-type damage from a move, its [Special Attack]{mechanic:special-attack} rises by one [stage]{mechanic:stage} and this item is consumed.</summary>
		public static readonly Items ABSORB_BULB			= new Items(588,	"ABSORB_BULB");
		/// <summary>Held :   When the holder takes []{type:electric}-type damage from a move, its [Attack]{mechanic:attack} rises by one [stage]{mechanic:stage} and this item is consumed.</summary>
		public static readonly Items CELL_BATTERY			= new Items(589,	"CELL_BATTERY");
		/// <summary>Held :   When the holder takes damage directly from a move and does not faint, it [switches out]{mechanic:switching-out} for another non-fainted Pokémon in its party, as chosen by the Trainer. This effect does not activate if another effect prevents the holder from switching out.</summary>
		public static readonly Items EJECT_BUTTON			= new Items(590,	"EJECT_BUTTON");
		/// <summary>Held :   When the holder uses a damaging []{type:fire}-type move, the move has 1.5× power and this item is consumed.</summary>
		public static readonly Items FIRE_GEM			= new Items(591,	"FIRE_GEM");
		/// <summary>Held :   When the holder uses a damaging []{type:water}-type move, the move has 1.5× power and this item is consumed.</summary>
		public static readonly Items WATER_GEM			= new Items(592,	"WATER_GEM");
		/// <summary>Held :   When the holder uses a damaging []{type:electric}-type move, the move has 1.5× power and this item is consumed.</summary>
		public static readonly Items ELECTRIC_GEM			= new Items(593,	"ELECTRIC_GEM");
		/// <summary>Held :   When the holder uses a damaging []{type:grass}-type move, the move has 1.5× power and this item is consumed.</summary>
		public static readonly Items GRASS_GEM			= new Items(594,	"GRASS_GEM");
		/// <summary>Held :   When the holder uses a damaging []{type:ice}-type move, the move has 1.5× power and this item is consumed.</summary>
		public static readonly Items ICE_GEM			= new Items(595,	"ICE_GEM");
		/// <summary>Held :   When the holder uses a damaging []{type:fighting}-type move, the move has 1.5× power and this item is consumed.</summary>
		public static readonly Items FIGHTING_GEM			= new Items(596,	"FIGHTING_GEM");
		/// <summary>Held :   When the holder uses a damaging []{type:poison}-type move, the move has 1.5× power and this item is consumed.</summary>
		public static readonly Items POISON_GEM			= new Items(597,	"POISON_GEM");
		/// <summary>Held :   When the holder uses a damaging []{type:ground}-type move, the move has 1.5× power and this item is consumed.</summary>
		public static readonly Items GROUND_GEM			= new Items(598,	"GROUND_GEM");
		/// <summary>Held :   When the holder uses a damaging []{type:flying}-type move, the move has 1.5× power and this item is consumed.</summary>
		public static readonly Items FLYING_GEM			= new Items(599,	"FLYING_GEM");
		/// <summary>Held :   When the holder uses a damaging []{type:psychic}-type move, the move has 1.5× power and this item is consumed.</summary>
		public static readonly Items PSYCHIC_GEM			= new Items(600,	"PSYCHIC_GEM");
		/// <summary>Held :   When the holder uses a damaging []{type:bug}-type move, the move has 1.5× power and this item is consumed.</summary>
		public static readonly Items BUG_GEM			= new Items(601,	"BUG_GEM");
		/// <summary>Held :   When the holder uses a damaging []{type:rock}-type move, the move has 1.5× power and this item is consumed.</summary>
		public static readonly Items ROCK_GEM			= new Items(602,	"ROCK_GEM");
		/// <summary>Held :   When the holder uses a damaging []{type:ghost}-type move, the move has 1.5× power and this item is consumed.</summary>
		public static readonly Items GHOST_GEM			= new Items(603,	"GHOST_GEM");
		/// <summary>Held :   When the holder uses a damaging []{type:dark}-type move, the move has 1.5× power and this item is consumed.</summary>
		public static readonly Items DARK_GEM			= new Items(604,	"DARK_GEM");
		/// <summary>Held :   When the holder uses a damaging []{type:steel}-type move, the move has 1.5× power and this item is consumed.</summary>
		public static readonly Items STEEL_GEM			= new Items(605,	"STEEL_GEM");
		/// <summary>Used on a party Pokémon :   Increases the target's [HP]{mechanic:hp} [effort]{mechanic:effort} by 1.</summary>
		public static readonly Items HEALTH_WING			= new Items(606,	"HEALTH_WING");
		/// <summary>Used on a party Pokémon :   Increases the target's [Attack]{mechanic:attack} [effort]{mechanic:effort} by 1.</summary>
		public static readonly Items MUSCLE_WING			= new Items(607,	"MUSCLE_WING");
		/// <summary>Used on a party Pokémon :   Increases the target's [Defense]{mechanic:defense} [effort]{mechanic:effort} by 1.</summary>
		public static readonly Items RESIST_WING			= new Items(608,	"RESIST_WING");
		/// <summary>Used on a party Pokémon :   Increases the target's [Special Attack]{mechanic:special-attack} [effort]{mechanic:effort} by 1.</summary>
		public static readonly Items GENIUS_WING			= new Items(609,	"GENIUS_WING");
		/// <summary>Used on a party Pokémon :   Increases the target's [Special Defense]{mechanic:special-defense} [effort]{mechanic:effort} by 1.</summary>
		public static readonly Items CLEVER_WING			= new Items(610,	"CLEVER_WING");
		/// <summary>Used on a party Pokémon :   Increases the target's [Speed]{mechanic:speed} [effort]{mechanic:effort} by 1.</summary>
		public static readonly Items SWIFT_WING			= new Items(611,	"SWIFT_WING");
		/// <summary>Vendor trash.</summary>
		public static readonly Items PRETTY_WING			= new Items(612,	"PRETTY_WING");
		/// <summary>Give to a scientist in a museum to receive a []{pokemon:tirtouga}.</summary>
		public static readonly Items COVER_FOSSIL			= new Items(613,	"COVER_FOSSIL");
		/// <summary>Give to a scientist in a museum to receive a []{pokemon:archen}.</summary>
		public static readonly Items PLUME_FOSSIL			= new Items(614,	"PLUME_FOSSIL");
		/// <summary>Allows passage on the []{location:castelia-city} ship, which leads to []{location:liberty-garden} and []{pokemon:victini}.</summary>
		public static readonly Items LIBERTY_PASS			= new Items(615,	"LIBERTY_PASS");
		/// <summary>Acts as currency to activate Pass Powers in the Entralink.</summary>
		public static readonly Items PASS_ORB			= new Items(616,	"PASS_ORB");
		/// <summary>Can only be used in Entree Forest, to catch Pokémon encountered in the Dream World.  Used in battle :   [Catches]{mechanic:catch} a wild Pokémon without fail.</summary>
		public static readonly Items DREAM_BALL			= new Items(617,	"DREAM_BALL");
		/// <summary>Used in battle :   Ends a wild battle.  Cannot be used in trainer battles.</summary>
		public static readonly Items POKE_TOY			= new Items(618,	"POKE_TOY");
		/// <summary>Stores props for the Pokémon Musical.</summary>
		public static readonly Items PROP_CASE			= new Items(619,	"PROP_CASE");
		/// <summary>Only used as a plot point; this item is given to the player and taken away in the same cutscene.</summary>
		public static readonly Items DRAGON_SKULL			= new Items(620,	"DRAGON_SKULL");
		/// <summary>Cult vendor trash.</summary>
		public static readonly Items BALM_MUSHROOM			= new Items(621,	"BALM_MUSHROOM");
		/// <summary>Cult vendor trash.</summary>
		public static readonly Items BIG_NUGGET			= new Items(622,	"BIG_NUGGET");
		/// <summary>Cult vendor trash.</summary>
		public static readonly Items PEARL_STRING			= new Items(623,	"PEARL_STRING");
		/// <summary>Cult vendor trash.</summary>
		public static readonly Items COMET_SHARD			= new Items(624,	"COMET_SHARD");
		/// <summary>Cult vendor trash.</summary>
		public static readonly Items RELIC_COPPER			= new Items(625,	"RELIC_COPPER");
		/// <summary>Cult vendor trash.</summary>
		public static readonly Items RELIC_SILVER			= new Items(626,	"RELIC_SILVER");
		/// <summary>Cult vendor trash.</summary>
		public static readonly Items RELIC_GOLD			= new Items(627,	"RELIC_GOLD");
		/// <summary>Cult vendor trash.</summary>
		public static readonly Items RELIC_VASE			= new Items(628,	"RELIC_VASE");
		/// <summary>Cult vendor trash.</summary>
		public static readonly Items RELIC_BAND			= new Items(629,	"RELIC_BAND");
		/// <summary>Cult vendor trash.</summary>
		public static readonly Items RELIC_STATUE			= new Items(630,	"RELIC_STATUE");
		/// <summary>Cult vendor trash.</summary>
		public static readonly Items RELIC_CROWN			= new Items(631,	"RELIC_CROWN");
		/// <summary>Used on a party Pokémon :   Cures any [status ailment]{mechanic:status-ailment} and [confusion]{mechanic:confusion}.</summary>
		public static readonly Items CASTELIACONE			= new Items(632,	"CASTELIACONE");
		/// <summary>Used on a party Pokémon in battle :   Raises the target's [critical hit]{mechanic:critical-hit} rate by two [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summary>
		public static readonly Items DIRE_HIT_2			= new Items(633,	"DIRE_HIT_2");
		/// <summary>Used on a party Pokémon in battle :   Raises the target's [Speed]{mechanic:speed} by two [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summary>
		public static readonly Items X_SPEED_2			= new Items(634,	"X_SPEED_2");
		/// <summary>Used on a party Pokémon in battle :   Raises the target's [Special Attack]{mechanic:special-attack} by two [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summary>
		public static readonly Items X_SP_ATK_2			= new Items(635,	"X_SP_ATK_2");
		/// <summary>Used on a party Pokémon in battle :   Raises the target's [Special Defense]{mechanic:special-defense} by two [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summary>
		public static readonly Items X_SP_DEF_2			= new Items(636,	"X_SP_DEF_2");
		/// <summary>Used on a party Pokémon in battle :   Raises the target's [Defense]{mechanic:defense} by two [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summary>
		public static readonly Items X_DEFENSE_2			= new Items(637,	"X_DEFENSE_2");
		/// <summary>Used on a party Pokémon in battle :   Raises the target's [Attack]{mechanic:attack} by two [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summary>
		public static readonly Items X_ATTACK_2			= new Items(638,	"X_ATTACK_2");
		/// <summary>Used on a party Pokémon in battle :   Raises the target's [accuracy]{mechanic:accuracy} by two [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summary>
		public static readonly Items X_ACCURACY_2			= new Items(639,	"X_ACCURACY_2");
		/// <summary>Used on a party Pokémon in battle :   Raises the target's [Speed]{mechanic:speed} by three [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summary>
		public static readonly Items X_SPEED_3			= new Items(640,	"X_SPEED_3");
		/// <summary>Used on a party Pokémon in battle :   Raises the target's [Special Attack]{mechanic:special-attack} by three [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summary>
		public static readonly Items X_SP_ATK_3			= new Items(641,	"X_SP_ATK_3");
		/// <summary>Used on a party Pokémon in battle :   Raises the target's [Special Defense]{mechanic:special-defense} by three [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summary>
		public static readonly Items X_SP_DEF_3			= new Items(642,	"X_SP_DEF_3");
		/// <summary>Used on a party Pokémon in battle :   Raises the target's [Defense]{mechanic:defense} by three [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summary>
		public static readonly Items X_DEFENSE_3			= new Items(643,	"X_DEFENSE_3");
		/// <summary>Used on a party Pokémon in battle :   Raises the target's [Attack]{mechanic:attack} by three [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summary>
		public static readonly Items X_ATTACK_3			= new Items(644,	"X_ATTACK_3");
		/// <summary>Used on a party Pokémon in battle :   Raises the target's [accuracy]{mechanic:accuracy} by three [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summary>
		public static readonly Items X_ACCURACY_3			= new Items(645,	"X_ACCURACY_3");
		/// <summary>Used on a party Pokémon in battle :   Raises the target's [Speed]{mechanic:speed} by six [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summary>
		public static readonly Items X_SPEED_6			= new Items(646,	"X_SPEED_6");
		/// <summary>Used on a party Pokémon in battle :   Raises the target's [Special Attack]{mechanic:special-attack} by six [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summary>
		public static readonly Items X_SP_ATK_6			= new Items(647,	"X_SP_ATK_6");
		/// <summary>Used on a party Pokémon in battle :   Raises the target's [Special Defense]{mechanic:special-defense} by six [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summary>
		public static readonly Items X_SP_DEF_6			= new Items(648,	"X_SP_DEF_6");
		/// <summary>Used on a party Pokémon in battle :   Raises the target's [Defense]{mechanic:defense} by six [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summary>
		public static readonly Items X_DEFENSE_6			= new Items(649,	"X_DEFENSE_6");
		/// <summary>Used on a party Pokémon in battle :   Raises the target's [Attack]{mechanic:attack} by six [stages]{mechanic:stage}.  This item can only be obtained or used via the Wonder Launcher.</summary>
		public static readonly Items X_ATTACK_6			= new Items(650,	"X_ATTACK_6");
		/// <summary>Used on a party Pokémon in battle :   Raises the target's [accuracy]{mechanic:accuracy} by six [stages]{mechanic:stage}.  This item can only be obtained or used via the Wonder Launcher.</summary>
		public static readonly Items X_ACCURACY_6			= new Items(651,	"X_ACCURACY_6");
		/// <summary>Used on a party Pokémon in battle :   Selects another friendly Pokémon at random.  If that Pokémon's ability is normally activated by some condition—i.e., is not continuous and passive—its effect is forcibly activated.  This item can only be obtained or used via the Wonder Launcher.</summary>
		public static readonly Items ABILITY_URGE			= new Items(652,	"ABILITY_URGE");
		/// <summary>Used on a party Pokémon in battle :   Selects another friendly Pokémon at random.  If that Pokémon is holding an item, that item is removed for the duration of the battle.  This item can only be obtained or used via the Wonder Launcher.</summary>
		public static readonly Items ITEM_DROP			= new Items(653,	"ITEM_DROP");
		/// <summary>Used on a party Pokémon in battle :   Selects another friendly Pokémon at random.  If that Pokémon is holding an item normally activated by some condition—i.e., not continuous and passive—its effect is forcibly activated.  This item can only be obtained or used via the Wonder Launcher.</summary>
		public static readonly Items ITEM_URGE			= new Items(654,	"ITEM_URGE");
		/// <summary>Used on a party Pokémon in battle :   Selects another friendly Pokémon at random.  Removes all of that Pokémon's stat changes.  This item can only be obtained or used via the Wonder Launcher.</summary>
		public static readonly Items RESET_URGE			= new Items(655,	"RESET_URGE");
		/// <summary>Used on a party Pokémon in battle :   Raises the target's [critical hit]{mechanic:critical-hit} rate by three [stages]{mechanic:stage}. This item can only be obtained or used via the Wonder Launcher.</summary>
		public static readonly Items DIRE_HIT_3			= new Items(656,	"DIRE_HIT_3");
		/// <summary>Summons []{pokemon:reshiram} for the final battle against N.</summary>
		public static readonly Items LIGHT_STONE			= new Items(657,	"LIGHT_STONE");
		/// <summary>Summons []{pokemon:zekrom} for the final battle against N.</summary>
		public static readonly Items DARK_STONE			= new Items(658,	"DARK_STONE");
		/*// <summary>Teaches []{move:wild-charge} to a compatible Pokémon.</summary>
		public static readonly Items TM93			= new Items(659,	"TM93");
		/// <summary>Teaches []{move:rock-smash} to a compatible Pokémon.</summary>
		public static readonly Items TM94			= new Items(660,	"TM94");
		/// <summary>Teaches []{move:snarl} to a compatible Pokémon.</summary>
		public static readonly Items TM95			= new Items(661,	"TM95");*/
		/// <summary>Makes four-way video calls.  Used for plot advancement in-game, but also works with other players via the C-Gear.</summary>
		public static readonly Items XTRANSCEIVER			= new Items(662,	"XTRANSCEIVER");
		/// <summary>Unknown.  Currently unused.</summary>
		public static readonly Items GOD_STONE			= new Items(663,	"GOD_STONE");
		/// <summary>Give to the []{pokemon:wingull} on []{location:unova-route-13}, along with []{item:gram-2} and []{item:gram-3}, to receive []{item:tm89}.</summary>
		public static readonly Items GRAM_1			= new Items(664,	"GRAM_1");
		/// <summary>Give to the []{pokemon:wingull} on []{location:unova-route-13}, along with []{item:gram-1} and []{item:gram-3}, to receive []{item:tm89}.</summary>
		public static readonly Items GRAM_2			= new Items(665,	"GRAM_2");
		/// <summary>Give to the []{pokemon:wingull} on []{location:unova-route-13}, along with []{item:gram-1} and []{item:gram-2}, to receive []{item:tm89}.</summary>
		public static readonly Items GRAM_3			= new Items(666,	"GRAM_3");
		/// <summary>Held :   When the holder uses a damaging []{type:dragon}-type move, the move has 1.5× power and this item is consumed.</summary>
		public static readonly Items DRAGON_GEM			= new Items(668,	"DRAGON_GEM");
		/// <summary>Held :   When the holder uses a damaging []{type:normal}-type move, the move has 1.5× power and this item is consumed.</summary>
		public static readonly Items NORMAL_GEM			= new Items(669,	"NORMAL_GEM");
		/// <summary>Holds medals recieved in the medal rally.</summary>
		public static readonly Items MEDAL_BOX			= new Items(670,	"MEDAL_BOX");
		/// <summary>Fuses Kyurem with Reshiram or Zekrom, or splits them apart again.</summary>
		public static readonly Items DNA_SPLICERS			= new Items(671,	"DNA_SPLICERS");
		/// <summary>Grants access to the Nature Preserve.</summary>
		public static readonly Items PERMIT			= new Items(673,	"PERMIT");
		/// <summary>Doubles the chance of two Pokémon producing an egg at the daycare every 255 steps.</summary>
		public static readonly Items OVAL_CHARM			= new Items(674,	"OVAL_CHARM");
		/// <summary>Raises the chance of finding a shiny Pokémon.</summary>
		public static readonly Items SHINY_CHARM			= new Items(675,	"SHINY_CHARM");
		/// <summary>Required to progress in the Plasma Frigate.</summary>
		public static readonly Items PLASMA_CARD			= new Items(676,	"PLASMA_CARD");
		/// <summary>Appears in the Café Warehouse on Sunday; return to the customer with a Patrat on Thursday.</summary>
		public static readonly Items GRUBBY_HANKY			= new Items(677,	"GRUBBY_HANKY");
		/// <summary>Wakes up the Crustle blocking the way in Seaside Cave.</summary>
		public static readonly Items COLRESS_MACHINE			= new Items(678,	"COLRESS_MACHINE");
		/// <summary>Returned to Curtis or Yancy as part of a sidequest.</summary>
		public static readonly Items DROPPED_ITEM			= new Items(679,	"DROPPED_ITEM");
		/// <summary>Switches Tornadus, Thundurus, and Landorus between Incarnate and Therian Forme.</summary>
		public static readonly Items REVEAL_GLASS			= new Items(681,	"REVEAL_GLASS");
		/// <summary>An item to be held by a Pokémon. Attack and Sp. Atk sharply increase if the holder is hit with a move it’s weak to.</summary>
		public static readonly Items WEAKNESS_POLICY			= new Items(682,	"WEAKNESS_POLICY");
		/// <summary>An item to be held by a Pokémon. This offensive vest raises Sp. Def but prevents the use of status moves.</summary>
		public static readonly Items ASSAULT_VEST			= new Items(683,	"ASSAULT_VEST");
		/// <summary>An item to be held by a Pokémon. It is a stone tablet that boosts the power of Fairy-type moves.</summary>
		public static readonly Items PIXIE_PLATE			= new Items(684,	"PIXIE_PLATE");
		/// <summary>A capsule that allows a Pokémon with two Abilities to switch between these Abilities when it is used.</summary>
		public static readonly Items ABILITY_CAPSULE			= new Items(685,	"ABILITY_CAPSULE");
		/// <summary>A soft and sweet treat made of fluffy, puffy, whipped and whirled cream. It’s loved by a certain Pokémon.</summary>
		public static readonly Items WHIPPED_DREAM			= new Items(686,	"WHIPPED_DREAM");
		/// <summary>A sachet filled with fragrant perfumes that are just slightly too overwhelming. Yet it’s loved by a certain Pokémon.</summary>
		public static readonly Items SACHET			= new Items(687,	"SACHET");
		/// <summary>An item to be held by a Pokémon. It boosts Sp. Def if hit with a Water- type attack. It can only be used once.</summary>
		public static readonly Items LUMINOUS_MOSS			= new Items(688,	"LUMINOUS_MOSS");
		/// <summary>An item to be held by a Pokémon. It boosts Attack if hit with an Ice-type attack. It can only be used once.</summary>
		public static readonly Items SNOWBALL			= new Items(689,	"SNOWBALL");
		/// <summary>An item to be held by a Pokémon. These goggles protect the holder from both weather-related damage and powder.</summary>
		public static readonly Items SAFETY_GOGGLES			= new Items(690,	"SAFETY_GOGGLES");
		/// <summary>Mulch to be used in a Berry field. It increases the Berry harvest without the need for particularly diligent care.</summary>
		public static readonly Items RICH_MULCH			= new Items(691,	"RICH_MULCH");
		/// <summary>Mulch to be used in a Berry field. It causes strange, sudden mutations based on the combination of Berries.</summary>
		public static readonly Items SURPRISE_MULCH			= new Items(692,	"SURPRISE_MULCH");
		/// <summary>Mulch to be used in a Berry field. It increases the Berry harvest that can be grown by diligent watering.</summary>
		public static readonly Items BOOST_MULCH			= new Items(693,	"BOOST_MULCH");
		/// <summary>Mulch to be used in a Berry field. An amazing Mulch with the effects of Rich, Surprise, and Boost Mulch.</summary>
		public static readonly Items AMAZE_MULCH			= new Items(694,	"AMAZE_MULCH");
		/// <summary>One variety of the mysterious Mega Stones. Have Gengar hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		public static readonly Items GENGARITE			= new Items(695,	"GENGARITE");
		/// <summary>One variety of the mysterious Mega Stones. Have Gardevoir hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		public static readonly Items GARDEVOIRITE			= new Items(696,	"GARDEVOIRITE");
		/// <summary>One variety of the mysterious Mega Stones. Have Ampharos hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		public static readonly Items AMPHAROSITE			= new Items(697,	"AMPHAROSITE");
		/// <summary>One variety of the mysterious Mega Stones. Have Venusaur hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		public static readonly Items VENUSAURITE			= new Items(698,	"VENUSAURITE");
		/// <summary>One variety of the mysterious Mega Stones. Have Charizard hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		public static readonly Items CHARIZARDITE_X			= new Items(699,	"CHARIZARDITE_X");
		/// <summary>One variety of the mysterious Mega Stones. Have Blastoise hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		public static readonly Items BLASTOISINITE			= new Items(700,	"BLASTOISINITE");
		/// <summary>One variety of the mysterious Mega Stones. Have Mewtwo hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		public static readonly Items MEWTWONITE_X			= new Items(701,	"MEWTWONITE_X");
		/// <summary>One variety of the mysterious Mega Stones. Have Mewtwo hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		public static readonly Items MEWTWONITE_Y			= new Items(702,	"MEWTWONITE_Y");
		/// <summary>One variety of the mysterious Mega Stones. Have Blaziken hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		public static readonly Items BLAZIKENITE			= new Items(703,	"BLAZIKENITE");
		/// <summary>One variety of the mysterious Mega Stones. Have Medicham hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		public static readonly Items MEDICHAMITE			= new Items(704,	"MEDICHAMITE");
		/// <summary>One variety of the mysterious Mega Stones. Have Houndoom hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		public static readonly Items HOUNDOOMINITE			= new Items(705,	"HOUNDOOMINITE");
		/// <summary>One variety of the mysterious Mega Stones. Have Aggron hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		public static readonly Items AGGRONITE			= new Items(706,	"AGGRONITE");
		/// <summary>One variety of the mysterious Mega Stones. Have Banette hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		public static readonly Items BANETTITE			= new Items(707,	"BANETTITE");
		/// <summary>One variety of the mysterious Mega Stones. Have Tyranitar hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		public static readonly Items TYRANITARITE			= new Items(708,	"TYRANITARITE");
		/// <summary>One variety of the mysterious Mega Stones. Have Scizor hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		public static readonly Items SCIZORITE			= new Items(709,	"SCIZORITE");
		/// <summary>One variety of the mysterious Mega Stones. Have Pinsir hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		public static readonly Items PINSIRITE			= new Items(710,	"PINSIRITE");
		/// <summary>One variety of the mysterious Mega Stones. Have Aerodactyl hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		public static readonly Items AERODACTYLITE			= new Items(711,	"AERODACTYLITE");
		/// <summary>One variety of the mysterious Mega Stones. Have Lucario hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		public static readonly Items LUCARIONITE			= new Items(712,	"LUCARIONITE");
		/// <summary>One variety of the mysterious Mega Stones. Have Abomasnow hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		public static readonly Items ABOMASITE			= new Items(713,	"ABOMASITE");
		/// <summary>One variety of the mysterious Mega Stones. Have Kangaskhan hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		public static readonly Items KANGASKHANITE			= new Items(714,	"KANGASKHANITE");
		/// <summary>One variety of the mysterious Mega Stones. Have Gyarados hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		public static readonly Items GYARADOSITE			= new Items(715,	"GYARADOSITE");
		/// <summary>One variety of the mysterious Mega Stones. Have Absol hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		public static readonly Items ABSOLITE			= new Items(716,	"ABSOLITE");
		/// <summary>One variety of the mysterious Mega Stones. Have Charizard hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		public static readonly Items CHARIZARDITE_Y			= new Items(717,	"CHARIZARDITE_Y");
		/// <summary>One variety of the mysterious Mega Stones. Have Alakazam hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		public static readonly Items ALAKAZITE			= new Items(718,	"ALAKAZITE");
		/// <summary>One variety of the mysterious Mega Stones. Have Heracross hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		public static readonly Items HERACRONITE			= new Items(719,	"HERACRONITE");
		/// <summary>One variety of the mysterious Mega Stones. Have Mawile hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		public static readonly Items MAWILITE			= new Items(720,	"MAWILITE");
		/// <summary>One variety of the mysterious Mega Stones. Have Manectric hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		public static readonly Items MANECTITE			= new Items(721,	"MANECTITE");
		/// <summary>One variety of the mysterious Mega Stones. Have Garchomp hold it, and this stone will enable it to Mega Evolve during battle.</summary>
		public static readonly Items GARCHOMPITE			= new Items(722,	"GARCHOMPITE");
		/// <summary>If held by a Pokémon, this Berry will lessen the damage taken from one supereffective Fairy-type attack.</summary>
		public static readonly Items ROSELI_BERRY			= new Items(723,	"ROSELI_BERRY");
		/// <summary>If held by a Pokémon, this Berry will increase the holder’s Defense if it’s hit with a physical move.</summary>
		public static readonly Items KEE_BERRY			= new Items(724,	"KEE_BERRY");
		/// <summary>If held by a Pokémon, this Berry will increase the holder’s Sp. Def if it’s hit with a special move.</summary>
		public static readonly Items MARANGA_BERRY			= new Items(725,	"MARANGA_BERRY");
		/// <summary>This special coupon allows you to buy items at a discount when you are shopping at a boutique.</summary>
		public static readonly Items DISCOUNT_COUPON			= new Items(726,	"DISCOUNT_COUPON");
		/// <summary>An ornament depicting a Pokémon that is venerated as a protector in some region far from Kalos.</summary>
		public static readonly Items STRANGE_SOUVENIR			= new Items(727,	"STRANGE_SOUVENIR");
		/// <summary>A popular treat in Lumiose City. It can be used once to heal all the status conditions of a Pokémon.</summary>
		public static readonly Items LUMIOSE_GALETTE			= new Items(728,	"LUMIOSE_GALETTE");
		/// <summary>A fossil from a prehistoric Pokémon that once lived on the land. It looks as if it could be a piece of a large jaw.</summary>
		public static readonly Items JAW_FOSSIL			= new Items(729,	"JAW_FOSSIL");
		/// <summary>A fossil from a prehistoric Pokémon that once lived on the land. It looks like the impression from a skin sail.</summary>
		public static readonly Items SAIL_FOSSIL			= new Items(730,	"SAIL_FOSSIL");
		/// <summary>A gem with an essence of the fey. When held, it strengthens the power of a Fairy-type move one time.</summary>
		public static readonly Items FAIRY_GEM			= new Items(731,	"FAIRY_GEM");
		/// <summary>This book contains all the points a new Trainer needs to know on a journey. It was handmade by a kind friend.</summary>
		public static readonly Items ADVENTURE_RULES			= new Items(732,	"ADVENTURE_RULES");
		/// <summary>A card key that activates the elevator in Lysandre Labs. It is emblazoned with Team Flare’s logo.</summary>
		public static readonly Items ELEVATOR_KEY			= new Items(733,	"ELEVATOR_KEY");
		/// <summary>A device that allows users to receive and view hologram clips at any time. It is also used to chat with others.</summary>
		public static readonly Items HOLO_CASTER			= new Items(734,	"HOLO_CASTER");
		/// <summary>A precious symbol that is awarded only to an individual who has done great things for the Kalos region.</summary>
		public static readonly Items HONOR_OF_KALOS			= new Items(735,	"HONOR_OF_KALOS");
		/// <summary>A rather curious stone that might appear to be valuable to some. It’s all in the eye of the beholder.</summary>
		public static readonly Items INTRIGUING_STONE			= new Items(736,	"INTRIGUING_STONE");
		/// <summary>A rather chic-looking case for carrying contact lenses.</summary>
		public static readonly Items LENS_CASE			= new Items(737,	"LENS_CASE");
		/// <summary>A ticket that was handmade by Looker. It’s decorated with a liberal amount of glittery paint.</summary>
		public static readonly Items LOOKER_TICKET			= new Items(738,	"LOOKER_TICKET");
		/// <summary>This ring contains an untold power that somehow enables Pokémon carrying Mega Stones to Mega Evolve in battle.</summary>
		public static readonly Items MEGA_RING			= new Items(739,	"MEGA_RING");
		/// <summary>This pass serves as an ID card for gaining access to the power plant that lies along Route 13.</summary>
		public static readonly Items POWER_PLANT_PASS			= new Items(740,	"POWER_PLANT_PASS");
		/// <summary>A letter that Professor Sycamore wrote to your mother. A faint but pleasant perfume seems to cling to the paper.</summary>
		public static readonly Items PROFS_LETTER			= new Items(741,	"PROFS_LETTER");
		/// <summary>Attaches roller skates to the bottom of your shoes, allowing you to glide quickly around and perform tricks.</summary>
		public static readonly Items ROLLER_SKATES			= new Items(742,	"ROLLER_SKATES");
		/// <summary>A watering can shaped like a Lotad. It helps promote the healthy growth of any Berries planted in good, soft soil.</summary>
		public static readonly Items SPRINKLOTAD			= new Items(743,	"SPRINKLOTAD");
		/// <summary>A commuter pass that allows the holder to ride the TMV between Lumiose City and Kiloude City at any time.</summary>
		public static readonly Items TMV_PASS			= new Items(744,	"TMV_PASS");
		/*// <summary>An attack that makes use of nature’s power. Its effects vary depending on the user’s environment.</summary>
		public static readonly Items TM96			= new Items(745,	"TM96");
		/// <summary>The user releases a horrible aura imbued with dark thoughts. This may also make the target flinch.</summary>
		public static readonly Items TM97			= new Items(746,	"TM97");
		/// <summary>Striking opponents over and over makes the user’s fists harder. Hitting a target raises the Attack stat.</summary>
		public static readonly Items TM98			= new Items(747,	"TM98");
		/// <summary>The user damages opposing Pokémon by emitting a powerful flash.</summary>
		public static readonly Items TM99			= new Items(748,	"TM99");
		/// <summary>The user tells the target a secret, and the target loses its ability to concentrate. This lowers the target’s Sp. Atk stat.</summary>
		public static readonly Items TM100			= new Items(749,	"TM100");*/
		/// <summary>Held: Allows Latias to Mega Evolve into Mega Latias. : 	Held: Allows Latias to Mega Evolve into Mega Latias.</summary>
		public static readonly Items LATIASITE			= new Items(760,	"LATIASITE");
		/// <summary>Held: Allows Latios to Mega Evolve into Mega Latios. : 	Held: Allows Latios to Mega Evolve into Mega Latios.</summary>
		public static readonly Items LATIOSITE			= new Items(761,	"LATIOSITE");
		/// <summary>Unknown. : 	Unknown.</summary>
		public static readonly Items COMMON_STONE			= new Items(762,	"COMMON_STONE");
		/// <summary>Allows the player to change their lipstick color. : 	Allows the player to change their lipstick color.</summary>
		public static readonly Items MAKEUP_BAG			= new Items(763,	"MAKEUP_BAG");
		/// <summary>Unobtainable, but allows the player to change clothes anywhere. : 	Unobtainable, but allows the player to change clothes anywhere.</summary>
		public static readonly Items TRAVEL_TRUNK			= new Items(764,	"TRAVEL_TRUNK");
		/// <summary>Cures any major status ailment and confusion. : 	Cures any major status ailment and confusion.</summary>
		public static readonly Items SHALOUR_SABLE			= new Items(765,	"SHALOUR_SABLE");
		/// <summary>Unused.  This appears as the girlplayer's Mega Bracelet in Pokémon Contests, but it cannot actually be obtained. : 	Unused Key Stone.</summary>
		public static readonly Items MEGA_CHARM			= new Items(768,	"MEGA_CHARM");
		/// <summary>Unused.  This is Korrina's Key Stone in X and Y, but it cannot be obtained by the player. : 	Unused NPC Key Stone.</summary>
		public static readonly Items MEGA_GLOVE			= new Items(769,	"MEGA_GLOVE");
		/// <summary>Allows Captain Stern to set out on his expedition. : 	Allows Captain Stern to set out on his expedition.</summary>
		public static readonly Items DEVON_PARTS			= new Items(770,	"DEVON_PARTS");
		/// <summary>Creates and stores Pokéblocks. : 	Creates and stores Pokéblocks.</summary>
		public static readonly Items POKEBLOCK_KIT			= new Items(772,	"POKEBLOCK_KIT");
		/// <summary>Unlocks the door to Room 1 in Sea Mauville. : 	Unlocks the door to Room 1 in Sea Mauville.</summary>
		public static readonly Items KEY_TO_ROOM_1			= new Items(773,	"KEY_TO_ROOM_1");
		/// <summary>Unlocks the door to Room 2 in Sea Mauville. : 	Unlocks the door to Room 2 in Sea Mauville.</summary>
		public static readonly Items KEY_TO_ROOM_2			= new Items(774,	"KEY_TO_ROOM_2");
		/// <summary>Unlocks the door to Room 4 in Sea Mauville. : 	Unlocks the door to Room 4 in Sea Mauville.</summary>
		public static readonly Items KEY_TO_ROOM_4			= new Items(775,	"KEY_TO_ROOM_4");
		/// <summary>Unlocks the door to Room 6 in Sea Mauville. : 	Unlocks the door to Room 6 in Sea Mauville.</summary>
		public static readonly Items KEY_TO_ROOM_6			= new Items(776,	"KEY_TO_ROOM_6");
		/// <summary>Worn by the player while underwater via Dive in Omega Ruby and Alpha Sapphire. : 	Worn by the player while underwater.</summary>
		public static readonly Items DEVON_SCUBA_GEAR			= new Items(779,	"DEVON_SCUBA_GEAR");
		/// <summary>Worn during Pokémon Contests. : 	Worn during Pokémon Contests.</summary>
		public static readonly Items CONTEST_COSTUME__JACKET			= new Items(780,	"CONTEST_COSTUME__JACKET");
		/// <summary>Allows the player to ride Groudon in the Cave of Origin. : 	Allows the player to ride Groudon in the Cave of Origin.</summary>
		public static readonly Items MAGMA_SUIT			= new Items(782,	"MAGMA_SUIT");
		/// <summary>Allows the player to ride Kyogre in the Cave of Origin. : 	Allows the player to ride Kyogre in the Cave of Origin.</summary>
		public static readonly Items AQUA_SUIT			= new Items(783,	"AQUA_SUIT");
		/// <summary>Allows the player and their mother to see the star show in the Mossdeep Space Center. : 	Allows the player and their mother to see the star show in the Mossdeep Space Center.</summary>
		public static readonly Items PAIR_OF_TICKETS			= new Items(784,	"PAIR_OF_TICKETS");
		/// <summary>Allows the player's Pokémon to Mega Evolve. : 	Allows the player's Pokémon to Mega Evolve.</summary>
		public static readonly Items MEGA_BRACELET			= new Items(785,	"MEGA_BRACELET");
		/// <summary>Unused.  This is Wally's Key Stone in Omega Ruby and Alpha Sapphire, but it cannot be obtained by the player. : 	Unused NPC Key Stone.</summary>
		public static readonly Items MEGA_PENDANT			= new Items(786,	"MEGA_PENDANT");
		/// <summary>Unused.  This is Maxie's Key Stone in Omega Ruby and Alpha Sapphire, but it cannot be obtained by the player. : 	Unused NPC Key Stone.</summary>
		public static readonly Items MEGA_GLASSES			= new Items(787,	"MEGA_GLASSES");
		/// <summary>Unused.  This is Archie's Key Stone in Omega Ruby and Alpha Sapphire, but it cannot be obtained by the player. : 	Unused NPC Key Stone.</summary>
		public static readonly Items MEGA_ANCHOR			= new Items(788,	"MEGA_ANCHOR");
		/// <summary>Unused.  This is Steven's Key Stone in Omega Ruby and Alpha Sapphire, but it cannot be obtained by the player. : 	Unused NPC Key Stone.</summary>
		public static readonly Items MEGA_STICKPIN			= new Items(789,	"MEGA_STICKPIN");
		/// <summary>Unused.  This is Lisia's Key Stone in Omega Ruby and Alpha Sapphire, but it cannot be obtained by the player. : 	Unused NPC Key Stone.</summary>
		public static readonly Items MEGA_TIARA			= new Items(790,	"MEGA_TIARA");
		/// <summary>Unused.  This is Zinnia's Key Stone in Omega Ruby and Alpha Sapphire, but it cannot be obtained by the player. : 	Unused NPC Key Stone.</summary>
		public static readonly Items MEGA_ANKLET			= new Items(791,	"MEGA_ANKLET");
		/// <summary>Held: Allows Swampert to Mega Evolve into Mega Swampert. : 	Held: Allows Swampert to Mega Evolve into Mega Swampert.</summary>
		public static readonly Items SWAMPERTITE			= new Items(793,	"SWAMPERTITE");
		/// <summary>Held: Allows Sceptile to Mega Evolve into Mega Sceptile. : 	Held: Allows Sceptile to Mega Evolve into Mega Sceptile.</summary>
		public static readonly Items SCEPTILITE			= new Items(794,	"SCEPTILITE");
		/// <summary>Held: Allows Sableye to Mega Evolve into Mega Sableye. : 	Held: Allows Sableye to Mega Evolve into Mega Sableye.</summary>
		public static readonly Items SABLENITE			= new Items(795,	"SABLENITE");
		/// <summary>Held: Allows Altaria to Mega Evolve into Mega Altaria. : 	Held: Allows Altaria to Mega Evolve into Mega Altaria.</summary>
		public static readonly Items ALTARIANITE			= new Items(796,	"ALTARIANITE");
		/// <summary>Held: Allows Gallade to Mega Evolve into Mega Gallade. : 	Held: Allows Gallade to Mega Evolve into Mega Gallade.</summary>
		public static readonly Items GALLADITE			= new Items(797,	"GALLADITE");
		/// <summary>Held: Allows Audino to Mega Evolve into Mega Audino. : 	Held: Allows Audino to Mega Evolve into Mega Audino.</summary>
		public static readonly Items AUDINITE			= new Items(798,	"AUDINITE");
		/// <summary>Held: Allows Metagross to Mega Evolve into Mega Metagross. : 	Held: Allows Metagross to Mega Evolve into Mega Metagross.</summary>
		public static readonly Items METAGROSSITE			= new Items(799,	"METAGROSSITE");
		/// <summary>Held: Allows Sharpedo to Mega Evolve into Mega Sharpedo. : 	Held: Allows Sharpedo to Mega Evolve into Mega Sharpedo.</summary>
		public static readonly Items SHARPEDONITE			= new Items(800,	"SHARPEDONITE");
		/// <summary>Held: Allows Slowbro to Mega Evolve into Mega Slowbro. : 	Held: Allows Slowbro to Mega Evolve into Mega Slowbro.</summary>
		public static readonly Items SLOWBRONITE			= new Items(801,	"SLOWBRONITE");
		/// <summary>Held: Allows Steelix to Mega Evolve into Mega Steelix. : 	Held: Allows Steelix to Mega Evolve into Mega Steelix.</summary>
		public static readonly Items STEELIXITE			= new Items(802,	"STEELIXITE");
		/// <summary>Held: Allows Pidgeot to Mega Evolve into Mega Pidgeot. : 	Held: Allows Pidgeot to Mega Evolve into Mega Pidgeot.</summary>
		public static readonly Items PIDGEOTITE			= new Items(803,	"PIDGEOTITE");
		/// <summary>Held: Allows Glalie to Mega Evolve into Mega Glalie. : 	Held: Allows Glalie to Mega Evolve into Mega Glalie.</summary>
		public static readonly Items GLALITITE			= new Items(804,	"GLALITITE");
		/// <summary>Held: Allows Diancie to Mega Evolve into Mega Diancie. : 	Held: Allows Diancie to Mega Evolve into Mega Diancie.</summary>
		public static readonly Items DIANCITE			= new Items(805,	"DIANCITE");
		/// <summary>Transforms Hoopa into its Unbound form for up to three days. : 	Transforms Hoopa into its Unbound form for up to three days.</summary>
		public static readonly Items PRISON_BOTTLE			= new Items(806,	"PRISON_BOTTLE");
		/// <summary>Unused.  This appears as the boy player's Mega Bracelet in Pokémon Contests, but it cannot actually be obtained. : 	Unused Key Stone.</summary>
		public static readonly Items MEGA_CUFF			= new Items(807,	"MEGA_CUFF");
		/// <summary>Held: Allows Camerupt to Mega Evolve into Mega Camerupt. : 	Held: Allows Camerupt to Mega Evolve into Mega Camerupt.</summary>
		public static readonly Items CAMERUPTITE			= new Items(808,	"CAMERUPTITE");
		/// <summary>Held: Allows Lopunny to Mega Evolve into Mega Lopunny. : 	Held: Allows Lopunny to Mega Evolve into Mega Lopunny.</summary>
		public static readonly Items LOPUNNITE			= new Items(809,	"LOPUNNITE");
		/// <summary>Held: Allows Salamence to Mega Evolve into Mega Salamence. : 	Held: Allows Salamence to Mega Evolve into Mega Salamence.</summary>
		public static readonly Items SALAMENCITE			= new Items(810,	"SALAMENCITE");
		/// <summary>Held: Allows Beedrill to Mega Evolve into Mega Beedrill. : 	Held: Allows Beedrill to Mega Evolve into Mega Beedrill.</summary>
		public static readonly Items BEEDRILLITE			= new Items(811,	"BEEDRILLITE");
		/// <summary>Allows the player's Pokémon to Mega Evolve. : 	Allows the player's Pokémon to Mega Evolve.</summary>
		public static readonly Items KEY_STONE			= new Items(814,	"KEY_STONE");
		/// <summary>Causes the Meteorite to transform to its final form, allowing Rayquaza to Mega Evolve. : 	Causes the Meteorite to transform to its final form, allowing Rayquaza to Mega Evolve.</summary>
		public static readonly Items METEORITE_SHARD			= new Items(815,	"METEORITE_SHARD");
		/// <summary>Summons Latias or Latios for a ride. : 	Summons Latias or Latios for a ride.</summary>
		public static readonly Items EON_FLUTE			= new Items(816,	"EON_FLUTE");
		/// <summary>Held: Allows a Pokémon to use the Z-move equivalents of its Normal moves. : 	Held: Allows a Pokémon to use the Z-move equivalents of its Normal moves.</summary>
		public static readonly Items NORMALIUM_Z__HELD			= new Items(817,	"NORMALIUM_Z__HELD");
		/// <summary>Held: Allows a Pokémon to use the Z-move equivalents of its Fire moves. : 	Held: Allows a Pokémon to use the Z-move equivalents of its Fire moves.</summary>
		public static readonly Items FIRIUM_Z__HELD			= new Items(818,	"FIRIUM_Z__HELD");
		/// <summary>Held: Allows a Pokémon to use the Z-move equivalents of its Water moves. : 	Held: Allows a Pokémon to use the Z-move equivalents of its Water moves.</summary>
		public static readonly Items WATERIUM_Z__HELD			= new Items(819,	"WATERIUM_Z__HELD");
		/// <summary>Held: Allows a Pokémon to use the Z-move equivalents of its Electric moves. : 	Held: Allows a Pokémon to use the Z-move equivalents of its Electric moves.</summary>
		public static readonly Items ELECTRIUM_Z__HELD			= new Items(820,	"ELECTRIUM_Z__HELD");
		/// <summary>Held: Allows a Pokémon to use the Z-move equivalents of its Grass moves. : 	Held: Allows a Pokémon to use the Z-move equivalents of its Grass moves.</summary>
		public static readonly Items GRASSIUM_Z__HELD			= new Items(821,	"GRASSIUM_Z__HELD");
		/// <summary>Held: Allows a Pokémon to use the Z-move equivalents of its Ice moves. : 	Held: Allows a Pokémon to use the Z-move equivalents of its Ice moves.</summary>
		public static readonly Items ICIUM_Z__HELD			= new Items(822,	"ICIUM_Z__HELD");
		/// <summary>Held: Allows a Pokémon to use the Z-move equivalents of its Fighting moves. : 	Held: Allows a Pokémon to use the Z-move equivalents of its Fighting moves.</summary>
		public static readonly Items FIGHTINIUM_Z__HELD			= new Items(823,	"FIGHTINIUM_Z__HELD");
		/// <summary>Held: Allows a Pokémon to use the Z-move equivalents of its Poison moves. : 	Held: Allows a Pokémon to use the Z-move equivalents of its Poison moves.</summary>
		public static readonly Items POISONIUM_Z__HELD			= new Items(824,	"POISONIUM_Z__HELD");
		/// <summary>Held: Allows a Pokémon to use the Z-move equivalents of its Ground moves. : 	Held: Allows a Pokémon to use the Z-move equivalents of its Ground moves.</summary>
		public static readonly Items GROUNDIUM_Z__HELD			= new Items(825,	"GROUNDIUM_Z__HELD");
		/// <summary>Held: Allows a Pokémon to use the Z-move equivalents of its Flying moves. : 	Held: Allows a Pokémon to use the Z-move equivalents of its Flying moves.</summary>
		public static readonly Items FLYINIUM_Z__HELD			= new Items(826,	"FLYINIUM_Z__HELD");
		/// <summary>Held: Allows a Pokémon to use the Z-move equivalents of its Psychic moves. : 	Held: Allows a Pokémon to use the Z-move equivalents of its Psychic moves.</summary>
		public static readonly Items PSYCHIUM_Z__HELD			= new Items(827,	"PSYCHIUM_Z__HELD");
		/// <summary>Held: Allows a Pokémon to use the Z-move equivalents of its Bug moves. : 	Held: Allows a Pokémon to use the Z-move equivalents of its Bug moves.</summary>
		public static readonly Items BUGINIUM_Z__HELD			= new Items(828,	"BUGINIUM_Z__HELD");
		/// <summary>Held: Allows a Pokémon to use the Z-move equivalents of its Rock moves. : 	Held: Allows a Pokémon to use the Z-move equivalents of its Rock moves.</summary>
		public static readonly Items ROCKIUM_Z__HELD			= new Items(829,	"ROCKIUM_Z__HELD");
		/// <summary>Held: Allows a Pokémon to use the Z-move equivalents of its Ghost moves. : 	Held: Allows a Pokémon to use the Z-move equivalents of its Ghost moves.</summary>
		public static readonly Items GHOSTIUM_Z__HELD			= new Items(830,	"GHOSTIUM_Z__HELD");
		/// <summary>Held: Allows a Pokémon to use the Z-move equivalents of its Dragon moves. : 	Held: Allows a Pokémon to use the Z-move equivalents of its Dragon moves.</summary>
		public static readonly Items DRAGONIUM_Z__HELD			= new Items(831,	"DRAGONIUM_Z__HELD");
		/// <summary>Held: Allows a Pokémon to use the Z-move equivalents of its Dark moves. : 	Held: Allows a Pokémon to use the Z-move equivalents of its Dark moves.</summary>
		public static readonly Items DARKINIUM_Z__HELD			= new Items(832,	"DARKINIUM_Z__HELD");
		/// <summary>Held: Allows a Pokémon to use the Z-move equivalents of its Steel moves. : 	Held: Allows a Pokémon to use the Z-move equivalents of its Steel moves.</summary>
		public static readonly Items STEELIUM_Z__HELD			= new Items(833,	"STEELIUM_Z__HELD");
		/// <summary>Held: Allows a Pokémon to use the Z-move equivalents of its Fairy moves. : 	Held: Allows a Pokémon to use the Z-move equivalents of its Fairy moves.</summary>
		public static readonly Items FAIRIUM_Z__HELD			= new Items(834,	"FAIRIUM_Z__HELD");
		/// <summary>Held: Allows []{pokemon:pikachu} to upgrade []{move:volt-tackle} into []{move:catastropika}. : 	Held: Allows Pikachu to upgrade Volt Tackle into Catastropika.</summary>
		public static readonly Items PIKANIUM_Z__HELD			= new Items(835,	"PIKANIUM_Z__HELD");
		/// <summary>Trade to Mr. Hyper to set one of a Pokémon's genes to 31. : 	Trade to Mr. Hyper to maximize one of a Pokémon's genes.</summary>
		public static readonly Items BOTTLE_CAP			= new Items(836,	"BOTTLE_CAP");
		/// <summary>Trade to Mr. Hyper to set all of a Pokémon's genes to 31. : 	Trade to Mr. Hyper to maximize all of a Pokémon's genes.</summary>
		public static readonly Items GOLD_BOTTLE_CAP			= new Items(837,	"GOLD_BOTTLE_CAP");
		/// <summary>Allows the player's Pokémon to use Z-moves. : 	Allows the player's Pokémon to use Z-moves.</summary>
		public static readonly Items Z_RING			= new Items(838,	"Z_RING");
		/// <summary>Held: Allows []{pokemon:decidueye} to upgrade []{move:spirit-shackle} into []{move:sinister-arrow-raid}. : 	Held: Allows Decidueye to upgrade Spirit Shackle into Sinister Arrow Raid.</summary>
		public static readonly Items DECIDIUM_Z__HELD			= new Items(839,	"DECIDIUM_Z__HELD");
		/// <summary>Held: Allows []{pokemon:incineroar} to upgrade []{move:darkest-lariat} into []{move:malicious-moonsault}. : 	Held: Allows Incineroar to upgrade Darkest Lariat into Malicious Moonsault.</summary>
		public static readonly Items INCINIUM_Z__HELD			= new Items(840,	"INCINIUM_Z__HELD");
		/// <summary>Held: Allows []{pokemon:primarina} to upgrade []{move:sparkling-aria} into []{move:oceanic-operetta}. : 	Held: Allows Primarina to upgrade Sparkling Aria into Oceanic Operetta.</summary>
		public static readonly Items PRIMARIUM_Z__HELD			= new Items(841,	"PRIMARIUM_Z__HELD");
		/// <summary>Held: Allows []{pokemon:tapu-koko}, []{pokemon:tapu-lele}, []{pokemon:tapu-bulu}, and []{pokemon:tapu-fini} to upgrade []{move:natures-madness} into []{move:guardian-of-alola}. : 	Held: Allows the Tapus to upgrade Nature's Madness into Guardian of Alola.</summary>
		public static readonly Items TAPUNIUM_Z__HELD			= new Items(842,	"TAPUNIUM_Z__HELD");
		/// <summary>Held: Allows []{pokemon:marshadow} to upgrade []{move:spectral-thief} into []{move:soul-stealing-7-star-strike}. : 	Held: Allows Marshadow to upgrade Spectral Thief into Soul-Stealing 7 Star Strike.</summary>
		public static readonly Items MARSHADIUM_Z__HELD			= new Items(843,	"MARSHADIUM_Z__HELD");
		/// <summary>Held: Allows Alola []{pokemon:raichu} to upgrade []{move:thunderbolt} into []{move:stoked-sparksurfer}. : 	Held: Allows Alola Raichu to upgrade Thunderbolt into Stoked Sparksurfer.</summary>
		public static readonly Items ALORAICHIUM_Z__HELD			= new Items(844,	"ALORAICHIUM_Z__HELD");
		/// <summary>Held: Allows []{pokemon:snorlax} to upgrade []{move:giga-impact} into []{move:pulverizing-pancake}. : 	Held: Allows Snorlax to upgrade Giga Impact into Pulverizing Pancake.</summary>
		public static readonly Items SNORLIUM_Z__HELD			= new Items(845,	"SNORLIUM_Z__HELD");
		/// <summary>Held: Allows []{pokemon:eevee} to upgrade []{move:last-resort} into []{move:extreme-evoboost}. : 	Held: Allows Eevee to upgrade Last Resort into Extreme Evoboost.</summary>
		public static readonly Items EEVIUM_Z__HELD			= new Items(846,	"EEVIUM_Z__HELD");
		/// <summary>Held: Allows []{pokemon:mew} to upgrade []{move:psychic} into []{move:genesis-supernova}. : 	Held: Allows Mew to upgrade Psychic into Genesis Supernova.</summary>
		public static readonly Items MEWNIUM_Z__HELD			= new Items(847,	"MEWNIUM_Z__HELD");
		/// <summary>Held: Allows cap-wearing []{pokemon:pikachu} to upgrade []{move:thunderbolt} into []{move:10-000-000-volt-thunderbolt}. : 	Held: Allows cap-wearing Pikachu to upgrade Thunderbolt into 10,000,000 Volt Thunderbolt.</summary>
		public static readonly Items PIKASHUNIUM_Z__HELD			= new Items(877,	"PIKASHUNIUM_Z__HELD");
		/// <summary>Holds ingredients during Mallow's trial. : 	Holds ingredients during Mallow's trial.</summary>
		public static readonly Items FORAGE_BAG			= new Items(878,	"FORAGE_BAG");
		/// <summary>Allows the player to fish for Pokémon. : 	Allows the player to fish for Pokémon.</summary>
		public static readonly Items FISHING_ROD			= new Items(879,	"FISHING_ROD");
		/// <summary>Lost by Professor Kukui. : 	Lost by Professor Kukui.</summary>
		public static readonly Items PROFESSORS_MASK			= new Items(880,	"PROFESSORS_MASK");
		/// <summary>Hosts a mission in Festival Plaza. : 	Hosts a mission in Festival Plaza.</summary>
		public static readonly Items FESTIVAL_TICKET			= new Items(881,	"FESTIVAL_TICKET");
		/// <summary>Required to obtain a Z-Ring. : 	Required to obtain a Z-Ring.</summary>
		public static readonly Items SPARKLING_STONE			= new Items(882,	"SPARKLING_STONE");
		/// <summary>Makes wild Pokémon more likely to summon allies.  Held: increases the holder's Speed by one stage when affected by Intimidate. : 	Makes wild Pokémon more likely to summon allies.  Held: increases the holder's Speed by one stage when affected by Intimidate.</summary>
		public static readonly Items ADRENALINE_ORB			= new Items(883,	"ADRENALINE_ORB");
		/// <summary>Contains collected Zygarde cells/cores.  Can teach Zygarde moves. : 	Contains collected Zygarde cells/cores.  Can teach Zygarde moves.</summary>
		public static readonly Items ZYGARDE_CUBE			= new Items(884,	"ZYGARDE_CUBE");
		/// <summary>Used on a party Pokémon :   Evolves an Alola []{pokemon:sandshrew} into Alola[]{pokemon:sandslash} or an Alola[]{pokemon:vulpix} into Alola[]{pokemon:ninetales}. : 	Evolves an Alola Sandshrew into Alola Sandslash or an Alola Vulpix into Alola Ninetales.</summary>
		public static readonly Items ICE_STONE			= new Items(885,	"ICE_STONE");
		/// <summary>Allows the player to summon a Ride Pokémon.  Unused, as this can be done simply by pressing Y. : 	Allows the player to summon a Ride Pokémon.</summary>
		public static readonly Items RIDE_PAGER			= new Items(886,	"RIDE_PAGER");
		/// <summary>Used in battle :   Attempts to [catch]{mechanic:catch} a wild Pokémon.If the wild Pokémon is an Ultra Beast, this ball has a catch rate of 5Ã—.  Otherwise, it has a catch rate of 0.1Ã—. If used in a trainer battle, nothing happens and the ball is lost. : 	Tries to catch a wild Pokémon.Success rate is 5Ã— for Ultra Beasts and 0.1Ã— for all other Pokémon.</ summary >
		public static readonly Items BEAST_BALL			= new Items(887,	"BEAST_BALL");
		/// <summary>Cures major status ailments and confusion. : 	Cures major status ailments and confusion.</summary>
		public static readonly Items BIG_MALASADA			= new Items(888,	"BIG_MALASADA");
		/// <summary>Changes Oricorio to Baile Style.  Single-use and cannot be used in battle. : 	Changes Oricorio to Baile Style.</summary>
		public static readonly Items RED_NECTAR			= new Items(889,	"RED_NECTAR");
		/// <summary>Changes Oricorio to Pom-Pom Style.  Single-use and cannot be used in battle. : 	Changes Oricorio to Pom-Pom Style.</summary>
		public static readonly Items YELLOW_NECTAR			= new Items(890,	"YELLOW_NECTAR");
		/// <summary>Changes Oricorio to Paâ€™u Style.  Single-use and cannot be used in battle. : 	Changes Oricorio to Paâ€™u Style.</summary>
		public static readonly Items PINK_NECTAR			= new Items(891,	"PINK_NECTAR");
		/// <summary>Changes Oricorio to Sensu Style.  Single-use and cannot be used in battle. : 	Changes Oricorio to Sensu Style.</summary>
		public static readonly Items PURPLE_NECTAR			= new Items(892,	"PURPLE_NECTAR");
		/// <summary>Evolves Nebby into Solgaleo when used at the Altar of the Sunne. : 	Evolves Nebby into Solgaleo when used at the Altar of the Sunne.</summary>
		public static readonly Items SUN_FLUTE			= new Items(893,	"SUN_FLUTE");
		/// <summary>Evolves Nebby into Lunala when used at the Altar of the Moone. : 	Evolves Nebby into Lunala when used at the Altar of the Moone.</summary>
		public static readonly Items MOON_FLUTE			= new Items(894,	"MOON_FLUTE");
		/// <summary>Unlocks Looker's motel room on Route 8. : 	Unlocks Looker's motel room on Route 8.</summary>
		public static readonly Items ENIGMATIC_CARD			= new Items(895,	"ENIGMATIC_CARD");
		/// <summary>Held: When the holder changes the Terrain (whether by move or ability), it will last  8 turns instead of 5. : 	Held: Extends the holder's Terrain effects to 8 turns.</summary>
		public static readonly Items TERRAIN_EXTENDER			= new Items(896,	"TERRAIN_EXTENDER");
		/// <summary>Held: Prevents side effects of contact moves used on the holder. : 	Held: Prevents side effects of contact moves used on the holder.</summary>
		public static readonly Items PROTECTIVE_PADS			= new Items(897,	"PROTECTIVE_PADS");
		/// <summary>Held: If the holder enters battle during Electric Terrain, or if Electric Terrain is activated while the holder is in battle, this item is consumed and the holder's Defense raises by one stage. : 	Held: Consumed on Electric Terrain and raises the holder's Defense by one stage.</summary>
		public static readonly Items ELECTRIC_SEED			= new Items(898,	"ELECTRIC_SEED");
		/// <summary>Held: If the holder enters battle during Psychic Terrain, or if Psychic Terrain is activated while the holder is in battle, this item is consumed and the holder's Special Defense raises by one stage. : 	Held: Consumed on Psychic Terrain and raises the holder's Special Defense by one stage.</summary>
		public static readonly Items PSYCHIC_SEED			= new Items(899,	"PSYCHIC_SEED");
		/// <summary>Held: If the holder enters battle during Misty Terrain, or if Misty Terrain is activated while the holder is in battle, this item is consumed and the holder's Special Defense raises by one stage. : 	Held: Consumed on Misty Terrain and raises the holder's Special Defense by one stage.</summary>
		public static readonly Items MISTY_SEED			= new Items(900,	"MISTY_SEED");
		/// <summary>Held: If the holder enters battle during Grassy Terrain, or if Grassy Terrain is activated while the holder is in battle, this item is consumed and the holder's Defense raises by one stage. : 	Held: Consumed on Grassy Terrain and raises the holder's Defense by one stage.</summary>
		public static readonly Items GRASSY_SEED			= new Items(901,	"GRASSY_SEED");
		/// <summary>Held: Changes Silvally to its Fighting form.  Changes Multi-Attack's type to Fighting. : 	Held: Changes Silvally to its Fighting form.  Changes Multi-Attack's type to Fighting.</summary>
		public static readonly Items FIGHTING_MEMORY			= new Items(902,	"FIGHTING_MEMORY");
		/// <summary>Held: Changes Silvally to its Flying form.  Changes Multi-Attack's type to Flying. : 	Held: Changes Silvally to its Flying form.  Changes Multi-Attack's type to Flying.</summary>
		public static readonly Items FLYING_MEMORY			= new Items(903,	"FLYING_MEMORY");
		/// <summary>Held: Changes Silvally to its Poison form.  Changes Multi-Attack's type to Poison. : 	Held: Changes Silvally to its Poison form.  Changes Multi-Attack's type to Poison.</summary>
		public static readonly Items POISON_MEMORY			= new Items(904,	"POISON_MEMORY");
		/// <summary>Held: Changes Silvally to its Ground form.  Changes Multi-Attack's type to Ground. : 	Held: Changes Silvally to its Ground form.  Changes Multi-Attack's type to Ground.</summary>
		public static readonly Items GROUND_MEMORY			= new Items(905,	"GROUND_MEMORY");
		/// <summary>Held: Changes Silvally to its Rock form.  Changes Multi-Attack's type to Rock. : 	Held: Changes Silvally to its Rock form.  Changes Multi-Attack's type to Rock.</summary>
		public static readonly Items ROCK_MEMORY			= new Items(906,	"ROCK_MEMORY");
		/// <summary>Held: Changes Silvally to its Bug form.  Changes Multi-Attack's type to Bug. : 	Held: Changes Silvally to its Bug form.  Changes Multi-Attack's type to Bug.</summary>
		public static readonly Items BUG_MEMORY			= new Items(907,	"BUG_MEMORY");
		/// <summary>Held: Changes Silvally to its Ghost form.  Changes Multi-Attack's type to Ghost. : 	Held: Changes Silvally to its Ghost form.  Changes Multi-Attack's type to Ghost.</summary>
		public static readonly Items GHOST_MEMORY			= new Items(908,	"GHOST_MEMORY");
		/// <summary>Held: Changes Silvally to its Steel form.  Changes Multi-Attack's type to Steel. : 	Held: Changes Silvally to its Steel form.  Changes Multi-Attack's type to Steel.</summary>
		public static readonly Items STEEL_MEMORY			= new Items(909,	"STEEL_MEMORY");
		/// <summary>Held: Changes Silvally to its Fire form.  Changes Multi-Attack's type to Fire. : 	Held: Changes Silvally to its Fire form.  Changes Multi-Attack's type to Fire.</summary>
		public static readonly Items FIRE_MEMORY			= new Items(910,	"FIRE_MEMORY");
		/// <summary>Held: Changes Silvally to its Water form.  Changes Multi-Attack's type to Water. : 	Held: Changes Silvally to its Water form.  Changes Multi-Attack's type to Water.</summary>
		public static readonly Items WATER_MEMORY			= new Items(911,	"WATER_MEMORY");
		/// <summary>Held: Changes Silvally to its Grass form.  Changes Multi-Attack's type to Grass. : 	Held: Changes Silvally to its Grass form.  Changes Multi-Attack's type to Grass.</summary>
		public static readonly Items GRASS_MEMORY			= new Items(912,	"GRASS_MEMORY");
		/// <summary>Held: Changes Silvally to its Electric form.  Changes Multi-Attack's type to Electric. : 	Held: Changes Silvally to its Electric form.  Changes Multi-Attack's type to Electric.</summary>
		public static readonly Items ELECTRIC_MEMORY			= new Items(913,	"ELECTRIC_MEMORY");
		/// <summary>Held: Changes Silvally to its Psychic form.  Changes Multi-Attack's type to Psychic. : 	Held: Changes Silvally to its Psychic form.  Changes Multi-Attack's type to Psychic.</summary>
		public static readonly Items PSYCHIC_MEMORY			= new Items(914,	"PSYCHIC_MEMORY");
		/// <summary>Held: Changes Silvally to its Ice form.  Changes Multi-Attack's type to Ice. : 	Held: Changes Silvally to its Ice form.  Changes Multi-Attack's type to Ice.</summary>
		public static readonly Items ICE_MEMORY			= new Items(915,	"ICE_MEMORY");
		/// <summary>Held: Changes Silvally to its Dragon form.  Changes Multi-Attack's type to Dragon. : 	Held: Changes Silvally to its Dragon form.  Changes Multi-Attack's type to Dragon.</summary>
		public static readonly Items DRAGON_MEMORY			= new Items(916,	"DRAGON_MEMORY");
		/// <summary>Held: Changes Silvally to its Dark form.  Changes Multi-Attack's type to Dark. : 	Held: Changes Silvally to its Dark form.  Changes Multi-Attack's type to Dark.</summary>
		public static readonly Items DARK_MEMORY			= new Items(917,	"DARK_MEMORY");
		/// <summary>Held: Changes Silvally to its Fairy form.  Changes Multi-Attack's type to Fairy. : 	Held: Changes Silvally to its Fairy form.  Changes Multi-Attack's type to Fairy.</summary>
		public static readonly Items FAIRY_MEMORY			= new Items(918,	"FAIRY_MEMORY");
		/// <summary>XXX new effect for bike--green : 	XXX new effect for bike--green</summary>
		public static readonly Items BIKE__GREEN			= new Items(919,	"BIKE__GREEN");
		/// <summary>XXX new effect for storage-key--galactic-warehouse : 	XXX new effect for storage-key--galactic-warehouse</summary>
		public static readonly Items STORAGE_KEY__GALACTIC_WAREHOUSE			= new Items(920,	"STORAGE_KEY__GALACTIC_WAREHOUSE");
		/// <summary>XXX new effect for basement-key--goldenrod : 	XXX new effect for basement-key--goldenrod</summary>
		public static readonly Items BASEMENT_KEY__GOLDENROD			= new Items(921,	"BASEMENT_KEY__GOLDENROD");
		/// <summary>XXX new effect for xtranceiver--red : 	XXX new effect for xtranceiver--red</summary>
		public static readonly Items XTRANCEIVER__RED			= new Items(922,	"XTRANCEIVER__RED");
		/// <summary>XXX new effect for xtranceiver--yellow : 	XXX new effect for xtranceiver--yellow</summary>
		public static readonly Items XTRANCEIVER__YELLOW			= new Items(923,	"XTRANCEIVER__YELLOW");
		/// <summary>XXX new effect for dna-splicers--merge : 	XXX new effect for dna-splicers--merge</summary>
		public static readonly Items DNA_SPLICERS__MERGE			= new Items(924,	"DNA_SPLICERS__MERGE");
		/// <summary>XXX new effect for dna-splicers--split : 	XXX new effect for dna-splicers--split</summary>
		public static readonly Items DNA_SPLICERS__SPLIT			= new Items(925,	"DNA_SPLICERS__SPLIT");
		/// <summary>XXX new effect for dropped-item--red : 	XXX new effect for dropped-item--red</summary>
		public static readonly Items DROPPED_ITEM__RED			= new Items(926,	"DROPPED_ITEM__RED");
		/// <summary>XXX new effect for dropped-item--yellow : 	XXX new effect for dropped-item--yellow</summary>
		public static readonly Items DROPPED_ITEM__YELLOW			= new Items(927,	"DROPPED_ITEM__YELLOW");
		/// <summary>XXX new effect for holo-caster--green : 	XXX new effect for holo-caster--green</summary>
		public static readonly Items HOLO_CASTER__GREEN			= new Items(928,	"HOLO_CASTER__GREEN");
		/// <summary>XXX new effect for bike--yellow : 	XXX new effect for bike--yellow</summary>
		public static readonly Items BIKE__YELLOW			= new Items(929,	"BIKE__YELLOW");
		/// <summary>XXX new effect for holo-caster--red : 	XXX new effect for holo-caster--red</summary>
		public static readonly Items HOLO_CASTER__RED			= new Items(930,	"HOLO_CASTER__RED");
		/// <summary>XXX new effect for basement-key--new-mauville : 	XXX new effect for basement-key--new-mauville</summary>
		public static readonly Items BASEMENT_KEY__NEW_MAUVILLE			= new Items(931,	"BASEMENT_KEY__NEW_MAUVILLE");
		/// <summary>XXX new effect for storage-key--sea-mauville : 	XXX new effect for storage-key--sea-mauville</summary>
		public static readonly Items STORAGE_KEY__SEA_MAUVILLE			= new Items(932,	"STORAGE_KEY__SEA_MAUVILLE");
		/// <summary>XXX new effect for ss-ticket--hoenn : 	XXX new effect for ss-ticket--hoenn</summary>
		public static readonly Items SS_TICKET__HOENN			= new Items(933,	"SS_TICKET__HOENN");
		/// <summary>XXX new effect for contest-costume--dress : 	XXX new effect for contest-costume--dress</summary>
		public static readonly Items CONTEST_COSTUME__DRESS			= new Items(934,	"CONTEST_COSTUME__DRESS");
		/// <summary>XXX new effect for meteorite--2 : 	XXX new effect for meteorite--2</summary>
		public static readonly Items METEORITE__2			= new Items(935,	"METEORITE__2");
		/// <summary>XXX new effect for meteorite--3 : 	XXX new effect for meteorite--3</summary>
		public static readonly Items METEORITE__3			= new Items(936,	"METEORITE__3");
		/// <summary>XXX new effect for meteorite--4 : 	XXX new effect for meteorite--4</summary>
		public static readonly Items METEORITE__4			= new Items(937,	"METEORITE__4");
		/// <summary>XXX new effect for normalium-z--bag : 	XXX new effect for normalium-z--bag</summary>
		public static readonly Items NORMALIUM_Z__BAG			= new Items(938,	"NORMALIUM_Z__BAG");
		/// <summary>XXX new effect for firium-z--bag : 	XXX new effect for firium-z--bag</summary>
		public static readonly Items FIRIUM_Z__BAG			= new Items(939,	"FIRIUM_Z__BAG");
		/// <summary>XXX new effect for waterium-z--bag : 	XXX new effect for waterium-z--bag</summary>
		public static readonly Items WATERIUM_Z__BAG			= new Items(940,	"WATERIUM_Z__BAG");
		/// <summary>XXX new effect for electrium-z--bag : 	XXX new effect for electrium-z--bag</summary>
		public static readonly Items ELECTRIUM_Z__BAG			= new Items(941,	"ELECTRIUM_Z__BAG");
		/// <summary>XXX new effect for grassium-z--bag : 	XXX new effect for grassium-z--bag</summary>
		public static readonly Items GRASSIUM_Z__BAG			= new Items(942,	"GRASSIUM_Z__BAG");
		/// <summary>XXX new effect for icium-z--bag : 	XXX new effect for icium-z--bag</summary>
		public static readonly Items ICIUM_Z__BAG			= new Items(943,	"ICIUM_Z__BAG");
		/// <summary>XXX new effect for fightinium-z--bag : 	XXX new effect for fightinium-z--bag</summary>
		public static readonly Items FIGHTINIUM_Z__BAG			= new Items(944,	"FIGHTINIUM_Z__BAG");
		/// <summary>XXX new effect for poisonium-z--bag : 	XXX new effect for poisonium-z--bag</summary>
		public static readonly Items POISONIUM_Z__BAG			= new Items(945,	"POISONIUM_Z__BAG");
		/// <summary>XXX new effect for groundium-z--bag : 	XXX new effect for groundium-z--bag</summary>
		public static readonly Items GROUNDIUM_Z__BAG			= new Items(946,	"GROUNDIUM_Z__BAG");
		/// <summary>XXX new effect for flyinium-z--bag : 	XXX new effect for flyinium-z--bag</summary>
		public static readonly Items FLYINIUM_Z__BAG			= new Items(947,	"FLYINIUM_Z__BAG");
		/// <summary>XXX new effect for psychium-z--bag : 	XXX new effect for psychium-z--bag</summary>
		public static readonly Items PSYCHIUM_Z__BAG			= new Items(948,	"PSYCHIUM_Z__BAG");
		/// <summary>XXX new effect for buginium-z--bag : 	XXX new effect for buginium-z--bag</summary>
		public static readonly Items BUGINIUM_Z__BAG			= new Items(949,	"BUGINIUM_Z__BAG");
		/// <summary>XXX new effect for rockium-z--bag : 	XXX new effect for rockium-z--bag</summary>
		public static readonly Items ROCKIUM_Z__BAG			= new Items(950,	"ROCKIUM_Z__BAG");
		/// <summary>XXX new effect for ghostium-z--bag : 	XXX new effect for ghostium-z--bag</summary>
		public static readonly Items GHOSTIUM_Z__BAG			= new Items(951,	"GHOSTIUM_Z__BAG");
		/// <summary>XXX new effect for dragonium-z--bag : 	XXX new effect for dragonium-z--bag</summary>
		public static readonly Items DRAGONIUM_Z__BAG			= new Items(952,	"DRAGONIUM_Z__BAG");
		/// <summary>XXX new effect for darkinium-z--bag : 	XXX new effect for darkinium-z--bag</summary>
		public static readonly Items DARKINIUM_Z__BAG			= new Items(953,	"DARKINIUM_Z__BAG");
		/// <summary>XXX new effect for steelium-z--bag : 	XXX new effect for steelium-z--bag</summary>
		public static readonly Items STEELIUM_Z__BAG			= new Items(954,	"STEELIUM_Z__BAG");
		/// <summary>XXX new effect for fairium-z--bag : 	XXX new effect for fairium-z--bag</summary>
		public static readonly Items FAIRIUM_Z__BAG			= new Items(955,	"FAIRIUM_Z__BAG");
		/// <summary>XXX new effect for pikanium-z--bag : 	XXX new effect for pikanium-z--bag</summary>
		public static readonly Items PIKANIUM_Z__BAG			= new Items(956,	"PIKANIUM_Z__BAG");
		/// <summary>XXX new effect for decidium-z--bag : 	XXX new effect for decidium-z--bag</summary>
		public static readonly Items DECIDIUM_Z__BAG			= new Items(957,	"DECIDIUM_Z__BAG");
		/// <summary>XXX new effect for incinium-z--bag : 	XXX new effect for incinium-z--bag</summary>
		public static readonly Items INCINIUM_Z__BAG			= new Items(958,	"INCINIUM_Z__BAG");
		/// <summary>XXX new effect for primarium-z--bag : 	XXX new effect for primarium-z--bag</summary>
		public static readonly Items PRIMARIUM_Z__BAG			= new Items(959,	"PRIMARIUM_Z__BAG");
		/// <summary>XXX new effect for tapunium-z--bag : 	XXX new effect for tapunium-z--bag</summary>
		public static readonly Items TAPUNIUM_Z__BAG			= new Items(960,	"TAPUNIUM_Z__BAG");
		/// <summary>XXX new effect for marshadium-z--bag : 	XXX new effect for marshadium-z--bag</summary>
		public static readonly Items MARSHADIUM_Z__BAG			= new Items(961,	"MARSHADIUM_Z__BAG");
		/// <summary>XXX new effect for aloraichium-z--bag : 	XXX new effect for aloraichium-z--bag</summary>
		public static readonly Items ALORAICHIUM_Z__BAG			= new Items(962,	"ALORAICHIUM_Z__BAG");
		/// <summary>XXX new effect for snorlium-z--bag : 	XXX new effect for snorlium-z--bag</summary>
		public static readonly Items SNORLIUM_Z__BAG			= new Items(963,	"SNORLIUM_Z__BAG");
		/// <summary>XXX new effect for eevium-z--bag : 	XXX new effect for eevium-z--bag</summary>
		public static readonly Items EEVIUM_Z__BAG			= new Items(964,	"EEVIUM_Z__BAG");
		/// <summary>XXX new effect for mewnium-z--bag : 	XXX new effect for mewnium-z--bag</summary>
		public static readonly Items MEWNIUM_Z__BAG			= new Items(965,	"MEWNIUM_Z__BAG");
		/// <summary>XXX new effect for pikashunium-z--bag : 	XXX new effect for pikashunium-z--bag</summary>
		public static readonly Items PIKASHUNIUM_Z__BAG			= new Items(966,	"PIKASHUNIUM_Z__BAG");
		/// <summary>XXX new effect for solganium-z--held : 	XXX new effect for solganium-z--held</summary>
		public static readonly Items SOLGANIUM_Z__HELD			= new Items(967,	"SOLGANIUM_Z__HELD");
		/// <summary>XXX new effect for lunalium-z--held : 	XXX new effect for lunalium-z--held</summary>
		public static readonly Items LUNALIUM_Z__HELD			= new Items(968,	"LUNALIUM_Z__HELD");
		/// <summary>XXX new effect for ultranecrozium-z--held : 	XXX new effect for ultranecrozium-z--held</summary>
		public static readonly Items ULTRANECROZIUM_Z__HELD			= new Items(969,	"ULTRANECROZIUM_Z__HELD");
		/// <summary>XXX new effect for mimikium-z--held : 	XXX new effect for mimikium-z--held</summary>
		public static readonly Items MIMIKIUM_Z__HELD			= new Items(970,	"MIMIKIUM_Z__HELD");
		/// <summary>XXX new effect for lycanium-z--held : 	XXX new effect for lycanium-z--held</summary>
		public static readonly Items LYCANIUM_Z__HELD			= new Items(971,	"LYCANIUM_Z__HELD");
		/// <summary>XXX new effect for kommonium-z--held : 	XXX new effect for kommonium-z--held</summary>
		public static readonly Items KOMMONIUM_Z__HELD			= new Items(972,	"KOMMONIUM_Z__HELD");
		/// <summary>XXX new effect for solganium-z--bag : 	XXX new effect for solganium-z--bag</summary>
		public static readonly Items SOLGANIUM_Z__BAG			= new Items(973,	"SOLGANIUM_Z__BAG");
		/// <summary>XXX new effect for lunalium-z--bag : 	XXX new effect for lunalium-z--bag</summary>
		public static readonly Items LUNALIUM_Z__BAG			= new Items(974,	"LUNALIUM_Z__BAG");
		/// <summary>XXX new effect for ultranecrozium-z--bag : 	XXX new effect for ultranecrozium-z--bag</summary>
		public static readonly Items ULTRANECROZIUM_Z__BAG			= new Items(975,	"ULTRANECROZIUM_Z__BAG");
		/// <summary>XXX new effect for mimikium-z--bag : 	XXX new effect for mimikium-z--bag</summary>
		public static readonly Items MIMIKIUM_Z__BAG			= new Items(976,	"MIMIKIUM_Z__BAG");
		/// <summary>XXX new effect for lycanium-z--bag : 	XXX new effect for lycanium-z--bag</summary>
		public static readonly Items LYCANIUM_Z__BAG			= new Items(977,	"LYCANIUM_Z__BAG");
		/// <summary>XXX new effect for kommonium-z--bag : 	XXX new effect for kommonium-z--bag</summary>
		public static readonly Items KOMMONIUM_Z__BAG			= new Items(978,	"KOMMONIUM_Z__BAG");
		/// <summary>XXX new effect for z-power-ring : 	XXX new effect for z-power-ring</summary>
		public static readonly Items Z_POWER_RING			= new Items(979,	"Z_POWER_RING");
		/// <summary>XXX new effect for pink-petal : 	XXX new effect for pink-petal</summary>
		public static readonly Items PINK_PETAL			= new Items(980,	"PINK_PETAL");
		/// <summary>XXX new effect for orange-petal : 	XXX new effect for orange-petal</summary>
		public static readonly Items ORANGE_PETAL			= new Items(981,	"ORANGE_PETAL");
		/// <summary>XXX new effect for blue-petal : 	XXX new effect for blue-petal</summary>
		public static readonly Items BLUE_PETAL			= new Items(982,	"BLUE_PETAL");
		/// <summary>XXX new effect for red-petal : 	XXX new effect for red-petal</summary>
		public static readonly Items RED_PETAL			= new Items(983,	"RED_PETAL");
		/// <summary>XXX new effect for green-petal : 	XXX new effect for green-petal</summary>
		public static readonly Items GREEN_PETAL			= new Items(984,	"GREEN_PETAL");
		/// <summary>XXX new effect for yellow-petal : 	XXX new effect for yellow-petal</summary>
		public static readonly Items YELLOW_PETAL			= new Items(985,	"YELLOW_PETAL");
		/// <summary>XXX new effect for purple-petal : 	XXX new effect for purple-petal</summary>
		public static readonly Items PURPLE_PETAL			= new Items(986,	"PURPLE_PETAL");
		/// <summary>XXX new effect for rainbow-flower : 	XXX new effect for rainbow-flower</summary>
		public static readonly Items RAINBOW_FLOWER			= new Items(987,	"RAINBOW_FLOWER");
		/// <summary>XXX new effect for surge-badge : 	XXX new effect for surge-badge</summary>
		public static readonly Items SURGE_BADGE			= new Items(988,	"SURGE_BADGE");
		/// <summary>XXX new effect for n-solarizer--merge : 	XXX new effect for n-solarizer--merge</summary>
		public static readonly Items N_SOLARIZER__MERGE			= new Items(989,	"N_SOLARIZER__MERGE");
		/// <summary>XXX new effect for n-lunarizer--merge : 	XXX new effect for n-lunarizer--merge</summary>
		public static readonly Items N_LUNARIZER__MERGE			= new Items(990,	"N_LUNARIZER__MERGE");
		/// <summary>XXX new effect for n-solarizer--split : 	XXX new effect for n-solarizer--split</summary>
		public static readonly Items N_SOLARIZER__SPLIT			= new Items(991,	"N_SOLARIZER__SPLIT");
		/// <summary>XXX new effect for n-lunarizer--split : 	XXX new effect for n-lunarizer--split</summary>
		public static readonly Items N_LUNARIZER__SPLIT			= new Items(992,	"N_LUNARIZER__SPLIT");
		/// <summary>XXX new effect for ilimas-normalium-z : 	XXX new effect for ilimas-normalium-z</summary>
		public static readonly Items ILIMAS_NORMALIUM_Z			= new Items(993,	"ILIMAS_NORMALIUM_Z");
		/// <summary>XXX new effect for left-poke-ball : 	XXX new effect for left-poke-ball</summary>
		public static readonly Items LEFT_POKE_BALL			= new Items(994,	"LEFT_POKE_BALL");
		/// <summary>XXX new effect for roto-hatch : 	XXX new effect for roto-hatch</summary>
		public static readonly Items ROTO_HATCH			= new Items(995,	"ROTO_HATCH");
		/// <summary>XXX new effect for roto-bargain : 	XXX new effect for roto-bargain</summary>
		public static readonly Items ROTO_BARGAIN			= new Items(996,	"ROTO_BARGAIN");
		/// <summary>XXX new effect for roto-prize-money : 	XXX new effect for roto-prize-money</summary>
		public static readonly Items ROTO_PRIZE_MONEY			= new Items(997,	"ROTO_PRIZE_MONEY");
		/// <summary>XXX new effect for roto-exp-points : 	XXX new effect for roto-exp-points</summary>
		public static readonly Items ROTO_EXP_POINTS			= new Items(998,	"ROTO_EXP_POINTS");
		/// <summary>XXX new effect for roto-friendship : 	XXX new effect for roto-friendship</summary>
		public static readonly Items ROTO_FRIENDSHIP			= new Items(999,	"ROTO_FRIENDSHIP");
		/// <summary>XXX new effect for roto-encounter : 	XXX new effect for roto-encounter</summary>
		public static readonly Items ROTO_ENCOUNTER			= new Items(1000,	"ROTO_ENCOUNTER");
		/// <summary>XXX new effect for roto-stealth : 	XXX new effect for roto-stealth</summary>
		public static readonly Items ROTO_STEALTH			= new Items(1001,	"ROTO_STEALTH");
		/// <summary>XXX new effect for roto-hp-restore : 	XXX new effect for roto-hp-restore</summary>
		public static readonly Items ROTO_HP_RESTORE			= new Items(1002,	"ROTO_HP_RESTORE");
		/// <summary>XXX new effect for roto-pp-restore : 	XXX new effect for roto-pp-restore</summary>
		public static readonly Items ROTO_PP_RESTORE			= new Items(1003,	"ROTO_PP_RESTORE");
		/// <summary>XXX new effect for roto-boost : 	XXX new effect for roto-boost</summary>
		public static readonly Items ROTO_BOOST			= new Items(1004,	"ROTO_BOOST");
		/// <summary>XXX new effect for roto-catch : 	XXX new effect for roto-catch</summary>
		public static readonly Items ROTO_CATCH			= new Items(1005,	"ROTO_CATCH");
	}
}