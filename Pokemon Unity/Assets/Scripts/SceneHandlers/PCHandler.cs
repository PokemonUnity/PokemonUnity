//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PokemonUnity.Monster;

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

    private DialogBoxHandler Dialog;

    private GameObject dialogBox;
    private GameObject choiceBox;

    private Text dialogText;
    private Text dialogTextShadow;
    private Text choiceText;
    private Text choiceTextShadow;

    //private PokemonOld selectedPokemon;

    private Transform selectedInfo;

    private Text selectedName;
    private Text selectedNameShadow;
    private Text selectedGender;
    private Text selectedGenderShadow;
    private int frame = 0;
    private Texture[] selectedSpriteAnimation;
    private Texture selectedSprite;
    private Texture selectedType1;
    private Texture selectedType2;
    private Text selectedLevel;
    private Text selectedLevelShadow;
    private Text selectedAbility;
    private Text selectedAbilityShadow;
    private Text selectedItem;
    private Text selectedItemShadow;
    private Texture selectedStatus;

    private Texture cursor;
    private Texture grabbedPokemon;
    private Texture grabbedPokemonItem;

    private Texture[] partyIcons = new Texture[6];
    private Texture[] partyItems = new Texture[6];
			   
    private GameObject currentBox;
    private GameObject nextBox;
    private GameObject previousBox;

    private Texture currentBoxTexture;
    private Texture nextBoxTexture;
    private Texture previousBoxTexture;
    private Text currentBoxHeader;
    private Text nextBoxHeader;
    private Text previousBoxHeader;
    private Text currentBoxHeaderShadow;
    private Text nextBoxHeaderShadow;
    private Text previousBoxHeaderShadow;

    private Transform currentBoxIcons;
    private Texture[] currentBoxIconsArray	= new Texture[30];
    private Texture[] currentBoxItemsArray	= new Texture[30];
    private Transform nextBoxIcons;
    private Texture[] nextBoxIconsArray		= new Texture[30];
    private Texture[] nextBoxItemsArray		= new Texture[30];
    private Transform previousBoxIcons;
    private Texture[] previousBoxIconsArray = new Texture[30];
    private Texture[] previousBoxItemsArray = new Texture[30];

    private AudioSource PCaudio;

    public AudioClip offClip;
    public AudioClip openClip;
    public AudioClip selectClip;
    public AudioClip pickUpClip;
    public AudioClip putDownClip;

    private bool running = false;
    private bool carrying = false;
    //private bool switching = false;

    //private SceneTransition sceneTransition;

    private float moveSpeed = 0.16f;

    void Awake()
    {
        Dialog = this.gameObject.GetComponent<DialogBoxHandler>();

        dialogBox = this.transform.Find("DialogBox").gameObject;
        choiceBox = this.transform.Find("ChoiceBox").gameObject;

        PCaudio = this.gameObject.GetComponent<AudioSource>();

        //sceneTransition = this.gameObject.GetComponent<SceneTransition>();

        //dialogText = dialogBox.transform.Find("BoxText").GetComponent<GUIText>();
        //dialogTextShadow = choiceBox.transform.Find("BoxTextShadow").GetComponent<GUIText>();
        //choiceText = choiceBox.transform.Find("BoxText").GetComponent<GUIText>();
        //choiceTextShadow = choiceBox.transform.Find("BoxTextShadow").GetComponent<GUIText>();

        //selectedPokemon = null;

        selectedInfo = this.transform.Find("SelectedInfo");

        //selectedName = selectedInfo.Find("SelectedName").GetComponent<GUIText>();
        //selectedNameShadow = selectedName.transform.Find("SelectedNameShadow").GetComponent<GUIText>();
        //selectedGender = selectedInfo.Find("SelectedGender").GetComponent<GUIText>();
        //selectedGenderShadow = selectedGender.transform.Find("SelectedGenderShadow").GetComponent<GUIText>();
        //selectedSprite = selectedInfo.Find("SelectedSprite").GetComponent<GUITexture>();
        //selectedType1 = selectedInfo.Find("SelectedType1").GetComponent<GUITexture>();
        //selectedType2 = selectedInfo.Find("SelectedType2").GetComponent<GUITexture>();
        //selectedLevel = selectedInfo.Find("SelectedLevel").GetComponent<GUIText>();
        //selectedLevelShadow = selectedLevel.transform.Find("SelectedLevelShadow").GetComponent<GUIText>();
        //selectedAbility = selectedInfo.Find("SelectedAbility").GetComponent<GUIText>();
        //selectedAbilityShadow = selectedAbility.transform.Find("SelectedAbilityShadow").GetComponent<GUIText>();
        //selectedItem = selectedInfo.Find("SelectedItem").GetComponent<GUIText>();
        //selectedItemShadow = selectedItem.transform.Find("SelectedItemShadow").GetComponent<GUIText>();
        //selectedStatus = selectedInfo.Find("SelectedStatus").GetComponent<GUITexture>();
		//
        //cursor = this.transform.Find("Cursor").GetComponent<GUITexture>();
        //grabbedPokemon = cursor.transform.Find("GrabbedPokemon").GetComponent<GUITexture>();
        //grabbedPokemonItem = grabbedPokemon.transform.Find("Item").GetComponent<GUITexture>();
		//
        //for (int i = 0; i < 6; i++)
        //{
        //    partyIcons[i] = transform.Find("Party").Find("Pokemon" + i).GetComponent<GUITexture>();
        //    partyItems[i] = partyIcons[i].transform.Find("Item").GetComponent<GUITexture>();
        //}

        currentBox = this.transform.Find("CurrentBox").gameObject;
        nextBox = currentBox.transform.Find("NextBox").gameObject;
        previousBox = currentBox.transform.Find("PreviousBox").gameObject;

        //currentBoxTexture = currentBox.GetComponent<GUITexture>();
        //nextBoxTexture = currentBox.transform.Find("NextBox").GetComponent<GUITexture>();
        //previousBoxTexture = currentBox.transform.Find("PreviousBox").GetComponent<GUITexture>();
        //currentBoxHeader = currentBox.transform.Find("BoxHeader").GetComponent<GUIText>();
        //nextBoxHeader = currentBox.transform.Find("NextBox").Find("BoxHeader").GetComponent<GUIText>();
        //previousBoxHeader = currentBox.transform.Find("PreviousBox").Find("BoxHeader").GetComponent<GUIText>();
        //currentBoxHeaderShadow = currentBoxHeader.transform.Find("BoxHeaderShadow").GetComponent<GUIText>();
        //nextBoxHeaderShadow = nextBoxHeader.transform.Find("BoxHeaderShadow").GetComponent<GUIText>();
        //previousBoxHeaderShadow = previousBoxHeader.transform.Find("BoxHeaderShadow").GetComponent<GUIText>();

        currentBoxIcons = currentBox.transform.Find("BoxIcons").transform;
        //for (int i = 0; i < 30; i++)
        //{
        //    currentBoxIconsArray[i] = currentBoxIcons.Find("Pokemon" + i).GetComponent<GUITexture>();
        //    currentBoxItemsArray[i] = currentBoxIconsArray[i].transform.Find("Item").GetComponent<GUITexture>();
        //}
		//
        //nextBoxIcons = nextBox.transform.Find("BoxIcons").transform;
        //for (int i = 0; i < 30; i++)
        //{
        //    nextBoxIconsArray[i] = nextBoxIcons.Find("Pokemon" + i).GetComponent<GUITexture>();
        //    nextBoxItemsArray[i] = nextBoxIconsArray[i].transform.Find("Item").GetComponent<GUITexture>();
        //}
		//
        //previousBoxIcons = previousBox.transform.Find("BoxIcons").transform;
        //for (int i = 0; i < 30; i++)
        //{
        //    previousBoxIconsArray[i] = previousBoxIcons.Find("Pokemon" + i).GetComponent<GUITexture>();
        //    previousBoxItemsArray[i] = previousBoxIconsArray[i].transform.Find("Item").GetComponent<GUITexture>();
        //}
    }

    void Start()
    {
        this.gameObject.SetActive(false);
    }

    private void updateBoxesAndParty()
    {
        //update box icons
        //for (int i = 0; i < 30; i++)
        //{
        //    if (SaveDataOld.currentSave.PC.boxes[currentBoxID][i] == null)
        //    {
        //        currentBoxIconsArray[i].texture = null;
        //        currentBoxItemsArray[i].enabled = false;
        //    }
        //    else
        //    {
        //        currentBoxIconsArray[i].texture = SaveDataOld.currentSave.PC.boxes[currentBoxID][i].GetIcons();
        //        if (!string.IsNullOrEmpty(SaveDataOld.currentSave.PC.boxes[currentBoxID][i].getHeldItem()))
        //        {
        //            currentBoxItemsArray[i].enabled = true;
        //        }
        //        else
        //        {
        //            currentBoxItemsArray[i].enabled = false;
        //        }
        //    }
        //}
        //for (int i = 0; i < 30; i++)
        //{
        //    if (SaveDataOld.currentSave.PC.boxes[nextBoxID][i] == null)
        //    {
        //        nextBoxIconsArray[i].texture = null;
        //        nextBoxItemsArray[i].enabled = false;
        //    }
        //    else
        //    {
        //        nextBoxIconsArray[i].texture = SaveDataOld.currentSave.PC.boxes[nextBoxID][i].GetIcons();
        //        if (!string.IsNullOrEmpty(SaveDataOld.currentSave.PC.boxes[nextBoxID][i].getHeldItem()))
        //        {
        //            nextBoxItemsArray[i].enabled = true;
        //        }
        //        else
        //        {
        //            nextBoxItemsArray[i].enabled = false;
        //        }
        //    }
        //}
        //for (int i = 0; i < 30; i++)
        //{
        //    if (SaveDataOld.currentSave.PC.boxes[previousBoxID][i] == null)
        //    {
        //        previousBoxIconsArray[i].texture = null;
        //        previousBoxItemsArray[i].enabled = false;
        //    }
        //    else
        //    {
        //        previousBoxIconsArray[i].texture = SaveDataOld.currentSave.PC.boxes[previousBoxID][i].GetIcons();
        //        if (!string.IsNullOrEmpty(SaveDataOld.currentSave.PC.boxes[previousBoxID][i].getHeldItem()))
        //        {
        //            previousBoxItemsArray[i].enabled = true;
        //        }
        //        else
        //        {
        //            previousBoxItemsArray[i].enabled = false;
        //        }
        //    }
        //}
		//
        ////update party textures
        //for (int i = 0; i < 6; i++)
        //{
        //    if (SaveDataOld.currentSave.PC.boxes[0][i] == null)
        //    {
        //        partyIcons[i].texture = null;
        //        partyItems[i].enabled = false;
        //    }
        //    else
        //    {
        //        partyIcons[i].texture = SaveDataOld.currentSave.PC.boxes[0][i].GetIcons();
        //        if (!string.IsNullOrEmpty(SaveDataOld.currentSave.PC.boxes[0][i].getHeldItem()))
        //        {
        //            partyItems[i].enabled = true;
        //        }
        //        else
        //        {
        //            partyItems[i].enabled = false;
        //        }
        //    }
        //}
		//
        ////update box names
        //if (string.IsNullOrEmpty(SaveDataOld.currentSave.PC.boxName[currentBoxID]))
        //{
        //    currentBoxHeader.text = "Box " + currentBoxID;
        //}
        //else
        //{
        //    currentBoxHeader.text = SaveDataOld.currentSave.PC.boxName[currentBoxID];
        //}
        //if (string.IsNullOrEmpty(SaveDataOld.currentSave.PC.boxName[nextBoxID]))
        //{
        //    nextBoxHeader.text = "Box " + nextBoxID;
        //}
        //else
        //{
        //    nextBoxHeader.text = SaveDataOld.currentSave.PC.boxName[nextBoxID];
        //}
        //if (string.IsNullOrEmpty(SaveDataOld.currentSave.PC.boxName[previousBoxID]))
        //{
        //    previousBoxHeader.text = "Box " + previousBoxID;
        //}
        //else
        //{
        //    previousBoxHeader.text = SaveDataOld.currentSave.PC.boxName[previousBoxID];
        //}
        currentBoxHeaderShadow.text = currentBoxHeader.text;
        nextBoxHeaderShadow.text = nextBoxHeader.text;
        previousBoxHeaderShadow.text = previousBoxHeader.text;

        //update box textures
        //if (SaveDataOld.currentSave.PC.boxTexture[currentBoxID] == 0)
        //{
        //    currentBoxTexture.texture = Resources.Load<Texture>("PCSprites/box" + currentBoxID);
        //}
        //else
        //{
        //    currentBoxTexture.texture =
        //        Resources.Load<Texture>("PCSprites/box" + SaveDataOld.currentSave.PC.boxTexture[currentBoxID]);
        //}
        //if (SaveDataOld.currentSave.PC.boxTexture[nextBoxID] == 0)
        //{
        //    nextBoxTexture.texture = Resources.Load<Texture>("PCSprites/box" + nextBoxID);
        //}
        //else
        //{
        //    nextBoxTexture.texture =
        //        Resources.Load<Texture>("PCSprites/box" + SaveDataOld.currentSave.PC.boxTexture[nextBoxID]);
        //}
        //if (SaveDataOld.currentSave.PC.boxTexture[previousBoxID] == 0)
        //{
        //    previousBoxTexture.texture = Resources.Load<Texture>("PCSprites/box" + previousBoxID);
        //}
        //else
        //{
        //    previousBoxTexture.texture =
        //        Resources.Load<Texture>("PCSprites/box" + SaveDataOld.currentSave.PC.boxTexture[previousBoxID]);
        //}

        //set Selected Info to null because nothing is selected by default
        selectedName.text = null;
        selectedNameShadow.text = null;
        selectedGender.text = null;
        selectedGenderShadow.text = null;
        selectedSpriteAnimation = new Texture[] {};
        //selectedSprite.texture = null;
        //selectedType1.texture = null;
        //selectedType2.texture = null;
        selectedLevel.text = null;
        selectedLevelShadow.text = null;
        selectedAbility.text = null;
        selectedAbilityShadow.text = null;
        selectedItem.text = null;
        selectedItemShadow.text = null;
        //selectedStatus.texture = null;
    }


    private void updateSelectedInfo(Pokemon selectedPokemon)
    {
        if (!carrying)
        {
            if (selectedPokemon == null)
            {
                selectedName.text = null;
                selectedNameShadow.text = null;
                selectedGender.text = null;
                selectedGenderShadow.text = null;
                selectedSpriteAnimation = new Texture[] {};
                //selectedSprite.texture = null;
                //selectedType1.texture = null;
                //selectedType2.texture = null;
                selectedLevel.text = null;
                selectedLevelShadow.text = null;
                selectedAbility.text = null;
                selectedAbilityShadow.text = null;
                selectedItem.text = null;
                selectedItemShadow.text = null;
                //selectedStatus.texture = null;
            }
            else
            {
                selectedName.text = selectedPokemon.Name;
                selectedNameShadow.text = selectedName.text;
                //if (selectedPokemon.getGender() == PokemonOld.Gender.FEMALE)
                //{
                //    selectedGender.text = "♀";
                //    selectedGender.color = new Color(1, 0.2f, 0.2f, 1);
                //}
                //else if (selectedPokemon.getGender() == PokemonOld.Gender.MALE)
                //{
                //    selectedGender.text = "♂";
                //    selectedGender.color = new Color(0.2f, 0.4f, 1, 1);
                //}
                //else
                //{
                //    selectedGender.text = null;
                //}
                selectedGenderShadow.text = selectedGender.text;
                //selectedSpriteAnimation = selectedPokemon.GetFrontAnim();
                //selectedSprite.texture = selectedSpriteAnimation[0];
                //string type1 = PokemonDatabaseOld.getPokemon(selectedPokemon.getID()).getType1().ToString();
                //string type2 = PokemonDatabaseOld.getPokemon(selectedPokemon.getID()).getType2().ToString();
                //selectedType1.texture = null;
                //selectedType2.texture = null;
                //if (type1 != "NONE")
                //{
                //    selectedType1.texture = Resources.Load<Texture>("PCSprites/type" + type1);
                //}
                //if (type2 != "NONE")
                //{
                //    selectedType2.texture = Resources.Load<Texture>("PCSprites/type" + type2);
                //}
                selectedLevel.text = "Level " + selectedPokemon.Level;
                selectedLevelShadow.text = selectedLevel.text;
                //selectedAbility.text =
                //    PokemonDatabaseOld.getPokemon(selectedPokemon.getID()).getAbility(selectedPokemon.getAbility());
                selectedAbilityShadow.text = selectedAbility.text;
                selectedItem.text = "None";
                if (selectedPokemon.Item != PokemonUnity.Inventory.Items.NONE)
                {
                    selectedItem.text = selectedPokemon.Item.ToString();
                }
                selectedItemShadow.text = selectedItem.text;
                //selectedStatus.texture = null;
                //if (selectedPokemon.getStatus() != PokemonOld.Status.NONE)
                //{
                //    selectedStatus.texture =
                //        Resources.Load<Texture>("PCSprites/status" + selectedPokemon.getStatus().ToString());
                //}
            }
        }
    }

    //Show the selectedInfo regardless of carrying or not.
    //private void updateSelectedInfoOverride(PokemonOld selectedPokemon)
    //{
    //    if (carrying)
    //    {
    //        carrying = false;
    //        updateSelectedInfo(selectedPokemon);
    //        carrying = true;
    //    }
    //    else
    //    {
    //        updateSelectedInfo(selectedPokemon);
    //    }
    //}

    private IEnumerator moveBox(int direction)
    {
        float increment = 0;
        float scrollSpeed = 0.4f;
        Vector3 startPosition = currentBox.transform.position;
        if (direction > 0)
        {
            //update destination box's icons incase something has been changed 
            //for (int i = 0; i < 30; i++)
            //{
            //    if (SaveDataOld.currentSave.PC.boxes[nextBoxID][i] == null)
            //    {
            //        nextBoxIconsArray[i].texture = null;
            //    }
            //    else
            //    {
            //        nextBoxIconsArray[i].texture = SaveDataOld.currentSave.PC.boxes[nextBoxID][i].GetIcons();
            //    }
            //}
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
            //for (int i = 0; i < 30; i++)
            //{
            //    if (SaveDataOld.currentSave.PC.boxes[previousBoxID][i] == null)
            //    {
            //        previousBoxIconsArray[i].texture = null;
            //    }
            //    else
            //    {
            //        previousBoxIconsArray[i].texture = SaveDataOld.currentSave.PC.boxes[previousBoxID][i].GetIcons();
            //    }
            //}
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
        //if (currentBoxID == 1)
        //{
        //    previousBoxID = SaveDataOld.currentSave.PC.boxes.Length - 1;
        //}
        //else if (currentBoxID < 1)
        //{
        //    currentBoxID = SaveDataOld.currentSave.PC.boxes.Length - 1;
        //    previousBoxID = SaveDataOld.currentSave.PC.boxes.Length - 2;
        //}
        //else if (currentBoxID == SaveDataOld.currentSave.PC.boxes.Length - 1)
        //{
        //    nextBoxID = 1;
        //}
        //else if (currentBoxID > SaveDataOld.currentSave.PC.boxes.Length - 1)
        //{
        //    currentBoxID = 1;
        //    nextBoxID = 2;
        //}

        //update box names
        //if (string.IsNullOrEmpty(SaveDataOld.currentSave.PC.boxName[currentBoxID]))
        //{
        //    currentBoxHeader.text = "Box " + currentBoxID;
        //}
        //else
        //{
        //    currentBoxHeader.text = SaveDataOld.currentSave.PC.boxName[currentBoxID];
        //}
        //if (string.IsNullOrEmpty(SaveDataOld.currentSave.PC.boxName[nextBoxID]))
        //{
        //    nextBoxHeader.text = "Box " + nextBoxID;
        //}
        //else
        //{
        //    nextBoxHeader.text = SaveDataOld.currentSave.PC.boxName[nextBoxID];
        //}
        //if (string.IsNullOrEmpty(SaveDataOld.currentSave.PC.boxName[previousBoxID]))
        //{
        //    previousBoxHeader.text = "Box " + previousBoxID;
        //}
        //else
        //{
        //    previousBoxHeader.text = SaveDataOld.currentSave.PC.boxName[previousBoxID];
        //}
        //currentBoxHeaderShadow.text = currentBoxHeader.text;
        //nextBoxHeaderShadow.text = nextBoxHeader.text;
        //previousBoxHeaderShadow.text = previousBoxHeader.text;
		//
        ////update box textures
        //if (SaveDataOld.currentSave.PC.boxTexture[currentBoxID] == 0)
        //{
        //    currentBoxTexture.texture = Resources.Load<Texture>("PCSprites/box" + currentBoxID);
        //}
        //else
        //{
        //    currentBoxTexture.texture =
        //        Resources.Load<Texture>("PCSprites/box" + SaveDataOld.currentSave.PC.boxTexture[currentBoxID]);
        //}
        //if (SaveDataOld.currentSave.PC.boxTexture[nextBoxID] == 0)
        //{
        //    nextBoxTexture.texture = Resources.Load<Texture>("PCSprites/box" + nextBoxID);
        //}
        //else
        //{
        //    nextBoxTexture.texture =
        //        Resources.Load<Texture>("PCSprites/box" + SaveDataOld.currentSave.PC.boxTexture[nextBoxID]);
        //}
        //if (SaveDataOld.currentSave.PC.boxTexture[previousBoxID] == 0)
        //{
        //    previousBoxTexture.texture = Resources.Load<Texture>("PCSprites/box" + previousBoxID);
        //}
        //else
        //{
        //    previousBoxTexture.texture =
        //        Resources.Load<Texture>("PCSprites/box" + SaveDataOld.currentSave.PC.boxTexture[previousBoxID]);
        //}

        //update box icons
        //for (int i = 0; i < 30; i++)
        //{
        //    if (SaveDataOld.currentSave.PC.boxes[currentBoxID][i] == null)
        //    {
        //        currentBoxIconsArray[i].texture = null;
        //        currentBoxItemsArray[i].enabled = false;
        //    }
        //    else
        //    {
        //        currentBoxIconsArray[i].texture = SaveDataOld.currentSave.PC.boxes[currentBoxID][i].GetIcons();
        //        if (!string.IsNullOrEmpty(SaveDataOld.currentSave.PC.boxes[currentBoxID][i].getHeldItem()))
        //        {
        //            currentBoxItemsArray[i].enabled = true;
        //        }
        //        else
        //        {
        //            currentBoxItemsArray[i].enabled = false;
        //        }
        //    }
        //}
        //for (int i = 0; i < 30; i++)
        //{
        //    if (SaveDataOld.currentSave.PC.boxes[nextBoxID][i] == null)
        //    {
        //        nextBoxIconsArray[i].texture = null;
        //        nextBoxItemsArray[i].enabled = false;
        //    }
        //    else
        //    {
        //        nextBoxIconsArray[i].texture = SaveDataOld.currentSave.PC.boxes[nextBoxID][i].GetIcons();
        //        if (!string.IsNullOrEmpty(SaveDataOld.currentSave.PC.boxes[nextBoxID][i].getHeldItem()))
        //        {
        //            nextBoxItemsArray[i].enabled = true;
        //        }
        //        else
        //        {
        //            nextBoxItemsArray[i].enabled = false;
        //        }
        //    }
        //}
        //for (int i = 0; i < 30; i++)
        //{
        //    if (SaveDataOld.currentSave.PC.boxes[previousBoxID][i] == null)
        //    {
        //        previousBoxIconsArray[i].texture = null;
        //        previousBoxItemsArray[i].enabled = false;
        //    }
        //    else
        //    {
        //        previousBoxIconsArray[i].texture = SaveDataOld.currentSave.PC.boxes[previousBoxID][i].GetIcons();
        //        if (!string.IsNullOrEmpty(SaveDataOld.currentSave.PC.boxes[previousBoxID][i].getHeldItem()))
        //        {
        //            previousBoxItemsArray[i].enabled = true;
        //        }
        //        else
        //        {
        //            previousBoxItemsArray[i].enabled = false;
        //        }
        //    }
        //}

        currentBox.transform.position = new Vector3(0.27f, currentBox.transform.position.y,
            currentBox.transform.position.z);
        yield return null;
    }

    private IEnumerator moveCursor(Vector2 destination)
    {
        float increment = 0;
        //float startX = cursor.pixelInset.x;
        //float startY = cursor.pixelInset.y;
        //float distanceX = destination.x - startX;
        //float distanceY = destination.y - startY;
        while (increment < 1)
        {
            increment += (1 / moveSpeed) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }
            //cursor.pixelInset = new Rect(startX + (increment * distanceX), startY + (increment * distanceY),
            //    cursor.pixelInset.width, cursor.pixelInset.height);
            //grabbedPokemon.pixelInset = new Rect(cursor.pixelInset.x, cursor.pixelInset.y - 11,
            //    grabbedPokemon.pixelInset.width, grabbedPokemon.pixelInset.height);
            //grabbedPokemonItem.pixelInset = new Rect(cursor.pixelInset.x + 17, cursor.pixelInset.y - 9,
            //    grabbedPokemonItem.pixelInset.width, grabbedPokemonItem.pixelInset.height);
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
            //Vector2[] partyPositions = new Vector2[]
            //{
            //    new Vector2(partyIcons[0].pixelInset.x, partyIcons[0].pixelInset.y),
            //    new Vector2(partyIcons[1].pixelInset.x, partyIcons[1].pixelInset.y),
            //    new Vector2(partyIcons[2].pixelInset.x, partyIcons[2].pixelInset.y),
            //    new Vector2(partyIcons[3].pixelInset.x, partyIcons[3].pixelInset.y),
            //    new Vector2(partyIcons[4].pixelInset.x, partyIcons[4].pixelInset.y),
            //    new Vector2(partyIcons[5].pixelInset.x, partyIcons[5].pixelInset.y)
            //};
            //while (hole < 4)
            //{
            //    //until the hole is either at the end of the party, or not there.
            //    SaveDataOld.currentSave.PC.swapPokemon(0, hole, 0, hole + 1);
            //    StartCoroutine(moveIcon(partyIcons[hole + 1], partyPositions[hole]));
            //    hole += 1;
            //}
            //SaveDataOld.currentSave.PC.swapPokemon(0, hole, 0, hole + 1);
            //yield return StartCoroutine(moveIcon(partyIcons[hole + 1], partyPositions[hole]));
            hole = currentPosition;
            while (hole < 5)
            {
                //partyIcons[hole].texture = partyIcons[hole + 1].texture;
                //partyItems[hole].enabled = partyItems[hole + 1].enabled;
                //partyIcons[hole].pixelInset = new Rect(partyPositions[hole].x, partyPositions[hole].y,
                //    partyIcons[hole].pixelInset.width, partyIcons[hole].pixelInset.height);
                //partyItems[hole].pixelInset = new Rect(partyPositions[hole].x + 17, partyPositions[hole].y + 2,
                //    partyItems[hole].pixelInset.width, partyItems[hole].pixelInset.height);
                hole += 1;
            }
            //partyIcons[5].texture = null;
            //partyItems[5].enabled = false;
            //partyIcons[5].pixelInset = new Rect(partyPositions[5].x, partyPositions[5].y, partyIcons[5].pixelInset.width,
            //    partyIcons[5].pixelInset.height);
            //partyItems[5].pixelInset = new Rect(partyPositions[5].x + 17, partyPositions[5].y + 2,
            //    partyItems[5].pixelInset.width, partyItems[5].pixelInset.height);
            selectedIndex = hole;
        }
		yield return null;
    }

    private IEnumerator endOfParty(int currentPosition)
    {
        int icon = currentPosition;
		//Vector2[] partyPositions = new Vector2[]
		//{
		//    new Vector2(partyIcons[0].pixelInset.x, partyIcons[0].pixelInset.y),
		//    new Vector2(partyIcons[1].pixelInset.x, partyIcons[1].pixelInset.y),
		//    new Vector2(partyIcons[2].pixelInset.x, partyIcons[2].pixelInset.y),
		//    new Vector2(partyIcons[3].pixelInset.x, partyIcons[3].pixelInset.y),
		//    new Vector2(partyIcons[4].pixelInset.x, partyIcons[4].pixelInset.y),
		//    new Vector2(partyIcons[5].pixelInset.x, partyIcons[5].pixelInset.y)
		//};
		//if (icon > 0)
		//{
		//    while (SaveDataOld.currentSave.PC.boxes[0][icon - 1] == null && icon > 1)
		//    {
		//        //if the previous spot is free, and is not first spot
		//        yield return StartCoroutine(moveIcon(partyIcons[icon], partyPositions[icon - 1]));
		//        partyIcons[icon - 1].texture = partyIcons[icon].texture;
		//        partyIcons[icon].texture = null;
		//        partyIcons[icon].pixelInset = new Rect(partyPositions[icon].x, partyPositions[icon].y,
		//            partyIcons[icon].pixelInset.width, partyIcons[icon].pixelInset.height);
		//        partyItems[icon - 1].enabled = partyItems[icon].enabled;
		//        partyItems[icon].enabled = false;
		//        partyItems[icon].pixelInset = new Rect(partyPositions[icon].x + 17, partyPositions[icon].y + 2,
		//            partyItems[icon].pixelInset.width, partyItems[icon].pixelInset.height);
		//        SaveDataOld.currentSave.PC.swapPokemon(0, icon - 1, 0, icon);
		//        icon -= 1;
		//        updateSelectedInfo(SaveDataOld.currentSave.PC.boxes[0][currentPosition]);
		//    }
		//    if (SaveDataOld.currentSave.PC.boxes[0][icon - 1] == null)
		//    {
		//        yield return StartCoroutine(moveIcon(partyIcons[icon], partyPositions[icon - 1]));
		//        partyIcons[icon - 1].texture = partyIcons[icon].texture;
		//        partyIcons[icon].texture = null;
		//        partyIcons[icon].pixelInset = new Rect(partyPositions[icon].x, partyPositions[icon].y,
		//            partyIcons[icon].pixelInset.width, partyIcons[icon].pixelInset.height);
		//        partyItems[icon - 1].enabled = partyItems[icon].enabled;
		//        partyItems[icon].enabled = false;
		//        partyItems[icon].pixelInset = new Rect(partyPositions[icon].x + 17, partyPositions[icon].y + 2,
		//            partyItems[icon].pixelInset.width, partyItems[icon].pixelInset.height);
		//        SaveDataOld.currentSave.PC.swapPokemon(0, icon - 1, 0, icon);
		//        updateSelectedInfo(SaveDataOld.currentSave.PC.boxes[0][currentPosition]);
		//    }
		//    icon = 0;
		//}
		yield return null;
    }

    private IEnumerator moveIcon(Texture icon, Vector2 destination)
    {
        //Texture item = icon.transform.Find("Item").GetComponent<Texture>();

        //float startX = icon.pixelInset.x;
        //float startY = icon.pixelInset.y;
        //float distanceX = destination.x - startX;
        //float distanceY = destination.y - startY;
        //float itemOffsetX = item.pixelInset.x - icon.pixelInset.x;
        //float itemOffsetY = item.pixelInset.y - icon.pixelInset.y;

        float increment = 0;
        while (increment < 1)
        {
            increment += (1 / moveSpeed) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }
            //icon.pixelInset = new Rect(startX + (increment * distanceX), startY + (increment * distanceY),
            //    icon.pixelInset.width, icon.pixelInset.height);
            //item.pixelInset = new Rect(startX + (increment * distanceX) + itemOffsetX,
            //    startY + (increment * distanceY) + itemOffsetY, item.pixelInset.width, item.pixelInset.height);
            yield return null;
        }
    }

    private IEnumerator pickUpPokemon(int currentBoxID, int currentPosition)
    {
        //selectedPokemon = SaveDataOld.currentSave.PC.boxes[currentBoxID][currentPosition];
        selectedBoxID = currentBoxID;
        selectedIndex = currentPosition;
        carrying = true;
        //cursor.border = new RectOffset(32, 0, 0, 32);
        SfxHandler.Play(pickUpClip);
		//yield return StartCoroutine(moveCursor(new Vector2(cursor.pixelInset.x, cursor.pixelInset.y - 10)));
		//grabbedPokemon.texture = selectedPokemon.GetIcons();
		//if (!string.IsNullOrEmpty(selectedPokemon.getHeldItem()))
		//{
		//    grabbedPokemonItem.enabled = true;
		//}
		//else
		//{
		//    grabbedPokemonItem.enabled = false;
		//}
		//if (currentBoxID == 0)
		//{
		//    partyIcons[currentPosition].texture = null;
		//    partyItems[currentPosition].enabled = false;
		//    StartCoroutine(packParty(currentPosition));
		//}
		//else
		//{
		//    currentBoxIconsArray[currentPosition].texture = null;
		//    currentBoxItemsArray[currentPosition].enabled = false;
		//}
		//cursor.border = new RectOffset(0, 32, 0, 32);
		//yield return StartCoroutine(moveCursor(new Vector2(cursor.pixelInset.x, cursor.pixelInset.y + 10)));
		yield return null;
    }

    private IEnumerator putDownPokemon(int currentBoxID, int currentPosition)
    {
        //bool originalSpot = false;
        //if (currentBoxID == selectedBoxID && currentPosition == selectedIndex)
        //{
        //    originalSpot = true;
        //}
		//if (SaveDataOld.currentSave.PC.boxes[currentBoxID][currentPosition] == null || originalSpot)
		//{
		//    SfxHandler.Play(putDownClip);
		//    yield return StartCoroutine(moveCursor(new Vector2(cursor.pixelInset.x, cursor.pixelInset.y - 10)));
		//    if (currentBoxID == 0)
		//    {
		//        partyIcons[currentPosition].texture = grabbedPokemon.texture;
		//        partyItems[currentPosition].enabled = grabbedPokemonItem.enabled;
		//        StartCoroutine(endOfParty(currentPosition));
		//    }
		//    else
		//    {
		//        currentBoxIconsArray[currentPosition].texture = grabbedPokemon.texture;
		//        currentBoxItemsArray[currentPosition].enabled = grabbedPokemonItem.enabled;
		//    }
		//    if (selectedBoxID == 0)
		//    {
		//        SaveDataOld.currentSave.PC.swapPokemon(selectedBoxID, 5, currentBoxID, currentPosition);
		//    }
		//    else
		//    {
		//        SaveDataOld.currentSave.PC.swapPokemon(selectedBoxID, selectedIndex, currentBoxID, currentPosition);
		//    }
		//    grabbedPokemon.texture = null;
		//    grabbedPokemonItem.enabled = false;
		//    cursor.border = new RectOffset(32, 0, 0, 32);
		//    yield return StartCoroutine(moveCursor(new Vector2(cursor.pixelInset.x, cursor.pixelInset.y + 10)));
		//    carrying = false;
		//    if (currentBoxID != 0)
		//    {
		//        //fully heal if depositing into PC
		//        SaveDataOld.currentSave.PC.boxes[currentBoxID][currentPosition].healFull();
		//        updateSelectedInfo(SaveDataOld.currentSave.PC.boxes[currentBoxID][currentPosition]);
		//    }
		//}
		yield return null;
    }

    private IEnumerator switchPokemon(int currentBoxID, int currentPosition)
    {
		//if (SaveDataOld.currentSave.PC.boxes[currentBoxID][currentPosition] != null)
		//{
		//    GUITexture targetIcon = null;
		//    GUITexture targetItem = null;
		//    if (currentBoxID == 0)
		//    {
		//        targetIcon = partyIcons[currentPosition];
		//        targetItem = partyItems[currentPosition];
		//    }
		//    else
		//    {
		//        targetIcon = currentBoxIconsArray[currentPosition];
		//        targetItem = currentBoxItemsArray[currentPosition];
		//    }
		//    cursor.border = new RectOffset(32, 0, 0, 32);
		//    SfxHandler.Play(putDownClip);
		//    StartCoroutine(moveIcon(grabbedPokemon,
		//        new Vector2(grabbedPokemon.pixelInset.x + 5, grabbedPokemon.pixelInset.y - 5)));
		//    yield return
		//        StartCoroutine(moveIcon(targetIcon,
		//            new Vector2(targetIcon.pixelInset.x - 5, targetIcon.pixelInset.y + 5)));
		//
		//    Texture temp = targetIcon.texture;
		//    bool itemTemp = targetItem.enabled;
		//    //swap target icon's position and grabbedPokemon's position, and update their new textures
		//    targetIcon.pixelInset = new Rect(targetIcon.pixelInset.x + 10, targetIcon.pixelInset.y,
		//        targetIcon.pixelInset.width, targetIcon.pixelInset.height);
		//    targetItem.pixelInset = new Rect(targetItem.pixelInset.x + 10, targetItem.pixelInset.y,
		//        targetItem.pixelInset.width, targetItem.pixelInset.height);
		//    targetIcon.texture = grabbedPokemon.texture;
		//    targetItem.enabled = grabbedPokemonItem.enabled;
		//    grabbedPokemon.pixelInset = new Rect(grabbedPokemon.pixelInset.x - 10, grabbedPokemon.pixelInset.y,
		//        grabbedPokemon.pixelInset.width, grabbedPokemon.pixelInset.height);
		//    grabbedPokemonItem.pixelInset = new Rect(grabbedPokemonItem.pixelInset.x - 10,
		//        grabbedPokemonItem.pixelInset.y, grabbedPokemonItem.pixelInset.width,
		//        grabbedPokemonItem.pixelInset.height);
		//    grabbedPokemon.texture = temp;
		//    grabbedPokemonItem.enabled = itemTemp;
		//
		//    //update selected info
		//    updateSelectedInfoOverride(SaveDataOld.currentSave.PC.boxes[currentBoxID][currentPosition]);
		//    //swap pokemon
		//    SaveDataOld.currentSave.PC.swapPokemon(selectedBoxID, selectedIndex, currentBoxID, currentPosition);
		//
		//    SfxHandler.Play(pickUpClip);
		//    StartCoroutine(moveIcon(grabbedPokemon,
		//        new Vector2(grabbedPokemon.pixelInset.x + 5, grabbedPokemon.pixelInset.y + 5)));
		//    yield return
		//        StartCoroutine(moveIcon(targetIcon,
		//            new Vector2(targetIcon.pixelInset.x - 5, targetIcon.pixelInset.y - 5)));
		//
		//    cursor.border = new RectOffset(0, 32, 0, 32);
		//
		//    if (currentBoxID != 0)
		//    {
		//        //fully heal if depositing into PC
		//        SaveDataOld.currentSave.PC.boxes[currentBoxID][currentPosition].healFull();
		//    }
		//}
		yield return null;
    }

    private IEnumerator releasePokemon(int currentBoxID, int currentPosition)
    {
        Texture targetIcon = null;
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
        //float startY = targetIcon.pixelInset.y;
        while (increment < 1)
        {
            increment += (1 / moveSpeedSlow) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }
            //targetIcon.pixelInset = new Rect(targetIcon.pixelInset.x, startY + (16 * increment), 32,
            //    (32f * (1f - increment)));
            yield return null;
        }
        //SaveDataOld.currentSave.PC.boxes[currentBoxID][currentPosition] = null;
        //targetIcon.texture = null;
        //targetIcon.pixelInset = new Rect(targetIcon.pixelInset.x, startY, 32, 32);
    }

    private IEnumerator withdrawPokemon(int currentPosition)
    {
        int targetPosition = 6;
        //for (int i = 1; i < 6; i++)
        //{
        //    if (SaveDataOld.currentSave.PC.boxes[0][i] == null)
        //    {
        //        targetPosition = i;
        //        i = 6;
        //    }
        //}
        if (targetPosition < 6)
        {
            yield return StartCoroutine(pickUpPokemon(currentBoxID, currentPosition));
            //float startX = cursor.pixelInset.x;
            //float startY = cursor.pixelInset.y;
            //yield return
            //    StartCoroutine(
            //        moveCursor(new Vector2(partyIcons[targetPosition].pixelInset.x + 267,
            //            partyIcons[targetPosition].pixelInset.y + 20)));
            yield return StartCoroutine(putDownPokemon(0, targetPosition));
            //yield return StartCoroutine(moveCursor(new Vector2(startX, startY)));
        }
    }

    private IEnumerator depositPokemon(int currentPosition, int targetPosition)
    {
        if (targetPosition < 30)
        {
            yield return StartCoroutine(pickUpPokemon(0, currentPosition));
            //float startX = cursor.pixelInset.x;
            //float startY = cursor.pixelInset.y;
            //yield return
            //    StartCoroutine(
            //        moveCursor(new Vector2(currentBoxIconsArray[targetPosition].pixelInset.x + 92,
            //            currentBoxIconsArray[targetPosition].pixelInset.y + 58)));
            yield return StartCoroutine(putDownPokemon(currentBoxID, targetPosition));
            //yield return StartCoroutine(moveCursor(new Vector2(startX, startY)));
        }
    }

    //private IEnumerator animatePokemon()
    //{
    //    frame = 0;
    //    while (true)
    //    {
    //        if (selectedSpriteAnimation.Length > 0)
    //        {
    //            if (frame < selectedSpriteAnimation.Length - 1)
    //            {
    //                frame += 1;
    //            }
    //            else
    //            {
    //                frame = 0;
    //            }
    //            selectedSprite.texture = selectedSpriteAnimation[frame];
    //        }
    //        yield return new WaitForSeconds(0.08f);
    //    }
    //}

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

        //cursor.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        cursorMode = CursorMode.Standard;


        currentBoxID = 1;
        nextBoxID = 2;
        //previousBoxID = SaveDataOld.currentSave.PC.boxes.Length - 1;

        updateBoxesAndParty();

        //updateSelectedInfo(SaveDataOld.currentSave.PC.boxes[currentBoxID][currentPosition - 3]);
        //cursor.pixelInset = new Rect(cursorPositions[currentPosition].x, cursorPositions[currentPosition].y,
        //    cursor.pixelInset.width, cursor.pixelInset.height);
        //grabbedPokemon.texture = null;
        //grabbedPokemonItem.enabled = false;

        running = true;
        StartCoroutine("animateCursor");
        StartCoroutine("animatePokemon");
        yield return new WaitForSeconds(0.5f);
        while (running)
        {
            //if cursor is in boxIndex
            if (currentPosition < 3)
            {
                if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    /*	if(currentPosition < 2){
                            currentPosition += 1;
                            SfxHandler.Play(selectClip);
                            updateSelectedInfo(null);}
                        yield return StartCoroutine(moveCursor(cursorPositions[currentPosition])); */
                    SfxHandler.Play(selectClip);
                    yield return StartCoroutine(moveBox(1));
                }
                else if (Input.GetAxisRaw("Vertical") < 0)
                {
                    if (currentPosition == 0)
                    {
                        currentPosition = 3;
                        SfxHandler.Play(selectClip);
                        //updateSelectedInfo(SaveDataOld.currentSave.PC.boxes[currentBoxID][currentPosition - 3]);
                    }
                    else if (currentPosition == 1)
                    {
                        currentPosition = 6;
                        SfxHandler.Play(selectClip);
                       // updateSelectedInfo(SaveDataOld.currentSave.PC.boxes[currentBoxID][currentPosition - 3]);
                    }
                    else if (currentPosition == 2)
                    {
                        currentPosition = 8;
                        SfxHandler.Play(selectClip);
                        //updateSelectedInfo(SaveDataOld.currentSave.PC.boxes[currentBoxID][currentPosition - 3]);
                    }
                    yield return StartCoroutine(moveCursor(cursorPositions[currentPosition]));
                }
                else if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    /*	if(currentPosition > 0){
                            currentPosition -= 1;
                            SfxHandler.Play(selectClip);
                            updateSelectedInfo(null);}
                        yield return StartCoroutine(moveCursor(cursorPositions[currentPosition])); */
                    SfxHandler.Play(selectClip);
                    yield return StartCoroutine(moveBox(-1));
                }
                else if (Input.GetButton("Select"))
                {
                    string[] choices = new string[] {"Jump", "Wallpaper", "Name", "Cancel"};
                    Dialog.drawDialogBox();
                    Dialog.drawTextInstant("What would you like to do?");
                    Dialog.drawChoiceBox(choices);
                    yield return new WaitForSeconds(0.2f);
                    yield return StartCoroutine(Dialog.choiceNavigate(choices));
                    Dialog.undrawChoiceBox();
                    Dialog.undrawDialogBox();
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
                        //StartCoroutine(Scene.main.Typing.control(8, currentBoxHeader.text, PokemonOld.Gender.NONE,
                        //    new Sprite[] {boxEditIcon}));
                        while (Scene.main.Typing.gameObject.activeSelf)
                        {
                            yield return null;
                        }
                        if (Scene.main.Typing.typedString.Length > 0)
                        {
                            //SaveDataOld.currentSave.PC.boxName[currentBoxID] = Scene.main.Typing.typedString;
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
                else if (Input.GetButton("Back"))
                {
                    if (carrying)
                    {
                        Dialog.drawDialogBox();
                        Dialog.drawTextInstant("You're holding a Pokémon!");
                        yield return new WaitForSeconds(0.2f);
                        while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                        {
                            yield return null;
                        }
                        Dialog.undrawDialogBox();
                        yield return new WaitForSeconds(0.2f);
                    }
                    else
                    {
                        currentPosition = 41;
                        SfxHandler.Play(selectClip);
                        updateSelectedInfo(null);
                        yield return StartCoroutine(moveCursor(cursorPositions[currentPosition]));

                        Dialog.drawDialogBox();
                        Dialog.drawTextInstant("Continue Box operations?");
                        Dialog.drawChoiceBox();
                        yield return new WaitForSeconds(0.2f);
                        yield return Dialog.StartCoroutine("choiceNavigate");
                        Dialog.undrawChoiceBox();
                        Dialog.undrawDialogBox();
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
                if (Input.GetAxisRaw("Vertical") > 0)
                {
                    //if along the top row, act differently depending on location
                    if (currentPosition < 9)
                    {
                        /*		if(currentPosition == 3){
                                    currentPosition = 0;
                                    SfxHandler.Play(selectClip);
                                    updateSelectedInfo(null);}
                                else if(currentPosition == 8){
                                    currentPosition = 2;
                                    SfxHandler.Play(selectClip);
                                    updateSelectedInfo(null);}
                                else{
                        */
                        currentPosition = 1;
                        SfxHandler.Play(selectClip);
                        updateSelectedInfo(null);
                    }
                    //	}
                    else
                    {
                        currentPosition -= 6;
                        SfxHandler.Play(selectClip);
                        //updateSelectedInfo(SaveDataOld.currentSave.PC.boxes[currentBoxID][currentPosition - 3]);
                    }
                    yield return StartCoroutine(moveCursor(cursorPositions[currentPosition]));
                }
                else if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    //if along the right column, move to party
                    if (currentPosition == 8 || currentPosition == 14)
                    {
                        currentPosition = 33;
                        SfxHandler.Play(selectClip);
                        //updateSelectedInfo(SaveDataOld.currentSave.PC.boxes[0][currentPosition - 33]);
                    }
                    else if (currentPosition == 20)
                    {
                        currentPosition = 35;
                        SfxHandler.Play(selectClip);
                        //updateSelectedInfo(SaveDataOld.currentSave.PC.boxes[0][currentPosition - 33]);
                    }
                    else if (currentPosition == 26 || currentPosition == 32)
                    {
                        currentPosition = 37;
                        SfxHandler.Play(selectClip);
                        //updateSelectedInfo(SaveDataOld.currentSave.PC.boxes[0][currentPosition - 33]);
                    }
                    //otherwise go one to the right
                    else
                    {
                        currentPosition += 1;
                        SfxHandler.Play(selectClip);
                        //updateSelectedInfo(SaveDataOld.currentSave.PC.boxes[currentBoxID][currentPosition - 3]);
                    }
                    yield return StartCoroutine(moveCursor(cursorPositions[currentPosition]));
                }
                else if (Input.GetAxisRaw("Vertical") < 0)
                {
                    if (currentPosition < 27)
                    {
                        currentPosition += 6;
                        SfxHandler.Play(selectClip);
                        //updateSelectedInfo(SaveDataOld.currentSave.PC.boxes[currentBoxID][currentPosition - 3]);
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
                else if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    //if not along the left column, move left one
                    if ((currentPosition + 3) % 6 != 0)
                    {
                        currentPosition -= 1;
                        SfxHandler.Play(selectClip);
                        //updateSelectedInfo(SaveDataOld.currentSave.PC.boxes[currentBoxID][currentPosition - 3]);
                    }
                    yield return StartCoroutine(moveCursor(cursorPositions[currentPosition]));
                }
                else if (Input.GetButton("Select"))
                {
                    //if (SaveDataOld.currentSave.PC.boxes[currentBoxID][currentPosition - 3] != null)
                    //{
                    //    updateSelectedInfo(SaveDataOld.currentSave.PC.boxes[currentBoxID][currentPosition - 3]);
                    //    //STANDARD
                    //    if (cursorMode == CursorMode.Standard)
                    //    {
                    //        if (!carrying)
                    //        {
                    //            string[] choices = new string[]
                    //                {"Pick Up", "Summary", "Item", "Withdraw", "Release", "Cancel"};
                    //            Dialog.drawDialogBox();
                    //            Dialog.drawTextInstant("What would you like to do with " +
                    //                                   SaveDataOld.currentSave.PC.boxes[currentBoxID][currentPosition - 3]
                    //                                       .getName() + "?");
                    //            Dialog.drawChoiceBox(choices);
                    //            yield return new WaitForSeconds(0.2f);
                    //            yield return StartCoroutine(Dialog.choiceNavigate(choices));
                    //            Dialog.undrawChoiceBox();
                    //            Dialog.undrawDialogBox();
                    //            int chosenIndex = Dialog.chosenIndex;
                    //            if (chosenIndex == 5)
                    //            {
                    //                yield return StartCoroutine(pickUpPokemon(currentBoxID, currentPosition - 3));
                    //            }
                    //            else if (chosenIndex == 4)
                    //            {
                    //                //SUMMARY
					//
                    //                SfxHandler.Play(selectClip);
                    //                //yield return new WaitForSeconds(sceneTransition.FadeOut(0.4f));
                    //                yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));
					//
                    //                //Set SceneSummary to be active so that it appears
                    //                Scene.main.Summary.gameObject.SetActive(true);
                    //                StartCoroutine(
                    //                    Scene.main.Summary.control(SaveDataOld.currentSave.PC.boxes[currentBoxID],
                    //                        currentPosition - 3));
                    //                //Start an empty loop that will only stop when ScenePC is no longer active (is closed)
                    //                while (Scene.main.Summary.gameObject.activeSelf)
                    //                {
                    //                    yield return null;
                    //                }
					//
                    //                //yield return new WaitForSeconds(sceneTransition.FadeIn(0.4f));
                    //                yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));
					//
                    //                chosenIndex = 0;
                    //            }
                    //            else if (chosenIndex == 3)
                    //            {
                    //                //ITEM
                    //                PokemonOld currentPokemon =
                    //                    SaveDataOld.currentSave.PC.boxes[currentBoxID][currentPosition - 3];
					//
                    //                Dialog.undrawChoiceBox();
                    //                Dialog.drawDialogBox();
                    //                if (!string.IsNullOrEmpty(currentPokemon.getHeldItem()))
                    //                {
                    //                    yield return
                    //                        StartCoroutine(
                    //                            Dialog.drawText(currentPokemon.getName() + " is holding " +
                    //                                            currentPokemon.getHeldItem() + "."));
                    //                    string[] itemChoices = new string[]
                    //                    {
                    //                        "Swap", "Take", "Cancel"
                    //                    };
                    //                    int itemChosenIndex = -1;
                    //                    Dialog.drawChoiceBox(itemChoices);
                    //                    yield return new WaitForSeconds(0.2f);
                    //                    yield return StartCoroutine(Dialog.choiceNavigate(itemChoices));
					//
                    //                    itemChosenIndex = Dialog.chosenIndex;
					//
                    //                    if (itemChosenIndex == 2)
                    //                    {
                    //                        //Swap
                    //                        SfxHandler.Play(selectClip);
                    //                        //yield return new WaitForSeconds(sceneTransition.FadeOut(0.4f));
                    //                        yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));
					//
                    //                        Scene.main.Bag.gameObject.SetActive(true);
                    //                        StartCoroutine(Scene.main.Bag.control(false, true));
                    //                        while (Scene.main.Bag.gameObject.activeSelf)
                    //                        {
                    //                            yield return null;
                    //                        }
					//
                    //                        string chosenItem = Scene.main.Bag.chosenItem;
					//
                    //                        Dialog.undrawChoiceBox();
                    //                        Dialog.undrawDialogBox();
					//
                    //                        //yield return new WaitForSeconds(sceneTransition.FadeIn(0.4f));
                    //                        yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));
					//
                    //                        if (!string.IsNullOrEmpty(chosenItem))
                    //                        {
                    //                            Dialog.drawDialogBox();
                    //                            yield return
                    //                                StartCoroutine(
                    //                                    Dialog.drawText("Swap " + currentPokemon.getHeldItem() + " for " +
                    //                                                    chosenItem + "?"));
                    //                            Dialog.drawChoiceBox();
                    //                            yield return StartCoroutine(Dialog.choiceNavigate());
					//
                    //                            itemChosenIndex = Dialog.chosenIndex;
                    //                            Dialog.undrawChoiceBox();
					//
                    //                            if (itemChosenIndex == 1)
                    //                            {
                    //                                string receivedItem = currentPokemon.swapHeldItem(chosenItem);
                    //                                SaveDataOld.currentSave.Bag.addItem(receivedItem, 1);
                    //                                SaveDataOld.currentSave.Bag.removeItem(chosenItem, 1);
					//
                    //                                Dialog.drawDialogBox();
                    //                                yield return
                    //                                    Dialog.StartCoroutine("drawText",
                    //                                        "Gave " + chosenItem + " to " + currentPokemon.getName() +
                    //                                        ",");
                    //                                while (!Input.GetButtonDown("Select") &&
                    //                                       !Input.GetButtonDown("Back"))
                    //                                {
                    //                                    yield return null;
                    //                                }
                    //                                Dialog.drawDialogBox();
                    //                                yield return
                    //                                    Dialog.StartCoroutine("drawText",
                    //                                        "and received " + receivedItem + " in return.");
                    //                                while (!Input.GetButtonDown("Select") &&
                    //                                       !Input.GetButtonDown("Back"))
                    //                                {
                    //                                    yield return null;
                    //                                }
                    //                            }
                    //                        }
                    //                    }
                    //                    else if (itemChosenIndex == 1)
                    //                    {
                    //                        //Take
                    //                        Dialog.undrawChoiceBox();
                    //                        string receivedItem = currentPokemon.swapHeldItem("");
                    //                        SaveDataOld.currentSave.Bag.addItem(receivedItem, 1);
					//
                    //                        //adjust displayed data
                    //                        updateSelectedInfo(currentPokemon);
                    //                        currentBoxItemsArray[currentPosition - 3].enabled = false;
					//
                    //                        Dialog.drawDialogBox();
                    //                        yield return
                    //                            StartCoroutine(
                    //                                Dialog.drawText("Took " + receivedItem + " from " +
                    //                                                currentPokemon.getName() + "."));
                    //                        while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                    //                        {
                    //                            yield return null;
                    //                        }
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    yield return
                    //                        StartCoroutine(
                    //                            Dialog.drawText(currentPokemon.getName() + " isn't holding anything."));
                    //                    string[] itemChoices = new string[]
                    //                    {
                    //                        "Give", "Cancel"
                    //                    };
                    //                    int itemChosenIndex = -1;
                    //                    Dialog.drawChoiceBox(itemChoices);
                    //                    yield return new WaitForSeconds(0.2f);
                    //                    yield return StartCoroutine(Dialog.choiceNavigate(itemChoices));
                    //                    itemChosenIndex = Dialog.chosenIndex;
					//
                    //                    if (itemChosenIndex == 1)
                    //                    {
                    //                        //Give
                    //                        SfxHandler.Play(selectClip);
                    //                        //yield return new WaitForSeconds(sceneTransition.FadeOut(0.4f));
                    //                        yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));
					//
                    //                        Scene.main.Bag.gameObject.SetActive(true);
                    //                        StartCoroutine(Scene.main.Bag.control(false, true));
                    //                        while (Scene.main.Bag.gameObject.activeSelf)
                    //                        {
                    //                            yield return null;
                    //                        }
					//
                    //                        string chosenItem = Scene.main.Bag.chosenItem;
					//
                    //                        Dialog.undrawChoiceBox();
                    //                        Dialog.undrawDialogBox();
					//
                    //                        //yield return new WaitForSeconds(sceneTransition.FadeIn(0.4f));
                    //                        yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));
					//
                    //                        if (!string.IsNullOrEmpty(chosenItem))
                    //                        {
                    //                            currentPokemon.swapHeldItem(chosenItem);
                    //                            SaveDataOld.currentSave.Bag.removeItem(chosenItem, 1);
					//
                    //                            //adjust displayed data
                    //                            updateSelectedInfo(currentPokemon);
                    //                            currentBoxItemsArray[currentPosition - 3].enabled = true;
					//
                    //                            Dialog.drawDialogBox();
                    //                            yield return
                    //                                Dialog.StartCoroutine("drawText",
                    //                                    "Gave " + chosenItem + " to " + currentPokemon.getName() + ".");
                    //                            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                    //                            {
                    //                                yield return null;
                    //                            }
                    //                        }
                    //                    }
                    //                }
                    //                Dialog.undrawChoiceBox();
                    //                Dialog.undrawDialogBox();
                    //                yield return new WaitForSeconds(0.2f);
                    //                chosenIndex = 0;
                    //            }
                    //            else if (chosenIndex == 2)
                    //            {
                    //                //WITHDRAW
                    //                if (SaveDataOld.currentSave.PC.boxes[0][5] != null)
                    //                {
                    //                    //if party is full
                    //                    Dialog.drawDialogBox();
                    //                    Dialog.drawTextInstant("Your party is full!");
                    //                    yield return new WaitForSeconds(0.2f);
                    //                    while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                    //                    {
                    //                        yield return null;
                    //                    }
                    //                    Dialog.undrawDialogBox();
                    //                }
                    //                else
                    //                {
                    //                    yield return StartCoroutine(withdrawPokemon(currentPosition - 3));
                    //                    updateSelectedInfo(
                    //                        SaveDataOld.currentSave.PC.boxes[currentBoxID][currentPosition - 3]);
                    //                    chosenIndex = 0;
                    //                }
                    //            }
                    //            else if (chosenIndex == 1)
                    //            {
                    //                //RELEASE
                    //                int releaseIndex = 1;
                    //                string pokemonName =
                    //                    SaveDataOld.currentSave.PC.boxes[currentBoxID][currentPosition - 3].getName();
                    //                while (releaseIndex != 0)
                    //                {
                    //                    Dialog.drawDialogBox();
                    //                    Dialog.drawTextInstant("Do you want to release " + pokemonName + "?");
                    //                    Dialog.drawChoiceBoxNo();
                    //                    yield return new WaitForSeconds(0.2f);
                    //                    yield return StartCoroutine(Dialog.choiceNavigateNo());
                    //                    Dialog.undrawChoiceBox();
                    //                    releaseIndex = Dialog.chosenIndex;
                    //                    if (releaseIndex == 1)
                    //                    {
                    //                        yield return
                    //                            StartCoroutine(releasePokemon(currentBoxID, currentPosition - 3));
                    //                        Dialog.drawDialogBox();
                    //                        Dialog.drawTextInstant(pokemonName + " was released.");
                    //                        yield return new WaitForSeconds(0.2f);
                    //                        while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                    //                        {
                    //                            yield return null;
                    //                        }
                    //                        Dialog.drawDialogBox();
                    //                        Dialog.drawTextInstant("Bye bye, " + pokemonName + "!");
                    //                        yield return new WaitForSeconds(0.2f);
                    //                        while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                    //                        {
                    //                            yield return null;
                    //                        }
                    //                        releaseIndex = 0;
                    //                        chosenIndex = 0;
                    //                        updateSelectedInfo(
                    //                            SaveDataOld.currentSave.PC.boxes[currentBoxID][currentPosition - 3]);
                    //                    }
                    //                    Dialog.undrawDialogBox();
                    //                }
                    //                yield return new WaitForSeconds(0.2f);
                    //            }
                    //            else
                    //            {
                    //                yield return new WaitForSeconds(0.2f);
                    //            }
                    //        }
                    //        else if (currentBoxID == selectedBoxID && currentPosition - 3 == selectedIndex)
                    //        {
                    //            yield return StartCoroutine(putDownPokemon(currentBoxID, currentPosition - 3));
                    //        }
                    //        else
                    //        {
                    //            yield return StartCoroutine(switchPokemon(currentBoxID, currentPosition - 3));
                    //        }
                    //    }
                    //    //HOT MOVE
                    //    else if (cursorMode == CursorMode.HotMove)
                    //    {
                    //        if (!carrying)
                    //        {
                    //            yield return StartCoroutine(pickUpPokemon(currentBoxID, currentPosition - 3));
                    //        }
                    //        else if (currentBoxID == selectedBoxID && currentPosition - 3 == selectedIndex)
                    //        {
                    //            yield return StartCoroutine(putDownPokemon(currentBoxID, currentPosition - 3));
                    //        }
                    //        else
                    //        {
                    //            yield return StartCoroutine(switchPokemon(currentBoxID, currentPosition - 3));
                    //        }
                    //    }
                    //    //WITHDRAW DEPOSIT
                    //    else
                    //    {
                    //        if (SaveDataOld.currentSave.PC.boxes[0][5] != null)
                    //        {
                    //            //if party is full
                    //            Dialog.drawDialogBox();
                    //            Dialog.drawTextInstant("Your party is full!");
                    //            yield return new WaitForSeconds(0.2f);
                    //            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back") &&
                    //                   Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
                    //            {
                    //                yield return null;
                    //            }
                    //            Dialog.undrawDialogBox();
                    //            yield return new WaitForSeconds(0.2f);
                    //        }
                    //        else
                    //        {
                    //            yield return StartCoroutine(withdrawPokemon(currentPosition - 3));
                    //            updateSelectedInfo(SaveDataOld.currentSave.PC.boxes[currentBoxID][currentPosition - 3]);
                    //        }
                    //    }
                    //}
                    //else if (carrying)
                    //{
                    //    yield return StartCoroutine(putDownPokemon(currentBoxID, currentPosition - 3));
                    //}
                }
                else if (Input.GetButton("Back"))
                {
                    if (carrying)
                    {
                        Dialog.drawDialogBox();
                        Dialog.drawTextInstant("You're holding a Pokémon!");
                        yield return new WaitForSeconds(0.2f);
                        while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                        {
                            yield return null;
                        }
                        Dialog.undrawDialogBox();
                        yield return new WaitForSeconds(0.2f);
                    }
                    else
                    {
                        currentPosition = 41;
                        SfxHandler.Play(selectClip);
                        updateSelectedInfo(null);
                        yield return StartCoroutine(moveCursor(cursorPositions[currentPosition]));

                        Dialog.drawDialogBox();
                        Dialog.drawTextInstant("Continue Box operations?");
                        Dialog.drawChoiceBox();
                        yield return new WaitForSeconds(0.2f);
                        yield return Dialog.StartCoroutine("choiceNavigate");
                        Dialog.undrawChoiceBox();
                        Dialog.undrawDialogBox();
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
                if (Input.GetAxisRaw("Vertical") > 0)
                {
                    if (currentPosition > 34)
                    {
                        currentPosition -= 2;
                        SfxHandler.Play(selectClip);
                        //updateSelectedInfo(SaveDataOld.currentSave.PC.boxes[0][currentPosition - 33]);
                    }
                    yield return StartCoroutine(moveCursor(cursorPositions[currentPosition]));
                }
                else if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    if (currentPosition % 2 == 1)
                    {
                        currentPosition += 1;
                        SfxHandler.Play(selectClip);
                        //updateSelectedInfo(SaveDataOld.currentSave.PC.boxes[0][currentPosition - 33]);
                    }
                    yield return StartCoroutine(moveCursor(cursorPositions[currentPosition]));
                }
                else if (Input.GetAxisRaw("Vertical") < 0)
                {
                    if (currentPosition < 37)
                    {
                        currentPosition += 2;
                        SfxHandler.Play(selectClip);
                        //updateSelectedInfo(SaveDataOld.currentSave.PC.boxes[0][currentPosition - 33]);
                    }
                    else
                    {
                        currentPosition = 41;
                        SfxHandler.Play(selectClip);
                        updateSelectedInfo(null);
                    }
                    yield return StartCoroutine(moveCursor(cursorPositions[currentPosition]));
                }
                else if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    if (currentPosition == 33)
                    {
                        currentPosition = 8;
                        SfxHandler.Play(selectClip);
                        //updateSelectedInfo(SaveDataOld.currentSave.PC.boxes[currentBoxID][currentPosition - 3]);
                    }
                    else if (currentPosition == 35)
                    {
                        currentPosition = 20;
                        SfxHandler.Play(selectClip);
                        //updateSelectedInfo(SaveDataOld.currentSave.PC.boxes[currentBoxID][currentPosition - 3]);
                    }
                    else if (currentPosition == 37)
                    {
                        currentPosition = 26;
                        SfxHandler.Play(selectClip);
                       // updateSelectedInfo(SaveDataOld.currentSave.PC.boxes[currentBoxID][currentPosition - 3]);
                    }
                    else
                    {
                        currentPosition -= 1;
                        SfxHandler.Play(selectClip);
                        //updateSelectedInfo(SaveDataOld.currentSave.PC.boxes[0][currentPosition - 33]);
                    }
                    yield return StartCoroutine(moveCursor(cursorPositions[currentPosition]));
                }
                else if (Input.GetButton("Select"))
                {
                    //if (SaveDataOld.currentSave.PC.boxes[0][currentPosition - 33] != null)
                    //{
                    //    //STANDARD
                    //    if (cursorMode == CursorMode.Standard)
                    //    {
                    //        updateSelectedInfo(SaveDataOld.currentSave.PC.boxes[0][currentPosition - 33]);
                    //        if (!carrying)
                    //        {
                    //            int chosenIndex = 1;
                    //            while (chosenIndex != 0)
                    //            {
                    //                string[] choices = new string[]
                    //                    {"Pick Up", "Summary", "Item", "Deposit", "Release", "Cancel"};
                    //                Dialog.drawDialogBox();
                    //                Dialog.drawTextInstant("What would you like to do?");
                    //                Dialog.drawChoiceBox(choices);
                    //                yield return new WaitForSeconds(0.2f);
                    //                yield return StartCoroutine(Dialog.choiceNavigate(choices));
                    //                Dialog.undrawChoiceBox();
                    //                Dialog.undrawDialogBox();
                    //                chosenIndex = Dialog.chosenIndex;
                    //                if (chosenIndex == 5)
                    //                {
                    //                    //PICK UP
                    //                    if (SaveDataOld.currentSave.PC.boxes[0][1] != null)
                    //                    {
                    //                        //if there is more than one pokemon in the party
                    //                        yield return StartCoroutine(pickUpPokemon(0, currentPosition - 33));
                    //                        chosenIndex = 0;
                    //                    }
                    //                    else
                    //                    {
                    //                        Dialog.drawDialogBox();
                    //                        Dialog.drawTextInstant("That's your last Pokémon!");
                    //                        yield return new WaitForSeconds(0.2f);
                    //                        while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                    //                        {
                    //                            yield return null;
                    //                        }
                    //                    }
                    //                }
                    //                else if (chosenIndex == 4)
                    //                {
                    //                    //SUMMARY
					//
                    //                    SfxHandler.Play(selectClip);
                    //                    //yield return new WaitForSeconds(sceneTransition.FadeOut(0.4f));
                    //                    yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));
					//
                    //                    //Set SceneSummary to be active so that it appears
                    //                    Scene.main.Summary.gameObject.SetActive(true);
                    //                    StartCoroutine(Scene.main.Summary.control(SaveDataOld.currentSave.PC.boxes[0],
                    //                        currentPosition - 33));
                    //                    //Start an empty loop that will only stop when ScenePC is no longer active (is closed)
                    //                    while (Scene.main.Summary.gameObject.activeSelf)
                    //                    {
                    //                        yield return null;
                    //                    }
                    //                    //yield return new WaitForSeconds(sceneTransition.FadeIn(0.4f));
                    //                    yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));
					//
                    //                    chosenIndex = 0;
                    //                }
                    //                else if (chosenIndex == 3)
                    //                {
                    //                    //ITEM
                    //                    PokemonOld currentPokemon = SaveDataOld.currentSave.PC.boxes[0][currentPosition - 33];
					//
                    //                    Dialog.undrawChoiceBox();
                    //                    Dialog.drawDialogBox();
                    //                    if (!string.IsNullOrEmpty(currentPokemon.getHeldItem()))
                    //                    {
                    //                        yield return
                    //                            StartCoroutine(
                    //                                Dialog.drawText(currentPokemon.getName() + " is holding " +
                    //                                                currentPokemon.getHeldItem() + "."));
                    //                        string[] itemChoices = new string[]
                    //                        {
                    //                            "Swap", "Take", "Cancel"
                    //                        };
                    //                        int itemChosenIndex = -1;
                    //                        Dialog.drawChoiceBox(itemChoices);
                    //                        yield return new WaitForSeconds(0.2f);
                    //                        yield return StartCoroutine(Dialog.choiceNavigate(itemChoices));
					//
                    //                        itemChosenIndex = Dialog.chosenIndex;
					//
                    //                        if (itemChosenIndex == 2)
                    //                        {
                    //                            //Swap
                    //                            SfxHandler.Play(selectClip);
                    //                            //yield return new WaitForSeconds(sceneTransition.FadeOut(0.4f));
                    //                            yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));
					//
                    //                            Scene.main.Bag.gameObject.SetActive(true);
                    //                            StartCoroutine(Scene.main.Bag.control(false, true));
                    //                            while (Scene.main.Bag.gameObject.activeSelf)
                    //                            {
                    //                                yield return null;
                    //                            }
					//
                    //                            string chosenItem = Scene.main.Bag.chosenItem;
					//
                    //                            Dialog.undrawChoiceBox();
                    //                            Dialog.undrawDialogBox();
					//
                    //                            //yield return new WaitForSeconds(sceneTransition.FadeIn(0.4f));
                    //                            yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));
					//
                    //                            if (!string.IsNullOrEmpty(chosenItem))
                    //                            {
                    //                                Dialog.drawDialogBox();
                    //                                yield return
                    //                                    StartCoroutine(
                    //                                        Dialog.drawText("Swap " + currentPokemon.getHeldItem() +
                    //                                                        " for " + chosenItem + "?"));
                    //                                Dialog.drawChoiceBox();
                    //                                yield return StartCoroutine(Dialog.choiceNavigate());
					//
                    //                                itemChosenIndex = Dialog.chosenIndex;
                    //                                Dialog.undrawChoiceBox();
					//
                    //                                if (itemChosenIndex == 1)
                    //                                {
                    //                                    string receivedItem = currentPokemon.swapHeldItem(chosenItem);
                    //                                    SaveDataOld.currentSave.Bag.addItem(receivedItem, 1);
                    //                                    SaveDataOld.currentSave.Bag.removeItem(chosenItem, 1);
					//
                    //                                    Dialog.drawDialogBox();
                    //                                    yield return
                    //                                        Dialog.StartCoroutine("drawText",
                    //                                            "Gave " + chosenItem + " to " + currentPokemon.getName() +
                    //                                            ",");
                    //                                    while (!Input.GetButtonDown("Select") &&
                    //                                           !Input.GetButtonDown("Back"))
                    //                                    {
                    //                                        yield return null;
                    //                                    }
                    //                                    Dialog.drawDialogBox();
                    //                                    yield return
                    //                                        Dialog.StartCoroutine("drawText",
                    //                                            "and received " + receivedItem + " in return.");
                    //                                    while (!Input.GetButtonDown("Select") &&
                    //                                           !Input.GetButtonDown("Back"))
                    //                                    {
                    //                                        yield return null;
                    //                                    }
                    //                                }
                    //                            }
                    //                        }
                    //                        else if (itemChosenIndex == 1)
                    //                        {
                    //                            //Take
                    //                            Dialog.undrawChoiceBox();
                    //                            string receivedItem = currentPokemon.swapHeldItem("");
                    //                            SaveDataOld.currentSave.Bag.addItem(receivedItem, 1);
					//
                    //                            //adjust displayed data
                    //                            updateSelectedInfo(currentPokemon);
                    //                            //partyItems[currentPosition - 33].enabled = false;
					//
                    //                            Dialog.drawDialogBox();
                    //                            yield return
                    //                                StartCoroutine(
                    //                                    Dialog.drawText("Took " + receivedItem + " from " +
                    //                                                    currentPokemon.getName() + "."));
                    //                            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                    //                            {
                    //                                yield return null;
                    //                            }
                    //                        }
                    //                    }
                    //                    else
                    //                    {
                    //                        yield return
                    //                            StartCoroutine(
                    //                                Dialog.drawText(currentPokemon.getName() +
                    //                                                " isn't holding anything."));
                    //                        string[] itemChoices = new string[]
                    //                        {
                    //                            "Give", "Cancel"
                    //                        };
                    //                        int itemChosenIndex = -1;
                    //                        Dialog.drawChoiceBox(itemChoices);
                    //                        yield return new WaitForSeconds(0.2f);
                    //                        yield return StartCoroutine(Dialog.choiceNavigate(itemChoices));
                    //                        itemChosenIndex = Dialog.chosenIndex;
					//
                    //                        if (itemChosenIndex == 1)
                    //                        {
                    //                            //Give
                    //                            SfxHandler.Play(selectClip);
                    //                            //yield return new WaitForSeconds(sceneTransition.FadeOut(0.4f));
                    //                            yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));
					//
                    //                            Scene.main.Bag.gameObject.SetActive(true);
                    //                            StartCoroutine(Scene.main.Bag.control(false, true));
                    //                            while (Scene.main.Bag.gameObject.activeSelf)
                    //                            {
                    //                                yield return null;
                    //                            }
					//
                    //                            string chosenItem = Scene.main.Bag.chosenItem;
					//
                    //                            Dialog.undrawChoiceBox();
                    //                            Dialog.undrawDialogBox();
					//
                    //                            //yield return new WaitForSeconds(sceneTransition.FadeIn(0.4f));
                    //                            yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));
					//
                    //                            if (!string.IsNullOrEmpty(chosenItem))
                    //                            {
                    //                                currentPokemon.swapHeldItem(chosenItem);
                    //                                SaveDataOld.currentSave.Bag.removeItem(chosenItem, 1);
					//
                    //                                //adjust displayed data
                    //                                updateSelectedInfo(currentPokemon);
                    //                                //partyItems[currentPosition - 33].enabled = true;
					//
                    //                                Dialog.drawDialogBox();
                    //                                yield return
                    //                                    Dialog.StartCoroutine("drawText",
                    //                                        "Gave " + chosenItem + " to " + currentPokemon.getName() +
                    //                                        ".");
                    //                                while (!Input.GetButtonDown("Select") &&
                    //                                       !Input.GetButtonDown("Back"))
                    //                                {
                    //                                    yield return null;
                    //                                }
                    //                            }
                    //                        }
                    //                    }
                    //                    Dialog.undrawChoiceBox();
                    //                    Dialog.undrawDialogBox();
                    //                    yield return new WaitForSeconds(0.2f);
                    //                    chosenIndex = 0;
                    //                }
                    //                else if (chosenIndex == 2)
                    //                {
                    //                    //DEPOSIT
                    //                    if (SaveDataOld.currentSave.PC.boxes[0][1] != null)
                    //                    {
                    //                        //if there is more than one pokemon in the party
                    //                        int targetPosition = 30;
                    //                        for (int i = 0; i < 30; i++)
                    //                        {
                    //                            if (SaveDataOld.currentSave.PC.boxes[currentBoxID][i] == null)
                    //                            {
                    //                                targetPosition = i;
                    //                                i = 30;
                    //                            }
                    //                        }
                    //                        if (targetPosition >= 30)
                    //                        {
                    //                            //if box is full
                    //                            Dialog.drawDialogBox();
                    //                            Dialog.drawTextInstant("The box is full!");
                    //                            yield return new WaitForSeconds(0.2f);
                    //                            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                    //                            {
                    //                                yield return null;
                    //                            }
                    //                            Dialog.undrawDialogBox();
                    //                        }
                    //                        else
                    //                        {
                    //                            yield return
                    //                                StartCoroutine(depositPokemon(currentPosition - 33, targetPosition))
                    //                                ;
                    //                            updateSelectedInfo(
                    //                                SaveDataOld.currentSave.PC.boxes[0][currentPosition - 33]);
                    //                            chosenIndex = 0;
                    //                        }
                    //                    }
                    //                    else
                    //                    {
                    //                        Dialog.drawDialogBox();
                    //                        Dialog.drawTextInstant("That's your last Pokémon!");
                    //                        yield return new WaitForSeconds(0.2f);
                    //                        while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                    //                        {
                    //                            yield return null;
                    //                        }
                    //                    }
                    //                }
                    //                else if (chosenIndex == 1)
                    //                {
                    //                    //RELEASE
                    //                    if (SaveDataOld.currentSave.PC.boxes[0][1] != null)
                    //                    {
                    //                        //if there is more than one pokemon in the party
                    //                        int releaseIndex = 1;
                    //                        string pokemonName =
                    //                            SaveDataOld.currentSave.PC.boxes[0][currentPosition - 33].getName();
                    //                        while (releaseIndex != 0)
                    //                        {
                    //                            Dialog.drawDialogBox();
                    //                            Dialog.drawTextInstant("Do you want to release " + pokemonName + "?");
                    //                            Dialog.drawChoiceBoxNo();
                    //                            yield return new WaitForSeconds(0.2f);
                    //                            yield return StartCoroutine(Dialog.choiceNavigateNo());
                    //                            Dialog.undrawChoiceBox();
                    //                            releaseIndex = Dialog.chosenIndex;
                    //                            if (releaseIndex == 1)
                    //                            {
                    //                                yield return StartCoroutine(releasePokemon(0, currentPosition - 33))
                    //                                    ;
                    //                                Dialog.drawDialogBox();
                    //                                Dialog.drawTextInstant(pokemonName + " was released.");
                    //                                yield return new WaitForSeconds(0.2f);
                    //                                while (!Input.GetButtonDown("Select") &&
                    //                                       !Input.GetButtonDown("Back"))
                    //                                {
                    //                                    yield return null;
                    //                                }
                    //                                Dialog.drawDialogBox();
                    //                                Dialog.drawTextInstant("Bye bye, " + pokemonName + "!");
                    //                                yield return new WaitForSeconds(0.2f);
                    //                                while (!Input.GetButtonDown("Select") &&
                    //                                       !Input.GetButtonDown("Back"))
                    //                                {
                    //                                    yield return null;
                    //                                }
                    //                                releaseIndex = 0;
                    //                                chosenIndex = 0;
                    //                                updateSelectedInfo(
                    //                                    SaveDataOld.currentSave.PC.boxes[0][currentPosition - 33]);
                    //                                Dialog.undrawDialogBox();
                    //                                StartCoroutine(packParty(currentPosition - 33));
                    //                            }
                    //                        }
                    //                    }
                    //                    else
                    //                    {
                    //                        Dialog.drawDialogBox();
                    //                        Dialog.drawTextInstant("That's your last Pokémon!");
                    //                        yield return new WaitForSeconds(0.2f);
                    //                        while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                    //                        {
                    //                            yield return null;
                    //                        }
                    //                    }
                    //                }
                    //                yield return new WaitForSeconds(0.2f);
                    //            }
                    //        }
                    //        else if (selectedBoxID == 0 && currentPosition - 33 == selectedIndex)
                    //        {
                    //            yield return StartCoroutine(putDownPokemon(0, currentPosition - 33));
                    //        }
                    //        else
                    //        {
                    //            yield return StartCoroutine(switchPokemon(0, currentPosition - 33));
                    //        }
                    //    }
                    //    //HOT MOVE
                    //    else if (cursorMode == CursorMode.HotMove)
                    //    {
                    //        if (!carrying)
                    //        {
                    //            if (SaveDataOld.currentSave.PC.boxes[0][1] != null)
                    //            {
                    //                //if there is more than one pokemon in the party
                    //                yield return StartCoroutine(pickUpPokemon(0, currentPosition - 33));
                    //            }
                    //            else
                    //            {
                    //                Dialog.drawDialogBox();
                    //                Dialog.drawTextInstant("That's your last Pokémon!");
                    //                yield return new WaitForSeconds(0.2f);
                    //                while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back") &&
                    //                       Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
                    //                {
                    //                    yield return null;
                    //                }
                    //                Dialog.undrawDialogBox();
                    //                yield return new WaitForSeconds(0.2f);
                    //            }
                    //        }
                    //        else if (selectedBoxID == 0 && currentPosition - 33 == selectedIndex)
                    //        {
                    //            yield return StartCoroutine(putDownPokemon(0, currentPosition - 33));
                    //        }
                    //        else
                    //        {
                    //            yield return StartCoroutine(switchPokemon(0, currentPosition - 33));
                    //        }
                    //    }
                    //    //WITHDRAW DEPOSIT
                    //    else
                    //    {
                    //        if (SaveDataOld.currentSave.PC.boxes[0][1] != null)
                    //        {
                    //            //if there is more than one pokemon in the party
                    //            int targetPosition = 30;
                    //            for (int i = 0; i < 30; i++)
                    //            {
                    //                if (SaveDataOld.currentSave.PC.boxes[currentBoxID][i] == null)
                    //                {
                    //                    targetPosition = i;
                    //                    i = 30;
                    //                }
                    //            }
                    //            if (targetPosition >= 30)
                    //            {
                    //                //if box is full
                    //                Dialog.drawDialogBox();
                    //                Dialog.drawTextInstant("The box is full!");
                    //                yield return new WaitForSeconds(0.2f);
                    //                while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back") &&
                    //                       Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
                    //                {
                    //                    yield return null;
                    //                }
                    //                Dialog.undrawDialogBox();
                    //                yield return new WaitForSeconds(0.2f);
                    //            }
                    //            else
                    //            {
                    //                yield return StartCoroutine(depositPokemon(currentPosition - 33, targetPosition));
                    //                updateSelectedInfo(SaveDataOld.currentSave.PC.boxes[0][currentPosition - 33]);
                    //            }
                    //        }
                    //        else
                    //        {
                    //            Dialog.drawDialogBox();
                    //            Dialog.drawTextInstant("That's your last Pokémon!");
                    //            yield return new WaitForSeconds(0.2f);
                    //            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back") &&
                    //                   Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
                    //            {
                    //                yield return null;
                    //            }
                    //            Dialog.undrawDialogBox();
                    //            yield return new WaitForSeconds(0.2f);
                    //        }
                    //    }
                    //}
                    //else if (carrying)
                    //{
                    //    yield return StartCoroutine(putDownPokemon(0, currentPosition - 33));
                    //}
                }
                else if (Input.GetButton("Back"))
                {
                    if (carrying)
                    {
                        Dialog.drawDialogBox();
                        Dialog.drawTextInstant("You're holding a Pokémon!");
                        yield return new WaitForSeconds(0.2f);
                        while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                        {
                            yield return null;
                        }
                        Dialog.undrawDialogBox();
                        yield return new WaitForSeconds(0.2f);
                    }
                    else
                    {
                        currentPosition = 41;
                        SfxHandler.Play(selectClip);
                        updateSelectedInfo(null);
                        yield return StartCoroutine(moveCursor(cursorPositions[currentPosition]));

                        Dialog.drawDialogBox();
                        Dialog.drawTextInstant("Continue Box operations?");
                        Dialog.drawChoiceBox();
                        yield return new WaitForSeconds(0.2f);
                        yield return Dialog.StartCoroutine("choiceNavigate");
                        Dialog.undrawChoiceBox();
                        Dialog.undrawDialogBox();
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
                if (Input.GetAxisRaw("Vertical") > 0)
                {
                    if (currentPosition == 39)
                    {
                        currentPosition = 28;
                        SfxHandler.Play(selectClip);
                        //updateSelectedInfo(SaveDataOld.currentSave.PC.boxes[currentBoxID][currentPosition - 3]);
                    }
                    else if (currentPosition == 40)
                    {
                        currentPosition = 32;
                        SfxHandler.Play(selectClip);
                        //updateSelectedInfo(SaveDataOld.currentSave.PC.boxes[currentBoxID][currentPosition - 3]);
                    }
                    else
                    {
                        currentPosition = 38;
                        SfxHandler.Play(selectClip);
                        //updateSelectedInfo(SaveDataOld.currentSave.PC.boxes[0][currentPosition - 33]);
                    }
                    yield return StartCoroutine(moveCursor(cursorPositions[currentPosition]));
                }
                else if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    if (currentPosition < 41)
                    {
                        currentPosition += 1;
                        SfxHandler.Play(selectClip);
                        updateSelectedInfo(null);
                    }
                    yield return StartCoroutine(moveCursor(cursorPositions[currentPosition]));
                }
                else if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    if (currentPosition > 39)
                    {
                        currentPosition -= 1;
                        SfxHandler.Play(selectClip);
                        updateSelectedInfo(null);
                    }
                    yield return StartCoroutine(moveCursor(cursorPositions[currentPosition]));
                }
                else if (Input.GetButton("Select"))
                {
                    if (currentPosition == 40)
                    {
                        SfxHandler.Play(selectClip);
                        if (cursorMode == CursorMode.Standard)
                        {
                            //cursor.color = new Color(0.85f, 0.45f, 0.25f, 0.5f);
                            cursorMode = CursorMode.HotMove;
                        }
                        else if (cursorMode == CursorMode.HotMove)
                        {
                            //cursor.color = new Color(0.75f, 0.25f, 0.9f, 0.5f);
                            cursorMode = CursorMode.WithdrawDeposit;
                        }
                        else
                        {
                            //cursor.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                            cursorMode = CursorMode.Standard;
                        }
                        yield return new WaitForSeconds(0.2f);
                    }
                    else if (currentPosition == 41)
                    {
                        if (carrying)
                        {
                            Dialog.drawDialogBox();
                            Dialog.drawTextInstant("You're holding a Pokémon!");
                            yield return new WaitForSeconds(0.2f);
                            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                            {
                                yield return null;
                            }
                            Dialog.undrawDialogBox();
                            yield return new WaitForSeconds(0.2f);
                        }
                        else
                        {
                            Dialog.drawDialogBox();
                            Dialog.drawTextInstant("Continue Box operations?");
                            Dialog.drawChoiceBox();
                            yield return new WaitForSeconds(0.2f);
                            yield return Dialog.StartCoroutine("choiceNavigate");
                            Dialog.undrawChoiceBox();
                            Dialog.undrawDialogBox();
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
                else if (Input.GetButton("Back"))
                {
                    if (carrying)
                    {
                        Dialog.drawDialogBox();
                        Dialog.drawTextInstant("You're holding a Pokémon!");
                        yield return new WaitForSeconds(0.2f);
                        while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                        {
                            yield return null;
                        }
                        Dialog.undrawDialogBox();
                        yield return new WaitForSeconds(0.2f);
                    }
                    else
                    {
                        currentPosition = 41;
                        SfxHandler.Play(selectClip);
                        updateSelectedInfo(null);
                        yield return StartCoroutine(moveCursor(cursorPositions[currentPosition]));

                        Dialog.drawDialogBox();
                        Dialog.drawTextInstant("Continue Box operations?");
                        Dialog.drawChoiceBox();
                        yield return new WaitForSeconds(0.2f);
                        yield return Dialog.StartCoroutine("choiceNavigate");
                        Dialog.undrawChoiceBox();
                        Dialog.undrawDialogBox();
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
        }
        SfxHandler.Play(offClip);
        //yield return new WaitForSeconds(sceneTransition.FadeOut());
        yield return StartCoroutine(ScreenFade.main.Fade(false, ScreenFade.defaultSpeed));
        GlobalVariables.global.resetFollower();
        yield return new WaitForSeconds(0.4f);
        this.gameObject.SetActive(false);
    }
}