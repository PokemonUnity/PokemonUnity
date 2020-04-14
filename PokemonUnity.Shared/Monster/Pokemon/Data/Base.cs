using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Attack;
using PokemonUnity.Inventory;

namespace PokemonUnity.Monster.Data
{
	public struct PokemonData
    {
        #region Variables
        private Types type1;// = Types.NONE;
        private Types type2;// = Types.NONE;
		private Abilities ability1;// = Abilities.NONE;
        private Abilities ability2;// = Abilities.NONE;
        private Abilities abilityh;// = Abilities.NONE;
		private EggGroups eggGroup1;// = EggGroups.NONE;
        private EggGroups eggGroup2;// = EggGroups.NONE;
		#endregion

		#region Properties
		/// <summary>
		/// Id is the database value for specific pokemon+form
		/// Different Pokemon forms share the same Pokedex number. 
		/// Values are loaded from <see cref="Pokemons"/>, where each form is registered to an Id.
		/// </summary>
		/// <example>
		/// Deoxys Pokedex# can be 1,
		/// but Deoxys-Power id# can be 32
		/// </example>
		/// <remarks>If game event/gm wants to "give" player Deoxys-Power form and not Speed form</remarks>
		public Pokemons ID { get; private set; }
        /// <summary>
        /// Supposed to be the order the pokemons were released, 
        /// the order they would appear in if their pre-evolves were sorted,
        /// and also the order they're assigned for specific gen/region.
        /// <para></para>
        /// Zero's mean they're absent for that particular pokedex
        /// </summary>
        /// <example>
        /// Different Gens assign different pokedex num
        /// example: Bulbasaur = [1,231]
        /// </example>
        /// <remarks>Think there is 3 pokedex</remarks>
        //public int RegionalDex { get; private set; }
        public int[] RegionalPokedex { get; private set; }
        public byte GenerationId { get; private set; }
		public int EvoChainId { get; private set; }
		public int Order { get; private set; }
        public bool IsDefault { get; private set; }
        public bool FormSwitchable { get; private set; }
        public bool HasGenderDiff { get; private set; }
        public bool IsBaby { get; private set; }
        /// <summary>
        /// Best used in battle simulator. 
        /// Check to see if pokemons are legendary, and exclude from battle
        /// </summary>
        public Rarity Rarity { get; set; }
        public EggGroups[] EggGroup { get { return new EggGroups[] { this.eggGroup1, this.eggGroup2 }; } }
        public Types[] Type { get { return new Types[] { this.type1, this.type2 }; } }
        /// <summary>
        /// All three pokemon abilities 
        /// (Abiltiy1, Ability2, HiddenAbility).
        /// </summary>
        public Abilities[] Ability { get { return new Abilities[] { this.ability1, this.ability2, this.abilityh }; } }

        /// <summary>
        /// The male ratio.
        /// <value>-1f is interpreted as genderless</value>
        /// </summary>
        public GenderRatio GenderEnum { get; private set; }
        /// <summary>
        /// Returns whether this Pokemon species is restricted to only ever being one gender (or genderless)
        /// </summary>
        public bool IsSingleGendered
        {
            get
            {
                switch (GenderEnum)
                {
                    case GenderRatio.AlwaysMale:
                    case GenderRatio.AlwaysFemale:
                    case GenderRatio.Genderless:
                        return true;
                    default:
                        return false;
                }
            }
        }

        public float ShinyChance { get; set; }
        /// <summary>
        /// The catch rate of the species. 
        /// Is a number between 0 and 255 inclusive. 
        /// The higher the number, the more likely a capture 
        /// (0 means it cannot be caught by anything except a Master Ball).
        /// </summary> 
        /// Also known as Pokemon's "Rareness"...
        public int CatchRate { get; private set; }
        public int HatchTime { get; private set; }

        //public float hitboxWidth { get { return this.hitboxWidth; } } //used for 3d battles just use collision detection from models
        public float Height { get; private set; }
        public float Weight { get; private set; }
        public Shape ShapeId { get; private set; }
        public Habitat HabitatId { get; private set; }
        public LevelingRate GrowthRate { get; private set; }
        public Color PokedexColor { get; private set; }

        /// <summary>
        /// Friendship levels is the same as pokemon Happiness.
        /// </summary>
        public int BaseFriendship { get; private set; }
        public int BaseExpYield { get; private set; }
        public int BaseStatsHP { get; private set; }
        public int BaseStatsATK { get; private set; }
        public int BaseStatsDEF { get; private set; }
        public int BaseStatsSPA { get; private set; }
        public int BaseStatsSPD { get; private set; }
        public int BaseStatsSPE { get; private set; }
        public int evYieldHP { get; private set; }
        public int evYieldATK { get; private set; }
        public int evYieldDEF { get; private set; }
        public int evYieldSPA { get; private set; }
        public int evYieldSPD { get; private set; }
        public int evYieldSPE { get; private set; }
        /// <summary>
        /// Returns the items this species can be found holding in the wild.
        /// [item id,% chance]
        /// </summary>
        /// <example>int[,] heldItems = { {1,2,3},{4,5,6} }
        /// <para>heldItems[1,0] == 4 as int</para>
        /// </example>
        /// <remarks>
        /// Or maybe...[item id,% chance,generationId/regionId]
        /// Custom Class needed here... <see cref="Items"/> as itemId
        /// </remarks>
        /// ToDo: Consider [itemcommon || 0,itemuncommon || 0,itemrare || 0]
        public int[,] HeldItem { get; private set; }

        /*// <summary>
        /// </summary>
        /// Not quite sure what this is for...
        public float Luminance { get; private set; }
        /// <summary>
        /// All the moves this pokemon species can learn, and the methods by which they learn them
        /// </summary>
        public PokemonMoveTree MoveTree { get; private set; }
        /// ToDo: Evolution class type array here
        public IPokemonEvolution[] Evolutions { get; private set; }*/

        /// <summary>
        /// The item that needs to be held by a parent when breeding in order for the egg to be this species. 
        /// If neither parent is holding the required item, the egg will be the next evolved species instead.
        /// <para></para>
        /// The only species that should have this line are ones which cannot breed, 
        /// but evolve into a species which can. That is, the species should be a "baby" species.
        /// Not all baby species need this line.
        /// </summary>
        public Items Incense { get; private set; }
        public string Name { get { return ToString(TextScripts.Name); } }
        public string Description { get { return ToString(TextScripts.Description); } }
        #endregion

        #region Constructors
        public PokemonData(Pokemons Id = Pokemons.NONE, int[] regionalDex = null, 
                            Types type1 = Types.NONE, Types type2 = Types.NONE, Abilities ability1 = Abilities.NONE, Abilities ability2 = Abilities.NONE, Abilities hiddenAbility = Abilities.NONE,//Abilities[] abilities,
                            GenderRatio? genderRatio = GenderRatio.Genderless, float? maleRatio = null, int catchRate = 1, EggGroups eggGroup1 = EggGroups.NONE, EggGroups eggGroup2 = EggGroups.NONE, int hatchTime = 0,
                            float height = 0f, float weight = 0f, int baseExpYield = 0, LevelingRate levelingRate = LevelingRate.MEDIUMFAST,
                            int evHP = 0, int evATK = 0, int evDEF = 0, int evSPA = 0, int evSPD = 0, int evSPE = 0,
                            Color pokedexColor = Color.NONE, int baseFriendship = 0,
                            int baseStatsHP = 0, int baseStatsATK = 0, int baseStatsDEF = 0, int baseStatsSPA = 0, int baseStatsSPD = 0, int baseStatsSPE = 0,
                            Rarity rarity = Rarity.Common, float luminance = 0f,
                            PokemonMoveset[] movesetmoves = null,
                            int[] movesetLevels = null, Moves[] movesetMoves = null, int[] tmList = null,
                            IPokemonEvolution[] evolution = null,
                            int[] evolutionID = null, int[] evolutionLevel = null, int[] evolutionMethod = null, //string[] evolutionRequirements,
							Pokemons evolutionFROM = Pokemons.NONE, List<int> evolutionTO = null, Items incense = Items.NONE,
							int evoChainId = 0, byte generationId = 0, bool isDefault = false, bool isBaby = false, bool formSwitchable = false, bool hasGenderDiff = false, 
							Habitat habitatId = Habitat.RARE, Shape shapeId = Shape.BLOB, int order = -1,
							Pokemons baseForm = Pokemons.NONE, //int forms = 0, 
                            int[,] heldItem = null) //: this(Id)
        {
			this.ID = Id;
			this.RegionalPokedex = regionalDex;

            this.type1 = type1; //!= null ? (Types)type1 : Types.NONE;
            this.type2 = type2; //!= null ? (Types)type2 : Types.NONE;
            //this.ability = abilities;
            this.ability1 = (Abilities)ability1;
            this.ability2 = (Abilities)ability2;
            this.abilityh = (Abilities)hiddenAbility;

            this.CatchRate = catchRate;
            this.eggGroup1 = eggGroup1;
            this.eggGroup2 = eggGroup2;
            this.HatchTime = hatchTime;

            this.Height = height;
            this.Weight = weight;
            this.BaseExpYield = baseExpYield;
            this.GrowthRate = (LevelingRate)levelingRate; //== null ? (LevelingRate)levelingRate : LevelingRate.NONE;

            this.evYieldHP = evHP;
            this.evYieldATK = evATK;
            this.evYieldDEF = evDEF;
            this.evYieldSPA = evSPA;
            this.evYieldSPD = evSPD;
            this.evYieldSPE = evSPE;

            this.BaseStatsHP = baseStatsHP;
            this.BaseStatsATK = baseStatsATK;
            this.BaseStatsDEF = baseStatsDEF;
            this.BaseStatsSPA = baseStatsSPA;
            this.BaseStatsSPD = baseStatsSPD;
            this.BaseStatsSPE = baseStatsSPE;
            this.BaseFriendship = baseFriendship;

            this.Rarity = rarity;
            //this.lightColor = lightColor;
            this.PokedexColor = pokedexColor | Color.NONE;

            //ToDo: wild pokemon held items not yet implemented
            this.HeldItem = heldItem; //[item id,% chance]

            /*this.Luminance = luminance;
			//if(movesetmoves != null)
			this.MoveTree = new PokemonMoveTree(movesetmoves);
            //this.MovesetLevels = movesetLevels;
            //this.MovesetMoves = movesetMoves; 
            //this.tmList = tmList; 

            this.Evolutions = evolution ?? new IPokemonEvolution[0];
			//this.EvolutionID = evolutionID;
			//this.evolutionMethod = evolutionMethod; 
			//this.evolutionRequirements = evolutionRequirements;*/

			this.Order			= order			;
			this.ShinyChance	= 0				;
			this.IsDefault		= isDefault		;
			this.IsBaby 		= isBaby 		;
			this.FormSwitchable	= formSwitchable; 
			this.HasGenderDiff 	= hasGenderDiff ;
			//this.EvolutionFROM= evolutionFROM ;
			//this.EvolutionTO 	= evolutionTO 	;
			this.Incense	 	= incense 		;
			this.EvoChainId 	= evoChainId 	;
			this.GenerationId 	= generationId 	;
			this.HabitatId 		= habitatId 	;
			this.ShapeId 		= shapeId 		;

			this.GenderEnum = GenderRatio.Genderless;
			if (genderRatio.HasValue)
				this.GenderEnum = genderRatio.Value;
			else 
				this.GenderEnum = maleRatio.HasValue ? getGenderRatio(maleRatio.Value) : GenderEnum; //genderRatio.Value;
		}
		#endregion

		#region Methods
        GenderRatio getGenderRatio(float maleRatioPercent)
        {
            /*switch ((int)maleRatioPercent)
            {
                case  -1:
                default:
                    return GenderRatio.Genderless;
                    //break;
            }*/
            if (maleRatioPercent == 100f) return GenderRatio.AlwaysMale;
            else if (maleRatioPercent == 0f) return GenderRatio.AlwaysFemale;
            else if (maleRatioPercent > 0f && maleRatioPercent < 12.5f) return GenderRatio.AlwaysFemale;
            else if (maleRatioPercent >= 12.5f && maleRatioPercent < 25f) return GenderRatio.FemaleSevenEighths;
            else if (maleRatioPercent >= 25f && maleRatioPercent < 37.5f) return GenderRatio.Female75Percent;
            else if (maleRatioPercent >= 37.5f && maleRatioPercent < 50f) return GenderRatio.Female75Percent;
            else if (maleRatioPercent >= 50f && maleRatioPercent < 62.5f) return GenderRatio.Female50Percent;
            else if (maleRatioPercent >= 62.5f && maleRatioPercent < 75f) return GenderRatio.Female50Percent;
            else if (maleRatioPercent >= 75f && maleRatioPercent < 87.5f) return GenderRatio.Female25Percent;
            else if (maleRatioPercent >= 87.5f && maleRatioPercent < 100f) return GenderRatio.FemaleOneEighth;
            else if (maleRatioPercent < 0 || maleRatioPercent > 100f) return GenderRatio.Genderless;
            else return GenderRatio.Genderless;
        }
		public bool Equals(PokemonData obj)
		{
			return this.ID == obj.ID; 
		}
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}
		public override int GetHashCode()
		{
			return (int)ID;
        }
        public string ToString(TextScripts text)
        {
            //create a switch, and return Locale Name or Description
            return base.ToString();
        }
        #endregion
    }
}