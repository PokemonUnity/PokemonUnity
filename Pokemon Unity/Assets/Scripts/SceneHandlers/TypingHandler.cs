//Original Scripts by IIColour (IIColour_Spectrum)

using System.Collections;
using PokemonUnity;
using PokemonUnity.Localization;
using PokemonUnity.Attack.Data;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;
using PokemonUnity.Monster;
using PokemonUnity.Overworld;
using PokemonUnity.Utility;
using PokemonEssentials;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
using UnityEngine;
using UnityEngine.UI;

public class TypingHandler : MonoBehaviour
{
    public string typedString;

    public int selectorIndex = 0;
    public int typeSpaceIndex = 0;
    public int pageIndex = 0;
    public int typeSpaceCount = 12;

    public AudioClip decideClip;
    public AudioClip typeClip;
    public AudioClip cancelClip;

    private Sprite[] iconAnim;

    private Image icon;
    private Image iconShadow;

    private Text genderText;
    private Text genderTextShadow;

    private Image[] typeSpace = new Image[12];
    private Text[] typeSpaceText = new Text[12];
    private Text[] typeSpaceTextShadow = new Text[12];

    private Transform controlPanel;
    private Image[] panelButton = new Image[6];

    private Image panelSelector;
    private Image keySelector;

    private RectTransform[] page = new RectTransform[4];

    private Text[][] key = new Text[][]
    {
        new Text[65],
        new Text[65],
        new Text[65]
    };

    private Text[][] keyShadow = new Text[][]
    {
        new Text[65],
        new Text[65],
        new Text[65]
    };

    private Text keyboardText;
    private Text keyboardTextShadow;

    private Vector2 selectorPosition;
    private Image currentSelector;

    void Awake()
    {
        Transform typeFrame = transform.Find("typeFrame");
        icon = typeFrame.Find("icon").GetComponent<Image>();
        iconShadow = typeFrame.Find("iconShadow").GetComponent<Image>();

        genderTextShadow = typeFrame.Find("gender").GetComponent<Text>();
        genderText = genderTextShadow.transform.Find("Text").GetComponent<Text>();

        for (int i = 0; i < 12; i++)
        {
            typeSpace[i] = typeFrame.Find("space" + i).GetComponent<Image>();
            typeSpaceTextShadow[i] = typeFrame.Find("letter" + i).GetComponent<Text>();
            typeSpaceText[i] = typeSpaceTextShadow[i].transform.Find("Text").GetComponent<Text>();
        }

        controlPanel = transform.Find("controlPanel");
        for (int i = 0; i < 6; i++)
        {
            panelButton[i] = controlPanel.Find("button" + i).GetComponent<Image>();
        }

        panelSelector = controlPanel.Find("Selector").GetComponent<Image>();
        keySelector = transform.Find("keySelector").GetComponent<Image>();

        for (int i = 0; i < 4; i++)
        {
            page[i] = transform.Find("page" + i).GetComponent<RectTransform>();
        }

        for (int i = 0; i < 3; i++)
        {
            for (int i2 = 0; i2 < 5; i2++)
            {
                Transform row = page[i].Find("row" + i2);
                for (int i3 = 0; i3 < 13; i3++)
                {
                    keyShadow[i][(i2 * 13) + i3] = row.Find("key" + i3).GetComponent<Text>();
                    key[i][(i2 * 13) + i3] =
                        keyShadow[i][(i2 * 13) + i3].transform.Find("Text").GetComponent<Text>();
                }
            }
        }
        keyboardTextShadow = page[3].Find("keyboard").GetComponent<Text>();
        keyboardText = keyboardTextShadow.transform.Find("Text").GetComponent<Text>();
    }

    void Start()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Y))
        {
            int newHeirIndex = page[0].GetSiblingIndex();
            int oldHeirIndex = page[2].GetSiblingIndex();
            page[0].SetSiblingIndex(oldHeirIndex);
            page[2].SetSiblingIndex(newHeirIndex);
        }
    }


    private float flipIncrement(float increment, bool flip)
    {
        return (flip) ? 1 - increment : increment;
    }

    private IEnumerator animateIcon()
    {
        while (iconAnim != null)
        {
            for (int i = 0; i < iconAnim.Length; i++)
            {
                icon.sprite = iconAnim[i];
                yield return new WaitForSeconds(0.3f);
            }
            yield return null;
        }
    }

    private IEnumerator animateTypeSpace()
    {
        for (int i = 0; i < 12; i++)
        {
            typeSpaceCount = (typeSpaceCount > 12) ? 12 : typeSpaceCount;
            typeSpace[i].enabled = (i < typeSpaceCount);
            typeSpaceTextShadow[i].gameObject.SetActive(i < typeSpaceCount);
        }
        int typeSpaceY = -14;

        float speed = 0.12f;
        float increment = 0f;
        bool flip = false;
        while (true)
        {
            while (increment < 1f)
            {
                increment += (1f / speed) * Time.deltaTime;
                if (increment > 1f)
                {
                    increment = 1f;
                }
                for (int i = 0; i < typeSpaceCount; i++)
                {
                    if (i == typeSpaceIndex)
                    {
                        typeSpace[i].rectTransform.localPosition =
                            new Vector3(typeSpace[i].rectTransform.localPosition.x,
                                typeSpaceY - (2f * flipIncrement(increment, flip)),
                                typeSpace[i].rectTransform.localPosition.z);
                    }
                    else
                    {
                        typeSpace[i].rectTransform.localPosition =
                            new Vector3(typeSpace[i].rectTransform.localPosition.x, typeSpaceY,
                                typeSpace[i].rectTransform.localPosition.z);
                    }
                }
                yield return null;
            }
            increment = 0f;
            flip = !flip;
        }
    }

    private IEnumerator animateSelector()
    {
        setSelectorPosition(selectorIndex);
        StartCoroutine(cycleSelectorColor());

        float speed = 0.25f;
        float increment = 0f;
        bool flip = false;
        while (true)
        {
            while (increment < 1f)
            {
                increment += (1f / speed) * Time.deltaTime;
                if (increment > 1f)
                {
                    increment = 1f;
                }
                if (selectorIndex < 4)
                {
                    currentSelector.rectTransform.sizeDelta = new Vector2(32, 24);
                }
                else if (selectorIndex < 6)
                {
                    currentSelector.rectTransform.sizeDelta = new Vector2(39, 24);
                }
                else if (pageIndex == 3)
                {
                    currentSelector.rectTransform.sizeDelta = new Vector2(146 + (4f * flipIncrement(increment, flip)),
                        14 + (4f * flipIncrement(increment, flip)));
                }
                else
                {
                    currentSelector.rectTransform.sizeDelta = new Vector2(14 + (4f * flipIncrement(increment, flip)),
                        14 + (4f * flipIncrement(increment, flip)));
                }
                yield return null;
            }
            increment = 0f;
            flip = !flip;
        }
    }

    private IEnumerator cycleSelectorColor()
    {
        float speed = 0.35f;
        float increment = 0f;
        bool flip = false;
        while (true)
        {
            while (increment < 1f)
            {
                increment += (1f / speed) * Time.deltaTime;
                if (increment > 1f)
                {
                    increment = 1f;
                }
                currentSelector.color = new Color(1, (flipIncrement(increment, flip) * 0.5f), 0, 1);
                yield return null;
            }
            increment = 0f;
            flip = !flip;
        }
    }

    private IEnumerator swapPages(int newPageIndex)
    {
        if (newPageIndex >= 0 && newPageIndex < 4 && newPageIndex != pageIndex)
        {
            float speed = 0.5f;
            float increment = 0f;

            //set all buttons except newlyActive to inactive
            for (int i = 0; i < 4; i++)
            {
                panelButton[i].enabled = (i == newPageIndex);
            }

            //render new page in front of the old one
            int newHeirIndex = page[newPageIndex].GetSiblingIndex();
            int oldHeirIndex = page[pageIndex].GetSiblingIndex();
            page[newPageIndex].SetSiblingIndex(oldHeirIndex);
            page[pageIndex].SetSiblingIndex(newHeirIndex);

            //move new page in from left, move old page away to bottom
            while (increment < 1f)
            {
                increment += (1f / speed) * Time.deltaTime;
                if (increment > 1f)
                {
                    increment = 1f;
                }

                page[newPageIndex].localPosition = new Vector3(-300f * (1 - increment), -31, 0);
                page[pageIndex].localPosition = new Vector3(0, -31 - 140f * increment, 0);

                yield return null;
            }

            //render pages in original positions
            page[newPageIndex].SetSiblingIndex(newHeirIndex);
            page[pageIndex].SetSiblingIndex(oldHeirIndex);

            pageIndex = newPageIndex;
        }
    }

    private void setSelectorPosition(int newPosition)
    {
        if (newPosition < 0)
        {
            newPosition = 0;
        }

        selectorIndex = newPosition;

        int[] panelColumn = new int[]
        {
            -88, -56, -24, 8, 46, 86
        };
        int panelRow = 33;

        int[] keyColumn = new int[]
        {
            -96, -80, -64, -48, -32, -16, 0, 16, 32, 48, 64, 80, 96
        };
        int[] keyRow = new int[]
        {
            7, -12, -31, -50, -69
        };

        if (newPosition < 6)
        {
            selectorPosition = new Vector2(panelColumn[newPosition], panelRow);
            currentSelector = panelSelector;
            currentSelector.rectTransform.localPosition = new Vector3(selectorPosition.x, selectorPosition.y, 0);

            panelSelector.enabled = true;
            keySelector.enabled = false;
        }
        else
        {
            if (pageIndex == 3)
            {
                selectorPosition = new Vector3(0, -21);
            }
            else
            {
                newPosition -= 6;
                if (newPosition > 64)
                {
                    newPosition = 64;
                }
                selectorPosition = new Vector2(keyColumn[Mathf.FloorToInt((float) newPosition % 13)],
                    keyRow[Mathf.FloorToInt((float) newPosition / 13f)]);
            }
            currentSelector = keySelector;
            currentSelector.rectTransform.localPosition = new Vector3(selectorPosition.x, selectorPosition.y, 0);

            panelSelector.enabled = false;
            keySelector.enabled = true;
        }
    }

    private void addCurrentKeyToString()
    {
        if (pageIndex < 3 && pageIndex >= 0)
        {
            if (typeSpaceIndex < typeSpaceCount)
            {
                typeSpaceText[typeSpaceIndex].text = key[pageIndex][selectorIndex - 6].text;
                typeSpaceTextShadow[typeSpaceIndex].text = typeSpaceText[typeSpaceIndex].text;
                typeSpaceIndex += 1;
            }
        }
    }

    private void resetText()
    {
        typedString = "";
        for (int i = 0; i < typeSpaceText.Length; ++i)
        {
            typeSpaceText[i].text = "";
            typeSpaceIndex = 0;
        }
    }

    private void addCharacterToString(bool caps, string character, string capsCharacter)
    {
        if (typeSpaceIndex < typeSpaceCount)
        {
            if (!caps)
            {
                typeSpaceText[typeSpaceIndex].text = character;
            }
            else
            {
                typeSpaceText[typeSpaceIndex].text = capsCharacter;
            }
            typeSpaceTextShadow[typeSpaceIndex].text = typeSpaceText[typeSpaceIndex].text;
            typeSpaceIndex += 1;
        }
    }

    private IEnumerator backspace()
    {
        if (typeSpaceIndex > 0)
        {
            typeSpaceText[typeSpaceIndex - 1].text = "";
            typeSpaceTextShadow[typeSpaceIndex - 1].text = typeSpaceText[typeSpaceIndex - 1].text;
            typeSpaceIndex -= 1;
            panelButton[4].enabled = true;
            yield return new WaitForSeconds(0.1f);
            panelButton[4].enabled = false;
        }
    }

    private void compileString()
    {
        typedString = "";
        for (int i = 0; i < typeSpaceCount; i++)
        {
            if (typeSpaceText[i].text.Length > 0)
            {
                typedString += typeSpaceText[i].text.Substring(0, 1);
            }
        }
        Debug.Log("String =" + typedString + ".");
    }


    public IEnumerator control(int typeSpaces, string defaultString, bool? genderDisplay = null,
        Sprite[] iconAnimation = null)
    {
        typeSpaceCount = typeSpaces;
        iconAnim = iconAnimation;
        if (iconAnimation != null)
        {
            icon.sprite = (iconAnimation.Length < 1) ? null : iconAnim[0];
        }

        selectorIndex = 0;
        pageIndex = 0;

        if (genderDisplay == false)
        {
            genderText.text = "♀";
            genderText.color = new Color(1, 0.2f, 0.2f, 1);
        }
        else if (genderDisplay == true)
        {
            genderText.text = "♂";
            genderText.color = new Color(0.2f, 0.4f, 1, 1);
        }
        else
        {
            genderText.text = null;
        }
        genderTextShadow.text = genderText.text;

        typeSpaceIndex = 0;
        
        for (int i = 0; i < typeSpaceCount; i++)
        {
            if (i < defaultString.Length)
            {
                typeSpaceText[i].text = defaultString.Substring(i, 1);
                typeSpaceIndex = i + 1;
            }
            else
            {
                typeSpaceText[i].text = "";
            }
            typeSpaceTextShadow[i].text = typeSpaceText[i].text;
        }

        page[0].localPosition = new Vector3(0, -31, 0);
        for (int i = 1; i < 4; i++)
        {
            page[i].localPosition = new Vector3(-300f, -31, 0);
        }

        panelButton[0].enabled = true;
        for (int i = 1; i < 6; i++)
        {
            panelButton[i].enabled = false;
        }

        float buttonDelay = 0.15f;

        StartCoroutine(animateSelector());
        StartCoroutine(animateTypeSpace());
        StartCoroutine(animateIcon());

        StartCoroutine(ScreenFade.main.Fade(true, ScreenFade.defaultSpeed));

        bool running = true;
        
        yield return StartCoroutine(typeWithKeyboard());
        
        /*
        while (running)
        {
            if (UnityEngine.Input.GetAxisRaw("Horizontal") > 0)
            {
                if (selectorIndex < 5)
                {
                    setSelectorPosition(selectorIndex + 1);
                    yield return new WaitForSeconds(buttonDelay);
                }
                else if (selectorIndex == 5)
                {
                    setSelectorPosition(selectorIndex - 5);
                    yield return new WaitForSeconds(buttonDelay);
                }
                else if (((selectorIndex - 6) - (Mathf.Floor((float) (selectorIndex - 6) / 13f) * 13)) % 13 < 12)
                {
                    setSelectorPosition(selectorIndex + 1);
                    yield return new WaitForSeconds(buttonDelay);
                }
                else if (((selectorIndex - 6) - (Mathf.Floor((float) (selectorIndex - 6) / 13f) * 13)) % 13 == 12)
                {
                    setSelectorPosition(selectorIndex - 12);
                    yield return new WaitForSeconds(buttonDelay);
                }
            }
            else if (UnityEngine.Input.GetAxisRaw("Horizontal") < 0)
            {
                if (selectorIndex <= 5)
                {
                    if (selectorIndex == 0)
                    {
                        setSelectorPosition(selectorIndex + 5);
                        yield return new WaitForSeconds(buttonDelay);
                    }
                    else if (selectorIndex > 0)
                    {
                        setSelectorPosition(selectorIndex - 1);
                        yield return new WaitForSeconds(buttonDelay);
                    }
                }
                else if (((selectorIndex - 6) - (Mathf.Floor((float) (selectorIndex - 6) / 13f) * 13)) % 13 > 0)
                {
                    setSelectorPosition(selectorIndex - 1);
                    yield return new WaitForSeconds(buttonDelay);
                }
                else if (((selectorIndex - 6) - (Mathf.Floor((float) (selectorIndex - 6) / 13f) * 13)) % 13 == 0)
                {
                    setSelectorPosition(selectorIndex + 12);
                    yield return new WaitForSeconds(buttonDelay);
                }
            }
            if (UnityEngine.Input.GetAxisRaw("Vertical") > 0)
            {
                if (pageIndex == 3)
                {
                    setSelectorPosition(3);
                    yield return new WaitForSeconds(buttonDelay);
                }
                else if (selectorIndex > 5)
                {
                    //row 0 of keys
                    if (selectorIndex < 19)
                    {
                        if (selectorIndex > 16)
                        {
                            setSelectorPosition(5);
                        }
                        else if (selectorIndex > 13)
                        {
                            setSelectorPosition(4);
                        }
                        else if (selectorIndex > 11)
                        {
                            setSelectorPosition(3);
                        }
                        else if (selectorIndex > 9)
                        {
                            setSelectorPosition(2);
                        }
                        else if (selectorIndex > 7)
                        {
                            setSelectorPosition(1);
                        }
                        else
                        {
                            setSelectorPosition(0);
                        }
                        yield return new WaitForSeconds(buttonDelay);
                    }
                    else
                    {
                        setSelectorPosition(selectorIndex - 13);
                        yield return new WaitForSeconds(buttonDelay);
                    }
                }
            }
            else if (UnityEngine.Input.GetAxisRaw("Vertical") < 0)
            {
                if (pageIndex == 3)
                {
                    setSelectorPosition(6);
                    yield return new WaitForSeconds(buttonDelay);
                }
                else if (selectorIndex < 6)
                {
                    if (selectorIndex == 5)
                    {
                        setSelectorPosition(17);
                    }
                    else if (selectorIndex == 4)
                    {
                        setSelectorPosition(14);
                    }
                    else if (selectorIndex == 3)
                    {
                        setSelectorPosition(12);
                    }
                    else if (selectorIndex == 2)
                    {
                        setSelectorPosition(10);
                    }
                    else if (selectorIndex == 1)
                    {
                        setSelectorPosition(8);
                    }
                    else
                    {
                        setSelectorPosition(6);
                    }
                    yield return new WaitForSeconds(buttonDelay);
                }
                else if (selectorIndex < 58)
                {
                    setSelectorPosition(selectorIndex + 13);
                    yield return new WaitForSeconds(buttonDelay);
                }
            }
            if (UnityEngine.Input.GetButtonDown("Start"))
            {
                setSelectorPosition(5);
                yield return new WaitForSeconds(buttonDelay);
            }
            else if (UnityEngine.Input.GetButtonDown("Back") || UnityEngine.Input.GetButtonDown("Run"))
            {
                yield return StartCoroutine(backspace());
            }
            else if (UnityEngine.Input.GetButtonDown("Select"))
            {
                if (selectorIndex == 0)
                {
                    //Page0
                    yield return StartCoroutine(swapPages(0));
                }
                else if (selectorIndex == 1)
                {
                    //Page1
                    yield return StartCoroutine(swapPages(1));
                }
                else if (selectorIndex == 2)
                {
                    //Page2
                    yield return StartCoroutine(swapPages(2));
                }
                else if (selectorIndex == 3)
                {
                    //Page3
                    yield return StartCoroutine(swapPages(3));
                }
                else if (selectorIndex == 4)
                {
                    //Back
                    yield return StartCoroutine(backspace());
                }
                else if (selectorIndex == 5)
                {
                    //OK
                    panelButton[5].enabled = true;
                    yield return new WaitForSeconds(0.1f);
                    panelButton[5].enabled = false;
                    compileString();
                    running = false;
                }
                else
                {
                    if (pageIndex == 3)
                    {
                        //Keyboard
                        yield return StartCoroutine(typeWithKeyboard());
                    }
                    else
                    {
                        //Keys
                        addCurrentKeyToString();
                    }
                }
            }

            yield return null;
        }
        */

        //yield return new WaitForSeconds(sceneTransition.FadeOut());
        yield return StartCoroutine(ScreenFade.main.Fade(false, ScreenFade.defaultSpeed));

        gameObject.SetActive(false);
    }

    private IEnumerator typeWithKeyboard()
    {
        bool running = true;
        bool caps = false;

        keyboardText.text = "Press Esc to stop typing";
        keyboardTextShadow.text = keyboardText.text;
        //yield return null;

        while (running)
        {
            if (UnityEngine.Input.GetKey(KeyCode.LeftShift))
            {
                caps = true;
            }
            else
            {
                caps = false;
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.BackQuote))
            {
                addCharacterToString(caps, "‘", "~");
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1))
            {
                addCharacterToString(caps, "1", "!");
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha2))
            {
                addCharacterToString(caps, "2", "@");
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha3))
            {
                addCharacterToString(caps, "3", "#");
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha4))
            {
                addCharacterToString(caps, "4", "4");
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha5))
            {
                addCharacterToString(caps, "5", "%");
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha6))
            {
                addCharacterToString(caps, "6", "6");
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha7))
            {
                addCharacterToString(caps, "7", "&");
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha8))
            {
                addCharacterToString(caps, "8", "*");
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha9))
            {
                addCharacterToString(caps, "9", "(");
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha0))
            {
                addCharacterToString(caps, "0", ")");
            }

            else if (UnityEngine.Input.GetKeyDown(KeyCode.Q))
            {
                addCharacterToString(caps, "q", "Q");
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.W))
            {
                addCharacterToString(caps, "w", "W");
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.E))
            {
                addCharacterToString(caps, "e", "E");
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.R))
            {
                addCharacterToString(caps, "r", "R");
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.T))
            {
                addCharacterToString(caps, "t", "T");
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.Y))
            {
                addCharacterToString(caps, "y", "Y");
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.U))
            {
                addCharacterToString(caps, "u", "U");
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.I))
            {
                addCharacterToString(caps, "i", "I");
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.O))
            {
                addCharacterToString(caps, "o", "O");
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.P))
            {
                addCharacterToString(caps, "p", "P");
            }

            else if (UnityEngine.Input.GetKeyDown(KeyCode.A))
            {
                addCharacterToString(caps, "a", "A");
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.S))
            {
                addCharacterToString(caps, "s", "S");
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.D))
            {
                addCharacterToString(caps, "d", "D");
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.F))
            {
                addCharacterToString(caps, "f", "F");
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.G))
            {
                addCharacterToString(caps, "g", "G");
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.H))
            {
                addCharacterToString(caps, "h", "H");
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.J))
            {
                addCharacterToString(caps, "j", "J");
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.K))
            {
                addCharacterToString(caps, "k", "K");
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.L))
            {
                addCharacterToString(caps, "l", "L");
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.Semicolon))
            {
                addCharacterToString(caps, ";", ":");
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.Quote))
            {
                addCharacterToString(caps, "’", "”");
            }

            else if (UnityEngine.Input.GetKeyDown(KeyCode.Z))
            {
                addCharacterToString(caps, "z", "Z");
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.X))
            {
                addCharacterToString(caps, "x", "X");
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.C))
            {
                addCharacterToString(caps, "c", "C");
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.V))
            {
                addCharacterToString(caps, "v", "V");
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.B))
            {
                addCharacterToString(caps, "b", "B");
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.N))
            {
                addCharacterToString(caps, "n", "N");
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.M))
            {
                addCharacterToString(caps, "m", "M");
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.Comma))
            {
                addCharacterToString(caps, ",", ",");
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.Period))
            {
                addCharacterToString(caps, ".", ".");
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.Slash))
            {
                addCharacterToString(caps, "/", "?");
            }

            else if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                addCharacterToString(caps, " ", " ");
            }

            else if (UnityEngine.Input.GetKeyDown(KeyCode.Backspace))
            {
                yield return StartCoroutine(backspace());
            }

            else if (UnityEngine.Input.GetButtonDown("Back") && !UnityEngine.Input.GetKeyDown(KeyCode.X))
            {
                running = false;
            }

            else if (UnityEngine.Input.GetButtonDown("Enter"))
            {
                compileString();
                if (typedString.Length > 0)
                {
                    SfxHandler.Play(decideClip);
                    running = false;
                }
                else
                {
                    SfxHandler.Play(cancelClip);
                }

                yield return new WaitForSeconds(0.4f);
            }

            yield return null;
        }

        keyboardText.text = "Select to use the Keyboard";
        keyboardTextShadow.text = keyboardText.text;
    }
}