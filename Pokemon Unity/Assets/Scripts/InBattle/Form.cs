using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Attack;
using PokemonUnity.Inventory;

namespace PokemonUnity.Battle
{
	//ToDo: Move to PokemonData Class?...
	//ToDo: Change Pokemon.Form from type `int` to `Form`
	public partial class Form : PokemonUnity.Monster.Pokemon.PokemonData
	{
		public Pokemons BaseSpecies { get; private set; }
		public bool IsMega { get; private set; }
		public Form(Pokemons Id = Pokemons.NONE, Pokemons baseSpecies = Pokemons.NONE, bool isMega = false, //int[] regionalDex = null, //string name, 
								Types type1 = Types.NONE, Types type2 = Types.NONE, Abilities ability1 = Abilities.NONE, Abilities ability2 = Abilities.NONE, Abilities hiddenAbility = Abilities.NONE,//Abilities[] abilities,
								GenderRatio genderRatio = GenderRatio.Genderless, float? maleRatio = null, int catchRate = 1, EggGroups eggGroup1 = EggGroups.NONE, EggGroups eggGroup2 = EggGroups.NONE, int hatchTime = 0,
								float height = 0f, float weight = 0f, int baseExpYield = 0, LevelingRate levelingRate = LevelingRate.MEDIUMFAST,
								//int? evYieldHP, int? evYieldATK, int? evYieldDEF, int? evYieldSPA, int? evYieldSPD, int? evYieldSPE,
								int evHP = 0, int evATK = 0, int evDEF = 0, int evSPA = 0, int evSPD = 0, int evSPE = 0,
								Color pokedexColor = Color.NONE, int baseFriendship = 0,// string species, string pokedexEntry,
								int baseStatsHP = 0, int baseStatsATK = 0, int baseStatsDEF = 0, int baseStatsSPA = 0, int baseStatsSPD = 0, int baseStatsSPE = 0,
								Rarity rarity = Rarity.Common, float luminance = 0f, //Color lightColor,
								PokemonMoveset[] movesetmoves = null,
								int[] movesetLevels = null, Moves[] movesetMoves = null, int[] tmList = null,
								IPokemonEvolution[] evolution = null,
								int[] evolutionID = null, int[] evolutionLevel = null, int[] evolutionMethod = null, //string[] evolutionRequirements,
								//ToDo: What if: `Pokemons form` to point back to base pokemon, and Pokemons.NONE, if they are the base form?
								//that way, we can assign values to pokemons with forms that give stat bonuses...
								//want to find a way to add pokemon froms from a different method. Maybe something like overwriting the `Database` values to match those of base pokemon for values that are duplicated.
								//Or I'll just add it at the bottom towards end of array using copy-paste method.
								Pokemons baseForm = Pokemons.NONE, //int forms = 0, 
								int[,] heldItem = null) :  base (Id)
		{
			PokemonUnity.Monster.Pokemon.PokemonData _base = GetPokemon(BaseSpecies);
			/*this.RegionalPokedex = _base.regionalDex;

			this.type1 = type1; //!= null ? (Types)type1 : Types.NONE;
			this.type2 = type2; //!= null ? (Types)type2 : Types.NONE;
			//this.ability = abilities;
			this.ability1 = (Abilities)ability1;
			this.ability2 = (Abilities)ability2;
			this.abilityh = (Abilities)hiddenAbility;

			this.MaleRatio = maleRatio.HasValue ? getGenderRatio(maleRatio.Value) : genderRatio; //ToDo: maleRatio; maybe `GenderRatio genderRatio(maleRatio);`
			this.CatchRate = catchRate;
			this.eggGroup1 = eggGroup1;
			this.eggGroup2 = eggGroup2;
			this.HatchTime = hatchTime;

			this.Height = height;
			this.Weight = weight;
			this.BaseExpYield = _base.baseExpYield;
			this.GrowthRate = (LevelingRate)_base.levelingRate; //== null ? (LevelingRate)levelingRate : LevelingRate.NONE;

			this.evYieldHP  = _base.evHP;
			this.evYieldATK = _base.evATK;
			this.evYieldDEF = _base.evDEF;
			this.evYieldSPA = _base.evSPA;
			this.evYieldSPD = _base.evSPD;
			this.evYieldSPE = _base.evSPE;

			this.BaseStatsHP  = _base.baseStatsHP;
			this.BaseStatsATK = _base.baseStatsATK;
			this.BaseStatsDEF = _base.baseStatsDEF;
			this.BaseStatsSPA = _base.baseStatsSPA;
			this.BaseStatsSPD = _base.baseStatsSPD;
			this.BaseStatsSPE = _base.baseStatsSPE;
			this.BaseFriendship = _base.baseFriendship;

			this.Rarity = _base.rarity;
			this.Luminance = _base.luminance;
			//this.lightColor = lightColor;
			this.PokedexColor = _base.pokedexColor | Color.NONE;

			//ToDo: wild pokemon held items not yet implemented
			this.HeldItem = _base.heldItem; //[item id,% chance]

			this.MoveTree = _base.MoveTree;//new PokemonMoveTree(movesetmoves);
			//this.MovesetLevels = movesetLevels;
			//this.MovesetMoves = movesetMoves; 
			//this.tmList = tmList; 

			this.Evolutions = _base.evolution ?? new IPokemonEvolution[0];
			//this.EvolutionID = evolutionID;
			//this.evolutionMethod = evolutionMethod; 
			//this.evolutionRequirements = evolutionRequirements;*/
		}

		//public static readonly PokemonUnity.Monster.Pokemon.PokemonData[] Mega; //{ get { if(_database == null) _database = LoadDatabase(); return _database; } private set; }
		public static readonly Form[] Forms; //{ get { if(_database == null) _database = LoadDatabase(); return _database; } private set; }
		static Form()
		{
			//ToDo: Add values for forms
			//Mega = new PokemonUnity.Monster.Pokemon.PokemonData[] {
			Forms = new Form[] {
			#region Mega Evolve Forms
        new Form(
                Id: Pokemons.VENUSAUR_MEGA ,
				baseForm: Pokemons.VENUSAUR ,
				isMega: true,
                //regionalDex: new int[]{3} ,
                type1: Types.GRASS ,
                type2: Types.POISON ,
                ability1: Abilities.THICK_FAT  ,
                maleRatio: 87.5f ,
                eggGroup1: EggGroups.MONSTER ,
                eggGroup2: EggGroups.GRASS ,
                height: 2.4f ,
                weight: 155.5f ,
                pokedexColor: Color.GREEN ,
                baseStatsHP: 80, baseStatsATK: 100, baseStatsDEF: 123, baseStatsSPA: 122, baseStatsSPD: 120, baseStatsSPE: 80   
			),

        new Form(
                Id: Pokemons.CHARIZARD_MEGA_X ,
				baseForm: Pokemons.CHARIZARD ,
				isMega: true,
                //regionalDex: new int[]{6} ,
                type1: Types.FIRE ,
                type2: Types.DRAGON ,
                ability1: Abilities.TOUGH_CLAWS  ,
                maleRatio: 87.5f ,
                eggGroup1: EggGroups.MONSTER ,
                eggGroup2: EggGroups.DRAGON ,
                height: 1.7f ,
                weight: 110.5f ,
                pokedexColor: Color.BLACK ,
                baseStatsHP: 78, baseStatsATK: 130, baseStatsDEF: 111, baseStatsSPA: 130, baseStatsSPD: 85, baseStatsSPE: 100   
			),

        new Form(
                Id: Pokemons.CHARIZARD_MEGA_Y ,
				baseForm: Pokemons.CHARIZARD ,
				isMega: true,
                //regionalDex: new int[]{6} ,
                type1: Types.FIRE ,
                type2: Types.DRAGON ,
                ability1: Abilities.TOUGH_CLAWS  ,
                maleRatio: 87.5f ,
                eggGroup1: EggGroups.MONSTER ,
                eggGroup2: EggGroups.DRAGON ,
                height: 1.7f ,
                weight: 110.5f ,
                pokedexColor: Color.BLACK ,
                baseStatsHP: 78, baseStatsATK: 130, baseStatsDEF: 111, baseStatsSPA: 130, baseStatsSPD: 85, baseStatsSPE: 100 
			)  
			#endregion
			};
		}
	}
}