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

public class PCHandler : MonoBehaviour
{
    public enum CursorMode
    {
        Standard,
        HotMove,
        WithdrawDeposit
    }

    public CursorMode cursorMode = CursorMode.Standard;

    public Sprite boxEditIcon;

    private int currentBoxID;
    private int nextBoxID;
    private int previousBoxID;

    private int selectedBoxID;
    private int selectedIndex;

    private DialogBoxHandlerNew Dialog;

    private IPokemon selectedPokemon;

    private Transform selectedInfo;

    private Text selectedName;
    private Text selectedNameShadow;
    private Text selectedGender;
    private Text selectedGenderShadow;
    private int frame = 0;
    private Sprite[] selectedSpriteAnimation;
    private Image selectedSprite;
    private Image selectedType1;
    private Image selectedType2;
    private Text selectedLevel;
    private Text selectedLevelShadow;
    private Text selectedAbility;
    private Text selectedAbilityShadow;
    private Text selectedItem;
    private Text selectedItemShadow;
    private Image selectedStatus;

    private Image cursor;
    private Image grabbedPokemon;
    private Image grabbedPokemonItem;

    private Image[] partyIcons = new Image[6];
    private Image[] partyItems = new Image[6];

    private GameObject currentBox;
    private GameObject nextBox;
    private GameObject previousBox;

    private Image currentBoxTexture;
    private Image nextBoxTexture;
    private Image previousBoxTexture;
    private Text currentBoxHeader;
    private Text nextBoxHeader;
    private Text previousBoxHeader;
    private Text currentBoxHeaderShadow;
    private Text nextBoxHeaderShadow;
    private Text previousBoxHeaderShadow;

    private Transform currentBoxIcons;
    private Image[] currentBoxIconsArray = new Image[30];
    private Image[] currentBoxItemsArray = new Image[30];
    private Transform nextBoxIcons;
    private Image[] nextBoxIconsArray = new Image[30];
    private Image[] nextBoxItemsArray = new Image[30];
    private Transform previousBoxIcons;
    private Image[] previousBoxIconsArray = new Image[30];
    private Image[] previousBoxItemsArray = new Image[30];

    private AudioSource PCaudio;

    public AudioClip offClip;
    public AudioClip openClip;
    public AudioClip selectClip;
    public AudioClip pickUpClip;
    public AudioClip putDownClip;

    private bool running = false;
    private bool carrying = false;
    private bool switching = false;

    //private SceneTransition sceneTransition;

    private float moveSpeed = 0.16f;

    void Awake()
    {
        Dialog = gameObject.GetComponent<DialogBoxHandlerNew>();

        PCaudio = gameObject.GetComponent<AudioSource>();

        //sceneTransition = this.gameObject.GetComponent<SceneTransition>();

        selectedPokemon = null;

        selectedInfo = transform.Find("SelectedInfo");

        selectedNameShadow = selectedInfo.Find("SelectedNameShadow").GetComponent<Text>();
        selectedName = selectedNameShadow.transform.Find("SelectedName").GetComponent<Text>();
        selectedGenderShadow = selectedInfo.Find("SelectedGenderShadow").GetComponent<Text>();
        selectedGender = selectedGenderShadow.transform.Find("SelectedGender").GetComponent<Text>();
        selectedSprite = selectedInfo.Find("SelectedSprite").GetComponent<Image>();
        selectedType1 = selectedInfo.Find("SelectedType1").GetComponent<Image>();
        selectedType2 = selectedInfo.Find("SelectedType2").GetComponent<Image>();
        selectedLevelShadow = selectedInfo.Find("SelectedLevelShadow").GetComponent<Text>();
        selectedLevel = selectedLevelShadow.transform.Find("SelectedLevel").GetComponent<Text>();
        selectedAbilityShadow = selectedInfo.Find("SelectedAbilityShadow").GetComponent<Text>();
        selectedAbility = selectedAbilityShadow.transform.Find("SelectedAbility").GetComponent<Text>();
        selectedItemShadow = selectedInfo.Find("SelectedItemShadow").GetComponent<Text>();
        selectedItem = selectedItemShadow.transform.Find("SelectedItem").GetComponent<Text>();
        selectedStatus = selectedInfo.Find("SelectedStatus").GetComponent<Image>();

        cursor = this.transform.Find("Cursor").GetComponent<Image>();
        grabbedPokemon = cursor.transform.Find("GrabbedPokemon").GetComponent<Image>();
        grabbedPokemonItem = grabbedPokemon.transform.Find("Item").GetComponent<Image>();

        for (int i = 0; i < 6; i++)
        {
            partyIcons[i] = transform.Find("Party").Find("Pokemon" + i).GetComponent<Image>();
            partyItems[i] = partyIcons[i].transform.Find("Item").GetComponent<Image>();
        }

        currentBox = this.transform.Find("CurrentBox").gameObject;
        nextBox = currentBox.transform.Find("NextBox").gameObject;
        previousBox = currentBox.transform.Find("PreviousBox").gameObject;

        currentBoxTexture = currentBox.GetComponent<Image>();
        nextBoxTexture = currentBox.transform.Find("NextBox").GetComponent<Image>();
        previousBoxTexture = currentBox.transform.Find("PreviousBox").GetComponent<Image>();
        
        currentBoxHeaderShadow = currentBox.transform.Find("BoxHeaderShadow").GetComponent<Text>();
        currentBoxHeader = currentBoxHeaderShadow.transform.Find("BoxHeader").GetComponent<Text>();
        
        nextBoxHeaderShadow = currentBox.transform.Find("NextBox").Find("BoxHeaderShadow").GetComponent<Text>();
        nextBoxHeader = nextBoxHeaderShadow.transform.Find("BoxHeader").GetComponent<Text>();
        
        previousBoxHeaderShadow = currentBox.transform.Find("PreviousBox").Find("BoxHeaderShadow").GetComponent<Text>();
        previousBoxHeader = previousBoxHeaderShadow.transform.Find("BoxHeader").GetComponent<Text>();

        currentBoxIcons = currentBox.transform.Find("BoxIcons").transform;
        for (int i = 0; i < 30; i++)
        {
            currentBoxIconsArray[i] = currentBoxIcons.Find("Pokemon" + i).GetComponent<Image>();
            currentBoxItemsArray[i] = currentBoxIconsArray[i].transform.Find("Item").GetComponent<Image>();
        }

        nextBoxIcons = nextBox.transform.Find("BoxIcons").transform;
        for (int i = 0; i < 30; i++)
        {
            nextBoxIconsArray[i] = nextBoxIcons.Find("Pokemon" + i).GetComponent<Image>();
            nextBoxItemsArray[i] = nextBoxIconsArray[i].transform.Find("Item").GetComponent<Image>();
        }

        previousBoxIcons = previousBox.transform.Find("BoxIcons").transform;
        for (int i = 0; i < 30; i++)
        {
            previousBoxIconsArray[i] = previousBoxIcons.Find("Pokemon" + i).GetComponent<Image>();
            previousBoxItemsArray[i] = previousBoxIconsArray[i].transform.Find("Item").GetComponent<Image>();
        }
    }

    void Start()
    {
        this.gameObject.SetActive(false);
    }

    private void updateBoxesAndParty()
    {
        //update box icons
        for (int i = 0; i < 30; i++)
        {
            if (SaveData.currentSave.PC.boxes[currentBoxID][i] == null)
            {
                currentBoxIconsArray[i].sprite = null;
                currentBoxItemsArray[i].enabled = false;
            }
            else
            {
                currentBoxIconsArray[i].sprite = SaveData.currentSave.PC.boxes[currentBoxID][i].GetIcons();
                if (!string.IsNullOrEmpty(SaveData.currentSave.PC.boxes[currentBoxID][i].getHeldItem()))
                {
                    currentBoxItemsArray[i].enabled = true;
                }
                else
                {
                    currentBoxItemsArray[i].enabled = false;
                }
            }
        }
        for (int i = 0; i < 30; i++)
        {
            if (SaveData.currentSave.PC.boxes[nextBoxID][i] == null)
            {
                nextBoxIconsArray[i].sprite = null;
                nextBoxItemsArray[i].enabled = false;
            }
            else
            {
                nextBoxIconsArray[i].sprite = SaveData.currentSave.PC.boxes[nextBoxID][i].GetIcons();
                if (!string.IsNullOrEmpty(SaveData.currentSave.PC.boxes[nextBoxID][i].getHeldItem()))
                {
                    nextBoxItemsArray[i].enabled = true;
                }
                else
                {
                    nextBoxItemsArray[i].enabled = false;
                }
            }
        }
        for (int i = 0; i < 30; i++)
        {
            if (SaveData.currentSave.PC.boxes[previousBoxID][i] == null)
            {
                previousBoxIconsArray[i].sprite = null;
                previousBoxItemsArray[i].enabled = false;
            }
            else
            {
                previousBoxIconsArray[i].sprite = SaveData.currentSave.PC.boxes[previousBoxID][i].GetIcons();
                if (!string.IsNullOrEmpty(SaveData.currentSave.PC.boxes[previousBoxID][i].getHeldItem()))
                {
                    previousBoxItemsArray[i].enabled = true;
                }
                else
                {
                    previousBoxItemsArray[i].enabled = false;
                }
            }
        }

        //update party textures
        for (int i = 0; i < 6; i++)
        {
            if (SaveData.currentSave.PC.boxes[0][i] == null)
            {
                partyIcons[i].sprite = null;
                partyItems[i].enabled = false;
            }
            else
            {
                partyIcons[i].sprite = SaveData.currentSave.PC.boxes[0][i].GetIcons();
                if (!string.IsNullOrEmpty(SaveData.currentSave.PC.boxes[0][i].getHeldItem()))
                {
                    partyItems[i].enabled = true;
                }
                else
                {
                    partyItems[i].enabled = false;
                }
            }
        }

        //update box names
        if (string.IsNullOrEmpty(SaveData.currentSave.PC.boxName[currentBoxID]))
        {
            currentBoxHeader.text = "Box " + currentBoxID;
        }
        else
        {
            currentBoxHeader.text = SaveData.currentSave.PC.boxName[currentBoxID];
        }
        if (string.IsNullOrEmpty(SaveData.currentSave.PC.boxName[nextBoxID]))
        {
            nextBoxHeader.text = "Box " + nextBoxID;
        }
        else
        {
            nextBoxHeader.text = SaveData.currentSave.PC.boxName[nextBoxID];
        }
        if (string.IsNullOrEmpty(SaveData.currentSave.PC.boxName[previousBoxID]))
        {
            previousBoxHeader.text = "Box " + previousBoxID;
        }
        else
        {
            previousBoxHeader.text = SaveData.currentSave.PC.boxName[previousBoxID];
        }
        currentBoxHeaderShadow.text = currentBoxHeader.text;
        nextBoxHeaderShadow.text = nextBoxHeader.text;
        previousBoxHeaderShadow.text = previousBoxHeader.text;

        //update box textures
        if (SaveData.currentSave.PC.boxTexture[currentBoxID] == 0)
        {
            currentBoxTexture.sprite = Resources.Load<Sprite>("PCSprites/box" + currentBoxID);
        }
        else
        {
            currentBoxTexture.sprite =
                Resources.Load<Sprite>("PCSprites/box" + SaveData.currentSave.PC.boxTexture[currentBoxID]);
        }
        if (SaveData.currentSave.PC.boxTexture[nextBoxID] == 0)
        {
            nextBoxTexture.sprite = Resources.Load<Sprite>("PCSprites/box" + nextBoxID);
        }
        else
        {
            nextBoxTexture.sprite =
                Resources.Load<Sprite>("PCSprites/box" + SaveData.currentSave.PC.boxTexture[nextBoxID]);
        }
        if (SaveData.currentSave.PC.boxTexture[previousBoxID] == 0)
        {
            previousBoxTexture.sprite = Resources.Load<Sprite>("PCSprites/box" + previousBoxID);
        }
        else
        {
            previousBoxTexture.sprite =
                Resources.Load<Sprite>("PCSprites/box" + SaveData.currentSave.PC.boxTexture[previousBoxID]);
        }

        //set Selected Info to null because nothing is selected by default
        selectedName.text = null;
        selectedNameShadow.text = null;
        selectedGender.text = null;
        selectedGenderShadow.text = null;
        selectedSpriteAnimation = new Sprite[] {};
        selectedSprite.sprite = null;
        selectedType1.sprite = null;
        selectedType2.sprite = null;
        selectedLevel.text = null;
        selectedLevelShadow.text = null;
        selectedAbility.text = null;
        selectedAbilityShadow.text = null;
        selectedItem.text = null;
        selectedItemShadow.text = null;
        selectedStatus.sprite = null;
    }


    private void updateSelectedInfo(IPokemon selectedPokemon)
    {
        if (!carrying)
        {
            if (selectedPokemon == null)
            {
                selectedName.text = null;
                selectedNameShadow.text = null;
                selectedGender.text = null;
                selectedGenderShadow.text = null;
                selectedSpriteAnimation = new Sprite[] {};
                selectedSprite.sprite = null;
                selectedType1.sprite = null;
                selectedType2.sprite = null;
                selectedLevel.text = null;
                selectedLevelShadow.text = null;
                selectedAbility.text = null;
                selectedAbilityShadow.text = null;
                selectedItem.text = null;
                selectedItemShadow.text = null;
                selectedStatus.sprite = null;
            }
            else
            {
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
                //selectedSpriteAnimation = selectedPokemon.GetFrontAnim_();
                selectedSprite.sprite = selectedSpriteAnimation[0];
                string type1 = selectedPokemon.Type1.ToString(TextScripts.Name);
                string type2 = selectedPokemon.Type2.ToString(TextScripts.Name);
                selectedType1.sprite = null;
                selectedType2.sprite = null;
                if (type1 != "NONE")
                {
                    selectedType1.sprite = Resources.Load<Sprite>("PCSprites/type" + type1);
                }
                if (type2 != "NONE")
                {
                    selectedType2.sprite = Resources.Load<Sprite>("PCSprites/type" + type2);
                }
                selectedLevel.text = "Level " + selectedPokemon.Level;
                selectedLevelShadow.text = selectedLevel.text;
                selectedAbility.text = selectedPokemon.Ability.ToString(TextScripts.Name);
                selectedAbilityShadow.text = selectedAbility.text;
                selectedItem.text = "None";
                if (selectedPokemon.Item != Items.NONE)
                {
                    selectedItem.text = selectedPokemon.Item.ToString(TextScripts.Name);
                }
                selectedItemShadow.text = selectedItem.text;
                selectedStatus.sprite = null;
                if (selectedPokemon.Status != Status.NONE)
                {
                    selectedStatus.sprite =
                        Resources.Load<Sprite>("PCSprites/status" + selectedPokemon.Status.ToString());
                }
            }
        }
    }

    //Show the selectedInfo regardless of carrying or not.
    private void updateSelectedInfoOverride(IPokemon selectedPokemon)
    {
        if (carrying)
        {
            carrying = false;
            updateSelectedInfo(selectedPokemon);
            carrying = true;
        }
        else
        {
            updateSelectedInfo(selectedPokemon);
        }
    }

    private IEnumerator moveBox(int direction)
    {
        float increment = 0;
        float scrollSpeed = 0.4f;
        Vector3 startPosition = currentBox.transform.position;
        if (direction > 0)
        {
            //update destination box's icons incase something has been changed 
            for (int i = 0; i < 30; i++)
            {
                if (SaveData.currentSave.PC.boxes[nextBoxID][i] == null)
                {
                    nextBoxIconsArray[i].sprite = null;
                }
                else
                {
                    nextBoxIconsArray[i].sprite = SaveData.currentSave.PC.boxes[nextBoxID][i].GetIcons();
                }
            }
            Vector3 destinationPosition = startPosition + new Vector3(-0.537f, 0, 0);
            while (increment <= 1)
            {
                increment += (1 / scrollSpeed) * Time.deltaTime;
                currentBox.transform.position = Vector3.Lerp(startPosition, destinationPosition, increment);
                yield return null;
            }
        }
        else if (direction < 0)
        {
            //update destination box's icons incase something has been changed 
            for (int i = 0; i < 30; i++)
            {
                if (SaveData.currentSave.PC.boxes[previousBoxID][i] == null)
                {
                    previousBoxIconsArray[i].sprite = null;
                }
                else
                {
                    previousBoxIconsArray[i].sprite = SaveData.currentSave.PC.boxes[previousBoxID][i].GetIcons();
                }
            }
            Vector3 destinationPosition = startPosition + new Vector3(0.537f, 0, 0);
            while (increment <= 1)
            {
                increment += (1 / scrollSpeed) * Time.deltaTime;
                currentBox.transform.position = Vector3.Lerp(startPosition, destinationPosition, increment);
                yield return null;
            }
        }
        //update BoxIDs
        currentBoxID += direction;
        nextBoxID = currentBoxID + 1;
        previousBoxID = currentBoxID - 1;
        if (currentBoxID == 1)
        {
            previousBoxID = SaveData.currentSave.PC.boxes.Length - 1;
        }
        else if (currentBoxID < 1)
        {
            currentBoxID = SaveData.currentSave.PC.boxes.Length - 1;
            previousBoxID = SaveData.currentSave.PC.boxes.Length - 2;
        }
        else if (currentBoxID == SaveData.currentSave.PC.boxes.Length - 1)
        {
            nextBoxID = 1;
        }
        else if (currentBoxID > SaveData.currentSave.PC.boxes.Length - 1)
        {
            currentBoxID = 1;
            nextBoxID = 2;
        }

        //update box names
        if (string.IsNullOrEmpty(SaveData.currentSave.PC.boxName[currentBoxID]))
        {
            currentBoxHeader.text = "Box " + currentBoxID;
        }
        else
        {
            currentBoxHeader.text = SaveData.currentSave.PC.boxName[currentBoxID];
        }
        if (string.IsNullOrEmpty(SaveData.currentSave.PC.boxName[nextBoxID]))
        {
            nextBoxHeader.text = "Box " + nextBoxID;
        }
        else
        {
            nextBoxHeader.text = SaveData.currentSave.PC.boxName[nextBoxID];
        }
        if (string.IsNullOrEmpty(SaveData.currentSave.PC.boxName[previousBoxID]))
        {
            previousBoxHeader.text = "Box " + previousBoxID;
        }
        else
        {
            previousBoxHeader.text = SaveData.currentSave.PC.boxName[previousBoxID];
        }
        currentBoxHeaderShadow.text = currentBoxHeader.text;
        nextBoxHeaderShadow.text = nextBoxHeader.text;
        previousBoxHeaderShadow.text = previousBoxHeader.text;

        //update box textures
        if (SaveData.currentSave.PC.boxTexture[currentBoxID] == 0)
        {
            currentBoxTexture.sprite = Resources.Load<Sprite>("PCSprites/box" + currentBoxID);
        }
        else
        {
            currentBoxTexture.sprite =
                Resources.Load<Sprite>("PCSprites/box" + SaveData.currentSave.PC.boxTexture[currentBoxID]);
        }
        if (SaveData.currentSave.PC.boxTexture[nextBoxID] == 0)
        {
            nextBoxTexture.sprite = Resources.Load<Sprite>("PCSprites/box" + nextBoxID);
        }
        else
        {
            nextBoxTexture.sprite =
                Resources.Load<Sprite>("PCSprites/box" + SaveData.currentSave.PC.boxTexture[nextBoxID]);
        }
        if (SaveData.currentSave.PC.boxTexture[previousBoxID] == 0)
        {
            previousBoxTexture.sprite = Resources.Load<Sprite>("PCSprites/box" + previousBoxID);
        }
        else
        {
            previousBoxTexture.sprite =
                Resources.Load<Sprite>("PCSprites/box" + SaveData.currentSave.PC.boxTexture[previousBoxID]);
        }

        //update box icons
        for (int i = 0; i < 30; i++)
        {
            if (SaveData.currentSave.PC.boxes[currentBoxID][i] == null)
            {
                currentBoxIconsArray[i].sprite = null;
                currentBoxItemsArray[i].enabled = false;
            }
            else
            {
                currentBoxIconsArray[i].sprite = SaveData.currentSave.PC.boxes[currentBoxID][i].GetIcons();
                if (!string.IsNullOrEmpty(SaveData.currentSave.PC.boxes[currentBoxID][i].getHeldItem()))
                {
                    currentBoxItemsArray[i].enabled = true;
                }
                else
                {
                    currentBoxItemsArray[i].enabled = false;
                }
            }
        }
        for (int i = 0; i < 30; i++)
        {
            if (SaveData.currentSave.PC.boxes[nextBoxID][i] == null)
            {
                nextBoxIconsArray[i].sprite = null;
                nextBoxItemsArray[i].enabled = false;
            }
            else
            {
                nextBoxIconsArray[i].sprite = SaveData.currentSave.PC.boxes[nextBoxID][i].GetIcons();
                if (!string.IsNullOrEmpty(SaveData.currentSave.PC.boxes[nextBoxID][i].getHeldItem()))
                {
                    nextBoxItemsArray[i].enabled = true;
                }
                else
                {
                    nextBoxItemsArray[i].enabled = false;
                }
            }
        }
        for (int i = 0; i < 30; i++)
        {
            if (SaveData.currentSave.PC.boxes[previousBoxID][i] == null)
            {
                previousBoxIconsArray[i].sprite = null;
                previousBoxItemsArray[i].enabled = false;
            }
            else
            {
                previousBoxIconsArray[i].sprite = SaveData.currentSave.PC.boxes[previousBoxID][i].GetIcons();
                if (!string.IsNullOrEmpty(SaveData.currentSave.PC.boxes[previousBoxID][i].getHeldItem()))
                {
                    previousBoxItemsArray[i].enabled = true;
                }
                else
                {
                    previousBoxItemsArray[i].enabled = false;
                }
            }
        }

        currentBox.transform.position = new Vector3(0.27f, currentBox.transform.position.y,
            currentBox.transform.position.z);
        yield return null;
    }

    private IEnumerator moveCursor(Vector2 destination)
    {
        float increment = 0;
        float startX = cursor.rectTransform.position.x;
        float startY = cursor.rectTransform.position.y;
        float distanceX = destination.x - startX;
        float distanceY = destination.y - startY;
        while (increment < 1)
        {
            increment += (1 / moveSpeed) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }
            cursor.rectTransform.position = new Vector2(startX + (increment * distanceX), startY + (increment * distanceY));
            grabbedPokemon.rectTransform.position = new Vector2(cursor.rectTransform.position.x, cursor.rectTransform.position.y - 11);
            grabbedPokemonItem.rectTransform.position = new Vector2(cursor.rectTransform.position.x + 17, cursor.rectTransform.position.y - 9);
            yield return null;
        }
    }

    private IEnumerator animateCursor()
    {
        while (running)
        {
            while (!carrying)
            {
                //cursor.border = new RectOffset(32, 0, 32, 0);
                yield return new WaitForSeconds(0.4f);
                if (!carrying)
                {
                    //cursor.border = new RectOffset(0, 32, 32, 0);
                    yield return new WaitForSeconds(0.4f);
                }
            }
            yield return null;
        }
    }

    private IEnumerator packParty(int currentPosition)
    {
        int hole = currentPosition;
        if (hole < 5)
        {
            Vector2[] partyPositions = new Vector2[]
            {
                new Vector2(partyIcons[0].rectTransform.position.x, partyIcons[0].rectTransform.position.y),
                new Vector2(partyIcons[1].rectTransform.position.x, partyIcons[1].rectTransform.position.y),
                new Vector2(partyIcons[2].rectTransform.position.x, partyIcons[2].rectTransform.position.y),
                new Vector2(partyIcons[3].rectTransform.position.x, partyIcons[3].rectTransform.position.y),
                new Vector2(partyIcons[4].rectTransform.position.x, partyIcons[4].rectTransform.position.y),
                new Vector2(partyIcons[5].rectTransform.position.x, partyIcons[5].rectTransform.position.y)
            };
            while (hole < 4)
            {
                //until the hole is either at the end of the party, or not there.
                SaveData.currentSave.PC.swapPokemon(0, hole, 0, hole + 1);
                StartCoroutine(moveIcon(partyIcons[hole + 1], partyPositions[hole]));
                hole += 1;
            }
            SaveData.currentSave.PC.swapPokemon(0, hole, 0, hole + 1);
            yield return StartCoroutine(moveIcon(partyIcons[hole + 1], partyPositions[hole]));
            hole = currentPosition;
            while (hole < 5)
            {
                partyIcons[hole].sprite = partyIcons[hole + 1].sprite;
                partyItems[hole].enabled = partyItems[hole + 1].enabled;
                partyIcons[hole].rectTransform.position = new Vector2(partyPositions[hole].x, partyPositions[hole].y);
                partyItems[hole].rectTransform.position = new Vector2(partyPositions[hole].x + 17, partyPositions[hole].y + 2);
                hole += 1;
            }
            partyIcons[5].sprite = null;
            partyItems[5].enabled = false;
            partyIcons[5].rectTransform.position = new Vector2(partyPositions[5].x, partyPositions[5].y);
            partyItems[5].rectTransform.position = new Vector2(partyPositions[5].x + 17, partyPositions[5].y + 2);
            selectedIndex = hole;
        }
    }

    private IEnumerator endOfParty(int currentPosition)
    {
        int icon = currentPosition;
        Vector2[] partyPositions = new Vector2[]
        {
            new Vector2(partyIcons[0].rectTransform.position.x, partyIcons[0].rectTransform.position.y),
            new Vector2(partyIcons[1].rectTransform.position.x, partyIcons[1].rectTransform.position.y),
            new Vector2(partyIcons[2].rectTransform.position.x, partyIcons[2].rectTransform.position.y),
            new Vector2(partyIcons[3].rectTransform.position.x, partyIcons[3].rectTransform.position.y),
            new Vector2(partyIcons[4].rectTransform.position.x, partyIcons[4].rectTransform.position.y),
            new Vector2(partyIcons[5].rectTransform.position.x, partyIcons[5].rectTransform.position.y)
        };
        if (icon > 0)
        {
            while (SaveData.currentSave.PC.boxes[0][icon - 1] == null && icon > 1)
            {
                //if the previous spot is free, and is not first spot
                yield return StartCoroutine(moveIcon(partyIcons[icon], partyPositions[icon - 1]));
                partyIcons[icon - 1].sprite = partyIcons[icon].sprite;
                partyIcons[icon].sprite = null;
                partyIcons[icon].rectTransform.position = new Vector2(partyPositions[icon].x, partyPositions[icon].y);
                partyItems[icon - 1].enabled = partyItems[icon].enabled;
                partyItems[icon].enabled = false;
                partyItems[icon].rectTransform.position = new Vector2(partyPositions[icon].x + 17, partyPositions[icon].y + 2);
                SaveData.currentSave.PC.swapPokemon(0, icon - 1, 0, icon);
                icon -= 1;
                updateSelectedInfo(PokemonUnity.Game.GameData.Trainer.party[currentPosition]);
            }
            if (SaveData.currentSave.PC.boxes[0][icon - 1] == null)
            {
                yield return StartCoroutine(moveIcon(partyIcons[icon], partyPositions[icon - 1]));
                partyIcons[icon - 1].sprite = partyIcons[icon].sprite;
                partyIcons[icon].sprite = null;
                partyIcons[icon].rectTransform.position = new Vector2(partyPositions[icon].x, partyPositions[icon].y);
                partyItems[icon - 1].enabled = partyItems[icon].enabled;
                partyItems[icon].enabled = false;
                partyItems[icon].rectTransform.position = new Vector2(partyPositions[icon].x + 17, partyPositions[icon].y + 2);
                SaveData.currentSave.PC.swapPokemon(0, icon - 1, 0, icon);
                updateSelectedInfo(PokemonUnity.Game.GameData.Trainer.party[currentPosition]);
            }
            icon = 0;
        }
    }

    private IEnumerator moveIcon(Image icon, Vector2 destination)
    {
        Image item = icon.transform.Find("Item").GetComponent<Image>();

        float startX = icon.rectTransform.position.x;
        float startY = icon.rectTransform.position.y;
        float distanceX = destination.x - startX;
        float distanceY = destination.y - startY;
        float itemOffsetX = item.rectTransform.position.x - icon.rectTransform.position.x;
        float itemOffsetY = item.rectTransform.position.y - icon.rectTransform.position.y;

        float increment = 0;
        while (increment < 1)
        {
            increment += (1 / moveSpeed) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }
            icon.rectTransform.position = new Vector2(startX + (increment * distanceX), startY + (increment * distanceY));
            item.rectTransform.position = new Vector2(startX + (increment * distanceX) + itemOffsetX,
                startY + (increment * distanceY) + itemOffsetY);
            yield return null;
        }
    }

    private IEnumerator pickUpPokemon(int currentBoxID, int currentPosition)
    {
        selectedPokemon = PokemonUnity.Game.GameData.Trainer.party[currentPosition];//[currentBoxID]
        selectedBoxID = currentBoxID;
        selectedIndex = currentPosition;
        carrying = true;
        //cursor.border = new RectOffset(32, 0, 0, 32);
        SfxHandler.Play(pickUpClip);
        yield return StartCoroutine(moveCursor(new Vector2(cursor.rectTransform.position.x, cursor.rectTransform.position.y - 10)));
        //grabbedPokemon.sprite = selectedPokemon.GetIcons();
        if (selectedPokemon.Item != Items.NONE)
        {
            grabbedPokemonItem.enabled = true;
        }
        else
        {
            grabbedPokemonItem.enabled = false;
        }
        if (currentBoxID == 0)
        {
            partyIcons[currentPosition].sprite = null;
            partyItems[currentPosition].enabled = false;
            StartCoroutine(packParty(currentPosition));
        }
        else
        {
            currentBoxIconsArray[currentPosition].sprite = null;
            currentBoxItemsArray[currentPosition].enabled = false;
        }
        //cursor.border = new RectOffset(0, 32, 0, 32);
        yield return StartCoroutine(moveCursor(new Vector2(cursor.rectTransform.position.x, cursor.rectTransform.position.y + 10)));
    }

    private IEnumerator putDownPokemon(int currentBoxID, int currentPosition)
    {
        bool originalSpot = false;
        if (currentBoxID == selectedBoxID && currentPosition == selectedIndex)
        {
            originalSpot = true;
        }
        if (SaveData.currentSave.PC.boxes[currentBoxID][currentPosition] == null || originalSpot)
        {
            SfxHandler.Play(putDownClip);
            yield return StartCoroutine(moveCursor(new Vector2(cursor.rectTransform.position.x, cursor.rectTransform.position.y - 10)));
            if (currentBoxID == 0)
            {
                partyIcons[currentPosition].sprite = grabbedPokemon.sprite;
                partyItems[currentPosition].enabled = grabbedPokemonItem.enabled;
                StartCoroutine(endOfParty(currentPosition));
            }
            else
            {
                currentBoxIconsArray[currentPosition].sprite = grabbedPokemon.sprite;
                currentBoxItemsArray[currentPosition].enabled = grabbedPokemonItem.enabled;
            }
            if (selectedBoxID == 0)
            {
                SaveData.currentSave.PC.swapPokemon(selectedBoxID, 5, currentBoxID, currentPosition);
            }
            else
            {
                SaveData.currentSave.PC.swapPokemon(selectedBoxID, selectedIndex, currentBoxID, currentPosition);
            }
            grabbedPokemon.sprite = null;
            grabbedPokemonItem.enabled = false;
            //cursor.border = new RectOffset(32, 0, 0, 32);
            yield return StartCoroutine(moveCursor(new Vector2(cursor.rectTransform.position.x, cursor.rectTransform.position.y + 10)));
            carrying = false;
            if (currentBoxID != 0)
            {
                //fully heal if depositing into PC
                SaveData.currentSave.PC.boxes[currentBoxID][currentPosition].healFull();
                updateSelectedInfo(PokemonUnity.Game.GameData.Trainer.party[currentPosition]);//[currentBoxID]
            }
        }
    }

    private IEnumerator switchPokemon(int currentBoxID, int currentPosition)
    {
        if (SaveData.currentSave.PC.boxes[currentBoxID][currentPosition] != null)
        {
            Image targetIcon = null;
            Image targetItem = null;
            if (currentBoxID == 0)
            {
                targetIcon = partyIcons[currentPosition];
                targetItem = partyItems[currentPosition];
            }
            else
            {
                targetIcon = currentBoxIconsArray[currentPosition];
                targetItem = currentBoxItemsArray[currentPosition];
            }
            //cursor.border = new RectOffset(32, 0, 0, 32);
            SfxHandler.Play(putDownClip);
            StartCoroutine(moveIcon(grabbedPokemon,
                new Vector2(grabbedPokemon.rectTransform.position.x + 5, grabbedPokemon.rectTransform.position.y - 5)));
            yield return
                StartCoroutine(moveIcon(targetIcon,
                    new Vector2(targetIcon.rectTransform.position.x - 5, targetIcon.rectTransform.position.y + 5)));

            Sprite temp = targetIcon.sprite;
            bool itemTemp = targetItem.enabled;
            //swap target icon's position and grabbedPokemon's position, and update their new textures
            targetIcon.rectTransform.position = new Vector2(targetIcon.rectTransform.position.x + 10, targetIcon.rectTransform.position.y);
            targetItem.rectTransform.position = new Vector2(targetItem.rectTransform.position.x + 10, targetItem.rectTransform.position.y);
            targetIcon.sprite = grabbedPokemon.sprite;
            targetItem.enabled = grabbedPokemonItem.enabled;
            grabbedPokemon.rectTransform.position = new Vector2(grabbedPokemon.rectTransform.position.x - 10, grabbedPokemon.rectTransform.position.y);
            grabbedPokemonItem.rectTransform.position = new Vector2(grabbedPokemonItem.rectTransform.position.x - 10,
                grabbedPokemonItem.rectTransform.position.y);
            grabbedPokemon.sprite = temp;
            grabbedPokemonItem.enabled = itemTemp;

            //update selected info
            updateSelectedInfoOverride(PokemonUnity.Game.GameData.Trainer.party[currentPosition]);//[currentBoxID]
            //swap pokemon
            SaveData.currentSave.PC.swapPokemon(selectedBoxID, selectedIndex, currentBoxID, currentPosition);

            SfxHandler.Play(pickUpClip);
            StartCoroutine(moveIcon(grabbedPokemon,
                new Vector2(grabbedPokemon.rectTransform.position.x + 5, grabbedPokemon.rectTransform.position.y + 5)));
            yield return
                StartCoroutine(moveIcon(targetIcon,
                    new Vector2(targetIcon.rectTransform.position.x - 5, targetIcon.rectTransform.position.y - 5)));

            //cursor.border = new RectOffset(0, 32, 0, 32);

            if (currentBoxID != 0)
            {
                //fully heal if depositing into PC
                SaveData.currentSave.PC.boxes[currentBoxID][currentPosition].healFull();
            }
        }
    }

    private IEnumerator releasePokemon(int currentBoxID, int currentPosition)
    {
        Image targetIcon = null;
        if (currentBoxID == 0)
        {
            targetIcon = partyIcons[currentPosition];
        }
        else
        {
            targetIcon = currentBoxIconsArray[currentPosition];
        }
        float increment = 0;
        float moveSpeedSlow = 0.4f;
        float startY = targetIcon.rectTransform.position.y;
        while (increment < 1)
        {
            increment += (1 / moveSpeedSlow) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }
            targetIcon.rectTransform.position = new Vector2(targetIcon.rectTransform.position.x, startY + (16 * increment));
            targetIcon.rectTransform.sizeDelta = new Vector2(32, (32f * (1f - increment)));
            yield return null;
        }
        SaveData.currentSave.PC.boxes[currentBoxID][currentPosition] = null;
        targetIcon.sprite = null;
        targetIcon.rectTransform.position = new Vector2(targetIcon.rectTransform.position.x, startY);
    }

    private IEnumerator withdrawPokemon(int currentPosition)
    {
        int targetPosition = 6;
        for (int i = 1; i < 6; i++)
        {
            if (SaveData.currentSave.PC.boxes[0][i] == null)
            {
                targetPosition = i;
                i = 6;
            }
        }
        if (targetPosition < 6)
        {
            yield return StartCoroutine(pickUpPokemon(currentBoxID, currentPosition));
            float startX = cursor.rectTransform.position.x;
            float startY = cursor.rectTransform.position.y;
            yield return
                StartCoroutine(
                    moveCursor(new Vector2(partyIcons[targetPosition].rectTransform.position.x + 267,
                        partyIcons[targetPosition].rectTransform.position.y + 20)));
            yield return StartCoroutine(putDownPokemon(0, targetPosition));
            yield return StartCoroutine(moveCursor(new Vector2(startX, startY)));
        }
    }

    private IEnumerator depositPokemon(int currentPosition, int targetPosition)
    {
        if (targetPosition < 30)
        {
            yield return StartCoroutine(pickUpPokemon(0, currentPosition));
            float startX = cursor.rectTransform.position.x;
            float startY = cursor.rectTransform.position.y;
            yield return
                StartCoroutine(
                    moveCursor(new Vector2(currentBoxIconsArray[targetPosition].rectTransform.position.x + 92,
                        currentBoxIconsArray[targetPosition].rectTransform.position.y + 58)));
            yield return StartCoroutine(putDownPokemon(currentBoxID, targetPosition));
            yield return StartCoroutine(moveCursor(new Vector2(startX, startY)));
        }
    }

    private IEnumerator animatePokemon()
    {
        frame = 0;
        while (true)
        {
            if (selectedSpriteAnimation.Length > 0)
            {
                if (frame < selectedSpriteAnimation.Length - 1)
                {
                    frame += 1;
                }
                else
                {
                    frame = 0;
                }
                selectedSprite.sprite = selectedSpriteAnimation[frame];
            }
            yield return new WaitForSeconds(0.08f);
        }
    }

    //Control the interface
    public IEnumerator control()
    {
        //sceneTransition.FadeIn();
        StartCoroutine(ScreenFade.main.Fade(true, ScreenFade.defaultSpeed));
        /*  0-2  = boxIndex				0 -d-> 3,   1 -d-> 5,   2 -d-> 8
         *  3-32 = boxContents			3 -u-> 0,   4-7 -u-> 1,   8 -u-> 2 
         * 								8/14 -r-> 33,   20 -r-> 35,   26/32 -r-> 37
         * 								27-30 -d-> 39,  31/32 -d-> 40
         * 33-38 = partyContents		33 -l-> 8,   35 -l-> 20,   37 -l-> 26
         * 								37/38 -d-> 41
         * 39,40 = SwitchItems, Cursor	39 -u-> 28, 40 -u-> 32
         *   ,41 = Return				41 -u-> 37
         */
        Vector2[] cursorPositions = new Vector2[]
        {
            new Vector2(94, 167), new Vector2(167, 167), new Vector2(236, 167),
            new Vector2(97, 151), new Vector2(121, 151), new Vector2(145, 151), new Vector2(169, 151),
            new Vector2(193, 151), new Vector2(217, 151),
            new Vector2(97, 128), new Vector2(121, 128), new Vector2(145, 128), new Vector2(169, 128),
            new Vector2(193, 128), new Vector2(217, 128),
            new Vector2(97, 105), new Vector2(121, 105), new Vector2(145, 105), new Vector2(169, 105),
            new Vector2(193, 105), new Vector2(217, 105),
            new Vector2(97, 82), new Vector2(121, 82), new Vector2(145, 82), new Vector2(169, 82), new Vector2(193, 82),
            new Vector2(217, 82),
            new Vector2(97, 59), new Vector2(121, 59), new Vector2(145, 59), new Vector2(169, 59), new Vector2(193, 59),
            new Vector2(217, 59),
            new Vector2(267, 142), new Vector2(305, 134), new Vector2(267, 110), new Vector2(305, 102),
            new Vector2(267, 78), new Vector2(305, 70),
            new Vector2(132, 16), new Vector2(224, 16), new Vector2(292, 16)
        };
        int currentPosition = 3;

        cursor.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        cursorMode = CursorMode.Standard;


        currentBoxID = 1;
        nextBoxID = 2;
        previousBoxID = SaveData.currentSave.PC.boxes.Length - 1;

        updateBoxesAndParty();

        updateSelectedInfo(PokemonUnity.Game.GameData.Trainer.party[currentPosition - 3]);//[currentBoxID]
        cursor.rectTransform.position = new Vector2(cursorPositions[currentPosition].x, cursorPositions[currentPosition].y);
        grabbedPokemon.sprite = null;
        grabbedPokemonItem.enabled = false;

        running = true;
        StartCoroutine("animateCursor");
        StartCoroutine("animatePokemon");
        yield return new WaitForSeconds(0.5f);
        //ToDo: Why is this coroutine so massive?
        /*while (running)
        {
            //if cursor is in boxIndex
            if (currentPosition < 3)
            {
                if (UnityEngine.Input.GetAxisRaw("Horizontal") > 0)
                {
                    //	if(currentPosition < 2){
                    //        currentPosition += 1;
                    //        SfxHandler.Play(selectClip);
                    //        updateSelectedInfo(null);}
                    //    yield return StartCoroutine(moveCursor(cursorPositions[currentPosition])); 
                    SfxHandler.Play(selectClip);
                    yield return StartCoroutine(moveBox(1));
                }
                else if (UnityEngine.Input.GetAxisRaw("Vertical") < 0)
                {
                    if (currentPosition == 0)
                    {
                        currentPosition = 3;
                        SfxHandler.Play(selectClip);
                        updateSelectedInfo(SaveData.currentSave.PC.boxes[currentBoxID][currentPosition - 3]);
                    }
                    else if (currentPosition == 1)
                    {
                        currentPosition = 6;
                        SfxHandler.Play(selectClip);
                        updateSelectedInfo(SaveData.currentSave.PC.boxes[currentBoxID][currentPosition - 3]);
                    }
                    else if (currentPosition == 2)
                    {
                        currentPosition = 8;
                        SfxHandler.Play(selectClip);
                        updateSelectedInfo(SaveData.currentSave.PC.boxes[currentBoxID][currentPosition - 3]);
                    }
                    yield return StartCoroutine(moveCursor(cursorPositions[currentPosition]));
                }
                else if (UnityEngine.Input.GetAxisRaw("Horizontal") < 0)
                {
                    //	if(currentPosition > 0){
                    //        currentPosition -= 1;
                    //        SfxHandler.Play(selectClip);
                    //        updateSelectedInfo(null);}
                    //    yield return StartCoroutine(moveCursor(cursorPositions[currentPosition])); 
                    SfxHandler.Play(selectClip);
                    yield return StartCoroutine(moveBox(-1));
                }
                else if (UnityEngine.Input.GetButton("Select"))
                {
                    string[] choices = new string[] {"Jump", "Wallpaper", "Name", "Cancel"};
                    Dialog.DrawDialogBox();
                    Dialog.DrawTextInstant("What would you like to do?");
                    yield return new WaitForSeconds(0.2f);
                    yield return StartCoroutine(Dialog.DrawChoiceBox(choices));
                    Dialog.UndrawChoiceBox();
                    Dialog.UndrawDialogBox();
                    int chosenIndex = Dialog.chosenIndex;
                    if (chosenIndex == 3)
                    {
                        Debug.Log("Jump not yet implemented");
                        yield return new WaitForSeconds(0.2f);
                    }
                    else if (chosenIndex == 2)
                    {
                        Debug.Log("Wallpaper not yet implemented");
                        yield return new WaitForSeconds(0.2f);
                    }
                    else if (chosenIndex == 1)
                    {
                        //NAME

                        SfxHandler.Play(selectClip);
                        //yield return new WaitForSeconds(sceneTransition.FadeOut(0.4f));
                        yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));

                        Scene.main.Typing.gameObject.SetActive(true);
                        StartCoroutine(Scene.main.Typing.control(8, currentBoxHeader.text, null,
                            new Sprite[] {boxEditIcon}));
                        while (Scene.main.Typing.gameObject.activeSelf)
                        {
                            yield return null;
                        }
                        if (Scene.main.Typing.typedString.Length > 0)
                        {
                            SaveData.currentSave.PC.boxName[currentBoxID] = Scene.main.Typing.typedString;
                        }
                        updateBoxesAndParty();

                        //yield return new WaitForSeconds(sceneTransition.FadeIn(0.4f));
                        yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));
                    }
                    else
                    {
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                else if (UnityEngine.Input.GetButton("Back"))
                {
                    if (carrying)
                    {
                        Dialog.DrawDialogBox();
                        Dialog.DrawTextInstant("You're holding a Pokémon!");
                        yield return new WaitForSeconds(0.2f);
                        while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back"))
                        {
                            yield return null;
                        }
                        Dialog.UndrawDialogBox();
                        yield return new WaitForSeconds(0.2f);
                    }
                    else
                    {
                        currentPosition = 41;
                        SfxHandler.Play(selectClip);
                        updateSelectedInfo(null);
                        yield return StartCoroutine(moveCursor(cursorPositions[currentPosition]));

                        Dialog.DrawDialogBox();
                        Dialog.DrawTextInstant("Continue Box operations?");
                        yield return new WaitForSeconds(0.2f);
                        yield return Dialog.StartCoroutine(Dialog.DrawChoiceBox());
                        Dialog.UndrawChoiceBox();
                        Dialog.UndrawDialogBox();
                        int chosenIndex = Dialog.chosenIndex;
                        if (chosenIndex == 0)
                        {
                            running = false;
                        }
                        else
                        {
                            yield return new WaitForSeconds(0.2f);
                        }
                    }
                }
            }
            //if cursor is in boxContents
            else if (currentPosition > 2 && currentPosition < 33)
            {
                if (UnityEngine.Input.GetAxisRaw("Vertical") > 0)
                {
                    //if along the top row, act differently depending on location
                    if (currentPosition < 9)
                    {
                        //		if(currentPosition == 3){
                        //          currentPosition = 0;
                        //          SfxHandler.Play(selectClip);
                        //          updateSelectedInfo(null);}
                        //      else if(currentPosition == 8){
                        //          currentPosition = 2;
                        //          SfxHandler.Play(selectClip);
                        //          updateSelectedInfo(null);}
                        //      else{
                        //
                        currentPosition = 1;
                        SfxHandler.Play(selectClip);
                        updateSelectedInfo(null);
                    }
                    //	}
                    else
                    {
                        currentPosition -= 6;
                        SfxHandler.Play(selectClip);
                        updateSelectedInfo(SaveData.currentSave.PC.boxes[currentBoxID][currentPosition - 3]);
                    }
                    yield return StartCoroutine(moveCursor(cursorPositions[currentPosition]));
                }
                else if (UnityEngine.Input.GetAxisRaw("Horizontal") > 0)
                {
                    //if along the right column, move to party
                    if (currentPosition == 8 || currentPosition == 14)
                    {
                        currentPosition = 33;
                        SfxHandler.Play(selectClip);
                        updateSelectedInfo(SaveData.currentSave.PC.boxes[0][currentPosition - 33]);
                    }
                    else if (currentPosition == 20)
                    {
                        currentPosition = 35;
                        SfxHandler.Play(selectClip);
                        updateSelectedInfo(SaveData.currentSave.PC.boxes[0][currentPosition - 33]);
                    }
                    else if (currentPosition == 26 || currentPosition == 32)
                    {
                        currentPosition = 37;
                        SfxHandler.Play(selectClip);
                        updateSelectedInfo(SaveData.currentSave.PC.boxes[0][currentPosition - 33]);
                    }
                    //otherwise go one to the right
                    else
                    {
                        currentPosition += 1;
                        SfxHandler.Play(selectClip);
                        updateSelectedInfo(SaveData.currentSave.PC.boxes[currentBoxID][currentPosition - 3]);
                    }
                    yield return StartCoroutine(moveCursor(cursorPositions[currentPosition]));
                }
                else if (UnityEngine.Input.GetAxisRaw("Vertical") < 0)
                {
                    if (currentPosition < 27)
                    {
                        currentPosition += 6;
                        SfxHandler.Play(selectClip);
                        updateSelectedInfo(SaveData.currentSave.PC.boxes[currentBoxID][currentPosition - 3]);
                    }
                    else if (currentPosition < 31)
                    {
                        currentPosition = 39;
                        SfxHandler.Play(selectClip);
                        updateSelectedInfo(null);
                    }
                    else
                    {
                        currentPosition = 40;
                        SfxHandler.Play(selectClip);
                        updateSelectedInfo(null);
                    }
                    yield return StartCoroutine(moveCursor(cursorPositions[currentPosition]));
                }
                else if (UnityEngine.Input.GetAxisRaw("Horizontal") < 0)
                {
                    //if not along the left column, move left one
                    if ((currentPosition + 3) % 6 != 0)
                    {
                        currentPosition -= 1;
                        SfxHandler.Play(selectClip);
                        updateSelectedInfo(SaveData.currentSave.PC.boxes[currentBoxID][currentPosition - 3]);
                    }
                    yield return StartCoroutine(moveCursor(cursorPositions[currentPosition]));
                }
                else if (UnityEngine.Input.GetButton("Select"))
                {
                    if (SaveData.currentSave.PC.boxes[currentBoxID][currentPosition - 3] != null)
                    {
                        updateSelectedInfo(SaveData.currentSave.PC.boxes[currentBoxID][currentPosition - 3]);
                        //STANDARD
                        if (cursorMode == CursorMode.Standard)
                        {
                            if (!carrying)
                            {
                                string[] choices = new string[]
                                    {"Pick Up", "Summary", "Item", "Withdraw", "Release", "Cancel"};
                                Dialog.DrawDialogBox();
                                Dialog.DrawTextInstant("What would you like to do with " +
                                                       SaveData.currentSave.PC.boxes[currentBoxID][currentPosition - 3]
                                                           .getName() + "?");
                                yield return new WaitForSeconds(0.2f);
                                yield return StartCoroutine(Dialog.DrawChoiceBox(choices));
                                Dialog.UndrawChoiceBox();
                                Dialog.UndrawDialogBox();
                                int chosenIndex = Dialog.chosenIndex;
                                if (chosenIndex == 5)
                                {
                                    yield return StartCoroutine(pickUpPokemon(currentBoxID, currentPosition - 3));
                                }
                                else if (chosenIndex == 4)
                                {
                                    //SUMMARY

                                    SfxHandler.Play(selectClip);
                                    //yield return new WaitForSeconds(sceneTransition.FadeOut(0.4f));
                                    yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));

                                    //Set SceneSummary to be active so that it appears
                                    Scene.main.Summary.gameObject.SetActive(true);
                                    //StartCoroutine( //ToDo: uncomment and review
                                    //    Scene.main.Summary.control(SaveData.currentSave.PC.boxes[currentBoxID],
                                    //        currentPosition - 3));
                                    //Start an empty loop that will only stop when ScenePC is no longer active (is closed)
                                    while (Scene.main.Summary.gameObject.activeSelf)
                                    {
                                        yield return null;
                                    }

                                    //yield return new WaitForSeconds(sceneTransition.FadeIn(0.4f));
                                    yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));

                                    chosenIndex = 0;
                                }
                                else if (chosenIndex == 3)
                                {
                                    //ITEM
                                    IPokemon currentPokemon =
                                        SaveData.currentSave.PC.boxes[currentBoxID][currentPosition - 3];

                                    Dialog.UndrawChoiceBox();
                                    Dialog.DrawDialogBox();
                                    if (currentPokemon.Item != Items.NONE)
                                    {
                                        yield return
                                            StartCoroutine(
                                                Dialog.DrawText(currentPokemon.Name + " is holding " +
                                                                currentPokemon.Item.ToString(TextScripts.Name) + "."));
                                        string[] itemChoices = new string[]
                                        {
                                            "Swap", "Take", "Cancel"
                                        };
                                        int itemChosenIndex = -1;
                                        yield return new WaitForSeconds(0.2f);
                                        yield return StartCoroutine(Dialog.DrawChoiceBox(itemChoices));

                                        itemChosenIndex = Dialog.chosenIndex;

                                        if (itemChosenIndex == 2)
                                        {
                                            //Swap
                                            SfxHandler.Play(selectClip);
                                            //yield return new WaitForSeconds(sceneTransition.FadeOut(0.4f));
                                            yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));

                                            Scene.main.Bag.gameObject.SetActive(true);
                                            StartCoroutine(Scene.main.Bag.control(false, true));
                                            while (Scene.main.Bag.gameObject.activeSelf)
                                            {
                                                yield return null;
                                            }

                                            string chosenItem = Scene.main.Bag.chosenItem;

                                            Dialog.UndrawChoiceBox();
                                            Dialog.UndrawDialogBox();

                                            //yield return new WaitForSeconds(sceneTransition.FadeIn(0.4f));
                                            yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));

                                            if (!string.IsNullOrEmpty(chosenItem))
                                            {
                                                Dialog.DrawDialogBox();
                                                yield return
                                                    StartCoroutine(
                                                        Dialog.DrawText("Swap " + currentPokemon.Item.ToString(TextScripts.Name) + " for " +
                                                                        chosenItem + "?"));
                                                yield return StartCoroutine( Dialog.DrawChoiceBox());

                                                itemChosenIndex = Dialog.chosenIndex;
                                                Dialog.UndrawChoiceBox();

                                                if (itemChosenIndex == 1)
                                                {
                                                    string receivedItem = currentPokemon.swapHeldItem(chosenItem);
                                                    SaveData.currentSave.Bag.addItem(receivedItem, 1);
                                                    SaveData.currentSave.Bag.removeItem(chosenItem, 1);

                                                    Dialog.DrawDialogBox();
                                                    yield return
                                                        Dialog.StartCoroutine(Dialog.DrawText(
                                                            "Gave " + chosenItem + " to " + currentPokemon.Name +
                                                            ","));
                                                    while (!UnityEngine.Input.GetButtonDown("Select") &&
                                                           !UnityEngine.Input.GetButtonDown("Back"))
                                                    {
                                                        yield return null;
                                                    }
                                                    Dialog.DrawDialogBox();
                                                    yield return
                                                        Dialog.StartCoroutine(Dialog.DrawText(
                                                            "and received " + receivedItem + " in return."));
                                                    while (!UnityEngine.Input.GetButtonDown("Select") &&
                                                           !UnityEngine.Input.GetButtonDown("Back"))
                                                    {
                                                        yield return null;
                                                    }
                                                }
                                            }
                                        }
                                        else if (itemChosenIndex == 1)
                                        {
                                            //Take
                                            Dialog.UndrawChoiceBox();
                                            string receivedItem = currentPokemon.swapHeldItem("");
                                            SaveData.currentSave.Bag.addItem(receivedItem, 1);

                                            //adjust displayed data
                                            updateSelectedInfo(currentPokemon);
                                            currentBoxItemsArray[currentPosition - 3].enabled = false;

                                            Dialog.DrawDialogBox();
                                            yield return
                                                StartCoroutine(
                                                    Dialog.DrawText("Took " + receivedItem + " from " +
                                                                    currentPokemon.Name + "."));
                                            while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back"))
                                            {
                                                yield return null;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        yield return
                                            StartCoroutine(
                                                Dialog.DrawText(currentPokemon.Name + " isn't holding anything."));
                                        string[] itemChoices = new string[]
                                        {
                                            "Give", "Cancel"
                                        };
                                        int itemChosenIndex = -1;
                                        yield return new WaitForSeconds(0.2f);
                                        yield return StartCoroutine(Dialog.DrawChoiceBox(itemChoices));
                                        itemChosenIndex = Dialog.chosenIndex;

                                        if (itemChosenIndex == 1)
                                        {
                                            //Give
                                            SfxHandler.Play(selectClip);
                                            //yield return new WaitForSeconds(sceneTransition.FadeOut(0.4f));
                                            yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));

                                            Scene.main.Bag.gameObject.SetActive(true);
                                            StartCoroutine(Scene.main.Bag.control(false, true));
                                            while (Scene.main.Bag.gameObject.activeSelf)
                                            {
                                                yield return null;
                                            }

                                            string chosenItem = Scene.main.Bag.chosenItem;

                                            Dialog.UndrawChoiceBox();
                                            Dialog.UndrawDialogBox();

                                            //yield return new WaitForSeconds(sceneTransition.FadeIn(0.4f));
                                            yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));

                                            if (!string.IsNullOrEmpty(chosenItem))
                                            {
                                                currentPokemon.swapHeldItem(chosenItem);
                                                SaveData.currentSave.Bag.removeItem(chosenItem, 1);

                                                //adjust displayed data
                                                updateSelectedInfo(currentPokemon);
                                                currentBoxItemsArray[currentPosition - 3].enabled = true;

                                                Dialog.DrawDialogBox();
                                                yield return
                                                    Dialog.StartCoroutine(Dialog.DrawText(
                                                        "Gave " + chosenItem + " to " + currentPokemon.Name + "."));
                                                while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back"))
                                                {
                                                    yield return null;
                                                }
                                            }
                                        }
                                    }
                                    Dialog.UndrawChoiceBox();
                                    Dialog.UndrawDialogBox();
                                    yield return new WaitForSeconds(0.2f);
                                    chosenIndex = 0;
                                }
                                else if (chosenIndex == 2)
                                {
                                    //WITHDRAW
                                    if (SaveData.currentSave.PC.boxes[0][5] != null)
                                    {
                                        //if party is full
                                        Dialog.DrawDialogBox();
                                        Dialog.DrawTextInstant("Your party is full!");
                                        yield return new WaitForSeconds(0.2f);
                                        while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back"))
                                        {
                                            yield return null;
                                        }
                                        Dialog.UndrawDialogBox();
                                    }
                                    else
                                    {
                                        yield return StartCoroutine(withdrawPokemon(currentPosition - 3));
                                        updateSelectedInfo(
                                            SaveData.currentSave.PC.boxes[currentBoxID][currentPosition - 3]);
                                        chosenIndex = 0;
                                    }
                                }
                                else if (chosenIndex == 1)
                                {
                                    //RELEASE
                                    int releaseIndex = 1;
                                    string pokemonName =
                                        SaveData.currentSave.PC.boxes[currentBoxID][currentPosition - 3].getName();
                                    while (releaseIndex != 0)
                                    {
                                        Dialog.DrawDialogBox();
                                        Dialog.DrawTextInstant("Do you want to release " + pokemonName + "?");
                                        yield return new WaitForSeconds(0.2f);
                                        yield return StartCoroutine(Dialog.DrawChoiceBoxNo());
                                        Dialog.UndrawChoiceBox();
                                        releaseIndex = Dialog.chosenIndex;
                                        if (releaseIndex == 1)
                                        {
                                            yield return
                                                StartCoroutine(releasePokemon(currentBoxID, currentPosition - 3));
                                            Dialog.DrawDialogBox();
                                            Dialog.DrawTextInstant(pokemonName + " was released.");
                                            yield return new WaitForSeconds(0.2f);
                                            while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back"))
                                            {
                                                yield return null;
                                            }
                                            Dialog.DrawDialogBox();
                                            Dialog.DrawTextInstant("Bye bye, " + pokemonName + "!");
                                            yield return new WaitForSeconds(0.2f);
                                            while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back"))
                                            {
                                                yield return null;
                                            }
                                            releaseIndex = 0;
                                            chosenIndex = 0;
                                            updateSelectedInfo(
                                                SaveData.currentSave.PC.boxes[currentBoxID][currentPosition - 3]);
                                        }
                                        Dialog.UndrawDialogBox();
                                    }
                                    yield return new WaitForSeconds(0.2f);
                                }
                                else
                                {
                                    yield return new WaitForSeconds(0.2f);
                                }
                            }
                            else if (currentBoxID == selectedBoxID && currentPosition - 3 == selectedIndex)
                            {
                                yield return StartCoroutine(putDownPokemon(currentBoxID, currentPosition - 3));
                            }
                            else
                            {
                                yield return StartCoroutine(switchPokemon(currentBoxID, currentPosition - 3));
                            }
                        }
                        //HOT MOVE
                        else if (cursorMode == CursorMode.HotMove)
                        {
                            if (!carrying)
                            {
                                yield return StartCoroutine(pickUpPokemon(currentBoxID, currentPosition - 3));
                            }
                            else if (currentBoxID == selectedBoxID && currentPosition - 3 == selectedIndex)
                            {
                                yield return StartCoroutine(putDownPokemon(currentBoxID, currentPosition - 3));
                            }
                            else
                            {
                                yield return StartCoroutine(switchPokemon(currentBoxID, currentPosition - 3));
                            }
                        }
                        //WITHDRAW DEPOSIT
                        else
                        {
                            if (SaveData.currentSave.PC.boxes[0][5] != null)
                            {
                                //if party is full
                                Dialog.DrawDialogBox();
                                Dialog.DrawTextInstant("Your party is full!");
                                yield return new WaitForSeconds(0.2f);
                                while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back") &&
                                       UnityEngine.Input.GetAxisRaw("Horizontal") == 0 && UnityEngine.Input.GetAxisRaw("Vertical") == 0)
                                {
                                    yield return null;
                                }
                                Dialog.UndrawDialogBox();
                                yield return new WaitForSeconds(0.2f);
                            }
                            else
                            {
                                yield return StartCoroutine(withdrawPokemon(currentPosition - 3));
                                updateSelectedInfo(SaveData.currentSave.PC.boxes[currentBoxID][currentPosition - 3]);
                            }
                        }
                    }
                    else if (carrying)
                    {
                        yield return StartCoroutine(putDownPokemon(currentBoxID, currentPosition - 3));
                    }
                }
                else if (UnityEngine.Input.GetButton("Back"))
                {
                    if (carrying)
                    {
                        Dialog.DrawDialogBox();
                        Dialog.DrawTextInstant("You're holding a Pokémon!");
                        yield return new WaitForSeconds(0.2f);
                        while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back"))
                        {
                            yield return null;
                        }
                        Dialog.UndrawDialogBox();
                        yield return new WaitForSeconds(0.2f);
                    }
                    else
                    {
                        currentPosition = 41;
                        SfxHandler.Play(selectClip);
                        updateSelectedInfo(null);
                        yield return StartCoroutine(moveCursor(cursorPositions[currentPosition]));

                        Dialog.DrawDialogBox();
                        Dialog.DrawTextInstant("Continue Box operations?");
                        yield return new WaitForSeconds(0.2f);
                        yield return Dialog.StartCoroutine(Dialog.DrawChoiceBox());
                        Dialog.UndrawChoiceBox();
                        Dialog.UndrawDialogBox();
                        int chosenIndex = Dialog.chosenIndex;
                        if (chosenIndex == 0)
                        {
                            running = false;
                        }
                        else
                        {
                            yield return new WaitForSeconds(0.2f);
                        }
                    }
                }
            }
            //if cursor is in partyContents
            else if (currentPosition > 32 && currentPosition < 39)
            {
                if (UnityEngine.Input.GetAxisRaw("Vertical") > 0)
                {
                    if (currentPosition > 34)
                    {
                        currentPosition -= 2;
                        SfxHandler.Play(selectClip);
                        updateSelectedInfo(SaveData.currentSave.PC.boxes[0][currentPosition - 33]);
                    }
                    yield return StartCoroutine(moveCursor(cursorPositions[currentPosition]));
                }
                else if (UnityEngine.Input.GetAxisRaw("Horizontal") > 0)
                {
                    if (currentPosition % 2 == 1)
                    {
                        currentPosition += 1;
                        SfxHandler.Play(selectClip);
                        updateSelectedInfo(SaveData.currentSave.PC.boxes[0][currentPosition - 33]);
                    }
                    yield return StartCoroutine(moveCursor(cursorPositions[currentPosition]));
                }
                else if (UnityEngine.Input.GetAxisRaw("Vertical") < 0)
                {
                    if (currentPosition < 37)
                    {
                        currentPosition += 2;
                        SfxHandler.Play(selectClip);
                        updateSelectedInfo(SaveData.currentSave.PC.boxes[0][currentPosition - 33]);
                    }
                    else
                    {
                        currentPosition = 41;
                        SfxHandler.Play(selectClip);
                        updateSelectedInfo(null);
                    }
                    yield return StartCoroutine(moveCursor(cursorPositions[currentPosition]));
                }
                else if (UnityEngine.Input.GetAxisRaw("Horizontal") < 0)
                {
                    if (currentPosition == 33)
                    {
                        currentPosition = 8;
                        SfxHandler.Play(selectClip);
                        updateSelectedInfo(SaveData.currentSave.PC.boxes[currentBoxID][currentPosition - 3]);
                    }
                    else if (currentPosition == 35)
                    {
                        currentPosition = 20;
                        SfxHandler.Play(selectClip);
                        updateSelectedInfo(SaveData.currentSave.PC.boxes[currentBoxID][currentPosition - 3]);
                    }
                    else if (currentPosition == 37)
                    {
                        currentPosition = 26;
                        SfxHandler.Play(selectClip);
                        updateSelectedInfo(SaveData.currentSave.PC.boxes[currentBoxID][currentPosition - 3]);
                    }
                    else
                    {
                        currentPosition -= 1;
                        SfxHandler.Play(selectClip);
                        updateSelectedInfo(SaveData.currentSave.PC.boxes[0][currentPosition - 33]);
                    }
                    yield return StartCoroutine(moveCursor(cursorPositions[currentPosition]));
                }
                else if (UnityEngine.Input.GetButton("Select"))
                {
                    if (SaveData.currentSave.PC.boxes[0][currentPosition - 33] != null)
                    {
                        //STANDARD
                        if (cursorMode == CursorMode.Standard)
                        {
                            updateSelectedInfo(SaveData.currentSave.PC.boxes[0][currentPosition - 33]);
                            if (!carrying)
                            {
                                int chosenIndex = 1;
                                while (chosenIndex != 0)
                                {
                                    string[] choices = new string[]
                                        {"Pick Up", "Summary", "Item", "Deposit", "Release", "Cancel"};
                                    Dialog.DrawDialogBox();
                                    Dialog.DrawTextInstant("What would you like to do?");
                                    yield return new WaitForSeconds(0.2f);
                                    yield return StartCoroutine(Dialog.DrawChoiceBox(choices));
                                    Dialog.UndrawChoiceBox();
                                    Dialog.UndrawDialogBox();
                                    chosenIndex = Dialog.chosenIndex;
                                    if (chosenIndex == 5)
                                    {
                                        //PICK UP
                                        if (SaveData.currentSave.PC.boxes[0][1] != null)
                                        {
                                            //if there is more than one pokemon in the party
                                            yield return StartCoroutine(pickUpPokemon(0, currentPosition - 33));
                                            chosenIndex = 0;
                                        }
                                        else
                                        {
                                            Dialog.DrawDialogBox();
                                            Dialog.DrawTextInstant("That's your last Pokémon!");
                                            yield return new WaitForSeconds(0.2f);
                                            while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back"))
                                            {
                                                yield return null;
                                            }
                                        }
                                    }
                                    else if (chosenIndex == 4)
                                    {
                                        //SUMMARY

                                        SfxHandler.Play(selectClip);
                                        //yield return new WaitForSeconds(sceneTransition.FadeOut(0.4f));
                                        yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));

                                        //Set SceneSummary to be active so that it appears
                                        Scene.main.Summary.gameObject.SetActive(true);
                                        StartCoroutine(Scene.main.Summary.control(SaveData.currentSave.PC.boxes[0],
                                            currentPosition - 33));
                                        //Start an empty loop that will only stop when ScenePC is no longer active (is closed)
                                        while (Scene.main.Summary.gameObject.activeSelf)
                                        {
                                            yield return null;
                                        }
                                        //yield return new WaitForSeconds(sceneTransition.FadeIn(0.4f));
                                        yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));

                                        chosenIndex = 0;
                                    }
                                    else if (chosenIndex == 3)
                                    {
                                        //ITEM
                                        Pokemon currentPokemon = SaveData.currentSave.PC.boxes[0][currentPosition - 33];

                                        Dialog.UndrawChoiceBox();
                                        Dialog.DrawDialogBox();
                                        if (!string.IsNullOrEmpty(currentPokemon.HeldItem))
                                        {
                                            yield return
                                                StartCoroutine(
                                                    Dialog.DrawText(currentPokemon.Name + " is holding " +
                                                                    currentPokemon.HeldItem + "."));
                                            string[] itemChoices = new string[]
                                            {
                                                "Swap", "Take", "Cancel"
                                            };
                                            int itemChosenIndex = -1;
                                            yield return new WaitForSeconds(0.2f);
                                            yield return StartCoroutine(Dialog.DrawChoiceBox(itemChoices));

                                            itemChosenIndex = Dialog.chosenIndex;

                                            if (itemChosenIndex == 2)
                                            {
                                                //Swap
                                                SfxHandler.Play(selectClip);
                                                //yield return new WaitForSeconds(sceneTransition.FadeOut(0.4f));
                                                yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));

                                                Scene.main.Bag.gameObject.SetActive(true);
                                                StartCoroutine(Scene.main.Bag.control(false, true));
                                                while (Scene.main.Bag.gameObject.activeSelf)
                                                {
                                                    yield return null;
                                                }

                                                string chosenItem = Scene.main.Bag.chosenItem;

                                                Dialog.UndrawChoiceBox();
                                                Dialog.UndrawDialogBox();

                                                //yield return new WaitForSeconds(sceneTransition.FadeIn(0.4f));
                                                yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));

                                                if (!string.IsNullOrEmpty(chosenItem))
                                                {
                                                    Dialog.DrawDialogBox();
                                                    yield return
                                                        StartCoroutine(
                                                            Dialog.DrawText("Swap " + currentPokemon.HeldItem +
                                                                            " for " + chosenItem + "?"));
                                                    yield return StartCoroutine(Dialog.DrawChoiceBox());

                                                    itemChosenIndex = Dialog.chosenIndex;
                                                    Dialog.UndrawChoiceBox();

                                                    if (itemChosenIndex == 1)
                                                    {
                                                        string receivedItem = currentPokemon.swapHeldItem(chosenItem);
                                                        SaveData.currentSave.Bag.addItem(receivedItem, 1);
                                                        SaveData.currentSave.Bag.removeItem(chosenItem, 1);

                                                        Dialog.DrawDialogBox();
                                                        yield return
                                                            Dialog.StartCoroutine(Dialog.DrawText(
                                                                "Gave " + chosenItem + " to " + currentPokemon.Name +
                                                                ","));
                                                        while (!UnityEngine.Input.GetButtonDown("Select") &&
                                                               !UnityEngine.Input.GetButtonDown("Back"))
                                                        {
                                                            yield return null;
                                                        }
                                                        Dialog.DrawDialogBox();
                                                        yield return
                                                            Dialog.StartCoroutine(Dialog.DrawText(
                                                                "and received " + receivedItem + " in return."));
                                                        while (!UnityEngine.Input.GetButtonDown("Select") &&
                                                               !UnityEngine.Input.GetButtonDown("Back"))
                                                        {
                                                            yield return null;
                                                        }
                                                    }
                                                }
                                            }
                                            else if (itemChosenIndex == 1)
                                            {
                                                //Take
                                                Dialog.UndrawChoiceBox();
                                                string receivedItem = currentPokemon.swapHeldItem("");
                                                SaveData.currentSave.Bag.addItem(receivedItem, 1);

                                                //adjust displayed data
                                                updateSelectedInfo(currentPokemon);
                                                partyItems[currentPosition - 33].enabled = false;

                                                Dialog.DrawDialogBox();
                                                yield return
                                                    StartCoroutine(
                                                        Dialog.DrawText("Took " + receivedItem + " from " +
                                                                        currentPokemon.Name + "."));
                                                while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back"))
                                                {
                                                    yield return null;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            yield return
                                                StartCoroutine(
                                                    Dialog.DrawText(currentPokemon.Name +
                                                                    " isn't holding anything."));
                                            string[] itemChoices = new string[]
                                            {
                                                "Give", "Cancel"
                                            };
                                            int itemChosenIndex = -1;
                                            yield return new WaitForSeconds(0.2f);
                                            yield return StartCoroutine(Dialog.DrawChoiceBox(itemChoices));
                                            itemChosenIndex = Dialog.chosenIndex;

                                            if (itemChosenIndex == 1)
                                            {
                                                //Give
                                                SfxHandler.Play(selectClip);
                                                //yield return new WaitForSeconds(sceneTransition.FadeOut(0.4f));
                                                yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));

                                                Scene.main.Bag.gameObject.SetActive(true);
                                                StartCoroutine(Scene.main.Bag.control(false, true));
                                                while (Scene.main.Bag.gameObject.activeSelf)
                                                {
                                                    yield return null;
                                                }

                                                string chosenItem = Scene.main.Bag.chosenItem;

                                                Dialog.UndrawChoiceBox();
                                                Dialog.UndrawDialogBox();

                                                //yield return new WaitForSeconds(sceneTransition.FadeIn(0.4f));
                                                yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));

                                                if (!string.IsNullOrEmpty(chosenItem))
                                                {
                                                    currentPokemon.swapHeldItem(chosenItem);
                                                    SaveData.currentSave.Bag.removeItem(chosenItem, 1);

                                                    //adjust displayed data
                                                    updateSelectedInfo(currentPokemon);
                                                    partyItems[currentPosition - 33].enabled = true;

                                                    Dialog.DrawDialogBox();
                                                    yield return
                                                        Dialog.StartCoroutine(Dialog.DrawText(
                                                            "Gave " + chosenItem + " to " + currentPokemon.Name +
                                                            "."));
                                                    while (!UnityEngine.Input.GetButtonDown("Select") &&
                                                           !UnityEngine.Input.GetButtonDown("Back"))
                                                    {
                                                        yield return null;
                                                    }
                                                }
                                            }
                                        }
                                        Dialog.UndrawChoiceBox();
                                        Dialog.UndrawDialogBox();
                                        yield return new WaitForSeconds(0.2f);
                                        chosenIndex = 0;
                                    }
                                    else if (chosenIndex == 2)
                                    {
                                        //DEPOSIT
                                        if (SaveData.currentSave.PC.boxes[0][1] != null)
                                        {
                                            //if there is more than one pokemon in the party
                                            int targetPosition = 30;
                                            for (int i = 0; i < 30; i++)
                                            {
                                                if (SaveData.currentSave.PC.boxes[currentBoxID][i] == null)
                                                {
                                                    targetPosition = i;
                                                    i = 30;
                                                }
                                            }
                                            if (targetPosition >= 30)
                                            {
                                                //if box is full
                                                Dialog.DrawDialogBox();
                                                Dialog.DrawTextInstant("The box is full!");
                                                yield return new WaitForSeconds(0.2f);
                                                while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back"))
                                                {
                                                    yield return null;
                                                }
                                                Dialog.UndrawDialogBox();
                                            }
                                            else
                                            {
                                                yield return
                                                    StartCoroutine(depositPokemon(currentPosition - 33, targetPosition))
                                                    ;
                                                updateSelectedInfo(
                                                    SaveData.currentSave.PC.boxes[0][currentPosition - 33]);
                                                chosenIndex = 0;
                                            }
                                        }
                                        else
                                        {
                                            Dialog.DrawDialogBox();
                                            Dialog.DrawTextInstant("That's your last Pokémon!");
                                            yield return new WaitForSeconds(0.2f);
                                            while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back"))
                                            {
                                                yield return null;
                                            }
                                        }
                                    }
                                    else if (chosenIndex == 1)
                                    {
                                        //RELEASE
                                        if (SaveData.currentSave.PC.boxes[0][1] != null)
                                        {
                                            //if there is more than one pokemon in the party
                                            int releaseIndex = 1;
                                            string pokemonName =
                                                SaveData.currentSave.PC.boxes[0][currentPosition - 33].getName();
                                            while (releaseIndex != 0)
                                            {
                                                Dialog.DrawDialogBox();
                                                Dialog.DrawTextInstant("Do you want to release " + pokemonName + "?");
                                                yield return new WaitForSeconds(0.2f);
                                                yield return StartCoroutine(Dialog.DrawChoiceBoxNo());
                                                Dialog.UndrawChoiceBox();
                                                releaseIndex = Dialog.chosenIndex;
                                                if (releaseIndex == 1)
                                                {
                                                    yield return StartCoroutine(releasePokemon(0, currentPosition - 33))
                                                        ;
                                                    Dialog.DrawDialogBox();
                                                    Dialog.DrawTextInstant(pokemonName + " was released.");
                                                    yield return new WaitForSeconds(0.2f);
                                                    while (!UnityEngine.Input.GetButtonDown("Select") &&
                                                           !UnityEngine.Input.GetButtonDown("Back"))
                                                    {
                                                        yield return null;
                                                    }
                                                    Dialog.DrawDialogBox();
                                                    Dialog.DrawTextInstant("Bye bye, " + pokemonName + "!");
                                                    yield return new WaitForSeconds(0.2f);
                                                    while (!UnityEngine.Input.GetButtonDown("Select") &&
                                                           !UnityEngine.Input.GetButtonDown("Back"))
                                                    {
                                                        yield return null;
                                                    }
                                                    releaseIndex = 0;
                                                    chosenIndex = 0;
                                                    updateSelectedInfo(
                                                        SaveData.currentSave.PC.boxes[0][currentPosition - 33]);
                                                    Dialog.UndrawDialogBox();
                                                    StartCoroutine(packParty(currentPosition - 33));
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Dialog.DrawDialogBox();
                                            Dialog.DrawTextInstant("That's your last Pokémon!");
                                            yield return new WaitForSeconds(0.2f);
                                            while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back"))
                                            {
                                                yield return null;
                                            }
                                        }
                                    }
                                    yield return new WaitForSeconds(0.2f);
                                }
                            }
                            else if (selectedBoxID == 0 && currentPosition - 33 == selectedIndex)
                            {
                                yield return StartCoroutine(putDownPokemon(0, currentPosition - 33));
                            }
                            else
                            {
                                yield return StartCoroutine(switchPokemon(0, currentPosition - 33));
                            }
                        }
                        //HOT MOVE
                        else if (cursorMode == CursorMode.HotMove)
                        {
                            if (!carrying)
                            {
                                if (SaveData.currentSave.PC.boxes[0][1] != null)
                                {
                                    //if there is more than one pokemon in the party
                                    yield return StartCoroutine(pickUpPokemon(0, currentPosition - 33));
                                }
                                else
                                {
                                    Dialog.DrawDialogBox();
                                    Dialog.DrawTextInstant("That's your last Pokémon!");
                                    yield return new WaitForSeconds(0.2f);
                                    while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back") &&
                                           UnityEngine.Input.GetAxisRaw("Horizontal") == 0 && UnityEngine.Input.GetAxisRaw("Vertical") == 0)
                                    {
                                        yield return null;
                                    }
                                    Dialog.UndrawDialogBox();
                                    yield return new WaitForSeconds(0.2f);
                                }
                            }
                            else if (selectedBoxID == 0 && currentPosition - 33 == selectedIndex)
                            {
                                yield return StartCoroutine(putDownPokemon(0, currentPosition - 33));
                            }
                            else
                            {
                                yield return StartCoroutine(switchPokemon(0, currentPosition - 33));
                            }
                        }
                        //WITHDRAW DEPOSIT
                        else
                        {
                            if (SaveData.currentSave.PC.boxes[0][1] != null)
                            {
                                //if there is more than one pokemon in the party
                                int targetPosition = 30;
                                for (int i = 0; i < 30; i++)
                                {
                                    if (SaveData.currentSave.PC.boxes[currentBoxID][i] == null)
                                    {
                                        targetPosition = i;
                                        i = 30;
                                    }
                                }
                                if (targetPosition >= 30)
                                {
                                    //if box is full
                                    Dialog.DrawDialogBox();
                                    Dialog.DrawTextInstant("The box is full!");
                                    yield return new WaitForSeconds(0.2f);
                                    while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back") &&
                                           UnityEngine.Input.GetAxisRaw("Horizontal") == 0 && UnityEngine.Input.GetAxisRaw("Vertical") == 0)
                                    {
                                        yield return null;
                                    }
                                    Dialog.UndrawDialogBox();
                                    yield return new WaitForSeconds(0.2f);
                                }
                                else
                                {
                                    yield return StartCoroutine(depositPokemon(currentPosition - 33, targetPosition));
                                    updateSelectedInfo(SaveData.currentSave.PC.boxes[0][currentPosition - 33]);
                                }
                            }
                            else
                            {
                                Dialog.DrawDialogBox();
                                Dialog.DrawTextInstant("That's your last Pokémon!");
                                yield return new WaitForSeconds(0.2f);
                                while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back") &&
                                       UnityEngine.Input.GetAxisRaw("Horizontal") == 0 && UnityEngine.Input.GetAxisRaw("Vertical") == 0)
                                {
                                    yield return null;
                                }
                                Dialog.UndrawDialogBox();
                                yield return new WaitForSeconds(0.2f);
                            }
                        }
                    }
                    else if (carrying)
                    {
                        yield return StartCoroutine(putDownPokemon(0, currentPosition - 33));
                    }
                }
                else if (UnityEngine.Input.GetButton("Back"))
                {
                    if (carrying)
                    {
                        Dialog.DrawDialogBox();
                        Dialog.DrawTextInstant("You're holding a Pokémon!");
                        yield return new WaitForSeconds(0.2f);
                        while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back"))
                        {
                            yield return null;
                        }
                        Dialog.UndrawDialogBox();
                        yield return new WaitForSeconds(0.2f);
                    }
                    else
                    {
                        currentPosition = 41;
                        SfxHandler.Play(selectClip);
                        updateSelectedInfo(null);
                        yield return StartCoroutine(moveCursor(cursorPositions[currentPosition]));

                        Dialog.DrawDialogBox();
                        Dialog.DrawTextInstant("Continue Box operations?");
                        yield return new WaitForSeconds(0.2f);
                        yield return Dialog.StartCoroutine(Dialog.DrawChoiceBox());
                        Dialog.UndrawChoiceBox();
                        Dialog.UndrawDialogBox();
                        int chosenIndex = Dialog.chosenIndex;
                        if (chosenIndex == 0)
                        {
                            running = false;
                        }
                        else
                        {
                            yield return new WaitForSeconds(0.2f);
                        }
                    }
                }
            }
            //if cursor is on the bottom buttons
            else
            {
                if (UnityEngine.Input.GetAxisRaw("Vertical") > 0)
                {
                    if (currentPosition == 39)
                    {
                        currentPosition = 28;
                        SfxHandler.Play(selectClip);
                        updateSelectedInfo(SaveData.currentSave.PC.boxes[currentBoxID][currentPosition - 3]);
                    }
                    else if (currentPosition == 40)
                    {
                        currentPosition = 32;
                        SfxHandler.Play(selectClip);
                        updateSelectedInfo(SaveData.currentSave.PC.boxes[currentBoxID][currentPosition - 3]);
                    }
                    else
                    {
                        currentPosition = 38;
                        SfxHandler.Play(selectClip);
                        updateSelectedInfo(SaveData.currentSave.PC.boxes[0][currentPosition - 33]);
                    }
                    yield return StartCoroutine(moveCursor(cursorPositions[currentPosition]));
                }
                else if (UnityEngine.Input.GetAxisRaw("Horizontal") > 0)
                {
                    if (currentPosition < 41)
                    {
                        currentPosition += 1;
                        SfxHandler.Play(selectClip);
                        updateSelectedInfo(null);
                    }
                    yield return StartCoroutine(moveCursor(cursorPositions[currentPosition]));
                }
                else if (UnityEngine.Input.GetAxisRaw("Horizontal") < 0)
                {
                    if (currentPosition > 39)
                    {
                        currentPosition -= 1;
                        SfxHandler.Play(selectClip);
                        updateSelectedInfo(null);
                    }
                    yield return StartCoroutine(moveCursor(cursorPositions[currentPosition]));
                }
                else if (UnityEngine.Input.GetButton("Select"))
                {
                    if (currentPosition == 40)
                    {
                        SfxHandler.Play(selectClip);
                        if (cursorMode == CursorMode.Standard)
                        {
                            cursor.color = new Color(0.85f, 0.45f, 0.25f, 0.5f);
                            cursorMode = CursorMode.HotMove;
                        }
                        else if (cursorMode == CursorMode.HotMove)
                        {
                            cursor.color = new Color(0.75f, 0.25f, 0.9f, 0.5f);
                            cursorMode = CursorMode.WithdrawDeposit;
                        }
                        else
                        {
                            cursor.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                            cursorMode = CursorMode.Standard;
                        }
                        yield return new WaitForSeconds(0.2f);
                    }
                    else if (currentPosition == 41)
                    {
                        if (carrying)
                        {
                            Dialog.DrawDialogBox();
                            Dialog.DrawTextInstant("You're holding a Pokémon!");
                            yield return new WaitForSeconds(0.2f);
                            while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back"))
                            {
                                yield return null;
                            }
                            Dialog.UndrawDialogBox();
                            yield return new WaitForSeconds(0.2f);
                        }
                        else
                        {
                            Dialog.DrawDialogBox();
                            Dialog.DrawTextInstant("Continue Box operations?");
                            yield return new WaitForSeconds(0.2f);
                            yield return Dialog.StartCoroutine(Dialog.DrawChoiceBox());
                            Dialog.UndrawChoiceBox();
                            Dialog.UndrawDialogBox();
                            int chosenIndex = Dialog.chosenIndex;
                            if (chosenIndex == 0)
                            {
                                running = false;
                            }
                            else
                            {
                                yield return new WaitForSeconds(0.2f);
                            }
                        }
                    }
                }
                else if (UnityEngine.Input.GetButton("Back"))
                {
                    if (carrying)
                    {
                        Dialog.DrawDialogBox();
                        Dialog.DrawTextInstant("You're holding a Pokémon!");
                        yield return new WaitForSeconds(0.2f);
                        while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back"))
                        {
                            yield return null;
                        }
                        Dialog.UndrawDialogBox();
                        yield return new WaitForSeconds(0.2f);
                    }
                    else
                    {
                        currentPosition = 41;
                        SfxHandler.Play(selectClip);
                        updateSelectedInfo(null);
                        yield return StartCoroutine(moveCursor(cursorPositions[currentPosition]));

                        Dialog.DrawDialogBox();
                        Dialog.DrawTextInstant("Continue Box operations?");
                        yield return new WaitForSeconds(0.2f);
                        yield return Dialog.StartCoroutine(Dialog.DrawChoiceBox());
                        Dialog.UndrawChoiceBox();
                        Dialog.UndrawDialogBox();
                        int chosenIndex = Dialog.chosenIndex;
                        if (chosenIndex == 0)
                        {
                            running = false;
                        }
                        else
                        {
                            yield return new WaitForSeconds(0.2f);
                        }
                    }
                }
            }
            yield return null;
        }*/
        SfxHandler.Play(offClip);
        //yield return new WaitForSeconds(sceneTransition.FadeOut());
        yield return StartCoroutine(ScreenFade.main.Fade(false, ScreenFade.defaultSpeed));
        GlobalVariables.global.resetFollower();
        yield return new WaitForSeconds(0.4f);
        this.gameObject.SetActive(false);
    }
}