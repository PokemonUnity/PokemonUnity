using System;
using System.Linq;
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

namespace PokemonUnity
{
	/// <summary>
	/// Fight menu (choose a move)
	/// </summary>
	//[RequireComponent(typeof())]
	public partial class FightMenuDisplay : MonoBehaviour, IFightMenuDisplay
	{
		private IWindow_CommandPokemon window;
		private IWindow_AdvancedTextPokemon info;
		private IIconSprite display;
		private IFightMenuButtons buttons;
		private IBattler _battler;
		private bool disposedValue;
		private string ctag;

		public IBattler battler
		{
			get { return _battler; }
			set
			{
				_battler = value;
				refresh();
			}
		}
		public int index { get; protected set; }
		/// <summary>
		/// 0=don't show, 1=show, 2=pressed
		/// </summary>
		public int megaButton { get; set; }

		public float x
		{
			get { return @window.x; }
			set
			{
				@window.x = value;
				@info.x = value;
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
				@info.y = value;
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
				@info.z = value;
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
				@info.ox = value;
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
				@info.oy = value;
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
				@info.visible = value;
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
				@info.color = value;
				if (@display != null) @display.color = value;
				if (@buttons != null) @buttons.color = value;
			}
		}

		void Awake()
		{
			buttons = transform.GetComponent<FightMenuButtons>();
		}

		public IFightMenuDisplay initialize(IBattler battler, IViewport viewport= null)
		{
			@display = null;
			/*if (PokeBattle_SceneConstants.USEFIGHTBOX)
			{
				//@display = new IconSprite().initialize(0, (Game.GameData as Game).Graphics.height - 96, viewport);
				@display.initialize(0, (Game.GameData as Game).Graphics.height - 96, viewport);
				@display.setBitmap("Graphics/Pictures/battleFight");
			}
			//@window = new Window_CommandPokemon().WithSize([],0,(Game.GameData as Game).Graphics.height - 96,320,96,viewport);
			@window.WithSize(new string[0],0,(Game.GameData as Game).Graphics.height - 96,320,96,viewport);
			@window.columns = 2;
			@window.columnSpacing = 4;
			@window.ignore_input = true;
			pbSetNarrowFont(@window.contents);
			//@info = new Window_AdvancedTextPokemon().WithSize(
			//	 "", 320, (Game.GameData as Game).Graphics.height - 96, (Game.GameData as Game).Graphics.width - 320, 96, viewport);
			@info.WithSize("", 320, (Game.GameData as Game).Graphics.height - 96, (Game.GameData as Game).Graphics.width - 320, 96, viewport);
			pbSetNarrowFont(@info.contents);
			@ctag = shadowctag(PokeBattle_SceneConstants.MENUBASECOLOR,
							 PokeBattle_SceneConstants.MENUSHADOWCOLOR);
			@buttons = null;
			_battler = battler;
			@index = 0;
			@megaButton = 0; // 0=don't show, 1=show, 2=pressed
			if (PokeBattle_SceneConstants.USEFIGHTBOX)
			{
				@window.opacity = 0;
				@window.x = (Game.GameData as Game).Graphics.width;
				@info.opacity = 0;
				@info.x = (Game.GameData as Game).Graphics.width + (Game.GameData as Game).Graphics.width - 96;
				//@buttons = new FightMenuButtons().initialize(this.index, null, viewport);
				@buttons.initialize(this.index, null, viewport);
			}*/
			refresh();
			return this;
		}

		public bool disposed
		{
			get
			{
				return @info.disposed || @window.disposed;
			}
		}

		public void dispose()
		{
			if (disposed) return;
			@info.dispose();
			if (@display != null) @display.dispose();
			if (@buttons != null) @buttons.Dispose();
			@window.dispose();
		}

		public bool setIndex(int value)
		{
			if (@battler != null && @battler.moves[value].id != 0)
			{
				@index = value;
				@window.index = value;
				refresh();
				return true;
			}
			return false;
		}

		public void refresh()
		{
			if (battler == null) return;
			IList<string> commands = new List<string>();
			for (int i = 0; i < 4; i++)
			{
				if (@battler.moves[i].id == 0) break;
				commands.Add(@battler.moves[i].Name);
			}
			@window.commands = commands.ToArray();
			IBattleMove selmove = @battler.moves[@index];
			string movetype = selmove.Type.ToString(TextScripts.Name);
			if (selmove.TotalPP == 0)
			{
				@info.text = string.Format("{0}PP: ---<br>TYPE/{1}", @ctag, movetype);
			}
			else
			{
				@info.text = string.Format("{0}PP: {1}/{2}<br>TYPE/{3}",
				   @ctag, selmove.PP, selmove.TotalPP, movetype);
			}
			if (@buttons != null) @buttons.refresh(this.index, @battler != null ? @battler.moves : null, @megaButton);
		}

		public void update()
		{
			@info.update();
			@window.update();
			if (@display != null) @display.update();
			if (@buttons != null)
			{
				IBattleMove[] moves = @battler != null ? @battler.moves : null;
				@buttons.update(this.index, moves, @megaButton);
			}
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