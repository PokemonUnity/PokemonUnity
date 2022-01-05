using System.Collections;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityGameFramework.Runtime;
using PokemonUnity.UX;
using PokemonUnity.Monster;
using System.Collections.Generic;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.Screen;

namespace PokemonUnity
{
	[ExecuteInEditMode]
	public class TypingForm : UGuiForm, IPokemonEntryScene, IPokemonEntryScene2
	{
		public int Id;
		int IScene.Id { get { return Id;} }
		public string helptext;
		public int Minlength;
		public int Maxlength;
		public bool init;
		public int[] blanks; //Array of char slots, if active 1, else 0
		//public string InitialText;
		public TextEntryTypes subject = 0;
		public IPokemon pokemon = null;
		public static readonly char[][] Characters=new char[][] {

			"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray(),	// "[*]"
			"0123456789   !@\\#$%^&*()   ~`-_+={}[]   :;'\"<>,.?/   ".ToCharArray()	//, "[A]"
		};
		public int @mode = 0;
		public int @symtype = 0;

		//[SerializeField]
		//private RectTransform m_Transform = null;
		//[SerializeField]
		//private float m_ScrollSpeed = 1f;
		//private float m_InitPosition = 0f;
		
		//private bool UseKeyboard;
		private System.Text.StringBuilder typeSpaceText;
		//public string typedString;

		public int selectorIndex  = 0;
		public int pageIndex      = 0;
		public int typeSpaceIndex = 0;
		public int charLimit      = 12;
		public static bool qwerty  = true;
		
		/// <summary>
		/// If the typing screen is for naming a Player, PC Box, or a Pokemon; 
		/// Change the icon to match sprite that represents subject.
		/// <see cref="PokemonUnity.UX.TextEntryTypes"/>
		/// </summary>
		/// Instantiate using prefab...
		/// Should include animation frames, and icon shadow
		public UnityEngine.GameObject   icon;
		//public  UnityEngine.GameObject[]   IconPrefabs; 
		public  UnityEngine.Sprite[] PageBackground;
		public  UnityEngine.UI.Image Page;
		public  UnityEngine.UI.Text entry;
		public  UnityEngine.UI.Text gender;
		//public  CharKeyItem         CharKeyPrefab;
		//public  GameObject          CharKeyPrefab;
		//public  GameObject          PagePrefab;
		public  IDictionary<string,GameObject>          sprites;
		
		private GameFrameworkAction<object> m_OnClickConfirm = null;
        private bool refreshOverlay;
        private int cursorpos;

        public string TypeEntryText { get { return typeSpaceText.ToString(); } }

		public static readonly string[] PageCharArray = new string[] {
			string.Format("{0}", qwerty? "QWERTYUIOP ,." : "ABCDEFGHIJ ,.") +
			string.Format("{0}", qwerty? "ASDFGHJKL  '-" : "KLMNOPQRST '-") +
			string.Format("{0}", qwerty? " ZXCVBNM   ♂♀" : "UVWXYZ     ♂♀") +
			"             " +
			"0123456789   ",

			string.Format("{0}", qwerty? "qwertyuiop ,." : "abcdefghij ,.") +
			string.Format("{0}", qwerty? "asdfghjkl  '-" : "klmnopqrst '-") +
			string.Format("{0}", qwerty? " zxcvbnm   ♂♀" : "uvwxyz     ♂♀") +
			"             " +
			"0123456789   ",

			",.:;!?   ♂♀  " +
			"\"'()<>[] +-= " +	//"“”‘’()       " +
			"~@#%*&$^_/\\| "  +	//"…·~@#%+-*/=  " +
			"◎○□△◇♠♥♦♣★♪  " +//"+-=^_/\\|     "
			"☀☁☂☃     ⤴⤵  "
		};
		public const int ROWS=13;
		public const int COLUMNS=5;
		public const int MODE1=-5;
		public const int MODE2=-4;
		public const int MODE3=-3;
		public const int BACK=-2;
		public const int OK=-1;

		#region UnityMonobehavior
		void Update()
		{
		}
		#endregion

		public void OnKeysButtonClick()
		{
			GameEntry.UseKeyboard = !GameEntry.UseKeyboard;
			//image.enabled = GameEntry.UseKeyboard;
			
			if(GameEntry.UseKeyboard) 
				TypeWithKeyboard();
		}

		//public void OnConfirmButtonClick()
		//{
		//	Close();
		//
		//	if (m_OnClickConfirm != null)
		//	{
		//		m_OnClickConfirm(m_UserData);
		//	}
		//}

		//public void OnBackButtonClick()
		//{
		//	Close();
		//
		//	if (m_OnClickBack != null)
		//	{
		//		m_OnClickBack(m_UserData);
		//	}
		//}

		#region MyRegion
		private bool CanBackspace() { if (typeSpaceIndex == 0) return false; else return true; }
		private void SelectUp() { typeSpaceIndex++; if (typeSpaceIndex > 12) typeSpaceIndex = 12; }
		private void SelectDown() { typeSpaceIndex--; if (typeSpaceIndex < 0) typeSpaceIndex = 0; }

		private void addCharacterToString(char character, char capsCharacter)
		{
			if (pageIndex == 3)
			{
				if (typeSpaceIndex < charLimit)
				{
					if (!GameEntry.IsCapLockOn)
					{
						//typeSpaceText[typeSpaceIndex].text = character;
						typeSpaceText[typeSpaceIndex] = character;
					}
					else
					{
						//typeSpaceText[typeSpaceIndex].text = capsCharacter;
						typeSpaceText[typeSpaceIndex] = capsCharacter;
					}
					//typeSpaceTextShadow[typeSpaceIndex].text = typeSpaceText[typeSpaceIndex].text;
					typeSpaceIndex += 1;
				}
			}
		}

		public void Backspace()
		{
			if (typeSpaceIndex > 0)
			{
				for(int a = typeSpaceIndex - 1; a < charLimit-1; a++)
				{
					//Backspace removes char infront, by replacing it with any character that follows after
					//typeSpaceText[a].text = typeSpaceText[a + 1].text;
					//typeSpaceTextShadow[a].text = typeSpaceText[a].text;
					typeSpaceText[a] = typeSpaceText[a + 1];
				}
				//Last char is replaced with an empty, since length shifted down by one
				//typeSpaceText[charLimit].text = ' ';
				//typeSpaceTextShadow[charLimit].text = typeSpaceText[charLimit].text;
				typeSpaceText[charLimit] = ' ';
				//typeSpaceIndex -= 1;
				SelectDown();
				//panelButton[4].enabled = true;
				//yield return new WaitForSeconds(0.1f);
				//panelButton[4].enabled = false;
			}
		}

		/*// Remove forward facing character
		private IEnumerator Delete()
		{
			if (typeSpaceIndex > 0)
			{
				typeSpaceText[typeSpaceIndex - 1].text = ' ';
				typeSpaceTextShadow[typeSpaceIndex - 1].text = typeSpaceText[typeSpaceIndex - 1].text;
				//typeSpaceIndex -= 1;
				SelectDown();
				//panelButton[4].enabled = true;
				yield return new WaitForSeconds(0.1f);
				//panelButton[4].enabled = false;
			}
		}*/

		private void TypeWithKeyboard()
		{
			//keyboardText.text = "Press Esc to stop typing";
			//keyboardTextShadow.text = keyboardText.text;

			/*if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.CapsLock))
			{
				GameEntry.IsCapLockOn = !GameEntry.IsCapLockOn;
			}
			if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
			{
				GameEntry.IsCapLockOn = !GameEntry.IsCapLockOn;
			}

			if (Input.GetKeyDown(KeyCode.BackQuote))
			{
				addCharacterToString('‘', '~');
			}
			else if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				addCharacterToString('1', '!');
			}
			else if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				addCharacterToString('2', '@');
			}
			else if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				addCharacterToString('3', '#');
			}
			else if (Input.GetKeyDown(KeyCode.Alpha4))
			{
				addCharacterToString('4', '4');
			}
			else if (Input.GetKeyDown(KeyCode.Alpha5))
			{
				addCharacterToString('5', '%');
			}
			else if (Input.GetKeyDown(KeyCode.Alpha6))
			{
				addCharacterToString('6', '6');
			}
			else if (Input.GetKeyDown(KeyCode.Alpha7))
			{
				addCharacterToString('7', '&');
			}
			else if (Input.GetKeyDown(KeyCode.Alpha8))
			{
				addCharacterToString('8', '*');
			}
			else if (Input.GetKeyDown(KeyCode.Alpha9))
			{
				addCharacterToString('9', '(');
			}
			else if (Input.GetKeyDown(KeyCode.Alpha0))
			{
				addCharacterToString('0', ')');
			}

			else if (Input.GetKeyDown(KeyCode.Q))
			{
				addCharacterToString('q', 'Q');
			}
			else if (Input.GetKeyDown(KeyCode.W))
			{
				addCharacterToString('w', 'W');
			}
			else if (Input.GetKeyDown(KeyCode.E))
			{
				addCharacterToString('e', 'E');
			}
			else if (Input.GetKeyDown(KeyCode.R))
			{
				addCharacterToString('r', 'R');
			}
			else if (Input.GetKeyDown(KeyCode.T))
			{
				addCharacterToString('t', 'T');
			}
			else if (Input.GetKeyDown(KeyCode.Y))
			{
				addCharacterToString('y', 'Y');
			}
			else if (Input.GetKeyDown(KeyCode.U))
			{
				addCharacterToString('u', 'U');
			}
			else if (Input.GetKeyDown(KeyCode.I))
			{
				addCharacterToString('i', 'I');
			}
			else if (Input.GetKeyDown(KeyCode.O))
			{
				addCharacterToString('o', 'O');
			}
			else if (Input.GetKeyDown(KeyCode.P))
			{
				addCharacterToString('p', 'P');
			}

			else if (Input.GetKeyDown(KeyCode.A))
			{
				addCharacterToString('a', 'A');
			}
			else if (Input.GetKeyDown(KeyCode.S))
			{
				addCharacterToString('s', 'S');
			}
			else if (Input.GetKeyDown(KeyCode.D))
			{
				addCharacterToString('d', 'D');
			}
			else if (Input.GetKeyDown(KeyCode.F))
			{
				addCharacterToString('f', 'F');
			}
			else if (Input.GetKeyDown(KeyCode.G))
			{
				addCharacterToString('g', 'G');
			}
			else if (Input.GetKeyDown(KeyCode.H))
			{
				addCharacterToString('h', 'H');
			}
			else if (Input.GetKeyDown(KeyCode.J))
			{
				addCharacterToString('j', 'J');
			}
			else if (Input.GetKeyDown(KeyCode.K))
			{
				addCharacterToString('k', 'K');
			}
			else if (Input.GetKeyDown(KeyCode.L))
			{
				addCharacterToString('l', 'L');
			}
			else if (Input.GetKeyDown(KeyCode.Semicolon))
			{
				addCharacterToString(';', ':');
			}
			else if (Input.GetKeyDown(KeyCode.Quote))
			{
				addCharacterToString('’', '”');
			}

			else if (Input.GetKeyDown(KeyCode.Z))
			{
				addCharacterToString('z', 'Z');
			}
			else if (Input.GetKeyDown(KeyCode.X))
			{
				addCharacterToString('x', 'X');
			}
			else if (Input.GetKeyDown(KeyCode.C))
			{
				addCharacterToString('c', 'C');
			}
			else if (Input.GetKeyDown(KeyCode.V))
			{
				addCharacterToString('v', 'V');
			}
			else if (Input.GetKeyDown(KeyCode.B))
			{
				addCharacterToString('b', 'B');
			}
			else if (Input.GetKeyDown(KeyCode.N))
			{
				addCharacterToString('n', 'N');
			}
			else if (Input.GetKeyDown(KeyCode.M))
			{
				addCharacterToString('m', 'M');
			}
			else if (Input.GetKeyDown(KeyCode.Comma))
			{
				addCharacterToString(',', ',');
			}
			else if (Input.GetKeyDown(KeyCode.Period))
			{
				addCharacterToString('.', '.');
			}
			else if (Input.GetKeyDown(KeyCode.Slash))
			{
				addCharacterToString('/', '?');
			}
			else if (Input.GetKeyDown(KeyCode.Tab))
			{
				pageIndex = pageIndex + 1 % 2;
			}
			else if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				SelectDown();
			}
			else if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				SelectUp();
			}

			else if (Input.GetKeyDown(KeyCode.Space))
			{
				addCharacterToString(' ', ' ');
			}

			else if (Input.GetKeyDown(KeyCode.Backspace))
			{
				//if(CanBackspace())
				//	yield return StartCoroutine(Backspace());
			}

			//else if (Input.GetButtonDown("Back") && !Input.GetKeyDown(KeyCode.X))
			else if (Input.GetButtonDown("Back") && !Input.GetKeyDown(KeyCode.X))
			{
				//running = false;
				GameEntry.UseKeyboard = false;
			}*/
		}
		#endregion

		public void pbRefresh() { }
		public void pbUpdate()
		{
    //for (int i = 0; i < 3; i++) {
    //  @bitmaps[i].update();
    //}
    //if (@init) { //|| Graphics.frame_count%5==0
    //  @init=false;
    //  int cursorpos=@helper.cursor;
    //  if (cursorpos>=Maxlength) cursorpos=Maxlength-1;
    //  if (cursorpos<0) cursorpos=0;
    //  //Maxlength.times {|i|
    //  for (int i = 0; i < Maxlength; i++) {
    //     if (i==cursorpos) {
    //       @blanks[i]=1;
    //     } else {
    //       @blanks[i]=0;
    //     }
    //     @sprites[$"blank#{i}"].y= new int[]{ 78,82 }[@blanks[i]];
    //  }
    //}
    //pbDoUpdateOverlay();
    ////pbUpdateSpriteHash(@sprites);
		}

		public void pbChangeTab(int newtab = 0)
		{
            //@sprites["cursor"].visible=false;
            //@sprites["toptab"].bitmap=@bitmaps[(newtab%3)+3];
            //21.times do;
            //  @sprites["toptab"].x+=24;
            //  @sprites["bottomtab"].y+=12;
            //  Graphics.update();
            //  Input.update();
            //  pbUpdate();
            //}
            //float tempx=@sprites["toptab"].x;
            //@sprites["toptab"].x=@sprites["bottomtab"].x;
            //@sprites["bottomtab"].x=tempx;
            //float tempy=@sprites["toptab"].y;
            //@sprites["toptab"].y=@sprites["bottomtab"].y;
            //@sprites["bottomtab"].y=tempy;
            //tempbitmap=@sprites["toptab"].bitmap;
            //@sprites["toptab"].bitmap=@sprites["bottomtab"].bitmap;
            //@sprites["bottomtab"].bitmap=tempbitmap;
            //Graphics.update();
            //Input.update();
            //pbUpdate();
            //@mode=(newtab)%3;
            //newtab=@bitmaps[((@mode+1)%3)+3];
            //@sprites["cursor"].visible=true;
            //@sprites["toptab"].bitmap=newtab;
            //@sprites["toptab"].x=22-504;
            //@sprites["toptab"].y=162;
            //pbDoUpdateOverlay2();
		}

		public bool pbColumnEmpty(int m)
		{
            if (m>=ROWS-1) return false;
            char[] chset=@Characters[@mode];//[0]; //key: char[]
            return (
               chset[m]==' ' &&
               chset[m+((ROWS-1))]==' ' &&
               chset[m+((ROWS-1)*2)]==' ' &&
               chset[m+((ROWS-1)*3)]==' '
            );
		}

		public void pbUpdateOverlay()
		{
            @refreshOverlay=true;
		}

		public void pbDoUpdateOverlay()
		{
            if (!@refreshOverlay) return;
            @refreshOverlay=false;
            //bgoverlay=@sprites["bgoverlay"].bitmap;
            //bgoverlay.clear();
            //pbSetSystemFont(bgoverlay);
            //List<object> textPositions=new List<object>() {
            //    new object() { @helptext, 160, 12, false, new Color(16, 24, 32), new Color(168, 184, 184) }
            //};
            //char[] chars=@helper.textChars;
            //int x=166;
            //foreach (char ch in chars) {
            //  textPositions.Add(new object() { ch, x, 48, false, new Color(16, 24, 32), new Color(168, 184, 184) });
            //  x+=24;
            //}
            //pbDrawTextPositions(bgoverlay,textPositions);
		}

		public void pbDoUpdateOverlay2()
		{
            //overlay=@sprites["overlay"].bitmap;
            //overlay.clear();
            //modeIcon=[["Graphics/Pictures/namingMode",48+@mode*64,120,@mode*60,0,60,44]];
            //pbDrawImagePositions(overlay,modeIcon);
		}

		public bool pbMoveCursor()
		{
            int oldcursor=@cursorpos;
            int cursordiv=@cursorpos/ROWS;
            int cursormod=@cursorpos%ROWS;
            int cursororigin=@cursorpos-cursormod;
            /*if (Input.repeat(Input.LEFT)) {
              if (@cursorpos<0) {		// Controls
                @cursorpos-=1;
                if (@cursorpos<MODE1) @cursorpos=OK;
              } else {
                do { 
                  cursormod=wrapmod((cursormod-1),ROWS);
                  @cursorpos=cursororigin+cursormod;
                } while (pbColumnEmpty(cursormod));
              }
            } else if (Input.repeat(Input.RIGHT)) {
              if (@cursorpos<0) {		// Controls
                @cursorpos+=1;
                if (@cursorpos>OK) @cursorpos=MODE1;
              } else {
                do {
                  cursormod=wrapmod((cursormod+1),ROWS);
                  @cursorpos=cursororigin+cursormod;
                } while (pbColumnEmpty(cursormod));
              }
            } else if (Input.repeat(Input.UP)) {
              if (@cursorpos<0) {		// Controls
                switch (@cursorpos) {
                case MODE1:
                  @cursorpos=ROWS*(COLUMNS-1);
                  break;
                case MODE2:
                  @cursorpos=ROWS*(COLUMNS-1)+2;
                  break;
                case MODE3:
                  @cursorpos=ROWS*(COLUMNS-1)+4;
                  break;
                case BACK:
                  @cursorpos=ROWS*(COLUMNS-1)+8;
                  break;
                case OK:
                  @cursorpos=ROWS*(COLUMNS-1)+11;
                  break;
                }
              } else if (@cursorpos<ROWS) {		// Top row of letters
                switch (@cursorpos) {
                case 0: case 1:
                  @cursorpos=MODE1;
                  break;
                case 2: case 3:
                  @cursorpos=MODE2;
                  break;
                case 4: case 5: case 6:
                  @cursorpos=MODE3;
                  break;
                case 7: case 8: case 9: case 10:
                  @cursorpos=BACK;
                  break;
                case 11: case 12:
                  @cursorpos=OK;
                  break;
                }
              } else {
                cursordiv=wrapmod((cursordiv-1),COLUMNS);
                @cursorpos=(cursordiv*ROWS)+cursormod;
              }
            } else if (Input.repeat(Input.DOWN)) {
              if (@cursorpos<0) {		// Controls
                switch (@cursorpos) {
                case MODE1:
                  @cursorpos=0;
                  break;
                case MODE2:
                  @cursorpos=2;
                  break;
                case MODE3:
                  @cursorpos=4;
                  break;
                case BACK:
                  @cursorpos=8;
                  break;
                case OK:
                  @cursorpos=11;
                  break;
                }
              } else if (@cursorpos>=ROWS*(COLUMNS-1)) {		// Bottom row of letters
                switch (@cursorpos) {
                case ROWS*(COLUMNS-1): case ROWS*(COLUMNS-1)+1:
                  @cursorpos=MODE1;
                  break;
                case ROWS*(COLUMNS-1)+2: case ROWS*(COLUMNS-1)+3:
                  @cursorpos=MODE2;
                  break;
                case ROWS*(COLUMNS-1)+4: case ROWS*(COLUMNS-1)+5: case ROWS*(COLUMNS-1)+6:
                  @cursorpos=MODE3;
                  break;
                case ROWS*(COLUMNS-1)+7: case ROWS*(COLUMNS-1)+8: case ROWS*(COLUMNS-1)+9: case ROWS*(COLUMNS-1)+10:
                  @cursorpos=BACK;
                  break;
                case ROWS*(COLUMNS-1)+11: case ROWS*(COLUMNS-1)+12:
                  @cursorpos=OK;
                  break;
                }
              } else {
                cursordiv=wrapmod((cursordiv+1),COLUMNS);
                @cursorpos=(cursordiv*ROWS)+cursormod;
              }
            }*/
            if (@cursorpos!=oldcursor) {		// Cursor position changed
              //@sprites["cursor"].setCursorPos(@cursorpos);
              //pbPlayCursorSE();
              return true;
            } else {
              return false;
            }
		}

		public int wrapmod(int x, int y)
		{
            int result=x%y;
            if (result<0) result+=y;
            return result;
		}

		public void pbStartScene(string helptext, int minlength, int maxlength, string initialText, TextEntryTypes subject = 0, IPokemon pokemon = null)
		{
            //@sprites={}
            //@viewport=new Viewport(0,0,Graphics.width,Graphics.height);
            //@viewport.z=99999;
            //if (GameEntry.UseKeyboard) {
            //  @sprites["entry"]=new Window_TextEntry_Keyboard(initialText,0,0,400-112,96,helptext,true);
            //} else {
            //  @sprites["entry"]=new Window_TextEntry(initialText,0,0,400,96,helptext,true);
            //}
	        entry.text = initialText;
            //@sprites["entry"].x=(Graphics.width/2)-(@sprites["entry"].width/2)+32;
            //@sprites["entry"].viewport=@viewport;
            //@sprites["entry"].visible=true;
            Minlength=minlength;
            Maxlength=maxlength;
            @symtype=0;
            //@sprites["entry"].maxlength=maxlength;
            //if (!GameEntry.UseKeyboard) {
            //  @sprites["entry2"]=new Window_CharacterEntry(@Characters[@symtype][0]); //Key: Char Array
            //  @sprites["entry2"].setOtherCharset(@Characters[@symtype][1]); //Value: Id
            //  @sprites["entry2"].viewport=@viewport;
            //  @sprites["entry2"].visible=true;
            //  @sprites["entry2"].x=(Graphics.width/2)-(@sprites["entry2"].width/2);
            //}
            //if (minlength==0) {
            //  @sprites["helpwindow"]=new Window_UnformattedTextPokemon().WithSize(
            //     Game._INTL("Enter text using the keyboard. Press\nESC to cancel, or ENTER to confirm."),
            //     32,Graphics.height-96,Graphics.width-64,96,@viewport;
            //  );
            //} else {
            //  @sprites["helpwindow"]=new Window_UnformattedTextPokemon().WithSize(
            //     Game._INTL("Enter text using the keyboard.\nPress ENTER to confirm."),
            //     32,Graphics.height-96,Graphics.width-64,96,@viewport;
            //  );
            //}
            //@sprites["helpwindow"].letterbyletter=false;
            //@sprites["helpwindow"].viewport=@viewport;
            //@sprites["helpwindow"].visible=GameEntry.UseKeyboard;
            //@sprites["helpwindow"].baseColor=new Color(16,24,32);
            //@sprites["helpwindow"].shadowColor=new Color(168,184,184);
            //addBackgroundPlane(@sprites,"background","naming2bg",@viewport);
            //switch (subject) {
            //case 1:   // Player
            //  if (Game.GameData.Global) {
            //    meta=pbGetMetadata(0,MetadataPlayerA+Game.GameData.Global.playerID);
            //    if (meta != null) {
            //      @sprites["shadow"]=new IconSprite(0,0,@viewport);
            //      @sprites["shadow"].setBitmap("Graphics/Pictures/namingShadow");
            //      @sprites["shadow"].x=33*2;
            //      @sprites["shadow"].y=32*2;
            //      string filename=pbGetPlayerCharset(meta,1);
            //      @sprites["subject"]=new TrainerWalkingCharSprite(filename,@viewport);
            //      int charwidth=@sprites["subject"].bitmap.width;
            //      int charheight=@sprites["subject"].bitmap.height;
            //      @sprites["subject"].x = 44*2 - charwidth/8;
            //      @sprites["subject"].y = 38*2 - charheight/4;
            //    }
            //  }
            //  break;
            //case 2:   // Pokémon
            //  if (pokemon) {
            //    @sprites["shadow"]=new IconSprite(0,0,@viewport);
            //    @sprites["shadow"].setBitmap("Graphics/Pictures/namingShadow");
            //    @sprites["shadow"].x=33*2;
            //    @sprites["shadow"].y=32*2;
            //    @sprites["subject"]=new PokemonIconSprite(pokemon,@viewport);
            //    @sprites["subject"].x=56;
            //    @sprites["subject"].y=14;
            //    @sprites["gender"]=new BitmapSprite(32,32,@viewport);
            //    @sprites["gender"].x=430;
            //    @sprites["gender"].y=54;
            //    @sprites["gender"].bitmap.clear;
            //    pbSetSystemFont(@sprites["gender"].bitmap);
            //    textpos=[];
            //    if (pokemon.isMale?) {
            //      textpos.Add([_INTL("♂"),0,0,false,new Color(0,128,248),new Color(168,184,184)]);
            //    } else if (pokemon.isFemale?) {
            //      textpos.Add([_INTL("♀"),0,0,false,new Color(248,24,24),new Color(168,184,184)]);
            //    }
            //    pbDrawTextPositions(@sprites["gender"].bitmap,textpos);
            //  }
            //  break;
            //case 3:   // Storage box
            //  @sprites["subject"]=new IconSprite(0,0,@viewport);
            //  @sprites["subject"].setBitmap("Graphics/Pictures/namingStorage");
            //  @sprites["subject"].x=68;
            //  @sprites["subject"].y=32;
            //  break;
            //case 4:   // NPC
            //  @sprites["shadow"]=new IconSprite(0,0,@viewport);
            //  @sprites["shadow"].setBitmap("Graphics/Pictures/namingShadow");
            //  @sprites["shadow"].x=33*2;
            //  @sprites["shadow"].y=32*2;
            //  @sprites["subject"]=new TrainerWalkingCharSprite(pokemon.ToString(),@viewport);
            //  charwidth=@sprites["subject"].bitmap.width;
            //  charheight=@sprites["subject"].bitmap.height;
            //  @sprites["subject"].x = 44*2 - charwidth/8;
            //  @sprites["subject"].y = 38*2 - charheight/4;
            //  break;
            //}
            //pbFadeInAndShow(@sprites);
		}

		public void pbEndScene()
		{
            //pbFadeOutAndHide(@sprites, () => { pbUpdate(); });
            //foreach (var bitmap in @bitmaps) {
            //  if (bitmap) bitmap.dispose();
            //}
            //@bitmaps.clear();
            //pbDisposeSpriteHash(@sprites);
            //@viewport.dispose();
		}

		public string pbEntry()
		{
			return GameEntry.UseKeyboard ? pbEntry1() : pbEntry2();
		}

		public string pbEntry1()
		{
            string ret="";
            //do { //;loop
            //  Graphics.update();
            //  Input.update();
            //  if (Input.triggerex(0x1B) && @minlength==0) {
            //    ret="";
            //    break;
            //  }
            //  if (Input.triggerex(13) && @sprites["entry"].text.Length>=@minlength) {
            //    ret=@sprites["entry"].text;
            //    break;
            //  }
            //  @sprites["helpwindow"].update();
            //  @sprites["entry"].update();
            //  if (@sprites["subject"]) @sprites["subject"].update();
            //}
            //Input.update();
            return ret;
		}

		public string pbEntry2()
		{
            string ret="";
            //do { //;loop
            //  Graphics.update();
            //  Input.update();
            //  @sprites["helpwindow"].update();
            //  @sprites["entry"].update();
            //  @sprites["entry2"].update();
            //  if (@sprites["subject"]) @sprites["subject"].update();
            //  if (Input.trigger(Input.t)) {
                //int index=@sprites["entry2"].command;
                //if (index==-3) {		// Confirm text
                //  ret=@sprites["entry"].text;
                //  if (ret.Length<@minlength || ret.Length>Maxlength) {
                //    //pbPlayBuzzerSE();
                //  } else {
                //    //pbPlayDecisionSE();
                //    //break;
                //  }
                //} else if (index==-1) {		// Insert a space
                //  if (@sprites["entry"].insert(" ")) {
                //    pbPlayDecisionSE();
                //  } else {
                //    pbPlayBuzzerSE();
                //  }
                //} else if (index==-2) {		// Change character set
                //  pbPlayDecisionSE();
                //  @symtype+=1;
                //  if (@symtype>=@Characters.Length) @symtype=0;
                //  @sprites["entry2"].setCharset(@Characters[@symtype][0]);
                //  @sprites["entry2"].setOtherCharset(@Characters[@symtype][1]);
                //} else { // Insert given character
                //  if (@sprites["entry"].insert(@sprites["entry2"].character)) {
                //    pbPlayDecisionSE() ;
                //  } else {
                //    pbPlayBuzzerSE();
                //  }
                //}
            //    continue;
            //  }
            //} while (true);
            //Input.update();
            return ret;
		}

		public void pbDisplay(string v)
		{
			throw new System.NotImplementedException();
		}

        public partial class NameEntryCursor {
			    public int @cursorPos;
                public void initialize(IViewport viewport) {
                    //@sprite=new SpriteWrapper(viewport);
                    //@cursortype=0;
                    //@cursor1=new AnimatedBitmap("Graphics/Pictures/NamingCursor1");
                    //@cursor2=new AnimatedBitmap("Graphics/Pictures/NamingCursor2");
                    //@cursor3=new AnimatedBitmap("Graphics/Pictures/NamingCursor3");
                    @cursorPos=0;
                    updateInternal();
                }

                public void setCursorPos(int value) {
                    @cursorPos=value;
                }

                public void updateCursorPos() {
                    int value=@cursorPos;
                    //if (value==PokemonEntryScene.MODE1) {		// Upper case
                    //  @sprite.x=48;
                    //  @sprite.y=120;
                    //  @cursortype=1;
                    //} else if (value==PokemonEntryScene.MODE2) {		// Lower case
                    //  @sprite.x=112;
                    //  @sprite.y=120;
                    //  @cursortype=1;
                    //} else if (value==PokemonEntryScene.MODE3) {		// Other symbols
                    //  @sprite.x=176;
                    //  @sprite.y=120;
                    //  @cursortype=1;
                    //} else if (value==PokemonEntryScene.BACK) {		// Back
                    //  @sprite.x=312;
                    //  @sprite.y=120;
                    //  @cursortype=2;
                    //} else if (value==PokemonEntryScene.OK) {		// OK
                    //  @sprite.x=392;
                    //  @sprite.y=120;
                    //  @cursortype=2;
                    //} else if (value>=0) {
                    //  @sprite.x=52+32*(value%PokemonEntryScene.ROWS);
                    //  @sprite.y=180+38*(value/PokemonEntryScene.ROWS);
                    //  @cursortype=0;
                    //}
                }

                //public bool visible { get {
                //  return @sprite.visible;
                //}
                //set {
                //  @sprite.visible=value;
                //} }
                //
                //public UnityEngine.Color color { get {
                //  return @sprite.color;
                //}
                //set {
                //  @sprite.color=value;
                //} }
	            //		
                //public bool disposed { get {
                //  return @sprite.disposed;
                //} }

                public void updateInternal() {
                    //@cursor1.update();
                    //@cursor2.update();
                    //@cursor3.update();
                    //updateCursorPos();
                    //switch (@cursortype) {
                    //case 0:
                    //  @sprite.bitmap=@cursor1.bitmap;
                    //  break;
                    //case 1:
                    //  @sprite.bitmap=@cursor2.bitmap;
                    //  break;
                    //case 2:
                    //  @sprite.bitmap=@cursor3.bitmap;
                    //  break;
                    //}
                }

                public void update() {
                    updateInternal();
                }

                public void dispose() {
                    //@cursor1.dispose();
                    //@cursor2.dispose();
                    //@cursor3.dispose();
                    //@sprite.dispose();
                }
            }
	}
}