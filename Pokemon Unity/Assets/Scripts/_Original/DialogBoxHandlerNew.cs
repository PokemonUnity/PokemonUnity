//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogBoxHandlerNew : MonoBehaviour
{
    public enum PrintTextMethod
    {
        Typewriter,
        Instant
    }
    
    public string debugBoxString;

    private Image dialogBox;
    private Text dialogBoxText;
    private Text dialogBoxTextShadow;
    private Image dialogBoxBorder;

    private Image choiceBox;
    private Text choiceBoxText;
    private Text choiceBoxTextShadow;
    private Image choiceBoxSelect;

    private Image nameBox;
    private Text nameBoxText;
    private Text nameBoxTextShadow;


    public AudioClip selectClip;

    private float charPerSec = 60f;
    public float scrollSpeed = 0.1f;

    public int chosenIndex;

    public bool hideDialogOnStart = true;
    public bool hideChoiceOnStart = true;

    private int defaultChoiceWidth = 55;
    public int defaultChoiceY = 0;
    public int defaultDialogLines = 2;

    private bool dialogBoxSpriteEmpty;
    private bool choiceBoxSpriteEmpty;

    private float selectY;


    void Awake()
    {
        Transform dialogBoxTrn = transform.Find("DialogBox");
        dialogBox = dialogBoxTrn.GetComponent<Image>();
        dialogBoxText = dialogBoxTrn.Find("BoxText").GetComponent<Text>();
        dialogBoxTextShadow = dialogBoxTrn.Find("BoxTextShadow").GetComponent<Text>();
        dialogBoxBorder = dialogBoxTrn.Find("BoxBorder").GetComponent<Image>();

        Transform choiceBoxTrn = transform.Find("ChoiceBox");
        choiceBox = choiceBoxTrn.GetComponent<Image>();
        
        choiceBoxTextShadow = choiceBoxTrn.Find("BoxTextShadow").GetComponent<Text>();
        choiceBoxText = choiceBoxTrn.transform.Find("BoxText").GetComponent<Text>();
        choiceBoxSelect = choiceBoxTrn.Find("BoxSelect").GetComponent<Image>();

        if (transform.Find("NameBox") != null)
        {
            Transform nameBoxTrn = transform.Find("NameBox");
            nameBox = nameBoxTrn.GetComponent<Image>();
            nameBoxText = nameBoxTrn.Find("BoxText").GetComponent<Text>();
            nameBoxTextShadow = nameBoxTrn.Find("BoxTextShadow").GetComponent<Text>();
        }

        defaultDialogLines = Mathf.RoundToInt((dialogBoxBorder.rectTransform.sizeDelta.y - 16f) / 14f);
        defaultChoiceY = Mathf.FloorToInt(choiceBox.rectTransform.localPosition.y);
        
        dialogBoxSpriteEmpty = (dialogBoxBorder.sprite == null);
        dialogBoxSpriteEmpty = (choiceBox.sprite == null);

        selectY = choiceBoxSelect.rectTransform.anchoredPosition.y;
    }

    void Start()
    {
        if (hideDialogOnStart)
        {
            dialogBox.gameObject.SetActive(false);
        }
        if (hideChoiceOnStart)
        {
            choiceBox.gameObject.SetActive(false);
        }
        if (nameBox != null)
        {
            nameBox.gameObject.SetActive(false);
        }
    }


    public IEnumerator DrawText(string text)
    {
        yield return StartCoroutine(DrawText(text, -1, false));
    }

    public IEnumerator DrawText(string text, float secPerChar)
    {
        yield return StartCoroutine(DrawText(text, secPerChar, false));
    }

    public IEnumerator DrawTextSilent(string text)
    {
        yield return StartCoroutine(DrawText(text, -1, true));
    }

    public void DrawTextInstant(string text)
    {
         StartCoroutine(DrawText(text, 0, false));
    }

    public void DrawTextInstantSilentFunction(string text)
    {
        StartCoroutine(DrawTextInstantSilent(text));
    }
    
    public IEnumerator DrawTextInstantSilent(string text)
    {
        yield return StartCoroutine(DrawText(text, 0, true));
    }

    public IEnumerator DrawText(string text, float secPerChar, bool silent)
    {
        if (secPerChar < 0)
        {
            int textSpeed = PlayerPrefs.GetInt("textSpeed") + 1;
            charPerSec = 16 + (textSpeed * textSpeed * 9);
            secPerChar = 1 / charPerSec;
        }
        
        string[] words = text.Split(new char[] {' '});

        if (!silent)
        {
            SfxHandler.Play(selectClip);
        }
        for (int i = 0; i < words.Length; i++)
        {
            if (secPerChar > 0)
            {
                yield return StartCoroutine(DrawWord(words[i], secPerChar));
            }
            else
            {
                StartCoroutine(DrawWord(words[i], secPerChar));
            }
        }
    }

    private IEnumerator DrawWord(string word, float secPerChar)
    {
        yield return StartCoroutine(DrawWord(word, false, false, false, secPerChar));
    }

    private IEnumerator DrawWord(string word, bool large, bool bold, bool italic, float secPerChar, string color = "")
    {
        char[] chars = word.ToCharArray();
        float startTime = Time.time;

        if (chars.Length > 0)
        {
            //ensure no blank words get processed

            if (chars[0] == '\\')
            {
                //Apply Operator
                switch (chars[1])
                {
                    case ('p'): //Player
                        if (secPerChar > 0)
                        {
                            yield return
                                StartCoroutine(DrawWord(SaveData.currentSave.playerName, large, bold, italic,
                                    secPerChar));
                        }
                        else
                        {
                            StartCoroutine(DrawWord(SaveData.currentSave.playerName, large, bold, italic, secPerChar));
                        }
                        break;
                    case ('l'): //Large
                        large = true;
                        break;
                    case ('b'): //Bold
                        bold = true;
                        break;
                    case ('i'): //Italic
                        italic = true;
                        break;
                    case ('n'): //New Line
                        dialogBoxText.text += "\n";
                        break;
                    /* colors */
                    case ('R'):
                        color = "#DD1F1F";
                        break;
                    case ('G'):
                        color = "#5FF34D";
                        break;
                    case ('B'):
                        color = "#1F95DD";
                        break;

                    }
                if (chars.Length > 2)
                {
                    //Run this function for the rest of the word
                    string remainingWord = "";
                    for (int i = 2; i < chars.Length; i++)
                    {
                        remainingWord += chars[i].ToString();
                    }
                    yield return StartCoroutine(DrawWord(remainingWord, large, bold, italic, secPerChar, color));
                }
            }
            else if (chars[0] == '{')
            {
                //Apply Operator
                switch (chars[1])
                {
                    case ('p'): //Player
                        if (secPerChar > 0)
                        {
                            yield return
                                StartCoroutine(DrawWord(SaveData.currentSave.playerName, large, bold, italic, secPerChar));
                        }
                        else
                        {
                            StartCoroutine(DrawWord(SaveData.currentSave.playerName, large, bold, italic, secPerChar));
                        }
                        break;
                    case ('f'):
                        if (secPerChar > 0)
                        {
                            yield return
                                StartCoroutine(DrawWord(SaveData.currentSave.PC.boxes[0][0].getName(), large, bold, italic, secPerChar));
                        }
                        else
                        {
                            StartCoroutine(DrawWord(SaveData.currentSave.PC.boxes[0][0].getName(), large, bold, italic, secPerChar));
                        }
                        break;
                    /* colors */
                    case ('R'):
                        color = "#DD1F1F";
                        break;
                    case ('G'):
                        color = "#5FF34D";
                        break;
                    case ('B'):
                        color = "#1F95DD";
                        break;
                }
            }
            else
            {
                //Draw Word
                string currentText = dialogBoxText.text;

                for (int i = 0; i <= chars.Length; i++)
                {
                    string added = "";

                    //apply open tags
                    added += (large) ? "<size=32>" : "";
                    added += (bold) ? "<b>" : "";
                    added += (italic) ? "<i>" : "";
                    if (color.Length > 0)
                    {
                        added += "<color=" + color + ">";
                    }

                    //apply displayed text
                    for (int i2 = 0; i2 < i; i2++)
                    {
                        added += chars[i2].ToString();
                    }

                    //apply hidden text
                    added += "<color=#0000>";
                    for (int i2 = i; i2 < chars.Length; i2++)
                    {
                        added += chars[i2].ToString();
                    }
                    added += "</color>";

                    //apply close tags
                    added += (italic) ? "</i>" : "";
                    added += (bold) ? "</b>" : "";
                    added += (large) ? "</size>" : "";
                    if (color.Length > 0)
                    {
                        added += "</color>";
                    }

                    dialogBoxText.text = currentText + added;
                    dialogBoxTextShadow.text = dialogBoxText.text;

                    while (Time.time < startTime + (secPerChar * (i + 1)))
                    {
                        yield return null;
                    }
                }

                //add a space after every word
                dialogBoxText.text += " ";
                dialogBoxTextShadow.text = dialogBoxText.text;
                while (Time.time < startTime + (secPerChar))
                {
                    yield return null;
                }
            }
        }
    }


    public void DrawDialogBox()
    {
        StartCoroutine(DrawDialogBox(defaultDialogLines, Color.white, false));
    }
    
    public void DrawBlackFrame()
    {
        StartCoroutine(DrawBlackFrame(defaultDialogLines, Color.white, false));
    }
    
    public void DrawScreamFrame()
    {
        StartCoroutine(DrawScreamFrame(defaultDialogLines, Color.white, false));
    }


    public void DrawDialogBox(int lines)
    {
        StartCoroutine(DrawDialogBox(lines, Color.white, false));
    }

    public void DrawSignBox(Color tint)
    {
        StartCoroutine(DrawDialogBox(defaultDialogLines, tint, true));
    }

    private IEnumerator DrawDialogBox(int lines, Color tint, bool sign)
    {
        dialogBox.gameObject.SetActive(true);

        dialogBoxBorder.sprite = Resources.Load<Sprite>("Frame/bwtextskin");
        
        if (sign)
        {
            dialogBoxText.color = new Color(1, 1, 1, 1);
            dialogBoxTextShadow.color = new Color(1, 1, 1, 1);
        }
        else
        {
            dialogBoxText.color = new Color(0.32f, 0.32f, 0.32f, 1);
            dialogBoxTextShadow.color = new Color(0.44f, 0.44f, 0.44f, 1);
        }

        dialogBoxBorder.color = tint;
        dialogBoxText.text = "";
        dialogBoxTextShadow.text = dialogBoxText.text;

        yield return null;
    }
    
    private IEnumerator DrawBlackFrame(int lines, Color tint, bool sign)
    {
        dialogBox.gameObject.SetActive(true);
        
        dialogBoxBorder.sprite = Resources.Load<Sprite>("Frame/blacktextskin");
        dialogBoxText.color = new Color(1, 1, 1, 1);
        dialogBoxTextShadow.color = new Color(1, 1, 1, 0.5f);
        
        dialogBoxBorder.color = tint;
        dialogBoxText.text = "";
        dialogBoxTextShadow.text = dialogBoxText.text;

        yield return null;
    }

    private IEnumerator DrawScreamFrame(int lines, Color tint, bool sign)
    {
        dialogBox.gameObject.SetActive(true);
        
        dialogBoxBorder.sprite = Resources.Load<Sprite>("Frame/scream");
        dialogBoxText.color = new Color(0.32f, 0.32f, 0.32f, 1);
        dialogBoxTextShadow.color = new Color(0.44f, 0.44f, 0.44f, 1);
        
        dialogBoxBorder.color = tint;
        dialogBoxText.text = "";
        dialogBoxTextShadow.text = dialogBoxText.text;

        yield return null;
    }
    
    // Name Boxes

    public void DrawNameBox(string name)
    {
        nameBoxText.text = name;
        nameBoxTextShadow.text = nameBoxText.text;
        nameBox.rectTransform.sizeDelta = new Vector2(nameBoxText.preferredWidth + 18 ,nameBox.rectTransform.sizeDelta.y);
        
        nameBox.gameObject.SetActive(true);
    }
    
    public void UndrawNameBox()
    {
        nameBox.gameObject.SetActive(false);
    }
    
    // Choices Boxes

    public IEnumerator DrawChoiceBox()
    {
        string[] choices;
        switch (Language.getLang())
        {
            case Language.Country.FRANCAIS:
                choices = new [] {"Oui", "Non"};
                break;
            default:
                choices = new [] {"Yes", "No"};
                break;
        }

        yield return
            StartCoroutine(DrawChoiceBox(choices, null, -1, defaultChoiceY, defaultChoiceWidth));
    }
    
    public IEnumerator DrawChoiceBoxNo()
    {
        yield return
            StartCoroutine(DrawChoiceBox(0));
    }

    public IEnumerator DrawChoiceBox(string[] choices)
    {
        yield return StartCoroutine(DrawChoiceBox(choices, null, -1, defaultChoiceY, defaultChoiceWidth));
    }

    public IEnumerator DrawChoiceBox(int startIndex)
    {
        string[] choices;
        switch (Language.getLang())
        {
            case Language.Country.FRANCAIS:
                choices = new [] {"Oui", "Non"};
                break;
            default:
                choices = new [] {"Yes", "No"};
                break;
        }
        
        yield return
            StartCoroutine(DrawChoiceBox(new string[] {"Yes", "No"}, null, startIndex, defaultChoiceY,
                defaultChoiceWidth));
    }

    public IEnumerator DrawChoiceBox(string[] choices, int startIndex)
    {
        yield return StartCoroutine(DrawChoiceBox(choices, null, startIndex, defaultChoiceY, defaultChoiceWidth));
    }

    public IEnumerator DrawChoiceBox(string[] choices, string[] flavourText)
    {
        yield return StartCoroutine(DrawChoiceBox(choices, flavourText, -1, defaultChoiceY, defaultChoiceWidth));
    }

    public IEnumerator DrawChoiceBox(string[] choices, string[] flavourText, int startIndex)
    {
        yield return StartCoroutine(DrawChoiceBox(choices, flavourText, startIndex, defaultChoiceY, defaultChoiceWidth));
    }

    public IEnumerator DrawChoiceBox(string[] choices, int yPosition, int width)
    {
        yield return
            StartCoroutine(DrawChoiceBox(choices, null, -1, yPosition, width));
    }

    public IEnumerator DrawChoiceBox(string[] choices, int startIndex, int yPosition, int width)
    {
        yield return
            StartCoroutine(DrawChoiceBox(choices, null, startIndex, yPosition,
                width));
    }

    public IEnumerator DrawChoiceBox(string[] choices, string[] flavourText, int startIndex, int yPosition, int width)
    {
        if (startIndex < 0)
        {
            startIndex = choices.Length - 1;
        }

        choiceBox.gameObject.SetActive(true);
        
        if (choiceBoxSpriteEmpty)
            choiceBox.sprite = Resources.Load<Sprite>("Frame/choice" + PlayerPrefs.GetInt("frameStyle"));

        

        choiceBoxText.text = "";
        for (int i = 0; i < choices.Length; i++)
        {
            choiceBoxText.text += choices[i];
            if (i != choices.Length - 1)
            {
                choiceBoxText.text += "\n";
            }
        }
        choiceBoxTextShadow.text = choiceBoxText.text;
        
        Debug.Log(choiceBoxText.preferredWidth);
        choiceBox.rectTransform.sizeDelta = new Vector2(choiceBoxText.preferredWidth + 37, 16f + (15.5f * choices.Length));
        choiceBoxSelect.rectTransform.anchoredPosition = new Vector2(choiceBoxSelect.rectTransform.anchoredPosition.x, -(15.5f * (choices.Length - 1 - startIndex)) - 15.5f);

        bool selected = false;
        UpdateChosenIndex(startIndex, choices.Length, flavourText, true);
        while (!selected)
        {
            if (Input.GetButtonDown("Select"))
            {
                selected = true;
            }
            else if (Input.GetButtonDown("Back"))
            {
                chosenIndex = 1;
                    //a little hack to bypass the newIndex != chosenIndex in the below method, ensuring a true return
                if (UpdateChosenIndex(0, choices.Length, flavourText))
                {
                    yield return new WaitForSeconds(0.2f);
                }
                selected = true;
            }
            else if (Input.GetAxisRaw("Vertical") > 0)
            {
                if (UpdateChosenIndex(chosenIndex + 1, choices.Length, flavourText))
                {
                    yield return new WaitForSeconds(0.2f);
                }
            }
            else if (Input.GetAxisRaw("Vertical") < 0)
            {
                if (UpdateChosenIndex(chosenIndex - 1, choices.Length, flavourText))
                {
                    yield return new WaitForSeconds(0.2f);
                }
            }
            yield return null;
        }
    }

    private bool UpdateChosenIndex(int newIndex, int choicesLength, string[] flavourText, bool silent = false)
    {
        //Check for an invalid new index
        if (newIndex < 0 || newIndex >= choicesLength)
        {
            return false;
        }
        //Even if new index is the same as old, set the graphics in case of needing to override modified graphics.
        choiceBoxSelect.rectTransform.anchoredPosition = new Vector2(choiceBoxSelect.rectTransform.anchoredPosition.x, -(15.5f * (choicesLength - 1 - newIndex)) - 15.5f);
        //choiceBoxSelect.rectTransform.localPosition = new Vector3(choiceBoxSelect.rectTransform.localPosition.x, 14.5f + (15.2f * newIndex), 0);
        if (flavourText != null)
        {
            DrawDialogBox();
            StartCoroutine(DrawText(flavourText[flavourText.Length - 1 - newIndex], 0));
        }
        //If chosen index is the same as before, do not play a sound effect, then return false
        if (chosenIndex == newIndex)
        {
            return false;
        }
        chosenIndex = newIndex;
        if (!silent)
        {
            SfxHandler.Play(selectClip);
        }
        return true;
    }


    public void UndrawDialogBox()
    {
        dialogBoxText.text = "";
        dialogBoxTextShadow.text = "";
        dialogBox.gameObject.SetActive(false);
    }

    public IEnumerator UndrawSignBox()
    {
        /*
        Vector3 initialPos = dialogBox.rectTransform.localPosition;

        LeanTween.moveY(dialogBox.rectTransform, initialPos.y - 50, 0.2f);
        
        yield return new WaitForSeconds(0.2f);
        dialogBox.gameObject.SetActive(false);
        LeanTween.moveY(dialogBox.rectTransform, initialPos.y, 0);
        */
        
        dialogBox.gameObject.SetActive(false);
        yield return null;
    }

    public void UndrawChoiceBox()
    {
        choiceBox.gameObject.SetActive(false);
    }
}