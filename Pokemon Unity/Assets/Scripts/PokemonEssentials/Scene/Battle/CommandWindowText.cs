using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
	public class CommandWindowText : WindowTextAdvance, IWindow_CommandPokemon
	{

		#region Unity's MonoBehavior Inspector Properties
		protected int Index;
		protected IList<string> cmds;
		#endregion

		#region Interface Properties Implementation
		public string[] commands { get { return cmds.ToArray(); } set { cmds = value; } }
		public int index { get { return Index; } set { Index = value; } }
		public bool doubleclick { get; }
		public int count { get; }
		public int rowHeight { get; set; }
		public int columns { get; set; }
		public int columnSpacing { get; set; }
		public int row_max { get; set; }
		public int top_row { get; set; }
		public int top_item { get; set; }
		public int page_row_max { get; set; }
		public int page_item_max { get; set; }
		public bool ignore_input { set => throw new NotImplementedException(); }
		int IWindow_DrawableCommand.baseColor { get; set; }
		int IWindow_DrawableCommand.shadowColor { get; set; }
		int ISpriteWindow_Selectable.itemCount { get; }
		#endregion

		#region Unity's MonoBehavior Method
		private void Awake()
		{
		}

		ISpriteWindow_Selectable ISpriteWindow_Selectable.initialize(float x, float y, float width, float height)
		{
			throw new NotImplementedException();
		}

		void Update()
		{
		}
		#endregion

		#region Interface Method Implementation
		public void adjustForZoom(ISprite sprite)
		{
		}

		public IRect drawCursor(int index, IRect rect)
		{
			throw new NotImplementedException();
		}

		public void drawItem(int index, int count, IRect rect)
		{
			throw new NotImplementedException();
		}

		public IWindow_CommandPokemon Empty(float x, float y, float width, float height, IViewport viewport = null)
		{
			throw new NotImplementedException();
		}

		public void getAutoDims(string[] commands, int[] dims, float? width = null)
		{
		}

		public IWindow_CommandPokemon initialize(string[] commands, float? width = null)
		{
			throw new NotImplementedException();
		}

		public void initialize(float x, float y, float width, float height, IViewport viewport = null)
		{
		}

		public void initUpDownArrow()
		{
		}

		public int itemCount()
		{
			throw new NotImplementedException();
		}

		public IRect itemRect(int item)
		{
			throw new NotImplementedException();
		}

		public void resizeToFit(string[] commands, float? width = null)
		{
		}

		public float textWidth(IBitmap bitmap, string text)
		{
			throw new NotImplementedException();
		}

		public void update_cursor_rect()
		{
		}

		public IWindow_CommandPokemon WithSize(string[] commands, float x, float y, float width, float height, IViewport viewport = null)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}