//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class MainMenuHandler : MonoBehaviour
{
    public int selectedButton = 0;
    public int selectedFile = 0;

    public Texture buttonSelected;
    public Texture buttonDimmed;

    private GameObject fileDataPanel;
    private GameObject continueButton;

    private GUITexture[] button = new GUITexture[3];
    private GUITexture[] buttonHighlight = new GUITexture[3];
    private GUIText[] buttonText = new GUIText[3];
    private GUIText[] buttonTextShadow = new GUIText[3];

    private GUIText fileNumbersText;
    private GUIText fileNumbersTextShadow;
    private GUIText fileSelected;

    private GUIText mapNameText;
    private GUIText mapNameTextShadow;
    private GUIText dataText;
    private GUIText dataTextShadow;
    private GUITexture[] pokemon = new GUITexture[6];

    void Awake()
    {
        SaveLoad.Load();

        fileDataPanel = transform.Find("FileData").gameObject;
        continueButton = transform.Find("Continue").gameObject;

        Transform newGameButton = transform.Find("NewGame");
        Transform settingsButton = transform.Find("Settings");

        Transform[] buttonTransforms = new Transform[]
        {
            continueButton.transform,
            newGameButton,
            settingsButton
        };
        for (int i = 0; i < 3; i++)
        {
            button[i] = buttonTransforms[i].Find("ButtonTexture").GetComponent<GUITexture>();
            buttonHighlight[i] = buttonTransforms[i].Find("ButtonHighlight").GetComponent<GUITexture>();
            buttonText[i] = buttonTransforms[i].Find("Text").GetComponent<GUIText>();
            buttonTextShadow[i] = buttonText[i].transform.Find("TextShadow").GetComponent<GUIText>();
        }

        fileNumbersText = continueButton.transform.Find("FileNumbers").GetComponent<GUIText>();
        fileNumbersTextShadow = fileNumbersText.transform.Find("FileNumbersShadow").GetComponent<GUIText>();
        fileSelected = fileNumbersText.transform.Find("FileSelected").GetComponent<GUIText>();

        mapNameText = fileDataPanel.transform.Find("MapName").GetComponent<GUIText>();
        mapNameTextShadow = mapNameText.transform.Find("MapNameShadow").GetComponent<GUIText>();
        dataText = fileDataPanel.transform.Find("DataText").GetComponent<GUIText>();
        dataTextShadow = dataText.transform.Find("DataTextShadow").GetComponent<GUIText>();

        for (int i = 0; i < 6; i++)
        {
            pokemon[i] = fileDataPanel.transform.Find("Pokemon" + i).GetComponent<GUITexture>();
        }
    }

    void Start()
    {
        
        StartCoroutine(control());
    }

    private void updateButton(int newButtonIndex)
    {
        if (newButtonIndex != selectedButton)
        {
            button[selectedButton].texture = buttonDimmed;
            buttonHighlight[selectedButton].enabled = false;
        }
        selectedButton = newButtonIndex;

        button[selectedButton].texture = buttonSelected;
        buttonHighlight[selectedButton].enabled = true;
    }

    private void updateFile(int newFileIndex)
    {
        selectedFile = newFileIndex;

        Vector2[] highlightPositions = new Vector2[]
        {
            new Vector2(132, 143),
            new Vector2(147, 143),
            new Vector2(162, 143)
        };
        fileSelected.pixelOffset = highlightPositions[selectedFile];
        fileSelected.text = "" + (selectedFile + 1);

        if (SaveLoad.savedGames[selectedFile].savefile != null)
        {
            int badgeTotal = 0;
            for (int i = 0; i < 12; i++)
            {
                if (SaveLoad.savedGames[selectedFile].Player.gymsBeaten[i])
                {
                    badgeTotal += 1;
                }
            }
            string playerTime = "" + SaveLoad.savedGames[selectedFile].Player.PlayTime.Minutes;
            if (playerTime.Length == 1)
            {
                playerTime = "0" + playerTime;
            }
            playerTime = SaveLoad.savedGames[selectedFile].Player.PlayTime.Hours + " : " + playerTime;

            mapNameText.text = SaveLoad.savedGames[selectedFile].savefile.mapName;
            mapNameTextShadow.text = mapNameText.text;
            dataText.text = SaveLoad.savedGames[selectedFile].Player.Name
                            + "\n" + badgeTotal
                            + "\n" + "0" //Pokedex not yet implemented
                            + "\n" + playerTime;
            dataTextShadow.text = dataText.text;
            
            for (int i = 0; i < SaveLoad.savedGames[selectedFile].Player.Party.Length; i++)
            {
                //if (SaveLoad.savedGames[selectedFile].PC.boxes[0][i] != null)
                if (SaveLoad.savedGames[selectedFile].Player.Party[i].IsNotNullOrNone())
                {
                    //pokemon[i].texture = SaveLoad.savedGames[selectedFile].PC.boxes[0][i].GetIcons();
                    pokemon[i].texture = SaveLoad.savedGames[selectedFile].Player.Party[i].GetIcons();
                }
                else
                {
                    
                    pokemon[i].texture = null;
                }
            }
        }
    }

    private IEnumerator animateIcons()
    {
        while (true)
        {
            for (int i = 0; i < 6; i++)
            {
                pokemon[i].border = new RectOffset(32, 0, 0, 0);
            }
            yield return new WaitForSeconds(0.15f);
            for (int i = 0; i < 6; i++)
            {
                pokemon[i].border = new RectOffset(0, 32, 0, 0);
            }
            yield return new WaitForSeconds(0.15f);
        }
    }

    public IEnumerator control()
    {
        int fileCount = SaveLoad.getSavedGamesCount();

        if (fileCount == 0)
        {
            updateButton(1);
            continueButton.SetActive(false);
            fileDataPanel.SetActive(false);
            for (int i = 1; i < 3; i++)
            {
                button[i].pixelInset = new Rect(button[i].pixelInset.x, button[i].pixelInset.y + 64f,
                    button[i].pixelInset.width, button[i].pixelInset.height);
                buttonHighlight[i].pixelInset = new Rect(buttonHighlight[i].pixelInset.x,
                    buttonHighlight[i].pixelInset.y + 64f, buttonHighlight[i].pixelInset.width,
                    buttonHighlight[i].pixelInset.height);
                buttonText[i].pixelOffset = new Vector2(buttonText[i].pixelOffset.x, buttonText[i].pixelOffset.y + 64f);
                buttonTextShadow[i].pixelOffset = new Vector2(buttonTextShadow[i].pixelOffset.x,
                    buttonTextShadow[i].pixelOffset.y + 64f);
            }
        }
        else
        {
            updateButton(0);
            updateFile(0);

            StartCoroutine(animateIcons());

            if (fileCount == 1)
            {
                fileNumbersText.text = "File     1";
            }
            else if (fileCount == 2)
            {
                fileNumbersText.text = "File     1   2";
            }
            else if (fileCount == 3)
            {
                fileNumbersText.text = "File     1   2   3";
            }
        }

        bool running = true;
        while (running)
        {
            if (Input.GetButtonDown("Select"))
            {
                if (selectedButton == 0)
                {
                    //CONTINUE
                    //yield return new WaitForSeconds(sceneTransition.FadeOut(0.4f));
                    yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));

                    SaveData.currentSave = SaveLoad.savedGames[selectedFile];

                    Debug.Log(SaveLoad.savedGames[0]);
                    Debug.Log(SaveLoad.savedGames[1]);
                    Debug.Log(SaveLoad.savedGames[2]);
                    GlobalVariables.global.playerPosition = SaveData.currentSave.Player.Position;
                    GlobalVariables.global.playerDirection = SaveData.currentSave.Player.Direction;

                    Application.LoadLevel(SaveData.currentSave.savefile.levelName);
                }
                else if (selectedButton == 1)
                {
                    //NEW GAME
                    //yield return new WaitForSeconds(sceneTransition.FadeOut(0.4f));
                    yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));

                    SaveData.currentSave = new SaveData(fileCount);

                    GlobalVariables.global.SetDEBUGFileData();

                    GlobalVariables.global.playerPosition = new Vector3(78, 0, 29);
                    GlobalVariables.global.playerDirection = 2;
                    GlobalVariables.global.fadeIn = true;
                    Application.LoadLevel("indoorsNW");
                }
                else if (selectedButton == 2)
                {
                    //SETTINGS
                    //yield return new WaitForSeconds(sceneTransition.FadeOut(0.4f));
                    yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));

                    Scene.main.Settings.gameObject.SetActive(true);
                    StartCoroutine(Scene.main.Settings.control());
                    while (Scene.main.Settings.gameObject.activeSelf)
                    {
                        yield return null;
                    }

                    //yield return new WaitForSeconds(sceneTransition.FadeIn(0.4f));
                    yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));
                }
            }
            else if (Input.GetKeyDown(KeyCode.Delete))
            {
                //delete save file
                float time = Time.time;
                bool released = false;
                Debug.Log("Save " + (selectedFile + 1) + " will be deleted! Release 'Delete' key to prevent this!");
                while (Time.time < time + 4 && !released)
                {
                    if (Input.GetKeyUp(KeyCode.Delete))
                    {
                        released = true;
                    }
                    yield return null;
                }

                if (Input.GetKey(KeyCode.Delete) && !released)
                {
                    SaveLoad.resetSaveGame(selectedFile);
                    Debug.Log("Save " + (selectedFile + 1) + " was deleted!");

                    yield return new WaitForSeconds(1f);

                    Application.LoadLevel(Application.loadedLevel);
                }
                else
                {
                    Debug.Log("'Delete' key was released!");
                }
            }
            else
            {
                if (Input.GetAxisRaw("Vertical") > 0)
                {
                    float minimumButton = (continueButton.activeSelf) ? 0 : 1;
                    if (selectedButton > minimumButton)
                    {
                        updateButton(selectedButton - 1);
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                else if (Input.GetAxisRaw("Vertical") < 0)
                {
                    if (selectedButton < 2)
                    {
                        updateButton(selectedButton + 1);
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    if (selectedButton == 0)
                    {
                        if (selectedFile < fileCount - 1)
                        {
                            updateFile(selectedFile + 1);
                            yield return new WaitForSeconds(0.2f);
                        }
                    }
                }
                else if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    if (selectedButton == 0)
                    {
                        if (selectedFile > 0)
                        {
                            updateFile(selectedFile - 1);
                            yield return new WaitForSeconds(0.2f);
                        }
                    }
                }
            }


            yield return null;
        }
    }
}