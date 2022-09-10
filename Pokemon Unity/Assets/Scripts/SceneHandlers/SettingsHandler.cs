//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SettingsHandler : MonoBehaviour
{
    private DialogBoxHandlerNew Dialog;

    private Image selectRow;

    private Text
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
        battleStyle,
        battleStyleShadow,
        battleStyleHighlight,
        screenSize,
        screenSizeShadow,
        screenSizeHighlight,
        fullscreen,
        fullscreenShadow,
        fullscreenHighlight;

    private bool running;
    private int selectedOption;

    private int[] selectedOptionSize =
    {
        3, 21, 21, 2, 2, 2
    };

    private int[] selectedOptionIndex =
    {
        2, 7, 14, 1, 0, 0
    };

    private string[] selectedOptionText = new string[]
    {
        "How quickly to draw text to the screen.",
        "Adjust the volume of the music.",
        "Adjust the volume of the sound effects.",
        "Display animations during battles.",
        "Switch before opponent's next Pokemon?",
        "Set the fullscreen mode."
    };

    private AudioSource SettingsAudio;
    public AudioClip selectClip;

    private GameObject DialogBox;
    private Text DialogBoxText;
    private Text DialogBoxTextShadow;
    private Image DialogBoxBorder;

    void Awake()
    {
        SettingsAudio = transform.GetComponent<AudioSource>();
        Dialog = gameObject.GetComponent<DialogBoxHandlerNew>();

        selectRow = transform.Find("selectRow").GetComponent<Image>();
    
        textSpeedShadow = transform.Find("TextSpeedShadow").GetComponent<Text>();
        textSpeed = textSpeedShadow.transform.Find("TextSpeed").GetComponent<Text>();
        textSpeedHighlight = textSpeedShadow.transform.Find("TextSpeedHighlight").GetComponent<Text>();
        
        musicVolumeShadow = transform.Find("MusicVolumeShadow").GetComponent<Text>();
        musicVolume = musicVolumeShadow.transform.Find("MusicVolume").GetComponent<Text>();
        musicVolumeHighlight = musicVolumeShadow.transform.Find("MusicVolumeHighlight").GetComponent<Text>();
        
        sfxVolumeShadow = transform.Find("SFXVolumeShadow").GetComponent<Text>();
        sfxVolume = sfxVolumeShadow.transform.Find("SFXVolume").GetComponent<Text>();
        sfxVolumeHighlight = sfxVolumeShadow.transform.Find("SFXVolumeHighlight").GetComponent<Text>();

        battleSceneShadow = transform.Find("BattleSceneShadow").GetComponent<Text>();
        battleScene = battleSceneShadow.transform.Find("BattleScene").GetComponent<Text>();
        battleSceneHighlight = battleSceneShadow.transform.Find("BattleSceneHighlight").GetComponent<Text>();
        
        battleStyleShadow = transform.Find("BattleStyleShadow").GetComponent<Text>();
        battleStyle = battleStyleShadow.transform.Find("BattleStyle").GetComponent<Text>();
        battleStyleHighlight = battleStyleShadow.transform.Find("BattleStyleHighlight").GetComponent<Text>();
        
        screenSizeShadow = transform.Find("ScreenSizeShadow").GetComponent<Text>();
        screenSize = screenSizeShadow.transform.Find("ScreenSize").GetComponent<Text>();
        screenSizeHighlight = screenSizeShadow.transform.Find("ScreenSizeHighlight").GetComponent<Text>();
        
        fullscreenShadow = transform.Find("FullscreenShadow").GetComponent<Text>();
        fullscreen = fullscreenShadow.transform.Find("Fullscreen").GetComponent<Text>();
        fullscreenHighlight = fullscreenShadow.transform.Find("FullscreenHighlight").GetComponent<Text>();

        DialogBox = transform.Find("Description").gameObject;
        DialogBoxText = DialogBox.transform.Find("DescriptionText").GetComponent<Text>();
        DialogBoxTextShadow = DialogBox.transform.Find("DescriptionTextShadow").GetComponent<Text>();
        DialogBoxBorder = DialogBox.transform.Find("DescriptionBorder").GetComponent<Image>();
    }

    void Start()
    {
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
        float startY = selectRow.rectTransform.position.y;
        float delta = 32;
        LeanTween.moveY(selectRow.gameObject, startY + direction*delta, moveSpeed);
        yield return new WaitForSeconds(moveSpeed);
    }

    private void updateOption()
    {
        if (selectedOption == 0)
        {
            if (selectedOptionIndex[selectedOption] == 0)
            {
                textSpeedHighlight.text = "Slow";
                LeanTween.moveX(textSpeedHighlight.gameObject, 450.8f, 0);
                PlayerPrefs.SetInt("textSpeed", 0);
            }
            else if (selectedOptionIndex[selectedOption] == 1)
            {
                textSpeedHighlight.text = "Medium";
                LeanTween.moveX(textSpeedHighlight.gameObject, 523.0f, 0);
                PlayerPrefs.SetInt("textSpeed", 1);
            }
            else
            {
                textSpeedHighlight.text = "Fast";
                LeanTween.moveX(textSpeedHighlight.gameObject, 617.4f, 0);
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
            if (selectedOptionIndex[selectedOption] == 0)
            {
                battleSceneHighlight.text = "Off";
                LeanTween.moveX(battleSceneHighlight.gameObject, 515.9f, 0);
            }
            else
            {
                battleSceneHighlight.text = "On";
                LeanTween.moveX(battleSceneHighlight.gameObject, 578.4f, 0);
            }
        }
        else if (selectedOption == 4)
        {
            if (selectedOptionIndex[selectedOption] == 0)
            {
                battleStyleHighlight.text = "Switch";
                LeanTween.moveX(battleStyleHighlight.gameObject, 485.6f, 0);
            }
            else
            {
                battleStyleHighlight.text = "Set";
                LeanTween.moveX(battleStyleHighlight.gameObject, 578, 0);
            }
        }
        else if (selectedOption == 5)
        {
            if (selectedOptionIndex[selectedOption] == 0)
            {
                fullscreenHighlight.text = "Off";
                LeanTween.moveX(fullscreenHighlight.gameObject, 516.6f, 0);
                Screen.SetResolution(684, 384, false);
            }
            else if (selectedOptionIndex[selectedOption] == 1)
            {
                fullscreenHighlight.text = "On";
                LeanTween.moveX(fullscreenHighlight.gameObject, 578.8f, 0);
                Screen.SetResolution(684, 384, true);
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
            PlayerPrefs.SetInt("battleScene", selectedOptionIndex[3]);
            PlayerPrefs.SetInt("battleStyle", selectedOptionIndex[4]);
            PlayerPrefs.SetInt("fullscreen", selectedOptionIndex[5]);
        }
        else
        {
            selectedOptionIndex[0] = PlayerPrefs.GetInt("textSpeed");
            selectedOptionIndex[1] = Mathf.RoundToInt(Mathf.Sqrt(PlayerPrefs.GetFloat("musicVolume")) * 20f);
            selectedOptionIndex[2] = Mathf.RoundToInt(Mathf.Sqrt(PlayerPrefs.GetFloat("sfxVolume")) * 20f);
            selectedOptionIndex[3] = PlayerPrefs.GetInt("battleScene");
            selectedOptionIndex[4] = PlayerPrefs.GetInt("battleStyle");
            selectedOptionIndex[5] = PlayerPrefs.GetInt("fullscreen");
        }
        for (int i = 0; i < 6; i++)
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
        PlayerPrefs.SetInt("battleScene", selectedOptionIndex[3]);
        PlayerPrefs.SetInt("battleStyle", selectedOptionIndex[4]);
        PlayerPrefs.SetInt("fullscreen", selectedOptionIndex[5]);
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
            selectedOptionIndex[5]
        };
        //	float originalMVol = PlayerPrefs.GetFloat("musicVolume");
//		float originalSVol = PlayerPrefs.GetFloat("sfxVolume");
        selectedOption = 0;
        LeanTween.moveY(selectRow.gameObject, 306.5f, 0);
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
                if (selectedOption < 5)
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
                Dialog.DrawDialogBox();
                yield return
                    Dialog.StartCoroutine(Dialog.DrawText( "Would you like to save the currently \\selected settings?"));
                yield return new WaitForSeconds(0.2f);
                yield return StartCoroutine( Dialog.DrawChoiceBoxNo());
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
                Dialog.UndrawDialogBox();
                Dialog.UndrawChoiceBox();
                //yield return new WaitForSeconds(sceneTransition.FadeOut());
                yield return StartCoroutine(ScreenFade.main.Fade(false, ScreenFade.defaultSpeed));
                running = false;
            }
            yield return null;
        }
        this.gameObject.SetActive(false);
    }
}