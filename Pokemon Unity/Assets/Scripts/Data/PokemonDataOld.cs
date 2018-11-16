//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

[System.Serializable]
[System.Obsolete("Use `Pokemon.cs` in `/Scripts2` folder...")]
public class PokemonDataOld {
	#region Variables
	/// <summary>
	/// Id is the database value for specific pokemon+form
    /// Different Pokemon forms share the same Pokedex number. 
    /// Values are loaded from <see cref="ePokemons.Pokemon"/>, where each form is registered to an Id.
	/// </summary>
	/// <example>
	/// Deoxys Pokedex# can be 1,
	/// but Deoxys-Power id# can be 32
	/// </example>
    /// <remarks>If game event/gm wants to "give" player Deoxys-Power form and not Speed form</remarks>
	private int ID;
    /// <summary>
    /// Different Gens assign different pokedex num
    /// </summary>
    /// <remarks>Think there is 3 pokedex</remarks>
    private int[] regionalPokedex;
	/// <summary>
    /// Deprecated; use <see cref="regionalPokedex"/>
	/// Pokedex number is the species identifier number
	/// <para>Charizard is a Species. 
	/// But megaCharizard is a Form.
	/// </para>
	/// </summary>
	/// <example>
	/// Deoxys Pokedex# can be 1,
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
    /// <summary>
    /// </summary>
	private string pokedexEntry; 
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

	private Type type1;
	private Type type2;
    /// <summary>
    /// Deprecated; <see cref="Abilities"/>
    /// </summary>
	private eAbility.Ability? ability1Id;
    /// <summary>
    /// Deprecated; <see cref="Abilities"/>
    /// </summary>
	private eAbility.Ability? ability2Id;
    /// <summary>
    /// Deprecated; <see cref="Abilities"/>
    /// </summary>
	private eAbility.Ability? hiddenAbilityId;
    /// <summary>
    /// Deprecated; <see cref="Abilities"/>
    /// </summary>
	private string ability1;
    /// <summary>
    /// Deprecated; <see cref="Abilities"/>
    /// </summary>
	private string ability2;
    /// <summary>
    /// Deprecated; <see cref="Abilities"/>
    /// </summary>
	private string hiddenAbility;
	/// <summary>
	/// All three pokemon abilities 
	/// (Abiltiy1, Ability2, HiddenAbility).
	/// </summary>
	///	<remarks>
	/// Should be [int? a1,int? a2,int? a3]
	/// instead of above...
	/// </remarks> 
	private eAbility.Ability[] Abilities = new eAbility.Ability[3];
	
	/// <summary>
	/// The male ratio.
	/// <value>-1f is interpreted as genderless</value>
	/// </summary>
	private float maleRatio;
    /// <summary>max is 255</summary> 
	private int catchRate;
	private EggGroup eggGroup1;
	private EggGroup eggGroup2;
	private int hatchTime;

	//private float hitboxWidth; //used for 3d battles; just use collision detection from models
	private float height;
	private float weight;
    private int shapeID; //enum

	private int baseExpYield;
	private LevelingRate levelingRate;

    private int evYieldHP; //unused
    private int evYieldATK; //unused
    private int evYieldDEF; //unused
    private int evYieldSPA; //unused
    private int evYieldSPD; //unused
    private int evYieldSPE; //unused

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
	/// <remarks>
    /// Or maybe...[item id,% chance,generationId/regionId]
    /// Custom Class needed here... <see cref="eItems.Item"/> as itemId
    /// </remarks>
	private int[,] heldItem;
	//private System.Collections.Generic.Dictionary<int, float> heldItem = new System.Collections.Generic.Dictionary<int, float>();

	private int[] movesetLevels;
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This should be an enum...
    /// </remarks>
	private string[] movesetMovesStrings;
	private eMoves.Move[] movesetMoves;

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
	

    public PokemonDataOld()
    {
        
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

	public string getName(){
		return Species;
	}
	
	public float getMaleRatio(){
		return maleRatio;
	}

	public PokemonDataOld.Type getType1(){
		return type1;
	}
	public PokemonDataOld.Type getType2(){
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
		return movesetMovesStrings;
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

	public PokemonDataOld.LevelingRate getLevelingRate(){
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
				moveset[3] = movesetMovesStrings[i];
			}
			i -= 1;
		}
		if(i >= 0){ //if i is at least 0 still, then you can grab the next move down.
			moveset[2] = movesetMovesStrings[i];
			i -= 1;
			if(i >= 0){ //if i is at least 0 still, then you can grab the next move down.
				moveset[1] = movesetMovesStrings[i];
				i -= 1;
				if(i >= 0){ //if i is at least 0 still, then you can grab the last move down.
					moveset[0] = movesetMovesStrings[i];
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
    /// <remarks>might need to make a new enum in PokemonData, type = x.Color...</remarks>
    public Color StringToColor(string PokemonType) {
        //private System.Collections.Generic.Dictionary<string, Color> StringToColorDic = new System.Collections.Generic.Dictionary<string, Color>() {//Dictionary<PokemonData.Type, Color>
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

        /*
        //{"Black",Color.black },//dark
        //{"", new Color() },//dark blue -> dark, 
        { "Blue",Color.blue },//water
		{ "Clear",Color.clear },
		{ "Cyan",Color.cyan },
		{ "Gray",Color.gray },//grAy-American
		//{"Grey",Color.grey },//grEy-European
		//{"Grey",Color.grey },//dark grey -> rock,
		{ "Green",Color.green },//grass
		//{"", new Color() },//dark green -> bug,
		{ "Magenta",Color.magenta },//magenta, purple -> poison
		{ "Red",Color.red },//orange, dark red -> fire
		{ "White",Color.white },//normals
		{ "Yellow",Color.yellow },//electric
		{ "Purple", new Color() },//ghost
		{ "Brown", new Color() },//fighting
		{ "Pink", new Color() }//,//fairy
        //{"", new Color() },//pink, lavender -> psychic, 
        //{"", new Color() },//ocre, brown -> ground
        //{"", new Color() },
        //{"", new Color() },
        //{"", new Color() }//fly, drag, steel, psychic, ice, shadow, unknown, bug, ground, poison?
        */
        return new Color();//StringToColorDic[PokemonType];
	}
	/// <summary>
	/// Converts the Pokemon Type to a Color in Unity. 
	/// </summary>
	/// <param name="PokemonType">pokemon type</param>
	/// <returns>return a Unity.Color</returns>
	/// <example>StringToColor(Electric)</example>
	public Color StringToColor(PokemonDataOld.Type PokemonType) {
		return StringToColor(PokemonType.ToString()); //Will fix later
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
				return StringToColor(color.ToString());
		}
	}
	#endregion

}