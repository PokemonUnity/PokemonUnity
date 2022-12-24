//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TrainerHandler : MonoBehaviour
{
    private DialogBoxHandlerNew Dialog;

    private Image cancel;

    private Transform screens;

    private Image card;

    private Image IDnoBox;
    private Text IDnoText;
    private Text IDnoTextShadow;
    private Text IDnoData;
    private Text IDnoDataShadow;
    private Image nameBox;
    private Text nameText;
    private Text nameTextShadow;
    private Text nameData;
    private Text nameDataShadow;
    private Image picture;
    private Image moneyBox;
    private Text moneyText;
    private Text moneyTextShadow;
    private Text moneyData;
    private Text moneyDataShadow;
    private Image pokedexBox;
    private Text pokedexText;
    private Text pokedexTextShadow;
    private Text pokedexData;
    private Text pokedexDataShadow;
    private Image scoreBox;
    private Text scoreText;
    private Text scoreTextShadow;
    private Text scoreData;
    private Text scoreDataShadow;
    private Image timeBox;
    private Text timeText;
    private Text timeTextShadow;
    private Text timeHour;
    private Text timeHourShadow;
    private Text timeColon;
    private Text timeColonShadow;
    private Text timeMinute;
    private Text timeMinuteShadow;
    private Image adventureBox;
    private Text adventureText;
    private Text adventureTextShadow;
    private Text adventureData;
    private Text adventureDataShadow;

    private Image badgeBox;

    private Image badgeBoxLid;
    private Image GLPictureBox;
    private Image GLPicture;
    private Image GLNameBox;
    private Text GLNameData;
    private Text GLNameDataShadow;
    private Image GLTypeBox;
    private Image GLType;
    private Image GLBeatenBox;
    private Text GLBeatenText;
    private Text GLBeatenTextShadow;
    private Text GLBeatenData;
    private Text GLBeatenDataShadow;
    private Image[] badges = new Image[12];
    private Image badgeSel;

    private Image background;

    private AudioSource TrainerAudio;
    public AudioClip selectClip;

    public Sprite cancelTex;
    public Sprite cancelHighlightTex;

    private bool running;
    private int currentScreen;
    private bool interactingScreen;
    private int currentBadge;
    private bool cancelSelected;

    void Awake()
    {
        //sceneTransition = transform.GetComponent<SceneTransition>();
        TrainerAudio = transform.GetComponent<AudioSource>();

        cancel = transform.Find("Cancel").GetComponent<Image>();

        screens = transform.Find("Screens");

        card = screens.Find("Card").GetComponent<Image>();

        IDnoBox = card.transform.Find("IDno").GetComponent<Image>();
        IDnoText = IDnoBox.transform.Find("IDnoText").GetComponent<Text>();
        IDnoTextShadow = IDnoBox.transform.Find("IDnoTextShadow").GetComponent<Text>();
        IDnoData = IDnoBox.transform.Find("IDnoData").GetComponent<Text>();
        IDnoDataShadow = IDnoBox.transform.Find("IDnoDataShadow").GetComponent<Text>();
        nameBox = card.transform.Find("NameBox").GetComponent<Image>();
        nameText = nameBox.transform.Find("NameText").GetComponent<Text>();
        nameTextShadow = nameBox.transform.Find("NameTextShadow").GetComponent<Text>();
        nameData = nameBox.transform.Find("NameData").GetComponent<Text>();
        nameDataShadow = nameBox.transform.Find("NameDataShadow").GetComponent<Text>();
        picture = card.transform.Find("Picture").GetComponent<Image>();
        moneyBox = card.transform.Find("Money").GetComponent<Image>();
        moneyText = moneyBox.transform.Find("MoneyText").GetComponent<Text>();
        moneyTextShadow = moneyBox.transform.Find("MoneyTextShadow").GetComponent<Text>();
        moneyData = moneyBox.transform.Find("MoneyData").GetComponent<Text>();
        moneyDataShadow = moneyBox.transform.Find("MoneyDataShadow").GetComponent<Text>();
        pokedexBox = card.transform.Find("Pokedex").GetComponent<Image>();
        pokedexText = pokedexBox.transform.Find("PokedexText").GetComponent<Text>();
        pokedexTextShadow = pokedexBox.transform.Find("PokedexTextShadow").GetComponent<Text>();
        pokedexData = pokedexBox.transform.Find("PokedexData").GetComponent<Text>();
        pokedexDataShadow = pokedexBox.transform.Find("PokedexDataShadow").GetComponent<Text>();
        scoreBox = card.transform.Find("Score").GetComponent<Image>();
        scoreText = scoreBox.transform.Find("ScoreText").GetComponent<Text>();
        scoreTextShadow = scoreBox.transform.Find("ScoreTextShadow").GetComponent<Text>();
        scoreData = scoreBox.transform.Find("ScoreData").GetComponent<Text>();
        scoreDataShadow = scoreBox.transform.Find("ScoreDataShadow").GetComponent<Text>();
        timeBox = card.transform.Find("Time").GetComponent<Image>();
        timeText = timeBox.transform.Find("TimeText").GetComponent<Text>();
        timeTextShadow = timeText.transform.Find("TimeTextShadow").GetComponent<Text>();
        timeHour = timeBox.transform.Find("TimeHour").GetComponent<Text>();
        timeHourShadow = timeHour.transform.Find("TimeHourShadow").GetComponent<Text>();
        timeColon = timeBox.transform.Find("TimeColon").GetComponent<Text>();
        timeColonShadow = timeColon.transform.Find("TimeColonShadow").GetComponent<Text>();
        timeMinute = timeBox.transform.Find("TimeMinute").GetComponent<Text>();
        timeMinuteShadow = timeMinute.transform.Find("TimeMinuteShadow").GetComponent<Text>();
        adventureBox = card.transform.Find("Adventure").GetComponent<Image>();
        adventureText = adventureBox.transform.Find("AdventureText").GetComponent<Text>();
        adventureTextShadow = adventureBox.transform.Find("AdventureTextShadow").GetComponent<Text>();
        adventureData = adventureBox.transform.Find("AdventureData").GetComponent<Text>();
        adventureDataShadow = adventureBox.transform.Find("AdventureDataShadow").GetComponent<Text>();

        badgeBox = screens.Find("BadgeBox").GetComponent<Image>();

        badgeBoxLid = badgeBox.transform.Find("BadgeBoxLid").GetComponent<Image>();
        GLPictureBox = badgeBox.transform.Find("GLPictureBox").GetComponent<Image>();
        GLPicture = GLPictureBox.transform.Find("Picture").GetComponent<Image>();
        GLNameBox = badgeBox.transform.Find("GLNameBox").GetComponent<Image>();
        GLNameData = GLNameBox.transform.Find("NameData").GetComponent<Text>();
        GLNameDataShadow = GLNameBox.transform.Find("NameDataShadow").GetComponent<Text>();
        GLTypeBox = badgeBox.transform.Find("GLTypeBox").GetComponent<Image>();
        GLType = GLTypeBox.transform.Find("Type").GetComponent<Image>();
        GLBeatenBox = badgeBox.transform.Find("GLBeatenBox").GetComponent<Image>();
        GLBeatenText = GLBeatenBox.transform.Find("BeatenText").GetComponent<Text>();
        GLBeatenTextShadow = GLBeatenBox.transform.Find("BeatenTextShadow").GetComponent<Text>();
        GLBeatenData = GLBeatenBox.transform.Find("BeatenData").GetComponent<Text>();
        GLBeatenDataShadow = GLBeatenBox.transform.Find("BeatenDataShadow").GetComponent<Text>();

        Transform badgesObject = badgeBox.transform.Find("Badges");

        badges[0] = badgesObject.Find("Badge0").GetComponent<Image>();
        badges[1] = badgesObject.Find("Badge1").GetComponent<Image>();
        badges[2] = badgesObject.Find("Badge2").GetComponent<Image>();
        badges[3] = badgesObject.Find("Badge3").GetComponent<Image>();
        badges[4] = badgesObject.Find("Badge4").GetComponent<Image>();
        badges[5] = badgesObject.Find("Badge5").GetComponent<Image>();
        badges[6] = badgesObject.Find("Badge6").GetComponent<Image>();
        badges[7] = badgesObject.Find("Badge7").GetComponent<Image>();
        badges[8] = badgesObject.Find("Badge8").GetComponent<Image>();
        badges[9] = badgesObject.Find("Badge9").GetComponent<Image>();
        badges[10] = badgesObject.Find("Badge10").GetComponent<Image>();
        badges[11] = badgesObject.Find("Badge11").GetComponent<Image>();
        badgeSel = badgesObject.Find("BadgeSel").GetComponent<Image>();

        background = transform.Find("background").GetComponent<Image>();
    }

    void Start()
    {
        updateData();
        this.gameObject.SetActive(false);
    }

    private IEnumerator boxLid(bool shutting)
    {
        float waitTime = 0.15f;
        float openCloseSpeed = 0.25f;
        float backSpeed = 0.2f;
        if (!shutting)
        {
            yield return new WaitForSeconds(waitTime);
            badgeBoxLid.rectTransform.position = new Vector2(6, 20);
            badgeBoxLid.rectTransform.sizeDelta = new Vector2(252, 165);
            
            float increment = 0;
            float startY = badgeBoxLid.rectTransform.position.y;
            while (increment < 1)
            {
                increment += (1 / openCloseSpeed) * Time.deltaTime;
                if (increment > 1)
                {
                    increment = 1;
                }
                badgeBoxLid.rectTransform.position = new Vector2(badgeBoxLid.rectTransform.position.x, startY + (162f * increment));
                badgeBoxLid.rectTransform.sizeDelta = new Vector2(252, 165f - (160f * increment));
                yield return null;
            }
            badgeBoxLid.rectTransform.position = new Vector2(badgeBoxLid.rectTransform.position.x, badgeBoxLid.rectTransform.position.y + 5);
            badgeBoxLid.rectTransform.position = new Vector2(252, -5);
            increment = 0;
            startY = badgeBoxLid.rectTransform.position.y;
            while (increment < 1)
            {
                increment += (1 / backSpeed) * Time.deltaTime;
                if (increment > 1)
                {
                    increment = 1;
                }
                badgeBoxLid.rectTransform.position = new Vector2(badgeBoxLid.rectTransform.position.x, startY + (112f * increment));
                badgeBoxLid.rectTransform.sizeDelta = new Vector2(252, -5f - (112f * increment));
                yield return null;
            }
        }
        else
        {
            badgeBoxLid.rectTransform.position = new Vector2(6, 299);
            badgeBoxLid.rectTransform.sizeDelta = new Vector2(252, -177);
            float increment = 0;
            float startY = badgeBoxLid.rectTransform.position.y;
            while (increment < 1)
            {
                increment += (1 / backSpeed) * Time.deltaTime;
                if (increment > 1)
                {
                    increment = 1;
                }
                badgeBoxLid.rectTransform.position = new Vector2(badgeBoxLid.rectTransform.position.x, startY - (112f * increment));
                badgeBoxLid.rectTransform.sizeDelta = new Vector2(252, -117f + (112f * increment));
                yield return null;
            }
            badgeBoxLid.rectTransform.position = new Vector2(badgeBoxLid.rectTransform.position.x, badgeBoxLid.rectTransform.position.y - 5);
            badgeBoxLid.rectTransform.sizeDelta = new Vector2(252, 5);
            increment = 0;
            startY = badgeBoxLid.rectTransform.position.y;
            while (increment < 1)
            {
                increment += (1 / openCloseSpeed) * Time.deltaTime;
                if (increment > 1)
                {
                    increment = 1;
                }
                badgeBoxLid.rectTransform.position = new Vector2(badgeBoxLid.rectTransform.position.x, startY - (162f * increment));
                badgeBoxLid.rectTransform.sizeDelta = new Vector2(252, 5f + (160f * increment));
                yield return null;
            }
        }
    }

    private float moveVisableScreen(int direction)
    {
        float moveSpeed = 0.6f;
        Vector3 destinationPosition = new Vector3(0, 0, 0);
        if (interactingScreen)
        {
            if (currentScreen == 1)
            {
                destinationPosition = new Vector3(-0.92f, 0, 0);
            }
        }
        else
        {
            if (direction > 0)
            {
                if (currentScreen == 1)
                {
                    destinationPosition = new Vector3(-0.806f, 0, 0);
                    StartCoroutine(boxLid(false));
                }
                else if (currentScreen == 0)
                {
                    destinationPosition = new Vector3(0, 0, 0);
                    currentBadge = 0;
                    badgeSel.rectTransform.position = badges[0].rectTransform.position;
                }
            }
            else
            {
                /*	if(currentScreen == 1){ //this code will move the screens across to show the
                                              //post-game gym badge box.
                        
                    }
                    else */
                if (currentScreen == 2)
                {
                    destinationPosition = new Vector3(0, 0, 0);
                    currentBadge = 0;
                    badgeSel.rectTransform.position = badges[0].rectTransform.position;
                    StartCoroutine(boxLid(true));
                }
            }
        }
        StartCoroutine(moveScreen(destinationPosition, moveSpeed));
        return moveSpeed;
    }

    private void updateData()
    {
        IDnoData.text = "" + SaveData.currentSave.playerID;
        IDnoDataShadow.text = IDnoData.text;
        nameData.text = SaveData.currentSave.playerName;
        nameDataShadow.text = nameData.text;
        //picture.sprite = null; //player sprites not yet implemented.
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
        moneyData.text = "$" + playerMoney;
        moneyDataShadow.text = moneyData.text;
        pokedexData.text = "0"; //pokedex not yet implemented.
        pokedexDataShadow.text = pokedexData.text;
        scoreData.text = "" + SaveData.currentSave.playerScore;
        scoreDataShadow.text = scoreData.text;
        timeHour.text = "" + SaveData.currentSave.playerHours;
        timeHourShadow.text = timeHour.text;
        timeMinute.text = "" + SaveData.currentSave.playerMinutes;
        if (timeMinute.text.Length == 1)
        {
            timeMinute.text = "0" + timeMinute.text;
        }
        timeMinuteShadow.text = timeMinute.text;
        adventureData.text = SaveData.currentSave.fileCreationDate;
        adventureDataShadow.text = adventureData.text;

        for (int i = 0; i < 12; i++)
        {
            if (SaveData.currentSave.gymsBeaten[i])
            {
                badges[i].enabled = true;
            }
            else
            {
                badges[i].enabled = false;
            }
        }
    }

    private void updateSelectedBadge()
    {
        if (interactingScreen)
        {
            GLNameBox.gameObject.SetActive(true);
            GLPictureBox.gameObject.SetActive(true);
            GLTypeBox.gameObject.SetActive(true);
            if (SaveData.currentSave.gymsEncountered[currentBadge])
            {
                if (currentBadge == 0)
                {
                    GLPicture.sprite = null;
                    GLNameData.text = "Jade";
                    GLType.sprite = Resources.Load<Sprite>("PCSprites/typeROCK");
                }
                else if (currentBadge == 1)
                {
                    GLPicture.sprite = null;
                    GLNameData.text = "Bob";
                    GLType.sprite = Resources.Load<Sprite>("PCSprites/typeNORMAL");
                }
                else if (currentBadge == 2)
                {
                    GLPicture.sprite = null;
                    GLNameData.text = "Avery";
                    GLType.sprite = Resources.Load<Sprite>("PCSprites/typeFLYING");
                }
                else if (currentBadge == 3)
                {
                    GLPicture.sprite = null;
                    GLNameData.text = "Linda";
                    GLType.sprite = Resources.Load<Sprite>("PCSprites/typeGRASS");
                }
                else if (currentBadge == 4)
                {
                    GLPicture.sprite = null;
                    GLNameData.text = "Cole";
                    GLType.sprite = Resources.Load<Sprite>("PCSprites/typeFIRE");
                }
                else if (currentBadge == 5)
                {
                    GLPicture.sprite = null;
                    GLNameData.text = "Brooke";
                    GLType.sprite = Resources.Load<Sprite>("PCSprites/typeWATER");
                }
                else if (currentBadge == 6)
                {
                    GLPicture.sprite = null;
                    GLNameData.text = "Doug";
                    GLType.sprite = Resources.Load<Sprite>("PCSprites/typeGROUND");
                }
                else if (currentBadge == 7)
                {
                    GLPicture.sprite = null;
                    GLNameData.text = "Apalala";
                    GLType.sprite = Resources.Load<Sprite>("PCSprites/typeDRAGON");
                }
                else if (currentBadge == 8)
                {
                    GLPicture.sprite = null;
                    GLNameData.text = "Zinka";
                    GLType.sprite = Resources.Load<Sprite>("PCSprites/typeSTEEL");
                }
                else if (currentBadge == 9)
                {
                    GLPicture.sprite = null;
                    GLNameData.text = "";
                    GLType.sprite = Resources.Load<Sprite>("PCSprites/typePSYCHIC");
                }
                else if (currentBadge == 10)
                {
                    GLPicture.sprite = null;
                    GLNameData.text = "";
                    GLType.sprite = Resources.Load<Sprite>("PCSprites/typeGHOST");
                }
                else if (currentBadge == 11)
                {
                    GLPicture.sprite = null;
                    GLNameData.text = "";
                    GLType.sprite = Resources.Load<Sprite>("PCSprites/typeDARK");
                }

                if (SaveData.currentSave.gymsBeaten[currentBadge])
                {
                    GLBeatenBox.gameObject.SetActive(true);
                    GLBeatenData.text = SaveData.currentSave.gymsBeatTime[currentBadge];
                }
                else
                {
                    GLBeatenBox.gameObject.SetActive(false);
                }
                GLNameDataShadow.text = GLNameData.text;
                GLBeatenDataShadow.text = GLBeatenData.text;
            }
            else
            {
                GLPicture.sprite = null;
                GLNameData.text = "???";
                GLType.sprite = null;
                GLBeatenBox.gameObject.SetActive(false);
                GLNameDataShadow.text = GLNameData.text;
            }
        }
        else
        {
            GLPictureBox.gameObject.SetActive(false);
            GLNameBox.gameObject.SetActive(false);
            GLTypeBox.gameObject.SetActive(false);
            GLBeatenBox.gameObject.SetActive(false);
        }
    }

    private IEnumerator moveBadgeSelect(Image target)
    {
        float increment = 0;
        float moveSpeed = 0.2f;
        float startX = badgeSel.rectTransform.position.x;
        float startY = badgeSel.rectTransform.position.y;
        float distanceX = target.rectTransform.position.x - startX;
        float distanceY = target.rectTransform.position.y - startY;
        while (increment < 1)
        {
            increment += (1 / moveSpeed) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }
            badgeSel.rectTransform.position = new Vector2(startX + (increment * distanceX), startY + (increment * distanceY));
            yield return null;
        }
    }

    private IEnumerator moveScreen(Vector3 destinationPosition, float moveSpeed)
    {
        Vector3 startPosition = screens.position;
        float increment = 0;
        while (increment < 1)
        {
            increment += (1 / moveSpeed) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }
            screens.position = Vector3.Lerp(startPosition, destinationPosition, increment);
            yield return null;
        }
        screens.position = destinationPosition;
    }

    private IEnumerator animColon()
    {
        while (running)
        {
            timeColon.text = ":";
            timeColonShadow.text = timeColon.text;
            yield return new WaitForSeconds(0.6f);
            timeColon.text = "";
            timeColonShadow.text = timeColon.text;
            yield return new WaitForSeconds(0.6f);
        }
    }

    private IEnumerator animBG()
    {
        float scrollSpeed = 1.2f;
        while (running)
        {
            float increment = 0;
            while (increment < 1)
            {
                increment += (1 / scrollSpeed) * Time.deltaTime;
                if (increment > 1)
                {
                    increment = 1;
                }
                background.rectTransform.position = new Vector2(Mathf.RoundToInt(-32f * increment), Mathf.RoundToInt(32f * increment));
                yield return null;
            }
        }
    }


    public IEnumerator control()
    {
        screens.position = new Vector3(0, 0, 0);
        badgeBoxLid.rectTransform.position = new Vector2(6, 20);
        badgeBoxLid.rectTransform.sizeDelta = new Vector2(252, 165);
        //sceneTransition.FadeIn();
        StartCoroutine(ScreenFade.main.Fade(true, ScreenFade.defaultSpeed));

        running = true;
        StartCoroutine(animBG());
        StartCoroutine(animColon());

        cancelSelected = false;
        cancel.sprite = cancelTex;
        updateData();
        currentScreen = 1;
        interactingScreen = false;
        badgeSel.enabled = false;
        currentBadge = 0;
        updateSelectedBadge();
        badgeSel.rectTransform.position = badges[0].rectTransform.position;


        while (running)
        {
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                if (!interactingScreen)
                {
                    if (currentScreen < 2)
                    {
                        SfxHandler.Play(selectClip);
                        yield return new WaitForSeconds(moveVisableScreen(1));
                        currentScreen += 1;
                    }
                }
                else
                {
                    if (currentBadge < 11 && currentBadge != 5 && !cancelSelected)
                    {
                        SfxHandler.Play(selectClip);
                        currentBadge += 1;
                        updateSelectedBadge();
                        yield return StartCoroutine(moveBadgeSelect(badges[currentBadge]));
                    }
                }
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                if (!interactingScreen)
                {
                    if (currentScreen > 1)
                    {
                        SfxHandler.Play(selectClip);
                        yield return new WaitForSeconds(moveVisableScreen(-1));
                        currentScreen -= 1;
                    }
                }
                else
                {
                    if (currentBadge > 0 && currentBadge != 6 && !cancelSelected)
                    {
                        SfxHandler.Play(selectClip);
                        currentBadge -= 1;
                        updateSelectedBadge();
                        yield return StartCoroutine(moveBadgeSelect(badges[currentBadge]));
                    }
                }
            }
            else if (Input.GetAxisRaw("Vertical") > 0)
            {
                if (interactingScreen)
                {
                    if (cancelSelected)
                    {
                        SfxHandler.Play(selectClip);
                        cancelSelected = false;
                        cancel.sprite = cancelTex;
                        badgeSel.enabled = true;
                        yield return new WaitForSeconds(0.2f);
                    }
                    else if (currentBadge > 5)
                    {
                        SfxHandler.Play(selectClip);
                        currentBadge -= 6;
                        updateSelectedBadge();
                        yield return StartCoroutine(moveBadgeSelect(badges[currentBadge]));
                    }
                }
                else
                {
                    if (cancelSelected)
                    {
                        SfxHandler.Play(selectClip);
                        cancelSelected = false;
                        cancel.sprite = cancelTex;
                        yield return new WaitForSeconds(0.2f);
                    }
                }
            }
            else if (Input.GetAxisRaw("Vertical") < 0)
            {
                if (interactingScreen)
                {
                    if (currentBadge < 6)
                    {
                        SfxHandler.Play(selectClip);
                        currentBadge += 6;
                        updateSelectedBadge();
                        yield return StartCoroutine(moveBadgeSelect(badges[currentBadge]));
                    }
                    else if (!cancelSelected)
                    {
                        SfxHandler.Play(selectClip);
                        cancelSelected = true;
                        cancel.sprite = cancelHighlightTex;
                        badgeSel.enabled = false;
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                else if (!cancelSelected)
                {
                    SfxHandler.Play(selectClip);
                    cancelSelected = true;
                    cancel.sprite = cancelHighlightTex;
                    yield return new WaitForSeconds(0.2f);
                }
            }
            else if (Input.GetButton("Select"))
            {
                if (cancelSelected && !interactingScreen)
                {
                    SfxHandler.Play(selectClip);
                    running = false;
                }
                else if (currentScreen == 2)
                {
                    if (!interactingScreen)
                    {
                        SfxHandler.Play(selectClip);
                        yield return StartCoroutine(moveScreen(new Vector3(-0.92f, 0, 0), 0.2f));
                        interactingScreen = true;
                        updateSelectedBadge();
                        badgeSel.enabled = true;
                    }
                    else if (cancelSelected)
                    {
                        SfxHandler.Play(selectClip);
                        badgeSel.enabled = false;
                        interactingScreen = false;
                        updateSelectedBadge();
                        cancelSelected = false;
                        cancel.sprite = cancelTex;
                        yield return StartCoroutine(moveScreen(new Vector3(-0.806f, 0, 0), 0.2f));
                    }
                }
            }
            else if (Input.GetButton("Back"))
            {
                if (interactingScreen)
                {
                    SfxHandler.Play(selectClip);
                    badgeSel.enabled = false;
                    interactingScreen = false;
                    updateSelectedBadge();
                    yield return StartCoroutine(moveScreen(new Vector3(-0.806f, 0, 0), 0.2f));
                }
                else
                {
                    SfxHandler.Play(selectClip);
                    cancelSelected = true;
                    cancel.sprite = cancelHighlightTex;
                    running = false;
                }
            }
            yield return null;
        }
        if (currentScreen != 1)
        {
            StartCoroutine(boxLid(true));
        }
        //yield return new WaitForSeconds(sceneTransition.FadeOut());
        yield return StartCoroutine(ScreenFade.main.Fade(false, ScreenFade.defaultSpeed));
        this.gameObject.SetActive(false);
    }
}