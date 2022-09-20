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

namespace PokemonUnity
{
	/// <summary>
	/// </summary>
	/// <remarks>
	/// This one class is used as parent gameobject for menu buttons, to manipulate children gaomeobjects (move1, move2, move3, move4)
	/// </remarks>
	public class FightMenuButtons : SpriteWrapper, IFightMenuButtons //BitmapSprite
	{
		const int UPPERGAP=46;
		public IAnimatedBitmap buttonbitmap;
		public IAnimatedBitmap typebitmap;
		public IAnimatedBitmap megaevobitmap;
		private FightMenuDisplay FightMenuDisplay;
		[SerializeField] private UnityEngine.RectTransform cursor;
		[SerializeField] private UnityEngine.Sprite[] spriteTypes;
		[SerializeField] private UnityEngine.Sprite[] spriteButtons;
		[SerializeField] private UnityEngine.Sprite[] spriteMegaEvolve;
		[SerializeField] private UnityEngine.GameObject button1;
		[SerializeField] private UnityEngine.GameObject button2;
		[SerializeField] private UnityEngine.GameObject button3;
		[SerializeField] private UnityEngine.GameObject button4;
		[SerializeField] private UnityEngine.GameObject selected;
		[SerializeField] private UnityEngine.UI.Toggle buttonMega;

		public IFightMenuButtons initialize(int index= 0, IBattleMove[] moves = null, IViewport viewport= null)
		{
			//super((Game.GameData as Game).Graphics.width, 96 + UPPERGAP, viewport);
			//(this as IBitmapSprite).initialize((Game.GameData as Game).Graphics.width, 96 + UPPERGAP, viewport);
			/*this.x = 0;
			this.y = (Game.GameData as Game).Graphics.height - 96 - UPPERGAP;
			pbSetNarrowFont(this.bitmap);*/
			//@buttonbitmap = new AnimatedBitmap(Game._INTL("Graphics/Pictures/battleFightButtons"));
			//@typebitmap = new AnimatedBitmap(Game._INTL("Graphics/Pictures/types"));
			//@megaevobitmap = new AnimatedBitmap(Game._INTL("Graphics/Pictures/battleMegaEvo"));
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
			if (moves == null) return;
			//this.bitmap.clear();
			//IList<ITextPosition> textpos = new List<ITextPosition>();
			//ToDo: Can merge the two for-loops into one, as there is no need to instantiate any object into scene by any set order...
			for (int i = 0; i < 4; i++)
			{
				if (i == index) continue; //if selected
				if (moves[i].id == Moves.NONE) continue;
				//x = ((i % 2) == 0) ? 4 : 192;	//move it to left or right, if it's odd or even
				//y = ((i / 2) == 0) ? 6 : 48;  //move it up or down, if it's even or odd
				//y += UPPERGAP;
				//this.bitmap.blt(x, y, @buttonbitmap.bitmap, new Rect(0, moves[i].Type * 46, 192, 46));
				//textpos.Add(new TextPosition(Game._INTL("{1}", moves[i].Name), x + 96, y + 8, 2,
				//   PokeBattle_SceneConstants.MENUBASECOLOR, PokeBattle_SceneConstants.MENUSHADOWCOLOR));
				if (i == 0)
				{
					//button1.GetComponent<UnityEngine.UI.Image>().sprite = spriteTypes[i];		//Assign Move-Type Sprite
					//button1.GetComponent<UnityEngine.UI.Image>().sprite = spriteButtons[i];	//Assign Move-Type Color to Button BG?
					//button1.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = buttonTextSelectedColor;
					button1.GetComponentInChildren<TMPro.TextMeshProUGUI>().SetText(Game._INTL("{1}", moves[i].Name));
				}
				else if (i == 1)
				{
					//button2.GetComponent<UnityEngine.UI.Image>().sprite = spriteTypes[i];		//Assign Move-Type Sprite
					//button2.GetComponent<UnityEngine.UI.Image>().sprite = spriteButtons[i];	//Assign Move-Type Color to Button BG?
					//button2.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = buttonTextSelectedColor;
					button2.GetComponentInChildren<TMPro.TextMeshProUGUI>().SetText(Game._INTL("{1}", moves[i].Name));
				}
				else if (i == 2)
				{
					//button3.GetComponent<UnityEngine.UI.Image>().sprite = spriteTypes[i];		//Assign Move-Type Sprite
					//button3.GetComponent<UnityEngine.UI.Image>().sprite = spriteButtons[i];	//Assign Move-Type Color to Button BG?
					//button3.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = buttonTextSelectedColor;
					button3.GetComponentInChildren<TMPro.TextMeshProUGUI>().SetText(Game._INTL("{1}", moves[i].Name));
				}
				else if (i == 3)
				{
					//button4.GetComponent<UnityEngine.UI.Image>().sprite = spriteTypes[i];		//Assign Move-Type Sprite
					//button4.GetComponent<UnityEngine.UI.Image>().sprite = spriteButtons[i];	//Assign Move-Type Color to Button BG?
					//button4.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = buttonTextSelectedColor;
					button4.GetComponentInChildren<TMPro.TextMeshProUGUI>().SetText(Game._INTL("{1}", moves[i].Name));
				}
			}
			UnityEngine.Color[] ppcolors = new UnityEngine.Color[] { //IColor[] ppcolors=new IColor[] {
				UnityEngine.Color.black,		//PokeBattle_SceneConstants.PPTEXTBASECOLOR, PokeBattle_SceneConstants.PPTEXTSHADOWCOLOR,
				UnityEngine.Color.black,		//PokeBattle_SceneConstants.PPTEXTBASECOLOR, PokeBattle_SceneConstants.PPTEXTSHADOWCOLOR,
				UnityEngine.Color.yellow,		//PokeBattle_SceneConstants.PPTEXTBASECOLORYELLOW, PokeBattle_SceneConstants.PPTEXTSHADOWCOLORYELLOW,
				UnityEngine.Color.magenta,		//PokeBattle_SceneConstants.PPTEXTBASECOLORORANGE, PokeBattle_SceneConstants.PPTEXTSHADOWCOLORORANGE,
				UnityEngine.Color.red			//PokeBattle_SceneConstants.PPTEXTBASECOLORRED, PokeBattle_SceneConstants.PPTEXTSHADOWCOLORRED
			};
			//Instantiate and assign selected last...
			for (int i = 0; i < 4; i++) {
				if (i != index) continue; //if not selected
				if (moves[i].id == Moves.NONE) continue;
				//x=((i%2)==0) ? 4 : 192;	//move it to left or right, if it's odd or even
				//y=((i/2)==0) ? 6 : 48;	//move it up or down, if it's even or odd
				//y+=UPPERGAP;
				//this.bitmap.blt(x, y, @buttonbitmap.bitmap,new Rect(192, moves[i].Type*46,192,46));
				//this.bitmap.blt(416,20+UPPERGAP,@typebitmap.bitmap,new Rect(0, moves[i].Type*28,64,28));
				//textpos.Add(new TextPosition(Game._INTL("{1}", moves[i].Name), x + 96, y + 8, 2,
				//	PokeBattle_SceneConstants.MENUBASECOLOR, PokeBattle_SceneConstants.MENUSHADOWCOLOR));
				if (i == 0)
				{
					//button1.GetComponent<UnityEngine.UI.Image>().sprite = spriteTypes[i];		//Assign Move-Type Sprite
					//button1.GetComponent<UnityEngine.UI.Image>().sprite = spriteButtons[i];	//Assign Move-Type Color to Button BG?
					//button1.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = buttonTextSelectedColor;
					button1.GetComponentInChildren<TMPro.TextMeshProUGUI>().SetText(Game._INTL("{1}", moves[i].Name));
					cursor.position = button1.GetComponent<RectTransform>().position;
				}
				else if (i == 1)
				{
					//button2.GetComponent<UnityEngine.UI.Image>().sprite = spriteTypes[i];		//Assign Move-Type Sprite
					//button2.GetComponent<UnityEngine.UI.Image>().sprite = spriteButtons[i];	//Assign Move-Type Color to Button BG?
					//button2.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = buttonTextSelectedColor;
					button2.GetComponentInChildren<TMPro.TextMeshProUGUI>().SetText(Game._INTL("{1}", moves[i].Name));
					cursor.position = button2.GetComponent<RectTransform>().position;
				}
				else if (i == 2)
				{
					//button3.GetComponent<UnityEngine.UI.Image>().sprite = spriteTypes[i];		//Assign Move-Type Sprite
					//button3.GetComponent<UnityEngine.UI.Image>().sprite = spriteButtons[i];	//Assign Move-Type Color to Button BG?
					//button3.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = buttonTextSelectedColor;
					button3.GetComponentInChildren<TMPro.TextMeshProUGUI>().SetText(Game._INTL("{1}", moves[i].Name));
					cursor.position = button3.GetComponent<RectTransform>().position;
				}
				else if (i == 3)
				{
					//button4.GetComponent<UnityEngine.UI.Image>().sprite = spriteTypes[i];		//Assign Move-Type Sprite
					//button4.GetComponent<UnityEngine.UI.Image>().sprite = spriteButtons[i];	//Assign Move-Type Color to Button BG?
					//button4.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = buttonTextSelectedColor;
					button4.GetComponentInChildren<TMPro.TextMeshProUGUI>().SetText(Game._INTL("{1}", moves[i].Name));
					cursor.position = button4.GetComponent<RectTransform>().position;
				}
				if (moves[i].TotalPP>0) {
					int ppfraction=(int)Math.Ceiling(4.0*moves[i].PP/moves[i].TotalPP);
					//textpos.Add(new TextPosition(Game._INTL("PP: {1}/{2}", moves[i].PP, moves[i].TotalPP),
					//	448, 50 + UPPERGAP, 2, ppcolors[(4 - ppfraction) * 2], ppcolors[(4 - ppfraction) * 2 + 1]));
					selected.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = (UnityEngine.Color)ppcolors[(4 - ppfraction) * 2];
					selected.GetComponentInChildren<TMPro.TextMeshProUGUI>().SetText(Game._INTL("PP: {1}/{2}", moves[i].PP, moves[i].TotalPP));
				}
			}
			//pbDrawTextPositions(this.bitmap, textpos); //foreach text in array, add as child to gameobject with sprite as background image
			if (megaButton > 0) //0=don't show, 1=show, 2=pressed
			{
				//this.bitmap.blt(146, 0, @megaevobitmap.bitmap, new Rect(0, (megaButton - 1) * 46, 96, 46));
				//buttonMega.image.sprite = spriteMegaEvolve[megaButton];
				buttonMega.isOn = megaButton==2;
			}
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
				//disposedValue = true;
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