using PokemonUnity.Overworld.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class DialogHandler : UnityEngine.MonoBehaviour
{
    #region Variables and Properties
    public string DialogText {
        get { return DialogUITextDump.text; }
        set {
            DialogUITextDump.text = value;
            //Whenever text field is set
            //Enable Canvas Layer
            //Begin text animation
            
            StopCoroutine("SetTextRoutine");
            StartCoroutine("SetTextRoutine");
        }
    }
    private static UnityEngine.GameObject DialogWindow;
    public static UnityEngine.GameObject[] DialogPrompt { get; private set; }
    //public DialogPromptHandler[] dialogPromptHandler;
    private static UnityEngine.GameObject DialogPromptDump;
    private static UnityEngine.GameObject DialogPromptWindow;
    private static UnityEngine.UI.Text DialogUIText;// = UnityEngine.GameObject.Find("DialogText").GetComponent<UnityEngine.UI.Text>();
    private static UnityEngine.UI.Text DialogUITextDump;// = UnityEngine.GameObject.Find("DialogTextDump").GetComponent<UnityEngine.UI.Text>();
    private static UnityEngine.UI.Text DialogUIScrollText;// = UnityEngine.GameObject.Find("DialogScrollText").GetComponent<UnityEngine.UI.Text>();
    public static bool InstantLine { get; set; }
    public bool InstantLine2 { get; set; }
    public bool InstantLine3 { get; set; }
    private int temp;
    private static float secPerChar
    {
        get { return secPerChar; }
        set
        {
            int txtSpd = GameVariables.textSpeed + 1;
            value = 1 / (16 + (txtSpd * txtSpd * 9));
        }
    }
	#endregion

	#region Unity
	void Awake()
    {
        DialogWindow = UnityEngine.GameObject.Find("DialogWindow");//.GetComponent<UnityEngine.UI.Text>();
        //DialogPrompt = UnityEngine.GameObject.Find("DialogPrompt");//.GetComponent<UnityEngine.UI.Text>();
        DialogPromptDump = UnityEngine.GameObject.Find("DialogPromptWindow_");//.GetComponent<UnityEngine.UI.Text>();
        DialogPromptWindow = UnityEngine.GameObject.Find("DialogPromptWindow");//.GetComponent<UnityEngine.UI.Text>();
        DialogUIText = UnityEngine.GameObject.Find("DialogText").GetComponent<UnityEngine.UI.Text>();
        DialogUITextDump = UnityEngine.GameObject.Find("DialogTextDump").GetComponent<UnityEngine.UI.Text>();
        DialogUIScrollText = UnityEngine.GameObject.Find("DialogScrollText").GetComponent<UnityEngine.UI.Text>();
    }

    void OnEnable()
    {
        //Every time the dialog gameobject is made visible
        //It'll load the dialogborder image stored in gamevariables
        //It'll load the text stored in the variable
        //If text is null or empty, it'll disable itself
        gameObject.SetActive(string.IsNullOrEmpty(DialogText));
        UnityEngine.GameObject.Find("DialogText").SetActive(string.IsNullOrEmpty(DialogText));
        //After text is loaded, then it pastes the text in dump window
        DialogUITextDump.text = DialogText ?? string.Empty;
        //After text is paste, it animates the text in display window
        //StartCoroutine();
    }

    void Start()
    {

    }

    void Update()
    {

    }
	#endregion

	#region Prompts
    public void PromptYesNo()
    {
        PromptOptions(new string[] { LanguageExtension.Translate(GameVariables.UserLanguage, "Yes"), LanguageExtension.Translate(GameVariables.UserLanguage, "No") });
    }

    public void PromptOptions(string options)
    {
        PromptOptions(options.Split(';'));
    }

    public void PromptOptions(string[] options)
    {
        //UnityEngine.GameObject dialogpromptDump = UnityEngine.GameObject.Find("DialogPromptWindow_");
        //Clear previous options
        DialogPromptDump.transform.ClearChildren();
        //resize dialog window to match option length
        DialogPromptDump.transform.position = new UnityEngine.Vector3(DialogPromptDump.transform.position.x, (20 * options.Length) + 20);
        DialogPrompt = new UnityEngine.GameObject[options.Length]; int i = 0;
        foreach (string item in options)
        {
            //grab dialog prompt object
            //UnityEngine.GameObject go = (UnityEngine.GameObject)Instantiate(UnityEngine.GameObject.Find("DialogText").GetComponent<UnityEngine.GameObject>(), gameObject.transform);
            //instantiate prefab text to window
            DialogPrompt[i] = (UnityEngine.GameObject)Instantiate(UnityEngine.GameObject.Find("DialogPrompt").GetComponent<UnityEngine.GameObject>(), DialogPromptDump.transform);
            //dialogPromptHandler.DialogPromptGameObject = (UnityEngine.GameObject)Instantiate(DialogPrompt.GetComponent<UnityEngine.GameObject>(), DialogPromptDump.transform);
            //overwrite text with option
            DialogPrompt[i].transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = item;
            //dialogPromptHandler.DialogPromptGameObject.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = item;
            DialogPrompt[i].SetActive(true);
            //dialogPromptHandler.DialogPromptGameObject.SetActive(true);
            //assign int value to prompt-option 
            i++;//promptValue(int i);
            //promptValue(int i, instanceId);
            UnityEngine.Debug.Log(DialogPrompt[i].GetInstanceID());
        }
        DialogPromptDump.SetActive(true);
    }
	#endregion

	#region Text and Message Display
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
        DialogPromptDump.SetActive(false);
        //Pause player/lock controller actions
        //ChangeScene(UICanvas.DialogWindow);
        //DialogText = message.Trim("\r\n".ToCharArray());
        string[] text = message.Trim("\r\n".ToCharArray()).Split(new string[] { "\r" }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < text.Length; i++)
        {
            //At the start of every message, disable the message fast-forwarder
            InstantLine = false;
            //turn off the bouncing cursor 
            //GameObject.Find...SetActive(false)
            
            //foreach (string line in text[i].Split(new string[] { "\n" }, StringSplitOptions.None)){}
            //DialogText = text[i].Replace("\n", " "+Environment.NewLine);
            //StartCoroutine(SetTextRoutine(text[i].Replace("\n", " " + Environment.NewLine)));
            //UpdateTextLinesFrmDump();
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
                PromptYesNo();
                //yield return 1;
            }
            else
            {
                //Display custom prompt options using stringArray
                //return int value based on user prompt select
                PromptOptions(promptOptions);
                //yield return 0;
            }
            //toggle visibility
        }
        else yield return -1;
        //Disable the dialog window
        //DialogPromptDump.SetActive(false);
    }

    public IEnumerator SetTextRoutine(string text)
    {
        // reset the paragraph text
        //DialogUIText.text = string.Empty;
        //ClearDialogText();

        // keep local start and end tag variables 
        string startTag = string.Empty;
        string endTag = string.Empty;

        for (int i = 0; i < text.Length; i++)
        {
            if (UnityEngine.Input.anyKeyDown)
            {
                InstantLine = true;
            }

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
            if (InstantLine) continue;
            else

                yield return new UnityEngine.WaitForSeconds(secPerChar);
        }
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
        for (int i = 0; i < DialogUIText.cachedTextGenerator.lineCount; i++)
        {
            int startIndex = DialogUIText.cachedTextGenerator.lines[i].startCharIdx;
            int endIndex = (i == DialogUIText.cachedTextGenerator.lines.Count - 1) ? DialogUIText.text.Length : DialogUIText.cachedTextGenerator.lines[i + 1].startCharIdx;
            int length = endIndex - startIndex;
            //UnityEngine.Debug.Log(DialogUIText.text.Substring(startIndex, length));
            lines[i] = DialogUIText.text.Substring(startIndex, length);
        }
        if (DialogUIText.cachedTextGenerator.lineCount > 2)
        {
            lines[0] = lines[1];
            lines[1] = lines[2] ?? string.Empty;
            lines[2] = null;
        }
        //string newText = lines[0] + Environment.NewLine + lines[1]; //+ Environment.NewLine;
        DialogUIText.text = lines[0] + Environment.NewLine + lines[1] + Environment.NewLine;
    }
    private void UpdateTextLinesFrmDump()
    {
        //UnityEngine.Canvas.ForceUpdateCanvases();
        string[] lines = new string[3];
        for (int i = 0; i < DialogUITextDump.cachedTextGenerator.lineCount; i++)
        {
            int startIndex = DialogUITextDump.cachedTextGenerator.lines[i].startCharIdx;
            int endIndex = (i == DialogUITextDump.cachedTextGenerator.lines.Count - 1) ? DialogUITextDump.text.Length : DialogUITextDump.cachedTextGenerator.lines[i + 1].startCharIdx;
            int length = endIndex - startIndex;
            //UnityEngine.Debug.Log(DialogUIText.text.Substring(startIndex, length));
            lines[i] = DialogUITextDump.text.Substring(startIndex, length);
        }
        if (DialogUITextDump.cachedTextGenerator.lineCount > 2)
        {
            lines[0] = lines[1];
            lines[1] = lines[2] ?? string.Empty;
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

	#region MyRegion
	public static readonly Color DefaultColor = new Color(16, 24, 32);
	public static readonly Color PlayerColor = new Color(0, 0, 180);

	public static int TextSpeed = 1;

	public string Text;
	private int currentChar = 0;
	private int currentLine = 0;
	private int fullLines = 0;
	private bool through = false;
	private bool clearNextLine = false;

	private string[] showText = new string[3];

	public bool Showing = false;
	private float Delay = 0.2F;
	private bool doReDelay = true;
	public float reDelay = 1.5F;
	public float PositionY = 0;
	public bool CanProceed = true;
	public Color TextColor = new Color(16, 24, 33);

	//public FontContainer TextFont = FontManager.GetFontContainer("textfont");

	private Entity[] Entities;

	private ChooseBox.DoAnswer ResultFunction = null;// TODO Change to default(_) if this is not a reference type

	public delegate void FollowUpDelegate();
	public FollowUpDelegate FollowUp = null;

	public void Show(string Text, ChooseBox.DoAnswer ResultFunction, bool doReDelay, bool CheckDelay, Color TextColor)
	{
		if (reDelay == 0.0F | CheckDelay == false)
		{
			if (this.Showing == false)
			{
				//PositionY = Core.windowSize.Height;
				Showing = true;
			}
			this.doReDelay = doReDelay;
			this.Text = Text;
			this.ResultFunction = ResultFunction;
			this.TextColor = TextColor;
			showText[0] = "";
			showText[1] = "";
			through = false;
			currentLine = 0;
			currentChar = 0;
			Delay = 0.2F;
			clearNextLine = false;

			FormatText();
		}
	}

	public void Show(string Text, Entity[] Entities, bool doReDelay, bool CheckDelay, Color TextColor)
	{
		if (reDelay == 0.0F | CheckDelay == false)
		{
			if (this.Showing == false)
			{
				//PositionY = Core.windowSize.Height;
				Showing = true;
			}
			this.doReDelay = doReDelay;
			this.Text = Text;
			this.Entities = Entities;
			this.TextColor = TextColor;
			showText[0] = "";
			showText[1] = "";
			through = false;
			currentLine = 0;
			currentChar = 0;
			Delay = 0.2F;
			clearNextLine = false;

			FormatText();
		}
	}

	public void Show(string Text, Entity[] Entities, bool doReDelay, bool CheckDelay)
	{
		this.Show(Text, Entities, doReDelay, CheckDelay, this.TextColor);
	}

	public void Show(string Text, Entity[] Entities, bool doReDelay)
	{
		this.Show(Text, Entities, doReDelay, true);
	}

	public void Show(string Text)
	{
		this.Show(Text,
			new Entity[]
		{
		}, false, false);
	}

	public void Hide()
	{
		Showing = false;
		if (this.doReDelay == true)
			this.reDelay = 1.0F;
	}

	public void Show(string Text, Entity[] Entities)
	{
		this.Show(Text, Entities, true);
	}

	private void FormatText()
	{
		string[] tokenSearchBuffer = this.Text.Split(System.Convert.ToChar("<"));
		int tokenEndIdx = 0;
		string validToken = "";
		//Token token = null;// TODO Change to default(_) if this is not a reference type 
		//foreach (string possibleToken in tokenSearchBuffer)
		//{
		//	tokenEndIdx = possibleToken.IndexOf(">");
		//	if (!tokenEndIdx == -1)
		//	{
		//		validToken = possibleToken.Substring(0, tokenEndIdx);
		//		if (Localization.LocalizationTokens.ContainsKey(validToken) == true)
		//		{
		//			if (Localization.LocalizationTokens.TryGetValue(validToken, token) == true)
		//				this.Text = this.Text.Replace("<" + validToken + ">", token.TokenContent);
		//		}
		//	}
		//}

		this.Text = this.Text.Replace("<playername>", GameVariables.playerTrainer.PlayerName);
		this.Text = this.Text.Replace("<rivalname>", GameVariables.playerTrainer.RivalName);

		this.Text = this.Text.Replace("[POKE]", "Poké");
		this.Text = this.Text.Replace("[POKEMON]", "Pokémon");
	}

	//public void Update()
	//{
	//	if (Showing == true)
	//	{
	//		ResetCursor();
	//		if (PositionY <= Core.windowSize.Height - 160.0F)
	//		{
	//			if (through == false)
	//			{
	//				if (Text.Count() > currentChar)
	//				{
	//					if (Delay <= 0.0F)
	//					{
	//						if (Text[currentChar].ToString() == @"\")
	//						{
	//							if (Text.Count() > currentChar + 1)
	//							{
	//								showText[currentLine] += Text[currentChar + 1];

	//								currentChar += 2;
	//							}
	//							else
	//								currentChar += 1;
	//						}
	//						else
	//						{
	//							switch (Text[currentChar])
	//							{
	//								case System.Convert.ToChar("~"):
	//									{
	//										if (currentLine == 1)
	//											through = true;
	//										else
	//											currentLine += 1;
	//										break;
	//									}

	//								case System.Convert.ToChar("*"):
	//									{
	//										currentLine = 0;
	//										clearNextLine = true;
	//										through = true;
	//										break;
	//									}

	//								case System.Convert.ToChar("%"):
	//									{
	//										ProcessChooseBox();
	//										break;
	//									}

	//								default:
	//									{
	//										showText[currentLine] += Text[currentChar];
	//										break;
	//									}
	//							}

	//							currentChar += 1;
	//						}

	//						if (KeyBoardHandler.KeyDown(KeyBindings.EnterKey1) | KeyBoardHandler.KeyDown(KeyBindings.EnterKey2) | MouseHandler.ButtonDown(MouseHandler.MouseButtons.LeftButton) == true | ControllerHandler.ButtonDown(Buttons.A) == true | ControllerHandler.ButtonDown(Buttons.B) == true)
	//							Delay = 0.0F;
	//						else
	//							Delay = GetTextSpeed();
	//					}
	//					else
	//						Delay -= 0.1F;
	//				}
	//				else
	//					through = true;
	//			}
	//			else if (Controls.Accept() | Controls.Dismiss())
	//			{
	//				SoundManager.PlaySound("select");
	//				if (Text.Count() <= currentChar)
	//				{
	//					if (CanProceed == true)
	//					{
	//						Showing = false;
	//						ResetCursor();

	//						if (!this.FollowUp == null)
	//						{
	//							this.FollowUp();
	//							this.FollowUp = null;
	//						}

	//						this.TextFont = FontManager.GetFontContainer("textfont");
	//						this.TextColor = TextBox.DefaultColor;
	//						if (this.doReDelay == true)
	//							this.reDelay = 1.0F;
	//					}
	//				}
	//				else
	//				{
	//					if (clearNextLine == true)
	//						showText[0] = "";
	//					else
	//						showText[0] = showText[1];
	//					showText[1] = "";
	//					through = false;
	//					clearNextLine = false;
	//				}
	//			}
	//		}
	//		else
	//		{
	//			float ySpeed = 3.5F;
	//			switch (TextSpeed)
	//			{
	//				case 1:
	//					{
	//						ySpeed = 3.5F;
	//						break;
	//					}
	//				case 2:
	//					{
	//						ySpeed = 4.5F;
	//						break;
	//					}
	//				case 3:
	//					{
	//						ySpeed = 6.5F;
	//						break;
	//					}
	//			}
	//			this.PositionY -= ySpeed;
	//		}
	//	}
	//	else if (reDelay > 0.0F)
	//	{
	//		reDelay -= 0.1F;
	//		if (reDelay <= 0.0F)
	//			reDelay = 0.0F;
	//	}
	//}

	//private void ResetCursor()
	//{
	//	if (Core.CurrentScreen.Identification == Screen.Identifications.OverworldScreen)
	//	{
	//		OverworldCamera c = (OverworldCamera)Screen.Camera;
	//		Mouse.SetPosition(System.Convert.ToInt32(Core.windowSize.Width / (double)2), System.Convert.ToInt32(Core.windowSize.Height / (double)2));
	//		c.oldX = System.Convert.ToInt32(Core.windowSize.Width / (double)2);
	//		c.oldY = System.Convert.ToInt32(Core.windowSize.Height / (double)2);
	//	}
	//}

	//public void Draw()
	//{
	//	if (this.Showing == true)
	//	{
	//		{
	//			var withBlock = Core.SpriteBatch;
	//			withBlock.Draw(TextureManager.GetTexture(@"GUI\Overworld\TextBox"), new Rectangle(System.Convert.ToInt32(Core.windowSize.Width / (double)2) - 240, System.Convert.ToInt32(PositionY), 480, 144), new Rectangle(0, 0, 160, 48), Color.White);

	//			float m = 1.0F;
	//			switch (this.TextFont.FontName.ToLower())
	//			{
	//				case "textfont":
	//				case "braille":
	//					{
	//						m = 2.0F;
	//						break;
	//					}
	//			}

	//			withBlock.DrawString(this.TextFont.SpriteFont, this.showText[0], new Vector2(System.Convert.ToInt32(Core.windowSize.Width / (double)2) - 210, System.Convert.ToInt32(PositionY) + 40), this.TextColor, 0.0F, Vector2.Zero, m, SpriteEffects.None, 0.0F);
	//			withBlock.DrawString(this.TextFont.SpriteFont, this.showText[1], new Vector2(System.Convert.ToInt32(Core.windowSize.Width / (double)2) - 210, System.Convert.ToInt32(PositionY) + 75), this.TextColor, 0.0F, Vector2.Zero, m, SpriteEffects.None, 0.0F);

	//			if (this.CanProceed == true & this.through == true)
	//				withBlock.Draw(TextureManager.GetTexture(@"GUI\Overworld\TextBox"), new Rectangle(System.Convert.ToInt32(Core.windowSize.Width / (double)2) + 192, System.Convert.ToInt32(PositionY) + 128, 16, 16), new Rectangle(0, 48, 16, 16), Color.White);
	//		}
	//	}
	//}

	private void ProcessChooseBox()
	{
		string SplitText = Text.Remove(0, currentChar + 1);
		SplitText = SplitText.Remove(SplitText.IndexOf("%"));
		through = true;
		string[] Options = SplitText.Split(System.Convert.ToChar("|"));
		Text = Text.Remove(currentChar, SplitText.Length + 1);
		if (this.Entities == null & this.ResultFunction != null || this.Entities.Length == 0 & this.ResultFunction != null)
			new ChooseBox().Show(Options, this.ResultFunction);
		else
			new ChooseBox().Show(Options, 0, Entities);
		//ChooseBox.TextFont = this.TextFont;
	}

	private float GetTextSpeed()
	{
		switch (TextSpeed)
		{
			case 1:
				{
					return 0.3F;
				}

			case 2:
				{
					return 0.2F;
				}

			case 3:
				{
					return 0.1F;
				}
		}
		return 0.2F;
	}
	#endregion
	#endregion

	#region Change a Pokémon's level
	/*
def pbChangeLevel(pokemon,newlevel,scene)
  newlevel=1 if newlevel<1
  newlevel=PBExperience::MAXLEVEL if newlevel>PBExperience::MAXLEVEL
  if pokemon.level>newlevel
    attackdiff=pokemon.attack
    defensediff=pokemon.defense
    speeddiff=pokemon.speed
    spatkdiff=pokemon.spatk
    spdefdiff=pokemon.spdef
    totalhpdiff=pokemon.totalhp
    pokemon.level=newlevel
    pokemon.calcStats
    scene.pbRefresh
    Kernel.pbMessage(_INTL("{1} was downgraded to Level {2}!",pokemon.name,pokemon.level))
    attackdiff=pokemon.attack-attackdiff
    defensediff=pokemon.defense-defensediff
    speeddiff=pokemon.speed-speeddiff
    spatkdiff=pokemon.spatk-spatkdiff
    spdefdiff=pokemon.spdef-spdefdiff
    totalhpdiff=pokemon.totalhp-totalhpdiff
    pbTopRightWindow(_INTL("Max. HP<r>{1}\r\nAttack<r>{2}\r\nDefense<r>{3}\r\nSp. Atk<r>{4}\r\nSp. Def<r>{5}\r\nSpeed<r>{6}",
       totalhpdiff,attackdiff,defensediff,spatkdiff,spdefdiff,speeddiff))
    pbTopRightWindow(_INTL("Max. HP<r>{1}\r\nAttack<r>{2}\r\nDefense<r>{3}\r\nSp. Atk<r>{4}\r\nSp. Def<r>{5}\r\nSpeed<r>{6}",
       pokemon.totalhp,pokemon.attack,pokemon.defense,pokemon.spatk,pokemon.spdef,pokemon.speed))
  elsif pokemon.level==newlevel
    Kernel.pbMessage(_INTL("{1}'s level remained unchanged.",pokemon.name))
  else
    attackdiff=pokemon.attack
    defensediff=pokemon.defense
    speeddiff=pokemon.speed
    spatkdiff=pokemon.spatk
    spdefdiff=pokemon.spdef
    totalhpdiff=pokemon.totalhp
    oldlevel=pokemon.level
    pokemon.level=newlevel
    pokemon.changeHappiness("levelup")
    pokemon.calcStats
    scene.pbRefresh
    Kernel.pbMessage(_INTL("{1} was elevated to Level {2}!",pokemon.name,pokemon.level))
    attackdiff=pokemon.attack-attackdiff
    defensediff=pokemon.defense-defensediff
    speeddiff=pokemon.speed-speeddiff
    spatkdiff=pokemon.spatk-spatkdiff
    spdefdiff=pokemon.spdef-spdefdiff
    totalhpdiff=pokemon.totalhp-totalhpdiff
    pbTopRightWindow(_INTL("Max. HP<r>+{1}\r\nAttack<r>+{2}\r\nDefense<r>+{3}\r\nSp. Atk<r>+{4}\r\nSp. Def<r>+{5}\r\nSpeed<r>+{6}",
       totalhpdiff,attackdiff,defensediff,spatkdiff,spdefdiff,speeddiff))
    pbTopRightWindow(_INTL("Max. HP<r>{1}\r\nAttack<r>{2}\r\nDefense<r>{3}\r\nSp. Atk<r>{4}\r\nSp. Def<r>{5}\r\nSpeed<r>{6}",
       pokemon.totalhp,pokemon.attack,pokemon.defense,pokemon.spatk,pokemon.spdef,pokemon.speed))
    movelist=pokemon.getMoveList
    for i in movelist
      if i[0]==pokemon.level          # Learned a new move
        pbLearnMove(pokemon,i[1],true)
      end
    end
    newspecies=pbCheckEvolution(pokemon)
    if newspecies>0
      pbFadeOutInWithMusic(99999){
         evo=PokemonEvolutionScene.new
         evo.pbStartScreen(pokemon,newspecies)
         evo.pbEvolution
         evo.pbEndScreen
      }
    end
  end
end

def pbTopRightWindow(text)
  window=Window_AdvancedTextPokemon.new(text)
  window.z=99999
  window.width=198
  window.y=0
  window.x=Graphics.width-window.width
  pbPlayDecisionSE()
  loop do
    Graphics.update
    Input.update
    window.update
    if Input.trigger?(Input::C)
      break
    end
  end
  window.dispose
end
	 */
	#endregion
}

public class DialogPromptHandler //: UnityEngine.MonoBehaviour
{
    public int DialogPromptValue { get; private set; }
    public UnityEngine.GameObject DialogPromptGameObject;
    /*protected static DialogPromptHandler Instance;
    //protected static UnityEngine.GameObject Instance;

    void Start()
    {
        Instance = this;
    }*/

    public void SetValue(int promptvalue)
    {
        DialogPromptValue = promptvalue;
    }
}

public class ChooseBox
{
	public delegate void DoAnswer(int result);

	public string[] Options;
	public int index = 0;

	private float PositionY = 0;

	public bool Showing = false;
	public bool readyForResult = false;
	public int result = 0;
	public int resultID = 0;
	public bool ActionScript = false;

	public static int CancelIndex = -1;

	//public FontContainer TextFont;

	public bool DoDelegate = false;

	private DoAnswer Subs;

	public Entity[] UpdateEntities;

	public void Show(string[] Options, DoAnswer DoSubs)
	{
		this.resultID = 0;
		this.Options = Options;
		this.index = 0;
		this.readyForResult = false;
		this.Showing = true;
		this.Subs = DoSubs;
		this.ActionScript = false;
		this.DoDelegate = true;
		//this.TextFont = FontManager.GetFontContainer("textfont");

		SetupOptions();
	}

	public void Show(string[] Options, int ID, bool ActionScript)
	{
		this.resultID = ID;
		this.Options = Options;
		this.index = 0;
		this.readyForResult = false;
		this.Showing = true;
		this.ActionScript = ActionScript;
		this.DoDelegate = false;
		//this.TextFont = FontManager.GetFontContainer("textfont");

		SetupOptions();
	}

	public void Show(string[] Options, int ID, Entity[] UpdateEntities)
	{
		this.resultID = ID;
		this.Options = Options;
		this.index = 0;
		this.readyForResult = false;
		this.Showing = true;
		this.UpdateEntities = UpdateEntities;
		this.ActionScript = false;
		this.DoDelegate = false;
		//this.TextFont = FontManager.GetFontContainer("textfont");

		SetupOptions();
	}

	private void SetupOptions()
	{
		for (var i = 0; i <= Options.Count() - 1; i++)
			Options[i] = Options[i].Replace("<playername>", GameVariables.playerTrainer.PlayerName);
	}

	public int getResult(int ID)
	{
		if (this.readyForResult == true)
		{
			if (this.resultID == ID)
				return result;
			else
				return -1;
		}
		else
			return -1;
	}

	//public void Update()
	//{
	//	Update(true);
	//}

	//public void Update(bool RaiseClickEvent)
	//{
	//	if (this.Showing == true)
	//	{
	//		if (Controls.Down(true, true, true))
	//			this.index += 1;
	//		if (Controls.Up(true, true, true))
	//			this.index -= 1;

	//		if (this.index < 0)
	//			this.index = this.Options.Count() - 1;
	//		if (this.index == this.Options.Count())
	//			this.index = 0;
	//		if (RaiseClickEvent == true)
	//		{
	//			if (Controls.Accept() == true)
	//			{
	//				this.PlayClickSound();
	//				this.result = index;
	//				this.HandleResult();
	//			}
	//			if (Controls.Dismiss() == true & CancelIndex > -1)
	//			{
	//				this.PlayClickSound();
	//				this.result = CancelIndex;
	//				this.HandleResult();
	//			}
	//		}
	//	}
	//}

	private void PlayClickSound()
	{
		if (GameVariables.TextBox.Showing == false)
			SoundManager.PlaySound("select");
	}

	private void HandleResult()
	{
		ChooseBox.CancelIndex = -1;
		this.readyForResult = true;
		this.Showing = false;
		//if (this.DoDelegate == true)
		//	Subs(result);
		//else if (Core.CurrentScreen.Identification == Screen.Identifications.OverworldScreen)
		//{
		//	if (this.ActionScript == true)
		//	{
		//		OverworldScreen c = (OverworldScreen)Core.CurrentScreen;
		//		c.ActionScript.Switch(this.Options[result]);
		//	}
		//	else
		//		foreach (Entity Entity in UpdateEntities)
		//			Entity.ResultFunction(result);
		//}
	}

	//public void Draw(Vector2 Position, bool DrawBox = true, float Size = 1.0F)
	//{
	//	if (this.Showing == true)
	//	{
	//		{
	//			var withBlock = Core.SpriteBatch;
	//			// Bounding box
	//			if (DrawBox)
	//			{
	//				withBlock.Draw(TextureManager.GetTexture(@"GUI\Overworld\ChooseBox"), new Rectangle(System.Convert.ToInt32(Position.X), System.Convert.ToInt32(Position.Y), 288, 48), new Rectangle(0, 0, 96, 16), Color.White);
	//				for (var i = 0; i <= Options.Count() - 2; i++)
	//					withBlock.Draw(TextureManager.GetTexture(@"GUI\Overworld\ChooseBox"), new Rectangle(System.Convert.ToInt32(Position.X), System.Convert.ToInt32(Position.Y) + 48 + i * 48, 288, 48), new Rectangle(0, 16, 96, 16), Color.White);
	//				withBlock.Draw(TextureManager.GetTexture(@"GUI\Overworld\ChooseBox"), new Rectangle(System.Convert.ToInt32(Position.X), System.Convert.ToInt32(Position.Y) + 96 + (Options.Count() - 2) * 48, 288, 48), new Rectangle(0, 32, 96, 16), Color.White);
	//			}
	//			// Text
	//			for (var i = 0; i <= Options.Count() - 1; i++)
	//			{
	//				float useSize = Size;
	//				switch (this.TextFont.FontName.ToLower())
	//				{
	//					case "textfont":
	//					case "braille":
	//						{
	//							useSize = 2 * Size;
	//							break;
	//						}
	//				}
	//				withBlock.DrawString(this.TextFont.SpriteFont, Options[i].Replace("[POKE]", "Poké"), new Vector2(System.Convert.ToInt32(Position.X + 40), System.Convert.ToInt32(Position.Y) + 32 + i * 48 * Size), Color.Black, 0.0F, Vector2.Zero, useSize, SpriteEffects.None, 0.0F);
	//			}
	//			// Cursor
	//			withBlock.Draw(TextureManager.GetTexture(@"GUI\Overworld\ChooseBox"), new Rectangle(System.Convert.ToInt32(Position.X + 20), System.Convert.ToInt32(Position.Y) + 36 + System.Convert.ToInt32(index * 48 * Size), System.Convert.ToInt32(10 * Size), System.Convert.ToInt32(20 * Size)), new Rectangle(96, 0, 3, 6), Color.White);
	//		}
	//	}
	//}

	//public void Draw()
	//{
	//	if (this.Showing == true)
	//	{
	//		Vector2 Position = new Vector2(System.Convert.ToInt32(Core.windowSize.Width / (double)2) - 48, Core.windowSize.Height - 160.0F - 96.0F - (Options.Count() - 1) * 48);
	//		this.Draw(Position);
	//	}
	//}
}