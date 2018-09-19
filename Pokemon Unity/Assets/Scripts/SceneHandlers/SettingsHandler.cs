//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class SettingsHandler : MonoBehaviour
{
    private DialogBoxHandler Dialog;

    private GUITexture selectRow;

    private GUIText
        textSpeed,
        textSpeedShadow,
        textSpeedHighlight,
        musicVolume,
        musicVolumeShadow,
        musicVolumeHighlight,
        sfxVolume,
        sfxVolumeShadow,
        sfxVolumeHighlight,
        frameStyle,
        frameStyleShadow,
        battleScene,
        battleSceneShadow,
        battleSceneHighlight,
        customSprites,
        customSpritesShadow,
        customSpritesHighlight,
        screenSize,
        screenSizeShadow,
        screenSizeHighlight,
        fullscreen,
        fullscreenShadow,
        fullscreenHighlight;

    private bool running;
    private int selectedOption;

    private int[] selectedOptionSize = new int[]
    {
        3, 21, 21, 2, 2, 2, 5, 3
    };

    private int[] selectedOptionIndex = new int[]
    {
        2, 7, 14, 0, 1, 0, 0, 0
    };

    private string[] selectedOptionText = new string[]
    {
        "How quickly to draw text to the screen.",
        "Adjust the volume of the music.",
        "Adjust the volume of the sound effects.",
        "Change the appearance of the text boxes.",
        "Display animations during battles.",
        "Use custom sprites in StreamingAssets.",
        "Adjust the resolution of the screen.",
        "Set the fullscreen mode."
    };

    private AudioSource SettingsAudio;
    public AudioClip selectClip;

    private GameObject DialogBox;
    private GUIText DialogBoxText;
    private GUIText DialogBoxTextShadow;
    private GUITexture DialogBoxBorder;

    void Awake()
    {
        SettingsAudio = transform.GetComponent<AudioSource>();
        Dialog = gameObject.GetComponent<DialogBoxHandler>();

        selectRow = transform.Find("selectRow").GetComponent<GUITexture>();

        textSpeed = transform.Find("TextSpeed").GetComponent<GUIText>();
        textSpeedShadow = textSpeed.transform.Find("TextSpeedShadow").GetComponent<GUIText>();
        textSpeedHighlight = textSpeed.transform.Find("TextSpeedHighlight").GetComponent<GUIText>();
        musicVolume = transform.Find("MusicVolume").GetComponent<GUIText>();
        musicVolumeShadow = musicVolume.transform.Find("MusicVolumeShadow").GetComponent<GUIText>();
        musicVolumeHighlight = musicVolume.transform.Find("MusicVolumeHighlight").GetComponent<GUIText>();
        sfxVolume = transform.Find("SFXVolume").GetComponent<GUIText>();
        sfxVolumeShadow = sfxVolume.transform.Find("SFXVolumeShadow").GetComponent<GUIText>();
        sfxVolumeHighlight = sfxVolume.transform.Find("SFXVolumeHighlight").GetComponent<GUIText>();
        frameStyle = transform.Find("FrameStyle").GetComponent<GUIText>();
        frameStyleShadow = frameStyle.transform.Find("FrameStyleShadow").GetComponent<GUIText>();
        battleScene = transform.Find("BattleScene").GetComponent<GUIText>();
        battleSceneShadow = battleScene.transform.Find("BattleSceneShadow").GetComponent<GUIText>();
        battleSceneHighlight = battleScene.transform.Find("BattleSceneHighlight").GetComponent<GUIText>();
        customSprites = transform.Find("CustomSprites").GetComponent<GUIText>();
        customSpritesShadow = customSprites.transform.Find("CustomSpritesShadow").GetComponent<GUIText>();
        customSpritesHighlight = customSprites.transform.Find("CustomSpritesHighlight").GetComponent<GUIText>();
        screenSize = transform.Find("ScreenSize").GetComponent<GUIText>();
        screenSizeShadow = screenSize.transform.Find("ScreenSizeShadow").GetComponent<GUIText>();
        screenSizeHighlight = screenSize.transform.Find("ScreenSizeHighlight").GetComponent<GUIText>();
        fullscreen = transform.Find("Fullscreen").GetComponent<GUIText>();
        fullscreenShadow = fullscreen.transform.Find("FullscreenShadow").GetComponent<GUIText>();
        fullscreenHighlight = fullscreen.transform.Find("FullscreenHighlight").GetComponent<GUIText>();

        DialogBox = transform.Find("Description").gameObject;
        DialogBoxText = DialogBox.transform.Find("DescriptionText").GetComponent<GUIText>();
        DialogBoxTextShadow = DialogBox.transform.Find("DescriptionTextShadow").GetComponent<GUIText>();
        DialogBoxBorder = DialogBox.transform.Find("DescriptionBorder").GetComponent<GUITexture>();
    }

    void Start()
    {
        updateResolutions();
        gameObject.SetActive(false);
    }

    public void drawDialogBox()
    {
        DialogBox.SetActive(true);
        DialogBoxText.text = "";
        DialogBoxTextShadow.text = "";
    }

    public IEnumerator drawText(string textLine)
    {
        int textSpeed = PlayerPrefs.GetInt("textSpeed") + 1;
        float charPerSec = 16 + (textSpeed * textSpeed * 9);
        float secPerChar = 1 / charPerSec;
        //split textLine into an array of each character, so it may be printed 1 bit at a time
        char[] chars = textLine.ToCharArray();
        for (int i = 0; i < textLine.Length; i++)
        {
            if (chars[i].Equals('\\'))
            {
                //   \ is used to designate line breaks
                DialogBoxText.text += "\n";
                DialogBoxTextShadow.text = DialogBoxText.text;
            }
            else
            {
                DialogBoxText.text += chars[i].ToString();
                DialogBoxTextShadow.text = DialogBoxText.text;
            }
            if (Time.deltaTime < secPerChar)
            {
                yield return new WaitForSeconds(secPerChar);
            } //wait for (seconds per character print) before printing the next
            else
            {
                i += 1;
                if (i < textLine.Length)
                {
                    //if not at the end, repeat and wait double time
                    if (chars[i].Equals('\\'))
                    {
                        //   \ is used to designate line breaks
                        DialogBoxText.text += "\n";
                        DialogBoxTextShadow.text = DialogBoxText.text;
                    }
                    else
                    {
                        DialogBoxText.text += chars[i].ToString();
                        DialogBoxTextShadow.text = DialogBoxText.text;
                    }
                    yield return new WaitForSeconds(secPerChar * 2);
                }
            }
        }
    }

    public void drawTextInstant(string textLine)
    {
        //split textLine into an array of each character, so it may be printed 1 bit at a time
        char[] chars = textLine.ToCharArray();
        for (int i = 0; i < textLine.Length; i++)
        {
            if (chars[i].Equals('\\'))
            {
                //   \ is used to designate line breaks
                DialogBoxText.text += "\n";
                DialogBoxTextShadow.text = DialogBoxText.text;
            }
            else
            {
                DialogBoxText.text += chars[i].ToString();
                DialogBoxTextShadow.text = DialogBoxText.text;
            }
        }
    }

    public IEnumerator moveSelection(int direction)
    {
        float increment = 0;
        float moveSpeed = 0.2f;
        float startY = selectRow.pixelInset.y;
        while (increment < 1)
        {
            increment += (1 / moveSpeed) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }
            selectRow.pixelInset = new Rect(selectRow.pixelInset.x, startY + (16 * increment * direction),
                selectRow.pixelInset.width, selectRow.pixelInset.height);
            yield return null;
        }
    }

    private void updateResolutions()
    {
        int resWidth = Screen.currentResolution.width + 10; //add a buffer
        int resHeight = Screen.currentResolution.height + 6;
        if (1710 < resWidth && 960 < resHeight)
        {
            screenSize.text = "x1     x2     x3     x4     x5";
            selectedOptionSize[6] = 5;
        }
        else if (1368 < resWidth && 768 < resHeight)
        {
            screenSize.text = "x1     x2     x3     x4";
            selectedOptionSize[6] = 4;
        }
        else if (1026 < resWidth && 576 < resHeight)
        {
            screenSize.text = "x1     x2     x3";
            selectedOptionSize[6] = 3;
        }
        else if (684 < resWidth && 384 < resHeight)
        {
            screenSize.text = "x1     x2";
            selectedOptionSize[6] = 2;
        }
        else
        {
            screenSize.text = "x1";
            selectedOptionSize[6] = 1;
        }
    }

    private void updateOption()
    {
        if (selectedOption == 0)
        {
            if (selectedOptionIndex[selectedOption] == 0)
            {
                textSpeedHighlight.text = "Slow";
                textSpeedHighlight.pixelOffset = new Vector2(155, 159);
                PlayerPrefs.SetInt("textSpeed", 0);
            }
            else if (selectedOptionIndex[selectedOption] == 1)
            {
                textSpeedHighlight.text = "Medium";
                textSpeedHighlight.pixelOffset = new Vector2(191, 159);
                PlayerPrefs.SetInt("textSpeed", 1);
            }
            else
            {
                textSpeedHighlight.text = "Fast";
                textSpeedHighlight.pixelOffset = new Vector2(238, 159);
                PlayerPrefs.SetInt("textSpeed", 2);
            }
        }
        else if (selectedOption == 1)
        {
            int repetitions = selectedOptionIndex[selectedOption];
            musicVolumeHighlight.text = "";
            for (int i = 0; i < repetitions; i++)
            {
                musicVolumeHighlight.text += "| ";
            }
            float mVol = ((float) selectedOptionIndex[1] / 20f) * ((float) selectedOptionIndex[1] / 20f);
            PlayerPrefs.SetFloat("musicVolume", mVol);
        }
        else if (selectedOption == 2)
        {
            int repetitions = selectedOptionIndex[selectedOption];
            sfxVolumeHighlight.text = "";
            for (int i = 0; i < repetitions; i++)
            {
                sfxVolumeHighlight.text += "| ";
            }
            float sVol = ((float) selectedOptionIndex[2] / 20f) * ((float) selectedOptionIndex[2] / 20f);
            PlayerPrefs.SetFloat("sfxVolume", sVol);
        }
        else if (selectedOption == 3)
        {
            frameStyle.text = "Style " + (selectedOptionIndex[selectedOption] + 1);
            frameStyleShadow.text = frameStyle.text;
            DialogBoxBorder.texture = Resources.Load<Texture>("Frame/dialog" + (selectedOptionIndex[3] + 1));
            PlayerPrefs.SetInt("frameStyle", selectedOptionIndex[3] + 1);
        }
        else if (selectedOption == 4)
        {
            if (selectedOptionIndex[selectedOption] == 0)
            {
                battleSceneHighlight.text = "Off";
                battleSceneHighlight.pixelOffset = new Vector2(186, 95);
            }
            else
            {
                battleSceneHighlight.text = "On";
                battleSceneHighlight.pixelOffset = new Vector2(217, 95);
            }
        }
        else if (selectedOption == 5)
        {
            if (selectedOptionIndex[selectedOption] == 0)
            {
                customSpritesHighlight.text = "Off";
                customSpritesHighlight.pixelOffset = new Vector2(186, 79);
                PlayerPrefs.SetInt("customSprites", selectedOptionIndex[5]);
                SaveDataOld.currentSave.playerOutfit = "hgss";
                PlayerMovement.player.updateAnimation("walk", 7);
                Debug.Log("Disabled custom sprites");
            }
            else
            {
                customSpritesHighlight.text = "On";
                SaveDataOld.currentSave.playerOutfit = "custom";
                PlayerMovement.player.updateAnimation("walk", 7);
                customSpritesHighlight.pixelOffset = new Vector2(217, 79);
                Debug.Log("Enabled custom sprites");
            }
        }
        else if (selectedOption == 6)
        {
            if (selectedOptionIndex[selectedOption] == 0)
            {
                screenSizeHighlight.text = "x1";
                screenSizeHighlight.pixelOffset = new Vector2(150, 63);
                if (!Screen.fullScreen)
                {
                    Screen.SetResolution(342, 192, Screen.fullScreen);
                }
            }
            else if (selectedOptionIndex[selectedOption] == 1)
            {
                screenSizeHighlight.text = "x2";
                screenSizeHighlight.pixelOffset = new Vector2(177, 63);
                if (!Screen.fullScreen)
                {
                    Screen.SetResolution(684, 384, Screen.fullScreen);
                }
            }
            else if (selectedOptionIndex[selectedOption] == 2)
            {
                screenSizeHighlight.text = "x3";
                screenSizeHighlight.pixelOffset = new Vector2(204, 63);
                if (!Screen.fullScreen)
                {
                    Screen.SetResolution(1026, 576, Screen.fullScreen);
                }
            }
            else if (selectedOptionIndex[selectedOption] == 3)
            {
                screenSizeHighlight.text = "x4";
                screenSizeHighlight.pixelOffset = new Vector2(231, 63);
                if (!Screen.fullScreen)
                {
                    Screen.SetResolution(1368, 768, Screen.fullScreen);
                }
            }
            else
            {
                screenSizeHighlight.text = "x5";
                screenSizeHighlight.pixelOffset = new Vector2(258, 63);
                if (!Screen.fullScreen)
                {
                    Screen.SetResolution(1710, 960, Screen.fullScreen);
                }
            }
        }
        else if (selectedOption == 7)
        {
            if (selectedOptionIndex[selectedOption] == 0)
            {
                fullscreenHighlight.text = "Off";
                fullscreenHighlight.pixelOffset = new Vector2(149, 47);
                Screen.SetResolution(342 * (selectedOptionIndex[6] + 1), 192 * (selectedOptionIndex[6] + 1), false);
            }
            else if (selectedOptionIndex[selectedOption] == 1)
            {
                fullscreenHighlight.text = "Border";
                fullscreenHighlight.pixelOffset = new Vector2(180, 47);
                Screen.SetResolution(342 * (selectedOptionIndex[6] + 1), 192 * (selectedOptionIndex[6] + 1), true);
            }
            else if (selectedOptionIndex[selectedOption] == 2)
            {
                fullscreenHighlight.text = "Stretch";
                fullscreenHighlight.pixelOffset = new Vector2(231, 47);
                Screen.SetResolution(342 * selectedOptionSize[6], 192 * selectedOptionSize[6], true);
            }
        }
    }

    private void loadSettings()
    {
        //PlayerPrefs.DeleteAll();
        if (!PlayerPrefs.HasKey("textSpeed"))
        {
            PlayerPrefs.SetInt("textSpeed", selectedOptionIndex[0]);
            float mVol = ((float) selectedOptionIndex[1] / 20f) * ((float) selectedOptionIndex[1] / 20f);
            float sVol = ((float) selectedOptionIndex[2] / 20f) * ((float) selectedOptionIndex[2] / 20f);
            PlayerPrefs.SetFloat("musicVolume", mVol);
            PlayerPrefs.SetFloat("sfxVolume", sVol);
            PlayerPrefs.SetInt("frameStyle", selectedOptionIndex[3] + 1);
            PlayerPrefs.SetInt("battleScene", selectedOptionIndex[4]);
            PlayerPrefs.SetInt("customSprites", selectedOptionIndex[5]);
            PlayerPrefs.SetInt("screenSize", selectedOptionIndex[6] + 1);
            PlayerPrefs.SetInt("fullscreen", selectedOptionIndex[7]);
        }
        else
        {
            selectedOptionIndex[0] = PlayerPrefs.GetInt("textSpeed");
            selectedOptionIndex[1] = Mathf.RoundToInt(Mathf.Sqrt(PlayerPrefs.GetFloat("musicVolume")) * 20f);
            selectedOptionIndex[2] = Mathf.RoundToInt(Mathf.Sqrt(PlayerPrefs.GetFloat("sfxVolume")) * 20f);
            selectedOptionIndex[3] = PlayerPrefs.GetInt("frameStyle") - 1;
            selectedOptionIndex[4] = PlayerPrefs.GetInt("battleScene");
            selectedOptionIndex[5] = PlayerPrefs.GetInt("customSprites");
            selectedOptionIndex[6] = PlayerPrefs.GetInt("screenSize") - 1;
            selectedOptionIndex[7] = PlayerPrefs.GetInt("fullscreen");
        }
        for (int i = 0; i < 8; i++)
        {
            selectedOption = i;
            updateOption();
        }
        selectedOption = 0;
    }

    private void saveSettings()
    {
        PlayerPrefs.SetInt("textSpeed", selectedOptionIndex[0]);
        float mVol = ((float) selectedOptionIndex[1] / 20f) * ((float) selectedOptionIndex[1] / 20f);
        float sVol = ((float) selectedOptionIndex[2] / 20f) * ((float) selectedOptionIndex[2] / 20f);
        PlayerPrefs.SetFloat("musicVolume", mVol);
        PlayerPrefs.SetFloat("sfxVolume", sVol);
        PlayerPrefs.SetInt("frameStyle", selectedOptionIndex[3] + 1);
        PlayerPrefs.SetInt("battleScene", selectedOptionIndex[4]);
        PlayerPrefs.SetInt("customSprites", selectedOptionIndex[5]);
        PlayerPrefs.SetInt("screenSize", selectedOptionIndex[6] + 1);
        PlayerPrefs.SetInt("fullscreen", selectedOptionIndex[7]);
        PlayerPrefs.Save();
    }


    public IEnumerator control()
    {
        //sceneTransition.FadeIn();
        StartCoroutine(ScreenFade.main.Fade(true, ScreenFade.defaultSpeed));

        running = true;
        loadSettings();
        int[] originalIndexes = new int[]
        {
            selectedOptionIndex[0],
            selectedOptionIndex[1],
            selectedOptionIndex[2],
            selectedOptionIndex[3],
            selectedOptionIndex[4],
            selectedOptionIndex[5],
            selectedOptionIndex[6],
            selectedOptionIndex[7]
        };
        //	float originalMVol = PlayerPrefs.GetFloat("musicVolume");
//		float originalSVol = PlayerPrefs.GetFloat("sfxVolume");
        selectedOption = 0;
        selectRow.pixelInset = new Rect(51, 144, selectRow.pixelInset.width, selectRow.pixelInset.height);
        drawDialogBox();
        drawTextInstant(selectedOptionText[0]);
        while (running)
        {
            if (Input.GetAxisRaw("Vertical") > 0)
            {
                if (selectedOption > 0)
                {
                    selectedOption -= 1;
                    drawDialogBox();
                    if (selectedOption == 0)
                    {
                        StartCoroutine("drawText", selectedOptionText[selectedOption]);
                    }
                    else
                    {
                        drawTextInstant(selectedOptionText[selectedOption]);
                    }
                    SfxHandler.Play(selectClip);
                    yield return StartCoroutine("moveSelection", 1);
                }
            }
            else if (Input.GetAxisRaw("Vertical") < 0)
            {
                if (selectedOption < 7)
                {
                    selectedOption += 1;
                    StopCoroutine("drawText");
                    drawDialogBox();
                    drawTextInstant(selectedOptionText[selectedOption]);
                    SfxHandler.Play(selectClip);
                    yield return StartCoroutine("moveSelection", -1);
                }
            }
            else if (Input.GetAxisRaw("Horizontal") > 0)
            {
                if (selectedOption == 1)
                {
                    if (selectedOptionIndex[selectedOption] < selectedOptionSize[selectedOption] - 1)
                    {
                        selectedOptionIndex[selectedOption] += 1;
                        updateOption();
                        yield return new WaitForSeconds(0.05f);
                    }
                }
                else if (selectedOption == 2)
                {
                    if (selectedOptionIndex[selectedOption] < selectedOptionSize[selectedOption] - 1)
                    {
                        selectedOptionIndex[selectedOption] += 1;
                        updateOption();
                        SfxHandler.Play(selectClip);
                        yield return new WaitForSeconds(0.05f);
                    }
                }
                else if (selectedOption == 3)
                {
                    selectedOptionIndex[selectedOption] += 1;
                    SfxHandler.Play(selectClip);
                    if (selectedOptionIndex[selectedOption] > selectedOptionSize[selectedOption] - 1)
                    {
                        selectedOptionIndex[selectedOption] = 0;
                    }
                    updateOption();
                    yield return new WaitForSeconds(0.2f);
                }
                else
                {
                    if (selectedOptionIndex[selectedOption] < selectedOptionSize[selectedOption] - 1)
                    {
                        selectedOptionIndex[selectedOption] += 1;
                        SfxHandler.Play(selectClip);
                        updateOption();
                        if (selectedOption == 0)
                        {
                            StopCoroutine("drawText");
                            drawDialogBox();
                            StartCoroutine("drawText", selectedOptionText[selectedOption]);
                        }
                        yield return new WaitForSeconds(0.2f);
                    }
                }
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                if (selectedOption == 1)
                {
                    if (selectedOptionIndex[selectedOption] > 0)
                    {
                        selectedOptionIndex[selectedOption] -= 1;
                        updateOption();
                        yield return new WaitForSeconds(0.05f);
                    }
                }
                else if (selectedOption == 2)
                {
                    if (selectedOptionIndex[selectedOption] > 0)
                    {
                        selectedOptionIndex[selectedOption] -= 1;
                        updateOption();
                        SfxHandler.Play(selectClip);
                        yield return new WaitForSeconds(0.05f);
                    }
                }
                else if (selectedOption == 3)
                {
                    selectedOptionIndex[selectedOption] -= 1;
                    SfxHandler.Play(selectClip);
                    if (selectedOptionIndex[selectedOption] < 0)
                    {
                        selectedOptionIndex[selectedOption] = selectedOptionSize[selectedOption] - 1;
                    }
                    updateOption();
                    yield return new WaitForSeconds(0.2f);
                }
                else
                {
                    if (selectedOptionIndex[selectedOption] > 0)
                    {
                        selectedOptionIndex[selectedOption] -= 1;
                        SfxHandler.Play(selectClip);
                        updateOption();
                        if (selectedOption == 0)
                        {
                            StopCoroutine("drawText");
                            drawDialogBox();
                            StartCoroutine("drawText", selectedOptionText[selectedOption]);
                        }
                        yield return new WaitForSeconds(0.2f);
                    }
                }
            }
            else if (Input.GetButton("Back"))
            {
                Dialog.drawDialogBox();
                yield return
                    Dialog.StartCoroutine("drawText", "Would you like to save the currently \\selected settings?");
                Dialog.drawChoiceBoxNo();
                yield return new WaitForSeconds(0.2f);
                yield return StartCoroutine(Dialog.choiceNavigateNo());
                int chosenIndex = Dialog.chosenIndex;
                if (chosenIndex == 1)
                {
                    saveSettings();
                }
                else
                {
                    //set music and sfx volume back
                    //	PlayerPrefs.SetFloat("musicVolume",originalMVol);
                    //	PlayerPrefs.SetFloat("sfxVolume",originalSVol);
                    //	PlayerPrefs.Save();
                    selectedOptionIndex = originalIndexes;
                    saveSettings();
                    GlobalVariables.global.updateResolution();
                }
                Dialog.undrawDialogBox();
                Dialog.undrawChoiceBox();
                //yield return new WaitForSeconds(sceneTransition.FadeOut());
                yield return StartCoroutine(ScreenFade.main.Fade(false, ScreenFade.defaultSpeed));
                running = false;
            }
            yield return null;
        }
        this.gameObject.SetActive(false);
    }
}