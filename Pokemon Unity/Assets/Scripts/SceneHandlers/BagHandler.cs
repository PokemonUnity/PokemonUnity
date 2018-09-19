//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

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

    /// <summary>
    /// This should be an <see cref="ItemDataOld.ItemPockets"/> enum;
    /// Use <seealso cref="currentBagScreen"/>
    /// </summary>
    /// <remarks>Is there a way to find out which int is for what screen? So confused...</remarks>
    public int currentScreenInt = 1;
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// currentMenuScreen? Also account for PC menu or even settings menu?
    /// ...maybe not, since it's "BagHandler" class
    /// </remarks>
    public ItemDataOld.ItemPockets currentBagScreen = ItemDataOld.ItemPockets.MISC;

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

    private string[] currentItemStringList;
    private ItemDataOld[] currentItemList;

    private bool shopMode;
    private string[] shopItemList;
    private int shopSelectedQuantity;

    //private AudioSource BagAudio;

    public AudioClip selectClip;
    public AudioClip healClip;
    public AudioClip tmBootupClip;
    public AudioClip forgetMoveClip;
    public AudioClip saleClip;

    void Awake()
    {
        Dialog = transform.GetComponent<DialogBoxHandler>();
        //BagAudio = transform.GetComponent<AudioSource>();

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
        float itemsLength = currentItemStringList.Length;
        float maxHeight = 78;
        float maxView = visableSlots;
        if (currentScreenInt == 4 && !shopMode)
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
            heightOffset -= ((maxHeight - barHeight) / (itemsLength - maxView)) * (currentTopPosition[currentScreenInt]);
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
            if (currentPosition[currentScreenInt] == i)
            {
                if (switching)
                {
                    if (i == selected - currentTopPosition[currentScreenInt] + 1)
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
                    if (i == selected - currentTopPosition[currentScreenInt] + 1)
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
        string[] itemsString = new string[8];
        ItemDataOld[] items = new ItemDataOld[8];

        int index = 0;
        for (int i = 0; i < 8; i++)
        {
            index = i + currentTopPosition[currentScreenInt] - 1; //Why is there a minus one?
            //not sure we need this IF here
            if (index < 0 || index >= currentItemStringList.Length)
            {
                itemsString[i] = null;
            }
            else
            {
                itemsString[i] = currentItemStringList[index];
                items[i] = currentItemList[index];
            }
            itemSlot[i].gameObject.SetActive(true);
            itemNameShadow[i].text = itemName[i].text = items[i].getName()??null;//itemsString[i]; Just seeing if shorthand was possible
            //a switch would be perfect here, but cant make sense of everything yet
            switch (items[i].getItemPocket()) {
                /*case null: //it'll use default value; cant be null
                    itemSlot[i].gameObject.SetActive(false);
                    break;*/
                case ItemDataOld.ItemPockets.MISC:
                    itemX[i].gameObject.SetActive(true);
                    if (shopMode && currentScreenInt == 1)
                    {
                        itemX[i].text = "$";
                        itemQuantity[i].text = items[i].getPrice().ToString();
                    }
                    else
                    {
                        itemX[i].text = "   x";
                        itemQuantity[i].text = SaveDataOld.currentSave.Bag.getQuantity(items[i]).ToString();
                    }
                    itemXShadow[i].text = itemX[i].text;
                    break;
                case ItemDataOld.ItemPockets.BATTLE:
                case ItemDataOld.ItemPockets.BERRY:
                case ItemDataOld.ItemPockets.MAIL:
                case ItemDataOld.ItemPockets.MEDICINE:
                case ItemDataOld.ItemPockets.POKEBALL:
                case ItemDataOld.ItemPockets.MACHINE:
                    itemX[i].gameObject.SetActive(false);
                    itemQuantityShadow[i].text = itemQuantity[i].text = "No. " + items[i].getTMNo();
                    //ToDo: Easier icon look-up
                    itemIcon[i].texture = Resources.Load<Texture>("Items/tm" + MoveDatabase.getMove(items[i].getName()).getType().ToString());
                    break;
                case ItemDataOld.ItemPockets.KEY: 
                    itemX[i].gameObject.SetActive(false);
                    //if (items[i].getItemFlags()) { } else//if countable else quantity null
                    itemQuantityShadow[i].text = itemQuantity[i].text = string.Empty;
                    //ToDo: Easier icon look-up
                    //itemIcon[i].texture = Resources.Load<Texture>("Items/tm" + MoveDatabase.getMove(items[i].getName()).getType().ToString());
                    break;
                default:
                    itemNameShadow[i].text = itemName[i].text = null; //items[i].getName();
                    itemSlot[i].gameObject.SetActive(false);
                    break;
            }
            /*if (itemsString[i] == null)
            {
                itemSlot[i].gameObject.SetActive(false);
            }
            else
            {
                ItemData item = ItemDatabase.getItem(itemsString[i]);
                itemSlot[i].gameObject.SetActive(true);
                itemName[i].text = items[i].getName();//itemsString[i];
                itemNameShadow[i].text = itemName[i].text;//itemsString[i];
                if (item.getItemType() == ItemData.ItemType.TM)
                {
                    itemIcon[i].texture =
                        Resources.Load<Texture>("Items/tm" + MoveDatabase.getMove(item.getName()).getType().ToString());
                }
                else
                {
                    itemIcon[i].texture = Resources.Load<Texture>("Items/" + itemsString[i]);
                }
                if (item.getItemType() == ItemData.ItemType.TM)
                {
                    itemX[i].gameObject.SetActive(false);
                    itemQuantity[i].text = "No. " + item.getTMNo();
                }
                else if (item.getItemType() == ItemData.ItemType.KEY)
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
                        itemQuantity[i].text = "" + item.getPrice();
                    }
                    else
                    {
                        itemX[i].text = "   x";
                        itemQuantity[i].text = "" + SaveData.currentSave.Bag.getQuantity(itemsString[i]);
                    }
                    itemXShadow[i].text = itemX[i].text;
                }
                itemQuantityShadow[i].text = itemQuantity[i].text;
            }*/
        }
    }

    private void updateDescription()
    {
        //resolve the index of the current item by adding the top and visible positions, minus 1.
        int index = currentPosition[currentScreenInt] + currentTopPosition[currentScreenInt] - 1;
        if (index < currentItemStringList.Length)
        {
            string selectedItem = currentItemStringList[index];
            if (currentScreenInt != 4)
            {
                itemDescription.text = ItemDatabaseOld.getItem(selectedItem).getDescription();
                itemDescriptionShadow.text = itemDescription.text;
            }
            else if (!shopMode)
            {
                MoveData selectedTM = MoveDatabase.getMove(selectedItem);
                tmType.texture = Resources.Load<Texture>("PCSprites/type" + selectedTM.getType().ToString());
                tmCategory.texture = Resources.Load<Texture>("PCSprites/category" + selectedTM.getCategory().ToString());
                tmPower.text = "" + selectedTM.getPower();
                if (tmPower.text == "0")
                {
                    tmPower.text = "-";
                }
                tmPowerShadow.text = tmPower.text;
                tmAccuracy.text = "" + Mathf.Round(selectedTM.getAccuracy() * 100f);
                if (tmAccuracy.text == "0")
                {
                    tmAccuracy.text = "-";
                }
                tmAccuracyShadow.text = tmAccuracy.text;
                tmDescription.text = ItemDatabaseOld.getItem(selectedItem).getDescription();
                tmDescriptionShadow.text = tmDescription.text;
            }
        }
        else
        {
            if (currentScreenInt != 4)
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
        if (currentScreenInt == 4 && !shopMode)
        {
            itemDescription.gameObject.SetActive(false);
            visableSlots = 5;
            currentItemStringList = SaveDataOld.currentSave.Bag.getItemTypeArray(ItemDataOld.ItemType.TM);
        }
        else
        {
            itemDescription.gameObject.SetActive(true);
            visableSlots = 6;
            if (!shopMode)
            {
                if (currentScreenInt == 1)
                {
                    currentItemStringList = SaveDataOld.currentSave.Bag.getItemTypeArray(ItemDataOld.ItemType.ITEM);
                }
                else if (currentScreenInt == 2)
                {
                    currentItemStringList = SaveDataOld.currentSave.Bag.getItemTypeArray(ItemDataOld.ItemType.MEDICINE);
                }
                else if (currentScreenInt == 3)
                {
                    currentItemStringList = SaveDataOld.currentSave.Bag.getItemTypeArray(ItemDataOld.ItemType.BERRY);
                }
                else if (currentScreenInt == 5)
                {
                    currentItemStringList = SaveDataOld.currentSave.Bag.getItemTypeArray(ItemDataOld.ItemType.KEY);
                }
            }
            else
            {
                if (currentScreenInt == 1)
                {
                    currentItemStringList = shopItemList;
                }
                else if (currentScreenInt == 2)
                {
                    currentItemStringList = SaveDataOld.currentSave.Bag.getSellableItemArray();
                }
                else if (currentScreenInt == 3)
                {
                    currentItemStringList = SaveDataOld.currentSave.Bag.getItemTypeArray(ItemDataOld.ItemType.ITEM);
                }
                else if (currentScreenInt == 4)
                {
                    currentItemStringList = SaveDataOld.currentSave.Bag.getItemTypeArray(ItemDataOld.ItemType.MEDICINE);
                }
                else if (currentScreenInt == 5)
                {
                    currentItemStringList = SaveDataOld.currentSave.Bag.getItemTypeArray(ItemDataOld.ItemType.BERRY);
                }
            }
        }
        if (!shopMode)
        {
            screens[currentScreenInt].SetActive(true);
            shopScreens[currentScreenInt].SetActive(false);
        }
        else
        {
            shopScreens[currentScreenInt].SetActive(true);
            screens[currentScreenInt].SetActive(false);
        }
        for (int i = 1; i < 6; i++)
        {
            if (i != currentScreenInt)
            {
                screens[i].SetActive(false);
                shopScreens[i].SetActive(false);
            }
        }
    }

    private IEnumerator scrollScrollBar(int direction)
    {
        float maxBarHeight = 78f;
        if (currentScreenInt == 4 && !shopMode)
        {
            maxBarHeight = 62;
        }
        float barInterval = ((maxBarHeight - scrollBar.pixelInset.height) / (currentItemStringList.Length - visableSlots));

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
        int currentTopPositionOnStart = currentTopPosition[currentScreenInt];

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
        PokemonOld currentPokemon;
        for (int i = 0; i < 6; i++)
        {
            currentPokemon = SaveDataOld.currentSave.PC.boxes[0][i];
            if (currentPokemon == null)
            {
                partySlot[i].gameObject.SetActive(false);
            }
            else
            {
                partyIcon[i].texture = currentPokemon.GetIcons();

                partyHPBar[i].pixelInset = new Rect(partyHPBar[i].pixelInset.x, partyHPBar[i].pixelInset.y,
                    Mathf.FloorToInt(48f * ((float) currentPokemon.getCurrentHP() / (float) currentPokemon.getHP())),
                    partyHPBar[i].pixelInset.height);

                if ((float) currentPokemon.getCurrentHP() < ((float) currentPokemon.getHP() / 4f))
                {
                    partyHPBar[i].color = new Color(1, 0.125f, 0, 1);
                }
                else if ((float) currentPokemon.getCurrentHP() < ((float) currentPokemon.getHP() / 2f))
                {
                    partyHPBar[i].color = new Color(1, 0.75f, 0, 1);
                }
                else
                {
                    partyHPBar[i].color = new Color(0.125f, 1, 0.065f, 1);
                }

                partyName[i].text = currentPokemon.getName();
                partyNameShadow[i].text = partyName[i].text;
                if (currentPokemon.getGender() == PokemonOld.Gender.FEMALE)
                {
                    partyGender[i].text = "♀";
                    partyGender[i].color = new Color(1, 0.2f, 0.2f, 1);
                }
                else if (currentPokemon.getGender() == PokemonOld.Gender.MALE)
                {
                    partyGender[i].text = "♂";
                    partyGender[i].color = new Color(0.2f, 0.4f, 1, 1);
                }
                else
                {
                    partyGender[i].text = null;
                }
                partyGenderShadow[i].text = partyGender[i].text;
                partyLevel[i].text = "" + currentPokemon.getLevel();
                partyLevelShadow[i].text = partyLevel[i].text;
                if (currentPokemon.getStatus() != PokemonOld.Status.NONE)
                {
                    partyStatus[i].texture =
                        Resources.Load<Texture>("PCSprites/status" + currentPokemon.getStatus().ToString());
                }
                else
                {
                    partyStatus[i].texture = null;
                }
                if (!string.IsNullOrEmpty(currentPokemon.getHeldItem()))
                {
                    partyItem[i].enabled = true;
                }
                else
                {
                    partyItem[i].enabled = false;
                }
            }
        }
        partyLength = SaveDataOld.currentSave.PC.getBoxLength(0);
    }

    private void updatePartyDisplays()
    {
        if (currentItemStringList.Length > 0)
        {
            int loopBreaker = 200;
            while (currentPosition[currentScreenInt] + currentTopPosition[currentScreenInt] - 1 >= currentItemStringList.Length &&
                   loopBreaker > 0)
            {
                Debug.Log("" + currentItemStringList.Length + " " + currentPosition[currentScreenInt] + " " +
                          (currentTopPosition[currentScreenInt] - 1));
                if (currentPosition[currentScreenInt] > 1)
                {
                    if (currentItemStringList.Length < visableSlots)
                    {
                        currentPosition[currentScreenInt] -= 1;
                        currentTopPosition[currentScreenInt] = 0;
                    }
                    else
                    {
                        currentTopPosition[currentScreenInt] -= 1;
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

            ItemDataOld selectedItem =
                ItemDatabaseOld.getItem(
                    currentItemStringList[currentPosition[currentScreenInt] + currentTopPosition[currentScreenInt] - 1]);
            if (selectedItem.getItemEffect() == ItemDataOld.ItemEffect.EVOLVE)
            {
                for (int i = 0; i < 6; i++)
                {
                    partyStandardDisplay[i].SetActive(false);
                    partyTextDisplay[i].gameObject.SetActive(true);
                    if (SaveDataOld.currentSave.PC.boxes[0][i] != null)
                    {
                        if (SaveDataOld.currentSave.PC.boxes[0][i].canEvolve("Stone," + selectedItem.getName()))
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
            else if (selectedItem.getItemEffect() == ItemDataOld.ItemEffect.TM)
            {
                for (int i = 0; i < 6; i++)
                {
                    partyStandardDisplay[i].SetActive(false);
                    partyTextDisplay[i].gameObject.SetActive(true);
                    if (SaveDataOld.currentSave.PC.boxes[0][i] != null)
                    {
                        if (SaveDataOld.currentSave.PC.boxes[0][i].HasMove(selectedItem.getName()))
                        {
                            partyTextDisplay[i].text = "LEARNED!";
                            partyTextDisplay[i].color = new Color(1, 1, 1, 1);
                        }
                        else if (SaveDataOld.currentSave.PC.boxes[0][i].CanLearnMove(selectedItem.getName()))
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
            currentPosition[currentScreenInt] = 1;
            currentTopPosition[currentScreenInt] = 0;
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
            string playerMoney = "" + SaveDataOld.currentSave.playerMoney;
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
            if (currentScreenInt == 1)
            {
                dataText.text = "In Bag:";
                if (currentItemStringList.Length > 0)
                {
                    dataValueText.text = "x " +
                                         SaveDataOld.currentSave.Bag.getQuantity(
                                             currentItemStringList[
                                                 currentPosition[currentScreenInt] + currentTopPosition[currentScreenInt] - 1]);
                }
                else
                {
                    dataValueText.text = "-";
                }
            }
            else
            {
                dataText.text = "Offer:";
                if (currentItemStringList.Length > 0)
                {
                    dataValueText.text = "$" +
                                         Mathf.Floor(
                                             (float)
                                             ItemDatabaseOld.getItem(
                                                 currentItemStringList[
                                                     currentPosition[currentScreenInt] + currentTopPosition[currentScreenInt] -
                                                     1]).getPrice() / 2f);
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


    private bool addItem(string itemName, int amount)
    {
        bool result = SaveDataOld.currentSave.Bag.addItem(itemName, amount);
        if (result)
        {
            ItemDataOld.ItemType itemType = ItemDatabaseOld.getItem(itemName).getItemType();
            bool relevantScreen = false;
            if (currentScreenInt == 1 && itemType == ItemDataOld.ItemType.ITEM)
            {
                relevantScreen = true;
            }
            else if (currentScreenInt == 2 && itemType == ItemDataOld.ItemType.MEDICINE)
            {
                relevantScreen = true;
            }
            else if (currentScreenInt == 3 && itemType == ItemDataOld.ItemType.BERRY)
            {
                relevantScreen = true;
            }
            else if (currentScreenInt == 4 && itemType == ItemDataOld.ItemType.TM)
            {
                relevantScreen = true;
            }
            else if (currentScreenInt == 5 && itemType == ItemDataOld.ItemType.KEY)
            {
                relevantScreen = true;
            }
            if (SaveDataOld.currentSave.Bag.getQuantity(itemName) == amount && relevantScreen)
            {
                if (currentPosition[currentScreenInt] > Mathf.Floor(visableSlots * 0.67f))
                {
                    currentTopPosition[currentScreenInt] += 1;
                    currentPosition[currentScreenInt] -= 1;
                }
            }
        }
        return result;
    }

    private bool removeItem(string itemName, int amount)
    {
        bool result = SaveDataOld.currentSave.Bag.removeItem(itemName, amount);
        if (result)
        {
            if (SaveDataOld.currentSave.Bag.getQuantity(itemName) == 0)
            {
                switching = false;
                selected = -1;

                updateScreen();

                if (visableSlots + currentTopPosition[currentScreenInt] + 1 > currentItemStringList.Length)
                {
                    if (currentPosition[currentScreenInt] > 1)
                    {
                        if (currentItemStringList.Length < visableSlots)
                        {
                            currentPosition[currentScreenInt] -= 1;
                        }
                        else
                        {
                            currentTopPosition[currentScreenInt] -= 1;
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
            shopNameShadow.gameObject.SetActive(true);
            //sceneTransition.FadeIn(0);
            ScreenFade.main.SetToFadedIn();
        }
        else
        {
            party.gameObject.SetActive(true);
            moneyBox.SetActive(false);
            dataBox.SetActive(false);
            shopName.gameObject.SetActive(false);
            shopNameShadow.gameObject.SetActive(false);
            //sceneTransition.FadeIn();
            StartCoroutine(ScreenFade.main.Fade(true, ScreenFade.defaultSpeed));
        }
        numbersBox.SetActive(false);

        chosenItem = "";

        currentScreenInt = 1;
        currentPosition = new int[]
        {
            -1, 1, 1, 1, 1, 1
        };
        currentTopPosition = new int[]
        {
            -1, 0, 0, 0, 0, 0
        };
        currentItemStringList = SaveDataOld.currentSave.Bag.getItemTypeArray(ItemDataOld.ItemType.ITEM);
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
                    if (currentPosition[currentScreenInt] > 1)
                    {
                        currentPosition[currentScreenInt] -= 1;
                        if (currentTopPosition[currentScreenInt] > 0 && currentPosition[currentScreenInt] < 3)
                        {
                            currentPosition[currentScreenInt] += 1;
                            StartCoroutine(animateSelection(currentPosition[currentScreenInt] - 1,
                                currentPosition[currentScreenInt]));
                            currentTopPosition[currentScreenInt] -= 1;
                            StartCoroutine("scrollScrollBar", -1);
                        }
                        else
                        {
                            StartCoroutine(animateSelection(currentPosition[currentScreenInt],
                                currentPosition[currentScreenInt] + 1));
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
                    if (currentPosition[currentScreenInt] < visableSlots &&
                        currentPosition[currentScreenInt] < currentItemStringList.Length)
                    {
                        currentPosition[currentScreenInt] += 1;
                        if (currentTopPosition[currentScreenInt] < currentItemStringList.Length - visableSlots &&
                            currentPosition[currentScreenInt] > Mathf.Floor(visableSlots * 0.67f))
                        {
                            currentPosition[currentScreenInt] -= 1;
                            StartCoroutine(animateSelection(currentPosition[currentScreenInt] + 1,
                                currentPosition[currentScreenInt]));
                            currentTopPosition[currentScreenInt] += 1;
                            StartCoroutine("scrollScrollBar", 1);
                        }
                        else
                        {
                            StartCoroutine(animateSelection(currentPosition[currentScreenInt],
                                currentPosition[currentScreenInt] - 1));
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
                    if (currentScreenInt < 5)
                    {
                        currentScreenInt += 1;
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
                        currentScreenInt = 2;
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
                    if (currentScreenInt > 1)
                    {
                        if (!shopMode)
                        {
                            currentScreenInt -= 1;
                        }
                        else
                        {
                            currentScreenInt = 1;
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

                    currentPosition[currentScreenInt] = selectedPosition;
                    currentTopPosition[currentScreenInt] = selectedTopPosition;

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
                if (!switching && currentItemStringList.Length > 0)
                {
                    if (!inParty)
                    {
                        if (shopMode)
                        {
                            selected = currentPosition[currentScreenInt] + currentTopPosition[currentScreenInt] - 1;
                            ItemDataOld selectedItem = ItemDatabaseOld.getItem(currentItemStringList[selected]);

                            switching = true;
                            selectedPosition = currentPosition[currentScreenInt];
                            selectedTopPosition = currentTopPosition[currentScreenInt];
                            updateSelectedItem();
                            SfxHandler.Play(selectClip);

                            chosenItem = currentItemStringList[selected];

                            if (currentScreenInt == 1)
                            {
                                //BUY
                                //custom prices not yet implemented
                                if (SaveDataOld.currentSave.playerMoney >= selectedItem.getPrice())
                                {
                                    Dialog.drawDialogBox(2);
                                    yield return
                                        StartCoroutine(
                                            Dialog.drawText(selectedItem.getName() +
                                                            "? Certainly. \nHow many would you like to buy?"));

                                    //quantity selection not yet implemented
                                    int maxQuantity =
                                        Mathf.FloorToInt((float) SaveDataOld.currentSave.playerMoney /
                                                         (float) selectedItem.getPrice());
                                    Debug.Log(maxQuantity);
                                    yield return StartCoroutine(openNumbersBox(selectedItem.getPrice(), maxQuantity));

                                    if (shopSelectedQuantity > 0)
                                    {
                                        Dialog.drawDialogBox(2);
                                        yield return
                                            StartCoroutine(
                                                Dialog.drawText(selectedItem.getName() + ", and you want " +
                                                                shopSelectedQuantity + ". \nThat will be $" +
                                                                (shopSelectedQuantity * selectedItem.getPrice()) +
                                                                ". OK?"));
                                        Dialog.drawChoiceBox(14);
                                        yield return StartCoroutine(Dialog.choiceNavigate());
                                        int chosenIndex2 = Dialog.chosenIndex;
                                        Dialog.undrawDialogBox();
                                        Dialog.undrawChoiceBox();

                                        if (chosenIndex2 == 1)
                                        {
                                            SfxHandler.Play(saleClip);

                                            addItem(selectedItem.getName(), shopSelectedQuantity);
                                            SaveDataOld.currentSave.playerMoney -= (shopSelectedQuantity *
                                                                                 selectedItem.getPrice());

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
                                        Dialog.drawText(selectedItem.getName() + "? \nHow many would you like to sell?"))
                                    ;

                                //quantity selection not yet implemented
                                int maxQuantity = SaveDataOld.currentSave.Bag.getQuantity(selectedItem.getName());
                                Debug.Log(maxQuantity);
                                yield return
                                    StartCoroutine(openNumbersBox(
                                        Mathf.FloorToInt((float) selectedItem.getPrice() / 2f), maxQuantity));

                                if (shopSelectedQuantity > 0)
                                {
                                    Dialog.drawDialogBox(2);
                                    yield return
                                        StartCoroutine(
                                            Dialog.drawText("I can pay $" +
                                                            (shopSelectedQuantity *
                                                             Mathf.FloorToInt((float) selectedItem.getPrice() / 2f)) +
                                                            ". \nWould that be OK?"));
                                    Dialog.drawChoiceBox(14);
                                    yield return StartCoroutine(Dialog.choiceNavigate());
                                    int chosenIndex2 = Dialog.chosenIndex;
                                    Dialog.undrawDialogBox();
                                    Dialog.undrawChoiceBox();

                                    if (chosenIndex2 == 1)
                                    {
                                        SfxHandler.Play(saleClip);

                                        removeItem(selectedItem.getName(), shopSelectedQuantity);
                                        //custom prices not yet implemented
                                        SaveDataOld.currentSave.playerMoney += shopSelectedQuantity *
                                                                            Mathf.FloorToInt(
                                                                                (float) selectedItem.getPrice() / 2f);

                                        updateMoneyBox();
                                        updateDataBox();

                                        Dialog.drawDialogBox(2);
                                        if (shopSelectedQuantity == 1)
                                        {
                                            yield return
                                                StartCoroutine(
                                                    Dialog.drawTextSilent("Turned over the " + selectedItem.getName() +
                                                                          " \nand received $" +
                                                                          (shopSelectedQuantity *
                                                                           Mathf.FloorToInt(
                                                                               (float) selectedItem.getPrice() / 2f)) +
                                                                          "."));
                                        }
                                        else
                                        {
                                            yield return
                                                StartCoroutine(
                                                    Dialog.drawTextSilent("Turned over the " + selectedItem.getName() +
                                                                          "s \nand received $" +
                                                                          (shopSelectedQuantity *
                                                                           Mathf.FloorToInt(
                                                                               (float) selectedItem.getPrice() / 2f)) +
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
                            selectedPosition = currentPosition[currentScreenInt];
                            selectedTopPosition = currentTopPosition[currentScreenInt];

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
                            selected = currentPosition[currentScreenInt] + currentTopPosition[currentScreenInt] - 1;
                            ItemDataOld selectedItem = ItemDatabaseOld.getItem(currentItemStringList[selected]);

                            if (selectedItem.getItemType() == ItemDataOld.ItemType.TM ||
                                selectedItem.getItemType() == ItemDataOld.ItemType.KEY)
                            {
                                Dialog.drawDialogBox();
                                yield return
                                    StartCoroutine(Dialog.drawText("The " + selectedItem.getName() + " can't be held."))
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
                                selectedPosition = currentPosition[currentScreenInt];
                                selectedTopPosition = currentTopPosition[currentScreenInt];
                                updateSelectedItem();
                                SfxHandler.Play(selectClip);

                                chosenItem = currentItemStringList[selected];
                                running = false;
                            }
                        }
                        else
                        {
                            switching = true;
                            selected = currentPosition[currentScreenInt] + currentTopPosition[currentScreenInt] - 1;
                            selectedPosition = currentPosition[currentScreenInt];
                            selectedTopPosition = currentTopPosition[currentScreenInt];
                            updateSelectedItem();
                            SfxHandler.Play(selectClip);
                            yield return new WaitForSeconds(0.2f);
                        }
                    }
                    else
                    {
                        //display options for Pokemon on their own.
                        if (SaveDataOld.currentSave.PC.boxes[0][partyPosition].getHeldItem().Length > 0)
                        {
                            Dialog.drawDialogBox();
                            yield return
                                Dialog.StartCoroutine("drawText",
                                    "Take " + SaveDataOld.currentSave.PC.boxes[0][partyPosition].getName() + "'s Item?");
                            Dialog.drawChoiceBox();
                            yield return Dialog.StartCoroutine("choiceNavigate");
                            int chosenIndex = Dialog.chosenIndex;
                            Dialog.undrawDialogBox();
                            Dialog.undrawChoiceBox();
                            if (chosenIndex == 1)
                            {
                                string receivedItem = SaveDataOld.currentSave.PC.boxes[0][partyPosition].swapHeldItem("");
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
                                        SaveDataOld.currentSave.PC.boxes[0][partyPosition].getName() + ".");
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
                    if (!inParty && currentItemStringList.Length > 0)
                    {
                        //display options for selected item
                        if (currentPosition[currentScreenInt] == selectedPosition &&
                            currentTopPosition[currentScreenInt] == selectedTopPosition)
                        {
                            Dialog.drawDialogBox();
                            yield return
                                Dialog.StartCoroutine("drawText", "Do what with " + currentItemStringList[selected] + "?");
                            string[] choices = new string[] {"Deselect", "Cancel"};
                            ItemDataOld selectedItem = ItemDatabaseOld.getItem(currentItemStringList[selected]);
                            if (selectedItem.getItemType() == ItemDataOld.ItemType.KEY)
                            {
                                if (selectedItem.getItemEffect() == ItemDataOld.ItemEffect.UNIQUE)
                                {
                                    choices = new string[]
                                    {
                                        "Use", "Register", "Deselect", "Cancel"
                                    };
                                }
                            }
                            else if (selectedItem.getItemEffect() != ItemDataOld.ItemEffect.NONE &&
                                     selectedItem.getItemEffect() != ItemDataOld.ItemEffect.FLEE &&
                                     selectedItem.getItemEffect() != ItemDataOld.ItemEffect.BALL &&
                                     selectedItem.getItemEffect() != ItemDataOld.ItemEffect.STATBOOST)
                            {
                                choices = new string[]
                                {
                                    "Use", "Deselect", "Cancel"
                                };
                            }

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
                                if (selectedItem.getItemType() == ItemDataOld.ItemType.KEY)
                                {
                                    //REGISTER (key item)
                                    Dialog.drawDialogBox();
                                    yield return
                                        Dialog.StartCoroutine("drawText",
                                            "Register " + selectedItem.getName() + " to which slot?");
                                    choices = new string[]
                                    {
                                        "Slot 1", "Slot 2", "Slot 3", "Slot 4", "Cancel"
                                    };
                                    Dialog.drawChoiceBox(choices);
                                    yield return StartCoroutine(Dialog.choiceNavigate(choices));
                                    chosenIndex = Dialog.chosenIndex;
                                    if (chosenIndex == 4)
                                    {
                                        SaveDataOld.currentSave.registeredItems[0] = selectedItem.getName();
                                    }
                                    else if (chosenIndex == 3)
                                    {
                                        SaveDataOld.currentSave.registeredItems[1] = selectedItem.getName();
                                    }
                                    else if (chosenIndex == 2)
                                    {
                                        SaveDataOld.currentSave.registeredItems[2] = selectedItem.getName();
                                    }
                                    else if (chosenIndex == 1)
                                    {
                                        SaveDataOld.currentSave.registeredItems[3] = selectedItem.getName();
                                    }
                                }
                                else
                                {
                                    //USE
                                    if (selectedItem.getItemEffect() == ItemDataOld.ItemEffect.UNIQUE)
                                    {
                                    }
                                    else if (partyLength > 0)
                                    {
                                        bool selectingPokemon = true;
                                        Dialog.drawDialogBox();
                                        if (selectedItem.getItemType() != ItemDataOld.ItemType.TM)
                                        {
                                            yield return
                                                Dialog.StartCoroutine("drawText",
                                                    "Use " + selectedItem.getName() + " on which Pokémon?");
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
                                                    "It contained " + selectedItem.getName() + ".");
                                            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                                            {
                                                yield return null;
                                            }

                                            Dialog.drawDialogBox(2);
                                            yield return
                                                Dialog.StartCoroutine("drawText",
                                                    "Teach " + selectedItem.getName() + " \nto a Pokémon?");
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
                                                        "Who should learn \n" + selectedItem.getName() + "?");
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
                                                PokemonOld currentPokemon = SaveDataOld.currentSave.PC.boxes[0][partyPosition];

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

                            SaveDataOld.currentSave.Bag.moveBehind(
                                SaveDataOld.currentSave.Bag.getIndexOf(currentItemStringList[selected]),
                                SaveDataOld.currentSave.Bag.getIndexOf(
                                    currentItemStringList[
                                        currentPosition[currentScreenInt] + currentTopPosition[currentScreenInt] - 1]));

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
                        PokemonOld currentPokemon = SaveDataOld.currentSave.PC.boxes[0][partyPosition];

                        Dialog.drawDialogBox();
                        yield return
                            Dialog.StartCoroutine("drawText", "Do what with " + currentItemStringList[selected] + "?");
                        string[] choices = new string[] {"Give", "Deselect", "Cancel"};
                        if (!string.IsNullOrEmpty(currentPokemon.getHeldItem()))
                        {
                            choices = new string[]
                            {
                                "Swap", "Take", "Deselect", "Cancel"
                            };
                        }
                        ItemDataOld selectedItem = ItemDatabaseOld.getItem(currentItemStringList[selected]);
                        //Adjust choices to suit situation
                        if (selectedItem.getItemType() != ItemDataOld.ItemType.KEY)
                        {
                            if (selectedItem.getItemEffect() == ItemDataOld.ItemEffect.HP ||
                                selectedItem.getItemEffect() == ItemDataOld.ItemEffect.PP ||
                                selectedItem.getItemEffect() == ItemDataOld.ItemEffect.STATUS ||
                                selectedItem.getItemEffect() == ItemDataOld.ItemEffect.EV ||
                                selectedItem.getItemEffect() == ItemDataOld.ItemEffect.EVOLVE)
                            {
                                if (string.IsNullOrEmpty(currentPokemon.getHeldItem()))
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
                            else if (selectedItem.getItemEffect() == ItemDataOld.ItemEffect.TM)
                            {
                                choices = new string[]
                                {
                                    "Use", "Deselect", "Cancel"
                                };
                            }
                            else if (selectedItem.getItemEffect() == ItemDataOld.ItemEffect.UNIQUE)
                            {
                                if (selectedItem.getName() == "Rare Candy")
                                {
                                    if (string.IsNullOrEmpty(currentPokemon.getHeldItem()))
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
                            }
                        }

                        Dialog.drawChoiceBox(choices);
                        yield return StartCoroutine(Dialog.choiceNavigate(choices));
                        int chosenIndex = Dialog.chosenIndex;
                        Dialog.undrawDialogBox();
                        Dialog.undrawChoiceBox();

                        string chosenChoice = choices[choices.Length - chosenIndex - 1];

                        if (chosenChoice == "Give")
                        {
                            currentPokemon.swapHeldItem(selectedItem.getName());
                            removeItem(selectedItem.getName(), 1);
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
                                    "Gave " + selectedItem.getName() + " to " + currentPokemon.getName() + ".");
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
                            string receivedItem = currentPokemon.swapHeldItem("");
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
                                    "Took the " + receivedItem + " from " + currentPokemon.getName() + ".");
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
                            string receivedItem = currentPokemon.swapHeldItem(selectedItem.getName());
                            addItem(receivedItem, 1);
                            removeItem(selectedItem.getName(), 1);
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
                                    "Gave " + selectedItem.getName() + " to " + currentPokemon.getName() + ",");
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
    private IEnumerator runItemEffect(ItemDataOld selectedItem, PokemonOld currentPokemon)
    {
        yield return StartCoroutine(runItemEffect(selectedItem, currentPokemon, false));
    }

    private IEnumerator runItemEffect(ItemDataOld selectedItem, PokemonOld currentPokemon, bool booted)
    {
        if (selectedItem.getItemEffect() == ItemDataOld.ItemEffect.HP)
        {
            //HP
            if (currentPokemon.getCurrentHP() < currentPokemon.getHP() &&
                currentPokemon.getStatus() != PokemonOld.Status.FAINTED)
            {
                //determine amount / intialise HP Bar Animation variables
                float amount = selectedItem.getFloatParameter();
                if (amount <= 1)
                {
                    amount = currentPokemon.healHP(currentPokemon.getHP() * amount);
                }
                else
                {
                    amount = currentPokemon.healHP(amount);
                }
                float startLength = partyHPBar[partyPosition].pixelInset.width;
                float difference =
                    Mathf.Floor(48f * ((float) currentPokemon.getCurrentHP() / (float) currentPokemon.getHP())) -
                    startLength;
                float increment = 0;
                float speed = 0.5f;

                SfxHandler.Play(healClip);
                removeItem(selectedItem.getName(), 1);

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
        else if (selectedItem.getItemEffect() == ItemDataOld.ItemEffect.PP)
        {
            //PP
            string[] currentPokemonMoveset = currentPokemon.getMoveset();
            int[] currentPokemonPP = currentPokemon.getPP();
            int[] currentPokemonMaxPP = currentPokemon.getMaxPP();
            bool loweredPP = false;
            for (int i = 0; i < 4; i++)
            {
                if (currentPokemonMoveset[i] != null)
                {
                    if (currentPokemonPP[i] < currentPokemonMaxPP[i])
                    {
                        loweredPP = true;
                    }
                    else
                    {
                        //remove any move from the array that has full PP.
                        currentPokemonMoveset[i] = null;
                    }
                }
            }
            if (loweredPP)
            {
                //check that the item is not healing all PP
                if (selectedItem.getStringParameter() == "All")
                {
                    //heal PP for each move.
                    float amount = selectedItem.getFloatParameter();
                    for (int i = 0; i < 4; i++)
                    {
                        if (currentPokemonMoveset[i] != null)
                        {
                            if (amount <= 1)
                            {
                                amount = currentPokemon.healPP(i,
                                    Mathf.RoundToInt(currentPokemon.getMaxPP()[i] * amount));
                            }
                            else
                            {
                                amount = currentPokemon.healPP(i, amount);
                            }
                        }
                    }

                    SfxHandler.Play(healClip);
                    removeItem(selectedItem.getName(), 1);

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
                        currentPokemonMoveset[0], currentPokemonMoveset[1],
                        currentPokemonMoveset[2], currentPokemonMoveset[3], "Cancel"
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
                            if (currentPokemonMoveset[i] == choices[choices.Length - chosenIndex - 1])
                            {
                                moveNumber = i;
                            }
                        }

                        //heal PP for selected move.
                        float amount = selectedItem.getFloatParameter();
                        if (amount <= 1)
                        {
                            amount = currentPokemon.healPP(moveNumber,
                                Mathf.RoundToInt(currentPokemon.getMaxPP()[moveNumber] * amount));
                        }
                        else
                        {
                            amount = currentPokemon.healPP(moveNumber, amount);
                        }

                        SfxHandler.Play(healClip);
                        removeItem(selectedItem.getName(), 1);

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
        else if (selectedItem.getItemEffect() == ItemDataOld.ItemEffect.STATUS)
        {
            //STATUS
            //Check current pokemon has the status the item cures
            string statusCurer = selectedItem.getStringParameter().ToUpper();
            //if an ALL is used, set it to cure anything but FAINTED or NONE.
            if (statusCurer == "ALL" && currentPokemon.getStatus().ToString() != "FAINTED" &&
                currentPokemon.getStatus().ToString() != "NONE")
            {
                statusCurer = currentPokemon.getStatus().ToString();
            }

            if (currentPokemon.getStatus().ToString() == statusCurer)
            {
                if (statusCurer == "FAINTED")
                {
                    //Revive
                    currentPokemon.setStatus(PokemonOld.Status.NONE);

                    //determine amount / intialise HP Bar Animation variables
                    float amount = selectedItem.getFloatParameter();
                    if (amount <= 1)
                    {
                        amount = currentPokemon.healHP(currentPokemon.getHP() * amount);
                    }
                    else
                    {
                        amount = currentPokemon.healHP(amount);
                    }
                    float startLength = partyHPBar[partyPosition].pixelInset.width;
                    float difference =
                        Mathf.Floor(48f * ((float) currentPokemon.getCurrentHP() / (float) currentPokemon.getHP())) -
                        startLength;
                    float increment = 0;
                    float speed = 0.5f;

                    SfxHandler.Play(healClip);
                    removeItem(selectedItem.getName(), 1);

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
                    currentPokemon.setStatus(PokemonOld.Status.NONE);

                    SfxHandler.Play(healClip);
                    removeItem(selectedItem.getName(), 1);

                    Dialog.drawDialogBox();
                    if (statusCurer == "ASLEEP")
                    {
                        yield return Dialog.StartCoroutine("drawTextSilent", currentPokemon.getName() + " woke up!");
                    }
                    else if (statusCurer == "BURNED")
                    {
                        yield return Dialog.StartCoroutine("drawTextSilent", currentPokemon.getName() + " was healed!");
                    }
                    else if (statusCurer == "FROZEN")
                    {
                        yield return Dialog.StartCoroutine("drawTextSilent", currentPokemon.getName() + " thawed out!");
                    }
                    else if (statusCurer == "PARALYZED")
                    {
                        yield return Dialog.StartCoroutine("drawTextSilent", currentPokemon.getName() + " was cured!");
                    }
                    else if (statusCurer == "POISONED")
                    {
                        yield return Dialog.StartCoroutine("drawTextSilent", currentPokemon.getName() + " was cured!");
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
        else if (selectedItem.getItemEffect() == ItemDataOld.ItemEffect.EV)
        {
            //EV
            string statBooster = selectedItem.getStringParameter().ToUpper();
            float amount = selectedItem.getFloatParameter();
            bool evsAdded = currentPokemon.addEVs(statBooster, amount);
            if (evsAdded)
            {
                SfxHandler.Play(healClip);
                removeItem(selectedItem.getName(), 1);

                Dialog.drawDialogBox();
                if (statBooster == "HP")
                {
                    yield return Dialog.StartCoroutine("drawTextSilent", currentPokemon.getName() + "'s HP rose!");
                }
                else if (statBooster == "ATK")
                {
                    yield return Dialog.StartCoroutine("drawTextSilent", currentPokemon.getName() + "'s Attack rose!");
                }
                else if (statBooster == "DEF")
                {
                    yield return Dialog.StartCoroutine("drawTextSilent", currentPokemon.getName() + "'s Defense rose!");
                }
                else if (statBooster == "SPA")
                {
                    yield return
                        Dialog.StartCoroutine("drawTextSilent", currentPokemon.getName() + "'s Special Attack rose!");
                }
                else if (statBooster == "SPD")
                {
                    yield return
                        Dialog.StartCoroutine("drawTextSilent", currentPokemon.getName() + "'s Special Defense rose!");
                }
                else if (statBooster == "SPE")
                {
                    yield return Dialog.StartCoroutine("drawTextSilent", currentPokemon.getName() + "'s Speed rose!");
                }

                while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                {
                    yield return null;
                }
                updateSelectedItem();
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
        else if (selectedItem.getItemEffect() == ItemDataOld.ItemEffect.EVOLVE)
        {
            //EVOLVE
            if (currentPokemon.canEvolve("Stone," + selectedItem.getName()))
            {
                int oldID = currentPokemon.getID();
                SfxHandler.Play(selectClip);
                BgmHandler.main.PlayOverlay(null, 0, 0.5f);
                //yield return new WaitForSeconds(sceneTransition.FadeOut());
                yield return StartCoroutine(ScreenFade.main.Fade(false, ScreenFade.defaultSpeed));

                //Set SceneEvolution to be active so that it appears
                Scene.main.Evolution.gameObject.SetActive(true);
                StartCoroutine(Scene.main.Evolution.control(currentPokemon, "Stone," + selectedItem.getName()));
                //Start an empty loop that will only stop when SceneEvolution is no longer active (is closed)
                while (Scene.main.Evolution.gameObject.activeSelf)
                {
                    yield return null;
                }

                if (oldID != currentPokemon.getID())
                {
                    //if evolved
                    removeItem(selectedItem.getName(), 1);
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
        else if (selectedItem.getItemEffect() == ItemDataOld.ItemEffect.TM)
        {
            //TM
            if (currentPokemon.HasMove(selectedItem.getName()))
            {
                Dialog.drawDialogBox(2);
                yield return
                    Dialog.StartCoroutine("drawText",
                        currentPokemon.getName() + " already knows \n" + selectedItem.getName() + ".");
                while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                {
                    yield return null;
                }
            }
            else if (currentPokemon.CanLearnMove(selectedItem.getName()))
            {
                //check if can learn move
                if (!booted)
                {
                    Dialog.drawDialogBox(2);
                    SfxHandler.Play(tmBootupClip);
                    yield return StartCoroutine(Dialog.drawTextSilent("Booted up a TM."));
                    while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                    {
                        yield return null;
                    }
                    Dialog.drawDialogBox(2);
                    yield return Dialog.StartCoroutine("drawText", "It contained " + selectedItem.getName() + ".");
                    while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                    {
                        yield return null;
                    }
                    Dialog.drawDialogBox(2);
                    yield return
                        Dialog.StartCoroutine("drawText",
                            "Teach " + selectedItem.getName() + "\nto " + currentPokemon.getName() + "?");
                    Dialog.drawChoiceBox(14);
                    yield return StartCoroutine(Dialog.choiceNavigate());
                    Dialog.undrawChoiceBox();

                    if (Dialog.chosenIndex == 1)
                    {
                        yield return StartCoroutine(LearnMove(currentPokemon, selectedItem.getName()));
                    }
                }
                else
                {
                    yield return StartCoroutine(LearnMove(currentPokemon, selectedItem.getName()));
                }
            }
            else
            {
                Dialog.drawDialogBox();
                yield return Dialog.StartCoroutine("drawText", currentPokemon.getName() + " can't learn that move.");
                while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                {
                    yield return null;
                }
            }
            Dialog.undrawDialogBox();
            switching = false;
            selected = -1;
            updateSelectedItem();
        }
        else if (selectedItem.getItemEffect() == ItemDataOld.ItemEffect.UNIQUE)
        {
            //UNIQUE
            string selectedItemName = selectedItem.getName();
            if (selectedItemName == "Rare Candy")
            {
                if (currentPokemon.getLevel() < 100)
                {
                    currentPokemon.healHP(1);
                    currentPokemon.addExp(currentPokemon.getExpNext() - currentPokemon.getExp());
                    SfxHandler.Play(healClip);
                    updateParty();

                    removeItem(selectedItem.getName(), 1); //remove item
                    updateSelectedItem();

                    Dialog.drawDialogBox();
                    yield return
                        Dialog.StartCoroutine("drawTextSilent",
                            currentPokemon.getName() + "'s level rose to " + currentPokemon.getLevel() + "!");
                    while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                    {
                        yield return null;
                    }

                    //stat displays not yet implemented

                    int pkmnID = currentPokemon.getID();
                    //check for level evolution. EVOLVE
                    if (currentPokemon.canEvolve("Level"))
                    {
                        SfxHandler.Play(selectClip);
                        BgmHandler.main.PlayOverlay(null, 0, 0.5f);
                        yield return StartCoroutine(ScreenFade.main.Fade(false, ScreenFade.defaultSpeed));

                        //Set SceneEvolution to be active so that it appears
                        Scene.main.Evolution.gameObject.SetActive(true);
                        StartCoroutine(Scene.main.Evolution.control(currentPokemon, "Level"));
                        //Start an empty loop that will only stop when SceneEvolution is no longer active (is closed)
                        while (Scene.main.Evolution.gameObject.activeSelf)
                        {
                            yield return null;
                        }

                        updateParty();

                        yield return StartCoroutine(ScreenFade.main.Fade(true, ScreenFade.defaultSpeed));
                    }
                    //if evolution not successful / wasn't called, check for moves to learn
                    if (pkmnID == currentPokemon.getID())
                    {
                        string move = currentPokemon.MoveLearnedAtLevel(currentPokemon.getLevel());
                        Debug.Log(move);
                        if (!string.IsNullOrEmpty(move) && !currentPokemon.HasMove(move))
                        {
                            yield return StartCoroutine(LearnMove(currentPokemon, move));
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
                    Dialog.drawDialogBox(2);
                    yield return
                        Dialog.StartCoroutine("drawText",
                            selectedPokemon.getName() + " wants to learn the \nmove " + move + ".");
                    while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                    {
                        yield return null;
                    }
                    Dialog.drawDialogBox(2);
                    yield return
                        Dialog.StartCoroutine("drawText",
                            "However, " + selectedPokemon.getName() + " already \nknows four moves.");
                    while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                    {
                        yield return null;
                    }
                    Dialog.drawDialogBox(2);
                    yield return
                        Dialog.StartCoroutine("drawText", "Should a move be deleted and \nreplaced with " + move + "?");

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
                                    selectedPokemon.getName() + " forgot how to \nuse " + replacedMove + ".");
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
                            StartCoroutine(Dialog.drawTextSilent(selectedPokemon.getName() + " learned \n" + move + "!"));
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
                    StartCoroutine(Dialog.drawTextSilent(selectedPokemon.getName() + " learned \n" + move + "!"));
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
            yield return Dialog.StartCoroutine("drawText", selectedPokemon.getName() + " did not learn \n" + move + ".")
                ;
            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
            {
                yield return null;
            }
        }
    }
}