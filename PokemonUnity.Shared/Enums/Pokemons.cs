using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;


/// <summary>
/// Namespace to nest all Pokemon Enums
/// </summary>
namespace PokemonUnity.Monster
{
	#region PokemonData Enumerators
	public enum Rarity
	{
		Common,
		/// <summary>
		/// Legendary Pokémon are a group of incredibly rare and often very powerful Pokémon, generally featured prominently in the legends and myths of the Pokémon world.
		/// </summary>
		Legendary,
		/// <summary>
		/// Mythical Pokémon are a group of Pokémon, which are usually event-exclusive (similar to legendary, but separate) 
		/// </summary>
		Mythical
	}
	/// <summary>
	/// The likelihood of a Pokémon of the species being a certain gender.
	/// </summary>
	public enum GenderRatio
	{
		AlwaysMale,
		FemaleOneEighth,
		Female25Percent,
        //no inbetween?
		Female50Percent,
        //divided by 8 and missing values in between...
		Female75Percent,
		FemaleSevenEighths,
		AlwaysFemale,
		Genderless
	}
	public enum LevelingRate
	{
		ERRATIC = 6, //fast then very slow?
		FAST = 3,
		MEDIUMFAST = 2, //Medium?
		MEDIUMSLOW = 4,
		SLOW = 1,
		FLUCTUATING = 5 //slow then fast?
	};
    public enum EggGroups
	{
		NONE = 0,
		MONSTER = 1,
		WATER1 = 2,
		BUG = 3,
		FLYING = 4,
		FIELD = 5, //"Ground"?
		FAIRY = 6,
		GRASS = 7, //"Plant"
		HUMANLIKE = 8, //"humanshape"
		WATER3 = 9,
		MINERAL = 10,
		AMORPHOUS = 11, //"indeterminate"
		WATER2 = 12,
		DITTO = 13,
		DRAGON = 14,
		UNDISCOVERED = 15 //"no-eggs"
	};
	public enum Shape
	{
		BALL		= 1,
		SQUIGGLE	= 2,
		FISH		= 3,
		ARMS		= 4,
		BLOB		= 5,
		UPRIGHT		= 6,
		LEGS		= 7,
		QUADRUPED	= 8,
		WINGS		= 9,
		TENTACLES	= 10,
		HEADS		= 11,
		HUMANOID	= 12,
		BUG_WINGS	= 13,
		ARMOR		= 14
	}
	public enum Habitat
	{
		NONE			= 0,
		CAVE			= 1,
		FOREST			= 2,
		GRASSLAND		= 3,
		MOUNTAIN		= 4,
		RARE			= 5,
		ROUGH_TERRAIN	= 6,
		SEA				= 7,
		URBAN			= 8,
		WATERS_EDGE		= 9
	}
	#endregion
	#region Nature
	public enum Natures
	{
		//UNSET,
		/*HARDY,
		LONELY,
		BRAVE,
		ADAMANT,
		NAUGHTY,
		BOLD,
		DOCILE,
		RELAXED,
		IMPISH,
		LAX,
		TIMID,
		HASTY,
		SERIOUS,
		JOLLY,
		NAIVE,
		MODEST,
		MILD,
		QUIET,
		BASHFUL,
		RASH,
		CALM,
		GENTLE,
		SASSY,
		CAREFUL,
		QUIRKY*/
		HARDY
		,BOLD
		,MODEST
		,CALM
		,TIMID
		,LONELY
		,DOCILE
		,MILD
		,GENTLE
		,HASTY
		,ADAMANT
		,IMPISH
		,BASHFUL
		,CAREFUL
		,RASH
		,JOLLY
		,NAUGHTY
		,LAX
		,QUIRKY
		,NAIVE
		,BRAVE
		,RELAXED
		,QUIET
		,SASSY
		,SERIOUS
	}
	#endregion
	#region Pokemon
	public enum Stats
	{
		ATTACK	= 1
		,DEFENSE= 2
		,SPEED	= 3
		,SPATK	= 4
		,SPDEF	= 5
		,HP		= 0
	}
	public enum LearnMethod
	{
		levelup = 1,
		egg = 2,
		tutor = 3,
		machine = 4,
		stadium_surfing_pikachu = 5,
		light_ball_egg = 6,
		colosseum_purification = 7,
		xd_shadow = 8,
		xd_purification = 9,
		form_change = 10,
		shadow,// = 8,
		purification// = 7,
	}
	/// <summary>
	/// no parameter,
	/// Positive integer,
	/// Item  = <see cref="Items"/>,
	/// Move = <see cref="Moves"/>,
	/// Species = <see cref="Pokemons"/>,
	/// Type = <see cref="Types"/>
	/// </summary>
	/// <example>
	/// <para>E.G.	Poliwhirl(61)
	///		<code>new int[]{62,186},
	///		new string[]{"Stone,Water Stone","Trade\Item,King's Rock"}),</code></para> 
	/// <para>
	/// E.G. to evolve to sylveon
	///		<code>new int[]{..., 700},
	///		new string[]{..., "Amie\Move,2\Fairy"}),</code>
	/// </para> 
	/// </example>
	/// ToDo: Custom class => new Evolve(){ EvolveMethod.Item, Items.Item } 
	/// ToDo: If all conditions are met in "new Evolve()" instance, then evolve.
	/// Ideas:
	/// Merge two or more existing methods together into a new one.
	/// Evolution that depends on the Pokémon's nature or form.
	/// Fusion evolution(e.g. for Magnemite/Slowpoke). Check that there is a Shellder in the party, and if so, delete it and evolve the levelled-up Slowpoke.
	/// Check how many EVs the Pokémon has, and allows evolution only if that amount is greater than or equal to the EV threshold value set by the parameter.
	/// Shiny-Random?
	public enum EvolutionMethod
	{
		/// <summary>
		///	if pokemon's level is greater or equal to int level<para></para>
		/// <code>Level,int level</code>
		/// </summary>
		Level,
		/// <summary>
		///	Exactly the same as <see cref="Level"/>, 
		///	except the Pokémon must also be male.<para></para>
		/// <code>Level,int level</code>
		/// </summary>
		/// <example>Burmy</example>
		LevelMale,
		/// <summary>
		///	Exactly the same as <see cref="Level"/>, 
		///	except the Pokémon must also be female.<para></para>
		/// <code>Level,int level</code>
		/// </summary>
		/// <example>Burmy, Combee</example>
		LevelFemale,
		///	<summary>
		///	The Pokémon will evolve if a particular item is used on it 
		///	(named by the parameter - typically an evolution stone).<para></para>
		/// <code>Stone,<see cref="Inventory.Items"/> itemName</code>
		///	</summary>
		/// <example>Clefairy, Cottonee, Eelektrik, Eevee, Exeggcute, Gloom, 
		/// Growlithe, Jigglypuff, Lampent, Lombre, Minccino, Misdreavus, 
		/// Munna, Murkrow, Nidorina, Nidorino, Nuzleaf, Panpour, Pansage, 
		/// Pansear, Petilil, Pikachu, Poliwhirl, Roselia, Shellder, Skitty, 
		/// Staryu, Sunkern, Togetic, Vulpix, Weepinbell</example>
		Item,
		/// <summary>
		///	Exactly the same as "<see cref="Item"/>", 
		///	except the Pokémon must also be male.
		/// </summary>
		/// <example>Kirlia</example>
		ItemMale,
		/// <summary>
		///	Exactly the same as "<see cref="Item"/>", 
		///	except the Pokémon must also be female.
		/// </summary>
		/// <example>Snorunt</example>
		ItemFemale,
		/// <summary>
		///	The Pokémon will evolve immediately after it is 
		///	<seealso cref="Pokemon.ObtainedMethod.TRADED"/>
		///	</summary>
		/// <example>Boldore, Graveler, Gurdurr, Haunter, Kadabra, Machoke</example>
		Trade,
		/// <summary>
		///	Exactly the same as <see cref="Trade"/>, 
		///	except the Pokémon must also be holding a particular item 
		///	(named by the parameter). 
		///	That item is removed afterwards.
		///	</summary>
		/// <example>Clamperl, Dusclops, Electabuzz, Feebas, Magmar, 
		/// Onix, Porygon, Porygon2, Poliwhirl, Rhydon, Scyther, Seadra, 
		/// Slowpoke</example>
		TradeItem,
		/// <summary>
		///	Exactly the same as <see cref="Trade"/>, 
		///	except the Pokémon must have been traded for a Pokémon of a certain species 
		///	(named by the parameter).
		///	</summary>
		/// <example>Karrablast, Shelmet</example>
		TradeSpecies,
		/// <summary>
		///	if pokemon's happiness is greater or equal to 220.<para></para>
		///	Note: Happiness checks should come last before all other methods.
		///	</summary>
		/// <example>Azurill, Buneary, Chansey, Cleffa, Golbat, 
		/// Igglybuff, Munchlax, Pichu, Swadloon, Togepi, Woobat</example>
		Happiness,
		/// <summary>
		///	Exactly the same as <see cref="Happiness"/>, 
		///	but will only evolve during the daytime.
		///	</summary>
		/// <example>Budew, Eevee, Riolu</example>
		HappinessDay,
		/// <summary>
		///	Exactly the same as <see cref="Happiness"/>, 
		///	but will only evolve during the night-time.
		///	</summary>
		/// <example>Chingling, Eevee</example>
		HappinessNight,
		/// <summary>
		///	This method is almost identical to <see cref="Happiness"/>, 
		///	with the sole changes of replacing the "greater than" sign 
		///	to a "less than" sign, and changing the threshold value. 
		///	If you use this method, 
		///	you may also want to make it easier to get a Pokémon to hate you in-game 
		///	(currently the only ways to do this are fainting and using herbal medicine, 
		///	which can easily be countered by the many more happiness-boosting methods).
		///	</summary>
		Hatred,
		/// <summary>
		///	if time is between 9PM and 4AM time is "Night". else time is "Day".
		///	if time is equal to string dayNight (either Day, or Night).<para></para>
		///	<code>Time,DatetimeOffset/bool dayNight</code>
		/// </summary>
		Time,
		/// <summary>
		///	if date is between Day-Month or maybe certain 1/4 (quarter) of the year.
		///	if date is equal to string season (either Summer, Winter, Spring, or Fall).<para></para>
		///	<code>Time,DatetimeOffset/bool season</code>
		/// </summary>
		/// is holiday to "spot-on" as an occasion or requirement for leveling-up?
		Season,
		/// <summary>
		///	if pokemon's heldItem is equal to string itemName<para></para>
		/// <example>Item,string itemName</example>
		/// </summary>
		/// Holding a certain item after leveling-up?
		HoldItem,
		/// <summary>
		///	The Pokémon will evolve if it levels up during the daytime 
		///	while holding a particular item (named by the parameter).<para></para>
		/// <code>Item,string itemName</code>
		/// </summary>
		/// <example>Happiny</example>
		HoldItemDay,
		/// <summary>
		///	The Pokémon will evolve if it levels up during the night-time 
		///	while holding a particular item (named by the parameter).<para></para>
		/// <code>Item,string itemName</code>
		/// </summary>
		/// <example>Gligar, Sneasel</example>
		HoldItemNight,
		/// <summary>
		/// The Pokémon will evolve when it levels up, 
		/// if its beauty stat is greater than or equal to the parameter.
		/// </summary>
		/// <example>Feebas</example>
		Beauty,
		/// <summary>
		///	The Pokémon will evolve if it levels up 
		///	while knowing a particular move 
		///	(named by the parameter).<para></para>
		/// <example>Move,string moveName</example>
		/// </summary>
		/// <example>Aipom, Bonsly, Lickitung, Mime Jr., Piloswine, Tangela, Yanma</example>
		Move,
		/// <summary>
		///	The Pokémon will evolve if it levels up 
		///	while the player has a Pokémon of a certain species in their party 
		///	(named by the parameter). 
		///	The named Pokémon is unaffected.<para></para>
		/// <example>Pokemon,string pokemonName</example>
		/// </summary>
		/// <example>Mantyke, Mantine</example>
		/// if party contains a Remoraid
		Party,
		/// <summary>
		///	The Pokémon will evolve if it levels up while 
		///	the player has a Pokémon of a certain type in their party 
		///	(named by the parameter). <para></para>
		/// <example>Type,string pokemonTypeName</example>
		/// </summary>
		/// <example>Pangoro</example>
		/// if party contains a dark pokemon
		Type,
		/// <summary>
		/// The Pokémon will evolve when it levels up, 
		/// if the player is currently on the map given by the parameter.<para></para>
		///	<code>Map,string mapName</code>
		/// </summary>
		Location,
		///	<summary>
		///	The Pokémon will evolve if it levels up only during a certain kind of overworld weather.
		///	</summary>
		/// <example>Goodra</example>
		///	if currentMap's weather is rain
		Weather,
		///	<summary>
		///	Exactly the same as <see cref="Level"/>, 
		///	except the Pokémon's Attack stat must also be greater than its Defense stat.
		///	</summary>
		/// <example>Hitmonlee</example>
		AttackGreater,
		///	<summary>
		///	Exactly the same as <see cref="Level"/>, 
		///	except the Pokémon's Attack stat must also be lower than its Defense stat.
		///	</summary>
		/// <example>Hitmonchan</example>
		DefenseGreater,
		/// <summary>
		/// Exactly the same as <see cref="Level"/>, 
		/// except the Pokémon's Attack stat must also be equal to its Defense stat.
		/// </summary>
		/// <example>Hitmontop</example>
		AtkDefEqual,
		///	<summary>if pokemon's shinyValue divided by 2's remainder is equal to 0 or 1</summary>
		///	What about level parameter? Maybe "ShinyYes/ShinyNo" or "Shiny0/Shiny1", in combination with Level or Happiness-parameter? 
		Shiny,
		/// <summary>Unique evolution methods: if pokemon's shinyValue divided by 2's remainder is equal to 0</summary>
		/// Shiny value? I thought it was based on "Friendship"
		Silcoon,
		///	<summary>
		///	Unique evolution methods: if pokemon's shinyValue divided by 2's remainder is equal to 1
		///	</summary>
		Cascoon,
		///	<summary>
		///	Unique evolution methods: 
		///	Exactly the same as <see cref="Level"/>. 
		///	There is no difference between the two methods at all. 
		///	Is used alongside the method <see cref="Shedinja"/>.
		///	</summary>
		Ninjask,
		///	<summary>
		///	Unique evolution methods: 
		///	Must be used with the method <see cref="Ninjask"/>. 
		///	Duplicates the Pokémon that just evolved 
		///	(if there is an empty space in the party), 
		///	and changes the duplicate's species to the given species.
		///	</summary>
		///	how is this different from "party"?
		Shedinja,
		///	<summary>
		///	Unique evolution methods: 
		///	This method is almost identical to <see cref="Level"/>. 
		///	However evolution only triggers if Pokemon has ability <see cref="Abilities.OWN_TEMPO"/>
		///	and time is Dusk (between hours of 5 pm to 5:59 pm in in-game time)
		///	</summary>
		Lycanroc,
		/// <summary>
		///	The Pokémon will evolve if it levels up while 
		///	the Pokémon has a move of a certain type in their moveset 
		///	(named by the parameter). <para></para>
		/// <example>Type,string pokemonTypeName</example>
		/// </summary>
		/// <example>Sylveon</example>
		/// if pokemon contains a fairy-type move
		Affection,
        /// <summary>
        /// </summary>
        /// Just wanted to see a requirement for after "fainting" too many times, your pokemon just died, and became a ghost-type...  
        Deaths
	}
	#endregion
	#region Form
	public enum Forms
	{
		NONE				= 0
		,BULBASAUR			= 1
		,IVYSAUR			= 2
		,VENUSAUR			= 3
		,CHARMANDER			= 4
		,CHARMELEON			= 5
		,CHARIZARD			= 6
		,SQUIRTLE			= 7
		,WARTORTLE			= 8
		,BLASTOISE			= 9
		,CATERPIE			= 10
		,METAPOD			= 11
		,BUTTERFREE			= 12
		,WEEDLE				= 13
		,KAKUNA				= 14
		,BEEDRILL			= 15
		,PIDGEY				= 16
		,PIDGEOTTO			= 17
		,PIDGEOT			= 18
		,RATTATA			= 19
		,RATICATE			= 20
		,SPEAROW			= 21
		,FEAROW				= 22
		,EKANS				= 23
		,ARBOK				= 24
		,PIKACHU			= 25
		,RAICHU				= 26
		,SANDSHREW			= 27
		,SANDSLASH			= 28
		,NIDORAN_F			= 29
		,NIDORINA			= 30
		,NIDOQUEEN			= 31
		,NIDORAN_M			= 32
		,NIDORINO			= 33
		,NIDOKING			= 34
		,CLEFAIRY			= 35
		,CLEFABLE			= 36
		,VULPIX				= 37
		,NINETALES			= 38
		,JIGGLYPUFF			= 39
		,WIGGLYTUFF			= 40
		,ZUBAT				= 41
		,GOLBAT				= 42
		,ODDISH				= 43
		,GLOOM				= 44
		,VILEPLUME			= 45
		,PARAS				= 46
		,PARASECT			= 47
		,VENONAT			= 48
		,VENOMOTH			= 49
		,DIGLETT			= 50
		,DUGTRIO			= 51
		,MEOWTH				= 52
		,PERSIAN			= 53
		,PSYDUCK			= 54
		,GOLDUCK			= 55
		,MANKEY				= 56
		,PRIMEAPE			= 57
		,GROWLITHE			= 58
		,ARCANINE			= 59
		,POLIWAG			= 60
		,POLIWHIRL			= 61
		,POLIWRATH			= 62
		,ABRA				= 63
		,KADABRA			= 64
		,ALAKAZAM			= 65
		,MACHOP				= 66
		,MACHOKE			= 67
		,MACHAMP			= 68
		,BELLSPROUT			= 69
		,WEEPINBELL			= 70
		,VICTREEBEL			= 71
		,TENTACOOL			= 72
		,TENTACRUEL			= 73
		,GEODUDE			= 74
		,GRAVELER			= 75
		,GOLEM				= 76
		,PONYTA				= 77
		,RAPIDASH			= 78
		,SLOWPOKE			= 79
		,SLOWBRO			= 80
		,MAGNEMITE			= 81
		,MAGNETON			= 82
		,FARFETCHD			= 83
		,DODUO				= 84
		,DODRIO				= 85
		,SEEL				= 86
		,DEWGONG			= 87
		,GRIMER				= 88
		,MUK				= 89
		,SHELLDER			= 90
		,CLOYSTER			= 91
		,GASTLY				= 92
		,HAUNTER			= 93
		,GENGAR				= 94
		,ONIX				= 95
		,DROWZEE			= 96
		,HYPNO				= 97
		,KRABBY				= 98
		,KINGLER			= 99
		,VOLTORB			= 100
		,ELECTRODE			= 101
		,EXEGGCUTE			= 102
		,EXEGGUTOR			= 103
		,CUBONE				= 104
		,MAROWAK			= 105
		,HITMONLEE			= 106
		,HITMONCHAN			= 107
		,LICKITUNG			= 108
		,KOFFING			= 109
		,WEEZING			= 110
		,RHYHORN			= 111
		,RHYDON				= 112
		,CHANSEY			= 113
		,TANGELA			= 114
		,KANGASKHAN			= 115
		,HORSEA				= 116
		,SEADRA				= 117
		,GOLDEEN			= 118
		,SEAKING			= 119
		,STARYU				= 120
		,STARMIE			= 121
		,MR_MIME			= 122
		,SCYTHER			= 123
		,JYNX				= 124
		,ELECTABUZZ			= 125
		,MAGMAR				= 126
		,PINSIR				= 127
		,TAUROS				= 128
		,MAGIKARP			= 129
		,GYARADOS			= 130
		,LAPRAS				= 131
		,DITTO				= 132
		,EEVEE				= 133
		,VAPOREON			= 134
		,JOLTEON			= 135
		,FLAREON			= 136
		,PORYGON			= 137
		,OMANYTE			= 138
		,OMASTAR			= 139
		,KABUTO				= 140
		,KABUTOPS			= 141
		,AERODACTYL			= 142
		,SNORLAX			= 143
		,ARTICUNO			= 144
		,ZAPDOS				= 145
		,MOLTRES			= 146
		,DRATINI			= 147
		,DRAGONAIR			= 148
		,DRAGONITE			= 149
		,MEWTWO				= 150
		,MEW				= 151
		,CHIKORITA			= 152
		,BAYLEEF			= 153
		,MEGANIUM			= 154
		,CYNDAQUIL			= 155
		,QUILAVA			= 156
		,TYPHLOSION			= 157
		,TOTODILE			= 158
		,CROCONAW			= 159
		,FERALIGATR			= 160
		,SENTRET			= 161
		,FURRET				= 162
		,HOOTHOOT			= 163
		,NOCTOWL			= 164
		,LEDYBA				= 165
		,LEDIAN				= 166
		,SPINARAK			= 167
		,ARIADOS			= 168
		,CROBAT				= 169
		,CHINCHOU			= 170
		,LANTURN			= 171
		,PICHU				= 172
		,CLEFFA				= 173
		,IGGLYBUFF			= 174
		,TOGEPI				= 175
		,TOGETIC			= 176
		,NATU				= 177
		,XATU				= 178
		,MAREEP				= 179
		,FLAAFFY			= 180
		,AMPHAROS			= 181
		,BELLOSSOM			= 182
		,MARILL				= 183
		,AZUMARILL			= 184
		,SUDOWOODO			= 185
		,POLITOED			= 186
		,HOPPIP				= 187
		,SKIPLOOM			= 188
		,JUMPLUFF			= 189
		,AIPOM				= 190
		,SUNKERN			= 191
		,SUNFLORA			= 192
		,YANMA				= 193
		,WOOPER				= 194
		,QUAGSIRE			= 195
		,ESPEON				= 196
		,UMBREON			= 197
		,MURKROW			= 198
		,SLOWKING			= 199
		,MISDREAVUS			= 200
		,UNOWN_A			= 201
		,WOBBUFFET			= 202
		,GIRAFARIG			= 203
		,PINECO				= 204
		,FORRETRESS			= 205
		,DUNSPARCE			= 206
		,GLIGAR				= 207
		,STEELIX			= 208
		,SNUBBULL			= 209
		,GRANBULL			= 210
		,QWILFISH			= 211
		,SCIZOR				= 212
		,SHUCKLE			= 213
		,HERACROSS			= 214
		,SNEASEL			= 215
		,TEDDIURSA			= 216
		,URSARING			= 217
		,SLUGMA				= 218
		,MAGCARGO			= 219
		,SWINUB				= 220
		,PILOSWINE			= 221
		,CORSOLA			= 222
		,REMORAID			= 223
		,OCTILLERY			= 224
		,DELIBIRD			= 225
		,MANTINE			= 226
		,SKARMORY			= 227
		,HOUNDOUR			= 228
		,HOUNDOOM			= 229
		,KINGDRA			= 230
		,PHANPY				= 231
		,DONPHAN			= 232
		,PORYGON2			= 233
		,STANTLER			= 234
		,SMEARGLE			= 235
		,TYROGUE			= 236
		,HITMONTOP			= 237
		,SMOOCHUM			= 238
		,ELEKID				= 239
		,MAGBY				= 240
		,MILTANK			= 241
		,BLISSEY			= 242
		,RAIKOU				= 243
		,ENTEI				= 244
		,SUICUNE			= 245
		,LARVITAR			= 246
		,PUPITAR			= 247
		,TYRANITAR			= 248
		,LUGIA				= 249
		,HO_OH				= 250
		,CELEBI				= 251
		,TREECKO			= 252
		,GROVYLE			= 253
		,SCEPTILE			= 254
		,TORCHIC			= 255
		,COMBUSKEN			= 256
		,BLAZIKEN			= 257
		,MUDKIP				= 258
		,MARSHTOMP			= 259
		,SWAMPERT			= 260
		,POOCHYENA			= 261
		,MIGHTYENA			= 262
		,ZIGZAGOON			= 263
		,LINOONE			= 264
		,WURMPLE			= 265
		,SILCOON			= 266
		,BEAUTIFLY			= 267
		,CASCOON			= 268
		,DUSTOX				= 269
		,LOTAD				= 270
		,LOMBRE				= 271
		,LUDICOLO			= 272
		,SEEDOT				= 273
		,NUZLEAF			= 274
		,SHIFTRY			= 275
		,TAILLOW			= 276
		,SWELLOW			= 277
		,WINGULL			= 278
		,PELIPPER			= 279
		,RALTS				= 280
		,KIRLIA				= 281
		,GARDEVOIR			= 282
		,SURSKIT			= 283
		,MASQUERAIN			= 284
		,SHROOMISH			= 285
		,BRELOOM			= 286
		,SLAKOTH			= 287
		,VIGOROTH			= 288
		,SLAKING			= 289
		,NINCADA			= 290
		,NINJASK			= 291
		,SHEDINJA			= 292
		,WHISMUR			= 293
		,LOUDRED			= 294
		,EXPLOUD			= 295
		,MAKUHITA			= 296
		,HARIYAMA			= 297
		,AZURILL			= 298
		,NOSEPASS			= 299
		,SKITTY				= 300
		,DELCATTY			= 301
		,SABLEYE			= 302
		,MAWILE				= 303
		,ARON				= 304
		,LAIRON				= 305
		,AGGRON				= 306
		,MEDITITE			= 307
		,MEDICHAM			= 308
		,ELECTRIKE			= 309
		,MANECTRIC			= 310
		,PLUSLE				= 311
		,MINUN				= 312
		,VOLBEAT			= 313
		,ILLUMISE			= 314
		,ROSELIA			= 315
		,GULPIN				= 316
		,SWALOT				= 317
		,CARVANHA			= 318
		,SHARPEDO			= 319
		,WAILMER			= 320
		,WAILORD			= 321
		,NUMEL				= 322
		,CAMERUPT			= 323
		,TORKOAL			= 324
		,SPOINK				= 325
		,GRUMPIG			= 326
		,SPINDA				= 327
		,TRAPINCH			= 328
		,VIBRAVA			= 329
		,FLYGON				= 330
		,CACNEA				= 331
		,CACTURNE			= 332
		,SWABLU				= 333
		,ALTARIA			= 334
		,ZANGOOSE			= 335
		,SEVIPER			= 336
		,LUNATONE			= 337
		,SOLROCK			= 338
		,BARBOACH			= 339
		,WHISCASH			= 340
		,CORPHISH			= 341
		,CRAWDAUNT			= 342
		,BALTOY				= 343
		,CLAYDOL			= 344
		,LILEEP				= 345
		,CRADILY			= 346
		,ANORITH			= 347
		,ARMALDO			= 348
		,FEEBAS				= 349
		,MILOTIC			= 350
		,CASTFORM			= 351
		,KECLEON			= 352
		,SHUPPET			= 353
		,BANETTE			= 354
		,DUSKULL			= 355
		,DUSCLOPS			= 356
		,TROPIUS			= 357
		,CHIMECHO			= 358
		,ABSOL				= 359
		,WYNAUT				= 360
		,SNORUNT			= 361
		,GLALIE				= 362
		,SPHEAL				= 363
		,SEALEO				= 364
		,WALREIN			= 365
		,CLAMPERL			= 366
		,HUNTAIL			= 367
		,GOREBYSS			= 368
		,RELICANTH			= 369
		,LUVDISC			= 370
		,BAGON				= 371
		,SHELGON			= 372
		,SALAMENCE			= 373
		,BELDUM				= 374
		,METANG				= 375
		,METAGROSS			= 376
		,REGIROCK			= 377
		,REGICE				= 378
		,REGISTEEL			= 379
		,LATIAS				= 380
		,LATIOS				= 381
		,KYOGRE				= 382
		,GROUDON			= 383
		,RAYQUAZA			= 384
		,JIRACHI			= 385
		,DEOXYS_NORMAL		= 386
		,TURTWIG			= 387
		,GROTLE				= 388
		,TORTERRA			= 389
		,CHIMCHAR			= 390
		,MONFERNO			= 391
		,INFERNAPE			= 392
		,PIPLUP				= 393
		,PRINPLUP			= 394
		,EMPOLEON			= 395
		,STARLY				= 396
		,STARAVIA			= 397
		,STARAPTOR			= 398
		,BIDOOF				= 399
		,BIBAREL			= 400
		,KRICKETOT			= 401
		,KRICKETUNE			= 402
		,SHINX				= 403
		,LUXIO				= 404
		,LUXRAY				= 405
		,BUDEW				= 406
		,ROSERADE			= 407
		,CRANIDOS			= 408
		,RAMPARDOS			= 409
		,SHIELDON			= 410
		,BASTIODON			= 411
		,BURMY_PLANT		= 412
		,WORMADAM_PLANT		= 413
		,MOTHIM_PLANT		= 414
		,COMBEE				= 415
		,VESPIQUEN			= 416
		,PACHIRISU			= 417
		,BUIZEL				= 418
		,FLOATZEL			= 419
		,CHERUBI			= 420
		,CHERRIM_OVERCAST	= 421
		,SHELLOS_WEST		= 422
		,GASTRODON_WEST		= 423
		,AMBIPOM			= 424
		,DRIFLOON			= 425
		,DRIFBLIM			= 426
		,BUNEARY			= 427
		,LOPUNNY			= 428
		,MISMAGIUS			= 429
		,HONCHKROW			= 430
		,GLAMEOW			= 431
		,PURUGLY			= 432
		,CHINGLING			= 433
		,STUNKY			= 434
		,SKUNTANK			= 435
		,BRONZOR			= 436
		,BRONZONG			= 437
		,BONSLY			= 438
		,MIME_JR			= 439
		,HAPPINY			= 440
		,CHATOT			= 441
		,SPIRITOMB			= 442
		,GIBLE			= 443
		,GABITE			= 444
		,GARCHOMP			= 445
		,MUNCHLAX			= 446
		,RIOLU			= 447
		,LUCARIO			= 448
		,HIPPOPOTAS			= 449
		,HIPPOWDON			= 450
		,SKORUPI			= 451
		,DRAPION			= 452
		,CROAGUNK			= 453
		,TOXICROAK			= 454
		,CARNIVINE			= 455
		,FINNEON			= 456
		,LUMINEON			= 457
		,MANTYKE			= 458
		,SNOVER			= 459
		,ABOMASNOW			= 460
		,WEAVILE			= 461
		,MAGNEZONE			= 462
		,LICKILICKY			= 463
		,RHYPERIOR			= 464
		,TANGROWTH			= 465
		,ELECTIVIRE			= 466
		,MAGMORTAR			= 467
		,TOGEKISS			= 468
		,YANMEGA			= 469
		,LEAFEON			= 470
		,GLACEON			= 471
		,GLISCOR			= 472
		,MAMOSWINE			= 473
		,PORYGON_Z			= 474
		,GALLADE			= 475
		,PROBOPASS			= 476
		,DUSKNOIR			= 477
		,FROSLASS			= 478
		,ROTOM			= 479
		,UXIE			= 480
		,MESPRIT			= 481
		,AZELF			= 482
		,DIALGA			= 483
		,PALKIA			= 484
		,HEATRAN			= 485
		,REGIGIGAS			= 486
		,GIRATINA_ALTERED			= 487
		,CRESSELIA			= 488
		,PHIONE			= 489
		,MANAPHY			= 490
		,DARKRAI			= 491
		,SHAYMIN_LAND			= 492
		,ARCEUS_NORMAL			= 493
		,VICTINI			= 494
		,SNIVY			= 495
		,SERVINE			= 496
		,SERPERIOR			= 497
		,TEPIG			= 498
		,PIGNITE			= 499
		,EMBOAR			= 500
		,OSHAWOTT			= 501
		,DEWOTT			= 502
		,SAMUROTT			= 503
		,PATRAT			= 504
		,WATCHOG			= 505
		,LILLIPUP			= 506
		,HERDIER			= 507
		,STOUTLAND			= 508
		,PURRLOIN			= 509
		,LIEPARD			= 510
		,PANSAGE			= 511
		,SIMISAGE			= 512
		,PANSEAR			= 513
		,SIMISEAR			= 514
		,PANPOUR			= 515
		,SIMIPOUR			= 516
		,MUNNA			= 517
		,MUSHARNA			= 518
		,PIDOVE			= 519
		,TRANQUILL			= 520
		,UNFEZANT			= 521
		,BLITZLE			= 522
		,ZEBSTRIKA			= 523
		,ROGGENROLA			= 524
		,BOLDORE			= 525
		,GIGALITH			= 526
		,WOOBAT			= 527
		,SWOOBAT			= 528
		,DRILBUR			= 529
		,EXCADRILL			= 530
		,AUDINO			= 531
		,TIMBURR			= 532
		,GURDURR			= 533
		,CONKELDURR			= 534
		,TYMPOLE			= 535
		,PALPITOAD			= 536
		,SEISMITOAD			= 537
		,THROH			= 538
		,SAWK			= 539
		,SEWADDLE			= 540
		,SWADLOON			= 541
		,LEAVANNY			= 542
		,VENIPEDE			= 543
		,WHIRLIPEDE			= 544
		,SCOLIPEDE			= 545
		,COTTONEE			= 546
		,WHIMSICOTT			= 547
		,PETILIL			= 548
		,LILLIGANT			= 549
		,BASCULIN_RED_STRIPED			= 550
		,SANDILE			= 551
		,KROKOROK			= 552
		,KROOKODILE			= 553
		,DARUMAKA			= 554
		,DARMANITAN_STANDARD			= 555
		,MARACTUS			= 556
		,DWEBBLE			= 557
		,CRUSTLE			= 558
		,SCRAGGY			= 559
		,SCRAFTY			= 560
		,SIGILYPH			= 561
		,YAMASK			= 562
		,COFAGRIGUS			= 563
		,TIRTOUGA			= 564
		,CARRACOSTA			= 565
		,ARCHEN			= 566
		,ARCHEOPS			= 567
		,TRUBBISH			= 568
		,GARBODOR			= 569
		,ZORUA			= 570
		,ZOROARK			= 571
		,MINCCINO			= 572
		,CINCCINO			= 573
		,GOTHITA			= 574
		,GOTHORITA			= 575
		,GOTHITELLE			= 576
		,SOLOSIS			= 577
		,DUOSION			= 578
		,REUNICLUS			= 579
		,DUCKLETT			= 580
		,SWANNA			= 581
		,VANILLITE			= 582
		,VANILLISH			= 583
		,VANILLUXE			= 584
		,DEERLING_SPRING			= 585
		,SAWSBUCK_SPRING			= 586
		,EMOLGA			= 587
		,KARRABLAST			= 588
		,ESCAVALIER			= 589
		,FOONGUS			= 590
		,AMOONGUSS			= 591
		,FRILLISH			= 592
		,JELLICENT			= 593
		,ALOMOMOLA			= 594
		,JOLTIK			= 595
		,GALVANTULA			= 596
		,FERROSEED			= 597
		,FERROTHORN			= 598
		,KLINK			= 599
		,KLANG			= 600
		,KLINKLANG			= 601
		,TYNAMO			= 602
		,EELEKTRIK			= 603
		,EELEKTROSS			= 604
		,ELGYEM			= 605
		,BEHEEYEM			= 606
		,LITWICK			= 607
		,LAMPENT			= 608
		,CHANDELURE			= 609
		,AXEW			= 610
		,FRAXURE			= 611
		,HAXORUS			= 612
		,CUBCHOO			= 613
		,BEARTIC			= 614
		,CRYOGONAL			= 615
		,SHELMET			= 616
		,ACCELGOR			= 617
		,STUNFISK			= 618
		,MIENFOO			= 619
		,MIENSHAO			= 620
		,DRUDDIGON			= 621
		,GOLETT			= 622
		,GOLURK			= 623
		,PAWNIARD			= 624
		,BISHARP			= 625
		,BOUFFALANT			= 626
		,RUFFLET			= 627
		,BRAVIARY			= 628
		,VULLABY			= 629
		,MANDIBUZZ			= 630
		,HEATMOR			= 631
		,DURANT			= 632
		,DEINO			= 633
		,ZWEILOUS			= 634
		,HYDREIGON			= 635
		,LARVESTA			= 636
		,VOLCARONA			= 637
		,COBALION			= 638
		,TERRAKION			= 639
		,VIRIZION			= 640
		,TORNADUS_INCARNATE			= 641
		,THUNDURUS_INCARNATE			= 642
		,RESHIRAM			= 643
		,ZEKROM			= 644
		,LANDORUS_INCARNATE			= 645
		,KYUREM			= 646
		,KELDEO_ORDINARY			= 647
		,MELOETTA_ARIA			= 648
		,GENESECT			= 649
		,CHESPIN			= 650
		,QUILLADIN			= 651
		,CHESNAUGHT			= 652
		,FENNEKIN			= 653
		,BRAIXEN			= 654
		,DELPHOX			= 655
		,FROAKIE			= 656
		,FROGADIER			= 657
		,GRENINJA			= 658
		,BUNNELBY			= 659
		,DIGGERSBY			= 660
		,FLETCHLING			= 661
		,FLETCHINDER			= 662
		,TALONFLAME			= 663
		,SCATTERBUG_ICY_SNOW			= 664
		,SPEWPA_ICY_SNOW			= 665
		,VIVILLON_MEADOW			= 666
		,LITLEO			= 667
		,PYROAR			= 668
		,FLABEBE_RED			= 669
		,FLOETTE_RED			= 670
		,FLORGES_RED			= 671
		,SKIDDO			= 672
		,GOGOAT			= 673
		,PANCHAM			= 674
		,PANGORO			= 675
		,FURFROU_NATURAL			= 676
		,ESPURR			= 677
		,MEOWSTIC_MALE			= 678
		,HONEDGE			= 679
		,DOUBLADE			= 680
		,AEGISLASH_SHIELD			= 681
		,SPRITZEE			= 682
		,AROMATISSE			= 683
		,SWIRLIX			= 684
		,SLURPUFF			= 685
		,INKAY			= 686
		,MALAMAR			= 687
		,BINACLE			= 688
		,BARBARACLE			= 689
		,SKRELP			= 690
		,DRAGALGE			= 691
		,CLAUNCHER			= 692
		,CLAWITZER			= 693
		,HELIOPTILE			= 694
		,HELIOLISK			= 695
		,TYRUNT			= 696
		,TYRANTRUM			= 697
		,AMAURA			= 698
		,AURORUS			= 699
		,SYLVEON			= 700
		,HAWLUCHA			= 701
		,DEDENNE			= 702
		,CARBINK			= 703
		,GOOMY			= 704
		,SLIGGOO			= 705
		,GOODRA			= 706
		,KLEFKI			= 707
		,PHANTUMP			= 708
		,TREVENANT			= 709
		,PUMPKABOO_AVERAGE			= 710
		,GOURGEIST_AVERAGE			= 711
		,BERGMITE			= 712
		,AVALUGG			= 713
		,NOIBAT			= 714
		,NOIVERN			= 715
		,XERNEAS_ACTIVE			= 716
		,YVELTAL			= 717
		,ZYGARDE			= 718
		,DIANCIE			= 719
		,HOOPA			= 720
		,VOLCANION			= 721
		,ROWLET			= 722
		,DARTRIX			= 723
		,DECIDUEYE			= 724
		,LITTEN			= 725
		,TORRACAT			= 726
		,INCINEROAR			= 727
		,POPPLIO			= 728
		,BRIONNE			= 729
		,PRIMARINA			= 730
		,PIKIPEK			= 731
		,TRUMBEAK			= 732
		,TOUCANNON			= 733
		,YUNGOOS			= 734
		,GUMSHOOS			= 735
		,GRUBBIN			= 736
		,CHARJABUG			= 737
		,VIKAVOLT			= 738
		,CRABRAWLER			= 739
		,CRABOMINABLE			= 740
		,ORICORIO_BAILE			= 741
		,CUTIEFLY			= 742
		,RIBOMBEE			= 743
		,ROCKRUFF			= 744
		,LYCANROC_MIDDAY			= 745
		,WISHIWASHI_SOLO			= 746
		,MAREANIE			= 747
		,TOXAPEX			= 748
		,MUDBRAY			= 749
		,MUDSDALE			= 750
		,DEWPIDER			= 751
		,ARAQUANID			= 752
		,FOMANTIS			= 753
		,LURANTIS			= 754
		,MORELULL			= 755
		,SHIINOTIC			= 756
		,SALANDIT			= 757
		,SALAZZLE			= 758
		,STUFFUL			= 759
		,BEWEAR			= 760
		,BOUNSWEET			= 761
		,STEENEE			= 762
		,TSAREENA			= 763
		,COMFEY			= 764
		,ORANGURU			= 765
		,PASSIMIAN			= 766
		,WIMPOD			= 767
		,GOLISOPOD			= 768
		,SANDYGAST			= 769
		,PALOSSAND			= 770
		,PYUKUMUKU			= 771
		,TYPE_NULL			= 772
		,SILVALLY_NORMAL			= 773
		,MINIOR_RED_METEOR			= 774
		,KOMALA			= 775
		,TURTONATOR			= 776
		,TOGEDEMARU			= 777
		,MIMIKYU_DISGUISED			= 778
		,BRUXISH			= 779
		,DRAMPA			= 780
		,DHELMISE			= 781
		,JANGMO_O			= 782
		,HAKAMO_O			= 783
		,KOMMO_O			= 784
		,TAPU_KOKO			= 785
		,TAPU_LELE			= 786
		,TAPU_BULU			= 787
		,TAPU_FINI			= 788
		,COSMOG			= 789
		,COSMOEM			= 790
		,SOLGALEO			= 791
		,LUNALA			= 792
		,NIHILEGO			= 793
		,BUZZWOLE			= 794
		,PHEROMOSA			= 795
		,XURKITREE			= 796
		,CELESTEELA			= 797
		,KARTANA			= 798
		,GUZZLORD			= 799
		,NECROZMA			= 800
		,MAGEARNA			= 801
		,MARSHADOW			= 802
		,POIPOLE			= 803
		,NAGANADEL			= 804
		,STAKATAKA			= 805
		,BLACEPHALON			= 806
		,ZERAORA			= 807
		,UNOWN_B			= 10001
		,UNOWN_C			= 10002
		,UNOWN_D			= 10003
		,UNOWN_E			= 10004
		,UNOWN_F			= 10005
		,UNOWN_G			= 10006
		,UNOWN_H			= 10007
		,UNOWN_I			= 10008
		,UNOWN_J			= 10009
		,UNOWN_K			= 10010
		,UNOWN_L			= 10011
		,UNOWN_M			= 10012
		,UNOWN_N			= 10013
		,UNOWN_O			= 10014
		,UNOWN_P			= 10015
		,UNOWN_Q			= 10016
		,UNOWN_R			= 10017
		,UNOWN_S			= 10018
		,UNOWN_T			= 10019
		,UNOWN_U			= 10020
		,UNOWN_V			= 10021
		,UNOWN_W			= 10022
		,UNOWN_X			= 10023
		,UNOWN_Y			= 10024
		,UNOWN_Z			= 10025
		,UNOWN_EXCLAMATION			= 10026
		,UNOWN_QUESTION			= 10027
		,CASTFORM_SUNNY			= 10028
		,CASTFORM_RAINY			= 10029
		,CASTFORM_SNOWY			= 10030
		,DEOXYS_ATTACK			= 10031
		,DEOXYS_DEFENSE			= 10032
		,DEOXYS_SPEED			= 10033
		,BURMY_SANDY			= 10034
		,BURMY_TRASH			= 10035
		,WORMADAM_SANDY			= 10036
		,WORMADAM_TRASH			= 10037
		,CHERRIM_SUNSHINE			= 10038
		,SHELLOS_EAST			= 10039
		,GASTRODON_EAST			= 10040
		,ARCEUS_BUG			= 10041
		,ARCEUS_DARK			= 10042
		,ARCEUS_DRAGON			= 10043
		,ARCEUS_ELECTRIC			= 10044
		,ARCEUS_FIGHTING			= 10045
		,ARCEUS_FIRE			= 10046
		,ARCEUS_FLYING			= 10047
		,ARCEUS_GHOST			= 10048
		,ARCEUS_GRASS			= 10049
		,ARCEUS_GROUND			= 10050
		,ARCEUS_ICE			= 10051
		,ARCEUS_POISON			= 10052
		,ARCEUS_PSYCHIC			= 10053
		,ARCEUS_ROCK			= 10054
		,ARCEUS_STEEL			= 10055
		,ARCEUS_WATER			= 10056
		,ARCEUS_UNKNOWN			= 10057
		,ROTOM_HEAT			= 10058
		,ROTOM_WASH			= 10059
		,ROTOM_FROST			= 10060
		,ROTOM_FAN			= 10061
		,ROTOM_MOW			= 10062
		,GIRATINA_ORIGIN			= 10063
		,SHAYMIN_SKY			= 10064
		,PICHU_SPIKY_EARED			= 10065
		,BASCULIN_BLUE_STRIPED			= 10066
		,DARMANITAN_ZEN			= 10067
		,DEERLING_SUMMER			= 10068
		,DEERLING_AUTUMN			= 10069
		,DEERLING_WINTER			= 10070
		,SAWSBUCK_SUMMER			= 10071
		,SAWSBUCK_AUTUMN			= 10072
		,SAWSBUCK_WINTER			= 10073
		,MELOETTA_PIROUETTE			= 10074
		,GENESECT_DOUSE			= 10075
		,GENESECT_SHOCK			= 10076
		,GENESECT_BURN			= 10077
		,GENESECT_CHILL			= 10078
		,TORNADUS_THERIAN			= 10079
		,THUNDURUS_THERIAN			= 10080
		,LANDORUS_THERIAN			= 10081
		,KYUREM_BLACK			= 10082
		,KYUREM_WHITE			= 10083
		,KELDEO_RESOLUTE			= 10084
		,ARCEUS_FAIRY			= 10085
		,VIVILLON_ICY_SNOW			= 10086
		,VIVILLON_POLAR			= 10087
		,VIVILLON_TUNDRA			= 10088
		,VIVILLON_CONTINENTAL			= 10089
		,VIVILLON_GARDEN			= 10090
		,VIVILLON_ELEGANT			= 10091
		,VIVILLON_MODERN			= 10092
		,VIVILLON_MARINE			= 10093
		,VIVILLON_ARCHIPELAGO			= 10094
		,VIVILLON_HIGH_PLAINS			= 10095
		,VIVILLON_SANDSTORM			= 10096
		,VIVILLON_RIVER			= 10097
		,VIVILLON_MONSOON			= 10098
		,VIVILLON_SAVANNA			= 10099
		,VIVILLON_SUN			= 10100
		,VIVILLON_OCEAN			= 10101
		,VIVILLON_JUNGLE			= 10102
		,FLABEBE_YELLOW			= 10103
		,FLABEBE_ORANGE			= 10104
		,FLABEBE_BLUE			= 10105
		,FLABEBE_WHITE			= 10106
		,FLOETTE_YELLOW			= 10107
		,FLOETTE_ORANGE			= 10108
		,FLOETTE_BLUE			= 10109
		,FLOETTE_WHITE			= 10110
		,FLORGES_YELLOW			= 10111
		,FLORGES_ORANGE			= 10112
		,FLORGES_BLUE			= 10113
		,FLORGES_WHITE			= 10114
		,FURFROU_HEART			= 10115
		,FURFROU_STAR			= 10116
		,FURFROU_DIAMOND			= 10117
		,FURFROU_DEBUTANTE			= 10118
		,FURFROU_MATRON			= 10119
		,FURFROU_DANDY			= 10120
		,FURFROU_LA_REINE			= 10121
		,FURFROU_KABUKI			= 10122
		,FURFROU_PHARAOH			= 10123
		,MEOWSTIC_FEMALE			= 10124
		,AEGISLASH_BLADE			= 10125
		,PUMPKABOO_SMALL			= 10126
		,PUMPKABOO_LARGE			= 10127
		,PUMPKABOO_SUPER			= 10128
		,GOURGEIST_SMALL			= 10129
		,GOURGEIST_LARGE			= 10130
		,GOURGEIST_SUPER			= 10131
		,XERNEAS_NEUTRAL			= 10132
		,VENUSAUR_MEGA			= 10133
		,CHARIZARD_MEGA_X			= 10134
		,CHARIZARD_MEGA_Y			= 10135
		,BLASTOISE_MEGA			= 10136
		,ALAKAZAM_MEGA			= 10137
		,GENGAR_MEGA			= 10138
		,KANGASKHAN_MEGA			= 10139
		,PINSIR_MEGA			= 10140
		,GYARADOS_MEGA			= 10141
		,AERODACTYL_MEGA			= 10142
		,MEWTWO_MEGA_X			= 10143
		,MEWTWO_MEGA_Y			= 10144
		,AMPHAROS_MEGA			= 10145
		,SCIZOR_MEGA			= 10146
		,HERACROSS_MEGA			= 10147
		,HOUNDOOM_MEGA			= 10148
		,TYRANITAR_MEGA			= 10149
		,BLAZIKEN_MEGA			= 10150
		,GARDEVOIR_MEGA			= 10151
		,MAWILE_MEGA			= 10152
		,AGGRON_MEGA			= 10153
		,MEDICHAM_MEGA			= 10154
		,MANECTRIC_MEGA			= 10155
		,BANETTE_MEGA			= 10156
		,ABSOL_MEGA			= 10157
		,GARCHOMP_MEGA			= 10158
		,LUCARIO_MEGA			= 10159
		,ABOMASNOW_MEGA			= 10160
		,VIVILLON_FANCY			= 10161
		,VIVILLON_POKE_BALL			= 10162
		,FLOETTE_ETERNAL			= 10163
		,LATIAS_MEGA			= 10164
		,LATIOS_MEGA			= 10165
		,SWAMPERT_MEGA			= 10166
		,SCEPTILE_MEGA			= 10167
		,SABLEYE_MEGA			= 10168
		,ALTARIA_MEGA			= 10169
		,GALLADE_MEGA			= 10170
		,AUDINO_MEGA			= 10171
		,SHARPEDO_MEGA			= 10172
		,SLOWBRO_MEGA			= 10173
		,STEELIX_MEGA			= 10174
		,PIDGEOT_MEGA			= 10175
		,GLALIE_MEGA			= 10176
		,DIANCIE_MEGA			= 10177
		,METAGROSS_MEGA			= 10178
		,KYOGRE_PRIMAL			= 10179
		,GROUDON_PRIMAL			= 10180
		,RAYQUAZA_MEGA			= 10181
		,PIKACHU_ROCK_STAR			= 10182
		,PIKACHU_BELLE			= 10183
		,PIKACHU_POP_STAR			= 10184
		,PIKACHU_PHD			= 10185
		,PIKACHU_LIBRE			= 10186
		,PIKACHU_COSPLAY			= 10187
		,HOOPA_UNBOUND			= 10188
		,CAMERUPT_MEGA			= 10189
		,LOPUNNY_MEGA			= 10190
		,SALAMENCE_MEGA			= 10191
		,BEEDRILL_MEGA			= 10192
		,RATTATA_ALOLA			= 10193
		,RATICATE_ALOLA			= 10194
		,RATICATE_TOTEM_ALOLA			= 10195
		,PIKACHU_ORIGINAL_CAP			= 10196
		,PIKACHU_HOENN_CAP			= 10197
		,PIKACHU_SINNOH_CAP			= 10198
		,PIKACHU_UNOVA_CAP			= 10199
		,PIKACHU_KALOS_CAP			= 10200
		,PIKACHU_ALOLA_CAP			= 10201
		,RAICHU_ALOLA			= 10202
		,SANDSHREW_ALOLA			= 10203
		,SANDSLASH_ALOLA			= 10204
		,VULPIX_ALOLA			= 10205
		,NINETALES_ALOLA			= 10206
		,DIGLETT_ALOLA			= 10207
		,DUGTRIO_ALOLA			= 10208
		,MEOWTH_ALOLA			= 10209
		,PERSIAN_ALOLA			= 10210
		,GEODUDE_ALOLA			= 10211
		,GRAVELER_ALOLA			= 10212
		,GOLEM_ALOLA			= 10213
		,GRIMER_ALOLA			= 10214
		,MUK_ALOLA			= 10215
		,EXEGGUTOR_ALOLA			= 10216
		,MAROWAK_ALOLA			= 10217
		,GRENINJA_BATTLE_BOND			= 10218
		,GRENINJA_ASH			= 10219
		,ZYGARDE_10			= 10220
		,ZYGARDE_50			= 10221
		,ZYGARDE_COMPLETE			= 10222
		,GUMSHOOS_TOTEM			= 10223
		,VIKAVOLT_TOTEM			= 10224
		,ORICORIO_POM_POM			= 10225
		,ORICORIO_PAU			= 10226
		,ORICORIO_SENSU			= 10227
		,LYCANROC_MIDNIGHT			= 10228
		,WISHIWASHI_SCHOOL			= 10229
		,LURANTIS_TOTEM			= 10230
		,SALAZZLE_TOTEM			= 10231
		,SILVALLY_FIGHTING			= 10232
		,SILVALLY_FLYING			= 10233
		,SILVALLY_POISON			= 10234
		,SILVALLY_GROUND			= 10235
		,SILVALLY_ROCK			= 10236
		,SILVALLY_BUG			= 10237
		,SILVALLY_GHOST			= 10238
		,SILVALLY_STEEL			= 10239
		,SILVALLY_FIRE			= 10240
		,SILVALLY_WATER			= 10241
		,SILVALLY_GRASS			= 10242
		,SILVALLY_ELECTRIC			= 10243
		,SILVALLY_PSYCHIC			= 10244
		,SILVALLY_ICE			= 10245
		,SILVALLY_DRAGON			= 10246
		,SILVALLY_DARK			= 10247
		,SILVALLY_FAIRY			= 10248
		,MINIOR_ORANGE_METEOR			= 10249
		,MINIOR_YELLOW_METEOR			= 10250
		,MINIOR_GREEN_METEOR			= 10251
		,MINIOR_BLUE_METEOR			= 10252
		,MINIOR_INDIGO_METEOR			= 10253
		,MINIOR_VIOLET_METEOR			= 10254
		,MINIOR_RED			= 10255
		,MINIOR_ORANGE			= 10256
		,MINIOR_YELLOW			= 10257
		,MINIOR_GREEN			= 10258
		,MINIOR_BLUE			= 10259
		,MINIOR_INDIGO			= 10260
		,MINIOR_VIOLET			= 10261
		,MIMIKYU_BUSTED			= 10262
		,MIMIKYU_TOTEM_DISGUISED			= 10263
		,MIMIKYU_TOTEM_BUSTED			= 10264
		,KOMMO_O_TOTEM			= 10265
		,MAGEARNA_ORIGINAL			= 10266
		,PIKACHU_PARTNER_CAP			= 10267
		,MAROWAK_TOTEM			= 10268
		,MOTHIM_SANDY			= 10269
		,MOTHIM_TRASH			= 10270
		,SCATTERBUG_POLAR			= 10271
		,SCATTERBUG_TUNDRA			= 10272
		,SCATTERBUG_CONTINENTAL			= 10273
		,SCATTERBUG_GARDEN			= 10274
		,SCATTERBUG_ELEGANT			= 10275
		,SCATTERBUG_MEADOW			= 10276
		,SCATTERBUG_MODERN			= 10277
		,SCATTERBUG_MARINE			= 10278
		,SCATTERBUG_ARCHIPELAGO			= 10279
		,SCATTERBUG_HIGH_PLAINS			= 10280
		,SCATTERBUG_SANDSTORM			= 10281
		,SCATTERBUG_RIVER			= 10282
		,SCATTERBUG_MONSOON			= 10283
		,SCATTERBUG_SAVANNA			= 10284
		,SCATTERBUG_SUN			= 10285
		,SCATTERBUG_OCEAN			= 10286
		,SCATTERBUG_JUNGLE			= 10287
		,SCATTERBUG_FANCY			= 10288
		,SCATTERBUG_POKE_BALL			= 10289
		,SPEWPA_POLAR			= 10290
		,SPEWPA_TUNDRA			= 10291
		,SPEWPA_CONTINENTAL			= 10292
		,SPEWPA_GARDEN			= 10293
		,SPEWPA_ELEGANT			= 10294
		,SPEWPA_MEADOW			= 10295
		,SPEWPA_MODERN			= 10296
		,SPEWPA_MARINE			= 10297
		,SPEWPA_ARCHIPELAGO			= 10298
		,SPEWPA_HIGH_PLAINS			= 10299
		,SPEWPA_SANDSTORM			= 10300
		,SPEWPA_RIVER			= 10301
		,SPEWPA_MONSOON			= 10302
		,SPEWPA_SAVANNA			= 10303
		,SPEWPA_SUN			= 10304
		,SPEWPA_OCEAN			= 10305
		,SPEWPA_JUNGLE			= 10306
		,SPEWPA_FANCY			= 10307
		,SPEWPA_POKE_BALL			= 10308
		,RIBOMBEE_TOTEM			= 10309
		,ROCKRUFF_OWN_TEMPO			= 10310
		,LYCANROC_DUSK			= 10311
		,ARAQUANID_TOTEM			= 10312
		,TOGEDEMARU_TOTEM			= 10313
		,NECROZMA_DUSK			= 10314
		,NECROZMA_DAWN			= 10315
		,NECROZMA_ULTRA			= 10316
	}
	#endregion
}