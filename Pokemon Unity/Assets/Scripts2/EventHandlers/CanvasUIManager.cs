using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CanvasUIHandler : UnityEngine.MonoBehaviour
{
    public CanvasUI CurrentCanvas;
    public UICanvas ActiveCanvasUI { get; private set; }
	/// <summary>
	/// This is text-script meant to be rendered by front-end UI
	/// </summary>
    private string DialogText { get; set; }
	/// <summary>
	/// This is the text that's displayed to user on front-end.
	/// The text is properly formatted for real-time rendering display in Unity.
	/// </summary>
    private static UnityEngine.UI.Text DialogUIText { get; set; }// = UnityEngine.GameObject.Find("DialogText").GetComponent<UnityEngine.UI.Text>();
	/// <summary>
	/// This is the text that's hidden from user, and is used in background processing.
	/// The text is the final rendered display of the text properly formatted for UI in Unity
	/// </summary>
	private static UnityEngine.UI.Text DialogUITextDump { get; set; }// = UnityEngine.GameObject.Find("DialogTextDump").GetComponent<UnityEngine.UI.Text>();
    private static UnityEngine.UI.Text DialogUIScrollText;// = UnityEngine.GameObject.Find("DialogScrollText").GetComponent<UnityEngine.UI.Text>();
    private static bool InstantLine; //ToDo: Global Bool => GameVariables.Instant, maybe?
    private static float secPerChar
    {
        get 
        {
            int txtSpd = GameVariables.textSpeed + 1;
            return 1 / (16 + (txtSpd * txtSpd * 9));
        }
    }

	#region Unity Stuff
	void Awake()
    {
		GameVariables.SetCanvasManager(this);
        DialogUIText = UnityEngine.GameObject.Find("DialogText").GetComponent<UnityEngine.UI.Text>();
        DialogUITextDump = UnityEngine.GameObject.Find("DialogTextDump").GetComponent<UnityEngine.UI.Text>();
        DialogUIScrollText = UnityEngine.GameObject.Find("DialogScrollText").GetComponent<UnityEngine.UI.Text>();
    }

    public void Start()
    {
        ShowMenu(CurrentCanvas);
    }

    /// <summary>
    /// Not needed, just wanted to test features...
    /// </summary>
    void Update()
    {
        //Test dialog skin
        //if (UnityEngine.Input.anyKeyDown) RefreshWindowSkin();
    }

    public void ShowMenu(CanvasUI screen)
    {
        if (CurrentCanvas != null) CurrentCanvas.IsActive = false;

        CurrentCanvas = screen;
        CurrentCanvas.IsActive = true;
    }
	#endregion

    #region Enumerator
    public enum UICanvas
    {
        NONE,
        /// <summary>
        /// DialogTextWindow:
        /// This is the Text Window for all dialog used throughout game 
        /// (between the Player and the Game), regardless of their purpose
        /// </summary>
        DialogWindow,
        /// <summary>
        /// StartupMenu:
        /// The main menu that occurs right after the StartupScene/Sequence
        /// </summary>
        MainMenu,
        /// <summary>
        /// StartupMenu_Settings:
        /// The settings option located on the <seealso cref="MainMenu"/> Screen
        /// </summary>
        SettingsMenu
    }
	#endregion

	#region Methods
	/// <summary>
	/// Reassigns all the <seealso cref="UnityEngine.Sprite"/> images 
	/// for every <seealso cref="UnityEngine.GameObject"/> with the tag "DialogWindow"
	/// </summary>
	public static void RefreshWindowSkin()
    {
        if (GameVariables.WindowSkinSprite != null)
        {
            UnityEngine.GameObject[] gos = UnityEngine.GameObject.FindGameObjectsWithTag("DialogWindow");
            foreach (UnityEngine.GameObject go in gos)
            {
                go.GetComponent<UnityEngine.UI.Image>().sprite = GameVariables.WindowSkinSprite;
            }
        }
        else return;
    }

    /// <summary>
    /// Reassigns all the <seealso cref="UnityEngine.Sprite"/> images 
    /// for every <seealso cref="UnityEngine.GameObject"/> with the tag "DialogWindow"
    /// </summary>
    public static void RefreshDialogSkin()
    {
        if (GameVariables.DialogSkinSprite != null)
        {
            UnityEngine.GameObject[] gos = UnityEngine.GameObject.FindGameObjectsWithTag("DialogWindow");
            foreach (UnityEngine.GameObject go in gos)
            {
                go.GetComponent<UnityEngine.UI.Image>().sprite = GameVariables.DialogSkinSprite;
            }
        }
        else return;
    }

    void ChangeScene(UICanvas scene)
    {
        if (ActiveCanvasUI == scene) return;
        else
        {
            UnityEngine.CanvasGroup cgroup;
            UnityEngine.GameObject[] gos = UnityEngine.GameObject.FindGameObjectsWithTag("CanvasUI");
            switch (scene)
            {
                case UICanvas.DialogWindow:
                    foreach(UnityEngine.GameObject go in gos)
                    {
                        cgroup = go.GetComponent<UnityEngine.CanvasGroup>();
                        if (go.name == scene.ToString())
                        {
                            go.SetActive(true);
                            ActiveCanvasUI = scene;
                            cgroup.blocksRaycasts = cgroup.interactable = true;
                        }
                        else
                        {
                            go.SetActive(false);
                            cgroup.blocksRaycasts = cgroup.interactable = false;
                        }
                    }
                    break;
                case UICanvas.MainMenu:
                case UICanvas.SettingsMenu:
                    foreach(UnityEngine.GameObject go in gos)
                    {
                        if (go.name == scene.ToString())
                        {
                            go.SetActive(true);
                            ActiveCanvasUI = scene;
                            cgroup = go.GetComponent<UnityEngine.CanvasGroup>();
                            cgroup.blocksRaycasts = cgroup.interactable = true;
                        }
                        else go.SetActive(false);
                    }
                    break;
                case UICanvas.NONE:
                default:
                    foreach(UnityEngine.GameObject go in gos)
                    {
                        go.SetActive(false);
                    }
                    //ClearDialogText();
                    break;
            }
        }
    }
    public IEnumerator<byte?> DisplayDialogMessage(string message, params string[] promptOptions)
    {
        //Pause player/lock controller actions
        ChangeScene(UICanvas.DialogWindow);
        //DialogText = message.Trim("\r\n".ToCharArray());
        string[] text = message.Trim("\r\n".ToCharArray()).Split(new string[] { "\r" }, StringSplitOptions.RemoveEmptyEntries);
        for(int i = 0; i < text.Length; i++)
        {
            //At the start of every message, turn off the bouncing cursor 
            //GameObject.Find...SetActive(false)
            //foreach (string line in text[i].Split(new string[] { "\n" }, StringSplitOptions.None)){}
            //DialogText = text[i].Replace("\n", " "+Environment.NewLine);
            InstantLine = false;
			//StartCoroutine(SetTextRoutine(text[i].Replace("\n", " " + Environment.NewLine)));
			//UpdateTextLinesFrmDump();
			IEnumerator e = SetTextRoutine(text[i].Replace("\n", " " + Environment.NewLine));
			while (e.MoveNext())
			{
				// If they press 'Escape', skip the cutscene
				//if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Escape))
				if (UnityEngine.Input.anyKey)
				{
					InstantLine = true;
					//break;
				}
			}
			//turn on the bouncing cursor to represent waiting on user's response/action
			//to continue the remaining dialog message
			if(i == text.Length) //Or add a `If anyKey` when text == "\r" 
			{
				//Set bouncing cursor to true
			}
			//if(i < text.Length && text.Length > 1) GameObject.Find...SetActive(true)
			//wait for user input before continueing to the next set of text...
			//Yield
		}
		while (UnityEngine.Input.anyKey)
		{
			yield return null;
		}
        if (promptOptions == null)
        {
            //Display Yes/No prompt
            //return int value based on user prompt selection
            yield return 1;
        }
        else
        {
			//Display custom prompt options using stringArray
			//return int value based on user prompt select
			for (int i = 0; i < promptOptions.Length; i++)
			{
				//Create Text Option by duplicating Child GameObject
				//promptOptions[i]
				//Setting display to true, setting Component<OnSubmit>.Value to `i` 
			}
            yield return 0;
        }
        //Disable the dialog window
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="prompt"></param>
    /// <param name="promptOptions"></param>
    /// <returns></returns>
    /// ToDo: Add a Param to adjust the width of the DialogPromptWindow
    public IEnumerator<int> DisplayDialogMessage(string message, bool prompt = false, params string[] promptOptions)
    {
        //Pause player/lock controller actions
        ChangeScene(UICanvas.DialogWindow);
        //DialogText = message.Trim("\r\n".ToCharArray());
        string[] text = message.Trim("\r\n".ToCharArray()).Split(new string[] { "\r" }, StringSplitOptions.RemoveEmptyEntries);
        for(int i = 0; i < text.Length; i++)
        {
            //At the start of every message, turn off the bouncing cursor 
            //GameObject.Find...SetActive(false)
            //foreach (string line in text[i].Split(new string[] { "\n" }, StringSplitOptions.None)){}
            //DialogText = text[i].Replace("\n", " "+Environment.NewLine);
            InstantLine = false;
			//StartCoroutine(SetTextRoutine(text[i].Replace("\n", " " + Environment.NewLine)));
			//UpdateTextLinesFrmDump();
			IEnumerator e = SetTextRoutine(text[i].Replace("\n", " " + Environment.NewLine));
			while (e.MoveNext())
			{
				// If they press 'Escape', skip the cutscene
				//if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Escape))
				if (UnityEngine.Input.anyKey)
				{
					InstantLine = true;
					//break;
				}
			}
			//turn on the bouncing cursor to represent waiting on user's response/action
			//to continue the remaining dialog message
			//if(i < text.Length && text.Length > 1) GameObject.Find...SetActive(true)
			//wait for user input before continueing to the next set of text...
			//Yield
		}
        if (prompt)
        {
            if (promptOptions == null)
            {
                //Display Yes/No prompt
                //return int value based on user prompt selection
                yield return 1;
            }
            else
            {
                //Display custom prompt options using stringArray
                //return int value based on user prompt select
                yield return 0;
            }
        }
        else yield return -1;
        //Disable the dialog window
    }
	public void drawTextInstant(string textLine)
    {
        //split textLine into an array of each character, so it may be printed 1 bit at a time
        char[] chars = textLine.ToCharArray();
        for (int i = 0; i < textLine.Length; i++)
        {
            /*if (chars[i].Equals('\\'))
            {
                //   \ is used to designate line breaks
                DialogBoxText.text += "\n";
                DialogBoxTextShadow.text = DialogBoxText.text;
            }
            else
            {*/
            //    DialogBoxText.text += chars[i].ToString();
            //  DialogBoxTextShadow.text = DialogBoxText.text;
            //}
        }
    }

    /*public IEnumerator drawText(string textLine)
    {
        //SfxHandler.Play(selectClip);
        //split textLine into an array of each character, so it may be printed 1 bit at a time
        char[] chars = textLine.ToCharArray();
        for (int i = 0; i < textLine.Length; i++)
        {
            //If back button pressed, complete the string without scrolling
            //wait for (seconds per character print) before printing the next
            //yield return new UnityEngine.WaitForSeconds(secPerChar);
            //yield return new UnityEngine.WaitForSeconds(secPerChar * 2);
            yield return StartCoroutine(drawChar(chars[i]));
        }
    }

    private IEnumerator drawChar(char character, bool richtext = false)
    {
        //DialogBoxText.text += character.ToString();
        UnityEngine.GameObject.Find("DialogText").GetComponent<UnityEngine.UI.Text>().text += character.ToString();
        //wait for (seconds per character print) before printing the next
        //if (richtext) yield return;
        //else
            yield return new UnityEngine.WaitForSeconds(secPerChar);//(0.000001f); 
    }*/

    public IEnumerator SetTextRoutine(string text)
    {
        // reset the paragraph text
        //DialogUIText.text = string.Empty;
        ClearDialogText();

        // keep local start and end tag variables 
        string startTag = string.Empty;
        string endTag = string.Empty;

        for (int i = 0; i < text.Length; i++)
        {
            //if (UnityEngine.Input.anyKeyDown)
            //{
            //    InstantLine = true;
            //}

            char c = text[i];

            #region RichText 
            // check to see if we're starting a tag
            if (c == '<')
            {
                // make sure we don't already have a starting tag
                // don't check for ending tag because we set these variables at the 
                // same time
                if (string.IsNullOrEmpty(startTag))
                {
                    // store the current index 
                    int currentIndex = i;

                    for (int j = currentIndex; j < text.Length; j++)
                    {
                        // add to our starting tag
                        startTag += text[j].ToString();

                        // check to see if we're going to end the tag
                        if (text[j] == '>')
                        {
                            // set our current index to the end of the tag
                            currentIndex = j;
                            // set our letter starting point to the current index (when we continue this will be currentIndex++)
                            i = currentIndex;

                            // find the end tag that goes with this tag
                            for (int k = currentIndex; k < text.Length; k++)
                            {
                                char next = text[k];

                                // check to see if we've reached our end tags start point
                                if (next == '<')
                                    break;

                                // if we have not increment currentindex
                                currentIndex++;
                            }
                            break;
                        }
                    }

                    // we start at current index since this is where our ending tag starts
                    for (int j = currentIndex; j < text.Length; j++)
                    {
                        // add to the ending tag
                        endTag += text[j].ToString();

                        // once the ending tag is finished we break out
                        if (text[j] == '>')
                        {
                            break;
                        }
                    }
                }
                else
                {
                    // go through the text and move past the ending tag
                    for (int j = i; j < text.Length; j++)
                    {
                        if (text[j] == '>')
                        {
                            // set i = j so we can start at the position of the next letter
                            i = j;
                            break;
                        }
                    }
                    // we reset our starting and ending tag
                    startTag = string.Empty;
                    endTag = string.Empty;
                }

                // continue to get the next character in the sequence
                continue;

            }
            #endregion

            DialogUITextDump.text += string.Format("{0}{1}{2}", startTag, c, endTag);

            UpdateTextLinesFrmDump();
            if (InstantLine) continue; else

            yield return new UnityEngine.WaitForSeconds(secPerChar);
        }
		//yield return null;
    }
    /// <summary>
    /// [Deprecated] Consider Scrolling Text Dialog.
    /// When text exeeds the dialog box limits (of 2 lines)
    /// the code will remove the first line and move everything up
    /// </summary>
    private void UpdateTextLines()
    {
        //UnityEngine.Canvas.ForceUpdateCanvases();
        string[] lines = new string[3];
        for(int i = 0; i < DialogUIText.cachedTextGenerator.lineCount; i++)
        {
            int startIndex = DialogUIText.cachedTextGenerator.lines[i].startCharIdx;
            int endIndex = (i == DialogUIText.cachedTextGenerator.lines.Count - 1)? DialogUIText.text.Length : DialogUIText.cachedTextGenerator.lines[i + 1].startCharIdx;
            int length = endIndex - startIndex;
            //UnityEngine.Debug.Log(DialogUIText.text.Substring(startIndex, length));
            lines[i] = DialogUIText.text.Substring(startIndex, length);
        }
        if (DialogUIText.cachedTextGenerator.lineCount > 2)
        {
            lines[0] = lines[1];
            lines[1] = lines[2]??string.Empty;
            lines[2] = null;
        }
        //string newText = lines[0] + Environment.NewLine + lines[1]; //+ Environment.NewLine;
        DialogUIText.text = lines[0] + Environment.NewLine + lines[1] + Environment.NewLine;
    }
    private void UpdateTextLinesFrmDump()
    {
        //UnityEngine.Canvas.ForceUpdateCanvases();
        string[] lines = new string[3];
        for(int i = 0; i < DialogUITextDump.cachedTextGenerator.lineCount; i++)
        {
            int startIndex = DialogUITextDump.cachedTextGenerator.lines[i].startCharIdx;
            int endIndex = (i == DialogUITextDump.cachedTextGenerator.lines.Count - 1)? DialogUITextDump.text.Length : DialogUITextDump.cachedTextGenerator.lines[i + 1].startCharIdx;
            int length = endIndex - startIndex;
            //UnityEngine.Debug.Log(DialogUIText.text.Substring(startIndex, length));
            lines[i] = DialogUITextDump.text.Substring(startIndex, length);
        }
        if (DialogUITextDump.cachedTextGenerator.lineCount > 2)
        {
            lines[0] = lines[1];
            lines[1] = lines[2]??string.Empty;
            lines[2] = null;
        }
        //string newText = lines[0] + Environment.NewLine + lines[1]; //+ Environment.NewLine;
        DialogUITextDump.text = lines[0] + Environment.NewLine + lines[1];
        //Use IndexOf and PrivatePropertyVariable to create placeholders to pop in {} start/end dynamic text
        DialogUIText.text = DialogUITextDump.text + Environment.NewLine;
    }
    private void UpdateScrollTextLines()
    {
        //UnityEngine.Canvas.ForceUpdateCanvases();
        /*string[] lines = new string[3];
        for(int i = 0; i < DialogUIScrollText.cachedTextGenerator.lineCount; i++)
        {
            int startIndex = DialogUIScrollText.cachedTextGenerator.lines[i].startCharIdx;
            int endIndex = (i == DialogUIScrollText.cachedTextGenerator.lines.Count - 1)? DialogUIScrollText.text.Length : DialogUIScrollText.cachedTextGenerator.lines[i + 1].startCharIdx;
            int length = endIndex - startIndex;
            //UnityEngine.Debug.Log(DialogUIText.text.Substring(startIndex, length));
            lines[i] = DialogUIScrollText.text.Substring(startIndex, length);
        }
        if (DialogUIText.cachedTextGenerator.lineCount > 2)
        {
            lines[0] = lines[1];
            lines[1] = lines[2];
            lines[2] = null;
        }*/
        DialogUIScrollText.rectTransform.sizeDelta = new UnityEngine.Vector2(DialogUIScrollText.rectTransform.sizeDelta.x, 15 * DialogUIScrollText.cachedTextGenerator.lineCount);
    } 
    private void ClearDialogText()
    {
        DialogUIText.text = DialogUITextDump.text = DialogUIScrollText.text = DialogText = null; 
    }
	#endregion

    public class CanvasUI : UnityEngine.MonoBehaviour
    {
        private UnityEngine.GameObject _canvasScene;
        private UnityEngine.CanvasGroup _canvasGroup;
        private UnityEngine.Animator _animator;
        //public UICanvas CanvasUITag { get; private set; }

        private bool _isActive
        {
            get
            {
                return _canvasScene.activeSelf;
            }
            set
            {
                _canvasScene.SetActive(value);
            }
        }
        public bool IsActive
        {
            get
            {
                return _animator.GetBool("");
            }
            set
            {
                _animator.SetBool("", value);
            }
        }

        public void Awake()
        {
            _canvasScene = gameObject;
            _animator = GetComponent<UnityEngine.Animator>();
        }

        public void Update()
        {
            //if (!_animator.GetCurrentAnimatorStateInfo(0).IsName(""))
            //    _canvasGroup.blocksRaycasts = _canvasGroup.interactable = false;
            //else _canvasGroup.blocksRaycasts = _canvasGroup.interactable = true;
        }
    }
}