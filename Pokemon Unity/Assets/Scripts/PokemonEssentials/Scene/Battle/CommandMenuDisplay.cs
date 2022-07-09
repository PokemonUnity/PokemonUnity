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

namespace PokemonUnity
{
	/// <summary>
	/// Command menu (Fight/Pokémon/Bag/Run)
	/// </summary>
	public partial class CommandMenuDisplay : ICommandMenuDisplay
	{
		public int mode { get; set; }
		public string title;
		public bool disposedValue;
		public IIconSprite display;
		public IWindow_CommandPokemon window;
		public IWindow_UnformattedTextPokemon msgbox;
		public ICommandMenuButtons buttons;

		public ICommandMenuDisplay initialize(IViewport viewport= null)
		{
			/*
			//@display = null; //set display to false
			if (PokeBattle_SceneConstants.USECOMMANDBOX)
			{
				//set display to true
				//@display = new IconSprite(0, (Game.GameData as Game).Graphics.height - 96, viewport);
				@display.initialize(0, (Game.GameData as Game).Graphics.height - 96, viewport);
				@display.setBitmap("Graphics/Pictures/battleCommand");
			}
			//@window = new Window_CommandPokemon().WithSize([],
			//	(Game.GameData as Game).Graphics.width - 240,(Game.GameData as Game).Graphics.height - 96,240,96,viewport);
			@window.WithSize(new string[0], (Game.GameData as Game).Graphics.width - 240,(Game.GameData as Game).Graphics.height - 96,240,96,viewport);
			@window.columns = 2;
			@window.columnSpacing = 4;
			@window.ignore_input = true;
			//@msgbox = new Window_UnformattedTextPokemon()WithSize(
			//	 "", 16, (Game.GameData as Game).Graphics.height - 96 + 2, 220, 96, viewport);
			@msgbox.WithSize("", 16, (Game.GameData as Game).Graphics.height - 96 + 2, 220, 96, viewport);
			@msgbox.baseColor = PokeBattle_SceneConstants.MESSAGEBASECOLOR;
			@msgbox.shadowColor = PokeBattle_SceneConstants.MESSAGESHADOWCOLOR;
			@msgbox.windowskin = null;
			@title = "";
			//@buttons = null; //set display to false
			if (PokeBattle_SceneConstants.USECOMMANDBOX)
			{
				//set display to true
				@window.opacity = 0;
				@window.x = (Game.GameData as Game).Graphics.width;
				//@buttons = new CommandMenuButtons(this.index, this.mode, viewport);
				@buttons.initialize(this.index, this.mode, viewport);
			}*/
			return this;
		}

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
				@window.visible = value;
				@msgbox.visible = value;
				if (@display != null) @display.visible = value;
				if (@buttons != null) @buttons.visible = value;
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

		public bool disposed 
		{ 
			get
			{
				return @msgbox.disposed || @window.disposed;
			} 
		}

		public void dispose()
		{
			if (disposed) return;
			@msgbox.dispose();
			@window.dispose();
			if (@display != null) @display.dispose();
			if (@buttons != null) @buttons.dispose();
		}

		public int index
		{
			get { return @window.index; }
			set { @window.index = value; }
		}

		public void setTexts(params string[] value)
		{
			@msgbox.text = value[0];
			IList<string> commands = new List<string>();
			for (int i = 1; i < 4; i++)
			{
				//if (value[i] && value[i] != null) 
				if (value.Length <= i && value[i] != null) 
					commands.Add(value[i]);
			}
			@window.commands = commands.ToArray();
		}

		public void refresh()
		{
			@msgbox.refresh();
			@window.refresh();
			if (@buttons != null) @buttons.refresh(this.index, this.mode);
		}

		public void update()
		{
			@msgbox.update();
			@window.update();
			if (@display != null) @display.update();
			if (@buttons != null) @buttons.update(this.index, this.mode);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects)
					dispose();
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
	}
}