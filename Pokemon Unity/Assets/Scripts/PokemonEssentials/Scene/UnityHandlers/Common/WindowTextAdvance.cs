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
	public class WindowTextAdvance : WindowText, IWindow_AdvancedTextPokemon
	{
		#region Unity's MonoBehavior Inspector Properties
		protected bool busy, pausing, displaying, needclear, nodraw, starting, frameskipChanged;
		protected int curchar, drawncurchar, lastDrawnChar, textlength, scrollstate, scrollY, linesdrawn, realframes, numtextchars, frameskip;
		protected int _waitcount;
		protected float bitmapwidth, bitmapheight;
		protected IRect endOfText;
		protected IAnimatedSprite pausesprite;
		#endregion

		#region Interface Properties Implementation
		public int lineHeight { get; set; }
		public int textspeed {
			get { return @frameskip; }
			set {
				@frameskip = value;
				@frameskipChanged = true; }
		}
		public int waitcount { get { return _waitcount; } set { _waitcount = (value <= 0) ? 0 : value; } }
		public int cursorMode { get; set; }
		string IWindow_AdvancedTextPokemon.text {
			get { return textDisplay.ToString(); }
			set {
				((IWindow_UnformattedTextPokemon)text).text = value;
				setText(value);
				DisplayText(); //Animation Coroutine
			}
		}
		bool IWindow_AdvancedTextPokemon.busy { get { return @displaying; } }
		bool IWindow_AdvancedTextPokemon.pausing { get { return @pausing && @displaying; } }
		#endregion

		#region Unity's MonoBehavior Method
		private void Awake()
		{
		}

		void Update()
		{
		}
		#endregion

		#region Interface Method Implementation
		public void allocPause()
		{
			//if (@pausesprite==null) {
			//	@pausesprite = AnimatedSprite.create("Graphics/Pictures/pause", 4, 3);
			//	@pausesprite.z = 100000;
			//	@pausesprite.visible = false;
			//}
		}

		public int maxPosition()
		{
			int pos = 0;
			//for (ch in @fmtchars)
			//	// index after the last character's index
			//	if (pos < ch[14] + 1) pos = ch[14] + 1;
			//}
			return pos;
		}

		public void moveCursor()
		{
			if (@pausesprite!=null) {
				int cursor = @cursorMode;
				//float pauseWidth, pauseHeight;
				if (cursor == 0 && @endOfText==null) cursor = 2;
				switch (cursor) {
					case 0: // End of text
						//@pausesprite.x = this.x + this.startX + @endOfText.x + @endOfText.width - 2;
						//@pausesprite.y = this.y + this.startY + @endOfText.y - @scrollY;
						break;
					case 1: // Lower right
						//pauseWidth = @pausesprite.bitmap != null ? @pausesprite.framewidth : 16;
						//pauseHeight = @pausesprite.bitmap != null ? @pausesprite.frameheight : 16;
						//@pausesprite.x = this.x + this.width - (20 * 2) + (pauseWidth / 2);
						//@pausesprite.y = this.y + this.height - (30 * 2) + (pauseHeight / 2);
						break;
					case 2: // Lower middle
						//pauseWidth = @pausesprite.bitmap != null ? @pausesprite.framewidth : 16;
						//pauseHeight = @pausesprite.bitmap != null ? @pausesprite.frameheight : 16;
						//@pausesprite.x = this.x + (this.width / 2) - (pauseWidth / 2);
						//@pausesprite.y = this.y + this.height - (18 * 2) + (pauseHeight / 2);
						break;
				}
			}
		}

		public int position()
		{
			if (@lastDrawnChar < 0)
				return 0;
			else if (@lastDrawnChar>=@fmtchars.Length)
				return @numtextchars;
			//else
			//	// index after the last character's index
			//	return @fmtchars[@lastDrawnChar][14] + 1;
			return -1;
		}

		public void redrawText()
		{
		}

		public void resizeToFit2(string text, int maxwidth, int maxheight)
		{
		}

		public bool resume()
		{
			if (!busy) {
				this.stopPause();
				return true;
			}
			if (@pausing) {
				@pausing = false;
				this.stopPause();
				return false;
			} else {
				return true;
			}
		}

		public void setText(string value)
		{
			@waitcount = 0;
			@curchar = 0;
			@drawncurchar = -1;
			@lastDrawnChar = -1;
			//string oldtext = textDisplay.ToString(); //@text;
			textDisplay.Clear(); textDisplay.Append(value); //@text = value;
			//@textlength = unformattedTextLength(value);
			@scrollstate = 0;
			@scrollY = 0;
			@linesdrawn = 0;
			@realframes = 0;
			@textchars.Clear();// = [];
			float width = 1;
			float height = 1;
			int numlines = 0;
			//float visiblelines = (this.height - this.borderY) / 32;
			if (value.Length == 0) {
				@fmtchars.Clear();// = [];
				@bitmapwidth = width;
				@bitmapheight = height;
				@numtextchars = 0;
			} else {
				//if (!@letterbyletter) {
				//	//@fmtchars = getFormattedText(this.contents, 0, 0,
				//	//	this.width - this.borderX - SpriteWindow_Base::TEXTPADDING, -1,
				//	//	shadowctag(@baseColor, @shadowColor) + value, 32, true);
				//	//@oldfont = this.contents.font.clone();
				//	foreach (char ch in @fmtchars) {
				//		//chx = ch[1] + ch[3];
				//		//chy = ch[2] + ch[4];
				//		//if (width < chx) width = chx;
				//		//if (height < chy) height = chy;
				//		@textchars.Append(ch[5] ? "" : ch[0]);
				//	}
				//} else {
				//	@fmtchars.Clear();// = [];
				//	//fmt=getFormattedText(this.contents,0,0,
				//	//	this.width-this.borderX-SpriteWindow_Base::TEXTPADDING,-1,
				//	//	shadowctag(@baseColor,@shadowColor)+value,32,true);
				//	//@oldfont=this.contents.font.clone();
				//	foreach (char ch in fmt) {
				//		//chx=ch[1]+ch[3];
				//		//chy=ch[2]+ch[4];
				//		//if (width < chx) width=chx;
				//		//if (height < chy) height=chy;
				//		if (ch[0]=="\n" && @letterbyletter) { //!ch[5] &&
				//			numlines += 1;
				//			if (numlines>=visiblelines) {
				//				char fclone = ch.clone();
				//				fclone[0] = "\1";
				//				@fmtchars.Append(fclone);
				//				@textchars.Append("\\1");
				//			}
				//		}
				//		// Don't add newline characters, since they
				//		// can slow down letter-by-letter display
				//		if ((ch[0]!="\r")) { //ch[5] ||
				//			@fmtchars.push(ch);
				//			@textchars.Append(ch[5] ? "" : ch[0]);
				//		}
				//	}
				//	fmt.clear();
				//}
				@bitmapwidth = width;
				@bitmapheight = height;
				@numtextchars = @textchars.Length;
			}
			stopPause();
			@displaying = @letterbyletter;
			@needclear = true;
			@nodraw = @letterbyletter;
			refresh();
		}

		public void startPause()
		{
			allocPause();
			@pausesprite.visible = true;
			@pausesprite.frame = 0;
			@pausesprite.start();
			moveCursor();
		}

		public void stopPause()
		{
			if (@pausesprite!=null) {
				@pausesprite.stop();
				@pausesprite.visible = false;
			}
		}

		public void updateInternal()
		{
		}

		public override void update()
		{
			if (@pausesprite!=null && @pausesprite.visible) {
				@pausesprite.update();
			}
			if (@waitcount > 0) {
				@waitcount -= 1;
				return;
			}
			if (busy) {
				if (!@frameskipChanged) refresh();
				updateInternal();
				// following line needed to allow "textspeed=-999" to work seamlessly
				if (@frameskipChanged) refresh();
			}
			@frameskipChanged = false;
		}

		public override void refresh()
		{
			IBitmap oldcontents = this.contents;
			//this.contents = DoEnsureBitmap(oldcontents, @bitmapwidth, @bitmapheight);
			this.oy = @scrollY;
			int numchars = @numtextchars;
			int startchar = 0;
			if (this.letterbyletter) numchars = Math.Min(@curchar, @numtextchars);
			if (busy && @drawncurchar == @curchar && @scrollstate == 0) {
				return;
			}
			if (!this.letterbyletter || !oldcontents.Equals(this.contents)) {
				@drawncurchar = -1;
				@needclear = true;
			}
			if (@needclear) {
				//if (@oldfont)this.contents.font=@oldfont;
				//this.contents.clear();
				@needclear = false;
			}
			if (@nodraw) {
				@nodraw = false;
				return;
			}
			//float maxX=this.width-this.borderX;
			//float maxY=this.height-this.borderY;
			//for (int i = 0; i + @drawncurchar+1 < numchars; i++) {
			//	if (i>=@fmtchars.Length) continue;//next
			//	if (!this.letterbyletter) {
			//		if (@fmtchars[i][1] >= maxX) continue;//next
			//		if (@fmtchars[i][2] >= maxY) continue;//next
			//	}
			//	drawSingleFormattedChar(this.contents,@fmtchars[i]);
			//	@lastDrawnChar = i;
			//}
			//if (!this.letterbyletter) {
			//	// all characters were drawn, reset old font
			//	if (@oldfont) this.contents.font = @oldfont;
			//}
			//if (numchars>0 && numchars != @numtextchars) {
			//	fch = @fmtchars[numchars - 1];
			//	if (fch) {
			//		IRect rcdst=new Rect(fch[1], fch[2], fch[3], fch[4]);
			//		if (@textchars[numchars]=="\\1") {
			//			@endOfText = rcdst;
			//			allocPause();
			//			moveCursor();
			//		} else {
			//			@endOfText=new Rect(rcdst.x+rcdst.width,rcdst.y,8,1);
			//		}
			//	}
			//}
			@drawncurchar = @curchar;
		}

		private void curcharSkip(int skip) {
			int i = 0; do { //skip.times
				@curchar += 1;
				if (@textchars[@curchar] == '\n' ||										// "\n"	| newline
					(@textchars[@curchar] == '\\' && @textchars[@curchar+1] == '1') ||	// "\1"	| pause
					(@textchars[@curchar] == '\\' && @textchars[@curchar+1] == '2') ||	// "\2"	| letter-by-letter break
					@textchars[@curchar] == null) break;
			} while (i < skip);
		}

		IWindow_AdvancedTextPokemon IWindow_AdvancedTextPokemon.initialize(string text)
		{
			@cursorMode=1;//MessageConfig::CURSORMODE
			@endOfText = null;
			@scrollstate = 0;
			@realframes = 0;
			@scrollY = 0;
			@nodraw = false;
			@lineHeight = 32;
			@linesdrawn = 0;
			//@bufferbitmap = null;
			@letterbyletter = false;
			@starting = true;
			@displaying = false;
			@lastDrawnChar = -1;
			@fmtchars.Clear();//=[]
			@frameskipChanged = false;
			//@frameskip = MessageConfig.GetTextSpeed();
			base.initialize(0, 0, 33, 33); //super(0, 0, 33, 33);
			//@pausesprite = null;
			//@text = "";
			//this.contents=new Bitmap(1, 1);
			//SetSystemFont(this.contents);
			//this.resizeToFit(text, Graphics.width);
			//IColor[] colors = getDefaultTextColors(this.windowskin);
			//@baseColor = colors[0];
			//@shadowColor = colors[1];
			textDisplay.Clear(); textDisplay.Append(text);//this.text = text;
			@starting = false;
			return this;
		}

		IWindow_AdvancedTextPokemon IWindow_AdvancedTextPokemon.WithSize(string text, float x, float y, int width, int height, IViewport viewport)
		{
			IWindow_AdvancedTextPokemon ret = ((IWindow_AdvancedTextPokemon)this).initialize(text);
			ret.x = x;
			ret.y = y;
			ret.width = width;
			ret.height = height;
			ret.viewport = viewport;
			return ret;
		}
		#endregion
	}
}