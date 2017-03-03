//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SummaryHandler : MonoBehaviour
{
    public string replacedMove;

    private Image
        selectedCaughtBall,
        selectedSprite,
        selectedStatus,
        selectedShiny;

    private Text
        selectedName,
        selectedNameShadow,
        selectedGender,
        selectedGenderShadow,
        selectedLevel,
        selectedLevelShadow,
        selectedHeldItem,
        selectedHeldItemShadow;

    private int frame = 0;
    private Sprite[] selectedSpriteAnimation;

    private GameObject[] pages = new GameObject[8];

    private Text
        dexNo,
        dexNoShadow,
        species,
        speciesShadow,
        OT,
        OTShadow,
        IDNo,
        IDNoShadow,
        expPoints,
        expPointsShadow,
        toNextLevel,
        toNextLevelShadow;

    private Image
        type1,
        type2,
        expBar;

    private Text
        nature,
        natureShadow,
        metDate,
        metDateShadow,
        metMap,
        metMapShadow,
        metLevel,
        metLevelShadow,
        characteristic,
        characteristicShadow;

    private Image HPBar;

    private Text
        HPText,
        HPTextShadow,
        HP,
        HPShadow,
        StatsTextShadow,
        Stats,
        StatsShadow,
        abilityName,
        abilityNameShadow,
        abilityDescription,
        abilityDescriptionShadow;

    private RectTransform moves;

    private Image
        moveSelector,
        selectedMove,
        selectedCategory,
        move1Type,
        move2Type,
        move3Type,
        move4Type;

    private Text
        move1Name,
        move1NameShadow,
        move1PPText,
        move1PPTextShadow,
        move1PP,
        move1PPShadow,
        move2Name,
        move2NameShadow,
        move2PPText,
        move2PPTextShadow,
        move2PP,
        move2PPShadow,
        move3Name,
        move3NameShadow,
        move3PPText,
        move3PPTextShadow,
        move3PP,
        move3PPShadow,
        move4Name,
        move4NameShadow,
        move4PPText,
        move4PPTextShadow,
        move4PP,
        move4PPShadow,
        selectedPower,
        selectedPowerShadow,
        selectedAccuracy,
        selectedAccuracyShadow,
        selectedDescription,
        selectedDescriptionShadow;

    private GameObject learnScreen;

    private RectTransform newMove;

    private Image
        moveNewType;

    private Text
        moveNewName,
        moveNewNameShadow,
        moveNewPPText,
        moveNewPPTextShadow,
        moveNewPP,
        moveNewPPShadow;

    private GameObject forget;

    //ribbons not yet implemented.

    public AudioClip selectClip;
    public AudioClip scrollClip;
    public AudioClip returnClip;


    void Awake()
    {
        Transform selectedInfo = transform.FindChild("SelectedInfo");

        selectedCaughtBall = selectedInfo.FindChild("SelectedCaughtBall").GetComponent<Image>();
        selectedNameShadow = selectedInfo.FindChild("SelectedName").GetComponent<Text>();
        selectedName = selectedNameShadow.transform.FindChild("Text").GetComponent<Text>();
        selectedGenderShadow = selectedInfo.FindChild("SelectedGender").GetComponent<Text>();
        selectedGender = selectedGenderShadow.transform.FindChild("Text").GetComponent<Text>();
        selectedLevelShadow = selectedInfo.FindChild("SelectedLevel").GetComponent<Text>();
        selectedLevel = selectedLevelShadow.transform.FindChild("Text").GetComponent<Text>();
        selectedSprite = selectedInfo.FindChild("SelectedSprite").GetComponent<Image>();
        selectedHeldItemShadow = selectedInfo.FindChild("SelectedHeldItem").GetComponent<Text>();
        selectedHeldItem = selectedHeldItemShadow.transform.FindChild("Text").GetComponent<Text>();
        selectedStatus = selectedInfo.FindChild("SelectedStatus").GetComponent<Image>();
        selectedShiny = selectedInfo.FindChild("SelectedShiny").GetComponent<Image>();

        pages[1] = transform.FindChild("1").gameObject;

        dexNoShadow = pages[1].transform.FindChild("DexNo").GetComponent<Text>();
        dexNo = dexNoShadow.transform.FindChild("Text").GetComponent<Text>();
        speciesShadow = pages[1].transform.FindChild("Species").GetComponent<Text>();
        species = speciesShadow.transform.FindChild("Text").GetComponent<Text>();
        type1 = pages[1].transform.FindChild("Type1").GetComponent<Image>();
        type2 = pages[1].transform.FindChild("Type2").GetComponent<Image>();
        OTShadow = pages[1].transform.FindChild("OT").GetComponent<Text>();
        OT = OTShadow.transform.FindChild("Text").GetComponent<Text>();
        IDNoShadow = pages[1].transform.FindChild("IDNo").GetComponent<Text>();
        IDNo = IDNoShadow.transform.FindChild("Text").GetComponent<Text>();
        expPointsShadow = pages[1].transform.FindChild("ExpPoints").GetComponent<Text>();
        expPoints = expPointsShadow.transform.FindChild("Text").GetComponent<Text>();
        toNextLevelShadow = pages[1].transform.FindChild("ToNextLevel").GetComponent<Text>();
        toNextLevel = toNextLevelShadow.transform.FindChild("Text").GetComponent<Text>();
        expBar = pages[1].transform.FindChild("ExpBar").GetComponent<Image>();

        pages[2] = transform.FindChild("2").gameObject;

        natureShadow = pages[2].transform.FindChild("Nature").GetComponent<Text>();
        nature = natureShadow.transform.FindChild("Text").GetComponent<Text>();
        metDateShadow = pages[2].transform.FindChild("MetDate").GetComponent<Text>();
        metDate = metDateShadow.transform.FindChild("Text").GetComponent<Text>();
        metMapShadow = pages[2].transform.FindChild("MetMap").GetComponent<Text>();
        metMap = metMapShadow.transform.FindChild("Text").GetComponent<Text>();
        metLevelShadow = pages[2].transform.FindChild("MetLevel").GetComponent<Text>();
        metLevel = metLevelShadow.transform.FindChild("Text").GetComponent<Text>();
        characteristicShadow = pages[2].transform.FindChild("Characteristic").GetComponent<Text>();
        characteristic = characteristicShadow.transform.FindChild("Text").GetComponent<Text>();

        pages[3] = transform.FindChild("3").gameObject;

        HPTextShadow = pages[3].transform.FindChild("HPText").GetComponent<Text>();
        HPText = HPTextShadow.transform.FindChild("Text").GetComponent<Text>();
        HPShadow = pages[3].transform.FindChild("HP").GetComponent<Text>();
        HP = HPShadow.transform.FindChild("Text").GetComponent<Text>();
        HPBar = pages[3].transform.FindChild("HPBar").GetComponent<Image>();
        StatsShadow = pages[3].transform.FindChild("Stats").GetComponent<Text>();
        Stats = StatsShadow.transform.FindChild("Text").GetComponent<Text>();
        StatsTextShadow = pages[3].transform.FindChild("StatsText").GetComponent<Text>();
        abilityNameShadow = pages[3].transform.FindChild("AbilityName").GetComponent<Text>();
        abilityName = abilityNameShadow.transform.FindChild("Text").GetComponent<Text>();
        abilityDescriptionShadow = pages[3].transform.FindChild("AbilityDescription").GetComponent<Text>();
        abilityDescription = abilityDescriptionShadow.transform.FindChild("Text").GetComponent<Text>();

        pages[4] = transform.FindChild("4").gameObject;

        moves = pages[4].transform.FindChild("Moves").GetComponent<RectTransform>();

        moveSelector = pages[4].transform.FindChild("MoveSelector").GetComponent<Image>();
        selectedMove = pages[4].transform.FindChild("SelectedMove").GetComponent<Image>();

        move1NameShadow = moves.FindChild("Move1").GetComponent<Text>();
        move1Name = move1NameShadow.transform.FindChild("Text").GetComponent<Text>();
        move1Type = moves.FindChild("Move1Type").GetComponent<Image>();
        move1PPTextShadow = moves.FindChild("Move1PPText").GetComponent<Text>();
        move1PPText = move1PPTextShadow.transform.FindChild("Text").GetComponent<Text>();
        move1PPShadow = moves.FindChild("Move1PP").GetComponent<Text>();
        move1PP = move1PPShadow.transform.FindChild("Text").GetComponent<Text>();
        move2NameShadow = moves.FindChild("Move2").GetComponent<Text>();
        move2Name = move2NameShadow.transform.FindChild("Text").GetComponent<Text>();
        move2Type = moves.FindChild("Move2Type").GetComponent<Image>();
        move2PPTextShadow = moves.FindChild("Move2PPText").GetComponent<Text>();
        move2PPText = move2PPTextShadow.transform.FindChild("Text").GetComponent<Text>();
        move2PPShadow = moves.FindChild("Move2PP").GetComponent<Text>();
        move2PP = move2PPShadow.transform.FindChild("Text").GetComponent<Text>();
        move3NameShadow = moves.FindChild("Move3").GetComponent<Text>();
        move3Name = move3NameShadow.transform.FindChild("Text").GetComponent<Text>();
        move3Type = moves.FindChild("Move3Type").GetComponent<Image>();
        move3PPTextShadow = moves.FindChild("Move3PPText").GetComponent<Text>();
        move3PPText = move3PPTextShadow.transform.FindChild("Text").GetComponent<Text>();
        move3PPShadow = moves.FindChild("Move3PP").GetComponent<Text>();
        move3PP = move3PPShadow.transform.FindChild("Text").GetComponent<Text>();
        move4NameShadow = moves.FindChild("Move4").GetComponent<Text>();
        move4Name = move4NameShadow.transform.FindChild("Text").GetComponent<Text>();
        move4Type = moves.FindChild("Move4Type").GetComponent<Image>();
        move4PPTextShadow = moves.FindChild("Move4PPText").GetComponent<Text>();
        move4PPText = move4PPTextShadow.transform.FindChild("Text").GetComponent<Text>();
        move4PPShadow = moves.FindChild("Move4PP").GetComponent<Text>();
        move4PP = move4PPShadow.transform.FindChild("Text").GetComponent<Text>();
        selectedCategory = pages[4].transform.FindChild("SelectedCategory").GetComponent<Image>();
        selectedPowerShadow = pages[4].transform.FindChild("SelectedPower").GetComponent<Text>();
        selectedPower = selectedPowerShadow.transform.FindChild("Text").GetComponent<Text>();
        selectedAccuracyShadow = pages[4].transform.FindChild("SelectedAccuracy").GetComponent<Text>();
        selectedAccuracy = selectedAccuracyShadow.transform.FindChild("Text").GetComponent<Text>();
        selectedDescriptionShadow = pages[4].transform.FindChild("SelectedDescription").GetComponent<Text>();
        selectedDescription = selectedDescriptionShadow.transform.FindChild("Text").GetComponent<Text>();

        learnScreen = pages[4].transform.FindChild("Learn").gameObject;

        newMove = pages[4].transform.FindChild("NewMove").GetComponent<RectTransform>();

        moveNewNameShadow = newMove.FindChild("MoveNew").GetComponent<Text>();
        moveNewName = moveNewNameShadow.transform.FindChild("Text").GetComponent<Text>();
        moveNewType = newMove.FindChild("MoveNewType").GetComponent<Image>();
        moveNewPPTextShadow = newMove.FindChild("MoveNewPPText").GetComponent<Text>();
        moveNewPPText = moveNewPPTextShadow.transform.FindChild("Text").GetComponent<Text>();
        moveNewPPShadow = newMove.FindChild("MoveNewPP").GetComponent<Text>();
        moveNewPP = moveNewPPShadow.transform.FindChild("Text").GetComponent<Text>();
        forget = newMove.FindChild("Forget").gameObject;


        pages[5] = transform.FindChild("5").gameObject;

        pages[6] = transform.FindChild("6").gameObject;
    }

    void Start()
    {
        gameObject.SetActive(false);
    }


    private void updateSelection(Pokemon selectedPokemon)
    {
        frame = 0;

        PlayCry(selectedPokemon);

        selectedCaughtBall.sprite = Resources.Load<Sprite>("null");
        selectedCaughtBall.sprite = Resources.Load<Sprite>("PCSprites/summary" + selectedPokemon.getCaughtBall());
        selectedName.text = selectedPokemon.getName();
        selectedNameShadow.text = selectedName.text;
        if (selectedPokemon.getGender() == Pokemon.Gender.FEMALE)
        {
            selectedGender.text = "♀";
            selectedGender.color = new Color(1, 0.2f, 0.2f, 1);
        }
        else if (selectedPokemon.getGender() == Pokemon.Gender.MALE)
        {
            selectedGender.text = "♂";
            selectedGender.color = new Color(0.2f, 0.4f, 1, 1);
        }
        else
        {
            selectedGender.text = null;
        }
        selectedGenderShadow.text = selectedGender.text;
        selectedLevel.text = "" + selectedPokemon.getLevel();
        selectedLevelShadow.text = selectedLevel.text;
        selectedSpriteAnimation = selectedPokemon.GetFrontAnim_();
        if (selectedSpriteAnimation.Length > 0)
        {
            selectedSprite.sprite = selectedSpriteAnimation[0];
        }
        if (string.IsNullOrEmpty(selectedPokemon.getHeldItem()))
        {
            selectedHeldItem.text = "None";
        }
        else
        {
            selectedHeldItem.text = selectedPokemon.getHeldItem();
        }
        selectedHeldItemShadow.text = selectedHeldItem.text;
        if (selectedPokemon.getStatus() != Pokemon.Status.NONE)
        {
            selectedStatus.sprite = Resources.Load<Sprite>("PCSprites/status" + selectedPokemon.getStatus().ToString());
        }
        else
        {
            selectedStatus.sprite = Resources.Load<Sprite>("null");
        }

        if (selectedPokemon.getIsShiny())
        {
            selectedShiny.sprite = Resources.Load<Sprite>("PCSprites/shiny");
        }
        else
        {
            selectedShiny.sprite = Resources.Load<Sprite>("null");
        }

        dexNo.text = selectedPokemon.getLongID();
        dexNoShadow.text = dexNo.text;
        species.text = PokemonDatabase.getPokemon(selectedPokemon.getID()).getName();
        speciesShadow.text = species.text;
        string type1string = PokemonDatabase.getPokemon(selectedPokemon.getID()).getType1().ToString();
        string type2string = PokemonDatabase.getPokemon(selectedPokemon.getID()).getType2().ToString();
        type1.sprite = Resources.Load<Sprite>("null");
        type2.sprite = Resources.Load<Sprite>("null");
        if (type1string != "NONE")
        {
            type1.sprite = Resources.Load<Sprite>("PCSprites/type" + type1string);
            type1.rectTransform.localPosition = new Vector3(71, type1.rectTransform.localPosition.y);
        }
        if (type2string != "NONE")
        {
            type2.sprite = Resources.Load<Sprite>("PCSprites/type" + type2string);
        }
        else
        {
            //if single type pokemon, center the type icon
            type1.rectTransform.localPosition = new Vector3(89, type1.rectTransform.localPosition.y);
        }
        OT.text = selectedPokemon.getOT();
        OTShadow.text = OT.text;
        IDNo.text = "" + selectedPokemon.getIDno();
        IDNoShadow.text = IDNo.text;
        expPoints.text = "" + selectedPokemon.getExp();
        expPointsShadow.text = expPoints.text;
        float expCurrentLevel =
            PokemonDatabase.getLevelExp(PokemonDatabase.getPokemon(selectedPokemon.getID()).getLevelingRate(),
                selectedPokemon.getLevel());
        float expNextlevel =
            PokemonDatabase.getLevelExp(PokemonDatabase.getPokemon(selectedPokemon.getID()).getLevelingRate(),
                selectedPokemon.getLevel() + 1);
        float expAlong = selectedPokemon.getExp() - expCurrentLevel;
        float expDistance = expAlong / (expNextlevel - expCurrentLevel);
        toNextLevel.text = "" + (expNextlevel - selectedPokemon.getExp());
        toNextLevelShadow.text = toNextLevel.text;
        expBar.rectTransform.sizeDelta = new Vector2(Mathf.Floor(expDistance * 64f), expBar.rectTransform.sizeDelta.y);

        string natureFormatted = selectedPokemon.getNature();
        natureFormatted = natureFormatted.Substring(0, 1) + natureFormatted.Substring(1).ToLowerInvariant();
        nature.text = "<color=#F22F>" + natureFormatted + "</color> nature.";
        natureShadow.text = natureFormatted + " nature.";
        metDate.text = "Met on " + selectedPokemon.getMetDate();
        metDateShadow.text = metDate.text;
        metMap.text = "<color=#F22F>" + selectedPokemon.getMetMap() + "</color>";
        metMapShadow.text = selectedPokemon.getMetMap();
        metLevel.text = "Met at Level " + selectedPokemon.getMetLevel() + ".";
        metLevelShadow.text = metLevel.text;

        string[][] characteristics = new string[][]
        {
            new string[]
            {
                "Loves to eat", "Takes plenty of siestas", "Nods off a lot", "Scatters things often", "Likes to relax"
            },
            new string[]
            {
                "Proud of its power", "Likes to thrash about", "A little quick tempered", "Likes to fight",
                "Quick tempered"
            },
            new string[]
            {
                "Sturdy body", "Capable of taking hits", "Highly persistent", "Good endurance", "Good perseverance"
            },
            new string[]
            {
                "Highly curious", "Mischievous", "Thoroughly cunning", "Often lost in thought", "Very finicky"
            },
            new string[]
            {
                "Strong willed", "Somewhat vain", "Strongly defiant", "Hates to lose", "Somewhat stubborn"
            },
            new string[]
            {
                "Likes to run", "Alert to sounds", "Impetuous and silly", "Somewhat of a clown", "Quick to flee"
            }
        };
        int highestIV = selectedPokemon.GetHighestIV();
        characteristic.text = characteristics[highestIV][selectedPokemon.GetIV(highestIV) % 5] + ".";
        characteristicShadow.text = characteristic.text;

        float currentHP = selectedPokemon.getCurrentHP();
        float maxHP = selectedPokemon.getHP();
        HP.text = currentHP + "/" + maxHP;
        HPShadow.text = HP.text;
        HPBar.rectTransform.sizeDelta = new Vector2(selectedPokemon.getPercentHP() * 48f,
            HPBar.rectTransform.sizeDelta.y);

        if (currentHP < (maxHP / 4f))
        {
            HPBar.color = new Color(1, 0.125f, 0, 1);
        }
        else if (currentHP < (maxHP / 2f))
        {
            HPBar.color = new Color(1, 0.75f, 0, 1);
        }
        else
        {
            HPBar.color = new Color(0.125f, 1, 0.065f, 1);
        }

        float[] natureMod = new float[]
        {
            NatureDatabase.getNature(selectedPokemon.getNature()).getATK(),
            NatureDatabase.getNature(selectedPokemon.getNature()).getDEF(),
            NatureDatabase.getNature(selectedPokemon.getNature()).getSPA(),
            NatureDatabase.getNature(selectedPokemon.getNature()).getSPD(),
            NatureDatabase.getNature(selectedPokemon.getNature()).getSPE()
        };
        Stats.text =
            selectedPokemon.getATK() + "\n" +
            selectedPokemon.getDEF() + "\n" +
            selectedPokemon.getSPA() + "\n" +
            selectedPokemon.getSPD() + "\n" +
            selectedPokemon.getSPE();
        StatsShadow.text = Stats.text;

        string[] statsLines = new string[] {"Attack", "Defence", "Sp. Atk", "Sp. Def", "Speed"};
        StatsTextShadow.text = "";
        for (int i = 0; i < 5; i++)
        {
            if (natureMod[i] > 1)
            {
                StatsTextShadow.text += "<color=#A01010FF>" + statsLines[i] + "</color>\n";
            }
            else if (natureMod[i] < 1)
            {
                StatsTextShadow.text += "<color=#0030A2FF>" + statsLines[i] + "</color>\n";
            }
            else
            {
                StatsTextShadow.text += statsLines[i] + "\n";
            }
        }


        abilityName.text = PokemonDatabase.getPokemon(selectedPokemon.getID()).getAbility(selectedPokemon.getAbility());
        abilityNameShadow.text = abilityName.text;
        //abilities not yet implemented
        abilityDescription.text = "";
        abilityDescriptionShadow.text = abilityDescription.text;

        updateSelectionMoveset(selectedPokemon);
    }

    private void updateSelectionMoveset(Pokemon selectedPokemon)
    {
        string[] moveset = selectedPokemon.getMoveset();
        int[] maxPP = selectedPokemon.getMaxPP();
        int[] PP = selectedPokemon.getPP();
        if (!string.IsNullOrEmpty(moveset[0]))
        {
            move1Name.text = moveset[0];
            move1NameShadow.text = move1Name.text;
            move1Type.sprite =
                Resources.Load<Sprite>("PCSprites/type" + MoveDatabase.getMove(moveset[0]).getType().ToString());
            move1PPText.text = "PP";
            move1PPTextShadow.text = move1PPText.text;
            move1PP.text = PP[0] + "/" + maxPP[0];
            move1PPShadow.text = move1PP.text;
        }
        else
        {
            move1Name.text = null;
            move1NameShadow.text = move1Name.text;
            move1Type.sprite = Resources.Load<Sprite>("null");
            move1PPText.text = null;
            move1PPTextShadow.text = move1PPText.text;
            move1PP.text = null;
            move1PPShadow.text = move1PP.text;
        }
        if (!string.IsNullOrEmpty(moveset[1]))
        {
            move2Name.text = moveset[1];
            move2NameShadow.text = move2Name.text;
            move2Type.sprite =
                Resources.Load<Sprite>("PCSprites/type" + MoveDatabase.getMove(moveset[1]).getType().ToString());
            move2PPText.text = "PP";
            move2PPTextShadow.text = move2PPText.text;
            move2PP.text = PP[1] + "/" + maxPP[1];
            move2PPShadow.text = move2PP.text;
        }
        else
        {
            move2Name.text = null;
            move2NameShadow.text = move2Name.text;
            move2Type.sprite = Resources.Load<Sprite>("null");
            move2PPText.text = null;
            move2PPTextShadow.text = move2PPText.text;
            move2PP.text = null;
            move2PPShadow.text = move2PP.text;
        }
        if (!string.IsNullOrEmpty(moveset[2]))
        {
            move3Name.text = moveset[2];
            move3NameShadow.text = move3Name.text;
            move3Type.sprite =
                Resources.Load<Sprite>("PCSprites/type" + MoveDatabase.getMove(moveset[2]).getType().ToString());
            move3PPText.text = "PP";
            move3PPTextShadow.text = move3PPText.text;
            move3PP.text = PP[2] + "/" + maxPP[2];
            move3PPShadow.text = move3PP.text;
        }
        else
        {
            move3Name.text = null;
            move3NameShadow.text = move3Name.text;
            move3Type.sprite = Resources.Load<Sprite>("null");
            move3PPText.text = null;
            move3PPTextShadow.text = move3PPText.text;
            move3PP.text = null;
            move3PPShadow.text = move3PP.text;
        }
        if (!string.IsNullOrEmpty(moveset[3]))
        {
            move4Name.text = moveset[3];
            move4NameShadow.text = move4Name.text;
            move4Type.sprite =
                Resources.Load<Sprite>("PCSprites/type" + MoveDatabase.getMove(moveset[3]).getType().ToString());
            move4PPText.text = "PP";
            move4PPTextShadow.text = move4PPText.text;
            move4PP.text = PP[3] + "/" + maxPP[3];
            move4PPShadow.text = move4PP.text;
        }
        else
        {
            move4Name.text = null;
            move4NameShadow.text = move4Name.text;
            move4Type.sprite = Resources.Load<Sprite>("null");
            move4PPText.text = null;
            move4PPTextShadow.text = move4PPText.text;
            move4PP.text = null;
            move4PPShadow.text = move4PP.text;
        }

        updateSelectedMove(null);
    }

    private void updateMoveToLearn(string moveName)
    {
        MoveData move = MoveDatabase.getMove(moveName);
        moveNewName.text = moveName;
        moveNewNameShadow.text = moveNewName.text;
        moveNewType.sprite = Resources.Load<Sprite>("PCSprites/type" + move.getType().ToString());
        moveNewPPText.text = "PP";
        moveNewPPTextShadow.text = moveNewPPText.text;
        moveNewPP.text = move.getPP() + "/" + move.getPP();
        moveNewPPShadow.text = moveNewPP.text;
    }

    private void updateSelectedMove(string moveName)
    {
        if (string.IsNullOrEmpty(moveName))
        {
            selectedCategory.sprite = Resources.Load<Sprite>("null");
            selectedPower.text = null;
            selectedPowerShadow.text = selectedPower.text;
            selectedAccuracy.text = null;
            selectedAccuracyShadow.text = selectedAccuracy.text;
            selectedDescription.text = null;
            selectedDescriptionShadow.text = selectedDescription.text;
        }
        else
        {
            MoveData selectedMove = MoveDatabase.getMove(moveName);
            selectedCategory.sprite =
                Resources.Load<Sprite>("PCSprites/category" + selectedMove.getCategory().ToString());
            selectedPower.text = "" + selectedMove.getPower();
            if (selectedPower.text == "0")
            {
                selectedPower.text = "-";
            }
            selectedPowerShadow.text = selectedPower.text;
            selectedAccuracy.text = "" + Mathf.Round(selectedMove.getAccuracy() * 100f);
            if (selectedAccuracy.text == "0")
            {
                selectedAccuracy.text = "-";
            }
            selectedAccuracyShadow.text = selectedAccuracy.text;
            selectedDescription.text = selectedMove.getDescription();
            selectedDescriptionShadow.text = selectedDescription.text;
        }
    }

    private IEnumerator moveMoveSelector(Vector3 destinationPosition)
    {
        Vector3 startPosition = moveSelector.rectTransform.localPosition;
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
            moveSelector.rectTransform.localPosition = startPosition + (distance * increment);
            yield return null;
        }
    }

    private IEnumerator animatePokemon()
    {
        frame = 0;
        while (true)
        {
            if (selectedSpriteAnimation.Length > 0)
            {
                frame = (frame < selectedSpriteAnimation.Length - 1) ? frame + 1 : 0;
                selectedSprite.sprite = selectedSpriteAnimation[frame];
            }
            yield return new WaitForSeconds(0.08f);
        }
    }

    private void PlayCry(Pokemon pokemon)
    {
        SfxHandler.Play(pokemon.GetCry(), pokemon.GetCryPitch());
    }


    public IEnumerator control(Pokemon[] pokemonList, int currentPosition)
    {
        yield return StartCoroutine(control(pokemonList, currentPosition, false, null));
    }

    public IEnumerator control(Pokemon pokemon, string newMoveString)
    {
        yield return StartCoroutine(control(new Pokemon[] {pokemon}, 0, true, newMoveString));
    }

    public IEnumerator control(Pokemon[] pokemonList, int currentPosition, bool learning, string newMoveString)
    {
        moves.localPosition = (learning) ? new Vector3(0, 32) : Vector3.zero;
        newMove.gameObject.SetActive(learning);
        learnScreen.SetActive(learning);

        moveSelector.enabled = false;
        selectedMove.enabled = false;

        forget.SetActive(false);

        pages[1].SetActive(!learning);
        pages[2].SetActive(false);
        pages[3].SetActive(false);
        pages[4].SetActive(learning);
        pages[5].SetActive(false);
        pages[6].SetActive(false);

        updateSelection(pokemonList[currentPosition]);
        if (learning)
        {
            updateMoveToLearn(newMoveString);
        }

        StartCoroutine("animatePokemon");

        bool running = true;
        int currentPage = (learning) ? 4 : 1;
        int checkPosition = currentPosition;

        replacedMove = null;

        yield return StartCoroutine(ScreenFade.main.Fade(true, ScreenFade.defaultSpeed));

        if (learning)
        {
            yield return StartCoroutine(NavigateMoves(pokemonList[currentPosition], true, newMoveString));
        }
        else
        {
            while (running)
            {
                //cycle through the pages
                if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    if (currentPage > 1)
                    {
                        pages[currentPage - 1].SetActive(true);
                        pages[currentPage].SetActive(false);
                        currentPage -= 1;
                        SfxHandler.Play(scrollClip);
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                else if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    if (currentPage < 6)
                    {
                        pages[currentPage + 1].SetActive(true);
                        pages[currentPage].SetActive(false);
                        currentPage += 1;
                        SfxHandler.Play(scrollClip);
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                //cycle through pokemon
                else if (Input.GetAxisRaw("Vertical") > 0)
                {
                    checkPosition = currentPosition;
                    if (checkPosition > 0)
                    {
                        checkPosition -= 1;
                    }
                    while (checkPosition > 0 && pokemonList[checkPosition] == null)
                    {
                        checkPosition -= 1;
                    }
                    if (pokemonList[checkPosition] != null && checkPosition != currentPosition)
                    {
                        currentPosition = checkPosition;
                        updateSelection(pokemonList[checkPosition]);
                        //SfxHandler.Play(scrollClip);
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                else if (Input.GetAxisRaw("Vertical") < 0)
                {
                    checkPosition = currentPosition;
                    if (checkPosition < pokemonList.Length - 1)
                    {
                        checkPosition += 1;
                    }
                    while (checkPosition < pokemonList.Length - 1 && pokemonList[checkPosition] == null)
                    {
                        checkPosition += 1;
                    }
                    if (pokemonList[checkPosition] != null && checkPosition != currentPosition)
                    {
                        currentPosition = checkPosition;
                        updateSelection(pokemonList[checkPosition]);
                        //SfxHandler.Play(scrollClip);
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                //rearrange moves/close summary
                else if (Input.GetButton("Select"))
                {
                    if (currentPage == 4)
                    {
                        if (pokemonList[currentPosition].getMoveset()[0] != null)
                        {
                            //if there are moves to rearrange
                            SfxHandler.Play(selectClip);
                            yield return StartCoroutine(NavigateMoves(pokemonList[currentPosition], false, ""));
                        }
                    }
                    else if (currentPage == 6)
                    {
                        running = false;
                    }
                }
                else if (Input.GetButton("Back"))
                {
                    running = false;
                }

                yield return null;
            }
        }

        yield return StartCoroutine(ScreenFade.main.Fade(false, ScreenFade.defaultSpeed));
        this.gameObject.SetActive(false);
    }

    private IEnumerator NavigateMoves(Pokemon pokemon, bool learning, string newMoveString)
    {
        learnScreen.SetActive(learning);
        newMove.gameObject.SetActive(learning);
        Vector3 positionMod = (learning) ? new Vector3(0, 32) : new Vector3(0, 0);
        moves.localPosition = positionMod;
        if (learning)
        {
            updateMoveToLearn(newMoveString);
        }

        string[] pokeMoveset = pokemon.getMoveset();
        string[] moveset = new string[]
        {
            pokeMoveset[0], pokeMoveset[1],
            pokeMoveset[2], pokeMoveset[3],
            newMoveString, newMoveString
        };
        Vector3[] positions = new Vector3[]
        {
            new Vector3(21, 32), new Vector3(108, 32),
            new Vector3(21, 0), new Vector3(108, 0),
            new Vector3(64, -32), new Vector3(64, -32)
        };

        moveSelector.enabled = true;
        selectedMove.enabled = false;

        bool navigatingMoves = true;
        bool selectingMove = false;
        int currentMoveNumber = 0;
        int selectedMoveNumber = -1;

        moveSelector.rectTransform.localPosition = positions[0] + positionMod;
        updateSelectedMove(moveset[currentMoveNumber]);
        yield return null;
        while (navigatingMoves)
        {
            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                if (currentMoveNumber == 1)
                {
                    currentMoveNumber = 0;
                    updateSelectedMove(moveset[currentMoveNumber]);
                    SfxHandler.Play(scrollClip);
                    yield return StartCoroutine(moveMoveSelector(positions[currentMoveNumber] + positionMod));
                }
                else if (currentMoveNumber == 3)
                {
                    if (!string.IsNullOrEmpty(moveset[2]))
                    {
                        currentMoveNumber = 2;
                        updateSelectedMove(moveset[currentMoveNumber]);
                        SfxHandler.Play(scrollClip);
                        yield return StartCoroutine(moveMoveSelector(positions[currentMoveNumber] + positionMod));
                    }
                }
                else if (learning)
                {
                    if (currentMoveNumber == 5)
                    {
                        currentMoveNumber = 4;
                    }
                }
            }
            else if (Input.GetAxisRaw("Horizontal") > 0)
            {
                if (currentMoveNumber == 0)
                {
                    if (!string.IsNullOrEmpty(moveset[1]))
                    {
                        currentMoveNumber = 1;
                        updateSelectedMove(moveset[currentMoveNumber]);
                        SfxHandler.Play(scrollClip);
                        yield return StartCoroutine(moveMoveSelector(positions[currentMoveNumber] + positionMod));
                    }
                }
                else if (currentMoveNumber == 2)
                {
                    if (!string.IsNullOrEmpty(moveset[3]))
                    {
                        currentMoveNumber = 3;
                        updateSelectedMove(moveset[currentMoveNumber]);
                        SfxHandler.Play(scrollClip);
                        yield return StartCoroutine(moveMoveSelector(positions[currentMoveNumber] + positionMod));
                    }
                }
                else if (learning)
                {
                    if (currentMoveNumber == 4)
                    {
                        currentMoveNumber = 5;
                    }
                }
            }
            else if (Input.GetAxisRaw("Vertical") > 0)
            {
                if (currentMoveNumber == 2)
                {
                    currentMoveNumber = 0;
                    updateSelectedMove(moveset[currentMoveNumber]);
                    SfxHandler.Play(scrollClip);
                    yield return StartCoroutine(moveMoveSelector(positions[currentMoveNumber] + positionMod));
                }
                else if (currentMoveNumber == 3)
                {
                    if (!string.IsNullOrEmpty(moveset[1]))
                    {
                        currentMoveNumber = 1;
                        updateSelectedMove(moveset[currentMoveNumber]);
                        SfxHandler.Play(scrollClip);
                        yield return StartCoroutine(moveMoveSelector(positions[currentMoveNumber] + positionMod));
                    }
                }
                else if (learning)
                {
                    if (currentMoveNumber == 4)
                    {
                        if (!string.IsNullOrEmpty(moveset[2]))
                        {
                            currentMoveNumber = 2;
                        }
                        else
                        {
                            currentMoveNumber = 0;
                        }
                        updateSelectedMove(moveset[currentMoveNumber]);
                        SfxHandler.Play(scrollClip);
                        yield return StartCoroutine(moveMoveSelector(positions[currentMoveNumber] + positionMod));
                    }
                    else if (currentMoveNumber == 5)
                    {
                        if (!string.IsNullOrEmpty(moveset[3]))
                        {
                            currentMoveNumber = 3;
                        }
                        else if (!string.IsNullOrEmpty(moveset[1]))
                        {
                            currentMoveNumber = 1;
                        }
                        else
                        {
                            currentMoveNumber = 0;
                        }
                        updateSelectedMove(moveset[currentMoveNumber]);
                        SfxHandler.Play(scrollClip);
                        yield return StartCoroutine(moveMoveSelector(positions[currentMoveNumber] + positionMod));
                    }
                }
            }
            else if (Input.GetAxisRaw("Vertical") < 0)
            {
                if (currentMoveNumber == 0)
                {
                    if (!string.IsNullOrEmpty(moveset[2]))
                    {
                        currentMoveNumber = 2;
                        updateSelectedMove(moveset[currentMoveNumber]);
                        SfxHandler.Play(scrollClip);
                        yield return StartCoroutine(moveMoveSelector(positions[currentMoveNumber] + positionMod));
                    }
                    else if (learning)
                    {
                        currentMoveNumber = 4;
                        updateSelectedMove(moveset[currentMoveNumber]);
                        SfxHandler.Play(scrollClip);
                        yield return StartCoroutine(moveMoveSelector(positions[currentMoveNumber] + positionMod));
                    }
                }
                else if (currentMoveNumber == 1)
                {
                    if (!string.IsNullOrEmpty(moveset[3]))
                    {
                        currentMoveNumber = 3;
                        updateSelectedMove(moveset[currentMoveNumber]);
                        SfxHandler.Play(scrollClip);
                        yield return StartCoroutine(moveMoveSelector(positions[currentMoveNumber] + positionMod));
                    }
                    else if (learning)
                    {
                        currentMoveNumber = 5;
                        updateSelectedMove(moveset[currentMoveNumber]);
                        SfxHandler.Play(scrollClip);
                        yield return StartCoroutine(moveMoveSelector(positions[currentMoveNumber] + positionMod));
                    }
                }
                else if (learning)
                {
                    if (currentMoveNumber == 2)
                    {
                        currentMoveNumber = 4;
                        updateSelectedMove(moveset[currentMoveNumber]);
                        SfxHandler.Play(scrollClip);
                        yield return StartCoroutine(moveMoveSelector(positions[currentMoveNumber] + positionMod));
                    }
                    else if (currentMoveNumber == 3)
                    {
                        currentMoveNumber = 5;
                        updateSelectedMove(moveset[currentMoveNumber]);
                        SfxHandler.Play(scrollClip);
                        yield return StartCoroutine(moveMoveSelector(positions[currentMoveNumber] + positionMod));
                    }
                }
            }
            else if (Input.GetButtonDown("Back"))
            {
                if (!learning)
                {
                    if (selectingMove)
                    {
                        selectingMove = false;
                        selectedMove.enabled = false;
                        yield return new WaitForSeconds(0.2f);
                    }
                    else
                    {
                        navigatingMoves = false;
                        moveSelector.enabled = false;
                        updateSelectedMove(null);
                        SfxHandler.Play(returnClip);
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                else
                {
                    //Cancel learning move
                    navigatingMoves = false;
                    SfxHandler.Play(returnClip);
                    yield return new WaitForSeconds(0.2f);
                }
            }
            else if (Input.GetButtonDown("Select"))
            {
                if (!learning)
                {
                    if (selectingMove)
                    {
                        pokemon.swapMoves(selectedMoveNumber, currentMoveNumber);
                        selectingMove = false;
                        selectedMove.enabled = false;
                        moveset = pokemon.getMoveset();
                        updateSelectionMoveset(pokemon);
                        updateSelectedMove(moveset[currentMoveNumber]);
                        SfxHandler.Play(selectClip);
                        yield return new WaitForSeconds(0.2f);
                    }
                    else
                    {
                        selectedMoveNumber = currentMoveNumber;
                        selectingMove = true;
                        selectedMove.rectTransform.localPosition = positions[currentMoveNumber] + positionMod;
                        selectedMove.enabled = true;
                        SfxHandler.Play(selectClip);
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                else
                {
                    if (currentMoveNumber < 4)
                    {
                        //Forget learned move
                        forget.SetActive(true);
                        selectedMove.enabled = true;
                        selectedMove.rectTransform.localPosition = positions[currentMoveNumber] + positionMod;
                        moveSelector.rectTransform.localPosition = positions[4] + positionMod;
                        SfxHandler.Play(selectClip);
                        yield return new WaitForSeconds(0.2f);

                        bool forgetPrompt = true;
                        while (forgetPrompt)
                        {
                            if (Input.GetButtonDown("Select"))
                            {
                                replacedMove = moveset[currentMoveNumber];
                                pokemon.replaceMove(currentMoveNumber, newMoveString);

                                forgetPrompt = false;
                                navigatingMoves = false;
                                SfxHandler.Play(selectClip);
                                yield return new WaitForSeconds(0.2f);
                            }
                            else if (Input.GetButtonDown("Back"))
                            {
                                forget.SetActive(false);
                                selectedMove.enabled = false;
                                moveSelector.rectTransform.localPosition = positions[currentMoveNumber] + positionMod;

                                forgetPrompt = false;
                                SfxHandler.Play(returnClip);
                                yield return new WaitForSeconds(0.2f);
                            }
                            yield return null;
                        }
                    }
                    else
                    {
                        //Cancel learning move
                        navigatingMoves = false;
                        SfxHandler.Play(selectClip);
                        yield return new WaitForSeconds(0.2f);
                    }
                }
            }

            yield return null;
        }
    }
}