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
	[RequireComponent(typeof(Image),typeof(TMPro.TextMeshProUGUI))]
	public class CommandMenuButtons : SpriteWrapper, ICommandMenuButtons //BitmapSprite
	{
		public int Index;
		public int Mode;
		public IAnimatedBitmap buttonbitmap;
		private CommandMenuDisplay commandMenuDisplay;
		private Image button;
		private TMPro.TextMeshProUGUI text;
		private UnityEngine.Sprite buttonDefault;
		private UnityEngine.Sprite buttonSelected;
		private UnityEngine.Color buttonTextDefaultColor;
		private UnityEngine.Color buttonTextSelectedColor;

		private void Awake()
		{
			button = GetComponent<Image>();
			text = GetComponent<TMPro.TextMeshProUGUI>();
		}

		public ICommandMenuButtons initialize(int index= 0, int mode = 0, IViewport viewport= null)
		{
			//base.initialize(260, 96, viewport);
			//this.x = Graphics.width - 260;
			//this.y = Graphics.height - 96;
			Mode = mode;
			//@buttonbitmap = new AnimatedBitmap(Game._INTL("Graphics/Pictures/battleCommandButtons"));
			refresh(index, mode);
			return this;
		}

		public void update(int index = 0, int mode = 0)
		{
			refresh(index, mode);
		}

		public void refresh(int index, int mode = 0)
		{
			//this.bitmap.clear();
			Mode = mode;
			int[] cmdarray = new int[] { 0, 2, 1, 3 };
			switch (Mode)
			{
				case 1:
					cmdarray = new int[] { 0, 2, 1, 4 }; // Use "Call"
					break;
				case 2:
					cmdarray = new int[] { 5, 7, 6, 3 }; // Safari Zone battle
					break;
				case 3:
					cmdarray = new int[] { 0, 8, 1, 3 }; // Bug Catching Contest
					break;
			}
			for (int i = 0; i < 4; i++)
			{
				if (i == index) continue; //if selected
				//x = ((i % 2) == 0) ? 0 : 126;
				//y = ((i / 2) == 0) ? 6 : 48;
				//this.bitmap.blt(x, y, @buttonbitmap.bitmap, new Rect(0, cmdarray[i] * 46, 130, 46));
				
				//Change sprite image/text/cursor to default
                button.sprite = buttonSelected;
                text.color = buttonTextSelectedColor;

                //cursor.position = button.GetComponent<RectTransform>().position;
			}
			for (int i = 0; i < 4; i++)
			{
				if (i != index) continue; //if not selected
				//x = ((i % 2) == 0) ? 0 : 126;
				//y = ((i / 2) == 0) ? 6 : 48;
				//this.bitmap.blt(x, y, @buttonbitmap.bitmap, new Rect(130, cmdarray[i] * 46, 130, 46));
				
				//Change sprite image/text/cursor to alternative design
                button.sprite = buttonDefault;
                text.color = buttonTextDefaultColor;
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
					base.Dispose(disposing);
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

		void IDisposable.Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		#region Implementing Interface
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