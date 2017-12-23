//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class PartyHandler : MonoBehaviour
{
    private DialogBoxHandler Dialog;

    private GUITexture cancel;

    public Texture selectBallOpen;
    public Texture selectBallClosed;

    public Texture panelRound;
    public Texture panelRect;
    public Texture panelRoundFaint;
    public Texture panelRectFaint;
    public Texture panelRoundSwap;
    public Texture panelRectSwap;
    public Texture panelRoundSel;
    public Texture panelRectSel;
    public Texture panelRoundFaintSel;
    public Texture panelRectFaintSel;
    public Texture panelRoundSwapSel;
    public Texture panelRectSwapSel;

    public Texture cancelTex;
    public Texture cancelHighlightTex;

    private GUITexture[] slot = new GUITexture[6];

    private GUITexture[] selectBall = new GUITexture[6];
    private GUITexture[] icon = new GUITexture[6];
    private GUIText[] pokemonName = new GUIText[6];
    private GUIText[] pokemonNameShadow = new GUIText[6];
    private GUIText[] gender = new GUIText[6];
    private GUIText[] genderShadow = new GUIText[6];
    private GUITexture[] HPBarBack = new GUITexture[6];
    private GUITexture[] HPBar = new GUITexture[6];
    private GUITexture[] lv = new GUITexture[6];
    private GUIText[] level = new GUIText[6];
    private GUIText[] levelShadow = new GUIText[6];
    private GUIText[] currentHP = new GUIText[6];
    private GUIText[] currentHPShadow = new GUIText[6];
    private GUIText[] slash = new GUIText[6];
    private GUIText[] slashShadow = new GUIText[6];
    private GUIText[] maxHp = new GUIText[6];
    private GUIText[] maxHPShadow = new GUIText[6];
    private GUITexture[] status = new GUITexture[6];
    private GUITexture[] item = new GUITexture[6];

    private AudioSource PartyAudio;
    public AudioClip selectClip;

    private int currentPosition;
    private int swapPosition = -1;
    private bool running = false;
    private bool switching = false;

    void Awake()
    {
        Dialog = transform.GetComponent<DialogBoxHandler>();
        //sceneTransition = transform.GetComponent<SceneTransition>();
        PartyAudio = transform.GetComponent<AudioSource>();

        cancel = transform.Find("Cancel").GetComponent<GUITexture>();

        for (int i = 0; i < 6; i++)
        {
            slot[i] = transform.Find("Slot" + i).GetComponent<GUITexture>();

            selectBall[i] = slot[i].transform.Find("SelectBall").GetComponent<GUITexture>();
            icon[i] = slot[i].transform.Find("Icon").GetComponent<GUITexture>();
            pokemonName[i] = slot[i].transform.Find("Name").GetComponent<GUIText>();
            pokemonNameShadow[i] = pokemonName[i].transform.Find("NameShadow").GetComponent<GUIText>();
            gender[i] = slot[i].transform.Find("Gender").GetComponent<GUIText>();
            genderShadow[i] = gender[i].transform.Find("GenderShadow").GetComponent<GUIText>();
            HPBarBack[i] = slot[i].transform.Find("HPBarBack").GetComponent<GUITexture>();
            HPBar[i] = slot[i].transform.Find("HPBar").GetComponent<GUITexture>();
            lv[i] = slot[i].transform.Find("Lv.").GetComponent<GUITexture>();
            level[i] = slot[i].transform.Find("Level").GetComponent<GUIText>();
            levelShadow[i] = level[i].transform.Find("LevelShadow").GetComponent<GUIText>();
            currentHP[i] = slot[i].transform.Find("CurrentHP").GetComponent<GUIText>();
            currentHPShadow[i] = currentHP[i].transform.Find("CurrentHPShadow").GetComponent<GUIText>();
            slash[i] = slot[i].transform.Find("Slash").GetComponent<GUIText>();
            slashShadow[i] = slash[i].transform.Find("SlashShadow").GetComponent<GUIText>();
            maxHp[i] = slot[i].transform.Find("MaxHP").GetComponent<GUIText>();
            maxHPShadow[i] = maxHp[i].transform.Find("MaxHPShadow").GetComponent<GUIText>();
            status[i] = slot[i].transform.Find("Status").GetComponent<GUITexture>();
            item[i] = slot[i].transform.Find("Item").GetComponent<GUITexture>();
        }
    }

    void Start()
    {
        updateParty();
        this.gameObject.SetActive(false);
    }

    private void updateParty()
    {
        for (int i = 0; i < 6; i++)
        {
            Pokemon selectedPokemon = SaveData.currentSave.PC.boxes[0][i];
            if (selectedPokemon == null)
            {
                slot[i].gameObject.SetActive(false);
            }
            else
            {
                slot[i].gameObject.SetActive(true);
                selectBall[i].texture = selectBallClosed;
                icon[i].texture = selectedPokemon.GetIcons();
                pokemonName[i].text = selectedPokemon.getName();
                pokemonNameShadow[i].text = pokemonName[i].text;
                if (selectedPokemon.getGender() == Pokemon.Gender.FEMALE)
                {
                    gender[i].text = "♀";
                    gender[i].color = new Color(1, 0.2f, 0.2f, 1);
                }
                else if (selectedPokemon.getGender() == Pokemon.Gender.MALE)
                {
                    gender[i].text = "♂";
                    gender[i].color = new Color(0.2f, 0.4f, 1, 1);
                }
                else
                {
                    gender[i].text = null;
                }
                genderShadow[i].text = gender[i].text;
                HPBar[i].pixelInset = new Rect(HPBar[i].pixelInset.x, HPBar[i].pixelInset.y,
                    Mathf.FloorToInt(48f * ((float) selectedPokemon.getCurrentHP() / (float) selectedPokemon.getHP())),
                    HPBar[i].pixelInset.height);

                if ((float) selectedPokemon.getCurrentHP() < ((float) selectedPokemon.getHP() / 4f))
                {
                    HPBar[i].color = new Color(1, 0.125f, 0, 1);
                }
                else if ((float) selectedPokemon.getCurrentHP() < ((float) selectedPokemon.getHP() / 2f))
                {
                    HPBar[i].color = new Color(1, 0.75f, 0, 1);
                }
                else
                {
                    HPBar[i].color = new Color(0.125f, 1, 0.065f, 1);
                }

                level[i].text = "" + selectedPokemon.getLevel();
                levelShadow[i].text = level[i].text;
                currentHP[i].text = "" + selectedPokemon.getCurrentHP();
                currentHPShadow[i].text = currentHP[i].text;
                maxHp[i].text = "" + selectedPokemon.getHP();
                maxHPShadow[i].text = maxHp[i].text;
                if (selectedPokemon.getStatus() != Pokemon.Status.NONE)
                {
                    status[i].texture =
                        Resources.Load<Texture>("PCSprites/status" + selectedPokemon.getStatus().ToString());
                }
                else
                {
                    status[i].texture = null;
                }
                if (!string.IsNullOrEmpty(selectedPokemon.getHeldItem()))
                {
                    item[i].enabled = true;
                }
                else
                {
                    item[i].enabled = false;
                }
            }
        }
    }

    private void shiftPosition(int move)
    {
        int repetitions = Mathf.Abs(move);
        for (int i = 0; i < repetitions; i++)
        {
            if (move > 0)
            {
                //add
                if (currentPosition < 5)
                {
                    if (SaveData.currentSave.PC.boxes[0][currentPosition + 1] == null)
                    {
                        currentPosition = 6;
                    }
                    else
                    {
                        currentPosition += 1;
                    }
                }
                else if (currentPosition == 5)
                {
                    currentPosition = 6;
                }
            }
            else if (move < 0)
            {
                //subtract
                if (currentPosition == 6)
                {
                    currentPosition -= 1;
                    while (SaveData.currentSave.PC.boxes[0][currentPosition] == null)
                    {
                        currentPosition -= 1;
                    }
                }
                else if (currentPosition > 0)
                {
                    currentPosition -= 1;
                }
            }
        }
        updateFrames();
    }

    private void updateFrames()
    {
        for (int i = 0; i < 6; i++)
        {
            Pokemon selectedPokemon = SaveData.currentSave.PC.boxes[0][i];
            if (selectedPokemon != null)
            {
                if (i == swapPosition)
                {
                    if (i == 0)
                    {
                        if (i == currentPosition)
                        {
                            slot[i].texture = panelRoundSwapSel;
                        }
                        else
                        {
                            slot[i].texture = panelRoundSwap;
                        }
                    }
                    else
                    {
                        if (i == currentPosition)
                        {
                            slot[i].texture = panelRectSwapSel;
                        }
                        else
                        {
                            slot[i].texture = panelRectSwap;
                        }
                    }
                }
                else
                {
                    if (selectedPokemon.getCurrentHP() == 0)
                    {
                        if (i == 0)
                        {
                            if (i == currentPosition)
                            {
                                if (switching)
                                {
                                    slot[i].texture = panelRoundSwapSel;
                                }
                                else
                                {
                                    slot[i].texture = panelRoundFaintSel;
                                }
                            }
                            else
                            {
                                slot[i].texture = panelRoundFaint;
                            }
                        }
                        else
                        {
                            if (i == currentPosition)
                            {
                                if (switching)
                                {
                                    slot[i].texture = panelRectSwapSel;
                                }
                                else
                                {
                                    slot[i].texture = panelRectFaintSel;
                                }
                            }
                            else
                            {
                                slot[i].texture = panelRectFaint;
                            }
                        }
                    }
                    else
                    {
                        if (i == 0)
                        {
                            if (i == currentPosition)
                            {
                                if (switching)
                                {
                                    slot[i].texture = panelRoundSwapSel;
                                }
                                else
                                {
                                    slot[i].texture = panelRoundSel;
                                }
                            }
                            else
                            {
                                slot[i].texture = panelRound;
                            }
                        }
                        else
                        {
                            if (i == currentPosition)
                            {
                                if (switching)
                                {
                                    slot[i].texture = panelRectSwapSel;
                                }
                                else
                                {
                                    slot[i].texture = panelRectSel;
                                }
                            }
                            else
                            {
                                slot[i].texture = panelRect;
                            }
                        }
                    }
                }
                if (i == currentPosition)
                {
                    selectBall[i].texture = selectBallOpen;
                }
                else
                {
                    selectBall[i].texture = selectBallClosed;
                }
            }
        }
        if (currentPosition == 6)
        {
            cancel.texture = cancelHighlightTex;
        }
        else
        {
            cancel.texture = cancelTex;
        }
    }

    private IEnumerator switchPokemon(int position1, int position2)
    {
        float increment = 0;
        float moveSpeed = 0.4f;
        Transform slot1 = slot[position1].transform;
        Transform slot2 = slot[position2].transform;
        if (position1 != position2 && position1 >= 0 && position2 >= 0 && position1 < 6 && position2 < 6)
        {
            while (increment < 1)
            {
                increment += (1 / moveSpeed) * Time.deltaTime;
                if (increment > 1)
                {
                    increment = 1;
                }
                if (position1 % 2 == 0)
                {
                    //left side
                    slot1.position = (new Vector3(-0.5f * increment, 0, slot1.position.z));
                }
                else
                {
                    //right side
                    slot1.position = (new Vector3(0.5f * increment, 0, slot1.position.z));
                }
                if (position2 % 2 == 0)
                {
                    //left side
                    slot2.position = (new Vector3(-0.5f * increment, 0, slot2.position.z));
                }
                else
                {
                    //right side
                    slot2.position = (new Vector3(0.5f * increment, 0, slot2.position.z));
                }
                yield return null;
            }

            SaveData.currentSave.PC.swapPokemon(0, position1, 0, position2);
            updateParty();

            increment = 0;
            while (increment < 1)
            {
                increment += (1 / moveSpeed) * Time.deltaTime;
                if (increment > 1)
                {
                    increment = 1;
                }
                if (position1 % 2 == 0)
                {
                    //left side
                    slot1.position = (new Vector3(-0.5f + (0.5f * increment), 0, slot1.position.z));
                }
                else
                {
                    //right side
                    slot1.position = (new Vector3(0.5f - (0.5f * increment), 0, slot1.position.z));
                }
                if (position2 % 2 == 0)
                {
                    //left side
                    slot2.position = (new Vector3(-0.5f + (0.5f * increment), 0, slot2.position.z));
                }
                else
                {
                    //right side
                    slot2.position = (new Vector3(0.5f - (0.5f * increment), 0, slot2.position.z));
                }
                yield return null;
            }
        }
    }

    private IEnumerator animateIcons()
    {
        while (running)
        {
            icon[0].border = new RectOffset(32, 0, 0, 0);
            icon[1].border = new RectOffset(32, 0, 0, 0);
            icon[2].border = new RectOffset(32, 0, 0, 0);
            icon[3].border = new RectOffset(32, 0, 0, 0);
            icon[4].border = new RectOffset(32, 0, 0, 0);
            icon[5].border = new RectOffset(32, 0, 0, 0);
            yield return new WaitForSeconds(0.15f);
            icon[0].border = new RectOffset(32, 0, 0, 0);
            icon[1].border = new RectOffset(32, 0, 0, 0);
            icon[2].border = new RectOffset(32, 0, 0, 0);
            icon[3].border = new RectOffset(32, 0, 0, 0);
            icon[4].border = new RectOffset(32, 0, 0, 0);
            icon[5].border = new RectOffset(32, 0, 0, 0);
            if (currentPosition < 6)
            {
                icon[currentPosition].border = new RectOffset(0, 32, 0, 0);
            }
            yield return new WaitForSeconds(0.15f);

            icon[0].border = new RectOffset(0, 32, 0, 0);
            icon[1].border = new RectOffset(0, 32, 0, 0);
            icon[2].border = new RectOffset(0, 32, 0, 0);
            icon[3].border = new RectOffset(0, 32, 0, 0);
            icon[4].border = new RectOffset(0, 32, 0, 0);
            icon[5].border = new RectOffset(0, 32, 0, 0);
            if (currentPosition < 6)
            {
                icon[currentPosition].border = new RectOffset(32, 0, 0, 0);
            }
            yield return new WaitForSeconds(0.15f);
            icon[0].border = new RectOffset(0, 32, 0, 0);
            icon[1].border = new RectOffset(0, 32, 0, 0);
            icon[2].border = new RectOffset(0, 32, 0, 0);
            icon[3].border = new RectOffset(0, 32, 0, 0);
            icon[4].border = new RectOffset(0, 32, 0, 0);
            icon[5].border = new RectOffset(0, 32, 0, 0);
            yield return new WaitForSeconds(0.15f);
        }
    }


    public IEnumerator control()
    {
        //sceneTransition.FadeIn();
        StartCoroutine(ScreenFade.main.Fade(true, ScreenFade.defaultSpeed));

        running = true;
        switching = false;
        swapPosition = -1;
        currentPosition = 0;
        SaveData.currentSave.PC.packParty();
        updateParty();
        updateFrames();
        Dialog.drawDialogBox();
        Dialog.drawTextInstant("Choose a Pokémon.");
        StartCoroutine("animateIcons");
        while (running)
        {
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                if (currentPosition < 6)
                {
                    shiftPosition(1);
                    SfxHandler.Play(selectClip);
                    yield return new WaitForSeconds(0.2f);
                }
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                if (currentPosition > 0)
                {
                    shiftPosition(-1);
                    SfxHandler.Play(selectClip);
                    yield return new WaitForSeconds(0.2f);
                }
            }
            else if (Input.GetAxisRaw("Vertical") > 0)
            {
                if (currentPosition > 0)
                {
                    if (currentPosition == 6)
                    {
                        shiftPosition(-1);
                    }
                    else
                    {
                        shiftPosition(-2);
                    }
                    SfxHandler.Play(selectClip);
                    yield return new WaitForSeconds(0.2f);
                }
            }
            else if (Input.GetAxisRaw("Vertical") < 0)
            {
                if (currentPosition < 6)
                {
                    shiftPosition(2);
                    SfxHandler.Play(selectClip);
                    yield return new WaitForSeconds(0.2f);
                }
            }
            else if (Input.GetButton("Select"))
            {
                if (currentPosition == 6)
                {
                    if (switching)
                    {
                        switching = false;
                        swapPosition = -1;
                        updateFrames();
                        Dialog.undrawChoiceBox();
                        Dialog.drawDialogBox();
                        Dialog.drawTextInstant("Choose a Pokémon.");
                        yield return new WaitForSeconds(0.2f);
                    }
                    else
                    {
                        SfxHandler.Play(selectClip);
                        running = false;
                    }
                }
                else if (switching)
                {
                    if (currentPosition == swapPosition)
                    {
                        switching = false;
                        swapPosition = -1;
                        updateFrames();
                        Dialog.undrawChoiceBox();
                        Dialog.drawDialogBox();
                        Dialog.drawTextInstant("Choose a Pokémon.");
                        yield return new WaitForSeconds(0.2f);
                    }
                    else
                    {
                        yield return StartCoroutine(switchPokemon(swapPosition, currentPosition));
                        switching = false;
                        swapPosition = -1;
                        updateFrames();
                        Dialog.undrawChoiceBox();
                        Dialog.drawDialogBox();
                        Dialog.drawTextInstant("Choose a Pokémon.");
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                else
                {
                    Pokemon selectedPokemon = SaveData.currentSave.PC.boxes[0][currentPosition];
                    int chosenIndex = -1;
                    while (chosenIndex != 0)
                    {
                        string[] choices = new string[]
                        {
                            "Summary", "Switch", "Item", "Cancel"
                        };
                        chosenIndex = -1;

                        SfxHandler.Play(selectClip);

                        Dialog.drawDialogBox();
                        Dialog.drawTextInstant("Do what with " + selectedPokemon.getName() + "?");
                        Dialog.drawChoiceBox(choices);
                        yield return new WaitForSeconds(0.2f);
                        yield return StartCoroutine(Dialog.choiceNavigate(choices));
                        chosenIndex = Dialog.chosenIndex;
                        if (chosenIndex == 3)
                        {
                            //Summary
                            SfxHandler.Play(selectClip);
                            //yield return new WaitForSeconds(sceneTransition.FadeOut(0.4f));
                            yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));

                            //Set SceneSummary to be active so that it appears
                            Scene.main.Summary.gameObject.SetActive(true);
                            StartCoroutine(Scene.main.Summary.control(SaveData.currentSave.PC.boxes[0], currentPosition));
                            //Start an empty loop that will only stop when SceneSummary is no longer active (is closed)
                            while (Scene.main.Summary.gameObject.activeSelf)
                            {
                                yield return null;
                            }
                            chosenIndex = 0;
                            Dialog.undrawChoiceBox();
                            Dialog.drawDialogBox();
                            Dialog.drawTextInstant("Choose a Pokémon.");
                            //yield return new WaitForSeconds(sceneTransition.FadeIn(0.4f));
                            yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));
                        }
                        else if (chosenIndex == 2)
                        {
                            //Switch
                            switching = true;
                            swapPosition = currentPosition;
                            updateFrames();
                            chosenIndex = 0;
                            Dialog.undrawChoiceBox();
                            Dialog.drawDialogBox();
                            Dialog.drawTextInstant("Move " + selectedPokemon.getName() + " to where?");
                            yield return new WaitForSeconds(0.2f);
                        }
                        else if (chosenIndex == 1)
                        {
                            //Item
                            Dialog.undrawChoiceBox();
                            Dialog.drawDialogBox();
                            if (!string.IsNullOrEmpty(selectedPokemon.getHeldItem()))
                            {
                                yield return
                                    StartCoroutine(
                                        Dialog.drawText(selectedPokemon.getName() + " is holding " +
                                                        selectedPokemon.getHeldItem() + "."));
                                choices = new string[]
                                {
                                    "Swap", "Take", "Cancel"
                                };
                                chosenIndex = -1;
                                Dialog.drawChoiceBox(choices);
                                yield return new WaitForSeconds(0.2f);
                                yield return StartCoroutine(Dialog.choiceNavigate(choices));

                                chosenIndex = Dialog.chosenIndex;

                                if (chosenIndex == 2)
                                {
                                    //Swap
                                    SfxHandler.Play(selectClip);
                                    //yield return new WaitForSeconds(sceneTransition.FadeOut(0.4f));
                                    yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));

                                    Scene.main.Bag.gameObject.SetActive(true);
                                    StartCoroutine(Scene.main.Bag.control(false, true));
                                    while (Scene.main.Bag.gameObject.activeSelf)
                                    {
                                        yield return null;
                                    }

                                    string chosenItem = Scene.main.Bag.chosenItem;

                                    Dialog.undrawChoiceBox();
                                    Dialog.drawDialogBox();
                                    if (string.IsNullOrEmpty(chosenItem))
                                    {
                                        Dialog.drawTextInstant("Choose a Pokémon.");
                                    }
                                    //yield return new WaitForSeconds(sceneTransition.FadeIn(0.4f));
                                    yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));

                                    if (!string.IsNullOrEmpty(chosenItem))
                                    {
                                        Dialog.drawDialogBox();
                                        yield return
                                            StartCoroutine(
                                                Dialog.drawText("Swap " + selectedPokemon.getHeldItem() + " for " +
                                                                chosenItem + "?"));
                                        Dialog.drawChoiceBox();
                                        yield return StartCoroutine(Dialog.choiceNavigate());

                                        chosenIndex = Dialog.chosenIndex;
                                        Dialog.undrawChoiceBox();

                                        if (chosenIndex == 1)
                                        {
                                            string receivedItem = selectedPokemon.swapHeldItem(chosenItem);
                                            SaveData.currentSave.Bag.addItem(receivedItem, 1);
                                            SaveData.currentSave.Bag.removeItem(chosenItem, 1);

                                            Dialog.drawDialogBox();
                                            yield return
                                                Dialog.StartCoroutine("drawText",
                                                    "Gave " + chosenItem + " to " + selectedPokemon.getName() + ",");
                                            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                                            {
                                                yield return null;
                                            }
                                            Dialog.drawDialogBox();
                                            yield return
                                                Dialog.StartCoroutine("drawText",
                                                    "and received " + receivedItem + " in return.");
                                            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                                            {
                                                yield return null;
                                            }
                                        }
                                    }
                                }
                                else if (chosenIndex == 1)
                                {
                                    //Take
                                    Dialog.undrawChoiceBox();
                                    string receivedItem = selectedPokemon.swapHeldItem("");
                                    SaveData.currentSave.Bag.addItem(receivedItem, 1);

                                    updateParty();
                                    updateFrames();

                                    Dialog.drawDialogBox();
                                    yield return
                                        StartCoroutine(
                                            Dialog.drawText("Took " + receivedItem + " from " +
                                                            selectedPokemon.getName() + "."));
                                    while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                                    {
                                        yield return null;
                                    }
                                }
                            }
                            else
                            {
                                yield return
                                    StartCoroutine(
                                        Dialog.drawText(selectedPokemon.getName() + " isn't holding anything."));
                                choices = new string[]
                                {
                                    "Give", "Cancel"
                                };
                                chosenIndex = -1;
                                Dialog.drawChoiceBox(choices);
                                yield return new WaitForSeconds(0.2f);
                                yield return StartCoroutine(Dialog.choiceNavigate(choices));
                                chosenIndex = Dialog.chosenIndex;

                                if (chosenIndex == 1)
                                {
                                    //Give
                                    SfxHandler.Play(selectClip);
                                    //yield return new WaitForSeconds(sceneTransition.FadeOut(0.4f));
                                    yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));

                                    Scene.main.Bag.gameObject.SetActive(true);
                                    StartCoroutine(Scene.main.Bag.control(false, true));
                                    while (Scene.main.Bag.gameObject.activeSelf)
                                    {
                                        yield return null;
                                    }

                                    string chosenItem = Scene.main.Bag.chosenItem;

                                    Dialog.undrawChoiceBox();
                                    Dialog.drawDialogBox();
                                    if (string.IsNullOrEmpty(chosenItem))
                                    {
                                        Dialog.drawTextInstant("Choose a Pokémon.");
                                    }
                                    //yield return new WaitForSeconds(sceneTransition.FadeIn(0.4f));
                                    yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));

                                    if (!string.IsNullOrEmpty(chosenItem))
                                    {
                                        selectedPokemon.swapHeldItem(chosenItem);
                                        SaveData.currentSave.Bag.removeItem(chosenItem, 1);

                                        updateParty();
                                        updateFrames();

                                        Dialog.drawDialogBox();
                                        yield return
                                            Dialog.StartCoroutine("drawText",
                                                "Gave " + chosenItem + " to " + selectedPokemon.getName() + ".");
                                        while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                                        {
                                            yield return null;
                                        }
                                    }
                                }
                            }


                            chosenIndex = 0;
                            yield return new WaitForSeconds(0.2f);
                        }
                    }
                    if (!switching)
                    {
                        Dialog.undrawChoiceBox();
                        Dialog.drawDialogBox();
                        Dialog.drawTextInstant("Choose a Pokémon.");
                    }
                }
            }
            else if (Input.GetButton("Back"))
            {
                if (switching)
                {
                    switching = false;
                    swapPosition = -1;
                    updateFrames();
                    Dialog.undrawChoiceBox();
                    Dialog.drawDialogBox();
                    Dialog.drawTextInstant("Choose a Pokémon.");
                    yield return new WaitForSeconds(0.2f);
                }
                else
                {
                    currentPosition = 6;
                    updateFrames();
                    SfxHandler.Play(selectClip);
                    running = false;
                }
            }
            yield return null;
        }
        StopCoroutine("animateIcons");
        //yield return new WaitForSeconds(sceneTransition.FadeOut());
        yield return StartCoroutine(ScreenFade.main.Fade(false, ScreenFade.defaultSpeed));
        GlobalVariables.global.resetFollower();
        this.gameObject.SetActive(false);
    }
}