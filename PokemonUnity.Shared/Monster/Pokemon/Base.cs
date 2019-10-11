using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Attack;
using PokemonUnity.Inventory;

namespace PokemonUnity.Monster
{
    public partial class Pokemon
    {
        //ToDo: Make into interface...
		public partial class PokemonData
        {
            #region Variables
            //private Pokemons id;
            //private int[] regionalPokedex;
            //private enum habitat; //ToDo: Grassland, Mountains...
            /// <summary>
            /// Name of the specific pokemon+form
            /// for given Id in database
            /// <para>Charizard is a Species. 
            /// But megaCharizard is a Form.
            /// </para>
            /// </summary>
            /// <example>
            /// Deoxys Pokedex# can be 1,
            /// but Deoxys-Power id# can be 32
            /// </example>
            private string name { get; set; }
            //private string species;
            //private string pokedexEntry;
            //private int forms; 
            /// <summary>
            /// 
            /// </summary>
            /// ToDo: Instead of string, what if it was pokemon enum?
            /// Enum would sync with xml translation, 
            /// and help with linking pokemon forms with original
            /// Or a new form class, to help with changing base data
            private string[] forms = new string[0];
            private Pokemons[] forms2 { get { return PokemonUnity.Battle.Form.Forms.Where( x => x.BaseSpecies == this.ID ).Select( y => y.ID ).ToArray(); } } 
            /*// <summary>
            /// Represents CURRENT form, if no form is active, current or does not exist
            /// then value is 0.
            /// </summary>
            /// ToDo: Make a PokemonForm class, that establishes the rule for 
            /// <see cref="Pokemons"/> and <see cref="Form"/>
            private int form;// = 0; */

            private Types type1 = Types.NONE;
            private Types type2 = Types.NONE;
            /// <summary>
            /// All three pokemon abilities 
            /// (Abiltiy1, Ability2, HiddenAbility).
            /// </summary>
            ///	<remarks>
            /// Should be [int? a1,int? a2,int? a3]
            /// instead of above...
            /// </remarks> 
            private readonly Abilities[] ability = new Abilities[3];
            private Abilities ability1 = Abilities.NONE;
            private Abilities ability2 = Abilities.NONE;
            private Abilities abilityh = Abilities.NONE;

            //private GenderRatio maleRatio;
            //private int catchRate;
            private EggGroups eggGroup1 = EggGroups.NONE;
            private EggGroups eggGroup2 = EggGroups.NONE;
            //private int hatchTime;

            //private float hitboxWidth; //used for 3d battles; just use collision detection from models
            //private float height;
            //private float weight;
            //private int shapeID; 

            //private int baseExpYield;
            //private LevelingRate levelingRate;

            //private Color pokedexColor;
            //private int baseFriendship;

            //private int baseStatsHP;
            //private int baseStatsATK;
            //private int baseStatsDEF;
            //private int baseStatsSPA;
            //private int baseStatsSPD;
            //private int baseStatsSPE;

            //private float luminance;
            //private UnityEngine.Color lightColor;

            //private int[,] heldItem;

            //private int[] movesetLevels;
            //private Moves[] movesetMoves;
            //private PokemonMoveset[] moveSet;

            //private string[] tmList; //Will be done thru ItemsClass

            //private int[] evolutionID;
            //private string[] evolutionRequirements;
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
            public int RegionalDex { get; private set; }
            public int[] RegionalPokedex { get; private set; }
            public byte GenerationId { get; private set; }
            /// <summary>
            /// Name of the specific pokemon+form
            /// for given Id in database
            /// <para>Charizard is a Species. 
            /// But megaCharizard is a Form.
            /// </para>
            /// </summary>
            /// <example>
            /// Deoxys Pokedex# can be 1,
            /// but Deoxys-Power id# can be 32
            /// </example>
            /// ToDo: Form = 0 should return null
            /// public string Name { get { return PokemonData.GetPokedexTranslation(this.ID).Forms[this.Form] ?? this.name; } }
            public string Name
            {
                get
                {
                    /*List<string> formvalues = new List<string>();
                    foreach (var formValue in this.ID.ToString().Translate().FieldNames) {
                        //fieldnames.Add(field);
                        if (formValue.Key.Contains("form")){
                            //_dictionary.Dictionaries[Core.UserLanguage.ToString()][text].Value = string.Format(_dictionary.Dictionaries[Core.UserLanguage.ToString()][text].Value, fieldnames.ToArray());//(_dictionary.Dictionaries[Core.UserLanguage.ToString()][text].Value, _dictionary.Dictionaries[Core.UserLanguage.ToString()][text].FieldNames)
                            //_dictionary.Dictionaries[Core.UserLanguage.ToString()][text].Value = fieldnames.Where() .JoinAsString("; ") +"\n"+ _dictionary.Dictionaries[Core.UserLanguage.ToString()][text].Value;//(_dictionary.Dictionaries[Core.UserLanguage.ToString()][text].Value, _dictionary.Dictionaries[Core.UserLanguage.ToString()][text].FieldNames)
                            formvalues.Add(formValue.Value);
                        }
                    }
                    return formvalues.ToArray()[this.Form] ?? this.name*/
                    return this.forms[this.Form] ?? this.name;
                }
            }
            /// <summary>
            /// Species is the pokemon breed/genus
            /// <para>Charizard is a Species. 
            /// But megaCharizard is a Form.
            /// </para>
            /// </summary>
            /// <example>
            /// Deoxys-Power, Speed, Defense
            /// are all part of the same species.
            /// </example>
            /// <remarks>Should be an int, not a string</remarks>
            public string Species { get; private set; }
            /// <summary>
            /// </summary>
            public string PokedexEntry { get; private set; }
            public int EvoChainId { get; private set; }
            /// <summary>
            /// Form is the same Pokemon Pokedex entry. 
            /// Changing forms should change name value.
            /// Represents CURRENT form, if no form is active, current or does not exist
            /// then value is 0.
            /// </summary>
            /// but a different PokemonId
            /// If null, returns this.Pokemon.Id
            /// ToDo: Make a PokemonForm class, that establishes the rule for 
            /// <see cref="Pokemons"/> and <see cref="Form"/>
            /// Maybe the stats set/reated for pokemon of this form #?
            /// Ex. Form 1 would have +10 more HP than base form...
            /// ToDo: use with `this.forms2[this.Form]`?
            public int Form { get; set; }
            //public Pokemons Form2 { get; private set; }
            /// ToDo: I should use the # of Forms from the .xml rather than from the database initializer/constructor
            public int Forms { get { return this.forms.Length; } }
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
            //public EggGroup EggGroup2 { get { return this.eggGroup2; } }
            //public virtual Type Type1 { get { return this.type1; } }
            //public virtual Type Type2 { get { return this.type2; } }
            //Maybe use this instead?
            public virtual Types[] Type { get { return new Types[] { this.type1, this.type2 }; } }
            /// <summary>
            /// All three pokemon abilities 
            /// (Abiltiy1, Ability2, HiddenAbility).
            /// </summary>
            ///	<remarks>
            /// Should be [int? a1,int? a2,int? a3]
            /// instead of above...
            /// </remarks> 
            /// ToDo: this.ability;
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
            /// </summary>
            /// Not quite sure what this is for...
            public float Luminance { get; private set; }

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
            /// <summary>
            /// All the moves this pokemon species can learn, and the methods by which they learn them
            /// </summary>
            public PokemonMoveTree MoveTree { get; private set; }
            /// ToDo: Evolution class type array here
            public IPokemonEvolution[] Evolutions { get; private set; }

            /// <summary>
            /// The item that needs to be held by a parent when breeding in order for the egg to be this species. 
            /// If neither parent is holding the required item, the egg will be the next evolved species instead.
            /// <para></para>
            /// The only species that should have this line are ones which cannot breed, 
            /// but evolve into a species which can. That is, the species should be a "baby" species.
            /// Not all baby species need this line.
            /// </summary>
            public Items Incense { get; private set; }
            #endregion

            #region Constructors
            //public PokemonData() { }// this.name = PokemonData.GetPokedexTranslation(this.ID).Forms[this.Form] ?? this.Name; } //name equals form name unless there is none.
            public PokemonData(Pokemons Id) //: this()
            {
                //PokedexTranslation translation = PokemonData.GetPokedexTranslation(Id);
                //var translation = Id.ToString().Translate();
                this.ID = Id;
                //this.name = translation.Name;
                //this.species = translation.Species;
                //this.pokedexEntry = translation.PokedexEntry;
                /*this.PokedexEntry = translation.Value.Trim('\n');
                //this.forms = forms; //| new Pokemon[] { Id }; //ToDo: need new mechanic for how this should work
                List<string> formvalues = new List<string>();
                foreach (var fieldValue in translation.FieldNames)
                {
                    //fieldnames.Add(field);
                    if (fieldValue.Key.Contains("form"))
                    {
                        //_dictionary.Dictionaries[Core.UserLanguage.ToString()][text].Value = string.Format(_dictionary.Dictionaries[Core.UserLanguage.ToString()][text].Value, fieldnames.ToArray());//(_dictionary.Dictionaries[Core.UserLanguage.ToString()][text].Value, _dictionary.Dictionaries[Core.UserLanguage.ToString()][text].FieldNames)
                        //_dictionary.Dictionaries[Core.UserLanguage.ToString()][text].Value = fieldnames.Where() .JoinAsString("; ") +"\n"+ _dictionary.Dictionaries[Core.UserLanguage.ToString()][text].Value;//(_dictionary.Dictionaries[Core.UserLanguage.ToString()][text].Value, _dictionary.Dictionaries[Core.UserLanguage.ToString()][text].FieldNames)
                        formvalues.Add(fieldValue.Value);
                    }
                    if (fieldValue.Key.Contains("genus"))
                    {
                        this.Species = fieldValue.Value;
                    }
                    if (fieldValue.Key.Contains("name"))
                    {
                        this.name = fieldValue.Value ?? translation.Identifier;
                    }
                }
                if (formvalues.Count != 0)
                {
                    this.forms = formvalues.ToArray();
                }
                else
                {
                    this.forms = new string[] { null };
                }*/
            }

            public PokemonData(Pokemons Id = Pokemons.NONE, int[] regionalDex = null, //string name, 
                                Types type1 = Types.NONE, Types type2 = Types.NONE, Abilities ability1 = Abilities.NONE, Abilities ability2 = Abilities.NONE, Abilities hiddenAbility = Abilities.NONE,//Abilities[] abilities,
                                GenderRatio? genderRatio = null, float? maleRatio = null, int catchRate = 1, EggGroups eggGroup1 = EggGroups.NONE, EggGroups eggGroup2 = EggGroups.NONE, int hatchTime = 0,
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
								Pokemons evolutionFROM = Pokemons.NONE, List<int> evolutionTO = null, 
								int evoChainId = 0, byte generationId = 0, bool isDefault = false, bool isBaby = false, bool formSwitchable = false, bool hasGenderDiff = false, 
								Habitat habitatId = Habitat.RARE, Shape shapeId = Shape.BLOB,
								//ToDo: What if: `Pokemons form` to point back to base pokemon, and Pokemons.NONE, if they are the base form?
								//that way, we can assign values to pokemons with forms that give stat bonuses...
								//want to find a way to add pokemon froms from a different method. Maybe something like overwriting the `Database` values to match those of base pokemon for values that are duplicated.
								//Or I'll just add it at the bottom towards end of array using copy-paste method.
								Pokemons baseForm = Pokemons.NONE, //int forms = 0, 
                                int[,] heldItem = null) : this(Id)
            {//new PokemonData(1,1,"Bulbasaur",12,4,65,null,34,45,1,7,20,7f,69f,64,4,PokemonData.PokedexColor.GREEN,"Seed","\"Bulbasaur can be seen napping in bright sunlight. There is a seed on its back. By soaking up the sun’s rays, the seed grows progressively larger.\"",45,49,49,65,65,45,0f,new int[]{1,3,7,9,13,13,15,19,21,25,27,31,33,37},new int[]{33,45,73,22,77,79,36,75,230,74,38,388,235,402},new int[]{14,15,70,76,92,104,113,148,156,164,182,188,207,213,214,216,218,219,237,241,249,263,267,290,412,447,474,496,497,590},new int[]{2},new int[]{16},new int[]{1})
                this.RegionalPokedex = regionalDex;

                this.type1 = type1; //!= null ? (Types)type1 : Types.NONE;
                this.type2 = type2; //!= null ? (Types)type2 : Types.NONE;
                //this.ability = abilities;
                this.ability1 = (Abilities)ability1;
                this.ability2 = (Abilities)ability2;
                this.abilityh = (Abilities)hiddenAbility;

				if (genderRatio.HasValue) 
					this.GenderEnum = genderRatio.Value; 
				else
                this.GenderEnum = maleRatio.HasValue ? getGenderRatio(maleRatio.Value) : getGenderRatio(-1); //genderRatio.Value;
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
                this.Luminance = luminance;
                //this.lightColor = lightColor;
                this.PokedexColor = pokedexColor | Color.NONE;

                //ToDo: wild pokemon held items not yet implemented
                this.HeldItem = heldItem; //[item id,% chance]

				//if(movesetmoves != null)
				this.MoveTree = new PokemonMoveTree(movesetmoves);
                //this.MovesetLevels = movesetLevels;
                //this.MovesetMoves = movesetMoves; 
                //this.tmList = tmList; 

                this.Evolutions = evolution ?? new IPokemonEvolution[0];
				//this.EvolutionID = evolutionID;
				//this.evolutionMethod = evolutionMethod; 
				//this.evolutionRequirements = evolutionRequirements;


				this.IsDefault		= isDefault		;
				this.IsBaby 		= isBaby 		;
				this.FormSwitchable	= formSwitchable; 
				this.HasGenderDiff 	= hasGenderDiff ;
				//this.EvolutionFROM= evolutionFROM ;
				//this.EvolutionTO 	= evolutionTO 	;
				this.EvoChainId 	= evoChainId 	;
				this.GenerationId 	= generationId 	;
				this.HabitatId 		= habitatId 	;
				this.ShapeId 		= shapeId 		;
		}
            
            public PokemonData(Pokemons Id, Pokemons BaseSpecies) :  this (Id)
            {
                PokemonUnity.Monster.Pokemon.PokemonData _base = GetPokemon(BaseSpecies);
                this.RegionalPokedex = _base.RegionalPokedex;

                this.type1 = _base.type1; //!= null ? (Types)type1 : Types.NONE;
                this.type2 = _base.type2; //!= null ? (Types)type2 : Types.NONE;
                //this.ability = abilities;
                this.ability1 = (Abilities)_base.ability1;
                this.ability2 = (Abilities)_base.ability2;
                this.abilityh = (Abilities)_base.abilityh;

                this.GenderEnum = _base.GenderEnum; 
                this.CatchRate = _base.CatchRate;
                this.eggGroup1 = _base.eggGroup1;
                this.eggGroup2 = _base.eggGroup2;
                this.HatchTime = _base.HatchTime;

                this.Height = _base.Height;
                this.Weight = _base.Weight;
                this.BaseExpYield = _base.BaseExpYield;
                this.GrowthRate = (LevelingRate)_base.GrowthRate; //== null ? (LevelingRate)levelingRate : LevelingRate.NONE;

                this.evYieldHP  = _base.evYieldHP;
                this.evYieldATK = _base.evYieldATK;
                this.evYieldDEF = _base.evYieldDEF;
                this.evYieldSPA = _base.evYieldSPA;
                this.evYieldSPD = _base.evYieldSPD;
                this.evYieldSPE = _base.evYieldSPE;

                this.BaseStatsHP  = _base.BaseStatsHP;
                this.BaseStatsATK = _base.BaseStatsATK;
                this.BaseStatsDEF = _base.BaseStatsDEF;
                this.BaseStatsSPA = _base.BaseStatsSPA;
                this.BaseStatsSPD = _base.BaseStatsSPD;
                this.BaseStatsSPE = _base.BaseStatsSPE;
                this.BaseFriendship = _base.BaseFriendship;

                this.Rarity = _base.Rarity;
                this.Luminance = _base.Luminance;
                //this.lightColor = lightColor;
                this.PokedexColor = _base.PokedexColor | Color.NONE;

                //ToDo: wild pokemon held items not yet implemented
                this.HeldItem = _base.HeldItem; //[item id,% chance]

                this.MoveTree = _base.MoveTree;//new PokemonMoveTree(movesetmoves);
                //this.MovesetLevels = movesetLevels;
                //this.MovesetMoves = movesetMoves; 
                //this.tmList = tmList; 

                this.Evolutions = _base.Evolutions ?? new IPokemonEvolution[0];
                //this.EvolutionID = evolutionID;
                //this.evolutionMethod = evolutionMethod; 
                //this.evolutionRequirements = evolutionRequirements;
            }
            #endregion

            #region Methods
      //      private static bool LoadPokemonLocale()
      //      {
      //          //public static readonly PokemonData[] Database; //{ get { if(_database == null) _database = LoadDatabase(); return _database; } private set; }
      //          //static PokemonData()
      //          //{
      //          //    Database = new PokemonData[] {
      //          var xmlDocument = new System.Xml.XmlDocument();
      //          xmlDocument.LoadXml(System..IO.File.ReadAllText(Core.FILEPATH));

      //          var localizationDictionaryNode = xmlDocument.SelectNodes("/localizationDictionary");
      //          if (localizationDictionaryNode == null || localizationDictionaryNode.Count <= 0)
      //          {
      //              //throw new Exception("A Localization Xml must include localizationDictionary as root node.");
      //              GameDebug.Log("A Localization Xml must include localizationDictionary as root node.");
      //              return false;
      //          }

      //          var cultureName = localizationDictionaryNode[0].GetAttributeValueOrNull("culture");
      //          if (string.IsNullOrEmpty(cultureName))
      //          {
      //              //throw new Exception("culture is not defined in language XML file!");
      //              GameDebug.Log("culture is not defined in language XML file!");
      //              return false;
      //          }

      //          int? languageInt = (int?)localizationDictionaryNode[0].GetAttributeValueOrNull("id");
      //          if (languageInt == null) //is not an int
      //          {
      //              //throw new Exception("Language int/enum value is not defined in language XML file!");
      //              GameDebug.Log("Language int/enum value is not defined in language XML file!");
      //              return false;
      //          }

      //          //var dictionary = new XmlLocalizationDictionary(CultureInfo.GetCultureInfo(cultureName));
      //          var dictionary = new XmlLocalizationDictionary((Languages)int.Parse(languageInt));

      //          var dublicateNames = new List<string>();
      //          //Make a list of all the node types
      //          //Maybe a dictionary<string,nodeType>?

      //          var nodes = xmlDocument.SelectSingleNode("/localizationDictionary/Species").ChildNodes;
      //          if (nodes != null)
      //          {
      //              PokemonData[] database = new PokemonData[nodes.Length];
      //              foreach (System.Xml.XmlNode node in nodes)
      //              {
      //                  //if (nodes.HasChildNodes)
      //                  //{
						//	//if(!nodeType.ContainsKey(nodes.Name.ToUpperInvariant())) nodeType.Add(nodes.Name.ToUpperInvariant(), new List<LocalizedString>());
      //                      //foreach (XmlNode node in nodes)
      //                      //{
      //                          if (node.HasChildNodes && node.FirstChild.NodeType == System.Xml.XmlNodeType.Text /*node.NodeType != XmlNodeType.Comment*/)
      //                          {
      //                              string id = node.GetAttributeValueOrNull("identifier");// ?? node.GetAttributeValueOrNull("name");
      //                              if (string.IsNullOrEmpty(id))
      //                              {
      //                                  id = node.LocalName.ToString();
      //                                  //throw new Exception("name attribute of a text is empty in given xml string.");
      //                              }

      //                              if (dictionary.Contains(id))
      //                              {
      //                                  dublicateNames.Add(id);
      //                              }

      //                              //dictionary[name] = (node.GetAttributeValueOrNull("value") ?? node.InnerText).NormalizeLineEndings();
      //                              dictionary[id] = new LocalizedString() { Identifier = id };
      //                              dictionary[id].Value = node.InnerText.TrimStart(new char[] { '\r', '\n' });//.NormalizeLineEndings();
      //                              //dictionary[id].Name =  node.GetAttributeValueOrNull("name") ?? id;//.NormalizeLineEndings();

      //                              #region MyRegion
      //                              //ToDo: Maybe add a forms array, and a new method for single name calls
      //                              dictionary[id].FieldNames = new KeyValuePair<string, string>[node.Attributes.Count];//new string
      //                              //int n = 0;//dictionary.Forms[0] = node.Attributes["name"].Value;//that or return an empty array T[0]
      //                              for (int i = 0; i < node.Attributes.Count; i++)//foreach(System.Xml.XmlAttribute attr in node)
      //                              {
      //                                  //Skipping first 4 values will save processing
      //                                  /*if (node.Attributes[i].LocalName.Contains("form")) //Name vs LocalName?
      //                                  {
      //                                      //translation.Forms[i-4] = node.Attributes[i].Value; //limits xml to only 4 set values 
      //                                      dictionary.FieldNames[n] = node.Attributes[i].Value; n++;
      //                                  }*/
      //                                  //dictionary[name].FieldNames[i] = node.Attributes[i].Value; //n++;
      //                                  dictionary[id].FieldNames[i] = new KeyValuePair<string, string>(node.Attributes[i].LocalName, node.Attributes[i].Value); //n++;
      //                              }

      //                              //fieldArray.Add(node.LocalName.ToString());
      //                              dictionary[id].NodeType = node.LocalName.ToString();
      //                              //nodeType.Add(node.LocalName.ToString());
      //                              #endregion
      //                          }
						//	//}
						//	//nodeType[nodes.Name.ToUpperInvariant()] = dictionary;
						////}
      //              }
      //          }
      //          else 
      //              return false;

      //          if (dublicateNames.Count > 0)
      //          {
      //              //throw new Exception("A dictionary can not contain same key twice. There are some duplicated names: " + dublicateNames.JoinAsString(", "));//string.Join(", ",dublicateNames.ToArray())
      //              GameDebug.Log("A dictionary can not contain same key twice. There are some duplicated names: " + dublicateNames.JoinAsString(", "));//string.Join(", ",dublicateNames.ToArray())
      //          }

      //          xmlDocument = null;
      //          //return dictionary; 
      //          return true;           
      //      }
      //      private static bool LoadPokemonDatabase()
      //      {
      //          //public static readonly PokemonData[] Database; //{ get { if(_database == null) _database = LoadDatabase(); return _database; } private set; }
      //          //static PokemonData()
      //          //{
      //          //    Database = new PokemonData[] {
      //          var xmlDocument = new System.Xml.XmlDocument();
      //          //try/catch if read file then loadxml?... else error/fail
      //          //ToDo: Ping Server and CheckForUpdate(filename)
      //          xmlDocument.LoadXml(System.IO.File.ReadAllText(Core.FILEPATH));

      //          var localizationDictionaryNode = xmlDocument.SelectNodes("/localizationDictionary");
      //          if (localizationDictionaryNode == null || localizationDictionaryNode.Count <= 0)
      //          {
      //              //throw new Exception("A Localization Xml must include localizationDictionary as root node.");
      //              GameDebug.Log("A Localization Xml must include localizationDictionary as root node.");
      //              return false;
      //          }

      //          //var dictionary = new XmlLocalizationDictionary(CultureInfo.GetCultureInfo(cultureName));
      //          //var dictionary = new XmlLocalizationDictionary((Languages)int.Parse(languageInt));

      //          var dublicateNames = new List<string>();
      //          //Make a list of all the node types
      //          //Maybe a dictionary<string,nodeType>?

      //          var textNodes = xmlDocument.SelectSingleNode("/localizationDictionary/Species").ChildNodes;
      //          if (textNodes != null)
      //          {
      //              PokemonData[] database = new PokemonData[textNodes.Count];
      //              foreach (System.Xml.XmlNode nodes in textNodes)
      //              {
      //                  if (nodes.HasChildNodes)
      //                  {
						//	//if(!nodeType.ContainsKey(nodes.Name.ToUpperInvariant())) nodeType.Add(nodes.Name.ToUpperInvariant(), new List<LocalizedString>());
      //                      foreach (System.Xml.XmlNode node in nodes)
      //                      {
      //                          if (node.HasChildNodes && node.FirstChild.NodeType == System.Xml.XmlNodeType.Text /*node.NodeType != XmlNodeType.Comment*/)
      //                          {
      //                              var id = node.GetAttributeValueOrNull("identifier");// ?? node.GetAttributeValueOrNull("name");
      //                              if (string.IsNullOrEmpty(id))
      //                              {
      //                                  id = node.LocalName.ToString();
      //                                  //throw new Exception("name attribute of a text is empty in given xml string.");
      //                              }

      //                              if (dictionary.Contains(id))
      //                              {
      //                                  dublicateNames.Add(id);
      //                              }

      //                              //dictionary[name] = (node.GetAttributeValueOrNull("value") ?? node.InnerText).NormalizeLineEndings();
      //                              dictionary[id] = new LocalizedString() { Identifier = id };
      //                              dictionary[id].Value = node.InnerText.TrimStart(new char[] { '\r', '\n' });//.NormalizeLineEndings();
      //                              //dictionary[id].Name =  node.GetAttributeValueOrNull("name") ?? id;//.NormalizeLineEndings();

      //                              #region MyRegion
      //                              //ToDo: Maybe add a forms array, and a new method for single name calls
      //                              dictionary[id].FieldNames = new KeyValuePair<string, string>[node.Attributes.Count];//new string
      //                              //int n = 0;//dictionary.Forms[0] = node.Attributes["name"].Value;//that or return an empty array T[0]
      //                              for (int i = 0; i < node.Attributes.Count; i++)//foreach(System.Xml.XmlAttribute attr in node)
      //                              {
      //                                  //Skipping first 4 values will save processing
      //                                  /*if (node.Attributes[i].LocalName.Contains("form")) //Name vs LocalName?
      //                                  {
      //                                      //translation.Forms[i-4] = node.Attributes[i].Value; //limits xml to only 4 set values 
      //                                      dictionary.FieldNames[n] = node.Attributes[i].Value; n++;
      //                                  }*/
      //                                  //dictionary[name].FieldNames[i] = node.Attributes[i].Value; //n++;
      //                                  dictionary[id].FieldNames[i] = new KeyValuePair<string, string>(node.Attributes[i].LocalName, node.Attributes[i].Value); //n++;
      //                              }

      //                              //fieldArray.Add(node.LocalName.ToString());
      //                              dictionary[id].NodeType = node.LocalName.ToString();
      //                              //nodeType.Add(node.LocalName.ToString());
      //                              #endregion
      //                          }
						//	}
						//	nodeType[nodes.Name.ToUpperInvariant()] = dictionary;
						//}
      //              }
      //          }
      //          else 
      //              return false;

      //          if (dublicateNames.Count > 0)
      //          {
      //              //throw new Exception("A dictionary can not contain same key twice. There are some duplicated names: " + dublicateNames.JoinAsString(", "));//string.Join(", ",dublicateNames.ToArray())
      //              GameDebug.Log("A dictionary can not contain same key twice. There are some duplicated names: " + dublicateNames.JoinAsString(", "));//string.Join(", ",dublicateNames.ToArray())
      //          }

      //          xmlDocument = null;
      //          //return dictionary; 
      //          return true;           
      //      }

			/// <summary>
            /// 
            /// </summary>
            /// <param name="ID"></param>
            /// <returns></returns>
            public static PokemonData GetPokemon(Pokemons ID)
            {
                //Debug.Log("Get Pokemons");
                /*PokemonData result = null;
                int i = 1;
                while(result == null){
                    if(Database[i].ID == ID){
                        //Debug.Log("Pokemon DB Success");
                        return result = Database[i];
                    }
                    i += 1;
                    if(i >= Database.Length){
                        Debug.Log("Pokemon DB Fail");
                        return null;}
                }
                return result;*/
                //foreach (PokemonData pokemon in Database)
                //{
                //    if (pokemon.ID == ID) return pokemon;
                //}
                return Game.PokemonData[ID];
                throw new System.Exception("Pokemon ID doesnt exist in the database. Please check PokemonData constructor.");
                //return null;
            }

			/// <summary>
            /// 
            /// </summary>
            /// <param name="ID"></param>
            /// <returns>Either PokemonData or PokemonUnity.Battle.Form (Inherits PokemonData)</returns>
            private void SetForm(Types type1, Types type2, Abilities ability1, Abilities ability2, Abilities hiddenAbility, //Abilities[] abilities,
                /*GenderRatio genderRatio,*/ float? maleRatio, int catchRate, EggGroups eggGroup1, EggGroups eggGroup2, int hatchTime,
                float height, float weight, int baseExpYield, LevelingRate levelingRate,
                //int? evYieldHP, int? evYieldATK, int? evYieldDEF, int? evYieldSPA, int? evYieldSPD, int? evYieldSPE,
                int evHP, int evATK, int evDEF, int evSPA, int evSPD, int evSPE,
                Color pokedexColor, //int baseFriendship, //string species, string pokedexEntry,
                int baseStatsHP, int baseStatsATK, int baseStatsDEF, int baseStatsSPA, int baseStatsSPD, int baseStatsSPE,
                //Rarity rarity, float luminance, //Color lightColor,
                //PokemonMoveset[] movesetmoves, int[] movesetLevels, Moves[] movesetMoves, int[] tmList, IPokemonEvolution[] evolution,
                //int[] evolutionID, int[] evolutionLevel, int[] evolutionMethod, //string[] evolutionRequirements,
                //Pokemons baseForm = Pokemons.NONE, //int forms = 0, 
                int[,] heldItem)
            {
                //this.RegionalPokedex = _base.regionalDex;

                this.type1 = type1; //!= null ? (Types)type1 : Types.NONE;
                this.type2 = type2; //!= null ? (Types)type2 : Types.NONE;
                //this.ability = abilities;
                this.ability1 = (Abilities)ability1;
                this.ability2 = (Abilities)ability2;
                this.abilityh = (Abilities)hiddenAbility;

                this.GenderEnum = (GenderRatio)(maleRatio.HasValue ? (GenderRatio)getGenderRatio((float)maleRatio.Value) : GenderRatio.Genderless);
				this.CatchRate = catchRate;
                this.eggGroup1 = eggGroup1;
                this.eggGroup2 = eggGroup2;
                this.HatchTime = hatchTime;

                this.Height = height;
                this.Weight = weight;
                this.BaseExpYield = baseExpYield;
                this.GrowthRate = (LevelingRate)levelingRate; 

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
                //this.BaseFriendship = baseFriendship;

                //this.Rarity = rarity;
                //this.Luminance = luminance;
                //this.lightColor = lightColor;
                this.PokedexColor = pokedexColor | Color.NONE;

                this.HeldItem = heldItem; //[item id,% chance]

                /*this.MoveTree = new PokemonMoveTree(movesetmoves);
                //this.MovesetLevels = movesetLevels;
                //this.MovesetMoves = movesetMoves; 
                //this.tmList = tmList; 

                this.Evolutions = evolution ?? new IPokemonEvolution[0];
                //this.EvolutionID = evolutionID;
                //this.evolutionMethod = evolutionMethod; 
                //this.evolutionRequirements = evolutionRequirements;*/
            }

            /// <summary>
            /// Returns the list of moves this Pokémon can learn by training method.
            /// </summary>
            public Moves[] GetMoveList(LearnMethod? method = null)
            {
                switch (method)
                {
                    case LearnMethod.egg:
                        return MoveTree.Egg;
                    case LearnMethod.levelup:
                        return MoveTree.LevelUp.Select(x => x.Key).ToArray();
                    case LearnMethod.machine:
                        return MoveTree.Machine;
                    case LearnMethod.tutor:
                        return MoveTree.Tutor;
                    case LearnMethod.shadow:
                    case LearnMethod.xd_shadow:
                        List<Moves> s = new List<Moves>();
                        s.AddRange(MoveTree.Shadow);
                        s.AddRange(MoveTree.Shadow.Where(x => !s.Contains(x)).Select(x => x));
                        return s.ToArray();
                    default:
                        List<Moves> list = new List<Moves>();
                        list.AddRange(MoveTree.Egg);
                        list.AddRange(MoveTree.Machine.Where(x => !list.Contains(x)).Select(x => x));
                        list.AddRange(MoveTree.Tutor.Where(x => !list.Contains(x)).Select(x => x));
                        list.AddRange(MoveTree.LevelUp.Where(x => !list.Contains(x.Key))/*(x => x.Value <= this.Level)*/.Select(x => x.Key));
                        return list.ToArray();
                }
            }

            //static int getPokemonArrayId(Pokemons ID)
            //{
            //    //Debug.Log("Get Pokemons");
            //    /*PokemonData result = null;
            //    int i = 1;
            //    while(result == null){
            //        if(Database[i].ID == ID){
            //            //Debug.Log("Pokemon DB Success");
            //            return result = Database[i];
            //        }
            //        i += 1;
            //        if(i >= Database.Length){
            //            Debug.Log("Pokemon DB Fail");
            //            return null;}
            //    }
            //    return result;*/
            //    /*foreach(PokemonData pokemon in Database)
            //    {
            //        if (pokemon.ID == ID) return pokemon;
            //    }*/
            //    for (int i = 0; i < Database.Length; i++)
            //    {
            //        if (Database[i].ID == ID)
            //        {
            //            return i;
            //        }
            //    }
            //    throw new System.Exception("Pokemon ID doesnt exist in the database. Please check PokemonData constructor.");
            //}

            /// <summary>
            /// Returns int value of Pokemon from PokemonData[] <see cref="Database"/>
            /// </summary>
            public int ArrayId
            {//(Pokemon ID)
                get
                {
                    //Debug.Log("Get Pokemons");
                    /*PokemonData result = null;
                    int i = 1;
                    while(result == null){
                        if(Database[i].ID == ID){
                            //Debug.Log("Pokemon DB Success");
                            return result = Database[i];
                        }
                        i += 1;
                        if(i >= Database.Length){
                            Debug.Log("Pokemon DB Fail");
                            return null;}
                    }
                    return result;*/
                    /*foreach(PokemonData pokemon in Database)
                    {
                        if (pokemon.ID == ID) return pokemon;
                    }*/
                    //for (int i = 0; i < Database.Length; i++)
                    //{
                    //    if (Database[i].ID == ID)
                    //    {
                    //        return i;
                    //    }
                    //}
                    for (int i = 0; i < Game.PokemonData.Count; i++)
                    {
                        if (Game.PokemonData.ElementAt(i).Value.ID == ID)
                        {
                            return i;
                        }
                    }
                    throw new System.Exception("Pokemon ID doesnt exist in the database. Please check PokemonData constructor.");
                }
            }
            /*
            /// <summary>
            /// [ability1,ability2,hiddenAbility]
            /// </summary>
            /// <example>int[,] Abilities = getAbilities() = { {1,2,3} }
            /// <para>int hiddenAbility = Abilities[0,3]</para>
            /// </example>
            /// <remarks>Something i think would work better than a string</remarks>
            public string getAbility(int ability){
                switch (ability)
                {
                    case 0:
                        return ability1;
                    case 1:
                        return ability2;
                    case 3:
                        return hiddenAbility;
                    default:
                        return null;
                }
            }*/

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
            #endregion
        }
    }
}