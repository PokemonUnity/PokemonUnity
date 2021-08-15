//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class DialogBoxHandler : MonoBehaviour
{
    //Legacy Code. Scenes that use Unity's Canvas GUI System should use DialogBoxHandlerNew

    public enum PrintTextMethod
    {
        Typewriter,
        Instant
    }

    public string DialogBoxString;
    private string[] DialogBoxStringArray;

    private GameObject DialogBox;
    private GUITexture DialogBoxTexture;
    private GUIText DialogBoxText;
    private GUIText DialogBoxTextShadow;
    private GUITexture DialogBoxBorder;

    private GameObject ChoiceBox;
    private GUITexture ChoiceBoxTexture;
    private GUIText ChoiceBoxText;
    private GUIText ChoiceBoxTextShadow;
    private GUITexture ChoiceBoxSelect;

    public AudioClip selectClip;
    private AudioSource DialogAudio;

    private float charPerSec = 60f;
    public float scrollSpeed = 0.1f;

    public int chosenIndex;

    public bool hideDialogOnStart = true;
    public bool hideChoiceOnStart = true;

    public int defaultChoiceWidth = 86;
    public int defaultChoiceY = 0;
    public int defaultDialogLines = 2;

    // Use this for initialization
    void Awake()
    {
        //link the DialogBox variable to the Object
        DialogBox = gameObject.transform.Find("DialogBox").gameObject;
        //link the ChoiceBoxTexture variable to the Object's texture
        DialogBoxTexture = DialogBox.GetComponent<GUITexture>();
        //link the DialogBoxText variable to the Text Component
        DialogBoxText = DialogBox.transform.Find("BoxText").GetComponent<GUIText>();
        //link the DialogBoxTextShadow variable to the Text Component
        DialogBoxTextShadow = DialogBox.transform.Find("BoxTextShadow").GetComponent<GUIText>();
        //link the DialogBoxBorder variable to the Texture Component
        DialogBoxBorder = DialogBox.transform.Find("BoxBorder").GetComponent<GUITexture>();

        //link the ChoiceBox variable to the Object
        ChoiceBox = gameObject.transform.Find("ChoiceBox").gameObject;
        //link the ChoiceBoxTexture variable to the Object's texture
        ChoiceBoxTexture = ChoiceBox.GetComponent<GUITexture>();
        //link the ChoiceBoxText variable to the Text Component
        ChoiceBoxText = ChoiceBox.transform.Find("BoxText").GetComponent<GUIText>();
        //link the ChoiceBoxTextShadow variable to the TextShadow Component
        ChoiceBoxTextShadow = ChoiceBox.transform.Find("BoxTextShadow").GetComponent<GUIText>();
        //link the ChoiceBoxSelect variable to the Texture Component
        ChoiceBoxSelect = ChoiceBox.transform.Find("BoxSelect").GetComponent<GUITexture>();

        DialogAudio = this.gameObject.GetComponent<AudioSource>();

        DialogBoxStringArray = DialogBoxString.Split('\\');

        defaultDialogLines = Mathf.RoundToInt((DialogBoxBorder.pixelInset.height - 16f) / 14f);
        defaultChoiceY = Mathf.FloorToInt(ChoiceBoxTexture.pixelInset.y);
    }

    void Start()
    {
        if (hideDialogOnStart)
        {
            DialogBox.SetActive(false);
        }
        if (hideChoiceOnStart)
        {
            ChoiceBox.SetActive(false);
        }
    }

    public void drawDialogBox()
    {
        drawDialogBox(defaultDialogLines);
    }

    public void drawDialogBox(int lines)
    {
        DialogBox.transform.position = new Vector3(0, 0, DialogBox.transform.position.z);
        DialogBox.SetActive(true);
        DialogBoxBorder.texture = Resources.Load<Texture>("Frame/dialog" + PlayerPrefs.GetInt("frameStyle"));
        DialogBoxTexture.texture = Resources.Load<Texture>("Frame/dialogBG");
        DialogBoxTexture.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        DialogBoxText.text = "";
        DialogBoxText.color = new Color(0.0625f, 0.0625f, 0.0625f, 1f);
        DialogBoxTextShadow.text = "";

        DialogBoxTexture.pixelInset = new Rect(DialogBoxTexture.pixelInset.x, DialogBoxTexture.pixelInset.y,
            DialogBoxTexture.pixelInset.width, Mathf.Round((float) lines * 14f) + 16f);
        DialogBoxBorder.pixelInset = new Rect(DialogBoxBorder.pixelInset.x, DialogBoxBorder.pixelInset.y,
            DialogBoxBorder.pixelInset.width, Mathf.Round((float) lines * 14f) + 16f);
        DialogBoxText.pixelOffset = new Vector2(DialogBoxText.pixelOffset.x, 7f + Mathf.Round((float) lines * 14f));
        DialogBoxTextShadow.pixelOffset = new Vector2(DialogBoxTextShadow.pixelOffset.x,
            6f + Mathf.Round((float) lines * 14f));
    }

    public IEnumerator drawSignBox()
    {
        yield return StartCoroutine(drawSignBox(new Color(0.5f, 0.5f, 0.5f, 1f)));
    }

    public IEnumerator drawSignBox(Color tint)
    {
        DialogBox.transform.position = new Vector3(0, -0.25f, DialogBox.transform.position.z);
        DialogBox.SetActive(true);

        DialogBoxBorder.texture = null;
        DialogBoxTexture.texture = Resources.Load<Texture>("Frame/signBG");
        DialogBoxTexture.color = tint;
        DialogBoxText.text = "";
        DialogBoxText.color = new Color(1f, 1f, 1f, 1f);
        DialogBoxTextShadow.text = "";

        float increment = 0f;
        while (increment < 1)
        {
            increment += (1f / 0.2f) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }
            DialogBox.transform.position = new Vector3(0, -0.25f * (1f - increment), DialogBox.transform.position.z);
            yield return null;
        }
        yield return null;
    }

    public void drawChoiceBox()
    {
        //No parametres means simply Yes/No
        drawChoiceBox(0);
    }

    public void drawChoiceBox(int customYOffset)
    {
        //No other parametres means simply Yes/No
        ChoiceBox.SetActive(true);
        ChoiceBoxTexture.texture = Resources.Load<Texture>("Frame/choice" + PlayerPrefs.GetInt("frameStyle"));
        ChoiceBoxTexture.pixelInset = new Rect(342 - defaultChoiceWidth - 1, defaultChoiceY + customYOffset,
            defaultChoiceWidth, 44f);
        ChoiceBoxSelect.pixelInset = new Rect(342 - defaultChoiceWidth + 7, ChoiceBoxTexture.pixelInset.y + 23f,
            ChoiceBoxSelect.pixelInset.width, ChoiceBoxSelect.pixelInset.height);
        ChoiceBoxText.pixelOffset = new Vector2(342 - defaultChoiceWidth + 21, ChoiceBoxTexture.pixelInset.y + 13f);
        ChoiceBoxTextShadow.pixelOffset = new Vector2(342 - defaultChoiceWidth + 22, ChoiceBoxTexture.pixelInset.y + 12f);
        ChoiceBoxText.text = "Yes \nNo";
        ChoiceBoxTextShadow.text = "Yes \nNo";
    }

    public void drawChoiceBoxNo()
    {
        //set default position to 0
        ChoiceBox.SetActive(true);
        ChoiceBoxTexture.texture = Resources.Load<Texture>("Frame/choice" + PlayerPrefs.GetInt("frameStyle"));
        ChoiceBoxTexture.pixelInset = new Rect(342 - defaultChoiceWidth - 1, defaultChoiceY, defaultChoiceWidth, 44f);
        ChoiceBoxSelect.pixelInset = new Rect(342 - defaultChoiceWidth + 7, ChoiceBoxTexture.pixelInset.y + 9f,
            ChoiceBoxSelect.pixelInset.width, ChoiceBoxSelect.pixelInset.height);
        ChoiceBoxText.pixelOffset = new Vector2(342 - defaultChoiceWidth + 21, ChoiceBoxTexture.pixelInset.y + 13f);
        ChoiceBoxTextShadow.pixelOffset = new Vector2(342 - defaultChoiceWidth + 22, ChoiceBoxTexture.pixelInset.y + 12f);
        ChoiceBoxText.text = "Yes \nNo";
        ChoiceBoxTextShadow.text = "Yes \nNo";
    }

    public void drawChoiceBox(string[] choices)
    {
        ChoiceBox.SetActive(true);
        ChoiceBoxTexture.texture = Resources.Load<Texture>("Frame/choice" + PlayerPrefs.GetInt("frameStyle"));
        ChoiceBoxTexture.pixelInset = new Rect(342 - defaultChoiceWidth - 1, defaultChoiceY, defaultChoiceWidth,
            30f + (14f * (choices.Length - 1)));
        ChoiceBoxSelect.pixelInset = new Rect(342 - defaultChoiceWidth + 7,
            ChoiceBoxTexture.pixelInset.y + 9f + (14f * (choices.Length - 1)), ChoiceBoxSelect.pixelInset.width,
            ChoiceBoxSelect.pixelInset.height);
        ChoiceBoxText.pixelOffset = new Vector2(342 - defaultChoiceWidth + 21, ChoiceBoxTexture.pixelInset.y + 13f);
        ChoiceBoxTextShadow.pixelOffset = new Vector2(342 - defaultChoiceWidth + 22, ChoiceBoxTexture.pixelInset.y + 12f);
        ChoiceBoxText.text = "";
        ChoiceBoxTextShadow.text = "";
        for (int i = 0; i < choices.Length; i++)
        {
            ChoiceBoxText.text += choices[i];
            ChoiceBoxTextShadow.text = ChoiceBoxText.text;
            if (i != choices.Length - 1)
            {
                ChoiceBoxText.text += "\n";
                ChoiceBoxTextShadow.text = ChoiceBoxText.text;
            }
        }
    }

    public void drawChoiceBoxWidth(string[] choices, int width)
    {
        ChoiceBox.SetActive(true);
        ChoiceBoxTexture.texture = Resources.Load<Texture>("Frame/choice" + PlayerPrefs.GetInt("frameStyle"));
        ChoiceBoxTexture.pixelInset = new Rect(342 - width - 1, defaultChoiceY, width,
            30f + (14f * (choices.Length - 1)));
        ChoiceBoxSelect.pixelInset = new Rect(342 - width + 7,
            ChoiceBoxTexture.pixelInset.y + 9f + (14f * (choices.Length - 1)), ChoiceBoxSelect.pixelInset.width,
            ChoiceBoxSelect.pixelInset.height);
        ChoiceBoxText.pixelOffset = new Vector2(342 - width + 21, ChoiceBoxTexture.pixelInset.y + 13f);
        ChoiceBoxTextShadow.pixelOffset = new Vector2(342 - width + 22, ChoiceBoxTexture.pixelInset.y + 12f);
        ChoiceBoxText.text = "";
        ChoiceBoxTextShadow.text = "";
        for (int i = 0; i < choices.Length; i++)
        {
            ChoiceBoxText.text += choices[i];
            ChoiceBoxTextShadow.text = ChoiceBoxText.text;
            if (i != choices.Length - 1)
            {
                ChoiceBoxText.text += "\n";
                ChoiceBoxTextShadow.text = ChoiceBoxText.text;
            }
        }
    }


    public void undrawDialogBox()
    {
        DialogBox.SetActive(false);
    }

    public IEnumerator undrawSignBox()
    {
        float increment = 0f;
        while (increment < 1)
        {
            increment += (1f / 0.2f) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }
            DialogBox.transform.position = new Vector3(0, -0.25f * increment, DialogBox.transform.position.z);
            yield return null;
        }
        DialogBox.SetActive(false);
    }

    public void undrawChoiceBox()
    {
        ChoiceBox.SetActive(false);
    }


    private IEnumerator drawChar(char character, float secPerChar)
    {
        if (character.Equals('\\'))
        {
            //   \ is used to designate line breaks
            DialogBoxText.text += "\n";
            DialogBoxTextShadow.text = DialogBoxText.text;
        }
        else
        {
            DialogBoxText.text += character.ToString();
            DialogBoxTextShadow.text = DialogBoxText.text;
        }
        yield return new WaitForSeconds(0.000001f); //wait for (seconds per character print) before printing the next
    }

    public IEnumerator drawText(string textLine)
    {
        SfxHandler.Play(selectClip);
        int textSpeed = PlayerPrefs.GetInt("textSpeed") + 1;
        charPerSec = 16 + (textSpeed * textSpeed * 9);
        float secPerChar = 1 / charPerSec;
        //split textLine into an array of each character, so it may be printed 1 bit at a time
        char[] chars = textLine.ToCharArray();
        for (int i = 0; i < textLine.Length; i++)
        {
            if (chars[i] == '{')
            {
                //extended operator
                if (chars[i + 1] == 'p' || chars[i + 1] == 'P')
                {
                    //player name
                    i += 1; //adjust for the extra character in the operator (e.g. "{P" )
                    char[] pChars = SaveData.currentSave.savefile.playerName.ToCharArray();
                    for (int i2 = 0; i2 < pChars.Length; i2++)
                    {
                        yield return StartCoroutine(drawChar(pChars[i2], secPerChar));
                    }
                }
            }
            else
            {
                yield return StartCoroutine(drawChar(chars[i], secPerChar));
            }
        }
    }

    public IEnumerator drawTextSilent(string textLine)
    {
        int textSpeed = PlayerPrefs.GetInt("textSpeed") + 1;
        charPerSec = 16 + (textSpeed * textSpeed * 9);
        float secPerChar = 1 / charPerSec;
        //split textLine into an array of each character, so it may be printed 1 bit at a time
        char[] chars = textLine.ToCharArray();
        for (int i = 0; i < textLine.Length; i++)
        {
            if (chars[i].Equals('\\'))
            {
                //   \ is used to designate line breaks
                DialogBoxText.text += "\n";
                DialogBoxTextShadow.text = DialogBoxText.text;
            }
            else
            {
                DialogBoxText.text += chars[i].ToString();
                DialogBoxTextShadow.text = DialogBoxText.text;
            }
            if (Time.deltaTime < secPerChar)
            {
                yield return new WaitForSeconds(secPerChar);
            } //wait for (seconds per character print) before printing the next
            else
            {
                i += 1;
                if (i < textLine.Length)
                {
                    //if not at the end, repeat and wait double time
                    if (chars[i].Equals('\\'))
                    {
                        //   \ is used to designate line breaks
                        DialogBoxText.text += "\n";
                        DialogBoxTextShadow.text = DialogBoxText.text;
                    }
                    else
                    {
                        DialogBoxText.text += chars[i].ToString();
                        DialogBoxTextShadow.text = DialogBoxText.text;
                    }
                    yield return new WaitForSeconds(secPerChar * 2);
                }
            }
        }
    }

    public void drawTextInstant(string textLine)
    {
        //split textLine into an array of each character, so it may be printed 1 bit at a time
        char[] chars = textLine.ToCharArray();
        for (int i = 0; i < textLine.Length; i++)
        {
            if (chars[i].Equals('\\'))
            {
                //   \ is used to designate line breaks
                DialogBoxText.text += "\n";
                DialogBoxTextShadow.text = DialogBoxText.text;
            }
            else
            {
                DialogBoxText.text += chars[i].ToString();
                DialogBoxTextShadow.text = DialogBoxText.text;
            }
        }
    }

    public IEnumerator scrollText()
    {
        float textPosDestination = DialogBoxText.pixelOffset.y + 14f; //the box must be scrolled up 14
        while (DialogBoxText.pixelOffset.y < textPosDestination)
        {
            if (Mathf.RoundToInt(DialogBoxText.pixelOffset.y) == textPosDestination - 5)
            {
                //if text is about to spill over the top
                string[] textMod = DialogBoxText.text.Split("\n"[0]); //remove the top line of text.
                DialogBoxText.text = textMod[1] + "\n" + textMod[2];
                    //this involves splitting the string by it's line breaks
                DialogBoxTextShadow.text = DialogBoxText.text;
                textPosDestination -= 14f; //reduce destination and position by 14 to account for the removed line. 
                DialogBoxText.pixelOffset = new Vector2(DialogBoxText.pixelOffset.x, DialogBoxText.pixelOffset.y - 14f);
                DialogBoxTextShadow.pixelOffset = new Vector2(DialogBoxTextShadow.pixelOffset.x,
                    DialogBoxTextShadow.pixelOffset.y - 14f);
            }
            DialogBoxText.pixelOffset = new Vector2(DialogBoxText.pixelOffset.x, DialogBoxText.pixelOffset.y + 1f);
            DialogBoxTextShadow.pixelOffset = new Vector2(DialogBoxTextShadow.pixelOffset.x,
                DialogBoxTextShadow.pixelOffset.y + 1f);
            yield return new WaitForSeconds(scrollSpeed / 14);
        }
        yield return null;
    }


    public IEnumerator choiceNavigate()
    {
        //No parametres means simply Yes/No
        chosenIndex = 1; //0 is the vertically lowest choice
        bool selected = false;
        while (!selected)
        {
            if (Input.GetButtonDown("Select"))
            {
                selected = true;
            }
            else if (Input.GetButtonDown("Back"))
            {
                while (chosenIndex > 0)
                {
                    chosenIndex -= 1;
                    ChoiceBoxSelect.pixelInset = new Rect(ChoiceBoxSelect.pixelInset.x,
                        ChoiceBoxSelect.pixelInset.y - 14f, ChoiceBoxSelect.pixelInset.width,
                        ChoiceBoxSelect.pixelInset.height);
                }
                SfxHandler.Play(selectClip);
                yield return new WaitForSeconds(0.2f);
                selected = true;
            }
            else
            {
                if (chosenIndex < 1)
                {
                    if (Input.GetAxisRaw("Vertical") > 0)
                    {
                        chosenIndex += 1;
                        ChoiceBoxSelect.pixelInset = new Rect(ChoiceBoxSelect.pixelInset.x,
                            ChoiceBoxSelect.pixelInset.y + 14f, ChoiceBoxSelect.pixelInset.width,
                            ChoiceBoxSelect.pixelInset.height);
                        SfxHandler.Play(selectClip);
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                if (chosenIndex > 0)
                {
                    if (Input.GetAxisRaw("Vertical") < 0)
                    {
                        chosenIndex -= 1;
                        ChoiceBoxSelect.pixelInset = new Rect(ChoiceBoxSelect.pixelInset.x,
                            ChoiceBoxSelect.pixelInset.y - 14f, ChoiceBoxSelect.pixelInset.width,
                            ChoiceBoxSelect.pixelInset.height);
                        SfxHandler.Play(selectClip);
                        yield return new WaitForSeconds(0.2f);
                    }
                }
            }
            yield return null;
        }
    }

    public IEnumerator choiceNavigateNo()
    {
        //For when No needs to be default;
        chosenIndex = 0;
        bool selected = false;
        while (!selected)
        {
            if (Input.GetButtonDown("Select"))
            {
                selected = true;
            }
            else if (Input.GetButtonDown("Back"))
            {
                while (chosenIndex > 0)
                {
                    chosenIndex -= 1;
                    ChoiceBoxSelect.pixelInset = new Rect(ChoiceBoxSelect.pixelInset.x,
                        ChoiceBoxSelect.pixelInset.y - 14f, ChoiceBoxSelect.pixelInset.width,
                        ChoiceBoxSelect.pixelInset.height);
                }
                SfxHandler.Play(selectClip);
                yield return new WaitForSeconds(0.2f);
                selected = true;
            }
            else
            {
                if (chosenIndex < 1)
                {
                    if (Input.GetAxisRaw("Vertical") > 0)
                    {
                        chosenIndex += 1;
                        ChoiceBoxSelect.pixelInset = new Rect(ChoiceBoxSelect.pixelInset.x,
                            ChoiceBoxSelect.pixelInset.y + 14f, ChoiceBoxSelect.pixelInset.width,
                            ChoiceBoxSelect.pixelInset.height);
                        SfxHandler.Play(selectClip);
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                if (chosenIndex > 0)
                {
                    if (Input.GetAxisRaw("Vertical") < 0)
                    {
                        chosenIndex -= 1;
                        ChoiceBoxSelect.pixelInset = new Rect(ChoiceBoxSelect.pixelInset.x,
                            ChoiceBoxSelect.pixelInset.y - 14f, ChoiceBoxSelect.pixelInset.width,
                            ChoiceBoxSelect.pixelInset.height);
                        SfxHandler.Play(selectClip);
                        yield return new WaitForSeconds(0.2f);
                    }
                }
            }
            yield return null;
        }
    }

    public IEnumerator choiceNavigate(string[] choices)
    {
        chosenIndex = choices.Length - 1; //0 is the vertically lowest choice
        bool selected = false;
        while (!selected)
        {
            if (Input.GetButtonDown("Select"))
            {
                selected = true;
            }
            else if (Input.GetButtonDown("Back"))
            {
                while (chosenIndex > 0)
                {
                    chosenIndex -= 1;
                    ChoiceBoxSelect.pixelInset = new Rect(ChoiceBoxSelect.pixelInset.x,
                        ChoiceBoxSelect.pixelInset.y - 14f, ChoiceBoxSelect.pixelInset.width,
                        ChoiceBoxSelect.pixelInset.height);
                }
                SfxHandler.Play(selectClip);
                yield return new WaitForSeconds(0.2f);
                selected = true;
            }
            else
            {
                if (chosenIndex < choices.Length - 1)
                {
                    if (Input.GetAxisRaw("Vertical") > 0)
                    {
                        chosenIndex += 1;
                        ChoiceBoxSelect.pixelInset = new Rect(ChoiceBoxSelect.pixelInset.x,
                            ChoiceBoxSelect.pixelInset.y + 14f, ChoiceBoxSelect.pixelInset.width,
                            ChoiceBoxSelect.pixelInset.height);
                        SfxHandler.Play(selectClip);
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                if (chosenIndex > 0)
                {
                    if (Input.GetAxisRaw("Vertical") < 0)
                    {
                        chosenIndex -= 1;
                        ChoiceBoxSelect.pixelInset = new Rect(ChoiceBoxSelect.pixelInset.x,
                            ChoiceBoxSelect.pixelInset.y - 14f, ChoiceBoxSelect.pixelInset.width,
                            ChoiceBoxSelect.pixelInset.height);
                        SfxHandler.Play(selectClip);
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                yield return null;
            }
        }
    }

    //When the textBox should display flavour text for each choice.
    public IEnumerator choiceNavigate(string[] choices, string[] flavour)
    {
        chosenIndex = choices.Length - 1; //0 is the vertically lowest choice
        bool selected = false;
        while (!selected)
        {
            if (Input.GetButtonDown("Select"))
            {
                selected = true;
            }
            else if (Input.GetButtonDown("Back"))
            {
                while (chosenIndex > 0)
                {
                    chosenIndex -= 1;
                    ChoiceBoxSelect.pixelInset = new Rect(ChoiceBoxSelect.pixelInset.x,
                        ChoiceBoxSelect.pixelInset.y - 14f, ChoiceBoxSelect.pixelInset.width,
                        ChoiceBoxSelect.pixelInset.height);
                }
                drawDialogBox();
                drawTextInstant(flavour[flavour.Length - 1 - chosenIndex]);
                SfxHandler.Play(selectClip);
                yield return new WaitForSeconds(0.2f);
                selected = true;
            }
            else
            {
                if (chosenIndex < choices.Length - 1)
                {
                    if (Input.GetAxisRaw("Vertical") > 0)
                    {
                        chosenIndex += 1;
                        ChoiceBoxSelect.pixelInset = new Rect(ChoiceBoxSelect.pixelInset.x,
                            ChoiceBoxSelect.pixelInset.y + 14f, ChoiceBoxSelect.pixelInset.width,
                            ChoiceBoxSelect.pixelInset.height);
                        SfxHandler.Play(selectClip);
                        drawDialogBox();
                        drawTextInstant(flavour[flavour.Length - 1 - chosenIndex]);
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                if (chosenIndex > 0)
                {
                    if (Input.GetAxisRaw("Vertical") < 0)
                    {
                        chosenIndex -= 1;
                        ChoiceBoxSelect.pixelInset = new Rect(ChoiceBoxSelect.pixelInset.x,
                            ChoiceBoxSelect.pixelInset.y - 14f, ChoiceBoxSelect.pixelInset.width,
                            ChoiceBoxSelect.pixelInset.height);
                        SfxHandler.Play(selectClip);
                        drawDialogBox();
                        drawTextInstant(flavour[flavour.Length - 1 - chosenIndex]);
                        yield return new WaitForSeconds(0.2f);
                    }
                }
            }
            yield return null;
        }
    }
}