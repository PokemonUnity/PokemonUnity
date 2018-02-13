//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class TrainerHandler : MonoBehaviour
{
    #region Variables
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
    #endregion

    void Awake()
    {
        //sceneTransition = transform.GetComponent<SceneTransition>();
        TrainerAudio = transform.GetComponent<AudioSource>();

        cancel = transform.Find("Cancel").GetComponent<GUITexture>();

        screens = transform.Find("Screens");

        card = screens.Find("Card").GetComponent<GUITexture>();

        IDnoBox = card.transform.Find("IDno").GetComponent<GUITexture>();
        IDnoText = IDnoBox.transform.Find("IDnoText").GetComponent<GUIText>();
        IDnoTextShadow = IDnoBox.transform.Find("IDnoTextShadow").GetComponent<GUIText>();
        IDnoData = IDnoBox.transform.Find("IDnoData").GetComponent<GUIText>();
        IDnoDataShadow = IDnoBox.transform.Find("IDnoDataShadow").GetComponent<GUIText>();
        nameBox = card.transform.Find("NameBox").GetComponent<GUITexture>();
        nameText = nameBox.transform.Find("NameText").GetComponent<GUIText>();
        nameTextShadow = nameBox.transform.Find("NameTextShadow").GetComponent<GUIText>();
        nameData = nameBox.transform.Find("NameData").GetComponent<GUIText>();
        nameDataShadow = nameBox.transform.Find("NameDataShadow").GetComponent<GUIText>();
        picture = card.transform.Find("Picture").GetComponent<GUITexture>();
        moneyBox = card.transform.Find("Money").GetComponent<GUITexture>();
        moneyText = moneyBox.transform.Find("MoneyText").GetComponent<GUIText>();
        moneyTextShadow = moneyBox.transform.Find("MoneyTextShadow").GetComponent<GUIText>();
        moneyData = moneyBox.transform.Find("MoneyData").GetComponent<GUIText>();
        moneyDataShadow = moneyBox.transform.Find("MoneyDataShadow").GetComponent<GUIText>();
        pokedexBox = card.transform.Find("Pokedex").GetComponent<GUITexture>();
        pokedexText = pokedexBox.transform.Find("PokedexText").GetComponent<GUIText>();
        pokedexTextShadow = pokedexBox.transform.Find("PokedexTextShadow").GetComponent<GUIText>();
        pokedexData = pokedexBox.transform.Find("PokedexData").GetComponent<GUIText>();
        pokedexDataShadow = pokedexBox.transform.Find("PokedexDataShadow").GetComponent<GUIText>();
        scoreBox = card.transform.Find("Score").GetComponent<GUITexture>();
        scoreText = scoreBox.transform.Find("ScoreText").GetComponent<GUIText>();
        scoreTextShadow = scoreBox.transform.Find("ScoreTextShadow").GetComponent<GUIText>();
        scoreData = scoreBox.transform.Find("ScoreData").GetComponent<GUIText>();
        scoreDataShadow = scoreBox.transform.Find("ScoreDataShadow").GetComponent<GUIText>();
        timeBox = card.transform.Find("Time").GetComponent<GUITexture>();
        timeText = timeBox.transform.Find("TimeText").GetComponent<GUIText>();
        timeTextShadow = timeText.transform.Find("TimeTextShadow").GetComponent<GUIText>();
        timeHour = timeBox.transform.Find("TimeHour").GetComponent<GUIText>();
        timeHourShadow = timeHour.transform.Find("TimeHourShadow").GetComponent<GUIText>();
        timeColon = timeBox.transform.Find("TimeColon").GetComponent<GUIText>();
        timeColonShadow = timeColon.transform.Find("TimeColonShadow").GetComponent<GUIText>();
        timeMinute = timeBox.transform.Find("TimeMinute").GetComponent<GUIText>();
        timeMinuteShadow = timeMinute.transform.Find("TimeMinuteShadow").GetComponent<GUIText>();
        adventureBox = card.transform.Find("Adventure").GetComponent<GUITexture>();
        adventureText = adventureBox.transform.Find("AdventureText").GetComponent<GUIText>();
        adventureTextShadow = adventureBox.transform.Find("AdventureTextShadow").GetComponent<GUIText>();
        adventureData = adventureBox.transform.Find("AdventureData").GetComponent<GUIText>();
        adventureDataShadow = adventureBox.transform.Find("AdventureDataShadow").GetComponent<GUIText>();

        badgeBox = screens.Find("BadgeBox").GetComponent<GUITexture>();

        badgeBoxLid = badgeBox.transform.Find("BadgeBoxLid").GetComponent<GUITexture>();
        GLPictureBox = badgeBox.transform.Find("GLPictureBox").GetComponent<GUITexture>();
        GLPicture = GLPictureBox.transform.Find("Picture").GetComponent<GUITexture>();
        GLNameBox = badgeBox.transform.Find("GLNameBox").GetComponent<GUITexture>();
        GLNameData = GLNameBox.transform.Find("NameData").GetComponent<GUIText>();
        GLNameDataShadow = GLNameBox.transform.Find("NameDataShadow").GetComponent<GUIText>();
        GLTypeBox = badgeBox.transform.Find("GLTypeBox").GetComponent<GUITexture>();
        GLType = GLTypeBox.transform.Find("Type").GetComponent<GUITexture>();
        GLBeatenBox = badgeBox.transform.Find("GLBeatenBox").GetComponent<GUITexture>();
        GLBeatenText = GLBeatenBox.transform.Find("BeatenText").GetComponent<GUIText>();
        GLBeatenTextShadow = GLBeatenBox.transform.Find("BeatenTextShadow").GetComponent<GUIText>();
        GLBeatenData = GLBeatenBox.transform.Find("BeatenData").GetComponent<GUIText>();
        GLBeatenDataShadow = GLBeatenBox.transform.Find("BeatenDataShadow").GetComponent<GUIText>();

        Transform badgesObject = badgeBox.transform.Find("Badges");

        badges[0] = badgesObject.Find("Badge0").GetComponent<GUITexture>();
        badges[1] = badgesObject.Find("Badge1").GetComponent<GUITexture>();
        badges[2] = badgesObject.Find("Badge2").GetComponent<GUITexture>();
        badges[3] = badgesObject.Find("Badge3").GetComponent<GUITexture>();
        badges[4] = badgesObject.Find("Badge4").GetComponent<GUITexture>();
        badges[5] = badgesObject.Find("Badge5").GetComponent<GUITexture>();
        badges[6] = badgesObject.Find("Badge6").GetComponent<GUITexture>();
        badges[7] = badgesObject.Find("Badge7").GetComponent<GUITexture>();
        badges[8] = badgesObject.Find("Badge8").GetComponent<GUITexture>();
        badges[9] = badgesObject.Find("Badge9").GetComponent<GUITexture>();
        badges[10] = badgesObject.Find("Badge10").GetComponent<GUITexture>();
        badges[11] = badgesObject.Find("Badge11").GetComponent<GUITexture>();
        badgeSel = badgesObject.Find("BadgeSel").GetComponent<GUITexture>();

        background = transform.Find("background").GetComponent<GUITexture>();
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
        moneyData.text = "$" + playerMoney;//¥
        moneyDataShadow.text = moneyData.text;
        pokedexData.text = "0"; //pokedex not yet implemented.
        pokedexDataShadow.text = pokedexData.text;
        scoreData.text = "" + SaveData.currentSave.playerScore;
        scoreDataShadow.text = scoreData.text;
        System.TimeSpan playTime = SaveData.currentSave.playerTime + SaveData.currentSave.startTime.Subtract(System.DateTime.UtcNow);
        timeHour.text = "";//playTime.Hours.ToString();//"" + SaveData.currentSave.playerHours;
        timeHourShadow.text = timeHour.text;
        timeMinute.text = System.String.Format("{0}   {1:00}", playTime.Hours, playTime.Minutes); //playTime.Minutes.ToString("00");//"" + SaveData.currentSave.playerMinutes;
        /*if (timeMinute.text.Length == 1)
        {
            timeMinute.text = "0" + timeMinute.text;
        }*/
        timeMinuteShadow.text = timeMinute.text;
        adventureData.text = SaveData.currentSave.fileCreationDate.Value.ToString("MMM d, yyyy");
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
                /* There isnt a difference, but i just feel like it's visually cleaner on the eyes. Less confusing...
                switch (currentBadge) 
                {
                    case 0:
                        GLPicture.texture = null;
                        GLNameData.text = "Jade";
                        GLType.texture = Resources.Load<Texture>("PCSprites/typeROCK");
                        break;
                    default:
                        GLPicture.texture = null;
                        GLNameData.text = "???";
                        GLType.texture = null;//Resources.Load<Texture>("PCSprites/typeNONE")? Isnt there a '???' Type?
                        break;
                }*/
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

                if (SaveData.currentSave.gymsBeatTime[currentBadge] != null)//(SaveData.currentSave.gymsBeaten[currentBadge])
                {
                    GLBeatenBox.gameObject.SetActive(true);
                    GLBeatenData.text = SaveData.currentSave.gymsBeatTime[currentBadge].Value.ToString("MMM d, yyyy");
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