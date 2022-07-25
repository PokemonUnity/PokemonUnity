//Original Scripts by IIColour (IIColour_Spectrum)

using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class CustomEvent : MonoBehaviour
{
    public CustomEventDetails.Logic Logic;
    public float CValue;
    public string CVar;
    
    public CustomEventTree[] interactEventTrees;
    public CustomEventTree[] bumpEventTrees;

    private DialogBoxHandlerNew Dialog;

    private NPCHandler thisNPCHandler;
    private bool deactivateOnFinish = false;

    private int eventTreeIndex = 0;
    private int currentEventIndex = 0;

    private bool isCameraDefaultPos = true;
    private Vector3 cameraDefaultPos;

    void Awake()
    {
        if (transform.GetComponent<NPCHandler>() != null)
        {
            thisNPCHandler = transform.GetComponent<NPCHandler>();
        }
    }

    private void Start()
    {
        Dialog = GameObject.Find("GUI").GetComponent<DialogBoxHandlerNew>();
    }

    public void CVariableToggle()
    {
        Debug.Log(gameObject.name);
        if (CVariablePredicate())
        {
            gameObject.SetActive(false);
        }
    }
    
    public bool CVariablePredicate()
    {
        if (CVar.Length > 0)
        {
            switch (Logic)
            {
                case CustomEventDetails.Logic.CVariableEquals:
                    if (SaveData.currentSave.getCVariable(CVar) == CValue)
                        return true;
                    break;
                case CustomEventDetails.Logic.CVariableGreaterThan:
                    if (SaveData.currentSave.getCVariable(CVar) > CValue)
                        return true;
                    break;
                case CustomEventDetails.Logic.CVariableLessThan:
                    if (SaveData.currentSave.getCVariable(CVar) < CValue)
                        return true;
                    break;
                case CustomEventDetails.Logic.GymBadgeNoOwned:
                    
                    Debug.Log("Testing Gym Badge Logic "+Mathf.FloorToInt(CValue));
                    
                    if (Mathf.FloorToInt(CValue) < SaveData.currentSave.gymsBeaten.Length &&
                        Mathf.FloorToInt(CValue) >= 0)
                    {
                        Debug.Log("It is "+SaveData.currentSave.gymsBeaten[Mathf.FloorToInt(CValue)]);
                        //ensure input number is valid
                        if (SaveData.currentSave.gymsBeaten[Mathf.FloorToInt(CValue)])
                        {
                            return true;
                        }
                    }
                    break;
                case CustomEventDetails.Logic.GymBadgesEarned:
                    //TODO add GymBadgesEarned Logic
                    break;
                case CustomEventDetails.Logic.PokemonIDIsInParty:
                    //TODO add PokemonIDIsInParty Logic
                    break;
                case CustomEventDetails.Logic.SpaceInParty:
                    //TODO add SpaceInParty Logic
                    break;
                case CustomEventDetails.Logic.IsMale:
                    return SaveData.currentSave.isMale;
                    break;
                case CustomEventDetails.Logic.GymBadgeNoNotOwned:
                    
                    Debug.Log("Testing Gym Badge Logic");
                    
                    if (Mathf.FloorToInt(CValue) < SaveData.currentSave.gymsBeaten.Length &&
                        Mathf.FloorToInt(CValue) >= 0)
                    {
                        Debug.Log("It is "+SaveData.currentSave.gymsBeaten[Mathf.FloorToInt(CValue)]);
                        //ensure input number is valid
                        if (!SaveData.currentSave.gymsBeaten[Mathf.FloorToInt(CValue)])
                        {
                            return true;
                        }
                    }
                    break;
            }
        }

        return false;
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
        if (!CVariablePredicate())
        {
            yield return StartCoroutine(runEventTrees(interactEventTrees));
        }
    }

    private IEnumerator bump()
    {
        if (PlayerMovement.player.busyWith == null && !CVariablePredicate())
            yield return StartCoroutine(runEventTrees(bumpEventTrees));
    }

    private IEnumerator runEventTrees(CustomEventTree[] treesArray)
    {
        if (treesArray.Length > 0)
        {
            eventTreeIndex = 0;
            if (PlayerMovement.player.setCheckBusyWith(gameObject))
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
                bool isNPC = true;
                PlayerMovement target = null;
                
                if (currentEvent.object0.GetComponent<NPCHandler>() != null)
                {
                    targetNPC = currentEvent.object0.GetComponent<NPCHandler>();
                }
                else if (currentEvent.object0.GetComponent<PlayerMovement>() != null)
                {
                    target = currentEvent.object0.GetComponent<PlayerMovement>();
                    isNPC = false;
                }
                if (targetNPC != null || target != null)
                {
                    if (isNPC)
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
                    else
                    {
                        if (currentEvent.object1 != null)
                        {
                            //calculate target objects's position relative to this objects's and set direction accordingly.
                            xDistance = target.hitBox.position.x - currentEvent.object1.transform.position.x;
                            zDistance = target.hitBox.position.z - currentEvent.object1.transform.position.z;
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
                            target.updateDirection(direction);
                        }
                        if (currentEvent.int0 != 0)
                        {
                            direction = target.direction + currentEvent.int0;
                            while (direction > 3)
                            {
                                direction -= 4;
                            }
                            while (direction < 0)
                            {
                                direction += 4;
                            }
                            target.updateDirection(direction);
                        }
                    }
                }
                break;

            case (CustomEventDetails.CustomEventType.Dialog):

                string name;
                
                switch (Language.getLang())
                {
                    case Language.Country.FRANCAIS:
                        name = currentEvent.fr_name;
                        break;
                    default:
                        name = currentEvent.en_name;
                        break;
                }

                string[] dialog;

                if (currentEvent.strings.Length == 0)
                {
                    switch (Language.getLang())
                    {
                        case Language.Country.FRANCAIS:
                            dialog = currentEvent.fr_dialog;
                            break;
                        default:
                            dialog = currentEvent.en_dialog;
                            break;
                    }
                }
                else
                {
                    dialog = currentEvent.strings;
                }
                
                
                for (int i = 0; i < dialog.Length; i++)
                {
                    switch (currentEvent.dialogFrame)
                    {
                        default:
                            Dialog.DrawDialogBox();
                            break;
                        case CustomEventDetails.DialogFrame.BlackFrame:
                            Dialog.DrawBlackFrame();
                            break;
                        case CustomEventDetails.DialogFrame.ScreamFrame:
                            SfxHandler.Play(currentEvent.sound);
                            Dialog.DrawScreamFrame();
                            break;
                    }
                    
                    if (!String.IsNullOrEmpty(name))
                    {
                        Dialog.DrawNameBox(name);
                    }

                    if (currentEvent.dialogFrame == CustomEventDetails.DialogFrame.ScreamFrame)
                    {
                        yield return StartCoroutine(Dialog.DrawTextSilent(dialog[i]));
                    }
                    else
                    {
                        yield return StartCoroutine(Dialog.DrawText(dialog[i]));
                    }
                    

                    if (i < dialog.Length - 1)
                    {
                        while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back"))
                        {
                            yield return null;
                        }
                    }
                }
                if (nextEvent != null)
                {
                    if (nextEvent.eventType != CustomEventDetails.CustomEventType.Choice)
                    {
                        while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back"))
                        {
                            yield return null;
                        }
                        if (!EventRequiresDialogBox(nextEvent.eventType))
                        {
                            Dialog.UndrawDialogBox();
                            Dialog.UndrawNameBox();
                        } // do not undraw the box if the next event needs it
                    }
                }
                else
                {
                    while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back"))
                    {
                        yield return null;
                    }
                    Dialog.UndrawDialogBox();
                    Dialog.UndrawNameBox();
                }
                break;

            case (CustomEventDetails.CustomEventType.Choice):

                string[] strings = new string[0]; //PokemonUnity.Game._INTL(currentEvent.en_dialog);
                //Language.getLang() switch
                //{
                //    Language.Country.ENGLISH => currentEvent.en_dialog,
                //    _ => currentEvent.fr_dialog
                //};

                if (strings.Length > 1)
                {
                    yield return StartCoroutine(Dialog.DrawChoiceBox(strings));
                }
                else if (currentEvent.fr_dialog.Length > 1)
                {
                    yield return StartCoroutine(Dialog.DrawChoiceBox(currentEvent.fr_dialog));
                }
                else
                {
                    yield return StartCoroutine(Dialog.DrawChoiceBox());
                }
                int chosenIndex = Dialog.chosenIndex;
                chosenIndex = currentEvent.ints.Length - 1 - chosenIndex; //flip it to reflect the original input
                Dialog.UndrawChoiceBox();
                Dialog.UndrawDialogBox();
                Dialog.UndrawNameBox();
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
                if (currentEvent.string0.Length > 0)
                {
                    if (currentEvent.string0 == "cry")
                    {
                        SfxHandler.Play(SaveData.currentSave.PC.boxes[0][0].GetCry());
                    }
                }
                else if (currentEvent.bool0)
                {
                    BgmHandler.main.PlayMFX(currentEvent.sound);
                }
                else
                {
                    SfxHandler.Play(currentEvent.sound);
                }
                break;

            case CustomEventDetails.CustomEventType.ReceiveItem:
                //Play Good for TM, Average for Item
                AudioClip itemGetMFX = (currentEvent.bool0)
                    ? Resources.Load<AudioClip>("Audio/mfx/GetGood")
                    : Resources.Load<AudioClip>("Audio/mfx/GetDecent");
                BgmHandler.main.PlayMFX(itemGetMFX);

                string firstLetter = currentEvent.string0.Substring(0, 1).ToLowerInvariant();
                Dialog.DrawDialogBox();
                if (currentEvent.bool0)
                {
                    Dialog.StartCoroutine(Dialog.DrawText(
                        SaveData.currentSave.playerName + " received TM" +
                        ItemDatabase.getItem(currentEvent.string0).getTMNo() + ": " + currentEvent.string0 + "!"));
                }
                else
                {
                    if (currentEvent.int0 > 1)
                    {
                        Dialog.StartCoroutine(Dialog.DrawText(
                            SaveData.currentSave.playerName + " received " + currentEvent.string0 + "s!"));
                    }
                    else if (firstLetter == "a" || firstLetter == "e" || firstLetter == "i" || firstLetter == "o" ||
                             firstLetter == "u")
                    {
                        Dialog.StartCoroutine(Dialog.DrawText(
                            SaveData.currentSave.playerName + " received an " + currentEvent.string0 + "!"));
                    }
                    else
                    {
                        Dialog.StartCoroutine(Dialog.DrawText(
                            SaveData.currentSave.playerName + " received a " + currentEvent.string0 + "!"));
                    }
                }
                yield return new WaitForSeconds(itemGetMFX.length);

                bool itemAdd = SaveData.currentSave.Bag.addItem(currentEvent.string0, currentEvent.int0);

                Dialog.DrawDialogBox();
                if (itemAdd)
                {
                    if (currentEvent.bool0)
                    {
                        yield return
                            Dialog.StartCoroutine(Dialog.DrawTextSilent(
                                SaveData.currentSave.playerName + " put the TM" +
                                ItemDatabase.getItem(currentEvent.string0).getTMNo() + " \\away into the bag."));
                    }
                    else
                    {
                        if (currentEvent.int0 > 1)
                        {
                            yield return
                                Dialog.StartCoroutine(Dialog.DrawTextSilent(
                                    SaveData.currentSave.playerName + " put the " + currentEvent.string0 +
                                    "s \\away into the bag."));
                        }
                        else
                        {
                            yield return
                                Dialog.StartCoroutine(Dialog.DrawTextSilent(
                                    SaveData.currentSave.playerName + " put the " + currentEvent.string0 +
                                    " \\away into the bag."));
                        }
                    }
                    while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back"))
                    {
                        yield return null;
                    }
                }
                else
                {
                    yield return Dialog.StartCoroutine(Dialog.DrawTextSilent( "But there was no room..."));
                    while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back"))
                    {
                        yield return null;
                    }
                }
                Dialog.UndrawDialogBox();
                break;

            case CustomEventDetails.CustomEventType.ReceivePokemon:
                /*if (SaveData.currentSave.PC.hasSpace(0))
                {
                    //Play Great for Pokemon
                    AudioClip pokeGetMFX = Resources.Load<AudioClip>("Audio/mfx/GetGreat");

                    PokemonData pkd = PokemonDatabase.getPokemon(currentEvent.ints[0]);

                    string pkName = pkd.Name;
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

                    Dialog.DrawDialogBox();

                    string text;

                    switch (Language.getLang())
                    {
                        default:
                            if (currentEvent.string0.Length > 0)
                            {
                                text = SaveData.currentSave.playerName + " caught a " + pkName + "!";
                            }
                            else
                            {
                                text = SaveData.currentSave.playerName + " received the " + pkName + "!";
                            }
                            break;
                        case Language.Country.FRANCAIS:
                            if (currentEvent.string0.Length > 0)
                            {
                                text = SaveData.currentSave.playerName + " a capturé un " + pkName + "!";
                            }
                            else
                            {
                                text = SaveData.currentSave.playerName + " a obtenu un " + pkName + "!";
                            }
                            break;
                    }
                    
                    yield return
                        Dialog.StartCoroutine(Dialog.DrawText(text));
                    BgmHandler.main.PlayMFX(pokeGetMFX);
                    yield return new WaitForSeconds(pokeGetMFX.length);

                    string nickname = currentEvent.strings[0];
                    if (currentEvent.strings[1].Length == 0)
                    {
                        //If no OT set, allow nicknaming of Pokemon

                        Dialog.DrawDialogBox();
                        
                        switch (Language.getLang())
                        {
                            default:
                                if (currentEvent.string0.Length > 0)
                                {
                                    text = "Would you like to give a nickname to \nthe " + pkName +
                                           " you caught?";
                                }
                                else
                                {
                                    text = "Would you like to give a nickname to \nthe " + pkName +
                                           " you received?";
                                }
                                break;
                            case Language.Country.FRANCAIS:
                                if (currentEvent.string0.Length > 0)
                                {
                                    text = "Voulez-vous renommer\nle " + pkName +
                                           " capturé ?";
                                }
                                else
                                {
                                    text = "Voulez-vous renommer\nle " + pkName +
                                           " obtenu ?";
                                }
                                break;
                        }
                        
                        yield return
                            StartCoroutine(
                                Dialog.DrawTextSilent(text));
                        yield return StartCoroutine(Dialog.DrawChoiceBox());
                        int nicknameCI = Dialog.chosenIndex;
                        Dialog.UndrawDialogBox();
                        Dialog.UndrawChoiceBox();

                        if (nicknameCI == 1)
                        {
                            //give nickname
                            //SfxHandler.Play(selectClip);
                            yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));

                            Scene.main.Typing.gameObject.SetActive(true);
                            StartCoroutine(Scene.main.Typing.control(10, "", pkGender,
                                Pokemon.GetIconsSpriteFromID(currentEvent.ints[0], currentEvent.bool0)));
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
                    if (nextEvent != null)
                        if (!EventRequiresDialogBox(nextEvent.eventType))
                        {
                            Dialog.UndrawDialogBox();
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

                    string pkNature = (currentEvent.ints[3] <= 0)
                        ? NatureDatabase.getRandomNature().Name
                        : NatureDatabase.getNature(currentEvent.ints[3] - 1).Name;

                    string[] pkMoveset = pkd.GenerateMoveset(currentEvent.ints[1]);
                    for (int i = 0; i < 4; i++)
                    {
                        if (currentEvent.strings.Length < 5) break;
                        if (currentEvent.strings[4 + i].Length > 0)
                        {
                            pkMoveset[i] = currentEvent.strings[4 + i];
                        }
                    }

                    Debug.Log(pkMoveset[0] + ", " + pkMoveset[1] + ", " + pkMoveset[2] + ", " + pkMoveset[3]);

                    string heldItem = "";
                    if (currentEvent.strings.Length > 3)
                    {
                        heldItem = currentEvent.strings[3];
                    }

                    PokemonEssentials.Interface.PokeBattle.IPokemon pk = new Pokemon(currentEvent.ints[0], nickname, pkGender, currentEvent.ints[1],
                        currentEvent.bool0, currentEvent.strings[2], heldItem,
                        currentEvent.strings[1], IVs[0], IVs[1], IVs[2], IVs[3], IVs[4], IVs[5], 0, 0, 0, 0, 0, 0,
                        pkNature, currentEvent.ints[4],
                        pkMoveset, new int[4]);

                    SaveData.currentSave.PC.addPokemon(pk);
                }
                else
                {
                    //jump to new tree
                    JumpToTree(currentEvent.int0);
                }*/
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
                                if ((int)PokemonUnity.Game.GameData.Trainer.party[pi].Species ==
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
                    case CustomEventDetails.Logic.IsMale:
                        passedCheck = SaveData.currentSave.isMale;
                        break;
                    case CustomEventDetails.Logic.Following:
                        passedCheck = PlayerMovement.player.npcFollower == currentEvent.object0.GetComponent<NPCFollower>();
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
                Trainer trainer = currentEvent.object0.GetComponent<Trainer>();
                
                //Automatic LoopStart usage not yet implemented
                Scene.main.Battle.gameObject.SetActive(true);
                
                if (trainer.battleBGM != null)
                {
                    Debug.Log(trainer.battleBGM.name);
                    Scene.main.Battle.battleBGM = trainer.battleBGM;
                    Scene.main.Battle.battleBGMLoopStart = trainer.samplesLoopStart;
                    BgmHandler.main.PlayOverlay(trainer.battleBGM, trainer.samplesLoopStart);
                }
                else
                {
                    Scene.main.Battle.battleBGM = Scene.main.Battle.defaultTrainerBGM;
                    Scene.main.Battle.battleBGMLoopStart = Scene.main.Battle.defaultTrainerBGMLoopStart;
                    BgmHandler.main.PlayOverlay(Scene.main.Battle.defaultTrainerBGM,
                        Scene.main.Battle.defaultTrainerBGMLoopStart);
                }
                Scene.main.Battle.gameObject.SetActive(false);
                
                //custom cutouts not yet implemented
                if (trainer.trainerClass == Trainer.Class.Champion)
                {
                    GlobalVariables.global.transform.Find("GUI/VsScreen").gameObject.SetActive(true);
                    yield return StartCoroutine(GlobalVariables.global.transform.Find("GUI/VsScreen")
                        .GetComponent<DefaultTransition>().animate());
                }
                else
                {
                    StartCoroutine(ScreenFade.main.FadeCutout(false, ScreenFade.slowedSpeed, null));
                    yield return new WaitForSeconds(1.6f);
                }


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
            case CustomEventDetails.CustomEventType.PlayMusicTheme:
                BgmHandler.main.PlayOverlay(currentEvent.sound, currentEvent.sampleLoopStart);
                break;
            
            case CustomEventDetails.CustomEventType.ResumeBGM:
                BgmHandler.main.ForceResumeMain(0.4f, PlayerMovement.player.accessedMapSettings.mapBGMClip, PlayerMovement.player.accessedMapSettings.mapBGMLoopStartSamples);
                break;
            
            case CustomEventDetails.CustomEventType.StopBGM:
                BgmHandler.main.PlayMain(null, 0);
                //BgmHandler.main.PlayOverlay(null, 0);
                break;
            case CustomEventDetails.CustomEventType.Emote:
                switch (currentEvent.string0)
                {
                    default:
                        if (currentEvent.sound != null) SfxHandler.Play(currentEvent.sound);
                    yield return StartCoroutine( exclaimAnimation(currentEvent.object0) );
                    break;
                }
                break;
            case CustomEventDetails.CustomEventType.ForcePauseInput:
                PlayerMovement.player.canInput = false;
                break;
            case CustomEventDetails.CustomEventType.Jump:
                if (currentEvent.object0.GetComponent<NPCHandler>() != null)
                {
                    yield return StartCoroutine(currentEvent.object0.GetComponent<NPCHandler>().jump(currentEvent.float0, currentEvent.sound, currentEvent.bool0));
                }
                else if (currentEvent.object0.GetComponent<PlayerMovement>() != null)
                {
                    //TODO Player jump
                }
                break;
            case CustomEventDetails.CustomEventType.ShakeCamera:
                yield return StartCoroutine(PlayerMovement.player.shakeCamera(currentEvent.int0, currentEvent.float0));
                break;
            case CustomEventDetails.CustomEventType.AlignHorizontal:
                if (currentEvent.object0.GetComponent<NPCHandler>() != null)
                {
                    targetNPC = currentEvent.object0.GetComponent<NPCHandler>();

                    int initialDirection = targetNPC.direction;
                    targetNPC.direction =
                        (int) currentEvent.object0.transform.position.z < currentEvent.object1.transform.position.z
                            ? 0 : 2;

                    int iteration = (int) Math.Abs(Math.Round(currentEvent.object0.transform.position.z - currentEvent.object1.transform.position.z));
                    
                    
                    
                    for (int i = 0; i < iteration; i++)
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

                    int dir = currentEvent.object0.transform.position.z < currentEvent.object1.transform.position.z
                        ? 0 : 2;
                    
                    PlayerMovement.player.direction = dir;

                    int iteration = (int) Math.Abs(Math.Round(currentEvent.object0.transform.position.z - currentEvent.object1.transform.position.z));

                    PlayerMovement.player.speed = (currentEvent.float0 > 0)
                        ? PlayerMovement.player.walkSpeed / currentEvent.float0
                        : PlayerMovement.player.walkSpeed;
                    
                    if (currentEvent.object0.transform.position.z != currentEvent.object1.transform.position.z)
                        for (int i = 0; i < iteration; i++)
                        {
                            PlayerMovement.player.updateDirection(dir);
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
            case CustomEventDetails.CustomEventType.AlignVertical:
                if (currentEvent.object0.GetComponent<NPCHandler>() != null)
                {
                    targetNPC = currentEvent.object0.GetComponent<NPCHandler>();

                    int initialDirection = targetNPC.direction;
                    targetNPC.direction =
                        (int) currentEvent.object0.transform.position.x < currentEvent.object1.transform.position.x
                            ? 1 : 3;

                    int iteration = (int) Math.Abs(Math.Round(currentEvent.object0.transform.position.x - currentEvent.object1.transform.position.x));
                    
                    
                    
                    for (int i = 0; i < iteration; i++)
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

                    int dir = currentEvent.object0.transform.position.x < currentEvent.object1.transform.position.x
                        ? 1 : 3;
                    
                    PlayerMovement.player.direction = dir;

                    int iteration = (int) Math.Abs(Math.Round(currentEvent.object0.transform.position.x - currentEvent.object1.transform.position.x));

                    PlayerMovement.player.speed = (currentEvent.float0 > 0)
                        ? PlayerMovement.player.walkSpeed / currentEvent.float0
                        : PlayerMovement.player.walkSpeed;
                    
                    if (currentEvent.object0.transform.position.x != currentEvent.object1.transform.position.x)
                        for (int i = 0; i < iteration; i++)
                        {
                            PlayerMovement.player.updateDirection(dir);
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
            case CustomEventDetails.CustomEventType.MoveCamera:
                yield return StartCoroutine(moveCamera(
                        PlayerMovement.player.transform.Find("Camera") != null ? 
                            PlayerMovement.player.transform.Find("Camera").transform : GameObject.Find("Camera").transform, 
                        new Vector3(currentEvent.ints[0], currentEvent.ints[1], currentEvent.ints[2]), currentEvent.float0));
                break;
            case CustomEventDetails.CustomEventType.ResetCamera:
                yield return StartCoroutine(resetCamera(
                    PlayerMovement.player.transform.Find("Camera") != null ? 
                        PlayerMovement.player.transform.Find("Camera").transform : GameObject.Find("Camera").transform, currentEvent.float0));
                break;
            case CustomEventDetails.CustomEventType.AddPokemon:
                /*if (SaveData.currentSave.PC.hasSpace(0))
                {
                    PokemonData pkd = PokemonDatabase.getPokemon(currentEvent.ints[0]);

                    string pkName = pkd.Name;
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
                    
                    string nickname = currentEvent.strings.Length > 0 ? currentEvent.strings[0] : "";

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
                        ? NatureDatabase.getRandomNature().Name
                        : NatureDatabase.getNature(currentEvent.ints[3] - 1).Name;

                    string[] pkMoveset = pkd.GenerateMoveset(currentEvent.ints[1]);
                    for (int i = 0; i < 4; i++)
                    {
                        if (currentEvent.strings[4 + i].Length > 0)
                        {
                            pkMoveset[i] = currentEvent.strings[4 + i];
                        }
                    }

                    Debug.Log(pkMoveset[0] + ", " + pkMoveset[1] + ", " + pkMoveset[2] + ", " + pkMoveset[3]);


                    PokemonEssentials.Interface.PokeBattle.IPokemon pk = new Pokemon(currentEvent.ints[0], nickname, pkGender, currentEvent.ints[1],
                        currentEvent.bool0, currentEvent.strings[2], currentEvent.strings[3],
                        currentEvent.strings[1], IVs[0], IVs[1], IVs[2], IVs[3], IVs[4], IVs[5], 0, 0, 0, 0, 0, 0,
                        pkNature, currentEvent.ints[4],
                        pkMoveset, new int[4]);

                    SaveData.currentSave.PC.addPokemon(pk);
                }*/
                break;
            case CustomEventDetails.CustomEventType.WithdrawPokemon:
                yield return StartCoroutine(PlayerMovement.player.followerScript.withdrawToBall());
                break;
            case CustomEventDetails.CustomEventType.ReleasePokemon:
                yield return StartCoroutine(PlayerMovement.player.followerScript.releaseFromBall());
                break;
            case CustomEventDetails.CustomEventType.SetRespawn:
                SaveData.currentSave.respawnScenePosition = new SeriV3(new Vector3(currentEvent.ints[0], currentEvent.ints[1], currentEvent.ints[2]));
                SaveData.currentSave.respawnSceneDirection = currentEvent.int0;
                switch (Language.getLang())
                {
                    default: 
                        SaveData.currentSave.respawnText = currentEvent.en_dialog;
                        break;
                    case Language.Country.FRANCAIS: 
                        SaveData.currentSave.respawnText = currentEvent.fr_dialog;
                        break;
                }

                if (currentEvent.string0.Length == 0)
                {
                    SaveData.currentSave.respawnSceneName = Application.loadedLevelName;
                }
                else
                {
                    SaveData.currentSave.respawnSceneName = currentEvent.string0;
                }
                
                break;
            case CustomEventDetails.CustomEventType.SetCameraPosition:
                yield return StartCoroutine(setCameraPosition(
                    PlayerMovement.player.transform.Find("Camera") != null ? 
                        PlayerMovement.player.transform.Find("Camera").transform : GameObject.Find("Camera").transform, 
                    new Vector3(currentEvent.ints[0], currentEvent.ints[1], currentEvent.ints[2]), currentEvent.float0));
                break;
            case CustomEventDetails.CustomEventType.StartCoroutine:
                yield return StartCoroutine(currentEvent.string0);
                break;
            case CustomEventDetails.CustomEventType.GiveBadge:

                if (currentEvent.int0 >= 0 && currentEvent.int0 < SaveData.currentSave.gymsBeaten.Length)
                {
                    SaveData.currentSave.gymsBeaten[currentEvent.int0] = true;
                    SaveData.currentSave.gymsBeatTime[currentEvent.int0] = DateTime.Now.ToString(CultureInfo.InvariantCulture);
                }

                BgmHandler.main.PlayMFX(Resources.Load<AudioClip>("Audio/mfx/getBadge"));
                
                Dialog.DrawBlackFrame();
                StartCoroutine(Dialog.DrawTextSilent("You receive the badge number " + (currentEvent.int0) + "!"));

                yield return new WaitForSeconds(5);
                
                while (!UnityEngine.Input.GetButtonDown("Select") && !UnityEngine.Input.GetButtonDown("Back"))
                {
                    yield return null;
                }
                Dialog.UndrawDialogBox();
                break;
            case CustomEventDetails.CustomEventType.StopOverlayBGM:
                BgmHandler.main.PlayOverlay(null, 0);
                break;
            case CustomEventDetails.CustomEventType.SetNPCFollower:
                if (GlobalVariables.global.followerOut)
                {
                    yield return StartCoroutine(PlayerMovement.player.followerScript.withdrawToBall());
                }

                if (PlayerMovement.player.npcFollower)
                {
                    PlayerMovement.player.npcFollower.hitBox.name = "NPC_Object";
                    PlayerMovement.player.npcFollower.enabled = false;
                    PlayerMovement.player.npcFollower = null;
                }
                
                currentEvent.object0.GetComponent<NPCFollower>().enabled = true;
                currentEvent.object0.GetComponent<NPCFollower>().hitBox.name = "NPC_Transparent";
                PlayerMovement.player.npcFollower = currentEvent.object0.GetComponent<NPCFollower>();
                break;
            case CustomEventDetails.CustomEventType.RemoveNPCFollower:
                PlayerMovement.player.npcFollower.hitBox.name = "NPC_Object";
                PlayerMovement.player.npcFollower.enabled = false;
                PlayerMovement.player.npcFollower = null;
                break;
        }
    }

    private IEnumerator resetCamera(Transform camera, float duration)
    {
        Debug.Log(cameraDefaultPos.ToString());

        LeanTween.move(camera.gameObject, cameraDefaultPos+PlayerMovement.player.transform.position, duration);
        yield return new WaitForSeconds(duration);

        camera.parent = PlayerMovement.player.transform;
        isCameraDefaultPos = true;
    }

    private IEnumerator moveCamera(Transform camera, Vector3 distance, float duration)
    {

        float increment = 0f;

        Vector3 startPosition = camera.position;
        
        if (isCameraDefaultPos)
        {
            cameraDefaultPos = camera.localPosition;
            isCameraDefaultPos = false;
            camera.parent = null;
        }

        LeanTween.move(camera.gameObject, startPosition + distance, duration);
        yield return new WaitForSeconds(duration);
    }

    private IEnumerator setCameraPosition(Transform camera, Vector3 position, float duration)
    {
        if (isCameraDefaultPos)
        {
            cameraDefaultPos = camera.localPosition;
            isCameraDefaultPos = false;
            camera.parent = null;
        }
        
        LeanTween.move(camera.gameObject, cameraDefaultPos+position, duration);
        yield return new WaitForSeconds(duration);
    }

    private IEnumerator exclaimAnimation(GameObject npc)
    {
        float increment = -1f;
        float speed = 0.15f;

        if (npc == null || npc.transform.Find("Exclaim") == null)
        {
            yield return null;
        }
        else
        {
            GameObject exclaim = npc.transform.Find("Exclaim").gameObject;

            exclaim.SetActive(true);

            while (increment < 0.3f)
            {
                increment += (1 / speed) * Time.deltaTime;
                if (increment > 0.3f)
                {
                    increment = 0.3f;
                }
                exclaim.transform.localScale = new Vector3(1, 1.3f + (-1.3f * increment * increment), 1);
                yield return null;
            }

            exclaim.transform.localScale = new Vector3(1, 1, 1);

            yield return new WaitForSeconds(1.2f);
            exclaim.SetActive(false);
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
    
    /* Custom Coroutines */

    private IEnumerator StarterChoice()
    {
        Debug.Log("Starting Starter Cutscene");
        
        yield return StartCoroutine(ScreenFade.main.Fade(false, ScreenFade.slowedSpeed));

        GameObject.Find("Weather").GetComponent<WeatherHandler>().disable = true;
        
        Scene.main.StarterChoice.gameObject.SetActive(true);
        yield return StartCoroutine(Scene.main.StarterChoice.control());
        Scene.main.StarterChoice.gameObject.SetActive(false);
        
        GameObject.Find("Weather").GetComponent<WeatherHandler>().disable = false;
        
        yield return StartCoroutine(ScreenFade.main.Fade(true, ScreenFade.slowedSpeed));
    }

    private IEnumerator HealParty()
    {
        //foreach (PokemonEssentials.Interface.PokeBattle.IPokemon p in SaveData.currentSave.PC.boxes[0])
        //{
        //    if (p == null) break;
        //    p.healFull();
        //}

        yield return null;
    }
    
    private IEnumerator FadeIn()
    {
        Debug.Log("Starting Fade In");
        
        yield return StartCoroutine(ScreenFade.main.Fade(true, ScreenFade.slowedSpeed));
        
    }
    
    private IEnumerator FadeOut()
    {
        Debug.Log("Starting Fade Out");
        
        yield return StartCoroutine(ScreenFade.main.Fade(false, ScreenFade.slowedSpeed));
        
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
        TrainerBattle, //object0: trainer script | bool0: allowed to lose | int0: tree to jump to on loss
        PlayMusicTheme, //Music to play (example: rival's theme) | sound: AudioClip
        ResumeBGM, //Resume the BGM, stops the Music Theme if played before
        StopBGM, //Stop the music played (must be a music, else it will just restart the main BGM)
        Emote, //Activate exclaim emote on object0
        ForcePauseInput,
        Jump, //Make the NPC/Player jump | sound: jump sound | bool0: has landing sound
        ShakeCamera, //float0: duration in seconds | int0: intensity
        MoveCamera, //Moves camera with coordinates (ints[0], ints[1], ints[2]) | float0: speed
        ResetCamera, //Reset camera to its original point | float0: speed
        AlignHorizontal, //Aligns object0 HORIZONTALLY with object1 | object0: npc to move | object1: target position | float0: speed | bool0: lock direction
        AlignVertical, //Aligns object0 VERTICALLY with object1
        AddPokemon,
        WithdrawPokemon,
        ReleasePokemon,
        SetRespawn, //Set respawn point | int0: direction | ints[0-3] position
        SetCameraPosition,
        StartCoroutine,
        GiveBadge,
        StopOverlayBGM,
        SetNPCFollower,
        RemoveNPCFollower
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
        SpaceInParty,
        IsMale,
        GymBadgeNoNotOwned,
        Following
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

    public enum DialogFrame
    {
        WhiteBubble,
        BlackFrame,
        ScreamFrame
    }

    public DialogFrame dialogFrame;
    
    public string en_name;
    public string fr_name;
    
    public string[] en_dialog;
    public string[] fr_dialog;

    public GameObject object0;
    public GameObject object1;

    public AudioClip sound;
    public int sampleLoopStart;

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