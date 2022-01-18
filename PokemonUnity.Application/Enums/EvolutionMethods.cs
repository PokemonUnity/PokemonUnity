namespace PokemonUnity.Enums
{
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
	/// Ideas:
	/// Merge two or more existing methods together into a new one.
	/// Evolution that depends on the Pokémon's nature or form.
	/// Fusion evolution(e.g. for Magnemite/Slowpoke). Check that there is a Shellder in the party, and if so, delete it and evolve the levelled-up Slowpoke.
	/// Check how many EVs the Pokémon has, and allows evolution only if that amount is greater than or equal to the EV threshold value set by the parameter.
	/// Shiny-Random?
	public class EvolutionMethods : PokemonUnity.Shared.Enums.EvolutionMethods
	{
		public bool CanEvolveDuringBattle { get; private set; }
		protected EvolutionMethods(int id, string name, bool inBattle = false) : base(id, name) { CanEvolveDuringBattle = inBattle; }
		/// <summary>
		///	if pokemon's level is greater or equal to int level<para></para>
		/// <code>Level,int level</code>
		/// </summary>
		public static readonly EvolutionMethods Level = new EvolutionMethods(0, "Level");
		/// <summary>
		///	Exactly the same as <see cref="Level"/>, 
		///	except the Pokémon must also be male.<para></para>
		/// <code>Level,int level</code>
		/// </summary>
		/// <example>Burmy</example>
		public static readonly EvolutionMethods LevelMale = new EvolutionMethods(0, "LevelMale");
		/// <summary>
		///	Exactly the same as <see cref="Level"/>, 
		///	except the Pokémon must also be female.<para></para>
		/// <code>Level,int level</code>
		/// </summary>
		/// <example>Burmy, Combee</example>
		public static readonly EvolutionMethods LevelFemale = new EvolutionMethods(0, "LevelFemale");
		/// <summary>
		///	The Pokémon will evolve if certain level, and weather is  
		///	<seealso cref="Overworld.FieldWeathers.Rain"/> or
		///	<seealso cref="Overworld.FieldWeathers.Thunderstorm"/>
		///	</summary>
		public static readonly EvolutionMethods LevelRain = new EvolutionMethods(0, "LevelRain");
		public static readonly EvolutionMethods LevelDay = new EvolutionMethods(0, "LevelDay");
		public static readonly EvolutionMethods LevelNight = new EvolutionMethods(0, "LevelNight");
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
		public static readonly EvolutionMethods Item = new EvolutionMethods(0, "Item");
		/// <summary>
		///	Exactly the same as "<see cref="Item"/>", 
		///	except the Pokémon must also be male.
		/// </summary>
		/// <example>Kirlia</example>
		public static readonly EvolutionMethods ItemMale = new EvolutionMethods(0, "ItemMale");
		/// <summary>
		///	Exactly the same as "<see cref="Item"/>", 
		///	except the Pokémon must also be female.
		/// </summary>
		/// <example>Snorunt</example>
		public static readonly EvolutionMethods ItemFemale = new EvolutionMethods(0, "ItemFemale");
		/// <summary>
		///	The Pokémon will evolve immediately after it is 
		///	<seealso cref="Pokemon.ObtainedMethod.TRADED"/>
		///	</summary>
		/// <example>Boldore, Graveler, Gurdurr, Haunter, Kadabra, Machoke</example>
		public static readonly EvolutionMethods Trade = new EvolutionMethods(0, "Trade");
		/// <summary>
		///	Exactly the same as <see cref="Trade"/>, 
		///	except the Pokémon must also be holding a particular item 
		///	(named by the parameter). 
		///	That item is removed afterwards.
		///	</summary>
		/// <example>Clamperl, Dusclops, Electabuzz, Feebas, Magmar, 
		/// Onix, Porygon, Porygon2, Poliwhirl, Rhydon, Scyther, Seadra, 
		/// Slowpoke</example>
		public static readonly EvolutionMethods TradeItem = new EvolutionMethods(0, "TradeItem");
		/// <summary>
		/// Exactly the same as <see cref="Trade"/>, 
		/// except the Pokémon must have been traded for a Pokémon of a certain species 
		///	(named by the parameter).
		///	</summary>
		/// <example>Karrablast, Shelmet</example>
		public static readonly EvolutionMethods TradeSpecies = new EvolutionMethods(0, "TradeSpecies");
		/// <summary>
		///	if pokemon's happiness is greater or equal to 220.<para></para>
		///	Note: Happiness checks should come last before all other methods.
		///	</summary>
		/// <example>Azurill, Buneary, Chansey, Cleffa, Golbat, 
		/// Igglybuff, Munchlax, Pichu, Swadloon, Togepi, Woobat</example>
		public static readonly EvolutionMethods Happiness = new EvolutionMethods(0, "Happiness");
		/// <summary>
		///	Exactly the same as <see cref="Happiness"/>, 
		///	but will only evolve during the daytime.
		///	</summary>
		/// <example>Budew, Eevee, Riolu</example>
		public static readonly EvolutionMethods HappinessDay = new EvolutionMethods(0, "HappinessDay");
		/// <summary>
		///	Exactly the same as <see cref="Happiness"/>, 
		///	but will only evolve during the night-time.
		///	</summary>
		/// <example>Chingling, Eevee</example>
		public static readonly EvolutionMethods HappinessNight = new EvolutionMethods(0, "HappinessNight");
		/// <summary>
		///	This method is almost identical to <see cref="Happiness"/>, 
		///	with the sole changes of replacing the "greater than" sign 
		///	to a "less than" sign, and changing the threshold value. 
		///	If you use this method, 
		///	you may also want to make it easier to get a Pokémon to hate you in-game 
		///	(currently the only ways to do this are fainting and using herbal medicine, 
		///	which can easily be countered by the many more happiness-boosting methods).
		///	</summary>
		public static readonly EvolutionMethods Hatred = new EvolutionMethods(0, "Hatred");
		/// <summary>
		///	if time is between 9PM and 4AM time is "Night". else time is "Day".
		///	if time is equal to string dayNight (either Day, or Night).<para></para>
		///	<code>Time,DatetimeOffset/bool dayNight</code>
		/// </summary>
		public static readonly EvolutionMethods Time = new EvolutionMethods(0, "Time");
		/// <summary>
		///	if date is between Day-Month or maybe certain 1/4 (quarter) of the year.
		///	if date is equal to string season (either Summer, Winter, Spring, or Fall).<para></para>
		///	<code>Time,DatetimeOffset/bool season</code>
		/// </summary>
		/// is holiday to "spot-on" as an occasion or requirement for leveling-up?
		public static readonly EvolutionMethods Season = new EvolutionMethods(0, "Season");
		/// <summary>
		///	if pokemon's heldItem is equal to string itemName<para></para>
		/// <example>Item,string itemName</example>
		/// </summary>
		/// Holding a certain item after leveling-up?
		public static readonly EvolutionMethods HoldItem = new EvolutionMethods(0, "HoldItem");
		/// <summary>
		///	The Pokémon will evolve if it levels up during the daytime 
		///	while holding a particular item (named by the parameter).<para></para>
		/// <code>Item,string itemName</code>
		/// </summary>
		/// <example>Happiny</example>
		public static readonly EvolutionMethods HoldItemDay = new EvolutionMethods(0, "HoldItemDay");
		/// <summary>
		///	The Pokémon will evolve if it levels up during the night-time 
		///	while holding a particular item (named by the parameter).<para></para>
		/// <code>Item,string itemName</code>
		/// </summary>
		/// <example>Gligar, Sneasel</example>
		public static readonly EvolutionMethods HoldItemNight = new EvolutionMethods(0, "HoldItemNight");
		/// <summary>
		/// The Pokémon will evolve when it levels up, 
		/// if its beauty stat is greater than or equal to the parameter.
		/// </summary>
		/// <example>Feebas</example>
		public static readonly EvolutionMethods Beauty = new EvolutionMethods(0, "Beauty");
		/// <summary>
		///	The Pokémon will evolve if it levels up 
		///	while knowing a particular move 
		///	(named by the parameter).<para></para>
		/// <example>Move,string moveName</example>
		/// </summary>
		/// <example>Aipom, Bonsly, Lickitung, Mime Jr., Piloswine, Tangela, Yanma</example>
		public static readonly EvolutionMethods Move = new EvolutionMethods(0, "Move");
		/// <summary>
		///	The Pokémon will evolve if it levels up 
		///	while the player has a Pokémon of a certain species in their party 
		///	(named by the parameter). 
		///	The named Pokémon is unaffected.<para></para>
		/// <example>Pokemon,string pokemonName</example>
		/// </summary>
		/// <example>Mantyke, Mantine</example>
		/// if party contains a Remoraid
		public static readonly EvolutionMethods Party = new EvolutionMethods(0, "Party");
		/// <summary>
		///	The Pokémon will evolve if it levels up while 
		///	the player has a Pokémon of a certain type in their party 
		///	(named by the parameter). <para></para>
		/// <example>Type,string pokemonTypeName</example>
		/// </summary>
		/// <example>Pangoro</example>
		/// if party contains a dark pokemon
		public static readonly EvolutionMethods Type = new EvolutionMethods(0, "Type");
		/// <summary>
		/// The Pokémon will evolve when it levels up, 
		/// if the player is currently on the map given by the parameter.<para></para>
		///	<code>Map,string mapName</code>
		/// </summary>
		public static readonly EvolutionMethods Location = new EvolutionMethods(0, "Location");
		///	<summary>
		///	The Pokémon will evolve if it levels up only during a certain kind of overworld weather.
		///	</summary>
		/// <example>Goodra</example>
		///	if currentMap's weather is rain
		public static readonly EvolutionMethods Weather = new EvolutionMethods(0, "Weather");
		///	<summary>
		///	Exactly the same as <see cref="Level"/>, 
		///	except the Pokémon's Attack stat must also be greater than its Defense stat.
		///	</summary>
		/// <example>Hitmonlee</example>
		public static readonly EvolutionMethods AttackGreater = new EvolutionMethods(0, "AttackGreater");
		///	<summary>
		///	Exactly the same as <see cref="Level"/>, 
		///	except the Pokémon's Attack stat must also be lower than its Defense stat.
		///	</summary>
		/// <example>Hitmonchan</example>
		public static readonly EvolutionMethods DefenseGreater = new EvolutionMethods(0, "DefenseGreater");
		/// <summary>
		/// Exactly the same as <see cref="Level"/>, 
		/// except the Pokémon's Attack stat must also be equal to its Defense stat.
		/// </summary>
		/// <example>Hitmontop</example>
		public static readonly EvolutionMethods AtkDefEqual = new EvolutionMethods(0, "AtkDefEqual");
		///	<summary>if pokemon's shinyValue divided by 2's remainder is equal to 0 or 1</summary>
		///	What about level parameter? Maybe "ShinyYes/ShinyNo" or "Shiny0/Shiny1", in combination with Level or Happiness-parameter? 
		public static readonly EvolutionMethods Shiny = new EvolutionMethods(0, "Shiny");
		/// <summary>Unique evolution methods: if pokemon's shinyValue divided by 2's remainder is equal to 0</summary>
		/// Shiny value? I thought it was based on "Friendship"
		public static readonly EvolutionMethods Silcoon = new EvolutionMethods(0, "Silcoon");
		///	<summary>
		///	Unique evolution methods: if pokemon's shinyValue divided by 2's remainder is equal to 1
		///	</summary>
		public static readonly EvolutionMethods Cascoon = new EvolutionMethods(0, "Cascoon");
		///	<summary>
		///	Unique evolution methods: 
		///	Exactly the same as <see cref="Level"/>. 
		///	There is no difference between the two methods at all. 
		///	Is used alongside the method <see cref="Shedinja"/>.
		///	</summary>
		public static readonly EvolutionMethods Ninjask = new EvolutionMethods(0, "Ninjask");
		///	<summary>
		///	Unique evolution methods: 
		///	Must be used with the method <see cref="Ninjask"/>. 
		///	Duplicates the Pokémon that just evolved 
		///	(if there is an empty space in the party), 
		///	and changes the duplicate's species to the given species.
		///	</summary>
		///	how is this different from "party"?
		public static readonly EvolutionMethods Shedinja = new EvolutionMethods(0, "Shedinja");
		///	<summary>
		///	Unique evolution methods: 
		///	This method is almost identical to <see cref="Level"/>. 
		///	However evolution only triggers if Pokemon has ability <see cref="Abilities.OWN_TEMPO"/>
		///	and time is Dusk (between hours of 5 pm to 5:59 pm in in-game time)
		///	</summary>
		public static readonly EvolutionMethods Lycanroc = new EvolutionMethods(0, "Lycanroc");
		/// <summary>
		///	The Pokémon will evolve if it levels up while 
		///	the Pokémon has a move of a certain type in their moveset 
		///	(named by the parameter). <para></para>
		/// <example>Type,string pokemonTypeName</example>
		/// </summary>
		/// <example>Sylveon</example>
		/// if pokemon contains a fairy-type move
		public static readonly EvolutionMethods Affection = new EvolutionMethods(0, "Affection");
		/// <summary>
		/// </summary>
		/// Just wanted to see a requirement for after "fainting" too many times, your pokemon just died, and became a ghost-type...  
		public static readonly EvolutionMethods Deaths = new EvolutionMethods(0, "Deaths");
	}
}