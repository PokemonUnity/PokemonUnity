using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using PokemonUnity.Battle;

//[ExecuteInEditMode]
public class BattleAnimationHandler : UnityEngine.MonoBehaviour
{
	//private static GameVariables PersistantPlayerData { get { return StartupSceneHandler.PersistantPlayerData; } }
	public static IEnumerator<BattleResults> BattleCoroutineResults { get; private set; }
	public IEnumerator<BattleResults> BattleCoroutine { get; private set; }
	public BattleResults victor { get { return GameVariables.battle.decision; } }
    //public float time;
    //AnimationClip anim;

	#region Unity's MonoBehavior Variables
	//public Image playerBase, enemyBase, fieldBackground;
	public Selectable PartySlotUI;
	private bool trainerBattle;

	//ToDo: Switch to "Global" Dialog Feature.. should be GET only
	private DialogBoxHandlerNew Dialog { get; set; }

	/* Audio Controls Should be placed in an Audio Class, and referenced from there.
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
		hitPoorClip;*/

	/*public Sprite
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
		overlayStatDownTex;*/

	
	#region TASK BUTTONS
	/*private Image
		buttonFight,
		buttonBag,
		buttonRun,
		buttonPoke;


	private Image
		buttonMegaEvolution,
		buttonMoveReturn;

	private Image
		buttonBackBag,
		buttonBackPoke;

	private Image
		buttonSwitch,
		buttonCheck;*/

	private Image buttonItemLastUsed;

	private Image[]
		buttonItemCategory = new Image[4],
		buttonItemList = new Image[8];

	private Image[] buttonPokemonSlot = new Image[6];

	private Text
		buttonCheckText,
		buttonCheckTextShadow;
	#endregion TASK BUTTONS
	
	#region Fight Menu Options
	public GameObject[] Move { get; private set; }
	#region MOVE BUTTON DETAILS
	private Image[] buttonMoveType { get; set; }
	private Image[] buttonMoveCover { get; set; }
	private Image[] buttonMove { get; set; }
	private Text[] buttonMoveName { get; set; }
	//private Text[] buttonMoveNameShadow { get; set; }
	private Text[] buttonMovePP  { get; set; }
	//private Text[] buttonMovePPShadow  { get; set; }
	private Text[] buttonMoveMaxPP  { get; set; }
	#endregion MOVE BUTTON DETAILS
	#endregion Fight Menu Options

	/*private GameObject bagObject;

	#region ITEM LIST DETAILS
	private GameObject itemList;

	private Text
		itemListCategoryText,
		//itemListCategoryTextShadow,
		itemListPageNumber;
		//itemListPageNumberShadow;

	private GameObject
		itemListArrowPrev,
		itemListArrowNext;

	//private GameObject[] itemListButton = new GameObject[8]; //unused?
	private Image[] itemListIcon = new Image[8];

	private Text[]
		itemListName = new Text[8],
		//itemListNameShadow = new Text[8],
		itemListQuantity = new Text[8];
		//itemListQuantityShadow = new Text[8];

	private Text itemListDescription { get; set; }
		//itemListDescriptionShadow;
	#endregion ITEM LIST DETAILS
		
	#region POKEMON LIST DETAILS
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
	#endregion POKEMON LIST DETAILS

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
		statsLevelShadow = new Text[6];*/

	//BACKGROUNDS
	public Image
		playerBase,
		opponentBase,
		background;

	//DEBUG
	//public bool canMegaEvolve = false;

	//private Sprite[] playerTrainer1Animation;
	//private AnimationClip playerTrainer1Animation;
	private Image playerTrainerSprite1;
	//private Sprite[] trainer1Animation;
	private Image trainerSprite1;

	//private Sprite[] player1Animation;
	//private Image player1;
	//private RawImage player1Overlay;
	//private Sprite[] opponent1Animation;
	//private Image opponent1;
	//private RawImage opponent1Overlay;

	//private Coroutine animatePlayer1;
	//private Coroutine animateOpponent1;
	//private Coroutine animatingPartyIcons;


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


	private int pokemonPerSide { get; set; }
	#endregion


	void Awake()
	{
		//Adds a listener to the main slider and invokes a method when the value changes.
		//hpSlider[0].onValueChanged.AddListener(delegate { ValueChangeCheck(); });
		/*
		//anim = this.GetComponent<Animator>()
		Dialog = transform.GetComponent<DialogBoxHandlerNew>();

		//BattleAudio = transform.GetComponent<AudioSource>();

		playerBase = transform.Find("player0").GetComponent<Image>();
        opponentBase = transform.Find("opponent0").GetComponent<Image>();
        background = transform.Find("Background").GetComponent<Image>();

        trainerSprite1 = opponentBase.transform.Find("Trainer").GetComponent<Image>();
        playerTrainerSprite1 = playerBase.transform.Find("Trainer").GetComponent<Image>();

        player1 = playerBase.transform.Find("Pokemon").Find("Mask").Find("Sprite").GetComponent<Image>();
        opponent1 =
            opponentBase.transform.Find("Pokemon").Find("Mask").Find("Sprite").GetComponent<Image>();
        player1Overlay = player1.transform.Find("Overlay").GetComponent<RawImage>();
        opponent1Overlay = opponent1.transform.Find("Overlay").GetComponent<RawImage>();

        Transform playerPartyBarTrn = transform.Find("playerParty");
        Transform opponentPartyBarTrn = transform.Find("opponentParty");
        //playerPartyBar = playerPartyBarTrn.Find("bar").GetComponent<Image>();
        //opponentPartyBar = opponentPartyBarTrn.Find("bar").GetComponent<Image>();
        //for (int i = 0; i < 6; i++)
        //{
        //    playerPartyBarSpace[i] = playerPartyBarTrn.Find("space" + i).GetComponent<Image>();
        //}
        //for (int i = 0; i < 6; i++)
        //{
        //    opponentPartyBarSpace[i] = opponentPartyBarTrn.Find("space" + i).GetComponent<Image>();
        //}
		//
        //pokemonStatsDisplay[0] = transform.Find("playerStats0").GetComponent<Image>();
        //statsNameShadow[0] = pokemonStatsDisplay[0].transform.Find("Name").GetComponent<Text>();
        //statsName[0] = statsNameShadow[0].transform.Find("Text").GetComponent<Text>();
        //statsGenderShadow[0] = pokemonStatsDisplay[0].transform.Find("Gender").GetComponent<Text>();
        //statsGender[0] = statsGenderShadow[0].transform.Find("Text").GetComponent<Text>();
        //statsLevelShadow[0] = pokemonStatsDisplay[0].transform.Find("Level").GetComponent<Text>();
        //statsLevel[0] = statsLevelShadow[0].transform.Find("Text").GetComponent<Text>();
        //statsHPBar[0] = pokemonStatsDisplay[0].transform.Find("HPBar").GetComponent<Image>();
        //statsStatus[0] = pokemonStatsDisplay[0].transform.Find("Status").GetComponent<Image>();
        //pokemon0CurrentHPShadow = pokemonStatsDisplay[0].transform.Find("CurrentHP").GetComponent<Text>();
        //pokemon0CurrentHP = pokemon0CurrentHPShadow.transform.Find("Text").GetComponent<Text>();
        //pokemon0MaxHPShadow = pokemonStatsDisplay[0].transform.Find("MaxHP").GetComponent<Text>();
        //pokemon0MaxHP = pokemon0MaxHPShadow.transform.Find("Text").GetComponent<Text>();
        //pokemon0ExpBar = pokemonStatsDisplay[0].transform.Find("ExpBar").GetComponent<Image>();
		//
        //pokemonStatsDisplay[3] = transform.Find("opponentStats0").GetComponent<Image>();
        //statsNameShadow[3] = pokemonStatsDisplay[3].transform.Find("Name").GetComponent<Text>();
        //statsName[3] = statsNameShadow[3].transform.Find("Text").GetComponent<Text>();
        //statsGenderShadow[3] = pokemonStatsDisplay[3].transform.Find("Gender").GetComponent<Text>();
        //statsGender[3] = statsGenderShadow[3].transform.Find("Text").GetComponent<Text>();
        //statsLevelShadow[3] = pokemonStatsDisplay[3].transform.Find("Level").GetComponent<Text>();
        //statsLevel[3] = statsLevelShadow[3].transform.Find("Text").GetComponent<Text>();
        //statsHPBar[3] = pokemonStatsDisplay[3].transform.Find("HPBar").GetComponent<Image>();
        //statsStatus[3] = pokemonStatsDisplay[3].transform.Find("Status").GetComponent<Image>();

        Transform optionBox = transform.Find("OptionBox");
        //buttonFight = optionBox.Find("ButtonFight").GetComponent<Image>();
        //buttonBag = optionBox.Find("ButtonBag").GetComponent<Image>();
        //buttonPoke = optionBox.Find("ButtonPoke").GetComponent<Image>();
        //buttonRun = optionBox.Find("ButtonRun").GetComponent<Image>();

		#region Fight Menu Options
		Move = new GameObject[4];
		buttonMoveCover = buttonMoveType = new Image[4];
		buttonMoveName = buttonMovePP = buttonMovePPShadow = buttonMoveNameShadow = new Text[4];
		for (int i = 0; i < 4; i++)
        {
			Move[i] = optionBox.Find("OptionsFight").Find("Move" + (i + 1)).gameObject;
			//Move[i] = optionBox.Find("OptionsFight").Find("Move" + (i + 1)).Find("Name").GetComponent<Text>();
			//Move[i] = optionBox.Find("OptionsFight").Find("Move" + (i + 1)).Find("Name").gameObject.Find("Text").GetComponent<Text>();
			//Move[i] = optionBox.Find("OptionsFight").Find("Move" + (i + 1)).Find("PP").GetComponent<Text>();
			//Move[i] = optionBox.Find("OptionsFight").Find("Move" + (i + 1)).Find("PP").gameObject.Find("Text").GetComponent<Text>();
			//Move[i] = optionBox.Find("OptionsFight").Find("Move" + (i + 1)).Find("Cover").GetComponent<Image>();//.color;
			//Move[i] = optionBox.Find("OptionsFight").Find("Move" + (i + 1)).Find("Type").GetComponent<Image>();//.sprite;
            //buttonMove[i]			= optionBox.Find("OptionsFight").Find("Move" + (i + 1)).GetComponent<Image>();
            buttonMoveType[i]		= optionBox.Find("OptionsFight").Find("Move" + (i + 1)).Find("Type").GetComponent<Image>();
            buttonMoveCover[i]		= optionBox.Find("OptionsFight").Find("Move" + (i + 1)).Find("Cover").GetComponent<Image>();
            buttonMoveNameShadow[i]	= optionBox.Find("OptionsFight").Find("Move" + (i + 1)).Find("Name").GetComponent<Text>();
            buttonMovePPShadow[i]	= optionBox.Find("OptionsFight").Find("Move" + (i + 1)).Find("PP").GetComponent<Text>();
            buttonMoveName[i]		= buttonMoveNameShadow[i].transform.Find("Text").GetComponent<Text>();
            buttonMovePP[i]			= buttonMovePPShadow[i].transform.Find("Text").GetComponent<Text>();
        }
		#endregion*/
        /*buttonMoveReturn = optionBox.Find("MoveReturn").GetComponent<Image>();
        buttonMegaEvolution = optionBox.Find("MoveMegaEvolution").GetComponent<Image>();

        bagObject = optionBox.Find("Bag").gameObject;
        pokeObject = optionBox.Find("Poke").gameObject;
        pokemonPartyObject = optionBox.Find("Party").gameObject;
		
        buttonBackBag			= bagObject.transform.Find("ButtonBack").GetComponent<Image>();
        buttonBackPoke			= pokeObject.transform.Find("ButtonBack").GetComponent<Image>();
		
        buttonItemCategory[0]	= bagObject.transform.Find("ButtonHPPPRestore").GetComponent<Image>();
        buttonItemCategory[1]	= bagObject.transform.Find("ButtonPokeBalls").GetComponent<Image>();
        buttonItemCategory[2]	= bagObject.transform.Find("ButtonStatusHealers").GetComponent<Image>();
        buttonItemCategory[3]	= bagObject.transform.Find("ButtonBattleItems").GetComponent<Image>();
        buttonItemLastUsed		= bagObject.transform.Find("ButtonItemUsedLast").GetComponent<Image>();

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
        pokemonMovesSelectedMove = pokemonMoves.transform.Find("SelectedMove").GetComponent<Image>();*/
    }

    void OnEnable()
    {
    }

    void Start()
    {
	}

    void Update()
    {
		/* Ping GameNetwork server every 15-45secs
         * If game server is offline or unable to
         * ping connection:
         * Netplay toggle-icon bool equals false
         * otherwise toggle bool to true
         */
		//StartCoroutine(PingServerEveryXsec);
		//Use coroutine that has a while loop instead of using here
		/*while (MainMenu.activeSelf)
        {
            //While scene is enabled, run coroutine to ping server
            break;
        }*/
		//int index = (int)(UnityEngine.Time.timeSinceLevelLoad * Settings.framesPerSecond);
		//index = index % sprites[].Length;string[] dir = AssetDatabase.GetSubFolders("Assets/Pokemons");
	}

	public void UpdatePartyStatusUI()
	{
		Transform playerParty = transform.Find("playerParty");
		Transform opponentParty = transform.Find("opponentParty");
		//Foreach UI Child in Unity
		for (int i = 0; i < playerParty.childCount; i++)
		{
			var pkmn = GameVariables.battle.party1[i];
			if (pkmn.Species != PokemonUnity.Pokemon.Pokemons.NONE)
			{
				var status = pkmn.Status;
				switch (status)
				{
					case PokemonUnity.Move.Status.SLEEP:
					case PokemonUnity.Move.Status.POISON:
					case PokemonUnity.Move.Status.PARALYSIS:
					case PokemonUnity.Move.Status.BURN:
					case PokemonUnity.Move.Status.FROZEN:
						playerParty.GetChild(i).GetComponent<Image>().sprite = PartySlotUI.spriteState.pressedSprite;
						break;
					case PokemonUnity.Move.Status.NONE:
					default:
						playerParty.GetChild(i).GetComponent<Image>().sprite = PartySlotUI.spriteState.highlightedSprite;
						break;
				}
				if(pkmn.isFainted())
					playerParty.GetChild(i).GetComponent<Image>().sprite = PartySlotUI.spriteState.disabledSprite;
			}
			else
				playerParty.GetChild(i).GetComponent<Image>().sprite = PartySlotUI.GetComponent<Image>().sprite;
		}

		for (int i = 0; i < opponentParty.childCount; i++)
		{
			var pkmn = GameVariables.battle.party1[i];
			if (pkmn.Species != PokemonUnity.Pokemon.Pokemons.NONE)
			{
				var status = pkmn.Status;
				switch (status)
				{
					case PokemonUnity.Move.Status.SLEEP:
					case PokemonUnity.Move.Status.POISON:
					case PokemonUnity.Move.Status.PARALYSIS:
					case PokemonUnity.Move.Status.BURN:
					case PokemonUnity.Move.Status.FROZEN:
						opponentParty.GetChild(i).GetComponent<Image>().sprite = PartySlotUI.spriteState.pressedSprite;
						break;
					case PokemonUnity.Move.Status.NONE:
					default:
						opponentParty.GetChild(i).GetComponent<Image>().sprite = PartySlotUI.spriteState.highlightedSprite;
						break;
				}
				if(pkmn.isFainted())
					opponentParty.GetChild(i).GetComponent<Image>().sprite = PartySlotUI.spriteState.disabledSprite;
			}
			else
				opponentParty.GetChild(i).GetComponent<Image>().sprite = PartySlotUI.GetComponent<Image>().sprite;
		}
	}

	/* Create an animation controller
     * By default, create the battle effects on that controller
     * Faint animation; attack animation; idle animation
     * 
     * When loading a pokemon, check the frame count (or sprite count ...that might eat memory)
     * Adjust the animation length to match the frames (using a 1:1 frames per second)
     */

	/*#region Pokemon Battle Animation Commands
	/// <summary>
	/// Invoked when the value of the slider changes.
	/// </summary>
	public void ValueChangeCheck()
	{
		Debug.Log(healthSlider.value);
		//GoodSlider(this.GetComponent<Slider>().value);
		//images = gameObject.GetComponentsInChildren<Image>();

		//each time the silder's value is changed, write to text displaying the hp

		Fill.color = hpzone0;
		if (healthSlider.value <= (healthSlider.normalizedValue.CompareTo(0.5f)))
		{ //  / 2)) {
		  //Change color of hp bar
			Fill.color = hpzone1;
			//Change background image for health slider
		}
		if (healthSlider.value <= (startingHealth / 4)) { Fill.color = hpzone2; }

		//Text = healthSlider.value; //Set text under hp to match slider currentHealth
	}
	//////////////////////////////////
	/// ANIMATIONS
	//
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

	private IEnumerator slidePokemonStats(int position, bool retract)
	{
		float distanceX = pokemonStatsDisplay[position].rectTransform.sizeDelta.x;
		float startX = (retract) ? 171f - (distanceX / 2) : 171f + (distanceX / 2);
		//flip values if opponent stats
		if (position > 2)
		{
			startX = -startX;
			distanceX = -distanceX;
		}
		//flip movement direction if retracting
		if (retract)
		{
			distanceX = -distanceX;
		}

		pokemonStatsDisplay[position].gameObject.SetActive(true);

		float speed = 0.3f;
		float increment = 0f;
		while (increment < 1)
		{
			increment += (1 / speed) * Time.deltaTime;
			if (increment > 1)
			{
				increment = 1f;
			}

			pokemonStatsDisplay[position].rectTransform.localPosition = new Vector3(startX - (distanceX * increment),
				pokemonStatsDisplay[position].rectTransform.localPosition.y, 0);

			yield return null;
		}
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
	private IEnumerator displayPartyBar(bool isOpponent, PokemonOld[] party)
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

		StartCoroutine(stretchBar(bar, 128, 320));
		yield return new WaitForSeconds(0.1f);
		for (int i = 0; i < 6; i++)
		{
			//Set space sprite
			space[i].sprite = partySpaceTex;
			if (party.Length > i)
			{
				if (party[i] != null)
				{
					if (party[i].getStatus() == PokemonOld.Status.FAINTED)
					{
						space[i].sprite = partyFaintTex;
					}
					else if (party[i].getStatus() == PokemonOld.Status.NONE)
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
			StartCoroutine(slidePartyBarBall(space[i], -96 + (16 * i) + 128, -99 + (16 * i), 0.35f));
			yield return new WaitForSeconds(0.05f);
		}
		//Wait for last space to stop moving
		yield return new WaitForSeconds(0.3f);
		//Slide all spaces back a tiny bit
		for (int i = 0; i < 6; i++)
		{
			StartCoroutine(slidePartyBarBall(space[i], -99 + (16 * i), -96 + (16 * i), 0.1f));
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
		ball.rectTransform.localPosition = new Vector3(startX, ball.rectTransform.localPosition.y, 0);

		float distanceX = destinationX - startX;

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
			yield return null;
		}
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
				setHPBarColor(bar, 48f);
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
				itemListString = SaveDataOld.currentSave.Bag.getBattleTypeArray(ItemDataOld.BattleType.HPPPRESTORE);
				itemListCategoryText.text = "HP/PP Restore";
			}
			else if (bagCategoryPosition == 1)
			{
				itemListString = SaveDataOld.currentSave.Bag.getBattleTypeArray(ItemDataOld.BattleType.POKEBALLS);
				itemListCategoryText.text = "Poké Balls";
			}
			else if (bagCategoryPosition == 2)
			{
				itemListString = SaveDataOld.currentSave.Bag.getBattleTypeArray(ItemDataOld.BattleType.STATUSHEALER);
				itemListCategoryText.text = "Status Healers";
			}
			else if (bagCategoryPosition == 3)
			{
				itemListString = SaveDataOld.currentSave.Bag.getBattleTypeArray(ItemDataOld.BattleType.BATTLEITEMS);
				itemListCategoryText.text = "Battle Items";
			}
			itemListCategoryTextShadow.text = itemListCategoryText.text;
			itemListPagePosition = 0;

			itemListPageCount = Mathf.CeilToInt((float)itemListString.Length / 8f);
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
			updatePokemonSummaryDisplay(SaveDataOld.currentSave.PC.boxes[0][pokePartyPosition]);
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
			statsName[position].text = pokemon[position].getName();
			statsNameShadow[position].text = statsName[position].text;
			if (pokemon[position].getGender() == PokemonOld.Gender.FEMALE)
			{
				statsGender[position].text = "♀";
				statsGender[position].color = new Color(1, 0.2f, 0.2f, 1);
			}
			else if (pokemon[position].getGender() == PokemonOld.Gender.MALE)
			{
				statsGender[position].text = "♂";
				statsGender[position].color = new Color(0.2f, 0.4f, 1, 1);
			}
			else
			{
				statsGender[position].text = null;
			}
			statsGenderShadow[position].text = statsGender[position].text;
			statsLevel[position].text = "" + pokemon[position].getLevel();
			statsLevelShadow[position].text = statsLevel[position].text;
			statsHPBar[position].rectTransform.sizeDelta =
				new Vector2(Mathf.CeilToInt(pokemon[position].getPercentHP() * 48f), 4f);

			setHPBarColor(statsHPBar[position], 48f);


			if (pokemon[position].getStatus() != PokemonOld.Status.NONE)
			{
				statsStatus[position].sprite =
					Resources.Load<Sprite>("PCSprites/status" + pokemon[position].getStatus().ToString());
			}
			else
			{
				statsStatus[position].sprite = Resources.Load<Sprite>("null");
			}
			if (position == 0 && pokemonPerSide == 1)
			{
				pokemon0CurrentHP.text = "" + pokemon[0].getCurrentHP();
				pokemon0CurrentHPShadow.text = pokemon0CurrentHP.text;
				pokemon0MaxHP.text = "" + pokemon[0].getHP();
				pokemon0MaxHPShadow.text = pokemon0MaxHP.text;
				float expCurrentLevel =
					PokemonDatabaseOld.getLevelExp(PokemonDatabaseOld.getPokemon(pokemon[0].getID()).getLevelingRate(),
						pokemon[0].getLevel());
				float expNextlevel =
					PokemonDatabaseOld.getLevelExp(PokemonDatabaseOld.getPokemon(pokemon[0].getID()).getLevelingRate(),
						pokemon[0].getLevel() + 1);
				float expAlong = pokemon[0].getExp() - expCurrentLevel;
				float expDistance = expAlong / (expNextlevel - expCurrentLevel);
				pokemon0ExpBar.rectTransform.sizeDelta = new Vector2(Mathf.Floor(expDistance * 80f), 2f);
			}
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
				PokemonDataOld.Type type = MoveDatabase.getMove(moveset[i]).getType();

				if (type == PokemonDataOld.Type.BUG)
				{
					buttonMoveCover[i].color = new Color(0.47f, 0.57f, 0.06f, 1);
					buttonMoveType[i].sprite = Resources.Load<Sprite>("PCSprites/type" + type.ToString());
				}
				else if (type == PokemonDataOld.Type.DARK)
				{
					buttonMoveCover[i].color = new Color(0.32f, 0.28f, 0.24f, 1);
					buttonMoveType[i].sprite = Resources.Load<Sprite>("PCSprites/type" + type.ToString());
				}
				else if (type == PokemonDataOld.Type.DRAGON)
				{
					buttonMoveCover[i].color = new Color(0.32f, 0.25f, 1f, 1);
					buttonMoveType[i].sprite = Resources.Load<Sprite>("PCSprites/type" + type.ToString());
				}
				else if (type == PokemonDataOld.Type.ELECTRIC)
				{
					buttonMoveCover[i].color = new Color(0.64f, 0.52f, 0.04f, 1);
					buttonMoveType[i].sprite = Resources.Load<Sprite>("PCSprites/type" + type.ToString());
				}
				else if (type == PokemonDataOld.Type.FAIRY)
				{
					buttonMoveCover[i].color = new Color(0.7f, 0.33f, 0.6f, 1);
					buttonMoveType[i].sprite = Resources.Load<Sprite>("PCSprites/type" + type.ToString());
				}
				else if (type == PokemonDataOld.Type.FIGHTING)
				{
					buttonMoveCover[i].color = new Color(0.75f, 0.19f, 0.15f, 1);
					buttonMoveType[i].sprite = Resources.Load<Sprite>("PCSprites/type" + type.ToString());
				}
				else if (type == PokemonDataOld.Type.FIRE)
				{
					buttonMoveCover[i].color = new Color(0.94f, 0.5f, 0.19f, 1);
					buttonMoveType[i].sprite = Resources.Load<Sprite>("PCSprites/type" + type.ToString());
				}
				else if (type == PokemonDataOld.Type.FLYING)
				{
					buttonMoveCover[i].color = new Color(0.5f, 0.43f, 0.72f, 1);
					buttonMoveType[i].sprite = Resources.Load<Sprite>("PCSprites/type" + type.ToString());
				}
				else if (type == PokemonDataOld.Type.GHOST)
				{
					buttonMoveCover[i].color = new Color(0.4f, 0.32f, 0.55f, 1);
					buttonMoveType[i].sprite = Resources.Load<Sprite>("PCSprites/type" + type.ToString());
				}
				else if (type == PokemonDataOld.Type.GRASS)
				{
					buttonMoveCover[i].color = new Color(0.34f, 0.5f, 0.25f, 1);
					buttonMoveType[i].sprite = Resources.Load<Sprite>("PCSprites/type" + type.ToString());
				}
				else if (type == PokemonDataOld.Type.GROUND)
				{
					buttonMoveCover[i].color = new Color(0.53f, 0.4f, 0.19f, 1);
					buttonMoveType[i].sprite = Resources.Load<Sprite>("PCSprites/type" + type.ToString());
				}
				else if (type == PokemonDataOld.Type.ICE)
				{
					buttonMoveCover[i].color = new Color(0.4f, 0.6f, 0.6f, 1);
					buttonMoveType[i].sprite = Resources.Load<Sprite>("PCSprites/type" + type.ToString());
				}
				else if (type == PokemonDataOld.Type.NORMAL)
				{
					buttonMoveCover[i].color = new Color(0.5f, 0.5f, 0.35f, 1);
					buttonMoveType[i].sprite = Resources.Load<Sprite>("PCSprites/type" + type.ToString());
				}
				else if (type == PokemonDataOld.Type.POISON)
				{
					buttonMoveCover[i].color = new Color(0.63f, 0.25f, 0.63f, 1);
					buttonMoveType[i].sprite = Resources.Load<Sprite>("PCSprites/type" + type.ToString());
				}
				else if (type == PokemonDataOld.Type.PSYCHIC)
				{
					buttonMoveCover[i].color = new Color(0.75f, 0.25f, 0.4f, 1);
					buttonMoveType[i].sprite = Resources.Load<Sprite>("PCSprites/type" + type.ToString());
				}
				else if (type == PokemonDataOld.Type.ROCK)
				{
					buttonMoveCover[i].color = new Color(0.48f, 0.35f, 0.14f, 1);
					buttonMoveType[i].sprite = Resources.Load<Sprite>("PCSprites/type" + type.ToString());
				}
				else if (type == PokemonDataOld.Type.STEEL)
				{
					buttonMoveCover[i].color = new Color(0.6f, 0.6f, 0.67f, 1);
					buttonMoveType[i].sprite = Resources.Load<Sprite>("PCSprites/type" + type.ToString());
				}
				else if (type == PokemonDataOld.Type.WATER)
				{
					buttonMoveCover[i].color = new Color(0.25f, 0.42f, 0.75f, 1);
					buttonMoveType[i].sprite = Resources.Load<Sprite>("PCSprites/type" + type.ToString());
				}

				buttonMoveName[i].text = moveset[i];
				buttonMoveNameShadow[i].text = buttonMoveName[i].text;
				buttonMovePP[i].text = PP[i] + "/" + maxPP[i];
				buttonMovePPShadow[i].text = buttonMovePP[i].text;
			}
			else
			{
				buttonMoveCover[i].color = new Color(0.5f, 0.5f, 0.5f, 1);
				buttonMoveType[i].sprite = Resources.Load<Sprite>("null");
				buttonMoveName[i].text = "";
				buttonMoveNameShadow[i].text = buttonMoveName[i].text;
				buttonMovePP[i].text = "";
				buttonMovePPShadow[i].text = buttonMovePP[i].text;
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
				itemListQuantity[i].text = "" + SaveDataOld.currentSave.Bag.getQuantity(itemListPageString[i]);
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
			PokemonOld selectedPokemon = SaveDataOld.currentSave.PC.boxes[0][i];
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
						if (selectedPokemon.getStatus() != PokemonOld.Status.FAINTED)
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
						if (selectedPokemon.getStatus() != PokemonOld.Status.FAINTED)
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
						if (selectedPokemon.getStatus() != PokemonOld.Status.FAINTED)
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
						if (selectedPokemon.getStatus() != PokemonOld.Status.FAINTED)
						{
							buttonPokemonSlot[i].sprite = buttonPokemonTex;
						}
						else
						{
							buttonPokemonSlot[i].sprite = buttonPokemonFntTex;
						}
					}
				}
				pokemonIconAnim[i] = selectedPokemon.GetIcons_();
				pokemonSlotIcon[i].sprite = pokemonIconAnim[i][0];
				pokemonSlotName[i].text = selectedPokemon.getName();
				pokemonSlotNameShadow[i].text = pokemonSlotName[i].text;
				if (selectedPokemon.getGender() == PokemonOld.Gender.FEMALE)
				{
					pokemonSlotGender[i].text = "♀";
					pokemonSlotGender[i].color = new Color(1, 0.2f, 0.2f, 1);
				}
				else if (selectedPokemon.getGender() == PokemonOld.Gender.MALE)
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
										 ((float)selectedPokemon.getCurrentHP() / (float)selectedPokemon.getHP())), 4);

				setHPBarColor(pokemonSlotHPBar[i], 48f);

				pokemonSlotLevel[i].text = "" + selectedPokemon.getLevel();
				pokemonSlotLevelShadow[i].text = pokemonSlotLevel[i].text;
				pokemonSlotCurrentHP[i].text = "" + selectedPokemon.getCurrentHP();
				pokemonSlotCurrentHPShadow[i].text = pokemonSlotCurrentHP[i].text;
				pokemonSlotMaxHP[i].text = "" + selectedPokemon.getHP();
				pokemonSlotMaxHPShadow[i].text = pokemonSlotMaxHP[i].text;
				if (selectedPokemon.getStatus() != PokemonOld.Status.NONE)
				{
					pokemonSlotStatus[i].sprite =
						Resources.Load<Sprite>("PCSprites/status" + selectedPokemon.getStatus().ToString());
				}
				else
				{
					pokemonSlotStatus[i].sprite = Resources.Load<Sprite>("null");
				}
				pokemonSlotItem[i].enabled = !string.IsNullOrEmpty(selectedPokemon.getHeldItem());
			}
		}
	}

	private void updatePokemonSummaryDisplay(PokemonOld selectedPokemon)
	{
		pokemonSelectedIcon.sprite = selectedPokemon.GetIcons_()[0];
		pokemonSelectedName.text = selectedPokemon.getName();
		pokemonSelectedNameShadow.text = pokemonSelectedName.text;
		if (selectedPokemon.getGender() == PokemonOld.Gender.FEMALE)
		{
			pokemonSelectedGender.text = "♀";
			pokemonSelectedGender.color = new Color(1, 0.2f, 0.2f, 1);
		}
		else if (selectedPokemon.getGender() == PokemonOld.Gender.MALE)
		{
			pokemonSelectedGender.text = "♂";
			pokemonSelectedGender.color = new Color(0.2f, 0.4f, 1, 1);
		}
		else
		{
			pokemonSelectedGender.text = null;
		}
		pokemonSelectedGenderShadow.text = pokemonSelectedGender.text;
		pokemonSelectedLevel.text = "" + selectedPokemon.getLevel();
		pokemonSelectedLevelShadow.text = pokemonSelectedLevel.text;
		if (selectedPokemon.getStatus() != PokemonOld.Status.NONE)
		{
			pokemonSelectedStatus.sprite =
				Resources.Load<Sprite>("PCSprites/status" + selectedPokemon.getStatus().ToString());
		}
		else
		{
			pokemonSelectedStatus.sprite = Resources.Load<Sprite>("null");
		}
		pokemonSelectedType1.sprite = Resources.Load<Sprite>("null");
		pokemonSelectedType2.sprite = Resources.Load<Sprite>("null");
		PokemonDataOld.Type type1 = PokemonDatabaseOld.getPokemon(selectedPokemon.getID()).getType1();
		PokemonDataOld.Type type2 = PokemonDatabaseOld.getPokemon(selectedPokemon.getID()).getType2();
		if (type1 != PokemonDataOld.Type.NONE)
		{
			pokemonSelectedType1.sprite = Resources.Load<Sprite>("PCSprites/type" + type1.ToString());
		}
		if (type2 != PokemonDataOld.Type.NONE)
		{
			pokemonSelectedType2.sprite = Resources.Load<Sprite>("PCSprites/type" + type2.ToString());
		}

		//Summary
		float expCurrentLevel =
			PokemonDatabaseOld.getLevelExp(PokemonDatabaseOld.getPokemon(selectedPokemon.getID()).getLevelingRate(),
				selectedPokemon.getLevel());
		float expNextlevel =
			PokemonDatabaseOld.getLevelExp(PokemonDatabaseOld.getPokemon(selectedPokemon.getID()).getLevelingRate(),
				selectedPokemon.getLevel() + 1);
		float expAlong = selectedPokemon.getExp() - expCurrentLevel;
		float expDistance = expAlong / (expNextlevel - expCurrentLevel);
		pokemonSummaryNextLevelEXP.text = "" + (expNextlevel - selectedPokemon.getExp());
		pokemonSummaryNextLevelEXPShadow.text = pokemonSummaryNextLevelEXP.text;
		pokemonSummaryEXPBar.rectTransform.sizeDelta = new Vector2(Mathf.Floor(expDistance * 64), 3f);
		pokemonSummaryItemIcon.sprite = Resources.Load<Sprite>("null");
		pokemonSummaryItemName.text = "No held item.";
		if (!string.IsNullOrEmpty(selectedPokemon.getHeldItem()))
		{
			pokemonSummaryItemIcon.sprite = Resources.Load<Sprite>("Items/" + selectedPokemon.getHeldItem());
			pokemonSummaryItemName.text = selectedPokemon.getHeldItem();
		}

		pokemonSummaryItemNameShadow.text = pokemonSummaryItemName.text;
		//Stats
		float currentHP = selectedPokemon.getCurrentHP();
		float maxHP = selectedPokemon.getHP();
		pokemonSummaryHP.text = currentHP + "/" + maxHP;
		pokemonSummaryHPShadow.text = pokemonSummaryHP.text;
		pokemonSummaryHPBar.rectTransform.sizeDelta = new Vector2(Mathf.Floor((1 - (maxHP - currentHP) / maxHP) * 48f),
			4f);

		setHPBarColor(pokemonSummaryHPBar, 48f);

		float[] natureMod = new float[]
		{
			NatureDatabaseOld.getNature(selectedPokemon.getNature()).getATK(),
			NatureDatabaseOld.getNature(selectedPokemon.getNature()).getDEF(),
			NatureDatabaseOld.getNature(selectedPokemon.getNature()).getSPA(),
			NatureDatabaseOld.getNature(selectedPokemon.getNature()).getSPD(),
			NatureDatabaseOld.getNature(selectedPokemon.getNature()).getSPE()
		};
		pokemonSummaryStats.text =
			selectedPokemon.getATK() + "\n" +
			selectedPokemon.getDEF() + "\n" +
			selectedPokemon.getSPA() + "\n" +
			selectedPokemon.getSPD() + "\n" +
			selectedPokemon.getSPE();
		pokemonSummaryStatsShadow.text = pokemonSummaryStats.text;

		string[] statsLines = new string[] { "Attack", "Defence", "Sp. Atk", "Sp. Def", "Speed" };
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

		pokemonSummaryAbilityName.text =
			PokemonDatabaseOld.getPokemon(selectedPokemon.getID()).getAbility(selectedPokemon.getAbility());
		pokemonSummaryAbilityNameShadow.text = pokemonSummaryAbilityName.text;
		//abilities not yet implemented
		pokemonSummaryAbilityDescription.text = "";
		pokemonSummaryAbilityDescriptionShadow.text = pokemonSummaryAbilityDescription.text;

		//Moves
		string[] moveset = selectedPokemon.getMoveset();
		int[] maxPP = selectedPokemon.getMaxPP();
		int[] PP = selectedPokemon.getPP();
		for (int i = 0; i < 4; i++)
		{
			if (moveset[i] != null)
			{
				pokemonMovesName[i].text = moveset[i];
				pokemonMovesNameShadow[i].text = pokemonMovesName[i].text;
				pokemonMovesType[i].sprite =
					Resources.Load<Sprite>("PCSprites/type" + MoveDatabase.getMove(moveset[i]).getType().ToString());
				pokemonMovesPPText[i].text = "PP";
				pokemonMovesPPTextShadow[i].text = pokemonMovesPPText[i].text;
				pokemonMovesPP[i].text = PP[i] + "/" + maxPP[i];
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

	private void updateSelectedTask(int newPosition)
	{
		taskPosition = newPosition;

		buttonBag.sprite = (taskPosition == 0 || taskPosition == 3) ? buttonBagSelTex : buttonBagTex;
		buttonFight.sprite = (taskPosition == 1) ? buttonFightSelTex : buttonFightTex;
		buttonPoke.sprite = (taskPosition == 2 || taskPosition == 5) ? buttonPokeSelTex : buttonPokeTex;
		buttonRun.sprite = (taskPosition == 4) ? buttonRunSelTex : buttonRunTex;
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
				ItemDatabaseOld.getItem(itemListString[newPosition + (itemListPagePosition * 8)]).getDescription();
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
			if (SaveDataOld.currentSave.PC.boxes[0][newPosition] == null)
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
						else if (SaveDataOld.currentSave.PC.boxes[0][checkPosition] != null)
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
								while (SaveDataOld.currentSave.PC.boxes[0][newPosition] == null && newPosition > 0)
								{
									newPosition -= 1;
								}
							}
							spaceFound = true;
						}
						else if (SaveDataOld.currentSave.PC.boxes[0][checkPosition] != null)
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
				if (SaveDataOld.currentSave.PC.boxes[0][newPosition] == null)
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
							else if (SaveDataOld.currentSave.PC.boxes[0][checkPosition] != null)
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
					if (SaveDataOld.currentSave.PC.boxes[0][i] != null)
					{
						buttonPokemonSlot[i].sprite = (SaveDataOld.currentSave.PC.boxes[0][i].getStatus() !=
													   PokemonOld.Status.FAINTED)
							? buttonPokemonRoundTex
							: buttonPokemonRoundFntTex;
					}
				}
				else if (i < 6)
				{
					if (SaveDataOld.currentSave.PC.boxes[0][i] != null)
					{
						buttonPokemonSlot[i].sprite = (SaveDataOld.currentSave.PC.boxes[0][i].getStatus() !=
													   PokemonOld.Status.FAINTED)
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
					if (SaveDataOld.currentSave.PC.boxes[0][i] != null)
					{
						buttonPokemonSlot[i].sprite = (SaveDataOld.currentSave.PC.boxes[0][i].getStatus() !=
													   PokemonOld.Status.FAINTED)
							? buttonPokemonRoundSelTex
							: buttonPokemonRoundFntSelTex;
					}
				}
				else if (i < 6)
				{
					if (SaveDataOld.currentSave.PC.boxes[0][i] != null)
					{
						buttonPokemonSlot[i].sprite = (SaveDataOld.currentSave.PC.boxes[0][i].getStatus() !=
													   PokemonOld.Status.FAINTED)
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
			string[] moveset = SaveDataOld.currentSave.PC.boxes[0][pokePartyPosition].getMoveset();
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
				MoveDataOld selectedMove = MoveDatabase.getMove(moveset[newPosition]);
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


	//////////////////////////////////
	/// BATTLE DATA MANAGEMENT
	//
	/// Calculates the base damage of an attack (before modifiers are applied).
	private float calculateDamage(int attackerPosition, int targetPosition, MoveDataOld move)
	{
		float baseDamage = 0;
		if (move.getCategory() == MoveDataOld.Category.PHYSICAL)
		{
			baseDamage = ((2f * (float)pokemon[attackerPosition].getLevel() + 10f) / 250f) *
						 ((float)pokemonStats[0][attackerPosition] / (float)pokemonStats[1][targetPosition]) *
						 (float)move.getPower() + 2f;
		}
		else if (move.getCategory() == MoveDataOld.Category.SPECIAL)
		{
			baseDamage = ((2f * (float)pokemon[attackerPosition].getLevel() + 10f) / 250f) *
						 ((float)pokemonStats[2][attackerPosition] / (float)pokemonStats[3][targetPosition]) *
						 (float)move.getPower() + 2f;
		}

		baseDamage *= Random.Range(0.85f, 1f);
		return baseDamage;
	}

	/// Uses the attacker's total critical ratio to randomly determine whether a Critical Hit should happen or not
	private bool calculateCritical(int attackerPosition, int targetPosition, MoveDataOld move)
	{
		int attackerCriticalRatio = 0;
		if (focusEnergy[attackerPosition])
		{
			attackerCriticalRatio += 1;
		}

		if (move.hasMoveEffect(MoveDataOld.Effect.Critical))
		{
			attackerCriticalRatio += 1;
		}

		bool applyCritical = false;
		if (move.getCategory() != MoveDataOld.Category.STATUS)
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
	private float calculateModifiedDamage(int attackerPosition, int targetPosition, MoveDataOld move, float baseDamage,
		bool applyCritical)
	{
		float modifiedDamage = baseDamage;

		//apply STAB
		if (pokemonType1[attackerPosition] == move.getType() ||
			pokemonType2[attackerPosition] == move.getType() ||
			pokemonType3[attackerPosition] == move.getType())
		{
			modifiedDamage *= 1.5f;
		}

		//apply Offence/Defence boosts 
		if (move.getCategory() == MoveDataOld.Category.PHYSICAL)
		{
			modifiedDamage *= calculateStatModifier(pokemonStatsMod[0][attackerPosition]);
			if (!applyCritical)
			{
				//exclude defensive buffs in a critical hit
				modifiedDamage /= calculateStatModifier(pokemonStatsMod[1][targetPosition]);
			}
		}
		else if (move.getCategory() == MoveDataOld.Category.SPECIAL)
		{
			modifiedDamage *= calculateStatModifier(pokemonStatsMod[2][attackerPosition]);
			if (!applyCritical)
			{
				//exclude defensive buffs in a critical hit
				modifiedDamage /= calculateStatModifier(pokemonStatsMod[3][targetPosition]);
			}
		}


		//not yet implemented
		//apply held item
		//apply ability
		//apply field advantages
		//reflect/lightScreen
		if (!applyCritical)
		{
			if (move.getCategory() == MoveDataOld.Category.PHYSICAL)
			{
				if (reflectTurns[Mathf.FloorToInt((float)targetPosition / 3f)] > 0)
				{
					modifiedDamage *= 0.5f;
				}
			}
			else if (move.getCategory() == MoveDataOld.Category.SPECIAL)
			{
				if (lightScreenTurns[Mathf.FloorToInt((float)targetPosition / 3f)] > 0)
				{
					modifiedDamage *= 0.5f;
				}
			}
		}

		//apply multi-target debuff 

		return Mathf.Floor(modifiedDamage);
	}

	private float calculateStatModifier(int modifier)
	{
		if (modifier > 0)
		{
			return ((2f + (float)modifier) / 2f);
		}
		else if (modifier < 0)
		{
			return (2f / (2f + Mathf.Abs((float)modifier)));
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
	private float getSuperEffectiveModifier(PokemonDataOld.Type attackingType, PokemonDataOld.Type targetType)
	{
		if (attackingType == PokemonDataOld.Type.BUG)
		{
			if (targetType == PokemonDataOld.Type.DARK || targetType == PokemonDataOld.Type.GRASS ||
				targetType == PokemonDataOld.Type.PSYCHIC)
			{
				return 2f;
			}
			else if (targetType == PokemonDataOld.Type.FAIRY || targetType == PokemonDataOld.Type.FIGHTING ||
					 targetType == PokemonDataOld.Type.FIRE || targetType == PokemonDataOld.Type.FLYING ||
					 targetType == PokemonDataOld.Type.GHOST || targetType == PokemonDataOld.Type.POISON ||
					 targetType == PokemonDataOld.Type.STEEL)
			{
				return 0.5f;
			}
		}
		else if (attackingType == PokemonDataOld.Type.DARK)
		{
			if (targetType == PokemonDataOld.Type.GHOST || targetType == PokemonDataOld.Type.PSYCHIC)
			{
				return 2f;
			}
			else if (targetType == PokemonDataOld.Type.DARK || targetType == PokemonDataOld.Type.FAIRY ||
					 targetType == PokemonDataOld.Type.FIGHTING)
			{
				return 0.5f;
			}
		}
		else if (attackingType == PokemonDataOld.Type.DRAGON)
		{
			if (targetType == PokemonDataOld.Type.DRAGON)
			{
				return 2f;
			}
			else if (targetType == PokemonDataOld.Type.STEEL)
			{
				return 0.5f;
			}
			else if (targetType == PokemonDataOld.Type.FAIRY)
			{
				return 0f;
			}
		}
		else if (attackingType == PokemonDataOld.Type.ELECTRIC)
		{
			if (targetType == PokemonDataOld.Type.FLYING || targetType == PokemonDataOld.Type.WATER)
			{
				return 2f;
			}
			else if (targetType == PokemonDataOld.Type.DRAGON || targetType == PokemonDataOld.Type.ELECTRIC ||
					 targetType == PokemonDataOld.Type.GRASS)
			{
				return 0.5f;
			}
			else if (targetType == PokemonDataOld.Type.GROUND)
			{
				return 0f;
			}
		}
		else if (attackingType == PokemonDataOld.Type.FAIRY)
		{
			if (targetType == PokemonDataOld.Type.DARK || targetType == PokemonDataOld.Type.DRAGON ||
				targetType == PokemonDataOld.Type.FIGHTING)
			{
				return 2f;
			}
			else if (targetType == PokemonDataOld.Type.FIRE || targetType == PokemonDataOld.Type.POISON ||
					 targetType == PokemonDataOld.Type.STEEL)
			{
				return 0.5f;
			}
		}
		else if (attackingType == PokemonDataOld.Type.FIGHTING)
		{
			if (targetType == PokemonDataOld.Type.DARK || targetType == PokemonDataOld.Type.ICE ||
				targetType == PokemonDataOld.Type.NORMAL || targetType == PokemonDataOld.Type.ROCK ||
				targetType == PokemonDataOld.Type.STEEL)
			{
				return 2f;
			}
			else if (targetType == PokemonDataOld.Type.BUG || targetType == PokemonDataOld.Type.FAIRY ||
					 targetType == PokemonDataOld.Type.FLYING || targetType == PokemonDataOld.Type.POISON ||
					 targetType == PokemonDataOld.Type.PSYCHIC)
			{
				return 0.5f;
			}
			else if (targetType == PokemonDataOld.Type.GHOST)
			{
				return 0f;
			}
		}
		else if (attackingType == PokemonDataOld.Type.FIRE)
		{
			if (targetType == PokemonDataOld.Type.BUG || targetType == PokemonDataOld.Type.GRASS ||
				targetType == PokemonDataOld.Type.ICE || targetType == PokemonDataOld.Type.STEEL)
			{
				return 2f;
			}
			else if (targetType == PokemonDataOld.Type.DRAGON || targetType == PokemonDataOld.Type.FIRE ||
					 targetType == PokemonDataOld.Type.ROCK || targetType == PokemonDataOld.Type.WATER)
			{
				return 0.5f;
			}
		}
		else if (attackingType == PokemonDataOld.Type.FLYING)
		{
			if (targetType == PokemonDataOld.Type.BUG || targetType == PokemonDataOld.Type.FIGHTING ||
				targetType == PokemonDataOld.Type.GRASS)
			{
				return 2f;
			}
			else if (targetType == PokemonDataOld.Type.ELECTRIC || targetType == PokemonDataOld.Type.ROCK ||
					 targetType == PokemonDataOld.Type.STEEL)
			{
				return 0.5f;
			}
		}
		else if (attackingType == PokemonDataOld.Type.GHOST)
		{
			if (targetType == PokemonDataOld.Type.GHOST || targetType == PokemonDataOld.Type.PSYCHIC)
			{
				return 2f;
			}
			else if (targetType == PokemonDataOld.Type.DARK)
			{
				return 0.5f;
			}
			else if (targetType == PokemonDataOld.Type.NORMAL)
			{
				return 0f;
			}
		}
		else if (attackingType == PokemonDataOld.Type.GRASS)
		{
			if (targetType == PokemonDataOld.Type.GROUND || targetType == PokemonDataOld.Type.ROCK ||
				targetType == PokemonDataOld.Type.WATER)
			{
				return 2f;
			}
			else if (targetType == PokemonDataOld.Type.BUG || targetType == PokemonDataOld.Type.DRAGON ||
					 targetType == PokemonDataOld.Type.FIRE || targetType == PokemonDataOld.Type.FLYING ||
					 targetType == PokemonDataOld.Type.GRASS || targetType == PokemonDataOld.Type.POISON ||
					 targetType == PokemonDataOld.Type.STEEL)
			{
				return 0.5f;
			}
		}
		else if (attackingType == PokemonDataOld.Type.GROUND)
		{
			if (targetType == PokemonDataOld.Type.ELECTRIC || targetType == PokemonDataOld.Type.FIRE ||
				targetType == PokemonDataOld.Type.POISON || targetType == PokemonDataOld.Type.ROCK ||
				targetType == PokemonDataOld.Type.STEEL)
			{
				return 2f;
			}
			else if (targetType == PokemonDataOld.Type.BUG || targetType == PokemonDataOld.Type.GRASS)
			{
				return 0.5f;
			}
			else if (targetType == PokemonDataOld.Type.FLYING)
			{
				return 0f;
			}
		}
		else if (attackingType == PokemonDataOld.Type.ICE)
		{
			if (targetType == PokemonDataOld.Type.DRAGON || targetType == PokemonDataOld.Type.FLYING ||
				targetType == PokemonDataOld.Type.GRASS || targetType == PokemonDataOld.Type.GROUND)
			{
				return 2f;
			}
			else if (targetType == PokemonDataOld.Type.FIRE || targetType == PokemonDataOld.Type.ICE ||
					 targetType == PokemonDataOld.Type.STEEL || targetType == PokemonDataOld.Type.WATER)
			{
				return 0.5f;
			}
		}
		else if (attackingType == PokemonDataOld.Type.NORMAL)
		{
			if (targetType == PokemonDataOld.Type.ROCK || targetType == PokemonDataOld.Type.STEEL)
			{
				return 0.5f;
			}
			else if (targetType == PokemonDataOld.Type.GHOST)
			{
				return 0f;
			}
		}
		else if (attackingType == PokemonDataOld.Type.POISON)
		{
			if (targetType == PokemonDataOld.Type.FAIRY || targetType == PokemonDataOld.Type.GRASS)
			{
				return 2f;
			}
			else if (targetType == PokemonDataOld.Type.POISON || targetType == PokemonDataOld.Type.GROUND ||
					 targetType == PokemonDataOld.Type.ROCK || targetType == PokemonDataOld.Type.GHOST)
			{
				return 0.5f;
			}
			else if (targetType == PokemonDataOld.Type.STEEL)
			{
				return 0f;
			}
		}
		else if (attackingType == PokemonDataOld.Type.PSYCHIC)
		{
			if (targetType == PokemonDataOld.Type.FIGHTING || targetType == PokemonDataOld.Type.POISON)
			{
				return 2f;
			}
			else if (targetType == PokemonDataOld.Type.PSYCHIC || targetType == PokemonDataOld.Type.STEEL)
			{
				return 0.5f;
			}
			else if (targetType == PokemonDataOld.Type.DARK)
			{
				return 0f;
			}
		}
		else if (attackingType == PokemonDataOld.Type.ROCK)
		{
			if (targetType == PokemonDataOld.Type.BUG || targetType == PokemonDataOld.Type.FIRE ||
				targetType == PokemonDataOld.Type.FLYING || targetType == PokemonDataOld.Type.ICE)
			{
				return 2f;
			}
			else if (targetType == PokemonDataOld.Type.FIGHTING || targetType == PokemonDataOld.Type.GROUND ||
					 targetType == PokemonDataOld.Type.STEEL)
			{
				return 0.5f;
			}
		}
		else if (attackingType == PokemonDataOld.Type.STEEL)
		{
			if (targetType == PokemonDataOld.Type.FAIRY || targetType == PokemonDataOld.Type.ICE ||
				targetType == PokemonDataOld.Type.ROCK)
			{
				return 2f;
			}
			else if (targetType == PokemonDataOld.Type.ELECTRIC || targetType == PokemonDataOld.Type.FIRE ||
					 targetType == PokemonDataOld.Type.STEEL || targetType == PokemonDataOld.Type.WATER)
			{
				return 0.5f;
			}
		}
		else if (attackingType == PokemonDataOld.Type.WATER)
		{
			if (targetType == PokemonDataOld.Type.FIRE || targetType == PokemonDataOld.Type.GROUND ||
				targetType == PokemonDataOld.Type.ROCK)
			{
				return 2f;
			}
			else if (targetType == PokemonDataOld.Type.DRAGON || targetType == PokemonDataOld.Type.GRASS ||
					 targetType == PokemonDataOld.Type.WATER)
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
				calculatedPokemonSpeed = (float)pokemonStats[4][i] * calculateStatModifier(pokemonStatsMod[4][i]);
				if (pokemon[i] != null)
				{
					if (pokemon[i].getStatus() == PokemonOld.Status.PARALYZED)
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
	private bool switchPokemon(int switchPosition, PokemonOld newPokemon)
	{
		return switchPokemon(switchPosition, newPokemon, false, false);
	}

	/// Switch Pokemon
	private bool switchPokemon(int switchPosition, PokemonOld newPokemon, bool batonPass)
	{
		return switchPokemon(switchPosition, newPokemon, batonPass, false);
	}

	/// Switch Pokemon
	private bool switchPokemon(int switchPosition, PokemonOld newPokemon, bool batonPass, bool forceSwitch)
	{
		if (newPokemon == null)
		{
			return false;
		}
		if (newPokemon.getStatus() == PokemonOld.Status.FAINTED)
		{
			return false;
		}
		// Return false if any condition is preventing the pokemon from switching out
		if (!forceSwitch)
		{
			//no condition can stop a fainted pokemon from switching out
			if (pokemon[switchPosition] != null)
			{
				if (pokemon[switchPosition].getStatus() != PokemonOld.Status.FAINTED)
				{
				}
			}
		}

		pokemon[switchPosition] = newPokemon;
		pokemonMoveset[switchPosition] = newPokemon.getMoveset();


		//set PokemonData
		updatePokemonStats(switchPosition);
		pokemonAbility[switchPosition] =
			PokemonDatabaseOld.getPokemon(newPokemon.getID()).getAbility(newPokemon.getAbility());
		pokemonType1[switchPosition] = PokemonDatabaseOld.getPokemon(newPokemon.getID()).getType1();
		pokemonType2[switchPosition] = PokemonDatabaseOld.getPokemon(newPokemon.getID()).getType2();
		pokemonType3[switchPosition] = PokemonDataOld.Type.NONE;

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

	private IEnumerator addExp(int position, int exp)
	{
		yield return
			StartCoroutine(drawTextAndWait(pokemon[position].getName() + " gained " + exp + " Exp. Points!", 1.8f, 1.8f))
			;
		Dialog.UndrawDialogBox();

		int expPool = exp;
		while (expPool > 0)
		{
			int expToNextLevel = pokemon[position].getExpNext() - pokemon[position].getExp();

			//if enough exp left to level up
			if (expPool >= expToNextLevel)
			{
				pokemon[position].addExp(expToNextLevel);
				expPool -= expToNextLevel;

				AudioSource fillSource = SfxHandler.Play(fillExpClip);
				yield return StartCoroutine(stretchBar(pokemon0ExpBar, 80f));
				SfxHandler.FadeSource(fillSource, 0.2f);
				SfxHandler.Play(expFullClip);
				yield return new WaitForSeconds(1f);

				updatePokemonStats(position);
				updatePokemonStatsDisplay(position);

				BgmHandler.main.PlayMFX(Resources.Load<AudioClip>("Audio/mfx/GetAverage"));
				yield return
					StartCoroutine(
						drawTextAndWait(
							pokemon[position].getName() + " grew to Level " + pokemon[position].getLevel() + "!", 1.8f,
							1.8f));

				string newMove = pokemon[position].MoveLearnedAtLevel(pokemon[position].getLevel());
				if (!string.IsNullOrEmpty(newMove) && !pokemon[position].HasMove(newMove))
				{
					yield return StartCoroutine(LearnMove(pokemon[position], newMove));
				}

				Dialog.UndrawDialogBox();
				yield return new WaitForSeconds(1f);
			}
			else
			{
				pokemon[position].addExp(expPool);
				expPool = 0;
				float levelStartExp =
					PokemonDatabaseOld.getLevelExp(
						PokemonDatabaseOld.getPokemon(pokemon[position].getID()).getLevelingRate(),
						pokemon[position].getLevel());
				float currentExpMinusStart = pokemon[position].getExp() - levelStartExp;
				float nextLevelExpMinusStart = pokemon[position].getExpNext() - levelStartExp;

				AudioSource fillSource = SfxHandler.Play(fillExpClip);
				yield return
					StartCoroutine(stretchBar(pokemon0ExpBar, 80f * (currentExpMinusStart / nextLevelExpMinusStart)));
				SfxHandler.FadeSource(fillSource, 0.2f);
				yield return new WaitForSeconds(1f);
			}


			yield return null;
		}
	}

	private IEnumerator LearnMove(PokemonOld selectedPokemon, string move)
	{
		int chosenIndex = 1;
		if (chosenIndex == 1)
		{
			bool learning = true;
			while (learning)
			{
				//Moveset is full
				if (selectedPokemon.getMoveCount() == 4)
				{
					yield return
						StartCoroutine(
							drawTextAndWait(selectedPokemon.getName() + " wants to learn the \nmove " + move + "."));
					yield return
						StartCoroutine(
							drawTextAndWait("However, " + selectedPokemon.getName() + " already \nknows four moves."));
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
						StartCoroutine(Scene.main.Summary.control(selectedPokemon, move));
						//Start an empty loop that will only stop when SceneSummary is no longer active (is closed)
						while (Scene.main.Summary.gameObject.activeSelf)
						{
							yield return null;
						}

						string replacedMove = Scene.main.Summary.replacedMove;
						yield return StartCoroutine(ScreenFade.main.Fade(true, ScreenFade.defaultSpeed));

						if (!string.IsNullOrEmpty(replacedMove))
						{
							Dialog.DrawDialogBox();
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
							while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
							{
								yield return null;
							}

							yield return
								StartCoroutine(
									drawTextAndWait(selectedPokemon.getName() + " forgot how to \nuse " + replacedMove +
													"."));
							yield return StartCoroutine(drawTextAndWait("And..."));

							Dialog.DrawDialogBox();
							AudioClip mfx = Resources.Load<AudioClip>("Audio/mfx/GetAverage");
							BgmHandler.main.PlayMFX(mfx);
							StartCoroutine(Dialog.DrawTextSilent(selectedPokemon.getName() + " learned \n" + move + "!"));
							yield return new WaitForSeconds(mfx.length);
							while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
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
					selectedPokemon.addMove(move);

					Dialog.DrawDialogBox();
					AudioClip mfx = Resources.Load<AudioClip>("Audio/mfx/GetAverage");
					BgmHandler.main.PlayMFX(mfx);
					StartCoroutine(Dialog.DrawTextSilent(selectedPokemon.getName() + " learned \n" + move + "!"));
					yield return new WaitForSeconds(mfx.length);
					while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
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
			yield return StartCoroutine(drawTextAndWait(selectedPokemon.getName() + " did not learn \n" + move + "."));
		}
	}


	private void updatePokemonStats(int position)
	{
		//set PokemonData
		pokemonStats[0][position] = pokemon[position].getATK();
		pokemonStats[1][position] = pokemon[position].getDEF();
		pokemonStats[2][position] = pokemon[position].getSPA();
		pokemonStats[3][position] = pokemon[position].getSPD();
		pokemonStats[4][position] = pokemon[position].getSPE();
	}


	/// Apply the Move Effect to the target pokemon if possible (with animation)
	private IEnumerator applyEffect(int attackerPosition, int targetPosition, MoveDataOld.Effect effect, float parameter)
	{
		yield return StartCoroutine(applyEffect(attackerPosition, targetPosition, effect, parameter, true));
	}

	/// Apply the Move Effect to the target pokemon if possible
	private IEnumerator applyEffect(int attackerPosition, int targetPosition, MoveDataOld.Effect effect, float parameter,
		bool animate)
	{
		//most effects won't happen if a target has fainted.
		if (pokemon[targetPosition] != null)
		{
			if (pokemon[targetPosition].getStatus() != PokemonOld.Status.FAINTED)
			{
				if (effect == MoveDataOld.Effect.ATK)
				{
					yield return StartCoroutine(ModifyStat(targetPosition, 0, parameter, animate));
				}
				else if (effect == MoveDataOld.Effect.DEF)
				{
					yield return StartCoroutine(ModifyStat(targetPosition, 1, parameter, animate));
				}
				else if (effect == MoveDataOld.Effect.SPA)
				{
					yield return StartCoroutine(ModifyStat(targetPosition, 2, parameter, animate));
				}
				else if (effect == MoveDataOld.Effect.SPD)
				{
					yield return StartCoroutine(ModifyStat(targetPosition, 3, parameter, animate));
				}
				else if (effect == MoveDataOld.Effect.SPE)
				{
					yield return StartCoroutine(ModifyStat(targetPosition, 4, parameter, animate));
				}
				else if (effect == MoveDataOld.Effect.ACC)
				{
					yield return StartCoroutine(ModifyStat(targetPosition, 5, parameter, animate));
				}
				else if (effect == MoveDataOld.Effect.EVA)
				{
					yield return StartCoroutine(ModifyStat(targetPosition, 6, parameter, animate));
				}
				else if (effect == MoveDataOld.Effect.BURN)
				{
					if (Random.value <= parameter)
					{
						if (pokemon[targetPosition].setStatus(PokemonOld.Status.BURNED))
						{
							yield return
								StartCoroutine(
									drawTextAndWait(
										generatePreString(targetPosition) + pokemon[targetPosition].getName() +
										" was burned!", 2.4f));
						}
					}
				}
				else if (effect == MoveDataOld.Effect.Freeze)
				{
					if (Random.value <= parameter)
					{
						if (pokemon[targetPosition].setStatus(PokemonOld.Status.FROZEN))
						{
							yield return
								StartCoroutine(
									drawTextAndWait(
										generatePreString(targetPosition) + pokemon[targetPosition].getName() +
										" was frozen solid!", 2.4f));
						}
					}
				}
				else if (effect == MoveDataOld.Effect.Paralyze)
				{
					if (Random.value <= parameter)
					{
						if (pokemon[targetPosition].setStatus(PokemonOld.Status.PARALYZED))
						{
							yield return
								StartCoroutine(
									drawTextAndWait(
										generatePreString(targetPosition) + pokemon[targetPosition].getName() +
										" was paralyzed! \\nIt may be unable to move!", 2.4f));
						}
					}
				}
				else if (effect == MoveDataOld.Effect.POISON)
				{
					if (Random.value <= parameter)
					{
						if (pokemon[targetPosition].setStatus(PokemonOld.Status.POISONED))
						{
							yield return
								StartCoroutine(
									drawTextAndWait(
										generatePreString(targetPosition) + pokemon[targetPosition].getName() +
										" was poisoned!", 2.4f));
						}
					}
				}
				else if (effect == MoveDataOld.Effect.Toxic)
				{
					if (Random.value <= parameter)
					{
						if (pokemon[targetPosition].setStatus(PokemonOld.Status.POISONED))
						{
							yield return
								StartCoroutine(
									drawTextAndWait(
										generatePreString(targetPosition) + pokemon[targetPosition].getName() +
										" was badly posioned!", 2.4f));
						}
					}
				}
				else if (effect == MoveDataOld.Effect.SLEEP)
				{
					if (Random.value <= parameter)
					{
						if (pokemon[targetPosition].setStatus(PokemonOld.Status.ASLEEP))
						{
							yield return
								StartCoroutine(
									drawTextAndWait(
										generatePreString(targetPosition) + pokemon[targetPosition].getName() +
										" fell asleep!", 2.4f));
						}
					}
				}
			}
		}
		//effects that happen regardless of target fainting or not
		if (effect == MoveDataOld.Effect.ATKself)
		{
			yield return StartCoroutine(ModifyStat(attackerPosition, 0, parameter, animate));
		}
		else if (effect == MoveDataOld.Effect.DEFself)
		{
			yield return StartCoroutine(ModifyStat(attackerPosition, 1, parameter, animate));
		}
		else if (effect == MoveDataOld.Effect.SPAself)
		{
			yield return StartCoroutine(ModifyStat(attackerPosition, 2, parameter, animate));
		}
		else if (effect == MoveDataOld.Effect.SPDself)
		{
			yield return StartCoroutine(ModifyStat(attackerPosition, 3, parameter, animate));
		}
		else if (effect == MoveDataOld.Effect.SPEself)
		{
			yield return StartCoroutine(ModifyStat(attackerPosition, 4, parameter, animate));
		}
		else if (effect == MoveDataOld.Effect.ACCself)
		{
			yield return StartCoroutine(ModifyStat(attackerPosition, 5, parameter, animate));
		}
		else if (effect == MoveDataOld.Effect.EVAself)
		{
			yield return StartCoroutine(ModifyStat(attackerPosition, 6, parameter, animate));
		}
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
						generatePreString(targetPosition) + pokemon[targetPosition].getName() + "'s " +
						statName[statIndex] + " \\nwon't go any higher!", 2.4f));
			canModify = false;
		}
		else if (pokemonStatsMod[statIndex][targetPosition] <= -6 && parameter < 0)
		{
			//can't go lower
			yield return
				StartCoroutine(
					drawTextAndWait(
						generatePreString(targetPosition) + pokemon[targetPosition].getName() + "'s " +
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
				RawImage overlay = (targetPosition < 3) ? player1Overlay : opponent1Overlay;
				if (parameter > 0)
				{
					SfxHandler.Play(statUpClip);
					StartCoroutine(animateOverlayer(overlay, overlayStatUpTex, -1, 0, 1.2f, 0.3f));
				}
				else if (parameter < 0)
				{
					SfxHandler.Play(statDownClip);
					StartCoroutine(animateOverlayer(overlay, overlayStatDownTex, 1, 0, 1.2f, 0.3f));
				}

				yield return new WaitForSeconds(statUpClip.length + 0.2f);
			}

			if (parameter == 1)
			{
				yield return
					StartCoroutine(
						drawTextAndWait(
							generatePreString(targetPosition) + pokemon[targetPosition].getName() + "'s " +
							statName[statIndex] + " \\nrose!", 2.4f));
			}
			else if (parameter == -1)
			{
				yield return
					StartCoroutine(
						drawTextAndWait(
							generatePreString(targetPosition) + pokemon[targetPosition].getName() + "'s " +
							statName[statIndex] + " \\nfell!", 2.4f));
			}
			else if (parameter == 2)
			{
				yield return
					StartCoroutine(
						drawTextAndWait(
							generatePreString(targetPosition) + pokemon[targetPosition].getName() + "'s " +
							statName[statIndex] + " \\nrose sharply!", 2.4f));
			}
			else if (parameter == -2)
			{
				yield return
					StartCoroutine(
						drawTextAndWait(
							generatePreString(targetPosition) + pokemon[targetPosition].getName() + "'s " +
							statName[statIndex] + " \\nharshly fell!", 2.4f));
			}
			else if (parameter >= 3)
			{
				yield return
					StartCoroutine(
						drawTextAndWait(
							generatePreString(targetPosition) + pokemon[targetPosition].getName() + "'s " +
							statName[statIndex] + " nrose drastically!", 2.4f));
			}
			else if (parameter <= -3)
			{
				yield return
					StartCoroutine(
						drawTextAndWait(
							generatePreString(targetPosition) + pokemon[targetPosition].getName() + "'s " +
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
		if ((!curingStatus && pokemon[index].getCurrentHP() == pokemon[index].getHP()) ||
			(curingStatus &&
			 (pokemon[index].getStatus() == PokemonOld.Status.NONE || pokemon[index].getStatus() == PokemonOld.Status.FAINTED)))
		{
			//no effect
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
				PokemonOld.Status status = pokemon[index].getStatus();
				pokemon[index].healStatus();
				updatePokemonStatsDisplay(index);
				yield return new WaitForSeconds(0.3f);
				if (status == PokemonOld.Status.ASLEEP)
				{
					yield return
						StartCoroutine(drawTextAndWait(
							generatePreString(index) + pokemon[index].getName() + " woke up!", 2.4f));
				}
				else if (status == PokemonOld.Status.BURNED)
				{
					yield return
						StartCoroutine(
							drawTextAndWait(
								generatePreString(index) + pokemon[index].getName() + "'s burn was healed!", 2.4f));
				}
				else if (status == PokemonOld.Status.FROZEN)
				{
					yield return
						StartCoroutine(
							drawTextAndWait(generatePreString(index) + pokemon[index].getName() + " thawed out!", 2.4f))
						;
				}
				else if (status == PokemonOld.Status.PARALYZED)
				{
					yield return
						StartCoroutine(
							drawTextAndWait(
								generatePreString(index) + pokemon[index].getName() + " was cured of its paralysis!",
								2.4f));
				}
				else if (status == PokemonOld.Status.POISONED)
				{
					yield return
						StartCoroutine(
							drawTextAndWait(
								generatePreString(index) + pokemon[index].getName() + " was cured of its poison!", 2.4f))
						;
				}
			}
			else
			{
				//if under 1.01f, heal based on percentage
				if (healAmount < 1.01f)
				{
					healAmount = Mathf.CeilToInt((float)pokemon[index].getHP() * healAmount);
				}

				//Heal the pokemon and record how much HP was healed
				int healedHP = pokemon[index].getCurrentHP();
				pokemon[index].healHP(healAmount);
				healedHP = pokemon[index].getCurrentHP() - healedHP;

				if (index == 0)
				{
					yield return
						StartCoroutine(stretchBar(statsHPBar[index], pokemon[index].getPercentHP() * 48f, 32, true,
							pokemon0CurrentHP, pokemon0CurrentHPShadow, pokemon[index].getCurrentHP()));
				}
				else
				{
					yield return
						StartCoroutine(stretchBar(statsHPBar[index], pokemon[index].getPercentHP() * 48f, 32, true, null,
							null, 0));
				}
				yield return
					StartCoroutine(
						drawTextAndWait(
							generatePreString(index) + pokemon[index].getName() + "'s HP was restored by " + healedHP +
							" point(s).", 2.4f));
			}
		}
	}


	/// Display the a textbox with a message, and wait until Select or Start/Back is pressed.
	private IEnumerator drawTextAndWait(string message)
	{
		yield return StartCoroutine(drawTextAndWait(message, 0, 0, true));
	}

	/// Display the a textbox with a message, and wait a set amount of time or until Select or Start/Back is pressed.
	private IEnumerator drawTextAndWait(string message, float time)
	{
		yield return StartCoroutine(drawTextAndWait(message, time, 0, true));
	}

	/// Display the a textbox with a message, and wait a set amount of time before until Select or Start/Back is pressed.
	private IEnumerator drawTextAndWait(string message, float time, float lockedTime)
	{
		yield return StartCoroutine(drawTextAndWait(message, time, lockedTime, true));
	}

	private IEnumerator drawTextAndWait(string message, bool silent)
	{
		yield return StartCoroutine(drawTextAndWait(message, 0, 0, silent));
	}

	private IEnumerator drawTextAndWait(string message, float time, float lockedTime, bool silent)
	{
		Dialog.DrawDialogBox();
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
			while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back") && Time.time < startTime + time)
			{
				yield return null;
			}
		}
		else
		{
			while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
			{
				yield return null;
			}
		}
	}

	private string generatePreString(int pokemonPosition)
	{
		string preString = "";
		if (pokemonPosition > 2)
		{
			preString = (trainerBattle) ? "The foe's " : "The wild ";
		}
		return preString;
	}


	private float PlayCry(PokemonOld pokemon)
	{
		SfxHandler.Play(pokemon.GetCry(), pokemon.GetCryPitch());
		return pokemon.GetCry().length / pokemon.GetCryPitch();
	}

	private IEnumerator PlayCryAndWait(PokemonOld pokemon)
	{
		yield return new WaitForSeconds(PlayCry(pokemon));
	}


	/// Basic Wild Battle
	public IEnumerator control(PokemonOld wildPokemon)
	{
		yield return StartCoroutine(control(false, new TrainerOld(new PokemonOld[] { wildPokemon }), false));
	}

	/// Basic Trainer Battle
	public IEnumerator control(TrainerOld trainer)
	{
		yield return StartCoroutine(control(true, trainer, false));
	}

	public IEnumerator control(bool isTrainerBattle, TrainerOld trainer, bool healedOnDefeat)
	{
		//Used to compare after the battle to check for evolutions.
		int[] initialLevels = new int[6];
		for (int i = 0; i < initialLevels.Length; i++)
		{
			if (SaveDataOld.currentSave.PC.boxes[0][i] != null)
			{
				initialLevels[i] = SaveDataOld.currentSave.PC.boxes[0][i].getLevel();
			}
		}

		trainerBattle = isTrainerBattle;
		PokemonOld[] opponentParty = trainer.GetParty();
		string opponentName = trainer.GetName();

		//GET BATTLE BACKGROUNDS
		int currentTileTag = PlayerMovement.player.currentMap.getTileTag(PlayerMovement.player.transform.position);
		Debug.Log(currentTileTag);
		background.sprite = PlayerMovement.player.accessedMapSettings.getBattleBackground(currentTileTag);

		playerBase.sprite = PlayerMovement.player.accessedMapSettings.getBattleBase(currentTileTag);
		opponentBase.sprite = playerBase.sprite;


		//Set Trainer Sprites
		trainer1Animation = new Sprite[] { Resources.Load<Sprite>("null") };
		if (trainerBattle)
		{
			trainer1Animation = trainer.GetSprites();
		}
		playerTrainer1Animation =
			Resources.LoadAll<Sprite>("PlayerSprites/" + SaveDataOld.currentSave.getPlayerSpritePrefix() + "back");
		playerTrainerSprite1.sprite = playerTrainer1Animation[0];
		//Note: the player animation should NEVER have no sprites 
		if (trainer1Animation.Length > 0)
		{
			trainerSprite1.sprite = trainer1Animation[0];
		}
		else
		{
			trainerSprite1.sprite = Resources.Load<Sprite>("null");
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

		for (int i = 0; i < 6; i++)
		{
			if (SaveDataOld.currentSave.PC.boxes[0][i] != null)
			{
				if (SaveDataOld.currentSave.PC.boxes[0][i].getStatus() != PokemonOld.Status.FAINTED)
				{
					switchPokemon(0, SaveDataOld.currentSave.PC.boxes[0][i], false, true);
					i = 6;
				}
			}
		}
		switchPokemon(3, opponentParty[0], false, true);


		player1Animation = pokemon[0].GetBackAnim_();
		opponent1Animation = pokemon[3].GetFrontAnim_();

		updatePokemonStatsDisplay(0);
		updatePokemonStatsDisplay(3);

		if (pokemon[0] != null)
		{
			updateMovesetDisplay(pokemonMoveset[0], pokemon[0].getPP(), pokemon[0].getMaxPP());
		}

		//Animate the Pokemon being released into battle
		player1.transform.parent.parent.gameObject.SetActive(false);
		opponent1.transform.parent.parent.gameObject.SetActive(false);
		player1Overlay.gameObject.SetActive(false);
		opponent1Overlay.gameObject.SetActive(false);


		animateOpponent1 = StartCoroutine(animatePokemon(opponent1, opponent1Animation));
		animatePlayer1 = StartCoroutine(animatePokemon(player1, player1Animation));

		pokemonStatsDisplay[0].gameObject.SetActive(false);
		pokemonStatsDisplay[3].gameObject.SetActive(false);

		hidePartyBar(true);
		hidePartyBar(false);

		//		Debug.Log(pokemon[0].getName()+": HP: "+pokemon[0].getHP()+"ATK: "+pokemon[0].getATK()+"DEF: "+pokemon[0].getDEF()+"SPA: "+pokemon[0].getSPA()+"SPD: "+pokemon[0].getSPD()+"SPE:"+pokemon[0].getSPE());
		//		Debug.Log(pokemon[3].getName()+": HP: "+pokemon[3].getHP()+"ATK: "+pokemon[3].getATK()+"DEF: "+pokemon[3].getDEF()+"SPA: "+pokemon[3].getSPA()+"SPD: "+pokemon[3].getSPD()+"SPE:"+pokemon[3].getSPE());


		if (trainerBattle)
		{
			StartCoroutine(ScreenFade.main.Fade(true, 1.2f));
			StartCoroutine(slidePokemon(opponentBase, opponent1, false, false, new Vector3(100, 0, 0)));
			StartCoroutine(slidePokemon(playerBase, player1, false, true, new Vector3(-80, -64, 0)));

			yield return new WaitForSeconds(0.9f);
			StartCoroutine(displayPartyBar(true, opponentParty));
			StartCoroutine(displayPartyBar(false, SaveDataOld.currentSave.PC.boxes[0]));

			yield return StartCoroutine(drawTextAndWait(opponentName + " wants to fight!", 2.6f, 2.6f));

			Dialog.DrawDialogBox();
			StartCoroutine(Dialog.DrawTextSilent(opponentName + " sent out " + pokemon[3].getName() + "!"));
			StartCoroutine(dismissPartyBar(true));
			yield return StartCoroutine(slideTrainer(opponentBase, trainerSprite1, true, true));
			yield return StartCoroutine(releasePokemon(opponent1));
			PlayCry(pokemon[3]);
			yield return new WaitForSeconds(0.3f);
			yield return StartCoroutine(slidePokemonStats(3, false));
			yield return new WaitForSeconds(0.9f);

			Dialog.DrawDialogBox();
			StartCoroutine(Dialog.DrawTextSilent("Go " + pokemon[0].getName() + "!"));
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
		}
		else
		{
			player1.transform.parent.parent.gameObject.SetActive(false);
			opponent1.transform.parent.parent.gameObject.SetActive(false);

			StartCoroutine(ScreenFade.main.Fade(true, ScreenFade.slowedSpeed));
			StartCoroutine(slidePokemon(opponentBase, opponent1, true, false, new Vector3(92, 0, 0)));
			yield return StartCoroutine(slidePokemon(playerBase, player1, false, true, new Vector3(-80, -64, 0)));
			Dialog.DrawDialogBox();
			StartCoroutine(Dialog.DrawTextSilent("A wild " + pokemon[3].getName() + " appeared!"));
			PlayCry(pokemon[3]);
			yield return StartCoroutine(slidePokemonStats(3, false));
			yield return new WaitForSeconds(1.2f);

			Dialog.DrawDialogBox();
			StartCoroutine(Dialog.DrawTextSilent("Go " + pokemon[0].getName() + "!"));
			StartCoroutine(animatePlayerThrow(playerTrainerSprite1, playerTrainer1Animation, true));
			yield return new WaitForSeconds(0.2f);
			StartCoroutine(slideTrainer(playerBase, playerTrainerSprite1, false, true));
			yield return new WaitForSeconds(0.5f);
			yield return StartCoroutine(releasePokemon(player1));
			PlayCry(pokemon[0]);
			yield return new WaitForSeconds(0.3f);
			yield return StartCoroutine(slidePokemonStats(0, false));
			yield return new WaitForSeconds(0.9f);
			Dialog.UndrawDialogBox();
		}
		//


		updateCurrentTask(0);

		int playerFleeAttempts = 0;
		while (running)
		{
			//Reset Turn Tasks
			command = new CommandType[6];
			commandMove = new MoveDataOld[6];
			commandTarget = new int[6];
			commandItem = new ItemDataOld[6];
			commandPokemon = new PokemonOld[6];
			//

			//Reset Turn Feedback
			pokemonHasMoved = new bool[6];


			if (pokemon[0] != null)
			{
				updateMovesetDisplay(pokemonMoveset[0], pokemon[0].getPP(), pokemon[0].getMaxPP());
			}

			updatePokemonStatsDisplay(0);
			updatePokemonStatsDisplay(3);
			updateCurrentTask(0);

			//		Debug.Log(pokemon[0].getName()+": HP: "+pokemon[0].getHP()+"ATK: "+pokemon[0].getATK()+"DEF: "+pokemon[0].getDEF()+"SPA: "+pokemon[0].getSPA()+"SPD: "+pokemon[0].getSPD()+"SPE:"+pokemon[0].getSPE());
			//		Debug.Log(pokemon[3].getName()+": HP: "+pokemon[3].getHP()+"ATK: "+pokemon[3].getATK()+"DEF: "+pokemon[3].getDEF()+"SPA: "+pokemon[3].getSPA()+"SPD: "+pokemon[3].getSPD()+"SPE:"+pokemon[3].getSPE());

			////////////////////////////////////////
			/// Task Selection State
			////////////////////////////////////////
			runState = true;
			while (runState)
			{
				//DEBUG OVERLAY TEXTURES
				if (Input.GetKeyDown(KeyCode.Y))
				{
					StartCoroutine(animateOverlayer(opponent1Overlay, overlayHealTex, -1f, 0, 1.2f, 0.3f));
				}
				if (Input.GetKeyDown(KeyCode.U))
				{
					StartCoroutine(animateOverlayer(opponent1Overlay, overlayStatUpTex, -1f, 0, 1.2f, 0.3f));
				}
				if (Input.GetKeyDown(KeyCode.I))
				{
					StartCoroutine(animateOverlayer(opponent1Overlay, overlayStatDownTex, 1f, 0, 1.2f, 0.3f));
				}

				//// NAVIGATE MAIN OPTIONS ////

				if (Input.GetAxisRaw("Horizontal") < 0)
				{
					if (taskPosition > 0 && taskPosition != 3)
					{
						updateSelectedTask(taskPosition - 1);
						SfxHandler.Play(scrollClip);
						yield return new WaitForSeconds(0.2f);
					}
				}
				else if (Input.GetAxisRaw("Horizontal") > 0)
				{
					if (taskPosition < 5 && taskPosition != 2)
					{
						updateSelectedTask(taskPosition + 1);
						SfxHandler.Play(scrollClip);
						yield return new WaitForSeconds(0.2f);
					}
				}
				else if (Input.GetAxisRaw("Vertical") > 0)
				{
					if (taskPosition > 2)
					{
						if (taskPosition == 4)
						{
							//if run selected
							updateSelectedTask(taskPosition - 3);
							SfxHandler.Play(scrollClip);
							yield return new WaitForSeconds(0.2f);
						}
						else
						{
							taskPosition -= 3;
						}
					}
				}
				else if (Input.GetAxisRaw("Vertical") < 0)
				{
					if (taskPosition < 3)
					{
						if (taskPosition == 1)
						{
							//if fight selected
							updateSelectedTask(taskPosition + 3);
							SfxHandler.Play(scrollClip);
							yield return new WaitForSeconds(0.2f);
						}
						else
						{
							taskPosition += 3;
						}
					}
				}
				else if (Input.GetButtonDown("Select"))
				{
					//// NAVIGATE MOVESET OPTIONS ////

					int currentPokemon = 0;

					if (taskPosition == 1)
					{
						updateCurrentTask(1);
						SfxHandler.Play(selectClip);
						yield return null;

						//while still in Move Selection menu
						while (currentTask == 1)
						{
							if (Input.GetAxisRaw("Horizontal") < 0)
							{
								if (movePosition == 1)
								{
									if (canMegaEvolve)
									{
										updateSelectedMove(0);
										SfxHandler.Play(scrollClip);
										yield return new WaitForSeconds(0.2f);
									}
									else
									{
										updateSelectedMove(3);
										SfxHandler.Play(scrollClip);
										yield return new WaitForSeconds(0.2f);
									}
								}
								else if (movePosition > 1 && movePosition != 3)
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
									else
									{
										updateSelectedMove(movePosition - 1);
										SfxHandler.Play(scrollClip);
										yield return new WaitForSeconds(0.2f);
									}
								}
							}
							else if (Input.GetAxisRaw("Horizontal") > 0)
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
										else
										{
											updateSelectedMove(1);
											SfxHandler.Play(scrollClip);
											yield return new WaitForSeconds(0.2f);
										}
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
							else if (Input.GetAxisRaw("Vertical") > 0)
							{
								if (movePosition == 3)
								{
									if (canMegaEvolve)
									{
										updateSelectedMove(movePosition - 3);
										SfxHandler.Play(scrollClip);
										yield return new WaitForSeconds(0.2f);
									}
									else
									{
										//otherwise, go down to return
										updateSelectedMove(1);
										SfxHandler.Play(scrollClip);
										yield return new WaitForSeconds(0.2f);
									}
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
							else if (Input.GetAxisRaw("Vertical") < 0)
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
										else
										{
											//otherwise, go down to return
											updateSelectedMove(3);
											SfxHandler.Play(scrollClip);
											yield return new WaitForSeconds(0.2f);
										}
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
										else
										{
											//otherwise, go down to return
											updateSelectedMove(3);
											SfxHandler.Play(scrollClip);
											yield return new WaitForSeconds(0.2f);
										}
									}
									else
									{
										updateSelectedMove(movePosition + 3);
										SfxHandler.Play(scrollClip);
										yield return new WaitForSeconds(0.2f);
									}
								}
							}
							else if (Input.GetButtonDown("Select"))
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
										int[] move = new int[] { 0, 0, 1, 0, 2, 3 };
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
									SfxHandler.Play(selectClip);
									command[currentPokemon] = CommandType.Move;
									updateCurrentTask(-1);
								}
							}
							else if (Input.GetButtonDown("Back"))
							{
								SfxHandler.Play(selectClip);
								updateCurrentTask(0);
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
							if (Input.GetAxisRaw("Vertical") < 0)
							{
								if (bagCategoryPosition < 4)
								{
									updateSelectedBagCategory(bagCategoryPosition + 2);
									SfxHandler.Play(scrollClip);
									yield return new WaitForSeconds(0.2f);
								}
							}
							else if (Input.GetAxisRaw("Horizontal") > 0)
							{
								if (bagCategoryPosition == 0 || bagCategoryPosition == 2 || bagCategoryPosition == 4)
								{
									updateSelectedBagCategory(bagCategoryPosition + 1);
									SfxHandler.Play(scrollClip);
									yield return new WaitForSeconds(0.2f);
								}
							}
							else if (Input.GetAxisRaw("Horizontal") < 0)
							{
								if (bagCategoryPosition == 1 || bagCategoryPosition == 3 || bagCategoryPosition == 5)
								{
									updateSelectedBagCategory(bagCategoryPosition - 1);
									SfxHandler.Play(scrollClip);
									yield return new WaitForSeconds(0.2f);
								}
							}
							else if (Input.GetAxisRaw("Vertical") > 0)
							{
								if (bagCategoryPosition > 1)
								{
									updateSelectedBagCategory(bagCategoryPosition - 2);
									SfxHandler.Play(scrollClip);
									yield return new WaitForSeconds(0.2f);
								}
							}
							else if (Input.GetButtonDown("Select"))
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
										if (Input.GetAxisRaw("Vertical") < 0)
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
										else if (Input.GetAxisRaw("Horizontal") > 0)
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
										else if (Input.GetAxisRaw("Horizontal") < 0)
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
										else if (Input.GetAxisRaw("Vertical") > 0)
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
										else if (Input.GetButtonDown("Select"))
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
												ItemDataOld selectedItem =
													ItemDatabaseOld.getItem(
														itemListString[itemListPosition + (8 * itemListPagePosition)]);
												//Check item can be used
												if (selectedItem.getItemEffect() == ItemDataOld.ItemEffect.HP)
												{
													//Check target pokemon's health is not full
													int target = 0; //target selection not yet implemented
													if (pokemon[target].getCurrentHP() < pokemon[target].getHP())
													{
														commandItem[currentPokemon] = selectedItem;
														commandTarget[currentPokemon] = target;
														SaveDataOld.currentSave.Bag.removeItem(selectedItem.getName(), 1);
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
												else if (selectedItem.getItemEffect() == ItemDataOld.ItemEffect.STATUS)
												{
													int target = 0; //target selection not yet implemented
																	//Check target pokemon has the status the item cures
													string statusCurer = selectedItem.getStringParameter().ToUpper();
													//if an ALL is used, set it to cure anything but FAINTED or NONE.
													if (statusCurer == "ALL" &&
														pokemon[target].getStatus().ToString() != "FAINTED" &&
														pokemon[target].getStatus().ToString() != "NONE")
													{
														statusCurer = pokemon[target].getStatus().ToString();
													}

													if (pokemon[target].getStatus().ToString() == statusCurer)
													{
														commandItem[currentPokemon] = selectedItem;
														commandTarget[currentPokemon] = target;
														SaveDataOld.currentSave.Bag.removeItem(
															itemListString[itemListPosition], 1);
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
													commandItem[currentPokemon] =
														ItemDatabaseOld.getItem(itemListString[itemListPosition]);
													SaveDataOld.currentSave.Bag.removeItem(
														itemListString[itemListPosition], 1);
													runState = false;
												}
												if (!runState)
												{
													//if an item was chosen.
													SfxHandler.Play(selectClip);
													command[currentPokemon] = CommandType.Item;
													updateCurrentTask(-1);
												}
											}
										}
										else if (Input.GetButtonDown("Back"))
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
							else if (Input.GetButtonDown("Back"))
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
							if (Input.GetAxisRaw("Vertical") < 0)
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
							else if (Input.GetAxisRaw("Horizontal") > 0)
							{
								if (pokePartyPosition < 6)
								{
									updateSelectedPokemonSlot(pokePartyPosition + 1);
									SfxHandler.Play(scrollClip);
									yield return new WaitForSeconds(0.2f);
								}
							}
							else if (Input.GetAxisRaw("Horizontal") < 0)
							{
								if (pokePartyPosition > 0)
								{
									updateSelectedPokemonSlot(pokePartyPosition - 1);
									SfxHandler.Play(scrollClip);
									yield return new WaitForSeconds(0.2f);
								}
							}
							else if (Input.GetAxisRaw("Vertical") > 0)
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
							else if (Input.GetButtonDown("Select"))
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
										if (Input.GetAxisRaw("Vertical") < 0)
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
													SaveDataOld.currentSave.PC.boxes[0][pokePartyPosition]);
												yield return new WaitForSeconds(0.2f);
											}
										}
										else if (Input.GetAxisRaw("Horizontal") > 0)
										{
											if (summaryPosition < 2)
											{
												summaryPosition = updateSummaryPosition(summaryPosition + 1);
												SfxHandler.Play(scrollClip);
												yield return new WaitForSeconds(0.2f);
											}
										}
										else if (Input.GetAxisRaw("Horizontal") < 0)
										{
											if (summaryPosition > 0)
											{
												summaryPosition = updateSummaryPosition(summaryPosition - 1);
												SfxHandler.Play(scrollClip);
												yield return new WaitForSeconds(0.2f);
											}
										}
										else if (Input.GetAxisRaw("Vertical") > 0)
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
													SaveDataOld.currentSave.PC.boxes[0][pokePartyPosition]);
												yield return new WaitForSeconds(0.2f);
											}
										}
										else if (Input.GetButtonDown("Select"))
										{
											if (summaryPosition == 0)
											{
												if (SaveDataOld.currentSave.PC.boxes[0][pokePartyPosition].getStatus() !=
													PokemonOld.Status.FAINTED)
												{
													//check that pokemon is not on the field
													bool notOnField = true;
													for (int i = 0; i < pokemonPerSide; i++)
													{
														if (SaveDataOld.currentSave.PC.boxes[0][pokePartyPosition] ==
															pokemon[i])
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
															SaveDataOld.currentSave.PC.boxes[0][pokePartyPosition];
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
																	SaveDataOld.currentSave.PC.boxes[0][pokePartyPosition]
																		.getName() + " is already fighting!"));
														Dialog.UndrawDialogBox();
													}
												}
												else
												{
													yield return
														StartCoroutine(
															drawTextAndWait(
																SaveDataOld.currentSave.PC.boxes[0][pokePartyPosition]
																	.getName() + " is unable to fight!"));
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
													if (Input.GetAxisRaw("Vertical") < 0)
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
													else if (Input.GetAxisRaw("Horizontal") > 0)
													{
														if (movesPosition != 1 || movesPosition != 3 ||
															movesPosition != 6)
														{
															movesPosition = updateMovesPosition(movesPosition + 1);
															SfxHandler.Play(scrollClip);
															yield return new WaitForSeconds(0.2f);
														}
													}
													else if (Input.GetAxisRaw("Horizontal") < 0)
													{
														if (movesPosition == 1 || movesPosition == 3 ||
															movesPosition > 4)
														{
															movesPosition = updateMovesPosition(movesPosition - 1);
															SfxHandler.Play(scrollClip);
															yield return new WaitForSeconds(0.2f);
														}
													}
													else if (Input.GetAxisRaw("Vertical") > 0)
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
													else if (Input.GetButtonDown("Select"))
													{
														if (movesPosition == 4)
														{
															if (
																SaveDataOld.currentSave.PC.boxes[0][pokePartyPosition]
																	.getStatus() != PokemonOld.Status.FAINTED)
															{
																//check that pokemon is not on the field
																bool notOnField = true;
																for (int i = 0; i < pokemonPerSide; i++)
																{
																	if (
																		SaveDataOld.currentSave.PC.boxes[0][
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
																		SaveDataOld.currentSave.PC.boxes[0][
																			pokePartyPosition];
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
																				SaveDataOld.currentSave.PC.boxes[0][
																					pokePartyPosition].getName() +
																				" is already fighting!"));
																	Dialog.UndrawDialogBox();
																}
															}
															else
															{
																yield return
																	StartCoroutine(
																		drawTextAndWait(
																			SaveDataOld.currentSave.PC.boxes[0][
																				pokePartyPosition].getName() +
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
													else if (Input.GetButtonDown("Back"))
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
										else if (Input.GetButtonDown("Back"))
										{
											updateCurrentTask(3);
											SfxHandler.Play(selectClip);
											yield return new WaitForSeconds(0.2f);
										}

										yield return null;
									}
								}
							}
							else if (Input.GetButtonDown("Back"))
							{
								SfxHandler.Play(selectClip);
								updateCurrentTask(0);
							}

							yield return null;
						}
					}
				}
				else if (Input.GetButtonDown("Back"))
				{
				}

				yield return null;
			}


			////////////////////////////////////////
			/// AI turn selection
			////////////////////////////////////////

			//AI not yet implemented properly.
			//the following code randomly chooses a move to use with no further thought.
			for (int i = 0; i < pokemonPerSide; i++)
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
						Debug.Log(commandMove[pi].getName() + ", PP: " + pokemon[pi].getPP(AImoveIndex));
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
						if (command[movingPokemon] == CommandType.Flee)
						{
							//RUN
							if (movingPokemon < 3)
							{
								//player attemps escape
								playerFleeAttempts += 1;

								int fleeChance = (pokemon[movingPokemon].getSPE() * 128) / pokemon[3].getSPE() +
												 30 * playerFleeAttempts;
								if (Random.Range(0, 256) < fleeChance)
								{
									running = false;

									SfxHandler.Play(runClip);
									Dialog.DrawDialogBox();
									yield return StartCoroutine(Dialog.DrawTextSilent("Got away safely!"));
									while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
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
								if (commandItem[movingPokemon].getItemEffect() == ItemDataOld.ItemEffect.BALL)
								{
									//debug autoselect targetIndex (target selection not yet implemented)
									int targetIndex = 3;
									//

									//pokeball animation not yet implemented
									yield return
										StartCoroutine(
											drawTextAndWait(
												SaveDataOld.currentSave.playerName + " used one " +
												commandItem[movingPokemon].getName() + "!", 2.4f));
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
										float ballRate = (float)commandItem[movingPokemon].getFloatParameter();
										float catchRate =
											(float)
											PokemonDatabaseOld.getPokemon(pokemon[targetIndex].getID()).getCatchRate();
										float statusRate = 1f;
										if ((pokemon[targetIndex].getStatus() != PokemonOld.Status.NONE))
										{
											statusRate = (pokemon[targetIndex].getStatus() == PokemonOld.Status.ASLEEP ||
														  pokemon[targetIndex].getStatus() == PokemonOld.Status.FROZEN)
												? 2.5f
												: 1.5f;
										}

										int modifiedRate =
											Mathf.FloorToInt(((3 * (float)pokemon[targetIndex].getHP() -
															   2 * (float)pokemon[targetIndex].getCurrentHP())
															  * catchRate * ballRate) /
															 (3 * (float)pokemon[targetIndex].getHP()) * statusRate);

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
											Debug.Log("Caught the " + pokemon[targetIndex].getName());
											running = false;

											//pokeball animation not yet implemented
											yield return StartCoroutine(faintPokemonAnimation(opponent1));
											yield return new WaitForSeconds(1f);

											yield return
												StartCoroutine(
													drawTextAndWait(
														generatePreString(targetIndex) + pokemon[targetIndex].getName() +
														" \\nwas caught!", 2.4f));

											Dialog.DrawDialogBox();
											yield return
												StartCoroutine(
													Dialog.DrawTextSilent(
														"Would you like to give a nickname \\nto your new " +
														pokemon[targetIndex].getName() + "?"));
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
													pokemon[targetIndex].getGender(), pokemon[targetIndex].GetIcons_()));
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
											Debug.Log("CurrentHP" + pokemon[targetIndex].getCurrentHP());
											SaveDataOld.currentSave.PC.addPokemon(new PokemonOld(pokemon[targetIndex],
												nickname, commandItem[movingPokemon].getName()));
										}

										Dialog.UndrawDialogBox();
									}
								}
								else if (commandItem[movingPokemon].getItemEffect() == ItemDataOld.ItemEffect.HP)
								{
									//commandTarget refers to the field position, healing a party member takes place before the turn.
									if (commandTarget[movingPokemon] < 3)
									{
										//if target is player
										yield return
											StartCoroutine(
												drawTextAndWait(
													SaveDataOld.currentSave.playerName + " used the " +
													commandItem[movingPokemon].getName() + "!", 2.4f));
										yield return
											StartCoroutine(Heal(commandTarget[movingPokemon],
												commandItem[movingPokemon].getFloatParameter()));
									}
								}
								else if (commandItem[movingPokemon].getItemEffect() == ItemDataOld.ItemEffect.STATUS)
								{
									//commandTarget refers to the field position, curing a party member takes place before the turn.
									if (commandTarget[movingPokemon] < 3)
									{
										//if target is player
										yield return
											StartCoroutine(
												drawTextAndWait(
													SaveDataOld.currentSave.playerName + " used the " +
													commandItem[movingPokemon].getName() + "!", 2.4f));
										yield return StartCoroutine(Heal(commandTarget[movingPokemon], true));
									}
								}
								else
								{
									//undefined effect
									yield return
										StartCoroutine(
											drawTextAndWait(
												SaveDataOld.currentSave.playerName + " used " +
												commandItem[movingPokemon].getName() + "!", 2.4f));
								}
							}
							else
							{
								yield return
									StartCoroutine(
										drawTextAndWait(
											opponentName + " used " + commandItem[movingPokemon].getName() + "!", 2.4f))
									;
							}

							pokemonHasMoved[movingPokemon] = true;
						}
						else if (command[movingPokemon] == CommandType.Move)
						{
							//MOVE
							//debug autoselect targetIndex (target selection not yet implemented)
							int targetIndex = 3;
							if (commandMove[movingPokemon].getTarget() == MoveDataOld.Target.SELF ||
								commandMove[movingPokemon].getTarget() == MoveDataOld.Target.ADJACENTALLYSELF)
							{
								targetIndex = movingPokemon;
							}
							else
							{
								if (movingPokemon > 2)
								{
									targetIndex = 0;
								}
							}
							//

							if (pokemon[movingPokemon].getStatus() != PokemonOld.Status.FAINTED)
							{
								//calculate and test accuracy
								float accuracy = commandMove[movingPokemon].getAccuracy() *
												 calculateAccuracyModifier(pokemonStatsMod[5][movingPokemon]) /
												 calculateAccuracyModifier(pokemonStatsMod[6][targetIndex]);
								bool canMove = true;
								if (pokemon[movingPokemon].getStatus() == PokemonOld.Status.PARALYZED)
								{
									if (Random.value > 0.75f)
									{
										yield return
											StartCoroutine(
												drawTextAndWait(
													generatePreString(movingPokemon) + pokemon[movingPokemon].getName() +
													" is paralyzed! \\nIt can't move!", 2.4f));
										canMove = false;
									}
								}
								else if (pokemon[movingPokemon].getStatus() == PokemonOld.Status.FROZEN)
								{
									if (Random.value > 0.2f)
									{
										yield return
											StartCoroutine(
												drawTextAndWait(
													generatePreString(movingPokemon) + pokemon[movingPokemon].getName() +
													" is \\nfrozen solid!", 2.4f));
										canMove = false;
									}
									else
									{
										pokemon[movingPokemon].setStatus(PokemonOld.Status.NONE);
										updatePokemonStatsDisplay(movingPokemon);
										yield return
											StartCoroutine(
												drawTextAndWait(
													generatePreString(movingPokemon) + pokemon[movingPokemon].getName() +
													" thawed out!", 2.4f));
									}
								}
								else if (pokemon[movingPokemon].getStatus() == PokemonOld.Status.ASLEEP)
								{
									pokemon[movingPokemon].removeSleepTurn();
									if (pokemon[movingPokemon].getStatus() == PokemonOld.Status.ASLEEP)
									{
										yield return
											StartCoroutine(
												drawTextAndWait(
													generatePreString(movingPokemon) + pokemon[movingPokemon].getName() +
													" is \\nfast asleep.", 2.4f));
										canMove = false;
									}
									else
									{
										updatePokemonStatsDisplay(movingPokemon);
										yield return
											StartCoroutine(
												drawTextAndWait(
													generatePreString(movingPokemon) + pokemon[movingPokemon].getName() +
													" woke up!", 2.4f));
									}
								}
								if (canMove)
								{
									//use the move
									//deduct PP from the move
									pokemon[movingPokemon].removePP(commandMove[movingPokemon].getName(), 1);
									yield return
										StartCoroutine(
											drawTextAndWait(
												generatePreString(movingPokemon) + pokemon[movingPokemon].getName() +
												" used " + commandMove[movingPokemon].getName() + "!", 1.2f, 1.2f));

									//adjust for accuracy
									if (accuracy != 0 && Random.value > accuracy)
									{
										//if missed, provide missed feedback
										yield return
											StartCoroutine(
												drawTextAndWait(
													generatePreString(movingPokemon) + pokemon[movingPokemon].getName() +
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
									if (commandMove[movingPokemon].hasMoveEffect(MoveDataOld.Effect.Heal))
									{
										yield return
											StartCoroutine(Heal(targetIndex,
												commandMove[movingPokemon].getMoveParameter(MoveDataOld.Effect.Heal)));
									}
									else if (commandMove[movingPokemon].hasMoveEffect(MoveDataOld.Effect.SetDamage))
									{
										damageToDeal =
											commandMove[movingPokemon].getMoveParameter(MoveDataOld.Effect.SetDamage);
										//if parameter is 0, then use the pokemon's level
										if (damageToDeal == 0)
										{
											damageToDeal = pokemon[movingPokemon].getLevel();
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
										//float damageBeforeMods = damageToDeal;
										if (commandMove[movingPokemon].getCategory() == MoveDataOld.Category.PHYSICAL)
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
												if (pokemon[movingPokemon].getStatus() == PokemonOld.Status.BURNED)
												{
													damageToDeal /= 2f;
												}
											}
										}
										else if (commandMove[movingPokemon].getCategory() == MoveDataOld.Category.SPECIAL)
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
									int DEBUG_beforeHP = pokemon[targetIndex].getCurrentHP();
									pokemon[targetIndex].removeHP(damageToDeal);
									Debug.Log(DEBUG_beforeHP + " - " + damageToDeal + " = " +
											  pokemon[targetIndex].getCurrentHP());

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
									}

									if (targetIndex == 0)
									{
										//if player pokemon 0 (only stats bar to display HP text)
										yield return
											StartCoroutine(stretchBar(statsHPBar[targetIndex],
												Mathf.CeilToInt(pokemon[targetIndex].getPercentHP() * 48f), 32f, true,
												pokemon0CurrentHP, pokemon0CurrentHPShadow,
												pokemon[targetIndex].getCurrentHP()));
									}
									else
									{
										yield return
											StartCoroutine(stretchBar(statsHPBar[targetIndex],
												Mathf.CeilToInt(pokemon[targetIndex].getPercentHP() * 48f), 32f, true,
												null, null, 0));
									}
									yield return new WaitForSeconds(0.4f);

									updatePokemonStatsDisplay(targetIndex);

									//Feedback on the damage dealt
									if (superEffectiveModifier == 0)
									{
										yield return StartCoroutine(drawTextAndWait("It had no effect...", 2.4f));
									}
									else if (commandMove[movingPokemon].getCategory() != MoveDataOld.Category.STATUS)
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

									//Faint the target if nessecary
									if (pokemon[targetIndex].getStatus() == PokemonOld.Status.FAINTED)
									{
										//debug = array of GUITextures not yet implemented
										yield return
											StartCoroutine(
												drawTextAndWait(
													generatePreString(targetIndex) + pokemon[targetIndex].getName() +
													" fainted!", 2.4f));
										Dialog.UndrawDialogBox();
										yield return new WaitForSeconds(0.2f);
										yield return new WaitForSeconds(PlayCry(pokemon[targetIndex]));
										//flexible faint animtions not yet implemented
										if (targetIndex == 0)
										{
											StartCoroutine(slidePokemonStats(0, true));
											yield return StartCoroutine(faintPokemonAnimation(player1));
										}
										else if (targetIndex == 3)
										{
											StartCoroutine(slidePokemonStats(3, true));
											yield return StartCoroutine(faintPokemonAnimation(opponent1));
										}

										//give EXP / add EXP
										if (targetIndex > 2)
										{
											for (int i2 = 0; i2 < pokemonPerSide; i2++)
											{
												if (pokemon[i2].getStatus() != PokemonOld.Status.FAINTED)
												{
													float isWildMod = (trainerBattle) ? 1.5f : 1f;
													float baseExpYield =
														PokemonDatabaseOld.getPokemon(pokemon[targetIndex].getID())
															.getBaseExpYield();
													float luckyEggMod = (pokemon[i2].getHeldItem() == "Lucky Egg")
														? 1.5f
														: 1f;
													float OTMod = (pokemon[i2].getIDno() !=
																   SaveDataOld.currentSave.playerID.ToString())
														? 1.5f
														: 1f;
													float sharedMod = 1f; //shared experience
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
																		 luckyEggMod *
																		 (float)pokemon[targetIndex].getLevel()) / 7 *
																		sharedMod);

													yield return StartCoroutine(addExp(i2, exp));
												}
											}
										}

										pokemon[targetIndex] = null;
									}

									//Move effects should not apply to those pokemon that are immune to that move. not yet implemented

									//apply move effects
									MoveDataOld.Effect[] moveEffects = commandMove[movingPokemon].getMoveEffects();
									float[] moveEffectParameters = commandMove[movingPokemon].getMoveParameters();

									//track these and prevent multiple statUp/Down anims
									bool statUpRun = false;
									bool statDownRun = false;
									bool statUpSelfRun = false;
									bool statDownSelfRun = false;
									for (int i2 = 0; i2 < moveEffects.Length; i2++)
									{
										//Check for Chance effect. if failed, no further effects will run
										if (moveEffects[i2] == MoveDataOld.Effect.Chance)
										{
											if (Random.value > moveEffectParameters[i2])
											{
												i2 = moveEffects.Length;
											}
										}
										else
										{
											//Check these booleans to prevent running an animation twice for one pokemon.
											bool animate = false;
											//check if statUp/Down Effect
											if (moveEffects[i2] == MoveDataOld.Effect.ATK ||
												moveEffects[i2] == MoveDataOld.Effect.DEF ||
												moveEffects[i2] == MoveDataOld.Effect.SPA ||
												moveEffects[i2] == MoveDataOld.Effect.SPD ||
												moveEffects[i2] == MoveDataOld.Effect.SPE ||
												moveEffects[i2] == MoveDataOld.Effect.ACC ||
												moveEffects[i2] == MoveDataOld.Effect.EVA)
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
											else if (moveEffects[i2] == MoveDataOld.Effect.ATKself ||
													 moveEffects[i2] == MoveDataOld.Effect.DEFself ||
													 moveEffects[i2] == MoveDataOld.Effect.SPAself ||
													 moveEffects[i2] == MoveDataOld.Effect.SPDself ||
													 moveEffects[i2] == MoveDataOld.Effect.SPEself ||
													 moveEffects[i2] == MoveDataOld.Effect.ACCself ||
													 moveEffects[i2] == MoveDataOld.Effect.EVAself)
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

							pokemonHasMoved[movingPokemon] = true;
						}
						else if (command[movingPokemon] == CommandType.Switch)
						{
							//switch pokemon
							//enemy switching not yet implemented

							yield return
								StartCoroutine(drawTextAndWait(pokemon[movingPokemon].getName() + ", come back!", 1.5f,
									1.5f));
							Dialog.UndrawDialogBox();

							StartCoroutine(slidePokemonStats(0, true));
							yield return StartCoroutine(withdrawPokemon(player1));
							yield return new WaitForSeconds(0.5f);

							switchPokemon(movingPokemon, commandPokemon[movingPokemon]);

							yield return new WaitForSeconds(0.5f);
							yield return
								StartCoroutine(drawTextAndWait("Go! " + pokemon[movingPokemon].getName() + "!", 1.5f,
									1.5f));
							Dialog.UndrawDialogBox();

							if (i == 0)
							{
								//DEBUG
								Debug.Log(pokemon[0].getLongID());
								StopCoroutine(animatePlayer1);
								animatePlayer1 = StartCoroutine(animatePokemon(player1, pokemon[0].GetBackAnim_()));
								yield return new WaitForSeconds(0.2f);
								updatePokemonStatsDisplay(i);
								yield return StartCoroutine(releasePokemon(player1));
								PlayCry(pokemon[0]);
								yield return new WaitForSeconds(0.3f);
								yield return StartCoroutine(slidePokemonStats(0, false));
							}
							pokemonHasMoved[movingPokemon] = true;
						}
					}
					else
					{
						//count pokemon as moved as pokemon does not exist.
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
						if (pokemon[i].getStatus() == PokemonOld.Status.BURNED ||
							pokemon[i].getStatus() == PokemonOld.Status.POISONED)
						{
							pokemon[i].removeHP(Mathf.Floor((float)pokemon[i].getHP() / 8f));
							if (pokemon[i].getStatus() == PokemonOld.Status.BURNED)
							{
								yield return
									StartCoroutine(
										drawTextAndWait(
											generatePreString(i) + pokemon[i].getName() + " is hurt by its burn!", 2.4f))
									;
							}
							else if (pokemon[i].getStatus() == PokemonOld.Status.POISONED)
							{
								yield return
									StartCoroutine(
										drawTextAndWait(
											generatePreString(i) + pokemon[i].getName() + " is hurt by poison!", 2.4f));
							}

							SfxHandler.Play(hitClip);

							if (i == 0)
							{
								//if player pokemon 0 (only stats bar to display HP text)
								yield return
									StartCoroutine(stretchBar(statsHPBar[i],
										Mathf.CeilToInt(pokemon[i].getPercentHP() * 48f), 32f, true, pokemon0CurrentHP,
										pokemon0CurrentHPShadow, pokemon[i].getCurrentHP()));
							}
							else
							{
								yield return
									StartCoroutine(stretchBar(statsHPBar[i],
										Mathf.CeilToInt(pokemon[i].getPercentHP() * 48f), 32f, true, null, null, 0));
							}
							yield return new WaitForSeconds(1.2f);

							Dialog.UndrawDialogBox();
							updatePokemonStatsDisplay(i);
						}


						if (pokemon[i].getStatus() == PokemonOld.Status.FAINTED)
						{
							//debug = array of GUITextures not yet implemented
							yield return
								StartCoroutine(drawTextAndWait(
									generatePreString(i) + pokemon[i].getName() + " fainted!", 2.4f));
							Dialog.UndrawDialogBox();
							yield return new WaitForSeconds(0.2f);
							yield return new WaitForSeconds(PlayCry(pokemon[i]));
							//flexible faint animtions not yet implemented
							if (i == 0)
							{
								StartCoroutine(slidePokemonStats(0, true));
								yield return StartCoroutine(faintPokemonAnimation(player1));
							}
							else if (i == 3)
							{
								StartCoroutine(slidePokemonStats(3, true));
								yield return StartCoroutine(faintPokemonAnimation(opponent1));
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
				for (int i = 0; i < opponentParty.Length; i++)
				{
					//check each opponent
					if (opponentParty[i].getStatus() != PokemonOld.Status.FAINTED)
					{
						allOpponentsDefeated = false;
					}
				}
				//check if any player Pokemon are left
				bool allPlayersDefeated = true;
				for (int i = 0; i < 6; i++)
				{
					//check each player
					if (SaveDataOld.currentSave.PC.boxes[0][i] != null)
					{
						if (SaveDataOld.currentSave.PC.boxes[0][i].getStatus() != PokemonOld.Status.FAINTED)
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
						//replace each opponent
						if (pokemon[i + 3] == null)
						{
							//select the first able pokemon
							for (int i2 = 0; i2 < opponentParty.Length; i2++)
							{
								if (opponentParty[i2].getStatus() != PokemonOld.Status.FAINTED)
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
													opponentName + " sent out " + pokemon[i + 3].getName() + "!", 1.5f,
													1.5f));
										Dialog.UndrawDialogBox();
										if (i == 0)
										{
											//DEBUG
											Debug.Log(pokemon[3].getLongID());
											StopCoroutine(animateOpponent1);
											animateOpponent1 =
												StartCoroutine(animatePokemon(opponent1, pokemon[3].GetFrontAnim_()));
											yield return new WaitForSeconds(0.2f);
											updatePokemonStatsDisplay(i + 3);
											yield return StartCoroutine(releasePokemon(opponent1));
											PlayCry(pokemon[3]);
											yield return new WaitForSeconds(0.3f);
											yield return StartCoroutine(slidePokemonStats(3, false));
										}
										i = pokemonPerSide;
										i2 = opponentParty.Length;
									}
								}
							}
						}
					}

					//replace fainted Player Pokemon
					for (int i = 0; i < pokemonPerSide; i++)
					{
						//replace each player
						if (pokemon[i] == null)
						{
							Dialog.UndrawDialogBox();

							updateCurrentTask(3);
							updateSelectedPokemonSlot(pokePartyPosition, false);
							yield return new WaitForSeconds(0.2f);
							while (currentTask == 3)
							{
								if (Input.GetAxisRaw("Vertical") < 0)
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
								else if (Input.GetAxisRaw("Horizontal") > 0)
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
								else if (Input.GetAxisRaw("Horizontal") < 0)
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
								else if (Input.GetAxisRaw("Vertical") > 0)
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
								else if (Input.GetButtonDown("Select"))
								{
									if (pokePartyPosition == 6)
									{
									} //debug
									else if (SaveDataOld.currentSave.PC.boxes[0][pokePartyPosition] != null)
									{
										updateCurrentTask(5);
										SfxHandler.Play(selectClip);
										int summaryPosition = updateSummaryPosition(0);
										//0 = Switch, 1 = Moves, 2 = Back

										yield return new WaitForSeconds(0.2f);
										while (currentTask == 5)
										{
											if (Input.GetAxisRaw("Vertical") < 0)
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
														SaveDataOld.currentSave.PC.boxes[0][pokePartyPosition]);
													yield return new WaitForSeconds(0.2f);
												}
											}
											else if (Input.GetAxisRaw("Horizontal") > 0)
											{
												if (summaryPosition < 2)
												{
													summaryPosition = updateSummaryPosition(summaryPosition + 1);
													SfxHandler.Play(scrollClip);
													yield return new WaitForSeconds(0.2f);
												}
											}
											else if (Input.GetAxisRaw("Horizontal") < 0)
											{
												if (summaryPosition > 0)
												{
													summaryPosition = updateSummaryPosition(summaryPosition - 1);
													SfxHandler.Play(scrollClip);
													yield return new WaitForSeconds(0.2f);
												}
											}
											else if (Input.GetAxisRaw("Vertical") > 0)
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
														SaveDataOld.currentSave.PC.boxes[0][pokePartyPosition]);
													yield return new WaitForSeconds(0.2f);
												}
											}
											else if (Input.GetButtonDown("Select"))
											{
												if (summaryPosition == 0)
												{
													// switch
													if (
														SaveDataOld.currentSave.PC.boxes[0][pokePartyPosition].getStatus() !=
														PokemonOld.Status.FAINTED)
													{
														//check that pokemon is not on the field
														bool notOnField = true;
														for (int i2 = 0; i2 < pokemonPerSide; i2++)
														{
															if (SaveDataOld.currentSave.PC.boxes[0][pokePartyPosition] ==
																pokemon[i2])
															{
																notOnField = false;
																i2 = pokemonPerSide;
															}
														}
														if (notOnField)
														{
															switchPokemon(i,
																SaveDataOld.currentSave.PC.boxes[0][pokePartyPosition]);
															updateCurrentTask(-1);
															SfxHandler.Play(selectClip);

															yield return
																StartCoroutine(
																	drawTextAndWait(
																		"Go! " + pokemon[i].getName() + "!", 1.5f, 1.5f))
																;
															Dialog.UndrawDialogBox();
															if (i == 0)
															{
																//DEBUG
																Debug.Log(pokemon[0].getLongID());
																StopCoroutine(animatePlayer1);
																animatePlayer1 =
																	StartCoroutine(animatePokemon(player1,
																		pokemon[0].GetBackAnim_()));
																yield return new WaitForSeconds(0.2f);
																updatePokemonStatsDisplay(i);
																yield return StartCoroutine(releasePokemon(player1));
																PlayCry(pokemon[0]);
																yield return new WaitForSeconds(0.3f);
																yield return StartCoroutine(slidePokemonStats(0, false))
																	;
															}
														}
														else
														{
															yield return
																StartCoroutine(
																	drawTextAndWait(
																		SaveDataOld.currentSave.PC.boxes[0][
																			pokePartyPosition].getName() +
																		" is already fighting!"));
															Dialog.UndrawDialogBox();
														}
													}
													else
													{
														yield return
															StartCoroutine(
																drawTextAndWait(
																	SaveDataOld.currentSave.PC.boxes[0][pokePartyPosition]
																		.getName() + " is unable to fight!"));
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
														if (Input.GetAxisRaw("Vertical") < 0)
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
														else if (Input.GetAxisRaw("Horizontal") > 0)
														{
															if (movesPosition != 1 || movesPosition != 3 ||
																movesPosition != 6)
															{
																movesPosition = updateMovesPosition(movesPosition + 1);
																SfxHandler.Play(scrollClip);
																yield return new WaitForSeconds(0.2f);
															}
														}
														else if (Input.GetAxisRaw("Horizontal") < 0)
														{
															if (movesPosition == 1 || movesPosition == 3 ||
																movesPosition > 4)
															{
																movesPosition = updateMovesPosition(movesPosition - 1);
																SfxHandler.Play(scrollClip);
																yield return new WaitForSeconds(0.2f);
															}
														}
														else if (Input.GetAxisRaw("Vertical") > 0)
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
														else if (Input.GetButtonDown("Select"))
														{
															if (movesPosition == 4)
															{
																// switch
																if (
																	SaveDataOld.currentSave.PC.boxes[0][pokePartyPosition]
																		.getStatus() != PokemonOld.Status.FAINTED)
																{
																	//check that pokemon is not on the field
																	bool notOnField = true;
																	for (int i2 = 0; i2 < pokemonPerSide; i2++)
																	{
																		if (
																			SaveDataOld.currentSave.PC.boxes[0][
																				pokePartyPosition] == pokemon[i2])
																		{
																			notOnField = false;
																			i2 = pokemonPerSide;
																		}
																	}
																	if (notOnField)
																	{
																		switchPokemon(i,
																			SaveDataOld.currentSave.PC.boxes[0][
																				pokePartyPosition]);
																		updateCurrentTask(-1);
																		SfxHandler.Play(selectClip);

																		yield return
																			StartCoroutine(
																				drawTextAndWait(
																					SaveDataOld.currentSave.playerName +
																					" sent out " + pokemon[i].getName() +
																					"!", 1.5f, 1.5f));
																		Dialog.UndrawDialogBox();
																		if (i == 0)
																		{
																			//DEBUG
																			Debug.Log(pokemon[0].getLongID());
																			StopCoroutine(animatePlayer1);
																			animatePlayer1 =
																				StartCoroutine(animatePokemon(player1,
																					pokemon[0].GetBackAnim_()));
																			yield return new WaitForSeconds(0.2f);
																			updatePokemonStatsDisplay(i);
																			yield return
																				StartCoroutine(releasePokemon(player1));
																			PlayCry(pokemon[0]);
																			yield return new WaitForSeconds(0.3f);
																			yield return
																				StartCoroutine(slidePokemonStats(0,
																					false));
																		}
																	}
																	else
																	{
																		yield return
																			StartCoroutine(
																				drawTextAndWait(
																					SaveDataOld.currentSave.PC.boxes[0][
																						pokePartyPosition].getName() +
																					" is already fighting!"));
																		Dialog.UndrawDialogBox();
																	}
																}
																else
																{
																	yield return
																		StartCoroutine(
																			drawTextAndWait(
																				SaveDataOld.currentSave.PC.boxes[0][
																					pokePartyPosition].getName() +
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
														else if (Input.GetButtonDown("Back"))
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
											else if (Input.GetButtonDown("Back"))
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


						yield return
							StartCoroutine(
								drawTextAndWait(SaveDataOld.currentSave.playerName + " defeated " + opponentName + "!",
									2.4f, 2.4f));
						Dialog.UndrawDialogBox();
						yield return StartCoroutine(slideTrainer(opponentBase, trainerSprite1, true, false));
						for (int di = 0; di < trainer.playerVictoryDialog.Length; di++)
						{
							yield return StartCoroutine(drawTextAndWait(trainer.playerVictoryDialog[di]));
						}
						Dialog.UndrawDialogBox();

						yield return
							StartCoroutine(
								drawTextAndWait(SaveDataOld.currentSave.playerName + " received $" +
												trainer.GetPrizeMoney() + " for winning!"));
						SaveDataOld.currentSave.playerMoney += trainer.GetPrizeMoney();
					}
					else
					{
						if (trainer.victoryBGM == null)
						{
							BgmHandler.main.PlayOverlay(defaultWildVictoryBGM, defaultWildVictoryBGMLoopStart);
						}
						else
						{
							BgmHandler.main.PlayOverlay(trainer.victoryBGM, trainer.victorySamplesLoopStart);
						}

						//wild exp print out not yet implemented here
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
								drawTextAndWait(opponentName + " defeated " + SaveDataOld.currentSave.playerName + "!",
									2.4f, 2.4f));
						Dialog.UndrawDialogBox();
						yield return StartCoroutine(slideTrainer(opponentBase, trainerSprite1, true, false));
						for (int di = 0; di < trainer.playerLossDialog.Length; di++)
						{
							yield return StartCoroutine(drawTextAndWait(trainer.playerLossDialog[di]));
						}
						Dialog.UndrawDialogBox();

						StartCoroutine(ScreenFade.main.Fade(false, 1f));
					}
					else
					{
						yield return
							StartCoroutine(
								drawTextAndWait(SaveDataOld.currentSave.playerName + " is out of usable Pokémon!", 2f));
						yield return
							StartCoroutine(drawTextAndWait(SaveDataOld.currentSave.playerName + " dropped $200 in panic!",
								2f));
						yield return StartCoroutine(drawTextAndWait("... ... ... ...", 2f));

						yield return
							StartCoroutine(drawTextAndWait(SaveDataOld.currentSave.playerName + " blacked out!", 1.8f, 1.8f))
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
						if (SaveDataOld.currentSave.PC.boxes[0][i] != null)
						{
							SaveDataOld.currentSave.PC.boxes[0][i].healFull();
						}
					}
				}
				Dialog.UndrawDialogBox();
				yield return new WaitForSeconds(0.4f);
			}
		}


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
			BgmHandler.main.ResumeMain(1.4f);
		}
		yield return new WaitForSeconds(1.4f);

		//check for evolutions to run ONLY if won, or healed on defeat
		if (victor == 0 || healedOnDefeat)
		{
			for (int i = 0; i < initialLevels.Length; i++)
			{
				if (SaveDataOld.currentSave.PC.boxes[0][i] != null)
				{
					//if level is different to it was at the start of the battle
					if (SaveDataOld.currentSave.PC.boxes[0][i].getLevel() != initialLevels[i])
					{
						//if can evolve
						if (SaveDataOld.currentSave.PC.boxes[0][i].canEvolve("Level"))
						{
							BgmHandler.main.PlayOverlay(null, 0, 0);

							//Set SceneEvolution to be active so that it appears
							Scene.main.Evolution.gameObject.SetActive(true);
							StartCoroutine(Scene.main.Evolution.control(SaveDataOld.currentSave.PC.boxes[0][i], "Level"));
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

		GlobalVariables.global.resetFollower();
		this.gameObject.SetActive(false);
	}
	#endregion*/
}
