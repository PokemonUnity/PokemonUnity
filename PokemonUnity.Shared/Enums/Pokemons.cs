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
		UNSET
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
		,HARDY
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
		ATTACK,
		DEFENSE,
		SPEED,
		SPATK,
		SPDEF,
		HP
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
        /// </summary>
        /// Just wanted to see a requirement for after "fainting" too many times, your pokemon just died, and became a ghost-type...  
        Deaths
	}
	#endregion
}