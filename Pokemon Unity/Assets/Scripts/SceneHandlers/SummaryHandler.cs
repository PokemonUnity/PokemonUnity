//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PokemonUnity.Monster;

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
        Transform selectedInfo = transform.Find("SelectedInfo");

        selectedCaughtBall = selectedInfo.Find("SelectedCaughtBall").GetComponent<Image>();
        selectedNameShadow = selectedInfo.Find("SelectedName").GetComponent<Text>();
        selectedName = selectedNameShadow.transform.Find("Text").GetComponent<Text>();
        selectedGenderShadow = selectedInfo.Find("SelectedGender").GetComponent<Text>();
        selectedGender = selectedGenderShadow.transform.Find("Text").GetComponent<Text>();
        selectedLevelShadow = selectedInfo.Find("SelectedLevel").GetComponent<Text>();
        selectedLevel = selectedLevelShadow.transform.Find("Text").GetComponent<Text>();
        selectedSprite = selectedInfo.Find("SelectedSprite").GetComponent<Image>();
        selectedHeldItemShadow = selectedInfo.Find("SelectedHeldItem").GetComponent<Text>();
        selectedHeldItem = selectedHeldItemShadow.transform.Find("Text").GetComponent<Text>();
        selectedStatus = selectedInfo.Find("SelectedStatus").GetComponent<Image>();
        selectedShiny = selectedInfo.Find("SelectedShiny").GetComponent<Image>();

        pages[1] = transform.Find("1").gameObject;

        dexNoShadow = pages[1].transform.Find("DexNo").GetComponent<Text>();
        dexNo = dexNoShadow.transform.Find("Text").GetComponent<Text>();
        speciesShadow = pages[1].transform.Find("Species").GetComponent<Text>();
        species = speciesShadow.transform.Find("Text").GetComponent<Text>();
        type1 = pages[1].transform.Find("Type1").GetComponent<Image>();
        type2 = pages[1].transform.Find("Type2").GetComponent<Image>();
        OTShadow = pages[1].transform.Find("OT").GetComponent<Text>();
        OT = OTShadow.transform.Find("Text").GetComponent<Text>();
        IDNoShadow = pages[1].transform.Find("IDNo").GetComponent<Text>();
        IDNo = IDNoShadow.transform.Find("Text").GetComponent<Text>();
        expPointsShadow = pages[1].transform.Find("ExpPoints").GetComponent<Text>();
        expPoints = expPointsShadow.transform.Find("Text").GetComponent<Text>();
        toNextLevelShadow = pages[1].transform.Find("ToNextLevel").GetComponent<Text>();
        toNextLevel = toNextLevelShadow.transform.Find("Text").GetComponent<Text>();
        expBar = pages[1].transform.Find("ExpBar").GetComponent<Image>();

        pages[2] = transform.Find("2").gameObject;

        natureShadow = pages[2].transform.Find("Nature").GetComponent<Text>();
        nature = natureShadow.transform.Find("Text").GetComponent<Text>();
        metDateShadow = pages[2].transform.Find("MetDate").GetComponent<Text>();
        metDate = metDateShadow.transform.Find("Text").GetComponent<Text>();
        metMapShadow = pages[2].transform.Find("MetMap").GetComponent<Text>();
        metMap = metMapShadow.transform.Find("Text").GetComponent<Text>();
        metLevelShadow = pages[2].transform.Find("MetLevel").GetComponent<Text>();
        metLevel = metLevelShadow.transform.Find("Text").GetComponent<Text>();
        characteristicShadow = pages[2].transform.Find("Characteristic").GetComponent<Text>();
        characteristic = characteristicShadow.transform.Find("Text").GetComponent<Text>();

        pages[3] = transform.Find("3").gameObject;

        HPTextShadow = pages[3].transform.Find("HPText").GetComponent<Text>();
        HPText = HPTextShadow.transform.Find("Text").GetComponent<Text>();
        HPShadow = pages[3].transform.Find("HP").GetComponent<Text>();
        HP = HPShadow.transform.Find("Text").GetComponent<Text>();
        HPBar = pages[3].transform.Find("HPBar").GetComponent<Image>();
        StatsShadow = pages[3].transform.Find("Stats").GetComponent<Text>();
        Stats = StatsShadow.transform.Find("Text").GetComponent<Text>();
        StatsTextShadow = pages[3].transform.Find("StatsText").GetComponent<Text>();
        abilityNameShadow = pages[3].transform.Find("AbilityName").GetComponent<Text>();
        abilityName = abilityNameShadow.transform.Find("Text").GetComponent<Text>();
        abilityDescriptionShadow = pages[3].transform.Find("AbilityDescription").GetComponent<Text>();
        abilityDescription = abilityDescriptionShadow.transform.Find("Text").GetComponent<Text>();

        pages[4] = transform.Find("4").gameObject;

        moves = pages[4].transform.Find("Moves").GetComponent<RectTransform>();

        moveSelector = pages[4].transform.Find("MoveSelector").GetComponent<Image>();
        selectedMove = pages[4].transform.Find("SelectedMove").GetComponent<Image>();

        move1NameShadow = moves.Find("Move1").GetComponent<Text>();
        move1Name = move1NameShadow.transform.Find("Text").GetComponent<Text>();
        move1Type = moves.Find("Move1Type").GetComponent<Image>();
        move1PPTextShadow = moves.Find("Move1PPText").GetComponent<Text>();
        move1PPText = move1PPTextShadow.transform.Find("Text").GetComponent<Text>();
        move1PPShadow = moves.Find("Move1PP").GetComponent<Text>();
        move1PP = move1PPShadow.transform.Find("Text").GetComponent<Text>();
        move2NameShadow = moves.Find("Move2").GetComponent<Text>();
        move2Name = move2NameShadow.transform.Find("Text").GetComponent<Text>();
        move2Type = moves.Find("Move2Type").GetComponent<Image>();
        move2PPTextShadow = moves.Find("Move2PPText").GetComponent<Text>();
        move2PPText = move2PPTextShadow.transform.Find("Text").GetComponent<Text>();
        move2PPShadow = moves.Find("Move2PP").GetComponent<Text>();
        move2PP = move2PPShadow.transform.Find("Text").GetComponent<Text>();
        move3NameShadow = moves.Find("Move3").GetComponent<Text>();
        move3Name = move3NameShadow.transform.Find("Text").GetComponent<Text>();
        move3Type = moves.Find("Move3Type").GetComponent<Image>();
        move3PPTextShadow = moves.Find("Move3PPText").GetComponent<Text>();
        move3PPText = move3PPTextShadow.transform.Find("Text").GetComponent<Text>();
        move3PPShadow = moves.Find("Move3PP").GetComponent<Text>();
        move3PP = move3PPShadow.transform.Find("Text").GetComponent<Text>();
        move4NameShadow = moves.Find("Move4").GetComponent<Text>();
        move4Name = move4NameShadow.transform.Find("Text").GetComponent<Text>();
        move4Type = moves.Find("Move4Type").GetComponent<Image>();
        move4PPTextShadow = moves.Find("Move4PPText").GetComponent<Text>();
        move4PPText = move4PPTextShadow.transform.Find("Text").GetComponent<Text>();
        move4PPShadow = moves.Find("Move4PP").GetComponent<Text>();
        move4PP = move4PPShadow.transform.Find("Text").GetComponent<Text>();
        selectedCategory = pages[4].transform.Find("SelectedCategory").GetComponent<Image>();
        selectedPowerShadow = pages[4].transform.Find("SelectedPower").GetComponent<Text>();
        selectedPower = selectedPowerShadow.transform.Find("Text").GetComponent<Text>();
        selectedAccuracyShadow = pages[4].transform.Find("SelectedAccuracy").GetComponent<Text>();
        selectedAccuracy = selectedAccuracyShadow.transform.Find("Text").GetComponent<Text>();
        selectedDescriptionShadow = pages[4].transform.Find("SelectedDescription").GetComponent<Text>();
        selectedDescription = selectedDescriptionShadow.transform.Find("Text").GetComponent<Text>();

        learnScreen = pages[4].transform.Find("Learn").gameObject;

        newMove = pages[4].transform.Find("NewMove").GetComponent<RectTransform>();

        moveNewNameShadow = newMove.Find("MoveNew").GetComponent<Text>();
        moveNewName = moveNewNameShadow.transform.Find("Text").GetComponent<Text>();
        moveNewType = newMove.Find("MoveNewType").GetComponent<Image>();
        moveNewPPTextShadow = newMove.Find("MoveNewPPText").GetComponent<Text>();
        moveNewPPText = moveNewPPTextShadow.transform.Find("Text").GetComponent<Text>();
        moveNewPPShadow = newMove.Find("MoveNewPP").GetComponent<Text>();
        moveNewPP = moveNewPPShadow.transform.Find("Text").GetComponent<Text>();
        forget = newMove.Find("Forget").gameObject;


        pages[5] = transform.Find("5").gameObject;

        pages[6] = transform.Find("6").gameObject;
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
        selectedCaughtBall.sprite = Resources.Load<Sprite>("PCSprites/summary" + selectedPokemon.ballUsed.toString());
        selectedName.text = selectedPokemon.Name;
        selectedNameShadow.text = selectedName.text;


        if (selectedPokemon.IsGenderless)
        {
            selectedGender.text = null;
        }
        if (!selectedPokemon.IsMale)
        {
            selectedGender.text = "♀";
            selectedGender.color = new Color(1, 0.2f, 0.2f, 1);
        }
        else if (selectedPokemon.IsMale)
        {
            selectedGender.text = "♂";
            selectedGender.color = new Color(0.2f, 0.4f, 1, 1);
        }
        selectedGenderShadow.text = selectedGender.text;
        selectedLevel.text = "" + selectedPokemon.Level;
        selectedLevelShadow.text = selectedLevel.text;
        selectedSpriteAnimation = selectedPokemon.GetFrontAnim_();
        if (selectedSpriteAnimation.Length > 0)
        {
            selectedSprite.sprite = selectedSpriteAnimation[0];
        }
        if (!selectedPokemon.hasItem())
        {
            selectedHeldItem.text = "None";
        }
        else
        {
            selectedHeldItem.text = selectedPokemon.Item.toString();
        }
        selectedHeldItemShadow.text = selectedHeldItem.text;
        if (selectedPokemon.Status != PokemonUnity.Status.NONE)
        {
            selectedStatus.sprite = Resources.Load<Sprite>("PCSprites/status" + selectedPokemon.Status.ToString());
        }
        else
        {
            selectedStatus.sprite = Resources.Load<Sprite>("null");
        }

        if (selectedPokemon.IsShiny)
        {
            selectedShiny.sprite = Resources.Load<Sprite>("PCSprites/shiny");
        }
        else
        {
            selectedShiny.sprite = Resources.Load<Sprite>("null");
        }

        dexNo.text = ((int)selectedPokemon.Species).ToString();
        dexNoShadow.text = dexNo.text;
        species.text = selectedPokemon.Species.toString();
        speciesShadow.text = species.text;
        string type1string = selectedPokemon.Type1.ToString();
        string type2string = selectedPokemon.Type2.ToString();
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
        OT.text = selectedPokemon.OT.Value.Name;
        OTShadow.text = OT.text;
        IDNo.text = int.Parse(selectedPokemon.TrainerId).ToString();
        IDNoShadow.text = IDNo.text;
        expPoints.text = selectedPokemon.Exp.ToString();
        expPointsShadow.text = expPoints.text;
        float expCurrentLevel =
            //PokemonDatabase.getLevelExp(selectedPokemon.GrowthRate,
            //    selectedPokemon.Level);
            PokemonUnity.Monster.Data.Experience.GetStartExperience(selectedPokemon.GrowthRate,
                selectedPokemon.Level);
        float expNextlevel =
            selectedPokemon.Experience.NextLevel;
        float expAlong = selectedPokemon.Exp - expCurrentLevel;
        float expDistance = expAlong / (expNextlevel - expCurrentLevel);
        toNextLevel.text = (expNextlevel - selectedPokemon.Exp).ToString();
        toNextLevelShadow.text = toNextLevel.text;
        expBar.rectTransform.sizeDelta = new Vector2(Mathf.Floor(expDistance * 64f), expBar.rectTransform.sizeDelta.y);

        string natureFormatted = selectedPokemon.Nature.ToString();
        natureFormatted = natureFormatted.Substring(0, 1) + natureFormatted.Substring(1).ToLowerInvariant();
        nature.text = "<color=#F22F>" + natureFormatted + "</color> nature.";
        natureShadow.text = natureFormatted + " nature.";
        metDate.text = "Met on " + "Not implemented";
        metDateShadow.text = metDate.text;
        metMap.text = "<color=#F22F>" + "Not implemented" + "</color>";
        metMapShadow.text = "Not implemented";
        metLevel.text = "Met at Level " + selectedPokemon.ObtainLevel + ".";
        metLevelShadow.text = metLevel.text;

        string[][] characteristics = new string[][]
        {
            new string[] // HP
            {
                "Loves to eat", "Takes plenty of siestas", "Nods off a lot", "Scatters things often", "Likes to relax"
            },
            new string[] // ATK?
            {
                "Proud of its power", "Likes to thrash about", "A little quick tempered", "Likes to fight",
                "Quick tempered"
            },
            new string[] // DEF?
            {
                "Sturdy body", "Capable of taking hits", "Highly persistent", "Good endurance", "Good perseverance"
            },
            new string[] // SPE?
            {
                "Likes to run", "Alert to sounds", "Impetuous and silly", "Somewhat of a clown", "Quick to flee"
            },
            new string[] // SPA?
            {
                "Highly curious", "Mischievous", "Thoroughly cunning", "Often lost in thought", "Very finicky"
            },
            new string[] // SPD?
            {
                "Strong willed", "Somewhat vain", "Strongly defiant", "Hates to lose", "Somewhat stubborn"
            }
        };
        int highestIV = selectedPokemon.GetHighestIV();
        characteristic.text = characteristics[highestIV][selectedPokemon.GetIV(highestIV) % 5] + ".";
        characteristicShadow.text = characteristic.text;

        float currentHP = selectedPokemon.HP;
        float maxHP = selectedPokemon.TotalHP;
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
            PokemonUnity.Game.NatureData[selectedPokemon.Nature].ATK,
            PokemonUnity.Game.NatureData[selectedPokemon.Nature].DEF,
            PokemonUnity.Game.NatureData[selectedPokemon.Nature].SPA,
            PokemonUnity.Game.NatureData[selectedPokemon.Nature].SPD,
            PokemonUnity.Game.NatureData[selectedPokemon.Nature].SPE,
        };
        Stats.text =
            selectedPokemon.ATK + "\n" +
            selectedPokemon.DEF + "\n" +
            selectedPokemon.SPA + "\n" +
            selectedPokemon.SPD + "\n" +
            selectedPokemon.SPE;
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


        abilityName.text = selectedPokemon.Ability.ToString();
        abilityNameShadow.text = abilityName.text;
        //abilities not yet implemented
        abilityDescription.text = "";
        abilityDescriptionShadow.text = abilityDescription.text;

        updateSelectionMoveset(selectedPokemon);
    }

    private void updateSelectionMoveset(Pokemon selectedPokemon)
    {
        PokemonUnity.Attack.Move[] moveset = selectedPokemon.moves;
        int[] maxPP = selectedPokemon.GetMaxPP();
        int[] PP = selectedPokemon.GetPP();
        if (moveset[0].MoveId != PokemonUnity.Moves.NONE)
        {
            move1Name.text = moveset[0].MoveId.toString();
            move1NameShadow.text = move1Name.text;
            move1Type.sprite =
                Resources.Load<Sprite>("PCSprites/type" + moveset[0].Type.ToString());
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
        if (moveset[1].MoveId != PokemonUnity.Moves.NONE)
        {
            move2Name.text = moveset[1].MoveId.toString();
            move2NameShadow.text = move2Name.text;
            move2Type.sprite =
                Resources.Load<Sprite>("PCSprites/type" + moveset[1].Type.ToString());
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
        if (moveset[2].MoveId != PokemonUnity.Moves.NONE)
        {
            move3Name.text = moveset[2].MoveId.toString();
            move3NameShadow.text = move3Name.text;
            move3Type.sprite =
                Resources.Load<Sprite>("PCSprites/type" + moveset[2].Type.ToString());
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
        if (moveset[3].MoveId != PokemonUnity.Moves.NONE)
        {
            move4Name.text = moveset[3].MoveId.toString();
            move4NameShadow.text = move4Name.text;
            move4Type.sprite =
                Resources.Load<Sprite>("PCSprites/type" + moveset[3].Type.ToString());
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

        updateSelectedMove(PokemonUnity.Moves.NONE);
    }

    private void updateMoveToLearn(PokemonUnity.Moves moveName)
    {
        PokemonUnity.Attack.Move move = new PokemonUnity.Attack.Move(moveName);
        moveNewName.text = moveName.toString();
        moveNewNameShadow.text = moveNewName.text;
        moveNewType.sprite = Resources.Load<Sprite>("PCSprites/type" + move.Type.ToString());
        moveNewPPText.text = "PP";
        moveNewPPTextShadow.text = moveNewPPText.text;
        moveNewPP.text = move.TotalPP + "/" + move.TotalPP;
        moveNewPPShadow.text = moveNewPP.text;
    }

    private void updateSelectedMove(PokemonUnity.Moves moveName)
    {
        if (moveName == PokemonUnity.Moves.NONE)
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
            PokemonUnity.Attack.Move selectedMove = new PokemonUnity.Attack.Move(moveName);
            selectedCategory.sprite =
                Resources.Load<Sprite>("PCSprites/category" + selectedMove.Category.ToString());
            selectedPower.text = "" + selectedMove.Power;
            if (selectedPower.text == "0")
            {
                selectedPower.text = "-";
            }
            selectedPowerShadow.text = selectedPower.text;
            selectedAccuracy.text = "" + Mathf.Round(selectedMove.Accuracy.GetValueOrDefault() /* 100f*/);
            if (selectedAccuracy.text == "0")
            {
                selectedAccuracy.text = "-";
            }
            selectedAccuracyShadow.text = selectedAccuracy.text;
            selectedDescription.text = PokemonUnity.Game.MoveData[selectedMove.MoveId].Description;
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
        yield return StartCoroutine(control(pokemonList, currentPosition, false, PokemonUnity.Moves.NONE));
    }

    public IEnumerator control(Pokemon pokemon, PokemonUnity.Moves newMoveString)
    {
        yield return StartCoroutine(control(new Pokemon[] {pokemon}, 0, true, newMoveString));
    }

    public IEnumerator control(Pokemon[] pokemonList, int currentPosition, bool learning, PokemonUnity.Moves newMoveString)
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
            yield return StartCoroutine(NavigateMoves(pokemonList[currentPosition], true, newMoveString.toString()));
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
                        if (pokemonList[currentPosition].moves[0].MoveId != PokemonUnity.Moves.NONE)
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
            updateMoveToLearn(newMoveString.ToMoves());
        }

        PokemonUnity.Attack.Move[] pokeMoveset = pokemon.moves;
        string[] moveset = new string[]
        {
            pokeMoveset[0].MoveId.toString(), pokeMoveset[1].MoveId.toString(),
            pokeMoveset[2].MoveId.toString(), pokeMoveset[3].MoveId.toString(),
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
        updateSelectedMove(moveset[currentMoveNumber].ToMoves());
        yield return null;
        while (navigatingMoves)
        {
            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                if (currentMoveNumber == 1)
                {
                    currentMoveNumber = 0;
                    updateSelectedMove(moveset[currentMoveNumber].ToMoves());
                    SfxHandler.Play(scrollClip);
                    yield return StartCoroutine(moveMoveSelector(positions[currentMoveNumber] + positionMod));
                }
                else if (currentMoveNumber == 3)
                {
                    if (!string.IsNullOrEmpty(moveset[2]))
                    {
                        currentMoveNumber = 2;
                        updateSelectedMove(moveset[currentMoveNumber].ToMoves());
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
                        updateSelectedMove(moveset[currentMoveNumber].ToMoves());
                        SfxHandler.Play(scrollClip);
                        yield return StartCoroutine(moveMoveSelector(positions[currentMoveNumber] + positionMod));
                    }
                }
                else if (currentMoveNumber == 2)
                {
                    if (!string.IsNullOrEmpty(moveset[3]))
                    {
                        currentMoveNumber = 3;
                        updateSelectedMove(moveset[currentMoveNumber].ToMoves());
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
                    updateSelectedMove(moveset[currentMoveNumber].ToMoves());
                    SfxHandler.Play(scrollClip);
                    yield return StartCoroutine(moveMoveSelector(positions[currentMoveNumber] + positionMod));
                }
                else if (currentMoveNumber == 3)
                {
                    if (!string.IsNullOrEmpty(moveset[1]))
                    {
                        currentMoveNumber = 1;
                        updateSelectedMove(moveset[currentMoveNumber].ToMoves());
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
                        updateSelectedMove(moveset[currentMoveNumber].ToMoves());
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
                        updateSelectedMove(moveset[currentMoveNumber].ToMoves());
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
                        updateSelectedMove(moveset[currentMoveNumber].ToMoves());
                        SfxHandler.Play(scrollClip);
                        yield return StartCoroutine(moveMoveSelector(positions[currentMoveNumber] + positionMod));
                    }
                    else if (learning)
                    {
                        currentMoveNumber = 4;
                        updateSelectedMove(moveset[currentMoveNumber].ToMoves());
                        SfxHandler.Play(scrollClip);
                        yield return StartCoroutine(moveMoveSelector(positions[currentMoveNumber] + positionMod));
                    }
                }
                else if (currentMoveNumber == 1)
                {
                    if (!string.IsNullOrEmpty(moveset[3]))
                    {
                        currentMoveNumber = 3;
                        updateSelectedMove(moveset[currentMoveNumber].ToMoves());
                        SfxHandler.Play(scrollClip);
                        yield return StartCoroutine(moveMoveSelector(positions[currentMoveNumber] + positionMod));
                    }
                    else if (learning)
                    {
                        currentMoveNumber = 5;
                        updateSelectedMove(moveset[currentMoveNumber].ToMoves());
                        SfxHandler.Play(scrollClip);
                        yield return StartCoroutine(moveMoveSelector(positions[currentMoveNumber] + positionMod));
                    }
                }
                else if (learning)
                {
                    if (currentMoveNumber == 2)
                    {
                        currentMoveNumber = 4;
                        updateSelectedMove(moveset[currentMoveNumber].ToMoves());
                        SfxHandler.Play(scrollClip);
                        yield return StartCoroutine(moveMoveSelector(positions[currentMoveNumber] + positionMod));
                    }
                    else if (currentMoveNumber == 3)
                    {
                        currentMoveNumber = 5;
                        updateSelectedMove(moveset[currentMoveNumber].ToMoves());
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
                        updateSelectedMove(PokemonUnity.Moves.NONE);
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
                        moveset = new string[]
                        {
                            pokemon.moves[0].MoveId.toString(), pokemon.moves[1].MoveId.toString(),
                            pokemon.moves[2].MoveId.toString(), pokemon.moves[3].MoveId.toString(),
                            newMoveString, newMoveString
                        };
                        //moveset = pokemon.moves;
                        updateSelectionMoveset(pokemon);
                        updateSelectedMove(moveset[currentMoveNumber].ToMoves());
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
                                pokemon.replaceMove(currentMoveNumber, newMoveString.ToMoves());

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