//Original Scripts by IIColour (IIColour_Spectrum)

using System.Collections;
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
    private UnityEngine.Sprite[] selectedSpriteAnimation;

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
        natureShadow = pages[1].transform.Find("Nature").GetComponent<Text>();
        nature = natureShadow.transform.Find("Text").GetComponent<Text>();
        metDateShadow = pages[1].transform.Find("MetDate").GetComponent<Text>();
        metDate = metDateShadow.transform.Find("Text").GetComponent<Text>();
        metMapShadow = pages[1].transform.Find("MetMap").GetComponent<Text>();
        metMap = metMapShadow.transform.Find("Text").GetComponent<Text>();
        metLevelShadow = pages[1].transform.Find("MetLevel").GetComponent<Text>();
        metLevel = metLevelShadow.transform.Find("Text").GetComponent<Text>();
        characteristicShadow = pages[1].transform.Find("Characteristic").GetComponent<Text>();
        characteristic = characteristicShadow.transform.Find("Text").GetComponent<Text>();
        
        pages[2] = transform.Find("2").gameObject;

        HPTextShadow = pages[2].transform.Find("HPText").GetComponent<Text>();
        HPText = HPTextShadow.transform.Find("Text").GetComponent<Text>();
        HPShadow = pages[2].transform.Find("HP").GetComponent<Text>();
        HP = HPShadow.transform.Find("Text").GetComponent<Text>();
        HPBar = pages[2].transform.Find("HPBar").GetComponent<Image>();
        StatsShadow = pages[2].transform.Find("Stats").GetComponent<Text>();
        Stats = StatsShadow.transform.Find("Text").GetComponent<Text>();
        StatsTextShadow = pages[2].transform.Find("StatsText").GetComponent<Text>();
        abilityNameShadow = pages[2].transform.Find("AbilityName").GetComponent<Text>();
        abilityName = abilityNameShadow.transform.Find("Text").GetComponent<Text>();
        abilityDescriptionShadow = pages[2].transform.Find("AbilityDescription").GetComponent<Text>();
        abilityDescription = abilityDescriptionShadow.transform.Find("Text").GetComponent<Text>();
        
        moveSelector = pages[2].transform.Find("MoveSelector").GetComponent<Image>();
        selectedMove = pages[2].transform.Find("SelectedMove").GetComponent<Image>();
        
        moves = pages[2].transform.Find("Moves").GetComponent<RectTransform>();

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
        selectedCategory = pages[2].transform.Find("SelectedCategory").GetComponent<Image>();
        selectedPowerShadow = pages[2].transform.Find("SelectedPower").GetComponent<Text>();
        selectedPower = selectedPowerShadow.transform.Find("Text").GetComponent<Text>();
        selectedAccuracyShadow = pages[2].transform.Find("SelectedAccuracy").GetComponent<Text>();
        selectedAccuracy = selectedAccuracyShadow.transform.Find("Text").GetComponent<Text>();
        selectedDescriptionShadow = pages[2].transform.Find("SelectedDescription").GetComponent<Text>();
        selectedDescription = selectedDescriptionShadow.transform.Find("Text").GetComponent<Text>();

        learnScreen = pages[2].transform.Find("Learn").gameObject;

        newMove = pages[2].transform.Find("NewMove").GetComponent<RectTransform>();

        moveNewNameShadow = newMove.Find("MoveNew").GetComponent<Text>();
        moveNewName = moveNewNameShadow.transform.Find("Text").GetComponent<Text>();
        moveNewType = newMove.Find("MoveNewType").GetComponent<Image>();
        moveNewPPTextShadow = newMove.Find("MoveNewPPText").GetComponent<Text>();
        moveNewPPText = moveNewPPTextShadow.transform.Find("Text").GetComponent<Text>();
        moveNewPPShadow = newMove.Find("MoveNewPP").GetComponent<Text>();
        moveNewPP = moveNewPPShadow.transform.Find("Text").GetComponent<Text>();
        forget = newMove.Find("Forget").gameObject;
    }

    void Start()
    {
        gameObject.SetActive(false);
    }


    private void updateSelection(IPokemon selectedPokemon)
    {
        frame = 0;

        PlayCry(selectedPokemon);

        //selectedCaughtBall.sprite = Resources.Load<Sprite>("null");
        selectedCaughtBall.sprite = Resources.Load<UnityEngine.Sprite>("PCSprites/summary" + selectedPokemon.ballUsed.ToString());
        selectedName.text = selectedPokemon.Name;
        selectedNameShadow.text = selectedName.text;
        if (selectedPokemon.Gender == false)
        {
            selectedGender.text = "♀";
            selectedGender.color = new Color(1, 0.2f, 0.2f, 1);
        }
        else if (selectedPokemon.Gender == true)
        {
            selectedGender.text = "♂";
            selectedGender.color = new Color(0.2f, 0.4f, 1, 1);
        }
        else
        {
            selectedGender.text = null;
        }
        selectedGenderShadow.text = selectedGender.text;
        selectedLevel.text = "" + selectedPokemon.Level;
        selectedLevelShadow.text = selectedLevel.text;
        selectedSpriteAnimation = new UnityEngine.Sprite[0]; //selectedPokemon.GetFrontAnim_();
        if (selectedSpriteAnimation.Length > 0)
        {
            selectedSprite.sprite = selectedSpriteAnimation[0];
            selectedSprite.rectTransform.sizeDelta = new Vector2(selectedSprite.sprite.texture.width, selectedSprite.sprite.texture.height);
        }
        if (selectedPokemon.Item == Items.NONE)
        {
            selectedHeldItem.text = "None";
        }
        else
        {
            selectedHeldItem.text = selectedPokemon.Item.ToString(TextScripts.Name);
        }
        selectedHeldItemShadow.text = selectedHeldItem.text;
        if (selectedPokemon.Status != Status.NONE)
        {
            selectedStatus.sprite = Resources.Load<UnityEngine.Sprite>("PCSprites/status" + selectedPokemon.Status.ToString());
        }
        else
        {
            selectedStatus.sprite = Resources.Load<UnityEngine.Sprite>("null");
        }

        if (selectedPokemon.IsShiny)
        {
            selectedShiny.sprite = Resources.Load<UnityEngine.Sprite>("PCSprites/shiny");
        }
        else
        {
            selectedShiny.sprite = Resources.Load<UnityEngine.Sprite>("null");
        }

        dexNo.text = selectedPokemon.Species.ToIdString();
        dexNoShadow.text = dexNo.text;
        species.text = selectedPokemon.Species.ToString(TextScripts.Name);
        speciesShadow.text = species.text;
        string type1string = selectedPokemon.Type1.ToString(TextScripts.Name);
        string type2string = selectedPokemon.Type2.ToString(TextScripts.Name);
        type1.sprite = Resources.Load<UnityEngine.Sprite>("null");
        type2.sprite = Resources.Load<UnityEngine.Sprite>("null");
        if (type1string != "NONE")
        {
            type1.sprite = Resources.Load<UnityEngine.Sprite>("PCSprites/type" + type1string);
        }
        if (type2string != "NONE")
        {
            type2.sprite = Resources.Load<UnityEngine.Sprite>("PCSprites/type" + type2string);
        }
        OT.text = ""; //selectedPokemon.getOT();
        OTShadow.text = OT.text;
        IDNo.text = "" + selectedPokemon.Species.ToIdString();
        IDNoShadow.text = IDNo.text;
        expPoints.text = "" + selectedPokemon.Exp;
        expPointsShadow.text = expPoints.text;
        PokemonUnity.Monster.Data.Experience exp = ((PokemonUnity.Monster.Pokemon)selectedPokemon).Experience;
        float expCurrentLevel = selectedPokemon.Level;
        float expNextlevel = exp.NextLevel;
        //    PokemonDatabase.getLevelExp(PokemonDatabase.getPokemon(selectedPokemon.getID()).getLevelingRate(),
        //        selectedPokemon.getLevel() + 1);
        //float expAlong = selectedPokemon.getExp() - expCurrentLevel;
        //float expDistance = expAlong / (expNextlevel - expCurrentLevel);
        toNextLevel.text = "" + exp.PointsNeeded;
        toNextLevelShadow.text = toNextLevel.text;
        expBar.rectTransform.sizeDelta = new Vector2(Mathf.Floor(exp.PointsNeeded * 64f), expBar.rectTransform.sizeDelta.y);

        string natureFormatted = selectedPokemon.Nature.ToString();
        natureFormatted = natureFormatted.Substring(0, 1) + natureFormatted.Substring(1).ToLowerInvariant();
        nature.text = "<color=#33BBFF>" + natureFormatted + "</color> nature.";
        natureShadow.text = natureFormatted + " nature.";
        metDate.text = "Met on " + selectedPokemon.obtainMap;
        metDateShadow.text = metDate.text;
        metMap.text = "<color=#33BBFF>" + selectedPokemon.obtainText + "</color>";
        metMapShadow.text = selectedPokemon.obtainText;
        metLevel.text = "Met at Level " + selectedPokemon.obtainLevel + ".";
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
        //int highestIV = selectedPokemon.GetHighestIV();
        //characteristic.text = characteristics[highestIV][selectedPokemon.GetIV(highestIV) % 5] + ".";
        characteristic.text = selectedPokemon.Nature.ToString(); //ToDo: return and fix this.
        characteristicShadow.text = characteristic.text;

        float currentHP = selectedPokemon.HP;
        float maxHP = selectedPokemon.TotalHP;
        HP.text = currentHP + "/" + maxHP;
        HPShadow.text = HP.text;
        HPBar.rectTransform.sizeDelta = new Vector2((selectedPokemon.HP / selectedPokemon.TotalHP) * 48f,
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
            selectedPokemon.ATK,
            selectedPokemon.DEF,
            selectedPokemon.SPA,
            selectedPokemon.SPD,
            selectedPokemon.SPE
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


        abilityName.text = selectedPokemon.Ability.ToString(TextScripts.Name);
        abilityNameShadow.text = abilityName.text;
        //abilities not yet implemented
        abilityDescription.text = "";
        abilityDescriptionShadow.text = abilityDescription.text;

        //updateSelectionMoveset(selectedPokemon);
    }

    private void updateSelectionMoveset(IPokemon selectedPokemon)
    {
        if (selectedPokemon.moves[0].IsNotNullOrNone())
        {
            move1Name.text = selectedPokemon.moves[0].id.ToString(TextScripts.Name);
            move1NameShadow.text = move1Name.text;
            move1Type.sprite =
                Resources.Load<UnityEngine.Sprite>("PCSprites/type" + selectedPokemon.moves[0].Type.ToString(TextScripts.Name));
            move1PPText.text = "PP";
            move1PPTextShadow.text = move1PPText.text;
            move1PP.text = selectedPokemon.moves[0].PP + "/" + selectedPokemon.moves[0].TotalPP;
            move1PPShadow.text = move1PP.text;
        }
        else
        {
            move1Name.text = null;
            move1NameShadow.text = move1Name.text;
            move1Type.sprite = Resources.Load<UnityEngine.Sprite>("null");
            move1PPText.text = null;
            move1PPTextShadow.text = move1PPText.text;
            move1PP.text = null;
            move1PPShadow.text = move1PP.text;
        }
        if (selectedPokemon.moves[1].IsNotNullOrNone())
        {
            move2Name.text = selectedPokemon.moves[1].id.ToString(TextScripts.Name);
            move2NameShadow.text = move2Name.text;
            move2Type.sprite =
                Resources.Load<UnityEngine.Sprite>("PCSprites/type" + selectedPokemon.moves[1].Type.ToString(TextScripts.Name));
            move2PPText.text = "PP";
            move2PPTextShadow.text = move2PPText.text;
            move2PP.text = selectedPokemon.moves[1].PP + "/" + selectedPokemon.moves[1].TotalPP;
            move2PPShadow.text = move2PP.text;
        }
        else
        {
            move2Name.text = null;
            move2NameShadow.text = move2Name.text;
            move2Type.sprite = Resources.Load<UnityEngine.Sprite>("null");
            move2PPText.text = null;
            move2PPTextShadow.text = move2PPText.text;
            move2PP.text = null;
            move2PPShadow.text = move2PP.text;
        }
        if (selectedPokemon.moves[2].IsNotNullOrNone())
        {
            move3Name.text = selectedPokemon.moves[2].id.ToString(TextScripts.Name);
            move3NameShadow.text = move3Name.text;
            move3Type.sprite =
                Resources.Load<UnityEngine.Sprite>("PCSprites/type" + selectedPokemon.moves[2].Type.ToString(TextScripts.Name));
            move3PPText.text = "PP";
            move3PPTextShadow.text = move3PPText.text;
            move3PP.text = selectedPokemon.moves[2].PP + "/" + selectedPokemon.moves[2].TotalPP;
            move3PPShadow.text = move3PP.text;
        }
        else
        {
            move3Name.text = null;
            move3NameShadow.text = move3Name.text;
            move3Type.sprite = Resources.Load<UnityEngine.Sprite>("null");
            move3PPText.text = null;
            move3PPTextShadow.text = move3PPText.text;
            move3PP.text = null;
            move3PPShadow.text = move3PP.text;
        }
        if (selectedPokemon.moves[3].IsNotNullOrNone())
        {
            move4Name.text = selectedPokemon.moves[3].id.ToString(TextScripts.Name);
            move4NameShadow.text = move4Name.text;
            move4Type.sprite =
                Resources.Load<UnityEngine.Sprite>("PCSprites/type" + selectedPokemon.moves[3].Type.ToString(TextScripts.Name));
            move4PPText.text = "PP";
            move4PPTextShadow.text = move4PPText.text;
            move4PP.text = selectedPokemon.moves[3].PP + "/" + selectedPokemon.moves[3].TotalPP;
            move4PPShadow.text = move4PP.text;
        }
        else
        {
            move4Name.text = null;
            move4NameShadow.text = move4Name.text;
            move4Type.sprite = Resources.Load<UnityEngine.Sprite>("null");
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
        moveNewType.sprite = Resources.Load<UnityEngine.Sprite>("PCSprites/type" + move.getType().ToString());
        moveNewPPText.text = "PP";
        moveNewPPTextShadow.text = moveNewPPText.text;
        moveNewPP.text = move.getPP() + "/" + move.getPP();
        moveNewPPShadow.text = moveNewPP.text;
    }

    private void updateSelectedMove(string moveName)
    {
        if (string.IsNullOrEmpty(moveName))
        {
            selectedCategory.sprite = Resources.Load<UnityEngine.Sprite>("null");
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
                Resources.Load<UnityEngine.Sprite>("PCSprites/category" + selectedMove.getCategory().ToString());
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
                selectedSprite.rectTransform.sizeDelta = new Vector2(selectedSpriteAnimation[frame].rect.width, selectedSpriteAnimation[frame].rect.height);
                selectedSprite.sprite = selectedSpriteAnimation[frame];
            }
            yield return new WaitForSeconds(0.08f);
        }
    }

    private void PlayCry(IPokemon pokemon)
    {
       // SfxHandler.Play(pokemon.GetCry(), pokemon.GetCryPitch());
    }

    public IEnumerator control(IPokemon[] pokemonList, int currentPosition = 0, bool learning = false, string newMoveString = null)
    {
        //moves.localPosition = (learning) ? new Vector3(0, 32) : Vector3.zero;
        newMove.gameObject.SetActive(learning);
        learnScreen.SetActive(learning);

        moveSelector.enabled = false;
        selectedMove.enabled = false;

        forget.SetActive(false);

        pages[1].SetActive(!learning);
        pages[2].SetActive(learning);

        updateSelection(pokemonList[currentPosition]);
        if (learning)
        {
            updateMoveToLearn(newMoveString);
        }

        StartCoroutine(animatePokemon());

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
                if (UnityEngine.Input.GetAxisRaw("Horizontal") < 0)
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
                else if (UnityEngine.Input.GetAxisRaw("Horizontal") > 0)
                {
                    if (currentPage < 2)
                    {
                        pages[currentPage + 1].SetActive(true);
                        pages[currentPage].SetActive(false);
                        currentPage += 1;
                        SfxHandler.Play(scrollClip);
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                //cycle through pokemon
                else if (UnityEngine.Input.GetAxisRaw("Vertical") > 0)
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
                else if (UnityEngine.Input.GetAxisRaw("Vertical") < 0)
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
                else if (UnityEngine.Input.GetButton("Select"))
                {
                    if (currentPage == 4)
                    {
                        //ToDo: Not sure what this script is supposed to do... why is it only the first move?
                        if (pokemonList[currentPosition].moves[0].IsNotNullOrNone())
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
                else if (UnityEngine.Input.GetButton("Back"))
                {
                    running = false;
                }

                yield return null;
            }
        }

        yield return StartCoroutine(ScreenFade.main.Fade(false, ScreenFade.defaultSpeed));
        this.gameObject.SetActive(false);
    }

    private IEnumerator NavigateMoves(IPokemon pokemon, bool learning, string newMoveString)
    {
        learnScreen.SetActive(learning);
        newMove.gameObject.SetActive(learning);
        Vector3 positionMod = (learning) ? new Vector3(0, 32) : new Vector3(0, 0);
        //moves.localPosition = positionMod;
        if (learning)
        {
            updateMoveToLearn(newMoveString);
        }

        //string[] pokeMoveset = pokemon.getMoveset();
        //string[] moveset = new string[]
        //{
        //    pokeMoveset[0], pokeMoveset[1],
        //    pokeMoveset[2], pokeMoveset[3],
        //    newMoveString, newMoveString
        //};
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
        updateSelectedMove(pokemon.moves[currentMoveNumber].id.ToString(TextScripts.Name));
        yield return null;
        //ToDo: Uncomment... use framework with this coroutine...
        /*while (navigatingMoves)
        {
            if (UnityEngine.Input.GetAxisRaw("Horizontal") < 0)
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
            else if (UnityEngine.Input.GetAxisRaw("Horizontal") > 0)
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
            else if (UnityEngine.Input.GetAxisRaw("Vertical") > 0)
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
            else if (UnityEngine.Input.GetAxisRaw("Vertical") < 0)
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
            else if (UnityEngine.Input.GetButtonDown("Back"))
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
            else if (UnityEngine.Input.GetButtonDown("Select"))
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
                            if (UnityEngine.Input.GetButtonDown("Select"))
                            {
                                replacedMove = moveset[currentMoveNumber];
                                pokemon.replaceMove(currentMoveNumber, newMoveString);

                                forgetPrompt = false;
                                navigatingMoves = false;
                                SfxHandler.Play(selectClip);
                                yield return new WaitForSeconds(0.2f);
                            }
                            else if (UnityEngine.Input.GetButtonDown("Back"))
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
        }*/
    }
}