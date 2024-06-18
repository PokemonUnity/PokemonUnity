using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
//using PokemonEssentials.Interface.PokeBattle.Rules;

namespace PokemonUnity.Interface.UnityEngine
{
	/// <summary>
	/// </summary>
	/// <remarks>
	/// This one class is used as parent gameobject for <seealso cref="FightMoveButton"/> buttons,
	/// to manipulate children gameobjects (<see cref="IBattler.moves"/>_1, <see cref="IBattler.moves"/>_2, <see cref="IBattler.moves"/>_3, <see cref="IBattler.moves"/>_4)
	/// </remarks>
	public class FightMenuButtons : SpriteWrapper, IFightMenuButtons, IGameObject //BitmapSprite
	{
		const int UPPERGAP=46;
		public IAnimatedBitmap buttonbitmap;
		public IAnimatedBitmap typebitmap;
		public IAnimatedBitmap megaevobitmap;
		private FightMenuDisplay FightMenuDisplay;
		//private global::UnityEngine.Color buttonTextDefaultColor = global::UnityEngine.Color.white;
		//private global::UnityEngine.Color buttonTextSelectedColor = new global::UnityEngine.Color(0.04313726f, 0.07058824f, 0.1843137f); //HEX=>0B122F
		[SerializeField] private global::UnityEngine.RectTransform cursor;
		[SerializeField] private global::UnityEngine.Sprite[] spriteTypes;
		[SerializeField] private global::UnityEngine.Sprite[] spriteButtons;
		[SerializeField] private global::UnityEngine.Sprite[] spriteMegaEvolve;
		[SerializeField] private FightMoveButton button1;
		[SerializeField] private FightMoveButton button2;
		[SerializeField] private FightMoveButton button3;
		[SerializeField] private FightMoveButton button4;
		//[SerializeField] private FightMoveButton selected;
		[SerializeField] private global::UnityEngine.UI.Toggle buttonMega;
		private static global::UnityEngine.Sprite spriteNull; //= Resources.Load<global::UnityEngine.Sprite>("null");

		private void Awake()
		{
			spriteNull = Resources.Load<global::UnityEngine.Sprite>("null");
		}

		public IFightMenuButtons initialize(int index= 0, IBattleMove[] moves = null, IViewport viewport= null)
		{
			//super((Game.GameData as Game).Graphics.width, 96 + UPPERGAP, viewport);
			//(this as IBitmapSprite).initialize((Game.GameData as Game).Graphics.width, 96 + UPPERGAP, viewport);
			/*this.x = 0;
			this.y = (Game.GameData as Game).Graphics.height - 96 - UPPERGAP;
			SetNarrowFont(this.bitmap);*/
			//@buttonbitmap = new AnimatedBitmap(Game._INTL("Graphics/Pictures/battleFightButtons"));
			//@typebitmap = new AnimatedBitmap(Game._INTL("Graphics/Pictures/types"));
			//@megaevobitmap = new AnimatedBitmap(Game._INTL("Graphics/Pictures/battleMegaEvo"));
			//ToDo: Use global::UnityEngine.Sprite instead for below...
			@buttonbitmap?.initialize(Game._INTL("Graphics/Pictures/battleFightButtons"));
			@typebitmap?.initialize(Game._INTL("Graphics/Pictures/types"));
			@megaevobitmap?.initialize(Game._INTL("Graphics/Pictures/battleMegaEvo"));
			if (viewport is IFightMenuDisplay)
				FightMenuDisplay = (FightMenuDisplay)viewport;
			refresh(index, moves, 0);
			return this;
		}

		public void update(int index= 0, IBattleMove[] moves = null, int megaButton= 0)
		{
			refresh(index, moves, megaButton);
		}

		public void refresh(int index, IBattleMove[] moves, int megaButton)
		{
			//global::UnityEngine.Sprite spriteNull = Resources.Load<global::UnityEngine.Sprite>("null");
			if (moves == null) return;
			//this.bitmap.clear();
			//IList<ITextPosition> textpos = new List<ITextPosition>();
			global::UnityEngine.Color[] ppcolors = new global::UnityEngine.Color[] { //IColor[] ppcolors=new IColor[] {
				global::UnityEngine.Color.white,		//PokeBattle_SceneConstants.PPTEXTBASECOLOR, PokeBattle_SceneConstants.PPTEXTSHADOWCOLOR,
				global::UnityEngine.Color.white,		//PokeBattle_SceneConstants.PPTEXTBASECOLOR, PokeBattle_SceneConstants.PPTEXTSHADOWCOLOR,
				global::UnityEngine.Color.yellow,		//PokeBattle_SceneConstants.PPTEXTBASECOLORYELLOW, PokeBattle_SceneConstants.PPTEXTSHADOWCOLORYELLOW,
				global::UnityEngine.Color.magenta,		//PokeBattle_SceneConstants.PPTEXTBASECOLORORANGE, PokeBattle_SceneConstants.PPTEXTSHADOWCOLORORANGE,
				global::UnityEngine.Color.red			//PokeBattle_SceneConstants.PPTEXTBASECOLORRED, PokeBattle_SceneConstants.PPTEXTSHADOWCOLORRED
			};
			//ToDo: Can merge the two for-loops into one, as there is no need to instantiate any object into scene by any set order...
			for (int i = 0; i < 4; i++)
			{
				if (i == index) continue; //if selected, ignore...
				//if (moves[i].id == Moves.NONE) continue; //if null, override and clear the text from previous entry
				//x = ((i % 2) == 0) ? 4 : 192;   //move it to left or right, if it's odd or even
				//y = ((i / 2) == 0) ? 6 : 48;    //move it up or down, if it's even or odd
				//y += UPPERGAP;
				//this.bitmap.blt(x, y, @buttonbitmap.bitmap, new Rect(0, moves[i].Type * 46, 192, 46));
				//textpos.Add(new TextPosition(Game._INTL("{1}", moves[i].Name), x + 96, y + 8, null,
				//   PokeBattle_SceneConstants.MENUBASECOLOR, PokeBattle_SceneConstants.MENUSHADOWCOLOR));
				if (i == 0)
				{
					if (moves[i].id == Moves.NONE)
					{
						button1.Type.sprite = spriteNull;
						//button1.Background.sprite = spriteNull;
						button1.Move.SetText("------");
						button1.PP.SetText("--/--");
						continue;
					}
					//button1.GetComponent<global::UnityEngine.UI.Image>().sprite = spriteTypes[i];		//Assign Move-Type Sprite
					//button1.GetComponent<global::UnityEngine.UI.Image>().sprite = spriteButtons[i];	//Assign Move-Type Color to Button BG?
					//button1.GetComponentInChildren<TMPro.TextMeshProUGUI>().SetText(Game._INTL("{1}", moves[i].Name));
					//button1.Type.sprite = spriteTypes[(int)moves[i].Type];			//Assign Move-Type Sprite
					//button1.Background.sprite = spriteButtons[(int)moves[i].Type];		//Assign Move-Type Color to Button BG?
					//button1.Move.text = Game._INTL("{1}", moves[i].Name);
					//button1.PP.text = Game._INTL("PP: {1}/{2}", moves[i].PP, moves[i].TotalPP);
					//button1.Move.color = buttonTextDefaultColor;
					button1.Move.SetText(Game._INTL("{1}", moves[i].Name));
					button1.PP.SetText("--/--");
					if (moves[i].TotalPP > 0)
					{
						int ppfraction = (int)Math.Ceiling(4.0 * moves[i].PP / moves[i].TotalPP);
						button1.PP.color = ppcolors[(4 - ppfraction) * 2];
						button1.PP.SetText(Game._INTL("{1}/{2}", moves[i].PP, moves[i].TotalPP));
					}
				}
				else if (i == 1)
				{
					if (moves[i].id == Moves.NONE)
					{
						button2.Type.sprite = spriteNull;
						//button2.Background.sprite = spriteNull;
						button2.Move.SetText("------");
						button2.PP.SetText("--/--");
						continue;
					}
					//button2.GetComponent<global::UnityEngine.UI.Image>().sprite = spriteTypes[i];		//Assign Move-Type Sprite
					//button2.GetComponent<global::UnityEngine.UI.Image>().sprite = spriteButtons[i];	//Assign Move-Type Color to Button BG?
					//button2.GetComponentInChildren<TMPro.TextMeshProUGUI>().SetText(Game._INTL("{1}", moves[i].Name));
					//button2.Type.sprite = spriteTypes[i];			//Assign Move-Type Sprite
					//button2.Background.sprite = spriteButtons[i];		//Assign Move-Type Color to Button BG?
					//button2.Move.text = Game._INTL("{1}", moves[i].Name);
					//button2.PP.text = Game._INTL("PP: {1}/{2}", moves[i].PP, moves[i].TotalPP);
					//button2.Move.color = buttonTextDefaultColor;
					button2.Move.SetText(Game._INTL("{1}", moves[i].Name));
					button2.PP.SetText("--/--");
					if (moves[i].TotalPP > 0)
					{
						int ppfraction = (int)Math.Ceiling(4.0 * moves[i].PP / moves[i].TotalPP);
						button2.PP.color = ppcolors[(4 - ppfraction) * 2];
						button2.PP.SetText(Game._INTL("{1}/{2}", moves[i].PP, moves[i].TotalPP));
					}
				}
				else if (i == 2)
				{
					if (moves[i].id == Moves.NONE)
					{
						button3.Type.sprite = spriteNull;
						//button3.Background.sprite = spriteNull;
						button3.Move.SetText("------");
						button3.PP.SetText("--/--");
						continue;
					}
					//button3.GetComponent<global::UnityEngine.UI.Image>().sprite = spriteTypes[i];		//Assign Move-Type Sprite
					//button3.GetComponent<global::UnityEngine.UI.Image>().sprite = spriteButtons[i];	//Assign Move-Type Color to Button BG?
					//button3.GetComponentInChildren<TMPro.TextMeshProUGUI>().SetText(Game._INTL("{1}", moves[i].Name));
					//button3.Type.sprite = spriteTypes[i];			//Assign Move-Type Sprite
					//button3.Background.sprite = spriteButtons[i];		//Assign Move-Type Color to Button BG?
					//button3.Move.text = Game._INTL("{1}", moves[i].Name);
					//button3.PP.text = Game._INTL("PP: {1}/{2}", moves[i].PP, moves[i].TotalPP);
					//button3.Move.color = buttonTextDefaultColor;
					button3.Move.SetText(Game._INTL("{1}", moves[i].Name));
					button3.PP.SetText("--/--");
					if (moves[i].TotalPP > 0)
					{
						int ppfraction = (int)Math.Ceiling(4.0 * moves[i].PP / moves[i].TotalPP);
						button3.PP.color = ppcolors[(4 - ppfraction) * 2];
						button3.PP.SetText(Game._INTL("{1}/{2}", moves[i].PP, moves[i].TotalPP));
					}
				}
				else if (i == 3)
				{
					if (moves[i].id == Moves.NONE)
					{
						button4.Type.sprite = spriteNull;
						//button4.Background.sprite = spriteNull;
						button4.Move.SetText("------");
						button4.PP.SetText("--/--");
						continue;
					}
					//button4.GetComponent<global::UnityEngine.UI.Image>().sprite = spriteTypes[i];		//Assign Move-Type Sprite
					//button4.GetComponent<global::UnityEngine.UI.Image>().sprite = spriteButtons[i];	//Assign Move-Type Color to Button BG?
					//button4.GetComponentInChildren<TMPro.TextMeshProUGUI>().SetText(Game._INTL("{1}", moves[i].Name));
					//button4.Type.sprite = spriteTypes[i];			//Assign Move-Type Sprite
					//button4.Background.sprite = spriteButtons[i];		//Assign Move-Type Color to Button BG?
					//button4.Move.text = Game._INTL("{1}", moves[i].Name);
					//button4.PP.text = Game._INTL("PP: {1}/{2}", moves[i].PP, moves[i].TotalPP);
					//button4.Move.color = buttonTextDefaultColor;
					button4.Move.SetText(Game._INTL("{1}", moves[i].Name));
					button4.PP.SetText("--/--");
					if (moves[i].TotalPP > 0)
					{
						int ppfraction = (int)Math.Ceiling(4.0 * moves[i].PP / moves[i].TotalPP);
						button4.PP.color = ppcolors[(4 - ppfraction) * 2];
						button4.PP.SetText(Game._INTL("{1}/{2}", moves[i].PP, moves[i].TotalPP));
					}
				}
			}
			//Instantiate and assign selected last...
			for (int i = 0; i < 4; i++) {
				if (i != index) continue; //if not selected, ignore...
				if (moves[i].id == Moves.NONE) continue;
				//x=((i%2)==0) ? 4 : 192;	//move it to left or right, if it's odd or even
				//y=((i/2)==0) ? 6 : 48;	//move it up or down, if it's even or odd
				//y+=UPPERGAP;
				//this.bitmap.blt(x, y, @buttonbitmap.bitmap,new Rect(192, moves[i].Type*46,192,46));
				//this.bitmap.blt(416,20+UPPERGAP,@typebitmap.bitmap,new Rect(0, moves[i].Type*28,64,28));
				//textpos.Add(new TextPosition(Game._INTL("{1}", moves[i].Name), x + 96, y + 8, null,
				//	PokeBattle_SceneConstants.MENUBASECOLOR, PokeBattle_SceneConstants.MENUSHADOWCOLOR));
				if (i == 0)
				{
					//button1.GetComponent<global::UnityEngine.UI.Image>().sprite = spriteTypes[i];		//Assign Move-Type Sprite
					//button1.GetComponent<global::UnityEngine.UI.Image>().sprite = spriteButtons[i];	//Assign Move-Type Color to Button BG?
					//button1.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = buttonTextSelectedColor;
					//button1.GetComponentInChildren<TMPro.TextMeshProUGUI>().SetText(Game._INTL("{1}", moves[i].Name));
					cursor.position = button1.GetComponent<RectTransform>().position;
					//button1.Type.sprite = spriteTypes[i];			//Assign Move-Type Sprite
					//button1.Background.sprite = spriteButtons[i];		//Assign Move-Type Color to Button BG?
					//button1.Move.text = Game._INTL("{1}", moves[i].Name);
					//button1.PP.text = Game._INTL("PP: {1}/{2}", moves[i].PP, moves[i].TotalPP);
					//button1.Move.color = buttonTextSelectedColor;
					button1.Move.SetText(Game._INTL("{1}", moves[i].Name));
					if (moves[i].TotalPP > 0)
					{
						int ppfraction = (int)Math.Ceiling(4.0 * moves[i].PP / moves[i].TotalPP);
						button1.PP.color = ppcolors[(4 - ppfraction) * 2];
						button1.PP.SetText(Game._INTL("{1}/{2}", moves[i].PP, moves[i].TotalPP));
					}
				}
				else if (i == 1)
				{
					//button2.GetComponent<global::UnityEngine.UI.Image>().sprite = spriteTypes[i];		//Assign Move-Type Sprite
					//button2.GetComponent<global::UnityEngine.UI.Image>().sprite = spriteButtons[i];	//Assign Move-Type Color to Button BG?
					//button2.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = buttonTextSelectedColor;
					//button2.GetComponentInChildren<TMPro.TextMeshProUGUI>().SetText(Game._INTL("{1}", moves[i].Name));
					cursor.position = button2.GetComponent<RectTransform>().position;
					//button2.Type.sprite = spriteTypes[i];			//Assign Move-Type Sprite
					//button2.Background.sprite = spriteButtons[i];		//Assign Move-Type Color to Button BG?
					//button2.Move.text = Game._INTL("{1}", moves[i].Name);
					//button2.PP.text = Game._INTL("PP: {1}/{2}", moves[i].PP, moves[i].TotalPP);
					//button2.Move.color = buttonTextSelectedColor;
					button2.Move.SetText(Game._INTL("{1}", moves[i].Name));
					if (moves[i].TotalPP > 0)
					{
						int ppfraction = (int)Math.Ceiling(4.0 * moves[i].PP / moves[i].TotalPP);
						button2.PP.color = ppcolors[(4 - ppfraction) * 2];
						button2.PP.SetText(Game._INTL("{1}/{2}", moves[i].PP, moves[i].TotalPP));
					}
				}
				else if (i == 2)
				{
					//button3.GetComponent<global::UnityEngine.UI.Image>().sprite = spriteTypes[i];		//Assign Move-Type Sprite
					//button3.GetComponent<global::UnityEngine.UI.Image>().sprite = spriteButtons[i];	//Assign Move-Type Color to Button BG?
					//button3.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = buttonTextSelectedColor;
					//button3.GetComponentInChildren<TMPro.TextMeshProUGUI>().SetText(Game._INTL("{1}", moves[i].Name));
					cursor.position = button3.GetComponent<RectTransform>().position;
					//button3.Type.sprite = spriteTypes[i];			//Assign Move-Type Sprite
					//button3.Background.sprite = spriteButtons[i];		//Assign Move-Type Color to Button BG?
					//button3.Move.text = Game._INTL("{1}", moves[i].Name);
					//button3.PP.text = Game._INTL("PP: {1}/{2}", moves[i].PP, moves[i].TotalPP);
					//button3.Move.color = buttonTextSelectedColor;
					button3.Move.SetText(Game._INTL("{1}", moves[i].Name));
					if (moves[i].TotalPP > 0)
					{
						int ppfraction = (int)Math.Ceiling(4.0 * moves[i].PP / moves[i].TotalPP);
						button3.PP.color = ppcolors[(4 - ppfraction) * 2];
						button3.PP.SetText(Game._INTL("{1}/{2}", moves[i].PP, moves[i].TotalPP));
					}
				}
				else if (i == 3)
				{
					//button4.GetComponent<global::UnityEngine.UI.Image>().sprite = spriteTypes[i];		//Assign Move-Type Sprite
					//button4.GetComponent<global::UnityEngine.UI.Image>().sprite = spriteButtons[i];	//Assign Move-Type Color to Button BG?
					//button4.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = buttonTextSelectedColor;
					//button4.GetComponentInChildren<TMPro.TextMeshProUGUI>().SetText(Game._INTL("{1}", moves[i].Name));
					cursor.position = button4.GetComponent<RectTransform>().position;
					//button4.Type.sprite = spriteTypes[i];			//Assign Move-Type Sprite
					//button4.Background.sprite = spriteButtons[i];		//Assign Move-Type Color to Button BG?
					//button4.Move.text = Game._INTL("{1}", moves[i].Name);
					//button4.PP.text = Game._INTL("PP: {1}/{2}", moves[i].PP, moves[i].TotalPP);
					//button4.Move.color = buttonTextSelectedColor;
					button4.Move.SetText(Game._INTL("{1}", moves[i].Name));
					if (moves[i].TotalPP>0) {
						int ppfraction=(int)Math.Ceiling(4.0*moves[i].PP/moves[i].TotalPP);
						button4.PP.color = ppcolors[(4 - ppfraction) * 2];
						button4.PP.SetText(Game._INTL("{1}/{2}", moves[i].PP, moves[i].TotalPP));
					}
				}
				//if (moves[i].TotalPP>0) {
				//	int ppfraction=(int)Math.Ceiling(4.0*moves[i].PP/moves[i].TotalPP);
				//	//textpos.Add(new TextPosition(Game._INTL("PP: {1}/{2}", moves[i].PP, moves[i].TotalPP),
				//	//	448, 50 + UPPERGAP, null, ppcolors[(4 - ppfraction) * 2], ppcolors[(4 - ppfraction) * 2 + 1]));
				//	selected.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = (global::UnityEngine.Color)ppcolors[(4 - ppfraction) * 2];
				//	selected.GetComponentInChildren<TMPro.TextMeshProUGUI>().SetText(Game._INTL("PP: {1}/{2}", moves[i].PP, moves[i].TotalPP));
				//}
			}
			//DrawTextPositions(this.bitmap, textpos); //foreach text in array, add as child to gameobject with sprite as background image
			if (buttonMega != null)
				if (megaButton > 0) //0=don't show, 1=show, 2=pressed
				{
					//this.bitmap.blt(146, 0, @megaevobitmap.bitmap, new Rect(0, (megaButton - 1) * 46, 96, 46));
					buttonMega.image.sprite = spriteMegaEvolve[megaButton];
					buttonMega.isOn = megaButton==2;
				}
				else
					buttonMega.image.sprite = spriteNull;
		}

		protected override void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects)
					@buttonbitmap.Dispose();
					@typebitmap.Dispose();
					@megaevobitmap.Dispose();
					base.Dispose(disposing);//super;
				}

				// TODO: free unmanaged resources (unmanaged objects) and override finalizer
				// TODO: set large fields to null
				//disposed = true;
			}
		}

		// // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
		// ~FightMenuDisplay()
		// {
		//     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		//     Dispose(disposing: false);
		// }

		//void IDisposable.Dispose()
		//{
		//	// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		//	Dispose(disposing: true);
		//	GC.SuppressFinalize(this);
		//}

		#region
		IBitmapSprite IBitmapSprite.initialize(int width, int height, IViewport viewport)
		{
			//throw new NotImplementedException();
			return this;
		}

		/*ISpriteWrapper ISpriteWrapper.initialize(IViewport viewport)
		{
			throw new NotImplementedException();
		}

		void ISpriteWrapper.flash(IColor color, int duration)
		{
			throw new NotImplementedException();
		}

		void ISpriteWrapper.update()
		{
			throw new NotImplementedException();
		}

		void ISprite.whiten()
		{
			throw new NotImplementedException();
		}

		void ISprite.appear()
		{
			throw new NotImplementedException();
		}

		void ISprite.escape()
		{
			throw new NotImplementedException();
		}

		void ISprite.collapse()
		{
			throw new NotImplementedException();
		}

		void ISprite.damage(int value, bool critical)
		{
			throw new NotImplementedException();
		}

		void ISprite.dispose_damage()
		{
			throw new NotImplementedException();
		}

		void ISprite.dispose_animation()
		{
			throw new NotImplementedException();
		}

		void ISprite.dispose_loop_animation()
		{
			throw new NotImplementedException();
		}

		void ISprite.blink_on()
		{
			throw new NotImplementedException();
		}

		void ISprite.blink_off()
		{
			throw new NotImplementedException();
		}

		void ISprite.update_animation()
		{
			throw new NotImplementedException();
		}

		void ISprite.update_loop_animation()
		{
			throw new NotImplementedException();
		}

		void IRPGSprite.update()
		{
			throw new NotImplementedException();
		}

		void IRPGSprite.flash(IColor color, int duration)
		{
			throw new NotImplementedException();
		}

		IRPGSprite IRPGSprite.initialize(IViewport viewport)
		{
			throw new NotImplementedException();
		}*/
		#endregion
	}
}