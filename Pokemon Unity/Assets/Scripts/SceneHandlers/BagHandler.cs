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
                ItemData item = ItemDatabase.getItem(items[i]);
                itemSlot[i].gameObject.SetActive(true);
                itemName[i].text = items[i];
                itemNameShadow[i].text = itemName[i].text;
                if (item.getItemType() == ItemData.ItemType.TM)
                {
                    itemIcon[i].texture =
                        Resources.Load<Texture>("Items/tm" + MoveDatabase.getMove(item.getName()).getType().ToString());
                }
                else
                {
                    itemIcon[i].texture = Resources.Load<Texture>("Items/" + items[i]);
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
                        itemQuantity[i].text = "" + SaveData.currentSave.Bag.getQuantity(items[i]);
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
                itemDescription.text = ItemDatabase.getItem(selectedItem).getDescription();
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
                tmDescription.text = ItemDatabase.getItem(selectedItem).getDescription();
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
            currentItemList = SaveData.currentSave.Bag.getItemTypeArray(ItemData.ItemType.TM);
        }
        else
        {
            itemDescription.gameObject.SetActive(true);
            visableSlots = 6;
            if (!shopMode)
            {
                if (currentScreen == 1)
                {
                    currentItemList = SaveData.currentSave.Bag.getItemTypeArray(ItemData.ItemType.ITEM);
                }
                else if (currentScreen == 2)
                {
                    currentItemList = SaveData.currentSave.Bag.getItemTypeArray(ItemData.ItemType.MEDICINE);
                }
                else if (currentScreen == 3)
                {
                    currentItemList = SaveData.currentSave.Bag.getItemTypeArray(ItemData.ItemType.BERRY);
                }
                else if (currentScreen == 5)
                {
                    currentItemList = SaveData.currentSave.Bag.getItemTypeArray(ItemData.ItemType.KEY);
                }
            }
            else
            {
                if (currentScreen == 1)
                {
                    currentItemList = shopItemList;
                }
                else if (currentScreen == 2)
                {
                    currentItemList = SaveData.currentSave.Bag.getSellableItemArray();
                }
                else if (currentScreen == 3)
                {
                    currentItemList = SaveData.currentSave.Bag.getItemTypeArray(ItemData.ItemType.ITEM);
                }
                else if (currentScreen == 4)
                {
                    currentItemList = SaveData.currentSave.Bag.getItemTypeArray(ItemData.ItemType.MEDICINE);
                }
                else if (currentScreen == 5)
                {
                    currentItemList = SaveData.currentSave.Bag.getItemTypeArray(ItemData.ItemType.BERRY);
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
            currentPokemon = SaveData.currentSave.PC.boxes[0][i];
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
                if (currentPokemon.getGender() == Pokemon.Gender.FEMALE)
                {
                    partyGender[i].text = "♀";
                    partyGender[i].color = new Color(1, 0.2f, 0.2f, 1);
                }
                else if (currentPokemon.getGender() == Pokemon.Gender.MALE)
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
                if (currentPokemon.getStatus() != Pokemon.Status.NONE)
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
        partyLength = SaveData.currentSave.PC.getBoxLength(0);
    }

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

            ItemData selectedItem =
                ItemDatabase.getItem(
                    currentItemList[currentPosition[currentScreen] + currentTopPosition[currentScreen] - 1]);
            if (selectedItem.getItemEffect() == ItemData.ItemEffect.EVOLVE)
            {
                for (int i = 0; i < 6; i++)
                {
                    partyStandardDisplay[i].SetActive(false);
                    partyTextDisplay[i].gameObject.SetActive(true);
                    if (SaveData.currentSave.PC.boxes[0][i] != null)
                    {
                        if (SaveData.currentSave.PC.boxes[0][i].canEvolve("Stone," + selectedItem.getName()))
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
            else if (selectedItem.getItemEffect() == ItemData.ItemEffect.TM)
            {
                for (int i = 0; i < 6; i++)
                {
                    partyStandardDisplay[i].SetActive(false);
                    partyTextDisplay[i].gameObject.SetActive(true);
                    if (SaveData.currentSave.PC.boxes[0][i] != null)
                    {
                        if (SaveData.currentSave.PC.boxes[0][i].HasMove(selectedItem.getName()))
                        {
                            partyTextDisplay[i].text = "LEARNED!";
                            partyTextDisplay[i].color = new Color(1, 1, 1, 1);
                        }
                        else if (SaveData.currentSave.PC.boxes[0][i].CanLearnMove(selectedItem.getName()))
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
            string playerMoney = "" + SaveData.currentSave.playerMoney;
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
                                         SaveData.currentSave.Bag.getQuantity(
                                             currentItemList[
                                                 currentPosition[currentScreen] + currentTopPosition[currentScreen] - 1]);
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
        bool result = SaveData.currentSave.Bag.addItem(itemName, amount);
        if (result)
        {
            ItemData.ItemType itemType = ItemDatabase.getItem(itemName).getItemType();
            bool relevantScreen = false;
            if (currentScreen == 1 && itemType == ItemData.ItemType.ITEM)
            {
                relevantScreen = true;
            }
            else if (currentScreen == 2 && itemType == ItemData.ItemType.MEDICINE)
            {
                relevantScreen = true;
            }
            else if (currentScreen == 3 && itemType == ItemData.ItemType.BERRY)
            {
                relevantScreen = true;
            }
            else if (currentScreen == 4 && itemType == ItemData.ItemType.TM)
            {
                relevantScreen = true;
            }
            else if (currentScreen == 5 && itemType == ItemData.ItemType.KEY)
            {
                relevantScreen = true;
            }
            if (SaveData.currentSave.Bag.getQuantity(itemName) == amount && relevantScreen)
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

    private bool removeItem(string itemName, int amount)
    {
        bool result = SaveData.currentSave.Bag.removeItem(itemName, amount);
        if (result)
        {
            if (SaveData.currentSave.Bag.getQuantity(itemName) == 0)
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
        currentItemList = SaveData.currentSave.Bag.getItemTypeArray(ItemData.ItemType.ITEM);
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
                            ItemData selectedItem = ItemDatabase.getItem(currentItemList[selected]);

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
                                if (SaveData.currentSave.playerMoney >= selectedItem.getPrice())
                                {
                                    Dialog.drawDialogBox(2);
                                    yield return
                                        StartCoroutine(
                                            Dialog.drawText(selectedItem.getName() +
                                                            "? Certainly. \nHow many would you like to buy?"));

                                    //quantity selection not yet implemented
                                    int maxQuantity =
                                        Mathf.FloorToInt((float) SaveData.currentSave.playerMoney /
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
                                            SaveData.currentSave.playerMoney -= (shopSelectedQuantity *
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
                                int maxQuantity = SaveData.currentSave.Bag.getQuantity(selectedItem.getName());
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
                                        SaveData.currentSave.playerMoney += shopSelectedQuantity *
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
                            ItemData selectedItem = ItemDatabase.getItem(currentItemList[selected]);

                            if (selectedItem.getItemType() == ItemData.ItemType.TM ||
                                selectedItem.getItemType() == ItemData.ItemType.KEY)
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
                        if (SaveData.currentSave.PC.boxes[0][partyPosition].getHeldItem().Length > 0)
                        {
                            Dialog.drawDialogBox();
                            yield return
                                Dialog.StartCoroutine("drawText",
                                    "Take " + SaveData.currentSave.PC.boxes[0][partyPosition].getName() + "'s Item?");
                            Dialog.drawChoiceBox();
                            yield return Dialog.StartCoroutine("choiceNavigate");
                            int chosenIndex = Dialog.chosenIndex;
                            Dialog.undrawDialogBox();
                            Dialog.undrawChoiceBox();
                            if (chosenIndex == 1)
                            {
                                string receivedItem = SaveData.currentSave.PC.boxes[0][partyPosition].swapHeldItem("");
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
                                        SaveData.currentSave.PC.boxes[0][partyPosition].getName() + ".");
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
                            ItemData selectedItem = ItemDatabase.getItem(currentItemList[selected]);
                            if (selectedItem.getItemType() == ItemData.ItemType.KEY)
                            {
                                if (selectedItem.getItemEffect() == ItemData.ItemEffect.UNIQUE)
                                {
                                    choices = new string[]
                                    {
                                        "Use", "Register", "Deselect", "Cancel"
                                    };
                                }
                            }
                            else if (selectedItem.getItemEffect() != ItemData.ItemEffect.NONE &&
                                     selectedItem.getItemEffect() != ItemData.ItemEffect.FLEE &&
                                     selectedItem.getItemEffect() != ItemData.ItemEffect.BALL &&
                                     selectedItem.getItemEffect() != ItemData.ItemEffect.STATBOOST)
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
                                if (selectedItem.getItemType() == ItemData.ItemType.KEY)
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
                                        SaveData.currentSave.registeredItems[0] = selectedItem.getName();
                                    }
                                    else if (chosenIndex == 3)
                                    {
                                        SaveData.currentSave.registeredItems[1] = selectedItem.getName();
                                    }
                                    else if (chosenIndex == 2)
                                    {
                                        SaveData.currentSave.registeredItems[2] = selectedItem.getName();
                                    }
                                    else if (chosenIndex == 1)
                                    {
                                        SaveData.currentSave.registeredItems[3] = selectedItem.getName();
                                    }
                                }
                                else
                                {
                                    //USE
                                    if (selectedItem.getItemEffect() == ItemData.ItemEffect.UNIQUE)
                                    {
                                    }
                                    else if (partyLength > 0)
                                    {
                                        bool selectingPokemon = true;
                                        Dialog.drawDialogBox();
                                        if (selectedItem.getItemType() != ItemData.ItemType.TM)
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
                                                Pokemon currentPokemon = SaveData.currentSave.PC.boxes[0][partyPosition];

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

                            SaveData.currentSave.Bag.moveBehind(
                                SaveData.currentSave.Bag.getIndexOf(currentItemList[selected]),
                                SaveData.currentSave.Bag.getIndexOf(
                                    currentItemList[
                                        currentPosition[currentScreen] + currentTopPosition[currentScreen] - 1]));

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
                        Pokemon currentPokemon = SaveData.currentSave.PC.boxes[0][partyPosition];

                        Dialog.drawDialogBox();
                        yield return
                            Dialog.StartCoroutine("drawText", "Do what with " + currentItemList[selected] + "?");
                        string[] choices = new string[] {"Give", "Deselect", "Cancel"};
                        if (!string.IsNullOrEmpty(currentPokemon.getHeldItem()))
                        {
                            choices = new string[]
                            {
                                "Swap", "Take", "Deselect", "Cancel"
                            };
                        }
                        ItemData selectedItem = ItemDatabase.getItem(currentItemList[selected]);
                        //Adjust choices to suit situation
                        if (selectedItem.getItemType() != ItemData.ItemType.KEY)
                        {
                            if (selectedItem.getItemEffect() == ItemData.ItemEffect.HP ||
                                selectedItem.getItemEffect() == ItemData.ItemEffect.PP ||
                                selectedItem.getItemEffect() == ItemData.ItemEffect.STATUS ||
                                selectedItem.getItemEffect() == ItemData.ItemEffect.EV ||
                                selectedItem.getItemEffect() == ItemData.ItemEffect.EVOLVE)
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
                            else if (selectedItem.getItemEffect() == ItemData.ItemEffect.TM)
                            {
                                choices = new string[]
                                {
                                    "Use", "Deselect", "Cancel"
                                };
                            }
                            else if (selectedItem.getItemEffect() == ItemData.ItemEffect.UNIQUE)
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
    private IEnumerator runItemEffect(ItemData selectedItem, Pokemon currentPokemon)
    {
        yield return StartCoroutine(runItemEffect(selectedItem, currentPokemon, false));
    }

    private IEnumerator runItemEffect(ItemData selectedItem, Pokemon currentPokemon, bool booted)
    {
        if (selectedItem.getItemEffect() == ItemData.ItemEffect.HP)
        {
            //HP
            if (currentPokemon.getCurrentHP() < currentPokemon.getHP() &&
                currentPokemon.getStatus() != Pokemon.Status.FAINTED)
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
        else if (selectedItem.getItemEffect() == ItemData.ItemEffect.PP)
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
        else if (selectedItem.getItemEffect() == ItemData.ItemEffect.STATUS)
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
                    currentPokemon.setStatus(Pokemon.Status.NONE);

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
                    currentPokemon.setStatus(Pokemon.Status.NONE);

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
        else if (selectedItem.getItemEffect() == ItemData.ItemEffect.EV)
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
        else if (selectedItem.getItemEffect() == ItemData.ItemEffect.EVOLVE)
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
        else if (selectedItem.getItemEffect() == ItemData.ItemEffect.TM)
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
        else if (selectedItem.getItemEffect() == ItemData.ItemEffect.UNIQUE)
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

    private IEnumerator LearnMove(Pokemon selectedPokemon, string move)
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