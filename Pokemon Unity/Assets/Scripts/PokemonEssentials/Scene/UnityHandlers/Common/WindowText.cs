using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using PokemonEssentials.Interface;
using PokemonUnity.Utility;
using UnityEngine;

namespace PokemonUnity.Interface.UnityEngine
{
	/// <summary>
	/// </summary>
	/// <remarks>
	/// </remarks>
	[RequireComponent(typeof(global::UnityEngine.UI.Image))]
	public class WindowText : Window, IWindow_UnformattedTextPokemon//, IWindow_AdvancedTextPokemon, IWindow_CommandPokemon
	{
		#region Unity's MonoBehavior Inspector Properties
		protected global::UnityEngine.UI.Text text;
		protected global::UnityEngine.UI.Text textShadow;
		protected Coroutine displayCoroutine;
		/// <summary>
		/// Text to be displayed after parsing
		/// </summary>
		protected StringBuilder textDisplay;
		protected StringBuilder fmtchars;
		/// <summary>
		/// Original text passed through parameters to be displayed in UI
		/// </summary>
		//public global::UnityEngine.UI.Text Text;
		public StringBuilder Text;
		protected StringBuilder textchars;
		protected bool letterByLetter;
		protected bool isFastSpeed = false;
		protected float defaultCharSpeed = 0.05f;
		//protected KeyCode fastSpeedKey = KeyCode.LeftShift;
		//protected KeyCode skipKey = KeyCode.Space;
		#endregion

		#region Interface Properties Implementation
		public IColor baseColor {
			get { return new SeriColor(text.color); }
			set { text.color = ((SeriColor)value).Color; }
		}
		public IColor shadowColor {
			get { return new SeriColor(textShadow.color); }
			set { textShadow.color = ((SeriColor)value).Color; }
		}
		public bool letterbyletter {
			get { return letterByLetter; }
			set { letterByLetter = value; }
		}
		string IWindow_UnformattedTextPokemon.text {
			get { return Text.ToString(); }
			set {
				Text.Clear();
				Text.Append(value);
				text.text = value;
				textShadow.text = value;
				refresh();
			}
		}
		#endregion

		#region Unity's MonoBehavior Method
		private void Awake()
		{
			rect = GetComponent<RectTransform>();
			background = GetComponent<global::UnityEngine.UI.Image>();
			Text = new StringBuilder();
			textchars = new StringBuilder();
			textDisplay = new StringBuilder();
		}

		void Update()
		{
			//if (global::UnityEngine.Input.GetKey(fastSpeedKey))
			if (GameManager.current.InputManager.repeat(PokemonUnity.Input.B)) //if held down
			{
				isFastSpeed = true;
			}
			else
			{
				isFastSpeed = false;
			}

			//if (global::UnityEngine.Input.GetKeyUp(skipKey) && displayCoroutine != null)
			if (GameManager.current.InputManager.press(PokemonUnity.Input.B) && displayCoroutine != null)
			{
				StopCoroutine(displayCoroutine);
				text.text = textDisplay.ToString();
				textShadow.text = RemoveCustomTags(textDisplay.ToString());
			}
		}
		#endregion

		#region Interface Method Implementation
		public IWindow_UnformattedTextPokemon initialize(string text = "")
		{
			base.initialize(0,0,33,33) ;//super(0,0,33,33)
			//this.contents=new Bitmap(1,1);
			//pbSetSystemFont(this.contents);
			Text.Append(text); //@text=text;
			@letterbyletter=false; // Not supported in this class
			//IColor[] colors=getDefaultTextColors(this.windowskin);
			//@baseColor=colors[0];
			//@shadowColor=colors[1];
			resizeToFit(text);
			return this;
		}

		public virtual void refresh()
		{
			//this.contents=DoEnsureBitmap(this.contents,this.width-this.borderX,
			//	this.height-this.borderY);
			//this.contents.Clear();
			//drawTextEx(this.contents,0,0,this.contents.width,0,
			//	//@text.gsub(/\r/,""),@baseColor,@shadowColor)
			//	Text.Replace("\r",""),@baseColor,@shadowColor);


			textDisplay = Text;
			//if any special characters are found, parse them
			textDisplay.Replace("\r", "");
			//change any color tags with word of color to hex value
		}

		public virtual void resizeHeightToFit(string text, int width = -1)
		{
			//Not used...
			//int[] dims = resizeToFitInternal(text, width);
			//this.width = width < 0 ? Graphics.width : width;
			//this.height = dims[1] + this.borderY;
			//refresh();
		}

		public virtual void resizeToFit(string text, int maxwidth = -1)
		{
			//Not used...
		}

		public virtual int[] resizeToFitInternal(string text, int maxwidth)
		{
			int[] dims=new int[] { 0, 0 };
			//float cwidth = maxwidth < 0 ? Graphics.width : maxwidth;
			//getLineBrokenChunks(self.contents,text,
			//	cwidth-self.borderX-SpriteWindow_Base::TEXTPADDING,dims,true);
			return dims;
		}

		public virtual void setTextToFit(string text, int maxwidth = -1)
		{
			//Not used...
		}

		public virtual IWindow_UnformattedTextPokemon WithSize(string text, float x, float y, int width, int height, IViewport viewport = null)
		{
			IWindow_UnformattedTextPokemon ret = initialize(text);
			ret.x = x;
			ret.y = y;
			ret.width = width;
			ret.height = height;
			ret.viewport = viewport;
			ret.refresh();
			return ret;
		}
		#endregion

		protected virtual void DisplayText()
		{
			if (displayCoroutine != null)
			{
				StopCoroutine(displayCoroutine);
			}
			displayCoroutine = StartCoroutine(TypeText());
		}

		IEnumerator TypeText()
		{
			text.text = "";
			textShadow.text = "";
			int totalLength = textDisplay.Length;
			int visibleCount = 0;

			while (visibleCount < totalLength)
			{
				text.text += textDisplay[visibleCount];
				textShadow.text += textDisplay[visibleCount];
				visibleCount++;
				yield return new WaitForSeconds(isFastSpeed ? defaultCharSpeed / 2 : defaultCharSpeed);

				//if a timer tag is found, wait for the specified time
				if (textDisplay[visibleCount - 1] == '\\' && textDisplay.ToString().Substring(visibleCount - 1).StartsWith("\\wait["))
				{
					var match = Regex.Match(textDisplay.ToString().Substring(visibleCount - 1), @"\\wait\[(\d+)\]");
					if (match.Success)
					{
						int waitTime = int.Parse(match.Groups[1].Value);
						yield return new WaitForSeconds(waitTime);
						visibleCount += match.Length - 1; // Skip over the entire wait tag
					}
				}

				//if a custom tag is found, apply the color to the text
				if (textDisplay[visibleCount - 1] == '<' && textDisplay.ToString().Substring(visibleCount - 1).StartsWith("<color="))
				{
					// "<color=#FF0000>text</color>" | <color=#FF0000> = 15 char length
					string match = textDisplay.ToString().Substring(visibleCount - 1, 15);
					text.text += match; // Add the color tag to the text, but not shadowText
					visibleCount += match.Length - 1; // Skip over the entire wait tag
				}
				if (textDisplay[visibleCount - 1] == '<' && textDisplay.ToString().Substring(visibleCount - 1).StartsWith("</color>"))
				{
					// "<color=#FF0000>text</color>" | </color> = 8 char length
					string match = textDisplay.ToString().Substring(visibleCount - 1, 8);
					text.text += match; // Add the color tag to the text, but not shadowText
					visibleCount += match.Length - 1; // Skip over the entire wait tag
				}
			}

			displayCoroutine = null; // Text finished displaying
		}

		/// <summary>
		/// Remove all color tags for complete display
		/// (shadow text needs to remain single color, but keep formatting)
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		private string RemoveCustomTags(string text)
		{
			return Regex.Replace(text, "</?color.*>", string.Empty);
		}
	}
}