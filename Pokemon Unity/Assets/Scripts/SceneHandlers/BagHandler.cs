//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BagHandler : MonoBehaviour
{
    private DialogBoxHandlerNew Dialog;

    public GameObject scene;

    public string chosenItem;
    

    public GameObject[] pkmParty = new GameObject[6];
    public Sprite pkmPartySlotSprite;
    public Sprite pkmPartySlotSpriteSelected;
    public Sprite itemSlotSprite;
    public Sprite itemSlotSpriteSelected;
    public Sprite itemSlotSpriteSwap;
    
    private Image[] pkmSlot = new Image[6];
    private Image[] pkmIcon = new Image[6];
    private Image[] pkmStatus = new Image[6];
    private Image[] pkmHPbar = new Image[6];
    private Image[] pkmItem = new Image[6];
    private Text[] pkmLevel = new Text[6];
    private Text[] pkmLevelShadow = new Text[6];

    public RectTransform bagScrollBar;
    public Image bagUp;
    public Image bagDown;

    public Sprite bagUpSprite;
    public Sprite bagDownSprite;
    
    public Sprite shopUpSprite;
    public Sprite shopDownSprite;

    //items
    public Image[] bagSlot = new Image[8];
    private Image[] bagItemIcon = new Image[8];
    private Text[] bagItemName = new Text[8];
    private Text[] bagItemNameShadow = new Text[8];
    
    //upper screen's general info
    private Text bagItemNameUp;
    private Text bagItemNameUpShadow;
    private Text bagItemAmount;
    private Text bagItemAmountShadow;
    private Text bagItemDescription;
    private Text bagItemDescriptionShadow;
    private Image bagItemPreview;
    
    //upper screen's TM info
    private Text bagItemPower;
    private Text bagItemPowerShadow;
    private Text bagItemAccuracy;
    private Text bagItemAccuracyShadow;
    private Text bagItemPP;
    private Text bagItemPPShadow;
    private Image bagItemType;
    private Image bagItemCategory;

    private GameObject[] bagCategory = new GameObject[6];
    private GameObject[] shopCategory = new GameObject[6];
    
    //Shop
    public GameObject shopInfos;
    private Text shopDataText;
    private Text shopDataTextShadow;
    private Text shopData;
    private Text shopDataShadow;
    private Text shopMoneyText;
    private Text shopMoneyTextShadow;
    private Text shopMoney;
    private Text shopMoneyShadow;
    
    private GameObject shopNumberBox;
    private Text shopNumberBoxText;
    private Text shopNumberBoxTextShadow;
    

    public int currentScreen = 1;

    public int[] currentPosition = new int[]
    {
        -1, 1, 1, 1, 1, 1
    };

    public int[] currentTopPosition = new int[]
    {
        -1, 0, 0, 0, 0, 0
    };

    private int visableSlots = 7;

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
    public AudioClip decideClip;
    public AudioClip cancelClip;
    
    public AudioClip healClip;
    public AudioClip tmBootupClip;
    public AudioClip forgetMoveClip;
    public AudioClip saleClip;

    void Awake()
    {
        Dialog = transform.GetComponent<DialogBoxHandlerNew>();
        BagAudio = transform.GetComponent<AudioSource>();
        
        for (int i = 0; i < 6; i++)
        {
            pkmSlot[i] = pkmParty[i].transform.GetComponent<Image>();
            pkmIcon[i] = pkmParty[i].transform.Find("icon").GetComponent<Image>();
            pkmStatus[i] = pkmParty[i].transform.Find("status").GetComponent<Image>();
            pkmHPbar[i] = pkmParty[i].transform.Find("advHPbar").Find("HPBar").GetComponent<Image>();
            pkmItem[i] = pkmParty[i].transform.Find("item").GetComponent<Image>();
            pkmLevel[i] = pkmParty[i].transform.Find("level").GetComponent<Text>();
            pkmLevelShadow[i] = pkmParty[i].transform.Find("level shadow").GetComponent<Text>();
        }

        for (int i = 0; i < 8; i++)
        {
            bagItemIcon[i] = bagSlot[i].transform.Find("Icon").GetComponent<Image>();
            bagItemNameShadow[i] = bagSlot[i].transform.Find("Name shadow").GetComponent<Text>();
            bagItemName[i] =bagItemNameShadow[i].transform.Find("Name").GetComponent<Text>();
        }
        
        //Informations de l'écran supérieur
        bagItemDescriptionShadow = transform.Find("ItemDescriptionShadow").GetComponent<Text>();
        bagItemDescription = bagItemDescriptionShadow.transform.Find("ItemDescription").GetComponent<Text>();
        
        bagItemNameUpShadow = transform.Find("Item Name shadow").GetComponent<Text>();
        bagItemNameUp = bagItemNameUpShadow.transform.Find("Item Name").GetComponent<Text>();
        
        bagItemAmountShadow = transform.Find("Item Amount Shadow").GetComponent<Text>();
        bagItemAmount = bagItemAmountShadow.transform.Find("Item Amount").GetComponent<Text>();

        bagItemPreview = transform.Find("itempreview").GetComponent<Image>();
        
        //Informations du shop
        shopDataText = shopInfos.transform.Find("DataText").GetComponent<Text>();
        shopDataTextShadow = shopInfos.transform.Find("DataTextShadow").GetComponent<Text>();
        shopData = shopInfos.transform.Find("Data").GetComponent<Text>();
        shopDataShadow = shopInfos.transform.Find("DataShadow").GetComponent<Text>();
        shopMoneyText = shopInfos.transform.Find("MoneyText").GetComponent<Text>();
        shopMoneyTextShadow = shopInfos.transform.Find("MoneyTextShadow").GetComponent<Text>();
        shopMoney = shopInfos.transform.Find("Money").GetComponent<Text>();
        shopMoneyShadow = shopInfos.transform.Find("MoneyShadow").GetComponent<Text>();

        shopNumberBox = transform.Find("NumberBox").gameObject;
        shopNumberBoxText = shopNumberBox.transform.Find("BoxText").GetComponent<Text>();
        shopNumberBoxTextShadow = shopNumberBox.transform.Find("BoxTextShadow").GetComponent<Text>();

        for (int i = 1; i < 6; i++)
        {
            bagCategory[i] = transform.Find("bag" + i).gameObject;
            shopCategory[i] = transform.Find("shop" + i).gameObject;
        }

        //Catégorie TM
        bagItemType = bagCategory[4].transform.Find("TMtype").GetComponent<Image>();
        bagItemCategory = bagCategory[4].transform.Find("TMcategory").GetComponent<Image>();
        bagItemPowerShadow = bagCategory[4].transform.Find("TMpower").GetComponent<Text>();
        bagItemPower = bagItemPowerShadow.transform.Find("text").GetComponent<Text>();
        bagItemAccuracyShadow = bagCategory[4].transform.Find("TMaccuracy").GetComponent<Text>();
        bagItemAccuracy = bagItemPowerShadow.transform.Find("text").GetComponent<Text>();
        bagItemPPShadow = bagCategory[4].transform.Find("TMpp").GetComponent<Text>();
        bagItemPP = bagItemPowerShadow.transform.Find("text").GetComponent<Text>();
    }

    void Start()
    {
        gameObject.SetActive(false);
    }


    private void updateScrollBar()
    {
        float topY = -27.1f;
        float bottomY = -167.91f;
        float itemsLength = currentItemList.Length;

        int index = currentPosition[currentScreen] + currentTopPosition[currentScreen] - 1;
        Debug.Log("Index: "+index);

        float height_coeff;
        float length = ((itemsLength - 4 <= 0) ? 1 : itemsLength - 4);
        
        if (index < 4)
        {
            height_coeff = 0 / length;
        }
        else if (index > length)
        {
            height_coeff = 1;
        }
        else
        {
            height_coeff = (index - 3) / length;
        }

        bagScrollBar.localPosition = new Vector3(bagScrollBar.localPosition.x, topY - (topY - bottomY) * height_coeff, bagScrollBar.localPosition.z);
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
                        //swap texture
                        bagSlot[i].sprite = itemSlotSpriteSwap;
                    }
                    else
                    {
                        //swap texture
                        bagSlot[i].sprite = itemSlotSpriteSelected;
                    }
                }
                else
                {
                    bagSlot[i].sprite = itemSlotSpriteSelected;
                }
            }
            else
            {
                if (switching)
                {
                    if (i == selected - currentTopPosition[currentScreen] + 1)
                    {
                        bagSlot[i].sprite = itemSlotSpriteSelected;
                    }
                    else
                    {
                        bagSlot[i].sprite = itemSlotSprite;
                    }
                }
                else
                {
                    bagSlot[i].sprite = itemSlotSprite;
                }
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
        
        //Updated
        for (int i = 0; i < 8; i++)
        {
            if (i >= 0)
            {
                if (items[i] == null)
                {
                    bagSlot[i].gameObject.SetActive(false);
                }
                else
                {
                    ItemData item = ItemDatabase.getItem(items[i]);
                    bagSlot[i].gameObject.SetActive(true);
                    bagItemName[i].text = items[i];
                    bagItemNameShadow[i].text = bagItemName[i].text;
                    if (item.getItemType() == ItemData.ItemType.TM)
                    {
                        bagItemIcon[i].sprite =
                            Resources.Load<Sprite>("Items/tm" + MoveDatabase.getMove(item.getName()).getType().ToString());
                    }
                    else
                    {
                        bagItemIcon[i].sprite = Resources.Load<Sprite>("Items/" + items[i]);
                    }
                    if (item.getItemType() == ItemData.ItemType.TM)
                    {
                        bagItemName[i].text = "TM"
                                              + (item.getTMNo() < 10 ? "0" : "")
                                              + item.getTMNo()
                                              + " " + bagItemName[i].text;
                        bagItemNameShadow[i].text = bagItemName[i].text;
                        bagItemAmountShadow.gameObject.SetActive(false);
                    }
                    else if (item.getItemType() == ItemData.ItemType.KEY)
                    {
                        bagItemAmountShadow.gameObject.SetActive(false);
                    }
                    else
                    {
                        bagItemAmountShadow.gameObject.SetActive(true);
                        if (shopMode && currentScreen == 1)
                        {
                            /* TODO shopmode
                            itemX[i].text = "$";
                            itemQuantity[i].text = "" + item.getPrice();
                            */
                        }
                    }
                    bagItemAmountShadow.text = bagItemAmount.text;
                }
            }
        }
    }

    private void updateDescription()
    {

        //resolve the index of the current item by adding the top and visible positions, minus 1.
        int index = currentPosition[currentScreen] + currentTopPosition[currentScreen] - 1;
        if (index < currentItemList.Length)
        {
            bagItemPreview.gameObject.SetActive(true);
            bagItemPreview.sprite = bagItemIcon[currentPosition[currentScreen]].sprite;
            string selectedItem = currentItemList[index];
            if (shopMode && currentScreen == 1)
            {
                //Replace Quantity by Price
                bagItemAmount.text = "$ "+ItemDatabase.getItem(selectedItem).getPrice();
                bagItemAmountShadow.text = bagItemAmount.text;
            }
            else
            {
                //Update Quantity
                bagItemAmount.text = "x "+SaveData.currentSave.Bag.getQuantity(selectedItem);
                bagItemAmountShadow.text = bagItemAmount.text;
            }
            if (currentScreen != 4)
            {

                //Update upper screen's item name
                bagItemNameUp.text = selectedItem;
                bagItemNameUpShadow.text = bagItemNameUp.text;
                
                //Update description of basic object
                bagItemDescription.text = ItemDatabase.getItem(selectedItem).getDescription();
                bagItemDescriptionShadow.text = bagItemDescription.text;
            }
            else if (!shopMode)
            {
                string TMName = "TM"
                                + (ItemDatabase.getItem(selectedItem).getTMNo() < 10 ? "0" : "")
                                + ItemDatabase.getItem(selectedItem).getTMNo()
                                + " " + selectedItem;
                MoveData selectedTM = MoveDatabase.getMove(selectedItem);

                //Update descriptions of TM
                bagItemType.sprite = Resources.Load<Sprite>("PCSprites/type" + selectedTM.getType().ToString());
                bagItemCategory.sprite = Resources.Load<Sprite>("PCSprites/category" + selectedTM.getCategory().ToString());
                bagItemPower.text = "" + selectedTM.getPower();
                if (bagItemPower.text == "0")
                {
                    bagItemPower.text = "-";
                }
                bagItemPowerShadow.text = bagItemPower.text;
                bagItemAccuracy.text = "" + Mathf.Round(selectedTM.getAccuracy() * 100f);
                if (bagItemAccuracy.text == "0")
                {
                    bagItemAccuracy.text = "-";
                }
                bagItemAccuracyShadow.text = bagItemAccuracy.text;
                
                bagItemDescription.text = ItemDatabase.getItem(selectedItem).getDescription();
                bagItemDescriptionShadow.text = bagItemDescription.text;
                
                //Update upper screen's item name
                bagItemNameUp.text = TMName;
                bagItemNameUpShadow.text = bagItemNameUp.text;
            }
        }
        else
        {
            bagItemPreview.gameObject.SetActive(false);
            if (currentScreen != 4)
            {

                bagItemDescription.text = "";
                bagItemDescriptionShadow.text = bagItemDescription.text;
                
                bagItemAmount.text = "";
                bagItemAmountShadow.text = "";

                bagItemNameUp.text = "";
                bagItemNameUpShadow.text = "";
            }
            else
            {

                bagItemDescription.text = "";
                bagItemDescriptionShadow.text = bagItemDescription.text;
                
                bagItemAmount.text = "";
                bagItemNameUpShadow.text = "";
            }
        }
    }

    private void updateScreen()
    {
        if (currentScreen == 4 && !shopMode)
        {
            visableSlots = 5;
            currentItemList = SaveData.currentSave.Bag.getItemTypeArray(ItemData.ItemType.TM);
        }
        else
        {
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
            bagCategory[currentScreen].SetActive(true);
        }
        else
        {
            shopCategory[currentScreen].SetActive(true);
            bagCategory[currentScreen].SetActive(false);
        }
        for (int i = 1; i < 6; i++)
        {
            if (i != currentScreen)
            {
                bagCategory[i].SetActive(false);
                shopCategory[i].SetActive(false);
            }
        }
    }

    private IEnumerator scrollScrollBar(int direction)
    {
        updateScrollBar();
        updateItemList();
        yield return null;
    }

    private void updateParty()
    {
        Pokemon currentPokemon;
        for (int i = 0; i < 6; i++)
        {
            currentPokemon = SaveData.currentSave.PC.boxes[0][i];
            if (currentPokemon == null)
            {
                pkmParty[i].gameObject.SetActive(false);
            }
            else
            {
                pkmIcon[i].sprite = currentPokemon.GetIconsSprite()[0];
                pkmHPbar[i].rectTransform.sizeDelta = new Vector2(64*((float) currentPokemon.getCurrentHP() / currentPokemon.getHP()), 2f);
                if (currentPokemon.getCurrentHP() < (currentPokemon.getHP() / 4f))
                {
                    pkmHPbar[i].color = new Color(1, 0.125f, 0, 1);
                }
                else if (currentPokemon.getCurrentHP() < (currentPokemon.getHP() / 2f))
                {
                    pkmHPbar[i].color = new Color(1, 0.75f, 0, 1);
                }
                else
                {
                    pkmHPbar[i].color = new Color(0.125f, 1, 0.065f, 1);
                }

                pkmLevel[i].text = "" + currentPokemon.getLevel();
                pkmLevelShadow[i].text = pkmLevel[i].text;
                if (currentPokemon.getStatus() != Pokemon.Status.NONE)
                {
                    pkmStatus[i].gameObject.SetActive(true);
                    pkmStatus[i].sprite =
                        Resources.Load<Sprite>("PCSprites/status" + currentPokemon.getStatus().ToString());
                }
                else
                {
                    pkmStatus[i].gameObject.SetActive(false);
                    pkmStatus[i].sprite = null;
                }
                if (!string.IsNullOrEmpty(currentPokemon.getHeldItem()))
                {
                    pkmItem[i].enabled = true;
                }
                else
                {
                    pkmItem[i].enabled = false;
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
                /*
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
                */
            }
            else if (selectedItem.getItemEffect() == ItemData.ItemEffect.TM)
            {
                /*
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
                */
            }
            else
            {
                /*
                for (int i = 0; i < 6; i++)
                {
                    partyStandardDisplay[i].SetActive(true);
                    partyTextDisplay[i].gameObject.SetActive(false);
                }
                */
            }
        }
        else
        {
            currentPosition[currentScreen] = 1;
            currentTopPosition[currentScreen] = 0;
            updateDescription();
            /*
            for (int i = 0; i < 6; i++)
            {
                partyStandardDisplay[i].SetActive(true);
                partyTextDisplay[i].gameObject.SetActive(false);
            }
            */
        }
        /*
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
        */
    }

    private void updateMoneyBox()
    {
        if (shopMode)
        {
            string playerMoney = "" + SaveData.currentSave.playerMoney;
            char[] playerMoneyChars = playerMoney.ToCharArray();
            playerMoney = "";
            //format playerMoney into a currency style (e.g. $1,000,000)
            for (int i = 0; i < playerMoneyChars.Length; i++)
            {
                playerMoney = playerMoneyChars[playerMoneyChars.Length - 1 - i] + playerMoney;
                if ((i + 1) % 3 == 0 && i != playerMoneyChars.Length - 1)
                {
                    playerMoney = " " + playerMoney;
                }
            }

            shopMoney.text = "$ " + SaveData.currentSave.playerMoney;
            shopMoneyShadow.text = shopMoney.text;
        }
        else
        {
            shopMoney.transform.parent.gameObject.SetActive(false);
        }
    }

    private void updateDataBox()
    {
        if (shopMode)
        {
            if (currentScreen == 1)
            {
                shopDataText.text = "In Bag:";
                if (currentItemList.Length > 0)
                {

                    shopData.text = "x " +
                                    SaveData.currentSave.Bag.getQuantity(
                                        currentItemList[
                                            currentPosition[currentScreen] + currentTopPosition[currentScreen] - 1]);
                }
                else
                {
                    shopData.text = "-";
                }
            }
            else
            {
                shopDataText.text = "Offer:";
                if (currentItemList.Length > 0)
                {

                    shopData.text = "$ " +
                                    Mathf.Floor(
                                        (float)
                                        ItemDatabase.getItem(
                                            currentItemList[
                                                currentPosition[currentScreen] + currentTopPosition[currentScreen] -
                                                1]).getPrice() / 2f);
                }
                else
                {
                    shopData.text = "-";
                }
            }

            shopDataShadow.text = shopData.text;
            shopDataTextShadow.text = shopDataText.text;
        }
        else
        {
            shopData.transform.parent.gameObject.SetActive(false);
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

        shopNumberBoxText.text =  "x " + numberString + "\n$ " + (price * shopSelectedQuantity);
        shopNumberBoxTextShadow.text = shopNumberBoxText.text;
    }

    private IEnumerator openNumbersBox(int price, int max)
    {
      
        shopNumberBox.SetActive(true);

        if (max > 99)
        {
            max = 99;
        }

        shopSelectedQuantity = 1;
        bool hoverOnTens = false;

        updateNumbersBox(price, hoverOnTens);

        while (shopNumberBox.activeSelf)
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
                shopNumberBox.SetActive(false);
            }
            else if (Input.GetButtonDown("Back"))
            {
                shopSelectedQuantity = 0;
                shopNumberBox.SetActive(false);
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

            //display shop and hide party
            transform.Find("Party").gameObject.SetActive(false);
            shopInfos.SetActive(true);
            bagUp.sprite = shopUpSprite;
            bagDown.sprite = shopDownSprite;

            //sceneTransition.FadeIn(0);
            ScreenFade.main.SetToFadedIn();
        }
        else
        {
            //display party and hide shop
            transform.Find("Party").gameObject.SetActive(true);
            shopInfos.SetActive(false);
            
            bagUp.sprite = bagUpSprite;
            bagDown.sprite = bagDownSprite;
            
            //sceneTransition.FadeIn();
            StartCoroutine(ScreenFade.main.Fade(true, ScreenFade.defaultSpeed));
        }
        shopNumberBox.SetActive(false);

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
                            currentTopPosition[currentScreen] -= 1;
                            StartCoroutine("scrollScrollBar", -1);
                        }
                        updatePartyDisplays();
                        updateSelectedItem();
                        updateDataBox();
                        SfxHandler.Play(selectClip);
                        yield return new WaitForSeconds(0.16f);
                        updateDescription();
                    }
                }
                else
                {
                    if (partyPosition > 0)
                    {
                        pkmSlot[partyPosition].sprite = pkmPartySlotSprite;
                        partyPosition -= 1;
                        pkmSlot[partyPosition].sprite = pkmPartySlotSpriteSelected;

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
                            currentTopPosition[currentScreen] += 1;
                            StartCoroutine("scrollScrollBar", 1);
                        }
                        updatePartyDisplays();
                        updateSelectedItem();
                        updateDataBox();
                        SfxHandler.Play(selectClip);
                        yield return new WaitForSeconds(0.16f);
                        updateDescription();
                    }
                }
                else
                {
                    if (partyPosition < partyLength - 1)
                    {
                        pkmSlot[partyPosition].sprite = pkmPartySlotSprite;
                        partyPosition += 1;
                        pkmSlot[partyPosition].sprite = pkmPartySlotSpriteSelected;

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
                    //update
                    pkmSlot[partyPosition].sprite = pkmPartySlotSprite;

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
                        //update
                        pkmSlot[partyPosition].sprite = pkmPartySlotSpriteSelected;

                        SfxHandler.Play(selectClip);
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                else if (!inParty && partyAccessible)
                {
                    inParty = true;

                    //update
                    pkmSlot[partyPosition].sprite = pkmPartySlotSpriteSelected;

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
                            SfxHandler.Play(decideClip);

                            chosenItem = currentItemList[selected];

                            if (currentScreen == 1)
                            {
                                //BUY
                                //custom prices not yet implemented
                                if (SaveData.currentSave.playerMoney >= selectedItem.getPrice())
                                {
                                    Dialog.DrawDialogBox(2);
                                    yield return
                                        StartCoroutine(
                                            Dialog.DrawText(selectedItem.getName() +
                                                            "? Certainly. \nHow many would you like to buy?"));

                                    //quantity selection not yet implemented
                                    int maxQuantity =
                                        Mathf.FloorToInt((float) SaveData.currentSave.playerMoney /
                                                         (float) selectedItem.getPrice());
                                    Debug.Log(maxQuantity);
                                    yield return StartCoroutine(openNumbersBox(selectedItem.getPrice(), maxQuantity));

                                    if (shopSelectedQuantity > 0)
                                    {
                                        Dialog.DrawDialogBox(2);
                                        yield return
                                            StartCoroutine(
                                                Dialog.DrawText(selectedItem.getName() + ", and you want " +
                                                                shopSelectedQuantity + ". \nThat will be $" +
                                                                (shopSelectedQuantity * selectedItem.getPrice()) +
                                                                ". OK?"));
                                        ;
                                        yield return StartCoroutine(Dialog.DrawChoiceBox());
                                        int chosenIndex2 = Dialog.chosenIndex;
                                        Dialog.UndrawDialogBox();
                                        Dialog.UndrawChoiceBox();

                                        if (chosenIndex2 == 1)
                                        {
                                            SfxHandler.Play(saleClip);

                                            addItem(selectedItem.getName(), shopSelectedQuantity);
                                            SaveData.currentSave.playerMoney -= (shopSelectedQuantity *
                                                                                 selectedItem.getPrice());

                                            updateMoneyBox();
                                            updateDataBox();

                                            Dialog.DrawDialogBox(2);
                                            yield return
                                                StartCoroutine(Dialog.DrawTextSilent("Here you are!\nThank you!"));
                                            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                                            {
                                                yield return null;
                                            }
                                            Dialog.UndrawDialogBox();
                                        }
                                    }
                                    Dialog.UndrawDialogBox();
                                }
                                else
                                {
                                    Dialog.DrawDialogBox(2);
                                    yield return StartCoroutine(Dialog.DrawText("You don't have enough money."));
                                    while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                                    {
                                        yield return null;
                                    }
                                    Dialog.UndrawDialogBox();
                                }
                            }
                            else
                            {
                                //SELL
                                Dialog.DrawDialogBox(2);
                                yield return
                                    StartCoroutine(
                                        Dialog.DrawText(selectedItem.getName() + "? \nHow many would you like to sell?"))
                                    ;

                                //quantity selection not yet implemented
                                int maxQuantity = SaveData.currentSave.Bag.getQuantity(selectedItem.getName());
                                Debug.Log(maxQuantity);
                                yield return
                                    StartCoroutine(openNumbersBox(
                                        Mathf.FloorToInt((float) selectedItem.getPrice() / 2f), maxQuantity));

                                if (shopSelectedQuantity > 0)
                                {
                                    Dialog.DrawDialogBox(2);
                                    yield return
                                        StartCoroutine(
                                            Dialog.DrawText("I can pay $" +
                                                            (shopSelectedQuantity *
                                                             Mathf.FloorToInt((float) selectedItem.getPrice() / 2f)) +
                                                            ". \nWould that be OK?"));
                                    ;
                                    yield return StartCoroutine(Dialog.DrawChoiceBox());
                                    int chosenIndex2 = Dialog.chosenIndex;
                                    Dialog.UndrawDialogBox();
                                    Dialog.UndrawChoiceBox();

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

                                        Dialog.DrawDialogBox(2);
                                        if (shopSelectedQuantity == 1)
                                        {
                                            yield return
                                                StartCoroutine(
                                                    Dialog.DrawTextSilent("Turned over the " + selectedItem.getName() +
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
                                                    Dialog.DrawTextSilent("Turned over the " + selectedItem.getName() +
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
                                        Dialog.UndrawDialogBox();
                                    }
                                }
                                Dialog.UndrawDialogBox();
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
                                Dialog.DrawDialogBox();
                                yield return
                                    StartCoroutine(Dialog.DrawText("The " + selectedItem.getName() + " can't be held."))
                                    ;
                                while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                                {
                                    yield return null;
                                }
                                Dialog.UndrawDialogBox();
                            }
                            else
                            {
                                switching = true;
                                selectedPosition = currentPosition[currentScreen];
                                selectedTopPosition = currentTopPosition[currentScreen];
                                updateSelectedItem();
                                SfxHandler.Play(decideClip);

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
                            Dialog.DrawDialogBox();
                            yield return
                                Dialog.StartCoroutine(Dialog.DrawText(
                                    "Take " + SaveData.currentSave.PC.boxes[0][partyPosition].getName() + "'s Item?"));
                            yield return Dialog.StartCoroutine(Dialog.DrawChoiceBox());
                            int chosenIndex = Dialog.chosenIndex;
                            Dialog.UndrawDialogBox();
                            Dialog.UndrawChoiceBox();
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

                                Dialog.DrawDialogBox();
                                yield return
                                    Dialog.StartCoroutine(Dialog.DrawText(
                                        "Took the " + receivedItem + " from " +
                                        SaveData.currentSave.PC.boxes[0][partyPosition].getName() + "."));
                                while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                                {
                                    yield return null;
                                }
                                Dialog.UndrawDialogBox();
                            }
                        }

                        SfxHandler.Play(decideClip);
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
                            Dialog.DrawDialogBox();
                            yield return
                                Dialog.StartCoroutine(Dialog.DrawText( "Do what with " + currentItemList[selected] + "?"));
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
                            
                            yield return StartCoroutine(Dialog.DrawChoiceBox(choices));
                            int chosenIndex = Dialog.chosenIndex;
                            Dialog.UndrawDialogBox();
                            Dialog.UndrawChoiceBox();
                            if (chosenIndex == 3)
                            {
                                //USE (unique key item) //unique effects not yet implemented
                            }
                            if (chosenIndex == 2)
                            {
                                if (selectedItem.getItemType() == ItemData.ItemType.KEY)
                                {
                                    //REGISTER (key item)
                                    Dialog.DrawDialogBox();
                                    yield return
                                        Dialog.StartCoroutine(Dialog.DrawText(
                                            "Register " + selectedItem.getName() + " to which slot?"));
                                    choices = new string[]
                                    {
                                        "Slot 1", "Slot 2", "Slot 3", "Slot 4", "Cancel"
                                    };
                                    yield return StartCoroutine(Dialog.DrawChoiceBox(choices));
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
                                        Dialog.DrawDialogBox();
                                        if (selectedItem.getItemType() != ItemData.ItemType.TM)
                                        {
                                            yield return
                                                Dialog.StartCoroutine(Dialog.DrawText(
                                                    "Use " + selectedItem.getName() + " on which Pokémon?"));
                                        }
                                        else
                                        {
                                            //display TM bootup text
                                            Dialog.DrawDialogBox(2);
                                            SfxHandler.Play(tmBootupClip);
                                            yield return StartCoroutine(Dialog.DrawTextSilent("Booted up a TM."));
                                            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                                            {
                                                yield return null;
                                            }
                                            Dialog.DrawDialogBox(2);
                                            yield return
                                                Dialog.StartCoroutine(Dialog.DrawText(
                                                    "It contained " + selectedItem.getName() + "."));
                                            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                                            {
                                                yield return null;
                                            }

                                            Dialog.DrawDialogBox(2);
                                            yield return
                                                Dialog.StartCoroutine(Dialog.DrawText(
                                                    "Teach " + selectedItem.getName() + " \nto a Pokémon?"));
                                            yield return StartCoroutine(Dialog.DrawChoiceBox());
                                            chosenIndex = Dialog.chosenIndex;
                                            Dialog.UndrawChoiceBox();
                                            if (chosenIndex == 0)
                                            {
                                                //If No is chosen
                                                selectingPokemon = false;
                                                    //set selecting false to prevent the using phase from happening.
                                            }
                                            else
                                            {
                                                Dialog.DrawDialogBox(2);
                                                yield return
                                                    Dialog.StartCoroutine(Dialog.DrawText(
                                                        "Who should learn \n" + selectedItem.getName() + "?"));
                                            }
                                        }
                                        partyPosition = 0;
                                        pkmSlot[partyPosition].sprite = pkmPartySlotSpriteSelected;

                                        // Begin Party Navigation to select target Pokemon.

                                        while (selectingPokemon)
                                        {
                                            if (Input.GetAxisRaw("Vertical") > 0)
                                            {
                                                if (partyPosition > 0)
                                                {
                                                    pkmSlot[partyPosition].sprite = pkmPartySlotSprite;
                                                    partyPosition -= 1;
                                                    pkmSlot[partyPosition].sprite = pkmPartySlotSpriteSelected;

                                                    SfxHandler.Play(selectClip);
                                                    yield return new WaitForSeconds(0.2f);
                                                }
                                            }
                                            else if (Input.GetAxisRaw("Vertical") < 0)
                                            {
                                                if (partyPosition < partyLength - 1)
                                                {
                                                    pkmSlot[partyPosition].sprite = pkmPartySlotSprite;
                                                    partyPosition += 1;
                                                    pkmSlot[partyPosition].sprite = pkmPartySlotSpriteSelected;

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
                                        yield return Dialog.StartCoroutine(Dialog.DrawText( "There's no one to use it on!"));
                                        while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                                        {
                                            yield return null;
                                        }
                                    }
                                }
                                pkmSlot[partyPosition].sprite = pkmPartySlotSprite;
                                Dialog.UndrawDialogBox();
                                Dialog.UndrawChoiceBox();
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

                        Dialog.DrawDialogBox();
                        yield return
                            Dialog.StartCoroutine(Dialog.DrawText( "Do what with " + currentItemList[selected] + "?"));
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
                        
                        yield return StartCoroutine(Dialog.DrawChoiceBox(choices));
                        int chosenIndex = Dialog.chosenIndex;
                        Dialog.UndrawDialogBox();
                        Dialog.UndrawChoiceBox();

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

                            Dialog.DrawDialogBox();
                            yield return
                                Dialog.StartCoroutine(Dialog.DrawText(
                                    "Gave " + selectedItem.getName() + " to " + currentPokemon.getName() + "."));
                            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                            {
                                yield return null;
                            }
                            Dialog.UndrawDialogBox();

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

                            Dialog.DrawDialogBox();
                            yield return
                                Dialog.StartCoroutine(Dialog.DrawText(
                                    "Took the " + receivedItem + " from " + currentPokemon.getName() + "."));
                            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                            {
                                yield return null;
                            }
                            Dialog.UndrawDialogBox();

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

                            Dialog.DrawDialogBox();
                            yield return
                                Dialog.StartCoroutine(Dialog.DrawText(
                                    "Gave " + selectedItem.getName() + " to " + currentPokemon.getName() + ","));
                            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                            {
                                yield return null;
                            }
                            Dialog.DrawDialogBox();
                            yield return
                                Dialog.StartCoroutine(Dialog.DrawText( "and received " + receivedItem + " in return."));
                            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                            {
                                yield return null;
                            }
                            Dialog.UndrawDialogBox();

                            switching = false;
                            selected = -1;
                            updateSelectedItem();
                        }
                        else if (chosenChoice == "Use")
                        {
                            yield return StartCoroutine(runItemEffect(selectedItem, currentPokemon));

                            updatePartyDisplays();

                            Dialog.UndrawDialogBox();
                            Dialog.UndrawChoiceBox();
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
                    pkmSlot[partyPosition].sprite = pkmPartySlotSprite;

                    updateScreen();
                    updateItemList();
                    updateSelectedItem();
                    SfxHandler.Play(selectClip);
                    yield return new WaitForSeconds(0.2f);
                }
                else
                {
                    running = false;
                    SfxHandler.Play(cancelClip);
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
                float startLength = pkmHPbar[partyPosition].rectTransform.sizeDelta.x;
                float difference =
                    Mathf.Floor(64f * ((float) currentPokemon.getCurrentHP() / (float) currentPokemon.getHP())) -
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

                    pkmHPbar[partyPosition].rectTransform.sizeDelta = new Vector2(
                        startLength + (difference * increment),
                        pkmHPbar[partyPosition].rectTransform.sizeDelta.y);
                    
                    //Color the bar
                    if (pkmHPbar[partyPosition].rectTransform.sizeDelta.x < 16f)
                    {
                        pkmHPbar[partyPosition].color = new Color(1, 0.125f, 0, 1);
                    }
                    else if (pkmHPbar[partyPosition].rectTransform.sizeDelta.x < 32f)
                    {
                        pkmHPbar[partyPosition].color = new Color(1, 0.75f, 0, 1);
                    }
                    else
                    {
                        pkmHPbar[partyPosition].color = new Color(0.125f, 1, 0.065f, 1);
                    }

                    yield return null;
                }

                Dialog.DrawDialogBox();
                yield return Dialog.StartCoroutine(Dialog.DrawTextSilent( "It restored " + amount + " points."));
                while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                {
                    yield return null;
                }
            }
            else
            {
                Dialog.DrawDialogBox();
                yield return Dialog.StartCoroutine(Dialog.DrawText( "It wouldn't have any effect."));
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

                    Dialog.DrawDialogBox();
                    yield return Dialog.StartCoroutine(Dialog.DrawTextSilent( "Restored all moves."));
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
                    Dialog.DrawDialogBox();
                    yield return Dialog.StartCoroutine(Dialog.DrawText( "Restore which move's PP?"));
                    yield return StartCoroutine(Dialog.DrawChoiceBox(choices));
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

                        Dialog.DrawDialogBox();
                        yield return Dialog.StartCoroutine(Dialog.DrawTextSilent( "It restored " + amount + " points."));
                        while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                        {
                            yield return null;
                        }
                    }
                }
            }
            else
            {
                Dialog.DrawDialogBox();
                yield return Dialog.StartCoroutine(Dialog.DrawText( "It wouldn't have any effect."));
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
                    float startLength = pkmHPbar[partyPosition].rectTransform.sizeDelta.x;
                    float difference =
                        Mathf.Floor(64f * ((float) currentPokemon.getCurrentHP() / (float) currentPokemon.getHP())) -
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
                        pkmHPbar[partyPosition].rectTransform.sizeDelta = new Vector2(
                            startLength + (difference * increment),
                            pkmHPbar[partyPosition].rectTransform.sizeDelta.y);
                        //Color the bar
                        if (pkmHPbar[partyPosition].rectTransform.sizeDelta.x < 16f)
                        {
                            pkmHPbar[partyPosition].color = new Color(1, 0.125f, 0, 1);
                        }
                        else if (pkmHPbar[partyPosition].rectTransform.sizeDelta.x < 32f)
                        {
                            pkmHPbar[partyPosition].color = new Color(1, 0.75f, 0, 1);
                        }
                        else
                        {
                            pkmHPbar[partyPosition].color = new Color(0.125f, 1, 0.065f, 1);
                        }

                        yield return null;
                    }

                    Dialog.DrawDialogBox();
                    yield return Dialog.StartCoroutine(Dialog.DrawTextSilent( "It restored " + amount + " points."));
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

                    Dialog.DrawDialogBox();
                    if (statusCurer == "ASLEEP")
                    {
                        yield return Dialog.StartCoroutine(Dialog.DrawTextSilent( currentPokemon.getName() + " woke up!"));
                    }
                    else if (statusCurer == "BURNED")
                    {
                        yield return Dialog.StartCoroutine(Dialog.DrawTextSilent( currentPokemon.getName() + " was healed!"));
                    }
                    else if (statusCurer == "FROZEN")
                    {
                        yield return Dialog.StartCoroutine(Dialog.DrawTextSilent( currentPokemon.getName() + " thawed out!"));
                    }
                    else if (statusCurer == "PARALYZED")
                    {
                        yield return Dialog.StartCoroutine(Dialog.DrawTextSilent( currentPokemon.getName() + " was cured!"));
                    }
                    else if (statusCurer == "POISONED")
                    {
                        yield return Dialog.StartCoroutine(Dialog.DrawTextSilent( currentPokemon.getName() + " was cured!"));
                    }

                    while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                    {
                        yield return null;
                    }
                }
            }
            else
            {
                Dialog.DrawDialogBox();
                yield return Dialog.StartCoroutine(Dialog.DrawText( "It wouldn't have any effect."));
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

                Dialog.DrawDialogBox();
                if (statBooster == "HP")
                {
                    yield return Dialog.StartCoroutine(Dialog.DrawTextSilent( currentPokemon.getName() + "'s HP rose!"));
                }
                else if (statBooster == "ATK")
                {
                    yield return Dialog.StartCoroutine(Dialog.DrawTextSilent( currentPokemon.getName() + "'s Attack rose!"));
                }
                else if (statBooster == "DEF")
                {
                    yield return Dialog.StartCoroutine(Dialog.DrawTextSilent( currentPokemon.getName() + "'s Defense rose!"));
                }
                else if (statBooster == "SPA")
                {
                    yield return
                        Dialog.StartCoroutine(Dialog.DrawTextSilent( currentPokemon.getName() + "'s Special Attack rose!"));
                }
                else if (statBooster == "SPD")
                {
                    yield return
                        Dialog.StartCoroutine(Dialog.DrawTextSilent( currentPokemon.getName() + "'s Special Defense rose!"));
                }
                else if (statBooster == "SPE")
                {
                    yield return Dialog.StartCoroutine(Dialog.DrawTextSilent( currentPokemon.getName() + "'s Speed rose!"));
                }

                while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                {
                    yield return null;
                }
                updateSelectedItem();
            }
            else
            {
                Dialog.DrawDialogBox();
                yield return Dialog.StartCoroutine(Dialog.DrawText( "It wouldn't have any effect."));
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
                Dialog.UndrawDialogBox();
            }
            else
            {
                Dialog.DrawDialogBox();
                yield return Dialog.StartCoroutine(Dialog.DrawText( "It wouldn't have any effect."));
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
                Dialog.DrawDialogBox(2);
                yield return
                    Dialog.StartCoroutine(Dialog.DrawText(
                        currentPokemon.getName() + " already knows \n" + selectedItem.getName() + "."));
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
                    Dialog.DrawDialogBox(2);
                    SfxHandler.Play(tmBootupClip);
                    yield return StartCoroutine(Dialog.DrawTextSilent("Booted up a TM."));
                    while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                    {
                        yield return null;
                    }
                    Dialog.DrawDialogBox(2);
                    yield return Dialog.StartCoroutine(Dialog.DrawText( "It contained " + selectedItem.getName() + "."));
                    while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                    {
                        yield return null;
                    }
                    Dialog.DrawDialogBox(2);
                    yield return
                        Dialog.StartCoroutine(Dialog.DrawText(
                            "Teach " + selectedItem.getName() + "\nto " + currentPokemon.getName() + "?"));
                    yield return StartCoroutine(Dialog.DrawChoiceBox());
                    Dialog.UndrawChoiceBox();

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
                Dialog.DrawDialogBox();
                yield return Dialog.StartCoroutine(Dialog.DrawText( currentPokemon.getName() + " can't learn that move."));
                while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                {
                    yield return null;
                }
            }
            Dialog.UndrawDialogBox();
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

                    Dialog.DrawDialogBox();
                    yield return
                        Dialog.StartCoroutine(Dialog.DrawTextSilent(
                            currentPokemon.getName() + "'s level rose to " + currentPokemon.getLevel() + "!"));
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
                    Dialog.DrawDialogBox();
                    yield return Dialog.StartCoroutine(Dialog.DrawText( "It wouldn't have any effect."));
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
                    Dialog.DrawDialogBox(2);
                    yield return
                        Dialog.StartCoroutine(Dialog.DrawText(
                            selectedPokemon.getName() + " wants to learn the \nmove " + move + "."));
                    while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                    {
                        yield return null;
                    }
                    Dialog.DrawDialogBox(2);
                    yield return
                        Dialog.StartCoroutine(Dialog.DrawText(
                            "However, " + selectedPokemon.getName() + " already \nknows four moves."));
                    while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                    {
                        yield return null;
                    }
                    Dialog.DrawDialogBox(2);
                    yield return
                        Dialog.StartCoroutine(Dialog.DrawText( "Should a move be deleted and \nreplaced with " + move + "?"));
                    
                    yield return StartCoroutine(Dialog.DrawChoiceBox());
                    chosenIndex = Dialog.chosenIndex;
                    Dialog.UndrawChoiceBox();
                    if (chosenIndex == 1)
                    {
                        Dialog.DrawDialogBox(2);
                        yield return Dialog.StartCoroutine(Dialog.DrawText( "Which move should \nbe forgotten?"));
                        while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                        {
                            yield return null;
                        }

                        yield return StartCoroutine(ScreenFade.main.Fade(false, ScreenFade.defaultSpeed));

                        //Set SceneSummary to be active so that it appears
                        Scene.main.Summary.gameObject.SetActive(true);
                        StartCoroutine(Scene.main.Summary.control(new Pokemon[] { selectedPokemon }, learning:learning, newMoveString:move));
                        //Start an empty loop that will only stop when SceneSummary is no longer active (is closed)
                        while (Scene.main.Summary.gameObject.activeSelf)
                        {
                            yield return null;
                        }

                        string replacedMove = Scene.main.Summary.replacedMove;
                        yield return StartCoroutine(ScreenFade.main.Fade(true, ScreenFade.defaultSpeed));

                        if (!string.IsNullOrEmpty(replacedMove))
                        {
                            Dialog.DrawDialogBox(2);
                            yield return Dialog.StartCoroutine(Dialog.DrawTextSilent( "1, "));
                            yield return new WaitForSeconds(0.4f);
                            yield return Dialog.StartCoroutine(Dialog.DrawTextSilent( "2, "));
                            yield return new WaitForSeconds(0.4f);
                            yield return Dialog.StartCoroutine(Dialog.DrawTextSilent( "and... "));
                            yield return new WaitForSeconds(0.4f);
                            yield return Dialog.StartCoroutine(Dialog.DrawTextSilent( "... "));
                            yield return new WaitForSeconds(0.4f);
                            yield return Dialog.StartCoroutine(Dialog.DrawTextSilent( "... "));
                            yield return new WaitForSeconds(0.4f);
                            SfxHandler.Play(forgetMoveClip);
                            yield return Dialog.StartCoroutine(Dialog.DrawTextSilent( "Poof!"));
                            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                            {
                                yield return null;
                            }

                            Dialog.DrawDialogBox(2);
                            yield return
                                Dialog.StartCoroutine(Dialog.DrawText(
                                    selectedPokemon.getName() + " forgot how to \nuse " + replacedMove + "."));
                            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                            {
                                yield return null;
                            }
                            Dialog.DrawDialogBox(2);
                            yield return Dialog.StartCoroutine(Dialog.DrawText( "And..."));
                            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                            {
                                yield return null;
                            }

                            Dialog.DrawDialogBox(2);
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
                        Dialog.DrawDialogBox(2);
                        yield return Dialog.StartCoroutine(Dialog.DrawText( "Give up on learning the move \n" + move + "?"));
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

                    Dialog.DrawDialogBox(2);
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
            Dialog.DrawDialogBox(2);
            yield return Dialog.StartCoroutine(Dialog.DrawText(selectedPokemon.getName() + " did not learn \n" + move + "."));
            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
            {
                yield return null;
            }
        }
    }
}