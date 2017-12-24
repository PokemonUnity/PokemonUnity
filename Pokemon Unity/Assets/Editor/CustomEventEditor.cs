using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(CustomEvent)), CanEditMultipleObjects]
public class CustomEventEditor : Editor
{
    //Event Trees Arrays
    public SerializedProperty
        interact_Prop,
        bump_Prop;

    void OnEnable()
    {
        // Setup the SerializedProperties
        interact_Prop = serializedObject.FindProperty("interactEventTrees");
        bump_Prop = serializedObject.FindProperty("bumpEventTrees");
    }

    //Event Trees
    private SerializedProperty
        events_Prop,
        treeName_Prop;

    //Event Variables
    private SerializedProperty
        eventType_Prop,
        dir_Prop,
        logic_Prop,
        int0_Prop,
        float0_Prop,
        string0_Prop,
        bool0_Prop,
        bool1_Prop,
        ints_Prop,
        strings_Prop,
        object0_Prop,
        object1_Prop,
        sound_Prop,
        runSimul_Prop;

    private bool interactUnfold = false;
    private bool[] interactTreesUnfold = new bool[1];

    private bool[][] interactEventsUnfold = new bool[][]
    {
        new bool[1], new bool[1], new bool[1], new bool[1], new bool[1], new bool[1],
        new bool[1], new bool[1], new bool[1], new bool[1], new bool[1], new bool[1]
    };

    private bool bumpUnfold = false;
    private bool[] bumpTreesUnfold = new bool[1];

    private bool[][] bumpEventsUnfold = new bool[][]
    {
        new bool[1], new bool[1], new bool[1], new bool[1], new bool[1], new bool[1],
        new bool[1], new bool[1], new bool[1], new bool[1], new bool[1], new bool[1]
    };


    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.indentLevel = 0;

        interactUnfold = EditorGUILayout.Foldout(interactUnfold,
            new GUIContent("Interact Event Trees (" + interact_Prop.arraySize + ")"));
        if (interactUnfold)
        {
            EditorGUI.indentLevel = 1;
            interact_Prop.arraySize = EditorGUILayout.IntField(new GUIContent("Number of Trees"),
                interact_Prop.arraySize);
            if (interact_Prop.arraySize > 12)
            {
                //set max trees at 12
                interact_Prop.arraySize = 12;
            }
            interactTreesUnfold = BoolArrayMatchToProp(interactTreesUnfold, interact_Prop);

            //Draw each tree either folded or unfolded
            int arrayInitialLength = interact_Prop.arraySize;
            for (int tNo = 0; tNo < arrayInitialLength && tNo < interact_Prop.arraySize; tNo++)
            {
                EditorGUI.indentLevel = 2;
                //Set the Current Serialized Object to the relative one in the list
                SerializedProperty currentSTree = interact_Prop.GetArrayElementAtIndex(tNo);

                treeName_Prop = currentSTree.FindPropertyRelative("treeName");
                events_Prop = currentSTree.FindPropertyRelative("events");

                //Get current tree's name
                string treeName = (tNo == 0)
                    ? "T0: Default (" + events_Prop.arraySize + ")"
                    : "T" + tNo + " (" + events_Prop.arraySize + ")";
                if (tNo > 0 && treeName_Prop.stringValue != null)
                {
                    if (treeName_Prop.stringValue.Length > 0)
                    {
                        treeName = "T" + tNo + ": " + treeName_Prop.stringValue + " (" + events_Prop.arraySize + ")";
                    }
                }

                //Display a foldout for the current tree
                interactTreesUnfold[tNo] = EditorGUILayout.Foldout(interactTreesUnfold[tNo], new GUIContent(treeName));
                if (interactTreesUnfold[tNo])
                {
                    EditorGUI.indentLevel = 3;
                    if (tNo > 0)
                    {
                        treeName_Prop.stringValue = EditorGUILayout.TextField(new GUIContent("Name of Tree"),
                            treeName_Prop.stringValue);
                    }
                    if (treeName_Prop.stringValue == "Default")
                    {
                        treeName_Prop.stringValue = null;
                    }

                    events_Prop.arraySize = EditorGUILayout.IntField(new GUIContent("Number of Events"),
                        events_Prop.arraySize);
                    interactEventsUnfold[tNo] = BoolArrayMatchToProp(interactEventsUnfold[tNo], events_Prop);

                    EditorGUILayout.Space();

                    //Draw each event either folded or unfolded
                    int eventArrayInitialLength = events_Prop.arraySize;
                    for (int eNo = 0; eNo < eventArrayInitialLength && eNo < events_Prop.arraySize; eNo++)
                    {
                        //Set the Current Serialized Object to the relative one in the list
                        SerializedProperty currentSEvent = events_Prop.GetArrayElementAtIndex(eNo);

                        //Display a foldout for the current event
                        interactEventsUnfold[tNo][eNo] = EditorGUILayout.Foldout(interactEventsUnfold[tNo][eNo],
                            new GUIContent("#" + (eNo + 1) + ": " + GetEventDescription(currentSEvent)));
                        if (interactEventsUnfold[tNo][eNo])
                        {
                            DrawUnfoldedEventButtons(events_Prop, interactEventsUnfold[tNo], eNo);
                            UpdateUnfoldedEventData(currentSEvent);
                        }
                    }
                }

                EditorGUILayout.Space();
            }
        }

        EditorGUILayout.Space();
        /* Draw a line */
        GUILayout.Box("", new GUILayoutOption[] {GUILayout.ExpandWidth(true), GUILayout.Height(1)});

        EditorGUI.indentLevel = 0;

        bumpUnfold = EditorGUILayout.Foldout(bumpUnfold,
            new GUIContent("Bump Event Trees (" + bump_Prop.arraySize + ")"));

        if (bumpUnfold)
        {
            EditorGUI.indentLevel = 1;
            bump_Prop.arraySize = EditorGUILayout.IntField(new GUIContent("Number of Trees"), bump_Prop.arraySize);
            if (bump_Prop.arraySize > 12)
            {
                //set max trees at 12
                bump_Prop.arraySize = 12;
            }
            bumpTreesUnfold = BoolArrayMatchToProp(bumpTreesUnfold, bump_Prop);

            //Draw each tree either folded or unfolded
            int arrayInitialLength = bump_Prop.arraySize;
            for (int tNo = 0; tNo < arrayInitialLength && tNo < bump_Prop.arraySize; tNo++)
            {
                EditorGUI.indentLevel = 2;
                //Set the Current Serialized Object to the relative one in the list
                SerializedProperty currentSTree = bump_Prop.GetArrayElementAtIndex(tNo);

                treeName_Prop = currentSTree.FindPropertyRelative("treeName");
                events_Prop = currentSTree.FindPropertyRelative("events");

                //Get current tree's name
                string treeName = (tNo == 0) ? "Default (0)" : "Tree " + (tNo);
                if (tNo > 0 && treeName_Prop.stringValue != null)
                {
                    if (treeName_Prop.stringValue.Length > 0)
                    {
                        treeName = treeName_Prop.stringValue + " (" + tNo + ")";
                    }
                }

                //Display a foldout for the current tree
                bumpTreesUnfold[tNo] = EditorGUILayout.Foldout(bumpTreesUnfold[tNo], new GUIContent(treeName));
                if (bumpTreesUnfold[tNo])
                {
                    EditorGUI.indentLevel = 3;
                    if (tNo > 0)
                    {
                        treeName_Prop.stringValue = EditorGUILayout.TextField(new GUIContent("Name of Tree"),
                            treeName_Prop.stringValue);
                    }
                    if (treeName_Prop.stringValue == "Default")
                    {
                        treeName_Prop.stringValue = null;
                    }

                    events_Prop.arraySize = EditorGUILayout.IntField(new GUIContent("Number of Events"),
                        events_Prop.arraySize);
                    bumpEventsUnfold[tNo] = BoolArrayMatchToProp(bumpEventsUnfold[tNo], events_Prop);

                    EditorGUILayout.Space();

                    //Draw each event either folded or unfolded
                    int eventArrayInitialLength = events_Prop.arraySize;
                    for (int eNo = 0; eNo < eventArrayInitialLength && eNo < events_Prop.arraySize; eNo++)
                    {
                        //Set the Current Serialized Object to the relative one in the list
                        SerializedProperty currentSEvent = events_Prop.GetArrayElementAtIndex(eNo);

                        //Display a foldout for the current event
                        bumpEventsUnfold[tNo][eNo] = EditorGUILayout.Foldout(bumpEventsUnfold[tNo][eNo],
                            new GUIContent("#" + (eNo + 1) + ": " + GetEventDescription(currentSEvent)));
                        if (bumpEventsUnfold[tNo][eNo])
                        {
                            DrawUnfoldedEventButtons(events_Prop, bumpEventsUnfold[tNo], eNo);
                            UpdateUnfoldedEventData(currentSEvent);
                        }
                    }
                }

                EditorGUILayout.Space();
            }
        }

        EditorGUILayout.Space();
        /* Draw a line */
        GUILayout.Box("", new GUILayoutOption[] {GUILayout.ExpandWidth(true), GUILayout.Height(1)});

        serializedObject.ApplyModifiedProperties();
    }


    private bool[] BoolArrayMatchToProp(bool[] inoutArray, SerializedProperty matchArray)
    {
        if (inoutArray.Length < matchArray.arraySize)
        {
            bool[] newArray = new bool[matchArray.arraySize];
            for (int i = 0; i < inoutArray.Length; i++)
            {
                newArray[i] = inoutArray[i];
            }
            inoutArray = newArray;
        }
        return inoutArray;
    }

    private bool[][] BoolArrayMatchToProp(bool[][] inoutArray, SerializedProperty matchArray)
    {
        if (inoutArray.Length < matchArray.arraySize)
        {
            bool[][] newArray = new bool[matchArray.arraySize][];
            for (int i = 0; i < inoutArray.Length; i++)
            {
                newArray[i] = inoutArray[i];
            }
            inoutArray = newArray;
        }
        return inoutArray;
    }

    private void SetEventProps(SerializedProperty currentSEvent)
    {
        // Setup the SerializedProperties
        eventType_Prop = currentSEvent.FindPropertyRelative("eventType");
        dir_Prop = currentSEvent.FindPropertyRelative("dir");
        logic_Prop = currentSEvent.FindPropertyRelative("logic");
        int0_Prop = currentSEvent.FindPropertyRelative("int0");
        float0_Prop = currentSEvent.FindPropertyRelative("float0");
        string0_Prop = currentSEvent.FindPropertyRelative("string0");
        bool0_Prop = currentSEvent.FindPropertyRelative("bool0");
        bool1_Prop = currentSEvent.FindPropertyRelative("bool1");
        ints_Prop = currentSEvent.FindPropertyRelative("ints");
        strings_Prop = currentSEvent.FindPropertyRelative("strings");
        object0_Prop = currentSEvent.FindPropertyRelative("object0");
        object1_Prop = currentSEvent.FindPropertyRelative("object1");
        sound_Prop = currentSEvent.FindPropertyRelative("sound");
        runSimul_Prop = currentSEvent.FindPropertyRelative("runSimultaneously");
    }

    private string GetEventDescription(SerializedProperty currentSEvent)
    {
        SetEventProps(currentSEvent);

        //add more details according to which type of event it is
        CustomEventDetails.CustomEventType ty = (CustomEventDetails.CustomEventType) eventType_Prop.enumValueIndex;

        //set the event description to be the enum name
        string eventDescription = ty.ToString();

        switch (ty)
        {
            case CustomEventDetails.CustomEventType.Walk:
                eventDescription = (bool0_Prop.boolValue) ? "Move " : "Walk ";
                eventDescription += (object0_Prop.objectReferenceValue != null)
                    ? "\"" + object0_Prop.objectReferenceValue.name + "\" "
                    : "\"null\" ";
                eventDescription += dir_Prop.enumDisplayNames[dir_Prop.enumValueIndex].ToLowerInvariant() + " " +
                                    int0_Prop.intValue + " spaces.";
                break;

            case CustomEventDetails.CustomEventType.TurnTo:
                int turns = int0_Prop.intValue;
                while (turns > 3)
                {
                    turns -= 4;
                }
                while (turns < 0)
                {
                    turns += 4;
                }
                eventDescription = (object0_Prop.objectReferenceValue != null)
                    ? "Turn \"" + object0_Prop.objectReferenceValue.name + "\""
                    : "Turn \"null\"";
                if (object1_Prop.objectReferenceValue != null)
                {
                    eventDescription += " towards \"" + object1_Prop.objectReferenceValue.name + "\"";
                    if (turns != 0)
                    {
                        eventDescription += ", then";
                    }
                }
                switch (turns)
                {
                    case 1:
                        eventDescription += " clockwise.";
                        break;
                    case 2:
                        eventDescription += " around.";
                        break;
                    case 3:
                        eventDescription += " counter clockwise.";
                        break;
                }
                break;

            case CustomEventDetails.CustomEventType.Wait:
                eventDescription = "Wait " + float0_Prop.floatValue + " seconds.";
                break;

            case CustomEventDetails.CustomEventType.Dialog:
                if (strings_Prop.arraySize > 0)
                {
                    //check for invalid values before attempting to use any.
                    if (strings_Prop.GetArrayElementAtIndex(0).stringValue != null)
                    {
                        if (strings_Prop.GetArrayElementAtIndex(0).stringValue.Length <= 32)
                        {
                            eventDescription = "\"" + strings_Prop.GetArrayElementAtIndex(0).stringValue + "\"";
                        }
                        else
                        {
                            eventDescription = "\"" +
                                               strings_Prop.GetArrayElementAtIndex(0).stringValue.Substring(0, 32) +
                                               "... \"";
                        }
                    }
                }
                break;

            case CustomEventDetails.CustomEventType.Choice:
                eventDescription = "Choices: ";
                int i = 0;
                while (eventDescription.Length < 32 && i < strings_Prop.arraySize)
                {
                    if (strings_Prop.GetArrayElementAtIndex(i).stringValue != null)
                    {
                        eventDescription += strings_Prop.GetArrayElementAtIndex(i).stringValue;
                        if (i + 1 < strings_Prop.arraySize)
                        {
                            eventDescription += ", ";
                        }
                    }
                    i += 1;
                }
                if (eventDescription.Length > 32)
                {
                    eventDescription = eventDescription.Substring(0, 32) + "...";
                }
                break;

            case CustomEventDetails.CustomEventType.ReceivePokemon:
                eventDescription = "Receive a Lv. " + ints_Prop.GetArrayElementAtIndex(1).intValue + " \"";
                PokemonData pkd = PokemonDatabase.getPokemon(ints_Prop.GetArrayElementAtIndex(0).intValue);
                eventDescription += (pkd != null) ? pkd.getName() : "null";
                eventDescription += "\" or Jump to " + int0_Prop.intValue + ".";
                break;

            case CustomEventDetails.CustomEventType.LogicCheck:
                CustomEventDetails.Logic lo = (CustomEventDetails.Logic) logic_Prop.enumValueIndex;

                if (lo == CustomEventDetails.Logic.CVariableEquals)
                {
                    eventDescription = "If \"" + string0_Prop.stringValue + "\" == " + float0_Prop.floatValue +
                                       ", Jump to " + int0_Prop.intValue + ".";
                }
                else if (lo == CustomEventDetails.Logic.CVariableGreaterThan)
                {
                    eventDescription = "If \"" + string0_Prop.stringValue + "\" > " + float0_Prop.floatValue +
                                       ", Jump to " + int0_Prop.intValue + ".";
                }
                else if (lo == CustomEventDetails.Logic.CVariableLessThan)
                {
                    eventDescription = "If \"" + string0_Prop.stringValue + "\" < " + float0_Prop.floatValue +
                                       ", Jump to " + int0_Prop.intValue + ".";
                }
                //Boolean Logic Checks
                else if (lo == CustomEventDetails.Logic.SpaceInParty)
                {
                    eventDescription = (bool0_Prop.boolValue) ? "If NOT " : "If ";
                    eventDescription += logic_Prop.enumDisplayNames[logic_Prop.enumValueIndex] + ", Jump to " +
                                        int0_Prop.intValue + ".";
                }
                else
                {
                    eventDescription = "If " + float0_Prop.floatValue + " " +
                                       logic_Prop.enumDisplayNames[logic_Prop.enumValueIndex] + ", Jump to " +
                                       int0_Prop.intValue + ".";
                }
                break;

            case CustomEventDetails.CustomEventType.SetCVariable:
                eventDescription = "Set C Variable \"" + string0_Prop.stringValue + "\" to " + float0_Prop.floatValue +
                                   ".";
                break;

            case CustomEventDetails.CustomEventType.SetActive:
                eventDescription = (bool0_Prop.boolValue) ? "Activate \"" : "Deactivate \"";
                eventDescription += (object0_Prop.objectReferenceValue != null)
                    ? object0_Prop.objectReferenceValue.name + "\""
                    : "null\"";
                break;

            case CustomEventDetails.CustomEventType.Sound:
                eventDescription = "Play sound: \"";
                eventDescription += (sound_Prop.objectReferenceValue != null)
                    ? sound_Prop.objectReferenceValue.name + "\""
                    : "null\"";
                break;

            case CustomEventDetails.CustomEventType.ReceiveItem:
                eventDescription = "Receive " + int0_Prop.intValue + " x \"" + string0_Prop.stringValue + "\"";
                break;

            case CustomEventDetails.CustomEventType.TrainerBattle:
                eventDescription = "Battle with ";
                eventDescription += (object0_Prop.objectReferenceValue != null)
                    ? object0_Prop.objectReferenceValue.name + "\""
                    : "null\"";
                if (bool0_Prop.boolValue)
                {
                    eventDescription += ", Jump To " + int0_Prop.intValue + " on Loss";
                }
                break;
        }


        return eventDescription;
    }

    private void UpdateUnfoldedEventData(SerializedProperty currentSEvent)
    {
        SetEventProps(currentSEvent);

        /* Draw a line */
        GUILayout.Box("", new GUILayoutOption[] {GUILayout.ExpandWidth(true), GUILayout.Height(1)});

        EditorGUILayout.PropertyField(eventType_Prop);

        EditorGUILayout.Space();

        CustomEventDetails.CustomEventType ty = (CustomEventDetails.CustomEventType) eventType_Prop.enumValueIndex;

        switch (ty)
        {
            case CustomEventDetails.CustomEventType.Dialog:
                strings_Prop.arraySize = EditorGUILayout.IntField(new GUIContent("Lines"), strings_Prop.arraySize);
                EditorGUILayout.Space();
                for (int i = 0; i < strings_Prop.arraySize; i++)
                {
                    strings_Prop.GetArrayElementAtIndex(i).stringValue =
                        EditorGUILayout.TextField(new GUIContent("Dialog " + (i + 1)),
                            strings_Prop.GetArrayElementAtIndex(i).stringValue);
                }
                break;

            case CustomEventDetails.CustomEventType.Choice:
                strings_Prop.arraySize = EditorGUILayout.IntField(new GUIContent("Choices"), strings_Prop.arraySize);
                ints_Prop.arraySize = strings_Prop.arraySize;
                EditorGUILayout.Space();
                for (int i = 0; i < strings_Prop.arraySize; i++)
                {
                    strings_Prop.GetArrayElementAtIndex(i).stringValue =
                        EditorGUILayout.TextField(new GUIContent("Choice " + (i + 1)),
                            strings_Prop.GetArrayElementAtIndex(i).stringValue);
                    ints_Prop.GetArrayElementAtIndex(i).intValue =
                        EditorGUILayout.IntField(new GUIContent("Jump to Tree:"),
                            ints_Prop.GetArrayElementAtIndex(i).intValue);
                }
                break;

            case CustomEventDetails.CustomEventType.Walk:
                EditorGUILayout.PropertyField(runSimul_Prop, new GUIContent("Run Simultaneously"));
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(object0_Prop, new GUIContent("Character"));
                EditorGUILayout.PropertyField(dir_Prop, new GUIContent("Direction"));
                EditorGUILayout.PropertyField(int0_Prop, new GUIContent("Steps"));
                EditorGUILayout.PropertyField(bool0_Prop, new GUIContent("Lock Direction"));

                EditorGUILayout.PropertyField(float0_Prop, new GUIContent("Speed Multiplier"));
                break;

            case CustomEventDetails.CustomEventType.TurnTo:
                EditorGUILayout.PropertyField(object0_Prop, new GUIContent("Character"));
                EditorGUILayout.PropertyField(object1_Prop, new GUIContent("Turn Towards"));
                int0_Prop.intValue = EditorGUILayout.IntField(new GUIContent("Direction Mod"), int0_Prop.intValue);
                break;

            case CustomEventDetails.CustomEventType.Wait:
                EditorGUILayout.PropertyField(float0_Prop, new GUIContent("Seconds"));
                break;

            case CustomEventDetails.CustomEventType.LogicCheck:
                CustomEventDetails.Logic lo = (CustomEventDetails.Logic) logic_Prop.enumValueIndex;

                //Boolean Logic Checks
                if (lo == CustomEventDetails.Logic.SpaceInParty)
                {
                    EditorGUILayout.PropertyField(bool0_Prop, new GUIContent("NOT"));
                }
                else
                {
                    EditorGUILayout.PropertyField(float0_Prop, new GUIContent("Check Value:"));
                }

                EditorGUILayout.PropertyField(logic_Prop, new GUIContent("Computation"));

                //CVariable Logic Checks
                if (lo == CustomEventDetails.Logic.CVariableEquals ||
                    lo == CustomEventDetails.Logic.CVariableGreaterThan ||
                    lo == CustomEventDetails.Logic.CVariableLessThan)
                {
                    EditorGUILayout.PropertyField(string0_Prop, new GUIContent("C Variable"));
                }

                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(int0_Prop, new GUIContent("Jump to Tree:"));
                break;

            case CustomEventDetails.CustomEventType.SetCVariable:
                EditorGUILayout.PropertyField(string0_Prop, new GUIContent("Set Variable:"));
                EditorGUILayout.PropertyField(float0_Prop, new GUIContent("To Value:"));
                break;

            case CustomEventDetails.CustomEventType.Sound:
                EditorGUILayout.PropertyField(sound_Prop, new GUIContent("Sound"));
                break;

            case CustomEventDetails.CustomEventType.SetActive:
                EditorGUILayout.PropertyField(object0_Prop, new GUIContent("Game Object"));
                EditorGUILayout.PropertyField(bool0_Prop, new GUIContent("Set Active"));
                break;

            case CustomEventDetails.CustomEventType.ReceiveItem:
                EditorGUILayout.PropertyField(bool0_Prop, new GUIContent("Is TM"));
                string0_Prop.stringValue = EditorGUILayout.TextField(new GUIContent("Item"), string0_Prop.stringValue);
                if (!bool0_Prop.boolValue)
                {
                    int0_Prop.intValue = EditorGUILayout.IntField(new GUIContent("Quantity"), int0_Prop.intValue);
                }
                break;

            case CustomEventDetails.CustomEventType.ReceivePokemon:
                ints_Prop.arraySize = 11;
                strings_Prop.arraySize = 8;

                int0_Prop.intValue = EditorGUILayout.IntField(new GUIContent("Jump To on Fail"), int0_Prop.intValue);
                EditorGUILayout.Space();

                ints_Prop.GetArrayElementAtIndex(0).intValue = EditorGUILayout.IntField(new GUIContent("Pokemon ID"),
                    ints_Prop.GetArrayElementAtIndex(0).intValue);
                PokemonData pkd = PokemonDatabase.getPokemon(ints_Prop.GetArrayElementAtIndex(0).intValue);
                string pokemonName = (pkd != null) ? pkd.getName() : "null";
                EditorGUILayout.LabelField(new GUIContent(" "), new GUIContent(pokemonName));
                EditorGUILayout.Space();
                strings_Prop.GetArrayElementAtIndex(0).stringValue =
                    EditorGUILayout.TextField(new GUIContent("Nickname"),
                        strings_Prop.GetArrayElementAtIndex(0).stringValue);
                ints_Prop.GetArrayElementAtIndex(1).intValue = EditorGUILayout.IntSlider(new GUIContent("Level"),
                    ints_Prop.GetArrayElementAtIndex(1).intValue, 1, 100);
                //Gender
                if (pkd != null)
                {
                    if (pkd.getMaleRatio() == -1)
                    {
                        EditorGUILayout.LabelField(new GUIContent("Gender"), new GUIContent("Genderless"));
                    }
                    else if (pkd.getMaleRatio() == 0)
                    {
                        EditorGUILayout.LabelField(new GUIContent("Gender"), new GUIContent("Female"));
                    }
                    else if (pkd.getMaleRatio() == 100)
                    {
                        EditorGUILayout.LabelField(new GUIContent("Gender"), new GUIContent("Male"));
                    }
                    else
                    {
//if not a set gender
                        ints_Prop.GetArrayElementAtIndex(2).intValue = EditorGUILayout.Popup(new GUIContent("Gender"),
                            ints_Prop.GetArrayElementAtIndex(2).intValue, new GUIContent[]
                            {
                                new GUIContent("Male"), new GUIContent("Female"), new GUIContent("Calculate")
                            });
                    }
                }
                else
                {
                    EditorGUILayout.LabelField(new GUIContent("Gender"));
                }
                EditorGUILayout.PropertyField(bool0_Prop, new GUIContent("Is Shiny"));
                strings_Prop.GetArrayElementAtIndex(1).stringValue =
                    EditorGUILayout.TextField(new GUIContent("Original Trainer"),
                        strings_Prop.GetArrayElementAtIndex(1).stringValue);
                strings_Prop.GetArrayElementAtIndex(2).stringValue =
                    EditorGUILayout.TextField(new GUIContent("Poké Ball"),
                        strings_Prop.GetArrayElementAtIndex(2).stringValue);
                strings_Prop.GetArrayElementAtIndex(3).stringValue =
                    EditorGUILayout.TextField(new GUIContent("Held Item"),
                        strings_Prop.GetArrayElementAtIndex(3).stringValue);
                //Nature
                string[] natureNames = NatureDatabase.getNatureNames();
                GUIContent[] natures = new GUIContent[natureNames.Length + 1];
                natures[0] = new GUIContent("Random");
                for (int i = 1; i < natures.Length; i++)
                {
                    natures[i] =
                        new GUIContent(natureNames[i - 1].Substring(0, 1) +
                                       natureNames[i - 1].Substring(1, natureNames[i - 1].Length - 1).ToLower() +
                                       "\t | " + NatureDatabase.getNature(i - 1).getUpStat() + "+ | " +
                                       NatureDatabase.getNature(i - 1).getDownStat() + "-");
                }
                ints_Prop.GetArrayElementAtIndex(3).intValue = EditorGUILayout.Popup(new GUIContent("Nature"),
                    ints_Prop.GetArrayElementAtIndex(3).intValue, natures);
                //Ability
                if (pkd != null)
                {
                    ints_Prop.GetArrayElementAtIndex(4).intValue = EditorGUILayout.Popup(new GUIContent("Ability"),
                        ints_Prop.GetArrayElementAtIndex(4).intValue, new GUIContent[]
                        {
                            new GUIContent("1: " + pkd.getAbility(0)), new GUIContent("2: " + pkd.getAbility(1)),
                            new GUIContent("(HA) " + pkd.getAbility(2))
                        });
                }
                else
                {
                    EditorGUILayout.LabelField(new GUIContent("Ability"));
                }

                EditorGUILayout.Space();

                EditorGUILayout.LabelField(new GUIContent("Custom Moveset"), new GUIContent("(Blanks will be default)"));
                EditorGUILayout.BeginHorizontal();
                strings_Prop.GetArrayElementAtIndex(4).stringValue =
                    EditorGUILayout.TextField(strings_Prop.GetArrayElementAtIndex(4).stringValue);
                strings_Prop.GetArrayElementAtIndex(5).stringValue =
                    EditorGUILayout.TextField(strings_Prop.GetArrayElementAtIndex(5).stringValue);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                strings_Prop.GetArrayElementAtIndex(6).stringValue =
                    EditorGUILayout.TextField(strings_Prop.GetArrayElementAtIndex(6).stringValue);
                strings_Prop.GetArrayElementAtIndex(7).stringValue =
                    EditorGUILayout.TextField(strings_Prop.GetArrayElementAtIndex(7).stringValue);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                string IVstring = (bool1_Prop.boolValue) ? "Using Custom IVs" : "Using Random IVs";
                bool1_Prop.boolValue = EditorGUILayout.Foldout(bool1_Prop.boolValue, new GUIContent(IVstring));
                if (bool1_Prop.boolValue)
                {
                    ints_Prop.GetArrayElementAtIndex(5).intValue = EditorGUILayout.IntSlider(new GUIContent("HP"),
                        ints_Prop.GetArrayElementAtIndex(5).intValue, 0, 31);
                    ints_Prop.GetArrayElementAtIndex(6).intValue = EditorGUILayout.IntSlider(new GUIContent("ATK"),
                        ints_Prop.GetArrayElementAtIndex(6).intValue, 0, 31);
                    ints_Prop.GetArrayElementAtIndex(7).intValue = EditorGUILayout.IntSlider(new GUIContent("DEF"),
                        ints_Prop.GetArrayElementAtIndex(7).intValue, 0, 31);
                    ints_Prop.GetArrayElementAtIndex(8).intValue = EditorGUILayout.IntSlider(new GUIContent("SPA"),
                        ints_Prop.GetArrayElementAtIndex(8).intValue, 0, 31);
                    ints_Prop.GetArrayElementAtIndex(9).intValue = EditorGUILayout.IntSlider(new GUIContent("SPD"),
                        ints_Prop.GetArrayElementAtIndex(9).intValue, 0, 31);
                    ints_Prop.GetArrayElementAtIndex(10).intValue = EditorGUILayout.IntSlider(new GUIContent("SPE"),
                        ints_Prop.GetArrayElementAtIndex(10).intValue, 0, 31);
                }
                break;

            case CustomEventDetails.CustomEventType.TrainerBattle:
                EditorGUILayout.PropertyField(object0_Prop, new GUIContent("Trainer Script"));
                EditorGUILayout.PropertyField(bool0_Prop, new GUIContent("Loss Allowed?"));
                if (bool0_Prop.boolValue)
                {
                    int0_Prop.intValue = EditorGUILayout.IntField(new GUIContent("Jump To on Loss"), int0_Prop.intValue);
                }
                break;
        }

        /* Draw a line */
        GUILayout.Box("", new GUILayoutOption[] {GUILayout.ExpandWidth(true), GUILayout.Height(1)});
        EditorGUILayout.Space();
    }

    private void DrawUnfoldedEventButtons(SerializedProperty array_Prop, bool[] arrayUnfold, int eventNo)
    {
        //Draw/Use buttons for reorganising/adding events
        Rect hRect = EditorGUILayout.BeginHorizontal();
        if (GUI.Button(new Rect(hRect.width - 64, hRect.y - 6, 24, 16), new GUIContent("▲")))
        {
            if (eventNo > 0)
            {
                array_Prop.MoveArrayElement(eventNo, eventNo - 1);
                bool temp = arrayUnfold[eventNo];
                arrayUnfold[eventNo] = arrayUnfold[eventNo - 1];
                arrayUnfold[eventNo - 1] = temp;
            }
        }
        if (GUI.Button(new Rect(hRect.width - 40, hRect.y - 6, 24, 16), new GUIContent("▼")))
        {
            if (eventNo < array_Prop.arraySize - 1)
            {
                array_Prop.MoveArrayElement(eventNo, eventNo + 1);
                bool temp = arrayUnfold[eventNo];
                arrayUnfold[eventNo] = arrayUnfold[eventNo + 1];
                arrayUnfold[eventNo + 1] = temp;
            }
        }
        if (GUI.Button(new Rect(hRect.width - 16, hRect.y - 6, 24, 16), new GUIContent("+")))
        {
            array_Prop.InsertArrayElementAtIndex(eventNo);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
    }
}