using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Globalization;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Inventory;
using PokemonUnity.Saving;
using System.IO;
using PokemonUnity.Overworld;

namespace PokemonEssentials.Interface
{
	public interface ITextPosition
	{
		/// <summary>
		/// Text to draw
		/// </summary>
		string Text { get; set; }
		/// <summary>
		/// X coordinate
		/// </summary>
		float X { get; set; }
		/// <summary>
		/// Y coordinate
		/// </summary>
		float Y { get; set; }
		/// <summary>
		/// If true or 1, the text is right aligned. If 2, the text is centered.
		/// Otherwise, the text is left aligned.
		/// </summary>
		bool? LeftAligned { get; set; }
		/// <summary>
		/// Base color
		/// </summary>
		IColor Base { get; set; }
		/// <summary>
		/// Shadow color
		/// </summary>
		IColor Shadow { get; set; }
	}

	public struct TextPosition : ITextPosition
	{
		/// <summary>
		/// Text to draw
		/// </summary>
		public string Text { get; set; }
		/// <summary>
		/// X coordinate
		/// </summary>
		public float X { get; set; }
		/// <summary>
		/// Y coordinate
		/// </summary>
		public float Y { get; set; }
		/// <summary>
		/// If true or 1, the text is right aligned. If 2, the text is centered.
		/// Otherwise, the text is left aligned.
		/// </summary>
		public bool? LeftAligned { get; set; }
		/// <summary>
		/// Base color
		/// </summary>
		public PokemonEssentials.Interface.IColor Base { get; set; }
		/// <summary>
		/// Shadow color
		/// </summary>
		public PokemonEssentials.Interface.IColor Shadow { get; set; }

		public TextPosition(string text, float x, float y, bool? leftAligned, IColor base_, IColor shadow)
		{
			Text = text;
			X = x;
			Y = y;
			LeftAligned = leftAligned;
			Base = base_;
			Shadow = shadow;
		}

		public TextPosition(string name, float x, float y, float srcx, float srcy, float width, float height)
		{
			Text = name;
			X = x;
			Y = y;
			LeftAligned = null;
			Base = null;
			Shadow = null;
		}
	}
}