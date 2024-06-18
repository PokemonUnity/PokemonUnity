using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
	/// Command menu (Fight/Pokémon/Bag/Run)
	/// </summary>
	[RequireComponent(typeof(RectTransform))]
	public partial class CommandMenuDisplay : MonoBehaviour, ICommandMenuDisplay, IViewport, IGameObject
	{
		protected global::UnityEngine.RectTransform rect;
		[SerializeField] protected CommandMenuButtons buttons;
		[SerializeField] protected CommandWindowText Window;
		[SerializeField] protected WindowText messageBox;
		protected bool disposedValue;
		protected IRect _rect;
		public int Index;
		public int Mode;
		public IIconSprite display;
		/// <summary>
		/// Collection of sprites, used to contain the background/text for unity button image
		/// that represents the command issued to pokemon during player's turn
		/// </summary>
		/// Should represent master collection of sprites, is assigned to UI using functions
		public global::UnityEngine.Sprite[] commandSpriteArray;
		//ToDo: a prefab of child button for the parent panel to instantiate custom/dynamic commands in game scene
		public IWindow_CommandPokemon window { get { return Window; } set { Window = value as CommandWindowText; } }
		public IWindow_UnformattedTextPokemon msgbox { get { return messageBox; } set { messageBox = value as WindowText; } }

		#region Property
		public int mode { get { return Mode; } set { Mode = value; } }

		public float x
		{
			get { return @window.x; }
			set
			{
				@window.x = value;
				@msgbox.x = value;
				if (@display != null) @display.x = value;
				if (@buttons != null) @buttons.x = value;
			}
		}

		public float y
		{
			get { return @window.y; }
			set
			{
				@window.y = value;
				@msgbox.y = value;
				if (@display != null) @display.y = value;
				if (@buttons != null) @buttons.y = value;
			}
		}

		public float z
		{
			get { return @window.z; }
			set
			{
				@window.z = value;
				@msgbox.z = value;
				if (@display != null) @display.z = value;
				if (@buttons != null) @buttons.z = value + 1;
			}
		}

		public float ox
		{
			get { return @window.ox; }
			set
			{
				@window.ox = value;
				@msgbox.ox = value;
				if (@display != null) @display.ox = value;
				if (@buttons != null) @buttons.ox = value;
			}
		}

		public float oy
		{
			get { return @window.oy; }
			set
			{
				@window.oy = value;
				@msgbox.oy = value;
				if (@display != null) @display.oy = value;
				if (@buttons != null) @buttons.oy = value;
			}
		}

		public bool visible
		{
			get { return @window.visible; }
			set
			{
				//@window.visible = value;
				//@msgbox.visible = value;
				//if (@display != null) @display.visible = value;
				if (@buttons != null) @buttons.visible = value;
				gameObject.SetActive(value); //set this unity go IsActive status to value
			}
		}

		public IColor color
		{
			get { return @window.color; }
			set
			{
				@window.color = value;
				@msgbox.color = value;
				if (@display != null) @display.color = value;
				if (@buttons != null) @buttons.color = value;
			}
		}

		public int index
		{
			//get { return @window.index; }
			//set { @window.index = value; }
			get { return Index; }
			set { Index = value; }
		}

		public bool disposed
		{
			get
			{
				return @msgbox.disposed || @window.disposed;
			}
		}

		IRect IViewport.rect
		{
			get { return _rect; }
			set
			{
				//rect = ((object)value as global::UnityEngine.GameObject).GetComponent<global::UnityEngine.RectTransform>();
				rect.rect.Set(value.x, value.y, value.width, value.height);
				_rect = value;
			}
		}
		#endregion
		private void Awake()
		{
			rect = GetComponent<RectTransform>();
		}

		public ICommandMenuDisplay initialize(IViewport viewport = null)
		{
			//@display = null; //set display to false
			if (PokeBattle_SceneConstants.USECOMMANDBOX)
			{
				//set display to true
				//@display = new IconSprite(0, (Game.GameData as Game).Graphics.height - 96, viewport);
				if (@window != null) {
					@display.initialize(0, (Game.GameData as Game).Graphics.height - 96, viewport);
					@display.setBitmap("Graphics/Pictures/battleCommand");
				}
			}
			//@window = new Window_CommandPokemon().WithSize([],
			//	(Game.GameData as Game).Graphics.width - 240,(Game.GameData as Game).Graphics.height - 96,240,96,viewport);
			if (@window != null) {
				@window.WithSize(new string[0], (Game.GameData as Game).Graphics.width - 240, (Game.GameData as Game).Graphics.height - 96, 240, 96, viewport);
				@window.columns = 2;
				@window.columnSpacing = 4;
				@window.ignore_input = true;
			}
			//@msgbox = new Window_UnformattedTextPokemon()WithSize(
			//	 "", 16, (Game.GameData as Game).Graphics.height - 96 + 2, 220, 96, viewport);
			if (@msgbox != null) {
				@msgbox.WithSize("", 16, (Game.GameData as Game).Graphics.height - 96 + 2, 220, 96, viewport);
				@msgbox.baseColor = PokeBattle_SceneConstants.MESSAGEBASECOLOR;
				@msgbox.shadowColor = PokeBattle_SceneConstants.MESSAGESHADOWCOLOR;
				@msgbox.windowskin = false; //Resources.Load<global::UnityEngine.Sprite>("null");
			}
			//@title = ""; //ToDo: no clue what this is for...
			//@buttons = null; //set display to false
			buttons.gameObject.SetActive(false);
			if (PokeBattle_SceneConstants.USECOMMANDBOX)
			{
				//set display to true
				@window.opacity = 0;
				@window.x = (Game.GameData as Game).Graphics.width;
				//@buttons = new CommandMenuButtons(this.index, this.mode, viewport);
				buttons.gameObject.SetActive(true);
				@buttons.initialize(this.index, this.mode, this);
			}
			return this;
		}

		public void setTexts(params string[] value)
		{
			if (value == null || value.Length == 0) return;
			if (@msgbox != null) @msgbox.text = value[0];
			//if using sprites/images as menu option, may need to change logic below
			IList<string> commands = new List<string>();
			for (int i = 1; i < 4; i++)
			{
				//if (value[i] && value[i] != null)
				if (value.Length <= i && value[i] != null)
					commands.Add(value[i]);
			}
			if (@window != null) @window.commands = commands.ToArray();
		}

		public void refresh()
		{
			@msgbox?.refresh();
			@window?.refresh();
			if (@buttons != null) @buttons.refresh(this.index, this.mode);
		}

		public void update()
		{
			@msgbox?.update();
			@window?.update();
			if (@display != null) @display.update();
			if (@buttons != null) @buttons.update(this.index, this.mode);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue || !disposed)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects)
					//if (disposed) return;
					@msgbox.Dispose();
					@window.Dispose();
					if (@display != null) @display.Dispose();
					if (@buttons != null) (@buttons as IDisposable).Dispose();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override finalizer
				// TODO: set large fields to null
				disposedValue = true;
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

		IViewport IViewport.initialize(float x, float y, float height, float width)
		{
			//throw new NotImplementedException();
			return this;
		}

		void IViewport.flash(IColor color, int duration)
		{
			//throw new NotImplementedException();
		}
	}
}