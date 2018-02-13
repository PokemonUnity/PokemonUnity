//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class PokemonData {
	#region Variables
	/// <summary>
	/// Id is the database value for specific pokemon+form
	/// </summary>
	/// <example>
	/// Deoxys pokedex# can be 1,
	/// but Deoxys-Power id# can be 32
	/// </example>
	private int ID;
	/// <summary>
	/// Pokedex number is the species identifier number
	/// <para>Charizard is a Species. 
	/// But megaCharizard is a Form.
	/// </para>
	/// </summary>
	/// <example>
	/// Deoxys pokedex# can be 1,
	/// but Deoxys-Power id# can be 32
	/// </example>
	private int PokedexNumber;
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
	private string Species;
	private string pokedexEntry; //not needed, make into a method that calls from xml text
	/// <summary>
	/// Name of the specific pokemon+form
	/// for given Id in database
	/// <para>Charizard is a Species. 
	/// But megaCharizard is a Form.
	/// </para>
	/// </summary>
	/// <example>
	/// Deoxys pokedex# can be 1,
	/// but Deoxys-Power id# can be 32
	/// </example>
	private string Name;
	//private int Form; //Not sure if this is needed here
	//public enum Ability : int? { } //made a class, dont think i need this or below
	//public System.Collections.Generic.Dictionary<int?, string> Ability = new System.Collections.Generic.Dictionary<int?, string>(); //{ };
	public enum Type{
		NONE = 0,
		NORMAL = 1,
		FIGHTING = 2,
		FLYING = 3,
		POISON = 4,
		GROUND = 5,
		ROCK = 6,
		BUG = 7,
		GHOST = 8,
		STEEL = 9,
		FIRE = 10,
		WATER = 11,
		GRASS = 12,
		ELECTRIC = 13,
		PSYCHIC = 14,
		ICE = 15,
		DRAGON = 16,
		DARK = 17,
		FAIRY = 18,
		UNKNOWN = 10001,
		SHADOW = 10002
	};
	public enum EggGroup{
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
	public enum LevelingRate{
		ERRATIC = 6, //fast then very slow?
		FAST = 3,
		MEDIUMFAST = 2, //Medium?
		MEDIUMSLOW = 4,
		SLOW = 1,
		FLUCTUATING = 5 //slow then fast?
	};
	public enum PokedexColor{
		RED = 8,
		BLUE = 2,
		YELLOW = 10,
		GREEN = 5,
		BLACK = 1,
		BROWN = 3,
		PURPLE = 7,
		GRAY = 4,
		WHITE = 9,
		PINK = 6,
        NONE = 0
    };

	/// <summary>
	/// Couldnt decide if best to use as a Collection or as a Method
	/// <para>Could work as both...</para>
	/// <seealso cref="StringToColor"/>
	/// </summary>
	/// <remarks>might need to make a new enum in PokemonData, type = x.Color...</remarks>
	private System.Collections.Generic.Dictionary<string, Color> StringToColorDic = new System.Collections.Generic.Dictionary<string, Color>() {//Dictionary<PokemonData.Type, Color>
		//http://www.epidemicjohto.com/t882-type-colors-hex-colors
		//Normal Type: A8A77A
		//Fire Type:  EE8130
		//Water Type: 6390F0
		//Electric Type:  F7D02C
		//Grass Type: 7AC74C
		//Ice Type: 96D9D6
		//Fighting Type:  C22E28
		//Poison Type: A33EA1
		//Ground Type: E2BF65
		//Flying Type: A98FF3
		//Psychic Type: F95587
		//Bug Type: A6B91A
		//Rock Type: B6A136
		//Ghost Type: 735797
		//Dragon Type:  6F35FC
		//Dark Type: 705746
		//Steel Type:  B7B7CE
		//Fairy Type: D685AD
			
		//http://www.serebiiforums.com/showthread.php?289595-Pokemon-type-color-associations
		//Normal -white
		//Fire - red
		//Water -blue
		//Electric -yellow
		//Grass - green
		//Ice - cyan
		//Poison -purple
		//Psychic - magenta
		//Fighting - dark red
		//Ground - brown
		//Rock - gray
		//Bug - yellow green
		//Flying - light blue
		//Dragon - orange
		//Ghost - light purple
		//Steel - dark gray
		//Dark - black

		//{"Black",Color.black },//dark
		//{"", new Color() },//dark blue -> dark, 
		{"Blue",Color.blue },//water
		{"Clear",Color.clear },
		{"Cyan",Color.cyan },
		{"Gray",Color.gray },//grAy-American
		//{"Grey",Color.grey },//grEy-European
		//{"Grey",Color.grey },//dark grey -> rock,
		{"Green",Color.green },//grass
		//{"", new Color() },//dark green -> bug,
		{"Magenta",Color.magenta },//magenta, purple -> poison
		{"Red",Color.red },//orange, dark red -> fire
		{"White",Color.white },//normals
		{"Yellow",Color.yellow },//electric
		{"Purple", new Color() },//ghost
		{"Brown", new Color() },//fighting
		{"Pink", new Color() }//,//fairy
		//{"", new Color() },//pink, lavender -> psychic, 
		//{"", new Color() },//ocre, brown -> ground
		//{"", new Color() },
		//{"", new Color() },
		//{"", new Color() }//fly, drag, steel, psychic, ice, shadow, unknown, bug, ground, poison?
	};
	private Type type1;
	private Type type2;
		private eAbility.Ability? ability1Id;
		private eAbility.Ability? ability2Id;
		private eAbility.Ability? hiddenAbilityId;
	private string ability1;
	private string ability2;
	private string hiddenAbility;
		/// <summary>
		/// All three pokemon abilities 
		/// (Abiltiy1, Ability2, HiddenAbility).
		/// </summary>
		///	<remarks>
		/// Should be [int? a1,int? a2,int? a3]
		/// instead of above...
		/// </remarks> 
		private eAbility.Ability?[,] abilities;
	
	/// <summary>
	/// The male ratio.
	/// <value>-1f is interpreted as genderless</value>
	/// </summary>
	private float maleRatio; 
	private int catchRate;
	private EggGroup eggGroup1;
	private EggGroup eggGroup2;
	private int hatchTime;

	//private float hitboxWidth; //used for 3d battles; just use collision detection from models
	private float height;
	private float weight;

	private int baseExpYield;
	private LevelingRate levelingRate;

    private int evYieldHP;
    private int evYieldATK;
    private int evYieldDEF;
    private int evYieldSPA;
    private int evYieldSPD;
    private int evYieldSPE;

    private PokedexColor pokedexColor;
    /// <summary>
    /// Friendship levels is the same as pokemon Happiness.
    /// </summary>
	private int baseFriendship;
	
	private int baseStatsHP;
	private int baseStatsATK;
	private int baseStatsDEF;
	private int baseStatsSPA;
	private int baseStatsSPD;
	private int baseStatsSPE;

	private float luminance;
	private Color lightColor;

	/// <summary>
	/// [item id,% chance]
	/// </summary>
	/// <example>int[,] heldItems = { {1,2,3},{4,5,6} }
	/// <para>heldItems[1,0] == 4 as int</para>
	/// </example>
	/// <remarks>Or maybe...[item id,% chance,generationId/regionId]</remarks>
	private int[,] heldItem;
	//private System.Collections.Generic.Dictionary<int, float> heldItem = new System.Collections.Generic.Dictionary<int, float>();

	private int[] movesetLevels;
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This should be an enum...
    /// </remarks>
	private string[] movesetMoves;

	private string[] tmList;

	private int[] evolutionID;
	/// <summary>
	/// <example>
	/// E.G.	Poliwhirl(61)
	///		<code>new int[]{62,186},
	///		new string[]{"Stone,Water Stone","Trade\Item,King's Rock"}),</code>
	/// <para>
	/// E.G. to evolve to sylveon
	///		<code>new int[]{..., 700},
	///		new string[]{..., "Amie\Move,2\Fairy"}),</code>
	/// </para> 
	/// </example>
	/// <list type="bullet"> 
	/// <item>
	/// <term>Level,int level</term>
	///	<description>if pokemon's level is greater or equal to int level</description>
	///	<item>
	/// <term>Stone,string itemName</term>
	///	<description>if name of stone is equal to string itemName</description>
	///	</item>
	/// <item>
	/// <term>Trade</term>
	///	<description>if currently trading pokemon</description>
	///	</item>
	/// <item>
	/// <term>Friendship</term>
	///	<description>if pokemon's friendship is greater or equal to 220</description>
	///	</item>
	/// <item>
	/// <term>Item,string itemName</term>
	///	<description>if pokemon's heldItem is equal to string itemName</description>
	/// </item>
	///	<item>
	/// <term>Gender,Pokemon.Gender</term>
	/// <description>if pokemon's gender is equal to Pokemon.Gender</description>
	/// </item>
	///	<item>
	/// <term>Move,string moveName</term>
	///	<description>if pokemon has a move thats name or typing is equal to string moveName</description>
	/// </item>
	///	<item>
	///	<term>Map,string mapName</term>
	///	<description>if currentMap is equal to string mapName</description>
	/// </item>
	///	<item>
	///	<term>Time,string dayNight</term>
	///	<description>if time is between 9PM and 4AM time is "Night". else time is "Day".
	///	if time is equal to string dayNight (either Day, or Night)</description>
	/// </item>
	/// <listheader><term>Exceptions</term><description>
	///		Unique evolution methods:
	/// </description></listheader>
	///	<item>
	/// <term>Mantine</term>
	///	<description>if party contains a Remoraid</description>
	/// </item>
	///	<item>
	///	<term>Pangoro</term>
	///	<description>if party contains a dark pokemon</description>
	/// </item>
	///	<item>
	///	<term>Goodra</term>
	///	<description>if currentMap's weather is rain</description>
	/// </item>
	///	<item>
	///	<term>Hitmonlee</term>
	///	<description>if pokemon's ATK is greater than DEF</description>
	/// </item>
	///	<item>
	///	<term>Hitmonchan</term>
	///	<description>if pokemon's ATK is lower than DEF</description>
	/// </item>
	///	<item>
	///	<term>Hitmontop</term>
	/// <description>if pokemon's ATK is equal to DEF</description>
	/// </item>
	///	<item>
	///	<term>Silcoon</term>
	/// <description>if pokemon's shinyValue divided by 2's remainder is equal to 0</description>
	/// </item>
	///	<item>
	///	<term>Cascoon</term>
	///	<description>if pokemon's shinyValue divided by 2's remainder is equal to 1</description>
	/// </item>
	/// </list>
	/// </summary>
	private string[] evolutionRequirements;
	#endregion

	


	public PokemonData (int ID, string name, Type type1, Type type2, string ability1, string ability2, string hiddenAbility,
	                    float maleRatio, int catchRate, EggGroup eggGroup1, EggGroup eggGroup2, int hatchTime,
	                    float height, float weight, int baseExpYield, LevelingRate levelingRate,
	                    int evYieldHP, int evYieldATK, int evYieldDEF, int evYieldSPA, int evYieldSPD, int evYieldSPE, 
	                    PokedexColor pokedexColor, int baseFriendship, 
	                    string species, string pokedexEntry, 
	                    int baseStatsHP, int baseStatsATK, int baseStatsDEF, int baseStatsSPA, int baseStatsSPD, int baseStatsSPE, 
	                    float luminance, Color lightColor, //string[] heldItem,
	                    int[] movesetLevels, string[] movesetMoves, string[] tmList,
	                    int[] evolutionID, string[] evolutionRequirements){
		this.ID = ID;
		this.Species = name;
		this.type1 = type1;
		this.type2 = type2;
		this.ability1 = ability1;
		this.ability2 = ability2;
		this.hiddenAbility = hiddenAbility;

		this.maleRatio = maleRatio;
		this.catchRate = catchRate;
		this.eggGroup1 = eggGroup1;
		this.eggGroup2 = eggGroup2;
		this.hatchTime = hatchTime;

		this.height = height;
		this.weight = weight;
		this.baseExpYield = baseExpYield;
		this.levelingRate = levelingRate;

		this.evYieldHP = evYieldHP;
		this.evYieldATK = evYieldATK;
		this.evYieldDEF = evYieldDEF;
		this.evYieldSPA = evYieldSPA;
		this.evYieldSPD = evYieldSPD;
		this.evYieldSPE = evYieldSPE;

		this.pokedexColor = pokedexColor;
		this.baseFriendship = baseFriendship;

		//this.species = species;
		this.pokedexEntry = pokedexEntry;

		this.baseStatsHP = baseStatsHP;
		this.baseStatsATK = baseStatsATK;
		this.baseStatsDEF = baseStatsDEF;
		this.baseStatsSPA = baseStatsSPA;
		this.baseStatsSPD = baseStatsSPD;
		this.baseStatsSPE = baseStatsSPE;

		this.luminance = luminance;
		this.lightColor = lightColor;

		//wild pokemon held items not yet implemented
		//this.heldItem = heldItem;

		this.movesetLevels = movesetLevels;
		this.movesetMoves = movesetMoves;
		this.tmList = tmList;

		this.evolutionID = evolutionID;
		this.evolutionRequirements = evolutionRequirements;
	}

	public PokemonData (int ID, string name, Type type1, Type type2, int? ability1, int? ability2, int? hiddenAbility,
	                    float maleRatio, int catchRate, EggGroup eggGroup1, EggGroup eggGroup2, int hatchTime,
	                    float height, float weight, int baseExpYield, LevelingRate levelingRate,
	                    int evYieldHP, int evYieldATK, int evYieldDEF, int evYieldSPA, int evYieldSPD, int evYieldSPE, 
	                    PokedexColor pokedexColor, int baseFriendship, int species, string pokedexEntry, 
	                    int baseStatsHP, int baseStatsATK, int baseStatsDEF, int baseStatsSPA, int baseStatsSPD, int baseStatsSPE, 
	                    float luminance, Color lightColor, int[] movesetLevels, int[] movesetMoves, int[] tmList,
	                    int[] evolutionID, string[] evolutionRequirements, int? forms, System.Collections.Generic.Dictionary<int, float> heldItem = null /*new int[1][] { new int[2]{-1,100} }*/){
		this.ID = ID;
		this.Species = name;
		this.type1 = type1;
		this.type2 = type2;
		this.ability1 = (string)ability1.ToString();
		this.ability2 = (string)ability2.ToString();
		this.hiddenAbility = (string)hiddenAbility.ToString();

		this.maleRatio = maleRatio;
		this.catchRate = catchRate;
		this.eggGroup1 = eggGroup1;
		this.eggGroup2 = eggGroup2;
		this.hatchTime = hatchTime;

		this.height = height;
		this.weight = weight;
		this.baseExpYield = baseExpYield;
		this.levelingRate = levelingRate;

		this.evYieldHP = evYieldHP;
		this.evYieldATK = evYieldATK;
		this.evYieldDEF = evYieldDEF;
		this.evYieldSPA = evYieldSPA;
		this.evYieldSPD = evYieldSPD;
		this.evYieldSPE = evYieldSPE;

		this.pokedexColor = pokedexColor;
		this.baseFriendship = baseFriendship;

		//this.Species = species;
		this.pokedexEntry = pokedexEntry;

		this.baseStatsHP = baseStatsHP;
		this.baseStatsATK = baseStatsATK;
		this.baseStatsDEF = baseStatsDEF;
		this.baseStatsSPA = baseStatsSPA;
		this.baseStatsSPD = baseStatsSPD;
		this.baseStatsSPE = baseStatsSPE;

		this.luminance = luminance;
		this.lightColor = lightColor;

		//wild pokemon held items not yet implemented
		//this.heldItem = heldItem; //[item id,% chance]

		this.movesetLevels = movesetLevels;
		//this.movesetMoves = movesetMoves;
		//this.tmList = tmList;

		this.evolutionID = evolutionID;
		this.evolutionRequirements = evolutionRequirements;
	}

	public PokemonData(int Id, int PokeId, string name, int? type1, int? type2, int? ability1, int? ability2, int? hiddenAbility,
						/*float maleRatio,*/ int catchRate, int? eggGroup1, int? eggGroup2, int hatchTime,
						float height, float weight, int baseExpYield, int levelingRate,
						/*int? evYieldHP, int? evYieldATK, int? evYieldDEF, int? evYieldSPA, int? evYieldSPD, int? evYieldSPE,*/
						PokedexColor pokedexColor, /*int baseFriendship,*/ string species, string pokedexEntry,
						int baseStatsHP, int baseStatsATK, int baseStatsDEF, int baseStatsSPA, int baseStatsSPD, int baseStatsSPE,
						float luminance, /*Color lightColor,*/ int[] movesetLevels, int[] movesetMoves, int[] tmList,
						int[] evolutionID, int[] evolutionLevel, int[] evolutionMethod, /*string[] evolutionRequirements,* /*int? forms,*/ int[,] heldItem = null)
	{//new PokemonData(1,1,"Bulbasaur",12,4,65,null,34,45,1,7,20,7f,69f,64,4,PokemonData.PokedexColor.GREEN,"Seed","\"Bulbasaur can be seen napping in bright sunlight. There is a seed on its back. By soaking up the sun’s rays, the seed grows progressively larger.\"",45,49,49,65,65,45,0f,new int[]{1,3,7,9,13,13,15,19,21,25,27,31,33,37},new int[]{33,45,73,22,77,79,36,75,230,74,38,388,235,402},new int[]{14,15,70,76,92,104,113,148,156,164,182,188,207,213,214,216,218,219,237,241,249,263,267,290,412,447,474,496,497,590},new int[]{2},new int[]{16},new int[]{1})
		new PokemonData(Id, PokeId, (PokemonData.Type)type1, type2 == null ? (PokemonData.Type)type2 : PokemonData.Type.NONE, (eAbility.Ability)ability1, (eAbility.Ability)ability2, (eAbility.Ability)hiddenAbility, catchRate,
            eggGroup1 == null ? (EggGroup)eggGroup1 : PokemonData.EggGroup.NONE, eggGroup2 == null ? (EggGroup)eggGroup2 : PokemonData.EggGroup.NONE, hatchTime, height, weight, baseExpYield, levelingRate, pokedexColor | PokemonData.PokedexColor.NONE,
            baseStatsHP, baseStatsATK, baseStatsDEF, baseStatsSPA, baseStatsSPD, baseStatsSPE, luminance, movesetLevels, movesetMoves, tmList, evolutionID, evolutionLevel, evolutionMethod, heldItem);
	}

    public PokemonData(int Id, int PokeId/*, string name*/, Type? type1, Type? type2, eAbility.Ability? ability1, eAbility.Ability? ability2, eAbility.Ability? hiddenAbility,
                        /*float maleRatio,*/ int catchRate, EggGroup? eggGroup1, EggGroup? eggGroup2, int hatchTime,
                        float height, float weight, int baseExpYield, int levelingRate,
                        /*int? evYieldHP, int? evYieldATK, int? evYieldDEF, int? evYieldSPA, int? evYieldSPD, int? evYieldSPE,*/
                        PokedexColor pokedexColor, /*int baseFriendship,* / string species, string pokedexEntry,*/
                        int baseStatsHP, int baseStatsATK, int baseStatsDEF, int baseStatsSPA, int baseStatsSPD, int baseStatsSPE,
                        float luminance, /*Color lightColor,*/ int[] movesetLevels, int[] movesetMoves, int[] tmList,
                        int[] evolutionID, int[] evolutionLevel, int[] evolutionMethod, /*string[] evolutionRequirements,* /*int? forms,*/ int[,] heldItem = null)
    {//new PokemonData(1,1,"Bulbasaur",12,4,65,null,34,45,1,7,20,7f,69f,64,4,PokemonData.PokedexColor.GREEN,"Seed","\"Bulbasaur can be seen napping in bright sunlight. There is a seed on its back. By soaking up the sun’s rays, the seed grows progressively larger.\"",45,49,49,65,65,45,0f,new int[]{1,3,7,9,13,13,15,19,21,25,27,31,33,37},new int[]{33,45,73,22,77,79,36,75,230,74,38,388,235,402},new int[]{14,15,70,76,92,104,113,148,156,164,182,188,207,213,214,216,218,219,237,241,249,263,267,290,412,447,474,496,497,590},new int[]{2},new int[]{16},new int[]{1})
        this.ID = Id;
        this.Name = PokemonDatabase.LoadPokedexLanguageText(SaveData.currentSave.playerLanguage)[Id, 0];//name;
        this.type1 = (PokemonData.Type)type1;
        this.type2 = type2 == null ? (PokemonData.Type)type2 : PokemonData.Type.NONE;
        this.ability1Id = (eAbility.Ability)ability1;
        this.ability2Id = (eAbility.Ability)ability2;
        this.hiddenAbilityId = (eAbility.Ability)hiddenAbility;

        //this.maleRatio = maleRatio;
        this.catchRate = catchRate;
        this.eggGroup1 = eggGroup1 == null ? (EggGroup)eggGroup1 : PokemonData.EggGroup.NONE;
        this.eggGroup2 = eggGroup2 == null ? (EggGroup)eggGroup2 : PokemonData.EggGroup.NONE;
        this.hatchTime = hatchTime;

        this.height = height;
        this.weight = weight;
        this.baseExpYield = baseExpYield;
        this.levelingRate = (LevelingRate)levelingRate; //== null ? (PokemonData.LevelingRate)levelingRate : PokemonData.LevelingRate.NONE;

        /* Not sure what kind of value goes here...
        this.evYieldHP = evYieldHP;
		this.evYieldATK = evYieldATK;
		this.evYieldDEF = evYieldDEF;
		this.evYieldSPA = evYieldSPA;
		this.evYieldSPD = evYieldSPD;
		this.evYieldSPE = evYieldSPE;*/

        this.pokedexColor = pokedexColor | PokemonData.PokedexColor.NONE;
        //this.baseFriendship = baseFriendship; //forgot to implement when transfering database

        this.Species = PokemonDatabase.LoadPokedexLanguageText(SaveData.currentSave.playerLanguage)[Id,1];//species;
        this.pokedexEntry = PokemonDatabase.LoadPokedexLanguageText(SaveData.currentSave.playerLanguage)[Id,2];//pokedexEntry;

        this.baseStatsHP = baseStatsHP;
        this.baseStatsATK = baseStatsATK;
        this.baseStatsDEF = baseStatsDEF;
        this.baseStatsSPA = baseStatsSPA;
        this.baseStatsSPD = baseStatsSPD;
        this.baseStatsSPE = baseStatsSPE;

        this.luminance = luminance;
        //this.lightColor = lightColor;

        //wild pokemon held items not yet implemented
        this.heldItem = heldItem; //[item id,% chance]

        this.movesetLevels = movesetLevels;
        //this.movesetMoves = movesetMoves;
        //this.tmList = tmList;

        this.evolutionID = evolutionID;
        //this.evolutionRequirements = evolutionRequirements;
        //return this.PokemonData();
    }

    #region Methods
    public override string ToString(){
		string result = ID.ToString() +", "+ Species +", "+ type1.ToString() +", ";
		result += type2.ToString() +", ";
		result += ability1 +", ";
		if (ability2 != null){
			result += ability2 +", ";
		}
		result += hiddenAbility +", ";
		return result;
	}

	public int getID(){
		return ID;
	}

	public string getName(){
		return Species;
	}
	
	public float getMaleRatio(){
		return maleRatio;
	}

	public PokemonData.Type getType1(){
		return type1;
	}
	public PokemonData.Type getType2(){
		return type2;
	}

	/// <summary>
	/// [ability1,ability2,hiddenAbility]
	/// </summary>
	/// <example>int[,] Abilities = getAbilities() = { {1,2,3} }
	/// <para>int hiddenAbility = Abilities[0,3]</para>
	/// </example>
	/// <remarks>Something i think would work better than a string</remarks>
	public string getAbility(int ability){
		/*if(ability == 0){
			return ability1;
		}
		else if(ability == 1){
			if(ability2 == null){ return ability1; }
			return ability2;
		}
		else if(ability == 2){
			if(hiddenAbility == null){ return ability1; }
			return hiddenAbility;
		}
		return null;*/
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
	}

	public int getBaseFriendship(){
		return baseFriendship;
	}

	public int getBaseExpYield(){
		return baseExpYield;
	}

	public int[] getBaseStats(){
		return new int[]{baseStatsHP, baseStatsATK, baseStatsDEF, baseStatsSPA, baseStatsSPD, baseStatsSPE}; 
	}

	public bool hasLight(){
		if (luminance > 0){
			return true;
		}
		return false;
	}

	public float getLuminance(){
		return luminance;
	}

	public Color getLightColor(){
		return lightColor;
	}

	public int[] getMovesetLevels(){
		return movesetLevels;
	}

	public string[] getMovesetMoves(){
		return movesetMoves;
	}

	public string[] getTmList(){
		return tmList;
	}

	public int[] getEvolutions(){
		return evolutionID;
	}

	public string[] getEvolutionRequirements(){
		return evolutionRequirements;
	}

	public PokemonData.LevelingRate getLevelingRate(){
		return levelingRate;
	}

	public int getCatchRate(){
		return catchRate;
	}

	public string[] GenerateMoveset(int level){
		//Set moveset based off of the highest level moves possible.
		string[] moveset = new string[4];
		int i = movesetLevels.Length-1; //work backwards so that the highest level move possible is grabbed first
		while(moveset[3] == null){
			if(movesetLevels[i] <= level){
				moveset[3] = movesetMoves[i];
			}
			i -= 1;
		}
		if(i >= 0){ //if i is at least 0 still, then you can grab the next move down.
			moveset[2] = movesetMoves[i];
			i -= 1;
			if(i >= 0){ //if i is at least 0 still, then you can grab the next move down.
				moveset[1] = movesetMoves[i];
				i -= 1;
				if(i >= 0){ //if i is at least 0 still, then you can grab the last move down.
					moveset[0] = movesetMoves[i];
					i -= 1;
				}
			}
		}
		i = 0;
		int i2 = 0;			//if the first move is null, then the array will need to be packed down
		if (moveset[0] == null){ 		//(nulls moved to the end of the array)
			while(i < 3){ 
				while(moveset[i] == null){
					i += 1;
				}
				moveset[i2] = moveset[i];
				moveset[i] = null;
				i2 += 1;
			}
		}
		return moveset;
	}
	/// <summary>
	/// Converts the string of a Pokemon Type to a Color in Unity. 
	/// </summary>
	/// <param name="PokemonType">string of pokemon type or name of a color</param>
	/// <returns>return a Unity.Color</returns>
	/// <example>StringToColor(Electric)</example>
	/// <example>StringToColor(Yellow)</example>
	public Color StringToColor(string PokemonType) {
		return StringToColorDic[PokemonType];
	}
	/// <summary>
	/// Converts the Pokemon Type to a Color in Unity. 
	/// </summary>
	/// <param name="PokemonType">pokemon type</param>
	/// <returns>return a Unity.Color</returns>
	/// <example>StringToColor(Electric)</example>
	public Color StringToColor(PokemonData.Type PokemonType) {
		return StringToColorDic[PokemonType.ToString()]; //Will fix later
	}
	/// <summary>
	/// Only an example. Do not use, will  not work.
	/// <para>Could be combined with database values 
	/// and used with ints instead of strings</para>
	/// <para>Convert the pokemon type into a color 
	/// that can be used with Unity's color lighting</para>
	/// </summary>
	/// <param name="color"></param>
	/// <returns></returns>
	public Color StringToColor(int color) {
		switch (color)
		{
			//case 1:
			//	return StringToColorDic["text"];
			default:
				return StringToColorDic[color.ToString()];
		}
	}
	#endregion

}

/*public class PrePokemonData
{
	public PrePokemonData()	{ }

	public PokemonData PrePokemonData(int ID, int? type1, int? type2, int? ability1, int? ability2, int? hiddenAbility,
						float maleRatio, int catchRate, int eggGroup1, int? eggGroup2, int hatchTime,
						float height, float weight, int baseExpYield, int levelingRate,
						int evYieldHP, int evYieldATK, int evYieldDEF, int evYieldSPA, int evYieldSPD, int evYieldSPE,
						int pokedexColor, int baseFriendship, int species, //string pokedexEntry,
						int baseStatsHP, int baseStatsATK, int baseStatsDEF, int baseStatsSPA, int baseStatsSPD, int baseStatsSPE,
						float luminance, Color lightColor, int[] movesetLevels, int[] movesetMoves, int[] tmList,
						int[] evolutionID, /*string[] evolutionRequirements,* int? forms, int[][] heldItem)
	{
		return new PokemonData();
	}
}*/