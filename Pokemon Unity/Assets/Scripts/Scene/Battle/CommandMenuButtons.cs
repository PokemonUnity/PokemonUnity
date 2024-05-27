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

namespace PokemonUnity.UX
{
	/// <summary>
	/// </summary>
	/// <remarks>
	/// This one class is used as parent gameobject for menu buttons, to manipulate children gameobjects (fight, party, bag, run)
	/// </remarks>
	public class CommandMenuButtons : SpriteWrapper, ICommandMenuButtons, IGameObject //BitmapSprite
	{
		//public int Mode;
		private IAnimatedBitmap buttonbitmap;
		private CommandMenuDisplay commandMenuDisplay;
		private UnityEngine.Color buttonTextDefaultColor = UnityEngine.Color.white;
		private UnityEngine.Color buttonTextSelectedColor = new UnityEngine.Color(0.04313726f, 0.07058824f, 0.1843137f); //HEX=>0B122F
		[SerializeField] private UnityEngine.RectTransform cursor;
		[SerializeField] private UnityEngine.Sprite buttonDefault;
		[SerializeField] private UnityEngine.Sprite buttonSelected;
		//ToDo: Create a prefab/class for button entities below...
		[SerializeField] private UnityEngine.GameObject button1;
		[SerializeField] private UnityEngine.GameObject button2;
		[SerializeField] private UnityEngine.GameObject button3;
		[SerializeField] private UnityEngine.GameObject button4;

		private void Awake()
		{
		}

		public ICommandMenuButtons initialize(int index= 0, int mode = 0, IViewport viewport= null)
		{
			//base.initialize(260, 96, viewport);
			//this.x = Graphics.width - 260;
			//this.y = Graphics.height - 96;
			//Mode = mode;
			//@buttonbitmap = new AnimatedBitmap(Game._INTL("Graphics/Pictures/battleCommandButtons"));
			if (viewport is ICommandMenuDisplay)
				commandMenuDisplay = (CommandMenuDisplay)viewport;
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
			//Mode = mode;
			//PokemonUnity.Combat.MenuCommands[] cmdarray = new PokemonUnity.Combat.MenuCommands[] { PokemonUnity.Combat.MenuCommands.FIGHT, PokemonUnity.Combat.MenuCommands.POKEMON, PokemonUnity.Combat.MenuCommands.BAG, PokemonUnity.Combat.MenuCommands.RUN };
			int[] cmdarray = new int[] { 0, 2, 1, 3 };
			switch (mode)
			{
				case 1:
					//cmdarray = new PokemonUnity.Combat.MenuCommands[] { PokemonUnity.Combat.MenuCommands.FIGHT, PokemonUnity.Combat.MenuCommands.BAG, PokemonUnity.Combat.MenuCommands.POKEMON, PokemonUnity.Combat.MenuCommands.CALL };
					cmdarray = new int[] { 0, 2, 1, 4 }; // Use "Call"
					break;
				case 2:
					//cmdarray = new PokemonUnity.Combat.MenuCommands[] { PokemonUnity.Combat.MenuCommands.BAIT, PokemonUnity.Combat.MenuCommands.SAFARI_BALL, PokemonUnity.Combat.MenuCommands.ROCK, PokemonUnity.Combat.MenuCommands.RUN };
					cmdarray = new int[] { 5, 7, 6, 3 }; // Safari Zone battle
					break;
				case 3:
					//cmdarray = new PokemonUnity.Combat.MenuCommands[] { PokemonUnity.Combat.MenuCommands.FIGHT, PokemonUnity.Combat.MenuCommands.BALL, PokemonUnity.Combat.MenuCommands.POKEMON, PokemonUnity.Combat.MenuCommands.RUN };
					cmdarray = new int[] { 0, 8, 1, 3 }; // Bug Catching Contest
					break;
			}
			//ToDo: Can merge the two for-loops into one, as there is no need to instantiate any object into scene by any set order...
			for (int i = 0; i < 4; i++)
			{
				if (i == index) continue; //if selected
				//x = ((i % 2) == 0) ? 0 : 126; //move it to left or right, if it's odd or even
				//y = ((i / 2) == 0) ? 6 : 48;	//move it up or down, if it's even or odd
				//this.bitmap.blt(x, y, @buttonbitmap.bitmap, new Rect(0, cmdarray[i] * 46, 130, 46)); //UI for selected menu item
				
				//Change non-selected sprite image/text/cursor to default
				if (i == 0)
				{
					//button1.GetComponent<UnityEngine.UI.Image>().sprite = commandMenuDisplay.commandSpriteArray[i];
					button1.GetComponent<UnityEngine.UI.Image>().sprite = buttonDefault;
					button1.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = buttonTextDefaultColor;
				}
				else if (i == 1)
				{
					//button2.GetComponent<UnityEngine.UI.Image>().sprite = commandMenuDisplay.commandSpriteArray[i];
					button2.GetComponent<UnityEngine.UI.Image>().sprite = buttonDefault;
					button2.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = buttonTextDefaultColor;
				}
				else if (i == 2)
				{
					//button3.GetComponent<UnityEngine.UI.Image>().sprite = commandMenuDisplay.commandSpriteArray[i];
					button3.GetComponent<UnityEngine.UI.Image>().sprite = buttonDefault;
					button3.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = buttonTextDefaultColor;
				}
				else if (i == 3)
				{
					//button4.GetComponent<UnityEngine.UI.Image>().sprite = commandMenuDisplay.commandSpriteArray[i];
					button4.GetComponent<UnityEngine.UI.Image>().sprite = buttonDefault;
					button4.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = buttonTextDefaultColor;
				}
			}
			for (int i = 0; i < 4; i++)
			{
				if (i != index) continue; //if not selected
				//x = ((i % 2) == 0) ? 0 : 126; //move it to left or right, if it's odd or even
				//y = ((i / 2) == 0) ? 6 : 48;	//move it up or down, if it's even or odd
				//this.bitmap.blt(x, y, @buttonbitmap.bitmap, new Rect(130, cmdarray[i] * 46, 130, 46)); //UI for default menu item
				
				//Change selected sprite image/text/cursor to alternative design
				if (i == 0)
				{
					//button1.GetComponent<UnityEngine.UI.Image>().sprite = commandMenuDisplay.commandSpriteArray[i];
					button1.GetComponent<UnityEngine.UI.Image>().sprite = buttonSelected;
					button1.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = buttonTextSelectedColor;
					cursor.position = button1.GetComponent<RectTransform>().position;
				}
				else if (i == 1) 
				{
					//button2.GetComponent<UnityEngine.UI.Image>().sprite = commandMenuDisplay.commandSpriteArray[i];
					button2.GetComponent<UnityEngine.UI.Image>().sprite = buttonSelected;
					button2.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = buttonTextSelectedColor;
					cursor.position = button2.GetComponent<RectTransform>().position;
				}
				else if (i == 2)
				{
					//button3.GetComponent<UnityEngine.UI.Image>().sprite = commandMenuDisplay.commandSpriteArray[i];
					button3.GetComponent<UnityEngine.UI.Image>().sprite = buttonSelected;
					button3.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = buttonTextSelectedColor;
					cursor.position = button3.GetComponent<RectTransform>().position;
				}
				else if (i == 3)
				{
					//button4.GetComponent<UnityEngine.UI.Image>().sprite = commandMenuDisplay.commandSpriteArray[i];
					button4.GetComponent<UnityEngine.UI.Image>().sprite = buttonSelected;
					button4.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = buttonTextSelectedColor;
					cursor.position = button4.GetComponent<RectTransform>().position;
				}
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

		//void IDisposable.Dispose()
		//{
		//	// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		//	Dispose(disposing: true);
		//	GC.SuppressFinalize(this);
		//}

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