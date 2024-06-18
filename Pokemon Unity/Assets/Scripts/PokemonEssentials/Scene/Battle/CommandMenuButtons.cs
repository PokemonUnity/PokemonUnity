using System;
using System.Collections;
using System.Collections.Generic;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
//using PokemonEssentials.Interface.PokeBattle.Rules;
using UnityEngine;
using UnityEngine.UI;

namespace PokemonUnity.Interface.UnityEngine
{
	/// <summary>
	/// UI panel for displaying buttons under menu options: "FIGHT", "PARTY", "BAG", "RUN".
	/// Nested as a child of <seealso cref="CommandMenuDisplay"/>
	/// </summary>
	/// <remarks>
	/// This one class is used as parent gameobject for menu buttons, to manipulate children gameobjects (FIGHT, PARTY, BAG, RUN)
	/// </remarks>
	public class CommandMenuButtons : SpriteWrapper, ICommandMenuButtons, IGameObject //BitmapSprite
	{
		//public int Mode;
		private int[] cmdarray;
		private IAnimatedBitmap buttonbitmap;
		private CommandMenuDisplay commandMenuDisplay;
		private global::UnityEngine.Color buttonTextDefaultColor = global::UnityEngine.Color.white;
		private global::UnityEngine.Color buttonTextSelectedColor = new global::UnityEngine.Color(0.04313726f, 0.07058824f, 0.1843137f); //HEX=>0B122F
		[SerializeField] private global::UnityEngine.RectTransform cursor;
		[SerializeField] private global::UnityEngine.Sprite buttonDefault;
		[SerializeField] private global::UnityEngine.Sprite buttonSelected;
		//ToDo: Create a prefab/class for button entities below...
		[SerializeField] private CommandMenuButton button1;
		[SerializeField] private CommandMenuButton button2;
		[SerializeField] private CommandMenuButton button3;
		[SerializeField] private CommandMenuButton button4;
		/// <summary>
		/// Sprite images for each menu option
		/// </summary>
		/// <remarks>Match logo image and order to <see cref="PokemonUnity.Combat.MenuCommands"/></remarks>
		/// Should contain logo array to assign to each button
		[SerializeField] private global::UnityEngine.Sprite[] Logos;

		private void Awake()
		{
		}

		/// <summary>
		/// </summary>
		/// <param name="index"></param>
		/// <param name="mode">
		/// Whether regular battle commands, shadow pokemon commands, safari battle commands, or bug catching contest.
		/// <see cref="PokemonUnity.Combat.MenuCommands"/>
		/// </param>
		/// <param name="viewport"><see cref="ICommandMenuDisplay"/></param>
		/// <returns></returns>
		public ICommandMenuButtons initialize(int index= 0, int mode = 0, IViewport viewport=null)
		{
			base.initialize(viewport); //base.initialize(260, 96, viewport);
			//this.x = Graphics.width - 260;
			//this.y = Graphics.height - 96;
			//Mode = mode;
			//@buttonbitmap = new AnimatedBitmap(Game._INTL("Graphics/Pictures/battleCommandButtons"));
			if (viewport is ICommandMenuDisplay)
				commandMenuDisplay = (CommandMenuDisplay)viewport;
			refresh(index, mode);
			//ToDo: Test if this is needed, or if it's already being done in the inspector
			if (cmdarray != null && cmdarray.Length > 3)
			{
				button1.Text.text = ((PokemonUnity.Combat.MenuCommands)cmdarray[0]).ToString();
				button2.Text.text = ((PokemonUnity.Combat.MenuCommands)cmdarray[1]).ToString();
				button3.Text.text = ((PokemonUnity.Combat.MenuCommands)cmdarray[2]).ToString();
				button4.Text.text = ((PokemonUnity.Combat.MenuCommands)cmdarray[3]).ToString();
				if (commandMenuDisplay.commandSpriteArray != null && commandMenuDisplay.commandSpriteArray.Length > 3)
				{
					button1.Logo.sprite = commandMenuDisplay.commandSpriteArray[cmdarray[0]]; //Logos[cmdarray[0]];
					button2.Logo.sprite = commandMenuDisplay.commandSpriteArray[cmdarray[1]]; //Logos[cmdarray[1]];
					button3.Logo.sprite = commandMenuDisplay.commandSpriteArray[cmdarray[2]]; //Logos[cmdarray[2]];
					button4.Logo.sprite = commandMenuDisplay.commandSpriteArray[cmdarray[3]]; //Logos[cmdarray[3]];
				}
			}
			return this;
		}

		public void update(int index = 0, int mode = 0)
		{
			refresh(index, mode);
		}

		/// <summary>
		/// </summary>
		/// <param name="index">cursor selection</param>
		/// <param name="mode">
		/// Whether regular battle commands, shadow pokemon commands, safari battle commands, or bug catching contest.
		/// <see cref="PokemonUnity.Combat.MenuCommands"/>
		/// </param>
		public void refresh(int index, int mode = 0)
		{
			//this.bitmap.clear();
			//Mode = mode;
			//PokemonUnity.Combat.MenuCommands[] cmdarray = new PokemonUnity.Combat.MenuCommands[] { PokemonUnity.Combat.MenuCommands.FIGHT, PokemonUnity.Combat.MenuCommands.POKEMON, PokemonUnity.Combat.MenuCommands.BAG, PokemonUnity.Combat.MenuCommands.RUN };
			cmdarray = new int[] { 0, 2, 1, 3 };
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
					//button1.GetComponent<global::UnityEngine.UI.Image>().sprite = buttonDefault;
					//button1.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = buttonTextDefaultColor;
					button1.Background.sprite = buttonDefault;
					button1.Text.color = buttonTextDefaultColor;
				}
				else if (i == 1)
				{
					//button2.GetComponent<UnityEngine.UI.Image>().sprite = commandMenuDisplay.commandSpriteArray[i];
					//button2.GetComponent<global::UnityEngine.UI.Image>().sprite = buttonDefault;
					//button2.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = buttonTextDefaultColor;
					button2.Background.sprite = buttonDefault;
					button2.Text.color = buttonTextDefaultColor;
				}
				else if (i == 2)
				{
					//button3.GetComponent<UnityEngine.UI.Image>().sprite = commandMenuDisplay.commandSpriteArray[i];
					//button3.GetComponent<global::UnityEngine.UI.Image>().sprite = buttonDefault;
					//button3.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = buttonTextDefaultColor;
					button3.Background.sprite = buttonDefault;
					button3.Text.color = buttonTextDefaultColor;
				}
				else if (i == 3)
				{
					//button4.GetComponent<UnityEngine.UI.Image>().sprite = commandMenuDisplay.commandSpriteArray[i];
					//button4.GetComponent<global::UnityEngine.UI.Image>().sprite = buttonDefault;
					//button4.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = buttonTextDefaultColor;
					button4.Background.sprite = buttonDefault;
					button4.Text.color = buttonTextDefaultColor;
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
					//button1.GetComponent<global::UnityEngine.UI.Image>().sprite = commandMenuDisplay.commandSpriteArray[i];
					//button1.GetComponent<global::UnityEngine.UI.Image>().sprite = buttonSelected;
					//button1.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = buttonTextSelectedColor;
					//cursor.position = button1.GetComponent<RectTransform>().position;
					button1.Background.sprite = buttonSelected;
					button1.Text.color = buttonTextSelectedColor;
					cursor.position = button1.Rect.position;
				}
				else if (i == 1)
				{
					//button2.GetComponent<UnityEngine.UI.Image>().sprite = commandMenuDisplay.commandSpriteArray[i];
					//button2.GetComponent<global::UnityEngine.UI.Image>().sprite = buttonSelected;
					//button2.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = buttonTextSelectedColor;
					//cursor.position = button2.GetComponent<RectTransform>().position;
					button2.Background.sprite = buttonSelected;
					button2.Text.color = buttonTextSelectedColor;
					cursor.position = button2.Rect.position;
				}
				else if (i == 2)
				{
					//button3.GetComponent<UnityEngine.UI.Image>().sprite = commandMenuDisplay.commandSpriteArray[i];
					//button3.GetComponent<global::UnityEngine.UI.Image>().sprite = buttonSelected;
					//button3.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = buttonTextSelectedColor;
					//cursor.position = button3.GetComponent<RectTransform>().position;
					button3.Background.sprite = buttonSelected;
					button3.Text.color = buttonTextSelectedColor;
					cursor.position = button3.Rect.position;
				}
				else if (i == 3)
				{
					//button4.GetComponent<UnityEngine.UI.Image>().sprite = commandMenuDisplay.commandSpriteArray[i];
					//button4.GetComponent<global::UnityEngine.UI.Image>().sprite = buttonSelected;
					//button4.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = buttonTextSelectedColor;
					//cursor.position = button4.GetComponent<RectTransform>().position;
					button4.Background.sprite = buttonSelected;
					button4.Text.color = buttonTextSelectedColor;
					cursor.position = button4.Rect.position;
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