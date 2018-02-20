//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class PauseHandler : MonoBehaviour
{
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

    private RotatableGUIItem iconPokedex;
    private RotatableGUIItem iconParty;
    private RotatableGUIItem iconBag;
    private RotatableGUIItem iconTrainer;
    private RotatableGUIItem iconSave;
    private RotatableGUIItem iconSettings;

    private GUITexture saveDataDisplay;
    private GUIText mapName;
    private GUIText mapNameShadow;
    private GUIText dataText;
    private GUIText dataTextShadow;

    private DialogBoxHandler Dialog;

    private AudioSource PauseAudio;
    public AudioClip selectClip;
    public AudioClip openClip;

    private int selectedIcon;
    private RotatableGUIItem targetIcon;

    private bool running;

    void Awake()
    {
        Dialog = GameObject.Find("GUI").GetComponent<DialogBoxHandler>();

        PauseAudio = transform.GetComponent<AudioSource>();

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

        saveDataDisplay = transform.Find("SaveDataDisplay").GetComponent<GUITexture>();
        mapName = saveDataDisplay.transform.Find("MapName").GetComponent<GUIText>();
        mapNameShadow = mapName.transform.Find("MapNameShadow").GetComponent<GUIText>();
        dataText = saveDataDisplay.transform.Find("DataText").GetComponent<GUIText>();
        dataTextShadow = dataText.transform.Find("DataTextShadow").GetComponent<GUIText>();
    }

    void Start()
    {
        pauseTop.pixelInset = new Rect(0, 192, pauseTop.pixelInset.width, pauseTop.pixelInset.height);
        pauseBottom.pixelInset = new Rect(0, -96, pauseBottom.pixelInset.width, pauseBottom.pixelInset.height);

        setSelectedText("");

        iconPokedex.transform.position = new Vector3(iconPokedex.transform.position.x, -0.0833f, 1);
        iconParty.transform.position = new Vector3(iconParty.transform.position.x, -0.0833f, 1);
        iconBag.transform.position = new Vector3(iconBag.transform.position.x, -0.0833f, 1);
        iconTrainer.transform.position = new Vector3(iconTrainer.transform.position.x, 1.0833f, 1);
        iconSave.transform.position = new Vector3(iconSave.transform.position.x, 1.0833f, 1);
        iconSettings.transform.position = new Vector3(iconSettings.transform.position.x, 1.0833f, 1);

        selectedIcon = 0;

        saveDataDisplay.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }

    private void setSelectedText(string text)
    {
        selectedText.text = text;
        selectedTextShadow.text = text;
    }

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
            pauseTop.pixelInset = new Rect(pauseTop.pixelInset.x, 192 - (increment * 96), pauseTop.pixelInset.width,
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
            pauseTop.pixelInset = new Rect(pauseTop.pixelInset.x, 96 + (increment * 96), pauseTop.pixelInset.width,
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

    public IEnumerator updateIcon(int index)
    {
        if (selectedIcon == 1)
        {
            targetIcon = iconPokedex;
            setSelectedText("Pokédex");
        }
        else if (selectedIcon == 2)
        {
            targetIcon = iconParty;
            setSelectedText("Pokémon Party");
        }
        else if (selectedIcon == 3)
        {
            targetIcon = iconBag;
            setSelectedText("Bag");
        }
        else if (selectedIcon == 4)
        {
            targetIcon = iconTrainer;
            setSelectedText(SaveData.currentSave.playerName);
        }
        else if (selectedIcon == 5)
        {
            targetIcon = iconSave;
            setSelectedText("Save Game");
        }
        else if (selectedIcon == 6)
        {
            targetIcon = iconSettings;
            setSelectedText("Settings");
        }
        else
        {
            targetIcon = null;
            setSelectedText("");
        }

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
        yield return null;
    }

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


    public IEnumerator control()
    {
        selectedIcon = 0;
        unhideIcons();
        StartCoroutine("updateIcon", selectedIcon);
        SfxHandler.Play(openClip);
        yield return StartCoroutine("openAnim");
        running = true;
        while (running)
        {
            if (selectedIcon == 0)
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
                    if (selectedIcon > 3)
                    {
                        selectedIcon -= 3;
                        StartCoroutine("updateIcon", selectedIcon);
                        SfxHandler.Play(selectClip);
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                else if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    if (selectedIcon != 3 && selectedIcon != 6)
                    {
                        selectedIcon += 1;
                        StartCoroutine("updateIcon", selectedIcon);
                        SfxHandler.Play(selectClip);
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                else if (Input.GetAxisRaw("Vertical") < 0)
                {
                    if (selectedIcon < 4)
                    {
                        selectedIcon += 3;
                        StartCoroutine("updateIcon", selectedIcon);
                        SfxHandler.Play(selectClip);
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                else if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    if (selectedIcon != 1 && selectedIcon != 4)
                    {
                        selectedIcon -= 1;
                        StartCoroutine("updateIcon", selectedIcon);
                        SfxHandler.Play(selectClip);
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                else if (Input.GetButton("Select"))
                {
                    if (selectedIcon == 1)
                    {
                        //Pokedex
                        Debug.Log("Pokédex not yet implemented");
                        yield return new WaitForSeconds(0.2f);
                    }
                    else if (selectedIcon == 2)
                    {
                        //Party
                        SfxHandler.Play(selectClip);
                        //StartCoroutine(fadeIcons(0.4f));
                        //yield return new WaitForSeconds(sceneTransition.FadeOut(0.4f));
                        yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));
                        hideIcons();

                        yield return StartCoroutine(runSceneUntilDeactivated(Scene.main.Party.gameObject));

                        unhideIcons();
                        //StartCoroutine(unfadeIcons(0.4f));
                        //yield return new WaitForSeconds(sceneTransition.FadeIn(0.4f));
                        yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));
                    }
                    else if (selectedIcon == 3)
                    {
                        //Bag
                        SfxHandler.Play(selectClip);
                        //StartCoroutine(fadeIcons(0.4f));
                        //yield return new WaitForSeconds(sceneTransition.FadeOut(0.4f));
                        yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));
                        hideIcons();

                        yield return StartCoroutine(runSceneUntilDeactivated(Scene.main.Bag.gameObject));

                        unhideIcons();
                        //StartCoroutine(unfadeIcons(0.4f));
                        //yield return new WaitForSeconds(sceneTransition.FadeIn(0.4f));
                        yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));
                    }
                    else if (selectedIcon == 4)
                    {
                        //TrainerCard
                        SfxHandler.Play(selectClip);
                        //StartCoroutine(fadeIcons(0.4f));
                        //yield return new WaitForSeconds(sceneTransition.FadeOut(0.4f));
                        yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));
                        hideIcons();

                        yield return StartCoroutine(runSceneUntilDeactivated(Scene.main.Trainer.gameObject));

                        unhideIcons();
                        //StartCoroutine(unfadeIcons(0.4f));
                        //yield return new WaitForSeconds(sceneTransition.FadeIn(0.4f));
                        yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));
                    }
                    else if (selectedIcon == 5)
                    {
                        //Save
                        saveDataDisplay.gameObject.SetActive(true);
                        saveDataDisplay.texture =
                            Resources.Load<Texture>("Frame/choice" + PlayerPrefs.GetInt("frameStyle"));
                        iconPokedex.hide = true; //hide this icon as it is in the way
                        int badgeTotal = 0;
                        for (int i = 0; i < 12; i++)
                        {
                            if (SaveData.currentSave.gymsBeaten[i])
                            {
                                badgeTotal += 1;
                            }
                        }
                        /*string playerTime = "" + SaveData.currentSave.playerMinutes;
                        if (playerTime.Length == 1)
                        {
                            playerTime = "0" + playerTime;
                        }
                        playerTime = SaveData.currentSave.playerHours + " : " + playerTime;*/
                        System.TimeSpan playTime = SaveData.currentSave.playerTime + SaveData.currentSave.startTime.Subtract(System.DateTime.UtcNow);

                        mapName.text = PlayerMovement.player.accessedMapSettings.mapName;
                        dataText.text = SaveData.currentSave.playerName + "\n" +
                                        badgeTotal + "\n" +
                                        "0" + "\n" + //pokedex not yet implemented
                                        System.String.Format("{0} : {1:00}", playTime.Hours, playTime.Minutes); //playerTime
                        mapNameShadow.text = mapName.text;
                        dataTextShadow.text = dataText.text;

                        Dialog.drawDialogBox();
                        yield return StartCoroutine(Dialog.drawText("Would you like to save the game?"));
                        Dialog.drawChoiceBoxNo();
                        yield return new WaitForSeconds(0.2f);
                        yield return StartCoroutine(Dialog.choiceNavigateNo());
                        int chosenIndex = Dialog.chosenIndex;
                        if (chosenIndex == 1)
                        {
                            //update save file
                            Dialog.undrawChoiceBox();
                            Dialog.drawDialogBox();

                            SaveData.currentSave.levelName = Application.loadedLevelName;
                            SaveData.currentSave.playerPosition = new SeriV3(PlayerMovement.player.transform.position);
                            SaveData.currentSave.playerDirection = PlayerMovement.player.direction;
                            SaveData.currentSave.mapName = PlayerMovement.player.accessedMapSettings.mapName;

                            NonResettingHandler.saveDataToGlobal();

                            SaveLoad.Save();

                            yield return
                                StartCoroutine(Dialog.drawText(SaveData.currentSave.playerName + " saved the game!"));
                            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                            {
                                yield return null;
                            }
                        }
                        Dialog.undrawDialogBox();
                        Dialog.undrawChoiceBox();
                        iconPokedex.hide = false;
                        saveDataDisplay.gameObject.SetActive(false);
                        yield return new WaitForSeconds(0.2f);
                    }
                    else if (selectedIcon == 6)
                    {
                        //Settings
                        SfxHandler.Play(selectClip);
                        //StartCoroutine(fadeIcons(0.4f));
                        //yield return new WaitForSeconds(sceneTransition.FadeOut(0.4f));
                        yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));
                        hideIcons();

                        yield return StartCoroutine(runSceneUntilDeactivated(Scene.main.Settings.gameObject));

                        unhideIcons();
                        //StartCoroutine(unfadeIcons(0.4f));
                        //yield return new WaitForSeconds(sceneTransition.FadeIn(0.4f));
                        yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));
                    }
                }
            }
            if (Input.GetButton("Start") || Input.GetButton("Back"))
            {
                running = false;
            }

            yield return null;
        }

        yield return StartCoroutine("closeAnim");

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