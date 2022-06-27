//Original Scripts by IIColour (IIColour_Spectrum)

using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//using UnityEngine.Rendering.PostProcessing;

public class PauseHandler : MonoBehaviour
{
    /*
    private GUITexture pauseTop;
    private GUITexture pauseBottom;

    private GUIText selectedText;
    private GUIText selectedTextShadow;
    
    public Texture2D iconPokedexTex;
    public Texture2D iconPartyTex;
    public Texture2D iconBagTex;
    public Texture2D iconTrainerTex;
    public Texture2D iconSaveTex;
    public Texture2D iconSettingsTex;
    */

    /*
    private RotatableGUIItem iconPokedex;
    private RotatableGUIItem iconParty;
    private RotatableGUIItem iconBag;
    private RotatableGUIItem iconTrainer;
    private RotatableGUIItem iconSave;
    private RotatableGUIItem iconSettings;
    */

    public GameObject
        slotParty,
        slotPokedex,
        slotBag,
        slotTrainer,
        slotSave,
        slotSettings;

    private Vector3[] slotPositions;

    public Sprite[]
        slotSprite,
        partySpriteIcon,
        pokedexSpriteIcon,
        bagSpriteIcon,
        trainerSpriteIcon,
        saveSpriteIcon,
        settingsSpriteIcon;

    private Image
        imageParty,
        imagePokedex,
        imageBag,
        imageTrainer,
        imageSave,
        imageSettings,

        iconPokedex,
        iconParty,
        iconBag,
        iconTrainer,
        iconSave,
        iconSettings;

    private Text trainerText;
    private Text trainerShadowText;

    private GameObject saveDataDisplay;
    private Text mapName;
    private Text mapNameShadow;
    private Text badges;
    private Text badgesShadow;
    private Text date;
    private Text dateShadow;
    private Text hour;
    private Text hourShadow;
    private Text pokedex;
    private Text pokedexShadow;
    private Text playTime;
    private Text playTimeShadow;
    private Image[] saveParty = new Image[6];

    private DialogBoxHandlerNew Dialog;

    private AudioSource PauseAudio;
    public AudioClip selectClip;
    public AudioClip openClip;
    public AudioClip decideClip;
    public AudioClip cancelClip;
    public AudioClip saveClip;

    public Text money;
    public Image[] pkmTeam;
    public Image[] pkmPV;

    private int selectedIcon;
    private Image targetImage;
    private Image targetIcon;

    private int targetIndex;

    private bool running;

    private RectTransform phone;
    
    private float transitionTime = 0.25f;
    
    private class App
    {
        public IEnumerator behavior;

        public App
            appUp = null,
            appDown = null,
            appLeft = null,
            appRight = null;

        public int index;
        
        public enum Type
        {
            NotImplemented,
            Save,
            Menu
        }

        public Type type;
        public GameObject scene;

        public App(int index)
        {
            this.index = index;
        }
    }
    
    private App party = new (1);
    private App pokedexApp = new (2);
    private App bag = new (3);
    private App trainer = new (4);
    private App save = new (5);
    private App settings = new (6);

    void Awake()
    {
        phone = transform.Find("phone").GetComponent<RectTransform>();
        
        PauseAudio = transform.GetComponent<AudioSource>();

        imageParty = slotParty.GetComponent<Image>();
        imagePokedex = slotPokedex.GetComponent<Image>();
        imageBag = slotBag.GetComponent<Image>();
        imageTrainer = slotTrainer.GetComponent<Image>();
        imageSave = slotSave.GetComponent<Image>();
        imageSettings = slotSettings.GetComponent<Image>();

        iconPokedex = slotPokedex.transform.Find("icon").GetComponent<Image>();
        iconParty = slotParty.transform.Find("icon").GetComponent<Image>();
        iconBag = slotBag.transform.Find("icon").GetComponent<Image>();
        iconTrainer = slotTrainer.transform.Find("icon").GetComponent<Image>();
        iconSave = slotSave.transform.Find("icon").GetComponent<Image>();
        iconSettings = slotSettings.transform.Find("icon").GetComponent<Image>();

        trainerText = slotTrainer.transform.Find("text").GetComponent<Text>();
        trainerShadowText = slotTrainer.transform.Find("text_shadow").GetComponent<Text>();

        //setup save display
        Dialog = transform.Find("savedisplay").GetComponent<DialogBoxHandlerNew>();
        
        saveDataDisplay = transform.Find("savedisplay").gameObject;

        mapNameShadow = saveDataDisplay.transform.Find("CurrentMap").GetComponent<Text>();
        mapName = mapNameShadow.transform.Find("text").GetComponent<Text>();
        badgesShadow = saveDataDisplay.transform.Find("Badges").GetComponent<Text>();
        badges = badgesShadow.transform.Find("text").GetComponent<Text>();
        dateShadow = saveDataDisplay.transform.Find("Date").GetComponent<Text>();
        date = dateShadow.transform.Find("text").GetComponent<Text>();
        hourShadow = saveDataDisplay.transform.Find("Hour").GetComponent<Text>();
        hour = hourShadow.transform.Find("text").GetComponent<Text>();
        pokedexShadow = saveDataDisplay.transform.Find("Encounters").GetComponent<Text>();
        pokedex = pokedexShadow.transform.Find("text").GetComponent<Text>();
        playTimeShadow = saveDataDisplay.transform.Find("PlayTime").GetComponent<Text>();
        playTime = playTimeShadow.transform.Find("text").GetComponent<Text>();
        
        //setup save party icons
        for (int i = 0; i < saveParty.Length; ++i)
        {
            int index = i + 1;
            saveParty[i] = saveDataDisplay.transform.Find("party" + index).GetComponent<Image>();
        }
        
        /*
        pauseTop = transform.Find("PauseTop").GetComponent<GUITexture>();
        pauseBottom = transform.Find("PauseBottom").GetComponent<GUITexture>();

        selectedText = transform.Find("SelectedText").GetComponent<GUIText>();
        selectedTextShadow = selectedText.transform.Find("SelectedTextShadow").GetComponent<GUIText>();

        iconPokedex = transform.Find("IconPokedex").GetComponent<RotatableGUIItem>();
        iconParty = transform.Find("IconParty").GetComponent<RotatableGUIItem>();
        iconBag = transform.Find("IconBag").GetComponent<RotatableGUIItem>();
        iconTrainer = transform.Find("IconTrainer").GetComponent<RotatableGUIItem>();
        iconSave = transform.Find("IconSave").GetComponent<RotatableGUIItem>();
        iconSettings = transform.Find("IconSettings").GetComponent<RotatableGUIItem>();
        */

        /*
        saveDataDisplay = transform.Find("SaveDataDisplay").GetComponent<GUITexture>();
        mapName = saveDataDisplay.transform.Find("MapName").GetComponent<GUIText>();
        mapNameShadow = mapName.transform.Find("MapNameShadow").GetComponent<GUIText>();
        dataText = saveDataDisplay.transform.Find("DataText").GetComponent<GUIText>();
        dataTextShadow = dataText.transform.Find("DataTextShadow").GetComponent<GUIText>();
        */
    }

    void Start()
    {
        transform.Find("left_interface").gameObject.SetActive(false);
        
        party.type = App.Type.Menu;
        party.scene = Scene.main.Party.gameObject;
        party.appDown = bag;
        party.appRight = pokedexApp;
        
        pokedexApp.type = App.Type.NotImplemented;
        pokedexApp.behavior = notYetImplementedMenu();
        pokedexApp.appDown = trainer;
        pokedexApp.appLeft = party;
        
        bag.type = App.Type.Menu;
        bag.scene = Scene.main.Bag.gameObject;
        bag.appDown = save;
        bag.appUp = party;
        bag.appRight = trainer;
        
        trainer.type = App.Type.Menu;
        trainer.scene = Scene.main.Trainer.gameObject;
        trainer.appDown = settings;
        trainer.appUp = pokedexApp;
        trainer.appLeft = bag;
        
        save.type = App.Type.Save;
        save.behavior = startSaveMenu();
        save.appUp = bag;
        save.appRight = settings;
        
        settings.type = App.Type.Menu;
        settings.scene = Scene.main.Settings.gameObject;
        settings.appUp = trainer;
        settings.appLeft = save;
        
        /*
        pauseTop.pixelInset = new Rect(0, 192, pauseTop.pixelInset.width, pauseTop.pixelInset.height);
        pauseBottom.pixelInset = new Rect(0, -96, pauseBottom.pixelInset.width, pauseBottom.pixelInset.height);

        setSelectedText("");

        iconPokedex.transform.position = new Vector3(iconPokedex.transform.position.x, -0.0833f, 1);
        iconParty.transform.position = new Vector3(iconParty.transform.position.x, -0.0833f, 1);
        iconBag.transform.position = new Vector3(iconBag.transform.position.x, -0.0833f, 1);
        iconTrainer.transform.position = new Vector3(iconTrainer.transform.position.x, 1.0833f, 1);
        iconSave.transform.position = new Vector3(iconSave.transform.position.x, 1.0833f, 1);
        iconSettings.transform.position = new Vector3(iconSettings.transform.position.x, 1.0833f, 1);
        
        */
        selectedIcon = 0;
        
        /*
        saveDataDisplay.gameObject.SetActive(false);
        */
        
        slotPositions = new []
        {
            slotParty.transform.position,
            slotPokedex.transform.position,
            slotBag.transform.position,
            slotTrainer.transform.position,
            slotSave.transform.position,
            slotSettings.transform.position
            
        };
        
        this.gameObject.SetActive(false);
    }
    
    /*
    private IEnumerator openAnim()
    {
        float increment = 0;
        while (increment < 1)
        {
            increment += (1 / 0.4f) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }
            pauseBottom.pixelInset = new Rect(pauseBottom.pixelInset.x, -96 + (increment * 96),
                pauseBottom.pixelInset.width, pauseBottom.pixelInset.height);
            pauseTop.pixelInset = new Rect(pauseTop.pixelInset.x, (192 - (increment * 96))*2, pauseTop.pixelInset.width,
                pauseTop.pixelInset.height);
            iconPokedex.transform.position = new Vector3(iconPokedex.transform.position.x,
                -0.0833f + (increment * 0.4167f), 1);
            iconParty.transform.position = new Vector3(iconParty.transform.position.x, -0.0833f + (increment * 0.4167f),
                1);
            iconBag.transform.position = new Vector3(iconBag.transform.position.x, -0.0833f + (increment * 0.4167f), 1);
            iconTrainer.transform.position = new Vector3(iconTrainer.transform.position.x,
                1.0833f - (increment * 0.4167f), 1);
            iconSave.transform.position = new Vector3(iconSave.transform.position.x, 1.0833f - (increment * 0.4167f), 1);
            iconSettings.transform.position = new Vector3(iconSettings.transform.position.x,
                1.0833f - (increment * 0.4167f), 1);
            yield return null;
        }
    }

    private IEnumerator closeAnim()
    {
        float increment = 0;
        setSelectedText("");
        while (increment < 1)
        {
            increment += (1 / 0.4f) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }
            pauseBottom.pixelInset = new Rect(pauseBottom.pixelInset.x, 0 - (increment * 96),
                pauseBottom.pixelInset.width, pauseBottom.pixelInset.height);
            pauseTop.pixelInset = new Rect(pauseTop.pixelInset.x, (96 + (increment * 96))*2, pauseTop.pixelInset.width,
                pauseTop.pixelInset.height);
            iconPokedex.transform.position = new Vector3(iconPokedex.transform.position.x,
                0.3333f - (increment * 0.4167f), 1);
            iconParty.transform.position = new Vector3(iconParty.transform.position.x, 0.3333f - (increment * 0.4167f),
                1);
            iconBag.transform.position = new Vector3(iconBag.transform.position.x, 0.3333f - (increment * 0.4167f), 1);
            iconTrainer.transform.position = new Vector3(iconTrainer.transform.position.x,
                0.6667f + (increment * 0.4167f), 1);
            iconSave.transform.position = new Vector3(iconSave.transform.position.x, 0.6667f + (increment * 0.4167f), 1);
            iconSettings.transform.position = new Vector3(iconSettings.transform.position.x,
                0.6667f + (increment * 0.4167f), 1);
            yield return null;
        }
    }

    public IEnumerator animActiveIcon()
    {
        float increment = 1f;
        float parabola = 0;
        float wobbleSpeed = 0.8f;
        while (running)
        {
            while (increment > -1)
            {
                increment -= (1 / wobbleSpeed) * Time.deltaTime;
                if (increment < -1)
                {
                    increment = -1;
                }
                parabola = 0.82f * (increment * increment) - 1;
                targetIcon.angle = (parabola * -15);
                yield return null;
            }
            while (increment < 1)
            {
                increment += (1 / wobbleSpeed) * Time.deltaTime;
                if (increment > 1)
                {
                    increment = 1;
                }
                parabola = 0.82f * (increment * increment) - 1;
                targetIcon.angle = (parabola * 15);
                yield return null;
            }
        }
    }
    */

    public Sprite[] getIconSprite(int index)
    {
        Sprite[] localSprites;
        switch (index)
        {
            case 1:
                localSprites = partySpriteIcon;
                break;
            case 2:
                localSprites = pokedexSpriteIcon;
                break;
            case 3:
                localSprites = bagSpriteIcon;
                break;
            case 4:
                localSprites = trainerSpriteIcon;
                break;
            case 5:
                localSprites = saveSpriteIcon;
                break;
            case 6:
                localSprites = settingsSpriteIcon;
                break;
            default:
                localSprites = partySpriteIcon;
                break;
        }

        return localSprites;
    }

    public IEnumerator updateIcon(int index)
    {
        if (targetIndex != null && targetImage != null && targetIcon != null)
        {
            targetImage.sprite = slotSprite[0];
            targetIcon.sprite = getIconSprite(targetIndex)[0];
        }
        //TODO remplacer les targeticon par des targetImage
        if (selectedIcon == 1)
        {
            targetImage = imageParty;
            targetIcon = iconParty;
        }
        else if (selectedIcon == 2)
        {
            targetImage = imagePokedex;
            targetIcon = iconPokedex;
        }
        else if (selectedIcon == 3)
        {
            targetImage = imageBag;
            targetIcon = iconBag;
        }
        else if (selectedIcon == 4)
        {
            targetImage = imageTrainer;
            targetIcon = iconTrainer;
        }
        else if (selectedIcon == 5)
        {
            targetImage = imageSave;
            targetIcon = iconSave;
        }
        else if (selectedIcon == 6)
        {
            targetImage = imageSettings;
            targetIcon = iconSettings;
        }
        else
        {
            targetImage = null;
            targetIcon = null;
        }

        if (targetImage != null && targetIcon != null)
        {
            targetImage.sprite = slotSprite[1];
            targetIcon.sprite = getIconSprite(selectedIcon)[1];
        }
        
        targetIndex = index;

        /*
        iconPokedex.angle = 0;
        iconParty.angle = 0;
        iconBag.angle = 0;
        iconTrainer.angle = 0;
        iconSave.angle = 0;
        iconSettings.angle = 0;

        iconPokedex.size = new Vector2(32, 32);
        iconParty.size = new Vector2(32, 32);
        iconBag.size = new Vector2(32, 32);
        iconTrainer.size = new Vector2(32, 32);
        iconSave.size = new Vector2(32, 32);
        iconSettings.size = new Vector2(32, 32);
        

        if (targetIcon != null)
        {
            StopCoroutine("animActiveIcon");
            StartCoroutine("animActiveIcon");
            //pulse
            float increment = 0f;
            float pulseSpeed = 0.15f;
            while (increment < 1)
            {
                increment += (1 / pulseSpeed) * Time.deltaTime;
                if (increment > 1)
                {
                    increment = 1;
                }
                targetIcon.size = new Vector2(32 + (16 * increment), 32 + (16 * increment));
                yield return null;
            }
            increment = 0f;
            while (increment < 1)
            {
                increment += (1 / pulseSpeed) * Time.deltaTime;
                if (increment > 1)
                {
                    increment = 1;
                }
                targetIcon.size = new Vector2(48 - (16 * increment), 48 - (16 * increment));
                yield return null;
            }
        }
        */
        
        yield return null;
    }

    /*
    
    private void hideIcons()
    {
        iconPokedex.hide = true;
        iconParty.hide = true;
        iconBag.hide = true;
        iconTrainer.hide = true;
        iconSave.hide = true;
        iconSettings.hide = true;
    }

    private void unhideIcons()
    {
        iconPokedex.hide = false;
        iconParty.hide = false;
        iconBag.hide = false;
        iconTrainer.hide = false;
        iconSave.hide = false;
        iconSettings.hide = false;
    }

    private IEnumerator fadeIcons(float speed)
    {
        float increment = 0f;
        while (increment < 1)
        {
            increment += (1 / speed * 1.2f) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }
            iconPokedex.color = new Color(1f - increment, 1f - increment, 1f - increment, 1f);
            iconParty.color = new Color(1f - increment, 1f - increment, 1f - increment, 1f);
            iconBag.color = new Color(1f - increment, 1f - increment, 1f - increment, 1f);
            iconTrainer.color = new Color(1f - increment, 1f - increment, 1f - increment, 1f);
            iconSave.color = new Color(1f - increment, 1f - increment, 1f - increment, 1f);
            iconSettings.color = new Color(1f - increment, 1f - increment, 1f - increment, 1f);
            yield return null;
        }
    }

    private IEnumerator unfadeIcons(float speed)
    {
        float increment = 0f;
        while (increment < 1)
        {
            increment += (1 / speed * 1.2f) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }
            iconPokedex.color = new Color(increment, increment, increment, 1f);
            iconParty.color = new Color(increment, increment, increment, 1f);
            iconBag.color = new Color(increment, increment, increment, 1f);
            iconTrainer.color = new Color(increment, increment, increment, 1f);
            iconSave.color = new Color(increment, increment, increment, 1f);
            iconSettings.color = new Color(increment, increment, increment, 1f);
            yield return null;
        }
    }
    */
    

    private IEnumerator notYetImplementedMenu()
    {
        Debug.Log("Pokédex not yet implemented");
        yield return new WaitForSeconds(0.2f);
    }
    
    private IEnumerator startMenu(GameObject sceneObject)
    {
        //Party
        SfxHandler.Play(decideClip);

        //Fade In
        yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));
        //Start the scene
        yield return StartCoroutine(runSceneUntilDeactivated(sceneObject));
        //Fade Out
        yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));
    }

    private IEnumerator startSaveMenu()
    {
        //Save

        saveDataDisplay.gameObject.SetActive(true);
        
        int badgeTotal = 0;
        for (int i = 0; i < 12; i++)
        {
            if (SaveData.currentSave.gymsBeaten[i])
            {
                badgeTotal += 1;
            }
        }
        string playerTime = "" + SaveData.currentSave.playerMinutes;
        if (playerTime.Length == 1)
        {
            playerTime = "0" + playerTime;
        }
        playerTime = SaveData.currentSave.playerHours + " : " + playerTime;
        
        //display party
        for (int i = 0; i < saveParty.Length; ++i)
        {
            Pokemon selectedPokemon = SaveData.currentSave.PC.boxes[0][i];

            if (selectedPokemon == null)
            {
                saveParty[i].gameObject.SetActive(false);
            }
            else
            {
                saveParty[i].gameObject.SetActive(true);
                saveParty[i].sprite = selectedPokemon.GetIconsSprite()[0];
            }
        }

        mapName.text = PlayerMovement.player.accessedMapSettings.mapName;
        date.text = SaveData.currentSave.playerName; //date not yet implemented
        hour.text = ""; //hour not yet implemented
        badges.text = ""+badgeTotal;
        pokedex.text = "0"; //pokedex not yet implemented
        playTime.text = playerTime;


        mapNameShadow.text = mapName.text;
        dateShadow.text = date.text;
        hourShadow.text = hour.text;
        badgesShadow.text = badges.text;
        pokedexShadow.text = pokedex.text;
        playTimeShadow.text = playTime.text;

        Dialog.DrawDialogBox();
        yield return StartCoroutine(Dialog.DrawText("Would you like to save the game?"));
        
        yield return new WaitForSeconds(0.2f);
        yield return StartCoroutine(Dialog.DrawChoiceBox(0));
        int chosenIndex = Dialog.chosenIndex;
        if (chosenIndex == 1)
        {
            //update save file
            Dialog.UndrawChoiceBox();
            Dialog.DrawDialogBox();
            
            // Saving the Scene and Positions of the player & follower
            SaveData.currentSave.levelName = Application.loadedLevelName;
            SaveData.currentSave.playerPosition = new SeriV3(PlayerMovement.player.transform.position);
            SaveData.currentSave.playerDirection = PlayerMovement.player.direction;
            SaveData.currentSave.followerPosition = new SeriV3(PlayerMovement.player.followerScript.transform.position);
            SaveData.currentSave.followerdirection = PlayerMovement.player.followerScript.direction;
            SaveData.currentSave.mapName = PlayerMovement.player.accessedMapSettings.mapName;
            SaveData.currentSave.followerOut = GlobalVariables.global.followerOut;

            // Saving the Non Resetting Events
            NonResettingHandler.saveDataToGlobal();

            SaveLoad.Save();
            
            SfxHandler.Play(saveClip);
            yield return
                StartCoroutine(Dialog.DrawText(SaveData.currentSave.playerName + " saved the game!"));
            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
            {
                yield return null;
            }
        }
        else if (chosenIndex == 0)
        {
            SfxHandler.Play(cancelClip);
        }
        Dialog.UndrawDialogBox();
        Dialog.UndrawChoiceBox();
        saveDataDisplay.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
    }

    public IEnumerator Blur(float speed)
    {
        //PostProcessVolume postProcessVolume = PlayerMovement.player.mainCamera.GetComponent<PostProcessVolume>();
        //DepthOfField dof;
        //
        //postProcessVolume.profile.TryGetSettings(out dof);
        //
        //dof.active = true;
        //
        //while (dof.focalLength.GetValue<float>() < 21)
        //{
        //    dof.focalLength.Override(dof.focalLength + Time.deltaTime * speed);
        //
        //    if (dof.focalLength.GetValue<float>() > 21) dof.focalLength.Override(21f);
        //    
        //    yield return null;
        //}
        yield return null;
    }
    
    public IEnumerator UnBlur(float speed)
    {
        //PostProcessVolume postProcessVolume = PlayerMovement.player.mainCamera.GetComponent<PostProcessVolume>();
        //DepthOfField dof;
        //
        //postProcessVolume.profile.TryGetSettings(out dof);
        //
        //while (dof.focalLength.GetValue<float>() > 1)
        //{
        //    dof.focalLength.Override(dof.focalLength - Time.deltaTime * speed);
        //
        //    if (dof.focalLength.GetValue<float>() < 1) dof.focalLength.Override(1);
        //    
        //    yield return null;
        //}
        //
        //dof.active = false;
        yield return null;
    }

    public IEnumerator animateDevelopArrow()
    {
        RectTransform arrow = transform.Find("develop").GetComponent<RectTransform>();
        int x = 61;
        arrow.gameObject.SetActive(true);

        while (true)
        {
            LeanTween.moveX(arrow, x, 0);
            yield return new WaitForSeconds(0.2f);
            LeanTween.moveX(arrow, x + 1, 0);
            yield return new WaitForSeconds(0.2f);
            LeanTween.moveX(arrow, x, 0);
            yield return new WaitForSeconds(0.2f);
            LeanTween.moveX(arrow, x - 1, 0);
            yield return new WaitForSeconds(0.2f);
        }
    }

    public IEnumerator control()
    {
        Coroutine developArrowAnim;
        GameObject left_interface = transform.Find("left_interface").gameObject;
        
        bool noPokemon = SaveData.currentSave.PC.getBoxLength(0) == 0;
        bool noPokedex = SaveData.currentSave.getCVariable("has_pokedex") == 0;
        bool noTrainerCard = SaveData.currentSave.getCVariable("has_trainerCard") == 0;
        
        trainerText.text = SaveData.currentSave.playerName;
        trainerShadowText.text = trainerText.text;
        
        /* Reset apps */
        
        party.type = App.Type.Menu;
        party.scene = Scene.main.Party.gameObject;
        party.appDown = bag;
        party.appRight = pokedexApp;
        
        pokedexApp.type = App.Type.NotImplemented;
        pokedexApp.behavior = notYetImplementedMenu();
        pokedexApp.appDown = trainer;
        pokedexApp.appLeft = party;
        
        bag.type = App.Type.Menu;
        bag.scene = Scene.main.Bag.gameObject;
        bag.appDown = save;
        bag.appUp = party;
        bag.appRight = trainer;
        
        trainer.type = App.Type.Menu;
        trainer.scene = Scene.main.Trainer.gameObject;
        trainer.appDown = settings;
        trainer.appUp = pokedexApp;
        trainer.appLeft = bag;
        
        save.type = App.Type.Save;
        save.behavior = startSaveMenu();
        save.appUp = bag;
        save.appRight = settings;
        
        settings.type = App.Type.Menu;
        settings.scene = Scene.main.Settings.gameObject;
        settings.appUp = trainer;
        settings.appLeft = save;
        
        slotParty.transform.localScale = Vector3.one;
        slotPokedex.transform.localScale = Vector3.one;
        slotTrainer.transform.localScale = Vector3.one;
        
        /*   */
        
        App selectedApp = party;
        selectedIcon = 1;
        if (noPokemon) //noPokemon
        {
            selectedApp = pokedexApp;
            selectedIcon = 2;

            // Cut links between party App and others
            bag.appUp = null;
            pokedexApp.appLeft = null;
            slotParty.transform.localScale = new Vector3(0,0,0);
        }
        if (noPokedex) //noPokedex && noPokemon
        {
            selectedApp = party;

            // Cut links between pokedex App and others
            trainer.appUp = null;
            party.appRight = null;
            slotPokedex.transform.localScale = new Vector3(0,0,0);

            if (noPokemon)
            {
                selectedIcon = 3;
                selectedApp = bag;
            }
        }
        if (noTrainerCard) // no Trainer Card
        {
            bag.appRight = null;
            settings.appUp = null;
            pokedexApp.appDown = null;
            slotTrainer.transform.localScale = new Vector3(0,0,0);
        }

        float speed = 50;

        //unhideIcons();
        yield return StartCoroutine("updateIcon", selectedIcon);

        SfxHandler.Play(openClip);
        
        StartCoroutine(Blur(speed));
        LeanTween.moveX(phone, 73, transitionTime);
        yield return new WaitForSeconds(transitionTime);
        
        left_interface.SetActive(true);
        money.text = "$" + SaveData.currentSave.playerMoney;
        money.transform.parent.GetComponent<Text>().text = money.text;

        if (SaveData.currentSave.PC.boxes[0].Length > 0)
        {
            pkmTeam[0].transform.parent.gameObject.SetActive(true);
            for (int i = 0; i < pkmTeam.Length; ++i)
            {
                if (SaveData.currentSave.PC.boxes[0][i] != null)
                {
                    Pokemon p = SaveData.currentSave.PC.boxes[0][i];
                    
                    pkmTeam[i].gameObject.SetActive(true);
                    pkmTeam[i].sprite = p.GetIconsSprite()[0];

                    if (p.getStatus() == Pokemon.Status.FAINTED)
                    {
                        pkmTeam[i].color = new Color(1, 1, 1, 0.2f);
                    }
                    else if (p.getStatus() == Pokemon.Status.POISONED)
                    {
                        pkmTeam[i].color = new Color(1, 0, 1, 1);
                    }
                    else if (p.getStatus() == Pokemon.Status.PARALYZED)
                    {
                        pkmTeam[i].color = new Color(1, 1, 0, 1);
                    }
                    else if (p.getStatus() == Pokemon.Status.ASLEEP)
                    {
                        pkmTeam[i].color = new Color(0.5f, 0.5f, 0.5f, 1);
                    }
                    else if (p.getStatus() == Pokemon.Status.BURNED)
                    {
                        pkmTeam[i].color = new Color(1, 0, 0, 1);
                    }
                    else if (p.getStatus() == Pokemon.Status.FROZEN)
                    {
                        pkmTeam[i].color = new Color(0, 1, 1, 1);
                    }
                    else
                    {
                        pkmTeam[i].color = new Color(1, 1, 1, 1);
                    }
                    
                    // PV Bar
                    float hp = p.getPercentHP();
                    pkmPV[i].GetComponent<RectTransform>().sizeDelta = new Vector2(hp * 22, 2);
                    
                    pkmPV[i].gameObject.SetActive(true);

                    pkmPV[i].color = new Color(0, 1, 0, 1);
                    
                    if (hp < 0.1)
                    {
                        pkmPV[i].color = new Color(1, 0, 0, 1);
                    }
                    else if (hp < 0.5)
                    {
                        pkmPV[i].color = new Color(1, 0.6f, 0f, 1);
                    }

                }
                else
                {
                    pkmTeam[i].sprite = null;
                    pkmTeam[i].gameObject.SetActive(false);
                    pkmPV[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            pkmTeam[0].transform.parent.gameObject.SetActive(false);
        }
        
        
        developArrowAnim = StartCoroutine(animateDevelopArrow());
        /*
        app_text.gameObject.SetActive(true);
        app_text_shadow.gameObject.SetActive(true);
        
        if (Language.getLang() == Language.Country.FRANCAIS)
        {
            app_text.text = "Appuyez sur R pour voir les applications";
        }
        else
        {
            app_text.text = "Press R to see applications";
        }
        app_text_shadow.text = app_text.text;
        */
        
        yield return new WaitForSeconds(0.2f);

        running = true;
        while (running)
        {
            if (selectedIcon <= 0)
            {
                if (Input.GetAxisRaw("Vertical") > 0)
                {
                    selectedIcon = 2;
                    StartCoroutine("updateIcon", selectedIcon);
                    SfxHandler.Play(selectClip);
                }
                else if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    selectedIcon = 1;
                    StartCoroutine("updateIcon", selectedIcon);
                    SfxHandler.Play(selectClip);
                }
                else if (Input.GetAxisRaw("Vertical") < 0)
                {
                    selectedIcon = 5;
                    StartCoroutine("updateIcon", selectedIcon);
                    SfxHandler.Play(selectClip);
                }
                else if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    selectedIcon = 3;
                    StartCoroutine("updateIcon", selectedIcon);
                    SfxHandler.Play(selectClip);
                }
            }
            else
            {
                if (Input.GetAxisRaw("Vertical") > 0)
                {
                    if (selectedApp.appUp != null)
                    {
                        selectedApp = selectedApp.appUp;
                        selectedIcon = selectedApp.index;
                        StartCoroutine("updateIcon", selectedIcon);
                        SfxHandler.Play(selectClip);
                        Debug.Log(selectedIcon);
                        yield return new WaitForSeconds(0.2f);
                    }
                    

                    /*
                    if (selectedIcon != 1 && selectedIcon != 2)
                    {
                        selectedIcon -= 2;
                        StartCoroutine("updateIcon", selectedIcon);
                        SfxHandler.Play(selectClip);
                        Debug.Log(selectedIcon);
                        yield return new WaitForSeconds(0.2f);
                    }
                    */
                }
                else if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    if (selectedApp.appRight != null)
                    {
                        selectedApp = selectedApp.appRight;
                        selectedIcon = selectedApp.index;
                        StartCoroutine("updateIcon", selectedIcon);
                        SfxHandler.Play(selectClip);
                        Debug.Log(selectedIcon);
                        yield return new WaitForSeconds(0.2f);
                    }
                    
                    /*
                    if (selectedIcon != 2 && selectedIcon != 4 && selectedIcon != 6)
                    {
                        selectedIcon += 1;
                        StartCoroutine("updateIcon", selectedIcon);
                        SfxHandler.Play(selectClip);
                        Debug.Log(selectedIcon);
                        yield return new WaitForSeconds(0.2f);
                    }
                    */
                }
                else if (Input.GetAxisRaw("Vertical") < 0)
                {
                    if (selectedApp.appDown != null)
                    {
                        selectedApp = selectedApp.appDown;
                        selectedIcon = selectedApp.index;
                        StartCoroutine("updateIcon", selectedIcon);
                        SfxHandler.Play(selectClip);
                        Debug.Log(selectedIcon);
                        yield return new WaitForSeconds(0.2f);
                    }
                    
                    /*
                    if (selectedIcon != 5 && selectedIcon != 6)
                    {
                        selectedIcon += 2;
                        StartCoroutine("updateIcon", selectedIcon);
                        SfxHandler.Play(selectClip);
                        Debug.Log(selectedIcon);
                        yield return new WaitForSeconds(0.2f);
                    }
                    */
                }
                else if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    if (selectedApp.appLeft != null)
                    {
                        selectedApp = selectedApp.appLeft;
                        selectedIcon = selectedApp.index;
                        StartCoroutine("updateIcon", selectedIcon);
                        SfxHandler.Play(selectClip);
                        Debug.Log(selectedIcon);
                        yield return new WaitForSeconds(0.2f);
                    }
                    
                    /*
                    if (selectedIcon != 1 && selectedIcon != 3  && selectedIcon != 5)
                    {
                        selectedIcon -= 1;
                        StartCoroutine("updateIcon", selectedIcon);
                        SfxHandler.Play(selectClip);
                        Debug.Log(selectedIcon);
                        yield return new WaitForSeconds(0.2f);
                    }
                    */
                }
                else if (Input.GetButton("Select"))
                {
                    switch (selectedApp.type)
                    {
                        case App.Type.Menu:
                            yield return StartCoroutine(startMenu(selectedApp.scene));
                            break;
                        case App.Type.NotImplemented:
                            yield return StartCoroutine(notYetImplementedMenu());
                            break;
                        case App.Type.Save:
                            yield return StartCoroutine(startSaveMenu());
                            break;
                    }

                    /*
                    if (selectedIcon == 1)
                    {
                        //Party
                        yield return StartCoroutine(startMenu(Scene.main.Party.gameObject));
                    }
                    else if (selectedIcon == 2)
                    {
                        //Pokedex
                        Debug.Log("Pokédex not yet implemented");
                        yield return new WaitForSeconds(0.2f);
                    }
                    else if (selectedIcon == 3)
                    {
                        //Bag
                        yield return StartCoroutine(startMenu(Scene.main.Bag.gameObject));
                    }
                    else if (selectedIcon == 4)
                    {
                        //TrainerCard
                        yield return StartCoroutine(startMenu(Scene.main.Trainer.gameObject));
                    }
                    else if (selectedIcon == 5)
                    {
                        //Save
                        yield return StartCoroutine(startSaveMenu());
                    }
                    else if (selectedIcon == 6)
                    {
                        //Settings
                        yield return StartCoroutine(startMenu(Scene.main.Settings.gameObject));
                    }
                    */
                }
            }
            if (Input.GetButton("Start") || Input.GetButton("Back"))
            {
                running = false;
                SfxHandler.Play(cancelClip);
            }

            yield return null;
        }

        /*
        app_text.gameObject.SetActive(false);
        app_text_shadow.gameObject.SetActive(false);
        */
        
        StopCoroutine(developArrowAnim);
        transform.Find("develop").gameObject.SetActive(false);
        left_interface.SetActive(false);
        
        StartCoroutine(UnBlur(speed));
        LeanTween.moveX(phone, 195, transitionTime);
        yield return new WaitForSeconds(transitionTime);

        this.gameObject.SetActive(false);
    }

    /// Only runs the default scene (no parameters)
    private IEnumerator runSceneUntilDeactivated(GameObject sceneInterface)
    {
        sceneInterface.SetActive(true);
        sceneInterface.SendMessage("control");
        while (sceneInterface.activeSelf)
        {
            yield return null;
        }
    }
}