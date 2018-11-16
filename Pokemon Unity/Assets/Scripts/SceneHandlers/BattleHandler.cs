//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BattleHandler : MonoBehaviour
{
    public int victor = -1; //0 = player, 1 = opponent, 2 = tie
    private bool trainerBattle;

    private DialogBoxHandlerNew Dialog;

    //private AudioSource BattleAudio;
    public AudioClip defaultTrainerBGM;
    public int defaultTrainerBGMLoopStart = 577000;

    public AudioClip defaultTrainerVictoryBGM;
    public int defaultTrainerVictoryBGMLoopStart = 79000;

    public AudioClip defaultWildBGM;
    public int defaultWildBGMLoopStart = 578748;

    public AudioClip defaultWildVictoryBGM;
    public int defaultWildVictoryBGMLoopStart = 65000;

    public AudioClip
        scrollClip,
        selectClip,
        runClip,
        statUpClip,
        statDownClip,
        healFieldClip,
        fillExpClip,
        expFullClip,
        pokeballOpenClip,
        pokeballBounceClip,
        faintClip,
        hitClip,
        hitSuperClip,
        hitPoorClip;

    public Sprite
        partySpaceTex,
        partyBallTex,
        partyStatusTex,
        partyFaintTex,
        buttonFightTex,
        buttonFightSelTex,
        buttonBagTex,
        buttonBagSelTex,
        buttonRunTex,
        buttonRunSelTex,
        buttonPokeTex,
        buttonPokeSelTex,
        buttonMoveBackgroundTex,
        buttonMoveBackgroundSelTex,
        buttonMegaTex,
        buttonMegaActiveTex,
        buttonMegaActiveSelTex,
        buttonReturnTex,
        buttonReturnSelTex,
        buttonBlueTex,
        buttonBlueSelTex,
        buttonBackBagTex,
        buttonBackBagSelTex,
        buttonBagItemCategoryTex,
        buttonBagItemCategorySelTex,
        buttonBagItemListTex,
        buttonBagItemListSelTex,
        buttonBackPokeTex,
        buttonBackPokeSelTex,
        buttonPokemonTex,
        buttonPokemonRoundTex,
        buttonPokemonFntTex,
        buttonPokemonRoundFntTex,
        buttonPokemonSelTex,
        buttonPokemonRoundSelTex,
        buttonPokemonFntSelTex,
        buttonPokemonRoundFntSelTex;

    public Texture
        overlayHealTex,
        overlayStatUpTex,
        overlayStatDownTex;


    //TASK BUTTONS
    private Image
        buttonFight,
        buttonBag,
        buttonRun,
        buttonPoke;

    private Image[]
        buttonMoveCover = new Image[4],
        buttonMove = new Image[4];

    private Image
        buttonMegaEvolution,
        buttonMoveReturn;

    private Image
        buttonBackBag,
        buttonBackPoke;

    private Image buttonItemLastUsed;

    private Image[]
        buttonItemCategory = new Image[4],
        buttonItemList = new Image[8];

    private Image[] buttonPokemonSlot = new Image[6];

    private Image
        buttonSwitch,
        buttonCheck;

    private Text
        buttonCheckText,
        buttonCheckTextShadow;


    //MOVE BUTTON DETAILS
    private Image[] buttonMoveType = new Image[4];

    private Text[]
        buttonMoveName = new Text[4],
        buttonMoveNameShadow = new Text[4],
        buttonMovePP = new Text[4],
        buttonMovePPShadow = new Text[4];

    private GameObject bagObject;
    //ITEM LIST DETAILS
    private GameObject itemList;

    private Text
        itemListCategoryText,
        itemListCategoryTextShadow,
        itemListPageNumber,
        itemListPageNumberShadow;

    private GameObject
        itemListArrowPrev,
        itemListArrowNext;

    //private GameObject[] itemListButton = new GameObject[8]; //unused?
    private Image[] itemListIcon = new Image[8];

    private Text[]
        itemListName = new Text[8],
        itemListNameShadow = new Text[8],
        itemListQuantity = new Text[8],
        itemListQuantityShadow = new Text[8];

    private Text
        itemListDescription,
        itemListDescriptionShadow;

    //POKEMON LIST DETAILS
    private GameObject pokemonPartyObject;
    private Image[] pokemonSlotIcon = new Image[6];

    private Text[]
        pokemonSlotName = new Text[6],
        pokemonSlotNameShadow = new Text[6],
        pokemonSlotGender = new Text[6],
        pokemonSlotGenderShadow = new Text[6],
        pokemonSlotLevel = new Text[6],
        pokemonSlotLevelShadow = new Text[6],
        pokemonSlotCurrentHP = new Text[6],
        pokemonSlotCurrentHPShadow = new Text[6],
        pokemonSlotMaxHP = new Text[6],
        pokemonSlotMaxHPShadow = new Text[6];

    private Image[]
        pokemonSlotHPBar = new Image[6],
        pokemonSlotStatus = new Image[6],
        pokemonSlotItem = new Image[6];

    private Sprite[][] pokemonIconAnim = new Sprite[][]
    {
        new Sprite[2],
        new Sprite[2],
        new Sprite[2],
        new Sprite[2],
        new Sprite[2],
        new Sprite[2]
    };

    //POKE SELECTED DETAILS
    private GameObject pokemonSelectedPokemon;

    private Image
        pokemonSelectedIcon,
        pokemonSelectedStatus,
        pokemonSelectedType1,
        pokemonSelectedType2;

    private Text
        pokemonSelectedName,
        pokemonSelectedNameShadow,
        pokemonSelectedGender,
        pokemonSelectedGenderShadow,
        pokemonSelectedLevel,
        pokemonSelectedLevelShadow;

    private GameObject pokeObject;
    //POKE SUMMARY DETAILS
    private GameObject pokemonSummary;

    private Text
        pokemonSummaryHP,
        pokemonSummaryHPShadow,
        pokemonSummaryStatsTextShadow,
        pokemonSummaryStats,
        pokemonSummaryStatsShadow,
        pokemonSummaryNextLevelEXP,
        pokemonSummaryNextLevelEXPShadow,
        pokemonSummaryItemName,
        pokemonSummaryItemNameShadow,
        pokemonSummaryAbilityName,
        pokemonSummaryAbilityNameShadow,
        pokemonSummaryAbilityDescription,
        pokemonSummaryAbilityDescriptionShadow;

    private Image
        pokemonSummaryHPBar,
        pokemonSummaryEXPBar,
        pokemonSummaryItemIcon;

    //POKE MOVES DETAILS
    private GameObject pokemonMoves;

    private Text[]
        pokemonMovesName = new Text[4],
        pokemonMovesNameShadow = new Text[4],
        pokemonMovesPPText = new Text[4],
        pokemonMovesPPTextShadow = new Text[4],
        pokemonMovesPP = new Text[4],
        pokemonMovesPPShadow = new Text[4];

    private Image[] pokemonMovesType = new Image[4];

    private Text
        pokemonMovesSelectedPower,
        pokemonMovesSelectedPowerShadow,
        pokemonMovesSelectedAccuracy,
        pokemonMovesSelectedAccuracyShadow,
        pokemonMovesSelectedDescription,
        pokemonMovesSelectedDescriptionShadow;

    private Image
        pokemonMovesSelectedCategory,
        pokemonMovesSelector,
        pokemonMovesSelectedMove;


    //PARTY DISPLAYS
    private Image
        playerPartyBar,
        opponentPartyBar;

    private Image[]
        playerPartyBarSpace = new Image[6],
        opponentPartyBarSpace = new Image[6];

    //POKEMON STAT DISPLAYS
    private Text
        pokemon0CurrentHP,
        pokemon0CurrentHPShadow,
        pokemon0MaxHP,
        pokemon0MaxHPShadow;

    private Image pokemon0ExpBar;

    private Image[]
        pokemonStatsDisplay = new Image[6],
        statsHPBar = new Image[6],
        statsStatus = new Image[6];

    private Text[]
        statsName = new Text[6],
        statsNameShadow = new Text[6],
        statsGender = new Text[6],
        statsGenderShadow = new Text[6],
        statsLevel = new Text[6],
        statsLevelShadow = new Text[6];

    //BACKGROUNDS
    private Image
        playerBase,
        opponentBase,
        background;

    //DEBUG
    public bool canMegaEvolve = false;

    private Sprite[] playerTrainer1Animation;
    private Image playerTrainerSprite1;
    private Sprite[] trainer1Animation;
    private Image trainerSprite1;

    private Sprite[] player1Animation;
    private Image player1;
    private RawImage player1Overlay;
    private Sprite[] opponent1Animation;
    private Image opponent1;
    private RawImage opponent1Overlay;

    private Coroutine animatePlayer1;
    private Coroutine animateOpponent1;

    private Coroutine animatingPartyIcons;


    //POSITIONS
    private int
        currentTask = 0,
        // 0 = task choice, 1 = move choice, 2 = bag choice, 3 = pokemon choice
        //								4 = item list,			  5 = summary, 6 = moves
        taskPosition = 1,
        // 0/3 = bag, 1 = fight, 2/5 = pokemon, 4 = run
        movePosition = 1,
        // 0 = Mega Evolution, 1/2/4/5 = move, 3 = back
        bagCategoryPosition = 0,
        // 0 = HPPP, 1 = Pokeballs, 2 = Status, 3 = Battle, 4 = Back
        pokePartyPosition = 0,
        // 0-5 = pokemon, 6 = Back
        itemListPagePosition = 0,
        //which item list page is currently open (displays +1 of variable)
        itemListPageCount = 0;

    private string[] itemListString;


    private int pokemonPerSide = 1;
	
    //[pokemonPosition][movePosition]
    private string[][] pokemonMoveset = new string[][]
    {
        new string[4],
        new string[4],
        new string[4],
        new string[4],
        new string[4],
        new string[4]
    };

    //Stats can be changed in battle. These changes never persist after swapping out.
    private int[][] pokemonStats = new int[][]
    {
        new int[6], //ATK
        new int[6], //DEF
        new int[6], //SPA
        new int[6], //SPD
        new int[6], //SPE
    };

    //Ability can be changed in battle. These changes never persist after swapping out.
    private string[] pokemonAbility = new string[6];
    //Types can be changed in battle. These changes never persist after swapping out.

    //pokemon stat boost data
    private int[][] pokemonStatsMod = new int[][]
    {
        new int[6], //ATK
        new int[6], //DEF
        new int[6], //SPA
        new int[6], //SPD
        new int[6], //SPE
        new int[6], //ACC
        new int[6], //EVA
    };

    //turn task data
    public enum CommandType
    {
        None,
        Move,
        Item,
        Switch,
        Flee
    }

    private CommandType[] command = new CommandType[6];
    private int[] commandTarget = new int[6];
    private MoveData[] commandMove = new MoveData[6];

    //Field effects
    private enum WeatherEffect
    {
        NONE,
        RAIN,
        SUN,
        SAND,
        HAIL,
        HEAVYRAIN,
        HEAVYSUN,
        STRONGWINDS
    }

    private enum TerrainEffect
    {
        NONE,
        ELECTRIC,
        GRASSY,
        MISTY
    }

    private WeatherEffect weather = WeatherEffect.NONE; //unused
    private int weatherTurns = 0; //unused
    private TerrainEffect terrain = TerrainEffect.NONE; //unused
    private int terrainTurns = 0; //unused
    private int gravityTurns = 0; //unused
    private int[] reflectTurns = new int[2]; //unused
    private int[] lightScreenTurns = new int[2]; //unused
    private int[] tailwindTurns = new int[2]; //unused
    private bool[] stealthRocks = new bool[2]; //unused
    private bool[] stickyWeb = new bool[2]; //unused
    private int[] spikesLayers = new int[2]; //unused
    private int[] toxicSpikesLayers = new int[2]; //unused

    //Pokemon Effects
    private bool[] confused = new bool[6];
    private int[] infatuatedBy = new int[] {-1, -1, -1, -1, -1, -1};
    private bool[] flinched = new bool[6];
    private int[] statusEffectTurns = new int[6];
    private int[] lockedTurns = new int[6];
    private int[] partTrappedTurns = new int[6];
    private bool[] trapped = new bool[6];
    private bool[] charging = new bool[6];
    private bool[] recharging = new bool[6];
    private bool[] protect = new bool[6];
    //specific moves
    private int[] seededBy = new int[] {-1, -1, -1, -1, -1, -1};
    private bool[] focusEnergy = new bool[6];
    private bool[] destinyBond = new bool[6];
    private bool[] minimized = new bool[6];
    private bool[] defenseCurled = new bool[6];

    //Turn Feedback Data
    private bool[] pokemonHasMoved = new bool[6];
    private string[] previousMove = new string[6]; //why is this unused
	
	


    /// Basic Wild Battle
    public IEnumerator control(Pokemon wildPokemon)
    {
        yield return StartCoroutine("");
    }

    /// Basic Trainer Battle
    public IEnumerator control(Trainer trainer)
    {
        yield return StartCoroutine(control(true, trainer, false));
    }

	public IEnumerator control(bool isTrainerBattle, Trainer trainer, bool healedOnDefeat) { yield return null; }
}