//Original Scripts by IIColour (IIColour_Spectrum)
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Localization;
using PokemonUnity.Attack.Data;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;
using PokemonUnity.Monster;
using PokemonUnity.Overworld;
using PokemonUnity.Utility;
using PokemonEssentials;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
//using DiscordPresence;

public partial class BattleHandler : MonoBehaviour
{
    #region Property Variables
    public int victor = -1; //0 = player, 1 = opponent, 2 = tie
    private bool trainerBattle;

    private bool fog;

    private DialogBoxHandlerNew Dialog;
    
    private AudioSource BattleAudio;

    [Header("Battle Musics")]
    
    public AudioClip defaultTrainerBGM;
    public int defaultTrainerBGMLoopStart = 577000;

    public AudioClip defaultTrainerVictoryBGM;
    public int defaultTrainerVictoryBGMLoopStart = 79000;

    public AudioClip defaultWildBGM;
    public int defaultWildBGMLoopStart = 578748;

    public AudioClip defaultWildVictoryBGM;
    public int defaultWildVictoryBGMLoopStart = 65000;
    
    public AudioClip unovaLegendaryBattleBGM;
    public int unovaLegendaryBattleBGMLoopStart = 65000;

    public AudioClip defaultLowHpBGM;
    public int defaultLowHpBGMLoopStart;

    private bool isLowHP;

    public AudioClip battleBGM;
    public int battleBGMLoopStart;
    
    private AudioClip lowHpBGM;
    public int lowHpBGMLoopStart;
    
    private int sample;
    
    public AudioClip
        scrollClip,
        selectClip,
        menuOpenClip,
        runClip,
        statUpClip,
        statDownClip,
        healFieldClip,
        fillExpClip,
        expFullClip,
        pokeballOpenClip,
        pokeballThrowClip,
        pokeballBounceClip,
        partyBallsClip,
        fallSmall,
        fallMid,
        fallLarge,
        faintClip,
        hitClip,
        hitSuperClip,
        hitPoorClip,
        megaEvolveStartClip,
        megaEvolveFinishClip,
        laser;

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
        buttonMovePPShadow = new Text[4],
        buttonMovePPText = new Text[4],
        buttonMovePPTextShadow = new Text[4];

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

    private GameObject[] itemListButton = new GameObject[8];
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
    
    private Text
        pokemon1CurrentHP,
        pokemon1CurrentHPShadow,
        pokemon1MaxHP,
        pokemon1MaxHPShadow;

    private Image pokemon0ExpBar;
    private Image pokemon1ExpBar;

    private Image[]
        pokemonStatsDisplay = new Image[6],
        statsHPBar = new Image[6],
        statsStatus = new Image[6];

    private Text[]
        statsName = new Text[6],
        statsNameShadow = new Text[6],
        statsNameShadow2 = new Text[6],
        statsNameShadow3 = new Text[6],
        statsNameShadow4 = new Text[6],
        statsNameShadow5 = new Text[6],
        statsNameShadow6 = new Text[6],
        statsNameShadow7 = new Text[6],
        statsNameShadow8 = new Text[6],
        statsGender = new Text[6],
        statsGenderShadow = new Text[6],
        statsLevel = new Text[6],
        statsLevelShadow = new Text[6],
        statsLevelShadow2 = new Text[6],
        statsLevelShadow3 = new Text[6],
        statsLevelShadow4 = new Text[6],
        statsLevelShadow5 = new Text[6],
        statsLevelShadow6 = new Text[6],
        statsLevelShadow7 = new Text[6],
        statsLevelShadow8 = new Text[6];

    //BACKGROUNDS
    private Image
        playerBase,
        opponentBase,
        background;
    
    //SPRITES
    private SpriteRenderer
        playerSpriteRenderer,
        opponentSpriteRenderer,
        playerShadowSpriteRenderer,
        opponentShadowSpriteRenderer,
        playerTrainerSpriteRenderer,
        opponentTrainerSpriteRenderer,
        opponentTrainerShadowSpriteRenderer,
        playerBall,
        opponentBall;
    
    private SpriteRenderer
        player2SpriteRenderer,
        opponent2SpriteRenderer,
        player2ShadowSpriteRenderer,
        opponent2ShadowSpriteRenderer,
        opponentTrainer2SpriteRenderer,
        opponentTrainer2ShadowSpriteRenderer,
        allySpriteRenderer,
        playerBall2,
        allyBall,
        opponentBall2;
    
    //MASKS
    private SpriteMask
        playerSpriteMask,
        opponentSpriteMask,
        player2SpriteMask,
        opponent2SpriteMask;
    
    // Particles
    private ParticleSystem
        playerPokeballParticles,
        opponentPokeballParticles,
        player2PokeballParticles,
        opponent2PokeballParticles;

    //BATTLE CAMERA DISPLAY
    private GameObject BattleDisplay;
    
    //BATTLE CAMERA TRAVELLING
    private BattleCameraHandler BattleCamera;
    private Coroutine BattleTravelling;
    
    //STAT DISPLAY ANIMATION
    private Coroutine statDisplayAnim;
    
    //BATTLE SCENE
    private GameObject BattleScene;

    //DEBUG

    public bool canMegaEvolve;
    private bool megaActivate;

    private Sprite[] playerTrainer1Animation;
    private Image playerTrainerSprite1;
    private Sprite[] trainer1Animation;
    private Sprite[] trainer2Animation;
    private Image trainerSprite1;

    private Sprite[] player1Animation;
    private Sprite[] player2Animation;
    private Image player1;
    private RawImage player1Overlay;
    private Sprite[] opponent1Animation;
    private Sprite[] opponent2Animation;
    private Image opponent1;
    private RawImage opponent1Overlay;

    private GameObject backgroundObject;

    private Coroutine animatePlayer1;
    private Coroutine animateOpponent1;
    
    //TODO attributs Sprite Renderer
    private Coroutine animatePlayer1SR;
    private Coroutine animateOpponent1SR;
    private Coroutine animateShadowPlayer1SR;
    private Coroutine animateShadowOpponent1SR;
    // 2v2
    private Coroutine animatePlayer2SR;
    private Coroutine animateOpponent2SR;
    private Coroutine animateShadowPlayer2SR;
    private Coroutine animateShadowOpponent2SR;

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

    private int expIndex;
    private int expIndex2;
    private bool[] expShare;

    private int pokemonPerSide = 1;

    //pokemon
    private IPokemon[] pokemon = new IPokemon[6];
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
    private PokemonData.Type[] pokemonType1 = new PokemonData.Type[]
    {
        PokemonData.Type.NONE, PokemonData.Type.NONE, PokemonData.Type.NONE,
        PokemonData.Type.NONE, PokemonData.Type.NONE, PokemonData.Type.NONE
    };

    private PokemonData.Type[] pokemonType2 = new PokemonData.Type[]
    {
        PokemonData.Type.NONE, PokemonData.Type.NONE, PokemonData.Type.NONE,
        PokemonData.Type.NONE, PokemonData.Type.NONE, PokemonData.Type.NONE
    };

    private PokemonData.Type[] pokemonType3 = new PokemonData.Type[]
    {
        PokemonData.Type.NONE, PokemonData.Type.NONE, PokemonData.Type.NONE,
        PokemonData.Type.NONE, PokemonData.Type.NONE, PokemonData.Type.NONE
    };

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
    private ItemData[] commandItem = new ItemData[6];
    private IPokemon[] commandPokemon = new IPokemon[6];


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
    private string[] previousMove = new string[6];
    
    // Battle Camera Angles
    private Vector3[] baseCameraAngle =
    {
        new (0.01f, 3.89f, -5.24f), // Position
        new (13.333f, 0.118f, 0) // Rotation
    };
    
    private Vector3[] startCameraAngle =
    {
        new Vector3(7.2f,3.6f,2.4f), // Position
        new Vector3(13.333f, -11.753f, 0) // Rotation
    };
    
    private Vector3[] opponentFocusCameraAngle =
    {
        new (4.7f,3.7f,2.15f), // Position
        new (13.333f, 0.118f, 0) // Rotation
    };
    
    private Vector3[] playerFocusCameraAngle =
    {
        new (-1.55f, 3.685f, -5.24f), // Position
        new (13.333f, 0.118f, 0) // Rotation
    };
    
    private Vector3[] trainerFocusCameraAngle =
    {
        new (-0.46f,254.45f,-9.34f), // Position
        new (13.333f, 0.118f, 0) // Rotation
    };

    private Vector3[] player1FocusCameraAngle =
    {
        new(-2.75f,3.89f,-5.24f), // Position
        new(13.333f, 0.118f, 0) // Rotation
    };
    
    private Vector3[] player2FocusCameraAngle =
    {
        new(-0.33f,3.89f,-5.24f), // Position
        new(13.333f, 0.118f, 0) // Rotation
    };
    
    // Battle Scene Handler
    private BattleSceneHandler battleScenehandler;

    // Ability Variables
    [System.NonSerialized]
    public bool
        pressurePlayer, // Pressure applied by players (player side)
        pressureOpponent // Pressure applied by opponents (opponent side)
        ;
    #endregion

    void Awake()
    {
        //GameDebug.OnLog += GameDebug_OnLog;
        //GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
        string englishLocalization = "..\\..\\..\\LocalizationStrings.xml";
        //System.Console.WriteLine(System.IO.Directory.GetParent(englishLocalization).FullName);
        Game.LocalizationDictionary = new PokemonUnity.Localization.XmlStringRes(null); //new Debugger());
        Game.LocalizationDictionary.Initialize(englishLocalization, (int)PokemonUnity.Languages.English);

        //Setup Battle Scene
        BattleScene = GameObject.Find("Global/BattleScene");

        //Setup Camera Display
        BattleDisplay = GameObject.Find("Global/MainCamera/BattleDisplay");
        
        //Setup Camera Movements
        BattleCamera = GameObject.Find("Global/BattleScene/Battle_Camera").GetComponent<BattleCameraHandler>();

        Dialog = transform.GetComponent<DialogBoxHandlerNew>();
        
        BattleAudio = transform.GetComponent<AudioSource>();

        playerBase = transform.Find("player0").GetComponent<Image>();
        opponentBase = transform.Find("opponent0").GetComponent<Image>();
        background = transform.Find("Background").GetComponent<Image>();
        
        //Setup of sprite objects
        playerSpriteRenderer =
            GameObject.Find("BattleScene/player0/Pokemon/Mask/Sprite").GetComponent<SpriteRenderer>();
        opponentSpriteRenderer =
            GameObject.Find("BattleScene/opponent0/Pokemon/Mask/Sprite").GetComponent<SpriteRenderer>();
        playerShadowSpriteRenderer =
            GameObject.Find("BattleScene/player0/Pokemon/Shadow").GetComponent<SpriteRenderer>();
        opponentShadowSpriteRenderer  =
            GameObject.Find("BattleScene/opponent0/Pokemon/Shadow").GetComponent<SpriteRenderer>();
        
        player2SpriteRenderer =
            GameObject.Find("BattleScene/player1/Pokemon/Mask/Sprite").GetComponent<SpriteRenderer>();
        opponent2SpriteRenderer =
            GameObject.Find("BattleScene/opponent1/Pokemon/Mask/Sprite").GetComponent<SpriteRenderer>();
        player2ShadowSpriteRenderer =
            GameObject.Find("BattleScene/player1/Pokemon/Shadow").GetComponent<SpriteRenderer>();
        opponent2ShadowSpriteRenderer  =
            GameObject.Find("BattleScene/opponent1/Pokemon/Shadow").GetComponent<SpriteRenderer>();
        
        //Setup of Sprite masks
        playerSpriteMask = playerSpriteRenderer.gameObject.GetComponent<SpriteMask>();
        opponentSpriteMask  = opponentSpriteRenderer.gameObject.GetComponent<SpriteMask>();
        
        player2SpriteMask = player2SpriteRenderer.gameObject.GetComponent<SpriteMask>();
        opponent2SpriteMask  = opponent2SpriteRenderer.gameObject.GetComponent<SpriteMask>();
        
        //Setup opponent trainer sprite renderer
        opponentTrainerSpriteRenderer = BattleScene.transform.Find("opponent_trainer").GetComponent<SpriteRenderer>();
        opponentTrainerShadowSpriteRenderer = BattleScene.transform.Find("opponent_trainer_shadow").GetComponent<SpriteRenderer>();
        
        opponentTrainer2SpriteRenderer = BattleScene.transform.Find("opponent_trainer2").GetComponent<SpriteRenderer>();
        opponentTrainer2ShadowSpriteRenderer = BattleScene.transform.Find("opponent_trainer2_shadow").GetComponent<SpriteRenderer>();
        
        //Setup pokeballs
        playerBall = BattleScene.transform.Find("playerball").GetComponent<SpriteRenderer>();
        playerBall2 = BattleScene.transform.Find("playerball_2").GetComponent<SpriteRenderer>();
        allyBall = BattleScene.transform.Find("allyball").GetComponent<SpriteRenderer>();
        opponentBall = BattleScene.transform.Find("opponentball").GetComponent<SpriteRenderer>();
        opponentBall2 = BattleScene.transform.Find("opponentball_2").GetComponent<SpriteRenderer>();
        
        //Setup pokeball particles
        playerPokeballParticles = playerSpriteRenderer.transform.Find("Pokeball Flash").GetComponent<ParticleSystem>();
        opponentPokeballParticles = opponentSpriteRenderer.transform.Find("Pokeball Flash").GetComponent<ParticleSystem>();
        
        player2PokeballParticles = player2SpriteRenderer.transform.Find("Pokeball Flash").GetComponent<ParticleSystem>();
        opponent2PokeballParticles = opponent2SpriteRenderer.transform.Find("Pokeball Flash").GetComponent<ParticleSystem>();

        //TODO Desactiver le visuel 2D de base
        playerBase.gameObject.SetActive(false);
        opponentBase.gameObject.SetActive(false);

        trainerSprite1 = opponentBase.transform.Find("Trainer").GetComponent<Image>();
        playerTrainerSprite1 = playerBase.transform.Find("Trainer").GetComponent<Image>();

        player1 = playerBase.transform.Find("Pokemon").Find("Mask").Find("Sprite").GetComponent<Image>();
        opponent1 =
            opponentBase.transform.Find("Pokemon").Find("Mask").Find("Sprite").GetComponent<Image>();
        player1Overlay = player1.transform.Find("Overlay").GetComponent<RawImage>();
        opponent1Overlay = opponent1.transform.Find("Overlay").GetComponent<RawImage>();

        Transform playerPartyBarTrn = transform.Find("playerParty");
        Transform opponentPartyBarTrn = transform.Find("opponentParty");
        playerPartyBar = playerPartyBarTrn.Find("bar").GetComponent<Image>();
        opponentPartyBar = opponentPartyBarTrn.Find("bar").GetComponent<Image>();
        for (int i = 0; i < 6; i++)
        {
            playerPartyBarSpace[i] = playerPartyBarTrn.Find("space" + i).GetComponent<Image>();
        }
        for (int i = 0; i < 6; i++)
        {
            opponentPartyBarSpace[i] = opponentPartyBarTrn.Find("space" + i).GetComponent<Image>();
        }

        pokemonStatsDisplay[0] = transform.Find("playerStats0").GetComponent<Image>();
        pokemonStatsDisplay[1] = transform.Find("playerStats1").GetComponent<Image>();
        pokemonStatsDisplay[3] = transform.Find("opponentStats0").GetComponent<Image>();
        pokemonStatsDisplay[4] = transform.Find("opponentStats1").GetComponent<Image>();
        for (int i = 0; i < 2; ++i)
        {
            Transform statname = pokemonStatsDisplay[i].transform.Find("Name").transform;
            
            statsNameShadow[i] = statname.Find("outline").GetComponent<Text>();
            statsNameShadow2[i] = statname.Find("outline2").GetComponent<Text>();
            statsNameShadow3[i] = statname.Find("outline3").GetComponent<Text>();
            statsNameShadow4[i] = statname.Find("outline4").GetComponent<Text>();
            statsNameShadow5[i] = statname.Find("outline5").GetComponent<Text>();
            statsNameShadow6[i] = statname.Find("outline6").GetComponent<Text>();
            statsNameShadow7[i] = statname.Find("outline7").GetComponent<Text>();
            statsNameShadow8[i] = statname.Find("outline8").GetComponent<Text>();
            
            statsName[i] = statname.Find("Text").GetComponent<Text>();
            
            statsGenderShadow[i] = pokemonStatsDisplay[i].transform.Find("Gender").GetComponent<Text>();
            statsGender[i] = statsGenderShadow[i].transform.Find("Text").GetComponent<Text>();
            Transform statlevel = pokemonStatsDisplay[i].transform.Find("Level").transform;
            statsLevelShadow[i] = statlevel.Find("outline").GetComponent<Text>();
            statsLevelShadow2[i] = statlevel.Find("outline2").GetComponent<Text>();
            statsLevelShadow3[i] = statlevel.Find("outline3").GetComponent<Text>();
            statsLevelShadow4[i] = statlevel.Find("outline4").GetComponent<Text>();
            statsLevelShadow5[i] = statlevel.Find("outline5").GetComponent<Text>();
            statsLevelShadow6[i] = statlevel.Find("outline6").GetComponent<Text>();
            statsLevelShadow7[i] = statlevel.Find("outline7").GetComponent<Text>();
            statsLevelShadow8[i] = statlevel.Find("outline8").GetComponent<Text>();
            
            statsLevel[i] = statlevel.Find("Text").GetComponent<Text>();
            
            statsHPBar[i] = pokemonStatsDisplay[i].transform.Find("HPBar").GetComponent<Image>();
            statsStatus[i] = pokemonStatsDisplay[i].transform.Find("Status").GetComponent<Image>();
        }

        for (int i = 3; i < 5; ++i)
        {
            statsGenderShadow[i] = pokemonStatsDisplay[i].transform.Find("Gender").GetComponent<Text>();
            statsGender[i] = statsGenderShadow[i].transform.Find("Text").GetComponent<Text>();

            Transform statname = pokemonStatsDisplay[i].transform.Find("Name").transform;
            
            statsNameShadow[i] = statname.Find("outline").GetComponent<Text>();
            statsNameShadow2[i] = statname.Find("outline2").GetComponent<Text>();
            statsNameShadow3[i] = statname.Find("outline3").GetComponent<Text>();
            statsNameShadow4[i] = statname.Find("outline4").GetComponent<Text>();
            statsNameShadow5[i] = statname.Find("outline5").GetComponent<Text>();
            statsNameShadow6[i] = statname.Find("outline6").GetComponent<Text>();
            statsNameShadow7[i] = statname.Find("outline7").GetComponent<Text>();
            statsNameShadow8[i] = statname.Find("outline8").GetComponent<Text>();
            
            statsName[i] = statname.Find("Text").GetComponent<Text>();

            statsLevelShadow[i] = pokemonStatsDisplay[i].transform.Find("Level").GetComponent<Text>();
            statsLevel[i] = statsLevelShadow[i].transform.Find("Text").GetComponent<Text>();
            
            Transform statlevel = pokemonStatsDisplay[i].transform.Find("Level").transform;
            statsLevelShadow[i] = statlevel.Find("outline").GetComponent<Text>();
            statsLevelShadow2[i] = statlevel.Find("outline2").GetComponent<Text>();
            statsLevelShadow3[i] = statlevel.Find("outline3").GetComponent<Text>();
            statsLevelShadow4[i] = statlevel.Find("outline4").GetComponent<Text>();
            statsLevelShadow5[i] = statlevel.Find("outline5").GetComponent<Text>();
            statsLevelShadow6[i] = statlevel.Find("outline6").GetComponent<Text>();
            statsLevelShadow7[i] = statlevel.Find("outline7").GetComponent<Text>();
            statsLevelShadow8[i] = statlevel.Find("outline8").GetComponent<Text>();
            
            statsHPBar[i] = pokemonStatsDisplay[i].transform.Find("HPBar").GetComponent<Image>();
            statsStatus[i] = pokemonStatsDisplay[i].transform.Find("Status").GetComponent<Image>();
        }
        
        pokemon0CurrentHPShadow = pokemonStatsDisplay[0].transform.Find("CurrentHP").GetComponent<Text>();
        pokemon0CurrentHP = pokemon0CurrentHPShadow.transform.Find("Text").GetComponent<Text>();
        pokemon0MaxHPShadow = pokemonStatsDisplay[0].transform.Find("MaxHP").GetComponent<Text>();
        pokemon0MaxHP = pokemon0MaxHPShadow.transform.Find("Text").GetComponent<Text>();
        pokemon0ExpBar = pokemonStatsDisplay[0].transform.Find("ExpBar").GetComponent<Image>();
        
        pokemon1CurrentHPShadow = pokemonStatsDisplay[1].transform.Find("CurrentHP").GetComponent<Text>();
        pokemon1CurrentHP = pokemon1CurrentHPShadow.transform.Find("Text").GetComponent<Text>();
        pokemon1MaxHPShadow = pokemonStatsDisplay[1].transform.Find("MaxHP").GetComponent<Text>();
        pokemon1MaxHP = pokemon1MaxHPShadow.transform.Find("Text").GetComponent<Text>();
        pokemon1ExpBar = pokemonStatsDisplay[1].transform.Find("ExpBar").GetComponent<Image>();

        Transform optionBox = transform.Find("OptionBox");
        buttonFight = optionBox.Find("ButtonFight").GetComponent<Image>();
        buttonBag = optionBox.Find("ButtonBag").GetComponent<Image>();
        buttonPoke = optionBox.Find("ButtonPoke").GetComponent<Image>();
        buttonRun = optionBox.Find("ButtonRun").GetComponent<Image>();

        for (int i = 0; i < 4; i++)
        {
            buttonMove[i] = optionBox.Find("Move" + (i + 1)).GetComponent<Image>();
            buttonMoveType[i] = buttonMove[i].transform.Find("Type").GetComponent<Image>();
            buttonMoveNameShadow[i] = buttonMove[i].transform.Find("Name").GetComponent<Text>();
            buttonMoveName[i] = buttonMoveNameShadow[i].transform.Find("Text").GetComponent<Text>();
            buttonMovePPShadow[i] = buttonMove[i].transform.Find("PP").GetComponent<Text>();
            buttonMovePP[i] = buttonMovePPShadow[i].transform.Find("Text").GetComponent<Text>();
            buttonMovePPTextShadow[i] = buttonMove[i].transform.Find("PP (1)").GetComponent<Text>();
            buttonMovePPText[i] = buttonMovePPTextShadow[i].transform.Find("Text").GetComponent<Text>();
            buttonMoveCover[i] = buttonMove[i].transform.Find("Cover").GetComponent<Image>();
        }
        buttonMoveReturn = optionBox.Find("MoveReturn").GetComponent<Image>();
        buttonMegaEvolution = optionBox.Find("MoveMegaEvolution").GetComponent<Image>();

        bagObject = optionBox.Find("Bag").gameObject;
        pokeObject = optionBox.Find("Poke").gameObject;
        pokemonPartyObject = optionBox.Find("Party").gameObject;

        buttonBackBag = bagObject.transform.Find("ButtonBack").GetComponent<Image>();
        buttonBackPoke = pokeObject.transform.Find("ButtonBack").GetComponent<Image>();

        buttonItemCategory[0] = bagObject.transform.Find("ButtonHPPPRestore").GetComponent<Image>();
        buttonItemCategory[1] = bagObject.transform.Find("ButtonPokeBalls").GetComponent<Image>();
        buttonItemCategory[2] = bagObject.transform.Find("ButtonStatusHealers").GetComponent<Image>();
        buttonItemCategory[3] = bagObject.transform.Find("ButtonBattleItems").GetComponent<Image>();
        buttonItemLastUsed = bagObject.transform.Find("ButtonItemUsedLast").GetComponent<Image>();

        itemList = bagObject.transform.Find("Items").gameObject;
        for (int i = 0; i < 8; i++)
        {
            buttonItemList[i] = itemList.transform.Find("Item" + i).GetComponent<Image>();
            itemListIcon[i] = buttonItemList[i].transform.Find("Icon").GetComponent<Image>();
            itemListNameShadow[i] = buttonItemList[i].transform.Find("Item").GetComponent<Text>();
            itemListName[i] = itemListNameShadow[i].transform.Find("Text").GetComponent<Text>();
            itemListQuantityShadow[i] = buttonItemList[i].transform.Find("Quantity").GetComponent<Text>();
            itemListQuantity[i] = itemListQuantityShadow[i].transform.Find("Text").GetComponent<Text>();
        }

        for (int i = 0; i < 6; i++)
        {
            buttonPokemonSlot[i] = pokemonPartyObject.transform.Find("Slot" + i).GetComponent<Image>();
            pokemonSlotIcon[i] = buttonPokemonSlot[i].transform.Find("Icon").GetComponent<Image>();
            pokemonSlotNameShadow[i] = buttonPokemonSlot[i].transform.Find("Name").GetComponent<Text>();
            pokemonSlotName[i] = pokemonSlotNameShadow[i].transform.Find("Text").GetComponent<Text>();
            pokemonSlotGenderShadow[i] = buttonPokemonSlot[i].transform.Find("Gender").GetComponent<Text>();
            pokemonSlotGender[i] = pokemonSlotGenderShadow[i].transform.Find("Text").GetComponent<Text>();
            pokemonSlotLevelShadow[i] = buttonPokemonSlot[i].transform.Find("Level").GetComponent<Text>();
            pokemonSlotLevel[i] = pokemonSlotLevelShadow[i].transform.Find("Text").GetComponent<Text>();
            pokemonSlotCurrentHPShadow[i] = buttonPokemonSlot[i].transform.Find("CurrentHP").GetComponent<Text>();
            pokemonSlotCurrentHP[i] = pokemonSlotCurrentHPShadow[i].transform.Find("Text").GetComponent<Text>();
            pokemonSlotMaxHPShadow[i] = buttonPokemonSlot[i].transform.Find("MaxHP").GetComponent<Text>();
            pokemonSlotMaxHP[i] = pokemonSlotMaxHPShadow[i].transform.Find("Text").GetComponent<Text>();
            pokemonSlotHPBar[i] = buttonPokemonSlot[i].transform.Find("HPBar").GetComponent<Image>();
            pokemonSlotStatus[i] = buttonPokemonSlot[i].transform.Find("Status").GetComponent<Image>();
            pokemonSlotItem[i] = buttonPokemonSlot[i].transform.Find("Item").GetComponent<Image>();
        }

        buttonSwitch = pokeObject.transform.Find("ButtonSwitch").GetComponent<Image>();
        buttonCheck = pokeObject.transform.Find("ButtonCheck").GetComponent<Image>();
        buttonCheckTextShadow = buttonCheck.transform.Find("Text").GetComponent<Text>();
        buttonCheckText = buttonCheckTextShadow.transform.Find("Text").GetComponent<Text>();

        //ITEM LIST DETAILS
        itemListCategoryTextShadow = itemList.transform.Find("Category").GetComponent<Text>();
        itemListCategoryText = itemListCategoryTextShadow.transform.Find("Text").GetComponent<Text>();
        itemListPageNumberShadow = itemList.transform.Find("Page").GetComponent<Text>();
        itemListPageNumber = itemListPageNumberShadow.transform.Find("Text").GetComponent<Text>();
        itemListArrowPrev = itemList.transform.Find("PageArrowPrev").gameObject;
        itemListArrowNext = itemList.transform.Find("PageArrowNext").gameObject;
        itemListDescriptionShadow = itemList.transform.Find("ItemDescription").GetComponent<Text>();
        itemListDescription = itemListDescriptionShadow.transform.Find("Text").GetComponent<Text>();

        //POKE SELECTED DETAILS
        pokemonSelectedPokemon = pokeObject.transform.Find("SelectedPokemon").gameObject;
        pokemonSelectedIcon = pokemonSelectedPokemon.transform.Find("Icon").GetComponent<Image>();
        pokemonSelectedNameShadow = pokemonSelectedPokemon.transform.Find("Name").GetComponent<Text>();
        pokemonSelectedName = pokemonSelectedNameShadow.transform.Find("Text").GetComponent<Text>();
        pokemonSelectedGenderShadow = pokemonSelectedPokemon.transform.Find("Gender").GetComponent<Text>();
        pokemonSelectedGender = pokemonSelectedGenderShadow.transform.Find("Text").GetComponent<Text>();
        pokemonSelectedLevelShadow = pokemonSelectedPokemon.transform.Find("Level").GetComponent<Text>();
        pokemonSelectedLevel = pokemonSelectedLevelShadow.transform.Find("Text").GetComponent<Text>();
        pokemonSelectedStatus = pokemonSelectedPokemon.transform.Find("Status").GetComponent<Image>();
        pokemonSelectedType1 = pokemonSelectedPokemon.transform.Find("Type1").GetComponent<Image>();
        pokemonSelectedType2 = pokemonSelectedPokemon.transform.Find("Type2").GetComponent<Image>();

        //POKE SUMMARY DETAILS
        pokemonSummary = pokeObject.transform.Find("Summary").gameObject;
        pokemonSummaryHPShadow = pokemonSummary.transform.Find("HP").GetComponent<Text>();
        pokemonSummaryHP = pokemonSummaryHPShadow.transform.Find("Text").GetComponent<Text>();
        pokemonSummaryHPBar = pokemonSummary.transform.Find("HPBar").GetComponent<Image>();
        pokemonSummaryStatsTextShadow = pokemonSummary.transform.Find("StatsText").GetComponent<Text>();
        pokemonSummaryStatsShadow = pokemonSummary.transform.Find("Stats").GetComponent<Text>();
        pokemonSummaryStats = pokemonSummaryStatsShadow.transform.Find("Text").GetComponent<Text>();
        pokemonSummaryNextLevelEXPShadow = pokemonSummary.transform.Find("ToNextLevel").GetComponent<Text>();
        pokemonSummaryNextLevelEXP = pokemonSummaryNextLevelEXPShadow.transform.Find("Text").GetComponent<Text>();
        pokemonSummaryEXPBar = pokemonSummary.transform.Find("ExpBar").GetComponent<Image>();
        pokemonSummaryItemIcon = pokemonSummary.transform.Find("ItemIcon").GetComponent<Image>();
        pokemonSummaryItemNameShadow = pokemonSummary.transform.Find("Item").GetComponent<Text>();
        pokemonSummaryItemName = pokemonSummaryItemNameShadow.transform.Find("Text").GetComponent<Text>();
        pokemonSummaryAbilityNameShadow = pokemonSummary.transform.Find("Ability").GetComponent<Text>();
        pokemonSummaryAbilityName = pokemonSummaryAbilityNameShadow.transform.Find("Text").GetComponent<Text>();
        pokemonSummaryAbilityDescriptionShadow =
            pokemonSummary.transform.Find("AbilityDescription").GetComponent<Text>();
        pokemonSummaryAbilityDescription =
            pokemonSummaryAbilityDescriptionShadow.transform.Find("Text").GetComponent<Text>();

        //POKE MOVES DETAILS
        pokemonMoves = pokeObject.transform.Find("Moves").gameObject;
        for (int i = 0; i < 4; i++)
        {
            pokemonMovesNameShadow[i] = pokemonMoves.transform.Find("Move" + (i + 1)).GetComponent<Text>();
            pokemonMovesName[i] = pokemonMovesNameShadow[i].transform.Find("Text").GetComponent<Text>();
            pokemonMovesType[i] = pokemonMoves.transform.Find("Move" + (i + 1) + "Type").GetComponent<Image>();
            pokemonMovesPPShadow[i] = pokemonMoves.transform.Find("Move" + (i + 1) + "PP").GetComponent<Text>();
            pokemonMovesPP[i] = pokemonMovesPPShadow[i].transform.Find("Text").GetComponent<Text>();
            pokemonMovesPPTextShadow[i] =
                pokemonMoves.transform.Find("Move" + (i + 1) + "PPText").GetComponent<Text>();
            pokemonMovesPPText[i] = pokemonMovesPPTextShadow[i].transform.Find("Text").GetComponent<Text>();
        }

        pokemonMovesSelectedCategory = pokemonMoves.transform.Find("SelectedCategory").GetComponent<Image>();
        pokemonMovesSelectedPowerShadow = pokemonMoves.transform.Find("SelectedPower").GetComponent<Text>();
        pokemonMovesSelectedPower = pokemonMovesSelectedPowerShadow.transform.Find("Text").GetComponent<Text>();
        pokemonMovesSelectedAccuracyShadow = pokemonMoves.transform.Find("SelectedAccuracy").GetComponent<Text>();
        pokemonMovesSelectedAccuracy =
            pokemonMovesSelectedAccuracyShadow.transform.Find("Text").GetComponent<Text>();
        pokemonMovesSelectedDescriptionShadow =
            pokemonMoves.transform.Find("SelectedDescription").GetComponent<Text>();
        pokemonMovesSelectedDescription =
            pokemonMovesSelectedDescriptionShadow.transform.Find("Text").GetComponent<Text>();
        pokemonMovesSelector = pokemonMoves.transform.Find("MoveSelector").GetComponent<Image>();
        pokemonMovesSelectedMove = pokemonMoves.transform.Find("SelectedMove").GetComponent<Image>();
    }

    void Update()
    {
        playerSpriteMask.sprite = playerSpriteRenderer.sprite;
        opponentSpriteMask.sprite = opponentSpriteRenderer.sprite;
        
        player2SpriteMask.sprite = player2SpriteRenderer.sprite;
        opponent2SpriteMask.sprite = opponent2SpriteRenderer.sprite;
    }
    
    void Start()
    {
        gameObject.SetActive(false);
    }

    private void initAbilityVariables()
    {
        pressurePlayer = false;
        pressureOpponent = false;
    }
    
    //Checking BGM

    public void PlayWildBGM(IPokemon pkm)
    {
        Pokemons id = pkm.Species;
        
        if (id == (Pokemons)643 || id == (Pokemons)644 ||id == (Pokemons)645)
        {
            battleBGM = unovaLegendaryBattleBGM;
            battleBGMLoopStart = unovaLegendaryBattleBGMLoopStart;
            
            BgmHandler.main.PlayOverlay(unovaLegendaryBattleBGM,
                unovaLegendaryBattleBGMLoopStart);
        }
        else
        {
            battleBGM = defaultWildBGM;
            battleBGMLoopStart = defaultWildBGMLoopStart;
            
            BgmHandler.main.PlayOverlay(defaultWildBGM,
                defaultWildBGMLoopStart);
        }
    }


    //////////////////////////////////
    /// ANIMATIONS
    //
    private IEnumerator animateMegaEvolution(bool isOpponent)
    {
        SpriteRenderer s = playerSpriteRenderer;
        SpriteRenderer whiteOverlay = s.transform.Find("WhiteOverlay").GetComponent<SpriteRenderer>();
        GameObject vfx = s.transform.Find("Mega Evolution").gameObject;
        
        // Change camera focus to the mega evolving Pokémon
        //LeanTween.moveLocal(BattleCamera.gameObject, playerFocusCameraAngle[0], 0.45f);
        //yield return new WaitForSeconds(1f);
        
        battleScenehandler.Darken(0.4f);
        
        yield return new WaitForSeconds(0.4f);

        LeanTween.color(s.gameObject, Color.black, 0.6f);
        
        vfx.transform.Find("Little Lights").GetComponent<ParticleSystem>().Play();
        
        SfxHandler.Play(megaEvolveStartClip);
        
        yield return new WaitForSeconds(0.6f);
        
        LeanTween.color(whiteOverlay.gameObject, Color.white, 0.4f);
        
        yield return new WaitForSeconds(0.6f);
        
        vfx.transform.Find("Little Lights").GetComponent<ParticleSystem>().Stop();
        
        vfx.transform.Find("Big Circle").GetComponent<ParticleSystem>().Play();
        
        yield return new WaitForSeconds(1.3f);
        
        LeanTween.color(whiteOverlay.gameObject, Color.clear, 0);
        LeanTween.color(s.gameObject, Color.white, 0);
        
        yield return new WaitForSeconds(1.6f);
        
        vfx.transform.Find("Sparks").GetComponent<ParticleSystem>().Play();
        StartCoroutine(shakeCamera(BattleCamera.transform));

        SfxHandler.Play(megaEvolveFinishClip);

        yield return new WaitForSeconds(0.5f);
        
        PlayCry(pokemon[0]);
        
        battleScenehandler.Lighten(0.4f);
        
        yield return null;
    }
    
    private IEnumerator animateStatDisplay(Transform statDisplay, bool doubleBattle)
    {
        Vector3 startPosition = statDisplay.GetComponent<RectTransform>().localPosition;
        int y = -77;
        float delay = 0.2f;

        if (doubleBattle)
        {
            if (statDisplay.gameObject.name == "playerStats0")
            {
                y = -45;
            }
            else if (statDisplay.gameObject.name == "playerStats1")
            {
                y = -77;
            }
        }
        else
        {
            y = -77;
        }
        
        while (true)
        {
            statDisplay.GetComponent<RectTransform>().localPosition = new Vector3(startPosition.x, y, startPosition.z);
            yield return new WaitForSeconds(delay);
            
            statDisplay.GetComponent<RectTransform>().localPosition = new Vector3(startPosition.x, y + 1, startPosition.z);
            yield return new WaitForSeconds(delay);
            
            statDisplay.GetComponent<RectTransform>().localPosition = new Vector3(startPosition.x, y, startPosition.z);
            yield return new WaitForSeconds(delay);
            
            statDisplay.GetComponent<RectTransform>().localPosition = new Vector3(startPosition.x, y - 1, startPosition.z);
            yield return new WaitForSeconds(delay);
        }
    }

    public IEnumerator DisplayAbility(int index, string ability_name)
    {
        RectTransform ability;
        string pkm_name = pokemon[index].Name;
        //string s = Language.getLang() switch
        //{
        //    _ => pkm_name[^1] == 's' ? "'" : "'s"
        //};
        string s = pkm_name[pkm_name.Length - 1] == 's' ? "'" : "'s";

        if (index < 3)
        {
            ability = transform.Find("player_ability").GetComponent<RectTransform>();

            foreach (Transform obj in ability.transform.Find("pkm_name"))
            {
                obj.GetComponent<Text>().text = Language.getLang() switch
                {
                    Language.Country.FRANCAIS => ability_name,
                    _ => pokemon[index].Name + s
                };
            }
            
            foreach (Transform obj in ability.transform.Find("ability"))
            {
                obj.GetComponent<Text>().text = Language.getLang() switch
                {
                    Language.Country.FRANCAIS => "de " + pokemon[index].Name,
                    _ => ability_name
                };
            }

            SfxHandler.Play(laser);
            
            LeanTween.move(ability, new Vector3(0, -19, 0), 0.3f);
        }
        else
        {
            ability = transform.Find("opponent_ability").GetComponent<RectTransform>();
            
            foreach (Transform obj in ability.transform.Find("pkm_name"))
            {
                obj.GetComponent<Text>().text = Language.getLang() switch
                {
                    Language.Country.FRANCAIS => ability_name,
                    _ => pokemon[index].Name + s
                };
            }
            
            foreach (Transform obj in ability.transform.Find("ability"))
            {
                obj.GetComponent<Text>().text = Language.getLang() switch
                {
                    Language.Country.FRANCAIS => "de " + pokemon[index].Name,
                    _ => ability_name
                };
            }
            
            SfxHandler.Play(laser);
            
            LeanTween.move(ability, new Vector3(342.5f, 22, 0), 0.3f);
        }

        yield return new WaitForSeconds(0.3f);
    }

    public IEnumerator HideAbility(int index)
    {
        RectTransform ability;

        if (index < 3)
        {
            ability = transform.Find("player_ability").GetComponent<RectTransform>();

            LeanTween.move(ability, new Vector3(-150, -19, 0), 0.3f);
        }
        else
        {
            ability = transform.Find("opponent_ability").GetComponent<RectTransform>();

            LeanTween.move(ability, new Vector3(490, 22, 0), 0.3f);
        }
        
        yield return new WaitForSeconds(0.3f);
    }

    private IEnumerator animateOpponentTrainer(SpriteRenderer trainer, Sprite[] animation)
    {
        int frame = 0;
        while (frame < animation.Length)
        {
            if (animation.Length > 0)
            {
                trainer.sprite = animation[frame++];
            }
            yield return new WaitForSeconds(0.04f);
        }

        yield return null;
    }

    private IEnumerator fadeopponentTrainer(SpriteRenderer trainer, bool isShadow)
    {
        Vector3 trainer_pos = trainer.transform.localPosition;

        //fade
        
        float increment = 0;
        float time = 0.3f;
        while (increment < 1)
        {
            increment += (1 / time) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }

            if (!isShadow)
            {
                //move while fading
                trainer.transform.localPosition = new Vector3(trainer_pos.x, trainer_pos.y, trainer_pos.z + increment*1.75f);
                trainer.color = new Color(1, 1, 1, 1 - increment);
            }
            else
            {
                trainer.color = new Color(0, 0, 0, 0.5f - increment/2);
            }
            
            yield return null;
        }

        trainer.transform.localScale = new Vector3(0, 0, 0);

        if (!isShadow)
        {
            trainer.transform.localPosition = trainer_pos;
            trainer.color = Color.white;
        }
        else
        {
            trainer.color = new Color(0, 0, 0, 0.5f);
        }

        yield return null;
    }

    private IEnumerator introTravelling(float time)
    {
        //ToDo: Missing Battle Camera logic
        //CPC_CameraPath camera_path =
        //    BattleScene.transform.Find("CameraPaths/Intro").GetComponent<CPC_CameraPath>();
        //camera_path.PlayPath(time);
        
        yield return new WaitForSeconds(time);
        
        yield return null;
    }
    
    private IEnumerator playerLaunchingBall(bool isTrainerBattle, bool doubleBattle, Trainer ally)
    {
        Animator trainer = BattleScene.transform.Find("player_trainer").GetComponent<Animator>();
        Animator trainer2 = null;
        trainer.Play("bw_battleback_idle");

        if (doubleBattle && ally != null)
        {
            trainer2 = BattleScene.transform.Find("ally_trainer").GetComponent<Animator>();
            
            trainer2.Play("bw_battleback_idle");
        }

        // Camera Movement
        LeanTween.move(BattleCamera.gameObject, trainerFocusCameraAngle[0], 0.45f);
        yield return new WaitForSeconds(0.45f);

        yield return new WaitForSeconds(0.3f);

        if (isTrainerBattle)
        {
            StartCoroutine(dismissPartyBar(false));
        }

        trainer.Play("bw_battleback_prepare");

        if (doubleBattle && ally != null)
        {
            trainer2.Play("bw_battleback_prepare");
        }
        
        yield return new WaitForSeconds((17/60));

        Dialog.DrawBlackFrame();
        if (doubleBattle && pokemon[1] != null)
        {
            StartCoroutine(Dialog.DrawTextSilent("Go " + pokemon[0].Name + "!\nGo " + pokemon[1].Name + "!"));
        }
        else
        {
            StartCoroutine(Dialog.DrawTextSilent("Go " + pokemon[0].Name + "!"));
        }
        
        yield return new WaitForSeconds(0.5f);
        Dialog.UndrawDialogBox();
        
        trainer.Play("bw_battleback_launch");
        
        if (doubleBattle && ally != null)
        {
            trainer2.Play("bw_battleback_launch");
        }
        
        yield return new WaitForSeconds(0.4f);
        
        SfxHandler.Play(pokeballThrowClip);

        LeanTween.color(trainer.transform.Find("body/m_bw_battleback_1").gameObject, Color.clear, 0.3f);
        LeanTween.color(trainer.transform.Find("body/m_bw_battleback_2").gameObject, Color.clear, 0.3f);
        LeanTween.color(trainer.transform.Find("body/m_bw_battleback_3").gameObject, Color.clear, 0.3f);
        LeanTween.color(trainer.transform.Find("body/arm1").gameObject, Color.clear, 0.3f);
        LeanTween.color(trainer.transform.Find("body/arm2").gameObject, Color.clear, 0.3f);
        LeanTween.color(trainer.transform.Find("body/arm3").gameObject, Color.clear, 0.3f);
        
        if (doubleBattle && ally != null)
        {
            LeanTween.color(trainer2.transform.Find("body/m_bw_battleback_1").gameObject, Color.clear, 0.3f);
            LeanTween.color(trainer2.transform.Find("body/m_bw_battleback_2").gameObject, Color.clear, 0.3f);
            LeanTween.color(trainer2.transform.Find("body/m_bw_battleback_3").gameObject, Color.clear, 0.3f);
            LeanTween.color(trainer2.transform.Find("body/arm1").gameObject, Color.clear, 0.3f);
            LeanTween.color(trainer2.transform.Find("body/arm2").gameObject, Color.clear, 0.3f);
            LeanTween.color(trainer2.transform.Find("body/arm3").gameObject, Color.clear, 0.3f);
        }

        if (doubleBattle)
        {
            if (ally == null)
            {
                if (pokemon[1] != null)
                    StartCoroutine(playerBallAnim(1));
            }
            else
            {
                StartCoroutine(allyBallAnim());
            }
        }
        yield return StartCoroutine(playerBallAnim(0));
        //yield return new WaitForSeconds(((110 - 30)/60));
        //TODO Modify Opacity in animation or directly here
        //trainer.gameObject.transform.localScale = new Vector3(0, 0, 0);
        
        //yield return new WaitForSeconds(0.35f);

        if (doubleBattle)
        {
            if (pokemon[1] != null)
                StartCoroutine(releasePokemon(player2SpriteRenderer, true));
        }
        yield return StartCoroutine(releasePokemon(playerSpriteRenderer, doubleBattle));

        //SfxHandler.Play(pokeballOpenClip);

        yield return new WaitForSeconds(1f);

        /*
        LeanTween.moveLocal(BattleCamera.gameObject, baseCameraAngle[1], 0.3f);
        LeanTween.rotateLocal(BattleCamera.gameObject, baseCameraAngle[1], 0.3f);
        yield return new WaitForSeconds(0.3f);
        */

        trainer.gameObject.transform.localScale = new Vector3(0, 0, 0);
        yield return null;
    }
    
    private IEnumerator animatePokemon(Image pokemon, Sprite[] animation)
    {
        int frame = 0;
        while (animation != null)
        {
            if (animation.Length > 0)
            {
                if (frame < animation.Length - 1)
                {
                    frame += 1;
                }
                else
                {
                    frame = 0;
                }
                pokemon.sprite = animation[frame];
            }
            yield return new WaitForSeconds(0.08f);
        }
    }
    
    private IEnumerator animatePokemonSpriteRenderer(SpriteRenderer pokemon, Sprite[] animation)
    {
        int frame = 0;
        while (animation != null)
        {
            if (animation.Length > 0)
            {
                if (frame < animation.Length - 1)
                {
                    frame += 1;
                }
                else
                {
                    frame = 0;
                }
                pokemon.sprite = animation[frame];
            }
            yield return new WaitForSeconds(0.08f);
        }
    }

    private IEnumerator animateOverlayer(RawImage overlay, Texture overlayTex, float verMovement, float hozMovement,
        float time, float fadeTime)
    {
        overlay.gameObject.SetActive(true);
        overlay.texture = overlayTex;
        float fadeStartIncrement = (time - fadeTime) / time;
        float initialAlpha = overlay.color.a;

        float increment = 0;
        while (increment < 1)
        {
            increment += (1 / time) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }

            overlay.uvRect = new Rect(hozMovement * increment, verMovement * increment, 1, 1);
            if (increment > fadeStartIncrement)
            {
                float increment2 = (increment - fadeStartIncrement) / (1 - fadeStartIncrement);
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b,
                    initialAlpha * (1 - increment2));
            }
            yield return null;
        }
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, initialAlpha);
        overlay.gameObject.SetActive(false);
    }

    /// Slides the Pokemon's Platform across the screen. Takes 1.9f seconds.
    private IEnumerator slidePokemon(Image platform, Image pokemon, bool showPokemon, bool fromRight,
        Vector3 destinationPosition)
    {
        Vector3 startPosition = (fromRight)
            ? new Vector3(171 + (platform.rectTransform.sizeDelta.x * platform.rectTransform.localScale.x / 2f),
                destinationPosition.y, 0)
            : new Vector3(-171 - (platform.rectTransform.sizeDelta.x * platform.rectTransform.localScale.x / 2f),
                destinationPosition.y, 0);
        Vector3 distance = destinationPosition - startPosition;

        pokemon.color = new Color(0.25f, 0.25f, 0.25f, 1);

        pokemon.transform.parent.parent.gameObject.SetActive(showPokemon);

        float speed = 1.5f;
        float increment = 0f;
        while (increment < 1)
        {
            increment += (1 / speed) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1f;
            }
            platform.rectTransform.localPosition = startPosition + (distance * increment);
            yield return null;
        }

        speed = 0.4f;
        increment = 0f;
        while (increment < 1)
        {
            increment += (1 / speed) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1f;
            }
            pokemon.color = new Color(0.25f + (0.25f * increment), 0.25f + (0.25f * increment),
                0.25f + (0.25f * increment), 1);
            yield return null;
        }
    }

    private IEnumerator slideTrainer(Image platform, Image trainer, bool isOpponent, bool slideOut)
    {
        Vector3 startPosition = trainer.rectTransform.localPosition;
        //assume !slide out for both
        float destinationPositionX = (isOpponent && !slideOut) ? platform.rectTransform.sizeDelta.x * 0.3f : 0;
        //if it actually was slide out, use the formula to find the hidden position
        if (slideOut)
        {
            destinationPositionX = (171 - Mathf.Abs(platform.rectTransform.localPosition.x)) /
                                   platform.rectTransform.localScale.x + trainer.rectTransform.sizeDelta.x / 2f;
        }
        //flip direction if is player
        if (!isOpponent)
        {
            destinationPositionX = -destinationPositionX;
        }

        Vector3 distance = new Vector3(destinationPositionX, startPosition.y, 0) - startPosition;

        float speed = 128f;
        float time = Mathf.Abs(distance.x) / speed;

        float increment = 0f;
        while (increment < 1)
        {
            increment += (1 / time) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1f;
            }
            trainer.rectTransform.localPosition = startPosition + (distance * increment);
            yield return null;
        }
    }

    private IEnumerator animatePlayerThrow(Image trainer, Sprite[] throwAnim, bool finishThrow)
    {
        trainer.sprite = throwAnim[1];
        yield return new WaitForSeconds(0.4f);
        trainer.sprite = throwAnim[2];
        yield return new WaitForSeconds(0.05f);
        trainer.sprite = throwAnim[3];
        if (finishThrow)
        {
            yield return new WaitForSeconds(0.05f);
            trainer.sprite = throwAnim[4];
        }
    }

    private Sprite[] getBallSprite(string caughtBall)
    {
        string spriteName;
        switch (caughtBall)
        {
            case "Master Ball":
                spriteName = "masterball";
                break;
            case "Hyper Ball":
                spriteName = "hyperball";
                break;
            case "Super Ball":
                spriteName = "superball";
                break;
            default:
                spriteName = "pokeball";
                break;
        }
        
        return Resources.LoadAll<Sprite>("Balls/" + spriteName);
    }

    private IEnumerator animateBall(SpriteRenderer ball, Sprite[] animation)
    {
        int frame = 0;
        while (animation != null)
        {
            if (animation.Length > 0)
            {
                if (frame < animation.Length - 1)
                {
                    frame += 1;
                }
                else
                {
                    frame = 0;
                }
                ball.sprite = animation[frame];
            }
            yield return new WaitForSeconds(0.04f);
        }
    }

    private IEnumerator playerBallAnim(int index)
    {
        Vector3 position;
        Vector3 distance;
        Vector3 target;
        SpriteRenderer ball;
        float height = 4f;

        if (index == 0)
        {
            target = playerSpriteRenderer.transform.position;
            ball = playerBall;
        }
        else
        {
            target = player2SpriteRenderer.transform.position;
            ball = playerBall2;
        }

        position = ball.transform.position;
        distance = (target - position) * 0.7f;

        Sprite[] animation = new Sprite[0]; //getBallSprite(pokemon[index].getCaughtBall());
        Coroutine anim_coroutine = StartCoroutine(animateBall(ball, animation));

        ball.gameObject.SetActive(true);

        float speed = 0.85f;

        LeanTween.moveX(ball.gameObject, position.x + distance.x, speed);
        LeanTween.moveY(ball.gameObject, position.y + height, speed/2)
            .setLoopPingPong(1);
        LeanTween.moveZ(ball.gameObject, position.z + distance.z, speed);

        yield return new WaitForSeconds(speed);

        yield return new WaitForSeconds(0.15f);

        StopCoroutine(anim_coroutine);
        ball.transform.position = position;
        ball.gameObject.SetActive(false);
        yield return null;
    }
    
    private IEnumerator allyBallAnim()
    {
        Vector3 position;
        Vector3 distance;
        Vector3 target;
        SpriteRenderer ball;
        float height = 4f;

        target = player2SpriteRenderer.transform.position;
        ball = allyBall;

        position = ball.transform.position;
        distance = (target - position) * 0.7f;

        Sprite[] animation = new Sprite[0]; //getBallSprite(pokemon[1].getCaughtBall());
        Coroutine anim_coroutine = StartCoroutine(animateBall(ball, animation));

        ball.gameObject.SetActive(true);

        float speed = 0.85f;

        LeanTween.moveX(ball.gameObject, position.x + distance.x, speed);
        LeanTween.moveY(ball.gameObject, position.y + height, speed/2)
            .setLoopPingPong(1);
        LeanTween.moveZ(ball.gameObject, position.z + distance.z, speed);

        yield return new WaitForSeconds(speed);

        yield return new WaitForSeconds(0.15f);

        StopCoroutine(anim_coroutine);
        ball.transform.position = position;
        ball.gameObject.SetActive(false);
        yield return null;
    }
    
    private IEnumerator opponentBallAnim(int index)
    {
        float offset = 3;
        Vector3 opponentPosition = opponentSpriteRenderer.transform.position;
        Vector3 position = new Vector3(opponentPosition.x, opponentPosition.y+offset, opponentPosition.z);
        
        Sprite[] animation = new Sprite[0]; //getBallSprite(pokemon[index].getCaughtBall());

        SpriteRenderer ball;
        if (index == 3)
        {
            ball = opponentBall;
        }
        else
        {
            ball = opponentBall2;
        }
        Coroutine anim_coroutine = StartCoroutine(animateBall(ball, animation));
        
        ball.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.4f);
        
        ball.gameObject.SetActive(false);
        StopCoroutine(anim_coroutine);
        
        yield return null;
    }

    private IEnumerator releasePokemon(Image pokemon)
    {
        Vector2 normalSize = pokemon.rectTransform.sizeDelta;
        pokemon.rectTransform.sizeDelta = new Vector2(0, 0);
        pokemon.color = new Color(0.812f, 0.312f, 0.312f, 1);

        pokemon.transform.parent.parent.gameObject.SetActive(true);

        SfxHandler.Play(pokeballOpenClip);

        float speed = 0.3f;
        float increment = 0f;
        while (increment < 1)
        {
            increment += (1 / speed) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1f;
            }
            pokemon.rectTransform.sizeDelta = normalSize * increment;
            pokemon.color = new Color(0.812f - (0.312f * increment), 0.312f + (0.188f * increment),
                0.312f + (0.188f * increment), 1);
            yield return null;
        }

        pokemon.rectTransform.sizeDelta = normalSize;
        pokemon.color = new Color(0.5f, 0.5f, 0.5f, 1);
    }
    
    private IEnumerator releasePokemon(SpriteRenderer sprite, bool doubleBattle)
    {
        RectTransform pokemonSprite = sprite.gameObject.GetComponent<RectTransform>();
        Transform mask = sprite.gameObject.transform.parent;
            
        SpriteRenderer overlay = sprite.transform.Find("WhiteOverlay").GetComponent<SpriteRenderer>();

        Vector3 normalSize = new Vector3();
        if (sprite.transform.parent.parent.parent.name.Contains("opponent"))
        {
            normalSize = new Vector3(-1, 1, 1);
        }
        else
        {
            normalSize = new Vector3(1, 1, 1);
        }
        

        float offset = 3;
        float startPositionY = mask.transform.position.y + offset;
        mask.transform.position = new Vector3(mask.transform.position.x, startPositionY, mask.transform.position.z);
        pokemonSprite.localScale = new Vector3(0, 0, 1);
        //overlay.color = new Color(1f, 1f, 1f, 0);

        pokemonSprite.transform.parent.parent.gameObject.SetActive(true);

        SfxHandler.Play(pokeballOpenClip);
        
        switch (pokemonSprite.transform.parent.parent.parent.gameObject.name)
        {
            case "player0":
                playerPokeballParticles.Play();
                break;
            case "player1":
                player2PokeballParticles.Play();
                break;
            case "opponent0":
                opponentPokeballParticles.Play();
                break;
            case "opponent1":
                opponent2PokeballParticles.Play();
                break;
        }
        
        float speed = 0.5f;
        float increment = 0f;
        while (increment < 1)
        {
            increment += (1 / speed) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1f;
            }
            pokemonSprite.localScale = normalSize * increment;
            overlay.color = new Color(1, 1, 1, 1-increment);
            yield return null;
        }

        pokemonSprite.localScale = normalSize;
        overlay.color = new Color(1, 1, 1, 0);
        
        IPokemon SFXPokemontarget = pokemon[0];
        int statTarget = 0;
        switch (pokemonSprite.transform.parent.parent.parent.gameObject.name)
        {
            case "player0":
                SFXPokemontarget = pokemon[0];
                statTarget = 0;
                break;
            case "player1":
                SFXPokemontarget = pokemon[1];
                statTarget = 1;
                break;
            case "opponent0":
                SFXPokemontarget = pokemon[3];
                statTarget = 3;
                break;
            case "opponent1":
                SFXPokemontarget = pokemon[4];
                statTarget = 4;
                break;
        }
        
        PlayCry(SFXPokemontarget);
        StartCoroutine(slidePokemonStats(statTarget, false, doubleBattle));

        yield return new WaitForSeconds(0.5f);
        increment = 0f;
        speed = 0.25f;
        while (increment < 1)
        {
            increment += (1 / speed) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1f;
            }
            mask.transform.position = new Vector3(mask.transform.position.x, startPositionY - increment*offset, mask.transform.position.z);
            yield return null;
        }

        mask.transform.position = new Vector3(mask.transform.position.x, mask.transform.position.y, mask.transform.position.z);
        sprite.transform.parent.parent.Find("Shadow").localScale = new Vector3(1, 1, 1);

        //PokemonData pokeData = PokemonDatabase.getPokemon(SFXPokemontarget.Species);
        PokemonUnity.Monster.Data.PokemonData pokeData = Kernal.PokemonData[SFXPokemontarget.Species];
        if (pokeData.Weight >= 100)
        {
            SfxHandler.Play(fallLarge);
        }
        else if (pokeData.Weight >= 25)
        {
            SfxHandler.Play(fallMid);
        }
        else if (pokeData.Weight >= 0)
        {
            SfxHandler.Play(fallSmall);
        }
        
        yield return StartCoroutine(shakeCamera(BattleCamera.GetComponent<Transform>()));
    }


    private IEnumerator shakeCamera(Transform camera)
    {
        Vector3 initial_pos = camera.position;
        float distance = 0.19f;
        float time = 0.125f/4;
        
        LeanTween.moveY(camera.gameObject, initial_pos.y + distance, time/2);
        yield return new WaitForSeconds(time/2);
        LeanTween.moveY(camera.gameObject, initial_pos.y - distance, time);
        yield return new WaitForSeconds(time);
        LeanTween.moveY(camera.gameObject, initial_pos.y, time/2);
        yield return new WaitForSeconds(time/2);
    }
    
    
    private IEnumerator withdrawPokemon(Image pokemon)
    {
        Vector2 normalSize = pokemon.rectTransform.sizeDelta;

        SfxHandler.Play(pokeballOpenClip);

        float speed = 0.3f;
        float increment = 0f;
        while (increment < 1)
        {
            increment += (1 / speed) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1f;
            }
            pokemon.rectTransform.sizeDelta = normalSize * (1 - increment);
            pokemon.color = new Color(0.5f + (0.312f * increment), 0.5f - (0.188f * increment),
                0.5f - (0.188f * increment), 1);
            yield return null;
        }
        pokemon.transform.parent.parent.gameObject.SetActive(false);

        pokemon.rectTransform.sizeDelta = normalSize;
        pokemon.color = new Color(0.5f, 0.5f, 0.5f, 1);
    }
    
    private IEnumerator withdrawPokemon(SpriteRenderer sprite)
    {
        RectTransform pokemonSprite = sprite.gameObject.GetComponent<RectTransform>();
        Transform mask = sprite.gameObject.transform.parent;

        Vector3 normalSize = new Vector3();
        if (sprite.transform.parent.parent.parent.name.Contains("opponent"))
        {
            normalSize = new Vector3(-1, 1, 1);
        }
        else
        {
            normalSize = new Vector3(1, 1, 1);
        }
        
        sprite.transform.parent.parent.Find("Shadow").localScale = new Vector3(0, 0, 0);
            
        SpriteRenderer overlay = sprite.transform.Find("WhiteOverlay").GetComponent<SpriteRenderer>();

        SfxHandler.Play(pokeballOpenClip);

        float speed = 0.3f;
        float increment = 0f;
        while (increment < 1)
        {
            increment += (1 / speed) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1f;
            }
            pokemonSprite.localScale = normalSize * (1 - increment);
            overlay.color = new Color(1,1 ,
                1, increment);
            yield return null;
        }
        //pokemon.transform.parent.parent.gameObject.SetActive(false);
        
        overlay.color = new Color(1,1 ,
            1, 1);
    }

    private IEnumerator faintPokemonAnimation(Image pokemon)
    {
        Vector3 startPosition = pokemon.rectTransform.localPosition;
        Vector3 distance = new Vector3(0, pokemon.rectTransform.sizeDelta.y, 0);
        

        pokemon.transform.parent.parent.gameObject.SetActive(true);

        SfxHandler.Play(faintClip);

        float speed = 0.5f;
        float increment = 0f;
        while (increment < 1)
        {
            increment += (1 / speed) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1f;
            }

            //	pokemon.fillAmount = 1-increment;
            pokemon.rectTransform.localPosition = startPosition - (distance * increment);

            yield return null;
        }

        pokemon.transform.parent.parent.gameObject.SetActive(false);

        pokemon.fillAmount = 1f;
        pokemon.rectTransform.localPosition = startPosition;
    }
    
    private IEnumerator faintPokemonAnimation(SpriteRenderer sprite)
    {
        RectTransform pokemonSprite = sprite.gameObject.GetComponent<RectTransform>();
        Transform mask = sprite.gameObject.transform.parent;
        Transform shadow = sprite.gameObject.transform.parent.parent.Find("Shadow").transform;
        
        Vector3 startPosition = mask.localPosition;
        Vector3 distance = new Vector3(0, 3, 0);
        
        shadow.localScale = new Vector3(0, 0, 0);

        //pokemon.transform.parent.parent.gameObject.SetActive(true);

        SfxHandler.Play(faintClip);

        LeanTween.color(sprite.gameObject, Color.clear, 0.25f);

        float speed = 0.25f;
        float increment = 0f;
        while (increment < 1)
        {
            increment += (1 / speed) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1f;
            }

            //	pokemon.fillAmount = 1-increment;
            mask.localPosition = startPosition - (distance * increment);

            yield return null;
        }

        //pokemon.transform.parent.parent.gameObject.SetActive(false);
        
        
        pokemonSprite.localScale = new Vector3(0, 0, 0);
        sprite.color = Color.white;
        mask.localPosition = startPosition;
    }

    private IEnumerator slidePokemonStats(int position, bool retract, bool doubleBattle)
    {
        float posX = retract ? 300 : 164;
        float firstPosX = retract ? 164 : 300;

        //flip values if opponent stats
        if (doubleBattle)
        {
            if (position <= 2)
            {
                posX = -posX;
                firstPosX = -firstPosX;
            }
            else
            {
                if (position == 3)
                {
                    posX = retract ? 300 : 63;
                }
            }
        }
        else
        {
            if (position <= 2)
            {
                posX = -posX;
                firstPosX = -firstPosX;
            }
        }

        pokemonStatsDisplay[position].gameObject.SetActive(true);

        LeanTween.moveX(pokemonStatsDisplay[position].rectTransform, firstPosX, 0);
        LeanTween.moveX(pokemonStatsDisplay[position].rectTransform, posX, 0.5f);
        
        yield return new WaitForSeconds(0.5f);
    }
    
    private IEnumerator slidePokemonStatsFast(int position, bool retract)
    {
        float posX = retract ? 300 : 164;
        float firstPosX = retract ? 164 : 300;

        //flip values if opponent stats
        if (position <= 2)
        {
            posX = -posX;
            firstPosX = -firstPosX;
        }

        pokemonStatsDisplay[position].gameObject.SetActive(true);

        LeanTween.moveX(pokemonStatsDisplay[position].rectTransform, firstPosX, 0);
        LeanTween.moveX(pokemonStatsDisplay[position].rectTransform, posX, 0.5f);
        
        yield return new WaitForSeconds(0.2f);
    }

    private void hidePartyBar(bool isOpponent)
    {
        Image bar = (isOpponent) ? opponentPartyBar : playerPartyBar;
        Image[] space = (isOpponent) ? opponentPartyBarSpace : playerPartyBarSpace;

        bar.color = new Color(bar.color.r, bar.color.g, bar.color.b, 0);
        bar.rectTransform.sizeDelta = new Vector2(0, bar.rectTransform.sizeDelta.y);
        for (int i = 0; i < 6; i++)
        {
            space[i].color = new Color(space[i].color.r, space[i].color.g, space[i].color.b, 0);
            space[i].rectTransform.localPosition = new Vector3(-96 + (16 * i) + 128,
                space[i].rectTransform.localPosition.y);
        }
    }

    /// <summary> Displays the party length bar. </summary>
    /// <param name="isOpponent">Does this bar belong to the opponent?</param>
    /// <param name="party">The party, used to determine length and if fainted.</param>
    private IEnumerator displayPartyBar(bool isOpponent, IPokemon[] party, IPokemon[] party2 = null)
    {
        Image bar = (isOpponent) ? opponentPartyBar : playerPartyBar;
        Image[] space = (isOpponent) ? opponentPartyBarSpace : playerPartyBarSpace;

        hidePartyBar(isOpponent); //this line reset the position to hidden, but also sets alpha to 0
        //set alpha to 1
        bar.color = new Color(bar.color.r, bar.color.g, bar.color.b, 1);
        for (int i = 0; i < 6; i++)
        {
            space[i].color = new Color(space[i].color.r, space[i].color.g, space[i].color.b, 1);
        }

        StartCoroutine(stretchBar(bar, bar.sprite.texture.width/2, 320));
        yield return new WaitForSeconds(0.1f);
        if (party2 == null)
        {
            for (int i = 0; i < 6; i++)
            {
                //Set space sprite
                space[i].sprite = partySpaceTex;
                if (party.Length > i)
                {
                    if (party[i] != null)
                    {
                        if (party[i].Status == Status.FAINT)
                        {
                            space[i].sprite = partyFaintTex;
                        }
                        else if (party[i].Status == Status.NONE)
                        {
                            space[i].sprite = partyBallTex;
                        }
                        else
                        {
                            space[i].sprite = partyStatusTex;
                        }
                    }
                }
                //slide down the line
                StartCoroutine(slidePartyBarBall(space[i], -96 + (14 * i) + 128, -99 + (14 * i), 0.35f));
                yield return new WaitForSeconds(0.05f);
            }
        }
        else
        {
            for (int i = 0; i <= 2; i++)
            {
                //Set space sprite
                space[i].sprite = partySpaceTex;
                if (party2.Length > i)
                {
                    if (party2[i] != null)
                    {
                        if (party2[i].Status == Status.FAINT)
                        {
                            space[i].sprite = partyFaintTex;
                        }
                        else if (party2[i].Status == Status.NONE)
                        {
                            space[i].sprite = partyBallTex;
                        }
                        else
                        {
                            space[i].sprite = partyStatusTex;
                        }
                    }
                }
                //slide down the line
                StartCoroutine(slidePartyBarBall(space[i], -96 + (14 * i) + 128, -99 + (14 * i), 0.35f));
                yield return new WaitForSeconds(0.05f);
            }
            for (int i = 3; i <= 5; i++)
            {
                //Set space sprite
                space[i].sprite = partySpaceTex;
                // Splits the party bar in two with the 3 first Pokémon of both parties
                if (party.Length > i - 3)
                {
                    if (party[i - 3] != null)
                    {
                        if (party[i - 3].Status == Status.FAINT)
                        {
                            space[i].sprite = partyFaintTex;
                        }
                        else if (party[i - 3].Status == Status.NONE)
                        {
                            space[i].sprite = partyBallTex;
                        }
                        else
                        {
                            space[i].sprite = partyStatusTex;
                        }
                    }
                }
                //slide down the line
                StartCoroutine(slidePartyBarBall(space[i], -96 + (14 * i) + 128, -99 + (14 * i), 0.35f));
                yield return new WaitForSeconds(0.05f);
            }
        }
        
        //Wait for last space to stop moving
        yield return new WaitForSeconds(0.3f);
        //Slide all spaces back a tiny bit
        for (int i = 0; i < 6; i++)
        {
            StartCoroutine(slidePartyBarBall(space[i], -99 + (16 * i), -96 + (16 * i), 0.1f, false));
        }
        //Wait for last space to stop moving
        yield return new WaitForSeconds(0.1f);
    }

    private IEnumerator dismissPartyBar(bool isOpponent)
    {
        Image bar = (isOpponent) ? opponentPartyBar : playerPartyBar;
        Image[] space = (isOpponent) ? opponentPartyBarSpace : playerPartyBarSpace;

        //Slide all out and fade
        Image[] images = new Image[space.Length + 1];
        images[0] = bar;
        for (int i = 0; i < space.Length; i++)
        {
            images[i + 1] = space[i];
        }
        StartCoroutine(fadeImages(images, 0.6f));

        StartCoroutine(stretchBar(bar, 192, 128));
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < 6; i++)
        {
            //slide down the line
            StartCoroutine(slidePartyBarBall(space[i], -96 + (16 * i), -96 + (16 * i) - 192, 0.5f));
            yield return new WaitForSeconds(0.05f + (0.02f * i));
        }
        //Wait for last space to stop moving
        yield return new WaitForSeconds(0.45f);
        hidePartyBar(isOpponent);
    }

    private IEnumerator fadeImages(Image[] images, float time)
    {
        float increment = 0f;
        while (increment < 1)
        {
            increment += (1 / time) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1f;
            }

            for (int i = 0; i < images.Length; i++)
            {
                images[i].color = new Color(images[i].color.r, images[i].color.g, images[i].color.b, 1 - increment);
            }
            yield return null;
        }
    }

    private IEnumerator slidePartyBarBall(Image ball, float startX, float destinationX, float speed)
    {
        yield return StartCoroutine(slidePartyBarBall(ball, startX, destinationX, speed, true));
    }
    private IEnumerator slidePartyBarBall(Image ball, float startX, float destinationX, float speed, bool rotate)
    {
        ball.rectTransform.localPosition = new Vector3(startX, ball.rectTransform.localPosition.y, 0);

        float distanceX = destinationX - startX;
        float rotation = (distanceX >= 0) ? 1 : -1;
        Quaternion startRotation = ball.rectTransform.localRotation;

        float increment = 0;
        while (increment < 1)
        {
            increment += (1 / speed) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }

            ball.rectTransform.localPosition = new Vector3(startX + (distanceX * increment),
                ball.rectTransform.localPosition.y, 0);
            if (rotate)
                ball.rectTransform.localRotation = Quaternion.Euler(ball.transform.eulerAngles.x,ball.transform.eulerAngles.y,0 + increment*360*rotation);
            yield return null;
        }

        ball.rectTransform.localRotation = startRotation;

    }

    private IEnumerator animatePartyIcons()
    {
        bool animating = true;
        while (animating)
        {
            for (int i = 0; i < 6; i++)
            {
                pokemonSlotIcon[i].sprite = pokemonIconAnim[i][0];
            }
            yield return new WaitForSeconds(0.15f);
            for (int i = 0; i < 6; i++)
            {
                pokemonSlotIcon[i].sprite = (i == pokePartyPosition) ? pokemonIconAnim[i][1] : pokemonIconAnim[i][0];
            }
            yield return new WaitForSeconds(0.15f);

            for (int i = 0; i < 6; i++)
            {
                pokemonSlotIcon[i].sprite = (i == pokePartyPosition) ? pokemonIconAnim[i][0] : pokemonIconAnim[i][1];
            }
            yield return new WaitForSeconds(0.15f);
            for (int i = 0; i < 6; i++)
            {
                pokemonSlotIcon[i].sprite = pokemonIconAnim[i][1];
            }
            yield return new WaitForSeconds(0.15f);
        }
    }


    private IEnumerator stretchBar(Image bar, float targetSize)
    {
        yield return StartCoroutine(stretchBar(bar, targetSize, 32f, false, null, null, 0));
    }

    private IEnumerator stretchBar(Image bar, float targetSize, float pixelsPerSec)
    {
        yield return StartCoroutine(stretchBar(bar, targetSize, pixelsPerSec, false, null, null, 0));
    }

    private IEnumerator stretchBar(Image bar, float targetSize, float pixelsPerSec, bool isHP, Text hpText,
        Text hpTextShadow, int endValue)
    {
        float increment = 0f;
        if (pixelsPerSec <= 0)
        {
            pixelsPerSec = 32;
        }
        float startSize = bar.rectTransform.sizeDelta.x;
        float distance = targetSize - startSize;
        float time = Mathf.Abs(distance) / pixelsPerSec;

        int startValue = (hpText != null) ? int.Parse(hpText.text) : 0;
        float valueDistance = endValue - startValue;

        while (increment < 1)
        {
            increment += (1 / time) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }

            bar.rectTransform.sizeDelta = new Vector2(startSize + (distance * increment), bar.rectTransform.sizeDelta.y);

            if (isHP)
            {
                setHPBarColor(bar, 51f);
            }
            if (hpText != null)
            {
                hpText.text = "" + (startValue + Mathf.FloorToInt(valueDistance * increment));
                if (hpTextShadow != null)
                {
                    hpTextShadow.text = hpText.text;
                }
            }
            yield return null;
        }
    }

    //////////////////////////////////


    //////////////////////////////////
    /// GUI Display Updaters
    // 
    /// updates the displayed task.
    /// 0 = task choice, 1 = move choice, 2 = bag choice, 3 = pokemon choice, 4 = item list, 5 = summary, 6 = moves
    private void updateCurrentTask(int newState)
    {
        if (currentTask == 0)
        {
            buttonBag.gameObject.SetActive(false);
            buttonFight.gameObject.SetActive(false);
            buttonPoke.gameObject.SetActive(false);
            buttonRun.gameObject.SetActive(false);
        }
        else if (currentTask == 1)
        {
            buttonMove[0].gameObject.SetActive(false);
            buttonMove[1].gameObject.SetActive(false);
            buttonMove[2].gameObject.SetActive(false);
            buttonMove[3].gameObject.SetActive(false);
            buttonMegaEvolution.gameObject.SetActive(false);
            buttonMoveReturn.gameObject.SetActive(false);
        }
        else if (currentTask == 2)
        {
            //Bag
            bagObject.SetActive(false);
            buttonItemCategory[0].gameObject.SetActive(false);
            buttonItemCategory[1].gameObject.SetActive(false);
            buttonItemCategory[2].gameObject.SetActive(false);
            buttonItemCategory[3].gameObject.SetActive(false);
            buttonItemLastUsed.gameObject.SetActive(false);
        }
        else if (currentTask == 3)
        {
            //Poke
            pokeObject.SetActive(false);
            pokemonPartyObject.SetActive(false);
            StopCoroutine(animatingPartyIcons);
        }
        else if (currentTask == 4)
        {
            //ItemList
            bagObject.SetActive(false);
            itemList.SetActive(false);
            updateItemListDisplay();
        }
        else if (currentTask == 5)
        {
            //Summary
            pokeObject.SetActive(false);
            buttonSwitch.gameObject.SetActive(false);
            buttonCheck.gameObject.SetActive(false);
            pokemonSelectedPokemon.SetActive(false);
            pokemonSummary.SetActive(false);
        }
        else if (currentTask == 6)
        {
            //Moves
            pokeObject.SetActive(false);
            buttonSwitch.gameObject.SetActive(false);
            buttonCheck.gameObject.SetActive(false);
            pokemonSelectedPokemon.SetActive(false);
            pokemonMoves.SetActive(false);
        }

        currentTask = newState;

        if (currentTask == 0)
        {
            buttonBag.gameObject.SetActive(true);
            buttonFight.gameObject.SetActive(true);
            buttonPoke.gameObject.SetActive(true);
            buttonRun.gameObject.SetActive(true);
        }
        else if (currentTask == 1)
        {
            buttonMove[0].gameObject.SetActive(true);
            buttonMove[1].gameObject.SetActive(true);
            buttonMove[2].gameObject.SetActive(true);
            buttonMove[3].gameObject.SetActive(true);
            buttonMegaEvolution.gameObject.SetActive(true);
            buttonMoveReturn.gameObject.SetActive(true);
        }
        else if (currentTask == 2)
        {
            //Bag
            bagObject.SetActive(true);
            buttonItemCategory[0].gameObject.SetActive(true);
            buttonItemCategory[1].gameObject.SetActive(true);
            buttonItemCategory[2].gameObject.SetActive(true);
            buttonItemCategory[3].gameObject.SetActive(true);
            buttonItemLastUsed.gameObject.SetActive(true);
            updateSelectedBagCategory(bagCategoryPosition);
        }
        else if (currentTask == 3)
        {
            //Poke
            pokeObject.SetActive(true);
            pokemonPartyObject.SetActive(true);
            updatePokemonSlotsDisplay();
            updateSelectedPokemonSlot(pokePartyPosition);
            animatingPartyIcons = StartCoroutine(animatePartyIcons());
        }
        else if (currentTask == 4)
        {
            //ItemList
            bagObject.SetActive(true);
            itemList.SetActive(true);
            if (bagCategoryPosition == 0)
            {
                itemListString = SaveData.currentSave.Bag.getBattleTypeArray(ItemData.BattleType.HPPPRESTORE);
                itemListCategoryText.text = "HP/PP Restore";
            }
            else if (bagCategoryPosition == 1)
            {
                itemListString = SaveData.currentSave.Bag.getBattleTypeArray(ItemData.BattleType.POKEBALLS);
                itemListCategoryText.text = "Poké Balls";
            }
            else if (bagCategoryPosition == 2)
            {
                itemListString = SaveData.currentSave.Bag.getBattleTypeArray(ItemData.BattleType.STATUSHEALER);
                itemListCategoryText.text = "Status Healers";
            }
            else if (bagCategoryPosition == 3)
            {
                itemListString = SaveData.currentSave.Bag.getBattleTypeArray(ItemData.BattleType.BATTLEITEMS);
                itemListCategoryText.text = "Battle Items";
            }
            itemListCategoryTextShadow.text = itemListCategoryText.text;
            itemListPagePosition = 0;

            itemListPageCount = Mathf.CeilToInt((float) itemListString.Length / 8f);
            updateItemListDisplay();
        }
        else if (currentTask == 5)
        {
            //Summary
            pokeObject.SetActive(true);
            buttonSwitch.gameObject.SetActive(true);
            buttonCheck.gameObject.SetActive(true);
            buttonCheckText.text = "Check Moves";
            buttonCheckTextShadow.text = buttonCheckText.text;
            pokemonSelectedPokemon.SetActive(true);
            pokemonSummary.SetActive(true);
            updatePokemonSummaryDisplay(SaveData.currentSave.PC.boxes[0][pokePartyPosition]);
        }
        else if (currentTask == 6)
        {
            //Moves
            pokeObject.SetActive(true);
            buttonSwitch.gameObject.SetActive(true);
            buttonCheck.gameObject.SetActive(true);
            buttonCheckText.text = "Check Summary";
            buttonCheckTextShadow.text = buttonCheckText.text;
            pokemonSelectedPokemon.SetActive(true);
            pokemonMoves.SetActive(true);
            updateMovesPosition(5);
        }
    }

    private void updatePokemonStatsDisplay(int position)
    {
        if (pokemon[position] != null)
        {
            statsName[position].text = pokemon[position].Name;
            statsNameShadow[position].text = statsName[position].text;
            statsNameShadow2[position].text = statsName[position].text;
            statsNameShadow3[position].text = statsName[position].text;
            statsNameShadow4[position].text = statsName[position].text;
            statsNameShadow5[position].text = statsName[position].text;
            statsNameShadow6[position].text = statsName[position].text;
            statsNameShadow7[position].text = statsName[position].text;
            statsNameShadow8[position].text = statsName[position].text;
            
            if (pokemon[position].Gender == false)
            {
                statsGender[position].text = "♀";
                statsGender[position].color = new Color(1, 0.2f, 0.2f, 1);
            }
            else if (pokemon[position].Gender == true)
            {
                statsGender[position].text = "♂";
                statsGender[position].color = new Color(0.2f, 0.4f, 1, 1);
            }
            else
            {
                statsGender[position].text = null;
            }
            statsGenderShadow[position].text = statsGender[position].text;
            statsLevel[position].text = "" + pokemon[position].Level;
            statsLevelShadow[position].text = statsLevel[position].text;
            statsLevelShadow2[position].text = statsLevel[position].text;
            statsLevelShadow3[position].text = statsLevel[position].text;
            statsLevelShadow4[position].text = statsLevel[position].text;
            statsLevelShadow5[position].text = statsLevel[position].text;
            statsLevelShadow6[position].text = statsLevel[position].text;
            statsLevelShadow7[position].text = statsLevel[position].text;
            statsLevelShadow8[position].text = statsLevel[position].text;
            
            statsHPBar[position].rectTransform.sizeDelta =
                new Vector2(Mathf.CeilToInt(pokemon[position].HP / pokemon[position].TotalHP * 51f), 4f);

            setHPBarColor(statsHPBar[position], 51f);

            if (pokemon[position].Status != Status.NONE)
            {
                statsStatus[position].sprite =
                    Resources.Load<Sprite>("PCSprites/status" + pokemon[position].Status.ToString());
            }
            else
            {
                statsStatus[position].sprite = Resources.Load<Sprite>("null");
            }
            //test
            /*if (position == 0)
            {
                pokemon0CurrentHP.text = "" + pokemon[0].HP;
                pokemon0CurrentHPShadow.text = pokemon0CurrentHP.text;
                pokemon0MaxHP.text = "" + pokemon[0].TotalHP;
                pokemon0MaxHPShadow.text = pokemon0MaxHP.text;
                float expCurrentLevel =
                    PokemonDatabase.getLevelExp(PokemonDatabase.getPokemon(pokemon[0].Species).getLevelingRate(),
                        pokemon[0].Level);
                float expNextlevel =
                    PokemonDatabase.getLevelExp(PokemonDatabase.getPokemon(pokemon[0].Species).getLevelingRate(),
                        pokemon[0].Level + 1);
                float expAlong = pokemon[0].Exp - expCurrentLevel;
                float expDistance = expAlong / (expNextlevel - expCurrentLevel);
                pokemon0ExpBar.rectTransform.sizeDelta = new Vector2(Mathf.Floor(expDistance * 77f), 2f);
            }
            else if (position == 1)
            {
                pokemon1CurrentHP.text = "" + pokemon[1].HP;
                pokemon1CurrentHPShadow.text = pokemon1CurrentHP.text;
                pokemon1MaxHP.text = "" + pokemon[1].TotalHP;
                pokemon1MaxHPShadow.text = pokemon1MaxHP.text;
                float expCurrentLevel =
                    PokemonDatabase.getLevelExp(PokemonDatabase.getPokemon(pokemon[1].Species).getLevelingRate(),
                        pokemon[1].Level);
                float expNextlevel =
                    PokemonDatabase.getLevelExp(PokemonDatabase.getPokemon(pokemon[1].Species).getLevelingRate(),
                        pokemon[1].Level + 1);
                float expAlong = pokemon[1].Exp - expCurrentLevel;
                float expDistance = expAlong / (expNextlevel - expCurrentLevel);
                pokemon1ExpBar.rectTransform.sizeDelta = new Vector2(Mathf.Floor(expDistance * 77f), 2f);
            }*/
        }
        else
        {
        }
    }

    private void updateMovesetDisplay(string[] moveset, int[] PP, int[] maxPP)
    {
        for (int i = 0; i < 4; i++)
        {
            if (moveset[i] != null)
            {
                PokemonData.Type type = MoveDatabase.getMove(moveset[i]).getType();

                switch (type)
                {
                    case PokemonData.Type.BUG:
                        buttonMoveCover[i].color = new Color(0.47f, 0.57f, 0.06f, 1);
                        buttonMoveType[i].sprite = Resources.Load<Sprite>("PCSprites/type" + type.ToString());
                        break;
                    case PokemonData.Type.DARK:
                        buttonMoveCover[i].color = new Color(0.32f, 0.28f, 0.24f, 1);
                        buttonMoveType[i].sprite = Resources.Load<Sprite>("PCSprites/type" + type.ToString());
                        break;
                    case PokemonData.Type.DRAGON:
                        buttonMoveCover[i].color = new Color(0.32f, 0.25f, 1f, 1);
                        buttonMoveType[i].sprite = Resources.Load<Sprite>("PCSprites/type" + type.ToString());
                        break;
                    case PokemonData.Type.ELECTRIC:
                        buttonMoveCover[i].color = new Color(0.64f, 0.52f, 0.04f, 1);
                        buttonMoveType[i].sprite = Resources.Load<Sprite>("PCSprites/type" + type.ToString());
                        break;
                    case PokemonData.Type.FAIRY:
                        buttonMoveCover[i].color = new Color(0.7f, 0.33f, 0.6f, 1);
                        buttonMoveType[i].sprite = Resources.Load<Sprite>("PCSprites/type" + type.ToString());
                        break;
                    case PokemonData.Type.FIGHTING:
                        buttonMoveCover[i].color = new Color(0.75f, 0.19f, 0.15f, 1);
                        buttonMoveType[i].sprite = Resources.Load<Sprite>("PCSprites/type" + type.ToString());
                        break;
                    case PokemonData.Type.FIRE:
                        buttonMoveCover[i].color = new Color(0.94f, 0.5f, 0.19f, 1);
                        buttonMoveType[i].sprite = Resources.Load<Sprite>("PCSprites/type" + type.ToString());
                        break;
                    case PokemonData.Type.FLYING:
                        buttonMoveCover[i].color = new Color(0.5f, 0.43f, 0.72f, 1);
                        buttonMoveType[i].sprite = Resources.Load<Sprite>("PCSprites/type" + type.ToString());
                        break;
                    case PokemonData.Type.GHOST:
                        buttonMoveCover[i].color = new Color(0.4f, 0.32f, 0.55f, 1);
                        buttonMoveType[i].sprite = Resources.Load<Sprite>("PCSprites/type" + type.ToString());
                        break;
                    case PokemonData.Type.GRASS:
                        buttonMoveCover[i].color = new Color(0.34f, 0.5f, 0.25f, 1);
                        buttonMoveType[i].sprite = Resources.Load<Sprite>("PCSprites/type" + type.ToString());
                        break;
                    case PokemonData.Type.GROUND:
                        buttonMoveCover[i].color = new Color(0.53f, 0.4f, 0.19f, 1);
                        buttonMoveType[i].sprite = Resources.Load<Sprite>("PCSprites/type" + type.ToString());
                        break;
                    case PokemonData.Type.ICE:
                        buttonMoveCover[i].color = new Color(0.4f, 0.6f, 0.6f, 1);
                        buttonMoveType[i].sprite = Resources.Load<Sprite>("PCSprites/type" + type.ToString());
                        break;
                    case PokemonData.Type.NORMAL:
                        buttonMoveCover[i].color = new Color(0.5f, 0.5f, 0.35f, 1);
                        buttonMoveType[i].sprite = Resources.Load<Sprite>("PCSprites/type" + type.ToString());
                        break;
                    case PokemonData.Type.POISON:
                        buttonMoveCover[i].color = new Color(0.63f, 0.25f, 0.63f, 1);
                        buttonMoveType[i].sprite = Resources.Load<Sprite>("PCSprites/type" + type.ToString());
                        break;
                    case PokemonData.Type.PSYCHIC:
                        buttonMoveCover[i].color = new Color(0.75f, 0.25f, 0.4f, 1);
                        buttonMoveType[i].sprite = Resources.Load<Sprite>("PCSprites/type" + type.ToString());
                        break;
                    case PokemonData.Type.ROCK:
                        buttonMoveCover[i].color = new Color(0.48f, 0.35f, 0.14f, 1);
                        buttonMoveType[i].sprite = Resources.Load<Sprite>("PCSprites/type" + type.ToString());
                        break;
                    case PokemonData.Type.STEEL:
                        buttonMoveCover[i].color = new Color(0.6f, 0.6f, 0.67f, 1);
                        buttonMoveType[i].sprite = Resources.Load<Sprite>("PCSprites/type" + type.ToString());
                        break;
                    case PokemonData.Type.WATER:
                        buttonMoveCover[i].color = new Color(0.25f, 0.42f, 0.75f, 1);
                        buttonMoveType[i].sprite = Resources.Load<Sprite>("PCSprites/type" + type.ToString());
                        break;
                }

                buttonMoveName[i].text = moveset[i];
                buttonMoveNameShadow[i].text = buttonMoveName[i].text;
                buttonMovePP[i].text = PP[i] + "/" + maxPP[i];
                buttonMovePPShadow[i].text = buttonMovePP[i].text;
                buttonMovePPText[i].text = "PP";
                buttonMovePPTextShadow[i].text = buttonMovePPText[i].text;
            }
            else
            {
                buttonMoveCover[i].color = new Color(0.5f, 0.5f, 0.5f, 1);
                buttonMoveType[i].sprite = Resources.Load<Sprite>("null");
                buttonMoveName[i].text = "";
                buttonMoveNameShadow[i].text = buttonMoveName[i].text;
                buttonMovePP[i].text = "";
                buttonMovePPShadow[i].text = buttonMovePP[i].text;
                buttonMovePPText[i].text = "";
                buttonMovePPTextShadow[i].text = "";
            }
        }
    }

    /// Updates the Item List to show the correct 8 items from the page
    private void updateItemListDisplay()
    {
        if (itemListPageCount < 1)
        {
            itemListPageCount = 1;
        }
        itemListPageNumber.text = (itemListPagePosition + 1) + "/" + itemListPageCount;
        itemListPageNumberShadow.text = itemListPageNumber.text;

        if (itemListPagePosition > 0)
        {
            itemListArrowPrev.SetActive(true);
        }
        else
        {
            itemListArrowPrev.SetActive(false);
        }

        if (itemListPagePosition + 1 < itemListPageCount)
        {
            itemListArrowNext.SetActive(true);
        }
        else
        {
            itemListArrowNext.SetActive(false);
        }

        string[] itemListPageString = new string[8];
        for (int i = 0; i < 8; i++)
        {
            if (i + (itemListPagePosition * 8) < itemListString.Length)
            {
                itemListPageString[i] = itemListString[i + (itemListPagePosition * 8)];
            }
        }

        for (int i = 0; i < 8; i++)
        {
            if (itemListPageString[i] != null)
            {
                buttonItemList[i].gameObject.SetActive(true);
                itemListIcon[i].sprite = Resources.Load<Sprite>("Items/" + itemListPageString[i]);
                itemListName[i].text = itemListPageString[i];
                itemListNameShadow[i].text = itemListName[i].text;
                itemListQuantity[i].text = "" + SaveData.currentSave.Bag.getQuantity(itemListPageString[i]);
                itemListQuantityShadow[i].text = itemListQuantity[i].text;
            }
            else
            {
                buttonItemList[i].gameObject.SetActive(false);
            }
        }
    }

    /// updates the pokemon slots to show the correct pokemon in the player's party
    private void updatePokemonSlotsDisplay()
    {
        for (int i = 0; i < 6; i++)
        {
            IPokemon selectedPokemon = SaveData.currentSave.PC.boxes[0][i];
            if (selectedPokemon == null)
            {
                buttonPokemonSlot[i].gameObject.SetActive(false);
            }
            else
            {
                buttonPokemonSlot[i].gameObject.SetActive(true);
                if (i == 0)
                {
                    if (i == pokePartyPosition)
                    {
                        if (selectedPokemon.Status != Status.FAINT)
                        {
                            buttonPokemonSlot[i].sprite = buttonPokemonRoundSelTex;
                        }
                        else
                        {
                            buttonPokemonSlot[i].sprite = buttonPokemonRoundFntSelTex;
                        }
                    }
                    else
                    {
                        if (selectedPokemon.Status != Status.FAINT)
                        {
                            buttonPokemonSlot[i].sprite = buttonPokemonRoundTex;
                        }
                        else
                        {
                            buttonPokemonSlot[i].sprite = buttonPokemonRoundFntTex;
                        }
                    }
                }
                else
                {
                    if (i == pokePartyPosition)
                    {
                        if (selectedPokemon.Status != Status.FAINT)
                        {
                            buttonPokemonSlot[i].sprite = buttonPokemonSelTex;
                        }
                        else
                        {
                            buttonPokemonSlot[i].sprite = buttonPokemonFntSelTex;
                        }
                    }
                    else
                    {
                        if (selectedPokemon.Status != Status.FAINT)
                        {
                            buttonPokemonSlot[i].sprite = buttonPokemonTex;
                        }
                        else
                        {
                            buttonPokemonSlot[i].sprite = buttonPokemonFntTex;
                        }
                    }
                }
                //pokemonIconAnim[i] = selectedPokemon.GetIconsSprite();
                pokemonSlotIcon[i].sprite = pokemonIconAnim[i][0];
                pokemonSlotName[i].text = selectedPokemon.Name;
                pokemonSlotNameShadow[i].text = pokemonSlotName[i].text;
                if (selectedPokemon.Gender == false)
                {
                    pokemonSlotGender[i].text = "♀";
                    pokemonSlotGender[i].color = new Color(1, 0.2f, 0.2f, 1);
                }
                else if (selectedPokemon.Gender == true)
                {
                    pokemonSlotGender[i].text = "♂";
                    pokemonSlotGender[i].color = new Color(0.2f, 0.4f, 1, 1);
                }
                else
                {
                    pokemonSlotGender[i].text = null;
                }
                pokemonSlotGenderShadow[i].text = pokemonSlotGender[i].text;
                pokemonSlotHPBar[i].rectTransform.sizeDelta =
                    new Vector2(
                        Mathf.FloorToInt(48f *
                                         ((float) selectedPokemon.HP / (float) selectedPokemon.TotalHP)), 4);

                setHPBarColor(pokemonSlotHPBar[i], 48f);

                pokemonSlotLevel[i].text = "" + selectedPokemon.Level;
                pokemonSlotLevelShadow[i].text = pokemonSlotLevel[i].text;
                pokemonSlotCurrentHP[i].text = "" + selectedPokemon.HP;
                pokemonSlotCurrentHPShadow[i].text = pokemonSlotCurrentHP[i].text;
                pokemonSlotMaxHP[i].text = "" + selectedPokemon.TotalHP;
                pokemonSlotMaxHPShadow[i].text = pokemonSlotMaxHP[i].text;
                if (selectedPokemon.Status != Status.NONE)
                {
                    pokemonSlotStatus[i].sprite =
                        Resources.Load<Sprite>("PCSprites/status" + selectedPokemon.Status.ToString());
                }
                else
                {
                    pokemonSlotStatus[i].sprite = Resources.Load<Sprite>("null");
                }
                pokemonSlotItem[i].enabled = selectedPokemon.Item != Items.NONE;
            }
        }
    }

    private void updatePokemonSummaryDisplay(IPokemon selectedPokemon)
    {
        //pokemonSelectedIcon.sprite = selectedPokemon.GetIconsSprite()[0];
        pokemonSelectedName.text = selectedPokemon.Name;
        pokemonSelectedNameShadow.text = pokemonSelectedName.text;
        if (selectedPokemon.Gender == false)
        {
            pokemonSelectedGender.text = "♀";
            pokemonSelectedGender.color = new Color(1, 0.2f, 0.2f, 1);
        }
        else if (selectedPokemon.Gender == true)
        {
            pokemonSelectedGender.text = "♂";
            pokemonSelectedGender.color = new Color(0.2f, 0.4f, 1, 1);
        }
        else
        {
            pokemonSelectedGender.text = null;
        }
        pokemonSelectedGenderShadow.text = pokemonSelectedGender.text;
        pokemonSelectedLevel.text = "" + selectedPokemon.Level;
        pokemonSelectedLevelShadow.text = pokemonSelectedLevel.text;
        if (selectedPokemon.Status != Status.NONE)
        {
            pokemonSelectedStatus.sprite =
                Resources.Load<Sprite>("PCSprites/status" + selectedPokemon.Status.ToString());
        }
        else
        {
            pokemonSelectedStatus.sprite = Resources.Load<Sprite>("null");
        }
        pokemonSelectedType1.sprite = Resources.Load<Sprite>("null");
        pokemonSelectedType2.sprite = Resources.Load<Sprite>("null");
        PokemonUnity.Types type1 = selectedPokemon.Type1;
        PokemonUnity.Types type2 = selectedPokemon.Type2;
        if (type1 != PokemonUnity.Types.NONE)
        {
            pokemonSelectedType1.sprite = Resources.Load<Sprite>("PCSprites/type" + type1.ToString());
        }
        if (type2 != PokemonUnity.Types.NONE)
        {
            pokemonSelectedType2.sprite = Resources.Load<Sprite>("PCSprites/type" + type2.ToString());
        }

        //Summary
        float expCurrentLevel = 0;
        //    PokemonDatabase.getLevelExp(PokemonDatabase.getPokemon(selectedPokemon.Species).getLevelingRate(),
        //        selectedPokemon.Level);
        float expNextlevel = 0;
        //    PokemonDatabase.getLevelExp(PokemonDatabase.getPokemon(selectedPokemon.Species).getLevelingRate(),
        //        selectedPokemon.Level + 1);
        float expAlong = selectedPokemon.Exp - expCurrentLevel;
        float expDistance = expAlong / (expNextlevel - expCurrentLevel);
        pokemonSummaryNextLevelEXP.text = "" + (expNextlevel - selectedPokemon.Exp);
        pokemonSummaryNextLevelEXPShadow.text = pokemonSummaryNextLevelEXP.text;
        pokemonSummaryEXPBar.rectTransform.sizeDelta = new Vector2(Mathf.Floor(expDistance * 64), 3f);
        pokemonSummaryItemIcon.sprite = Resources.Load<Sprite>("null");
        pokemonSummaryItemName.text = "No held item.";
        if (selectedPokemon.Item != Items.NONE)
        {
            pokemonSummaryItemIcon.sprite = Resources.Load<Sprite>("Items/" + selectedPokemon.Item);
            pokemonSummaryItemName.text = selectedPokemon.Item.ToString(TextScripts.Name);
        }

        pokemonSummaryItemNameShadow.text = pokemonSummaryItemName.text;
        //Stats
        float currentHP = selectedPokemon.HP;
        float maxHP = selectedPokemon.TotalHP;
        pokemonSummaryHP.text = currentHP + "/" + maxHP;
        pokemonSummaryHPShadow.text = pokemonSummaryHP.text;
        pokemonSummaryHPBar.rectTransform.sizeDelta = new Vector2(Mathf.Floor((1 - (maxHP - currentHP) / maxHP) * 48f),
            4f);

        setHPBarColor(pokemonSummaryHPBar, 48f);

        float[] natureMod = new float[]
        {
            Kernal.NatureData[selectedPokemon.Nature].ATK,
            Kernal.NatureData[selectedPokemon.Nature].DEF,
            Kernal.NatureData[selectedPokemon.Nature].SPA,
            Kernal.NatureData[selectedPokemon.Nature].SPD,
            Kernal.NatureData[selectedPokemon.Nature].SPE
        };
        pokemonSummaryStats.text =
            selectedPokemon.ATK + "\n" +
            selectedPokemon.DEF + "\n" +
            selectedPokemon.SPA + "\n" +
            selectedPokemon.SPD + "\n" +
            selectedPokemon.SPE;
        pokemonSummaryStatsShadow.text = pokemonSummaryStats.text;

        string[] statsLines = new string[] {"Attack", "Defence", "Sp. Atk", "Sp. Def", "Speed"};
        pokemonSummaryStatsTextShadow.text = "";
        for (int i = 0; i < 5; i++)
        {
            if (natureMod[i] > 1)
            {
                pokemonSummaryStatsTextShadow.text += "<color=#A01010FF>" + statsLines[i] + "</color>\n";
            }
            else if (natureMod[i] < 1)
            {
                pokemonSummaryStatsTextShadow.text += "<color=#0030A2FF>" + statsLines[i] + "</color>\n";
            }
            else
            {
                pokemonSummaryStatsTextShadow.text += statsLines[i] + "\n";
            }
        }

        pokemonSummaryAbilityName.text = selectedPokemon.Ability.ToString(TextScripts.Name);
        pokemonSummaryAbilityNameShadow.text = pokemonSummaryAbilityName.text;
        //abilities not yet implemented
        pokemonSummaryAbilityDescription.text = "";
        pokemonSummaryAbilityDescriptionShadow.text = pokemonSummaryAbilityDescription.text;

        //Moves
        IMove[] moveset = selectedPokemon.moves;
        //int[] maxPP = selectedPokemon.MaxPP;
        //int[] PP = selectedPokemon.PP;
        for (int i = 0; i < 4; i++)
        {
            if (moveset[i] != null)
            {
                pokemonMovesName[i].text = moveset[i].id.ToString(TextScripts.Name);
                pokemonMovesNameShadow[i].text = pokemonMovesName[i].text;
                pokemonMovesType[i].sprite =
                    Resources.Load<Sprite>("PCSprites/type" + moveset[i].Type.ToString());
                pokemonMovesPPText[i].text = "PP";
                pokemonMovesPPTextShadow[i].text = pokemonMovesPPText[i].text;
                pokemonMovesPP[i].text = selectedPokemon.moves[i].PP + "/" + selectedPokemon.moves[i].TotalPP;
                pokemonMovesPPShadow[i].text = pokemonMovesPP[i].text;
            }
            else
            {
                pokemonMovesName[i].text = null;
                pokemonMovesNameShadow[i].text = pokemonMovesName[i].text;
                pokemonMovesType[i].sprite = Resources.Load<Sprite>("null");
                pokemonMovesPPText[i].text = null;
                pokemonMovesPPTextShadow[i].text = pokemonMovesPPText[i].text;
                pokemonMovesPP[i].text = null;
                pokemonMovesPPShadow[i].text = pokemonMovesPP[i].text;
            }
        }
    }

    private IEnumerator displayMainTask()
    {
        float deltaTime = 0.08f;

        Image[] tmp =
        {
            buttonFight,
            buttonPoke,
            buttonBag,
            buttonRun
        };

        for (int i = 0; i <= 3; ++i)
        {
            LeanTween.moveX(tmp[i].rectTransform, 275, 0);
        }
        
        for (int i = 0; i <= 3; ++i)
        {
            if (i + 1 == taskPosition)
            {
                LeanTween.moveX(tmp[i].rectTransform, 171, deltaTime);
            }
            else
            {
                LeanTween.moveX(tmp[i].rectTransform, 193, deltaTime);
            }
            yield return new WaitForSeconds(deltaTime);
        }

        yield return null;
    }

    private void updateSelectedTask(int newPosition)
    {
        taskPosition = newPosition;

        buttonBag.sprite = (taskPosition == 0 || taskPosition == 3) ? buttonBagSelTex : buttonBagTex;
        buttonFight.sprite = (taskPosition == 1) ? buttonFightSelTex : buttonFightTex;
        buttonPoke.sprite = (taskPosition == 2 || taskPosition == 5) ? buttonPokeSelTex : buttonPokeTex;
        buttonRun.sprite = (taskPosition == 4) ? buttonRunSelTex : buttonRunTex;

        Image[] tmp =
        {
            buttonFight,
            buttonPoke,
            buttonBag,
            buttonRun
        };

        for (int i = 0; i <= 3; ++i)
        {
            if (i + 1 == taskPosition)
            {
                LeanTween.moveX(tmp[i].rectTransform, 171, 0.15f);
            }
            else
            {
                LeanTween.moveX(tmp[i].rectTransform, 193, 0.15f);
            }
        }
    }

    private void updateSelectedMove(int newPosition)
    {
        movePosition = newPosition;

        if (movePosition == 0)
        {
            buttonMegaEvolution.sprite = buttonMegaActiveSelTex;
        }
        else
        {
            buttonMegaEvolution.sprite = (canMegaEvolve) ? buttonMegaActiveTex : buttonMegaTex;
        }

        buttonMove[0].sprite = (movePosition == 1) ? buttonMoveBackgroundSelTex : buttonMoveBackgroundTex;
        buttonMove[1].sprite = (movePosition == 2) ? buttonMoveBackgroundSelTex : buttonMoveBackgroundTex;
        buttonMoveReturn.sprite = (movePosition == 3) ? buttonReturnSelTex : buttonReturnTex;
        buttonMove[2].sprite = (movePosition == 4) ? buttonMoveBackgroundSelTex : buttonMoveBackgroundTex;
        buttonMove[3].sprite = (movePosition == 5) ? buttonMoveBackgroundSelTex : buttonMoveBackgroundTex;
        
        buttonMove[0].color = (movePosition == 1) ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0.75f);
        buttonMove[1].color = (movePosition == 2) ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0.75f);
        buttonMoveReturn.color = (movePosition == 3) ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0.75f);
        buttonMove[2].color = (movePosition == 4) ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0.75f);
        buttonMove[3].color = (movePosition == 5) ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0.75f);
    }

    private void updateSelectedBagCategory(int newPosition)
    {
        bagCategoryPosition = newPosition;

        for (int i = 0; i < 6; i++)
        {
            if (i != bagCategoryPosition)
            {
                //deselect
                if (i == 4)
                {
                    buttonBackBag.sprite = buttonBackBagTex;
                }
                else if (i < 4)
                {
                    buttonItemCategory[i].sprite = buttonBagItemCategoryTex;
                }
                else
                {
                    buttonItemLastUsed.sprite = buttonBlueTex;
                }
            }
            else
            {
                //select
                if (i == 4)
                {
                    buttonBackBag.sprite = buttonBackBagSelTex;
                }
                else if (i < 4)
                {
                    buttonItemCategory[i].sprite = buttonBagItemCategorySelTex;
                }
                else
                {
                    buttonItemLastUsed.sprite = buttonBlueSelTex;
                }
            }
        }
    }

    private int updateSelectedItemListSlot(int currentPosition, int modifier)
    {
        int newPosition = currentPosition + modifier;

        //adjust for empty slots
        if (newPosition < 8)
        {
            if (!buttonItemList[newPosition].gameObject.activeSelf)
            {
                bool spaceFound = false;
                int checkPosition = currentPosition;
                //keep going back by modifier until avaiable position found
                while (!spaceFound)
                {
                    checkPosition += modifier;
                    if (checkPosition < 0)
                    {
                        if (buttonItemList[0].gameObject.activeSelf)
                        {
                            newPosition = 0; //move to first button
                        }
                        else
                        {
                            newPosition = 8; //move to back button
                        }
                        spaceFound = true;
                    }
                    else if (checkPosition > 7)
                    {
                        newPosition = 8; //set position to Back button
                        spaceFound = true;
                    }
                    else if (buttonItemList[checkPosition].gameObject.activeSelf)
                    {
                        newPosition = checkPosition; //adjust the position
                        spaceFound = true;
                    }
                    if (modifier == 0)
                    {
                        modifier = 1; //prevent infinite loops in case the modifier is set to never increment
                    }
                }
            }
        }
        else
        {
            newPosition = 8;
        }

        for (int i = 0; i < 8; i++)
        {
            if (i == newPosition)
            {
                buttonItemList[i].sprite = buttonBagItemListSelTex;
                buttonItemList[i].rectTransform.SetSiblingIndex(7);
            }
            else
            {
                buttonItemList[i].sprite = buttonBagItemListTex;
            }
        }
        if (newPosition == 8)
        {
            buttonBackBag.sprite = buttonBackBagSelTex;
        }
        else
        {
            buttonBackBag.sprite = buttonBackBagTex;
        }

        if (newPosition + (itemListPagePosition * 8) < itemListString.Length)
        {
            itemListDescription.text =
                ItemDatabase.getItem(itemListString[newPosition + (itemListPagePosition * 8)]).getDescription();
        }
        else
        {
            itemListDescription.text = "";
        }
        itemListDescriptionShadow.text = itemListDescription.text;

        return newPosition;
    }

    private void updateSelectedPokemonSlot(int newPosition)
    {
        updateSelectedPokemonSlot(newPosition, true);
    }

    private void updateSelectedPokemonSlot(int newPosition, bool backSelectable)
    {
        int maxPosition = 5;
        if (backSelectable)
        {
            maxPosition = 6;
        }
        if (newPosition < 6)
        {
            if (SaveData.currentSave.PC.boxes[0][newPosition] == null)
            {
                int checkPosition = pokePartyPosition;
                bool spaceFound = false;
                if (newPosition < pokePartyPosition)
                {
                    //keep going back 1 until avaiable position found
                    while (!spaceFound)
                    {
                        checkPosition -= 1;
                        if (checkPosition < 0)
                        {
                            newPosition = pokePartyPosition; //don't move the position
                            spaceFound = true;
                        }
                        else if (SaveData.currentSave.PC.boxes[0][checkPosition] != null)
                        {
                            newPosition = checkPosition; //adjust the position
                            spaceFound = true;
                        }
                    }
                }
                else
                {
                    //keep going forward 1
                    while (!spaceFound)
                    {
                        checkPosition += 1;
                        if (checkPosition > 5)
                        {
                            if (backSelectable)
                            {
                                newPosition = 6;
                            } //set position to Back button
                            else
                            {
                                newPosition = 5;
                                while (SaveData.currentSave.PC.boxes[0][newPosition] == null && newPosition > 0)
                                {
                                    newPosition -= 1;
                                }
                            }
                            spaceFound = true;
                        }
                        else if (SaveData.currentSave.PC.boxes[0][checkPosition] != null)
                        {
                            newPosition = checkPosition; //adjust the position
                            spaceFound = true;
                        }
                    }
                }
            }
        }
        else
        {
            newPosition = maxPosition;
            if (newPosition < 6)
            {
                if (SaveData.currentSave.PC.boxes[0][newPosition] == null)
                {
                    int checkPosition = pokePartyPosition;
                    bool spaceFound = false;
                    if (newPosition < pokePartyPosition)
                    {
                        //keep going back 1 until avaiable position found
                        while (!spaceFound)
                        {
                            checkPosition -= 1;
                            if (checkPosition < 0)
                            {
                                newPosition = pokePartyPosition; //don't move the position
                                spaceFound = true;
                            }
                            else if (SaveData.currentSave.PC.boxes[0][checkPosition] != null)
                            {
                                newPosition = checkPosition; //adjust the position
                                spaceFound = true;
                            }
                        }
                    }
                }
            }
        }

        pokePartyPosition = newPosition;

        for (int i = 0; i < 7; i++)
        {
            if (i != pokePartyPosition)
            {
                //unhighlight
                if (i == 0)
                {
                    if (SaveData.currentSave.PC.boxes[0][i] != null)
                    {
                        buttonPokemonSlot[i].sprite = (SaveData.currentSave.PC.boxes[0][i].Status !=
                                                       Status.FAINT)
                            ? buttonPokemonRoundTex
                            : buttonPokemonRoundFntTex;
                    }
                }
                else if (i < 6)
                {
                    if (SaveData.currentSave.PC.boxes[0][i] != null)
                    {
                        buttonPokemonSlot[i].sprite = (SaveData.currentSave.PC.boxes[0][i].Status !=
                                                       Status.FAINT)
                            ? buttonPokemonTex
                            : buttonPokemonFntTex;
                    }
                }
                else
                {
                    buttonBackPoke.sprite = buttonBackPokeTex;
                }
            }
            else
            {
                //highlight
                if (i == 0)
                {
                    if (SaveData.currentSave.PC.boxes[0][i] != null)
                    {
                        buttonPokemonSlot[i].sprite = (SaveData.currentSave.PC.boxes[0][i].Status !=
                                                       Status.FAINT)
                            ? buttonPokemonRoundSelTex
                            : buttonPokemonRoundFntSelTex;
                    }
                }
                else if (i < 6)
                {
                    if (SaveData.currentSave.PC.boxes[0][i] != null)
                    {
                        buttonPokemonSlot[i].sprite = (SaveData.currentSave.PC.boxes[0][i].Status !=
                                                       Status.FAINT)
                            ? buttonPokemonSelTex
                            : buttonPokemonFntSelTex;
                    }
                }
                else
                {
                    buttonBackPoke.sprite = buttonBackPokeSelTex;
                }
            }
        }
    }

    private int updateSummaryPosition(int newPosition)
    {
        buttonSwitch.sprite = (newPosition == 0) ? buttonBlueSelTex : buttonBlueTex;
        buttonCheck.sprite = (newPosition == 1) ? buttonBlueSelTex : buttonBlueTex;
        buttonBackPoke.sprite = (newPosition == 2) ? buttonBackPokeSelTex : buttonBackPokeTex;
        return newPosition;
    }

    private int updateMovesPosition(int newPosition)
    {
        Vector3[] positions = new Vector3[]
        {
            new Vector3(-36, 51, 0), new Vector3(51, 51, 0),
            new Vector3(-36, 19, 0), new Vector3(51, 19, 0)
        };

        buttonSwitch.sprite = buttonBlueTex;
        buttonCheck.sprite = buttonBlueTex;
        buttonBackPoke.sprite = buttonBackPokeTex;

        if (newPosition < 4)
        {
            string[] moveset = SaveData.currentSave.PC.boxes[0][pokePartyPosition].getMoveset();
            if (string.IsNullOrEmpty(moveset[newPosition]))
            {
                pokemonMovesSelectedCategory.sprite = Resources.Load<Sprite>("null");
                pokemonMovesSelectedPower.text = null;
                pokemonMovesSelectedPowerShadow.text = pokemonMovesSelectedPower.text;
                pokemonMovesSelectedAccuracy.text = null;
                pokemonMovesSelectedAccuracyShadow.text = pokemonMovesSelectedAccuracy.text;
                pokemonMovesSelectedDescription.text = null;
                pokemonMovesSelectedDescriptionShadow.text = pokemonMovesSelectedDescription.text;
            }
            else
            {
                MoveData selectedMove = MoveDatabase.getMove(moveset[newPosition]);
                pokemonMovesSelectedCategory.sprite =
                    Resources.Load<Sprite>("PCSprites/category" + selectedMove.getCategory().ToString());
                pokemonMovesSelectedPower.text = "" + selectedMove.getPower();
                if (pokemonMovesSelectedPower.text == "0")
                {
                    pokemonMovesSelectedPower.text = "-";
                }
                pokemonMovesSelectedPowerShadow.text = pokemonMovesSelectedPower.text;
                pokemonMovesSelectedAccuracy.text = "" + Mathf.Round(selectedMove.getAccuracy() * 100f);
                if (pokemonMovesSelectedAccuracy.text == "0")
                {
                    pokemonMovesSelectedAccuracy.text = "-";
                }
                pokemonMovesSelectedAccuracyShadow.text = pokemonMovesSelectedAccuracy.text;
                pokemonMovesSelectedDescription.text = selectedMove.getDescription();
                pokemonMovesSelectedDescriptionShadow.text = pokemonMovesSelectedDescription.text;

                pokemonMovesSelectedMove.rectTransform.localPosition = positions[newPosition];
                pokemonMovesSelectedMove.enabled = true;
                if (pokemonMovesSelector.enabled)
                {
                    StartCoroutine(moveMoveSelector(positions[newPosition]));
                }
                else
                {
                    pokemonMovesSelector.enabled = true;
                    pokemonMovesSelector.rectTransform.localPosition = positions[newPosition];
                }
            }
        }
        else
        {
            pokemonMovesSelectedCategory.sprite = Resources.Load<Sprite>("null");
            pokemonMovesSelectedPower.text = null;
            pokemonMovesSelectedPowerShadow.text = pokemonMovesSelectedPower.text;
            pokemonMovesSelectedAccuracy.text = null;
            pokemonMovesSelectedAccuracyShadow.text = pokemonMovesSelectedAccuracy.text;
            pokemonMovesSelectedDescription.text = null;
            pokemonMovesSelectedDescriptionShadow.text = pokemonMovesSelectedDescription.text;

            pokemonMovesSelectedMove.enabled = false;
            pokemonMovesSelector.enabled = false;
            if (newPosition == 4)
            {
                buttonSwitch.sprite = buttonBlueSelTex;
            }
            else if (newPosition == 5)
            {
                buttonCheck.sprite = buttonBlueSelTex;
            }
            else if (newPosition == 6)
            {
                buttonBackPoke.sprite = buttonBackPokeSelTex;
            }
        }
        return newPosition;
    }

    private IEnumerator moveMoveSelector(Vector3 destinationPosition)
    {
        Vector3 startPosition = pokemonMovesSelector.rectTransform.localPosition;

        Vector3 distance = destinationPosition - startPosition;

        float increment = 0f;
        float speed = 0.2f;
        while (increment < 1)
        {
            increment += (1f / speed) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }
            pokemonMovesSelector.rectTransform.localPosition = startPosition + (distance * increment);
            yield return null;
        }
    }


    private void setHPBarColor(Image bar, float maxSize)
    {
        if (bar.rectTransform.sizeDelta.x < maxSize / 4f)
        {
            bar.color = new Color(0.625f, 0.125f, 0, 1);
        }
        else if (bar.rectTransform.sizeDelta.x < maxSize / 2f)
        {
            bar.color = new Color(0.687f, 0.562f, 0, 1);
        }
        else
        {
            bar.color = new Color(0.125f, 0.625f, 0, 1);
        }
    }

    //////////////////////////////////
    /// GETTERS
    /// 
    public IPokemon getPokemon(int index)
    {
        return pokemon[index];
    }

    public DialogBoxHandlerNew getDialog()
    {
        return Dialog;
    }

    //////////////////////////////////
    /// BATTLE DATA MANAGEMENT
    //
    /// Calculates the base damage of an attack (before modifiers are applied).
    private float calculateDamage(int attackerPosition, int targetPosition, MoveData move)
    {
        float baseDamage = 0;
        if (move.getCategory() == MoveData.Category.PHYSICAL)
        {
            baseDamage = ((2f * (float) pokemon[attackerPosition].Level + 10f) / 250f) *
                         ((float) pokemonStats[0][attackerPosition] / (float) pokemonStats[1][targetPosition]) *
                         (float) move.getPower() + 2f;
        }
        else if (move.getCategory() == MoveData.Category.SPECIAL)
        {
            baseDamage = ((2f * (float) pokemon[attackerPosition].Level + 10f) / 250f) *
                         ((float) pokemonStats[2][attackerPosition] / (float) pokemonStats[3][targetPosition]) *
                         (float) move.getPower() + 2f;
        }

        baseDamage *= Random.Range(0.85f, 1f);
        return baseDamage;
    }

    /// Uses the attacker's total critical ratio to randomly determine whether a Critical Hit should happen or not
    private bool calculateCritical(int attackerPosition, int targetPosition, MoveData move)
    {
        int attackerCriticalRatio = 0;
        if (focusEnergy[attackerPosition])
        {
            attackerCriticalRatio += 1;
        }

        if (move.hasMoveEffect(MoveData.Effect.Critical))
        {
            attackerCriticalRatio += 1;
        }

        bool applyCritical = false;
        if (move.getCategory() != MoveData.Category.STATUS)
        {
            if (attackerCriticalRatio == 0)
            {
                if (Random.value <= 0.0625)
                {
                    applyCritical = true;
                }
            }
            else if (attackerCriticalRatio == 1)
            {
                if (Random.value <= 0.125)
                {
                    applyCritical = true;
                }
            }
            else if (attackerCriticalRatio == 2)
            {
                if (Random.value <= 0.5)
                {
                    applyCritical = true;
                }
            }
            else if (attackerCriticalRatio > 2)
            {
                applyCritical = true;
            }
        }

        return applyCritical;
    }


    /// TEMPORARILY USED FOR EVERYTHING NOT COVERED BY AN EXISTING METHOD
    //private float calculateModifiedDamage(int attackerPosition, int targetPosition, MoveData move, float baseDamage,
    //    bool applyCritical)
    //{
    //    float modifiedDamage = baseDamage;
    //
    //    //apply STAB
    //    if (pokemonType1[attackerPosition] == move.getType() ||
    //        pokemonType2[attackerPosition] == move.getType() ||
    //        pokemonType3[attackerPosition] == move.getType())
    //    {
    //        modifiedDamage *= 1.5f;
    //    }
    //
    //    //apply Offence/Defence boosts 
    //    if (move.getCategory() == MoveData.Category.PHYSICAL)
    //    {
    //        modifiedDamage *= calculateStatModifier(pokemonStatsMod[0][attackerPosition]);
    //        if (!applyCritical)
    //        {
    //            //exclude defensive buffs in a critical hit
    //            modifiedDamage /= calculateStatModifier(pokemonStatsMod[1][targetPosition]);
    //        }
    //    }
    //    else if (move.getCategory() == MoveData.Category.SPECIAL)
    //    {
    //        modifiedDamage *= calculateStatModifier(pokemonStatsMod[2][attackerPosition]);
    //        if (!applyCritical)
    //        {
    //            //exclude defensive buffs in a critical hit
    //            modifiedDamage /= calculateStatModifier(pokemonStatsMod[3][targetPosition]);
    //        }
    //    }
    //
    //
    //    //not yet implemented
    //    //apply held item
    //    //apply ability
    //    //apply field advantages
    //    //reflect/lightScreen
    //    if (!applyCritical)
    //    {
    //        if (move.getCategory() == MoveData.Category.PHYSICAL)
    //        {
    //            //if (reflectTurns[Mathf.FloorToInt((float) targetPosition / 3f)] > 0)
    //            //{
    //            //    modifiedDamage *= 0.5f;
    //            //}
    //        }
    //        else if (move.getCategory() == MoveData.Category.SPECIAL)
    //        {
    //            //if (lightScreenTurns[Mathf.FloorToInt((float) targetPosition / 3f)] > 0)
    //            //{
    //            //    modifiedDamage *= 0.5f;
    //            //}
    //        }
    //    }
    //
    //    //apply multi-target debuff 
    //
    //    return Mathf.Floor(modifiedDamage);
    //}

    private float calculateStatModifier(int modifier)
    {
        if (modifier > 0)
        {
            return ((2f + (float) modifier) / 2f);
        }
        else if (modifier < 0)
        {
            return (2f / (2f + Mathf.Abs((float) modifier)));
        }
        return 1f;
    }

    private float calculateAccuracyModifier(int modifier)
    {
        if (modifier > 0)
        {
            return ((3f + modifier) / 3f);
        }
        else if (modifier < 0)
        {
            return (3f / (3f + modifier));
        }
        return 1f;
    }

    /// returns the modifier of a type vs. type. returns as 0f-2f
    private float getSuperEffectiveModifier(PokemonData.Type attackingType, PokemonData.Type targetType)
    {
        if (attackingType == PokemonData.Type.BUG)
        {
            if (targetType == PokemonData.Type.DARK || targetType == PokemonData.Type.GRASS ||
                targetType == PokemonData.Type.PSYCHIC)
            {
                return 2f;
            }
            else if (targetType == PokemonData.Type.FAIRY || targetType == PokemonData.Type.FIGHTING ||
                     targetType == PokemonData.Type.FIRE || targetType == PokemonData.Type.FLYING ||
                     targetType == PokemonData.Type.GHOST || targetType == PokemonData.Type.POISON ||
                     targetType == PokemonData.Type.STEEL)
            {
                return 0.5f;
            }
        }
        else if (attackingType == PokemonData.Type.DARK)
        {
            if (targetType == PokemonData.Type.GHOST || targetType == PokemonData.Type.PSYCHIC)
            {
                return 2f;
            }
            else if (targetType == PokemonData.Type.DARK || targetType == PokemonData.Type.FAIRY ||
                     targetType == PokemonData.Type.FIGHTING)
            {
                return 0.5f;
            }
        }
        else if (attackingType == PokemonData.Type.DRAGON)
        {
            if (targetType == PokemonData.Type.DRAGON)
            {
                return 2f;
            }
            else if (targetType == PokemonData.Type.STEEL)
            {
                return 0.5f;
            }
            else if (targetType == PokemonData.Type.FAIRY)
            {
                return 0f;
            }
        }
        else if (attackingType == PokemonData.Type.ELECTRIC)
        {
            if (targetType == PokemonData.Type.FLYING || targetType == PokemonData.Type.WATER)
            {
                return 2f;
            }
            else if (targetType == PokemonData.Type.DRAGON || targetType == PokemonData.Type.ELECTRIC ||
                     targetType == PokemonData.Type.GRASS)
            {
                return 0.5f;
            }
            else if (targetType == PokemonData.Type.GROUND)
            {
                return 0f;
            }
        }
        else if (attackingType == PokemonData.Type.FAIRY)
        {
            if (targetType == PokemonData.Type.DARK || targetType == PokemonData.Type.DRAGON ||
                targetType == PokemonData.Type.FIGHTING)
            {
                return 2f;
            }
            else if (targetType == PokemonData.Type.FIRE || targetType == PokemonData.Type.POISON ||
                     targetType == PokemonData.Type.STEEL)
            {
                return 0.5f;
            }
        }
        else if (attackingType == PokemonData.Type.FIGHTING)
        {
            if (targetType == PokemonData.Type.DARK || targetType == PokemonData.Type.ICE ||
                targetType == PokemonData.Type.NORMAL || targetType == PokemonData.Type.ROCK ||
                targetType == PokemonData.Type.STEEL)
            {
                return 2f;
            }
            else if (targetType == PokemonData.Type.BUG || targetType == PokemonData.Type.FAIRY ||
                     targetType == PokemonData.Type.FLYING || targetType == PokemonData.Type.POISON ||
                     targetType == PokemonData.Type.PSYCHIC)
            {
                return 0.5f;
            }
            else if (targetType == PokemonData.Type.GHOST)
            {
                return 0f;
            }
        }
        else if (attackingType == PokemonData.Type.FIRE)
        {
            if (targetType == PokemonData.Type.BUG || targetType == PokemonData.Type.GRASS ||
                targetType == PokemonData.Type.ICE || targetType == PokemonData.Type.STEEL)
            {
                return 2f;
            }
            else if (targetType == PokemonData.Type.DRAGON || targetType == PokemonData.Type.FIRE ||
                     targetType == PokemonData.Type.ROCK || targetType == PokemonData.Type.WATER)
            {
                return 0.5f;
            }
        }
        else if (attackingType == PokemonData.Type.FLYING)
        {
            if (targetType == PokemonData.Type.BUG || targetType == PokemonData.Type.FIGHTING ||
                targetType == PokemonData.Type.GRASS)
            {
                return 2f;
            }
            else if (targetType == PokemonData.Type.ELECTRIC || targetType == PokemonData.Type.ROCK ||
                     targetType == PokemonData.Type.STEEL)
            {
                return 0.5f;
            }
        }
        else if (attackingType == PokemonData.Type.GHOST)
        {
            if (targetType == PokemonData.Type.GHOST || targetType == PokemonData.Type.PSYCHIC)
            {
                return 2f;
            }
            else if (targetType == PokemonData.Type.DARK)
            {
                return 0.5f;
            }
            else if (targetType == PokemonData.Type.NORMAL)
            {
                return 0f;
            }
        }
        else if (attackingType == PokemonData.Type.GRASS)
        {
            if (targetType == PokemonData.Type.GROUND || targetType == PokemonData.Type.ROCK ||
                targetType == PokemonData.Type.WATER)
            {
                return 2f;
            }
            else if (targetType == PokemonData.Type.BUG || targetType == PokemonData.Type.DRAGON ||
                     targetType == PokemonData.Type.FIRE || targetType == PokemonData.Type.FLYING ||
                     targetType == PokemonData.Type.GRASS || targetType == PokemonData.Type.POISON ||
                     targetType == PokemonData.Type.STEEL)
            {
                return 0.5f;
            }
        }
        else if (attackingType == PokemonData.Type.GROUND)
        {
            if (targetType == PokemonData.Type.ELECTRIC || targetType == PokemonData.Type.FIRE ||
                targetType == PokemonData.Type.POISON || targetType == PokemonData.Type.ROCK ||
                targetType == PokemonData.Type.STEEL)
            {
                return 2f;
            }
            else if (targetType == PokemonData.Type.BUG || targetType == PokemonData.Type.GRASS)
            {
                return 0.5f;
            }
            else if (targetType == PokemonData.Type.FLYING)
            {
                return 0f;
            }
        }
        else if (attackingType == PokemonData.Type.ICE)
        {
            if (targetType == PokemonData.Type.DRAGON || targetType == PokemonData.Type.FLYING ||
                targetType == PokemonData.Type.GRASS || targetType == PokemonData.Type.GROUND)
            {
                return 2f;
            }
            else if (targetType == PokemonData.Type.FIRE || targetType == PokemonData.Type.ICE ||
                     targetType == PokemonData.Type.STEEL || targetType == PokemonData.Type.WATER)
            {
                return 0.5f;
            }
        }
        else if (attackingType == PokemonData.Type.NORMAL)
        {
            if (targetType == PokemonData.Type.ROCK || targetType == PokemonData.Type.STEEL)
            {
                return 0.5f;
            }
            else if (targetType == PokemonData.Type.GHOST)
            {
                return 0f;
            }
        }
        else if (attackingType == PokemonData.Type.POISON)
        {
            if (targetType == PokemonData.Type.FAIRY || targetType == PokemonData.Type.GRASS)
            {
                return 2f;
            }
            else if (targetType == PokemonData.Type.POISON || targetType == PokemonData.Type.GROUND ||
                     targetType == PokemonData.Type.ROCK || targetType == PokemonData.Type.GHOST)
            {
                return 0.5f;
            }
            else if (targetType == PokemonData.Type.STEEL)
            {
                return 0f;
            }
        }
        else if (attackingType == PokemonData.Type.PSYCHIC)
        {
            if (targetType == PokemonData.Type.FIGHTING || targetType == PokemonData.Type.POISON)
            {
                return 2f;
            }
            else if (targetType == PokemonData.Type.PSYCHIC || targetType == PokemonData.Type.STEEL)
            {
                return 0.5f;
            }
            else if (targetType == PokemonData.Type.DARK)
            {
                return 0f;
            }
        }
        else if (attackingType == PokemonData.Type.ROCK)
        {
            if (targetType == PokemonData.Type.BUG || targetType == PokemonData.Type.FIRE ||
                targetType == PokemonData.Type.FLYING || targetType == PokemonData.Type.ICE)
            {
                return 2f;
            }
            else if (targetType == PokemonData.Type.FIGHTING || targetType == PokemonData.Type.GROUND ||
                     targetType == PokemonData.Type.STEEL)
            {
                return 0.5f;
            }
        }
        else if (attackingType == PokemonData.Type.STEEL)
        {
            if (targetType == PokemonData.Type.FAIRY || targetType == PokemonData.Type.ICE ||
                targetType == PokemonData.Type.ROCK)
            {
                return 2f;
            }
            else if (targetType == PokemonData.Type.ELECTRIC || targetType == PokemonData.Type.FIRE ||
                     targetType == PokemonData.Type.STEEL || targetType == PokemonData.Type.WATER)
            {
                return 0.5f;
            }
        }
        else if (attackingType == PokemonData.Type.WATER)
        {
            if (targetType == PokemonData.Type.FIRE || targetType == PokemonData.Type.GROUND ||
                targetType == PokemonData.Type.ROCK)
            {
                return 2f;
            }
            else if (targetType == PokemonData.Type.DRAGON || targetType == PokemonData.Type.GRASS ||
                     targetType == PokemonData.Type.WATER)
            {
                return 0.5f;
            }
        }

        return 1f;
    }

    /// Returns the Pokemon index that should move first, that hasn't moved this turn.
    private int getHighestSpeedIndex()
    {
        int topSpeed = 0;
        int topPriority = -7;
        int topSpeedPosition = 0;

        //calculate the highest speed remaining in the list, the set that speed to -1
        for (int i = 0; i < 6; i++)
        {
            if (!pokemonHasMoved[i])
            {
                //calculate speed
                float calculatedPokemonSpeed = 0;
                float calculatedPokemonPriority = 0;
                calculatedPokemonSpeed = (float) pokemonStats[4][i] * calculateStatModifier(pokemonStatsMod[4][i]);
                if (pokemon[i] != null)
                {
                    if (pokemon[i].Status == Status.PARALYSIS)
                    {
                        calculatedPokemonSpeed /= 4f;
                    }
                }

                //only the player gets increased priority on fleeing
                if (command[i] == CommandType.Flee && i < 3)
                {
                    calculatedPokemonPriority = 6;
                }
                else if (command[i] == CommandType.Item)
                {
                    calculatedPokemonPriority = 6;
                }
                else if (command[i] == CommandType.Move)
                {
                    calculatedPokemonPriority = commandMove[i].getPriority();
                }
                else if (command[i] == CommandType.Switch)
                {
                    //6 priority for regular swapping out.
                    calculatedPokemonPriority = 6;
                }

                //if the top speed is greater than the old, AND the priority is no lower than       OR   the priority is greater than the old
                if ((calculatedPokemonSpeed >= topSpeed && calculatedPokemonPriority >= topPriority) ||
                    calculatedPokemonPriority > topPriority)
                {
                    if (calculatedPokemonSpeed == topSpeed && calculatedPokemonPriority == topPriority)
                    {
                        //if the speed/priority is exactly equal to the current highest, then to randomize the order
                        //in which speed is chosen (to break a speed tie), update the topSpeed position ONLY when a
                        //random float value (0f-1f) is greater than 0.5f.
                        if (Random.value > 0.5f)
                        {
                            topSpeedPosition = i;
                        }
                    }
                    else
                    {
                        topSpeed = Mathf.FloorToInt(calculatedPokemonSpeed);
                        topPriority = Mathf.FloorToInt(calculatedPokemonPriority);
                        topSpeedPosition = i;
                    }
                }
            }
        }
        return topSpeedPosition;
    }

    /// Switch Pokemon (not yet implemented fully)
    private bool switchPokemon(int switchPosition, IPokemon newPokemon)
    {
        return switchPokemon(switchPosition, newPokemon, false, false);
    }

    /// Switch Pokemon
    private bool switchPokemon(int switchPosition, IPokemon newPokemon, bool batonPass)
    {
        return switchPokemon(switchPosition, newPokemon, batonPass, false);
    }

    /// Switch Pokemon
    private bool switchPokemon(int switchPosition, IPokemon newPokemon, bool batonPass, bool forceSwitch)
    {
        if (newPokemon == null)
        {
            return false;
        }
        if (newPokemon.Status == Status.FAINT)
        {
            return false;
        }
        // Return false if any condition is preventing the pokemon from switching out
        if (!forceSwitch)
        {
            //no condition can stop a fainted pokemon from switching out
            if (pokemon[switchPosition] != null)
            {
                if (pokemon[switchPosition].Status != Status.FAINT)
                {
                }
            }
        }

        pokemon[switchPosition] = newPokemon;
        //pokemonMoveset[switchPosition] = newPokemon.getMoveset();


        //set PokemonData
        updatePokemonStats(switchPosition);
        //pokemonAbility[switchPosition] = newPokemon.Ability;
        //pokemonType1[switchPosition] = newPokemon.Type1;
        //pokemonType2[switchPosition] = newPokemon.Type2;
        pokemonType3[switchPosition] = PokemonData.Type.NONE;

        //reset Pokemon Effects
        confused[switchPosition] = false;
        infatuatedBy[switchPosition] = -1;
        flinched[switchPosition] = false;
        statusEffectTurns[switchPosition] = 0;
        lockedTurns[switchPosition] = 0;
        partTrappedTurns[switchPosition] = 0;
        trapped[switchPosition] = false;
        charging[switchPosition] = false;
        recharging[switchPosition] = false;
        protect[switchPosition] = false;
        //specific moves
        seededBy[switchPosition] = -1;
        destinyBond[switchPosition] = false;
        minimized[switchPosition] = false;
        defenseCurled[switchPosition] = false;
        if (!batonPass)
        {
            pokemonStatsMod[0][switchPosition] = 0;
            pokemonStatsMod[1][switchPosition] = 0;
            pokemonStatsMod[2][switchPosition] = 0;
            pokemonStatsMod[3][switchPosition] = 0;
            pokemonStatsMod[4][switchPosition] = 0;
            pokemonStatsMod[5][switchPosition] = 0;
            pokemonStatsMod[6][switchPosition] = 0;

            //Pokemon Effects
            focusEnergy[switchPosition] = false;
        }
        else
        {
        }


        return true;
    }

    //private IEnumerator addExp(int position, int exp)
    //{
    //    if (pokemon[position].Level < 100)
    //    {
    //        yield return
    //        StartCoroutine(drawTextAndWait(pokemon[position].Name + " gained " + exp + " Exp. Points!", 1.8f, 1));
    //        Dialog.UndrawDialogBox();
    //
    //        int expPool = exp;
    //        while (expPool > 0)
    //        {
    //            int expToNextLevel = 0; //pokemon[position].getExpNext() - pokemon[position].getExp();
    //
    //            //if enough exp left to level up
    //            if (expPool >= expToNextLevel)
    //            {
    //                pokemon[position].addExp(expToNextLevel);
    //                expPool -= expToNextLevel;
    //
    //                AudioSource fillSource = SfxHandler.Play(fillExpClip);
    //                yield return StartCoroutine(stretchBar(position == 0 ? pokemon0ExpBar : pokemon1ExpBar, 77f, 35f));
    //                SfxHandler.FadeSource(fillSource, 0.2f);
    //                SfxHandler.Play(expFullClip);
    //                yield return new WaitForSeconds(0.7f);
    //
    //                updatePokemonStats(position);
    //                updatePokemonStatsDisplay(position);
    //
    //                BgmHandler.main.PlayMFX(Resources.Load<AudioClip>("Audio/mfx/GetAverage"));
    //                yield return
    //                    StartCoroutine(
    //                        drawTextAndWait(
    //                            pokemon[position].Name + " grew to Level " + pokemon[position].Level + "!", 1.8f,
    //                            0.5f));
    //
    //                string newMove = pokemon[position].MoveLearnedAtLevel(pokemon[position].Level);
    //                if (!string.IsNullOrEmpty(newMove) && !pokemon[position].HasMove(newMove))
    //                {
    //                    yield return StartCoroutine(LearnMove(pokemon[position], newMove));
    //                }
    //
    //                Dialog.UndrawDialogBox();
    //                yield return new WaitForSeconds(0.5f);
    //            }
    //            else
    //            {
    //                pokemon[position].addExp(expPool);
    //                expPool = 0;
    //                float levelStartExp =
    //                    PokemonDatabase.getLevelExp(
    //                        PokemonDatabase.getPokemon(pokemon[position].Species).getLevelingRate(),
    //                        pokemon[position].Level);
    //                float currentExpMinusStart = pokemon[position].getExp() - levelStartExp;
    //                float nextLevelExpMinusStart = pokemon[position].getExpNext() - levelStartExp;
    //
    //                AudioSource fillSource = SfxHandler.Play(fillExpClip);
    //                yield return
    //                    StartCoroutine(stretchBar(position == 0 ? pokemon0ExpBar : pokemon1ExpBar, 77f * (currentExpMinusStart / nextLevelExpMinusStart), 35f));
    //                SfxHandler.FadeSource(fillSource, 0.2f);
    //                yield return new WaitForSeconds(0.5f);
    //            }
    //
    //
    //            yield return null;
    //        }
    //    }
    //}
    
    private IEnumerator addExpHidden(int position, int exp)
    {
        if (SaveData.currentSave.PC.boxes[0][position].Level < 100)
        {

            yield return
                StartCoroutine(drawTextAndWait(
                    SaveData.currentSave.PC.boxes[0][position].Name + " gained " + exp + " Exp. Points!", 1.8f,
                    0.5f));
            Dialog.UndrawDialogBox();

            int expPool = exp;
            while (expPool > 0)
            {
                int expToNextLevel = SaveData.currentSave.PC.boxes[0][position].getExpNext() -
                                     SaveData.currentSave.PC.boxes[0][position].getExp();

                //if enough exp left to level up
                if (expPool >= expToNextLevel)
                {
                    SaveData.currentSave.PC.boxes[0][position].addExp(expToNextLevel);
                    expPool -= expToNextLevel;

                    BgmHandler.main.PlayMFX(Resources.Load<AudioClip>("Audio/mfx/GetAverage"));
                    yield return
                        StartCoroutine(
                            drawTextAndWait(
                                SaveData.currentSave.PC.boxes[0][position].Name + " grew to Level " +
                                SaveData.currentSave.PC.boxes[0][position].Level + "!", 1.8f,
                                1.8f));

                    string newMove = SaveData.currentSave.PC.boxes[0][position]
                        .MoveLearnedAtLevel(SaveData.currentSave.PC.boxes[0][position].Level);
                    if (!string.IsNullOrEmpty(newMove) && !SaveData.currentSave.PC.boxes[0][position].HasMove(newMove))
                    {
                        yield return StartCoroutine(LearnMove(SaveData.currentSave.PC.boxes[0][position], newMove));
                    }

                    Dialog.UndrawDialogBox();
                    yield return new WaitForSeconds(0.8f);
                }
                else
                {
                    SaveData.currentSave.PC.boxes[0][position].addExp(expPool);
                    expPool = 0;
                    float levelStartExp =
                        PokemonDatabase.getLevelExp(
                            PokemonDatabase.getPokemon(SaveData.currentSave.PC.boxes[0][position].Species)
                                .getLevelingRate(),
                            SaveData.currentSave.PC.boxes[0][position].Level);
                }


                yield return null;
            }
        }
    }

    private IEnumerator LearnMove(IPokemon selectedPokemon, string move)
    {
        int chosenIndex = 1;
        if (chosenIndex == 1)
        {
            bool learning = true;
            while (learning)
            {
                //Moveset is full
                if (selectedPokemon.numMoves == 4)
                {
                    yield return
                        StartCoroutine(
                            drawTextAndWait(selectedPokemon.Name + " wants to learn the \nmove " + move + "."));
                    yield return
                        StartCoroutine(
                            drawTextAndWait("However, " + selectedPokemon.Name + " already \nknows four moves."));
                    yield return
                        StartCoroutine(drawTextAndWait("Should a move be deleted and \nreplaced with " + move + "?",
                            0.1f));

                    yield return StartCoroutine(Dialog.DrawChoiceBox());
                    chosenIndex = Dialog.chosenIndex;
                    Dialog.UndrawChoiceBox();
                    if (chosenIndex == 1)
                    {
                        yield return StartCoroutine(drawTextAndWait("Which move should \nbe forgotten?"));

                        yield return StartCoroutine(ScreenFade.main.Fade(false, ScreenFade.defaultSpeed));

                        //Set SceneSummary to be active so that it appears
                        Scene.main.Summary.gameObject.SetActive(true);
                        StartCoroutine(Scene.main.Summary.control(new IPokemon[] { selectedPokemon }, learning: true, newMoveString:move));
                        //Start an empty loop that will only stop when SceneSummary is no longer active (is closed)
                        while (Scene.main.Summary.gameObject.activeSelf)
                        {
                            yield return null;
                        }

                        string replacedMove = Scene.main.Summary.replacedMove;
                        yield return StartCoroutine(ScreenFade.main.Fade(true, ScreenFade.defaultSpeed));

                        if (!string.IsNullOrEmpty(replacedMove))
                        {
                            Dialog.DrawBlackFrame();
                            yield return StartCoroutine(Dialog.DrawTextSilent("1, "));
                            yield return new WaitForSeconds(0.4f);
                            yield return StartCoroutine(Dialog.DrawTextSilent("2, "));
                            yield return new WaitForSeconds(0.4f);
                            yield return StartCoroutine(Dialog.DrawTextSilent("and... "));
                            yield return new WaitForSeconds(0.4f);
                            yield return StartCoroutine(Dialog.DrawTextSilent("... "));
                            yield return new WaitForSeconds(0.4f);
                            yield return StartCoroutine(Dialog.DrawTextSilent("... "));
                            yield return new WaitForSeconds(0.4f);
                            SfxHandler.Play(pokeballBounceClip);
                            yield return StartCoroutine(Dialog.DrawTextSilent("Poof!"));
                            while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back"))
                            {
                                yield return null;
                            }

                            yield return
                                StartCoroutine(
                                    drawTextAndWait(selectedPokemon.Name + " forgot how to \nuse " + replacedMove +
                                                    "."));
                            yield return StartCoroutine(drawTextAndWait("And..."));

                            Dialog.DrawBlackFrame();
                            AudioClip mfx = Resources.Load<AudioClip>("Audio/mfx/GetAverage");
                            BgmHandler.main.PlayMFX(mfx);
                            StartCoroutine(Dialog.DrawTextSilent(selectedPokemon.Name + " learned \n" + move + "!"));
                            yield return new WaitForSeconds(mfx.length);
                            while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back"))
                            {
                                yield return null;
                            }
                            Dialog.UndrawDialogBox();
                            learning = false;
                        }
                        else
                        {
                            //give up?
                            chosenIndex = 0;
                        }
                    }
                    if (chosenIndex == 0)
                    {
                        //NOT ELSE because this may need to run after (chosenIndex == 1) runs
                        yield return
                            StartCoroutine(drawTextAndWait("Give up on learning the move \n" + move + "?", 0.1f));

                        yield return StartCoroutine(Dialog.DrawChoiceBox());
                        chosenIndex = Dialog.chosenIndex;
                        Dialog.UndrawChoiceBox();
                        if (chosenIndex == 1)
                        {
                            learning = false;
                            chosenIndex = 0;
                        }
                    }
                }
                //Moveset is not full, can fit the new move easily
                else
                {
                    //selectedPokemon.addMove(move);

                    Dialog.DrawBlackFrame();
                    AudioClip mfx = Resources.Load<AudioClip>("Audio/mfx/GetAverage");
                    BgmHandler.main.PlayMFX(mfx);
                    StartCoroutine(Dialog.DrawTextSilent(selectedPokemon.Name + " learned \n" + move + "!"));
                    yield return new WaitForSeconds(mfx.length);
                    while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back"))
                    {
                        yield return null;
                    }
                    Dialog.UndrawDialogBox();
                    learning = false;
                }
            }
        }
        if (chosenIndex == 0)
        {
            //NOT ELSE because this may need to run after (chosenIndex == 1) runs
            //cancel learning loop
            yield return StartCoroutine(drawTextAndWait(selectedPokemon.Name + " did not learn \n" + move + "."));
        }
    }


    private void updatePokemonStats(int position)
    {
        //set PokemonData
        pokemonStats[0][position] = pokemon[position].ATK;
        pokemonStats[1][position] = pokemon[position].DEF;
        pokemonStats[2][position] = pokemon[position].SPA;
        pokemonStats[3][position] = pokemon[position].SPD;
        pokemonStats[4][position] = pokemon[position].SPE;
    }


    /// Apply the Move Effect to the target pokemon if possible (with animation)
    private IEnumerator applyEffect(int attackerPosition, int targetPosition, MoveData.Effect effect, float parameter)
    {
        yield return StartCoroutine(applyEffect(attackerPosition, targetPosition, effect, parameter, true));
    }

    /// Apply the Move Effect to the target pokemon if possible
    private IEnumerator applyEffect(int attackerPosition, int targetPosition, MoveData.Effect effect, float parameter,
        bool animate)
    {
        //most effects won't happen if a target has fainted.
        /*if (pokemon[targetPosition] != null)
        {
            if (pokemon[targetPosition].Status != Status.FAINT)
            {
                if (effect == MoveData.Effect.ATK)
                {
                    yield return StartCoroutine(ModifyStat(targetPosition, 0, parameter, animate));
                }
                else if (effect == MoveData.Effect.DEF)
                {
                    yield return StartCoroutine(ModifyStat(targetPosition, 1, parameter, animate));
                }
                else if (effect == MoveData.Effect.SPA)
                {
                    yield return StartCoroutine(ModifyStat(targetPosition, 2, parameter, animate));
                }
                else if (effect == MoveData.Effect.SPD)
                {
                    yield return StartCoroutine(ModifyStat(targetPosition, 3, parameter, animate));
                }
                else if (effect == MoveData.Effect.SPE)
                {
                    yield return StartCoroutine(ModifyStat(targetPosition, 4, parameter, animate));
                }
                else if (effect == MoveData.Effect.ACC)
                {
                    yield return StartCoroutine(ModifyStat(targetPosition, 5, parameter, animate));
                }
                else if (effect == MoveData.Effect.EVA)
                {
                    yield return StartCoroutine(ModifyStat(targetPosition, 6, parameter, animate));
                }
                else if (effect == MoveData.Effect.Burn)
                {
                    if (Random.value <= parameter)
                    {
                        if (pokemon[targetPosition].setStatus(Status.BURN))
                        {
                            yield return
                                StartCoroutine(
                                    drawTextAndWait(
                                        generatePreString(targetPosition) + pokemon[targetPosition].Name +
                                        " was burned!", 2.4f));
                        }
                    }
                }
                else if (effect == MoveData.Effect.Freeze)
                {
                    if (Random.value <= parameter)
                    {
                        if (pokemon[targetPosition].setStatus(Status.FROZEN))
                        {
                            yield return
                                StartCoroutine(
                                    drawTextAndWait(
                                        generatePreString(targetPosition) + pokemon[targetPosition].Name +
                                        " was frozen solid!", 2.4f));
                        }
                    }
                }
                else if (effect == MoveData.Effect.Paralyze)
                {
                    if (Random.value <= parameter)
                    {
                        if (pokemon[targetPosition].setStatus(Status.PARALYZED))
                        {
                            yield return
                                StartCoroutine(
                                    drawTextAndWait(
                                        generatePreString(targetPosition) + pokemon[targetPosition].Name +
                                        " was paralyzed! \\nIt may be unable to move!", 2.4f));
                        }
                    }
                }
                else if (effect == MoveData.Effect.Poison)
                {
                    if (Random.value <= parameter)
                    {
                        if (pokemon[targetPosition].setStatus(Status.POISONED))
                        {
                            yield return
                                StartCoroutine(
                                    drawTextAndWait(
                                        generatePreString(targetPosition) + pokemon[targetPosition].Name +
                                        " was poisoned!", 2.4f));
                        }
                    }
                }
                else if (effect == MoveData.Effect.Toxic)
                {
                    if (Random.value <= parameter)
                    {
                        if (pokemon[targetPosition].setStatus(Status.POISONED))
                        {
                            yield return
                                StartCoroutine(
                                    drawTextAndWait(
                                        generatePreString(targetPosition) + pokemon[targetPosition].Name +
                                        " was badly posioned!", 2.4f));
                        }
                    }
                }
                else if (effect == MoveData.Effect.Sleep)
                {
                    if (Random.value <= parameter)
                    {
                        if (pokemon[targetPosition].setStatus(Status.ASLEEP))
                        {
                            yield return
                                StartCoroutine(
                                    drawTextAndWait(
                                        generatePreString(targetPosition) + pokemon[targetPosition].Name +
                                        " fell asleep!", 2.4f));
                        }
                    }
                }
            }
        }
        //effects that happen regardless of target fainting or not
        if (effect == MoveData.Effect.ATKself)
        {
            yield return StartCoroutine(ModifyStat(attackerPosition, 0, parameter, animate));
        }
        else if (effect == MoveData.Effect.DEFself)
        {
            yield return StartCoroutine(ModifyStat(attackerPosition, 1, parameter, animate));
        }
        else if (effect == MoveData.Effect.SPAself)
        {
            yield return StartCoroutine(ModifyStat(attackerPosition, 2, parameter, animate));
        }
        else if (effect == MoveData.Effect.SPDself)
        {
            yield return StartCoroutine(ModifyStat(attackerPosition, 3, parameter, animate));
        }
        else if (effect == MoveData.Effect.SPEself)
        {
            yield return StartCoroutine(ModifyStat(attackerPosition, 4, parameter, animate));
        }
        else if (effect == MoveData.Effect.ACCself)
        {
            yield return StartCoroutine(ModifyStat(attackerPosition, 5, parameter, animate));
        }
        else if (effect == MoveData.Effect.EVAself)
        {
            yield return StartCoroutine(ModifyStat(attackerPosition, 6, parameter, animate));
        }*/
        yield return null;
    }

    private IEnumerator ModifyStat(int targetPosition, int statIndex, float param, bool animate)
    {
        int parameter = Mathf.FloorToInt(param);

        string[] statName = new string[]
        {
            "Attack", "Defense", "Special Attack", "Special Defense", "Speed", "Accuracy", "Evasion"
        };

        bool canModify = true;
        if (pokemonStatsMod[statIndex][targetPosition] >= 6 && parameter > 0)
        {
            //can't go higher
            yield return
                StartCoroutine(
                    drawTextAndWait(
                        generatePreString(targetPosition) + pokemon[targetPosition].Name + "'s " +
                        statName[statIndex] + " \\nwon't go any higher!", 2.4f));
            canModify = false;
        }
        else if (pokemonStatsMod[statIndex][targetPosition] <= -6 && parameter < 0)
        {
            //can't go lower
            yield return
                StartCoroutine(
                    drawTextAndWait(
                        generatePreString(targetPosition) + pokemon[targetPosition].Name + "'s " +
                        statName[statIndex] + " won't go any lower!", 2.4f));
            canModify = false;
        }

        if (canModify)
        {
            pokemonStatsMod[statIndex][targetPosition] += parameter;
            if (pokemonStatsMod[statIndex][targetPosition] > 6)
            {
                pokemonStatsMod[statIndex][targetPosition] = 6;
            }
            else if (pokemonStatsMod[statIndex][targetPosition] < -6)
            {
                pokemonStatsMod[statIndex][targetPosition] = -6;
            }

            if (animate)
            {
                //multiple pokemon not yet implemented
                Transform target = null;

                if (targetPosition == 0)
                {
                    target = playerSpriteRenderer.transform;
                }
                else if (targetPosition == 1)
                {
                    target = player2SpriteRenderer.transform;
                }
                else if (targetPosition == 3)
                {
                    target = opponentSpriteRenderer.transform;
                }
                else if (targetPosition == 4)
                {
                    target = opponent2SpriteRenderer.transform;
                }
                
                RawImage overlay = (targetPosition < 3) ? player1Overlay : opponent1Overlay;
                if (parameter > 0)
                {
                    target.Find("Particles Stat Up").GetComponent<ParticleSystem>().Play();
                    SfxHandler.Play(statUpClip);
                    StartCoroutine(animateOverlayer(overlay, overlayStatUpTex, -1, 0, 1.2f, 0.3f));
                }
                else if (parameter < 0)
                {
                    target.Find("Particles Stat Down").GetComponent<ParticleSystem>().Play();
                    SfxHandler.Play(statDownClip);
                    StartCoroutine(animateOverlayer(overlay, overlayStatDownTex, 1, 0, 1.2f, 0.3f));
                }

                yield return new WaitForSeconds(statUpClip.length + 0.2f);
                target.Find("Particles Stat Up").GetComponent<ParticleSystem>().Stop();
                target.Find("Particles Stat Down").GetComponent<ParticleSystem>().Stop();
            }

            if (parameter == 1)
            {
                yield return
                    StartCoroutine(
                        drawTextAndWait(
                            generatePreString(targetPosition) + pokemon[targetPosition].Name + "'s " +
                            statName[statIndex] + " \\nrose!", 2.4f));
            }
            else if (parameter == -1)
            {
                yield return
                    StartCoroutine(
                        drawTextAndWait(
                            generatePreString(targetPosition) + pokemon[targetPosition].Name + "'s " +
                            statName[statIndex] + " \\nfell!", 2.4f));
            }
            else if (parameter == 2)
            {
                yield return
                    StartCoroutine(
                        drawTextAndWait(
                            generatePreString(targetPosition) + pokemon[targetPosition].Name + "'s " +
                            statName[statIndex] + " \\nrose sharply!", 2.4f));
            }
            else if (parameter == -2)
            {
                yield return
                    StartCoroutine(
                        drawTextAndWait(
                            generatePreString(targetPosition) + pokemon[targetPosition].Name + "'s " +
                            statName[statIndex] + " \\nharshly fell!", 2.4f));
            }
            else if (parameter >= 3)
            {
                yield return
                    StartCoroutine(
                        drawTextAndWait(
                            generatePreString(targetPosition) + pokemon[targetPosition].Name + "'s " +
                            statName[statIndex] + " nrose drastically!", 2.4f));
            }
            else if (parameter <= -3)
            {
                yield return
                    StartCoroutine(
                        drawTextAndWait(
                            generatePreString(targetPosition) + pokemon[targetPosition].Name + "'s " +
                            statName[statIndex] + " \\nseverely fell!", 2.4f));
            }
        }
    }


    //////////////////////////////////

    //////////////////////////////////
    /// REPEATED SEQUENCES
    //
    ///This sequence heals a pokemon on the field. Do not use to heal a pokemon in the party.  
    private IEnumerator Heal(int index, float healAmount)
    {
        yield return StartCoroutine(Heal(index, healAmount, false));
    }

    private IEnumerator Heal(int index, bool curingStatus)
    {
        yield return StartCoroutine(Heal(index, -1, curingStatus));
    }

    private IEnumerator Heal(int index, float healAmount, bool curingStatus)
    {
        //If healing, and HP is already full    OR   if curing status, and status is already none or fainted (fainted pokemon can only be cured off-field).
        /*if ((!curingStatus && pokemon[index].HP == pokemon[index].TotalHP) ||
            (curingStatus &&
             (pokemon[index].Status == Status.NONE || pokemon[index].Status == Status.FAINT)))
        {
            //no effect
            Dialog.DrawBlackFrame();
            yield return StartCoroutine(drawTextAndWait("It had no effect...", 2.4f));
        }
        else
        {
            //SOUND
            SfxHandler.Play(healFieldClip);
            //Animate
            RawImage overlay = (index < 3) ? player1Overlay : opponent1Overlay;
            yield return new WaitForSeconds(1.2f);
            yield return StartCoroutine(animateOverlayer(overlay, overlayHealTex, -1, 0, 1.2f, 0.3f));
            if (curingStatus)
            {
                Status status = pokemon[index].Status;
                pokemon[index].HealStatus();
                updatePokemonStatsDisplay(index);
                yield return new WaitForSeconds(0.3f);
                Dialog.DrawBlackFrame();
                if (status == Status.SLEEP)
                {
                    yield return
                        StartCoroutine(drawTextAndWait(
                            generatePreString(index) + pokemon[index].Name + " woke up!", 2.4f));
                }
                else if (status == Status.BURN)
                {
                    yield return
                        StartCoroutine(
                            drawTextAndWait(
                                generatePreString(index) + pokemon[index].Name + "'s burn was healed!", 2.4f));
                }
                else if (status == Status.FROZEN)
                {
                    yield return
                        StartCoroutine(
                            drawTextAndWait(generatePreString(index) + pokemon[index].Name + " thawed out!", 2.4f))
                        ;
                }
                else if (status == Status.PARALYSIS)
                {
                    yield return
                        StartCoroutine(
                            drawTextAndWait(
                                generatePreString(index) + pokemon[index].Name + " was cured of its paralysis!",
                                2.4f));
                }
                else if (status == Status.POISON)
                {
                    yield return
                        StartCoroutine(
                            drawTextAndWait(
                                generatePreString(index) + pokemon[index].Name + " was cured of its poison!", 2.4f))
                        ;
                }
            }
            else
            {
                //if under 1.01f, heal based on percentage
                if (healAmount < 1.01f)
                {
                    healAmount = Mathf.CeilToInt((float) pokemon[index].TotalHP * healAmount);
                }

                //Heal the pokemon and record how much HP was healed
                int healedHP = pokemon[index].HP;
                pokemon[index].HealHP(healAmount);
                healedHP = pokemon[index].HP - healedHP;

                if (index == 0)
                {
                    yield return
                        StartCoroutine(stretchBar(statsHPBar[index], pokemon[index].PercentHP * 51f, 32, true,
                            pokemon0CurrentHP, pokemon0CurrentHPShadow, pokemon[index].HP));
                }
                else if (index == 1)
                {
                    yield return
                        StartCoroutine(stretchBar(statsHPBar[index], pokemon[index].PercentHP * 51f, 32, true,
                            pokemon1CurrentHP, pokemon1CurrentHPShadow, pokemon[index].HP));
                }
                else
                {
                    yield return
                        StartCoroutine(stretchBar(statsHPBar[index], pokemon[index].PercentHP * 51f, 32, true, null,
                            null, 0));
                }
                
                Dialog.DrawBlackFrame();
                yield return
                    StartCoroutine(
                        drawTextAndWait(
                            generatePreString(index) + pokemon[index].Name + "'s HP was restored by " + healedHP +
                            " point(s).", 2.4f));
            }
        }*/
        yield return null;
    }


    /// Display the a textbox with a message, and wait until Select or Start/Back is pressed.
    public IEnumerator drawTextAndWait(string message)
    {
        yield return StartCoroutine(drawTextAndWait(message, 0, 0, true));
    }

    /// Display the a textbox with a message, and wait a set amount of time or until Select or Start/Back is pressed.
    public IEnumerator drawTextAndWait(string message, float time)
    {
        yield return StartCoroutine(drawTextAndWait(message, time, 0, true));
    }

    /// Display the a textbox with a message, and wait a set amount of time before until Select or Start/Back is pressed.
    public IEnumerator drawTextAndWait(string message, float time, float lockedTime)
    {
        yield return StartCoroutine(drawTextAndWait(message, time, lockedTime, true));
    }

    public IEnumerator drawTextAndWait(string message, bool silent)
    {
        yield return StartCoroutine(drawTextAndWait(message, 0, 0, silent));
    }

    public IEnumerator drawTextAndWait(string message, float time, float lockedTime, bool silent)
    {
        Dialog.DrawBlackFrame();
        float startTime = Time.time;
        if (silent)
        {
            yield return StartCoroutine(Dialog.DrawTextSilent(message));
        }
        else
        {
            yield return StartCoroutine(Dialog.DrawText(message));
        }

        if (lockedTime > 0)
        {
            while (Time.time < startTime + lockedTime)
            {
                yield return null;
            }
        }

        if (time > 0)
        {
            while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back") && Time.time < startTime + time)
            {
                yield return null;
            }
        }
        else
        {
            while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back"))
            {
                yield return null;
            }
        }
    }

    public string generatePreString(int pokemonPosition)
    {
        if (pokemonPosition > 2)
        {
            return ((trainerBattle) ? "The foe's " : "The wild ");

        }
        return "";
    }
    
    public string preStringName(int pokemonPosition)
    {
        string preString = pokemon[pokemonPosition].Name;
        if (pokemonPosition > 2)
        {
            preString = Language.getLang() switch
            {
                Language.Country.FRANCAIS => pokemon[pokemonPosition].Name + ((trainerBattle) ? " adverse" : " sauvage"),
                _ => ((trainerBattle) ? "The foe's " : "The wild ") + pokemon[pokemonPosition].Name
            };

        }
        return preString;
    }


    private float PlayCry(IPokemon pokemon)
    {
        //SfxHandler.Play(pokemon.GetCry(), pokemon.GetCryPitch());
        return 0; //pokemon.GetCry().length / pokemon.GetCryPitch();
    }

    private IEnumerator PlayCryAndWait(IPokemon pokemon)
    {
        yield return new WaitForSeconds(PlayCry(pokemon));
    }

    private IEnumerator HitAnimation(SpriteRenderer sprite)
    {
        float time = 0.075f;
        Color[] colors =
        {
            Color.black,
            Color.clear,
            Color.white
        };

        for (int i = 1; i <= 3; ++i) // Number of blink iteration
        {
            for (int j = 0; j < colors.Length; ++j) // apply each color of the table 
            {
                sprite.color = colors[j];

                yield return new WaitForSeconds(time);
            }
        }

        yield return null;
    }

    private void UpdateLowHpPlay(int target, Trainer ally)
    {
        /*if (ally == null && target <= 2 || ally != null && target == 0)
        {
            if (pokemon[target].Status == Status.FAINT)
            {
                if (ally == null)
                {
                    if (pokemon[(target + 1) % 2] == null && isLowHP ||
                        pokemon[(target + 1) % 2] != null && isLowHP && pokemon[(target + 1) % 2].PercentHP >= 0.25f)
                    {
                        isLowHP = false;
                        BgmHandler.main.PlayOverlay(battleBGM, battleBGMLoopStart, sample);
                    }
                }
                else
                {
                    isLowHP = false;
                    BgmHandler.main.PlayOverlay(battleBGM, battleBGMLoopStart, sample);
                }
            }
            else if (pokemon[target].PercentHP < 0.25f)
            {
                if (!isLowHP)
                {
                    isLowHP = true;
                    sample = BgmHandler.main.GetSample();
                    BgmHandler.main.PlayOverlay(lowHpBGM, lowHpBGMLoopStart);
                }
            }
            else
            {
                if (ally == null)
                {
                    if (pokemon[(target + 1) % 2] == null && isLowHP ||
                        pokemon[(target + 1) % 2] != null && isLowHP &&
                        pokemon[(target + 1) % 2].PercentHP >= 0.25f)
                    {
                        isLowHP = false;
                        BgmHandler.main.PlayOverlay(battleBGM, battleBGMLoopStart, sample);
                    }
                }
                else
                {
                    isLowHP = false;
                    BgmHandler.main.PlayOverlay(battleBGM, battleBGMLoopStart, sample);
                }
            }
        }*/
    }

    public int GetUsablePokemon(Trainer ally, bool doubleBattle)
    {
        IPokemon[] party = new IPokemon[0]; //SaveData.currentSave.PC.boxes[0];
        int usablePokemon = 0;
        foreach (IPokemon p in party)
        {
            if (p != null && p.Status != Status.FAINT)
            {
                if (doubleBattle)
                {
                    if (ally == null)
                    {
                        if (p != pokemon[0] && p != pokemon[1])
                        {
                            usablePokemon++;
                        }
                    }
                    else
                    {
                        if (p != pokemon[0])
                        {
                            usablePokemon++;
                        }
                    }
                }
                else
                {
                    if (p != pokemon[0])
                    {
                        usablePokemon++;
                    }
                }
            }
        }

        return usablePokemon;
    }

    /// Basic Wild Battle
    public IEnumerator control(IPokemon wildPokemon)
    {
        yield return null; //StartCoroutine(control(false, new Trainer(new IPokemon[] {wildPokemon}), false));
    }

    /// Basic Trainer Battle
    public IEnumerator control(Trainer trainer, bool doubleBattle = false, Trainer trainer2 = null, Trainer ally = null)
    {
        yield return StartCoroutine(control(true, trainer, false, doubleBattle, trainer2, ally));
    }

    public IEnumerator control(bool isTrainerBattle, Trainer trainer, bool healedOnDefeat, bool doubleBattle = false, Trainer trainer2 = null, Trainer ally = null)
    {
        pokemon = new IPokemon[6];

        ItemData tmpItem = null;
        
        expShare = new bool [SaveData.currentSave.PC.getBoxLength(0)];
        expIndex = 0;
        expIndex2 = 1;
        pokemonPerSide = doubleBattle ? 2 : 1;
        sample = 0;

        if (isTrainerBattle)
        {
            if (trainer.lowHpBGM == null)
            {
                lowHpBGM = defaultLowHpBGM;
                lowHpBGMLoopStart = defaultLowHpBGMLoopStart;
            }
            else
            {
                lowHpBGM = trainer.lowHpBGM;
                lowHpBGMLoopStart = trainer.lowHpBGMSamplesLoopStart;
            }
        }
        else
        {
            lowHpBGM = defaultLowHpBGM;
            lowHpBGMLoopStart = defaultLowHpBGMLoopStart;
        }

        LeanTween.moveLocal(BattleCamera.gameObject, startCameraAngle[0], 0);
        LeanTween.rotateLocal(BattleCamera.gameObject, startCameraAngle[1], 0);

        // Activate BattleDisplay and Scene
        BattleDisplay.SetActive(true);
        BattleScene.SetActive(true);

        // Update Discord Informations
        UpdateRPC(isTrainerBattle, trainer);
        
        //Set Pokémon size to 0
        playerSpriteRenderer.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 1);
        playerShadowSpriteRenderer.transform.localScale = new Vector3(0, 0, 0);
        
        player2SpriteRenderer.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 1);
        player2ShadowSpriteRenderer.transform.localScale = new Vector3(0, 0, 0);
        
        opponent2SpriteRenderer.GetComponent<RectTransform>().localScale = new Vector2(0, 0);
        opponent2ShadowSpriteRenderer.GetComponent<RectTransform>().localScale = new Vector2(0, 0);
        
        //Set trainer size to 2 and visible
        Transform playerTrainerSprite = BattleScene.transform.Find("player_trainer").transform;
        playerTrainerSprite.localScale = new Vector3(2, 2, 2);
        LeanTween.color(playerTrainerSprite.transform.Find("body/m_bw_battleback_1").gameObject, Color.white, 0);
        LeanTween.color(playerTrainerSprite.transform.Find("body/m_bw_battleback_2").gameObject, Color.white, 0);
        LeanTween.color(playerTrainerSprite.transform.Find("body/m_bw_battleback_3").gameObject, Color.white, 0);
        LeanTween.color(playerTrainerSprite.transform.Find("body/arm1").gameObject, Color.white, 0);
        LeanTween.color(playerTrainerSprite.transform.Find("body/arm2").gameObject, Color.white, 0);
        LeanTween.color(playerTrainerSprite.transform.Find("body/arm3").gameObject, Color.white, 0);

        // Toggle Ally trainer sprite
        Transform allyTrainerSprite = BattleScene.transform.Find("ally_trainer").transform;
        if (doubleBattle && ally != null)
        {
            allyTrainerSprite.localScale = new Vector3(2, 2, 2);
            LeanTween.color(allyTrainerSprite.transform.Find("body/m_bw_battleback_1").gameObject, Color.white, 0);
            LeanTween.color(allyTrainerSprite.transform.Find("body/m_bw_battleback_2").gameObject, Color.white, 0);
            LeanTween.color(allyTrainerSprite.transform.Find("body/m_bw_battleback_3").gameObject, Color.white, 0);
            LeanTween.color(allyTrainerSprite.transform.Find("body/arm1").gameObject, Color.white, 0);
            LeanTween.color(allyTrainerSprite.transform.Find("body/arm2").gameObject, Color.white, 0);
            LeanTween.color(allyTrainerSprite.transform.Find("body/arm3").gameObject, Color.white, 0);
        }
        else
        {
            allyTrainerSprite.localScale = new Vector3(0, 0, 0);
        }

        // Place the sprites at the right position depending on double battle
        if (doubleBattle)
        {
            //pokemonStatsDisplay[0].gameObject.SetActive(true);
            LeanTween.moveY(pokemonStatsDisplay[0].GetComponent<RectTransform>(), -45, 0);
            //pokemonStatsDisplay[0].gameObject.SetActive(false);

            LeanTween.moveX(opponentSpriteRenderer.GetComponent<RectTransform>(), -2, 0);
            LeanTween.moveX(opponentShadowSpriteRenderer.GetComponent<RectTransform>(), -2, 0);
            
            LeanTween.moveX(playerSpriteRenderer.GetComponent<RectTransform>(), -2, 0);
            LeanTween.moveX(playerShadowSpriteRenderer.GetComponent<RectTransform>(), -2, 0);
            
            LeanTween.moveX(opponentBall.gameObject, opponentSpriteRenderer.transform.position.x, 0);
            LeanTween.moveX(opponentBall2.gameObject, opponent2SpriteRenderer.transform.position.x, 0);

            if (trainer2 != null)
            {
                LeanTween.moveX(opponentTrainerSpriteRenderer.gameObject, 3.36f, 0);
                LeanTween.moveX(opponentTrainerShadowSpriteRenderer.gameObject, 3.36f, 0);
            }
            else
            {
                LeanTween.moveX(opponentTrainerSpriteRenderer.gameObject, 4.93f, 0);
                LeanTween.moveX(opponentTrainerShadowSpriteRenderer.gameObject, 4.93f, 0);
            }
        }
        else
        {
            //pokemonStatsDisplay[0].gameObject.SetActive(true);
            LeanTween.moveY(pokemonStatsDisplay[0].GetComponent<RectTransform>(), -77, 0);
            //pokemonStatsDisplay[0].gameObject.SetActive(false);

            LeanTween.moveX(opponentSpriteRenderer.GetComponent<RectTransform>(), 0, 0);
            LeanTween.moveX(opponentShadowSpriteRenderer.GetComponent<RectTransform>(), 0, 0);
            
            LeanTween.moveX(playerSpriteRenderer.GetComponent<RectTransform>(), 0, 0);
            LeanTween.moveX(playerShadowSpriteRenderer.GetComponent<RectTransform>(), 0, 0);
            
            LeanTween.moveX(opponentTrainerSpriteRenderer.gameObject, 4.93f, 0);
            LeanTween.moveX(opponentTrainerShadowSpriteRenderer.gameObject, 4.93f, 0);
            
            LeanTween.moveX(opponentBall.gameObject, 4.958f, 0);

            opponentTrainer2SpriteRenderer.transform.localScale = new Vector2(0, 0);
            opponentTrainer2ShadowSpriteRenderer.transform.localScale = new Vector2(0, 0);
        }

        //Used to compare after the battle to check for evolutions.
        int[] initialLevels = new int[6];
        for (int i = 0; i < initialLevels.Length; i++)
        {
            if (SaveData.currentSave.PC.boxes[0][i] != null)
            {
                initialLevels[i] = SaveData.currentSave.PC.boxes[0][i].Level;
            }
        }

        trainerBattle = isTrainerBattle;
        IPokemon[] opponentParty = new IPokemon[0]; //trainer.Party;
        string opponentName = trainer.GetName();
        IPokemon[] opponent2Party = new IPokemon[0];
        string opponent2Name = "";

        //if (doubleBattle && trainer2 != null)
        //{
        //    opponent2Party = trainer2.Party;
        //    opponent2Name = trainer2.GetName();
        //}
        
        IPokemon[] allyParty = new IPokemon[0];
        string allyName = "";
        
        //if (doubleBattle && ally != null)
        //{
        //    allyParty = ally.GetParty();
        //    allyName = ally.GetName();
        //}

        //GET BATTLE BACKGROUNDS
        int currentTileTag = PlayerMovement.player.currentMap.getTileTag(PlayerMovement.player.transform.position);
        Debug.Log(currentTileTag);
        backgroundObject = Instantiate(PlayerMovement.player.accessedMapSettings.getBattleBackground(currentTileTag),
            GameObject.Find("Global/BattleScene").transform);
        backgroundObject.name = "Scene";
        
        battleScenehandler = BattleScene.transform.Find("Scene").GetComponent<BattleSceneHandler>();

        playerBase.sprite = PlayerMovement.player.accessedMapSettings.getBattleBase(currentTileTag);
        opponentBase.sprite = playerBase.sprite;

        //Set and pokemon Trainer Sprites
        trainer1Animation = new Sprite[] {Resources.Load<Sprite>("null")};
        if (trainerBattle)
        {
            trainer1Animation = trainer.GetSprites();
            opponentSpriteRenderer.transform.localScale = new Vector3(0, 0, 0);
            opponentShadowSpriteRenderer.transform.localScale = new Vector3(0, 0, 0);
            opponentTrainerSpriteRenderer.transform.localScale = new Vector3(-1, 1, 1);
            opponentTrainerShadowSpriteRenderer.transform.localScale = new Vector3(1, 1, 1);
            opponentTrainerSpriteRenderer.color = Color.black;
            opponentTrainerShadowSpriteRenderer.color = new Color(0, 0, 0, 0.5f);

            if (doubleBattle && trainer2 != null)
            {
                trainer2Animation = trainer2.GetSprites();
                opponent2SpriteRenderer.transform.localScale = new Vector3(0, 0, 0);
                opponent2ShadowSpriteRenderer.transform.localScale = new Vector3(0, 0, 0);
                opponentTrainer2SpriteRenderer.transform.localScale = new Vector3(-1, 1, 1);
                opponentTrainer2ShadowSpriteRenderer.transform.localScale = new Vector3(1, 1, 1);
                opponentTrainer2SpriteRenderer.color = Color.black;
                opponentTrainer2ShadowSpriteRenderer.color = new Color(0, 0, 0, 0.5f);
            }
            else
            {
                opponent2SpriteRenderer.transform.localScale = new Vector3(0, 0, 0);
                opponent2ShadowSpriteRenderer.transform.localScale = new Vector3(0, 0, 0);
                opponentTrainer2SpriteRenderer.transform.localScale = new Vector3(0, 0, 0);
                opponentTrainer2ShadowSpriteRenderer.transform.localScale = new Vector3(0, 0, 0);
            }
        }
        else
        {
            opponentSpriteRenderer.transform.localScale = new Vector3(-1, 1, 1);
            opponentShadowSpriteRenderer.transform.localScale = new Vector3(1, 1, 1);
            opponentTrainerSpriteRenderer.transform.localScale = new Vector3(0, 0, 0);
            opponentTrainerShadowSpriteRenderer.transform.localScale = new Vector3(0, 0, 0);
            opponentSpriteRenderer.color = Color.black;
            
            opponent2SpriteRenderer.transform.localScale = new Vector3(0, 0, 0);
            opponent2ShadowSpriteRenderer.transform.localScale = new Vector3(0, 0, 0);
            opponentTrainer2SpriteRenderer.transform.localScale = new Vector3(0, 0, 0);
            opponentTrainer2ShadowSpriteRenderer.transform.localScale = new Vector3(0, 0, 0);
            opponent2SpriteRenderer.color = Color.black;
        }
        
        playerTrainer1Animation =
            Resources.LoadAll<Sprite>("PlayerSprites/" + "m_hgss_" + "back");
        playerTrainerSprite1.sprite = playerTrainer1Animation[0];
        //Note: the player animation should NEVER have no sprites 
        if (trainer1Animation.Length > 0)
        {
            trainerSprite1.sprite = trainer1Animation[0];
            opponentTrainerSpriteRenderer.sprite = trainer1Animation[0];
        }
        else
        {
            trainerSprite1.sprite = Resources.Load<Sprite>("null");
        }

        if (trainer2 != null && trainer2Animation.Length > 0)
        {
            opponentTrainerSpriteRenderer.sprite = trainer1Animation[0];
        }

        //Set trainer sprites to the center of the platform initially
        playerTrainerSprite1.rectTransform.localPosition = new Vector3(0,
            playerTrainerSprite1.rectTransform.localPosition.y, 0);
        trainerSprite1.rectTransform.localPosition = new Vector3(0, trainerSprite1.rectTransform.localPosition.y, 0);


        victor = -1; //0 = player, 1 = opponent, 2 = tie

        //Reset position variables
        currentTask = 0;
        taskPosition = 1;
        movePosition = 1;
        bagCategoryPosition = 0;
        pokePartyPosition = 0;
        itemListPagePosition = 0;
        itemListPageCount = 0;

        updateCurrentTask(-1);
        updateSelectedTask(1);
        updateSelectedMove(1);


        bool running = true;
        bool runState = true;
        int firstPkmIndex = 0;
        
        for (int i = 0; i < 6; i++)
        {
            if (SaveData.currentSave.PC.boxes[0][i] != null)
            {
                if (SaveData.currentSave.PC.boxes[0][i].Status != Status.FAINT)
                {
                    switchPokemon(0, SaveData.currentSave.PC.boxes[0][i], false, true);
                    firstPkmIndex = i;
                    break;
                }
            }
        }
        
        int secondPkmIndex;
        if (doubleBattle)
        {
            if (ally == null)
            {
                secondPkmIndex = firstPkmIndex + 1;
                
                for (; secondPkmIndex < 6; secondPkmIndex++)
                {
                    if (SaveData.currentSave.PC.boxes[0][secondPkmIndex] != null)
                    {
                        if (SaveData.currentSave.PC.boxes[0][secondPkmIndex].Status != Status.FAINT)
                        {
                            switchPokemon(1, SaveData.currentSave.PC.boxes[0][secondPkmIndex], false, true);
                            break;
                        }
                    }
                }

                if (secondPkmIndex >= 6 || SaveData.currentSave.PC.boxes[0][secondPkmIndex].Status == Status.FAINT ||
                    SaveData.currentSave.PC.boxes[0][secondPkmIndex] == null)
                {
                    secondPkmIndex = -1;
                }
            }
            else
            {
                switchPokemon(1, allyParty[0], false, true);
            }
        }
        
        
        switchPokemon(3, opponentParty[0], false, true);
        if (doubleBattle)
        {
            if (trainer2 != null)
            {
                switchPokemon(4, opponent2Party[0], false, true);
            }
            else if (opponentParty.Length > 1 && opponentParty[1] != null)
            {
                switchPokemon(4, opponentParty[1], false, true);
            }
        }

        //player1Animation = pokemon[0].GetBackAnim_();
        //opponent1Animation = pokemon[3].GetFrontAnim_();
        //if (pokemon[1] != null)
        //{
        //    player2Animation = pokemon[1].GetBackAnim_();
        //}
        //if (pokemon[4] != null)
        //{
        //    opponent2Animation = pokemon[4].GetFrontAnim_();
        //}
       

        updatePokemonStatsDisplay(0);
        updatePokemonStatsDisplay(3);
        if (pokemon[1] != null)
        {
            updatePokemonStatsDisplay(1);
        }
        if (pokemon[4] != null)
        {
            updatePokemonStatsDisplay(4);
        }

        if (pokemon[0] != null)
        {
            //updateMovesetDisplay(pokemonMoveset[0], pokemon[0].PP, pokemon[0].MaxPP);
        }

        //Animate the Pokemon being released into battle
        player1.transform.parent.parent.gameObject.SetActive(false);
        opponent1.transform.parent.parent.gameObject.SetActive(false);
        player1Overlay.gameObject.SetActive(false);
        opponent1Overlay.gameObject.SetActive(false);

        animateOpponent1 = StartCoroutine(animatePokemon(opponent1, opponent1Animation));
        animatePlayer1 = StartCoroutine(animatePokemon(player1, player1Animation));

        //TODO Animer le Sprite Renderer
        animatePlayer1SR = StartCoroutine(animatePokemonSpriteRenderer(playerSpriteRenderer, player1Animation));
        animateShadowPlayer1SR = StartCoroutine(animatePokemonSpriteRenderer(playerShadowSpriteRenderer, player1Animation));
        animateOpponent1SR = StartCoroutine(animatePokemonSpriteRenderer(opponentSpriteRenderer, opponent1Animation));
        animateShadowOpponent1SR = StartCoroutine(animatePokemonSpriteRenderer(opponentShadowSpriteRenderer, opponent1Animation));
        
        animatePlayer2SR = StartCoroutine(animatePokemonSpriteRenderer(player2SpriteRenderer, player2Animation));
        animateShadowPlayer2SR = StartCoroutine(animatePokemonSpriteRenderer(player2ShadowSpriteRenderer, player2Animation));
        animateOpponent2SR = StartCoroutine(animatePokemonSpriteRenderer(opponent2SpriteRenderer, opponent2Animation));
        animateShadowOpponent2SR = StartCoroutine(animatePokemonSpriteRenderer(opponent2ShadowSpriteRenderer, opponent2Animation));
        
        pokemonStatsDisplay[0].gameObject.SetActive(false);
        pokemonStatsDisplay[1].gameObject.SetActive(false);
        pokemonStatsDisplay[3].gameObject.SetActive(false);
        pokemonStatsDisplay[4].gameObject.SetActive(false);

        hidePartyBar(true);
        hidePartyBar(false);

//		Debug.Log(pokemon[0].Name+": HP: "+pokemon[0].TotalHP+"ATK: "+pokemon[0].ATK+"DEF: "+pokemon[0].DEF+"SPA: "+pokemon[0].SPA+"SPD: "+pokemon[0].SPD+"SPE:"+pokemon[0].SPE);
//		Debug.Log(pokemon[3].Name+": HP: "+pokemon[3].TotalHP+"ATK: "+pokemon[3].ATK+"DEF: "+pokemon[3].DEF+"SPA: "+pokemon[3].SPA+"SPD: "+pokemon[3].SPD+"SPE:"+pokemon[3].SPE);

        if (GlobalVariables.global.followerOut)
        {
            Animator trainerAnim = BattleScene.transform.Find("player_trainer").GetComponent<Animator>();
            
            playerSpriteRenderer.transform.localScale = new Vector3(1, 1, 1);
            playerShadowSpriteRenderer.transform.localScale = new Vector3(1, 1, 1);
            
            LeanTween.color(trainerAnim.transform.Find("body/m_bw_battleback_1").gameObject, Color.clear, 0f);
            LeanTween.color(trainerAnim.transform.Find("body/m_bw_battleback_2").gameObject, Color.clear, 0f);
            LeanTween.color(trainerAnim.transform.Find("body/m_bw_battleback_3").gameObject, Color.clear, 0f);
            LeanTween.color(trainerAnim.transform.Find("body/arm1").gameObject, Color.clear, 0f);
            LeanTween.color(trainerAnim.transform.Find("body/arm2").gameObject, Color.clear, 0f);
            LeanTween.color(trainerAnim.transform.Find("body/arm3").gameObject, Color.clear, 0f);
        }

        if (trainerBattle)
        {
            SfxHandler.Play(partyBallsClip, 1f, 0.95f);
            if (doubleBattle && trainer2 != null)
            {
                StartCoroutine(displayPartyBar(true, opponentParty, opponent2Party));
            }
            else
            {
                StartCoroutine(displayPartyBar(true, opponentParty));
            }
            
            //if (GlobalVariables.global.followerOut)
            //{
            //    StartCoroutine(displayPartyBar(false, SaveData.currentSave.PC.boxes[0]));
            //}

            opponentTrainerSpriteRenderer.color = Color.white;
            StartCoroutine(animateOpponentTrainer(opponentTrainerSpriteRenderer, trainer1Animation));
            StartCoroutine(animateOpponentTrainer(opponentTrainerShadowSpriteRenderer, trainer1Animation));

            if (doubleBattle && trainer2 != null)
            {
                opponentTrainer2SpriteRenderer.color = Color.white;
                StartCoroutine(animateOpponentTrainer(opponentTrainer2SpriteRenderer, trainer2Animation));
                StartCoroutine(animateOpponentTrainer(opponentTrainer2ShadowSpriteRenderer, trainer2Animation));
            }
            
            StartCoroutine(ScreenFade.main.Fade(true, 1.2f));
            
            LeanTween.moveLocal(BattleCamera.gameObject, opponentFocusCameraAngle[0], 0.8f);
            LeanTween.rotateLocal(BattleCamera.gameObject, opponentFocusCameraAngle[1], 0.8f);
            
            yield return new WaitForSeconds(0.9f);
            
            Dialog.DrawBlackFrame();

            if (doubleBattle && trainer2 != null)
            {
                StartCoroutine(Dialog.DrawTextSilent("Your are challenged by\n" + opponentName + " and " + opponent2Name + "!"));
            }
            else
            {
                StartCoroutine(Dialog.DrawTextSilent("Your are challenged by\n" + opponentName + "!"));
            }
            

            LeanTween.moveLocal(BattleCamera.gameObject, baseCameraAngle[0], 1f);
            LeanTween.rotateLocal(BattleCamera.gameObject, baseCameraAngle[1], 1f);
            
            yield return new WaitForSeconds(2f);
            
            Dialog.UndrawDialogBox();
            
            SfxHandler.Play(pokeballThrowClip);

            StartCoroutine(fadeopponentTrainer(opponentTrainerSpriteRenderer, false));
            StartCoroutine(fadeopponentTrainer(opponentTrainerShadowSpriteRenderer, true));
            
            StartCoroutine(fadeopponentTrainer(opponentTrainer2SpriteRenderer, false));
            StartCoroutine(fadeopponentTrainer(opponentTrainer2ShadowSpriteRenderer, true));

            if (doubleBattle) {
                StartCoroutine(opponentBallAnim(4));
            }
            yield return StartCoroutine(opponentBallAnim(3));
            

            //launchpokeball

            Dialog.DrawBlackFrame();
            if (trainer2 == null)
            {
                if (doubleBattle)
                {
                    StartCoroutine(Dialog.DrawTextSilent(opponentName + " sent out " + pokemon[3].Name + "\nand " + pokemon[4].Name + "!"));
                }
                else
                {
                    StartCoroutine(Dialog.DrawTextSilent(opponentName + " sent out " + pokemon[3].Name + "!"));
                }
                
            }
            else
            {
                StartCoroutine(Dialog.DrawTextSilent(opponentName + " sent out " + pokemon[3].Name + "!\n" + opponent2Name + " sent out " + pokemon[4].Name + "!"));
            }
            StartCoroutine(dismissPartyBar(true));
            if (GlobalVariables.global.followerOut)
            {
                StartCoroutine(dismissPartyBar(false));
            }
            //yield return StartCoroutine(slideTrainer(opponentBase, trainerSprite1, true, true));
            //yield return StartCoroutine(releasePokemon(opponent1));

            if (doubleBattle)
            {
                StartCoroutine(releasePokemon(opponent2SpriteRenderer, doubleBattle));
            }
            yield return StartCoroutine(releasePokemon(opponentSpriteRenderer, doubleBattle));
            //yield return new WaitForSeconds(0.3f);
            //yield return StartCoroutine(slidePokemonStats(3, false));
            yield return new WaitForSeconds(0.9f);
            if (!GlobalVariables.global.followerOut)
            {
                SfxHandler.Play(partyBallsClip, 1f, 0.95f);
                //StartCoroutine(displayPartyBar(false, SaveData.currentSave.PC.boxes[0]));
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                Dialog.UndrawDialogBox();
            }

            /*
            Dialog.DrawBlackFrame();
            StartCoroutine(Dialog.DrawTextSilent("Go " + pokemon[0].Name + "!"));
            StartCoroutine(animatePlayerThrow(playerTrainerSprite1, playerTrainer1Animation, true));
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(dismissPartyBar(false));
            StartCoroutine(slideTrainer(playerBase, playerTrainerSprite1, false, true));
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(releasePokemon(player1));
            PlayCry(pokemon[0]);
            yield return new WaitForSeconds(0.3f);
            yield return StartCoroutine(slidePokemonStats(0, false));
            yield return new WaitForSeconds(0.9f);
            Dialog.UndrawDialogBox();
            */
        }
        else
        {
            player1.transform.parent.parent.gameObject.SetActive(false);
            opponent1.transform.parent.parent.gameObject.SetActive(false);

            StartCoroutine(ScreenFade.main.Fade(true, ScreenFade.slowedSpeed));
            
            LeanTween.moveLocal(BattleCamera.gameObject, opponentFocusCameraAngle[0], 0.6f);
            LeanTween.rotateLocal(BattleCamera.gameObject, opponentFocusCameraAngle[1], 0.6f);

            yield return new WaitForSeconds(0.7f);
            //StartCoroutine(slidePokemon(opponentBase, opponent1, true, false, new Vector3(92, 0, 0)));

            //yield return StartCoroutine(slidePokemon(playerBase, player1, false, true, new Vector3(-80, -64, 0)));

            PlayCry(pokemon[3]);
            LeanTween.color(opponentSpriteRenderer.gameObject, Color.white, 0.5f);
            StartCoroutine(slidePokemonStats(3, false, doubleBattle));
            
            LeanTween.moveLocal(BattleCamera.gameObject, baseCameraAngle[0], 1f);
            LeanTween.rotateLocal(BattleCamera.gameObject, baseCameraAngle[1], 1f);
            
            yield return new WaitForSeconds(1.2f);
            
            Dialog.DrawBlackFrame();
            yield return StartCoroutine(drawTextAndWait("A wild " + pokemon[3].Name + " appeared!", 2f, 0.5f, true));
            Dialog.UndrawDialogBox();
            
            /*
            Dialog.DrawBlackFrame();
            StartCoroutine(Dialog.DrawTextSilent("Go " + pokemon[0].Name + "!"));
            StartCoroutine(animatePlayerThrow(playerTrainerSprite1, playerTrainer1Animation, true));
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(slideTrainer(playerBase, playerTrainerSprite1, false, true));
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(releasePokemon(player1));
            PlayCry(pokemon[0]);
            yield return new WaitForSeconds(0.3f);
            yield return new WaitForSeconds(0.9f);
            Dialog.UndrawDialogBox();
            */
        }

        if (!GlobalVariables.global.followerOut)
        {
            yield return playerLaunchingBall(trainerBattle, doubleBattle, ally);
        }

        /*AbilityData a = AbilityDatabase.getAbility(PokemonDatabase.getPokemon(pokemon[0].Species).getAbility(pokemon[0].Ability));
        AbilityData a2 = AbilityDatabase.getAbility(PokemonDatabase.getPokemon(pokemon[3].Species).getAbility(pokemon[3].getAbility()));

        if (a != null || a2 != null)
        {
            LeanTween.moveLocal(BattleCamera.gameObject, baseCameraAngle[0], 0.25f);
            LeanTween.rotateLocal(BattleCamera.gameObject, baseCameraAngle[1], 0.25f);

            yield return new WaitForSeconds(0.25f);
            
            if (a != null)
            {
                yield return StartCoroutine(a.EffectOnSent(this, 0));
            
                yield return new WaitForSeconds(0.4f);
            }

            if (a2 != null)
            {
                yield return StartCoroutine(a2.EffectOnSent(this, 3));
            
                yield return new WaitForSeconds(0.4f);
            }
        }

        if (doubleBattle)
        {
 
            a = (pokemon[1] != null) ? AbilityDatabase.getAbility(PokemonDatabase.getPokemon(pokemon[1].Species).getAbility(pokemon[1].getAbility())) : null;
            a2 = (pokemon[4] != null) ? AbilityDatabase.getAbility(PokemonDatabase.getPokemon(pokemon[4].Species).getAbility(pokemon[4].getAbility())) : null;

            if (a != null || a2 != null)
            {
                LeanTween.moveLocal(BattleCamera.gameObject, baseCameraAngle[0], 0.25f);
                LeanTween.rotateLocal(BattleCamera.gameObject, baseCameraAngle[1], 0.25f);

                yield return new WaitForSeconds(0.25f);
            
                if (a != null)
                {
                    yield return StartCoroutine(a.EffectOnSent(this, 1));
            
                    yield return new WaitForSeconds(0.4f);
                }

                if (a2 != null)
                {
                    yield return StartCoroutine(a2.EffectOnSent(this, 4));
            
                    yield return new WaitForSeconds(0.4f);
                }
            }
        }*/

        if (GlobalVariables.global.followerOut)
        {
            yield return StartCoroutine(slidePokemonStatsFast(0, false));
        }

        updateCurrentTask(0);
        UpdateLowHpPlay(0, ally);
        if (pokemon[1] != null)
        {
            UpdateLowHpPlay(1, ally);
        }

        expShare[0] = true;

        int currentActivePokemon = 0;

        int playerFleeAttempts = 0;
        /*while (running)
        {
            //Reset Turn Tasks
            command = new CommandType[6];
            commandMove = new MoveData[6];
            commandTarget = new int[6];
            commandItem = new ItemData[6];
            commandPokemon = new IPokemon[6];
            //

            //Reset Turn Feedback
            pokemonHasMoved = new bool[6];

            if (pokemon[0] != null)
            {
                updateMovesetDisplay(pokemonMoveset[0], pokemon[0].PP, pokemon[0].MaxPP);
            }
            else
            {
                updateMovesetDisplay(pokemonMoveset[1], pokemon[1].PP, pokemon[1].MaxPP);
                currentActivePokemon = 1;
            }

            if (pokemon[0] != null)
            {
                updatePokemonStatsDisplay(0);
            }
            
            if (pokemon[3] != null)
            {
                updatePokemonStatsDisplay(3);
            }

            if (pokemon[1] != null)
            {
                updatePokemonStatsDisplay(1);
            }
            
            if (pokemon[1] != null)
            {
                updatePokemonStatsDisplay(4);
            }
            
            updateCurrentTask(0);

            if (doubleBattle)
            {
                if (currentActivePokemon == 0)
                {
                    LeanTween.moveLocal(BattleCamera.gameObject, player1FocusCameraAngle[0], 0.25f);
                    LeanTween.rotateLocal(BattleCamera.gameObject, player1FocusCameraAngle[1], 0.25f);
                }
                else if (currentActivePokemon == 1)
                {
                    LeanTween.moveLocal(BattleCamera.gameObject, player2FocusCameraAngle[0], 0.25f);
                    LeanTween.rotateLocal(BattleCamera.gameObject, player2FocusCameraAngle[1], 0.25f);
                }
            }
            else
            {
                LeanTween.moveLocal(BattleCamera.gameObject, playerFocusCameraAngle[0], 0.25f);
                LeanTween.rotateLocal(BattleCamera.gameObject, playerFocusCameraAngle[1], 0.25f);
            }
            
            
            SfxHandler.Play(menuOpenClip, 1f, 0.75f);
            yield return StartCoroutine(displayMainTask());

            //		Debug.Log(pokemon[0].Name+": HP: "+pokemon[0].TotalHP+"ATK: "+pokemon[0].ATK+"DEF: "+pokemon[0].DEF+"SPA: "+pokemon[0].SPA+"SPD: "+pokemon[0].SPD+"SPE:"+pokemon[0].SPE);
            //		Debug.Log(pokemon[3].Name+": HP: "+pokemon[3].TotalHP+"ATK: "+pokemon[3].ATK+"DEF: "+pokemon[3].DEF+"SPA: "+pokemon[3].SPA+"SPD: "+pokemon[3].SPD+"SPE:"+pokemon[3].SPE);

            ////////////////////////////////////////
            /// Task Selection State
            ////////////////////////////////////////
            //TODO start travelling again
            //BattleTravelling = StartCoroutine(BattleCamera.StartTravelling());
            if (currentActivePokemon == 0)
            {
                statDisplayAnim = StartCoroutine(animateStatDisplay(pokemonStatsDisplay[0].transform, doubleBattle));
            }
            else if (currentActivePokemon == 1)
            {
                statDisplayAnim = StartCoroutine(animateStatDisplay(pokemonStatsDisplay[1].transform, doubleBattle));
            }
            
            //Dialog.DrawBlackFrame();
            //StartCoroutine(Dialog.DrawTextInstantSilent("What will "+pokemon[0].Name+" do?"));
            
            
            
            runState = true;
            while (runState)
            {
                if (pokemon[currentActivePokemon] != null)
                {
                    updateMovesetDisplay(pokemonMoveset[currentActivePokemon], pokemon[currentActivePokemon].PP, pokemon[currentActivePokemon].MaxPP);
                }
                
                //DEBUG OVERLAY TEXTURES
                
                
                if (UnityEngine.Input.GetKeyDown(KeyCode.Y))
                {
                    SfxHandler.Play(hitClip);
                    StartCoroutine(HitAnimation(opponentSpriteRenderer));
                    //StartCoroutine(animateOverlayer(opponent1Overlay, overlayHealTex, -1f, 0, 1.2f, 0.3f));
                }
                
                if (UnityEngine.Input.GetKeyDown(KeyCode.U))
                {
                    SfxHandler.Play(hitClip);
                    StartCoroutine(HitAnimation(playerSpriteRenderer));
                    //StartCoroutine(animateOverlayer(opponent1Overlay, overlayStatUpTex, -1f, 0, 1.2f, 0.3f));
                }
                
                if (UnityEngine.Input.GetKeyDown(KeyCode.M))
                {
                    StartCoroutine(animateMegaEvolution(false));
                    //StartCoroutine(animateOverlayer(opponent1Overlay, overlayStatUpTex, -1f, 0, 1.2f, 0.3f));
                }
                
                
                //if (UnityEngine.Input.GetKeyDown(KeyCode.I))
                //{
                //    StartCoroutine(animateOverlayer(opponent1Overlay, overlayStatDownTex, 1f, 0, 1.2f, 0.3f));
                //}
                
                

                //// NAVIGATE MAIN OPTIONS ////

                
                //buttonBag.sprite = (taskPosition == 0 || taskPosition == 3) ? buttonBagSelTex : buttonBagTex;
                //buttonFight.sprite = (taskPosition == 1) ? buttonFightSelTex : buttonFightTex;
                //buttonPoke.sprite = (taskPosition == 2 || taskPosition == 5) ? buttonPokeSelTex : buttonPokeTex;
                //buttonRun.sprite = (taskPosition == 4) ? buttonRunSelTex : buttonRunTex;
                
                
                if (UnityEngine.Input.GetAxisRaw("Vertical") > 0)
                {
                    if (taskPosition > 1)
                    {
                        updateSelectedTask(taskPosition - 1);
                        SfxHandler.Play(scrollClip);
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                else if (UnityEngine.Input.GetAxisRaw("Vertical") < 0)
                {
                    if (taskPosition < 4)
                    {
                        updateSelectedTask(taskPosition + 1);
                        SfxHandler.Play(scrollClip);
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                else if (UnityEngine.Input.GetButtonDown("Select"))
                {
                    int currentPokemon = currentActivePokemon;
                    
                    Debug.Log("CurrentPokemon : "+currentPokemon);
                    
                    //// NAVIGATE MOVESET OPTIONS ////

                    if (taskPosition == 1)
                    {
                        LeanTween.moveLocal(BattleCamera.gameObject, baseCameraAngle[0], 0.25f);
                        LeanTween.rotateLocal(BattleCamera.gameObject, baseCameraAngle[1], 0.25f);
                        
                        updateCurrentTask(1);
                        SfxHandler.Play(selectClip);
                        yield return null;

                        //while still in Move Selection menu
                        while (currentTask == 1)
                        {
                            if (UnityEngine.Input.GetAxisRaw("Horizontal") < 0) // Left
                            {
                                if (movePosition > 1 && movePosition != 3)
                                {
                                    if (movePosition == 5)
                                    {
                                        if (pokemonMoveset[currentPokemon][2] != null)
                                        {
                                            //check destination has a move there
                                            updateSelectedMove(movePosition - 1);
                                            SfxHandler.Play(scrollClip);
                                            yield return new WaitForSeconds(0.2f);
                                        }
                                    }
                                    else if (movePosition != 4)
                                    {
                                        updateSelectedMove(movePosition - 1);
                                        SfxHandler.Play(scrollClip);
                                        yield return new WaitForSeconds(0.2f);
                                    }
                                }
                            }
                            else if (UnityEngine.Input.GetAxisRaw("Horizontal") > 0) // Right
                            {
                                if (movePosition < 5 && movePosition != 2)
                                {
                                    if (movePosition == 1)
                                    {
                                        if (pokemonMoveset[currentPokemon][1] != null)
                                        {
                                            //check destination has a move there
                                            updateSelectedMove(movePosition + 1);
                                            SfxHandler.Play(scrollClip);
                                            yield return new WaitForSeconds(0.2f);
                                        }
                                    }
                                    else if (movePosition == 3)
                                    {
                                        if (pokemonMoveset[currentPokemon][2] != null)
                                        {
                                            //check destination has a move there
                                            updateSelectedMove(movePosition + 1);
                                            SfxHandler.Play(scrollClip);
                                            yield return new WaitForSeconds(0.2f);
                                        }
                                        //else
                                        //{
                                        //    updateSelectedMove(1);
                                        //    SfxHandler.Play(scrollClip);
                                        //    yield return new WaitForSeconds(0.2f);
                                        //}
                                    }
                                    else if (movePosition == 4)
                                    {
                                        if (pokemonMoveset[currentPokemon][3] != null)
                                        {
                                            //check destination has a move there
                                            updateSelectedMove(movePosition + 1);
                                            SfxHandler.Play(scrollClip);
                                            yield return new WaitForSeconds(0.2f);
                                        }
                                        else if (pokemonMoveset[currentPokemon][1] != null)
                                        {
                                            //check there is a move 2
                                            updateSelectedMove(2);
                                            SfxHandler.Play(scrollClip);
                                            yield return new WaitForSeconds(0.2f);
                                        }
                                    }
                                    else
                                    {
                                        updateSelectedMove(movePosition + 1);
                                        SfxHandler.Play(scrollClip);
                                        yield return new WaitForSeconds(0.2f);
                                    }
                                }
                            }
                            else if (UnityEngine.Input.GetAxisRaw("Vertical") > 0) // Up
                            {
                                if (movePosition == 3)
                                {
                                    //if (canMegaEvolve)
                                    //{
                                    //    updateSelectedMove(movePosition - 3);
                                    //    SfxHandler.Play(scrollClip);
                                    //    yield return new WaitForSeconds(0.2f);
                                    //}
                                    //else
                                    //{
                                        //otherwise, go down to return
                                        updateSelectedMove(1);
                                        SfxHandler.Play(scrollClip);
                                        yield return new WaitForSeconds(0.2f);
                                    //}
                                }
                                else if (movePosition > 3)
                                {
                                    if (movePosition == 5)
                                    {
                                        if (pokemonMoveset[currentPokemon][1] != null)
                                        {
                                            //check destination has a move there
                                            updateSelectedMove(movePosition - 3);
                                            SfxHandler.Play(scrollClip);
                                            yield return new WaitForSeconds(0.2f);
                                        }
                                    }
                                    else
                                    {
                                        updateSelectedMove(movePosition - 3);
                                        SfxHandler.Play(scrollClip);
                                        yield return new WaitForSeconds(0.2f);
                                    }
                                }
                            }
                            else if (UnityEngine.Input.GetAxisRaw("Vertical") < 0) // Down
                            {
                                if (movePosition < 3)
                                {
                                    if (movePosition == 1)
                                    {
                                        if (pokemonMoveset[currentPokemon][2] != null)
                                        {
                                            //check destination has a move there
                                            updateSelectedMove(movePosition + 3);
                                            SfxHandler.Play(scrollClip);
                                            yield return new WaitForSeconds(0.2f);
                                        }
                                        //else
                                        //{
                                        //    //otherwise, go down to return
                                        //    updateSelectedMove(3);
                                        //    SfxHandler.Play(scrollClip);
                                        //    yield return new WaitForSeconds(0.2f);
                                        //}
                                    }
                                    else if (movePosition == 2)
                                    {
                                        if (pokemonMoveset[currentPokemon][3] != null)
                                        {
                                            //check destination has a move there
                                            updateSelectedMove(movePosition + 3);
                                            SfxHandler.Play(scrollClip);
                                            yield return new WaitForSeconds(0.2f);
                                        }
                                        else if (pokemonMoveset[currentPokemon][2] != null)
                                        {
                                            //check if there is a move 3
                                            updateSelectedMove(4);
                                            SfxHandler.Play(scrollClip);
                                            yield return new WaitForSeconds(0.2f);
                                        }
                                        //else
                                        //{
                                        //    //otherwise, go down to return
                                        //    updateSelectedMove(3);
                                        //    SfxHandler.Play(scrollClip);
                                        //    yield return new WaitForSeconds(0.2f);
                                        //}
                                    }
                                    else
                                    {
                                        updateSelectedMove(movePosition + 3);
                                        SfxHandler.Play(scrollClip);
                                        yield return new WaitForSeconds(0.2f);
                                    }
                                }
                            }
                            else if (UnityEngine.Input.GetButtonDown("Select"))
                            {
                                if (movePosition == 0)
                                {
                                    //if mega evolution selected (mega evolution not yet implemented)
                                }
                                else if (movePosition == 3)
                                {
                                    //if back selected
                                    SfxHandler.Play(selectClip);
                                    updateCurrentTask(0);
                                }
                                else
                                {
                                    //if a move is selected
                                    //check if struggle is to be used (no PP left in any move)
                                    if (pokemon[currentPokemon].getPP(0) == 0 && pokemon[currentPokemon].getPP(1) == 0 &&
                                        pokemon[currentPokemon].getPP(2) == 0 && pokemon[currentPokemon].getPP(3) == 0)
                                    {
                                        commandMove[currentPokemon] = MoveDatabase.getMove("Struggle");
                                        runState = false;
                                    }
                                    else
                                    {
                                        //convert movePosition to moveset index
                                        int[] move = new int[] {0, 0, 1, 0, 2, 3};
                                        if (pokemon[currentPokemon].getPP(move[movePosition]) > 0)
                                        {
                                            commandMove[currentPokemon] =
                                                MoveDatabase.getMove(pokemonMoveset[currentPokemon][move[movePosition]]);
                                            runState = false;
                                        }
                                        else
                                        {
                                            yield return StartCoroutine(drawTextAndWait("That move is out of PP!"));
                                            Dialog.UndrawDialogBox();
                                        }
                                    }
                                }
                                if (!runState)
                                {
                                    //if a move was chosen.

                                    if (doubleBattle && pokemon[3] != null && pokemon[3].Status != Status.FAINT
                                        && pokemon[4] != null && pokemon[4].Status != Status.FAINT)
                                    {
                                        // OPPONENT TARGET SELECTION

                                        LeanTween.moveLocal(BattleCamera.gameObject, baseCameraAngle[0], 0.25f);
                                        LeanTween.rotateLocal(BattleCamera.gameObject, baseCameraAngle[1], 0.25f);

                                        yield return new WaitForSeconds(0.25f);

                                        updateCurrentTask(-1);

                                        int target = 3;
                                        bool targetSelection = true;
                                        while (targetSelection)
                                        {
                                            if (UnityEngine.Input.GetAxisRaw("Horizontal") < 0) // Left
                                            {
                                                target = 3;
                                                SfxHandler.Play(scrollClip);
                                                yield return new WaitForSeconds(0.2f);
                                            }
                                            else if (UnityEngine.Input.GetAxisRaw("Horizontal") > 0) // Right
                                            {
                                                target = 4;
                                                SfxHandler.Play(scrollClip);
                                                yield return new WaitForSeconds(0.2f);
                                            }
                                            else if (UnityEngine.Input.GetButtonDown("Select"))
                                            {
                                                commandTarget[currentPokemon] = target;
                                                Debug.Log("Target = " + target);
                                                command[currentPokemon] = CommandType.Move;

                                                // if in double battle you finished your choice with your first pokemon
                                                if (ally == null && currentActivePokemon == 0 && pokemon[1] != null)
                                                {
                                                    currentActivePokemon++;
                                                    runState = true;
                                                    updateCurrentTask(0);
                                                    SfxHandler.Play(selectClip);
                                                    StopCoroutine(statDisplayAnim);
                                                    statDisplayAnim =
                                                        StartCoroutine(
                                                            animateStatDisplay(pokemonStatsDisplay[1].transform, true));

                                                    // goes to the second pokemon

                                                    LeanTween.moveLocal(BattleCamera.gameObject,
                                                        player2FocusCameraAngle[0], 0.25f);
                                                    LeanTween.rotateLocal(BattleCamera.gameObject,
                                                        player2FocusCameraAngle[1], 0.25f);

                                                    targetSelection = false;
                                                }
                                                else
                                                {
                                                    SfxHandler.Play(selectClip);
                                                    if (BattleTravelling != null)
                                                        StopCoroutine(BattleTravelling);
                                                    if (statDisplayAnim != null)
                                                        StopCoroutine(statDisplayAnim);
                                                    BattleCamera.StopTravelling();
                                                    updateCurrentTask(-1);
                                                    targetSelection = false;
                                                    currentActivePokemon = 0;
                                                }
                                            }
                                            else if (UnityEngine.Input.GetButtonDown("Back"))
                                            {
                                                runState = true;
                                                targetSelection = false;
                                                updateCurrentTask(1);

                                                LeanTween.moveLocal(BattleCamera.gameObject, baseCameraAngle[0], 0.25f);
                                                LeanTween.rotateLocal(BattleCamera.gameObject, baseCameraAngle[1],
                                                    0.25f);
                                            }

                                            yield return null;
                                        }
                                    }
                                    // if in solo battle you finished your choice with your first pokemon
                                    else if (doubleBattle)
                                    {
                                        if (pokemon[3] == null || pokemon[3] != null && pokemon[3].Status != Status.FAINT)
                                        {
                                            commandTarget[currentPokemon] = 4;
                                        }
                                        else
                                        {
                                            commandTarget[currentPokemon] = 3;
                                        }
                                            
                                        // if in double battle you finished your choice with your first pokemon
                                        if (ally == null && currentActivePokemon == 0 && pokemon[1] != null)
                                        {
                                            currentActivePokemon++;
                                            runState = true;
                                            command[currentPokemon] = CommandType.Move;
                                            updateCurrentTask(0);
                                            SfxHandler.Play(selectClip);
                                            StopCoroutine(statDisplayAnim);
                                            statDisplayAnim =
                                                StartCoroutine(
                                                    animateStatDisplay(pokemonStatsDisplay[1].transform, true));

                                            // goes to the second pokemon

                                            LeanTween.moveLocal(BattleCamera.gameObject,
                                                player2FocusCameraAngle[0], 0.25f);
                                            LeanTween.rotateLocal(BattleCamera.gameObject,
                                                player2FocusCameraAngle[1], 0.25f);
                                        }
                                        else
                                        {
                                            SfxHandler.Play(selectClip);
                                            command[currentPokemon] = CommandType.Move;
                                            if (BattleTravelling != null)
                                                StopCoroutine(BattleTravelling);
                                            if (statDisplayAnim != null)
                                                StopCoroutine(statDisplayAnim);
                                            BattleCamera.StopTravelling();
                                            updateCurrentTask(-1);
                                            currentActivePokemon = 0;
                                        }
                                    }
                                    else
                                    {
                                        commandTarget[currentPokemon] = 3;
                                        SfxHandler.Play(selectClip);
                                        command[currentPokemon] = CommandType.Move;
                                        if (BattleTravelling != null)
                                            StopCoroutine(BattleTravelling);
                                        if (statDisplayAnim != null)
                                            StopCoroutine(statDisplayAnim);
                                        BattleCamera.StopTravelling();
                                        updateCurrentTask(-1);
                                    }
                                }
                            }
                            else if (UnityEngine.Input.GetButtonDown("Back"))
                            {
                                SfxHandler.Play(selectClip);
                                updateCurrentTask(0);
                                
                                if (doubleBattle)
                                {
                                    if (currentActivePokemon == 0)
                                    {
                                        LeanTween.moveLocal(BattleCamera.gameObject, player1FocusCameraAngle[0], 0.25f);
                                        LeanTween.rotateLocal(BattleCamera.gameObject, player1FocusCameraAngle[1], 0.25f);
                                    }
                                    else if (currentActivePokemon == 1)
                                    {
                                        LeanTween.moveLocal(BattleCamera.gameObject, player2FocusCameraAngle[0], 0.25f);
                                        LeanTween.rotateLocal(BattleCamera.gameObject, player2FocusCameraAngle[1], 0.25f);
                                    }
                                }
                                else
                                {
                                    LeanTween.moveLocal(BattleCamera.gameObject, playerFocusCameraAngle[0], 0.25f);
                                    LeanTween.rotateLocal(BattleCamera.gameObject, playerFocusCameraAngle[1], 0.25f);
                                }
                            }
                            else if (canMegaEvolve && UnityEngine.Input.GetKeyDown(KeyCode.M))
                            {
                                megaActivate = !megaActivate;
                                updateSelectedMove(movePosition);
                                SfxHandler.Play(scrollClip);
                                yield return new WaitForSeconds(0.2f);
                            }

                            yield return null;
                        }
                    }

                    //// ATTEMPT TO FLEE ////

                    else if (taskPosition == 4)
                    {
                        SfxHandler.Play(selectClip);
                        if (trainerBattle)
                        {
                            yield return
                                StartCoroutine(drawTextAndWait("No! There's no running from \\na Trainer Battle!"));
                            Dialog.UndrawDialogBox();
                        }
                        else
                        {
                            command[currentPokemon] = CommandType.Flee;
                            runState = false;
                            updateCurrentTask(-1);

                            if (doubleBattle && currentPokemon == 0 && ally == null)
                            {
                                runState = true;
                                currentActivePokemon++;
                                updateCurrentTask(0);

                                yield return new WaitForSeconds(0.2f);
                            }
                            else
                            {
                                currentActivePokemon = 0;
                            }
                        }
                    }

                    //// OPEN BAG INTERFACE ////

                    else if (taskPosition == 0 || taskPosition == 3)
                    {
                        updateCurrentTask(2);
                        SfxHandler.Play(selectClip);
                        yield return null;

                        updateSelectedBagCategory(bagCategoryPosition);
                        //while still in Bag menu
                        while (currentTask == 2)
                        {
                            if (UnityEngine.Input.GetAxisRaw("Vertical") < 0)
                            {
                                if (bagCategoryPosition < 4)
                                {
                                    updateSelectedBagCategory(bagCategoryPosition + 2);
                                    SfxHandler.Play(scrollClip);
                                    yield return new WaitForSeconds(0.2f);
                                }
                            }
                            else if (UnityEngine.Input.GetAxisRaw("Horizontal") > 0)
                            {
                                if (bagCategoryPosition == 0 || bagCategoryPosition == 2 || bagCategoryPosition == 4)
                                {
                                    updateSelectedBagCategory(bagCategoryPosition + 1);
                                    SfxHandler.Play(scrollClip);
                                    yield return new WaitForSeconds(0.2f);
                                }
                            }
                            else if (UnityEngine.Input.GetAxisRaw("Horizontal") < 0)
                            {
                                if (bagCategoryPosition == 1 || bagCategoryPosition == 3 || bagCategoryPosition == 5)
                                {
                                    updateSelectedBagCategory(bagCategoryPosition - 1);
                                    SfxHandler.Play(scrollClip);
                                    yield return new WaitForSeconds(0.2f);
                                }
                            }
                            else if (UnityEngine.Input.GetAxisRaw("Vertical") > 0)
                            {
                                if (bagCategoryPosition > 1)
                                {
                                    updateSelectedBagCategory(bagCategoryPosition - 2);
                                    SfxHandler.Play(scrollClip);
                                    yield return new WaitForSeconds(0.2f);
                                }
                            }
                            else if (UnityEngine.Input.GetButtonDown("Select"))
                            {
                                if (bagCategoryPosition < 4)
                                {
                                    //Item Category
                                    updateCurrentTask(4);
                                    SfxHandler.Play(selectClip);

                                    int itemListPosition = updateSelectedItemListSlot(0, 0);
                                    yield return new WaitForSeconds(0.2f);
                                    while (currentTask == 4)
                                    {
                                        if (UnityEngine.Input.GetAxisRaw("Vertical") < 0)
                                        {
                                            if (itemListPosition < 8)
                                            {
                                                int positionBeforeModification = itemListPosition;
                                                if (itemListPosition < 7)
                                                {
                                                    itemListPosition = updateSelectedItemListSlot(itemListPosition, 2);
                                                }
                                                else
                                                {
                                                    itemListPosition = updateSelectedItemListSlot(itemListPosition, 1);
                                                }
                                                if (positionBeforeModification != itemListPosition)
                                                {
                                                    SfxHandler.Play(scrollClip);
                                                    yield return new WaitForSeconds(0.2f);
                                                }
                                            }
                                        }
                                        else if (UnityEngine.Input.GetAxisRaw("Horizontal") > 0)
                                        {
                                            if (itemListPosition < 8)
                                            {
                                                if (itemListPosition % 2 == 0)
                                                {
                                                    int positionBeforeModification = itemListPosition;
                                                    itemListPosition = updateSelectedItemListSlot(itemListPosition, 1);
                                                    if (positionBeforeModification != itemListPosition)
                                                    {
                                                        SfxHandler.Play(scrollClip);
                                                        yield return new WaitForSeconds(0.2f);
                                                    }
                                                }
                                                //go to next page of items
                                                else if (itemListPagePosition < itemListPageCount - 1)
                                                {
                                                    int positionBeforeModification = itemListPosition;
                                                    itemListPagePosition += 1;
                                                    updateItemListDisplay();
                                                    itemListPosition = updateSelectedItemListSlot(itemListPosition, -1);
                                                    if (positionBeforeModification != itemListPosition)
                                                    {
                                                        SfxHandler.Play(scrollClip);
                                                        yield return new WaitForSeconds(0.2f);
                                                    }
                                                }
                                            }
                                        }
                                        else if (UnityEngine.Input.GetAxisRaw("Horizontal") < 0)
                                        {
                                            if (itemListPosition < 8)
                                            {
                                                if (itemListPosition % 2 == 1)
                                                {
                                                    int positionBeforeModification = itemListPosition;
                                                    itemListPosition = updateSelectedItemListSlot(itemListPosition, -1);
                                                    if (positionBeforeModification != itemListPosition)
                                                    {
                                                        SfxHandler.Play(scrollClip);
                                                        yield return new WaitForSeconds(0.2f);
                                                    }
                                                }
                                                //go to previous page of items
                                                else if (itemListPagePosition > 0)
                                                {
                                                    int positionBeforeModification = itemListPosition;
                                                    itemListPagePosition -= 1;
                                                    updateItemListDisplay();
                                                    itemListPosition = updateSelectedItemListSlot(itemListPosition, 1);
                                                    if (positionBeforeModification != itemListPosition)
                                                    {
                                                        SfxHandler.Play(scrollClip);
                                                        yield return new WaitForSeconds(0.2f);
                                                    }
                                                }
                                            }
                                        }
                                        else if (UnityEngine.Input.GetAxisRaw("Vertical") > 0)
                                        {
                                            if (itemListPosition > 1)
                                            {
                                                int positionBeforeModification = itemListPosition;
                                                itemListPosition = updateSelectedItemListSlot(itemListPosition, -2);
                                                if (positionBeforeModification != itemListPosition)
                                                {
                                                    SfxHandler.Play(scrollClip);
                                                    yield return new WaitForSeconds(0.2f);
                                                }
                                            }
                                        }
                                        else if (UnityEngine.Input.GetButtonDown("Select"))
                                        {
                                            if (itemListPosition == 8)
                                            {
                                                SfxHandler.Play(selectClip);
                                                updateCurrentTask(2);
                                                yield return new WaitForSeconds(0.2f);
                                            }
                                            else
                                            {
                                                //use item
                                                ItemData selectedItem =
                                                    ItemDatabase.getItem(
                                                        itemListString[itemListPosition + (8 * itemListPagePosition)]);

                                                //Check item can be used
                                                if (selectedItem.getItemEffect() == ItemData.ItemEffect.HP)
                                                {
                                                    //Check target pokemon's health is not full
                                                    int target = currentPokemon; // TODO target selection not yet implemented
                                                    
                                                    if (pokemon[target].HP < pokemon[target].TotalHP)
                                                    {
                                                        commandItem[currentPokemon] = selectedItem;
                                                        commandTarget[currentPokemon] = target;
                                                        SaveData.currentSave.Bag.removeItem(selectedItem.Name, 1);
                                                        runState = false;
                                                    }
                                                    else
                                                    {
                                                        yield return
                                                            StartCoroutine(drawTextAndWait("It won't have any effect!"))
                                                            ;
                                                        Dialog.UndrawDialogBox();
                                                    }
                                                }
                                                else if (selectedItem.getItemEffect() == ItemData.ItemEffect.STATUS)
                                                {
                                                    int target = 0; //target selection not yet implemented

                                                    //Check target pokemon has the status the item cures
                                                    string statusCurer = selectedItem.getStringParameter().ToUpper();
                                                    //if an ALL is used, set it to cure anything but FAINTED or NONE.
                                                    if (statusCurer == "ALL" &&
                                                        pokemon[target].Status.ToString() != "FAINTED" &&
                                                        pokemon[target].Status.ToString() != "NONE")
                                                    {
                                                        statusCurer = pokemon[target].Status.ToString();
                                                    }

                                                    if (pokemon[target].Status.ToString() == statusCurer)
                                                    {
                                                        commandItem[currentPokemon] = selectedItem;
                                                        commandTarget[currentPokemon] = target;
                                                        SaveData.currentSave.Bag.removeItem(
                                                            selectedItem.Name, 1);
                                                        runState = false;
                                                    }
                                                    else
                                                    {
                                                        yield return
                                                            StartCoroutine(drawTextAndWait("It won't have any effect!"))
                                                            ;
                                                        Dialog.UndrawDialogBox();
                                                    }
                                                }
                                                else
                                                {
                                                    commandItem[currentPokemon] = selectedItem; //itemListString[itemListPosition]
                                                    SaveData.currentSave.Bag.removeItem(
                                                        itemListString[itemListPosition], 1);
                                                    runState = false;
                                                }
                                                if (!runState)
                                                {
                                                    if (doubleBattle && ally == null && pokemon[1] != null && currentPokemon == 0)
                                                    {
                                                        // If an item was chosen for the first Pokemon in 2v2

                                                        currentActivePokemon++;
                                                        runState = true;
                                                        command[currentPokemon] = CommandType.Item;
                                                        updateCurrentTask(0);
                                                        SfxHandler.Play(selectClip);
                                                        StopCoroutine(statDisplayAnim);
                                                        statDisplayAnim =
                                                            StartCoroutine(
                                                                animateStatDisplay(pokemonStatsDisplay[1].transform, true));

                                                        // goes to the second pokemon

                                                        LeanTween.moveLocal(BattleCamera.gameObject,
                                                            player2FocusCameraAngle[0], 0.25f);
                                                        LeanTween.rotateLocal(BattleCamera.gameObject,
                                                            player2FocusCameraAngle[1], 0.25f);
                                                    }
                                                    else
                                                    {
                                                        currentActivePokemon = 0;
                                                        
                                                        //if an item was chosen.
                                                        SfxHandler.Play(selectClip);
                                                        command[currentPokemon] = CommandType.Item;
                                                        updateCurrentTask(-1);
                                                    }
                                                    
                                                    
                                                }
                                            }
                                        }
                                        else if (UnityEngine.Input.GetButtonDown("Back"))
                                        {
                                            SfxHandler.Play(selectClip);
                                            updateCurrentTask(2);
                                            yield return new WaitForSeconds(0.2f);
                                        }


                                        yield return null;
                                    }
                                }
                                else if (bagCategoryPosition == 4)
                                {
                                    //Back
                                    updateCurrentTask(0);
                                    SfxHandler.Play(selectClip);
                                    yield return new WaitForSeconds(0.2f);
                                }
                                else if (bagCategoryPosition == 5)
                                {
                                    //Item used last
                                    SfxHandler.Play(selectClip);
                                    yield return new WaitForSeconds(0.2f);
                                }
                            }
                            else if (UnityEngine.Input.GetButtonDown("Back"))
                            {
                                SfxHandler.Play(selectClip);
                                updateCurrentTask(0);
                                yield return new WaitForSeconds(0.2f);
                            }

                            yield return null;
                        }
                    }

                    //// OPEN POKEMON INTERFACE ////

                    else if (taskPosition == 2 || taskPosition == 5)
                    {
                        updateCurrentTask(3);
                        SfxHandler.Play(selectClip);
                        yield return null;

                        //while still in Poke menu
                        while (currentTask == 3)
                        {
                            if (UnityEngine.Input.GetAxisRaw("Vertical") < 0)
                            {
                                if (pokePartyPosition < 6)
                                {
                                    if (pokePartyPosition == 5)
                                    {
                                        updateSelectedPokemonSlot(pokePartyPosition + 1);
                                    }
                                    else
                                    {
                                        updateSelectedPokemonSlot(pokePartyPosition + 2);
                                    }
                                    SfxHandler.Play(scrollClip);
                                    yield return new WaitForSeconds(0.2f);
                                }
                            }
                            else if (UnityEngine.Input.GetAxisRaw("Horizontal") > 0)
                            {
                                if (pokePartyPosition < 6)
                                {
                                    updateSelectedPokemonSlot(pokePartyPosition + 1);
                                    SfxHandler.Play(scrollClip);
                                    yield return new WaitForSeconds(0.2f);
                                }
                            }
                            else if (UnityEngine.Input.GetAxisRaw("Horizontal") < 0)
                            {
                                if (pokePartyPosition > 0)
                                {
                                    updateSelectedPokemonSlot(pokePartyPosition - 1);
                                    SfxHandler.Play(scrollClip);
                                    yield return new WaitForSeconds(0.2f);
                                }
                            }
                            else if (UnityEngine.Input.GetAxisRaw("Vertical") > 0)
                            {
                                if (pokePartyPosition > 1)
                                {
                                    if (pokePartyPosition == 6)
                                    {
                                        updateSelectedPokemonSlot(pokePartyPosition - 1);
                                    }
                                    else
                                    {
                                        updateSelectedPokemonSlot(pokePartyPosition - 2);
                                    }
                                    SfxHandler.Play(scrollClip);
                                    yield return new WaitForSeconds(0.2f);
                                }
                            }
                            else if (UnityEngine.Input.GetButtonDown("Select"))
                            {
                                if (pokePartyPosition == 6)
                                {
                                    //Back
                                    SfxHandler.Play(selectClip);
                                    updateCurrentTask(0);
                                    yield return new WaitForSeconds(0.2f);
                                }
                                else
                                {
                                    updateCurrentTask(5);
                                    SfxHandler.Play(selectClip);
                                    int summaryPosition = updateSummaryPosition(0); //0 = Switch, 1 = Moves, 2 = Back

                                    yield return new WaitForSeconds(0.2f);
                                    while (currentTask == 5)
                                    {
                                        if (UnityEngine.Input.GetAxisRaw("Vertical") < 0)
                                        {
                                            if (pokePartyPosition < 5)
                                            {
                                                int positionBeforeModification = pokePartyPosition;
                                                updateSelectedPokemonSlot(pokePartyPosition + 1, false);
                                                if (positionBeforeModification != pokePartyPosition)
                                                {
                                                    SfxHandler.Play(scrollClip);
                                                }
                                                updatePokemonSummaryDisplay(
                                                    SaveData.currentSave.PC.boxes[0][pokePartyPosition]);
                                                yield return new WaitForSeconds(0.2f);
                                            }
                                        }
                                        else if (UnityEngine.Input.GetAxisRaw("Horizontal") > 0)
                                        {
                                            if (summaryPosition < 2)
                                            {
                                                summaryPosition = updateSummaryPosition(summaryPosition + 1);
                                                SfxHandler.Play(scrollClip);
                                                yield return new WaitForSeconds(0.2f);
                                            }
                                        }
                                        else if (UnityEngine.Input.GetAxisRaw("Horizontal") < 0)
                                        {
                                            if (summaryPosition > 0)
                                            {
                                                summaryPosition = updateSummaryPosition(summaryPosition - 1);
                                                SfxHandler.Play(scrollClip);
                                                yield return new WaitForSeconds(0.2f);
                                            }
                                        }
                                        else if (UnityEngine.Input.GetAxisRaw("Vertical") > 0)
                                        {
                                            if (pokePartyPosition > 0)
                                            {
                                                int positionBeforeModification = pokePartyPosition;
                                                updateSelectedPokemonSlot(pokePartyPosition - 1, false);
                                                if (positionBeforeModification != pokePartyPosition)
                                                {
                                                    SfxHandler.Play(scrollClip);
                                                }
                                                updatePokemonSummaryDisplay(
                                                    SaveData.currentSave.PC.boxes[0][pokePartyPosition]);
                                                yield return new WaitForSeconds(0.2f);
                                            }
                                        }
                                        else if (UnityEngine.Input.GetButtonDown("Select"))
                                        {
                                            if (summaryPosition == 0)
                                            {
                                                if (SaveData.currentSave.PC.boxes[0][pokePartyPosition].Status !=
                                                    Status.FAINT)
                                                {
                                                    //check that pokemon is not on the field
                                                    bool notOnField = true;
                                                    for (int i = 0; i < pokemonPerSide; i++)
                                                    {
                                                        if (SaveData.currentSave.PC.boxes[0][pokePartyPosition] ==
                                                            pokemon[i])
                                                        {
                                                            notOnField = false;
                                                            i = pokemonPerSide;
                                                        }
                                                    }

                                                    if (currentPokemon == 1 && command[0] == CommandType.Switch)
                                                    {
                                                        if (commandPokemon[0] == SaveData.currentSave.PC.boxes[0][pokePartyPosition])
                                                            notOnField = false;
                                                    }
                                                    
                                                    if (notOnField)
                                                    {
                                                        command[currentPokemon] = CommandType.Switch;
                                                        commandPokemon[currentPokemon] =
                                                            SaveData.currentSave.PC.boxes[0][pokePartyPosition];
                                                        if (currentPokemon == 0)
                                                        {
                                                            expIndex = pokePartyPosition;
                                                        }
                                                        else
                                                        {
                                                            expIndex2 = pokePartyPosition;
                                                        }
                                                        
                                                        Debug.Log("Switching to index "+expIndex);
                                                        runState = false;

                                                        if (doubleBattle && ally == null && pokemon[1] != null && currentPokemon == 0)
                                                        {
                                                            // If an item was chosen for the first Pokemon in 2v2

                                                            currentActivePokemon++;
                                                            runState = true;
                                                            updateCurrentTask(0);
                                                            StopCoroutine(statDisplayAnim);
                                                            statDisplayAnim =
                                                                StartCoroutine(
                                                                    animateStatDisplay(pokemonStatsDisplay[1].transform, true));

                                                            // goes to the second pokemon

                                                            LeanTween.moveLocal(BattleCamera.gameObject,
                                                                player2FocusCameraAngle[0], 0.25f);
                                                            LeanTween.rotateLocal(BattleCamera.gameObject,
                                                                player2FocusCameraAngle[1], 0.25f);
                                                        }
                                                        else
                                                        {
                                                            currentActivePokemon = 0;
                                                            
                                                            //if an switch was chosen.
                                                            updateCurrentTask(-1);
                                                        }
                                                        
                                                        
                                                        SfxHandler.Play(selectClip);
                                                        yield return new WaitForSeconds(0.2f);
                                                    }
                                                    else
                                                    {
                                                        yield return
                                                            StartCoroutine(
                                                                drawTextAndWait(
                                                                    SaveData.currentSave.PC.boxes[0][pokePartyPosition]
                                                                        .Name + " is already fighting!"));
                                                        Dialog.UndrawDialogBox();
                                                    }
                                                }
                                                else
                                                {
                                                    yield return
                                                        StartCoroutine(
                                                            drawTextAndWait(
                                                                SaveData.currentSave.PC.boxes[0][pokePartyPosition]
                                                                    .Name + " is unable to fight!"));
                                                    Dialog.UndrawDialogBox();
                                                }
                                            }
                                            else if (summaryPosition == 1)
                                            {
//check moves
                                                updateCurrentTask(6);
                                                SfxHandler.Play(selectClip);
                                                yield return new WaitForSeconds(0.2f);

                                                int movesPosition = 5; //0-3 = Moves, 4 = Switch, 5 = Summary, 6 = Back
                                                while (currentTask == 6)
                                                {
                                                    if (UnityEngine.Input.GetAxisRaw("Vertical") < 0)
                                                    {
                                                        if (movesPosition < 4)
                                                        {
                                                            if (movesPosition == 2)
                                                            {
                                                                movesPosition = updateMovesPosition(movesPosition + 3);
                                                            }
                                                            else
                                                            {
                                                                movesPosition = updateMovesPosition(movesPosition + 2);
                                                            }
                                                            SfxHandler.Play(scrollClip);
                                                            yield return new WaitForSeconds(0.2f);
                                                        }
                                                    }
                                                    else if (UnityEngine.Input.GetAxisRaw("Horizontal") > 0)
                                                    {
                                                        if (movesPosition != 1 || movesPosition != 3 ||
                                                            movesPosition != 6)
                                                        {
                                                            movesPosition = updateMovesPosition(movesPosition + 1);
                                                            SfxHandler.Play(scrollClip);
                                                            yield return new WaitForSeconds(0.2f);
                                                        }
                                                    }
                                                    else if (UnityEngine.Input.GetAxisRaw("Horizontal") < 0)
                                                    {
                                                        if (movesPosition == 1 || movesPosition == 3 ||
                                                            movesPosition > 4)
                                                        {
                                                            movesPosition = updateMovesPosition(movesPosition - 1);
                                                            SfxHandler.Play(scrollClip);
                                                            yield return new WaitForSeconds(0.2f);
                                                        }
                                                    }
                                                    else if (UnityEngine.Input.GetAxisRaw("Vertical") > 0)
                                                    {
                                                        if (movesPosition > 1)
                                                        {
                                                            if (movesPosition > 3)
                                                            {
                                                                movesPosition = updateMovesPosition(2);
                                                            }
                                                            else
                                                            {
                                                                movesPosition = updateMovesPosition(movesPosition - 2);
                                                            }
                                                            SfxHandler.Play(scrollClip);
                                                            yield return new WaitForSeconds(0.2f);
                                                        }
                                                    }
                                                    else if (UnityEngine.Input.GetButtonDown("Select"))
                                                    {
                                                        if (movesPosition == 4)
                                                        {
                                                            if (
                                                                SaveData.currentSave.PC.boxes[0][pokePartyPosition]
                                                                    .Status != Status.FAINT)
                                                            {
                                                                //check that pokemon is not on the field
                                                                bool notOnField = true;
                                                                for (int i = 0; i < pokemonPerSide; i++)
                                                                {
                                                                    if (
                                                                        SaveData.currentSave.PC.boxes[0][
                                                                            pokePartyPosition] == pokemon[i])
                                                                    {
                                                                        notOnField = false;
                                                                        i = pokemonPerSide;
                                                                    }
                                                                }
                                                                if (notOnField)
                                                                {
                                                                    //debug
                                                                    command[currentPokemon] = CommandType.Switch;
                                                                    commandPokemon[currentPokemon] =
                                                                        SaveData.currentSave.PC.boxes[0][
                                                                            pokePartyPosition];
                                                                    if (currentPokemon == 0)
                                                                    {
                                                                        expIndex = pokePartyPosition;
                                                                    }
                                                                    else
                                                                    {
                                                                        expIndex2 = pokePartyPosition;
                                                                    }
                                                                    Debug.Log("Switching to index "+expIndex);
                                                                    runState = false;
                                                                    updateCurrentTask(-1);
                                                                    SfxHandler.Play(selectClip);
                                                                    yield return new WaitForSeconds(0.2f);
                                                                }
                                                                else
                                                                {
                                                                    yield return
                                                                        StartCoroutine(
                                                                            drawTextAndWait(
                                                                                SaveData.currentSave.PC.boxes[0][
                                                                                    pokePartyPosition].Name +
                                                                                " is already fighting!"));
                                                                    Dialog.UndrawDialogBox();
                                                                }
                                                            }
                                                            else
                                                            {
                                                                yield return
                                                                    StartCoroutine(
                                                                        drawTextAndWait(
                                                                            SaveData.currentSave.PC.boxes[0][
                                                                                pokePartyPosition].Name +
                                                                            " is unable to fight!"));
                                                                Dialog.UndrawDialogBox();
                                                            }
                                                        }
                                                        else if (movesPosition == 5)
                                                        {
//check summary
                                                            updateCurrentTask(5);
                                                            SfxHandler.Play(selectClip);
                                                            yield return new WaitForSeconds(0.2f);
                                                        }
                                                        else if (movesPosition == 6)
                                                        {
//back
                                                            updateCurrentTask(3);
                                                            SfxHandler.Play(selectClip);
                                                            yield return new WaitForSeconds(0.2f);
                                                        }
                                                    }
                                                    else if (UnityEngine.Input.GetButtonDown("Back"))
                                                    {
                                                        updateCurrentTask(3);
                                                        SfxHandler.Play(selectClip);
                                                        yield return new WaitForSeconds(0.2f);
                                                    }

                                                    yield return null;
                                                }
                                            }
                                            else if (summaryPosition == 2)
                                            {
//back
                                                updateCurrentTask(3);
                                                SfxHandler.Play(selectClip);
                                                yield return new WaitForSeconds(0.2f);
                                            }
                                        }
                                        else if (UnityEngine.Input.GetButtonDown("Back"))
                                        {
                                            updateCurrentTask(3);
                                            SfxHandler.Play(selectClip);
                                            yield return new WaitForSeconds(0.2f);
                                        }

                                        yield return null;
                                    }
                                }
                            }
                            else if (UnityEngine.Input.GetButtonDown("Back"))
                            {
                                SfxHandler.Play(selectClip);
                                updateCurrentTask(0);
                            }

                            yield return null;
                        }
                    }
                }
                else if (UnityEngine.Input.GetButtonDown("Back"))
                {
                    if (currentActivePokemon == 1 && pokemon[0] != null)
                    {
                        // Roll back to first Pokémon
                        if (commandItem[0] != null)
                        {
                            SaveData.currentSave.Bag.addItem(commandItem[0].Name, 1);
                            commandItem[0] = null;
                        }
                        SfxHandler.Play(selectClip);
                        currentActivePokemon--;
                        updateCurrentTask(0);
                        StopCoroutine(statDisplayAnim);
                        statDisplayAnim =
                            StartCoroutine(animateStatDisplay(pokemonStatsDisplay[0].transform, true));

                        LeanTween.moveLocal(BattleCamera.gameObject, player1FocusCameraAngle[0], 0.25f);
                        LeanTween.rotateLocal(BattleCamera.gameObject, player1FocusCameraAngle[1], 0.25f);
                        
                        yield return new WaitForSeconds(0.2f);
                    }
                }

                yield return null;
            }


            ////////////////////////////////////////
            /// AI turn selection
            ////////////////////////////////////////

            //AI not yet implemented properly.
            //the following code randomly chooses a move to use with no further thought.
            for (int i = 0; i < pokemonPerSide; i++)    // Opponent Random AI
            {
                //do for every pokemon on enemy side
                int pi = i + 3;
                if (pokemon[pi] != null)
                {
                    //check if struggle is to be used (no PP left in any move)
                    if (pokemon[pi].getPP(0) == 0 && pokemon[pi].getPP(1) == 0 &&
                        pokemon[pi].getPP(2) == 0 && pokemon[pi].getPP(3) == 0)
                    {
                        commandMove[pi] = MoveDatabase.getMove("Struggle");
                    }
                    else
                    {
                        //Randomly choose a move from the moveset
                        int AImoveIndex = Random.Range(0, 4);
                        while (pokemonMoveset[pi] != null && string.IsNullOrEmpty(pokemonMoveset[pi][AImoveIndex]) &&
                               pokemon[pi].getPP(AImoveIndex) == 0)
                        {
                            AImoveIndex = Random.Range(0, 4);
                        }
                        command[pi] = CommandType.Move;
                        commandMove[pi] = MoveDatabase.getMove(pokemonMoveset[pi][AImoveIndex]);
                        commandTarget[pi] = Random.value > 0.5f ? 0 : 1;
                        Debug.Log(commandMove[pi].Name + ", PP: " + pokemon[pi].getPP(AImoveIndex));
                    }
                }
            }
            
            ////////////////////////////////////////
            // AI ALLY //
            ////////////////////////////////////////
            //the following code randomly chooses a move to use with no further thought.
            if (doubleBattle && ally != null)
            {
                if (pokemon[1] != null)
                {
                    //check if struggle is to be used (no PP left in any move)
                    if (pokemon[1].getPP(0) == 0 && pokemon[1].getPP(1) == 0 &&
                        pokemon[1].getPP(2) == 0 && pokemon[1].getPP(3) == 0)
                    {
                        commandMove[1] = MoveDatabase.getMove("Struggle");
                    }
                    else
                    {
                        //Randomly choose a move from the moveset
                        int AImoveIndex = Random.Range(0, 4);
                        while (pokemonMoveset[1] != null && string.IsNullOrEmpty(pokemonMoveset[1][AImoveIndex]) &&
                               pokemon[1].getPP(AImoveIndex) == 0)
                        {
                            AImoveIndex = Random.Range(0, 4);
                        }
                        command[1] = CommandType.Move;
                        commandMove[1] = MoveDatabase.getMove(pokemonMoveset[1][AImoveIndex]);
                        commandTarget[1] = Random.value > 0.5f ? 3 : 4;

                        Debug.Log(commandMove[1].Name + ", PP: " + pokemon[1].getPP(AImoveIndex));
                    }
                }
            }


            yield return new WaitForSeconds(0.3f);

            ////////////////////////////////////////
            /// Battle State
            ////////////////////////////////////////


            //for each pokemon on field, in order of speed/priority, run their command
            for (int i = 0; i < 6; i++)
            {
                if (running)
                {
                    //running may be set to false by a flee command
                    int movingPokemon = getHighestSpeedIndex();
                    
                    if (pokemon[movingPokemon] != null)
                    {
                        string commandTypeString = "";
                        switch (command[movingPokemon])
                        {
                            case CommandType.Flee:
                                commandTypeString = "Flee";
                                break;
                            case CommandType.Item:
                                commandTypeString = "Item";
                                break;
                            case CommandType.Move:
                                commandTypeString = "Move";
                                break;
                            case CommandType.Switch:
                                commandTypeString = "Switch";
                                break;
                        }
                        Debug.Log(movingPokemon + " Turn : " + commandTypeString);
                        
                        if (command[movingPokemon] == CommandType.Flee)
                        {
                            //RUN
                            if (movingPokemon < 3)
                            {
                                if (statDisplayAnim != null)
                                    StopCoroutine(statDisplayAnim);
                                
                                //player attemps escape
                                playerFleeAttempts += 1;

                                int fleeChance = (pokemon[movingPokemon].SPE * 128) / pokemon[3].SPE +
                                                 30 * playerFleeAttempts;
                                if (Random.Range(0, 256) < fleeChance)
                                {
                                    running = false;

                                    SfxHandler.Play(runClip);
                                    Dialog.DrawBlackFrame();
                                    yield return StartCoroutine(Dialog.DrawTextSilent("Got away safely!"));
                                    while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back"))
                                    {
                                        yield return null;
                                    }
                                    Dialog.UndrawDialogBox();
                                }
                                else
                                {
                                    yield return StartCoroutine(drawTextAndWait("Can't escape!"));
                                }
                            }

                            pokemonHasMoved[movingPokemon] = true;
                        }
                        else if (command[movingPokemon] == CommandType.Item)
                        {
                            //ITEM
                            //item effects not yet implemented fully

                            if (i < 3)
                            {
                                if (commandItem[movingPokemon].getItemEffect() == ItemData.ItemEffect.BALL)
                                {
                                    //debug autoselect targetIndex (target selection not yet implemented)
                                    int targetIndex = 3;
                                    //
                                    
                                    if (statDisplayAnim != null)
                                        StopCoroutine(statDisplayAnim);

                                    //pokeball animation not yet implemented
                                    yield return
                                        StartCoroutine(
                                            drawTextAndWait(
                                                SaveData.currentSave.playerName + " used one " +
                                                commandItem[movingPokemon].Name + "!", 2.4f));
                                    yield return new WaitForSeconds(1.2f);
                                    if (trainerBattle)
                                    {
                                        yield return
                                            StartCoroutine(drawTextAndWait("The trainer blocked the ball!", 2.4f));
                                        yield return StartCoroutine(drawTextAndWait("Don't be a theif!", 2.4f));
                                    }
                                    //calculate catch chance
                                    else
                                    {
                                        float ballRate = (float) commandItem[movingPokemon].getFloatParameter();
                                        float catchRate =
                                            (float)
                                            PokemonDatabase.getPokemon(pokemon[targetIndex].Species).getCatchRate();
                                        float statusRate = 1f;
                                        if ((pokemon[targetIndex].Status != Status.NONE))
                                        {
                                            statusRate = (pokemon[targetIndex].Status == Status.ASLEEP ||
                                                          pokemon[targetIndex].Status == Status.FROZEN)
                                                ? 2.5f
                                                : 1.5f;
                                        }

                                        int modifiedRate =
                                            Mathf.FloorToInt(((3 * (float) pokemon[targetIndex].TotalHP -
                                                               2 * (float) pokemon[targetIndex].HP)
                                                              * catchRate * ballRate) /
                                                             (3 * (float) pokemon[targetIndex].TotalHP) * statusRate);

                                        Debug.Log("modifiedRate: " + modifiedRate);

                                        //GEN VI
                                        //int shakeProbability = Mathf.FloorToInt(65536f / Mathf.Pow((255f/modifiedRate),0.1875f));
                                        //GEN V
                                        int shakeProbability =
                                            Mathf.FloorToInt(65536f / Mathf.Sqrt(Mathf.Sqrt(255f / modifiedRate)));

                                        int shakes = 0;

                                        string debugString = "";
                                        for (int shake = 0; shake < 4; shake++)
                                        {
                                            int shakeCheck = Random.Range(0, 65535);
                                            debugString += shake + ":(" + shakeCheck + "<" + shakeProbability + ")? ";
                                            if (shakeCheck < shakeProbability)
                                            {
                                                debugString += "Pass.   ";
                                                shakes += 1;
                                            }
                                            else
                                            {
                                                debugString += "Fail.   ";
                                                shake = 4;
                                            }
                                        }
                                        Debug.Log("(" + shakes + ")" + debugString);

                                        if (shakes == 4)
                                        {
                                            Debug.Log("Caught the " + pokemon[targetIndex].Name);
                                            running = false;

                                            //pokeball animation not yet implemented
                                            yield return StartCoroutine(faintPokemonAnimation(opponentSpriteRenderer));
                                            yield return new WaitForSeconds(1f);

                                            yield return
                                                StartCoroutine(
                                                    drawTextAndWait(
                                                        generatePreString(targetIndex) + pokemon[targetIndex].Name +
                                                        " \\nwas caught!", 2.4f));

                                            Dialog.DrawBlackFrame();
                                            yield return
                                                StartCoroutine(
                                                    Dialog.DrawTextSilent(
                                                        "Would you like to give a nickname \\nto your new " +
                                                        pokemon[targetIndex].Name + "?"));
                                            yield return StartCoroutine(Dialog.DrawChoiceBox());
                                            int chosenIndex = Dialog.chosenIndex;
                                            Dialog.UndrawDialogBox();
                                            Dialog.UndrawChoiceBox();

                                            string nickname = null;
                                            if (chosenIndex == 1)
                                            {
                                                //give nickname
                                                SfxHandler.Play(selectClip);
                                                yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));

                                                Scene.main.Typing.gameObject.SetActive(true);
                                                StartCoroutine(Scene.main.Typing.control(10, "",
                                                    pokemon[targetIndex].Gender, pokemon[targetIndex].GetIconsSprite()));
                                                while (Scene.main.Typing.gameObject.activeSelf)
                                                {
                                                    yield return null;
                                                }
                                                if (Scene.main.Typing.typedString.Length > 0)
                                                {
                                                    nickname = Scene.main.Typing.typedString;
                                                }

                                                yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));
                                            }
                                            Debug.Log("CurrentHP" + pokemon[targetIndex].HP);
                                            SaveData.currentSave.PC.addPokemon(new Pokemon(pokemon[targetIndex],
                                                nickname, commandItem[movingPokemon].Name));
                                        }

                                        Dialog.UndrawDialogBox();
                                    }
                                }
                                else if (commandItem[movingPokemon].getItemEffect() == ItemData.ItemEffect.HP)
                                {
                                    //commandTarget refers to the field position, healing a party member takes place before the turn.
                                    if (commandTarget[movingPokemon] < 3)
                                    {
                                        if (statDisplayAnim != null)
                                            StopCoroutine(statDisplayAnim);
                                        
                                        //if target is player
                                        yield return
                                            StartCoroutine(
                                                drawTextAndWait(
                                                    SaveData.currentSave.playerName + " used the " +
                                                    commandItem[movingPokemon].Name + "!", 2.4f));
                                        yield return
                                            StartCoroutine(Heal(commandTarget[movingPokemon],
                                                commandItem[movingPokemon].getFloatParameter()));

                                        UpdateLowHpPlay(movingPokemon, ally);
                                    }
                                }
                                else if (commandItem[movingPokemon].getItemEffect() == ItemData.ItemEffect.STATUS)
                                {
                                    //commandTarget refers to the field position, curing a party member takes place before the turn.
                                    if (commandTarget[movingPokemon] < 3)
                                    {
                                        if (statDisplayAnim != null)
                                            StopCoroutine(statDisplayAnim);
                                        
                                        //if target is player
                                        yield return
                                            StartCoroutine(
                                                drawTextAndWait(
                                                    SaveData.currentSave.playerName + " used the " +
                                                    commandItem[movingPokemon].Name + "!", 2.4f));
                                        yield return StartCoroutine(Heal(commandTarget[movingPokemon], true));
                                    }
                                }
                                else
                                {
                                    if (statDisplayAnim != null)
                                        StopCoroutine(statDisplayAnim);
                                    
                                    //undefined effect
                                    yield return
                                        StartCoroutine(
                                            drawTextAndWait(
                                                SaveData.currentSave.playerName + " used " +
                                                commandItem[movingPokemon].Name + "!", 2.4f));
                                }
                            }
                            else
                            {
                                if (statDisplayAnim != null)
                                    StopCoroutine(statDisplayAnim);
                                
                                yield return
                                    StartCoroutine(
                                        drawTextAndWait(
                                            opponentName + " used " + commandItem[movingPokemon].Name + "!", 2.4f))
                                    ;
                            }

                            pokemonHasMoved[movingPokemon] = true;
                        }
                        else if (command[movingPokemon] == CommandType.Move)
                        {
                            //MOVE
                            
                            // Replace camera
                            LeanTween.moveLocal(BattleCamera.gameObject, baseCameraAngle[0], 0.25f);
                            LeanTween.rotateLocal(BattleCamera.gameObject, baseCameraAngle[1], 0.25f);
                            
                            //debug autoselect targetIndex (target selection not yet implemented)
                            int targetIndex = commandTarget[movingPokemon];
                            Debug.Log(movingPokemon + " Turn : target = " + targetIndex);
                            if (commandMove[movingPokemon].getTarget() == MoveData.Target.SELF ||
                                commandMove[movingPokemon].getTarget() == MoveData.Target.ADJACENTALLYSELF)
                            {
                                targetIndex = movingPokemon;
                            }
                            //

                            if (pokemon[movingPokemon].Status != Status.FAINT)
                            {
                                // If target fainted change to adjacent target
                                if (pokemon[targetIndex] == null)
                                {
                                    if (targetIndex == 3)
                                    {
                                        targetIndex = 4;
                                    }
                                    else if (targetIndex == 4)
                                    {
                                        targetIndex = 3;
                                    }
                                    else if (targetIndex == 0)
                                    {
                                        if (movingPokemon < 3)
                                        {
                                            // Set target to inexistant if trying to hit an ally
                                            targetIndex = 2;
                                        }
                                        else
                                        {
                                            targetIndex = 1;
                                        }
                                    }
                                    else if (targetIndex == 1)
                                    {
                                        if (movingPokemon < 3)
                                        {
                                            // Set target to inexistant if trying to hit an ally
                                            targetIndex = 2;
                                        }
                                        else
                                        {
                                            targetIndex = 0;
                                        }
                                    }
                                }

                                // If (new) target fainted abort
                                if (pokemon[targetIndex] != null)
                                {
                                    //calculate and test accuracy
                                    float accuracy = commandMove[movingPokemon].getAccuracy() *
                                                     calculateAccuracyModifier(pokemonStatsMod[5][movingPokemon]) /
                                                     calculateAccuracyModifier(pokemonStatsMod[6][targetIndex]);
                                    bool canMove = true;
                                    if (pokemon[movingPokemon].Status == Status.PARALYSIS)
                                    {
                                        if (Random.value > 0.75f)
                                        {
                                            yield return
                                                StartCoroutine(
                                                    drawTextAndWait(
                                                        generatePreString(movingPokemon) + pokemon[movingPokemon].Name +
                                                        " is paralyzed! \\nIt can't move!", 2.4f));
                                            canMove = false;
                                        }
                                    }
                                    else if (pokemon[movingPokemon].Status == Status.FROZEN)
                                    {
                                        if (Random.value > 0.2f)
                                        {
                                            yield return
                                                StartCoroutine(
                                                    drawTextAndWait(
                                                        generatePreString(movingPokemon) + pokemon[movingPokemon].Name +
                                                        " is \\nfrozen solid!", 2.4f));
                                            canMove = false;
                                        }
                                        else
                                        {
                                            pokemon[movingPokemon].setStatus(Status.NONE);
                                            updatePokemonStatsDisplay(movingPokemon);
                                            yield return
                                                StartCoroutine(
                                                    drawTextAndWait(
                                                        generatePreString(movingPokemon) + pokemon[movingPokemon].Name +
                                                        " thawed out!", 2.4f));
                                        }
                                    }
                                    else if (pokemon[movingPokemon].Status == Status.SLEEP)
                                    {
                                        pokemon[movingPokemon].removeSleepTurn();
                                        if (pokemon[movingPokemon].Status == Status.ASLEEP)
                                        {
                                            yield return
                                                StartCoroutine(
                                                    drawTextAndWait(
                                                        generatePreString(movingPokemon) + pokemon[movingPokemon].Name +
                                                        " is \\nfast asleep.", 2.4f));
                                            canMove = false;
                                        }
                                        else
                                        {
                                            updatePokemonStatsDisplay(movingPokemon);
                                            yield return
                                                StartCoroutine(
                                                    drawTextAndWait(
                                                        generatePreString(movingPokemon) + pokemon[movingPokemon].Name +
                                                        " woke up!", 2.4f));
                                        }
                                    }
                                    if (canMove)
                                    {
                                        //use the move
                                        //deduct PP from the move
                                        if (movingPokemon > 2)
                                        {
                                            pokemon[movingPokemon].removePP(commandMove[movingPokemon].Name, pressurePlayer ? 2 : 1);
                                        }
                                        else
                                        {
                                            pokemon[movingPokemon].removePP(commandMove[movingPokemon].Name, pressureOpponent ? 2 : 1);
                                        }
                                        yield return
                                            StartCoroutine(
                                                drawTextAndWait(
                                                    generatePreString(movingPokemon) + pokemon[movingPokemon].Name +
                                                    " used " + commandMove[movingPokemon].Name + "!", 1.2f, 1.2f));
                                        
                                        Dialog.UndrawDialogBox();

                                        //adjust for accuracy
                                        if (accuracy != 0 && Random.value > accuracy)
                                        {
                                            Dialog.DrawBlackFrame();
                                            //if missed, provide missed feedback
                                            yield return
                                                StartCoroutine(
                                                    drawTextAndWait(
                                                        generatePreString(movingPokemon) + pokemon[movingPokemon].Name +
                                                        "'s attack missed!", 2.4f));
                                            canMove = false;
                                        }
                                    }
                                    if (canMove)
                                    {
                                        //if didn't miss
                                        //set up variables needed later
                                        float damageToDeal = 0;
                                        bool applyCritical = false;
                                        float superEffectiveModifier = -1;

                                        //check for move effects that change how damage is calculated (Heal / Set Damage / etc.) (not yet implemented fully)
                                        if (commandMove[movingPokemon].hasMoveEffect(MoveData.Effect.Heal))
                                        {
                                            yield return
                                                StartCoroutine(Heal(targetIndex,
                                                    commandMove[movingPokemon].getMoveParameter(MoveData.Effect.Heal)));
                                        }
                                        else if (commandMove[movingPokemon].hasMoveEffect(MoveData.Effect.SetDamage))
                                        {
                                            damageToDeal =
                                                commandMove[movingPokemon].getMoveParameter(MoveData.Effect.SetDamage);
                                            //if parameter is 0, then use the pokemon's level
                                            if (damageToDeal == 0)
                                            {
                                                damageToDeal = pokemon[movingPokemon].Level;
                                            }
                                            //check for any ineffectivity
                                            superEffectiveModifier =
                                                getSuperEffectiveModifier(commandMove[movingPokemon].getType(),
                                                    pokemonType1[targetIndex]) *
                                                getSuperEffectiveModifier(commandMove[movingPokemon].getType(),
                                                    pokemonType2[targetIndex]) *
                                                getSuperEffectiveModifier(commandMove[movingPokemon].getType(),
                                                    pokemonType3[targetIndex]);
                                            //if able to hit, set to 1 to prevent super effective messages appearing
                                            if (superEffectiveModifier > 0f)
                                            {
                                                superEffectiveModifier = 1f;
                                            }
                                        }
                                        else
                                        {
                                            //calculate damage
                                            damageToDeal = calculateDamage(movingPokemon, targetIndex,
                                                commandMove[movingPokemon]);
                                            applyCritical = calculateCritical(movingPokemon, targetIndex,
                                                commandMove[movingPokemon]);
                                            if (applyCritical)
                                            {
                                                damageToDeal *= 1.5f;
                                            }
                                            superEffectiveModifier =
                                                getSuperEffectiveModifier(commandMove[movingPokemon].getType(),
                                                    pokemonType1[targetIndex]) *
                                                getSuperEffectiveModifier(commandMove[movingPokemon].getType(),
                                                    pokemonType2[targetIndex]) *
                                                getSuperEffectiveModifier(commandMove[movingPokemon].getType(),
                                                    pokemonType3[targetIndex]);
                                            damageToDeal *= superEffectiveModifier;
                                            //apply offense/defense boosts.
                                            float damageBeforeMods = damageToDeal;
                                            if (commandMove[movingPokemon].getCategory() == MoveData.Category.PHYSICAL)
                                            {
                                                if (applyCritical)
                                                {
                                                    //if a critical lands
                                                    if (pokemonStatsMod[0][movingPokemon] > 0)
                                                    {
                                                        //only apply ATKmod if positive
                                                        damageToDeal *=
                                                            calculateStatModifier(pokemonStatsMod[0][movingPokemon]);
                                                    }
                                                    if (pokemonStatsMod[1][targetIndex] < 0)
                                                    {
                                                        //only apply DEFmod if negative
                                                        damageToDeal *=
                                                            calculateStatModifier(pokemonStatsMod[1][targetIndex]);
                                                    }
                                                }
                                                else
                                                {
                                                    //apply ATK and DEF mods normally (also half damage if burned)
                                                    damageToDeal *= calculateStatModifier(pokemonStatsMod[0][movingPokemon]);
                                                    damageToDeal /= calculateStatModifier(pokemonStatsMod[1][targetIndex]);
                                                    if (pokemon[movingPokemon].Status == Status.BURNED)
                                                    {
                                                        damageToDeal /= 2f;
                                                    }
                                                }
                                            }
                                            else if (commandMove[movingPokemon].getCategory() == MoveData.Category.SPECIAL)
                                            {
                                                if (applyCritical)
                                                {
                                                    //same as above, only using the Special varients
                                                    if (pokemonStatsMod[2][movingPokemon] > 0)
                                                    {
                                                        damageToDeal *=
                                                            calculateStatModifier(pokemonStatsMod[2][movingPokemon]);
                                                    }
                                                    if (pokemonStatsMod[3][targetIndex] < 0)
                                                    {
                                                        damageToDeal *=
                                                            calculateStatModifier(pokemonStatsMod[3][targetIndex]);
                                                    }
                                                }
                                                else
                                                {
                                                    damageToDeal *= calculateStatModifier(pokemonStatsMod[2][movingPokemon]);
                                                    damageToDeal /= calculateStatModifier(pokemonStatsMod[3][targetIndex]);
                                                }
                                            }
                                        }

                                        //inflict damage
                                        int DEBUG_beforeHP = pokemon[targetIndex].HP;
                                        pokemon[targetIndex].removeHP(damageToDeal);
                                        Debug.Log(DEBUG_beforeHP + " - " + damageToDeal + " = " +
                                                  pokemon[targetIndex].HP);

                                        
                                        Dialog.UndrawDialogBox();

                                        yield return new WaitForSeconds(0.2f);

                                        if (damageToDeal > 0)
                                        {
                                            if (superEffectiveModifier > 1.01f)
                                            {
                                                SfxHandler.Play(hitSuperClip);
                                            }
                                            else if (superEffectiveModifier < 0.99f)
                                            {
                                                SfxHandler.Play(hitPoorClip);
                                            }
                                            else
                                            {
                                                SfxHandler.Play(hitClip);
                                            }

                                            if (targetIndex == 0)
                                            {
                                                StartCoroutine(HitAnimation(playerSpriteRenderer));
                                            }
                                            else if (targetIndex == 1)
                                            {
                                                StartCoroutine(HitAnimation(player2SpriteRenderer));
                                            }
                                            else if (targetIndex == 3)
                                            {
                                                StartCoroutine(HitAnimation(opponentSpriteRenderer));
                                            }
                                            else if (targetIndex == 4)
                                            {
                                                StartCoroutine(HitAnimation(opponent2SpriteRenderer));
                                            }
                                        }

                                        if (targetIndex == 0)
                                        {
                                            //if player pokemon 0
                                            yield return
                                                StartCoroutine(stretchBar(statsHPBar[targetIndex],
                                                    Mathf.CeilToInt(pokemon[targetIndex].PercentHP * 51f), 60f, true,
                                                    pokemon0CurrentHP, pokemon0CurrentHPShadow,
                                                    pokemon[targetIndex].HP));
                                        }
                                        else if (targetIndex == 1)
                                        {
                                            //if player pokemon 1
                                            yield return
                                                StartCoroutine(stretchBar(statsHPBar[targetIndex],
                                                    Mathf.CeilToInt(pokemon[targetIndex].PercentHP * 51f), 60f, true,
                                                    pokemon1CurrentHP, pokemon1CurrentHPShadow,
                                                    pokemon[targetIndex].HP));
                                        }
                                        else
                                        {
                                            yield return
                                                StartCoroutine(stretchBar(statsHPBar[targetIndex],
                                                    Mathf.CeilToInt(pokemon[targetIndex].PercentHP * 51f), 60f, true,
                                                    null, null, 0));
                                        }

                                        yield return new WaitForSeconds(0.4f);

                                        updatePokemonStatsDisplay(targetIndex);

                                        //Feedback on the damage dealt
                                        if (superEffectiveModifier == 0 && commandMove[movingPokemon].Target != MoveData.Target.SELF && commandMove[movingPokemon].getPower() > 0)
                                        {
                                            yield return StartCoroutine(drawTextAndWait("It had no effect...", 2.4f));
                                        }
                                        else if (commandMove[movingPokemon].getCategory() != MoveData.Category.STATUS && commandMove[movingPokemon].getPower() > 0)
                                        {
                                            if (applyCritical)
                                            {
                                                yield return StartCoroutine(drawTextAndWait("A Critical Hit!", 2.4f));
                                            }
                                            if (superEffectiveModifier > 1)
                                            {
                                                yield return StartCoroutine(drawTextAndWait("It's Super Effective!", 2.4f));
                                            }
                                            else if (superEffectiveModifier < 1)
                                            {
                                                yield return
                                                    StartCoroutine(drawTextAndWait("It's not very effective.", 2.4f));
                                            }
                                        }
                                        
                                        // If move drains HP (like Absorb), heal the user
                                        if (commandMove[movingPokemon].hasMoveEffect(MoveData.Effect.HPDrain) && pokemon[movingPokemon].PercentHP < 1)
                                        {
                                            Dialog.UndrawDialogBox();
                                            
                                            if (damageToDeal > DEBUG_beforeHP && damageToDeal > 0)
                                            {
                                                Debug.Log("Heal : damageToDeal > DEBUG_beforeHP => " + DEBUG_beforeHP / 2);
                                                yield return Heal(movingPokemon, DEBUG_beforeHP / 2);
                                            }
                                            else
                                            {
                                                Debug.Log("Heal : DEBUG_beforeHP > damageToDeal => " + damageToDeal / 2);
                                                yield return Heal(movingPokemon, damageToDeal / 2);
                                            }
                                            
                                            UpdateLowHpPlay(movingPokemon, ally);
                                        }
                                        
                                        UpdateLowHpPlay(targetIndex, ally);

                                        //Faint the target if nessecary
                                        if (pokemon[targetIndex].Status == Status.FAINT)
                                        {
                                            a = AbilityDatabase.getAbility(PokemonDatabase.getPokemon(pokemon[targetIndex].Species).getAbility(pokemon[targetIndex].Ability));

                                            if (a != null)
                                            {
                                                yield return StartCoroutine(a.EffectOnFainted(this, targetIndex));
                                            }
                                            
                                            //debug = array of GUITextures not yet implemented

                                            Dialog.DrawBlackFrame();
                                            yield return
                                                StartCoroutine(
                                                    drawTextAndWait(
                                                        generatePreString(targetIndex) + pokemon[targetIndex].Name +
                                                        " fainted!", 2.4f));
                                            Dialog.UndrawDialogBox();
                                            yield return new WaitForSeconds(0.2f);
                                            yield return new WaitForSeconds(PlayCry(pokemon[targetIndex]));
                                            //flexible faint animtions not yet implemented
                                            if (targetIndex == 0)
                                            {
                                                StartCoroutine(slidePokemonStats(0, true, doubleBattle));
                                                yield return StartCoroutine(faintPokemonAnimation(playerSpriteRenderer));
                                            }
                                            else if (targetIndex == 1)
                                            {
                                                StartCoroutine(slidePokemonStats(1, true, doubleBattle));
                                                yield return StartCoroutine(faintPokemonAnimation(player2SpriteRenderer));
                                            }
                                            else if (targetIndex == 3)
                                            {
                                                StartCoroutine(slidePokemonStats(3, true, doubleBattle));
                                                yield return StartCoroutine(faintPokemonAnimation(opponentSpriteRenderer));
                                            }
                                            else if (targetIndex == 4)
                                            {
                                                StartCoroutine(slidePokemonStats(4, true, doubleBattle));
                                                yield return StartCoroutine(faintPokemonAnimation(opponent2SpriteRenderer));
                                            }

                                            //give EXP / add EXP
                                            if (targetIndex > 2 && (ally == null && movingPokemon < 3  || ally != null && movingPokemon == 0))
                                            {
                                                if (pokemon[movingPokemon].Status != Status.FAINT)
                                                {
                                                    float isWildMod = (trainerBattle) ? 1.5f : 1f;
                                                    float baseExpYield =
                                                        PokemonDatabase.getPokemon(pokemon[targetIndex].Species)
                                                            .getBaseExpYield();
                                                    float luckyEggMod = (pokemon[movingPokemon].Item == "Lucky Egg")
                                                        ? 1.5f
                                                        : 1f;
                                                    float OTMod = (pokemon[movingPokemon].getIDno() !=
                                                                   SaveData.currentSave.playerID)
                                                        ? 1.5f
                                                        : 1f;
                                                    float sharedMod = 0; //shared experience

                                                    for (int k = 0; k < expShare.Length; ++k)
                                                    {
                                                        if (expShare[k] && SaveData.currentSave.PC.boxes[0][k].Level < 100) sharedMod++;
                                                    }

                                                    if (sharedMod <= 0) sharedMod = 1;
                                                    
                                                    Debug.Log("Share Mod = "+sharedMod);
                                                    
                                                    float IVMod = 0.85f +
                                                                  (float)
                                                                  (pokemon[targetIndex].getIV_HP() +
                                                                   pokemon[targetIndex].getIV_ATK() +
                                                                   pokemon[targetIndex].getIV_DEF() +
                                                                   pokemon[targetIndex].getIV_SPA() +
                                                                   pokemon[targetIndex].getIV_SPD() +
                                                                   pokemon[targetIndex].getIV_SPE()) / 480f;
                                                    //IV Mod is unique to Pokemon Unity
                                                    int exp =
                                                        Mathf.CeilToInt((isWildMod * baseExpYield * IVMod * OTMod *
                                                                         luckyEggMod * pokemon[targetIndex].Level) 
                                                                        / (7 * sharedMod));

                                                    yield return StartCoroutine(addExp(movingPokemon, exp));

                                                    if (doubleBattle && ally == null)
                                                    {
                                                        if (pokemon[(movingPokemon == 0) ? 1 : 0] != null && pokemon[(movingPokemon == 0) ? 1 : 0].Status != Status.FAINT)
                                                            yield return StartCoroutine(addExp((movingPokemon == 0) ? 1 : 0, exp));
                                                        
                                                        expShare[expIndex] = false;
                                                        expShare[expIndex2] = false;
                                                    }
                                                    else
                                                    {
                                                        expShare[expIndex] = false;
                                                    }

                                                    for (int k = 0; k < expShare.Length; ++k)
                                                    {
                                                        if (expShare[k] && SaveData.currentSave.PC.boxes[0][k].Status != Status.FAINT)
                                                            yield return StartCoroutine(addExpHidden(k, exp));
                                                    }

                                                    if (doubleBattle && ally == null)
                                                    {
                                                        expShare[expIndex] = true;
                                                        expShare[expIndex2] = true;
                                                    }
                                                    else
                                                    {
                                                        expShare[expIndex] = false;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                expShare[expIndex] = false;
                                            }

                                            pokemon[targetIndex] = null;
                                        }

                                        // Move effects should not apply to those pokemon that are immune to that move. not yet implemented

                                        //apply move effects
                                        MoveData.Effect[] moveEffects = commandMove[movingPokemon].getMoveEffects();
                                        float[] moveEffectParameters = commandMove[movingPokemon].getMoveParameters();

                                        //track these and prevent multiple statUp/Down anims
                                        bool statUpRun = false;
                                        bool statDownRun = false;
                                        bool statUpSelfRun = false;
                                        bool statDownSelfRun = false;
                                        for (int i2 = 0; i2 < moveEffects.Length; i2++)
                                        {
                                            //Check for Chance effect. if failed, no further effects will run
                                            if (moveEffects[i2] == MoveData.Effect.Chance)
                                            {
                                                if (Random.value > moveEffectParameters[i2])
                                                {
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                //Check these booleans to prevent running an animation twice for one pokemon.
                                                bool animate = false;
                                                //check if statUp/Down Effect
                                                if (moveEffects[i2] == MoveData.Effect.ATK ||
                                                    moveEffects[i2] == MoveData.Effect.DEF ||
                                                    moveEffects[i2] == MoveData.Effect.SPA ||
                                                    moveEffects[i2] == MoveData.Effect.SPD ||
                                                    moveEffects[i2] == MoveData.Effect.SPE ||
                                                    moveEffects[i2] == MoveData.Effect.ACC ||
                                                    moveEffects[i2] == MoveData.Effect.EVA)
                                                {
                                                    //if statUp, and haven't run statUp yet, set statUpRun bool to true;
                                                    if (moveEffectParameters[i2] > 0 && !statUpRun)
                                                    {
                                                        statUpRun = true;
                                                        animate = true;
                                                    }
                                                    else if (moveEffectParameters[i2] < 0 && !statDownRun)
                                                    {
                                                        statDownRun = true;
                                                        animate = true;
                                                    }
                                                }
                                                //check if Self statUp/Down Effect
                                                else if (moveEffects[i2] == MoveData.Effect.ATKself ||
                                                         moveEffects[i2] == MoveData.Effect.DEFself ||
                                                         moveEffects[i2] == MoveData.Effect.SPAself ||
                                                         moveEffects[i2] == MoveData.Effect.SPDself ||
                                                         moveEffects[i2] == MoveData.Effect.SPEself ||
                                                         moveEffects[i2] == MoveData.Effect.ACCself ||
                                                         moveEffects[i2] == MoveData.Effect.EVAself)
                                                {
                                                    //if statUp, and haven't run statUp yet, set statUpRun bool to true;
                                                    if (moveEffectParameters[i2] > 0 && !statUpSelfRun)
                                                    {
                                                        statUpSelfRun = true;
                                                        animate = true;
                                                    }
                                                    else if (moveEffectParameters[i2] < 0 && !statDownSelfRun)
                                                    {
                                                        statDownSelfRun = true;
                                                        animate = true;
                                                    }
                                                }
                                                else
                                                {
                                                    animate = true;
                                                }
                                                
                                                yield return
                                                    StartCoroutine(applyEffect(movingPokemon, targetIndex, moveEffects[i2],
                                                        moveEffectParameters[i2], animate));
                                            }
                                        }

                                        updatePokemonStatsDisplay(targetIndex);
                                    }
                                }
                            }

                            pokemonHasMoved[movingPokemon] = true;
                        }
                        else if (command[movingPokemon] == CommandType.Switch)
                        {
                            //switch pokemon
                            //enemy switching not yet implemented
                            if (BattleTravelling != null)
                                StopCoroutine(BattleTravelling);
                            BattleCamera.StopTravelling();
                            if (statDisplayAnim != null)
                                StopCoroutine(statDisplayAnim);

                            yield return
                                StartCoroutine(drawTextAndWait(pokemon[movingPokemon].Name + ", come back!", 1.5f,
                                    1.5f));
                            Dialog.UndrawDialogBox();

                            StartCoroutine(slidePokemonStats(movingPokemon, true, doubleBattle));

                            if (movingPokemon == 0)
                            {
                                yield return StartCoroutine(withdrawPokemon(playerSpriteRenderer));
                            }
                            else if (movingPokemon == 1)
                            {
                                yield return StartCoroutine(withdrawPokemon(player2SpriteRenderer));
                            }
                            yield return new WaitForSeconds(0.5f);

                            switchPokemon(movingPokemon, commandPokemon[movingPokemon]);
                            
                            UpdateLowHpPlay(movingPokemon, ally);
                            
                            expShare[expIndex] = true;

                            yield return new WaitForSeconds(0.5f);
                            yield return
                                StartCoroutine(drawTextAndWait("Go! " + pokemon[movingPokemon].Name + "!", 1.5f,
                                    1.5f));
                            Dialog.UndrawDialogBox();

                            if (movingPokemon == 0)
                            {
                                //DEBUG
                                Debug.Log(pokemon[0].getLongID());
                                StopCoroutine(animatePlayer1);
                                StopCoroutine(animatePlayer1SR);
                                StopCoroutine(animateShadowPlayer1SR);
                                
                                Sprite[] anim = pokemon[0].GetBackAnim_();
                                animatePlayer1 = StartCoroutine(animatePokemon(player1, anim));
                                animatePlayer1SR = StartCoroutine(animatePokemonSpriteRenderer(playerSpriteRenderer, anim));
                                animateShadowPlayer1SR = StartCoroutine(animatePokemonSpriteRenderer(playerShadowSpriteRenderer, anim));
                                yield return new WaitForSeconds(0.2f);
                                updatePokemonStatsDisplay(movingPokemon);
                                yield return StartCoroutine(releasePokemon(playerSpriteRenderer, doubleBattle));
                                
                                a = AbilityDatabase.getAbility(PokemonDatabase.getPokemon(pokemon[0].Species).getAbility(pokemon[0].getAbility()));

                                if (a != null)
                                {
                                    yield return StartCoroutine(a.EffectOnSent(this, 0));
                                }
                                
                                yield return new WaitForSeconds(0.3f);
                                //yield return StartCoroutine(slidePokemonStats(0, false));
                            }
                            else if (movingPokemon == 1)
                            {
                                //DEBUG
                                Debug.Log(pokemon[1].getLongID());
                                StopCoroutine(animatePlayer2SR);
                                StopCoroutine(animateShadowPlayer2SR);
                                
                                Sprite[] anim = pokemon[1].GetBackAnim_();
                                animatePlayer2SR = StartCoroutine(animatePokemonSpriteRenderer(player2SpriteRenderer, anim));
                                animateShadowPlayer2SR = StartCoroutine(animatePokemonSpriteRenderer(player2ShadowSpriteRenderer, anim));
                                yield return new WaitForSeconds(0.2f);
                                updatePokemonStatsDisplay(1);
                                yield return StartCoroutine(releasePokemon(player2SpriteRenderer, doubleBattle));
                                
                                a = AbilityDatabase.getAbility(PokemonDatabase.getPokemon(pokemon[1].Species).getAbility(pokemon[1].getAbility()));

                                if (a != null)
                                {
                                    yield return StartCoroutine(a.EffectOnSent(this, 1));
                                }
                                
                                yield return new WaitForSeconds(0.3f);
                                //yield return StartCoroutine(slidePokemonStats(0, false));
                            }
                            pokemonHasMoved[movingPokemon] = true;
                        }
                    }
                    else
                    {
                        //count pokemon as moved as pokemon does not exist.
                        Debug.Log(movingPokemon + " Turn : is null");
                        pokemonHasMoved[movingPokemon] = true;
                    }
                }
            }


            ////////////////////////////////////////
            /// After-Effects State
            ////////////////////////////////////////

            if (running)
            {
                //running may be set to false by a successful flee
                //Apply after-moved effects
                for (int i = 0; i < 6; i++)
                {
                    if (pokemon[i] != null)
                    {
                        if (pokemon[i].Status == Status.BURNED ||
                            pokemon[i].Status == Status.POISONED)
                        {
                            pokemon[i].removeHP(Mathf.Floor((float) pokemon[i].TotalHP / 8f));
                            if (pokemon[i].Status == Status.BURNED)
                            {
                                yield return
                                    StartCoroutine(
                                        drawTextAndWait(
                                            generatePreString(i) + pokemon[i].Name + " is hurt by its burn!", 2.4f))
                                    ;
                            }
                            else if (pokemon[i].Status == Status.POISONED)
                            {
                                yield return
                                    StartCoroutine(
                                        drawTextAndWait(
                                            generatePreString(i) + pokemon[i].Name + " is hurt by poison!", 2.4f));
                            }

                            SfxHandler.Play(hitClip);

                            if (i == 0)
                            {
                                //if player pokemon 0 (only stats bar to display HP text)
                                yield return
                                    StartCoroutine(stretchBar(statsHPBar[i],
                                        Mathf.CeilToInt(pokemon[i].getPercentHP() * 51f), 51f, true, pokemon0CurrentHP,
                                        pokemon0CurrentHPShadow, pokemon[i].HP));
                            }
                            else
                            {
                                yield return
                                    StartCoroutine(stretchBar(statsHPBar[i],
                                        Mathf.CeilToInt(pokemon[i].getPercentHP() * 51f), 51f, true, null, null, 0));
                            }
                            
                            UpdateLowHpPlay(i, ally);
                            
                            yield return new WaitForSeconds(1.2f);

                            Dialog.UndrawDialogBox();
                            updatePokemonStatsDisplay(i);
                        }

                        UpdateLowHpPlay(i, ally);

                        if (pokemon[i].Status == Status.FAINT)
                        {
                            //debug = array of GUITextures not yet implemented

                            yield return
                                StartCoroutine(drawTextAndWait(
                                    generatePreString(i) + pokemon[i].Name + " fainted!", 2.4f));
                            Dialog.UndrawDialogBox();
                            yield return new WaitForSeconds(0.2f);
                            yield return new WaitForSeconds(PlayCry(pokemon[i]));
                            //flexible faint animtions not yet implemented
                            StartCoroutine(slidePokemonStats(i, true, doubleBattle));
                            if (i == 0)
                            {
                                expShare[expIndex] = false;
                                yield return StartCoroutine(faintPokemonAnimation(playerSpriteRenderer));
                            }
                            else if (i == 1)
                            {
                                expShare[expIndex] = false;
                                yield return StartCoroutine(faintPokemonAnimation(player2SpriteRenderer));
                            }
                            else if (i == 3)
                            {
                                yield return StartCoroutine(faintPokemonAnimation(opponentSpriteRenderer));
                            }
                            else if (i == 4)
                            {
                                yield return StartCoroutine(faintPokemonAnimation(opponent2SpriteRenderer));
                            }
                            pokemon[i] = null;
                        }
                    }
                }
            }


            ////////////////////////////////////////
            /// Replacement State
            ////////////////////////////////////////

            if (running)
            {
                //check if any opponents are left
                bool allOpponentsDefeated = true;
                if (doubleBattle && trainer2 != null) // Double battle (vs duo trainers)
                {
                    for (int i = 0; i < opponentParty.Length && i < 3; i++)
                    {
                        //check each opponent
                        if (opponentParty[i].Status != Status.FAINT)
                        {
                            allOpponentsDefeated = false;
                        }
                    } 
                    for (int i = 0; i < opponent2Party.Length && i < 3; i++)
                    {
                        //check each opponent
                        if (opponent2Party[i].Status != Status.FAINT)
                        {
                            allOpponentsDefeated = false;
                        }
                    } 
                }
                else  // Solo battle or double battle (vs one trainer)
                {
                    for (int i = 0; i < opponentParty.Length; i++)
                    {
                        //check each opponent
                        if (opponentParty[i].Status != Status.FAINT)
                        {
                            allOpponentsDefeated = false;
                        }
                    }
                }
                
                //check if any player Pokemon are left
                bool allPlayersDefeated = true;
                for (int i = 0; i < 6; i++)
                {
                    //check each player
                    if (SaveData.currentSave.PC.boxes[0][i] != null)
                    {
                        if (SaveData.currentSave.PC.boxes[0][i].Status != Status.FAINT)
                        {
                            allPlayersDefeated = false;
                        }
                    }
                }

                //if both sides have Pokemon left
                if (!allOpponentsDefeated && !allPlayersDefeated)
                {
                    //replace fainted Opponent Pokemon (switch/set not yet implemented)
                    for (int i = 0; i < pokemonPerSide; i++)
                    {
                        if (!doubleBattle || doubleBattle && trainer2 == null)
                        {
                            //replace each opponent
                            if (pokemon[i + 3] == null)
                            {
                                //select the first able pokemon
                                for (int i2 = 0; i2 < opponentParty.Length; i2++)
                                {
                                    if (opponentParty[i2].Status != Status.FAINT)
                                    {
                                        //check that pokemon is not on the field
                                        bool notOnField = true;
                                        for (int i3 = 0; i3 < pokemonPerSide; i3++)
                                        {
                                            //flexible faint animtions not yet implemented
                                            if (opponentParty[i2] == pokemon[i3 + 3])
                                            {
                                                notOnField = false;
                                                i3 = pokemonPerSide;
                                            }
                                        }

                                        if (notOnField)
                                        {
                                            switchPokemon(i + 3, opponentParty[i2]);

                                            yield return
                                                StartCoroutine(
                                                    drawTextAndWait(
                                                        opponentName + " sent out " + pokemon[i + 3].Name + "!",
                                                        1.5f,
                                                        1.5f));
                                            Dialog.UndrawDialogBox();
                                            if (i == 0)
                                            {
                                                //DEBUG
                                                Debug.Log(pokemon[3].getLongID());
                                                StopCoroutine(animateOpponent1);
                                                StopCoroutine(animateOpponent1SR);
                                                StopCoroutine(animateShadowOpponent1SR);

                                                Sprite[] anim = pokemon[3].GetFrontAnim_();
                                                animateOpponent1 =
                                                    StartCoroutine(animatePokemon(opponent1, anim));
                                                animateOpponent1SR =
                                                    StartCoroutine(animatePokemonSpriteRenderer(opponentSpriteRenderer,
                                                        anim));
                                                animateShadowOpponent1SR =
                                                    StartCoroutine(
                                                        animatePokemonSpriteRenderer(opponentShadowSpriteRenderer,
                                                            anim));
                                                yield return new WaitForSeconds(0.2f);
                                                updatePokemonStatsDisplay(i + 3);

                                                yield return StartCoroutine(opponentBallAnim(3));

                                                yield return StartCoroutine(releasePokemon(opponentSpriteRenderer,
                                                    doubleBattle));

                                                a = AbilityDatabase.getAbility(PokemonDatabase
                                                    .getPokemon(pokemon[i + 3].Species)
                                                    .getAbility(pokemon[i + 3].getAbility()));

                                                if (a != null)
                                                {
                                                    yield return StartCoroutine(a.EffectOnSent(this, i + 3));
                                                }
                                            }
                                            else if (i == 0)
                                            {
                                                //DEBUG
                                                Debug.Log(pokemon[3].getLongID());
                                                StopCoroutine(animateOpponent1);
                                                StopCoroutine(animateOpponent1SR);
                                                StopCoroutine(animateShadowOpponent1SR);

                                                Sprite[] anim = pokemon[3].GetFrontAnim_();
                                                animateOpponent1 =
                                                    StartCoroutine(animatePokemon(opponent1, anim));
                                                animateOpponent1SR =
                                                    StartCoroutine(animatePokemonSpriteRenderer(opponentSpriteRenderer,
                                                        anim));
                                                animateShadowOpponent1SR =
                                                    StartCoroutine(
                                                        animatePokemonSpriteRenderer(opponentShadowSpriteRenderer,
                                                            anim));
                                                yield return new WaitForSeconds(0.2f);
                                                updatePokemonStatsDisplay(i + 3);

                                                yield return StartCoroutine(opponentBallAnim(3));

                                                yield return StartCoroutine(releasePokemon(opponentSpriteRenderer,
                                                    doubleBattle));

                                                a = AbilityDatabase.getAbility(PokemonDatabase
                                                    .getPokemon(pokemon[i + 3].Species)
                                                    .getAbility(pokemon[i + 3].getAbility()));

                                                if (a != null)
                                                {
                                                    yield return StartCoroutine(a.EffectOnSent(this, i + 3));
                                                }
                                            }
                                            else if (i == 1)
                                            {
                                                //DEBUG
                                                Debug.Log(pokemon[4].getLongID());
                                                StopCoroutine(animateOpponent2SR);
                                                StopCoroutine(animateShadowOpponent2SR);

                                                Sprite[] anim = pokemon[4].GetFrontAnim_();
                                                animateOpponent2SR =
                                                    StartCoroutine(animatePokemonSpriteRenderer(opponent2SpriteRenderer,
                                                        anim));
                                                animateShadowOpponent2SR =
                                                    StartCoroutine(
                                                        animatePokemonSpriteRenderer(opponent2ShadowSpriteRenderer,
                                                            anim));
                                                yield return new WaitForSeconds(0.2f);
                                                updatePokemonStatsDisplay(i + 3);

                                                yield return StartCoroutine(opponentBallAnim(4));

                                                yield return StartCoroutine(releasePokemon(opponent2SpriteRenderer,
                                                    doubleBattle));

                                                a = AbilityDatabase.getAbility(PokemonDatabase
                                                    .getPokemon(pokemon[i + 3].Species)
                                                    .getAbility(pokemon[i + 3].getAbility()));

                                                if (a != null)
                                                {
                                                    yield return StartCoroutine(a.EffectOnSent(this, i + 3));
                                                }
                                            }

                                            i = pokemonPerSide;
                                            i2 = opponentParty.Length;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            Pokemon[] party = (i == 0) ? opponentParty : opponent2Party;

                            //replace each opponent
                            if (pokemon[i + 3] == null)
                            {
                                //select the first able pokemon
                                for (int i2 = 0; i2 < party.Length && i2 < 3; i2++)
                                {
                                    if (party[i2].Status != Status.FAINT)
                                    {
                                        //check that pokemon is not on the field
                                        bool notOnField = true;
                                        for (int i3 = 0; i3 < pokemonPerSide; i3++)
                                        {
                                            //flexible faint animtions not yet implemented
                                            if (party[i2] == pokemon[i3 + 3])
                                            {
                                                notOnField = false;
                                                i3 = pokemonPerSide;
                                            }
                                        }

                                        if (notOnField)
                                        {
                                            switchPokemon(i + 3, party[i2]);

                                            yield return
                                                StartCoroutine(
                                                    drawTextAndWait(
                                                        ((i == 0) ? opponentName : opponent2Name) + " sent out " + pokemon[i + 3].Name + "!",
                                                        1.5f,
                                                        1.5f));
                                            Dialog.UndrawDialogBox();
                                            if (i == 0)
                                            {
                                                //DEBUG
                                                Debug.Log(pokemon[3].getLongID());
                                                StopCoroutine(animateOpponent1);
                                                StopCoroutine(animateOpponent1SR);
                                                StopCoroutine(animateShadowOpponent1SR);

                                                Sprite[] anim = pokemon[3].GetFrontAnim_();
                                                animateOpponent1 =
                                                    StartCoroutine(animatePokemon(opponent1, anim));
                                                animateOpponent1SR =
                                                    StartCoroutine(animatePokemonSpriteRenderer(opponentSpriteRenderer,
                                                        anim));
                                                animateShadowOpponent1SR =
                                                    StartCoroutine(
                                                        animatePokemonSpriteRenderer(opponentShadowSpriteRenderer,
                                                            anim));
                                                yield return new WaitForSeconds(0.2f);
                                                updatePokemonStatsDisplay(i + 3);

                                                yield return StartCoroutine(opponentBallAnim(3));

                                                yield return StartCoroutine(releasePokemon(opponentSpriteRenderer,
                                                    doubleBattle));

                                                a = AbilityDatabase.getAbility(PokemonDatabase
                                                    .getPokemon(pokemon[i + 3].Species)
                                                    .getAbility(pokemon[i + 3].getAbility()));

                                                if (a != null)
                                                {
                                                    yield return StartCoroutine(a.EffectOnSent(this, i + 3));
                                                }
                                            }
                                            else
                                            {
                                                //DEBUG
                                                Debug.Log(pokemon[4].getLongID());
                                                StopCoroutine(animateOpponent2SR);
                                                StopCoroutine(animateShadowOpponent2SR);

                                                Sprite[] anim = pokemon[4].GetFrontAnim_();
                                                animateOpponent2SR =
                                                    StartCoroutine(animatePokemonSpriteRenderer(opponent2SpriteRenderer,
                                                        anim));
                                                animateShadowOpponent2SR =
                                                    StartCoroutine(
                                                        animatePokemonSpriteRenderer(opponent2ShadowSpriteRenderer,
                                                            anim));
                                                yield return new WaitForSeconds(0.2f);
                                                updatePokemonStatsDisplay(i + 3);

                                                yield return StartCoroutine(opponentBallAnim(4));

                                                yield return StartCoroutine(releasePokemon(opponent2SpriteRenderer,
                                                    doubleBattle));

                                                a = AbilityDatabase.getAbility(PokemonDatabase
                                                    .getPokemon(pokemon[i + 3].Species)
                                                    .getAbility(pokemon[i + 3].getAbility()));

                                                if (a != null)
                                                {
                                                    yield return StartCoroutine(a.EffectOnSent(this, i + 3));
                                                }
                                            }

                                            i = pokemonPerSide;
                                            i2 = opponentParty.Length;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //replace fainted Player Pokemon
                    for (int i = 0; i < pokemonPerSide; i++)
                    {
                        //replace each player
                        if (pokemon[i] == null && (ally != null && i == 0 && GetUsablePokemon(ally, doubleBattle) > 0
                            || ally == null && GetUsablePokemon(ally, doubleBattle) > 1))
                        {
                            Dialog.UndrawDialogBox();

                            updateCurrentTask(3);
                            updateSelectedPokemonSlot(pokePartyPosition, false);
                            yield return new WaitForSeconds(0.2f);
                            while (currentTask == 3)
                            {
                                if (UnityEngine.Input.GetAxisRaw("Vertical") < 0)
                                {
                                    if (pokePartyPosition < 5)
                                    {
                                        int positionBeforeModification = pokePartyPosition;
                                        if (pokePartyPosition == 4)
                                        {
                                            updateSelectedPokemonSlot(pokePartyPosition + 1, false);
                                        }
                                        else
                                        {
                                            updateSelectedPokemonSlot(pokePartyPosition + 2, false);
                                        }
                                        if (positionBeforeModification != pokePartyPosition)
                                        {
                                            SfxHandler.Play(scrollClip);
                                            yield return new WaitForSeconds(0.2f);
                                        }
                                    }
                                }
                                else if (UnityEngine.Input.GetAxisRaw("Horizontal") > 0)
                                {
                                    if (pokePartyPosition < 6)
                                    {
                                        int positionBeforeModification = pokePartyPosition;
                                        updateSelectedPokemonSlot(pokePartyPosition + 1, false);
                                        if (positionBeforeModification != pokePartyPosition)
                                        {
                                            SfxHandler.Play(scrollClip);
                                            yield return new WaitForSeconds(0.2f);
                                        }
                                    }
                                }
                                else if (UnityEngine.Input.GetAxisRaw("Horizontal") < 0)
                                {
                                    if (pokePartyPosition > 0)
                                    {
                                        int positionBeforeModification = pokePartyPosition;
                                        updateSelectedPokemonSlot(pokePartyPosition - 1, false);
                                        if (positionBeforeModification != pokePartyPosition)
                                        {
                                            SfxHandler.Play(scrollClip);
                                            yield return new WaitForSeconds(0.2f);
                                        }
                                    }
                                }
                                else if (UnityEngine.Input.GetAxisRaw("Vertical") > 0)
                                {
                                    if (pokePartyPosition > 1)
                                    {
                                        int positionBeforeModification = pokePartyPosition;
                                        if (pokePartyPosition == 6)
                                        {
                                            updateSelectedPokemonSlot(pokePartyPosition - 1, false);
                                        }
                                        else
                                        {
                                            updateSelectedPokemonSlot(pokePartyPosition - 2, false);
                                        }
                                        if (positionBeforeModification != pokePartyPosition)
                                        {
                                            SfxHandler.Play(scrollClip);
                                            yield return new WaitForSeconds(0.2f);
                                        }
                                    }
                                }
                                else if (UnityEngine.Input.GetButtonDown("Select"))
                                {
                                    if (pokePartyPosition == 6)
                                    {
                                    } //debug
                                    else if (SaveData.currentSave.PC.boxes[0][pokePartyPosition] != null)
                                    {
                                        updateCurrentTask(5);
                                        SfxHandler.Play(selectClip);
                                        int summaryPosition = updateSummaryPosition(0);
                                            //0 = Switch, 1 = Moves, 2 = Back

                                        yield return new WaitForSeconds(0.2f);
                                        while (currentTask == 5)
                                        {
                                            if (UnityEngine.Input.GetAxisRaw("Vertical") < 0)
                                            {
                                                if (pokePartyPosition < 5)
                                                {
                                                    int positionBeforeModification = pokePartyPosition;
                                                    updateSelectedPokemonSlot(pokePartyPosition + 1, false);
                                                    if (positionBeforeModification != pokePartyPosition)
                                                    {
                                                        SfxHandler.Play(scrollClip);
                                                    }
                                                    updatePokemonSummaryDisplay(
                                                        SaveData.currentSave.PC.boxes[0][pokePartyPosition]);
                                                    yield return new WaitForSeconds(0.2f);
                                                }
                                            }
                                            else if (UnityEngine.Input.GetAxisRaw("Horizontal") > 0)
                                            {
                                                if (summaryPosition < 2)
                                                {
                                                    summaryPosition = updateSummaryPosition(summaryPosition + 1);
                                                    SfxHandler.Play(scrollClip);
                                                    yield return new WaitForSeconds(0.2f);
                                                }
                                            }
                                            else if (UnityEngine.Input.GetAxisRaw("Horizontal") < 0)
                                            {
                                                if (summaryPosition > 0)
                                                {
                                                    summaryPosition = updateSummaryPosition(summaryPosition - 1);
                                                    SfxHandler.Play(scrollClip);
                                                    yield return new WaitForSeconds(0.2f);
                                                }
                                            }
                                            else if (UnityEngine.Input.GetAxisRaw("Vertical") > 0)
                                            {
                                                if (pokePartyPosition > 0)
                                                {
                                                    int positionBeforeModification = pokePartyPosition;
                                                    updateSelectedPokemonSlot(pokePartyPosition - 1, false);
                                                    if (positionBeforeModification != pokePartyPosition)
                                                    {
                                                        SfxHandler.Play(scrollClip);
                                                    }
                                                    updatePokemonSummaryDisplay(
                                                        SaveData.currentSave.PC.boxes[0][pokePartyPosition]);
                                                    yield return new WaitForSeconds(0.2f);
                                                }
                                            }
                                            else if (UnityEngine.Input.GetButtonDown("Select"))
                                            {
                                                if (summaryPosition == 0)
                                                {
                                                    // switch
                                                    if (
                                                        SaveData.currentSave.PC.boxes[0][pokePartyPosition].Status !=
                                                        Status.FAINT)
                                                    {
                                                        //check that pokemon is not on the field
                                                        bool notOnField = true;
                                                        for (int i2 = 0; i2 < pokemonPerSide; i2++)
                                                        {
                                                            if (SaveData.currentSave.PC.boxes[0][pokePartyPosition] ==
                                                                pokemon[i2])
                                                            {
                                                                notOnField = false;
                                                                i2 = pokemonPerSide;
                                                            }
                                                        }
                                                        if (notOnField)
                                                        {
                                                            switchPokemon(i,
                                                                SaveData.currentSave.PC.boxes[0][pokePartyPosition]);
                                                            updateCurrentTask(-1);
                                                            SfxHandler.Play(selectClip);

                                                            yield return
                                                                StartCoroutine(
                                                                    drawTextAndWait(
                                                                        "Go! " + pokemon[i].Name + "!", 1.5f, 1.5f))
                                                                ;
                                                            Dialog.UndrawDialogBox();
                                                            if (i == 0)
                                                            {
                                                                //DEBUG
                                                                Debug.Log(pokemon[0].getLongID());
                                                                StopCoroutine(animatePlayer1);
                                                                StopCoroutine(animatePlayer1SR);
                                                                StopCoroutine(animateShadowPlayer1SR);
                                
                                                                Sprite[] anim = pokemon[0].GetBackAnim_();
                                                                animatePlayer1 =
                                                                    StartCoroutine(animatePokemon(player1,
                                                                        anim));
                                                                animatePlayer1SR = 
                                                                    StartCoroutine(animatePokemonSpriteRenderer(playerSpriteRenderer, anim));
                                                                animateShadowPlayer1SR = 
                                                                    StartCoroutine(animatePokemonSpriteRenderer(playerShadowSpriteRenderer, anim));
                                                                yield return new WaitForSeconds(0.2f);
                                                                updatePokemonStatsDisplay(0);
                                                                yield return StartCoroutine(releasePokemon(playerSpriteRenderer, doubleBattle));
                                                                yield return new WaitForSeconds(0.3f);
                                                                ;
                                                            }
                                                            else if (i == 1)
                                                            {
                                                                //DEBUG
                                                                Debug.Log(pokemon[1].getLongID());
                                                                StopCoroutine(animatePlayer2SR);
                                                                StopCoroutine(animateShadowPlayer2SR);
                                
                                                                Sprite[] anim = pokemon[1].GetBackAnim_();
                                                                animatePlayer2SR = 
                                                                    StartCoroutine(animatePokemonSpriteRenderer(player2SpriteRenderer, anim));
                                                                animateShadowPlayer2SR = 
                                                                    StartCoroutine(animatePokemonSpriteRenderer(player2ShadowSpriteRenderer, anim));
                                                                yield return new WaitForSeconds(0.2f);
                                                                updatePokemonStatsDisplay(1);
                                                                yield return StartCoroutine(releasePokemon(player2SpriteRenderer, doubleBattle));
                                                                yield return new WaitForSeconds(0.3f);
                                                                ;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            yield return
                                                                StartCoroutine(
                                                                    drawTextAndWait(
                                                                        SaveData.currentSave.PC.boxes[0][
                                                                            pokePartyPosition].Name +
                                                                        " is already fighting!"));
                                                            Dialog.UndrawDialogBox();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        yield return
                                                            StartCoroutine(
                                                                drawTextAndWait(
                                                                    SaveData.currentSave.PC.boxes[0][pokePartyPosition]
                                                                        .Name + " is unable to fight!"));
                                                        Dialog.UndrawDialogBox();
                                                    }
                                                }
                                                else if (summaryPosition == 1)
                                                {
//check moves
                                                    updateCurrentTask(6);
                                                    SfxHandler.Play(selectClip);
                                                    yield return new WaitForSeconds(0.2f);

                                                    int movesPosition = 5;
                                                        //0-3 = Moves, 4 = Switch, 5 = Summary, 6 = Back
                                                    while (currentTask == 6)
                                                    {
                                                        if (UnityEngine.Input.GetAxisRaw("Vertical") < 0)
                                                        {
                                                            if (movesPosition < 4)
                                                            {
                                                                if (movesPosition == 2)
                                                                {
                                                                    movesPosition =
                                                                        updateMovesPosition(movesPosition + 3);
                                                                }
                                                                else
                                                                {
                                                                    movesPosition =
                                                                        updateMovesPosition(movesPosition + 2);
                                                                }
                                                                SfxHandler.Play(scrollClip);
                                                                yield return new WaitForSeconds(0.2f);
                                                            }
                                                        }
                                                        else if (UnityEngine.Input.GetAxisRaw("Horizontal") > 0)
                                                        {
                                                            if (movesPosition != 1 || movesPosition != 3 ||
                                                                movesPosition != 6)
                                                            {
                                                                movesPosition = updateMovesPosition(movesPosition + 1);
                                                                SfxHandler.Play(scrollClip);
                                                                yield return new WaitForSeconds(0.2f);
                                                            }
                                                        }
                                                        else if (UnityEngine.Input.GetAxisRaw("Horizontal") < 0)
                                                        {
                                                            if (movesPosition == 1 || movesPosition == 3 ||
                                                                movesPosition > 4)
                                                            {
                                                                movesPosition = updateMovesPosition(movesPosition - 1);
                                                                SfxHandler.Play(scrollClip);
                                                                yield return new WaitForSeconds(0.2f);
                                                            }
                                                        }
                                                        else if (UnityEngine.Input.GetAxisRaw("Vertical") > 0)
                                                        {
                                                            if (movesPosition > 1)
                                                            {
                                                                if (movesPosition > 3)
                                                                {
                                                                    movesPosition = updateMovesPosition(2);
                                                                }
                                                                else
                                                                {
                                                                    movesPosition =
                                                                        updateMovesPosition(movesPosition - 2);
                                                                }
                                                                SfxHandler.Play(scrollClip);
                                                                yield return new WaitForSeconds(0.2f);
                                                            }
                                                        }
                                                        else if (UnityEngine.Input.GetButtonDown("Select"))
                                                        {
                                                            if (movesPosition == 4)
                                                            {
                                                                // switch
                                                                if (
                                                                    SaveData.currentSave.PC.boxes[0][pokePartyPosition]
                                                                        .Status != Status.FAINT)
                                                                {
                                                                    //check that pokemon is not on the field
                                                                    bool notOnField = true;
                                                                    for (int i2 = 0; i2 < pokemonPerSide; i2++)
                                                                    {
                                                                        if (
                                                                            SaveData.currentSave.PC.boxes[0][
                                                                                pokePartyPosition] == pokemon[i2])
                                                                        {
                                                                            notOnField = false;
                                                                            i2 = pokemonPerSide;
                                                                        }
                                                                    }
                                                                    if (notOnField)
                                                                    {
                                                                        switchPokemon(i,
                                                                            SaveData.currentSave.PC.boxes[0][
                                                                                pokePartyPosition]);
                                                                        updateCurrentTask(-1);
                                                                        SfxHandler.Play(selectClip);

                                                                        yield return
                                                                            StartCoroutine(
                                                                                drawTextAndWait(
                                                                                    SaveData.currentSave.playerName +
                                                                                    " sent out " + pokemon[i].Name +
                                                                                    "!", 1.5f, 0.75f));
                                                                        Dialog.UndrawDialogBox();
                                                                        if (i == 0)
                                                                        {
                                                                            //DEBUG
                                                                            Debug.Log(pokemon[0].getLongID());
                                                                            StopCoroutine(animatePlayer1);
                                                                            StopCoroutine(animatePlayer1SR);
                                                                            StopCoroutine(animateShadowPlayer1SR);

                                                                            Sprite[] anim = pokemon[0].GetBackAnim_();
                                                                            animatePlayer1 =
                                                                                StartCoroutine(animatePokemon(player1,
                                                                                    anim));
                                                                                StartCoroutine(animatePokemonSpriteRenderer(playerSpriteRenderer,
                                                                                    anim));
                                                                                StartCoroutine(animatePokemonSpriteRenderer(playerShadowSpriteRenderer,
                                                                                    anim));
                                                                            yield return new WaitForSeconds(0.2f);
                                                                            updatePokemonStatsDisplay(i);
                                                                            yield return
                                                                                StartCoroutine(releasePokemon(playerSpriteRenderer, doubleBattle));
                                                                            yield return new WaitForSeconds(0.3f);
                                                                            yield return
                                                                                StartCoroutine(slidePokemonStats(0,
                                                                                    false, doubleBattle));
                                                                            
                                                                            a = AbilityDatabase.getAbility(PokemonDatabase.getPokemon(pokemon[i].Species).getAbility(pokemon[i].getAbility()));

                                                                            if (a != null)
                                                                            {
                                                                                yield return StartCoroutine(a.EffectOnSent(this, i));
                                                                            }
                                                                        }
                                                                        else if (i == 1)
                                                                        {
                                                                            //DEBUG
                                                                            Debug.Log(pokemon[1].getLongID());
                                                                            StopCoroutine(animatePlayer2SR);
                                                                            StopCoroutine(animateShadowPlayer2SR);

                                                                            Sprite[] anim = pokemon[1].GetBackAnim_();
                                                                            StartCoroutine(animatePokemonSpriteRenderer(player2SpriteRenderer,
                                                                                    anim));
                                                                                StartCoroutine(animatePokemonSpriteRenderer(player2ShadowSpriteRenderer,
                                                                                    anim));
                                                                            yield return new WaitForSeconds(0.2f);
                                                                            updatePokemonStatsDisplay(i);
                                                                            yield return
                                                                                StartCoroutine(releasePokemon(player2SpriteRenderer, doubleBattle));
                                                                            yield return new WaitForSeconds(0.3f);
                                                                            yield return
                                                                                StartCoroutine(slidePokemonStats(1,
                                                                                    false, doubleBattle));
                                                                            
                                                                            a = AbilityDatabase.getAbility(PokemonDatabase.getPokemon(pokemon[i].Species).getAbility(pokemon[i].getAbility()));

                                                                            if (a != null)
                                                                            {
                                                                                yield return StartCoroutine(a.EffectOnSent(this, i));
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        yield return
                                                                            StartCoroutine(
                                                                                drawTextAndWait(
                                                                                    SaveData.currentSave.PC.boxes[0][
                                                                                        pokePartyPosition].Name +
                                                                                    " is already fighting!"));
                                                                        Dialog.UndrawDialogBox();
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    yield return
                                                                        StartCoroutine(
                                                                            drawTextAndWait(
                                                                                SaveData.currentSave.PC.boxes[0][
                                                                                    pokePartyPosition].Name +
                                                                                " is unable to fight!"));
                                                                    Dialog.UndrawDialogBox();
                                                                }
                                                            }
                                                            else if (movesPosition == 5)
                                                            {
//check summary
                                                                updateCurrentTask(5);
                                                                SfxHandler.Play(selectClip);
                                                                yield return new WaitForSeconds(0.2f);
                                                            }
                                                            else if (movesPosition == 6)
                                                            {
//back
                                                                updateCurrentTask(3);
                                                                SfxHandler.Play(selectClip);
                                                                yield return new WaitForSeconds(0.2f);
                                                            }
                                                        }
                                                        else if (UnityEngine.Input.GetButtonDown("Back"))
                                                        {
                                                            updateCurrentTask(3);
                                                            SfxHandler.Play(selectClip);
                                                            yield return new WaitForSeconds(0.2f);
                                                        }

                                                        yield return null;
                                                    }
                                                }
                                                else if (summaryPosition == 2)
                                                {
//back
                                                    updateCurrentTask(3);
                                                    SfxHandler.Play(selectClip);
                                                    yield return new WaitForSeconds(0.2f);
                                                }
                                            }
                                            else if (UnityEngine.Input.GetButtonDown("Back"))
                                            {
                                                updateCurrentTask(3);
                                                SfxHandler.Play(selectClip);
                                                yield return new WaitForSeconds(0.2f);
                                            }

                                            yield return null;
                                        }
                                    }
                                }
                                yield return null;
                            }
                            updateCurrentTask(-1);
                        }
                        else if (i == 1 && pokemon[i] == null && ally != null)
                        {
                            //select the first able pokemon
                            for (int i2 = 0; i2 < allyParty.Length; i2++)
                            {
                                if (allyParty[i2].Status != Status.FAINT)
                                {
                                    //check that pokemon is not on the field
                                    bool notOnField = !(allyParty[i2] == pokemon[1]);

                                    if (notOnField)
                                    {
                                        switchPokemon(1, allyParty[i2]);

                                        yield return
                                            StartCoroutine(
                                                drawTextAndWait(
                                                    allyName + " sent out " + pokemon[1].Name + "!",
                                                    1.5f,
                                                    1));
                                        Dialog.UndrawDialogBox();
                                        //DEBUG
                                        Debug.Log(pokemon[1].getLongID());
                                        StopCoroutine(animatePlayer2SR);
                                        StopCoroutine(animateShadowPlayer2SR);

                                        Sprite[] anim = pokemon[1].GetBackAnim_();
                                        animatePlayer2SR =
                                            StartCoroutine(animatePokemonSpriteRenderer(player2SpriteRenderer,
                                                anim));
                                        animateShadowPlayer2SR =
                                            StartCoroutine(
                                                animatePokemonSpriteRenderer(player2ShadowSpriteRenderer,
                                                    anim));
                                        yield return new WaitForSeconds(0.2f);
                                        updatePokemonStatsDisplay(1);

                                        yield return StartCoroutine(releasePokemon(player2SpriteRenderer,
                                            doubleBattle));

                                        a = AbilityDatabase.getAbility(PokemonDatabase
                                            .getPokemon(pokemon[1].Species)
                                            .getAbility(pokemon[1].getAbility()));

                                        if (a != null)
                                        {
                                            yield return StartCoroutine(a.EffectOnSent(this, 1));
                                        }

                                        break;
                                    }
                                }
                            }
                        }
                    }
                }


                ////////////////////////////////////////
                /// End-Check
                ////////////////////////////////////////

                if (allPlayersDefeated)
                {
                    victor = 1;
                }
                else if (allOpponentsDefeated)
                {
                    victor = 0;
                }


                if (victor == 0)
                {
                    if (trainerBattle)
                    {
                        if (trainer.victoryBGM == null)
                        {
                            BgmHandler.main.PlayOverlay(defaultTrainerVictoryBGM, defaultTrainerVictoryBGMLoopStart);
                        }
                        else
                        {
                            BgmHandler.main.PlayOverlay(trainer.victoryBGM, trainer.victorySamplesLoopStart);
                        }

                        if (doubleBattle && trainer2 != null)
                        {
                            yield return
                                StartCoroutine(
                                    drawTextAndWait(SaveData.currentSave.playerName + " defeated\n" + opponentName + " and " + opponent2Name + "!",
                                        2.4f, 1.2f));
                        }
                        else
                        {
                            yield return
                                StartCoroutine(
                                    drawTextAndWait(SaveData.currentSave.playerName + " defeated\n" + opponentName + "!",
                                        2.4f, 1.2f));
                        }
                        
                        Dialog.UndrawDialogBox();
                        // TODO slide trainer
                        yield return StartCoroutine(slideTrainer(opponentBase, trainerSprite1, true, false));
                        for (int di = 0; di < trainer.en_playerVictoryDialog.Length; di++)
                        {
                            yield return StartCoroutine(drawTextAndWait(trainer.en_playerVictoryDialog[di]));
                        }
                        Dialog.UndrawDialogBox();

                        yield return
                            StartCoroutine(
                                drawTextAndWait(SaveData.currentSave.playerName + " received $" +
                                                trainer.GetPrizeMoney() + " for winning!"));
                        SaveData.currentSave.playerMoney += trainer.GetPrizeMoney();
                    }
                    else //wild battle
                    {
                        if (trainer.victoryBGM == null)
                        {
                            BgmHandler.main.PlayOverlay(defaultWildVictoryBGM, defaultWildVictoryBGMLoopStart);
                        }
                        else
                        {
                            BgmHandler.main.PlayOverlay(trainer.victoryBGM, trainer.victorySamplesLoopStart);
                        }

                        //TODO wild exp print out not yet implemented here
                    }

                    yield return new WaitForSeconds(0.2f);
                    running = false;
                }
                else if (victor == 1)
                {
                    if (trainerBattle)
                    {
                        yield return
                            StartCoroutine(
                                drawTextAndWait(opponentName + " defeated " + SaveData.currentSave.playerName + "!",
                                    2.4f, 2.4f));
                        Dialog.UndrawDialogBox();
                        yield return StartCoroutine(slideTrainer(opponentBase, trainerSprite1, true, false));
                        for (int di = 0; di < trainer.en_playerLossDialog.Length; di++)
                        {
                            yield return StartCoroutine(drawTextAndWait(trainer.en_playerLossDialog[di]));
                        }
                        Dialog.UndrawDialogBox();

                        StartCoroutine(ScreenFade.main.Fade(false, 1f));
                    }
                    else //wild battle
                    {
                        yield return
                            StartCoroutine(
                                drawTextAndWait(SaveData.currentSave.playerName + " is out of usable Pokémon!", 2f));
                        yield return
                            StartCoroutine(drawTextAndWait(SaveData.currentSave.playerName + " dropped $200 in panic!",
                                2f));
                        yield return StartCoroutine(drawTextAndWait("... ... ... ...", 2f));

                        yield return
                            StartCoroutine(drawTextAndWait(SaveData.currentSave.playerName + " blacked out!", 1.8f, 1.8f))
                            ;
                        Dialog.UndrawDialogBox();
                        //overlayed dialog box not yet implemented
                        StartCoroutine(ScreenFade.main.Fade(false, 1f));
                    }
                    
                    yield return new WaitForSeconds(0.2f);
                    running = false;

                    //fully heal players party so that they cant walk around with a defeated party
                    for (int i = 0; i < 6; i++)
                    {
                        if (SaveData.currentSave.PC.boxes[0][i] != null)
                        {
                            SaveData.currentSave.PC.boxes[0][i].healFull();
                        }
                    }
                }
                Dialog.UndrawDialogBox();
                yield return new WaitForSeconds(0.4f);
            }
        }*/


        //if defeated
        if (victor == 1)
        {
            //empty the paused clip, as the paused audio won't be resumed upon respawning
            BgmHandler.main.ResumeMain(1.4f, null, 0);
        }
        else
        {
            //if not defeated, the scene won't have faded out already
            StartCoroutine(ScreenFade.main.Fade(false, 1f));
            BgmHandler.main.ResumeMain(1.4f, PlayerMovement.player.accessedMapSettings.getBGM());
        }
        yield return new WaitForSeconds(1.4f);

        //check for evolutions to run ONLY if won, or healed on defeat
        if (victor == 0 || healedOnDefeat)
        {
            for (int i = 0; i < initialLevels.Length; i++)
            {
                if (SaveData.currentSave.PC.boxes[0][i] != null)
                {
                    //if level is different to it was at the start of the battle
                    if (SaveData.currentSave.PC.boxes[0][i].Level != initialLevels[i])
                    {
                        //if can evolve
                        if (SaveData.currentSave.PC.boxes[0][i].canEvolve("Level"))
                        {
                            BgmHandler.main.PlayOverlay(null, 0, 0);

                            //Set SceneEvolution to be active so that it appears
                            Scene.main.Evolution.gameObject.SetActive(true);
                            StartCoroutine(Scene.main.Evolution.control(SaveData.currentSave.PC.boxes[0][i], "Level"));
                            //Start an empty loop that will only stop when SceneEvolution is no longer active (is closed)
                            while (Scene.main.Evolution.gameObject.activeSelf)
                            {
                                yield return null;
                            }
                        }
                    }
                }
            }
        }


        //if defeated
        if (victor == 1)
        {
            if (!healedOnDefeat)
            {
                GlobalVariables.global.Respawn();
            }
        }
        if (BattleTravelling != null)
            StopCoroutine(BattleTravelling);
        BattleCamera.StopTravelling();
        if (statDisplayAnim != null)
            StopCoroutine(statDisplayAnim);
        GlobalVariables.global.resetFollower();
        PlayerMovement.player.UpdateRPC();
        //sceneWatch.SetActive(true);
        Destroy(backgroundObject);
        this.gameObject.SetActive(false);
        BattleDisplay.SetActive(false);
        BattleScene.SetActive(false);
    }
    
    public void UpdateRPC(bool isTrainer, Trainer trainer)
    {
        //Update Discord RPC
        float hour = System.DateTime.Now.Hour;

        string dayTime;

        if (hour < 6)
        {
            dayTime = "Night";
        }
        else if (hour < 12)
        {
            dayTime = "Morning";
        }
        else if (hour < 13)
        {
            dayTime = "Midday";
        }
        else if (hour < 18)
        {
            dayTime = "Afternoon";
        }
        else if (hour < 20)
        {
            dayTime = "Evening";
        }
        else
        {
            dayTime = "Night";
        }

        //Discord API Library
        //PresenceManager.UpdatePresence(
        //    detail: PlayerMovement.player.accessedMapSettings.mapName+" - "+dayTime, 
        //    state: "In "+((isTrainer) ? "Trainer" : "Wild")+" Battle",
        //    largeKey: PlayerMovement.player.accessedMapSettings.RPCImageKey,
        //    largeText: "",
        //    smallKey: "pokeball", 
        //    smallText: "Vs " + ((isTrainer) ? trainer.GetName() : trainer.GetParty()[0].Name + " Lv." + trainer.GetParty()[0].Level )
        //);
    }
}