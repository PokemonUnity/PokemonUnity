//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;
using PokemonUnity.Monster;

public class BagHandler : MonoBehaviour
{
    private DialogBoxHandler Dialog;

    public string chosenItem;

    private Transform party;

    private GUITexture[] partySlot = new GUITexture[6];
    private GUITexture[] partyIcon = new GUITexture[6];
    private GUIText[] partyName = new GUIText[6];
    private GUIText[] partyNameShadow = new GUIText[6];
    private GUIText[] partyGender = new GUIText[6];
    private GUIText[] partyGenderShadow = new GUIText[6];
    private GameObject[] partyStandardDisplay = new GameObject[6];
    private GUITexture[] partyHPBarBack = new GUITexture[6];
    private GUITexture[] partyHPBar = new GUITexture[6];
    private GUITexture[] partyLv = new GUITexture[6];
    private GUIText[] partyLevel = new GUIText[6];
    private GUIText[] partyLevelShadow = new GUIText[6];
    private GUIText[] partyTextDisplay = new GUIText[6];
    private GUIText[] partyTextDisplayShadow = new GUIText[6];
    private GUITexture[] partyStatus = new GUITexture[6];
    private GUITexture[] partyItem = new GUITexture[6];

    private GUITexture scrollBar;

    private Transform itemList;

    private GUITexture[] itemSlot = new GUITexture[8];
    private GUIText[] itemName = new GUIText[8];
    private GUIText[] itemNameShadow = new GUIText[8];
    private GUITexture[] itemIcon = new GUITexture[8];
    private GUIText[] itemX = new GUIText[8];
    private GUIText[] itemXShadow = new GUIText[8];
    private GUIText[] itemQuantity = new GUIText[8];
    private GUIText[] itemQuantityShadow = new GUIText[8];

    private GUIText itemDescription;
    private GUIText itemDescriptionShadow;

    private GameObject[] screens = new GameObject[6];
    private GameObject[] shopScreens = new GameObject[6];

    private GameObject numbersBox;
    private GUITexture numbersBoxBorder;
    private GUIText numbersBoxText;
    private GUIText numbersBoxTextShadow;
    private GUIText numbersBoxSelector;
    private GUIText numbersBoxSelectorShadow;

    private GUIText shopName;
    private GUIText shopNameShadow;

    private GameObject moneyBox;
    private GUITexture moneyBoxBorder;
    private GUIText moneyValueText;
    private GUIText moneyValueTextShadow;

    private GameObject dataBox;
    private GUITexture dataBoxBorder;
    private GUIText dataText;
    private GUIText dataTextShadow;
    private GUIText dataValueText;
    private GUIText dataValueTextShadow;

    private GUITexture tmType;
    private GUITexture tmCategory;
    private GUIText tmPower;
    private GUIText tmPowerShadow;
    private GUIText tmAccuracy;
    private GUIText tmAccuracyShadow;
    private GUIText tmDescription;
    private GUIText tmDescriptionShadow;

    public Texture itemListTex;
    public Texture itemListHighlightTex;
    public Texture itemListPlaceTex;
    public Texture itemListSelectedTex;
    public Texture itemListPlaceSelectedTex;

    public Texture partySlotTex;
    public Texture partySlotSelectedTex;

    public int currentScreen = 1;

    public int[] currentPosition = new int[]
    {
        -1, 1, 1, 1, 1, 1
    };

    public int[] currentTopPosition = new int[]
    {
        -1, 0, 0, 0, 0, 0
    };

    private int visableSlots = 6;

    public int unselectedItemIconX = 138;
    public int unselectedItemNameX = 159;

    public bool switching = false;
    public int selected = -1;

    private int selectedPosition = 0;
    private int selectedTopPosition = 0;

    public bool inParty = false;
    public int partyPosition = 0;
    public int partyLength;

    private string[] currentItemList;

    private bool shopMode;
    private string[] shopItemList;
    private int shopSelectedQuantity;

    private AudioSource BagAudio;

    public AudioClip selectClip;
    public AudioClip healClip;
    public AudioClip tmBootupClip;
    public AudioClip forgetMoveClip;
    public AudioClip saleClip;

    void Awake()
    {
        Dialog = transform.GetComponent<DialogBoxHandler>();
        BagAudio = transform.GetComponent<AudioSource>();

        party = transform.Find("Party");
        for (int i = 0; i < 6; i++)
        {
            partySlot[i] = party.Find("Slot" + i).GetComponent<GUITexture>();
            partyIcon[i] = partySlot[i].transform.Find("Icon").GetComponent<GUITexture>();
            partyName[i] = partySlot[i].transform.Find("Name").GetComponent<GUIText>();
            partyNameShadow[i] = partyName[i].transform.Find("NameShadow").GetComponent<GUIText>();
            partyGender[i] = partySlot[i].transform.Find("Gender").GetComponent<GUIText>();
            partyGenderShadow[i] = partyGender[i].transform.Find("GenderShadow").GetComponent<GUIText>();
            partyStandardDisplay[i] = partySlot[i].transform.Find("StandardDisplay").gameObject;
            partyHPBarBack[i] = partyStandardDisplay[i].transform.Find("HPBarBack").GetComponent<GUITexture>();
            partyHPBar[i] = partyStandardDisplay[i].transform.Find("HPBar").GetComponent<GUITexture>();
            partyLv[i] = partyStandardDisplay[i].transform.Find("Lv").GetComponent<GUITexture>();
            partyLevel[i] = partyStandardDisplay[i].transform.Find("Level").GetComponent<GUIText>();
            partyLevelShadow[i] = partyLevel[i].transform.Find("LevelShadow").GetComponent<GUIText>();
            partyTextDisplay[i] = partySlot[i].transform.Find("TextDisplay").GetComponent<GUIText>();
            partyTextDisplayShadow[i] =
                partyTextDisplay[i].transform.Find("TextDisplayShadow").GetComponent<GUIText>();
            partyStatus[i] = partySlot[i].transform.Find("Status").GetComponent<GUITexture>();
            partyItem[i] = partySlot[i].transform.Find("Item").GetComponent<GUITexture>();
        }
        scrollBar = transform.Find("ScrollBar").GetComponent<GUITexture>();

        itemList = transform.Find("ItemList");
        for (int i = 0; i < 8; i++)
        {
            itemSlot[i] = itemList.Find("Item" + i).GetComponent<GUITexture>();
            itemName[i] = itemSlot[i].transform.Find("Name").GetComponent<GUIText>();
            itemNameShadow[i] = itemSlot[i].transform.Find("NameShadow").GetComponent<GUIText>();
            itemIcon[i] = itemSlot[i].transform.Find("Icon").GetComponent<GUITexture>();
            itemX[i] = itemSlot[i].transform.Find("x").GetComponent<GUIText>();
            itemXShadow[i] = itemX[i].transform.Find("xShadow").GetComponent<GUIText>();
            itemQuantity[i] = itemSlot[i].transform.Find("Quantity").GetComponent<GUIText>();
            itemQuantityShadow[i] = itemSlot[i].transform.Find("QuantityShadow").GetComponent<GUIText>();
        }

        itemDescription = transform.Find("ItemDescription").GetComponent<GUIText>();
        itemDescriptionShadow = itemDescription.transform.Find("ItemDescriptionShadow").GetComponent<GUIText>();

        for (int i = 1; i < 6; i++)
        {
            screens[i] = transform.Find("bag" + i).gameObject;
            shopScreens[i] = transform.Find("shop" + i).gameObject;
        }

        numbersBox = transform.Find("NumbersBox").gameObject;
        numbersBoxBorder = numbersBox.transform.GetComponent<GUITexture>();
        numbersBoxText = numbersBox.transform.Find("NumberText").GetComponent<GUIText>();
        numbersBoxTextShadow = numbersBox.transform.Find("NumberTextShadow").GetComponent<GUIText>();
        numbersBoxSelector = numbersBox.transform.Find("Selector").GetComponent<GUIText>();
        numbersBoxSelectorShadow = numbersBox.transform.Find("SelectorShadow").GetComponent<GUIText>();

        shopName = transform.Find("ShopName").GetComponent<GUIText>();
        shopNameShadow = shopName.transform.Find("ShopNameShadow").GetComponent<GUIText>();

        moneyBox = transform.Find("MoneyBox").gameObject;
        moneyBoxBorder = moneyBox.transform.Find("BoxBorder").GetComponent<GUITexture>();
        moneyValueText = moneyBox.transform.Find("Money").GetComponent<GUIText>();
        moneyValueTextShadow = moneyBox.transform.Find("MoneyShadow").GetComponent<GUIText>();

        dataBox = transform.Find("DataBox").gameObject;
        dataBoxBorder = dataBox.transform.Find("BoxBorder").GetComponent<GUITexture>();
        dataText = dataBox.transform.Find("DataText").GetComponent<GUIText>();
        dataTextShadow = dataBox.transform.Find("DataTextShadow").GetComponent<GUIText>();
        dataValueText = dataBox.transform.Find("Data").GetComponent<GUIText>();
        dataValueTextShadow = dataBox.transform.Find("DataShadow").GetComponent<GUIText>();

        tmType = screens[4].transform.Find("TMType").GetComponent<GUITexture>();
        tmCategory = screens[4].transform.Find("TMCategory").GetComponent<GUITexture>();
        tmPower = screens[4].transform.Find("TMPower").GetComponent<GUIText>();
        tmPowerShadow = tmPower.transform.Find("TMPowerShadow").GetComponent<GUIText>();
        tmAccuracy = screens[4].transform.Find("TMAccuracy").GetComponent<GUIText>();
        tmAccuracyShadow = tmAccuracy.transform.Find("TMAccuracyShadow").GetComponent<GUIText>();
        tmDescription = screens[4].transform.Find("TMDescription").GetComponent<GUIText>();
        tmDescriptionShadow = tmDescription.transform.Find("TMDescriptionShadow").GetComponent<GUIText>();
    }

    void Start()
    {
        this.gameObject.SetActive(false);
    }


    private void updateScrollBar()
    {
        float min = 18;
        float topY = 141;
        float itemsLength = currentItemList.Length;
        float maxHeight = 78;
        float maxView = visableSlots;
        if (currentScreen == 4 && !shopMode)
        {
            maxHeight = 62;
        }
        float barHeight = maxHeight / itemsLength * maxView;
        if (barHeight > maxHeight)
        {
            barHeight = maxHeight;
        }
        else if (barHeight < min)
        {
            barHeight = min;
        }
        float heightOffset = maxHeight - barHeight;
        if (itemsLength != maxView)
        {
            //to prevent dividing by 0.
            heightOffset -= ((maxHeight - barHeight) / (itemsLength - maxView)) * (currentTopPosition[currentScreen]);
        }
        if (heightOffset < 0)
        {
            heightOffset = 0;
        }
        scrollBar.pixelInset = new Rect(scrollBar.pixelInset.x, topY - maxHeight + heightOffset,
            scrollBar.pixelInset.width, barHeight);
    }

    private void updateSelectedItem()
    {
        for (int i = 0; i < 8; i++)
        {
            if (currentPosition[currentScreen] == i)
            {
                if (switching)
                {
                    if (i == selected - currentTopPosition[currentScreen] + 1)
                    {
                        itemSlot[i].texture = itemListPlaceSelectedTex;
                    }
                    else
                    {
                        itemSlot[i].texture = itemListPlaceTex;
                    }
                }
                else
                {
                    itemSlot[i].texture = itemListHighlightTex;
                }
                itemIcon[i].pixelInset = new Rect(unselectedItemIconX - 4, itemSlot[i].pixelInset.y - 4, 24, 24);
                itemIcon[i].transform.localPosition = new Vector3(0, 0, 21.5f);
                itemName[i].pixelOffset = new Vector2(unselectedItemNameX + 6, itemName[i].pixelOffset.y);
                itemNameShadow[i].pixelOffset = new Vector2(unselectedItemNameX + 7, itemNameShadow[i].pixelOffset.y);
            }
            else
            {
                if (switching)
                {
                    if (i == selected - currentTopPosition[currentScreen] + 1)
                    {
                        itemSlot[i].texture = itemListSelectedTex;
                    }
                    else
                    {
                        itemSlot[i].texture = itemListTex;
                    }
                }
                else
                {
                    itemSlot[i].texture = itemListTex;
                }
                itemIcon[i].pixelInset = new Rect(unselectedItemIconX, itemSlot[i].pixelInset.y, 16, 16);
                itemIcon[i].transform.localPosition = new Vector3(0, 0, 20.5f);
                itemName[i].pixelOffset = new Vector2(unselectedItemNameX, itemName[i].pixelOffset.y);
                itemNameShadow[i].pixelOffset = new Vector2(unselectedItemNameX + 1, itemNameShadow[i].pixelOffset.y);
            }
        }
    }

    private void updateItemList()
    {
        string[] items = new string[8];

        int index = 0;
        for (int i = 0; i < 8; i++)
        {
            index = i + currentTopPosition[currentScreen] - 1;
            if (index < 0 || index >= currentItemList.Length)
            {
                items[i] = null;
            }
            else
            {
                items[i] = currentItemList[index];
            }
        }

        for (int i = 0; i < 8; i++)
        {
            if (items[i] == null)
            {
                itemSlot[i].gameObject.SetActive(false);
            }
            else
            {
                PokemonUnity.Inventory.ItemData item = PokemonUnity.Game.ItemData[items[i].ToItems()];
                itemSlot[i].gameObject.SetActive(true);
                itemName[i].text = items[i];
                itemNameShadow[i].text = itemName[i].text;
                if (item.Pocket.Value == PokemonUnity.Inventory.ItemPockets.MACHINE)
                {
                    Debug.Log("Error: No TM, YET");
                    //itemIcon[i].texture =
                    //    Resources.Load<Texture>("Items/tm" + MoveDatabase.getMoveType(((TMData)item).MoveID).ToString());
                }
                else
                {
                    itemIcon[i].texture = Resources.Load<Texture>("Items/" + items[i]);
                }
                if (item.Pocket.Value == PokemonUnity.Inventory.ItemPockets.MACHINE)
                {
                    itemX[i].gameObject.SetActive(false);
                    //itemQuantity[i].text = "No. " + ((TMData)item).getTMNo;
                }
                else if (item.Pocket.Value == PokemonUnity.Inventory.ItemPockets.KEY)
                {
                    itemX[i].gameObject.SetActive(false);
                    itemQuantity[i].text = "";
                }
                else
                {
                    itemX[i].gameObject.SetActive(true);
                    if (shopMode && currentScreen == 1)
                    {
                        itemX[i].text = "$";
                        itemQuantity[i].text = "" + item.Price;
                    }
                    else
                    {
                        itemX[i].text = "   x";
                        itemQuantity[i].text = "" + SaveData.currentSave.Bag.GetItemAmount(items[i].ToItems());
                    }
                    itemXShadow[i].text = itemX[i].text;
                }
                itemQuantityShadow[i].text = itemQuantity[i].text;
            }
        }
    }

    private void updateDescription()
    {
        //resolve the index of the current item by adding the top and visible positions, minus 1.
        int index = currentPosition[currentScreen] + currentTopPosition[currentScreen] - 1;
        if (index < currentItemList.Length)
        {
            string selectedItem = currentItemList[index];
            if (currentScreen != 4)
            {
                itemDescription.text = ItemDatabase.getItem(selectedItem.ToItems()).Description;
                itemDescriptionShadow.text = itemDescription.text;
            }
            else if (!shopMode) // ?
            {
                PokemonUnity.Attack.Move selectedTM = new PokemonUnity.Attack.Move(selectedItem.ToMoves());
                tmType.texture = Resources.Load<Texture>("PCSprites/type" + selectedTM.Type.ToString());
                tmCategory.texture = Resources.Load<Texture>("PCSprites/category" + selectedTM.Category.ToString());
                tmPower.text = "" + selectedTM.Power;
                if (tmPower.text == "0")
                {
                    tmPower.text = "-";
                }
                tmPowerShadow.text = tmPower.text;
                tmAccuracy.text = "" + Mathf.Round(selectedTM.Accuracy.GetValueOrDefault());
                if (tmAccuracy.text == "0")
                {
                    tmAccuracy.text = "-";
                }
                tmAccuracyShadow.text = tmAccuracy.text;
                tmDescription.text = ItemDatabase.getItem(selectedItem.ToItems()).Description;
                tmDescriptionShadow.text = tmDescription.text;
            }
        }
        else
        {
            if (currentScreen != 4)
            {
                itemDescription.text = "";
                itemDescriptionShadow.text = itemDescription.text;
            }
            else
            {
                tmDescription.text = "";
                tmDescriptionShadow.text = tmDescription.text;
            }
        }
    }

    private void updateScreen()
    {
        if (currentScreen == 4 && !shopMode)
        {
            itemDescription.gameObject.SetActive(false);
            visableSlots = 5;
            currentItemList = SaveData.currentSave.Bag.getItemTypeArray(PokemonUnity.Inventory.ItemPockets.MACHINE, false);
        }
        else
        {
            itemDescription.gameObject.SetActive(true);
            visableSlots = 6;
            if (!shopMode)
            {
                if (currentScreen == 1)
                {
                    currentItemList = SaveData.currentSave.Bag.getItemTypeArray(PokemonUnity.Inventory.ItemPockets.MISC, false);
                }
                else if (currentScreen == 2)
                {
                    currentItemList = SaveData.currentSave.Bag.getItemTypeArray(PokemonUnity.Inventory.ItemPockets.MEDICINE, false);
                }
                else if (currentScreen == 3)
                {
                    currentItemList = SaveData.currentSave.Bag.getItemTypeArray(PokemonUnity.Inventory.ItemPockets.BERRY, false);
                }
                else if (currentScreen == 5)
                {
                    currentItemList = SaveData.currentSave.Bag.getItemTypeArray(PokemonUnity.Inventory.ItemPockets.KEY, false);
                }
            }
            else // Shop
            {
                if (currentScreen == 1)
                {
                    currentItemList = shopItemList;
                }
                else if (currentScreen == 2)
                {
                    currentItemList = SaveData.currentSave.Bag.getItemTypeArray(PokemonUnity.Inventory.ItemPockets.MISC, true);
                }
                else if (currentScreen == 3)
                {
                    currentItemList = SaveData.currentSave.Bag.getItemTypeArray(PokemonUnity.Inventory.ItemPockets.MISC, false);
                }
                else if (currentScreen == 4)
                {
                    currentItemList = SaveData.currentSave.Bag.getItemTypeArray(PokemonUnity.Inventory.ItemPockets.MEDICINE, false);
                }
                else if (currentScreen == 5)
                {
                    currentItemList = SaveData.currentSave.Bag.getItemTypeArray(PokemonUnity.Inventory.ItemPockets.BERRY, false);
                }
            }
        }
        if (!shopMode)
        {
            screens[currentScreen].SetActive(true);
            shopScreens[currentScreen].SetActive(false);
        }
        else
        {
            shopScreens[currentScreen].SetActive(true);
            screens[currentScreen].SetActive(false);
        }
        for (int i = 1; i < 6; i++)
        {
            if (i != currentScreen)
            {
                screens[i].SetActive(false);
                shopScreens[i].SetActive(false);
            }
        }
    }

    private IEnumerator scrollScrollBar(int direction)
    {
        float maxBarHeight = 78f;
        if (currentScreen == 4 && !shopMode)
        {
            maxBarHeight = 62;
        }
        float barInterval = ((maxBarHeight - scrollBar.pixelInset.height) / (currentItemList.Length - visableSlots));

        float increment = 0f;
        float speed = 0.16f;

        float barYStart = scrollBar.pixelInset.y;

        if (direction > 0)
        {
            //down one
            while (increment < 1)
            {
                increment += (1 / speed) * Time.deltaTime;
                if (increment > 1)
                {
                    increment = 1;
                }
                scrollBar.pixelInset = new Rect(scrollBar.pixelInset.x, barYStart - (barInterval * increment),
                    scrollBar.pixelInset.width, scrollBar.pixelInset.height);
                itemList.position = new Vector3(0, (0.083f * increment), 0);
                yield return null;
            }
        }
        else
        {
            //up one
            while (increment < 1)
            {
                increment += (1 / speed) * Time.deltaTime;
                if (increment > 1)
                {
                    increment = 1;
                }
                scrollBar.pixelInset = new Rect(scrollBar.pixelInset.x, barYStart + (barInterval * increment),
                    scrollBar.pixelInset.width, scrollBar.pixelInset.height);
                itemList.position = new Vector3(0, (-0.083f * increment), 0);
                yield return null;
            }
        }

        itemList.position = new Vector3(0, 0, 0);
        updateItemList();
    }

    private IEnumerator animateSelection(int selectionPosition, int deselectionPosition)
    {
        float increment = 0f;
        float speed = 0.16f;

        GUITexture previousIcon = itemIcon[deselectionPosition];
        GUITexture newIcon = itemIcon[selectionPosition];

        Rect previousIconStart = previousIcon.pixelInset;
        Rect newIconStart = newIcon.pixelInset;

        GUIText previousItemName = itemName[deselectionPosition];
        GUIText previousItemNameShadow = itemNameShadow[deselectionPosition];
        GUIText newItemName = itemName[selectionPosition];
        GUIText newItemNameShadow = itemNameShadow[selectionPosition];

        float previousItemNameXStart = previousItemName.pixelOffset.x;
        float newItemNameXStart = newItemName.pixelOffset.x;

        newIcon.transform.localPosition = new Vector3(0, 0, 21.5f);
        previousIcon.transform.localPosition = new Vector3(0, 0, 20.5f);

        bool updatedListBackgrounds = false;
        int currentTopPositionOnStart = currentTopPosition[currentScreen];

        while (increment < 1)
        {
            increment += (1 / speed) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }

            previousIcon.pixelInset = new Rect(previousIconStart.x + (4f * increment),
                previousIconStart.y + (4f * increment), previousIconStart.width - (8f * increment),
                previousIconStart.height - (8f * increment));
            newIcon.pixelInset = new Rect(newIconStart.x - (4f * increment), newIconStart.y - (4f * increment),
                newIconStart.width + (8f * increment), newIconStart.height + (8f * increment));

            previousItemName.pixelOffset = new Vector2(previousItemNameXStart - (6f * increment),
                previousItemName.pixelOffset.y);
            newItemName.pixelOffset = new Vector2(newItemNameXStart + (6f * increment), newItemName.pixelOffset.y);
            previousItemNameShadow.pixelOffset = new Vector2(previousItemNameXStart + 1 - (6f * increment),
                previousItemNameShadow.pixelOffset.y);
            newItemNameShadow.pixelOffset = new Vector2(newItemNameXStart + 1 + (6f * increment),
                newItemNameShadow.pixelOffset.y);

            if (increment >= 0.5f && !updatedListBackgrounds)
            {
                updatedListBackgrounds = true;
                if (switching)
                {
                    if (selectionPosition + currentTopPositionOnStart - 1 == selected)
                    {
                        itemSlot[selectionPosition].texture = itemListPlaceSelectedTex;
                    }
                    else
                    {
                        itemSlot[selectionPosition].texture = itemListPlaceTex;
                    }
                    if (deselectionPosition + currentTopPositionOnStart - 1 == selected)
                    {
                        itemSlot[deselectionPosition].texture = itemListSelectedTex;
                    }
                    else
                    {
                        itemSlot[deselectionPosition].texture = itemListTex;
                    }
                }
                else
                {
                    itemSlot[deselectionPosition].texture = itemListTex;
                    itemSlot[selectionPosition].texture = itemListHighlightTex;
                }
            }

            yield return null;
        }

        updateSelectedItem();
    }

    private void updateParty()
    {
        Pokemon currentPokemon;
        for (int i = 0; i < 6; i++)
        {
            //currentPokemon = SaveData.currentSave.PC.boxes[0][i];
            currentPokemon = SaveData.currentSave.Player.Party[i];
            if (currentPokemon == null || (int)currentPokemon.Species == 0)
            {
                partySlot[i].gameObject.SetActive(false);
            }
            else
            {
                partyIcon[i].texture = currentPokemon.GetIcons();

                partyHPBar[i].pixelInset = new Rect(partyHPBar[i].pixelInset.x, partyHPBar[i].pixelInset.y,
                    Mathf.FloorToInt(48f * ((float) currentPokemon.HP / (float) currentPokemon.TotalHP)),
                    partyHPBar[i].pixelInset.height);

                if ((float) currentPokemon.HP < ((float) currentPokemon.TotalHP / 4f))
                {
                    partyHPBar[i].color = new Color(1, 0.125f, 0, 1);
                }
                else if ((float) currentPokemon.HP < ((float) currentPokemon.TotalHP / 2f))
                {
                    partyHPBar[i].color = new Color(1, 0.75f, 0, 1);
                }
                else
                {
                    partyHPBar[i].color = new Color(0.125f, 1, 0.065f, 1);
                }

                partyName[i].text = currentPokemon.Name;
                partyNameShadow[i].text = partyName[i].text;
                if (currentPokemon.IsGenderless)
                {
                    partyGender[i].text = null;
                }
                else if (!currentPokemon.IsMale)
                {
                    partyGender[i].text = "♀";
                    partyGender[i].color = new Color(1, 0.2f, 0.2f, 1);
                }
                else if (currentPokemon.IsMale)
                {
                    partyGender[i].text = "♂";
                    partyGender[i].color = new Color(0.2f, 0.4f, 1, 1);
                }

                partyGenderShadow[i].text = partyGender[i].text;
                partyLevel[i].text = "" + currentPokemon.Level;
                partyLevelShadow[i].text = partyLevel[i].text;
                if (currentPokemon.Status != PokemonUnity.Status.NONE)
                {
                    partyStatus[i].texture =
                        Resources.Load<Texture>("PCSprites/status" + currentPokemon.Status.ToString());
                }
                else
                {
                    partyStatus[i].texture = null;
                }
                if (currentPokemon.hasItem())
                {
                    partyItem[i].enabled = true;
                }
                else
                {
                    partyItem[i].enabled = false;
                }
            }
        }
        //partyLength = SaveData.currentSave.PC.getBoxLength(0);
        partyLength = SaveData.currentSave.Player.Party.GetCount();
    }

    // Maybe I need to redo my ItemData
    private void updatePartyDisplays()
    {
        if (currentItemList.Length > 0)
        {
            int loopBreaker = 200;
            while (currentPosition[currentScreen] + currentTopPosition[currentScreen] - 1 >= currentItemList.Length &&
                   loopBreaker > 0)
            {
                Debug.Log("" + currentItemList.Length + " " + currentPosition[currentScreen] + " " +
                          (currentTopPosition[currentScreen] - 1));
                if (currentPosition[currentScreen] > 1)
                {
                    if (currentItemList.Length < visableSlots)
                    {
                        currentPosition[currentScreen] -= 1;
                        currentTopPosition[currentScreen] = 0;
                    }
                    else
                    {
                        currentTopPosition[currentScreen] -= 1;
                    }
                }

                loopBreaker -= 1;
                if (loopBreaker == 0)
                {
                    Debug.Log("LOOPBREAKER");
                }
                updateItemList();
                updateSelectedItem();
                updateScrollBar();
                updateDescription();
            }

            PokemonUnity.Inventory.ItemData selectedItem =
                ItemDatabase.getItem(
                    currentItemList[currentPosition[currentScreen] + currentTopPosition[currentScreen] - 1].ToItems());

            if (selectedItem.Category == PokemonUnity.Inventory.ItemCategory.EVOLUTION)
            {
                for (int i = 0; i < 6; i++)
                {
                    partyStandardDisplay[i].SetActive(false);
                    partyTextDisplay[i].gameObject.SetActive(true);
                    if (SaveData.currentSave.Player.Party[i] != null)
                    {
                        if (SaveData.currentSave.Player.Party[i].getEvolutionID(EvolutionMethod.Item, selectedItem.Name.ToItems()) != -1)
                        {
                            partyTextDisplay[i].text = "ABLE!";
                            partyTextDisplay[i].color = new Color(0, 0.5f, 1, 1);
                        }
                        else
                        {
                            partyTextDisplay[i].text = "UNABLE!";
                            partyTextDisplay[i].color = new Color(1, 0.25f, 0, 1);
                        }
                        partyTextDisplayShadow[i].text = partyTextDisplay[i].text;
                    }
                }
            }
            // Need to add Machines support
            else if (selectedItem.Category == PokemonUnity.Inventory.ItemCategory.ALL_MACHINES)
            {
                //TMData SelectedTMItem = (TMData)selectedItem;
                //for (int i = 0; i < 6; i++)
                //{
                //    partyStandardDisplay[i].SetActive(false);
                //    partyTextDisplay[i].gameObject.SetActive(true);
                //    if (SaveData.currentSave.Player.Party[i] != null)
                //    {
                //        if (SaveData.currentSave.Player.Party[i].hasMove(SelectedTMItem.MoveID))
                //        {
                //            partyTextDisplay[i].text = "LEARNED!";
                //            partyTextDisplay[i].color = new Color(1, 1, 1, 1);
                //        }
                //        else if (SaveData.currentSave.Player.Party[i].CanLearnMove(SelectedTMItem.MoveID, LearnMethod.machine))
                //        {
                //            partyTextDisplay[i].text = "ABLE!";
                //            partyTextDisplay[i].color = new Color(0, 0.5f, 1, 1);
                //        }
                //        else
                //        {
                //            partyTextDisplay[i].text = "UNABLE!";
                //            partyTextDisplay[i].color = new Color(1, 0.25f, 0, 1);
                //        }
                //        partyTextDisplayShadow[i].text = partyTextDisplay[i].text;
                //    }
                //}
            }
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    partyStandardDisplay[i].SetActive(true);
                    partyTextDisplay[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            currentPosition[currentScreen] = 1;
            currentTopPosition[currentScreen] = 0;
            updateDescription();

            for (int i = 0; i < 6; i++)
            {
                partyStandardDisplay[i].SetActive(true);
                partyTextDisplay[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < 6; i++)
        {
            if (i != partyPosition)
            {
                partySlot[i].texture = partySlotTex;
            }
            else if (inParty)
            {
                partySlot[i].texture = partySlotSelectedTex;
            }
        }
    }

    private void updateMoneyBox()
    {
        if (shopMode)
        {
            moneyBox.SetActive(true);
            moneyBoxBorder.texture = Resources.Load<Texture>("Frame/dialog" + PlayerPrefs.GetInt("frameStyle"));
            string playerMoney = "" + SaveData.currentSave.Player.Money;
            char[] playerMoneyChars = playerMoney.ToCharArray();
            playerMoney = "";
            //format playerMoney into a currency style (e.g. $1,000,000)
            for (int i = 0; i < playerMoneyChars.Length; i++)
            {
                playerMoney = playerMoneyChars[playerMoneyChars.Length - 1 - i] + playerMoney;
                if ((i + 1) % 3 == 0 && i != playerMoneyChars.Length - 1)
                {
                    playerMoney = "," + playerMoney;
                }
            }
            moneyValueText.text = "$" + playerMoney;
            moneyValueTextShadow.text = moneyValueText.text;
        }
        else
        {
            moneyBox.SetActive(false);
        }
    }

    private void updateDataBox()
    {
        if (shopMode)
        {
            dataBox.SetActive(true);
            dataBoxBorder.texture = Resources.Load<Texture>("Frame/dialog" + PlayerPrefs.GetInt("frameStyle"));
            if (currentScreen == 1)
            {
                dataText.text = "In Bag:";
                if (currentItemList.Length > 0)
                {
                    dataValueText.text = "x " +
                                         SaveData.currentSave.Bag.GetItemAmount(
                                             currentItemList[
                                                 currentPosition[currentScreen] + currentTopPosition[currentScreen] - 1].ToItems());
                }
                else
                {
                    dataValueText.text = "-";
                }
            }
            else
            {
                dataText.text = "Offer:";
                if (currentItemList.Length > 0)
                {
                    dataValueText.text = "$" +
                                         Mathf.Floor(
                                             (float)
                                             ItemDatabase.getItem(
                                                 currentItemList[
                                                     currentPosition[currentScreen] + currentTopPosition[currentScreen] -
                                                     1].ToItems()).Price / 2f);
                }
                else
                {
                    dataValueText.text = "-";
                }
            }
            dataTextShadow.text = dataText.text;
            dataValueTextShadow.text = dataValueText.text;
        }
        else
        {
            dataBox.SetActive(false);
        }
    }

    private bool addItem(PokemonUnity.Inventory.Items item, int amount)
    {
        string itemName = item.toString();
        bool result = SaveData.currentSave.Bag.addItem(itemName.ToItems(), amount);
        if (result)
        {
            PokemonUnity.Inventory.ItemPockets itemType = ItemDatabase.getItem(itemName.ToItems()).Pocket.Value;
            bool relevantScreen = false;
            if (currentScreen == 1 && itemType == PokemonUnity.Inventory.ItemPockets.MISC)
            {
                relevantScreen = true;
            }
            else if (currentScreen == 2 && itemType == PokemonUnity.Inventory.ItemPockets.MEDICINE)
            {
                relevantScreen = true;
            }
            else if (currentScreen == 3 && itemType == PokemonUnity.Inventory.ItemPockets.BERRY)
            {
                relevantScreen = true;
            }
            else if (currentScreen == 4 && itemType == PokemonUnity.Inventory.ItemPockets.MACHINE)
            {
                relevantScreen = true;
            }
            else if (currentScreen == 5 && itemType == PokemonUnity.Inventory.ItemPockets.KEY)
            {
                relevantScreen = true;
            }
            if (SaveData.currentSave.Bag.GetItemAmount(itemName.ToItems()) == amount && relevantScreen)
            {
                if (currentPosition[currentScreen] > Mathf.Floor(visableSlots * 0.67f))
                {
                    currentTopPosition[currentScreen] += 1;
                    currentPosition[currentScreen] -= 1;
                }
            }
        }
        return result;
    }

    private bool addItem(string itemName, int amount)
    {
        bool result = SaveData.currentSave.Bag.addItem(itemName.ToItems(), amount);
        if (result)
        {
            PokemonUnity.Inventory.ItemPockets itemType = ItemDatabase.getItem(itemName.ToItems()).Pocket.Value;
            bool relevantScreen = false;
            if (currentScreen == 1 && itemType == PokemonUnity.Inventory.ItemPockets.MISC)
            {
                relevantScreen = true;
            }
            else if (currentScreen == 2 && itemType == PokemonUnity.Inventory.ItemPockets.MEDICINE)
            {
                relevantScreen = true;
            }
            else if (currentScreen == 3 && itemType == PokemonUnity.Inventory.ItemPockets.BERRY)
            {
                relevantScreen = true;
            }
            else if (currentScreen == 4 && itemType == PokemonUnity.Inventory.ItemPockets.MACHINE)
            {
                relevantScreen = true;
            }
            else if (currentScreen == 5 && itemType == PokemonUnity.Inventory.ItemPockets.KEY)
            {
                relevantScreen = true;
            }
            if (SaveData.currentSave.Bag.GetItemAmount(itemName.ToItems()) == amount && relevantScreen)
            {
                if (currentPosition[currentScreen] > Mathf.Floor(visableSlots * 0.67f))
                {
                    currentTopPosition[currentScreen] += 1;
                    currentPosition[currentScreen] -= 1;
                }
            }
        }
        return result;
    }

    private bool removeItem(PokemonUnity.Inventory.Items itemName, int amount)
    {
        bool result = SaveData.currentSave.Bag.removeItem(itemName, amount);
        if (result)
        {
            if (SaveData.currentSave.Bag.GetItemAmount(itemName) == 0)
            {
                switching = false;
                selected = -1;

                updateScreen();

                if (visableSlots + currentTopPosition[currentScreen] + 1 > currentItemList.Length)
                {
                    if (currentPosition[currentScreen] > 1)
                    {
                        if (currentItemList.Length < visableSlots)
                        {
                            currentPosition[currentScreen] -= 1;
                        }
                        else
                        {
                            currentTopPosition[currentScreen] -= 1;
                        }
                    }
                }
            }
        }
        updateParty();
        updateItemList();
        updateSelectedItem();
        updateDescription();
        updatePartyDisplays();
        updateScrollBar();
        return result;
    }

    private void updateNumbersBox(int price, bool hoverOnTens)
    {
        string numberString = "" + shopSelectedQuantity;
        if (shopSelectedQuantity < 10)
        {
            numberString = "0" + shopSelectedQuantity;
        }

        numbersBoxText.text = "x " + numberString + "\n$" + (price * shopSelectedQuantity);
        numbersBoxTextShadow.text = numbersBoxText.text;
        if (hoverOnTens)
        {
            numbersBoxSelector.pixelOffset = new Vector2(325, numbersBoxSelector.pixelOffset.y);
        }
        else
        {
            numbersBoxSelector.pixelOffset = new Vector2(331, numbersBoxSelector.pixelOffset.y);
        }
        numbersBoxSelectorShadow.pixelOffset = new Vector2(numbersBoxSelector.pixelOffset.x + 1,
            numbersBoxSelector.pixelOffset.y);
    }

    private IEnumerator openNumbersBox(int price, int max)
    {
        numbersBox.SetActive(true);
        numbersBoxBorder.texture = Resources.Load<Texture>("Frame/choice" + PlayerPrefs.GetInt("frameStyle"));

        if (max > 99)
        {
            max = 99;
        }

        shopSelectedQuantity = 1;
        bool hoverOnTens = false;

        updateNumbersBox(price, hoverOnTens);

        while (numbersBox.activeSelf)
        {
            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                hoverOnTens = true;
                updateNumbersBox(price, hoverOnTens);
            }
            else if (Input.GetAxisRaw("Horizontal") > 0)
            {
                hoverOnTens = false;
                updateNumbersBox(price, hoverOnTens);
            }

            if (Input.GetAxisRaw("Vertical") < 0)
            {
                int previousQuantity = shopSelectedQuantity;
                if (hoverOnTens)
                {
                    shopSelectedQuantity -= 10;
                }
                else
                {
                    shopSelectedQuantity -= 1;
                }

                if (shopSelectedQuantity < 1)
                {
                    if (previousQuantity != 1)
                    {
                        shopSelectedQuantity = 1;
                    }
                    else
                    {
                        shopSelectedQuantity = max;
                    }
                }

                SfxHandler.Play(selectClip);
                updateNumbersBox(price, hoverOnTens);
                yield return new WaitForSeconds(0.16f);
            }
            else if (Input.GetAxisRaw("Vertical") > 0)
            {
                int previousQuantity = shopSelectedQuantity;
                if (hoverOnTens)
                {
                    shopSelectedQuantity += 10;
                }
                else
                {
                    shopSelectedQuantity += 1;
                }

                if (shopSelectedQuantity > max)
                {
                    if (previousQuantity != max)
                    {
                        shopSelectedQuantity = max;
                    }
                    else
                    {
                        shopSelectedQuantity = 1;
                    }
                }

                SfxHandler.Play(selectClip);
                updateNumbersBox(price, hoverOnTens);
                yield return new WaitForSeconds(0.16f);
            }

            if (Input.GetButtonDown("Select"))
            {
                numbersBox.SetActive(false);
            }
            else if (Input.GetButtonDown("Back"))
            {
                shopSelectedQuantity = 0;
                numbersBox.SetActive(false);
            }

            yield return null;
        }
    }

    public IEnumerator control()
    {
        yield return StartCoroutine(control(true, false, false, null));
    }

    public IEnumerator control(bool partyAccessible, bool getItem)
    {
        yield return StartCoroutine(control(partyAccessible, getItem, false, null));
    }

    public IEnumerator control(string[] shopStock)
    {
        yield return StartCoroutine(control(false, false, true, shopStock));
    }

    private IEnumerator control(bool partyAccessible, bool getItem, bool setShopMode, string[] shopStock)
    {
        shopMode = setShopMode;
        if (shopMode)
        {
            shopItemList = shopStock;
            party.gameObject.SetActive(false);
            shopName.gameObject.SetActive(true);
            //sceneTransition.FadeIn(0);
            ScreenFade.main.SetToFadedIn();
        }
        else
        {
            party.gameObject.SetActive(true);
            moneyBox.SetActive(false);
            dataBox.SetActive(false);
            shopName.gameObject.SetActive(false);
            //sceneTransition.FadeIn();
            StartCoroutine(ScreenFade.main.Fade(true, ScreenFade.defaultSpeed));
        }
        numbersBox.SetActive(false);

        chosenItem = "";

        currentScreen = 1;
        currentPosition = new int[]
        {
            -1, 1, 1, 1, 1, 1
        };
        currentTopPosition = new int[]
        {
            -1, 0, 0, 0, 0, 0
        };
        currentItemList = SaveData.currentSave.Bag.getItemTypeArray(PokemonUnity.Inventory.ItemPockets.MISC, false); // Item?
        partyPosition = 0;
        inParty = false;

        updateScreen();

        updateParty();
        updateItemList();
        updateSelectedItem();
        updateDescription();
        updatePartyDisplays();
        updateScrollBar();

        updateMoneyBox();
        updateDataBox();

        bool running = true;

        while (running)
        {
            if (Input.GetAxisRaw("Vertical") > 0)
            {
                if (!inParty)
                {
                    if (currentPosition[currentScreen] > 1)
                    {
                        currentPosition[currentScreen] -= 1;
                        if (currentTopPosition[currentScreen] > 0 && currentPosition[currentScreen] < 3)
                        {
                            currentPosition[currentScreen] += 1;
                            StartCoroutine(animateSelection(currentPosition[currentScreen] - 1,
                                currentPosition[currentScreen]));
                            currentTopPosition[currentScreen] -= 1;
                            StartCoroutine("scrollScrollBar", -1);
                        }
                        else
                        {
                            StartCoroutine(animateSelection(currentPosition[currentScreen],
                                currentPosition[currentScreen] + 1));
                        }
                        updateDescription();
                        updatePartyDisplays();
                        updateDataBox();
                        SfxHandler.Play(selectClip);
                        yield return new WaitForSeconds(0.16f);
                    }
                }
                else
                {
                    if (partyPosition > 0)
                    {
                        partySlot[partyPosition].texture = partySlotTex;
                        partyPosition -= 1;
                        partySlot[partyPosition].texture = partySlotSelectedTex;

                        SfxHandler.Play(selectClip);
                        yield return new WaitForSeconds(0.16f);
                    }
                }
            }
            else if (Input.GetAxisRaw("Vertical") < 0)
            {
                if (!inParty)
                {
                    if (currentPosition[currentScreen] < visableSlots &&
                        currentPosition[currentScreen] < currentItemList.Length)
                    {
                        currentPosition[currentScreen] += 1;
                        if (currentTopPosition[currentScreen] < currentItemList.Length - visableSlots &&
                            currentPosition[currentScreen] > Mathf.Floor(visableSlots * 0.67f))
                        {
                            currentPosition[currentScreen] -= 1;
                            StartCoroutine(animateSelection(currentPosition[currentScreen] + 1,
                                currentPosition[currentScreen]));
                            currentTopPosition[currentScreen] += 1;
                            StartCoroutine("scrollScrollBar", 1);
                        }
                        else
                        {
                            StartCoroutine(animateSelection(currentPosition[currentScreen],
                                currentPosition[currentScreen] - 1));
                        }
                        updateDescription();
                        updatePartyDisplays();
                        updateDataBox();
                        SfxHandler.Play(selectClip);
                        yield return new WaitForSeconds(0.16f);
                    }
                }
                else
                {
                    if (partyPosition < partyLength - 1)
                    {
                        partySlot[partyPosition].texture = partySlotTex;
                        partyPosition += 1;
                        partySlot[partyPosition].texture = partySlotSelectedTex;

                        SfxHandler.Play(selectClip);
                        yield return new WaitForSeconds(0.16f);
                    }
                }
            }
            else if (Input.GetAxisRaw("Horizontal") > 0)
            {
                if (inParty)
                {
                    inParty = false;
                    partySlot[partyPosition].texture = partySlotTex;

                    SfxHandler.Play(selectClip);
                    yield return new WaitForSeconds(0.2f);
                }
                else if (!switching)
                {
                    if (currentScreen < 5)
                    {
                        currentScreen += 1;
                        updateScreen();
                        updateItemList();
                        updateSelectedItem();
                        updateDescription();
                        updatePartyDisplays();
                        updateScrollBar();
                        updateDataBox();
                        SfxHandler.Play(selectClip);
                        yield return new WaitForSeconds(0.2f);
                    }
                    else if (shopMode)
                    {
                        currentScreen = 2;
                        updateScreen();
                        updateItemList();
                        updateSelectedItem();
                        updateDescription();
                        updatePartyDisplays();
                        updateScrollBar();
                        updateDataBox();
                        SfxHandler.Play(selectClip);
                        yield return new WaitForSeconds(0.2f);
                    }
                }
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                if (!switching)
                {
                    if (currentScreen > 1)
                    {
                        if (!shopMode)
                        {
                            currentScreen -= 1;
                        }
                        else
                        {
                            currentScreen = 1;
                        }
                        updateScreen();
                        updateItemList();
                        updateSelectedItem();
                        updateDescription();
                        updatePartyDisplays();
                        updateScrollBar();
                        updateDataBox();
                        SfxHandler.Play(selectClip);
                        yield return new WaitForSeconds(0.2f);
                    }
                    else if (partyLength > 0 && partyAccessible)
                    {
                        inParty = true;
                        partySlot[partyPosition].texture = partySlotSelectedTex;

                        SfxHandler.Play(selectClip);
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                else if (!inParty && partyAccessible)
                {
                    inParty = true;
                    partySlot[partyPosition].texture = partySlotSelectedTex;

                    currentPosition[currentScreen] = selectedPosition;
                    currentTopPosition[currentScreen] = selectedTopPosition;

                    updateItemList();
                    updateSelectedItem();
                    updateDescription();
                    updatePartyDisplays();
                    updateScrollBar();

                    SfxHandler.Play(selectClip);
                    yield return new WaitForSeconds(0.2f);
                }
            }
            else if (Input.GetButton("Select"))
            {
                if (!switching && currentItemList.Length > 0)
                {
                    if (!inParty)
                    {
                        if (shopMode)
                        {
                            selected = currentPosition[currentScreen] + currentTopPosition[currentScreen] - 1;
                            PokemonUnity.Inventory.ItemData selectedItem = ItemDatabase.getItem(currentItemList[selected].ToItems());

                            switching = true;
                            selectedPosition = currentPosition[currentScreen];
                            selectedTopPosition = currentTopPosition[currentScreen];
                            updateSelectedItem();
                            SfxHandler.Play(selectClip);

                            chosenItem = currentItemList[selected];

                            if (currentScreen == 1)
                            {
                                //BUY
                                //custom prices not yet implemented
                                if (SaveData.currentSave.Player.Money >= selectedItem.Price)
                                {
                                    Dialog.drawDialogBox(2);
                                    yield return
                                        StartCoroutine(
                                            Dialog.drawText(selectedItem.Name +
                                                            "? Certainly. \nHow many would you like to buy?"));

                                    //quantity selection not yet implemented
                                    int maxQuantity =
                                        Mathf.FloorToInt((float) SaveData.currentSave.Player.Money /
                                                         (float) selectedItem.Price);
                                    Debug.Log(maxQuantity);
                                    yield return StartCoroutine(openNumbersBox(selectedItem.Price, maxQuantity));

                                    if (shopSelectedQuantity > 0)
                                    {
                                        Dialog.drawDialogBox(2);
                                        yield return
                                            StartCoroutine(
                                                Dialog.drawText(selectedItem.Name + ", and you want " +
                                                                shopSelectedQuantity + ". \nThat will be $" +
                                                                (shopSelectedQuantity * selectedItem.Price) +
                                                                ". OK?"));
                                        Dialog.drawChoiceBox(14);
                                        yield return StartCoroutine(Dialog.choiceNavigate());
                                        int chosenIndex2 = Dialog.chosenIndex;
                                        Dialog.undrawDialogBox();
                                        Dialog.undrawChoiceBox();

                                        if (chosenIndex2 == 1)
                                        {
                                            SfxHandler.Play(saleClip);

                                            addItem(selectedItem.Id, shopSelectedQuantity);
                                            SaveData.currentSave.Player.Money -= (shopSelectedQuantity *
                                                                                 selectedItem.Price);

                                            updateMoneyBox();
                                            updateDataBox();

                                            Dialog.drawDialogBox(2);
                                            yield return
                                                StartCoroutine(Dialog.drawTextSilent("Here you are!\nThank you!"));
                                            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                                            {
                                                yield return null;
                                            }
                                            Dialog.undrawDialogBox();
                                        }
                                    }
                                    Dialog.undrawDialogBox();
                                }
                                else
                                {
                                    Dialog.drawDialogBox(2);
                                    yield return StartCoroutine(Dialog.drawText("You don't have enough money."));
                                    while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                                    {
                                        yield return null;
                                    }
                                    Dialog.undrawDialogBox();
                                }
                            }
                            else
                            {
                                //SELL
                                Dialog.drawDialogBox(2);
                                yield return
                                    StartCoroutine(
                                        Dialog.drawText(selectedItem.Name + "? \nHow many would you like to sell?"))
                                    ;

                                //quantity selection not yet implemented
                                int maxQuantity = SaveData.currentSave.Bag.GetItemAmount(selectedItem.Name.ToItems());
                                Debug.Log(maxQuantity);
                                yield return
                                    StartCoroutine(openNumbersBox(
                                        Mathf.FloorToInt((float) selectedItem.Price / 2f), maxQuantity));

                                if (shopSelectedQuantity > 0)
                                {
                                    Dialog.drawDialogBox(2);
                                    yield return
                                        StartCoroutine(
                                            Dialog.drawText("I can pay $" +
                                                            (shopSelectedQuantity *
                                                             Mathf.FloorToInt((float) selectedItem.Price / 2f)) +
                                                            ". \nWould that be OK?"));
                                    Dialog.drawChoiceBox(14);
                                    yield return StartCoroutine(Dialog.choiceNavigate());
                                    int chosenIndex2 = Dialog.chosenIndex;
                                    Dialog.undrawDialogBox();
                                    Dialog.undrawChoiceBox();

                                    if (chosenIndex2 == 1)
                                    {
                                        SfxHandler.Play(saleClip);

                                        removeItem(selectedItem.Id, shopSelectedQuantity);
                                        //custom prices not yet implemented
                                        SaveData.currentSave.Player.Money += shopSelectedQuantity *
                                                                            Mathf.FloorToInt(
                                                                                (float) selectedItem.Price / 2f);

                                        updateMoneyBox();
                                        updateDataBox();

                                        Dialog.drawDialogBox(2);
                                        if (shopSelectedQuantity == 1)
                                        {
                                            yield return
                                                StartCoroutine(
                                                    Dialog.drawTextSilent("Turned over the " + selectedItem.Name +
                                                                          " \nand received $" +
                                                                          (shopSelectedQuantity *
                                                                           Mathf.FloorToInt(
                                                                               (float) selectedItem.Price / 2f)) +
                                                                          "."));
                                        }
                                        else
                                        {
                                            yield return
                                                StartCoroutine(
                                                    Dialog.drawTextSilent("Turned over the " + selectedItem.Name +
                                                                          "s \nand received $" +
                                                                          (shopSelectedQuantity *
                                                                           Mathf.FloorToInt(
                                                                               (float) selectedItem.Price / 2f)) +
                                                                          "."));
                                        }
                                        while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                                        {
                                            yield return null;
                                        }
                                        Dialog.undrawDialogBox();
                                    }
                                }
                                Dialog.undrawDialogBox();
                            }

                            updateMoneyBox();
                            updateDataBox();

                            switching = false;
                            selectedPosition = currentPosition[currentScreen];
                            selectedTopPosition = currentTopPosition[currentScreen];

                            updateScreen();
                            updateItemList();
                            updateParty();
                            updateSelectedItem();
                            updateDescription();
                            updatePartyDisplays();
                            updateScrollBar();

                            yield return new WaitForSeconds(0.2f);
                        }
                        else if (getItem)
                        {
                            selected = currentPosition[currentScreen] + currentTopPosition[currentScreen] - 1;
                            PokemonUnity.Inventory.ItemData selectedItem = ItemDatabase.getItem(currentItemList[selected].ToItems());

                            if (selectedItem.Pocket.Value == PokemonUnity.Inventory.ItemPockets.MACHINE ||
                                selectedItem.Pocket.Value == PokemonUnity.Inventory.ItemPockets.KEY)
                            {
                                Dialog.drawDialogBox();
                                yield return
                                    StartCoroutine(Dialog.drawText("The " + selectedItem.Name + " can't be held."))
                                    ;
                                while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                                {
                                    yield return null;
                                }
                                Dialog.undrawDialogBox();
                            }
                            else
                            {
                                switching = true;
                                selectedPosition = currentPosition[currentScreen];
                                selectedTopPosition = currentTopPosition[currentScreen];
                                updateSelectedItem();
                                SfxHandler.Play(selectClip);

                                chosenItem = currentItemList[selected];
                                running = false;
                            }
                        }
                        else
                        {
                            switching = true;
                            selected = currentPosition[currentScreen] + currentTopPosition[currentScreen] - 1;
                            selectedPosition = currentPosition[currentScreen];
                            selectedTopPosition = currentTopPosition[currentScreen];
                            updateSelectedItem();
                            SfxHandler.Play(selectClip);
                            yield return new WaitForSeconds(0.2f);
                        }
                    }
                    else
                    {
                        //display options for Pokemon on their own.
                        if (SaveData.currentSave.Player.Party[partyPosition].Item.ToString().Length > 0)
                        {
                            Dialog.drawDialogBox();
                            yield return
                                Dialog.StartCoroutine("drawText",
                                    "Take " + SaveData.currentSave.Player.Party[partyPosition].Name + "'s Item?");
                            Dialog.drawChoiceBox();
                            yield return Dialog.StartCoroutine("choiceNavigate");
                            int chosenIndex = Dialog.chosenIndex;
                            Dialog.undrawDialogBox();
                            Dialog.undrawChoiceBox();
                            if (chosenIndex == 1)
                            {
                                PokemonUnity.Inventory.Items receivedItem = SaveData.currentSave.Player.Party[partyPosition].SwapItem(PokemonUnity.Inventory.Items.NONE);
                                addItem(receivedItem, 1);
                                updateScreen();
                                updateItemList();
                                updateParty();
                                updateSelectedItem();
                                updateDescription();
                                updatePartyDisplays();
                                updateScrollBar();

                                Dialog.drawDialogBox();
                                yield return
                                    Dialog.StartCoroutine("drawText",
                                        "Took the " + receivedItem + " from " +
                                        SaveData.currentSave.Player.Party[partyPosition].Name + ".");
                                while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                                {
                                    yield return null;
                                }
                                Dialog.undrawDialogBox();
                            }
                        }

                        SfxHandler.Play(selectClip);
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                else
                {
                    if (!inParty && currentItemList.Length > 0)
                    {
                        //display options for selected item
                        if (currentPosition[currentScreen] == selectedPosition &&
                            currentTopPosition[currentScreen] == selectedTopPosition)
                        {
                            Dialog.drawDialogBox();
                            yield return
                                Dialog.StartCoroutine("drawText", "Do what with " + currentItemList[selected] + "?");
                            string[] choices = new string[] {"Deselect", "Cancel"};
                            PokemonUnity.Inventory.ItemData selectedItem = ItemDatabase.getItem(currentItemList[selected].ToItems());
                            if (selectedItem.Pocket == PokemonUnity.Inventory.ItemPockets.KEY)
                            {
                                new System.NotImplementedException();
                                //if (selectedItem.getItemEffect() == ItemData.ItemEffect.UNIQUE)
                                //{
                                //    choices = new string[]
                                //    {
                                //        "Use", "Register", "Deselect", "Cancel"
                                //    };
                                //}
                            }
                            //else if (selectedItem.getItemEffect() != ItemData.ItemEffect.NONE &&
                            //         selectedItem.getItemEffect() != ItemData.ItemEffect.FLEE &&
                            //         selectedItem.getItemEffect() != ItemData.ItemEffect.BALL &&
                            //         selectedItem.getItemEffect() != ItemData.ItemEffect.STATBOOST)
                            //{
                            //    new System.NotImplementedException();
                            //    choices = new string[]
                            //    {
                            //        "Use", "Deselect", "Cancel"
                            //    };
                            //}

                            Dialog.drawChoiceBox(choices);
                            yield return StartCoroutine(Dialog.choiceNavigate(choices));
                            int chosenIndex = Dialog.chosenIndex;
                            Dialog.undrawDialogBox();
                            Dialog.undrawChoiceBox();
                            if (chosenIndex == 3)
                            {
                                //USE (unique key item) //unique effects not yet implemented
                            }
                            if (chosenIndex == 2)
                            {
                                if (selectedItem.Pocket == PokemonUnity.Inventory.ItemPockets.KEY)
                                {
                                    //REGISTER (key item)
                                    Dialog.drawDialogBox();
                                    yield return
                                        Dialog.StartCoroutine("drawText",
                                            "Register " + selectedItem.Name + " to which slot?");
                                    choices = new string[]
                                    {
                                        "Slot 1", "Slot 2", "Slot 3", "Slot 4", "Cancel"
                                    };
                                    Dialog.drawChoiceBox(choices);
                                    yield return StartCoroutine(Dialog.choiceNavigate(choices));
                                    chosenIndex = Dialog.chosenIndex;
                                    if (chosenIndex == 4)
                                    {
                                        SaveData.currentSave.Player.registeredItems[0] = selectedItem.Name;
                                    }
                                    else if (chosenIndex == 3)
                                    {
                                        SaveData.currentSave.Player.registeredItems[1] = selectedItem.Name;
                                    }
                                    else if (chosenIndex == 2)
                                    {
                                        SaveData.currentSave.Player.registeredItems[2] = selectedItem.Name;
                                    }
                                    else if (chosenIndex == 1)
                                    {
                                        SaveData.currentSave.Player.registeredItems[3] = selectedItem.Name;
                                    }
                                }
                                else
                                {
                                    //USE
                                    //if (selectedItem.getItemEffect() == ItemData.ItemEffect.UNIQUE)
                                    //{
                                    //}
                                    //else if (partyLength > 0)
                                    if (partyLength > 0)
                                    {
                                        bool selectingPokemon = true;
                                        Dialog.drawDialogBox();
                                        if (selectedItem.Pocket != PokemonUnity.Inventory.ItemPockets.MACHINE)
                                        {
                                            yield return
                                                Dialog.StartCoroutine("drawText",
                                                    "Use " + selectedItem.Name + " on which Pokémon?");
                                        }
                                        else
                                        {
                                            //display TM bootup text
                                            Dialog.drawDialogBox(2);
                                            SfxHandler.Play(tmBootupClip);
                                            yield return StartCoroutine(Dialog.drawTextSilent("Booted up a TM."));
                                            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                                            {
                                                yield return null;
                                            }
                                            Dialog.drawDialogBox(2);
                                            yield return
                                                Dialog.StartCoroutine("drawText",
                                                    "It contained " + selectedItem.Name + ".");
                                            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                                            {
                                                yield return null;
                                            }

                                            Dialog.drawDialogBox(2);
                                            yield return
                                                Dialog.StartCoroutine("drawText",
                                                    "Teach " + selectedItem.Name + " \nto a Pokémon?");
                                            Dialog.drawChoiceBox(14);
                                            yield return StartCoroutine(Dialog.choiceNavigate());
                                            chosenIndex = Dialog.chosenIndex;
                                            Dialog.undrawChoiceBox();
                                            if (chosenIndex == 0)
                                            {
                                                //If No is chosen
                                                selectingPokemon = false;
                                                    //set selecting false to prevent the using phase from happening.
                                            }
                                            else
                                            {
                                                Dialog.drawDialogBox(2);
                                                yield return
                                                    Dialog.StartCoroutine("drawText",
                                                        "Who should learn \n" + selectedItem.Name + "?");
                                            }
                                        }
                                        partyPosition = 0;
                                        partySlot[partyPosition].texture = partySlotSelectedTex;

                                        // Begin Party Navigation to select target Pokemon.

                                        while (selectingPokemon)
                                        {
                                            if (Input.GetAxisRaw("Vertical") > 0)
                                            {
                                                if (partyPosition > 0)
                                                {
                                                    partySlot[partyPosition].texture = partySlotTex;
                                                    partyPosition -= 1;
                                                    partySlot[partyPosition].texture = partySlotSelectedTex;

                                                    SfxHandler.Play(selectClip);
                                                    yield return new WaitForSeconds(0.2f);
                                                }
                                            }
                                            else if (Input.GetAxisRaw("Vertical") < 0)
                                            {
                                                if (partyPosition < partyLength - 1)
                                                {
                                                    partySlot[partyPosition].texture = partySlotTex;
                                                    partyPosition += 1;
                                                    partySlot[partyPosition].texture = partySlotSelectedTex;

                                                    SfxHandler.Play(selectClip);
                                                    yield return new WaitForSeconds(0.2f);
                                                }
                                            }
                                            else if (Input.GetButtonDown("Select"))
                                            {
                                                Pokemon currentPokemon = SaveData.currentSave.Player.Party[partyPosition];

                                                yield return
                                                    StartCoroutine(runItemEffect(selectedItem, currentPokemon, true));

                                                selectingPokemon = false;
                                            }
                                            else if (Input.GetAxisRaw("Horizontal") > 0 || Input.GetButton("Back"))
                                            {
                                                selectingPokemon = false;
                                                switching = false;
                                                selected = -1;
                                                updateSelectedItem();
                                                SfxHandler.Play(selectClip);
                                                yield return new WaitForSeconds(0.2f);
                                            }

                                            yield return null;
                                        }
                                        updatePartyDisplays();
                                    }
                                    else
                                    {
                                        yield return Dialog.StartCoroutine("drawText", "There's no one to use it on!");
                                        while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                                        {
                                            yield return null;
                                        }
                                    }
                                }
                                partySlot[partyPosition].texture = partySlotTex;
                                Dialog.undrawDialogBox();
                                Dialog.undrawChoiceBox();
                            }
                            else if (chosenIndex == 1)
                            {
                                switching = false;
                                selected = -1;
                                updateSelectedItem();
                            }
                            SfxHandler.Play(selectClip);
                            yield return new WaitForSeconds(0.2f);
                        }
                        else
                        {
                            //switch item's location
                            switching = false;
                            Debug.Log("No moveBehind?");
                            //SaveData.currentSave.Bag.moveBehind(
                            //    SaveData.currentSave.Bag.getIndexOf(currentItemList[selected]),
                            //    SaveData.currentSave.Bag.getIndexOf(
                            //        currentItemList[
                            //            currentPosition[currentScreen] + currentTopPosition[currentScreen] - 1]));

                            selected = -1;
                            updateScreen();
                            updateItemList();
                            updateSelectedItem();
                            updateDescription();
                            updatePartyDisplays();
                            SfxHandler.Play(selectClip);
                            yield return new WaitForSeconds(0.2f);
                        }
                    }
                    else if (inParty)
                    {
                        //display options for using selected item on Pokemon
                        Pokemon currentPokemon = SaveData.currentSave.Player.Party[partyPosition];

                        Dialog.drawDialogBox();
                        yield return
                            Dialog.StartCoroutine("drawText", "Do what with " + currentItemList[selected] + "?");
                        string[] choices = new string[] {"Give", "Deselect", "Cancel"};
                        if (currentPokemon.hasItem())
                        {
                            choices = new string[]
                            {
                                "Swap", "Take", "Deselect", "Cancel"
                            };
                        }
                        PokemonUnity.Inventory.ItemData selectedItem = ItemDatabase.getItem(currentItemList[selected].ToItems());
                        //Adjust choices to suit situation
                        if (selectedItem.Pocket != PokemonUnity.Inventory.ItemPockets.KEY)
                        {
                            if (selectedItem.Pocket == PokemonUnity.Inventory.ItemPockets.MEDICINE ||
                                //selectedItem.ItemType == ItemType.EV ||
                                selectedItem.Category == PokemonUnity.Inventory.ItemCategory.EVOLUTION)
                            {
                                if (!currentPokemon.hasItem())
                                {
                                    choices = new string[]
                                    {
                                        "Use", "Give", "Deselect", "Cancel"
                                    };
                                }
                                else
                                {
                                    choices = new string[]
                                    {
                                        "Use", "Swap", "Take", "Deselect", "Cancel"
                                    };
                                }
                            }
                            if (selectedItem.Pocket == PokemonUnity.Inventory.ItemPockets.MACHINE)
                            {
                                choices = new string[]
                                {
                                    "Use", "Deselect", "Cancel"
                                };
                            }
                            if (selectedItem.Name == "Rare Candy")
                            {
                                if (!currentPokemon.hasItem())
                                {
                                    choices = new string[]
                                    {
                                            "Use", "Give", "Deselect", "Cancel"
                                    };
                                }
                                else
                                {
                                    choices = new string[]
                                    {
                                            "Use", "Swap", "Take", "Deselect", "Cancel"
                                    };
                                }
                            }
                            //else if (selectedItem.getItemEffect() == ItemData.ItemEffect.UNIQUE)
                            //{
                            //    if (selectedItem.Name == "Rare Candy")
                            //    {
                            //        if (string.IsNullOrEmpty(currentPokemon.getHeldItem()))
                            //        {
                            //            choices = new string[]
                            //            {
                            //                "Use", "Give", "Deselect", "Cancel"
                            //            };
                            //        }
                            //        else
                            //        {
                            //            choices = new string[]
                            //            {
                            //                "Use", "Swap", "Take", "Deselect", "Cancel"
                            //            };
                            //        }
                            //    }
                            //}
                        }

                        Dialog.drawChoiceBox(choices);
                        yield return StartCoroutine(Dialog.choiceNavigate(choices));
                        int chosenIndex = Dialog.chosenIndex;
                        Dialog.undrawDialogBox();
                        Dialog.undrawChoiceBox();

                        string chosenChoice = choices[choices.Length - chosenIndex - 1];

                        if (chosenChoice == "Give")
                        {
                            currentPokemon.SwapItem(selectedItem.Name.ToItems());
                            removeItem(selectedItem.Id, 1);
                            updateScreen();
                            updateItemList();
                            updateParty();
                            updateSelectedItem();
                            updateDescription();
                            updatePartyDisplays();
                            updateScrollBar();

                            Dialog.drawDialogBox();
                            yield return
                                Dialog.StartCoroutine("drawText",
                                    "Gave " + selectedItem.Name + " to " + currentPokemon.Name + ".");
                            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                            {
                                yield return null;
                            }
                            Dialog.undrawDialogBox();

                            switching = false;
                            selected = -1;
                            updateSelectedItem();
                        }
                        else if (chosenChoice == "Take")
                        {
                            PokemonUnity.Inventory.Items receivedItem = currentPokemon.SwapItem(PokemonUnity.Inventory.Items.NONE);
                            addItem(receivedItem, 1);
                            updateScreen();
                            updateItemList();
                            updateParty();
                            updateSelectedItem();
                            updateDescription();
                            updatePartyDisplays();
                            updateScrollBar();

                            Dialog.drawDialogBox();
                            yield return
                                Dialog.StartCoroutine("drawText",
                                    "Took the " + receivedItem + " from " + currentPokemon.Name + ".");
                            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                            {
                                yield return null;
                            }
                            Dialog.undrawDialogBox();

                            switching = false;
                            selected = -1;
                            updateSelectedItem();
                        }
                        else if (chosenChoice == "Swap")
                        {
                            PokemonUnity.Inventory.Items receivedItem = currentPokemon.SwapItem(selectedItem.Name.ToItems());
                            addItem(receivedItem, 1);
                            removeItem(selectedItem.Id, 1);
                            updateScreen();
                            updateItemList();
                            updateParty();
                            updateSelectedItem();
                            updateDescription();
                            updatePartyDisplays();
                            updateScrollBar();

                            Dialog.drawDialogBox();
                            yield return
                                Dialog.StartCoroutine("drawText",
                                    "Gave " + selectedItem.Name + " to " + currentPokemon.Name + ",");
                            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                            {
                                yield return null;
                            }
                            Dialog.drawDialogBox();
                            yield return
                                Dialog.StartCoroutine("drawText", "and received " + receivedItem + " in return.");
                            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                            {
                                yield return null;
                            }
                            Dialog.undrawDialogBox();

                            switching = false;
                            selected = -1;
                            updateSelectedItem();
                        }
                        else if (chosenChoice == "Use")
                        {
                            yield return StartCoroutine(runItemEffect(selectedItem, currentPokemon));

                            updatePartyDisplays();

                            Dialog.undrawDialogBox();
                            Dialog.undrawChoiceBox();
                        }
                        else if (chosenChoice == "Deselect")
                        {
                            switching = false;
                            selected = -1;
                            updateSelectedItem();
                        }

                        SfxHandler.Play(selectClip);
                        yield return new WaitForSeconds(0.2f);
                    }
                }
            }
            else if (Input.GetButton("Back"))
            {
                if (switching)
                {
                    switching = false;
                    selected = -1;

                    inParty = false;
                    partySlot[partyPosition].texture = partySlotTex;

                    updateScreen();
                    updateItemList();
                    updateSelectedItem();
                    SfxHandler.Play(selectClip);
                    yield return new WaitForSeconds(0.2f);
                }
                else
                {
                    running = false;
                }
            }
            yield return null;
        }

        if (!shopMode)
        {
            yield return StartCoroutine(ScreenFade.main.Fade(false, ScreenFade.defaultSpeed));
            //yield return new WaitForSeconds(sceneTransition.FadeOut());
        }

        switching = false;
        selected = -1;
        inParty = false;

        GlobalVariables.global.resetFollower();
        gameObject.SetActive(false);
    }


    //RUN ITEM EFFECT.		This code handles the usage of items in the bag.
    private IEnumerator runItemEffect(PokemonUnity.Inventory.ItemData selectedItem, Pokemon currentPokemon)
    {
        yield return StartCoroutine(runItemEffect(selectedItem, currentPokemon, false));
    }

    private IEnumerator runItemEffect(PokemonUnity.Inventory.ItemData selectedItem, Pokemon currentPokemon, bool booted)
    {
        if (selectedItem.Pocket == PokemonUnity.Inventory.ItemPockets.MEDICINE)
        {
            if (ItemDatabase.IsPotion(selectedItem.Id)) // Potion
            {
                //HP
                if (currentPokemon.HP < currentPokemon.TotalHP &&
                    currentPokemon.Status != PokemonUnity.Status.FAINT)
                {
                    //determine amount / intialise HP Bar Animation variables
                    float amount = ItemDatabase.GetPotionHealItem(selectedItem.Id);
                    if (amount <= 1)
                    {
                        amount = currentPokemon.healHP(currentPokemon.HP * amount);
                    }
                    else
                    {
                        amount = currentPokemon.healHP(amount);
                    }
                    float startLength = partyHPBar[partyPosition].pixelInset.width;
                    float difference =
                        Mathf.Floor(48f * (currentPokemon.HP / currentPokemon.TotalHP)) -
                        startLength;
                    float increment = 0;
                    float speed = 0.5f;

                    SfxHandler.Play(healClip);
                    removeItem(selectedItem.Id, 1);

                    //animate HP bar restoring
                    while (increment < 1)
                    {
                        increment += (1 / speed) * Time.deltaTime;
                        if (increment > 1)
                        {
                            increment = 1;
                        }
                        partyHPBar[partyPosition].pixelInset = new Rect(partyHPBar[partyPosition].pixelInset.x,
                            partyHPBar[partyPosition].pixelInset.y,
                            startLength + (difference * increment), partyHPBar[partyPosition].pixelInset.height);
                        //Color the bar
                        if (partyHPBar[partyPosition].pixelInset.width < 12f)
                        {
                            partyHPBar[partyPosition].color = new Color(1, 0.125f, 0, 1);
                        }
                        else if (partyHPBar[partyPosition].pixelInset.width < 24f)
                        {
                            partyHPBar[partyPosition].color = new Color(1, 0.75f, 0, 1);
                        }
                        else
                        {
                            partyHPBar[partyPosition].color = new Color(0.125f, 1, 0.065f, 1);
                        }

                        yield return null;
                    }

                    Dialog.drawDialogBox();
                    yield return Dialog.StartCoroutine("drawTextSilent", "It restored " + amount + " points.");
                    while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                    {
                        yield return null;
                    }
                }
                else
                {
                    Dialog.drawDialogBox();
                    yield return Dialog.StartCoroutine("drawText", "It wouldn't have any effect.");
                    while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                    {
                        yield return null;
                    }
                    switching = false;
                    selected = -1;
                    updateSelectedItem();
                }
            }
            else if (selectedItem.Category == PokemonUnity.Inventory.ItemCategory.STATUS_CURES)
            {
                //STATUS
                //Check current pokemon has the status the item cures
                CureStatus statusCure = ItemDatabase.GetCureStatusItem(selectedItem.Id);

                //if an ALL is used, set it to cure anything but FAINTED or NONE.
                if (statusCure == CureStatus.ALL && currentPokemon.Status != PokemonUnity.Status.FAINT &&
                    currentPokemon.Status!= PokemonUnity.Status.NONE)
                {
                    statusCure = (CureStatus)currentPokemon.Status;
                }

                if ((CureStatus)currentPokemon.Status == statusCure)
                {
                    if (statusCure == CureStatus.FAINT)
                    {
                        //Revive
                        currentPokemon.setStatus(PokemonUnity.Status.NONE);

                        //determine amount / intialise HP Bar Animation variables
                        //float amount = selectedItem.getFloatParameter();
                        float amount = 0;
                        if (amount <= 1)
                        {
                            amount = currentPokemon.healHP(currentPokemon.HP * amount);
                        }
                        else
                        {
                            amount = currentPokemon.healHP(amount);
                        }
                        float startLength = partyHPBar[partyPosition].pixelInset.width;
                        float difference =
                            Mathf.Floor(48f * ((float)currentPokemon.HP / (float)currentPokemon.TotalHP)) -
                            startLength;
                        float increment = 0;
                        float speed = 0.5f;

                        SfxHandler.Play(healClip);
                        removeItem(selectedItem.Id, 1);

                        //animate HP bar restoring
                        while (increment < 1)
                        {
                            increment += (1 / speed) * Time.deltaTime;
                            if (increment > 1)
                            {
                                increment = 1;
                            }
                            partyHPBar[partyPosition].pixelInset = new Rect(partyHPBar[partyPosition].pixelInset.x,
                                partyHPBar[partyPosition].pixelInset.y,
                                startLength + (difference * increment), partyHPBar[partyPosition].pixelInset.height);
                            //Color the bar
                            if (partyHPBar[partyPosition].pixelInset.width < 12f)
                            {
                                partyHPBar[partyPosition].color = new Color(1, 0.125f, 0, 1);
                            }
                            else if (partyHPBar[partyPosition].pixelInset.width < 24f)
                            {
                                partyHPBar[partyPosition].color = new Color(1, 0.75f, 0, 1);
                            }
                            else
                            {
                                partyHPBar[partyPosition].color = new Color(0.125f, 1, 0.065f, 1);
                            }

                            yield return null;
                        }

                        Dialog.drawDialogBox();
                        yield return Dialog.StartCoroutine("drawTextSilent", "It restored " + amount + " points.");
                        while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                        {
                            yield return null;
                        }
                    }
                    else
                    {
                        currentPokemon.setStatus(PokemonUnity.Status.NONE);

                        SfxHandler.Play(healClip);
                        removeItem(selectedItem.Id, 1);

                        Dialog.drawDialogBox();
                        if (statusCure == CureStatus.SLEEP)
                        {
                            yield return Dialog.StartCoroutine("drawTextSilent", currentPokemon.Name + " woke up!");
                        }
                        else if (statusCure == CureStatus.BURN)
                        {
                            yield return Dialog.StartCoroutine("drawTextSilent", currentPokemon.Name + " was healed!");
                        }
                        else if (statusCure == CureStatus.FROZEN)
                        {
                            yield return Dialog.StartCoroutine("drawTextSilent", currentPokemon.Name + " thawed out!");
                        }
                        else if (statusCure == CureStatus.PARALYSIS)
                        {
                            yield return Dialog.StartCoroutine("drawTextSilent", currentPokemon.Name + " was cured!");
                        }
                        else if (statusCure == CureStatus.POISON)
                        {
                            yield return Dialog.StartCoroutine("drawTextSilent", currentPokemon.Name + " was cured!");
                        }

                        while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                        {
                            yield return null;
                        }
                    }
                }
                else
                {
                    Dialog.drawDialogBox();
                    yield return Dialog.StartCoroutine("drawText", "It wouldn't have any effect.");
                    while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                    {
                        yield return null;
                    }
                    switching = false;
                    selected = -1;
                    updateSelectedItem();
                }
            }
            else if (selectedItem.Category == PokemonUnity.Inventory.ItemCategory.PP_RECOVERY)
            {
                //PP
                PokemonUnity.Attack.Move[] currentPokemonMoveset = currentPokemon.moves;
                bool[] currentPokemonMovesPP = new bool[4];
                int[] currentPokemonPP = currentPokemon.GetPP();
                int[] currentPokemonMaxPP = currentPokemon.GetMaxPP();
                bool loweredPP = false;
                for (int i = 0; i < 4; i++)
                {
                    if (currentPokemonMoveset[i].MoveId != PokemonUnity.Moves.NONE)
                    {
                        if (currentPokemonPP[i] < currentPokemonMaxPP[i])
                        {
                            loweredPP = true;
                        }
                        else
                        {
                            //remove any move from the array that has full PP.
                            currentPokemonMovesPP[i] = false;
                        }
                    }
                    else
                        currentPokemonMovesPP[i] = false;
                }
                if (loweredPP)
                {
                    //check that the item is not healing all PP
                    if (ItemDatabase.IsHealAllMovesPPItem(selectedItem.Id))
                    {
                        //heal PP for each move.
                        float amount = ItemDatabase.GetPPHealItem(selectedItem.Id);
                        for (int i = 0; i < 4; i++)
                        {
                            if (currentPokemonMoveset[i].MoveId != PokemonUnity.Moves.NONE)
                            {
                                if (amount <= 1)
                                {
                                    amount = currentPokemon.healPP(i,
                                        Mathf.RoundToInt(currentPokemon.getMaxPP(i) * amount));
                                }
                                else
                                {
                                    amount = currentPokemon.healPP(i, amount);
                                }
                            }
                        }

                        SfxHandler.Play(healClip);
                        removeItem(selectedItem.Id, 1);

                        Dialog.drawDialogBox();
                        yield return Dialog.StartCoroutine("drawTextSilent", "Restored all moves.");
                        while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                        {
                            yield return null;
                        }
                    }
                    else
                    {
                        //Set up the choices string to only contain moves with lowered PP.
                        string[] choices = new string[]
                        {
                        currentPokemonMoveset[0].MoveId.toString(), currentPokemonMoveset[1].MoveId.toString(),
                        currentPokemonMoveset[2].MoveId.toString(), currentPokemonMoveset[3].MoveId.toString(), "Cancel"
                        };
                        string[] packedChoices = new string[5];
                        int packedIndex = 0;
                        for (int i = 0; i < 5; i++)
                        {
                            if (choices[i] != null)
                            {
                                packedChoices[packedIndex] = choices[i];
                                packedIndex += 1;
                            }
                        }
                        choices = new string[packedIndex];
                        for (int i = 0; i < packedIndex; i++)
                        {
                            choices[i] = packedChoices[i];
                        }

                        //Start the dialog, now that the choices have been set up.
                        Dialog.drawDialogBox();
                        yield return Dialog.StartCoroutine("drawText", "Restore which move's PP?");
                        Dialog.drawChoiceBoxWidth(choices, 110);
                        yield return StartCoroutine(Dialog.choiceNavigate(choices));
                        int chosenIndex = Dialog.chosenIndex;
                        if (chosenIndex != 0)
                        {
                            //resolve move number
                            int moveNumber = 0;
                            for (int i = 0; i < 4; i++)
                            {
                                if (currentPokemonMoveset[i].MoveId == choices[choices.Length - chosenIndex - 1].ToMoves())
                                {
                                    moveNumber = i;
                                }
                            }

                            //heal PP for selected move.
                            float amount = ItemDatabase.GetPPHealItem(selectedItem.Id);
                            if (amount <= 1)
                            {
                                amount = currentPokemon.healPP(moveNumber,
                                    Mathf.RoundToInt(currentPokemon.getMaxPP(moveNumber) * amount));
                            }
                            else
                            {
                                amount = currentPokemon.healPP(moveNumber, amount);
                            }

                            SfxHandler.Play(healClip);
                            removeItem(selectedItem.Id, 1);

                            Dialog.drawDialogBox();
                            yield return Dialog.StartCoroutine("drawTextSilent", "It restored " + amount + " points.");
                            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                            {
                                yield return null;
                            }
                        }
                    }
                }
                else
                {
                    Dialog.drawDialogBox();
                    yield return Dialog.StartCoroutine("drawText", "It wouldn't have any effect.");
                    while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                    {
                        yield return null;
                    }
                    switching = false;
                    selected = -1;
                    updateSelectedItem();
                }
            }
            else if (selectedItem.Id == PokemonUnity.Inventory.Items.RARE_CANDY)
            {
                if (currentPokemon.Level < 100)
                {
                    int exp = PokemonUnity.Monster.Data.Experience.GetStartExperience(currentPokemon.GrowthRate, currentPokemon.Level);
                    currentPokemon.Experience.AddExperience(currentPokemon.Experience.NextLevel - exp - currentPokemon.Exp);

                    SfxHandler.Play(healClip);
                    updateParty();

                    removeItem(selectedItem.Id, 1); //remove item
                    updateSelectedItem();

                    Dialog.drawDialogBox();
                    yield return
                        Dialog.StartCoroutine("drawTextSilent",
                            currentPokemon.Name + "'s level rose to " + currentPokemon.Level + "!");
                    while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                    {
                        yield return null;
                    }

                    //stat displays not yet implemented

                    int pkmnID = (int)currentPokemon.Species;
                    //check for level evolution. EVOLVE
                    int evid = currentPokemon.getEvolutionID(EvolutionMethod.Level, currentPokemon.Level);
                    if (evid != -1)
                    {
                        SfxHandler.Play(selectClip);
                        BgmHandler.main.PlayOverlay(null, 0, 0.5f);
                        yield return StartCoroutine(ScreenFade.main.Fade(false, ScreenFade.defaultSpeed));

                        //Set SceneEvolution to be active so that it appears
                        Scene.main.Evolution.gameObject.SetActive(true);
                        StartCoroutine(Scene.main.Evolution.control(currentPokemon, evid));
                        //Start an empty loop that will only stop when SceneEvolution is no longer active (is closed)
                        while (Scene.main.Evolution.gameObject.activeSelf)
                        {
                            yield return null;
                        }

                        updateParty();

                        yield return StartCoroutine(ScreenFade.main.Fade(true, ScreenFade.defaultSpeed));
                    }
                    //if evolution not successful / wasn't called, check for moves to learn
                    if (pkmnID == (int)currentPokemon.Species)
                    {
                        string move = currentPokemon.MoveLearnedAtLevel(currentPokemon.Level);
                        //Debug.Log(move);
                        if (!string.IsNullOrEmpty(move) && !currentPokemon.hasMove(move.ToMoves()))
                        {
                            yield return StartCoroutine(LearnMove(currentPokemon, move.ToMoves()));
                        }
                    }
                }
                else
                {
                    Dialog.drawDialogBox();
                    yield return Dialog.StartCoroutine("drawText", "It wouldn't have any effect.");
                    while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                    {
                        yield return null;
                    }
                    switching = false;
                    selected = -1;
                    updateSelectedItem();
                }
            }
        }
        //else if (selectedItem.getItemEffect() == ItemData.ItemEffect.EV)
        //{
        //    //EV
        //    string statBooster = selectedItem.getStringParameter().ToUpper();
        //    float amount = selectedItem.getFloatParameter();
        //    bool evsAdded = currentPokemon.addEVs(statBooster, amount);
        //    if (evsAdded)
        //    {
        //        SfxHandler.Play(healClip);
        //        removeItem(selectedItem.Name, 1);
        //
        //        Dialog.drawDialogBox();
        //        if (statBooster == "HP")
        //        {
        //            yield return Dialog.StartCoroutine("drawTextSilent", currentPokemon.Name + "'s HP rose!");
        //        }
        //        else if (statBooster == "ATK")
        //        {
        //            yield return Dialog.StartCoroutine("drawTextSilent", currentPokemon.Name + "'s Attack rose!");
        //        }
        //        else if (statBooster == "DEF")
        //        {
        //            yield return Dialog.StartCoroutine("drawTextSilent", currentPokemon.Name + "'s Defense rose!");
        //        }
        //        else if (statBooster == "SPA")
        //        {
        //            yield return
        //                Dialog.StartCoroutine("drawTextSilent", currentPokemon.Name + "'s Special Attack rose!");
        //        }
        //        else if (statBooster == "SPD")
        //        {
        //            yield return
        //                Dialog.StartCoroutine("drawTextSilent", currentPokemon.Name + "'s Special Defense rose!");
        //        }
        //        else if (statBooster == "SPE")
        //        {
        //            yield return Dialog.StartCoroutine("drawTextSilent", currentPokemon.Name + "'s Speed rose!");
        //        }
        //
        //        while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
        //        {
        //            yield return null;
        //        }
        //        updateSelectedItem();
        //    }
        //    else
        //    {
        //        Dialog.drawDialogBox();
        //        yield return Dialog.StartCoroutine("drawText", "It wouldn't have any effect.");
        //        while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
        //        {
        //            yield return null;
        //        }
        //        switching = false;
        //        selected = -1;
        //        updateSelectedItem();
        //    }
        //}
        else if (selectedItem.Category == PokemonUnity.Inventory.ItemCategory.EVOLUTION)
        {
            //EVOLVE
            //if (currentPokemon.canEvolve("Stone," + selectedItem.Name))
            //if (currentPokemon.canEvolve(PokemonUnity.Monster.EvolutionMethod.Item, ConverterNames.ChangeItemToEnum(selectedItem.Name)))
            int evid = currentPokemon.getEvolutionID(EvolutionMethod.Item, selectedItem.Name.ToItems());
            if (evid != -1)
            {
                int oldID = (int)currentPokemon.Species;
                SfxHandler.Play(selectClip);
                BgmHandler.main.PlayOverlay(null, 0, 0.5f);
                //yield return new WaitForSeconds(sceneTransition.FadeOut());
                yield return StartCoroutine(ScreenFade.main.Fade(false, ScreenFade.defaultSpeed));
        
                //Set SceneEvolution to be active so that it appears
                Scene.main.Evolution.gameObject.SetActive(true);
                StartCoroutine(Scene.main.Evolution.control(currentPokemon, evid));
                //Start an empty loop that will only stop when SceneEvolution is no longer active (is closed)
                while (Scene.main.Evolution.gameObject.activeSelf)
                {
                    yield return null;
                }
        
                if (oldID != (int)currentPokemon.Species)
                {
                    //if evolved
                    removeItem(selectedItem.Id, 1);
                } //remove item
        
                //yield return new WaitForSeconds(sceneTransition.FadeIn());
                yield return StartCoroutine(ScreenFade.main.Fade(true, ScreenFade.defaultSpeed));
                Dialog.undrawDialogBox();
            }
            else
            {
                Dialog.drawDialogBox();
                yield return Dialog.StartCoroutine("drawText", "It wouldn't have any effect.");
                while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                {
                    yield return null;
                }
                switching = false;
                selected = -1;
                updateSelectedItem();
            }
        }
        else if (selectedItem.Pocket == PokemonUnity.Inventory.ItemPockets.MACHINE)
        {
            Debug.Log("Not implement");
            //TMData tm = (TMData)selectedItem;
            ////TM
            //if (currentPokemon.hasMove(tm.MoveID))
            //{
            //    Dialog.drawDialogBox(2);
            //    yield return
            //        Dialog.StartCoroutine("drawText",
            //            currentPokemon.Name + " already knows \n" + selectedItem.Name + ".");
            //    while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
            //    {
            //        yield return null;
            //    }
            //}
            //else if (currentPokemon.CanLearnMove(tm.MoveID, LearnMethod.machine))
            //{
            //    //check if can learn move
            //    if (!booted)
            //    {
            //        Dialog.drawDialogBox(2);
            //        SfxHandler.Play(tmBootupClip);
            //        yield return StartCoroutine(Dialog.drawTextSilent("Booted up a TM."));
            //        while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
            //        {
            //            yield return null;
            //        }
            //        Dialog.drawDialogBox(2);
            //        yield return Dialog.StartCoroutine("drawText", "It contained " + selectedItem.Name + ".");
            //        while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
            //        {
            //            yield return null;
            //        }
            //        Dialog.drawDialogBox(2);
            //        yield return
            //            Dialog.StartCoroutine("drawText",
            //                "Teach " + selectedItem.Name + "\nto " + currentPokemon.Name + "?");
            //        Dialog.drawChoiceBox(14);
            //        yield return StartCoroutine(Dialog.choiceNavigate());
            //        Dialog.undrawChoiceBox();
            //
            //        if (Dialog.chosenIndex == 1)
            //        {
            //            yield return StartCoroutine(LearnMove(currentPokemon, tm.MoveID));
            //        }
            //    }
            //    else
            //    {
            //        yield return StartCoroutine(LearnMove(currentPokemon, tm.MoveID));
            //    }
            //}
            //else
            //{
            //    Dialog.drawDialogBox();
            //    yield return Dialog.StartCoroutine("drawText", currentPokemon.Name + " can't learn that move.");
            //    while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
            //    {
            //        yield return null;
            //    }
            //}
            Dialog.undrawDialogBox();
            switching = false;
            selected = -1;
            updateSelectedItem();
        }
        
    }

    private IEnumerator LearnMove(Pokemon selectedPokemon, PokemonUnity.Moves move)
    {
        int chosenIndex = 1;
        if (chosenIndex == 1)
        {
            bool learning = true;
            while (learning)
            {
                //Moveset is full
                if (selectedPokemon.countMoves() == 4)
                {
                    Dialog.drawDialogBox(2);
                    yield return
                        Dialog.StartCoroutine("drawText",
                            selectedPokemon.Name + " wants to learn the \nmove " + move + ".");
                    while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                    {
                        yield return null;
                    }
                    Dialog.drawDialogBox(2);
                    yield return
                        Dialog.StartCoroutine("drawText",
                            "However, " + selectedPokemon.Name + " already \nknows four moves.");
                    while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                    {
                        yield return null;
                    }
                    Dialog.drawDialogBox(2);
                    yield return
                        Dialog.StartCoroutine("drawText", "Should a move be deleted and \nreplaced with " + move.toString() + "?");

                    Dialog.drawChoiceBox(14);
                    yield return StartCoroutine(Dialog.choiceNavigate());
                    chosenIndex = Dialog.chosenIndex;
                    Dialog.undrawChoiceBox();
                    if (chosenIndex == 1)
                    {
                        Dialog.drawDialogBox(2);
                        yield return Dialog.StartCoroutine("drawText", "Which move should \nbe forgotten?");
                        while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                        {
                            yield return null;
                        }

                        yield return StartCoroutine(ScreenFade.main.Fade(false, ScreenFade.defaultSpeed));

                        //Set SceneSummary to be active so that it appears
                        Scene.main.Summary.gameObject.SetActive(true);
                        StartCoroutine(Scene.main.Summary.control(selectedPokemon, move.toString()));
                        //Start an empty loop that will only stop when SceneSummary is no longer active (is closed)
                        while (Scene.main.Summary.gameObject.activeSelf)
                        {
                            yield return null;
                        }

                        string replacedMove = Scene.main.Summary.replacedMove;
                        yield return StartCoroutine(ScreenFade.main.Fade(true, ScreenFade.defaultSpeed));

                        if (!string.IsNullOrEmpty(replacedMove))
                        {
                            Dialog.drawDialogBox(2);
                            yield return Dialog.StartCoroutine("drawTextSilent", "1, ");
                            yield return new WaitForSeconds(0.4f);
                            yield return Dialog.StartCoroutine("drawTextSilent", "2, ");
                            yield return new WaitForSeconds(0.4f);
                            yield return Dialog.StartCoroutine("drawTextSilent", "and... ");
                            yield return new WaitForSeconds(0.4f);
                            yield return Dialog.StartCoroutine("drawTextSilent", "... ");
                            yield return new WaitForSeconds(0.4f);
                            yield return Dialog.StartCoroutine("drawTextSilent", "... ");
                            yield return new WaitForSeconds(0.4f);
                            SfxHandler.Play(forgetMoveClip);
                            yield return Dialog.StartCoroutine("drawTextSilent", "Poof!");
                            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                            {
                                yield return null;
                            }

                            Dialog.drawDialogBox(2);
                            yield return
                                Dialog.StartCoroutine("drawText",
                                    selectedPokemon.Name + " forgot how to \nuse " + replacedMove + ".");
                            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                            {
                                yield return null;
                            }
                            Dialog.drawDialogBox(2);
                            yield return Dialog.StartCoroutine("drawText", "And...");
                            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                            {
                                yield return null;
                            }

                            Dialog.drawDialogBox(2);
                            AudioClip mfx = Resources.Load<AudioClip>("Audio/mfx/GetAverage");
                            BgmHandler.main.PlayMFX(mfx);
                            StartCoroutine(Dialog.drawTextSilent(selectedPokemon.Name + " learned \n" + move + "!"));
                            yield return new WaitForSeconds(mfx.length);
                            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                            {
                                yield return null;
                            }
                            Dialog.undrawDialogBox();
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
                        Dialog.drawDialogBox(2);
                        yield return Dialog.StartCoroutine("drawText", "Give up on learning the move \n" + move + "?");
                        Dialog.drawChoiceBox(14);
                        yield return StartCoroutine(Dialog.choiceNavigate());
                        chosenIndex = Dialog.chosenIndex;
                        Dialog.undrawChoiceBox();
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

                    Dialog.drawDialogBox(2);
                    AudioClip mfx = Resources.Load<AudioClip>("Audio/mfx/GetAverage");
                    BgmHandler.main.PlayMFX(mfx);
                    StartCoroutine(Dialog.drawTextSilent(selectedPokemon.Name + " learned \n" + move.toString() + "!"));
                    yield return new WaitForSeconds(mfx.length);
                    while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                    {
                        yield return null;
                    }
                    Dialog.undrawDialogBox();
                    learning = false;
                }
            }
        }
        if (chosenIndex == 0)
        {
            //NOT ELSE because this may need to run after (chosenIndex == 1) runs
            //cancel learning loop
            Dialog.drawDialogBox(2);
            yield return Dialog.StartCoroutine("drawText", selectedPokemon.Name + " did not learn \n" + move + ".")
                ;
            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
            {
                yield return null;
            }
        }
    }
}