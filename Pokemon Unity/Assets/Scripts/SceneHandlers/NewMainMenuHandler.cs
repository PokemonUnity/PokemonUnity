using UnityEngine;
using System.Collections;

public class NewMainMenuHandler : MonoBehaviour
{
    public int selectedButton = 0;
    public int selectedFile = 0;

    //public Texture buttonSelected;
    //public Texture buttonDimmed;

    private Color buttonDimmedColor = new Color(227, 227, 227);

    public GameObject fileDataPanel;
    public GameObject continueButton;

    public UnityEngine.UI.Image[] button;
    public UnityEngine.UI.Image[] buttonHighlight;

    void Awake()
    {
        SaveLoad.Load(); // Create File if it is not exist. Then Load File.

        fileDataPanel = transform.Find("FileData").gameObject;
        continueButton = transform.Find("Continue").gameObject;

        //Transform newGameButton = transform.Find("NewGame");
        //Transform settingsButton = transform.Find("Settings");
        //
        //Transform[] buttonTransforms = new Transform[]
        //{
        //    continueButton.transform,
        //    newGameButton,
        //    settingsButton
        //};
        //
        //for (int i = 0; i < 3; i++)
        //{
        //    button[i] = buttonTransforms[i].Find("ButtonTexture").GetComponent<UnityEngine.UI.Image>();
        //    buttonHighlight[i] = buttonTransforms[i].Find("ButtonHighlight").GetComponent<UnityEngine.UI.Image>();
        //}
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(control());
    }

    private void UpdateButton(int newButtonIndex)
    {
        if (newButtonIndex != selectedButton)
        {
            //button[selectedButton].sprite = Sprite.Create(buttonDimmed);
            button[selectedButton].color = buttonDimmedColor; // Color Dimmed
            buttonHighlight[selectedButton].gameObject.SetActive(false); // Disable Highlight
        }
        selectedButton = newButtonIndex;

        button[selectedButton].color = Color.white;
        buttonHighlight[selectedButton].gameObject.SetActive(true);
    }

    enum Inputs
    {
        Select,
        Start,
        Vertical,
        Horizontal
    }

    public IEnumerator control()
    {
        // Setup
        int fileCount = SaveLoad.getSavedGamesCount();

        if (fileCount == 0)
        {
            UpdateButton(1);
            continueButton.SetActive(false);
            fileDataPanel.SetActive(false);
            //for (int i = 1; i < 3; i++)
            //{
            //    // RePosition of Button
            //}
        }

        bool running = true;
        while(running)
        {
            if (Input.GetButtonDown(Inputs.Select.ToString()))
            {
                if (selectedButton == 0)
                {
                    //CONTINUE
                    new System.NotImplementedException();
                    ////yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f)); // Fade
                    //SaveData.currentSave = SaveLoad.savedGames[selectedFile];
                    //
                    //Debug.Log(SaveLoad.savedGames[0]);
                    //Debug.Log(SaveLoad.savedGames[1]);
                    //Debug.Log(SaveLoad.savedGames[2]);
                    //
                    //GlobalVariables.global.playerPosition = SaveData.currentSave.playerPosition.v3;
                    //GlobalVariables.global.playerDirection = SaveData.currentSave.playerDirection;
                    //
                    //UnityEngine.SceneManagement.SceneManager.LoadScene(SaveData.currentSave.levelName);
                }
                else if (selectedButton == 1)
                {
                    //NEW GAME
                    yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));

                    SaveData.currentSave = new SaveData(fileCount);

                    GlobalVariables.global.SetDEBUGFileData(); // Set to Debug file

                    GlobalVariables.global.playerPosition = new Vector3(78, 0, 29); // Setup Player
                    GlobalVariables.global.playerDirection = 2;                     // Setup Player
                    GlobalVariables.global.fadeIn = true;                           // Setup Player

                    UnityEngine.SceneManagement.SceneManager.LoadScene("indoorsNW"); // Where new Player started
                }
                else if (selectedButton == 2)
                {
                    Debug.Log("Not Implemented");
                    new System.NotImplementedException();
                    
                    ////SETTINGS
                    //yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));
                    //
                    //Scene.main.Settings.gameObject.SetActive(true);
                    //StartCoroutine(Scene.main.Settings.control());
                    //while (Scene.main.Settings.gameObject.activeSelf)
                    //{
                    //    yield return null;
                    //}
                    //
                    ////yield return new WaitForSeconds(sceneTransition.FadeIn(0.4f));
                    //yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));
                }
            }
            else
            {
                if (Input.GetAxisRaw("Vertical") > 0)
                {
                    float minimumButton = (continueButton.activeSelf) ? 0 : 1;
                    if (selectedButton > minimumButton)
                    {
                        UpdateButton(selectedButton - 1);
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                else if (Input.GetAxisRaw("Vertical") < 0)
                {
                    if (selectedButton < 2)
                    {
                        UpdateButton(selectedButton + 1);
                        yield return new WaitForSeconds(0.2f);
                    }
                }

                // This can be better
                if (Input.GetAxisRaw("Horizontal") > 0) 
                {
                    if (selectedButton == 0)
                    {
                        if (selectedFile < fileCount - 1)
                        {
                            //updateFile(selectedFile + 1);
                            new System.NotImplementedException("Multiple Save File not supported");
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
                            //updateFile(selectedFile - 1);
                            new System.NotImplementedException("Multiple Save File not supported");
                            yield return new WaitForSeconds(0.2f);
                        }
                    }
                }
            }
            yield return null;
        }
        
    }
}
