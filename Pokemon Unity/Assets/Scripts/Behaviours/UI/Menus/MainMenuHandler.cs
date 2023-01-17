//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System;
using EasyButtons;
using UnityEngine.SceneManagement;

[AddComponentMenu("Pokemon Unity/UI/Menus/Main Menu")]
public class MainMenuHandler : MonoBehaviour {
    // TODO: implement file selection and mystery gift menus
    [Description("Make sure the scene is also in the build settings")]
    public string NewGameScene = "overworldTest";

    #region Old Variables

    public AudioClip scrollClip;
    public AudioClip decideClip;
    public AudioClip cancelClip;
    public AudioClip pokeGetMFX;
    public AudioClip itemGetMFX;

    public int selectedButton = 0;
    public int selectedFile = 0;

    public Sprite buttonSelectedSprite;
    public Sprite buttonDimmedSprite;
    public Sprite maleSprite;
    public Sprite femaleSprite;

    private GameObject fileDataPanel;
    private GameObject newGameButton;
    private GameObject mysteryGiftButton;
    private GameObject settingsButton;

    private List<Image> activeButtons = new List<Image>();
    private List<Image> buttonImages = new List<Image>();
    private Text[] buttonTextImage = new Text[4];
    private Text[] buttonTextShadowImage = new Text[4];

    private Text fileNumbers;
    private Text fileNumbersShadow;
    private Text fileSelectedText;

    private Text playerNameShadow;
    private Text playerName;
    private Text currentMap;
    private Text currentMapShadow;
    private Text badges;
    private Text badgesShadow;
    private Text time;
    private Text timeShadow;
    private Image playerSprite;

    private Text fileNumbersText;
    private Text fileNumbersTextShadow;
    private Text fileSelected;

    private Text mapNameText;
    private Text mapNameTextShadow;
    private Image[] pokemon = new Image[6];

    public GameObject mysteryGiftMenu;
    public AudioClip mysteryGiftBGM;
    public int loopSampleStart;

    private Image[] MGButtonImage = new Image[2];

    #endregion

    void Start() {
        if (SaveLoad.GetSavedGamesCount() == 0)
            ShowContinueButton();

        return;
        //playerSprite = fileDataPanel.transform.Find("player").GetComponent<Image>();
        //playerNameShadow = fileDataPanel.transform.Find("playername").GetComponent<Text>();
        //playerName = playerNameShadow.transform.Find("Text").GetComponent<Text>();
        //currentMapShadow = fileDataPanel.transform.Find("MapName").GetComponent<Text>();
        //currentMap = currentMapShadow.transform.Find("Text").GetComponent<Text>();
        //badgesShadow = fileDataPanel.transform.Find("Badges").GetComponent<Text>();
        //badges = badgesShadow.transform.Find("Text").GetComponent<Text>();
        //timeShadow = fileDataPanel.transform.Find("Time").GetComponent<Text>();
        //time = timeShadow.transform.Find("Text").GetComponent<Text>();

        ///*
        //dataText = fileDataPanel.transform.Find("DataText").GetComponent<Text>();
        //dataTextShadow = dataText.transform.Find("DataTextShadow").GetComponent<Text>();
        //*/

        //for (int i = 0; i < 6; ++i) {
        //    pokemon[i] = fileDataPanel.transform.Find("pokemon" + i).GetComponent<Image>();
        //}

        ////Mystery Gift Menu

        //MGButtonImage = new Image[]
        //{
        //    mysteryGiftMenu.transform.Find("EnterCode").GetComponent<Image>(),
        //    mysteryGiftMenu.transform.Find("Quit").GetComponent<Image>()
        //};
        ////mysteryGiftMenu.SetActive(false);
    }

    public void NewGame() => StartCoroutine(NewGameCoroutine());
    public void ContinueGame() => StartCoroutine(ContinueGameCoroutine());
    public void MysteryGift() => StartCoroutine(MysteryGiftCoroutine());
    
    public IEnumerator NewGameCoroutine() {
        yield return StartCoroutine(ScreenFade.Singleton.Fade(false, ScreenFade.DefaultSpeed));

        SaveData.currentSave = new SaveData(SaveLoad.GetSavedGamesCount());
        //SaveData.SetDebugFileData();

        GlobalVariables.Singleton.playerPosition = new Vector3(2, 0, -3);
        GlobalVariables.Singleton.playerDirection = 0;
        GlobalVariables.Singleton.fadeIn = true;

        SceneManager.LoadScene(NewGameScene);
        
        yield return StartCoroutine(ScreenFade.Singleton.Fade(true, ScreenFade.DefaultSpeed));
    }

    public IEnumerator ContinueGameCoroutine() {
        //CONTINUE
        SfxHandler.Play(decideClip);
        yield return StartCoroutine(ScreenFade.Singleton.Fade(false, ScreenFade.DefaultSpeed));

        SaveData.currentSave = SaveLoad.savedGames[selectedFile];

        Debug.Log(SaveLoad.savedGames[0]);
        Debug.Log(SaveLoad.savedGames[1]);
        Debug.Log(SaveLoad.savedGames[2]);

        GlobalVariables.Singleton.playerPosition = SaveData.currentSave.playerPosition.v3;
        GlobalVariables.Singleton.playerDirection = SaveData.currentSave.playerDirection;
        GlobalVariables.Singleton.isFollowerOut = SaveData.currentSave.followerOut;

        if (SaveData.currentSave.followerPosition != null && SaveData.currentSave.followerdirection != null) {
            GlobalVariables.Singleton.followerPosition = SaveData.currentSave.followerPosition.Value.v3;
            GlobalVariables.Singleton.followerDirection = SaveData.currentSave.followerdirection;
        }

        GlobalVariables.Singleton.fadeIn = true;

        SceneManager.LoadScene(SaveData.currentSave.levelName);
    }

    public IEnumerator MysteryGiftCoroutine() {
        SfxHandler.Play(decideClip);

        DialogBoxHandlerNew Dialog = mysteryGiftMenu.GetComponent<DialogBoxHandlerNew>();

        MGButtonImage[0].sprite = buttonSelectedSprite;
        MGButtonImage[1].sprite = buttonDimmedSprite;

        yield return StartCoroutine(ScreenFade.Singleton.Fade(false, ScreenFade.DefaultSpeed));
        mysteryGiftMenu.SetActive(true);
        yield return StartCoroutine(ScreenFade.Singleton.Fade(true, ScreenFade.DefaultSpeed));

        BgmHandler.main.PlayMain(mysteryGiftBGM, loopSampleStart);

        bool leave = false;
        int selectedButton = 1;
        int minimumButton = 0;
        int maximumButton = MGButtonImage.Length - 1;

        #region commented code

        /*yield return StartCoroutine(mysteryGiftMenu.GetComponent<MysteryGiftHandler>().startRequest());
        
        MysteryGiftHandler.MysteryGift[] giftList = mysteryGiftMenu.GetComponent<MysteryGiftHandler>().getList();

        if (giftList.Length == 0)
        {
            leave = true;
            //start dialog to tell that the connection wasn't established
            //===> add a dialog box handler
        }
            

        while (!Input.GetButtonDown("Back") && !leave)
        {
            //TODO Mystery Gift menu
            if (Input.GetAxisRaw("Vertical") < 0)
            {
                if (selectedButton > minimumButton)
                {
                    updateMGButton(selectedButton--, selectedButton);
                    SfxHandler.Play(scrollClip);
                    Debug.Log(selectedButton);
                    yield return new WaitForSeconds(0.2f);
                }
            }
            else if (Input.GetAxisRaw("Vertical") > 0)
            {
                if (selectedButton < maximumButton)
                {
                    updateMGButton(selectedButton++, selectedButton);
                    SfxHandler.Play(scrollClip);
                    Debug.Log(selectedButton);
                    yield return new WaitForSeconds(0.2f);
                }
            }
            else if (Input.GetButtonDown("Select"))
            {
                switch (selectedButton)
                {
                    case 0:
                        //Quit
                        leave = true;
                        break;
                    case 1:
                        SfxHandler.Play(decideClip);
                        string code = "";
                        
                        //Enter Code
                        yield return StartCoroutine(ScreenFade.main.Fade(false, MenuFadeTime));

                        Scene.main.Typing.gameObject.SetActive(true);
                        StartCoroutine(Scene.main.Typing.control(10, ""));
                        while (Scene.main.Typing.gameObject.activeSelf)
                        {
                            yield return null;
                        }
                        if (Scene.main.Typing.typedString.Length > 0)
                        {
                            code = Scene.main.Typing.typedString;
                        }
                        
                        Debug.Log(code);

                        bool validCode = false;
                        int giftIndex = -1;

                        for (int i = 0; i < giftList.Length; ++i)
                        {
                            if (giftList[i].code == code)
                            {
                                validCode = true;
                                giftIndex = i;
                                break;
                            }
                        }

                        if (SaveLoad.savedGames[selectedFile] != null && validCode && giftIndex > -1 && !SaveLoad.savedGames[selectedFile].mysteryGiftCodes.Contains(code))
                        {
                            //Add dialog and show pokemon
                            Debug.Log("Mystery Gift Found!");
                            MysteryGiftHandler.MysteryGift gift = giftList[giftIndex];
                            Debug.Log(giftList[giftIndex].ToString());
                            
                            mysteryGiftMenu.transform.Find("EnterCode").gameObject.SetActive(false);
                            mysteryGiftMenu.transform.Find("Quit").gameObject.SetActive(false);

                            Transform giftScene = mysteryGiftMenu.transform.Find("GiftScene");
                            
                            giftScene.gameObject.SetActive(true);
                            
                            if (gift.type == "pokemon")
                            {
                                //Generate Pokémon
                                PokemonData pkd = PokemonDatabase.getPokemon(gift.id);

                                string pkName = pkd.getName();
                                Pokemon.Gender pkGender = Pokemon.Gender.CALCULATE;

                                if (pkd.getMaleRatio() == -1)
                                {
                                    pkGender = Pokemon.Gender.NONE;
                                }
                                else if (pkd.getMaleRatio() == 0)
                                {
                                    pkGender = Pokemon.Gender.FEMALE;
                                }
                                else if (pkd.getMaleRatio() == 100)
                                {
                                    pkGender = Pokemon.Gender.MALE;
                                }
                                else
                                {
                                    //if not a set gender
                                    if (gift.gender == 0)
                                    {
                                        pkGender = Pokemon.Gender.MALE;
                                    }
                                    else if (gift.gender == 1)
                                    {
                                        pkGender = Pokemon.Gender.FEMALE;
                                    }
                                }

                                string nickname = "";
                                if (!string.IsNullOrEmpty(gift.nickname))
                                {
                                    nickname = gift.nickname;
                                }

                                int[] IVs = new int[]
                                {
                                    Random.Range(0, 32), Random.Range(0, 32), Random.Range(0, 32),
                                    Random.Range(0, 32), Random.Range(0, 32), Random.Range(0, 32)
                                };
                                //if using Custom IVs
                                if (gift.IVsHP != null)
                                {
                                    IVs[0] = gift.IVsHP;
                                }
                                if (gift.IVsAtk != null)
                                {
                                    IVs[1] = gift.IVsAtk;
                                }
                                if (gift.IVsDef != null)
                                {
                                    IVs[2] = gift.IVsDef;
                                }
                                if (gift.IVsAtkSpe != null)
                                {
                                    IVs[3] = gift.IVsAtkSpe;
                                }
                                if (gift.IVsDefSpe != null)
                                {
                                    IVs[4] = gift.IVsDefSpe;
                                }
                                if (gift.IVsSpe != null)
                                {
                                    IVs[5] = gift.IVsSpe;
                                }

                                string pkNature = (gift.nature == 0)
                                    ? NatureDatabase.getRandomNature().getName()
                                    : NatureDatabase.getNature(gift.nature - 1).getName();

                                string[] pkMoveset = pkd.GenerateMoveset(gift.id);
                                if (!string.IsNullOrEmpty(gift.move1))
                                {
                                    pkMoveset[0] = gift.move1;
                                }
                                if (!string.IsNullOrEmpty(gift.move2))
                                {
                                    pkMoveset[1] = gift.move2;
                                }
                                if (!string.IsNullOrEmpty(gift.move3))
                                {
                                    pkMoveset[2] = gift.move3;
                                }
                                if (!string.IsNullOrEmpty(gift.move4))
                                {
                                    pkMoveset[3] = gift.move4;
                                }
                                
                                Pokemon pk = new Pokemon(gift.id, nickname, pkGender, gift.level,
                                    gift.isShiny, gift.ball, gift.item,
                                    gift.OGTrainer, IVs[0], IVs[1], IVs[2], IVs[3], IVs[4], IVs[5], 0, 0, 0, 0, 0, 0,
                                    pkNature, gift.ability,
                                    pkMoveset, new int[4]);
                                //End
                                
                                SaveLoad.savedGames[selectedFile].PC.addPokemon(pk, true);

                                SaveLoad.savedGames[selectedFile].mysteryGiftCodes.Add(code);
                                SaveLoad.Save(selectedFile);
                                updateFile(selectedFile);
                                
                                SfxHandler.Play(pk.GetCry());
                                giftScene.Find("SelectedSprite").GetComponent<Image>().sprite = pk.GetFrontAnim_()[0];
                                
                                yield return StartCoroutine(ScreenFade.main.Fade(true, MenuFadeTime));

                                yield return new WaitForSeconds(1);

                                Dialog.DrawDialogBox();
                                yield return
                                    StartCoroutine(Dialog.DrawText("You received a "+pk.getName()+"!"));
                                BgmHandler.main.PlayMFX(pokeGetMFX);
                                yield return new WaitForSeconds(pokeGetMFX.length);
                                Dialog.UndrawDialogBox();
                                Dialog.DrawDialogBox();
                                yield return StartCoroutine(Dialog.DrawText(pk.getName()+"\nhas been placed in your PC."));
                                while (!Input.GetButtonDown("Select"))
                                {
                                    yield return null;
                                }
                                Dialog.UndrawDialogBox();

                                yield return new WaitForSeconds(0.5f);
                                
                                yield return StartCoroutine(ScreenFade.main.Fade(false, MenuFadeTime));
                                giftScene.gameObject.SetActive(false);
                                mysteryGiftMenu.transform.Find("EnterCode").gameObject.SetActive(true);
                                mysteryGiftMenu.transform.Find("Quit").gameObject.SetActive(true);
                            }
                        }
                        else if (SaveLoad.savedGames[selectedFile] == null)
                        {
                            //No Mystery Gift Found
                            mysteryGiftMenu.transform.Find("EnterCode").gameObject.SetActive(false);
                            mysteryGiftMenu.transform.Find("Quit").gameObject.SetActive(false);
                            yield return StartCoroutine(ScreenFade.main.Fade(true, MenuFadeTime));
                            
                            Debug.Log("No Save Found.");
                            yield return new WaitForSeconds(1);
                            
                            Dialog.DrawDialogBox();
                            yield return StartCoroutine(Dialog.DrawText("Please create a save to redeem\nmystery gifts."));
                            while (!Input.GetButtonDown("Select"))
                            {
                                yield return null;
                            }
                            Dialog.UndrawDialogBox();
                            
                            yield return new WaitForSeconds(0.5f);

                            yield return StartCoroutine(ScreenFade.main.Fade(false, MenuFadeTime));
                            mysteryGiftMenu.transform.Find("EnterCode").gameObject.SetActive(true);
                            mysteryGiftMenu.transform.Find("Quit").gameObject.SetActive(true);
                        }
                        else
                        {
                            //No Mystery Gift Found
                            mysteryGiftMenu.transform.Find("EnterCode").gameObject.SetActive(false);
                            mysteryGiftMenu.transform.Find("Quit").gameObject.SetActive(false);
                            yield return StartCoroutine(ScreenFade.main.Fade(true, MenuFadeTime));
                            
                            Debug.Log("No Mystery Gift Found");
                            yield return new WaitForSeconds(1);
                            
                            Dialog.DrawDialogBox();
                            yield return StartCoroutine(Dialog.DrawText("No Mystery Gift found with this code."));
                            while (!Input.GetButtonDown("Select"))
                            {
                                yield return null;
                            }
                            Dialog.UndrawDialogBox();
                            
                            yield return new WaitForSeconds(0.5f);

                            yield return StartCoroutine(ScreenFade.main.Fade(false, MenuFadeTime));
                            mysteryGiftMenu.transform.Find("EnterCode").gameObject.SetActive(true);
                            mysteryGiftMenu.transform.Find("Quit").gameObject.SetActive(true);
                        }
                        
                        yield return StartCoroutine(ScreenFade.main.Fade(true, MenuFadeTime));
                        break;
                }
            }
            yield return null;
        }
        if (giftList.Length > 0)
            SfxHandler.Play(cancelClip);*/

        #endregion

        BgmHandler.main.PlayMain(null, 0);
        yield return StartCoroutine(ScreenFade.Singleton.Fade(false, ScreenFade.DefaultSpeed));
        mysteryGiftMenu.SetActive(false);
        yield return StartCoroutine(ScreenFade.Singleton.Fade(true, ScreenFade.DefaultSpeed));
    }

    public void ShowContinueButton() {
        // TODO

        //changeButtons(new GameObject[] { newGameButton, settingsButton });
        ///*updateButton(1);
        //continueButton.SetActive(false);
        //fileDataPanel.SetActive(false);*/
        //for (int i = 1; i < 4; i++) {
        //    buttonImages[i].rectTransform.position = new Vector3(
        //        buttonImages[i].rectTransform.position.x,
        //        buttonImages[i].rectTransform.position.y + 26f,
        //        buttonImages[i].rectTransform.position.z
        //        );
        //}
    }

    public void ShowFileSelection() {
        //changeButtons(new GameObject[] { newGameButton, settingsButton, fileDataPanel }, 1);
        //updateButton(0);
        updateFile(0);

        StartCoroutine(animateIcons());

        int fileCount = SaveLoad.GetSavedGamesCount();
        if (fileCount == 1) {
            fileNumbersText.text = "File     1";
        } else if (fileCount == 2) {
            fileNumbersText.text = "File     1   2";
        } else if (fileCount == 3) {
            fileNumbersText.text = "File     1   2   3";
        }
    }

    #region everything below is old code

    private void updateFile(int newFileIndex) {
        selectedFile = newFileIndex;

        Vector3[] highlightPositions = new Vector3[]
        {
            new Vector3(31f, 0, 0),
            new Vector3(46, 0, 0),
            new Vector3(61, 0, 0)
        };
        fileSelected.rectTransform.localPosition = highlightPositions[selectedFile];
        fileSelected.text = "" + (selectedFile + 1);

        if (SaveLoad.savedGames[selectedFile] != null) {
            int badgeTotal = 0;
            for (int i = 0; i < 12; i++) {
                if (SaveLoad.savedGames[selectedFile].gymsBeaten[i]) {
                    badgeTotal += 1;
                }
            }
            string playerTime = "" + SaveLoad.savedGames[selectedFile].playerMinutes;
            if (playerTime.Length == 1) {
                playerTime = "0" + playerTime;
            }
            playerTime = SaveLoad.savedGames[selectedFile].playerHours + " : " + playerTime;

            currentMap.text = SaveLoad.savedGames[selectedFile].mapName;
            currentMapShadow.text = currentMap.text;

            /*
            private Text currentMap;
            private Text currentMapShadow;
            private Text badges;
            private Text badgesShadow;
            private Text time;
            private Text timeShadow;
            private Image playerSprite;
            */

            badges.text = "" + badgeTotal;
            badgesShadow.text = badges.text;
            time.text = "" + playerTime;
            timeShadow.text = time.text;
            playerName.text = SaveLoad.savedGames[selectedFile].playerName;
            playerNameShadow.text = playerName.text;


            if (SaveLoad.savedGames[selectedFile].isMale) {
                playerSprite.sprite = maleSprite;
                playerName.color = new Color(0.259f, 0.557f, 0.937f, 1.00f);
            } else {
                playerSprite.sprite = femaleSprite;
                playerName.color = new Color(0.965f, 0.255f, 0.255f, 1.00f);
            }

            /*
            dataText.text = SaveLoad.savedGames[selectedFile].playerName
                            + "\n" + badgeTotal
                            + "\n" + "0" //Pokedex not yet implemented
                            + "\n" + playerTime;
            dataTextShadow.text = dataText.text;
            */

            for (int i = 0; i < 6; i++) {
                if (SaveLoad.savedGames[selectedFile].PC.boxes[0][i] != null) {
                    pokemon[i].enabled = true;
                    pokemon[i].sprite = SaveLoad.savedGames[selectedFile].PC.boxes[0][i].GetIconsSprite()[0];
                } else {
                    pokemon[i].sprite = null;
                    pokemon[i].enabled = false;
                }
            }
        }
    }

    private IEnumerator animateIcons() {
        while (true) {
            for (int i = 0; i < 6; i++) {
                //pokemon[i].sprite = SaveLoad.savedGames[selectedFile].PC.boxes[0][i].GetIconsSprite()[0];
            }
            yield return new WaitForSeconds(0.15f);
            for (int i = 0; i < 6; i++) {
                //pokemon[i].sprite = SaveLoad.savedGames[selectedFile].PC.boxes[0][i].GetIconsSprite()[1];
            }
            yield return new WaitForSeconds(0.15f);
        }
    }

    public IEnumerator control() {
        while (true) {
            if (Input.GetKeyDown(KeyCode.Delete)) {
                //delete save file
                float time = Time.time;
                bool released = false;
                Debug.Log("Save " + (selectedFile + 1) + " will be deleted! Release 'Delete' key to prevent this!");
                while (Time.time < time + 4 && !released) {
                    if (Input.GetKeyUp(KeyCode.Delete)) {
                        released = true;
                    }
                    yield return null;
                }

                if (Input.GetKey(KeyCode.Delete) && !released) {
                    SaveLoad.resetSaveGame(selectedFile);
                    Debug.Log("Save " + (selectedFile + 1) + " was deleted!");

                    yield return new WaitForSeconds(1f);

                    Application.LoadLevel(Application.loadedLevel);
                } else {
                    Debug.Log("'Delete' key was released!");
                }
            }

            yield return null;
        }
    }

    #endregion
}