//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogBoxHandlerNew : MonoBehaviour
{
    public string debugBoxString;

    private Image dialogBox;
    private Text dialogBoxText;
    private Text dialogBoxTextShadow;
    private Image dialogBoxBorder;

    private Image choiceBox;
    private Text choiceBoxText;
    private Text choiceBoxTextShadow;
    private Image choiceBoxSelect;

    public AudioClip selectClip;

    private float charPerSec = 60f;
    public float scrollSpeed = 0.1f;

    public int chosenIndex;

    public bool hideDialogOnStart = true;
    public bool hideChoiceOnStart = true;

    public int defaultChoiceWidth = 86;
    public int defaultChoiceY = 0;
    public int defaultDialogLines = 2;


    void Awake()
    {
        Transform dialogBoxTrn = transform.Find("DialogBox");
        dialogBox = dialogBoxTrn.GetComponent<Image>();
        dialogBoxText = dialogBoxTrn.Find("BoxText").GetComponent<Text>();
        dialogBoxTextShadow = dialogBoxTrn.Find("BoxTextShadow").GetComponent<Text>();
        dialogBoxBorder = dialogBoxTrn.Find("BoxBorder").GetComponent<Image>();

        Transform choiceBoxTrn = transform.Find("ChoiceBox");
        choiceBox = choiceBoxTrn.GetComponent<Image>();
        choiceBoxText = choiceBoxTrn.Find("BoxText").GetComponent<Text>();
        choiceBoxTextShadow = choiceBoxTrn.Find("BoxTextShadow").GetComponent<Text>();
        choiceBoxSelect = choiceBoxTrn.Find("BoxSelect").GetComponent<Image>();

        defaultDialogLines = Mathf.RoundToInt((dialogBoxBorder.rectTransform.sizeDelta.y - 16f) / 14f);
        defaultChoiceY = Mathf.FloorToInt(choiceBox.rectTransform.localPosition.y);
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
    }


    public IEnumerator DrawText(string text)
    {
        yield return StartCoroutine(DrawText(text, 1f / charPerSec, false));
    }

    public IEnumerator DrawText(string text, float secPerChar)
    {
        yield return StartCoroutine(DrawText(text, secPerChar, false));
    }

    public IEnumerator DrawTextSilent(string text)
    {
        yield return StartCoroutine(DrawText(text, 1f / charPerSec, true));
    }

    public IEnumerator DrawTextInstant(string text)
    {
        yield return StartCoroutine(DrawText(text, 0, false));
    }

    public IEnumerator DrawText(string text, float secPerChar, bool silent)
    {
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

    private IEnumerator DrawWord(string word, bool large, bool bold, bool italic, float secPerChar)
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
                                StartCoroutine(DrawWord(SaveDataOld.currentSave.playerName, large, bold, italic, secPerChar))
                                ;
                        }
                        else
                        {
                            StartCoroutine(DrawWord(SaveDataOld.currentSave.playerName, large, bold, italic, secPerChar));
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
                }
                if (chars.Length > 2)
                {
                    //Run this function for the rest of the word
                    string remainingWord = "";
                    for (int i = 2; i < chars.Length; i++)
                    {
                        remainingWord += chars[i].ToString();
                    }
                    yield return StartCoroutine(DrawWord(remainingWord, large, bold, italic, secPerChar));
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
                    added += (large) ? "<size=26>" : "";
                    added += (bold) ? "<b>" : "";
                    added += (italic) ? "<i>" : "";

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
        StartCoroutine(DrawDialogBox(defaultDialogLines, new Color(1, 1, 1, 1), false));
    }

    public void DrawDialogBox(int lines)
    {
        StartCoroutine(DrawDialogBox(lines, new Color(1, 1, 1, 1), false));
    }

    public void DrawSignBox(Color tint)
    {
        StartCoroutine(DrawDialogBox(defaultDialogLines, tint, true));
    }

    private IEnumerator DrawDialogBox(int lines, Color tint, bool sign)
    {
        dialogBox.gameObject.SetActive(true);
        dialogBoxBorder.sprite = (sign)
            ? null
            : Resources.Load<Sprite>("Frame/dialog" + PlayerPrefs.GetInt("frameStyle"));
        dialogBox.sprite = (sign) ? Resources.Load<Sprite>("Frame/signBG") : Resources.Load<Sprite>("Frame/dialogBG");
        dialogBox.color = tint;
        dialogBoxText.text = "";
        dialogBoxText.color = (sign) ? new Color(1f, 1f, 1f, 1f) : new Color(0.0625f, 0.0625f, 0.0625f, 1f);
        dialogBoxTextShadow.text = dialogBoxText.text;

        dialogBox.rectTransform.sizeDelta = new Vector2(dialogBox.rectTransform.sizeDelta.x,
            Mathf.Round((float) lines * 14f) + 16f);
        dialogBoxBorder.rectTransform.sizeDelta = new Vector2(dialogBox.rectTransform.sizeDelta.x,
            dialogBox.rectTransform.sizeDelta.y);
        dialogBoxText.rectTransform.localPosition = new Vector3(dialogBoxText.rectTransform.localPosition.x,
            -37f + Mathf.Round((float) lines * 14f), 0);
        dialogBoxTextShadow.rectTransform.localPosition = new Vector3(
            dialogBoxTextShadow.rectTransform.localPosition.x, dialogBoxText.rectTransform.localPosition.y - 1f, 0);

        if (sign)
        {
            float increment = 0f;
            while (increment < 1)
            {
                increment += (1f / 0.2f) * Time.deltaTime;
                if (increment > 1)
                {
                    increment = 1;
                }

                dialogBox.rectTransform.localPosition = new Vector2(dialogBox.rectTransform.localPosition.x,
                    -dialogBox.rectTransform.sizeDelta.y + (dialogBox.rectTransform.sizeDelta.y * increment));
                yield return null;
            }
        }
    }

    public IEnumerator DrawChoiceBox()
    {
        yield return
            StartCoroutine(DrawChoiceBox(new string[] {"Yes", "No"}, null, -1, defaultChoiceY, defaultChoiceWidth));
    }

    public IEnumerator DrawChoiceBox(string[] choices)
    {
        yield return StartCoroutine(DrawChoiceBox(choices, null, -1, defaultChoiceY, defaultChoiceWidth));
    }

    public IEnumerator DrawChoiceBox(int startIndex)
    {
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
        yield return StartCoroutine(DrawChoiceBox(choices, flavourText, startIndex, defaultChoiceY, defaultChoiceWidth))
            ;
    }

    public IEnumerator DrawChoiceBox(string[] choices, int yPosition, int width)
    {
        yield return
            StartCoroutine(DrawChoiceBox(new string[] {"Yes", "No"}, null, -1, defaultChoiceY, defaultChoiceWidth));
    }

    public IEnumerator DrawChoiceBox(string[] choices, int startIndex, int yPosition, int width)
    {
        yield return
            StartCoroutine(DrawChoiceBox(new string[] {"Yes", "No"}, null, startIndex, defaultChoiceY,
                defaultChoiceWidth));
    }

    public IEnumerator DrawChoiceBox(string[] choices, string[] flavourText, int startIndex, int yPosition, int width)
    {
        if (startIndex < 0)
        {
            startIndex = choices.Length - 1;
        }

        choiceBox.gameObject.SetActive(true);
        choiceBox.sprite = Resources.Load<Sprite>("Frame/choice" + PlayerPrefs.GetInt("frameStyle"));
        choiceBox.rectTransform.localPosition = new Vector3(171 - width - 1, yPosition - 96, 0);
        choiceBox.rectTransform.sizeDelta = new Vector2(width, 16f + (14f * choices.Length));
        choiceBoxSelect.rectTransform.localPosition = new Vector3(8, 9f + (14f * startIndex), 0);
        choiceBoxText.rectTransform.sizeDelta = new Vector2(width - 30, choiceBox.rectTransform.sizeDelta.y);
        choiceBoxTextShadow.rectTransform.sizeDelta = new Vector2(choiceBoxText.rectTransform.sizeDelta.x,
            choiceBoxText.rectTransform.sizeDelta.y);

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

        bool selected = false;
        UpdateChosenIndex(startIndex, choices.Length, flavourText);
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

    private bool UpdateChosenIndex(int newIndex, int choicesLength, string[] flavourText)
    {
        //Check for an invalid new index
        if (newIndex < 0 || newIndex >= choicesLength)
        {
            return false;
        }
        //Even if new index is the same as old, set the graphics in case of needing to override modified graphics.
        choiceBoxSelect.rectTransform.localPosition = new Vector3(8, 9f + (14f * newIndex), 0);
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
        SfxHandler.Play(selectClip);
        return true;
    }


    public void UndrawDialogBox()
    {
        dialogBox.gameObject.SetActive(false);
    }

    public IEnumerator UndrawSignBox()
    {
        float increment = 0f;
        while (increment < 1)
        {
            increment += (1f / 0.2f) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }

            dialogBox.rectTransform.localPosition = new Vector2(dialogBox.rectTransform.localPosition.x,
                -dialogBox.rectTransform.sizeDelta.y * increment);
            yield return null;
        }
        dialogBox.gameObject.SetActive(false);
    }

    public void UndrawChoiceBox()
    {
        choiceBox.gameObject.SetActive(false);
    }
}