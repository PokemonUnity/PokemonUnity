# Defining a species

## Defining a species

A Pokémon species begins with its definition. This means that it is listed as an enum in the csharp file `Scripts2/Enum/Pokemons.cs`, so that it can be recognised by the game as a species.

Note that this only defines the basic properties common to all individual Pokémon of that species (e.g. base stats, move sets, evolution paths, etc.).

## Pokemon Database File "Scripts2/Pokemon/PokemonDatabase.cs"

The Pokemon Database File `Scripts2/Pokemon/PokemonDatabase.cs` lists all the defined Pokémon species in the game. Each section in this file is one separate species, where a section begins with a line containing an ID number in square brackets and ends when the next section begins. Each line in a section is one separate piece of information about that species.

Aside from the ID line, every line in a section follows the format:

```
X=Y
```

where X is a property and Y is the value or values associated with it. For example:

```csharp
new PokemonData(
                    Id: Pokemons.BULBASAUR ,
                    //regionalDex: new int[]{1} ,
                    type1: Types.GRASS ,
                    type2: Types.POISON ,
                    ability1: Abilities.OVERGROW  ,
                    hiddenAbility: Abilities.CHLOROPHYLL ,
                    maleRatio: 87.5f ,
                    catchRate: 45 ,
                    eggGroup1: EggGroups.MONSTER ,
                    eggGroup2: EggGroups.GRASS ,
                    hatchTime: 5140 ,
                    height: 0.7f ,
                    weight: 6.9f ,
                    levelingRate: LevelingRate.MEDIUMSLOW ,
                    pokedexColor: Color.GREEN ,
                    baseFriendship: 70 ,
                    baseExpYield: 64 ,
                    baseStatsHP: 45 ,baseStatsATK: 49 ,baseStatsDEF: 49 ,baseStatsSPA: 65 ,baseStatsSPD: 65 ,baseStatsSPE: 45    ,
                    evSPA: 1   ,
#region Learnable Moves
                    movesetmoves: new PokemonMoveset[] {
	                    new PokemonMoveset(
		                    moveId: Moves.VINE_WHIP,
		                    method: LearnMethod.levelup,
		                    level: 13
		                    //,generation: 1
	                    ), new PokemonMoveset(
		                    moveId: Moves.TACKLE,
		                    method: LearnMethod.levelup,
		                    level: 1
		                    //,generation: 1
	                    ),

	                    new PokemonMoveset(
		                    moveId: Moves.SWORDS_DANCE,
		                    method: LearnMethod.machine
	                    ), new PokemonMoveset(
		                    moveId: Moves.CUT,
		                    method: LearnMethod.machine
	                    ), new PokemonMoveset(
		                    moveId: Moves.HEADBUTT,
		                    method: LearnMethod.machine
	                    ),

	                    new PokemonMoveset(
		                    moveId: Moves.RAZOR_WIND,
		                    method: LearnMethod.egg
	                    ), new PokemonMoveset(
		                    moveId: Moves.PETAL_DANCE,
		                    method: LearnMethod.egg
	                    ), new PokemonMoveset(
		                    moveId: Moves.LIGHT_SCREEN,
		                    method: LearnMethod.egg
	                    ),

	                    new PokemonMoveset(
		                    moveId: Moves.SWORDS_DANCE,
		                    method: LearnMethod.tutor
	                    ), new PokemonMoveset(
		                    moveId: Moves.BIND,
		                    method: LearnMethod.tutor
	                    ), new PokemonMoveset(
		                    moveId: Moves.HEADBUTT,
		                    method: LearnMethod.tutor
	                    )
                    },
                    evolution: new IPokemonEvolution[] {
	                    new PokemonEvolution<int>(Pokemons.IVYSAUR, EvolutionMethod.Level, 16)
                    })
#endregion
```

Some pieces of information must be defined for every species, while other pieces of information are optional and can be excluded if they don't apply. The order of the lines does not matter.

Required information

Data | Description
--- | --- 
Id | This line must come first in a section, because, as mentioned above, this line is defined as one which begins a new section. This line contains a number inside square brackets, e.g. [42].
This number must be different for each species. It must be a whole number greater than or equal to 1 (it cannot be 0). While you can skip numbers, minor bugs can occur if you do so, so it is recommended that you don't. The order in which species are numbered is not important; however, keeping them in order will make it easier to spot gaps or mistakes.<br><br>This ID number is the default National Pokédex number of the species. If there are no Regional Pokédex lists defined, or if viewing the default Pokédex list in the Pokédex, that list will contain all the defined Pokémon species sorted by this number.
~~Name~~ | ~~The name of the species, as seen by the player.~~
InternalName | This is how the scripts refer to the species. Typically this is the species name, but written in all capital letters and with no spaces or symbols. The internal name is never seen by the player.
Type1<br>Type2 | The internal name(s) of the species' primary and secondary elemental types.<br>`Type2` is optional, and can be omitted if it does not apply to the species.<br>
BaseStats | Six comma-separated values, corresponding to (in order):<br><br>
1. HP
2. Attack
3. Defense
4. Speed
5. Special Attack
6. Special Defense<br>Each value can be between 0 and 255 inclusive.
GenderRate | The likelihood of a Pokémon of the species being a certain gender. Must be one of the following words:<br>
- AlwaysMale
- FemaleOneEighth
- Female25Percent
- Female50Percent
- Female75Percent
- FemaleSevenEighths
- AlwaysFemale
- Genderless
GrowthRate | The rate at which a Pokémon of the species gains levels (i.e. how much Experience is needed to level up). Must be one of the following words:<br>
- Fast
- Medium or MediumFast
- Slow
- Parabolic or MediumSlow
- Erratic
- Fluctuating<br>
BaseEXP | The base amount of Experience gained from defeating a Pokémon of the species. It must be a whole number between 0 and 65535 inclusive.<br>This base amount is used in a calculation to determine the actual number of Exp. points awarded for defeating a Pokémon of the species.
EffortPoints | The number of EVs gained by defeating a Pokémon of the species. Is six comma-separated numbers, corresponding to (in order):<br>
- HP
- Attack
- Defense
- Speed
- Special Attack
- Special Defense<br>As a rule, the total of these numbers should be between 1 and 3, and higher evolutions tend to give more EVs.
Rareness | The catch rate of the species. Is a number between 0 and 255 inclusive. The higher the number, the more likely a capture (0 means it cannot be caught by anything except a Master Ball).
Happiness | The amount of happiness a newly caught Pokémon of the species will have. Is a number between 0 and 255 inclusive, although it is typically 70.
Moves | The moves that all Pokémon of the species learn as they level up. There are two (comma-separated) parts to each move:<br>Level at which the move is learned (0 means the move can only be learned when a Pokémon evolves into the species).<br>The internal name of the move.<br><br>These couplets are also comma-separated in this line.
Compatibility | The egg groups that the species belongs to. Is either one or two (comma-separated) of the following words:<br>
- Monster
- Water1
- Bug
- Flying
- Field
- Fairy
- Grass
- Humanlike
- Water3
- Mineral
- Amorphous
- Water2
- Ditto
- Dragon
- Undiscovered<br><br>"Water1" is for sea creatures, "Water2" is for fish, and "Water3" is for shellfish. "Ditto" should contain only Ditto, as a species in that group can breed with any other breedable Pokémon. If either egg group is "Undiscovered", the species cannot breed.
StepsToHatch | The number of steps it takes to hatch an egg of the species. Note that this is not the number of egg cycles for the species, but the actual number of steps.
Height | The height of the species in meters, to one decimal place. Use a period for the decimal point, and do not use commas for thousands.<br>The Pokédex will automatically show this height in feet/inches if the game recognises that the player is in the USA. This is only cosmetic; the rest of the scripts still perform calculations using the meters value defined.<br>
Weight | The weight of the species in kilograms, to one decimal place. Use a period for the decimal point, and do not use commas for thousands.<br>The Pokédex will automatically show this weight in pounds if the game recognises that the player is in the USA. This is only cosmetic; the rest of the scripts still perform calculations using the kilograms value defined.
Color | The main colour of the species. Must be one of the colours defined in the script section PBColors, which by default are:<br>
- Black
- Blue
- Brown
- Gray
- Green
- Pink
- Purple
- Red
- White
- Yellow
Shape | The body shape of the species. The Pokédex can search for Pokémon of particular shapes. This is a number from 1 to 14 inclusive, as follows:<br>
- 1 = Only a head
- 2 = Serpent-like
- 3 = Fish
- 4 = Head and arms
- 5 = Head and base
- 6 = Bipedal with tail
- 7 = Head and legs
- 8 = Quadruped
- 9 = Has two wings
- 10 = Tentacles/multiple legs
- 11 = Multiple fused bodies
- 12 = Humanoid
- 13 = Winged insectoid
- 14 = Insectoid
Kind | The species' kind, which is displayed in the Pokédex. For example, Bulbasaur is the Seed Pokémon. The word "Pokémon" is automatically added to the end, so only "Seed" needs to be here.
~~Pokedex~~ | ~~The Pokédex entry.~~

Optional information

Data | Description
--- | --- 
Abilities | The internal name(s) of one or two abilities that the species can have. If there are two abilities, separate them with a comma.
HiddenAbility | The internal names of up to four additional abilities that the species can have. If there are multiple abilities here, they are separated by commas.<br>Pokémon cannot have any hidden ability naturally, and must be specially given one.
EggMoves | A comma-separated list of the internal names of moves that a Pokémon of the species can only learn as an egg (obtained through breeding). Only species that can be in eggs should have this line (typically only unevolved species).
Habitat | The kind of location that the species can typically be found in. Is one of the following words:<br>
- Cave
- Forest
- Grassland
- Mountain
- Rare
- RoughTerrain
- Sea
- Urban
- WatersEdge<br><br>"Rare" can be taken to mean "unknown" here.<br><br>This information is unused in Essentials.
RegionalNumbers | One or more comma-separated numbers. Each number is the Pokédex number of the species in the corresponding Regional Pokédex. A number of 0 means that the species does not appear in that Regional Pokédex.
- WildItemCommon
- WildItemUncommon
WildItemRare | The internal names of items that a wild Pokémon of the species may be found holding. Each line can only list one item.<br>The chances of holding the item are 50%, 5% and 1% respectively. If all three are the same item, then the chance of holding it is 100% instead.
BattlerPlayerY | Affects the positioning of the back sprite of the species in battle. A higher number means the back sprite is placed lower down the screen. Can be positive or negative, and is 0 by default.
BattlerEnemyY | Affects the positioning of the front sprite of the species in battle. A higher number means the front sprite is placed lower down the screen. Can be positive or negative, and is 0 by default.
BattlerAltitude | Affects the positioning of the front sprite of the species in battle relative to its base. A higher number means the front sprite is placed further up the screen. Can only be positive or 0, and is 0 by default.<br>If this value is greater than 0, the Pokémon's shadow is shown in battle, thus giving it the impression of being airborne. Pokémon without shadows look like they are grounded. This is cosmetic only, though, and doesn't relate to the state of being airborne which affects certain battle mechanics.
Evolutions | The evolution paths the species can take. For each possible evolution of the species, there are three parts:<br>The internal name of the evolved species.  <br>The evolution method. Must be one of the methods defined at the top of the script section Pokemon_Evolution, which by default are:
- Happiness - (-)
- HappinessDay - (-)
- HappinessNight - (-)
- Level - (level)
- Trade - (-)
- TradeItem - (item's internal name)
- Item - (item's internal name)
- AttackGreater - (level)
- AtkDefEqual - (level)
- DefenseGreater - (level)
- Silcoon - (level)
- Cascoon - (level)
- Ninjask - (level)
- Shedinja - (level)
- Beauty - (minimum beauty value)
- ItemMale - (item's internal name)
- ItemFemale - (item's internal name)
- DayHoldItem - (item's internal name)
- NightHoldItem - (item's internal name)
- HasMove - (move's internal name)
- HasInParty - (species' internal name)
- LevelMale - (level)
- LevelFemale - (level)
- Location - (map ID number)
- TradeSpecies - (species' internal name)
- LevelDay - (level)
- LevelNight - (level)
- LevelDarkInParty - (level)
- LevelRain - (level)
- HappinessMoveType - (elemental type's internal name)
- Custom 1-5 - (number between 0-65535)<br>The value/name as mentioned above.<br><br>See the page Evolution for more details.
FormName | The name of this form of the species (form 0), if it has one.<br>If this is blank, then its form name as shown in the Pokédex's Forms page will be "Male"/"Female" if the species is gendered. If the species is genderless, this name will be "Genderless" (if this is the only form for the species) or "One Form" (if the species also has other forms).
Incense | The internal name of an item that needs to be held by a parent when breeding in order for the egg to be the species. If neither parent is holding the required item, the egg will be the next evolved species instead.<br>The only species that should have this line are ones which cannot breed, but evolve into a species which can. That is, the species should be a "baby" species. Not all baby species need this line. Note that Essentials does not have any formal definition of what a "baby" species is.

## Graphics and audio

A Pokémon species has one of each of the following:

A 128x64 pixel two-frame icon, used mainly in the party screen and Pokémon storage screen
Four battle sprites, used in a variety of places in-game:
- Front normal
- Rear normal
- Front shiny
- Rear shiny
A 32x32 graphic depicting its footprint, for use in the Pokédex
An audio file depicting its cry, played in various places
The icon is composed of two 64x64 pictures side by side, and is animated automatically in the party screen (it is not animated in the Pokémon storage screen). It is placed in the folder "Graphics/Icons", with the name "iconXXX.png", where "XXX" is either the internal name of that species or its ID number padded to 3 digits (e.g. Bulbasaur is "001", Pikachu is "025", Mewtwo is "150").

The battle sprites can be any size, and are placed in the folder "Graphics/Battlers" with the following names (the "XXX" is as above):

- XXX.png - Front normal
- XXXb.png - Rear normal
- XXXs.png - Front shiny
- XXXsb.png - Rear shiny
The footprint graphic is placed in the folder "Graphics/Icons/Footprints" with the name "footprintXXX.png".

The cry file is placed in the folder "Audio/SE/Cries" with the name "XXXCry", and can be of any supported audio type.

## Multiple forms

Main article: Forms
Main article: Mega Evolution
If a Pokémon species has more than one form (including having mechanically differing male/female versions, and Mega Evolutions), then it will need additional graphics and possibly additional cries to depict them. Alternate forms are defined in the PBS file "pokemonforms.txt", which is laid out in much the same way as "pokemon.txt".