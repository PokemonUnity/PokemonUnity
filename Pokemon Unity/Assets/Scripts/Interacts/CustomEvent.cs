//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class CustomEvent : MonoBehaviour
{
    public CustomEventTree[] interactEventTrees;
    public CustomEventTree[] bumpEventTrees;

    private DialogBoxHandler Dialog;

    private NPCHandler thisNPCHandler;
    private bool deactivateOnFinish = false;

    private int eventTreeIndex = 0;
    private int currentEventIndex = 0;


    void Awake()
    {
        if (transform.GetComponent<NPCHandler>() != null)
        {
            thisNPCHandler = transform.GetComponent<NPCHandler>();
        }
        Dialog = GameObject.Find("GUI").GetComponent<DialogBoxHandler>();
    }

    public void setThisNPCHandlerBusy(bool boolValue)
    {
        if (thisNPCHandler != null)
        {
            thisNPCHandler.busy = boolValue;
        }
    }


    private IEnumerator interact()
    {
        yield return StartCoroutine(runEventTrees(interactEventTrees));
    }

    private IEnumerator bump()
    {
        yield return StartCoroutine(runEventTrees(bumpEventTrees));
    }

    private IEnumerator runEventTrees(CustomEventTree[] treesArray)
    {
        if (treesArray.Length > 0)
        {
            eventTreeIndex = 0;
            if (PlayerMovement.player.setCheckBusyWith(this.gameObject))
            {
                setThisNPCHandlerBusy(true);

                for (currentEventIndex = 0;
                    currentEventIndex < treesArray[eventTreeIndex].events.Length;
                    currentEventIndex++)
                {
                    if (!treesArray[eventTreeIndex].events[currentEventIndex].runSimultaneously)
                    {
                        yield return StartCoroutine(runEvent(treesArray, currentEventIndex));
                    }
                    else
                    {
                        StartCoroutine(runEvent(treesArray, currentEventIndex));
                    }
                }

                setThisNPCHandlerBusy(false);
                PlayerMovement.player.unsetCheckBusyWith(this.gameObject);
            }
            if (deactivateOnFinish)
            {
                this.gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator runEvent(CustomEventTree[] treesArray, int index)
    {
        CustomEventDetails currentEvent = treesArray[eventTreeIndex].events[index];
        CustomEventDetails nextEvent = null;
        if (index + 1 < treesArray[eventTreeIndex].events.Length)
        {
            //if not the last event
            nextEvent = treesArray[eventTreeIndex].events[index + 1];
        }

        NPCHandler targetNPC = null;

        CustomEventDetails.CustomEventType ty = currentEvent.eventType;

        Debug.Log("Run event. Type: " + ty.ToString());

        switch (ty)
        {
            case (CustomEventDetails.CustomEventType.Wait):
                yield return new WaitForSeconds(currentEvent.float0);
                break;

            case (CustomEventDetails.CustomEventType.Walk):
                if (currentEvent.object0.GetComponent<NPCHandler>() != null)
                {
                    targetNPC = currentEvent.object0.GetComponent<NPCHandler>();

                    int initialDirection = targetNPC.direction;
                    targetNPC.direction = (int) currentEvent.dir;
                    for (int i = 0; i < currentEvent.int0; i++)
                    {
                        targetNPC.direction = (int) currentEvent.dir;
                        Vector3 forwardsVector = targetNPC.getForwardsVector(true);
                        if (currentEvent.bool0)
                        {
                            //if direction locked in
                            targetNPC.direction = initialDirection;
                        }
                        while (forwardsVector == new Vector3(0, 0, 0))
                        {
                            targetNPC.direction = (int) currentEvent.dir;
                            forwardsVector = targetNPC.getForwardsVector(true);
                            if (currentEvent.bool0)
                            {
                                //if direction locked in
                                targetNPC.direction = initialDirection;
                            }
                            yield return new WaitForSeconds(0.1f);
                        }

                        targetNPC.setOverrideBusy(true);
                        yield return StartCoroutine(targetNPC.move(forwardsVector, currentEvent.float0));
                        targetNPC.setOverrideBusy(false);
                    }
                    targetNPC.setFrameStill();
                } //Move the player if set to player
                if (currentEvent.object0 == PlayerMovement.player.gameObject)
                {
                    int initialDirection = PlayerMovement.player.direction;

                    PlayerMovement.player.speed = (currentEvent.float0 > 0)
                        ? PlayerMovement.player.walkSpeed / currentEvent.float0
                        : PlayerMovement.player.walkSpeed;
                    for (int i = 0; i < currentEvent.int0; i++)
                    {
                        PlayerMovement.player.updateDirection((int) currentEvent.dir);
                        Vector3 forwardsVector = PlayerMovement.player.getForwardVector();
                        if (currentEvent.bool0)
                        {
                            //if direction locked in
                            PlayerMovement.player.updateDirection(initialDirection);
                        }

                        PlayerMovement.player.setOverrideAnimPause(true);
                        yield return
                            StartCoroutine(PlayerMovement.player.move(forwardsVector, false, currentEvent.bool0));
                        PlayerMovement.player.setOverrideAnimPause(false);
                    }
                    PlayerMovement.player.speed = PlayerMovement.player.walkSpeed;
                }
                break;

            case (CustomEventDetails.CustomEventType.TurnTo):
                int direction;
                float xDistance;
                float zDistance;
                if (currentEvent.object0.GetComponent<NPCHandler>() != null)
                {
                    targetNPC = currentEvent.object0.GetComponent<NPCHandler>();
                }
                if (targetNPC != null)
                {
                    if (currentEvent.object1 != null)
                    {
                        //calculate target objects's position relative to this objects's and set direction accordingly.
                        xDistance = targetNPC.hitBox.position.x - currentEvent.object1.transform.position.x;
                        zDistance = targetNPC.hitBox.position.z - currentEvent.object1.transform.position.z;
                        if (xDistance >= Mathf.Abs(zDistance))
                        {
                            //Mathf.Abs() converts zDistance to a positive always.
                            direction = 3;
                        } //this allows for better accuracy when checking orientation.
                        else if (xDistance <= Mathf.Abs(zDistance) * -1)
                        {
                            direction = 1;
                        }
                        else if (zDistance >= Mathf.Abs(xDistance))
                        {
                            direction = 2;
                        }
                        else
                        {
                            direction = 0;
                        }
                        targetNPC.setDirection(direction);
                    }
                    if (currentEvent.int0 != 0)
                    {
                        direction = targetNPC.direction + currentEvent.int0;
                        while (direction > 3)
                        {
                            direction -= 4;
                        }
                        while (direction < 0)
                        {
                            direction += 4;
                        }
                        targetNPC.setDirection(direction);
                    }
                }
                break;

            case (CustomEventDetails.CustomEventType.Dialog):
                for (int i = 0; i < currentEvent.strings.Length; i++)
                {
                    Dialog.drawDialogBox();
                    yield return StartCoroutine(Dialog.drawText(currentEvent.strings[i]));

                    if (i < currentEvent.strings.Length - 1)
                    {
                        while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                        {
                            yield return null;
                        }
                    }
                }
                if (nextEvent != null)
                {
                    if (nextEvent.eventType != CustomEventDetails.CustomEventType.Choice)
                    {
                        while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                        {
                            yield return null;
                        }
                        if (!EventRequiresDialogBox(nextEvent.eventType))
                        {
                            Dialog.undrawDialogBox();
                        } // do not undraw the box if the next event needs it
                    }
                }
                else
                {
                    while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                    {
                        yield return null;
                    }
                    Dialog.undrawDialogBox();
                }
                break;

            case (CustomEventDetails.CustomEventType.Choice):
                if (currentEvent.strings.Length > 1)
                {
                    Dialog.drawChoiceBox(currentEvent.strings);
                    yield return StartCoroutine(Dialog.choiceNavigate(currentEvent.strings));
                }
                else
                {
                    Dialog.drawChoiceBox();
                    yield return StartCoroutine(Dialog.choiceNavigate());
                }
                int chosenIndex = Dialog.chosenIndex;
                chosenIndex = currentEvent.ints.Length - 1 - chosenIndex; //flip it to reflect the original input
                Dialog.undrawChoiceBox();
                Dialog.undrawDialogBox();
                if (chosenIndex < currentEvent.ints.Length)
                {
                    //only change tree if index is valid
                    if (currentEvent.ints[chosenIndex] != eventTreeIndex &&
                        currentEvent.ints[chosenIndex] < treesArray.Length)
                    {
                        JumpToTree(currentEvent.ints[chosenIndex]);
                    }
                }
                break;

            case CustomEventDetails.CustomEventType.Sound:
                SfxHandler.Play(currentEvent.sound);
                break;

            case CustomEventDetails.CustomEventType.ReceiveItem:
                //Play Good for TM, Average for Item
                AudioClip itemGetMFX = (currentEvent.bool0)
                    ? Resources.Load<AudioClip>("Audio/mfx/GetGood")
                    : Resources.Load<AudioClip>("Audio/mfx/GetDecent");
                BgmHandler.main.PlayMFX(itemGetMFX);

                string firstLetter = currentEvent.string0.Substring(0, 1).ToLowerInvariant();
                Dialog.drawDialogBox();
                if (currentEvent.bool0)
                {
                    Dialog.StartCoroutine("drawText",
                        SaveData.currentSave.playerName + " received TM" +
                        ItemDatabase.getItem(currentEvent.string0).getTMNo() + ": " + currentEvent.string0 + "!");
                }
                else
                {
                    if (currentEvent.int0 > 1)
                    {
                        Dialog.StartCoroutine("drawText",
                            SaveData.currentSave.playerName + " received " + currentEvent.string0 + "s!");
                    }
                    else if (firstLetter == "a" || firstLetter == "e" || firstLetter == "i" || firstLetter == "o" ||
                             firstLetter == "u")
                    {
                        Dialog.StartCoroutine("drawText",
                            SaveData.currentSave.playerName + " received an " + currentEvent.string0 + "!");
                    }
                    else
                    {
                        Dialog.StartCoroutine("drawText",
                            SaveData.currentSave.playerName + " received a " + currentEvent.string0 + "!");
                    }
                }
                yield return new WaitForSeconds(itemGetMFX.length);

                bool itemAdd = SaveData.currentSave.Bag.addItem(currentEvent.string0, currentEvent.int0);

                Dialog.drawDialogBox();
                if (itemAdd)
                {
                    if (currentEvent.bool0)
                    {
                        yield return
                            Dialog.StartCoroutine("drawTextSilent",
                                SaveData.currentSave.playerName + " put the TM" +
                                ItemDatabase.getItem(currentEvent.string0).getTMNo() + " \\away into the bag.");
                    }
                    else
                    {
                        if (currentEvent.int0 > 1)
                        {
                            yield return
                                Dialog.StartCoroutine("drawTextSilent",
                                    SaveData.currentSave.playerName + " put the " + currentEvent.string0 +
                                    "s \\away into the bag.");
                        }
                        else
                        {
                            yield return
                                Dialog.StartCoroutine("drawTextSilent",
                                    SaveData.currentSave.playerName + " put the " + currentEvent.string0 +
                                    " \\away into the bag.");
                        }
                    }
                    while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                    {
                        yield return null;
                    }
                }
                else
                {
                    yield return Dialog.StartCoroutine("drawTextSilent", "But there was no room...");
                    while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                    {
                        yield return null;
                    }
                }
                Dialog.undrawDialogBox();
                break;

            case CustomEventDetails.CustomEventType.ReceivePokemon:
                if (SaveData.currentSave.PC.hasSpace(0))
                {
                    //Play Great for Pokemon
                    AudioClip pokeGetMFX = Resources.Load<AudioClip>("Audio/mfx/GetGreat");

                    PokemonData pkd = PokemonDatabase.getPokemon(currentEvent.ints[0]);

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
                        if (currentEvent.ints[2] == 0)
                        {
                            pkGender = Pokemon.Gender.MALE;
                        }
                        else if (currentEvent.ints[2] == 1)
                        {
                            pkGender = Pokemon.Gender.FEMALE;
                        }
                    }

                    Dialog.drawDialogBox();
                    yield return
                        Dialog.StartCoroutine("drawText",
                            SaveData.currentSave.playerName + " received the " + pkName + "!");
                    BgmHandler.main.PlayMFX(pokeGetMFX);
                    yield return new WaitForSeconds(pokeGetMFX.length);

                    string nickname = currentEvent.strings[0];
                    if (currentEvent.strings[1].Length == 0)
                    {
                        //If no OT set, allow nicknaming of Pokemon

                        Dialog.drawDialogBox();
                        yield return
                            StartCoroutine(
                                Dialog.drawTextSilent("Would you like to give a nickname to \nthe " + pkName +
                                                      " you received?"));
                        Dialog.drawChoiceBox();
                        yield return StartCoroutine(Dialog.choiceNavigate());
                        int nicknameCI = Dialog.chosenIndex;
                        Dialog.undrawDialogBox();
                        Dialog.undrawChoiceBox();

                        if (nicknameCI == 1)
                        {
                            //give nickname
                            //SfxHandler.Play(selectClip);
                            yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));

                            Scene.main.Typing.gameObject.SetActive(true);
                            StartCoroutine(Scene.main.Typing.control(10, "", pkGender,
                                Pokemon.GetIconsFromID_(currentEvent.ints[0], currentEvent.bool0)));
                            while (Scene.main.Typing.gameObject.activeSelf)
                            {
                                yield return null;
                            }
                            if (Scene.main.Typing.typedString.Length > 0)
                            {
                                nickname = Scene.main.Typing.typedString;
                            }

                            yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));
                        }
                    }
                    if (!EventRequiresDialogBox(nextEvent.eventType))
                    {
                        Dialog.undrawDialogBox();
                    }

                    int[] IVs = new int[]
                    {
                        Random.Range(0, 32), Random.Range(0, 32), Random.Range(0, 32),
                        Random.Range(0, 32), Random.Range(0, 32), Random.Range(0, 32)
                    };
                    if (currentEvent.bool1)
                    {
                        //if using Custom IVs
                        IVs[0] = currentEvent.ints[5];
                        IVs[1] = currentEvent.ints[6];
                        IVs[2] = currentEvent.ints[7];
                        IVs[3] = currentEvent.ints[8];
                        IVs[4] = currentEvent.ints[9];
                        IVs[5] = currentEvent.ints[10];
                    }

                    string pkNature = (currentEvent.ints[3] == 0)
                        ? NatureDatabase.getRandomNature().getName()
                        : NatureDatabase.getNature(currentEvent.ints[3] - 1).getName();

                    string[] pkMoveset = pkd.GenerateMoveset(currentEvent.ints[1]);
                    for (int i = 0; i < 4; i++)
                    {
                        if (currentEvent.strings[4 + i].Length > 0)
                        {
                            pkMoveset[i] = currentEvent.strings[4 + i];
                        }
                    }

                    Debug.Log(pkMoveset[0] + ", " + pkMoveset[1] + ", " + pkMoveset[2] + ", " + pkMoveset[3]);


                    Pokemon pk = new Pokemon(currentEvent.ints[0], nickname, pkGender, currentEvent.ints[1],
                        currentEvent.bool0, currentEvent.strings[2], currentEvent.strings[3],
                        currentEvent.strings[1], IVs[0], IVs[1], IVs[2], IVs[3], IVs[4], IVs[5], 0, 0, 0, 0, 0, 0,
                        pkNature, currentEvent.ints[4],
                        pkMoveset, new int[4]);

                    SaveData.currentSave.PC.addPokemon(pk);
                }
                else
                {
                    //jump to new tree
                    JumpToTree(currentEvent.int0);
                }
                break;

            case (CustomEventDetails.CustomEventType.SetActive):
                if (currentEvent.bool0)
                {
                    currentEvent.object0.SetActive(true);
                }
                else
                {
                    if (currentEvent.object0 == this.gameObject)
                    {
                        deactivateOnFinish = true;
                    }
                    else if (currentEvent.object0 != PlayerMovement.player.gameObject)
                    {
                        //important to never deactivate the player
                        currentEvent.object0.SetActive(false);
                    }
                }
                break;

            case CustomEventDetails.CustomEventType.SetCVariable:
                SaveData.currentSave.setCVariable(currentEvent.string0, currentEvent.float0);
                break;

            case (CustomEventDetails.CustomEventType.LogicCheck):
                bool passedCheck = false;

                CustomEventDetails.Logic lo = currentEvent.logic;

                switch (lo)
                {
                    case CustomEventDetails.Logic.CVariableEquals:
                        if (currentEvent.float0 == SaveData.currentSave.getCVariable(currentEvent.string0))
                        {
                            passedCheck = true;
                        }
                        break;
                    case CustomEventDetails.Logic.CVariableGreaterThan:
                        if (SaveData.currentSave.getCVariable(currentEvent.string0) > currentEvent.float0)
                        {
                            passedCheck = true;
                        }
                        break;
                    case CustomEventDetails.Logic.CVariableLessThan:
                        if (SaveData.currentSave.getCVariable(currentEvent.string0) < currentEvent.float0)
                        {
                            passedCheck = true;
                        }
                        break;
                    case CustomEventDetails.Logic.GymBadgeNoOwned:
                        if (Mathf.FloorToInt(currentEvent.float0) < SaveData.currentSave.gymsBeaten.Length &&
                            Mathf.FloorToInt(currentEvent.float0) >= 0)
                        {
                            //ensure input number is valid
                            if (SaveData.currentSave.gymsBeaten[Mathf.FloorToInt(currentEvent.float0)])
                            {
                                passedCheck = true;
                            }
                        }
                        break;
                    case CustomEventDetails.Logic.GymBadgesEarned:
                        int badgeCount = 0;
                        for (int bi = 0; bi < SaveData.currentSave.gymsBeaten.Length; bi++)
                        {
                            if (SaveData.currentSave.gymsBeaten[bi])
                            {
                                badgeCount += 1;
                            }
                        }
                        if (badgeCount >= currentEvent.float0)
                        {
                            passedCheck = true;
                        }
                        break;
                    case CustomEventDetails.Logic.PokemonIDIsInParty:
                        for (int pi = 0; pi < 6; pi++)
                        {
                            if (SaveData.currentSave.PC.boxes[0][pi] != null)
                            {
                                if (SaveData.currentSave.PC.boxes[0][pi].getID() ==
                                    Mathf.FloorToInt(currentEvent.float0))
                                {
                                    passedCheck = true;
                                    pi = 6;
                                }
                            }
                        }
                        break;
                    case CustomEventDetails.Logic.SpaceInParty:
                        if (currentEvent.bool0)
                        {
                            if (!SaveData.currentSave.PC.hasSpace(0))
                            {
                                passedCheck = true;
                            }
                        }
                        else
                        {
                            if (SaveData.currentSave.PC.hasSpace(0))
                            {
                                passedCheck = true;
                            }
                        }
                        break;
                }

                if (passedCheck)
                {
                    int newTreeIndex = currentEvent.int0;
                    if (newTreeIndex != eventTreeIndex && //only change tree if index is valid
                        newTreeIndex < treesArray.Length)
                    {
                        JumpToTree(newTreeIndex);
                    }
                }
                break;

            case CustomEventDetails.CustomEventType.TrainerBattle:

                //custom cutouts not yet implemented
                StartCoroutine(ScreenFade.main.FadeCutout(false, ScreenFade.slowedSpeed, null));

                //Automatic LoopStart usage not yet implemented
                Scene.main.Battle.gameObject.SetActive(true);

                Trainer trainer = currentEvent.object0.GetComponent<Trainer>();

                if (trainer.battleBGM != null)
                {
                    Debug.Log(trainer.battleBGM.name);
                    BgmHandler.main.PlayOverlay(trainer.battleBGM, trainer.samplesLoopStart);
                }
                else
                {
                    BgmHandler.main.PlayOverlay(Scene.main.Battle.defaultTrainerBGM,
                        Scene.main.Battle.defaultTrainerBGMLoopStart);
                }
                Scene.main.Battle.gameObject.SetActive(false);
                yield return new WaitForSeconds(1.6f);

                Scene.main.Battle.gameObject.SetActive(true);
                StartCoroutine(Scene.main.Battle.control(true, trainer, currentEvent.bool0));

                while (Scene.main.Battle.gameObject.activeSelf)
                {
                    yield return null;
                }

                //yield return new WaitForSeconds(sceneTransition.FadeIn(0.4f));
                yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));

                if (currentEvent.bool0)
                {
                    if (Scene.main.Battle.victor == 1)
                    {
                        int newTreeIndex = currentEvent.int0;
                        if (newTreeIndex != eventTreeIndex && //only change tree if index is valid
                            newTreeIndex < treesArray.Length)
                        {
                            JumpToTree(newTreeIndex);
                        }
                    }
                }

                break;
        }
    }

    private bool EventRequiresDialogBox(CustomEventDetails.CustomEventType eventType)
    {
        //Events that require immediate use of the DialogBox
        if (eventType == CustomEventDetails.CustomEventType.Dialog ||
            eventType == CustomEventDetails.CustomEventType.Choice ||
            eventType == CustomEventDetails.CustomEventType.ReceiveItem ||
            eventType == CustomEventDetails.CustomEventType.ReceivePokemon)
        {
            return true;
        }
        return false;
    }

    private void JumpToTree(int index)
    {
        eventTreeIndex = index;
        currentEventIndex = -1;
    }
}


[System.Serializable]
public class CustomEventTree
{
    public string treeName;
    public CustomEventDetails[] events;
}

[System.Serializable]
public class CustomEventDetails
{
    public enum CustomEventType
    {
        Wait, //float0: wait time
        Walk, //direction | int0: walk spaces | object0: Character to move | bool0: lock direction
        TurnTo, //int0: direction modifier | object0: NPC to turn | object1: Object to turn to
        Dialog, //strings: lines of dialog
        Choice, //strings: choices (none for Yes/No) | ints: eventTrees to jump to (same for continue)
        Sound, //sound: sound to play
        ReceiveItem,
        ReceivePokemon, //ints[0]: Pokemon ID | ints[1]: Level | ints[2]: Gender | ints[3]: Nature | ints[4]: Ability
        //strings[0]: Nickname | strings[1]: OT | strings[2]: Poké Ball | strings[3]: Held Item
        //ints[5-10]: Custom IVs | strings[4-7]: Custom Moves | bool0: Is Shiny | bool1: Use Custom IVs
        SetActive, //object0: game object to activate
        SetCVariable, //string0: CVariable name | float0: new value
        LogicCheck, //logic | float0: check value | string0: CVariable name
        TrainerBattle //object0: trainer script | bool0: allowed to lose | int0: tree to jump to on loss
    }

    public enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }

    public enum Logic
    {
        CVariableEquals,
        CVariableGreaterThan,
        CVariableLessThan,
        GymBadgeNoOwned,
        GymBadgesEarned,
        PokemonIDIsInParty,
        SpaceInParty
    }

    public CustomEventType eventType;

    public Direction dir;

    public Logic logic;

    public int int0;
    public float float0;
    public string string0;

    public bool bool0;
    public bool bool1;

    public int[] ints;
    public string[] strings;

    public GameObject object0;
    public GameObject object1;

    public AudioClip sound;


    public bool runSimultaneously;
}

[System.Serializable]
public class CVariable
{
    public string name;
    public float value;

    public CVariable(string name, float value)
    {
        this.name = name;
        this.value = value;
    }
}