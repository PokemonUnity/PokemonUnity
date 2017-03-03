//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class TrainerHandler : MonoBehaviour
{
    private DialogBoxHandler Dialog;

    private GUITexture cancel;

    private Transform screens;

    private GUITexture card;

    private GUITexture IDnoBox;
    private GUIText IDnoText;
    private GUIText IDnoTextShadow;
    private GUIText IDnoData;
    private GUIText IDnoDataShadow;
    private GUITexture nameBox;
    private GUIText nameText;
    private GUIText nameTextShadow;
    private GUIText nameData;
    private GUIText nameDataShadow;
    private GUITexture picture;
    private GUITexture moneyBox;
    private GUIText moneyText;
    private GUIText moneyTextShadow;
    private GUIText moneyData;
    private GUIText moneyDataShadow;
    private GUITexture pokedexBox;
    private GUIText pokedexText;
    private GUIText pokedexTextShadow;
    private GUIText pokedexData;
    private GUIText pokedexDataShadow;
    private GUITexture scoreBox;
    private GUIText scoreText;
    private GUIText scoreTextShadow;
    private GUIText scoreData;
    private GUIText scoreDataShadow;
    private GUITexture timeBox;
    private GUIText timeText;
    private GUIText timeTextShadow;
    private GUIText timeHour;
    private GUIText timeHourShadow;
    private GUIText timeColon;
    private GUIText timeColonShadow;
    private GUIText timeMinute;
    private GUIText timeMinuteShadow;
    private GUITexture adventureBox;
    private GUIText adventureText;
    private GUIText adventureTextShadow;
    private GUIText adventureData;
    private GUIText adventureDataShadow;

    private GUITexture badgeBox;

    private GUITexture badgeBoxLid;
    private GUITexture GLPictureBox;
    private GUITexture GLPicture;
    private GUITexture GLNameBox;
    private GUIText GLNameData;
    private GUIText GLNameDataShadow;
    private GUITexture GLTypeBox;
    private GUITexture GLType;
    private GUITexture GLBeatenBox;
    private GUIText GLBeatenText;
    private GUIText GLBeatenTextShadow;
    private GUIText GLBeatenData;
    private GUIText GLBeatenDataShadow;
    private GUITexture[] badges = new GUITexture[12];
    private GUITexture badgeSel;

    private GUITexture background;

    private AudioSource TrainerAudio;
    public AudioClip selectClip;

    public Texture cancelTex;
    public Texture cancelHighlightTex;

    private bool running;
    private int currentScreen;
    private bool interactingScreen;
    private int currentBadge;
    private bool cancelSelected;

    void Awake()
    {
        //sceneTransition = transform.GetComponent<SceneTransition>();
        TrainerAudio = transform.GetComponent<AudioSource>();

        cancel = transform.FindChild("Cancel").GetComponent<GUITexture>();

        screens = transform.FindChild("Screens");

        card = screens.FindChild("Card").GetComponent<GUITexture>();

        IDnoBox = card.transform.FindChild("IDno").GetComponent<GUITexture>();
        IDnoText = IDnoBox.transform.FindChild("IDnoText").GetComponent<GUIText>();
        IDnoTextShadow = IDnoBox.transform.FindChild("IDnoTextShadow").GetComponent<GUIText>();
        IDnoData = IDnoBox.transform.FindChild("IDnoData").GetComponent<GUIText>();
        IDnoDataShadow = IDnoBox.transform.FindChild("IDnoDataShadow").GetComponent<GUIText>();
        nameBox = card.transform.FindChild("NameBox").GetComponent<GUITexture>();
        nameText = nameBox.transform.FindChild("NameText").GetComponent<GUIText>();
        nameTextShadow = nameBox.transform.FindChild("NameTextShadow").GetComponent<GUIText>();
        nameData = nameBox.transform.FindChild("NameData").GetComponent<GUIText>();
        nameDataShadow = nameBox.transform.FindChild("NameDataShadow").GetComponent<GUIText>();
        picture = card.transform.FindChild("Picture").GetComponent<GUITexture>();
        moneyBox = card.transform.FindChild("Money").GetComponent<GUITexture>();
        moneyText = moneyBox.transform.FindChild("MoneyText").GetComponent<GUIText>();
        moneyTextShadow = moneyBox.transform.FindChild("MoneyTextShadow").GetComponent<GUIText>();
        moneyData = moneyBox.transform.FindChild("MoneyData").GetComponent<GUIText>();
        moneyDataShadow = moneyBox.transform.FindChild("MoneyDataShadow").GetComponent<GUIText>();
        pokedexBox = card.transform.FindChild("Pokedex").GetComponent<GUITexture>();
        pokedexText = pokedexBox.transform.FindChild("PokedexText").GetComponent<GUIText>();
        pokedexTextShadow = pokedexBox.transform.FindChild("PokedexTextShadow").GetComponent<GUIText>();
        pokedexData = pokedexBox.transform.FindChild("PokedexData").GetComponent<GUIText>();
        pokedexDataShadow = pokedexBox.transform.FindChild("PokedexDataShadow").GetComponent<GUIText>();
        scoreBox = card.transform.FindChild("Score").GetComponent<GUITexture>();
        scoreText = scoreBox.transform.FindChild("ScoreText").GetComponent<GUIText>();
        scoreTextShadow = scoreBox.transform.FindChild("ScoreTextShadow").GetComponent<GUIText>();
        scoreData = scoreBox.transform.FindChild("ScoreData").GetComponent<GUIText>();
        scoreDataShadow = scoreBox.transform.FindChild("ScoreDataShadow").GetComponent<GUIText>();
        timeBox = card.transform.FindChild("Time").GetComponent<GUITexture>();
        timeText = timeBox.transform.FindChild("TimeText").GetComponent<GUIText>();
        timeTextShadow = timeText.transform.FindChild("TimeTextShadow").GetComponent<GUIText>();
        timeHour = timeBox.transform.FindChild("TimeHour").GetComponent<GUIText>();
        timeHourShadow = timeHour.transform.FindChild("TimeHourShadow").GetComponent<GUIText>();
        timeColon = timeBox.transform.FindChild("TimeColon").GetComponent<GUIText>();
        timeColonShadow = timeColon.transform.FindChild("TimeColonShadow").GetComponent<GUIText>();
        timeMinute = timeBox.transform.FindChild("TimeMinute").GetComponent<GUIText>();
        timeMinuteShadow = timeMinute.transform.FindChild("TimeMinuteShadow").GetComponent<GUIText>();
        adventureBox = card.transform.FindChild("Adventure").GetComponent<GUITexture>();
        adventureText = adventureBox.transform.FindChild("AdventureText").GetComponent<GUIText>();
        adventureTextShadow = adventureBox.transform.FindChild("AdventureTextShadow").GetComponent<GUIText>();
        adventureData = adventureBox.transform.FindChild("AdventureData").GetComponent<GUIText>();
        adventureDataShadow = adventureBox.transform.FindChild("AdventureDataShadow").GetComponent<GUIText>();

        badgeBox = screens.FindChild("BadgeBox").GetComponent<GUITexture>();

        badgeBoxLid = badgeBox.transform.FindChild("BadgeBoxLid").GetComponent<GUITexture>();
        GLPictureBox = badgeBox.transform.FindChild("GLPictureBox").GetComponent<GUITexture>();
        GLPicture = GLPictureBox.transform.FindChild("Picture").GetComponent<GUITexture>();
        GLNameBox = badgeBox.transform.FindChild("GLNameBox").GetComponent<GUITexture>();
        GLNameData = GLNameBox.transform.FindChild("NameData").GetComponent<GUIText>();
        GLNameDataShadow = GLNameBox.transform.FindChild("NameDataShadow").GetComponent<GUIText>();
        GLTypeBox = badgeBox.transform.FindChild("GLTypeBox").GetComponent<GUITexture>();
        GLType = GLTypeBox.transform.FindChild("Type").GetComponent<GUITexture>();
        GLBeatenBox = badgeBox.transform.FindChild("GLBeatenBox").GetComponent<GUITexture>();
        GLBeatenText = GLBeatenBox.transform.FindChild("BeatenText").GetComponent<GUIText>();
        GLBeatenTextShadow = GLBeatenBox.transform.FindChild("BeatenTextShadow").GetComponent<GUIText>();
        GLBeatenData = GLBeatenBox.transform.FindChild("BeatenData").GetComponent<GUIText>();
        GLBeatenDataShadow = GLBeatenBox.transform.FindChild("BeatenDataShadow").GetComponent<GUIText>();

        Transform badgesObject = badgeBox.transform.FindChild("Badges");

        badges[0] = badgesObject.FindChild("Badge0").GetComponent<GUITexture>();
        badges[1] = badgesObject.FindChild("Badge1").GetComponent<GUITexture>();
        badges[2] = badgesObject.FindChild("Badge2").GetComponent<GUITexture>();
        badges[3] = badgesObject.FindChild("Badge3").GetComponent<GUITexture>();
        badges[4] = badgesObject.FindChild("Badge4").GetComponent<GUITexture>();
        badges[5] = badgesObject.FindChild("Badge5").GetComponent<GUITexture>();
        badges[6] = badgesObject.FindChild("Badge6").GetComponent<GUITexture>();
        badges[7] = badgesObject.FindChild("Badge7").GetComponent<GUITexture>();
        badges[8] = badgesObject.FindChild("Badge8").GetComponent<GUITexture>();
        badges[9] = badgesObject.FindChild("Badge9").GetComponent<GUITexture>();
        badges[10] = badgesObject.FindChild("Badge10").GetComponent<GUITexture>();
        badges[11] = badgesObject.FindChild("Badge11").GetComponent<GUITexture>();
        badgeSel = badgesObject.FindChild("BadgeSel").GetComponent<GUITexture>();

        background = transform.FindChild("background").GetComponent<GUITexture>();
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
            badgeBoxLid.pixelInset = new Rect(6, 20, 252, 165);
            float increment = 0;
            float startY = badgeBoxLid.pixelInset.y;
            while (increment < 1)
            {
                increment += (1 / openCloseSpeed) * Time.deltaTime;
                if (increment > 1)
                {
                    increment = 1;
                }
                badgeBoxLid.pixelInset = new Rect(badgeBoxLid.pixelInset.x, startY + (162f * increment), 252,
                    165f - (160f * increment));
                yield return null;
            }
            badgeBoxLid.pixelInset = new Rect(badgeBoxLid.pixelInset.x, badgeBoxLid.pixelInset.y + 5, 252, -5);
            increment = 0;
            startY = badgeBoxLid.pixelInset.y;
            while (increment < 1)
            {
                increment += (1 / backSpeed) * Time.deltaTime;
                if (increment > 1)
                {
                    increment = 1;
                }
                badgeBoxLid.pixelInset = new Rect(badgeBoxLid.pixelInset.x, startY + (112f * increment), 252,
                    -5f - (112f * increment));
                yield return null;
            }
        }
        else
        {
            badgeBoxLid.pixelInset = new Rect(6, 299, 252, -177);
            float increment = 0;
            float startY = badgeBoxLid.pixelInset.y;
            while (increment < 1)
            {
                increment += (1 / backSpeed) * Time.deltaTime;
                if (increment > 1)
                {
                    increment = 1;
                }
                badgeBoxLid.pixelInset = new Rect(badgeBoxLid.pixelInset.x, startY - (112f * increment), 252,
                    -117f + (112f * increment));
                yield return null;
            }
            badgeBoxLid.pixelInset = new Rect(badgeBoxLid.pixelInset.x, badgeBoxLid.pixelInset.y - 5, 252, +5);
            increment = 0;
            startY = badgeBoxLid.pixelInset.y;
            while (increment < 1)
            {
                increment += (1 / openCloseSpeed) * Time.deltaTime;
                if (increment > 1)
                {
                    increment = 1;
                }
                badgeBoxLid.pixelInset = new Rect(badgeBoxLid.pixelInset.x, startY - (162f * increment), 252,
                    5f + (160f * increment));
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
                    StartCoroutine("boxLid", false);
                }
                else if (currentScreen == 0)
                {
                    destinationPosition = new Vector3(0, 0, 0);
                    currentBadge = 0;
                    badgeSel.pixelInset = badges[0].pixelInset;
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
                    badgeSel.pixelInset = badges[0].pixelInset;
                    StartCoroutine("boxLid", true);
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
        //picture.texture = null; //player sprites not yet implemented.
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
                    GLPicture.texture = null;
                    GLNameData.text = "Jade";
                    GLType.texture = Resources.Load<Texture>("PCSprites/typeROCK");
                }
                else if (currentBadge == 1)
                {
                    GLPicture.texture = null;
                    GLNameData.text = "Bob";
                    GLType.texture = Resources.Load<Texture>("PCSprites/typeNORMAL");
                }
                else if (currentBadge == 2)
                {
                    GLPicture.texture = null;
                    GLNameData.text = "Avery";
                    GLType.texture = Resources.Load<Texture>("PCSprites/typeFLYING");
                }
                else if (currentBadge == 3)
                {
                    GLPicture.texture = null;
                    GLNameData.text = "Linda";
                    GLType.texture = Resources.Load<Texture>("PCSprites/typeGRASS");
                }
                else if (currentBadge == 4)
                {
                    GLPicture.texture = null;
                    GLNameData.text = "Cole";
                    GLType.texture = Resources.Load<Texture>("PCSprites/typeFIRE");
                }
                else if (currentBadge == 5)
                {
                    GLPicture.texture = null;
                    GLNameData.text = "Brooke";
                    GLType.texture = Resources.Load<Texture>("PCSprites/typeWATER");
                }
                else if (currentBadge == 6)
                {
                    GLPicture.texture = null;
                    GLNameData.text = "Doug";
                    GLType.texture = Resources.Load<Texture>("PCSprites/typeGROUND");
                }
                else if (currentBadge == 7)
                {
                    GLPicture.texture = null;
                    GLNameData.text = "Apalala";
                    GLType.texture = Resources.Load<Texture>("PCSprites/typeDRAGON");
                }
                else if (currentBadge == 8)
                {
                    GLPicture.texture = null;
                    GLNameData.text = "Zinka";
                    GLType.texture = Resources.Load<Texture>("PCSprites/typeSTEEL");
                }
                else if (currentBadge == 9)
                {
                    GLPicture.texture = null;
                    GLNameData.text = "";
                    GLType.texture = Resources.Load<Texture>("PCSprites/typePSYCHIC");
                }
                else if (currentBadge == 10)
                {
                    GLPicture.texture = null;
                    GLNameData.text = "";
                    GLType.texture = Resources.Load<Texture>("PCSprites/typeGHOST");
                }
                else if (currentBadge == 11)
                {
                    GLPicture.texture = null;
                    GLNameData.text = "";
                    GLType.texture = Resources.Load<Texture>("PCSprites/typeDARK");
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
                GLPicture.texture = null;
                GLNameData.text = "???";
                GLType.texture = null;
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

    private IEnumerator moveBadgeSelect(GUITexture target)
    {
        float increment = 0;
        float moveSpeed = 0.2f;
        float startX = badgeSel.pixelInset.x;
        float startY = badgeSel.pixelInset.y;
        float distanceX = target.pixelInset.x - startX;
        float distanceY = target.pixelInset.y - startY;
        while (increment < 1)
        {
            increment += (1 / moveSpeed) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }
            badgeSel.pixelInset = new Rect(startX + (increment * distanceX), startY + (increment * distanceY),
                badgeSel.pixelInset.width, badgeSel.pixelInset.height);
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
                background.pixelInset = new Rect(Mathf.RoundToInt(-32f * increment), Mathf.RoundToInt(32f * increment),
                    background.pixelInset.width, background.pixelInset.height);
                yield return null;
            }
        }
    }


    public IEnumerator control()
    {
        screens.position = new Vector3(0, 0, 0);
        badgeBoxLid.pixelInset = new Rect(6, 20, 252, 165);
        //sceneTransition.FadeIn();
        StartCoroutine(ScreenFade.main.Fade(true, ScreenFade.defaultSpeed));

        running = true;
        StartCoroutine("animBG");
        StartCoroutine("animColon");

        cancelSelected = false;
        cancel.texture = cancelTex;
        updateData();
        currentScreen = 1;
        interactingScreen = false;
        badgeSel.enabled = false;
        currentBadge = 0;
        updateSelectedBadge();
        badgeSel.pixelInset = badges[0].pixelInset;


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
                        cancel.texture = cancelTex;
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
                        cancel.texture = cancelTex;
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
                        cancel.texture = cancelHighlightTex;
                        badgeSel.enabled = false;
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                else if (!cancelSelected)
                {
                    SfxHandler.Play(selectClip);
                    cancelSelected = true;
                    cancel.texture = cancelHighlightTex;
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
                        cancel.texture = cancelTex;
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
                    cancel.texture = cancelHighlightTex;
                    running = false;
                }
            }
            yield return null;
        }
        if (currentScreen != 1)
        {
            StartCoroutine("boxLid", true);
        }
        //yield return new WaitForSeconds(sceneTransition.FadeOut());
        yield return StartCoroutine(ScreenFade.main.Fade(false, ScreenFade.defaultSpeed));
        this.gameObject.SetActive(false);
    }
}